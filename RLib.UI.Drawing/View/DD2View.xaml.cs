//using System;
//using System.Collections.Generic;
//using System.Globalization;
//using System.Linq;
//using System.Runtime.InteropServices;
//using System.Text;
//using System.Threading.Tasks;
//using System.Windows;
//using System.Windows.Controls;
//using System.Windows.Data;
//using System.Windows.Documents;
//using System.Windows.Input;
//using System.Windows.Interop;
//using System.Windows.Media;
//using System.Windows.Media.Imaging;
//using System.Windows.Navigation;
//using System.Windows.Shapes;
//using DataModel;
//using RLib.Base;
//using SharpDX;
//using D2D = SharpDX.Direct2D1;
//using SharpDX.Direct3D;
//using SharpDX.DirectWrite;
//using SharpDX.Mathematics.Interop;
//using Wintellect.PowerCollections;
//using DXGI = SharpDX.DXGI;
//using D3D11 = SharpDX.Direct3D11;
//using D3D9 = SharpDX.Direct3D9;
//using Point = System.Windows.Point;
//using Color = System.Windows.Media.Color;

//namespace RLib.UI.Drawing
//{
//    /// <summary>
//    /// View.xaml 的交互逻辑
//    /// </summary>
//    public partial class D2DView : D2dControl,ID2View 
//    {
//        public override void Render(D2D.RenderTarget target)
//        {

//        }


//        public void         DrawLine(Point start, Point end, Color strokeColor, int strokeWidth = 1, ELineStyle strokeStyle = ELineStyle.LINE_SOLID, int zindex = 0)
//        {
//            DrawLine(new List<Point>(){start, end}, strokeColor, strokeWidth, strokeStyle, zindex);
//        }

//        public void         DrawLine(List<Point> vs, Color strokeColor, int strokeWidth = 1, ELineStyle strokeStyle = ELineStyle.LINE_SOLID, int zindex = 0)
//        {
//            LineInfo li = new LineInfo();
//            li.StrokeColor = strokeColor; 
//            li.StrokWidth = strokeWidth;
//            li.StrokeStyle = strokeStyle;
//            li.ZIndex = zindex;

//            li.Nodes = vs;

//            __AddDrawingInfo(li);
//        }


//        public void DrawRectangle(Rect rect, Color strokeColor, int strokeWidth = 1,
//            ELineStyle strokeStyle = ELineStyle.LINE_SOLID, Color? fillColor = null, int zindex = 0)
//        {
//            RectangleInfo info = new RectangleInfo();
//            info.StrokeColor = strokeColor; 
//            info.StrokWidth = strokeWidth;
//            info.StrokeStyle = strokeStyle;
//            info.ZIndex = zindex;

//            info.Nodes = new List<Point>(){rect.TopLeft, rect.TopRight};

//            __AddDrawingInfo(info);
//        }

//        //void                DrawTriangle(Point a, Point b, Point c, Color clr, int zindex = 0);

//        //void                DrawQuad(Point a, Point b, Point c, Point d, Color clr, int zindex = 0);
//        //void                DrawQuad(Point ld, Point ur, Color clr, int zindex = 0);

//        public TextInfo            DrawText(string text,  TextFormat textFormat, Rect rect, Color foregroundColor, int zindex = 0)
//        {
//            TextInfo ti = new TextInfo();
//            ti.ForegroundColor = foregroundColor;
//            ti.Text = text;
//            ti.Format = textFormat;
//            ti.Rect = rect;
//            ti.ZIndex = zindex;

//            //ti.Extents = m_pTextPrinter.Measure(text, ti.Font, new RectangleF(0, 0, 1000, 100), TextPrinterOptions.Default, alignment);

//            //ti.Rect = new RectangleF((float)point.X, (float)(Height - point.Y - te.BoundingBox.Height), te.BoundingBox.Width+5, te.BoundingBox.Height);

//            __AddDrawingInfo(ti);

//            return ti;
//        }

//        protected void      __AddDrawingInfo(DrawingInfo d)
//        {// 后期渲染合并优化在这做，先不做优化
            
//            if (_DrawingInfos.ContainsKey(d.ZIndex))
//                _DrawingInfos[d.ZIndex].Add(d);
//            else
//                _DrawingInfos.Add(d.ZIndex, new List<DrawingInfo>(){d});
//        }

//        public void         ClearDrawings()                                // 删除所有绘制元素
//        {
//            _DrawingInfos.Clear();
//        }



//        public View()
//        {
//            InitializeComponent();

//            _D3D11Device = new D3D11.Device(DriverType.Hardware, D3D11.DeviceCreationFlags.BgraSupport);
//            var d3DContext = new D3D9.Direct3DEx();

//            var presentParams = GetPresentParameters();
//            var createFlags = D3D9.CreateFlags.HardwareVertexProcessing | D3D9.CreateFlags.Multithreaded | D3D9.CreateFlags.FpuPreserve;
//            _D3D9DeviceEx = new D3D9.DeviceEx(d3DContext, 0, D3D9.DeviceType.Hardware, IntPtr.Zero, createFlags, presentParams);

