/********************************************************************
    created:	2018/7/20 11:22:50
    author:		rush
    email:		
	
    purpose:	代表一个区间, 区间可以组合区间，以致无穷
                区间既可以是一个终节点，也可以作为一个

                range 对象必须实现IComparable
*********************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataModel;

namespace RLib.Base
{
    public interface IRange: ISerializable, ICloneable
    {
        string              ToString();



        ERangeOp            Operator { get; set; }                          // 操作符
        object              Operand { get; set; }                            // 操作数
        IRange              Other { get; set; }                             // 另一个操作数，如果存在

        bool                IsEnd { get; }                                  // 是否作为叶子节点
        bool                IsValid { get; }                                // 是否定义正确

        bool                Contains(object o);                             // 是否包含一个元素
//        bool                Contains(IRange r);                             // 这个实现有困难, 暂不支持
    }
}
