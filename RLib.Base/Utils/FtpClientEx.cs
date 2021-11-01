/********************************************************************
    created:	2018/8/10 14:01:42
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
using FluentFTP;

namespace RLib.Base
{




	public static class FtpClientEx
	{
		public static FtpClient Clone(this FtpClient client)				// clone 一个，防止互相影响
		{
			FtpClient ret = new FtpClient(client.Host, client.Port, client.Credentials);

			return ret;
		}



		class Progress : IProgress<FtpProgress>
		{
			public void			Report(FtpProgress p)						// cur downloadFile Progress
			{
			    double value = p.Progress;

				value /= 100;

				DateTime now = DateTime.Now;
				TimeSpan deltaSpan = now - _LastProgressTime;
				_LastProgressTime = now;



				double deltaProgress = value - _LastProgress;
				long deltaData = (long)(CurDownloadItem.Size * deltaProgress);
				_CompleteSize += deltaData;
				_LastProgress = value;


				_CountTimeSpan += deltaSpan;
				_CountDeltaSize += deltaData;

				if (_CountTimeSpan.Seconds >= 1)
				{
					double speed = _CountDeltaSize / _CountTimeSpan.Seconds;
					speed /= 1024;

					double progress = (double)_CompleteSize / (double)_AllSize;
					progress *= 100;


					_CountTimeSpan = TimeSpan.Zero;
					_CountDeltaSize = 0;

					if (_Action != null)
						_Action(speed, progress);
				}
			}

			public FtpListItem CurDownloadItem
			{
				get { return _CurDownloadItem; }
				set
				{
					_LastProgress = 0;
					_LastProgressTime = DateTime.Now;

					_CountTimeSpan = TimeSpan.Zero;
					_CountDeltaSize = 0;

					_CurDownloadItem = value;
				}
			}


			public Progress(Action<double, double> action, long allSize)
			{
				_Action = action;

				_AllSize = allSize;
			}


			protected double _LastProgress;
			protected DateTime _LastProgressTime;

			protected TimeSpan _CountTimeSpan;
			protected long _CountDeltaSize;

			protected long _AllSize;
			protected long _CompleteSize;
			protected Action<double, double> _Action;
			private FtpListItem _CurDownloadItem;
		}

		// Action<double, double>  progress, 第一个参数是下载速度kb/s， 第二个参数是完成%
		public static async Task<int>  DownloadDirectoryAsync(this FtpClient client, string dir, string remoteDir, Action<double, double> progress = null) // 下载整个目录中的所有文件，并且保持文件结构
        {
			if(!client.IsConnected)
				await client.ConnectAsync();

	        List<FtpListItem> items = await client.GetListingFilesAsync(remoteDir);

	        return await client.DownloadFilesAsyncEx(dir, items, progress);
        }

		// Action<double, double>  progress, 第一个参数是下载速度kb/s， 第二个参数是完成%
		public static async Task<int>  DownloadFilesAsyncEx(this FtpClient client, string dir, IEnumerable<string> files, Action<double, double> progress = null) // 下载整个目录中的所有文件，并且保持文件结构
        {
			if(!client.IsConnected)
				await client.ConnectAsync();

	        List<FtpListItem> items = await client.GetFileInfosAsync(files);

	        return await client.DownloadFilesAsyncEx(dir, items, progress);

        }

		//todo: version file
		//public static async Task<int>  DownloadFilesAsyncEx(this FtpClient client, string dir, string verDir, IEnumerable<VersionFile> files, Action<double, double> progress = null) // 针对VersionFile的一个版本
  //      {
		//	if(!client.IsConnected)
		//		await client.ConnectAsync();

		//	// 通过versionFile自己构建FtpListItem(相当于对本地置信，这样速度会快不少，不用从服务器获取fileInfo)
		//	List<FtpListItem> fl = new List<FtpListItem>();
	 //       foreach (VersionFile vf in files)
	 //       {
		//		FtpListItem f = new FtpListItem();
		//        f.FullName =  verDir + "\\" + vf.File + ".zip";
		//        f.Size = vf.ZipSize;
		//        f.Type = FtpFileSystemObjectType.File;
				
		//		fl.Add(f);
	 //       }

	 //       return await client.DownloadFilesAsyncEx(dir, fl, progress);
  //      }

		// Action<double, double>  progress, 第一个参数是下载速度kb/s， 第二个参数是完成%
		public static async Task<int>  DownloadFilesAsyncEx(this FtpClient client, string dir, IEnumerable<FtpListItem> files, Action<double, double> progress = null) // 下载整个目录中的所有文件，并且保持文件结构
        {
			// 默认是已连接的
			if(!client.IsConnected)
				await client.ConnectAsync();

	        int ret = 0;

	        if (!Directory.Exists(dir))
		        Directory.CreateDirectory(dir);
			

	        List<FtpListItem> items = files.ToList();

	        long all = items.DataSize();

            try
            {
                Progress p = new Progress(progress, all);
                foreach (FtpListItem item in items)
                {
                    p.CurDownloadItem = item;
                    string localPath = PathEx.CombineRelative(dir, item.FullName);
                    bool ok = await client.DownloadFileAsync(localPath, item.FullName, FtpLocalExists.Overwrite, FtpVerify.None, p);
                    if (!ok)
                        return ret;
                    else
                        ret++;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

	        //await client.DisconnectAsync();

			// 最后100%确定下
	        if (progress != null)
		        progress(0, 100);

	        return ret;
        }


		public static long	DataSize(this IEnumerable<FtpListItem> items) // 获取大小，如果是dir则
		{
			return items.Sum(p => p.Size);
		}

		public static async Task<List<FtpListItem>> GetFileInfosAsync(this FtpClient client, IEnumerable<string> files) // 返回递归目录后的所有file对象
		{
			List<FtpListItem> ret = new List<FtpListItem>();
			foreach (string f in files)
			{
				FtpListItem item = await client.GetObjectInfoAsync(f);

				ret.Add(item);
			}

			return ret;
		}

		public static async Task<List<FtpListItem>> GetListingFilesAsync(this FtpClient client, IEnumerable<FtpListItem> items) // 返回递归目录后的所有file对象
		{
			if (!client.IsConnected)
				await client.ConnectAsync();

			List<FtpListItem> ret = new List<FtpListItem>();
			ret.AddRange(items.Where(p=>p.Type == FtpFileSystemObjectType.File));


			IEnumerable<FtpListItem> dirs = items.Where(p => p.Type == FtpFileSystemObjectType.Directory);
			foreach(FtpListItem dir in dirs)
			{
				FtpListItem[] its = await client.GetListingAsync(dir.FullName);

				List<FtpListItem> r = await GetListingFilesAsync(client, its);

				ret.AddRange(r);
			}

			return ret;
		}

		public static async Task<List<FtpListItem>> GetListingFilesAsync(this FtpClient client, string remoteDir) // 返回递归目录后的所有file对象
		{
			if (!client.IsConnected)
				await client.ConnectAsync();

	        FtpListItem[] items = await client.GetListingAsync(remoteDir);

			return await client.GetListingFilesAsync(items);
		}

	}
}
