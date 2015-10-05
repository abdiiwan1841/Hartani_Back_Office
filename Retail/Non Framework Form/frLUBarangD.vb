Imports System.Data.SqlClient
Imports System.Windows.Forms
Imports DevExpress.XtraEditors
Imports DevExpress.XtraGrid.Views.Base
Imports DevExpress.XtraGrid.Columns
Public Class frLUBarangD
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
    Friend WithEvents ImageList1 As System.Windows.Forms.ImageList
    Friend WithEvents SimpleButton1 As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents GridControl1 As DevExpress.XtraGrid.GridControl
    Friend WithEvents GridView1 As DevExpress.XtraGrid.Views.Grid.GridView
    Friend WithEvents RepositoryItemCheckEdit1 As DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit
    Friend WithEvents RepositoryItemLookUpEdit1 As DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit
    Friend WithEvents Label2 As System.Windows.Forms.Label
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frLUBarangD))
        Me.PanelControl1 = New DevExpress.XtraEditors.PanelControl
        Me.Label2 = New System.Windows.Forms.Label
        Me.PanelControl2 = New DevExpress.XtraEditors.PanelControl
        Me.cmClose = New DevExpress.XtraEditors.SimpleButton
        Me.ImageList1 = New System.Windows.Forms.ImageList(Me.components)
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
        Me.RepositoryItemCheckEdit1 = New DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit
        Me.RepositoryItemLookUpEdit1 = New DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit
        CType(Me.PanelControl1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.PanelControl1.SuspendLayout()
        CType(Me.PanelControl2, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.PanelControl2.SuspendLayout()
        CType(Me.PanelControl3, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.PanelControl3.SuspendLayout()
        CType(Me.PanelControl4, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.PanelControl4.SuspendLayout()
        CType(Me.GridControl1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.GridView1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RepositoryItemCheckEdit1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RepositoryItemLookUpEdit1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
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
        Me.PanelControl1.Size = New System.Drawing.Size(640, 38)
        Me.PanelControl1.TabIndex = 4
        '
        'Label2
        '
        Me.Label2.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label2.BackColor = System.Drawing.Color.Transparent
        Me.Label2.Font = New System.Drawing.Font("Rockwell", 15.75!, CType((System.Drawing.FontStyle.Bold), System.Drawing.FontStyle), System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.ForeColor = System.Drawing.SystemColors.ActiveCaption
        Me.Label2.Location = New System.Drawing.Point(38, 9)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(376, 23)
        Me.Label2.TabIndex = 1
        Me.Label2.Text = "Daftar Detil Barang"
        '
        'PanelControl2
        '
        Me.PanelControl2.Appearance.BackColor = System.Drawing.Color.Transparent
        Me.PanelControl2.Appearance.Options.UseBackColor = True
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
        Me.PanelControl2.Size = New System.Drawing.Size(96, 634)
        Me.PanelControl2.TabIndex = 5
        Me.PanelControl2.Visible = False
        '
        'cmClose
        '
        Me.cmClose.Cursor = System.Windows.Forms.Cursors.Hand
        Me.cmClose.ImageIndex = 1
        Me.cmClose.ImageList = Me.ImageList1
        Me.cmClose.Location = New System.Drawing.Point(5, 460)
        Me.cmClose.Name = "cmClose"
        Me.cmClose.Size = New System.Drawing.Size(85, 34)
        Me.cmClose.TabIndex = 6
        Me.cmClose.Text = "&Tutup"
        '
        'ImageList1
        '
        Me.ImageList1.ImageStream = CType(resources.GetObject("ImageList1.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.ImageList1.TransparentColor = System.Drawing.Color.Transparent
        Me.ImageList1.Images.SetKeyName(0, "applications.gif")
        Me.ImageList1.Images.SetKeyName(1, "bookmark.gif")
        Me.ImageList1.Images.SetKeyName(2, "save.gif")
        Me.ImageList1.Images.SetKeyName(3, "search.gif")
        Me.ImageList1.Images.SetKeyName(4, "NetByte Design Studio - 1288.png")
        Me.ImageList1.Images.SetKeyName(5, "NetByte Design Studio - 1240.png")
        Me.ImageList1.Images.SetKeyName(6, "NetByte Design Studio - 1247.png")
        Me.ImageList1.Images.SetKeyName(7, "NetByte Design Studio - 1248.png")
        Me.ImageList1.Images.SetKeyName(8, "NetByte Design Studio - 1256.png")
        Me.ImageList1.Images.SetKeyName(9, "NetByte Design Studio - 1274.png")
        Me.ImageList1.Images.SetKeyName(10, "NetByte Design Studio - 1287.png")
        Me.ImageList1.Images.SetKeyName(11, "NetByte Design Studio - 0920.png")
        Me.ImageList1.Images.SetKeyName(12, "NetByte Design Studio - 0915.png")
        Me.ImageList1.Images.SetKeyName(13, "NetByte Design Studio - 0952.png")
        '
        'cmdBaru
        '
        Me.cmdBaru.Cursor = System.Windows.Forms.Cursors.Hand
        Me.cmdBaru.ImageIndex = 6
        Me.cmdBaru.ImageList = Me.ImageList1
        Me.cmdBaru.Location = New System.Drawing.Point(5, 6)
        Me.cmdBaru.Name = "cmdBaru"
        Me.cmdBaru.Size = New System.Drawing.Size(85, 34)
        Me.cmdBaru.TabIndex = 0
        Me.cmdBaru.Text = "&New"
        '
        'cmdRefresh
        '
        Me.cmdRefresh.Cursor = System.Windows.Forms.Cursors.Hand
        Me.cmdRefresh.ImageIndex = 13
        Me.cmdRefresh.ImageList = Me.ImageList1
        Me.cmdRefresh.Location = New System.Drawing.Point(5, 205)
        Me.cmdRefresh.Name = "cmdRefresh"
        Me.cmdRefresh.Size = New System.Drawing.Size(85, 34)
        Me.cmdRefresh.TabIndex = 5
        Me.cmdRefresh.Text = "&Refresh"
        '
        'cmdEdit
        '
        Me.cmdEdit.Cursor = System.Windows.Forms.Cursors.Hand
        Me.cmdEdit.ImageIndex = 5
        Me.cmdEdit.ImageList = Me.ImageList1
        Me.cmdEdit.Location = New System.Drawing.Point(5, 44)
        Me.cmdEdit.Name = "cmdEdit"
        Me.cmdEdit.Size = New System.Drawing.Size(85, 34)
        Me.cmdEdit.TabIndex = 1
        Me.cmdEdit.Text = "&Edit"
        '
        'cmdHapus
        '
        Me.cmdHapus.Cursor = System.Windows.Forms.Cursors.Hand
        Me.cmdHapus.ImageIndex = 7
        Me.cmdHapus.ImageList = Me.ImageList1
        Me.cmdHapus.Location = New System.Drawing.Point(5, 167)
        Me.cmdHapus.Name = "cmdHapus"
        Me.cmdHapus.Size = New System.Drawing.Size(85, 34)
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
        Me.PanelControl3.Location = New System.Drawing.Point(96, 618)
        Me.PanelControl3.Name = "PanelControl3"
        Me.PanelControl3.Size = New System.Drawing.Size(544, 54)
        Me.PanelControl3.TabIndex = 6
        '
        'SimpleButton1
        '
        Me.SimpleButton1.Cursor = System.Windows.Forms.Cursors.Hand
        Me.SimpleButton1.ImageIndex = 13
        Me.SimpleButton1.ImageList = Me.ImageList1
        Me.SimpleButton1.Location = New System.Drawing.Point(6, 8)
        Me.SimpleButton1.Name = "SimpleButton1"
        Me.SimpleButton1.Size = New System.Drawing.Size(85, 34)
        Me.SimpleButton1.TabIndex = 6
        Me.SimpleButton1.Text = "&Refresh"
        '
        'cmdCancel
        '
        Me.cmdCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdCancel.Appearance.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdCancel.Appearance.Options.UseFont = True
        Me.cmdCancel.Cursor = System.Windows.Forms.Cursors.Hand
        Me.cmdCancel.ImageIndex = 10
        Me.cmdCancel.ImageList = Me.ImageList1
        Me.cmdCancel.Location = New System.Drawing.Point(439, 8)
        Me.cmdCancel.Name = "cmdCancel"
        Me.cmdCancel.Size = New System.Drawing.Size(80, 34)
        Me.cmdCancel.TabIndex = 3
        Me.cmdCancel.Text = "&Cancel"
        '
        'cmdOK
        '
        Me.cmdOK.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdOK.Appearance.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdOK.Appearance.Options.UseFont = True
        Me.cmdOK.Cursor = System.Windows.Forms.Cursors.Hand
        Me.cmdOK.ImageIndex = 4
        Me.cmdOK.ImageList = Me.ImageList1
        Me.cmdOK.Location = New System.Drawing.Point(353, 8)
        Me.cmdOK.Name = "cmdOK"
        Me.cmdOK.Size = New System.Drawing.Size(80, 34)
        Me.cmdOK.TabIndex = 2
        Me.cmdOK.Text = "&Ok"
        '
        'PanelControl4
        '
        Me.PanelControl4.Controls.Add(Me.GridControl1)
        Me.PanelControl4.Dock = System.Windows.Forms.DockStyle.Fill
        Me.PanelControl4.Location = New System.Drawing.Point(96, 38)
        Me.PanelControl4.Name = "PanelControl4"
        Me.PanelControl4.Size = New System.Drawing.Size(544, 580)
        Me.PanelControl4.TabIndex = 7
        '
        'GridControl1
        '
        Me.GridControl1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.GridControl1.Location = New System.Drawing.Point(2, 2)
        Me.GridControl1.MainView = Me.GridView1
        Me.GridControl1.Name = "GridControl1"
        Me.GridControl1.RepositoryItems.AddRange(New DevExpress.XtraEditors.Repository.RepositoryItem() {Me.RepositoryItemCheckEdit1, Me.RepositoryItemLookUpEdit1})
        Me.GridControl1.Size = New System.Drawing.Size(540, 576)
        Me.GridControl1.TabIndex = 0
        Me.GridControl1.ViewCollection.AddRange(New DevExpress.XtraGrid.Views.Base.BaseView() {Me.GridView1})
        '
        'GridView1
        '
        Me.GridView1.GridControl = Me.GridControl1
        Me.GridView1.Name = "GridView1"
        Me.GridView1.OptionsBehavior.AllowIncrementalSearch = True
        Me.GridView1.OptionsBehavior.AutoSelectAllInEditor = False
        Me.GridView1.OptionsBehavior.Editable = False
        Me.GridView1.OptionsView.ColumnAutoWidth = False
        Me.GridView1.OptionsView.HeaderFilterButtonShowMode = DevExpress.XtraEditors.Controls.FilterButtonShowMode.Button
        Me.GridView1.OptionsView.ShowAutoFilterRow = True
        Me.GridView1.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.ShowAlways
        Me.GridView1.OptionsView.ShowGroupPanel = False
        '
        'RepositoryItemCheckEdit1
        '
        Me.RepositoryItemCheckEdit1.AutoHeight = False
        Me.RepositoryItemCheckEdit1.Name = "RepositoryItemCheckEdit1"
        Me.RepositoryItemCheckEdit1.NullStyle = DevExpress.XtraEditors.Controls.StyleIndeterminate.Inactive
        '
        'RepositoryItemLookUpEdit1
        '
        Me.RepositoryItemLookUpEdit1.AutoHeight = False
        Me.RepositoryItemLookUpEdit1.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.RepositoryItemLookUpEdit1.Name = "RepositoryItemLookUpEdit1"
        '
        'frLUBarangD
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 14)
        Me.ClientSize = New System.Drawing.Size(640, 672)
        Me.Controls.Add(Me.PanelControl4)
        Me.Controls.Add(Me.PanelControl3)
        Me.Controls.Add(Me.PanelControl2)
        Me.Controls.Add(Me.PanelControl1)
        Me.Name = "frLUBarangD"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Daftar Barang Detil "
        CType(Me.PanelControl1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.PanelControl1.ResumeLayout(False)
        CType(Me.PanelControl2, System.ComponentModel.ISupportInitialize).EndInit()
        Me.PanelControl2.ResumeLayout(False)
        CType(Me.PanelControl3, System.ComponentModel.ISupportInitialize).EndInit()
        Me.PanelControl3.ResumeLayout(False)
        CType(Me.PanelControl4, System.ComponentModel.ISupportInitialize).EndInit()
        Me.PanelControl4.ResumeLayout(False)
        CType(Me.GridControl1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.GridView1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RepositoryItemCheckEdit1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RepositoryItemLookUpEdit1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

#End Region
    Public IDBarangD As Long
    Public IDBarang As Long
    Public Kode As String
    Public Nama As String
    Public barcode As String
    Dim BolehAmbilData As Boolean 
    Public Sub TampilData()
        Dim oConn As New SqlConnection
        Dim ocmd As SqlCommand
        Dim oDA As SqlDataAdapter
        Dim oDS As New DataSet
        Dim strsql As String
        'Dim isAda As Boolean
        Dim curentcursor As Cursor = Windows.Forms.Cursor.Current
        Windows.Forms.Cursor.Current = Cursors.WaitCursor
        strsql = "Select MBarangD.NoID as IDBarangD,MBarangD.IDBarang,MBarang.Kode,Mbarang.Nama,MBarangD.Barcode " & _
        "FROM MBarangD inner join Mbarang on MBarangD.IDBarang=MBarang.NoID WHERE MBarangD.IsActive=1 and MBarang.IsActive=1 "
        Try
            oConn.ConnectionString = StrKonSql
            ocmd = New SqlCommand(strsql, oConn)
            oConn.Open()
            oDA = New SqlDataAdapter(ocmd)
            oDA.Fill(oDS, "MAlamat")
        Catch ex As Exception
            XtraMessageBox.Show(Me, ex.Message, ".:: Pesan Kesalahan ::.", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            oConn.Close()
        End Try
        GridControl1.DataSource = oDS.Tables("MAlamat")
        Windows.Forms.Cursor.Current = curentcursor
        Application.DoEvents()
        GridView1.FocusedRowHandle = GridView1.LocateByValue("IDBarangD", IDBarangD)
    End Sub


    Private Sub SimpleButton5_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim view As ColumnView = GridControl1.FocusedView
        'Try
        Dim row As System.Data.DataRow = view.GetDataRow(GridView1.FocusedRowHandle)
        Dim NoID As Long = row("IDBarangD")
        'HapusData(Noid, "MASTSTNK")
        If XtraMessageBox.Show(Me, "Yakin Mau Hapus data ini?", ".:: Hapus Data Barang::.", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
            EksekusiSQL("Update MBarangD set IsActive=0 where NoID=" & IDBarangD)
            TampilData()
        End If
    End Sub


    Private Sub GridControl1_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs)
        AmbilData()
    End Sub
    Sub AmbilData()
        Dim view As ColumnView = GridControl1.FocusedView
        Dim row As System.Data.DataRow = view.GetDataRow(GridView1.FocusedRowHandle)
        IDBarangD = NullToLong(row("IDBarangD"))
        IDBarang = NullToLong(row("IDBarang"))
        Kode = NullToStr(row("Kode"))
        Nama = NullTostr(row("Nama"))
        barcode = NullToStr(row("Barcode"))
        DialogResult = Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub

    Private Sub cmdCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdCancel.Click
        DialogResult = Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub cmdOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdOK.Click
        AmbilData()
    End Sub

    Private Sub cmdEdit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdEdit.Click

    End Sub

    Private Sub cmdRefresh_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdRefresh.Click
        TampilData()
    End Sub

    Private Sub SimpleButton1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SimpleButton1.Click
        TampilData()
    End Sub

    Private Sub GridControl1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles GridControl1.Click

    End Sub

    Private Sub GridControl1_DoubleClick1(ByVal sender As Object, ByVal e As System.EventArgs) Handles GridControl1.DoubleClick
        If BolehAmbilData Then
            AmbilData()
        End If

    End Sub

    Private Sub GridControl1_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles GridControl1.KeyDown
        If e.KeyCode = Keys.Enter Then
            AmbilData()
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

    Private Sub frLUAlamat_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        GridView1.SaveLayoutToXml(Application.StartupPath & "\System\Layouts\DaftarAlamatNF.xml")

    End Sub

    Private Sub frDaftarAlamat_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        TampilData()
        If System.IO.File.Exists(Application.StartupPath & "\System\Layouts\DaftarAlamatNF.xml") Then
            GridView1.RestoreLayoutFromXml(Application.StartupPath & "\System\Layouts\DaftarAlamatNF.xml")
        End If
    End Sub

    Private Sub cmdHapus_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdHapus.Click

    End Sub
End Class

