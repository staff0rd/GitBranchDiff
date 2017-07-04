using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GitBranchDiff.Model
{
    public class FileItem : Item
    {
        public string Extension
        {
            get
            {
                return System.IO.Path.GetExtension(Path);
            }
        }
    }
}
