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
using Microsoft.Maui.Graphics;

namespace RLib.Graphics
{
public interface ID2View : INotifyPropertyChanged
{
    double Width  { get; }
    double Height { get; }

    Color Background { get; set; } // 背景色

    bool EnableBatching { get; set; } // 开启batching的时候,不保证绘图顺序，须通过zindex自己控制
    bool IsDirty        { get; }      // 是否dirty，如果dirty，需要重新绘制


    void DrawLine(Point              start, Point  end,    Stroke stroke,      Rect? clip   = null, int zindex = 0);
    void DrawLine(IEnumerable<Point> vs,    Stroke stroke, Rect?  clip = null, int   zindex = 0);

    void DrawRectangle(Rect rect, Stroke stroke, Fill fill=null, Rect? clip = null, int zindex = 0);

    ////void                DrawTriangle(Point a, Point b, Point c, Color clr, int zindex = 0);

    ////void                DrawQuad(Point a, Point b, Point c, Point d, Color clr, int zindex = 0);
    ////void                DrawQuad(Point ld, Point ur, Color clr, int zindex = 0);

    //TextInfo DrawText(string text,                                 string             fontFamlity, int fontSize, Rect rect, Color foregroundColor, Color? backColor = null,
    //    TextAlignment        textAlignment = TextAlignment.Center, ParagraphAlignment paragraphAlignment = ParagraphAlignment.Center, int zindex = 0);

    //// 绘制bitmap，dest不设置的话，直接以原始尺寸绘制到(0,0)
    //// src不设置的话，绘制原始尺寸
    //void DrawImage(System.Drawing.Bitmap image, string imageId, Rect? destinationRectangle = null, float opacity = 1.0f, BitmapInterpolationMode interpolationMode = BitmapInterpolationMode.Linear,
    //    Rect?                            sourceRectangle = null, int zindex = 0);

    //void DrawImage(string imageFilePath,          Rect? destinationRectangle = null, float opacity = 1.0f, BitmapInterpolationMode interpolationMode = BitmapInterpolationMode.Linear,
    //    Rect?             sourceRectangle = null, int   zindex               = 0);

    //void DrawGeometry(List<Point> points, Color strokeColor, float strokeWidth = 1, ELineStyle strokeStyle = ELineStyle.LineSolid, Color? fillColor = null, int zindex = 0);


    void Reset();       // 删除所有绘制元素

    void OnRender();

    event Action<ID2View> OnRendering; // Render 开始前
    event Action<ID2View> OnRendered;  // render结束


}

}