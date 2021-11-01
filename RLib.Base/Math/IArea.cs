/********************************************************************
    created:	2017/4/24 17:48:19
    author:		rush
    email:		
	
    purpose:	

*********************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RLib.Base
{
    public enum	EAreaType	//区域类型
	{
		Circle = 1,	            //圆形
        Triangle = 2,           //三角形
		Quadrilateral =3,		//四边形
	}

    public interface IArea
    {
        EAreaType           Type { get; }

        bool                Contains(RVector2 pt);                           // 是否包含该点（忽略y坐标）

        RVector2              CenterPoint { get; }                            //区域中心点
        //PointF              RandomPoint { get; }                            //区域内随机坐标

        RColor               Color { get; set; }                             // 表示颜色
        bool                Show { get; set; }
    }

    public interface ICircle:IArea                                          //圆
    {
        float               Radius { get; }                                 // 半径
    }

    public interface ITriangle : IArea                                      //三角形
    {
        RVector2              X { get; }
        RVector2              Y { get; }
        RVector2              Z { get; }
    }

    public interface IQuadrilateral:IArea                                   // 任意四边形
    {
        RVector2              W { get; }
        RVector2              X { get; }
        RVector2              Y { get; }
        RVector2              Z { get; }

        RVector2              this[int index] { get; }
    }

}
