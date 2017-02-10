Public Class frmItemMasterData

    Private Sub btnChooseFrom_Click(sender As System.Object, e As System.EventArgs) Handles btnChooseFrom.Click
        Dim sErrDesc As String = String.Empty

        Customer_CFL(p_oCompDef.p_sQuery, sErrDesc)
        txtItemCodeFrom.Text = p_sCFLValue


    End Sub

    Public Function Customer_CFL(ByVal sQuery As String, ByRef sErrDesc As String) As Long

        Try
            Dim sFuncName As String = String.Empty
            Dim sSQL As String = String.Empty
            Dim oDS As New DataSet
            Dim oItemMaster As AE_YESNATURALS_DLL.clsItemMaster = New AE_YESNATURALS_DLL.clsItemMaster
            sFuncName = "CustomerCFLCommonFunction() "
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Starting Function...", sFuncName)
            'If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Execute SQL" & sSQL, sFuncName)

            CFL.CFL_Customer.DataSource = oItemMaster.GetData(sQuery, p_oCompany)

            CFL.CFL_Customer.Columns(1).Width = 300

            '  CFL.CFL_Customer.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.DisplayedCells)
            Customer_CFL = RTN_SUCCESS
            CFL.ShowDialog()

        Catch ex As Exception
            Customer_CFL = RTN_ERROR
        End Try
    End Function

    Private Sub btnChooseTo_Click(sender As System.Object, e As System.EventArgs) Handles btnChooseTo.Click
        Dim sErrDesc As String = String.Empty

        Customer_CFL("Select ItemCode,ItemName from OITM", sErrDesc)
        txtItemCodeTo.Text = p_sCFLValue
    End Sub

    Private Sub frmItemMasterData_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        Dim sFuncName As String = String.Empty
        Dim sErrDesc As String = String.Empty

        Try
            sFuncName = "frmItemMasterData_Load()"
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Starting Function", sFuncName)
            WriteToStatusScreen(True, sFuncName & " : Starting Function ")

            'Getting the Parameter Values from App Cofig File
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Calling GetSystemIntializeInfo()", sFuncName)
            WriteToStatusScreen(False, sFuncName & " : Calling GetSystemIntializeInfo() ")
            If GetSystemIntializeInfo(p_oCompDef, sErrDesc) <> RTN_SUCCESS Then Throw New ArgumentException(sErrDesc)

            'Function to connect the Company
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Calling ConnectToTargetCompany()", sFuncName)
            WriteToStatusScreen(False, sFuncName & " : Calling ConnectToTargetCompany() ")
            If ConnectToTargetCompany(p_oCompany, sErrDesc) <> RTN_SUCCESS Then Throw New ArgumentException(sErrDesc)

            WriteToStatusScreen(False, sFuncName & " : Completed with SUCCESS ")
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed with SUCCESS", sFuncName)

        Catch ex As Exception
            WriteToStatusScreen(False, sFuncName & " : Completed with ERROR : " & ex.Message)
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed with ERROR", sFuncName)
            Call WriteToLogFile(ex.Message, sFuncName)
        End Try
      
    End Sub

    Private Sub btnExportCSV_Click(sender As System.Object, e As System.EventArgs) Handles btnExportCSV.Click
        Dim sFuncName As String = String.Empty
        Dim sErrDesc As String = String.Empty
        Dim oItemMaster As AE_YESNATURALS_DLL.clsItemMaster = New AE_YESNATURALS_DLL.clsItemMaster
        Try
            sFuncName = "btnExportCSV_Click()"
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Starting Function", sFuncName)
            WriteToStatusScreen(False, sFuncName & " : Starting Function ")
            If Not String.IsNullOrEmpty(txtItemCodeFrom.Text.Trim) Then
                p_oCompDef.p_sQuery += " and T0.ItemCode >='" & txtItemCodeFrom.Text.Trim & "'"
            End If

            If Not String.IsNullOrEmpty(txtItemCodeTo.Text.Trim) Then
                p_oCompDef.p_sQuery += " and T0.ItemCode <='" & txtItemCodeTo.Text.Trim & "'"
            End If

            p_oCompDef.p_sQuery += " and (case when isnull(T0.UpdateDate,'')='' then T0.CreateDate else T0.UpdateDate end)>='" & CDate(dtDateFrom.Value).ToString("yyyyMMdd") & "'"

            p_oCompDef.p_sQuery += " and (case when isnull(T0.UpdateDate,'')='' then T0.CreateDate else T0.UpdateDate end)<='" & CDate(dtDateTo.Value).ToString("yyyyMMdd") & "'"

            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Calling ExportToCSV()", sFuncName)

            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug(p_oCompDef.p_sQuery, sFuncName)
            WriteToStatusScreen(False, sFuncName & " : Calling ExportToCSV() ")

            oItemMaster.ExportToCSV(p_oCompDef.p_sQuery, p_oCompany, sErrDesc)

            WriteToStatusScreen(False, sFuncName & " : Completed with SUCCESS ")
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed with SUCCESS", sFuncName)

        Catch ex As Exception
            WriteToStatusScreen(False, sFuncName & " : Completed with ERROR : " & ex.Message)
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed with ERROR", sFuncName)
            Call WriteToLogFile(ex.Message, sFuncName)
        End Try
    End Sub

    Public Sub WriteToStatusScreen(ByVal Clear As Boolean, ByVal msg As String)
        If Clear Then
            txtMessage.Text = ""
        End If
        txtMessage.HideSelection = True
        txtMessage.Text &= msg & vbCrLf
        txtMessage.SelectAll()
        txtMessage.ScrollToCaret()
        txtMessage.Refresh()
    End Sub
End Class
