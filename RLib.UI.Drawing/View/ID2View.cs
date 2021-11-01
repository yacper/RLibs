/********************************************************************
    created:	2020/2/17 15:34:27
    author:		rush
    email:		
	
    purpose:	一个基本2d视图控件
*********************************************************************/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;
using DataModel;
using RLib.Base;
using SharpDX.Direct2D1;
using SharpDX.DirectWrite;
using FontWeight = System.Windows.FontWeight;

namespace RLib.UI.Drawing
{
    public interface ID2View:INotifyPropertyChanged
    {
        double              Width { get; }
        double              Height { get; }
        event SizeChangedEventHandler SizeChanged;

        Color               Background { get;}                              // 背景色


        void                DrawLine(Point start, Point end, Color strokeColor, int strokeWidth=1, ELineStyle strokeStyle = ELineStyle.LineSolid, Rect? clip = null, int zindex = 0);
        void                DrawLine(List<Point> vs, Color strokeColor, int strokeWidth=1, ELineStyle strokeStyle = ELineStyle.LineSolid, Rect? clip = null, int zindex = 0);


        void                DrawRectangle(Rect rect, Color strokeColor, int strokeWidth=1, ELineStyle strokeStyle = ELineStyle.LineSolid, Color? fillColor = null, int zindex = 0);

        //void                DrawTriangle(Point a, Point b, Point c, Color clr, int zindex = 0);

        //void                DrawQuad(Point a, Point b, Point c, Point d, Color clr, int zindex = 0);
        //void                DrawQuad(Point ld, Point ur, Color clr, int zindex = 0);

        TextInfo            DrawText(string text, string fontFamlity, int fontSize, Rect rect, Color foregroundColor, Color? backColor = null,
            TextAlignment textAlignment = TextAlignment.Center, ParagraphAlignment paragraphAlignment=ParagraphAlignment.Center, int zindex = 0);

        // 绘制bitmap，dest不设置的话，直接以原始尺寸绘制到(0,0)
        // src不设置的话，绘制原始尺寸
        void                DrawImage(System.Drawing.Bitmap image, string imageId, Rect? destinationRectangle= null, float opacity =1.0f, BitmapInterpolationMode interpolationMode= BitmapInterpolationMode.Linear, Rect? sourceRectangle= null, int zindex = 0);
        void                DrawImage(string imageFilePath, Rect? destinationRectangle= null, float opacity =1.0f, BitmapInterpolationMode interpolationMode= BitmapInterpolationMode.Linear, Rect? sourceRectangle= null, int zindex = 0);

        void                DrawGeometry(List<Point> points, Color strokeColor, float strokeWidth=1, ELineStyle strokeStyle = ELineStyle.LineSolid, Color? fillColor=null, int zindex = 0);

        void                ClearDrawings();                                // 删除所有绘制元素

        void                OnRender();

        event Action<ID2View> RenderEx;                                     // 额外Render事件
    }

    public enum EDrawing  // 基础绘图元素
    {
        Line        = 1,
        Triangle    = 2,
        Quad        = 4,
        Text        = 8,        // 文字
        Rectangle   = 16,        // 长方形
        Image       = 32,        // 图片
        Geometry    = 64,       // 
    }


    public class DrawingInfo  // 
    {
        public virtual EDrawing Type { get { throw new NotImplementedException();} }
        public int          ZIndex { get; set; }
    }

    public class LineInfo:DrawingInfo                                       // 代表连续的线
    {
        public override EDrawing Type { get{ return EDrawing.Line; } }

        public ELineStyle   StrokeStyle { get; set; }
        public Color       StrokeColor { get; set; }                             // 线颜色（线颜色相同的线可以合并）
        public int          StrokWidth { get; set; }
        public Rect?        Clip { get; set; }

        public List<Point> Nodes = new List<Point>(2);
        public virtual int  Count { get { return Nodes.Count / 2; } }
    }



    public class RectangleInfo:DrawingInfo
    {
        public override EDrawing Type { get{ return EDrawing.Rectangle; } }

        public ELineStyle   StrokeStyle { get; set; }
        public Color        StrokeColor { get; set; }                             // 线颜色（线颜色相同的线可以合并）
        public int          StrokWidth { get; set; }

        public Color?       FillColor { get; set; }                         // 填充颜色(填充颜色相同的quad可以合并, 否则不能)

        public Rect   Rect { get; set; }
    }

    //public class TriangleInfo:LineInfo
    //{
    //    public override EDrawing  Type { get{ return EDrawing.Triangle; } }

    //    public Color       FillColor { get; set; }                         // 填充颜色(填充颜色相同的quad可以合并, 否则不能)
    //    public override int Count { get { return Nodes.Count / 3; } }
    //}

    //public class QuadInfo:LineInfo
    //{
    //    public override EDrawing  Type { get{ return EDrawing.Quad; } }

    //    public Color       FillColor { get; set; }                         // 填充颜色(填充颜色相同的quad可以合并, 否则不能)
    //    public override int Count { get { return Nodes.Count / 4; } }
    //}

