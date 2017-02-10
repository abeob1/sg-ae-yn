Imports System.Data.SqlClient
Imports System.Globalization

Public Class Documents

    Public Sub CreateJE(ByVal ConnectionString As String)
        Dim oJE As SAPbobsCOM.JournalEntries
        Dim sqlConx As SqlConnection = New SqlConnection(ConnectionString)
        Try
            Dim oCompany As SAPbobsCOM.Company = PublicVariable.oCompany
            Dim query As String
            query = "Select " +
                    "	BPCode = T1.CardCode " +
                    "	,OutgoingEntry = T0.DocEntry" +
                    "	,T0.ProjectCode" +
                    "	,T0.Term" +
                    "	,T0.Day" +
                    "	,T0.Date" +
                    "	,DailyInterest = T0.Value " +
                    "   ,T1.DocCurr " +
                    "from " +
                    "	InterestOVPM T0" +
                    "	left join OVPM T1 on T0.DocEntry = T1.DocEntry " +
                    "where Value <> 0 and Synced = 0 and Cancelled = 0 and [Date] <= GETDATE() and IsNull(U_FullPaid,'') <> 'Y'"
            sqlConx.Open()

            Dim data As DataTable = GetDataSQL(query, sqlConx)

            Dim nErr As Integer
            Dim errMsg As String = ""
            For Each row As DataRow In data.Rows
                oJE = oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oJournalEntries)
                oJE.ReferenceDate = row("Date")
                oJE.Reference3 = row("OutgoingEntry")
                oJE.ProjectCode = row("ProjectCode")
                oJE.Lines.ReferenceDate1 = row("Date")
                oJE.Lines.ShortName = row("BPCode")
                oJE.Lines.TaxDate = row("Date")
                If row("DocCurr") <> "SGD" Then
                    oJE.Lines.FCCredit = 0
                    oJE.Lines.FCDebit = row("DailyInterest")
                    oJE.Lines.FCCurrency = row("DocCurr").ToString
                Else
                    oJE.Lines.Credit = 0
                    oJE.Lines.Debit = row("DailyInterest")
                End If
                oJE.Lines.Add()
                oJE.Lines.SetCurrentLine(1)
                oJE.Lines.AccountCode = PublicVariable.CreditAccount
                If row("DocCurr") <> "SGD" Then
                    oJE.Lines.FCCredit = row("DailyInterest")
                    oJE.Lines.FCDebit = 0
                    oJE.Lines.FCCurrency = row("DocCurr").ToString
                Else
                    oJE.Lines.Credit = row("DailyInterest")
                    oJE.Lines.Debit = 0
                End If

                oJE.Lines.ReferenceDate1 = row("Date")
                oJE.Lines.ShortName = PublicVariable.CreditAccount
                oJE.Lines.TaxDate = row("Date")

                query = "UPDATE InterestOVPM " +
                        "Set Synced = {0}, ErrorMess = '{1}' " +
                        "Where DocEntry = {2} and Term = {3} and Day = {4} and CardCode = '{5}'"

                If (0 <> oJE.Add()) Then
                    oCompany.GetLastError(nErr, errMsg)
                    query = String.Format(query, 0, errMsg, row("OutgoingEntry"), row("Term"), row("Day"), row("BPCode"))
                    UpdateDataSQL(query, sqlConx)
                Else
                    UpdateOutStanding(row("OutgoingEntry"), row("BPCode"), row("ProjectCode"), sqlConx)
                    query = String.Format(query, 1, "", row("OutgoingEntry"), row("Term"), row("Day"), row("BPCode"))
                    UpdateDataSQL(query, sqlConx)
                    RecalculateInterest(row("OutgoingEntry"), row("Term"), row("Day"), row("BPCode"), row("ProjectCode"), sqlConx)
                End If
            Next

        Catch ex As Exception
            Functions.WriteLog(ex.Message)
        Finally
            oJE = Nothing
            sqlConx.Close()
        End Try
    End Sub


    Public Sub CancelJE(ByVal ConnectionString As String)
        Dim oJE As SAPbobsCOM.JournalEntries
        Dim oCompany As SAPbobsCOM.Company = PublicVariable.oCompany
        Dim sqlConx As SqlConnection = New SqlConnection(ConnectionString)
        Try
            sqlConx.Open()
            'Functions.WriteLog("Opened Connectrion SQL")
            Dim query As String = "     select T1.TransId, T0.DocEntry, T0.CardCode, T0.ProjectCode " +
                                    "   from " +
                                    "       CancelOVPM T0 " +
                                    "       left join OJDT T1 on T1.Ref3 = T0.DocEntry and T1.Project = T0.ProjectCode " +
                                    "   where " +
                                    "       T0.Cancelled = 0 " +
                                    "       and ISNULL(T1.TransId,'') <> '' "
            Dim table As DataTable = GetDataSQL(query, sqlConx)
            Dim table1 As DataTable = New DataTable
            table1.Columns.Add("DocEntry")
            table1.Columns.Add("ProjectCode")
            table1.Columns.Add("CardCode")

            Dim TransId As Integer
            Dim DocEntry As Integer
            Dim PrjCode As String
            Dim CardCode As String
            Dim nErr As Integer
            Dim errMsg As String = ""
            oCompany.StartTransaction()
            
            For Each row As DataRow In table.Rows
                oJE = oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oJournalEntries)
                TransId = row("TransId")
                DocEntry = row("DocEntry")
                PrjCode = row("ProjectCode")
                CardCode = row("CardCode")
                If oJE.GetByKey(TransId) Then
                    'Functions.WriteLog("Got Document to cancel")
                    If (0 <> oJE.Cancel()) Then
                        oCompany.GetLastError(nErr, errMsg)
                        Throw New Exception(nErr.ToString() + " -- " + errMsg)
                    Else
                        table1.Rows.Add(DocEntry, PrjCode, CardCode)
                    End If
                End If
            Next
            oCompany.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_Commit)

            query = "Update CancelOVPM " +
                    "Set Cancelled = 1 " +
                    "Where DocEntry = {0} and CardCode = '{1}' and ProjectCode = '{2}' "
            For Each row As DataRow In table1.Rows
                DocEntry = row("DocEntry")
                PrjCode = row("ProjectCode")
                CardCode = row("CardCode")
                'Functions.WriteLog("Canceled Document")
                UpdateOutStanding(DocEntry, CardCode, PrjCode, sqlConx)
                'Functions.WriteLog("Updated Outstanding")
                query = String.Format(query, DocEntry, CardCode, PrjCode)
                UpdateDataSQL(query, sqlConx)
                'Functions.WriteLog("Updated Table CancelOVPM")
            Next
            'Functions.WriteLog("Completed cancel JE")
        Catch ex As Exception
            Functions.WriteLog(ex.Message)
            oCompany.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_RollBack)
        Finally
            oJE = Nothing
            sqlConx.Close()
        End Try
    End Sub
    Private Sub RecalculateInterest(ByVal OutgoingEntry As Integer, ByVal Term As Integer, ByVal Day As Integer, ByVal BPCode As String, ByVal ProjectCode As String, ByVal sqlConx As SqlConnection)
        Dim query As String = String.Format(My.Resources.RecalculateInterest, BPCode, ProjectCode, OutgoingEntry, Term, Day)
        'Functions.WriteLog(query)
        UpdateDataSQL(query, sqlConx)
    End Sub
    Private Function GetDataSQL(ByVal query As String, ByVal sqlConx As SqlConnection) As DataTable
        Try
            Dim sqlAdapter As SqlDataAdapter = New SqlDataAdapter(query, sqlConx)
            Dim table As DataTable = New DataTable("InterestOVPM")
            sqlAdapter.Fill(table)
            Return table
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Private Sub UpdateDataSQL(ByVal query As String, ByVal sqlConx As SqlConnection)
        Try
            Dim sqlCommand As SqlCommand = sqlConx.CreateCommand()
            sqlCommand.CommandText = query
            sqlCommand.ExecuteNonQuery()
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Private Sub UpdateOutStanding(ByVal DocEntry As Integer, ByVal CardCode As String, ByVal ProjectCode As String, ByVal sqlConx As SqlConnection)
        Try
            Dim sqlCommand As SqlCommand = sqlConx.CreateCommand()
            ' Dim query As String = String.Format("Select SUM(Debit - Credit) From JDT1 where ShortName = '{0}' and Project = '{1}'", CardCode, ProjectCode)
            'sqlCommand.CommandText = query
            'Dim OutStanding As Double = Double.Parse(sqlCommand.ExecuteScalar(), Functions.GetCulture())
            'Dim OutStanding As Decimal = Decimal.Parse(sqlCommand.ExecuteScalar(), CultureInfo.InvariantCulture)
            Dim query As String = String.Format("Update OVPM Set U_Outstanding = (Select SUM(Debit - Credit) From JDT1 where ShortName = '{0}' and Project = '{1}') where DocEntry = {2}", CardCode, ProjectCode, DocEntry)
            'Functions.WriteLog(query)
            sqlCommand.CommandText = query
            sqlCommand.ExecuteNonQuery()
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
End Class
