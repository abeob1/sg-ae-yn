Public Class AddJE
    Dim DocEntry_Invoice As Integer = 0
#Region "Build Table Structure"
    Private Function BuildTableOJDT() As DataTable 'Incoming Payment
        Dim dt As New DataTable("OJDT")
        dt.Columns.Add("RefDate")
        dt.Columns.Add("DueDate")
        dt.Columns.Add("Memo")
        dt.Columns.Add("Ref1")
        Return dt
    End Function
    Private Function BuildTableJDT1() As DataTable 'Incoming Payment Detail
        Dim dt As New DataTable("JDT1")
        dt.Columns.Add("ShortName")
        dt.Columns.Add("Debit")
        dt.Columns.Add("Credit")
        dt.Columns.Add("ProfitCode")
        Return dt
    End Function
#End Region
#Region "Insert into Table"
    Private Function InsertIntoJDT1(dt As DataTable, dr As DataRow) As DataTable
        'If dr("PaymentMethod") <> "CASH" And dr("PaymentMethod") <> "TRANSFER" Then
        Dim drNew As DataRow = dt.NewRow
        drNew("CardValid") = "99990101"
        Dim ccno As String = dr("CreditCardNo")
        If ccno = "" Then
            ccno = "999"
        End If
        drNew("CrCardNum") = ccno
        drNew("CreditSum") = Math.Abs(dr("Amount"))
        drNew("VoucherNum") = ccno
        dt.Rows.Add(drNew)
        'End If

        Return dt
    End Function
    Private Function InsertIntoOJDT(dt As DataTable, dr As DataRow) As DataTable
        Dim drNew As DataRow = dt.NewRow
        drNew("DocDate") = CDate(dr("PaymentDate")).ToString("yyyyMMdd")
        drNew("DocDueDate") = CDate(dr("PaymentDate")).ToString("yyyyMMdd")
        drNew("CardCode") = dr("CardCode")
        drNew("NoDocSum") = dr("Amount")

        drNew("DocType") = "C"
        drNew("CounterRef") = dr("ReceiptNo").ToString
        drNew("U_Description") = dr("Description").ToString
        drNew("U_POSTxNo") = dr("ReservationCode").ToString
        dt.Rows.Add(drNew)
        Return dt
    End Function
#End Region

#Region "Create Advance"
    Public Sub CreateJE()
        Dim DocType As String = "30"
        Dim xm As New oXML

        Try
            Dim dt As DataTable
            If Not IsNothing(dt) Then

                Dim sErrMsg As String
                sErrMsg = Functions.SystemInitial
                If sErrMsg <> "" Then
                    Functions.WriteLog(sErrMsg)
                    Return
                End If
                For Each dr As DataRow In dt.Rows
                    CreateJE_CancelFee(dr)
                Next
            End If
        Catch ex As Exception
            Functions.WriteLog(ex.Message)
        End Try
    End Sub

    Private Function CreateJE_CancelFee(dr As DataRow) As String
        Dim dtOJDT As DataTable = BuildTableOJDT()
        Dim dtJDT1 As DataTable = BuildTableJDT1()
        Dim HeaderID As String = dr.Item("ID")
        Dim Memo As String = "Cancellation Charge - " + dr("CardCode").ToString

        Dim drNew As DataRow = dtOJDT.NewRow
        drNew("RefDate") = CDate(dr("PaymentDate")).ToString("yyyyMMdd")
        drNew("DueDate") = CDate(dr("PaymentDate")).ToString("yyyyMMdd")
        drNew("Memo") = Memo
        drNew("Ref1") = "Cancellation Charge"
        dtOJDT.Rows.Add(drNew)

        drNew = dtJDT1.NewRow
        drNew("ShortName") = dr("CardCode").ToString
        drNew("Credit") = 0
        drNew("Debit") = dr("Amount")
        dtJDT1.Rows.Add(drNew)

        drNew = dtJDT1.NewRow
        drNew("ShortName") = "7002001002"
        drNew("Credit") = dr("Amount")
        drNew("Debit") = 0
        dtJDT1.Rows.Add(drNew)

        Dim ds As DataSet = New DataSet
        ds.Tables.Add(dtOJDT.Copy)
        ds.Tables.Add(dtJDT1.Copy)
        Dim DocType As String = "30"
        Dim ret As String = ""
        Dim xm As New oXML

        Dim xmlstr As String = xm.ToXMLStringFromDS(DocType, ds)
        ret = xm.CreateMarketingDocument("", xmlstr, DocType)


        If ret.Contains("'") Then
            ret = ret.Replace("'", " ")
        End If
        If xmlstr.Contains("'") Then
            xmlstr = xmlstr.Replace("'", " ")
        End If
        Return ret

    End Function
#End Region
End Class
