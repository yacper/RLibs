///********************************************************************
//    created:	2017/5/20 18:02:32
//    author:		rush
//    email:		
	
//    purpose:	manager 弄了2个，observable版本的，在数量上万的时候，效率极低，所以大量的情况下，不能用
//*********************************************************************/
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace RLib.Base
//{
//    public class ConfiguredInstance:HaveIDAndObservable<string>
//    {
//        public string       Facotry { get { return m_strFactory; } }
//        public IParams       Params { get { return m_pParams; } }

//#region C&D
//        public              ConfiguredInstance(string id, string factory, IParams p)
//            : base(id)
//        {
//            m_strFactory = factory;
//            m_pParams = p;
//        }
//#endregion
        
//        protected string    m_strFactory;
//        protected IParams   m_pParams;
//    }


//    public interface IManager<TID, T>:IReadOnlyDictionary<TID, T>, IGottaInit where T : IProduct<TID>
//    {
//#region Configured instance
//        string              ConfigureFile { get; }
//        IObservableDictionary<string, ConfiguredInstance> Configured { get; }     // 已经配置好的instance
//        void                ReadConfig();
//        void                WriteConfig();
//#endregion

//        IDictionary<string, IFactory> Factories { get; }                // facotry 


//        T                   Instantiate(string factory,bool init, params object[] args); // 是否对其进行init
//        T                   Instantiate(string factory, params object[] args); // 是否对其进行init

//        T                   InstantiateAndAdd(string factory, params object[] args);
//        T                   InstantiateAndAdd(string factory,bool init, params object[] args); // 是否对其进行init

//        void                UnInInstantiateAndRemove(TID key);
//        void                UnInInstantiateAndRemove(T val);

//        void                UnInInstantiateAndRemoveAll();
//    }

//    public interface IObservableManager<TID, T>:IReadonlyObservableDictionary<TID, T>, IGottaInit where T : IProduct<TID>
//    {
//#region Configured instance
//        string              ConfigureFile { get; }
//        IObservableDictionary<string, ConfiguredInstance> Configured { get; }     // 已经配置好的instance
//        void                ReadConfig();
//        void                WriteConfig();
//#endregion

//        IObservableDictionary<string, IFactory> Factories { get; }                // facotry 


//        T                   Instantiate(string factory,bool init, params object[] args); // 是否对其进行init
//        T                   Instantiate(string factory, params object[] args); // 是否对其进行init

//        T                   InstantiateAndAdd(string factory, params object[] args);
//        T                   InstantiateAndAdd(string factory,bool init, params object[] args); // 是否对其进行init

//        void                UnInInstantiateAndRemove(TID key);
//        void                UnInInstantiateAndRemove(T val);

//        void                UnInInstantiateAndRemoveAll();
//    }
//}
