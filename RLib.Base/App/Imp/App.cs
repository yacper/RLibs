/********************************************************************
    created:	2017/11/13 17:37:52
    author:	rush
    email:		
	
    purpose:	
*********************************************************************/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Xml;

using log4net;
using log4net.Repository.Hierarchy;

namespace RLib.Base
{
    public partial class App:GottaInit, IApp
    {
#region App

#region IUpdate
        public virtual void OnUpdate(float deltaTime)
        {
            RaisePropertyChanged("Duration");
            Timer.Update();
        }

        protected void      __FireUpdate(float deltaTime)
        {
            if (OnUpdateEvent != null)
            {
                var e = OnUpdateEvent;
                e(this, deltaTime);
            }
        }


        public event EventHandler<float> OnUpdateEvent;
#endregion
        public override void Init()                                         // 初始化
        {
            base.Init();
            s_pInstance = this;
        }

        public virtual void _UnInit()                                       // 反初始化
        {
            
        }

        public string       Name { get; set; }

        // Unity Container
        // container.RegisterType<IT, T>();
        // var window = container.Resolve<IT>(new ResolverOverride[]
        //                        {
        //                             new ParameterOverride("a", 21)
        //                        });
        // window.Print();

        public Autofac.IContainer     Container { get; protected set; }


        public virtual void Run(float fps) // 运行fps
        {
            if (fps >= 0)
            {
                m_bLimitFrame = true;
                m_nFrameDuration = 1000 / fps;
            }
	        _LastFrameTick = DateTime.Now.Ticks;

            while (true)
            {
                BeginFrame();

	            long now = DateTime.Now.Ticks;

				float deltaMilSeconds = new TimeSpan(now - _LastFrameTick).Milliseconds;

	            _LastFrameTick = now;

                OnUpdate(deltaMilSeconds);

                EndFrame();
            }
        }

        public virtual void BeginFrame()
        {
            m_pFrameWatch.Reset();
            m_pFrameWatch.Start();

        }

        public virtual void EndFrame()
        {
            m_pFrameWatch.Stop();
            if (m_pFrameWatch.ElapsedMilliseconds < m_nFrameDuration)
            {
                Thread.Sleep((int)(m_nFrameDuration - m_pFrameWatch.ElapsedMilliseconds));
                // 找个timer
            }
        }

        #endregion

#region Propertychanged
        public event PropertyChangedEventHandler PropertyChanged;

        public virtual void RaisePropertyChanged(string propertyName) 
        {
            var handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        protected bool      Set<T>(string propertyName, ref T field, T newValue)
        {
            if (EqualityComparer<T>.Default.Equals(field, newValue))
            {
                return false;
            }

            field = newValue;

            // ReSharper disable ExplicitCallerInfoArgument
            RaisePropertyChanged(propertyName);
            // ReSharper restore ExplicitCallerInfoArgument
            
            return true;
        }
#endregion




        public IAppConfig   Config { get { return m_pAppConfig; } }

        public ILog         DefaultLog { get { return m_pDefautLog; } }


	    public object		LastErrCode
	    {
		    get { return _LastErrCode; }
		    set
		    {
			    _LastErrCode = value;
			    DefaultLog.Error("设置错误码：" + value);
		    }
	    } // 上一个错误码



        public DateTime     StartTime { get; protected set; }                              // 启动时间
        public TimeSpan     Duration { get { return DateTime.Now - StartTime; } }                               // 运行时间


        public string       DataPath { get; set; }                          // 额外数据路径


        public void         AttachExceptionHandler() // 挂载异常处理
        {
            //// 未处理的异常
            ////UI线程未捕获异常处理事件（UI主线程）
            //Application.Current.DispatcherUnhandledException += App_DispatcherUnhandledException;
            ////非UI线程未捕获异常处理事件(例如自己创建的一个子线程)
            //AppDomain.CurrentDomain.UnhandledException += __CurrentDomain_UnhandledException;
            ////Task线程内未捕获异常处理事件
            //TaskScheduler.UnobservedTaskException += __TaskScheduler_UnobservedTaskException;
        }
        //protected void          App_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        //{
        //    RLibBase.Logger.Error("程序异常:" + Environment.NewLine + e.Exception.Message);
        //    e.Handled = true;

        //    OnException(e.Exception, "UI线程异常");
        //}

        protected bool      _OnException;                                   // 发生了错误

        //非UI线程未捕获异常处理事件(例如自己创建的一个子线程)
        //如果UI线程异常DispatcherUnhandledException未注册，则如果发生了UI线程未处理异常也会触发此异常事件
        //此机制的异常捕获后应用程序会直接终止。没有像DispatcherUnhandledException事件中的Handler=true的处理方式，可以通过比如Dispatcher.Invoke将子线程异常丢在UI主线程异常处理机制中处理
        private void        __CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Exception ex = e.ExceptionObject as Exception;
            OnException(ex, "非UI线程异常");
        }
 
