// created: 2022/07/26 14:52
// author:  rush
// email:   yacper@gmail.com
// 
// purpose:
// modifiers:

using System.Net.Mime;
using Microsoft.Maui.Graphics;
using Microsoft.Maui.Graphics.Text;

namespace RLib.Graphics;

public enum EDrawing // 基础绘图元素
{
    Line      = 1,
    Triangle  = 2,
    Quad      = 4,
    Text      = 8,  // 文字
    Rectangle = 16, // 长方形
    Ellipse = 16, // 椭圆
    Path = 16, // Path
    Image     = 32, // 图片
    String  = 64, // 
}

internal class DrawingInfo // 
{
    public virtual EDrawing Type   { get { throw new NotImplementedException(); } }
    public         int      ZIndex { get; set; }
    public         Rect?    Clip   { get; set; } // 如果有

    public          Stroke   Stroke { get; set; }
    public          Fill     Fill   { get; set; }
    public          Shadow   Shadow  { get; set; }

    public          FontSpec   Font  { get; set; }

    public override int GetHashCode()       // state hash code
    {
        int ret = 0;
        if (Stroke != null)
            ret ^= Stroke.GetHashCode();
       if (Fill != null)
            ret ^= Fill.GetHashCode();
       if (Shadow != null)
            ret ^= Shadow.GetHashCode();
       if (Font != null)
            ret ^= Font.GetHashCode();


       if (Clip != null)
            ret ^= Clip.GetHashCode();
       return ret;
    }


    public virtual void OnDraw(ICanvas c)
    {

    }
}

internal class LineInfo : DrawingInfo // 代表连续的线
{
    public override EDrawing Type   { get { return EDrawing.Line; } }

    public         List<Point> Nodes = new();
    public virtual int         Count { get { return Nodes.Count / 2; } }

    public override void OnDraw(ICanvas c)
    {
        for (int i = 0; i <= Nodes.Count - 2; i++) { c.DrawLine(Nodes[i], Nodes[1]); }
    }
}

internal class RectangleInfo : DrawingInfo
{
    public override EDrawing Type   { get { return EDrawing.Rectangle; } }

    public Rect   Rect         { get; set; }

    public float? CornerRadius { get; set; }        // 圆角矩形

    public override void OnDraw(ICanvas c)
    {
        if (CornerRadius == null)
        {
            if (Fill != null)
            {
                if (Fill.Color != null)
                    c.FillRectangle(Rect);

                if (Fill.Paint != null)
                    c.SetFillPaint(Fill.Paint, Rect);
            }

            c.DrawRectangle(Rect);
        }
        else// 圆角矩形
        {
            if (Fill != null)
            {
                if (Fill.Color != null)
                    c.FillRoundedRectangle(Rect, (double)CornerRadius.Value);

                if (Fill.Paint != null)
                    c.SetFillPaint(Fill.Paint, Rect);
            }

            c.DrawRoundedRectangle(Rect, (double)CornerRadius.Value);
        }
     }
}

internal class EllipseInfo : DrawingInfo
{
    public override EDrawing Type   { get { return EDrawing.Ellipse; } }

    public Rect   Rect         { get; set; }

    public override void OnDraw(ICanvas c)
    {
        if (Fill != null)
        {
            if (Fill.Color != null)
                c.FillEllipse(Rect);
        }

        c.DrawEllipse(Rect);
    }
}

internal class PathInfo : DrawingInfo
{
    public override EDrawing Type   { get { return EDrawing.Path; } }

    public PathF   Path         { get; set; }

    public override void OnDraw(ICanvas c)
    {
        if (Fill != null)
        {
            if (Fill.Color != null)
                c.FillPath(Path);
        }

        c.FillPath(Path);
    }
}

internal class ImageInfo : DrawingInfo
{
    public override EDrawing Type   { get { return EDrawing.Image; } }

    public IImage Image { get; set; }
    public Rect      Rect { get; set; }

