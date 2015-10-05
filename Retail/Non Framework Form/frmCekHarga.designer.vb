<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FrmCekHargaJual
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
        Me.txtPath = New DevExpress.XtraEditors.ButtonEdit
        Me.LabelControl1 = New DevExpress.XtraEditors.LabelControl
        Me.LabelControl3 = New DevExpress.XtraEditors.LabelControl
        Me.cmdTutup = New DevExpress.XtraEditors.SimpleButton
        Me.TglDari = New DevExpress.XtraEditors.DateEdit
        Me.LabelControl2 = New DevExpress.XtraEditors.LabelControl
        Me.TglSampai = New DevExpress.XtraEditors.DateEdit
        Me.txtHargaJual = New DevExpress.XtraEditors.TextEdit
        Me.txtDiskon = New DevExpress.XtraEditors.TextEdit
        Me.txtHargaNetto = New DevExpress.XtraEditors.TextEdit
        Me.LabelControl4 = New DevExpress.XtraEditors.LabelControl
        Me.LabelControl5 = New DevExpress.XtraEditors.LabelControl
        Me.LabelControl6 = New DevExpress.XtraEditors.LabelControl
        Me.txtNama = New DevExpress.XtraEditors.ButtonEdit
        Me.LabelControl7 = New DevExpress.XtraEditors.LabelControl
        Me.txtSatuan = New DevExpress.XtraEditors.ButtonEdit
        Me.GridControl1 = New DevExpress.XtraGrid.GridControl
        Me.GridView1 = New DevExpress.XtraGrid.Views.Grid.GridView
        Me.CNoUrut = New DevExpress.XtraGrid.Columns.GridColumn
        Me.CKode = New DevExpress.XtraGrid.Columns.GridColumn
        Me.cBarcode = New DevExpress.XtraGrid.Columns.GridColumn
        Me.CNama = New DevExpress.XtraGrid.Columns.GridColumn
        Me.CHargaJual = New DevExpress.XtraGrid.Columns.GridColumn
        Me.cSatuan = New DevExpress.XtraGrid.Columns.GridColumn
        Me.GridView2 = New DevExpress.XtraGrid.Views.Grid.GridView
        Me.SimpleButton1 = New DevExpress.XtraEditors.SimpleButton
        Me.SimpleButton2 = New DevExpress.XtraEditors.SimpleButton
        CType(Me.txtPath.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.TglDari.Properties.VistaTimeProperties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.TglDari.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.TglSampai.Properties.VistaTimeProperties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.TglSampai.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtHargaJual.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtDiskon.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtHargaNetto.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtNama.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtSatuan.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.GridControl1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.GridView1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.GridView2, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'lbKassa
        '
        Me.lbKassa.Appearance.Font = New System.Drawing.Font("Calibri", 18.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbKassa.Cursor = System.Windows.Forms.Cursors.Hand
        Me.lbKassa.Location = New System.Drawing.Point(9, 149)
        Me.lbKassa.Name = "lbKassa"
        Me.lbKassa.Size = New System.Drawing.Size(184, 29)
        Me.lbKassa.TabIndex = 39
        Me.lbKassa.Text = "Harga Jual Normal"
        '
        'txtPath
        '
        Me.txtPath.Location = New System.Drawing.Point(302, 23)
        Me.txtPath.Name = "txtPath"
        Me.txtPath.Properties.Appearance.Font = New System.Drawing.Font("Calibri", 18.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtPath.Properties.Appearance.Options.UseFont = True
        Me.txtPath.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton})
        Me.txtPath.Size = New System.Drawing.Size(442, 36)
        Me.txtPath.TabIndex = 40
        '
        'LabelControl1
        '
        Me.LabelControl1.Appearance.Font = New System.Drawing.Font("Calibri", 18.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LabelControl1.Cursor = System.Windows.Forms.Cursors.Hand
        Me.LabelControl1.Location = New System.Drawing.Point(9, 26)
        Me.LabelControl1.Name = "LabelControl1"
        Me.LabelControl1.Size = New System.Drawing.Size(253, 29)
        Me.LabelControl1.TabIndex = 41
        Me.LabelControl1.Text = "Masukkan Kode/Barcode"
        '
        'LabelControl3
        '
        Me.LabelControl3.Appearance.Font = New System.Drawing.Font("Calibri", 18.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LabelControl3.Cursor = System.Windows.Forms.Cursors.Hand
        Me.LabelControl3.Location = New System.Drawing.Point(9, 190)
        Me.LabelControl3.Name = "LabelControl3"
        Me.LabelControl3.Size = New System.Drawing.Size(166, 29)
        Me.LabelControl3.TabIndex = 43
        Me.LabelControl3.Text = "Tanggal Promosi"
        '
        'cmdTutup
        '
        Me.cmdTutup.Appearance.Font = New System.Drawing.Font("Calibri", 18.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdTutup.Appearance.ForeColor = System.Drawing.Color.Blue
        Me.cmdTutup.Appearance.Options.UseFont = True
        Me.cmdTutup.Appearance.Options.UseForeColor = True
        Me.cmdTutup.ImageIndex = 11
        Me.cmdTutup.Location = New System.Drawing.Point(625, 608)
        Me.cmdTutup.Name = "cmdTutup"
        Me.cmdTutup.Size = New System.Drawing.Size(119, 52)
        Me.cmdTutup.TabIndex = 47
        Me.cmdTutup.Text = "&Tutup"
        '
        'TglDari
        '
        Me.TglDari.EditValue = Nothing
        Me.TglDari.Location = New System.Drawing.Point(223, 187)
        Me.TglDari.Name = "TglDari"
        Me.TglDari.Properties.Appearance.Font = New System.Drawing.Font("Calibri", 18.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TglDari.Properties.Appearance.Options.UseFont = True
        Me.TglDari.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.TglDari.Properties.Mask.EditMask = "dd-MM-yyyy"
        Me.TglDari.Properties.Mask.UseMaskAsDisplayFormat = True
        Me.TglDari.Properties.ReadOnly = True
        Me.TglDari.Properties.VistaTimeProperties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton})
        Me.TglDari.Size = New System.Drawing.Size(218, 36)
        Me.TglDari.TabIndex = 42
        '
        'LabelControl2
        '
        Me.LabelControl2.Appearance.Font = New System.Drawing.Font("Calibri", 18.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LabelControl2.Location = New System.Drawing.Point(460, 192)
        Me.LabelControl2.Name = "LabelControl2"
        Me.LabelControl2.Size = New System.Drawing.Size(33, 29)
        Me.LabelControl2.TabIndex = 44
        Me.LabelControl2.Text = "s/d"
        '
        'TglSampai
        '
        Me.TglSampai.EditValue = Nothing
        Me.TglSampai.Location = New System.Drawing.Point(522, 185)
        Me.TglSampai.Name = "TglSampai"
        Me.TglSampai.Properties.Appearance.Font = New System.Drawing.Font("Calibri", 18.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TglSampai.Properties.Appearance.Options.UseFont = True
        Me.TglSampai.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.TglSampai.Properties.Mask.EditMask = "dd-MM-yyyy"
        Me.TglSampai.Properties.Mask.UseMaskAsDisplayFormat = True
        Me.TglSampai.Properties.ReadOnly = True
        Me.TglSampai.Properties.VistaTimeProperties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton})
        Me.TglSampai.Size = New System.Drawing.Size(222, 36)
        Me.TglSampai.TabIndex = 45
        '
        'txtHargaJual
        '
        Me.txtHargaJual.EditValue = New Decimal(New Integer() {0, 0, 0, 0})
        Me.txtHargaJual.EnterMoveNextControl = True
        Me.txtHargaJual.Location = New System.Drawing.Point(223, 146)
        Me.txtHargaJual.Name = "txtHargaJual"
        Me.txtHargaJual.Properties.Appearance.Font = New System.Drawing.Font("Calibri", 18.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtHargaJual.Properties.Appearance.Options.UseFont = True
        Me.txtHargaJual.Properties.Mask.EditMask = "n2"
        Me.txtHargaJual.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric
        Me.txtHargaJual.Properties.Mask.UseMaskAsDisplayFormat = True
        Me.txtHargaJual.Properties.ReadOnly = True
        Me.txtHargaJual.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.txtHargaJual.Size = New System.Drawing.Size(270, 36)
        Me.txtHargaJual.TabIndex = 48
        '
        'txtDiskon
        '
        Me.txtDiskon.EditValue = New Decimal(New Integer() {0, 0, 0, 0})
        Me.txtDiskon.EnterMoveNextControl = True
        Me.txtDiskon.Location = New System.Drawing.Point(223, 228)
        Me.txtDiskon.Name = "txtDiskon"
        Me.txtDiskon.Properties.Appearance.Font = New System.Drawing.Font("Calibri", 18.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtDiskon.Properties.Appearance.Options.UseFont = True
        Me.txtDiskon.Properties.Mask.EditMask = "n2"
        Me.txtDiskon.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric
        Me.txtDiskon.Properties.Mask.UseMaskAsDisplayFormat = True
        Me.txtDiskon.Properties.ReadOnly = True
        Me.txtDiskon.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.txtDiskon.Size = New System.Drawing.Size(270, 36)
        Me.txtDiskon.TabIndex = 49
        '
        'txtHargaNetto
        '
        Me.txtHargaNetto.EditValue = New Decimal(New Integer() {0, 0, 0, 0})
        Me.txtHargaNetto.EnterMoveNextControl = True
        Me.txtHargaNetto.Location = New System.Drawing.Point(223, 269)
        Me.txtHargaNetto.Name = "txtHargaNetto"
        Me.txtHargaNetto.Properties.Appearance.Font = New System.Drawing.Font("Calibri", 18.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtHargaNetto.Properties.Appearance.Options.UseFont = True
        Me.txtHargaNetto.Properties.Mask.EditMask = "n2"
        Me.txtHargaNetto.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric
        Me.txtHargaNetto.Properties.Mask.UseMaskAsDisplayFormat = True
        Me.txtHargaNetto.Properties.ReadOnly = True
        Me.txtHargaNetto.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.txtHargaNetto.Size = New System.Drawing.Size(270, 36)
        Me.txtHargaNetto.TabIndex = 50
        '
        'LabelControl4
        '
        Me.LabelControl4.Appearance.Font = New System.Drawing.Font("Calibri", 18.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LabelControl4.Cursor = System.Windows.Forms.Cursors.Hand
        Me.LabelControl4.Location = New System.Drawing.Point(9, 231)
        Me.LabelControl4.Name = "LabelControl4"
        Me.LabelControl4.Size = New System.Drawing.Size(107, 29)
        Me.LabelControl4.TabIndex = 51
        Me.LabelControl4.Text = "Diskon Rp."
        '
        'LabelControl5
        '
        Me.LabelControl5.Appearance.Font = New System.Drawing.Font("Calibri", 18.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LabelControl5.Cursor = System.Windows.Forms.Cursors.Hand
        Me.LabelControl5.Location = New System.Drawing.Point(9, 272)
        Me.LabelControl5.Name = "LabelControl5"
        Me.LabelControl5.Size = New System.Drawing.Size(132, 29)
        Me.LabelControl5.TabIndex = 52
        Me.LabelControl5.Text = "Harga Promo"
        '
        'LabelControl6
        '
        Me.LabelControl6.Appearance.Font = New System.Drawing.Font("Calibri", 18.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LabelControl6.Cursor = System.Windows.Forms.Cursors.Hand
        Me.LabelControl6.Location = New System.Drawing.Point(9, 67)
        Me.LabelControl6.Name = "LabelControl6"
        Me.LabelControl6.Size = New System.Drawing.Size(135, 29)
        Me.LabelControl6.TabIndex = 54
        Me.LabelControl6.Text = "Nama Barang"
        '
        'txtNama
        '
        Me.txtNama.Location = New System.Drawing.Point(221, 64)
        Me.txtNama.Name = "txtNama"
        Me.txtNama.Properties.Appearance.Font = New System.Drawing.Font("Calibri", 18.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtNama.Properties.Appearance.Options.UseFont = True
        Me.txtNama.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton})
        Me.txtNama.Size = New System.Drawing.Size(523, 36)
        Me.txtNama.TabIndex = 53
        '
        'LabelControl7
        '
        Me.LabelControl7.Appearance.Font = New System.Drawing.Font("Calibri", 18.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LabelControl7.Cursor = System.Windows.Forms.Cursors.Hand
        Me.LabelControl7.Location = New System.Drawing.Point(9, 108)
        Me.LabelControl7.Name = "LabelControl7"
        Me.LabelControl7.Size = New System.Drawing.Size(69, 29)
        Me.LabelControl7.TabIndex = 56
        Me.LabelControl7.Text = "Satuan"
        '
        'txtSatuan
        '
        Me.txtSatuan.Location = New System.Drawing.Point(221, 105)
        Me.txtSatuan.Name = "txtSatuan"
        Me.txtSatuan.Properties.Appearance.Font = New System.Drawing.Font("Calibri", 18.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtSatuan.Properties.Appearance.Options.UseFont = True
        Me.txtSatuan.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton})
        Me.txtSatuan.Size = New System.Drawing.Size(523, 36)
        Me.txtSatuan.TabIndex = 55
        '
        'GridControl1
        '
        Me.GridControl1.Location = New System.Drawing.Point(12, 311)
        Me.GridControl1.MainView = Me.GridView1
        Me.GridControl1.Name = "GridControl1"
        Me.GridControl1.Size = New System.Drawing.Size(737, 296)
        Me.GridControl1.TabIndex = 57
        Me.GridControl1.ViewCollection.AddRange(New DevExpress.XtraGrid.Views.Base.BaseView() {Me.GridView1, Me.GridView2})
        '
        'GridView1
        '
        Me.GridView1.Appearance.HeaderPanel.Font = New System.Drawing.Font("Tahoma", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GridView1.Appearance.HeaderPanel.Options.UseFont = True
        Me.GridView1.Appearance.Row.Font = New System.Drawing.Font("Tahoma", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GridView1.Appearance.Row.Options.UseFont = True
        Me.GridView1.Columns.AddRange(New DevExpress.XtraGrid.Columns.GridColumn() {Me.CNoUrut, Me.CKode, Me.cBarcode, Me.CNama, Me.CHargaJual, Me.cSatuan})
        Me.GridView1.GridControl = Me.GridControl1
        Me.GridView1.Name = "GridView1"
        Me.GridView1.OptionsView.ShowGroupPanel = False
        '
        'CNoUrut
        '
        Me.CNoUrut.Caption = "No."
        Me.CNoUrut.DisplayFormat.FormatString = "##0"
        Me.CNoUrut.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
        Me.CNoUrut.FieldName = "NoUrut"
        Me.CNoUrut.Name = "CNoUrut"
        Me.CNoUrut.Visible = True
        Me.CNoUrut.VisibleIndex = 0
        Me.CNoUrut.Width = 67
        '
        'CKode
        '
        Me.CKode.Caption = "Kode"
        Me.CKode.FieldName = "Kode"
        Me.CKode.Name = "CKode"
        Me.CKode.Visible = True
        Me.CKode.VisibleIndex = 1
        Me.CKode.Width = 64
        '
        'cBarcode
        '
        Me.cBarcode.Caption = "Barcode"
        Me.cBarcode.FieldName = "Barcode"
        Me.cBarcode.Name = "cBarcode"
        Me.cBarcode.Visible = True
        Me.cBarcode.VisibleIndex = 2
        Me.cBarcode.Width = 133
        '
        'CNama
        '
        Me.CNama.Caption = "Nama"
        Me.CNama.FieldName = "Nama"
        Me.CNama.Name = "CNama"
        Me.CNama.Visible = True
        Me.CNama.VisibleIndex = 3
        Me.CNama.Width = 246
        '
        'CHargaJual
        '
        Me.CHargaJual.Caption = "Harga Jual"
        Me.CHargaJual.DisplayFormat.FormatString = "n2"
        Me.CHargaJual.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
        Me.CHargaJual.FieldName = "HargaJual"
        Me.CHargaJual.Name = "CHargaJual"
        Me.CHargaJual.Visible = True
        Me.CHargaJual.VisibleIndex = 5
        Me.CHargaJual.Width = 124
        '
        'cSatuan
        '
        Me.cSatuan.Caption = "Satuan"
        Me.cSatuan.FieldName = "Satuan"
        Me.cSatuan.Name = "cSatuan"
        Me.cSatuan.Visible = True
        Me.cSatuan.VisibleIndex = 4
        Me.cSatuan.Width = 85
        '
        'GridView2
        '
        Me.GridView2.GridControl = Me.GridControl1
        Me.GridView2.Name = "GridView2"
        '
        'SimpleButton1
        '
        Me.SimpleButton1.Appearance.Font = New System.Drawing.Font("Calibri", 18.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.SimpleButton1.Appearance.ForeColor = System.Drawing.Color.Blue
        Me.SimpleButton1.Appearance.Options.UseFont = True
        Me.SimpleButton1.Appearance.Options.UseForeColor = True
        Me.SimpleButton1.ImageIndex = 11
        Me.SimpleButton1.Location = New System.Drawing.Point(460, 608)
        Me.SimpleButton1.Name = "SimpleButton1"
        Me.SimpleButton1.Size = New System.Drawing.Size(159, 52)
        Me.SimpleButton1.TabIndex = 58
        Me.SimpleButton1.Text = "&Clear All"
        '
        'SimpleButton2
        '
        Me.SimpleButton2.Appearance.Font = New System.Drawing.Font("Calibri", 18.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.SimpleButton2.Appearance.ForeColor = System.Drawing.Color.Blue
        Me.SimpleButton2.Appearance.Options.UseFont = True
        Me.SimpleButton2.Appearance.Options.UseForeColor = True
        Me.SimpleButton2.ImageIndex = 11
        Me.SimpleButton2.Location = New System.Drawing.Point(302, 608)
        Me.SimpleButton2.Name = "SimpleButton2"
        Me.SimpleButton2.Size = New System.Drawing.Size(151, 52)
        Me.SimpleButton2.TabIndex = 59
        Me.SimpleButton2.Text = "&Delete Item"
        '
        'FrmCekHargaJual
        '
        Me.Appearance.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(128, Byte), Integer))
        Me.Appearance.Options.UseBackColor = True
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(758, 672)
        Me.Controls.Add(Me.SimpleButton2)
        Me.Controls.Add(Me.SimpleButton1)
        Me.Controls.Add(Me.GridControl1)
        Me.Controls.Add(Me.LabelControl7)
        Me.Controls.Add(Me.txtSatuan)
        Me.Controls.Add(Me.LabelControl6)
        Me.Controls.Add(Me.txtNama)
        Me.Controls.Add(Me.LabelControl5)
        Me.Controls.Add(Me.LabelControl4)
        Me.Controls.Add(Me.txtHargaNetto)
        Me.Controls.Add(Me.txtDiskon)
        Me.Controls.Add(Me.txtHargaJual)
        Me.Controls.Add(Me.cmdTutup)
        Me.Controls.Add(Me.TglSampai)
        Me.Controls.Add(Me.LabelControl2)
        Me.Controls.Add(Me.LabelControl3)
        Me.Controls.Add(Me.TglDari)
        Me.Controls.Add(Me.LabelControl1)
        Me.Controls.Add(Me.txtPath)
        Me.Controls.Add(Me.lbKassa)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.Name = "FrmCekHargaJual"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "CEK HARGA JUAL"
        CType(Me.txtPath.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.TglDari.Properties.VistaTimeProperties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.TglDari.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.TglSampai.Properties.VistaTimeProperties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.TglSampai.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtHargaJual.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtDiskon.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtHargaNetto.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtNama.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtSatuan.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.GridControl1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.GridView1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.GridView2, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents lbKassa As DevExpress.XtraEditors.LabelControl
    Friend WithEvents txtPath As DevExpress.XtraEditors.ButtonEdit
    Friend WithEvents LabelControl1 As DevExpress.XtraEditors.LabelControl
    Friend WithEvents LabelControl3 As DevExpress.XtraEditors.LabelControl
    Friend WithEvents cmdTutup As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents TglDari As DevExpress.XtraEditors.DateEdit
    Friend WithEvents LabelControl2 As DevExpress.XtraEditors.LabelControl
    Friend WithEvents TglSampai As DevExpress.XtraEditors.DateEdit
    Friend WithEvents txtHargaJual As DevExpress.XtraEditors.TextEdit
    Friend WithEvents txtDiskon As DevExpress.XtraEditors.TextEdit
    Friend WithEvents txtHargaNetto As DevExpress.XtraEditors.TextEdit
    Friend WithEvents LabelControl4 As DevExpress.XtraEditors.LabelControl
    Friend WithEvents LabelControl5 As DevExpress.XtraEditors.LabelControl
    Friend WithEvents LabelControl6 As DevExpress.XtraEditors.LabelControl
    Friend WithEvents txtNama As DevExpress.XtraEditors.ButtonEdit
    Friend WithEvents LabelControl7 As DevExpress.XtraEditors.LabelControl
    Friend WithEvents txtSatuan As DevExpress.XtraEditors.ButtonEdit
    Friend WithEvents GridControl1 As DevExpress.XtraGrid.GridControl
    Friend WithEvents GridView1 As DevExpress.XtraGrid.Views.Grid.GridView
    Friend WithEvents GridView2 As DevExpress.XtraGrid.Views.Grid.GridView
    Friend WithEvents SimpleButton1 As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents CKode As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents cBarcode As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents CNama As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents CHargaJual As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents cSatuan As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents CNoUrut As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents SimpleButton2 As DevExpress.XtraEditors.SimpleButton
End Class
