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
        Me.txtTglSampai = New DevExpress.XtraEditors.DateEdit
        Me.LabelControl1 = New DevExpress.XtraEditors.LabelControl
        Me.txtTglDari = New DevExpress.XtraEditors.DateEdit
        Me.LabelControl12 = New DevExpress.XtraEditors.LabelControl
        CType(Me.ProgressBarControl1.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtTglSampai.Properties.VistaTimeProperties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtTglSampai.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtTglDari.Properties.VistaTimeProperties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtTglDari.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'ProgressBarControl1
        '
        Me.ProgressBarControl1.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.ProgressBarControl1.Location = New System.Drawing.Point(0, 80)
        Me.ProgressBarControl1.Name = "ProgressBarControl1"
        Me.ProgressBarControl1.Properties.ShowTitle = True
        Me.ProgressBarControl1.Size = New System.Drawing.Size(536, 21)
        Me.ProgressBarControl1.TabIndex = 1
        Me.ProgressBarControl1.Visible = False
        '
        'cmdCancel
        '
        Me.cmdCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdCancel.Appearance.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdCancel.Appearance.Options.UseFont = True
        Me.cmdCancel.Cursor = System.Windows.Forms.Cursors.Hand
        Me.cmdCancel.ImageIndex = 10
        Me.cmdCancel.Location = New System.Drawing.Point(450, 40)
        Me.cmdCancel.Name = "cmdCancel"
        Me.cmdCancel.Size = New System.Drawing.Size(80, 34)
        Me.cmdCancel.TabIndex = 13
        Me.cmdCancel.Text = "&Cancel"
        '
        'cmdOK
        '
        Me.cmdOK.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdOK.Appearance.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdOK.Appearance.Options.UseFont = True
        Me.cmdOK.Cursor = System.Windows.Forms.Cursors.Hand
        Me.cmdOK.ImageIndex = 4
        Me.cmdOK.Location = New System.Drawing.Point(364, 40)
        Me.cmdOK.Name = "cmdOK"
        Me.cmdOK.Size = New System.Drawing.Size(80, 34)
        Me.cmdOK.TabIndex = 12
        Me.cmdOK.Text = "&Ok"
        '
        'txtTglSampai
        '
        Me.txtTglSampai.EditValue = Nothing
        Me.txtTglSampai.EnterMoveNextControl = True
        Me.txtTglSampai.Location = New System.Drawing.Point(323, 12)
        Me.txtTglSampai.Name = "txtTglSampai"
        Me.txtTglSampai.Properties.AllowNullInput = DevExpress.Utils.DefaultBoolean.[False]
        Me.txtTglSampai.Properties.Appearance.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtTglSampai.Properties.Appearance.Options.UseFont = True
        Me.txtTglSampai.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.txtTglSampai.Properties.Mask.EditMask = "D"
        Me.txtTglSampai.Properties.Mask.UseMaskAsDisplayFormat = True
        Me.txtTglSampai.Properties.VistaTimeProperties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton})
        Me.txtTglSampai.Size = New System.Drawing.Size(207, 22)
        Me.txtTglSampai.TabIndex = 11
        '
        'LabelControl1
        '
        Me.LabelControl1.Appearance.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LabelControl1.Location = New System.Drawing.Point(299, 15)
        Me.LabelControl1.Name = "LabelControl1"
        Me.LabelControl1.Size = New System.Drawing.Size(18, 16)
        Me.LabelControl1.TabIndex = 10
        Me.LabelControl1.Text = "s/d"
        '
        'txtTglDari
        '
        Me.txtTglDari.EditValue = Nothing
        Me.txtTglDari.EnterMoveNextControl = True
        Me.txtTglDari.Location = New System.Drawing.Point(86, 12)
        Me.txtTglDari.Name = "txtTglDari"
        Me.txtTglDari.Properties.Appearance.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtTglDari.Properties.Appearance.Options.UseFont = True
        Me.txtTglDari.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.txtTglDari.Properties.Mask.EditMask = "D"
        Me.txtTglDari.Properties.Mask.UseMaskAsDisplayFormat = True
        Me.txtTglDari.Properties.VistaTimeProperties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton})
        Me.txtTglDari.Size = New System.Drawing.Size(207, 22)
        Me.txtTglDari.TabIndex = 9
        '
        'LabelControl12
        '
        Me.LabelControl12.Appearance.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LabelControl12.Location = New System.Drawing.Point(7, 15)
        Me.LabelControl12.Name = "LabelControl12"
        Me.LabelControl12.Size = New System.Drawing.Size(73, 16)
        Me.LabelControl12.TabIndex = 8
        Me.LabelControl12.Text = "Dari Tanggal"
        '
        'frmPerbaikanPosting
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(536, 101)
        Me.Controls.Add(Me.cmdCancel)
        Me.Controls.Add(Me.cmdOK)
        Me.Controls.Add(Me.txtTglSampai)
        Me.Controls.Add(Me.LabelControl1)
        Me.Controls.Add(Me.txtTglDari)
        Me.Controls.Add(Me.LabelControl12)
        Me.Controls.Add(Me.ProgressBarControl1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.Name = "frmPerbaikanPosting"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Perbaikan Stock"
        CType(Me.ProgressBarControl1.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtTglSampai.Properties.VistaTimeProperties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtTglSampai.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtTglDari.Properties.VistaTimeProperties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtTglDari.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents ProgressBarControl1 As DevExpress.XtraEditors.ProgressBarControl
    Friend WithEvents cmdCancel As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents cmdOK As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents txtTglSampai As DevExpress.XtraEditors.DateEdit
    Friend WithEvents LabelControl1 As DevExpress.XtraEditors.LabelControl
    Friend WithEvents txtTglDari As DevExpress.XtraEditors.DateEdit
    Friend WithEvents LabelControl12 As DevExpress.XtraEditors.LabelControl
End Class
