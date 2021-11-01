///********************************************************************
//    created:	2017/2/19 17:38:01
//    author:		rush
//    email:		
	
//    purpose:	这是个winform control， 暂时在wpf中用winform host使用
                
//                以后有时间将其改成完全的wpf控件

//*********************************************************************/
//using System;
//using System.Collections.Generic;
//using System.ComponentModel;
//using System.Diagnostics;
//using System.Diagnostics.CodeAnalysis;
//using System.Drawing;
//using System.Linq;
//using System.Linq.Expressions;
//using System.Reflection;
//using System.Text;
//using System.Windows;
//using System.Windows.Forms;
//using System.Windows.Media;
//using DataModel;
//using GalaSoft.MvvmLight;
//using OpenTK;
//using OpenTK.Graphics;
//using OpenTK.Graphics.OpenGL;
//using RLib.Base;
//using BeginMode = OpenTK.Graphics.BeginMode;
//using BlendingFactorDest = OpenTK.Graphics.BlendingFactorDest;
//using BlendingFactorSrc = OpenTK.Graphics.BlendingFactorSrc;
//using ClearBufferMask = OpenTK.Graphics.ClearBufferMask;
//using EnableCap = OpenTK.Graphics.EnableCap;
//using GL = OpenTK.Graphics.GL;
//using HintMode = OpenTK.Graphics.HintMode;
//using HintTarget = OpenTK.Graphics.HintTarget;
//using MatrixMode = OpenTK.Graphics.MatrixMode;
//using PixelFormat = OpenTK.Graphics.PixelFormat;
//using PixelInternalFormat = OpenTK.Graphics.PixelInternalFormat;
//using PixelType = OpenTK.Graphics.PixelType;
//using TextAlignment = OpenTK.Graphics.TextAlignment;
//using TextureMagFilter = OpenTK.Graphics.TextureMagFilter;
//using TextureMinFilter = OpenTK.Graphics.TextureMinFilter;
//using TextureParameterName = OpenTK.Graphics.TextureParameterName;
//using TextureTarget = OpenTK.Graphics.TextureTarget;

//using Color = System.Drawing.Color;
//using Size = System.Windows.Size;
//using FontFamily = System.Drawing.FontFamily;
//using Wintellect.PowerCollections;

//namespace RLib.UI
//{
//    public partial class View:GLControl, IView
//    {
//#region OberservableObject

//        /// <summary>
//        /// Occurs after a property value changes.
//        /// </summary>
//        public event PropertyChangedEventHandler PropertyChanged;

//        /// <summary>
//        /// Provides access to the PropertyChanged event handler to derived classes.
//        /// </summary>
//        protected PropertyChangedEventHandler PropertyChangedHandler
//        {
//            get
//            {
//                return PropertyChanged;
//            }
//        }

//#if !PORTABLE && !SL4
//        /// <summary>
//        /// Occurs before a property value changes.
//        /// </summary>
//        public event PropertyChangingEventHandler PropertyChanging;

//        /// <summary>
//        /// Provides access to the PropertyChanging event handler to derived classes.
//        /// </summary>
//        protected PropertyChangingEventHandler PropertyChangingHandler
//        {
//            get
//            {
//                return PropertyChanging;
//            }
//        }
//#endif

//        /// <summary>
//        /// Verifies that a property name exists in this ViewModel. This method
//        /// can be called before the property is used, for instance before
//        /// calling RaisePropertyChanged. It avoids errors when a property name
//        /// is changed but some places are missed.
//        /// </summary>
//        /// <remarks>This method is only active in DEBUG mode.</remarks>
//        /// <param name="propertyName">The name of the property that will be
//        /// checked.</param>
//        [Conditional("DEBUG")]
//        [DebuggerStepThrough]
//        public void VerifyPropertyName(string propertyName)
//        {
//            return;
//            var myType = GetType();

//#if NETFX_CORE
//            var info = myType.GetTypeInfo();

//            if (!string.IsNullOrEmpty(propertyName)
//                && info.GetDeclaredProperty(propertyName) == null)
//            {
//                // Check base types
//                var found = false;

//                while (info.BaseType != typeof(Object))
//                {
//                    info = info.BaseType.GetTypeInfo();

