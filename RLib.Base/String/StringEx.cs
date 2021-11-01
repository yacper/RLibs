using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace RLib.Base
{
    public static class StringEx
    {

        public static bool IsNullOrWhiteSpace(this string str)
        {
            return string.IsNullOrWhiteSpace(str);
        }

        public static string Join(this IEnumerable<string> strs, char separator)
        {
            return string.Join(separator, strs);
        }

        public static ulong ToUlong(this string str)
        {
            return Convert.ToUInt64(str);
        }

        public static string TrimEnds(this string str, string ends)
        {
            if (str.EndsWith(ends))
                return str.Substring(0, str.Length - ends.Length);

            return str;
        }

        public static string TrimStarts(this string str, string starts)
        {
            if (str.StartsWith(starts))
                return str.Substring(starts.Length, str.Length - starts.Length);

            return str;
        }

	    public static T     ParseNumberSimple<T>(this string str, int? precesion = null)           // 简单版速度快(100万测试比下面正则版快6倍)，但是有瑕疵
        {
            // 由于正则表达的方法太慢了，不适应使用率非常高的场合
            // 数字的形式应该满足 "^[+-]?\d*[.]?\d*$"

            T result = default(T);

            if (!string.IsNullOrEmpty(str))
            {
                bool frac = false;   // 是否是小数
                StringBuilder sb = new StringBuilder();

                foreach (char c in str)
                {
                    int cn = Convert.ToInt32(c);
                    if ((cn >= 48 && cn <= 57))  // 数字
                    {
                        sb.Append(c);
                    }
                    else if (cn == 45)// 减号 （加号对结果不影响，不考虑）
                    {
                        if (sb.Length == 0)
                            sb.Append(c);           // 减号只有在开始才成立，否则忽略
                    }
                    else if (cn == 46) // 小数点
                    {
                        if (!frac) // 小数点只有一
                        {
                            frac = true;
                            sb.Append(c);
                        }
                        else
                            return result;   // 两个小数点，判为错误
                    }
                }

                result = (T) Convert.ChangeType(sb.ToString(), typeof(T));

                if (precesion != null)
                {
                    if (typeof(T).Name == "Single" ||
                        typeof(T).Name == "Double"
                    )
                    {
                        double r2 = (double)Convert.ChangeType(result, typeof(double));
                        r2 = Math.Round(r2, precesion.Value);

                        result = (T)Convert.ChangeType(r2, typeof(T));
                    }
                }
            }

            return result;
        }


	    public static T     ParseNumber<T>(this string str, int? precesion = null)  // 取整个string的里面的所有数字
        {
            T result = default(T);
            if (!string.IsNullOrEmpty(str))
            {
                // 正则表达式剔除非数字字符（不包含小数点.）
                //str = Regex.Replace(str, @"[^/d./d]", "");
                str = Regex.Replace(str, @"[^\d.\d]", "");
                if (string.IsNullOrEmpty(str))
                {
                    return result;
                }
                // 如果是数字，则转换为double类型
                //if (Regex.IsMatch(str, @"^[+-]?\d*[.]?\d*$"))
                if (Regex.IsMatch(str, @"^[+-]?\d*[.]?\d+$"))  // 至少有一个数字
                {
                    result = (T)Convert.ChangeType(str, typeof(T));

                    if (precesion != null)
                    {
                        if (typeof(T).Name == "Single" ||
                            typeof(T).Name == "Double"
                        )
                        {
                            double r2 = (double)Convert.ChangeType(result, typeof(double));
                            r2 = Math.Round(r2, precesion.Value);

                            result = (T)Convert.ChangeType(r2, typeof(T));
                        }
                    }

                }
            }
            return result;
        }

        public static T     ParseNumberEnd<T>(this string str, int? precesion = null) // 取string最后的number
        {
            T result = default(T);
            if (string.IsNullOrWhiteSpace(str))
                return result;

            Regex r = new Regex(@"[+-]?\d*[.]?\d+$");
            var ms = r.Matches(str);
            if (ms.Count > 0)
            {
                string num = (ms.OfType<Match>().Last()).ToString();

                result = (T)Convert.ChangeType(num, typeof(T));

                if (precesion != null)
                {
                    if (typeof(T).Name == "Single" ||
                        typeof(T).Name == "Double"
                    )
                    {
                        double r2 = (double)Convert.ChangeType(result, typeof(double));
                        r2 = Math.Round(r2, precesion.Value);

                        result = (T)Convert.ChangeType(r2, typeof(T));
                    }
                }
            }

            return result;
        }

        public static T     ParseNumberStart<T>(this string str, int? precesion = null) // 取string开头的number
        {
            T result = default(T);
            if (string.IsNullOrWhiteSpace(str))
                return result;

            Regex r = new Regex(@"^[+-]?\d*[.]?\d+");
            var ms = r.Matches(str);
            if (ms.Count > 0)
            {
                string num = (ms.OfType<Match>().Last()).ToString();

                result = (T)Convert.ChangeType(num, typeof(T));

                if (precesion != null)
                {
                    if (typeof(T).Name == "Single" ||
                        typeof(T).Name == "Double"
                    )
                    {
                        double r2 = (double)Convert.ChangeType(result, typeof(double));
                        r2 = Math.Round(r2, precesion.Value);

                        result = (T)Convert.ChangeType(r2, typeof(T));
                    }
                }
            }

            return result;
        }

        public static string TrimeAllWhiteSpace(this string str)
        {
            if (str == null)
                return null;

            str = str.Trim();
            str = str.Replace(" ", "");

            return str;
        }
  

        public static void SaveFile(this string content, string path)
        {
            File.WriteAllText(path, content);
        }


    }
}
