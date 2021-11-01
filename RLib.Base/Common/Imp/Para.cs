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
            return $"{OriginalName}:{Value}";
        }


#region ISerializable
        public virtual IMessage ToDm()
        {
            if (Value == null)
                return null;

            KVDM ret = new KVDM() { Key = OriginalName, Value = new VarDM(Value) };
            if (NestingParas != null)
                ret.Nestings.AddRange(NestingParas.ToDm());

            return ret;
        } // 序列化成protobufdm
#endregion


        public object         Host
        {
            get { return m_pHost; }
        }

        public virtual string Name { get ; set;}

        public string OriginalName { get; protected set; }                           // 原始名称
        public virtual string Group { get; set; }
        public IEnumerable<string> GroupEx { get; set; }
        public virtual string Desc { get; set; }
        //public Type         ValueType { get { return m_pObj!=null?m_pObj.GetType():m_pPI.PropertyType; } }
        public virtual Type ValueType
        {
            get { return m_pValueType; }
            set
            {
                if (m_pValueType == value)
                    return;

                m_pValueType = value;

                if (RReflector.IsNullableType(value))
                {
                    Nullable = true;
                    m_pValueType = System.Nullable.GetUnderlyingType(value);
                }
                //else if(value.Name == "String")
                //    Nullable = true;
            }
        }

        public bool         HideInParaGrid { get; set; }                    // 不在ParaGrid中编辑

        IEnumerable<object> IPara.Presets { get { return m_lPresets; }
            set { m_lPresets = value.ToList(); }
        }                                // 预设项
        public object       DefValue { get { return m_pDefVal; }
            set { m_pDefVal = value; }
        }
        public object       Min
        {
            get { return m_pMinVal;}
            set
            {
                if (RMath.Equals(m_pMinVal, value))
                    return;
                    m_pMinVal = value;
                    RaisePropertyChanged("Min");
            } }
        public object Max
        {
            get { return m_pMaxVal; }
            set
            {
                if (RMath.Equals(m_pMaxVal, value))
                    return;
                m_pMaxVal = value;
                RaisePropertyChanged("Max");
            }
        }
        public object       Step { get;  set; }

        public object       RangeMin
        {
            get
            {
                if (ExArgs == null)
                    return null;

                object o;
                if (ExArgs.TryGetValue("RangeMin", out o))
                    return o;
                return null;
            }
            set
            {
                object rmin = RangeMin;
                if (value == rmin)
                    return;

                _CheckExArags();

                if (rmin == null)
                    ExArgs.Add("RangeMin", value);
                else
                    ExArgs["RangeMin"] = value;

                ValueChanged = true;        // todo: 目前简单处理
                RaisePropertyChanged("Value");
            }
        }

        public bool         IsNumRange { get; set; }                        // 是否作为数字Range( 不同于UseRange)

        public object       RangeMax
        {
           get
            {
                if (ExArgs == null)
                    return null;

                object o;
                if (ExArgs.TryGetValue("RangeMax", out o))
                    return o;
                return null;
            }
            set
            {
                object rmin = RangeMax;
                if (value == rmin)
                    return;

                _CheckExArags();

                if (rmin == null)
                    ExArgs.Add("RangeMax", value);
                else
                    ExArgs["RangeMax"] = value;

                ValueChanged = true;        // todo: 目前简单处理
                RaisePropertyChanged("Value");
            }
        }
        public object       RangeStep
        {
            get
            {
                if (ExArgs == null)
                    return null;

                object o;
                if (ExArgs.TryGetValue("RangeStep", out o))
                    return o;
                return null;
            }
            set
            {
                object rmin = RangeStep;
                if (value == rmin)
                    return;

                _CheckExArags();

                if (rmin == null)
                    ExArgs.Add("RangeStep", value);
                else
                    ExArgs["RangeStep"] = value;

                ValueChanged = true;        // todo: 目前简单处理
                RaisePropertyChanged("Value");
            }

        }

        public bool         Multi { get;  set; }                                  // 多选-- 默认false
        public int          Row { get;  set; }                                    // 设定多少行
        public int          Col { get;  set; }                                    // 设定多少列

		public bool			PresetsUseEnum { get; set; }					// 当valueType是enum，不设Presets的时候，取Enum的值作为preset
        public bool         Nullable { get; protected set; }                               // 是否可以设置为NUll
        public string       NullPresentation { get; set; }                       // 代表NUll时的string表示
        public string		NullOperator { get; set; }						// Null 操作表示

        public Func<object,string> ValueFormater { get;set; }               // value 的formater 

        public bool                UseRange { get; }                               // 使用Range

        public bool                IsFile { get; set; }                                 // value string 作为file
        public bool                IsDir { get; set; }                                  //  value string 作为dir



        public Dictionary<string, object> ExArgs { get;set; }                      // 额外参数 

        protected void      _CheckExArags()
        {
            if(ExArgs == null)
                ExArgs = new Dictionary<string, object>();
        }

        public Dictionary<string, object> CompoundArgs { get;set; }                // para Property 内部的参数

        public List<IPara>         NestingParas { get; set; }

        public PropertyInfo        PI
        {
            get { return m_pPI; }
        }

        public void Reset(bool def = false)
        {
            if (NestingParas!=null && NestingParas.Any())
            {
                foreach (IPara nestingPara in NestingParas)
                {
                    nestingPara.Reset(def);
                }
            }
            else
            {
                if (def)
                    Value = m_pDefVal;
                else
                    Value = _StartVal;
            } // 设为初始值
        }




        public List<object> Presets { get { return m_lPresets; } set { m_lPresets = value; } }                                // 预设项

        public object       Value
        {
            get
            {
                if (m_pPI != null)
                {
                    if (m_pHost != null)
                        return m_pPI.GetValue(m_pHost);
                    else
                        return DefValue;  // 这种情况主要是factory的para（没有实体，直接返回默认值）
                }
                return m_pObj;
            }
            set
            {
                if (RMath.Equal(Value, value))
                    return;

                if (m_pPI != null)
                {
                    // 由于c#不支持隐式转换，比如string在赋值给RColor的时候，无法作用
                    // 这里需要针对这种情况先处理下
                    if (ValueType.Name == typeof(RColor).Name)
                    {
                        RColor c = RColor.FromObject(value);

                        m_pPI.SetValue(m_pHost, c);
                    }
                    else
                    {
                        try
                        {
                            m_pPI.SetValue(m_pHost, value);
                        }
                        catch (Exception e)
                        {
                            m_pPI.SetValue(m_pHost,  Convert.ChangeType(value, ValueType));
                        }

                    }
                }
                else
                    m_pObj = value;


                if (object.Equals(_StartVal,value))
                    ValueChanged = false;
                else
                    ValueChanged = true;
                RaisePropertyChanged("Value");
            }
        } // 最终值

        public bool         ValueChanged
        {
            get { return m_bValueChanged; }
            protected set { Set("ValueChanged", ref m_bValueChanged, value); }
        } // 相对一开始是否有变化
        public object       CompareValue { get; set; }                      // 对比变化Value，如果不设置，默认就是初始Value


