using GitBranchDiff.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace GitBranchDiff
{
    class MainToolWindowViewModel : INotifyPropertyChanged
    {
        public MainToolWindowViewModel(MainToolWindow window)
        {
            MainToolWindow = window;
            Items = new ObservableCollection<Item>();
            Branches = new ObservableCollection<Branch>();
        }

        public ObservableCollection<Item> Items { get; set; }

        private Branch _selectedBranch;

        public Branch SelectedBranch
        {
            get { return _selectedBranch; }
            set { _selectedBranch = value;
                NotifyPropertyChanged("SelectedBranch");
            }
        }


        public ObservableCollection<Branch> Branches { get; set; }

        private MainToolWindow MainToolWindow { get; set; }

        public string GitRoot { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void NotifyPropertyChanged(String info)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(info));
        }

        public void Reload(bool reloadBranches = true)
        {
            var solutionPath = VisualStudioService.dte2.Solution.FullName;
            if (!string.IsNullOrEmpty(solutionPath))
            {
                GitRoot = new ShellCommand().Execute("git", "rev-parse --show-toplevel", new FileInfo(solutionPath).DirectoryName)[0];
                if (reloadBranches)
                    ReloadBranches();
                ReloadChanges();
            }
        }

        public class Branch
        {
            public string Name { get; set; }
        }

        private void ReloadBranches()
        {
            var selectedBranch = SelectedBranch?.Name;
            Branches.Clear();
            var branches = new ShellCommand().Execute("git", "branch", GitRoot).Select(p => p.Trim());
            foreach (var branch in branches)
            {
                Branches.Add(new Branch { Name = branch });
            }
            if (selectedBranch != null)
                SelectedBranch = Branches.SingleOrDefault(p => p.Name == selectedBranch);
            else
                SelectedBranch = Branches.FirstOrDefault();
        }

        private void ReloadChanges()
        {
            if (SelectedBranch != null)
            {
                Items.Clear();
                var items = new ItemProvider().GetItems(GitRoot, SelectedBranch.Name);
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
            if (File.Exists(path))
                VisualStudioService.dte2.ItemOperations.OpenFile(path);
            else
                MessageBox.Show($"This file does not exist on disk.\n{path}");
        }
    }
}
