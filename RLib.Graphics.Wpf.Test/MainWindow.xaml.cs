using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Maui.Graphics;
using Color = Microsoft.Maui.Graphics.Color;
using Colors = Microsoft.Maui.Graphics.Colors;
using Point = Microsoft.Maui.Graphics.Point;
using Rect = Microsoft.Maui.Graphics.Rect;
using Size = Microsoft.Maui.Graphics.Size;

namespace RLib.Graphics.Wpf.Test
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            D2.D2View.OnRendering += D2View_OnRendering;
        }

        private void D2View_OnRendering(ID2View view)
        {
            view.Reset();

            view.Background = Colors.Black;
            view.DrawLine(new Point(0,100), new Point(100,0), new Stroke(){Color = Colors.Yellow}, new Rect(0, 50, 50, 50));
            view.DrawLine(new Point(0,0), new Point(100,100), new Stroke(){Color = Colors.Red});

            view.DrawString("hello", new Point(100,100), new FontSpec(){Color = Colors.White, Font = Font.Default, Size = 14}, Microsoft.Maui.Graphics.HorizontalAlignment.Left);
            view.DrawString("world", new Rect(new Point(100,100), new Microsoft.Maui.Graphics.Size(100, 20)), new FontSpec(){Color = Colors.White, Font = Font.Default, Size = 14}, Microsoft.Maui.Graphics.HorizontalAlignment.Left, Microsoft.Maui.Graphics.VerticalAlignment.Top);

            Size sz = view.GetStringSize("hello", Font.Default, 14, Microsoft.Maui.Graphics.HorizontalAlignment.Left, Microsoft.Maui.Graphics.VerticalAlignment.Center);
            //sz.Width += 3;
            //sz.Height += 3;
            view.DrawString("hello", new Rect(new Point(100,200), sz), new FontSpec(){Color = Colors.White, Font = Font.Default, Size = 14}, Microsoft.Maui.Graphics.HorizontalAlignment.Left, Microsoft.Maui.Graphics.VerticalAlignment.Center, TextFlow.OverflowBounds);

        }

    }
}
