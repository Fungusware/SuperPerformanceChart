using System.ComponentModel;
using System.Drawing;

namespace SuperPerformanceChart
{
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class LineStyle
    {
        public bool Visible { get; set; } = true;

        private Pen m_pen;

        public LineStyle(Color oColor)
        {
            m_pen = new Pen(oColor, 2);
        }

        [Browsable(false)]
        public Pen Pen { get => m_pen; }

        public Color Color
        {
            get
            {
                return m_pen.Color;
            }

            set
            {
                m_pen.Color = value;
            }
        }

        public System.Drawing.Drawing2D.DashStyle DashStyle
        {
            get
            {
                return m_pen.DashStyle;
            }

            set
            {
                m_pen.DashStyle = value;
            }
        }

        public float Width
        {
            get
            {
                return m_pen.Width;
            }

            set
            {
                m_pen.Width = value;
            }
        }
    }
}