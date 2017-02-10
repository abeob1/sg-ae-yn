Public Class frmItemMaster
   
    Private Sub btnChooseFrom_Click(sender As System.Object, e As System.EventArgs) Handles btnChooseFrom.Click

        ' **********************************************************************************
        '   Function    :   frmItemMasterData_Load()
        '   Purpose     :   This Event will call Customer_CFL().
        '   Author      :   JOHN
        '   Date        :   AUG 2014
        ' **********************************************************************************

        Dim sFuncName As String = String.Empty
        Dim sErrDesc As String = String.Empty

        sFuncName = "GeneratingPDF_WithDocumentRange()"
        If Customer_CFL("1", sErrDesc) <> RTN_SUCCESS Then Throw New ArgumentException(sErrDesc)


    End Sub

    Public Function Customer_CFL(ByVal sCondition As String, ByRef sErrDesc As String) As Long

        ' **********************************************************************************
        '   Function    :   Customer_CFL()
        '   Purpose     :   This function will be providing the Item Master to select
        '               
        '   Parameters  :   ByVal sCondition As String
        '                       sCondition =  Identify the Text Box
        '                   ByRef sErrDesc AS String 
        '                       sErrDesc = Error Description to be returned to calling function
        '   Return      :   0 - FAILURE
        '                   1 - SUCCESS
        '   Author      :   JOHN
        '   Date        :   AUG 2014
        ' **********************************************************************************

        Dim oItemMaster As AE_YESNATURALS_DLL.clsItemMaster = New AE_YESNATURALS_DLL.clsItemMaster
        Dim sFuncName As String = String.Empty
        Dim sSQL As String = String.Empty
        Dim oDS As New DataSet

        Try
           
            sFuncName = "Customer_CFL()"
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Starting Function...", sFuncName)
            sSQL = "SELECT T0.[ItemCode], T0.[ItemName] from OITM T0 order by T0.itemcode"
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Execute SQL" & sSQL, sFuncName)
            oDS = oItemMaster.ExecuteSQLQuery(sSQL, P_sConString)
            CFL.CFL_Customer.DataSource = oDS.Tables(0)
            CFL.CFL_Customer.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.DisplayedCells)
            CFL.TextBox1.Text = sCondition
            CFL.ShowDialog()
            Customer_CFL = RTN_SUCCESS
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed with SUCCESS", sFuncName)
        Catch ex As Exception
            Customer_CFL = RTN_ERROR
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed with ERROR", sFuncName)
        End Try
    End Function

    Private Sub btnChooseTo_Click(sender As System.Object, e As System.EventArgs) Handles btnChooseTo.Click

        ' **********************************************************************************
        '   Function    :   frmItemMasterData_Load()
        '   Purpose     :   This Event will call Customer_CFL().
        '   Author      :   JOHN
        '   Date        :   AUG 2014
        ' **********************************************************************************

        Dim sFuncName As String = String.Empty
        Dim sErrDesc As String = String.Empty

        sFuncName = "GeneratingPDF_WithDocumentRange()"
        If Customer_CFL("2", sErrDesc) <> RTN_SUCCESS Then Throw New ArgumentException(sErrDesc)

    End Sub

    Private Sub frmItemMasterData_Load(sender As Object, e As System.EventArgs) Handles Me.Load

        ' **********************************************************************************
        '   Function    :   frmItemMasterData_Load()
        '   Purpose     :   This Event will Call the GetSystemIntializeInfo() for getting the values from AppConfig File.
        '   Author      :   JOHN
        '   Date        :   AUG 2014
        ' **********************************************************************************

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
            P_sConString = String.Empty
            P_sConString = "Data Source=" & p_oCompDef.p_sServerName & ";Initial Catalog=" & p_oCompDef.p_sTradingDBName & ";User ID=" & p_oCompDef.p_sDBUserName & "; Password=" & p_oCompDef.p_sDBPassword
            WriteToStatusScreen(False, sFuncName & " : Completed with SUCCESS ")
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed with SUCCESS", sFuncName)

        Catch ex As Exception
            WriteToStatusScreen(False, sFuncName & " : Completed with ERROR : " & ex.Message)
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed with ERROR", sFuncName)
            Call WriteToLogFile(ex.Message, sFuncName)
        End Try

    End Sub

    Private Sub btnExportCSV_Click(sender As System.Object, e As System.EventArgs) Handles btnExportCSV.Click

        ' **********************************************************************************
        '   Function    :   btnExportCSV_Click()
        '   Purpose     :   This Event start with validation, Query building then Export the data's into the csv file.
        '   Author      :   JOHN
        '   Date        :   AUG 2014
        ' **********************************************************************************

        Dim sFuncName As String = String.Empty
        Dim sErrDesc As String = String.Empty
        Dim oItemMaster As AE_YESNATURALS_DLL.clsItemMaster = New AE_YESNATURALS_DLL.clsItemMaster
        Dim sQuery As String = String.Empty

        Try
            Me.btnExportCSV.Enabled = False
            sFuncName = "btnExportCSV_Click()"

            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Starting Function", sFuncName)
            WriteToStatusScreen(False, sFuncName & " : Starting Function ")

            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Calling Validation()", sFuncName)
            If Validation() = False Then Exit Sub
            sQuery = "select * from AE_VW001_ItemMasterExport T0 where T0.ListName = '" & p_oCompDef.p_sPriceList & "'"
            If Not String.IsNullOrEmpty(txtItemCodeFrom.Text.Trim) Then
                sQuery += " and T0.InventoryCode >='" & txtItemCodeFrom.Text.Trim & "'"
            End If

            If Not String.IsNullOrEmpty(txtItemCodeTo.Text.Trim) Then
                sQuery += " and T0.InventoryCode <='" & txtItemCodeTo.Text.Trim & "'"
            End If

            sQuery += " and (case when isnull(T0.UpdateDate,'')='' then T0.CreateDate else T0.UpdateDate end)>='" & CDate(dtDateFrom.Value).ToString("yyyyMMdd") & "'"

            sQuery += " and (case when isnull(T0.UpdateDate,'')='' then T0.CreateDate else T0.UpdateDate end)<='" & CDate(dtDateTo.Value).ToString("yyyyMMdd") & "'"

            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Calling ExportToCSV()", sFuncName)

            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug(p_oCompDef.p_sQuery, sFuncName)
            WriteToStatusScreen(False, sFuncName & " : Calling ExportToCSV() ")

            If oItemMaster.ExportToCSV(sQuery, P_sConString, sErrDesc) <> RTN_SUCCESS Then
                ' Throw New ArgumentException(sErrDesc)
                WriteToStatusScreen(False, sFuncName & " : " & sErrDesc)
            Else
                If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("The CSV File has been Exported in the OUTPUT Folder", sFuncName)
                WriteToStatusScreen(False, sFuncName & " : The CSV File has been Exported in the OUTPUT Folder. ")
            End If

            WriteToStatusScreen(False, sFuncName & " : Completed with SUCCESS ")
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed with SUCCESS", sFuncName)


        Catch ex As Exception
            Me.btnExportCSV.Enabled = True
            WriteToStatusScreen(False, sFuncName & " : Completed with ERROR : " & ex.Message)
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed with ERROR", sFuncName)
            Call WriteToLogFile(ex.Message, sFuncName)

        Finally
            Me.btnExportCSV.Enabled = True
        End Try
    End Sub

    Public Sub WriteToStatusScreen(ByVal Clear As Boolean, ByVal msg As String)

        ' **********************************************************************************
        '   Function    :   WriteToStatusScreen()
        '   Purpose     :   This Subroutine will Display the status message in the Message text box.
        '   Author      :   JOHN
        '   Date        :   AUG 2014
        ' **********************************************************************************

        If Clear Then
            txtMessage.Text = ""
        End If
        txtMessage.HideSelection = True
        txtMessage.Text &= msg & vbCrLf
        txtMessage.SelectAll()
        txtMessage.ScrollToCaret()
        txtMessage.Refresh()
    End Sub

    Private Sub chkAllItems_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles chkAllItems.CheckedChanged

        ' **********************************************************************************
        '   Function    :   chkAllItems_CheckedChanged()
        '   Purpose     :   This Event will be disable and enable the controls based on the check box.
        '   Author      :   JOHN
        '   Date        :   AUG 2014
        ' **********************************************************************************

        Try
            If chkAllItems.Checked = True Then
                txtItemCodeFrom.Clear()
                txtItemCodeTo.Clear()
                txtItemCodeFrom.Enabled = False
                txtItemCodeTo.Enabled = False
                btnChooseFrom.Enabled = False
                btnChooseTo.Enabled = False
            Else
                btnChooseFrom.Enabled = True
                btnChooseTo.Enabled = True
                txtItemCodeFrom.Enabled = True
                txtItemCodeTo.Enabled = True
                txtItemCodeFrom.Focus()

            End If
        Catch ex As Exception

        End Try
    End Sub

    Function Validation() As Boolean

        ' **********************************************************************************
        '   Function    :   Validation()
        '   Purpose     :   This Function will be validate the Itemcodes Text Boxes.
        '   Author      :   JOHN
        '   Date        :   AUG 2014
        ' **********************************************************************************

        Dim sFuncName As String = String.Empty
        Try
            sFuncName = "Validation()"
            WriteToStatusScreen(False, sFuncName & " : Starting Function ")
            If chkAllItems.Checked = False Then
                If txtItemCodeFrom.Text.Trim = String.Empty Then
                    WriteToStatusScreen(False, "Select the Item Code From the List")
                    txtItemCodeFrom.Focus()
                    Return False
                ElseIf txtItemCodeTo.Text.Trim = String.Empty Then
                    WriteToStatusScreen(False, "Select the Item Code From the List")
                    txtItemCodeTo.Focus()
                    Return False
                End If
            End If
            WriteToStatusScreen(False, sFuncName & " : Completed With SUCCESS ")
        Catch ex As Exception
            WriteToStatusScreen(False, sFuncName & ": Completed With ERROR : " & ex.Message)
            Return False
        End Try
        Return True
    End Function

    Private Sub btnClear_Click(sender As System.Object, e As System.EventArgs) Handles btnClear.Click

        ' **********************************************************************************
        '   Function    :   btnClear_Click()
        '   Purpose     :   This Event will clear the data's in all controls.
        '   Author      :   JOHN
        '   Date        :   AUG 2014
        ' **********************************************************************************

        txtItemCodeFrom.Clear()
        txtItemCodeTo.Clear()
        chkAllItems.Checked = False
        txtMessage.Clear()
        txtItemCodeFrom.Focus()


    End Sub
End Class