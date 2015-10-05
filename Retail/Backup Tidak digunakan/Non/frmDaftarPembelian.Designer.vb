<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmDaftarPembelian
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmDaftarPembelian))
        Me.PanelControl1 = New DevExpress.XtraEditors.PanelControl
        Me.TglSampai = New DevExpress.XtraEditors.DateEdit
        Me.BarManager1 = New DevExpress.XtraBars.BarManager(Me.components)
        Me.Bar2 = New DevExpress.XtraBars.Bar
        Me.BarSubItem1 = New DevExpress.XtraBars.BarSubItem
        Me.BarButtonItem1 = New DevExpress.XtraBars.BarButtonItem
        Me.BarButtonItem2 = New DevExpress.XtraBars.BarButtonItem
        Me.BarButtonItem3 = New DevExpress.XtraBars.BarButtonItem
        Me.BarButtonItem5 = New DevExpress.XtraBars.BarButtonItem
        Me.BarButtonItem6 = New DevExpress.XtraBars.BarButtonItem
        Me.BarButtonItem4 = New DevExpress.XtraBars.BarButtonItem
        Me.mnFaktur = New DevExpress.XtraBars.BarButtonItem
        Me.mnPosting = New DevExpress.XtraBars.BarButtonItem
        Me.barDockControlTop = New DevExpress.XtraBars.BarDockControl
        Me.barDockControlBottom = New DevExpress.XtraBars.BarDockControl
        Me.barDockControlLeft = New DevExpress.XtraBars.BarDockControl
        Me.barDockControlRight = New DevExpress.XtraBars.BarDockControl
        Me.mnUnPosting = New DevExpress.XtraBars.BarButtonItem
        Me.LabelControl2 = New DevExpress.XtraEditors.LabelControl
        Me.LabelControl1 = New DevExpress.XtraEditors.LabelControl
        Me.TglDari = New DevExpress.XtraEditors.DateEdit
        Me.lbDaftar = New DevExpress.XtraEditors.LabelControl
        Me.PanelControl2 = New DevExpress.XtraEditors.PanelControl
        Me.PanelControl3 = New DevExpress.XtraEditors.PanelControl
        Me.cmdFaktur = New DevExpress.XtraEditors.SimpleButton
        Me.ImageList1 = New System.Windows.Forms.ImageList(Me.components)
        Me.cmdPreview = New DevExpress.XtraEditors.SimpleButton
        Me.cmdExcel = New DevExpress.XtraEditors.SimpleButton
        Me.cmdOK = New DevExpress.XtraEditors.SimpleButton
        Me.cmdTutup = New DevExpress.XtraEditors.SimpleButton
        Me.cmdRefresh = New DevExpress.XtraEditors.SimpleButton
        Me.cmdHapus = New DevExpress.XtraEditors.SimpleButton
        Me.cmdEdit = New DevExpress.XtraEditors.SimpleButton
        Me.cmdBaru = New DevExpress.XtraEditors.SimpleButton
        Me.PanelControl4 = New DevExpress.XtraEditors.PanelControl
        Me.GC1 = New DevExpress.XtraGrid.GridControl
        Me.GV1 = New DevExpress.XtraGrid.Views.Grid.GridView
        Me.PopupMenu1 = New DevExpress.XtraBars.PopupMenu(Me.components)
        CType(Me.PanelControl1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.PanelControl1.SuspendLayout()
        CType(Me.TglSampai.Properties.VistaTimeProperties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.TglSampai.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.BarManager1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.TglDari.Properties.VistaTimeProperties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.TglDari.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PanelControl2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PanelControl3, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.PanelControl3.SuspendLayout()
        CType(Me.PanelControl4, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.PanelControl4.SuspendLayout()
        CType(Me.GC1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.GV1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PopupMenu1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'PanelControl1
        '
        Me.PanelControl1.Controls.Add(Me.TglSampai)
        Me.PanelControl1.Controls.Add(Me.LabelControl2)
        Me.PanelControl1.Controls.Add(Me.LabelControl1)
        Me.PanelControl1.Controls.Add(Me.TglDari)
        Me.PanelControl1.Controls.Add(Me.lbDaftar)
        Me.PanelControl1.Dock = System.Windows.Forms.DockStyle.Top
        Me.PanelControl1.Location = New System.Drawing.Point(0, 22)
        Me.PanelControl1.Name = "PanelControl1"
        Me.PanelControl1.Size = New System.Drawing.Size(1036, 61)
        Me.PanelControl1.TabIndex = 0
        '
        'TglSampai
        '
        Me.TglSampai.EditValue = Nothing
        Me.TglSampai.Location = New System.Drawing.Point(269, 28)
        Me.TglSampai.MenuManager = Me.BarManager1
        Me.TglSampai.Name = "TglSampai"
        Me.TglSampai.Properties.Appearance.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TglSampai.Properties.Appearance.Options.UseFont = True
        Me.TglSampai.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.TglSampai.Properties.Mask.EditMask = "dd-MM-yyyy"
        Me.TglSampai.Properties.Mask.UseMaskAsDisplayFormat = True
        Me.TglSampai.Properties.VistaTimeProperties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton})
        Me.TglSampai.Size = New System.Drawing.Size(162, 22)
        Me.TglSampai.TabIndex = 5
        '
        'BarManager1
        '
        Me.BarManager1.Bars.AddRange(New DevExpress.XtraBars.Bar() {Me.Bar2})
        Me.BarManager1.DockControls.Add(Me.barDockControlTop)
        Me.BarManager1.DockControls.Add(Me.barDockControlBottom)
        Me.BarManager1.DockControls.Add(Me.barDockControlLeft)
        Me.BarManager1.DockControls.Add(Me.barDockControlRight)
        Me.BarManager1.Form = Me
        Me.BarManager1.Items.AddRange(New DevExpress.XtraBars.BarItem() {Me.BarSubItem1, Me.BarButtonItem1, Me.BarButtonItem2, Me.BarButtonItem3, Me.BarButtonItem4, Me.BarButtonItem5, Me.BarButtonItem6, Me.mnFaktur, Me.mnPosting, Me.mnUnPosting})
        Me.BarManager1.MainMenu = Me.Bar2
        Me.BarManager1.MaxItemId = 10
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
        Me.BarSubItem1.LinksPersistInfo.AddRange(New DevExpress.XtraBars.LinkPersistInfo() {New DevExpress.XtraBars.LinkPersistInfo(Me.BarButtonItem1), New DevExpress.XtraBars.LinkPersistInfo(Me.BarButtonItem2), New DevExpress.XtraBars.LinkPersistInfo(Me.BarButtonItem3), New DevExpress.XtraBars.LinkPersistInfo(Me.BarButtonItem5, True), New DevExpress.XtraBars.LinkPersistInfo(Me.BarButtonItem6), New DevExpress.XtraBars.LinkPersistInfo(Me.BarButtonItem4, True), New DevExpress.XtraBars.LinkPersistInfo(Me.mnFaktur), New DevExpress.XtraBars.LinkPersistInfo(Me.mnPosting)})
        Me.BarSubItem1.Name = "BarSubItem1"
        '
        'BarButtonItem1
        '
        Me.BarButtonItem1.Caption = "Baru"
        Me.BarButtonItem1.Id = 1
        Me.BarButtonItem1.ItemShortcut = New DevExpress.XtraBars.BarShortcut(System.Windows.Forms.Keys.F1)
        Me.BarButtonItem1.Name = "BarButtonItem1"
        '
        'BarButtonItem2
        '
        Me.BarButtonItem2.Caption = "Edit"
        Me.BarButtonItem2.Id = 2
        Me.BarButtonItem2.ItemShortcut = New DevExpress.XtraBars.BarShortcut(System.Windows.Forms.Keys.F2)
        Me.BarButtonItem2.Name = "BarButtonItem2"
        '
        'BarButtonItem3
        '
        Me.BarButtonItem3.Caption = "Hapus"
        Me.BarButtonItem3.Id = 3
        Me.BarButtonItem3.ItemShortcut = New DevExpress.XtraBars.BarShortcut(System.Windows.Forms.Keys.F3)
        Me.BarButtonItem3.Name = "BarButtonItem3"
        '
        'BarButtonItem5
        '
        Me.BarButtonItem5.Caption = "Export Excel"
        Me.BarButtonItem5.Id = 5
        Me.BarButtonItem5.ItemShortcut = New DevExpress.XtraBars.BarShortcut(System.Windows.Forms.Keys.F4)
        Me.BarButtonItem5.Name = "BarButtonItem5"
        '
        'BarButtonItem6
        '
        Me.BarButtonItem6.Caption = "Preview"
        Me.BarButtonItem6.Id = 6
        Me.BarButtonItem6.ItemShortcut = New DevExpress.XtraBars.BarShortcut(System.Windows.Forms.Keys.F5)
        Me.BarButtonItem6.Name = "BarButtonItem6"
        '
        'BarButtonItem4
        '
        Me.BarButtonItem4.Caption = "Tutup"
        Me.BarButtonItem4.Id = 4
        Me.BarButtonItem4.ItemShortcut = New DevExpress.XtraBars.BarShortcut(System.Windows.Forms.Keys.F12)
        Me.BarButtonItem4.Name = "BarButtonItem4"
        '
        'mnFaktur
        '
        Me.mnFaktur.Caption = "Faktur"
        Me.mnFaktur.Id = 7
        Me.mnFaktur.ItemShortcut = New DevExpress.XtraBars.BarShortcut(System.Windows.Forms.Keys.F8)
        Me.mnFaktur.Name = "mnFaktur"
        '
        'mnPosting
        '
        Me.mnPosting.Caption = "Posting"
        Me.mnPosting.Id = 8
        Me.mnPosting.ItemShortcut = New DevExpress.XtraBars.BarShortcut(System.Windows.Forms.Keys.F9)
        Me.mnPosting.Name = "mnPosting"
        '
        'barDockControlTop
        '
        Me.barDockControlTop.CausesValidation = False
        Me.barDockControlTop.Dock = System.Windows.Forms.DockStyle.Top
        Me.barDockControlTop.Location = New System.Drawing.Point(0, 0)
        Me.barDockControlTop.Size = New System.Drawing.Size(1036, 22)
        '
        'barDockControlBottom
        '
        Me.barDockControlBottom.CausesValidation = False
        Me.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.barDockControlBottom.Location = New System.Drawing.Point(0, 379)
        Me.barDockControlBottom.Size = New System.Drawing.Size(1036, 0)
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
        Me.barDockControlRight.Location = New System.Drawing.Point(1036, 22)
        Me.barDockControlRight.Size = New System.Drawing.Size(0, 357)
        '
        'mnUnPosting
        '
        Me.mnUnPosting.Caption = "UnPosting"
        Me.mnUnPosting.Id = 9
        Me.mnUnPosting.Name = "mnUnPosting"
        '
        'LabelControl2
        '
        Me.LabelControl2.Appearance.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LabelControl2.Location = New System.Drawing.Point(245, 31)
        Me.LabelControl2.Name = "LabelControl2"
        Me.LabelControl2.Size = New System.Drawing.Size(18, 16)
        Me.LabelControl2.TabIndex = 4
        Me.LabelControl2.Text = "s/d"
        '
        'LabelControl1
        '
        Me.LabelControl1.Appearance.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LabelControl1.Cursor = System.Windows.Forms.Cursors.Hand
        Me.LabelControl1.Location = New System.Drawing.Point(25, 31)
        Me.LabelControl1.Name = "LabelControl1"
        Me.LabelControl1.Size = New System.Drawing.Size(46, 16)
        Me.LabelControl1.TabIndex = 3
        Me.LabelControl1.Text = "Tanggal"
        Me.LabelControl1.ToolTip = "Klik disini untuk mengaktifkan atau menonaktifkan filter Tanggal, klik Refresh un" & _
            "tuk menampilkan hasil filter"
        '
        'TglDari
        '
        Me.TglDari.EditValue = Nothing
        Me.TglDari.Location = New System.Drawing.Point(77, 28)
        Me.TglDari.MenuManager = Me.BarManager1
        Me.TglDari.Name = "TglDari"
        Me.TglDari.Properties.Appearance.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TglDari.Properties.Appearance.Options.UseFont = True
        Me.TglDari.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.TglDari.Properties.Mask.EditMask = "dd-MM-yyyy"
        Me.TglDari.Properties.Mask.UseMaskAsDisplayFormat = True
        Me.TglDari.Properties.VistaTimeProperties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton})
        Me.TglDari.Size = New System.Drawing.Size(162, 22)
        Me.TglDari.TabIndex = 2
        '
        'lbDaftar
        '
        Me.lbDaftar.Appearance.Font = New System.Drawing.Font("Rockwell", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbDaftar.Appearance.ForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.lbDaftar.Location = New System.Drawing.Point(25, 6)
        Me.lbDaftar.Name = "lbDaftar"
        Me.lbDaftar.Size = New System.Drawing.Size(50, 19)
        Me.lbDaftar.TabIndex = 1
        Me.lbDaftar.Text = "Daftar"
        '
        'PanelControl2
        '
        Me.PanelControl2.Dock = System.Windows.Forms.DockStyle.Left
        Me.PanelControl2.Location = New System.Drawing.Point(0, 83)
        Me.PanelControl2.Name = "PanelControl2"
        Me.PanelControl2.Size = New System.Drawing.Size(25, 296)
        Me.PanelControl2.TabIndex = 1
        Me.PanelControl2.Visible = False
        '
        'PanelControl3
        '
        Me.PanelControl3.Controls.Add(Me.cmdFaktur)
        Me.PanelControl3.Controls.Add(Me.cmdPreview)
        Me.PanelControl3.Controls.Add(Me.cmdExcel)
        Me.PanelControl3.Controls.Add(Me.cmdOK)
        Me.PanelControl3.Controls.Add(Me.cmdTutup)
        Me.PanelControl3.Controls.Add(Me.cmdRefresh)
        Me.PanelControl3.Controls.Add(Me.cmdHapus)
        Me.PanelControl3.Controls.Add(Me.cmdEdit)
        Me.PanelControl3.Controls.Add(Me.cmdBaru)
        Me.PanelControl3.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.PanelControl3.Location = New System.Drawing.Point(25, 333)
        Me.PanelControl3.Name = "PanelControl3"
        Me.PanelControl3.Size = New System.Drawing.Size(1011, 46)
        Me.PanelControl3.TabIndex = 2
        '
        'cmdFaktur
        '
        Me.cmdFaktur.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.cmdFaktur.Appearance.Font = New System.Drawing.Font("Tahoma", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdFaktur.Appearance.ForeColor = System.Drawing.Color.Blue
        Me.cmdFaktur.Appearance.Options.UseFont = True
        Me.cmdFaktur.Appearance.Options.UseForeColor = True
        Me.cmdFaktur.ImageIndex = 7
        Me.cmdFaktur.ImageList = Me.ImageList1
        Me.cmdFaktur.Location = New System.Drawing.Point(501, 11)
        Me.cmdFaktur.Name = "cmdFaktur"
        Me.cmdFaktur.Size = New System.Drawing.Size(93, 31)
        Me.cmdFaktur.TabIndex = 13
        Me.cmdFaktur.Text = "&Faktur"
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
        'cmdPreview
        '
        Me.cmdPreview.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.cmdPreview.Appearance.Font = New System.Drawing.Font("Tahoma", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdPreview.Appearance.ForeColor = System.Drawing.Color.Blue
        Me.cmdPreview.Appearance.Options.UseFont = True
        Me.cmdPreview.Appearance.Options.UseForeColor = True
        Me.cmdPreview.ImageIndex = 4
        Me.cmdPreview.ImageList = Me.ImageList1
        Me.cmdPreview.Location = New System.Drawing.Point(402, 11)
        Me.cmdPreview.Name = "cmdPreview"
        Me.cmdPreview.Size = New System.Drawing.Size(93, 31)
        Me.cmdPreview.TabIndex = 11
        Me.cmdPreview.Text = "&Preview"
        '
        'cmdExcel
        '
        Me.cmdExcel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.cmdExcel.Appearance.Font = New System.Drawing.Font("Tahoma", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdExcel.Appearance.ForeColor = System.Drawing.Color.Blue
        Me.cmdExcel.Appearance.Options.UseFont = True
        Me.cmdExcel.Appearance.Options.UseForeColor = True
        Me.cmdExcel.ImageIndex = 3
        Me.cmdExcel.ImageList = Me.ImageList1
        Me.cmdExcel.Location = New System.Drawing.Point(303, 11)
        Me.cmdExcel.Name = "cmdExcel"
        Me.cmdExcel.Size = New System.Drawing.Size(93, 31)
        Me.cmdExcel.TabIndex = 10
        Me.cmdExcel.Text = "&Excel"
        '
        'cmdOK
        '
        Me.cmdOK.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdOK.Appearance.Font = New System.Drawing.Font("Tahoma", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdOK.Appearance.ForeColor = System.Drawing.Color.Blue
        Me.cmdOK.Appearance.Options.UseFont = True
        Me.cmdOK.Appearance.Options.UseForeColor = True
        Me.cmdOK.ImageIndex = 10
        Me.cmdOK.ImageList = Me.ImageList1
        Me.cmdOK.Location = New System.Drawing.Point(800, 11)
        Me.cmdOK.Name = "cmdOK"
        Me.cmdOK.Size = New System.Drawing.Size(100, 31)
        Me.cmdOK.TabIndex = 9
        Me.cmdOK.Text = "&OK"
        Me.cmdOK.Visible = False
        '
        'cmdTutup
        '
        Me.cmdTutup.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdTutup.Appearance.Font = New System.Drawing.Font("Tahoma", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdTutup.Appearance.ForeColor = System.Drawing.Color.Blue
        Me.cmdTutup.Appearance.Options.UseFont = True
        Me.cmdTutup.Appearance.Options.UseForeColor = True
        Me.cmdTutup.ImageIndex = 11
        Me.cmdTutup.ImageList = Me.ImageList1
        Me.cmdTutup.Location = New System.Drawing.Point(906, 10)
        Me.cmdTutup.Name = "cmdTutup"
        Me.cmdTutup.Size = New System.Drawing.Size(100, 31)
        Me.cmdTutup.TabIndex = 8
        Me.cmdTutup.Text = "&Tutup"
        '
        'cmdRefresh
        '
        Me.cmdRefresh.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.cmdRefresh.Appearance.Font = New System.Drawing.Font("Tahoma", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdRefresh.Appearance.ForeColor = System.Drawing.Color.Blue
        Me.cmdRefresh.Appearance.Options.UseFont = True
        Me.cmdRefresh.Appearance.Options.UseForeColor = True
        Me.cmdRefresh.ImageIndex = 9
        Me.cmdRefresh.ImageList = Me.ImageList1
        Me.cmdRefresh.Location = New System.Drawing.Point(600, 11)
        Me.cmdRefresh.Name = "cmdRefresh"
        Me.cmdRefresh.Size = New System.Drawing.Size(93, 31)
        Me.cmdRefresh.TabIndex = 7
        Me.cmdRefresh.Text = "&Refresh"
        '
        'cmdHapus
        '
        Me.cmdHapus.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.cmdHapus.Appearance.Font = New System.Drawing.Font("Tahoma", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdHapus.Appearance.ForeColor = System.Drawing.Color.Blue
        Me.cmdHapus.Appearance.Options.UseFont = True
        Me.cmdHapus.Appearance.Options.UseForeColor = True
        Me.cmdHapus.ImageIndex = 2
        Me.cmdHapus.ImageList = Me.ImageList1
        Me.cmdHapus.Location = New System.Drawing.Point(204, 11)
        Me.cmdHapus.Name = "cmdHapus"
        Me.cmdHapus.Size = New System.Drawing.Size(93, 31)
        Me.cmdHapus.TabIndex = 2
        Me.cmdHapus.Text = "&Hapus"
        '
        'cmdEdit
        '
        Me.cmdEdit.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.cmdEdit.Appearance.Font = New System.Drawing.Font("Tahoma", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdEdit.Appearance.ForeColor = System.Drawing.Color.Blue
        Me.cmdEdit.Appearance.Options.UseFont = True
        Me.cmdEdit.Appearance.Options.UseForeColor = True
        Me.cmdEdit.ImageIndex = 1
        Me.cmdEdit.ImageList = Me.ImageList1
        Me.cmdEdit.Location = New System.Drawing.Point(105, 11)
        Me.cmdEdit.Name = "cmdEdit"
        Me.cmdEdit.Size = New System.Drawing.Size(93, 31)
        Me.cmdEdit.TabIndex = 1
        Me.cmdEdit.Text = "&Edit"
        '
        'cmdBaru
        '
        Me.cmdBaru.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.cmdBaru.Appearance.Font = New System.Drawing.Font("Tahoma", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdBaru.Appearance.ForeColor = System.Drawing.Color.Blue
        Me.cmdBaru.Appearance.Options.UseFont = True
        Me.cmdBaru.Appearance.Options.UseForeColor = True
        Me.cmdBaru.ImageIndex = 0
        Me.cmdBaru.ImageList = Me.ImageList1
        Me.cmdBaru.Location = New System.Drawing.Point(6, 11)
        Me.cmdBaru.Name = "cmdBaru"
        Me.cmdBaru.Size = New System.Drawing.Size(93, 31)
        Me.cmdBaru.TabIndex = 0
        Me.cmdBaru.Text = "&Baru"
        '
        'PanelControl4
        '
        Me.PanelControl4.Controls.Add(Me.GC1)
        Me.PanelControl4.Dock = System.Windows.Forms.DockStyle.Fill
        Me.PanelControl4.Location = New System.Drawing.Point(25, 83)
        Me.PanelControl4.Name = "PanelControl4"
        Me.PanelControl4.Size = New System.Drawing.Size(1011, 250)
        Me.PanelControl4.TabIndex = 3
        '
        'GC1
        '
        Me.GC1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.GC1.Location = New System.Drawing.Point(2, 2)
        Me.GC1.MainView = Me.GV1
        Me.GC1.Name = "GC1"
        Me.GC1.Size = New System.Drawing.Size(1007, 246)
        Me.GC1.TabIndex = 5
        Me.GC1.ViewCollection.AddRange(New DevExpress.XtraGrid.Views.Base.BaseView() {Me.GV1})
        '
        'GV1
        '
        Me.GV1.GridControl = Me.GC1
        Me.GV1.Name = "GV1"
        Me.GV1.OptionsBehavior.AllowIncrementalSearch = True
        Me.GV1.OptionsBehavior.Editable = False
        Me.GV1.OptionsSelection.MultiSelect = True
        Me.GV1.OptionsView.ColumnAutoWidth = False
        Me.GV1.OptionsView.ShowFooter = True
        Me.GV1.OptionsView.ShowGroupPanel = False
        '
        'PopupMenu1
        '
        Me.PopupMenu1.LinksPersistInfo.AddRange(New DevExpress.XtraBars.LinkPersistInfo() {New DevExpress.XtraBars.LinkPersistInfo(Me.mnPosting), New DevExpress.XtraBars.LinkPersistInfo(Me.mnUnPosting)})
        Me.PopupMenu1.Manager = Me.BarManager1
        Me.PopupMenu1.Name = "PopupMenu1"
        '
        'frmDaftarPembelian
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1036, 379)
        Me.Controls.Add(Me.PanelControl4)
        Me.Controls.Add(Me.PanelControl3)
        Me.Controls.Add(Me.PanelControl2)
        Me.Controls.Add(Me.PanelControl1)
        Me.Controls.Add(Me.barDockControlLeft)
        Me.Controls.Add(Me.barDockControlRight)
        Me.Controls.Add(Me.barDockControlBottom)
        Me.Controls.Add(Me.barDockControlTop)
        Me.Name = "frmDaftarPembelian"
        Me.Text = "Daftar Pembelian"
        CType(Me.PanelControl1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.PanelControl1.ResumeLayout(False)
        Me.PanelControl1.PerformLayout()
        CType(Me.TglSampai.Properties.VistaTimeProperties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.TglSampai.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.BarManager1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.TglDari.Properties.VistaTimeProperties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.TglDari.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PanelControl2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PanelControl3, System.ComponentModel.ISupportInitialize).EndInit()
        Me.PanelControl3.ResumeLayout(False)
        CType(Me.PanelControl4, System.ComponentModel.ISupportInitialize).EndInit()
        Me.PanelControl4.ResumeLayout(False)
        CType(Me.GC1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.GV1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PopupMenu1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents PanelControl1 As DevExpress.XtraEditors.PanelControl
    Friend WithEvents PanelControl2 As DevExpress.XtraEditors.PanelControl
    Friend WithEvents PanelControl3 As DevExpress.XtraEditors.PanelControl
    Friend WithEvents PanelControl4 As DevExpress.XtraEditors.PanelControl
    Friend WithEvents GC1 As DevExpress.XtraGrid.GridControl
    Friend WithEvents GV1 As DevExpress.XtraGrid.Views.Grid.GridView
    Friend WithEvents cmdHapus As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents cmdEdit As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents cmdBaru As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents cmdTutup As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents cmdRefresh As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents cmdOK As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents ImageList1 As System.Windows.Forms.ImageList
    Friend WithEvents BarManager1 As DevExpress.XtraBars.BarManager
    Friend WithEvents Bar2 As DevExpress.XtraBars.Bar
    Friend WithEvents BarSubItem1 As DevExpress.XtraBars.BarSubItem
    Friend WithEvents BarButtonItem1 As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents BarButtonItem2 As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents BarButtonItem3 As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents BarButtonItem4 As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents barDockControlTop As DevExpress.XtraBars.BarDockControl
    Friend WithEvents barDockControlBottom As DevExpress.XtraBars.BarDockControl
    Friend WithEvents barDockControlLeft As DevExpress.XtraBars.BarDockControl
    Friend WithEvents barDockControlRight As DevExpress.XtraBars.BarDockControl
    Friend WithEvents cmdPreview As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents cmdExcel As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents BarButtonItem5 As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents BarButtonItem6 As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents lbDaftar As DevExpress.XtraEditors.LabelControl
    Friend WithEvents TglSampai As DevExpress.XtraEditors.DateEdit
    Friend WithEvents LabelControl2 As DevExpress.XtraEditors.LabelControl
    Friend WithEvents LabelControl1 As DevExpress.XtraEditors.LabelControl
    Friend WithEvents TglDari As DevExpress.XtraEditors.DateEdit
    Friend WithEvents cmdFaktur As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents mnFaktur As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents mnPosting As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents PopupMenu1 As DevExpress.XtraBars.PopupMenu
    Friend WithEvents mnUnPosting As DevExpress.XtraBars.BarButtonItem
End Class
