///********************************************************************
//    created:	2018/1/26 19:33:37
//    author:	rush
//    email:		
	
//    purpose:	
//*********************************************************************/
//using System;
//using System.Collections.Generic;
//using System.Collections.Specialized;
//using System.Diagnostics;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using DataModel;

//namespace RLib.Base
//{
//    public interface IBarStream : IStream<IBar>
//    {
//        FloatStream Open { get; }
//        FloatStream High { get; }
//        FloatStream Low { get; }
//        FloatStream Close { get;  }

//        FloatStream Volume { get;  }
//    }

//    public class BarStream : Stream<IBar>, IBarStream
//    {

//        #region Overrides
//        //public new void Add(OcBar item)
//        //{
//        //    m_pOpen.Add(item.Open);
//        //    m_pClose.Add(item.Close);

//        //    base.Add(item);
//        //}

//        //public new void Insert(int index, OcBar item)
//        //{
//        //    m_pOpen.Insert(index, item.Open);
//        //    m_pClose.Insert(index, item.Close);


//        //    base.Insert(index, item);
//        //}

//        public override IBar Min(int from, int to)                           // 获得一个各方面都最小的bar
//        {
//            Debug.Assert(from <= to &&
//                from >= 0 && from < Count &&
//                to >= 0 && to < Count);

//            Bar ret = new Bar();
//            ret.Open = Open.Min(from, to);
//            ret.High = High.Min(from, to);
//            ret.Low = Low.Min(from, to);
//            ret.Close = Close.Min(from, to);

//            return ret;
//        }

//        public override IBar Max(int from, int to)
//        {
//            Debug.Assert(from <= to &&
//                from >= 0 && from < Count &&
//                to >= 0 && to < Count);

//            Bar ret = new Bar();
//            ret.Open = Open.Max(from, to);
//            ret.High = High.Max(from, to);
//            ret.Low = Low.Max(from, to);
//            ret.Close = Close.Max(from, to);

//            return ret;
//        }

//        public override IBar Avg(int from, int to)
//        {
//            Debug.Assert(from <= to &&
//                from >= 0 && from < Count &&
//                to >= 0 && to < Count);

//            Bar ret = new Bar();
//            ret.Open = Open.Avg(from, to);
//            ret.High = High.Avg(from, to);
//            ret.Low = Low.Avg(from, to);
//            ret.Close = Close.Avg(from, to);

//            return ret;
//        }


//        public override bool IsSuppurtVolume { get { return true; } }                        // 是否支持Volume 数据

//#endregion


//        public override EStreamType  Type { get { return EStreamType.BarStream;} }

//#region
//        public FloatStream  Open { get { return m_pOpen; } }
//        public FloatStream  High { get { return m_pHigh; } }
//        public FloatStream  Low { get { return m_pLow; } }
//        public FloatStream  Close { get { return m_pClose; }  }

//        public FloatStream  Volume { get { return m_pClose; }  }
//#endregion


//        protected void      __NotifyCollectionChangedEventHandler(object sender, NotifyCollectionChangedEventArgs e)
//        {
//            //todo:
//            switch (e.Action)
//            {
//                case NotifyCollectionChangedAction.Add:
//                {
//                    int i = e.NewStartingIndex;
//                    foreach (IBar v in e.NewItems)
//                    {
//                            m_pOpen.Insert(i, v.Open);
//                            m_pHigh.Insert(i, v.High);
//                            m_pLow.Insert(i, v.Low);
//                            m_pClose.Insert(i, v.Close);

//                            m_pVolume.Insert(i, v.Volume);

//                        i++;
//                    }
                    
//                }
//                    break;
//                case NotifyCollectionChangedAction.Remove:
//                    break;
//                case NotifyCollectionChangedAction.Replace:
//                    break;
//                case NotifyCollectionChangedAction.Move:
//                    break;
//                case NotifyCollectionChangedAction.Reset:
//                    break;
//            }
//        }

//#region C&D
//        public              BarStream(ITsAnalyzer ana, string name, 
//            int first = 0,
//            EStreamShapeType shape = EStreamShapeType.Line, int extent = 0, bool visible = true, RColor c = new RColor(), ELineStyle style = ELineStyle.LINE_SOLID, int width = 1)
//            :base(ana, name, first,
//            shape, extent, visible, c, style, width
//            )
//        {
//            m_pOpen = new FloatStream(ana, name + "_Open", first, EStreamShapeType.Line, extent, false);
//            m_pHigh = new FloatStream(ana, name + "_High", first, EStreamShapeType.Line, extent, false);
//            m_pLow = new FloatStream(ana, name + "_Low", first, EStreamShapeType.Line, extent, false);
//            m_pClose = new FloatStream(ana, name + "_Close", first, EStreamShapeType.Line, extent, false);
//            m_pVolume = new FloatStream(ana, name + "_Volume", first, EStreamShapeType.Bar, extent, false);

//            CollectionChanged += __NotifyCollectionChangedEventHandler;
//        }
//        ~BarStream()

//        {
//            CollectionChanged -= __NotifyCollectionChangedEventHandler;

//        }
//#endregion

//#region Members
//        protected FloatStream m_pOpen;
//        protected FloatStream m_pHigh;
//        protected FloatStream m_pLow;
//        protected FloatStream m_pClose;
//        protected FloatStream m_pVolume;
//#endregion
//    }
//}
