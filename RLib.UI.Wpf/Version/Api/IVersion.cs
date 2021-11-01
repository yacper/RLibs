/********************************************************************
    created:	2018/8/9 13:25:09
    author:		rush
    email:		
	
    purpose:	版本控制相關
*********************************************************************/
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using DataModel;
using NPOI.SS.Formula.Functions;

namespace RLib.Base
{
	public enum EVersionStat
	{
		Normal,
		Checking,					// 检查当前版本是否有被改动
		Downloading,				// 下载当前版本
		NeedUpdat,					// 已下载完，等待更新
		Updating,					// 更新到当前版本
        Error                       // 出错
	}
public class VersionFileDM                  // version 数据
{
    public string     file { get; set; }         // 文件名/目录名
    public string     md5 { get; set; }			// file有效,dir无效
    public long      size { get; set; }			// file有效,dir无效
    public string     zipMd5 { get; set; } 
    public long      ZipSize { get; set; } 
	public bool		isDir { get; set; }		//是否作为目录(以目录为单位打整包)
}
public class VersionDM                  // version 数据
{
    public string     num { get; set; }         // 版本号
    public string     name { get; set; } 
    public string     desc { get; set; } 
    public long      time { get; set; } 
    public long?      size { get; set; } 
    public long?      ZipSize { get; set; } 

	public string		exe { get; set; }		// 主执行文件，如果有
	public List<string>		detectExes { get; set; }		// 其他检测执行文件，如果有，比如Updater等(当Updater在运行是，也同样认为version在运行状态)

    public List<VersionFileDM> files { get; set; } 
}



	public interface IVersion:IDynamicProduct<string>
	{
        System.Version      Number { get; }
//		string				Number { get; }									// 版本号，同时也是version的id
		string				Name { get; }
		string				Desc { get; }
		DateTime			Time { get; }
		long?				Size { get; }									// 版本大小
		long?				ZipSize { get; }								// zip 后的大小

		IEnumerable<VersionFile> Files { get; }

		string				RemoteDirectory { get; }						// 版本目录
		string				LocalDirectory { get; }							// 本地Exe目录


		EVersionStat		Stat { get; }
		float				Progress { get; }								// 0-100的
		float				DownloadSpeed { get; }							// 以k为单位的下载速度

        ///  如果有EXE
	    string		        ExeFile { get; }		                        // 可执行文件，如果有
	    IEnumerable<string> DetectExes { get; }		// 其他需要检测执行文件，如果有，比如Updater等(当Updater在运行是，也同样认为version在运行状态)
        string              ExePath { get; }                                // 可执行文件路径
        Process             StartExe();                                     // 启动可执行文件
        Process             DetectExistsExe();                              // 检查是否启动Exe
        Process             ExeProcess { get; }
        bool                IsMainExeRunning { get; }                       // 当前运行的是主exe，还是其他exe（比如Updater）



		bool				Check();										// 检查当前版本文件是否正常，有没有被修改
		Task<bool>			CheckAsync();										// 检查当前版本文件是否正常，有没有被修改
		Task<bool>			CheckStore(IEnumerable<VersionFile> files = null);// 检查


		IEnumerable<VersionFile> Different(IVersion v);						// 区别的file


		Task<bool>			DownloadUpdate(IVersion from = null);			// download相对于另一个版本的更新文件(null 则下载全部)
		Task<bool>			UpdateTo(IVersion to);					    // 从另一个版本进行更新，更新文件到本目录
	}




	public struct VersionFile:IEqualityComparer<VersionFile>												// 在Version管理下的File
	{
		public override string ToString()
		{
			return File;
		}

		public bool			Equals(VersionFile x, VersionFile y)
		{
			if (RMath.Equal(x.File, y.File))
			{
				if (x.DM.zipMd5.IsNullOrWhiteSpace() && y.DM.zipMd5.IsNullOrWhiteSpace())
					return Md5HashEx.Equal(x.ZipMd5, y.ZipMd5);
				else if (x.DM.md5.IsNullOrWhiteSpace() && y.DM.md5.IsNullOrWhiteSpace())
					return Md5HashEx.Equal(x.Md5, y.Md5);
				else
					return false;
			}
			else
				return false;
		}

		public int			GetHashCode(VersionFile obj)
		{
			if(obj.ZipMd5 != null)
				return obj.ZipMd5.GetHashCode();
			else if (obj.Md5 != null)
				return obj.Md5.GetHashCode();
			else
				return obj.File.GetHashCode();
		}


		public object ToDm()
		{
			return _DM;
		}


		public VersionFileDM DM{get { return _DM; }}

		public string		File { get { return DM.file; } }
		public string		Md5 { get
		{
			if (DM.md5.IsNullOrWhiteSpace()) return DM.md5;
			else return null;
		} }
		public string       ZipMd5
		{
			get
			{
				if(DM.zipMd5.IsNullOrWhiteSpace())
					return DM.zipMd5;
				else
					return null;
			}
		}
		public long			Size { get { return DM.size; } }
		public long			ZipSize { get { return DM.ZipSize; } }

		public bool			IsDir { get { return DM.isDir; } }


		public				VersionFile(VersionFileDM dm)
		{
			_DM = dm;
		}


		private VersionFileDM _DM;
	}

	public static class VersionFileEx
	{
		public static long	DataSize(this IEnumerable<VersionFile> items) // 获取大小
		{
			return items.Sum(p => p.ZipSize);
		}
	}

		

}
