Imports System.Xml
Imports System.IO
Imports System.Globalization
Public Class clsCommon

    Public Function GetDataViewFromCSV(ByVal CurrFileToUpload As String) As DataView

        ' **********************************************************************************
        '   Function    :   GetDataViewFromCSV()
        '   Purpose     :   This function will upload the data from CSV file to Dataview
        '   Parameters  :   ByRef CurrFileToUpload AS String 
        '                       CurrFileToUpload = File Name
        '   Author      :   JOHN
        '   Date        :   MAY 2014 20
        ' **********************************************************************************

        Dim da As OleDb.OleDbDataAdapter
        Dim dt As DataTable
        Dim dv As DataView
        Dim sLocalPath As String = String.Empty
        Dim sLocalFile As String = String.Empty
        Dim sFuncName As String = String.Empty

        Try
            sFuncName = "GetDataViewFromCSV()"
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Starting Function...", sFuncName)

            Dim sConnectionString As String = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" & System.IO.Path.GetDirectoryName(CurrFileToUpload) & "\;Extended Properties=""text;HDR=NO;FMT=Delimited"""
            'Dim sConnectionString As String = "provider=Microsoft.Jet.OLEDB.4.0; " & _
            '  "data source='" & CurrFileToUpload & " '; " & "Extended Properties=Excel 8.0;"
            Dim objConn As New System.Data.OleDb.OleDbConnection(sConnectionString)

            Dim iIndex As Integer = InStrRev(CurrFileToUpload, "\", -1)
            sLocalPath = Left(CurrFileToUpload, iIndex)
            sLocalFile = Mid(CurrFileToUpload, iIndex + 1, CurrFileToUpload.ToString.Length - iIndex)

            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Create_schema() ", sFuncName)
            Create_schema(sLocalPath, sLocalFile)

            'Open Data Adapter to Read from Excel file

            da = New System.Data.OleDb.OleDbDataAdapter("SELECT * FROM [" & System.IO.Path.GetFileName(CurrFileToUpload) & "]", objConn)
            dt = New DataTable("JE")
            'Fill dataset using dataadapter
            da.Fill(dt)

            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed With SUCCESS", sFuncName)

            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Del_schema() ", sFuncName)
            Del_schema(sLocalPath)

            dv = New DataView(dt)
            Return dv

        Catch ex As Exception
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Error occured while reading content of  " & ex.Message, sFuncName, sLogFolderPath)
            Call WriteToLogFile(ex.Message, sFuncName, sLogFolderPath)
            Return Nothing
        End Try

    End Function

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
            Del_schema = RTN_SUCCESS
        Catch ex As Exception
            WriteToLogFile(ex.Message, sFuncName, sLogFolderPath)
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed with ERROR", sFuncName)
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
            'Dim s1, s2, s3, s4, s5 As String

            srOutput.WriteLine("[" & csvFileName & "]")
            srOutput.WriteLine("ColNameHeader=False")
            srOutput.WriteLine("Format=CSVDelimited")
            srOutput.WriteLine("Col1=F1 Text")
            srOutput.WriteLine("Col2=F2 Text")
            srOutput.WriteLine("Col3=F3 Text")
            srOutput.WriteLine("Col4=F4 Text")
            srOutput.WriteLine("Col5=F5 Text")
            srOutput.WriteLine("Col6=F6 Text")
            srOutput.WriteLine("Col7=F7 Text")
            srOutput.WriteLine("Col8=F8 Text")
            srOutput.WriteLine("Col9=F9 Double")
            srOutput.WriteLine("Col10=F10 Double")
            srOutput.WriteLine("Col11=F11 Double")
            srOutput.WriteLine("Col12=F12 Double")
            srOutput.WriteLine("Col13=F13 Double")
            srOutput.WriteLine("Col14=F14 Double")
            srOutput.WriteLine("Col15=F15 Text")
            srOutput.WriteLine("Col16=F16 Text")
            srOutput.WriteLine("Col17=F17 Text")
            srOutput.WriteLine("Col18=F18 Double")
            srOutput.WriteLine("MaxScanRows=0")
            srOutput.WriteLine("CharacterSet=OEM")

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

    Public Function GetDate(ByVal sDate As String, ByRef oCompany As SAPbobsCOM.Company, ByRef sErrDesc As String) As String

        ' ***********************************************************************************
        '   Function   :    GetDate()
        '   Purpose    :    This function is handles - Return the Date which is in SAP Format
        '   Parameters :    ByVal sDate As String
        '                       sDate = Passing Document Date
        '                   ByRef oCompany As SAPbobsCOM.Company
        '                       oCompany = Passing the Company which has been connected
        '                   ByRef sErrDesc As String
        '                       sErrDesc=Error Description to be returned to calling function
        '   Author     :    SRINIVASAN
        '   Date       :    15/08/2014 
        '   Change     :   
        '                   
        ' ***********************************************************************************
        Dim dateValue As DateTime
        Dim DateString As String = String.Empty
        Dim sSQL As String = String.Empty
        Dim oRs As SAPbobsCOM.Recordset
        Dim sDatesep As String
        Dim sFuncName As String = String.Empty

        Try

        sFuncName = "GetDate()"
        If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Starting Function...", sFuncName)

        oRs = oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)

        sSQL = "SELECT DateFormat,DateSep FROM OADM"

        oRs.DoQuery(sSQL)

        If Not oRs.EoF Then
            sDatesep = oRs.Fields.Item("DateSep").Value

            Select Case oRs.Fields.Item("DateFormat").Value
                Case 0
                    If Date.TryParseExact(sDate, "dd" & sDatesep & "MM" & sDatesep & "yy", _
                       New CultureInfo("en-US"), _
                       DateTimeStyles.None, _
                       dateValue) Then
                        DateString = dateValue.ToString("yyyyMMdd")
                    End If
                Case 2
                    If Date.TryParseExact(sDate, "MM" & sDatesep & "dd" & sDatesep & "yy", _
                        New CultureInfo("en-US"), _
                        DateTimeStyles.None, _
                        dateValue) Then
                        DateString = dateValue.ToString("yyyyMMdd")
                    End If
                Case 3
                    If Date.TryParseExact(sDate, "MM" & sDatesep & "dd" & sDatesep & "yyyy", _
                        New CultureInfo("en-US"), _
                        DateTimeStyles.None, _
                        dateValue) Then
                        DateString = dateValue.ToString("yyyyMMdd")
                    End If
                Case 5
                    If Date.TryParseExact(sDate, "dd" & sDatesep & "MMMM" & sDatesep & "yyyy", _
                        New CultureInfo("en-US"), _
                        DateTimeStyles.None, _
                        dateValue) Then
                        DateString = dateValue.ToString("yyyyMMdd")
                    End If
                Case 6
                    If Date.TryParseExact(sDate, "yy" & sDatesep & "MM" & sDatesep & "dd", _
                        New CultureInfo("en-US"), _
                        DateTimeStyles.None, _
                        dateValue) Then
                        DateString = dateValue.ToString("yyyyMMdd")
                    End If
                Case Else
                    DateString = dateValue.ToString("yyyyMMdd")
                End Select
            End If
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed with SUCCESS", sFuncName)
            GetDate = RTN_SUCCESS
            Return DateString
        Catch ex As Exception
            WriteToLogFile(ex.Message, sFuncName, sLogFolderPath)
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed with ERROR", sFuncName)
            GetDate = RTN_ERROR
        End Try



    End Function

    Public Function GetSingleValue(ByVal Query As String, ByRef p_oDICompany As SAPbobsCOM.Company, ByRef sErrDesc As String) As String

        ' ***********************************************************************************
        '   Function   :    GetSingleValue()
        '   Purpose    :    This function is handles - Return single value based on Query
        '   Parameters :    ByVal Query As String
        '                       sDate = Passing Query 
        '                   ByRef oCompany As SAPbobsCOM.Company
        '                       oCompany = Passing the Company which has been connected
        '                   ByRef sErrDesc As String
        '                       sErrDesc=Error Description to be returned to calling function
        '   Author     :    SRINIVASAN
        '   Date       :    15/08/2014 
        '   Change     :   
        '                   
        ' ***********************************************************************************

        Dim sFuncName As String = String.Empty
        Dim objRS As SAPbobsCOM.Recordset = p_oDICompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
        Try
            sFuncName = "GetSingleValue()"
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Starting Function", sFuncName)

            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Query : " & Query, sFuncName)

            objRS.DoQuery(Query)
            If objRS.RecordCount > 0 Then
                If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed with SUCCESS", sFuncName)
                GetSingleValue = RTN_SUCCESS
                Return objRS.Fields.Item(0).Value.ToString
            End If
        Catch ex As Exception
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed with ERROR", sFuncName)
            WriteToLogFile(ex.Message, sFuncName, sLogFolderPath)
            GetSingleValue = RTN_SUCCESS
            Return ""
        End Try
        Return Nothing
    End Function

End Class
