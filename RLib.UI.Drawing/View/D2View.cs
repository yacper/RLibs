/********************************************************************
    created:	2020/2/20 13:59:59
    author:		rush
    email:		
	
    purpose:	
*********************************************************************/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using DataModel;

using RLib.Base;
using SharpDX.Direct2D1;
using SharpDX.DirectWrite;
using SharpDX.Mathematics.Interop;
using Wintellect.PowerCollections;
using D2D = SharpDX.Direct2D1;
using PathGeometry = SharpDX.Direct2D1.PathGeometry;
using SolidColorBrush = System.Windows.Media.SolidColorBrush;

namespace RLib.UI.Drawing
{
    public class D2View:D2dControl, ID2View
    {
#region observableObject
  /// <summary>
        /// Occurs after a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Provides access to the PropertyChanged event handler to derived classes.
        /// </summary>
        protected PropertyChangedEventHandler PropertyChangedHandler
        {
            get
            {
                return PropertyChanged;
            }
        }

#if !PORTABLE && !SL4
        /// <summary>
        /// Occurs before a property value changes.
        /// </summary>
        public event PropertyChangingEventHandler PropertyChanging;

        /// <summary>
        /// Provides access to the PropertyChanging event handler to derived classes.
        /// </summary>
        protected PropertyChangingEventHandler PropertyChangingHandler
        {
            get
            {
                return PropertyChanging;
            }
        }
#endif

        /// <summary>
        /// Verifies that a property name exists in this ViewModel. This method
        /// can be called before the property is used, for instance before
        /// calling RaisePropertyChanged. It avoids errors when a property name
        /// is changed but some places are missed.
        /// </summary>
        /// <remarks>This method is only active in DEBUG mode.</remarks>
        /// <param name="propertyName">The name of the property that will be
        /// checked.</param>
        [Conditional("DEBUG")]
        [DebuggerStepThrough]
        public void VerifyPropertyName(string propertyName)
        {
//#if DEBUG
            return;                 // verify 比较耗效率，不使用了
//#endif

            var myType = GetType();

#if NETFX_CORE
            var info = myType.GetTypeInfo();

            if (!string.IsNullOrEmpty(propertyName)
                && info.GetDeclaredProperty(propertyName) == null)
            {
                // Check base types
                var found = false;

                while (info.BaseType != typeof(Object))
                {
                    info = info.BaseType.GetTypeInfo();

                    if (info.GetDeclaredProperty(propertyName) != null)
                    {
                        found = true;
                        break;
                    }
                }

                if (!found)
                {
                    throw new ArgumentException("Property not found", propertyName);
                }
            }
#else
            if (!string.IsNullOrEmpty(propertyName)
                && myType.GetProperty(propertyName) == null)
            {
#if !SILVERLIGHT
                var descriptor = this as ICustomTypeDescriptor;

                if (descriptor != null)
                {
                    if (descriptor.GetProperties()
                        .Cast<PropertyDescriptor>()
                        .Any(property => property.Name == propertyName))
                    {
                        return;
                    }
                }
#endif

                throw new ArgumentException("Property not found", propertyName);
            }
#endif
        }

#if !PORTABLE && !SL4
#if CMNATTR
        /// <summary>
        /// Raises the PropertyChanging event if needed.
        /// </summary>
        /// <remarks>If the propertyName parameter
        /// does not correspond to an existing property on the current class, an
        /// exception is thrown in DEBUG configuration only.</remarks>
        /// <param name="propertyName">(optional) The name of the property that
        /// changed.</param>
        [SuppressMessage(
            "Microsoft.Design", 
            "CA1030:UseEventsWhereAppropriate",
            Justification = "This cannot be an event")]
        public virtual void RaisePropertyChanging(
            [CallerMemberName] string propertyName = null)
#else
        /// <summary>
        /// Raises the PropertyChanging event if needed.
        /// </summary>
        /// <remarks>If the propertyName parameter
        /// does not correspond to an existing property on the current class, an
        /// exception is thrown in DEBUG configuration only.</remarks>
        /// <param name="propertyName">The name of the property that
        /// changed.</param>
        [SuppressMessage(
            "Microsoft.Design", 
            "CA1030:UseEventsWhereAppropriate",
            Justification = "This cannot be an event")]
        public virtual void RaisePropertyChanging(
            string propertyName)
#endif
        {
            VerifyPropertyName(propertyName);

            var handler = PropertyChanging;
            if (handler != null)
            {
                handler(this, new PropertyChangingEventArgs(propertyName));
            }
        }
#endif