    public override void OnDraw(ICanvas c)
    {
        c.DrawImage(Image, (float)Rect.X, (float)Rect.Y, (float)Rect.Width, (float)Rect.Height);
    }
}

internal class StringInfo : DrawingInfo
{
    public override EDrawing Type   { get { return EDrawing.String; } }

    public string              Value                 { get; set; }
    public RectF               Rect                  { get; set; }
    public HorizontalAlignment HorizontalAlignment   { get; set; } = HorizontalAlignment.Left;
    public VerticalAlignment   VerticalAlignment     { get; set; } = VerticalAlignment.Top;
    public TextFlow            TextFlow              { get; set; } = TextFlow.ClipBounds;
    public float               LineSpacingAdjustment { get; set; } = 0;

    public IAttributedText  Text { get; set; }

    public override void OnDraw(ICanvas c)
    {
        if (Value != null)
        {
            if (double.IsNaN(Rect.Width))
                c.DrawString(Value, Rect.X, Rect.Y, HorizontalAlignment);
            else
                c.DrawString(Value, Rect, HorizontalAlignment, VerticalAlignment, TextFlow, LineSpacingAdjustment);
        }

        if (Text != null)
        {
            c.DrawText(Text, Rect.X, Rect.Y, Rect.Width, Rect.Height);
        }
     }
}


//public class TriangleInfo:LineInfo
//{
//    public override EDrawing  Type { get{ return EDrawing.Triangle; } }

//    public Color       FillColor { get; set; }                         // 填充颜色(填充颜色相同的quad可以合并, 否则不能)
//    public override int Count { get { return Nodes.Count / 3; } }
//}


//  public enum TextAlignment
//  {
//      /// <summary>
//      /// <dd> <p>The leading edge of the paragraph text is aligned to the leading edge of the layout box.</p> </dd>
//      /// </summary>
//      /// <doc-id>dd368131</doc-id>
//      /// <unmanaged>DWRITE_TEXT_ALIGNMENT_LEADING</unmanaged>
//      /// <unmanaged-short>DWRITE_TEXT_ALIGNMENT_LEADING</unmanaged-short>
//      Leading,
//      /// <summary>
//      /// <dd> <p>The trailing edge of the paragraph text is aligned to the  trailing edge of the layout box.</p> </dd>
//      /// </summary>
//      /// <doc-id>dd368131</doc-id>
//      /// <unmanaged>DWRITE_TEXT_ALIGNMENT_TRAILING</unmanaged>
//      /// <unmanaged-short>DWRITE_TEXT_ALIGNMENT_TRAILING</unmanaged-short>
//      Trailing,
//      /// <summary>
//      /// <dd> <p>The center of the paragraph text is aligned to the center of the layout box.</p> </dd>
//      /// </summary>
//      /// <doc-id>dd368131</doc-id>
//      /// <unmanaged>DWRITE_TEXT_ALIGNMENT_CENTER</unmanaged>
//      /// <unmanaged-short>DWRITE_TEXT_ALIGNMENT_CENTER</unmanaged-short>
//      Center,
//      /// <summary>
//      /// <dd> <p>Align text to the leading side, and also justify text to fill the lines.</p> </dd>
//      /// </summary>
//      /// <doc-id>dd368131</doc-id>
//      /// <unmanaged>DWRITE_TEXT_ALIGNMENT_JUSTIFIED</unmanaged>
//      /// <unmanaged-short>DWRITE_TEXT_ALIGNMENT_JUSTIFIED</unmanaged-short>
//      Justified,
//  }
//  public enum ParagraphAlignment
//  {
//      /// <summary>
//      /// <dd> <p>The top of the text flow is aligned to the top edge of the layout box.</p> </dd>
//      /// </summary>
//      /// <doc-id>dd368112</doc-id>
//      /// <unmanaged>DWRITE_PARAGRAPH_ALIGNMENT_NEAR</unmanaged>
//      /// <unmanaged-short>DWRITE_PARAGRAPH_ALIGNMENT_NEAR</unmanaged-short>
//      Near,
//      /// <summary>
//      /// <dd> <p>The bottom of the text flow is aligned to the bottom edge of the layout box.</p> </dd>
//      /// </summary>
//      /// <doc-id>dd368112</doc-id>
//      /// <unmanaged>DWRITE_PARAGRAPH_ALIGNMENT_FAR</unmanaged>
//      /// <unmanaged-short>DWRITE_PARAGRAPH_ALIGNMENT_FAR</unmanaged-short>
//      Far,
//      /// <summary>
//      /// <dd> <p>The center of the flow is aligned to the center of the layout box.</p> </dd>
//      /// </summary>
//      /// <doc-id>dd368112</doc-id>
//      /// <unmanaged>DWRITE_PARAGRAPH_ALIGNMENT_CENTER</unmanaged>
//      /// <unmanaged-short>DWRITE_PARAGRAPH_ALIGNMENT_CENTER</unmanaged-short>
//      Center,
//  }