//                    if (info.GetDeclaredProperty(propertyName) != null)
//                    {
//                        found = true;
//                        break;
//                    }
//                }

//                if (!found)
//                {
//                    throw new ArgumentException("Property not found", propertyName);
//                }
//            }
//#else
//            if (!string.IsNullOrEmpty(propertyName)
//                && myType.GetProperty(propertyName) == null)
//            {
//#if !SILVERLIGHT
//                var descriptor = this as ICustomTypeDescriptor;

//                if (descriptor != null)
//                {
//                    if (descriptor.GetProperties()
//                        .Cast<PropertyDescriptor>()
//                        .Any(property => property.Name == propertyName))
//                    {
//                        return;
//                    }
//                }
//#endif

//                throw new ArgumentException("Property not found", propertyName);
//            }
//#endif
//        }

//#if !PORTABLE && !SL4
//#if CMNATTR
//        /// <summary>
//        /// Raises the PropertyChanging event if needed.
//        /// </summary>
//        /// <remarks>If the propertyName parameter
//        /// does not correspond to an existing property on the current class, an
//        /// exception is thrown in DEBUG configuration only.</remarks>
//        /// <param name="propertyName">(optional) The name of the property that
//        /// changed.</param>
//        [SuppressMessage(
//            "Microsoft.Design", 
//            "CA1030:UseEventsWhereAppropriate",
//            Justification = "This cannot be an event")]
//        public virtual void RaisePropertyChanging(
//            [CallerMemberName] string propertyName = null)
//#else
//        /// <summary>
//        /// Raises the PropertyChanging event if needed.
//        /// </summary>
//        /// <remarks>If the propertyName parameter
//        /// does not correspond to an existing property on the current class, an
//        /// exception is thrown in DEBUG configuration only.</remarks>
//        /// <param name="propertyName">The name of the property that
//        /// changed.</param>
//        [SuppressMessage(
//            "Microsoft.Design", 
//            "CA1030:UseEventsWhereAppropriate",
//            Justification = "This cannot be an event")]
//        public virtual void RaisePropertyChanging(
//            string propertyName)
//#endif
//        {
//            VerifyPropertyName(propertyName);

//            var handler = PropertyChanging;
//            if (handler != null)
//            {
//                handler(this, new PropertyChangingEventArgs(propertyName));
//            }
//        }
//#endif

//#if CMNATTR
//        /// <summary>
//        /// Raises the PropertyChanged event if needed.
//        /// </summary>
//        /// <remarks>If the propertyName parameter
//        /// does not correspond to an existing property on the current class, an
//        /// exception is thrown in DEBUG configuration only.</remarks>
//        /// <param name="propertyName">(optional) The name of the property that
//        /// changed.</param>
//        [SuppressMessage(
//            "Microsoft.Design", 
//            "CA1030:UseEventsWhereAppropriate",
//            Justification = "This cannot be an event")]
//        public virtual void RaisePropertyChanged(
//            [CallerMemberName] string propertyName = null)
//#else
//        /// <summary>
//        /// Raises the PropertyChanged event if needed.
//        /// </summary>
//        /// <remarks>If the propertyName parameter
//        /// does not correspond to an existing property on the current class, an
//        /// exception is thrown in DEBUG configuration only.</remarks>
//        /// <param name="propertyName">The name of the property that
//        /// changed.</param>
//        [SuppressMessage(
//            "Microsoft.Design", 
//            "CA1030:UseEventsWhereAppropriate",
//            Justification = "This cannot be an event")]
//        public virtual void RaisePropertyChanged(
//            string propertyName) 
//#endif
//        {
//            VerifyPropertyName(propertyName);

//            var handler = PropertyChanged;
//            if (handler != null)
//            {
//                handler(this, new PropertyChangedEventArgs(propertyName));
//            }
//        }

