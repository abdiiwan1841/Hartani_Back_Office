<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmDaftarPiutangPerCustomer
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
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmDaftarPiutangPerCustomer))
        Me.PanelControl1 = New DevExpress.XtraEditors.PanelControl
        Me.chkLunas = New DevExpress.XtraEditors.CheckEdit
        Me.LabelControl6 = New DevExpress.XtraEditors.LabelControl
        Me.ButtonEdit1 = New DevExpress.XtraEditors.ButtonEdit
        Me.LabelControl5 = New DevExpress.XtraEditors.LabelControl
        Me.LabelControl4 = New DevExpress.XtraEditors.LabelControl
        Me.LabelControl3 = New DevExpress.XtraEditors.LabelControl
        Me.TglSampai = New DevExpress.XtraEditors.DateEdit
        Me.tglDari = New DevExpress.XtraEditors.DateEdit
        Me.LabelControl2 = New DevExpress.XtraEditors.LabelControl
        Me.txtKasBank = New DevExpress.XtraEditors.ButtonEdit
        Me.LabelControl1 = New DevExpress.XtraEditors.LabelControl
        Me.txtAlamat = New DevExpress.XtraEditors.ButtonEdit
        Me.PanelControl2 = New DevExpress.XtraEditors.PanelControl
        Me.PanelControl3 = New DevExpress.XtraEditors.PanelControl
        Me.cmdExport = New DevExpress.XtraEditors.SimpleButton
        Me.ImageList1 = New System.Windows.Forms.ImageList(Me.components)
        Me.cmdRefresh = New DevExpress.XtraEditors.SimpleButton
        Me.cmdPrint = New DevExpress.XtraEditors.SimpleButton
        Me.cmdClose = New DevExpress.XtraEditors.SimpleButton
        Me.PanelControl4 = New DevExpress.XtraEditors.PanelControl
        Me.GridControl1 = New DevExpress.XtraGrid.GridControl
        Me.GridView1 = New DevExpress.XtraGrid.Views.Grid.GridView
        Me.CCustomer = New DevExpress.XtraGrid.Columns.GridColumn
        Me.cTelp = New DevExpress.XtraGrid.Columns.GridColumn
        Me.cKontak = New DevExpress.XtraGrid.Columns.GridColumn
        Me.cAlamat = New DevExpress.XtraGrid.Columns.GridColumn
        Me.cJumlahDiskon = New DevExpress.XtraGrid.Columns.GridColumn
        Me.cJumlahBayar = New DevExpress.XtraGrid.Columns.GridColumn
        Me.cTotal = New DevExpress.XtraGrid.Columns.GridColumn
        Me.RepositoryItemCheckEdit1 = New DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit
        CType(Me.PanelControl1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.PanelControl1.SuspendLayout()
        CType(Me.chkLunas.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.ButtonEdit1.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.TglSampai.Properties.VistaTimeProperties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.TglSampai.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.tglDari.Properties.VistaTimeProperties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.tglDari.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtKasBank.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtAlamat.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PanelControl2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PanelControl3, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.PanelControl3.SuspendLayout()
        CType(Me.PanelControl4, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.PanelControl4.SuspendLayout()
        CType(Me.GridControl1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.GridView1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RepositoryItemCheckEdit1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'PanelControl1
        '
        Me.PanelControl1.Controls.Add(Me.chkLunas)
        Me.PanelControl1.Controls.Add(Me.LabelControl6)
        Me.PanelControl1.Controls.Add(Me.ButtonEdit1)
        Me.PanelControl1.Controls.Add(Me.LabelControl5)
        Me.PanelControl1.Controls.Add(Me.LabelControl4)
        Me.PanelControl1.Controls.Add(Me.LabelControl3)
        Me.PanelControl1.Controls.Add(Me.TglSampai)
        Me.PanelControl1.Controls.Add(Me.tglDari)
        Me.PanelControl1.Controls.Add(Me.LabelControl2)
        Me.PanelControl1.Controls.Add(Me.txtKasBank)
        Me.PanelControl1.Controls.Add(Me.LabelControl1)
        Me.PanelControl1.Controls.Add(Me.txtAlamat)
        Me.PanelControl1.Dock = System.Windows.Forms.DockStyle.Top
        Me.PanelControl1.Location = New System.Drawing.Point(0, 0)
        Me.PanelControl1.Name = "PanelControl1"
        Me.PanelControl1.Size = New System.Drawing.Size(1028, 58)
        Me.PanelControl1.TabIndex = 0
        '
        'chkLunas
        '
        Me.chkLunas.EditValue = True
        Me.chkLunas.Location = New System.Drawing.Point(821, 9)
        Me.chkLunas.Name = "chkLunas"
        Me.chkLunas.Properties.Caption = "Hanya yang belum Lunas"
        Me.chkLunas.Size = New System.Drawing.Size(201, 19)
        Me.chkLunas.TabIndex = 11
        '
        'LabelControl6
        '
        Me.LabelControl6.Appearance.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LabelControl6.Location = New System.Drawing.Point(579, 33)
        Me.LabelControl6.Name = "LabelControl6"
        Me.LabelControl6.Size = New System.Drawing.Size(40, 16)
        Me.LabelControl6.TabIndex = 10
        Me.LabelControl6.Text = "Barang"
        Me.LabelControl6.Visible = False
        '
        'ButtonEdit1
        '
        Me.ButtonEdit1.Location = New System.Drawing.Point(637, 30)
        Me.ButtonEdit1.Name = "ButtonEdit1"
        Me.ButtonEdit1.Properties.Appearance.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonEdit1.Properties.Appearance.Options.UseFont = True
        Me.ButtonEdit1.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton, New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Close)})
        Me.ButtonEdit1.Size = New System.Drawing.Size(149, 22)
        Me.ButtonEdit1.TabIndex = 9
        Me.ButtonEdit1.Visible = False
        '
        'LabelControl5
        '
        Me.LabelControl5.Appearance.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LabelControl5.Location = New System.Drawing.Point(12, 12)
        Me.LabelControl5.Name = "LabelControl5"
        Me.LabelControl5.Size = New System.Drawing.Size(187, 16)
        Me.LabelControl5.TabIndex = 8
        Me.LabelControl5.Text = "Daftar Piutang Per Customer"
        '
        'LabelControl4
        '
        Me.LabelControl4.Appearance.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LabelControl4.Location = New System.Drawing.Point(420, 31)
        Me.LabelControl4.Name = "LabelControl4"
        Me.LabelControl4.Size = New System.Drawing.Size(18, 16)
        Me.LabelControl4.TabIndex = 7
        Me.LabelControl4.Text = "s/d"
        '
        'LabelControl3
        '
        Me.LabelControl3.Appearance.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LabelControl3.Cursor = System.Windows.Forms.Cursors.Hand
        Me.LabelControl3.Location = New System.Drawing.Point(249, 32)
        Me.LabelControl3.Name = "LabelControl3"
        Me.LabelControl3.Size = New System.Drawing.Size(46, 16)
        Me.LabelControl3.TabIndex = 6
        Me.LabelControl3.Text = "Tanggal"
        '
        'TglSampai
        '
        Me.TglSampai.EditValue = Nothing
        Me.TglSampai.Location = New System.Drawing.Point(444, 28)
        Me.TglSampai.Name = "TglSampai"
        Me.TglSampai.Properties.Appearance.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TglSampai.Properties.Appearance.Options.UseFont = True
        Me.TglSampai.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.TglSampai.Properties.Mask.EditMask = "dd/MM/yyyy"
        Me.TglSampai.Properties.Mask.UseMaskAsDisplayFormat = True
        Me.TglSampai.Properties.VistaTimeProperties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton})
        Me.TglSampai.Size = New System.Drawing.Size(116, 22)
        Me.TglSampai.TabIndex = 5
        '
        'tglDari
        '
        Me.tglDari.EditValue = Nothing
        Me.tglDari.Location = New System.Drawing.Point(305, 28)
        Me.tglDari.Name = "tglDari"
        Me.tglDari.Properties.Appearance.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tglDari.Properties.Appearance.Options.UseFont = True
        Me.tglDari.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.tglDari.Properties.Mask.EditMask = "dd/MM/yyyy"
        Me.tglDari.Properties.Mask.UseMaskAsDisplayFormat = True
        Me.tglDari.Properties.VistaTimeProperties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton})
        Me.tglDari.Size = New System.Drawing.Size(109, 22)
        Me.tglDari.TabIndex = 4
        '
        'LabelControl2
        '
        Me.LabelControl2.Appearance.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LabelControl2.Location = New System.Drawing.Point(579, 8)
        Me.LabelControl2.Name = "LabelControl2"
        Me.LabelControl2.Size = New System.Drawing.Size(51, 16)
        Me.LabelControl2.TabIndex = 3
        Me.LabelControl2.Text = "Operator"
        Me.LabelControl2.Visible = False
        '
        'txtKasBank
        '
        Me.txtKasBank.Location = New System.Drawing.Point(637, 5)
        Me.txtKasBank.Name = "txtKasBank"
        Me.txtKasBank.Properties.Appearance.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtKasBank.Properties.Appearance.Options.UseFont = True
        Me.txtKasBank.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton, New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Close)})
        Me.txtKasBank.Size = New System.Drawing.Size(149, 22)
        Me.txtKasBank.TabIndex = 2
        Me.txtKasBank.Visible = False
        '
        'LabelControl1
        '
        Me.LabelControl1.Appearance.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LabelControl1.Location = New System.Drawing.Point(249, 8)
        Me.LabelControl1.Name = "LabelControl1"
        Me.LabelControl1.Size = New System.Drawing.Size(55, 16)
        Me.LabelControl1.TabIndex = 1
        Me.LabelControl1.Text = "Customer"
        '
        'txtAlamat
        '
        Me.txtAlamat.Location = New System.Drawing.Point(305, 5)
        Me.txtAlamat.Name = "txtAlamat"
        Me.txtAlamat.Properties.Appearance.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtAlamat.Properties.Appearance.Options.UseFont = True
        Me.txtAlamat.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton, New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Close)})
        Me.txtAlamat.Size = New System.Drawing.Size(255, 22)
        Me.txtAlamat.TabIndex = 0
        '
        'PanelControl2
        '
        Me.PanelControl2.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.PanelControl2.Location = New System.Drawing.Point(0, 565)
        Me.PanelControl2.Name = "PanelControl2"
        Me.PanelControl2.Size = New System.Drawing.Size(1028, 64)
        Me.PanelControl2.TabIndex = 1
        Me.PanelControl2.Visible = False
        '
        'PanelControl3
        '
        Me.PanelControl3.Controls.Add(Me.cmdExport)
        Me.PanelControl3.Controls.Add(Me.cmdRefresh)
        Me.PanelControl3.Controls.Add(Me.cmdPrint)
        Me.PanelControl3.Controls.Add(Me.cmdClose)
        Me.PanelControl3.Dock = System.Windows.Forms.DockStyle.Left
        Me.PanelControl3.Location = New System.Drawing.Point(0, 58)
        Me.PanelControl3.Name = "PanelControl3"
        Me.PanelControl3.Size = New System.Drawing.Size(133, 507)
        Me.PanelControl3.TabIndex = 2
        '
        'cmdExport
        '
        Me.cmdExport.Appearance.BackColor = System.Drawing.Color.Snow
        Me.cmdExport.Appearance.BackColor2 = System.Drawing.Color.PowderBlue
        Me.cmdExport.Appearance.BorderColor = System.Drawing.SystemColors.ButtonFace
        Me.cmdExport.Appearance.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdExport.Appearance.ForeColor = System.Drawing.Color.Black
        Me.cmdExport.Appearance.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.ForwardDiagonal
        Me.cmdExport.Appearance.Options.UseBackColor = True
        Me.cmdExport.Appearance.Options.UseBorderColor = True
        Me.cmdExport.Appearance.Options.UseFont = True
        Me.cmdExport.Appearance.Options.UseForeColor = True
        Me.cmdExport.ButtonStyle = DevExpress.XtraEditors.Controls.BorderStyles.HotFlat
        Me.cmdExport.Cursor = System.Windows.Forms.Cursors.Hand
        Me.cmdExport.ImageIndex = 14
        Me.cmdExport.ImageList = Me.ImageList1
        Me.cmdExport.Location = New System.Drawing.Point(12, 384)
        Me.cmdExport.Name = "cmdExport"
        Me.cmdExport.Size = New System.Drawing.Size(115, 32)
        Me.cmdExport.TabIndex = 24
        Me.cmdExport.Text = "&Export Excel"
        '
        'ImageList1
        '
        Me.ImageList1.ImageStream = CType(resources.GetObject("ImageList1.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.ImageList1.TransparentColor = System.Drawing.Color.Transparent
        Me.ImageList1.Images.SetKeyName(0, "applications.gif")
        Me.ImageList1.Images.SetKeyName(1, "bookmark.gif")
        Me.ImageList1.Images.SetKeyName(2, "save.gif")
        Me.ImageList1.Images.SetKeyName(3, "search.gif")
        Me.ImageList1.Images.SetKeyName(4, "NetByte Design Studio - 1288.png")
        Me.ImageList1.Images.SetKeyName(5, "NetByte Design Studio - 1240.png")
        Me.ImageList1.Images.SetKeyName(6, "NetByte Design Studio - 1247.png")
        Me.ImageList1.Images.SetKeyName(7, "NetByte Design Studio - 1248.png")
        Me.ImageList1.Images.SetKeyName(8, "NetByte Design Studio - 1256.png")
        Me.ImageList1.Images.SetKeyName(9, "NetByte Design Studio - 1274.png")
        Me.ImageList1.Images.SetKeyName(10, "NetByte Design Studio - 1287.png")
        Me.ImageList1.Images.SetKeyName(11, "NetByte Design Studio - 0920.png")
        Me.ImageList1.Images.SetKeyName(12, "NetByte Design Studio - 0915.png")
        Me.ImageList1.Images.SetKeyName(13, "NetByte Design Studio - 0952.png")
        Me.ImageList1.Images.SetKeyName(14, "Microsoft-Excel-n-icon.png")
        '
        'cmdRefresh
        '
        Me.cmdRefresh.Appearance.BackColor = System.Drawing.Color.Snow
        Me.cmdRefresh.Appearance.BackColor2 = System.Drawing.Color.PowderBlue
        Me.cmdRefresh.Appearance.BorderColor = System.Drawing.SystemColors.ButtonFace
        Me.cmdRefresh.Appearance.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdRefresh.Appearance.ForeColor = System.Drawing.Color.Black
        Me.cmdRefresh.Appearance.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.ForwardDiagonal
        Me.cmdRefresh.Appearance.Options.UseBackColor = True
        Me.cmdRefresh.Appearance.Options.UseBorderColor = True
        Me.cmdRefresh.Appearance.Options.UseFont = True
        Me.cmdRefresh.Appearance.Options.UseForeColor = True
        Me.cmdRefresh.ButtonStyle = DevExpress.XtraEditors.Controls.BorderStyles.HotFlat
        Me.cmdRefresh.Cursor = System.Windows.Forms.Cursors.Hand
        Me.cmdRefresh.ImageIndex = 13
        Me.cmdRefresh.ImageList = Me.ImageList1
        Me.cmdRefresh.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleLeft
        Me.cmdRefresh.Location = New System.Drawing.Point(12, 20)
        Me.cmdRefresh.Name = "cmdRefresh"
        Me.cmdRefresh.Size = New System.Drawing.Size(115, 34)
        Me.cmdRefresh.TabIndex = 23
        Me.cmdRefresh.Text = "&Refresh"
        '
        'cmdPrint
        '
        Me.cmdPrint.Appearance.BackColor = System.Drawing.Color.Snow
        Me.cmdPrint.Appearance.BackColor2 = System.Drawing.Color.PowderBlue
        Me.cmdPrint.Appearance.BorderColor = System.Drawing.SystemColors.ButtonFace
        Me.cmdPrint.Appearance.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdPrint.Appearance.ForeColor = System.Drawing.Color.Black
        Me.cmdPrint.Appearance.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.ForwardDiagonal
        Me.cmdPrint.Appearance.Options.UseBackColor = True
        Me.cmdPrint.Appearance.Options.UseBorderColor = True
        Me.cmdPrint.Appearance.Options.UseFont = True
        Me.cmdPrint.Appearance.Options.UseForeColor = True
        Me.cmdPrint.ButtonStyle = DevExpress.XtraEditors.Controls.BorderStyles.HotFlat
        Me.cmdPrint.Cursor = System.Windows.Forms.Cursors.Hand
        Me.cmdPrint.ImageIndex = 11
        Me.cmdPrint.ImageList = Me.ImageList1
        Me.cmdPrint.Location = New System.Drawing.Point(12, 344)
        Me.cmdPrint.Name = "cmdPrint"
        Me.cmdPrint.Size = New System.Drawing.Size(115, 34)
        Me.cmdPrint.TabIndex = 22
        Me.cmdPrint.Text = "&Print"
        '
        'cmdClose
        '
        Me.cmdClose.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.cmdClose.Appearance.BackColor = System.Drawing.Color.Snow
        Me.cmdClose.Appearance.BackColor2 = System.Drawing.Color.PowderBlue
        Me.cmdClose.Appearance.BorderColor = System.Drawing.SystemColors.ButtonFace
        Me.cmdClose.Appearance.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdClose.Appearance.ForeColor = System.Drawing.Color.Black
        Me.cmdClose.Appearance.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.ForwardDiagonal
        Me.cmdClose.Appearance.Options.UseBackColor = True
        Me.cmdClose.Appearance.Options.UseBorderColor = True
        Me.cmdClose.Appearance.Options.UseFont = True
        Me.cmdClose.Appearance.Options.UseForeColor = True
        Me.cmdClose.ButtonStyle = DevExpress.XtraEditors.Controls.BorderStyles.HotFlat
        Me.cmdClose.Cursor = System.Windows.Forms.Cursors.Hand
        Me.cmdClose.ImageIndex = 1
        Me.cmdClose.ImageList = Me.ImageList1
        Me.cmdClose.Location = New System.Drawing.Point(12, 463)
        Me.cmdClose.Name = "cmdClose"
        Me.cmdClose.Size = New System.Drawing.Size(115, 34)
        Me.cmdClose.TabIndex = 17
        Me.cmdClose.Text = "&Tutup"
        '
        'PanelControl4
        '
        Me.PanelControl4.Controls.Add(Me.GridControl1)
        Me.PanelControl4.Dock = System.Windows.Forms.DockStyle.Fill
        Me.PanelControl4.Location = New System.Drawing.Point(133, 58)
        Me.PanelControl4.Name = "PanelControl4"
        Me.PanelControl4.Size = New System.Drawing.Size(895, 507)
        Me.PanelControl4.TabIndex = 3
        '
        'GridControl1
        '
        Me.GridControl1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.GridControl1.Location = New System.Drawing.Point(2, 2)
        Me.GridControl1.MainView = Me.GridView1
        Me.GridControl1.Name = "GridControl1"
        Me.GridControl1.RepositoryItems.AddRange(New DevExpress.XtraEditors.Repository.RepositoryItem() {Me.RepositoryItemCheckEdit1})
        Me.GridControl1.Size = New System.Drawing.Size(891, 503)
        Me.GridControl1.TabIndex = 0
        Me.GridControl1.ViewCollection.AddRange(New DevExpress.XtraGrid.Views.Base.BaseView() {Me.GridView1})
        '
        'GridView1
        '
        Me.GridView1.Columns.AddRange(New DevExpress.XtraGrid.Columns.GridColumn() {Me.CCustomer, Me.cTelp, Me.cKontak, Me.cAlamat, Me.cJumlahDiskon, Me.cJumlahBayar, Me.cTotal})
        Me.GridView1.CustomizationFormBounds = New System.Drawing.Rectangle(756, 391, 208, 170)
        Me.GridView1.GridControl = Me.GridControl1
        Me.GridView1.Name = "GridView1"
        Me.GridView1.OptionsBehavior.Editable = False
        Me.GridView1.OptionsSelection.MultiSelect = True
        Me.GridView1.OptionsView.ColumnAutoWidth = False
        Me.GridView1.OptionsView.ShowFooter = True
        Me.GridView1.OptionsView.ShowGroupPanel = False
        '
        'CCustomer
        '
        Me.CCustomer.Caption = "Customer"
        Me.CCustomer.FieldName = "Customer"
        Me.CCustomer.Name = "CCustomer"
        Me.CCustomer.SummaryItem.DisplayFormat = "{0:###,##0.00}"
        Me.CCustomer.SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Count
        Me.CCustomer.Visible = True
        Me.CCustomer.VisibleIndex = 0
        '
        'cTelp
        '
        Me.cTelp.Caption = "Telp"
        Me.cTelp.FieldName = "Telp"
        Me.cTelp.Name = "cTelp"
        Me.cTelp.Visible = True
        Me.cTelp.VisibleIndex = 1
        Me.cTelp.Width = 116
        '
        'cKontak
        '
        Me.cKontak.Caption = "Kontak"
        Me.cKontak.FieldName = "Kontak"
        Me.cKontak.Name = "cKontak"
        Me.cKontak.Visible = True
        Me.cKontak.VisibleIndex = 2
        '
        'cAlamat
        '
        Me.cAlamat.Caption = "Alamat"
        Me.cAlamat.FieldName = "Alamat"
        Me.cAlamat.Name = "cAlamat"
        Me.cAlamat.Visible = True
        Me.cAlamat.VisibleIndex = 3
        Me.cAlamat.Width = 136
        '
        'cJumlahDiskon
        '
        Me.cJumlahDiskon.Caption = "Belum JT (<30 hari)"
        Me.cJumlahDiskon.DisplayFormat.FormatString = "###,##0.00"
        Me.cJumlahDiskon.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
        Me.cJumlahDiskon.FieldName = "BelumJT"
        Me.cJumlahDiskon.Name = "cJumlahDiskon"
        Me.cJumlahDiskon.SummaryItem.DisplayFormat = "{0:###,##0.00}"
        Me.cJumlahDiskon.SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum
        Me.cJumlahDiskon.Visible = True
        Me.cJumlahDiskon.VisibleIndex = 4
        Me.cJumlahDiskon.Width = 181
        '
        'cJumlahBayar
        '
        Me.cJumlahBayar.Caption = "Sudah JT (>=30 hari)"
        Me.cJumlahBayar.DisplayFormat.FormatString = "###,##0.00"
        Me.cJumlahBayar.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
        Me.cJumlahBayar.FieldName = "SudahJT"
        Me.cJumlahBayar.Name = "cJumlahBayar"
        Me.cJumlahBayar.SummaryItem.DisplayFormat = "{0:###,##0.00}"
        Me.cJumlahBayar.SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum
        Me.cJumlahBayar.Visible = True
        Me.cJumlahBayar.VisibleIndex = 5
        Me.cJumlahBayar.Width = 172
        '
        'cTotal
        '
        Me.cTotal.Caption = "Total"
        Me.cTotal.DisplayFormat.FormatString = "###,##0.00"
        Me.cTotal.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
        Me.cTotal.FieldName = "Total"
        Me.cTotal.Name = "cTotal"
        Me.cTotal.SummaryItem.DisplayFormat = "{0:###,##0.00}"
        Me.cTotal.SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum
        Me.cTotal.Visible = True
        Me.cTotal.VisibleIndex = 6
        Me.cTotal.Width = 220
        '
        'RepositoryItemCheckEdit1
        '
        Me.RepositoryItemCheckEdit1.Appearance.Options.UseTextOptions = True
        Me.RepositoryItemCheckEdit1.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.RepositoryItemCheckEdit1.AutoHeight = False
        Me.RepositoryItemCheckEdit1.Name = "RepositoryItemCheckEdit1"
        Me.RepositoryItemCheckEdit1.NullStyle = DevExpress.XtraEditors.Controls.StyleIndeterminate.Unchecked
        '
        'frmDaftarPiutangPerCustomer
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1028, 629)
        Me.Controls.Add(Me.PanelControl4)
        Me.Controls.Add(Me.PanelControl3)
        Me.Controls.Add(Me.PanelControl2)
        Me.Controls.Add(Me.PanelControl1)
        Me.Name = "frmDaftarPiutangPerCustomer"
        Me.Text = "Daftar Piutang Per Customer"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        CType(Me.PanelControl1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.PanelControl1.ResumeLayout(False)
        Me.PanelControl1.PerformLayout()
        CType(Me.chkLunas.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.ButtonEdit1.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.TglSampai.Properties.VistaTimeProperties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.TglSampai.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.tglDari.Properties.VistaTimeProperties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.tglDari.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtKasBank.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtAlamat.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PanelControl2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PanelControl3, System.ComponentModel.ISupportInitialize).EndInit()
        Me.PanelControl3.ResumeLayout(False)
        CType(Me.PanelControl4, System.ComponentModel.ISupportInitialize).EndInit()
        Me.PanelControl4.ResumeLayout(False)
        CType(Me.GridControl1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.GridView1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RepositoryItemCheckEdit1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents PanelControl1 As DevExpress.XtraEditors.PanelControl
    Friend WithEvents txtAlamat As DevExpress.XtraEditors.ButtonEdit
    Friend WithEvents PanelControl2 As DevExpress.XtraEditors.PanelControl
    Friend WithEvents PanelControl3 As DevExpress.XtraEditors.PanelControl
    Friend WithEvents PanelControl4 As DevExpress.XtraEditors.PanelControl
    Friend WithEvents LabelControl4 As DevExpress.XtraEditors.LabelControl
    Friend WithEvents LabelControl3 As DevExpress.XtraEditors.LabelControl
    Friend WithEvents TglSampai As DevExpress.XtraEditors.DateEdit
    Friend WithEvents tglDari As DevExpress.XtraEditors.DateEdit
    Friend WithEvents LabelControl2 As DevExpress.XtraEditors.LabelControl
    Friend WithEvents txtKasBank As DevExpress.XtraEditors.ButtonEdit
    Friend WithEvents LabelControl1 As DevExpress.XtraEditors.LabelControl
    Friend WithEvents cmdClose As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents ImageList1 As System.Windows.Forms.ImageList
    Friend WithEvents cmdPrint As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents LabelControl5 As DevExpress.XtraEditors.LabelControl
    Friend WithEvents GridControl1 As DevExpress.XtraGrid.GridControl
    Friend WithEvents GridView1 As DevExpress.XtraGrid.Views.Grid.GridView
    Friend WithEvents cmdRefresh As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents cmdExport As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents CCustomer As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents cTelp As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents cTotal As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents RepositoryItemCheckEdit1 As DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit
    Friend WithEvents LabelControl6 As DevExpress.XtraEditors.LabelControl
    Friend WithEvents ButtonEdit1 As DevExpress.XtraEditors.ButtonEdit
    Friend WithEvents cJumlahBayar As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents cJumlahDiskon As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents chkLunas As DevExpress.XtraEditors.CheckEdit
    Friend WithEvents cKontak As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents cAlamat As DevExpress.XtraGrid.Columns.GridColumn
End Class
