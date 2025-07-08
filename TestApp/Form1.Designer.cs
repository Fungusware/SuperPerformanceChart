using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace TestApp
{
    [Microsoft.VisualBasic.CompilerServices.DesignerGenerated()]
    public partial class frmMain : Form
    {

        // Form overrides dispose to clean up the component list.
        [DebuggerNonUserCode()]
        protected override void Dispose(bool disposing)
        {
            try
            {
                if (disposing && components is not null)
                {
                    components.Dispose();
                }
            }
            finally
            {
                base.Dispose(disposing);
            }
        }

        // Required by the Windows Form Designer
        private System.ComponentModel.IContainer components;

        // NOTE: The following procedure is required by the Windows Form Designer
        // It can be modified using the Windows Form Designer.  
        // Do not modify it using the code editor.
        [DebuggerStepThrough()]
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            Label1 = new Label();
            pcMem = new PerformanceCounter();
            pcCPU = new PerformanceCounter();
            tmrMain = new Timer(components);
            tmrMain.Tick += new EventHandler(tmrMain_Tick);
            btnMakeWOrk = new Button();
            btnMakeWOrk.Click += new EventHandler(btnMakeWOrk_Click);
            pcThreads = new PerformanceCounter();
            lblMemSeriesLastValue = new Label();
            pg1 = new PropertyGrid();
            spcClientDriven = new SuperPerformanceChart.SuperPerfChart();
            pcMem.BeginInit();
            pcCPU.BeginInit();
            pcThreads.BeginInit();
            SuspendLayout();
            // 
            // Label1
            // 
            Label1.AutoSize = true;
            Label1.Location = new Point(12, 9);
            Label1.Name = "Label1";
            Label1.Size = new Size(83, 16);
            Label1.TabIndex = 125;
            Label1.Text = "Client Driven";
            // 
            // pcMem
            // 
            pcMem.CategoryName = "Process";
            pcMem.CounterName = "Private Bytes";
            pcMem.InstanceName = "testapp";
            // 
            // pcCPU
            // 
            pcCPU.CategoryName = "Process";
            pcCPU.CounterName = "% Processor Time";
            pcCPU.InstanceName = "testapp";
            // 
            // tmrMain
            // 
            tmrMain.Enabled = true;
            // 
            // btnMakeWOrk
            // 
            btnMakeWOrk.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnMakeWOrk.Location = new Point(769, 528);
            btnMakeWOrk.Name = "btnMakeWOrk";
            btnMakeWOrk.Size = new Size(276, 23);
            btnMakeWOrk.TabIndex = 130;
            btnMakeWOrk.Text = "Do Some Work";
            btnMakeWOrk.UseVisualStyleBackColor = true;
            // 
            // pcThreads
            // 
            pcThreads.CategoryName = "Process";
            pcThreads.CounterName = "Thread Count";
            pcThreads.InstanceName = "testapp";
            // 
            // lblMemSeriesLastValue
            // 
            lblMemSeriesLastValue.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            lblMemSeriesLastValue.AutoSize = true;
            lblMemSeriesLastValue.Location = new Point(12, 612);
            lblMemSeriesLastValue.Name = "lblMemSeriesLastValue";
            lblMemSeriesLastValue.Size = new Size(26, 16);
            lblMemSeriesLastValue.TabIndex = 131;
            lblMemSeriesLastValue.Text = "val";
            // 
            // pg1
            // 
            pg1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right;
            pg1.HelpVisible = false;
            pg1.Location = new Point(769, 27);
            pg1.Name = "pg1";
            pg1.SelectedObject = spcClientDriven;
            pg1.Size = new Size(276, 495);
            pg1.TabIndex = 129;
            pg1.ToolbarVisible = false;
            // 
            // spcClientDriven
            // 
            spcClientDriven.ExpandSeriesCacheToFillScreen = false;
            spcClientDriven.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;

            spcClientDriven.AntiAliasing = true;
            spcClientDriven.Location = new Point(15, 27);
            spcClientDriven.Name = "spcClientDriven";
            spcClientDriven.Progress = 0;
            spcClientDriven.Size = new Size(748, 566);
            spcClientDriven.TabIndex = 126;
            spcClientDriven.TimerInterval = 100;
            spcClientDriven.TimerMode = SuperPerformanceChart.TimerMode.Disabled;
            spcClientDriven.UnifiedVerticalScale = false;
            spcClientDriven.ValueSpacing = 5;
            // 
            // frmMain
            // 
            AutoScaleDimensions = new SizeF(8.0f, 16.0f);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1057, 637);
            Controls.Add(lblMemSeriesLastValue);
            Controls.Add(btnMakeWOrk);
            Controls.Add(pg1);
            Controls.Add(spcClientDriven);
            Controls.Add(Label1);
            Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Regular, GraphicsUnit.Point, 0);
            Margin = new Padding(4, 3, 4, 3);
            Name = "frmMain";
            Text = "PerfChart Tester";
            pcMem.EndInit();
            pcCPU.EndInit();
            pcThreads.EndInit();
            Load += new EventHandler(frmMain_Load);
            ResumeLayout(false);
            PerformLayout();

        }
        internal Label Label1;
        internal SuperPerformanceChart.SuperPerfChart spcClientDriven;
        internal PerformanceCounter pcMem;
        internal PerformanceCounter pcCPU;
        internal Timer tmrMain;
        internal PropertyGrid pg1;
        internal Button btnMakeWOrk;
        internal PerformanceCounter pcThreads;
        internal Label lblMemSeriesLastValue;
    }
}