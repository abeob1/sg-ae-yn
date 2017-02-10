Imports System.Text
Imports System.Xml

Public Class oXML
    Public Function ToXMLStringFromDS(ObjType As String, ds As DataSet) As String
        Try
            'Dim gf As New GeneralFunctions()
            Dim XmlString As New StringBuilder()
            Dim writer As XmlWriter = XmlWriter.Create(XmlString)
            writer.WriteStartDocument()
            If True Then
                writer.WriteStartElement("BOM")
                If True Then
                    writer.WriteStartElement("BO")
                    If True Then
                        '#Region "write ADMINFO_ELEMENT"
                        writer.WriteStartElement("AdmInfo")
                        If True Then
                            writer.WriteStartElement("Object")
                            If True Then
                                writer.WriteString(ObjType)
                            End If
                            writer.WriteEndElement()
                        End If
                        writer.WriteEndElement()
                        '#End Region

                        '#Region "Header&Line XML"
                        For Each dt As DataTable In ds.Tables
                            If dt.Rows.Count > 0 Then
                                writer.WriteStartElement(dt.TableName.ToString())
                                If True Then
                                    For Each row As DataRow In dt.Rows
                                        writer.WriteStartElement("row")
                                        If True Then
                                            For Each column As DataColumn In dt.Columns
                                                If column.DefaultValue.ToString() <> "xx_remove_xx" Then
                                                    If row(column).ToString() <> "" Then
                                                        writer.WriteStartElement(column.ColumnName)
                                                        'Write Tag
                                                        If True Then
                                                            writer.WriteString(row(column).ToString())
                                                        End If
                                                        writer.WriteEndElement()
                                                    End If
                                                End If
                                            Next
                                        End If
                                        writer.WriteEndElement()
                                    Next
                                End If
                                writer.WriteEndElement()
                            End If
                        Next
                        '#End Region
                    End If
                    writer.WriteEndElement()
                End If
                writer.WriteEndElement()
            End If
            writer.WriteEndDocument()

            writer.Flush()

            Return XmlString.ToString()
        Catch ex As Exception
            Return ex.Message()
        End Try
    End Function
    
    Public Function CreateMarketingDocument(ByVal Key As String, ByVal strXml As String, ByVal DocType As String) As String
        Try
            Dim sStr As String = ""
            Dim lErrCode As Integer
            Dim sErrMsg As String
            Dim oDocment
            Select Case DocType
                Case "30"
                    oDocment = DirectCast(oDocment, SAPbobsCOM.JournalEntries)
                Case "97"
                    oDocment = DirectCast(oDocment, SAPbobsCOM.SalesOpportunities)
                Case "191"
                    oDocment = DirectCast(oDocment, SAPbobsCOM.ServiceCalls)
                Case "33"
                    oDocment = DirectCast(oDocment, SAPbobsCOM.Contacts)
                Case "221"
                    oDocment = DirectCast(oDocment, SAPbobsCOM.Attachments2)
                Case "2"
                    oDocment = DirectCast(oDocment, SAPbobsCOM.BusinessPartners)
                Case Else
                    oDocment = DirectCast(oDocment, SAPbobsCOM.Documents)
            End Select

            sErrMsg = Functions.SystemInitial
            If sErrMsg <> "" Then
                Return sErrMsg
            End If

            PublicVariable.oCompany.XMLAsString = True
            oDocment = PublicVariable.oCompany.GetBusinessObjectFromXML(strXml, 0)
            If Key <> "" Then
                If oDocment.getbyKey(Key) = True Then
                    lErrCode = oDocment.Update()
                Else
                    lErrCode = oDocment.add()
                End If
            Else
                lErrCode = oDocment.add()
            End If


            If lErrCode <> 0 Then
                PublicVariable.oCompany.GetLastError(lErrCode, sErrMsg)
                Return sErrMsg
            Else
                Return ""
            End If

        Catch ex As Exception
            Return ex.ToString
        End Try
    End Function
    
End Class
