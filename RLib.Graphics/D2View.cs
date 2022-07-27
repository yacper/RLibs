/********************************************************************
    created:	2020/2/20 13:59:59
    author:		rush
    email:		
	
    purpose:	
*********************************************************************/

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using Microsoft.Maui.Graphics;
using Microsoft.Maui.Graphics.Text;
using Microsoft.Toolkit.Mvvm.ComponentModel;

namespace RLib.Graphics;

public class D2View : ObservableObject, ID2View
{
    public double Width  { get => Width_;  set => SetProperty(ref Width_, value); }
    public double Height { get => Height_; set => SetProperty(ref Height_, value); }

    public bool EnableBatching { get; set; } = false; // 开启batching的时候,不保证绘图顺序，须通过zindex自己控制
    public bool IsDirty        { get; set; }          // 是否dirty，如果dirty，需要重新绘制

    public Color Background
    {
        get { return Background_; }
        set
        {
            if (value.Equals(Background))
                return;

            Background_ = value;
            IsDirty     = true;
            OnPropertyChanged();
        }
    }

    public ICanvas Canvas { get; set; }

    public virtual void Render() // 重新绘制
    {
        //Reset();

        OnRendering?.Invoke(this);

        OnRender();

        // draw background
        DrawRectangle(new Rect(0, 0, Width, Height), null, new Fill(){Color = Background}, null, -1);

        // 绘制
        foreach (Dictionary<int, List<DrawingInfo>> infos in DrawingInfos_.OrderBy(p=>p.Key).Select(p=>p.Value))
        {
            foreach (List<DrawingInfo> l in infos.Values)
            {
                Canvas.SaveState();
                var info = l.FirstOrDefault();
                Canvas.Apply(info.Shadow);
                Canvas.Apply(info.Stroke);
                Canvas.Apply(info.Fill);

                if(info.Clip != null)
                    Canvas.ClipRectangle(info.Clip.Value);

                foreach (DrawingInfo d in l) { d.OnDraw(Canvas); }

                Canvas.RestoreState();
            }
        }

        OnRendered?.Invoke(this);
        IsDirty = false;
    }

