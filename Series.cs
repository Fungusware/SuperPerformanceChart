using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;

namespace SuperPerformanceChart
{
    public class Series
    {
        private string _legendMask = "@name - Current: @current";
        private StringFormatter _legendFormatter;

        [Flags()]
        public enum ValueDisplayTypeEnum
        {
            None = 0,

            Current = 1,

            Max = 2,
            Min = 4,
            Mean = 8,

            VisibleMax = 16,
            VisibleMin = 32,
            VisibleMean = 64
        }

        /// <summary>
        /// Scale mode for value aspect ratio
        /// </summary>
        public enum ScaleModeEnum
        {
            /// <summary>
            /// Percent Scale Mode: Values from 0 to 100 are accepted and displayed
            /// </summary>
            Percent,

            /// <summary>
            /// Relative Scale Mode: Value is shown scaled within the bounds of the currrently visible values
            /// </summary>
            RelativeVisble,

            /// <summary>
            /// Relative Scale Mode: Value is shown scaled within the bounds of all value in the seeries
            /// </summary>
            RelativeAll,

            /// <summary>
            /// Absolute Scale Mode: Values from 0 to the Series Maximum
            /// </summary>
            Absolute
        }

        [Category("Statistics")]
        public LineStyle BestFitLine { get; }

        [Category("Statistics")]
        public LineStyle AverageLine { get; }

        [Category("Statistics")]
        public LineStyle MaxLine { get; }

        [Category("Statistics")]
        public LineStyle MinLine { get; }

        [Category("Appearance")]
        public string NumberFormat { get; set; } = "0.0#";

        [Category("Appearance")]
        public bool Smooth { get; set; } = false;

        [Category("Appearance")]
        public float Tension { get; set; } = 0.5F;

        [Category("Appearance")]
        public FillStyle Fill { get; }

        [Category("Appearance")]
        public LineStyle Line { get; }

        [Category("Appearance")]
        public TextStyle Text { get; }

        [Category("Appearance")]
        public PointStyle Point { get; }

        [Category("Appearance")]
        public int ZOrder { get; set; }

        [Category("Appearance")]
        public bool Visible { get; set; } = true;

        [Category("Behaviour")]
        public ScaleModeEnum ScaleMode { get; set; } = ScaleModeEnum.RelativeAll;

        [Category("Behaviour")]
        public string LegendMask
        {
            get => _legendMask;
            set
            {
                _legendMask = value;
                _legendFormatter = new StringFormatter(_legendMask);
            }
        }

        public StringFormatter LegendFormatter { get => _legendFormatter; }

        /// <summary>
        /// Determine the amount data that a Series will store for Statistical purposes
        /// </summary>
        [Category("Behaviour")]
        public int CacheSize { get; set; } = 256;

        public string Name { get; set; }
        public string Key { get; set; }
        public float MaxValue { get; set; } = 1000;
        public float MinValue { get; set; } = 0;
        internal List<float> DrawValues { get; set; }
        internal Queue<float> QueuedValues { get; set; }
        public bool LegendVisible { get; set; } = true;

        public Series(string sKey, string sName, Color basecolor)
        {
            Key = sKey;
            Name = sName;
            DrawValues = new List<float>();
            QueuedValues = new Queue<float>();

            // Defaults
            Text = new TextStyle(basecolor);

            Line = new LineStyle(basecolor);

            Fill = new FillStyle(basecolor);
            Fill.Visible = false;

            Point = new PointStyle(basecolor);
            Point.Visible = false;

            MinLine = new LineStyle(basecolor);
            MinLine.DashStyle = System.Drawing.Drawing2D.DashStyle.Dot;
            MinLine.Visible = false;

            MaxLine = new LineStyle(basecolor);
            MaxLine.DashStyle = System.Drawing.Drawing2D.DashStyle.Dot;
            MaxLine.Visible = false;

            AverageLine = new LineStyle(basecolor);
            AverageLine.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
            AverageLine.Visible = false;

            BestFitLine = new LineStyle(basecolor);
            BestFitLine.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
            BestFitLine.Visible = false;

            // a cached legend formatter
            _legendFormatter = new StringFormatter(_legendMask);

            // by default we can use "0.0" for most values
            ValueFormatter = (t, v) => v.ToString("0.0");
        }

        public float Current()
        {
            if (DrawValues.Count > 0)
            {
                return DrawValues.First();
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// Returns the current max displayed value, based on the series Scale Mode
        /// </summary>
        /// <returns></returns>
        public float GetVerticalLimit(int visibleValues)
        {
            float maxValue = 0;
            switch (ScaleMode)
            {
                case SuperPerformanceChart.Series.ScaleModeEnum.Absolute:
                    maxValue = MaxValue;
                    break;

                case SuperPerformanceChart.Series.ScaleModeEnum.Percent:
                    maxValue = 100;
                    break;

                case SuperPerformanceChart.Series.ScaleModeEnum.RelativeVisble:

                    maxValue = VisibleMax(visibleValues);
                    break;

                case SuperPerformanceChart.Series.ScaleModeEnum.RelativeAll:
                    maxValue = Max();
                    break;
            }

            return maxValue;
        }

        public float Max()
        {
            if (DrawValues.Count > 0)
            {
                return DrawValues.Max();
            }
            else
            {
                return 0;
            }
        }

        public float VisibleMax(int visibleCount)
        {
            if (DrawValues.Count > 0)
            {
                return DrawValues.Take(visibleCount).Max();
            }
            else
            {
                return 0;
            }
        }

        public float Min()
        {
            if (DrawValues.Count > 0)
            {
                return DrawValues.Min();
            }
            else
            {
                return 0;
            }
        }

        public float VisibleMin(int visibleCount)
        {
            if (DrawValues.Count > 0)
            {
                return DrawValues.Take(visibleCount).Min();
            }
            else
            {
                return 0;
            }
        }

        public float Mean()
        {
            if (DrawValues.Count > 0)
            {
                return DrawValues.Sum() / DrawValues.Count;
            }
            else
            {
                return 0;
            }
        }

        public float VisibleMean(int visibleCount)
        {
            if (DrawValues.Count > 0)
            {
                var lst = DrawValues.Take(visibleCount);
                return lst.Sum() / lst.Count();
            }
            else
            {
                return 0;
            }
        }

        public void BestFit(ref float YIntercept, ref float Slope)
        {
            LeastSquaresLine(DrawValues, ref YIntercept, ref Slope);
        }

        public void BestFit(int iToIndex, ref float YIntercept, ref float Slope)
        {
            var lst = DrawValues.Take(iToIndex);
            LeastSquaresLine(lst.ToList(), ref YIntercept, ref Slope);
        }

        private void LeastSquaresLine(List<float> lstValues, ref float YIntercept, ref float Slope)
        {
            float SumX = default, SumXSquared = default, SumY = default, SumXY = default;
            float Determinant;
            for (int i = 0, loopTo = lstValues.Count - 1; i <= loopTo; i++)
            {
                SumX += i;
                SumXSquared += i * i;
                SumY += lstValues[i];
                SumXY += i * lstValues[i];
            }

            Determinant = lstValues.Count * SumXSquared - SumX * SumX;
            Slope = (lstValues.Count * SumXY - SumY * SumX) / Determinant;
            YIntercept = (SumY * SumXSquared - SumX * SumXY) / Determinant;
        }

        #region Delegates

        public ValueFormatter_Delegate ValueFormatter;

        public delegate string ValueFormatter_Delegate(ValueDisplayTypeEnum valueType, float value);

        #endregion Delegates

    }
}