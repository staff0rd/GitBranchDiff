using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GitBranchDiff
{
    public class ShellCommand
    {
        public List<string> Execute(string fileName, string arguments, string workingDirectory)
        {
            var result = new List<string>();

            var proc = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = fileName,
                    Arguments = arguments,
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    CreateNoWindow = true,
                    WorkingDirectory = workingDirectory
                }
            };

            proc.Start();

            while (!proc.StandardError.EndOfStream)
            {
                result.Add(proc.StandardError.ReadLine());
            }

            while (!proc.StandardOutput.EndOfStream)
            {
                result.Add(proc.StandardOutput.ReadLine());
            }

            return result;
        }
    }
}
