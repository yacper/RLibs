///********************************************************************
//    created:	2017/5/20 18:02:13
//    author:		rush
//    email:		
	
//    purpose:	

//*********************************************************************/
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace RLib.Base
//{
//    public interface IFactory :IHaveID<string>                              // 子类Factory
//    {
//        object              Manager { get; }

//        Type                ProductType { get; }
//        IParams             ProtoParams { get; }                            // 元参数

//        List<IPara>         Paras { get; }

//        object              GetPropertyValue(string prop);                  // 获取某个property的值, 可能无法获取

//        object              Create(params object[] args);
//    }


//    public interface IFactory<TID, T>:IFactory where T:IProduct<TID>        // 静态factory, 可自己定义, 自己添加, 主要用来替代不能用反射的情况(主要就ios)
//    {
//        new T               Create(params object[] args);
//    }

//    public interface IDynamicFactory : IFactory         // 动态factory, 通过反射获得
//    {

//    }

//    public interface IAttrFactory :IFactory                          // 类中Member的attribute方式
//    {
//        FactoryAttribute    FactoryAttribute { get; }
//    }


//    [AttributeUsage(AttributeTargets.Property|AttributeTargets.Method, AllowMultiple = true)] // 只能用于property
//    public class FactoryAttribute : Attribute
//    {
//        public string       ID { get; protected set; }


//        public virtual object Create(object host = null, params object[] methodParas)
//        {
//            throw new NotImplementedException();
//        }


//#region C&D
//        public              FactoryAttribute(string id)
//        {
//            ID = id;
//        }
//#endregion
//    }
//}
