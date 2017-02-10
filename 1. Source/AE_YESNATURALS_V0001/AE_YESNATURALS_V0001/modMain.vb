Module modMain

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

        Public p_sQuery As String
    End Structure


    ' Return Value Variable Control
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

    Sub Main()

        Dim sFuncName As String = String.Empty
        Dim sErrDesc As String = String.Empty
        Dim sQuery As String = String.Empty
        Dim oCommon As AE_YESNATURALS_DLL.clsCommon = New AE_YESNATURALS_DLL.clsCommon
        Dim oItemMaster As AE_YESNATURALS_DLL.clsItemMaster = New AE_YESNATURALS_DLL.clsItemMaster
        Try
            sFuncName = "Main()"
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Starting Function", sFuncName)

            'Getting the Parameter Values from App Cofig File
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Calling GetSystemIntializeInfo()", sFuncName)
            If GetSystemIntializeInfo(p_oCompDef, sErrDesc) <> RTN_SUCCESS Then Throw New ArgumentException(sErrDesc)

            'Function to connect the Company
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Calling ConnectToTargetCompany()", sFuncName)
            If ConnectToTargetCompany(p_oTradingCompany, p_oCompDef.p_sTradingDBName, sErrDesc) <> RTN_SUCCESS Then Throw New ArgumentException(sErrDesc)

            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Calling ExportToCSV()", sFuncName)
            sQuery = p_oCompDef.p_sQuery

            sQuery += " and (case when isnull(T0.UpdateDate,'')='' then T0.CreateDate else T0.UpdateDate end)=convert(date,Getdate(),108)"

            P_sConString = String.Empty
            P_sConString = "Data Source=" & p_oCompDef.p_sServerName & ";Initial Catalog=" & p_oCompDef.p_sTradingDBName & ";User ID=" & p_oCompDef.p_sDBUserName & "; Password=" & p_oCompDef.p_sDBPassword

            If oItemMaster.ExportToCSV(sQuery, P_sConString, sErrDesc) = RTN_ERROR Then Throw New ArgumentException(sErrDesc)

            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed With SUCCESS ExportToCSV()", sFuncName)


            ' ''Function to Read and Store the CreditCard Details in Array Variable
            ''If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Calling GetCredeitCardDetails()", sFuncName)

            ''p_iArrayCount = 0
            ''If GetCredeitCardDetails(sErrDesc) <> RTN_SUCCESS Then Throw New ArgumentException(sErrDesc)


            ' ''Find the Files placed in that Folder
            ''Dim DirInfo As New System.IO.DirectoryInfo(p_oCompDef.p_sInputDir)
            ''Dim files() As System.IO.FileInfo

            ''If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Calling Identify the CSV File ", sFuncName)
            ''files = DirInfo.GetFiles("*_POS.csv")
            ''If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed with SUCCESS Identify the CSV File ", sFuncName)

            ''For Each File As System.IO.FileInfo In files

            ''    If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Calling GetDataViewFromCSV()", sFuncName)

            ''    p_oDataView = oCommon.GetDataViewFromCSV(p_oCompDef.p_sInputDir & "\" & File.Name)

            ''    If p_oDataView Is Nothing Then
            ''        If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("No Datas in the CSV file", sFuncName)
            ''        If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed with SUCCESS GetDataViewFromCSV()", sFuncName)
            ''        Exit Sub
            ''    Else
            ''        If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed with SUCCESS GetDataViewFromCSV()", sFuncName)

            ''        If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Start the Transaction ", sFuncName)

            ''        If p_oCompany.InTransaction = False Then p_oCompany.StartTransaction()

            ''        If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Calling ARInvoice()", sFuncName)

            ''        If AR_Invoice(p_oDataView, sErrDesc) <> RTN_SUCCESS Then Throw New ArgumentException(sErrDesc)

            ''        If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed with SUCCESS ARInvoice()", sFuncName)

            ''        If p_oCompany.InTransaction = True Then p_oCompany.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_Commit)

            ''        If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Committed the Transaction ", sFuncName)

            ''    End If


            ''Next
        Catch ex As Exception
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed with ERROR", sFuncName)
            Call WriteToLogFile(ex.Message, sFuncName)
        End Try
    End Sub

End Module
