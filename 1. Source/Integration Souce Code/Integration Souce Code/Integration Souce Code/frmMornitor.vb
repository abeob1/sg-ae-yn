Imports System.IO

Public Class frmMornitor
#Region "Service"
    Dim LiveServiceName As String = "SAPIntegration"
    Private Sub RefreshStatus()
        Dim a As New ServiceController(LiveServiceName)
        If a.Status = "" Then
            btnReg.Enabled = True
            btnUnReg.Enabled = False
            btnStart.Enabled = False
            btnStop.Enabled = False
            btn_Settings.Enabled = True
        ElseIf a.Status = "Stopped" Then
            btnStart.Enabled = True
            btnStop.Enabled = False
            btnReg.Enabled = False
            btnUnReg.Enabled = True
            btn_Settings.Enabled = True
        ElseIf a.Status = "Running" Then
            btnStart.Enabled = False
            btnStop.Enabled = True
            btnReg.Enabled = False
            btnUnReg.Enabled = True
            btn_Settings.Enabled = False
        End If
        Panel5.Visible = False
        Refresh()
    End Sub
    Private Sub btnStart_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnStart.Click
#If DEBUG Then
        PublicVariable.GetSettingInfo()
        Functions.SystemInitial()
        'If PublicVariable.TimeCreateJE = Date.Now.ToString("HH:mm") Then
        Dim doc As Documents = New Documents
        Dim str As String = String.Format("Data Source={0};Initial Catalog={1};User ID={2};Password={3}", PublicVariable.ServerName, PublicVariable.Database,
                                                                                                                PublicVariable.UserName, PublicVariable.Password)
        doc.CreateJE(str)
        MessageBox.Show("Create JE sucessfulluy")
        'End If
#Else
        Dim a As New ServiceController(LiveServiceName)
        Dim str As String
        Panel5.Visible = True
        Refresh()
        Threading.Thread.Sleep(3000)
        str = a.Start()
        While a.Status <> "Running"
        End While
        RefreshStatus()
#End If
        'Dim a As New ServiceController(LiveServiceName)
        'Dim str As String
        'str = a.Start()
        'Threading.Thread.Sleep(3000)
        'RefreshStatus()

    End Sub
    Private Sub btnStop_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnStop.Click
        Dim a As New ServiceController(LiveServiceName)
        Panel5.Visible = True
        Refresh()
        Threading.Thread.Sleep(3000)
        Dim str As String = a.Stop()
        While a.Status <> "Stopped"
        End While
        RefreshStatus()
    End Sub
    Private Sub btnReg_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnReg.Click
        Dim a As New ServiceController(LiveServiceName)
        a.Description = LiveServiceName
        a.DisplayName = LiveServiceName
        a.ServiceName = LiveServiceName
        a.StartupType = ServiceController.ServiceStartupType.Automatic

        Dim sReturn As String
        sReturn = a.Register(Application.ExecutablePath + " -service")
        If sReturn = "" Then
            MessageBox.Show("Register Sucessfull!")
            RefreshStatus()
            'Application.Exit()
        Else
            MessageBox.Show("Error: " + sReturn)
        End If
        Threading.Thread.Sleep(3000)
        RefreshStatus()
    End Sub
    Private Sub btnUnReg_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnUnReg.Click
        Dim a As New ServiceController(LiveServiceName)
        Dim sReturn As String
        sReturn = a.Unregister()
        If sReturn = "" Then
            MessageBox.Show("UnRegister Sucessfull!")
        Else
            MessageBox.Show("Error: " + sReturn)
        End If
        Threading.Thread.Sleep(3000)
        RefreshStatus()
    End Sub
#End Region
#Region "Events"
    Private Sub frmMornitor_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        RefreshStatus()
    End Sub
    
    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Me.Close()
        Application.Exit()
    End Sub
    'Private Sub Timer1_Tick(sender As System.Object, e As System.EventArgs) Handles Timer1.Tick
    '    Timer1.Enabled = False
    '    If ckAutoRef.Checked Then
    '    End If
    '    Timer1.Enabled = True
    'End Sub
    Private Sub btnLog_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim LogFileName As String = Application.StartupPath + "\logfile.txt"
        If File.Exists(LogFileName) Then
            System.Diagnostics.Process.Start(LogFileName)
        End If
    End Sub
    Private Sub btnRetryAll_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
    End Sub
   

    
#End Region


    
    Private Sub btnRetry_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)

    End Sub

    Private Sub btn_Settings_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Settings.Click
        Dim settingForm As Settings
        settingForm = New Settings
        settingForm.ShowDialog()
    End Sub

    
End Class