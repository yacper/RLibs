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
	public static class PathEx
	{
        public static string CombinePath(this string path1, string path2) // path.combine
        {
            return Path.Combine(path1, path2);
        }

        public static string CombinePath(this string path1, params string[] paths) // path.combine
        {
            return Path.Combine(path1, Path.Combine(paths));
        }
        public static string CombinePath(this IEnumerable<string> paths) // path.combine
        {
            return Path.Combine(paths.ToArray());
        }


		public static string CombineRelative(string path1, string path2)	// 
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
