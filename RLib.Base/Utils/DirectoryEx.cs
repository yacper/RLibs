/********************************************************************
    created:	2018/8/9 17:21:03
    author:		rush
    email:		
	
    purpose:	
*********************************************************************/
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;

namespace RLib.Base
{
	public static class DirectoryEx
	{
	    public static bool  CreateDirectory(string path)                    // 传入的可以是一个文件，如果是一个文件，创建到这个文件名的文件夹
	    {
	        if (Path.HasExtension(path))
	        {
	            string file = Path.GetFileName(path);
	            path = path.TrimEnd(file.ToCharArray());
	        }

	        try
	        {
                if(!Directory.Exists(path))
	                Directory.CreateDirectory(path);

	            return true;
	        }
	        catch (Exception e)
	        {
	            return false;
	        }
	    }

	    public static string NewFileName(string file, IEnumerable<string> exists)
        {
            string ret = file;

            int index = int.MinValue;
            foreach (string s in exists)
            {
                if (s.StartsWith(file))
                {
                    if (int.TryParse(s.Remove(0,file.Length), out int newindex)&& newindex>index)
                    {
                        index = newindex;
                    }
                }
            }

            if (index != int.MinValue)
                index++;
            else
                index = 0;
            
            if (index != 0)
                ret += index.ToString();
            else if (exists.Contains(file))
            {
                ret += 1.ToString();
            }
            return ret;
        }

	    


		public static List<string> GetFilesRecursively(string dir, IEnumerable<string> exceptFiles = null, IEnumerable<string> exceptDirs = null)           // 递归获取dir下所有文件
        {
            List<string> ret = new List<string>();
            try
            {

                if (exceptFiles == null)
                    ret.AddRange(Directory.EnumerateFiles(dir));
                else
                    ret.AddRange(Directory.EnumerateFiles(dir).Except(exceptFiles, null, (a, b) => { return b.Contains(a); }));

                if (exceptDirs == null)
                {
                    foreach (string d in Directory.EnumerateDirectories(dir))
                    {
                        ret.AddRange(GetFilesRecursively(d, exceptFiles, exceptDirs));
                    }
                }
                else
                {
                    foreach (string d in Directory.EnumerateDirectories(dir).Except(exceptDirs, null, (a, b) => { return b.Contains(a); }))
                    {
                        ret.AddRange(GetFilesRecursively(d, exceptFiles, exceptDirs));
                    }
                }
            }
            catch (Exception e)
            {
                RLibBase.Logger.Error($"GetFilesRecursively:{dir}\n{e}");
            }


            return ret;
		}


		public static string GetName(string dir)							// 获取路径的目录名
		{
			if (true)  // 如果是dir
			{
				dir = dir.TrimEnd('/').TrimEnd('\\');

				char sep = '/';
				if (dir.Contains('\\'))
					sep = '\\';

				string[] d = dir.Split(sep);
				return d.LastOrDefault();
			}
		}

		public static bool	Move(string source, string dest, bool replace = true) // 如果dest directory不存在，创建  如果有同名文件存在，根据replace
		{
			string dir = Path.GetDirectoryName(dest);
			if (!string.IsNullOrWhiteSpace(dir) && 
				!Directory.Exists(dir))
				Directory.CreateDirectory(dir);

		    if (!Directory.Exists(source))
		        return false;

		    foreach (string f in Directory.GetFiles(source))
		    {
		        string relpath = f.Replace(source, "");
		        if (relpath.StartsWith("\\")||
		            relpath.StartsWith("/")
		            )
		            relpath = relpath.Substring(1, relpath.Length - 1);
		        FileEX.Move(f, PathEx.CombineRelative(dest, relpath));
		    }

		    foreach (string d in Directory.GetDirectories(source))
		    {
		        Move(d, PathEx.CombineRelative(dest, DirectoryEx.GetName(d)));
		    }

		    if (Directory.Exists(source))
		    {
                Directory.Delete(source, true);
		    }

		    return true;
		}

