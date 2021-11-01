/********************************************************************
    created:	2018/8/9 13:37:43
    author:		rush
    email:		
	
    purpose:	为了让玩家的下载量达到最小，基本原则是压缩每个文件，并且下载单元是单个文件
*********************************************************************/
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Permissions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DataModel;
using FluentFTP;
using NPOI.OpenXmlFormats.Spreadsheet;

namespace RLib.Base
{
	public class VersionManager:Manager<string, IVersion>, IVersionManager
	{
		public override void Init()
		{
			base.Init();
			
			_MD5 = MD5.Create();  // 创建一个Md5的实例

			try
			{
				if (LocalExeDir != null)
				{
					IVersion v = DetectVersion(LocalExeDir);
					if(v != null)
						CurVersion = v;
				}
			}
			catch (Exception e)
			{
				Console.WriteLine(e);
			}
		}

		public EVMStat		Stat { get { return _Stat; } set { Set("Stat", ref _Stat, value); } }

		public string		RemoteVersionsDir { get; set; }		// 远程版本目录
		public string		LocalExeDir
		{
		    get { return _LocalExeDir;}
		    set
		    {
                // 一旦设置目录，需要查看目录的权限
		        if (string.Compare(_LocalExeDir, value) == 0)
		            return;

                if(!Directory.Exists(value))
                    throw new Exception("不存在目录");

		        _LocalExeDir = value;
		    }
        }				// 本地执行版本目录

		public IVersion		CurVersion { get { return _CurVersion; } set { Set("CurVersion", ref _CurVersion, value); }}
		public IVersion		LatestVersion { get { return _LatestVersion; } set { Set("LatestVersion", ref _LatestVersion, value); }}							// 


        public string              LastError { get; set; }                              // 出错信息

		public async Task<bool>   CheckLatest()									// 发起检查是否有更新
		{// 从服务器目录中找到最新版本

			// 假设当前版本正常，或者为空

			//FtpClient ftp = RLibBase.FtpClient.Clone();
		    FtpClient ftp = RLibBase.FtpClient;

			Stat = EVMStat.Checking;
		    try
		    {
		        //Debug.Print(SynchronizationContext.Current.ToString());

		        await ftp.ConnectAsync();

		        FtpListItem[] items = await ftp.GetListingAsync(RemoteVersionsDir);
		        // 查找一个File，并且其名就是最新版本的名称(sharkev_2019.4.13.0_.txt)
		        string dir = items.Where(p => p.Type == FtpFileSystemObjectType.File).LastOrDefault()?.Name;
		        if (string.IsNullOrWhiteSpace(dir))
		        {// 没有最新版本，返回正常
                    if (CurVersion.DetectExistsExe() == null)
		                Stat = EVMStat.Executable;

		            return true;
		        }

		        // 判断ver和当前ver是否一致
                string[] slatests = dir.Split('_');
                string slatest = slatests[1];
		        System.Version latest = System.Version.Parse(slatest);
		        if (CurVersion != null &&
		            CurVersion.Number >= latest) // 当前的version>=服务器latest(有可能服务器还没来及更新)
		        {
		            if (CurVersion.DetectExistsExe() == null)
		                Stat = EVMStat.Executable;

		            return true;
		        }

		        dir = slatests[0] + "_" + slatests[1];


		        // 不一致，下载最新版本信息
		        string localDir = Path.Combine(RemoteVersionsDir, dir);
		        Directory.CreateDirectory(localDir);

		        string latestDmFile = Path.Combine(localDir, Version.VersionFileName);

		        int ret = await ftp.DownloadFilesAsync(AppDomain.CurrentDomain.BaseDirectory, new[] {latestDmFile});
		        if (ret == 0)
		        {
		            throw new Exception("从服务器下载版本文件出错");
		        }

		        //VersionDM dm = ProtobufHelper.DeserializeMsg<VersionDM>(latestDmFile);
		        VersionDM dm = latestDmFile.FileToJsonObj<VersionDM>();
		        if (LatestVersion != null &&
		            dm.num == LatestVersion.Number.ToString())
		        {

		        }
		        else
		        {
		            // 去除同名version
		            if (this.ContainsKey(dm.num))
		                this.Remove(dm.num);

		            Version v = InstantiateAndAdd(typeof(Version).FullName, dm) as Version;

		            LatestVersion = v;
		        }

		        if (CurVersion != null)
		            Stat = EVMStat.NeedDownload; // 需要进行下载
		        else
		            Stat = EVMStat.CanInstall;

		        await ftp.DisconnectAsync();

		        LastError = null;
		        return true;
		    }
		    catch (Exception e)
		    {
		        RLibBase.Logger.Error(e);

		        LastError = "检查平台更新出错, 请检查网络配置或联系客服";
                await MessageBoxEx.ShowAsync("检查平台更新出错, 请检查网络配置或联系客服", "错误");
                Stat = EVMStat.NeedCheck;
                await ftp.DisconnectAsync();

		        return false;
		    }

            return false;
		}

