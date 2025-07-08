using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;
using static SuperPerformanceChart.Series;

namespace SuperPerformanceChart
{
    public partial class SuperPerfChart : UserControl
    {
        // Define the things that never change
        private const int GRID_SPACING = 16;
        private const string NAME = "@name";
        private const string CURRENT = "@current";
        private const string MIN = "@min";
        private const string VISIBLEMIN = "@vmin";
        private const string MAX = "@max";
        private const string VISIBLEMAX = "@vmax";
        private const string MEAN = "@mean";
        private const string VISIBLEMEAN = "@vmean";

        // Timer Mode
        private TimerMode _timerMode;

        // The currently highest displayed value, required for Relative Scale Mode
        private float _currentMaxValue = 0;

        // Offset value for the scrolling grid
        private int _gridScrollOffset = 0;

        // Progress
        private int _progress;

        private Point _origin;

        public SuperPerfChart()
        {
            InitializeComponent();

            // Initialize Variables
            Series = new Dictionary<string, Series>();
            VerticalGridLine = new LineStyle(Color.WhiteSmoke);
            VerticalGridLine.Visible = true;
            VerticalGridLine.DashStyle = DashStyle.Dot;
            HorizontalGridLine = new LineStyle(Color.WhiteSmoke);
            HorizontalGridLine.DashStyle = DashStyle.Dot;
            BackgroundStyle = new FillStyle(Color.White);
            BackgroundStyle.Alpha = 255;
            BackgroundStyle.GradientAngle = 90;
            ProgressStyle = new ProgressStyle();
            ProgressStyle.Visible = false;
            ProgressStyle.Fill.GradientAngle = 0;

            //Set Optimized Double Buffer to reduce flickering
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);

            // Redraw on Resize
            SetStyle(ControlStyles.ResizeRedraw, true);

            AntiAliasing = true;
            UnifiedVerticalScale = true;
            DoubleBuffered = true;

            // Redraw when resized
            SetStyle(ControlStyles.ResizeRedraw, true);
        }

        #region Properties

        [Category("Chart Options")]
        public bool UseBitmapedGrid { get; set; } = true;

        [Category("Chart Options")]
        public bool ExpandSeriesCacheToFillScreen { get; set; } = false;

        [Category("Chart Options")]
        public bool UnifiedVerticalScale { get; set; } = true;

        [Category("Chart Options")]
        public TimerMode TimerMode
        {
            get => _timerMode;
            set
            {
                if (value == TimerMode.Disabled)
                {
                    // Stop and append only when changed
                    if (_timerMode != TimerMode.Disabled)
                    {
                        _timerMode = value;
                        tmrRefresh.Stop();
                        // If there are any values in the queue, append them
                        ChartAppendFromQueue();
                    }
                }
                else
                {
                    _timerMode = value;
                    tmrRefresh.Start();
                }
            }
        }

        [Category("Chart Options")]
        public int TimerInterval { get => tmrRefresh.Interval; set => tmrRefresh.Interval = Math.Max(10, value); }

        [Category("Progress Options")]
        public ProgressStyle ProgressStyle { get; }

        [Category("Progress Options")]
        public int Progress
        {
            get => _progress;
            set
            {
                _progress = value;
                if (_progress < 0)
                    _progress = 0;
                if (_progress > 100)
                    _progress = 100;
            }
        }

        // Horizontal value space in Pixels
        [Category("Appearance")]
        public int ValueSpacing { get; set; } = 5;

        [Category("Appearance")]
        public float DisplayTextLineSpacing { get; set; } = 0.0F;

        [Category("Appearance")]
        public float MaximumValueScale { get; set; } = 0.95f;

        [Category("Appearance")]
        public FillStyle BackgroundStyle { get; }

        [Category("Appearance")]
        public LineStyle VerticalGridLine { get; }

        [Category("Appearance")]
        public LineStyle HorizontalGridLine { get; }

        [Category("Appearance")]
        public bool AntiAliasing { get; set; }