//#if !PORTABLE && !SL4
//        /// <summary>
//        /// Raises the PropertyChanging event if needed.
//        /// </summary>
//        /// <typeparam name="T">The type of the property that
//        /// changes.</typeparam>
//        /// <param name="propertyExpression">An expression identifying the property
//        /// that changes.</param>
//        [SuppressMessage(
//            "Microsoft.Design", 
//            "CA1030:UseEventsWhereAppropriate",
//            Justification = "This cannot be an event")]
//        [SuppressMessage(
//            "Microsoft.Design",
//            "CA1006:GenericMethodsShouldProvideTypeParameter",
//            Justification = "This syntax is more convenient than other alternatives.")]
//        public virtual void RaisePropertyChanging<T>(Expression<Func<T>> propertyExpression)
//        {
//            var handler = PropertyChanging;
//            if (handler != null)
//            {
//                var propertyName = GetPropertyName(propertyExpression);
//                handler(this, new PropertyChangingEventArgs(propertyName));
//            }
//        }
//#endif

//        /// <summary>
//        /// Raises the PropertyChanged event if needed.
//        /// </summary>
//        /// <typeparam name="T">The type of the property that
//        /// changed.</typeparam>
//        /// <param name="propertyExpression">An expression identifying the property
//        /// that changed.</param>
//        [SuppressMessage(
//            "Microsoft.Design", 
//            "CA1030:UseEventsWhereAppropriate",
//            Justification = "This cannot be an event")]
//        [SuppressMessage(
//            "Microsoft.Design",
//            "CA1006:GenericMethodsShouldProvideTypeParameter",
//            Justification = "This syntax is more convenient than other alternatives.")]
//        public virtual void RaisePropertyChanged<T>(Expression<Func<T>> propertyExpression)
//        {
//            var handler = PropertyChanged;

//            if (handler != null)
//            {
//                var propertyName = GetPropertyName(propertyExpression);

//                if (!string.IsNullOrEmpty(propertyName))
//                {
//                    // ReSharper disable once ExplicitCallerInfoArgument
//                    RaisePropertyChanged(propertyName);
//                }
//            }
//        }

//        /// <summary>
//        /// Extracts the name of a property from an expression.
//        /// </summary>
//        /// <typeparam name="T">The type of the property.</typeparam>
//        /// <param name="propertyExpression">An expression returning the property's name.</param>
//        /// <returns>The name of the property returned by the expression.</returns>
//        /// <exception cref="ArgumentNullException">If the expression is null.</exception>
//        /// <exception cref="ArgumentException">If the expression does not represent a property.</exception>
//        [SuppressMessage(
//            "Microsoft.Design", 
//            "CA1011:ConsiderPassingBaseTypesAsParameters",
//            Justification = "This syntax is more convenient than the alternatives."), 
//         SuppressMessage(
//            "Microsoft.Design",
//            "CA1006:DoNotNestGenericTypesInMemberSignatures",
//            Justification = "This syntax is more convenient than the alternatives.")]
//        protected static string GetPropertyName<T>(Expression<Func<T>> propertyExpression)
//        {
//            if (propertyExpression == null)
//            {
//                throw new ArgumentNullException("propertyExpression");
//            }

//            var body = propertyExpression.Body as MemberExpression;

//            if (body == null)
//            {
//                throw new ArgumentException("Invalid argument", "propertyExpression");
//            }

//            var property = body.Member as PropertyInfo;

//            if (property == null)
//            {
//                throw new ArgumentException("Argument is not a property", "propertyExpression");
//            }

//            return property.Name;
//        }

//        /// <summary>
//        /// Assigns a new value to the property. Then, raises the
//        /// PropertyChanged event if needed. 
//        /// </summary>
//        /// <typeparam name="T">The type of the property that
//        /// changed.</typeparam>
//        /// <param name="propertyExpression">An expression identifying the property
//        /// that changed.</param>
//        /// <param name="field">The field storing the property's value.</param>
//        /// <param name="newValue">The property's value after the change
//        /// occurred.</param>
//        /// <returns>True if the PropertyChanged event has been raised,
//        /// false otherwise. The event is not raised if the old
//        /// value is equal to the new value.</returns>
//        [SuppressMessage(
//            "Microsoft.Design",
//            "CA1006:DoNotNestGenericTypesInMemberSignatures",
//            Justification = "This syntax is more convenient than the alternatives."), 
//         SuppressMessage(
//            "Microsoft.Design", 
//            "CA1045:DoNotPassTypesByReference",
//            MessageId = "1#",
//            Justification = "This syntax is more convenient than the alternatives.")]
//        protected bool Set<T>(
//            Expression<Func<T>> propertyExpression,
//            ref T field,
//            T newValue)
//        {
//            if (EqualityComparer<T>.Default.Equals(field, newValue))
//            {
//                return false;
//            }

