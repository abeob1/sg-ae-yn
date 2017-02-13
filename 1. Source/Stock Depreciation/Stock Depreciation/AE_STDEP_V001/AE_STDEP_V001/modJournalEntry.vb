Module modJournalEntry


    Public Function JournalEntry_Posting(ByVal oDs_Journal As DataSet, ByRef oCompany As SAPbobsCOM.Company, ByRef sErrDesc As String) As Long

        Dim sFuncName As String = String.Empty
        Dim oDV_StockDepreciation As DataView = Nothing
        '  Dim oDT_DisctinctItemGroup As DataTable = Nothing
        Dim oDT_GLAccount As DataTable = Nothing
        Dim sCreditGL As String = String.Empty
        Dim sDebitGL As String = String.Empty
        Dim ival As Integer
        Dim iErr As Integer
        Dim sErr As String = String.Empty
        Dim sJV As String = String.Empty
        Dim oRset As SAPbobsCOM.Recordset = oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)

        Try
            sFuncName = "JournalEntry_Posting"
            Console.WriteLine("Starting Function ", sFuncName)
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Starting Function ", sFuncName)

            oDV_StockDepreciation = oDs_Journal.Tables("Stock Depreciation").DefaultView
            ' oDT_DisctinctItemGroup = oDV_StockDepreciation.Table.DefaultView.ToTable(True, "F2")
            oDT_GLAccount = oDs_Journal.Tables("GL Account")


            Select Case oDT_GLAccount.Rows(0).Item(0)
                Case "Credit"
                    sCreditGL = oDT_GLAccount.Rows(0).Item(1)
                Case "Debit"
                    sDebitGL = oDT_GLAccount.Rows(0).Item(1)
                Case Else
            End Select

            Select Case oDT_GLAccount.Rows(1).Item(0)
                Case "Credit"
                    sCreditGL = oDT_GLAccount.Rows(1).Item(1)
                Case "Debit"
                    sDebitGL = oDT_GLAccount.Rows(1).Item(1)
                Case Else
            End Select

            If String.IsNullOrEmpty(sCreditGL) Then
                Console.WriteLine("Credit GL Account is Empty ...... ! ", sFuncName)
                If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Credit GL Account is Empty ...... ! ", sFuncName)
                JournalEntry_Posting = RTN_ERROR
                Exit Function
            End If

            If String.IsNullOrEmpty(sDebitGL) Then
                Console.WriteLine("Credit GL Account is Empty ...... ! ", sFuncName)
                If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Credit GL Account is Empty ...... ! ", sFuncName)
                JournalEntry_Posting = RTN_ERROR
                Exit Function
            End If

            If oDV_StockDepreciation Is Nothing AndAlso oDV_StockDepreciation.Count < 0 Then
                Console.WriteLine("No datas for posting JE ...... ! ", sFuncName)
                If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("No datas for posting JE ......... !", sFuncName)
                JournalEntry_Posting = RTN_ERROR
                Exit Function
            End If

            If oCompany.InTransaction = False Then
                oCompany.StartTransaction()
            End If

            For Each drv As DataRowView In oDV_StockDepreciation
                Dim oJournalEntry As SAPbobsCOM.JournalEntries = oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oJournalEntries)
                oJournalEntry.ReferenceDate = Now.Date
                oJournalEntry.DueDate = Now.Date
                oJournalEntry.TaxDate = Now.Date
                oJournalEntry.UseAutoStorno = SAPbobsCOM.BoYesNoEnum.tYES
                oJournalEntry.StornoDate = Now.Date.AddDays(1)
                oJournalEntry.Reference = "Stock Depreciation " & drv(1).ToString.Trim  'oDVJournal.Table.Rows(1).Item(1).ToString.Trim
                oJournalEntry.Memo = "Stock Depreciation for Month " & MonthName(Now.Month, True) & " " & drv(1).ToString.Trim 'oDVJournal.Table.Rows(1).Item(5).ToString.Trim
                'Journal Entry Document Line Information

                'MsgBox(CDbl(drv(14).ToString.Trim))
                oJournalEntry.Lines.AccountCode = sDebitGL
                oJournalEntry.Lines.Debit = CDbl(drv(16).ToString.Trim)
                oJournalEntry.Lines.Credit = "0"
                oJournalEntry.Lines.Add()
                oJournalEntry.Lines.AccountCode = sCreditGL
                oJournalEntry.Lines.Credit = CDbl(drv(16).ToString.Trim)
                oJournalEntry.Lines.Debit = "0"

                Console.WriteLine("Attempting to Add the Journal Entry ", sFuncName)

                If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Attempting to Add the Journal Entry", sFuncName)
                ival = oJournalEntry.Add()

                If ival <> 0 Then
                    oCompany.GetLastError(iErr, sErr)
                    Call WriteToLogFile("Completed with ERROR ---" & sErr, sFuncName)
                    Console.WriteLine("Completed with ERROR ", sFuncName)
                    If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed with ERROR " & sErr, sFuncName)
                    sErrDesc = sErr
                    JournalEntry_Posting = RTN_ERROR
                    Exit Function
                Else
                    Console.WriteLine("Completed with SUCCESS", sFuncName)
                    If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed with SUCCESS ", sFuncName)
                    oCompany.GetNewObjectCode(sJV)
                    sErrDesc = ""
                    Console.WriteLine("Journal Entry DocEntry  " & drv(1).ToString.Trim & "  " & sJV, sFuncName)
                    If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Journal Entry DocEntry  " & sJV, sFuncName)
                    JournalEntry_Posting = RTN_SUCCESS
                End If
            Next

            If oCompany.InTransaction = True Then
                oCompany.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_Commit)
                If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Commiting the transaction...... !  ", sFuncName)
            End If

            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Executing Query " & "[AE_SP002_UpdateStatus_StockDepreciation]'" & UCase(MonthName(Month(Now.Date), True)) & "','" & Year(Now.Date) & "'", sFuncName)
            oRset.DoQuery("[AE_SP002_UpdateStatus_StockDepreciation]'" & UCase(MonthName(Month(Now.Date), True)) & "','" & Year(Now.Date) & "'")
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Status Updated Successfully ...... !", sFuncName)

        Catch ex As Exception
            Call WriteToLogFile(ex.Message, sFuncName)
            If oCompany.InTransaction = True Then
                oCompany.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_RollBack)
                If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("RollBacking the transaction...... !  ", sFuncName)
            End If
            sErrDesc = sErr
            Console.WriteLine("Completed with ERROR ", sFuncName)
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed with ERROR " & ex.Message, sFuncName)
            JournalEntry_Posting = RTN_ERROR
            Exit Function
        End Try

    End Function

End Module
