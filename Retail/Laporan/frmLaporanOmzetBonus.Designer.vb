<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmLaporanOmzetBonus
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
        Me.LabelControl1 = New DevExpress.XtraEditors.LabelControl
        Me.txtPeriode = New DevExpress.XtraEditors.DateEdit
        Me.lbDaftar = New DevExpress.XtraEditors.LabelControl
        Me.cmdOmzetBulanan = New DevExpress.XtraEditors.SimpleButton
        Me.cmdFPSewaPKP = New DevExpress.XtraEditors.SimpleButton
        Me.cmdPPNBulanan = New DevExpress.XtraEditors.SimpleButton
        Me.cmdFPObatPKP = New DevExpress.XtraEditors.SimpleButton
        Me.cmdFPObatBulanan = New DevExpress.XtraEditors.SimpleButton
        Me.cmdTutup = New DevExpress.XtraEditors.SimpleButton
        Me.PanelControl1 = New DevExpress.XtraEditors.PanelControl
        Me.cmdHitungUlang = New DevExpress.XtraEditors.SimpleButton
        Me.cmdRefresh = New DevExpress.XtraEditors.SimpleButton
        Me.ckEdit = New DevExpress.XtraEditors.CheckEdit
        Me.cmdFPObatHarian = New DevExpress.XtraEditors.SimpleButton
        Me.CrViewer = New CrystalDecisions.Windows.Forms.CrystalReportViewer
        Me.PanelControl2 = New DevExpress.XtraEditors.PanelControl
        Me.ProgressBarControl1 = New DevExpress.XtraEditors.ProgressBarControl
        Me.GC1 = New DevExpress.XtraGrid.GridControl
        Me.GV1 = New DevExpress.XtraGrid.Views.Grid.GridView
        Me.cmdFPSewaNPKP = New DevExpress.XtraEditors.SimpleButton
        Me.cmdFPObatNPKP = New DevExpress.XtraEditors.SimpleButton
        CType(Me.txtPeriode.Properties.VistaTimeProperties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtPeriode.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PanelControl1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.PanelControl1.SuspendLayout()
        CType(Me.ckEdit.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PanelControl2, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.PanelControl2.SuspendLayout()
        CType(Me.ProgressBarControl1.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.GC1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.GV1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'LabelControl1
        '
        Me.LabelControl1.Appearance.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LabelControl1.Cursor = System.Windows.Forms.Cursors.Hand
        Me.LabelControl1.Location = New System.Drawing.Point(6, 8)
        Me.LabelControl1.Name = "LabelControl1"
        Me.LabelControl1.Size = New System.Drawing.Size(31, 16)
        Me.LabelControl1.TabIndex = 0
        Me.LabelControl1.Text = "Bulan"
        '
        'txtPeriode
        '
        Me.txtPeriode.EditValue = Nothing
        Me.txtPeriode.EnterMoveNextControl = True
        Me.txtPeriode.Location = New System.Drawing.Point(42, 5)
        Me.txtPeriode.Name = "txtPeriode"
        Me.txtPeriode.Properties.Appearance.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtPeriode.Properties.Appearance.Options.UseFont = True
        Me.txtPeriode.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.txtPeriode.Properties.DisplayFormat.FormatString = "MMMM-yyyy"
        Me.txtPeriode.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime
        Me.txtPeriode.Properties.EditFormat.FormatString = "MMMM-yyyy"
        Me.txtPeriode.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.DateTime
        Me.txtPeriode.Properties.Mask.EditMask = "MMMM-yyyy"
        Me.txtPeriode.Properties.Mask.UseMaskAsDisplayFormat = True
        Me.txtPeriode.Properties.VistaTimeProperties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton})
        Me.txtPeriode.Size = New System.Drawing.Size(153, 22)
        Me.txtPeriode.TabIndex = 1
        '
        'lbDaftar
        '
        Me.lbDaftar.Appearance.Font = New System.Drawing.Font("Rockwell", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbDaftar.Appearance.ForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.lbDaftar.Location = New System.Drawing.Point(12, 12)
        Me.lbDaftar.Name = "lbDaftar"
        Me.lbDaftar.Size = New System.Drawing.Size(297, 19)
        Me.lbDaftar.TabIndex = 0
        Me.lbDaftar.Text = "Laporan Omzet Bulanan (Accounting)"
        '
        'cmdOmzetBulanan
        '
        Me.cmdOmzetBulanan.Appearance.Font = New System.Drawing.Font("Tahoma", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdOmzetBulanan.Appearance.ForeColor = System.Drawing.Color.Blue
        Me.cmdOmzetBulanan.Appearance.Options.UseFont = True
        Me.cmdOmzetBulanan.Appearance.Options.UseForeColor = True
        Me.cmdOmzetBulanan.ImageIndex = 0
        Me.cmdOmzetBulanan.Location = New System.Drawing.Point(6, 76)
        Me.cmdOmzetBulanan.Name = "cmdOmzetBulanan"
        Me.cmdOmzetBulanan.Size = New System.Drawing.Size(190, 31)
        Me.cmdOmzetBulanan.TabIndex = 3
        Me.cmdOmzetBulanan.Text = "Omzet Bulanan"
        '
        'cmdFPSewaPKP
        '
        Me.cmdFPSewaPKP.Appearance.Font = New System.Drawing.Font("Tahoma", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdFPSewaPKP.Appearance.ForeColor = System.Drawing.Color.Blue
        Me.cmdFPSewaPKP.Appearance.Options.UseFont = True
        Me.cmdFPSewaPKP.Appearance.Options.UseForeColor = True
        Me.cmdFPSewaPKP.ImageIndex = 0
        Me.cmdFPSewaPKP.Location = New System.Drawing.Point(6, 113)
        Me.cmdFPSewaPKP.Name = "cmdFPSewaPKP"
        Me.cmdFPSewaPKP.Size = New System.Drawing.Size(190, 31)
        Me.cmdFPSewaPKP.TabIndex = 4
        Me.cmdFPSewaPKP.Text = "Penj. Promosi PKP"
        '
        'cmdPPNBulanan
        '
        Me.cmdPPNBulanan.Appearance.Font = New System.Drawing.Font("Tahoma", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdPPNBulanan.Appearance.ForeColor = System.Drawing.Color.Blue
        Me.cmdPPNBulanan.Appearance.Options.UseFont = True
        Me.cmdPPNBulanan.Appearance.Options.UseForeColor = True
        Me.cmdPPNBulanan.ImageIndex = 0
        Me.cmdPPNBulanan.Location = New System.Drawing.Point(6, 261)
        Me.cmdPPNBulanan.Name = "cmdPPNBulanan"
        Me.cmdPPNBulanan.Size = New System.Drawing.Size(190, 31)
        Me.cmdPPNBulanan.TabIndex = 8
        Me.cmdPPNBulanan.Text = "Rekap PPN Bulanan"
        '
        'cmdFPObatPKP
        '
        Me.cmdFPObatPKP.Appearance.Font = New System.Drawing.Font("Tahoma", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdFPObatPKP.Appearance.ForeColor = System.Drawing.Color.Blue
        Me.cmdFPObatPKP.Appearance.Options.UseFont = True
        Me.cmdFPObatPKP.Appearance.Options.UseForeColor = True
        Me.cmdFPObatPKP.ImageIndex = 0
        Me.cmdFPObatPKP.Location = New System.Drawing.Point(6, 187)
        Me.cmdFPObatPKP.Name = "cmdFPObatPKP"
        Me.cmdFPObatPKP.Size = New System.Drawing.Size(190, 31)
        Me.cmdFPObatPKP.TabIndex = 6
        Me.cmdFPObatPKP.Text = "Penj. Obat PKP"
        '
        'cmdFPObatBulanan
        '
        Me.cmdFPObatBulanan.Appearance.Font = New System.Drawing.Font("Tahoma", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdFPObatBulanan.Appearance.ForeColor = System.Drawing.Color.Blue
        Me.cmdFPObatBulanan.Appearance.Options.UseFont = True
        Me.cmdFPObatBulanan.Appearance.Options.UseForeColor = True
        Me.cmdFPObatBulanan.ImageIndex = 0
        Me.cmdFPObatBulanan.Location = New System.Drawing.Point(6, 335)
        Me.cmdFPObatBulanan.Name = "cmdFPObatBulanan"
        Me.cmdFPObatBulanan.Size = New System.Drawing.Size(190, 31)
        Me.cmdFPObatBulanan.TabIndex = 10
        Me.cmdFPObatBulanan.Text = "Lap. Obat Bulanan"
        '
        'cmdTutup
        '
        Me.cmdTutup.Appearance.Font = New System.Drawing.Font("Tahoma", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdTutup.Appearance.ForeColor = System.Drawing.Color.Blue
        Me.cmdTutup.Appearance.Options.UseFont = True
        Me.cmdTutup.Appearance.Options.UseForeColor = True
        Me.cmdTutup.ImageIndex = 0
        Me.cmdTutup.Location = New System.Drawing.Point(6, 409)
        Me.cmdTutup.Name = "cmdTutup"
        Me.cmdTutup.Size = New System.Drawing.Size(190, 31)
        Me.cmdTutup.TabIndex = 12
        Me.cmdTutup.Text = "Tutup"
        '
        'PanelControl1
        '
        Me.PanelControl1.Controls.Add(Me.cmdFPObatNPKP)
        Me.PanelControl1.Controls.Add(Me.cmdFPSewaNPKP)
        Me.PanelControl1.Controls.Add(Me.cmdHitungUlang)
        Me.PanelControl1.Controls.Add(Me.cmdRefresh)
        Me.PanelControl1.Controls.Add(Me.ckEdit)
        Me.PanelControl1.Controls.Add(Me.cmdFPObatHarian)
        Me.PanelControl1.Controls.Add(Me.cmdTutup)
        Me.PanelControl1.Controls.Add(Me.txtPeriode)
        Me.PanelControl1.Controls.Add(Me.cmdFPObatBulanan)
        Me.PanelControl1.Controls.Add(Me.LabelControl1)
        Me.PanelControl1.Controls.Add(Me.cmdPPNBulanan)
        Me.PanelControl1.Controls.Add(Me.cmdOmzetBulanan)
        Me.PanelControl1.Controls.Add(Me.cmdFPObatPKP)
        Me.PanelControl1.Controls.Add(Me.cmdFPSewaPKP)
        Me.PanelControl1.Dock = System.Windows.Forms.DockStyle.Left
        Me.PanelControl1.Location = New System.Drawing.Point(0, 52)
        Me.PanelControl1.Name = "PanelControl1"
        Me.PanelControl1.Size = New System.Drawing.Size(201, 624)
        Me.PanelControl1.TabIndex = 1
        '
        'cmdHitungUlang
        '
        Me.cmdHitungUlang.Appearance.Font = New System.Drawing.Font("Tahoma", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdHitungUlang.Appearance.ForeColor = System.Drawing.Color.Blue
        Me.cmdHitungUlang.Appearance.Options.UseFont = True
        Me.cmdHitungUlang.Appearance.Options.UseForeColor = True
        Me.cmdHitungUlang.ImageIndex = 0
        Me.cmdHitungUlang.Location = New System.Drawing.Point(6, 39)
        Me.cmdHitungUlang.Name = "cmdHitungUlang"
        Me.cmdHitungUlang.Size = New System.Drawing.Size(190, 31)
        Me.cmdHitungUlang.TabIndex = 2
        Me.cmdHitungUlang.Text = "Hitung Z Report"
        '
        'cmdRefresh
        '
        Me.cmdRefresh.Appearance.Font = New System.Drawing.Font("Tahoma", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdRefresh.Appearance.ForeColor = System.Drawing.Color.Blue
        Me.cmdRefresh.Appearance.Options.UseFont = True
        Me.cmdRefresh.Appearance.Options.UseForeColor = True
        Me.cmdRefresh.ImageIndex = 0
        Me.cmdRefresh.Location = New System.Drawing.Point(6, 372)
        Me.cmdRefresh.Name = "cmdRefresh"
        Me.cmdRefresh.Size = New System.Drawing.Size(190, 31)
        Me.cmdRefresh.TabIndex = 11
        Me.cmdRefresh.Text = "&Refresh Data"
        '
        'ckEdit
        '
        Me.ckEdit.Location = New System.Drawing.Point(4, 446)
        Me.ckEdit.Name = "ckEdit"
        Me.ckEdit.Properties.Appearance.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ckEdit.Properties.Appearance.Options.UseFont = True
        Me.ckEdit.Properties.Caption = "Tampilkan Editor"
        Me.ckEdit.Size = New System.Drawing.Size(173, 21)
        Me.ckEdit.TabIndex = 13
        '
        'cmdFPObatHarian
        '
        Me.cmdFPObatHarian.Appearance.Font = New System.Drawing.Font("Tahoma", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdFPObatHarian.Appearance.ForeColor = System.Drawing.Color.Blue
        Me.cmdFPObatHarian.Appearance.Options.UseFont = True
        Me.cmdFPObatHarian.Appearance.Options.UseForeColor = True
        Me.cmdFPObatHarian.ImageIndex = 0
        Me.cmdFPObatHarian.Location = New System.Drawing.Point(6, 298)
        Me.cmdFPObatHarian.Name = "cmdFPObatHarian"
        Me.cmdFPObatHarian.Size = New System.Drawing.Size(190, 31)
        Me.cmdFPObatHarian.TabIndex = 9
        Me.cmdFPObatHarian.Text = "Lap. Obat Harian"
        '
        'CrViewer
        '
        Me.CrViewer.ActiveViewIndex = -1
        Me.CrViewer.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.CrViewer.DisplayGroupTree = False
        Me.CrViewer.Dock = System.Windows.Forms.DockStyle.Fill
        Me.CrViewer.Location = New System.Drawing.Point(201, 52)
        Me.CrViewer.Name = "CrViewer"
        Me.CrViewer.SelectionFormula = ""
        Me.CrViewer.ShowCloseButton = False
        Me.CrViewer.Size = New System.Drawing.Size(874, 624)
        Me.CrViewer.TabIndex = 2
        Me.CrViewer.ViewTimeSelectionFormula = ""
        '
        'PanelControl2
        '
        Me.PanelControl2.Controls.Add(Me.ProgressBarControl1)
        Me.PanelControl2.Controls.Add(Me.lbDaftar)
        Me.PanelControl2.Dock = System.Windows.Forms.DockStyle.Top
        Me.PanelControl2.Location = New System.Drawing.Point(0, 0)
        Me.PanelControl2.Name = "PanelControl2"
        Me.PanelControl2.Size = New System.Drawing.Size(1075, 52)
        Me.PanelControl2.TabIndex = 0
        '
        'ProgressBarControl1
        '
        Me.ProgressBarControl1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ProgressBarControl1.Location = New System.Drawing.Point(6, 35)
        Me.ProgressBarControl1.Name = "ProgressBarControl1"
        Me.ProgressBarControl1.Size = New System.Drawing.Size(1064, 12)
        Me.ProgressBarControl1.TabIndex = 1
        Me.ProgressBarControl1.Visible = False
        '
        'GC1
        '
        Me.GC1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.GC1.Location = New System.Drawing.Point(201, 52)
        Me.GC1.MainView = Me.GV1
        Me.GC1.Name = "GC1"
        Me.GC1.Size = New System.Drawing.Size(874, 624)
        Me.GC1.TabIndex = 3
        Me.GC1.ViewCollection.AddRange(New DevExpress.XtraGrid.Views.Base.BaseView() {Me.GV1})
        Me.GC1.Visible = False
        '
        'GV1
        '
        Me.GV1.GridControl = Me.GC1
        Me.GV1.Name = "GV1"
        Me.GV1.OptionsBehavior.AllowIncrementalSearch = True
        Me.GV1.OptionsBehavior.Editable = False
        Me.GV1.OptionsCustomization.AllowSort = False
        Me.GV1.OptionsSelection.MultiSelect = True
        Me.GV1.OptionsView.ColumnAutoWidth = False
        Me.GV1.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never
        Me.GV1.OptionsView.ShowFooter = True
        Me.GV1.OptionsView.ShowGroupPanel = False
        '
        'cmdFPSewaNPKP
        '
        Me.cmdFPSewaNPKP.Appearance.Font = New System.Drawing.Font("Tahoma", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdFPSewaNPKP.Appearance.ForeColor = System.Drawing.Color.Blue
        Me.cmdFPSewaNPKP.Appearance.Options.UseFont = True
        Me.cmdFPSewaNPKP.Appearance.Options.UseForeColor = True
        Me.cmdFPSewaNPKP.ImageIndex = 0
        Me.cmdFPSewaNPKP.Location = New System.Drawing.Point(6, 150)
        Me.cmdFPSewaNPKP.Name = "cmdFPSewaNPKP"
        Me.cmdFPSewaNPKP.Size = New System.Drawing.Size(190, 31)
        Me.cmdFPSewaNPKP.TabIndex = 5
        Me.cmdFPSewaNPKP.Text = "Penj. Promosi NPKP"
        '
        'cmdFPObatNPKP
        '
        Me.cmdFPObatNPKP.Appearance.Font = New System.Drawing.Font("Tahoma", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdFPObatNPKP.Appearance.ForeColor = System.Drawing.Color.Blue
        Me.cmdFPObatNPKP.Appearance.Options.UseFont = True
        Me.cmdFPObatNPKP.Appearance.Options.UseForeColor = True
        Me.cmdFPObatNPKP.ImageIndex = 0
        Me.cmdFPObatNPKP.Location = New System.Drawing.Point(6, 224)
        Me.cmdFPObatNPKP.Name = "cmdFPObatNPKP"
        Me.cmdFPObatNPKP.Size = New System.Drawing.Size(190, 31)
        Me.cmdFPObatNPKP.TabIndex = 7
        Me.cmdFPObatNPKP.Text = "Penj. Obat NPKP"
        '
        'frmLaporanOmzetBonus
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1075, 676)
        Me.Controls.Add(Me.CrViewer)
        Me.Controls.Add(Me.GC1)
        Me.Controls.Add(Me.PanelControl1)
        Me.Controls.Add(Me.PanelControl2)
        Me.Name = "frmLaporanOmzetBonus"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Laporan Omzet Bulanan"
        CType(Me.txtPeriode.Properties.VistaTimeProperties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtPeriode.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PanelControl1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.PanelControl1.ResumeLayout(False)
        Me.PanelControl1.PerformLayout()
        CType(Me.ckEdit.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PanelControl2, System.ComponentModel.ISupportInitialize).EndInit()
        Me.PanelControl2.ResumeLayout(False)
        Me.PanelControl2.PerformLayout()
        CType(Me.ProgressBarControl1.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.GC1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.GV1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents LabelControl1 As DevExpress.XtraEditors.LabelControl
    Friend WithEvents txtPeriode As DevExpress.XtraEditors.DateEdit
    Friend WithEvents lbDaftar As DevExpress.XtraEditors.LabelControl
    Friend WithEvents cmdOmzetBulanan As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents cmdFPSewaPKP As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents cmdPPNBulanan As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents cmdFPObatPKP As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents cmdFPObatBulanan As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents cmdTutup As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents PanelControl1 As DevExpress.XtraEditors.PanelControl
    Friend WithEvents CrViewer As CrystalDecisions.Windows.Forms.CrystalReportViewer
    Friend WithEvents cmdFPObatHarian As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents PanelControl2 As DevExpress.XtraEditors.PanelControl
    Friend WithEvents ckEdit As DevExpress.XtraEditors.CheckEdit
    Friend WithEvents GC1 As DevExpress.XtraGrid.GridControl
    Friend WithEvents GV1 As DevExpress.XtraGrid.Views.Grid.GridView
    Friend WithEvents cmdRefresh As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents cmdHitungUlang As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents ProgressBarControl1 As DevExpress.XtraEditors.ProgressBarControl
    Friend WithEvents cmdFPSewaNPKP As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents cmdFPObatNPKP As DevExpress.XtraEditors.SimpleButton
End Class
