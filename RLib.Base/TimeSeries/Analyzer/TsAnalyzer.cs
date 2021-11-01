///********************************************************************
//    created:	2017/12/20 11:48:05
//    author:	rush
//    email:		
	
//    purpose:	
//*********************************************************************/
//using System;
//using System.Collections.Generic;
//using System.Collections.Specialized;
//using System.ComponentModel;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//
//using log4net.Repository.Hierarchy;

//namespace RLib.Base
//{
//    public class TsAnalyzer : ObservableObject, ITsAnalyzer, INotifyPropertyChanged
//    {

//#region 时间数据
//        public virtual int  TimeFrame { get { return Base.TimeFrame; }}                               // 对应的TimeFrame，用int通用一些

//        public DateTime     From { get { return Base.From; } }
//        public DateTime     To { get { return m_lDt[m_lDt.Count - 1]; } }

//        public int          Extent
//        {
//            get { return m_lDt.Count - Base.Count; }
//            protected set
//            {
//                if (Extent == value)
//                    return;

//                if (value > Extent)
//                {
//                    // todo
                    
//                }
//                else if (value < Extent)
//                {
                    
//                }

//            }
//        }                                 // 

//        public DateTime     Time(int index)
//        {
//            RDebug.EnsureArgRange(index, 0, m_lDt.Count-1);

//            return m_lDt[index];
//        }
//        public int          Index(DateTime time) // 找不到返回-1
//        {
//            int ret = -1;
//            // todo: 待优化
//            foreach (DateTime t in m_lDt)
//            {
//                ret++;
//                if (t == time)
//                {
//                    return ret;
//                }

//            }

//            return -1;
//        }

//        protected List<DateTime> m_lDt = new List<DateTime>(); 
//#endregion

//#region IInstance
//        public IReadonlyTimeSeries Base { get { return m_pBase; } }                            // 底层序列
//        public bool         IsLive { get { return m_bIsLive; } }             // 是否是活的数据，一个instance有可能加载的中间一段的历史数据，从而为死的数据

//        public bool         IsLoadingData
//        {
//            get { return m_bIsLoading; }
//            set { Set("IsLoadingData", ref m_bIsLoading, value); }
//        }  // 是否正在Loading 数据

//        public IReadonlyObservableDictionary<string, IStream> Streams { get { return m_pStreams; } }

//        #endregion


//#region Events
//        public event PropertyChangedEventHandler PropertyChanged;
//#endregion


//        #region Scripts
//        public IReadonlyObservableDictionary<ulong, ITsScript> Scripts { get { return m_pScripts; } }                   // 用户无法操作
//        public void         _UpdateScripts(int n)
//        {
//            foreach (KeyValuePair<ulong, ITsScript> kv in Scripts)
//            {
//                TsScript s = kv.Value as TsScript;
                
//                try
//                {
//                    //// 先将scripts里的stream设置为0
//                    //foreach (var stream in s.Streams.Items)
//                    //{
//                    //    stream[n] = 0;

//                    //}

//                    s.OnUpdate(n);
//                }
//                catch (Exception e)
//                {

//                }
//            }
//        }
//        public void         _RefreshScripts()
//        {
//            for (int i = 0; i != Base.Count; ++i)
//                _UpdateScripts(i);
//        }
     



//        protected void      __NotifyScriptStreamsChanged(object sender, NotifyCollectionChangedEventArgs e)
//        {
//            __CheckExtent();



//            //switch (e.Action)
//            //{
//            //    case NotifyCollectionChangedAction.Add:
//            //        foreach (IStream o in e.NewItems)
//            //        {
//            //            if (o.Extent > Extent)
//            //                Extent = o.Extent;
//            //        }
//            //        break;


//            //}

//        }


//        protected virtual void  __NotifyBaseTsChanged(object sender, NotifyCollectionChangedEventArgs e) // 基础ts改变
//        {
//            switch (e.Action)
//            {
//                case NotifyCollectionChangedAction.Add:
//                    {
//                        /// 更新script

//                        foreach (KeyValuePair<ulong, ITsScript> kv in Scripts)
//                        {
//                            TsScript s = kv.Value as TsScript;

//                            try
//                            {
//                                s.OnUpdate(Base.Count - 1);
//                            }
//                            catch (Exception )
//                            {

//                            }
//                        }

//                    }
//                    break;
//            }
            
//        }

//        protected void      __CheckExtent()
//        {
//            int maxExtent = 0;
//            foreach (var kv in Streams)
//            {
//                if (kv.Value.Extent > maxExtent)
//                    maxExtent = kv.Value.Extent;
//            }

//            foreach (KeyValuePair<ulong, ITsScript> kv in Scripts)
//            {
//                foreach (KeyValuePair<string, IReadonlyStream> kv2 in kv.Value.Streams)
//                {
//                    if (kv2.Value.Extent > maxExtent)
//                        maxExtent = kv2.Value.Extent;
//                }
//            }

//            Extent = maxExtent;
//        }

//#endregion

//#region OutputStream
//        //public IOutputStream AddStream(EStreamType type, int firstPeriod, int extent = 0)
//        //{
//        //    IOutputStream s = App.Instance.StreamManager.InstantiateAndAdd(this, type, firstPeriod, extent) as IOutputStream;
//        //    m_dOutputstreams.Add(s.ID, s);

//        //    return s;
//        //}

//        //public IOutputStream GetStream(ulong id)
//        //{
//        //    if (m_dOutputstreams.ContainsKey(id))
//        //        return m_dOutputstreams[id];
//        //    else
//        //        return null;
//        //}

//        //public void         _RemoveStream(ulong id)
//        //{
//        //    if (m_dOutputstreams.ContainsKey(id))
//        //        m_dOutputstreams.Remove(id);
//        //}

//        //public List<IOutputStream> Streams { get { return m_dOutputstreams.Values.ToList(); } }

//        //protected Dictionary<ulong, IOutputStream> m_dOutputstreams = new Dictionary<ulong, IOutputStream>(); 
//#endregion


//        protected void      __OnAddedTv(object sender, object e)
//        {

            
//        }

//        public void         RaisePropertyChanged(string propertyName)
//        {
//            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
//        }
//#region C&D
//        public              TsAnalyzer(IReadonlyTimeSeries s)
//        {
//            m_pBase = s;

//            m_pBase.CollectionChanged += __NotifyBaseTsChanged;



//            // 填充时间
//            int n = s.Count;
//            for(int i = 0; i < n; ++i)
//                m_lDt.Add(s.Time(i));



////                Scripts.CollectionChanged += __NotifyScriptsChanged;
//        }

//        public          TsAnalyzer()
//        {
            
//            //Scripts.CollectionChanged += __NotifyScriptsChanged;
//        }
       

//        ~TsAnalyzer()
//        {
//            m_pBase.CollectionChanged -= __NotifyBaseTsChanged;

////            Scripts.CollectionChanged -= __NotifyScriptsChanged;
//        }
//#endregion



//#region Members

//        protected IReadonlyTimeSeries m_pBase;

//        protected bool      m_bIsLoading;

//        protected bool      m_bIsLive = true;


//        /// loading 过程中的更新
////        protected List<EventArgs<ITick, ETimeFrame>> m_lCacheUpdates = new List<EventArgs<ITick, ETimeFrame>>();


//        protected RObservableDictionary<ulong, ITsScript> m_pScripts = new RObservableDictionary<ulong, ITsScript>();


//        protected RObservableDictionary<string, IStream> m_pStreams = new RObservableDictionary<string, IStream>();
//#endregion

//    }
//}
