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
using System.Text;
using System.Threading.Tasks;
using DataModel;
using log4net.Appender;
using RLib.Base;
using RLib.Fin;

namespace NeoLib
{
    public class InternalDataSeries : Datas, IInternalDataSeries
    {

        public IScript             Host { get; set; }                                   // 所属的Script
    }


    public class OutputDataSeries:Datas, IOutputDataSeries
    {
        public IScript             Host { get; set; }                                   // 所属的Script

        public virtual EVisualDataSeriesShape Shape
        {
            get
            {
                return _Shape;
            }
            set
            {
                if (RMath.Equal(Shape, value))
                    return;

                _Shape = value;
                //RaisePropertyChanged("ShapeType");

                _Invalidate();
            }
        }

        public Color               Color
        {
            get { return Line.Color;}
            set { Line = new LineAppearance(Line.Style, Line.Width, value); }
        }
        public ELineStyle          LineStyle
        {
            get { return Line.Style;}
            set
            {
                Line = new LineAppearance(value, Line.Width, Line.Color);
            } }
        public int                 LineWidth
        {
            get { return Line.Width;}
            set
            {
                Line = new LineAppearance(Line.Style, value, Color);
            } }

        public LineAppearance      Line { get; set; }
        public bool         Highlight { get; set; }                         // 高亮状态

        public bool         _CanIteract(int index, double value)           // 检查是否交汇
        {
            return true;
        }

        public bool        Hide { get; set; }


        public bool        IsMouseHover { get; set; }                       // 当前是否鼠标悬停
    
#region Colors
        //public RColor        Color
        //{
        //    get
        //    {
        //        return _Color;
        //    }
        //    set
        //    {
        //        if (RMath.Equal(Color, value))
        //            return;

        //        _Color = value;
        //        //RaisePropertyChanged("Color");

        //        _Invalidate();
        //    }
        //}

        //public RColor       GetColor(int index)
        //{
        //    if (_Colors.Count == 0)
        //        return Line.Color;
        //    else
        //    {
        //        if (index >= Count)
        //        {
        //            int n = index - Count;
        //            for (int i = 0; i <= n; i++)
        //            {
        //                _Colors.Add(Line.Color);
        //            }
        //        }

        //        return _Colors[index];
        //    }
        //}

        //public void         SetColor(int index, RColor color)
        //{
        //    if (index < Count)
        //        _Colors[index] = color;
        //    else if (index == Count)
        //        _Colors.Add(color);
        //    else
        //    {
        //        int n = index - Count;
        //        for (int i = 0; i < n; i++)
        //        {
        //            _Colors.Add(Line.Color);
        //        }

        //        _Colors.Add(color);
        //    }

        //}

#endregion


        //public ELineStyle   LineStyle
        //{
        //    get { return _LineStyle; }
        //    set
        //    {
        //        if (RMath.Equal(LineStyle, value))
        //            return;

        //        _LineStyle = value;
        //        //RaisePropertyChanged("LineStyle");
        //        _Invalidate();
        //    }
        //}

        //public int          LineWidth
        //{
        //    get
        //    {
        //        return _LineWidth;
        //    }
        //    set
        //    {
        //        if (RMath.Equal(LineWidth, value))
        //            return;

        //        _LineWidth = value;
        //        //RaisePropertyChanged("LineWidth");
        //        _Invalidate();
        //    }
        //}

        public OutputDataSeries()
        {
            Line = new LineAppearance(ELineStyle.LineSolid, 1, ColoredConsoleAppender.Colors.Green);
        }

        public virtual void _Invalidate() {}


        //protected RColor     _Color;
        //protected ELineStyle _LineStyle;
        //protected int       _LineWidth;
        protected EVisualDataSeriesShape _Shape;

        protected List<RColor> _Colors = new List<RColor>();

    }
}