        [Category("Data")]
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Dictionary<string, Series> Series { get; }

        [Category("Data")]
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public List<Series> SeriesList { get => Series.Values.ToList(); }

        //public int OffsetY { get; set; }
        //public int ModHeightGridSpacing {  get => this.Height % GRID_SPACING; }

        #endregion Properties

        #region Drawing Methods

        /// <summary>
        /// Draws the chart (w/o background or grid, but with border) to the Graphics canvas
        /// </summary>
        /// <param name="g">Graphics</param>
        private void DrawChart(Graphics g)
        {
            float fOffset = 2.0F;

            try
            {
                if (UnifiedVerticalScale)
                    _currentMaxValue = GetMaxValueForAllSeries();

                // Draw in reverse ZOrder
                foreach (Series ser in Series.Values.
                        Where(s => s.DrawValues.Count > 0 && s.Visible).
                        OrderByDescending(s => s.ZOrder))
                {
                    DrawSeriesData(ser, g, ref fOffset);
                }

                // Draw Percentage
                if (ProgressStyle.Visible)
                {
                    if (ProgressStyle.DrawOnTop)
                    {
                        DrawPercentageBar(g);
                    }
                    DrawPercentageText(g);
                }
            }
            catch (Exception ex)
            {
                using (var br = new SolidBrush(Color.Red))
                {
                    g.DrawString(ex.Message, new Font("Arial", 8), br, 4.0F, 2);
                }
            }
        }

        private void DrawSeriesData(Series series, Graphics g, ref float fOffset)
        {
            // how many values to show
            var visibleValues = GetVisibleValueCount(series);

            // Relative values
            if (!UnifiedVerticalScale && series.ScaleMode != SuperPerformanceChart.Series.ScaleModeEnum.Percent)
            {
                _currentMaxValue = series.GetVerticalLimit(visibleValues);
            }

            // our actual points have 4 extra points used to close the loop
            var points = new Point[visibleValues + 4];

            // Data
            // Most recent value is at [0] ...... [Visible Size] ...... [Cache Size]

            // Connect all visible values with lines
            for (int i = 0; i < visibleValues; i++)
            {
                points[i].X = _origin.X - (i * ValueSpacing);
                points[i].Y = CalcVerticalPosition(series.ScaleMode, series.DrawValues[i]);
            }

            // Across to the X minimum -- less a margin so we dont see it
            points[visibleValues] = new Point(-2, points[visibleValues - 1].Y);

            // X Min to Y Origin
            points[visibleValues + 1] = new Point(-2, _origin.Y);

            // back to origin
            points[visibleValues + 2] = _origin;

            //// The origin to the first point to close the path
            points[visibleValues + 3] = points[0];

            // Now we have all points for a polygon
            if (series.Line.Visible)
            {
                if (series.Smooth)
                {
                    g.DrawCurve(series.Line.Pen, points, series.Tension);
                }
                else
                {
                    g.DrawLines(series.Line.Pen, points);
                }
            }

            // points
            if (series.Point.Visible)
            {
                foreach (var point in points)
                {
                    var halfPointSize = (series.Point.Size - 1) / 2;

                    g.FillEllipse(series.Point.Brush, point.X - halfPointSize, point.Y - halfPointSize, series.Point.Size, series.Point.Size);
                }
            }

            // do we want to fill the area under the curve ?
            if (series.Fill.Visible)
            {
                // Continue finishing the poly

                // create the fill brush
                Brush brFill;
                if (series.Fill.HasGradient)
                {
                    brFill = new LinearGradientBrush(new Rectangle(0, 0, Width, Height), series.Fill.ActualStartColor, series.Fill.ActualEndColor, series.Fill.GradientAngle);
                }
                else
                {
                    brFill = new SolidBrush(series.Fill.ActualStartColor);
                }

                if (series.Smooth)
                {
                    g.FillClosedCurve(brFill, points, FillMode.Winding, series.Tension);
                }
                else
                {
                    g.FillPolygon(brFill, points);
                }

                brFill.Dispose();
            }

            // Trends
            if (visibleValues > 0)
            {
                if (series.AverageLine.Visible)
                    DrawAverageLine(g, series, visibleValues);
                if (series.BestFitLine.Visible)
                    DrawBestFitLine(g, series, visibleValues);
                if (series.MaxLine.Visible)
                    DrawMaxLine(g, series, visibleValues);
                if (series.MinLine.Visible)
                    DrawMinLine(g, series, visibleValues);
            }

            // Labels
            if (series.LegendVisible)
            {
                string label = GetValueDisplayText(series, visibleValues);
                if (!string.IsNullOrEmpty(label))
                {
                    g.DrawString(label, series.Text.Font, series.Text.Brush, 4, fOffset);
                    fOffset += g.MeasureString(label, series.Text.Font).Height + DisplayTextLineSpacing;
                }
            }
        }

