///********************************************************************
//    created:	2018/9/14 17:15:00
//    author:		rush
//    email:		
	
//    purpose:	
//*********************************************************************/
//using System;
//using System.Collections.Generic;
//using System.ComponentModel;
//using System.IO;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using System.Xml;
//using Google.Protobuf;


//namespace RLib.Base
//{
//    public class AppConfig : ObservableObject, IAppConfig
//    {
//	    public virtual IMessage ToDm()
//	    {
//			throw new NotImplementedException();
//	    }
//        public virtual string Path                                          
//        {
//            get
//            {
//                throw new NotImplementedException();
//            }
//        }

//        public virtual bool Load()
//        {
//            try
//            {
//				if(IsUsingXmlMode)
//					_XmlDocument.Load(Path);

//            }
//            catch (Exception e)
//            {
//                RLibBase.Logger.Error(e);

//                return false;
//            }

//            return true;
//        }
//	    public virtual void Save()
//	    {
		    
//	    }

//		public virtual string ConfigDir{get { return ""; } }                              // 配置目录
//		public virtual string DefaultConfigDir{get { return ""; }}						// 默认配置目录


//		public bool			IsUsingXmlMode { get; set; }					// 是否使用纯粹的xml模式
//        public XmlDocument Document { get { return _XmlDocument; } }
//        public void         Set<T>(string xPath, T value)
//        {
//            string key = xPath + "_" + typeof (T);
//            m_dicKeyValues[key] = value;
//        }
//        public T            Get<T>(string xPath, T defValue = default(T))
//        {
//            string key = xPath + "_" + typeof (T);
//            object obj = null;
//            if (m_dicKeyValues.TryGetValue(key, out obj))
//            {
//                return (T) obj;
//            }
//            XmlNode node= _XmlDocument.SelectSingleNode(xPath);
//            if (node != null)
//            {
//                try
//                {
//                    if (typeof(T) == typeof(bool))
//                    {
//                        object b = string.Equals(node.InnerText, "true", StringComparison.CurrentCultureIgnoreCase) ||
//                                 node.InnerText == "1";
//                        m_dicKeyValues.Add(key, b);
//                        return (T)b;
//                    }
//                    else
//                    {
//                        object o = Convert.ChangeType(node.InnerText, typeof(T));
//                        T t = (T)o;
//                        m_dicKeyValues.Add(key, t);
//                        return t;
//                    }
//                }
//                catch (Exception exception)
//                {
//                    RLibBase.Logger.Error(xPath + ", Exception : " + exception);
//                    return defValue;
//                }
//            }
//            return defValue;
//        }
//        public List<T>		GetList<T>(string xPath)
//        {
//            List<T> list = null;
//            string key = xPath + "_" + typeof(T);
//            object obj = null;
//            if (m_dicKeyValues.TryGetValue(key, out obj))
//            {
//                list = (List<T>)obj;
//                return list;
//            }
//            XmlNodeList nodeList = _XmlDocument.SelectNodes(xPath);
//            if (nodeList != null)
//            {
//                list = new List<T>();
//                foreach (XmlNode node in nodeList)
//                {
//                    try
//                    {
//                        object o = Convert.ChangeType(node.InnerText, typeof(T));
//                        list.Add((T)o);
//                    }
//                    catch (Exception exception)
//                    {
//                        RLibBase.Logger.Error(xPath + ", Exception : " + exception);
//                        return list;
//                    }
//                }
//                m_dicKeyValues.Add(key, list);
//            }
//            return list;
//        }

//        protected virtual void _PropertyChanged(object sender, PropertyChangedEventArgs e)
//        {
//			    Save();
//        }


//#region C&D
//	    public				AppConfig()
//	    {
//		    IsUsingXmlMode = false;

//		    if (!string.IsNullOrWhiteSpace(ConfigDir) &&
//				!Directory.Exists(ConfigDir))
//			    Directory.CreateDirectory(ConfigDir);

//		    if (!string.IsNullOrWhiteSpace(DefaultConfigDir) &&
//				!Directory.Exists(DefaultConfigDir))
//			    Directory.CreateDirectory(DefaultConfigDir);

//	        PropertyChanged += _PropertyChanged;
//	    }
//#endregion

//#region Members
//        protected XmlDocument _XmlDocument = new XmlDocument();
//        protected Dictionary<string, object> m_dicKeyValues = new Dictionary<string, object>();
//	    protected IMessage _DM;
//#endregion
//    }
	
//}
