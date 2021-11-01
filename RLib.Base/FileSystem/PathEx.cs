/********************************************************************
    created:	2018/8/10 15:10:22
    author:		rush
    email:		
	
    purpose:	
*********************************************************************/
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RLib.Base
{
	public class PathEx
	{

		public static string CombineRelative(string path1, string path2)	// 可以combine2个relative path (Path.Combine 如果第二个参数是相对路径， 只会返回第二个参数)
		{// todo: 增加各类判断

			string left = path1.TrimEnd('/').TrimEnd('\\');
			string right = path2.TrimStart('/').TrimStart('\\');

			return left + "\\" + right;
		}

		public static string TrimExtension(string path)					// 移除ext
		{// todo: 增加各类判断

			int index = path.LastIndexOf(".");
			if (index == -1)
				return path;

			string ret = path.Substring(0, path.Length - (path.Length - index));
			return ret;
		}

	    public static string GetDirectory(string path)                      // 根据path获取完整dir
	    {
	        string filename = Path.GetFileName(path);
	        string ret = path.Replace(filename, "");
	        return ret;
	    }
	}
}
