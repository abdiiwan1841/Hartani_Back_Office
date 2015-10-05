<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmDownloadPenjualanKasir
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
        Me.lbKassa = New DevExpress.XtraEditors.LabelControl
        Me.txtKassa = New DevExpress.XtraEditors.SearchLookUpEdit
        Me.gvKassa = New DevExpress.XtraGrid.Views.Grid.GridView
        Me.txtPath = New DevExpress.XtraEditors.ButtonEdit
        Me.LabelControl1 = New DevExpress.XtraEditors.LabelControl
        Me.TglSampai = New DevExpress.XtraEditors.DateEdit
        Me.LabelControl2 = New DevExpress.XtraEditors.LabelControl
        Me.LabelControl3 = New DevExpress.XtraEditors.LabelControl
        Me.TglDari = New DevExpress.XtraEditors.DateEdit
        Me.cmdSave = New DevExpress.XtraEditors.SimpleButton
        Me.cmdTutup = New DevExpress.XtraEditors.SimpleButton
        Me.PB1 = New DevExpress.XtraEditors.ProgressBarControl
        Me.CheckEdit1 = New DevExpress.XtraEditors.CheckEdit
        Me.ProgressBarControl1 = New DevExpress.XtraEditors.ProgressBarControl
        CType(Me.txtKassa.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.gvKassa, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtPath.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.TglSampai.Properties.VistaTimeProperties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.TglSampai.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.TglDari.Properties.VistaTimeProperties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.TglDari.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PB1.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.CheckEdit1.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.ProgressBarControl1.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'lbKassa
        '
        Me.lbKassa.Appearance.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbKassa.Cursor = System.Windows.Forms.Cursors.Hand
        Me.lbKassa.Location = New System.Drawing.Point(15, 43)
        Me.lbKassa.Name = "lbKassa"
        Me.lbKassa.Size = New System.Drawing.Size(33, 16)
        Me.lbKassa.TabIndex = 39
        Me.lbKassa.Text = "Kassa"
        '
        'txtKassa
        '
        Me.txtKassa.EditValue = ""
        Me.txtKassa.EnterMoveNextControl = True
        Me.txtKassa.Location = New System.Drawing.Point(109, 40)
        Me.txtKassa.Name = "txtKassa"
        Me.txtKassa.Properties.Appearance.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtKassa.Properties.Appearance.Options.UseFont = True
        Me.txtKassa.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.txtKassa.Properties.DisplayMember = "Nama"
        Me.txtKassa.Properties.NullText = ""
        Me.txtKassa.Properties.ValueMember = "NoID"
        Me.txtKassa.Properties.View = Me.gvKassa
        Me.txtKassa.Size = New System.Drawing.Size(382, 22)
        Me.txtKassa.TabIndex = 38
        '
        'gvKassa
        '
        Me.gvKassa.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus
        Me.gvKassa.Name = "gvKassa"
        Me.gvKassa.OptionsSelection.EnableAppearanceFocusedCell = False
        Me.gvKassa.OptionsView.ShowGroupPanel = False
        '
        'txtPath
        '
        Me.txtPath.Location = New System.Drawing.Point(109, 68)
        Me.txtPath.Name = "txtPath"
        Me.txtPath.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton})
        Me.txtPath.Size = New System.Drawing.Size(382, 20)
        Me.txtPath.TabIndex = 40
        '
        'LabelControl1
        '
        Me.LabelControl1.Appearance.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LabelControl1.Cursor = System.Windows.Forms.Cursors.Hand
        Me.LabelControl1.Location = New System.Drawing.Point(15, 69)
        Me.LabelControl1.Name = "LabelControl1"
        Me.LabelControl1.Size = New System.Drawing.Size(72, 16)
        Me.LabelControl1.TabIndex = 41
        Me.LabelControl1.Text = "Path Aplikasi"
        '
        'TglSampai
        '
        Me.TglSampai.EditValue = Nothing
        Me.TglSampai.Location = New System.Drawing.Point(329, 9)
        Me.TglSampai.Name = "TglSampai"
        Me.TglSampai.Properties.Appearance.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TglSampai.Properties.Appearance.Options.UseFont = True
        Me.TglSampai.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.TglSampai.Properties.Mask.EditMask = "dd-MM-yyyy"
        Me.TglSampai.Properties.Mask.UseMaskAsDisplayFormat = True
        Me.TglSampai.Properties.VistaTimeProperties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton})
        Me.TglSampai.Size = New System.Drawing.Size(162, 22)
        Me.TglSampai.TabIndex = 45
        '
        'LabelControl2
        '
        Me.LabelControl2.Appearance.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LabelControl2.Location = New System.Drawing.Point(290, 12)
        Me.LabelControl2.Name = "LabelControl2"
        Me.LabelControl2.Size = New System.Drawing.Size(18, 16)
        Me.LabelControl2.TabIndex = 44
        Me.LabelControl2.Text = "s/d"
        '
        'LabelControl3
        '
        Me.LabelControl3.Appearance.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LabelControl3.Cursor = System.Windows.Forms.Cursors.Hand
        Me.LabelControl3.Location = New System.Drawing.Point(15, 12)
        Me.LabelControl3.Name = "LabelControl3"
        Me.LabelControl3.Size = New System.Drawing.Size(46, 16)
        Me.LabelControl3.TabIndex = 43
        Me.LabelControl3.Text = "Tanggal"
        Me.LabelControl3.ToolTip = "Klik disini untuk mengaktifkan atau menonaktifkan filter Tanggal, klik Refresh un" & _
            "tuk menampilkan hasil filter"
        '
        'TglDari
        '
        Me.TglDari.EditValue = Nothing
        Me.TglDari.Location = New System.Drawing.Point(109, 9)
        Me.TglDari.Name = "TglDari"
        Me.TglDari.Properties.Appearance.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TglDari.Properties.Appearance.Options.UseFont = True
        Me.TglDari.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.TglDari.Properties.Mask.EditMask = "dd-MM-yyyy"
        Me.TglDari.Properties.Mask.UseMaskAsDisplayFormat = True
        Me.TglDari.Properties.VistaTimeProperties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton})
        Me.TglDari.Size = New System.Drawing.Size(162, 22)
        Me.TglDari.TabIndex = 42
        '
        'cmdSave
        '
        Me.cmdSave.Appearance.Font = New System.Drawing.Font("Tahoma", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdSave.Appearance.ForeColor = System.Drawing.Color.Blue
        Me.cmdSave.Appearance.Options.UseFont = True
        Me.cmdSave.Appearance.Options.UseForeColor = True
        Me.cmdSave.ImageIndex = 10
        Me.cmdSave.Location = New System.Drawing.Point(315, 142)
        Me.cmdSave.Name = "cmdSave"
        Me.cmdSave.Size = New System.Drawing.Size(88, 25)
        Me.cmdSave.TabIndex = 46
        Me.cmdSave.Text = "&Proses"
        '
        'cmdTutup
        '
        Me.cmdTutup.Appearance.Font = New System.Drawing.Font("Tahoma", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdTutup.Appearance.ForeColor = System.Drawing.Color.Blue
        Me.cmdTutup.Appearance.Options.UseFont = True
        Me.cmdTutup.Appearance.Options.UseForeColor = True
        Me.cmdTutup.ImageIndex = 11
        Me.cmdTutup.Location = New System.Drawing.Point(409, 143)
        Me.cmdTutup.Name = "cmdTutup"
        Me.cmdTutup.Size = New System.Drawing.Size(82, 25)
        Me.cmdTutup.TabIndex = 47
        Me.cmdTutup.Text = "&Tutup"
        '
        'PB1
        '
        Me.PB1.Location = New System.Drawing.Point(15, 119)
        Me.PB1.Name = "PB1"
        Me.PB1.Size = New System.Drawing.Size(476, 18)
        Me.PB1.TabIndex = 49
        '
        'CheckEdit1
        '
        Me.CheckEdit1.Location = New System.Drawing.Point(12, 148)
        Me.CheckEdit1.Name = "CheckEdit1"
        Me.CheckEdit1.Properties.Caption = "Replace jika ada (Jika tidak di centang akan dilewati)"
        Me.CheckEdit1.Size = New System.Drawing.Size(296, 19)
        Me.CheckEdit1.TabIndex = 50
        '
        'ProgressBarControl1
        '
        Me.ProgressBarControl1.Location = New System.Drawing.Point(15, 95)
        Me.ProgressBarControl1.Name = "ProgressBarControl1"
        Me.ProgressBarControl1.Size = New System.Drawing.Size(476, 18)
        Me.ProgressBarControl1.TabIndex = 51
        '
        'FrmDownloadPenjualanKasir
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(502, 179)
        Me.Controls.Add(Me.ProgressBarControl1)
        Me.Controls.Add(Me.cmdSave)
        Me.Controls.Add(Me.CheckEdit1)
        Me.Controls.Add(Me.PB1)
        Me.Controls.Add(Me.cmdTutup)
        Me.Controls.Add(Me.TglSampai)
        Me.Controls.Add(Me.LabelControl2)
        Me.Controls.Add(Me.LabelControl3)
        Me.Controls.Add(Me.TglDari)
        Me.Controls.Add(Me.LabelControl1)
        Me.Controls.Add(Me.txtPath)
        Me.Controls.Add(Me.lbKassa)
        Me.Controls.Add(Me.txtKassa)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.Name = "FrmDownloadPenjualanKasir"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Download Data Penjualan Kasir"
        CType(Me.txtKassa.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.gvKassa, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtPath.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.TglSampai.Properties.VistaTimeProperties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.TglSampai.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.TglDari.Properties.VistaTimeProperties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.TglDari.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PB1.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.CheckEdit1.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.ProgressBarControl1.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents lbKassa As DevExpress.XtraEditors.LabelControl
    Friend WithEvents txtKassa As DevExpress.XtraEditors.SearchLookUpEdit
    Friend WithEvents gvKassa As DevExpress.XtraGrid.Views.Grid.GridView
    Friend WithEvents txtPath As DevExpress.XtraEditors.ButtonEdit
    Friend WithEvents LabelControl1 As DevExpress.XtraEditors.LabelControl
    Friend WithEvents TglSampai As DevExpress.XtraEditors.DateEdit
    Friend WithEvents LabelControl2 As DevExpress.XtraEditors.LabelControl
    Friend WithEvents LabelControl3 As DevExpress.XtraEditors.LabelControl
    Friend WithEvents TglDari As DevExpress.XtraEditors.DateEdit
    Friend WithEvents cmdSave As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents cmdTutup As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents PB1 As DevExpress.XtraEditors.ProgressBarControl
    Friend WithEvents CheckEdit1 As DevExpress.XtraEditors.CheckEdit
    Friend WithEvents ProgressBarControl1 As DevExpress.XtraEditors.ProgressBarControl
End Class