        private string GetValueDisplayText(Series series, int visibleValues)
        {
            //string label = "";

            if (string.IsNullOrEmpty(series.LegendMask)) return string.Empty;

            var legendFormatter = series.LegendFormatter;

            // only calculate what the legend text has requested to save processing
            if (legendFormatter.HasParameter(NAME)) legendFormatter.Set(NAME, series.Name);
            if (legendFormatter.HasParameter(CURRENT)) legendFormatter.Set(CURRENT, series.ValueFormatter(ValueDisplayTypeEnum.Current, series.Current()));

            if (legendFormatter.HasParameter(MIN)) legendFormatter.Set(MIN, series.ValueFormatter(ValueDisplayTypeEnum.Min, series.Min()));
            if (legendFormatter.HasParameter(VISIBLEMIN)) legendFormatter.Set(VISIBLEMIN, series.ValueFormatter(ValueDisplayTypeEnum.VisibleMin, series.VisibleMin(visibleValues)));

            if (legendFormatter.HasParameter(MAX)) legendFormatter.Set(MAX, series.ValueFormatter(ValueDisplayTypeEnum.Max, series.Max()));
            if (legendFormatter.HasParameter(VISIBLEMAX)) legendFormatter.Set(VISIBLEMAX, series.ValueFormatter(ValueDisplayTypeEnum.VisibleMax, series.VisibleMax(visibleValues)));

            if (legendFormatter.HasParameter(MEAN)) legendFormatter.Set(MEAN, series.ValueFormatter(ValueDisplayTypeEnum.Mean, series.Mean()));
            if (legendFormatter.HasParameter(VISIBLEMEAN)) legendFormatter.Set(VISIBLEMEAN, series.ValueFormatter(ValueDisplayTypeEnum.VisibleMean, series.VisibleMean(visibleValues)));

            return legendFormatter.ToString();
        }

        /// <summary>
        /// Draws the Percentage on th canvas
        /// </summary>
        /// <param name="g"></param>
        /// <remarks></remarks>
        private void DrawPercentageBar(Graphics g)
        {
            if (ProgressStyle.Visible)
            {
                float fYOffset = Height * (1 - ProgressStyle.HeightFraction);

                // Bar
                if (_progress > 0)
                {
                    int iXPerc = (int)(Width * (_progress / (double)100));

                    if (ProgressStyle.Fill.Visible)
                    {
                        var progRectangle = new Rectangle(0, (int)(fYOffset), iXPerc, Height);
                        using (Brush progressBrush = ProgressStyle.Fill.GetFillBrush(progRectangle))
                        {
                            g.FillRectangle(progressBrush, progRectangle);
                        }
                    }
                    using (var pn = ProgressStyle.Fill.GetLinePen())
                    {
                        g.DrawLine(pn, iXPerc - 1, fYOffset, iXPerc - 1, Height);
                    }
                }
            }
        }

