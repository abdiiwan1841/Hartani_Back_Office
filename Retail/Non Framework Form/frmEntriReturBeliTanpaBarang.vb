Imports System.Data
Imports System.Data.SqlClient
Imports DevExpress.XtraEditors
Imports DevExpress.Utils
Imports DevExpress.XtraGrid.Views.Base

Public Class frmEntriReturBeliTanpaBarang
    Dim SQL As String = ""
    Public Enum pStatus
        Baru = 0
        Edit = 1
    End Enum
    Public pTipe As pStatus
    Public IsPosted As Boolean = False
    Public NoID As Long = -1
    Public IDWilayah As Long = DefIDWilayah

    Dim oda2 As SqlDataAdapter

    Dim oDS As New DataSet
    Dim BS As New BindingSource

    'For Back Action
    Public FormNameDaftar As String = ""
    Public TableNameDaftar As String = ""
    Public TextDaftar As String = ""
    Public FormEntriDaftar As String = ""
    Public TableMasterDaftar As String = ""
    Dim IDAdmin As Long = -1
    Dim IsTempInsertBaru As Boolean = False
    Dim IDBeli As Long = -1
    Private Sub RefreshNotaJual()
        Dim repChekEdit As New DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit

        Dim ds As New DataSet
        Try
            SQL = "select BL.*,ISNULL(RB.NilaiRetur,0) as Retur,ISNULL(BYR.NilaiBayar,0) as Bayar,BL.NilaiNota-ISNULL(RB.NilaiRetur,0)-ISNULL(BYR.NilaiBayar,0) as Sisa  from "
            SQL = SQL & "(select mbeli.Kode,NoID,mbeli.IDSupplier,mbeli.tanggal,mbeli.Total as NilaiNota from mbeli where isposted=1) BL "
            SQL = SQL & "left join (Select IDBeli,Sum(MReturBeli.Total) as NilaiRetur from MReturBeli "
            SQL = SQL & "group by IDBeli) RB on BL.NoID=RB.IDBeli "
            SQL = SQL & "left join (Select IDBeli,Sum(MbayarHutangD.Bayar) as NilaiBayar from MBayarHutangD inner join MBayarHutang "
            SQL = SQL & "on MBayarHutangD.IDbayarHutang=MbayarHutang.NoID where MBayarHutang.IsJual=0 "
            SQL = SQL & "group by IDBeli ) BYR on BL.NoID=BYR.IDBeli "
            SQL = SQL & "where (BL.NoID=" & IDBeli & " or BL.NilaiNota-ISNULL(RB.NilaiRetur,0)-ISNULL(BYR.NilaiBayar,0)<>0) "
            SQL = SQL & "and BL.IDSupplier= " & NullToLong(txtKodeSupplier.EditValue)
            ds = ExecuteDataset("MJual", SQL)
            txtNotaJual.Properties.DataSource = ds.Tables("MJual")

            For x As Integer = 0 To gvNotaJual.Columns.Count - 1
                Select Case gvNotaJual.Columns(x).ColumnType.Name.ToLower
                    Case "int32", "int64", "int"
                        gvNotaJual.Columns(x).DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
                        gvNotaJual.Columns(x).DisplayFormat.FormatString = "n0"
                    Case "decimal", "single", "money", "double"
                        gvNotaJual.Columns(x).DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
                        gvNotaJual.Columns(x).DisplayFormat.FormatString = "n2"
                    Case "string"
                        gvNotaJual.Columns(x).DisplayFormat.FormatType = DevExpress.Utils.FormatType.None
                        gvNotaJual.Columns(x).DisplayFormat.FormatString = ""
                    Case "date", "datetime"
                        If gvNotaJual.Columns(x).FieldName.Trim.ToLower = "jam" Then
                            gvNotaJual.Columns(x).DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime
                            gvNotaJual.Columns(x).DisplayFormat.FormatString = "HH:mm"
                        Else
                            gvNotaJual.Columns(x).DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime
                            gvNotaJual.Columns(x).DisplayFormat.FormatString = "dd-MM-yyyy"
                        End If
                    Case "boolean"
                        gvNotaJual.Columns(x).ColumnEdit = repChekEdit
                End Select
            Next
            'If System.IO.File.Exists(FolderLayouts & Me.Name & gvNotaJual.Name & ".xml") Then
            '    gvNotaJual.RestoreLayoutFromXml(FolderLayouts & Me.Name & gvNotaJual.Name & ".xml")
            'End If

        Catch ex As Exception
            XtraMessageBox.Show("Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            ds.Dispose()
        End Try
    End Sub
    Private Sub RefreshDataKontak()
        Dim ds As New DataSet
        Try
            SQL = "SELECT NoID, Kode, Nama FROM MAlamat WHERE IsActive=1 AND IsSupplier=1"
            ds = ExecuteDataset("master", SQL)
            txtKodeSupplier.Properties.DataSource = ds.Tables("master")
            If System.IO.File.Exists(folderLayouts & Me.Name & SearchLookUpEdit1View.Name & ".xml") Then
                SearchLookUpEdit1View.RestoreLayoutFromXml(folderLayouts & Me.Name & SearchLookUpEdit1View.Name & ".xml")
            End If

            SQL = "SELECT MGudang.NoID, MGudang.Kode, MGudang.Kode + ' - ' + MGudang.Nama AS Nama, MWilayah.Nama AS Wilayah FROM MGudang LEFT JOIN MWilayah ON MGudang.IDWilayah=MWilayah.NoID WHERE MGudang.IsActive=1 AND MGudang.InTransit=0 AND MGudang.IDWilayah=" & DefIDWilayah
            ds = ExecuteDataset("master", SQL)
            txtGudang.Properties.DataSource = ds.Tables("master")
            txtGudang.Properties.ValueMember = "NoID"
            txtGudang.Properties.DisplayMember = "Nama"
            If System.IO.File.Exists(folderLayouts & Me.Name & gvGudang.Name & ".xml") Then
                gvGudang.RestoreLayoutFromXml(folderLayouts & Me.Name & gvGudang.Name & ".xml")
            End If

            SQL = "SELECT MKategori.NoID, MKategori.Kode, MKategori.Nama, MParent.Nama AS KategoriUtama FROM MKategori LEFT JOIN MKategori MParent ON MParent.NoID=MKategori.IDParent WHERE MKategori.IsActive=1 AND MKategori.IDParent>=1"
            ds = ExecuteDataset("master", SQL)
            txtKategori.Properties.DataSource = ds.Tables("master")
            txtKategori.Properties.ValueMember = "NoID"
            txtKategori.Properties.DisplayMember = "Nama"
            If System.IO.File.Exists(FolderLayouts & Me.Name & gvKategori.Name & ".xml") Then
                gvKategori.RestoreLayoutFromXml(FolderLayouts & Me.Name & gvKategori.Name & ".xml")
            End If
        Catch ex As Exception
            XtraMessageBox.Show("Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            ds.Dispose()
        End Try
    End Sub
    Dim DefImageList As New ImageList

    Private Sub frmEntriReturBeli_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        'If IsTempInsertBaru Then
        '    If XtraMessageBox.Show("Yakin ingin keluar?" & vbCrLf & "Ya, akan menghapus transaksi dan detil item.", NamaAplikasi, MessageBoxButtons.YesNo, MessageBoxIcon.Information, MessageBoxDefaultButton.Button2) = Windows.Forms.DialogResult.Yes Then
        '        EksekusiSQL("DELETE FROM MReturBeli WHERE NoID=" & NoID)
        '        EksekusiSQL("DELETE FROM MReturBeliD WHERE IDReturBeli=" & NoID)
        '    Else
        '        e.Cancel = True
        '    End If
        'End If
    End Sub

    Private Sub frmEntriBeli_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
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
            Me.Width = Ini.BacaIniPath(folderLayouts & Me.Name & ".ini", "Form", "Width", Me.Width)
            Me.Height = Ini.BacaIniPath(folderLayouts & Me.Name & ".ini", "Form", "Height", Me.Height)

            Me.Left = (Screen.PrimaryScreen.WorkingArea.Width - Me.Width) \ 2
            Me.Top = (Screen.PrimaryScreen.WorkingArea.Height - Me.Height) \ 2

            If System.IO.File.Exists(folderLayouts & Me.Name & LayoutControl1.Name & ".xml") Then
                LayoutControl1.RestoreLayoutFromXml(folderLayouts & Me.Name & LayoutControl1.Name & ".xml")
            End If

            If System.IO.File.Exists(folderLayouts & Me.Name & SearchLookUpEdit1View.Name & ".xml") Then
                SearchLookUpEdit1View.RestoreLayoutFromXml(folderLayouts & Me.Name & SearchLookUpEdit1View.Name & ".xml")
            End If
            If System.IO.File.Exists(folderLayouts & Me.Name & gvGudang.Name & ".xml") Then
                gvGudang.RestoreLayoutFromXml(folderLayouts & Me.Name & gvGudang.Name & ".xml")
            End If
            If System.IO.File.Exists(FolderLayouts & Me.Name & gvNotaJual.Name & ".xml") Then
                gvNotaJual.RestoreLayoutFromXml(FolderLayouts & Me.Name & gvNotaJual.Name & ".xml")
            End If
            FungsiControl.SetForm(Me)
            HighLightTxt()
        Catch ex As Exception
            XtraMessageBox.Show(ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            Windows.Forms.Cursor.Current = Cursors.Default
            dlg.Close()
            dlg.Dispose()
        End Try
    End Sub
    Private Sub HighLightTxt()
        For Each ctrl In LayoutControl1.Controls
            If TypeOf ctrl Is DevExpress.XtraEditors.TextEdit Then
                AddHandler TryCast(ctrl, DevExpress.XtraEditors.TextEdit).GotFocus, AddressOf txt_GotFocus
            End If
        Next
    End Sub
    Private Sub txt_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim txt As DevExpress.XtraEditors.TextEdit = TryCast(sender, DevExpress.XtraEditors.TextEdit)
        If txt.Properties.Mask.MaskType = Mask.MaskType.Numeric Then
            txt.SelectAll()
        End If
    End Sub
    Private Sub RefreshData()
        Dim DS As New DataSet
        Try
            RefreshDataKontak()
            If pTipe = pStatus.Baru Then
                IsiDefault()
            Else
                SQL = "SELECT MReturBeli.*, MWilayah.Nama AS Wilayah, MAlamat.Alamat, MAlamat.Nama AS NamaAlamat, MAlamat.Kode as KodeAlamat FROM MReturBeli LEFT JOIN (MGudang LEFT JOIN MWilayah ON MWilayah.NoID=MGudang.IDWilayah) ON MGudang.NoID=MReturBeli.IDGudang LEFT JOIN MAlamat ON MAlamat.NoID=MReturBeli.IDSupplier WHERE MReturBeli.NoID=" & NoID
                DS = ExecuteDataset("Tabel", SQL)
                If DS.Tables("Tabel").Rows.Count >= 1 Then
                    txtKodeSupplier.EditValue = NullTolong(DS.Tables(0).Rows(0).Item("IDSupplier"))
                    'txtKodeSupplier.Text = NullTostr(DS.Tables(0).Rows(0).Item("KodeAlamat"))
                    txtGudang.EditValue = NullToLong(DS.Tables(0).Rows(0).Item("IDGudang"))
                    RubahGudang()
                    txtNamaSupplier.Text = NullTostr(DS.Tables(0).Rows(0).Item("NamaAlamat"))
                    txtAlamatSupplier.Text = NullTostr(DS.Tables(0).Rows(0).Item("Alamat"))
                    txtKode.Text = NullTostr(DS.Tables(0).Rows(0).Item("Kode"))
                    KodeLama = txtKode.Text
                    txtKodeReff.Text = NullTostr(DS.Tables(0).Rows(0).Item("KodeReff"))
                    Tgl.DateTime = NullToDate(DS.Tables(0).Rows(0).Item("Tanggal"))
                    tglStok.DateTime = NullToDate(DS.Tables(0).Rows(0).Item("TanggalStock"))
                    tglJatuhTempo.DateTime = NullToDate(DS.Tables(0).Rows(0).Item("JatuhTempo"))
                    tglSJ.DateTime = NullToDate(DS.Tables(0).Rows(0).Item("TanggalSJ"))
                    txtNoSJ.Text = NullTostr(DS.Tables(0).Rows(0).Item("NoSJ"))
                    txtTotalJual.EditValue = NullToDbl(DS.Tables(0).Rows(0).Item("TotalJual"))
                    txtSubtotal.EditValue = NullToDbl(DS.Tables(0).Rows(0).Item("SubTotal"))
                    txtDiscPersen.EditValue = NullToDbl(DS.Tables(0).Rows(0).Item("DiskonNotaProsen"))
                    txtDiscRp.EditValue = NullToDbl(DS.Tables(0).Rows(0).Item("DiskonNotaRp"))
                    txtDiscTotal.EditValue = NullToDbl(DS.Tables(0).Rows(0).Item("DiskonNotaTotal"))
                    txtBiaya.EditValue = NullToDbl(DS.Tables(0).Rows(0).Item("Biaya"))
                    txtTotal.EditValue = NullToDbl(DS.Tables(0).Rows(0).Item("Total"))
                    txtBayar.EditValue = NullToDbl(DS.Tables(0).Rows(0).Item("Bayar"))
                    txtSisa.EditValue = NullToDbl(DS.Tables(0).Rows(0).Item("Sisa"))
                    IsPosted = NullTobool(DS.Tables(0).Rows(0).Item("IsPosted"))
                    txtKeterangan.Text = NullToStr(DS.Tables(0).Rows(0).Item("Keterangan"))
                    txtNotaJual.EditValue = NullToLong(DS.Tables(0).Rows(0).Item("IDBeli"))
                    IDBeli = NullToLong(DS.Tables(0).Rows(0).Item("IDBeli"))
                    txtKategori.EditValue = NullToLong(DS.Tables(0).Rows(0).Item("IDKategori"))
                    'txtWilayah.Text = NullTostr(DS.Tables(0).Rows(0).Item("Wilayah"))
                    Tgl.Properties.ReadOnly = True
                Else
                    IsiDefault()
                End If
            End If
        Catch ex As Exception
            XtraMessageBox.Show("Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally

            DS.Dispose()
        End Try
    End Sub

    Private Sub HitungTotal()
        Try

            txtDiscTotal.EditValue = (NullToDbl(txtDiscPersen.EditValue) * NullToDbl(txtSubtotal.EditValue) / 100) '+ txtDiscRp.EditValue
            txtSubtotal2.EditValue = NullToDbl(txtSubtotal.EditValue) - NullToDbl(txtDiscTotal.EditValue)
            txtTotal.EditValue = txtSubtotal.EditValue - txtDiscTotal.EditValue + txtBiaya.EditValue
            txtSisa.EditValue = txtTotal.EditValue - txtBayar.EditValue
        Catch ex As Exception
            XtraMessageBox.Show("Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub
    Private Sub IsiDefault()
        Tgl.DateTime = TanggalSystem
        tglJatuhTempo.DateTime = DateAdd(DateInterval.Month, 1, Tgl.DateTime)
        tglSJ.DateTime = TanggalSystem
        tglStok.DateTime = TanggalSystem
        SetTombol()
        txtKodeSupplier.EditValue = DefIDSupplier
        RubahSupplier()
        txtGudang.EditValue = DefIDGudang
        RubahGudang()
        Tgl.Properties.ReadOnly = False
    End Sub

    Private Sub BarButtonItem1_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem1.ItemClick
        cmdSave.PerformClick()
    End Sub

    Private Sub BarButtonItem2_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs)
        cmdTutup.PerformClick()
    End Sub

    Private Sub cmdTutup_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdTutup.Click
        DialogResult = Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub
    Private Sub SetTombol()
        DefImageList = frmMain.ImageList1



        cmdSave.ImageList = DefImageList
        cmdSave.ImageIndex = 6

        cmdTutup.ImageList = DefImageList
        cmdTutup.ImageIndex = 3


    End Sub

    Private Sub cmdSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSave.Click
        Dim PesanSalah As String = ""
        If Not IsPosted Then
            'If pTipe = pStatus.Baru Then
            '    txtKode.Text = clsKode.MintaKodeBaru("RBTB", "MReturBeli", Tgl.DateTime, IDWilayah, 5, "ISNULL(MReturBeli.IsTanpaBarang,0)=1")
            'End If
            If IsValidasi() Then
                If Simpan(PesanSalah) = True Then
                    'If pTipe = pStatus.Baru Then
                    '    KodeLama = txtKode.Text
                    '    pTipe = pStatus.Edit
                    '    SetTombol()

                    '    isTempInsertbaru = True
                    'Else
                    clsPostingPembelian.PostingStokBarangReturPembelian(NoID)
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
                    'End If
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
        If txtKodeSupplier.Text = "" Then
            XtraMessageBox.Show("Supplier masih kosong.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
            txtKodeSupplier.Focus()
            Return False
            Exit Function
        End If
        If txtGudang.Text = "" Then
            XtraMessageBox.Show("Gudang masih kosong.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
            txtGudang.Focus()
            Return False
            Exit Function
        End If
        If Tgl.Text = "" Then
            XtraMessageBox.Show("Tanggal masih kosong.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
            Tgl.Focus()
            Return False
            Exit Function
        End If
        'If txtDiscRp.EditValue < 0 Then
        '    XtraMessageBox.Show("Diskon rupiah masih dibawah 0.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
        '    txtDiscRp.Focus()
        '    Return False
        '    Exit Function
        'End If
        'If txtDiscPersen.EditValue < 0 Then
        '    XtraMessageBox.Show("Diskon persen masih dibawah 0.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
        '    txtDiscPersen.Focus()
        '    Return False
        '    Exit Function
        'End If
        'If txtBayar.EditValue < 0 Then
        '    XtraMessageBox.Show("Bayar masih dibawah 0.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
        '    txtBayar.Focus()
        '    Return False
        '    Exit Function
        'End If
        'If txtBiaya.EditValue < 0 Then
        '    XtraMessageBox.Show("Biaya masih dibawah 0.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
        '    txtBiaya.Focus()
        '    Return False
        '    Exit Function
        'End If
        If txtTotal.EditValue > txtTotalJual.EditValue Then
            If XtraMessageBox.Show("Total Penjualan lebih rendah dari harga pokok.", NamaAplikasi, MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) = Windows.Forms.DialogResult.No Then
                txtSubtotal.Focus()
                Return False
                Exit Function
            End If
        End If
        If CekKodeValid(txtKode.Text, KodeLama, "MReturBeli", "Kode", IIf(pTipe = pStatus.Edit, True, False)) Then
            XtraMessageBox.Show("Kode sudah dipakai.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
            txtKode.Focus()
            Return False
            Exit Function
        End If
        'If Not CekSupplierJual(NullToLong(txtKodeSupplier.EditValue)) Then
        '    If XtraMessageBox.Show("Belum ada transaksi pembelian ke Supplier " & txtKodeSupplier.Text & "." & vbCrLf & "Ingin melanjutkan penyimpanan?", NamaAplikasi, MessageBoxButtons.YesNo, MessageBoxIcon.Information) = Windows.Forms.DialogResult.No Then
        '        txtKodeSupplier.Focus()
        '        Return False
        '        Exit Function
        '    Else
        '        Dim x As New frmOtorisasiAdmin
        '        Try
        '            If x.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
        '            Else
        '                txtKodeSupplier.Focus()
        '                Return False
        '                Exit Function
        '            End If
        '        Catch ex As Exception
        '            XtraMessageBox.Show("En", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
        '        End Try
        '    End If
        'End If

        Return True
    End Function
    Dim KodeLama As String = ""
    Public Function Simpan(ByRef PesanSalah As String) As Boolean
        Dim Sukses As Boolean = False
        HitungTotal()
        Try
            If pTipe = pStatus.Baru Then
                NoID = GetNewID("MReturBeli")
                SQL = "INSERT INTO MReturBeli (IsTanpaBarang,NoID,IDGudang,IDWilayah,IDAdmin,Kode,KodeReff,Tanggal,TanggalStock,JatuhTempo,"
                SQL &= " IDSupplier,TanggalSJ,NoSJ,TotalJual,SubTotal,DiskonNotaProsen,DiskonNotaRp,DiskonNotaTotal,"
                SQL &= "  Biaya, Total, Bayar, Sisa, Keterangan,IDBeli,IDKategori) VALUES (1," & vbCrLf
                SQL &= NoID & ","
                SQL &= NullToLong(txtGudang.EditValue) & ","
                SQL &= IDWilayah & ","
                SQL &= IDAdmin & ","
                SQL &= "'" & FixApostropi(txtKode.Text) & "',"
                SQL &= "'" & FixApostropi(txtKodeReff.Text) & "',"
                SQL &= "'" & Tgl.DateTime.ToString("yyyy-MM-dd HH:mm") & "',"
                SQL &= "'" & tglStok.DateTime.ToString("yyyy-MM-dd HH:mm") & "',"
                SQL &= "'" & tglJatuhTempo.DateTime.ToString("yyyy-MM-dd HH:mm") & "',"
                SQL &= txtKodeSupplier.EditValue & ","
                SQL &= "'" & tglSJ.DateTime.ToString("yyyy-MM-dd HH:mm") & "',"
                SQL &= "'" & FixApostropi(txtNoSJ.Text) & "',"
                SQL &= FixKoma(txtTotalJual.EditValue) & ","
                SQL &= FixKoma(txtSubtotal.EditValue) & ","
                SQL &= FixKoma(txtDiscPersen.EditValue) & ","
                SQL &= FixKoma(txtDiscRp.EditValue) & ","
                SQL &= FixKoma(txtDiscTotal.EditValue) & ","
                SQL &= FixKoma(txtBiaya.EditValue) & ","
                SQL &= FixKoma(txtTotal.EditValue) & ","
                SQL &= FixKoma(txtBayar.EditValue) & ","
                SQL &= FixKoma(txtSisa.EditValue) & ","
                SQL &= "'" & FixApostropi(txtKeterangan.Text) & "',"
                SQL &= NullToLong(txtNotaJual.EditValue) & ","
                SQL &= NullToLong(txtKategori.EditValue) & ")"
                EksekusiSQL(SQL)
            Else
                SQL = "UPDATE MReturBeli SET "
                If Not Tgl.Properties.ReadOnly Or Not txtKode.Properties.ReadOnly Then
                    SQL &= "IDAdmin=" & IDAdmin & ","
                End If
                SQL &= "IDGudang=" & NullToLong(txtGudang.EditValue) & ","
                SQL &= "IsTanpaBarang=" & 1 & ","
                SQL &= "IDWilayah=" & IDWilayah & ","
                SQL &= "Kode='" & FixApostropi(txtKode.Text) & "',"
                SQL &= "KodeReff='" & FixApostropi(txtKodeReff.Text) & "',"
                SQL &= "Tanggal='" & Tgl.DateTime.ToString("yyyy-MM-dd HH:mm") & "',"
                SQL &= "TanggalStock='" & tglStok.DateTime.ToString("yyyy-MM-dd HH:mm") & "',"
                SQL &= "JatuhTempo='" & tglJatuhTempo.DateTime.ToString("yyyy-MM-dd HH:mm") & "',"
                SQL &= "IDSupplier=" & txtKodeSupplier.EditValue & ","
                SQL &= "IDKategori=" & NullToLong(txtKategori.EditValue) & ","
                'SQL &= "IDGudang=" & txtGudang.EditValue & ","
                SQL &= "TanggalSJ='" & tglSJ.DateTime.ToString("yyyy-MM-dd HH:mm") & "',"
                SQL &= "NoSJ='" & FixApostropi(txtNoSJ.Text) & "',"
                SQL &= "TotalJual=" & FixKoma(txtTotalJual.EditValue) & ","
                SQL &= "SubTotal=" & FixKoma(txtSubtotal.EditValue) & ","
                SQL &= "DiskonNotaProsen=" & FixKoma(txtDiscPersen.EditValue) & ","
                SQL &= "DiskonNotaRp=" & FixKoma(txtDiscRp.EditValue) & ","
                SQL &= "DiskonNotaTotal=" & FixKoma(txtDiscTotal.EditValue) & ","
                SQL &= "Biaya=" & FixKoma(txtBiaya.EditValue) & ","
                SQL &= "Total=" & FixKoma(txtTotal.EditValue) & ","
                SQL &= "Bayar=" & FixKoma(txtBayar.EditValue) & ","
                SQL &= "Sisa=" & FixKoma(txtSisa.EditValue) & ","
                SQL &= "IDBeli=" & NullToLong(txtNotaJual.EditValue) & ","
                SQL &= "Keterangan='" & FixApostropi(txtKeterangan.Text) & "'"
                SQL &= " WHERE NoID=" & NoID
                EksekusiSQL(SQL)
            End If
            Sukses = True
            If Sukses Then
                SQL = "UPDATE MReturBeliD SET IDGudang=" & NullToLong(txtGudang.EditValue) & " WHERE IDReturBeli=" & NoID
                EksekusiSQL(SQL)
            End If
        Catch ex As Exception
            PesanSalah = ex.Message
            Sukses = False
        Finally

        End Try
        Return Sukses
    End Function

    Private Sub BarButtonItem3_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem3.ItemClick
        Dim xOtorisasi As New frmOtorisasiAdmin
        Try
            If XtraMessageBox.Show("Simpan Layout?", NamaAplikasi, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = Windows.Forms.DialogResult.Yes AndAlso xOtorisasi.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
                Ini.TulisIniPath(folderLayouts & Me.Name & ".ini", "Form", "Width", Me.Width)
                Ini.TulisIniPath(folderLayouts & Me.Name & ".ini", "Form", "Height", Me.Height)
                Me.Left = (Screen.PrimaryScreen.WorkingArea.Width - Me.Width) \ 2
                Me.Top = (Screen.PrimaryScreen.WorkingArea.Height - Me.Height) \ 2

                LayoutControl1.SaveLayoutToXml(folderLayouts & Me.Name & LayoutControl1.Name & ".xml")
                SearchLookUpEdit1View.SaveLayoutToXml(FolderLayouts & Me.Name & SearchLookUpEdit1View.Name & ".xml")
                gvGudang.SaveLayoutToXml(FolderLayouts & Me.Name & gvGudang.Name & ".xml")
                gvNotaJual.SaveLayoutToXml(FolderLayouts & Me.Name & gvNotaJual.Name & ".xml")
                gvKategori.SaveLayoutToXml(FolderLayouts & Me.Name & gvKategori.Name & ".xml")
            End If
        Catch ex As Exception
            XtraMessageBox.Show("Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            xOtorisasi.Dispose()
        End Try
    End Sub


    Private Sub txtKodeSupplier_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtKodeSupplier.EditValueChanged
        RubahSupplier()
    End Sub
    Private Sub RubahSupplier()
        Dim TglAdd As Long = 0
        Try
            txtNamaSupplier.Text = NullTostr(EksekusiSQlSkalarNew("SELECT Nama FROM MAlamat WHERE NoID=" & NullTolong(txtKodeSupplier.EditValue)))
            txtAlamatSupplier.Text = NullTostr(EksekusiSQlSkalarNew("SELECT Alamat FROM MAlamat WHERE NoID=" & NullTolong(txtKodeSupplier.EditValue)))
            TglAdd = NullTolong(EksekusiSQlSkalarNew("SELECT JatuhTempoSupplier FROM MAlamat WHERE NoID=" & NullTolong(txtKodeSupplier.EditValue)))
            If TglAdd = 0 Then
                tglJatuhTempo.DateTime = DateAdd(DateInterval.Month, 2, Tgl.DateTime)
            Else
                tglJatuhTempo.DateTime = DateAdd(DateInterval.Day, TglAdd, Tgl.DateTime)
            End If
            RefreshNotaJual()
        Catch ex As Exception

        End Try
    End Sub
    Private Sub Tgl_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Tgl.EditValueChanged
        Dim TglAdd As Long = 0
        Try
            'txtNamaSupplier.Text = NullTostr(SearchLookUpEdit1View.GetRowCellValue(SearchLookUpEdit1View.FocusedRowHandle, "Nama"))
            'txtAlamatSupplier.Text = NullTostr(EksekusiSQlSkalarNew("SELECT Alamat FROM MAlamat WHERE NoID=" & NullTolong(txtKodeSupplier.EditValue)))
            TglAdd = NullTolong(EksekusiSQlSkalarNew("SELECT JatuhTempoSupplier FROM MAlamat WHERE NoID=" & NullTolong(txtKodeSupplier.EditValue)))
            If TglAdd = 0 Then
                tglJatuhTempo.DateTime = DateAdd(DateInterval.Month, 2, Tgl.DateTime)
            Else
                tglJatuhTempo.DateTime = DateAdd(DateInterval.Day, TglAdd, Tgl.DateTime)
            End If

            If pTipe = pStatus.Baru Then
                txtKode.Text = clsKode.MintaKodeBaru("RBTB", "MReturBeli", Tgl.DateTime, IDWilayah, 5, "ISNULL(MReturBeli.IsTanpaBarang,0)=1")
            End If

        Catch ex As Exception

        End Try
    End Sub

    Private Sub txtSubtotal_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtSubtotal.EditValueChanged

    End Sub

    Private Sub txtDiscPersen_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtDiscPersen.EditValueChanged
        txtDiscRp.EditValue = txtSubtotal.EditValue * txtDiscPersen.EditValue / 100
        HitungTotal()
    End Sub

    Private Sub txtSubtotal_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtSubtotal.LostFocus
        HitungTotal()
    End Sub

    Private Sub txtDiscPersen_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtDiscPersen.LostFocus
        txtDiscRp.EditValue = txtSubtotal.EditValue * txtDiscPersen.EditValue / 100
        HitungTotal()
    End Sub

    Private Sub txtDiscRp_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtDiscRp.EditValueChanged
        txtDiscPersen.EditValue = txtDiscRp.EditValue / IIf(txtSubtotal.EditValue = 0, 1, txtSubtotal.EditValue) * 100
        HitungTotal()
    End Sub

    Private Sub txtDiscRp_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtDiscRp.KeyDown
        If e.KeyCode = Keys.F7 Then
            Dim x As New frmOtorisasiAdmin
            Try
                If txtDiscRp.Properties.ReadOnly Then
                    If x.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
                        IDAdmin = x.IDUserAdmin
                        txtDiscRp.Properties.ReadOnly = False
                    End If
                End If
            Catch ex As Exception
                XtraMessageBox.Show("Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End If
    End Sub

    Private Sub txtDiscRp_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtDiscRp.LostFocus
        txtDiscPersen.EditValue = txtDiscRp.EditValue / IIf(txtSubtotal.EditValue = 0, 1, txtSubtotal.EditValue) * 100
        HitungTotal()
    End Sub

    Private Sub txtDiscTotal_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtDiscTotal.LostFocus
        HitungTotal()
    End Sub

    Private Sub txtDiscTotal_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtDiscTotal.EditValueChanged

    End Sub

    Private Sub txtBiaya_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtBiaya.EditValueChanged
        HitungTotal()
    End Sub

    Private Sub txtBiaya_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtBiaya.LostFocus
        HitungTotal()
    End Sub

    Private Sub txtTotal_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtTotal.EditValueChanged

    End Sub

    Private Sub txtTotal_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtTotal.LostFocus
        HitungTotal()
    End Sub

    Private Sub txtBayar_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtBayar.EditValueChanged

    End Sub

    Private Sub txtBayar_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtBayar.LostFocus
        HitungTotal()
    End Sub

    Private Sub txtSisa_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtSisa.EditValueChanged

    End Sub

    Private Sub txtSisa_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtSisa.LostFocus
        HitungTotal()
    End Sub


    Private Sub BarButtonItem7_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem7.ItemClick
        cmdTutup.PerformClick()
    End Sub


    Private Sub Tgl_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Tgl.KeyDown
        If e.KeyCode = Keys.F7 Then
            Dim x As New frmOtorisasiAdmin
            Try
                If Tgl.Properties.ReadOnly Then
                    If x.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
                        IDAdmin = x.IDUserAdmin
                        Tgl.Properties.ReadOnly = False
                        tglStok.Properties.ReadOnly = False
                        tglJatuhTempo.Properties.ReadOnly = False
                    End If
                End If
            Catch ex As Exception
                XtraMessageBox.Show("Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End If
    End Sub

    Private Sub txtKode_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtKode.KeyDown
        If e.KeyCode = Keys.F7 Then
            Dim x As New frmOtorisasiAdmin
            Try
                If txtKode.Properties.ReadOnly Then
                    If x.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
                        IDAdmin = x.IDUserAdmin
                        txtKode.Properties.ReadOnly = False
                    End If
                End If
            Catch ex As Exception
                XtraMessageBox.Show("Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        ElseIf e.KeyCode = Keys.Enter Then
            If pTipe = pStatus.Baru Then
                If NullToLong(EksekusiSQlSkalarNew("select NoID from MReturBeli where ISNULL(IsTanpaBarang,0)=1 and Kode='" & FixApostropi(txtKode.Text) & "'")) > 0 Then
                    MsgBox("Kode Sudah Ada!", MsgBoxStyle.Exclamation)
                End If
            Else
                If NullToLong(EksekusiSQlSkalarNew("select NoID from MReturBeli where ISNULL(IsTanpaBarang,0)=1 and Kode<>'" & FixApostropi(KodeLama) & " and Kode='" & FixApostropi(txtKode.Text) & "'")) > 0 Then
                    MsgBox("Kode Sudah Ada!", MsgBoxStyle.Exclamation)
                End If
            End If

        End If
    End Sub

    Private Sub txtSubtotal2_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtSubtotal2.EditValueChanged

    End Sub

    Private Sub txtSubtotal2_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtSubtotal2.LostFocus
        HitungTotal()
    End Sub

    Private Sub txtGudang_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtGudang.EditValueChanged
        RubahGudang()
    End Sub
    Private Sub RubahGudang()
        Dim Ds As New DataSet
        Try
            SQL = "SELECT MWilayah.NoID AS IDWilayah, MWilayah.Nama AS Wilayah FROM MGudang LEFT JOIN MWilayah ON MWilayah.NoID=MGudang.IDWilayah WHERE MGudang.NOID=" & NullToLong(txtGudang.EditValue)
            Ds = ExecuteDataset("Tabel", SQL)
            If Ds.Tables("Tabel").Rows.Count >= 1 Then
                txtWilayah.Text = NullToStr(Ds.Tables(0).Rows(0).Item("Wilayah"))
                IDWilayah = NullToLong(Ds.Tables(0).Rows(0).Item("IDWilayah"))
            End If
        Catch ex As Exception
            XtraMessageBox.Show("Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            Ds.Dispose()
        End Try
    End Sub

    Private Sub gvGudang_DataSourceChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvGudang.DataSourceChanged
        If System.IO.File.Exists(folderLayouts & Me.Name & gvGudang.Name & ".xml") Then
            gvGudang.RestoreLayoutFromXml(folderLayouts & Me.Name & gvGudang.Name & ".xml")
        End If
        With gvGudang
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

    Private Sub SearchLookUpEdit1View_DataSourceChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles SearchLookUpEdit1View.DataSourceChanged
        If System.IO.File.Exists(folderLayouts & Me.Name & SearchLookUpEdit1View.Name & ".xml") Then
            SearchLookUpEdit1View.RestoreLayoutFromXml(folderLayouts & Me.Name & SearchLookUpEdit1View.Name & ".xml")
        End If
        With SearchLookUpEdit1View
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

    Private Sub gvNotaJual_DataSourceChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvNotaJual.DataSourceChanged
        If System.IO.File.Exists(FolderLayouts & Me.Name & gvNotaJual.Name & ".xml") Then
            SearchLookUpEdit1View.RestoreLayoutFromXml(FolderLayouts & Me.Name & gvNotaJual.Name & ".xml")
        End If
        With gvNotaJual
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

    Private Sub gvKategori_DataSourceChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvKategori.DataSourceChanged
        If System.IO.File.Exists(FolderLayouts & Me.Name & gvKategori.Name & ".xml") Then
            gvKategori.RestoreLayoutFromXml(FolderLayouts & Me.Name & gvKategori.Name & ".xml")
        End If
        With gvKategori
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
End Class