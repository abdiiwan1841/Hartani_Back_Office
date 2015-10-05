<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmDashBoard
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmDashBoard))
        Me.BUTTON1 = New DevExpress.XtraEditors.SimpleButton
        Me.ImageCollection1 = New DevExpress.Utils.ImageCollection(Me.components)
        Me.LayoutControl1 = New DevExpress.XtraLayout.LayoutControl
        Me.PanelControl1 = New DevExpress.XtraEditors.PanelControl
        Me.BUTTON2 = New DevExpress.XtraEditors.SimpleButton
        Me.BUTTON3 = New DevExpress.XtraEditors.SimpleButton
        Me.BUTTON4 = New DevExpress.XtraEditors.SimpleButton
        Me.LayoutControlGroup1 = New DevExpress.XtraLayout.LayoutControlGroup
        Me.EmptySpaceItem1 = New DevExpress.XtraLayout.EmptySpaceItem
        Me.LayoutControlItem5 = New DevExpress.XtraLayout.LayoutControlItem
        Me.BarManager1 = New DevExpress.XtraBars.BarManager(Me.components)
        Me.Bar2 = New DevExpress.XtraBars.Bar
        Me.BarSubItem1 = New DevExpress.XtraBars.BarSubItem
        Me.mnSaveLayouts = New DevExpress.XtraBars.BarButtonItem
        Me.mnCustomLayouts = New DevExpress.XtraBars.BarButtonItem
        Me.Bar3 = New DevExpress.XtraBars.Bar
        Me.barDockControlTop = New DevExpress.XtraBars.BarDockControl
        Me.barDockControlBottom = New DevExpress.XtraBars.BarDockControl
        Me.barDockControlLeft = New DevExpress.XtraBars.BarDockControl
        Me.barDockControlRight = New DevExpress.XtraBars.BarDockControl
        Me.PictureEdit1 = New DevExpress.XtraEditors.PictureEdit
        CType(Me.ImageCollection1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControl1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.LayoutControl1.SuspendLayout()
        CType(Me.PanelControl1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.PanelControl1.SuspendLayout()
        CType(Me.LayoutControlGroup1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.EmptySpaceItem1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem5, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.BarManager1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PictureEdit1.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'BUTTON1
        '
        Me.BUTTON1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.BUTTON1.Appearance.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BUTTON1.Appearance.ForeColor = System.Drawing.Color.Blue
        Me.BUTTON1.Appearance.Options.UseFont = True
        Me.BUTTON1.Appearance.Options.UseForeColor = True
        Me.BUTTON1.ImageIndex = 0
        Me.BUTTON1.ImageList = Me.ImageCollection1
        Me.BUTTON1.ImageLocation = DevExpress.XtraEditors.ImageLocation.TopCenter
        Me.BUTTON1.Location = New System.Drawing.Point(3, 6)
        Me.BUTTON1.Name = "BUTTON1"
        Me.BUTTON1.Size = New System.Drawing.Size(142, 95)
        Me.BUTTON1.TabIndex = 0
        '
        'ImageCollection1
        '
        Me.ImageCollection1.ImageSize = New System.Drawing.Size(64, 64)
        Me.ImageCollection1.ImageStream = CType(resources.GetObject("ImageCollection1.ImageStream"), DevExpress.Utils.ImageCollectionStreamer)
        Me.ImageCollection1.Images.SetKeyName(0, "LittleBee Icon 06.png")
        Me.ImageCollection1.Images.SetKeyName(1, "LittleBee Icon 31.png")
        Me.ImageCollection1.Images.SetKeyName(2, "LittleBee Icon 42.png")
        Me.ImageCollection1.Images.SetKeyName(3, "LittleBee Icon 58.png")
        '
        'LayoutControl1
        '
        Me.LayoutControl1.Controls.Add(Me.PanelControl1)
        Me.LayoutControl1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.LayoutControl1.Location = New System.Drawing.Point(0, 22)
        Me.LayoutControl1.Name = "LayoutControl1"
        Me.LayoutControl1.OptionsCustomizationForm.DesignTimeCustomizationFormPositionAndSize = New System.Drawing.Rectangle(240, 207, 250, 350)
        Me.LayoutControl1.Root = Me.LayoutControlGroup1
        Me.LayoutControl1.Size = New System.Drawing.Size(784, 293)
        Me.LayoutControl1.TabIndex = 4
        Me.LayoutControl1.Text = "LayoutControl1"
        '
        'PanelControl1
        '
        Me.PanelControl1.Appearance.BackColor = System.Drawing.Color.Transparent
        Me.PanelControl1.Appearance.Options.UseBackColor = True
        Me.PanelControl1.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder
        Me.PanelControl1.Controls.Add(Me.BUTTON1)
        Me.PanelControl1.Controls.Add(Me.BUTTON2)
        Me.PanelControl1.Controls.Add(Me.BUTTON3)
        Me.PanelControl1.Controls.Add(Me.BUTTON4)
        Me.PanelControl1.Location = New System.Drawing.Point(12, 176)
        Me.PanelControl1.Name = "PanelControl1"
        Me.PanelControl1.Size = New System.Drawing.Size(760, 105)
        Me.PanelControl1.TabIndex = 4
        '
        'BUTTON2
        '
        Me.BUTTON2.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.BUTTON2.Appearance.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BUTTON2.Appearance.ForeColor = System.Drawing.Color.Blue
        Me.BUTTON2.Appearance.Options.UseFont = True
        Me.BUTTON2.Appearance.Options.UseForeColor = True
        Me.BUTTON2.ImageIndex = 1
        Me.BUTTON2.ImageList = Me.ImageCollection1
        Me.BUTTON2.ImageLocation = DevExpress.XtraEditors.ImageLocation.TopCenter
        Me.BUTTON2.Location = New System.Drawing.Point(204, 6)
        Me.BUTTON2.Name = "BUTTON2"
        Me.BUTTON2.Size = New System.Drawing.Size(142, 95)
        Me.BUTTON2.TabIndex = 1
        '
        'BUTTON3
        '
        Me.BUTTON3.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.BUTTON3.Appearance.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BUTTON3.Appearance.ForeColor = System.Drawing.Color.Blue
        Me.BUTTON3.Appearance.Options.UseFont = True
        Me.BUTTON3.Appearance.Options.UseForeColor = True
        Me.BUTTON3.ImageIndex = 2
        Me.BUTTON3.ImageList = Me.ImageCollection1
        Me.BUTTON3.ImageLocation = DevExpress.XtraEditors.ImageLocation.TopCenter
        Me.BUTTON3.Location = New System.Drawing.Point(411, 6)
        Me.BUTTON3.Name = "BUTTON3"
        Me.BUTTON3.Size = New System.Drawing.Size(142, 95)
        Me.BUTTON3.TabIndex = 2
        '
        'BUTTON4
        '
        Me.BUTTON4.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.BUTTON4.Appearance.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BUTTON4.Appearance.ForeColor = System.Drawing.Color.Blue
        Me.BUTTON4.Appearance.Options.UseFont = True
        Me.BUTTON4.Appearance.Options.UseForeColor = True
        Me.BUTTON4.ImageIndex = 3
        Me.BUTTON4.ImageList = Me.ImageCollection1
        Me.BUTTON4.ImageLocation = DevExpress.XtraEditors.ImageLocation.TopCenter
        Me.BUTTON4.Location = New System.Drawing.Point(615, 6)
        Me.BUTTON4.Name = "BUTTON4"
        Me.BUTTON4.Size = New System.Drawing.Size(142, 95)
        Me.BUTTON4.TabIndex = 3
        '
        'LayoutControlGroup1
        '
        Me.LayoutControlGroup1.CustomizationFormText = "Root"
        Me.LayoutControlGroup1.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.[True]
        Me.LayoutControlGroup1.GroupBordersVisible = False
        Me.LayoutControlGroup1.Items.AddRange(New DevExpress.XtraLayout.BaseLayoutItem() {Me.EmptySpaceItem1, Me.LayoutControlItem5})
        Me.LayoutControlGroup1.Location = New System.Drawing.Point(0, 0)
        Me.LayoutControlGroup1.Name = "Root"
        Me.LayoutControlGroup1.Size = New System.Drawing.Size(784, 293)
        Me.LayoutControlGroup1.Text = "Root"
        Me.LayoutControlGroup1.TextVisible = False
        '
        'EmptySpaceItem1
        '
        Me.EmptySpaceItem1.CustomizationFormText = "EmptySpaceItem1"
        Me.EmptySpaceItem1.Location = New System.Drawing.Point(0, 0)
        Me.EmptySpaceItem1.Name = "EmptySpaceItem1"
        Me.EmptySpaceItem1.Size = New System.Drawing.Size(764, 164)
        Me.EmptySpaceItem1.Text = "EmptySpaceItem1"
        Me.EmptySpaceItem1.TextSize = New System.Drawing.Size(0, 0)
        '
        'LayoutControlItem5
        '
        Me.LayoutControlItem5.Control = Me.PanelControl1
        Me.LayoutControlItem5.CustomizationFormText = "Panel"
        Me.LayoutControlItem5.Location = New System.Drawing.Point(0, 164)
        Me.LayoutControlItem5.MaxSize = New System.Drawing.Size(764, 109)
        Me.LayoutControlItem5.MinSize = New System.Drawing.Size(764, 109)
        Me.LayoutControlItem5.Name = "LayoutControlItem5"
        Me.LayoutControlItem5.Size = New System.Drawing.Size(764, 109)
        Me.LayoutControlItem5.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom
        Me.LayoutControlItem5.Text = "Panel"
        Me.LayoutControlItem5.TextSize = New System.Drawing.Size(0, 0)
        Me.LayoutControlItem5.TextToControlDistance = 0
        Me.LayoutControlItem5.TextVisible = False
        '
        'BarManager1
        '
        Me.BarManager1.Bars.AddRange(New DevExpress.XtraBars.Bar() {Me.Bar2, Me.Bar3})
        Me.BarManager1.DockControls.Add(Me.barDockControlTop)
        Me.BarManager1.DockControls.Add(Me.barDockControlBottom)
        Me.BarManager1.DockControls.Add(Me.barDockControlLeft)
        Me.BarManager1.DockControls.Add(Me.barDockControlRight)
        Me.BarManager1.Form = Me
        Me.BarManager1.Items.AddRange(New DevExpress.XtraBars.BarItem() {Me.BarSubItem1, Me.mnSaveLayouts, Me.mnCustomLayouts})
        Me.BarManager1.MainMenu = Me.Bar2
        Me.BarManager1.MaxItemId = 3
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
        Me.BarSubItem1.Caption = "&File"
        Me.BarSubItem1.Id = 0
        Me.BarSubItem1.LinksPersistInfo.AddRange(New DevExpress.XtraBars.LinkPersistInfo() {New DevExpress.XtraBars.LinkPersistInfo(Me.mnSaveLayouts), New DevExpress.XtraBars.LinkPersistInfo(Me.mnCustomLayouts)})
        Me.BarSubItem1.Name = "BarSubItem1"
        '
        'mnSaveLayouts
        '
        Me.mnSaveLayouts.Caption = "&Save Layouts"
        Me.mnSaveLayouts.Id = 1
        Me.mnSaveLayouts.ItemShortcut = New DevExpress.XtraBars.BarShortcut(System.Windows.Forms.Keys.F10)
        Me.mnSaveLayouts.Name = "mnSaveLayouts"
        '
        'mnCustomLayouts
        '
        Me.mnCustomLayouts.Caption = "&Custom Layouts"
        Me.mnCustomLayouts.Id = 2
        Me.mnCustomLayouts.ItemShortcut = New DevExpress.XtraBars.BarShortcut((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.F10))
        Me.mnCustomLayouts.Name = "mnCustomLayouts"
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
        Me.barDockControlTop.Size = New System.Drawing.Size(784, 22)
        '
        'barDockControlBottom
        '
        Me.barDockControlBottom.CausesValidation = False
        Me.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.barDockControlBottom.Location = New System.Drawing.Point(0, 315)
        Me.barDockControlBottom.Size = New System.Drawing.Size(784, 23)
        '
        'barDockControlLeft
        '
        Me.barDockControlLeft.CausesValidation = False
        Me.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left
        Me.barDockControlLeft.Location = New System.Drawing.Point(0, 22)
        Me.barDockControlLeft.Size = New System.Drawing.Size(0, 293)
        '
        'barDockControlRight
        '
        Me.barDockControlRight.CausesValidation = False
        Me.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right
        Me.barDockControlRight.Location = New System.Drawing.Point(784, 22)
        Me.barDockControlRight.Size = New System.Drawing.Size(0, 293)
        '
        'PictureEdit1
        '
        Me.PictureEdit1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.PictureEdit1.Location = New System.Drawing.Point(0, 22)
        Me.PictureEdit1.Name = "PictureEdit1"
        Me.PictureEdit1.Properties.Appearance.BackColor = System.Drawing.Color.Transparent
        Me.PictureEdit1.Properties.Appearance.Options.UseBackColor = True
        Me.PictureEdit1.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder
        Me.PictureEdit1.Properties.ShowMenu = False
        Me.PictureEdit1.Properties.SizeMode = DevExpress.XtraEditors.Controls.PictureSizeMode.Squeeze
        Me.PictureEdit1.Size = New System.Drawing.Size(784, 293)
        Me.PictureEdit1.TabIndex = 9
        '
        'frmDashBoard
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(784, 338)
        Me.ControlBox = False
        Me.Controls.Add(Me.LayoutControl1)
        Me.Controls.Add(Me.PictureEdit1)
        Me.Controls.Add(Me.barDockControlLeft)
        Me.Controls.Add(Me.barDockControlRight)
        Me.Controls.Add(Me.barDockControlBottom)
        Me.Controls.Add(Me.barDockControlTop)
        Me.DoubleBuffered = True
        Me.Name = "frmDashBoard"
        Me.ShowIcon = False
        Me.ShowInTaskbar = False
        Me.Text = "Dash Board"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        CType(Me.ImageCollection1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControl1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.LayoutControl1.ResumeLayout(False)
        CType(Me.PanelControl1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.PanelControl1.ResumeLayout(False)
        CType(Me.LayoutControlGroup1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.EmptySpaceItem1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem5, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.BarManager1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PictureEdit1.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents BUTTON1 As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents BUTTON2 As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents BUTTON3 As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents BUTTON4 As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents LayoutControl1 As DevExpress.XtraLayout.LayoutControl
    Friend WithEvents LayoutControlGroup1 As DevExpress.XtraLayout.LayoutControlGroup
    Friend WithEvents EmptySpaceItem1 As DevExpress.XtraLayout.EmptySpaceItem
    Friend WithEvents BarManager1 As DevExpress.XtraBars.BarManager
    Friend WithEvents Bar2 As DevExpress.XtraBars.Bar
    Friend WithEvents BarSubItem1 As DevExpress.XtraBars.BarSubItem
    Friend WithEvents Bar3 As DevExpress.XtraBars.Bar
    Friend WithEvents barDockControlTop As DevExpress.XtraBars.BarDockControl
    Friend WithEvents barDockControlBottom As DevExpress.XtraBars.BarDockControl
    Friend WithEvents barDockControlLeft As DevExpress.XtraBars.BarDockControl
    Friend WithEvents barDockControlRight As DevExpress.XtraBars.BarDockControl
    Friend WithEvents mnSaveLayouts As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents mnCustomLayouts As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents PanelControl1 As DevExpress.XtraEditors.PanelControl
    Friend WithEvents LayoutControlItem5 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents PictureEdit1 As DevExpress.XtraEditors.PictureEdit
    Friend WithEvents ImageCollection1 As DevExpress.Utils.ImageCollection
End Class