        // 带old mod:2018/9/17
        public virtual void RaisePropertyChanging(string propertyName, object newval, object oldval) 
        {
            VerifyPropertyName(propertyName);

            var handler = PropertyChanging;
            if (handler != null)
            {
                handler(this, new PropertyChangingEventArgsEx(propertyName, newval, oldval));
            }
        }


#if CMNATTR
        /// <summary>
        /// Raises the PropertyChanged event if needed.
        /// </summary>
        /// <remarks>If the propertyName parameter
        /// does not correspond to an existing property on the current class, an
        /// exception is thrown in DEBUG configuration only.</remarks>
        /// <param name="propertyName">(optional) The name of the property that
        /// changed.</param>
        [SuppressMessage(
            "Microsoft.Design", 
            "CA1030:UseEventsWhereAppropriate",
            Justification = "This cannot be an event")]
        public virtual void RaisePropertyChanged(
            [CallerMemberName] string propertyName = null)
#else
        /// <summary>
        /// Raises the PropertyChanged event if needed.
        /// </summary>
        /// <remarks>If the propertyName parameter
        /// does not correspond to an existing property on the current class, an
        /// exception is thrown in DEBUG configuration only.</remarks>
        /// <param name="propertyName">The name of the property that
        /// changed.</param>
        [SuppressMessage(
            "Microsoft.Design", 
            "CA1030:UseEventsWhereAppropriate",
            Justification = "This cannot be an event")]
        public virtual void RaisePropertyChanged(
            string propertyName) 
#endif
        {
            VerifyPropertyName(propertyName);

            var handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }


        // 带old mod:2018/6/21
        public virtual void RaisePropertyChanged(string propertyName, object newval, object oldval) 
        {
            VerifyPropertyName(propertyName);

            var handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgsEx(propertyName, newval, oldval));
            }
        }

#if !PORTABLE && !SL4
        /// <summary>
        /// Raises the PropertyChanging event if needed.
        /// </summary>
        /// <typeparam name="T">The type of the property that
        /// changes.</typeparam>
        /// <param name="propertyExpression">An expression identifying the property
        /// that changes.</param>
        [SuppressMessage(
            "Microsoft.Design", 
            "CA1030:UseEventsWhereAppropriate",
            Justification = "This cannot be an event")]
        [SuppressMessage(
            "Microsoft.Design",
            "CA1006:GenericMethodsShouldProvideTypeParameter",
            Justification = "This syntax is more convenient than other alternatives.")]
        public virtual void RaisePropertyChanging<T>(Expression<Func<T>> propertyExpression)
        {
            var handler = PropertyChanging;
            if (handler != null)
            {
                var propertyName = GetPropertyName(propertyExpression);
                handler(this, new PropertyChangingEventArgs(propertyName));
            }
        }
