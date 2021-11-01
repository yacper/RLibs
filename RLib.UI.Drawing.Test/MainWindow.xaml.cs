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
using System.Windows.Media;
using DataModel;

namespace RLib.UI.Drawing.Test
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            string text = "hello, world. oh my god........";

            D2Control.RenderEx += (s) =>
            {
                s.DrawText(text, "Calibri", 18, new Rect(0, 0, 200, 200), Colors.White, Colors.Blue);
                s.DrawText(text, "Calibri", 18, new Rect(200, 0, 200, 200), Colors.White, Colors.Red, TextAlignment.Leading, ParagraphAlignment.Center);
                s.DrawText(text, "Calibri", 18, new Rect(400, 0, 200, 200), Colors.White, Colors.Red, TextAlignment.Trailing, ParagraphAlignment.Center);
                s.DrawText(text, "Calibri", 18, new Rect(0, 200, 200, 200), Colors.White, Colors.Green, TextAlignment.Center, ParagraphAlignment.Center);
                s.DrawText(text, "Calibri", 18, new Rect(200, 200, 200, 200), Colors.White, Colors.Red, TextAlignment.Leading, ParagraphAlignment.Far);
                s.DrawText(text, "Calibri", 18, new Rect(400, 200, 200, 200), Colors.White, Colors.Red, TextAlignment.Trailing, ParagraphAlignment.Near);

                s.DrawLine(new Point(0, 0), new Point(100, 100), Colors.White);


                s.DrawImage("ctp.png", new Rect(600, 0, 100, 100));


                s.DrawGeometry(new List<Point>(){new Point(600, 300), new Point(700, 300), new Point(550, 500)}, Colors.Red, 1, ELineStyle.LINE_SOLID, Colors.Red);
            };

        }
    }
}
