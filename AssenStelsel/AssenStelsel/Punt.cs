using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace AssenStelsel
{
    public class Punt
    {
        public Punt(Canvas value)
        {
           cnv = value;
        }
        public Canvas cnv {get; private set;}
        public double ScreenX { get; set; }
        public double ScreenY { get; set; }
        public double CartesianX { get; set; }
        public double CartesianY { get; set; }
        public int rasterX { get; set; }
        public int rasterY { get; set; }
        public Brush color { get; set; }
        public double thickness { get; set; }
        public Brush colorBorder { get; set; }
        public double thicknessBorder { get; set; }
        public double size { get; set; }
        public string ScreenCoordinatesAsString {
            get { return String.Format("screen X: {0}, screen Y: {1}", ScreenX, ScreenY); }
        }
        public string CartesianCoordinatesAsString
        {
            get { return String.Format("Cartesian X: {0}, Cartesian Y: {1}", CartesianX, CartesianY); }
        }
        public string AllCoordinatesAsString
        {
            get { return ScreenCoordinatesAsString + ". " + CartesianCoordinatesAsString; }
        }

        public override string ToString()
        {
            return ScreenCoordinatesAsString;
        }

        public void DrawPunt()
        {
            Ellipse ell         = new Ellipse();
            ell.Width = ell.Height = size;
            ell.Stroke          = colorBorder;
            ell.StrokeThickness = thicknessBorder;
            ell.Fill            = color;
            ell.Margin = new Thickness(10,10,10,10);
            //ell.LayoutTransform = new TranslateTransform(ScreenX, ScreenY);
            cnv.Children.Add(ell);
        }

    }
}
