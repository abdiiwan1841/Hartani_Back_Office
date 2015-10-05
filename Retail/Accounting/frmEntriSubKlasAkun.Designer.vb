<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmEntriSubKlasAkun
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
        Dim SerializableAppearanceObject1 As DevExpress.Utils.SerializableAppearanceObject = New DevExpress.Utils.SerializableAppearanceObject
        Dim SerializableAppearanceObject2 As DevExpress.Utils.SerializableAppearanceObject = New DevExpress.Utils.SerializableAppearanceObject
        Me.txtKlas = New DevExpress.XtraEditors.ButtonEdit
        Me.LabelControl2 = New DevExpress.XtraEditors.LabelControl
        Me.LabelControl8 = New DevExpress.XtraEditors.LabelControl
        Me.LabelControl4 = New DevExpress.XtraEditors.LabelControl
        Me.LabelControl10 = New DevExpress.XtraEditors.LabelControl
        Me.cmdSave = New DevExpress.XtraEditors.SimpleButton
        Me.cmdClose = New DevExpress.XtraEditors.SimpleButton
        Me.txtKode = New DevExpress.XtraEditors.TextEdit
        Me.txtNama = New DevExpress.XtraEditors.TextEdit
        Me.txtAlias = New DevExpress.XtraEditors.TextEdit
        Me.ckAktif = New DevExpress.XtraEditors.CheckEdit
        CType(Me.txtKlas.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtKode.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtNama.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtAlias.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.ckAktif.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'txtKlas
        '
        Me.txtKlas.EnterMoveNextControl = True
        Me.txtKlas.Location = New System.Drawing.Point(128, 12)
        Me.txtKlas.Name = "txtKlas"
        Me.txtKlas.Properties.Appearance.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtKlas.Properties.Appearance.Options.UseFont = True
        Me.txtKlas.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Ellipsis, "", -1, True, True, False, DevExpress.XtraEditors.ImageLocation.MiddleCenter, Nothing, New DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.PageDown), SerializableAppearanceObject1, "", Nothing, Nothing, True), New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Close, "", -1, True, True, False, DevExpress.XtraEditors.ImageLocation.MiddleCenter, Nothing, New DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.Delete), SerializableAppearanceObject2, "", Nothing, Nothing, True)})
        Me.txtKlas.Properties.ReadOnly = True
        Me.txtKlas.Size = New System.Drawing.Size(207, 22)
        Me.txtKlas.TabIndex = 1
        '
        'LabelControl2
        '
        Me.LabelControl2.Appearance.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LabelControl2.Location = New System.Drawing.Point(7, 15)
        Me.LabelControl2.Name = "LabelControl2"
        Me.LabelControl2.Size = New System.Drawing.Size(59, 15)
        Me.LabelControl2.TabIndex = 0
        Me.LabelControl2.Text = "Klasifikasi"
        '
        'LabelControl8
        '
        Me.LabelControl8.Appearance.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LabelControl8.Location = New System.Drawing.Point(6, 43)
        Me.LabelControl8.Name = "LabelControl8"
        Me.LabelControl8.Size = New System.Drawing.Size(27, 15)
        Me.LabelControl8.TabIndex = 2
        Me.LabelControl8.Text = "Kode"
        '
        'LabelControl4
        '
        Me.LabelControl4.Appearance.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LabelControl4.Location = New System.Drawing.Point(6, 71)
        Me.LabelControl4.Name = "LabelControl4"
        Me.LabelControl4.Size = New System.Drawing.Size(32, 15)
        Me.LabelControl4.TabIndex = 4
        Me.LabelControl4.Text = "Nama"
        '
        'LabelControl10
        '
        Me.LabelControl10.Appearance.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LabelControl10.Location = New System.Drawing.Point(6, 99)
        Me.LabelControl10.Name = "LabelControl10"
        Me.LabelControl10.Size = New System.Drawing.Size(28, 15)
        Me.LabelControl10.TabIndex = 6
        Me.LabelControl10.Text = "Alias"
        '
        'cmdSave
        '
        Me.cmdSave.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdSave.Appearance.BackColor = System.Drawing.Color.Snow
        Me.cmdSave.Appearance.BackColor2 = System.Drawing.Color.PowderBlue
        Me.cmdSave.Appearance.BorderColor = System.Drawing.SystemColors.ButtonFace
        Me.cmdSave.Appearance.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdSave.Appearance.ForeColor = System.Drawing.Color.Black
        Me.cmdSave.Appearance.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.ForwardDiagonal
        Me.cmdSave.Appearance.Options.UseBackColor = True
        Me.cmdSave.Appearance.Options.UseBorderColor = True
        Me.cmdSave.Appearance.Options.UseFont = True
        Me.cmdSave.Appearance.Options.UseForeColor = True
        Me.cmdSave.Cursor = System.Windows.Forms.Cursors.Hand
        Me.cmdSave.ImageIndex = 2
        Me.cmdSave.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleLeft
        Me.cmdSave.Location = New System.Drawing.Point(122, 156)
        Me.cmdSave.Name = "cmdSave"
        Me.cmdSave.Size = New System.Drawing.Size(104, 34)
        Me.cmdSave.TabIndex = 9
        Me.cmdSave.Text = "&Save"
        '
        'cmdClose
        '
        Me.cmdClose.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdClose.Appearance.BackColor = System.Drawing.Color.Snow
        Me.cmdClose.Appearance.BackColor2 = System.Drawing.Color.PowderBlue
        Me.cmdClose.Appearance.BorderColor = System.Drawing.SystemColors.ButtonFace
        Me.cmdClose.Appearance.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdClose.Appearance.ForeColor = System.Drawing.Color.Black
        Me.cmdClose.Appearance.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.ForwardDiagonal
        Me.cmdClose.Appearance.Options.UseBackColor = True
        Me.cmdClose.Appearance.Options.UseBorderColor = True
        Me.cmdClose.Appearance.Options.UseFont = True
        Me.cmdClose.Appearance.Options.UseForeColor = True
        Me.cmdClose.Cursor = System.Windows.Forms.Cursors.Hand
        Me.cmdClose.ImageIndex = 1
        Me.cmdClose.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleLeft
        Me.cmdClose.Location = New System.Drawing.Point(231, 156)
        Me.cmdClose.Name = "cmdClose"
        Me.cmdClose.Size = New System.Drawing.Size(104, 34)
        Me.cmdClose.TabIndex = 10
        Me.cmdClose.Text = "&Close"
        '
        'txtKode
        '
        Me.txtKode.EnterMoveNextControl = True
        Me.txtKode.Location = New System.Drawing.Point(128, 40)
        Me.txtKode.Name = "txtKode"
        Me.txtKode.Properties.Appearance.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtKode.Properties.Appearance.Options.UseFont = True
        Me.txtKode.Size = New System.Drawing.Size(207, 22)
        Me.txtKode.TabIndex = 3
        '
        'txtNama
        '
        Me.txtNama.EnterMoveNextControl = True
        Me.txtNama.Location = New System.Drawing.Point(128, 68)
        Me.txtNama.Name = "txtNama"
        Me.txtNama.Properties.Appearance.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtNama.Properties.Appearance.Options.UseFont = True
        Me.txtNama.Size = New System.Drawing.Size(207, 22)
        Me.txtNama.TabIndex = 5
        '
        'txtAlias
        '
        Me.txtAlias.EditValue = ""
        Me.txtAlias.EnterMoveNextControl = True
        Me.txtAlias.Location = New System.Drawing.Point(128, 96)
        Me.txtAlias.Name = "txtAlias"
        Me.txtAlias.Properties.Appearance.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtAlias.Properties.Appearance.Options.UseFont = True
        Me.txtAlias.Size = New System.Drawing.Size(207, 22)
        Me.txtAlias.TabIndex = 7
        '
        'ckAktif
        '
        Me.ckAktif.EditValue = True
        Me.ckAktif.Location = New System.Drawing.Point(126, 122)
        Me.ckAktif.Name = "ckAktif"
        Me.ckAktif.Properties.Appearance.Font = New System.Drawing.Font("Tahoma", 9.75!)
        Me.ckAktif.Properties.Appearance.Options.UseFont = True
        Me.ckAktif.Properties.Caption = "Aktif"
        Me.ckAktif.Size = New System.Drawing.Size(75, 20)
        Me.ckAktif.TabIndex = 8
        Me.ckAktif.Visible = False
        '
        'frmEntriSubKlasAkun
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(346, 202)
        Me.Controls.Add(Me.cmdSave)
        Me.Controls.Add(Me.cmdClose)
        Me.Controls.Add(Me.ckAktif)
        Me.Controls.Add(Me.txtAlias)
        Me.Controls.Add(Me.txtNama)
        Me.Controls.Add(Me.txtKode)
        Me.Controls.Add(Me.LabelControl10)
        Me.Controls.Add(Me.LabelControl8)
        Me.Controls.Add(Me.LabelControl4)
        Me.Controls.Add(Me.txtKlas)
        Me.Controls.Add(Me.LabelControl2)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmEntriSubKlasAkun"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Entri Sub Klas Akun"
        CType(Me.txtKlas.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtKode.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtNama.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtAlias.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.ckAktif.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents txtKlas As DevExpress.XtraEditors.ButtonEdit
    Friend WithEvents LabelControl2 As DevExpress.XtraEditors.LabelControl
    Friend WithEvents LabelControl8 As DevExpress.XtraEditors.LabelControl
    Friend WithEvents LabelControl4 As DevExpress.XtraEditors.LabelControl
    Friend WithEvents LabelControl10 As DevExpress.XtraEditors.LabelControl
    Friend WithEvents cmdSave As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents cmdClose As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents txtKode As DevExpress.XtraEditors.TextEdit
    Friend WithEvents txtNama As DevExpress.XtraEditors.TextEdit
    Friend WithEvents txtAlias As DevExpress.XtraEditors.TextEdit
    Friend WithEvents ckAktif As DevExpress.XtraEditors.CheckEdit
End Class