        //Task线程内未捕获异常处理事件
        private void        __TaskScheduler_UnobservedTaskException(object sender, UnobservedTaskExceptionEventArgs e)
        {
            Exception ex = e.Exception;
            OnException(ex, "Task线程异常");
        }

        public virtual void OnException(Exception ex, string extype = null) // 处理未捕获的异常
        {
            string errorMsg = extype + ":";
            if (ex.InnerException != null)
            {
                errorMsg += String.Format("【InnerException】{0}\n{1}\n", ex.InnerException.Message, ex.InnerException.StackTrace);
            }
            errorMsg += String.Format("{0}\n{1}", ex.Message, ex.StackTrace);

            // 重启+1
            RLibBase.Logger.Error(errorMsg);//自己封装的日志管理
        }

        public virtual void RunInMainThread(Action a) // 使Action在app主线程运行
        {
            throw new NotImplementedException();
        }


#region 多开检测
        public bool         IsAllowMultiple { get; set; }                   // 是否允许多开
        public bool         CheckMultiple(int waitTime = 0) // 检测多开
        {
            string err = null;
            bool bMultple = false;

            if (AppEx.IsExeMultiple(Name))
            {
                bMultple = true;
                err = $"{Name}已经运行";
            }
            //else if (RLib.Base.AppEx.IsExeOpen("StockEvUpdater"))
            //{
            //    bMultple = true;
            //    err = "鲨鱼量化更新器正在运行";
            //}

            if (!bMultple) // 正常
            {
                return false;
            }
            else
            {
                if (waitTime != 0)
                {
                    int waitGap = 1000;
                    int waitTimes = waitTime / waitGap;            // 检测次数

                    for(int i = 0; i!= waitTimes; i++)
                    {
                        System.Threading.Thread.Sleep(waitGap); // 每隔1s检查一次

                        if(!CheckMultiple())
                            return false;
                    }
                }

                return true;
            }
        }

        public bool         CheckRunning(IEnumerable<string> exes, int waitTime = 0) // 检测是否有exe正在運行, 等待多少时间
        {
            string err = null;
            bool bRunning = false;

            foreach (string ex in exes)
            {
                if (AppEx.IsExeRunnging(ex))
                {
                    bRunning = true;
                    err = $"{ex}正在运行";
                }
            }


            if (!bRunning) // 正常
            {
                return false;
            }
            else
            {
                if (waitTime != 0)
                {
                    int waitGap = 1000;
                    int waitTimes = waitTime / waitGap;            // 检测次数

                    for(int i = 0; i!= waitTimes; i++)
                    {
                        System.Threading.Thread.Sleep(waitGap); // 每隔1s检查一次

                        if(!CheckRunning(exes))
                            return false;
                    }
                }

                return true;
            }
        }

#endregion



#region C&D
        public              App()
	    {
            StartTime = DateTime.Now;

            if(Assembly.GetEntryAssembly() != null)
	            Name = Assembly.GetEntryAssembly().GetName().Name;  // 默认名字




	        //string id = "123";
	        string id = AppDomain.CurrentDomain.FriendlyName;
	        id = PathEx.TrimExtension(id);

	        DataPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\" + id;

	        if (!Directory.Exists(DataPath))
	            Directory.CreateDirectory(DataPath);


	    }
        #endregion


        #region  Members
        protected ILog      m_pDefautLog;
        protected IAppConfig m_pAppConfig;
     //   protected object     m_pFpsTimer;

	    protected long	    _LastFrameTick;
		

        protected Stopwatch m_pFrameWatch = new Stopwatch();
        protected float     m_nFrameDuration;

        protected bool      m_bLimitFrame;

	    protected object    _LastErrCode = 0;
#endregion
    }
}
