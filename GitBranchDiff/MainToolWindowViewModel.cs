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
                GitRoot = new ShellCommand().ExecuteGit("rev-parse --show-toplevel", new FileInfo(solutionPath).DirectoryName)[0];
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
            var branches = new ShellCommand().ExecuteGit("branch", GitRoot).Select(p => p.Trim());
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

        public static Encoding GetEncoding(string filename)
        {
            // Read the BOM
            var bom = new byte[4];
            using (var file = new FileStream(filename, FileMode.Open, FileAccess.Read))
            {
                file.Read(bom, 0, 4);
            }

            // Analyze the BOM
            if (bom[0] == 0x2b && bom[1] == 0x2f && bom[2] == 0x76) return Encoding.UTF7;
            if (bom[0] == 0xef && bom[1] == 0xbb && bom[2] == 0xbf) return Encoding.UTF8;
            if (bom[0] == 0xff && bom[1] == 0xfe) return Encoding.Unicode; //UTF-16LE
            if (bom[0] == 0xfe && bom[1] == 0xff) return Encoding.BigEndianUnicode; //UTF-16BE
            if (bom[0] == 0 && bom[1] == 0 && bom[2] == 0xfe && bom[3] == 0xff) return Encoding.UTF32;
            return Encoding.ASCII;
        }

        internal void OpenFile(FileItem file)
        {
            var path = file.Path;
            var fullPath = path;
            if (!fullPath.Contains(GitRoot))
                fullPath = Path.Combine(GitRoot, fullPath);

            if (file.IsModified)
            {
                var tempFile = Path.GetTempFileName();
                var oldFile = new ShellCommand().ExecuteGit($"show {SelectedBranch.Name}:\"{path}\"", GitRoot);
                File.WriteAllLines(tempFile, oldFile, GetEncoding(fullPath));
                
                VisualStudioService.dte2.ExecuteCommand("Tools.DiffFiles", $"\"{tempFile}\" \"{fullPath}\"");
            }
            else
            {
                if (File.Exists(fullPath))
                    VisualStudioService.dte2.ItemOperations.OpenFile(fullPath);
                else
                    MessageBox.Show($"This file does not exist on disk.\n{fullPath}");
            }
        }
    }
}
