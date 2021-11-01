/********************************************************************
    created:	2017/5/20 18:25:45
    author:		rush
    email:		
	
    purpose:	

*********************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using NPOI.SS.Formula.Functions;


namespace RLib.Base
{
    public class Factory<TID, T>:HaveIDAndObservable<String>, IFactory<TID, T> where T:IProduct<TID>
    {
        public object       Manager { get { return m_pManager; } }

        public IParams      ProtoParams { get { return m_lprotoParams; } }// 元参数
        public Type         ProductType { get { return typeof(T); } }

        public List<IPara>  Paras { get; protected set; }

        public object       GetPropertyValue(string prop)                   // 获取某个property的值
        {
            if (m_pProtoObj != null)
            {
                //PropertyInfo info = ProductType.GetProperty(prop);

                PropertyInfo info = ProductType.GetProperties().Single(p =>
                    p.Name == prop && p.PropertyType == ProductType);

                if (info != null)
                    return info.GetValue(m_pProtoObj);
            }

            return null;
        }


        object              IFactory.Create(params object[] args)
        {
            return Create(args);
        }

        public new virtual T Create(params object[] args)
        {
            T o =(T)Activator.CreateInstance(ProductType, args);

            return o;
        }


#region C&D
        public              Factory(object man, string id)
            : base(id)
        {
            m_pManager = man;

            // 如果有无参的，创建一个proto对象
            try
            {
                m_pProtoObj =(T)Activator.CreateInstance(ProductType);
            }
            catch (Exception e)
            {
            }
        }
        #endregion

#region Members
        protected object    m_pManager;
        protected IParams   m_lprotoParams;
        protected T         m_pProtoObj;
#endregion
    }

    public class DynamicFactory : HaveIDAndObservable<String>, IDynamicFactory
    {
        public object       Manager { get { return m_pManager; } }
        public Type         ProductType { get { return m_pProductType; } }
        public IParams      ProtoParams { get { return m_lprotoParams; } }// 元参数

        public List<IPara>  Paras { get; protected set; }

        public virtual object    Create(params object[] args)
        {
            object o = null;

            //            o = Activator.CreateInstance(m_pProductType, args);

            //todo:之前写的代码，不记得这样写的理由了
            if (args.Length >= 2) // 代表有id, 有多个参数
            {
                //if(args[1] is IParams)
                o = Activator.CreateInstance(m_pProductType, args);
            }
            else // 无id
            {
                if (args.Length == 1 && !(args[0] is IParams))
                    o = Activator.CreateInstance(m_pProductType, args[0]);
                else
                    o = Activator.CreateInstance(m_pProductType);
            }

            if (o != null)
            {
                // 对nesting property赋初值
                foreach (IPara p in o.GetType().GetParas())
                {
                    if (p.NestingParas != null)
                    {
                        object nest = null;
                        if (p.ValueType.IsInterface)
                            nest = App.Instance.Container.Resolve(p.ValueType);
                        else
                            nest = Activator.CreateInstance(p.ValueType);

                        (p as Para).PI.SetValue(o, nest);
                    }
                }

                // 设置para
                foreach (IPara p in o.GetParas())
                {
                    //object def = RReflector.GetDefaultValue(p.ValueType);

                    // 如果是默认值，设置def
                    if (p.DefValue != null &&
                       object.Equals(p.Value, RReflector.GetDefaultValue(p.ValueType)))
                    {
                        p.Value = p.DefValue;
                    }


                    // 内部复合参数
                    if (p.DefValue==null && p.NestingParas != null)
                    {// 生成一个默认的对象，并对其赋嵌套para

                        foreach (IPara n in p.NestingParas)
                        {
                            n.Value = n.DefValue;
                        }
                    }


                }
            }


            return o;
        }

        public object       GetPropertyValue(string prop)                   // 获取某个property的值
        {
            if (m_pProtoObj != null)
            {
                //PropertyInfo info = ProductType.GetProperty(prop);
                PropertyInfo info = null;

                // 如果有同名prop，取继承最外层
                PropertyInfo[] pis = ProductType.GetProperties();
                foreach (PropertyInfo pi in pis)
                {
                    if (pi.Name == prop)
                    {
                        info = pi;
                        break;
                    }
                }

                if (info != null)
                    return info.GetValue(m_pProtoObj);
            }

            return null;
        }


        public static List<object> GetCustomAttributes(Type t, Type attributeType)  // 由于系统提供的函数有问题，只能自己实现
        {
            List<object> attributes = new List<object>(t.GetCustomAttributes(attributeType, false));

            Type ancestor = t.BaseType;
            if (ancestor != null)
            {
                attributes.AddRange(GetCustomAttributes(ancestor, attributeType));
            }

            return attributes;
        }

        #region C&D
        public              DynamicFactory(object man, Type t)
            : base(t.FullName)
        {
            m_pManager = man;

            //获取类的Attribute


            // 获取Paras
            {
                if (m_pProtoObj != null)
                    Paras = m_pProtoObj.GetParas();
                else
                    Paras = t.GetParas();
            }

            {
                // 父类的同名param会被替换
                List<IParam> lp = new List<IParam>();

                //object[] attrs = System.Attribute.GetCustomAttributes(t,typeof(ParamAttribute),true);
                //object[] attrs = t.GetCustomAttributes(typeof(ParamAttribute), true);
                List<object> attrs = GetCustomAttributes(t, typeof(ParamAttribute));
                foreach (object a in attrs)
                {
                    lp.Add((a as ParamAttribute).Param);
                }
                m_lprotoParams = new Params(lp);
            }


            m_pProductType = t;

            // 如果有无参的，创建一个proto对象
            try
            {
                m_pProtoObj =Activator.CreateInstance(ProductType);
            }
            catch (Exception e)
            {
            }

         
        }
#endregion

        protected object m_pManager;
        protected Type      m_pProductType;
        protected IParams   m_lprotoParams;
        protected object    m_pProtoObj;
    }


    public class AttrFactory : HaveIDAndObservable<String>, IAttrFactory
    {
        public object       Manager { get { return m_pManager; } }
        public Type         ProductType { get { return m_pProductType; } }
        public IParams      ProtoParams { get { return null; } }// 元参数
        public FactoryAttribute    FactoryAttribute { get { return m_pFactoryAttribute; } }

        public List<IPara>  Paras { get; protected set; }

        public virtual object    Create(params object[] args)
        {
                return m_pFactoryAttribute.Create(args);
        }

        public object       GetPropertyValue(string prop)                   // 获取某个property的值
        {
            if (m_pProtoObj != null)
            {
                //PropertyInfo info = ProductType.GetProperty(prop);
                PropertyInfo info = null;

                // 如果有同名prop，取继承最外层
                PropertyInfo[] pis = m_pProtoObj.GetType().GetProperties();
                foreach (PropertyInfo pi in pis)
                {
                    if (pi.Name == prop)
                    {
                        info = pi;
                        break;
                    }
                }

                if (info != null)
                    return info.GetValue(m_pProtoObj);
            }

            return null;
        }


        public static List<object> GetCustomAttributes(Type t, Type attributeType)  // 由于系统提供的函数有问题，只能自己实现
        {
            List<object> attributes = new List<object>(t.GetCustomAttributes(attributeType, false));

            Type ancestor = t.BaseType;
            if (ancestor != null)
            {
                attributes.AddRange(GetCustomAttributes(ancestor, attributeType));
            }

            return attributes;
        }

#region C&D
        public              AttrFactory(object man, Type t, FactoryAttribute facAttr)
            : base(facAttr.ID)
        {
            m_pManager = man;

            m_pProductType = t;

            m_pFactoryAttribute = facAttr;
            // 这里比较特殊
            m_pProtoObj = facAttr;
        }
#endregion

#region Members
        protected FactoryAttribute m_pFactoryAttribute;
        protected object m_pManager;
        protected Type      m_pProductType;
        protected object    m_pProtoObj;
#endregion
    }
    

}
