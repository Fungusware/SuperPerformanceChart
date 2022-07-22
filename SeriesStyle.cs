using System;
using System.ComponentModel;
using System.Drawing;

namespace SuperPerformanceChart
{
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class SeriesStyle
    {
        public SeriesStyle(Color basecolor)
        {
            // Defaults
            Text = new TextStyle(Color.Black);
            Text.Font = new Font("Verdana", 8);

            Line = new LineStyle(basecolor);
            Line.Width = 2;

            AverageLine = new LineStyle(basecolor);
            AverageLine.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
            AverageLine.Visible = false;

            BestFitLine = new LineStyle(basecolor);
            BestFitLine.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
            BestFitLine.Visible = false;

            Fill = new FillStyle(basecolor);
            Fill.Alpha = 120;

            Point = new PointStyle(basecolor);
            Point.Visible = false;

            Smooth = false;
            Tension = 0.5F;
        }

        public SeriesStyle() : this(Color.CornflowerBlue)
        {
        }

        #region Properties

        public string NumberFormat { get; set; } = "0.00";

        public bool Smooth { get; set; }
        public float Tension { get; set; } = 0.3F;

        public FillStyle Fill { get; }

        public LineStyle Line { get; }

        public LineStyle BestFitLine { get; }

        public LineStyle AverageLine { get; }

        public TextStyle Text { get; }

        public PointStyle Point { get; }

        #endregion 
    }
}