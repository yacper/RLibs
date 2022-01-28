/********************************************************************
    created:	2020/2/15 17:49:40
    author:	rush
    email:		
	
    purpose:	带输出表现的dataSeries,用于图标显示
*********************************************************************/
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using DataModel;
using RLib.Base;

namespace NeoLib
{

    public interface IInternalDataSeries : IDataSeries
    {
        new double this[int index] { get; set; }

        IScript             Host { get; }                                   // 所属的Script
    }


    public struct LineAppearance
    {
        public ELineStyle Style;
        public int Width;
        public Color Color;

        public      LineAppearance(ELineStyle style, int width, Color color)
        {
            Style = style;
            Width = width;
            Color = color;
        }

        public LineAppearance(ELineStyle style, int width, string color)
        :this(style, width, ColorEx.FromString(color))
        {
        }

        //public      LineAppearance()
        //{
        //    Style = ELineStyle.LineSolid;
        //    Width = 1;

        //    Color = Colors.Red;
        //}
    }

    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class LineParaAttribute : ParaAttribute
    {
        public              LineParaAttribute(string name, string desc = "", string group="",  ELineStyle style= ELineStyle.LineSolid, int width = 1, string color="#ffff0000")
        :base(name, desc, group, new LineAppearance(style, width, ColorEx.FromString(color)))
        {
        }
    }


    public enum EVisualDataSeriesShape
    {
        Dot     = 0,            // 点输出
        Line    = 1,
        Bar     = 2,
    }
    public struct Level             // 水平线
    {
        public double       Value;
        public LineAppearance Line;

        public              Level(double value, LineAppearance line)
        {
            Value = value;
            Line = line;
        }
    }

    public interface IOutputDataSeries:IInternalDataSeries
    {
//        new double this[int index] { get; set; }


        [Para("Shape", "Shape", "Style", EVisualDataSeriesShape.Line, HideInParaGrid = true)]
        EVisualDataSeriesShape    Shape { get; set; }                              // 表现形式

//#region Colors
       
//        RColor              GetColor(int index);    
//        void                SetColor(int index, RColor color);
//#endregion
        Color               Color { get; set; }
        ELineStyle          LineStyle { get; set; }
        int                 LineWidth { get; set; }

        [Para("Line", "Line", "Style")]
        LineAppearance      Line { get; set; }

        bool                Hide { get; set; }
      

        bool                IsMouseHover { get; }                           // 当前是否鼠标悬停

    }

    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class OutputAttribute : ParaAttribute                            // 输出
    {
        public object       Color { get; set; }
        public ELineStyle   LineStyle { get; set; }
        public int          LineWidth { get; set; }
        public EVisualDataSeriesShape ShapeType { get; set; }                              // 表现形式

        public override IPara        GetPara(PropertyInfo pi)
        {
            Type ot = pi.PropertyType;

            List<IPara> lps = ot.GetParas();
            //lps.ApplyValues(new KeyValuePair<string, object>[]
            //{
            //    new KeyValuePair<string, object>("LineStyle", LineStyle), 
            //    new KeyValuePair<string, object>("LineColor", LineColor), 
            //    new KeyValuePair<string, object>("LineWidth", LineWidth), 
            //    new KeyValuePair<string, object>("ShapeType", ShapeType), 
            //});


            return new Para(pi, Group, Desc, DefVal){IsFile = IsFile, IsDir = IsDir, Min = Min, Max = Max, Step = Step,
                NestingParas = lps
            };
        }

        public override IPara        GetPara(object host, PropertyInfo pi)
        {
            Type t = pi.PropertyType;

            object o = pi.GetValue(host);  // 内部property对象

            List<IPara> lps = o.GetParas();
            //lps.ApplyDefValues(new KeyValuePair<string, object>[]
            //{
            //    new KeyValuePair<string, object>("LineStyle", LineStyle),
            //    new KeyValuePair<string, object>("Color", RColor.From(Color)),
            //    new KeyValuePair<string, object>("LineWidth", LineWidth),
            //    new KeyValuePair<string, object>("Shape", ShapeType),
            //});
            lps.ApplyDefValues(new KeyValuePair<string, object>[]
            {
                //new KeyValuePair<string, object>("Line", new LineAppearance(LineStyle, LineWidth,  ColorEx.FromString(Color as string))),
                new KeyValuePair<string, object>("Line", new LineAppearance(LineStyle, LineWidth,  ColorEx.Parse(Color))),
                new KeyValuePair<string, object>("Shape", ShapeType),
            });


            return  new Para(host, pi, Group, Desc, DefVal, Presets)
            {
                Name = Name,
                IsFile = IsFile, IsDir = IsDir, Min = Min, Max = Max, Step = Step,
                NestingParas = lps
            };
        }

        public              OutputAttribute(string name, string desc = "", string group="Other")
        :base(name, desc, group)
        {
            this.LineStyle = ELineStyle.LineSolid;
            this.Color = "#FFFF0000";
            LineWidth = 1;
            ShapeType = EVisualDataSeriesShape.Line;
        }
    }


}
