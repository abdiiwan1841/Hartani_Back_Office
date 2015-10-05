<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmMutasiData
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmMutasiData))
        Me.XtraTabControl1 = New DevExpress.XtraTab.XtraTabControl
        Me.XtraTabPage1 = New DevExpress.XtraTab.XtraTabPage
        Me.cmdSave = New DevExpress.XtraEditors.SimpleButton
        Me.PB1 = New DevExpress.XtraEditors.ProgressBarControl
        Me.LabelControl3 = New DevExpress.XtraEditors.LabelControl
        Me.TglMutasiDataBarang = New DevExpress.XtraEditors.DateEdit
        Me.LabelControl2 = New DevExpress.XtraEditors.LabelControl
        Me.XtraTabPage2 = New DevExpress.XtraTab.XtraTabPage
        Me.LabelControl1 = New DevExpress.XtraEditors.LabelControl
        Me.XtraTabPage3 = New DevExpress.XtraTab.XtraTabPage
        Me.SimpleButton1 = New DevExpress.XtraEditors.SimpleButton
        Me.ProgressBarControl1 = New DevExpress.XtraEditors.ProgressBarControl
        Me.LabelControl5 = New DevExpress.XtraEditors.LabelControl
        Me.DateEdit1 = New DevExpress.XtraEditors.DateEdit
        Me.LabelControl4 = New DevExpress.XtraEditors.LabelControl
        CType(Me.XtraTabControl1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.XtraTabControl1.SuspendLayout()
        Me.XtraTabPage1.SuspendLayout()
        CType(Me.PB1.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.TglMutasiDataBarang.Properties.VistaTimeProperties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.TglMutasiDataBarang.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.XtraTabPage2.SuspendLayout()
        Me.XtraTabPage3.SuspendLayout()
        CType(Me.ProgressBarControl1.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.DateEdit1.Properties.VistaTimeProperties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.DateEdit1.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'XtraTabControl1
        '
        Me.XtraTabControl1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.XtraTabControl1.Location = New System.Drawing.Point(0, 0)
        Me.XtraTabControl1.Name = "XtraTabControl1"
        Me.XtraTabControl1.SelectedTabPage = Me.XtraTabPage1
        Me.XtraTabControl1.Size = New System.Drawing.Size(681, 314)
        Me.XtraTabControl1.TabIndex = 52
        Me.XtraTabControl1.TabPages.AddRange(New DevExpress.XtraTab.XtraTabPage() {Me.XtraTabPage1, Me.XtraTabPage2, Me.XtraTabPage3})
        '
        'XtraTabPage1
        '
        Me.XtraTabPage1.Controls.Add(Me.cmdSave)
        Me.XtraTabPage1.Controls.Add(Me.PB1)
        Me.XtraTabPage1.Controls.Add(Me.LabelControl3)
        Me.XtraTabPage1.Controls.Add(Me.TglMutasiDataBarang)
        Me.XtraTabPage1.Controls.Add(Me.LabelControl2)
        Me.XtraTabPage1.Name = "XtraTabPage1"
        Me.XtraTabPage1.Size = New System.Drawing.Size(675, 288)
        Me.XtraTabPage1.Text = "Mutasi Data Stok Barang"
        '
        'cmdSave
        '
        Me.cmdSave.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.cmdSave.Appearance.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdSave.Appearance.ForeColor = System.Drawing.Color.Blue
        Me.cmdSave.Appearance.Options.UseFont = True
        Me.cmdSave.Appearance.Options.UseForeColor = True
        Me.cmdSave.ImageIndex = 10
        Me.cmdSave.Location = New System.Drawing.Point(406, 172)
        Me.cmdSave.Name = "cmdSave"
        Me.cmdSave.Size = New System.Drawing.Size(88, 22)
        Me.cmdSave.TabIndex = 51
        Me.cmdSave.Text = "&Proses"
        '
        'PB1
        '
        Me.PB1.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.PB1.Location = New System.Drawing.Point(11, 200)
        Me.PB1.Name = "PB1"
        Me.PB1.Size = New System.Drawing.Size(657, 23)
        Me.PB1.TabIndex = 50
        Me.PB1.Visible = False
        '
        'LabelControl3
        '
        Me.LabelControl3.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.LabelControl3.Appearance.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LabelControl3.Location = New System.Drawing.Point(11, 175)
        Me.LabelControl3.Name = "LabelControl3"
        Me.LabelControl3.Size = New System.Drawing.Size(221, 16)
        Me.LabelControl3.TabIndex = 4
        Me.LabelControl3.Text = "Tanggal Data Barang yg Akan dimutasi"
        '
        'TglMutasiDataBarang
        '
        Me.TglMutasiDataBarang.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.TglMutasiDataBarang.EditValue = New Date(2015, 9, 12, 11, 59, 32, 874)
        Me.TglMutasiDataBarang.Location = New System.Drawing.Point(238, 172)
        Me.TglMutasiDataBarang.Name = "TglMutasiDataBarang"
        Me.TglMutasiDataBarang.Properties.Appearance.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TglMutasiDataBarang.Properties.Appearance.Options.UseFont = True
        Me.TglMutasiDataBarang.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.TglMutasiDataBarang.Properties.Mask.EditMask = "dd-MM-yyyy"
        Me.TglMutasiDataBarang.Properties.Mask.UseMaskAsDisplayFormat = True
        Me.TglMutasiDataBarang.Properties.VistaTimeProperties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton})
        Me.TglMutasiDataBarang.Size = New System.Drawing.Size(162, 22)
        Me.TglMutasiDataBarang.TabIndex = 3
        '
        'LabelControl2
        '
        Me.LabelControl2.Appearance.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LabelControl2.Appearance.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap
        Me.LabelControl2.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
        Me.LabelControl2.Dock = System.Windows.Forms.DockStyle.Top
        Me.LabelControl2.Location = New System.Drawing.Point(0, 0)
        Me.LabelControl2.Name = "LabelControl2"
        Me.LabelControl2.Padding = New System.Windows.Forms.Padding(50)
        Me.LabelControl2.Size = New System.Drawing.Size(675, 120)
        Me.LabelControl2.TabIndex = 1
        Me.LabelControl2.Text = resources.GetString("LabelControl2.Text")
        '
        'XtraTabPage2
        '
        Me.XtraTabPage2.Controls.Add(Me.LabelControl1)
        Me.XtraTabPage2.Name = "XtraTabPage2"
        Me.XtraTabPage2.Size = New System.Drawing.Size(675, 288)
        Me.XtraTabPage2.Text = "Mutasi Data Poin Customer"
        '
        'LabelControl1
        '
        Me.LabelControl1.Appearance.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LabelControl1.Appearance.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap
        Me.LabelControl1.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
        Me.LabelControl1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.LabelControl1.Location = New System.Drawing.Point(0, 0)
        Me.LabelControl1.Name = "LabelControl1"
        Me.LabelControl1.Padding = New System.Windows.Forms.Padding(100)
        Me.LabelControl1.Size = New System.Drawing.Size(675, 288)
        Me.LabelControl1.TabIndex = 0
        Me.LabelControl1.Text = "Data Poin Customer Akan Berakhir / Hangus. Otomatis Pada Tanggal 31 Desember Tahu" & _
            "n Aktif." & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "Untuk karenanya tidak memerlukan perpindahan data Poin Customer."
        '
        'XtraTabPage3
        '
        Me.XtraTabPage3.Controls.Add(Me.SimpleButton1)
        Me.XtraTabPage3.Controls.Add(Me.ProgressBarControl1)
        Me.XtraTabPage3.Controls.Add(Me.LabelControl5)
        Me.XtraTabPage3.Controls.Add(Me.DateEdit1)
        Me.XtraTabPage3.Controls.Add(Me.LabelControl4)
        Me.XtraTabPage3.Name = "XtraTabPage3"
        Me.XtraTabPage3.Size = New System.Drawing.Size(675, 288)
        Me.XtraTabPage3.Text = "Potong Data Penjualan Kasir"
        '
        'SimpleButton1
        '
        Me.SimpleButton1.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.SimpleButton1.Appearance.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.SimpleButton1.Appearance.ForeColor = System.Drawing.Color.Blue
        Me.SimpleButton1.Appearance.Options.UseFont = True
        Me.SimpleButton1.Appearance.Options.UseForeColor = True
        Me.SimpleButton1.ImageIndex = 10
        Me.SimpleButton1.Location = New System.Drawing.Point(403, 173)
        Me.SimpleButton1.Name = "SimpleButton1"
        Me.SimpleButton1.Size = New System.Drawing.Size(88, 22)
        Me.SimpleButton1.TabIndex = 55
        Me.SimpleButton1.Text = "&Proses"
        '
        'ProgressBarControl1
        '
        Me.ProgressBarControl1.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ProgressBarControl1.Location = New System.Drawing.Point(11, 201)
        Me.ProgressBarControl1.Name = "ProgressBarControl1"
        Me.ProgressBarControl1.Size = New System.Drawing.Size(655, 23)
        Me.ProgressBarControl1.TabIndex = 54
        Me.ProgressBarControl1.Visible = False
        '
        'LabelControl5
        '
        Me.LabelControl5.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.LabelControl5.Appearance.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LabelControl5.Location = New System.Drawing.Point(11, 176)
        Me.LabelControl5.Name = "LabelControl5"
        Me.LabelControl5.Size = New System.Drawing.Size(232, 16)
        Me.LabelControl5.TabIndex = 53
        Me.LabelControl5.Text = "Tanggal Data Penjualan yg akan dihapus"
        '
        'DateEdit1
        '
        Me.DateEdit1.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.DateEdit1.EditValue = Nothing
        Me.DateEdit1.Location = New System.Drawing.Point(246, 173)
        Me.DateEdit1.Name = "DateEdit1"
        Me.DateEdit1.Properties.Appearance.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.DateEdit1.Properties.Appearance.Options.UseFont = True
        Me.DateEdit1.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.DateEdit1.Properties.Mask.EditMask = "dd-MM-yyyy"
        Me.DateEdit1.Properties.Mask.UseMaskAsDisplayFormat = True
        Me.DateEdit1.Properties.VistaTimeProperties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton})
        Me.DateEdit1.Size = New System.Drawing.Size(151, 22)
        Me.DateEdit1.TabIndex = 52
        '
        'LabelControl4
        '
        Me.LabelControl4.Appearance.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LabelControl4.Appearance.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap
        Me.LabelControl4.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
        Me.LabelControl4.Dock = System.Windows.Forms.DockStyle.Top
        Me.LabelControl4.Location = New System.Drawing.Point(0, 0)
        Me.LabelControl4.Name = "LabelControl4"
        Me.LabelControl4.Padding = New System.Windows.Forms.Padding(50)
        Me.LabelControl4.Size = New System.Drawing.Size(675, 120)
        Me.LabelControl4.TabIndex = 2
        Me.LabelControl4.Text = resources.GetString("LabelControl4.Text")
        '
        'frmMutasiData
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(681, 314)
        Me.Controls.Add(Me.XtraTabControl1)
        Me.MaximizeBox = False
        Me.MaximumSize = New System.Drawing.Size(697, 353)
        Me.MinimumSize = New System.Drawing.Size(522, 259)
        Me.Name = "frmMutasiData"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Mutasi Data"
        CType(Me.XtraTabControl1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.XtraTabControl1.ResumeLayout(False)
        Me.XtraTabPage1.ResumeLayout(False)
        Me.XtraTabPage1.PerformLayout()
        CType(Me.PB1.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.TglMutasiDataBarang.Properties.VistaTimeProperties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.TglMutasiDataBarang.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        Me.XtraTabPage2.ResumeLayout(False)
        Me.XtraTabPage3.ResumeLayout(False)
        Me.XtraTabPage3.PerformLayout()
        CType(Me.ProgressBarControl1.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.DateEdit1.Properties.VistaTimeProperties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.DateEdit1.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents XtraTabControl1 As DevExpress.XtraTab.XtraTabControl
    Friend WithEvents XtraTabPage1 As DevExpress.XtraTab.XtraTabPage
    Friend WithEvents XtraTabPage2 As DevExpress.XtraTab.XtraTabPage
    Friend WithEvents XtraTabPage3 As DevExpress.XtraTab.XtraTabPage
    Friend WithEvents LabelControl1 As DevExpress.XtraEditors.LabelControl
    Friend WithEvents LabelControl2 As DevExpress.XtraEditors.LabelControl
    Friend WithEvents LabelControl3 As DevExpress.XtraEditors.LabelControl
    Friend WithEvents TglMutasiDataBarang As DevExpress.XtraEditors.DateEdit
    Friend WithEvents PB1 As DevExpress.XtraEditors.ProgressBarControl
    Friend WithEvents cmdSave As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents LabelControl4 As DevExpress.XtraEditors.LabelControl
    Friend WithEvents SimpleButton1 As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents ProgressBarControl1 As DevExpress.XtraEditors.ProgressBarControl
    Friend WithEvents LabelControl5 As DevExpress.XtraEditors.LabelControl
    Friend WithEvents DateEdit1 As DevExpress.XtraEditors.DateEdit
End Class