#region C&D
        public              Para(object host, PropertyInfo pi , string group , string desc,  object defval , ICollection<object> presets = null)
        {
            m_pHost = host;
            m_pPI = pi;
            Name = pi.Name;
            OriginalName = pi.Name;

            ValueType = pi.PropertyType;

            Group = group;
            Desc = desc;

            m_pDefVal = defval;
            m_lPresets = new List<object>();
            if(presets != null)
                m_lPresets.AddRange(presets);

            NullPresentation = "Any";
	        PresetsUseEnum = true;

            _StartVal = m_pPI.GetValue(m_pHost);
        }

        public              Para(PropertyInfo pi, string group , string desc,  object defval , IEnumerable<object> presets = null,bool nullable = false)
        {
            //if(obj == null)
            //    throw new Exception("Initial obj can't be null");

            //m_pObj = obj;
            Name = pi.Name;
            OriginalName = pi.Name;
            m_pPI = pi;

            ValueType = pi.PropertyType;

            Nullable = nullable;

            Group = group;
            Desc = desc;

            m_pDefVal = defval;
            m_lPresets = new List<object>();
            if(presets != null)
                m_lPresets.AddRange(presets);

            NullPresentation = "Any";
	        PresetsUseEnum = true;

            _StartVal = m_pObj;
        }
        public              Para(object host, string name,  string group , string desc,  object defval , IEnumerable<object> presets = null,bool nullable = false)
        {
            //if(obj == null)
            //    throw new Exception("Initial obj can't be null");

            m_pObj = host;
            Name = name;
            //m_pPI = pi;

            if(host!=null)
                ValueType = host.GetType();

            Nullable = nullable;

            Group = group;
            Desc = desc;

            m_pDefVal = defval;
            m_lPresets = new List<object>();
            if(presets != null)
                m_lPresets.AddRange(presets);

            NullPresentation = "Any";
	        PresetsUseEnum = true;

            _StartVal = m_pObj;
        }

#endregion


#region Members

        protected List<object> m_lPresets;
        protected Type m_pValueType;
        protected bool m_bValueChanged;
        protected object    m_pDefVal;
        public object       _StartVal;                                      // 特殊，正常不应改，用于source的动态提取
        protected object    m_pMinVal;
        protected object    m_pMaxVal;
        // 模式一
        protected object    m_pObj;                                         

        // 模式二
        protected object    m_pHost;
        protected PropertyInfo m_pPI;
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
