// created: 2022/07/26 16:11
// author:  rush
// email:   yacper@gmail.com
// 
// purpose:
// modifiers:

using System.Windows.Controls;
using Microsoft.Maui.Graphics;
using Microsoft.Maui.Graphics.Skia;
using SkiaSharp;
using SkiaSharp.Views.WPF;

namespace RLib.Graphics.Wpf;

public class D2ViewControl:UserControl
{
    public ID2View D2View { get; protected set; }

    public virtual void OnRendering(ID2View view)
    {
        view.Reset();

    }


    public D2ViewControl()
    {
        D2View                  =  new D2View();
        D2View.OnRendering += OnRendering;
        SKElement_              =  new SKElement();
        SKElement_.PaintSurface += SKElement__PaintSurface;
        SKElement_.SizeChanged  += SKElement__SizeChanged;

        Content = SKElement_;
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

  
    private SKElement SKElement_;

    
}