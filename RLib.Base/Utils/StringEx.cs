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

        public static ulong ToUlong(this string str)
        {
            return Convert.ToUInt64(str);
        }

        public static string Join(this IEnumerable<string> strs, char separator)
        {
            return string.Join(separator, strs);
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

        public static bool  ContainsChinese(this string str)                // 是否包含中文
        {
            return Regex.IsMatch(str, @"[\u4e00-\u9fa5]");
        }

        public static bool  ContainsEnglish(this string str)                // 是否包含英文
        {
            return Regex.IsMatch(str, "[a-zA-Z]");
        }

        public static bool  IsEnglishLetter(this char c)                // 是否包含英文
        {
            return (c >= 'a' && c <= 'z') ||
                   c >= 'A' && c <= 'Z';
        }


        public static float Similarity(string str1, string str2)            // 计算2字符串的相似度0-1
        {
            str1 = str1.Trim().ToUpper();
            str2 = str2.Trim().ToUpper();

            ArrayList pairs1 = _WordLetterPairs(str1);
            ArrayList pairs2 = _WordLetterPairs(str2);
            float intersection = 0;
            int union = pairs1.Count + pairs2.Count;
            bool wordStartSim = false;
            for (int i = 0; i < pairs1.Count; i++)
            {
                string pair1 = pairs1[i] as string;
                for (int j = 0; j < pairs2.Count; j++)
                {
                    string pair2 = pairs2[j] as string;
                    if (pair1.Equals(pair2))
                    {
                        intersection++;
                        pairs2.Remove(j);
                        break;
                    }
                    else if (pair1.StartsWith(pair2) || pair2.StartsWith(pair1))
                    {
                        wordStartSim = true;
                    }
                }
            }


            // 提高开头匹配的权重
            float x = 2.0f * intersection;
            if ((intersection * 2) < union &&
                (str1.StartsWith(str2) || str2.StartsWith(str1))
            )
                x += 0.9f;

            // 如果存在中间开头匹配, 在给一点权重
            if(wordStartSim)
                x += 0.25f;

            return x / (float)union ;
        }
        private static string[] _LetterPairs(string str)
        {
            // 判断是否是中文，如果是中文，认为2个相邻字符是组成一个新的词组，英文则不需要

            if (str.ContainsChinese() && str.Length >1) // 如果中文才两两组词
            {
                // 可能同时存在中英文
                List<string> pairs = new List<string>();
                int engStart = 0;
                int engLength = 0;
                for (int i = 0; i < str.Length - 1; i++)
                {
                    if (str[i].IsEnglishLetter())  // 包含英文成分
                    {
                        if (engStart == 0)
                            engStart = i;

                       engLength++;
                    }
                    else
                    {
                        if (engStart != 0)
                        {
                            pairs.Add(str.Substring(engStart, engLength));
                            engStart = 0;
                            engLength = 0;
                        }

                        if(!str[i+1].IsEnglishLetter())
                            pairs.Add(str.Substring(i, 2));
                        else if (pairs.Any() && pairs.Last().Contains(str[i]))
                        {
                        }
                        else
                            pairs.Add(str.Substring(i, 1));
                    }
                }

                return pairs.ToArray();
            }
            else
                return new []{str};
        }

        private static ArrayList _WordLetterPairs(string str)
        {
            ArrayList allPairs = new ArrayList();
            
            string[] words = Regex.Split(str, "\\s");
            for (int w = 0; w < words.Length; w++)
            {
                // 判断是否是中文，如果是中文，认为2个相邻字符是组成一个新的词组，英文则不需要
                string[] pairsInWord = _LetterPairs(words[w]);
                for (int p = 0; p < pairsInWord.Length; p++)
                {
                    allPairs.Add(pairsInWord[p]);
                }
            }
            return allPairs;
        }


        /// <summary>  
        /// 比较字符串相似度算法：编辑距离（Levenshtein Distance）  
        /// </summary>  
        /// <param name="source">源串</param>  
        /// <param name="target">目标串</param>  
        /// <param name="similarity">输出：相似度，值在0～１</param>  
        /// <param name="isCaseSensitive">是否大小写敏感</param>  
        /// <returns>源串和目标串之间的编辑距离</returns>  
        public static Int32 LevenshteinDistance(String source, String target, out double similarity, bool isCaseSensitive = false)
        {
            if (String.IsNullOrEmpty(source))
            {
                if (String.IsNullOrEmpty(target))
                {
                    similarity = 1;
                    return 0;
                }
                else
                {
                    similarity = 0;
                    return target.Length;
                }
            }
            else if (String.IsNullOrEmpty(target))
            {
                similarity = 0;
                return source.Length;
            }

            String From, To;
            if (isCaseSensitive)
            {   // 大小写敏感  
                From = source;
                To = target;
            }
            else
            {   // 大小写无关  
                From = source.ToLower();
                To = target.ToLower();
            }

            // 初始化  
            Int32 m = From.Length;
            Int32 n = To.Length;
            Int32[,] H = new Int32[m + 1, n + 1];
            for (Int32 i = 0; i <= m; i++) H[i, 0] = i;  // 注意：初始化[0,0]  
            for (Int32 j = 1; j <= n; j++) H[0, j] = j;

            // 迭代  
            for (Int32 i = 1; i <= m; i++)
            {
                Char SI = From[i - 1];
                for (Int32 j = 1; j <= n; j++)
                {   // 删除（deletion） 插入（insertion） 替换（substitution）  
                    if (SI == To[j - 1])
                        H[i, j] = H[i - 1, j - 1];
                    else
                        H[i, j] = Math.Min(H[i - 1, j - 1], Math.Min(H[i - 1, j], H[i, j - 1])) + 1;
                }
            }

            // 计算相似度  
            Int32 MaxLength = Math.Max(m, n);   // 两字符串的最大长度  
            similarity = ((Double)(MaxLength - H[m, n])) / MaxLength;

            return H[m, n];    // 编辑距离  
        }

        public static bool  LevenshteinMatch(string source, string target, double similarity, bool isCaseSensitive = false)
        {
            double sim = 0;
            StringEx.LevenshteinDistance(source, target, out sim, isCaseSensitive);
            return (sim >= similarity);
        }


        public static void SaveFile(this string content, string path)
        {
            File.WriteAllText(path, content);
        }

        public static byte[] ToBytesUtf8(this string str)
        {
            return System.Text.Encoding.UTF8.GetBytes(str);
        }

    }
}