//#if !PORTABLE && !SL4
//            RaisePropertyChanging(propertyExpression);
//#endif
//            field = newValue;
//            RaisePropertyChanged(propertyExpression);
//            return true;
//        }

//        /// <summary>
//        /// Assigns a new value to the property. Then, raises the
//        /// PropertyChanged event if needed. 
//        /// </summary>
//        /// <typeparam name="T">The type of the property that
//        /// changed.</typeparam>
//        /// <param name="propertyName">The name of the property that
//        /// changed.</param>
//        /// <param name="field">The field storing the property's value.</param>
//        /// <param name="newValue">The property's value after the change
//        /// occurred.</param>
//        /// <returns>True if the PropertyChanged event has been raised,
//        /// false otherwise. The event is not raised if the old
//        /// value is equal to the new value.</returns>
//        [SuppressMessage(
//            "Microsoft.Design", 
//            "CA1045:DoNotPassTypesByReference",
//            MessageId = "1#",
//            Justification = "This syntax is more convenient than the alternatives.")]
//        protected bool Set<T>(
//            string propertyName,
//            ref T field,
//            T newValue)
//        {
//            if (EqualityComparer<T>.Default.Equals(field, newValue))
//            {
//                return false;
//            }

//#if !PORTABLE && !SL4
//            RaisePropertyChanging(propertyName);
//#endif
//            T old = field;      // mod:2018/6/21

//            field = newValue;

//            // ReSharper disable ExplicitCallerInfoArgument
//            RaisePropertyChanged(propertyName, newValue, old);
//            // ReSharper restore ExplicitCallerInfoArgument
            
//            return true;
//        }

//        // 带old mod:2018/6/21
//        public virtual void RaisePropertyChanged(string propertyName, object newval, object oldval) 
//        {
//            VerifyPropertyName(propertyName);

//            var handler = PropertyChanged;
//            if (handler != null)
//            {
//                handler(this, new PropertyChangedEventArgsEx(propertyName, newval, oldval));
//            }
//        }

//#if CMNATTR
//        /// <summary>
//        /// Assigns a new value to the property. Then, raises the
//        /// PropertyChanged event if needed. 
//        /// </summary>
//        /// <typeparam name="T">The type of the property that
//        /// changed.</typeparam>
//        /// <param name="field">The field storing the property's value.</param>
//        /// <param name="newValue">The property's value after the change
//        /// occurred.</param>
//        /// <param name="propertyName">(optional) The name of the property that
//        /// changed.</param>
//        /// <returns>True if the PropertyChanged event has been raised,
//        /// false otherwise. The event is not raised if the old
//        /// value is equal to the new value.</returns>
//        protected bool Set<T>(
//            ref T field,
//            T newValue,
//            [CallerMemberName] string propertyName = null)
//        {
//            return Set(propertyName, ref field, newValue);
//        }
//#endif


//        #endregion

//#region Overrides
//        public new int      Width
//        {
//            get { return base.Width; }
//            set
//            {
//                if (base.Width != value)
//                {
//                    base.Width = value;

//                    RaisePropertyChanged("SizeProperty");
//                }
//            }
//        }
//        public new int      Height
//        {
//            get { return base.Height; }
//            set
//            {
//                if (base.Height != value)
//                {
//                    base.Height = value;

//                    RaisePropertyChanged("SizeProperty");
//                }
//            }
            
//        }

//        public override Color BackColor
//        {
//            get
//            {
//                return base.BackColor;
//            }
//            set
//            {
//                if (base.BackColor != value)
//                {
//                    base.BackColor = value;

//                    RaisePropertyChanged("BackColorProperty");
//                }
//            }
//        }
//        public new System.Drawing.Size Size
//        {
//            get
//            {
//                return base.Size;
//            }
//            set
//            {
//                if (base.Size != value)
//                {
//                    base.Size = value;

//                    RaisePropertyChanged("SizeProperty");
//                }
//            }
//        }
        

//#endregion

