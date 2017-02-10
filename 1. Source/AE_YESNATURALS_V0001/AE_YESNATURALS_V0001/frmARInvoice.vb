Public Class frmARInvoice

    Private Sub frmARInvoice_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        End

    End Sub

    Private Sub btnCreate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCreate.Click
        Dim sFuncName As String = String.Empty
        Dim sErrDesc As String = String.Empty
        Dim oCommon As AE_YESNATURALS_DLL.clsCommon = New AE_YESNATURALS_DLL.clsCommon
        Dim oFileInfo As System.IO.FileInfo = Nothing
        Dim oFileFullName As String = String.Empty


        Try
            sFuncName = "Main()"
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Starting Function", sFuncName)

            WriteToStatusScreen(True, sFuncName & " : Starting Function ")

            'Getting the Parameter Values from App Cofig File
            WriteToStatusScreen(False, sFuncName & " : Calling GetSystemIntializeInfo() ")
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Calling GetSystemIntializeInfo()", sFuncName)
            If GetSystemIntializeInfo(p_oCompDef, sErrDesc) <> RTN_SUCCESS Then Throw New ArgumentException(sErrDesc)

            'Find the Files placed in that Folder
            Dim DirInfo As New System.IO.DirectoryInfo(p_oCompDef.p_sInputDir)
            Dim files() As System.IO.FileInfo

            WriteToStatusScreen(False, sFuncName & " : Calling Identify the CSV File ")
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Calling Identify the CSV File ", sFuncName)

            files = DirInfo.GetFiles("*_POS.csv")


            If files.Count = 0 Then Call WriteToStatusScreen(False, sFuncName & " : There is No CSV Files in INPUT Folder ")
            If files.Count = 0 Then Call WriteToLogFile_Debug("There is No CSV Files in INPUT Folder ", sFuncName)

            For Each File As System.IO.FileInfo In files

                WriteToStatusScreen(False, sFuncName & " : Completed with SUCCESS Identify the CSV File ")
                If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed with SUCCESS Identify the CSV File ", sFuncName)

                WriteToStatusScreen(False, sFuncName & " : Calling GetDataViewFromCSV() ")
                If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Calling GetDataViewFromCSV()", sFuncName)

                p_oDataView = oCommon.GetDataViewFromCSV(p_oCompDef.p_sInputDir & "\" & File.Name)

                oFileInfo = File
                oFileFullName = File.FullName

                If p_oDataView Is Nothing Then
                    WriteToStatusScreen(False, sFuncName & " : No Datas in the CSV file ")
                    If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("No Datas in the CSV file", sFuncName)

                    WriteToStatusScreen(False, sFuncName & " : Completed with SUCCESS GetDataViewFromCSV() ")
                    If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed with SUCCESS GetDataViewFromCSV()", sFuncName)

                    Continue For
                Else
                    WriteToStatusScreen(False, sFuncName & " : Completed with SUCCESS GetDataViewFromCSV() ")
                    If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed with SUCCESS GetDataViewFromCSV()", sFuncName)

                    Dim sCompanyName As String = String.Empty


                    For iCompCount As Integer = 0 To 1

                        oDICompany(iCompCount) = New SAPbobsCOM.Company

                        If iCompCount = 0 Then
                            sCompanyName = p_oCompDef.p_sTradingDBName
                        ElseIf iCompCount = 1 Then
                            sCompanyName = p_oCompDef.p_sRetailDBName
                        End If


                        WriteToStatusScreen(False, sFuncName & " : Calling ConnectToTargetCompany() ")

                        If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Calling ConnectToTargetCompany()", sFuncName)

                        If ConnectToTargetCompany(oDICompany(iCompCount), sCompanyName, sErrDesc) <> RTN_SUCCESS Then
                            Throw New ArgumentException(sErrDesc)
                            End
                        End If


                    Next

                    WriteToStatusScreen(False, sFuncName & " : Start the " & p_oCompDef.p_sTradingDBName & " Company Transaction ")
                    If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Start the Company Transaction ", sFuncName)
                    If oDICompany(0).InTransaction = False Then oDICompany(0).StartTransaction()

                    WriteToStatusScreen(False, sFuncName & " : Calling DeliveryOrder() ")
                    If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Calling DeliveryOrder()", sFuncName)
                    If DeliveryOrder(p_oDataView, sErrDesc) <> RTN_SUCCESS Then Throw New ArgumentException(sErrDesc)

                    If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed with SUCCESS DeliveryOrder()", sFuncName)
                    WriteToStatusScreen(False, sFuncName & " : Completed with SUCCESS DeliveryOrder() ")

                    'Function to Read and Store the CreditCard Details in Array Variable
                    WriteToStatusScreen(False, sFuncName & " : Calling GetCredeitCardDetails() ")
                    If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Calling GetCredeitCardDetails()", sFuncName)
                    p_iArrayCount = 0
                    If GetCredeitCardDetails(sErrDesc) <> RTN_SUCCESS Then Throw New ArgumentException(sErrDesc)

                    If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed with SUCCESS GetCredeitCardDetails()", sFuncName)
                    WriteToStatusScreen(False, sFuncName & " : Completed with SUCCESS GetCredeitCardDetails() ")

                    WriteToStatusScreen(False, sFuncName & " : Start the " & p_oCompDef.p_sTradingDBName & " Company Transaction ")
                    If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Start the Company Transaction ", sFuncName)
                    If oDICompany(1).InTransaction = False Then oDICompany(1).StartTransaction()

                    WriteToStatusScreen(False, sFuncName & " : Calling ARInvoice() ")
                    If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Calling ARInvoice()", sFuncName)
                    If AR_Invoice(p_oDataView, sErrDesc) <> RTN_SUCCESS Then Throw New ArgumentException(sErrDesc)

                    If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed with SUCCESS ARInvoice()", sFuncName)
                    WriteToStatusScreen(False, sFuncName & " : Completed with SUCCESS ARInvoice() ")

                    WriteToStatusScreen(False, sFuncName & " : Calling FileMoveToArchive for Moving CSV File to Success Folder ")
                    If p_iDebugMode = DEBUG_ON Then WriteToLogFile_Debug("Calling FileMoveToArchive for Moving CSV File to Success Folder", sFuncName)
                    FileMoveToArchive(File, File.FullName, RTN_SUCCESS, sErrDesc)

                    WriteToStatusScreen(False, sFuncName & " : Successfully Moved the CSV File to Success Folder : " & File.Name)
                    If p_iDebugMode = DEBUG_ON Then WriteToLogFile_Debug("Successfully Moved the CSV File to Success Folder : " & File.Name, sFuncName)


                    WriteToStatusScreen(False, sFuncName & " : Calling Company Commit Function ")
                    If p_iDebugMode = DEBUG_ON Then WriteToLogFile_Debug(" Calling Company Commit Function ", sFuncName)

                    Company_Commit(sErrDesc)

                End If
            Next

            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed with SUCCESS", sFuncName)
            WriteToStatusScreen(False, sFuncName & " : Completed with SUCCESS ")

        Catch ex As Exception

            Call WriteToLogFile(ex.Message, sFuncName)

            WriteToStatusScreen(False, sFuncName & " : Calling Company Rollback Function ")
            If p_iDebugMode = DEBUG_ON Then WriteToLogFile_Debug(" Calling Company Rollback Function ", sFuncName)

            Company_Rollback(sErrDesc)


            WriteToStatusScreen(False, sFuncName & " : Calling FileMoveToArchive for Moving CSV File to Failure Folder ")
            If p_iDebugMode = DEBUG_ON Then WriteToLogFile_Debug("Calling FileMoveToArchive for Moving CSV File to Failure Folder", sFuncName)

            FileMoveToArchive(oFileInfo, oFileFullName, RTN_ERROR, sErrDesc)

            WriteToStatusScreen(False, sFuncName & " : Successfully Moved the CSV File to Failure Folder : " & oFileInfo.Name)
            If p_iDebugMode = DEBUG_ON Then WriteToLogFile_Debug("Successfully Moved the CSV File to Failure Folder : " & oFileInfo.Name, sFuncName)

            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed with ERROR", sFuncName)
            WriteToStatusScreen(False, sFuncName & " : Completed with ERROR ")

        End Try

    End Sub

    Function DeliveryOrder(ByRef DataView_ As DataView, ByRef sErrDesc As String) As Long
        Dim sFuncName As String = String.Empty
        Dim p_oDVJE As DataView = Nothing
        Dim oDTDistinct As DataTable = Nothing
        Dim oDTRowFilter As DataTable = Nothing
        Dim bSuccess As Boolean = True

        Dim oDeliveryOrder As AE_YESNATURALS_DLL.clsDeliveryOrder = New AE_YESNATURALS_DLL.clsDeliveryOrder
        Try
            sFuncName = "DeliveryOrder()"
            p_oDVJE = DataView_
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Starting function", sFuncName)
            oDTDistinct = p_oDVJE.Table.DefaultView.ToTable(True, "F3")


            'For Quantity Checking :

            AE_YESNATURALS_DLL.sConnString = "Data Source=" & p_oCompDef.p_sServerName & ";Initial Catalog=" & p_oCompDef.p_sTradingDBName & ";Persist Security Info=True;User ID=" & p_oCompDef.p_sDBUserName & ";Password=" & p_oCompDef.p_sDBPassword & ""

            For IntRow As Integer = 0 To oDTDistinct.Rows.Count - 1

                If String.IsNullOrEmpty(oDTDistinct.Rows(IntRow).Item(0).ToString) Then Continue For

                If oDTDistinct.Rows(IntRow).Item(0).ToString.ToUpper = "LOCATION/WH" Then Continue For

                p_oDVJE.RowFilter = "F3 = '" & oDTDistinct.Rows(IntRow).Item(0).ToString & "'"

                WriteToStatusScreen(False, "Calling Quantity_Checking() Function for Branch Code : " & oDTDistinct.Rows(IntRow).Item(0).ToString)

                If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Calling Quantity_Checking() Function for Branch Code : " & oDTDistinct.Rows(IntRow).Item(0).ToString, sFuncName)

                If oDeliveryOrder.Quantity_Checking(p_oDVJE, oDICompany(0), sErrDesc) <> RTN_SUCCESS Then
                    bSuccess = False
                    If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed Quantity_Checking() Function for Branch Code : " & oDTDistinct.Rows(IntRow).Item(0).ToString, sFuncName)
                    Continue For
                End If

            Next

            'For posting the Delivery Order:

            If bSuccess = True Then


                For IntRow As Integer = 0 To oDTDistinct.Rows.Count - 1

                    If String.IsNullOrEmpty(oDTDistinct.Rows(IntRow).Item(0).ToString) Then Continue For

                    If oDTDistinct.Rows(IntRow).Item(0).ToString.ToUpper = "LOCATION/WH" Then Continue For

                    p_oDVJE.RowFilter = "F3 = '" & oDTDistinct.Rows(IntRow).Item(0).ToString & "'"

                    WriteToStatusScreen(False, "Calling DeliveryOrder() Function for Branch Code : " & oDTDistinct.Rows(IntRow).Item(0).ToString)

                    If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Calling DeliveryOrder() Function for Branch Code : " & oDTDistinct.Rows(IntRow).Item(0).ToString, sFuncName)

                    If oDeliveryOrder.DeliveryOrder(p_oDVJE, oDICompany(0), sErrDesc) <> RTN_SUCCESS Then Throw New ArgumentException(sErrDesc)

                    If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed DeliveryOrder() Function for Branch Code : " & oDTDistinct.Rows(IntRow).Item(0).ToString, sFuncName)

                Next

            End If

            If bSuccess = False Then Throw New ArgumentException(sErrDesc)

            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed with SUCCESS", sFuncName)

            DeliveryOrder = RTN_SUCCESS


        Catch ex As Exception
            sErrDesc = ex.Message
            Call WriteToLogFile(ex.Message, sFuncName)
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed with ERROR", sFuncName)
            DeliveryOrder = RTN_ERROR

        End Try

    End Function

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

    'Public Function QunatityChecking(ByRef oDV As DataView, ByRef oDICompany As SAPbobsCOM.Company, ByRef sErrDesc As String)

    '    Dim sFuncName As String = String.Empty
    '    Dim oDTDist As DataTable = Nothing
    '    Dim oDTRowFilter As DataTable = Nothing
    '    Dim oCommon As AE_YESNATURALS_DLL.clsCommon = New AE_YESNATURALS_DLL.clsCommon
    '    Dim sInventryItem As String = String.Empty
    '    Dim sItemCode As String = String.Empty
    '    Dim dQuantity As Double = 0.0
    '    Dim dOnHand As Double = 0.0
    '    Dim sQtyErrorMsg As String = String.Empty
    '    Dim sQueryString As String = String.Empty
    '    Try
    '        sFuncName = "DeliveryOrder()"

    '        If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Starting function", sFuncName)

    '        oDTDist = oDV.Table.DefaultView.ToTable(True, "ITEM_CODE")

    '        For IntRow As Integer = 0 To oDTDist.Rows.Count - 1

    '            sItemCode = oDTDist.Rows(IntRow).Item(0).ToString

    '            sInventryItem = oCommon.GetSingleValue("select isnull(InvntItem,'N')  from OITM where ItemCode ='" & sItemCode & "'", oDICompany, sErrDesc)

    '            If sInventryItem = "N" Then Continue For

    '            oDV.RowFilter = "ITEM_CODE = '" & sItemCode & "'"

    '            For iRowCount As Integer = 0 To oDV.Count










    '            Next


    '            If (dQuantity > dOnHand) Then

    '                If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Inside the Falls Quantity : ", sFuncName)

    '                sQtyErrorMsg = "Quantity Falls : Invoice Quantity : " & dQuantity & " : OnHand Quantity : " & dOnHand

    '                sQueryString += " insert into AB_ErrorLog Values(GETDATE (),convert(date,'" & dDelDocDate & "',103),'" & sExcelLineNum & "' " & _
    '                    " ,'" & sPOSNumber & "','" & sItemCode & "','" & sLocation & "','" & dQuantity & "','" & sQtyErrorMsg & "')"


    '                If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed Recordset : " & sQueryString, sFuncName)

    '                If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("After Inside the Falls Quantity : " & sQueryString, sFuncName)


    '            End If

    '        Next

    '        If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed with SUCCESS", sFuncName)

    '        QunatityChecking = RTN_SUCCESS
    '    Catch ex As Exception
    '        sErrDesc = ex.Message
    '        Call WriteToLogFile(ex.Message, sFuncName)
    '        If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed with ERROR", sFuncName)
    '        QunatityChecking = RTN_ERROR
    '    End Try
    'End Function



End Class