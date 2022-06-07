///********************************************************************
//    created:	2018/5/24 15:47:41
//    author:	rush
//    email:		
	
//    purpose:	成员属性-- 通常用于表示用户可以更改的属性，并且可以搭配paraEditor使用
//*********************************************************************/
//using System;
//using System.Collections.Generic;
//using System.ComponentModel;
//using System.Linq;
//using System.Reflection;
//using System.Text;
//using System.Threading.Tasks;
//using DataModel;

//namespace RLib.Base
//{
//    public interface IPara:INotifyPropertyChanged, IProtoSerializable
//    {
//        object              Host { get; }
//        PropertyInfo        PI { get; } 

//        string              PropertyName { get; }
//        Type                PropertyType { get; }
//        ParaAttribute       Attribute { get; }
        
//        object              Value { get; set; }                             // 最终值
//        bool                ValueChanged { get; }                           // 相对一开始是否有变化
//        object              StartValue { get; }                             // 用于比较value是否change， 通常在para创建时设置，中间para应用后也可以更新

//        void                Reset(bool toDefault = false);                  // 返回初始值, 或者返回默认值
//    }

//    public static class ParaEx
//    {
//        public static List<ParaAttribute> GetParaAttributes(this Type t)    // 由于系统提供的函数有问题，只能自己实现
//        {
//            List<ParaAttribute> ret = new List<ParaAttribute>();

//            // 遍历t的继承链上的interface等
//            //if (!t.IsInterface)
//            {
//                Type[] intefaces = t.GetInterfaces();
//                foreach (Type i in intefaces)
//                {
//                    ret.AddRange(GetParaAttributes(i));
//                }
//            }

//            // 遍历t的所有property，找到带有attr的属性
//            // 遍历t的集成链上的比如interface等
//            foreach (PropertyInfo pi in t.GetProperties().Where(p => p.IsDefined(typeof(ParaAttribute))))
//            {
//                ret.Add(pi.GetCustomAttribute<ParaAttribute>());
//            }

//            return ret;
//        }


//        //public static List<IPara>  GetParas(this Type t)                   // 由于系统提供的函数有问题，只能自己实现
//        //{
//        //    List<IPara> ret = new List<IPara>();

//        //    // 遍历t的继承链上的interface等
//        //    if (!t.IsInterface)
//        //    {
//        //        Type[] intefaces = t.GetInterfaces();
//        //        foreach (Type i in intefaces)
//        //        {
//        //            ret.AddRange(GetParas(i));
//        //        }
//        //    }


//        //    // 遍历t的所有property，找到带有attr的属性
//        //    // 遍历t的集成链上的比如interface等
//        //    foreach (PropertyInfo pi in t.GetProperties())
//        //    {
//        //        if (!pi.IsDefined(typeof(ParaAttribute)))
//        //            continue;

//        //        ParaAttribute attr = pi.GetCustomAttribute(typeof(ParaAttribute)) as ParaAttribute;
//        //        ret.Add(attr.GetPara(pi));
//        //    }

//        //    return ret;
//        //}

//        public static List<IPara>  GetParas(this object o, Type t = null)                   // 由于系统提供的函数有问题，只能自己实现
//        {
//            List<IPara> ret = new List<IPara>();

//            if (o == null)
//                return ret;

//            if(t == null)
//                t = o.GetType();

//            // 遍历t的继承链上的interface等
//            //if (!t.IsInterface)
//            {
//                Type[] intefaces = t.GetInterfaces();
//                foreach (Type i in intefaces)
//                {
//                    ret.AddRange(GetParas(o, i));
//                }
//            }

//            // 遍历t的所有property，找到带有attr的属性
//            foreach (PropertyInfo pi in t.GetProperties().Where(p=>p.IsDefined(typeof(ParaAttribute))))
//            {
//                //ParaAttribute attr = pi.GetCustomAttribute(typeof(ParaAttribute)) as ParaAttribute;
//                ret.Add(new Para(o, pi));
//            }

//            return ret;
//        }


//         public static void ApplyValues(this IEnumerable<IPara> paras, IEnumerable<KeyValuePair<string, object>> kvs) 
//        {
//            foreach (KeyValuePair<string, object> kv in kvs)
//            {
//                IPara para = paras.FirstOrDefault(p => p.PI.Name == kv.Key);
//                if (para != null)
//                    para.Value = kv.Value;
//            }
//        }
       
//        public static void ApplyDMs(this IEnumerable<IPara> paras, IEnumerable<KVDM> kvs) 
//        {
//            foreach (KVDM kv in kvs)
//            {
//                IPara para = paras.FirstOrDefault(p => p.PI.Name == kv.Key);
//                if (para != null)
//                {
//                    para.Value = kv.Value.Value;
//                    //if(para.NestingParas == null)
//                    //    para.Value = kv.value.Value;
//                    //else
//                    //{
//                    //    para.Value.GetParas().ApplyDM(kv.nestings);  // 赋值nesting paras
//                    //}
//                }
//            }
//        }
//        public static void ApplyDMs(this object o, IEnumerable<KVDM> kvs)
//        {
//            o.GetParas().ApplyDMs(kvs);
//        }


//        public static List<DataModel.KVDM> ToDms(this IEnumerable<IPara> paras) 
//        {
//            List<DataModel.KVDM> l = new List<KVDM>();
//            foreach (IPara p in paras)
//            {
//                KVDM dm = p.ToDm() as KVDM;
//                if(dm != null)
//                l.Add(dm);
//            }

