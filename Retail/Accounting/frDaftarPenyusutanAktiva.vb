Imports System.Data.SqlClient
Imports System.Windows.Forms
Imports DevExpress.XtraEditors
Imports DevExpress.XtraGrid.Views.Base

Public Class frDaftarPenyusutanAktiva
    Inherits DevExpress.XtraEditors.XtraForm

#Region " Windows Form Designer generated code "

    Public Sub New()
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()

        'Add any initialization after the InitializeComponent() call

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
    Friend WithEvents ImageList1 As System.Windows.Forms.ImageList
    Friend WithEvents PanelControl1 As DevExpress.XtraEditors.PanelControl
    Friend WithEvents PanelControl2 As DevExpress.XtraEditors.PanelControl
    Friend WithEvents PanelControl3 As DevExpress.XtraEditors.PanelControl
    Friend WithEvents SimpleButton7 As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents SimpleButton6 As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents cmdPosting As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents SimpleButton2 As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents SimpleButton1 As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents GridControl1 As DevExpress.XtraGrid.GridControl
    Friend WithEvents GridView1 As DevExpress.XtraGrid.Views.Grid.GridView
    Friend WithEvents colTipe As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colNama As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents cBiayaSusut As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents cCatatan As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents cNoID As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents cTanggalPerolehan As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents cTanggal As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents cIsPosted As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents RepositoryItemCheckEdit1 As DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit
    Friend WithEvents TglDari As DevExpress.XtraEditors.DateEdit
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents TglSampai As DevExpress.XtraEditors.DateEdit
    Friend WithEvents Label14 As System.Windows.Forms.Label
    Friend WithEvents c2NoID As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents c2Kode As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents c2Tanggal As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents c2Jumlah As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents cmdUnposting As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents cNilaiBuku As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents cNilaiPerolehan As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents cAkumulasiSusut As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents SaveFileDialog1 As System.Windows.Forms.SaveFileDialog
    Friend WithEvents cAkunAkumulasi As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents cAkunSusut As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents SimpleButton3 As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents txtTipe As DevExpress.XtraEditors.ButtonEdit
    Friend WithEvents txtAsset As DevExpress.XtraEditors.ButtonEdit
    Friend WithEvents lbTipe As DevExpress.XtraEditors.LabelControl
    Friend WithEvents lbAsset As DevExpress.XtraEditors.LabelControl
    Friend WithEvents cKode As DevExpress.XtraGrid.Columns.GridColumn
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim GridLevelNode1 As DevExpress.XtraGrid.GridLevelNode = New DevExpress.XtraGrid.GridLevelNode
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frDaftarPenyusutanAktiva))
        Me.c2NoID = New DevExpress.XtraGrid.Columns.GridColumn
        Me.c2Kode = New DevExpress.XtraGrid.Columns.GridColumn
        Me.c2Tanggal = New DevExpress.XtraGrid.Columns.GridColumn
        Me.c2Jumlah = New DevExpress.XtraGrid.Columns.GridColumn
        Me.GridControl1 = New DevExpress.XtraGrid.GridControl
        Me.GridView1 = New DevExpress.XtraGrid.Views.Grid.GridView
        Me.cNoID = New DevExpress.XtraGrid.Columns.GridColumn
        Me.colTipe = New DevExpress.XtraGrid.Columns.GridColumn
        Me.cKode = New DevExpress.XtraGrid.Columns.GridColumn
        Me.colNama = New DevExpress.XtraGrid.Columns.GridColumn
        Me.cCatatan = New DevExpress.XtraGrid.Columns.GridColumn
        Me.cTanggal = New DevExpress.XtraGrid.Columns.GridColumn
        Me.cTanggalPerolehan = New DevExpress.XtraGrid.Columns.GridColumn
        Me.cBiayaSusut = New DevExpress.XtraGrid.Columns.GridColumn
        Me.cNilaiPerolehan = New DevExpress.XtraGrid.Columns.GridColumn
        Me.cNilaiBuku = New DevExpress.XtraGrid.Columns.GridColumn
        Me.cAkumulasiSusut = New DevExpress.XtraGrid.Columns.GridColumn
        Me.cIsPosted = New DevExpress.XtraGrid.Columns.GridColumn
        Me.RepositoryItemCheckEdit1 = New DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit
        Me.cAkunAkumulasi = New DevExpress.XtraGrid.Columns.GridColumn
        Me.cAkunSusut = New DevExpress.XtraGrid.Columns.GridColumn
        Me.ImageList1 = New System.Windows.Forms.ImageList(Me.components)
        Me.PanelControl1 = New DevExpress.XtraEditors.PanelControl
        Me.lbTipe = New DevExpress.XtraEditors.LabelControl
        Me.lbAsset = New DevExpress.XtraEditors.LabelControl
        Me.txtTipe = New DevExpress.XtraEditors.ButtonEdit
        Me.txtAsset = New DevExpress.XtraEditors.ButtonEdit
        Me.Label1 = New System.Windows.Forms.Label
        Me.TglSampai = New DevExpress.XtraEditors.DateEdit
        Me.Label14 = New System.Windows.Forms.Label
        Me.TglDari = New DevExpress.XtraEditors.DateEdit
        Me.PanelControl2 = New DevExpress.XtraEditors.PanelControl
        Me.PanelControl3 = New DevExpress.XtraEditors.PanelControl
        Me.SimpleButton3 = New DevExpress.XtraEditors.SimpleButton
        Me.cmdUnposting = New DevExpress.XtraEditors.SimpleButton
        Me.SimpleButton7 = New DevExpress.XtraEditors.SimpleButton
        Me.SimpleButton6 = New DevExpress.XtraEditors.SimpleButton
        Me.SimpleButton2 = New DevExpress.XtraEditors.SimpleButton
        Me.SimpleButton1 = New DevExpress.XtraEditors.SimpleButton
        Me.cmdPosting = New DevExpress.XtraEditors.SimpleButton
        Me.SaveFileDialog1 = New System.Windows.Forms.SaveFileDialog
        CType(Me.GridControl1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.GridView1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RepositoryItemCheckEdit1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PanelControl1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.PanelControl1.SuspendLayout()
        CType(Me.txtTipe.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtAsset.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.TglSampai.Properties.VistaTimeProperties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.TglSampai.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.TglDari.Properties.VistaTimeProperties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.TglDari.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PanelControl2, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.PanelControl2.SuspendLayout()
        CType(Me.PanelControl3, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.PanelControl3.SuspendLayout()
        Me.SuspendLayout()
        '
        'c2NoID
        '
        Me.c2NoID.Name = "c2NoID"
        '
        'c2Kode
        '
        Me.c2Kode.Name = "c2Kode"
        '
        'c2Tanggal
        '
        Me.c2Tanggal.Name = "c2Tanggal"
        '
        'c2Jumlah
        '
        Me.c2Jumlah.Name = "c2Jumlah"
        '
        'GridControl1
        '
        Me.GridControl1.Dock = System.Windows.Forms.DockStyle.Fill
        GridLevelNode1.RelationName = "Level1"
        Me.GridControl1.LevelTree.Nodes.AddRange(New DevExpress.XtraGrid.GridLevelNode() {GridLevelNode1})
        Me.GridControl1.Location = New System.Drawing.Point(105, 2)
        Me.GridControl1.MainView = Me.GridView1
        Me.GridControl1.Name = "GridControl1"
        Me.GridControl1.RepositoryItems.AddRange(New DevExpress.XtraEditors.Repository.RepositoryItem() {Me.RepositoryItemCheckEdit1})
        Me.GridControl1.Size = New System.Drawing.Size(904, 436)
        Me.GridControl1.TabIndex = 0
        Me.GridControl1.ViewCollection.AddRange(New DevExpress.XtraGrid.Views.Base.BaseView() {Me.GridView1})
        '
        'GridView1
        '
        Me.GridView1.Columns.AddRange(New DevExpress.XtraGrid.Columns.GridColumn() {Me.cNoID, Me.colTipe, Me.cKode, Me.colNama, Me.cCatatan, Me.cTanggal, Me.cTanggalPerolehan, Me.cBiayaSusut, Me.cNilaiPerolehan, Me.cNilaiBuku, Me.cAkumulasiSusut, Me.cIsPosted, Me.cAkunAkumulasi, Me.cAkunSusut})
        Me.GridView1.CustomizationFormBounds = New System.Drawing.Rectangle(808, 462, 216, 171)
        Me.GridView1.GridControl = Me.GridControl1
        Me.GridView1.GroupSummary.AddRange(New DevExpress.XtraGrid.GridSummaryItem() {New DevExpress.XtraGrid.GridGroupSummaryItem(DevExpress.Data.SummaryItemType.Sum, "HargaSusut", Nothing, "{0}: [#image]{1} {2}")})
        Me.GridView1.Name = "GridView1"
        Me.GridView1.OptionsBehavior.AllowIncrementalSearch = True
        Me.GridView1.OptionsBehavior.AutoSelectAllInEditor = False
        Me.GridView1.OptionsBehavior.Editable = False
        Me.GridView1.OptionsSelection.MultiSelect = True
        Me.GridView1.OptionsView.ColumnAutoWidth = False
        Me.GridView1.OptionsView.ShowFooter = True
        '
        'cNoID
        '
        Me.cNoID.Caption = "NoID"
        Me.cNoID.FieldName = "NoID"
        Me.cNoID.Name = "cNoID"
        Me.cNoID.Width = 36
        '
        'colTipe
        '
        Me.colTipe.Caption = "Tipe"
        Me.colTipe.FieldName = "Tipe"
        Me.colTipe.Name = "colTipe"
        Me.colTipe.Visible = True
        Me.colTipe.VisibleIndex = 1
        Me.colTipe.Width = 79
        '
        'cKode
        '
        Me.cKode.Caption = "Kode"
        Me.cKode.FieldName = "Kode"
        Me.cKode.Name = "cKode"
        Me.cKode.Visible = True
        Me.cKode.VisibleIndex = 0
        Me.cKode.Width = 36
        '
        'colNama
        '
        Me.colNama.Caption = "Nama"
        Me.colNama.FieldName = "Nama"
        Me.colNama.Name = "colNama"
        Me.colNama.SummaryItem.DisplayFormat = "{0:###,###,###,##0}"
        Me.colNama.SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Count
        Me.colNama.Visible = True
        Me.colNama.VisibleIndex = 2
        Me.colNama.Width = 82
        '
        'cCatatan
        '
        Me.cCatatan.Caption = "Catatan"
        Me.cCatatan.FieldName = "Catatan"
        Me.cCatatan.Name = "cCatatan"
        Me.cCatatan.Visible = True
        Me.cCatatan.VisibleIndex = 6
        Me.cCatatan.Width = 51
        '
        'cTanggal
        '
        Me.cTanggal.Caption = "Tanggal"
        Me.cTanggal.DisplayFormat.FormatString = "dd-MM-yyyy"
        Me.cTanggal.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime
        Me.cTanggal.FieldName = "Tanggal"
        Me.cTanggal.Name = "cTanggal"
        Me.cTanggal.Visible = True
        Me.cTanggal.VisibleIndex = 4
        Me.cTanggal.Width = 74
        '
        'cTanggalPerolehan
        '
        Me.cTanggalPerolehan.Caption = "Tanggal Perolehan"
        Me.cTanggalPerolehan.DisplayFormat.FormatString = "dd-MM-yyyy"
        Me.cTanggalPerolehan.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime
        Me.cTanggalPerolehan.FieldName = "TanggalPerolehan"
        Me.cTanggalPerolehan.Name = "cTanggalPerolehan"
        Me.cTanggalPerolehan.Visible = True
        Me.cTanggalPerolehan.VisibleIndex = 3
        Me.cTanggalPerolehan.Width = 104
        '
        'cBiayaSusut
        '
        Me.cBiayaSusut.AppearanceCell.Options.UseTextOptions = True
        Me.cBiayaSusut.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
        Me.cBiayaSusut.AppearanceHeader.Options.UseTextOptions = True
        Me.cBiayaSusut.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
        Me.cBiayaSusut.Caption = "Biaya Susut"
        Me.cBiayaSusut.DisplayFormat.FormatString = "###,###,###,##0.00"
        Me.cBiayaSusut.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
        Me.cBiayaSusut.FieldName = "HargaSusut"
        Me.cBiayaSusut.Name = "cBiayaSusut"
        Me.cBiayaSusut.SummaryItem.DisplayFormat = "{0:###,###,###,##0.00}"
        Me.cBiayaSusut.SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum
        Me.cBiayaSusut.Visible = True
        Me.cBiayaSusut.VisibleIndex = 5
        Me.cBiayaSusut.Width = 45
        '
        'cNilaiPerolehan
        '
        Me.cNilaiPerolehan.Caption = "Harga Perolehan"
        Me.cNilaiPerolehan.DisplayFormat.FormatString = "###,##0.00"
        Me.cNilaiPerolehan.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
        Me.cNilaiPerolehan.FieldName = "HargaPerolehan"
        Me.cNilaiPerolehan.Name = "cNilaiPerolehan"
        Me.cNilaiPerolehan.SummaryItem.DisplayFormat = "{0:###,###,###,##0.00}"
        Me.cNilaiPerolehan.SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum
        Me.cNilaiPerolehan.Visible = True
        Me.cNilaiPerolehan.VisibleIndex = 9
        '
        'cNilaiBuku
        '
        Me.cNilaiBuku.Caption = "Nilai Buku"
        Me.cNilaiBuku.DisplayFormat.FormatString = "###,###,###,##0.00"
        Me.cNilaiBuku.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
        Me.cNilaiBuku.FieldName = "NilaiBuku"
        Me.cNilaiBuku.Name = "cNilaiBuku"
        Me.cNilaiBuku.SummaryItem.DisplayFormat = "{0:###,###,###,##0.00}"
        Me.cNilaiBuku.SummaryItem.FieldName = "HargaBuku"
        Me.cNilaiBuku.SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum
        Me.cNilaiBuku.Visible = True
        Me.cNilaiBuku.VisibleIndex = 8
        '
        'cAkumulasiSusut
        '
        Me.cAkumulasiSusut.Caption = "Akumulasi Susut"
        Me.cAkumulasiSusut.DisplayFormat.FormatString = "###,###,###,##0.00"
        Me.cAkumulasiSusut.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
        Me.cAkumulasiSusut.FieldName = "AkumulasiSusut"
        Me.cAkumulasiSusut.Name = "cAkumulasiSusut"
        Me.cAkumulasiSusut.SummaryItem.DisplayFormat = "{0:###,###,###,##0.00}"
        Me.cAkumulasiSusut.SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum
        Me.cAkumulasiSusut.Visible = True
        Me.cAkumulasiSusut.VisibleIndex = 10
        '
        'cIsPosted
        '
        Me.cIsPosted.AppearanceCell.Options.UseTextOptions = True
        Me.cIsPosted.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.cIsPosted.AppearanceHeader.Options.UseTextOptions = True
        Me.cIsPosted.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.cIsPosted.Caption = "IsPosted"
        Me.cIsPosted.ColumnEdit = Me.RepositoryItemCheckEdit1
        Me.cIsPosted.FieldName = "IsPost"
        Me.cIsPosted.Name = "cIsPosted"
        Me.cIsPosted.Visible = True
        Me.cIsPosted.VisibleIndex = 7
        Me.cIsPosted.Width = 54
        '
        'RepositoryItemCheckEdit1
        '
        Me.RepositoryItemCheckEdit1.AutoHeight = False
        Me.RepositoryItemCheckEdit1.Name = "RepositoryItemCheckEdit1"
        '
        'cAkunAkumulasi
        '
        Me.cAkunAkumulasi.Caption = "Akun Akumulasi"
        Me.cAkunAkumulasi.FieldName = "AkunAkumulasi"
        Me.cAkunAkumulasi.Name = "cAkunAkumulasi"
        Me.cAkunAkumulasi.Visible = True
        Me.cAkunAkumulasi.VisibleIndex = 11
        '
        'cAkunSusut
        '
        Me.cAkunSusut.Caption = "Akun Penyusutan"
        Me.cAkunSusut.FieldName = "AkunPenyusutan"
        Me.cAkunSusut.Name = "cAkunSusut"
        Me.cAkunSusut.Visible = True
        Me.cAkunSusut.VisibleIndex = 12
        '
        'ImageList1
        '
        Me.ImageList1.ImageStream = CType(resources.GetObject("ImageList1.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.ImageList1.TransparentColor = System.Drawing.Color.Transparent
        Me.ImageList1.Images.SetKeyName(0, "")
        Me.ImageList1.Images.SetKeyName(1, "")
        Me.ImageList1.Images.SetKeyName(2, "")
        Me.ImageList1.Images.SetKeyName(3, "")
        Me.ImageList1.Images.SetKeyName(4, "")
        Me.ImageList1.Images.SetKeyName(5, "")
        Me.ImageList1.Images.SetKeyName(6, "")
        Me.ImageList1.Images.SetKeyName(7, "")
        Me.ImageList1.Images.SetKeyName(8, "")
        Me.ImageList1.Images.SetKeyName(9, "")
        Me.ImageList1.Images.SetKeyName(10, "")
        Me.ImageList1.Images.SetKeyName(11, "")
        Me.ImageList1.Images.SetKeyName(12, "")
        Me.ImageList1.Images.SetKeyName(13, "")
        Me.ImageList1.Images.SetKeyName(14, "")
        Me.ImageList1.Images.SetKeyName(15, "")
        Me.ImageList1.Images.SetKeyName(16, "")
        Me.ImageList1.Images.SetKeyName(17, "")
        Me.ImageList1.Images.SetKeyName(18, "")
        Me.ImageList1.Images.SetKeyName(19, "")
        Me.ImageList1.Images.SetKeyName(20, "")
        Me.ImageList1.Images.SetKeyName(21, "")
        Me.ImageList1.Images.SetKeyName(22, "")
        Me.ImageList1.Images.SetKeyName(23, "")
        Me.ImageList1.Images.SetKeyName(24, "add.png")
        Me.ImageList1.Images.SetKeyName(25, "delete.png")
        '
        'PanelControl1
        '
        Me.PanelControl1.Appearance.BackColor = System.Drawing.Color.Transparent
        Me.PanelControl1.Appearance.Options.UseBackColor = True
        Me.PanelControl1.ContentImageAlignment = System.Drawing.ContentAlignment.MiddleLeft
        Me.PanelControl1.Controls.Add(Me.lbTipe)
        Me.PanelControl1.Controls.Add(Me.lbAsset)
        Me.PanelControl1.Controls.Add(Me.txtTipe)
        Me.PanelControl1.Controls.Add(Me.txtAsset)
        Me.PanelControl1.Controls.Add(Me.Label1)
        Me.PanelControl1.Controls.Add(Me.TglSampai)
        Me.PanelControl1.Controls.Add(Me.Label14)
        Me.PanelControl1.Controls.Add(Me.TglDari)
        Me.PanelControl1.Dock = System.Windows.Forms.DockStyle.Top
        Me.PanelControl1.Location = New System.Drawing.Point(0, 0)
        Me.PanelControl1.LookAndFeel.SkinName = "Glass Oceans"
        Me.PanelControl1.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.UltraFlat
        Me.PanelControl1.Name = "PanelControl1"
        Me.PanelControl1.Size = New System.Drawing.Size(1011, 38)
        Me.PanelControl1.TabIndex = 0
        '
        'lbTipe
        '
        Me.lbTipe.Appearance.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbTipe.Appearance.ForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.lbTipe.Appearance.Options.UseFont = True
        Me.lbTipe.Appearance.Options.UseForeColor = True
        Me.lbTipe.Cursor = System.Windows.Forms.Cursors.Hand
        Me.lbTipe.Location = New System.Drawing.Point(753, 11)
        Me.lbTipe.Name = "lbTipe"
        Me.lbTipe.Size = New System.Drawing.Size(60, 16)
        Me.lbTipe.TabIndex = 9
        Me.lbTipe.Text = "Tipe Asset"
        Me.lbTipe.ToolTip = "klik disini untuk mengaktifkan/menonaktifkan filter asset"
        '
        'lbAsset
        '
        Me.lbAsset.Appearance.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbAsset.Appearance.ForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.lbAsset.Appearance.Options.UseFont = True
        Me.lbAsset.Appearance.Options.UseForeColor = True
        Me.lbAsset.Cursor = System.Windows.Forms.Cursors.Hand
        Me.lbAsset.Location = New System.Drawing.Point(493, 9)
        Me.lbAsset.Name = "lbAsset"
        Me.lbAsset.Size = New System.Drawing.Size(31, 16)
        Me.lbAsset.TabIndex = 8
        Me.lbAsset.Text = "Asset"
        Me.lbAsset.ToolTip = "klik disini untuk mengaktifkan/menonaktifkan filter asset"
        '
        'txtTipe
        '
        Me.txtTipe.Enabled = False
        Me.txtTipe.EnterMoveNextControl = True
        Me.txtTipe.Location = New System.Drawing.Point(820, 6)
        Me.txtTipe.Name = "txtTipe"
        Me.txtTipe.Properties.Appearance.BackColor = System.Drawing.Color.White
        Me.txtTipe.Properties.Appearance.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtTipe.Properties.Appearance.Options.UseBackColor = True
        Me.txtTipe.Properties.Appearance.Options.UseFont = True
        Me.txtTipe.Properties.AppearanceDisabled.BackColor = System.Drawing.Color.Silver
        Me.txtTipe.Properties.AppearanceDisabled.BackColor2 = System.Drawing.Color.Silver
        Me.txtTipe.Properties.AppearanceDisabled.Options.UseBackColor = True
        Me.txtTipe.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton, New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Delete)})
        Me.txtTipe.Properties.ReadOnly = True
        Me.txtTipe.Size = New System.Drawing.Size(184, 22)
        Me.txtTipe.TabIndex = 6
        Me.txtTipe.TabStop = False
        '
        'txtAsset
        '
        Me.txtAsset.Enabled = False
        Me.txtAsset.EnterMoveNextControl = True
        Me.txtAsset.Location = New System.Drawing.Point(542, 6)
        Me.txtAsset.Name = "txtAsset"
        Me.txtAsset.Properties.Appearance.BackColor = System.Drawing.Color.White
        Me.txtAsset.Properties.Appearance.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtAsset.Properties.Appearance.Options.UseBackColor = True
        Me.txtAsset.Properties.Appearance.Options.UseFont = True
        Me.txtAsset.Properties.AppearanceDisabled.BackColor = System.Drawing.Color.Silver
        Me.txtAsset.Properties.AppearanceDisabled.BackColor2 = System.Drawing.Color.Silver
        Me.txtAsset.Properties.AppearanceDisabled.Options.UseBackColor = True
        Me.txtAsset.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton, New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Delete)})
        Me.txtAsset.Properties.ReadOnly = True
        Me.txtAsset.Size = New System.Drawing.Size(184, 22)
        Me.txtAsset.TabIndex = 4
        Me.txtAsset.TabStop = False
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.BackColor = System.Drawing.Color.Transparent
        Me.Label1.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.ForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.Label1.Location = New System.Drawing.Point(288, 8)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(26, 16)
        Me.Label1.TabIndex = 2
        Me.Label1.Text = "s/d"
        '
        'TglSampai
        '
        Me.TglSampai.EditValue = Nothing
        Me.TglSampai.EnterMoveNextControl = True
        Me.TglSampai.Location = New System.Drawing.Point(320, 5)
        Me.TglSampai.Name = "TglSampai"
        Me.TglSampai.Properties.Appearance.BackColor = System.Drawing.Color.White
        Me.TglSampai.Properties.Appearance.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TglSampai.Properties.Appearance.Options.UseBackColor = True
        Me.TglSampai.Properties.Appearance.Options.UseFont = True
        Me.TglSampai.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.TglSampai.Properties.VistaTimeProperties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton})
        Me.TglSampai.Size = New System.Drawing.Size(141, 22)
        Me.TglSampai.TabIndex = 3
        Me.TglSampai.TabStop = False
        '
        'Label14
        '
        Me.Label14.AutoSize = True
        Me.Label14.BackColor = System.Drawing.Color.Transparent
        Me.Label14.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label14.ForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.Label14.Location = New System.Drawing.Point(12, 9)
        Me.Label14.Name = "Label14"
        Me.Label14.Size = New System.Drawing.Size(123, 16)
        Me.Label14.TabIndex = 0
        Me.Label14.Text = "Tanggal Penyusutan"
        '
        'TglDari
        '
        Me.TglDari.EditValue = Nothing
        Me.TglDari.EnterMoveNextControl = True
        Me.TglDari.Location = New System.Drawing.Point(141, 5)
        Me.TglDari.Name = "TglDari"
        Me.TglDari.Properties.Appearance.BackColor = System.Drawing.Color.White
        Me.TglDari.Properties.Appearance.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TglDari.Properties.Appearance.Options.UseBackColor = True
        Me.TglDari.Properties.Appearance.Options.UseFont = True
        Me.TglDari.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.TglDari.Properties.VistaTimeProperties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton})
        Me.TglDari.Size = New System.Drawing.Size(141, 22)
        Me.TglDari.TabIndex = 1
        Me.TglDari.TabStop = False
        '
        'PanelControl2
        '
        Me.PanelControl2.Appearance.BackColor = System.Drawing.Color.Transparent
        Me.PanelControl2.Appearance.Options.UseBackColor = True
        Me.PanelControl2.Controls.Add(Me.GridControl1)
        Me.PanelControl2.Controls.Add(Me.PanelControl3)
        Me.PanelControl2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.PanelControl2.Location = New System.Drawing.Point(0, 38)
        Me.PanelControl2.LookAndFeel.SkinName = "Glass Oceans"
        Me.PanelControl2.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.UltraFlat
        Me.PanelControl2.Name = "PanelControl2"
        Me.PanelControl2.Size = New System.Drawing.Size(1011, 440)
        Me.PanelControl2.TabIndex = 1
        '
        'PanelControl3
        '
        Me.PanelControl3.Appearance.BackColor = System.Drawing.Color.LightSalmon
        Me.PanelControl3.Appearance.Options.UseBackColor = True
        Me.PanelControl3.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder
        Me.PanelControl3.Controls.Add(Me.SimpleButton3)
        Me.PanelControl3.Controls.Add(Me.cmdUnposting)
        Me.PanelControl3.Controls.Add(Me.SimpleButton7)
        Me.PanelControl3.Controls.Add(Me.SimpleButton6)
        Me.PanelControl3.Controls.Add(Me.SimpleButton2)
        Me.PanelControl3.Controls.Add(Me.SimpleButton1)
        Me.PanelControl3.Controls.Add(Me.cmdPosting)
        Me.PanelControl3.Dock = System.Windows.Forms.DockStyle.Left
        Me.PanelControl3.Location = New System.Drawing.Point(2, 2)
        Me.PanelControl3.Name = "PanelControl3"
        Me.PanelControl3.Size = New System.Drawing.Size(103, 436)
        Me.PanelControl3.TabIndex = 1
        '
        'SimpleButton3
        '
        Me.SimpleButton3.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.SimpleButton3.Cursor = System.Windows.Forms.Cursors.Hand
        Me.SimpleButton3.Location = New System.Drawing.Point(2, 123)
        Me.SimpleButton3.Name = "SimpleButton3"
        Me.SimpleButton3.Size = New System.Drawing.Size(99, 34)
        Me.SimpleButton3.TabIndex = 8
        Me.SimpleButton3.Text = "&View Jurnal"
        '
        'cmdUnposting
        '
        Me.cmdUnposting.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdUnposting.Cursor = System.Windows.Forms.Cursors.Hand
        Me.cmdUnposting.Location = New System.Drawing.Point(2, 83)
        Me.cmdUnposting.Name = "cmdUnposting"
        Me.cmdUnposting.Size = New System.Drawing.Size(99, 34)
        Me.cmdUnposting.TabIndex = 7
        Me.cmdUnposting.Text = "&UnPosting"
        '
        'SimpleButton7
        '
        Me.SimpleButton7.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.SimpleButton7.Cursor = System.Windows.Forms.Cursors.Hand
        Me.SimpleButton7.ImageIndex = 2
        Me.SimpleButton7.ImageList = Me.ImageList1
        Me.SimpleButton7.Location = New System.Drawing.Point(1, 391)
        Me.SimpleButton7.Name = "SimpleButton7"
        Me.SimpleButton7.Size = New System.Drawing.Size(99, 34)
        Me.SimpleButton7.TabIndex = 0
        Me.SimpleButton7.Text = "&Close"
        '
        'SimpleButton6
        '
        Me.SimpleButton6.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.SimpleButton6.Cursor = System.Windows.Forms.Cursors.Hand
        Me.SimpleButton6.ImageIndex = 17
        Me.SimpleButton6.ImageList = Me.ImageList1
        Me.SimpleButton6.Location = New System.Drawing.Point(2, 3)
        Me.SimpleButton6.Name = "SimpleButton6"
        Me.SimpleButton6.Size = New System.Drawing.Size(99, 34)
        Me.SimpleButton6.TabIndex = 0
        Me.SimpleButton6.Text = "&Refresh"
        '
        'SimpleButton2
        '
        Me.SimpleButton2.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.SimpleButton2.Cursor = System.Windows.Forms.Cursors.Hand
        Me.SimpleButton2.ImageIndex = 0
        Me.SimpleButton2.ImageList = Me.ImageList1
        Me.SimpleButton2.Location = New System.Drawing.Point(2, 247)
        Me.SimpleButton2.Name = "SimpleButton2"
        Me.SimpleButton2.Size = New System.Drawing.Size(99, 34)
        Me.SimpleButton2.TabIndex = 5
        Me.SimpleButton2.Text = "&Export Excel"
        '
        'SimpleButton1
        '
        Me.SimpleButton1.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.SimpleButton1.Cursor = System.Windows.Forms.Cursors.Hand
        Me.SimpleButton1.ImageIndex = 10
        Me.SimpleButton1.ImageList = Me.ImageList1
        Me.SimpleButton1.Location = New System.Drawing.Point(2, 207)
        Me.SimpleButton1.Name = "SimpleButton1"
        Me.SimpleButton1.Size = New System.Drawing.Size(99, 34)
        Me.SimpleButton1.TabIndex = 1
        Me.SimpleButton1.Text = "&Preview"
        '
        'cmdPosting
        '
        Me.cmdPosting.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdPosting.Cursor = System.Windows.Forms.Cursors.Hand
        Me.cmdPosting.Location = New System.Drawing.Point(2, 43)
        Me.cmdPosting.Name = "cmdPosting"
        Me.cmdPosting.Size = New System.Drawing.Size(99, 34)
        Me.cmdPosting.TabIndex = 6
        Me.cmdPosting.Text = "&Posting"
        '
        'frDaftarPenyusutanAktiva
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 14)
        Me.ClientSize = New System.Drawing.Size(1011, 478)
        Me.Controls.Add(Me.PanelControl2)
        Me.Controls.Add(Me.PanelControl1)
        Me.Name = "frDaftarPenyusutanAktiva"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Daftar Penyusutan Asset"
        CType(Me.GridControl1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.GridView1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RepositoryItemCheckEdit1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PanelControl1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.PanelControl1.ResumeLayout(False)
        Me.PanelControl1.PerformLayout()
        CType(Me.txtTipe.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtAsset.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.TglSampai.Properties.VistaTimeProperties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.TglSampai.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.TglDari.Properties.VistaTimeProperties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.TglDari.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PanelControl2, System.ComponentModel.ISupportInitialize).EndInit()
        Me.PanelControl2.ResumeLayout(False)
        CType(Me.PanelControl3, System.ComponentModel.ISupportInitialize).EndInit()
        Me.PanelControl3.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub

#End Region
    Public IsGetOnly As Boolean
    Public NoID As Long
    Public Kode As String
    Dim BolehAmbilData As Boolean
    Public IDAsset As Long
    Public IDTipeAsset As Long
    Sub RefreshData()
        Dim oConn As New SqlConnection
        Dim ocmd As SqlCommand
        Dim oDA As SqlDataAdapter
        Dim oDS As New DataSet
        Dim strsql As String
        Dim IsPerluwhere As Boolean = True
        Dim curentcursor As Cursor = Windows.Forms.Cursor.Current
        Windows.Forms.Cursor.Current = Cursors.WaitCursor
        'MAsset.NilaiDibukukan-MAssetD.AkumulasiSusut as NilaiBuku 
        strsql = "SELECT MAssetD.*,MAsset.HargaPerolehan,MAsset.Tanggal as TanggalPerolehan, MAsset.Kode,MAsset.Nama,MTypeAsset.Nama as Tipe, " & _
        "MAsset.NilaiDibukukan-MAssetD.AkumulasiSusut as NilaiBuku,Makun.Nama as AkunAkumulasi,MakunSusut.Nama as AkunPenyusutan " & _
        "from MAssetD Left join Masset on MAssetD.IDAsset=MAsset.NoID " & _
        "Left Join MTypeAsset On MAsset.IDTypeAsset=MTypeAsset.NoID " & _
        "left Join Makun on MAsset.IDAAkumulasiSusut=MAkun.ID " & _
"left Join Makun MAkunSusut on MAsset.IDAPenyusutan=MAkunSusut.ID "

        If TglDari.Enabled Then
            strsql = strsql & " WHERE MAssetD.Tanggal>=convert(datetime,'" & Format(TglDari.DateTime, "MM/dd/yyyy") & "',101) " & _
            "AND MAssetD.Tanggal<=convert(datetime,'" & Format(TglSampai.DateTime, "yyyy/MM/dd") & "',101)"
            IsPerluwhere = False
        End If
        If txtAsset.Enabled = True Then
            If IsPerluwhere Then
                strsql = strsql & " where "
            Else
                strsql = strsql & " and "
            End If
            strsql = strsql & " MAssetD.IDAsset=" & IDAsset.ToString
            IsPerluwhere = False
        End If
        If txtTipe.Enabled = True Then
            If IsPerluwhere Then
                strsql = strsql & " where "
            Else
                strsql = strsql & " and "
            End If
            strsql = strsql & " MAsset.IDTypeAsset=" & IDTipeAsset.ToString
        End If
        Try
            oConn.ConnectionString = StrKonSql
            ocmd = New SqlCommand(strsql, oConn)
            oConn.Open()
            oDA = New SqlDataAdapter(ocmd)
            If oDS.Tables("MAssetD") Is Nothing Then
            Else
                oDS.Tables("MAssetD").Clear()
            End If
            oDA.Fill(oDS, "MAssetD")
        Catch ex As Exception
            XtraMessageBox.Show(Me, ex.Message, ".:: Pesan Kesalahan ::.", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            oConn.Close()
        End Try
        GridControl1.DataSource = oDS.Tables("MAssetD")
        Windows.Forms.Cursor.Current = curentcursor
    End Sub

    Private Sub frDaftarPenyusutanAktiva_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Activated
        'GridToPrint = GridControl1
    End Sub

    Private Sub frDaftarPenyusutanAktiva_Deactivate(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Deactivate
        'GridToPrint = Nothing
    End Sub

    Private Sub frDaftarPinjaman_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        GridView1.SaveLayoutToXml(Application.StartupPath & "\system\layouts\" & Me.Name & GridView1.Name & ".xml")
        'GridView2.SaveLayoutToXml(Application.StartupPath & "\layout\" & Me.Name & GridView2.Name & ".xml")
    End Sub
    Private Sub frDaftarBBM_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        If IsGetOnly Then PanelControl3.Visible = False
        TglDari.DateTime = Date.Today
        TglSampai.DateTime = Date.Today
        RefreshData()
        If Dir(Application.StartupPath & "\system\layouts\" & Me.Name & GridView1.Name & ".xml") <> "" Then
            GridView1.RestoreLayoutFromXml(Application.StartupPath & "\system\layouts\" & Me.Name & GridView1.Name & ".xml")
        End If
        Setme()
    End Sub

    Private Sub Setme()
        'SetButton(cmdPosting, button_.cmdPosting)
        'SetButton(cmdUnposting, button_.cmdUnposting)
    End Sub
    Private Sub SimpleButton3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdPosting.Click
        If MsgBox("Mau posting penyusutan asset?", MsgBoxStyle.Question + MsgBoxStyle.YesNoCancel) = MsgBoxResult.Yes Then
            Try
                Dim view As ColumnView = GridControl1.FocusedView
                Dim I As Integer
                Dim focusrow As Long
                focusrow = GridView1.FocusedRowHandle
                For I = 0 To view.SelectedRowsCount - 1
                    Dim row As System.Data.DataRow = view.GetDataRow(view.GetSelectedRows(I))
                    NoID = NullToLong(row("NoID"))
                    If NullToBool(row("IsPost")) = False Then
                        PostingPenyusutan(NoID)
                        'IDJurnal = GetNewID("MJurnal")

                        'clsSqlServer.EksekusiSQl("UPDATE MPiutangKaryawan SET IsPosted=1 where NoID=" & NoID)
                    End If
                Next
                RefreshData()
                GridView1.FocusedRowHandle = focusrow
                GridView1.SelectRow(focusrow)
            Catch ex As Exception
                XtraMessageBox.Show(Me, ex.Message, ".:: Pesan Kesalahan ::.", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End If
    End Sub

    Private Sub SimpleButton4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        DialogResult = Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub SimpleButton1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SimpleButton1.Click
        Try
            'GridView1.OptionsPrint.SizeMode = DevExpress.XtraCharts.Printing.PrintSizeMode.Zoom
            GridControl1.ShowPrintPreview()
        Catch
        End Try
    End Sub

    Private Sub SimpleButton2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SimpleButton2.Click

        Try
            Dim X As String
            SaveFileDialog1.Filter = "*.xls|Excel Files"
            If SaveFileDialog1.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
                X = SaveFileDialog1.FileName
                'GridControl1.OptionsPrint.SizeMode = DevExpress.XtraCharts.Printing.PrintSizeMode.Zoom
                GridControl1.ExportToXls(X)
                BukaFile(X)
            End If

        Catch
        End Try
    End Sub

    Private Sub SimpleButton5_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim view As ColumnView = GridControl1.FocusedView
        Dim row As System.Data.DataRow = view.GetDataRow(GridView1.FocusedRowHandle)
        Dim NoId As Long = row("NOID")
        If XtraMessageBox.Show(Me, "Yakin Mau Hapus data ini?", ".:: HAPUS DATA PINJAMAN ::.", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
            DeleteRowByID("MPiutangKaryawan", "NoID", NoId)
            RefreshData()
        End If
    End Sub

    Private Sub SimpleButton6_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SimpleButton6.Click
        RefreshData()
    End Sub

    Private Sub GridControl1_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs)
        If e.KeyCode = Keys.Enter Then
            AmbilData()
        ElseIf e.KeyCode = Keys.Escape Then
            DialogResult = Windows.Forms.DialogResult.Cancel
        End If
    End Sub

    Private Sub GridControl1_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs)
        AmbilData()
    End Sub
    Sub AmbilData()
        Dim view As ColumnView = GridControl1.FocusedView
        Dim row As System.Data.DataRow = view.GetDataRow(GridView1.FocusedRowHandle)
        NoID = row("NoID")
        Kode = row("Kode")
        DialogResult = Windows.Forms.DialogResult.OK
    End Sub
    '

    Private Sub SimpleButton7_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SimpleButton7.Click
        DialogResult = Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub

    Private Sub GridControl1_DoubleClick1(ByVal sender As Object, ByVal e As System.EventArgs) Handles GridControl1.DoubleClick
        If BolehAmbilData Then
            SimpleButton2.PerformClick()
        End If
    End Sub

    Private Sub GridControl1_KeyDown1(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles GridControl1.KeyDown
        If e.KeyCode = Keys.Enter Then
            SimpleButton2.PerformClick()
        ElseIf e.KeyCode = Keys.Escape Then
            DialogResult = Windows.Forms.DialogResult.Cancel
            Me.Close()
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

    Private Sub Label14_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Label14.Click
        TglDari.Enabled = Not TglDari.Enabled
        TglSampai.Enabled = TglDari.Enabled
    End Sub

    Private Sub cmdUnposting_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdUnposting.Click
        If MsgBox("Mau membatalkan posting penyusutan asset?", MsgBoxStyle.Question + MsgBoxStyle.YesNoCancel) = MsgBoxResult.Yes Then
            Try
                Dim view As ColumnView = GridControl1.FocusedView
                Dim I As Integer
                Dim focusrow As Long
                focusrow = GridView1.FocusedRowHandle

                For I = 0 To view.SelectedRowsCount - 1
                    Dim row As System.Data.DataRow = view.GetDataRow(view.GetSelectedRows(I))
                    NoID = NullToLong(row("NoID"))
                    If NullToBool(row("IsPost")) = True Then
                        UnPosting(NoID)
                    End If
                Next
                RefreshData()
                GridView1.FocusedRowHandle = focusrow
                GridView1.SelectRow(focusrow)
            Catch ex As Exception
                XtraMessageBox.Show(Me, ex.Message, ".:: Pesan Kesalahan ::.", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End If
    End Sub

    Function PostingPenyusutan(ByVal IDAssetD As Long) As Boolean
        Dim IDJurnal As Long
        Dim strsql As String
        IDJurnal = GetNewID("MJurnal", "id")
        strsql = "insert into MJurnal(ID,Kode,KodeReff,Tanggal,Keterangan,IDJenisTransaksi,IDUserEntry,IDUserPosting,IDTransaksi,IsPosting,IDDepartemenUser) "
        strsql = strsql & " select " & IDJurnal & ",Masset.Kode,Masset.Kode,Massetd.Tanggal,'Penyusutan ' + Masset.Nama,26," & IDUserAktif & "," & IDUserAktif & "," & IDAssetD & ",1,1 "
        strsql = strsql & " from Massetd inner join masset on massetd.idasset=masset.noid where massetd.noid=" & IDAssetD
        EksekusiSQL(strsql)

        strsql = "insert into MJurnald(IDJurnal,IDDepartemen,IDAkun,IDMataUang,Kurs,Debet,Kredit,DebetA,KreditA,Keterangan,IsBalancing) "
        strsql = strsql & " select " & IDJurnal & ",1,MAsset.IDAPenyusutan,1,1,MAssetD.HargaSusut,0,MAssetD.HargaSusut,0,'Penyusutan ' + MAsset.Nama,0 "
        strsql = strsql & " from Massetd inner join masset on massetd.idasset=masset.noid where massetd.noid=" & IDAssetD
        EksekusiSQL(strsql)

        strsql = "insert into MJurnald(IDJurnal,IDDepartemen,IDAkun,IDMataUang,Kurs,Debet,Kredit,DebetA,KreditA,Keterangan,IsBalancing) "
        strsql = strsql & " select " & IDJurnal & ",1,MAsset.IDAAkumulasisusut,1,1,0,MAssetD.HargaSusut,0,MAssetD.HargaSusut,'Penyusutan ' + MAsset.Nama,0 "
        strsql = strsql & " from Massetd inner join masset on massetd.idasset=masset.noid where massetd.noid=" & IDAssetD
        EksekusiSQL(strsql)
        EksekusiSQL("Update MAssetD set IsPost=1 where NoID=" & IDAssetD)

    End Function

    Function UnPosting(ByVal IDAssetD As Long) As Boolean
        Dim strsql As String
        strsql = "delete MJurnald  from MJurnald inner join Mjurnal on mjurnald.idjurnal=mjurnal.id " & _
                "where MJurnal.IDJenistransaksi=26 and MJurnal.idtransaksi=" & IDAssetD
        EksekusiSQL(strsql)

        strsql = "delete MJurnal from MJurnal " & _
                "where MJurnal.IDJenistransaksi=26 and MJurnal.idtransaksi=" & IDAssetD
        EksekusiSQL(strsql)
        EksekusiSQL("Update MAssetD set IsPost=0 where NoID=" & IDAssetD)


    End Function

    Private Sub SimpleButton3_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SimpleButton3.Click
        Try
            Dim view As ColumnView = GridControl1.FocusedView
            Dim focusrow As Long
            focusrow = GridView1.FocusedRowHandle
            'For I = 0 To view.SelectedRowsCount - 1
            Dim row As System.Data.DataRow = view.GetDataRow(view.GetSelectedRows(0))
            NoID = NullToLong(row("NoID"))
            If NullToBool(row("IsPost")) = True Then
                Dim x As New frmViewJurnal
                x.IDTransaksi = NoID
                x.IDTypeTransaksi = 26
                x.ShowDialog()
                x.Dispose()
            End If
            'Next
            RefreshData()
            GridView1.FocusedRowHandle = focusrow
        Catch ex As Exception
            XtraMessageBox.Show(Me, ex.Message, ".:: Pesan Kesalahan ::.", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub


    Private Sub txtTipe_ButtonClick(ByVal sender As Object, ByVal e As DevExpress.XtraEditors.Controls.ButtonPressedEventArgs) Handles txtTipe.ButtonClick
        Select Case e.Button.Index
            Case 0
                Dim x As New frmDaftarTypeAsset
                If x.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
                    IDTipeAsset = NullToLong(x.row("NoID"))
                    txtTipe.Text = NullToStr(x.row("Nama"))
                End If
            Case 1
                IDTipeAsset = 0
                txtTipe.Text = ""
        End Select
        RefreshData()
    End Sub


    Private Sub txtAsset_ButtonClick(ByVal sender As Object, ByVal e As DevExpress.XtraEditors.Controls.ButtonPressedEventArgs) Handles txtAsset.ButtonClick
        Select Case e.Button.Index
            Case 0
                Dim x As New frDaftarAktivaTetap
                x.IsLookup = True
                If x.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
                    IDAsset = NullToLong(x.row("NoID"))
                    txtAsset.Text = NullToStr(x.row("Nama"))
                End If
            Case 1
                IDAsset = 0
                txtAsset.Text = ""
        End Select
        RefreshData()
    End Sub

    Private Sub lbAsset_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lbAsset.Click
        txtAsset.Enabled = Not txtAsset.Enabled
    End Sub

    Private Sub lbTipe_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lbTipe.Click
        txtTipe.Enabled = Not txtTipe.Enabled
    End Sub
End Class

