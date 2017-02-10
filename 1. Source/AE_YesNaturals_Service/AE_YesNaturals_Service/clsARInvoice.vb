Public Class clsARInvoice

    Public Function Create_ARInvoice(ByRef sErrDesc As String) As Long

        Dim sFuncName As String = String.Empty
        Dim oFileInfo As System.IO.FileInfo = Nothing
        Dim oFileFullName As String = String.Empty
        Dim oCommon As AE_YESNATURALS_DLL.clsCommon = New AE_YESNATURALS_DLL.clsCommon

        Try
            sFuncName = "Main()"
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Starting Function", sFuncName)

            'Find the Files placed in that Folder
            Dim DirInfo As New System.IO.DirectoryInfo(p_oCompDef.p_sInputDir)
            Dim files() As System.IO.FileInfo


            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Calling Identify the CSV File ", sFuncName)

            files = DirInfo.GetFiles("*_POS.csv")

            If files.Count = 0 Then Call WriteToLogFile_Debug("There is No CSV Files in INPUT Folder ", sFuncName)

            For Each File As System.IO.FileInfo In files


                If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed with SUCCESS Identify the CSV File ", sFuncName)
                If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Calling GetDataViewFromCSV()", sFuncName)

                p_oDataView = oCommon.GetDataViewFromCSV(p_oCompDef.p_sInputDir & "\" & File.Name)

                oFileInfo = File
                oFileFullName = File.FullName

                If p_oDataView Is Nothing Then

                    If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("No Datas in the CSV file", sFuncName)

                    If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed with SUCCESS GetDataViewFromCSV()", sFuncName)

                    Continue For
                Else


                    If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed with SUCCESS GetDataViewFromCSV()", sFuncName)

                    Dim sCompanyName As String = String.Empty


                    For iCompCount As Integer = 0 To 1

                        oDICompany(iCompCount) = New SAPbobsCOM.Company

                        If iCompCount = 0 Then
                            sCompanyName = p_oCompDef.p_sTradingDBName
                        ElseIf iCompCount = 1 Then
                            sCompanyName = p_oCompDef.p_sRetailDBName
                        End If

                        If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Calling ConnectToTargetCompany()", sFuncName)

                        If ConnectToTargetCompany(oDICompany(iCompCount), sCompanyName, sErrDesc) <> RTN_SUCCESS Then
                            Throw New ArgumentException(sErrDesc)
                            End
                        End If

                        If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Start the Company Transaction ", sFuncName)
                        If oDICompany(iCompCount).InTransaction = False Then oDICompany(iCompCount).StartTransaction()

                    Next

                    If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Calling DeliveryOrder()", sFuncName)
                    If DeliveryOrder(p_oDataView, sErrDesc) <> RTN_SUCCESS Then Throw New ArgumentException(sErrDesc)

                    If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed with SUCCESS DeliveryOrder()", sFuncName)


                    'Function to Read and Store the CreditCard Details in Array Variable

                    If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Calling GetCredeitCardDetails()", sFuncName)
                    p_iArrayCount = 0
                    If GetCredeitCardDetails(sErrDesc) <> RTN_SUCCESS Then Throw New ArgumentException(sErrDesc)

                    If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed with SUCCESS GetCredeitCardDetails()", sFuncName)

                    If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Calling ARInvoice()", sFuncName)
                    If AR_Invoice(p_oDataView, sErrDesc) <> RTN_SUCCESS Then Throw New ArgumentException(sErrDesc)

                    If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed with SUCCESS ARInvoice()", sFuncName)

                    If p_iDebugMode = DEBUG_ON Then WriteToLogFile_Debug("Calling FileMoveToArchive for Moving CSV File to Success Folder", sFuncName)
                    FileMoveToArchive(File, File.FullName, RTN_SUCCESS, sErrDesc)

                    If p_iDebugMode = DEBUG_ON Then WriteToLogFile_Debug("Successfully Moved the CSV File to Success Folder : " & File.Name, sFuncName)

                    If p_iDebugMode = DEBUG_ON Then WriteToLogFile_Debug(" Calling Company Commit Function ", sFuncName)

                    Company_Commit(sErrDesc)

                End If
            Next

            Return RTN_SUCCESS

        Catch ex As Exception

            Call WriteToLogFile(ex.Message, sFuncName)
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed with ERROR", sFuncName)

            If p_iDebugMode = DEBUG_ON Then WriteToLogFile_Debug(" Calling Company Rollback Function ", sFuncName)

            Company_Rollback(sErrDesc)

            If p_iDebugMode = DEBUG_ON Then WriteToLogFile_Debug("Calling FileMoveToArchive for Moving CSV File to Failure Folder", sFuncName)

            FileMoveToArchive(oFileInfo, oFileFullName, RTN_ERROR, sErrDesc)

            If p_iDebugMode = DEBUG_ON Then WriteToLogFile_Debug("Successfully Moved the CSV File to Failure Folder : " & oFileInfo.Name, sFuncName)
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed with ERROR", sFuncName)

            Return RTN_ERROR

        End Try
    End Function

    Public Function ConnectToTargetCompany(ByRef oCompany As SAPbobsCOM.Company, _
                                             ByVal sEntity As String, ByRef sErrDesc As String) As Long

        ' **********************************************************************************
        '   Function    :   ConnectToTargetCompany()
        '   Purpose     :   This function will be providing to proceed the connectivity of 
        '                   using SAP DIAPI function
        '               
        '   Parameters  :   ByRef oCompany As SAPbobsCOM.Company
        '                       oCompany =  set the SAP DI Company Object
        '                   ByRef sErrDesc AS String 
        '                       sErrDesc = Error Description to be returned to calling function
        '               
        '   Return      :   0 - FAILURE
        '                   1 - SUCCESS
        '   Author      :   JOHN
        '   Date        :   MAY 2013 21
        ' **********************************************************************************

        Dim sFuncName As String = String.Empty
        Dim iRetValue As Integer = -1
        Dim iErrCode As Integer = -1
        Dim sSQL As String = String.Empty
        Dim oDs As New DataSet

        Try
            sFuncName = "ConnectToTargetCompany()"
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Starting function", sFuncName)

            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Initializing the Company Object", sFuncName)

            oCompany = New SAPbobsCOM.Company

            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Assigning the representing database name", sFuncName)

            oCompany.Server = p_oCompDef.p_sServerName
            oCompany.LicenseServer = p_oCompDef.p_sLicServerName
            oCompany.DbUserName = p_oCompDef.p_sDBUserName
            oCompany.DbPassword = p_oCompDef.p_sDBPassword
            oCompany.language = SAPbobsCOM.BoSuppLangs.ln_English
            oCompany.UseTrusted = False

            If p_oCompDef.p_sSQLType = 2012 Then
                oCompany.DbServerType = SAPbobsCOM.BoDataServerTypes.dst_MSSQL2012
            ElseIf p_oCompDef.p_sSQLType = 2008 Then
                oCompany.DbServerType = SAPbobsCOM.BoDataServerTypes.dst_MSSQL2008
            End If

            oCompany.CompanyDB = sEntity
            oCompany.UserName = p_oCompDef.p_sSAPUserName
            oCompany.Password = p_oCompDef.p_sSAPPassword

            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Connecting to the Company Database.", sFuncName)

            iRetValue = oCompany.Connect()

            If iRetValue <> 0 Then
                oCompany.GetLastError(iErrCode, sErrDesc)

                sErrDesc = String.Format("Connection to Database ({0}) {1} {2} {3}", _
                    oCompany.CompanyDB, System.Environment.NewLine, _
                                vbTab, sErrDesc)

                Throw New ArgumentException(sErrDesc)
            End If

            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed with SUCCESS", sFuncName)
            ConnectToTargetCompany = RTN_SUCCESS
        Catch ex As Exception
            sErrDesc = ex.Message
            Call WriteToLogFile(ex.Message, sFuncName)
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed with ERROR", sFuncName)
            ConnectToTargetCompany = RTN_ERROR
        End Try
    End Function

    Function GetCredeitCardDetails(ByRef sErrDesc As String) As Long

        ' **********************************************************************************
        '   Function    :   GetCredeitCardDetails()
        '   Purpose     :   This function will be providing the Credit Card Details.        '               
        '   Parameters  :   ByRef sErrDesc AS String 
        '                       sErrDesc = Error Description to be returned to calling function
        '   Return      :   0 - FAILURE
        '                   1 - SUCCESS
        '   Author      :   SRINIVASAN
        '   Date        :   AUG 2014 14
        ' **********************************************************************************

        Dim sFuncName As String = String.Empty
        Dim oRS As SAPbobsCOM.Recordset = oDICompany(1).GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
        Try
            sFuncName = "GetCredeitCardDetails()"

            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Starting function", sFuncName)

            oRS.DoQuery("SELECT T0.[Code], T0.[U_AE_CCNum], T0.[U_AE_CCValid],T0.[U_AE_Account] FROM [dbo].[@PAYMODE_ACCT_MAP]  T0 with (Nolock) ")

            AE_YESNATURALS_DLL.p_sCreditCard.Rows.Clear()

            If AE_YESNATURALS_DLL.p_sCreditCard.Columns.Contains("Code") = False Then
                AE_YESNATURALS_DLL.p_sCreditCard.Columns.Add("Code", GetType(String))
            End If
            If AE_YESNATURALS_DLL.p_sCreditCard.Columns.Contains("CCNumber") = False Then
                AE_YESNATURALS_DLL.p_sCreditCard.Columns.Add("CCNumber", GetType(String))

            End If
            If AE_YESNATURALS_DLL.p_sCreditCard.Columns.Contains("CCValid") = False Then
                AE_YESNATURALS_DLL.p_sCreditCard.Columns.Add("CCValid", GetType(String))
            End If

            If AE_YESNATURALS_DLL.p_sCreditCard.Columns.Contains("Account") = False Then
                AE_YESNATURALS_DLL.p_sCreditCard.Columns.Add("Account", GetType(String))
            End If



            If oRS.RecordCount > 0 Then
                For IRow As Integer = 1 To oRS.RecordCount
                    AE_YESNATURALS_DLL.p_sCreditCard.Rows.Add(oRS.Fields.Item(0).Value, oRS.Fields.Item(1).Value, oRS.Fields.Item(2).Value, oRS.Fields.Item(3).Value)
                    oRS.MoveNext()
                Next
            Else
                If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("There is No CreditCard Details in Table", sFuncName)
                Call WriteToLogFile("There is No CreditCard Details in Table", sFuncName)
                GetCredeitCardDetails = RTN_ERROR
                Exit Function
            End If

            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed with SUCCESS", sFuncName)

            GetCredeitCardDetails = RTN_SUCCESS
        Catch ex As Exception
            sErrDesc = ex.Message
            Call WriteToLogFile(ex.Message, sFuncName)
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed with ERROR", sFuncName)
            GetCredeitCardDetails = RTN_ERROR
        End Try

    End Function

    Function AR_Invoice(ByRef DataView_ As DataView, ByRef sErrDesc As String) As Long
        Dim sFuncName As String = String.Empty
        Dim p_oDVJE As DataView = Nothing
        Dim oDTDistinct As DataTable = Nothing
        Dim oDTRowFilter As DataTable = Nothing

        Dim oARInvoice As AE_YESNATURALS_DLL.clsARInvoice = New AE_YESNATURALS_DLL.clsARInvoice
        Try
            sFuncName = "AR_Invoice()"
            p_oDVJE = DataView_
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Starting function", sFuncName)
            oDTDistinct = p_oDVJE.Table.DefaultView.ToTable(True, "F3")

            For IntRow As Integer = 0 To oDTDistinct.Rows.Count - 1

                If String.IsNullOrEmpty(oDTDistinct.Rows(IntRow).Item(0).ToString) Then Continue For
                If oDTDistinct.Rows(IntRow).Item(0).ToString.ToUpper = "LOCATION/WH" Then Continue For

                p_oDVJE.RowFilter = "F3 = '" & oDTDistinct.Rows(IntRow).Item(0).ToString & "'"

                If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Calling ARInvoice() Function for Branch Code : " & oDTDistinct.Rows(IntRow).Item(0).ToString, sFuncName)

                P_sConString = String.Empty
                P_sConString = "Data Source=" & p_oCompDef.p_sServerName & ";Initial Catalog=" & p_oCompDef.p_sTradingDBName & ";User ID=" & p_oCompDef.p_sDBUserName & "; Password=" & p_oCompDef.p_sDBPassword

                If oARInvoice.ARInvoice(p_oDVJE, oDICompany(1), P_sConString, sErrDesc) <> RTN_SUCCESS Then Return RTN_ERROR

                If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed ARInvoice() Function for Branch Code : " & oDTDistinct.Rows(IntRow).Item(0).ToString, sFuncName)

            Next

            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed with SUCCESS", sFuncName)
            AR_Invoice = RTN_SUCCESS
        Catch ex As Exception
            sErrDesc = ex.Message
            Call WriteToLogFile(ex.Message, sFuncName)
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed with ERROR", sFuncName)
            AR_Invoice = RTN_ERROR

        End Try

    End Function

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

    Function DeliveryOrder_OLD(ByRef DataView_ As DataView, ByRef sErrDesc As String) As Long
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

            For IntRow As Integer = 0 To oDTDistinct.Rows.Count - 1

                If String.IsNullOrEmpty(oDTDistinct.Rows(IntRow).Item(0).ToString) Then Continue For
                If oDTDistinct.Rows(IntRow).Item(0).ToString.ToUpper = "LOCATION/WH" Then Continue For

                p_oDVJE.RowFilter = "F3 = '" & oDTDistinct.Rows(IntRow).Item(0).ToString & "'"

                If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Calling DeliveryOrder() Function for Branch Code : " & oDTDistinct.Rows(IntRow).Item(0).ToString, sFuncName)

                ''If oDeliveryOrder.DeliveryOrder(p_oDVJE, oDICompany(0), sErrDesc) <> RTN_SUCCESS Then Return RTN_ERROR
                If oDeliveryOrder.DeliveryOrder(p_oDVJE, oDICompany(0), sErrDesc) <> RTN_SUCCESS Then
                    bSuccess = False
                    If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed with ERROR. Ref Location : " & oDTDistinct.Rows(IntRow).Item(0).ToString, sFuncName)
                End If

                If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed DeliveryOrder() Function for Branch Code : " & oDTDistinct.Rows(IntRow).Item(0).ToString, sFuncName)

            Next
            If bSuccess = False Then Throw New ArgumentException(sErrDesc)

            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed with SUCCESS", sFuncName)

            DeliveryOrder_OLD = RTN_SUCCESS
        Catch ex As Exception
            sErrDesc = ex.Message
            Call WriteToLogFile(ex.Message, sFuncName)
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed with ERROR", sFuncName)
            DeliveryOrder_OLD = RTN_ERROR

        End Try

    End Function

    Public Sub FileMoveToArchive(ByVal oFile As System.IO.FileInfo, ByVal CurrFileToUpload As String, ByVal iStatus As Integer, ByVal sErrDesc As String)

        'Event      :   FileMoveToArchive
        'Purpose    :   For Renaming the file with current time stamp & moving to archive folder
        'Author     :   JOHN 
        'Date       :   21 MAY 2014

        Dim sFuncName As String = String.Empty

        Try
            sFuncName = "FileMoveToArchive"

            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Starting Function ", sFuncName)

            Dim RenameCurrFileToUpload As String = Mid(oFile.Name, 1, oFile.Name.Length - 4) & "_" & Now.ToString("yyyyMMddhhmmss") & ".csv"

            If iStatus = RTN_SUCCESS Then

                If p_iDebugMode = DEBUG_ON Then WriteToLogFile_Debug("Moving CSV file to success folder", sFuncName)
                oFile.MoveTo(p_oCompDef.p_sSuccessDir & "\" & RenameCurrFileToUpload)
            Else
                If p_iDebugMode = DEBUG_ON Then WriteToLogFile_Debug("Moving CSV file to Fail folder", sFuncName)
                oFile.MoveTo(p_oCompDef.p_sFailDir & "\" & RenameCurrFileToUpload)
            End If
        Catch ex As Exception

            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Error in renaming/copying/moving", sFuncName)
            Call WriteToLogFile(ex.Message, sFuncName)
        End Try
    End Sub

End Class
