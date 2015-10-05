<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmCekPoin
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
        Me.lbKassa = New DevExpress.XtraEditors.LabelControl
        Me.txtPath = New DevExpress.XtraEditors.ButtonEdit
        Me.LabelControl1 = New DevExpress.XtraEditors.LabelControl
        Me.cmdTutup = New DevExpress.XtraEditors.SimpleButton
        Me.txtHargaJual = New DevExpress.XtraEditors.TextEdit
        Me.LabelControl6 = New DevExpress.XtraEditors.LabelControl
        Me.txtNama = New DevExpress.XtraEditors.ButtonEdit
        Me.LabelControl7 = New DevExpress.XtraEditors.LabelControl
        Me.txtSatuan = New DevExpress.XtraEditors.ButtonEdit
        Me.GridControl1 = New DevExpress.XtraGrid.GridControl
        Me.GridView1 = New DevExpress.XtraGrid.Views.Grid.GridView
        Me.GridView2 = New DevExpress.XtraGrid.Views.Grid.GridView
        Me.SimpleButton1 = New DevExpress.XtraEditors.SimpleButton
        Me.SimpleButton2 = New DevExpress.XtraEditors.SimpleButton
        CType(Me.txtPath.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtHargaJual.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtNama.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtSatuan.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.GridControl1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.GridView1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.GridView2, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'lbKassa
        '
        Me.lbKassa.Appearance.Font = New System.Drawing.Font("Calibri", 18.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbKassa.Cursor = System.Windows.Forms.Cursors.Hand
        Me.lbKassa.Location = New System.Drawing.Point(9, 149)
        Me.lbKassa.Name = "lbKassa"
        Me.lbKassa.Size = New System.Drawing.Size(101, 29)
        Me.lbKassa.TabIndex = 39
        Me.lbKassa.Text = "Total Poin"
        '
        'txtPath
        '
        Me.txtPath.Location = New System.Drawing.Point(302, 23)
        Me.txtPath.Name = "txtPath"
        Me.txtPath.Properties.Appearance.Font = New System.Drawing.Font("Calibri", 18.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtPath.Properties.Appearance.Options.UseFont = True
        Me.txtPath.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton})
        Me.txtPath.Size = New System.Drawing.Size(442, 36)
        Me.txtPath.TabIndex = 40
        '
        'LabelControl1
        '
        Me.LabelControl1.Appearance.Font = New System.Drawing.Font("Calibri", 18.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LabelControl1.Cursor = System.Windows.Forms.Cursors.Hand
        Me.LabelControl1.Location = New System.Drawing.Point(9, 26)
        Me.LabelControl1.Name = "LabelControl1"
        Me.LabelControl1.Size = New System.Drawing.Size(253, 29)
        Me.LabelControl1.TabIndex = 41
        Me.LabelControl1.Text = "Masukkan Kode/Barcode"
        '
        'cmdTutup
        '
        Me.cmdTutup.Appearance.Font = New System.Drawing.Font("Calibri", 18.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdTutup.Appearance.ForeColor = System.Drawing.Color.Blue
        Me.cmdTutup.Appearance.Options.UseFont = True
        Me.cmdTutup.Appearance.Options.UseForeColor = True
        Me.cmdTutup.ImageIndex = 11
        Me.cmdTutup.Location = New System.Drawing.Point(625, 608)
        Me.cmdTutup.Name = "cmdTutup"
        Me.cmdTutup.Size = New System.Drawing.Size(119, 52)
        Me.cmdTutup.TabIndex = 47
        Me.cmdTutup.Text = "&Tutup"
        '
        'txtHargaJual
        '
        Me.txtHargaJual.EditValue = New Decimal(New Integer() {0, 0, 0, 0})
        Me.txtHargaJual.EnterMoveNextControl = True
        Me.txtHargaJual.Location = New System.Drawing.Point(221, 146)
        Me.txtHargaJual.Name = "txtHargaJual"
        Me.txtHargaJual.Properties.Appearance.Font = New System.Drawing.Font("Calibri", 18.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtHargaJual.Properties.Appearance.Options.UseFont = True
        Me.txtHargaJual.Properties.Mask.EditMask = "n2"
        Me.txtHargaJual.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric
        Me.txtHargaJual.Properties.Mask.UseMaskAsDisplayFormat = True
        Me.txtHargaJual.Properties.ReadOnly = True
        Me.txtHargaJual.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.txtHargaJual.Size = New System.Drawing.Size(272, 36)
        Me.txtHargaJual.TabIndex = 48
        '
        'LabelControl6
        '
        Me.LabelControl6.Appearance.Font = New System.Drawing.Font("Calibri", 18.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LabelControl6.Cursor = System.Windows.Forms.Cursors.Hand
        Me.LabelControl6.Location = New System.Drawing.Point(9, 67)
        Me.LabelControl6.Name = "LabelControl6"
        Me.LabelControl6.Size = New System.Drawing.Size(152, 29)
        Me.LabelControl6.TabIndex = 54
        Me.LabelControl6.Text = "Nama Member"
        '
        'txtNama
        '
        Me.txtNama.Location = New System.Drawing.Point(221, 64)
        Me.txtNama.Name = "txtNama"
        Me.txtNama.Properties.Appearance.Font = New System.Drawing.Font("Calibri", 18.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtNama.Properties.Appearance.Options.UseFont = True
        Me.txtNama.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton})
        Me.txtNama.Size = New System.Drawing.Size(523, 36)
        Me.txtNama.TabIndex = 53
        '
        'LabelControl7
        '
        Me.LabelControl7.Appearance.Font = New System.Drawing.Font("Calibri", 18.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LabelControl7.Cursor = System.Windows.Forms.Cursors.Hand
        Me.LabelControl7.Location = New System.Drawing.Point(9, 108)
        Me.LabelControl7.Name = "LabelControl7"
        Me.LabelControl7.Size = New System.Drawing.Size(73, 29)
        Me.LabelControl7.TabIndex = 56
        Me.LabelControl7.Text = "Alamat"
        '
        'txtSatuan
        '
        Me.txtSatuan.Location = New System.Drawing.Point(221, 105)
        Me.txtSatuan.Name = "txtSatuan"
        Me.txtSatuan.Properties.Appearance.Font = New System.Drawing.Font("Calibri", 18.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtSatuan.Properties.Appearance.Options.UseFont = True
        Me.txtSatuan.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton})
        Me.txtSatuan.Size = New System.Drawing.Size(523, 36)
        Me.txtSatuan.TabIndex = 55
        '
        'GridControl1
        '
        Me.GridControl1.Location = New System.Drawing.Point(12, 188)
        Me.GridControl1.MainView = Me.GridView1
        Me.GridControl1.Name = "GridControl1"
        Me.GridControl1.Size = New System.Drawing.Size(737, 414)
        Me.GridControl1.TabIndex = 57
        Me.GridControl1.ViewCollection.AddRange(New DevExpress.XtraGrid.Views.Base.BaseView() {Me.GridView1, Me.GridView2})
        '
        'GridView1
        '
        Me.GridView1.Appearance.HeaderPanel.Font = New System.Drawing.Font("Tahoma", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GridView1.Appearance.HeaderPanel.Options.UseFont = True
        Me.GridView1.Appearance.Row.Font = New System.Drawing.Font("Tahoma", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GridView1.Appearance.Row.Options.UseFont = True
        Me.GridView1.GridControl = Me.GridControl1
        Me.GridView1.Name = "GridView1"
        Me.GridView1.OptionsView.ShowFooter = True
        Me.GridView1.OptionsView.ShowGroupPanel = False
        '
        'GridView2
        '
        Me.GridView2.GridControl = Me.GridControl1
        Me.GridView2.Name = "GridView2"
        '
        'SimpleButton1
        '
        Me.SimpleButton1.Appearance.Font = New System.Drawing.Font("Calibri", 18.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.SimpleButton1.Appearance.ForeColor = System.Drawing.Color.Blue
        Me.SimpleButton1.Appearance.Options.UseFont = True
        Me.SimpleButton1.Appearance.Options.UseForeColor = True
        Me.SimpleButton1.ImageIndex = 11
        Me.SimpleButton1.Location = New System.Drawing.Point(460, 608)
        Me.SimpleButton1.Name = "SimpleButton1"
        Me.SimpleButton1.Size = New System.Drawing.Size(159, 52)
        Me.SimpleButton1.TabIndex = 58
        Me.SimpleButton1.Text = "&Clear All"
        Me.SimpleButton1.Visible = False
        '
        'SimpleButton2
        '
        Me.SimpleButton2.Appearance.Font = New System.Drawing.Font("Calibri", 18.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.SimpleButton2.Appearance.ForeColor = System.Drawing.Color.Blue
        Me.SimpleButton2.Appearance.Options.UseFont = True
        Me.SimpleButton2.Appearance.Options.UseForeColor = True
        Me.SimpleButton2.ImageIndex = 11
        Me.SimpleButton2.Location = New System.Drawing.Point(302, 608)
        Me.SimpleButton2.Name = "SimpleButton2"
        Me.SimpleButton2.Size = New System.Drawing.Size(151, 52)
        Me.SimpleButton2.TabIndex = 59
        Me.SimpleButton2.Text = "&Delete Item"
        Me.SimpleButton2.Visible = False
        '
        'frmCekPoin
        '
        Me.Appearance.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(128, Byte), Integer))
        Me.Appearance.Options.UseBackColor = True
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(758, 672)
        Me.Controls.Add(Me.SimpleButton2)
        Me.Controls.Add(Me.SimpleButton1)
        Me.Controls.Add(Me.GridControl1)
        Me.Controls.Add(Me.LabelControl7)
        Me.Controls.Add(Me.txtSatuan)
        Me.Controls.Add(Me.LabelControl6)
        Me.Controls.Add(Me.txtNama)
        Me.Controls.Add(Me.txtHargaJual)
        Me.Controls.Add(Me.cmdTutup)
        Me.Controls.Add(Me.LabelControl1)
        Me.Controls.Add(Me.txtPath)
        Me.Controls.Add(Me.lbKassa)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.Name = "frmCekPoin"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "CEK POIN CUSTOMER"
        CType(Me.txtPath.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtHargaJual.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtNama.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtSatuan.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.GridControl1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.GridView1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.GridView2, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents lbKassa As DevExpress.XtraEditors.LabelControl
    Friend WithEvents txtPath As DevExpress.XtraEditors.ButtonEdit
    Friend WithEvents LabelControl1 As DevExpress.XtraEditors.LabelControl
    Friend WithEvents cmdTutup As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents txtHargaJual As DevExpress.XtraEditors.TextEdit
    Friend WithEvents LabelControl6 As DevExpress.XtraEditors.LabelControl
    Friend WithEvents txtNama As DevExpress.XtraEditors.ButtonEdit
    Friend WithEvents LabelControl7 As DevExpress.XtraEditors.LabelControl
    Friend WithEvents txtSatuan As DevExpress.XtraEditors.ButtonEdit
    Friend WithEvents GridControl1 As DevExpress.XtraGrid.GridControl
    Friend WithEvents GridView1 As DevExpress.XtraGrid.Views.Grid.GridView
    Friend WithEvents GridView2 As DevExpress.XtraGrid.Views.Grid.GridView
    Friend WithEvents SimpleButton1 As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents SimpleButton2 As DevExpress.XtraEditors.SimpleButton
End Class
