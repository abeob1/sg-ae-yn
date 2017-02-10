Imports System.Data.SqlClient
Imports System.Data.OleDb

Public Class clsARInvoice

    Function ARInvoice(ByRef DataView_ As DataView, ByRef oDICompany As SAPbobsCOM.Company, _
                       ByVal sConnString As String, ByRef sErrDesc As String) As Long

        'Function   :   ARInvoice()
        'Purpose    :   Create a AR invoice through DI API
        'Parameters :   ByRef DataView_ As DataView
        '               oDICompany As SAPbobsCOM.Company
        '               ByRef sErrDesc As String

        '                   sErrDesc=Error Description to be returned to calling function

        'Return     :   0 - FAILURE
        '               1 - SUCCESS

        'Author     :   SRINIVASAN
        'Date       :   SEP-14
        'Change     :

        Dim sFuncName As String = String.Empty
        Dim lRetCode As Long
        Dim sDocEntry As String = String.Empty
        Dim sSeries As String = String.Empty
        Dim oCommon As clsCommon = New clsCommon
        Dim sItemCodes As String = String.Empty
        Dim sAccountCode As String = String.Empty
        Dim dTotalAmount As Double = 0.0
        Dim dDocTotal As Double = 0.0
        Dim oItemMaster As clsItemMaster = New clsItemMaster
        Dim oDataset As DataSet = Nothing
        Dim sQuery As String = String.Empty
        Dim sCustRefNum As String = String.Empty
        Dim sCustRefVal As String = String.Empty
        Dim sLocation As String = String.Empty

        Dim oRecordset As SAPbobsCOM.Recordset = oDICompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)

        Try
            sFuncName = "ARInvoice"
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Starting Function", sFuncName)

            Dim sInvoiceDate As String = DataView_.Item(0).Row(3).ToString
            Dim dInvDocDate As Date ' ÝYYYMMdd
            If sInvoiceDate.Length = 8 Then
                dInvDocDate = Left(sInvoiceDate, 4) & "/" & Mid(sInvoiceDate, 5, 2) & "/" & Right(sInvoiceDate, 2)
            ElseIf sInvoiceDate.Length = 7 Then
                dInvDocDate = Left(sInvoiceDate, 4) & "/" & Mid(sInvoiceDate, 5, 2) & "/0" & Right(sInvoiceDate, 1)
            ElseIf sInvoiceDate.Length = 6 Then
                dInvDocDate = Left(sInvoiceDate, 4) & "/0" & Mid(sInvoiceDate, 5, 1) & "/0" & Right(sInvoiceDate, 1)
            End If

            Dim oARInvoice As SAPbobsCOM.Documents = oDICompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oInvoices)

            sCustRefNum = "POS-" & Left(sInvoiceDate, 4) & Mid(sInvoiceDate, 5, 2) & Right(sInvoiceDate, 2) & "-" & DataView_.Item(0).Row(2).ToString

            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Calling GetSingleValue() ", sFuncName)

            sCustRefVal = oCommon.GetSingleValue("select distinct  DocEntry from OINV where NumAtCard ='" & sCustRefNum & "' ", oDICompany, sErrDesc)

            If String.IsNullOrEmpty(sCustRefVal) = False Then

                If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Already Exists. Reference Num : " & sCustRefNum, sFuncName)

                If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed with SUCCESS.", sFuncName)

                Return RTN_SUCCESS

            End If

            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Calling GetSingleValue() ", sFuncName)

            sCardCode = oCommon.GetSingleValue("SELECT T0.[Name] FROM [dbo].[@CUSTOMERMAPPPING]  T0  " &
                                                         " WHERE T0.[Code] ='" & DataView_.Item(0).Row(2).ToString & "'", oDICompany, sErrDesc)

            sLocation = DataView_.Item(0).Row(1).ToString

            oARInvoice.CardCode = CStr(sCardCode)

            oARInvoice.NumAtCard = sCustRefNum

            oARInvoice.DocDate = dInvDocDate
            oARInvoice.DocDueDate = dInvDocDate
            oARInvoice.TaxDate = dInvDocDate

            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Calling GetSingleValue() ", sFuncName)

            oARInvoice.Series = oCommon.GetSingleValue("SELECT T0.[Series] FROM NNM1 T0 WHERE T0.[ObjectCode] ='13' " & _
                                                       "and  T0.[SeriesName] ='" & DataView_.Item(0).Row(2).ToString & "-IN-" & Mid(sInvoiceDate, 3, 2) & "'", oDICompany, sErrDesc)


            oARInvoice.DocType = SAPbobsCOM.BoDocumentTypes.dDocument_Service

            sItemCodes = String.Empty

            'To get the ItemCodes from the Excel:
            For IntRow As Integer = 0 To DataView_.Count - 1
                If DataView_.Item(IntRow).Row(0).ToString.ToUpper = "D" Then

                    sItemCodes += "'" & DataView_.Item(IntRow).Row(5).ToString & "',"

                End If
            Next

            'Get the Revenue Account from Trading DB

            sItemCodes = sItemCodes.Substring(0, sItemCodes.Length - 1)

            sQuery = " select T1.ItemCode,T1.ItemName ,T1.ItmsGrpCod ,T2.ItmsGrpNam,T0.DfltIncom  from OGAR T0 with (nolock) " & _
                    "JOIN OITM T1 with (nolock) ON T0.ItmsGrpCod =T1.ItmsGrpCod  join OITB T2 with (nolock) on T1.ItmsGrpCod =T2.ItmsGrpCod " & _
                    " JOIN OCRD T3 with (nolock) on T0.BPGrpCod =T3.GroupCode  Where  T3.CardCode  ='" & sCardCode & "'and T0.Year =" & Year(dInvDocDate) & " and T1.ItemCode in (" & sItemCodes & ")"


            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Calling ExecuteSQLQuery() ", sFuncName)
            oDataset = oItemMaster.ExecuteSQLQuery(sQuery, sConnString)

            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Starting Validation for ItemGroup ", sFuncName)

            Dim oDVItemGroup As DataView = New DataView(oDataset.Tables(0))
            Dim oDTItemGroup As DataTable = Nothing

            oDTItemGroup = oDVItemGroup.Table.DefaultView.ToTable(True, "ItemCode")

            For iRowCount As Integer = 0 To oDTItemGroup.Rows.Count - 1
                If String.IsNullOrEmpty(oDTItemGroup.Rows(iRowCount).Item(0).ToString) Then Continue For

                oDVItemGroup.RowFilter = "ItemCode = '" & oDTItemGroup.Rows(iRowCount).Item(0).ToString & "'"

                If oDVItemGroup.Count > 1 Then
                    sErrDesc = oDVItemGroup.Item(0).Row(3).ToString & " Group Having Multiple G/L Account at Advanced G/L Account Determination Rules in Trading DataBase"
                    Call WriteToLogFile(sErrDesc, sFuncName)
                    If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed with ERROR for Validating Item Group", sFuncName)
                    If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed with ERROR", sFuncName)
                    Return RTN_ERROR
                End If
            Next

            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed with SUCCESS for Validating Item Group", sFuncName)

            oDataset.Tables.Add(DataView_.ToTable)

            Dim oDTFinal As New DataTable

            ' Create four typed columns in the DataTable.
            oDTFinal.Columns.Add("ItemCode", GetType(Integer))
            oDTFinal.Columns.Add("ItmGrpCode", GetType(String))
            oDTFinal.Columns.Add("RevenueAC", GetType(String))
            oDTFinal.Columns.Add("LineTotAmount", GetType(Double))
            oDTFinal.Columns.Add("ItmGrpName", GetType(String))
            oDTFinal.Columns.Add("UnitPrice", GetType(Double))
            oDTFinal.Columns.Add("DocTotal", GetType(Double))

            For iParent As Integer = 0 To oDataset.Tables(1).Rows.Count - 1

                If oDataset.Tables(1).Rows(iParent).Item(0).ToString = "D" Then

                    For iChild As Integer = 0 To oDataset.Tables(0).Rows.Count - 1

                        If oDataset.Tables(1).Rows(iParent).Item(5).ToString = oDataset.Tables(0).Rows(iChild).Item(0) Then

                            oDTFinal.Rows.Add(oDataset.Tables(1).Rows(iParent).Item(5).ToString _
                                            , oDataset.Tables(0).Rows(iChild).Item(2).ToString _
                                             , oDataset.Tables(0).Rows(iChild).Item(4).ToString _
                                            , oDataset.Tables(1).Rows(iParent).Item(11) _
                                            , oDataset.Tables(0).Rows(iChild).Item(3).ToString _
                                            , oDataset.Tables(1).Rows(iParent).Item(9) _
                                            , oDataset.Tables(1).Rows(iParent).Item(14))
                        End If

                    Next

                End If

            Next

            oDataset.Tables.Add(oDTFinal)

            Dim oDView As DataView = New DataView(oDataset.Tables(2))
            Dim oDistDT As DataTable = Nothing
            Dim dTotUnitPrice As Double = 0
            oDistDT = oDView.Table.DefaultView.ToTable(True, "ItmGrpCode")
            dDocTotal = 0
            For iRow As Integer = 0 To oDistDT.Rows.Count - 1

                If String.IsNullOrEmpty(oDistDT.Rows(iRow).Item(0).ToString) Then Continue For

                dTotalAmount = 0
                dTotUnitPrice = 0

                oDView.RowFilter = "ItmGrpCode = '" & oDistDT.Rows(iRow).Item(0).ToString & "'"

                For iRowCount As Integer = 0 To oDView.Count - 1

                    dTotalAmount += oDView.Item(iRowCount).Row(3).ToString
                    dTotUnitPrice += oDView.Item(iRowCount).Row(5).ToString
                    dDocTotal += oDView.Item(iRowCount).Row(6).ToString

                Next

                oARInvoice.Lines.AccountCode = oDView.Item(0).Row(2).ToString
                oARInvoice.Lines.ItemDescription = oDView.Item(0).Row(4).ToString
                oARInvoice.Lines.TaxCode = "SO"
                oARInvoice.Lines.COGSCostingCode = sLocation
                oARInvoice.Lines.CostingCode = sLocation
                oARInvoice.Lines.LocationCode = oCommon.GetSingleValue("SELECT T0.[Code] FROM OLCT T0 WHERE T0.[Location]  ='" & sLocation & "'", oDICompany, sErrDesc)

                oARInvoice.Lines.UnitPrice = dTotUnitPrice
                oARInvoice.Lines.LineTotal = dTotalAmount
                oARInvoice.Lines.Add()

            Next

            oARInvoice.DocTotal = dDocTotal

            lRetCode = oARInvoice.Add
            If lRetCode <> 0 Then
                sErrDesc = oDICompany.GetLastErrorDescription
                Call WriteToLogFile(sErrDesc, sFuncName)
                If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed with ERROR", sFuncName)
                ARInvoice = RTN_ERROR
            Else
                oDICompany.GetNewObjectCode(sDocEntry)
                If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed with SUCCESS : " & sDocEntry, sFuncName)

                If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Calling AR_IncomingPayment() ", sFuncName)

                If AR_IncomingPayment(DataView_, sDocEntry, oDICompany, sErrDesc) = RTN_SUCCESS Then
                    ARInvoice = RTN_SUCCESS
                Else
                    sErrDesc = oDICompany.GetLastErrorDescription
                    Call WriteToLogFile(sErrDesc, sFuncName)
                    If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed with ERROR", sFuncName)
                    ARInvoice = RTN_ERROR
                End If
            End If

        Catch ex As Exception
            sErrDesc = ex.Message
            Call WriteToLogFile(sErrDesc, sFuncName)
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed with ERROR", sFuncName)
            ARInvoice = RTN_ERROR
        End Try
    End Function

    Function AR_IncomingPayment(ByRef DataView_ As DataView, ByVal sDocEntry As String, ByRef oDICompany As SAPbobsCOM.Company, _
                                ByRef sErrDesc As String) As Long
        Dim sFuncName As String = String.Empty
        Dim sPayDocEntry As String = String.Empty
        Dim lRetCode As Long
        Dim iCount As Int16 = 0
        Dim oCommon As clsCommon = New clsCommon
        Dim oDV_CreditCard As New DataView(p_sCreditCard)
        Dim sCreditCardName As String = String.Empty
        Dim sLocation As String = String.Empty

        Try
            sFuncName = "AR_IncomingPayment"

            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Starting Function", sFuncName)

            Dim sIncomeDate As String = DataView_.Item(0).Row(3).ToString
            Dim dIncomeDate As Date
            If sIncomeDate.Length = 8 Then
                dIncomeDate = Left(sIncomeDate, 4) & "/" & Mid(sIncomeDate, 5, 2) & "/" & Right(sIncomeDate, 2)
            ElseIf sIncomeDate.Length = 7 Then
                dIncomeDate = Left(sIncomeDate, 4) & "/" & Mid(sIncomeDate, 5, 2) & "/0" & Right(sIncomeDate, 1)
            ElseIf sIncomeDate.Length = 6 Then
                dIncomeDate = Left(sIncomeDate, 4) & "/0" & Mid(sIncomeDate, 5, 1) & "/0" & Right(sIncomeDate, 1)
            End If

            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Fetched Date", sFuncName)

            Dim oIncomingPayment As SAPbobsCOM.Payments = oDICompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oIncomingPayments)
            sLocation = DataView_.Item(0).Row(1).ToString

            oIncomingPayment.DocType = SAPbobsCOM.BoRcptTypes.rCustomer
            oIncomingPayment.CardCode = CStr(sCardCode)
            oIncomingPayment.DocDate = dIncomeDate
            oIncomingPayment.DueDate = dIncomeDate
            oIncomingPayment.DueDate = dIncomeDate
            oIncomingPayment.Series = oCommon.GetSingleValue("SELECT T0.[Series] FROM NNM1 T0 WHERE T0.[ObjectCode] ='24' and  T0.[SeriesName] ='" & DataView_.Item(0).Row(2).ToString & "-IP-" & Mid(sIncomeDate, 3, 2) & "'", oDICompany, sErrDesc)

            oIncomingPayment.DocObjectCode = SAPbobsCOM.BoPaymentsObjectType.bopot_IncomingPayments
            oIncomingPayment.Invoices.DocEntry = sDocEntry
            oIncomingPayment.Invoices.InvoiceType = SAPbobsCOM.BoRcptInvTypes.it_Invoice
            oIncomingPayment.Invoices.DistributionRule = sLocation

            oIncomingPayment.Invoices.Add()

            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Add Event Starts", sFuncName)

            For IntRow As Integer = 0 To DataView_.Count - 1

                If DataView_.Item(IntRow).Row(0).ToString.ToUpper = "P" Then
                    If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Start CreditCard Details : " & IntRow, sFuncName)

                    sCreditCardName = oCommon.GetSingleValue("SELECT T0.[CreditCard] FROM " & sRetailDB & "..OCRC T0 WHERE T0.[CardName] ='" & DataView_.Item(IntRow).Row(15).ToString & "'", oDICompany, sErrDesc)
                    oIncomingPayment.CreditCards.CreditCard = sCreditCardName
                    oDV_CreditCard.RowFilter = "Code = '" & DataView_.Item(IntRow).Row(15).ToString & "'"
                    oIncomingPayment.CreditCards.CreditCardNumber = oDV_CreditCard.Item(0).Row(1).ToString
                    oIncomingPayment.CreditCards.CardValidUntil = Convert.ToDateTime("20" & Right(oDV_CreditCard.Item(0).Row(2).ToString, 2) & "-" & Left(oDV_CreditCard.Item(0).Row(2).ToString, 2) & "-01")
                    oIncomingPayment.CreditCards.CreditAcct = oDV_CreditCard.Item(0).Row(3).ToString
                    oIncomingPayment.CreditCards.VoucherNum = Left(sIncomeDate, 4) & Mid(sIncomeDate, 5, 2) & Right(sIncomeDate, 2) & "-" & iCount + 1
                    oIncomingPayment.CreditCards.CreditSum = CDbl(DataView_.Item(IntRow).Row(17))

                    If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("CreditCard " & sCreditCardName & " CreditCardNumber " & oDV_CreditCard.Item(0).Row(1).ToString & " CardValidUntil " & Convert.ToDateTime("20" & Right(oDV_CreditCard.Item(0).Row(2).ToString, 2) & "-" & Left(oDV_CreditCard.Item(0).Row(2).ToString, 2) & "-01") & " CreditAcct " & oDV_CreditCard.Item(0).Row(3).ToString & " VoucherNum " & Left(sIncomeDate, 4) & Mid(sIncomeDate, 5, 2) & Right(sIncomeDate, 2) & "-" & iCount + 1, sFuncName)

                    oIncomingPayment.CreditCards.Add()

                    If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Added CreditCard Details : " & IntRow, sFuncName)
                    iCount = iCount + 1
                End If
            Next
            oIncomingPayment.CashSum = 0

            lRetCode = oIncomingPayment.Add()

            If lRetCode <> 0 Then
                sErrDesc = oDICompany.GetLastErrorDescription
                Call WriteToLogFile(sErrDesc, sFuncName)
                If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed with ERROR", sFuncName)
                AR_IncomingPayment = RTN_ERROR
                Return RTN_ERROR
            Else
                oDICompany.GetNewObjectCode(sPayDocEntry)
                If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed with SUCCESS : " & sPayDocEntry, sFuncName)
                Return RTN_SUCCESS
            End If
        Catch ex As Exception
            sErrDesc = ex.Message
            Call WriteToLogFile(sErrDesc, sFuncName)
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed with ERROR", sFuncName)
            AR_IncomingPayment = RTN_ERROR
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

        Dim connetionString As String
        Dim connection As SqlConnection
        Dim command As SqlCommand
        Dim adapter As New SqlDataAdapter
        Dim ds As New DataSet
        Dim i As Integer
        Dim sql As String

        connetionString = sConString
        sql = sQuery

        connection = New SqlConnection(connetionString)

        Try
            Dim sFuncName As String = String.Empty
            sFuncName = "ExecuteSQLQuery()"
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Starting Function", sFuncName)

            connection.Open()
            command = New SqlCommand(sql, connection)
            adapter.SelectCommand = command
            adapter.Fill(ds)
            adapter.Dispose()
            command.Dispose()
            connection.Close()

        Catch ex As Exception
            MsgBox("Can not open connection ! ")
        End Try

        Return ds

    End Function

    Function QuantityChecking(ByVal sItemCode As String, ByVal sWhsCode As String, ByVal sConnString As String)

        Dim connetionString As String
        Dim connection As SqlConnection
        Dim command As SqlCommand
        Dim adapter As New SqlDataAdapter
        Dim ds As New DataSet
        Dim sql As String
        Dim dQuantity As Double

        connetionString = sConnString
        sql = "SELECT T0.[OnHand] FROM OITW T0 WHERE T0.[ItemCode] ='" & sItemCode & "' and T0.[WhsCode]='" & sWhsCode & "'"

        connection = New SqlConnection(connetionString)

        Try
            Dim sFuncName As String = String.Empty
            sFuncName = "QuantityChecking()"
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Starting Function", sFuncName)

            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Query : " & sql, sFuncName)

            connection.Open()
            command = New SqlCommand(sql, connection)
            adapter.SelectCommand = command
            adapter.Fill(ds)
            adapter.Dispose()
            command.Dispose()
            connection.Close()

            If ds.Tables(0).Rows.Count > 0 Then

            End If


        Catch ex As Exception
            MsgBox("Can not open connection ! ")
        End Try
        Return True

    End Function

End Class
