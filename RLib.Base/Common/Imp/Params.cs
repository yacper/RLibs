/********************************************************************
    created:	2017/3/26 12:29:20
    author:		rush
    email:		
	
    purpose:	
*********************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace RLib.Base
{
    public partial class Param<T>:IParam
    {
        public virtual new string ToString { get { return m_oValue.ToString();} }
        public virtual void FromString(string str){ throw new NotImplementedException();}                         // 反序列化

        public virtual XmlNode             ToXmlNode(XmlDocument doc)
        {
            XmlNode param = doc.CreateElement("Param");

            XmlAttribute n = doc.CreateAttribute("Name");
            n.Value = Name;
            param.Attributes.Append(n);

            XmlAttribute t = doc.CreateAttribute("Type");
            t.Value = Type.ToString();
            param.Attributes.Append(t);

            XmlAttribute value = doc.CreateAttribute("Value");
            value.Value = Value.ToString();
            param.Attributes.Append(value);

            return param;
        }

        public string       Name { get { return m_strName; }  }
        public virtual EParamType Type { get { throw new NotImplementedException(); }  }
        public object       Value { get { return m_oValue; } }
        public T            Value2 { get { return m_oValue; } }
        public string       Desc { get { return m_strDesc; } }
        public string       Group { get { return m_strGroup; } }
        public Type         ValueType { get { return m_oValue != null?m_oValue.GetType():null; }  }


        public bool         Multi { get { return m_bMulti; } }                                  // 是否多选
        public List<object> Values { get { return m_pValues; }  }                               // 如果多选



        public virtual object Clone()
        {
            return MemberwiseClone();
        }

        public virtual void _SetValue(object val)
        {
            if (val is T)
            {
                m_oValue = (T)val;
            }
        }

#region C&D
        public              Param(string name, T value, string desc = "", string group="")
        {
            m_strName = name;
            m_oValue = value;
            m_strDesc = desc;
            m_strGroup = group;
        }
#endregion

#region Members
        protected string    m_strName;
        protected T         m_oValue;
        protected string    m_strDesc;
        protected string    m_strGroup;

        protected bool      m_bMulti;
        protected List<object> m_pValues;
#endregion
    }


    public class StringParam : Param<String>
    {

        public override void FromString(string str) { m_oValue = str; }                         // 反序列化

        public override EParamType Type { get { return EParamType.String; }  }
        public              StringParam(string name, string value="", string desc = "", string group="")
            :base(name, value,desc, group)
        {
        }
    }
    public class StringParamAttribute :ParamAttribute 
    {
        public StringParamAttribute(string name, string value="", string desc = "", string group="")
        {
            m_pParam = new StringParam(name, value, desc, group);
        }
    }

    public class ObjectParam : Param<object>
    {
        public override EParamType Type { get { return EParamType.Object; }  }
        public              ObjectParam(string name, object value=null, string desc = "", string group="")
            :base(name, value,desc, group)
        {
        }
    }
    public class ObjectParamAttribute :ParamAttribute 
    {
        public ObjectParamAttribute(string name, object value=null, string desc = "", string group="")
        {
            m_pParam = new ObjectParam(name, value, desc, group);
        }
    }

    public class BooleanParam : Param<bool>
    {
        public override void FromString(string str) { m_oValue = Convert.ToBoolean(str); }                         // 反序列化

        public override EParamType Type { get { return EParamType.Boolean; }  }
        public              BooleanParam(string name, bool value=false, string desc = "", string group="")
            :base(name, value,desc, group)
        {
        }
    }
    public class BooleanParamAttribute :ParamAttribute 
    {
        public BooleanParamAttribute(string name, bool value=false, string desc = "", string group="")
        {
            m_pParam = new BooleanParam(name, value, desc, group);
        }
    }
    public class IntParam : Param<Int32>
    {
        public override void FromString(string str) { m_oValue = Convert.ToInt32(str); }                         // 反序列化

        public override EParamType Type { get { return EParamType.Integer; }  }
        public              IntParam(string name, int value=0, string desc = "", string group="")
            :base(name, value,desc, group)
        {
        }
    }
    public class IntParamAttribute :ParamAttribute 
    {
        public IntParamAttribute(string name, int value=0, string desc = "", string group="")
        {
            m_pParam = new IntParam(name, value, desc, group);
        }
    }
    public class EnumParam : Param<object>
    {
        public override void FromString(string str)
        {
            string[] splits = str.Split('.');
            string type = splits[0];
            string value = splits[1];

            m_oValue = EnumHelper.FromString(System.Type.GetType(type), value);
        }                         // 反序列化

        public override string ToString { get { return m_oValue.ToString();} }

        public override EParamType Type { get { return EParamType.Enum; } }

        public              EnumParam(string name, object value = null, string desc = "", string group="")
            :base(name, value,desc, group)
        {
        }
    }
    public class EnumParamAttribute :ParamAttribute 
    {
        public              EnumParamAttribute(string name, object value, string desc = "", string group="")
        {
            m_pParam = new EnumParam(name, value, desc, group);
        }
    }
    public class FloatParam : Param<float>
    {
        public override void FromString(string str) { m_oValue = Convert.ToSingle(str); }                         // 反序列化

        public override EParamType Type { get { return EParamType.Float; }  }
        public              FloatParam(string name, float value=0f, string desc = "", string group="")
            :base(name, value,desc, group)
        {
        }
    }
    public class FloatParamAttribute :ParamAttribute 
    {
        public FloatParamAttribute(string name, float value=0f, string desc = "", string group="")
        {
            m_pParam = new FloatParam(name, value, desc, group);
        }
    }
    public class DoubleParam : Param<Double>
    {
        public override void FromString(string str) { m_oValue = Convert.ToDouble(str); }                         // 反序列化

        public override EParamType Type { get { return EParamType.Double; }  }
        public              DoubleParam(string name, double value=0d, string desc = "", string group="")
            :base(name, value,desc, group)
        {
        }
    }
    public class DoubleParamAttribute :ParamAttribute 
    {
        public DoubleParamAttribute(string name, double value=0d, string desc = "", string group="")
        {
            m_pParam = new DoubleParam(name, value, desc, group);
        }
    }
    public class ColorParam : Param<RColor>
    {
        public override string ToString { get { return string.Format("({0},{1},{2})", m_oValue.R, m_oValue.G, m_oValue.B);} }

        public override void FromString(string str)                         // 反序列化
        {
            string input = str.TrimStart('(');
            input = str.TrimEnd(')');

            string[] vals = str.Split(',');

            m_oValue = RColor.FromArgb(Convert.ToInt32(vals[0]), Convert.ToInt32(vals[1]), Convert.ToInt32(vals[2]));
        }                         

        public override EParamType Type { get { return EParamType.Color; }  }
        public              ColorParam(string name, RColor value=new RColor(), string desc = "", string group="")
            :base(name, value, desc, group)
        {
        }
    }
    public class ColorParamAttribute :ParamAttribute 
    {
        public ColorParamAttribute(string name, int r, int g, int b, string desc = "", string group="")
        {
            m_pParam = new ColorParam(name, new RColor(255, r, g, b), desc, group);
        }
    }
    public class FileParam : StringParam, IFileParam                                  // 文件
    {

        public override void FromString(string str) { m_oValue = str; }                         // 反序列化

        public override XmlNode ToXmlNode(XmlDocument doc)
        {
            XmlNode ret = base.ToXmlNode(doc);

            XmlAttribute value = doc.CreateAttribute("Appendix");
            value.Value = Appendix;
            ret.Attributes.Append(value);

            return ret;
        }

        public string       Appendix { get { return m_strAppendix; } }                               // 后缀

        public override EParamType Type { get { return EParamType.String; }  }
        public              FileParam(string name, string value="", string appendix = "", string desc = "", string group="")
            :base(name, value,desc, group)
        {
            m_strAppendix = appendix;
        }

        protected string    m_strAppendix;                                  // 根据appedix选择文件
    }

    public class FileParamAttribute :ParamAttribute 
    {
        public FileParamAttribute(string name, string value="", string appendix = "", string desc = "", string group="")
        {
            m_pParam = new FileParam(name, value, appendix, desc, group);
        }
    }

    public class DirectoryParam : StringParam                                  // 文件
    {


        public override void FromString(string str) { m_oValue = str; }                         // 反序列化

        public override XmlNode ToXmlNode(XmlDocument doc)
        {
            XmlNode ret = base.ToXmlNode(doc);

            XmlAttribute value = doc.CreateAttribute("Wildchars");
            value.Value = Wildchars;
            ret.Attributes.Append(value);

            return ret;
        }

        public string       Wildchars { get { return m_strWildchars; } }                               // 后缀
        public override EParamType Type { get { return EParamType.String; }  }
        public              DirectoryParam(string name, string value="", string wildchars = "", string desc = "", string group="")
            :base(name, value,desc, group)
        {
            m_strWildchars = wildchars;
        }
        protected string    m_strWildchars;                                  // 根据appedix选择文件
    }
    public class DirectoryParamAttribute :ParamAttribute 
    {
        public DirectoryParamAttribute(string name, string value="", string wildchars = "", string desc = "", string group="")
        {
            m_pParam = new DirectoryParam(name, value, wildchars, desc, group);
        }
    }


    public class ElasticParam<T> : Param<T>
    {
        public T                   Min { get { return m_oMin; } }
        public T                   Max { get { return m_oMax; } }


#region C&D
        public              ElasticParam(string name, T value, T min, T max, string desc = "", string group="")
            :base(name, value, desc, group)
        {
            m_oMin = min;
            m_oMax = max;
        }
#endregion
        
#region Members
        protected T         m_oMin;
        protected T         m_oMax;
#endregion
    }
    public class IntElasticParam : ElasticParam<Int32>
    {
        public override string ToString { get { return string.Format("{0},{1},{2}", m_oValue, m_oMin, m_oMax);} }

        public override void FromString(string str)                         // 反序列化
        {
            string[] vals = str.Split(',');
            m_oValue = Convert.ToInt32(vals[0]);
            m_oMin = Convert.ToInt32(vals[1]);
            m_oMax = Convert.ToInt32(vals[2]);
        }                         

        public override EParamType Type { get { return EParamType.IntElastic; }  }
        public              IntElasticParam(string name, int value=0, int min = Int32.MinValue, int max = Int32.MaxValue, string desc = "", string group="")
            :base(name, value,min, max, desc, group)
        {
        }
    }
    public class IntElasticParamAttribute :ParamAttribute 
    {
        public IntElasticParamAttribute(string name, int value=0, int min = Int32.MinValue, int max = Int32.MaxValue, string desc = "", string group="")
        {
            m_pParam = new IntElasticParam(name, value, min, max, desc, group);
        }
    }




    public class DoubleElasticParam : ElasticParam<Double>
    {
        public override string ToString { get { return string.Format("{0},{1},{2}", m_oValue, m_oMin, m_oMax);} }
        public override void FromString(string str)                         // 反序列化
        {
            string[] vals = str.Split(',');
            m_oValue = Convert.ToDouble(vals[0]);
            m_oMin = Convert.ToDouble(vals[1]);
            m_oMax = Convert.ToDouble(vals[2]);
        }                         

        public override EParamType Type { get { return EParamType.DoubleElastic; }  }
        public              DoubleElasticParam(string name, double value =0d, double min = Double.MinValue, double max = Double.MaxValue, string desc = "", string group="")
            :base(name, value,min, max, desc, group)
        {
        }
    }
    public class DoubleElasticParamAttribute :ParamAttribute 
    {
        public DoubleElasticParamAttribute(string name, double value =0d, double min = Double.MinValue, double max = Double.MaxValue, string desc = "", string group="")
        {
            m_pParam = new DoubleElasticParam(name, value, min, max, desc, group);
        }
    }

    public class FloatElasticParam : ElasticParam<Double>
    {
        public override string ToString { get { return string.Format("{0},{1},{2}", m_oValue, m_oMin, m_oMax);} }
        public override void FromString(string str)                         // 反序列化
        {
            string[] vals = str.Split(',');
            m_oValue = Convert.ToSingle(vals[0]);
            m_oMin = Convert.ToSingle(vals[1]);
            m_oMax = Convert.ToSingle(vals[2]);
        }                         

        public override EParamType Type { get { return EParamType.FloatElastic; }  }
        public              FloatElasticParam(string name, float value =0f, float min = Single.MinValue, float max = Single.MaxValue, string desc = "", string group="")
            :base(name, value,min, max, desc, group)
        {
        }
    }
    public class FloatElasticParamAttribute :ParamAttribute 
    {
        public FloatElasticParamAttribute(string name, float value =0f, float min = Single.MinValue, float max = Single.MaxValue, string desc = "", string group="")
        {
            m_pParam = new FloatElasticParam(name, value, min, max, desc, group);
        }
    }



#region ComboPram  // 多选一
    public class ComboPram<T> : Param<T>
    {
        public List<T>         Choices {  get { return m_lChoices; } }

#region C&D
        public              ComboPram(string name,  T value, T choice1, string desc = "", string group="")
            :this(name, value, choice1, default(T), default(T), desc, group)
        {
        }
        public              ComboPram(string name,  T value, T choice1, T choice2, string desc = "", string group="")
            :this(name, value, choice1, choice2, default(T), desc, group)
        {
        }
        public              ComboPram(string name,  T value, T choice1, T choice2, T choice3, string desc = "", string group="")
            :base(name, value, desc, group)
        {
            m_lChoices.Add(value);
            if (EqualityComparer<T>.Default.Equals(choice1, default(T)))
                m_lChoices.Add(choice1);
            if (EqualityComparer<T>.Default.Equals(choice2, default(T)))
                m_lChoices.Add(choice1);
            if (EqualityComparer<T>.Default.Equals(choice3, default(T)))
                m_lChoices.Add(choice1);
        }
#endregion
        
#region Members
        protected List<T>         m_lChoices = new List<T>();
#endregion
    }
    public class StringComboParam : ComboPram<string>
    {
        //public override string ToString { get { return string.Format("{0},{1},{2}", m_oValue, m_oMin, m_oMax);} }

        //public override void FromString(string str)                         // 反序列化
        //{
        //    string[] vals = str.Split(',');
        //    m_oValue = Convert.ToInt32(vals[0]);
        //    m_oMin = Convert.ToInt32(vals[1]);
        //    m_oMax = Convert.ToInt32(vals[2]);
        //}                         

        public override EParamType Type { get { return EParamType.StringCombo; }  }

        public StringComboParam(string name, string value, string choice1, string choice2, string choice3, string desc = "", string group = "")
            : base(name, value, choice1, choice2, choice3, desc, group)
        {
        }
     
    }

    public class StringComboParamAttribute :ParamAttribute 
    {
        public StringComboParamAttribute(string name, string value, string choice1,  string desc = "", string group="")
            :this(name, value, choice1, default(string), default (string), desc, group)
        {
        }
        public StringComboParamAttribute(string name, string value, string choice1, string choice2,  string desc = "", string group="")
            :this(name, value, choice1, choice2, default (string), desc, group)
        {
        }
        public StringComboParamAttribute(string name, string value, string choice1, string choice2, string choice3,  string desc = "", string group="")
        {
            m_pParam = new StringComboParam(name, value, choice1, choice2, choice3, desc, group);
        }
    }

    public class IntComboParam : ComboPram<int>
    {
        //public override string ToString { get { return string.Format("{0},{1},{2}", m_oValue, m_oMin, m_oMax);} }

        //public override void FromString(string str)                         // 反序列化
        //{
        //    string[] vals = str.Split(',');
        //    m_oValue = Convert.ToInt32(vals[0]);
        //    m_oMin = Convert.ToInt32(vals[1]);
        //    m_oMax = Convert.ToInt32(vals[2]);
        //}                         

        public override EParamType Type { get { return EParamType.IntCombo; }  }

        public IntComboParam(string name, int value, int choice1, int choice2, int choice3, string desc = "", string group = "")
            : base(name, value, choice1, choice2, choice3, desc, group)
        {
        }
     
    }

    public class IntComboParamAttribute :ParamAttribute 
    {
        public IntComboParamAttribute(string name, int value, int choice1,  string desc = "", string group="")
            :this(name, value, choice1, default(int), default (int), desc, group)
        {
        }
        public IntComboParamAttribute(string name, int value, int choice1, int choice2,  string desc = "", string group="")
            :this(name, value, choice1, choice2, default (int), desc, group)
        {
        }
        public IntComboParamAttribute(string name, int value, int choice1, int choice2, int choice3,  string desc = "", string group="")
        {
            m_pParam = new IntComboParam(name, value, choice1, choice2, choice3, desc, group);
        }
    }

    //public class IntComboParam : ComboPram<int>
    //{
    //    //public override string ToString { get { return string.Format("{0},{1},{2}", m_oValue, m_oMin, m_oMax);} }

    //    //public override void FromString(string str)                         // 反序列化
    //    //{
    //    //    string[] vals = str.Split(',');
    //    //    m_oValue = Convert.ToInt32(vals[0]);
    //    //    m_oMin = Convert.ToInt32(vals[1]);
    //    //    m_oMax = Convert.ToInt32(vals[2]);
    //    //}                         

    //    public override EParamType Type { get { return EParamType.StringCombo; }  }

    //    public IntComboParam(string name, int value, int choice1, int choice2, int choice3, string desc = "", string group = "")
    //        : base(name, value, choice1, choice2, choice3, desc, group)
    //    {
    //    }
     
    //}

    //public class IntComboParamAttribute :ParamAttribute 
    //{
    //    public IntComboParamAttribute(string name, int value, int choice1,  string desc = "", string group="")
    //        :this(name, value, choice1, default(int), default (int), desc, group)
    //    {
    //    }
    //    public IntComboParamAttribute(string name, int value, int choice1, int choice2,  string desc = "", string group="")
    //        :this(name, value, choice1, choice2, default (int), desc, group)
    //    {
    //    }
    //    public IntComboParamAttribute(string name, int value, int choice1, int choice2, int choice3,  string desc = "", string group="")
    //    {
    //        m_pParam = new IntComboParam(name, value, choice1, choice2, choice3, desc, group);
    //    }
    //}
#endregion

    public class Params:IParams
    {
        public List<IParam> ToList { get { return m_dParams; } }

        public IParams      Clone
        {
            get
            {
                List<IParam> lp = new List<IParam>();
                foreach (IParam p in m_dParams)
                {
                    lp.Add(p.Clone() as IParam);
                }

                return new Params(lp);
            }
        }

        //public List<KeyValuePair<string, object>> ToKvList
        //{
        //    get
        //    {
        //        List<KeyValuePair<string, object>> l = new List<KeyValuePair<string, object>>();
        //        foreach (KeyValuePair<string, IParam> kv in m_dParams)
        //        {
        //            l.Add(new KeyValuePair<string, object>(kv.Key, kv.Value.Value));
        //        }

        //        return l;
        //    }
        //}

        public IParam       GetParam(string name)
        {
            return m_dParams.Find(p => string.Compare(p.Name , name) == 0);

            //if (m_dParams.ContainsKey(name))
            //    return m_dParams[name];
            //else
            //    return null;
        }

        public T            GetParam<T>(string name)
        {
            IParam p = GetParam(name);
            if (p != null)
                return (T) p;
            else
                return default(T);
        }

        public object       GetValue(string name)
        {
            IParam p = GetParam(name);
            if (p != null)
                return p.Value;
            else
                return null;
        }

        public T            GetValue<T>(string name)
        {
            object ret = GetValue(name);
            if (ret != null)
                return (T) ret;
            else
                return default(T);
        }



        public void         SetValue(string name, object value)
        {
            IParam p = GetParam(name);
            if (p == null)
                return;

            p._SetValue(value);
        }

#region C&D
        public Params()
        {
            
        }
        public Params(List<IParam> list)
        {
            m_dParams = list;
            //foreach (IParam param in list)
            //{
            //    try
            //    {
            //        m_dParams.Add(param.Name, param);
            //    }
            //    catch (Exception e) // 可能有重复，忽略
            //    {
                    
            //    }
            //}
        }
#endregion


#region Members
//        protected Dictionary<string, IParam> m_dParams = new Dictionary<string, IParam>(); 
        protected List<IParam> m_dParams = new List<IParam>(); 
#endregion
    }


}
