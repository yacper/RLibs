/********************************************************************
    created:	2018/7/20 13:49:38
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
using Google.Protobuf;
using NPOI.SS.Formula.Functions;

namespace RLib.Base
{
    public class Range:IRange
    {
        public override string ToString()
        {
            __CheckValid();

            string ret="";
            if (IsEnd)
            {
                switch (Operator)
                {
                    case ERangeOp.Requal:
                        ret = "=";
                        break;
                    case ERangeOp.RnotEqual:
                        ret = "!=";
                        break;
                    case ERangeOp.Rgreater:
                        ret = ">";
                        break;
                    case ERangeOp.RgreaterEqual:
                        ret = ">=";
                        break;
                    case ERangeOp.Rsmaller:
                        ret = "<";
                        break;
                    case ERangeOp.RsmallerEqual:
                        ret = "<=";
                        break;
                }

                ret += Operand.ToString();
            }
            else
            {
                string left = (Operand as IRange).ToString();
                if (!(Operand as IRange).IsEnd)
                    left = "(" + left + ")";

                string right = Other.ToString();
                if (!Other.IsEnd)
                    right = "(" + right + ")";

                ret = (Operator == ERangeOp.Rand) ? "&&" : "||";
                ret = left + " " + ret + " " +right;
            }

            return ret;
        }

        public object       Clone()
        {
            Range r =new Range(this.ToDm() as RangeDM);

            return r;
        }


        public virtual IMessage ToDm()
        {
            __CheckValid();

            RangeDM dm = new RangeDM();
            dm.Operator = Operator;
            if (IsEnd)
                dm.Operand = new VarDM(Operand);
            else
            {
                dm.Operand2 = (Operand as IRange).ToDm() as RangeDM;
                if(Other != null)
                    dm.Other = Other.ToDm() as RangeDM;
            }

            return dm;
        }

        public ERangeOp     Operator { get; set; }                          // 操作符
        public object       Operand { get; set; }                            // 操作数
        public IRange       Other { get; set; }                             // 另一个操作数，如果存在

        public bool         IsEnd { get { return Operator < ERangeOp.Rand; } } // 是否作为叶子节点

        public bool         IsValid
        {
            get
            {
                if (IsEnd)
                    return Operand != null;
                else
                    return Operand != null && Other != null;
            }
        } // 是否定义正确

        protected bool      __CheckValid()
        {
            if (!IsValid)
            {
                System.Diagnostics.Debug.Assert(false, "错误的定义:" + ToString());  // 开发阶段表明
            }

            return false;
        }


        public bool         Contains(object o)
        {
            __CheckValid();


            switch(Operator)
            {
                case ERangeOp.Requal:
                    return object.Equals(Operand, o);
                    break;
                case ERangeOp.RnotEqual:
                    return !object.Equals(Operand, o);
                    break;
                case ERangeOp.Rgreater:
                    return (o as IComparable).CompareTo(Operand) > 0;
                    break;
                case ERangeOp.RgreaterEqual:
                    return (o as IComparable).CompareTo(Operand) >= 0;
                    break;
                case ERangeOp.Rsmaller:
                    return (Operand as IComparable).CompareTo(o) > 0;
                    break;
                case ERangeOp.RsmallerEqual:
                    return (Operand as IComparable).CompareTo(o) >= 0;
                    break;

                case ERangeOp.Rand:
                    return (Operand as IRange).Contains(o) && Other.Contains(o);
                    break;

                case ERangeOp.Ror:
                    return (Operand as IRange).Contains(o) || Other.Contains(o);
                    break;

            }

            throw new Exception();
        }


#region C&D
        public              Range(ERangeOp op, object operand, IRange other = null)
        {
            Operator = op;
            Operand = operand;
            Other = other;
        }

        public              Range(RangeDM dm)
        {
            Operator = dm.Operator;
            if (IsEnd)
                Operand = dm.Operand.Value;
            else
            {
                Operand = new Range(dm.Operand2);
                Other = new Range(dm.Other);
            }
        }
#endregion
    }
}
