/********************************************************************
    created:	2017/4/5 12:21:26
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

namespace RLib.Base
{
    public class PluginManager:Manager<String, IPlugin>, IPluginManager
    {

        public string        Directory { get { return m_strDirectory; } }                              // 插件目录

        //public IPlugin      InstantiateAndAdd(string ID)
        //{
        //    if (Contains(ID) ||
        //        !m_dProfiles.ContainsKey(ID))
        //        return null;


        //    // 实例化一个Type
        //    Type t = m_dProfiles[ID];
        //    object obj = Activator.CreateInstance(t, new object[] {this});
        //    if (obj == null)
        //        return null;

        //    IPlugin p = obj as IPlugin;

        //    Add(p);

        //    return p;
        //}

        //public void      _AddProfile(Type t)                             // 添加一个proto
        //{
        //    if (typeof(IPlugin).IsAssignableFrom(t))
        //    {
        //        //PropertyInfo pi = t.GetProperty("ID");

        //        //object obj = Activator.CreateInstance(t, new object[] { this});
        //        //if (obj == null)
        //        //{
        //        //    Logger.LogError("can't be");
        //        //    return;
        //        //}

        //        //string id = pi.GetValue(obj, null) as string;

        //        string id = t.Name;

        //        if (m_dProfiles.ContainsKey(id))
        //        {
        //            Logger.LogError("两个同名");
        //            return;
        //        }
        //        else
        //        {
        //            m_dProfiles.Add(id, t);
        //        }
        //    }
        //}


        protected string    m_strDirectory; // 插件目录
    }
}
