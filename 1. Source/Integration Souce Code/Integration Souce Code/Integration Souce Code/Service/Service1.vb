

Public Class Service1
#Region "General"
    Protected Overrides Sub OnStart(ByVal args() As String)
        Try
            PublicVariable.GetSettingInfo()
            Functions.SystemInitial()
            Timer1.Enabled = True
            Timer1.Interval = PublicVariable.Timer * 1000
            Timer1.Start()

            'Timer2.Enabled = True
            'Timer2.Start()
        Catch ex As Exception
            Functions.WriteLog("OnStart: " + ex.Message)
        End Try
    End Sub
    Protected Overrides Sub OnStop()
        Try
            Timer1.Enabled = False
            Timer1.Stop()
        Catch ex As Exception
            Functions.WriteLog(ex.Message)
        End Try
    End Sub
    Private Sub Timer1_Elapsed(ByVal sender As System.Object, ByVal e As System.Timers.ElapsedEventArgs) Handles Timer1.Elapsed
        Try

            Timer1.Enabled = False
            'Dim functions As New Functions
            Functions.AutoRun()
            Timer1.Enabled = True
        Catch ex As Exception
            Functions.WriteLog(ex.Message)
            Timer1.Enabled = True
        End Try
    End Sub
#End Region


End Class
