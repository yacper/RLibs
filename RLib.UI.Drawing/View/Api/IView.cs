///********************************************************************
//    created:	2017/2/19 17:19:52
//    author:		rush
//    email:		
	
//    purpose:	一个视窗, 用于绘制硬件加速的图形
//                目前的实现是一个winform，这个后面需要改进
//*********************************************************************/
//using System;
//using System.Collections.Generic;
//using System.ComponentModel;
//using System.Drawing;
//using System.Linq;
//using System.Text;
//using DataModel;
//using OpenTK.Graphics;
//using RLib.Base;
//using Color = System.Drawing.Color;
//using Size = System.Windows.Size;
//using TextAlignment = OpenTK.Graphics.TextAlignment;

//namespace RLib.UI 
//{
//    public partial class View
//    {
////        public static readonly Font Serif = new Font(FontFamily.GenericSerif, 9);
//        public static readonly Font Serif = new Font(FontFamily.GenericSerif, 9);
//        public static readonly Font Sans = new Font(FontFamily.GenericSansSerif, 9);
//        public static readonly Font Mono = new Font(FontFamily.GenericMonospace, 12);

//        public static readonly Font Default = new Font(FontFamily.GenericSerif, 9);
//    }


//    public interface IView:INotifyPropertyChanged
//    {
//        RSize               SizeProperty { get; set; }
//        RColor              BackColorProperty { get; set; }
//        bool                IsRebuildEveryFrame { get; set; }               // 是否每帧重构绘图信息

//        int                 Layers { get; }



//        void                DrawLine(RVector2 start, RVector2 end, RColor c, int thickness=1, ELineStyle style = ELineStyle.LINE_SOLID, int layer = 0);
//        void                DrawLine(List<RVector2> vs, RColor c, int thickness=1, ELineStyle style = ELineStyle.LINE_SOLID, int layer = 0);

//        void                DrawTriangle(RVector2 a, RVector2 b, RVector2 c, RColor clr, int layer = 0);
//        void                DrawQuad(RVector2 a, RVector2 b, RVector2 c, RVector2 d, RColor clr, int layer = 0);
//        void                DrawQuad(RVector2 ld, RVector2 ur, RColor clr, int layer = 0);

//        TextInfo            DrawText(RVector2 point, string text, RColor color, Font font = null , TextAlignment alignment = TextAlignment.Near, int layer = 0);

//        void                ClearDrawing();                                 // 清理

//        void                _OnRender();

//    }


//    public enum EDrawing
//    {
//        Line        = 1,
//        Triangle    = 2,
//        Quad        = 4,
//        Text        = 8,        // 文字
//    }


//    public class DrawingInfo  // 
//    {
//        public virtual EDrawing Type { get { throw new NotImplementedException();} }
//        public int          Layer { get; set; }
//        public RColor       Color { get; set; }                             // 线颜色（线颜色相同的线可以合并）
//    }

//    public class LineInfo:DrawingInfo
//    {
//        public override EDrawing Type { get{ return EDrawing.Line; } }

//        public ELineStyle   LineStyle { get; set; }

//        public List<RVector2> Nodes { get; set; } 
//        public int          Thinkness { get; set; }
//        public virtual int  Count { get { return Nodes.Count / 2; } }
//    }

//    public class TriangleInfo:LineInfo
//    {
//        public override EDrawing  Type { get{ return EDrawing.Triangle; } }

//        public RColor       FillColor { get; set; }                         // 填充颜色(填充颜色相同的quad可以合并, 否则不能)
//        public override int Count { get { return Nodes.Count / 3; } }
//    }

//    public class QuadInfo:LineInfo
//    {
//        public override EDrawing  Type { get{ return EDrawing.Quad; } }

//        public RColor       FillColor { get; set; }                         // 填充颜色(填充颜色相同的quad可以合并, 否则不能)
//        public override int Count { get { return Nodes.Count / 4; } }
//    }

//    public class TextInfo:DrawingInfo          // 简化，不提供排版等功能   暂时不支持空格
//    {
//        public override EDrawing Type { get{ return EDrawing.Text; } }

//        public string       Text { get; set; }
//        public Font         Font { get; set; }
//        public RVector2       Point { get; set; }
//        public TextAlignment Alignment { get; set; }
//        public RectangleF   Rect { get; set; }
//        public TextExtents Extents { get; set; }
//    }

//}
