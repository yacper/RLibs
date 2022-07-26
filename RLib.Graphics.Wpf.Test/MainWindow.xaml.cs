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
using Colors = Microsoft.Maui.Graphics.Colors;
using Point = Microsoft.Maui.Graphics.Point;

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

        private void D2View_OnRendering(ID2View obj)
        {
            obj.Reset();

            obj.Background = Colors.Black;
            obj.DrawLine(new Point(0,0), new Point(100,100), new Stroke());
        }
    }
}
