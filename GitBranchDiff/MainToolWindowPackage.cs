﻿//------------------------------------------------------------------------------
// <copyright file="MainToolWindowPackage.cs" company="Company">
//     Copyright (c) Company.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Runtime.InteropServices;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.OLE.Interop;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.Win32;
using EnvDTE80;
using EnvDTE;

namespace GitBranchDiff
{
    /// <summary>
    /// This is the class that implements the package exposed by this assembly.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The minimum requirement for a class to be considered a valid package for Visual Studio
    /// is to implement the IVsPackage interface and register itself with the shell.
    /// This package uses the helper classes defined inside the Managed Package Framework (MPF)
    /// to do it: it derives from the Package class that provides the implementation of the
    /// IVsPackage interface and uses the registration attributes defined in the framework to
    /// register itself and its components with the shell. These attributes tell the pkgdef creation
    /// utility what data to put into .pkgdef file.
    /// </para>
    /// <para>
    /// To get loaded into VS, the package must be referred by &lt;Asset Type="Microsoft.VisualStudio.VsPackage" ...&gt; in .vsixmanifest file.
    /// </para>
    /// </remarks>
    [PackageRegistration(UseManagedResourcesOnly = true)]
    [InstalledProductRegistration("#110", "#112", "1.0", IconResourceID = 400)] // Info on this package for Help/About
    [ProvideMenuResource("Menus.ctmenu", 1)]
    [ProvideToolWindow(typeof(MainToolWindow))]
    [Guid(MainToolWindowPackage.PackageGuidString)]
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "pkgdef, VS and vsixmanifest are valid VS terms")]
    [ProvideAutoLoad(VSConstants.UICONTEXT.SolutionExists_string)]
    public sealed class MainToolWindowPackage : Package
    {
        /// <summary>
        /// MainToolWindowPackage GUID string.
        /// </summary>
        public const string PackageGuidString = "c28547c4-5b77-41aa-8959-c87bb66111c5";

        /// <summary>
        /// Initializes a new instance of the <see cref="MainToolWindow"/> class.
        /// </summary>
        public MainToolWindowPackage()
        {
            // Inside this method you can place any initialization code that does not require
            // any Visual Studio service because at this point the package object is created but
            // not sited yet inside Visual Studio environment. The place to do all the other
            // initialization is the Initialize method.
        }

        /// <summary>
        /// Initialization of the package; this method is called right after the package is sited, so this is the place
        /// where you can put all the initialization code that rely on services provided by VisualStudio.
        /// </summary>
        protected override void Initialize()
        {
            MainToolWindowCommand.Initialize(this);
            base.Initialize();

            VisualStudioService.Initialize(this);
            VisualStudioService.SolutionEvents.Opened += SolutionEvents_Opened;
        }

        private void WindowEvents_WindowActivated(Window GotFocus, Window LostFocus)
        {
            var window = MainToolWindowCommand.Instance.GetWindow();
            if (GotFocus.Caption == window.Caption)
                window.Reload();
        }

        private void BuildEvents_OnBuildBegin(vsBuildScope Scope, vsBuildAction Action)
        {
            MainToolWindowCommand.Instance.GetWindow().Reload();
        }

        private void SolutionEvents_Opened()
        {
            MainToolWindowCommand.Instance.GetWindow().Reload();
        }
    }
}
