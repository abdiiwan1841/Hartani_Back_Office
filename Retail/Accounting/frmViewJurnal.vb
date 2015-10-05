Imports System.Data.SqlClient
Imports System.Windows.Forms
Imports DevExpress.XtraEditors
Imports DevExpress.XtraGrid.Views.Base
Imports DevExpress.XtraGrid.Columns

Public Class frmViewJurnal

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
    Friend WithEvents GridControl1 As DevExpress.XtraGrid.GridControl
    Friend WithEvents GridView1 As DevExpress.XtraGrid.Views.Grid.GridView
    Friend WithEvents cIDJurnal As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colNama As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents cDebet As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents cKeterangan As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents cNoID As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents RepositoryItemCheckEdit1 As DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit
    Friend WithEvents Tgl As DevExpress.XtraEditors.DateEdit
    Friend WithEvents TglPosting As DevExpress.XtraEditors.DateEdit
    Friend WithEvents Label14 As System.Windows.Forms.Label
    Friend WithEvents c2NoID As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents c2Kode As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents c2Tanggal As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents c2Jumlah As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents cDebetA As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents cKredit As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents cKreditA As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents SaveFileDialog1 As System.Windows.Forms.SaveFileDialog
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents TextEdit4 As DevExpress.XtraEditors.TextEdit
    Friend WithEvents TextEdit2 As DevExpress.XtraEditors.TextEdit
    Friend WithEvents TextEdit1 As DevExpress.XtraEditors.TextEdit
    Friend WithEvents CheckEdit1 As DevExpress.XtraEditors.CheckEdit
    Friend WithEvents TextEdit3 As DevExpress.XtraEditors.MemoEdit
    Friend WithEvents cKode As DevExpress.XtraGrid.Columns.GridColumn
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmViewJurnal))
        Me.c2NoID = New DevExpress.XtraGrid.Columns.GridColumn
        Me.c2Kode = New DevExpress.XtraGrid.Columns.GridColumn
        Me.c2Tanggal = New DevExpress.XtraGrid.Columns.GridColumn
        Me.c2Jumlah = New DevExpress.XtraGrid.Columns.GridColumn
        Me.GridControl1 = New DevExpress.XtraGrid.GridControl
        Me.GridView1 = New DevExpress.XtraGrid.Views.Grid.GridView
        Me.cNoID = New DevExpress.XtraGrid.Columns.GridColumn
        Me.cIDJurnal = New DevExpress.XtraGrid.Columns.GridColumn
        Me.cKode = New DevExpress.XtraGrid.Columns.GridColumn
        Me.colNama = New DevExpress.XtraGrid.Columns.GridColumn
        Me.cKeterangan = New DevExpress.XtraGrid.Columns.GridColumn
        Me.cDebet = New DevExpress.XtraGrid.Columns.GridColumn
        Me.cKredit = New DevExpress.XtraGrid.Columns.GridColumn
        Me.cDebetA = New DevExpress.XtraGrid.Columns.GridColumn
        Me.cKreditA = New DevExpress.XtraGrid.Columns.GridColumn
        Me.RepositoryItemCheckEdit1 = New DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit
        Me.ImageList1 = New System.Windows.Forms.ImageList(Me.components)
        Me.PanelControl1 = New DevExpress.XtraEditors.PanelControl
        Me.TextEdit4 = New DevExpress.XtraEditors.TextEdit
        Me.TextEdit2 = New DevExpress.XtraEditors.TextEdit
        Me.TextEdit1 = New DevExpress.XtraEditors.TextEdit
        Me.CheckEdit1 = New DevExpress.XtraEditors.CheckEdit
        Me.Label5 = New System.Windows.Forms.Label
        Me.Label4 = New System.Windows.Forms.Label
        Me.Label1 = New System.Windows.Forms.Label
        Me.Label3 = New System.Windows.Forms.Label
        Me.Label2 = New System.Windows.Forms.Label
        Me.TglPosting = New DevExpress.XtraEditors.DateEdit
        Me.Label14 = New System.Windows.Forms.Label
        Me.Tgl = New DevExpress.XtraEditors.DateEdit
        Me.TextEdit3 = New DevExpress.XtraEditors.MemoEdit
        Me.PanelControl2 = New DevExpress.XtraEditors.PanelControl
        Me.PanelControl3 = New DevExpress.XtraEditors.PanelControl
        Me.SimpleButton7 = New DevExpress.XtraEditors.SimpleButton
        Me.SimpleButton6 = New DevExpress.XtraEditors.SimpleButton
        Me.SaveFileDialog1 = New System.Windows.Forms.SaveFileDialog
        CType(Me.GridControl1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.GridView1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RepositoryItemCheckEdit1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PanelControl1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.PanelControl1.SuspendLayout()
        CType(Me.TextEdit4.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.TextEdit2.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.TextEdit1.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.CheckEdit1.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.TglPosting.Properties.VistaTimeProperties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.TglPosting.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Tgl.Properties.VistaTimeProperties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Tgl.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.TextEdit3.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
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
        Me.GridControl1.Location = New System.Drawing.Point(2, 2)
        Me.GridControl1.MainView = Me.GridView1
        Me.GridControl1.Name = "GridControl1"
        Me.GridControl1.RepositoryItems.AddRange(New DevExpress.XtraEditors.Repository.RepositoryItem() {Me.RepositoryItemCheckEdit1})
        Me.GridControl1.Size = New System.Drawing.Size(952, 303)
        Me.GridControl1.TabIndex = 0
        Me.GridControl1.ViewCollection.AddRange(New DevExpress.XtraGrid.Views.Base.BaseView() {Me.GridView1})
        '
        'GridView1
        '
        Me.GridView1.Columns.AddRange(New DevExpress.XtraGrid.Columns.GridColumn() {Me.cNoID, Me.cIDJurnal, Me.cKode, Me.colNama, Me.cKeterangan, Me.cDebet, Me.cKredit, Me.cDebetA, Me.cKreditA})
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
        Me.GridView1.OptionsView.ShowGroupPanel = False
        '
        'cNoID
        '
        Me.cNoID.Caption = "NoID"
        Me.cNoID.FieldName = "ID"
        Me.cNoID.Name = "cNoID"
        Me.cNoID.Width = 36
        '
        'cIDJurnal
        '
        Me.cIDJurnal.Caption = "IDJurnal"
        Me.cIDJurnal.FieldName = "IDJurnal"
        Me.cIDJurnal.Name = "cIDJurnal"
        Me.cIDJurnal.Width = 79
        '
        'cKode
        '
        Me.cKode.Caption = "Kode Akun"
        Me.cKode.FieldName = "KodeAkun"
        Me.cKode.Name = "cKode"
        Me.cKode.Visible = True
        Me.cKode.VisibleIndex = 0
        Me.cKode.Width = 60
        '
        'colNama
        '
        Me.colNama.Caption = "Nama Akun"
        Me.colNama.FieldName = "NamaAkun"
        Me.colNama.Name = "colNama"
        Me.colNama.SummaryItem.DisplayFormat = "{0:###,###,###,##0}"
        Me.colNama.SummaryItem.FieldName = "Nama"
        Me.colNama.SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Count
        Me.colNama.Visible = True
        Me.colNama.VisibleIndex = 1
        Me.colNama.Width = 121
        '
        'cKeterangan
        '
        Me.cKeterangan.Caption = "Keterangan"
        Me.cKeterangan.FieldName = "Keterangan"
        Me.cKeterangan.Name = "cKeterangan"
        Me.cKeterangan.Visible = True
        Me.cKeterangan.VisibleIndex = 2
        Me.cKeterangan.Width = 224
        '
        'cDebet
        '
        Me.cDebet.AppearanceCell.Options.UseTextOptions = True
        Me.cDebet.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
        Me.cDebet.AppearanceHeader.Options.UseTextOptions = True
        Me.cDebet.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
        Me.cDebet.Caption = "Debet"
        Me.cDebet.DisplayFormat.FormatString = "###,###,###,##0.00"
        Me.cDebet.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
        Me.cDebet.FieldName = "Debet"
        Me.cDebet.Name = "cDebet"
        Me.cDebet.SummaryItem.DisplayFormat = "{0:###,###,###,##0.00}"
        Me.cDebet.SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum
        Me.cDebet.Visible = True
        Me.cDebet.VisibleIndex = 3
        Me.cDebet.Width = 117
        '
        'cKredit
        '
        Me.cKredit.Caption = "Kredit"
        Me.cKredit.DisplayFormat.FormatString = "###,##0.00"
        Me.cKredit.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
        Me.cKredit.FieldName = "Kredit"
        Me.cKredit.Name = "cKredit"
        Me.cKredit.SummaryItem.DisplayFormat = "{0:###,###,###,##0.00}"
        Me.cKredit.SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum
        Me.cKredit.Visible = True
        Me.cKredit.VisibleIndex = 5
        Me.cKredit.Width = 122
        '
        'cDebetA
        '
        Me.cDebetA.Caption = "DebetA"
        Me.cDebetA.DisplayFormat.FormatString = "###,###,###,##0.00"
        Me.cDebetA.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
        Me.cDebetA.FieldName = "DebetA"
        Me.cDebetA.Name = "cDebetA"
        Me.cDebetA.SummaryItem.DisplayFormat = "{0:###,###,###,##0.00}"
        Me.cDebetA.SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum
        Me.cDebetA.Visible = True
        Me.cDebetA.VisibleIndex = 4
        Me.cDebetA.Width = 147
        '
        'cKreditA
        '
        Me.cKreditA.Caption = "KreditA"
        Me.cKreditA.DisplayFormat.FormatString = "###,###,###,##0.00"
        Me.cKreditA.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
        Me.cKreditA.FieldName = "KreditA"
        Me.cKreditA.Name = "cKreditA"
        Me.cKreditA.SummaryItem.DisplayFormat = "{0:###,###,###,##0.00}"
        Me.cKreditA.SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum
        Me.cKreditA.Visible = True
        Me.cKreditA.VisibleIndex = 6
        Me.cKreditA.Width = 114
        '
        'RepositoryItemCheckEdit1
        '
        Me.RepositoryItemCheckEdit1.AutoHeight = False
        Me.RepositoryItemCheckEdit1.Name = "RepositoryItemCheckEdit1"
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
        Me.PanelControl1.Controls.Add(Me.TextEdit4)
        Me.PanelControl1.Controls.Add(Me.TextEdit2)
        Me.PanelControl1.Controls.Add(Me.TextEdit1)
        Me.PanelControl1.Controls.Add(Me.CheckEdit1)
        Me.PanelControl1.Controls.Add(Me.Label5)
        Me.PanelControl1.Controls.Add(Me.Label4)
        Me.PanelControl1.Controls.Add(Me.Label1)
        Me.PanelControl1.Controls.Add(Me.Label3)
        Me.PanelControl1.Controls.Add(Me.Label2)
        Me.PanelControl1.Controls.Add(Me.TglPosting)
        Me.PanelControl1.Controls.Add(Me.Label14)
        Me.PanelControl1.Controls.Add(Me.Tgl)
        Me.PanelControl1.Controls.Add(Me.TextEdit3)
        Me.PanelControl1.Dock = System.Windows.Forms.DockStyle.Top
        Me.PanelControl1.Location = New System.Drawing.Point(0, 0)
        Me.PanelControl1.LookAndFeel.SkinName = "Glass Oceans"
        Me.PanelControl1.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.UltraFlat
        Me.PanelControl1.Name = "PanelControl1"
        Me.PanelControl1.Size = New System.Drawing.Size(956, 126)
        Me.PanelControl1.TabIndex = 0
        '
        'TextEdit4
        '
        Me.TextEdit4.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TextEdit4.Location = New System.Drawing.Point(740, 71)
        Me.TextEdit4.Name = "TextEdit4"
        Me.TextEdit4.Properties.Appearance.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextEdit4.Properties.Appearance.Options.UseFont = True
        Me.TextEdit4.Properties.ReadOnly = True
        Me.TextEdit4.Size = New System.Drawing.Size(204, 22)
        Me.TextEdit4.TabIndex = 13
        '
        'TextEdit2
        '
        Me.TextEdit2.Location = New System.Drawing.Point(92, 43)
        Me.TextEdit2.Name = "TextEdit2"
        Me.TextEdit2.Properties.Appearance.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextEdit2.Properties.Appearance.Options.UseFont = True
        Me.TextEdit2.Properties.ReadOnly = True
        Me.TextEdit2.Size = New System.Drawing.Size(311, 22)
        Me.TextEdit2.TabIndex = 11
        '
        'TextEdit1
        '
        Me.TextEdit1.Location = New System.Drawing.Point(92, 15)
        Me.TextEdit1.Name = "TextEdit1"
        Me.TextEdit1.Properties.Appearance.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextEdit1.Properties.Appearance.Options.UseFont = True
        Me.TextEdit1.Properties.ReadOnly = True
        Me.TextEdit1.Size = New System.Drawing.Size(311, 22)
        Me.TextEdit1.TabIndex = 10
        '
        'CheckEdit1
        '
        Me.CheckEdit1.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.CheckEdit1.Location = New System.Drawing.Point(738, 99)
        Me.CheckEdit1.Name = "CheckEdit1"
        Me.CheckEdit1.Properties.Appearance.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CheckEdit1.Properties.Appearance.Options.UseFont = True
        Me.CheckEdit1.Properties.Caption = "Posting"
        Me.CheckEdit1.Properties.ReadOnly = True
        Me.CheckEdit1.Size = New System.Drawing.Size(206, 21)
        Me.CheckEdit1.TabIndex = 9
        '
        'Label5
        '
        Me.Label5.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label5.AutoSize = True
        Me.Label5.BackColor = System.Drawing.Color.Transparent
        Me.Label5.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.ForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.Label5.Location = New System.Drawing.Point(655, 74)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(79, 16)
        Me.Label5.TabIndex = 8
        Me.Label5.Text = "Posting Oleh"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.BackColor = System.Drawing.Color.Transparent
        Me.Label4.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.ForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.Label4.Location = New System.Drawing.Point(13, 73)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(73, 16)
        Me.Label4.TabIndex = 7
        Me.Label4.Text = "Keterangan"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.BackColor = System.Drawing.Color.Transparent
        Me.Label1.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.ForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.Label1.Location = New System.Drawing.Point(23, 46)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(63, 16)
        Me.Label1.TabIndex = 6
        Me.Label1.Text = "Kode Reff"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.BackColor = System.Drawing.Color.Transparent
        Me.Label3.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.ForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.Label3.Location = New System.Drawing.Point(50, 18)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(36, 16)
        Me.Label3.TabIndex = 5
        Me.Label3.Text = "Kode"
        '
        'Label2
        '
        Me.Label2.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label2.AutoSize = True
        Me.Label2.BackColor = System.Drawing.Color.Transparent
        Me.Label2.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.ForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.Label2.Location = New System.Drawing.Point(635, 46)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(99, 16)
        Me.Label2.TabIndex = 4
        Me.Label2.Text = "Tanggal Posting"
        '
        'TglPosting
        '
        Me.TglPosting.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TglPosting.EditValue = Nothing
        Me.TglPosting.EnterMoveNextControl = True
        Me.TglPosting.Location = New System.Drawing.Point(740, 43)
        Me.TglPosting.Name = "TglPosting"
        Me.TglPosting.Properties.Appearance.BackColor = System.Drawing.Color.White
        Me.TglPosting.Properties.Appearance.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TglPosting.Properties.Appearance.Options.UseBackColor = True
        Me.TglPosting.Properties.Appearance.Options.UseFont = True
        Me.TglPosting.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.TglPosting.Properties.VistaTimeProperties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton})
        Me.TglPosting.Size = New System.Drawing.Size(204, 22)
        Me.TglPosting.TabIndex = 3
        Me.TglPosting.TabStop = False
        '
        'Label14
        '
        Me.Label14.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label14.AutoSize = True
        Me.Label14.BackColor = System.Drawing.Color.Transparent
        Me.Label14.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label14.ForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.Label14.Location = New System.Drawing.Point(680, 18)
        Me.Label14.Name = "Label14"
        Me.Label14.Size = New System.Drawing.Size(54, 16)
        Me.Label14.TabIndex = 0
        Me.Label14.Text = "Tanggal"
        '
        'Tgl
        '
        Me.Tgl.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Tgl.EditValue = Nothing
        Me.Tgl.EnterMoveNextControl = True
        Me.Tgl.Location = New System.Drawing.Point(740, 15)
        Me.Tgl.Name = "Tgl"
        Me.Tgl.Properties.Appearance.BackColor = System.Drawing.Color.White
        Me.Tgl.Properties.Appearance.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Tgl.Properties.Appearance.Options.UseBackColor = True
        Me.Tgl.Properties.Appearance.Options.UseFont = True
        Me.Tgl.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.Tgl.Properties.ReadOnly = True
        Me.Tgl.Properties.VistaTimeProperties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton})
        Me.Tgl.Size = New System.Drawing.Size(204, 22)
        Me.Tgl.TabIndex = 1
        Me.Tgl.TabStop = False
        '
        'TextEdit3
        '
        Me.TextEdit3.Location = New System.Drawing.Point(92, 71)
        Me.TextEdit3.Name = "TextEdit3"
        Me.TextEdit3.Properties.Appearance.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextEdit3.Properties.Appearance.Options.UseFont = True
        Me.TextEdit3.Properties.ReadOnly = True
        Me.TextEdit3.Size = New System.Drawing.Size(311, 49)
        Me.TextEdit3.TabIndex = 12
        '
        'PanelControl2
        '
        Me.PanelControl2.Appearance.BackColor = System.Drawing.Color.Transparent
        Me.PanelControl2.Appearance.Options.UseBackColor = True
        Me.PanelControl2.Controls.Add(Me.GridControl1)
        Me.PanelControl2.Controls.Add(Me.PanelControl3)
        Me.PanelControl2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.PanelControl2.Location = New System.Drawing.Point(0, 126)
        Me.PanelControl2.LookAndFeel.SkinName = "Glass Oceans"
        Me.PanelControl2.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.UltraFlat
        Me.PanelControl2.Name = "PanelControl2"
        Me.PanelControl2.Size = New System.Drawing.Size(956, 352)
        Me.PanelControl2.TabIndex = 1
        '
        'PanelControl3
        '
        Me.PanelControl3.Appearance.BackColor = System.Drawing.Color.Transparent
        Me.PanelControl3.Appearance.Options.UseBackColor = True
        Me.PanelControl3.Controls.Add(Me.SimpleButton7)
        Me.PanelControl3.Controls.Add(Me.SimpleButton6)
        Me.PanelControl3.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.PanelControl3.Location = New System.Drawing.Point(2, 305)
        Me.PanelControl3.Name = "PanelControl3"
        Me.PanelControl3.Size = New System.Drawing.Size(952, 45)
        Me.PanelControl3.TabIndex = 1
        '
        'SimpleButton7
        '
        Me.SimpleButton7.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.SimpleButton7.Cursor = System.Windows.Forms.Cursors.Hand
        Me.SimpleButton7.ImageIndex = 2
        Me.SimpleButton7.ImageList = Me.ImageList1
        Me.SimpleButton7.Location = New System.Drawing.Point(843, 6)
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
        Me.SimpleButton6.Location = New System.Drawing.Point(738, 316)
        Me.SimpleButton6.Name = "SimpleButton6"
        Me.SimpleButton6.Size = New System.Drawing.Size(99, 34)
        Me.SimpleButton6.TabIndex = 0
        Me.SimpleButton6.Text = "&Refresh"
        '
        'frmViewJurnal
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 14)
        Me.ClientSize = New System.Drawing.Size(956, 478)
        Me.Controls.Add(Me.PanelControl2)
        Me.Controls.Add(Me.PanelControl1)
        Me.Name = "frmViewJurnal"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Jurnal"
        CType(Me.GridControl1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.GridView1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RepositoryItemCheckEdit1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PanelControl1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.PanelControl1.ResumeLayout(False)
        Me.PanelControl1.PerformLayout()
        CType(Me.TextEdit4.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.TextEdit2.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.TextEdit1.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.CheckEdit1.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.TglPosting.Properties.VistaTimeProperties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.TglPosting.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Tgl.Properties.VistaTimeProperties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Tgl.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.TextEdit3.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PanelControl2, System.ComponentModel.ISupportInitialize).EndInit()
        Me.PanelControl2.ResumeLayout(False)
        CType(Me.PanelControl3, System.ComponentModel.ISupportInitialize).EndInit()
        Me.PanelControl3.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub

#End Region
    Public IsGetOnly As Boolean
    Public NoID As Long
    Public IDTypeTransaksi As Long = 0
    Public IDTransaksi As Long = 0

    Dim BolehAmbilData As Boolean
    Sub RefreshData()
        Dim oConn As New SqlConnection
        Dim ocmd As SqlCommand
        Dim oDA As SqlDataAdapter
        Dim oDS As New DataSet
        Dim strsql As String
        Dim curentcursor As Cursor = Windows.Forms.Cursor.Current
        Windows.Forms.Cursor.Current = Cursors.WaitCursor
        'MAsset.NilaiDibukukan-MAssetD.AkumulasiSusut as NilaiBuku 
        strsql = "SELECT MJurnalD.*,MJurnal.Kode,MJurnal.KodeReff,Mjurnal.IsPosting,Mjurnal.Keterangan as Ket,Mjurnal.Tanggal,Mjurnal.TanggalEntry,Makun.Kode as KodeAkun,MAkun.Nama as NamaAkun, " & _
        "Muser.Nama as UserPosting,MJurnal.TanggalPosting " & _
        "from MJurnalD Inner Join Mjurnal on MJurnalD.IDJurnal=MJurnal.ID " & _
        "Left Join Makun On Mjurnald.IDAkun=Makun.ID " & _
        "left Join Muser on Mjurnal.IDUserPosting=Muser.NoID"
        Select Case IDTypeTransaksi
            Case 2, 3
                strsql &= " where Mjurnal.IDJenistransaksi= " & IDTypeTransaksi & " and MJurnal.ID=" & IDTransaksi
            Case Else
                strsql &= " where Mjurnal.IDJenistransaksi= " & IDTypeTransaksi & " and MJurnal.IDTransaksi=" & IDTransaksi
        End Select
        Try
            oConn.ConnectionString = StrKonSql
            ocmd = New SqlCommand(strsql, oConn)
            oConn.Open()
            oDA = New SqlDataAdapter(ocmd)
            If oDS.Tables("MJurnal") Is Nothing Then
            Else
                oDS.Tables("MJurnal").Clear()
            End If
            oDA.Fill(oDS, "MJurnal")
            If oDS.Tables("MJurnal").Rows.Count >= 1 Then
                TextEdit1.EditValue = oDS.Tables("Mjurnal").Rows(0).Item("Kode")
                TextEdit2.EditValue = oDS.Tables("Mjurnal").Rows(0).Item("KodeReff")
                TextEdit3.EditValue = oDS.Tables("Mjurnal").Rows(0).Item("Ket")
                TextEdit4.EditValue = oDS.Tables("Mjurnal").Rows(0).Item("UserPosting")
                Tgl.EditValue = oDS.Tables("Mjurnal").Rows(0).Item("Tanggal")
                TglPosting.EditValue = oDS.Tables("Mjurnal").Rows(0).Item("TanggalPosting")
                CheckEdit1.EditValue = oDS.Tables("Mjurnal").Rows(0).Item("IsPosting")
            End If
        Catch ex As Exception
            XtraMessageBox.Show(Me, ex.Message, ".:: Pesan Kesalahan ::.", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            oConn.Close()
        End Try
        GridControl1.DataSource = oDS.Tables("MJurnal")
        Windows.Forms.Cursor.Current = curentcursor
    End Sub

    Private Sub frmViewJurnal_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Activated
        'GridToPrint = GridControl1
    End Sub

    Private Sub frmViewJurnal_Deactivate(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Deactivate
        'GridToPrint = Nothing
    End Sub

    Private Sub frDaftarPinjaman_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        GridView1.SaveLayoutToXml(Application.StartupPath & "\system\layouts\" & Me.Name & GridView1.Name & ".xml")
        'GridView2.SaveLayoutToXml(Application.StartupPath & "\layout\" & Me.Name & GridView2.Name & ".xml")
    End Sub
    Private Sub frDaftarBBM_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        If IsGetOnly Then PanelControl3.Visible = False
        Tgl.DateTime = Date.Today
        TglPosting.DateTime = Date.Today
        RefreshData()
        If Dir(Application.StartupPath & "\system\layouts\" & Me.Name & GridView1.Name & ".xml") <> "" Then
            GridView1.RestoreLayoutFromXml(Application.StartupPath & "\system\layouts\" & Me.Name & GridView1.Name & ".xml")
        End If
        'Setme()
    End Sub

    'Private Sub Setme()
    '    SetButton(cmdPosting, button_.cmdPosting)
    '    SetButton(cmdUnposting, button_.cmdUnposting)
    'End Sub

    Private Sub SimpleButton4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        DialogResult = Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub SimpleButton1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Try
            'GridView1.OptionsPrint.SizeMode = DevExpress.XtraCharts.Printing.PrintSizeMode.Zoom
            GridControl1.ShowPrintPreview()
        Catch
        End Try
    End Sub

    Private Sub SimpleButton2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Try
            Dim X As String
            SaveFileDialog1.Filter = "*.xls|Excel Files"
            If SaveFileDialog1.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
                X = SaveFileDialog1.FileName
                'GridControl1.OptionsPrint.SizeMode = DevExpress.XtraCharts.Printing.PrintSizeMode.Zoom
                GridControl1.ExportToXls(X)
            End If
        Catch
        End Try
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
        DialogResult = Windows.Forms.DialogResult.OK
    End Sub

    Private Sub SimpleButton7_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SimpleButton7.Click
        DialogResult = Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub

    'Private Sub GridControl1_DoubleClick1(ByVal sender As Object, ByVal e As System.EventArgs) Handles GridControl1.DoubleClick
    '    If BolehAmbilData Then
    '        SimpleButton2.PerformClick()
    '    End If
    'End Sub

    Private Sub GridControl1_KeyDown1(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles GridControl1.KeyDown
        If e.KeyCode = Keys.Enter Then
            'SimpleButton2.PerformClick()
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
        Tgl.Enabled = Not Tgl.Enabled
        TglPosting.Enabled = Tgl.Enabled
    End Sub

End Class