//#region IView
//        public RSize        SizeProperty
//        {
//            get { return new RSize(base.Size.Width, base.Size.Height); }
//            set { Size = new System.Drawing.Size((int)value.Width, (int)value.Height);}
//        }
//        public RColor       BackColorProperty
//        {
//            get
//            {
//                return new RColor(BackColor.A, BackColor.R, BackColor.G, BackColor.B);
//            }
//            set
//            {
//                BackColor = Color.FromArgb(value.A, value.R, value.G, value.B);
//            }
//        }

//        public bool         IsRebuildEveryFrame { get; set; }               // 是否每帧重构绘图信息
//#endregion



//        public int          Layers { get { return m_nLayers; }}
////        public virtual RColor BackColor { get { return m_cBGColor; } set { throw  new NotImplementedException();} }                        // 背景色

//        public void         DrawLine(RVector2 start, RVector2 end, RColor c, int thickness = 1, ELineStyle style = ELineStyle.LINE_SOLID, int layer = 0)
//        {
//            LineInfo li = new LineInfo();
//            li.Color = c;
//            li.Nodes = new List<RVector2>(){start, end};
//            li.Thinkness = thickness;
//            li.LineStyle = style;
//            li.Layer = layer;


//            __AddDrawingInfo(li);
//        }
//        public void         DrawLine(List<RVector2> vs, RColor c, int thickness = 1, ELineStyle style = ELineStyle.LINE_SOLID, int layer = 0)
//        {
//            System.Diagnostics.Debug.Assert(vs != null && vs.Count != 0);

//            LineInfo li = new LineInfo();
//            li.Color = c;
//            li.Nodes = vs;
//            li.Thinkness = thickness;
//            li.LineStyle = style;
//            li.Layer = layer;

//            __AddDrawingInfo(li);
//        }
//        public void         DrawTriangle(RVector2 a, RVector2 b, RVector2 c, RColor clr, int layer = 0)
//        {
//            TriangleInfo qi = new TriangleInfo();
//            qi.Color = clr;
//            qi.Nodes = new List<RVector2>(){a, b, c};
//            qi.Layer = layer;

//            __AddDrawingInfo(qi);
//        }
//        public void         DrawQuad(RVector2 a, RVector2 b, RVector2 c, RVector2 d, RColor clr, int layer = 0)
//        {
//            QuadInfo qi = new QuadInfo();
//            qi.Color = clr;
//            qi.Nodes = new List<RVector2>(){a, b, c, d};
//            qi.Layer = layer;

//            __AddDrawingInfo(qi);
//        }
//        public void         DrawQuad(RVector2 ld, RVector2 ur, RColor clr, int layer = 0)
//        {
//            QuadInfo qi = new QuadInfo();
//            qi.Color = clr;
//            qi.Nodes = new List<RVector2>(){ld, new RVector2(ld.X, ur.Y), ur, new RVector2(ur.X, ld.Y)};
//            qi.Layer = layer;

//            __AddDrawingInfo(qi);
//        }

//        public TextInfo     DrawText(RVector2 point, string text, RColor color, Font font = null, TextAlignment alignment = TextAlignment.Near, int layer = 0)
//        {
//            TextInfo ti = new TextInfo();
//            ti.Color = color;
//            ti.Text = text;
//            ti.Font = font;
//            if (font == null)
//                ti.Font = Default;
//            ti.Alignment = alignment;
//            ti.Layer = layer;
//            ti.Point = point;
//            ti.Layer = layer;

//            ti.Extents = m_pTextPrinter.Measure(text, ti.Font, new RectangleF(0, 0, 1000, 100), TextPrinterOptions.Default, alignment);

//            //ti.Rect = new RectangleF((float)point.X, (float)(Height - point.Y - te.BoundingBox.Height), te.BoundingBox.Width+5, te.BoundingBox.Height);

//            __AddDrawingInfo(ti);

//            return ti;
//        }

//        protected void      __AddDrawingInfo(DrawingInfo d)
//        {// 后期渲染合并优化在这做，先不做优化
            
//            if (m_dDrawingInfos.ContainsKey(d.Layer))
//                m_dDrawingInfos[d.Layer].Add(d);
//            else
//                m_dDrawingInfos.Add(d.Layer, new List<DrawingInfo>(){d});
//        }

