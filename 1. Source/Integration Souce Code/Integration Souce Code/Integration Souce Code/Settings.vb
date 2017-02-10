Imports System.Data.SqlClient
Imports System.Configuration

Public Class Settings

    Private Sub Settings_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        Dim fileMap As New ExeConfigurationFileMap()
        Dim config As System.Configuration.Configuration
        fileMap.ExeConfigFilename = "SAPIntegration.exe.config"
        config = ConfigurationManager.OpenMappedExeConfiguration(fileMap, ConfigurationUserLevel.None)
        '   Sets values to config file.
        If config.HasFile() Then
            txt_CreditAcc.Text = config.AppSettings.Settings.Item("CreditAccount").Value
            txt_TimeJE.Text = config.AppSettings.Settings.Item("TimeCreateJE").Value

            Dim MyArr As Array = config.AppSettings.Settings.Item("SAPConnectionString").Value.Split(";")
            cmb_Database.Text = MyArr(0).ToString()
            txt_SAPUser.Text = MyArr(1).ToString()
            txt_SAPPass.Text = MyArr(2).ToString()
            txt_ServerName.Text = MyArr(3).ToString()
            txt_UserName.Text = MyArr(4).ToString()
            txt_Password.Text = MyArr(5).ToString()
            txt_LicenseServer.Text = MyArr(6).ToString()
        End If
    End Sub

    Private Sub btn_Ok_Click(sender As System.Object, e As System.EventArgs) Handles btn_Ok.Click
        SaveSettings()
        Close()
    End Sub

    Private Sub cmb_Database_DropDown(sender As System.Object, e As System.EventArgs) Handles cmb_Database.DropDown
        Dim table As DataTable = New DataTable("DBName")
        table.Columns.Add("Name")
        Dim str As String = String.Format("Data Source={0};User ID={1};Password={2}", txt_ServerName.Text, txt_UserName.Text, txt_Password.Text)
        Dim oldValue As String = cmb_Database.Text
        Dim sqlConx As SqlConnection = New SqlConnection(str)
        Try

            sqlConx.Open()
            Dim tblDatabases As DataTable = sqlConx.GetSchema("Databases")
            sqlConx.Close()

            For Each row As DataRow In tblDatabases.Rows
                table.Rows.Add(row("database_name"))
            Next
            cmb_Database.DataSource = table
            cmb_Database.DisplayMember = "Name"
            cmb_Database.ValueMember = "Name"
            cmb_Database.Text = ""
            cmb_Database.SelectedText = oldValue
        Catch
            MessageBox.Show("Can not get database list", "Error", MessageBoxButtons.OK, MessageBoxIcon.[Error])
        End Try

    End Sub

    Private Sub btn_Create_Click(sender As System.Object, e As System.EventArgs) Handles btn_Create.Click
        Try
            'SaveSettings()
            'Dim str As String = String.Format("Data Source={0};Initial Catalog={1};User ID={2};Password={3}", txt_ServerName.Text, cmb_Database.Text, txt_UserName.Text, txt_Password.Text)
            'Dim sqlConx As SqlConnection = New SqlConnection(str)
            'Dim sqlCommand As SqlCommand = sqlConx.CreateCommand()
            'sqlConx.Open()

            'sqlCommand.CommandText = My.Resources.CreateTables
            'sqlCommand.ExecuteNonQuery()

            'sqlCommand.CommandText = My.Resources.SBO_SP_TransactionNotification
            'sqlCommand.ExecuteNonQuery()
            'sqlConx.Close()

            'MessageBox.Show("Table and stored procedure created successfully", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information)
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub SaveSettings()
        Try
            Dim str As String = cmb_Database.Text + ";" + txt_SAPUser.Text + ";" + txt_SAPPass.Text + ";" + txt_ServerName.Text +
                                ";" + txt_UserName.Text + ";" + txt_Password.Text + ";" + txt_LicenseServer.Text

            Dim fileMap As New ExeConfigurationFileMap()
            Dim config As System.Configuration.Configuration
            fileMap.ExeConfigFilename = "SAPIntegration.exe.config"
            config = ConfigurationManager.OpenMappedExeConfiguration(fileMap, ConfigurationUserLevel.None)

            '   Sets values to config file.
            If config.HasFile() Then

                config.AppSettings.Settings.Item("SAPConnectionString").Value = str
                config.AppSettings.Settings.Item("TimeCreateJE").Value = txt_TimeJE.Text
                config.AppSettings.Settings.Item("CreditAccount").Value = txt_CreditAcc.Text
                config.Save(ConfigurationSaveMode.Modified)
                ConfigurationManager.RefreshSection("AppSettings")
                'PublicVariable.GetSettingInfo()
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub
End Class