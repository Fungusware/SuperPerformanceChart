Public Class frmMain
    Private _testValue As Decimal
    Private _adder As Integer = 1
    Private _test2Value As Decimal
    Private _adder2 As Integer

    Private Sub frmMain_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        SetupChart(spcClientDriven)
    End Sub

    Public Sub SetupChart(chrt As SuperPerformanceChart.SuperPerfChart)
        chrt.Series.Clear()

        chrt.AddSeries("Test", "Test", Color.LightSeaGreen)
        With chrt.Series("Test")
            .CacheSize = 200
            .ZOrder = 0
        End With

        'Dim serTestStyle2 As New SuperPerformanceChart.SeriesStyle(Color.Orange)
        'serTestStyle2.Fill.Alpha = 100
        'serTestStyle2.Text.Font = ofnt
        'serTestStyle2.Text.Color = Color.Orange
        'serTestStyle2.Smooth = False
        'chrt.AddSeries("Test2", "Test2", serTestStyle2)
        'With chrt.Series("Test2")
        '    .CacheSize = 200
        '    .SeriesValueFormatter = Function(d) "Test2 : " & d.ToString("00.0")
        '    .ValueDisplayType = SuperPerformanceChart.Series.ValueDisplayTypeEnum.Current Or
        '        SuperPerformanceChart.Series.ValueDisplayTypeEnum.Mean Or
        '        SuperPerformanceChart.Series.ValueDisplayTypeEnum.VisibleMean
        '    .ZOrder = 0
        'End With

        chrt.AddSeries("CPU", "CPU", Color.SteelBlue)
        With chrt.Series("CPU")
            .LegendMask = "@name - Current : @current | Mean : @mean | Mean.V : @vmean"
            .ZOrder = 1
        End With

        chrt.AddSeries("Mem", "Mem", Color.DarkGoldenrod)
        With chrt.Series("Mem")
            .CacheSize = 100
            .LegendMask = "@name - Current : @current Mb | Mean : @mean Mb | Mean.V : @vmean  Mb"
            .ValueFormatter = Function(t, v) (v / 1024 / 1024).ToString("0.0")
            .ZOrder = 2
        End With

        chrt.AddSeries("Threads", "Threads", Color.ForestGreen)
        With chrt.Series("Threads")
            .CacheSize = 100
            .ValueFormatter = Function(t, v) (v / 1024 / 1024).ToString("0.0")
            .ZOrder = 3
        End With

        chrt.ExpandSeriesCacheToFillScreen = True

        chrt.BackgroundStyle.StartColor = Color.White
        chrt.BackgroundStyle.EndColor = Color.White

        chrt.VerticalGridLine.Visible = True
    End Sub

    Private Sub tmrMain_Tick(sender As Object, e As EventArgs) Handles tmrMain.Tick
        spcClientDriven.AddValue("Test", _testValue)
        If (_testValue >= 20) Then
            _adder = -1
        ElseIf (_testValue <= 0) Then
            _adder = 1
        End If
        _testValue += _adder

        spcClientDriven.AddValue("Test2", _test2Value)
        If (_test2Value >= 30) Then
            _adder2 = -1
        ElseIf (_test2Value <= 0) Then
            _adder2 = 1
        End If
        _test2Value += _adder2

        spcClientDriven.AddValue("CPU", CDec(pcCPU.NextValue) / Environment.ProcessorCount)
        spcClientDriven.AddValue("Mem", CDec(pcMem.NextValue))
        spcClientDriven.AddValue("Threads", CDec(pcThreads.NextValue))

        lblMemSeriesLastValue.Text = $"Test : {_testValue}      Test 2 : {_test2Value}"
    End Sub

    Private Sub btnMakeWOrk_Click(sender As Object, e As EventArgs) Handles btnMakeWOrk.Click
        Threading.ThreadPool.QueueUserWorkItem(Sub(o)
                                                   Dim lst As New List(Of Double)

                                                   For i = 0 To 10000000
                                                       Dim d = Math.Log(i)

                                                       lst.Add(d)
                                                   Next
                                               End Sub)
    End Sub

    'Private Sub chkLocalProc_CheckedChanged(sender As Object, e As EventArgs)
    '    pcCPU.InstanceName = If(chkLocalProc.Checked, "testapp", "")
    '    pcMem.InstanceName = If(chkLocalProc.Checked, "testapp", "")
    '    pcThreads.InstanceName = If(chkLocalProc.Checked, "testapp", "")
    'End Sub
End Class
