using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace GitBranchDiff
{
    public class ShellCommand
    {
        private List<string> Execute(string fileName, string arguments, string workingDirectory)
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

        internal List<string> ExecuteGit(string arguments, string directoryName)
        {
            return Execute("git", $"--no-pager {arguments}", directoryName);
        }
    }
}
