/********************************************************************
    created:	2019/9/13 14:52:34
    author:		rush
    email:		
	
    purpose:	
*********************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using DataModel;
using RLib.Base;

namespace NeoLib
{
    public enum ESource
    {
        Open        = 1,
        High        = 2,
        Low         = 4,
        Close       = 8,
        Custom      = 16
    }

    public interface IDatas:IReadOnlyList<double>, IStringSerializable                      // 数据序列
    {
        int                 FirstNotNanIndex { get; }                       // 第一有有意义（非NAN）的Index

        double              LastValue { get; }
        double              Last(int index = 0);
    }
  

   

    //[AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    //public class OutDataSeriesAttribute : ParaAttribute                            // 输出
    //{
    //    public ELineStyle   LineStyle { get; set; }
    //    public object       LineColor { get; set; }
    //    public int          LineWidth { get; set; }
    //    public EVisualDataSeriesShape ShapeType { get; set; }                              // 表现形式

    //    public override IPara        GetPara(PropertyInfo pi)
    //    {
    //        Type ot = pi.PropertyType;

    //        List<IPara> lps = ot.GetParas();
    //        //lps.ApplyValues(new KeyValuePair<string, object>[]
    //        //{
    //        //    new KeyValuePair<string, object>("LineStyle", LineStyle), 
    //        //    new KeyValuePair<string, object>("LineColor", LineColor), 
    //        //    new KeyValuePair<string, object>("LineWidth", LineWidth), 
    //        //    new KeyValuePair<string, object>("ShapeType", ShapeType), 
    //        //});


    //        return new Para(pi, Group, Desc, DefVal){IsFile = IsFile, IsDir = IsDir, Min = Min, Max = Max, Step = Step,
    //            NestingParas = lps
    //        };
    //    }

    //    public override IPara        GetPara(object host, PropertyInfo pi)
    //    {
    //        Type t = pi.PropertyType;

    //        object o = pi.GetValue(host);  // 内部property对象

    //        List<IPara> lps = o.GetParas();
    //        lps.ApplyDefValues(new KeyValuePair<string, object>[]
    //        {
    //            new KeyValuePair<string, object>("LineStyle", LineStyle),
    //            new KeyValuePair<string, object>("LineColor", RColor.From(LineColor)),
    //            new KeyValuePair<string, object>("LineWidth", LineWidth),
    //            new KeyValuePair<string, object>("ShapeType", ShapeType),
    //        });


    //        return  new Para(host, pi, Group, Desc, DefVal, Presets)
    //        {
    //            IsFile = IsFile, IsDir = IsDir, Min = Min, Max = Max, Step = Step,
    //            NestingParas = lps
    //        };
    //    }

    //    public              OutDataSeriesAttribute(string name, string desc = "", string group="")
    //    :base(name, desc, group)
    //    {
    //        this.LineStyle = ELineStyle.LineSolid;
    //        this.LineColor = RColor.Red;
    //        LineWidth = 1;
    //        ShapeType = EVisualDataSeriesShape.Line;
    //    }
    //}



    //[AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    //public class OHLCSeriesAttribute : ParaAttribute                            // 输出
    //{
    //    public object BullOutlineColor { get; set; }
    //    public object BearOutlineColor { get; set; }
    //    public object BullFillColor { get; set; }
    //    public object BearFillColor { get; set; }

    //    public override IPara        GetPara(PropertyInfo pi)
    //    {
    //        Type ot = pi.PropertyType;

    //        List<IPara> lps = ot.GetParas();
    //        //lps.ApplyValues(new KeyValuePair<string, object>[]
    //        //{
    //        //    new KeyValuePair<string, object>("LineStyle", LineStyle), 
    //        //    new KeyValuePair<string, object>("LineColor", LineColor), 
    //        //    new KeyValuePair<string, object>("LineWidth", LineWidth), 
    //        //    new KeyValuePair<string, object>("ShapeType", ShapeType), 
    //        //});


    //        return new Para(pi, Group, Desc, DefVal){IsFile = IsFile, IsDir = IsDir, Min = Min, Max = Max, Step = Step,
    //            NestingParas = lps
    //        };
    //    }

    //    public override IPara        GetPara(object host, PropertyInfo pi)
    //    {
    //        Type t = pi.PropertyType;

    //        object o = pi.GetValue(host);  // 内部property对象

    //        List<IPara> lps = o.GetParas();
    //        lps.ApplyDefValues(new KeyValuePair<string, object>[]
    //        {
    //            new KeyValuePair<string, object>("BullOutlineColor", RColor.From(BullOutlineColor)),
    //            new KeyValuePair<string, object>("BearOutlineColor", RColor.From(BearOutlineColor)),
    //            new KeyValuePair<string, object>("BullFillColor", RColor.From(BullFillColor)),
    //            new KeyValuePair<string, object>("BearFillColor", RColor.From(BearFillColor)),
    //        });


    //        return  new Para(host, pi, Group, Desc, DefVal, Presets)
    //        {
    //            IsFile = IsFile, IsDir = IsDir, Min = Min, Max = Max, Step = Step,
    //            NestingParas = lps
    //        };
    //    }

    //    public              OHLCSeriesAttribute(string name, string desc = "", string group="")
    //    :base(name, desc, group)
    //    {
    //        BullOutlineColor = RColor.Green;
    //        BearOutlineColor = RColor.Red;
    //        BullFillColor = RColor.Green;
    //        BearFillColor = RColor.Red;
    //    }
    //}

}
