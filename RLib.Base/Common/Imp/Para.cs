/********************************************************************
    created:	2018/6/13 17:44:42
    author:	rush
    email:		
	
    purpose:	
*********************************************************************/
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using DataModel;
using Google.Protobuf;
using NPOI.SS.Util;

namespace RLib.Base
{
    public partial class Para:ObservableObject, IPara
    {
        public override string ToString()
        {
            return $"{PropertyName}:{Value}";
        }

#region ISerializable
        public virtual IMessage ToDm()
        {
            if (Value == null)
                return null;

            KVDM ret = new KVDM() { Key = PropertyName, Value = new VarDM(Value) };
            //if (NestingParas != null)
            //    ret.Nestings.AddRange(NestingParas.ToDm());

            return ret;
        } // 序列化成protobufdm
#endregion

        public object           Host { get; protected set; }
        public PropertyInfo     PI { get; protected set; }

        public string           PropertyName => PI.Name;

        public ParaAttribute    Attribute => PI.GetCustomAttribute<ParaAttribute>() as ParaAttribute;

        public Type             PropertyType => PI.PropertyType;

//        public Func<object,string> ValueFormater { get;set; }               // value 的formater 

        public void         Reset(bool def = false)
        {
            //if (NestingParas!=null && NestingParas.Any())
            //{
            //    foreach (IPara nestingPara in NestingParas)
            //    {
            //        nestingPara.Reset(def);
            //    }
            //}
            //else
            {
                if (def)
                    Value = Attribute.Default;
                else
                    Value = StartValue;
            } // 设为初始值
        }

        public object       Value
        {
            get
            {
                return PI.GetValue(Host);
            }
            set
            {
                if (value.EqualEx(Value))
                    return;

                {
                    // 由于c#不支持隐式转换，比如string在赋值给RColor的时候，无法作用
                    // 这里需要针对这种情况先处理下
                    //if (ValueType.Name == typeof(RColor).Name)
                    //{
                    //    RColor c = RColor.FromObject(value);

                    //    m_pPI.SetValue(m_pHost, c);
                    //}
                    //else
                    {
                        try
                        {
                            PI.SetValue(Host, value);
                        }
                        catch (Exception e)
                        {
                            PI.SetValue(Host,  Convert.ChangeType(value, PropertyType));
                        }

                    }
                }


                if (value.EqualEx(StartValue))
                    ValueChanged = false;
                else
                    ValueChanged = true;
                RaisePropertyChanged("Value");
            }
        } // 最终值

        public bool         ValueChanged
        {
            get { return ValueChanged_; }
            protected set { Set(nameof(ValueChanged), ref ValueChanged_, value); }
        } // 相对一开始是否有变化

        public object       StartValue { get; protected set; }              // 用于比较value是否change， 通常在para创建时设置，中间para应用后也可以更新


#region C&D
        public Para(object host, PropertyInfo pi)
        {
            host.EnsureNotNull();
            pi.EnsureNotNull();

            Host = host;
            PI = pi;

            StartValue = Value;
        }
#endregion


#region Members
        protected bool ValueChanged_;
    
#endregion
    }

    //public class StringParaAttribute :ParaAttribute 
    //{
    //    public StringParaAttribute(string desc = "", string group="",  params string[] presets)
    //    {
    //        m_pPara = new Para<string>("", desc, group, "", presets);
    //    }
    //}

    //public class EnumParaAttribute :ParaAttribute 
    //{
    //    public              EnumParaAttribute(string name, string desc = "", string group="", object value=null, params object[] presets)
    //    {
    //        m_pPara = new Para<object>(name, desc, group, value, presets);
    //    }
    //}

    //public class BooleanParaAttribute :ParaAttribute 
    //{
    //    public BooleanParaAttribute(string name, string desc = "", string group="", bool value=false, params bool[] presets)
    //    {
    //        m_pPara = new Para<bool>(name,  desc, group, value, presets);
    //    }
    //}

    //public class IntParaAttribute :ParaAttribute 
    //{
    //    public IntParaAttribute(string name, string desc = "", string group="", int value=0, params int[] presets)
    //    {
    //        m_pPara = new Para<int>(name,  desc, group, value, presets);
    //    }
    //}

    //public class FloatParaAttribute :ParaAttribute 
    //{
    //    public FloatParaAttribute(string name, string desc = "", string group="", float value=0, params float[] presets)
    //    {
    //        m_pPara = new Para<float>(name,  desc, group, value, presets);
    //    }
    //}

    //public class DoubleParaAttribute :ParaAttribute 
    //{
    //    public DoubleParaAttribute(string name, string desc = "", string group="", double value=0, params double[] presets)
    //    {
    //        m_pPara = new Para<double>(name,  desc, group, value, presets);
    //    }
    //}

    //public class ColorParaAttribute :ParaAttribute 
    //{
    //    public ColorParaAttribute(string name, string desc = "", string group="", RColor? value = null, params RColor[] presets)
    //    {
    //        if(value != null)
    //            m_pPara = new Para<RColor>(name,  desc, group, value.Value, presets);
    //        else
    //            m_pPara = new Para<RColor>(name,  desc, group, RColor.Empty, presets);
    //    }
    //}

    //public class RangeParaAttribute :ParaAttribute 
    //{
    //    public              RangeParaAttribute(string name, string desc = "", string group="", RangeDM value = null, params RangeDM[] presets)
    //    {
    //            m_pPara = new Para<RangeDM>(name,  desc, group, value, presets);
    //    }
    //}
}
