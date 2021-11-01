///********************************************************************
//    created:	2017/3/26 0:10:23
//    author:		rush
//    email:		
	
//    purpose:	
//*********************************************************************/
//using System;
//using System.Collections.Generic;
//using System.Collections.Specialized;
//using System.Drawing;
//using System.Linq;
//using System.Text;
//using RLib.Base;

//namespace RLib.Base
//{
//    public class TsScript:DynamicProduct<string>, ITsScript 
//    {
//        public override string ToString()
//        {
//            StringBuilder sb = new StringBuilder();
//            sb.Append(Name);

//            if (Streams.Count == 0)
//                return sb.ToString();


//            sb.Append(":");

//            // 单个stream，直接显示，不显示stream名
//            if (Streams.Count == 1)
//            {
//                sb.Append(Streams.First().Value.Back().ToString());
//            }
//            else
//            {
//                foreach (KeyValuePair<string, IReadonlyStream> kv in Streams)
//                {
//                    sb.Append(" ");

//                    sb.Append(kv.Value.Label);
//                    sb.Append(":");
//                    sb.Append(kv.Value.Back().ToString());
//                }
//            }

//            return sb.ToString();
//        }

//        public string       ToString(int period)
//        {
//            StringBuilder sb = new StringBuilder();
//            sb.Append(Name);

//            if (Streams.Count == 0)
//                return sb.ToString();


//            sb.Append(":");

//            // 单个stream，直接显示，不显示stream名
//            if (Streams.Count(p=>p.Value.IsVisible) == 1)
//            {
//                sb.Append(Streams.First(p=>p.Value.IsVisible).Value[period].ToString());
//            }
//            else
//            {
//                foreach (KeyValuePair<string, IReadonlyStream> kv in Streams)
//                {
//                    if(!kv.Value.IsVisible)
//                        continue;

//                    sb.Append(" ");

//                    sb.Append(kv.Value.Label);
//                    sb.Append(":");
//                    sb.Append(kv.Value[period].ToString());
//                }
//            }

//            return sb.ToString();
//        }

//        public override void UnInit()
//        {
//            base.UnInit();

//            if (Source != null)
//            {
//                _Source.CollectionChanged -= __NotifyCollectionChangedEventHandler;
//                Source = null;
//            }
//        }

//        public string       GenID                                           // 这个GENID可以生成一个tsScript
//        {
//            get
//            {
//                // MVA(XAU/USD.W1.Close,7)

//                //profile.id() + "(" + instance.source.name() + "," + n + ")";

//                throw new NotImplementedException();
//            }
//        } 


//        #region IScript
//        public virtual string        Name { get{throw new NotImplementedException(); } }
//        public virtual string        Group
//        {
//            get { return "Others"; }
//        }
//        public virtual ESourceType   RequiredSource
//        {
//            get { return ESourceType.Tick; }
//        }                    // source 类型

//        public virtual ETsScriptType ScriptType { get { throw new NotImplementedException();} }

////        public string Label { get; protected set; }

//        public ITsAnalyzer  Host { get { return m_pHost; } set { m_pHost = value; }}

//        public IStream      Source
//        {
//            get
//            {
//                return _Source;
//            }
//            set
//            {
//                _Source = value;
//                _Source.CollectionChanged += __NotifyCollectionChangedEventHandler;
//            }
//        }


//        public IReadonlyObservableDictionary<string, IReadonlyStream> Streams { get { return m_pStreams; } }

//        public IStream      AddStream(string id, EStreamType type, EStreamShapeType shape, string label, RColor color, int firstPeriod = 0, int extent = 0)
//        {
//            IStream ret = null;

//            switch (type)
//            {
//                case EStreamType.FloatStream:
//                    ret = new FloatStream(Host, id, firstPeriod, shape, extent, true, color);
//                    break;

//                case EStreamType.BarStream:
//                    ret = new BarStream(Host, id, firstPeriod, shape, extent, true, color);
//                    break;
                    
//            }
            
//            m_pStreams.Add(ret.ID, ret);

//            return ret;
//        }

//        public IStream      AddInternalStream(string id, EStreamType type = EStreamType.FloatStream, int firstPeriod = 0, int extent = 0)
//        {
//            IStream ret = null;

//            switch (type)
//            {
//                case EStreamType.FloatStream:
//                    ret = new FloatStream(Host, id, firstPeriod, EStreamShapeType.Line, extent, false);
//                    break;

//                case EStreamType.BarStream:
//                    ret = new BarStream(Host, id, firstPeriod,  EStreamShapeType.Bar, extent, false);
//                    break;
                    
//            }
            
//            m_pStreams.Add(ret.ID, ret);

//            return ret;
//        }


//        public void         _Update(int period)                            // 系統更新
//        {
//            //// 默认设置stream
//            //foreach (KeyValuePair<string, IReadonlyStream> kv in m_pStreams)
//            //{
//            //    if(kv.Value is FloatStream)
//            //        (kv.Value as FloatStream)[period] = 0;
//            //    else if (kv.Value is BarStream)
//            //        (kv.Value as BarStream)[period] = default(Bar);
//            //}
            
//            OnUpdate(period);
//        }

     
//        public virtual void OnUpdate(int period)
//        {
            
//        }
     

//#endregion

//        #region Protected
//        protected void      __NotifyCollectionChangedEventHandler(object sender, NotifyCollectionChangedEventArgs e)
//        {
//            switch (e.Action)
//            {
//                case NotifyCollectionChangedAction.Add:
//                    _Update(Source.Count-Source.Extent-1);
//                    break;
//                case NotifyCollectionChangedAction.Remove:
//                    break;
//                case NotifyCollectionChangedAction.Replace:
//                    _Update(Source.Count-Source.Extent-1);
//                    break;
//                case NotifyCollectionChangedAction.Reset:
//                    //todo: 重置所有
//                    break;
//            }
//        }

//#endregion

//#region C&D
//        public              TsScript()                                      // 后面补设
//            : base(_GenerateID())
//        {
            
//            //Label = this.GetType().Name;  // 默认直接类型名
//        }

//        protected static string _GenerateID()
//        {
//            throw new NotImplementedException();

//        }
//#endregion

//#region Members
//        protected ITsAnalyzer m_pHost;
//        protected static ulong s_nID;

//        protected string    m_strLabel;

//        protected IStream   _Source;                                      // 源

//        protected RObservableDictionary<string, IReadonlyStream> m_pStreams = new RObservableDictionary<string, IReadonlyStream>();
//#endregion
//    }

//}
