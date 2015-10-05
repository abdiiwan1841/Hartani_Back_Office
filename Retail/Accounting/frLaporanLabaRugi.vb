Imports System.Data.SqlClient
Imports System.Windows.Forms
Imports DevExpress.XtraEditors
Imports DevExpress.XtraGrid.Views.Base
Imports DevExpress.XtraGrid.Columns
Imports VPoint.mdlCetakCR
Public Class frLaporanLabaRugi
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
    Friend WithEvents TglDari As DevExpress.XtraEditors.DateEdit
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents TglSampai As DevExpress.XtraEditors.DateEdit
    Friend WithEvents Label14 As System.Windows.Forms.Label
    Friend WithEvents c2NoID As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents c2Kode As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents c2Tanggal As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents c2Jumlah As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents SaveFileDialog1 As System.Windows.Forms.SaveFileDialog
    Friend WithEvents PanelControl4 As DevExpress.XtraEditors.PanelControl
    Friend WithEvents PanelControl2 As DevExpress.XtraEditors.PanelControl
    Friend WithEvents GridControl1 As DevExpress.XtraGrid.GridControl
    Friend WithEvents GridView1 As DevExpress.XtraGrid.Views.Grid.GridView
    Friend WithEvents RepositoryItemCheckEdit1 As DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit
    Friend WithEvents PanelControl3 As DevExpress.XtraEditors.PanelControl
    Friend WithEvents SimpleButton3 As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents SimpleButton7 As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents SimpleButton6 As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents SimpleButton2 As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents SimpleButton1 As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents cNoID As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents cKode As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents cKodeReff As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents cKeterangan As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents cDebet As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents cKredit As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents cIsLock As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents cKodeAkun As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents cNamaAkun As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents cTanggal As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents SimpleButton5 As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents SimpleButton4 As DevExpress.XtraEditors.SimpleButton
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frLaporanLabaRugi))
        Dim GridLevelNode1 As DevExpress.XtraGrid.GridLevelNode = New DevExpress.XtraGrid.GridLevelNode
        Me.c2NoID = New DevExpress.XtraGrid.Columns.GridColumn
        Me.c2Kode = New DevExpress.XtraGrid.Columns.GridColumn
        Me.c2Tanggal = New DevExpress.XtraGrid.Columns.GridColumn
        Me.c2Jumlah = New DevExpress.XtraGrid.Columns.GridColumn
        Me.ImageList1 = New System.Windows.Forms.ImageList(Me.components)
        Me.PanelControl1 = New DevExpress.XtraEditors.PanelControl
        Me.SimpleButton5 = New DevExpress.XtraEditors.SimpleButton
        Me.Label1 = New System.Windows.Forms.Label
        Me.SimpleButton4 = New DevExpress.XtraEditors.SimpleButton
        Me.TglSampai = New DevExpress.XtraEditors.DateEdit
        Me.Label14 = New System.Windows.Forms.Label
        Me.TglDari = New DevExpress.XtraEditors.DateEdit
        Me.SaveFileDialog1 = New System.Windows.Forms.SaveFileDialog
        Me.PanelControl4 = New DevExpress.XtraEditors.PanelControl
        Me.PanelControl2 = New DevExpress.XtraEditors.PanelControl
        Me.GridControl1 = New DevExpress.XtraGrid.GridControl
        Me.GridView1 = New DevExpress.XtraGrid.Views.Grid.GridView
        Me.cNoID = New DevExpress.XtraGrid.Columns.GridColumn
        Me.cTanggal = New DevExpress.XtraGrid.Columns.GridColumn
        Me.cKode = New DevExpress.XtraGrid.Columns.GridColumn
        Me.cKodeReff = New DevExpress.XtraGrid.Columns.GridColumn
        Me.cKeterangan = New DevExpress.XtraGrid.Columns.GridColumn
        Me.cDebet = New DevExpress.XtraGrid.Columns.GridColumn
        Me.cKredit = New DevExpress.XtraGrid.Columns.GridColumn
        Me.cIsLock = New DevExpress.XtraGrid.Columns.GridColumn
        Me.RepositoryItemCheckEdit1 = New DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit
        Me.cKodeAkun = New DevExpress.XtraGrid.Columns.GridColumn
        Me.cNamaAkun = New DevExpress.XtraGrid.Columns.GridColumn
        Me.PanelControl3 = New DevExpress.XtraEditors.PanelControl
        Me.SimpleButton3 = New DevExpress.XtraEditors.SimpleButton
        Me.SimpleButton7 = New DevExpress.XtraEditors.SimpleButton
        Me.SimpleButton6 = New DevExpress.XtraEditors.SimpleButton
        Me.SimpleButton2 = New DevExpress.XtraEditors.SimpleButton
        Me.SimpleButton1 = New DevExpress.XtraEditors.SimpleButton
        CType(Me.PanelControl1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.PanelControl1.SuspendLayout()
        CType(Me.TglSampai.Properties.VistaTimeProperties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.TglSampai.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.TglDari.Properties.VistaTimeProperties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.TglDari.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PanelControl4, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PanelControl2, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.PanelControl2.SuspendLayout()
        CType(Me.GridControl1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.GridView1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RepositoryItemCheckEdit1, System.ComponentModel.ISupportInitialize).BeginInit()
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
        Me.PanelControl1.Controls.Add(Me.SimpleButton5)
        Me.PanelControl1.Controls.Add(Me.Label1)
        Me.PanelControl1.Controls.Add(Me.SimpleButton4)
        Me.PanelControl1.Controls.Add(Me.TglSampai)
        Me.PanelControl1.Controls.Add(Me.Label14)
        Me.PanelControl1.Controls.Add(Me.TglDari)
        Me.PanelControl1.Dock = System.Windows.Forms.DockStyle.Top
        Me.PanelControl1.Location = New System.Drawing.Point(0, 0)
        Me.PanelControl1.LookAndFeel.SkinName = "Glass Oceans"
        Me.PanelControl1.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.UltraFlat
        Me.PanelControl1.Name = "PanelControl1"
        Me.PanelControl1.Size = New System.Drawing.Size(1011, 53)
        Me.PanelControl1.TabIndex = 0
        '
        'SimpleButton5
        '
        Me.SimpleButton5.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.SimpleButton5.Cursor = System.Windows.Forms.Cursors.Hand
        Me.SimpleButton5.ImageIndex = 0
        Me.SimpleButton5.ImageList = Me.ImageList1
        Me.SimpleButton5.Location = New System.Drawing.Point(607, 5)
        Me.SimpleButton5.Name = "SimpleButton5"
        Me.SimpleButton5.Size = New System.Drawing.Size(153, 34)
        Me.SimpleButton5.TabIndex = 10
        Me.SimpleButton5.Text = "&Dg Perbandingan"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.BackColor = System.Drawing.Color.Transparent
        Me.Label1.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.ForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.Label1.Location = New System.Drawing.Point(306, 15)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(26, 16)
        Me.Label1.TabIndex = 2
        Me.Label1.Text = "s/d"
        '
        'SimpleButton4
        '
        Me.SimpleButton4.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.SimpleButton4.Cursor = System.Windows.Forms.Cursors.Hand
        Me.SimpleButton4.ImageIndex = 0
        Me.SimpleButton4.ImageList = Me.ImageList1
        Me.SimpleButton4.Location = New System.Drawing.Point(485, 5)
        Me.SimpleButton4.Name = "SimpleButton4"
        Me.SimpleButton4.Size = New System.Drawing.Size(116, 34)
        Me.SimpleButton4.TabIndex = 9
        Me.SimpleButton4.Text = "&Print Preview"
        '
        'TglSampai
        '
        Me.TglSampai.EditValue = Nothing
        Me.TglSampai.EnterMoveNextControl = True
        Me.TglSampai.Location = New System.Drawing.Point(338, 12)
        Me.TglSampai.Name = "TglSampai"
        Me.TglSampai.Properties.Appearance.BackColor = System.Drawing.Color.White
        Me.TglSampai.Properties.Appearance.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TglSampai.Properties.Appearance.Options.UseBackColor = True
        Me.TglSampai.Properties.Appearance.Options.UseFont = True
        Me.TglSampai.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.TglSampai.Properties.Mask.EditMask = "dd-MM-yyyy"
        Me.TglSampai.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.DateTimeAdvancingCaret
        Me.TglSampai.Properties.Mask.UseMaskAsDisplayFormat = True
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
        Me.Label14.Location = New System.Drawing.Point(101, 14)
        Me.Label14.Name = "Label14"
        Me.Label14.Size = New System.Drawing.Size(51, 16)
        Me.Label14.TabIndex = 0
        Me.Label14.Text = "Periode"
        '
        'TglDari
        '
        Me.TglDari.EditValue = Nothing
        Me.TglDari.EnterMoveNextControl = True
        Me.TglDari.Location = New System.Drawing.Point(158, 12)
        Me.TglDari.Name = "TglDari"
        Me.TglDari.Properties.Appearance.BackColor = System.Drawing.Color.White
        Me.TglDari.Properties.Appearance.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TglDari.Properties.Appearance.Options.UseBackColor = True
        Me.TglDari.Properties.Appearance.Options.UseFont = True
        Me.TglDari.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.TglDari.Properties.Mask.EditMask = "dd-MM-yyyy"
        Me.TglDari.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.DateTimeAdvancingCaret
        Me.TglDari.Properties.Mask.UseMaskAsDisplayFormat = True
        Me.TglDari.Properties.VistaTimeProperties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton})
        Me.TglDari.Size = New System.Drawing.Size(141, 22)
        Me.TglDari.TabIndex = 1
        Me.TglDari.TabStop = False
        '
        'PanelControl4
        '
        Me.PanelControl4.Appearance.BackColor = System.Drawing.Color.Transparent
        Me.PanelControl4.Appearance.Options.UseBackColor = True
        Me.PanelControl4.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.PanelControl4.Location = New System.Drawing.Point(0, 433)
        Me.PanelControl4.LookAndFeel.SkinName = "Glass Oceans"
        Me.PanelControl4.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.UltraFlat
        Me.PanelControl4.Name = "PanelControl4"
        Me.PanelControl4.Size = New System.Drawing.Size(1011, 45)
        Me.PanelControl4.TabIndex = 2
        Me.PanelControl4.Visible = False
        '
        'PanelControl2
        '
        Me.PanelControl2.Appearance.BackColor = System.Drawing.Color.Transparent
        Me.PanelControl2.Appearance.Options.UseBackColor = True
        Me.PanelControl2.Controls.Add(Me.GridControl1)
        Me.PanelControl2.Controls.Add(Me.PanelControl3)
        Me.PanelControl2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.PanelControl2.Location = New System.Drawing.Point(0, 53)
        Me.PanelControl2.LookAndFeel.SkinName = "Glass Oceans"
        Me.PanelControl2.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.UltraFlat
        Me.PanelControl2.Name = "PanelControl2"
        Me.PanelControl2.Size = New System.Drawing.Size(1011, 380)
        Me.PanelControl2.TabIndex = 3
        Me.PanelControl2.Visible = False
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
        Me.GridControl1.Size = New System.Drawing.Size(904, 376)
        Me.GridControl1.TabIndex = 0
        Me.GridControl1.ViewCollection.AddRange(New DevExpress.XtraGrid.Views.Base.BaseView() {Me.GridView1})
        Me.GridControl1.Visible = False
        '
        'GridView1
        '
        Me.GridView1.Columns.AddRange(New DevExpress.XtraGrid.Columns.GridColumn() {Me.cNoID, Me.cTanggal, Me.cKode, Me.cKodeReff, Me.cKeterangan, Me.cDebet, Me.cKredit, Me.cIsLock, Me.cKodeAkun, Me.cNamaAkun})
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
        Me.cNoID.Visible = True
        Me.cNoID.VisibleIndex = 7
        '
        'cTanggal
        '
        Me.cTanggal.Caption = "Tanggal"
        Me.cTanggal.DisplayFormat.FormatString = "dd-MM-yyyy"
        Me.cTanggal.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime
        Me.cTanggal.FieldName = "Tanggal"
        Me.cTanggal.Name = "cTanggal"
        Me.cTanggal.Visible = True
        Me.cTanggal.VisibleIndex = 1
        '
        'cKode
        '
        Me.cKode.Caption = "Kode"
        Me.cKode.FieldName = "Kode"
        Me.cKode.Name = "cKode"
        Me.cKode.Visible = True
        Me.cKode.VisibleIndex = 0
        '
        'cKodeReff
        '
        Me.cKodeReff.Caption = "KodeReff"
        Me.cKodeReff.FieldName = "KodeReff"
        Me.cKodeReff.Name = "cKodeReff"
        Me.cKodeReff.Visible = True
        Me.cKodeReff.VisibleIndex = 2
        '
        'cKeterangan
        '
        Me.cKeterangan.Caption = "Keterangan"
        Me.cKeterangan.FieldName = "Keterangan"
        Me.cKeterangan.Name = "cKeterangan"
        Me.cKeterangan.Visible = True
        Me.cKeterangan.VisibleIndex = 3
        '
        'cDebet
        '
        Me.cDebet.Caption = "Debet"
        Me.cDebet.DisplayFormat.FormatString = "###,##0.00"
        Me.cDebet.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
        Me.cDebet.FieldName = "Debet"
        Me.cDebet.Name = "cDebet"
        Me.cDebet.SummaryItem.DisplayFormat = "{0:###,##0.00}"
        Me.cDebet.SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum
        Me.cDebet.Visible = True
        Me.cDebet.VisibleIndex = 4
        '
        'cKredit
        '
        Me.cKredit.Caption = "Kredit"
        Me.cKredit.DisplayFormat.FormatString = "###,##0.00"
        Me.cKredit.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
        Me.cKredit.FieldName = "Kredit"
        Me.cKredit.Name = "cKredit"
        Me.cKredit.SummaryItem.DisplayFormat = "{0:###,##0.00}"
        Me.cKredit.SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum
        Me.cKredit.Visible = True
        Me.cKredit.VisibleIndex = 5
        '
        'cIsLock
        '
        Me.cIsLock.Caption = "IsLock"
        Me.cIsLock.ColumnEdit = Me.RepositoryItemCheckEdit1
        Me.cIsLock.FieldName = "IsLock"
        Me.cIsLock.Name = "cIsLock"
        Me.cIsLock.Visible = True
        Me.cIsLock.VisibleIndex = 6
        '
        'RepositoryItemCheckEdit1
        '
        Me.RepositoryItemCheckEdit1.AutoHeight = False
        Me.RepositoryItemCheckEdit1.Name = "RepositoryItemCheckEdit1"
        '
        'cKodeAkun
        '
        Me.cKodeAkun.Caption = "KodeAkun"
        Me.cKodeAkun.FieldName = "KodeAkun"
        Me.cKodeAkun.Name = "cKodeAkun"
        Me.cKodeAkun.Visible = True
        Me.cKodeAkun.VisibleIndex = 8
        '
        'cNamaAkun
        '
        Me.cNamaAkun.Caption = "NamaAkun"
        Me.cNamaAkun.FieldName = "Nama"
        Me.cNamaAkun.Name = "cNamaAkun"
        Me.cNamaAkun.Visible = True
        Me.cNamaAkun.VisibleIndex = 9
        '
        'PanelControl3
        '
        Me.PanelControl3.Appearance.BackColor = System.Drawing.Color.LightSalmon
        Me.PanelControl3.Appearance.Options.UseBackColor = True
        Me.PanelControl3.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder
        Me.PanelControl3.Controls.Add(Me.SimpleButton3)
        Me.PanelControl3.Controls.Add(Me.SimpleButton7)
        Me.PanelControl3.Controls.Add(Me.SimpleButton6)
        Me.PanelControl3.Controls.Add(Me.SimpleButton2)
        Me.PanelControl3.Controls.Add(Me.SimpleButton1)
        Me.PanelControl3.Dock = System.Windows.Forms.DockStyle.Left
        Me.PanelControl3.Location = New System.Drawing.Point(2, 2)
        Me.PanelControl3.Name = "PanelControl3"
        Me.PanelControl3.Size = New System.Drawing.Size(103, 376)
        Me.PanelControl3.TabIndex = 1
        Me.PanelControl3.Visible = False
        '
        'SimpleButton3
        '
        Me.SimpleButton3.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.SimpleButton3.Cursor = System.Windows.Forms.Cursors.Hand
        Me.SimpleButton3.Location = New System.Drawing.Point(3, 43)
        Me.SimpleButton3.Name = "SimpleButton3"
        Me.SimpleButton3.Size = New System.Drawing.Size(99, 34)
        Me.SimpleButton3.TabIndex = 8
        Me.SimpleButton3.Text = "&View Buku Besar"
        Me.SimpleButton3.Visible = False
        '
        'SimpleButton7
        '
        Me.SimpleButton7.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.SimpleButton7.Cursor = System.Windows.Forms.Cursors.Hand
        Me.SimpleButton7.ImageIndex = 2
        Me.SimpleButton7.ImageList = Me.ImageList1
        Me.SimpleButton7.Location = New System.Drawing.Point(1, 331)
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
        Me.SimpleButton6.Visible = False
        '
        'SimpleButton2
        '
        Me.SimpleButton2.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.SimpleButton2.Cursor = System.Windows.Forms.Cursors.Hand
        Me.SimpleButton2.ImageIndex = 0
        Me.SimpleButton2.ImageList = Me.ImageList1
        Me.SimpleButton2.Location = New System.Drawing.Point(3, 123)
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
        Me.SimpleButton1.Location = New System.Drawing.Point(3, 83)
        Me.SimpleButton1.Name = "SimpleButton1"
        Me.SimpleButton1.Size = New System.Drawing.Size(99, 34)
        Me.SimpleButton1.TabIndex = 1
        Me.SimpleButton1.Text = "&Preview"
        Me.SimpleButton1.Visible = False
        '
        'frLaporanLabaRugi
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 14)
        Me.ClientSize = New System.Drawing.Size(1011, 478)
        Me.Controls.Add(Me.PanelControl2)
        Me.Controls.Add(Me.PanelControl4)
        Me.Controls.Add(Me.PanelControl1)
        Me.Name = "frLaporanLabaRugi"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Laporan Laba Rugi"
        CType(Me.PanelControl1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.PanelControl1.ResumeLayout(False)
        Me.PanelControl1.PerformLayout()
        CType(Me.TglSampai.Properties.VistaTimeProperties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.TglSampai.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.TglDari.Properties.VistaTimeProperties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.TglDari.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PanelControl4, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PanelControl2, System.ComponentModel.ISupportInitialize).EndInit()
        Me.PanelControl2.ResumeLayout(False)
        CType(Me.GridControl1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.GridView1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RepositoryItemCheckEdit1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PanelControl3, System.ComponentModel.ISupportInitialize).EndInit()
        Me.PanelControl3.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub

#End Region
    Public IsGetOnly As Boolean
    Public NoID As Long
    Public Kode As String
    Dim BolehAmbilData As Boolean
    Public IDAkun As Long
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
        strsql = "SELECT MAkun.Kode KodeAkun,MAkun.Nama,MJurnal.IDTransaksi IDTran,MJurnal.IDJenisTransaksi,MJurnal.Tanggal,MJurnal.Kode,MJurnal.KodeReff, MJurnalD.* " & _
        "from MJurnalD inner Join MJurnal on MJurnald.IDJurnal=MJurnal.ID " & _
        "Inner Join MAkun On MJurnalD.IDAkun=Makun.ID where MJurnal.IDJenisTransaksi<>0"
        IsPerluwhere = False
        If TglDari.Enabled Then
            If IsPerluwhere Then
                strsql = strsql & " where "
            Else
                strsql = strsql & " and "
            End If
            strsql = strsql & "MJurnal.Tanggal>=convert(datetime,'" & Format(TglDari.DateTime, "MM/dd/yyyy") & "',101) " & _
            "AND MJurnal.Tanggal<=convert(datetime,'" & Format(TglSampai.DateTime, "yyyy/MM/dd") & "',101)"
            IsPerluwhere = False
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


    Private Sub frDaftarPinjaman_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        GridView1.SaveLayoutToXml(Application.StartupPath & "\system\layouts\" & Me.Name & GridView1.Name & ".xml")
        'GridView2.SaveLayoutToXml(Application.StartupPath & "\layout\" & Me.Name & GridView2.Name & ".xml")
    End Sub
    Private Sub frDaftarBBM_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        If IsGetOnly Then PanelControl3.Visible = False
        TglDari.DateTime = CDate(TanggalSystem.ToString("yyyy,MM,01"))
        TglSampai.DateTime = CDate(TanggalSystem.ToString("yyyy,MM,01")).AddMonths(1).AddDays(-1)
        RefreshData()
        If Dir(Application.StartupPath & "\system\layouts\" & Me.Name & GridView1.Name & ".xml") <> "" Then
            GridView1.RestoreLayoutFromXml(Application.StartupPath & "\system\layouts\" & Me.Name & GridView1.Name & ".xml")
        End If
    End Sub





    Private Sub SimpleButton6_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
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


    Private Sub GridControl1_DoubleClick1(ByVal sender As Object, ByVal e As System.EventArgs)
        If BolehAmbilData Then
            SimpleButton2.PerformClick()
        End If
    End Sub

    Private Sub GridControl1_KeyDown1(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs)
        If e.KeyCode = Keys.Enter Then
            SimpleButton2.PerformClick()
        ElseIf e.KeyCode = Keys.Escape Then
            DialogResult = Windows.Forms.DialogResult.Cancel
            Me.Close()
        End If
    End Sub

    Private Sub GridControl1_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs)
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

    Private Sub SimpleButton3_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs)

    End Sub

       
    Private Sub SimpleButton6_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SimpleButton6.Click
        RefreshData()
    End Sub

    Private Sub SimpleButton3_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles SimpleButton3.Click
        Try
            Dim view As ColumnView = GridControl1.FocusedView
            Dim focusrow As Long
            focusrow = GridView1.FocusedRowHandle
            'For I = 0 To view.SelectedRowsCount - 1
            Dim row As System.Data.DataRow = view.GetDataRow(view.GetSelectedRows(0))
            NoID = NullToLong(row("IDTran"))
            Dim IDTypeTransaksi As Integer = NullToLong(row("IDJenisTransaksi"))
            ' If NullToBool(row("IsPost")) = True Then
            Dim x As New frmViewJurnal
            x.IDTransaksi = NoID
            x.IDTypeTransaksi = IDTypeTransaksi
            x.ShowDialog()
            x.Dispose()
            'End If
            'Next
            RefreshData()
            GridView1.FocusedRowHandle = focusrow
        Catch ex As Exception
            XtraMessageBox.Show(Me, ex.Message, ".:: Pesan Kesalahan ::.", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub SimpleButton7_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SimpleButton7.Click
        DialogResult = Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub

    Private Sub SimpleButton1_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SimpleButton1.Click
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

    Private Sub SimpleButton4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SimpleButton4.Click
        CetakFaktur(action_.Preview, "LabaRugi")
    End Sub

    Private Sub CetakFaktur(ByVal action As action_, ByVal NamaReport As String)
        Dim namafile As String

        Try
            namafile = Application.StartupPath & "\report\" & NamaReport & ".rpt"
            If System.IO.File.Exists(namafile) Then
                If EditReport Then
                    action = action_.Edit
                Else
                    action = action_.Preview
                End If

                ViewReport(Me.ParentForm, action, namafile, Me.Text, , , "DariTanggal=cdate(" & Format(TglDari.DateTime, "yyyy,MM,dd") & ")&" & "SampaiTanggal=cdate(" & Format(TglSampai.DateTime, "yyyy,MM,dd") & ")")
            Else
                DevExpress.XtraEditors.XtraMessageBox.Show("Nama File :" & namafile & " tidak ditemukan.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Stop)
            End If
        Catch EX As Exception
            XtraMessageBox.Show("Pesan kesalahan :" & EX.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
            'DevExpress.XtraEditors.XtraMessageBox.Show("Kesalahan:" & EX.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
        End Try
    End Sub

    Private Sub BuatView()
        'EksekusiSQL("drop view vSaldoTransaksiAkun")
        'Application.DoEvents()
        'EksekusiSQL("CREATE VIEW [dbo].[vSaldoTransaksiAkun] AS SELECT dbo.MAkun.ID, SUM(dbo.MJurnalD.DebetA) AS JumDebet, SUM(dbo.MJurnalD.KreditA) AS JumKredit FROM dbo.MJurnalD INNER JOIN dbo.MJurnal ON dbo.MJurnalD.IDJurnal = dbo.MJurnal.ID RIGHT OUTER JOIN dbo.MAkun ON dbo.MJurnalD.IDAkun = dbo.MAkun.ID Where (dbo.MJurnal.IDJenistransaksi <>0) GROUP BY dbo.MAkun.ID,dbo.MjurnalD.IDDepartemen, YEAR(dbo.MJurnal.Tanggal), MONTH(dbo.MJurnal.Tanggal), DAY(dbo.MJurnal.Tanggal) Having (Year(dbo.MJurnal.Tanggal) = " & TglDari.DateTime.Year & ") And (Month(dbo.MJurnal.Tanggal) = " & TglDari.DateTime.Month & ") And (Day(dbo.MJurnal.Tanggal) = " & TglDari.DateTime.Day & ") ")
        'Application.DoEvents()
        EksekusiSQL("drop view vSaldoAwalAkun")
        Application.DoEvents()
        EksekusiSQL("CREATE VIEW [dbo].[vSaldoAwalAkun] AS SELECT     dbo.MAkun.ID, SUM(dbo.MJurnalD.DebetA) AS SADebet, SUM(dbo.MJurnalD.KreditA) AS SAKredit FROM         dbo.MJurnalD INNER JOIN dbo.MJurnal ON dbo.MJurnalD.IDJurnal = dbo.MJurnal.ID RIGHT OUTER JOIN dbo.MAkun ON dbo.MJurnalD.IDAkun = dbo.MAkun.ID Where (dbo.MJurnal.IDJenistransaksi = 0) GROUP BY dbo.MAkun.ID,dbo.MjurnalD.IDDepartemen, YEAR(dbo.MJurnal.Tanggal), MONTH(dbo.MJurnal.Tanggal) Having (Year(dbo.MJurnal.Tanggal) = " & TglDari.DateTime.Year & ") And (Month(dbo.MJurnal.Tanggal) = " & TglDari.DateTime.Month & ") ")
    End Sub

    Private Sub SimpleButton5_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SimpleButton5.Click
        BuatView()
        CetakFaktur(action_.Preview, "LabaRugiPerbandingan")
    End Sub
End Class