    public enum TextAlignment
    {
        /// <summary>
        /// <dd> <p>The leading edge of the paragraph text is aligned to the leading edge of the layout box.</p> </dd>
        /// </summary>
        /// <doc-id>dd368131</doc-id>
        /// <unmanaged>DWRITE_TEXT_ALIGNMENT_LEADING</unmanaged>
        /// <unmanaged-short>DWRITE_TEXT_ALIGNMENT_LEADING</unmanaged-short>
        Leading,
        /// <summary>
        /// <dd> <p>The trailing edge of the paragraph text is aligned to the  trailing edge of the layout box.</p> </dd>
        /// </summary>
        /// <doc-id>dd368131</doc-id>
        /// <unmanaged>DWRITE_TEXT_ALIGNMENT_TRAILING</unmanaged>
        /// <unmanaged-short>DWRITE_TEXT_ALIGNMENT_TRAILING</unmanaged-short>
        Trailing,
        /// <summary>
        /// <dd> <p>The center of the paragraph text is aligned to the center of the layout box.</p> </dd>
        /// </summary>
        /// <doc-id>dd368131</doc-id>
        /// <unmanaged>DWRITE_TEXT_ALIGNMENT_CENTER</unmanaged>
        /// <unmanaged-short>DWRITE_TEXT_ALIGNMENT_CENTER</unmanaged-short>
        Center,
        /// <summary>
        /// <dd> <p>Align text to the leading side, and also justify text to fill the lines.</p> </dd>
        /// </summary>
        /// <doc-id>dd368131</doc-id>
        /// <unmanaged>DWRITE_TEXT_ALIGNMENT_JUSTIFIED</unmanaged>
        /// <unmanaged-short>DWRITE_TEXT_ALIGNMENT_JUSTIFIED</unmanaged-short>
        Justified,
    }
    public enum ParagraphAlignment
    {
        /// <summary>
        /// <dd> <p>The top of the text flow is aligned to the top edge of the layout box.</p> </dd>
        /// </summary>
        /// <doc-id>dd368112</doc-id>
        /// <unmanaged>DWRITE_PARAGRAPH_ALIGNMENT_NEAR</unmanaged>
        /// <unmanaged-short>DWRITE_PARAGRAPH_ALIGNMENT_NEAR</unmanaged-short>
        Near,
        /// <summary>
        /// <dd> <p>The bottom of the text flow is aligned to the bottom edge of the layout box.</p> </dd>
        /// </summary>
        /// <doc-id>dd368112</doc-id>
        /// <unmanaged>DWRITE_PARAGRAPH_ALIGNMENT_FAR</unmanaged>
        /// <unmanaged-short>DWRITE_PARAGRAPH_ALIGNMENT_FAR</unmanaged-short>
        Far,
        /// <summary>
        /// <dd> <p>The center of the flow is aligned to the center of the layout box.</p> </dd>
        /// </summary>
        /// <doc-id>dd368112</doc-id>
        /// <unmanaged>DWRITE_PARAGRAPH_ALIGNMENT_CENTER</unmanaged>
        /// <unmanaged-short>DWRITE_PARAGRAPH_ALIGNMENT_CENTER</unmanaged-short>
        Center,
    }

    public class TextInfo:DrawingInfo          // 简化，暂不提供排版等功能   暂时不支持空格
    {
        public override EDrawing Type { get{ return EDrawing.Text; } }

        public string       Text { get; set; }
        public Rect         Rect { get; set; }
        public Color        ForegroundColor { get; set; }                   // 字颜色
        public Color?       BackroundColor { get; set; }                    // 背景色

        public string       FontFamilyName { get; set; }
        public int          FontSize { get; set; }
        public TextAlignment TextAlignment { get; set; }
        public ParagraphAlignment ParagraphAlignment { get; set; }

        public TextFormat   _Format { get; set; }

    }

    public enum BitmapInterpolationMode
    {
        /// <summary>No documentation.</summary>
        /// <doc-id>dd368073</doc-id>
        /// <unmanaged>D2D1_BITMAP_INTERPOLATION_MODE_NEAREST_NEIGHBOR</unmanaged>
        /// <unmanaged-short>D2D1_BITMAP_INTERPOLATION_MODE_NEAREST_NEIGHBOR</unmanaged-short>
        NearestNeighbor,
        /// <summary>No documentation.</summary>
        /// <doc-id>dd368073</doc-id>
        /// <unmanaged>D2D1_BITMAP_INTERPOLATION_MODE_LINEAR</unmanaged>
        /// <unmanaged-short>D2D1_BITMAP_INTERPOLATION_MODE_LINEAR</unmanaged-short>
        Linear,
    }
    public class ImageInfo:DrawingInfo          //
    {
        public override EDrawing Type { get{ return EDrawing.Image; } }

        // 使用Image对象和iD
        public System.Drawing.Bitmap Image { get; set; }
        public string       ImageId { get; set; }

        // 直接使用ImagePath加载
        public string       ImagePath { get; set; }

        public Rect?        DestRect { get; set; }
        public Rect?        SrcRect { get; set; }

        public float        Opacity { get; set; }
        public BitmapInterpolationMode InterpolationMode => BitmapInterpolationMode.Linear;
    }

    public class GeometryInfo:DrawingInfo          //
    {
        public override EDrawing Type { get{ return EDrawing.Geometry; } }

        public ELineStyle   StrokeStyle { get; set; }
        public Color        StrokeColor { get; set; }                             // 线颜色（线颜色相同的线可以合并）
        public float        StrokWidth { get; set; }
        public Rect?        Clip { get; set; }

        public Color?       FillColor { get; set; }                             

        public List<Point>  Nodes { get; set; } 
    }

}
