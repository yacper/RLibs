/********************************************************************
    created:	2018/5/24 15:47:41
    author:	rush
    email:		
	
    purpose:	成员属性-- 通常用于表示用户可以更改的属性，并且可以搭配paraEditor使用
*********************************************************************/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using DataModel;
using NPOI.Util;

namespace RLib.Base
{
    public interface IPara:INotifyPropertyChanged, ISerializable
    {
        object              Host { get; }

        string              Name { get; set; }
        string              OriginalName { get; }                           // 原始名称
        string              Group { get; set; }
        IEnumerable<string> GroupEx { get; set; }                           // 其他组别
        string              Desc { get; set; }
        Type                ValueType { get; }

        object              DefValue { get; set; }                                // 默认
        IEnumerable<object> Presets { get; set; }                                // 预设项
        
        object              Value { get; set; }                             // 最终值
        bool                ValueChanged { get; }                           // 相对一开始是否有变化
        object              CompareValue { get; set; }                      // 对比变化Value，如果不设置，默认就是初始Value

        bool                HideInParaGrid { get; set; }                    // 不在ParaGrid中编辑

        bool                Nullable { get; }                               // 是否可以设置为NUll
        string              NullPresentation { get; set; }                       // 代表NUll时的string表示
        string				NullOperator { get; set; }						// Null 操作表示


        // 数字可选
        object              Min { get; set; }
        object              Max { get; set; }
        object              Step { get; set; }

        // 作为数字range的时候
        bool                IsNumRange { get; set; }                        // 是否作为数字Range( 不同于UseRange)
        object              RangeMin { get; set; }
        object              RangeMax { get; set; }
        object              RangeStep { get; set; }


        // enum 可选
		bool				PresetsUseEnum { get; set; }					// 当valueType是enum，不设Presets的时候，取Enum的值作为preset
        bool                Multi { get; }                                  // 多选-- 默认false
        int                 Row { get; }                                    // 设定多少行
        int                 Col { get; }                                    // 设定多少列


        bool                UseRange { get; }                               // 使用Range类, value返回也是range类

        // File & Dir
        bool                IsFile { get; }                                 // value string 作为file
        bool                IsDir { get; }                                  //  value string 作为dir


        Func<object,string> ValueFormater { get;set; }                      // value 的formater 
        Dictionary<string, object> ExArgs { get;set; }                      // 额外参数 

//        Dictionary<string, object> CompoundArgs { get;set; }                // para Property 内部的参数
        List<IPara>         NestingParas { get; set; }                      // para Property 内部的参数


        void                Reset(bool def = false); // 返回初始值
        PropertyInfo        PI { get; }
    }

    public static class ParaEx
    {
        public static List<IPara>  GetParas(this Type t)                   // 由于系统提供的函数有问题，只能自己实现
        {
            List<IPara> ret = new List<IPara>();

            // 遍历t的继承链上的interface等
            if (!t.IsInterface)
            {
                Type[] intefaces = t.GetInterfaces();
                foreach (Type i in intefaces)
                {
                    ret.AddRange(GetParas(i));
                }
            }


            // 遍历t的所有property，找到带有attr的属性
            // 遍历t的集成链上的比如interface等
            foreach (PropertyInfo pi in t.GetProperties())
            {
                if (!pi.IsDefined(typeof(ParaAttribute)))
                    continue;

                ParaAttribute attr = pi.GetCustomAttribute(typeof(ParaAttribute)) as ParaAttribute;
                ret.Add(attr.GetPara(pi));
            }

            return ret;
        }

        public static List<IPara>  GetParas(this object o, Type t = null)                   // 由于系统提供的函数有问题，只能自己实现
        {
            if(t == null)
                t = o.GetType();

            List<IPara> ret = new List<IPara>();

            // 遍历t的继承链上的interface等
            if (!t.IsInterface)
            {
                Type[] intefaces = t.GetInterfaces();
                foreach (Type i in intefaces)
                {
                    ret.AddRange(GetParas(o, i));
                }
            }


            // 遍历t的所有property，找到带有attr的属性
            // 遍历t的集成链上的比如interface等
            foreach (PropertyInfo pi in t.GetProperties())
            {
                if (!pi.IsDefined(typeof(ParaAttribute)))
                    continue;

                ParaAttribute attr = pi.GetCustomAttribute(typeof(ParaAttribute)) as ParaAttribute;
                ret.Add(attr.GetPara(o, pi));
            }

            return ret;
        }


