/********************************************************************
    created:	2019/9/16 15:43:27
    author:		rush
    email:		
	
    purpose:	
*********************************************************************/
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RLib.Base
{
	public static class AppEx
    {
        public static bool  IsExeRunnging(string exeName)                   // 是否exe正在运行
        {
            Process[] processes = Process.GetProcessesByName(exeName);
            return processes.Any();
        }

        public static bool  IsExeMultiple(string exeName)                   // 是否存在多个程序
        {
            Process[] processes = Process.GetProcessesByName(exeName);
            return processes.Length >= 2;
        }

        public static Process StartExe(string path)
        {
            try
            {
                return System.Diagnostics.Process.Start(path);
            }
            catch (Exception e)
            {
                return null;
            }
        }

        //public void Restart()
        //{
        //}
    }
}