#endif

        /// <summary>
        /// Raises the PropertyChanged event if needed.
        /// </summary>
        /// <typeparam name="T">The type of the property that
        /// changed.</typeparam>
        /// <param name="propertyExpression">An expression identifying the property
        /// that changed.</param>
        [SuppressMessage(
            "Microsoft.Design", 
            "CA1030:UseEventsWhereAppropriate",
            Justification = "This cannot be an event")]
        [SuppressMessage(
            "Microsoft.Design",
            "CA1006:GenericMethodsShouldProvideTypeParameter",
            Justification = "This syntax is more convenient than other alternatives.")]
        public virtual void RaisePropertyChanged<T>(Expression<Func<T>> propertyExpression)
        {
            var handler = PropertyChanged;

            if (handler != null)
            {
                var propertyName = GetPropertyName(propertyExpression);

                if (!string.IsNullOrEmpty(propertyName))
                {
                    // ReSharper disable once ExplicitCallerInfoArgument
                    RaisePropertyChanged(propertyName);
                }
            }
        }

        /// <summary>
        /// Extracts the name of a property from an expression.
        /// </summary>
        /// <typeparam name="T">The type of the property.</typeparam>
        /// <param name="propertyExpression">An expression returning the property's name.</param>
        /// <returns>The name of the property returned by the expression.</returns>
        /// <exception cref="ArgumentNullException">If the expression is null.</exception>
        /// <exception cref="ArgumentException">If the expression does not represent a property.</exception>
        [SuppressMessage(
            "Microsoft.Design", 
            "CA1011:ConsiderPassingBaseTypesAsParameters",
            Justification = "This syntax is more convenient than the alternatives."), 
         SuppressMessage(
            "Microsoft.Design",
            "CA1006:DoNotNestGenericTypesInMemberSignatures",
            Justification = "This syntax is more convenient than the alternatives.")]
        protected static string GetPropertyName<T>(Expression<Func<T>> propertyExpression)
        {
            if (propertyExpression == null)
            {
                throw new ArgumentNullException("propertyExpression");
            }

            var body = propertyExpression.Body as MemberExpression;

            if (body == null)
            {
                throw new ArgumentException("Invalid argument", "propertyExpression");
            }

            var property = body.Member as PropertyInfo;

            if (property == null)
            {
                throw new ArgumentException("Argument is not a property", "propertyExpression");
            }

            return property.Name;
        }

        /// <summary>
        /// Assigns a new value to the property. Then, raises the
        /// PropertyChanged event if needed. 
        /// </summary>
        /// <typeparam name="T">The type of the property that
        /// changed.</typeparam>
        /// <param name="propertyExpression">An expression identifying the property
        /// that changed.</param>
        /// <param name="field">The field storing the property's value.</param>
        /// <param name="newValue">The property's value after the change
        /// occurred.</param>
        /// <returns>True if the PropertyChanged event has been raised,
        /// false otherwise. The event is not raised if the old
        /// value is equal to the new value.</returns>
        [SuppressMessage(
            "Microsoft.Design",
            "CA1006:DoNotNestGenericTypesInMemberSignatures",
            Justification = "This syntax is more convenient than the alternatives."), 
         SuppressMessage(
            "Microsoft.Design", 
            "CA1045:DoNotPassTypesByReference",
            MessageId = "1#",
            Justification = "This syntax is more convenient than the alternatives.")]
        protected bool Set<T>(
            Expression<Func<T>> propertyExpression,
            ref T field,
            T newValue)
        {
            if (EqualityComparer<T>.Default.Equals(field, newValue))
            {
                return false;
            }

#if !PORTABLE && !SL4
            RaisePropertyChanging(propertyExpression);
#endif
            field = newValue;
            RaisePropertyChanged(propertyExpression);
            return true;
        }

        /// <summary>
        /// Assigns a new value to the property. Then, raises the
        /// PropertyChanged event if needed. 
        /// </summary>
        /// <typeparam name="T">The type of the property that
        /// changed.</typeparam>
        /// <param name="propertyName">The name of the property that
        /// changed.</param>
        /// <param name="field">The field storing the property's value.</param>
        /// <param name="newValue">The property's value after the change
        /// occurred.</param>
        /// <returns>True if the PropertyChanged event has been raised,
        /// false otherwise. The event is not raised if the old
        /// value is equal to the new value.</returns>
        [SuppressMessage(
            "Microsoft.Design", 
            "CA1045:DoNotPassTypesByReference",
            MessageId = "1#",
            Justification = "This syntax is more convenient than the alternatives.")]
        protected bool Set<T>(
            string propertyName,
            ref T field,
            T newValue)
        {
            if (EqualityComparer<T>.Default.Equals(field, newValue))
            {
                return false;
            }

#if !PORTABLE && !SL4
            RaisePropertyChanging(propertyName);
#endif
            T old = field;      // mod:2018/6/21

            field = newValue;

            // ReSharper disable ExplicitCallerInfoArgument
            RaisePropertyChanged(propertyName, newValue, old);
            // ReSharper restore ExplicitCallerInfoArgument
            
            return true;
        }

