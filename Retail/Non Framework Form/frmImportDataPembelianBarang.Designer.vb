<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmImportDataPembelianBarang
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmImportDataPembelianBarang))
        Me.PanelControl1 = New DevExpress.XtraEditors.PanelControl
        Me.txtFileName = New DevExpress.XtraEditors.ButtonEdit
        Me.BarManager1 = New DevExpress.XtraBars.BarManager(Me.components)
        Me.Bar2 = New DevExpress.XtraBars.Bar
        Me.BarSubItem1 = New DevExpress.XtraBars.BarSubItem
        Me.BarButtonItem5 = New DevExpress.XtraBars.BarButtonItem
        Me.BarButtonItem6 = New DevExpress.XtraBars.BarButtonItem
        Me.BarButtonItem4 = New DevExpress.XtraBars.BarButtonItem
        Me.mnRefresh = New DevExpress.XtraBars.BarButtonItem
        Me.mnHasilPosting = New DevExpress.XtraBars.BarButtonItem
        Me.barDockControlTop = New DevExpress.XtraBars.BarDockControl
        Me.barDockControlBottom = New DevExpress.XtraBars.BarDockControl
        Me.barDockControlLeft = New DevExpress.XtraBars.BarDockControl
        Me.barDockControlRight = New DevExpress.XtraBars.BarDockControl
        Me.mnSaveLayouts = New DevExpress.XtraBars.BarButtonItem
        Me.mnSetPending = New DevExpress.XtraBars.BarButtonItem
        Me.mnDownload = New DevExpress.XtraBars.BarButtonItem
        Me.mnSetNonPending = New DevExpress.XtraBars.BarButtonItem
        Me.LabelControl3 = New DevExpress.XtraEditors.LabelControl
        Me.LabelControl1 = New DevExpress.XtraEditors.LabelControl
        Me.lbDaftar = New DevExpress.XtraEditors.LabelControl
        Me.PanelControl3 = New DevExpress.XtraEditors.PanelControl
        Me.ProgressBarControl1 = New DevExpress.XtraEditors.ProgressBarControl
        Me.SimpleButton2 = New DevExpress.XtraEditors.SimpleButton
        Me.ImageList1 = New System.Windows.Forms.ImageList(Me.components)
        Me.SimpleButton1 = New DevExpress.XtraEditors.SimpleButton
        Me.SimpleButton5 = New DevExpress.XtraEditors.SimpleButton
        Me.SimpleButton4 = New DevExpress.XtraEditors.SimpleButton
        Me.SimpleButton6 = New DevExpress.XtraEditors.SimpleButton
        Me.SimpleButton8 = New DevExpress.XtraEditors.SimpleButton
        Me.PopupMenu1 = New DevExpress.XtraBars.PopupMenu(Me.components)
        Me.LayoutControlItem19 = New DevExpress.XtraLayout.LayoutControlItem
        Me.LayoutControlItem18 = New DevExpress.XtraLayout.LayoutControlItem
        Me.XtraTabPage1 = New DevExpress.XtraTab.XtraTabPage
        Me.GC1 = New DevExpress.XtraGrid.GridControl
        Me.GV1 = New DevExpress.XtraGrid.Views.Grid.GridView
        Me.GridView1 = New DevExpress.XtraGrid.Views.Grid.GridView
        Me.XtraTabControl1 = New DevExpress.XtraTab.XtraTabControl
        CType(Me.PanelControl1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.PanelControl1.SuspendLayout()
        CType(Me.txtFileName.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.BarManager1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PanelControl3, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.PanelControl3.SuspendLayout()
        CType(Me.ProgressBarControl1.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PopupMenu1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem19, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem18, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.XtraTabPage1.SuspendLayout()
        CType(Me.GC1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.GV1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.GridView1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.XtraTabControl1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.XtraTabControl1.SuspendLayout()
        Me.SuspendLayout()
        '
        'PanelControl1
        '
        Me.PanelControl1.Controls.Add(Me.txtFileName)
        Me.PanelControl1.Controls.Add(Me.LabelControl3)
        Me.PanelControl1.Controls.Add(Me.LabelControl1)
        Me.PanelControl1.Controls.Add(Me.lbDaftar)
        Me.PanelControl1.Dock = System.Windows.Forms.DockStyle.Top
        Me.PanelControl1.Location = New System.Drawing.Point(0, 22)
        Me.PanelControl1.Name = "PanelControl1"
        Me.PanelControl1.Size = New System.Drawing.Size(1209, 54)
        Me.PanelControl1.TabIndex = 0
        '
        'txtFileName
        '
        Me.txtFileName.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtFileName.Location = New System.Drawing.Point(97, 29)
        Me.txtFileName.MenuManager = Me.BarManager1
        Me.txtFileName.Name = "txtFileName"
        Me.txtFileName.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton})
        Me.txtFileName.Size = New System.Drawing.Size(1100, 20)
        Me.txtFileName.TabIndex = 8
        '
        'BarManager1
        '
        Me.BarManager1.Bars.AddRange(New DevExpress.XtraBars.Bar() {Me.Bar2})
        Me.BarManager1.DockControls.Add(Me.barDockControlTop)
        Me.BarManager1.DockControls.Add(Me.barDockControlBottom)
        Me.BarManager1.DockControls.Add(Me.barDockControlLeft)
        Me.BarManager1.DockControls.Add(Me.barDockControlRight)
        Me.BarManager1.Form = Me
        Me.BarManager1.Items.AddRange(New DevExpress.XtraBars.BarItem() {Me.BarSubItem1, Me.BarButtonItem4, Me.BarButtonItem5, Me.BarButtonItem6, Me.mnRefresh, Me.mnHasilPosting, Me.mnSaveLayouts, Me.mnSetPending, Me.mnDownload, Me.mnSetNonPending})
        Me.BarManager1.MainMenu = Me.Bar2
        Me.BarManager1.MaxItemId = 22
        '
        'Bar2
        '
        Me.Bar2.BarName = "Main menu"
        Me.Bar2.DockCol = 0
        Me.Bar2.DockRow = 0
        Me.Bar2.DockStyle = DevExpress.XtraBars.BarDockStyle.Top
        Me.Bar2.LinksPersistInfo.AddRange(New DevExpress.XtraBars.LinkPersistInfo() {New DevExpress.XtraBars.LinkPersistInfo(Me.BarSubItem1)})
        Me.Bar2.OptionsBar.MultiLine = True
        Me.Bar2.OptionsBar.UseWholeRow = True
        Me.Bar2.Text = "Main menu"
        Me.Bar2.Visible = False
        '
        'BarSubItem1
        '
        Me.BarSubItem1.Caption = "Action"
        Me.BarSubItem1.Id = 0
        Me.BarSubItem1.LinksPersistInfo.AddRange(New DevExpress.XtraBars.LinkPersistInfo() {New DevExpress.XtraBars.LinkPersistInfo(Me.BarButtonItem5), New DevExpress.XtraBars.LinkPersistInfo(Me.BarButtonItem6), New DevExpress.XtraBars.LinkPersistInfo(Me.BarButtonItem4, True), New DevExpress.XtraBars.LinkPersistInfo(Me.mnRefresh), New DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.Caption, Me.mnHasilPosting, "Lihat Belanjaan")})
        Me.BarSubItem1.Name = "BarSubItem1"
        '
        'BarButtonItem5
        '
        Me.BarButtonItem5.Caption = "Export Excel"
        Me.BarButtonItem5.Id = 5
        Me.BarButtonItem5.ItemShortcut = New DevExpress.XtraBars.BarShortcut(System.Windows.Forms.Keys.F11)
        Me.BarButtonItem5.Name = "BarButtonItem5"
        '
        'BarButtonItem6
        '
        Me.BarButtonItem6.Caption = "Preview"
        Me.BarButtonItem6.Id = 6
        Me.BarButtonItem6.ItemShortcut = New DevExpress.XtraBars.BarShortcut(System.Windows.Forms.Keys.F8)
        Me.BarButtonItem6.Name = "BarButtonItem6"
        '
        'BarButtonItem4
        '
        Me.BarButtonItem4.Caption = "Tutup"
        Me.BarButtonItem4.Id = 4
        Me.BarButtonItem4.ItemShortcut = New DevExpress.XtraBars.BarShortcut(System.Windows.Forms.Keys.F3)
        Me.BarButtonItem4.Name = "BarButtonItem4"
        '
        'mnRefresh
        '
        Me.mnRefresh.Caption = "&Refresh"
        Me.mnRefresh.Id = 16
        Me.mnRefresh.ItemShortcut = New DevExpress.XtraBars.BarShortcut(System.Windows.Forms.Keys.F5)
        Me.mnRefresh.Name = "mnRefresh"
        '
        'mnHasilPosting
        '
        Me.mnHasilPosting.Caption = "Hasil Posting"
        Me.mnHasilPosting.Id = 17
        Me.mnHasilPosting.ItemShortcut = New DevExpress.XtraBars.BarShortcut(System.Windows.Forms.Keys.F6)
        Me.mnHasilPosting.Name = "mnHasilPosting"
        '
        'barDockControlTop
        '
        Me.barDockControlTop.CausesValidation = False
        Me.barDockControlTop.Dock = System.Windows.Forms.DockStyle.Top
        Me.barDockControlTop.Location = New System.Drawing.Point(0, 0)
        Me.barDockControlTop.Size = New System.Drawing.Size(1209, 22)
        '
        'barDockControlBottom
        '
        Me.barDockControlBottom.CausesValidation = False
        Me.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.barDockControlBottom.Location = New System.Drawing.Point(0, 379)
        Me.barDockControlBottom.Size = New System.Drawing.Size(1209, 0)
        '
        'barDockControlLeft
        '
        Me.barDockControlLeft.CausesValidation = False
        Me.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left
        Me.barDockControlLeft.Location = New System.Drawing.Point(0, 22)
        Me.barDockControlLeft.Size = New System.Drawing.Size(0, 357)
        '
        'barDockControlRight
        '
        Me.barDockControlRight.CausesValidation = False
        Me.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right
        Me.barDockControlRight.Location = New System.Drawing.Point(1209, 22)
        Me.barDockControlRight.Size = New System.Drawing.Size(0, 357)
        '
        'mnSaveLayouts
        '
        Me.mnSaveLayouts.Caption = "Save Layouts"
        Me.mnSaveLayouts.Id = 18
        Me.mnSaveLayouts.ItemShortcut = New DevExpress.XtraBars.BarShortcut((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.F10))
        Me.mnSaveLayouts.Name = "mnSaveLayouts"
        '
        'mnSetPending
        '
        Me.mnSetPending.Caption = "&Set Pending"
        Me.mnSetPending.Id = 19
        Me.mnSetPending.Name = "mnSetPending"
        Me.mnSetPending.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
        '
        'mnDownload
        '
        Me.mnDownload.Caption = "Download Data dari Kassa"
        Me.mnDownload.Id = 20
        Me.mnDownload.Name = "mnDownload"
        Me.mnDownload.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
        '
        'mnSetNonPending
        '
        Me.mnSetNonPending.Caption = "Set Non Pending"
        Me.mnSetNonPending.Id = 21
        Me.mnSetNonPending.Name = "mnSetNonPending"
        Me.mnSetNonPending.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
        '
        'LabelControl3
        '
        Me.LabelControl3.Appearance.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LabelControl3.Location = New System.Drawing.Point(763, 31)
        Me.LabelControl3.Name = "LabelControl3"
        Me.LabelControl3.Size = New System.Drawing.Size(0, 16)
        Me.LabelControl3.TabIndex = 7
        Me.LabelControl3.Visible = False
        '
        'LabelControl1
        '
        Me.LabelControl1.Appearance.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LabelControl1.Cursor = System.Windows.Forms.Cursors.Hand
        Me.LabelControl1.Location = New System.Drawing.Point(25, 30)
        Me.LabelControl1.Name = "LabelControl1"
        Me.LabelControl1.Size = New System.Drawing.Size(66, 16)
        Me.LabelControl1.TabIndex = 3
        Me.LabelControl1.Text = "Nama File :"
        Me.LabelControl1.ToolTip = "Pilih file excel..."
        '
        'lbDaftar
        '
        Me.lbDaftar.Appearance.Font = New System.Drawing.Font("Rockwell", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbDaftar.Appearance.ForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.lbDaftar.Location = New System.Drawing.Point(25, 6)
        Me.lbDaftar.Name = "lbDaftar"
        Me.lbDaftar.Size = New System.Drawing.Size(144, 19)
        Me.lbDaftar.TabIndex = 1
        Me.lbDaftar.Text = "Import Pembelian"
        '
        'PanelControl3
        '
        Me.PanelControl3.Controls.Add(Me.ProgressBarControl1)
        Me.PanelControl3.Controls.Add(Me.SimpleButton2)
        Me.PanelControl3.Controls.Add(Me.SimpleButton1)
        Me.PanelControl3.Controls.Add(Me.SimpleButton5)
        Me.PanelControl3.Controls.Add(Me.SimpleButton4)
        Me.PanelControl3.Controls.Add(Me.SimpleButton6)
        Me.PanelControl3.Controls.Add(Me.SimpleButton8)
        Me.PanelControl3.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.PanelControl3.Location = New System.Drawing.Point(0, 333)
        Me.PanelControl3.Name = "PanelControl3"
        Me.PanelControl3.Size = New System.Drawing.Size(1209, 46)
        Me.PanelControl3.TabIndex = 2
        '
        'ProgressBarControl1
        '
        Me.ProgressBarControl1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ProgressBarControl1.Location = New System.Drawing.Point(253, 19)
        Me.ProgressBarControl1.MenuManager = Me.BarManager1
        Me.ProgressBarControl1.Name = "ProgressBarControl1"
        Me.ProgressBarControl1.Properties.ShowTitle = True
        Me.ProgressBarControl1.Size = New System.Drawing.Size(838, 18)
        Me.ProgressBarControl1.TabIndex = 14
        '
        'SimpleButton2
        '
        Me.SimpleButton2.Appearance.Font = New System.Drawing.Font("Tahoma", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.SimpleButton2.Appearance.ForeColor = System.Drawing.Color.Blue
        Me.SimpleButton2.Appearance.Options.UseFont = True
        Me.SimpleButton2.Appearance.Options.UseForeColor = True
        Me.SimpleButton2.ImageIndex = 1
        Me.SimpleButton2.ImageList = Me.ImageList1
        Me.SimpleButton2.Location = New System.Drawing.Point(121, 6)
        Me.SimpleButton2.Name = "SimpleButton2"
        Me.SimpleButton2.Size = New System.Drawing.Size(126, 31)
        Me.SimpleButton2.TabIndex = 13
        Me.SimpleButton2.Text = "&Import"
        '
        'ImageList1
        '
        Me.ImageList1.ImageStream = CType(resources.GetObject("ImageList1.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.ImageList1.TransparentColor = System.Drawing.Color.Transparent
        Me.ImageList1.Images.SetKeyName(0, "F1.png")
        Me.ImageList1.Images.SetKeyName(1, "F2.png")
        Me.ImageList1.Images.SetKeyName(2, "F3.png")
        Me.ImageList1.Images.SetKeyName(3, "F4.png")
        Me.ImageList1.Images.SetKeyName(4, "F5.png")
        Me.ImageList1.Images.SetKeyName(5, "F6.png")
        Me.ImageList1.Images.SetKeyName(6, "F7.png")
        Me.ImageList1.Images.SetKeyName(7, "F8.png")
        Me.ImageList1.Images.SetKeyName(8, "F9.png")
        Me.ImageList1.Images.SetKeyName(9, "F10.png")
        Me.ImageList1.Images.SetKeyName(10, "F11.png")
        Me.ImageList1.Images.SetKeyName(11, "F12.png")
        '
        'SimpleButton1
        '
        Me.SimpleButton1.Appearance.Font = New System.Drawing.Font("Tahoma", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.SimpleButton1.Appearance.ForeColor = System.Drawing.Color.Blue
        Me.SimpleButton1.Appearance.Options.UseFont = True
        Me.SimpleButton1.Appearance.Options.UseForeColor = True
        Me.SimpleButton1.ImageIndex = 5
        Me.SimpleButton1.ImageList = Me.ImageList1
        Me.SimpleButton1.Location = New System.Drawing.Point(510, 10)
        Me.SimpleButton1.Name = "SimpleButton1"
        Me.SimpleButton1.Size = New System.Drawing.Size(181, 31)
        Me.SimpleButton1.TabIndex = 12
        Me.SimpleButton1.Text = "&Lihat Belanjaan"
        Me.SimpleButton1.Visible = False
        '
        'SimpleButton5
        '
        Me.SimpleButton5.Appearance.Font = New System.Drawing.Font("Tahoma", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.SimpleButton5.Appearance.ForeColor = System.Drawing.Color.Blue
        Me.SimpleButton5.Appearance.Options.UseFont = True
        Me.SimpleButton5.Appearance.Options.UseForeColor = True
        Me.SimpleButton5.ImageIndex = 7
        Me.SimpleButton5.ImageList = Me.ImageList1
        Me.SimpleButton5.Location = New System.Drawing.Point(404, 10)
        Me.SimpleButton5.Name = "SimpleButton5"
        Me.SimpleButton5.Size = New System.Drawing.Size(100, 31)
        Me.SimpleButton5.TabIndex = 11
        Me.SimpleButton5.Text = "&Preview"
        Me.SimpleButton5.Visible = False
        '
        'SimpleButton4
        '
        Me.SimpleButton4.Appearance.Font = New System.Drawing.Font("Tahoma", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.SimpleButton4.Appearance.ForeColor = System.Drawing.Color.Blue
        Me.SimpleButton4.Appearance.Options.UseFont = True
        Me.SimpleButton4.Appearance.Options.UseForeColor = True
        Me.SimpleButton4.ImageIndex = 10
        Me.SimpleButton4.ImageList = Me.ImageList1
        Me.SimpleButton4.Location = New System.Drawing.Point(316, 10)
        Me.SimpleButton4.Name = "SimpleButton4"
        Me.SimpleButton4.Size = New System.Drawing.Size(82, 31)
        Me.SimpleButton4.TabIndex = 10
        Me.SimpleButton4.Text = "&Excel"
        Me.SimpleButton4.Visible = False
        '
        'SimpleButton6
        '
        Me.SimpleButton6.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.SimpleButton6.Appearance.Font = New System.Drawing.Font("Tahoma", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.SimpleButton6.Appearance.ForeColor = System.Drawing.Color.Blue
        Me.SimpleButton6.Appearance.Options.UseFont = True
        Me.SimpleButton6.Appearance.Options.UseForeColor = True
        Me.SimpleButton6.ImageIndex = 2
        Me.SimpleButton6.ImageList = Me.ImageList1
        Me.SimpleButton6.Location = New System.Drawing.Point(1097, 11)
        Me.SimpleButton6.Name = "SimpleButton6"
        Me.SimpleButton6.Size = New System.Drawing.Size(100, 31)
        Me.SimpleButton6.TabIndex = 8
        Me.SimpleButton6.Text = "&Tutup"
        '
        'SimpleButton8
        '
        Me.SimpleButton8.Appearance.Font = New System.Drawing.Font("Tahoma", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.SimpleButton8.Appearance.ForeColor = System.Drawing.Color.Blue
        Me.SimpleButton8.Appearance.Options.UseFont = True
        Me.SimpleButton8.Appearance.Options.UseForeColor = True
        Me.SimpleButton8.ImageIndex = 4
        Me.SimpleButton8.ImageList = Me.ImageList1
        Me.SimpleButton8.Location = New System.Drawing.Point(5, 6)
        Me.SimpleButton8.Name = "SimpleButton8"
        Me.SimpleButton8.Size = New System.Drawing.Size(100, 31)
        Me.SimpleButton8.TabIndex = 7
        Me.SimpleButton8.Text = "&Refresh"
        '
        'PopupMenu1
        '
        Me.PopupMenu1.LinksPersistInfo.AddRange(New DevExpress.XtraBars.LinkPersistInfo() {New DevExpress.XtraBars.LinkPersistInfo(Me.mnHasilPosting), New DevExpress.XtraBars.LinkPersistInfo(Me.mnSetPending, True), New DevExpress.XtraBars.LinkPersistInfo(Me.mnSetNonPending), New DevExpress.XtraBars.LinkPersistInfo(Me.mnDownload, True)})
        Me.PopupMenu1.Manager = Me.BarManager1
        Me.PopupMenu1.Name = "PopupMenu1"
        '
        'LayoutControlItem19
        '
        Me.LayoutControlItem19.CustomizationFormText = "Kode Customer"
        Me.LayoutControlItem19.Location = New System.Drawing.Point(548, 0)
        Me.LayoutControlItem19.Name = "LayoutControlItem19"
        Me.LayoutControlItem19.Size = New System.Drawing.Size(347, 26)
        Me.LayoutControlItem19.Text = "Kode Customer"
        Me.LayoutControlItem19.TextSize = New System.Drawing.Size(83, 13)
        Me.LayoutControlItem19.TextToControlDistance = 5
        '
        'LayoutControlItem18
        '
        Me.LayoutControlItem18.CustomizationFormText = "Nama Customer"
        Me.LayoutControlItem18.Location = New System.Drawing.Point(548, 26)
        Me.LayoutControlItem18.Name = "LayoutControlItem18"
        Me.LayoutControlItem18.Size = New System.Drawing.Size(347, 26)
        Me.LayoutControlItem18.Text = "Nama Customer"
        Me.LayoutControlItem18.TextSize = New System.Drawing.Size(83, 13)
        Me.LayoutControlItem18.TextToControlDistance = 5
        '
        'XtraTabPage1
        '
        Me.XtraTabPage1.Controls.Add(Me.GC1)
        Me.XtraTabPage1.Name = "XtraTabPage1"
        Me.XtraTabPage1.Size = New System.Drawing.Size(1203, 231)
        Me.XtraTabPage1.Text = "View Data"
        '
        'GC1
        '
        Me.GC1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.GC1.Location = New System.Drawing.Point(0, 0)
        Me.GC1.MainView = Me.GV1
        Me.GC1.Name = "GC1"
        Me.GC1.Size = New System.Drawing.Size(1203, 231)
        Me.GC1.TabIndex = 5
        Me.GC1.ViewCollection.AddRange(New DevExpress.XtraGrid.Views.Base.BaseView() {Me.GV1, Me.GridView1})
        '
        'GV1
        '
        Me.GV1.GridControl = Me.GC1
        Me.GV1.Name = "GV1"
        Me.GV1.OptionsBehavior.AllowIncrementalSearch = True
        Me.GV1.OptionsBehavior.Editable = False
        Me.GV1.OptionsCustomization.AllowSort = False
        Me.GV1.OptionsSelection.MultiSelect = True
        Me.GV1.OptionsView.ColumnAutoWidth = False
        Me.GV1.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never
        Me.GV1.OptionsView.ShowFooter = True
        Me.GV1.OptionsView.ShowGroupPanel = False
        '
        'GridView1
        '
        Me.GridView1.GridControl = Me.GC1
        Me.GridView1.Name = "GridView1"
        '
        'XtraTabControl1
        '
        Me.XtraTabControl1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.XtraTabControl1.Location = New System.Drawing.Point(0, 76)
        Me.XtraTabControl1.Name = "XtraTabControl1"
        Me.XtraTabControl1.SelectedTabPage = Me.XtraTabPage1
        Me.XtraTabControl1.Size = New System.Drawing.Size(1209, 257)
        Me.XtraTabControl1.TabIndex = 13
        Me.XtraTabControl1.TabPages.AddRange(New DevExpress.XtraTab.XtraTabPage() {Me.XtraTabPage1})
        '
        'frmImportDataPembelianBarang
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1209, 379)
        Me.Controls.Add(Me.XtraTabControl1)
        Me.Controls.Add(Me.PanelControl3)
        Me.Controls.Add(Me.PanelControl1)
        Me.Controls.Add(Me.barDockControlLeft)
        Me.Controls.Add(Me.barDockControlRight)
        Me.Controls.Add(Me.barDockControlBottom)
        Me.Controls.Add(Me.barDockControlTop)
        Me.Name = "frmImportDataPembelianBarang"
        Me.Text = "Import Pembelian"
        CType(Me.PanelControl1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.PanelControl1.ResumeLayout(False)
        Me.PanelControl1.PerformLayout()
        CType(Me.txtFileName.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.BarManager1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PanelControl3, System.ComponentModel.ISupportInitialize).EndInit()
        Me.PanelControl3.ResumeLayout(False)
        CType(Me.ProgressBarControl1.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PopupMenu1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem19, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem18, System.ComponentModel.ISupportInitialize).EndInit()
        Me.XtraTabPage1.ResumeLayout(False)
        CType(Me.GC1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.GV1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.GridView1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.XtraTabControl1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.XtraTabControl1.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents PanelControl1 As DevExpress.XtraEditors.PanelControl
    Friend WithEvents PanelControl3 As DevExpress.XtraEditors.PanelControl
    Friend WithEvents SimpleButton6 As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents SimpleButton8 As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents ImageList1 As System.Windows.Forms.ImageList
    Friend WithEvents BarManager1 As DevExpress.XtraBars.BarManager
    Friend WithEvents Bar2 As DevExpress.XtraBars.Bar
    Friend WithEvents BarSubItem1 As DevExpress.XtraBars.BarSubItem
    Friend WithEvents BarButtonItem4 As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents barDockControlTop As DevExpress.XtraBars.BarDockControl
    Friend WithEvents barDockControlBottom As DevExpress.XtraBars.BarDockControl
    Friend WithEvents barDockControlLeft As DevExpress.XtraBars.BarDockControl
    Friend WithEvents barDockControlRight As DevExpress.XtraBars.BarDockControl
    Friend WithEvents SimpleButton5 As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents SimpleButton4 As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents BarButtonItem5 As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents BarButtonItem6 As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents lbDaftar As DevExpress.XtraEditors.LabelControl
    Friend WithEvents LabelControl1 As DevExpress.XtraEditors.LabelControl
    Friend WithEvents PopupMenu1 As DevExpress.XtraBars.PopupMenu
    Friend WithEvents mnRefresh As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents mnHasilPosting As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents mnSaveLayouts As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents LabelControl3 As DevExpress.XtraEditors.LabelControl
    Friend WithEvents LayoutControlItem19 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents LayoutControlItem18 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents mnSetPending As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents mnDownload As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents mnSetNonPending As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents SimpleButton1 As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents SimpleButton2 As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents txtFileName As DevExpress.XtraEditors.ButtonEdit
    Friend WithEvents XtraTabControl1 As DevExpress.XtraTab.XtraTabControl
    Friend WithEvents XtraTabPage1 As DevExpress.XtraTab.XtraTabPage
    Friend WithEvents GC1 As DevExpress.XtraGrid.GridControl
    Friend WithEvents GV1 As DevExpress.XtraGrid.Views.Grid.GridView
    Friend WithEvents GridView1 As DevExpress.XtraGrid.Views.Grid.GridView
    Friend WithEvents ProgressBarControl1 As DevExpress.XtraEditors.ProgressBarControl
End Class