//  public class TextInfo:DrawingInfo          // 简化，暂不提供排版等功能   暂时不支持空格
//  {
//      public override EDrawing Type { get{ return EDrawing.Text; } }

//  // font relative
//public Color FontColor { get; set; }
//public IFont Font      { get; set; }
//public float FontSize  { get; set; }


//      public string       Text { get; set; }
//      public Rect         Rect { get; set; }
//      public Color        ForegroundColor { get; set; }                   // 字颜色
//      public Color?       BackroundColor { get; set; }                    // 背景色

//      public string       FontFamilyName { get; set; }
//      public int          FontSize { get; set; }
//      public TextAlignment TextAlignment { get; set; }
//      public ParagraphAlignment ParagraphAlignment { get; set; }

//      public TextFormat   _Format { get; set; }

//  }

//  public enum BitmapInterpolationMode
//  {
//      /// <summary>No documentation.</summary>
//      /// <doc-id>dd368073</doc-id>
//      /// <unmanaged>D2D1_BITMAP_INTERPOLATION_MODE_NEAREST_NEIGHBOR</unmanaged>
//      /// <unmanaged-short>D2D1_BITMAP_INTERPOLATION_MODE_NEAREST_NEIGHBOR</unmanaged-short>
//      NearestNeighbor,
//      /// <summary>No documentation.</summary>
//      /// <doc-id>dd368073</doc-id>
//      /// <unmanaged>D2D1_BITMAP_INTERPOLATION_MODE_LINEAR</unmanaged>
//      /// <unmanaged-short>D2D1_BITMAP_INTERPOLATION_MODE_LINEAR</unmanaged-short>
//      Linear,
//  }
//  public class ImageInfo:DrawingInfo          //
//  {
//      public override EDrawing Type { get{ return EDrawing.Image; } }

//      // 使用Image对象和iD
//      public System.Drawing.Bitmap Image { get; set; }
//      public string       ImageId { get; set; }

//      // 直接使用ImagePath加载
//      public string       ImagePath { get; set; }

//      public Rect?        DestRect { get; set; }
//      public Rect?        SrcRect { get; set; }

//      public float        Opacity { get; set; }
//      public BitmapInterpolationMode InterpolationMode => BitmapInterpolationMode.Linear;
//  }

//  public class GeometryInfo:DrawingInfo          //
//  {
//      public override EDrawing Type { get{ return EDrawing.Geometry; } }

//      public ELineStyle   StrokeStyle { get; set; }
//      public Color        StrokeColor { get; set; }                             // 线颜色（线颜色相同的线可以合并）
//      public float        StrokWidth { get; set; }
//      public Rect?        Clip { get; set; }

//      public Color?       FillColor { get; set; }                             

//      public List<Point>  Nodes { get; set; } 
//  }