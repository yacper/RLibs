/********************************************************************
    created:	2017/3/28 11:08:11
    author:		rush
    email:		
	
    purpose:	

*********************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using RLib.Base;
using Wintellect.PowerCollections;

//namespace Rlib.Base
//{
//    public class TsScriptManager:Manager<ulong, ITsScript>, ITsScriptManager
//    {
////#region

////        public override void _OnAdded(IScript s)
////        {
////            base._OnAdded(s);

////            m_dGroups
////        }


////        public override void _OnRemoving(IScript s)
////        {
////            base._OnRemoving(s);
            
////        }


////        #endregion


//        public ICollection<string> FactoryGroups
//        {
//            get
//            {
//                Set<string> groups = new Set<string>();
//                foreach (var f in Factories)
//                {
//                    groups.Add(f.Value.ProtoParams.GetValue<string>("Group"));
//                }

//                return groups;
//            }
//        } // 
//        public ICollection<IFactory> GetFactoriesByGroup(string group)
//        {
//            List<IFactory> lf = new List<IFactory>();
//            foreach (var f in Factories)
//            {
//                if(string.Compare(f.Value.ProtoParams.GetValue<string>("Group"), group) == 0)
//                    lf.Add(f.Value);
//            }

//            return lf;
//        }



//#region C&D
//        public              TsScriptManager(string configureFile = "ScriptsConfigure.xml")
//            :base(configureFile)
//        {
//        }
//#endregion


//#region Members
//#endregion

//    }
//}