        private void DrawPercentageText(Graphics g)
        {
            float fYOffset = Height * (1 - ProgressStyle.HeightFraction);

            // Text
            string sPerc = _progress + "%";
            var szTextWidth = g.MeasureString(sPerc, Font);
            float fBarHeight = Height * ProgressStyle.HeightFraction;
            float fYTextMidPoint = fBarHeight / 2 + fYOffset;
            float fY = fYTextMidPoint - szTextWidth.Height / 2;
            float fX = (float)(Width * (_progress / (double)100)) + 2;

            // check the X co-ord is not off the left of the screen
            fX = Math.Min(Width - szTextWidth.Width, fX);
            g.DrawString(sPerc, ProgressStyle.Text.Font, ProgressStyle.Text.Brush, fX, fY);
        }

        /// <summary>
        /// Draws the background gradient and the grid into Graphics <paramref name="g"/>
        /// </summary>
        /// <param name="g">Graphic</param>
        private void DrawBackgroundAndGrid(Graphics g)
        {
            // Gets a reference to the current BufferedGraphicsContext
            var currentContext = BufferedGraphicsManager.Current;
            // Creates a BufferedGraphics instance. Drawing to the buffer, is 2x the speed of doing it directly
            // as we do not need to enable Antialiasing on the background drawing surface.
            var buffer = currentContext.Allocate(g, this.DisplayRectangle);

            // Draw the background Gradient rectangle
            using (var oBrush = BackgroundStyle.GetFillBrush(g.VisibleClipBounds))
            {
                buffer.Graphics.FillRectangle(oBrush, g.VisibleClipBounds);
            }

            if (UseBitmapedGrid)
            {
                //if (_cachedGridTile == null)
                using (var cachedGridTile = GenerateGridTile())
                {
                    using (var brush = new TextureBrush(cachedGridTile))
                    {
                        buffer.Graphics.FillRectangle(brush, g.VisibleClipBounds);
                    }
                }
            }
            else
            {
                //Draw all visible, vertical gridlines(if wanted)
                if (VerticalGridLine.Visible)
                {
                    int i = 0;
                    while (i < Width)
                    {
                        buffer.Graphics.DrawLine(VerticalGridLine.Pen, i, 0, i, Height);
                        i += GRID_SPACING;
                    }
                }

                // Draw all visible, horizontal gridlines (if wanted)
                if (HorizontalGridLine.Visible)
                {
                    int i = Height;
                    while (i > 0)
                    {
                        buffer.Graphics.DrawLine(HorizontalGridLine.Pen, 0, i, Width, i);
                        i -= GRID_SPACING;
                    }
                }
            }

            // Percentage
            if (!ProgressStyle.DrawOnTop)
                DrawPercentageBar(buffer.Graphics);

            buffer.Render();
        }

        private Bitmap GenerateGridTile()
        {
            var b = new Bitmap(GRID_SPACING, GRID_SPACING);

            using (Graphics gfx = Graphics.FromImage(b))
            {
                gfx.Clear(Color.Magenta);

                // offset to start at bottom of screen
                //var offsetY = this.Height % GRID_SPACING;

                if (HorizontalGridLine.Visible)
                    gfx.DrawLine(HorizontalGridLine.Pen, 0, GRID_SPACING - 1, b.Width, GRID_SPACING - 1);

                if (VerticalGridLine.Visible)
                    gfx.DrawLine(VerticalGridLine.Pen, GRID_SPACING - 1, 0, GRID_SPACING - 1, b.Height);

                b.MakeTransparent(Color.Magenta);
            }

            return b;
        }

        #endregion Drawing Methods

        #region Trend Lines

        private void DrawConstLine(Graphics g, Series.ScaleModeEnum scaleMode, LineStyle lineStyle, int visibleValues, float value)
        {
            int verticalPosition = CalcVerticalPosition(scaleMode, value);
            int x = _origin.X - (visibleValues * ValueSpacing);
            g.DrawLine(lineStyle.Pen, x, verticalPosition, _origin.X, verticalPosition);
        }

        private void DrawMinLine(Graphics g, Series series, int visibleValues)
        {
            DrawConstLine(g, series.ScaleMode, series.MinLine, visibleValues, series.VisibleMax(visibleValues));
        }

