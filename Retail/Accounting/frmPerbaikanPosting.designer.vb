<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmPerbaikanPosting
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
        Me.ProgressBarControl1 = New DevExpress.XtraEditors.ProgressBarControl
        Me.cmdCancel = New DevExpress.XtraEditors.SimpleButton
        Me.cmdOK = New DevExpress.XtraEditors.SimpleButton
        Me.TglSampai = New DevExpress.XtraEditors.DateEdit
        Me.LabelControl1 = New DevExpress.XtraEditors.LabelControl
        Me.TglDari = New DevExpress.XtraEditors.DateEdit
        Me.LabelControl12 = New DevExpress.XtraEditors.LabelControl
        Me.txtBarang = New DevExpress.XtraEditors.SearchLookUpEdit
        Me.gvBarang = New DevExpress.XtraGrid.Views.Grid.GridView
        Me.LabelControl2 = New DevExpress.XtraEditors.LabelControl
        Me.lbStock = New DevExpress.XtraEditors.LabelControl
        Me.CheckEdit1 = New DevExpress.XtraEditors.CheckEdit
        Me.ckPostingUlang = New DevExpress.XtraEditors.CheckEdit
        CType(Me.ProgressBarControl1.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.TglSampai.Properties.VistaTimeProperties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.TglSampai.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.TglDari.Properties.VistaTimeProperties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.TglDari.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtBarang.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.gvBarang, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.CheckEdit1.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.ckPostingUlang.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'ProgressBarControl1
        '
        Me.ProgressBarControl1.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.ProgressBarControl1.Location = New System.Drawing.Point(0, 121)
        Me.ProgressBarControl1.Name = "ProgressBarControl1"
        Me.ProgressBarControl1.Properties.ShowTitle = True
        Me.ProgressBarControl1.Size = New System.Drawing.Size(536, 21)
        Me.ProgressBarControl1.TabIndex = 11
        Me.ProgressBarControl1.Visible = False
        '
        'cmdCancel
        '
        Me.cmdCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdCancel.Appearance.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdCancel.Appearance.Options.UseFont = True
        Me.cmdCancel.Cursor = System.Windows.Forms.Cursors.Hand
        Me.cmdCancel.ImageIndex = 10
        Me.cmdCancel.Location = New System.Drawing.Point(450, 81)
        Me.cmdCancel.Name = "cmdCancel"
        Me.cmdCancel.Size = New System.Drawing.Size(80, 34)
        Me.cmdCancel.TabIndex = 10
        Me.cmdCancel.Text = "&Cancel"
        '
        'cmdOK
        '
        Me.cmdOK.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdOK.Appearance.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdOK.Appearance.Options.UseFont = True
        Me.cmdOK.Cursor = System.Windows.Forms.Cursors.Hand
        Me.cmdOK.ImageIndex = 4
        Me.cmdOK.Location = New System.Drawing.Point(364, 81)
        Me.cmdOK.Name = "cmdOK"
        Me.cmdOK.Size = New System.Drawing.Size(80, 34)
        Me.cmdOK.TabIndex = 9
        Me.cmdOK.Text = "&Ok"
        '
        'TglSampai
        '
        Me.TglSampai.EditValue = Nothing
        Me.TglSampai.EnterMoveNextControl = True
        Me.TglSampai.Location = New System.Drawing.Point(323, 12)
        Me.TglSampai.Name = "TglSampai"
        Me.TglSampai.Properties.AllowNullInput = DevExpress.Utils.DefaultBoolean.[False]
        Me.TglSampai.Properties.Appearance.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TglSampai.Properties.Appearance.Options.UseFont = True
        Me.TglSampai.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.TglSampai.Properties.Mask.EditMask = "D"
        Me.TglSampai.Properties.Mask.UseMaskAsDisplayFormat = True
        Me.TglSampai.Properties.VistaTimeProperties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton})
        Me.TglSampai.Size = New System.Drawing.Size(207, 22)
        Me.TglSampai.TabIndex = 3
        '
        'LabelControl1
        '
        Me.LabelControl1.Appearance.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LabelControl1.Location = New System.Drawing.Point(299, 15)
        Me.LabelControl1.Name = "LabelControl1"
        Me.LabelControl1.Size = New System.Drawing.Size(18, 16)
        Me.LabelControl1.TabIndex = 2
        Me.LabelControl1.Text = "s/d"
        '
        'TglDari
        '
        Me.TglDari.EditValue = Nothing
        Me.TglDari.EnterMoveNextControl = True
        Me.TglDari.Location = New System.Drawing.Point(86, 12)
        Me.TglDari.Name = "TglDari"
        Me.TglDari.Properties.Appearance.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TglDari.Properties.Appearance.Options.UseFont = True
        Me.TglDari.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.TglDari.Properties.Mask.EditMask = "D"
        Me.TglDari.Properties.Mask.UseMaskAsDisplayFormat = True
        Me.TglDari.Properties.VistaTimeProperties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton})
        Me.TglDari.Size = New System.Drawing.Size(207, 22)
        Me.TglDari.TabIndex = 1
        '
        'LabelControl12
        '
        Me.LabelControl12.Appearance.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LabelControl12.Location = New System.Drawing.Point(7, 15)
        Me.LabelControl12.Name = "LabelControl12"
        Me.LabelControl12.Size = New System.Drawing.Size(73, 16)
        Me.LabelControl12.TabIndex = 0
        Me.LabelControl12.Text = "Dari Tanggal"
        '
        'txtBarang
        '
        Me.txtBarang.EditValue = ""
        Me.txtBarang.EnterMoveNextControl = True
        Me.txtBarang.Location = New System.Drawing.Point(86, 40)
        Me.txtBarang.Name = "txtBarang"
        Me.txtBarang.Properties.Appearance.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtBarang.Properties.Appearance.Options.UseFont = True
        Me.txtBarang.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo), New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Delete)})
        Me.txtBarang.Properties.DisplayMember = "Kode"
        Me.txtBarang.Properties.NullText = ""
        Me.txtBarang.Properties.ValueMember = "NoID"
        Me.txtBarang.Properties.View = Me.gvBarang
        Me.txtBarang.Size = New System.Drawing.Size(207, 22)
        Me.txtBarang.TabIndex = 5
        '
        'gvBarang
        '
        Me.gvBarang.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus
        Me.gvBarang.Name = "gvBarang"
        Me.gvBarang.OptionsSelection.EnableAppearanceFocusedCell = False
        Me.gvBarang.OptionsView.ShowGroupPanel = False
        '
        'LabelControl2
        '
        Me.LabelControl2.Appearance.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LabelControl2.Location = New System.Drawing.Point(7, 43)
        Me.LabelControl2.Name = "LabelControl2"
        Me.LabelControl2.Size = New System.Drawing.Size(40, 16)
        Me.LabelControl2.TabIndex = 4
        Me.LabelControl2.Text = "Barang"
        '
        'lbStock
        '
        Me.lbStock.Appearance.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbStock.Location = New System.Drawing.Point(299, 43)
        Me.lbStock.Name = "lbStock"
        Me.lbStock.Size = New System.Drawing.Size(5, 16)
        Me.lbStock.TabIndex = 6
        Me.lbStock.Text = "-"
        '
        'CheckEdit1
        '
        Me.CheckEdit1.Location = New System.Drawing.Point(84, 96)
        Me.CheckEdit1.Name = "CheckEdit1"
        Me.CheckEdit1.Properties.Caption = "Update HPP ke Master Stock/Barang "
        Me.CheckEdit1.Size = New System.Drawing.Size(232, 19)
        Me.CheckEdit1.TabIndex = 8
        '
        'ckPostingUlang
        '
        Me.ckPostingUlang.Location = New System.Drawing.Point(84, 71)
        Me.ckPostingUlang.Name = "ckPostingUlang"
        Me.ckPostingUlang.Properties.Caption = "UnPosting lalu Posting Ulang"
        Me.ckPostingUlang.Size = New System.Drawing.Size(232, 19)
        Me.ckPostingUlang.TabIndex = 7
        Me.ckPostingUlang.Visible = False
        '
        'frmPerbaikanPosting
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(536, 142)
        Me.Controls.Add(Me.ckPostingUlang)
        Me.Controls.Add(Me.CheckEdit1)
        Me.Controls.Add(Me.lbStock)
        Me.Controls.Add(Me.LabelControl2)
        Me.Controls.Add(Me.txtBarang)
        Me.Controls.Add(Me.cmdCancel)
        Me.Controls.Add(Me.cmdOK)
        Me.Controls.Add(Me.TglSampai)
        Me.Controls.Add(Me.LabelControl1)
        Me.Controls.Add(Me.TglDari)
        Me.Controls.Add(Me.LabelControl12)
        Me.Controls.Add(Me.ProgressBarControl1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.Name = "frmPerbaikanPosting"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Perbaikan Stock"
        CType(Me.ProgressBarControl1.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.TglSampai.Properties.VistaTimeProperties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.TglSampai.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.TglDari.Properties.VistaTimeProperties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.TglDari.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtBarang.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.gvBarang, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.CheckEdit1.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.ckPostingUlang.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents ProgressBarControl1 As DevExpress.XtraEditors.ProgressBarControl
    Friend WithEvents cmdCancel As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents cmdOK As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents TglSampai As DevExpress.XtraEditors.DateEdit
    Friend WithEvents LabelControl1 As DevExpress.XtraEditors.LabelControl
    Friend WithEvents TglDari As DevExpress.XtraEditors.DateEdit
    Friend WithEvents LabelControl12 As DevExpress.XtraEditors.LabelControl
    Friend WithEvents txtBarang As DevExpress.XtraEditors.SearchLookUpEdit
    Friend WithEvents gvBarang As DevExpress.XtraGrid.Views.Grid.GridView
    Friend WithEvents LabelControl2 As DevExpress.XtraEditors.LabelControl
    Friend WithEvents lbStock As DevExpress.XtraEditors.LabelControl
    Friend WithEvents CheckEdit1 As DevExpress.XtraEditors.CheckEdit
    Friend WithEvents ckPostingUlang As DevExpress.XtraEditors.CheckEdit
End Class
