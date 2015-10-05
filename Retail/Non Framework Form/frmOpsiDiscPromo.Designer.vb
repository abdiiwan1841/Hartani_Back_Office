<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmOpsiDiscPromo
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
        Me.cmdBatal = New DevExpress.XtraEditors.SimpleButton
        Me.cmdSave = New DevExpress.XtraEditors.SimpleButton
        Me.LabelControl3 = New DevExpress.XtraEditors.LabelControl
        Me.txtTglDari = New DevExpress.XtraEditors.DateEdit
        Me.LabelControl1 = New DevExpress.XtraEditors.LabelControl
        Me.txtDiscRp = New DevExpress.XtraEditors.TextEdit
        Me.LabelControl2 = New DevExpress.XtraEditors.LabelControl
        Me.LabelControl4 = New DevExpress.XtraEditors.LabelControl
        Me.txtTglSampai = New DevExpress.XtraEditors.DateEdit
        Me.LabelControl5 = New DevExpress.XtraEditors.LabelControl
        Me.txtDiscProsen = New DevExpress.XtraEditors.TextEdit
        Me.txtUserEdit = New DevExpress.XtraEditors.TextEdit
        Me.LabelControl6 = New DevExpress.XtraEditors.LabelControl
        Me.txtTglEdit = New DevExpress.XtraEditors.DateEdit
        Me.LabelControl7 = New DevExpress.XtraEditors.LabelControl
        Me.txtQty = New DevExpress.XtraEditors.TextEdit
        Me.txtDiscRp2 = New DevExpress.XtraEditors.TextEdit
        CType(Me.txtTglDari.Properties.VistaTimeProperties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtTglDari.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtDiscRp.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtTglSampai.Properties.VistaTimeProperties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtTglSampai.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtDiscProsen.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtUserEdit.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtTglEdit.Properties.VistaTimeProperties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtTglEdit.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtQty.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtDiscRp2.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'cmdBatal
        '
        Me.cmdBatal.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdBatal.Appearance.Font = New System.Drawing.Font("Tahoma", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdBatal.Appearance.ForeColor = System.Drawing.Color.Blue
        Me.cmdBatal.Appearance.Options.UseFont = True
        Me.cmdBatal.Appearance.Options.UseForeColor = True
        Me.cmdBatal.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.cmdBatal.ImageIndex = 11
        Me.cmdBatal.Location = New System.Drawing.Point(229, 209)
        Me.cmdBatal.Name = "cmdBatal"
        Me.cmdBatal.Size = New System.Drawing.Size(100, 31)
        Me.cmdBatal.TabIndex = 16
        Me.cmdBatal.Text = "&Batal"
        '
        'cmdSave
        '
        Me.cmdSave.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdSave.Appearance.Font = New System.Drawing.Font("Tahoma", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdSave.Appearance.ForeColor = System.Drawing.Color.Blue
        Me.cmdSave.Appearance.Options.UseFont = True
        Me.cmdSave.Appearance.Options.UseForeColor = True
        Me.cmdSave.ImageIndex = 4
        Me.cmdSave.Location = New System.Drawing.Point(123, 209)
        Me.cmdSave.Name = "cmdSave"
        Me.cmdSave.Size = New System.Drawing.Size(100, 31)
        Me.cmdSave.TabIndex = 15
        Me.cmdSave.Text = "&Simpan"
        '
        'LabelControl3
        '
        Me.LabelControl3.Appearance.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LabelControl3.Cursor = System.Windows.Forms.Cursors.Default
        Me.LabelControl3.Location = New System.Drawing.Point(9, 12)
        Me.LabelControl3.Name = "LabelControl3"
        Me.LabelControl3.Size = New System.Drawing.Size(43, 16)
        Me.LabelControl3.TabIndex = 0
        Me.LabelControl3.Text = "Periode"
        '
        'txtTglDari
        '
        Me.txtTglDari.EditValue = New Date(2012, 10, 30, 19, 8, 42, 0)
        Me.txtTglDari.EnterMoveNextControl = True
        Me.txtTglDari.Location = New System.Drawing.Point(117, 9)
        Me.txtTglDari.Name = "txtTglDari"
        Me.txtTglDari.Properties.Appearance.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtTglDari.Properties.Appearance.Options.UseFont = True
        Me.txtTglDari.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.txtTglDari.Properties.Mask.EditMask = "dd-MM-yyyy"
        Me.txtTglDari.Properties.Mask.UseMaskAsDisplayFormat = True
        Me.txtTglDari.Properties.VistaTimeProperties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton})
        Me.txtTglDari.Size = New System.Drawing.Size(89, 22)
        Me.txtTglDari.TabIndex = 1
        '
        'LabelControl1
        '
        Me.LabelControl1.Appearance.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LabelControl1.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Top
        Me.LabelControl1.Appearance.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap
        Me.LabelControl1.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
        Me.LabelControl1.Cursor = System.Windows.Forms.Cursors.Hand
        Me.LabelControl1.Location = New System.Drawing.Point(9, 68)
        Me.LabelControl1.Name = "LabelControl1"
        Me.LabelControl1.Size = New System.Drawing.Size(93, 47)
        Me.LabelControl1.TabIndex = 6
        Me.LabelControl1.Text = "Disc Promo (Rp)"
        Me.LabelControl1.ToolTip = "Klik disini untuk merubah Diskon"
        '
        'txtDiscRp
        '
        Me.txtDiscRp.EditValue = 0
        Me.txtDiscRp.EnterMoveNextControl = True
        Me.txtDiscRp.Location = New System.Drawing.Point(117, 65)
        Me.txtDiscRp.Name = "txtDiscRp"
        Me.txtDiscRp.Properties.Appearance.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtDiscRp.Properties.Appearance.Options.UseFont = True
        Me.txtDiscRp.Properties.Mask.EditMask = "n2"
        Me.txtDiscRp.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric
        Me.txtDiscRp.Properties.Mask.UseMaskAsDisplayFormat = True
        Me.txtDiscRp.Properties.ReadOnly = True
        Me.txtDiscRp.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.txtDiscRp.Size = New System.Drawing.Size(212, 22)
        Me.txtDiscRp.TabIndex = 7
        '
        'LabelControl2
        '
        Me.LabelControl2.Appearance.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LabelControl2.Cursor = System.Windows.Forms.Cursors.Default
        Me.LabelControl2.Location = New System.Drawing.Point(9, 152)
        Me.LabelControl2.Name = "LabelControl2"
        Me.LabelControl2.Size = New System.Drawing.Size(51, 16)
        Me.LabelControl2.TabIndex = 11
        Me.LabelControl2.Text = "User Edit"
        '
        'LabelControl4
        '
        Me.LabelControl4.Appearance.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LabelControl4.Cursor = System.Windows.Forms.Cursors.Hand
        Me.LabelControl4.Location = New System.Drawing.Point(9, 40)
        Me.LabelControl4.Name = "LabelControl4"
        Me.LabelControl4.Size = New System.Drawing.Size(90, 16)
        Me.LabelControl4.TabIndex = 4
        Me.LabelControl4.Text = "Disc Promo (%)"
        Me.LabelControl4.ToolTip = "Klik disini untuk merubah Diskon"
        '
        'txtTglSampai
        '
        Me.txtTglSampai.EditValue = New Date(2012, 10, 30, 19, 8, 42, 0)
        Me.txtTglSampai.EnterMoveNextControl = True
        Me.txtTglSampai.Location = New System.Drawing.Point(236, 9)
        Me.txtTglSampai.Name = "txtTglSampai"
        Me.txtTglSampai.Properties.Appearance.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtTglSampai.Properties.Appearance.Options.UseFont = True
        Me.txtTglSampai.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.txtTglSampai.Properties.Mask.EditMask = "dd-MM-yyyy"
        Me.txtTglSampai.Properties.Mask.UseMaskAsDisplayFormat = True
        Me.txtTglSampai.Properties.VistaTimeProperties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton})
        Me.txtTglSampai.Size = New System.Drawing.Size(93, 22)
        Me.txtTglSampai.TabIndex = 3
        '
        'LabelControl5
        '
        Me.LabelControl5.Appearance.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LabelControl5.Cursor = System.Windows.Forms.Cursors.Default
        Me.LabelControl5.Location = New System.Drawing.Point(212, 12)
        Me.LabelControl5.Name = "LabelControl5"
        Me.LabelControl5.Size = New System.Drawing.Size(18, 16)
        Me.LabelControl5.TabIndex = 2
        Me.LabelControl5.Text = "s/d"
        '
        'txtDiscProsen
        '
        Me.txtDiscProsen.EditValue = 0
        Me.txtDiscProsen.EnterMoveNextControl = True
        Me.txtDiscProsen.Location = New System.Drawing.Point(117, 37)
        Me.txtDiscProsen.Name = "txtDiscProsen"
        Me.txtDiscProsen.Properties.Appearance.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtDiscProsen.Properties.Appearance.Options.UseFont = True
        Me.txtDiscProsen.Properties.Mask.EditMask = "n2"
        Me.txtDiscProsen.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric
        Me.txtDiscProsen.Properties.Mask.UseMaskAsDisplayFormat = True
        Me.txtDiscProsen.Properties.ReadOnly = True
        Me.txtDiscProsen.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.txtDiscProsen.Size = New System.Drawing.Size(212, 22)
        Me.txtDiscProsen.TabIndex = 5
        '
        'txtUserEdit
        '
        Me.txtUserEdit.EditValue = ""
        Me.txtUserEdit.EnterMoveNextControl = True
        Me.txtUserEdit.Location = New System.Drawing.Point(117, 149)
        Me.txtUserEdit.Name = "txtUserEdit"
        Me.txtUserEdit.Properties.Appearance.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtUserEdit.Properties.Appearance.Options.UseFont = True
        Me.txtUserEdit.Properties.ReadOnly = True
        Me.txtUserEdit.Size = New System.Drawing.Size(212, 22)
        Me.txtUserEdit.TabIndex = 12
        '
        'LabelControl6
        '
        Me.LabelControl6.Appearance.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LabelControl6.Cursor = System.Windows.Forms.Cursors.Default
        Me.LabelControl6.Location = New System.Drawing.Point(9, 180)
        Me.LabelControl6.Name = "LabelControl6"
        Me.LabelControl6.Size = New System.Drawing.Size(43, 16)
        Me.LabelControl6.TabIndex = 13
        Me.LabelControl6.Text = "Tgl Edit"
        '
        'txtTglEdit
        '
        Me.txtTglEdit.EditValue = New Date(2012, 10, 30, 19, 8, 42, 0)
        Me.txtTglEdit.EnterMoveNextControl = True
        Me.txtTglEdit.Location = New System.Drawing.Point(117, 177)
        Me.txtTglEdit.Name = "txtTglEdit"
        Me.txtTglEdit.Properties.Appearance.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtTglEdit.Properties.Appearance.Options.UseFont = True
        Me.txtTglEdit.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.txtTglEdit.Properties.Mask.EditMask = "dd-MM-yyyy"
        Me.txtTglEdit.Properties.Mask.UseMaskAsDisplayFormat = True
        Me.txtTglEdit.Properties.ReadOnly = True
        Me.txtTglEdit.Properties.VistaTimeProperties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton})
        Me.txtTglEdit.Size = New System.Drawing.Size(212, 22)
        Me.txtTglEdit.TabIndex = 14
        '
        'LabelControl7
        '
        Me.LabelControl7.Appearance.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LabelControl7.Cursor = System.Windows.Forms.Cursors.Hand
        Me.LabelControl7.Location = New System.Drawing.Point(9, 124)
        Me.LabelControl7.Name = "LabelControl7"
        Me.LabelControl7.Size = New System.Drawing.Size(64, 16)
        Me.LabelControl7.TabIndex = 9
        Me.LabelControl7.Text = "QtyPcs PDP"
        Me.LabelControl7.ToolTip = "Klik disini untuk merubah Diskon"
        '
        'txtQty
        '
        Me.txtQty.EditValue = 0
        Me.txtQty.EnterMoveNextControl = True
        Me.txtQty.Location = New System.Drawing.Point(117, 121)
        Me.txtQty.Name = "txtQty"
        Me.txtQty.Properties.Appearance.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtQty.Properties.Appearance.Options.UseFont = True
        Me.txtQty.Properties.Mask.EditMask = "n2"
        Me.txtQty.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric
        Me.txtQty.Properties.Mask.UseMaskAsDisplayFormat = True
        Me.txtQty.Properties.ReadOnly = True
        Me.txtQty.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.txtQty.Size = New System.Drawing.Size(212, 22)
        Me.txtQty.TabIndex = 10
        '
        'txtDiscRp2
        '
        Me.txtDiscRp2.EditValue = 0
        Me.txtDiscRp2.EnterMoveNextControl = True
        Me.txtDiscRp2.Location = New System.Drawing.Point(117, 93)
        Me.txtDiscRp2.Name = "txtDiscRp2"
        Me.txtDiscRp2.Properties.Appearance.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtDiscRp2.Properties.Appearance.Options.UseFont = True
        Me.txtDiscRp2.Properties.Mask.EditMask = "n2"
        Me.txtDiscRp2.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric
        Me.txtDiscRp2.Properties.Mask.UseMaskAsDisplayFormat = True
        Me.txtDiscRp2.Properties.ReadOnly = True
        Me.txtDiscRp2.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.txtDiscRp2.Size = New System.Drawing.Size(212, 22)
        Me.txtDiscRp2.TabIndex = 8
        Me.txtDiscRp2.Visible = False
        '
        'frmOpsiDiscPromo
        '
        Me.AcceptButton = Me.cmdSave
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.cmdBatal
        Me.ClientSize = New System.Drawing.Size(341, 252)
        Me.Controls.Add(Me.txtDiscRp2)
        Me.Controls.Add(Me.LabelControl7)
        Me.Controls.Add(Me.txtQty)
        Me.Controls.Add(Me.txtTglEdit)
        Me.Controls.Add(Me.LabelControl6)
        Me.Controls.Add(Me.txtDiscProsen)
        Me.Controls.Add(Me.LabelControl5)
        Me.Controls.Add(Me.LabelControl4)
        Me.Controls.Add(Me.txtTglSampai)
        Me.Controls.Add(Me.LabelControl2)
        Me.Controls.Add(Me.LabelControl1)
        Me.Controls.Add(Me.LabelControl3)
        Me.Controls.Add(Me.txtTglDari)
        Me.Controls.Add(Me.cmdBatal)
        Me.Controls.Add(Me.cmdSave)
        Me.Controls.Add(Me.txtDiscRp)
        Me.Controls.Add(Me.txtUserEdit)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.Name = "frmOpsiDiscPromo"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Opsi Disc / Promo"
        CType(Me.txtTglDari.Properties.VistaTimeProperties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtTglDari.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtDiscRp.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtTglSampai.Properties.VistaTimeProperties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtTglSampai.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtDiscProsen.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtUserEdit.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtTglEdit.Properties.VistaTimeProperties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtTglEdit.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtQty.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtDiscRp2.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents cmdBatal As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents cmdSave As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents LabelControl3 As DevExpress.XtraEditors.LabelControl
    Friend WithEvents txtTglDari As DevExpress.XtraEditors.DateEdit
    Friend WithEvents LabelControl1 As DevExpress.XtraEditors.LabelControl
    Friend WithEvents txtDiscRp As DevExpress.XtraEditors.TextEdit
    Friend WithEvents LabelControl2 As DevExpress.XtraEditors.LabelControl
    Friend WithEvents LabelControl4 As DevExpress.XtraEditors.LabelControl
    Friend WithEvents txtTglSampai As DevExpress.XtraEditors.DateEdit
    Friend WithEvents LabelControl5 As DevExpress.XtraEditors.LabelControl
    Friend WithEvents txtDiscProsen As DevExpress.XtraEditors.TextEdit
    Friend WithEvents txtUserEdit As DevExpress.XtraEditors.TextEdit
    Friend WithEvents LabelControl6 As DevExpress.XtraEditors.LabelControl
    Friend WithEvents txtTglEdit As DevExpress.XtraEditors.DateEdit
    Friend WithEvents LabelControl7 As DevExpress.XtraEditors.LabelControl
    Friend WithEvents txtQty As DevExpress.XtraEditors.TextEdit
    Friend WithEvents txtDiscRp2 As DevExpress.XtraEditors.TextEdit
End Class
