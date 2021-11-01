/********************************************************************
    created:	2018/8/9 13:25:19
    author:		rush
    email:		
	
    purpose:	
*********************************************************************/
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using DataModel;
using FluentFTP;

namespace RLib.Base
{
	public partial class Version:DynamicProduct<string>, IVersion
	{
		public const string VersionFileName = "Version.dm";

		public override string ToString()
		{
			return Name + "_" +Number ;
		}

		public override void Init()
		{
			base.Init();

            if(App.Instance != null)
			    App.Instance.OnUpdateEvent += _Update;
		}

		public override void UnInit()
		{
			base.UnInit();
			
            if(App.Instance != null)
			    App.Instance.OnUpdateEvent -= _Update;
		}


		//public ProtoBuf.IExtensible ToDm()
		//{
  //          throw new NotImplementedException();
		//}

        public System.Version Number { get; protected set; }
//		public string		Number { get { return ID; } set { ID = value; }}	// 版本号，同时也是version的id
		public string		Name { get { return _Name; } }
		public string		Desc { get { return _Desc; } }
		public DateTime		Time { get { return _Time; } }
		public long?		Size { get { return _Size; } }									// 版本大小
		public long?		ZipSize { get { return _ZipSize; } }								// zip 后的大小
		public IEnumerable<VersionFile> Files { get { return _Files; } }


		public string		RemoteDirectory { get { return Path.Combine((Manager as VersionManager).RemoteVersionsDir, Name + "_" + Number); } }	// 版本目錄
		public string		LocalDirectory { get { return (Manager as VersionManager).LocalExeDir; } }	// 本地

		public EVersionStat	Stat { get { return _Stat; } set { Set("Stat", ref _Stat, value); } }
		public float		Progress { get { return _Progress; } set { Set("Progress", ref _Progress, value); } }								// 0-1的
		public float		DownloadSpeed { get { return _DownloadSpeed; } set { Set("DownloadSpeed", ref _DownloadSpeed, value); } }							// 以k为单位的下载速度

	    public string		ExeFile { get; protected set; }		                        // 可执行文件，如果有
	    public IEnumerable<string> DetectExes { get; protected set; }		// 其他需要检测执行文件，如果有，比如Updater等(当Updater在运行是，也同样认为version在运行状态)

	    public string       ExePath
	    {
	        get
	        {
                if (string.IsNullOrWhiteSpace(LocalDirectory))
                    return null;
                else
                    return PathEx.CombineRelative(LocalDirectory, ExeFile + ".exe");

	        }
	    } // 可执行文件路径

	    public virtual Process StartExe() // 启动可执行文件
        {
            if (string.IsNullOrWhiteSpace(ExePath))
                return null;

            if (_ExeProcess != null)
                return _ExeProcess;

            try
            {
                _ExeProcess = System.Diagnostics.Process.Start(ExePath);
                _ExeProcess.EnableRaisingEvents = true;  // 需要设置为true，不然不通知
                //                _ExeProcess.Exited +=;

                _ExeProcess.Exited += __ProcessExitedAsync;

                //   _ExeProcess.SynchronizingObject = this;  // 设置使用主线程的

                IsMainExeRunning = true;

                (Manager as VersionManager).Stat = EVMStat.Executing;
            }
            catch (Exception e)
            {
				RLibBase.Logger.Error(e.ToString());
                //MessageBox.Show("无法启动，版本损坏，请点击修复！");

	            //VersionManager.Stat = EVMStat.Corrupted;
                (Manager as VersionManager).Stat = EVMStat.Corrupted;
            }

            return ExeProcess;
        }


