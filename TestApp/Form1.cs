using System;
using System.Collections.Generic;
using System.Drawing;

namespace TestApp
{
    public partial class frmMain
    {
        private double _testValue;
        private double _test2Value;
        private int _adder2;

        private const double TwoPi = 2d * Math.PI;
        private const double CircleIncrement = Math.PI / 100d;

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

            chrt.AddSeries("Test", "Test", Color.LightSeaGreen);
            {
                chrt.Series["Test"].CacheSize = 200;
                chrt.Series["Test"].ZOrder = 0;
            }

            chrt.AddSeries("CPU", "CPU", Color.SteelBlue);
            {
                chrt.Series["CPU"].LegendMask = "@name - Current : @current | Mean : @mean | Mean.V : @vmean";
                chrt.Series["CPU"].ZOrder = 1;
            }

            chrt.AddSeries("Mem", "Mem", Color.DarkGoldenrod);
            {
                chrt.Series["Mem"].CacheSize = 100;
                chrt.Series["Mem"].LegendMask = "@name - Current : @current Mb | Mean : @mean Mb | Mean.V : @vmean  Mb";
                chrt.Series["Mem"].ValueFormatter = (SuperPerformanceChart.Series.ValueDisplayTypeEnum t, float v) => (v / 1024f / 1024f).ToString("0.0");
                chrt.Series["Mem"].ZOrder = 2;
            }

            chrt.AddSeries("Threads", "Threads", Color.ForestGreen);
            {
                chrt.Series["Threads"].CacheSize = 100;
                chrt.Series["Threads"].ValueFormatter = (SuperPerformanceChart.Series.ValueDisplayTypeEnum t, float v) => (v / 1024f / 1024f).ToString("0.0");
                chrt.Series["Threads"].ZOrder = 3;
            }

            chrt.ExpandSeriesCacheToFillScreen = true;

            chrt.BackgroundStyle.StartColor = Color.White;
            chrt.BackgroundStyle.EndColor = Color.White;

            chrt.VerticalGridLine.Visible = true;
        }

        private void tmrMain_Tick(object sender, EventArgs e)
        {
            spcClientDriven.AddValue("Test", (float)Math.Sin(_testValue));

            double increment = CircleIncrement;
            if (_testValue >= TwoPi)
            {
                increment = -increment;
            }
            _testValue += increment;

            spcClientDriven.AddValue("Test2", (float)_test2Value);
            if (_test2Value >= 30d)
            {
                _adder2 = -1;
            }
            else if (_test2Value <= 0d)
            {
                _adder2 = 1;
            }
            _test2Value += _adder2;

            spcClientDriven.AddValue("CPU", (float)((decimal)pcCPU.NextValue() / Environment.ProcessorCount));
            spcClientDriven.AddValue("Mem", (float)(decimal)pcMem.NextValue());
            spcClientDriven.AddValue("Threads", (float)(decimal)pcThreads.NextValue());

            lblMemSeriesLastValue.Text = $"Test : {_testValue}      Test 2 : {_test2Value}";
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