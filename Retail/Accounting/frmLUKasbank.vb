 
Imports DevExpress.XtraGrid.Views.Base 
Public Class frmLUKas
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
    Friend WithEvents PanelControl4 As DevExpress.XtraEditors.PanelControl
    Friend WithEvents SimpleButton1 As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents GridControl1 As DevExpress.XtraGrid.GridControl
    Friend WithEvents GridView1 As DevExpress.XtraGrid.Views.Grid.GridView
    Friend WithEvents ColNoID As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colKode As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colNama As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colAlamat As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colAktif As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents RepositoryItemCheckEdit2 As DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit
    Friend WithEvents RepositoryItemLookUpEdit2 As DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit
    Friend WithEvents colKota As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colTelpon As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colKeterangan As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents coGudang As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colAkun As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colWilayah As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colCabang As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents txtGudang As DevExpress.XtraEditors.ButtonEdit
    Friend WithEvents LabelControl2 As DevExpress.XtraEditors.LabelControl
    Friend WithEvents colMataUang As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colKodeMataUang As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents CheckEdit1 As DevExpress.XtraEditors.CheckEdit
    Friend WithEvents cNoRekening As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents cAkunIntransit As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents cIsKas As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents cIsBank As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents Label2 As System.Windows.Forms.Label
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Dim SerializableAppearanceObject1 As DevExpress.Utils.SerializableAppearanceObject = New DevExpress.Utils.SerializableAppearanceObject
        Dim SerializableAppearanceObject2 As DevExpress.Utils.SerializableAppearanceObject = New DevExpress.Utils.SerializableAppearanceObject
        Me.PanelControl1 = New DevExpress.XtraEditors.PanelControl
        Me.CheckEdit1 = New DevExpress.XtraEditors.CheckEdit
        Me.txtGudang = New DevExpress.XtraEditors.ButtonEdit
        Me.LabelControl2 = New DevExpress.XtraEditors.LabelControl
        Me.Label2 = New System.Windows.Forms.Label
        Me.PanelControl2 = New DevExpress.XtraEditors.PanelControl
        Me.cmClose = New DevExpress.XtraEditors.SimpleButton
        Me.cmdBaru = New DevExpress.XtraEditors.SimpleButton
        Me.cmdRefresh = New DevExpress.XtraEditors.SimpleButton
        Me.cmdEdit = New DevExpress.XtraEditors.SimpleButton
        Me.cmdHapus = New DevExpress.XtraEditors.SimpleButton
        Me.PanelControl3 = New DevExpress.XtraEditors.PanelControl
        Me.SimpleButton1 = New DevExpress.XtraEditors.SimpleButton
        Me.cmdCancel = New DevExpress.XtraEditors.SimpleButton
        Me.cmdOK = New DevExpress.XtraEditors.SimpleButton
        Me.PanelControl4 = New DevExpress.XtraEditors.PanelControl
        Me.GridControl1 = New DevExpress.XtraGrid.GridControl
        Me.GridView1 = New DevExpress.XtraGrid.Views.Grid.GridView
        Me.ColNoID = New DevExpress.XtraGrid.Columns.GridColumn
        Me.colKode = New DevExpress.XtraGrid.Columns.GridColumn
        Me.colNama = New DevExpress.XtraGrid.Columns.GridColumn
        Me.colAlamat = New DevExpress.XtraGrid.Columns.GridColumn
        Me.colAktif = New DevExpress.XtraGrid.Columns.GridColumn
        Me.RepositoryItemCheckEdit2 = New DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit
        Me.colKota = New DevExpress.XtraGrid.Columns.GridColumn
        Me.colTelpon = New DevExpress.XtraGrid.Columns.GridColumn
        Me.colKeterangan = New DevExpress.XtraGrid.Columns.GridColumn
        Me.coGudang = New DevExpress.XtraGrid.Columns.GridColumn
        Me.colAkun = New DevExpress.XtraGrid.Columns.GridColumn
        Me.colWilayah = New DevExpress.XtraGrid.Columns.GridColumn
        Me.colCabang = New DevExpress.XtraGrid.Columns.GridColumn
        Me.colMataUang = New DevExpress.XtraGrid.Columns.GridColumn
        Me.colKodeMataUang = New DevExpress.XtraGrid.Columns.GridColumn
        Me.cNoRekening = New DevExpress.XtraGrid.Columns.GridColumn
        Me.cAkunIntransit = New DevExpress.XtraGrid.Columns.GridColumn
        Me.cIsKas = New DevExpress.XtraGrid.Columns.GridColumn
        Me.cIsBank = New DevExpress.XtraGrid.Columns.GridColumn
        Me.RepositoryItemLookUpEdit2 = New DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit
        CType(Me.PanelControl1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.PanelControl1.SuspendLayout()
        CType(Me.CheckEdit1.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtGudang.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PanelControl2, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.PanelControl2.SuspendLayout()
        CType(Me.PanelControl3, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.PanelControl3.SuspendLayout()
        CType(Me.PanelControl4, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.PanelControl4.SuspendLayout()
        CType(Me.GridControl1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.GridView1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RepositoryItemCheckEdit2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RepositoryItemLookUpEdit2, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'PanelControl1
        '
        Me.PanelControl1.Appearance.BackColor = System.Drawing.Color.Transparent
        Me.PanelControl1.Appearance.Options.UseBackColor = True
        Me.PanelControl1.ContentImageAlignment = System.Drawing.ContentAlignment.MiddleLeft
        Me.PanelControl1.Controls.Add(Me.CheckEdit1)
        Me.PanelControl1.Controls.Add(Me.txtGudang)
        Me.PanelControl1.Controls.Add(Me.LabelControl2)
        Me.PanelControl1.Controls.Add(Me.Label2)
        Me.PanelControl1.Dock = System.Windows.Forms.DockStyle.Top
        Me.PanelControl1.Location = New System.Drawing.Point(0, 0)
        Me.PanelControl1.LookAndFeel.SkinName = "Glass Oceans"
        Me.PanelControl1.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.UltraFlat
        Me.PanelControl1.Name = "PanelControl1"
        Me.PanelControl1.Size = New System.Drawing.Size(693, 38)
        Me.PanelControl1.TabIndex = 2
        '
        'CheckEdit1
        '
        Me.CheckEdit1.EditValue = True
        Me.CheckEdit1.Location = New System.Drawing.Point(340, 10)
        Me.CheckEdit1.Name = "CheckEdit1"
        Me.CheckEdit1.Properties.Appearance.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CheckEdit1.Properties.Appearance.Options.UseFont = True
        Me.CheckEdit1.Properties.Caption = "Filter"
        Me.CheckEdit1.Size = New System.Drawing.Size(75, 21)
        Me.CheckEdit1.TabIndex = 6
        Me.CheckEdit1.Visible = False
        '
        'txtGudang
        '
        Me.txtGudang.EnterMoveNextControl = True
        Me.txtGudang.Location = New System.Drawing.Point(111, 10)
        Me.txtGudang.Name = "txtGudang"
        Me.txtGudang.Properties.Appearance.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtGudang.Properties.Appearance.Options.UseFont = True
        Me.txtGudang.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Ellipsis, "", -1, True, True, False, DevExpress.XtraEditors.ImageLocation.MiddleCenter, Nothing, New DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.PageDown), SerializableAppearanceObject1, "", Nothing, Nothing, True), New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Close, "", -1, True, True, False, DevExpress.XtraEditors.ImageLocation.MiddleCenter, Nothing, New DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.Delete), SerializableAppearanceObject2, "", Nothing, Nothing, True)})
        Me.txtGudang.Size = New System.Drawing.Size(223, 22)
        Me.txtGudang.TabIndex = 5
        Me.txtGudang.Visible = False
        '
        'LabelControl2
        '
        Me.LabelControl2.Appearance.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LabelControl2.Location = New System.Drawing.Point(12, 13)
        Me.LabelControl2.Name = "LabelControl2"
        Me.LabelControl2.Size = New System.Drawing.Size(29, 16)
        Me.LabelControl2.TabIndex = 4
        Me.LabelControl2.Text = "Divisi"
        Me.LabelControl2.Visible = False
        '
        'Label2
        '
        Me.Label2.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label2.BackColor = System.Drawing.Color.Transparent
        Me.Label2.Font = New System.Drawing.Font("Rockwell", 15.75!, CType((System.Drawing.FontStyle.Bold), System.Drawing.FontStyle), System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.ForeColor = System.Drawing.SystemColors.HotTrack
        Me.Label2.ImageAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.Label2.Location = New System.Drawing.Point(340, 9)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(348, 23)
        Me.Label2.TabIndex = 0
        Me.Label2.Text = "Daftar Kas && Bank"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'PanelControl2
        '
        Me.PanelControl2.Appearance.BackColor = System.Drawing.SystemColors.ButtonFace
        Me.PanelControl2.Appearance.Options.UseBackColor = True
        Me.PanelControl2.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder
        Me.PanelControl2.Controls.Add(Me.cmClose)
        Me.PanelControl2.Controls.Add(Me.cmdBaru)
        Me.PanelControl2.Controls.Add(Me.cmdRefresh)
        Me.PanelControl2.Controls.Add(Me.cmdEdit)
        Me.PanelControl2.Controls.Add(Me.cmdHapus)
        Me.PanelControl2.Dock = System.Windows.Forms.DockStyle.Left
        Me.PanelControl2.Location = New System.Drawing.Point(0, 38)
        Me.PanelControl2.LookAndFeel.SkinName = "Glass Oceans"
        Me.PanelControl2.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.UltraFlat
        Me.PanelControl2.Name = "PanelControl2"
        Me.PanelControl2.Size = New System.Drawing.Size(96, 507)
        Me.PanelControl2.TabIndex = 3
        '
        'cmClose
        '
        Me.cmClose.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.cmClose.Cursor = System.Windows.Forms.Cursors.Hand
        Me.cmClose.ImageIndex = 1
        Me.cmClose.Location = New System.Drawing.Point(3, 463)
        Me.cmClose.Name = "cmClose"
        Me.cmClose.Size = New System.Drawing.Size(85, 34)
        Me.cmClose.TabIndex = 4
        Me.cmClose.Text = "&Close"
        '
        'cmdBaru
        '
        Me.cmdBaru.Cursor = System.Windows.Forms.Cursors.Hand
        Me.cmdBaru.ImageIndex = 6
        Me.cmdBaru.Location = New System.Drawing.Point(3, 46)
        Me.cmdBaru.Name = "cmdBaru"
        Me.cmdBaru.Size = New System.Drawing.Size(85, 34)
        Me.cmdBaru.TabIndex = 0
        Me.cmdBaru.Text = "&New"
        '
        'cmdRefresh
        '
        Me.cmdRefresh.Cursor = System.Windows.Forms.Cursors.Hand
        Me.cmdRefresh.ImageIndex = 13
        Me.cmdRefresh.Location = New System.Drawing.Point(3, 6)
        Me.cmdRefresh.Name = "cmdRefresh"
        Me.cmdRefresh.Size = New System.Drawing.Size(85, 34)
        Me.cmdRefresh.TabIndex = 3
        Me.cmdRefresh.Text = "&Refresh"
        '
        'cmdEdit
        '
        Me.cmdEdit.Cursor = System.Windows.Forms.Cursors.Hand
        Me.cmdEdit.ImageIndex = 5
        Me.cmdEdit.Location = New System.Drawing.Point(3, 84)
        Me.cmdEdit.Name = "cmdEdit"
        Me.cmdEdit.Size = New System.Drawing.Size(85, 34)
        Me.cmdEdit.TabIndex = 1
        Me.cmdEdit.Text = "&Edit"
        '
        'cmdHapus
        '
        Me.cmdHapus.Cursor = System.Windows.Forms.Cursors.Hand
        Me.cmdHapus.ImageIndex = 7
        Me.cmdHapus.Location = New System.Drawing.Point(3, 124)
        Me.cmdHapus.Name = "cmdHapus"
        Me.cmdHapus.Size = New System.Drawing.Size(85, 34)
        Me.cmdHapus.TabIndex = 2
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
        Me.PanelControl3.Location = New System.Drawing.Point(96, 491)
        Me.PanelControl3.Name = "PanelControl3"
        Me.PanelControl3.Size = New System.Drawing.Size(597, 54)
        Me.PanelControl3.TabIndex = 1
        '
        'SimpleButton1
        '
        Me.SimpleButton1.Cursor = System.Windows.Forms.Cursors.Hand
        Me.SimpleButton1.ImageIndex = 13
        Me.SimpleButton1.Location = New System.Drawing.Point(6, 8)
        Me.SimpleButton1.Name = "SimpleButton1"
        Me.SimpleButton1.Size = New System.Drawing.Size(85, 34)
        Me.SimpleButton1.TabIndex = 0
        Me.SimpleButton1.Text = "&Refresh"
        '
        'cmdCancel
        '
        Me.cmdCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdCancel.Appearance.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdCancel.Appearance.Options.UseFont = True
        Me.cmdCancel.Cursor = System.Windows.Forms.Cursors.Hand
        Me.cmdCancel.ImageIndex = 10
        Me.cmdCancel.Location = New System.Drawing.Point(492, 8)
        Me.cmdCancel.Name = "cmdCancel"
        Me.cmdCancel.Size = New System.Drawing.Size(80, 34)
        Me.cmdCancel.TabIndex = 2
        Me.cmdCancel.Text = "&Cancel"
        '
        'cmdOK
        '
        Me.cmdOK.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdOK.Appearance.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdOK.Appearance.Options.UseFont = True
        Me.cmdOK.Cursor = System.Windows.Forms.Cursors.Hand
        Me.cmdOK.ImageIndex = 4
        Me.cmdOK.Location = New System.Drawing.Point(406, 8)
        Me.cmdOK.Name = "cmdOK"
        Me.cmdOK.Size = New System.Drawing.Size(80, 34)
        Me.cmdOK.TabIndex = 1
        Me.cmdOK.Text = "&Ok"
        '
        'PanelControl4
        '
        Me.PanelControl4.Controls.Add(Me.GridControl1)
        Me.PanelControl4.Dock = System.Windows.Forms.DockStyle.Fill
        Me.PanelControl4.Location = New System.Drawing.Point(96, 38)
        Me.PanelControl4.Name = "PanelControl4"
        Me.PanelControl4.Size = New System.Drawing.Size(597, 453)
        Me.PanelControl4.TabIndex = 0
        '
        'GridControl1
        '
        Me.GridControl1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.GridControl1.Location = New System.Drawing.Point(2, 2)
        Me.GridControl1.MainView = Me.GridView1
        Me.GridControl1.Name = "GridControl1"
        Me.GridControl1.RepositoryItems.AddRange(New DevExpress.XtraEditors.Repository.RepositoryItem() {Me.RepositoryItemCheckEdit2, Me.RepositoryItemLookUpEdit2})
        Me.GridControl1.Size = New System.Drawing.Size(593, 449)
        Me.GridControl1.TabIndex = 0
        Me.GridControl1.ViewCollection.AddRange(New DevExpress.XtraGrid.Views.Base.BaseView() {Me.GridView1})
        '
        'GridView1
        '
        Me.GridView1.Columns.AddRange(New DevExpress.XtraGrid.Columns.GridColumn() {Me.ColNoID, Me.colKode, Me.colNama, Me.colAlamat, Me.colAktif, Me.colKota, Me.colTelpon, Me.colKeterangan, Me.coGudang, Me.colAkun, Me.colWilayah, Me.colCabang, Me.colMataUang, Me.colKodeMataUang, Me.cNoRekening, Me.cAkunIntransit, Me.cIsKas, Me.cIsBank})
        Me.GridView1.CustomizationFormBounds = New System.Drawing.Rectangle(808, 463, 216, 178)
        Me.GridView1.GridControl = Me.GridControl1
        Me.GridView1.Name = "GridView1"
        Me.GridView1.OptionsBehavior.AllowIncrementalSearch = True
        Me.GridView1.OptionsBehavior.AutoSelectAllInEditor = False
        Me.GridView1.OptionsBehavior.Editable = False
        Me.GridView1.OptionsView.ColumnAutoWidth = False
        Me.GridView1.OptionsView.ShowFooter = True
        Me.GridView1.OptionsView.ShowGroupPanel = False
        '
        'ColNoID
        '
        Me.ColNoID.Caption = "No."
        Me.ColNoID.FieldName = "ID"
        Me.ColNoID.GroupFormat.FormatString = "###,##0"
        Me.ColNoID.GroupFormat.FormatType = DevExpress.Utils.FormatType.Numeric
        Me.ColNoID.Name = "ColNoID"
        Me.ColNoID.Width = 46
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
        'colAlamat
        '
        Me.colAlamat.AppearanceCell.Options.UseTextOptions = True
        Me.colAlamat.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
        Me.colAlamat.AppearanceHeader.Options.UseTextOptions = True
        Me.colAlamat.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
        Me.colAlamat.Caption = "Alamat"
        Me.colAlamat.FieldName = "Alamat"
        Me.colAlamat.Name = "colAlamat"
        Me.colAlamat.Visible = True
        Me.colAlamat.VisibleIndex = 2
        '
        'colAktif
        '
        Me.colAktif.Caption = "Aktif"
        Me.colAktif.ColumnEdit = Me.RepositoryItemCheckEdit2
        Me.colAktif.FieldName = "IsAktif"
        Me.colAktif.Name = "colAktif"
        Me.colAktif.Visible = True
        Me.colAktif.VisibleIndex = 3
        '
        'RepositoryItemCheckEdit2
        '
        Me.RepositoryItemCheckEdit2.AutoHeight = False
        Me.RepositoryItemCheckEdit2.Name = "RepositoryItemCheckEdit2"
        Me.RepositoryItemCheckEdit2.NullStyle = DevExpress.XtraEditors.Controls.StyleIndeterminate.Inactive
        '
        'colKota
        '
        Me.colKota.Caption = "Kota"
        Me.colKota.FieldName = "Kota"
        Me.colKota.Name = "colKota"
        Me.colKota.Visible = True
        Me.colKota.VisibleIndex = 4
        '
        'colTelpon
        '
        Me.colTelpon.Caption = "Telpon"
        Me.colTelpon.FieldName = "Telpon"
        Me.colTelpon.Name = "colTelpon"
        Me.colTelpon.Visible = True
        Me.colTelpon.VisibleIndex = 5
        '
        'colKeterangan
        '
        Me.colKeterangan.Caption = "Keterangan"
        Me.colKeterangan.FieldName = "Keterangan"
        Me.colKeterangan.Name = "colKeterangan"
        Me.colKeterangan.Visible = True
        Me.colKeterangan.VisibleIndex = 6
        '
        'coGudang
        '
        Me.coGudang.Caption = "Gudang"
        Me.coGudang.FieldName = "Gudang"
        Me.coGudang.Name = "coGudang"
        Me.coGudang.Visible = True
        Me.coGudang.VisibleIndex = 7
        '
        'colAkun
        '
        Me.colAkun.AppearanceCell.Options.UseTextOptions = True
        Me.colAkun.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.colAkun.AppearanceHeader.Options.UseTextOptions = True
        Me.colAkun.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.colAkun.Caption = "Akun"
        Me.colAkun.FieldName = "Akun"
        Me.colAkun.Name = "colAkun"
        Me.colAkun.Visible = True
        Me.colAkun.VisibleIndex = 8
        '
        'colWilayah
        '
        Me.colWilayah.Caption = "Wilayah"
        Me.colWilayah.FieldName = "Wilayah"
        Me.colWilayah.Name = "colWilayah"
        '
        'colCabang
        '
        Me.colCabang.Caption = "Cabang"
        Me.colCabang.FieldName = "Cabang"
        Me.colCabang.Name = "colCabang"
        '
        'colMataUang
        '
        Me.colMataUang.Caption = "Mata Uang"
        Me.colMataUang.FieldName = "MataUang"
        Me.colMataUang.Name = "colMataUang"
        Me.colMataUang.Visible = True
        Me.colMataUang.VisibleIndex = 9
        '
        'colKodeMataUang
        '
        Me.colKodeMataUang.Caption = "Kode MU"
        Me.colKodeMataUang.FieldName = "KodeMataUang"
        Me.colKodeMataUang.Name = "colKodeMataUang"
        Me.colKodeMataUang.Visible = True
        Me.colKodeMataUang.VisibleIndex = 10
        '
        'cNoRekening
        '
        Me.cNoRekening.Caption = "No. Rekening"
        Me.cNoRekening.FieldName = "NoRekening"
        Me.cNoRekening.Name = "cNoRekening"
        Me.cNoRekening.Visible = True
        Me.cNoRekening.VisibleIndex = 11
        '
        'cAkunIntransit
        '
        Me.cAkunIntransit.Caption = "Akun Intransit"
        Me.cAkunIntransit.FieldName = "AKunIntransit"
        Me.cAkunIntransit.Name = "cAkunIntransit"
        Me.cAkunIntransit.Visible = True
        Me.cAkunIntransit.VisibleIndex = 12
        '
        'cIsKas
        '
        Me.cIsKas.Caption = "IsKas"
        Me.cIsKas.FieldName = "IsKas"
        Me.cIsKas.Name = "cIsKas"
        Me.cIsKas.Visible = True
        Me.cIsKas.VisibleIndex = 13
        '
        'cIsBank
        '
        Me.cIsBank.Caption = "IsBank"
        Me.cIsBank.FieldName = "IsBank"
        Me.cIsBank.Name = "cIsBank"
        Me.cIsBank.Visible = True
        Me.cIsBank.VisibleIndex = 14
        '
        'RepositoryItemLookUpEdit2
        '
        Me.RepositoryItemLookUpEdit2.AutoHeight = False
        Me.RepositoryItemLookUpEdit2.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.RepositoryItemLookUpEdit2.Name = "RepositoryItemLookUpEdit2"
        '
        'frmLUKas
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 14)
        Me.ClientSize = New System.Drawing.Size(693, 545)
        Me.Controls.Add(Me.PanelControl4)
        Me.Controls.Add(Me.PanelControl3)
        Me.Controls.Add(Me.PanelControl2)
        Me.Controls.Add(Me.PanelControl1)
        Me.Name = "frmLUKas"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Daftar Kas & Bank"
        CType(Me.PanelControl1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.PanelControl1.ResumeLayout(False)
        Me.PanelControl1.PerformLayout()
        CType(Me.CheckEdit1.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtGudang.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PanelControl2, System.ComponentModel.ISupportInitialize).EndInit()
        Me.PanelControl2.ResumeLayout(False)
        CType(Me.PanelControl3, System.ComponentModel.ISupportInitialize).EndInit()
        Me.PanelControl3.ResumeLayout(False)
        CType(Me.PanelControl4, System.ComponentModel.ISupportInitialize).EndInit()
        Me.PanelControl4.ResumeLayout(False)
        CType(Me.GridControl1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.GridView1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RepositoryItemCheckEdit2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RepositoryItemLookUpEdit2, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

#End Region
    Public NoID, IDWilayah, IDCabang, IDAKun, IDMataUang As Long
    Public Kode As String
    Public Nama, NamaMataUang, KodeMataUang, Akun As String
    Dim BolehAmbilData As Boolean
    Public Filter As String, pStatus As mdlAccPublik.Ptipe
    Public row As System.Data.DataRow = Nothing
    Public Sub TampilData()
        Dim strsql As String
        'Dim isAda As Boolean
        Dim curentcursor As Cursor = Windows.Forms.Cursor.Current
        Windows.Forms.Cursor.Current = Cursors.WaitCursor
        strsql = "SELECT " & "MKas.*,  MAkun.Nama as Akun,MGudang.Nama AS Gudang " & vbCrLf
        strsql &= " FROM ((MBank MKas " & vbCrLf
        strsql &= " LEFT JOIN MGudang  ON MKas.IDGudang = MGudang.NoID) " & vbCrLf
        strsql &= " LEFT JOIN MAkun ON MKas.IDAkun=MAkun.ID )  " & vbCrLf
        'If IDGudang >= 1 Then
        '    strsql = strsql & " WHERE MKas.IDGudang<1 or MKas.IDGudang=" & IDGudang
        'Else
        '    strsql = strsql & " WHERE 1=1"
        'End If
        'If CheckEdit1.Checked andalso Filter <> "" Then
        '    strsql &= " AND " & Filter
        'End If
        Try
            ExecuteDBGrid(GridControl1, strsql)
        Catch ex As Exception
            MsgBox(ex.Message, MessageBoxButtons.OK + MessageBoxIcon.Error)
        End Try
        Windows.Forms.Cursor.Current = curentcursor
        Application.DoEvents()
        GridView1.FocusedRowHandle = GridView1.LocateByValue(0, ColNoID, NoID)
    End Sub
    Sub AmbilData()
        Try
            Dim view As ColumnView = GridControl1.FocusedView
            row = GridView1.GetDataRow(GridView1.FocusedRowHandle)
            NoID = NullTolong(row("NoID"))
            IDGudang = NullTolong(row("IDGudang"))
            Kode = NullTostr(row("Kode"))
            Nama = NullTostr(row("Nama"))
            IDAKun = NullTolong(row("IDAkun"))
            Akun = NullTostr(row("Akun"))
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



    Private Sub cmdRefresh_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdRefresh.Click
        TampilData()
    End Sub

    Private Sub SimpleButton1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SimpleButton1.Click
        TampilData()
    End Sub
    Private Sub SetCtlMe()

        If IDGudang <= 0 Then
            IDGudang = DefIDGudang
            txtGudang.Text = defNamaGudang
        End If
        If Not IsAdministrator Then
            txtGudang.Enabled = False
        End If
        If pStatus = mdlAccPublik.ptipe.Lihat Then
            PanelControl3.Visible = False
            PanelControl2.Visible = True
        Else
            PanelControl3.Visible = True
            PanelControl2.Visible = False
        End If

    End Sub

    Private Sub frmLUAkun_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        GridView1.SaveLayoutToXml(LayOutKu(Name & GridView1.Name))
    End Sub
    Private Sub frmDaftarAlamat_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        IDGudang = -1
        SetCtlMe()
        TampilData()
        If System.IO.File.Exists(LayOutKu(Name & GridView1.Name)) Then
            GridView1.RestoreLayoutFromXml(LayOutKu(Name & GridView1.Name))
        End If
    End Sub

    Private Sub cmClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmClose.Click
        Close()
    End Sub

    Private Sub cmdHapus_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdHapus.Click
        Dim view As ColumnView = GridControl1.FocusedView
        'Try
        Dim row As System.Data.DataRow = view.GetDataRow(GridView1.FocusedRowHandle)
        Dim NOid As Long = row("NoID")
        If MsgBox("Yakin Mau Hapus data ini?", MessageBoxButtons.YesNo + MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
            EksekusiSQL("DELETE FROM MBank WHERE NoID=" & NOid)
            TampilData()
        End If
    End Sub

    Private Sub GridControl1_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles GridControl1.Click

    End Sub

    Private Sub GridControl1_DoubleClick2(ByVal sender As Object, ByVal e As System.EventArgs) Handles GridControl1.DoubleClick
        If BolehAmbilData Then
            If pStatus = mdlAccPublik.ptipe.LookUp Then
                AmbilData()
            ElseIf pStatus = mdlAccPublik.ptipe.Lihat Then
                cmdEdit.PerformClick()
            End If
        End If
    End Sub

    Private Sub GridControl1_KeyDown1(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles GridControl1.KeyDown
        If e.KeyCode = Keys.Enter Then
            If pStatus = mdlAccPublik.ptipe.LookUp Then
                AmbilData()
            ElseIf pStatus = mdlAccPublik.ptipe.Lihat Then
                cmdEdit.PerformClick()
            End If
        ElseIf e.KeyCode = Keys.Escape Then
            DialogResult = Windows.Forms.DialogResult.Cancel
            Close()
            'ElseIf e.Keydata = Keys.Control + Keys.P Then
            '    PrintGrid(GridControl1)
            'ElseIf e.Keydata = Keys.Control + Keys.S Then
            '    ExportGrid(GridControl1, ExportTo.Excel)
        End If
    End Sub

    Private Sub GridControl1_MouseDown1(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles GridControl1.MouseDown
        Dim HI As New DevExpress.XtraGrid.Views.Grid.ViewInfo.GridHitInfo
        HI = GridView1.CalcHitInfo(e.X, e.Y)
        If HI.InRow Then
            BolehAmbilData = True
        Else
            BolehAmbilData = False
        End If
    End Sub
    Public IDGudang As Long
    Private Sub txtGudang_ButtonClick(ByVal sender As Object, ByVal e As DevExpress.XtraEditors.Controls.ButtonPressedEventArgs) Handles txtGudang.ButtonClick
        Select Case e.Button.Index
            Case 0
                Dim x As New frmLUGudang
                x.IDWilayah = DefIDWilayah
                x.pStatus = mdlAccPublik.ptipe.LookUp
                If x.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
                    IDGudang = x.NoID
                    txtGudang.Text = String.Format("{0} - {1}", x.Kode, x.Nama)
                End If
                x.Dispose()
                TampilData()
            Case 1
                IDGudang = -1
                txtGudang.Text = ""
                TampilData()
        End Select
    End Sub

    Private Sub txtGudang_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtGudang.EditValueChanged

    End Sub

    Private Sub cmdBaru_MouseHover(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdBaru.MouseHover

    End Sub

    Private Sub cmdBaru_MouseLeave(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdBaru.MouseLeave

    End Sub

    Private Sub CheckEdit1_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckEdit1.CheckedChanged
        cmdRefresh.PerformClick()
    End Sub

 
    Private Sub cmdEdit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdEdit.Click

        Dim x As New frmEntriKasBank
        Dim view As ColumnView = GridControl1.FocusedView
        'Try
        Dim row As System.Data.DataRow = view.GetDataRow(GridView1.FocusedRowHandle)
        Dim NOid As Long = row("NoID")
        x.ID = NOid
        x.pStatus = frmEntriKasBank.ptipe.Edit
        x.ShowDialog(Me)
        x.Dispose()
    End Sub

    Private Sub cmdBaru_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdBaru.Click
        Dim x As New frmEntriKasBank
        x.pStatus = frmEntriKasBank.ptipe.Baru
        x.ShowDialog(Me)
        x.Dispose()
    End Sub
End Class

