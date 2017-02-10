Imports System.Configuration
Imports System.Data.SqlClient
Imports System.Net.Mail
Imports System.Net.Mime

Module modCommon

    Public Function GetSystemIntializeInfo(ByRef oCompDef As CompanyDefault, ByRef sErrDesc As String) As Long

        ' **********************************************************************************
        '   Function    :   GetSystemIntializeInfo()
        '   Purpose     :   This function will be providing information about the initialing variables
        '               
        '   Parameters  :   ByRef oCompDef As CompanyDefault
        '                       oCompDef =  set the Company Default structure
        '                   ByRef sErrDesc AS String 
        '                       sErrDesc = Error Description to be returned to calling function
        '               
        '   Return      :   0 - FAILURE
        '                   1 - SUCCESS
        '   Author      :   JOHN
        '   Date        :   MAY 2014
        ' **********************************************************************************

        Dim sFuncName As String = String.Empty

        Try

            sFuncName = "GetSystemIntializeInfo()"

            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Starting Function", sFuncName)


            oCompDef.p_sServerName = String.Empty
            oCompDef.p_sLicServerName = String.Empty
            oCompDef.p_sDBUserName = String.Empty
            oCompDef.p_sDBPassword = String.Empty

            oCompDef.p_sDataBaseName = String.Empty
            oCompDef.p_sSAPUserName = String.Empty
            oCompDef.p_sSAPPassword = String.Empty

            oCompDef.p_sInputDir = String.Empty
            oCompDef.p_sOutputDir = String.Empty
            oCompDef.p_sFailDir = String.Empty
            oCompDef.p_sLogDir = String.Empty
            oCompDef.p_sSuccessDir = String.Empty

            oCompDef.p_sDebug = String.Empty
            oCompDef.p_sCardCode = String.Empty
            oCompDef.p_sTradingDBName = String.Empty
            oCompDef.p_sRetailDBName = String.Empty
            oCompDef.p_sPriceList = String.Empty

            p_oCompDef.p_sQuery = String.Empty

            If Not String.IsNullOrEmpty(ConfigurationManager.AppSettings("Server")) Then
                oCompDef.p_sServerName = ConfigurationManager.AppSettings("Server")
            End If

            If Not String.IsNullOrEmpty(ConfigurationManager.AppSettings("LicenseServer")) Then
                oCompDef.p_sLicServerName = ConfigurationManager.AppSettings("LicenseServer")
            End If

            If Not String.IsNullOrEmpty(ConfigurationManager.AppSettings("TradingDBName")) Then
                oCompDef.p_sTradingDBName = ConfigurationManager.AppSettings("TradingDBName")
                AE_YESNATURALS_DLL.sTradingDB = oCompDef.p_sTradingDBName
            End If

            If Not String.IsNullOrEmpty(ConfigurationManager.AppSettings("RetailDBName")) Then
                oCompDef.p_sRetailDBName = ConfigurationManager.AppSettings("RetailDBName")
                AE_YESNATURALS_DLL.sRetailDB = oCompDef.p_sRetailDBName
            End If

            If Not String.IsNullOrEmpty(ConfigurationManager.AppSettings("SAPUserName")) Then
                oCompDef.p_sSAPUserName = ConfigurationManager.AppSettings("SAPUserName")
            End If

            If Not String.IsNullOrEmpty(ConfigurationManager.AppSettings("SAPPassword")) Then
                oCompDef.p_sSAPPassword = ConfigurationManager.AppSettings("SAPPassword")
            End If


            If Not String.IsNullOrEmpty(ConfigurationManager.AppSettings("DBUser")) Then
                oCompDef.p_sDBUserName = ConfigurationManager.AppSettings("DBUser")
            End If

            If Not String.IsNullOrEmpty(ConfigurationManager.AppSettings("DBPwd")) Then
                oCompDef.p_sDBPassword = ConfigurationManager.AppSettings("DBPwd")
            End If

            If Not String.IsNullOrEmpty(ConfigurationManager.AppSettings("SQLType")) Then
                oCompDef.p_sSQLType = ConfigurationManager.AppSettings("SQLType")
            End If

            ' folder
            If Not String.IsNullOrEmpty(ConfigurationManager.AppSettings("InboxDir")) Then
                oCompDef.p_sInputDir = ConfigurationManager.AppSettings("InboxDir")
            End If

            If Not String.IsNullOrEmpty(ConfigurationManager.AppSettings("OutboxDir")) Then
                oCompDef.p_sOutputDir = ConfigurationManager.AppSettings("OutboxDir")
                AE_YESNATURALS_DLL.sOutputPath = oCompDef.p_sOutputDir
            End If

            If Not String.IsNullOrEmpty(ConfigurationManager.AppSettings("SuccessDir")) Then
                oCompDef.p_sSuccessDir = ConfigurationManager.AppSettings("SuccessDir")
            End If

            If Not String.IsNullOrEmpty(ConfigurationManager.AppSettings("FailDir")) Then
                oCompDef.p_sFailDir = ConfigurationManager.AppSettings("FailDir")
            End If

            If Not String.IsNullOrEmpty(ConfigurationManager.AppSettings("LogDir")) Then
                oCompDef.p_sLogDir = ConfigurationManager.AppSettings("LogDir")
                AE_YESNATURALS_DLL.sLogFolderPath = oCompDef.p_sLogDir
            End If

            If Not String.IsNullOrEmpty(ConfigurationManager.AppSettings("CardCode")) Then
                oCompDef.p_sCardCode = ConfigurationManager.AppSettings("CardCode")
                AE_YESNATURALS_DLL.sCardCode = oCompDef.p_sCardCode
            End If



            If Not String.IsNullOrEmpty(ConfigurationManager.AppSettings("PriceList")) Then
                oCompDef.p_sPriceList = ConfigurationManager.AppSettings("PriceList")
            End If

            If Not String.IsNullOrEmpty(ConfigurationManager.AppSettings("Debug")) Then
                oCompDef.p_sDebug = ConfigurationManager.AppSettings("Debug")
                If p_oCompDef.p_sDebug.ToUpper = "ON" Then
                    p_iDebugMode = 1
                    AE_YESNATURALS_DLL.p_iDebugMode = 1
                Else
                    p_iDebugMode = 0
                    AE_YESNATURALS_DLL.p_iDebugMode = 0
                End If
            End If


            Console.WriteLine("Completed with SUCCESS ", sFuncName)
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed with SUCCESS", sFuncName)
            GetSystemIntializeInfo = RTN_SUCCESS

        Catch ex As Exception
            WriteToLogFile(ex.Message, sFuncName)
            Console.WriteLine("Completed with ERROR ", sFuncName)
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed with ERROR", sFuncName)
            GetSystemIntializeInfo = RTN_ERROR
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

    Public Sub FileMoveToArchive(ByVal oFile As System.IO.FileInfo, ByVal CurrFileToUpload As String, ByVal iStatus As Integer, ByVal sErrDesc As String)

        'Event      :   FileMoveToArchive
        'Purpose    :   For Renaming the file with current time stamp & moving to archive folder
        'Author     :   JOHN 
        'Date       :   21 MAY 2014

        Dim sFuncName As String = String.Empty

        Try
            sFuncName = "FileMoveToArchive"
            Console.WriteLine("Starting Function ", sFuncName)
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Starting Function ", sFuncName)
            'Dim RenameCurrFileToUpload = Replace(CurrFileToUpload.ToUpper, ".CSV", "") & "_" & Format(Now, "yyyyMMddHHmmss") & ".csv"
            Dim RenameCurrFileToUpload As String = Mid(oFile.Name, 1, oFile.Name.Length - 4) & "_" & Now.ToString("yyyyMMddhhmmss") & ".csv"

            If iStatus = RTN_SUCCESS Then

                If p_iDebugMode = DEBUG_ON Then WriteToLogFile_Debug("Moving CSV file to success folder", sFuncName)
                oFile.MoveTo(p_oCompDef.p_sSuccessDir & "\" & RenameCurrFileToUpload)
            Else

                If p_iDebugMode = DEBUG_ON Then WriteToLogFile_Debug("Moving CSV file to Fail folder", sFuncName)
                oFile.MoveTo(p_oCompDef.p_sFailDir & "\" & RenameCurrFileToUpload)
            End If
        Catch ex As Exception
            Console.WriteLine("Error in renaming/copying/moving ", sFuncName)
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Error in renaming/copying/moving", sFuncName)
            Call WriteToLogFile(ex.Message, sFuncName)
        End Try
    End Sub

    Public Sub Company_Commit(ByRef sErrDesc As String)

        ' **********************************************************************************
        '   Function    :   Company_Commit()
        '   Purpose     :   This Subroutine for Committing the Transaction          
        '   Parameters  :   ByRef sErrDesc AS String 
        '                       sErrDesc = Error Description to be returned to calling function
        '   Author      :   SRINIVASAN
        '   Date        :  22.09.14
        ' **********************************************************************************

        Dim sFuncName As String = String.Empty
        Dim ofrmARInvoice As frmARInvoice = New frmARInvoice

        Try
            sFuncName = "Company_Commit()"

            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Starting function", sFuncName)

            For iTransCount As Integer = 0 To 1
                If Not oDICompany(iTransCount) Is Nothing Then
                    If oDICompany(iTransCount).Connected = True Then
                        If oDICompany(iTransCount).InTransaction = True Then
                            ofrmARInvoice.WriteToStatusScreen(False, sFuncName & " : Commit Transaction on Company Database " & oDICompany(iTransCount).CompanyDB)
                            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Commit Transaction on Company Database " & oDICompany(iTransCount).CompanyDB, sFuncName)
                            oDICompany(iTransCount).EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_Commit)
                        End If
                        ofrmARInvoice.WriteToStatusScreen(False, sFuncName & " : Disconnecting Company Database " & oDICompany(iTransCount).CompanyDB)
                        If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Disconnecting Company Database " & oDICompany(iTransCount).CompanyDB, sFuncName)
                        oDICompany(iTransCount).Disconnect()
                        oDICompany(iTransCount) = Nothing
                    End If
                End If
            Next

            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed with SUCCESS", sFuncName)

        Catch ex As Exception

            Call WriteToLogFile(ex.Message, sFuncName)
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed with ERROR", sFuncName)

        End Try
    End Sub

    Public Sub Company_Rollback(ByRef sErrDesc As String)

        ' **********************************************************************************
        '   Function    :   Company_Rollback()
        '   Purpose     :   This Subroutine for Rollback the Transactions        
        '   Parameters  :   ByRef sErrDesc AS String 
        '                       sErrDesc = Error Description to be returned to calling function
        '   Author      :   SRINIVASAN
        '   Date        :  22.09.14
        ' **********************************************************************************

        Dim sFuncName As String = String.Empty
        Dim ofrmARInvoice As frmARInvoice = New frmARInvoice

        Try
            sFuncName = "Company_Rollback()"

            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Starting function", sFuncName)

            For iTransCount As Integer = 0 To 1
                If Not oDICompany(iTransCount) Is Nothing Then
                    If oDICompany(iTransCount).Connected = True Then
                        If oDICompany(iTransCount).InTransaction = True Then
                            ofrmARInvoice.WriteToStatusScreen(False, sFuncName & " : Rollback Transaction on Company Database " & oDICompany(iTransCount).CompanyDB)
                            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Rollback Transaction on Company Database " & oDICompany(iTransCount).CompanyDB, sFuncName)
                            oDICompany(iTransCount).EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_RollBack)
                        End If
                        ofrmARInvoice.WriteToStatusScreen(False, sFuncName & " : Disconnecting Company Database " & oDICompany(iTransCount).CompanyDB)
                        If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Disconnecting Company Database " & oDICompany(iTransCount).CompanyDB, sFuncName)
                        oDICompany(iTransCount).Disconnect()
                        oDICompany(iTransCount) = Nothing
                    End If
                End If
            Next

            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed with SUCCESS", sFuncName)

        Catch ex As Exception

            Call WriteToLogFile(ex.Message, sFuncName)
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed with ERROR", sFuncName)

        End Try
    End Sub

End Module
