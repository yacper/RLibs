///********************************************************************
//    created:	2017/5/20 16:09:32
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
//    public class GottaInit: ObservableObject, IGottaInit
//    {
//#region IGottaInit
//#region properties
//        public EInitState   InitStat { get { return _InitStat; } set { Set("InitStat", ref _InitStat, value); } }                         // init 状态
//#endregion

//#region virtuals
//	    public virtual void Init()
//	    {
//		    InitStat = EInitState.Initing;

//			OnIniting();

//		    InitStat = EInitState.Inited;
//	    }

//	    public virtual void UnInit()
//	    {
//		    InitStat = EInitState.Uniniting;

//			OnUnIniting();

//		    InitStat = EInitState.Uninited;
//	    }

//	    public virtual void	OnIniting(){}
//	    public virtual void	OnUnIniting(){}
//#endregion

//#region Members
//	    protected EInitState _InitStat;
//#endregion

//#endregion
//    }
//}
