///********************************************************************
//    created:	2017/11/13 17:36:52
//    author:	rush
//    email:		
	
//    purpose:	
//*********************************************************************/
//using System;
//using System.Collections.Generic;
//using System.ComponentModel;
//using System.Linq;
//using System.Text;
//using log4net;
//using System.Xml;

//namespace RLib.Base
//{
//    public partial class App
//    {
//        public static IApp   Instance
//        {
//            get { return s_pInstance; }
//        }

//        protected static IApp s_pInstance;
//    }
//    public interface IApp:IUpdate, IGottaInit, INotifyPropertyChanged
//    {
//        string              Name { get; set; }

//        Autofac.IContainer  Container { get; }

//        void                Run(float fps=0f);                              // 运行fps-帧率, 0代表事件驱动

//        IAppConfig          Config { get; }

//        ILog                DefaultLog { get; }

//        object              LastErrCode { get; set; }                       // 上一个错误码

//        DateTime            StartTime { get; }                              // 启动时间
//        TimeSpan            Duration { get; }                               // 运行时间

//        string              DataPath { get; set; }                          // 额外数据路径

//        void                AttachExceptionHandler();                       // 挂载异常处理
//        void                OnException(Exception ex, string extype = null);// 处理未捕获的异常

//        void                RunInMainThread(Action a);                      // 使Action在app主线程运行

//#region 多开检测
//        // 多开检测
//        bool                IsAllowMultiple { get; set; }                   // 是否允许多开
//        bool                CheckMultiple(int waitTime = 0);                // 检测多开, 等待多少时间

//        bool                CheckRunning(IEnumerable<string> exes, int waitTime = 0);// 检测是否有exe正在運行, 等待多少时间
        

//#endregion

//    }
//}
