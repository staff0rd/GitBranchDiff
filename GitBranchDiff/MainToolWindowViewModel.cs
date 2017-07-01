using GitBranchDiff.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GitBranchDiff
{
    class MainToolWindowViewModel
    {
        public MainToolWindowViewModel(MainToolWindow window)
        {
            Branch = "dev";
            MainToolWindow = window;
            Items = new ObservableCollection<Item>();
        }

        public ObservableCollection<Item> Items { get; set; }

        public string Branch { get; set; }

        public ObservableCollection<string> Branches { get; set; }

        private MainToolWindow MainToolWindow { get; set; }

        public string GitRoot { get; set; }

        public void Reload()
        {
            var solutionPath = VisualStudioService.dte2.Solution.FullName;
            if (!string.IsNullOrEmpty(solutionPath))
            {
                GitRoot = new ShellCommand().Execute("git", "rev-parse --show-toplevel", new FileInfo(solutionPath).DirectoryName)[0];
                Items.Clear();
                var items = new ItemProvider().GetItems(GitRoot, Branch);
                foreach (var item in items)
                {
                    Items.Add(item);
                }
            }
        }

        internal void OpenFile(string path)
        {
            if (!path.Contains(GitRoot))
                path = Path.Combine(GitRoot, path);
            VisualStudioService.dte2.ItemOperations.OpenFile(path);
        }
    }
}
