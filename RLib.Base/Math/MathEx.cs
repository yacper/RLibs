/********************************************************************
    created:	2017/4/25 12:08:59
    author:		rush
    email:		
	
    purpose:	

*********************************************************************/

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace RLib.Base
{
public static class MathEx
{
    public const float Epsilon       = 1e-005f;
    public const float DoubleEpsilon = 1e-007f;

    public static int Random(int min, int max) // 从包括min和不包括max中，获取一个随机的值
    {
        Debug.Assert(min <= max);

        if (min == max)
            return min;

        Random r     = new Random(Guid.NewGuid().GetHashCode());
        int    delay = r.Next(min, max);

        return delay;
    }


    public static bool Equal(string a, string b)
    {
        if (string.Compare(a, b) == 0)
            return true;
        else
            return false;
    }

    public static bool Equal(object a, object b)
    {
        if (object.Equals(a, b))
            return true;
        else
            return false;
    }

    public static bool Equal(bool a, bool b)
    {
        if (a == b)
            return true;
        else
            return false;
    }

    public static bool Equal(long a, long b)
    {
        if (a == b)
            return true;
        else
            return false;
    }

    public static bool Equal(float a, float b)
    {
        if (Math.Abs(a - b) <= Epsilon)
            return true;
        else
            return false;
    }

    public static bool Equal(double a, double b)
    {
        if (Math.Abs(a - b) <= Epsilon)
            return true;
        else
            return false;
    }

    public static float  Lerp(float  value1, float  value2, float  amount) { return value1 + (value2 - value1) * amount; }
    public static double Lerp(double value1, double value2, double amount) { return value1 + (value2 - value1) * amount; }

    public static bool IsIntegear(double value) { return value - Math.Floor(value) < Epsilon; }

    public static string ToSimpleFormat(this double v)
    {
        bool negtive = false;
        if (v < 0)
        {
            v       = Math.Abs(v);
            negtive = true;
        }

        string ret;

        var num = 1000;

        if (v < num)
            ret = v.ToString();
        else if (v < Math.Pow(num, 2)) // 小于m的
        {
            double v2 = v / num;
            if (MathEx.IsIntegear(v2))
                ret = v2 + "K";
            else
                ret = v.ToString();
        }
        else // 超过m的
        {
            double v2 = v / (num * num);
            if (MathEx.IsIntegear(v2))
                ret = v2 + "m";
            else
                ret = v.ToString();
        }

        if (negtive)
            return "-" + ret;
        else
            return ret;
    }


    public static double ToDegrees(double radians)
    {
        // This method uses double precission internally,
        // though it returns single double
        // Factor = 180 / pi
        return (double)(radians * 57.295779513082320876798154814105);
    }

    public static double ToRadians(double degrees)
    {
        // This method uses double precission internally,
        // though it returns single double
        // Factor = pi / 180
        return (double)(degrees * 0.017453292519943295769236907684886);
    }

    public static bool IsPowerOfTwo(int value) { return (value > 0) && ((value & (value - 1)) == 0); }

    public static int Clamp(int value, int min, int max)
    {
        Debug.Assert(min <= max);

        return Clamp<int>(value, min, max);
    }

    public static T Clamp<T>(T value, T min, T max) where T : IComparable
    {
        Debug.Assert(min.CompareTo(max) <= 0);

        // First we check to see if we're greater than the max
        value = (value.CompareTo(max) > 0) ? max : value;

        // Then we check to see if we're less than the min.
        value = (value.CompareTo(min) < 0) ? min : value;

        // There's no check to see if min > max.
        return value;
    }

    public static double Clamp(double value, double min, double max) { return Clamp<double>(value, min, max); }
    public static float  Clamp(float  value, float  min, float  max) { return Clamp<float>(value, min, max); }

    public static bool IsBetween<T>(T value, T min, T max) where T : IComparable
    {
        if (value.CompareTo(min) >= 0 && value.CompareTo(max) <= 0)
            return true;
        else
            return false;
    }


#region 对一系列数据
    public static T Min<T>(this IReadOnlyList<T> source, int from, int to) where T : IComparable // 获取最小
    {
          Debug.Assert(from <= to &&
                     from >= 0 && 
                     to < source.Count);

          if (to >= source.Count)
            to = source.Count - 1;

        T min = source[from];
        for (int i = from; i <= to; ++i)
        {
            if (source[i].CompareTo(min) < 0)
                min = source[i];
        }

        return min;
    }
    public static T Min<T>(this IReadOnlyList<T> source, int count) where T : IComparable // 获取最小
    {
        return source.Min(source.Count - count, source.Count - 1);
    }
    public static T Min<T>(this ICollection<T> source) where T : IComparable // 获取最小
    {
        T min = default(T);
        foreach (T v in source)
        {
            if (min.CompareTo(default(T)) == 0)
                min = v;

            if (min.CompareTo(v) < 1)
                min = v;
        }

        return min;
    }

    public static double Min(this IEnumerable<double> source) // 获取最小
    {
        double min = double.PositiveInfinity;
        foreach (double v in source)
        {
            if (double.IsNaN(v))
                continue;

            if (min.CompareTo(v) < 1)
                min = v;
        }

        if (double.IsPositiveInfinity(min))
            return double.NaN;

        return min;
    }
    public static double Min(this IReadOnlyList<double> source, int from, int to, out int index)
    {
        Debug.Assert(from <= to &&
                     from >= 0); 
                     //to < source.Count);

            // 为了兼容
                     if (to >= source.Count)
                         to = source.Count - 1;

        index = -1;
        int    outIndex = 0;
        double min      = double.PositiveInfinity;

        for (int i = from; i <= to; ++i)
        {
            if (double.IsNaN(source[i]))
                continue;

            if (source[i].CompareTo(min) < 0)
            {
                min      = source[i];
                outIndex = i;
            }
        }

        if (double.IsPositiveInfinity(min))
            return double.NaN;

        index = outIndex;
        return min;
    }
    public static double Min(this IReadOnlyList<double> source, int from, int to)
    {
        int index = 0;
        return Min(source, from, to, out index);
    }
    public static double Min(this IReadOnlyList<double> source, int count)
    {
        return source.Min(source.Count - count, source.Count - 1);
    }


    public static T Max<T>(this IReadOnlyList<T> source, int from, int to) where T : IComparable // 获取最大
    {
        Debug.Assert(from <= to &&
                     from >= 0 && from < source.Count &&
                     to >= 0);

        if (to >= source.Count)
            to = source.Count - 1;


        T max = source[from];
        for (int i = from; i <= to; ++i)
        {
            if (max.CompareTo(source[i]) < 0)
                max = source[i];
        }

        return max;
    }
    public static T Max<T>(this IReadOnlyList<T> source, int n) where T : IComparable // 获取最近的n的最大值
    {
        return source.Max(source.Count - n, source.Count - 1);
    }

    public static double Max(IEnumerable<double> source)
    {
        double max = Double.NegativeInfinity;
        foreach (double v in source)
        {
            if (double.IsNaN(v))
                continue;

            if (max.CompareTo(v) < 0)
                max = v;
        }

        if (double.IsNegativeInfinity(max))
            return double.NaN;

        return max;
    }
    public static double Max(this IReadOnlyList<double> source, int from, int to, out int index)
    {
        Debug.Assert(from <= to &&
                     from >= 0); 
                     //to < source.Count);
            // 为了兼容
                     if (to >= source.Count)
                         to = source.Count - 1;


        index = -1;
        int    outIndex = 0;
        double max      = double.NegativeInfinity;
        for (int i = from; i <= to; ++i)
        {
            if (double.IsNaN(source[i]))
                continue;

            if (max.CompareTo(source[i]) < 0)
            {
                max      = source[i];
                outIndex = i;
            }
        }

        if (double.IsNegativeInfinity(max))
            return double.NaN;

        index = outIndex;
        return max;
    }
    public static double Max(this IReadOnlyList<double> source, int from, int to)
    {
        int index;
        return Max(source, from, to, out index);
    }
    public static double Max(this IReadOnlyList<double> source, int n, out int index) // 获取最近的n的最大值
    {
        return source.Max(source.Count - n, source.Count - 1, out index);
    }
    public static double Max(this IReadOnlyList<double> source, int n) // 获取最近的n的最大值
    {
        return source.Max(source.Count - n, source.Count - 1);
    }


    public static double Sum(IEnumerable<double> source)
    {
        int notNans;
        return Sum(source, out notNans);
    }
    public static double Sum(IEnumerable<double> source, out int notNans)
    {
        int    notNanNumber = 0;
        double sum          = 0f;
        foreach (double v in source)
        {
            if (double.IsNaN(v))
                continue;
            sum += v;
            notNanNumber++;
        }

        notNans = notNanNumber;
        return sum;
    }
    public static double Sum(IReadOnlyList<double> source, int from, int to)
    {
        int notNans;
        return Sum(source, from, to, out notNans);
    }
    public static double Sum(IReadOnlyList<double> source, int from, int to, out int notNans)
    {
        Debug.Assert(from <= to &&
                     from >= 0);
                     //to >= 0 && to < source.Count);

            // 为了兼容
                     if (to >= source.Count)
                         to = source.Count - 1;




        int    notNanNumber = 0;
        double sum = 0f;
        for (int i = from; i <= to; ++i)
        {
            if (double.IsNaN(source[i]))
                continue;
            notNanNumber++;
            sum += source[i];
        }

        notNans = notNanNumber;
        return sum;
    }
    public static double Sum(IReadOnlyList<double> source, int count, out int notNans)
    {
        return Sum(source, source.Count - count, source.Count-1, out notNans);
    }
    public static double Sum(IReadOnlyList<double> source, int count)
    {
        return Sum(source, source.Count - count, source.Count-1);
    }


    public static int NotNans(IEnumerable<double> source)
    {
        int sum = 0;
        foreach (double v in source)
        {
            if (double.IsNaN(v))
                continue;
            sum++;
        }

        return sum;
    }
    public static int NotNans(IReadOnlyList<double> source, int from, int to)
    {
        Debug.Assert(from <= to &&
                     from >= 0 && from < source.Count &&
                     to >= 0 && to < source.Count);

        int sum = 0;
        for (int i = from; i <= to; ++i)
        {
            if (double.IsNaN(source[i]))
                continue;

            sum++;
        }

        return sum;
    }
    public static int NotNans(IReadOnlyList<double> source, int count)
    {
        return NotNans(source, source.Count - count, source.Count-1);
    }


    public static double Avg(ICollection<double> source)
    {
        int    nn;
        double sum = Sum(source, out nn);

        return nn == 0 ? double.NaN : sum / nn;
    }
    public static double Avg(this IReadOnlyList<double> source, int from, int to)
    {
        Debug.Assert(from <= to &&
                     from >= 0);
                     //to < source.Count);

                     // 为了兼容
                     if (to >= source.Count)
                         to = source.Count - 1;

        int    nn;
        double sum = Sum(source, from, to, out nn);

        return nn == 0 ? double.NaN : sum / nn;
    }
    public static double Avg(this IReadOnlyList<double> source, int count)
    {
        return Avg(source, source.Count - count, count - 1);
    }


        //todo: 需要处理nan
    public static double Stdev(this IEnumerable<double> source)
    {
        double avg = source.Average();

        double variance = 0;
        foreach (float v in source)
        {
            variance += Math.Pow(v - avg, 2); // 求方差
        }

        variance /= source.Count();

        double sd = Math.Pow(variance, 0.5); //求标准差

        return sd;
    }
        // todo: 需要处理nan
    public static double Stdev(this IReadOnlyList<double> source, int from, int to) // 标准差
    {
        Debug.Assert(from <= to &&
                     from >= 0 && from < source.Count &&
                     to >= 0 && to < source.Count);

        double avg = Avg(source, from, to);

        double variance = 0;
        for (int i = from; i <= to; ++i)
        {
            variance += Math.Pow(source[i] - avg, 2); // 求方差
        }

        variance /= to - from + 1;

        double sd = Math.Pow(variance, 0.5); //求标准差

        return sd;
    }

    public static bool CrossOver(this IReadOnlyList<double> source, IReadOnlyList<double> othea, int? period1 = null, int? period2 = null) // 向上穿过
    {
        if (period1 == null)
            period1 = source.Count - 1;

        int p2 = period1.Value;
        if (period2 != null)
            p2 = period2.Value;

        //Debug.Assert(source.Count > period1 && period1 >= 1 );
        //Debug.Assert(othea.Count > p2 && p2 >= 1 );

        if (source.Count <= period1 || period1 < 1 ||
            othea.Count <= p2 || p2 < 1)
            return false;

        if (source[period1.Value - 1] < othea[p2 - 1] &&
            source[period1.Value] >= othea[p2])
            return true;
        else
            return false;
    }
    //public static bool  CrossOver(this IList<double> source, IList<double> othea, int period1, int? period2 = null)  // 向上穿过
    //{
    //    int p2 = period1;
    //    if (period2 != null)
    //        p2 = period2.Value;

    //    Debug.Assert(source.Count > period1 && period1 >= 1 );
    //    Debug.Assert(othea.Count > p2 && p2 >= 1 );

    //    if (source[period1 - 1] < othea[p2 - 1] &&
    //        source[period1] > othea[p2])
    //        return true;
    //    else
    //        return false;
    //}
    public static bool CrossDown(this IReadOnlyList<double> source, IReadOnlyList<double> othea, int? period1 = null, int? period2 = null) // 向下穿过
    {
        if (period1 == null)
            period1 = source.Count - 1;

        int p2 = period1.Value;
        if (period2 != null)
            p2 = period2.Value;

        //Debug.Assert(source.Count > period1 && period1 >= 1 );
        //Debug.Assert(othea.Count > p2 && p2 >= 1 );

        if (source.Count <= period1 || period1 < 1 ||
            othea.Count <= p2 || p2 < 1)
            return false;

        if (source[period1.Value - 1] > othea[p2 - 1] &&
            source[period1.Value] <= othea[p2])
            return true;
        else
            return false;
    }
    //public static bool  CrossDown(this IList<double> source, IList<double> othea, int period1, int? period2 = null)  // 向下穿过
    //{
    //    int p2 = period1;
    //    if (period2 != null)
    //        p2 = period2.Value;

    //    Debug.Assert(source.Count > period1 && period1 >= 1 );
    //    Debug.Assert(othea.Count > p2 && p2 >= 1 );

    //    if (source[period1 - 1] > othea[p2 - 1] &&
    //        source[period1] < othea[p2])
    //        return true;
    //    else
    //        return false;
    //}

#endregion

#region 基本几何

    public static bool PointInTriangle(RVector3 A, RVector3 B, RVector3 C, RVector3 P) { return SameSide(A, B, C, P) && SameSide(B, C, A, P) && SameSide(C, A, B, P); }

    public static bool SameSide(RVector3 A, RVector3 B, RVector3 C, RVector3 P)
    {
        RVector3 AB = B - A;
        RVector3 AC = C - A;
        RVector3 AP = P - A;

        RVector3 v1 = RVector3.Cross(AB, AC);
        RVector3 v2 = RVector3.Cross(AB, AP);

        // v1 and v2 should point to the same direction
        return RVector3.Dot(v1, v2) >= 0;
    }


#region CohenSutherlandLineClip

    public enum EOutCode : int
    {
        INSIDE = 0, // 0000
        LEFT   = 1, // 0001
        RIGHT  = 2, // 0010
        BOTTOM = 4, // 0100
        TOP    = 8, // 1000
    }

    // Compute the bit code for a point (x, y) using the clip rectangle
    // bounded diagonally by (xmin, ymin), and (xmax, ymax)

    // ASSUME THAT xmax, xmin, ymax and ymin are global constants.

    public static EOutCode __ComputeOutCode(double x,    double y,
        int                                        xmin, int    xmax, int ymin, int ymax)
    {
        EOutCode code;

        code = EOutCode.INSIDE; // initialised as being inside of [[clip window]]

        if (x < xmin) // to the left of clip window
            code |= EOutCode.LEFT;
        else if (x > xmax) // to the right of clip window
            code |= EOutCode.RIGHT;
        if (y < ymin) // below the clip window
            code |= EOutCode.BOTTOM;
        else if (y > ymax) // above the clip window
            code |= EOutCode.TOP;

        return code;
    }

    // Cohen–Sutherland clipping algorithm clips a line from
    // P0 = (x0, y0) to P1 = (x1, y1) against a rectangle with 
    // diagonal from (xmin, ymin) to (xmax, ymax).
    public static bool CohenSutherlandLineClip(ref float x0,   ref float y0,   ref float x1,   ref float y1,
        int                                              xmin, int       xmax, int       ymin, int       ymax)
    {
        // compute outcodes for P0, P1, and whatever point lies outside the clip rectangle
        EOutCode outcode0 = __ComputeOutCode(x0, y0, xmin, xmax, ymin, ymax);
        EOutCode outcode1 = __ComputeOutCode(x1, y1, xmin, xmax, ymin, ymax);
        bool     accept   = false;

        while (true)
        {
            if (!Convert.ToBoolean((outcode0 | outcode1)))
            {
                // Bitwise OR is 0. Trivially accept and get out of loop
                accept = true;
                break;
            }
            else if (Convert.ToBoolean(outcode0 & outcode1))
            {
                // Bitwise AND is not 0. (implies both end points are in the same region outside the window). Reject and get out of loop
                break;
            }
            else
            {
                // failed both tests, so calculate the line segment to clip
                // from an outside point to an intersection with clip edge
                float x = 0, y = 0;

                // At least one endpoint is outside the clip rectangle; pick it.
                EOutCode outcodeOut = Convert.ToBoolean(outcode0) ? outcode0 : outcode1;

                // Now find the intersection point;
                // use formulas y = y0 + slope * (x - x0), x = x0 + (1 / slope) * (y - y0)
                if (Convert.ToBoolean(outcodeOut & EOutCode.TOP))
                {
                    // point is above the clip rectangle
                    x = x0 + (x1 - x0) * (ymax - y0) / (y1 - y0);
                    y = ymax;
                }
                else if (Convert.ToBoolean(outcodeOut & EOutCode.BOTTOM))
                {
                    // point is below the clip rectangle
                    x = x0 + (x1 - x0) * (ymin - y0) / (y1 - y0);
                    y = ymin;
                }
                else if (Convert.ToBoolean(outcodeOut & EOutCode.RIGHT))
                {
                    // point is to the right of clip rectangle
                    y = y0 + (y1 - y0) * (xmax - x0) / (x1 - x0);
                    x = xmax;
                }
                else if (Convert.ToBoolean(outcodeOut & EOutCode.LEFT))
                {
                    // point is to the left of clip rectangle
                    y = y0 + (y1 - y0) * (xmin - x0) / (x1 - x0);
                    x = xmin;
                }

                // Now we move outside point to intersection point to clip
                // and get ready for next pass.
                if (outcodeOut == outcode0)
                {
                    x0       = x;
                    y0       = y;
                    outcode0 = __ComputeOutCode(x0, y0, xmin, xmax, ymin, ymax);
                }
                else
                {
                    x1       = x;
                    y1       = y;
                    outcode1 = __ComputeOutCode(x1, y1, xmin, xmax, ymin, ymax);
                }
            }
        }

        if (accept)
        {
            // Following functions are left for implementation by user based on
            // their platform (OpenGL/graphics.h etc.)
            //DrawRectangle(xmin, ymin, xmax, ymax);
            //LineSegment(x0, y0, x1, y1);
        }

        return accept;
    }

#endregion


#region Bezier

    //public static Vector3 CalculateCubicBezierPoint(float t, Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3)  // 计算3次贝塞尔曲线的点
    //{
    //    float u = 1 - t;
    //    float tt = t * t;
    //    float uu = u * u;
    //    float uuu = uu * u;
    //    float ttt = tt * t;

    //    Vector3 p = uuu * p0;
    //    p += 3 * uu * t * p1;
    //    p += 3 * u * tt * p2;
    //    p += ttt * p3;

    //    return p;
    //}  

#endregion

#endregion


    /// <summary>
    ///double取整为int
    /// </summary>
    /// <param name="val">
    /// <returns></returns>
    public static int DoubleToInt(double val) { return (0 < val) ? (int)(val + 0.5) : (int)(val - 0.5); }


    // 笛卡尔积 https://ericlippert.com/2010/06/28/computing-a-cartesian-product-with-linq/
    public static IEnumerable<IEnumerable<T>> CartesianProduct<T>(this IEnumerable<IEnumerable<T>> sequences)
    {
        IEnumerable<IEnumerable<T>> emptyProduct = new[] { Enumerable.Empty<T>() };
        return sequences.Aggregate(
                                   emptyProduct,
                                   (accumulator, sequence) =>
                                       from accseq in accumulator
                                       from item in sequence
                                       select accseq.Concat(new[] { item }));
        //select accseq.Concat( item ));
    }


#region MaxDrawdown
    public class MaxDrawdown
    {
        public MaxDrawdown(double drawdownPercentage, double drawdownAmount)
        {
            DrawdownPercentage = drawdownPercentage;
            DrawdownAmount     = drawdownAmount;
        }

        public double DrawdownPercentage { get; private set; }
        public double DrawdownAmount     { get; private set; }
    }

    /// <summary>
    /// Finds the largest percentage loss from a peak to a trough, before a new peak is attained.
    /// </summary>
    /// <returns>Drawdown percentage and amount</returns>
    public static MaxDrawdown CalculateMaxPercentageDrawdown(IList<double> values) => CalculateMaxDrawdown(values, byPercentage: true);

    /// <summary>
    /// Finds the largest loss (in absolute numbers, for example USD) from a peak to a trough, before a new peak is attained.
    /// </summary>
    /// <returns>Drawdown percentage and amount</returns>
    public static MaxDrawdown CalculateMaxAmountDrawdown(IList<double> values) => CalculateMaxDrawdown(values, byPercentage: false);

    private static MaxDrawdown CalculateMaxDrawdown(IList<double> values, bool byPercentage)
    {
        if (values.Count() == 0) return new MaxDrawdown(0, 0);
        if (values.First() == 0)
            throw new ArgumentException("Cannot calculate drawdown percentage where the initial value is zero, dividing any loss with zero results in \"infinite\" loss.", nameof(values));

        double maxPeak                   = double.NaN;
        double maxAmountDrop             = double.MinValue;
        double maxPercentageDrawdown     = double.MaxValue;
        double percentageDrawdown        = double.MaxValue;
        double maxPercentageDropInAmount = 0;
        foreach (var value in values)
        {
            if (double.IsNaN(maxPeak))
            {
                //first value is always the first peak
                maxPeak = value;
                continue;
            }

            var amountDrawdown = maxPeak - value;
            maxPeak = amountDrawdown < 0 ? value : maxPeak;

            if (maxAmountDrop < amountDrawdown & amountDrawdown > 0)
            {
                //new low drop in amount
                maxAmountDrop      = amountDrawdown;
                percentageDrawdown = (maxAmountDrop / maxPeak) * -1d;
            }

            //if we're finding the largest ABSOLUTE/AMOUNT drop (not percentage), use this value as the new percentage drop
            //if we're finding the largest PERCENTAGE drop, we need to check if this is the new maximum percentage drop
            if (!byPercentage || percentageDrawdown < maxPercentageDrawdown)
            {
                maxPercentageDrawdown     = percentageDrawdown;
                maxPercentageDropInAmount = maxAmountDrop;
            }
        }

        double percentage = maxPercentageDrawdown != double.MaxValue && maxPercentageDrawdown < 0 ? Math.Abs(maxPercentageDrawdown) : 0;
        maxAmountDrop             = Math.Round(maxAmountDrop != double.MinValue && maxAmountDrop > 0 ? maxAmountDrop : 0, 2);
        maxPercentageDropInAmount = Math.Round(maxPercentageDropInAmount != double.MinValue && maxPercentageDropInAmount > 0 ? maxPercentageDropInAmount : 0, 2);

        if (byPercentage)
            return new MaxDrawdown(drawdownPercentage: percentage, drawdownAmount: maxPercentageDropInAmount);
        else
            return new MaxDrawdown(drawdownPercentage: percentage, drawdownAmount: maxAmountDrop);
    }


#region 增进式算mdd
    public class DrawDown
    {
        public double Peak      { get; protected set; }
        public int PeakIndex { get; protected set; }

        public double Trough      { get; protected set; }
        public int TroughIndex { get;  protected set; }

        public double DrawdownAmount => Math.Round(Peak - Trough, 2);
        public double DrawdownRate   => Math.Round((Peak - Trough)/Peak, 4);

        public void UpdatePeak(double peak, int peakIndex)
        {
            Peak        = peak;
            PeakIndex   = peakIndex;
            Trough      = peak;
            TroughIndex = peakIndex;
        }

        public void UpdateTrough(double trough, int troughIndex)
        {
            Trough = trough;
            TroughIndex = troughIndex;
        }

        public DrawDown(double peak, int peakIndex, double trough, int troughIndex)
        {
            Peak        = peak;
            PeakIndex   = peakIndex;
            Trough      = trough;
            TroughIndex = troughIndex;
        }
            public DrawDown(){}
    }


    // 增量计算values的高低点
    public static void GetDrawdowns(ref List<DrawDown> drawDowns, IReadOnlyList<double> values, int start = 0)
    {
        if (drawDowns == null)
            throw new ArgumentNullException(nameof(drawDowns));
        if (values == null)
            throw new ArgumentNullException(nameof(values));

        DrawDown dd                = null;
        bool     useLastPeakTrough = false;
        if (drawDowns.Any())
        {
            dd                = drawDowns.Last();
            useLastPeakTrough = true;
        }

        for (int i = start; i != values.Count; ++i)
        {
            double value = values[i];

            if (dd == null)
            {
                //first value is always the first peak
                dd = new DrawDown(value, i, value, i);
                continue;
            }

            if (value > dd.Peak) // 新的高点
            {
                // 储存上一个高低区间
                if (!dd.Peak.NearlyEqual(dd.Trough) && !useLastPeakTrough)
                {
                    drawDowns.Add(dd);
                }
                dd = new DrawDown();
                dd.UpdatePeak(value, i);               
                useLastPeakTrough = false;
            }
            else if (value < dd.Trough) // 新的低点
            {
                dd.UpdateTrough(value, i);
            }
        }

        // 加入最后一个高低点
        if (!useLastPeakTrough)
            drawDowns.Add(dd);
    }

    public static DrawDown MaxDrawdownAmount(this List<DrawDown> peakTroughs)
    {
        return peakTroughs.MaxBy(p => p.DrawdownAmount);
    }
    public static DrawDown MaxDrawdownRate(this List<DrawDown> peakTroughs)
    {
        return peakTroughs.MaxBy(p => p.DrawdownRate);
    }
    public static DrawDown MaxDrawdownAmount(IReadOnlyList<double> values, int start = 0)
    {
        List<DrawDown> dds = new();
        GetDrawdowns(ref dds, values, start);
        return dds.MaxDrawdownAmount();
    }
    public static DrawDown MaxDrawdownRate(IReadOnlyList<double> values, int start = 0)
    {
        List<DrawDown> dds = new();
        GetDrawdowns(ref dds, values, start);
        return dds.MaxDrawdownRate();
    }

#endregion


#endregion
}
}