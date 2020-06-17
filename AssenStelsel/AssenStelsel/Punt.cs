using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssenStelsel
{
    public class Punt
    {
        private double ScreenX { get; set; }
        private double ScreenY { get; set; }
        private double CartesianX { get; set; }
        private double CartesianY { get; set; }
        private int rasterX { get; set; }
        private int rasterY { get; set; }
        private int color { get; set; }
        private int thickness { get; set; }
        private int colorBorder { get; set; }
        private int thicknessBorder { get; set; }
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


    }
}
