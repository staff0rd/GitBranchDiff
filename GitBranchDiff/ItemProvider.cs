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
            var changes = new ShellCommand().Execute("git", $"diff --name-only {branch}", gitRoot);
            return MakeTreeFromPaths(changes);
        }

        public List<Item> MakeTreeFromPaths(List<string> paths, string rootNodeName = "", char separator = '/')
        {
            var rootNode = new DirectoryItem();
            foreach (var path in paths.Where(x => !string.IsNullOrEmpty(x.Trim())))
            {
                var currentNode = rootNode;
                var pathItems = path.Split(separator);

                for (int i = 0; i < pathItems.Count(); i++) 
                {
                    var item = pathItems[i];
                    var tmp = currentNode.Items.Where(x => x.Name.Equals(item));
                    if (tmp.Count() == 0)
                    {
                        if (i == pathItems.Count() - 1)
                        {
                            var newFile = new FileItem { Name = item, Path = path };
                            currentNode.Items.Add(newFile);
                        } else
                        {
                            var newDirectory = new DirectoryItem { Path = PathUntil(pathItems, i), Name = item, Items = new List<Item>() };
                            currentNode.Items.Add(newDirectory);
                            currentNode = newDirectory;
                        }
                    }
                }
            }
            return rootNode.Items;
        }

        public string PathUntil(string[] path, int until)
        {
            return string.Join("/", path.Take(until));
        }
    }
}
