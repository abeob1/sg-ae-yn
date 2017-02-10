Imports System.Configuration

Module ModCommon

    Public Structure CompanyDefault

        Public p_sServerName As String
        Public p_sLicServerName As String
        Public p_sDataBaseName As String
        Public p_sDBUserName As String
        Public p_sDBPassword As String
        Public p_sSAPUserName As String
        Public p_sSAPPassword As String
        Public p_sSQLType As Int16

        Public p_sTradingDBName As String
        Public p_sRetailDBName As String

        Public p_sInputDir As String
        Public p_sOutputDir As String
        Public p_sSuccessDir As String
        Public p_sFailDir As String
        Public p_sLogDir As String
        Public p_sDebug As String
        Public p_sCardCode As String
        Public p_sPriceList As String

        Public p_sItemMasterExport As String
        Public p_sInvoice_IncomingPayment As String
        Public p_sInvoiceSerStartTime As String
        Public p_sItemSerStartTime As String

        Public p_sQuery As String


    End Structure


    Public Const RTN_SUCCESS As Int16 = 1
    Public Const RTN_ERROR As Int16 = 0
    ' Debug Value Variable Control
    Public Const DEBUG_ON As Int16 = 1
    Public Const DEBUG_OFF As Int16 = 0

    ' Global variables group
    Public p_iDebugMode As Int16
    Public p_iErrDispMethod As Int16
    Public p_iDeleteDebugLog As Int16
    Public p_oCompDef As CompanyDefault


    Public p_oTradingCompany As SAPbobsCOM.Company
    Public p_oRetailCompany As SAPbobsCOM.Company
    Public p_oDataView As DataView = Nothing
    Public P_sConString As String = String.Empty

    Public oDICompany(2) As SAPbobsCOM.Company

    Public p_sCCDetails(100, 3) As String
    Public p_iArrayCount As Int16
    Public p_sCFLValue As String = String.Empty
    Public ClsItemmaster As New ClsItemMasterExport

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

            oCompDef.p_sItemSerStartTime = String.Empty
            oCompDef.p_sInvoice_IncomingPayment = String.Empty
            oCompDef.p_sItemSerStartTime = String.Empty
            oCompDef.p_sInvoiceSerStartTime = String.Empty
            oCompDef.p_sPriceList = String.Empty

            If Not String.IsNullOrEmpty(ConfigurationManager.AppSettings("Server")) Then
                oCompDef.p_sServerName = ConfigurationManager.AppSettings("Server")
            End If

            If Not String.IsNullOrEmpty(ConfigurationManager.AppSettings("LicenseServer")) Then
                oCompDef.p_sLicServerName = ConfigurationManager.AppSettings("LicenseServer")
            End If

            If Not String.IsNullOrEmpty(ConfigurationManager.AppSettings("TradingDBName")) Then
                oCompDef.p_sTradingDBName = ConfigurationManager.AppSettings("TradingDBName")
            End If

            If Not String.IsNullOrEmpty(ConfigurationManager.AppSettings("RetailDBName")) Then
                oCompDef.p_sRetailDBName = ConfigurationManager.AppSettings("RetailDBName")
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


            If Not String.IsNullOrEmpty(ConfigurationManager.AppSettings("Service_ItemMasterExport")) Then
                oCompDef.p_sItemMasterExport = ConfigurationManager.AppSettings("Service_ItemMasterExport")
            End If

            If Not String.IsNullOrEmpty(ConfigurationManager.AppSettings("Service_Invoice_IncomingPayments")) Then
                oCompDef.p_sInvoice_IncomingPayment = ConfigurationManager.AppSettings("Service_Invoice_IncomingPayments")
            End If

            If Not String.IsNullOrEmpty(ConfigurationManager.AppSettings("ItemSerStartTime")) Then
                oCompDef.p_sItemSerStartTime = ConfigurationManager.AppSettings("ItemSerStartTime")
            End If


            If Not String.IsNullOrEmpty(ConfigurationManager.AppSettings("InvoiceSerStartTime")) Then
                oCompDef.p_sInvoiceSerStartTime = ConfigurationManager.AppSettings("InvoiceSerStartTime")
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

        Try
            sFuncName = "Company_Commit()"

            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Starting function", sFuncName)

            For iTransCount As Integer = 0 To 1
                If Not oDICompany(iTransCount) Is Nothing Then
                    If oDICompany(iTransCount).Connected = True Then
                        If oDICompany(iTransCount).InTransaction = True Then
                            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Commit Transaction on Company Database " & oDICompany(iTransCount).CompanyDB, sFuncName)
                            oDICompany(iTransCount).EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_Commit)
                        End If
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

        Try
            sFuncName = "Company_Rollback()"

            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Starting function", sFuncName)

            For iTransCount As Integer = 0 To 1
                If Not oDICompany(iTransCount) Is Nothing Then
                    If oDICompany(iTransCount).Connected = True Then
                        If oDICompany(iTransCount).InTransaction = True Then
                            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Rollback Transaction on Company Database " & oDICompany(iTransCount).CompanyDB, sFuncName)
                            oDICompany(iTransCount).EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_RollBack)
                        End If
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