        public static void ApplyValues(this IEnumerable<IPara> paras, IEnumerable<KeyValuePair<string, object>> kvs) 
        {
            foreach (KeyValuePair<string, object> kv in kvs)
            {
                IPara para = paras.FirstOrDefault(p => p.OriginalName == kv.Key);
                if (para != null)
                    para.Value = kv.Value;
            }
        }

        public static void ApplyDefValues(this IEnumerable<IPara> paras, IEnumerable<KeyValuePair<string, object>> kvs) 
        {
            foreach (KeyValuePair<string, object> kv in kvs)
            {
                IPara para = paras.FirstOrDefault(p => p.OriginalName == kv.Key);
                if (para != null)
                    (para as Para).DefValue = kv.Value;
            }
        }

        public static void ApplyDM(this IEnumerable<IPara> paras, IEnumerable<KVDM> kvs) 
        {
            foreach (KVDM kv in kvs)
            {
                IPara para = paras.FirstOrDefault(p => p.OriginalName == kv.Key);
                if (para != null)
                {
                    if(para.NestingParas == null)
                        para.Value = kv.Value.Value;
                    else
                    {
                        para.Value.GetParas().ApplyDM(kv.Nestings);  // 赋值nesting paras
                    }
                }
            }
        }


        public static List<DataModel.KVDM> ToDm(this IEnumerable<IPara> paras) 
        {
            List<DataModel.KVDM> l = new List<KVDM>();
            foreach (IPara p in paras)
            {
                KVDM dm = p.ToDm() as KVDM;
                if(dm != null)
                l.Add(dm);
            }

            return l;
        }
    }
    
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]       // 只能用于property
    public class ParaAttribute : Attribute
    {
        public string       Name { get; set;}
        public string       Desc { get; set; }
        public string       Group { get; set; }

        public bool         HideInParaGrid { get; set; }

        public object       DefVal { get;set; }
        public List<object> Presets { get; set; }

        // 区间
        public object       Min { get; set; }
        public object       Max { get; set; }
        public object       Step { get; set; }

        public bool         IsFile { get; set; }                                 // value string 作为file
        public bool         IsDir { get; set; }                                  //  value string 作为dir

        // enum
        public bool         Multi { get; set; }


        public virtual IPara        GetPara(PropertyInfo pi)
        {
            object def = DefVal;
            //todo: 针对Color处理
            //if(pi.PropertyType == typeof(Color))
            //{
            //    def = ColorEx.FromString(DefVal as string);
            //}

            return new Para(pi, Group, Desc, def){Name = Name, IsFile = IsFile, IsDir = IsDir, Min = Min, Max = Max, Step = Step, Multi = Multi, HideInParaGrid = HideInParaGrid};
        }

        public virtual IPara        GetPara(object host, PropertyInfo pi)
        {
            Type t = pi.PropertyType;

            object def = DefVal;
            //todo: 针对Color处理
            //if(pi.PropertyType == typeof(Color))
            //{
            //    def = ColorEx.FromString(DefVal as string);
            //}

            return new Para(host, pi, Group, Desc, def, Presets){Name = Name, IsFile = IsFile, IsDir = IsDir, Min = Min, Max = Max, Step = Step, Multi = Multi, HideInParaGrid = HideInParaGrid};
        }


   
        public              ParaAttribute(string name, string desc = "", string group="",  object defVal = null/*, params object[] presets*/)
        {
            Name = name;
            Desc = desc;
            Group = group;
            DefVal = defVal;
//            Presets = new List<object>(presets);
        }

        //public              ParaAttribute(EProtoDataType type, string name, string desc = "", string group="",  object defVal = null, params object[] presets)
        //{
        //    Name = name;
        //    Desc = desc;
        //    Group = group;
        //    DefVal = defVal;
        //    Presets = new List<object>(presets);
        //}
        
        //public              ParaAttribute(string desc = "", string group="",  object defVal = null, params object[] presets)
        //{
        //    Desc = desc;
        //    Group = group;
        //    DefVal = defVal;
        //    Presets = new List<object>(presets);
        //}

        // numrange
        //public              ParaAttribute(string desc = "", string group="",  ECompareOp op= ECompareOp.Equal, double l = Double.NaN ,double r =Double.NaN, params object[] presets)
        //{
        //    Desc = desc;
        //    Group = group;
        //    DefVal = new NumRange(op, l, r);

        //    if (presets.Count() != 0)
        //    {
        //        Presets = new List<object>();
        //        int n = presets.Count() / 3;
        //        for (int i = 0; i != n; ++i)
        //        {
        //            NumRange nr = new NumRange((ECompareOp)presets.ElementAt(i*3), (double)presets.ElementAt(i*3+1), (double)presets.ElementAt(i*3+2));
                    
        //            Presets.Add(nr);
        //        }
                
        //    }

        //}

        //// RColor
        //public              ParaAttribute(string desc = "", string group = "", uint defVal = 0x00000000, params object[] presets)
        //{
        //    Desc = desc;
        //    Group = group;
        //    DefVal = new RColor(defVal);
        //    Presets = new List<object>();
        //    foreach (uint v in presets)
        //    {
        //        Presets.Add(new RColor(v));
        //    }

        //}

        //// fontdm
        //public              ParaAttribute(string desc = "", string group = "", string fface="宋体", float fsize =12, uint fcode = 0,
        //   bool fblod= false,bool fitalic = false, bool fline = false,params object[] presets)
        //{
        //    Desc = desc;
        //    Group = group;
        //    DefVal = new FontDM()
        //    {
        //        face = fface,
        //        size = fsize,
        //        color = (int)fcode,
        //        bold = fblod,
        //        italic = fitalic,
        //        underline = fline,
        //    };

        //    if (presets.Count() != 0)
        //    {
        //        Presets = new List<object>();
        //        int n = presets.Count() / 6;
        //        for (int i = 0; i != n; ++i)
        //        {
        //            FontDM fontDm = new FontDM()
        //            {
        //                face = (string)presets.ElementAt(i * 6),
        //                size = (float)presets.ElementAt(i * 6 + 1),
        //                color = (int)(uint)presets.ElementAt(i * 6 + 2),
        //                bold = (bool)presets.ElementAt(i *6 + 3),
        //                italic = (bool)presets.ElementAt(i * 6 + 4),
        //                underline = (bool)presets.ElementAt(i * 6 + 5),
        //            };
        //            Presets.Add(fontDm);
        //        }
        //    }
        //}


    }

    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class ColorParaAttribute : ParaAttribute
    {

        public              ColorParaAttribute(string name, string desc = "", string group="",  object defVal= null/*, params object[] presets*/)
        :base(name, desc, group, RColor.From(defVal)/*, presets*/)
        {
        }
    }

     
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class FileParaAttribute : ParaAttribute
    {
        public              FileParaAttribute(string name, string desc = "", string group=""/*,  params object[] presets*/)
        :base(name, desc, group,  null/*, presets*/)
        {
            IsFile = true;
        }
    }

    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class DirectoryParaAttribute : ParaAttribute
    {
        public              DirectoryParaAttribute(string name, string desc = "", string group=""/*,  params object[] presets*/)
        :base(name, desc, group,  null/*, presets*/)
        {
            IsDir = true;
        }
    }



    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class NestingParaAttribute : ParaAttribute                            // 输出
    {
        public override IPara GetPara(PropertyInfo pi)
        {
            Type ot = pi.PropertyType;

            List<IPara> lps = ot.GetParas();
            return new Para(pi, Group, Desc, DefVal)
            {
                IsFile = IsFile,
                IsDir = IsDir,
                Min = Min,
                Max = Max,
                Step = Step,
                NestingParas = lps
            };
        }

        public override IPara GetPara(object host, PropertyInfo pi)
        {
            Type t = pi.PropertyType;

            object o = pi.GetValue(host);  // 内部property对象
           
            List<IPara> lps = o.GetParas();

            return new Para(host, pi, Group, Desc, DefVal, Presets)
            {
                IsFile = IsFile,
                IsDir = IsDir,
                Min = Min,
                Max = Max,
                Step = Step,
                NestingParas = lps,
            };
        }

        public NestingParaAttribute(string name, string desc = "", string group = "")
        : base(name, desc, group)
        {

        }
    }
}
