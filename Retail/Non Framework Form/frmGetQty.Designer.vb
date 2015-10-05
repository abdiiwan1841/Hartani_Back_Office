<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmGetQty
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
        Me.cmdTutup = New DevExpress.XtraEditors.SimpleButton
        Me.txtQty = New DevExpress.XtraEditors.TextEdit
        Me.txtDiscRp2 = New DevExpress.XtraEditors.TextEdit
        Me.txtDiscRp1 = New DevExpress.XtraEditors.TextEdit
        Me.txtDiscPersen2 = New DevExpress.XtraEditors.TextEdit
        Me.txtDiscPersen1 = New DevExpress.XtraEditors.TextEdit
        Me.cmdOK = New DevExpress.XtraEditors.SimpleButton
        Me.LayoutControlItem6 = New DevExpress.XtraLayout.LayoutControlItem
        Me.LayoutControlItem8 = New DevExpress.XtraLayout.LayoutControlItem
        Me.LayoutControlItem9 = New DevExpress.XtraLayout.LayoutControlItem
        Me.LayoutControlItem10 = New DevExpress.XtraLayout.LayoutControlItem
        Me.LayoutControlGroup1 = New DevExpress.XtraLayout.LayoutControlGroup
        Me.LayoutControlItem2 = New DevExpress.XtraLayout.LayoutControlItem
        Me.lbQty = New DevExpress.XtraLayout.LayoutControlItem
        Me.LayoutControlItem1 = New DevExpress.XtraLayout.LayoutControlItem
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
        CType(Me.LayoutControl1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.LayoutControl1.SuspendLayout()
        CType(Me.txtQty.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtDiscRp2.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtDiscRp1.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtDiscPersen2.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtDiscPersen1.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem6, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem8, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem9, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem10, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlGroup1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.lbQty, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.BarManager1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'LayoutControl1
        '
        Me.LayoutControl1.Controls.Add(Me.cmdTutup)
        Me.LayoutControl1.Controls.Add(Me.txtQty)
        Me.LayoutControl1.Controls.Add(Me.txtDiscRp2)
        Me.LayoutControl1.Controls.Add(Me.txtDiscRp1)
        Me.LayoutControl1.Controls.Add(Me.txtDiscPersen2)
        Me.LayoutControl1.Controls.Add(Me.txtDiscPersen1)
        Me.LayoutControl1.Controls.Add(Me.cmdOK)
        Me.LayoutControl1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.LayoutControl1.HiddenItems.AddRange(New DevExpress.XtraLayout.BaseLayoutItem() {Me.LayoutControlItem6, Me.LayoutControlItem8, Me.LayoutControlItem9, Me.LayoutControlItem10})
        Me.LayoutControl1.Location = New System.Drawing.Point(0, 0)
        Me.LayoutControl1.Name = "LayoutControl1"
        Me.LayoutControl1.OptionsCustomizationForm.DesignTimeCustomizationFormPositionAndSize = New System.Drawing.Rectangle(758, 170, 250, 350)
        Me.LayoutControl1.Root = Me.LayoutControlGroup1
        Me.LayoutControl1.Size = New System.Drawing.Size(319, 96)
        Me.LayoutControl1.TabIndex = 0
        Me.LayoutControl1.Text = "LayoutControl1"
        '
        'cmdTutup
        '
        Me.cmdTutup.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdTutup.Appearance.Font = New System.Drawing.Font("Tahoma", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdTutup.Appearance.ForeColor = System.Drawing.Color.Blue
        Me.cmdTutup.Appearance.Options.UseFont = True
        Me.cmdTutup.Appearance.Options.UseForeColor = True
        Me.cmdTutup.ImageIndex = 11
        Me.cmdTutup.Location = New System.Drawing.Point(162, 38)
        Me.cmdTutup.Name = "cmdTutup"
        Me.cmdTutup.Size = New System.Drawing.Size(145, 25)
        Me.cmdTutup.StyleController = Me.LayoutControl1
        Me.cmdTutup.TabIndex = 35
        Me.cmdTutup.Text = "&Cancel"
        '
        'txtQty
        '
        Me.txtQty.EditValue = New Decimal(New Integer() {0, 0, 0, 0})
        Me.txtQty.Location = New System.Drawing.Point(34, 12)
        Me.txtQty.Name = "txtQty"
        Me.txtQty.Properties.Appearance.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtQty.Properties.Appearance.Options.UseFont = True
        Me.txtQty.Properties.Mask.EditMask = "n2"
        Me.txtQty.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric
        Me.txtQty.Properties.Mask.UseMaskAsDisplayFormat = True
        Me.txtQty.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.txtQty.Size = New System.Drawing.Size(273, 22)
        Me.txtQty.StyleController = Me.LayoutControl1
        Me.txtQty.TabIndex = 23
        '
        'txtDiscRp2
        '
        Me.txtDiscRp2.EditValue = New Decimal(New Integer() {0, 0, 0, 0})
        Me.txtDiscRp2.EnterMoveNextControl = True
        Me.txtDiscRp2.Location = New System.Drawing.Point(72, 214)
        Me.txtDiscRp2.Name = "txtDiscRp2"
        Me.txtDiscRp2.Properties.Appearance.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtDiscRp2.Properties.Appearance.Options.UseFont = True
        Me.txtDiscRp2.Properties.Mask.EditMask = "n2"
        Me.txtDiscRp2.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric
        Me.txtDiscRp2.Properties.Mask.UseMaskAsDisplayFormat = True
        Me.txtDiscRp2.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.txtDiscRp2.Size = New System.Drawing.Size(259, 22)
        Me.txtDiscRp2.StyleController = Me.LayoutControl1
        Me.txtDiscRp2.TabIndex = 23
        '
        'txtDiscRp1
        '
        Me.txtDiscRp1.EditValue = New Decimal(New Integer() {0, 0, 0, 0})
        Me.txtDiscRp1.EnterMoveNextControl = True
        Me.txtDiscRp1.Location = New System.Drawing.Point(72, 214)
        Me.txtDiscRp1.Name = "txtDiscRp1"
        Me.txtDiscRp1.Properties.Appearance.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtDiscRp1.Properties.Appearance.Options.UseFont = True
        Me.txtDiscRp1.Properties.Mask.EditMask = "n2"
        Me.txtDiscRp1.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric
        Me.txtDiscRp1.Properties.Mask.UseMaskAsDisplayFormat = True
        Me.txtDiscRp1.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.txtDiscRp1.Size = New System.Drawing.Size(259, 22)
        Me.txtDiscRp1.StyleController = Me.LayoutControl1
        Me.txtDiscRp1.TabIndex = 23
        '
        'txtDiscPersen2
        '
        Me.txtDiscPersen2.EditValue = New Decimal(New Integer() {0, 0, 0, 0})
        Me.txtDiscPersen2.EnterMoveNextControl = True
        Me.txtDiscPersen2.Location = New System.Drawing.Point(72, 214)
        Me.txtDiscPersen2.Name = "txtDiscPersen2"
        Me.txtDiscPersen2.Properties.Appearance.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtDiscPersen2.Properties.Appearance.Options.UseFont = True
        Me.txtDiscPersen2.Properties.Mask.EditMask = "n2"
        Me.txtDiscPersen2.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric
        Me.txtDiscPersen2.Properties.Mask.UseMaskAsDisplayFormat = True
        Me.txtDiscPersen2.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.txtDiscPersen2.Size = New System.Drawing.Size(259, 22)
        Me.txtDiscPersen2.StyleController = Me.LayoutControl1
        Me.txtDiscPersen2.TabIndex = 11
        '
        'txtDiscPersen1
        '
        Me.txtDiscPersen1.EditValue = New Decimal(New Integer() {0, 0, 0, 0})
        Me.txtDiscPersen1.EnterMoveNextControl = True
        Me.txtDiscPersen1.Location = New System.Drawing.Point(72, 214)
        Me.txtDiscPersen1.Name = "txtDiscPersen1"
        Me.txtDiscPersen1.Properties.Appearance.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtDiscPersen1.Properties.Appearance.Options.UseFont = True
        Me.txtDiscPersen1.Properties.Mask.EditMask = "n2"
        Me.txtDiscPersen1.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric
        Me.txtDiscPersen1.Properties.Mask.UseMaskAsDisplayFormat = True
        Me.txtDiscPersen1.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.txtDiscPersen1.Size = New System.Drawing.Size(242, 22)
        Me.txtDiscPersen1.StyleController = Me.LayoutControl1
        Me.txtDiscPersen1.TabIndex = 11
        '
        'cmdOK
        '
        Me.cmdOK.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdOK.Appearance.Font = New System.Drawing.Font("Tahoma", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdOK.Appearance.ForeColor = System.Drawing.Color.Blue
        Me.cmdOK.Appearance.Options.UseFont = True
        Me.cmdOK.Appearance.Options.UseForeColor = True
        Me.cmdOK.ImageIndex = 10
        Me.cmdOK.Location = New System.Drawing.Point(12, 38)
        Me.cmdOK.Name = "cmdOK"
        Me.cmdOK.Size = New System.Drawing.Size(146, 25)
        Me.cmdOK.StyleController = Me.LayoutControl1
        Me.cmdOK.TabIndex = 20
        Me.cmdOK.Text = "&OK"
        '
        'LayoutControlItem6
        '
        Me.LayoutControlItem6.Control = Me.txtDiscPersen1
        Me.LayoutControlItem6.CustomizationFormText = "Disc 1 (%)"
        Me.LayoutControlItem6.Location = New System.Drawing.Point(0, 202)
        Me.LayoutControlItem6.Name = "LayoutControlItem6"
        Me.LayoutControlItem6.Size = New System.Drawing.Size(306, 26)
        Me.LayoutControlItem6.Text = "Disc 1 (%)"
        Me.LayoutControlItem6.TextSize = New System.Drawing.Size(56, 13)
        Me.LayoutControlItem6.TextToControlDistance = 5
        '
        'LayoutControlItem8
        '
        Me.LayoutControlItem8.Control = Me.txtDiscPersen2
        Me.LayoutControlItem8.CustomizationFormText = "Disc 2 (%)"
        Me.LayoutControlItem8.Location = New System.Drawing.Point(0, 202)
        Me.LayoutControlItem8.Name = "LayoutControlItem8"
        Me.LayoutControlItem8.Size = New System.Drawing.Size(323, 26)
        Me.LayoutControlItem8.Text = "Disc 2 (%)"
        Me.LayoutControlItem8.TextSize = New System.Drawing.Size(56, 13)
        Me.LayoutControlItem8.TextToControlDistance = 5
        '
        'LayoutControlItem9
        '
        Me.LayoutControlItem9.Control = Me.txtDiscRp1
        Me.LayoutControlItem9.CustomizationFormText = "Disc 1 (Rp.)"
        Me.LayoutControlItem9.Location = New System.Drawing.Point(0, 202)
        Me.LayoutControlItem9.Name = "LayoutControlItem9"
        Me.LayoutControlItem9.Size = New System.Drawing.Size(323, 26)
        Me.LayoutControlItem9.Text = "Disc 1 (Rp.)"
        Me.LayoutControlItem9.TextSize = New System.Drawing.Size(56, 13)
        Me.LayoutControlItem9.TextToControlDistance = 5
        '
        'LayoutControlItem10
        '
        Me.LayoutControlItem10.Control = Me.txtDiscRp2
        Me.LayoutControlItem10.CustomizationFormText = "Disc 2 (Rp.)"
        Me.LayoutControlItem10.Location = New System.Drawing.Point(0, 202)
        Me.LayoutControlItem10.Name = "LayoutControlItem10"
        Me.LayoutControlItem10.Size = New System.Drawing.Size(323, 26)
        Me.LayoutControlItem10.Text = "Disc 2 (Rp.)"
        Me.LayoutControlItem10.TextSize = New System.Drawing.Size(56, 13)
        Me.LayoutControlItem10.TextToControlDistance = 5
        '
        'LayoutControlGroup1
        '
        Me.LayoutControlGroup1.CustomizationFormText = "Root"
        Me.LayoutControlGroup1.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.[True]
        Me.LayoutControlGroup1.GroupBordersVisible = False
        Me.LayoutControlGroup1.Items.AddRange(New DevExpress.XtraLayout.BaseLayoutItem() {Me.LayoutControlItem2, Me.lbQty, Me.LayoutControlItem1})
        Me.LayoutControlGroup1.Location = New System.Drawing.Point(0, 0)
        Me.LayoutControlGroup1.Name = "Root"
        Me.LayoutControlGroup1.Size = New System.Drawing.Size(319, 96)
        Me.LayoutControlGroup1.Text = "Root"
        Me.LayoutControlGroup1.TextVisible = False
        '
        'LayoutControlItem2
        '
        Me.LayoutControlItem2.Control = Me.cmdOK
        Me.LayoutControlItem2.CustomizationFormText = "LayoutControlItem2"
        Me.LayoutControlItem2.Location = New System.Drawing.Point(0, 26)
        Me.LayoutControlItem2.Name = "LayoutControlItem2"
        Me.LayoutControlItem2.Size = New System.Drawing.Size(150, 50)
        Me.LayoutControlItem2.Text = "LayoutControlItem2"
        Me.LayoutControlItem2.TextSize = New System.Drawing.Size(0, 0)
        Me.LayoutControlItem2.TextToControlDistance = 0
        Me.LayoutControlItem2.TextVisible = False
        '
        'lbQty
        '
        Me.lbQty.Control = Me.txtQty
        Me.lbQty.CustomizationFormText = "Qty"
        Me.lbQty.Location = New System.Drawing.Point(0, 0)
        Me.lbQty.Name = "lbQty"
        Me.lbQty.Size = New System.Drawing.Size(299, 26)
        Me.lbQty.Text = "Qty"
        Me.lbQty.TextSize = New System.Drawing.Size(18, 13)
        '
        'LayoutControlItem1
        '
        Me.LayoutControlItem1.Control = Me.cmdTutup
        Me.LayoutControlItem1.CustomizationFormText = "LayoutControlItem1"
        Me.LayoutControlItem1.Location = New System.Drawing.Point(150, 26)
        Me.LayoutControlItem1.Name = "LayoutControlItem1"
        Me.LayoutControlItem1.Size = New System.Drawing.Size(149, 50)
        Me.LayoutControlItem1.Text = "LayoutControlItem1"
        Me.LayoutControlItem1.TextSize = New System.Drawing.Size(0, 0)
        Me.LayoutControlItem1.TextToControlDistance = 0
        Me.LayoutControlItem1.TextVisible = False
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
        Me.barDockControlTop.Size = New System.Drawing.Size(319, 0)
        '
        'barDockControlBottom
        '
        Me.barDockControlBottom.CausesValidation = False
        Me.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.barDockControlBottom.Location = New System.Drawing.Point(0, 96)
        Me.barDockControlBottom.Size = New System.Drawing.Size(319, 0)
        '
        'barDockControlLeft
        '
        Me.barDockControlLeft.CausesValidation = False
        Me.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left
        Me.barDockControlLeft.Location = New System.Drawing.Point(0, 0)
        Me.barDockControlLeft.Size = New System.Drawing.Size(0, 96)
        '
        'barDockControlRight
        '
        Me.barDockControlRight.CausesValidation = False
        Me.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right
        Me.barDockControlRight.Location = New System.Drawing.Point(319, 0)
        Me.barDockControlRight.Size = New System.Drawing.Size(0, 96)
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
        'frmGetQty
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(319, 96)
        Me.Controls.Add(Me.LayoutControl1)
        Me.Controls.Add(Me.barDockControlLeft)
        Me.Controls.Add(Me.barDockControlRight)
        Me.Controls.Add(Me.barDockControlBottom)
        Me.Controls.Add(Me.barDockControlTop)
        Me.Name = "frmGetQty"
        Me.Text = "Seting Qty"
        CType(Me.LayoutControl1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.LayoutControl1.ResumeLayout(False)
        CType(Me.txtQty.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtDiscRp2.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtDiscRp1.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtDiscPersen2.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtDiscPersen1.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem6, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem8, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem9, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem10, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlGroup1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.lbQty, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.BarManager1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents LayoutControl1 As DevExpress.XtraLayout.LayoutControl
    Friend WithEvents LayoutControlGroup1 As DevExpress.XtraLayout.LayoutControlGroup
    Friend WithEvents cmdOK As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents LayoutControlItem2 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents txtDiscPersen2 As DevExpress.XtraEditors.TextEdit
    Friend WithEvents txtDiscPersen1 As DevExpress.XtraEditors.TextEdit
    Friend WithEvents LayoutControlItem6 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents LayoutControlItem8 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents txtQty As DevExpress.XtraEditors.TextEdit
    Friend WithEvents txtDiscRp2 As DevExpress.XtraEditors.TextEdit
    Friend WithEvents txtDiscRp1 As DevExpress.XtraEditors.TextEdit
    Friend WithEvents LayoutControlItem9 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents LayoutControlItem10 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents lbQty As DevExpress.XtraLayout.LayoutControlItem
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
End Class
