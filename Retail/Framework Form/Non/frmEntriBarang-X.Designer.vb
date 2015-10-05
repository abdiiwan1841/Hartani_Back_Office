<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmEntriBarang
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmEntriBarang))
        Me.pnlJudul = New DevExpress.XtraEditors.PanelControl
        Me.pnlTombol = New DevExpress.XtraEditors.PanelControl
        Me.PanelControl3 = New DevExpress.XtraEditors.PanelControl
        Me.LC1 = New DevExpress.XtraLayout.LayoutControl
        Me.GridControl1 = New DevExpress.XtraGrid.GridControl
        Me.GridView1 = New DevExpress.XtraGrid.Views.Grid.GridView
        Me.cSatuan = New DevExpress.XtraGrid.Columns.GridColumn
        Me.RepositoryItemGridLookUpEdit1 = New DevExpress.XtraEditors.Repository.RepositoryItemGridLookUpEdit
        Me.RepositoryItemGridLookUpEdit1View = New DevExpress.XtraGrid.Views.Grid.GridView
        Me.cKonversi = New DevExpress.XtraGrid.Columns.GridColumn
        Me.cHargaJualA = New DevExpress.XtraGrid.Columns.GridColumn
        Me.cHargaJualB = New DevExpress.XtraGrid.Columns.GridColumn
        Me.cHargaJualC = New DevExpress.XtraGrid.Columns.GridColumn
        Me.cHargaJualD = New DevExpress.XtraGrid.Columns.GridColumn
        Me.cHargaJualE = New DevExpress.XtraGrid.Columns.GridColumn
        Me.cIsJual = New DevExpress.XtraGrid.Columns.GridColumn
        Me.RepositoryItemCheckEdit1 = New DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit
        Me.cDiBeli = New DevExpress.XtraGrid.Columns.GridColumn
        Me.cBarcode = New DevExpress.XtraGrid.Columns.GridColumn
        Me.cIsSatuanBase = New DevExpress.XtraGrid.Columns.GridColumn
        Me.BarManager1 = New DevExpress.XtraBars.BarManager(Me.components)
        Me.Bar2 = New DevExpress.XtraBars.Bar
        Me.BarSubItem1 = New DevExpress.XtraBars.BarSubItem
        Me.BarButtonItem1 = New DevExpress.XtraBars.BarButtonItem
        Me.BarButtonItem2 = New DevExpress.XtraBars.BarButtonItem
        Me.barDockControlTop = New DevExpress.XtraBars.BarDockControl
        Me.barDockControlBottom = New DevExpress.XtraBars.BarDockControl
        Me.barDockControlLeft = New DevExpress.XtraBars.BarDockControl
        Me.barDockControlRight = New DevExpress.XtraBars.BarDockControl
        Me.SimpleButton3 = New DevExpress.XtraEditors.SimpleButton
        Me.ImageList1 = New System.Windows.Forms.ImageList(Me.components)
        Me.SimpleButton1 = New DevExpress.XtraEditors.SimpleButton
        Me.LayoutControlGroup1 = New DevExpress.XtraLayout.LayoutControlGroup
        Me.LayoutControlItem1 = New DevExpress.XtraLayout.LayoutControlItem
        Me.LayoutControlItem2 = New DevExpress.XtraLayout.LayoutControlItem
        Me.EmptySpaceItem2 = New DevExpress.XtraLayout.EmptySpaceItem
        Me.EmptySpaceItem3 = New DevExpress.XtraLayout.EmptySpaceItem
        Me.LayoutControlItem4 = New DevExpress.XtraLayout.LayoutControlItem
        CType(Me.pnlJudul, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.pnlTombol, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PanelControl3, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.PanelControl3.SuspendLayout()
        CType(Me.LC1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.LC1.SuspendLayout()
        CType(Me.GridControl1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.GridView1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RepositoryItemGridLookUpEdit1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RepositoryItemGridLookUpEdit1View, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RepositoryItemCheckEdit1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.BarManager1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlGroup1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.EmptySpaceItem2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.EmptySpaceItem3, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem4, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'pnlJudul
        '
        Me.pnlJudul.Dock = System.Windows.Forms.DockStyle.Top
        Me.pnlJudul.Location = New System.Drawing.Point(0, 22)
        Me.pnlJudul.Name = "pnlJudul"
        Me.pnlJudul.Size = New System.Drawing.Size(592, 10)
        Me.pnlJudul.TabIndex = 0
        Me.pnlJudul.Visible = False
        '
        'pnlTombol
        '
        Me.pnlTombol.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.pnlTombol.Location = New System.Drawing.Point(0, 425)
        Me.pnlTombol.Name = "pnlTombol"
        Me.pnlTombol.Size = New System.Drawing.Size(592, 14)
        Me.pnlTombol.TabIndex = 1
        Me.pnlTombol.Visible = False
        '
        'PanelControl3
        '
        Me.PanelControl3.Controls.Add(Me.LC1)
        Me.PanelControl3.Dock = System.Windows.Forms.DockStyle.Fill
        Me.PanelControl3.Location = New System.Drawing.Point(0, 32)
        Me.PanelControl3.Name = "PanelControl3"
        Me.PanelControl3.Size = New System.Drawing.Size(592, 393)
        Me.PanelControl3.TabIndex = 2
        '
        'LC1
        '
        Me.LC1.Controls.Add(Me.GridControl1)
        Me.LC1.Controls.Add(Me.SimpleButton3)
        Me.LC1.Controls.Add(Me.SimpleButton1)
        Me.LC1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.LC1.Location = New System.Drawing.Point(2, 2)
        Me.LC1.Name = "LC1"
        Me.LC1.Root = Me.LayoutControlGroup1
        Me.LC1.Size = New System.Drawing.Size(588, 389)
        Me.LC1.TabIndex = 0
        Me.LC1.Text = "LayoutControl1"
        '
        'GridControl1
        '
        Me.GridControl1.Location = New System.Drawing.Point(12, 12)
        Me.GridControl1.MainView = Me.GridView1
        Me.GridControl1.MenuManager = Me.BarManager1
        Me.GridControl1.Name = "GridControl1"
        Me.GridControl1.RepositoryItems.AddRange(New DevExpress.XtraEditors.Repository.RepositoryItem() {Me.RepositoryItemGridLookUpEdit1, Me.RepositoryItemCheckEdit1})
        Me.GridControl1.Size = New System.Drawing.Size(564, 338)
        Me.GridControl1.TabIndex = 7
        Me.GridControl1.ViewCollection.AddRange(New DevExpress.XtraGrid.Views.Base.BaseView() {Me.GridView1})
        '
        'GridView1
        '
        Me.GridView1.Columns.AddRange(New DevExpress.XtraGrid.Columns.GridColumn() {Me.cSatuan, Me.cKonversi, Me.cHargaJualA, Me.cHargaJualB, Me.cHargaJualC, Me.cHargaJualD, Me.cHargaJualE, Me.cIsJual, Me.cDiBeli, Me.cBarcode, Me.cIsSatuanBase})
        Me.GridView1.GridControl = Me.GridControl1
        Me.GridView1.Name = "GridView1"
        Me.GridView1.OptionsView.NewItemRowPosition = DevExpress.XtraGrid.Views.Grid.NewItemRowPosition.Bottom
        Me.GridView1.OptionsView.ShowGroupPanel = False
        '
        'cSatuan
        '
        Me.cSatuan.Caption = "Satuan"
        Me.cSatuan.ColumnEdit = Me.RepositoryItemGridLookUpEdit1
        Me.cSatuan.FieldName = "IDSatuan"
        Me.cSatuan.Name = "cSatuan"
        Me.cSatuan.Visible = True
        Me.cSatuan.VisibleIndex = 0
        '
        'RepositoryItemGridLookUpEdit1
        '
        Me.RepositoryItemGridLookUpEdit1.AutoHeight = False
        Me.RepositoryItemGridLookUpEdit1.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.RepositoryItemGridLookUpEdit1.DisplayMember = "Nama"
        Me.RepositoryItemGridLookUpEdit1.Name = "RepositoryItemGridLookUpEdit1"
        Me.RepositoryItemGridLookUpEdit1.ValueMember = "NoID"
        Me.RepositoryItemGridLookUpEdit1.View = Me.RepositoryItemGridLookUpEdit1View
        '
        'RepositoryItemGridLookUpEdit1View
        '
        Me.RepositoryItemGridLookUpEdit1View.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus
        Me.RepositoryItemGridLookUpEdit1View.Name = "RepositoryItemGridLookUpEdit1View"
        Me.RepositoryItemGridLookUpEdit1View.OptionsSelection.EnableAppearanceFocusedCell = False
        Me.RepositoryItemGridLookUpEdit1View.OptionsView.ShowGroupPanel = False
        '
        'cKonversi
        '
        Me.cKonversi.Caption = "Konversi (Isi)"
        Me.cKonversi.DisplayFormat.FormatString = "n2"
        Me.cKonversi.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
        Me.cKonversi.FieldName = "Konversi"
        Me.cKonversi.Name = "cKonversi"
        Me.cKonversi.Visible = True
        Me.cKonversi.VisibleIndex = 1
        '
        'cHargaJualA
        '
        Me.cHargaJualA.Caption = "Harga Jual A"
        Me.cHargaJualA.DisplayFormat.FormatString = "n2"
        Me.cHargaJualA.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
        Me.cHargaJualA.FieldName = "HargaJualA"
        Me.cHargaJualA.Name = "cHargaJualA"
        Me.cHargaJualA.Visible = True
        Me.cHargaJualA.VisibleIndex = 2
        '
        'cHargaJualB
        '
        Me.cHargaJualB.Caption = "Harga Jual B"
        Me.cHargaJualB.DisplayFormat.FormatString = "n2"
        Me.cHargaJualB.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
        Me.cHargaJualB.FieldName = "HargaJualB"
        Me.cHargaJualB.Name = "cHargaJualB"
        Me.cHargaJualB.Visible = True
        Me.cHargaJualB.VisibleIndex = 3
        '
        'cHargaJualC
        '
        Me.cHargaJualC.Caption = "Harga Jual C"
        Me.cHargaJualC.DisplayFormat.FormatString = "n2"
        Me.cHargaJualC.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
        Me.cHargaJualC.FieldName = "HargaJualC"
        Me.cHargaJualC.Name = "cHargaJualC"
        Me.cHargaJualC.Visible = True
        Me.cHargaJualC.VisibleIndex = 4
        '
        'cHargaJualD
        '
        Me.cHargaJualD.Caption = "Harga Jual D"
        Me.cHargaJualD.DisplayFormat.FormatString = "n2"
        Me.cHargaJualD.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
        Me.cHargaJualD.FieldName = "HargaJualD"
        Me.cHargaJualD.Name = "cHargaJualD"
        Me.cHargaJualD.Visible = True
        Me.cHargaJualD.VisibleIndex = 5
        '
        'cHargaJualE
        '
        Me.cHargaJualE.Caption = "Harga Jual E"
        Me.cHargaJualE.DisplayFormat.FormatString = "n2"
        Me.cHargaJualE.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
        Me.cHargaJualE.FieldName = "HargaJualE"
        Me.cHargaJualE.Name = "cHargaJualE"
        Me.cHargaJualE.Visible = True
        Me.cHargaJualE.VisibleIndex = 6
        '
        'cIsJual
        '
        Me.cIsJual.Caption = "Di Jual"
        Me.cIsJual.ColumnEdit = Me.RepositoryItemCheckEdit1
        Me.cIsJual.FieldName = "IsJual"
        Me.cIsJual.Name = "cIsJual"
        Me.cIsJual.Visible = True
        Me.cIsJual.VisibleIndex = 7
        '
        'RepositoryItemCheckEdit1
        '
        Me.RepositoryItemCheckEdit1.AutoHeight = False
        Me.RepositoryItemCheckEdit1.Name = "RepositoryItemCheckEdit1"
        Me.RepositoryItemCheckEdit1.NullStyle = DevExpress.XtraEditors.Controls.StyleIndeterminate.Unchecked
        '
        'cDiBeli
        '
        Me.cDiBeli.Caption = "Di Beli"
        Me.cDiBeli.ColumnEdit = Me.RepositoryItemCheckEdit1
        Me.cDiBeli.FieldName = "IsBeli"
        Me.cDiBeli.Name = "cDiBeli"
        Me.cDiBeli.Visible = True
        Me.cDiBeli.VisibleIndex = 8
        '
        'cBarcode
        '
        Me.cBarcode.Caption = "Barcode"
        Me.cBarcode.FieldName = "Barcode"
        Me.cBarcode.Name = "cBarcode"
        Me.cBarcode.Visible = True
        Me.cBarcode.VisibleIndex = 9
        '
        'cIsSatuanBase
        '
        Me.cIsSatuanBase.Caption = "IsSatuanBase"
        Me.cIsSatuanBase.ColumnEdit = Me.RepositoryItemCheckEdit1
        Me.cIsSatuanBase.FieldName = "IsSatuanBase"
        Me.cIsSatuanBase.Name = "cIsSatuanBase"
        Me.cIsSatuanBase.Visible = True
        Me.cIsSatuanBase.VisibleIndex = 10
        '
        'BarManager1
        '
        Me.BarManager1.Bars.AddRange(New DevExpress.XtraBars.Bar() {Me.Bar2})
        Me.BarManager1.DockControls.Add(Me.barDockControlTop)
        Me.BarManager1.DockControls.Add(Me.barDockControlBottom)
        Me.BarManager1.DockControls.Add(Me.barDockControlLeft)
        Me.BarManager1.DockControls.Add(Me.barDockControlRight)
        Me.BarManager1.Form = Me
        Me.BarManager1.Items.AddRange(New DevExpress.XtraBars.BarItem() {Me.BarSubItem1, Me.BarButtonItem1, Me.BarButtonItem2})
        Me.BarManager1.MainMenu = Me.Bar2
        Me.BarManager1.MaxItemId = 3
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
        Me.BarSubItem1.Caption = "&Action"
        Me.BarSubItem1.Id = 0
        Me.BarSubItem1.LinksPersistInfo.AddRange(New DevExpress.XtraBars.LinkPersistInfo() {New DevExpress.XtraBars.LinkPersistInfo(Me.BarButtonItem1), New DevExpress.XtraBars.LinkPersistInfo(Me.BarButtonItem2)})
        Me.BarSubItem1.Name = "BarSubItem1"
        '
        'BarButtonItem1
        '
        Me.BarButtonItem1.Caption = "Save"
        Me.BarButtonItem1.Id = 1
        Me.BarButtonItem1.ItemShortcut = New DevExpress.XtraBars.BarShortcut(System.Windows.Forms.Keys.F2)
        Me.BarButtonItem1.Name = "BarButtonItem1"
        '
        'BarButtonItem2
        '
        Me.BarButtonItem2.Caption = "Close"
        Me.BarButtonItem2.Id = 2
        Me.BarButtonItem2.ItemShortcut = New DevExpress.XtraBars.BarShortcut(System.Windows.Forms.Keys.F12)
        Me.BarButtonItem2.Name = "BarButtonItem2"
        '
        'barDockControlTop
        '
        Me.barDockControlTop.CausesValidation = False
        Me.barDockControlTop.Dock = System.Windows.Forms.DockStyle.Top
        Me.barDockControlTop.Location = New System.Drawing.Point(0, 0)
        Me.barDockControlTop.Size = New System.Drawing.Size(592, 22)
        '
        'barDockControlBottom
        '
        Me.barDockControlBottom.CausesValidation = False
        Me.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.barDockControlBottom.Location = New System.Drawing.Point(0, 439)
        Me.barDockControlBottom.Size = New System.Drawing.Size(592, 0)
        '
        'barDockControlLeft
        '
        Me.barDockControlLeft.CausesValidation = False
        Me.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left
        Me.barDockControlLeft.Location = New System.Drawing.Point(0, 22)
        Me.barDockControlLeft.Size = New System.Drawing.Size(0, 417)
        '
        'barDockControlRight
        '
        Me.barDockControlRight.CausesValidation = False
        Me.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right
        Me.barDockControlRight.Location = New System.Drawing.Point(592, 22)
        Me.barDockControlRight.Size = New System.Drawing.Size(0, 417)
        '
        'SimpleButton3
        '
        Me.SimpleButton3.Appearance.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.SimpleButton3.Appearance.ForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.SimpleButton3.Appearance.Options.UseFont = True
        Me.SimpleButton3.Appearance.Options.UseForeColor = True
        Me.SimpleButton3.ImageIndex = 1
        Me.SimpleButton3.ImageList = Me.ImageList1
        Me.SimpleButton3.Location = New System.Drawing.Point(480, 354)
        Me.SimpleButton3.Name = "SimpleButton3"
        Me.SimpleButton3.Size = New System.Drawing.Size(96, 23)
        Me.SimpleButton3.StyleController = Me.LC1
        Me.SimpleButton3.TabIndex = 5
        Me.SimpleButton3.Text = "&Close"
        '
        'ImageList1
        '
        Me.ImageList1.ImageStream = CType(resources.GetObject("ImageList1.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.ImageList1.TransparentColor = System.Drawing.Color.Transparent
        Me.ImageList1.Images.SetKeyName(0, "F2.png")
        Me.ImageList1.Images.SetKeyName(1, "cmdCancelSave.ico")
        '
        'SimpleButton1
        '
        Me.SimpleButton1.Appearance.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.SimpleButton1.Appearance.ForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.SimpleButton1.Appearance.Options.UseFont = True
        Me.SimpleButton1.Appearance.Options.UseForeColor = True
        Me.SimpleButton1.ImageIndex = 0
        Me.SimpleButton1.ImageList = Me.ImageList1
        Me.SimpleButton1.Location = New System.Drawing.Point(363, 354)
        Me.SimpleButton1.Name = "SimpleButton1"
        Me.SimpleButton1.Size = New System.Drawing.Size(97, 23)
        Me.SimpleButton1.StyleController = Me.LC1
        Me.SimpleButton1.TabIndex = 4
        Me.SimpleButton1.Text = "&Save"
        '
        'LayoutControlGroup1
        '
        Me.LayoutControlGroup1.CustomizationFormText = "LayoutControlGroup1"
        Me.LayoutControlGroup1.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.[True]
        Me.LayoutControlGroup1.GroupBordersVisible = False
        Me.LayoutControlGroup1.Items.AddRange(New DevExpress.XtraLayout.BaseLayoutItem() {Me.LayoutControlItem1, Me.LayoutControlItem2, Me.EmptySpaceItem2, Me.EmptySpaceItem3, Me.LayoutControlItem4})
        Me.LayoutControlGroup1.Location = New System.Drawing.Point(0, 0)
        Me.LayoutControlGroup1.Name = "Root"
        Me.LayoutControlGroup1.Size = New System.Drawing.Size(588, 389)
        Me.LayoutControlGroup1.Text = "Root"
        Me.LayoutControlGroup1.TextVisible = False
        '
        'LayoutControlItem1
        '
        Me.LayoutControlItem1.Control = Me.SimpleButton1
        Me.LayoutControlItem1.CustomizationFormText = "LayoutControlItem1"
        Me.LayoutControlItem1.Location = New System.Drawing.Point(351, 342)
        Me.LayoutControlItem1.MaxSize = New System.Drawing.Size(101, 27)
        Me.LayoutControlItem1.MinSize = New System.Drawing.Size(101, 27)
        Me.LayoutControlItem1.Name = "LayoutControlItem1"
        Me.LayoutControlItem1.Size = New System.Drawing.Size(101, 27)
        Me.LayoutControlItem1.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom
        Me.LayoutControlItem1.Text = "LayoutControlItem1"
        Me.LayoutControlItem1.TextSize = New System.Drawing.Size(0, 0)
        Me.LayoutControlItem1.TextToControlDistance = 0
        Me.LayoutControlItem1.TextVisible = False
        '
        'LayoutControlItem2
        '
        Me.LayoutControlItem2.Control = Me.SimpleButton3
        Me.LayoutControlItem2.CustomizationFormText = "LayoutControlItem2"
        Me.LayoutControlItem2.Location = New System.Drawing.Point(468, 342)
        Me.LayoutControlItem2.MaxSize = New System.Drawing.Size(100, 27)
        Me.LayoutControlItem2.MinSize = New System.Drawing.Size(100, 27)
        Me.LayoutControlItem2.Name = "LayoutControlItem2"
        Me.LayoutControlItem2.Size = New System.Drawing.Size(100, 27)
        Me.LayoutControlItem2.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom
        Me.LayoutControlItem2.Text = "LayoutControlItem2"
        Me.LayoutControlItem2.TextSize = New System.Drawing.Size(0, 0)
        Me.LayoutControlItem2.TextToControlDistance = 0
        Me.LayoutControlItem2.TextVisible = False
        '
        'EmptySpaceItem2
        '
        Me.EmptySpaceItem2.CustomizationFormText = "EmptySpaceItem2"
        Me.EmptySpaceItem2.Location = New System.Drawing.Point(0, 342)
        Me.EmptySpaceItem2.Name = "EmptySpaceItem2"
        Me.EmptySpaceItem2.Size = New System.Drawing.Size(351, 27)
        Me.EmptySpaceItem2.Text = "EmptySpaceItem2"
        Me.EmptySpaceItem2.TextSize = New System.Drawing.Size(0, 0)
        '
        'EmptySpaceItem3
        '
        Me.EmptySpaceItem3.CustomizationFormText = "EmptySpaceItem3"
        Me.EmptySpaceItem3.Location = New System.Drawing.Point(452, 342)
        Me.EmptySpaceItem3.Name = "EmptySpaceItem3"
        Me.EmptySpaceItem3.Size = New System.Drawing.Size(16, 27)
        Me.EmptySpaceItem3.Text = "EmptySpaceItem3"
        Me.EmptySpaceItem3.TextSize = New System.Drawing.Size(0, 0)
        '
        'LayoutControlItem4
        '
        Me.LayoutControlItem4.Control = Me.GridControl1
        Me.LayoutControlItem4.CustomizationFormText = " "
        Me.LayoutControlItem4.Location = New System.Drawing.Point(0, 0)
        Me.LayoutControlItem4.Name = "LayoutControlItem4"
        Me.LayoutControlItem4.Size = New System.Drawing.Size(568, 342)
        Me.LayoutControlItem4.Text = " "
        Me.LayoutControlItem4.TextSize = New System.Drawing.Size(0, 0)
        Me.LayoutControlItem4.TextToControlDistance = 0
        Me.LayoutControlItem4.TextVisible = False
        '
        'frmEntriBarang
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(592, 439)
        Me.Controls.Add(Me.PanelControl3)
        Me.Controls.Add(Me.pnlTombol)
        Me.Controls.Add(Me.pnlJudul)
        Me.Controls.Add(Me.barDockControlLeft)
        Me.Controls.Add(Me.barDockControlRight)
        Me.Controls.Add(Me.barDockControlBottom)
        Me.Controls.Add(Me.barDockControlTop)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "frmEntriBarang"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Entri Data"
        CType(Me.pnlJudul, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.pnlTombol, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PanelControl3, System.ComponentModel.ISupportInitialize).EndInit()
        Me.PanelControl3.ResumeLayout(False)
        CType(Me.LC1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.LC1.ResumeLayout(False)
        CType(Me.GridControl1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.GridView1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RepositoryItemGridLookUpEdit1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RepositoryItemGridLookUpEdit1View, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RepositoryItemCheckEdit1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.BarManager1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlGroup1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.EmptySpaceItem2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.EmptySpaceItem3, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem4, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents pnlJudul As DevExpress.XtraEditors.PanelControl
    Friend WithEvents pnlTombol As DevExpress.XtraEditors.PanelControl
    Friend WithEvents PanelControl3 As DevExpress.XtraEditors.PanelControl
    Friend WithEvents LC1 As DevExpress.XtraLayout.LayoutControl
    Friend WithEvents LayoutControlGroup1 As DevExpress.XtraLayout.LayoutControlGroup
    Friend WithEvents SimpleButton3 As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents SimpleButton1 As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents LayoutControlItem1 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents LayoutControlItem2 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents EmptySpaceItem2 As DevExpress.XtraLayout.EmptySpaceItem
    Friend WithEvents EmptySpaceItem3 As DevExpress.XtraLayout.EmptySpaceItem
    Friend WithEvents ImageList1 As System.Windows.Forms.ImageList
    Friend WithEvents BarManager1 As DevExpress.XtraBars.BarManager
    Friend WithEvents Bar2 As DevExpress.XtraBars.Bar
    Friend WithEvents BarSubItem1 As DevExpress.XtraBars.BarSubItem
    Friend WithEvents BarButtonItem1 As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents BarButtonItem2 As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents barDockControlTop As DevExpress.XtraBars.BarDockControl
    Friend WithEvents barDockControlBottom As DevExpress.XtraBars.BarDockControl
    Friend WithEvents barDockControlLeft As DevExpress.XtraBars.BarDockControl
    Friend WithEvents barDockControlRight As DevExpress.XtraBars.BarDockControl
    Friend WithEvents GridControl1 As DevExpress.XtraGrid.GridControl
    Friend WithEvents GridView1 As DevExpress.XtraGrid.Views.Grid.GridView
    Friend WithEvents cSatuan As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents RepositoryItemGridLookUpEdit1 As DevExpress.XtraEditors.Repository.RepositoryItemGridLookUpEdit
    Friend WithEvents RepositoryItemGridLookUpEdit1View As DevExpress.XtraGrid.Views.Grid.GridView
    Friend WithEvents LayoutControlItem4 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents cKonversi As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents cHargaJualA As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents cHargaJualB As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents cHargaJualC As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents cHargaJualD As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents cHargaJualE As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents cIsJual As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents RepositoryItemCheckEdit1 As DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit
    Friend WithEvents cDiBeli As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents cBarcode As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents cIsSatuanBase As DevExpress.XtraGrid.Columns.GridColumn
End Class
