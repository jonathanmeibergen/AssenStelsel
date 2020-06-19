using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
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

        private int selectedTool;

        private List<Punt> Punten;

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

        private Path getUnitPathFromInt(int unit, Point loc)
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
            unitGeom = formattedText.BuildGeometry(loc);

            Path textPath = new Path();
            textPath.Fill = Brushes.Black;
            textPath.Data = unitGeom;

            return textPath;
        }


        private void MainCanvas_MouseDown(object sender, MouseButtonEventArgs e)
        {

            Point pt = Mouse.GetPosition(mainCanvas);
            

            switch (selectedTool)
            {
                case 1:
                    mainCanvas.Children.Clear();
                    drawLargeGrid(pt);
                    ptM = pt;
                    break;
                case 2:
                    drawPoint(pt);
                    break;
                default:
                    break;
            }
            
        }


        private void drawLargeGrid(Point pt){

            Point offset = new Point(pt.X % raster, pt.Y % raster);
            Point offsetUnit = new Point(pt.X % (raster * unit), pt.Y % (raster * unit));
            //calculate unit index
            int xAxisUnitLabel = -1 * Convert.ToInt32(Math.Ceiling(pt.X / (raster * unit)));
            int yAxisUnitLabel = Convert.ToInt32(Math.Ceiling(pt.Y / (raster * unit)));

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

            for (int v = Convert.ToInt32(offset.X % raster); v < canvasWidth; v+=raster)
            {
                LineGeometry verticalLine = new LineGeometry();
                verticalLine.StartPoint = new Point(v, 0);
                verticalLine.EndPoint = new Point(v, canvasHeight);

                if ((v - offsetUnit.X) % (raster * unit) == 0)
                {
                    if (v == Convert.ToInt32(pt.X))
                        gridAxisGeom.AddGeometry(verticalLine);
                    else
                        gridUnitGeom.AddGeometry(verticalLine);

                    // add x axis unit label
                    xAxisUnitLabel += 1;
                    mainCanvas.Children.Add(getUnitPathFromInt(xAxisUnitLabel * unit, 
                                                                new Point(v + 5, pt.Y + 2)));
                }
                else
                    gridSubGeom.AddGeometry(verticalLine);
            }

            for (int h = Convert.ToInt32(offset.Y % raster); h < canvasHeight; h += raster)
            {
                LineGeometry horizontalLine = new LineGeometry();
                horizontalLine.StartPoint = new Point(0, h);
                horizontalLine.EndPoint = new Point(canvasWidth, h);

                if ((h - offsetUnit.Y) % (raster * unit) == 0)
                {
                    yAxisUnitLabel -= 1;

                    if (h == Convert.ToInt32(pt.Y))
                        gridAxisGeom.AddGeometry(horizontalLine);
                    else
                    {
                        gridUnitGeom.AddGeometry(horizontalLine);
                        // add y axis unit label
                        mainCanvas.Children.Add(getUnitPathFromInt(yAxisUnitLabel * unit,
                                                                new Point(pt.X + 5, h + 2)));
                    }
                }
                else
                    gridSubGeom.AddGeometry(horizontalLine);
            }

            gridSubPath.Data  = gridSubGeom;
            gridUnitPath.Data = gridUnitGeom;
            gridAxisPath.Data = gridAxisGeom;

            mainCanvas.Children.Add(gridSubPath);
            mainCanvas.Children.Add(gridUnitPath);
            mainCanvas.Children.Add(gridAxisPath);

        }

        public void drawPoint(Point pt)
        {
            Punt punt = new Punt(mainCanvas);
            Color color = (Color)(cmbColorsFill.SelectedItem as PropertyInfo).GetValue(null, null);
            Color colorBorder = (Color)(cmbColorsStroke.SelectedItem as PropertyInfo).GetValue(null, null);

            //StackPanel stpFill = cmbColorsFill;
            //StackPanel stpStroke = cmbColorsStroke.SelectedItem as StackPanel;
            //Brush rectFill = ((Rectangle)stpFill.FindName("StrokeFill")).Fill;
            //Rectangle rectStroke = stpStroke.Children[0] as Rectangle;


            punt.color = new SolidColorBrush(color);
            punt.colorBorder = new SolidColorBrush(colorBorder);
            punt.size = sldSize.Value;
            punt.thicknessBorder = sldStrokeThickness.Value;
            punt.ScreenX = pt.X;
            punt.ScreenY = pt.Y;
            //punt.DrawPunt();

            // Create an Ellipse    
            Ellipse ell = new Ellipse();
            ell.Height = sldSize.Value*2;
            ell.Width = sldSize.Value*2;
            // Create a blue and a black Brush    
            // Set Ellipse's width and color    
            ell.StrokeThickness = sldStrokeThickness.Value;
            ell.Stroke = punt.colorBorder;
            // Fill rectangle with blue color    
            ell.Fill = punt.color;
            ell.RenderTransform = new TranslateTransform(pt.X, pt.Y);
            // Add Ellipse to the Grid.    
            mainCanvas.Children.Add(ell);

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

        private void Punt_Click(object sender, RoutedEventArgs e)
        {
            selectedTool = 2;
        }

        private void Grid_Click(object sender, RoutedEventArgs e)
        {
            selectedTool = 1;
        }
    }
}
