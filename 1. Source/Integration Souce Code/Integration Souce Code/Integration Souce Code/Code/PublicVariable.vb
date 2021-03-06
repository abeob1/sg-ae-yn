Imports System.Data.SqlClient
Imports System.Configuration

Public Class PublicVariable
    Public Shared company As SAPbobsCOM.Company ' = New SAPbobsCOM.Company
    Private Shared arrayConnection As Array
    Public Shared Timer As Integer = 1 'minute
    Public Shared crdAccount As String = ""
    Public Shared timetoCreateJE As String
    Public Shared Property oCompany As SAPbobsCOM.Company
        Get
            Return company
        End Get
        Set(ByVal value As SAPbobsCOM.Company)
            company = value
        End Set
    End Property
    Public Shared ReadOnly Property CreditAccount As String
        Get
            Return crdAccount
        End Get
    End Property
    Public Shared ReadOnly Property TimeCreateJE As String
        Get
            Return timetoCreateJE
        End Get
    End Property
    Public Shared ReadOnly Property Database As String
        Get
            Return arrayConnection(0).ToString()
        End Get
    End Property
    Public Shared ReadOnly Property SAPUser As String
        Get
            Return arrayConnection(1).ToString()
        End Get
    End Property
    Public Shared ReadOnly Property SAPPassword As String
        Get
            Return arrayConnection(2).ToString()
        End Get
    End Property
    Public Shared ReadOnly Property ServerName As String
        Get
            Return arrayConnection(3).ToString()
        End Get
    End Property
    Public Shared ReadOnly Property UserName As String
        Get
            Return arrayConnection(4).ToString()
        End Get
    End Property
    Public Shared ReadOnly Property Password As String
        Get
            Return arrayConnection(5).ToString()
        End Get
    End Property
    Public Shared ReadOnly Property LicenseServer As String
        Get
            Return arrayConnection(6).ToString()
        End Get
    End Property

    Public Shared Sub GetSettingInfo()
#If DEBUG Then
        Dim fileMap As New ExeConfigurationFileMap()
        Dim config As System.Configuration.Configuration
        fileMap.ExeConfigFilename = "SAPIntegration.exe.config"
        Functions.WriteLog(fileMap.ExeConfigFilename)
        config = ConfigurationManager.OpenMappedExeConfiguration(fileMap, ConfigurationUserLevel.None)
        '   Sets values to config file.
        If config.HasFile() Then
            crdAccount = config.AppSettings.Settings.Item("CreditAccount").Value
            Functions.WriteLog(crdAccount)
            timetoCreateJE = config.AppSettings.Settings.Item("TimeCreateJE").Value.ToString()
            Functions.WriteLog(timetoCreateJE)
            arrayConnection = config.AppSettings.Settings.Item("SAPConnectionString").Value.Split(";")
            Functions.WriteLog(arrayConnection.ToString)
        End If
#Else
                arrayConnection = System.Configuration.ConfigurationSettings.AppSettings.Get("SAPConnectionString").Split(";")
                timetoCreateJE = System.Configuration.ConfigurationSettings.AppSettings.Get("TimeCreateJE")
                crdAccount = System.Configuration.ConfigurationSettings.AppSettings.Get("CreditAccount")
#End If
        'arrayConnection = System.Configuration.ConfigurationSettings.AppSettings.Get("SAPConnectionString").Split(";")
        'timetoCreateJE = System.Configuration.ConfigurationSettings.AppSettings.Get("TimeCreateJE")
        'crdAccount = System.Configuration.ConfigurationSettings.AppSettings.Get("CreditAccount")
    End Sub
End Class
