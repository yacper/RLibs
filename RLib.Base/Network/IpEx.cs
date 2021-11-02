using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DotEx.Common
{
	public static class IpEx
	{

		public static string LocalIP(out string mac)                        // 获取本地ip，以及mac
		{
			mac = null;
			string userIP = null;

			try
			{

                if (Environment.OSVersion.Platform == PlatformID.Win32NT)
                {// 需要windows依赖
                    //System.Net.NetworkInformation.NetworkInterface[] fNetworkInterfaces = System.Net.NetworkInformation.NetworkInterface.GetAllNetworkInterfaces();
                    //foreach (System.Net.NetworkInformation.NetworkInterface adapter in fNetworkInterfaces)
                    //{
                    //    string fRegistryKey = "SYSTEM\\CurrentControlSet\\Control\\Network\\{4D36E972-E325-11CE-BFC1-08002BE10318}\\" + adapter.Id + "\\Connection";
                    //    Microsoft.Win32.RegistryKey rk = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(fRegistryKey, false);
                    //    if (rk != null)
                    //    {
                    //        // 区分 PnpInstanceID      
                    //        // 如果前面有 PCI 就是本机的真实网卡          
                    //        string fPnpInstanceID = rk.GetValue("PnpInstanceID", "").ToString();
                    //        int fMediaSubType = Convert.ToInt32(rk.GetValue("MediaSubType", 0));
                    //        if (fPnpInstanceID.Length > 3 &&
                    //        fPnpInstanceID.Substring(0, 3) == "PCI")
                    //        {
                    //            //string fCardType = "物理网卡";
                    //            System.Net.NetworkInformation.IPInterfaceProperties fIPInterfaceProperties = adapter.GetIPProperties();
                    //            System.Net.NetworkInformation.UnicastIPAddressInformationCollection UnicastIPAddressInformationCollection = fIPInterfaceProperties.UnicastAddresses;
                    //            foreach (System.Net.NetworkInformation.UnicastIPAddressInformation UnicastIPAddressInformation in UnicastIPAddressInformationCollection)
                    //            {
                    //                if (UnicastIPAddressInformation.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                    //                {
                    //                    mac = adapter.GetPhysicalAddress().ToString();

                    //                    userIP = UnicastIPAddressInformation.Address.ToString(); // Ip 地址     
                    //                }
                    //            }
                    //            break;
                    //        }
                    //    }
                    //}

                }
            }
			catch (Exception e)
			{
				Console.WriteLine(e);
				throw;
			}

			return userIP;
		}

		public static string ExternalIP()                                   // 获取外网真实IP
		{
			string ip = null;

			try
			{
				// 调用百度搜索获取，尝试了不同的几个网站，结果相差巨大，貌似百度这个最正确一点
				HttpWebRequest request = HttpWebRequest.Create("https://www.baidu.com/s?wd=ip&rsv_spt=1&rsv_iqid=0xfb38fa8500024d85&issp=1&f=8&rsv_bp=1&rsv_idx=2&ie=utf-8&rqlang=cn&tn=baiduhome_pg&rsv_enter=1&oq=ip%255D&inputT=217&rsv_t=8e45h9eKVZWQcoz7nFtaavuvL71ockW5INirZz8FHiX55NlA91hxTEmCJ0KZze5IVqV5&rsv_pq=fec2a08900029935&rsv_sug3=10&rsv_sug1=4&rsv_sug7=100&rsv_sug2=0&rsv_sug4=705") as HttpWebRequest;
				//HttpWebRequest request = HttpWebRequest.Create("https://www.cnblogs.com/liuqiyun/p/6866158.html") as HttpWebRequest;

				request.UseDefaultCredentials = true;
				request.ContentType = "text/html";

				request.UserAgent = "Mozilla/4.0 (compatible; MSIE 8.0; Windows NT 5.0; .NET CLR 1.1.4322; .NET CLR 2.0.50215;)";
				request.Method = "GET";
				request.CookieContainer = new CookieContainer();
				request.Method = "GET";
				//request.ContentType = "application/x-www-form-urlencoded";
				//request.UserAgent = "Mozilla/5.0";
				WebResponse response = request.GetResponse();
				using (StreamReader reader = new StreamReader(response.GetResponseStream()))
				{
					string result = reader.ReadToEnd();

					// 找到本机ip:       		<span class="c-gap-right">本机IP:&nbsp;49.221.232.101</span>上海市上海市 长城宽带	    
					int index = result.IndexOf("本机IP");
					string line = result.Substring(index, 30);

					string pattern = @"\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}";
					ip = Regex.Match(line, pattern).ToString();
				}
			}
			catch (Exception e)
			{
			}

			return ip; // result: 210.125.21.xxx
		}
	}


}
