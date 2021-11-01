/********************************************************************
    created:	2018/12/12 19:48:14
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
using DataModel;

namespace RLib.Base
{
    public enum ELanguage
    {
        Internal,       // 内部代号
        English,
        Chinese,
    }

    public class LanStrDM
    {
        public int          Index { get; set; }
        public string       Interior { get; set; }
        public string       Eng { get; set; }
        public string       Ch { get; set; }
        public override string ToString()
        {
            return Index +"-"+ Interior +":"+Eng +"|"+ Ch;
            //return this.ConvertToString((string)null, (IFormatProvider)null);
        }
    }

    public struct LanStr
    {
        public int Index { get;  }
        public IEnumerable<KeyValuePair<ELanguage, string>> Lans { get;  }

        public          LanStr(int index, IEnumerable<KeyValuePair<ELanguage, string>> lans)
        {
            Index = index;
            Lans = lans;
        }

        public          LanStr(LanStrDM dm)
        {
            Index = dm.Index;
            Lans = new List<KeyValuePair<ELanguage, string>>()
            {
            };

            if(dm.Interior.IsNullOrWhiteSpace())
                (Lans as List<KeyValuePair<ELanguage, string>>).Add(new KeyValuePair<ELanguage, string>(ELanguage.Internal, dm.Interior));
            if(dm.Eng.IsNullOrWhiteSpace())
                (Lans as List<KeyValuePair<ELanguage, string>>).Add(new KeyValuePair<ELanguage, string>(ELanguage.English, dm.Eng));
            if(dm.Ch.IsNullOrWhiteSpace())
                (Lans as List<KeyValuePair<ELanguage, string>>).Add(new KeyValuePair<ELanguage, string>(ELanguage.Chinese, dm.Ch));
        }
    }

    public interface ILanguageManager:IGottaInit
    {
        ELanguage       CurLanguage { get; set; }

        bool            LoadFile(string excelFile);

        string          GetString(int index, ELanguage? e = null);

        string          GetString(string str, ELanguage? want = null);
        string          GetString(string str, ELanguage e, ELanguage? want = null);

    }
}
