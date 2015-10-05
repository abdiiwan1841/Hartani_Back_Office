<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmGenerateStockOpnamePerKategori
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
        Me.LayoutControl1 = New DevExpress.XtraLayout.LayoutControl
        Me.txtGudang = New DevExpress.XtraEditors.SearchLookUpEdit
        Me.gvGudang = New DevExpress.XtraGrid.Views.Grid.GridView
        Me.ProgressBarControl1 = New DevExpress.XtraEditors.ProgressBarControl
        Me.BarManager1 = New DevExpress.XtraBars.BarManager(Me.components)
        Me.barDockControlTop = New DevExpress.XtraBars.BarDockControl
        Me.barDockControlBottom = New DevExpress.XtraBars.BarDockControl
        Me.barDockControlLeft = New DevExpress.XtraBars.BarDockControl
        Me.barDockControlRight = New DevExpress.XtraBars.BarDockControl
        Me.BarSubItem1 = New DevExpress.XtraBars.BarSubItem
        Me.cmdSaveLayout = New DevExpress.XtraBars.BarButtonItem
        Me.BarButtonItem1 = New DevExpress.XtraBars.BarButtonItem
        Me.BarButtonItem2 = New DevExpress.XtraBars.BarButtonItem
        Me.mnDesain = New DevExpress.XtraBars.BarButtonItem
        Me.txtKode = New DevExpress.XtraEditors.TextEdit
        Me.TglSampai = New DevExpress.XtraEditors.DateEdit
        Me.TglDari = New DevExpress.XtraEditors.DateEdit
        Me.ckAll = New DevExpress.XtraEditors.CheckEdit
        Me.txtKategori = New DevExpress.XtraEditors.SearchLookUpEdit
        Me.gvKategori = New DevExpress.XtraGrid.Views.Grid.GridView
        Me.cmdTutup = New DevExpress.XtraEditors.SimpleButton
        Me.cmdOK = New DevExpress.XtraEditors.SimpleButton
        Me.LayoutControlItem11 = New DevExpress.XtraLayout.LayoutControlItem
        Me.LayoutControlGroup1 = New DevExpress.XtraLayout.LayoutControlGroup
        Me.LayoutControlItem2 = New DevExpress.XtraLayout.LayoutControlItem
        Me.LayoutControlItem1 = New DevExpress.XtraLayout.LayoutControlItem
        Me.LayoutControlItem3 = New DevExpress.XtraLayout.LayoutControlItem
        Me.LayoutControlItem4 = New DevExpress.XtraLayout.LayoutControlItem
        Me.LayoutControlItem5 = New DevExpress.XtraLayout.LayoutControlItem
        Me.LayoutControlItem7 = New DevExpress.XtraLayout.LayoutControlItem
        Me.LayoutControlItem12 = New DevExpress.XtraLayout.LayoutControlItem
        Me.LayoutControlItem13 = New DevExpress.XtraLayout.LayoutControlItem
        CType(Me.LayoutControl1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.LayoutControl1.SuspendLayout()
        CType(Me.txtGudang.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.gvGudang, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.ProgressBarControl1.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.BarManager1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtKode.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.TglSampai.Properties.VistaTimeProperties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.TglSampai.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.TglDari.Properties.VistaTimeProperties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.TglDari.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.ckAll.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtKategori.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.gvKategori, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem11, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlGroup1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem3, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem4, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem5, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem7, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem12, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem13, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'LayoutControl1
        '
        Me.LayoutControl1.Controls.Add(Me.txtGudang)
        Me.LayoutControl1.Controls.Add(Me.ProgressBarControl1)
        Me.LayoutControl1.Controls.Add(Me.txtKode)
        Me.LayoutControl1.Controls.Add(Me.TglSampai)
        Me.LayoutControl1.Controls.Add(Me.TglDari)
        Me.LayoutControl1.Controls.Add(Me.ckAll)
        Me.LayoutControl1.Controls.Add(Me.txtKategori)
        Me.LayoutControl1.Controls.Add(Me.cmdTutup)
        Me.LayoutControl1.Controls.Add(Me.cmdOK)
        Me.LayoutControl1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.LayoutControl1.HiddenItems.AddRange(New DevExpress.XtraLayout.BaseLayoutItem() {Me.LayoutControlItem11})
        Me.LayoutControl1.Location = New System.Drawing.Point(0, 0)
        Me.LayoutControl1.Name = "LayoutControl1"
        Me.LayoutControl1.OptionsCustomizationForm.DesignTimeCustomizationFormPositionAndSize = New System.Drawing.Rectangle(758, 170, 250, 350)
        Me.LayoutControl1.Root = Me.LayoutControlGroup1
        Me.LayoutControl1.Size = New System.Drawing.Size(393, 203)
        Me.LayoutControl1.TabIndex = 0
        Me.LayoutControl1.Text = "LayoutControl1"
        '
        'txtGudang
        '
        Me.txtGudang.EditValue = ""
        Me.txtGudang.EnterMoveNextControl = True
        Me.txtGudang.Location = New System.Drawing.Point(168, 12)
        Me.txtGudang.Name = "txtGudang"
        Me.txtGudang.Properties.Appearance.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtGudang.Properties.Appearance.Options.UseFont = True
        Me.txtGudang.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.txtGudang.Properties.DisplayMember = "Kode"
        Me.txtGudang.Properties.NullText = ""
        Me.txtGudang.Properties.ValueMember = "NoID"
        Me.txtGudang.Properties.View = Me.gvGudang
        Me.txtGudang.Size = New System.Drawing.Size(213, 22)
        Me.txtGudang.StyleController = Me.LayoutControl1
        Me.txtGudang.TabIndex = 31
        '
        'gvGudang
        '
        Me.gvGudang.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus
        Me.gvGudang.Name = "gvGudang"
        Me.gvGudang.OptionsSelection.EnableAppearanceFocusedCell = False
        Me.gvGudang.OptionsView.ShowGroupPanel = False
        '
        'ProgressBarControl1
        '
        Me.ProgressBarControl1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ProgressBarControl1.Location = New System.Drawing.Point(12, 139)
        Me.ProgressBarControl1.MenuManager = Me.BarManager1
        Me.ProgressBarControl1.Name = "ProgressBarControl1"
        Me.ProgressBarControl1.Size = New System.Drawing.Size(369, 18)
        Me.ProgressBarControl1.StyleController = Me.LayoutControl1
        Me.ProgressBarControl1.TabIndex = 21
        '
        'BarManager1
        '
        Me.BarManager1.DockControls.Add(Me.barDockControlTop)
        Me.BarManager1.DockControls.Add(Me.barDockControlBottom)
        Me.BarManager1.DockControls.Add(Me.barDockControlLeft)
        Me.BarManager1.DockControls.Add(Me.barDockControlRight)
        Me.BarManager1.Form = Me
        Me.BarManager1.Items.AddRange(New DevExpress.XtraBars.BarItem() {Me.BarSubItem1, Me.cmdSaveLayout, Me.BarButtonItem1, Me.BarButtonItem2, Me.mnDesain})
        Me.BarManager1.MaxItemId = 5
        '
        'barDockControlTop
        '
        Me.barDockControlTop.CausesValidation = False
        Me.barDockControlTop.Dock = System.Windows.Forms.DockStyle.Top
        Me.barDockControlTop.Location = New System.Drawing.Point(0, 0)
        Me.barDockControlTop.Size = New System.Drawing.Size(393, 0)
        '
        'barDockControlBottom
        '
        Me.barDockControlBottom.CausesValidation = False
        Me.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.barDockControlBottom.Location = New System.Drawing.Point(0, 203)
        Me.barDockControlBottom.Size = New System.Drawing.Size(393, 0)
        '
        'barDockControlLeft
        '
        Me.barDockControlLeft.CausesValidation = False
        Me.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left
        Me.barDockControlLeft.Location = New System.Drawing.Point(0, 0)
        Me.barDockControlLeft.Size = New System.Drawing.Size(0, 203)
        '
        'barDockControlRight
        '
        Me.barDockControlRight.CausesValidation = False
        Me.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right
        Me.barDockControlRight.Location = New System.Drawing.Point(393, 0)
        Me.barDockControlRight.Size = New System.Drawing.Size(0, 203)
        '
        'BarSubItem1
        '
        Me.BarSubItem1.Caption = "&File"
        Me.BarSubItem1.Id = 0
        Me.BarSubItem1.LinksPersistInfo.AddRange(New DevExpress.XtraBars.LinkPersistInfo() {New DevExpress.XtraBars.LinkPersistInfo(Me.cmdSaveLayout), New DevExpress.XtraBars.LinkPersistInfo(Me.BarButtonItem1), New DevExpress.XtraBars.LinkPersistInfo(Me.BarButtonItem2), New DevExpress.XtraBars.LinkPersistInfo(Me.mnDesain)})
        Me.BarSubItem1.Name = "BarSubItem1"
        '
        'cmdSaveLayout
        '
        Me.cmdSaveLayout.Caption = "&Simpan Layout"
        Me.cmdSaveLayout.Id = 1
        Me.cmdSaveLayout.ItemShortcut = New DevExpress.XtraBars.BarShortcut(System.Windows.Forms.Keys.F10)
        Me.cmdSaveLayout.Name = "cmdSaveLayout"
        '
        'BarButtonItem1
        '
        Me.BarButtonItem1.Caption = "Save"
        Me.BarButtonItem1.Id = 2
        Me.BarButtonItem1.ItemShortcut = New DevExpress.XtraBars.BarShortcut(System.Windows.Forms.Keys.F6)
        Me.BarButtonItem1.Name = "BarButtonItem1"
        '
        'BarButtonItem2
        '
        Me.BarButtonItem2.Caption = "Cancel"
        Me.BarButtonItem2.Id = 3
        Me.BarButtonItem2.ItemShortcut = New DevExpress.XtraBars.BarShortcut(System.Windows.Forms.Keys.F3)
        Me.BarButtonItem2.Name = "BarButtonItem2"
        '
        'mnDesain
        '
        Me.mnDesain.Caption = "Desain"
        Me.mnDesain.Id = 4
        Me.mnDesain.ItemShortcut = New DevExpress.XtraBars.BarShortcut(System.Windows.Forms.Keys.F2)
        Me.mnDesain.Name = "mnDesain"
        '
        'txtKode
        '
        Me.txtKode.EnterMoveNextControl = True
        Me.txtKode.Location = New System.Drawing.Point(168, 12)
        Me.txtKode.Name = "txtKode"
        Me.txtKode.Properties.Appearance.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtKode.Properties.Appearance.Options.UseFont = True
        Me.txtKode.Properties.ReadOnly = True
        Me.txtKode.Size = New System.Drawing.Size(196, 22)
        Me.txtKode.StyleController = Me.LayoutControl1
        Me.txtKode.TabIndex = 5
        '
        'TglSampai
        '
        Me.TglSampai.EditValue = Nothing
        Me.TglSampai.Location = New System.Drawing.Point(168, 113)
        Me.TglSampai.MenuManager = Me.BarManager1
        Me.TglSampai.Name = "TglSampai"
        Me.TglSampai.Properties.Appearance.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TglSampai.Properties.Appearance.Options.UseFont = True
        Me.TglSampai.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.TglSampai.Properties.Mask.EditMask = "dd-MM-yyyy"
        Me.TglSampai.Properties.Mask.UseMaskAsDisplayFormat = True
        Me.TglSampai.Properties.VistaTimeProperties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton})
        Me.TglSampai.Size = New System.Drawing.Size(213, 22)
        Me.TglSampai.StyleController = Me.LayoutControl1
        Me.TglSampai.TabIndex = 6
        '
        'TglDari
        '
        Me.TglDari.EditValue = Nothing
        Me.TglDari.Location = New System.Drawing.Point(168, 87)
        Me.TglDari.MenuManager = Me.BarManager1
        Me.TglDari.Name = "TglDari"
        Me.TglDari.Properties.Appearance.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TglDari.Properties.Appearance.Options.UseFont = True
        Me.TglDari.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.TglDari.Properties.Mask.EditMask = "dd-MM-yyyy"
        Me.TglDari.Properties.Mask.UseMaskAsDisplayFormat = True
        Me.TglDari.Properties.VistaTimeProperties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton})
        Me.TglDari.Size = New System.Drawing.Size(213, 22)
        Me.TglDari.StyleController = Me.LayoutControl1
        Me.TglDari.TabIndex = 5
        '
        'ckAll
        '
        Me.ckAll.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ckAll.Location = New System.Drawing.Point(12, 64)
        Me.ckAll.MenuManager = Me.BarManager1
        Me.ckAll.Name = "ckAll"
        Me.ckAll.Properties.Caption = "Cek Item Berdasarkan Nota ini Saja"
        Me.ckAll.Size = New System.Drawing.Size(369, 19)
        Me.ckAll.StyleController = Me.LayoutControl1
        Me.ckAll.TabIndex = 31
        '
        'txtKategori
        '
        Me.txtKategori.EditValue = ""
        Me.txtKategori.EnterMoveNextControl = True
        Me.txtKategori.Location = New System.Drawing.Point(168, 38)
        Me.txtKategori.Name = "txtKategori"
        Me.txtKategori.Properties.Appearance.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtKategori.Properties.Appearance.Options.UseFont = True
        Me.txtKategori.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.txtKategori.Properties.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtKategori.Properties.CloseUpKey = New DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.F3)
        Me.txtKategori.Properties.DisplayMember = "Kode"
        Me.txtKategori.Properties.HideSelection = False
        Me.txtKategori.Properties.NullText = ""
        Me.txtKategori.Properties.PopupFindMode = DevExpress.XtraEditors.FindMode.Always
        Me.txtKategori.Properties.ValidateOnEnterKey = True
        Me.txtKategori.Properties.ValueMember = "NoID"
        Me.txtKategori.Properties.View = Me.gvKategori
        Me.txtKategori.Size = New System.Drawing.Size(213, 22)
        Me.txtKategori.StyleController = Me.LayoutControl1
        Me.txtKategori.TabIndex = 32
        '
        'gvKategori
        '
        Me.gvKategori.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus
        Me.gvKategori.Name = "gvKategori"
        Me.gvKategori.OptionsFind.AlwaysVisible = True
        Me.gvKategori.OptionsFind.FindMode = DevExpress.XtraEditors.FindMode.Always
        Me.gvKategori.OptionsSelection.EnableAppearanceFocusedCell = False
        Me.gvKategori.OptionsView.ShowGroupPanel = False
        Me.gvKategori.ShowButtonMode = DevExpress.XtraGrid.Views.Base.ShowButtonModeEnum.ShowForFocusedRow
        '
        'cmdTutup
        '
        Me.cmdTutup.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdTutup.Appearance.Font = New System.Drawing.Font("Tahoma", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdTutup.Appearance.ForeColor = System.Drawing.Color.Blue
        Me.cmdTutup.Appearance.Options.UseFont = True
        Me.cmdTutup.Appearance.Options.UseForeColor = True
        Me.cmdTutup.ImageIndex = 11
        Me.cmdTutup.Location = New System.Drawing.Point(198, 161)
        Me.cmdTutup.Name = "cmdTutup"
        Me.cmdTutup.Size = New System.Drawing.Size(183, 25)
        Me.cmdTutup.StyleController = Me.LayoutControl1
        Me.cmdTutup.TabIndex = 35
        Me.cmdTutup.Text = "&Cancel"
        '
        'cmdOK
        '
        Me.cmdOK.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdOK.Appearance.Font = New System.Drawing.Font("Tahoma", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdOK.Appearance.ForeColor = System.Drawing.Color.Blue
        Me.cmdOK.Appearance.Options.UseFont = True
        Me.cmdOK.Appearance.Options.UseForeColor = True
        Me.cmdOK.ImageIndex = 10
        Me.cmdOK.Location = New System.Drawing.Point(12, 161)
        Me.cmdOK.Name = "cmdOK"
        Me.cmdOK.Size = New System.Drawing.Size(182, 25)
        Me.cmdOK.StyleController = Me.LayoutControl1
        Me.cmdOK.TabIndex = 20
        Me.cmdOK.Text = "&OK"
        '
        'LayoutControlItem11
        '
        Me.LayoutControlItem11.Control = Me.txtKode
        Me.LayoutControlItem11.CustomizationFormText = "No. Nota Stock Opname"
        Me.LayoutControlItem11.Location = New System.Drawing.Point(0, 0)
        Me.LayoutControlItem11.Name = "LayoutControlItem11"
        Me.LayoutControlItem11.Size = New System.Drawing.Size(356, 26)
        Me.LayoutControlItem11.Text = "No. Nota Stock Opname"
        Me.LayoutControlItem11.TextSize = New System.Drawing.Size(152, 13)
        Me.LayoutControlItem11.TextToControlDistance = 5
        '
        'LayoutControlGroup1
        '
        Me.LayoutControlGroup1.CustomizationFormText = "Root"
        Me.LayoutControlGroup1.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.[True]
        Me.LayoutControlGroup1.GroupBordersVisible = False
        Me.LayoutControlGroup1.Items.AddRange(New DevExpress.XtraLayout.BaseLayoutItem() {Me.LayoutControlItem2, Me.LayoutControlItem1, Me.LayoutControlItem3, Me.LayoutControlItem4, Me.LayoutControlItem5, Me.LayoutControlItem7, Me.LayoutControlItem12, Me.LayoutControlItem13})
        Me.LayoutControlGroup1.Location = New System.Drawing.Point(0, 0)
        Me.LayoutControlGroup1.Name = "Root"
        Me.LayoutControlGroup1.Size = New System.Drawing.Size(393, 203)
        Me.LayoutControlGroup1.Text = "Root"
        Me.LayoutControlGroup1.TextVisible = False
        '
        'LayoutControlItem2
        '
        Me.LayoutControlItem2.Control = Me.cmdOK
        Me.LayoutControlItem2.CustomizationFormText = "LayoutControlItem2"
        Me.LayoutControlItem2.Location = New System.Drawing.Point(0, 149)
        Me.LayoutControlItem2.Name = "LayoutControlItem2"
        Me.LayoutControlItem2.Size = New System.Drawing.Size(186, 34)
        Me.LayoutControlItem2.Text = "LayoutControlItem2"
        Me.LayoutControlItem2.TextSize = New System.Drawing.Size(0, 0)
        Me.LayoutControlItem2.TextToControlDistance = 0
        Me.LayoutControlItem2.TextVisible = False
        '
        'LayoutControlItem1
        '
        Me.LayoutControlItem1.Control = Me.cmdTutup
        Me.LayoutControlItem1.CustomizationFormText = "LayoutControlItem1"
        Me.LayoutControlItem1.Location = New System.Drawing.Point(186, 149)
        Me.LayoutControlItem1.Name = "LayoutControlItem1"
        Me.LayoutControlItem1.Size = New System.Drawing.Size(187, 34)
        Me.LayoutControlItem1.Text = "LayoutControlItem1"
        Me.LayoutControlItem1.TextSize = New System.Drawing.Size(0, 0)
        Me.LayoutControlItem1.TextToControlDistance = 0
        Me.LayoutControlItem1.TextVisible = False
        '
        'LayoutControlItem3
        '
        Me.LayoutControlItem3.Control = Me.txtKategori
        Me.LayoutControlItem3.CustomizationFormText = "Kategori"
        Me.LayoutControlItem3.Location = New System.Drawing.Point(0, 26)
        Me.LayoutControlItem3.Name = "LayoutControlItem3"
        Me.LayoutControlItem3.Size = New System.Drawing.Size(373, 26)
        Me.LayoutControlItem3.Text = "Kategori"
        Me.LayoutControlItem3.TextSize = New System.Drawing.Size(152, 13)
        '
        'LayoutControlItem4
        '
        Me.LayoutControlItem4.Control = Me.ckAll
        Me.LayoutControlItem4.CustomizationFormText = "LayoutControlItem4"
        Me.LayoutControlItem4.Location = New System.Drawing.Point(0, 52)
        Me.LayoutControlItem4.Name = "LayoutControlItem4"
        Me.LayoutControlItem4.Size = New System.Drawing.Size(373, 23)
        Me.LayoutControlItem4.Text = "LayoutControlItem4"
        Me.LayoutControlItem4.TextSize = New System.Drawing.Size(0, 0)
        Me.LayoutControlItem4.TextToControlDistance = 0
        Me.LayoutControlItem4.TextVisible = False
        '
        'LayoutControlItem5
        '
        Me.LayoutControlItem5.Control = Me.TglDari
        Me.LayoutControlItem5.CustomizationFormText = "Cek Stock Opname dari Tanggal"
        Me.LayoutControlItem5.Location = New System.Drawing.Point(0, 75)
        Me.LayoutControlItem5.Name = "LayoutControlItem5"
        Me.LayoutControlItem5.Size = New System.Drawing.Size(373, 26)
        Me.LayoutControlItem5.Text = "Cek Stock Opname dari Tanggal"
        Me.LayoutControlItem5.TextSize = New System.Drawing.Size(152, 13)
        '
        'LayoutControlItem7
        '
        Me.LayoutControlItem7.Control = Me.TglSampai
        Me.LayoutControlItem7.CustomizationFormText = "Sampai Dengan Tanggal"
        Me.LayoutControlItem7.Location = New System.Drawing.Point(0, 101)
        Me.LayoutControlItem7.Name = "LayoutControlItem7"
        Me.LayoutControlItem7.Size = New System.Drawing.Size(373, 26)
        Me.LayoutControlItem7.Text = "Sampai Dengan Tanggal"
        Me.LayoutControlItem7.TextSize = New System.Drawing.Size(152, 13)
        '
        'LayoutControlItem12
        '
        Me.LayoutControlItem12.Control = Me.ProgressBarControl1
        Me.LayoutControlItem12.CustomizationFormText = "LayoutControlItem12"
        Me.LayoutControlItem12.Location = New System.Drawing.Point(0, 127)
        Me.LayoutControlItem12.Name = "LayoutControlItem12"
        Me.LayoutControlItem12.Size = New System.Drawing.Size(373, 22)
        Me.LayoutControlItem12.Text = "LayoutControlItem12"
        Me.LayoutControlItem12.TextSize = New System.Drawing.Size(0, 0)
        Me.LayoutControlItem12.TextToControlDistance = 0
        Me.LayoutControlItem12.TextVisible = False
        '
        'LayoutControlItem13
        '
        Me.LayoutControlItem13.Control = Me.txtGudang
        Me.LayoutControlItem13.CustomizationFormText = "Gudang"
        Me.LayoutControlItem13.Location = New System.Drawing.Point(0, 0)
        Me.LayoutControlItem13.Name = "LayoutControlItem13"
        Me.LayoutControlItem13.Size = New System.Drawing.Size(373, 26)
        Me.LayoutControlItem13.Text = "Gudang"
        Me.LayoutControlItem13.TextSize = New System.Drawing.Size(152, 13)
        '
        'frmGenerateStockOpnamePerKategori
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(393, 203)
        Me.Controls.Add(Me.LayoutControl1)
        Me.Controls.Add(Me.barDockControlLeft)
        Me.Controls.Add(Me.barDockControlRight)
        Me.Controls.Add(Me.barDockControlBottom)
        Me.Controls.Add(Me.barDockControlTop)
        Me.Name = "frmGenerateStockOpnamePerKategori"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Menggenerate Item untuk Stock Opname"
        CType(Me.LayoutControl1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.LayoutControl1.ResumeLayout(False)
        CType(Me.txtGudang.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.gvGudang, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.ProgressBarControl1.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.BarManager1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtKode.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.TglSampai.Properties.VistaTimeProperties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.TglSampai.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.TglDari.Properties.VistaTimeProperties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.TglDari.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.ckAll.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtKategori.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.gvKategori, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem11, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlGroup1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem3, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem4, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem5, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem7, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem12, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem13, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents LayoutControl1 As DevExpress.XtraLayout.LayoutControl
    Friend WithEvents LayoutControlGroup1 As DevExpress.XtraLayout.LayoutControlGroup
    Friend WithEvents cmdOK As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents LayoutControlItem2 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents BarManager1 As DevExpress.XtraBars.BarManager
    Friend WithEvents BarSubItem1 As DevExpress.XtraBars.BarSubItem
    Friend WithEvents cmdSaveLayout As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents barDockControlTop As DevExpress.XtraBars.BarDockControl
    Friend WithEvents barDockControlBottom As DevExpress.XtraBars.BarDockControl
    Friend WithEvents barDockControlLeft As DevExpress.XtraBars.BarDockControl
    Friend WithEvents barDockControlRight As DevExpress.XtraBars.BarDockControl
    Friend WithEvents BarButtonItem1 As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents BarButtonItem2 As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents mnDesain As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents cmdTutup As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents LayoutControlItem1 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents txtKategori As DevExpress.XtraEditors.SearchLookUpEdit
    Friend WithEvents gvKategori As DevExpress.XtraGrid.Views.Grid.GridView
    Friend WithEvents LayoutControlItem3 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents ckAll As DevExpress.XtraEditors.CheckEdit
    Friend WithEvents LayoutControlItem4 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents TglDari As DevExpress.XtraEditors.DateEdit
    Friend WithEvents LayoutControlItem5 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents TglSampai As DevExpress.XtraEditors.DateEdit
    Friend WithEvents LayoutControlItem7 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents txtKode As DevExpress.XtraEditors.TextEdit
    Friend WithEvents LayoutControlItem11 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents ProgressBarControl1 As DevExpress.XtraEditors.ProgressBarControl
    Friend WithEvents LayoutControlItem12 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents txtGudang As DevExpress.XtraEditors.SearchLookUpEdit
    Friend WithEvents gvGudang As DevExpress.XtraGrid.Views.Grid.GridView
    Friend WithEvents LayoutControlItem13 As DevExpress.XtraLayout.LayoutControlItem
End Class
