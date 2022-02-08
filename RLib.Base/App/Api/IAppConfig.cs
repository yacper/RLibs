/********************************************************************
    created:	2018/9/14 17:14:10
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
using System.Xml;

namespace RLib.Base
{
    public interface IAppConfig:INotifyPropertyChanged, IProtoSerializable
    {
        string              Path { get; }
        bool                Load();
	    void				Save();

		string				ConfigDir{get;}									// 配置目录
		string				DefaultConfigDir{get;}							// 默认配置目录


#region Xml mode
		bool				IsUsingXmlMode { get; set; }					// 是否使用纯粹的xml模式
        XmlDocument         Document { get; }
        T                   Get<T>(string xPath, T defValue = default(T));
        List<T>             GetList<T>(string xPath);
#endregion
    }
}
