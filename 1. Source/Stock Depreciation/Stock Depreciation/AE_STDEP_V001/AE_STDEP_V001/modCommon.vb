Imports System.Configuration
Imports System.Data.SqlClient
Imports System.Net.Mail
Imports System.Net.Mime
Imports System.IO
Imports System.Text





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
        Dim sConnection As String = String.Empty
        Dim sSqlstr As String = String.Empty
        Try

            sFuncName = "GetSystemIntializeInfo()"
            Console.WriteLine("Starting Function", sFuncName)
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Starting Function", sFuncName)

            oCompDef.sDBName = String.Empty
            oCompDef.sServer = String.Empty
            oCompDef.sLicenseServer = String.Empty
            oCompDef.iServerLanguage = 3
            oCompDef.iServerType = 7
            oCompDef.sSAPUser = String.Empty
            oCompDef.sSAPPwd = String.Empty
            oCompDef.sSAPDBName = String.Empty
            oCompDef.sLogPath = String.Empty
            oCompDef.sDebug = String.Empty


            If Not String.IsNullOrEmpty(ConfigurationManager.AppSettings("Server")) Then
                oCompDef.sServer = ConfigurationManager.AppSettings("Server")
            End If

            If Not String.IsNullOrEmpty(ConfigurationManager.AppSettings("SQLType")) Then
                oCompDef.iServerType = ConfigurationManager.AppSettings("SQLType")
            End If

            If Not String.IsNullOrEmpty(ConfigurationManager.AppSettings("LicenseServer")) Then
                oCompDef.sLicenseServer = ConfigurationManager.AppSettings("LicenseServer")
            End If

            If Not String.IsNullOrEmpty(ConfigurationManager.AppSettings("SAPDBName")) Then
                oCompDef.sSAPDBName = ConfigurationManager.AppSettings("SAPDBName")
            End If

            If Not String.IsNullOrEmpty(ConfigurationManager.AppSettings("SAPUserName")) Then
                oCompDef.sSAPUser = ConfigurationManager.AppSettings("SAPUserName")
            End If

            If Not String.IsNullOrEmpty(ConfigurationManager.AppSettings("SAPPassword")) Then
                oCompDef.sSAPPwd = ConfigurationManager.AppSettings("SAPPassword")
            End If

            If Not String.IsNullOrEmpty(ConfigurationManager.AppSettings("DBUser")) Then
                oCompDef.sDBUser = ConfigurationManager.AppSettings("DBUser")
            End If

            If Not String.IsNullOrEmpty(ConfigurationManager.AppSettings("DBPwd")) Then
                oCompDef.sDBPwd = ConfigurationManager.AppSettings("DBPwd")
            End If

            If Not String.IsNullOrEmpty(ConfigurationManager.AppSettings("LogPath")) Then
                oCompDef.sLogPath = ConfigurationManager.AppSettings("LogPath")
            End If

            If Not String.IsNullOrEmpty(ConfigurationManager.AppSettings("Debug")) Then
                oCompDef.sDebug = ConfigurationManager.AppSettings("Debug")
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


    Public Function ExecuteSQLQuery_DT(ByRef sErrDesc As String) As DataSet

        '**************************************************************
        ' Function      : ExecuteQuery
        ' Purpose       : Execute SQL
        ' Parameters    : ByVal sSQL - string command Text
        ' Author        : JOHN
        ' Date          : MAY 2014 20
        ' Change        :
        '**************************************************************

        Dim sConstr As String = "Data Source=" & p_oCompDef.sServer & ";Initial Catalog=" & p_oCompDef.sSAPDBName & ";User ID=" & p_oCompDef.sDBUser & "; Password=" & p_oCompDef.sDBPwd

        Dim oCon As New SqlConnection(sConstr)
        Dim oCmd As New SqlCommand
        Dim oDs As New DataSet
        Dim sFuncName As String = String.Empty
        Dim sDate As String = String.Empty

        Dim oDT_Status As New DataTable
        Try
            sFuncName = "ExecExecuteSQLQuery_DT()"
            Console.WriteLine("Starting Fucntion.. ", sFuncName)
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Starting Fucntion...", sFuncName)
            sDate = Format(Now.Date, "yyyy-MM-dd")

            Dim sValidation_Query As String = "SELECT isnull(T0.[U_AE_Stat],'') [U_AE_Stat] FROM [dbo].[@AE_STKDEP_REG]  T0 WHERE T0.[U_AE_Month] = left(datename(month,'" & sDate & "'),3) " & _
                "and  T0.[U_AE_Year] = year('" & sDate & "') group by T0.[U_AE_Stat] "

            Dim sStockDep_SP As String = "[AB_SP001_StockDepreciation]'" & sDate & "'"
            Dim sGL_SP As String = "SELECT Code, Name  FROM [dbo].[@AE_DEPN_STOCK_ACC]  T0"

            oCon.Open()
            oCmd.CommandType = CommandType.Text
            oCmd.CommandText = sValidation_Query
            oCmd.Connection = oCon
            oCmd.CommandTimeout = 0
            Dim da As New SqlDataAdapter(oCmd)
            'oCmd.ExecuteNonQuery()
            da.Fill(oDT_Status)
            da.Dispose()

            'MsgBox(Month(Convert.ToDateTime(sDate)))

            If oDT_Status.Rows.Count = 1 Then
                If oDT_Status.Rows(0).Item(0).ToString = "C" Then
                    Console.WriteLine("Depreciation Aleardy Done for this Month .. " & MonthName(Month(Convert.ToDateTime(sDate)), True) & " - " & Year(Convert.ToDateTime(sDate)), sFuncName)
                    If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Depreciation Aleardy Done for this Month .. " & MonthName(Month(sDate), True) & " - " & Year(sDate), sFuncName)
                    sErrDesc = "Depreciation Aleardy Done for this Month"
                    Return Nothing

                ElseIf oDT_Status.Rows(0).Item(0).ToString = "O" Then
                    oCmd.CommandText = "Select * from [@AE_STKDEP_REG]"
                End If
            ElseIf oDT_Status.Rows.Count = 0 Then
                oCmd.CommandText = sStockDep_SP
            End If

            oCmd.CommandType = CommandType.Text
            oCmd.Connection = oCon
            oCmd.CommandTimeout = 0
            da = New SqlDataAdapter(oCmd)
            da.Fill(oDs.Tables.Add("Stock Depreciation"))
            da.Dispose()


            oCmd.CommandType = CommandType.Text
            oCmd.CommandText = sGL_SP
            oCmd.Connection = oCon
            oCmd.CommandTimeout = 0
            da = New SqlDataAdapter(oCmd)
            oCmd.ExecuteNonQuery()
            ''da1.Fill(oDs.Tables(1))
            da.Fill(oDs.Tables.Add("GL Account"))
            oCon.Dispose()
            oCmd.Dispose()
            da.Dispose()
            Console.WriteLine("Completed Successfully. ", sFuncName)
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed Successfully.", sFuncName)

            Return oDs
            sErrDesc = ""
        Catch ex As Exception
            sErrDesc = ex.Message
            WriteToLogFile(ex.Message, sFuncName)
            Console.WriteLine("Completed with ERROR ", sFuncName)
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed with ERROR", sFuncName)
            Throw New Exception(ex.Message)
        Finally
            oCon.Dispose()
        End Try

    End Function


    ''Public Function IdentifyCSVFile_JournalEntry(ByRef sErrDesc As String) As Long

    ''    ' **********************************************************************************
    ''    '   Function    :   IdentifyCSVFile_JournalEntry()
    ''    '   Purpose     :   This function will identify the CSV file of Journal Entry
    ''    '                    Upload the file into Dataview and provide the information to post transaction in SAP.
    ''    '                     Transaction Success : Move the CSV file to SUCESS folder
    ''    '                     Transaction Fail :    Move the CSV file to FAIL folder and send Error notification to concern person
    ''    '               
    ''    '   Parameters  :   ByRef sErrDesc AS String 
    ''    '                       sErrDesc = Error Description to be returned to calling function
    ''    '               
    ''    '   Return      :   0 - FAILURE
    ''    '                   1 - SUCCESS
    ''    '   Author      :   JOHN
    ''    '   Date        :   MAY 2014 20
    ''    ' **********************************************************************************


    ''    Dim sSqlstr As String = String.Empty
    ''    Dim bJEFileExist As Boolean
    ''    Dim sFileType As String = String.Empty
    ''    Dim oDTDistinct As DataTable = Nothing
    ''    Dim oDTRowFilter As DataTable = Nothing
    ''    Dim oDVJE As DataView = Nothing
    ''    Dim oDICompany() As SAPbobsCOM.Company = Nothing

    ''    Dim sFuncName As String = String.Empty

    ''    Try
    ''        sFuncName = "IdentifyCSVFile_JournalEntry()"
    ''        Console.WriteLine("Starting Function", sFuncName)
    ''        If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Starting Function ", sFuncName)



    ''            Console.WriteLine("GetDataViewFromCSV() ", sFuncName)
    ''            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("GetDataViewFromCSV() ", sFuncName)
    ''        ' oDVJE = GetDataViewFromCSV(File.FullName, File.Name)

    ''            Console.WriteLine("Getting Distinct of Entity", sFuncName)
    ''        If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Getting Distinct of Item Group ", sFuncName)
    ''            oDTDistinct = oDVJE.Table.DefaultView.ToTable(True, "F1")
    ''            ReDim oDICompany(oDTDistinct.Rows.Count)

    ''            For imjs As Integer = 1 To oDTDistinct.Rows.Count - 1

    ''                oDICompany(imjs) = New SAPbobsCOM.Company

    ''                Console.WriteLine("Calling ConnectToTargetCompany()", sFuncName)
    ''                If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Calling ConnectToTargetCompany()", sFuncName)
    ''                If ConnectToTargetCompany(oDICompany(imjs), oDTDistinct.Rows(imjs).Item(0).ToString, sErrDesc) <> RTN_SUCCESS Then
    ''                    Throw New ArgumentException(sErrDesc)
    ''                End If

    ''                Console.WriteLine("Starting transaction on company database " & oDICompany(imjs).CompanyDB, sFuncName)
    ''                If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Starting transaction on company database " & oDICompany(imjs).CompanyDB, sFuncName)
    ''                oDICompany(imjs).StartTransaction()


    ''                Console.WriteLine("Filtering data with respective Entity -  " & oDTDistinct.Rows(imjs).Item(0).ToString, sFuncName)
    ''                If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Filtering data with respective Entity -  " & oDTDistinct.Rows(imjs).Item(0).ToString, sFuncName)
    ''                oDVJE.RowFilter = "F1 = '" & oDTDistinct.Rows(imjs).Item(0).ToString & "'"

    ''                Console.WriteLine("Calling Function JournalEntry_Posting() ", sFuncName)
    ''                If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Calling Function JournalEntry_Posting() ", sFuncName)

    ''                If JournalEntry_Posting(oDVJE, oDICompany(imjs), sErrDesc) <> RTN_SUCCESS Then
    ''                    For lCounter As Integer = 0 To UBound(oDICompany)
    ''                        If Not oDICompany(lCounter) Is Nothing Then
    ''                            If oDICompany(lCounter).Connected = True Then
    ''                                If oDICompany(lCounter).InTransaction = True Then
    ''                                    If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Rollback transaction on company database " & oDICompany(lCounter).CompanyDB, sFuncName)
    ''                                    oDICompany(lCounter).EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_RollBack)
    ''                                End If
    ''                                If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Disconnecting company database " & oDICompany(lCounter).CompanyDB, sFuncName)
    ''                                oDICompany(lCounter).Disconnect()
    ''                                oDICompany(lCounter) = Nothing
    ''                            End If
    ''                        End If
    ''                    Next

    ''                    Console.WriteLine("Calling FileMoveToArchive for moving CSV file to archive folder ", sFuncName)
    ''                    If p_iDebugMode = DEBUG_ON Then WriteToLogFile_Debug("Calling FileMoveToArchive for moving CSV file to archive folder", sFuncName)
    ''                    'AddDataToTable(p_oDtError, File.Name, "Error", sErrDesc)
    ''                ' FileMoveToArchive(File, File.FullName, RTN_ERROR, p_sJournalEntryError)

    ''                    Console.WriteLine("Error in updation. RollBack executed for ", sFuncName)
    ''                    If p_iDebugMode = DEBUG_ON Then WriteToLogFile_Debug("Error in updation. RollBack executed for " & File.FullName, sFuncName)
    ''                    IdentifyCSVFile_JournalEntry = RTN_ERROR
    ''                    Exit Function

    ''                Else
    ''                    Console.WriteLine("Calling FileMoveToArchive for moving CSV file to archive folder", sFuncName)
    ''                    If p_iDebugMode = DEBUG_ON Then WriteToLogFile_Debug("Calling FileMoveToArchive for moving CSV file to archive folder", sFuncName)
    ''                '  FileMoveToArchive(File, File.FullName, RTN_SUCCESS, "")
    ''                End If
    ''            Next imjs
    ''        Next

    ''        If bJEFileExist = True Then
    ''            For lCounter As Integer = 0 To UBound(oDICompany)
    ''                If Not oDICompany(lCounter) Is Nothing Then
    ''                    If oDICompany(lCounter).Connected = True Then
    ''                        If oDICompany(lCounter).InTransaction = True Then
    ''                            Console.WriteLine("Commit transaction on company database " & oDICompany(lCounter).CompanyDB, sFuncName)
    ''                            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Commit transaction on company database " & oDICompany(lCounter).CompanyDB, sFuncName)
    ''                            oDICompany(lCounter).EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_Commit)
    ''                        End If
    ''                        Console.WriteLine("Disconnecting company database " & oDICompany(lCounter).CompanyDB, sFuncName)
    ''                        If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Disconnecting company database " & oDICompany(lCounter).CompanyDB, sFuncName)
    ''                        oDICompany(lCounter).Disconnect()
    ''                        oDICompany(lCounter) = Nothing
    ''                    End If
    ''                End If
    ''            Next


    ''        Else
    ''            Console.WriteLine("Journal Entry CSV File is not Identified ", sFuncName)
    ''            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Journal Entry CSV File is not Identified ", sFuncName)
    ''        End If

    ''        Console.WriteLine("Completed With SUCCESS ", sFuncName)
    ''        If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed With SUCCESS", sFuncName)
    ''        IdentifyCSVFile_JournalEntry = RTN_SUCCESS

    ''    Catch ex As Exception
    ''        WriteToLogFile(ex.Message, sFuncName)
    ''        Console.WriteLine("Completed With ERROR", sFuncName)
    ''        If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed With ERROR", sFuncName)
    ''        IdentifyCSVFile_JournalEntry = RTN_ERROR
    ''    End Try

    ''End Function

 

    Public Function ConnectToTargetCompany(ByRef oCompany As SAPbobsCOM.Company, _
                                           ByRef sErrDesc As String) As Long

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
            Console.WriteLine("Starting function", sFuncName)
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Starting function", sFuncName)


            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug(" Calling GetBankingDetails ", sFuncName)
            Console.WriteLine("Calling GetBankingDetails ", sFuncName)

            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Initializing the Company Object", sFuncName)
                Console.WriteLine("Initializing the Company Object ", sFuncName)
                oCompany = New SAPbobsCOM.Company

                If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Assigning the representing database name", sFuncName)
                Console.WriteLine("Assigning the representing database name ", sFuncName)
                oCompany.Server = p_oCompDef.sServer

            If p_oCompDef.iServerType = 2008 Then
                oCompany.DbServerType = SAPbobsCOM.BoDataServerTypes.dst_MSSQL2008
            ElseIf p_oCompDef.iServerType = 2012 Then
                oCompany.DbServerType = SAPbobsCOM.BoDataServerTypes.dst_MSSQL2012
            End If

            oCompany.LicenseServer = p_oCompDef.sLicenseServer
            oCompany.CompanyDB = p_oCompDef.sSAPDBName
            oCompany.UserName = p_oCompDef.sSAPUser
            oCompany.Password = p_oCompDef.sSAPPwd

                oCompany.language = SAPbobsCOM.BoSuppLangs.ln_English

                oCompany.UseTrusted = False

                If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Connecting to the Company Database.", sFuncName)
                Console.WriteLine("Connecting to the Company Database. ", sFuncName)
                iRetValue = oCompany.Connect()

                If iRetValue <> 0 Then
                    oCompany.GetLastError(iErrCode, sErrDesc)

                    sErrDesc = String.Format("Connection to Database ({0}) {1} {2} {3}", _
                        oCompany.CompanyDB, System.Environment.NewLine, _
                                    vbTab, sErrDesc)

                    Throw New ArgumentException(sErrDesc)
                End If



            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed with SUCCESS", sFuncName)
            Console.WriteLine("Completed with SUCCESS ", sFuncName)
            ConnectToTargetCompany = RTN_SUCCESS
        Catch ex As Exception
            sErrDesc = ex.Message
            Call WriteToLogFile(ex.Message, sFuncName)
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed with ERROR", sFuncName)
            Console.WriteLine("Completed with ERROR ", sFuncName)
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
                Console.WriteLine("Moving CSV file to success folder ", sFuncName)
                If p_iDebugMode = DEBUG_ON Then WriteToLogFile_Debug("Moving CSV file to success folder", sFuncName)

            Else
                Console.WriteLine("Sending ERROR notification mail ", sFuncName)
                If p_iDebugMode = DEBUG_ON Then WriteToLogFile_Debug("Sending ERROR notification mail", sFuncName)

            End If
        Catch ex As Exception
            Console.WriteLine("Error in renaming/copying/moving ", sFuncName)
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Error in renaming/copying/moving", sFuncName)
            Call WriteToLogFile(ex.Message, sFuncName)
        End Try
    End Sub

    Public Function Del_schema(ByVal csvFileFolder As String) As Long

        ' ***********************************************************************************
        '   Function   :    Del_schema()
        '   Purpose    :    This function is handles - Delete the Schema file
        '   Parameters :    ByVal csvFileFolder As String
        '                       csvFileFolder = Passing file name
        '   Author     :    JOHN
        '   Date       :    26/06/2014 
        '   Change     :   
        '                   
        ' ***********************************************************************************
        Dim sFuncName As String = String.Empty
        Try
            sFuncName = "Del_schema()"
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Starting Function...", sFuncName)
            Console.WriteLine("Starting Function... " & sFuncName)

            Dim FileToDelete As String
            FileToDelete = csvFileFolder & "\\schema.ini"
            If System.IO.File.Exists(FileToDelete) = True Then
                System.IO.File.Delete(FileToDelete)
            End If
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed with SUCCESS", sFuncName)
            Console.WriteLine("Completed with SUCCESS " & sFuncName)
            Del_schema = RTN_SUCCESS
        Catch ex As Exception
            WriteToLogFile(ex.Message, sFuncName)
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed with ERROR", sFuncName)
            Console.WriteLine("Completed with Error " & sFuncName)
            Del_schema = RTN_ERROR
        End Try
    End Function

    Public Function Create_schema(ByVal csvFileFolder As String, ByVal FileName As String) As Long

        ' ***********************************************************************************
        '   Function   :    Create_schema()
        '   Purpose    :    This function is handles - Create the Schema file
        '   Parameters :    ByVal csvFileFolder As String
        '                       csvFileFolder = Passing file name
        '   Author     :    JOHN
        '   Date       :    26/06/2014 
        '   Change     :   
        '                   
        ' ***********************************************************************************
        Dim sFuncName As String = String.Empty
        Try
            sFuncName = "Create_schema()"
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Starting Function...", sFuncName)
            Console.WriteLine("Starting Function... " & sFuncName)

            Dim csvFileName As String = FileName
            Dim fsOutput As FileStream = New FileStream(csvFileFolder & "\\schema.ini", FileMode.Create, FileAccess.Write)
            Dim srOutput As StreamWriter = New StreamWriter(fsOutput)
            Dim s1, s2, s3, s4, s5 As String
            s1 = "[" & csvFileName & "]"
            s2 = "ColNameHeader=False"
            s3 = "Format=CSVDelimited"
            s4 = "MaxScanRows=0"
            s5 = "CharacterSet=OEM"
            srOutput.WriteLine(s1.ToString() + ControlChars.Lf + s2.ToString() + ControlChars.Lf + s3.ToString() + ControlChars.Lf + s4.ToString() + ControlChars.Lf)
            srOutput.Close()
            fsOutput.Close()

            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed with SUCCESS", sFuncName)
            Console.WriteLine("Completed with SUCCESS " & sFuncName)
            Create_schema = RTN_SUCCESS

        Catch ex As Exception
            WriteToLogFile(ex.Message, sFuncName)
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed with ERROR", sFuncName)
            Console.WriteLine("Completed with Error " & sFuncName)
            Create_schema = RTN_ERROR
        End Try

    End Function

End Module