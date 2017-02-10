Public Class Service1

    Public oTimer_ItemExportCSV As New System.Timers.Timer
    Public oTimer_ARinvoice As New System.Timers.Timer


    Protected Overrides Sub OnStart(ByVal args() As String)
        ' Add code here to start your service. This method should set things
        ' in motion so your service can do its work.
        Dim sFuncName As String = String.Empty
        Dim sErrDesc As String = String.Empty
        Dim sQuery As String = String.Empty
        Try
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Starting Function", sFuncName)

            'Getting the Parameter Values from App Cofig File
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Calling GetSystemIntializeInfo()", sFuncName)
            If GetSystemIntializeInfo(p_oCompDef, sErrDesc) <> RTN_SUCCESS Then Throw New ArgumentException(sErrDesc)
            If p_oCompDef.p_sItemMasterExport = "ON" Then
                If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("p_sItemMasterExport = ON", sFuncName)
                oTimer_ItemExportCSV.Interval = 60000
                oTimer_ItemExportCSV.Start()
                AddHandler oTimer_ItemExportCSV.Elapsed, AddressOf TimerCallingFunction_ItemmasterExport
                If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Timer Activated for Item Master Export ", sFuncName)
            Else
                If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("p_sItemMasterExport = OFF", sFuncName)
            End If

            'AR Invoice Create
            If p_oCompDef.p_sInvoice_IncomingPayment = "ON" Then
                If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("p_sInvoice_IncomingPayment = ON", sFuncName)
                oTimer_ARinvoice.Interval = 60000
                oTimer_ARinvoice.Start()
                AddHandler oTimer_ARinvoice.Elapsed, AddressOf TimerCallingFunction_ARInvoice
                If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Timer Activated for AR Invoice and Incoming Payment ", sFuncName)
            Else
                If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("p_sInvoice_IncomingPayment = OFF", sFuncName)
            End If

            P_sConString = String.Empty
            P_sConString = "Data Source=" & p_oCompDef.p_sServerName & ";Initial Catalog=" & p_oCompDef.p_sTradingDBName & ";User ID=" & p_oCompDef.p_sDBUserName & "; Password=" & p_oCompDef.p_sDBPassword

        Catch ex As Exception
            WriteToLogFile(ex.Message, sFuncName)
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed with ERROR", sFuncName)
        End Try
    End Sub

    Protected Overrides Sub OnStop()
        ' Add code here to perform any tear-down necessary to stop your service.
        Dim sFuncName As String = String.Empty
        sFuncName = "OnStop()"
        If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Attempting Function ", sFuncName)
        oTimer_ARinvoice.Stop()
        oTimer_ItemExportCSV.Stop()

    End Sub

    Public Sub New()
        MyBase.New()
        InitializeComponent()
    End Sub

    Private Sub TimerCallingFunction_ARInvoice()

        ' **********************************************************************************
        '   Function    :   TimerCallingFunction_ItemmasterExport()
        '   Purpose     :   This Subroutine will help to Export Item Master Data's from SAP to .CSV file.
        '   Author      :   JOHN
        '   Date        :   AUG 2014
        ' **********************************************************************************

        Dim sFuncName As String = String.Empty
        Dim sErrDesc As String = String.Empty
        Dim oARInvoice As clsARInvoice = New clsARInvoice

        Try
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Time " & (Format(DateTime.Now, "HH:mm")) & "  -   " & p_oCompDef.p_sInvoiceSerStartTime, sFuncName)
            If (Format(DateTime.Now, "HH:mm")) = p_oCompDef.p_sInvoiceSerStartTime Then

                sFuncName = "TimerCallingFunction_ARInvoice"
                If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Starting Function ", sFuncName)
                If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Calling Create_ARInvoice()", sFuncName)
                If oARInvoice.Create_ARInvoice(sErrDesc) <> RTN_SUCCESS Then Throw New ArgumentException(sErrDesc)
                If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed with SUCCESS ", sFuncName)
            End If
        Catch ex As Exception
            WriteToLogFile(ex.Message, sFuncName)
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed with ERROR", sFuncName)
        End Try

    End Sub

    Private Sub TimerCallingFunction_ItemmasterExport()

        ' **********************************************************************************
        '   Function    :   TimerCallingFunction_ItemmasterExport()
        '   Purpose     :   This Subroutine will help to Export Item Master Data's from SAP to .CSV file.
        '   Author      :   JOHN
        '   Date        :   AUG 2014
        ' **********************************************************************************

        Dim sFuncName As String = String.Empty
        Dim sErrDesc As String = String.Empty
        Try
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Time ItemmasterExport " & (Format(DateTime.Now, "HH:mm")) & "  -   " & p_oCompDef.p_sItemSerStartTime, sFuncName)

            If (Format(DateTime.Now, "HH:mm")) = p_oCompDef.p_sItemSerStartTime Then

                sFuncName = "TimerCallingFunction_ItemmasterExport"
                If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Starting Function ", sFuncName)
                If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Calling ItemMasterExport()", sFuncName)
                If ClsItemmaster.ItemMasterExport(sErrDesc) <> RTN_SUCCESS Then Throw New ArgumentException(sErrDesc)
                If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed with SUCCESS ", sFuncName)
            End If
        Catch ex As Exception
            WriteToLogFile(ex.Message, sFuncName)
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed with ERROR", sFuncName)
        End Try

    End Sub

End Class
