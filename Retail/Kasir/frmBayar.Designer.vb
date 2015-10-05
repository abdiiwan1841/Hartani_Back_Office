<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmBayar
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
        Me.LabelControl1 = New DevExpress.XtraEditors.LabelControl
        Me.txtSubtotal = New DevExpress.XtraEditors.TextEdit
        Me.LabelControl2 = New DevExpress.XtraEditors.LabelControl
        Me.txtTunai = New DevExpress.XtraEditors.TextEdit
        Me.LabelControl9 = New DevExpress.XtraEditors.LabelControl
        Me.LabelControl3 = New DevExpress.XtraEditors.LabelControl
        Me.txtKembali = New DevExpress.XtraEditors.TextEdit
        Me.LabelControl4 = New DevExpress.XtraEditors.LabelControl
        Me.txtCC = New DevExpress.XtraEditors.TextEdit
        Me.LabelControl5 = New DevExpress.XtraEditors.LabelControl
        Me.txtDC = New DevExpress.XtraEditors.TextEdit
        Me.LabelControl6 = New DevExpress.XtraEditors.LabelControl
        Me.LabelControl7 = New DevExpress.XtraEditors.LabelControl
        Me.txtBankCC = New DevExpress.XtraEditors.SearchLookUpEdit
        Me.gvBankCC = New DevExpress.XtraGrid.Views.Grid.GridView
        Me.txtBankDC = New DevExpress.XtraEditors.SearchLookUpEdit
        Me.gvBankDC = New DevExpress.XtraGrid.Views.Grid.GridView
        Me.txtNoCC = New DevExpress.XtraEditors.TextEdit
        Me.txtNoDC = New DevExpress.XtraEditors.TextEdit
        Me.BarManager1 = New DevExpress.XtraBars.BarManager(Me.components)
        Me.Bar2 = New DevExpress.XtraBars.Bar
        Me.BarSubItem1 = New DevExpress.XtraBars.BarSubItem
        Me.cmdSimpan = New DevExpress.XtraBars.BarButtonItem
        Me.mnSaveLayOuts = New DevExpress.XtraBars.BarButtonItem
        Me.cmdCancel = New DevExpress.XtraBars.BarButtonItem
        Me.Bar3 = New DevExpress.XtraBars.Bar
        Me.barDockControlTop = New DevExpress.XtraBars.BarDockControl
        Me.barDockControlBottom = New DevExpress.XtraBars.BarDockControl
        Me.barDockControlLeft = New DevExpress.XtraBars.BarDockControl
        Me.barDockControlRight = New DevExpress.XtraBars.BarDockControl
        Me.txtChargeDC = New DevExpress.XtraEditors.TextEdit
        Me.txtChargeCC = New DevExpress.XtraEditors.TextEdit
        Me.txtBK1 = New DevExpress.XtraEditors.TextEdit
        Me.txtBK2 = New DevExpress.XtraEditors.TextEdit
        Me.txtRounding = New DevExpress.XtraEditors.TextEdit
        Me.LabelControl8 = New DevExpress.XtraEditors.LabelControl
        CType(Me.txtSubtotal.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtTunai.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtKembali.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtCC.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtDC.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtBankCC.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.gvBankCC, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtBankDC.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.gvBankDC, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtNoCC.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtNoDC.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.BarManager1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtChargeDC.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtChargeCC.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtBK1.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtBK2.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtRounding.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'LabelControl1
        '
        Me.LabelControl1.Appearance.Font = New System.Drawing.Font("Tahoma", 24.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LabelControl1.Location = New System.Drawing.Point(12, 31)
        Me.LabelControl1.Name = "LabelControl1"
        Me.LabelControl1.Size = New System.Drawing.Size(96, 39)
        Me.LabelControl1.TabIndex = 19
        Me.LabelControl1.Text = "TOTAL"
        '
        'txtSubtotal
        '
        Me.txtSubtotal.EditValue = New Decimal(New Integer() {0, 0, 0, 0})
        Me.txtSubtotal.EnterMoveNextControl = True
        Me.txtSubtotal.Location = New System.Drawing.Point(114, 28)
        Me.txtSubtotal.Name = "txtSubtotal"
        Me.txtSubtotal.Properties.Appearance.BackColor = System.Drawing.Color.Yellow
        Me.txtSubtotal.Properties.Appearance.Font = New System.Drawing.Font("Tahoma", 24.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtSubtotal.Properties.Appearance.Options.UseBackColor = True
        Me.txtSubtotal.Properties.Appearance.Options.UseFont = True
        Me.txtSubtotal.Properties.Mask.EditMask = "n2"
        Me.txtSubtotal.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric
        Me.txtSubtotal.Properties.Mask.UseMaskAsDisplayFormat = True
        Me.txtSubtotal.Properties.ReadOnly = True
        Me.txtSubtotal.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.txtSubtotal.Size = New System.Drawing.Size(597, 45)
        Me.txtSubtotal.TabIndex = 20
        '
        'LabelControl2
        '
        Me.LabelControl2.Appearance.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LabelControl2.Location = New System.Drawing.Point(47, 119)
        Me.LabelControl2.Name = "LabelControl2"
        Me.LabelControl2.Size = New System.Drawing.Size(32, 16)
        Me.LabelControl2.TabIndex = 2
        Me.LabelControl2.Text = "Tunai"
        '
        'txtTunai
        '
        Me.txtTunai.EditValue = New Decimal(New Integer() {0, 0, 0, 0})
        Me.txtTunai.EnterMoveNextControl = True
        Me.txtTunai.Location = New System.Drawing.Point(85, 116)
        Me.txtTunai.Name = "txtTunai"
        Me.txtTunai.Properties.Appearance.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtTunai.Properties.Appearance.Options.UseFont = True
        Me.txtTunai.Properties.Mask.EditMask = "n2"
        Me.txtTunai.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric
        Me.txtTunai.Properties.Mask.UseMaskAsDisplayFormat = True
        Me.txtTunai.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.txtTunai.Size = New System.Drawing.Size(626, 22)
        Me.txtTunai.TabIndex = 3
        '
        'LabelControl9
        '
        Me.LabelControl9.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.LabelControl9.Appearance.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(0, Byte), Integer))
        Me.LabelControl9.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
        Me.LabelControl9.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder
        Me.LabelControl9.Location = New System.Drawing.Point(-97, 79)
        Me.LabelControl9.Name = "LabelControl9"
        Me.LabelControl9.Size = New System.Drawing.Size(1027, 3)
        Me.LabelControl9.TabIndex = 13
        '
        'LabelControl3
        '
        Me.LabelControl3.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.LabelControl3.Appearance.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(0, Byte), Integer))
        Me.LabelControl3.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
        Me.LabelControl3.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder
        Me.LabelControl3.Location = New System.Drawing.Point(-125, 200)
        Me.LabelControl3.Name = "LabelControl3"
        Me.LabelControl3.Size = New System.Drawing.Size(1027, 3)
        Me.LabelControl3.TabIndex = 15
        '
        'txtKembali
        '
        Me.txtKembali.EditValue = New Decimal(New Integer() {0, 0, 0, 0})
        Me.txtKembali.EnterMoveNextControl = True
        Me.txtKembali.Location = New System.Drawing.Point(146, 209)
        Me.txtKembali.Name = "txtKembali"
        Me.txtKembali.Properties.Appearance.BackColor = System.Drawing.Color.WhiteSmoke
        Me.txtKembali.Properties.Appearance.Font = New System.Drawing.Font("Tahoma", 24.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtKembali.Properties.Appearance.Options.UseBackColor = True
        Me.txtKembali.Properties.Appearance.Options.UseFont = True
        Me.txtKembali.Properties.Mask.EditMask = "n2"
        Me.txtKembali.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric
        Me.txtKembali.Properties.Mask.UseMaskAsDisplayFormat = True
        Me.txtKembali.Properties.ReadOnly = True
        Me.txtKembali.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.txtKembali.Size = New System.Drawing.Size(565, 45)
        Me.txtKembali.TabIndex = 17
        '
        'LabelControl4
        '
        Me.LabelControl4.Appearance.Font = New System.Drawing.Font("Tahoma", 24.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LabelControl4.Location = New System.Drawing.Point(12, 212)
        Me.LabelControl4.Name = "LabelControl4"
        Me.LabelControl4.Size = New System.Drawing.Size(128, 39)
        Me.LabelControl4.TabIndex = 16
        Me.LabelControl4.Text = "KEMBALI"
        '
        'txtCC
        '
        Me.txtCC.EditValue = New Decimal(New Integer() {0, 0, 0, 0})
        Me.txtCC.EnterMoveNextControl = True
        Me.txtCC.Location = New System.Drawing.Point(355, 144)
        Me.txtCC.Name = "txtCC"
        Me.txtCC.Properties.Appearance.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtCC.Properties.Appearance.Options.UseFont = True
        Me.txtCC.Properties.Mask.EditMask = "n2"
        Me.txtCC.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric
        Me.txtCC.Properties.Mask.UseMaskAsDisplayFormat = True
        Me.txtCC.Properties.ReadOnly = True
        Me.txtCC.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.txtCC.Size = New System.Drawing.Size(143, 22)
        Me.txtCC.TabIndex = 7
        '
        'LabelControl5
        '
        Me.LabelControl5.Appearance.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LabelControl5.Location = New System.Drawing.Point(12, 147)
        Me.LabelControl5.Name = "LabelControl5"
        Me.LabelControl5.Size = New System.Drawing.Size(67, 16)
        Me.LabelControl5.TabIndex = 4
        Me.LabelControl5.Text = "Kartu Kredit"
        '
        'txtDC
        '
        Me.txtDC.EditValue = New Decimal(New Integer() {0, 0, 0, 0})
        Me.txtDC.EnterMoveNextControl = True
        Me.txtDC.Location = New System.Drawing.Point(355, 172)
        Me.txtDC.Name = "txtDC"
        Me.txtDC.Properties.Appearance.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtDC.Properties.Appearance.Options.UseFont = True
        Me.txtDC.Properties.Mask.EditMask = "n2"
        Me.txtDC.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric
        Me.txtDC.Properties.Mask.UseMaskAsDisplayFormat = True
        Me.txtDC.Properties.ReadOnly = True
        Me.txtDC.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.txtDC.Size = New System.Drawing.Size(143, 22)
        Me.txtDC.TabIndex = 13
        '
        'LabelControl6
        '
        Me.LabelControl6.Appearance.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LabelControl6.Location = New System.Drawing.Point(12, 175)
        Me.LabelControl6.Name = "LabelControl6"
        Me.LabelControl6.Size = New System.Drawing.Size(67, 16)
        Me.LabelControl6.TabIndex = 10
        Me.LabelControl6.Text = "Kartu Debet"
        '
        'LabelControl7
        '
        Me.LabelControl7.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.LabelControl7.Appearance.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LabelControl7.Location = New System.Drawing.Point(551, 269)
        Me.LabelControl7.Name = "LabelControl7"
        Me.LabelControl7.Size = New System.Drawing.Size(160, 16)
        Me.LabelControl7.TabIndex = 18
        Me.LabelControl7.Text = "END : SIMPAN, DEL : BATAL"
        '
        'txtBankCC
        '
        Me.txtBankCC.EditValue = ""
        Me.txtBankCC.EnterMoveNextControl = True
        Me.txtBankCC.Location = New System.Drawing.Point(85, 144)
        Me.txtBankCC.Name = "txtBankCC"
        Me.txtBankCC.Properties.Appearance.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtBankCC.Properties.Appearance.Options.UseFont = True
        Me.txtBankCC.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.txtBankCC.Properties.DisplayMember = "Kode"
        Me.txtBankCC.Properties.NullText = ""
        Me.txtBankCC.Properties.ValueMember = "NoID"
        Me.txtBankCC.Properties.View = Me.gvBankCC
        Me.txtBankCC.Size = New System.Drawing.Size(109, 22)
        Me.txtBankCC.TabIndex = 5
        '
        'gvBankCC
        '
        Me.gvBankCC.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus
        Me.gvBankCC.Name = "gvBankCC"
        Me.gvBankCC.OptionsSelection.EnableAppearanceFocusedCell = False
        Me.gvBankCC.OptionsView.ShowGroupPanel = False
        '
        'txtBankDC
        '
        Me.txtBankDC.EditValue = ""
        Me.txtBankDC.EnterMoveNextControl = True
        Me.txtBankDC.Location = New System.Drawing.Point(85, 172)
        Me.txtBankDC.Name = "txtBankDC"
        Me.txtBankDC.Properties.Appearance.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtBankDC.Properties.Appearance.Options.UseFont = True
        Me.txtBankDC.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.txtBankDC.Properties.DisplayMember = "Kode"
        Me.txtBankDC.Properties.NullText = ""
        Me.txtBankDC.Properties.ValueMember = "NoID"
        Me.txtBankDC.Properties.View = Me.gvBankDC
        Me.txtBankDC.Size = New System.Drawing.Size(109, 22)
        Me.txtBankDC.TabIndex = 11
        '
        'gvBankDC
        '
        Me.gvBankDC.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus
        Me.gvBankDC.Name = "gvBankDC"
        Me.gvBankDC.OptionsSelection.EnableAppearanceFocusedCell = False
        Me.gvBankDC.OptionsView.ShowGroupPanel = False
        '
        'txtNoCC
        '
        Me.txtNoCC.EnterMoveNextControl = True
        Me.txtNoCC.Location = New System.Drawing.Point(200, 144)
        Me.txtNoCC.Name = "txtNoCC"
        Me.txtNoCC.Properties.Appearance.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtNoCC.Properties.Appearance.Options.UseFont = True
        Me.txtNoCC.Size = New System.Drawing.Size(149, 22)
        Me.txtNoCC.TabIndex = 6
        '
        'txtNoDC
        '
        Me.txtNoDC.EnterMoveNextControl = True
        Me.txtNoDC.Location = New System.Drawing.Point(200, 172)
        Me.txtNoDC.Name = "txtNoDC"
        Me.txtNoDC.Properties.Appearance.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtNoDC.Properties.Appearance.Options.UseFont = True
        Me.txtNoDC.Size = New System.Drawing.Size(149, 22)
        Me.txtNoDC.TabIndex = 12
        '
        'BarManager1
        '
        Me.BarManager1.Bars.AddRange(New DevExpress.XtraBars.Bar() {Me.Bar2, Me.Bar3})
        Me.BarManager1.DockControls.Add(Me.barDockControlTop)
        Me.BarManager1.DockControls.Add(Me.barDockControlBottom)
        Me.BarManager1.DockControls.Add(Me.barDockControlLeft)
        Me.BarManager1.DockControls.Add(Me.barDockControlRight)
        Me.BarManager1.Form = Me
        Me.BarManager1.Items.AddRange(New DevExpress.XtraBars.BarItem() {Me.BarSubItem1, Me.cmdSimpan, Me.mnSaveLayOuts, Me.cmdCancel})
        Me.BarManager1.MainMenu = Me.Bar2
        Me.BarManager1.MaxItemId = 12
        Me.BarManager1.StatusBar = Me.Bar3
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
        Me.BarSubItem1.Caption = "&Menu"
        Me.BarSubItem1.Id = 0
        Me.BarSubItem1.LinksPersistInfo.AddRange(New DevExpress.XtraBars.LinkPersistInfo() {New DevExpress.XtraBars.LinkPersistInfo(Me.cmdSimpan), New DevExpress.XtraBars.LinkPersistInfo(Me.mnSaveLayOuts), New DevExpress.XtraBars.LinkPersistInfo(Me.cmdCancel)})
        Me.BarSubItem1.Name = "BarSubItem1"
        '
        'cmdSimpan
        '
        Me.cmdSimpan.Caption = "&Simpan"
        Me.cmdSimpan.Id = 1
        Me.cmdSimpan.ItemShortcut = New DevExpress.XtraBars.BarShortcut(System.Windows.Forms.Keys.[End])
        Me.cmdSimpan.Name = "cmdSimpan"
        '
        'mnSaveLayOuts
        '
        Me.mnSaveLayOuts.Caption = "&Simpan Layout"
        Me.mnSaveLayOuts.Id = 3
        Me.mnSaveLayOuts.ItemShortcut = New DevExpress.XtraBars.BarShortcut(System.Windows.Forms.Keys.F10)
        Me.mnSaveLayOuts.Name = "mnSaveLayOuts"
        '
        'cmdCancel
        '
        Me.cmdCancel.Caption = "&Cancel"
        Me.cmdCancel.Id = 11
        Me.cmdCancel.ItemShortcut = New DevExpress.XtraBars.BarShortcut(System.Windows.Forms.Keys.Delete)
        Me.cmdCancel.Name = "cmdCancel"
        '
        'Bar3
        '
        Me.Bar3.BarName = "Status bar"
        Me.Bar3.CanDockStyle = DevExpress.XtraBars.BarCanDockStyle.Bottom
        Me.Bar3.DockCol = 0
        Me.Bar3.DockRow = 0
        Me.Bar3.DockStyle = DevExpress.XtraBars.BarDockStyle.Bottom
        Me.Bar3.OptionsBar.AllowQuickCustomization = False
        Me.Bar3.OptionsBar.DrawDragBorder = False
        Me.Bar3.OptionsBar.UseWholeRow = True
        Me.Bar3.Text = "Status bar"
        Me.Bar3.Visible = False
        '
        'barDockControlTop
        '
        Me.barDockControlTop.CausesValidation = False
        Me.barDockControlTop.Dock = System.Windows.Forms.DockStyle.Top
        Me.barDockControlTop.Location = New System.Drawing.Point(0, 0)
        Me.barDockControlTop.Size = New System.Drawing.Size(723, 22)
        '
        'barDockControlBottom
        '
        Me.barDockControlBottom.CausesValidation = False
        Me.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.barDockControlBottom.Location = New System.Drawing.Point(0, 291)
        Me.barDockControlBottom.Size = New System.Drawing.Size(723, 23)
        '
        'barDockControlLeft
        '
        Me.barDockControlLeft.CausesValidation = False
        Me.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left
        Me.barDockControlLeft.Location = New System.Drawing.Point(0, 22)
        Me.barDockControlLeft.Size = New System.Drawing.Size(0, 269)
        '
        'barDockControlRight
        '
        Me.barDockControlRight.CausesValidation = False
        Me.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right
        Me.barDockControlRight.Location = New System.Drawing.Point(723, 22)
        Me.barDockControlRight.Size = New System.Drawing.Size(0, 269)
        '
        'txtChargeDC
        '
        Me.txtChargeDC.EditValue = New Decimal(New Integer() {0, 0, 0, 0})
        Me.txtChargeDC.EnterMoveNextControl = True
        Me.txtChargeDC.Location = New System.Drawing.Point(504, 172)
        Me.txtChargeDC.Name = "txtChargeDC"
        Me.txtChargeDC.Properties.Appearance.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtChargeDC.Properties.Appearance.Options.UseFont = True
        Me.txtChargeDC.Properties.Mask.EditMask = "n2"
        Me.txtChargeDC.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric
        Me.txtChargeDC.Properties.Mask.UseMaskAsDisplayFormat = True
        Me.txtChargeDC.Properties.ReadOnly = True
        Me.txtChargeDC.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.txtChargeDC.Size = New System.Drawing.Size(56, 22)
        Me.txtChargeDC.TabIndex = 14
        '
        'txtChargeCC
        '
        Me.txtChargeCC.EditValue = New Decimal(New Integer() {0, 0, 0, 0})
        Me.txtChargeCC.EnterMoveNextControl = True
        Me.txtChargeCC.Location = New System.Drawing.Point(504, 144)
        Me.txtChargeCC.Name = "txtChargeCC"
        Me.txtChargeCC.Properties.Appearance.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtChargeCC.Properties.Appearance.Options.UseFont = True
        Me.txtChargeCC.Properties.Mask.EditMask = "n2"
        Me.txtChargeCC.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric
        Me.txtChargeCC.Properties.Mask.UseMaskAsDisplayFormat = True
        Me.txtChargeCC.Properties.ReadOnly = True
        Me.txtChargeCC.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.txtChargeCC.Size = New System.Drawing.Size(56, 22)
        Me.txtChargeCC.TabIndex = 8
        '
        'txtBK1
        '
        Me.txtBK1.EditValue = New Decimal(New Integer() {0, 0, 0, 0})
        Me.txtBK1.EnterMoveNextControl = True
        Me.txtBK1.Location = New System.Drawing.Point(566, 144)
        Me.txtBK1.Name = "txtBK1"
        Me.txtBK1.Properties.Appearance.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtBK1.Properties.Appearance.Options.UseFont = True
        Me.txtBK1.Properties.Mask.EditMask = "n2"
        Me.txtBK1.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric
        Me.txtBK1.Properties.Mask.UseMaskAsDisplayFormat = True
        Me.txtBK1.Properties.ReadOnly = True
        Me.txtBK1.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.txtBK1.Size = New System.Drawing.Size(145, 22)
        Me.txtBK1.TabIndex = 9
        '
        'txtBK2
        '
        Me.txtBK2.EditValue = New Decimal(New Integer() {0, 0, 0, 0})
        Me.txtBK2.EnterMoveNextControl = True
        Me.txtBK2.Location = New System.Drawing.Point(566, 172)
        Me.txtBK2.Name = "txtBK2"
        Me.txtBK2.Properties.Appearance.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtBK2.Properties.Appearance.Options.UseFont = True
        Me.txtBK2.Properties.Mask.EditMask = "n2"
        Me.txtBK2.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric
        Me.txtBK2.Properties.Mask.UseMaskAsDisplayFormat = True
        Me.txtBK2.Properties.ReadOnly = True
        Me.txtBK2.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.txtBK2.Size = New System.Drawing.Size(145, 22)
        Me.txtBK2.TabIndex = 15
        '
        'txtRounding
        '
        Me.txtRounding.EditValue = New Decimal(New Integer() {0, 0, 0, 0})
        Me.txtRounding.EnterMoveNextControl = True
        Me.txtRounding.Location = New System.Drawing.Point(85, 88)
        Me.txtRounding.Name = "txtRounding"
        Me.txtRounding.Properties.Appearance.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtRounding.Properties.Appearance.Options.UseFont = True
        Me.txtRounding.Properties.Mask.EditMask = "n2"
        Me.txtRounding.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric
        Me.txtRounding.Properties.Mask.UseMaskAsDisplayFormat = True
        Me.txtRounding.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.txtRounding.Size = New System.Drawing.Size(626, 22)
        Me.txtRounding.TabIndex = 1
        '
        'LabelControl8
        '
        Me.LabelControl8.Appearance.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LabelControl8.Location = New System.Drawing.Point(12, 91)
        Me.LabelControl8.Name = "LabelControl8"
        Me.LabelControl8.Size = New System.Drawing.Size(67, 16)
        Me.LabelControl8.TabIndex = 0
        Me.LabelControl8.Text = "Pembulatan"
        '
        'frmBayar
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(723, 314)
        Me.Controls.Add(Me.txtRounding)
        Me.Controls.Add(Me.LabelControl8)
        Me.Controls.Add(Me.txtBK2)
        Me.Controls.Add(Me.txtBK1)
        Me.Controls.Add(Me.txtChargeCC)
        Me.Controls.Add(Me.txtChargeDC)
        Me.Controls.Add(Me.txtNoDC)
        Me.Controls.Add(Me.txtNoCC)
        Me.Controls.Add(Me.txtBankDC)
        Me.Controls.Add(Me.txtBankCC)
        Me.Controls.Add(Me.LabelControl7)
        Me.Controls.Add(Me.txtDC)
        Me.Controls.Add(Me.LabelControl6)
        Me.Controls.Add(Me.txtCC)
        Me.Controls.Add(Me.LabelControl5)
        Me.Controls.Add(Me.LabelControl4)
        Me.Controls.Add(Me.LabelControl3)
        Me.Controls.Add(Me.txtKembali)
        Me.Controls.Add(Me.LabelControl9)
        Me.Controls.Add(Me.txtTunai)
        Me.Controls.Add(Me.LabelControl2)
        Me.Controls.Add(Me.txtSubtotal)
        Me.Controls.Add(Me.LabelControl1)
        Me.Controls.Add(Me.barDockControlLeft)
        Me.Controls.Add(Me.barDockControlRight)
        Me.Controls.Add(Me.barDockControlBottom)
        Me.Controls.Add(Me.barDockControlTop)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.Name = "frmBayar"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = ".:: VPOINT POS ::."
        CType(Me.txtSubtotal.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtTunai.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtKembali.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtCC.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtDC.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtBankCC.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.gvBankCC, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtBankDC.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.gvBankDC, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtNoCC.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtNoDC.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.BarManager1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtChargeDC.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtChargeCC.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtBK1.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtBK2.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtRounding.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents LabelControl1 As DevExpress.XtraEditors.LabelControl
    Friend WithEvents txtSubtotal As DevExpress.XtraEditors.TextEdit
    Friend WithEvents LabelControl2 As DevExpress.XtraEditors.LabelControl
    Friend WithEvents txtTunai As DevExpress.XtraEditors.TextEdit
    Friend WithEvents LabelControl9 As DevExpress.XtraEditors.LabelControl
    Friend WithEvents LabelControl3 As DevExpress.XtraEditors.LabelControl
    Friend WithEvents txtKembali As DevExpress.XtraEditors.TextEdit
    Friend WithEvents LabelControl4 As DevExpress.XtraEditors.LabelControl
    Friend WithEvents txtCC As DevExpress.XtraEditors.TextEdit
    Friend WithEvents LabelControl5 As DevExpress.XtraEditors.LabelControl
    Friend WithEvents txtDC As DevExpress.XtraEditors.TextEdit
    Friend WithEvents LabelControl6 As DevExpress.XtraEditors.LabelControl
    Friend WithEvents LabelControl7 As DevExpress.XtraEditors.LabelControl
    Friend WithEvents txtBankCC As DevExpress.XtraEditors.SearchLookUpEdit
    Friend WithEvents gvBankCC As DevExpress.XtraGrid.Views.Grid.GridView
    Friend WithEvents txtBankDC As DevExpress.XtraEditors.SearchLookUpEdit
    Friend WithEvents gvBankDC As DevExpress.XtraGrid.Views.Grid.GridView
    Friend WithEvents txtNoCC As DevExpress.XtraEditors.TextEdit
    Friend WithEvents txtNoDC As DevExpress.XtraEditors.TextEdit
    Friend WithEvents BarManager1 As DevExpress.XtraBars.BarManager
    Friend WithEvents Bar2 As DevExpress.XtraBars.Bar
    Friend WithEvents BarSubItem1 As DevExpress.XtraBars.BarSubItem
    Friend WithEvents cmdSimpan As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents mnSaveLayOuts As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents Bar3 As DevExpress.XtraBars.Bar
    Friend WithEvents barDockControlTop As DevExpress.XtraBars.BarDockControl
    Friend WithEvents barDockControlBottom As DevExpress.XtraBars.BarDockControl
    Friend WithEvents barDockControlLeft As DevExpress.XtraBars.BarDockControl
    Friend WithEvents barDockControlRight As DevExpress.XtraBars.BarDockControl
    Friend WithEvents cmdCancel As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents txtChargeCC As DevExpress.XtraEditors.TextEdit
    Friend WithEvents txtChargeDC As DevExpress.XtraEditors.TextEdit
    Friend WithEvents txtBK2 As DevExpress.XtraEditors.TextEdit
    Friend WithEvents txtBK1 As DevExpress.XtraEditors.TextEdit
    Friend WithEvents txtRounding As DevExpress.XtraEditors.TextEdit
    Friend WithEvents LabelControl8 As DevExpress.XtraEditors.LabelControl
End Class
