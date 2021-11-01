/********************************************************************
    created:	2017/4/1 10:56:54
    author:		rush
    email:		
	
    purpose:	

*********************************************************************/
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Xml;

namespace RLib.Base
{
    public enum EParamType
    {
        Boolean = 1,
        Integer,
        Enum,
        Double,
        Float,
        String,
        File,
        Directory,
        Color,
        IntElastic,
        FloatElastic,
        DoubleElastic,
        StringCombo,            // 多个string
        IntCombo,            // 多个string
        Object
    }

    public partial class Param
    {
        public static IParam       FromXmlNode(XmlNode node)                       // 从xmlnode获取
        {
            IParam ret = null;
            string name = node.Attributes["Name"].Value;
            EParamType type = (EParamType)Enum.Parse(typeof(EParamType), node.Attributes["Type"].Value);
            string value = node.Attributes["Value"].Value;
            switch (type)
            {
                case EParamType.Boolean:
                    ret = new BooleanParam(name);
                    ret.FromString(value);
                    break;
                case EParamType.Integer:
                    ret = new IntParam(name);
                    ret.FromString(value);
                    break;
                case EParamType.Double:
                    ret = new DoubleParam(name);
                    ret.FromString(value);
                    break;
                case EParamType.Float:
                    ret = new FloatParam(name);
                    ret.FromString(value);
                    break;
                case EParamType.String:
                    ret = new StringParam(name);
                    ret.FromString(value);
                    break;

                case EParamType.File:
                    ret = new FileParam(name);
                    ret.FromString(value);
                    break;
                case EParamType.Directory:
                    ret = new DirectoryParam(name);
                    ret.FromString(value);
                    break;

                case EParamType.Color:
                    ret = new ColorParam(name);
                    ret.FromString(value);
                    break;
                case EParamType.IntElastic:
                    ret = new IntElasticParam(name);
                    ret.FromString(value);
                    break;
                case EParamType.FloatElastic:
                    ret = new FloatElasticParam(name);
                    ret.FromString(value);
                    break;
                case EParamType.DoubleElastic:
                    ret = new DoubleElasticParam(name);
                    ret.FromString(value);
                    break;
            }

            return ret;
        }
    }

    public interface IParam
    {
        string              ToString { get; }
        void                FromString(string str);                         // 反序列化
        XmlNode             ToXmlNode(XmlDocument doc);

        string              Name { get; }
        EParamType          Type { get; }
        string              Desc { get; }
        string              Group { get; }
        object              Value { get ;  }
        Type                ValueType { get ;  }
        //bool                ReadOnly { get; }

        bool                Multi { get; }                                  // 是否多选
        List<object>        Values { get ;  }                               // 如果多选

        object              Clone();

        void                _SetValue(object val);
    }

    public interface IFileParam : IParam
    {
        string              Appendix { get; }                               // 后缀
    }
    public interface IDirectoryParam : IParam
    {
        string              Wildchars { get; }                               // wildchars
    }

    public interface IParams
    {
        List<IParam>        ToList { get; }

        IParams             Clone { get; }

        IParam              GetParam(string name);
        T                   GetParam<T>(string name);

        object              GetValue(string name);
        T                   GetValue<T>(string name);

        void                SetValue(string name, object value);
    }

    [AttributeUsage(AttributeTargets.Class|AttributeTargets.Interface, AllowMultiple = true)]
    public class ParamAttribute : Attribute
    {
        public IParam Param { get { return m_pParam; } }


        protected IParam m_pParam;
    }
    

}
