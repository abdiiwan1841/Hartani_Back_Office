<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmSettingAkun
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
        Dim SerializableAppearanceObject2 As DevExpress.Utils.SerializableAppearanceObject = New DevExpress.Utils.SerializableAppearanceObject
        Dim SerializableAppearanceObject1 As DevExpress.Utils.SerializableAppearanceObject = New DevExpress.Utils.SerializableAppearanceObject
        Me.LayoutControl1 = New DevExpress.XtraLayout.LayoutControl
        Me.LayoutControlGroup1 = New DevExpress.XtraLayout.LayoutControlGroup
        Me.SimpleButton9 = New DevExpress.XtraEditors.SimpleButton
        Me.SimpleButton1 = New DevExpress.XtraEditors.SimpleButton
        Me.LayoutControlItem1 = New DevExpress.XtraLayout.LayoutControlItem
        Me.LayoutControlItem2 = New DevExpress.XtraLayout.LayoutControlItem
        Me.EmptySpaceItem1 = New DevExpress.XtraLayout.EmptySpaceItem
        Me.txtAkunBeli = New DevExpress.XtraEditors.SearchLookUpEdit
        Me.gvAkunBeli = New DevExpress.XtraGrid.Views.Grid.GridView
        Me.LayoutControlItem3 = New DevExpress.XtraLayout.LayoutControlItem
        Me.txtAkunReturBeli = New DevExpress.XtraEditors.SearchLookUpEdit
        Me.gvAkunReturBeli = New DevExpress.XtraGrid.Views.Grid.GridView
        Me.LayoutControlItem4 = New DevExpress.XtraLayout.LayoutControlItem
        CType(Me.LayoutControl1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.LayoutControl1.SuspendLayout()
        CType(Me.LayoutControlGroup1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.EmptySpaceItem1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtAkunBeli.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.gvAkunBeli, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem3, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtAkunReturBeli.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.gvAkunReturBeli, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem4, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'LayoutControl1
        '
        Me.LayoutControl1.Controls.Add(Me.txtAkunReturBeli)
        Me.LayoutControl1.Controls.Add(Me.txtAkunBeli)
        Me.LayoutControl1.Controls.Add(Me.SimpleButton1)
        Me.LayoutControl1.Controls.Add(Me.SimpleButton9)
        Me.LayoutControl1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.LayoutControl1.Location = New System.Drawing.Point(0, 0)
        Me.LayoutControl1.Name = "LayoutControl1"
        Me.LayoutControl1.OptionsCustomizationForm.DesignTimeCustomizationFormPositionAndSize = New System.Drawing.Rectangle(710, 149, 250, 350)
        Me.LayoutControl1.Root = Me.LayoutControlGroup1
        Me.LayoutControl1.Size = New System.Drawing.Size(378, 384)
        Me.LayoutControl1.TabIndex = 0
        Me.LayoutControl1.Text = "LayoutControl1"
        '
        'LayoutControlGroup1
        '
        Me.LayoutControlGroup1.CustomizationFormText = "Root"
        Me.LayoutControlGroup1.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.[True]
        Me.LayoutControlGroup1.GroupBordersVisible = False
        Me.LayoutControlGroup1.Items.AddRange(New DevExpress.XtraLayout.BaseLayoutItem() {Me.LayoutControlItem1, Me.LayoutControlItem2, Me.EmptySpaceItem1, Me.LayoutControlItem3, Me.LayoutControlItem4})
        Me.LayoutControlGroup1.Location = New System.Drawing.Point(0, 0)
        Me.LayoutControlGroup1.Name = "Root"
        Me.LayoutControlGroup1.Size = New System.Drawing.Size(378, 384)
        Me.LayoutControlGroup1.Text = "Root"
        Me.LayoutControlGroup1.TextVisible = False
        '
        'SimpleButton9
        '
        Me.SimpleButton9.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.SimpleButton9.Appearance.BackColor = System.Drawing.Color.Snow
        Me.SimpleButton9.Appearance.BackColor2 = System.Drawing.Color.PowderBlue
        Me.SimpleButton9.Appearance.BorderColor = System.Drawing.SystemColors.ButtonFace
        Me.SimpleButton9.Appearance.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.SimpleButton9.Appearance.ForeColor = System.Drawing.Color.Black
        Me.SimpleButton9.Appearance.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.ForwardDiagonal
        Me.SimpleButton9.Appearance.Options.UseBackColor = True
        Me.SimpleButton9.Appearance.Options.UseBorderColor = True
        Me.SimpleButton9.Appearance.Options.UseFont = True
        Me.SimpleButton9.Appearance.Options.UseForeColor = True
        Me.SimpleButton9.Cursor = System.Windows.Forms.Cursors.Hand
        Me.SimpleButton9.ImageIndex = 2
        Me.SimpleButton9.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleLeft
        Me.SimpleButton9.Location = New System.Drawing.Point(12, 349)
        Me.SimpleButton9.Name = "SimpleButton9"
        Me.SimpleButton9.Size = New System.Drawing.Size(175, 23)
        Me.SimpleButton9.StyleController = Me.LayoutControl1
        Me.SimpleButton9.TabIndex = 6
        Me.SimpleButton9.Text = "&Save"
        '
        'SimpleButton1
        '
        Me.SimpleButton1.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.SimpleButton1.Appearance.BackColor = System.Drawing.Color.Snow
        Me.SimpleButton1.Appearance.BackColor2 = System.Drawing.Color.PowderBlue
        Me.SimpleButton1.Appearance.BorderColor = System.Drawing.SystemColors.ButtonFace
        Me.SimpleButton1.Appearance.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.SimpleButton1.Appearance.ForeColor = System.Drawing.Color.Black
        Me.SimpleButton1.Appearance.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.ForwardDiagonal
        Me.SimpleButton1.Appearance.Options.UseBackColor = True
        Me.SimpleButton1.Appearance.Options.UseBorderColor = True
        Me.SimpleButton1.Appearance.Options.UseFont = True
        Me.SimpleButton1.Appearance.Options.UseForeColor = True
        Me.SimpleButton1.Cursor = System.Windows.Forms.Cursors.Hand
        Me.SimpleButton1.ImageIndex = 1
        Me.SimpleButton1.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleLeft
        Me.SimpleButton1.Location = New System.Drawing.Point(191, 349)
        Me.SimpleButton1.Name = "SimpleButton1"
        Me.SimpleButton1.Size = New System.Drawing.Size(175, 23)
        Me.SimpleButton1.StyleController = Me.LayoutControl1
        Me.SimpleButton1.TabIndex = 7
        Me.SimpleButton1.Text = "&Close"
        '
        'LayoutControlItem1
        '
        Me.LayoutControlItem1.Control = Me.SimpleButton9
        Me.LayoutControlItem1.CustomizationFormText = "LayoutControlItem1"
        Me.LayoutControlItem1.Location = New System.Drawing.Point(0, 337)
        Me.LayoutControlItem1.Name = "LayoutControlItem1"
        Me.LayoutControlItem1.Size = New System.Drawing.Size(179, 27)
        Me.LayoutControlItem1.Text = "LayoutControlItem1"
        Me.LayoutControlItem1.TextSize = New System.Drawing.Size(0, 0)
        Me.LayoutControlItem1.TextToControlDistance = 0
        Me.LayoutControlItem1.TextVisible = False
        '
        'LayoutControlItem2
        '
        Me.LayoutControlItem2.Control = Me.SimpleButton1
        Me.LayoutControlItem2.CustomizationFormText = "LayoutControlItem2"
        Me.LayoutControlItem2.Location = New System.Drawing.Point(179, 337)
        Me.LayoutControlItem2.Name = "LayoutControlItem2"
        Me.LayoutControlItem2.Size = New System.Drawing.Size(179, 27)
        Me.LayoutControlItem2.Text = "LayoutControlItem2"
        Me.LayoutControlItem2.TextSize = New System.Drawing.Size(0, 0)
        Me.LayoutControlItem2.TextToControlDistance = 0
        Me.LayoutControlItem2.TextVisible = False
        '
        'EmptySpaceItem1
        '
        Me.EmptySpaceItem1.CustomizationFormText = "EmptySpaceItem1"
        Me.EmptySpaceItem1.Location = New System.Drawing.Point(0, 52)
        Me.EmptySpaceItem1.Name = "EmptySpaceItem1"
        Me.EmptySpaceItem1.Size = New System.Drawing.Size(358, 285)
        Me.EmptySpaceItem1.Text = "EmptySpaceItem1"
        Me.EmptySpaceItem1.TextSize = New System.Drawing.Size(0, 0)
        '
        'txtAkunBeli
        '
        Me.txtAkunBeli.EditValue = ""
        Me.txtAkunBeli.EnterMoveNextControl = True
        Me.txtAkunBeli.Location = New System.Drawing.Point(121, 12)
        Me.txtAkunBeli.Name = "txtAkunBeli"
        Me.txtAkunBeli.Properties.Appearance.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtAkunBeli.Properties.Appearance.Options.UseFont = True
        Me.txtAkunBeli.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo), New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.OK, "Refresh Master", -1, True, True, False, DevExpress.XtraEditors.ImageLocation.MiddleCenter, Nothing, New DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), SerializableAppearanceObject2, "", Nothing, Nothing, True)})
        Me.txtAkunBeli.Properties.DisplayMember = "Kode"
        Me.txtAkunBeli.Properties.NullText = ""
        Me.txtAkunBeli.Properties.ValueMember = "NoID"
        Me.txtAkunBeli.Properties.View = Me.gvAkunBeli
        Me.txtAkunBeli.Size = New System.Drawing.Size(245, 22)
        Me.txtAkunBeli.StyleController = Me.LayoutControl1
        Me.txtAkunBeli.TabIndex = 28
        '
        'gvAkunBeli
        '
        Me.gvAkunBeli.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus
        Me.gvAkunBeli.Name = "gvAkunBeli"
        Me.gvAkunBeli.OptionsSelection.EnableAppearanceFocusedCell = False
        Me.gvAkunBeli.OptionsView.ShowGroupPanel = False
        '
        'LayoutControlItem3
        '
        Me.LayoutControlItem3.Control = Me.txtAkunBeli
        Me.LayoutControlItem3.CustomizationFormText = "Akun Pembelian"
        Me.LayoutControlItem3.Location = New System.Drawing.Point(0, 0)
        Me.LayoutControlItem3.Name = "LayoutControlItem3"
        Me.LayoutControlItem3.Size = New System.Drawing.Size(358, 26)
        Me.LayoutControlItem3.Text = "Akun Pembelian"
        Me.LayoutControlItem3.TextSize = New System.Drawing.Size(105, 13)
        '
        'txtAkunReturBeli
        '
        Me.txtAkunReturBeli.EditValue = ""
        Me.txtAkunReturBeli.EnterMoveNextControl = True
        Me.txtAkunReturBeli.Location = New System.Drawing.Point(121, 38)
        Me.txtAkunReturBeli.Name = "txtAkunReturBeli"
        Me.txtAkunReturBeli.Properties.Appearance.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtAkunReturBeli.Properties.Appearance.Options.UseFont = True
        Me.txtAkunReturBeli.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo), New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.OK, "Refresh Master", -1, True, True, False, DevExpress.XtraEditors.ImageLocation.MiddleCenter, Nothing, New DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), SerializableAppearanceObject1, "", Nothing, Nothing, True)})
        Me.txtAkunReturBeli.Properties.DisplayMember = "Kode"
        Me.txtAkunReturBeli.Properties.NullText = ""
        Me.txtAkunReturBeli.Properties.ValueMember = "NoID"
        Me.txtAkunReturBeli.Properties.View = Me.gvAkunReturBeli
        Me.txtAkunReturBeli.Size = New System.Drawing.Size(245, 22)
        Me.txtAkunReturBeli.StyleController = Me.LayoutControl1
        Me.txtAkunReturBeli.TabIndex = 28
        '
        'gvAkunReturBeli
        '
        Me.gvAkunReturBeli.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus
        Me.gvAkunReturBeli.Name = "gvAkunReturBeli"
        Me.gvAkunReturBeli.OptionsSelection.EnableAppearanceFocusedCell = False
        Me.gvAkunReturBeli.OptionsView.ShowGroupPanel = False
        '
        'LayoutControlItem4
        '
        Me.LayoutControlItem4.Control = Me.txtAkunReturBeli
        Me.LayoutControlItem4.CustomizationFormText = "Akun Retur Pembelian"
        Me.LayoutControlItem4.Location = New System.Drawing.Point(0, 26)
        Me.LayoutControlItem4.Name = "LayoutControlItem4"
        Me.LayoutControlItem4.Size = New System.Drawing.Size(358, 26)
        Me.LayoutControlItem4.Text = "Akun Retur Pembelian"
        Me.LayoutControlItem4.TextSize = New System.Drawing.Size(105, 13)
        '
        'frmSettingAkun
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(378, 384)
        Me.Controls.Add(Me.LayoutControl1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmSettingAkun"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Setting Akun"
        CType(Me.LayoutControl1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.LayoutControl1.ResumeLayout(False)
        CType(Me.LayoutControlGroup1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.EmptySpaceItem1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtAkunBeli.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.gvAkunBeli, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem3, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtAkunReturBeli.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.gvAkunReturBeli, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem4, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents LayoutControl1 As DevExpress.XtraLayout.LayoutControl
    Friend WithEvents LayoutControlGroup1 As DevExpress.XtraLayout.LayoutControlGroup
    Friend WithEvents SimpleButton1 As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents SimpleButton9 As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents LayoutControlItem1 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents LayoutControlItem2 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents EmptySpaceItem1 As DevExpress.XtraLayout.EmptySpaceItem
    Friend WithEvents txtAkunReturBeli As DevExpress.XtraEditors.SearchLookUpEdit
    Friend WithEvents gvAkunReturBeli As DevExpress.XtraGrid.Views.Grid.GridView
    Friend WithEvents txtAkunBeli As DevExpress.XtraEditors.SearchLookUpEdit
    Friend WithEvents gvAkunBeli As DevExpress.XtraGrid.Views.Grid.GridView
    Friend WithEvents LayoutControlItem3 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents LayoutControlItem4 As DevExpress.XtraLayout.LayoutControlItem
End Class