#if CMNATTR
        /// <summary>
        /// Assigns a new value to the property. Then, raises the
        /// PropertyChanged event if needed. 
        /// </summary>
        /// <typeparam name="T">The type of the property that
        /// changed.</typeparam>
        /// <param name="field">The field storing the property's value.</param>
        /// <param name="newValue">The property's value after the change
        /// occurred.</param>
        /// <param name="propertyName">(optional) The name of the property that
        /// changed.</param>
        /// <returns>True if the PropertyChanged event has been raised,
        /// false otherwise. The event is not raised if the old
        /// value is equal to the new value.</returns>
        protected bool Set<T>(
            ref T field,
            T newValue,
            [CallerMemberName] string propertyName = null)
        {
            return Set(propertyName, ref field, newValue);
        }
#endif


#endregion


        public Color         Background
        {
            get { return _Background; }
            set
            {
                if (value == Background)
                    return;

                _Background = value;
                Dirty = true;
                RaisePropertyChanged("Background");
            }
        }                              // 背景



        public override void Render(SharpDX.Direct2D1.RenderTarget renderTarget)
        {
            // todo:以后再优化, 这个OnRender位置不应放这里
            ClearDrawings();
            renderTarget.Clear(Background.ToRawColor4());

            OnRender();
            RenderEx?.Invoke(this);

            // 现在用的directDraw无法合并批次，以后有时间使用d3d合并
            foreach (KeyValuePair<int, List<DrawingInfo>> kv in _DrawingInfos)
            {
                foreach (DrawingInfo d in kv.Value)
                {
                    _RenderDrawingInfo(renderTarget, d);
                }
            }


            LineInfoPool.Reset();
            RectPool.Reset();
            TextPool.Reset();
            ImagePool.Reset();
            GeoPool.Reset();
        }

        protected void      _RenderDrawingInfo(SharpDX.Direct2D1.RenderTarget renderTarget, DrawingInfo d)
        {
            switch (d.Type)
            {
                case EDrawing.Line:
                    {
                        //todo: style 后面做
                        LineInfo info = d as LineInfo;
                        SharpDX.Direct2D1.SolidColorBrush strokeColor = ResolveSolidColorBrush(info.StrokeColor);
                        StrokeStyle st = ResolveStrokStype(info.StrokeStyle.ToDashStyle());

                        if(info.Clip != null)
                            renderTarget.PushAxisAlignedClip(info.Clip.Value.ToRawRectangleF(), AntialiasMode.Aliased);

                        for (int i = 0; i <= info.Nodes.Count - 2; i++)
                        {
                            renderTarget.DrawLine(info.Nodes[i].ToRawVector2(), info.Nodes[i + 1].ToRawVector2(), strokeColor, info.StrokWidth, st);
                        }

                        if(info.Clip != null)
                            renderTarget.PopAxisAlignedClip();
                        break;
                    }
                case EDrawing.Rectangle:
                    {
                        //todo: style 后面做
                        RectangleInfo info = d as RectangleInfo;
                        SharpDX.Direct2D1.SolidColorBrush strokeColor = ResolveSolidColorBrush(info.StrokeColor);
                        SharpDX.Direct2D1.SolidColorBrush fillColor = null;
                        if (info.FillColor != null)
                            fillColor = ResolveSolidColorBrush(info.FillColor.Value);
                     
                            //renderTarget.DrawRectangle(info.Rects[i].ToRawRectangleF(), strokeColor, info.StrokWidth);
                            if (fillColor != null)
                            {
                                renderTarget.FillRectangle(info.Rect.ToRawRectangleF(), fillColor);
                                if (strokeColor != fillColor)
                                    renderTarget.DrawRectangle(info.Rect.ToRawRectangleF(), strokeColor, info.StrokWidth);

                            }
                            else
                                renderTarget.DrawRectangle(info.Rect.ToRawRectangleF(), strokeColor, info.StrokWidth);
                       

                        break;
                    }
                case EDrawing.Geometry:
                    {
                        //todo: style 后面做
                        GeometryInfo info = d as GeometryInfo;
                        SharpDX.Direct2D1.SolidColorBrush strokeColor = ResolveSolidColorBrush(info.StrokeColor);
                        SharpDX.Direct2D1.SolidColorBrush fillColor = null;
                        if (info.FillColor != null)
                            fillColor = ResolveSolidColorBrush(info.FillColor.Value);

                        using (PathGeometry geo = new PathGeometry(renderTarget.Factory))
                        {
                            using (GeometrySink geo_Sink = geo.Open())
                            {
                                geo_Sink.BeginFigure(info.Nodes[0].ToRawVector2(), FigureBegin.Filled);
                                for (int i = 1; i != info.Nodes.Count; ++i)
                                    geo_Sink.AddLine(info.Nodes[i].ToRawVector2());
                                geo_Sink.EndFigure(FigureEnd.Closed);

                                geo_Sink.Close();
                            }

                            renderTarget.DrawGeometry(geo, strokeColor, info.StrokWidth);
                            if(info.FillColor != null)
                                renderTarget.FillGeometry(geo, fillColor);
                        }
                        break;
                    }


                case EDrawing.Text:
                    {
                        //todo: style 后面做
                        TextInfo info = d as TextInfo;
                        SharpDX.Direct2D1.SolidColorBrush strokeColor = ResolveSolidColorBrush(info.ForegroundColor);

                        // 绘制背景
                        if(info.BackroundColor != null)
                        {
                            SharpDX.Direct2D1.SolidColorBrush fillColor = fillColor = ResolveSolidColorBrush(info.BackroundColor.Value);
                            renderTarget.FillRectangle(info.Rect.ToRawRectangleF(), fillColor);
                        }

                        info._Format.TextAlignment = (SharpDX.DirectWrite.TextAlignment)info.TextAlignment;
                        info._Format.ParagraphAlignment = (SharpDX.DirectWrite.ParagraphAlignment)info.ParagraphAlignment;

                        renderTarget.DrawText(info.Text, info._Format, info.Rect.ToRawRectangleF(), strokeColor);

                        break;
                    }

                case EDrawing.Image:
                    {
                        //todo: style 后面做
                        ImageInfo info = d as ImageInfo;
                        SharpDX.Direct2D1.Bitmap bitmap = string.IsNullOrWhiteSpace(info.ImagePath)?ResolveBitmap(info.ImageId, info.Image):ResolveBitmap(info.ImagePath);

                        renderTarget.DrawBitmap(bitmap, info.DestRect.ToRawRectangleF(), info.Opacity, (SharpDX.Direct2D1.BitmapInterpolationMode)info.InterpolationMode, info.SrcRect.ToRawRectangleF());

                        break;
                    }


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



        public void         DrawLine(Point start, Point end, Color strokeColor, int strokeWidth = 1, ELineStyle strokeStyle = ELineStyle.LineSolid,Rect? clip = null, int zindex = 0)
        {
            LineInfo li = LineInfoPool.Get();
            li.StrokeColor = strokeColor; 
            li.StrokWidth = strokeWidth;
            li.StrokeStyle = strokeStyle;
            li.ZIndex = zindex;
            li.Clip = clip;

            li.Nodes.Clear();
            li.Nodes.Add(start);
            li.Nodes.Add(end);

            __AddDrawingInfo(li);
        }

        public void         DrawLine(List<Point> vs, Color strokeColor, int strokeWidth = 1, ELineStyle strokeStyle = ELineStyle.LineSolid,Rect? clip = null, int zindex = 0)
        {
            LineInfo li = LineInfoPool.Get();
            li.StrokeColor = strokeColor; 
            li.StrokWidth = strokeWidth;
            li.StrokeStyle = strokeStyle;
            li.ZIndex = zindex;
            li.Clip = clip;

            li.Nodes.Clear();
            li.Nodes.AddRange(vs);

            __AddDrawingInfo(li);
        }


        public void         DrawRectangle(Rect rect, Color strokeColor, int strokeWidth = 1, ELineStyle strokeStyle = ELineStyle.LineSolid, Color? fillColor = null, int zindex = 0)
        {
            RectangleInfo info = RectPool.Get();
            info.StrokeColor = strokeColor; 
            info.StrokWidth = strokeWidth;
            info.StrokeStyle = strokeStyle;
            info.ZIndex = zindex;
            info.FillColor = fillColor;

            info.Rect = rect;

            __AddDrawingInfo(info);
        }

        //void                DrawTriangle(Point a, Point b, Point c, Color clr, int zindex = 0);

        //void                DrawQuad(Point a, Point b, Point c, Point d, Color clr, int zindex = 0);
        //void                DrawQuad(Point ld, Point ur, Color clr, int zindex = 0);

        //view.DrawText("你好，世界 hello, world 123456", "微软雅黑", 16, new Rect(new Point(300, 300), new Size(100, 100)), Colors.Black);
        public TextInfo            DrawText(string text, string fontFamlity, int fontSize, Rect rect, Color foregroundColor, Color? backColor = null,
            TextAlignment textAlignment = TextAlignment.Center, ParagraphAlignment paragraphAlignment=ParagraphAlignment.Center, int zindex = 0)
        {
            TextInfo ti = TextPool.Get();
            ti.ForegroundColor = foregroundColor;
            ti.BackroundColor = backColor;
            ti.Text = text;
            ti.Rect = rect;
            ti.ZIndex = zindex;

            ti.FontFamilyName = fontFamlity;
            ti.FontSize = fontSize;
            ti._Format = ResolveTextFormat(fontFamlity, fontSize);

            ti.TextAlignment = textAlignment;
            ti.ParagraphAlignment = paragraphAlignment;

            //ti.Extents = m_pTextPrinter.Measure(text, ti.Font, new RectangleF(0, 0, 1000, 100), TextPrinterOptions.Default, alignment);

            //ti.Rect = new RectangleF((float)point.X, (float)(Height - point.Y - te.BoundingBox.Height), te.BoundingBox.Width+5, te.BoundingBox.Height);

            __AddDrawingInfo(ti);

            return ti;
        }

        public void         DrawImage(System.Drawing.Bitmap image, string imageId, Rect? destinationRectangle= null, float opacity =1.0f, BitmapInterpolationMode interpolationMode= BitmapInterpolationMode.Linear, Rect? sourceRectangle= null, int zindex = 0)
        {
            ImageInfo info = ImagePool.Get();
            info.Image = image;
            info.ImageId = imageId;

            info.DestRect = destinationRectangle;
            info.SrcRect = sourceRectangle;
            info.Opacity = opacity;

            __AddDrawingInfo(info);
        }

        public void         DrawImage(string imageFilePath, Rect? destinationRectangle = null, float opacity = 1.0f,
            BitmapInterpolationMode interpolationMode = BitmapInterpolationMode.Linear, Rect? sourceRectangle = null,
            int zindex = 0)
        {
            ImageInfo info = ImagePool.Get();
            info.ImagePath = imageFilePath;

            info.DestRect = destinationRectangle;
            info.SrcRect = sourceRectangle;
            info.Opacity = opacity;

            __AddDrawingInfo(info);
        }

        public void         DrawGeometry(List<Point> points, Color strokeColor, float strokeWidth = 1,
            ELineStyle strokeStyle = ELineStyle.LineSolid, Color? fillColor = null, int zindex = 0)
        {

            GeometryInfo info = GeoPool.Get();
            info.Nodes = points;
            info.StrokeColor = strokeColor;
            info.StrokWidth = strokeWidth;
            info.StrokeStyle = strokeStyle;
            info.FillColor = fillColor;

            __AddDrawingInfo(info);
        }

        public TextFormat   ResolveTextFormat(string fontFamlity, int fontSize)
        {
            TextFormat ret = _TextFormats.FirstOrDefault(p => p.FontFamilyName == fontFamlity && p.FontSize == fontSize);
            if (ret == null)
            {
                ret = new TextFormat(FactoryDWrite, fontFamlity, fontSize);

                _TextFormats.Add(ret);
            }

            return ret;
        }

        public D2D.SolidColorBrush ResolveSolidColorBrush(Color c)
        {
            string id = c.ToARGB().ToString();

            object ret;
            if (resCache.TryGetValue(id, out ret))
            {
                return ret as D2D.SolidColorBrush;
            }

            resCache.Add(id, t => new D2D.SolidColorBrush( t, c.ToRawColor4()) );

            return resCache[id] as D2D.SolidColorBrush;
        }

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

        public D2D.StrokeStyle   ResolveStrokStype(D2D.DashStyle style)
        {
            D2D.StrokeStyle ret = _StrokeStyles.FirstOrDefault(p => p.DashStyle == style);
            if (ret == null)
            {
                ret = new D2D.StrokeStyle( d2DFactory, new StrokeStyleProperties(){ DashStyle = style});

                _StrokeStyles.Add(ret);
            }

            return ret;
        }

        public SharpDX.Direct2D1.Bitmap ResolveBitmap(string imagePath)
        {
            string id = $"Img:{imagePath}";
            if (!resCache.ContainsKey(id))
                resCache.Add(id, t => SharpDxHelper.LoadFromFile(t, imagePath) );

            return resCache[id] as SharpDX.Direct2D1.Bitmap;
        }

        public SharpDX.Direct2D1.Bitmap ResolveBitmap(string imageId, System.Drawing.Bitmap bm)
        {
            string id = $"Img:{imageId}";
            if (!resCache.ContainsKey(id))
                resCache.Add(id, t => SharpDxHelper.LoadBitmap(t, bm) );

            return resCache[id] as SharpDX.Direct2D1.Bitmap;
        }


        protected void      __AddDrawingInfo(DrawingInfo d)
        {// 后期渲染合并优化在这做，先不做优化
            List<DrawingInfo> l = null;
            if (!_DrawingInfos.TryGetValue(d.ZIndex, out l))
                _DrawingInfos.Add(d.ZIndex, new List<DrawingInfo>(){d});
            else
                l.Add(d);
        }

        public void         ClearDrawings()                                // 删除所有绘制元素
        {
            _DrawingInfos.Clear();
        }

        public virtual void OnRender()
        {

        }

        public event Action<ID2View> RenderEx;


        public              D2View()
        {
            Background = Colors.Black;
        }



        protected OrderedDictionary<int, List<DrawingInfo>> _DrawingInfos = new OrderedDictionary<int, List<DrawingInfo>>();
        protected Color     _Background;
        protected List<TextFormat> _TextFormats = new List<TextFormat>();

        public static ObjectPool<LineInfo> LineInfoPool = new ObjectPool<LineInfo>(200000);
        public static ObjectPool<RectangleInfo> RectPool = new ObjectPool<RectangleInfo>(200000);
        public static ObjectPool<TextInfo> TextPool = new ObjectPool<TextInfo>(5000);
        public static ObjectPool<ImageInfo> ImagePool = new ObjectPool<ImageInfo>(5000);
        public static ObjectPool<GeometryInfo> GeoPool = new ObjectPool<GeometryInfo>(5000);

    }

    public static class ELineStyleEx
    {
        public static D2D.DashStyle ToDashStyle(this ELineStyle style)
        {
            switch (style)
            {
                case ELineStyle.LineSolid:
                    return D2D.DashStyle.Solid;
                case ELineStyle.LineDash:
                    return D2D.DashStyle.Dash;
                case ELineStyle.LineDashdot:
                    return D2D.DashStyle.DashDot;
                default:
                    return D2D.DashStyle.Solid;
            }
        }
    }


    public class ObjectPool<T> where T:class
    {
        public void         Reset()
        {
            Index = 0;
        }

        public T            Get()
        {
            if (Index > _List.Count)
            {
                int expand = _List.Count / 2;
                for(int i = 0; i!= expand; ++i)
                    _List.Add(Activator.CreateInstance<T>());
            }

            return _List[Index++];
        }

        public              ObjectPool(int count)
        {
            for (int i = 0; i != count; ++i)
                _List.Add(Activator.CreateInstance<T>());
            Index = count;
        }

        protected int Index = 0;
        protected List<T> _List = new List<T>();
    }


}
