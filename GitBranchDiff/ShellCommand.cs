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

            proc.OutputDataReceived += new DataReceivedEventHandler((sender, e) =>
            {
                if (!string.IsNullOrEmpty(e.Data))
                {
                    result.Add(e.Data);
                }
            });

            proc.Start();

            proc.BeginOutputReadLine();

            proc.WaitForExit();
            proc.Close();

            return result;
        }

        internal List<string> ExecuteGit(string arguments, string directoryName)
        {
            return Execute("git", $"--no-pager {arguments}", directoryName);
        }
    }
}
