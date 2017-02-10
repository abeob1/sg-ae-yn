<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmItemMasterData
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
        Me.pnlLeft = New System.Windows.Forms.Panel()
        Me.pnlRight = New System.Windows.Forms.Panel()
        Me.pnlBottom = New System.Windows.Forms.Panel()
        Me.pnlTop = New System.Windows.Forms.Panel()
        Me.pnlCenter = New System.Windows.Forms.Panel()
        Me.tpnlCenter = New System.Windows.Forms.TableLayoutPanel()
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
        Me.btnExportCSV = New System.Windows.Forms.Button()
        Me.gboxBottom = New System.Windows.Forms.GroupBox()
        Me.txtMessage = New System.Windows.Forms.TextBox()
        Me.pnlCenter.SuspendLayout()
        Me.tpnlCenter.SuspendLayout()
        Me.gboxHeader.SuspendLayout()
        Me.tpnlHeader.SuspendLayout()
        Me.gboxBottom.SuspendLayout()
        Me.SuspendLayout()
        '
        'pnlLeft
        '
        Me.pnlLeft.Dock = System.Windows.Forms.DockStyle.Left
        Me.pnlLeft.Location = New System.Drawing.Point(0, 0)
        Me.pnlLeft.Name = "pnlLeft"
        Me.pnlLeft.Size = New System.Drawing.Size(10, 456)
        Me.pnlLeft.TabIndex = 0
        '
        'pnlRight
        '
        Me.pnlRight.Dock = System.Windows.Forms.DockStyle.Right
        Me.pnlRight.Location = New System.Drawing.Point(641, 0)
        Me.pnlRight.Name = "pnlRight"
        Me.pnlRight.Size = New System.Drawing.Size(10, 456)
        Me.pnlRight.TabIndex = 1
        '
        'pnlBottom
        '
        Me.pnlBottom.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.pnlBottom.Location = New System.Drawing.Point(10, 446)
        Me.pnlBottom.Name = "pnlBottom"
        Me.pnlBottom.Size = New System.Drawing.Size(631, 10)
        Me.pnlBottom.TabIndex = 2
        '
        'pnlTop
        '
        Me.pnlTop.Dock = System.Windows.Forms.DockStyle.Top
        Me.pnlTop.Location = New System.Drawing.Point(10, 0)
        Me.pnlTop.Name = "pnlTop"
        Me.pnlTop.Size = New System.Drawing.Size(631, 10)
        Me.pnlTop.TabIndex = 3
        '
        'pnlCenter
        '
        Me.pnlCenter.Controls.Add(Me.tpnlCenter)
        Me.pnlCenter.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pnlCenter.Location = New System.Drawing.Point(10, 10)
        Me.pnlCenter.Name = "pnlCenter"
        Me.pnlCenter.Size = New System.Drawing.Size(631, 436)
        Me.pnlCenter.TabIndex = 4
        '
        'tpnlCenter
        '
        Me.tpnlCenter.ColumnCount = 4
        Me.tpnlCenter.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.tpnlCenter.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.tpnlCenter.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.tpnlCenter.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
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
        Me.tpnlCenter.Size = New System.Drawing.Size(631, 436)
        Me.tpnlCenter.TabIndex = 0
        '
        'gboxHeader
        '
        Me.tpnlCenter.SetColumnSpan(Me.gboxHeader, 2)
        Me.gboxHeader.Controls.Add(Me.tpnlHeader)
        Me.gboxHeader.Dock = System.Windows.Forms.DockStyle.Fill
        Me.gboxHeader.Location = New System.Drawing.Point(23, 3)
        Me.gboxHeader.Name = "gboxHeader"
        Me.gboxHeader.Size = New System.Drawing.Size(584, 124)
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
        Me.tpnlHeader.Controls.Add(Me.btnExportCSV, 5, 3)
        Me.tpnlHeader.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tpnlHeader.Location = New System.Drawing.Point(3, 16)
        Me.tpnlHeader.Name = "tpnlHeader"
        Me.tpnlHeader.RowCount = 4
        Me.tpnlHeader.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.tpnlHeader.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.tpnlHeader.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.tpnlHeader.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35.0!))
        Me.tpnlHeader.Size = New System.Drawing.Size(578, 105)
        Me.tpnlHeader.TabIndex = 0
        '
        'lblItemCodeFrom
        '
        Me.lblItemCodeFrom.AutoSize = True
        Me.lblItemCodeFrom.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lblItemCodeFrom.Location = New System.Drawing.Point(3, 0)
        Me.lblItemCodeFrom.Name = "lblItemCodeFrom"
        Me.lblItemCodeFrom.Size = New System.Drawing.Size(99, 25)
        Me.lblItemCodeFrom.TabIndex = 0
        Me.lblItemCodeFrom.Text = "Item Code From      "
        Me.lblItemCodeFrom.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lblItemCodeTo
        '
        Me.lblItemCodeTo.AutoSize = True
        Me.lblItemCodeTo.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lblItemCodeTo.Location = New System.Drawing.Point(3, 25)
        Me.lblItemCodeTo.Name = "lblItemCodeTo"
        Me.lblItemCodeTo.Size = New System.Drawing.Size(99, 25)
        Me.lblItemCodeTo.TabIndex = 1
        Me.lblItemCodeTo.Text = "Item Code To"
        Me.lblItemCodeTo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'txtItemCodeFrom
        '
        Me.txtItemCodeFrom.Dock = System.Windows.Forms.DockStyle.Fill
        Me.txtItemCodeFrom.Location = New System.Drawing.Point(108, 3)
        Me.txtItemCodeFrom.Name = "txtItemCodeFrom"
        Me.txtItemCodeFrom.Size = New System.Drawing.Size(155, 20)
        Me.txtItemCodeFrom.TabIndex = 2
        '
        'txtItemCodeTo
        '
        Me.txtItemCodeTo.Dock = System.Windows.Forms.DockStyle.Fill
        Me.txtItemCodeTo.Location = New System.Drawing.Point(108, 28)
        Me.txtItemCodeTo.Name = "txtItemCodeTo"
        Me.txtItemCodeTo.Size = New System.Drawing.Size(155, 20)
        Me.txtItemCodeTo.TabIndex = 3
        '
        'btnChooseFrom
        '
        Me.btnChooseFrom.Dock = System.Windows.Forms.DockStyle.Fill
        Me.btnChooseFrom.Location = New System.Drawing.Point(269, 3)
        Me.btnChooseFrom.Name = "btnChooseFrom"
        Me.btnChooseFrom.Size = New System.Drawing.Size(29, 19)
        Me.btnChooseFrom.TabIndex = 4
        Me.btnChooseFrom.Text = "..."
        Me.btnChooseFrom.UseVisualStyleBackColor = True
        '
        'btnChooseTo
        '
        Me.btnChooseTo.Dock = System.Windows.Forms.DockStyle.Fill
        Me.btnChooseTo.Location = New System.Drawing.Point(269, 28)
        Me.btnChooseTo.Name = "btnChooseTo"
        Me.btnChooseTo.Size = New System.Drawing.Size(29, 19)
        Me.btnChooseTo.TabIndex = 5
        Me.btnChooseTo.Text = "..."
        Me.btnChooseTo.UseVisualStyleBackColor = True
        '
        'lblDateFrom
        '
        Me.lblDateFrom.AutoSize = True
        Me.lblDateFrom.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lblDateFrom.Location = New System.Drawing.Point(324, 0)
        Me.lblDateFrom.Name = "lblDateFrom"
        Me.lblDateFrom.Size = New System.Drawing.Size(89, 25)
        Me.lblDateFrom.TabIndex = 6
        Me.lblDateFrom.Text = "Date From           "
        Me.lblDateFrom.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lblDateTo
        '
        Me.lblDateTo.AutoSize = True
        Me.lblDateTo.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lblDateTo.Location = New System.Drawing.Point(324, 25)
        Me.lblDateTo.Name = "lblDateTo"
        Me.lblDateTo.Size = New System.Drawing.Size(89, 25)
        Me.lblDateTo.TabIndex = 7
        Me.lblDateTo.Text = "Date To"
        Me.lblDateTo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'dtDateFrom
        '
        Me.dtDateFrom.CustomFormat = "dd/MM/yyyy"
        Me.dtDateFrom.Dock = System.Windows.Forms.DockStyle.Fill
        Me.dtDateFrom.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.dtDateFrom.Location = New System.Drawing.Point(419, 3)
        Me.dtDateFrom.Name = "dtDateFrom"
        Me.dtDateFrom.Size = New System.Drawing.Size(156, 20)
        Me.dtDateFrom.TabIndex = 8
        '
        'dtDateTo
        '
        Me.dtDateTo.CustomFormat = "dd/MM/yyyy"
        Me.dtDateTo.Dock = System.Windows.Forms.DockStyle.Fill
        Me.dtDateTo.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.dtDateTo.Location = New System.Drawing.Point(419, 28)
        Me.dtDateTo.Name = "dtDateTo"
        Me.dtDateTo.Size = New System.Drawing.Size(156, 20)
        Me.dtDateTo.TabIndex = 9
        '
        'btnExportCSV
        '
        Me.btnExportCSV.Dock = System.Windows.Forms.DockStyle.Fill
        Me.btnExportCSV.Location = New System.Drawing.Point(419, 73)
        Me.btnExportCSV.Name = "btnExportCSV"
        Me.btnExportCSV.Size = New System.Drawing.Size(156, 29)
        Me.btnExportCSV.TabIndex = 10
        Me.btnExportCSV.Text = "Export CSV"
        Me.btnExportCSV.UseVisualStyleBackColor = True
        '
        'gboxBottom
        '
        Me.tpnlCenter.SetColumnSpan(Me.gboxBottom, 2)
        Me.gboxBottom.Controls.Add(Me.txtMessage)
        Me.gboxBottom.Dock = System.Windows.Forms.DockStyle.Fill
        Me.gboxBottom.Location = New System.Drawing.Point(23, 153)
        Me.gboxBottom.Name = "gboxBottom"
        Me.gboxBottom.Size = New System.Drawing.Size(584, 259)
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
        Me.txtMessage.Size = New System.Drawing.Size(578, 240)
        Me.txtMessage.TabIndex = 0
        '
        'frmItemMasterData
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(651, 456)
        Me.Controls.Add(Me.pnlCenter)
        Me.Controls.Add(Me.pnlTop)
        Me.Controls.Add(Me.pnlBottom)
        Me.Controls.Add(Me.pnlRight)
        Me.Controls.Add(Me.pnlLeft)
        Me.MaximizeBox = False
        Me.Name = "frmItemMasterData"
        Me.ShowIcon = False
        Me.Text = "Item Master Data"
        Me.pnlCenter.ResumeLayout(False)
        Me.tpnlCenter.ResumeLayout(False)
        Me.gboxHeader.ResumeLayout(False)
        Me.tpnlHeader.ResumeLayout(False)
        Me.tpnlHeader.PerformLayout()
        Me.gboxBottom.ResumeLayout(False)
        Me.gboxBottom.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents pnlLeft As System.Windows.Forms.Panel
    Friend WithEvents pnlRight As System.Windows.Forms.Panel
    Friend WithEvents pnlBottom As System.Windows.Forms.Panel
    Friend WithEvents pnlTop As System.Windows.Forms.Panel
    Friend WithEvents pnlCenter As System.Windows.Forms.Panel
    Friend WithEvents tpnlCenter As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents gboxHeader As System.Windows.Forms.GroupBox
    Friend WithEvents gboxBottom As System.Windows.Forms.GroupBox
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
    Friend WithEvents txtMessage As System.Windows.Forms.TextBox

End Class
