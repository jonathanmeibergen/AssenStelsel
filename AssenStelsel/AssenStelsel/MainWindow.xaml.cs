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

namespace AssenStelsel
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Point ptM;

        public MainWindow()
        {
            InitializeComponent();

            this.MouseMove += MainWindow_MouseMove;

            mainCanvas.MouseDown += MainCanvas_MouseDown;

            ptM = new Point(0, 0);

        }

        private void MainCanvas_MouseDown(object sender, MouseButtonEventArgs e)
        {

            Point pt = Mouse.GetPosition(mainCanvas);
            PathGeometry axis = new PathGeometry();

            LineGeometry xAxis = new LineGeometry();
            xAxis.StartPoint = new Point(0, pt.Y);
            xAxis.EndPoint = new Point(mainCanvas.ActualWidth, pt.Y);

            LineGeometry yAxis = new LineGeometry();
            yAxis.StartPoint = new Point(pt.X, 0);
            yAxis.EndPoint = new Point(pt.X, mainCanvas.ActualHeight);

            axis.AddGeometry(xAxis);
            axis.AddGeometry(yAxis);

            Path xPath = new Path();
            xPath.Stroke = Brushes.Black;
            xPath.StrokeThickness = 1;
            xPath.Data = axis;

            mainCanvas.Children.Clear();
            mainCanvas.Children.Add(xPath);

            ptM = pt;
        }

        private void MainWindow_MouseMove(object sender, MouseEventArgs e)
        {
            Point pt = Mouse.GetPosition(mainCanvas);
            double xn, yn;
            


            lbVCX.Content = "x: " + pt.X;
            lbVCY.Content = "y: " + pt.Y;

            //normalize x and y coordinates
            xn = pt.X - ptM.X;
            yn = pt.Y - ptM.Y;

            lbMX.Content = "x: " + xn;
            lbMY.Content = "y: " + yn;

            //output cartesian coordinates
            lbWCX.Content = "x: " + xn;
            lbWCY.Content = "y: " + -1 * yn;

        }

    }
}
