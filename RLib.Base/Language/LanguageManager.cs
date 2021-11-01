/********************************************************************
    created:	2018/12/12 20:05:59
    author:		rush
    email:		
	
    purpose:	
*********************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataModel;

namespace RLib.Base
{
    public class LanguageManager:GottaInit, ILanguageManager
    {
        public override void OnIniting()
        {

        }

        public ELanguage       CurLanguage
        {
            get { return _CurLanguage;}
            set { Set("CurLanguage", ref _CurLanguage, value); }
        }

        public string   GetString(int index, ELanguage? e)
        {
            if (e == null)
                e = CurLanguage;

            if (_Dics.ContainsKey(index))
                return _Dics[index].Lans.FirstOrDefault(p => p.Key == e).Value;
            else
                return null;
        }

        public string   GetString(string str, ELanguage? want = null)
        {
            if (want == null)
                want = CurLanguage;

            foreach (KeyValuePair<ELanguage, Dictionary<string, LanStr>> kv in _indexs)
            {
                if (kv.Value.ContainsKey(str))
                    return kv.Value[str].Lans.FirstOrDefault(p => p.Key == want).Value;

            }

            return null;
        }

        public string       GetString(string str, ELanguage e, ELanguage? want = null)
        {
            if (want == null)
                want = CurLanguage;

            if (_indexs.ContainsKey(e) && _indexs[e].ContainsKey(str))
            {
                return _indexs[e][str].Lans.FirstOrDefault(p => p.Key == want).Value;
            }

            return null;
        }


        public bool            LoadFile(string excelFile)
        {
            try
            {
                IList<LanStrDM> l = excelFile.FromCsv<LanStrDM>();

                foreach (LanStrDM dm in l)
                {
                    if(dm.Index == 0)
                        continue;

                    try
                    {
                        LanStr ls = new LanStr(dm);
                        _Dics.Add(dm.Index, ls);

                        foreach (KeyValuePair<ELanguage, string> kv in ls.Lans)
                        {
                            if(string.IsNullOrWhiteSpace(kv.Value))
                                continue;

                            if (!_indexs.ContainsKey(kv.Key))
                                _indexs.Add(kv.Key, new Dictionary<string, LanStr>());

                            _indexs[kv.Key].Add(kv.Value, ls);
                        }

                    }
                    catch (Exception e)
                    {
                        RLibBase.Logger.Error(e);
                    }
                }

                return true;
            }
            catch (Exception e)
            {
                RLibBase.Logger.Error(e);
                return false;
            }
        }


#region Members
        protected Dictionary<int, LanStr> _Dics = new Dictionary<int, LanStr>();
        protected Dictionary<ELanguage, Dictionary<string, LanStr>> _indexs = new Dictionary<ELanguage, Dictionary<string, LanStr>>();

        protected ELanguage _CurLanguage = ELanguage.English;
#endregion
    }
}
