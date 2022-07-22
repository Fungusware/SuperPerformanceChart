using System.ComponentModel;
using System.Drawing;

namespace SuperPerformanceChart
{
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class ProgressStyle
    {
        private FillStyle m_oFillStyle;
        private TextStyle m_oTextStyle;

        public bool Visible { get; set; } = true;
        public bool DrawOnTop { get; set; } = false;
        public float HeightFraction { get; set; } = 1;

        public ProgressStyle()
        {
            m_oFillStyle = new FillStyle(Color.Green);
            m_oTextStyle = new TextStyle(Color.Black);
        }

        public FillStyle Fill
        {
            get
            {
                return m_oFillStyle;
            }
        }

        public TextStyle Text
        {
            get
            {
                return m_oTextStyle;
            }
        }
    }
}