        private void DrawMaxLine(Graphics g, Series series, int visibleValues)
        {
            DrawConstLine(g, series.ScaleMode, series.MaxLine, visibleValues, series.VisibleMin(visibleValues));
        }

        private void DrawAverageLine(Graphics g, Series series, int visibleValues)
        {
            DrawConstLine(g, series.ScaleMode, series.AverageLine, visibleValues, series.VisibleMean(visibleValues));
        }

        private void DrawBestFitLine(Graphics g, Series series, int visibleValues)
        {
            var dYintercept = 0f;
            var dSlope = 0f;
            series.BestFit(visibleValues, ref dYintercept, ref dSlope);
            int dY1 = CalcVerticalPosition(series.ScaleMode, dYintercept);
            int dY2 = CalcVerticalPosition(series.ScaleMode, dYintercept + visibleValues * -dSlope);
            int x = _origin.X - (visibleValues * ValueSpacing);
            g.DrawLine(series.BestFitLine.Pen, x, dY1, _origin.X, dY2);
        }

        #endregion Trend Lines

        #region Overrrides

        /// <summary>
        /// Override OnPaint method
        /// </summary>
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            // +1 on Height means we dont see the Bottom horzontal Line of the curve
            _origin = new Point(Width + 1, Height + 2);

            // Enable AntiAliasing, if needed
            e.Graphics.SmoothingMode = AntiAliasing ? SmoothingMode.AntiAlias : SmoothingMode.None;
            e.Graphics.TextRenderingHint = AntiAliasing ? System.Drawing.Text.TextRenderingHint.AntiAlias : System.Drawing.Text.TextRenderingHint.SystemDefault;

