using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L4d2_Mod_Manager_Tool.Arthitecture
{
    public static class RunProcess
    {
        public static string Run(string exePath, string workDir, string args)
        {
            Process p = new Process();
            ProcessStartInfo si = new ProcessStartInfo();
            si.CreateNoWindow = true;
            si.FileName = exePath;
            si.RedirectStandardError = true;
            si.RedirectStandardInput = true;
            si.RedirectStandardOutput = true;
            si.WorkingDirectory = workDir;
            string arg = args;
            si.Arguments = arg;

            p.StartInfo = si;
            p.Start();

            return p.StandardOutput.ReadToEnd();
        }

        public static string Run(string exePath, string args)
        {
            return Run(exePath, Environment.CurrentDirectory, args);
        }
    }
}
