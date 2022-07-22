using System.ComponentModel;
using System.Drawing;

namespace SuperPerformanceChart
{
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class PointStyle
    {
        private SolidBrush _brush;
        private Color _color;

        public bool Visible { get; set; } = true;

        public PointStyle(Color oColor)
        {
            _brush = new SolidBrush(oColor);
            _color = oColor;
        }

        [Browsable(false)]
        public SolidBrush Brush { get => _brush; }

        public Color Color
        {
            get => _color;
            set
            {
                _color = value;

                _brush.Dispose();
                _brush = new SolidBrush(_color);
            }
        }

        public float Size { get; set; } = 3;
    }
}