    internal void RenderDrawingInfo_(DrawingInfo d)
    {
        switch (d.Type)
        {
            case EDrawing.Line:
            {
                //Canvas.SaveState();
                LineInfo info = d as LineInfo; 
                Canvas.Apply(info.Stroke);
             
                if (info.Clip != null)
                    Canvas.ClipRectangle(d.Clip.Value);


                for (int i = 0; i <= info.Nodes.Count - 2; i++) { Canvas.DrawLine(info.Nodes[i], info.Nodes[1]); }

                //Canvas.RestoreState();
                break;
            }
            case EDrawing.Rectangle:
                {
                    //Canvas.SaveState();
                    RectangleInfo info = d as RectangleInfo;
                    if (info.Stroke != null)
                        Canvas.Apply(info.Stroke);
                    if (info.Fill != null)
                        Canvas.Apply(info.Fill);

                    if (info.Clip != null)
                        Canvas.ClipRectangle(d.Clip.Value);


                    ////renderTarget.DrawRectangle(info.Rects[i].ToRawRectangleF(), strokeColor, info.StrokWidth);
                    //if (fillColor != null)
                    //{
                    //    renderTarget.FillRectangle(info.Rect.ToRawRectangleF(), fillColor);
                    //    if (strokeColor != fillColor)
                    //        renderTarget.DrawRectangle(info.Rect.ToRawRectangleF(), strokeColor, info.StrokWidth);
                    //}
                    //else
                    //    renderTarget.DrawRectangle(info.Rect.ToRawRectangleF(), strokeColor, info.StrokWidth);


                    break;
                }
                //case EDrawing.Geometry:
                //{
                //    //todo: style 后面做
                //    GeometryInfo                      info        = d as GeometryInfo;
                //    SharpDX.Direct2D1.SolidColorBrush strokeColor = ResolveSolidColorBrush(info.StrokeColor);
                //    SharpDX.Direct2D1.SolidColorBrush fillColor   = null;
                //    if (info.FillColor != null)
                //        fillColor = ResolveSolidColorBrush(info.FillColor.Value);

                //    using (PathGeometry geo = new PathGeometry(renderTarget.Factory))
                //    {
                //        using (GeometrySink geo_Sink = geo.Open())
                //        {
                //            geo_Sink.BeginFigure(info.Nodes[0].ToRawVector2(), FigureBegin.Filled);
                //            for (int i = 1; i != info.Nodes.Count; ++i)
                //                geo_Sink.AddLine(info.Nodes[i].ToRawVector2());
                //            geo_Sink.EndFigure(FigureEnd.Closed);

                //            geo_Sink.Close();
                //        }

                //        renderTarget.DrawGeometry(geo, strokeColor, info.StrokWidth);
                //        if (info.FillColor != null)
                //            renderTarget.FillGeometry(geo, fillColor);
                //    }

                //    break;
                //}


                //case EDrawing.Text:
                //{
                //    //todo: style 后面做
                //    TextInfo                          info        = d as TextInfo;
                //    SharpDX.Direct2D1.SolidColorBrush strokeColor = ResolveSolidColorBrush(info.ForegroundColor);

                //    // 绘制背景
                //    if (info.BackroundColor != null)
                //    {
                //        SharpDX.Direct2D1.SolidColorBrush fillColor = fillColor = ResolveSolidColorBrush(info.BackroundColor.Value);
                //        renderTarget.FillRectangle(info.Rect.ToRawRectangleF(), fillColor);
                //    }

                //    info._Format.TextAlignment      = (SharpDX.DirectWrite.TextAlignment)info.TextAlignment;
                //    info._Format.ParagraphAlignment = (SharpDX.DirectWrite.ParagraphAlignment)info.ParagraphAlignment;

                //    renderTarget.DrawText(info.Text, info._Format, info.Rect.ToRawRectangleF(), strokeColor);

                //    break;
                //}

                //case EDrawing.Image:
                //{
                //    //todo: style 后面做
                //    ImageInfo                info   = d as ImageInfo;
                //    SharpDX.Direct2D1.Bitmap bitmap = string.IsNullOrWhiteSpace(info.ImagePath) ? ResolveBitmap(info.ImageId, info.Image) : ResolveBitmap(info.ImagePath);

                //    renderTarget.DrawBitmap(bitmap, info.DestRect.ToRawRectangleF(), info.Opacity, (SharpDX.Direct2D1.BitmapInterpolationMode)info.InterpolationMode, info.SrcRect.ToRawRectangleF());

                //    break;
                //}


                //case EDrawing.Triangle:
                //    GL.LineWidth((d as LineInfo).Thinkness);

                //    GL.Begin(BeginMode.Triangles);
                //    GL.Color3(Color.FromArgb(d.Color.ToArgb()));
                //    foreach (var n in (d as TriangleInfo).Nodes)
                //        GL.Vertex2(n.X, n.Y);
                //    GL.End();
                //    break;

                //case EDrawing.Quad:
                //    GL.LineWidth((d as LineInfo).Thinkness);

                //    GL.Begin(BeginMode.Quads);
                //    GL.Color3(Color.FromArgb(d.Color.ToArgb()));
                //    foreach (var n in (d as QuadInfo).Nodes)
                //        GL.Vertex2(n.X, n.Y);
                //    GL.End();
                //    break;

                //case EDrawing.Text:
                //{
                //    GL.MatrixMode(MatrixMode.Projection);
                //    GL.LoadIdentity();
                //    GL.Ortho(0, Width, Height, 0, -1, 1);
                //    GL.Viewport(0, 0, Width, Height); // Use all of the glControl painting area

                //    GL.MatrixMode(MatrixMode.Modelview);
                //    GL.LoadIdentity();

                //    TextInfo ti = d as TextInfo;

                //    ti.Rect = new RectangleF((float)ti.Point.X, (float)(Height - ti.Point.Y - ti.Extents.BoundingBox.Height), ti.Extents.BoundingBox.Width+5, ti.Extents.BoundingBox.Height);


                //    m_pTextPrinter.Print(ti.Text, ti.Font, Color.FromArgb(ti.Color.ToArgb()), ti.Rect, TextPrinterOptions.Default, ti.Alignment);


                //    GL.MatrixMode(MatrixMode.Projection);
                //    GL.LoadIdentity();
                //    GL.Ortho(0, Width, 0, Height, -1, 1); // Bottom-left corner pixel has coordinate (0, 0)
                //    GL.Viewport(0, 0, Width, Height); // Use all of the glControl painting area

                //    GL.MatrixMode(MatrixMode.Modelview);
                //    GL.LoadIdentity();

                //}
                //    break;
        }
    }

  
    public void DrawLine(Point start, Point end, Stroke stroke, Rect? clip = null, int zindex = 0)
    {
        LineInfo li = LineInfoPool.Get();
        li.Stroke = stroke;
        li.ZIndex = zindex;
        li.Clip   = clip;

        li.Nodes.Clear();
        li.Nodes.Add(start);
        li.Nodes.Add(end);

        AddDrawingInfo_(li);
    }

