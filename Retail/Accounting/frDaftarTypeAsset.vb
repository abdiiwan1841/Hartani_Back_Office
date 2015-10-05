Imports System.Data.SqlClient
Imports System.Windows.Forms
Imports DevExpress.XtraEditors
Imports DevExpress.XtraGrid.Views.Base
Imports DevExpress.XtraGrid.Columns
Public Class frDaftarTypeAsset
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
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents PanelControl2 As DevExpress.XtraEditors.PanelControl
    Friend WithEvents PanelControl4 As DevExpress.XtraEditors.PanelControl
    Friend WithEvents GridControl1 As DevExpress.XtraGrid.GridControl
    Friend WithEvents PanelControl3 As DevExpress.XtraEditors.PanelControl
    Friend WithEvents SimpleButton7 As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents SimpleButton6 As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents SimpleButton5 As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents SimpleButton4 As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents SimpleButton3 As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents SimpleButton2 As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents SimpleButton1 As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents GridView1 As DevExpress.XtraGrid.Views.Grid.GridView
    Friend WithEvents colNoID As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colKode As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents cIDAAset As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colIsActive As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents RepositoryItemCheckEdit1 As DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit
    Friend WithEvents cIDASusut As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents cIDAAkumulasiSusut As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents CAkunAkumulasiSusut As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents CAkunAsset As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents CAkunSusut As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents cKodeAkunAsset As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents cKodeAkunSusut As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents cKodeAkunAkumulasiSusut As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colNama As DevExpress.XtraGrid.Columns.GridColumn
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frDaftarTypeAsset))
        Me.ImageList1 = New System.Windows.Forms.ImageList(Me.components)
        Me.PanelControl1 = New DevExpress.XtraEditors.PanelControl
        Me.Label2 = New System.Windows.Forms.Label
        Me.PanelControl2 = New DevExpress.XtraEditors.PanelControl
        Me.PanelControl4 = New DevExpress.XtraEditors.PanelControl
        Me.GridControl1 = New DevExpress.XtraGrid.GridControl
        Me.GridView1 = New DevExpress.XtraGrid.Views.Grid.GridView
        Me.colNoID = New DevExpress.XtraGrid.Columns.GridColumn
        Me.colKode = New DevExpress.XtraGrid.Columns.GridColumn
        Me.colNama = New DevExpress.XtraGrid.Columns.GridColumn
        Me.cIDAAset = New DevExpress.XtraGrid.Columns.GridColumn
        Me.cIDASusut = New DevExpress.XtraGrid.Columns.GridColumn
        Me.cIDAAkumulasiSusut = New DevExpress.XtraGrid.Columns.GridColumn
        Me.CAkunAkumulasiSusut = New DevExpress.XtraGrid.Columns.GridColumn
        Me.CAkunAsset = New DevExpress.XtraGrid.Columns.GridColumn
        Me.CAkunSusut = New DevExpress.XtraGrid.Columns.GridColumn
        Me.cKodeAkunAsset = New DevExpress.XtraGrid.Columns.GridColumn
        Me.colIsActive = New DevExpress.XtraGrid.Columns.GridColumn
        Me.RepositoryItemCheckEdit1 = New DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit
        Me.cKodeAkunSusut = New DevExpress.XtraGrid.Columns.GridColumn
        Me.cKodeAkunAkumulasiSusut = New DevExpress.XtraGrid.Columns.GridColumn
        Me.PanelControl3 = New DevExpress.XtraEditors.PanelControl
        Me.SimpleButton7 = New DevExpress.XtraEditors.SimpleButton
        Me.SimpleButton6 = New DevExpress.XtraEditors.SimpleButton
        Me.SimpleButton5 = New DevExpress.XtraEditors.SimpleButton
        Me.SimpleButton4 = New DevExpress.XtraEditors.SimpleButton
        Me.SimpleButton3 = New DevExpress.XtraEditors.SimpleButton
        Me.SimpleButton2 = New DevExpress.XtraEditors.SimpleButton
        Me.SimpleButton1 = New DevExpress.XtraEditors.SimpleButton
        CType(Me.PanelControl1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.PanelControl1.SuspendLayout()
        CType(Me.PanelControl2, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.PanelControl2.SuspendLayout()
        CType(Me.PanelControl4, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.PanelControl4.SuspendLayout()
        CType(Me.GridControl1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.GridView1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RepositoryItemCheckEdit1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PanelControl3, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.PanelControl3.SuspendLayout()
        Me.SuspendLayout()
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
        '
        'PanelControl1
        '
        Me.PanelControl1.Appearance.BackColor = System.Drawing.Color.Transparent
        Me.PanelControl1.Appearance.Options.UseBackColor = True
        Me.PanelControl1.ContentImageAlignment = System.Drawing.ContentAlignment.MiddleLeft
        Me.PanelControl1.Controls.Add(Me.Label2)
        Me.PanelControl1.Dock = System.Windows.Forms.DockStyle.Top
        Me.PanelControl1.Location = New System.Drawing.Point(0, 0)
        Me.PanelControl1.LookAndFeel.SkinName = "Glass Oceans"
        Me.PanelControl1.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.UltraFlat
        Me.PanelControl1.Name = "PanelControl1"
        Me.PanelControl1.Size = New System.Drawing.Size(597, 38)
        Me.PanelControl1.TabIndex = 4
        '
        'Label2
        '
        Me.Label2.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label2.BackColor = System.Drawing.Color.Transparent
        Me.Label2.Font = New System.Drawing.Font("Rockwell", 15.75!, CType((System.Drawing.FontStyle.Bold), System.Drawing.FontStyle), System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.ForeColor = System.Drawing.SystemColors.ActiveCaption
        Me.Label2.Location = New System.Drawing.Point(216, 9)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(376, 23)
        Me.Label2.TabIndex = 1
        Me.Label2.Text = "Daftar Tipe Asset"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'PanelControl2
        '
        Me.PanelControl2.Appearance.BackColor = System.Drawing.Color.Transparent
        Me.PanelControl2.Appearance.Options.UseBackColor = True
        Me.PanelControl2.Controls.Add(Me.PanelControl4)
        Me.PanelControl2.Controls.Add(Me.PanelControl3)
        Me.PanelControl2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.PanelControl2.Location = New System.Drawing.Point(0, 38)
        Me.PanelControl2.LookAndFeel.SkinName = "Glass Oceans"
        Me.PanelControl2.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.UltraFlat
        Me.PanelControl2.Name = "PanelControl2"
        Me.PanelControl2.Size = New System.Drawing.Size(597, 376)
        Me.PanelControl2.TabIndex = 2
        '
        'PanelControl4
        '
        Me.PanelControl4.Controls.Add(Me.GridControl1)
        Me.PanelControl4.Dock = System.Windows.Forms.DockStyle.Fill
        Me.PanelControl4.Location = New System.Drawing.Point(104, 2)
        Me.PanelControl4.Name = "PanelControl4"
        Me.PanelControl4.Size = New System.Drawing.Size(491, 372)
        Me.PanelControl4.TabIndex = 0
        '
        'GridControl1
        '
        Me.GridControl1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.GridControl1.Location = New System.Drawing.Point(2, 2)
        Me.GridControl1.MainView = Me.GridView1
        Me.GridControl1.Name = "GridControl1"
        Me.GridControl1.RepositoryItems.AddRange(New DevExpress.XtraEditors.Repository.RepositoryItem() {Me.RepositoryItemCheckEdit1})
        Me.GridControl1.Size = New System.Drawing.Size(487, 368)
        Me.GridControl1.TabIndex = 0
        Me.GridControl1.ViewCollection.AddRange(New DevExpress.XtraGrid.Views.Base.BaseView() {Me.GridView1})
        '
        'GridView1
        '
        Me.GridView1.Columns.AddRange(New DevExpress.XtraGrid.Columns.GridColumn() {Me.colNoID, Me.colKode, Me.colNama, Me.cIDAAset, Me.cIDASusut, Me.cIDAAkumulasiSusut, Me.CAkunAkumulasiSusut, Me.CAkunAsset, Me.CAkunSusut, Me.cKodeAkunAsset, Me.colIsActive, Me.cKodeAkunSusut, Me.cKodeAkunAkumulasiSusut})
        Me.GridView1.GridControl = Me.GridControl1
        Me.GridView1.Name = "GridView1"
        Me.GridView1.OptionsBehavior.AllowIncrementalSearch = True
        Me.GridView1.OptionsBehavior.AutoSelectAllInEditor = False
        Me.GridView1.OptionsBehavior.Editable = False
        Me.GridView1.OptionsView.ColumnAutoWidth = False
        '
        'colNoID
        '
        Me.colNoID.Caption = "No."
        Me.colNoID.FieldName = "NoID"
        Me.colNoID.GroupFormat.FormatString = "###,##0"
        Me.colNoID.GroupFormat.FormatType = DevExpress.Utils.FormatType.Numeric
        Me.colNoID.Name = "colNoID"
        Me.colNoID.Visible = True
        Me.colNoID.VisibleIndex = 0
        Me.colNoID.Width = 46
        '
        'colKode
        '
        Me.colKode.Caption = "Kode"
        Me.colKode.FieldName = "Kode"
        Me.colKode.Name = "colKode"
        Me.colKode.Visible = True
        Me.colKode.VisibleIndex = 1
        Me.colKode.Width = 91
        '
        'colNama
        '
        Me.colNama.Caption = "Nama"
        Me.colNama.FieldName = "Nama"
        Me.colNama.Name = "colNama"
        Me.colNama.Visible = True
        Me.colNama.VisibleIndex = 2
        Me.colNama.Width = 208
        '
        'cIDAAset
        '
        Me.cIDAAset.Caption = "ID Akun Aset"
        Me.cIDAAset.FieldName = "IDAAset"
        Me.cIDAAset.Name = "cIDAAset"
        '
        'cIDASusut
        '
        Me.cIDASusut.Caption = "ID Akun Penyusutan"
        Me.cIDASusut.FieldName = "IDASusut"
        Me.cIDASusut.Name = "cIDASusut"
        Me.cIDASusut.Width = 107
        '
        'cIDAAkumulasiSusut
        '
        Me.cIDAAkumulasiSusut.Caption = "ID Akun Akumulasi Penyusutan"
        Me.cIDAAkumulasiSusut.FieldName = "IDAAkumulasiSusut"
        Me.cIDAAkumulasiSusut.Name = "cIDAAkumulasiSusut"
        Me.cIDAAkumulasiSusut.Width = 157
        '
        'CAkunAkumulasiSusut
        '
        Me.CAkunAkumulasiSusut.Caption = "Akun Akumulasi Susut"
        Me.CAkunAkumulasiSusut.FieldName = "AkunAkumulasiSusut"
        Me.CAkunAkumulasiSusut.Name = "CAkunAkumulasiSusut"
        Me.CAkunAkumulasiSusut.Visible = True
        Me.CAkunAkumulasiSusut.VisibleIndex = 4
        Me.CAkunAkumulasiSusut.Width = 113
        '
        'CAkunAsset
        '
        Me.CAkunAsset.Caption = "Akun Asset"
        Me.CAkunAsset.FieldName = "AkunAsset"
        Me.CAkunAsset.Name = "CAkunAsset"
        Me.CAkunAsset.Visible = True
        Me.CAkunAsset.VisibleIndex = 5
        '
        'CAkunSusut
        '
        Me.CAkunSusut.Caption = "Akun Penyusutan"
        Me.CAkunSusut.FieldName = "AkunSusut"
        Me.CAkunSusut.Name = "CAkunSusut"
        Me.CAkunSusut.Visible = True
        Me.CAkunSusut.VisibleIndex = 7
        Me.CAkunSusut.Width = 93
        '
        'cKodeAkunAsset
        '
        Me.cKodeAkunAsset.Caption = "Kode Akun Asset"
        Me.cKodeAkunAsset.FieldName = "KodeAkunAsset"
        Me.cKodeAkunAsset.Name = "cKodeAkunAsset"
        Me.cKodeAkunAsset.Visible = True
        Me.cKodeAkunAsset.VisibleIndex = 6
        Me.cKodeAkunAsset.Width = 90
        '
        'colIsActive
        '
        Me.colIsActive.Caption = "Aktif"
        Me.colIsActive.ColumnEdit = Me.RepositoryItemCheckEdit1
        Me.colIsActive.FieldName = "IsActive"
        Me.colIsActive.Name = "colIsActive"
        Me.colIsActive.Visible = True
        Me.colIsActive.VisibleIndex = 3
        '
        'RepositoryItemCheckEdit1
        '
        Me.RepositoryItemCheckEdit1.AutoHeight = False
        Me.RepositoryItemCheckEdit1.Name = "RepositoryItemCheckEdit1"
        Me.RepositoryItemCheckEdit1.NullStyle = DevExpress.XtraEditors.Controls.StyleIndeterminate.Inactive
        '
        'cKodeAkunSusut
        '
        Me.cKodeAkunSusut.Caption = "Kode Akun Susut"
        Me.cKodeAkunSusut.FieldName = "KodeAkunSusut"
        Me.cKodeAkunSusut.Name = "cKodeAkunSusut"
        Me.cKodeAkunSusut.Visible = True
        Me.cKodeAkunSusut.VisibleIndex = 8
        Me.cKodeAkunSusut.Width = 90
        '
        'cKodeAkunAkumulasiSusut
        '
        Me.cKodeAkunAkumulasiSusut.Caption = "Kode Akun Akumulasi Penyusutan"
        Me.cKodeAkunAkumulasiSusut.FieldName = "KodeAkunAkumulasiSusut"
        Me.cKodeAkunAkumulasiSusut.Name = "cKodeAkunAkumulasiSusut"
        Me.cKodeAkunAkumulasiSusut.Visible = True
        Me.cKodeAkunAkumulasiSusut.VisibleIndex = 9
        Me.cKodeAkunAkumulasiSusut.Width = 170
        '
        'PanelControl3
        '
        Me.PanelControl3.Appearance.BackColor = System.Drawing.Color.LightSalmon
        Me.PanelControl3.Appearance.Options.UseBackColor = True
        Me.PanelControl3.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder
        Me.PanelControl3.Controls.Add(Me.SimpleButton7)
        Me.PanelControl3.Controls.Add(Me.SimpleButton6)
        Me.PanelControl3.Controls.Add(Me.SimpleButton5)
        Me.PanelControl3.Controls.Add(Me.SimpleButton4)
        Me.PanelControl3.Controls.Add(Me.SimpleButton3)
        Me.PanelControl3.Controls.Add(Me.SimpleButton2)
        Me.PanelControl3.Controls.Add(Me.SimpleButton1)
        Me.PanelControl3.Dock = System.Windows.Forms.DockStyle.Left
        Me.PanelControl3.Location = New System.Drawing.Point(2, 2)
        Me.PanelControl3.Name = "PanelControl3"
        Me.PanelControl3.Size = New System.Drawing.Size(102, 372)
        Me.PanelControl3.TabIndex = 1
        '
        'SimpleButton7
        '
        Me.SimpleButton7.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.SimpleButton7.Cursor = System.Windows.Forms.Cursors.Hand
        Me.SimpleButton7.ImageIndex = 2
        Me.SimpleButton7.ImageList = Me.ImageList1
        Me.SimpleButton7.Location = New System.Drawing.Point(0, 319)
        Me.SimpleButton7.Name = "SimpleButton7"
        Me.SimpleButton7.Size = New System.Drawing.Size(99, 34)
        Me.SimpleButton7.TabIndex = 6
        Me.SimpleButton7.Text = "&Close"
        '
        'SimpleButton6
        '
        Me.SimpleButton6.Cursor = System.Windows.Forms.Cursors.Hand
        Me.SimpleButton6.ImageIndex = 17
        Me.SimpleButton6.ImageList = Me.ImageList1
        Me.SimpleButton6.Location = New System.Drawing.Point(1, 3)
        Me.SimpleButton6.Name = "SimpleButton6"
        Me.SimpleButton6.Size = New System.Drawing.Size(99, 34)
        Me.SimpleButton6.TabIndex = 5
        Me.SimpleButton6.Text = "&Refresh"
        '
        'SimpleButton5
        '
        Me.SimpleButton5.Cursor = System.Windows.Forms.Cursors.Hand
        Me.SimpleButton5.ImageIndex = 6
        Me.SimpleButton5.ImageList = Me.ImageList1
        Me.SimpleButton5.Location = New System.Drawing.Point(1, 120)
        Me.SimpleButton5.Name = "SimpleButton5"
        Me.SimpleButton5.Size = New System.Drawing.Size(99, 34)
        Me.SimpleButton5.TabIndex = 4
        Me.SimpleButton5.Text = "&Hapus"
        '
        'SimpleButton4
        '
        Me.SimpleButton4.Cursor = System.Windows.Forms.Cursors.Hand
        Me.SimpleButton4.ImageIndex = 21
        Me.SimpleButton4.ImageList = Me.ImageList1
        Me.SimpleButton4.Location = New System.Drawing.Point(1, 198)
        Me.SimpleButton4.Name = "SimpleButton4"
        Me.SimpleButton4.Size = New System.Drawing.Size(99, 34)
        Me.SimpleButton4.TabIndex = 3
        Me.SimpleButton4.Text = "&Cancel"
        '
        'SimpleButton3
        '
        Me.SimpleButton3.Cursor = System.Windows.Forms.Cursors.Hand
        Me.SimpleButton3.ImageIndex = 19
        Me.SimpleButton3.ImageList = Me.ImageList1
        Me.SimpleButton3.Location = New System.Drawing.Point(1, 159)
        Me.SimpleButton3.Name = "SimpleButton3"
        Me.SimpleButton3.Size = New System.Drawing.Size(99, 34)
        Me.SimpleButton3.TabIndex = 2
        Me.SimpleButton3.Text = "&Simpan"
        '
        'SimpleButton2
        '
        Me.SimpleButton2.Cursor = System.Windows.Forms.Cursors.Hand
        Me.SimpleButton2.ImageIndex = 0
        Me.SimpleButton2.ImageList = Me.ImageList1
        Me.SimpleButton2.Location = New System.Drawing.Point(1, 81)
        Me.SimpleButton2.Name = "SimpleButton2"
        Me.SimpleButton2.Size = New System.Drawing.Size(99, 34)
        Me.SimpleButton2.TabIndex = 1
        Me.SimpleButton2.Text = "&Edit"
        '
        'SimpleButton1
        '
        Me.SimpleButton1.Cursor = System.Windows.Forms.Cursors.Hand
        Me.SimpleButton1.ImageIndex = 10
        Me.SimpleButton1.ImageList = Me.ImageList1
        Me.SimpleButton1.Location = New System.Drawing.Point(1, 42)
        Me.SimpleButton1.Name = "SimpleButton1"
        Me.SimpleButton1.Size = New System.Drawing.Size(99, 34)
        Me.SimpleButton1.TabIndex = 0
        Me.SimpleButton1.Text = "&Baru"
        '
        'frDaftarTypeAsset
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 14)
        Me.ClientSize = New System.Drawing.Size(597, 414)
        Me.Controls.Add(Me.PanelControl2)
        Me.Controls.Add(Me.PanelControl1)
        Me.Name = "frDaftarTypeAsset"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Daftar Tipe Asset"
        CType(Me.PanelControl1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.PanelControl1.ResumeLayout(False)
        CType(Me.PanelControl2, System.ComponentModel.ISupportInitialize).EndInit()
        Me.PanelControl2.ResumeLayout(False)
        CType(Me.PanelControl4, System.ComponentModel.ISupportInitialize).EndInit()
        Me.PanelControl4.ResumeLayout(False)
        CType(Me.GridControl1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.GridView1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RepositoryItemCheckEdit1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PanelControl3, System.ComponentModel.ISupportInitialize).EndInit()
        Me.PanelControl3.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub

#End Region
    Public NoID As Long = 1
    Public Kode As String
    Public Nama As String
    Public row As System.Data.DataRow
    Public Sub RefreshData()
        Dim oConn As New SqlConnection
        Dim ocmd As SqlCommand
        Dim oDA As SqlDataAdapter
        Dim oDS As New DataSet
        Dim strsql As String
        'Dim isAda As Boolean
        Dim curentcursor As Cursor = Windows.Forms.Cursor.Current
        Windows.Forms.Cursor.Current = Cursors.WaitCursor
        strsql = "Select MTA.* , MAA.Kode as KodeAkunAsset,MAA.Nama as AkunAsset," & _
        "MAAk.Kode as KodeAkunAkumulasiSusut,MAAk.Nama as AkunAkumulasiSusut," & _
        "MAS.Kode as KodeAkunSusut, MAS.Nama as AkunSusut " & _
        "from MTypeAsset MTA " & _
        "Left Join MAkun MAA On MTA.IDAAsset=MAA.ID " & _
        "Left Join MAkun MAAk On MTA.IDAAkumulasiSusut=MAAk.ID " & _
        "Left Join MAkun MAS On MTA.IDASusut=MAS.ID " 
        Try
            oConn.ConnectionString = StrKonSql
            ocmd = New SqlCommand(strsql, oConn)
            oConn.Open()
            oDA = New SqlDataAdapter(ocmd)
            oDA.Fill(oDS, "MTypeAsset")
        Catch ex As Exception
            XtraMessageBox.Show(Me, ex.Message, ".:: Pesan Kesalahan ::.", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            oConn.Close()
        End Try
        GridControl1.DataSource = oDS.Tables("MTypeAsset")
        Windows.Forms.Cursor.Current = curentcursor
        Application.DoEvents()
        GridView1.FocusedRowHandle = GridView1.LocateByValue(0, colNoID, NoID)
    End Sub



    Private Sub SimpleButton3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SimpleButton3.Click
        Dim view As ColumnView = GridControl1.FocusedView
        row = view.GetDataRow(GridView1.FocusedRowHandle)
        NoID = row("NoID")
        Kode = row("Kode")
        Nama = row("Nama")

        DialogResult = Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub

    Private Sub SimpleButton4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SimpleButton4.Click
        DialogResult = Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub


    Private Sub SimpleButton5_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SimpleButton5.Click
        Dim view As ColumnView = GridControl1.FocusedView
        'Try
        Dim row As System.Data.DataRow = view.GetDataRow(GridView1.FocusedRowHandle)
        Dim NoID As Long = row("NOID")
        'HapusData(Noid, "MASTSTNK")
        If XtraMessageBox.Show(Me, "Yakin Mau Hapus data ini?", ".:: HAPUS DATA MGUDANG ::.", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
            DeleteRowByID("MTypeAsset", "NoID", NoID)
            RefreshData()
        End If
    End Sub

    Private Sub SimpleButton6_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SimpleButton6.Click
        RefreshData()
    End Sub

    Private Sub GridControl1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles GridControl1.Click

    End Sub

    Private Sub GridControl1_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles GridControl1.KeyDown
        If e.KeyCode = Keys.Enter Then
            AmbilData()

        ElseIf e.KeyCode = Keys.Escape Then
            DialogResult = Windows.Forms.DialogResult.OK
        End If
    End Sub

    Private Sub GridControl1_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles GridControl1.DoubleClick
        AmbilData()
    End Sub
    Sub AmbilData()
        Dim view As ColumnView = GridControl1.FocusedView
        row = view.GetDataRow(GridView1.FocusedRowHandle)
        NoID = row("NoID")
        Kode = row("Kode")
        Nama = row("Nama")
        DialogResult = Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub
    '

    Private Sub SimpleButton7_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SimpleButton7.Click
        DialogResult = Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub

    Private Sub frDaftarTypeAsset_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Activated
        'GridToPrint = GridControl1
    End Sub

    Private Sub frDaftarTypeAsset_Deactivate(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Deactivate
        'GridToPrint = Nothing
    End Sub

    Private Sub frDaftarTypeAsset_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        RefreshData()
    End Sub

    Private Sub SimpleButton1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SimpleButton1.Click
        Dim fr As New frmEntriTypeAsset
        fr.pStatus = mdlAccPublik.ptipe.Baru
        If fr.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
            RefreshData()
            GridView1.ClearSelection()
            GridView1.FocusedRowHandle = GridView1.LocateByDisplayText(0, GridView1.Columns("NoID"), fr.ID.ToString("###0"))
            GridView1.SelectRow(GridView1.FocusedRowHandle)
        End If
        fr.Dispose()
    End Sub

    Private Sub SimpleButton2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SimpleButton2.Click
        Dim view As ColumnView = GridControl1.FocusedView
        Dim x As New frmEntriTypeAsset
        Try
            Dim row As System.Data.DataRow = view.GetDataRow(GridView1.FocusedRowHandle)
            Dim NoID As Long = row("NoID")
            x.ID = NoID
            x.pStatus = mdlAccPublik.ptipe.Edit
            If x.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
                RefreshData()
                GridView1.ClearSelection()
                GridView1.FocusedRowHandle = GridView1.LocateByDisplayText(0, GridView1.Columns("NoID"), x.ID.ToString("###0"))
                GridView1.SelectRow(GridView1.FocusedRowHandle)
            End If
        Catch ex As Exception
            FxMessage("Error : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            x.Close()
            x.Dispose()
        End Try
    End Sub
End Class

