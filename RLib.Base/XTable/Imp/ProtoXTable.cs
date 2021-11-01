/********************************************************************
    created:	2018/4/2 14:14:19
    author:	rush
    email:		
	
    purpose:	excel打包成proto文件后解析
*********************************************************************/
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RLib.Base
{
    class ProtoXTable<TTableArray, TRow>:XTable< TRow>
    {


        public override void OnResourceLoaded(byte[] bytes)
        {
            //MemoryStream ms = new MemoryStream(bytes);

            //m_pTableArray = ProtoBuf.Serializer.Deserialize<TTableArray>(ms);

            //if (OnTableLoadedEvent != null)
            //    OnTableLoadedEvent(this);
        }

    }
}
