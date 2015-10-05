<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmPindahKodeBarang
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
        Me.LayoutControl1 = New DevExpress.XtraLayout.LayoutControl
        Me.txtNama = New DevExpress.XtraEditors.TextEdit
        Me.txtKode = New DevExpress.XtraEditors.TextEdit
        Me.txtBarcode = New DevExpress.XtraEditors.TextEdit
        Me.cmdTutup = New DevExpress.XtraEditors.SimpleButton
        Me.cmdSave = New DevExpress.XtraEditors.SimpleButton
        Me.txtBarang = New DevExpress.XtraEditors.SearchLookUpEdit
        Me.gvBarang = New DevExpress.XtraGrid.Views.Grid.GridView
        Me.LayoutControlGroup1 = New DevExpress.XtraLayout.LayoutControlGroup
        Me.LayoutControlItem2 = New DevExpress.XtraLayout.LayoutControlItem
        Me.LayoutControlItem3 = New DevExpress.XtraLayout.LayoutControlItem
        Me.LayoutControlItem4 = New DevExpress.XtraLayout.LayoutControlItem
        Me.LayoutControlItem5 = New DevExpress.XtraLayout.LayoutControlItem
        Me.LayoutControlItem6 = New DevExpress.XtraLayout.LayoutControlItem
        Me.LayoutControlItem7 = New DevExpress.XtraLayout.LayoutControlItem
        CType(Me.LayoutControl1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.LayoutControl1.SuspendLayout()
        CType(Me.txtNama.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtKode.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtBarcode.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtBarang.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.gvBarang, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlGroup1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem3, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem4, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem5, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem6, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem7, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'LayoutControl1
        '
        Me.LayoutControl1.Controls.Add(Me.txtNama)
        Me.LayoutControl1.Controls.Add(Me.txtKode)
        Me.LayoutControl1.Controls.Add(Me.txtBarcode)
        Me.LayoutControl1.Controls.Add(Me.cmdTutup)
        Me.LayoutControl1.Controls.Add(Me.cmdSave)
        Me.LayoutControl1.Controls.Add(Me.txtBarang)
        Me.LayoutControl1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.LayoutControl1.Location = New System.Drawing.Point(0, 0)
        Me.LayoutControl1.Name = "LayoutControl1"
        Me.LayoutControl1.Root = Me.LayoutControlGroup1
        Me.LayoutControl1.Size = New System.Drawing.Size(459, 157)
        Me.LayoutControl1.TabIndex = 0
        Me.LayoutControl1.Text = "LayoutControl1"
        '
        'txtNama
        '
        Me.txtNama.Location = New System.Drawing.Point(80, 60)
        Me.txtNama.Name = "txtNama"
        Me.txtNama.Size = New System.Drawing.Size(367, 20)
        Me.txtNama.StyleController = Me.LayoutControl1
        Me.txtNama.TabIndex = 25
        '
        'txtKode
        '
        Me.txtKode.Location = New System.Drawing.Point(80, 36)
        Me.txtKode.Name = "txtKode"
        Me.txtKode.Size = New System.Drawing.Size(367, 20)
        Me.txtKode.StyleController = Me.LayoutControl1
        Me.txtKode.TabIndex = 24
        '
        'txtBarcode
        '
        Me.txtBarcode.Location = New System.Drawing.Point(80, 12)
        Me.txtBarcode.Name = "txtBarcode"
        Me.txtBarcode.Size = New System.Drawing.Size(367, 20)
        Me.txtBarcode.StyleController = Me.LayoutControl1
        Me.txtBarcode.TabIndex = 23
        '
        'cmdTutup
        '
        Me.cmdTutup.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdTutup.Appearance.Font = New System.Drawing.Font("Tahoma", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdTutup.Appearance.ForeColor = System.Drawing.Color.Blue
        Me.cmdTutup.Appearance.Options.UseFont = True
        Me.cmdTutup.Appearance.Options.UseForeColor = True
        Me.cmdTutup.ImageIndex = 11
        Me.cmdTutup.Location = New System.Drawing.Point(246, 110)
        Me.cmdTutup.Name = "cmdTutup"
        Me.cmdTutup.Size = New System.Drawing.Size(201, 25)
        Me.cmdTutup.StyleController = Me.LayoutControl1
        Me.cmdTutup.TabIndex = 22
        Me.cmdTutup.Text = "&Tutup"
        '
        'cmdSave
        '
        Me.cmdSave.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdSave.Appearance.Font = New System.Drawing.Font("Tahoma", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdSave.Appearance.ForeColor = System.Drawing.Color.Blue
        Me.cmdSave.Appearance.Options.UseFont = True
        Me.cmdSave.Appearance.Options.UseForeColor = True
        Me.cmdSave.ImageIndex = 10
        Me.cmdSave.Location = New System.Drawing.Point(12, 110)
        Me.cmdSave.Name = "cmdSave"
        Me.cmdSave.Size = New System.Drawing.Size(230, 25)
        Me.cmdSave.StyleController = Me.LayoutControl1
        Me.cmdSave.TabIndex = 21
        Me.cmdSave.Text = "&Simpan"
        Me.cmdSave.Visible = False
        '
        'txtBarang
        '
        Me.txtBarang.EditValue = ""
        Me.txtBarang.EnterMoveNextControl = True
        Me.txtBarang.Location = New System.Drawing.Point(80, 84)
        Me.txtBarang.Name = "txtBarang"
        Me.txtBarang.Properties.Appearance.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtBarang.Properties.Appearance.Options.UseFont = True
        Me.txtBarang.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.txtBarang.Properties.DisplayMember = "Kode"
        Me.txtBarang.Properties.NullText = ""
        Me.txtBarang.Properties.ValueMember = "NoID"
        Me.txtBarang.Properties.View = Me.gvBarang
        Me.txtBarang.Size = New System.Drawing.Size(367, 22)
        Me.txtBarang.StyleController = Me.LayoutControl1
        Me.txtBarang.TabIndex = 7
        '
        'gvBarang
        '
        Me.gvBarang.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus
        Me.gvBarang.Name = "gvBarang"
        Me.gvBarang.OptionsSelection.EnableAppearanceFocusedCell = False
        Me.gvBarang.OptionsView.ShowGroupPanel = False
        '
        'LayoutControlGroup1
        '
        Me.LayoutControlGroup1.CustomizationFormText = "LayoutControlGroup1"
        Me.LayoutControlGroup1.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.[True]
        Me.LayoutControlGroup1.GroupBordersVisible = False
        Me.LayoutControlGroup1.Items.AddRange(New DevExpress.XtraLayout.BaseLayoutItem() {Me.LayoutControlItem2, Me.LayoutControlItem3, Me.LayoutControlItem4, Me.LayoutControlItem5, Me.LayoutControlItem6, Me.LayoutControlItem7})
        Me.LayoutControlGroup1.Location = New System.Drawing.Point(0, 0)
        Me.LayoutControlGroup1.Name = "LayoutControlGroup1"
        Me.LayoutControlGroup1.Size = New System.Drawing.Size(459, 157)
        Me.LayoutControlGroup1.Text = "LayoutControlGroup1"
        Me.LayoutControlGroup1.TextVisible = False
        '
        'LayoutControlItem2
        '
        Me.LayoutControlItem2.Control = Me.txtBarang
        Me.LayoutControlItem2.CustomizationFormText = "Menjadi Kode"
        Me.LayoutControlItem2.Location = New System.Drawing.Point(0, 72)
        Me.LayoutControlItem2.Name = "LayoutControlItem2"
        Me.LayoutControlItem2.Size = New System.Drawing.Size(439, 26)
        Me.LayoutControlItem2.Text = "Menjadi Kode"
        Me.LayoutControlItem2.TextSize = New System.Drawing.Size(64, 13)
        '
        'LayoutControlItem3
        '
        Me.LayoutControlItem3.Control = Me.cmdSave
        Me.LayoutControlItem3.CustomizationFormText = "LayoutControlItem3"
        Me.LayoutControlItem3.Location = New System.Drawing.Point(0, 98)
        Me.LayoutControlItem3.Name = "LayoutControlItem3"
        Me.LayoutControlItem3.Size = New System.Drawing.Size(234, 39)
        Me.LayoutControlItem3.Text = "LayoutControlItem3"
        Me.LayoutControlItem3.TextSize = New System.Drawing.Size(0, 0)
        Me.LayoutControlItem3.TextToControlDistance = 0
        Me.LayoutControlItem3.TextVisible = False
        '
        'LayoutControlItem4
        '
        Me.LayoutControlItem4.Control = Me.cmdTutup
        Me.LayoutControlItem4.CustomizationFormText = "LayoutControlItem4"
        Me.LayoutControlItem4.Location = New System.Drawing.Point(234, 98)
        Me.LayoutControlItem4.Name = "LayoutControlItem4"
        Me.LayoutControlItem4.Size = New System.Drawing.Size(205, 39)
        Me.LayoutControlItem4.Text = "LayoutControlItem4"
        Me.LayoutControlItem4.TextSize = New System.Drawing.Size(0, 0)
        Me.LayoutControlItem4.TextToControlDistance = 0
        Me.LayoutControlItem4.TextVisible = False
        '
        'LayoutControlItem5
        '
        Me.LayoutControlItem5.Control = Me.txtBarcode
        Me.LayoutControlItem5.CustomizationFormText = "Barcode"
        Me.LayoutControlItem5.Location = New System.Drawing.Point(0, 0)
        Me.LayoutControlItem5.Name = "LayoutControlItem5"
        Me.LayoutControlItem5.Size = New System.Drawing.Size(439, 24)
        Me.LayoutControlItem5.Text = "Barcode"
        Me.LayoutControlItem5.TextSize = New System.Drawing.Size(64, 13)
        '
        'LayoutControlItem6
        '
        Me.LayoutControlItem6.Control = Me.txtKode
        Me.LayoutControlItem6.CustomizationFormText = "Kode"
        Me.LayoutControlItem6.Location = New System.Drawing.Point(0, 24)
        Me.LayoutControlItem6.Name = "LayoutControlItem6"
        Me.LayoutControlItem6.Size = New System.Drawing.Size(439, 24)
        Me.LayoutControlItem6.Text = "Kode"
        Me.LayoutControlItem6.TextSize = New System.Drawing.Size(64, 13)
        '
        'LayoutControlItem7
        '
        Me.LayoutControlItem7.Control = Me.txtNama
        Me.LayoutControlItem7.CustomizationFormText = "Nama"
        Me.LayoutControlItem7.Location = New System.Drawing.Point(0, 48)
        Me.LayoutControlItem7.Name = "LayoutControlItem7"
        Me.LayoutControlItem7.Size = New System.Drawing.Size(439, 24)
        Me.LayoutControlItem7.Text = "Nama"
        Me.LayoutControlItem7.TextSize = New System.Drawing.Size(64, 13)
        '
        'frmPindahKodeBarang
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(459, 157)
        Me.Controls.Add(Me.LayoutControl1)
        Me.Name = "frmPindahKodeBarang"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Pindah Kode Barang"
        CType(Me.LayoutControl1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.LayoutControl1.ResumeLayout(False)
        CType(Me.txtNama.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtKode.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtBarcode.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtBarang.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.gvBarang, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlGroup1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem3, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem4, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem5, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem6, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem7, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents LayoutControl1 As DevExpress.XtraLayout.LayoutControl
    Friend WithEvents LayoutControlGroup1 As DevExpress.XtraLayout.LayoutControlGroup
    Friend WithEvents txtBarang As DevExpress.XtraEditors.SearchLookUpEdit
    Friend WithEvents gvBarang As DevExpress.XtraGrid.Views.Grid.GridView
    Friend WithEvents LayoutControlItem2 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents cmdSave As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents LayoutControlItem3 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents cmdTutup As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents LayoutControlItem4 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents txtNama As DevExpress.XtraEditors.TextEdit
    Friend WithEvents txtKode As DevExpress.XtraEditors.TextEdit
    Friend WithEvents txtBarcode As DevExpress.XtraEditors.TextEdit
    Friend WithEvents LayoutControlItem5 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents LayoutControlItem6 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents LayoutControlItem7 As DevExpress.XtraLayout.LayoutControlItem
End Class