    public void DrawLine(IEnumerable<Point> vs, Stroke stroke, Rect? clip = null, int zindex = 0)
    {
        LineInfo li = LineInfoPool.Get();
        li.Stroke = stroke;
        li.ZIndex = zindex;
        li.Clip   = clip;

        li.Nodes.Clear();
        li.Nodes.AddRange(vs);

        AddDrawingInfo_(li);
    }

    public void DrawRectangle(Rect rect, Stroke stroke, Fill fill = null, Rect? clip = null, int zindex = 0)
    {
        RectangleInfo info = RectPool.Get();
        info.Stroke = stroke;
        info.Fill   = fill;
        info.ZIndex = zindex;
        info.Clip   = clip;

        info.Rect = rect;

        AddDrawingInfo_(info);
    }

    public void DrawRoundedRectangle(Rect rect, float cornerRadius, Stroke stroke, Fill fill = null, Rect? clip = null, int zindex = 0)
    {
        RectangleInfo info = RectPool.Get();
        info.Stroke = stroke;
        info.Fill   = fill;
        info.ZIndex = zindex;
        info.Clip   = clip;

        info.Rect         = rect;
        info.CornerRadius = cornerRadius;

        AddDrawingInfo_(info);

    }


    public void DrawEllipse(Rect rect, Stroke stroke, Fill fill = null, Rect? clip = null, int zindex = 0)
    {
        EllipseInfo info = EllipsePool.Get();
        info.Stroke = stroke;
        info.Fill   = fill;
        info.ZIndex = zindex;
        info.Clip   = clip;

        info.Rect         = rect;

        AddDrawingInfo_(info);
    }

    public void DrawPath(PathF path, Stroke stroke, Fill fill =null, WindingMode windingMode =WindingMode.NonZero, Rect? clip = null, int zindex = 0)
    {
        PathInfo info = PathPool.Get();
        info.Stroke = stroke;
        info.Fill   = fill;
        info.ZIndex = zindex;
        info.Clip   = clip;

        info.Path         = path;

        AddDrawingInfo_(info);
    }

    public void DrawImage(IImage image, Rect rect, Stroke stroke, Fill fill = null, Rect? clip = null, int zindex = 0)
    {
        ImageInfo info = ImagePool.Get();
        info.Stroke = stroke;
        info.Fill   = fill;
        info.ZIndex = zindex;
        info.Clip   = clip;

        info.Image         = image;
        info.Rect         = rect;

        AddDrawingInfo_(info);
    }

    public void DrawString(string value, Point pt, FontSpec font, HorizontalAlignment horizontalAlignment, Rect? clip = null, int zindex = 0)
    {
        DrawString(value, new Rect(pt.X, pt.Y, double.NaN, double.NaN), font, horizontalAlignment, VerticalAlignment.Top, TextFlow.ClipBounds, 0, clip, zindex);
    }

    public void DrawString(string value, Rect rect, FontSpec font, HorizontalAlignment horizontalAlignment, VerticalAlignment verticalAlignment = VerticalAlignment.Center, TextFlow textFlow = TextFlow.ClipBounds,
        float                     lineSpacingAdjustment = 0, Rect? clip = null, int zindex = 0)
    {
        StringInfo info = StringPool.Get();
        info.Font   = font;
        info.ZIndex = zindex;
        info.Clip   = clip;

        info.Value               = value;
        info.Rect                = rect;
        info.HorizontalAlignment = horizontalAlignment;
        info.VerticalAlignment = verticalAlignment;
        info.TextFlow = textFlow;
        info.LineSpacingAdjustment = lineSpacingAdjustment;

        AddDrawingInfo_(info);
    }

    public SizeF GetStringSize(string value, IFont font, float fontSize) => Canvas.GetStringSize(value, font, fontSize);

    public SizeF GetStringSize(string value, IFont font, float fontSize, HorizontalAlignment horizontalAlignment, VerticalAlignment verticalAlignment) =>
        Canvas.GetStringSize(value, font, fontSize, horizontalAlignment, verticalAlignment);


    public void DrawText(IAttributedText value, Rect rect, FontSpec font, Rect? clip = null, int zindex = 0)
    {
        StringInfo info = StringPool.Get();
        info.Font   = font;
        info.ZIndex = zindex;
        info.Clip   = clip;

        info.Text               = value;
        info.Rect                = rect;

        AddDrawingInfo_(info);
    }




