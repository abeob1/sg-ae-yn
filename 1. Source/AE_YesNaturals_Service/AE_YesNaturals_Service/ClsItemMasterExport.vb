Public Class ClsItemMasterExport

    Public Function ItemMasterExport(ByRef sErrDesc As String) As Long

        ' ***********************************************************************************
        '   Function   :    ItemMasterExport()
        '   Purpose    :    This function is handles - Export Item Master Data from SAP to CSV file into the OUTPUT Folder.
        '   Parameters :    ByRef sErrDesc As String
        '                       sErrDesc=Error Description to be returned to calling function
        '   Return      :   0 - FAILURE
        '                   1 - SUCCESS
        '   Author     :    SRINIVASAN
        '   Date       :    15/08/2014 
        '   Change     :   
        '                   
        ' ***********************************************************************************


        Dim sFuncName As String = String.Empty
        Dim oItemMaster As AE_YESNATURALS_DLL.clsItemMaster = New AE_YESNATURALS_DLL.clsItemMaster
        Dim sQuery As String = String.Empty

        Try
            sFuncName = "btnExportCSV_Click()"
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Starting Function", sFuncName)

            sQuery = "select * from AE_VW001_ItemMasterExport T0 where T0.ListName = '" & p_oCompDef.p_sPriceList & "'"

            sQuery += " and (case when isnull(T0.UpdateDate,'')='' then T0.CreateDate else T0.UpdateDate end)=convert(date,Getdate(),108)"

            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Query " & sQuery, sFuncName)

            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Calling ExportToCSV()", sFuncName)

            P_sConString = String.Empty
            P_sConString = "Data Source=" & p_oCompDef.p_sServerName & ";Initial Catalog=" & p_oCompDef.p_sTradingDBName & ";User ID=" & p_oCompDef.p_sDBUserName & "; Password=" & p_oCompDef.p_sDBPassword


            If oItemMaster.ExportToCSV(sQuery, P_sConString, sErrDesc) <> RTN_SUCCESS Then
                ' Throw New ArgumentException(sErrDesc)
                If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Calling ExportToCSV() " & sErrDesc, sFuncName)
            Else
                If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("The CSV File has been Exported in the OUTPUT Folder", sFuncName)
            End If

            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed with SUCCESS", sFuncName)
            ItemMasterExport = RTN_SUCCESS
            Return RTN_SUCCESS

        Catch ex As Exception
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed with ERROR", sFuncName)
            WriteToLogFile(ex.Message, sFuncName)
            ItemMasterExport = RTN_ERROR
            Return RTN_ERROR
        End Try

    End Function

End Class