        public Process      DetectExistsExe()                               // 检查是否启动Exe
        {
            if (ExeProcess != null)
                return ExeProcess;

            _ExeProcess  = Process.GetProcessesByName(ExeFile).FirstOrDefault();
            if(_ExeProcess  != null)
                _IsMainExeRunning = true;
            else
            {
                foreach (var exe in DetectExes)
                {
                    _ExeProcess  = Process.GetProcessesByName(exe).FirstOrDefault();
                    break;
                }

                _IsMainExeRunning = false;
            }

            if (_ExeProcess != null)
            {
                try
                {
                    _ExeProcess.EnableRaisingEvents = true;  // 需要设置为true，不然不通知  // 这里有可能会报错，拒绝访问，这个时候，直接返回null
                    _ExeProcess.Exited += __ProcessExitedAsync;


                    (Manager as VersionManager).Stat = EVMStat.Executing;
                }
                catch (Exception e)
                {
                    return null;
                }
            }

            return _ExeProcess;
        }


        public Process       ExeProcess
        {
            get { return _ExeProcess; }
        }

        public bool                IsMainExeRunning
        {
            get { return _IsMainExeRunning; }
            set { Set("IsMainExeRunning", ref _IsMainExeRunning, value); }
        }                       // 当前运行的是主exe，还是其他exe（比如Updater）

        protected void      __ProcessExitedAsync(object sender, EventArgs e)  // 子线程函数
        {
            // 使其返回主线程执行
            System.Windows.Application.Current.Dispatcher.BeginInvoke(new Action(delegate
                {
                    int exitCode = (sender as Process).ExitCode;

                    if (exitCode == 0 ||        // 正常退出
                        exitCode == 1 ||        // 强退（任务管理器中止）
                        exitCode == -903||       // updater被关掉   
                        exitCode == 13 ||       // updater被关掉   
                        exitCode == -705||      // 断连后发生，没有看到实际情况
                        exitCode == -805306369||   // ps死了，不知道具体原因   
                        exitCode == -910   // 
                        )       
                    {
                        //(Manager as VersionManager).Stat = EVMStat.Executable;
                        (Manager as VersionManager).CheckLatest(); // 关闭后，应该在检查一次
                    }
                    else
                    {
                        RLibBase.Logger.Error("VersionExe 退出异常:" + exitCode);

                        (Manager as VersionManager).Stat = EVMStat.Corrupted;  // 损坏
                    }

                    _ExeProcess = null;
                }),
                System.Windows.Threading.DispatcherPriority.ApplicationIdle, null);

            // 进程已经退出后，无法获取它的名字
	        //// 使其返回主线程执行
	        //System.Windows.Application.Current.Dispatcher.BeginInvoke(new Action<object[]>(delegate(object[] args)
	        //    {
	        //        int exitCode = (int)args[1] ;

	        //        if (exitCode == 0 || // 正常退出
	        //            exitCode == 1 || // 强退（任务管理器中止）
	        //            exitCode == -903 || // updater被关掉   
	        //            exitCode == 13) // updater被关掉   
	        //        {
	        //            //(Manager as VersionManager).Stat = EVMStat.Executable;
	        //            (Manager as VersionManager).CheckLatest(); // 关闭后，应该在检查一次
	        //        }
	        //        else
	        //        {
	        //            RLibBase.Logger.Error(args[0].ToString() + " VersionExe 退出异常:" + exitCode);

	        //            (Manager as VersionManager).Stat = EVMStat.Corrupted; // 损坏
	        //        }

	        //        _ExeProcess = null;
	        //    }),
	        //    System.Windows.Threading.DispatcherPriority.ApplicationIdle,
	        //    new object[] {(sender as Process).ProcessName, (sender as Process).ExitCode});

        }


		public bool			Check()											// 检查当前版本文件是否正常，有没有被修改
		{
			foreach(VersionFile vf in this.Files)
			{
				string filepath = vf.File;

				if(!File.Exists(filepath))
					return false;

				if (!Md5HashEx.VerifyFileMd5Hash(filepath, vf.Md5))
					return false;
			}

			return true;
		}

		public async Task<bool>	CheckAsync()											// 检查当前版本文件是否正常，有没有被修改
		{
			Task<bool> task = new Task<bool>
				(
				 () =>
				 {
					 return Check();
				 }
			 );

			task.Start();

			await Task.WhenAll(task);

			return task.Result;
		}



