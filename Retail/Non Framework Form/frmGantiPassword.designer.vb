<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FrmGantiPassword
    Inherits DevExpress.XtraEditors.XtraForm

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing AndAlso components IsNot Nothing Then
            components.Dispose()
        End If
        MyBase.Dispose(disposing)
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.GroupControl1 = New DevExpress.XtraEditors.GroupControl
        Me.txtUserName = New DevExpress.XtraEditors.TextEdit
        Me.LabelControl4 = New DevExpress.XtraEditors.LabelControl
        Me.LabelControl3 = New DevExpress.XtraEditors.LabelControl
        Me.LabelControl2 = New DevExpress.XtraEditors.LabelControl
        Me.LabelControl1 = New DevExpress.XtraEditors.LabelControl
        Me.txtConfr = New DevExpress.XtraEditors.TextEdit
        Me.txtBaru = New DevExpress.XtraEditors.TextEdit
        Me.txtLama = New DevExpress.XtraEditors.TextEdit
        Me.btnSave = New DevExpress.XtraEditors.SimpleButton
        Me.BtnCansel = New DevExpress.XtraEditors.SimpleButton
        CType(Me.GroupControl1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GroupControl1.SuspendLayout()
        CType(Me.txtUserName.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtConfr.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtBaru.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtLama.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'GroupControl1
        '
        Me.GroupControl1.Controls.Add(Me.txtUserName)
        Me.GroupControl1.Controls.Add(Me.LabelControl4)
        Me.GroupControl1.Controls.Add(Me.LabelControl3)
        Me.GroupControl1.Controls.Add(Me.LabelControl2)
        Me.GroupControl1.Controls.Add(Me.LabelControl1)
        Me.GroupControl1.Controls.Add(Me.txtConfr)
        Me.GroupControl1.Controls.Add(Me.txtBaru)
        Me.GroupControl1.Controls.Add(Me.txtLama)
        Me.GroupControl1.Location = New System.Drawing.Point(7, 8)
        Me.GroupControl1.Name = "GroupControl1"
        Me.GroupControl1.Size = New System.Drawing.Size(307, 132)
        Me.GroupControl1.TabIndex = 0
        Me.GroupControl1.Text = "Ganti Password"
        '
        'txtUserName
        '
        Me.txtUserName.EnterMoveNextControl = True
        Me.txtUserName.Location = New System.Drawing.Point(121, 25)
        Me.txtUserName.Name = "txtUserName"
        Me.txtUserName.Properties.Appearance.BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.txtUserName.Properties.Appearance.Options.UseBackColor = True
        Me.txtUserName.Properties.ReadOnly = True
        Me.txtUserName.Size = New System.Drawing.Size(170, 20)
        Me.txtUserName.TabIndex = 1
        '
        'LabelControl4
        '
        Me.LabelControl4.Location = New System.Drawing.Point(21, 26)
        Me.LabelControl4.Name = "LabelControl4"
        Me.LabelControl4.Size = New System.Drawing.Size(52, 13)
        Me.LabelControl4.TabIndex = 0
        Me.LabelControl4.Text = "Nama User"
        '
        'LabelControl3
        '
        Me.LabelControl3.Location = New System.Drawing.Point(21, 105)
        Me.LabelControl3.Name = "LabelControl3"
        Me.LabelControl3.Size = New System.Drawing.Size(86, 13)
        Me.LabelControl3.TabIndex = 6
        Me.LabelControl3.Text = "Confirm Password"
        '
        'LabelControl2
        '
        Me.LabelControl2.Location = New System.Drawing.Point(21, 81)
        Me.LabelControl2.Name = "LabelControl2"
        Me.LabelControl2.Size = New System.Drawing.Size(71, 13)
        Me.LabelControl2.TabIndex = 4
        Me.LabelControl2.Text = "Password Baru"
        '
        'LabelControl1
        '
        Me.LabelControl1.Location = New System.Drawing.Point(21, 54)
        Me.LabelControl1.Name = "LabelControl1"
        Me.LabelControl1.Size = New System.Drawing.Size(74, 13)
        Me.LabelControl1.TabIndex = 2
        Me.LabelControl1.Text = "Password Lama"
        '
        'txtConfr
        '
        Me.txtConfr.EnterMoveNextControl = True
        Me.txtConfr.Location = New System.Drawing.Point(121, 103)
        Me.txtConfr.Name = "txtConfr"
        Me.txtConfr.Properties.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtConfr.Properties.PasswordChar = Global.Microsoft.VisualBasic.ChrW(42)
        Me.txtConfr.Size = New System.Drawing.Size(170, 20)
        Me.txtConfr.TabIndex = 7
        '
        'txtBaru
        '
        Me.txtBaru.EnterMoveNextControl = True
        Me.txtBaru.Location = New System.Drawing.Point(121, 77)
        Me.txtBaru.Name = "txtBaru"
        Me.txtBaru.Properties.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtBaru.Properties.PasswordChar = Global.Microsoft.VisualBasic.ChrW(42)
        Me.txtBaru.Size = New System.Drawing.Size(170, 20)
        Me.txtBaru.TabIndex = 5
        '
        'txtLama
        '
        Me.txtLama.EnterMoveNextControl = True
        Me.txtLama.Location = New System.Drawing.Point(121, 51)
        Me.txtLama.Name = "txtLama"
        Me.txtLama.Properties.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtLama.Properties.PasswordChar = Global.Microsoft.VisualBasic.ChrW(42)
        Me.txtLama.Size = New System.Drawing.Size(170, 20)
        Me.txtLama.TabIndex = 3
        '
        'btnSave
        '
        Me.btnSave.Location = New System.Drawing.Point(145, 146)
        Me.btnSave.Name = "btnSave"
        Me.btnSave.Size = New System.Drawing.Size(79, 26)
        Me.btnSave.TabIndex = 1
        Me.btnSave.Text = "&Simpan"
        '
        'BtnCansel
        '
        Me.BtnCansel.Location = New System.Drawing.Point(230, 146)
        Me.BtnCansel.Name = "BtnCansel"
        Me.BtnCansel.Size = New System.Drawing.Size(79, 26)
        Me.BtnCansel.TabIndex = 2
        Me.BtnCansel.Text = "&Tutup"
        '
        'FrmGantiPassword
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(321, 180)
        Me.Controls.Add(Me.BtnCansel)
        Me.Controls.Add(Me.btnSave)
        Me.Controls.Add(Me.GroupControl1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.Name = "FrmGantiPassword"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Change Password"
        CType(Me.GroupControl1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GroupControl1.ResumeLayout(False)
        Me.GroupControl1.PerformLayout()
        CType(Me.txtUserName.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtConfr.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtBaru.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtLama.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents GroupControl1 As DevExpress.XtraEditors.GroupControl
    Friend WithEvents txtConfr As DevExpress.XtraEditors.TextEdit
    Friend WithEvents txtBaru As DevExpress.XtraEditors.TextEdit
    Friend WithEvents txtLama As DevExpress.XtraEditors.TextEdit
    Friend WithEvents btnSave As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents BtnCansel As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents LabelControl3 As DevExpress.XtraEditors.LabelControl
    Friend WithEvents LabelControl2 As DevExpress.XtraEditors.LabelControl
    Friend WithEvents LabelControl1 As DevExpress.XtraEditors.LabelControl
    Friend WithEvents txtUserName As DevExpress.XtraEditors.TextEdit
    Friend WithEvents LabelControl4 As DevExpress.XtraEditors.LabelControl
End Class