            // Draw stuff
            DrawBackgroundAndGrid(e.Graphics);
            DrawChart(e.Graphics);
        }

        protected override void OnResize(EventArgs e)
        {
            // MyBase.OnResize(e)

            // ' Cause the background to be cleared and redraw.
            // Invalidate()
        }

        #endregion Overrrides

        #region Event Handlers

        private void colorSet_ColorSetChanged(object sender, EventArgs e)
        {
            Invalidate();
        }

        private void tmrRefresh_Tick(object sender, EventArgs e)
        {
            // Don't execute event if running in design time
            if (DesignMode)
            {
                return;
            }

            ChartAppendFromQueue();
        }

        //private void SuperPerfChart_SizeChanged(object sender, EventArgs e)
        //{
        //    Invalidate();
        //}

        #endregion Event Handlers

        #region Misc

        private int GetVisibleValueCount(Series ser)
        {
            return (int)Math.Min(Width / (double)ValueSpacing, ser.DrawValues.Count);
        }

        /// <summary>
        /// Clears the whole chart
        /// </summary>
        public void Clear()
        {
            foreach (var oKVP in Series)
                oKVP.Value.DrawValues.Clear();
            Invalidate();
        }

        /// <summary>
        /// Adds a value to the Chart Line
        /// </summary>
        /// <param name="value">progress value</param>
        public void AddValue(string series, float value)
        {
            Series oSer = null;
            if (Series.TryGetValue(series, out oSer))
            {
                switch (_timerMode)
                {
                    case TimerMode.Disabled:
                        ChartAppend(oSer, value);
                        Invalidate();
                        break;

                    case TimerMode.Simple:
                    case TimerMode.SynchronizedAverage:
                    case TimerMode.SynchronizedSum:
                        // For all Timer based modes, the Values are stored in the Queue
                        oSer.QueuedValues.Enqueue(value);
                        break;

                    default:
                        throw new Exception(string.Format("Unsupported TimerMode: {0}", _timerMode));
                }
            }
        }

        public Series AddSeries(string sKey, string sName, Color baseColor)
        {
            Series oSer = null;
            if (!Series.ContainsKey(sKey))
            {
                oSer = new Series(sKey, sName, baseColor);
                Series.Add(sKey, oSer);
            }

            return oSer;
        }

        /// <summary>
        /// Appends value <paramref name="value"/> to the chart (without redrawing)
        /// </summary>
        /// <param name="value">performance value</param>
        private void ChartAppend(Series oSer, float value)
        {
            // check values
            if (oSer.ScaleMode == SuperPerformanceChart.Series.ScaleModeEnum.Percent && value > 100f)
            {
                value = 100;
            }

            // Insert at first position; Negative values are flatten to 0 (zero)
            oSer.DrawValues.Insert(0, Math.Max(value, 0));

            // Remove last item if maximum value count is reached
            var cacheSize = GetVisibleValueCount(oSer);
            if (ExpandSeriesCacheToFillScreen)
                cacheSize = Math.Max(cacheSize, oSer.CacheSize);
            else
                cacheSize = Math.Min(cacheSize, oSer.CacheSize);

            if (oSer.DrawValues.Count > cacheSize)
            {
                oSer.DrawValues.RemoveRange(cacheSize, oSer.DrawValues.Count - cacheSize);
            }

            // Calculate horizontal grid offset for "scrolling" effect
            _gridScrollOffset += ValueSpacing;
            if (_gridScrollOffset > GRID_SPACING)
            {
                _gridScrollOffset %= GRID_SPACING;
            }
        }

        /// <summary>
        /// Appends Values from queue
        /// </summary>
        private void ChartAppendFromQueue()
        {
            foreach (var oSer in Series.Values)
            {
                // Proceed only if there are values at all
                if (oSer.QueuedValues.Count > 0)
                {
                    if (_timerMode == TimerMode.Simple)
                    {
                        while (oSer.QueuedValues.Count > 0)
                            ChartAppend(oSer, oSer.QueuedValues.Dequeue());
                    }
                    else if (_timerMode == TimerMode.SynchronizedAverage || _timerMode == TimerMode.SynchronizedSum)
                    {
                        // appendValue variable is used for calculating the average or sum value
                        float appendValue = 0f;
                        int valueCount = oSer.QueuedValues.Count;
                        while (oSer.QueuedValues.Count > 0)
                            appendValue += oSer.QueuedValues.Dequeue();

                        // Calculate Average value in SynchronizedAverage Mode
                        if (_timerMode == TimerMode.SynchronizedAverage)
                        {
                            appendValue = appendValue / valueCount;
                        }

                        // Finally append the value
                        ChartAppend(oSer, appendValue);
                    }
                }
                else
                {
                    // Always add 0 (Zero) if there are no values in the queue
                    ChartAppend(oSer, 0f);
                }
            }

            // Refresh the Chart
            Invalidate();
        }

        /// <summary>
        /// Calculates the vertical Position of a value in relation the chart size,
        /// Scale Mode and, if ScaleMode is Relative, to the current maximum value
        /// </summary>
        /// <param name="value">performance value</param>
        /// <returns>vertical Point position in Pixels</returns>
        private int CalcVerticalPosition(Series.ScaleModeEnum ScaleMode, float value)
        {
            int result = 0;

            // little margin at the top
            value *= MaximumValueScale;

            switch (ScaleMode)
            {
                case SuperPerformanceChart.Series.ScaleModeEnum.Percent:
                    value = value * (Height - 2) / 100;
                    break;

                default:
                    value = _currentMaxValue > 0 ? value * (Height - 2) / _currentMaxValue : 0;
                    break;
            }

            result = (int)Math.Round(Height - value);

            return result;
        }

        /// <summary>
        /// Returns the current max value, across all visible series
        /// </summary>
        /// <returns></returns>
        private float GetMaxValueForAllSeries()
        {
            float maxValue = 0;
            foreach (var oSer in Series.Values)
            {
                if (oSer.Visible)
                {
                    var visibleValues = GetVisibleValueCount(oSer);
                    var seriesMaxValue = oSer.GetVerticalLimit(visibleValues);
                    if (seriesMaxValue > maxValue)
                    {
                        maxValue = seriesMaxValue;
                    }
                }
            }

            return maxValue;
        }

        #endregion Misc
    }
}