    //void                DrawTriangle(Point a, Point b, Point c, Color clr, int zindex = 0);

    //void                DrawQuad(Point a, Point b, Point c, Point d, Color clr, int zindex = 0);
    //void                DrawQuad(Point ld, Point ur, Color clr, int zindex = 0);

    //view.DrawText("你好，世界 hello, world 123456", "微软雅黑", 16, new Rect(new Point(300, 300), new Size(100, 100)), Colors.Black);
    //public TextInfo DrawText(string text,                                 string             fontFamlity, int fontSize, Rect rect, Color foregroundColor, Color? backColor = null,
    //    TextAlignment               textAlignment = TextAlignment.Center, ParagraphAlignment paragraphAlignment = ParagraphAlignment.Center, int zindex = 0)
    //{
    //    TextInfo ti = TextPool.Get();
    //    ti.ForegroundColor = foregroundColor;
    //    ti.BackroundColor  = backColor;
    //    ti.Text            = text;
    //    ti.Rect            = rect;
    //    ti.ZIndex          = zindex;

    //    ti.FontFamilyName = fontFamlity;
    //    ti.FontSize       = fontSize;
    //    ti._Format        = ResolveTextFormat(fontFamlity, fontSize);

    //    ti.TextAlignment      = textAlignment;
    //    ti.ParagraphAlignment = paragraphAlignment;

    //    //ti.Extents = m_pTextPrinter.Measure(text, ti.Font, new RectangleF(0, 0, 1000, 100), TextPrinterOptions.Default, alignment);

    //    //ti.Rect = new RectangleF((float)point.X, (float)(Height - point.Y - te.BoundingBox.Height), te.BoundingBox.Width+5, te.BoundingBox.Height);

    //    AddDrawingInfo_(ti);

    //    return ti;
    //}

    //public void DrawImage(System.Drawing.Bitmap image,                                              string imageId,                Rect? destinationRectangle = null, float opacity = 1.0f,
    //    BitmapInterpolationMode                 interpolationMode = BitmapInterpolationMode.Linear, Rect?  sourceRectangle = null, int   zindex               = 0)
    //{
    //    ImageInfo info = ImagePool.Get();
    //    info.Image   = image;
    //    info.ImageId = imageId;

    //    info.DestRect = destinationRectangle;
    //    info.SrcRect  = sourceRectangle;
    //    info.Opacity  = opacity;

    //    AddDrawingInfo_(info);
    //}

    //public void DrawImage(string imageFilePath,                                      Rect? destinationRectangle = null, float opacity = 1.0f,
    //    BitmapInterpolationMode  interpolationMode = BitmapInterpolationMode.Linear, Rect? sourceRectangle      = null,
    //    int                      zindex            = 0)
    //{
    //    ImageInfo info = ImagePool.Get();
    //    info.ImagePath = imageFilePath;

    //    info.DestRect = destinationRectangle;
    //    info.SrcRect  = sourceRectangle;
    //    info.Opacity  = opacity;

    //    AddDrawingInfo_(info);
    //}

    //public void DrawGeometry(List<Point> points,                             Color  strokeColor,      float strokeWidth = 1,
    //    ELineStyle                       strokeStyle = ELineStyle.LineSolid, Color? fillColor = null, int   zindex      = 0)
    //{
    //    GeometryInfo info = GeoPool.Get();
    //    info.Nodes       = points;
    //    info.StrokeColor = strokeColor;
    //    info.StrokWidth  = strokeWidth;
    //    info.StrokeStyle = strokeStyle;
    //    info.FillColor   = fillColor;

    //    AddDrawingInfo_(info);
    //}

    //public TextFormat ResolveTextFormat(string fontFamlity, int fontSize)
    //{
    //    TextFormat ret = _TextFormats.FirstOrDefault(p => p.FontFamilyName == fontFamlity && p.FontSize == fontSize);
    //    if (ret == null)
    //    {
    //        ret = new TextFormat(FactoryDWrite, fontFamlity, fontSize);

    //        _TextFormats.Add(ret);
    //    }

    //    return ret;
    //}

    //public D2D.SolidColorBrush ResolveSolidColorBrush(Color c)
    //{
    //    string id = c.ToARGB().ToString();

    //    object ret;
    //    if (resCache.TryGetValue(id, out ret)) { return ret as D2D.SolidColorBrush; }

    //    resCache.Add(id, t => new D2D.SolidColorBrush(t, c.ToRawColor4()));

    //    return resCache[id] as D2D.SolidColorBrush;
    //}

