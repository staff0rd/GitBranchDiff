using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GitBranchDiff.Model
{
    public class Item
    {
        public string Name { get; set; }
        public string Path { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsAdded { get; set; }
        public bool IsModified {  get { return !IsDeleted && !IsAdded; } }
    }
}