//            FactoryDWrite = new SharpDX.DirectWrite.Factory();

//            Loaded += _Loaded;
//        }


//        private void _Loaded(object sender, RoutedEventArgs e)
//        {
//            CreateAndBindTargets();

//            SizeChanged += (s, e2) => { CreateAndBindTargets(); };

//            SizeChanged += CompositionTarget_Rendering;
//        }

//        private void CompositionTarget_Rendering(object sender, EventArgs e)
//        {
//            _d2DRenderTarget.BeginDraw();

//            OnRender(_d2DRenderTarget);

//            _d2DRenderTarget.EndDraw();

//            _D3DImage.Lock();

//            _D3DImage.AddDirtyRect(new Int32Rect(0, 0, _D3DImage.PixelWidth, _D3DImage.PixelHeight));

//            _D3DImage.Unlock();
//        }

//        private void OnRender(D2D.RenderTarget renderTarget)
//        {
//            // 如果是solidColorBrush，用来clear
//            // todo:如果不是的情况，后面也要处理，就需要一个clear color
//            if(Background is SolidColorBrush)
//                renderTarget.Clear((Background as SolidColorBrush).ToRawColor4());


//            // 以后使用d3d9进行合并批次之类的操作
//            foreach (KeyValuePair<int, List<DrawingInfo>> kv in _DrawingInfos)
//            {
//                foreach (DrawingInfo d in kv.Value)
//                {
//                    switch (d.Type)
//                    {
//                        case EDrawing.Line:
//                            //todo: style 后面做
//                            LineInfo li = d as LineInfo;
//                            SharpDX.Direct2D1.SolidColorBrush strokeColor = new D2D.SolidColorBrush(renderTarget, li.StrokeColor.ToRawColor4());
//                            for (int i = 0; i <= li.Nodes.Count -2; i++)
//                            {
//                                renderTarget.DrawLine(li.Nodes[i].ToRawVector2(), li.Nodes[i+1].ToRawVector2(), strokeColor, li.StrokWidth);
//                            }

//                            break;

//                        //case EDrawing.Triangle:
//                        //    GL.LineWidth((d as LineInfo).Thinkness);

//                        //    GL.Begin(BeginMode.Triangles);
//                        //    GL.Color3(Color.FromArgb(d.Color.ToArgb()));
//                        //    foreach (var n in (d as TriangleInfo).Nodes)
//                        //        GL.Vertex2(n.X, n.Y);
//                        //    GL.End();
//                        //    break;

//                        //case EDrawing.Quad:
//                        //    GL.LineWidth((d as LineInfo).Thinkness);

//                        //    GL.Begin(BeginMode.Quads);
//                        //    GL.Color3(Color.FromArgb(d.Color.ToArgb()));
//                        //    foreach (var n in (d as QuadInfo).Nodes)
//                        //        GL.Vertex2(n.X, n.Y);
//                        //    GL.End();
//                        //    break;

//                        //case EDrawing.Text:
//                        //{
//                        //    GL.MatrixMode(MatrixMode.Projection);
//                        //    GL.LoadIdentity();
//                        //    GL.Ortho(0, Width, Height, 0, -1, 1);
//                        //    GL.Viewport(0, 0, Width, Height); // Use all of the glControl painting area

//                        //    GL.MatrixMode(MatrixMode.Modelview);
//                        //    GL.LoadIdentity();

//                        //    TextInfo ti = d as TextInfo;

//                        //    ti.Rect = new RectangleF((float)ti.Point.X, (float)(Height - ti.Point.Y - ti.Extents.BoundingBox.Height), ti.Extents.BoundingBox.Width+5, ti.Extents.BoundingBox.Height);


//                        //    m_pTextPrinter.Print(ti.Text, ti.Font, Color.FromArgb(ti.Color.ToArgb()), ti.Rect, TextPrinterOptions.Default, ti.Alignment);


//                        //    GL.MatrixMode(MatrixMode.Projection);
//                        //    GL.LoadIdentity();
//                        //    GL.Ortho(0, Width, 0, Height, -1, 1); // Bottom-left corner pixel has coordinate (0, 0)
//                        //    GL.Viewport(0, 0, Width, Height); // Use all of the glControl painting area

//                        //    GL.MatrixMode(MatrixMode.Modelview);
//                        //    GL.LoadIdentity();

//                        //}
//                        //    break;
//                    }

//                }
//            }




//            //for (int i = 0; i != 100; ++i)
//            //{
//            //    for (int j = 0; j != 100; ++j)
//            //    {

//            //        renderTarget.DrawLine(new RawVector2(0, 0), new RawVector2((float)Width, j), brush);
//            //    }

//            //}


//            //SharpDX.Direct2D1.SolidColorBrush SceneColorBrush = new SharpDX.Direct2D1.SolidColorBrush(renderTarget, SharpDX.Color.Black);
//            //TextFormat tf = new TextFormat(FactoryDWrite, "宋体", 32) {TextAlignment = SharpDX.DirectWrite.TextAlignment.Leading, ParagraphAlignment = ParagraphAlignment.Near};
//            //TextLayout tl = new TextLayout(FactoryDWrite, "你好，世界", tf, _D3DImage.PixelWidth, _D3DImage.PixelHeight);
//            //renderTarget.DrawTextLayout(new SharpDX.Vector2(0,0), tl, SceneColorBrush, D2D.DrawTextOptions.None);


