//------------------------------------------------------------------------------
// <copyright file="MainToolWindow.cs" company="Company">
//     Copyright (c) Company.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

namespace GitBranchDiff
{
    using System;
    using System.Runtime.InteropServices;
    using Microsoft.VisualStudio.Shell;

    /// <summary>
    /// This class implements the tool window exposed by this package and hosts a user control.
    /// </summary>
    /// <remarks>
    /// In Visual Studio tool windows are composed of a frame (implemented by the shell) and a pane,
    /// usually implemented by the package implementer.
    /// <para>
    /// This class derives from the ToolWindowPane class provided from the MPF in order to use its
    /// implementation of the IVsUIElementPane interface.
    /// </para>
    /// </remarks>
    [Guid("79120ca0-9215-43a0-b824-1bd92891535d")]
    public class MainToolWindow : ToolWindowPane
    {
        private MainToolWindowControl MainToolWindowControl { get; set; }

        private MainToolWindowViewModel ViewModel { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="MainToolWindow"/> class.
        /// </summary>
        public MainToolWindow() : base(null)
        {
            this.Caption = "MainToolWindow";

            // This is the user control hosted by the tool window; Note that, even if this class implements IDisposable,
            // we are not calling Dispose on this object. This is because ToolWindowPane calls Dispose on
            // the object returned by the Content property.
            MainToolWindowControl = new MainToolWindowControl();
            ViewModel = new MainToolWindowViewModel(this);
            MainToolWindowControl.DataContext = ViewModel;
            this.Content = MainToolWindowControl;
        }

        public void Reload()
        {
            ViewModel.Reload();
        }
    }
}
