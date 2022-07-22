using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace SuperPerformanceChart
{
    public partial class SuperPerfChart : System.Windows.Forms.UserControl
    {

        // UserControl overrides dispose to clean up the component list.
        [DebuggerNonUserCode()]
        protected override void Dispose(bool disposing)
        {
            try
            {
                if (disposing && components is object)
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
            _tmrRefresh = new System.Windows.Forms.Timer(components);
            _tmrRefresh.Tick += new EventHandler(tmrRefresh_Tick);
            SuspendLayout();
            // 
            // tmrRefresh
            // 
            // 
            // SuperPerfChart
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(6.0F, 13.0F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            DoubleBuffered = true;
            Name = "SuperPerfChart";
            Size = new System.Drawing.Size(450, 348);
            //SizeChanged += new EventHandler(SuperPerfChart_SizeChanged);
            ResumeLayout(false);
        }

        private System.Windows.Forms.Timer _tmrRefresh;

        internal System.Windows.Forms.Timer tmrRefresh
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _tmrRefresh;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_tmrRefresh != null)
                {
                    _tmrRefresh.Tick -= tmrRefresh_Tick;
                }

                _tmrRefresh = value;
                if (_tmrRefresh != null)
                {
                    _tmrRefresh.Tick += tmrRefresh_Tick;
                }
            }
        }
    }
}