		public async Task<bool>	CheckStore(IEnumerable<VersionFile> files = null) // // 检查version目录里面的文件是否ok
		{
			// 检查目录是否ok
			if(!Directory.Exists(RemoteDirectory))
				return false;

			if(files == null)
				files = this.Files;

			Stat = EVersionStat.Checking;
			Progress = 0;

			foreach (VersionFile vf in files)
			{
				string filepath = PathEx.CombineRelative(RemoteDirectory, vf.File) + ".zip";

				if (!File.Exists(filepath))
					return false;

				if (!Md5HashEx.VerifyFileMd5Hash(filepath, vf.ZipMd5))
					return false;
			}

			Progress = 100;
			Stat = EVersionStat.Normal;

			return true;
		}

		public IEnumerable<VersionFile> Different(IVersion v)				// 区别的file
		{
			return v.Files.Except(this.Files, new VersionFile());
		}

		public async Task<bool>	Download()						// download 整个版本到一个目录
		{
			FtpClient ftp = RLibBase.FtpClient.Clone();

            if(!ftp.IsConnected)
			    await ftp.ConnectAsync();

			Stat = EVersionStat.Downloading;
			Progress = 0;

			int ret = await ftp.DownloadDirectoryAsync(AppDomain.CurrentDomain.BaseDirectory, RemoteDirectory, (x, y)=>
				{
					_DownloadSpeedAsync = x;   // 設置Async 更新值， todo: 如果以後有更好的方法
					_ProgressAsync = y;
				});

			Progress = 100;
			Stat = EVersionStat.Normal;

			//await ftp.DisconnectAsync();

			return ret != 0;
		}

		public async Task<bool> DownloadUpdate(IVersion from = null)
		{
			List<VersionFile> files = null;
			if (from != null)
				files = this.Files.Except(from.Files, new VersionFile()).ToList(); // 传入比较条件
			else
				files = this.Files.ToList();

			// 检测本地是否已下载部分或者之前被中断，如果有，只下载剩余部分
			List<VersionFile> downloaded = (Manager as VersionManager).CheckDownloadVersionDir(RemoteDirectory);
			if(downloaded.Count != 0)
			{
				files = files.Except(downloaded, new VersionFile()).ToList();
			}

			if(files.Count == 0)
				return true;

		    try
		    {

                FtpClient ftp = RLibBase.FtpClient.Clone();
                await ftp.ConnectAsync();

                Stat = EVersionStat.Downloading;
                float initProcess = 100f - ((float)files.DataSize() / this.ZipSize.Value) * 100f;   // 初始已经下载完成的比例
                _ProgressAsync = initProcess;

                int ret = await ftp.DownloadFilesAsyncEx(AppDomain.CurrentDomain.BaseDirectory, RemoteDirectory, files, (x, y) =>
                    {
                        _DownloadSpeedAsync = x;   // 設置Async 更新值， todo: 如果以後有更好的方法
                        _ProgressAsync = initProcess + y * ((100f - initProcess) / 100f);
                    });

                Progress = 100;

                Stat = EVersionStat.Normal;

                await ftp.DisconnectAsync();

                return true;
            }
            catch (Exception e)   // 错误，中间断网等待
            {
                Stat = EVersionStat.Normal;

                _ProgressAsync = 100;
                Progress = 100;
                RLibBase.Logger.Error(e);
                return false;
            }

        }



