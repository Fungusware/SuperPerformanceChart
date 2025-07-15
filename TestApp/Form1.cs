using SuperPerformanceChart;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace TestApp
{
    public partial class frmMain
    {
        private double _testValue;
        private double _test2Value;

        private const double TWOPI = 2d * Math.PI;
        private const double CIRCLEINCREMENT = Math.PI / 100d;

        public frmMain()
        {
            InitializeComponent();
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            SetupChart(spcClientDriven);
        }

        public void SetupChart(SuperPerformanceChart.SuperPerfChart chrt)
        {
            chrt.Series.Clear();

            var testSeries = chrt.AddSeries("Test", "Test", Color.LightSeaGreen);
            testSeries.Fill.Visible = true;
            testSeries.CacheSize = 200;
            testSeries.ValueFormatter = (SuperPerformanceChart.Series.ValueDisplayTypeEnum t, float v) => $"{v:0.00}";
            testSeries.ZOrder = 0;
            AddAsTabPage(testSeries);

            var testSeries2 = chrt.AddSeries("Test2", "Test2", Color.DarkSeaGreen);
            testSeries2.Fill.Visible = true;
            testSeries2.CacheSize = 200;
            testSeries2.ValueFormatter = (SuperPerformanceChart.Series.ValueDisplayTypeEnum t, float v) => $"{v:0.00}";
            testSeries2.ZOrder = 0;
            AddAsTabPage(testSeries2);

            var cpuSeries = chrt.AddSeries("CPU", "CPU", Color.SteelBlue);
            cpuSeries.LegendMask = "@name - Current : @current | Mean : @mean | Mean.V : @vmean | Max : @max | Max.V : @vmax | Min : @min | Min.V : @vmin";
            cpuSeries.ZOrder = 1;
            AddAsTabPage(cpuSeries);

            var memSeries = chrt.AddSeries("Mem", "Mem", Color.DarkGoldenrod);
            memSeries.CacheSize = 100;
            memSeries.LegendMask = "@name - Current : @current Mb | Mean : @mean Mb | Mean.V : @vmean  Mb";
            memSeries.ValueFormatter = (SuperPerformanceChart.Series.ValueDisplayTypeEnum t, float v) => (v / 1024f / 1024f).ToString("0.0");
            memSeries.ZOrder = 2;
            AddAsTabPage(memSeries);

            var threadSeries = chrt.AddSeries("Threads", "Threads", Color.ForestGreen);
            threadSeries.CacheSize = 100;
            threadSeries.ValueFormatter = (SuperPerformanceChart.Series.ValueDisplayTypeEnum t, float v) => (v / 1024f / 1024f).ToString("0.0");
            threadSeries.ZOrder = 3;
            AddAsTabPage(threadSeries);

            chrt.ExpandSeriesCacheToFillScreen = true;
            chrt.BackgroundStyle.StartColor = Color.White;
            chrt.BackgroundStyle.EndColor = Color.White;
            chrt.VerticalGridLine.Visible = true;
        }

        private void AddAsTabPage(Series theSeries)
        {
            // property grid for the series
            var pg = new PropertyGrid();
            pg.SelectedObject = theSeries;

            // create a new tab page with the series name and add the property grid to it
            var tabPage = new TabPage(theSeries.Name) { Tag = theSeries };
            tabPage.Controls.Add(pg);
            pg.Dock = DockStyle.Fill;
            tabs.Controls.Add(tabPage);
        }

        private void tmrMain_Tick(object sender, EventArgs e)
        {
            spcClientDriven.AddValue("Test", (float)Math.Sin(_testValue));
            _testValue += CIRCLEINCREMENT;
             if (_testValue >= TWOPI)
            {
                _testValue = 0;
            }

            spcClientDriven.AddValue("Test2", (float)Math.Cos(_test2Value));
            _test2Value += CIRCLEINCREMENT;
            if (_test2Value >= TWOPI)
            {
                _test2Value = 0;
            }

            spcClientDriven.AddValue("CPU", (float)((decimal)pcCPU.NextValue() / Environment.ProcessorCount));
            spcClientDriven.AddValue("Mem", (float)(decimal)pcMem.NextValue());
            spcClientDriven.AddValue("Threads", (float)(decimal)pcThreads.NextValue());

            //lblMemSeriesLastValue.Text = $"Test : {_testValue}      Test 2 : {_test2Value}";
        }

        private void btnMakeWOrk_Click(object sender, EventArgs e)
        {
            System.Threading.ThreadPool.QueueUserWorkItem(o =>
                {
                    var lst = new List<double>();

                    for (int i = 0; i <= 10000000; i++)
                    {
                        double d = Math.Log(i);

                        lst.Add(d);
                    }
                });
        }

        // Private Sub chkLocalProc_CheckedChanged(sender As Object, e As EventArgs)
        // pcCPU.InstanceName = If(chkLocalProc.Checked, "testapp", "")
        // pcMem.InstanceName = If(chkLocalProc.Checked, "testapp", "")
        // pcThreads.InstanceName = If(chkLocalProc.Checked, "testapp", "")
        // End Sub
    }
}