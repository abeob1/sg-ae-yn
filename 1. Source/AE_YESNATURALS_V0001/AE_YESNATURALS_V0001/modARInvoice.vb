Module modARInvoice

    Function AR_Invoice(ByRef DataView_ As DataView, ByRef sErrDesc As String) As Long
        Dim sFuncName As String = String.Empty
        Dim p_oDVJE As DataView = Nothing
        Dim oDTDistinct As DataTable = Nothing
        Dim oDTRowFilter As DataTable = Nothing
        Dim ofrmARInvoice As frmARInvoice = New frmARInvoice

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

                ofrmARInvoice.WriteToStatusScreen(False, "Calling ARInvoice() Function for Branch Code : " & oDTDistinct.Rows(IntRow).Item(0).ToString)

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

End Module
