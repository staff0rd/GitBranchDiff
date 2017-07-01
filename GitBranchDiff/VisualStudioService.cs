using System;
using EnvDTE;
using EnvDTE80;
using System.ComponentModel.Design;
using Microsoft.VisualStudio.Shell.Interop;

namespace GitBranchDiff
{
    public static class VisualStudioService
    {
        public static string GitRoot { get; set; }

        public static DTE2 dte2 { get; set; }

        public static SolutionEvents SolutionEvents { get; set; }

        public static BuildEvents BuildEvents { get; set; }

        private static Events Events { get; set; }

        internal static void Initialize(MainToolWindowPackage package)
        {
            IServiceContainer serviceContainer = package as IServiceContainer;
            dte2 = serviceContainer.GetService(typeof(SDTE)) as DTE2;
            Events = dte2.Events;
            SolutionEvents = Events.SolutionEvents;
            BuildEvents = Events.BuildEvents;
        }
    }
}