    //public D2D.SolidColorBrush   ResolveSolidColorBrush(RawColor4 c)
    //{
    //    D2D.SolidColorBrush ret = resCache.Values.OfType<D2D.SolidColorBrush>().FirstOrDefault(p => p.Color.Equal(c));
    //    if (ret == null)
    //    {
    //        string id = Guid.NewGuid().ToString();
    //        resCache.Add(id, t => new D2D.SolidColorBrush( t, c) );
    //        ret = resCache[id] as D2D.SolidColorBrush;
    //    }

    //    return ret;
    //}

    //public D2D.StrokeStyle ResolveStrokStype(D2D.DashStyle style)
    //{
    //    D2D.StrokeStyle ret = _StrokeStyles.FirstOrDefault(p => p.DashStyle == style);
    //    if (ret == null)
    //    {
    //        ret = new D2D.StrokeStyle(d2DFactory, new StrokeStyleProperties() { DashStyle = style });

    //        _StrokeStyles.Add(ret);
    //    }

    //    return ret;
    //}

    //public SharpDX.Direct2D1.Bitmap ResolveBitmap(string imagePath)
    //{
    //    string id = $"Img:{imagePath}";
    //    if (!resCache.ContainsKey(id))
    //        resCache.Add(id, t => SharpDxHelper.LoadFromFile(t, imagePath));

    //    return resCache[id] as SharpDX.Direct2D1.Bitmap;
    //}

    //public SharpDX.Direct2D1.Bitmap ResolveBitmap(string imageId, System.Drawing.Bitmap bm)
    //{
    //    string id = $"Img:{imageId}";
    //    if (!resCache.ContainsKey(id))
    //        resCache.Add(id, t => SharpDxHelper.LoadBitmap(t, bm));

    //    return resCache[id] as SharpDX.Direct2D1.Bitmap;
    //}


    internal void AddDrawingInfo_(DrawingInfo d)
    {
        // 根据state hashcode分组

        Dictionary<int, List<DrawingInfo>> dic = null;
        if (!DrawingInfos_.TryGetValue(d.ZIndex, out dic))
        {
            DrawingInfos_.Add(d.ZIndex, new() { });
            DrawingInfos_.TryGetValue(d.ZIndex, out dic);
        }

        int               code = d.GetHashCode();
        List<DrawingInfo> l    = null;
        if (!dic.TryGetValue(code, out l))
            dic[code] = new List<DrawingInfo>(){d};
        else
           l.Add(d);
    }

    public void Reset() // 删除所有绘制元素
    {
        DrawingInfos_.Clear();


        LineInfoPool.Reset();
        RectPool.Reset();
        //TextPool.Reset();
        //ImagePool.Reset();
        //GeoPool.Reset();
    }

    public virtual void OnRender() { }

    public event Action<ID2View> OnRendering;
    public event Action<ID2View> OnRendered;

    protected double Width_;
    protected double Height_;


    internal Dictionary<int, Dictionary<int, List<DrawingInfo>>> DrawingInfos_ = new();
    protected Color                         Background_   = Colors.Black;
//    protected List<TextFormat>                          _TextFormats  = new List<TextFormat>();

    internal ObjectPool<LineInfo> LineInfoPool = new ObjectPool<LineInfo>(200000);

    internal ObjectPool<RectangleInfo> RectPool = new ObjectPool<RectangleInfo>(200000);
    internal ObjectPool<EllipseInfo> EllipsePool = new (2000);
    internal ObjectPool<PathInfo> PathPool = new (2000);
    internal ObjectPool<ImageInfo> ImagePool = new (200);
    internal ObjectPool<StringInfo> StringPool = new (200);
    //public static ObjectPool<TextInfo>      TextPool     = new ObjectPool<TextInfo>(5000);
    //public static ObjectPool<ImageInfo>     ImagePool    = new ObjectPool<ImageInfo>(5000);
    //public static ObjectPool<GeometryInfo>  GeoPool      = new ObjectPool<GeometryInfo>(5000);
}

//public static class ELineStyleEx
//{
//    public static D2D.DashStyle ToDashStyle(this ELineStyle style)
//    {
//        switch (style)
//        {
//            case ELineStyle.LineSolid:
//                return D2D.DashStyle.Solid;
//            case ELineStyle.LineDash:
//                return D2D.DashStyle.Dash;
//            case ELineStyle.LineDashdot:
//                return D2D.DashStyle.DashDot;
//            default:
//                return D2D.DashStyle.Solid;
//        }
//    }
//}