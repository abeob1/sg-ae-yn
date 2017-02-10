Imports System.Data.SqlClient

Public Class clsDeliveryOrder

    Function DeliveryOrder(ByRef DataView_ As DataView, ByVal oDICompany As SAPbobsCOM.Company, _
                       ByRef sErrDesc As String) As Long

        'Function   :   DeliveryOrder()
        'Purpose    :   Create a Delivery Order through DI API
        'Parameters :   ByRef DataView_ As DataView
        '               oDICompany As SAPbobsCOM.Company
        '               ByRef sErrDesc As String

        '                   sErrDesc=Error Description to be returned to calling function

        'Return     :   0 - FAILURE
        '               1 - SUCCESS

        'Author     :   SRINIVASAN
        'Date       :   01/09/2014  
        'Change     :

        Dim sFuncName As String = String.Empty
        Dim lRetCode As Long
        Dim sDocEntry As String = String.Empty
        Dim oCommon As clsCommon = New clsCommon
        Dim dDocTotal As Double = 0.0
        Dim sItemCode As String = String.Empty
        Dim sWarehouseCode As String = String.Empty
        Dim sCustRefNum As String = String.Empty
        Dim sCustRefVal As String = String.Empty
        Dim sQueryString As String = String.Empty
        Dim sExcelLineNum As String = String.Empty
        Dim sPOSNumber As String = String.Empty
        Dim sLocation As String = String.Empty
        Dim dQuantity As Double = 0.0
        Dim dOnHand As Double = 0.0
        Dim sQtyErrorMsg As String = String.Empty
        Dim sInventryItem As String = String.Empty

        Try
            sFuncName = "DeliveryOrder()"
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Starting Function", sFuncName)

            Dim sDeliveryDate As String = DataView_.Item(0).Row(3).ToString
            Dim dDelDocDate As Date
            If sDeliveryDate.Length = 8 Then
                dDelDocDate = Left(sDeliveryDate, 4) & "/" & Mid(sDeliveryDate, 5, 2) & "/" & Right(sDeliveryDate, 2)
            ElseIf sDeliveryDate.Length = 7 Then
                dDelDocDate = Left(sDeliveryDate, 4) & "/" & Mid(sDeliveryDate, 5, 2) & "/0" & Right(sDeliveryDate, 1)
            ElseIf sDeliveryDate.Length = 6 Then
                dDelDocDate = Left(sDeliveryDate, 4) & "/0" & Mid(sDeliveryDate, 5, 1) & "/0" & Right(sDeliveryDate, 1)
            End If

            Dim oDeliveryOrder As SAPbobsCOM.Documents = oDICompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oDeliveryNotes)

            sLocation = DataView_.Item(0).Row(2).ToString

            sCustRefNum = "POS-" & Left(sDeliveryDate, 4) & Mid(sDeliveryDate, 5, 2) & Right(sDeliveryDate, 2) & "-" & sLocation

            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Calling GetSingleValue() ", sFuncName)

            sCustRefVal = oCommon.GetSingleValue("select distinct  DocEntry from ODLN where NumAtCard ='" & sCustRefNum & "' ", oDICompany, sErrDesc)

            If String.IsNullOrEmpty(sCustRefVal) = False Then

                If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Already Exists. Reference Num : " & sCustRefNum, sFuncName)

                If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed with SUCCESS.", sFuncName)

                Return RTN_SUCCESS

            End If


            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Calling GetSingleValues() Function", sFuncName)

            oDeliveryOrder.CardCode = oCommon.GetSingleValue("SELECT T0.[U_CardCode] FROM [dbo].[@LOCATIONMAPPING]  T0 WHERE T0.[Code] ='" & DataView_.Item(0).Row(2).ToString & "'", oDICompany, sErrDesc)

            oDeliveryOrder.NumAtCard = sCustRefNum  'Left(sDeliveryDate, 4) & Mid(sDeliveryDate, 5, 2) & Right(sDeliveryDate, 2) & " - " & DataView_.Item(0).Row(2).ToString
            oDeliveryOrder.DocDate = dDelDocDate
            oDeliveryOrder.DocDueDate = dDelDocDate
            oDeliveryOrder.TaxDate = dDelDocDate
            oDeliveryOrder.DocType = SAPbobsCOM.BoDocumentTypes.dDocument_Items

            ''dDocTotal = 0.0

            For IntRow As Integer = 0 To DataView_.Count - 1

                If DataView_.Item(IntRow).Row(0).ToString.ToUpper = "D" Then

                    sItemCode = DataView_.Item(IntRow).Row(5).ToString
                    sWarehouseCode = DataView_.Item(IntRow).Row(2).ToString
                    sPOSNumber = CStr(DataView_.Item(IntRow).Row(4).ToString)
                    dQuantity = CDbl(DataView_.Item(IntRow).Row(7).ToString)
                    sExcelLineNum = IntRow

                    ''select InvntItem  from OITM where ItemCode ='00005314'

                    sInventryItem = oCommon.GetSingleValue("select isnull(InvntItem,'N')  from OITM where ItemCode ='" & sItemCode & "'", oDICompany, sErrDesc)

                    If sInventryItem = "N" Then Continue For

                    oDeliveryOrder.Lines.ItemCode = sItemCode
                    oDeliveryOrder.Lines.Quantity = dQuantity
                    oDeliveryOrder.Lines.WarehouseCode = sWarehouseCode

                    If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Calling GetSingleValues() Function to Get Unit Price", sFuncName)

                    oDeliveryOrder.Lines.Price = oCommon.GetSingleValue("select isnull(T0.Price,0)  from ITM1 T0 JOIN OPLN T1 on T0.PriceList =T1.ListNum " &
                                                                        "where T1.ListName =(select U_PriceList from [@LOCATIONMAPPING] " &
                                                                        " where Code='" & sWarehouseCode & "' and T0.ItemCode ='" & sItemCode & "')", oDICompany, sErrDesc)

                    oDeliveryOrder.Lines.COGSCostingCode = DataView_.Item(IntRow).Row(1).ToString
                    ''  oDeliveryOrder.Lines.LineTotal = CDbl(DataView_.Item(IntRow).Row(11).ToString)
                    oDeliveryOrder.Lines.TaxCode = "SO"
                    oDeliveryOrder.Lines.UserFields.Fields.Item("U_AB_POSNum").Value = sPOSNumber


                    oDeliveryOrder.Lines.Add()

                End If
            Next
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Before Add : " & sQueryString, sFuncName)

            lRetCode = oDeliveryOrder.Add

            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("After Add : lRetCode " & lRetCode, sFuncName)

            If lRetCode <> 0 Then
                sErrDesc = oDICompany.GetLastErrorDescription
                Call WriteToLogFile(sErrDesc, sFuncName)

                If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed with ERROR", sFuncName)

                DeliveryOrder = RTN_ERROR
            Else

                If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed with SUCCESS : " & sDocEntry, sFuncName)
                DeliveryOrder = RTN_SUCCESS
            End If

        Catch ex As Exception
            sErrDesc = ex.Message
            Call WriteToLogFile(sErrDesc, sFuncName)
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed with ERROR", sFuncName)
            DeliveryOrder = RTN_ERROR
        End Try
    End Function

    Function Quantity_Checking(ByRef DataView_ As DataView, ByVal oDICompany As SAPbobsCOM.Company, _
                       ByRef sErrDesc As String) As Long

        'Function   :   DeliveryOrder()
        'Purpose    :   Create a Delivery Order through DI API
        'Parameters :   ByRef DataView_ As DataView
        '               oDICompany As SAPbobsCOM.Company
        '               ByRef sErrDesc As String

        '                   sErrDesc=Error Description to be returned to calling function

        'Return     :   0 - FAILURE
        '               1 - SUCCESS

        'Author     :   SRINIVASAN
        'Date       :   01/09/2014  
        'Change     :

        Dim sFuncName As String = String.Empty
        Dim sDocEntry As String = String.Empty
        Dim oCommon As clsCommon = New clsCommon
        Dim dDocTotal As Double = 0.0
        Dim sItemCode As String = String.Empty
        Dim sWarehouseCode As String = String.Empty
        Dim sCustRefNum As String = String.Empty
        Dim sCustRefVal As String = String.Empty
        ''  Dim sQueryString As String = String.Empty
        Dim sExcelLineNum As String = String.Empty
        Dim sPOSNumber As String = String.Empty
        Dim sLocation As String = String.Empty
        Dim dQuantity As Double = 0.0
        Dim dOnHand As Double = 0.0
        Dim sQtyErrorMsg As String = String.Empty
        Dim sInventryItem As String = String.Empty

        Dim oDTDistItemcode As DataTable = New DataTable

        Try
            sFuncName = "Quantity_Checking()"
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Starting Function", sFuncName)

            Dim sDeliveryDate As String = DataView_.Item(0).Row(3).ToString
            Dim dDelDocDate As Date
            If sDeliveryDate.Length = 8 Then
                dDelDocDate = Left(sDeliveryDate, 4) & "/" & Mid(sDeliveryDate, 5, 2) & "/" & Right(sDeliveryDate, 2)
            ElseIf sDeliveryDate.Length = 7 Then
                dDelDocDate = Left(sDeliveryDate, 4) & "/" & Mid(sDeliveryDate, 5, 2) & "/0" & Right(sDeliveryDate, 1)
            ElseIf sDeliveryDate.Length = 6 Then
                dDelDocDate = Left(sDeliveryDate, 4) & "/0" & Mid(sDeliveryDate, 5, 1) & "/0" & Right(sDeliveryDate, 1)
            End If

            sLocation = DataView_.Item(0).Row(2).ToString
            sQueryString = String.Empty

            sCustRefNum = "POS-" & Left(sDeliveryDate, 4) & Mid(sDeliveryDate, 5, 2) & Right(sDeliveryDate, 2) & "-" & sLocation

            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Calling GetSingleValue() ", sFuncName)

            sCustRefVal = oCommon.GetSingleValue("select distinct  DocEntry from ODLN where NumAtCard ='" & sCustRefNum & "' ", oDICompany, sErrDesc)

            If String.IsNullOrEmpty(sCustRefVal) = False Then

                If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Already Exists. Reference Num : " & sCustRefNum, sFuncName)

                If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed with SUCCESS.", sFuncName)

                Return RTN_SUCCESS

            End If


            Dim sCode As String = "D"

            DataView_.RowFilter = "F1 = '" & sCode & "'"

            oDTDistItemcode = DataView_.Table.DefaultView.ToTable(True, "F6")

            For iItemCount As Integer = 0 To oDTDistItemcode.Rows.Count - 1

                sItemCode = oDTDistItemcode.Rows(iItemCount).Item(0).ToString

                If sItemCode = "ITEM_CODE" Then Continue For

                sInventryItem = oCommon.GetSingleValue("select isnull(InvntItem,'N')  from OITM where ItemCode ='" & sItemCode & "'", oDICompany, sErrDesc)

                If sInventryItem = "N" Then Continue For

                DataView_.RowFilter = "F6 = '" & sItemCode & "' and F3='" & sLocation & "'"

                dQuantity = 0
                sPOSNumber = String.Empty

                If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Dataview Row Count :" & DataView_.Count - 1 & " ItemCode : " & sItemCode, sFuncName)

                For iRowCount As Integer = 0 To DataView_.Count - 1
                    If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Before Add the Quantity Row :" & iRowCount & " ItemCode : " & sItemCode, sFuncName)

                    dQuantity += CDbl(DataView_.Item(iRowCount).Row(7).ToString)

                    sPOSNumber += CStr(DataView_.Item(iRowCount).Row(4).ToString) & ","

                    If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("After Added the Quantity Row :" & iRowCount & " ItemCode : " & sItemCode, sFuncName)
                Next

                If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("After the Loop :" & DataView_.Count - 1 & " ItemCode : " & sItemCode, sFuncName)

                If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Calling GetSingleValue() for Taking the stock from SAP", sFuncName)

                dOnHand = oCommon.GetSingleValue("SELECT ISNULL(T0.[OnHand],0) FROM OITW T0 WITH (NOLOCK) WHERE T0.[ItemCode] ='" & sItemCode & "' and  T0.[WhsCode] ='" & sLocation & "'", oDICompany, sErrDesc)

                If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("ItemCode : " & sItemCode & " Quantity : " & dQuantity & " OnHand : " & dOnHand, sFuncName)

                If (dQuantity > dOnHand) Then

                    sPOSNumber = sPOSNumber.Substring(0, sPOSNumber.Length - 1)

                    If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Inside the Falls Quantity : ", sFuncName)

                    sQtyErrorMsg = "Quantity Falls : Invoice Quantity : " & dQuantity & " : OnHand Quantity : " & dOnHand

                    sQueryString += " insert into AB_ErrorLog Values(GETDATE (),convert(date,'" & dDelDocDate & "',103),'" & sExcelLineNum & "' " & _
                        " ,'" & sPOSNumber & "','" & sItemCode & "','" & sLocation & "','" & dQuantity & "','" & sQtyErrorMsg & "')"

                    If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("After Inside the Falls Quantity : " & sQueryString, sFuncName)

                End If

            Next

            If sQueryString <> String.Empty Then

                If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Calling Update_ErrorLogTable() Function for Update the Error Log", sFuncName)


                Update_ErrorLogTable(sQueryString, sErrDesc)


                If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed with SUCCESS", sFuncName)

                Return RTN_ERROR

            End If

            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed with SUCCESS", sFuncName)
            Quantity_Checking = RTN_SUCCESS

        Catch ex As Exception
            sErrDesc = ex.Message
            Call WriteToLogFile(sErrDesc, sFuncName)
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed with ERROR", sFuncName)
            Quantity_Checking = RTN_ERROR
        End Try
    End Function

    Private Function Update_ErrorLogTable(ByVal sQueryString As String, ByRef sErrDesc As String) As Long
        Dim con As New SqlConnection
        Dim cmd As New SqlCommand
        Dim sFuncName As String = String.Empty
        Try
            sFuncName = "Update_ErrorLogTable()"

            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Starting Function", sFuncName)

            con.ConnectionString = sConnString
            con.Open()
            cmd.Connection = con

            cmd = New SqlCommand(sQueryString, con)

            cmd.ExecuteNonQuery()

            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed with SUCCESS", sFuncName)

            Update_ErrorLogTable = RTN_SUCCESS
        Catch ex As Exception
            sErrDesc = ex.Message
            Call WriteToLogFile(sErrDesc, sFuncName)
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed with ERROR", sFuncName)
            Update_ErrorLogTable = RTN_ERROR

        Finally
            con.Close()
        End Try

    End Function

End Class
