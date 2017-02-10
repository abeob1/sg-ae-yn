Imports System.Data
Imports System.Data.SqlClient
Imports System.IO
Imports System.ServiceProcess
Imports Microsoft.Reporting.WinForms
Imports System.Net.Mail
Imports System.IO.Packaging
Imports System
Imports System.Text
Imports System.Web.Services.Protocols
Imports System.Xml
Imports System.Xml.Serialization
Imports System.Net
Imports System.Globalization
Imports SAPbobsCOM

Public Class Functions
    'Public Shared SBO_Application As SAPbouiCOM.Application

    Public Shared Sub WriteLog(ByVal Str As String)
        Dim oWrite As IO.StreamWriter
        Dim FilePath As String
        FilePath = Application.StartupPath + "\logfile.txt"

        If IO.File.Exists(FilePath) Then
            oWrite = IO.File.AppendText(FilePath)
        Else
            oWrite = IO.File.CreateText(FilePath)
        End If
        oWrite.Write(Str + vbCrLf)
        oWrite.Close()
    End Sub

    Public Shared Sub AutoRun()
        Try
            'WriteLog(PublicVariable.TimeCreateJE + "--" + Date.Now.ToString("HH:mm"))
            If PublicVariable.TimeCreateJE = Date.Now.ToString("HH:mm") Then
                Dim document As Documents
                document = New Documents
                Dim str As String = String.Format("Data Source={0};Initial Catalog={1};User ID={2};Password={3}", PublicVariable.ServerName, PublicVariable.Database,
                                                                                                                    PublicVariable.UserName, PublicVariable.Password)
                document.CreateJE(str)
                document.CancelJE(str)
            End If
        Catch ex As Exception
            WriteLog(ex.Message)
        End Try
    End Sub
    'Public Shared Sub SetApplication()
    '    Dim sbogui As SAPbouiCOM.SboGuiApi
    '    Dim oconnection As String
    '    sbogui = New SAPbouiCOM.SboGuiApi
    '    If Environment.GetCommandLineArgs().Length = 1 Then
    '        oconnection = "0030002C0030002C00530041005000420044005F00440061007400650076002C0050004C006F006D0056004900490056"
    '    Else
    '        oconnection = Environment.GetCommandLineArgs.GetValue(1)
    '    End If

    '    Try
    '        sbogui.Connect(oconnection)
    '    Catch ex As Exception
    '        MsgBox("No SAP Application Running")
    '        End
    '    End Try
    '    SBO_Application = sbogui.GetApplication(-1)
    'End Sub
    Public Shared Sub SystemInitial()
        'SetApplication()
        Dim lRetCode As Integer
        Dim lErrCode As Integer
        Dim sErrMsg As String = ""
        Dim oCompany As SAPbobsCOM.Company = New SAPbobsCOM.Company
        Try
            'If Not oCompany.Connected Then
            oCompany.CompanyDB = PublicVariable.Database
            oCompany.UserName = PublicVariable.SAPUser
            oCompany.Password = PublicVariable.SAPPassword
            oCompany.Server = PublicVariable.ServerName
            oCompany.DbUserName = PublicVariable.UserName
            oCompany.DbPassword = PublicVariable.Password
            oCompany.LicenseServer = PublicVariable.LicenseServer
            oCompany.DbServerType = SAPbobsCOM.BoDataServerTypes.dst_MSSQL2008

            lRetCode = oCompany.Connect
            If lRetCode <> 0 Then
                PublicVariable.oCompany.GetLastError(lErrCode, sErrMsg)
                Throw New Exception(sErrMsg)
            Else
                PublicVariable.oCompany = oCompany
                WriteLog(Now.ToString() + "--" + "SystemInitial: " + "Connected to B1")
            End If
            'End If
        Catch ex As Exception
            WriteLog("SystemInitial: " + ex.Message)
        End Try
    End Sub
    'Public Shared Function GetCulture() As CultureInfo
    '    Dim adminInfo As SAPbobsCOM.AdminInfo = PublicVariable.oCompany.GetCompanyService().GetAdminInfo()

    '    Dim strCultureName As String

    '    Select Case SBO_Application.Language
    '        Case SAPbouiCOM.BoLanguages.ln_Hebrew
    '            strCultureName = "he"
    '            Exit Select
    '        Case SAPbouiCOM.BoLanguages.ln_Spanish_Ar
    '            strCultureName = "es-AR"
    '            Exit Select
    '        Case SAPbouiCOM.BoLanguages.ln_English
    '            strCultureName = "en-US"
    '            Exit Select
    '        Case SAPbouiCOM.BoLanguages.ln_Polish
    '            strCultureName = "pl"
    '            Exit Select
    '        Case SAPbouiCOM.BoLanguages.ln_English_Sg
    '            strCultureName = "en-US"
    '            Exit Select
    '        Case SAPbouiCOM.BoLanguages.ln_Spanish_Pa
    '            strCultureName = "es-PA"
    '            Exit Select
    '        Case SAPbouiCOM.BoLanguages.ln_English_Gb
    '            strCultureName = "en-GB"
    '            Exit Select
    '        Case SAPbouiCOM.BoLanguages.ln_German
    '            strCultureName = "de"
    '            Exit Select
    '        Case SAPbouiCOM.BoLanguages.ln_Serbian
    '            strCultureName = "sr-SP-Cyrl"
    '            Exit Select
    '        Case SAPbouiCOM.BoLanguages.ln_Danish
    '            strCultureName = "da"
    '            Exit Select
    '        Case SAPbouiCOM.BoLanguages.ln_Norwegian
    '            strCultureName = "no"
    '            Exit Select
    '        Case SAPbouiCOM.BoLanguages.ln_Italian
    '            strCultureName = "it"
    '            Exit Select
    '        Case SAPbouiCOM.BoLanguages.ln_Hungarian
    '            strCultureName = "hu"
    '            Exit Select
    '        Case SAPbouiCOM.BoLanguages.ln_Chinese
    '            strCultureName = "zh-CN"
    '            Exit Select
    '        Case SAPbouiCOM.BoLanguages.ln_Dutch
    '            strCultureName = "nl"
    '            Exit Select
    '        Case SAPbouiCOM.BoLanguages.ln_Finnish
    '            strCultureName = "fi"
    '            Exit Select
    '        Case SAPbouiCOM.BoLanguages.ln_Greek
    '            strCultureName = "el"
    '            Exit Select
    '        Case SAPbouiCOM.BoLanguages.ln_Portuguese
    '            strCultureName = "pt"
    '            Exit Select
    '        Case SAPbouiCOM.BoLanguages.ln_Swedish
    '            strCultureName = "sv"
    '            Exit Select
    '        Case SAPbouiCOM.BoLanguages.ln_English_Cy
    '            strCultureName = "en"
    '            Exit Select
    '        Case SAPbouiCOM.BoLanguages.ln_French
    '            strCultureName = "fr"
    '            Exit Select
    '        Case SAPbouiCOM.BoLanguages.ln_Spanish
    '            strCultureName = "es"
    '            Exit Select
    '        Case SAPbouiCOM.BoLanguages.ln_Russian
    '            strCultureName = "ru"
    '            Exit Select
    '        Case SAPbouiCOM.BoLanguages.ln_Spanish_La
    '            strCultureName = "es"
    '            Exit Select
    '        Case SAPbouiCOM.BoLanguages.ln_Czech_Cz
    '            strCultureName = "cs"
    '            Exit Select
    '        Case SAPbouiCOM.BoLanguages.ln_Slovak_Sk
    '            strCultureName = "sk"
    '            Exit Select
    '        Case SAPbouiCOM.BoLanguages.ln_Korean_Kr
    '            strCultureName = "ko"
    '            Exit Select
    '        Case SAPbouiCOM.BoLanguages.ln_Portuguese_Br
    '            strCultureName = "pt-BR"
    '            Exit Select
    '        Case SAPbouiCOM.BoLanguages.ln_Japanese_Jp
    '            strCultureName = "ja"
    '            Exit Select
    '        Case SAPbouiCOM.BoLanguages.ln_Turkish_Tr
    '            strCultureName = "tr"
    '            Exit Select
    '        Case SAPbouiCOM.BoLanguages.ln_TrdtnlChinese_Hk
    '            strCultureName = "zh-HK"
    '            Exit Select
    '        Case Else
    '            strCultureName = "en-US"
    '            Exit Select
    '    End Select

    '    Dim cultureInfo__1 As CultureInfo = CultureInfo.CreateSpecificCulture(strCultureName)

    '    cultureInfo__1.NumberFormat.CurrencyDecimalSeparator = adminInfo.DecimalSeparator
    '    cultureInfo__1.NumberFormat.CurrencyGroupSeparator = adminInfo.ThousandsSeparator
    '    cultureInfo__1.NumberFormat.NumberDecimalSeparator = adminInfo.DecimalSeparator
    '    cultureInfo__1.NumberFormat.NumberGroupSeparator = adminInfo.ThousandsSeparator

    '    Select Case adminInfo.DateTemplate
    '        Case BoDateTemplate.dt_DDMMYY
    '            cultureInfo__1.DateTimeFormat.ShortDatePattern = String.Format("dd{0}MM{0}yy", adminInfo.DateSeparator)
    '            Exit Select
    '        Case BoDateTemplate.dt_DDMMCCYY
    '            cultureInfo__1.DateTimeFormat.ShortDatePattern = String.Format("dd{0}MM{0}yyyy", adminInfo.DateSeparator)
    '            Exit Select
    '        Case BoDateTemplate.dt_MMDDYY
    '            cultureInfo__1.DateTimeFormat.ShortDatePattern = String.Format("MM{0}dd{0}yy", adminInfo.DateSeparator)
    '            Exit Select
    '        Case BoDateTemplate.dt_MMDDCCYY
    '            cultureInfo__1.DateTimeFormat.ShortDatePattern = String.Format("MM{0}dd{0}yyyy", adminInfo.DateSeparator)
    '            Exit Select
    '        Case BoDateTemplate.dt_DDMonthYYYY
    '            cultureInfo__1.DateTimeFormat.ShortDatePattern = String.Format("dd{0}MMMM{0}yyyy", adminInfo.DateSeparator)
    '            Exit Select
    '    End Select

    '    Select Case adminInfo.TimeTemplate
    '        Case BoTimeTemplate.tt_12H
    '            If True Then
    '                cultureInfo__1.DateTimeFormat.ShortTimePattern = cultureInfo__1.DateTimeFormat.ShortTimePattern.Replace("HH", "hh").Replace("H", "h")

    '                cultureInfo__1.DateTimeFormat.LongTimePattern = cultureInfo__1.DateTimeFormat.LongTimePattern.Replace("HH", "hh").Replace("H", "h")
    '            End If
    '            Exit Select
    '        Case BoTimeTemplate.tt_24H
    '            If True Then
    '                cultureInfo__1.DateTimeFormat.ShortTimePattern = cultureInfo__1.DateTimeFormat.ShortTimePattern.Replace("hh", "HH").Replace("h", "H")

    '                cultureInfo__1.DateTimeFormat.LongTimePattern = cultureInfo__1.DateTimeFormat.LongTimePattern.Replace("hh", "HH").Replace("h", "H")
    '            End If
    '            Exit Select
    '    End Select

    '    Return cultureInfo__1
    'End Function

End Class
