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
using Microsoft.Maui.Graphics.Text;

namespace RLib.Graphics
{
public interface ID2View : INotifyPropertyChanged
{
    double Width  { get; }
    double Height { get; }

    Color Background { get; set; } // 背景色

    bool EnableBatching { get; set; } // 开启batching的时候,不保证绘图顺序，须通过zindex自己控制
    bool IsDirty        { get; }      // 是否dirty，如果dirty，需要重新绘制

    void Reset();       // 删除所有绘制元素
    void OnRender();

    event Action<ID2View> OnRendering; // Render 开始前
    event Action<ID2View> OnRendered;  // render结束



    void DrawLine(Point              start, Point  end,    Stroke stroke,      Rect? clip   = null, int zindex = 0);
    void DrawLine(IEnumerable<Point> vs,    Stroke stroke, Rect?  clip = null, int   zindex = 0);

    void DrawRectangle(Rect rect, Stroke stroke, Fill fill=null, Rect? clip = null, int zindex = 0);
    void DrawRoundedRectangle(Rect rect, float cornerRadius, Stroke stroke, Fill fill=null, Rect? clip = null, int zindex = 0);

    void DrawEllipse(Rect rect, Stroke stroke, Fill fill=null, Rect? clip = null, int zindex = 0);

    void DrawPath(PathF path, Stroke stroke, Fill fill =null, WindingMode windingMode =WindingMode.NonZero, Rect? clip = null, int zindex = 0);

    void DrawImage(IImage image, Rect rect, Stroke stroke, Fill fill=null, Rect? clip = null, int zindex = 0);

    public void DrawString(string value, Point pt, FontSpec font, HorizontalAlignment horizontalAlignment, Rect? clip = null, int zindex = 0);
    public void DrawString(string value, Rect rect, FontSpec font,  HorizontalAlignment horizontalAlignment, VerticalAlignment verticalAlignment, TextFlow textFlow = TextFlow.ClipBounds,
        float                     lineSpacingAdjustment = 0, Rect? clip = null, int zindex = 0);
	public SizeF GetStringSize(string value, IFont font, float fontSize);
    public SizeF GetStringSize(string value, IFont font, float fontSize, HorizontalAlignment horizontalAlignment, VerticalAlignment verticalAlignment);

    public void DrawText(IAttributedText value, Rect rect, FontSpec font, Rect? clip = null, int zindex = 0);


    //TextInfo DrawText(string text,                                 string             fontFamlity, int fontSize, Rect rect, Color foregroundColor, Color? backColor = null,
    //    TextAlignment        textAlignment = TextAlignment.Center, ParagraphAlignment paragraphAlignment = ParagraphAlignment.Center, int zindex = 0);

    //// 绘制bitmap，dest不设置的话，直接以原始尺寸绘制到(0,0)
    //// src不设置的话，绘制原始尺寸
    //void DrawImage(System.Drawing.Bitmap image, string imageId, Rect? destinationRectangle = null, float opacity = 1.0f, BitmapInterpolationMode interpolationMode = BitmapInterpolationMode.Linear,
    //    Rect?                            sourceRectangle = null, int zindex = 0);

    //void DrawImage(string imageFilePath,          Rect? destinationRectangle = null, float opacity = 1.0f, BitmapInterpolationMode interpolationMode = BitmapInterpolationMode.Linear,
    //    Rect?             sourceRectangle = null, int   zindex               = 0);




}

}