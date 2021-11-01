/********************************************************************
    created:	2017/5/20 16:06:26
    author:		rush
    email:		
	
    purpose:	

*********************************************************************/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RLib.Base
{
    public enum EInitState      // 初始化状态 , 只能从1往后走，不能回头
    {
        None        = 0,                // 没有初始化
        Initing     = 2,                // 初始化中
        Inited      = 4,                // 已经初始化了
        Uniniting   = 8,                // 开始反初始化
        Uninited    = 16                // 已经反初始化了
    }
    public interface IGottaInit:INotifyPropertyChanged, INotifyPropertyChanging
    {
#region properties
        EInitState          InitStat { get; }                         // init 状态
#endregion


#region vituals
        void                Init();
        void                UnInit();

	    void				OnIniting();
	    void				OnUnIniting();

#endregion
    }
}
