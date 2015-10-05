<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frLaporanSaldoHutangRekapPerSupplier
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
        Dim SerializableAppearanceObject1 As DevExpress.Utils.SerializableAppearanceObject = New DevExpress.Utils.SerializableAppearanceObject
        Dim SerializableAppearanceObject2 As DevExpress.Utils.SerializableAppearanceObject = New DevExpress.Utils.SerializableAppearanceObject
        Me.PanelControl1 = New DevExpress.XtraEditors.PanelControl
        Me.CheckEdit1 = New DevExpress.XtraEditors.CheckEdit
        Me.BarManager1 = New DevExpress.XtraBars.BarManager(Me.components)
        Me.Bar2 = New DevExpress.XtraBars.Bar
        Me.BarSubItem1 = New DevExpress.XtraBars.BarSubItem
        Me.BarButtonItem1 = New DevExpress.XtraBars.BarButtonItem
        Me.BarButtonItem3 = New DevExpress.XtraBars.BarButtonItem
        Me.BarButtonItem6 = New DevExpress.XtraBars.BarButtonItem
        Me.barDockControlTop = New DevExpress.XtraBars.BarDockControl
        Me.barDockControlBottom = New DevExpress.XtraBars.BarDockControl
        Me.barDockControlLeft = New DevExpress.XtraBars.BarDockControl
        Me.barDockControlRight = New DevExpress.XtraBars.BarDockControl
        Me.BarButtonItem7 = New DevExpress.XtraBars.BarButtonItem
        Me.lbAkunDenda = New DevExpress.XtraEditors.LabelControl
        Me.lbAkunDisc = New DevExpress.XtraEditors.LabelControl
        Me.LabelControl9 = New DevExpress.XtraEditors.LabelControl
        Me.LabelControl1 = New DevExpress.XtraEditors.LabelControl
        Me.txtAlamat = New DevExpress.XtraEditors.ButtonEdit
        Me.PanelControl2 = New DevExpress.XtraEditors.PanelControl
        Me.cmdPreview = New DevExpress.XtraEditors.SimpleButton
        Me.cmdExcel = New DevExpress.XtraEditors.SimpleButton
        Me.cmdRefresh = New DevExpress.XtraEditors.SimpleButton
        Me.cmdSave = New DevExpress.XtraEditors.SimpleButton
        Me.cmdTutup = New DevExpress.XtraEditors.SimpleButton
        Me.GridControl1 = New DevExpress.XtraGrid.GridControl
        Me.GridView1 = New DevExpress.XtraGrid.Views.Grid.GridView
        Me.CKodeSupplier = New DevExpress.XtraGrid.Columns.GridColumn
        Me.cNamaSupplier = New DevExpress.XtraGrid.Columns.GridColumn
        Me.cTanggal = New DevExpress.XtraGrid.Columns.GridColumn
        Me.cJumlah = New DevExpress.XtraGrid.Columns.GridColumn
        Me.cterBayar = New DevExpress.XtraGrid.Columns.GridColumn
        Me.RepositoryItemTextEdit1 = New DevExpress.XtraEditors.Repository.RepositoryItemTextEdit
        Me.cRetur = New DevExpress.XtraGrid.Columns.GridColumn
        Me.cSisa = New DevExpress.XtraGrid.Columns.GridColumn
        CType(Me.PanelControl1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.PanelControl1.SuspendLayout()
        CType(Me.CheckEdit1.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.BarManager1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtAlamat.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PanelControl2, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.PanelControl2.SuspendLayout()
        CType(Me.GridControl1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.GridView1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RepositoryItemTextEdit1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'PanelControl1
        '
        Me.PanelControl1.Controls.Add(Me.CheckEdit1)
        Me.PanelControl1.Controls.Add(Me.lbAkunDenda)
        Me.PanelControl1.Controls.Add(Me.lbAkunDisc)
        Me.PanelControl1.Controls.Add(Me.LabelControl9)
        Me.PanelControl1.Controls.Add(Me.LabelControl1)
        Me.PanelControl1.Controls.Add(Me.txtAlamat)
        Me.PanelControl1.Dock = System.Windows.Forms.DockStyle.Top
        Me.PanelControl1.Location = New System.Drawing.Point(0, 22)
        Me.PanelControl1.Name = "PanelControl1"
        Me.PanelControl1.Size = New System.Drawing.Size(1050, 49)
        Me.PanelControl1.TabIndex = 0
        '
        'CheckEdit1
        '
        Me.CheckEdit1.Location = New System.Drawing.Point(524, 15)
        Me.CheckEdit1.MenuManager = Me.BarManager1
        Me.CheckEdit1.Name = "CheckEdit1"
        Me.CheckEdit1.Properties.Caption = "Tampilkan Juga Nota Sudah Lunas"
        Me.CheckEdit1.Size = New System.Drawing.Size(269, 19)
        Me.CheckEdit1.TabIndex = 28
        '
        'BarManager1
        '
        Me.BarManager1.Bars.AddRange(New DevExpress.XtraBars.Bar() {Me.Bar2})
        Me.BarManager1.DockControls.Add(Me.barDockControlTop)
        Me.BarManager1.DockControls.Add(Me.barDockControlBottom)
        Me.BarManager1.DockControls.Add(Me.barDockControlLeft)
        Me.BarManager1.DockControls.Add(Me.barDockControlRight)
        Me.BarManager1.Form = Me
        Me.BarManager1.Items.AddRange(New DevExpress.XtraBars.BarItem() {Me.BarSubItem1, Me.BarButtonItem1, Me.BarButtonItem3, Me.BarButtonItem6, Me.BarButtonItem7})
        Me.BarManager1.MainMenu = Me.Bar2
        Me.BarManager1.MaxItemId = 10
        '
        'Bar2
        '
        Me.Bar2.BarName = "Main menu"
        Me.Bar2.DockCol = 0
        Me.Bar2.DockRow = 0
        Me.Bar2.DockStyle = DevExpress.XtraBars.BarDockStyle.Top
        Me.Bar2.FloatLocation = New System.Drawing.Point(252, 166)
        Me.Bar2.LinksPersistInfo.AddRange(New DevExpress.XtraBars.LinkPersistInfo() {New DevExpress.XtraBars.LinkPersistInfo(Me.BarSubItem1)})
        Me.Bar2.OptionsBar.MultiLine = True
        Me.Bar2.OptionsBar.UseWholeRow = True
        Me.Bar2.Text = "Main menu"
        Me.Bar2.Visible = False
        '
        'BarSubItem1
        '
        Me.BarSubItem1.Caption = "&Menu"
        Me.BarSubItem1.Id = 0
        Me.BarSubItem1.LinksPersistInfo.AddRange(New DevExpress.XtraBars.LinkPersistInfo() {New DevExpress.XtraBars.LinkPersistInfo(Me.BarButtonItem1), New DevExpress.XtraBars.LinkPersistInfo(Me.BarButtonItem3), New DevExpress.XtraBars.LinkPersistInfo(Me.BarButtonItem6)})
        Me.BarSubItem1.Name = "BarSubItem1"
        '
        'BarButtonItem1
        '
        Me.BarButtonItem1.Caption = "&Simpan"
        Me.BarButtonItem1.Id = 1
        Me.BarButtonItem1.ItemShortcut = New DevExpress.XtraBars.BarShortcut(System.Windows.Forms.Keys.F6)
        Me.BarButtonItem1.Name = "BarButtonItem1"
        '
        'BarButtonItem3
        '
        Me.BarButtonItem3.Caption = "&Simpan Layout"
        Me.BarButtonItem3.Id = 3
        Me.BarButtonItem3.ItemShortcut = New DevExpress.XtraBars.BarShortcut(System.Windows.Forms.Keys.F10)
        Me.BarButtonItem3.Name = "BarButtonItem3"
        '
        'BarButtonItem6
        '
        Me.BarButtonItem6.Caption = "Refresh"
        Me.BarButtonItem6.Id = 8
        Me.BarButtonItem6.ItemShortcut = New DevExpress.XtraBars.BarShortcut(System.Windows.Forms.Keys.F5)
        Me.BarButtonItem6.Name = "BarButtonItem6"
        '
        'barDockControlTop
        '
        Me.barDockControlTop.CausesValidation = False
        Me.barDockControlTop.Dock = System.Windows.Forms.DockStyle.Top
        Me.barDockControlTop.Location = New System.Drawing.Point(0, 0)
        Me.barDockControlTop.Size = New System.Drawing.Size(1050, 22)
        '
        'barDockControlBottom
        '
        Me.barDockControlBottom.CausesValidation = False
        Me.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.barDockControlBottom.Location = New System.Drawing.Point(0, 471)
        Me.barDockControlBottom.Size = New System.Drawing.Size(1050, 0)
        '
        'barDockControlLeft
        '
        Me.barDockControlLeft.CausesValidation = False
        Me.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left
        Me.barDockControlLeft.Location = New System.Drawing.Point(0, 22)
        Me.barDockControlLeft.Size = New System.Drawing.Size(0, 449)
        '
        'barDockControlRight
        '
        Me.barDockControlRight.CausesValidation = False
        Me.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right
        Me.barDockControlRight.Location = New System.Drawing.Point(1050, 22)
        Me.barDockControlRight.Size = New System.Drawing.Size(0, 449)
        '
        'BarButtonItem7
        '
        Me.BarButtonItem7.Caption = "Close"
        Me.BarButtonItem7.Id = 9
        Me.BarButtonItem7.ItemShortcut = New DevExpress.XtraBars.BarShortcut(System.Windows.Forms.Keys.F3)
        Me.BarButtonItem7.Name = "BarButtonItem7"
        '
        'lbAkunDenda
        '
        Me.lbAkunDenda.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lbAkunDenda.Appearance.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbAkunDenda.Location = New System.Drawing.Point(811, 110)
        Me.lbAkunDenda.Name = "lbAkunDenda"
        Me.lbAkunDenda.Size = New System.Drawing.Size(5, 16)
        Me.lbAkunDenda.TabIndex = 27
        Me.lbAkunDenda.Text = "-"
        '
        'lbAkunDisc
        '
        Me.lbAkunDisc.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lbAkunDisc.Appearance.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbAkunDisc.Location = New System.Drawing.Point(811, 82)
        Me.lbAkunDisc.Name = "lbAkunDisc"
        Me.lbAkunDisc.Size = New System.Drawing.Size(5, 16)
        Me.lbAkunDisc.TabIndex = 26
        Me.lbAkunDisc.Text = "-"
        '
        'LabelControl9
        '
        Me.LabelControl9.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.LabelControl9.Appearance.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(0, Byte), Integer))
        Me.LabelControl9.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
        Me.LabelControl9.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder
        Me.LabelControl9.Location = New System.Drawing.Point(0, 39)
        Me.LabelControl9.Name = "LabelControl9"
        Me.LabelControl9.Size = New System.Drawing.Size(1058, 3)
        Me.LabelControl9.TabIndex = 4
        '
        'LabelControl1
        '
        Me.LabelControl1.Appearance.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LabelControl1.Location = New System.Drawing.Point(12, 17)
        Me.LabelControl1.Name = "LabelControl1"
        Me.LabelControl1.Size = New System.Drawing.Size(47, 16)
        Me.LabelControl1.TabIndex = 2
        Me.LabelControl1.Text = "Supplier"
        '
        'txtAlamat
        '
        Me.txtAlamat.EnterMoveNextControl = True
        Me.txtAlamat.Location = New System.Drawing.Point(73, 14)
        Me.txtAlamat.Name = "txtAlamat"
        Me.txtAlamat.Properties.Appearance.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtAlamat.Properties.Appearance.Options.UseFont = True
        Me.txtAlamat.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Ellipsis, "", -1, True, True, False, DevExpress.XtraEditors.ImageLocation.MiddleCenter, Nothing, New DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.PageDown), SerializableAppearanceObject1, "", Nothing, Nothing, True), New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Close, "", -1, True, True, False, DevExpress.XtraEditors.ImageLocation.MiddleCenter, Nothing, New DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.Delete), SerializableAppearanceObject2, "", Nothing, Nothing, True)})
        Me.txtAlamat.Properties.ReadOnly = True
        Me.txtAlamat.Size = New System.Drawing.Size(435, 22)
        Me.txtAlamat.TabIndex = 3
        '
        'PanelControl2
        '
        Me.PanelControl2.Controls.Add(Me.cmdPreview)
        Me.PanelControl2.Controls.Add(Me.cmdExcel)
        Me.PanelControl2.Controls.Add(Me.cmdRefresh)
        Me.PanelControl2.Controls.Add(Me.cmdSave)
        Me.PanelControl2.Controls.Add(Me.cmdTutup)
        Me.PanelControl2.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.PanelControl2.Location = New System.Drawing.Point(0, 435)
        Me.PanelControl2.Name = "PanelControl2"
        Me.PanelControl2.Size = New System.Drawing.Size(1050, 36)
        Me.PanelControl2.TabIndex = 1
        '
        'cmdPreview
        '
        Me.cmdPreview.Appearance.Font = New System.Drawing.Font("Tahoma", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdPreview.Appearance.ForeColor = System.Drawing.Color.Blue
        Me.cmdPreview.Appearance.Options.UseFont = True
        Me.cmdPreview.Appearance.Options.UseForeColor = True
        Me.cmdPreview.ImageIndex = 7
        Me.cmdPreview.Location = New System.Drawing.Point(249, 6)
        Me.cmdPreview.Name = "cmdPreview"
        Me.cmdPreview.Size = New System.Drawing.Size(116, 25)
        Me.cmdPreview.TabIndex = 24
        Me.cmdPreview.Text = "&Preview"
        '
        'cmdExcel
        '
        Me.cmdExcel.Appearance.Font = New System.Drawing.Font("Tahoma", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdExcel.Appearance.ForeColor = System.Drawing.Color.Blue
        Me.cmdExcel.Appearance.Options.UseFont = True
        Me.cmdExcel.Appearance.Options.UseForeColor = True
        Me.cmdExcel.ImageIndex = 10
        Me.cmdExcel.Location = New System.Drawing.Point(127, 6)
        Me.cmdExcel.Name = "cmdExcel"
        Me.cmdExcel.Size = New System.Drawing.Size(116, 25)
        Me.cmdExcel.TabIndex = 23
        Me.cmdExcel.Text = "&Excel"
        '
        'cmdRefresh
        '
        Me.cmdRefresh.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.cmdRefresh.Appearance.Font = New System.Drawing.Font("Tahoma", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdRefresh.Appearance.ForeColor = System.Drawing.Color.Blue
        Me.cmdRefresh.Appearance.Options.UseFont = True
        Me.cmdRefresh.Appearance.Options.UseForeColor = True
        Me.cmdRefresh.ImageIndex = 10
        Me.cmdRefresh.Location = New System.Drawing.Point(5, 6)
        Me.cmdRefresh.Name = "cmdRefresh"
        Me.cmdRefresh.Size = New System.Drawing.Size(116, 25)
        Me.cmdRefresh.TabIndex = 22
        Me.cmdRefresh.Text = "&Refresh"
        '
        'cmdSave
        '
        Me.cmdSave.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdSave.Appearance.Font = New System.Drawing.Font("Tahoma", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdSave.Appearance.ForeColor = System.Drawing.Color.Blue
        Me.cmdSave.Appearance.Options.UseFont = True
        Me.cmdSave.Appearance.Options.UseForeColor = True
        Me.cmdSave.ImageIndex = 10
        Me.cmdSave.Location = New System.Drawing.Point(807, 6)
        Me.cmdSave.Name = "cmdSave"
        Me.cmdSave.Size = New System.Drawing.Size(116, 25)
        Me.cmdSave.TabIndex = 20
        Me.cmdSave.Text = "&Simpan"
        '
        'cmdTutup
        '
        Me.cmdTutup.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdTutup.Appearance.Font = New System.Drawing.Font("Tahoma", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdTutup.Appearance.ForeColor = System.Drawing.Color.Blue
        Me.cmdTutup.Appearance.Options.UseFont = True
        Me.cmdTutup.Appearance.Options.UseForeColor = True
        Me.cmdTutup.ImageIndex = 11
        Me.cmdTutup.Location = New System.Drawing.Point(929, 6)
        Me.cmdTutup.Name = "cmdTutup"
        Me.cmdTutup.Size = New System.Drawing.Size(116, 25)
        Me.cmdTutup.TabIndex = 21
        Me.cmdTutup.Text = "&Tutup"
        '
        'GridControl1
        '
        Me.GridControl1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.GridControl1.Location = New System.Drawing.Point(0, 71)
        Me.GridControl1.MainView = Me.GridView1
        Me.GridControl1.Name = "GridControl1"
        Me.GridControl1.RepositoryItems.AddRange(New DevExpress.XtraEditors.Repository.RepositoryItem() {Me.RepositoryItemTextEdit1})
        Me.GridControl1.Size = New System.Drawing.Size(1050, 364)
        Me.GridControl1.TabIndex = 0
        Me.GridControl1.ViewCollection.AddRange(New DevExpress.XtraGrid.Views.Base.BaseView() {Me.GridView1})
        '
        'GridView1
        '
        Me.GridView1.Columns.AddRange(New DevExpress.XtraGrid.Columns.GridColumn() {Me.CKodeSupplier, Me.cNamaSupplier, Me.cTanggal, Me.cJumlah, Me.cterBayar, Me.cRetur, Me.cSisa})
        Me.GridView1.GridControl = Me.GridControl1
        Me.GridView1.Name = "GridView1"
        Me.GridView1.OptionsView.ShowFooter = True
        Me.GridView1.OptionsView.ShowGroupPanel = False
        '
        'CKodeSupplier
        '
        Me.CKodeSupplier.Caption = "Kode Supplier"
        Me.CKodeSupplier.FieldName = "KodeSupplier"
        Me.CKodeSupplier.Name = "CKodeSupplier"
        Me.CKodeSupplier.Visible = True
        Me.CKodeSupplier.VisibleIndex = 0
        '
        'cNamaSupplier
        '
        Me.cNamaSupplier.Caption = "Supplier"
        Me.cNamaSupplier.FieldName = "Supplier"
        Me.cNamaSupplier.Name = "cNamaSupplier"
        Me.cNamaSupplier.Visible = True
        Me.cNamaSupplier.VisibleIndex = 1
        '
        'cTanggal
        '
        Me.cTanggal.Caption = "Tanggal"
        Me.cTanggal.DisplayFormat.FormatString = "dd-MM-yyyy"
        Me.cTanggal.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime
        Me.cTanggal.FieldName = "Tanggal"
        Me.cTanggal.Name = "cTanggal"
        Me.cTanggal.OptionsColumn.AllowEdit = False
        Me.cTanggal.OptionsColumn.ReadOnly = True
        Me.cTanggal.Visible = True
        Me.cTanggal.VisibleIndex = 2
        Me.cTanggal.Width = 80
        '
        'cJumlah
        '
        Me.cJumlah.Caption = "Jumlah"
        Me.cJumlah.DisplayFormat.FormatString = "n2"
        Me.cJumlah.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
        Me.cJumlah.FieldName = "Jumlah"
        Me.cJumlah.Name = "cJumlah"
        Me.cJumlah.SummaryItem.DisplayFormat = "{0:###,##0.00}"
        Me.cJumlah.SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum
        Me.cJumlah.Visible = True
        Me.cJumlah.VisibleIndex = 3
        '
        'cterBayar
        '
        Me.cterBayar.Caption = "Terbayar"
        Me.cterBayar.ColumnEdit = Me.RepositoryItemTextEdit1
        Me.cterBayar.DisplayFormat.FormatString = "n2"
        Me.cterBayar.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
        Me.cterBayar.FieldName = "Terbayar"
        Me.cterBayar.Name = "cterBayar"
        Me.cterBayar.OptionsColumn.AllowEdit = False
        Me.cterBayar.OptionsColumn.ReadOnly = True
        Me.cterBayar.SummaryItem.DisplayFormat = "{0:###,##0.00}"
        Me.cterBayar.SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum
        Me.cterBayar.Visible = True
        Me.cterBayar.VisibleIndex = 4
        Me.cterBayar.Width = 117
        '
        'RepositoryItemTextEdit1
        '
        Me.RepositoryItemTextEdit1.AutoHeight = False
        Me.RepositoryItemTextEdit1.Mask.EditMask = "###,###,###,###,##0.00"
        Me.RepositoryItemTextEdit1.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric
        Me.RepositoryItemTextEdit1.Mask.UseMaskAsDisplayFormat = True
        Me.RepositoryItemTextEdit1.Name = "RepositoryItemTextEdit1"
        '
        'cRetur
        '
        Me.cRetur.Caption = "Retur"
        Me.cRetur.DisplayFormat.FormatString = "n2"
        Me.cRetur.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
        Me.cRetur.FieldName = "Retur"
        Me.cRetur.Name = "cRetur"
        Me.cRetur.SummaryItem.DisplayFormat = "{0:###,##0.00}"
        Me.cRetur.SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum
        Me.cRetur.Visible = True
        Me.cRetur.VisibleIndex = 6
        '
        'cSisa
        '
        Me.cSisa.Caption = "Sisa"
        Me.cSisa.ColumnEdit = Me.RepositoryItemTextEdit1
        Me.cSisa.DisplayFormat.FormatString = "n2"
        Me.cSisa.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
        Me.cSisa.FieldName = "Sisa"
        Me.cSisa.Name = "cSisa"
        Me.cSisa.OptionsColumn.AllowEdit = False
        Me.cSisa.OptionsColumn.ReadOnly = True
        Me.cSisa.SummaryItem.DisplayFormat = "{0:###,##0.00}"
        Me.cSisa.SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum
        Me.cSisa.Visible = True
        Me.cSisa.VisibleIndex = 5
        Me.cSisa.Width = 72
        '
        'frLaporanSaldoHutangRekapPerSupplier
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1050, 471)
        Me.Controls.Add(Me.GridControl1)
        Me.Controls.Add(Me.PanelControl1)
        Me.Controls.Add(Me.PanelControl2)
        Me.Controls.Add(Me.barDockControlLeft)
        Me.Controls.Add(Me.barDockControlRight)
        Me.Controls.Add(Me.barDockControlBottom)
        Me.Controls.Add(Me.barDockControlTop)
        Me.Name = "frLaporanSaldoHutangRekapPerSupplier"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Laporan Rekap Saldo  Hutang Per Supplier"
        CType(Me.PanelControl1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.PanelControl1.ResumeLayout(False)
        Me.PanelControl1.PerformLayout()
        CType(Me.CheckEdit1.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.BarManager1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtAlamat.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PanelControl2, System.ComponentModel.ISupportInitialize).EndInit()
        Me.PanelControl2.ResumeLayout(False)
        CType(Me.GridControl1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.GridView1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RepositoryItemTextEdit1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents PanelControl1 As DevExpress.XtraEditors.PanelControl
    Friend WithEvents PanelControl2 As DevExpress.XtraEditors.PanelControl
    Friend WithEvents LabelControl1 As DevExpress.XtraEditors.LabelControl
    Friend WithEvents txtAlamat As DevExpress.XtraEditors.ButtonEdit
    Friend WithEvents LabelControl9 As DevExpress.XtraEditors.LabelControl
    Friend WithEvents lbAkunDenda As DevExpress.XtraEditors.LabelControl
    Friend WithEvents lbAkunDisc As DevExpress.XtraEditors.LabelControl
    Friend WithEvents cmdSave As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents cmdTutup As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents cmdRefresh As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents BarManager1 As DevExpress.XtraBars.BarManager
    Friend WithEvents Bar2 As DevExpress.XtraBars.Bar
    Friend WithEvents BarSubItem1 As DevExpress.XtraBars.BarSubItem
    Friend WithEvents BarButtonItem1 As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents BarButtonItem3 As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents BarButtonItem6 As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents BarButtonItem7 As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents barDockControlTop As DevExpress.XtraBars.BarDockControl
    Friend WithEvents barDockControlBottom As DevExpress.XtraBars.BarDockControl
    Friend WithEvents barDockControlLeft As DevExpress.XtraBars.BarDockControl
    Friend WithEvents barDockControlRight As DevExpress.XtraBars.BarDockControl
    Friend WithEvents GridControl1 As DevExpress.XtraGrid.GridControl
    Friend WithEvents GridView1 As DevExpress.XtraGrid.Views.Grid.GridView
    Friend WithEvents cTanggal As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents RepositoryItemTextEdit1 As DevExpress.XtraEditors.Repository.RepositoryItemTextEdit
    Friend WithEvents cterBayar As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents cSisa As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents cRetur As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents CheckEdit1 As DevExpress.XtraEditors.CheckEdit
    Friend WithEvents CKodeSupplier As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents cNamaSupplier As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents cmdPreview As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents cmdExcel As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents cJumlah As DevExpress.XtraGrid.Columns.GridColumn
End Class
