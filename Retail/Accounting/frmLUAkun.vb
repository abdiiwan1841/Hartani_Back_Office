Imports DevExpress.XtraGrid.Views.Base

Public Class frmLUAkun
    Inherits DevExpress.XtraEditors.XtraForm

#Region " Windows Form Designer generated code "

    Public Sub New()
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()

        'Add any initialization after the InitializeComponent() : setme call

    End Sub

    'Form overrides dispose to clean up the component list.
    Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing Then
            If Not (components Is Nothing) Then
                components.Dispose()
            End If
        End If
        MyBase.Dispose(disposing)
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    Friend WithEvents PanelControl1 As DevExpress.XtraEditors.PanelControl
    Friend WithEvents PanelControl2 As DevExpress.XtraEditors.PanelControl
    Friend WithEvents cmClose As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents cmdBaru As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents cmdRefresh As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents cmdEdit As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents cmdHapus As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents cmdOK As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents cmdCancel As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents PanelControl3 As DevExpress.XtraEditors.PanelControl
    Friend WithEvents SimpleButton1 As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents GridControl1 As DevExpress.XtraGrid.GridControl
    Friend WithEvents GridView1 As DevExpress.XtraGrid.Views.Grid.GridView
    Friend WithEvents colNoID As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colKode As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colNama As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colSubKlasifikasi As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colIsActive As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents RepositoryItemCheckEdit1 As DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit
    Friend WithEvents CIDSubKlas As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents cKlasifikasi As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents cIDKlas As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents cAktif As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents cIsDebet As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents cmdExport As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents cmdPreview As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents LayoutControl1 As DevExpress.XtraLayout.LayoutControl
    Friend WithEvents LayoutControlGroup1 As DevExpress.XtraLayout.LayoutControlGroup
    Friend WithEvents PanelControl4 As DevExpress.XtraEditors.PanelControl
    Friend WithEvents LayoutControlItem1 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents LayoutControlItem3 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents Label2 As System.Windows.Forms.Label
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.PanelControl1 = New DevExpress.XtraEditors.PanelControl
        Me.Label2 = New System.Windows.Forms.Label
        Me.PanelControl2 = New DevExpress.XtraEditors.PanelControl
        Me.cmdExport = New DevExpress.XtraEditors.SimpleButton
        Me.cmdPreview = New DevExpress.XtraEditors.SimpleButton
        Me.cmClose = New DevExpress.XtraEditors.SimpleButton
        Me.cmdBaru = New DevExpress.XtraEditors.SimpleButton
        Me.cmdRefresh = New DevExpress.XtraEditors.SimpleButton
        Me.cmdEdit = New DevExpress.XtraEditors.SimpleButton
        Me.cmdHapus = New DevExpress.XtraEditors.SimpleButton
        Me.PanelControl3 = New DevExpress.XtraEditors.PanelControl
        Me.SimpleButton1 = New DevExpress.XtraEditors.SimpleButton
        Me.cmdCancel = New DevExpress.XtraEditors.SimpleButton
        Me.cmdOK = New DevExpress.XtraEditors.SimpleButton
        Me.GridControl1 = New DevExpress.XtraGrid.GridControl
        Me.GridView1 = New DevExpress.XtraGrid.Views.Grid.GridView
        Me.colNoID = New DevExpress.XtraGrid.Columns.GridColumn
        Me.colKode = New DevExpress.XtraGrid.Columns.GridColumn
        Me.colNama = New DevExpress.XtraGrid.Columns.GridColumn
        Me.colSubKlasifikasi = New DevExpress.XtraGrid.Columns.GridColumn
        Me.colIsActive = New DevExpress.XtraGrid.Columns.GridColumn
        Me.RepositoryItemCheckEdit1 = New DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit
        Me.CIDSubKlas = New DevExpress.XtraGrid.Columns.GridColumn
        Me.cKlasifikasi = New DevExpress.XtraGrid.Columns.GridColumn
        Me.cIDKlas = New DevExpress.XtraGrid.Columns.GridColumn
        Me.cAktif = New DevExpress.XtraGrid.Columns.GridColumn
        Me.cIsDebet = New DevExpress.XtraGrid.Columns.GridColumn
        Me.LayoutControl1 = New DevExpress.XtraLayout.LayoutControl
        Me.PanelControl4 = New DevExpress.XtraEditors.PanelControl
        Me.LayoutControlGroup1 = New DevExpress.XtraLayout.LayoutControlGroup
        Me.LayoutControlItem1 = New DevExpress.XtraLayout.LayoutControlItem
        Me.LayoutControlItem3 = New DevExpress.XtraLayout.LayoutControlItem
        CType(Me.PanelControl1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.PanelControl1.SuspendLayout()
        CType(Me.PanelControl2, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.PanelControl2.SuspendLayout()
        CType(Me.PanelControl3, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.PanelControl3.SuspendLayout()
        CType(Me.GridControl1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.GridView1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RepositoryItemCheckEdit1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControl1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.LayoutControl1.SuspendLayout()
        CType(Me.PanelControl4, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.PanelControl4.SuspendLayout()
        CType(Me.LayoutControlGroup1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem3, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'PanelControl1
        '
        Me.PanelControl1.Appearance.BackColor = System.Drawing.Color.Transparent
        Me.PanelControl1.Appearance.Options.UseBackColor = True
        Me.PanelControl1.ContentImageAlignment = System.Drawing.ContentAlignment.MiddleLeft
        Me.PanelControl1.Controls.Add(Me.Label2)
        Me.PanelControl1.Location = New System.Drawing.Point(12, 12)
        Me.PanelControl1.LookAndFeel.SkinName = "Glass Oceans"
        Me.PanelControl1.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.UltraFlat
        Me.PanelControl1.Name = "PanelControl1"
        Me.PanelControl1.Size = New System.Drawing.Size(669, 39)
        Me.PanelControl1.TabIndex = 4
        '
        'Label2
        '
        Me.Label2.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label2.BackColor = System.Drawing.Color.Transparent
        Me.Label2.Font = New System.Drawing.Font("Rockwell", 15.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.ForeColor = System.Drawing.SystemColors.HotTrack
        Me.Label2.ImageAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.Label2.Location = New System.Drawing.Point(358, 9)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(306, 23)
        Me.Label2.TabIndex = 1
        Me.Label2.Text = "DAFTAR AKUN"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'PanelControl2
        '
        Me.PanelControl2.Appearance.BackColor = System.Drawing.SystemColors.ButtonFace
        Me.PanelControl2.Appearance.Options.UseBackColor = True
        Me.PanelControl2.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder
        Me.PanelControl2.Controls.Add(Me.cmdExport)
        Me.PanelControl2.Controls.Add(Me.cmdPreview)
        Me.PanelControl2.Controls.Add(Me.cmClose)
        Me.PanelControl2.Controls.Add(Me.cmdBaru)
        Me.PanelControl2.Controls.Add(Me.cmdRefresh)
        Me.PanelControl2.Controls.Add(Me.cmdEdit)
        Me.PanelControl2.Controls.Add(Me.cmdHapus)
        Me.PanelControl2.Dock = System.Windows.Forms.DockStyle.Left
        Me.PanelControl2.Location = New System.Drawing.Point(2, 2)
        Me.PanelControl2.LookAndFeel.SkinName = "Glass Oceans"
        Me.PanelControl2.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.UltraFlat
        Me.PanelControl2.Name = "PanelControl2"
        Me.PanelControl2.Size = New System.Drawing.Size(100, 474)
        Me.PanelControl2.TabIndex = 5
        '
        'cmdExport
        '
        Me.cmdExport.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdExport.Cursor = System.Windows.Forms.Cursors.Hand
        Me.cmdExport.ImageIndex = 13
        Me.cmdExport.Location = New System.Drawing.Point(5, 202)
        Me.cmdExport.Name = "cmdExport"
        Me.cmdExport.Size = New System.Drawing.Size(92, 34)
        Me.cmdExport.TabIndex = 8
        Me.cmdExport.Text = "&Export"
        '
        'cmdPreview
        '
        Me.cmdPreview.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdPreview.Cursor = System.Windows.Forms.Cursors.Hand
        Me.cmdPreview.ImageIndex = 13
        Me.cmdPreview.Location = New System.Drawing.Point(5, 162)
        Me.cmdPreview.Name = "cmdPreview"
        Me.cmdPreview.Size = New System.Drawing.Size(92, 34)
        Me.cmdPreview.TabIndex = 7
        Me.cmdPreview.Text = "&Preview"
        '
        'cmClose
        '
        Me.cmClose.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmClose.Cursor = System.Windows.Forms.Cursors.Hand
        Me.cmClose.ImageIndex = 1
        Me.cmClose.Location = New System.Drawing.Point(5, 430)
        Me.cmClose.Name = "cmClose"
        Me.cmClose.Size = New System.Drawing.Size(92, 34)
        Me.cmClose.TabIndex = 6
        Me.cmClose.Text = "&Close"
        '
        'cmdBaru
        '
        Me.cmdBaru.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdBaru.Cursor = System.Windows.Forms.Cursors.Hand
        Me.cmdBaru.ImageIndex = 6
        Me.cmdBaru.Location = New System.Drawing.Point(5, 44)
        Me.cmdBaru.Name = "cmdBaru"
        Me.cmdBaru.Size = New System.Drawing.Size(92, 34)
        Me.cmdBaru.TabIndex = 0
        Me.cmdBaru.Text = "&New"
        '
        'cmdRefresh
        '
        Me.cmdRefresh.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdRefresh.Cursor = System.Windows.Forms.Cursors.Hand
        Me.cmdRefresh.ImageIndex = 13
        Me.cmdRefresh.Location = New System.Drawing.Point(5, 4)
        Me.cmdRefresh.Name = "cmdRefresh"
        Me.cmdRefresh.Size = New System.Drawing.Size(92, 34)
        Me.cmdRefresh.TabIndex = 5
        Me.cmdRefresh.Text = "&Refresh"
        '
        'cmdEdit
        '
        Me.cmdEdit.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdEdit.Cursor = System.Windows.Forms.Cursors.Hand
        Me.cmdEdit.ImageIndex = 5
        Me.cmdEdit.Location = New System.Drawing.Point(5, 82)
        Me.cmdEdit.Name = "cmdEdit"
        Me.cmdEdit.Size = New System.Drawing.Size(92, 34)
        Me.cmdEdit.TabIndex = 1
        Me.cmdEdit.Text = "&Edit"
        '
        'cmdHapus
        '
        Me.cmdHapus.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdHapus.Cursor = System.Windows.Forms.Cursors.Hand
        Me.cmdHapus.ImageIndex = 7
        Me.cmdHapus.Location = New System.Drawing.Point(5, 122)
        Me.cmdHapus.Name = "cmdHapus"
        Me.cmdHapus.Size = New System.Drawing.Size(92, 34)
        Me.cmdHapus.TabIndex = 4
        Me.cmdHapus.Text = "&Delete"
        '
        'PanelControl3
        '
        Me.PanelControl3.Appearance.BackColor = System.Drawing.SystemColors.Control
        Me.PanelControl3.Appearance.Options.UseBackColor = True
        Me.PanelControl3.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder
        Me.PanelControl3.Controls.Add(Me.SimpleButton1)
        Me.PanelControl3.Controls.Add(Me.cmdCancel)
        Me.PanelControl3.Controls.Add(Me.cmdOK)
        Me.PanelControl3.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.PanelControl3.Location = New System.Drawing.Point(102, 422)
        Me.PanelControl3.Name = "PanelControl3"
        Me.PanelControl3.Size = New System.Drawing.Size(565, 54)
        Me.PanelControl3.TabIndex = 6
        '
        'SimpleButton1
        '
        Me.SimpleButton1.Cursor = System.Windows.Forms.Cursors.Hand
        Me.SimpleButton1.ImageIndex = 13
        Me.SimpleButton1.Location = New System.Drawing.Point(6, 8)
        Me.SimpleButton1.Name = "SimpleButton1"
        Me.SimpleButton1.Size = New System.Drawing.Size(85, 34)
        Me.SimpleButton1.TabIndex = 6
        Me.SimpleButton1.Text = "&Refresh"
        Me.SimpleButton1.Visible = False
        '
        'cmdCancel
        '
        Me.cmdCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdCancel.Appearance.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdCancel.Appearance.Options.UseFont = True
        Me.cmdCancel.Cursor = System.Windows.Forms.Cursors.Hand
        Me.cmdCancel.ImageIndex = 10
        Me.cmdCancel.Location = New System.Drawing.Point(460, 8)
        Me.cmdCancel.Name = "cmdCancel"
        Me.cmdCancel.Size = New System.Drawing.Size(80, 34)
        Me.cmdCancel.TabIndex = 3
        Me.cmdCancel.Text = "&Cancel"
        '
        'cmdOK
        '
        Me.cmdOK.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdOK.Appearance.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdOK.Appearance.Options.UseFont = True
        Me.cmdOK.Cursor = System.Windows.Forms.Cursors.Hand
        Me.cmdOK.ImageIndex = 4
        Me.cmdOK.Location = New System.Drawing.Point(374, 8)
        Me.cmdOK.Name = "cmdOK"
        Me.cmdOK.Size = New System.Drawing.Size(80, 34)
        Me.cmdOK.TabIndex = 2
        Me.cmdOK.Text = "&Ok"
        '
        'GridControl1
        '
        Me.GridControl1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.GridControl1.Location = New System.Drawing.Point(102, 2)
        Me.GridControl1.MainView = Me.GridView1
        Me.GridControl1.Name = "GridControl1"
        Me.GridControl1.RepositoryItems.AddRange(New DevExpress.XtraEditors.Repository.RepositoryItem() {Me.RepositoryItemCheckEdit1})
        Me.GridControl1.Size = New System.Drawing.Size(565, 420)
        Me.GridControl1.TabIndex = 0
        Me.GridControl1.ViewCollection.AddRange(New DevExpress.XtraGrid.Views.Base.BaseView() {Me.GridView1})
        '
        'GridView1
        '
        Me.GridView1.Columns.AddRange(New DevExpress.XtraGrid.Columns.GridColumn() {Me.colNoID, Me.colKode, Me.colNama, Me.colSubKlasifikasi, Me.colIsActive, Me.CIDSubKlas, Me.cKlasifikasi, Me.cIDKlas, Me.cAktif, Me.cIsDebet})
        Me.GridView1.CustomizationFormBounds = New System.Drawing.Rectangle(808, 463, 216, 178)
        Me.GridView1.GridControl = Me.GridControl1
        Me.GridView1.Name = "GridView1"
        Me.GridView1.OptionsBehavior.AllowIncrementalSearch = True
        Me.GridView1.OptionsBehavior.Editable = False
        Me.GridView1.OptionsView.ColumnAutoWidth = False
        Me.GridView1.OptionsView.ShowFooter = True
        '
        'colNoID
        '
        Me.colNoID.Caption = "No."
        Me.colNoID.FieldName = "ID"
        Me.colNoID.GroupFormat.FormatString = "###,##0"
        Me.colNoID.GroupFormat.FormatType = DevExpress.Utils.FormatType.Numeric
        Me.colNoID.Name = "colNoID"
        Me.colNoID.Width = 46
        '
        'colKode
        '
        Me.colKode.Caption = "Kode"
        Me.colKode.FieldName = "Kode"
        Me.colKode.Name = "colKode"
        Me.colKode.Visible = True
        Me.colKode.VisibleIndex = 0
        Me.colKode.Width = 91
        '
        'colNama
        '
        Me.colNama.Caption = "Nama"
        Me.colNama.FieldName = "Nama"
        Me.colNama.Name = "colNama"
        Me.colNama.Visible = True
        Me.colNama.VisibleIndex = 1
        Me.colNama.Width = 208
        '
        'colSubKlasifikasi
        '
        Me.colSubKlasifikasi.Caption = "Sub Klasifikasi"
        Me.colSubKlasifikasi.FieldName = "SubKlasifikasi"
        Me.colSubKlasifikasi.Name = "colSubKlasifikasi"
        Me.colSubKlasifikasi.Visible = True
        Me.colSubKlasifikasi.VisibleIndex = 2
        Me.colSubKlasifikasi.Width = 205
        '
        'colIsActive
        '
        Me.colIsActive.Caption = "Aktif"
        Me.colIsActive.ColumnEdit = Me.RepositoryItemCheckEdit1
        Me.colIsActive.FieldName = "IsActive"
        Me.colIsActive.Name = "colIsActive"
        '
        'RepositoryItemCheckEdit1
        '
        Me.RepositoryItemCheckEdit1.AutoHeight = False
        Me.RepositoryItemCheckEdit1.Name = "RepositoryItemCheckEdit1"
        Me.RepositoryItemCheckEdit1.NullStyle = DevExpress.XtraEditors.Controls.StyleIndeterminate.Inactive
        '
        'CIDSubKlas
        '
        Me.CIDSubKlas.Caption = "IDSubKlas"
        Me.CIDSubKlas.FieldName = "IDSubKlas"
        Me.CIDSubKlas.Name = "CIDSubKlas"
        '
        'cKlasifikasi
        '
        Me.cKlasifikasi.Caption = "Klasifikasi"
        Me.cKlasifikasi.FieldName = "Klasifikasi"
        Me.cKlasifikasi.Name = "cKlasifikasi"
        Me.cKlasifikasi.Visible = True
        Me.cKlasifikasi.VisibleIndex = 3
        '
        'cIDKlas
        '
        Me.cIDKlas.Caption = "IDKlas"
        Me.cIDKlas.FieldName = "IDKlas"
        Me.cIDKlas.Name = "cIDKlas"
        '
        'cAktif
        '
        Me.cAktif.Caption = "IsAktif"
        Me.cAktif.ColumnEdit = Me.RepositoryItemCheckEdit1
        Me.cAktif.FieldName = "IsAktif"
        Me.cAktif.Name = "cAktif"
        Me.cAktif.Visible = True
        Me.cAktif.VisibleIndex = 4
        '
        'cIsDebet
        '
        Me.cIsDebet.Caption = "Normal Debet"
        Me.cIsDebet.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
        Me.cIsDebet.FieldName = "IsDebet"
        Me.cIsDebet.Name = "cIsDebet"
        Me.cIsDebet.Visible = True
        Me.cIsDebet.VisibleIndex = 5
        '
        'LayoutControl1
        '
        Me.LayoutControl1.Controls.Add(Me.PanelControl4)
        Me.LayoutControl1.Controls.Add(Me.PanelControl1)
        Me.LayoutControl1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.LayoutControl1.Location = New System.Drawing.Point(0, 0)
        Me.LayoutControl1.Name = "LayoutControl1"
        Me.LayoutControl1.Root = Me.LayoutControlGroup1
        Me.LayoutControl1.Size = New System.Drawing.Size(693, 545)
        Me.LayoutControl1.TabIndex = 7
        Me.LayoutControl1.Text = "LayoutControl1"
        '
        'PanelControl4
        '
        Me.PanelControl4.Controls.Add(Me.GridControl1)
        Me.PanelControl4.Controls.Add(Me.PanelControl3)
        Me.PanelControl4.Controls.Add(Me.PanelControl2)
        Me.PanelControl4.Location = New System.Drawing.Point(12, 55)
        Me.PanelControl4.Name = "PanelControl4"
        Me.PanelControl4.Size = New System.Drawing.Size(669, 478)
        Me.PanelControl4.TabIndex = 8
        '
        'LayoutControlGroup1
        '
        Me.LayoutControlGroup1.CustomizationFormText = "LayoutControlGroup1"
        Me.LayoutControlGroup1.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.[True]
        Me.LayoutControlGroup1.GroupBordersVisible = False
        Me.LayoutControlGroup1.Items.AddRange(New DevExpress.XtraLayout.BaseLayoutItem() {Me.LayoutControlItem1, Me.LayoutControlItem3})
        Me.LayoutControlGroup1.Location = New System.Drawing.Point(0, 0)
        Me.LayoutControlGroup1.Name = "LayoutControlGroup1"
        Me.LayoutControlGroup1.Size = New System.Drawing.Size(693, 545)
        Me.LayoutControlGroup1.Text = "LayoutControlGroup1"
        Me.LayoutControlGroup1.TextVisible = False
        '
        'LayoutControlItem1
        '
        Me.LayoutControlItem1.Control = Me.PanelControl1
        Me.LayoutControlItem1.CustomizationFormText = "LayoutControlItem1"
        Me.LayoutControlItem1.Location = New System.Drawing.Point(0, 0)
        Me.LayoutControlItem1.MaxSize = New System.Drawing.Size(0, 43)
        Me.LayoutControlItem1.MinSize = New System.Drawing.Size(104, 43)
        Me.LayoutControlItem1.Name = "LayoutControlItem1"
        Me.LayoutControlItem1.Size = New System.Drawing.Size(673, 43)
        Me.LayoutControlItem1.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom
        Me.LayoutControlItem1.Text = "LayoutControlItem1"
        Me.LayoutControlItem1.TextSize = New System.Drawing.Size(0, 0)
        Me.LayoutControlItem1.TextToControlDistance = 0
        Me.LayoutControlItem1.TextVisible = False
        '
        'LayoutControlItem3
        '
        Me.LayoutControlItem3.Control = Me.PanelControl4
        Me.LayoutControlItem3.CustomizationFormText = "LayoutControlItem3"
        Me.LayoutControlItem3.Location = New System.Drawing.Point(0, 43)
        Me.LayoutControlItem3.Name = "LayoutControlItem3"
        Me.LayoutControlItem3.Size = New System.Drawing.Size(673, 482)
        Me.LayoutControlItem3.Text = "LayoutControlItem3"
        Me.LayoutControlItem3.TextSize = New System.Drawing.Size(0, 0)
        Me.LayoutControlItem3.TextToControlDistance = 0
        Me.LayoutControlItem3.TextVisible = False
        '
        'frmLUAkun
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 14)
        Me.ClientSize = New System.Drawing.Size(693, 545)
        Me.Controls.Add(Me.LayoutControl1)
        Me.Name = "frmLUAkun"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Daftar Akun"
        CType(Me.PanelControl1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.PanelControl1.ResumeLayout(False)
        CType(Me.PanelControl2, System.ComponentModel.ISupportInitialize).EndInit()
        Me.PanelControl2.ResumeLayout(False)
        CType(Me.PanelControl3, System.ComponentModel.ISupportInitialize).EndInit()
        Me.PanelControl3.ResumeLayout(False)
        CType(Me.GridControl1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.GridView1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RepositoryItemCheckEdit1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControl1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.LayoutControl1.ResumeLayout(False)
        CType(Me.PanelControl4, System.ComponentModel.ISupportInitialize).EndInit()
        Me.PanelControl4.ResumeLayout(False)
        CType(Me.LayoutControlGroup1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem3, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

#End Region
    Public NoID As Long
    Public Kode As String
    Public Nama As String
    Public SubKlas As String
    Dim BolehAmbilData As Boolean
    Public IsDebet As Boolean = False
    Public IDDepartemen As Long = -1
    Public row As System.Data.DataRow
    Public pStatus As mdlAccPublik.Ptipe
    Public Sub TampilData()
        Dim strsql As String
        'Dim isAda As Boolean
        Dim curentcursor As Cursor = Windows.Forms.Cursor.Current
        Windows.Forms.Cursor.Current = Cursors.WaitCursor
        strsql = "SELECT MAkun.ID,Makun.IDMataUang, MMataUang.Nama AS NamaMataUang, MAkun.Kode,MAkun.Nama,MSubklasAkun.Nama as SubKlasifikasi,MKlasAkun.Nama as Klasifikasi,MKlasAkun.IsDebet " & _
                 " FROM ((MAkun LEFT JOIN MSubKlasAkun On MAkun.IDSubklasAkun=MSubKlasAkun.ID)" & _
                 " LEFT JOIN MKlasAkun On MSubKlasAkun.IDKlasAkun=MKlasAkun.ID)" & _
                 " LEFT JOIN MMataUang On MMataUang.ID=MAkun.IDMataUang"
        'If txtDepartemen.Text <> "" Then
        '    strsql &= " WHERE MAkun.IDDepartemen=" & IDDepartemen
        'End If
        Try
            ExecuteDBGrid(GridControl1, strsql)
        Catch ex As Exception
            MsgBox(ex.Message, MessageBoxButtons.OK & MessageBoxIcon.Error, "Pesan Kesalahan ::.")
        End Try
        Windows.Forms.Cursor.Current = curentcursor
        Application.DoEvents()
        GridView1.FocusedRowHandle = GridView1.LocateByValue(0, colNoID, NoID)
    End Sub
    Sub AmbilData()
        Try
            Dim view As ColumnView = GridControl1.FocusedView
            row = view.GetDataRow(GridView1.FocusedRowHandle)
            NoID = NullTolong(row("ID"))
            Kode = NullTostr(row("Kode"))
            Nama = NullTostr(row("Nama"))
            IsDebet = NullTobool(row("IsDebet"))
            SubKlas = NullTostr(row("SubKlasifikasi"))
            DialogResult = Windows.Forms.DialogResult.OK
            Close()
        Catch ex As Exception

        End Try
    End Sub

    Private Sub cmdCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdCancel.Click
        DialogResult = Windows.Forms.DialogResult.Cancel
        Close()
    End Sub

    Private Sub cmdOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdOK.Click
        AmbilData()
    End Sub

    Private Sub cmdEdit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdEdit.Click
        Dim frmEntri As New frmEntriAkun
        Dim view As ColumnView = GridControl1.FocusedView
        Try
            Dim row As System.Data.DataRow = view.GetDataRow(GridView1.FocusedRowHandle)
            Dim NoID As Long = row("ID")
            frmEntri.pStatus = mdlAccPublik.ptipe.Edit
            frmEntri.IDAkun = NoID
            If frmEntri.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
                TampilData()
                GridView1.ClearSelection()
                GridView1.FocusedRowHandle = GridView1.LocateByValue("ID", frmEntri.IDAkun)
                GridView1.SelectRow(GridView1.FocusedRowHandle)
            End If
        Catch ex As Exception
            MsgBox("Untuk mengedit pilih data yang akan diedit terlebih dahulu lalu tekan tombol edit", MessageBoxButtons.OK + MessageBoxIcon.Error, "Pesan Kesalahan")
        Finally
            frmEntri.Close()
            frmEntri.Dispose()
        End Try
    End Sub

    Private Sub cmdRefresh_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdRefresh.Click
        TampilData()
        'GridControl1.ExportToXls(Application.StartupPath & "\DaftarAkun.xls")
    End Sub

    Private Sub SimpleButton1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SimpleButton1.Click
        TampilData()
    End Sub

    Private Sub GridControl1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles GridControl1.Click

    End Sub

    Private Sub GridControl1_DoubleClick1(ByVal sender As Object, ByVal e As System.EventArgs) Handles GridControl1.DoubleClick
        If BolehAmbilData Then
            If pStatus = mdlAccPublik.ptipe.LookUp Then
                AmbilData()
            ElseIf pStatus = mdlAccPublik.ptipe.Lihat Then
                cmdEdit.PerformClick()
            End If
        End If
    End Sub

    Private Sub GridControl1_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles GridControl1.KeyDown
        If e.KeyCode = Keys.Enter Then
            If pStatus = mdlAccPublik.ptipe.LookUp Then
                AmbilData()
            ElseIf pStatus = mdlAccPublik.ptipe.Lihat Then
                cmdEdit.PerformClick()
            End If
        ElseIf e.KeyCode = Keys.Escape Then
            DialogResult = Windows.Forms.DialogResult.Cancel
            Close()
            'ElseIf e.KeyData = Keys.Control + Keys.P Then
            '    PrintGrid(GridControl1)
            'ElseIf e.KeyData = Keys.Control + Keys.S Then
            '    ExportGrid(GridControl1, ExportTo.Excel)
        End If
    End Sub

    Private Sub GridControl1_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles GridControl1.MouseDown
        Dim HI As New DevExpress.XtraGrid.Views.Grid.ViewInfo.GridHitInfo
        HI = GridView1.CalcHitInfo(e.X, e.Y)
        If HI.InRow Then
            BolehAmbilData = True
        Else
            BolehAmbilData = False
        End If

    End Sub

    Private Sub frmLUAkun_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        GridView1.SaveLayoutToXml(Application.StartupPath & "\System\Layouts\" & (Me.Name & GridView1.Name) & ".xml")
        LayoutControl1.SaveLayoutToXml(Application.StartupPath & "\System\Layouts\" & (Me.Name & LayoutControl1.Name) & ".xml")
    End Sub
    Private Sub frmDaftarAlamat_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        TampilData()
        If System.IO.File.Exists(Application.StartupPath & "\System\Layouts\" & (Me.Name & GridView1.Name) & ".xml") Then
            GridView1.RestoreLayoutFromXml(Application.StartupPath & "\System\Layouts\" & (Me.Name & GridView1.Name) & ".xml")
        End If
    End Sub

    Private Sub cmdBaru_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdBaru.Click
        Dim x As New frmEntriAkun
        Try
            x.pStatus = mdlAccPublik.ptipe.Baru
            x.IDAkun = 0
            If x.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
                TampilData()
                GridView1.ClearSelection()
                GridView1.FocusedRowHandle = GridView1.LocateByValue("ID", x.IDAkun)
                GridView1.SelectRow(GridView1.FocusedRowHandle)
            End If
        Catch ex As Exception
            FxMessage(ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error, , , ex.StackTrace)
        Finally
            x.Close()
            x.Dispose()
        End Try
    End Sub

    Private Sub cmClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmClose.Click
        Close()
    End Sub

    Private Sub cmdHapus_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdHapus.Click
        Dim view As ColumnView = GridControl1.FocusedView
        'Try
        Dim row As System.Data.DataRow = view.GetDataRow(GridView1.FocusedRowHandle)
        Dim NOid As Long = row("ID")
        If FxMessage("Yakin Mau Hapus data ini?", ".:: HAPUS DATA AKUN ::.", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
            EksekusiSQL("DELETE FROM MAkun WHERE ID=" & NOid)
            TampilData()
        End If
    End Sub

 
    Private Sub txtDepartemen_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)

    End Sub

    Private Sub cmdPreview_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdPreview.Click
        PrintPreview()
    End Sub

    Private Sub PrintPreview()
        'GridControl1.ShowPrintPreview()
        Dim NamaFile As String = Application.StartupPath & "\Report\Akun.rpt"
        Try
            If EditReport Then
                mdlCetakCR.ViewReport(Me.MdiParent, mdlCetakCR.action_.Edit, NamaFile, "Daftar Akun / Perkiraan")
            Else
                mdlCetakCR.ViewReport(Me.MdiParent, mdlCetakCR.action_.Preview, NamaFile, "Daftar Akun / Perkiraan")
            End If
        Catch ex As Exception

        End Try
    End Sub

    Private Sub ExportExcel()
        Dim dlgsave As New SaveFileDialog
        dlgsave.Title = "Export Daftar ke Excel"
        dlgsave.Filter = "Excel Files|*.xls"
        If dlgsave.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
            GridView1.ExportToXls(dlgsave.FileName)
            BukaFile(dlgsave.FileName)
        End If
        dlgsave.Dispose()
    End Sub

    Private Sub cmdExport_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdExport.Click
        ExportExcel()
    End Sub

End Class

