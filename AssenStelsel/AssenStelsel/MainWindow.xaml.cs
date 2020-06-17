using System;
using System.Collections.Generic;
using System.Globalization;
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
            Geometry textGeom;

            LineGeometry xAxis = new LineGeometry();
            xAxis.StartPoint = new Point(0, pt.Y);
            xAxis.EndPoint = new Point(mainCanvas.ActualWidth, pt.Y);

            LineGeometry yAxis = new LineGeometry();
            yAxis.StartPoint = new Point(pt.X, 0);
            yAxis.EndPoint = new Point(pt.X, mainCanvas.ActualHeight);

            axis.AddGeometry(xAxis);
            axis.AddGeometry(yAxis);

            Path axisPath = new Path();
            axisPath.Stroke = Brushes.Black;
            axisPath.StrokeThickness = 1;
            axisPath.Data = axis;

            string testString = "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor";

            // Create the initial formatted text string.
            FormattedText formattedText = new FormattedText(
                testString,
                CultureInfo.GetCultureInfo("en-us"),
                FlowDirection.LeftToRight,
                new Typeface("Verdana"),
                16,
                Brushes.Black,
                VisualTreeHelper.GetDpi(this).PixelsPerDip);

            // Set a maximum width and height. If the text overflows these values, an ellipsis "..." appears.
            formattedText.MaxTextWidth = 300;
            formattedText.MaxTextHeight = 240;

            // Use a larger font size beginning at the first (zero-based) character and continuing for 5 characters.
            // The font size is calculated in terms of points -- not as device-independent pixels.
            //formattedText.SetFontSize(36 * (96.0 / 72.0), 0, 5);

            // Use a Bold font weight beginning at the 6th character and continuing for 11 characters.
            formattedText.SetFontWeight(FontWeights.Bold, 6, 11);

            // Use a linear gradient brush beginning at the 6th character and continuing for 11 characters.
            //formattedText.SetForegroundBrush(Brushes.Black);

            // Use an Italic font style beginning at the 28th character and continuing for 28 characters.
            formattedText.SetFontStyle(FontStyles.Italic, 28, 28);

            // Draw the formatted text string to the DrawingContext of the control.
            textGeom = formattedText.BuildGeometry(new Point(5, 5));

            Path textPath = new Path();
            textPath.Fill = Brushes.Black;
            textPath.Data = textGeom;

            mainCanvas.Children.Clear();
            mainCanvas.Children.Add(axisPath);
            mainCanvas.Children.Add(textPath);

            ptM = pt;
        }

        private void drawGrid()
        {

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
