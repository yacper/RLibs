/********************************************************************
    created:	2017/12/20 14:18:22
    author:	rush
    email:		
	
    purpose:	
*********************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Google.Protobuf;

namespace RLib.Base
{
    public interface IProtoSerializable
    {
        IMessage         ToDm();                                         // 序列化成protobufdm
    }

    public interface  IStringSerializable
    {
        string              SerializeString();                              // 序列化成string
        object              DeserializeString(string str);                  // string反序列化
    }
 
}
