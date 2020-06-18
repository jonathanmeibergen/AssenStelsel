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
        private int raster = 16;
        private int unit = 8;

        public MainWindow()
        {
            InitializeComponent();

            this.MouseMove += MainWindow_MouseMove;

            cmbColorsStroke.ItemsSource = typeof(Colors).GetProperties();
            cmbColorsStroke.SelectedIndex = 1;
            cmbColorsFill.ItemsSource = typeof(Colors).GetProperties();
            cmbColorsFill.SelectedIndex = 1;

            mainCanvas.MouseDown += MainCanvas_MouseDown;

            ptM = new Point(0, 0);

        }

        private Path getUnitPathFromInt(int unit)
        {
            string unitString = unit.ToString();
            Geometry unitGeom;

            // Create the initial formatted text string.
            FormattedText formattedText = new FormattedText(unitString,
                                                            CultureInfo.GetCultureInfo(CultureInfo.CurrentCulture.Name),
                                                            FlowDirection.LeftToRight,
                                                            new Typeface("Verdana"),
                                                            10,
                                                            Brushes.Black,
                                                            VisualTreeHelper.GetDpi(this).PixelsPerDip);

            // Use an Italic font style beginning at the 28th character and continuing for 28 characters.
            formattedText.SetFontStyle(FontStyles.Normal);

            // Convert
            unitGeom = formattedText.BuildGeometry(new Point(5, 5));

            Path textPath = new Path();
            textPath.Fill = Brushes.Black;
            textPath.Data = unitGeom;

            return textPath;
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

            Path axisPath = new Path();
            axisPath.Stroke = Brushes.Black;
            axisPath.StrokeThickness = 1;
            axisPath.Data = axis;

            mainCanvas.Children.Clear();
            //mainCanvas.Children.Add(axisPath);

            drawLargeGrid(pt);

            //mainCanvas.Children.Add(textPath);

            //axis.Transform = new TranslateTransform(5, 60);

            ptM = pt;
        }

        private void drawGrid(Point origin)
        {
            PathGeometry gridGeom = new PathGeometry();
            Path gridPath = new Path();
            gridPath.Stroke = Brushes.Black;
            gridPath.StrokeThickness = 1;
            

            int canvasWidth = Convert.ToInt32(mainCanvas.ActualWidth);
            int canvasHeight = Convert.ToInt32(mainCanvas.ActualHeight);

            //where to start drawing sub unit lines from left and top most position of the canvas
            int offsetX = Convert.ToInt32(origin.X % raster);
            int offsetY = Convert.ToInt32(origin.Y % raster);

            //where to start drawing unit lines from left and top most position of the canvas
            int offsetXUnit = Convert.ToInt32(origin.X % (raster * unit));
            int offsetYUnit = Convert.ToInt32(origin.Y % (raster * unit));

            int drawx = offsetX;
            while (drawx < canvasWidth)
            {
                LineGeometry verticalUnit = new LineGeometry();
                //verticalUnit.StartPoint = new Point(0, pt.Y);
                //verticalUnit.EndPoint = new Point(mainCanvas.ActualWidth, pt.Y);

                drawx += raster;
            }

            //gridPath.Data = 

            //LineGeometry yAxis = new LineGeometry();
            //yAxis.StartPoint = new Point(pt.X, 0);
            //yAxis.EndPoint = new Point(pt.X, mainCanvas.ActualHeight);

            //axis.AddGeometry(xAxis);
            //axis.AddGeometry(yAxis);
        }

        private void drawLargeGrid(Point pt){

            Point offset = new Point(pt.X % raster, pt.Y % raster);
            Point offsetUnit = new Point(pt.X % (raster * unit), pt.Y % (raster * unit));

            PathGeometry gridSubGeom  = new PathGeometry();
            PathGeometry gridUnitGeom = new PathGeometry();
            PathGeometry gridAxisGeom = new PathGeometry();
            Path         gridSubPath  = new Path();
            Path         gridUnitPath = new Path();
            Path         gridAxisPath = new Path();

            int canvasWidth  = Convert.ToInt32(mainCanvas.ActualWidth);
            int canvasHeight = Convert.ToInt32(mainCanvas.ActualHeight);

            gridSubPath.Stroke          = Brushes.Gray;
            gridSubPath.StrokeThickness = 0.1;

            gridUnitPath.Stroke          = Brushes.Black;
            gridUnitPath.StrokeThickness = 2;

            gridAxisPath.Stroke = Brushes.Red;
            gridAxisPath.StrokeThickness = 2;

            for (int i = Convert.ToInt32(offset.X % raster); i < canvasWidth; i+=raster)
            {
                LineGeometry verticalLine = new LineGeometry();
                verticalLine.StartPoint = new Point(i, 0);
                verticalLine.EndPoint = new Point(i, canvasHeight);

                if ((i - offsetUnit.X) % (raster * unit) == 0)
                {
                    if (i == Convert.ToInt32(pt.X))
                        gridAxisGeom.AddGeometry(verticalLine);
                    else
                        gridUnitGeom.AddGeometry(verticalLine);
                }
                else
                    gridSubGeom.AddGeometry(verticalLine);
            }

            //Transform = new TranslateTransform(new Point((canvasWidth/2) offset.X,offset.Y);

            //gridSubGeom.Transform = offsetTransform;
            gridSubPath.Data  = gridSubGeom;

            //gridUnitGeom.Transform = offsetTransform;
            gridUnitPath.Data = gridUnitGeom;

            gridAxisPath.Data = gridAxisGeom;

            mainCanvas.Children.Add(gridSubPath);
            mainCanvas.Children.Add(gridUnitPath);
            mainCanvas.Children.Add(gridAxisPath);

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
