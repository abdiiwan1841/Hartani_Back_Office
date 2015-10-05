<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmOpsiExportFP
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
        Me.SimpleButton6 = New DevExpress.XtraEditors.SimpleButton
        Me.cmdRefresh = New DevExpress.XtraEditors.SimpleButton
        Me.LabelControl3 = New DevExpress.XtraEditors.LabelControl
        Me.TgMasapajak = New DevExpress.XtraEditors.DateEdit
        Me.LabelControl1 = New DevExpress.XtraEditors.LabelControl
        Me.txtPembetulan = New DevExpress.XtraEditors.TextEdit
        Me.LabelControl2 = New DevExpress.XtraEditors.LabelControl
        Me.txtFileName = New DevExpress.XtraEditors.ButtonEdit
        CType(Me.TgMasapajak.Properties.VistaTimeProperties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.TgMasapajak.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtPembetulan.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtFileName.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'SimpleButton6
        '
        Me.SimpleButton6.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.SimpleButton6.Appearance.Font = New System.Drawing.Font("Tahoma", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.SimpleButton6.Appearance.ForeColor = System.Drawing.Color.Blue
        Me.SimpleButton6.Appearance.Options.UseFont = True
        Me.SimpleButton6.Appearance.Options.UseForeColor = True
        Me.SimpleButton6.ImageIndex = 11
        Me.SimpleButton6.Location = New System.Drawing.Point(195, 103)
        Me.SimpleButton6.Name = "SimpleButton6"
        Me.SimpleButton6.Size = New System.Drawing.Size(100, 31)
        Me.SimpleButton6.TabIndex = 7
        Me.SimpleButton6.Text = "&Batal"
        '
        'cmdRefresh
        '
        Me.cmdRefresh.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdRefresh.Appearance.Font = New System.Drawing.Font("Tahoma", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdRefresh.Appearance.ForeColor = System.Drawing.Color.Blue
        Me.cmdRefresh.Appearance.Options.UseFont = True
        Me.cmdRefresh.Appearance.Options.UseForeColor = True
        Me.cmdRefresh.ImageIndex = 4
        Me.cmdRefresh.Location = New System.Drawing.Point(89, 103)
        Me.cmdRefresh.Name = "cmdRefresh"
        Me.cmdRefresh.Size = New System.Drawing.Size(100, 31)
        Me.cmdRefresh.TabIndex = 6
        Me.cmdRefresh.Text = "&Simpan"
        '
        'LabelControl3
        '
        Me.LabelControl3.Appearance.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LabelControl3.Cursor = System.Windows.Forms.Cursors.Default
        Me.LabelControl3.Location = New System.Drawing.Point(12, 12)
        Me.LabelControl3.Name = "LabelControl3"
        Me.LabelControl3.Size = New System.Drawing.Size(65, 16)
        Me.LabelControl3.TabIndex = 0
        Me.LabelControl3.Text = "Masa Pajak"
        Me.LabelControl3.ToolTip = "Klik disini untuk mengaktifkan atau menonaktifkan filter Tanggal, klik Refresh un" & _
            "tuk menampilkan hasil filter"
        '
        'TgMasapajak
        '
        Me.TgMasapajak.EditValue = New Date(2012, 10, 30, 19, 8, 42, 0)
        Me.TgMasapajak.EnterMoveNextControl = True
        Me.TgMasapajak.Location = New System.Drawing.Point(83, 9)
        Me.TgMasapajak.Name = "TgMasapajak"
        Me.TgMasapajak.Properties.Appearance.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TgMasapajak.Properties.Appearance.Options.UseFont = True
        Me.TgMasapajak.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.TgMasapajak.Properties.Mask.EditMask = "MMMM-yyyy"
        Me.TgMasapajak.Properties.Mask.UseMaskAsDisplayFormat = True
        Me.TgMasapajak.Properties.ReadOnly = True
        Me.TgMasapajak.Properties.VistaTimeProperties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton})
        Me.TgMasapajak.Size = New System.Drawing.Size(212, 22)
        Me.TgMasapajak.TabIndex = 1
        '
        'LabelControl1
        '
        Me.LabelControl1.Appearance.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LabelControl1.Cursor = System.Windows.Forms.Cursors.Default
        Me.LabelControl1.Location = New System.Drawing.Point(12, 40)
        Me.LabelControl1.Name = "LabelControl1"
        Me.LabelControl1.Size = New System.Drawing.Size(67, 16)
        Me.LabelControl1.TabIndex = 2
        Me.LabelControl1.Text = "Pembetulan"
        '
        'txtPembetulan
        '
        Me.txtPembetulan.EditValue = 0
        Me.txtPembetulan.EnterMoveNextControl = True
        Me.txtPembetulan.Location = New System.Drawing.Point(83, 37)
        Me.txtPembetulan.Name = "txtPembetulan"
        Me.txtPembetulan.Properties.Appearance.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtPembetulan.Properties.Appearance.Options.UseFont = True
        Me.txtPembetulan.Properties.Mask.EditMask = "n0"
        Me.txtPembetulan.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric
        Me.txtPembetulan.Properties.Mask.UseMaskAsDisplayFormat = True
        Me.txtPembetulan.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.txtPembetulan.Size = New System.Drawing.Size(212, 22)
        Me.txtPembetulan.TabIndex = 3
        '
        'LabelControl2
        '
        Me.LabelControl2.Appearance.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LabelControl2.Cursor = System.Windows.Forms.Cursors.Default
        Me.LabelControl2.Location = New System.Drawing.Point(12, 68)
        Me.LabelControl2.Name = "LabelControl2"
        Me.LabelControl2.Size = New System.Drawing.Size(48, 16)
        Me.LabelControl2.TabIndex = 4
        Me.LabelControl2.Text = "File CSV"
        '
        'txtFileName
        '
        Me.txtFileName.EditValue = ""
        Me.txtFileName.Location = New System.Drawing.Point(83, 65)
        Me.txtFileName.Name = "txtFileName"
        Me.txtFileName.Properties.Appearance.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtFileName.Properties.Appearance.Options.UseFont = True
        Me.txtFileName.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton})
        Me.txtFileName.Size = New System.Drawing.Size(212, 22)
        Me.txtFileName.TabIndex = 5
        '
        'frmOpsiExportFP
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(307, 146)
        Me.Controls.Add(Me.LabelControl2)
        Me.Controls.Add(Me.LabelControl1)
        Me.Controls.Add(Me.LabelControl3)
        Me.Controls.Add(Me.TgMasapajak)
        Me.Controls.Add(Me.SimpleButton6)
        Me.Controls.Add(Me.cmdRefresh)
        Me.Controls.Add(Me.txtPembetulan)
        Me.Controls.Add(Me.txtFileName)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.Name = "frmOpsiExportFP"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Export FP"
        CType(Me.TgMasapajak.Properties.VistaTimeProperties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.TgMasapajak.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtPembetulan.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtFileName.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents SimpleButton6 As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents cmdRefresh As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents LabelControl3 As DevExpress.XtraEditors.LabelControl
    Friend WithEvents TgMasapajak As DevExpress.XtraEditors.DateEdit
    Friend WithEvents LabelControl1 As DevExpress.XtraEditors.LabelControl
    Friend WithEvents txtPembetulan As DevExpress.XtraEditors.TextEdit
    Friend WithEvents LabelControl2 As DevExpress.XtraEditors.LabelControl
    Friend WithEvents txtFileName As DevExpress.XtraEditors.ButtonEdit
End Class
