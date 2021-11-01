using SharpDX;
using SharpDX.Direct2D1;
using SharpDX.Direct3D;
using SharpDX.Direct3D11;
using SharpDX.DXGI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;
using SharpDX.Mathematics.Interop;
using PixelFormat = SharpDX.Direct2D1.PixelFormat;

namespace RLib.UI.Drawing
{
    public abstract class D2dControl : System.Windows.Controls.Image
    {
        // - field -----------------------------------------------------------------------

        private SharpDX.Direct3D11.Device device;
        private Texture2D renderTarget;
        private Dx11ImageSource d3DSurface;
        private RenderTarget d2DRenderTarget;
        public SharpDX.Direct2D1.Factory d2DFactory;

        public SharpDX.DirectWrite.Factory FactoryDWrite ;

        private readonly Stopwatch renderTimer = new Stopwatch();

        protected ResourceCache resCache = new ResourceCache();

        private long lastFrameTime = 0;
        private long lastRenderTime = 0;
        private int frameCount = 0;
        private int frameCountHistTotal = 0;
        private Queue<int> frameCountHist = new Queue<int>();


        public int? FpsLimit { get; set; }                          // 限制fps

        public virtual bool Dirty
        {
            get { return _Dirty;}
            set { _Dirty = value; }
        }

        protected bool _Dirty;

        // - property --------------------------------------------------------------------

        public static bool IsInDesignMode
        {
            get
            {
                var prop = DesignerProperties.IsInDesignModeProperty;
                var isDesignMode = (bool)DependencyPropertyDescriptor.FromProperty(prop, typeof(FrameworkElement)).Metadata.DefaultValue;
                return isDesignMode;
            }
        }

        private static readonly DependencyPropertyKey FpsPropertyKey = DependencyProperty.RegisterReadOnly(
            "Fps",
            typeof(int),
            typeof(D2dControl),
            new FrameworkPropertyMetadata(0, FrameworkPropertyMetadataOptions.None)
            );

        public static readonly DependencyProperty FpsProperty = FpsPropertyKey.DependencyProperty;

        public int Fps
        {
            get { return (int)GetValue(FpsProperty); }
            protected set { SetValue(FpsPropertyKey, value); }
        }

        public static DependencyProperty RenderWaitProperty = DependencyProperty.Register(
            "RenderWait",
            typeof(int),
            typeof(D2dControl),
            new FrameworkPropertyMetadata(2, OnRenderWaitChanged)
            );

        public int RenderWait
        {
            get { return (int)GetValue(RenderWaitProperty); }
            set { SetValue(RenderWaitProperty, value); }
        }

        // - public methods --------------------------------------------------------------

        public D2dControl()
        {
            FpsLimit = 1000;

            Dirty = true;

            base.Loaded += Window_Loaded;
            base.Unloaded += Window_Closing;


            base.Stretch = System.Windows.Media.Stretch.Fill;
            RenderOptions.SetBitmapScalingMode(this, BitmapScalingMode.NearestNeighbor);   // 必须设置，不然会出现偏色情况，类似开了antialise模式
        }

        public abstract void Render(RenderTarget target);

        // - event handler ---------------------------------------------------------------

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (D2dControl.IsInDesignMode)
            {
                return;
            }

            StartD3D();
            StartRendering();
        }

        private void Window_Closing(object sender, RoutedEventArgs e)
        {
            if (D2dControl.IsInDesignMode)
            {
                return;
            }

            StopRendering();
            EndD3D();
        }

        public void         OnRendering(object sender, EventArgs e)       // 该函数挂载CompositionTarget.Rendering, 以大约60fps被调用
        {
            if (!renderTimer.IsRunning)
            {
                return;
            }

            if (!Dirty)
                return;


            // fps limit 这里fps limit的实现有一定问题
            //if (FpsLimit != null)
            //{
            //    if (renderTimer.ElapsedMilliseconds - lastRenderTime < 1000 / FpsLimit)
            //        return;
            //}


            // 添加一个dirty判断，如果dirty，再渲染，否则不变, 这样可以大大加快速度


            try
            {
                PrepareAndCallRender();
                d3DSurface.InvalidateD3DImage();

                lastRenderTime = renderTimer.ElapsedMilliseconds;


            }
            catch (Exception exception)
            {
                RLibUIDrawing.Logger.Error("D2dControl:OnRendering " + exception);
            }

                Dirty = false;
        }

        protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
        {
            Dirty = true;
            CreateAndBindTargets();
            base.OnRenderSizeChanged(sizeInfo);
            Debug.WriteLine(ActualWidth + " " + ActualHeight);
        }

        private void OnIsFrontBufferAvailableChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (d3DSurface.IsFrontBufferAvailable)
            {
                StartRendering();
            }
            else
            {
                StopRendering();
            }
        }

        private static void OnRenderWaitChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = (D2dControl)d;
            control.d3DSurface.RenderWait = (int)e.NewValue;
        }

        // - private methods -------------------------------------------------------------

        private void StartD3D()
        {
            FactoryDWrite = new SharpDX.DirectWrite.Factory();
            device = new SharpDX.Direct3D11.Device(DriverType.Hardware, DeviceCreationFlags.BgraSupport);

            d3DSurface = new Dx11ImageSource();
            d3DSurface.IsFrontBufferAvailableChanged += OnIsFrontBufferAvailableChanged;

            d2DFactory = new SharpDX.Direct2D1.Factory();

            //FactoryDWrite = new SharpDX.DirectWrite.Factory();

            CreateAndBindTargets();

            base.Source = d3DSurface;
        }

        private void EndD3D()
        {
            _StrokeStyles.ForEach(p=>p.Dispose());
            _StrokeStyles.Clear();


            if(d3DSurface != null)
                d3DSurface.IsFrontBufferAvailableChanged -= OnIsFrontBufferAvailableChanged;
            base.Source = null;

            Disposer.SafeDispose(ref d2DRenderTarget);
            Disposer.SafeDispose(ref d2DFactory);
            Disposer.SafeDispose(ref FactoryDWrite);
            Disposer.SafeDispose(ref d3DSurface);
            Disposer.SafeDispose(ref renderTarget);
            Disposer.SafeDispose(ref device);
        }

        private void CreateAndBindTargets()
        {
            if (d3DSurface == null)
            {
                return;
            }

            d3DSurface.SetRenderTarget(null);

            Disposer.SafeDispose(ref d2DRenderTarget);
            Disposer.SafeDispose(ref renderTarget);

            // 不能为0， 至少要设一个大于0的值
            // 所以Image的height不可以过小
            var width = Math.Max((int)ActualWidth, 1);
            var height = Math.Max((int)ActualHeight, 1);

            //var width = (int) ActualWidth;
            //var height = (int) ActualHeight;

            var renderDesc = new Texture2DDescription
            {
                BindFlags = BindFlags.RenderTarget | BindFlags.ShaderResource,
                Format = Format.B8G8R8A8_UNorm,
                Width = width,
                Height = height,
                MipLevels = 1,
                SampleDescription = new SampleDescription(1, 0),
                Usage = ResourceUsage.Default,
                OptionFlags = ResourceOptionFlags.Shared,
                CpuAccessFlags = CpuAccessFlags.None,
                ArraySize = 1
            };

            renderTarget = new Texture2D(device, renderDesc);

            var surface = renderTarget.QueryInterface<Surface>();

            var rtp = new RenderTargetProperties(new PixelFormat(Format.Unknown, SharpDX.Direct2D1.AlphaMode.Premultiplied));
            d2DRenderTarget = new RenderTarget(d2DFactory, surface, rtp);
            d2DRenderTarget.AntialiasMode = AntialiasMode.Aliased;          // 必须设置
            d2DRenderTarget.TextAntialiasMode = TextAntialiasMode.Cleartype;
            //d2DRenderTarget.Transform = new RawMatrix3x2(1, 0, 0, 1, 0, 0);
            resCache.RenderTarget = d2DRenderTarget;

            d3DSurface.SetRenderTarget(renderTarget);

            device.ImmediateContext.Rasterizer.SetViewport(0, 0, width, height, 0.0f, 1.0f);
        }

        private void StartRendering()
        {
            if (renderTimer.IsRunning)
            {
                return;
            }

            System.Windows.Media.CompositionTarget.Rendering += OnRendering;
            renderTimer.Start();
        }

        private void StopRendering()
        {
            if (!renderTimer.IsRunning)
            {
                return;
            }

            System.Windows.Media.CompositionTarget.Rendering -= OnRendering;
            renderTimer.Stop();
        }

        private void PrepareAndCallRender()
        {
            if (device == null)
            {
                return;
            }

            d2DRenderTarget.BeginDraw();
            Render(d2DRenderTarget);
            d2DRenderTarget.EndDraw();

            CalcFps();

            device.ImmediateContext.Flush();
        }

        private void CalcFps()
        {
            frameCount++;
            if (renderTimer.ElapsedMilliseconds - lastFrameTime > 1000)
            {
                frameCountHist.Enqueue(frameCount);
                frameCountHistTotal += frameCount;
                if (frameCountHist.Count > 5)
                {
                    frameCountHistTotal -= frameCountHist.Dequeue();
                }

                Fps = frameCountHistTotal / frameCountHist.Count;

                frameCount = 0;
                lastFrameTime = renderTimer.ElapsedMilliseconds;
            }
        }


        protected List<StrokeStyle> _StrokeStyles = new List<StrokeStyle>();
    }
}
