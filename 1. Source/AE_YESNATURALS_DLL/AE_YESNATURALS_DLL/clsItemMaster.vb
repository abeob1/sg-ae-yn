Imports System.Text
Imports System.Data
Imports System.Data.SqlClient
Imports System.IO
Public Class clsItemMaster

    Public Function ExportToCSV(ByVal sQuery As String, ByVal sConString As String, _
                                ByRef sErrDesc As String) As Long
        'Get the data from database into datatable


        ' ***********************************************************************************
        '   Function   :    ExportToCSV()
        '   Purpose    :    This function is handles - Export the Data from DataTable to .CSV file.
        '   Parameters :    ByVal Query As String
        '                       sDate = Passing Query 
        '                   ByVal sConString As String
        '                       sConString = Passing the Connection String
        '                   ByRef sErrDesc As String
        '                       sErrDesc=Error Description to be returned to calling function
        '   Return      :   0 - FAILURE
        '                   1 - SUCCESS
        '   Author     :    SRINIVASAN
        '   Date       :    14/08/2014 
        '   Change     :   
        '                   
        ' ***********************************************************************************


        Dim oDataTable As DataTable = New DataTable
        Dim writer As System.IO.StreamWriter
        Dim sFuncName As String = String.Empty
        Dim oFile As File = Nothing
        Dim sFilePath As String = String.Empty

        Try
            sFuncName = "ExportToCSV()"
            p_sSelCriteriaMessage = String.Empty
            Dim sep As String = ""
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Starting Function", sFuncName)

            sFilePath = sOutputPath & "\" & Today.Year & Today.Month.ToString.PadLeft(2, "0"c) & Today.Day.ToString.PadLeft(2, "0"c) & "_ITEMMASTER.CSV"

            oDataTable = ExecuteSQLQuery(sQuery, sConString).Tables(0)

            If oDataTable.Rows.Count = 0 Then

                WriteToLogFile_Debug("There is no matching records Based on Your Selection Criteria!!", sFuncName)
                'p_sSelCriteriaMessage = "There is no matching records Based on Your Selection Criteria!!"
                sErrDesc = "There is no matching records Based on Your Selection Criteria!!"
                Return RTN_ERROR
            End If

            If File.Exists(sFilePath) Then
                File.Delete(sFilePath)
            End If

            writer = New System.IO.StreamWriter(sFilePath)

            ' first write a line with the columns name
            Dim builder As New System.Text.StringBuilder
            For Each col As DataColumn In oDataTable.Columns
                If col.ColumnName.Contains("CreateDate") = True Then Continue For
                If col.ColumnName.Contains("UpdateDate") = True Then Continue For
                If col.ColumnName.Contains("ListName") = True Then Continue For
                builder.Append(sep).Append(col.ColumnName)
                sep = ","
            Next
            writer.WriteLine(builder.ToString())

            ' then write all the rows
            For Each row As DataRow In oDataTable.Rows
                builder = New System.Text.StringBuilder
                sep = ""
                For Each col As DataColumn In oDataTable.Columns
                    If col.ColumnName.Contains("CreateDate") = True Then Continue For
                    If col.ColumnName.Contains("UpdateDate") = True Then Continue For
                    If col.ColumnName.Contains("ListName") = True Then Continue For
                    If row(col.ColumnName).ToString.Contains(",") Then
                        builder.Append(sep).Append("""" + row(col.ColumnName) + """")
                    Else
                        builder.Append(sep).Append(row(col.ColumnName))
                    End If
                    sep = ","
                Next
                writer.WriteLine(builder.ToString())
            Next

            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed with SUCCESS", sFuncName)
            ExportToCSV = RTN_SUCCESS
        Catch ex As Exception
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed with ERROR", sFuncName)
            Call WriteToLogFile(ex.Message, sFuncName)
            ExportToCSV = RTN_ERROR
        Finally
            If Not writer Is Nothing Then writer.Close()
        End Try
    End Function

    Public Function GetData(ByVal sQuery As String, ByRef oDICompany As SAPbobsCOM.Company, ByRef sErrDesc As String) As DataTable

        ' ***********************************************************************************
        '   Function   :    GetData()
        '   Purpose    :    This function is handles - Passing recordset to Another function "ConvertRecordset(oRS)"
        '   Parameters :    ByVal sQuery As String
        '                       sQuery = Passing Query 
        '                   ByRef oDICompany As SAPbobsCOM.Company
        '                       oDICompany = Passing the SAP Company
        '                   ByRef sErrDesc As String
        '                       sErrDesc=Error Description to be returned to calling function
        '   Author     :    SRINIVASAN
        '   Date       :    14/08/2014 
        '   Change     :   
        '                   
        ' ***********************************************************************************


        Dim dt As New DataTable()
        Dim oRS As SAPbobsCOM.Recordset = oDICompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
        Dim sFuncName As String = String.Empty
        Try
            sFuncName = "btnExportCSV_Click()"
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Starting Function", sFuncName)

            oRS.DoQuery(sQuery)

            If oRS.RecordCount <> 0 Then
                If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Calling ConvertRecordset()", sFuncName)
                Return ConvertRecordset(oRS)
            End If

            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed with SUCCESS", sFuncName)

        Catch ex As Exception
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed with ERROR", sFuncName)
            Call WriteToLogFile(ex.Message, sFuncName)
            Return Nothing
        End Try
    End Function

    Private Function ConvertRecordset(ByVal SAPRecordset As SAPbobsCOM.Recordset) As DataTable

        '\ This function will take an SAP recordset from the SAPbobsCOM library and convert it to a more
        '\ easily used ADO.NET datatable which can be used for data binding much easier.

        ' ***********************************************************************************
        '   Function   :    ConvertRecordset()
        '   Purpose    :    This function is handles - Return the DataTable from the Recordset
        '   Parameters :    ByVal SAPRecordset As SAPbobsCOM.Recordset
        '                       SAPRecordset = Passing Recordset
        '   Author     :    SRINIVASAN
        '   Date       :    14/08/2014 
        '   Change     :   
        '                   
        ' ***********************************************************************************


        Dim dtTable As New DataTable
        Dim NewCol As DataColumn
        Dim NewRow As DataRow
        Dim ColCount As Integer
        Dim sFuncName As String = String.Empty
        Try
            sFuncName = "ConvertRecordset()"
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Starting Function", sFuncName)

            For ColCount = 0 To SAPRecordset.Fields.Count - 4
                NewCol = New DataColumn(SAPRecordset.Fields.Item(ColCount).Name)
                dtTable.Columns.Add(NewCol)
            Next

            Do Until SAPRecordset.EoF

                NewRow = dtTable.NewRow
                'populate each column in the row we're creating
                For ColCount = 0 To SAPRecordset.Fields.Count - 4

                    NewRow.Item(SAPRecordset.Fields.Item(ColCount).Name) = SAPRecordset.Fields.Item(ColCount).Value

                Next

                'Add the row to the datatable
                dtTable.Rows.Add(NewRow)

                SAPRecordset.MoveNext()
            Loop
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed with SUCCESS", sFuncName)
            Return dtTable

        Catch ex As Exception
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed with ERROR", sFuncName)
            Call WriteToLogFile(ex.Message, sFuncName)
            Return Nothing
        End Try

    End Function

    Public Function ExecuteSQLQuery(ByVal sQuery As String, ByVal sConString As String) As DataSet

        ' ***********************************************************************************
        '   Function   :    ExecuteSQLQuery()
        '   Purpose    :    This function is handles - Return the DataSet Based on Query string
        '   Parameters :    ByVal sQuery As String
        '                       sQuery = Passing the Query String
        '                   ByVal sConString As String
        '                       sConString = Passing the SQL Connection String
        '   Author     :    SRINIVASAN
        '   Date       :    14/08/2014 
        '   Change     :   
        '                   
        ' ***********************************************************************************

        Dim sFuncName As String = String.Empty
        If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Starting Function", sFuncName)
        ' Dim sConstr As String = "DRIVER={HDBODBC32};UID=" & p_oCompDef.sDBUser & ";PWD=" & p_oCompDef.sDBPwd & ";SERVERNODE=" & p_oCompDef.sServer & ";CS=" & p_oCompDef.sSAPDBName
        Dim sConstr As String = sConString '"Data Source=" & p_oCompDef.p_sServerName & ";Initial Catalog=" & p_oCompDef.p_sDataBaseName & ";User ID=" & p_oCompDef.p_sDBUserName & "; Password=" & p_oCompDef.p_sDBPassword
        Dim oCon As New SqlConnection(sConstr)
        Dim oCmd As New SqlCommand
        Dim oDs As New DataSet

        Try
            sFuncName = "ExecuteQuery()"
            oCon.ConnectionString = sConstr
            oCon.Open()
            oCmd.CommandType = CommandType.Text
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Query " & sQuery, sFuncName)
            oCmd.CommandText = sQuery
            oCmd.Connection = oCon
            oCmd.CommandTimeout = 180
            Dim da As New SqlDataAdapter(oCmd)
            da.Fill(oDs)
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Function Completed With SUCCESS.", sFuncName)

        Catch ex As Exception
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Function Completed With ERROR.", sFuncName)
            Throw New Exception(ex.Message)
        Finally
            oCon.Dispose()
        End Try
        Return oDs
    End Function

End Class
