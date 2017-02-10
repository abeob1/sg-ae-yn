<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmItemMaster
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.gboxHeader = New System.Windows.Forms.GroupBox()
        Me.tpnlHeader = New System.Windows.Forms.TableLayoutPanel()
        Me.lblItemCodeFrom = New System.Windows.Forms.Label()
        Me.lblItemCodeTo = New System.Windows.Forms.Label()
        Me.txtItemCodeFrom = New System.Windows.Forms.TextBox()
        Me.txtItemCodeTo = New System.Windows.Forms.TextBox()
        Me.btnChooseFrom = New System.Windows.Forms.Button()
        Me.btnChooseTo = New System.Windows.Forms.Button()
        Me.lblDateFrom = New System.Windows.Forms.Label()
        Me.lblDateTo = New System.Windows.Forms.Label()
        Me.dtDateFrom = New System.Windows.Forms.DateTimePicker()
        Me.dtDateTo = New System.Windows.Forms.DateTimePicker()
        Me.TableLayoutPanel1 = New System.Windows.Forms.TableLayoutPanel()
        Me.btnExportCSV = New System.Windows.Forms.Button()
        Me.btnClear = New System.Windows.Forms.Button()
        Me.chkAllItems = New System.Windows.Forms.CheckBox()
        Me.gboxBottom = New System.Windows.Forms.GroupBox()
        Me.txtMessage = New System.Windows.Forms.TextBox()
        Me.tpnlCenter = New System.Windows.Forms.TableLayoutPanel()
        Me.pnlCenter = New System.Windows.Forms.Panel()
        Me.pnlLeft = New System.Windows.Forms.Panel()
        Me.pnlRight = New System.Windows.Forms.Panel()
        Me.pnlTop = New System.Windows.Forms.Panel()
        Me.pnlBottom = New System.Windows.Forms.Panel()
        Me.gboxHeader.SuspendLayout()
        Me.tpnlHeader.SuspendLayout()
        Me.TableLayoutPanel1.SuspendLayout()
        Me.gboxBottom.SuspendLayout()
        Me.tpnlCenter.SuspendLayout()
        Me.pnlCenter.SuspendLayout()
        Me.SuspendLayout()
        '
        'gboxHeader
        '
        Me.tpnlCenter.SetColumnSpan(Me.gboxHeader, 2)
        Me.gboxHeader.Controls.Add(Me.tpnlHeader)
        Me.gboxHeader.Dock = System.Windows.Forms.DockStyle.Fill
        Me.gboxHeader.Location = New System.Drawing.Point(23, 3)
        Me.gboxHeader.Name = "gboxHeader"
        Me.gboxHeader.Size = New System.Drawing.Size(602, 128)
        Me.gboxHeader.TabIndex = 0
        Me.gboxHeader.TabStop = False
        Me.gboxHeader.Text = "Filter"
        '
        'tpnlHeader
        '
        Me.tpnlHeader.ColumnCount = 6
        Me.tpnlHeader.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
        Me.tpnlHeader.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.tpnlHeader.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 35.0!))
        Me.tpnlHeader.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.tpnlHeader.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
        Me.tpnlHeader.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.tpnlHeader.Controls.Add(Me.lblItemCodeFrom, 0, 0)
        Me.tpnlHeader.Controls.Add(Me.lblItemCodeTo, 0, 1)
        Me.tpnlHeader.Controls.Add(Me.txtItemCodeFrom, 1, 0)
        Me.tpnlHeader.Controls.Add(Me.txtItemCodeTo, 1, 1)
        Me.tpnlHeader.Controls.Add(Me.btnChooseFrom, 2, 0)
        Me.tpnlHeader.Controls.Add(Me.btnChooseTo, 2, 1)
        Me.tpnlHeader.Controls.Add(Me.lblDateFrom, 4, 0)
        Me.tpnlHeader.Controls.Add(Me.lblDateTo, 4, 1)
        Me.tpnlHeader.Controls.Add(Me.dtDateFrom, 5, 0)
        Me.tpnlHeader.Controls.Add(Me.dtDateTo, 5, 1)
        Me.tpnlHeader.Controls.Add(Me.TableLayoutPanel1, 1, 3)
        Me.tpnlHeader.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tpnlHeader.Location = New System.Drawing.Point(3, 16)
        Me.tpnlHeader.Name = "tpnlHeader"
        Me.tpnlHeader.RowCount = 4
        Me.tpnlHeader.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.tpnlHeader.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.tpnlHeader.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.tpnlHeader.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40.0!))
        Me.tpnlHeader.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.tpnlHeader.Size = New System.Drawing.Size(596, 109)
        Me.tpnlHeader.TabIndex = 0
        '
        'lblItemCodeFrom
        '
        Me.lblItemCodeFrom.AutoSize = True
        Me.lblItemCodeFrom.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lblItemCodeFrom.Location = New System.Drawing.Point(3, 0)
        Me.lblItemCodeFrom.Name = "lblItemCodeFrom"
        Me.lblItemCodeFrom.Size = New System.Drawing.Size(99, 24)
        Me.lblItemCodeFrom.TabIndex = 0
        Me.lblItemCodeFrom.Text = "Item Code From      "
        Me.lblItemCodeFrom.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lblItemCodeTo
        '
        Me.lblItemCodeTo.AutoSize = True
        Me.lblItemCodeTo.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lblItemCodeTo.Location = New System.Drawing.Point(3, 24)
        Me.lblItemCodeTo.Name = "lblItemCodeTo"
        Me.lblItemCodeTo.Size = New System.Drawing.Size(99, 24)
        Me.lblItemCodeTo.TabIndex = 1
        Me.lblItemCodeTo.Text = "Item Code To"
        Me.lblItemCodeTo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'txtItemCodeFrom
        '
        Me.txtItemCodeFrom.Dock = System.Windows.Forms.DockStyle.Fill
        Me.txtItemCodeFrom.Location = New System.Drawing.Point(108, 3)
        Me.txtItemCodeFrom.Name = "txtItemCodeFrom"
        Me.txtItemCodeFrom.Size = New System.Drawing.Size(164, 20)
        Me.txtItemCodeFrom.TabIndex = 2
        '
        'txtItemCodeTo
        '
        Me.txtItemCodeTo.Dock = System.Windows.Forms.DockStyle.Fill
        Me.txtItemCodeTo.Location = New System.Drawing.Point(108, 27)
        Me.txtItemCodeTo.Name = "txtItemCodeTo"
        Me.txtItemCodeTo.Size = New System.Drawing.Size(164, 20)
        Me.txtItemCodeTo.TabIndex = 3
        '
        'btnChooseFrom
        '
        Me.btnChooseFrom.Dock = System.Windows.Forms.DockStyle.Fill
        Me.btnChooseFrom.Location = New System.Drawing.Point(278, 3)
        Me.btnChooseFrom.Name = "btnChooseFrom"
        Me.btnChooseFrom.Size = New System.Drawing.Size(29, 18)
        Me.btnChooseFrom.TabIndex = 4
        Me.btnChooseFrom.Text = "..."
        Me.btnChooseFrom.UseVisualStyleBackColor = True
        '
        'btnChooseTo
        '
        Me.btnChooseTo.Dock = System.Windows.Forms.DockStyle.Fill
        Me.btnChooseTo.Location = New System.Drawing.Point(278, 27)
        Me.btnChooseTo.Name = "btnChooseTo"
        Me.btnChooseTo.Size = New System.Drawing.Size(29, 18)
        Me.btnChooseTo.TabIndex = 5
        Me.btnChooseTo.Text = "..."
        Me.btnChooseTo.UseVisualStyleBackColor = True
        '
        'lblDateFrom
        '
        Me.lblDateFrom.AutoSize = True
        Me.lblDateFrom.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lblDateFrom.Location = New System.Drawing.Point(333, 0)
        Me.lblDateFrom.Name = "lblDateFrom"
        Me.lblDateFrom.Size = New System.Drawing.Size(89, 24)
        Me.lblDateFrom.TabIndex = 6
        Me.lblDateFrom.Text = "Date From           "
        Me.lblDateFrom.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lblDateTo
        '
        Me.lblDateTo.AutoSize = True
        Me.lblDateTo.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lblDateTo.Location = New System.Drawing.Point(333, 24)
        Me.lblDateTo.Name = "lblDateTo"
        Me.lblDateTo.Size = New System.Drawing.Size(89, 24)
        Me.lblDateTo.TabIndex = 7
        Me.lblDateTo.Text = "Date To"
        Me.lblDateTo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'dtDateFrom
        '
        Me.dtDateFrom.CustomFormat = "dd/MM/yyyy"
        Me.dtDateFrom.Dock = System.Windows.Forms.DockStyle.Fill
        Me.dtDateFrom.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.dtDateFrom.Location = New System.Drawing.Point(428, 3)
        Me.dtDateFrom.Name = "dtDateFrom"
        Me.dtDateFrom.Size = New System.Drawing.Size(165, 20)
        Me.dtDateFrom.TabIndex = 8
        '
        'dtDateTo
        '
        Me.dtDateTo.CustomFormat = "dd/MM/yyyy"
        Me.dtDateTo.Dock = System.Windows.Forms.DockStyle.Fill
        Me.dtDateTo.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.dtDateTo.Location = New System.Drawing.Point(428, 27)
        Me.dtDateTo.Name = "dtDateTo"
        Me.dtDateTo.Size = New System.Drawing.Size(165, 20)
        Me.dtDateTo.TabIndex = 9
        '
        'TableLayoutPanel1
        '
        Me.TableLayoutPanel1.ColumnCount = 5
        Me.tpnlHeader.SetColumnSpan(Me.TableLayoutPanel1, 5)
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100.0!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100.0!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100.0!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100.0!))
        Me.TableLayoutPanel1.Controls.Add(Me.btnExportCSV, 4, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.btnClear, 3, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.chkAllItems, 0, 0)
        Me.TableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TableLayoutPanel1.Location = New System.Drawing.Point(108, 71)
        Me.TableLayoutPanel1.Name = "TableLayoutPanel1"
        Me.TableLayoutPanel1.RowCount = 1
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel1.Size = New System.Drawing.Size(485, 35)
        Me.TableLayoutPanel1.TabIndex = 12
        '
        'btnExportCSV
        '
        Me.btnExportCSV.Dock = System.Windows.Forms.DockStyle.Fill
        Me.btnExportCSV.Location = New System.Drawing.Point(388, 3)
        Me.btnExportCSV.Name = "btnExportCSV"
        Me.btnExportCSV.Size = New System.Drawing.Size(94, 29)
        Me.btnExportCSV.TabIndex = 10
        Me.btnExportCSV.Text = "Export CSV"
        Me.btnExportCSV.UseVisualStyleBackColor = True
        '
        'btnClear
        '
        Me.btnClear.Dock = System.Windows.Forms.DockStyle.Fill
        Me.btnClear.Location = New System.Drawing.Point(288, 3)
        Me.btnClear.Name = "btnClear"
        Me.btnClear.Size = New System.Drawing.Size(94, 29)
        Me.btnClear.TabIndex = 11
        Me.btnClear.Text = "Clear"
        Me.btnClear.UseVisualStyleBackColor = True
        '
        'chkAllItems
        '
        Me.chkAllItems.AutoSize = True
        Me.chkAllItems.Dock = System.Windows.Forms.DockStyle.Fill
        Me.chkAllItems.Location = New System.Drawing.Point(3, 3)
        Me.chkAllItems.Name = "chkAllItems"
        Me.chkAllItems.Size = New System.Drawing.Size(94, 29)
        Me.chkAllItems.TabIndex = 11
        Me.chkAllItems.Text = "All Items"
        Me.chkAllItems.UseVisualStyleBackColor = True
        '
        'gboxBottom
        '
        Me.tpnlCenter.SetColumnSpan(Me.gboxBottom, 2)
        Me.gboxBottom.Controls.Add(Me.txtMessage)
        Me.gboxBottom.Dock = System.Windows.Forms.DockStyle.Fill
        Me.gboxBottom.Location = New System.Drawing.Point(23, 157)
        Me.gboxBottom.Name = "gboxBottom"
        Me.gboxBottom.Size = New System.Drawing.Size(602, 267)
        Me.gboxBottom.TabIndex = 1
        Me.gboxBottom.TabStop = False
        Me.gboxBottom.Text = "Message"
        '
        'txtMessage
        '
        Me.txtMessage.Dock = System.Windows.Forms.DockStyle.Fill
        Me.txtMessage.Location = New System.Drawing.Point(3, 16)
        Me.txtMessage.Multiline = True
        Me.txtMessage.Name = "txtMessage"
        Me.txtMessage.Size = New System.Drawing.Size(596, 248)
        Me.txtMessage.TabIndex = 0
        '
        'tpnlCenter
        '
        Me.tpnlCenter.ColumnCount = 4
        Me.tpnlCenter.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.tpnlCenter.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.tpnlCenter.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.tpnlCenter.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 21.0!))
        Me.tpnlCenter.Controls.Add(Me.gboxHeader, 1, 0)
        Me.tpnlCenter.Controls.Add(Me.gboxBottom, 1, 2)
        Me.tpnlCenter.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tpnlCenter.Location = New System.Drawing.Point(0, 0)
        Me.tpnlCenter.Name = "tpnlCenter"
        Me.tpnlCenter.RowCount = 4
        Me.tpnlCenter.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.0!))
        Me.tpnlCenter.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.tpnlCenter.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 67.0!))
        Me.tpnlCenter.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.tpnlCenter.Size = New System.Drawing.Size(649, 448)
        Me.tpnlCenter.TabIndex = 0
        '
        'pnlCenter
        '
        Me.pnlCenter.Controls.Add(Me.tpnlCenter)
        Me.pnlCenter.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pnlCenter.Location = New System.Drawing.Point(10, 10)
        Me.pnlCenter.Name = "pnlCenter"
        Me.pnlCenter.Size = New System.Drawing.Size(649, 448)
        Me.pnlCenter.TabIndex = 9
        '
        'pnlLeft
        '
        Me.pnlLeft.Dock = System.Windows.Forms.DockStyle.Left
        Me.pnlLeft.Location = New System.Drawing.Point(0, 10)
        Me.pnlLeft.Name = "pnlLeft"
        Me.pnlLeft.Size = New System.Drawing.Size(10, 448)
        Me.pnlLeft.TabIndex = 5
        '
        'pnlRight
        '
        Me.pnlRight.Dock = System.Windows.Forms.DockStyle.Right
        Me.pnlRight.Location = New System.Drawing.Point(659, 10)
        Me.pnlRight.Name = "pnlRight"
        Me.pnlRight.Size = New System.Drawing.Size(10, 448)
        Me.pnlRight.TabIndex = 6
        '
        'pnlTop
        '
        Me.pnlTop.Dock = System.Windows.Forms.DockStyle.Top
        Me.pnlTop.Location = New System.Drawing.Point(0, 0)
        Me.pnlTop.Name = "pnlTop"
        Me.pnlTop.Size = New System.Drawing.Size(669, 10)
        Me.pnlTop.TabIndex = 8
        '
        'pnlBottom
        '
        Me.pnlBottom.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.pnlBottom.Location = New System.Drawing.Point(0, 458)
        Me.pnlBottom.Name = "pnlBottom"
        Me.pnlBottom.Size = New System.Drawing.Size(669, 10)
        Me.pnlBottom.TabIndex = 7
        '
        'frmItemMaster
        '
        Me.AcceptButton = Me.btnExportCSV
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(669, 468)
        Me.Controls.Add(Me.pnlCenter)
        Me.Controls.Add(Me.pnlLeft)
        Me.Controls.Add(Me.pnlRight)
        Me.Controls.Add(Me.pnlTop)
        Me.Controls.Add(Me.pnlBottom)
        Me.MaximizeBox = False
        Me.Name = "frmItemMaster"
        Me.ShowIcon = False
        Me.Text = "Item Master Data"
        Me.gboxHeader.ResumeLayout(False)
        Me.tpnlHeader.ResumeLayout(False)
        Me.tpnlHeader.PerformLayout()
        Me.TableLayoutPanel1.ResumeLayout(False)
        Me.TableLayoutPanel1.PerformLayout()
        Me.gboxBottom.ResumeLayout(False)
        Me.gboxBottom.PerformLayout()
        Me.tpnlCenter.ResumeLayout(False)
        Me.pnlCenter.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents gboxHeader As System.Windows.Forms.GroupBox
    Friend WithEvents tpnlCenter As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents gboxBottom As System.Windows.Forms.GroupBox
    Friend WithEvents txtMessage As System.Windows.Forms.TextBox
    Friend WithEvents tpnlHeader As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents lblItemCodeFrom As System.Windows.Forms.Label
    Friend WithEvents lblItemCodeTo As System.Windows.Forms.Label
    Friend WithEvents txtItemCodeFrom As System.Windows.Forms.TextBox
    Friend WithEvents txtItemCodeTo As System.Windows.Forms.TextBox
    Friend WithEvents btnChooseFrom As System.Windows.Forms.Button
    Friend WithEvents btnChooseTo As System.Windows.Forms.Button
    Friend WithEvents lblDateFrom As System.Windows.Forms.Label
    Friend WithEvents lblDateTo As System.Windows.Forms.Label
    Friend WithEvents dtDateFrom As System.Windows.Forms.DateTimePicker
    Friend WithEvents dtDateTo As System.Windows.Forms.DateTimePicker
    Friend WithEvents btnExportCSV As System.Windows.Forms.Button
    Friend WithEvents pnlCenter As System.Windows.Forms.Panel
    Friend WithEvents pnlLeft As System.Windows.Forms.Panel
    Friend WithEvents pnlRight As System.Windows.Forms.Panel
    Friend WithEvents pnlTop As System.Windows.Forms.Panel
    Friend WithEvents pnlBottom As System.Windows.Forms.Panel
    Friend WithEvents chkAllItems As System.Windows.Forms.CheckBox
    Friend WithEvents TableLayoutPanel1 As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents btnClear As System.Windows.Forms.Button
End Class