//            //renderTarget.DrawRectangle(new RawRectangleF(_x, _y, _x + 10, _y + 10), brush);
//        }


//        private void        CreateAndBindTargets()
//        {
//            if(_D3D9Texture != null)
//                _D3D9Texture.Dispose();

//            if(_d2DRenderTarget != null)
//                _d2DRenderTarget.Dispose();

//            if(_D3D11Texture2D != null)
//                _D3D11Texture2D.Dispose();



//            var width = Math.Max((int) ActualWidth, 100);
//            var height = Math.Max((int) ActualHeight, 100);

//            var renderDesc = new D3D11.Texture2DDescription
//            {
//                BindFlags = D3D11.BindFlags.RenderTarget | D3D11.BindFlags.ShaderResource,
//                Format = DXGI.Format.B8G8R8A8_UNorm,
//                Width = width,
//                Height = height,
//                MipLevels = 1,
//                SampleDescription = new DXGI.SampleDescription(1, 0),
//                Usage = D3D11.ResourceUsage.Default,
//                OptionFlags = D3D11.ResourceOptionFlags.Shared,
//                CpuAccessFlags = D3D11.CpuAccessFlags.None,
//                ArraySize = 1
//            };


//            _D3D11Texture2D = new D3D11.Texture2D(_D3D11Device, renderDesc);

//            var surface = _D3D11Texture2D.QueryInterface<DXGI.Surface>();

//            var renderTargetProperties = new D2D.RenderTargetProperties(new D2D.PixelFormat(DXGI.Format.Unknown, D2D.AlphaMode.Premultiplied));

//            _d2DRenderTarget = new D2D.RenderTarget(_d2DFactory, surface, renderTargetProperties);

//            _SetRenderTarget(_D3D11Texture2D);

//            _D3D11Device.ImmediateContext.Rasterizer.SetViewport(0, 0, (int) ActualWidth, (int) ActualHeight);

//            CompositionTarget.Rendering += CompositionTarget_Rendering;
//            //SizeChanged += CompositionTarget_Rendering;
//        }

//        private void        _SetRenderTarget(D3D11.Texture2D d3d11Tex)
//        {
//            var format = TranslateFormat(d3d11Tex);
//            var handle = _GetSharedHandle(d3d11Tex);

//            _D3D9Texture = new D3D9.Texture(_D3D9DeviceEx, d3d11Tex.Description.Width, d3d11Tex.Description.Height, 1,
//                D3D9.Usage.RenderTarget, format, D3D9.Pool.Default, ref handle);

//            using (var surface = _D3D9Texture.GetSurfaceLevel(0))
//            {
//                _D3DImage.Lock();
//                _D3DImage.SetBackBuffer(D3DResourceType.IDirect3DSurface9, surface.NativePointer);
//                _D3DImage.Unlock();
//            }
//        }

//        private static D3D9.PresentParameters GetPresentParameters()
//        {
//            var presentParams = new D3D9.PresentParameters();

//            presentParams.Windowed = true;
//            presentParams.SwapEffect = D3D9.SwapEffect.Discard;
//            presentParams.DeviceWindowHandle =NativeMethods.GetDesktopWindow();
//            presentParams.PresentationInterval = D3D9.PresentInterval.Default;

//            return presentParams;
//        }
//        private IntPtr      _GetSharedHandle(D3D11.Texture2D texture)
//        {
//            using (var resource = texture.QueryInterface<DXGI.Resource>())
//            {
//                return resource.SharedHandle;
//            }
//        }
//        private static D3D9.Format TranslateFormat(D3D11.Texture2D texture)
//        {
//            switch (texture.Description.Format)
//            {
//                case SharpDX.DXGI.Format.R10G10B10A2_UNorm:
//                    return D3D9.Format.A2B10G10R10;
//                case SharpDX.DXGI.Format.R16G16B16A16_Float:
//                    return D3D9.Format.A16B16G16R16F;
//                case SharpDX.DXGI.Format.B8G8R8A8_UNorm:
//                    return D3D9.Format.A8R8G8B8;
//                default:
//                    return D3D9.Format.Unknown;
//            }
//        }


//        protected D3D11.Device _D3D11Device;
//        protected D2D.Factory _d2DFactory = new D2D.Factory();
//        protected D3D9.DeviceEx _D3D9DeviceEx;
//        protected D3D9.Texture _D3D9Texture;
//        protected D3D11.Texture2D _D3D11Texture2D;
//        protected D2D.RenderTarget _d2DRenderTarget;

//        public SharpDX.DirectWrite.Factory FactoryDWrite { get; private set; }

//        protected OrderedDictionary<int, List<DrawingInfo>> _DrawingInfos = new OrderedDictionary<int, List<DrawingInfo>>(); 
//    }
  
    
//}
