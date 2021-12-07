/********************************************************************
    created:	2017/6/21 14:43:01
    author:		rush
    email:		
	
    purpose:	

*********************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataModel;
using Google.Protobuf;
using RLib.Base;

namespace RLib.Fin
{
    public class TimeValue:ITimeValue 
    {
#region Overrides
        //public virtual object  ToDm()                                          // 序列化成protobufdm
        //{
        //    throw new NotImplementedException();
        //}                                         

        public virtual int CompareTo(object obj)                            // 比较时间，而不是值
        {
            return (int)(Time.Ticks - (obj as IReadonlyTimeValue).Time.Ticks);
        }
#endregion

#region ITimeValue
        public DateTime     Time { get; set; }


        //object              IReadonlyTimeValue.Value
        //{
        //    get { return m_pValue; }
        //}
        //object              ITimeValue.Value
        //{
        //    get { return m_pValue; }
        //    set { Value = (T) value; }
        //}
        //public T            Value
        //{
        //    get { return m_pValue; }
        //    set
        //    {
        //        if (value == null ||
        //            !MathEx.Equal(m_pValue, value)
        //            )
        //        {
        //            m_pValue = value;
        //        }
        //    }
        //}
#endregion


#region c&D
        public              TimeValue(DateTime dt)
        {
            Time = dt;
        }
        //public              TimeValue(IMessage dm)
        //{
        //}
        public              TimeValue() { }
#endregion

#region Members
//        protected DateTime  m_dt;
#endregion
    }

    public class    ObjTv : TimeValue
    {
        public object       Value { get; protected set; }

#region C&D
        public ObjTv(DateTime dt, object val)
            : base(dt)
        {
            Value = val;
        }
#endregion
    }


//    public class    ObjTv<T> : ObjTv
//    {
//        public new T        Value { get { return (T)base.Value; } protected set { base.Value = value; } }

//#region C&D
//        public ObjTv(DateTime dt, T val)
//            : base(dt, val)
//        {
//        }
//#endregion
//    }


//    //    public class FloatTv : TimeValue<float>
//    //    {
//    //        public override IExtensible  ToDm()                                          // 序列化成protobufdm
//    //        {
//    //            return new FloatTvDM()
//    //            {
//    //                time = Time.Ticks,
//    //                value = Value
//    //            };
//    //        }                                         

//    //#region C&D
//    //        public              FloatTv(DateTime dt, float val)
//    //            :base(dt, val)
//    //        {
//    //        }
//    //        public              FloatTv(FloatTvDM dm)
//    //            :base(dm)
//    //        {
//    //            m_dt = new DateTime(dm.time);
//    //            m_pValue = dm.value;
//    //        }
//    //#endregion
//    //    }

//public class DoubleTvDM
//{
//    public long  time { get; set; }
//    public double value { get; set; }
//}

//    public class DoubleTv : ObjTv<double>
//    {
//        public override object ToDm()                                          // 序列化成protobufdm
//        {
//            return new DoubleTvDM()
//            {
//                time = Time.Ticks,
//                value = Value
//            };
//        }

//#region C&D
//        public DoubleTv(DateTime dt, double val)
//            : base(dt, val)
//        {
//        }
//        //public DoubleTv(DoubleTvDM dm)
//        //    : base(dm)
//        //{
//        //    m_dt = new DateTime(dm.time);
//        //    m_pValue = dm.value;
//        //}
//#endregion
//    }

//    public class StringTv : ObjTv<string>
//    {
//        public override object ToDm()                                          // 序列化成protobufdm
//        {
//            throw new NotImplementedException();

//            //return new DoubleTvDM()
//            //{
//            //    time = Time.Ticks,
//            //    value = Value
//            //};
//        }

//#region C&D
//        public StringTv(DateTime dt, string val)
//            : base(dt, val)
//        {
//        }
//        //public DoubleTv(DoubleTvDM dm)
//        //    : base(dm)
//        //{
//        //    m_dt = new DateTime(dm.time);
//        //    m_pValue = dm.value;
//        //}
//#endregion
//    }

    //    public class OhlcBarTv : TimeValue<OhlcBar>
    //    {
    //        public override IExtensible  ToDm()                                          // 序列化成protobufdm
    //        {
    //            return new OhlcBarTvDM()
    //            {
    //                time = Time.Ticks,
    //                bar = ((OhlcBar)m_pValue).ToDm() as OhlcBarDM,
    //            };
    //        }                                         


    //#region C&D
    //        public              OhlcBarTv(DateTime dt, OhlcBar val)
    //            :base(dt, val)
    //        {
    //        }
    //        public              OhlcBarTv(OhlcBarTvDM dm)
    //            :base(dm)
    //        {
    //            m_dt = new DateTime(dm.time);
    //            m_pValue = new OhlcBar(dm.bar);
    //        }
    //        public              OhlcBarTv()
    //        { }
    //#endregion

    //        protected ulong     m_nVol;
    //    }

}
