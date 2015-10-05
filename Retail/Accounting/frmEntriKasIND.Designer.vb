<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmEntriKasIND
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
        Dim SerializableAppearanceObject1 As DevExpress.Utils.SerializableAppearanceObject = New DevExpress.Utils.SerializableAppearanceObject
        Dim SerializableAppearanceObject2 As DevExpress.Utils.SerializableAppearanceObject = New DevExpress.Utils.SerializableAppearanceObject
        Dim SerializableAppearanceObject3 As DevExpress.Utils.SerializableAppearanceObject = New DevExpress.Utils.SerializableAppearanceObject
        Dim SerializableAppearanceObject4 As DevExpress.Utils.SerializableAppearanceObject = New DevExpress.Utils.SerializableAppearanceObject
        Me.LayoutControl1 = New DevExpress.XtraLayout.LayoutControl
        Me.txtGudang = New DevExpress.XtraEditors.ButtonEdit
        Me.txtJml = New DevExpress.XtraEditors.ButtonEdit
        Me.txtAkun = New DevExpress.XtraEditors.ButtonEdit
        Me.txtCatatan = New DevExpress.XtraEditors.ButtonEdit
        Me.cmdClose = New DevExpress.XtraEditors.SimpleButton
        Me.cmdSave = New DevExpress.XtraEditors.SimpleButton
        Me.LayoutControlGroup1 = New DevExpress.XtraLayout.LayoutControlGroup
        Me.LayoutControlItem1 = New DevExpress.XtraLayout.LayoutControlItem
        Me.LayoutControlItem4 = New DevExpress.XtraLayout.LayoutControlItem
        Me.LayoutControlItem7 = New DevExpress.XtraLayout.LayoutControlItem
        Me.LayoutControlItem8 = New DevExpress.XtraLayout.LayoutControlItem
        Me.EmptySpaceItem1 = New DevExpress.XtraLayout.EmptySpaceItem
        Me.LayoutControlItem10 = New DevExpress.XtraLayout.LayoutControlItem
        Me.LayoutControlItem11 = New DevExpress.XtraLayout.LayoutControlItem
        CType(Me.LayoutControl1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.LayoutControl1.SuspendLayout()
        CType(Me.txtGudang.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtJml.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtAkun.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtCatatan.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlGroup1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem4, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem7, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem8, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.EmptySpaceItem1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem10, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem11, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'LayoutControl1
        '
        Me.LayoutControl1.Controls.Add(Me.txtGudang)
        Me.LayoutControl1.Controls.Add(Me.txtJml)
        Me.LayoutControl1.Controls.Add(Me.txtAkun)
        Me.LayoutControl1.Controls.Add(Me.txtCatatan)
        Me.LayoutControl1.Controls.Add(Me.cmdClose)
        Me.LayoutControl1.Controls.Add(Me.cmdSave)
        Me.LayoutControl1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.LayoutControl1.Location = New System.Drawing.Point(0, 0)
        Me.LayoutControl1.Name = "LayoutControl1"
        Me.LayoutControl1.Root = Me.LayoutControlGroup1
        Me.LayoutControl1.Size = New System.Drawing.Size(375, 164)
        Me.LayoutControl1.TabIndex = 19
        Me.LayoutControl1.Text = "LayoutControl1"
        '
        'txtGudang
        '
        Me.txtGudang.EnterMoveNextControl = True
        Me.txtGudang.Location = New System.Drawing.Point(61, 12)
        Me.txtGudang.Name = "txtGudang"
        Me.txtGudang.Properties.Appearance.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtGudang.Properties.Appearance.Options.UseFont = True
        Me.txtGudang.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Ellipsis, "", -1, True, True, False, DevExpress.XtraEditors.ImageLocation.MiddleCenter, Nothing, New DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.PageDown), SerializableAppearanceObject1, "", Nothing, Nothing, True), New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Close, "", -1, True, True, False, DevExpress.XtraEditors.ImageLocation.MiddleCenter, Nothing, New DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.Delete), SerializableAppearanceObject2, "", Nothing, Nothing, True)})
        Me.txtGudang.Properties.ReadOnly = True
        Me.txtGudang.Size = New System.Drawing.Size(302, 22)
        Me.txtGudang.StyleController = Me.LayoutControl1
        Me.txtGudang.TabIndex = 1
        '
        'txtJml
        '
        Me.txtJml.EditValue = New Decimal(New Integer() {0, 0, 0, 0})
        Me.txtJml.EnterMoveNextControl = True
        Me.txtJml.Location = New System.Drawing.Point(61, 64)
        Me.txtJml.Name = "txtJml"
        Me.txtJml.Properties.Appearance.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtJml.Properties.Appearance.Options.UseFont = True
        Me.txtJml.Properties.DisplayFormat.FormatString = "###,###,###,##0.00"
        Me.txtJml.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
        Me.txtJml.Properties.EditFormat.FormatString = "###,###,###,##0.00"
        Me.txtJml.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric
        Me.txtJml.Properties.Mask.EditMask = "n2"
        Me.txtJml.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric
        Me.txtJml.Properties.Mask.UseMaskAsDisplayFormat = True
        Me.txtJml.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.txtJml.Size = New System.Drawing.Size(302, 22)
        Me.txtJml.StyleController = Me.LayoutControl1
        Me.txtJml.TabIndex = 13
        '
        'txtAkun
        '
        Me.txtAkun.EnterMoveNextControl = True
        Me.txtAkun.Location = New System.Drawing.Point(61, 38)
        Me.txtAkun.Name = "txtAkun"
        Me.txtAkun.Properties.Appearance.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtAkun.Properties.Appearance.Options.UseFont = True
        Me.txtAkun.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Ellipsis, "", -1, True, True, False, DevExpress.XtraEditors.ImageLocation.MiddleCenter, Nothing, New DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.PageDown), SerializableAppearanceObject3, "", Nothing, Nothing, True), New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Close, "", -1, True, True, False, DevExpress.XtraEditors.ImageLocation.MiddleCenter, Nothing, New DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.Delete), SerializableAppearanceObject4, "", Nothing, Nothing, True)})
        Me.txtAkun.Properties.ReadOnly = True
        Me.txtAkun.Size = New System.Drawing.Size(302, 22)
        Me.txtAkun.StyleController = Me.LayoutControl1
        Me.txtAkun.TabIndex = 7
        '
        'txtCatatan
        '
        Me.txtCatatan.EnterMoveNextControl = True
        Me.txtCatatan.Location = New System.Drawing.Point(61, 90)
        Me.txtCatatan.Name = "txtCatatan"
        Me.txtCatatan.Properties.Appearance.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtCatatan.Properties.Appearance.Options.UseFont = True
        Me.txtCatatan.Size = New System.Drawing.Size(302, 22)
        Me.txtCatatan.StyleController = Me.LayoutControl1
        Me.txtCatatan.TabIndex = 15
        '
        'cmdClose
        '
        Me.cmdClose.Anchor = System.Windows.Forms.AnchorStyles.Bottom
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
        Me.cmdClose.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleLeft
        Me.cmdClose.Location = New System.Drawing.Point(190, 129)
        Me.cmdClose.Name = "cmdClose"
        Me.cmdClose.Size = New System.Drawing.Size(173, 23)
        Me.cmdClose.StyleController = Me.LayoutControl1
        Me.cmdClose.TabIndex = 18
        Me.cmdClose.Text = "&Close"
        '
        'cmdSave
        '
        Me.cmdSave.Anchor = System.Windows.Forms.AnchorStyles.Bottom
        Me.cmdSave.Appearance.BackColor = System.Drawing.Color.Snow
        Me.cmdSave.Appearance.BackColor2 = System.Drawing.Color.PowderBlue
        Me.cmdSave.Appearance.BorderColor = System.Drawing.SystemColors.ButtonFace
        Me.cmdSave.Appearance.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdSave.Appearance.ForeColor = System.Drawing.Color.Black
        Me.cmdSave.Appearance.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.ForwardDiagonal
        Me.cmdSave.Appearance.Options.UseBackColor = True
        Me.cmdSave.Appearance.Options.UseBorderColor = True
        Me.cmdSave.Appearance.Options.UseFont = True
        Me.cmdSave.Appearance.Options.UseForeColor = True
        Me.cmdSave.Cursor = System.Windows.Forms.Cursors.Hand
        Me.cmdSave.ImageIndex = 2
        Me.cmdSave.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleLeft
        Me.cmdSave.Location = New System.Drawing.Point(12, 129)
        Me.cmdSave.Name = "cmdSave"
        Me.cmdSave.Size = New System.Drawing.Size(174, 23)
        Me.cmdSave.StyleController = Me.LayoutControl1
        Me.cmdSave.TabIndex = 17
        Me.cmdSave.Text = "&Save"
        '
        'LayoutControlGroup1
        '
        Me.LayoutControlGroup1.CustomizationFormText = "Root"
        Me.LayoutControlGroup1.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.[True]
        Me.LayoutControlGroup1.GroupBordersVisible = False
        Me.LayoutControlGroup1.Items.AddRange(New DevExpress.XtraLayout.BaseLayoutItem() {Me.LayoutControlItem1, Me.LayoutControlItem4, Me.LayoutControlItem7, Me.LayoutControlItem8, Me.EmptySpaceItem1, Me.LayoutControlItem10, Me.LayoutControlItem11})
        Me.LayoutControlGroup1.Location = New System.Drawing.Point(0, 0)
        Me.LayoutControlGroup1.Name = "Root"
        Me.LayoutControlGroup1.Size = New System.Drawing.Size(375, 164)
        Me.LayoutControlGroup1.Text = "Root"
        Me.LayoutControlGroup1.TextVisible = False
        '
        'LayoutControlItem1
        '
        Me.LayoutControlItem1.Control = Me.txtGudang
        Me.LayoutControlItem1.CustomizationFormText = "Gudang"
        Me.LayoutControlItem1.Location = New System.Drawing.Point(0, 0)
        Me.LayoutControlItem1.Name = "LayoutControlItem1"
        Me.LayoutControlItem1.Size = New System.Drawing.Size(355, 26)
        Me.LayoutControlItem1.Text = "Divisi"
        Me.LayoutControlItem1.TextSize = New System.Drawing.Size(45, 13)
        '
        'LayoutControlItem4
        '
        Me.LayoutControlItem4.Control = Me.txtAkun
        Me.LayoutControlItem4.CustomizationFormText = "Perkiraan"
        Me.LayoutControlItem4.Location = New System.Drawing.Point(0, 26)
        Me.LayoutControlItem4.Name = "LayoutControlItem4"
        Me.LayoutControlItem4.Size = New System.Drawing.Size(355, 26)
        Me.LayoutControlItem4.Text = "Perkiraan"
        Me.LayoutControlItem4.TextSize = New System.Drawing.Size(45, 13)
        '
        'LayoutControlItem7
        '
        Me.LayoutControlItem7.Control = Me.txtJml
        Me.LayoutControlItem7.CustomizationFormText = "Jumlah"
        Me.LayoutControlItem7.Location = New System.Drawing.Point(0, 52)
        Me.LayoutControlItem7.Name = "LayoutControlItem7"
        Me.LayoutControlItem7.Size = New System.Drawing.Size(355, 26)
        Me.LayoutControlItem7.Text = "Jumlah"
        Me.LayoutControlItem7.TextSize = New System.Drawing.Size(45, 13)
        '
        'LayoutControlItem8
        '
        Me.LayoutControlItem8.Control = Me.txtCatatan
        Me.LayoutControlItem8.CustomizationFormText = "Catatan"
        Me.LayoutControlItem8.Location = New System.Drawing.Point(0, 78)
        Me.LayoutControlItem8.Name = "LayoutControlItem8"
        Me.LayoutControlItem8.Size = New System.Drawing.Size(355, 26)
        Me.LayoutControlItem8.Text = "Catatan"
        Me.LayoutControlItem8.TextSize = New System.Drawing.Size(45, 13)
        '
        'EmptySpaceItem1
        '
        Me.EmptySpaceItem1.CustomizationFormText = "EmptySpaceItem1"
        Me.EmptySpaceItem1.Location = New System.Drawing.Point(0, 104)
        Me.EmptySpaceItem1.Name = "EmptySpaceItem1"
        Me.EmptySpaceItem1.Size = New System.Drawing.Size(355, 13)
        Me.EmptySpaceItem1.Text = "EmptySpaceItem1"
        Me.EmptySpaceItem1.TextSize = New System.Drawing.Size(0, 0)
        '
        'LayoutControlItem10
        '
        Me.LayoutControlItem10.Control = Me.cmdSave
        Me.LayoutControlItem10.CustomizationFormText = "Save"
        Me.LayoutControlItem10.Location = New System.Drawing.Point(0, 117)
        Me.LayoutControlItem10.Name = "LayoutControlItem10"
        Me.LayoutControlItem10.Size = New System.Drawing.Size(178, 27)
        Me.LayoutControlItem10.Text = "Save"
        Me.LayoutControlItem10.TextSize = New System.Drawing.Size(0, 0)
        Me.LayoutControlItem10.TextToControlDistance = 0
        Me.LayoutControlItem10.TextVisible = False
        '
        'LayoutControlItem11
        '
        Me.LayoutControlItem11.Control = Me.cmdClose
        Me.LayoutControlItem11.CustomizationFormText = "Cancel"
        Me.LayoutControlItem11.Location = New System.Drawing.Point(178, 117)
        Me.LayoutControlItem11.Name = "LayoutControlItem11"
        Me.LayoutControlItem11.Size = New System.Drawing.Size(177, 27)
        Me.LayoutControlItem11.Text = "Cancel"
        Me.LayoutControlItem11.TextSize = New System.Drawing.Size(0, 0)
        Me.LayoutControlItem11.TextToControlDistance = 0
        Me.LayoutControlItem11.TextVisible = False
        '
        'frmEntriKasIND
        '
        Me.Appearance.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Appearance.Options.UseFont = True
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(375, 164)
        Me.Controls.Add(Me.LayoutControl1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.Name = "frmEntriKasIND"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Entri Item Kas Masuk"
        CType(Me.LayoutControl1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.LayoutControl1.ResumeLayout(False)
        CType(Me.txtGudang.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtJml.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtAkun.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtCatatan.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlGroup1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem4, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem7, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem8, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.EmptySpaceItem1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem10, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem11, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents cmdClose As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents cmdSave As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents txtGudang As DevExpress.XtraEditors.ButtonEdit
    Friend WithEvents txtAkun As DevExpress.XtraEditors.ButtonEdit
    Friend WithEvents txtCatatan As DevExpress.XtraEditors.ButtonEdit
    Friend WithEvents txtJml As DevExpress.XtraEditors.ButtonEdit
    Friend WithEvents LayoutControl1 As DevExpress.XtraLayout.LayoutControl
    Friend WithEvents LayoutControlGroup1 As DevExpress.XtraLayout.LayoutControlGroup
    Friend WithEvents LayoutControlItem1 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents LayoutControlItem4 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents LayoutControlItem7 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents LayoutControlItem8 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents EmptySpaceItem1 As DevExpress.XtraLayout.EmptySpaceItem
    Friend WithEvents LayoutControlItem10 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents LayoutControlItem11 As DevExpress.XtraLayout.LayoutControlItem
End Class
