/********************************************************************
    created:	2019/9/16 15:43:27
    author:		rush
    email:		
	
    purpose:	
*********************************************************************/
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RLib.Base
{
	 public static class CmdEx
    {
        /// <summary>
        /// Execute command without output under cmd.
        /// </summary>
        /// <param name="command"></param>
        public static void  Execute(string command)
        {
            var processInfo = new ProcessStartInfo(Cmd, CmdParam + command)
            {
                CreateNoWindow = true,
                UseShellExecute = true,
                WindowStyle = ProcessWindowStyle.Hidden
            };

            Process.Start(processInfo);
        }

        /// <summary>
        /// Execute command with output under cmd. 
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public static string ExecuteWithOutput(string command, out string err)
        {
            var processInfo = new ProcessStartInfo(Cmd, CmdParam + command)
            {
                CreateNoWindow = true,
                UseShellExecute = false,
                WindowStyle = ProcessWindowStyle.Hidden,
                RedirectStandardOutput = true,
                RedirectStandardError = true
            };

            var process = new Process {StartInfo = processInfo};
            process.Start();
            var outpup = process.StandardOutput.ReadToEnd();
            err = process.StandardError.ReadToEnd();

            process.WaitForExit();
            return outpup;

        }

        /// <summary>
        /// The name of Command Prompt (cmd).
        /// </summary>
        private static string Cmd { get; } = "cmd.exe";

        // https://docs.microsoft.com/en-us/windows-server/administration/windows-commands/cmd
        // `/C` means carrying out the command specified by String and then stops.
        // `/S` means any text following the closing quotation marks is preserved.
        private static string CmdParam { get; } = "/S /C ";
    }
}