        public async Task<bool>	UpdateTo(IVersion to)						// 更新到一个to，如果to没有，更新到versionManager的local
		{
			List<VersionFile> files = null;
			if (to != null)
				files = Files.Except(to.Files, new VersionFile()).ToList(); // 传入比较条件
			else
				files = Files.ToList();

			if(!await CheckStore(files))
				return false;

            /// 检查当前文件夹是否被占用，如果正在运行，要求用户关闭后再执行
		    string fileInUse = null;
		    string dir = Path.GetFileName(Environment.CurrentDirectory);
		    //while (DirectoryEx.IsInUsing(LocalDirectory, out fileInUse, null, new string[]{"Updater"}))
		    while (DirectoryEx.IsInUsing(LocalDirectory, out fileInUse, null, new string[]{dir}))
		    {
		        List<Process> lp = FileEX.WhoIsLocking(fileInUse);
		        string ps = null;
		        lp.Select(p => p.ProcessName).ForEach(p => ps += p + "\n");

		        MessageBoxResult res = await MessageBoxEx.ShowAsync("以下程序正在运行，请在关闭后点击'确定'以更新, 否则请点'取消'键!\n\n" + ps, "错误", MessageBoxButton.OKCancel);
		        if (res == MessageBoxResult.OK)
		        {
		        }
		        else
		        {
			        Stat = EVersionStat.NeedUpdat;
		            return false;
		        }
		    }



			Stat = EVersionStat.Updating;
			Progress = 0;

			/// 解压并替换
			int n = files.Count;
			int index = 1;
			foreach (VersionFile p in files)
			{
				string path = PathEx.CombineRelative(RemoteDirectory, p.File);

				try
				{
				    if (!p.IsDir)
				    {
				        await ZipHelper.DecomparessFileAsync(path + ".zip");
					    FileEX.Move(path, PathEx.CombineRelative(LocalDirectory , p.File));
				    }
				    else
				    {
				        await ZipHelper.DecomparessFileAsync(path + ".zip", path);
					    DirectoryEx.Move(path, PathEx.CombineRelative(LocalDirectory , p.File));
				    }
				}
				catch (Exception e)
				{
					RLibBase.Logger.Error("无法复制移动文件:" + p.File + " " + e);

				    await MessageBoxEx.ShowAsync(Name + "无法更新版本，请关闭相关占用的程序后再试!", "错误", MessageBoxButton.OK);

				    Stat = EVersionStat.NeedUpdat;
                    return false;
				}

				Progress = (float)index / (float)n *100;
				index += 1;
			}

			File.Copy(PathEx.CombineRelative(RemoteDirectory, "Version.dm"), PathEx.CombineRelative(LocalDirectory , "Version.dm"), true);

			Progress = 100;
			Stat = EVersionStat.Normal;

			return true;
		}




		protected void		_Update(object sender, float deltaSec)
		{
			if (Stat == EVersionStat.Downloading)
			{
				DownloadSpeed = (float)_DownloadSpeedAsync;
				Progress = (float) _ProgressAsync;
			}

		    if (ExeProcess == null)
		    {
		        DetectExistsExe();

		    }
		}

#region C&D
		public				Version(VersionDM dm)
			:base(dm.num)
		{
		    Number = System.Version.Parse(dm.num);

			_Name = dm.name;
			if (dm.desc.IsNullOrWhiteSpace())
				_Desc = dm.desc;
			_Time = new DateTime(dm.time);
			_Files = new List<VersionFile>();

			if (dm.size.HasValue)
				_Size = dm.size;
			if (dm.ZipSize.HasValue)
				_ZipSize = dm.ZipSize;


            // 也可能没有exe
		    //if (dm.exeSpecified)
		        ExeFile = dm.exe;

            // 其他需要检测的exe，可能没有
		    DetectExes = dm.detectExes;

			foreach (VersionFileDM vdm in dm.files)
			{
				_Files.Add(new VersionFile(vdm));
			}
		}
#endregion



#region Members
		protected string	_Name;
		protected string	_Desc;
		protected DateTime	_Time;
		protected long?		_Size;
		protected long?		_ZipSize;
		protected List<VersionFile> _Files;

		protected EVersionStat _Stat;

		protected float		_DownloadSpeed;
		protected float		_Progress;

		protected double	_ProgressAsync;
		public double		_DownloadSpeedAsync;

	    protected Process   _ExeProcess;
	    protected bool      _IsMainExeRunning;
#endregion
	}
}
