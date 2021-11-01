/********************************************************************
    created:	2018/8/9 13:36:08
    author:		rush
    email:		
	
    purpose:	
*********************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RLib.Base
{
	public enum EVMStat
	{
		None,					// 无版本
		Corrupted,				// 当前版本已损坏
		NeedCheck,			    // 需要检查
		Checking,               // 正在检查游戏,
		CanInstall,				// 可以安装
		NeedDownload,			// 需要下载更新,
		Downloading,			// 下载中
		NeedUpdate,				// 已下载需要更新替换,
		Updating,				// 更新中,
		Executable,				// 可启动
		Executing,				// 正在运行
        NeedIOPermission,       // 需要IO权限，权限不足，处于这种状态，只能重启以管理员方式运行了
        BadSetting,             // 版本設置錯誤
        UnKnownError,           // 位置错误
        NetWorkErro,            // 网络错误
	}



	public interface IVersionManager:IManager<string, IVersion>
	{
		EVMStat				Stat { get; set; }


		string				RemoteVersionsDir { get; }						// 远程版本目录
		string				LocalExeDir { get; set; }						// 本地执行版本目录

		IVersion			CurVersion { get; }
		IVersion			LatestVersion { get; }							// 

        string              LastError { get; }                              // 出错信息


		Task<bool>	        CheckLatest();			// 发起检查是否有更新, 是否完全忽略本地版本
		Task<bool>			UpdateToLatest(bool ignoreLocal = false);								// 更新到最新 (是一个系列过程，下载在更新等)


		IVersion			MakeVersion(string name, string num, string dir, bool produce = true, string desc= null, string exe=null, IEnumerable<string> detectExes =null,DateTime? time = null, bool wrapDir = true);  // 创建一个Version, 在文件夹的平行目录生成一个Version目录

		Task<IVersion>		MakeVersionAsync(string name, string num, string dir, bool produce = true, string desc= null, string exe=null, IEnumerable<string> detectExes =null, DateTime? time = null, bool wrapDir = true);  // 创建一个Version, 在文件夹的平行目录生成一个Version目录

		IVersion			DetectVersion(string dir, string exe= null);	// 检查目录, 获取Version (如果有exe，还要比对exe是否ok-- 有可能用户自己进行更新了)

		List<VersionFile>	CheckDownloadVersionDir(string dir);			// 检查目录, 获取VersionFile(主要用于比较下载不全的目录)

	}
}