		public async Task<bool> UpdateToLatest(bool ignoreLocal = false) // 更新到最新 (是一个系列过程，下载在更新等)
		{
	        Stat = EVMStat.Checking;

			if (ignoreLocal)
			{
				// 首先，检查下当前版本是否ok
				if(CurVersion != null)
				{
					if(!await CurVersion.CheckAsync())
					{
						string dirName = DirectoryEx.GetName(LocalExeDir);
                        UnInInstantiateAndRemove(CurVersion);  // 卸载老的, 必须
						CurVersion = await MakeVersionAsync(dirName, "0.0.0.0", LocalExeDir, false);
					}
				}
			}

			if(LatestVersion == null ||
			    LatestVersion == CurVersion)
			    if (!await CheckLatest())
			    {
                    return false;
                }

			/// 更新到最新
			if (LatestVersion != null &&
			    LatestVersion != CurVersion)
			{
				try
				{
					Stat = EVMStat.Downloading;
					bool ok = await LatestVersion.DownloadUpdate(CurVersion);
					if(!ok)  // 下载出错
					{
                        //
					    await MessageBoxEx.ShowAsync("下载版本出错, 请检查网络配置或稍后再试", "错误");

						Stat = EVMStat.NeedDownload;
						return false;
					}


					Stat = EVMStat.Updating;
					ok = await LatestVersion.UpdateTo(CurVersion); // 更新到最新
					if (!ok)
					{
					    await MessageBoxEx.ShowAsync("更新版本出错", "错误");

						Stat = EVMStat.NeedUpdate;
						return false;
					}

					CurVersion = LatestVersion;

					Stat = EVMStat.Executable;

					return true;
				}
				catch (Exception e)
				{
					Console.WriteLine(e);
					return false;
				}
			}
			else   // 当前就是最新版本了
			{
			    Process p = CurVersion.DetectExistsExe(); // 检查是否可启动

				return true;
			}
		}


		public IVersion		DetectVersion(string dir, string exe= null)						// 检查目录, 获取Version
		{
		    try
		    {
                /// 检查安装目录版本状况
                string versionFile = PathEx.CombineRelative(dir, "Version.dm");
                if (File.Exists(versionFile))
                {
                    //VersionDM dm = ProtobufHelper.DeserializeMsg<VersionDM>(versionFile);
                    VersionDM dm = versionFile.FileToJsonObj<VersionDM>();
                    if (dm == null)
                        return null;

                    if (exe != null)
                    {
                        string exePath = PathEx.CombineRelative(dir, exe);
                        if (File.Exists(exePath))
                        {
                            var versInfo = FileVersionInfo.GetVersionInfo(exePath);
                            String ver = versInfo.FileVersion;

                            if (!RMath.Equal(ver, dm.num)) // 如果exe和dm的版本不同，说明当前版本已经过时，认为其无效
                                return null;
                        }
                    }

                    // 去除同名version
                    if (this.ContainsKey(dm.num))
                        this.Remove(dm.num);
                    Version v = InstantiateAndAdd(typeof(Version).FullName, dm) as Version;

                    return v;
                }
                else
                    return null;
            }
            catch (Exception e)
            {
                return null;

            }
		}

