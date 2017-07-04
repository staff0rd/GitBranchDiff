using GitBranchDiff.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EnvDTE80;
using Microsoft.VisualStudio.Shell;
using System.IO;
using System.Diagnostics;

namespace GitBranchDiff
{
    public class ItemProvider
    {
        public List<Item> GetItems(string gitRoot, string branch)
        {
            if (branch.Contains("*"))
                branch = branch.Replace("*", "").Trim();
            var diff = new ShellCommand().ExecuteGit($"diff --name-status {branch}", gitRoot);

            var changes = new ShellCommand().ExecuteGit("status --porcelain -u", gitRoot);

            diff.AddRange(ConvertChanges(changes));

            return MakeTreeFromPaths(diff);
        }

        public List<string> ConvertChanges(List<string> changes)
        {
            var result = new List<string>();
            foreach(var change in changes)
            {
                var path = change.Substring(3);
                switch (change.Substring(0, 2))
                {
                    case "??":
                    case "A ": result.Add($"A\t{path}"); break;
                    case "D ": result.Add($"D\t{path}"); break;
                    case "M ": result.Add($"M\t{path}"); break;
                }
            }
            return result;
        }

        public List<Item> MakeTreeFromPaths(List<string> changes, string rootNodeName = "", char separator = '/')
        {
            var rootNode = new DirectoryItem();
            foreach (var change in changes.Where(x => !string.IsNullOrEmpty(x.Trim())))
            {
                var split = change.Split('\t');
                var action = split[0];
                var path = split[1];

                var currentNode = rootNode;
                var pathItems = path.Split(separator);

                for (int i = 0; i < pathItems.Count(); i++) 
                {
                    var item = pathItems[i];
                    var tmp = currentNode.Items.FirstOrDefault(x => x.Name.Equals(item));
                    if (tmp == null)
                    {
                        if (i == pathItems.Count() - 1)
                        {
                            var newFile = new FileItem { Name = item, Path = path, IsAdded = action == "A", IsDeleted = action == "D" };
                            currentNode.Items.Add(newFile);
                        } else
                        {
                            var newDirectory = new DirectoryItem { Path = PathUntil(pathItems, i), Name = item, Items = new List<Item>() };
                            currentNode.Items.Add(newDirectory);
                            currentNode = newDirectory;
                        }
                    } else if (tmp.GetType() == typeof(DirectoryItem))
                    {
                        currentNode = (DirectoryItem)tmp;
                    }
                }
            }
            Sort(rootNode);
            return rootNode.Items;
        }

        private void Sort(DirectoryItem node)
        {
            node.Items = node.Items.OrderBy(p => p is FileItem).ThenBy(p => p.Name).ToList();
            foreach (var child in node.Items)
            {
                if (child.GetType() == typeof(DirectoryItem))
                {
                    Sort((DirectoryItem)child);
                }
            }
        }

        public string PathUntil(string[] path, int until)
        {
            return string.Join("/", path.Take(until));
        }
    }
}
