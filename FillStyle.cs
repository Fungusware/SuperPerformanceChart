using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace SuperPerformanceChart
{
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class FillStyle
    {
        public Color StartColor { get; set; }
        public Color EndColor { get; set; }
        public int GradientAngle { get; set; } = 270;
        public int Alpha { get; set; } = 120;
        public bool Visible { get; set; } = true;

        public FillStyle(Color color)
        {
            StartColor = color;
            EndColor = color;
        }

        public FillStyle() : this(Color.CornflowerBlue)
        {
        }

        /* TODO ERROR: Skipped RegionDirectiveTrivia */

        internal Brush GetFillBrush(RectangleF rt)
        {
            if (ActualEndColor == Color.Empty || ActualEndColor == ActualStartColor)
            {
                // this is not a gradient
                return new SolidBrush(ActualStartColor);
            }
            else
            {
                return new LinearGradientBrush(rt, ActualStartColor, ActualEndColor, GradientAngle);
            }
        }

        internal Pen GetLinePen()
        {
            return new Pen(EndColor, 2);
        }

        internal Color ActualStartColor
        {
            get
            {
                return Color.FromArgb(Alpha, StartColor);
            }
        }

        internal Color ActualEndColor
        {
            get
            {
                return Color.FromArgb(Alpha, EndColor);
            }
        }

        internal bool HasGradient
        {
            get
            {
                return StartColor != EndColor;
            }
        }

        /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
    }
}