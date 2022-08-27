// created: 2022/07/26 16:11
// author:  rush
// email:   yacper@gmail.com
// 
// purpose:
// modifiers:

using System.Windows.Controls;
using System.Windows.Threading;
using Microsoft.Maui.Graphics;
using Microsoft.Maui.Graphics.Skia;
using SkiaSharp;
using SkiaSharp.Views.WPF;

namespace RLib.Graphics.Wpf;

public class D2ViewControl:UserControl
{
    public ID2View D2View { get; protected set; }

    public virtual bool Dirty { get; set; } // 是否dirty，如果dirty，需要重新绘制

    public virtual void OnRendering(ID2View view)
    {
        view.Reset();
    }


    public D2ViewControl()
    {
        D2View                  =  new D2View();
        D2View.OnRendering += OnRendering;
        SKElement_              =  new SKElement(){IgnorePixelScaling = true}; // 忽略高dpi scale情况
        SKElement_.PaintSurface += SKElement__PaintSurface;
        SKElement_.SizeChanged  += SKElement__SizeChanged;

        Content = SKElement_;

        Timer_ = RLib.Base.Timer.Add((objects =>
                                         {
                                             if (Dirty)
                                                 SKElement_.InvalidateVisual();
                                             Dirty = false;

                                         }), null, new TimeSpan(0, 0, 0, 0, 10), 0);

        //m_pTimer.Interval  =  TimeSpan.FromMilliseconds(1000 / 120);
        //m_pTimer.Tick += (sender, args) =>
        //{
        //    if(Dirty)
        //        SKElement_.InvalidateVisual();
        //    Dirty = false;
        //};
        //m_pTimer.IsEnabled =  true;
        //m_pTimer.Start();
    }

    private void SKElement__SizeChanged(object sender, System.Windows.SizeChangedEventArgs e)
    {
        SKElement_.InvalidateVisual();
    }

    private void SKElement__PaintSurface(object sender, SkiaSharp.Views.Desktop.SKPaintSurfaceEventArgs e)
    {
        ICanvas canvas = new SkiaCanvas() { Canvas = e.Surface.Canvas };
        (D2View as D2View).Canvas = canvas;
        (D2View as D2View).Width = e.Info.Width;
        (D2View as D2View).Height = e.Info.Height;

        (D2View as D2View).Render();
    }

  
    protected SKElement SKElement_;

    //DispatcherTimer m_pTimer = new DispatcherTimer(DispatcherPriority.Send);
    private object Timer_ = null;
}