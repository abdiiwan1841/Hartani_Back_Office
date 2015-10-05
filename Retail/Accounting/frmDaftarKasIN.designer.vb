<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmDaftarKasIN
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
        Me.GridControl1 = New DevExpress.XtraGrid.GridControl
        Me.GridView1 = New DevExpress.XtraGrid.Views.Grid.GridView
        Me.colID = New DevExpress.XtraGrid.Columns.GridColumn
        Me.colIsPosting = New DevExpress.XtraGrid.Columns.GridColumn
        Me.colKode = New DevExpress.XtraGrid.Columns.GridColumn
        Me.colNoReff = New DevExpress.XtraGrid.Columns.GridColumn
        Me.colTanggal = New DevExpress.XtraGrid.Columns.GridColumn
        Me.colKet = New DevExpress.XtraGrid.Columns.GridColumn
        Me.colTotal = New DevExpress.XtraGrid.Columns.GridColumn
        Me.cNamaKas = New DevExpress.XtraGrid.Columns.GridColumn
        Me.colNamaAkun = New DevExpress.XtraGrid.Columns.GridColumn
        Me.RepositoryItemCheckEdit1 = New DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit
        Me.RepositoryItemCheckEdit2 = New DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit
        Me.PanelControl1 = New DevExpress.XtraEditors.PanelControl
        Me.txtkas = New DevExpress.XtraEditors.ButtonEdit
        Me.lbKas = New DevExpress.XtraEditors.LabelControl
        Me.Label2 = New System.Windows.Forms.Label
        Me.LabelControl4 = New DevExpress.XtraEditors.LabelControl
        Me.LabelControl5 = New DevExpress.XtraEditors.LabelControl
        Me.TglSampai = New DevExpress.XtraEditors.DateEdit
        Me.tglDari = New DevExpress.XtraEditors.DateEdit
        Me.cbStatus = New DevExpress.XtraEditors.CheckedComboBoxEdit
        Me.BarManager1 = New DevExpress.XtraBars.BarManager(Me.components)
        Me.Bar1 = New DevExpress.XtraBars.Bar
        Me.BarSubItem1 = New DevExpress.XtraBars.BarSubItem
        Me.BarSubItem2 = New DevExpress.XtraBars.BarSubItem
        Me.mnPosting = New DevExpress.XtraBars.BarButtonItem
        Me.mnUnPosting = New DevExpress.XtraBars.BarButtonItem
        Me.mnRepairPosting = New DevExpress.XtraBars.BarButtonItem
        Me.BarSubItem5 = New DevExpress.XtraBars.BarSubItem
        Me.BarButtonItem10 = New DevExpress.XtraBars.BarButtonItem
        Me.BarButtonItem11 = New DevExpress.XtraBars.BarButtonItem
        Me.barDockControlTop = New DevExpress.XtraBars.BarDockControl
        Me.barDockControlBottom = New DevExpress.XtraBars.BarDockControl
        Me.barDockControlLeft = New DevExpress.XtraBars.BarDockControl
        Me.barDockControlRight = New DevExpress.XtraBars.BarDockControl
        Me.mnHasilPosting = New DevExpress.XtraBars.BarButtonItem
        Me.RepositoryItemTextEdit1 = New DevExpress.XtraEditors.Repository.RepositoryItemTextEdit
        Me.LabelControl3 = New DevExpress.XtraEditors.LabelControl
        Me.LabelControl2 = New DevExpress.XtraEditors.LabelControl
        Me.txtAkun = New DevExpress.XtraEditors.ButtonEdit
        Me.PanelControl3 = New DevExpress.XtraEditors.PanelControl
        Me.cmdExport = New DevExpress.XtraEditors.SimpleButton
        Me.cmdRefresh = New DevExpress.XtraEditors.SimpleButton
        Me.cmdDelete = New DevExpress.XtraEditors.SimpleButton
        Me.cmdEdit = New DevExpress.XtraEditors.SimpleButton
        Me.cmdClose = New DevExpress.XtraEditors.SimpleButton
        Me.cmdNew = New DevExpress.XtraEditors.SimpleButton
        Me.cmdMark = New DevExpress.XtraEditors.SimpleButton
        Me.PopupMenu1 = New DevExpress.XtraBars.PopupMenu(Me.components)
        CType(Me.GridControl1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.GridView1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RepositoryItemCheckEdit1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RepositoryItemCheckEdit2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PanelControl1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.PanelControl1.SuspendLayout()
        CType(Me.txtkas.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.TglSampai.Properties.VistaTimeProperties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.TglSampai.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.tglDari.Properties.VistaTimeProperties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.tglDari.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.cbStatus.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.BarManager1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RepositoryItemTextEdit1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtAkun.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PanelControl3, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.PanelControl3.SuspendLayout()
        CType(Me.PopupMenu1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'GridControl1
        '
        Me.GridControl1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.GridControl1.Location = New System.Drawing.Point(133, 86)
        Me.GridControl1.MainView = Me.GridView1
        Me.GridControl1.Name = "GridControl1"
        Me.GridControl1.RepositoryItems.AddRange(New DevExpress.XtraEditors.Repository.RepositoryItem() {Me.RepositoryItemCheckEdit1, Me.RepositoryItemCheckEdit2})
        Me.GridControl1.ShowOnlyPredefinedDetails = True
        Me.GridControl1.Size = New System.Drawing.Size(887, 253)
        Me.GridControl1.TabIndex = 9
        Me.GridControl1.ViewCollection.AddRange(New DevExpress.XtraGrid.Views.Base.BaseView() {Me.GridView1})
        '
        'GridView1
        '
        Me.GridView1.Columns.AddRange(New DevExpress.XtraGrid.Columns.GridColumn() {Me.colID, Me.colIsPosting, Me.colKode, Me.colNoReff, Me.colTanggal, Me.colKet, Me.colTotal, Me.cNamaKas, Me.colNamaAkun})
        Me.GridView1.CustomizationFormBounds = New System.Drawing.Rectangle(592, 298, 208, 168)
        Me.GridView1.GridControl = Me.GridControl1
        Me.GridView1.Name = "GridView1"
        Me.GridView1.OptionsBehavior.Editable = False
        Me.GridView1.OptionsDetail.EnableDetailToolTip = True
        Me.GridView1.OptionsSelection.MultiSelect = True
        Me.GridView1.OptionsView.ColumnAutoWidth = False
        Me.GridView1.OptionsView.ShowFooter = True
        '
        'colID
        '
        Me.colID.FieldName = "ID"
        Me.colID.Name = "colID"
        Me.colID.Width = 23
        '
        'colIsPosting
        '
        Me.colIsPosting.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
        Me.colIsPosting.FieldName = "IsPosted"
        Me.colIsPosting.Name = "colIsPosting"
        Me.colIsPosting.SummaryItem.DisplayFormat = "{0}"
        Me.colIsPosting.SummaryItem.FieldName = "IsPosting"
        Me.colIsPosting.SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Count
        Me.colIsPosting.Visible = True
        Me.colIsPosting.VisibleIndex = 0
        Me.colIsPosting.Width = 59
        '
        'colKode
        '
        Me.colKode.FieldName = "Kode"
        Me.colKode.Name = "colKode"
        Me.colKode.Visible = True
        Me.colKode.VisibleIndex = 1
        Me.colKode.Width = 36
        '
        'colNoReff
        '
        Me.colNoReff.Caption = "No. Reff"
        Me.colNoReff.FieldName = "KodeReff"
        Me.colNoReff.Name = "colNoReff"
        Me.colNoReff.Visible = True
        Me.colNoReff.VisibleIndex = 6
        '
        'colTanggal
        '
        Me.colTanggal.DisplayFormat.FormatString = "dd/MM/yyyy"
        Me.colTanggal.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime
        Me.colTanggal.FieldName = "Tanggal"
        Me.colTanggal.Name = "colTanggal"
        Me.colTanggal.Visible = True
        Me.colTanggal.VisibleIndex = 2
        Me.colTanggal.Width = 50
        '
        'colKet
        '
        Me.colKet.Caption = "Keterangan"
        Me.colKet.FieldName = "Keterangan"
        Me.colKet.Name = "colKet"
        Me.colKet.Visible = True
        Me.colKet.VisibleIndex = 3
        Me.colKet.Width = 28
        '
        'colTotal
        '
        Me.colTotal.Caption = "Jumlah"
        Me.colTotal.DisplayFormat.FormatString = "n2"
        Me.colTotal.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
        Me.colTotal.FieldName = "Jumlah"
        Me.colTotal.Name = "colTotal"
        Me.colTotal.SummaryItem.DisplayFormat = "{0:n2}"
        Me.colTotal.SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum
        Me.colTotal.Visible = True
        Me.colTotal.VisibleIndex = 4
        Me.colTotal.Width = 36
        '
        'cNamaKas
        '
        Me.cNamaKas.Caption = "Kas/Bank"
        Me.cNamaKas.FieldName = "KasBank"
        Me.cNamaKas.Name = "cNamaKas"
        Me.cNamaKas.Visible = True
        Me.cNamaKas.VisibleIndex = 5
        '
        'colNamaAkun
        '
        Me.colNamaAkun.FieldName = "NamaAkun"
        Me.colNamaAkun.Name = "colNamaAkun"
        Me.colNamaAkun.Width = 66
        '
        'RepositoryItemCheckEdit1
        '
        Me.RepositoryItemCheckEdit1.Appearance.Options.UseTextOptions = True
        Me.RepositoryItemCheckEdit1.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.RepositoryItemCheckEdit1.Caption = ""
        Me.RepositoryItemCheckEdit1.Name = "RepositoryItemCheckEdit1"
        Me.RepositoryItemCheckEdit1.NullStyle = DevExpress.XtraEditors.Controls.StyleIndeterminate.Inactive
        Me.RepositoryItemCheckEdit1.Tag = False
        '
        'RepositoryItemCheckEdit2
        '
        Me.RepositoryItemCheckEdit2.AutoHeight = False
        Me.RepositoryItemCheckEdit2.CheckStyle = DevExpress.XtraEditors.Controls.CheckStyles.Style1
        Me.RepositoryItemCheckEdit2.Name = "RepositoryItemCheckEdit2"
        Me.RepositoryItemCheckEdit2.NullStyle = DevExpress.XtraEditors.Controls.StyleIndeterminate.Unchecked
        '
        'PanelControl1
        '
        Me.PanelControl1.Controls.Add(Me.txtkas)
        Me.PanelControl1.Controls.Add(Me.lbKas)
        Me.PanelControl1.Controls.Add(Me.Label2)
        Me.PanelControl1.Controls.Add(Me.LabelControl4)
        Me.PanelControl1.Controls.Add(Me.LabelControl5)
        Me.PanelControl1.Controls.Add(Me.TglSampai)
        Me.PanelControl1.Controls.Add(Me.tglDari)
        Me.PanelControl1.Controls.Add(Me.cbStatus)
        Me.PanelControl1.Controls.Add(Me.LabelControl3)
        Me.PanelControl1.Controls.Add(Me.LabelControl2)
        Me.PanelControl1.Controls.Add(Me.txtAkun)
        Me.PanelControl1.Dock = System.Windows.Forms.DockStyle.Top
        Me.PanelControl1.Location = New System.Drawing.Point(0, 22)
        Me.PanelControl1.Name = "PanelControl1"
        Me.PanelControl1.Size = New System.Drawing.Size(1020, 64)
        Me.PanelControl1.TabIndex = 1
        '
        'txtkas
        '
        Me.txtkas.Enabled = False
        Me.txtkas.Location = New System.Drawing.Point(424, 10)
        Me.txtkas.Name = "txtkas"
        Me.txtkas.Properties.Appearance.BackColor = System.Drawing.Color.White
        Me.txtkas.Properties.Appearance.BackColor2 = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.txtkas.Properties.Appearance.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtkas.Properties.Appearance.Options.UseBackColor = True
        Me.txtkas.Properties.Appearance.Options.UseFont = True
        Me.txtkas.Properties.AppearanceDisabled.BackColor = System.Drawing.Color.DarkGray
        Me.txtkas.Properties.AppearanceDisabled.BackColor2 = System.Drawing.Color.Gray
        Me.txtkas.Properties.AppearanceDisabled.Options.UseBackColor = True
        Me.txtkas.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton, New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Close)})
        Me.txtkas.Properties.ReadOnly = True
        Me.txtkas.Size = New System.Drawing.Size(255, 22)
        Me.txtkas.TabIndex = 24
        '
        'lbKas
        '
        Me.lbKas.Appearance.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbKas.Cursor = System.Windows.Forms.Cursors.Hand
        Me.lbKas.Location = New System.Drawing.Point(336, 9)
        Me.lbKas.Name = "lbKas"
        Me.lbKas.Size = New System.Drawing.Size(52, 16)
        Me.lbKas.TabIndex = 23
        Me.lbKas.Text = "Kas/Bank"
        '
        'Label2
        '
        Me.Label2.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label2.BackColor = System.Drawing.Color.Transparent
        Me.Label2.Font = New System.Drawing.Font("Rockwell", 15.75!, CType((System.Drawing.FontStyle.Bold), System.Drawing.FontStyle), System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.ForeColor = System.Drawing.SystemColors.ActiveCaption
        Me.Label2.ImageAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.Label2.Location = New System.Drawing.Point(769, 9)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(239, 23)
        Me.Label2.TabIndex = 13
        Me.Label2.Text = "DAFTAR KAS MASUK"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'LabelControl4
        '
        Me.LabelControl4.Appearance.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LabelControl4.Location = New System.Drawing.Point(181, 13)
        Me.LabelControl4.Name = "LabelControl4"
        Me.LabelControl4.Size = New System.Drawing.Size(18, 16)
        Me.LabelControl4.TabIndex = 11
        Me.LabelControl4.Text = "s/d"
        '
        'LabelControl5
        '
        Me.LabelControl5.Appearance.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LabelControl5.Cursor = System.Windows.Forms.Cursors.Hand
        Me.LabelControl5.Location = New System.Drawing.Point(8, 13)
        Me.LabelControl5.Name = "LabelControl5"
        Me.LabelControl5.Size = New System.Drawing.Size(46, 16)
        Me.LabelControl5.TabIndex = 10
        Me.LabelControl5.Text = "Tanggal"
        '
        'TglSampai
        '
        Me.TglSampai.EditValue = Nothing
        Me.TglSampai.Location = New System.Drawing.Point(205, 10)
        Me.TglSampai.Name = "TglSampai"
        Me.TglSampai.Properties.Appearance.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TglSampai.Properties.Appearance.Options.UseFont = True
        Me.TglSampai.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.TglSampai.Properties.Mask.EditMask = "dd/MM/yyyy"
        Me.TglSampai.Properties.Mask.UseMaskAsDisplayFormat = True
        Me.TglSampai.Properties.VistaTimeProperties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton})
        Me.TglSampai.Size = New System.Drawing.Size(116, 22)
        Me.TglSampai.TabIndex = 9
        '
        'tglDari
        '
        Me.tglDari.EditValue = Nothing
        Me.tglDari.Location = New System.Drawing.Point(66, 10)
        Me.tglDari.Name = "tglDari"
        Me.tglDari.Properties.Appearance.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tglDari.Properties.Appearance.Options.UseFont = True
        Me.tglDari.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.tglDari.Properties.Mask.EditMask = "dd/MM/yyyy"
        Me.tglDari.Properties.Mask.UseMaskAsDisplayFormat = True
        Me.tglDari.Properties.VistaTimeProperties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton})
        Me.tglDari.Size = New System.Drawing.Size(109, 22)
        Me.tglDari.TabIndex = 8
        '
        'cbStatus
        '
        Me.cbStatus.EditValue = ""
        Me.cbStatus.Location = New System.Drawing.Point(66, 38)
        Me.cbStatus.MenuManager = Me.BarManager1
        Me.cbStatus.Name = "cbStatus"
        Me.cbStatus.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.cbStatus.Properties.Items.AddRange(New DevExpress.XtraEditors.Controls.CheckedListBoxItem() {New DevExpress.XtraEditors.Controls.CheckedListBoxItem("0", "UnPosting"), New DevExpress.XtraEditors.Controls.CheckedListBoxItem("1", "Posting")})
        Me.cbStatus.Size = New System.Drawing.Size(255, 20)
        Me.cbStatus.TabIndex = 7
        '
        'BarManager1
        '
        Me.BarManager1.Bars.AddRange(New DevExpress.XtraBars.Bar() {Me.Bar1})
        Me.BarManager1.DockControls.Add(Me.barDockControlTop)
        Me.BarManager1.DockControls.Add(Me.barDockControlBottom)
        Me.BarManager1.DockControls.Add(Me.barDockControlLeft)
        Me.BarManager1.DockControls.Add(Me.barDockControlRight)
        Me.BarManager1.Form = Me
        Me.BarManager1.Items.AddRange(New DevExpress.XtraBars.BarItem() {Me.BarSubItem1, Me.BarSubItem2, Me.mnPosting, Me.BarSubItem5, Me.BarButtonItem10, Me.BarButtonItem11, Me.mnUnPosting, Me.mnHasilPosting, Me.mnRepairPosting})
        Me.BarManager1.MainMenu = Me.Bar1
        Me.BarManager1.MaxItemId = 23
        Me.BarManager1.RepositoryItems.AddRange(New DevExpress.XtraEditors.Repository.RepositoryItem() {Me.RepositoryItemTextEdit1})
        '
        'Bar1
        '
        Me.Bar1.BarName = "Custom 1"
        Me.Bar1.DockCol = 0
        Me.Bar1.DockRow = 0
        Me.Bar1.DockStyle = DevExpress.XtraBars.BarDockStyle.Top
        Me.Bar1.LinksPersistInfo.AddRange(New DevExpress.XtraBars.LinkPersistInfo() {New DevExpress.XtraBars.LinkPersistInfo(Me.BarSubItem1)})
        Me.Bar1.OptionsBar.MultiLine = True
        Me.Bar1.OptionsBar.UseWholeRow = True
        Me.Bar1.Text = "Custom 1"
        Me.Bar1.Visible = False
        '
        'BarSubItem1
        '
        Me.BarSubItem1.Caption = "Menu"
        Me.BarSubItem1.Id = 0
        Me.BarSubItem1.LinksPersistInfo.AddRange(New DevExpress.XtraBars.LinkPersistInfo() {New DevExpress.XtraBars.LinkPersistInfo(Me.BarSubItem2), New DevExpress.XtraBars.LinkPersistInfo(Me.BarSubItem5)})
        Me.BarSubItem1.Name = "BarSubItem1"
        Me.BarSubItem1.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
        '
        'BarSubItem2
        '
        Me.BarSubItem2.Caption = "Action"
        Me.BarSubItem2.Id = 2
        Me.BarSubItem2.LinksPersistInfo.AddRange(New DevExpress.XtraBars.LinkPersistInfo() {New DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, Me.mnPosting, DevExpress.XtraBars.BarItemPaintStyle.Standard), New DevExpress.XtraBars.LinkPersistInfo(Me.mnUnPosting), New DevExpress.XtraBars.LinkPersistInfo(Me.mnRepairPosting, True)})
        Me.BarSubItem2.Name = "BarSubItem2"
        '
        'mnPosting
        '
        Me.mnPosting.Caption = "Posting"
        Me.mnPosting.Id = 16
        Me.mnPosting.Name = "mnPosting"
        '
        'mnUnPosting
        '
        Me.mnUnPosting.Caption = "UnPosting"
        Me.mnUnPosting.Id = 20
        Me.mnUnPosting.Name = "mnUnPosting"
        '
        'mnRepairPosting
        '
        Me.mnRepairPosting.Caption = "Repair Posting"
        Me.mnRepairPosting.Id = 22
        Me.mnRepairPosting.Name = "mnRepairPosting"
        '
        'BarSubItem5
        '
        Me.BarSubItem5.Caption = "Cetak"
        Me.BarSubItem5.Id = 17
        Me.BarSubItem5.LinksPersistInfo.AddRange(New DevExpress.XtraBars.LinkPersistInfo() {New DevExpress.XtraBars.LinkPersistInfo(Me.BarButtonItem10), New DevExpress.XtraBars.LinkPersistInfo(Me.BarButtonItem11)})
        Me.BarSubItem5.Name = "BarSubItem5"
        '
        'BarButtonItem10
        '
        Me.BarButtonItem10.Caption = "BUKTI KAS MASUK"
        Me.BarButtonItem10.Id = 18
        Me.BarButtonItem10.Name = "BarButtonItem10"
        '
        'BarButtonItem11
        '
        Me.BarButtonItem11.Caption = "BUKTI BANK MASUK"
        Me.BarButtonItem11.Id = 19
        Me.BarButtonItem11.Name = "BarButtonItem11"
        '
        'barDockControlTop
        '
        Me.barDockControlTop.CausesValidation = False
        Me.barDockControlTop.Dock = System.Windows.Forms.DockStyle.Top
        Me.barDockControlTop.Location = New System.Drawing.Point(0, 0)
        Me.barDockControlTop.Size = New System.Drawing.Size(1020, 22)
        '
        'barDockControlBottom
        '
        Me.barDockControlBottom.CausesValidation = False
        Me.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.barDockControlBottom.Location = New System.Drawing.Point(0, 339)
        Me.barDockControlBottom.Size = New System.Drawing.Size(1020, 0)
        '
        'barDockControlLeft
        '
        Me.barDockControlLeft.CausesValidation = False
        Me.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left
        Me.barDockControlLeft.Location = New System.Drawing.Point(0, 22)
        Me.barDockControlLeft.Size = New System.Drawing.Size(0, 317)
        '
        'barDockControlRight
        '
        Me.barDockControlRight.CausesValidation = False
        Me.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right
        Me.barDockControlRight.Location = New System.Drawing.Point(1020, 22)
        Me.barDockControlRight.Size = New System.Drawing.Size(0, 317)
        '
        'mnHasilPosting
        '
        Me.mnHasilPosting.Caption = "Hasil Posting"
        Me.mnHasilPosting.Id = 21
        Me.mnHasilPosting.Name = "mnHasilPosting"
        '
        'RepositoryItemTextEdit1
        '
        Me.RepositoryItemTextEdit1.AutoHeight = False
        Me.RepositoryItemTextEdit1.Name = "RepositoryItemTextEdit1"
        '
        'LabelControl3
        '
        Me.LabelControl3.Appearance.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LabelControl3.Cursor = System.Windows.Forms.Cursors.Hand
        Me.LabelControl3.Location = New System.Drawing.Point(8, 39)
        Me.LabelControl3.Name = "LabelControl3"
        Me.LabelControl3.Size = New System.Drawing.Size(36, 16)
        Me.LabelControl3.TabIndex = 6
        Me.LabelControl3.Text = "Status"
        '
        'LabelControl2
        '
        Me.LabelControl2.Appearance.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LabelControl2.Cursor = System.Windows.Forms.Cursors.Hand
        Me.LabelControl2.Location = New System.Drawing.Point(336, 42)
        Me.LabelControl2.Name = "LabelControl2"
        Me.LabelControl2.Size = New System.Drawing.Size(84, 16)
        Me.LabelControl2.TabIndex = 5
        Me.LabelControl2.Text = "Akun Kas/Bank"
        '
        'txtAkun
        '
        Me.txtAkun.Enabled = False
        Me.txtAkun.Location = New System.Drawing.Point(424, 39)
        Me.txtAkun.Name = "txtAkun"
        Me.txtAkun.Properties.Appearance.BackColor = System.Drawing.Color.White
        Me.txtAkun.Properties.Appearance.BackColor2 = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.txtAkun.Properties.Appearance.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtAkun.Properties.Appearance.Options.UseBackColor = True
        Me.txtAkun.Properties.Appearance.Options.UseFont = True
        Me.txtAkun.Properties.AppearanceDisabled.BackColor = System.Drawing.Color.DarkGray
        Me.txtAkun.Properties.AppearanceDisabled.BackColor2 = System.Drawing.Color.Gray
        Me.txtAkun.Properties.AppearanceDisabled.Options.UseBackColor = True
        Me.txtAkun.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton, New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Close)})
        Me.txtAkun.Properties.ReadOnly = True
        Me.txtAkun.Size = New System.Drawing.Size(255, 22)
        Me.txtAkun.TabIndex = 4
        '
        'PanelControl3
        '
        Me.PanelControl3.Appearance.BackColor = System.Drawing.SystemColors.ButtonFace
        Me.PanelControl3.Appearance.Options.UseBackColor = True
        Me.PanelControl3.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder
        Me.PanelControl3.Controls.Add(Me.cmdExport)
        Me.PanelControl3.Controls.Add(Me.cmdRefresh)
        Me.PanelControl3.Controls.Add(Me.cmdDelete)
        Me.PanelControl3.Controls.Add(Me.cmdEdit)
        Me.PanelControl3.Controls.Add(Me.cmdClose)
        Me.PanelControl3.Controls.Add(Me.cmdNew)
        Me.PanelControl3.Controls.Add(Me.cmdMark)
        Me.PanelControl3.Dock = System.Windows.Forms.DockStyle.Left
        Me.PanelControl3.Location = New System.Drawing.Point(0, 86)
        Me.PanelControl3.Name = "PanelControl3"
        Me.PanelControl3.Size = New System.Drawing.Size(133, 253)
        Me.PanelControl3.TabIndex = 6
        '
        'cmdExport
        '
        Me.cmdExport.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.cmdExport.Appearance.BackColor = System.Drawing.Color.Snow
        Me.cmdExport.Appearance.BackColor2 = System.Drawing.Color.PowderBlue
        Me.cmdExport.Appearance.BorderColor = System.Drawing.SystemColors.ButtonFace
        Me.cmdExport.Appearance.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdExport.Appearance.ForeColor = System.Drawing.Color.Black
        Me.cmdExport.Appearance.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.ForwardDiagonal
        Me.cmdExport.Appearance.Options.UseBackColor = True
        Me.cmdExport.Appearance.Options.UseBorderColor = True
        Me.cmdExport.Appearance.Options.UseFont = True
        Me.cmdExport.Appearance.Options.UseForeColor = True
        Me.cmdExport.Cursor = System.Windows.Forms.Cursors.Hand
        Me.cmdExport.ImageIndex = 14
        Me.cmdExport.Location = New System.Drawing.Point(9, 171)
        Me.cmdExport.Name = "cmdExport"
        Me.cmdExport.Size = New System.Drawing.Size(115, 32)
        Me.cmdExport.TabIndex = 30
        Me.cmdExport.Text = "&Export Excel"
        '
        'cmdRefresh
        '
        Me.cmdRefresh.Appearance.BackColor = System.Drawing.Color.Snow
        Me.cmdRefresh.Appearance.BackColor2 = System.Drawing.Color.PowderBlue
        Me.cmdRefresh.Appearance.BorderColor = System.Drawing.SystemColors.ButtonFace
        Me.cmdRefresh.Appearance.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdRefresh.Appearance.ForeColor = System.Drawing.Color.Black
        Me.cmdRefresh.Appearance.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.ForwardDiagonal
        Me.cmdRefresh.Appearance.Options.UseBackColor = True
        Me.cmdRefresh.Appearance.Options.UseBorderColor = True
        Me.cmdRefresh.Appearance.Options.UseFont = True
        Me.cmdRefresh.Appearance.Options.UseForeColor = True
        Me.cmdRefresh.Cursor = System.Windows.Forms.Cursors.Hand
        Me.cmdRefresh.ImageIndex = 13
        Me.cmdRefresh.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleLeft
        Me.cmdRefresh.Location = New System.Drawing.Point(9, 5)
        Me.cmdRefresh.Name = "cmdRefresh"
        Me.cmdRefresh.Size = New System.Drawing.Size(115, 32)
        Me.cmdRefresh.TabIndex = 29
        Me.cmdRefresh.Text = "&Refresh"
        '
        'cmdDelete
        '
        Me.cmdDelete.Appearance.BackColor = System.Drawing.Color.Snow
        Me.cmdDelete.Appearance.BackColor2 = System.Drawing.Color.PowderBlue
        Me.cmdDelete.Appearance.BorderColor = System.Drawing.SystemColors.ButtonFace
        Me.cmdDelete.Appearance.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdDelete.Appearance.ForeColor = System.Drawing.Color.Black
        Me.cmdDelete.Appearance.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.ForwardDiagonal
        Me.cmdDelete.Appearance.Options.UseBackColor = True
        Me.cmdDelete.Appearance.Options.UseBorderColor = True
        Me.cmdDelete.Appearance.Options.UseFont = True
        Me.cmdDelete.Appearance.Options.UseForeColor = True
        Me.cmdDelete.Cursor = System.Windows.Forms.Cursors.Hand
        Me.cmdDelete.ImageIndex = 10
        Me.cmdDelete.Location = New System.Drawing.Point(9, 125)
        Me.cmdDelete.Name = "cmdDelete"
        Me.cmdDelete.Size = New System.Drawing.Size(115, 32)
        Me.cmdDelete.TabIndex = 28
        Me.cmdDelete.Text = "&Delete"
        '
        'cmdEdit
        '
        Me.cmdEdit.Appearance.BackColor = System.Drawing.Color.Snow
        Me.cmdEdit.Appearance.BackColor2 = System.Drawing.Color.PowderBlue
        Me.cmdEdit.Appearance.BorderColor = System.Drawing.SystemColors.ButtonFace
        Me.cmdEdit.Appearance.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdEdit.Appearance.ForeColor = System.Drawing.Color.Black
        Me.cmdEdit.Appearance.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.ForwardDiagonal
        Me.cmdEdit.Appearance.Options.UseBackColor = True
        Me.cmdEdit.Appearance.Options.UseBorderColor = True
        Me.cmdEdit.Appearance.Options.UseFont = True
        Me.cmdEdit.Appearance.Options.UseForeColor = True
        Me.cmdEdit.Cursor = System.Windows.Forms.Cursors.Hand
        Me.cmdEdit.ImageIndex = 5
        Me.cmdEdit.Location = New System.Drawing.Point(9, 85)
        Me.cmdEdit.Name = "cmdEdit"
        Me.cmdEdit.Size = New System.Drawing.Size(115, 32)
        Me.cmdEdit.TabIndex = 25
        Me.cmdEdit.Text = "&Edit"
        '
        'cmdClose
        '
        Me.cmdClose.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
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
        Me.cmdClose.Location = New System.Drawing.Point(9, 209)
        Me.cmdClose.Name = "cmdClose"
        Me.cmdClose.Size = New System.Drawing.Size(115, 32)
        Me.cmdClose.TabIndex = 27
        Me.cmdClose.Text = "&Close"
        '
        'cmdNew
        '
        Me.cmdNew.Appearance.BackColor = System.Drawing.Color.Snow
        Me.cmdNew.Appearance.BackColor2 = System.Drawing.Color.PowderBlue
        Me.cmdNew.Appearance.BorderColor = System.Drawing.SystemColors.ButtonFace
        Me.cmdNew.Appearance.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdNew.Appearance.ForeColor = System.Drawing.Color.Black
        Me.cmdNew.Appearance.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.ForwardDiagonal
        Me.cmdNew.Appearance.Options.UseBackColor = True
        Me.cmdNew.Appearance.Options.UseBorderColor = True
        Me.cmdNew.Appearance.Options.UseFont = True
        Me.cmdNew.Appearance.Options.UseForeColor = True
        Me.cmdNew.Cursor = System.Windows.Forms.Cursors.Hand
        Me.cmdNew.ImageIndex = 6
        Me.cmdNew.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleLeft
        Me.cmdNew.Location = New System.Drawing.Point(9, 45)
        Me.cmdNew.Name = "cmdNew"
        Me.cmdNew.Size = New System.Drawing.Size(115, 32)
        Me.cmdNew.TabIndex = 26
        Me.cmdNew.Text = "&New"
        '
        'cmdMark
        '
        Me.cmdMark.Appearance.BackColor = System.Drawing.Color.Snow
        Me.cmdMark.Appearance.BackColor2 = System.Drawing.Color.PowderBlue
        Me.cmdMark.Appearance.BorderColor = System.Drawing.SystemColors.ButtonFace
        Me.cmdMark.Appearance.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdMark.Appearance.ForeColor = System.Drawing.Color.Black
        Me.cmdMark.Appearance.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.ForwardDiagonal
        Me.cmdMark.Appearance.Options.UseBackColor = True
        Me.cmdMark.Appearance.Options.UseBorderColor = True
        Me.cmdMark.Appearance.Options.UseFont = True
        Me.cmdMark.Appearance.Options.UseForeColor = True
        Me.cmdMark.ButtonStyle = DevExpress.XtraEditors.Controls.BorderStyles.HotFlat
        Me.cmdMark.Cursor = System.Windows.Forms.Cursors.Hand
        Me.cmdMark.ImageIndex = 9
        Me.cmdMark.Location = New System.Drawing.Point(12, 298)
        Me.cmdMark.Name = "cmdMark"
        Me.cmdMark.Size = New System.Drawing.Size(115, 34)
        Me.cmdMark.TabIndex = 21
        Me.cmdMark.Text = "Mark"
        Me.cmdMark.Visible = False
        '
        'PopupMenu1
        '
        Me.PopupMenu1.LinksPersistInfo.AddRange(New DevExpress.XtraBars.LinkPersistInfo() {New DevExpress.XtraBars.LinkPersistInfo(Me.BarSubItem2), New DevExpress.XtraBars.LinkPersistInfo(Me.BarSubItem5), New DevExpress.XtraBars.LinkPersistInfo(Me.mnHasilPosting)})
        Me.PopupMenu1.Manager = Me.BarManager1
        Me.PopupMenu1.Name = "PopupMenu1"
        '
        'frmDaftarKasIN
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1020, 339)
        Me.Controls.Add(Me.GridControl1)
        Me.Controls.Add(Me.PanelControl3)
        Me.Controls.Add(Me.PanelControl1)
        Me.Controls.Add(Me.barDockControlLeft)
        Me.Controls.Add(Me.barDockControlRight)
        Me.Controls.Add(Me.barDockControlBottom)
        Me.Controls.Add(Me.barDockControlTop)
        Me.Name = "frmDaftarKasIN"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Daftar Kas Masuk"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        CType(Me.GridControl1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.GridView1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RepositoryItemCheckEdit1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RepositoryItemCheckEdit2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PanelControl1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.PanelControl1.ResumeLayout(False)
        Me.PanelControl1.PerformLayout()
        CType(Me.txtkas.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.TglSampai.Properties.VistaTimeProperties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.TglSampai.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.tglDari.Properties.VistaTimeProperties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.tglDari.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.cbStatus.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.BarManager1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RepositoryItemTextEdit1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtAkun.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PanelControl3, System.ComponentModel.ISupportInitialize).EndInit()
        Me.PanelControl3.ResumeLayout(False)
        CType(Me.PopupMenu1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents PanelControl1 As DevExpress.XtraEditors.PanelControl
    Friend WithEvents LabelControl2 As DevExpress.XtraEditors.LabelControl
    Friend WithEvents txtAkun As DevExpress.XtraEditors.ButtonEdit
    Friend WithEvents BarManager1 As DevExpress.XtraBars.BarManager
    Friend WithEvents Bar1 As DevExpress.XtraBars.Bar
    Friend WithEvents BarSubItem1 As DevExpress.XtraBars.BarSubItem
    Friend WithEvents BarSubItem2 As DevExpress.XtraBars.BarSubItem
    Friend WithEvents mnPosting As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents BarSubItem5 As DevExpress.XtraBars.BarSubItem
    Friend WithEvents BarButtonItem10 As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents BarButtonItem11 As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents barDockControlTop As DevExpress.XtraBars.BarDockControl
    Friend WithEvents barDockControlBottom As DevExpress.XtraBars.BarDockControl
    Friend WithEvents barDockControlLeft As DevExpress.XtraBars.BarDockControl
    Friend WithEvents barDockControlRight As DevExpress.XtraBars.BarDockControl
    Friend WithEvents RepositoryItemTextEdit1 As DevExpress.XtraEditors.Repository.RepositoryItemTextEdit
    Friend WithEvents PanelControl3 As DevExpress.XtraEditors.PanelControl
    Friend WithEvents cmdExport As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents cmdRefresh As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents cmdDelete As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents cmdEdit As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents cmdClose As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents cmdNew As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents cmdMark As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents LabelControl3 As DevExpress.XtraEditors.LabelControl
    Friend WithEvents cbStatus As DevExpress.XtraEditors.CheckedComboBoxEdit
    Friend WithEvents LabelControl4 As DevExpress.XtraEditors.LabelControl
    Friend WithEvents LabelControl5 As DevExpress.XtraEditors.LabelControl
    Friend WithEvents TglSampai As DevExpress.XtraEditors.DateEdit
    Friend WithEvents tglDari As DevExpress.XtraEditors.DateEdit
    Friend WithEvents GridControl1 As DevExpress.XtraGrid.GridControl
    Friend WithEvents RepositoryItemCheckEdit1 As DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit
    Friend WithEvents RepositoryItemCheckEdit2 As DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit
    Friend WithEvents PopupMenu1 As DevExpress.XtraBars.PopupMenu
    Friend WithEvents mnUnPosting As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents GridView1 As DevExpress.XtraGrid.Views.Grid.GridView
    Friend WithEvents colID As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colIsPosting As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colKode As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colTanggal As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colKet As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colTotal As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colNamaAkun As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents mnHasilPosting As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents colNoReff As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents txtkas As DevExpress.XtraEditors.ButtonEdit
    Friend WithEvents lbKas As DevExpress.XtraEditors.LabelControl
    Friend WithEvents cNamaKas As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents mnRepairPosting As DevExpress.XtraBars.BarButtonItem
    'Friend WithEvents MkasoutdTableAdapter As ACCT.LintasDataSetTableAdapters.mkasoutdTableAdapter
End Class