//        public void         ClearDrawing()
//        {
//            m_nLayers = 0;
//            m_dDrawingInfos.Clear();
//        }

//		protected override void OnLoad(EventArgs e) 
//        {
//			base.OnLoad(e);
//			m_bLoaded = true;
//			__SetupViewport();
//		}

//		private void __SetupViewport() 
//        {
//			var w = Width;
//			var h = Height;

//			GL.ClearColor(Color.FromArgb(BackColor.ToArgb()));

//			GL.MatrixMode(MatrixMode.Projection);
//			GL.LoadIdentity();
            
//            //GL.Ortho(-1.0, 1.0, -1.0, 1.0, 0.0, 4.0);
//            GL.Ortho(0, w, 0, h, -1, 1); // Bottom-left corner pixel has coordinate (0, 0)
//            GL.Viewport(0, 0, w, h); // Use all of the glControl painting area
//		}

//		protected override void OnResize(EventArgs e) 
//        {
//            base.OnResize(e);

//            if (!this.m_bLoaded)
//            {
//                return;
//            }
//            this.__SetupViewport();
//            this.Invalidate();

////            Logger.Log("{" + Width + "," + Height +"}");
//		}

//        protected override void OnPaint(PaintEventArgs e)
//        {
//            base.OnPaint(e);

//            if (!m_bLoaded)
//            {
//                return;
//            }

//            __Render();
//        }

//        protected void      __Render()
//        {
//            MakeCurrent();

//			GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

//			GL.MatrixMode(MatrixMode.Projection);
//			GL.LoadIdentity();
//            GL.Ortho(0, Width, 0, Height, -1, 1); // Bottom-left corner pixel has coordinate (0, 0)
//            GL.Viewport(0, 0, Width, Height); // Use all of the glControl painting area

//			GL.MatrixMode(MatrixMode.Modelview);
//			GL.LoadIdentity();

//            if(IsRebuildEveryFrame)
//                ClearDrawing();

//            // user render
//            _OnRender();


//          //  GL.Enable(EnableCap.Blend);
////            GL.Enable(EnableCap.LineSmooth);      // 不能使用smooth，不然会变成2个像素
////            GL.Enable(EnableCap.PointSmooth);
////            GL.Hint(HintTarget.LineSmoothHint, HintMode.Nicest);
//          //  GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);


//            foreach (KeyValuePair<int, List<DrawingInfo>> kv in m_dDrawingInfos)
//            {
//                foreach (DrawingInfo d in kv.Value)
//                {
//                    switch (d.Type)
//                    {
//                        case EDrawing.Line:
//                            GL.LineWidth((d as LineInfo).Thinkness);
//                            if ((d as LineInfo).LineStyle == ELineStyle.LINE_DASH)
//                            {
//                                GL.Enable(EnableCap.LineStipple);
//                                //                            GL.Enable(EnableCap.LineSmooth);
//                                GL.LineStipple(1, 0X0707);
//                            }

//                            GL.Begin(BeginMode.Lines);
//                            GL.Color3(Color.FromArgb(d.Color.ToArgb()));
//                            foreach (var n in (d as LineInfo).Nodes)
//                                GL.Vertex2(n.X, n.Y);
//                            GL.End();

//                            if ((d as LineInfo).LineStyle == ELineStyle.LINE_DASH)
//                            {
//                                GL.Disable(EnableCap.LineStipple);
//                            }
//                            break;

//                        case EDrawing.Triangle:
//                            GL.LineWidth((d as LineInfo).Thinkness);

//                            GL.Begin(BeginMode.Triangles);
//                            GL.Color3(Color.FromArgb(d.Color.ToArgb()));
//                            foreach (var n in (d as TriangleInfo).Nodes)
//                                GL.Vertex2(n.X, n.Y);
//                            GL.End();
//                            break;

//                        case EDrawing.Quad:
//                            GL.LineWidth((d as LineInfo).Thinkness);

//                            GL.Begin(BeginMode.Quads);
//                            GL.Color3(Color.FromArgb(d.Color.ToArgb()));
//                            foreach (var n in (d as QuadInfo).Nodes)
//                                GL.Vertex2(n.X, n.Y);
//                            GL.End();
//                            break;

