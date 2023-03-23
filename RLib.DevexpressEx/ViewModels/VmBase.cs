/********************************************************************
    created:	2022/2/9 17:21:26
    author:		rush
    email:		yacper@gmail.com	
	
    purpose:    基础ViewModel
    modifiers:	
*********************************************************************/

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Media;
using DevExpress.Mvvm;

namespace RLib.DevexpressEx.ViewModels
{
public abstract class VmBase : DevExpress.Mvvm.ViewModelBase, IDisposable
{
    public string BindableName { get { return GetBindableName(DisplayName); } }

    public virtual string DisplayName { get => GetProperty(() => DisplayName); set => SetProperty(() => DisplayName, value); }

    public virtual object DisplayValue
    {
        get => GetProperty(() => DisplayValue);
        set => SetProperty(() => DisplayValue, value, (p) => { DisplayName = (DisplayFormatter != null) ? DisplayFormatter(DisplayValue) : DisplayValue.ToString(); });
    }

    public Func<object, string> DisplayFormatter { get => GetProperty(() => DisplayFormatter); set => SetProperty(() => DisplayFormatter, value); }

    public virtual ImageSource Glyph        { get => GetProperty(() => Glyph);        set => SetProperty(() => Glyph, value); }
    public virtual ImageSource StateImg     { get => GetProperty(() => StateImg);     set => SetProperty(() => StateImg, value); }     // 状态图标
    public virtual string      BadgeContent { get => GetProperty(() => BadgeContent); set => SetProperty(() => BadgeContent, value); } // badge

    public virtual double MaxWidth { get => GetProperty(() => MaxWidth); set => SetProperty(() => MaxWidth, value); }


    string GetBindableName(string name)
    {
        if (name == null)
            return "";
        return "_" + Regex.Replace(name, @"\W", "");
    }

#region IDisposable Members
    public void Dispose()
    {
        if (Disposed_)
            return;

        OnDispose();

        Disposed_ = true;

#if DEBUG
        Debug.WriteLine($"{DisplayName}({GetType().Name})({GetHashCode()}) Disposed");
#endif
    }

    protected virtual void OnDispose() { }
    ~VmBase()
    {
        Dispose();

#if DEBUG
        Debug.WriteLine($"{DisplayName}({GetType().Name})({GetHashCode()}) Finalized");
#endif
    }

    private bool Disposed_ = false;
#endregion
}
}