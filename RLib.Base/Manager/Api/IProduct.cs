///********************************************************************
//    created:	2017/5/20 17:57:35
//    author:		rush
//    email:		
	
//    purpose:	产品
// *              工厂生出出产品，然后交由Manager管理

//*********************************************************************/
//using System;
//using System.Collections.Generic;
//using System.Text;
//using System.Threading.Tasks;

//namespace RLib.Base
//{
//    public interface IProduct<T>:IHaveIDAndObservable<T>, IGottaInit 
//    {
//        object              Manager { get; }
//        IFactory            Factory { get; }

//#region Dynamic factory 生成
//        void                _SetManager(object m);
//        void                _SetFactory(IFactory f);
//#endregion
//    }
//    public interface IDynamicProduct<T> : IProduct<T>
//    {
//        IParams             Params { get; }                                 // 如果有(由dynamic factory生成的，都有该属性)

//        void                _SetParams(IParams p);
//    }
//}
