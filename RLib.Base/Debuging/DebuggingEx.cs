/********************************************************************
    created:	2018/1/2 19:54:16
    author:	rush
    email:		
	
    purpose:	
*********************************************************************/
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace RLib.Base
{
    public static class DebuggingEx
    {
        //public static bool CheckEmpty(ICollection c)
        //{
        //    if (c.Count == 0)
        //    {
        //        throw new Exception("Empty Collection");
        //        return true;
        //    }

        //    return false;
        //}

        public static void EnsureArgRange<T>(T val, T min, T max) where T:IComparable            // arg 符合range
        {
            if (val.CompareTo(min) < 0||
                val.CompareTo(max) > 0)
                throw new ArgumentException(string.Format("Bad Args [%0,%1]:%2", min, max, val));
        }

        public static void EnsureNonempty<T>(T col) where T:ICollection        // collection 非空
        {
            if (col.Count == 0)
                throw new ArgumentException(string.Format("Collecion is Empty!"));
        }

        public static string GetStack(int? n)                                // 打印最近多少个堆栈
        {
            StackTrace st = new StackTrace(1, true);

            if (n == null)
            {
                return st.ToString();
            }
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i != n; ++i)
            {
                StackFrame frame = st.GetFrame(i);//1代表上级，2代表上上级，以此类推

                sb.Append(frame.ToString());
            }

            return sb.ToString();
        }
        public static string GetStackUntilFunc(string func)                 // 打印最近多少个堆栈
        {
            StackTrace st = new StackTrace(1, true);

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i != st.FrameCount; ++i)
            {
                StackFrame frame = st.GetFrame(i);//1代表上级，2代表上上级，以此类推
                sb.Append(frame.ToString());

                if(frame.GetMethod().Name.Contains(func))
                    break;
            }

            return sb.ToString();
        }

    }
}
