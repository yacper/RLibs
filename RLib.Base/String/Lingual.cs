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
    public static class Lingual
    {
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
        public static Int32 LevenshteinDistance(string source, string target, out double similarity, bool isCaseSensitive = false)
        {
            if (string.IsNullOrWhiteSpace(source))
            {
                if (string.IsNullOrEmpty(target))
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
            else if (string.IsNullOrEmpty(target))
            {
                similarity = 0;
                return source.Length;
            }

            System.String From, To;
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
            LevenshteinDistance(source, target, out sim, isCaseSensitive);
            return (sim >= similarity);
        }


       

    }
}
	
