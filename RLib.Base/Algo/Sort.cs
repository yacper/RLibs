/********************************************************************
    created:	2020/7/2 21:50:38
    author:	rush
    email:		
	
    purpose:排序算法
https://www.cnblogs.com/onepixel/articles/7674659.html
*********************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RLib.Base
{
    public static class Sort
    {
        public static void BubbleSort<T>(this List<T> arr) where T : IComparable //冒泡排序
        {// https://baike.baidu.com/item/%E5%86%92%E6%B3%A1%E6%8E%92%E5%BA%8F/4602306?fr=aladdin
            //比较相邻的元素。如果第一个比第二个大，就交换他们两个。 [1] 
            //对每一对相邻元素做同样的工作，从开始第一对到结尾的最后一对。在这一点，最后的元素应该会是最大的数。 [1] 
            //针对所有的元素重复以上的步骤，除了最后一个。 [1] 
            //持续每次对越来越少的元素重复上面的步骤，直到没有任何一对数字需要比较。
            // O(n^2)

            for (int i = 0; i < arr.Count - 1; i++)
            {
                #region 将大的数字移到数组的arr.Length-1-i
                for (int j = 0; j < arr.Count - 1 - i; j++)
                {
                    if (arr[j].CompareTo(arr[j + 1]) > 0)
                    {
                        T temp = arr[j + 1];
                        arr[j + 1] = arr[j];
                        arr[j] = temp;
                    }
                }
                #endregion
            }
        }

        public static void  QSort<T>(this List<T> arr) where T : IComparable // 快速排序
        {// https://baike.baidu.com/item/%E5%BF%AB%E9%80%9F%E6%8E%92%E5%BA%8F%E7%AE%97%E6%B3%95?fromtitle=%E5%BF%AB%E9%80%9F%E6%8E%92%E5%BA%8F&fromid=2084344
            //快速排序算法通过多次比较和交换来实现排序，其排序流程如下： [2] 
            //(1)首先设定一个分界值，通过该分界值将数组分成左右两部分。 [2] 
            //(2)将大于或等于分界值的数据集中到数组右边，小于分界值的数据集中到数组的左边。此时，左边部分中各元素都小于或等于分界值，而右边部分中各元素都大于或等于分界值。 [2] 
            //(3)然后，左边和右边的数据可以独立排序。对于左侧的数组数据，又可以取一个分界值，将该部分数据分成左右两部分，同样在左边放置较小值，右边放置较大值。右侧的数组数据也可以做类似处理。 [2] 
            //(4)重复上述过程，可以看出，这是一个递归定义。通过递归将左侧部分排好序后，再递归排好右侧部分的顺序。当左、右两个部分各数据排序完成后，整个数组的排序也就完成了。

            QSort(arr, 0, arr.Count - 1);
        }
        /**快速排序 */
        public static void  QSort<T>(List<T> array, int low, int high) where T:IComparable
        {
            if (low >= high)
                return;
            /*完成一次单元排序*/
            int index = QSortUnit(array, low, high);
            /*对左边单元进行排序*/
            QSort(array, low, index - 1);
            /*对右边单元进行排序*/
            QSort(array, index + 1, high);
        }

        /**@param array排序数组 
        **@param low排序起始位置 
        **@param high排序结束位置
        **@return单元排序后的数组 */
        private static int QSortUnit<T>(List<T> array, int low, int high) where T:IComparable
        {
            T key = array[low];
            while (low < high)
            {
                /*从后向前搜索比key小的值*/
                while (array[high].CompareTo(key) >=0 && high > low)
                    --high;
                /*比key小的放左边*/
                array[low] = array[high];
                /*从前向后搜索比key大的值，比key大的放右边*/
                while (array[low].CompareTo(key) <=0 && high > low)
                    ++low;
                /*比key大的放右边*/
                array[high] = array[low];
            }
            /*左边都比key小，右边都比key大。//将key放在游标当前位置。//此时low等于high */
        
            return high;
        }
       

    }



}
