Imports System.Data
Imports System.Data.SqlClient
Imports DevExpress.XtraEditors
Imports DevExpress.Utils
Imports DevExpress.XtraGrid.Views.Base
Imports System.IO.File

Public Class frmEntriMutasiGudang
    Dim SQL As String = ""
    Public Enum pStatus
        Baru = 0
        Edit = 1
    End Enum
    Public pTipe As pStatus
    Public IsPosted As Boolean = False
    Public NoID As Long = -1
    Dim oda2 As SqlDataAdapter
    Dim KodeLama As String = ""
    Dim oDS As New DataSet
    Dim BS As New BindingSource
    Dim IDAdmin As Long = -1
    'For Back Action
    Public FormNameDaftar As String = ""
    Public TableNameDaftar As String = ""
    Public TextDaftar As String = ""
    Public FormEntriDaftar As String = ""
    Public TableMasterDaftar As String = ""

    Private Sub RefreshDataKontak()
        Dim ds As New DataSet
        Try
            SQL = "SELECT MGudang.NoID, MGudang.Kode + ' - ' + MGudang.Nama AS Gudang, MWilayah.Nama AS Wilayah" & _
                  " FROM MGudang LEFT JOIN MWilayah ON MWilayah.NoID=MGudang.IDWilayah where MGudang.IsActive=1"
            '"MRuleMutasiD(" & _")
            '     " INNER JOIN MRuleMutasi ON MRuleMutasi.NoID=MRuleMutasiD.IDRuleMutasi " & _
            '     " LEFT JOIN (MGudang LEFT JOIN MWilayah ON MWilayah.NoID=MGudang.IDWilayah) ON MGudang.NoID=MRuleMutasiD.IDGudangAsal" & _
            '     " WHERE MGudang.IsActive = 1 And MRuleMutasi.IDPegawai = " & NullToLong(DefIDPegawai) & " And MRuleMutasiD.IsAsal = 1 And MRuleMutasi.IsActive = 1  And MRuleMutasiD.IsActive = 1 "
            ds = ExecuteDataset("master", SQL)
            txtGudangAsal.Properties.DataSource = ds.Tables("master")
            txtGudangAsal.Properties.ValueMember = "NoID"
            txtGudangAsal.Properties.DisplayMember = "Gudang"

            SQL = "SELECT MGudang.NoID, MGudang.Kode + ' - ' + MGudang.Nama AS Gudang, MWilayah.Nama AS Wilayah" & _
                  " FROM MGudang LEFT JOIN MWilayah ON MWilayah.NoID=MGudang.IDWilayah where MGudang.IsActive=1"
            '"MRuleMutasiD(" & _")
            '    " INNER JOIN MRuleMutasi ON MRuleMutasi.NoID=MRuleMutasiD.IDRuleMutasi " & _
            '    " LEFT JOIN (MGudang LEFT JOIN MWilayah ON MWilayah.NoID=MGudang.IDWilayah) ON MGudang.NoID=MRuleMutasiD.IDGudangTujuan" & _
               '   " WHERE MGudang.IsActive = 1 And MRuleMutasi.IDPegawai = " & NullToLong(DefIDPegawai) & " And MRuleMutasiD.IsAsal = 0 And MRuleMutasi.IsActive = 1  And MRuleMutasiD.IsActive = 1 AND MRuleMutasiD.IDGudangAsal=" & NullToLong(txtGudangAsal.EditValue)
            ds = ExecuteDataset("master", SQL)
            txtGudangTujuan.Properties.DataSource = ds.Tables("master")
            txtGudangTujuan.Properties.ValueMember = "NoID"
            txtGudangTujuan.Properties.DisplayMember = "Gudang"
        Catch ex As Exception
            XtraMessageBox.Show("Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            ds.Dispose()
        End Try
    End Sub
    Dim DefImageList As New ImageList
    Dim IsTempInsertBaru As Boolean = False

    Private Sub frmEntriMutasiGudang_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        If IsTempInsertBaru Then
            If XtraMessageBox.Show("Yakin ingin keluar?" & vbCrLf & "Ya, akan menghapus transaksi dan detil item.", NamaAplikasi, MessageBoxButtons.YesNo, MessageBoxIcon.Information, MessageBoxDefaultButton.Button2) = Windows.Forms.DialogResult.Yes Then
                EksekusiSQL("DELETE FROM MMutasiGudang WHERE NoID=" & NoID)
                EksekusiSQL("DELETE FROM MMutasiGudangD WHERE IDMutasiGudang=" & NoID)
            Else
                e.Cancel = True
            End If
        End If
    End Sub

    Private Sub frmEntriPO_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim dlg As DevExpress.Utils.WaitDialogForm = Nothing
        Try
            Dim curentcursor As Cursor = Windows.Forms.Cursor.Current
            Windows.Forms.Cursor.Current = Cursors.WaitCursor
            dlg = New DevExpress.Utils.WaitDialogForm(String.Format("Creating component and analize database.{0}MOHON TUNGGU ...", vbCrLf), Application.ProductName)
            dlg.TopMost = False
            dlg.Show()
            Windows.Forms.Cursor.Current = Cursors.WaitCursor
            SetTombol()
            RefreshData()
            SetTombol()
            Me.Width = Ini.BacaIniPath(FolderLayouts & Me.Name & ".ini", "Form", "Width", Me.Width)
            Me.Height = Ini.BacaIniPath(FolderLayouts & Me.Name & ".ini", "Form", "Height", Me.Height)

            Me.Left = (Screen.PrimaryScreen.WorkingArea.Width - Me.Width) \ 2
            Me.Top = (Screen.PrimaryScreen.WorkingArea.Height - Me.Height) \ 2

            If System.IO.File.Exists(FolderLayouts & Me.Name & LayoutControl1.Name & ".xml") Then
                LayoutControl1.RestoreLayoutFromXml(FolderLayouts & Me.Name & LayoutControl1.Name & ".xml")
            End If
            If System.IO.File.Exists(FolderLayouts & Me.Name & GV1.Name & ".xml") Then
                GV1.RestoreLayoutFromXml(FolderLayouts & Me.Name & GV1.Name & ".xml")
            End If
            LoadLayout()
            FungsiControl.SetForm(Me)
            Tgl.Properties.Mask.MaskType = Mask.MaskType.DateTime
            Tgl.Properties.Mask.EditMask = "dd-MM-yyyy HH:mm"
        Catch ex As Exception
            XtraMessageBox.Show(ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            Windows.Forms.Cursor.Current = Cursors.Default
            dlg.Close()
            dlg.Dispose()
        End Try
    End Sub
    Private Sub RefreshData()
        Dim DS As New DataSet
        Try
            RefreshDataKontak()
            If pTipe = pStatus.Baru Then
                IsiDefault()
            Else
                SQL = "SELECT MMutasiGudang.*, MGudangAsal.Nama AS GudangAsal, MGudangTujuan.Nama AS GudangTujuan"
                SQL &= " FROM MMutasiGudang LEFT OUTER JOIN"
                SQL &= " MGudang AS MGudangAsal ON MMutasiGudang.IDGudangAsal = MGudangAsal.NoID LEFT OUTER JOIN"
                SQL &= " MGudang AS MGudangTujuan ON MMutasiGudang.IDGudangTujuan = MGudangTujuan.NoID"
                SQL &= " WHERE MMutasiGudang.NoID = " & NoID
                DS = ExecuteDataset("Tabel", SQL)
                If DS.Tables("Tabel").Rows.Count >= 1 Then
                    txtKode.Text = NullToStr(DS.Tables(0).Rows(0).Item("Kode"))
                    KodeLama = txtKode.Text
                    Tgl.DateTime = NullToDate(DS.Tables(0).Rows(0).Item("Tanggal"))
                    txtGudangAsal.EditValue = NullToLong(DS.Tables(0).Rows(0).Item("IDGudangAsal"))
                    RubahGudang()
                    txtGudangTujuan.EditValue = NullToLong(DS.Tables(0).Rows(0).Item("IDGudangTujuan"))
                    IsPosted = NullToBool(DS.Tables(0).Rows(0).Item("IsPosted"))
                    txtCatatan.Text = NullToStr(DS.Tables(0).Rows(0).Item("Keterangan"))
                Else
                    IsiDefault()
                End If
            End If
        Catch ex As Exception
            XtraMessageBox.Show("Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            RefreshDetil()
            DS.Dispose()
        End Try
    End Sub
    Public Sub RefreshDetil()
        Dim strsql As String
        Dim curentcursor As Cursor = Windows.Forms.Cursor.Current
        Dim repChekEdit As New DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit
        Windows.Forms.Cursor.Current = Cursors.WaitCursor
        Try
            strsql = "SELECT MMutasiGudangD.*,MBarangD.Barcode, MBarang.Kode AS KodeBarang, MBarang.Nama AS NamaBarang, MSatuan.Kode AS Satuan" & vbCrLf & _
                     " FROM MMutasiGudangD" & vbCrLf & _
                     " LEFT JOIN MBarangD ON MBarangD.NoID=MMutasiGudangD.IDBarangD" & vbCrLf & _
                     " LEFT JOIN MBarang ON MBarang.NoID=MMutasiGudangD.IDBarang" & vbCrLf & _
                     " LEFT JOIN MSatuan ON MSatuan.NoID=MMutasiGudangD.IDSatuan" & vbCrLf & _
                     " where MMutasiGudangD.IDMutasiGudang = " & NoID
            ExecuteDBGrid(GC1, strsql, "NoID")
            'SetGridView(GC1)
            For x As Integer = 0 To GV1.Columns.Count - 1
                Select Case GV1.Columns(x).ColumnType.Name.ToLower
                    Case "int32", "int64", "int"
                        GV1.Columns(x).DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
                        GV1.Columns(x).DisplayFormat.FormatString = "n0"
                    Case "decimal", "single", "money", "double"
                        Select Case GV1.Columns(x).FieldName.Trim.ToLower
                            Case "discpersen1", "discpersen2", "discpersen3"
                                GV1.Columns(x).DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
                                GV1.Columns(x).DisplayFormat.FormatString = "n2"
                            Case Else
                                GV1.Columns(x).DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
                                GV1.Columns(x).DisplayFormat.FormatString = "n2"
                        End Select
                    Case "string"
                        GV1.Columns(x).DisplayFormat.FormatType = DevExpress.Utils.FormatType.None
                        GV1.Columns(x).DisplayFormat.FormatString = ""
                    Case "date", "datetime"
                        If GV1.Columns(x).FieldName.Trim.ToLower = "jam" Then
                            GV1.Columns(x).DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime
                            GV1.Columns(x).DisplayFormat.FormatString = "HH:mm"
                        Else
                            GV1.Columns(x).DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime
                            GV1.Columns(x).DisplayFormat.FormatString = "dd-MM-yyyy"
                        End If
                    Case "boolean"
                        GV1.Columns(x).ColumnEdit = repChekEdit
                End Select
            Next
            GV1.HideFindPanel()
        Catch ex As Exception
            XtraMessageBox.Show(ex.Message, ".:: Pesan Kesalahan ::.", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
        Windows.Forms.Cursor.Current = curentcursor
        Application.DoEvents()
    End Sub
    Private Sub IsiDefault()
        Tgl.DateTime = TanggalSystem
        SetTombol()
        'txtGudangAsal.EditValue = DefIDGudang
    End Sub

    Private Sub BarButtonItem1_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem1.ItemClick
        cmdSave.PerformClick()
    End Sub

    Private Sub cmdTutup_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdTutup.Click
        DialogResult = Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub
    Private Sub SetTombol()
        DefImageList = frmMain.ImageList1

        cmdBAru.ImageList = DefImageList
        cmdBAru.ImageIndex = 1

        cmdEdit.ImageList = DefImageList
        cmdEdit.ImageIndex = 2

        cmdDelete.ImageList = DefImageList
        cmdDelete.ImageIndex = 4

        cmdRefresh.ImageList = DefImageList
        cmdRefresh.ImageIndex = 5

        cmdSave.ImageList = DefImageList
        cmdSave.ImageIndex = 6

        cmdTutup.ImageList = DefImageList
        cmdTutup.ImageIndex = 3

        If pTipe = pStatus.Baru Or IsPosted Then
            cmdBAru.Enabled = False
            cmdEdit.Enabled = False
            cmdDelete.Enabled = False
        Else
            cmdBAru.Enabled = True
            cmdEdit.Enabled = True
            cmdDelete.Enabled = True
        End If
    End Sub
    Private Sub cmdSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSave.Click
        Dim PesanSalah As String = ""
        If Not IsPosted Then
            If IsValidasi() Then
                If Simpan(PesanSalah) = True Then
                    If pTipe = pStatus.Baru Then
                        KodeLama = txtKode.Text
                        pTipe = pStatus.Edit
                        SetTombol()
                        txtBarang.Focus()
                        IsTempInsertBaru = True
                        'txtKodeSupplier.Properties.ReadOnly = True
                    Else
                        clsPostingPembelian.PostingStokBarangMutasiGudang(NoID)
                        'KODE DIBAWAH UNTUK MEMBUKA FORM HANYA 1
                        Dim frmEntri As frmDaftarMasterDetil = Nothing
                        Dim F As Object
                        For Each F In MdiParent.MdiChildren
                            If TypeOf F Is frmDaftarMasterDetil Then
                                frmEntri = F
                                If frmEntri.FormName = FormNameDaftar Then
                                    Exit For
                                Else
                                    frmEntri = Nothing
                                End If
                            End If
                        Next
                        If frmEntri Is Nothing Then
                            frmEntri = New frmDaftarMasterDetil
                            frmEntri.WindowState = FormWindowState.Maximized
                            frmEntri.MdiParent = Me
                            frmEntri.FormName = FormNameDaftar
                            frmEntri.TableName = TableNameDaftar
                            frmEntri.Text = TextDaftar
                            frmEntri.FormEntriName = FormEntriDaftar
                            frmEntri.TableMaster = TableMasterDaftar
                        End If
                        frmEntri.DirectNoID = NoID
                        frmEntri.ShowNoID = True
                        frmEntri.Show()
                        frmEntri.Focus()

                        IsTempInsertBaru = False
                        'DialogResult = Windows.Forms.DialogResult.OK
                        Close()
                        Dispose()
                    End If
                Else
                    XtraMessageBox.Show("Info Kesalahan : " & vbCrLf & PesanSalah, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                End If
            End If
        End If
    End Sub
    Private Function IsValidasi() As Boolean
        If txtKode.Text = "" Then
            XtraMessageBox.Show("Kode masih kosong.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
            txtKode.Focus()
            Return False
            Exit Function
        End If
        If txtGudangTujuan.Text = "" Then
            XtraMessageBox.Show("Gudang Tujuan masih kosong.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
            txtGudangTujuan.Focus()
            Return False
            Exit Function
        End If
        If txtGudangAsal.Text = "" Then
            XtraMessageBox.Show("Gudang Asal masih kosong.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
            txtGudangAsal.Focus()
            Return False
            Exit Function
        End If
        If Tgl.Text = "" Then
            XtraMessageBox.Show("Tanggal masih kosong.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
            Tgl.Focus()
            Return False
            Exit Function
        End If
        If CekKodeValid(txtKode.Text, KodeLama, "MMutasiGudang", "Kode", IIf(pTipe = pStatus.Edit, True, False)) Then
            XtraMessageBox.Show("Kode sudah dipakai.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
            txtKode.Focus()
            If pTipe = pStatus.Baru Then
                txtKode.Text = clsKode.MintaKodeBaru("MTS", "MMutasiGudang", Tgl.DateTime, DefIDWilayah, 5)
            End If
            Return False
            Exit Function
        End If
        Return True
    End Function
    Public Function Simpan(ByRef PesanSalah As String) As Boolean
        Dim Sukses As Boolean = False
        Try
            If pTipe = pStatus.Baru Then
                NoID = GetNewID("MMutasiGudang")
                SQL = "INSERT INTO [MMutasiGudang] ([IDWilayah],[NoID],[Kode],[Nomor],[Tanggal],[Jam],[IsPosted],[IDGudangAsal],[IDGudangTujuan],[Keterangan],[IDUser]) VALUES (" & vbCrLf
                SQL &= DefIDWilayah & ","
                SQL &= NoID & ","
                SQL &= "'" & FixApostropi(txtKode.Text) & "',"
                SQL &= GetNewID("MMutasiGudang", "Nomor") & ","
                SQL &= "'" & Tgl.DateTime.ToString("yyyy-MM-dd HH:mm") & "',"
                SQL &= "'" & Tgl.DateTime.ToString("HH:mm") & "',"
                SQL &= "0,"
                SQL &= NullToLong(txtGudangAsal.EditValue) & ","
                SQL &= NullToLong(txtGudangTujuan.EditValue) & ",'"
                SQL &= FixApostropi(txtCatatan.Text) & "'," & IDUserAktif & ")"
                EksekusiSQL(SQL)
            Else
                SQL = "UPDATE MMutasiGudang SET "
                SQL &= " IDWilayah=" & DefIDWilayah & ","
                SQL &= " Kode='" & FixApostropi(txtKode.Text) & "',"
                SQL &= " Tanggal='" & Tgl.DateTime.ToString("yyyy-MM-dd HH:mm") & "',"
                SQL &= " Jam='" & Tgl.DateTime.ToString("HH:mm") & "',"
                SQL &= " IDGudangAsal=" & NullToLong(txtGudangAsal.EditValue) & ","
                SQL &= " IDGudangTujuan=" & NullToLong(txtGudangTujuan.EditValue) & ","
                SQL &= " Keterangan='" & FixApostropi(txtCatatan.Text) & "',"
                SQL &= " IDUser=" & IDUserAktif
                SQL &= " WHERE NoID=" & NoID
                EksekusiSQL(SQL)
            End If
            Sukses = True
        Catch ex As Exception
            PesanSalah = ex.Message
            Sukses = False
        Finally

        End Try
        Return Sukses
    End Function
    Private Sub LoadLayout()
        If Exists(FolderLayouts & Me.Name & gvGudangAsal.Name & ".xml") Then
            gvGudangAsal.RestoreLayoutFromXml(FolderLayouts & Me.Name & gvGudangAsal.Name & ".xml")
        End If
        If Exists(FolderLayouts & Me.Name & GV1.Name & ".xml") Then
            GV1.RestoreLayoutFromXml(FolderLayouts & Me.Name & GV1.Name & ".xml")
        End If
        If Exists(FolderLayouts & Me.Name & gvGudangTujuan.Name & ".xml") Then
            gvGudangTujuan.RestoreLayoutFromXml(FolderLayouts & Me.Name & gvGudangTujuan.Name & ".xml")
        End If
        If Exists(FolderLayouts & Me.Name & LayoutControl1.Name & ".xml") Then
            LayoutControl1.RestoreLayoutFromXml(FolderLayouts & Me.Name & LayoutControl1.Name & ".xml")
        End If
        If Exists(FolderLayouts & Me.Name & LayoutControl1.Name & ".xml") Then
            LayoutControl1.RestoreLayoutFromXml(FolderLayouts & Me.Name & LayoutControl1.Name & ".xml")
        End If
    End Sub
    Private Sub BarButtonItem3_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem3.ItemClick
        Dim xOtorisasi As New frmOtorisasiAdmin
        Try
            If XtraMessageBox.Show("Simpan Layout?", NamaAplikasi, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = Windows.Forms.DialogResult.Yes AndAlso xOtorisasi.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
                Ini.TulisIniPath(FolderLayouts & Me.Name & ".ini", "Form", "Width", Me.Width)
                Ini.TulisIniPath(FolderLayouts & Me.Name & ".ini", "Form", "Height", Me.Height)
                Me.Left = (Screen.PrimaryScreen.WorkingArea.Width - Me.Width) \ 2
                Me.Top = (Screen.PrimaryScreen.WorkingArea.Height - Me.Height) \ 2

                LayoutControl1.SaveLayoutToXml(FolderLayouts & Me.Name & LayoutControl1.Name & ".xml")
                GV1.SaveLayoutToXml(FolderLayouts & Me.Name & GV1.Name & ".xml")
                gvGudangTujuan.SaveLayoutToXml(FolderLayouts & Me.Name & gvGudangTujuan.Name & ".xml")
                gvGudangAsal.SaveLayoutToXml(FolderLayouts & Me.Name & gvGudangAsal.Name & ".xml")
            End If
        Catch ex As Exception
            XtraMessageBox.Show("Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            xOtorisasi.Dispose()
        End Try
    End Sub

    Private Sub cmdRefresh_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdRefresh.Click
        RefreshDetil()
    End Sub

    Private Sub cmdDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdDelete.Click
        Dim view As ColumnView = GC1.FocusedView
        Try
            Dim row As System.Data.DataRow = view.GetDataRow(GV1.FocusedRowHandle)
            Dim dc As Integer = GV1.FocusedRowHandle
            Dim IDDetil As Long = NullToLong(row("NoID"))
            If XtraMessageBox.Show("Item ini mau dihapus?", NamaAplikasi, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) = Windows.Forms.DialogResult.Yes Then
                EksekusiSQL("Delete From MMutasiGudangD where NoID=" & IDDetil.ToString)
                RefreshDetil()
            End If
            txtBarang.Focus()
        Catch ex As Exception
            XtraMessageBox.Show("Untuk mengedit pilih item satuan yang akan diedit terlebih dahulu lalu tekan tombol edit", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub txtBarang_ButtonClick(ByVal sender As Object, ByVal e As DevExpress.XtraEditors.Controls.ButtonPressedEventArgs) Handles txtBarang.ButtonClick
        If pTipe = pStatus.Baru Then Exit Sub
        Select Case e.Button.Index
            Case 0
                InsertIntoDetil()
                txtBarang.Text = ""
                txtBarang.Focus()
            Case 1
                txtBarang.Text = ""
        End Select
    End Sub
    Private Function CariBarang(ByRef IDBarangD As Long, ByRef NamaBarang As String, ByRef KodeBarang As String, ByRef Barcode As String, ByRef Konversi As Double, ByRef IDSatuan As Integer, ByRef HargaJual As Double) As Boolean
        Dim x As Boolean = False
        Dim SQL As String = ""
        Dim oDS As New DataSet
        Try
            SQL = "SELECT MBarangD.NoID,MBarangD.IDBarang,MBarangD.IDSatuan,MBarangD.Konversi,MBarangD.HargaJualA,MBarangD.Barcode,MBarang.Kode,MBarang.Nama FROM MBarangD inner join MBarang  on MBarangD.IDBarang=Mbarang.NoID WHERE MBarang.IsActive=1 and (MBarangD.Barcode='" & NamaBarang.Replace("'", "''").ToUpper & "' OR MBarang.Kode = '" & NamaBarang.Replace("'", "''").ToUpper & "') ORDER BY Kode,Konversi ASC"
            oDS = ExecuteDataset("Tbl", SQL)
            If oDS.Tables("Tbl").Rows.Count >= 1 Then
                NamaBarang = NullToStr(oDS.Tables(0).Rows(0).Item("Nama"))
                KodeBarang = NullToStr(oDS.Tables(0).Rows(0).Item("Kode"))
                IDBarangD = NullToLong(oDS.Tables(0).Rows(0).Item("NoID"))
                IDSatuan = NullToLong(oDS.Tables(0).Rows(0).Item("IDSatuan"))
                Konversi = NullToDbl(oDS.Tables(0).Rows(0).Item("Konversi"))
                HargaJual = NullToDbl(oDS.Tables(0).Rows(0).Item("HargaJualA"))
                Barcode = NullToStr(oDS.Tables(0).Rows(0).Item("Barcode"))
                x = True
            Else
                x = False
            End If
        Catch ex As Exception
            x = False
        End Try
        Return x
    End Function
    Private Sub InsertIntoDetil()
        Dim SQL As String = ""
        Dim NamaBarang As String = txtBarang.Text
        Dim KodeBarang As String = ""
        Dim IDBarang As Long = -1
        Dim IDBarangD As Long = -1
        Dim IDDetil As Long = -1
        Dim frmEntri As New frmEntriMutasiGudangD
        Dim Konversi As Double = 0.0
        Dim Barcode As String = ""
        Dim IDSatuan As Integer
        Dim HargaJual As Double
        Try
            If txtGudangAsal.Text = "" Then Exit Sub
            If txtGudangTujuan.Text = "" Then Exit Sub

            If CariBarang(IDBarangD, NamaBarang, KodeBarang, Barcode, Konversi, IDSatuan, HargaJual) Then
                ''If XtraMessageBox.Show("Ingin menambah Item Mutasi Gudang dengan Kode Stock " & KodeBarang & " Nama Stock " & NamaBarang, "Fast Entri Says", MessageBoxButtons.YesNo) = Windows.Forms.DialogResult.Yes Then
                'IDDetil = GetNewID("MMutasiGudangD", "NoID")
                ''Konversi = NullToDbl(EksekusiSQlSkalarNew("SELECT Konversi FROM MBarangD WHERE IDBarang=" & IDBarang & " AND IDSatuan=" & DefIDSatuan))
                ''If Konversi <> 0 Then
                'SQL = "INSERT INTO MMutasiGudangD (NoID,IDBarangD,IDBarang,IDSatuan,QtyGudangPcs,Konversi,HargaJual,IDMutasiGudang,NoUrut,Tanggal,Jam) VALUES " & vbCrLf
                'SQL &= "(" & IDDetil & "," & IDBarangD & "," & IDBarang & "," & IDSatuan & "," & FixKoma(clsPostingKartuStok.CekSaldoStockVerian(NullToLong(txtGudangAsal.EditValue), IDBarang, Tgl.DateTime)) & "," & FixKoma(Konversi) & "," & FixKoma(HargaJual) & "," & NoID & "," & GetNewID("MMutasiGudangD", "NoUrut", " WHERE IDMutasiGudang=" & NoID) & ",GetDate(),GetDate())"
                ''Else
                ''    SQL = "INSERT INTO MMutasiGudangD (NoID,IDBarang,IDMutasiGudang,NoUrut,Tanggal,Jam) VALUES " & vbCrLf
                ''    SQL &= "(" & IDDetil & "," & IDBarang & "," & NoID & "," & GetNewID("MMutasiGudangD", "NoUrut", " WHERE IDMutasiGudang=" & NoID) & ",GetDate(),GetDate())"
                ''End If
                'EksekusiSQL(SQL)
                frmEntri.IsNew = True
                frmEntri.NoID = IDDetil
                frmEntri.IDMutasiGudang = NoID
                frmEntri.IDGudang = NullToLong(txtGudangAsal.EditValue)
                frmEntri.Tgl = Tgl.DateTime
                frmEntri.IsFastEntri = False
                frmEntri.FormPemanggil = Me
                frmEntri.txtBarcode.EditValue = IDBarangD
                frmEntri.Show()
                frmEntri.Focus()
                'If frmEntri.ShowDialog(Me)= Windows.Forms.DialogResult.OK Then
                '    RefreshDetil()
                '    GV1.ClearSelection()
                '    GV1.FocusedRowHandle = GV1.LocateByDisplayText(0, GV1.Columns("NoID"), IDDetil.ToString("#,##0"))
                '    GV1.SelectRow(GV1.FocusedRowHandle)
                'Else
                '    SQL = "DELETE FROM MMutasiGudangD WHERE NoID=" & IDDetil
                '    EksekusiSQL(SQL)
                '    RefreshDetil()
                '    GV1.ClearSelection()
                '    GV1.FocusedRowHandle = GV1.LocateByDisplayText(0, GV1.Columns("NoID"), IDDetil.ToString("#,##0"))
                '    GV1.SelectRow(GV1.FocusedRowHandle)
                'End If
                'End If
            End If
        Catch ex As Exception
            DevExpress.XtraEditors.XtraMessageBox.Show("Kesalahan : " & ex.Message, "Fast Entri Says", MessageBoxButtons.OK)
            'frmEntri.Dispose()
        End Try
    End Sub

    Private Sub cmdBAru_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdBAru.Click
        Dim frmEntri As New frmEntriMutasiGudangD
        Try
            Dim IDDetil As Long = -1
            frmEntri.NoID = IDDetil
            frmEntri.IsNew = True
            frmEntri.IDMutasiGudang = NoID
            If txtGudangAsal.Text <> "" Then
                frmEntri.IDGudang = NullToLong(txtGudangAsal.EditValue)
                frmEntri.Tgl = Tgl.DateTime
                frmEntri.NoID = IDDetil
                frmEntri.IsNew = True
                frmEntri.IDMutasiGudang = NoID
                frmEntri.FormPemanggil = Me
                frmEntri.Show()
                frmEntri.Focus()
                'If frmEntri.ShowDialog(Me)= Windows.Forms.DialogResult.OK Then
                '    RefreshDetil()
                'End If
                'txtBarang.Focus()
            Else
                XtraMessageBox.Show("Isi dulu Gudang, lalu click baru.", NamaAplikasi, MessageBoxButtons.OK)
            End If
        Catch ex As Exception
            XtraMessageBox.Show("Untuk membuat baru tekan tombol baru", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            'frmEntri.Dispose()
        End Try
    End Sub

    Private Sub cmdEdit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdEdit.Click
        Dim view As ColumnView = GC1.FocusedView
        Dim frmEntri As New frmEntriMutasiGudangD
        Try
            Dim row As System.Data.DataRow = view.GetDataRow(GV1.FocusedRowHandle)
            Dim dc As Integer = GV1.FocusedRowHandle
            Dim IDDetil As Long = NullToLong(row("NoID"))
            If txtGudangAsal.Text <> "" Then
                frmEntri.IDGudang = NullToLong(txtGudangAsal.EditValue)
                frmEntri.Tgl = Tgl.DateTime
                frmEntri.IsNew = False
                frmEntri.NoID = IDDetil
                frmEntri.IDMutasiGudang = NoID
                frmEntri.FormPemanggil = Me
                frmEntri.Show()
                frmEntri.Focus()
                'If frmEntri.ShowDialog(Me)= Windows.Forms.DialogResult.OK Then
                '    RefreshDetil()
                'End If
                'txtBarang.Focus()
            Else
                XtraMessageBox.Show("Isi dulu gudang, lalu click Edit.", NamaAplikasi, MessageBoxButtons.OK)
            End If
        Catch ex As Exception
            XtraMessageBox.Show("Untuk mengedit pilih item satuan yang akan diedit terlebih dahulu lalu tekan tombol edit", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            'frmEntri.Dispose()
        End Try
    End Sub

    Private Sub txtBarang_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtBarang.EditValueChanged

    End Sub

    Private Sub BarButtonItem2_ItemClick_1(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem2.ItemClick
        If cmdBAru.Enabled Then
            cmdBAru.PerformClick()
        End If
    End Sub

    Private Sub BarButtonItem4_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem4.ItemClick
        If cmdEdit.Enabled Then
            cmdEdit.PerformClick()
        End If
    End Sub

    Private Sub BarButtonItem5_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem5.ItemClick
        If cmdDelete.Enabled Then
            cmdDelete.PerformClick()
        End If
    End Sub

    Private Sub BarButtonItem6_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem6.ItemClick
        cmdRefresh.PerformClick()

    End Sub

    Private Sub BarButtonItem7_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem7.ItemClick
        cmdTutup.PerformClick()
    End Sub


    Private Sub txtGudang_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtGudangAsal.EditValueChanged
        RubahGudang()
    End Sub
    Private Sub RubahGudang()
        txtWilayahAsal.Text = NullToStr(EksekusiSQlSkalarNew("SELECT MWilayah.Nama FROM MGudang LEFT JOIN MWilayah ON MWilayah.NOID=MGudang.IDWilayah WHERE MGudang.NoID=" & NullToLong(txtGudangAsal.EditValue)))
        RefreshDataKontak()
        'txtGudangTujuan.EditValue = NullTolong(EksekusiSQlSkalarNew("SELECT MGudang.NoID FROM MGudang LEFT JOIN MWilayah ON MWilayah.NOID=MGudang.IDWilayah WHERE MGudang.NoID=" & NullTolong(txtGudangAsal.EditValue)))
    End Sub

    Private Sub Tgl_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Tgl.KeyDown
        If e.KeyCode = Keys.F7 Then
            Dim x As New frmOtorisasiAdmin
            Try
                If Tgl.Properties.ReadOnly Then
                    If x.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
                        IDAdmin = x.IDUserAdmin
                        Tgl.Properties.ReadOnly = False
                    End If
                End If
            Catch ex As Exception
                XtraMessageBox.Show("Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End If
    End Sub

    Private Sub txtBarang_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtBarang.KeyDown
        If pTipe = pStatus.Baru Then Exit Sub
        Select Case e.KeyCode
            Case Keys.Enter
                InsertIntoDetil()
                txtBarang.Text = ""
                'txtBarang.Focus()
            Case Keys.Escape
                txtBarang.Text = ""
        End Select
    End Sub

    Private Sub gvGudangAsal_DataSourceChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvGudangAsal.DataSourceChanged
        If System.IO.File.Exists(FolderLayouts & Me.Name & gvGudangAsal.Name & ".xml") Then
            gvGudangAsal.RestoreLayoutFromXml(FolderLayouts & Me.Name & gvGudangAsal.Name & ".xml")
        End If
        With gvGudangAsal
            For x As Integer = 0 To .Columns.Count - 1
                Select Case .Columns(x).ColumnType.Name.ToLower
                    Case "int32", "int64", "int"
                        .Columns(x).DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
                        .Columns(x).DisplayFormat.FormatString = "n0"
                    Case "decimal", "single", "money", "double"
                        .Columns(x).DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
                        .Columns(x).DisplayFormat.FormatString = "n2"
                    Case "string"
                        .Columns(x).DisplayFormat.FormatType = DevExpress.Utils.FormatType.None
                        .Columns(x).DisplayFormat.FormatString = ""
                    Case "date", "datetime"
                        If .Columns(x).FieldName.Trim.ToLower = "jam" Then
                            .Columns(x).DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime
                            .Columns(x).DisplayFormat.FormatString = "HH:mm"
                        Else
                            .Columns(x).DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime
                            .Columns(x).DisplayFormat.FormatString = "dd-MM-yyyy"
                        End If
                End Select
            Next
        End With
    End Sub

    Private Sub gvGudangTujuan_DataSourceChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvGudangTujuan.DataSourceChanged
        If System.IO.File.Exists(FolderLayouts & Me.Name & gvGudangTujuan.Name & ".xml") Then
            gvGudangTujuan.RestoreLayoutFromXml(FolderLayouts & Me.Name & gvGudangTujuan.Name & ".xml")
        End If
        With gvGudangTujuan
            For x As Integer = 0 To .Columns.Count - 1
                Select Case .Columns(x).ColumnType.Name.ToLower
                    Case "int32", "int64", "int"
                        .Columns(x).DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
                        .Columns(x).DisplayFormat.FormatString = "n0"
                    Case "decimal", "single", "money", "double"
                        .Columns(x).DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
                        .Columns(x).DisplayFormat.FormatString = "n2"
                    Case "string"
                        .Columns(x).DisplayFormat.FormatType = DevExpress.Utils.FormatType.None
                        .Columns(x).DisplayFormat.FormatString = ""
                    Case "date", "datetime"
                        If .Columns(x).FieldName.Trim.ToLower = "jam" Then
                            .Columns(x).DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime
                            .Columns(x).DisplayFormat.FormatString = "HH:mm"
                        Else
                            .Columns(x).DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime
                            .Columns(x).DisplayFormat.FormatString = "dd-MM-yyyy"
                        End If
                End Select
            Next
        End With
    End Sub

    Private Sub Tgl_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Tgl.EditValueChanged
        Try
            If pTipe = pStatus.Baru Then
                txtKode.Text = clsKode.MintaKodeBaru("MTS", "MMutasiGudang", Tgl.DateTime, DefIDWilayah, 5)
            End If
        Catch ex As Exception

        End Try
    End Sub

    Private Sub GC1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles GC1.Click

    End Sub

    Private Sub txtImportSoftCopy_ButtonClick(ByVal sender As Object, ByVal e As DevExpress.XtraEditors.Controls.ButtonPressedEventArgs) Handles txtImportSoftCopy.ButtonClick
        Try
            If e.Button.Index = 0 Then
                Dim x As New OpenFileDialog
                x.Title = "Buka File Excel"
                x.Filter = "Excel Files|*.xls"
                If x.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
                    txtImportSoftCopy.Text = x.FileName
                Else
                    txtImportSoftCopy.Text = ""
                End If
            ElseIf e.Button.Index = 1 Then
                If pTipe = pStatus.Edit AndAlso Not IsPosted AndAlso txtImportSoftCopy.Text <> "" AndAlso System.IO.File.Exists(txtImportSoftCopy.Text) Then 'Import
                    Dim cn As New OleDb.OleDbConnection
                    Dim com As New OleDb.OleDbCommand
                    Dim oDA As New OleDb.OleDbDataAdapter
                    Dim ds As New DataSet
                    Dim IDBarangD As Long, IDBarang As Long
                    Dim IDSatuan As Long, Konversi As Double
                    Dim SaldoGudangVarian As Double
                    Try
                        SQL = "SELECT [MMutasiGudang$].* FROM [MMutasiGudang$] WHERE CDBL([MMutasiGudang$].IsLoad)=0 ORDER BY [MMutasiGudang$].NoUrut "
                        cn.ConnectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=""" & txtImportSoftCopy.Text & """;Extended Properties=Excel 8.0;"
                        cn.Open()
                        com.Connection = cn
                        com.CommandText = SQL
                        If Not ds.Tables("Excel") Is Nothing Then
                            ds.Tables("Excel").Clear()
                        End If
                        oDA.SelectCommand = com
                        oDA.Fill(ds, "Excel")
                        For i As Integer = 0 To ds.Tables("Excel").Rows.Count - 1
                            With ds.Tables("Excel").Rows(i)
                                IDBarang = NullToLong(EksekusiSQlSkalarNew("SELECT MBarang.NoID FROM MBarang WHERE REPLACE(UPPER(MBarang.Kode),' ','')='" & FixApostropi(NullToStr(.Item("Kode"))).ToUpper.Replace(" ", "") & "'"))
                                IDBarangD = NullToLong(EksekusiSQlSkalarNew("SELECT MBarangD.NoID FROM MBarangD INNER JOIN MBarang ON MBarang.NoID=MBarangD.IDBarang WHERE REPLACE(UPPER(MBarangD.Barcode),' ','')='" & FixApostropi(NullToStr(.Item("Barcode"))).ToUpper.Replace(" ", "") & "' AND REPLACE(UPPER(MBarang.Kode),' ','')='" & FixApostropi(NullToStr(.Item("Kode"))).ToUpper.Replace(" ", "") & "'"))
                                IDSatuan = NullToLong(EksekusiSQlSkalarNew("SELECT IDSatuan FROM MBarangD WHERE NoID=" & NullToLong(IDBarangD)))
                                Konversi = NullToDbl(EksekusiSQlSkalarNew("SELECT Konversi FROM MBarangD WHERE NoID=" & NullToLong(IDBarangD)))
                                SaldoGudangVarian = clsPostingKartuStok.CekSaldoStockVerian(txtGudangAsal.EditValue, NullToLong(IDBarangD), Tgl.DateTime)

                                SQL = "INSERT INTO MMutasiGudangD (NoID,NoUrut,IDMutasiGudang,IDBarangD,IDBarang,Tanggal,Jam,IDSatuan,Konversi,Qty,HargaJual,Jumlah,QtyPcs,QtyGudangPcs,CTN,Operator,Keterangan,IDOperator) VALUES (" & vbCrLf & _
                                      NullToLong(GetNewID("MMutasiGudangD", "NoID")) & "," & vbCrLf & _
                                      NullToLong(GetNewID("MMutasiGudangD", "NoUrut", " WHERE IDMutasiGudang=" & NoID)) & "," & vbCrLf & _
                                      NoID & "," & vbCrLf & _
                                      IDBarangD & "," & vbCrLf & _
                                      IDBarang & "," & vbCrLf & _
                                      "GetDate()," & vbCrLf & _
                                      "GetDate()," & vbCrLf & _
                                      FixKoma(IDSatuan) & "," & vbCrLf & _
                                      FixKoma(Konversi) & "," & vbCrLf & _
                                      FixKoma(.Item("Qty")) & "," & vbCrLf & _
                                      FixKoma(.Item("HargaJual")) & "," & vbCrLf & _
                                      FixKoma(.Item("Jumlah")) & "," & vbCrLf & _
                                      FixKoma(.Item("Qty") * Konversi) & "," & vbCrLf & _
                                      FixKoma(SaldoGudangVarian) & "," & vbCrLf & _
                                      FixKoma(0) & "," & vbCrLf & _
                                      "'" & NullToStr(NamaUserAktif) & "'," & vbCrLf & _
                                      "'" & NullToStr(.Item("Keterangan")) & "'," & vbCrLf & _
                                      NullToLong(IDUserAktif) & vbCrLf & _
                                      ")"
                                EksekusiSQL(SQL)
                                SQL = "UPDATE [MMutasiGudang$] SET IsLoad=1 WHERE CLNG(NoUrut)=" & NullToLong(.Item("NoUrut"))
                                com.CommandText = SQL
                                com.ExecuteNonQuery()
                            End With
                            Application.DoEvents()
                        Next
                        RefreshDetil()
                    Catch ex As Exception
                        XtraMessageBox.Show("Info Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
                    Finally
                        If cn.State = ConnectionState.Open Then
                            cn.Close()
                        End If
                        cn.Dispose()
                        com.Dispose()
                        oDA.Dispose()
                        ds.Dispose()
                    End Try
                End If
            ElseIf e.Button.Index = 2 Then
                txtImportSoftCopy.Text = ""
            End If
        Catch ex As Exception
            XtraMessageBox.Show("Info Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
        End Try
    End Sub

    Private Sub txtImportSoftCopy_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtImportSoftCopy.EditValueChanged

    End Sub
End Class