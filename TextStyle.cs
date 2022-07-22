using System.ComponentModel;
using System.Drawing;

namespace SuperPerformanceChart
{
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class TextStyle
    {
        private SolidBrush _brush;
        private Font m_font;

        public TextStyle(Color oColor)
        {
            _brush = new SolidBrush(oColor);
            m_font = new Font(System.Windows.Forms.SystemInformation.MenuFont.Name, 8, FontStyle.Regular);
        }

        public Font Font
        {
            get
            {
                return m_font;
            }

            set
            {
                m_font.Dispose();
                m_font = value;
            }
        }

        [Browsable(false)]
        public SolidBrush Brush { get => _brush; }

        public Color Color
        {
            get
            {
                return _brush.Color;
            }

            set
            {
                // recreate these
                _brush.Dispose();
                _brush = new SolidBrush(value);
            }
        }
    }
}