		public List<VersionFile> CheckDownloadVersionDir(string _dir) // 检查目录, 获取VersionFile(主要用于比较下载不全的目录，或者被损坏的目录)
		{
			List<VersionFile> lfv = new List<VersionFile>();

			// 检查目录
			if (!Directory.Exists(_dir))
				return lfv;

			List<string> files = DirectoryEx.GetFilesRecursively(_dir);
			if (!files.Any())
				return lfv;
			
			foreach (string f in files)
			{
				//string relFile = f.TrimStart(_dir.ToCharArray()) ;  // 将头部文件夹去掉

				string relFile = f.Substring(_dir.Length +1, f.Length - _dir.Length -1) ;  // 将头部文件夹去掉
				relFile = PathEx.TrimExtension(relFile);
				System.IO.FileInfo info = new System.IO.FileInfo(f);

				VersionFileDM fdm = new VersionFileDM()
				{
					file = relFile,
					zipMd5 = Md5HashEx.GetMd5HashFromFile(f),
					ZipSize = info.Length,
				};

				lfv.Add(new VersionFile(fdm));
			}

			return lfv;
		}


		public IVersion		MakeVersion(string _name, string _num, string _dir, bool produce = true, string _desc= null, string _exe = null, IEnumerable<string> detectExes =null, DateTime? time = null, bool wrapDir = true) // 创建一个Version, 在文件夹的平行目录生成一个Version目录
		{
		    try
            {
                _dir = _dir.TrimEnd('\\').TrimEnd('/');

                VersionDM dm = new VersionDM()
                {
                    name = _name,
                    num = _num,
                    exe = _exe,
                };
                if (detectExes != null)
                    dm.detectExes.AddRange(detectExes);

                if (_desc != null)
                    dm.desc = _desc;
                if (time == null)
                    time = DateTime.Now;
                dm.time = time.Value.Ticks;

                // 检查目录
                if (!Directory.Exists(_dir))
                {
                    RLibBase.Logger.Error(string.Format("不存在目录{0}", _dir));

                    return null;
                }

                if (_exe != null)
                {
                    if(!File.Exists(PathEx.CombineRelative(_dir, _exe + ".exe")))
                        RLibBase.Logger.Error(string.Format("不存在exe:{0}", _exe));
                }



                List<string> files = DirectoryEx.GetFilesRecursively(_dir);
                if (!files.Any())
                {
                    RLibBase.Logger.Error("MakeVersion目录为空:" + _dir);
                    return null;
                }


                // 创建一个平行的版本目录
                string versionDir = null;
                if (produce)
                    versionDir = _dir + "_" + _num;
                else
                {
                    versionDir = System.IO.Path.GetTempPath() + "\\" + _num;  // 使用临时目录，系统目录会报错
                }
                Directory.CreateDirectory(versionDir);


                List<VersionFileDM> lfv = new List<VersionFileDM>();
                long size = 0;
                long zipSize = 0;
                if (wrapDir)
                    files = Directory.GetFiles(_dir).ToList();  // 如果wrapdir， 包装直属目录下的文件
                foreach (string f in files)
                {
                    //string relFile = f.TrimStart(_dir.ToCharArray()) ;  // 将头部文件夹去掉

                    string relFile = f.Substring(_dir.Length + 1, f.Length - _dir.Length - 1);  // 将头部文件夹去掉
                    System.IO.FileInfo info = new System.IO.FileInfo(f);

                    VersionFileDM fdm = new VersionFileDM()
                    {
                        file = relFile,
                        md5 = Md5HashEx.GetMd5HashFromFile(f),
                        size = info.Length,
                    };

                    size += info.Length;


                    if (produce) // 如果不生成，就不创建zip
                    {
                        // 將文件zip后放入versionDir
                        string zipPath = versionDir + "\\" + relFile + ".zip";      // 最后加一个zip后缀
                        ZipHelper.CompressFile(f, zipPath, null, null, 9);

                        fdm.zipMd5 = Md5HashEx.GetMd5HashFromFile(zipPath);
                        fdm.ZipSize = new FileInfo(zipPath).Length;

                        zipSize += fdm.ZipSize;
                    }

                    lfv.Add(fdm);

                }
                if (!wrapDir)
                    dm.size = size;

                if (wrapDir)  // 打包直属目录下的子目录
                {
                    List<string> dirs = Directory.GetDirectories(_dir).ToList();
                    foreach (string d in dirs)
                    {
                        string reldir = d.Substring(_dir.Length + 1, d.Length - _dir.Length - 1);  // 将头部文件夹去掉
                        System.IO.DirectoryInfo info = new System.IO.DirectoryInfo(d);

                        VersionFileDM fdm = new VersionFileDM()
                        {
                            file = reldir,
                            isDir = true
                        };

                        if (produce) // 如果不生成，就不创建zip
                        {
                            // 將文件zip后放入versionDir
                            string zipPath = versionDir + "\\" + reldir + ".zip";      // 最后加一个zip后缀

                            ZipHelper.CompressFile(Directory.GetFileSystemEntries(d), zipPath, null, null, 9);

                            fdm.zipMd5 = Md5HashEx.GetMd5HashFromFile(zipPath);
                            fdm.ZipSize = new FileInfo(zipPath).Length;

                            zipSize += fdm.ZipSize;
                        }

                        lfv.Add(fdm);
                    }

                }
                dm.files = lfv;

                // 最后将dm文件也放到版本目录中
                if (produce)
                {
                    dm.ZipSize = zipSize;

                    //ProtobufHelper.SerializeMsg(dm, versionDir + "\\" + "Version.dm");
                    dm.ToJsonFile(versionDir + "\\" + "Version.dm");

                }
                else
                {
                    try
                    {// 可能权限不足
                    //    ProtobufHelper.SerializeMsg(dm, _dir + "\\" + "Version.dm");
                        dm.ToJsonFile(versionDir + "\\" + "Version.dm");
                        Directory.Delete(versionDir, true);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                        throw;
                    }
                }

                // 去除同名version
                if (this.ContainsKey(dm.num))
                    this.Remove(dm.num);

                return InstantiateAndAdd(typeof(Version).FullName, dm) as Version;  // 可能会存在同名version
            }
            catch (Exception e)
            {
                RLibBase.Logger.Error(e);
                return null;
            }
        }


		public async Task<IVersion> MakeVersionAsync(string name, string num, string dir, bool produce = true, string desc = null,string exe = null,IEnumerable<string> detectExes =null, DateTime? time = null, bool wrapDir = true) // 创建一个Version, 在文件夹的平行目录生成一个Version目录
		{

			Task<IVersion> task = new Task<IVersion>
				(
				 (obj) =>
				 {
					 var o = (dynamic) obj;
					 return MakeVersion(o.name, o.num, o.dir, o.produce, o.desc, o.exe, detectExes, o.time, wrapDir);
				 },
				 new {name, num, dir, produce, desc, exe, detectExes, time, wrapDir}
			 );

			task.Start();

			await Task.WhenAll(task);

			return task.Result;
		}

	

#region C&D
		public				VersionManager()
		{
		}
#endregion

#region Members
		protected MD5		_MD5;
		protected EVMStat	_Stat;

		protected IVersion	_CurVersion;
		protected IVersion	_LatestVersion;

	    protected string    _LocalExeDir;
#endregion
	}
}
