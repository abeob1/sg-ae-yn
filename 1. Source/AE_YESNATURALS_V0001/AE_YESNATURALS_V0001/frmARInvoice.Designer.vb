<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmARInvoice
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
        Me.pnlTop = New System.Windows.Forms.Panel()
        Me.pnlBottom = New System.Windows.Forms.Panel()
        Me.pnlFill = New System.Windows.Forms.Panel()
        Me.tblFill = New System.Windows.Forms.TableLayoutPanel()
        Me.grpboxMessage = New System.Windows.Forms.GroupBox()
        Me.txtMessage = New System.Windows.Forms.TextBox()
        Me.grpboxButtom = New System.Windows.Forms.GroupBox()
        Me.tblBottom = New System.Windows.Forms.TableLayoutPanel()
        Me.btnCancel = New System.Windows.Forms.Button()
        Me.btnCreate = New System.Windows.Forms.Button()
        Me.pnlFill.SuspendLayout()
        Me.tblFill.SuspendLayout()
        Me.grpboxMessage.SuspendLayout()
        Me.grpboxButtom.SuspendLayout()
        Me.tblBottom.SuspendLayout()
        Me.SuspendLayout()
        '
        'pnlLeft
        '
        Me.pnlLeft.Dock = System.Windows.Forms.DockStyle.Left
        Me.pnlLeft.Location = New System.Drawing.Point(0, 0)
        Me.pnlLeft.Name = "pnlLeft"
        Me.pnlLeft.Size = New System.Drawing.Size(10, 409)
        Me.pnlLeft.TabIndex = 2
        '
        'pnlRight
        '
        Me.pnlRight.Dock = System.Windows.Forms.DockStyle.Right
        Me.pnlRight.Location = New System.Drawing.Point(496, 0)
        Me.pnlRight.Name = "pnlRight"
        Me.pnlRight.Size = New System.Drawing.Size(10, 409)
        Me.pnlRight.TabIndex = 3
        '
        'pnlTop
        '
        Me.pnlTop.Dock = System.Windows.Forms.DockStyle.Top
        Me.pnlTop.Location = New System.Drawing.Point(10, 0)
        Me.pnlTop.Name = "pnlTop"
        Me.pnlTop.Size = New System.Drawing.Size(486, 10)
        Me.pnlTop.TabIndex = 4
        '
        'pnlBottom
        '
        Me.pnlBottom.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.pnlBottom.Location = New System.Drawing.Point(10, 399)
        Me.pnlBottom.Name = "pnlBottom"
        Me.pnlBottom.Size = New System.Drawing.Size(486, 10)
        Me.pnlBottom.TabIndex = 5
        '
        'pnlFill
        '
        Me.pnlFill.Controls.Add(Me.tblFill)
        Me.pnlFill.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pnlFill.Location = New System.Drawing.Point(10, 10)
        Me.pnlFill.Name = "pnlFill"
        Me.pnlFill.Size = New System.Drawing.Size(486, 389)
        Me.pnlFill.TabIndex = 6
        '
        'tblFill
        '
        Me.tblFill.ColumnCount = 2
        Me.tblFill.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.tblFill.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.tblFill.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.tblFill.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.tblFill.Controls.Add(Me.grpboxMessage, 0, 0)
        Me.tblFill.Controls.Add(Me.grpboxButtom, 0, 1)
        Me.tblFill.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tblFill.Location = New System.Drawing.Point(0, 0)
        Me.tblFill.Name = "tblFill"
        Me.tblFill.RowCount = 2
        Me.tblFill.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.tblFill.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 60.0!))
        Me.tblFill.Size = New System.Drawing.Size(486, 389)
        Me.tblFill.TabIndex = 0
        '
        'grpboxMessage
        '
        Me.tblFill.SetColumnSpan(Me.grpboxMessage, 2)
        Me.grpboxMessage.Controls.Add(Me.txtMessage)
        Me.grpboxMessage.Dock = System.Windows.Forms.DockStyle.Fill
        Me.grpboxMessage.Location = New System.Drawing.Point(3, 3)
        Me.grpboxMessage.Name = "grpboxMessage"
        Me.grpboxMessage.Size = New System.Drawing.Size(480, 323)
        Me.grpboxMessage.TabIndex = 0
        Me.grpboxMessage.TabStop = False
        Me.grpboxMessage.Text = "Message"
        '
        'txtMessage
        '
        Me.txtMessage.Dock = System.Windows.Forms.DockStyle.Fill
        Me.txtMessage.Enabled = False
        Me.txtMessage.Location = New System.Drawing.Point(3, 16)
        Me.txtMessage.Multiline = True
        Me.txtMessage.Name = "txtMessage"
        Me.txtMessage.Size = New System.Drawing.Size(474, 304)
        Me.txtMessage.TabIndex = 0
        '
        'grpboxButtom
        '
        Me.tblFill.SetColumnSpan(Me.grpboxButtom, 2)
        Me.grpboxButtom.Controls.Add(Me.tblBottom)
        Me.grpboxButtom.Dock = System.Windows.Forms.DockStyle.Fill
        Me.grpboxButtom.Location = New System.Drawing.Point(3, 332)
        Me.grpboxButtom.Name = "grpboxButtom"
        Me.grpboxButtom.Size = New System.Drawing.Size(480, 54)
        Me.grpboxButtom.TabIndex = 1
        Me.grpboxButtom.TabStop = False
        '
        'tblBottom
        '
        Me.tblBottom.ColumnCount = 3
        Me.tblBottom.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 200.0!))
        Me.tblBottom.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100.0!))
        Me.tblBottom.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.tblBottom.Controls.Add(Me.btnCancel, 1, 0)
        Me.tblBottom.Controls.Add(Me.btnCreate, 0, 0)
        Me.tblBottom.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tblBottom.Location = New System.Drawing.Point(3, 16)
        Me.tblBottom.Name = "tblBottom"
        Me.tblBottom.RowCount = 1
        Me.tblBottom.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.tblBottom.Size = New System.Drawing.Size(474, 35)
        Me.tblBottom.TabIndex = 0
        '
        'btnCancel
        '
        Me.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnCancel.Dock = System.Windows.Forms.DockStyle.Fill
        Me.btnCancel.Location = New System.Drawing.Point(203, 3)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(94, 29)
        Me.btnCancel.TabIndex = 1
        Me.btnCancel.Text = "Cancel"
        Me.btnCancel.UseVisualStyleBackColor = True
        '
        'btnCreate
        '
        Me.btnCreate.Dock = System.Windows.Forms.DockStyle.Fill
        Me.btnCreate.Location = New System.Drawing.Point(3, 3)
        Me.btnCreate.Name = "btnCreate"
        Me.btnCreate.Size = New System.Drawing.Size(194, 29)
        Me.btnCreate.TabIndex = 0
        Me.btnCreate.Text = "Create AR Invoice and Payment"
        Me.btnCreate.UseVisualStyleBackColor = True
        '
        'frmARInvoice
        '
        Me.AcceptButton = Me.btnCreate
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.btnCancel
        Me.ClientSize = New System.Drawing.Size(506, 409)
        Me.Controls.Add(Me.pnlFill)
        Me.Controls.Add(Me.pnlBottom)
        Me.Controls.Add(Me.pnlTop)
        Me.Controls.Add(Me.pnlRight)
        Me.Controls.Add(Me.pnlLeft)
        Me.MaximizeBox = False
        Me.Name = "frmARInvoice"
        Me.ShowIcon = False
        Me.Text = "AR Invoice"
        Me.pnlFill.ResumeLayout(False)
        Me.tblFill.ResumeLayout(False)
        Me.grpboxMessage.ResumeLayout(False)
        Me.grpboxMessage.PerformLayout()
        Me.grpboxButtom.ResumeLayout(False)
        Me.tblBottom.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents pnlLeft As System.Windows.Forms.Panel
    Friend WithEvents pnlRight As System.Windows.Forms.Panel
    Friend WithEvents pnlTop As System.Windows.Forms.Panel
    Friend WithEvents pnlBottom As System.Windows.Forms.Panel
    Friend WithEvents pnlFill As System.Windows.Forms.Panel
    Friend WithEvents tblFill As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents grpboxMessage As System.Windows.Forms.GroupBox
    Friend WithEvents txtMessage As System.Windows.Forms.TextBox
    Friend WithEvents grpboxButtom As System.Windows.Forms.GroupBox
    Friend WithEvents tblBottom As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents btnCreate As System.Windows.Forms.Button
    Friend WithEvents btnCancel As System.Windows.Forms.Button
End Class
