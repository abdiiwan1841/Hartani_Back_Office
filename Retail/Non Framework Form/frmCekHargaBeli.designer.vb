<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FrmCekHargaBeli
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
        Me.CTotalBeli = New DevExpress.XtraGrid.Columns.GridColumn
        Me.cHargaBeliRataRata = New DevExpress.XtraGrid.Columns.GridColumn
        Me.GridView2 = New DevExpress.XtraGrid.Views.Grid.GridView
        Me.SimpleButton1 = New DevExpress.XtraEditors.SimpleButton
        Me.SimpleButton2 = New DevExpress.XtraEditors.SimpleButton
        Me.Tgl2 = New DevExpress.XtraEditors.DateEdit
        Me.LabelControl8 = New DevExpress.XtraEditors.LabelControl
        Me.LabelControl9 = New DevExpress.XtraEditors.LabelControl
        Me.Tgl1 = New DevExpress.XtraEditors.DateEdit
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
        CType(Me.Tgl2.Properties.VistaTimeProperties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Tgl2.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Tgl1.Properties.VistaTimeProperties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Tgl1.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'lbKassa
        '
        Me.lbKassa.Appearance.Font = New System.Drawing.Font("Calibri", 18.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbKassa.Cursor = System.Windows.Forms.Cursors.Hand
        Me.lbKassa.Location = New System.Drawing.Point(9, 180)
        Me.lbKassa.Name = "lbKassa"
        Me.lbKassa.Size = New System.Drawing.Size(184, 29)
        Me.lbKassa.TabIndex = 10
        Me.lbKassa.Text = "Harga Jual Normal"
        Me.lbKassa.Visible = False
        '
        'txtPath
        '
        Me.txtPath.Location = New System.Drawing.Point(302, 54)
        Me.txtPath.Name = "txtPath"
        Me.txtPath.Properties.Appearance.Font = New System.Drawing.Font("Calibri", 18.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtPath.Properties.Appearance.Options.UseFont = True
        Me.txtPath.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton})
        Me.txtPath.Size = New System.Drawing.Size(670, 36)
        Me.txtPath.TabIndex = 5
        '
        'LabelControl1
        '
        Me.LabelControl1.Appearance.Font = New System.Drawing.Font("Calibri", 18.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LabelControl1.Cursor = System.Windows.Forms.Cursors.Hand
        Me.LabelControl1.Location = New System.Drawing.Point(9, 57)
        Me.LabelControl1.Name = "LabelControl1"
        Me.LabelControl1.Size = New System.Drawing.Size(253, 29)
        Me.LabelControl1.TabIndex = 4
        Me.LabelControl1.Text = "Masukkan Kode/Barcode"
        '
        'LabelControl3
        '
        Me.LabelControl3.Appearance.Font = New System.Drawing.Font("Calibri", 18.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LabelControl3.Cursor = System.Windows.Forms.Cursors.Hand
        Me.LabelControl3.Location = New System.Drawing.Point(9, 221)
        Me.LabelControl3.Name = "LabelControl3"
        Me.LabelControl3.Size = New System.Drawing.Size(166, 29)
        Me.LabelControl3.TabIndex = 12
        Me.LabelControl3.Text = "Tanggal Promosi"
        Me.LabelControl3.Visible = False
        '
        'cmdTutup
        '
        Me.cmdTutup.Appearance.Font = New System.Drawing.Font("Calibri", 18.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdTutup.Appearance.ForeColor = System.Drawing.Color.Blue
        Me.cmdTutup.Appearance.Options.UseFont = True
        Me.cmdTutup.Appearance.Options.UseForeColor = True
        Me.cmdTutup.ImageIndex = 11
        Me.cmdTutup.Location = New System.Drawing.Point(853, 613)
        Me.cmdTutup.Name = "cmdTutup"
        Me.cmdTutup.Size = New System.Drawing.Size(119, 52)
        Me.cmdTutup.TabIndex = 23
        Me.cmdTutup.Text = "&Tutup"
        '
        'TglDari
        '
        Me.TglDari.EditValue = Nothing
        Me.TglDari.Location = New System.Drawing.Point(223, 218)
        Me.TglDari.Name = "TglDari"
        Me.TglDari.Properties.Appearance.Font = New System.Drawing.Font("Calibri", 18.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TglDari.Properties.Appearance.Options.UseFont = True
        Me.TglDari.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.TglDari.Properties.Mask.EditMask = "dd-MM-yyyy"
        Me.TglDari.Properties.Mask.UseMaskAsDisplayFormat = True
        Me.TglDari.Properties.ReadOnly = True
        Me.TglDari.Properties.VistaTimeProperties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton})
        Me.TglDari.Size = New System.Drawing.Size(218, 36)
        Me.TglDari.TabIndex = 13
        Me.TglDari.Visible = False
        '
        'LabelControl2
        '
        Me.LabelControl2.Appearance.Font = New System.Drawing.Font("Calibri", 18.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LabelControl2.Location = New System.Drawing.Point(460, 223)
        Me.LabelControl2.Name = "LabelControl2"
        Me.LabelControl2.Size = New System.Drawing.Size(33, 29)
        Me.LabelControl2.TabIndex = 14
        Me.LabelControl2.Text = "s/d"
        Me.LabelControl2.Visible = False
        '
        'TglSampai
        '
        Me.TglSampai.EditValue = Nothing
        Me.TglSampai.Location = New System.Drawing.Point(522, 216)
        Me.TglSampai.Name = "TglSampai"
        Me.TglSampai.Properties.Appearance.Font = New System.Drawing.Font("Calibri", 18.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TglSampai.Properties.Appearance.Options.UseFont = True
        Me.TglSampai.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.TglSampai.Properties.Mask.EditMask = "dd-MM-yyyy"
        Me.TglSampai.Properties.Mask.UseMaskAsDisplayFormat = True
        Me.TglSampai.Properties.ReadOnly = True
        Me.TglSampai.Properties.VistaTimeProperties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton})
        Me.TglSampai.Size = New System.Drawing.Size(222, 36)
        Me.TglSampai.TabIndex = 15
        Me.TglSampai.Visible = False
        '
        'txtHargaJual
        '
        Me.txtHargaJual.EditValue = New Decimal(New Integer() {0, 0, 0, 0})
        Me.txtHargaJual.EnterMoveNextControl = True
        Me.txtHargaJual.Location = New System.Drawing.Point(223, 177)
        Me.txtHargaJual.Name = "txtHargaJual"
        Me.txtHargaJual.Properties.Appearance.Font = New System.Drawing.Font("Calibri", 18.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtHargaJual.Properties.Appearance.Options.UseFont = True
        Me.txtHargaJual.Properties.Mask.EditMask = "n2"
        Me.txtHargaJual.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric
        Me.txtHargaJual.Properties.Mask.UseMaskAsDisplayFormat = True
        Me.txtHargaJual.Properties.ReadOnly = True
        Me.txtHargaJual.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.txtHargaJual.Size = New System.Drawing.Size(270, 36)
        Me.txtHargaJual.TabIndex = 11
        Me.txtHargaJual.Visible = False
        '
        'txtDiskon
        '
        Me.txtDiskon.EditValue = New Decimal(New Integer() {0, 0, 0, 0})
        Me.txtDiskon.EnterMoveNextControl = True
        Me.txtDiskon.Location = New System.Drawing.Point(223, 259)
        Me.txtDiskon.Name = "txtDiskon"
        Me.txtDiskon.Properties.Appearance.Font = New System.Drawing.Font("Calibri", 18.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtDiskon.Properties.Appearance.Options.UseFont = True
        Me.txtDiskon.Properties.Mask.EditMask = "n2"
        Me.txtDiskon.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric
        Me.txtDiskon.Properties.Mask.UseMaskAsDisplayFormat = True
        Me.txtDiskon.Properties.ReadOnly = True
        Me.txtDiskon.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.txtDiskon.Size = New System.Drawing.Size(270, 36)
        Me.txtDiskon.TabIndex = 17
        Me.txtDiskon.Visible = False
        '
        'txtHargaNetto
        '
        Me.txtHargaNetto.EditValue = New Decimal(New Integer() {0, 0, 0, 0})
        Me.txtHargaNetto.EnterMoveNextControl = True
        Me.txtHargaNetto.Location = New System.Drawing.Point(223, 300)
        Me.txtHargaNetto.Name = "txtHargaNetto"
        Me.txtHargaNetto.Properties.Appearance.Font = New System.Drawing.Font("Calibri", 18.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtHargaNetto.Properties.Appearance.Options.UseFont = True
        Me.txtHargaNetto.Properties.Mask.EditMask = "n2"
        Me.txtHargaNetto.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric
        Me.txtHargaNetto.Properties.Mask.UseMaskAsDisplayFormat = True
        Me.txtHargaNetto.Properties.ReadOnly = True
        Me.txtHargaNetto.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.txtHargaNetto.Size = New System.Drawing.Size(270, 36)
        Me.txtHargaNetto.TabIndex = 19
        Me.txtHargaNetto.Visible = False
        '
        'LabelControl4
        '
        Me.LabelControl4.Appearance.Font = New System.Drawing.Font("Calibri", 18.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LabelControl4.Cursor = System.Windows.Forms.Cursors.Hand
        Me.LabelControl4.Location = New System.Drawing.Point(9, 262)
        Me.LabelControl4.Name = "LabelControl4"
        Me.LabelControl4.Size = New System.Drawing.Size(107, 29)
        Me.LabelControl4.TabIndex = 16
        Me.LabelControl4.Text = "Diskon Rp."
        Me.LabelControl4.Visible = False
        '
        'LabelControl5
        '
        Me.LabelControl5.Appearance.Font = New System.Drawing.Font("Calibri", 18.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LabelControl5.Cursor = System.Windows.Forms.Cursors.Hand
        Me.LabelControl5.Location = New System.Drawing.Point(9, 303)
        Me.LabelControl5.Name = "LabelControl5"
        Me.LabelControl5.Size = New System.Drawing.Size(132, 29)
        Me.LabelControl5.TabIndex = 18
        Me.LabelControl5.Text = "Harga Promo"
        Me.LabelControl5.Visible = False
        '
        'LabelControl6
        '
        Me.LabelControl6.Appearance.Font = New System.Drawing.Font("Calibri", 18.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LabelControl6.Cursor = System.Windows.Forms.Cursors.Hand
        Me.LabelControl6.Location = New System.Drawing.Point(9, 98)
        Me.LabelControl6.Name = "LabelControl6"
        Me.LabelControl6.Size = New System.Drawing.Size(135, 29)
        Me.LabelControl6.TabIndex = 6
        Me.LabelControl6.Text = "Nama Barang"
        '
        'txtNama
        '
        Me.txtNama.Location = New System.Drawing.Point(221, 95)
        Me.txtNama.Name = "txtNama"
        Me.txtNama.Properties.Appearance.Font = New System.Drawing.Font("Calibri", 18.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtNama.Properties.Appearance.Options.UseFont = True
        Me.txtNama.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton})
        Me.txtNama.Size = New System.Drawing.Size(751, 36)
        Me.txtNama.TabIndex = 7
        '
        'LabelControl7
        '
        Me.LabelControl7.Appearance.Font = New System.Drawing.Font("Calibri", 18.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LabelControl7.Cursor = System.Windows.Forms.Cursors.Hand
        Me.LabelControl7.Location = New System.Drawing.Point(9, 139)
        Me.LabelControl7.Name = "LabelControl7"
        Me.LabelControl7.Size = New System.Drawing.Size(69, 29)
        Me.LabelControl7.TabIndex = 20
        Me.LabelControl7.Text = "Satuan"
        Me.LabelControl7.Visible = False
        '
        'txtSatuan
        '
        Me.txtSatuan.Location = New System.Drawing.Point(221, 136)
        Me.txtSatuan.Name = "txtSatuan"
        Me.txtSatuan.Properties.Appearance.Font = New System.Drawing.Font("Calibri", 18.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtSatuan.Properties.Appearance.Options.UseFont = True
        Me.txtSatuan.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton})
        Me.txtSatuan.Size = New System.Drawing.Size(523, 36)
        Me.txtSatuan.TabIndex = 9
        Me.txtSatuan.Visible = False
        '
        'GridControl1
        '
        Me.GridControl1.Location = New System.Drawing.Point(9, 139)
        Me.GridControl1.MainView = Me.GridView1
        Me.GridControl1.Name = "GridControl1"
        Me.GridControl1.Size = New System.Drawing.Size(963, 468)
        Me.GridControl1.TabIndex = 8
        Me.GridControl1.ViewCollection.AddRange(New DevExpress.XtraGrid.Views.Base.BaseView() {Me.GridView1, Me.GridView2})
        '
        'GridView1
        '
        Me.GridView1.Appearance.HeaderPanel.Font = New System.Drawing.Font("Tahoma", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GridView1.Appearance.HeaderPanel.Options.UseFont = True
        Me.GridView1.Appearance.Row.Font = New System.Drawing.Font("Tahoma", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GridView1.Appearance.Row.Options.UseFont = True
        Me.GridView1.Columns.AddRange(New DevExpress.XtraGrid.Columns.GridColumn() {Me.CNoUrut, Me.CKode, Me.cBarcode, Me.CNama, Me.CTotalBeli, Me.cHargaBeliRataRata})
        Me.GridView1.GridControl = Me.GridControl1
        Me.GridView1.Name = "GridView1"
        Me.GridView1.OptionsBehavior.Editable = False
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
        'CTotalBeli
        '
        Me.CTotalBeli.AppearanceCell.Options.UseTextOptions = True
        Me.CTotalBeli.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
        Me.CTotalBeli.AppearanceHeader.Options.UseTextOptions = True
        Me.CTotalBeli.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
        Me.CTotalBeli.Caption = "Total Beli"
        Me.CTotalBeli.DisplayFormat.FormatString = "n2"
        Me.CTotalBeli.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
        Me.CTotalBeli.FieldName = "TotalBeli"
        Me.CTotalBeli.Name = "CTotalBeli"
        Me.CTotalBeli.Visible = True
        Me.CTotalBeli.VisibleIndex = 4
        Me.CTotalBeli.Width = 124
        '
        'cHargaBeliRataRata
        '
        Me.cHargaBeliRataRata.AppearanceCell.Options.UseTextOptions = True
        Me.cHargaBeliRataRata.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
        Me.cHargaBeliRataRata.AppearanceHeader.Options.UseTextOptions = True
        Me.cHargaBeliRataRata.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
        Me.cHargaBeliRataRata.Caption = "Harga Beli Rata-Rata"
        Me.cHargaBeliRataRata.DisplayFormat.FormatString = "n2"
        Me.cHargaBeliRataRata.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
        Me.cHargaBeliRataRata.FieldName = "AvgHarga"
        Me.cHargaBeliRataRata.Name = "cHargaBeliRataRata"
        Me.cHargaBeliRataRata.Visible = True
        Me.cHargaBeliRataRata.VisibleIndex = 5
        Me.cHargaBeliRataRata.Width = 184
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
        Me.SimpleButton1.Location = New System.Drawing.Point(688, 613)
        Me.SimpleButton1.Name = "SimpleButton1"
        Me.SimpleButton1.Size = New System.Drawing.Size(159, 52)
        Me.SimpleButton1.TabIndex = 22
        Me.SimpleButton1.Text = "&Clear All"
        '
        'SimpleButton2
        '
        Me.SimpleButton2.Appearance.Font = New System.Drawing.Font("Calibri", 18.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.SimpleButton2.Appearance.ForeColor = System.Drawing.Color.Blue
        Me.SimpleButton2.Appearance.Options.UseFont = True
        Me.SimpleButton2.Appearance.Options.UseForeColor = True
        Me.SimpleButton2.ImageIndex = 11
        Me.SimpleButton2.Location = New System.Drawing.Point(530, 613)
        Me.SimpleButton2.Name = "SimpleButton2"
        Me.SimpleButton2.Size = New System.Drawing.Size(151, 52)
        Me.SimpleButton2.TabIndex = 21
        Me.SimpleButton2.Text = "&Delete Item"
        '
        'Tgl2
        '
        Me.Tgl2.EditValue = Nothing
        Me.Tgl2.Location = New System.Drawing.Point(522, 12)
        Me.Tgl2.Name = "Tgl2"
        Me.Tgl2.Properties.Appearance.Font = New System.Drawing.Font("Calibri", 18.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Tgl2.Properties.Appearance.Options.UseFont = True
        Me.Tgl2.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.Tgl2.Properties.Mask.EditMask = "dd-MM-yyyy"
        Me.Tgl2.Properties.Mask.UseMaskAsDisplayFormat = True
        Me.Tgl2.Properties.VistaTimeProperties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton})
        Me.Tgl2.Size = New System.Drawing.Size(222, 36)
        Me.Tgl2.TabIndex = 3
        '
        'LabelControl8
        '
        Me.LabelControl8.Appearance.Font = New System.Drawing.Font("Calibri", 18.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LabelControl8.Location = New System.Drawing.Point(458, 17)
        Me.LabelControl8.Name = "LabelControl8"
        Me.LabelControl8.Size = New System.Drawing.Size(33, 29)
        Me.LabelControl8.TabIndex = 2
        Me.LabelControl8.Text = "s/d"
        '
        'LabelControl9
        '
        Me.LabelControl9.Appearance.Font = New System.Drawing.Font("Calibri", 18.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LabelControl9.Cursor = System.Windows.Forms.Cursors.Hand
        Me.LabelControl9.Location = New System.Drawing.Point(7, 15)
        Me.LabelControl9.Name = "LabelControl9"
        Me.LabelControl9.Size = New System.Drawing.Size(121, 29)
        Me.LabelControl9.TabIndex = 0
        Me.LabelControl9.Text = "Tanggal BPB"
        '
        'Tgl1
        '
        Me.Tgl1.EditValue = Nothing
        Me.Tgl1.Location = New System.Drawing.Point(221, 12)
        Me.Tgl1.Name = "Tgl1"
        Me.Tgl1.Properties.Appearance.Font = New System.Drawing.Font("Calibri", 18.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Tgl1.Properties.Appearance.Options.UseFont = True
        Me.Tgl1.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.Tgl1.Properties.Mask.EditMask = "dd-MM-yyyy"
        Me.Tgl1.Properties.Mask.UseMaskAsDisplayFormat = True
        Me.Tgl1.Properties.VistaTimeProperties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton})
        Me.Tgl1.Size = New System.Drawing.Size(218, 36)
        Me.Tgl1.TabIndex = 1
        '
        'FrmCekHargaBeli
        '
        Me.Appearance.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(128, Byte), Integer))
        Me.Appearance.Options.UseBackColor = True
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(984, 672)
        Me.Controls.Add(Me.Tgl2)
        Me.Controls.Add(Me.LabelControl8)
        Me.Controls.Add(Me.LabelControl9)
        Me.Controls.Add(Me.Tgl1)
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
        Me.Name = "FrmCekHargaBeli"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "CEK HARGA BELI"
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
        CType(Me.Tgl2.Properties.VistaTimeProperties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Tgl2.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Tgl1.Properties.VistaTimeProperties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Tgl1.Properties, System.ComponentModel.ISupportInitialize).EndInit()
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
    Friend WithEvents CTotalBeli As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents cHargaBeliRataRata As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents CNoUrut As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents SimpleButton2 As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents Tgl2 As DevExpress.XtraEditors.DateEdit
    Friend WithEvents LabelControl8 As DevExpress.XtraEditors.LabelControl
    Friend WithEvents LabelControl9 As DevExpress.XtraEditors.LabelControl
    Friend WithEvents Tgl1 As DevExpress.XtraEditors.DateEdit
End Class