//            return l;
//        }
//    }
    
//    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]       // 只能用于property
//    public class ParaAttribute : Attribute
//    {
//        public string       Name { get; set;}                               // Para显示的名字
//        public string       Desc { get; set; }                  
//        public string       Group { get; set; }                             // para的组别

//        public bool         Show { get; set; }                              // 是否在paragrid中显示

//        public object       Default { get;set; }                            // 默认值

//        // number区间
//        public object       Min { get; set; }
//        public object       Max { get; set; }
//        public object       Step { get; set; }

//        public bool         AsFile { get; set; }                            // value string 作为file
//        public bool         AsDir { get; set; }                             //  value string 作为dir

//        // enum
//        public bool         Multi { get; set; }
   
//        public              ParaAttribute(string name, string desc = "", string group="",  object def = null/*, params object[] presets*/)
//        {
//            Name = name;
//            Desc = desc;
//            Group = group;
//            Default = def;
////            Presets = new List<object>(presets);
//        }

//        //public              ParaAttribute(EProtoDataType type, string name, string desc = "", string group="",  object defVal = null, params object[] presets)
//        //{
//        //    Name = name;
//        //    Desc = desc;
//        //    Group = group;
//        //    DefVal = defVal;
//        //    Presets = new List<object>(presets);
//        //}
        
//        //public              ParaAttribute(string desc = "", string group="",  object defVal = null, params object[] presets)
//        //{
//        //    Desc = desc;
//        //    Group = group;
//        //    DefVal = defVal;
//        //    Presets = new List<object>(presets);
//        //}

//        // numrange
//        //public              ParaAttribute(string desc = "", string group="",  ECompareOp op= ECompareOp.Equal, double l = Double.NaN ,double r =Double.NaN, params object[] presets)
//        //{
//        //    Desc = desc;
//        //    Group = group;
//        //    DefVal = new NumRange(op, l, r);

//        //    if (presets.Count() != 0)
//        //    {
//        //        Presets = new List<object>();
//        //        int n = presets.Count() / 3;
//        //        for (int i = 0; i != n; ++i)
//        //        {
//        //            NumRange nr = new NumRange((ECompareOp)presets.ElementAt(i*3), (double)presets.ElementAt(i*3+1), (double)presets.ElementAt(i*3+2));
                    
//        //            Presets.Add(nr);
//        //        }
                
//        //    }

//        //}

//        //// RColor
//        //public              ParaAttribute(string desc = "", string group = "", uint defVal = 0x00000000, params object[] presets)
//        //{
//        //    Desc = desc;
//        //    Group = group;
//        //    DefVal = new RColor(defVal);
//        //    Presets = new List<object>();
//        //    foreach (uint v in presets)
//        //    {
//        //        Presets.Add(new RColor(v));
//        //    }

//        //}

//        //// fontdm
//        //public              ParaAttribute(string desc = "", string group = "", string fface="宋体", float fsize =12, uint fcode = 0,
//        //   bool fblod= false,bool fitalic = false, bool fline = false,params object[] presets)
//        //{
//        //    Desc = desc;
//        //    Group = group;
//        //    DefVal = new FontDM()
//        //    {
//        //        face = fface,
//        //        size = fsize,
//        //        color = (int)fcode,
//        //        bold = fblod,
//        //        italic = fitalic,
//        //        underline = fline,
//        //    };

//        //    if (presets.Count() != 0)
//        //    {
//        //        Presets = new List<object>();
//        //        int n = presets.Count() / 6;
//        //        for (int i = 0; i != n; ++i)
//        //        {
//        //            FontDM fontDm = new FontDM()
//        //            {
//        //                face = (string)presets.ElementAt(i * 6),
//        //                size = (float)presets.ElementAt(i * 6 + 1),
//        //                color = (int)(uint)presets.ElementAt(i * 6 + 2),
//        //                bold = (bool)presets.ElementAt(i *6 + 3),
//        //                italic = (bool)presets.ElementAt(i * 6 + 4),
//        //                underline = (bool)presets.ElementAt(i * 6 + 5),
//        //            };
//        //            Presets.Add(fontDm);
//        //        }
//        //    }
//        //}


//    }


//    //[AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
//    //public class NestingParaAttribute : ParaAttribute                            // 输出
//    //{
//    //    public override IPara GetPara(PropertyInfo pi)
//    //    {
//    //        Type ot = pi.PropertyType;

//    //        List<IPara> lps = ot.GetParas();
//    //        return new Para(pi, Group, Desc, DefVal)
//    //        {
//    //            IsFile = IsFile,
//    //            IsDir = IsDir,
//    //            Min = Min,
//    //            Max = Max,
//    //            Step = Step,
//    //            NestingParas = lps
//    //        };
//    //    }

//    //    public override IPara GetPara(object host, PropertyInfo pi)
//    //    {
//    //        Type t = pi.PropertyType;

//    //        object o = pi.GetValue(host);  // 内部property对象
           
//    //        List<IPara> lps = o.GetParas();

//    //        return new Para(host, pi, Group, Desc, DefVal, Presets)
//    //        {
//    //            IsFile = IsFile,
//    //            IsDir = IsDir,
//    //            Min = Min,
//    //            Max = Max,
//    //            Step = Step,
//    //            NestingParas = lps,
//    //        };
//    //    }

//    //    public NestingParaAttribute(string name, string desc = "", string group = "")
//    //    : base(name, desc, group)
//    //    {

//    //    }
//    //}
//}
