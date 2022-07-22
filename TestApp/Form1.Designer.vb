<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmMain
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.pcMem = New System.Diagnostics.PerformanceCounter()
        Me.pcCPU = New System.Diagnostics.PerformanceCounter()
        Me.tmrMain = New System.Windows.Forms.Timer(Me.components)
        Me.btnMakeWOrk = New System.Windows.Forms.Button()
        Me.pcThreads = New System.Diagnostics.PerformanceCounter()
        Me.lblMemSeriesLastValue = New System.Windows.Forms.Label()
        Me.pg1 = New System.Windows.Forms.PropertyGrid()
        Me.spcClientDriven = New SuperPerformanceChart.SuperPerfChart()
        CType(Me.pcMem, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.pcCPU, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.pcThreads, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(12, 9)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(83, 16)
        Me.Label1.TabIndex = 125
        Me.Label1.Text = "Client Driven"
        '
        'pcMem
        '
        Me.pcMem.CategoryName = "Process"
        Me.pcMem.CounterName = "Private Bytes"
        Me.pcMem.InstanceName = "testapp"
        '
        'pcCPU
        '
        Me.pcCPU.CategoryName = "Process"
        Me.pcCPU.CounterName = "% Processor Time"
        Me.pcCPU.InstanceName = "testapp"
        '
        'tmrMain
        '
        Me.tmrMain.Enabled = True
        '
        'btnMakeWOrk
        '
        Me.btnMakeWOrk.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnMakeWOrk.Location = New System.Drawing.Point(769, 528)
        Me.btnMakeWOrk.Name = "btnMakeWOrk"
        Me.btnMakeWOrk.Size = New System.Drawing.Size(276, 23)
        Me.btnMakeWOrk.TabIndex = 130
        Me.btnMakeWOrk.Text = "Do Some Work"
        Me.btnMakeWOrk.UseVisualStyleBackColor = True
        '
        'pcThreads
        '
        Me.pcThreads.CategoryName = "Process"
        Me.pcThreads.CounterName = "Thread Count"
        Me.pcThreads.InstanceName = "testapp"
        '
        'lblMemSeriesLastValue
        '
        Me.lblMemSeriesLastValue.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lblMemSeriesLastValue.AutoSize = True
        Me.lblMemSeriesLastValue.Location = New System.Drawing.Point(12, 612)
        Me.lblMemSeriesLastValue.Name = "lblMemSeriesLastValue"
        Me.lblMemSeriesLastValue.Size = New System.Drawing.Size(26, 16)
        Me.lblMemSeriesLastValue.TabIndex = 131
        Me.lblMemSeriesLastValue.Text = "val"
        '
        'pg1
        '
        Me.pg1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.pg1.HelpVisible = False
        Me.pg1.Location = New System.Drawing.Point(769, 27)
        Me.pg1.Name = "pg1"
        Me.pg1.SelectedObject = Me.spcClientDriven
        Me.pg1.Size = New System.Drawing.Size(276, 495)
        Me.pg1.TabIndex = 129
        Me.pg1.ToolbarVisible = False
        '
        'spcClientDriven
        '
        Me.spcClientDriven.ExpandSeriesCacheToFillScreen = False
        Me.spcClientDriven.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.spcClientDriven.AntiAliasing = True
        Me.spcClientDriven.Location = New System.Drawing.Point(15, 27)
        Me.spcClientDriven.Name = "spcClientDriven"
        Me.spcClientDriven.Progress = 0
        Me.spcClientDriven.Size = New System.Drawing.Size(748, 566)
        Me.spcClientDriven.TabIndex = 126
        Me.spcClientDriven.TimerInterval = 100
        Me.spcClientDriven.TimerMode = SuperPerformanceChart.TimerMode.Disabled
        Me.spcClientDriven.UnifiedVerticalScale = False
        Me.spcClientDriven.ValueSpacing = 5
        '
        'frmMain
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1057, 637)
        Me.Controls.Add(Me.lblMemSeriesLastValue)
        Me.Controls.Add(Me.btnMakeWOrk)
        Me.Controls.Add(Me.pg1)
        Me.Controls.Add(Me.spcClientDriven)
        Me.Controls.Add(Me.Label1)
        Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.Name = "frmMain"
        Me.Text = "PerfChart Tester"
        CType(Me.pcMem, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.pcCPU, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.pcThreads, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents spcClientDriven As SuperPerformanceChart.SuperPerfChart
    Friend WithEvents pcMem As System.Diagnostics.PerformanceCounter
    Friend WithEvents pcCPU As System.Diagnostics.PerformanceCounter
    Friend WithEvents tmrMain As System.Windows.Forms.Timer
    Friend WithEvents pg1 As System.Windows.Forms.PropertyGrid
    Friend WithEvents btnMakeWOrk As System.Windows.Forms.Button
    Friend WithEvents pcThreads As System.Diagnostics.PerformanceCounter
    Friend WithEvents lblMemSeriesLastValue As Label
End Class
