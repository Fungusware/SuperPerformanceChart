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
            btnMakeWOrk = new Button();
            pcThreads = new PerformanceCounter();
            lblMemSeriesLastValue = new Label();
            pg1 = new PropertyGrid();
            spcClientDriven = new SuperPerformanceChart.SuperPerfChart();
            tabs = new TabControl();
            tabPage1 = new TabPage();
            (pcMem).BeginInit();
            (pcCPU).BeginInit();
            (pcThreads).BeginInit();
            tabs.SuspendLayout();
            tabPage1.SuspendLayout();
            SuspendLayout();
            // 
            // Label1
            // 
            Label1.AutoSize = true;
            Label1.Location = new Point(12, 9);
            Label1.Name = "Label1";
            Label1.Size = new Size(82, 16);
            Label1.TabIndex = 125;
            Label1.Text = "Client Driven";
            // 
            // pcMem
            // 
            pcMem.CategoryName = "Process";
            pcMem.CounterName = "Private Bytes";
            pcMem.InstanceLifetime = PerformanceCounterInstanceLifetime.Global;
            pcMem.InstanceName = "testapp";
            pcMem.MachineName = ".";
            pcMem.ReadOnly = true;
            // 
            // pcCPU
            // 
            pcCPU.CategoryName = "Process";
            pcCPU.CounterName = "% Processor Time";
            pcCPU.InstanceLifetime = PerformanceCounterInstanceLifetime.Global;
            pcCPU.InstanceName = "testapp";
            pcCPU.MachineName = ".";
            pcCPU.ReadOnly = true;
            // 
            // tmrMain
            // 
            tmrMain.Enabled = true;
            tmrMain.Tick += tmrMain_Tick;
            // 
            // btnMakeWOrk
            // 
            btnMakeWOrk.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnMakeWOrk.Location = new Point(762, 609);
            btnMakeWOrk.Name = "btnMakeWOrk";
            btnMakeWOrk.Size = new Size(276, 23);
            btnMakeWOrk.TabIndex = 130;
            btnMakeWOrk.Text = "Do Some Work";
            btnMakeWOrk.UseVisualStyleBackColor = true;
            btnMakeWOrk.Click += btnMakeWOrk_Click;
            // 
            // pcThreads
            // 
            pcThreads.CategoryName = "Process";
            pcThreads.CounterName = "Thread Count";
            pcThreads.InstanceLifetime = PerformanceCounterInstanceLifetime.Global;
            pcThreads.InstanceName = "testapp";
            pcThreads.MachineName = ".";
            pcThreads.ReadOnly = true;
            // 
            // lblMemSeriesLastValue
            // 
            lblMemSeriesLastValue.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            lblMemSeriesLastValue.AutoSize = true;
            lblMemSeriesLastValue.Location = new Point(12, 612);
            lblMemSeriesLastValue.Name = "lblMemSeriesLastValue";
            lblMemSeriesLastValue.Size = new Size(25, 16);
            lblMemSeriesLastValue.TabIndex = 131;
            lblMemSeriesLastValue.Text = "val";
            // 
            // pg1
            // 
            pg1.Dock = DockStyle.Fill;
            pg1.HelpVisible = false;
            pg1.Location = new Point(3, 3);
            pg1.Name = "pg1";
            pg1.SelectedObject = spcClientDriven;
            pg1.Size = new Size(402, 559);
            pg1.TabIndex = 129;
            pg1.ToolbarVisible = false;
            // 
            // spcClientDriven
            // 
            spcClientDriven.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            spcClientDriven.AntiAliasing = true;
            spcClientDriven.DisplayTextLineSpacing = 0F;
            spcClientDriven.ExpandSeriesCacheToFillScreen = false;
            spcClientDriven.Location = new Point(15, 27);
            spcClientDriven.Margin = new Padding(4, 3, 4, 3);
            spcClientDriven.MaximumValueScale = 0.95F;
            spcClientDriven.Name = "spcClientDriven";
            spcClientDriven.Progress = 0;
            spcClientDriven.Size = new Size(607, 566);
            spcClientDriven.TabIndex = 126;
            spcClientDriven.TimerInterval = 100;
            spcClientDriven.TimerMode = SuperPerformanceChart.TimerMode.Disabled;
            spcClientDriven.UnifiedVerticalScale = false;
            spcClientDriven.UseBitmapedGrid = true;
            spcClientDriven.ValueSpacing = 5;
            // 
            // tabs
            // 
            tabs.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right;
            tabs.Controls.Add(tabPage1);
            tabs.Location = new Point(629, 9);
            tabs.Name = "tabs";
            tabs.SelectedIndex = 0;
            tabs.Size = new Size(416, 594);
            tabs.TabIndex = 132;
            // 
            // tabPage1
            // 
            tabPage1.Controls.Add(pg1);
            tabPage1.Location = new Point(4, 25);
            tabPage1.Name = "tabPage1";
            tabPage1.Padding = new Padding(3);
            tabPage1.Size = new Size(408, 565);
            tabPage1.TabIndex = 0;
            tabPage1.Text = "Plot";
            tabPage1.UseVisualStyleBackColor = true;
            // 
            // frmMain
            // 
            AutoScaleDimensions = new SizeF(8F, 16F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1057, 637);
            Controls.Add(tabs);
            Controls.Add(lblMemSeriesLastValue);
            Controls.Add(btnMakeWOrk);
            Controls.Add(spcClientDriven);
            Controls.Add(Label1);
            Font = new Font("Microsoft Sans Serif", 9.75F, FontStyle.Regular, GraphicsUnit.Point);
            Margin = new Padding(4, 3, 4, 3);
            Name = "frmMain";
            Text = "PerfChart Tester";
            Load += frmMain_Load;
            (pcMem).EndInit();
            (pcCPU).EndInit();
            (pcThreads).EndInit();
            tabs.ResumeLayout(false);
            tabPage1.ResumeLayout(false);
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
        private TabControl tabs;
        private TabPage tabPage1;
    }
}