        public static bool  Delete(string dir)
        {
            try
            {
                //去除文件夹和子文件的只读属性
                //去除文件夹的只读属性
                System.IO.DirectoryInfo fileInfo = new DirectoryInfo(dir);
                fileInfo.Attributes = FileAttributes.Normal & FileAttributes.Directory;

                //去除文件的只读属性
                System.IO.File.SetAttributes(dir, System.IO.FileAttributes.Normal);

                //判断文件夹是否还存在
                if (Directory.Exists(dir))
                {
                    foreach (string f in Directory.GetFileSystemEntries(dir))
                    {
                        if (File.Exists(f))
                        {
                            //如果有子文件删除文件
                            File.Delete(f);
                            //Console.WriteLine(f);
                        }
                        else
                        {
                            //循环递归删除子文件夹
                            Delete(f);
                        }
                    }

                    //删除空文件夹
                    Directory.Delete(dir);
                    //Console.WriteLine(dir);
                }

            }
            catch (Exception ex) // 异常处理
            {
                Debug.WriteLine(ex.Message.ToString());// 异常信息
                return false;
            }

            return true;
        }



        /// 返回指示文件是否已被其它程序使用的布尔值
        /// </summary>
        /// <param name="fileFullName">文件的完全限定名，例如：“C:\MyFile.txt”。</param>
        /// <returns>如果文件已被其它程序使用，则为 true；否则为 false。</returns>
        public static bool IsInUsing(string dir, out string file, IEnumerable<string> exceptFiles = null, IEnumerable<string> exceptDirs= null )
        {
            bool result = false;
            file = null;

            //判断文件是否存在，如果不存在，直接返回 false
            if (!Directory.Exists(dir))
            {
                result = false;
            }
            else
            {//如果文件存在，则继续判断文件是否已被其它程序使用
                foreach (string s in GetFilesRecursively(dir, exceptFiles, exceptDirs))
                {
                    if (FileEX.IsInUsing(s))
                    {
                        file = s;

                        return true;
                    }
                }
            }

            return false;
        }

     //   // 尝试了很多方法，都没有特别好的, 直接操作是最简单有效的，但是效率可能差一些
	    //public static FileIOPermissionAccess GetFileIoPermission(string dir)
	    //{

	    //}
	
        // 尝试了很多方法，都没有特别好的, 直接操作是最简单有效的，但是效率可能差一些
	    public static bool  AllowFileIoPermission(FileIOPermissionAccess permit, string dir)
	    {
	        string[] files = null;

	        try
            {
                if ((permit & FileIOPermissionAccess.PathDiscovery) != 0)
                {
                    files = Directory.GetFiles(dir);
                }

                string tempPath = null;
                if ((permit & FileIOPermissionAccess.Append) != 0)
                {
                    tempPath = PathEx.CombineRelative(dir, Guid.NewGuid() + ".txt");
                    File.AppendAllText(tempPath, "hello");
                }

                if ((permit & FileIOPermissionAccess.Read) != 0)
                {
                    if (tempPath == null)
                    {
                        tempPath = PathEx.CombineRelative(dir, Guid.NewGuid() + ".txt");
                        File.AppendAllText(tempPath, "hello");
                    }

                    using (FileStream fs = File.OpenRead(tempPath))
                    {
                    }
                }

                if ((permit & FileIOPermissionAccess.Write) != 0)
                {
                    if (tempPath == null)
                    {
                        tempPath = PathEx.CombineRelative(dir, Guid.NewGuid() + ".txt");
                    }

                    File.WriteAllText(tempPath, "world");
                }

                try
                {
                    if (tempPath != null)
                        File.Delete(tempPath);
                }
                catch (Exception e)
                {
                }
            }
            catch (Exception e)
            {
                return false;
            }

            return true;
	    }
	}
}
