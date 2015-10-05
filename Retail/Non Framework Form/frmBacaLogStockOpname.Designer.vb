<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmBacaLogStockOpname
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmBacaLogStockOpname))
        Me.pnlJudul = New DevExpress.XtraEditors.PanelControl
        Me.LabelControl1 = New DevExpress.XtraEditors.LabelControl
        Me.txtGudang = New DevExpress.XtraEditors.SearchLookUpEdit
        Me.gvGudang = New DevExpress.XtraGrid.Views.Grid.GridView
        Me.pnlTombol = New DevExpress.XtraEditors.PanelControl
        Me.ProgressBarControl1 = New DevExpress.XtraEditors.ProgressBarControl
        Me.BarManager1 = New DevExpress.XtraBars.BarManager(Me.components)
        Me.Bar2 = New DevExpress.XtraBars.Bar
        Me.BarSubItem1 = New DevExpress.XtraBars.BarSubItem
        Me.BarButtonItem1 = New DevExpress.XtraBars.BarButtonItem
        Me.BarButtonItem2 = New DevExpress.XtraBars.BarButtonItem
        Me.BarButtonItem3 = New DevExpress.XtraBars.BarButtonItem
        Me.barDockControlTop = New DevExpress.XtraBars.BarDockControl
        Me.barDockControlBottom = New DevExpress.XtraBars.BarDockControl
        Me.barDockControlLeft = New DevExpress.XtraBars.BarDockControl
        Me.barDockControlRight = New DevExpress.XtraBars.BarDockControl
        Me.SimpleButton2 = New DevExpress.XtraEditors.SimpleButton
        Me.SimpleButton1 = New DevExpress.XtraEditors.SimpleButton
        Me.cmdSave = New DevExpress.XtraEditors.SimpleButton
        Me.SimpleButton3 = New DevExpress.XtraEditors.SimpleButton
        Me.PanelControl3 = New DevExpress.XtraEditors.PanelControl
        Me.LC1 = New DevExpress.XtraLayout.LayoutControl
        Me.MemoEdit1 = New DevExpress.XtraEditors.MemoEdit
        Me.LayoutControlGroup1 = New DevExpress.XtraLayout.LayoutControlGroup
        Me.LayoutControlItem1 = New DevExpress.XtraLayout.LayoutControlItem
        Me.ImageList1 = New System.Windows.Forms.ImageList(Me.components)
        CType(Me.pnlJudul, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.pnlJudul.SuspendLayout()
        CType(Me.txtGudang.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.gvGudang, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.pnlTombol, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.pnlTombol.SuspendLayout()
        CType(Me.ProgressBarControl1.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.BarManager1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PanelControl3, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.PanelControl3.SuspendLayout()
        CType(Me.LC1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.LC1.SuspendLayout()
        CType(Me.MemoEdit1.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlGroup1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'pnlJudul
        '
        Me.pnlJudul.Controls.Add(Me.LabelControl1)
        Me.pnlJudul.Controls.Add(Me.txtGudang)
        Me.pnlJudul.Dock = System.Windows.Forms.DockStyle.Top
        Me.pnlJudul.Location = New System.Drawing.Point(0, 22)
        Me.pnlJudul.Name = "pnlJudul"
        Me.pnlJudul.Size = New System.Drawing.Size(584, 40)
        Me.pnlJudul.TabIndex = 0
        '
        'LabelControl1
        '
        Me.LabelControl1.Location = New System.Drawing.Point(16, 11)
        Me.LabelControl1.Name = "LabelControl1"
        Me.LabelControl1.Size = New System.Drawing.Size(37, 13)
        Me.LabelControl1.TabIndex = 9
        Me.LabelControl1.Text = "Gudang"
        '
        'txtGudang
        '
        Me.txtGudang.EditValue = ""
        Me.txtGudang.EnterMoveNextControl = True
        Me.txtGudang.Location = New System.Drawing.Point(59, 8)
        Me.txtGudang.Name = "txtGudang"
        Me.txtGudang.Properties.Appearance.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtGudang.Properties.Appearance.Options.UseFont = True
        Me.txtGudang.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.txtGudang.Properties.DisplayMember = "Kode"
        Me.txtGudang.Properties.NullText = ""
        Me.txtGudang.Properties.ValueMember = "NoID"
        Me.txtGudang.Properties.View = Me.gvGudang
        Me.txtGudang.Size = New System.Drawing.Size(258, 22)
        Me.txtGudang.TabIndex = 8
        '
        'gvGudang
        '
        Me.gvGudang.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus
        Me.gvGudang.Name = "gvGudang"
        Me.gvGudang.OptionsSelection.EnableAppearanceFocusedCell = False
        Me.gvGudang.OptionsView.ShowGroupPanel = False
        '
        'pnlTombol
        '
        Me.pnlTombol.Controls.Add(Me.ProgressBarControl1)
        Me.pnlTombol.Controls.Add(Me.SimpleButton2)
        Me.pnlTombol.Controls.Add(Me.SimpleButton1)
        Me.pnlTombol.Controls.Add(Me.cmdSave)
        Me.pnlTombol.Controls.Add(Me.SimpleButton3)
        Me.pnlTombol.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.pnlTombol.Location = New System.Drawing.Point(0, 394)
        Me.pnlTombol.Name = "pnlTombol"
        Me.pnlTombol.Size = New System.Drawing.Size(584, 56)
        Me.pnlTombol.TabIndex = 2
        '
        'ProgressBarControl1
        '
        Me.ProgressBarControl1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ProgressBarControl1.Location = New System.Drawing.Point(5, 5)
        Me.ProgressBarControl1.MenuManager = Me.BarManager1
        Me.ProgressBarControl1.Name = "ProgressBarControl1"
        Me.ProgressBarControl1.Size = New System.Drawing.Size(565, 10)
        Me.ProgressBarControl1.TabIndex = 21
        '
        'BarManager1
        '
        Me.BarManager1.Bars.AddRange(New DevExpress.XtraBars.Bar() {Me.Bar2})
        Me.BarManager1.DockControls.Add(Me.barDockControlTop)
        Me.BarManager1.DockControls.Add(Me.barDockControlBottom)
        Me.BarManager1.DockControls.Add(Me.barDockControlLeft)
        Me.BarManager1.DockControls.Add(Me.barDockControlRight)
        Me.BarManager1.Form = Me
        Me.BarManager1.Items.AddRange(New DevExpress.XtraBars.BarItem() {Me.BarSubItem1, Me.BarButtonItem1, Me.BarButtonItem2, Me.BarButtonItem3})
        Me.BarManager1.MainMenu = Me.Bar2
        Me.BarManager1.MaxItemId = 4
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
        Me.BarSubItem1.LinksPersistInfo.AddRange(New DevExpress.XtraBars.LinkPersistInfo() {New DevExpress.XtraBars.LinkPersistInfo(Me.BarButtonItem1), New DevExpress.XtraBars.LinkPersistInfo(Me.BarButtonItem2), New DevExpress.XtraBars.LinkPersistInfo(Me.BarButtonItem3, True)})
        Me.BarSubItem1.Name = "BarSubItem1"
        '
        'BarButtonItem1
        '
        Me.BarButtonItem1.Caption = "Save"
        Me.BarButtonItem1.Id = 1
        Me.BarButtonItem1.ItemShortcut = New DevExpress.XtraBars.BarShortcut(System.Windows.Forms.Keys.F6)
        Me.BarButtonItem1.Name = "BarButtonItem1"
        '
        'BarButtonItem2
        '
        Me.BarButtonItem2.Caption = "Close"
        Me.BarButtonItem2.Id = 2
        Me.BarButtonItem2.ItemShortcut = New DevExpress.XtraBars.BarShortcut(System.Windows.Forms.Keys.F3)
        Me.BarButtonItem2.Name = "BarButtonItem2"
        '
        'BarButtonItem3
        '
        Me.BarButtonItem3.Caption = "Save Layout"
        Me.BarButtonItem3.Id = 3
        Me.BarButtonItem3.ItemShortcut = New DevExpress.XtraBars.BarShortcut(System.Windows.Forms.Keys.F10)
        Me.BarButtonItem3.Name = "BarButtonItem3"
        '
        'barDockControlTop
        '
        Me.barDockControlTop.CausesValidation = False
        Me.barDockControlTop.Dock = System.Windows.Forms.DockStyle.Top
        Me.barDockControlTop.Location = New System.Drawing.Point(0, 0)
        Me.barDockControlTop.Size = New System.Drawing.Size(584, 22)
        '
        'barDockControlBottom
        '
        Me.barDockControlBottom.CausesValidation = False
        Me.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.barDockControlBottom.Location = New System.Drawing.Point(0, 450)
        Me.barDockControlBottom.Size = New System.Drawing.Size(584, 0)
        '
        'barDockControlLeft
        '
        Me.barDockControlLeft.CausesValidation = False
        Me.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left
        Me.barDockControlLeft.Location = New System.Drawing.Point(0, 22)
        Me.barDockControlLeft.Size = New System.Drawing.Size(0, 428)
        '
        'barDockControlRight
        '
        Me.barDockControlRight.CausesValidation = False
        Me.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right
        Me.barDockControlRight.Location = New System.Drawing.Point(584, 22)
        Me.barDockControlRight.Size = New System.Drawing.Size(0, 428)
        '
        'SimpleButton2
        '
        Me.SimpleButton2.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.SimpleButton2.Appearance.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.SimpleButton2.Appearance.ForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.SimpleButton2.Appearance.Options.UseFont = True
        Me.SimpleButton2.Appearance.Options.UseForeColor = True
        Me.SimpleButton2.ImageIndex = 1
        Me.SimpleButton2.ImageList = Me.ImageList1
        Me.SimpleButton2.Location = New System.Drawing.Point(120, 21)
        Me.SimpleButton2.Name = "SimpleButton2"
        Me.SimpleButton2.Size = New System.Drawing.Size(97, 23)
        Me.SimpleButton2.TabIndex = 7
        Me.SimpleButton2.Text = "&Save File"
        '
        'SimpleButton1
        '
        Me.SimpleButton1.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.SimpleButton1.Appearance.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.SimpleButton1.Appearance.ForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.SimpleButton1.Appearance.Options.UseFont = True
        Me.SimpleButton1.Appearance.Options.UseForeColor = True
        Me.SimpleButton1.ImageIndex = 0
        Me.SimpleButton1.ImageList = Me.ImageList1
        Me.SimpleButton1.Location = New System.Drawing.Point(8, 21)
        Me.SimpleButton1.Name = "SimpleButton1"
        Me.SimpleButton1.Size = New System.Drawing.Size(106, 23)
        Me.SimpleButton1.TabIndex = 6
        Me.SimpleButton1.Text = "&Open File"
        '
        'cmdSave
        '
        Me.cmdSave.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdSave.Appearance.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdSave.Appearance.ForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.cmdSave.Appearance.Options.UseFont = True
        Me.cmdSave.Appearance.Options.UseForeColor = True
        Me.cmdSave.ImageIndex = 5
        Me.cmdSave.ImageList = Me.ImageList1
        Me.cmdSave.Location = New System.Drawing.Point(366, 21)
        Me.cmdSave.Name = "cmdSave"
        Me.cmdSave.Size = New System.Drawing.Size(97, 23)
        Me.cmdSave.TabIndex = 4
        Me.cmdSave.Text = "&Proses"
        '
        'SimpleButton3
        '
        Me.SimpleButton3.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.SimpleButton3.Appearance.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.SimpleButton3.Appearance.ForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.SimpleButton3.Appearance.Options.UseFont = True
        Me.SimpleButton3.Appearance.Options.UseForeColor = True
        Me.SimpleButton3.ImageIndex = 2
        Me.SimpleButton3.ImageList = Me.ImageList1
        Me.SimpleButton3.Location = New System.Drawing.Point(469, 21)
        Me.SimpleButton3.Name = "SimpleButton3"
        Me.SimpleButton3.Size = New System.Drawing.Size(96, 23)
        Me.SimpleButton3.TabIndex = 5
        Me.SimpleButton3.Text = "&Tutup"
        '
        'PanelControl3
        '
        Me.PanelControl3.Controls.Add(Me.LC1)
        Me.PanelControl3.Dock = System.Windows.Forms.DockStyle.Fill
        Me.PanelControl3.Location = New System.Drawing.Point(0, 62)
        Me.PanelControl3.Name = "PanelControl3"
        Me.PanelControl3.Size = New System.Drawing.Size(584, 332)
        Me.PanelControl3.TabIndex = 1
        '
        'LC1
        '
        Me.LC1.Controls.Add(Me.MemoEdit1)
        Me.LC1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.LC1.Location = New System.Drawing.Point(2, 2)
        Me.LC1.Name = "LC1"
        Me.LC1.Root = Me.LayoutControlGroup1
        Me.LC1.Size = New System.Drawing.Size(580, 328)
        Me.LC1.TabIndex = 0
        Me.LC1.Text = "LayoutControl1"
        '
        'MemoEdit1
        '
        Me.MemoEdit1.Location = New System.Drawing.Point(12, 12)
        Me.MemoEdit1.MenuManager = Me.BarManager1
        Me.MemoEdit1.Name = "MemoEdit1"
        Me.MemoEdit1.Size = New System.Drawing.Size(556, 304)
        Me.MemoEdit1.StyleController = Me.LC1
        Me.MemoEdit1.TabIndex = 4
        '
        'LayoutControlGroup1
        '
        Me.LayoutControlGroup1.CustomizationFormText = "LayoutControlGroup1"
        Me.LayoutControlGroup1.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.[True]
        Me.LayoutControlGroup1.GroupBordersVisible = False
        Me.LayoutControlGroup1.Items.AddRange(New DevExpress.XtraLayout.BaseLayoutItem() {Me.LayoutControlItem1})
        Me.LayoutControlGroup1.Location = New System.Drawing.Point(0, 0)
        Me.LayoutControlGroup1.Name = "Root"
        Me.LayoutControlGroup1.Size = New System.Drawing.Size(580, 328)
        Me.LayoutControlGroup1.Text = "Root"
        Me.LayoutControlGroup1.TextVisible = False
        '
        'LayoutControlItem1
        '
        Me.LayoutControlItem1.Control = Me.MemoEdit1
        Me.LayoutControlItem1.CustomizationFormText = "LayoutControlItem1"
        Me.LayoutControlItem1.Location = New System.Drawing.Point(0, 0)
        Me.LayoutControlItem1.Name = "LayoutControlItem1"
        Me.LayoutControlItem1.Size = New System.Drawing.Size(560, 308)
        Me.LayoutControlItem1.Text = "LayoutControlItem1"
        Me.LayoutControlItem1.TextSize = New System.Drawing.Size(0, 0)
        Me.LayoutControlItem1.TextToControlDistance = 0
        Me.LayoutControlItem1.TextVisible = False
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
        'frmBacaLogStockOpname
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(584, 450)
        Me.Controls.Add(Me.PanelControl3)
        Me.Controls.Add(Me.pnlTombol)
        Me.Controls.Add(Me.pnlJudul)
        Me.Controls.Add(Me.barDockControlLeft)
        Me.Controls.Add(Me.barDockControlRight)
        Me.Controls.Add(Me.barDockControlBottom)
        Me.Controls.Add(Me.barDockControlTop)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "frmBacaLogStockOpname"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Entri Data"
        Me.TopMost = True
        CType(Me.pnlJudul, System.ComponentModel.ISupportInitialize).EndInit()
        Me.pnlJudul.ResumeLayout(False)
        Me.pnlJudul.PerformLayout()
        CType(Me.txtGudang.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.gvGudang, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.pnlTombol, System.ComponentModel.ISupportInitialize).EndInit()
        Me.pnlTombol.ResumeLayout(False)
        CType(Me.ProgressBarControl1.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.BarManager1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PanelControl3, System.ComponentModel.ISupportInitialize).EndInit()
        Me.PanelControl3.ResumeLayout(False)
        CType(Me.LC1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.LC1.ResumeLayout(False)
        CType(Me.MemoEdit1.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlGroup1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents pnlJudul As DevExpress.XtraEditors.PanelControl
    Friend WithEvents pnlTombol As DevExpress.XtraEditors.PanelControl
    Friend WithEvents PanelControl3 As DevExpress.XtraEditors.PanelControl
    Friend WithEvents LC1 As DevExpress.XtraLayout.LayoutControl
    Friend WithEvents LayoutControlGroup1 As DevExpress.XtraLayout.LayoutControlGroup
    Friend WithEvents SimpleButton3 As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents cmdSave As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents BarManager1 As DevExpress.XtraBars.BarManager
    Friend WithEvents Bar2 As DevExpress.XtraBars.Bar
    Friend WithEvents BarSubItem1 As DevExpress.XtraBars.BarSubItem
    Friend WithEvents BarButtonItem1 As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents BarButtonItem2 As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents barDockControlTop As DevExpress.XtraBars.BarDockControl
    Friend WithEvents barDockControlBottom As DevExpress.XtraBars.BarDockControl
    Friend WithEvents barDockControlLeft As DevExpress.XtraBars.BarDockControl
    Friend WithEvents barDockControlRight As DevExpress.XtraBars.BarDockControl
    Friend WithEvents BarButtonItem3 As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents MemoEdit1 As DevExpress.XtraEditors.MemoEdit
    Friend WithEvents LayoutControlItem1 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents SimpleButton2 As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents SimpleButton1 As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents LabelControl1 As DevExpress.XtraEditors.LabelControl
    Friend WithEvents txtGudang As DevExpress.XtraEditors.SearchLookUpEdit
    Friend WithEvents gvGudang As DevExpress.XtraGrid.Views.Grid.GridView
    Friend WithEvents ProgressBarControl1 As DevExpress.XtraEditors.ProgressBarControl
    Friend WithEvents ImageList1 As System.Windows.Forms.ImageList
End Class