//                        case EDrawing.Text:
//                        {
//                            GL.MatrixMode(MatrixMode.Projection);
//                            GL.LoadIdentity();
//                            GL.Ortho(0, Width, Height, 0, -1, 1);
//                            GL.Viewport(0, 0, Width, Height); // Use all of the glControl painting area

//                            GL.MatrixMode(MatrixMode.Modelview);
//                            GL.LoadIdentity();

//                            TextInfo ti = d as TextInfo;

//                            ti.Rect = new RectangleF((float)ti.Point.X, (float)(Height - ti.Point.Y - ti.Extents.BoundingBox.Height), ti.Extents.BoundingBox.Width+5, ti.Extents.BoundingBox.Height);


//                            m_pTextPrinter.Print(ti.Text, ti.Font, Color.FromArgb(ti.Color.ToArgb()), ti.Rect, TextPrinterOptions.Default, ti.Alignment);


//                            GL.MatrixMode(MatrixMode.Projection);
//                            GL.LoadIdentity();
//                            GL.Ortho(0, Width, 0, Height, -1, 1); // Bottom-left corner pixel has coordinate (0, 0)
//                            GL.Viewport(0, 0, Width, Height); // Use all of the glControl painting area

//                            GL.MatrixMode(MatrixMode.Modelview);
//                            GL.LoadIdentity();

//                        }
//                            break;
//                    }

//                }
//            }


//            ///// render Text
//            //GL.Enable(EnableCap.Texture2D);
//            //GL.BindTexture(TextureTarget.Texture2D, m_pTextRenderer.Texture);
//            //GL.Begin(BeginMode.Quads);

//            ////GL.TexCoord2(0.0f, 1.0f); GL.Vertex2(0, Height);
//            ////GL.TexCoord2(1.0f, 1.0f); GL.Vertex2(Width, Height);
//            ////GL.TexCoord2(1.0f, 0.0f); GL.Vertex2(Width, 0);
//            ////GL.TexCoord2(0.0f, 0.0f); GL.Vertex2(0, 0);

//            //GL.TexCoord2(0.0f, 1.0f); GL.Vertex2(0, 0);
//            //GL.TexCoord2(1.0f, 1.0f); GL.Vertex2(Width, 0);
//            //GL.TexCoord2(1.0f, 0.0f); GL.Vertex2(Width, Height);
//            //GL.TexCoord2(0.0f, 0.0f); GL.Vertex2(0, Height);

//            //GL.End();


//			this.SwapBuffers();
//        }


//        public virtual void      _OnRender()
//        {
////            DrawLine(new RVector2(0, 0), new RVector2(Width, Height), RColor.Yellow);
			
//            //if (this.Focused) {
//            //    GL.Color3(RColor.Yellow);
//            //}
//            //else {
//            //    GL.Color3(RColor.Blue);
//            //}
//            //GL.Begin(BeginMode.Triangles);
//            //GL.Vertex2(0, 0);
//            //GL.Vertex2(Width, Height);
//            //GL.Vertex2(Width, 0);
//            //GL.End();

//		}

//        //protected override void OnKeyDown(KeyEventArgs e) {
//        //    base.OnKeyDown(e);
//        //    if (e.KeyCode == Keys.Space) {
//        //        this._x++;
//        //        this.Invalidate();
//        //    }
//        //}

//        public View()
//        {
//            IsRebuildEveryFrame = false;

//        }


//#region Members
//		protected bool m_bLoaded;

//        protected RColor     m_cBGColor;
//        protected Size      m_vSize;

//        protected OrderedDictionary<int, List<DrawingInfo>> m_dDrawingInfos = new OrderedDictionary<int, List<DrawingInfo>>(); 
//        protected int       m_nLayers;



//        public static Font      m_pSerif = new Font(FontFamily.GenericSerif, 9);
//        public static Font      m_pSans = new Font(FontFamily.GenericSansSerif, 9);
//        public static Font      m_pMono = new Font(FontFamily.GenericMonospace, 12);

//		OpenTK.Graphics.TextPrinter m_pTextPrinter = new OpenTK.Graphics.TextPrinter();

//        OpenTK.Graphics.
//#endregion
//    }
//}
