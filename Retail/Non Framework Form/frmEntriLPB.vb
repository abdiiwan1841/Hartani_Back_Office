Imports System.Data
Imports System.Data.SqlClient
Imports DevExpress.XtraEditors
Imports DevExpress.Utils
Imports DevExpress.XtraGrid.Views.Base
Imports System.IO.File

Public Class frmEntriLPB
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
    Dim IsTempInsertBaru As Boolean = False

    Private Sub RefreshDataKontak()
        Dim ds As New DataSet
        Try
            SQL = "SELECT NoID, Kode, Nama FROM MAlamat WHERE IsActive=1"
            If rbDari.SelectedIndex = 0 Then
                SQL &= " AND IsSupplier=1"
            ElseIf rbDari.SelectedIndex = 1 Then
                SQL &= " AND IsCustomer=1"
            End If
            ds = ExecuteDataset("master", SQL)
            txtKodeSupplier.Properties.DataSource = ds.Tables("master")
            If System.IO.File.Exists(folderLayouts & Me.Name & SearchLookUpEdit1View.Name & ".xml") Then
                SearchLookUpEdit1View.RestoreLayoutFromXml(folderLayouts & Me.Name & SearchLookUpEdit1View.Name & ".xml")
            End If

            SQL = "SELECT NoID, Kode, Nama FROM MAlamat WHERE IsActive=1 AND IsPegawai=1"
            ds = ExecuteDataset("master", SQL)
            txtPenerima.Properties.ValueMember = "NoID"
            txtPenerima.Properties.DisplayMember = "Nama"
            txtPenerima.Properties.DataSource = ds.Tables("master")
            If System.IO.File.Exists(folderLayouts & Me.Name & gvPenerima.Name & ".xml") Then
                gvPenerima.RestoreLayoutFromXml(folderLayouts & Me.Name & gvPenerima.Name & ".xml")
            End If
            SQL = "SELECT MGudang.NoID, MGudang.Kode, MGudang.Kode + ' - ' + MGudang.Nama AS Nama, MWilayah.Nama AS Wilayah FROM MGudang LEFT JOIN MWilayah ON MGudang.IDWilayah=MWilayah.NoID WHERE MGudang.IsActive=1 AND MGudang.InTransit=0 AND MGudang.IDWilayah=" & DefIDWilayah
            ds = ExecuteDataset("master", SQL)
            txtGudang.Properties.DataSource = ds.Tables("master")
            txtGudang.Properties.ValueMember = "NoID"
            txtGudang.Properties.DisplayMember = "Nama"
            If System.IO.File.Exists(folderLayouts & Me.Name & gvGudang.Name & ".xml") Then
                gvGudang.RestoreLayoutFromXml(folderLayouts & Me.Name & gvGudang.Name & ".xml")
            End If
        Catch ex As Exception
            XtraMessageBox.Show("Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            ds.Dispose()
        End Try
    End Sub
    Dim DefImageList As New ImageList

    Private Sub frmEntriLPB_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        If IsTempInsertBaru Then
            If XtraMessageBox.Show("Yakin ingin keluar?" & vbCrLf & "Ya, akan menghapus transaksi dan detil item.", NamaAplikasi, MessageBoxButtons.YesNo, MessageBoxIcon.Information, MessageBoxDefaultButton.Button2) = Windows.Forms.DialogResult.Yes Then
                EksekusiSQL("DELETE FROM MLPB WHERE NoID=" & NoID)
                EksekusiSQL("DELETE FROM MLPBD WHERE IDLPB=" & NoID)
            Else
                e.Cancel = True
            End If
        End If
    End Sub
    Sub HighLightTxt()
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
            Me.Width = Ini.BacaIniPath(folderLayouts & Me.Name & ".ini", "Form", "Width", Me.Width)
            Me.Height = Ini.BacaIniPath(folderLayouts & Me.Name & ".ini", "Form", "Height", Me.Height)

            Me.Left = (Screen.PrimaryScreen.WorkingArea.Width - Me.Width) \ 2
            Me.Top = (Screen.PrimaryScreen.WorkingArea.Height - Me.Height) \ 2

            If System.IO.File.Exists(folderLayouts & Me.Name & LayoutControl1.Name & ".xml") Then
                LayoutControl1.RestoreLayoutFromXml(folderLayouts & Me.Name & LayoutControl1.Name & ".xml")
            End If
            If System.IO.File.Exists(folderLayouts & Me.Name & GV1.Name & ".xml") Then
                GV1.RestoreLayoutFromXml(folderLayouts & Me.Name & GV1.Name & ".xml")
            End If
            If System.IO.File.Exists(folderLayouts & Me.Name & SearchLookUpEdit1View.Name & ".xml") Then
                SearchLookUpEdit1View.RestoreLayoutFromXml(folderLayouts & Me.Name & SearchLookUpEdit1View.Name & ".xml")
            End If
            LoadLayout()
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
    Private Sub RefreshData()
        Dim DS As New DataSet
        Try
            RefreshDataKontak()
            If pTipe = pStatus.Baru Then
                IsiDefault()
            Else
                SQL = "SELECT MLPB.*, MAlamat.Alamat AS Alamat, MAlamat.Nama AS NamaAlamat, MAlamat.Kode as KodeAlamat, MWilayah.Nama AS Wilayah FROM MLPB LEFT JOIN MAlamat ON MAlamat.NoID=MLPB.IDSupplier LEFT JOIN (MGudang LEFT JOIN MWilayah ON MWilayah.NoID=MGudang.IDWilayah) ON MGudang.NoID=MLPB.IDGudangPenerima WHERE MLPB.NoID=" & NoID
                DS = ExecuteDataset("Tabel", SQL)
                If DS.Tables("Tabel").Rows.Count >= 1 Then
                    rbDari.SelectedIndex = NullToLong(DS.Tables(0).Rows(0).Item("Dari"))
                    RubahDari()
                    txtKodeSupplier.EditValue = NullToLong(DS.Tables(0).Rows(0).Item("IDSupplier"))
                    'txtKodeSupplier.Text = NullTostr(DS.Tables(0).Rows(0).Item("KodeAlamat"))
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
                    txtSubtotal.EditValue = NullToDbl(DS.Tables(0).Rows(0).Item("SubTotal"))
                    txtDiscPersen.EditValue = NullToDbl(DS.Tables(0).Rows(0).Item("DiskonNotaProsen"))
                    txtDiscRp.EditValue = NullToDbl(DS.Tables(0).Rows(0).Item("DiskonNotaRp"))
                    txtDiscTotal.EditValue = NullToDbl(DS.Tables(0).Rows(0).Item("DiskonNotaTotal"))
                    txtBiaya.EditValue = NullToDbl(DS.Tables(0).Rows(0).Item("Biaya"))
                    txtTotal.EditValue = NullToDbl(DS.Tables(0).Rows(0).Item("Total"))
                    txtBayar.EditValue = NullToDbl(DS.Tables(0).Rows(0).Item("Bayar"))
                    txtSisa.EditValue = NullToDbl(DS.Tables(0).Rows(0).Item("Sisa"))
                    IsPosted = NullTobool(DS.Tables(0).Rows(0).Item("IsPosted"))
                    txtGudang.EditValue = NullTolong(DS.Tables(0).Rows(0).Item("IDGudangPenerima"))
                    txtPenerima.EditValue = NullTolong(DS.Tables(0).Rows(0).Item("IDBagPembelian"))
                    txtPengirim.Text = NullTostr(DS.Tables(0).Rows(0).Item("Pengirim"))
                    txtCatatan.Text = NullTostr(DS.Tables(0).Rows(0).Item("Keterangan"))
                    txtWilayahGudang.Text = NullTostr(DS.Tables(0).Rows(0).Item("Wilayah"))
                    If IsPosted Then
                        txtKodeSupplier.Properties.ReadOnly = True
                    End If
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
            strsql = "select MLPBD.* ,MBarang.CtnPcs AS IsiCtn, MBarang.Kode as KodeStock,MBarang.Nama as NamaStock,MBarang.KodeAlias as KodeAlias, MBarang.NamaAlias,MSatuan.Kode as Satuan,Mgudang.Kode as KodeGudang " & vbCrLf
            strsql &= " From (MLPBD Inner Join MLPB On MLPBD.IDLPB=MLPB.NoID) " & vbCrLf
            strsql &= " LEFT JOIN MBarang ON MLPBD.IDBarang=MBarang.NoID " & vbCrLf
            strsql &= " LEFT JOIN MSatuan ON MLPBD.IDSatuan=MSatuan.NoID " & vbCrLf
            strsql &= " LEFT JOIN MGudang ON MLPBD.IDGudang=MGudang.NoID " & vbCrLf
            strsql &= " where MLPBD.IDLPB = " & NoID
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
            HitungTotal()
            GV1.HideFindPanel()
        Catch ex As Exception
            XtraMessageBox.Show(ex.Message, ".:: Pesan Kesalahan ::.", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
        Windows.Forms.Cursor.Current = curentcursor
        Application.DoEvents()
    End Sub
    Private Sub HitungTotal()
        Dim temp As Double = 0.0
        Try
            For i = 0 To GV1.RowCount
                temp = temp + NullToDbl(GV1.GetRowCellValue(i, "Jumlah"))
            Next
            txtSubtotal.EditValue = temp
            txtDiscTotal.EditValue = (NullToDbl(txtDiscPersen.EditValue) * NullToDbl(txtSubtotal.EditValue) / 100) '+ txtDiscRp.EditValue
            txtTotal.EditValue = txtSubtotal.EditValue - txtDiscTotal.EditValue + txtBiaya.EditValue
            txtSisa.EditValue = txtTotal.EditValue - txtBayar.EditValue
            txtTotalCTN.EditValue = NullToDbl(EksekusiSQlSkalarNew("SELECT SUM(Ctn) FROM MLPBD WHERE IDLPB=" & NoID))
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

        rbDari.SelectedIndex = 0
        RubahDari()
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
            If pTipe = pStatus.Baru Then
                txtKode.Text = clsKode.MintaKodeBaru("STB", "MLPB", Tgl.DateTime, DefIDWilayah, 5)
            End If
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
        If txtKodeSupplier.Text = "" Then
            XtraMessageBox.Show("Supplier / customer masih kosong.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
            txtKodeSupplier.Focus()
            Return False
            Exit Function
        End If
        If txtPengirim.Text = "" Then
            XtraMessageBox.Show("Pengirim masih kosong.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
            txtPengirim.Focus()
            Return False
            Exit Function
        End If
        If txtPenerima.Text = "" Then
            XtraMessageBox.Show("Penerima masih kosong.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
            txtPenerima.Focus()
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
        If txtDiscRp.EditValue < 0 Then
            XtraMessageBox.Show("Diskon rupiah masih dibawah 0.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
            txtDiscRp.Focus()
            Return False
            Exit Function
        End If
        If txtDiscPersen.EditValue > 100 Then
            XtraMessageBox.Show("Diskon persen masih diatas 100%.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
            txtDiscPersen.Focus()
            Return False
            Exit Function
        End If
        If txtDiscRp.EditValue > txtSubtotal.EditValue Then
            XtraMessageBox.Show("Diskon melebihi subtotal.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
            txtDiscRp.Focus()
            Return False
            Exit Function
        End If

        If txtDiscPersen.EditValue < 0 Then
            XtraMessageBox.Show("Diskon persen masih dibawah 0.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
            txtDiscPersen.Focus()
            Return False
            Exit Function
        End If
        If txtBayar.EditValue < 0 Then
            XtraMessageBox.Show("Bayar masih dibawah 0.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
            txtBayar.Focus()
            Return False
            Exit Function
        End If
        If txtBiaya.EditValue < 0 Then
            XtraMessageBox.Show("Biaya masih dibawah 0.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
            txtBiaya.Focus()
            Return False
            Exit Function
        End If
        If CekKodeValid(txtKode.Text, KodeLama, "MLPB", "Kode", IIf(pTipe = pStatus.Edit, True, False)) Then
            XtraMessageBox.Show("Kode sudah dipakai.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
            txtKode.Focus()
            Return False
            Exit Function
        End If
        If pTipe = pStatus.Edit AndAlso GV1.RowCount <= 0 Then
            XtraMessageBox.Show("Item detil masih kosong." & vbCrLf & "Isi item detil atau tutup bila ingin membatailkan.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
            Return False
            Exit Function
        End If
        Return True
    End Function
    Public Function Simpan(ByRef PesanSalah As String) As Boolean
        Dim Sukses As Boolean = False
        HitungTotal()
        Try
            If pTipe = pStatus.Baru Then
                NoID = GetNewID("MLPB")
                SQL = "INSERT INTO MLPB (Dari,NoID,IDWilayah,Kode,KodeReff,Tanggal,TanggalStock,JatuhTempo,"
                SQL &= " IDSupplier,TanggalSJ,NoSJ,SubTotal,DiskonNotaProsen,DiskonNotaRp,DiskonNotaTotal,"
                SQL &= "  Biaya, Total, Bayar, Sisa,IDGudangPenerima,IDBagPembelian,Pengirim,Keterangan,IDAdmin) VALUES (" & vbCrLf
                SQL &= rbDari.SelectedIndex & ","
                SQL &= NoID & ","
                SQL &= DefIDWilayah & ","
                SQL &= "'" & FixApostropi(txtKode.Text) & "',"
                SQL &= "'" & FixApostropi(txtKodeReff.Text) & "',"
                SQL &= "'" & Tgl.DateTime.ToString("yyyy-MM-dd HH:mm") & "',"
                SQL &= "'" & tglStok.DateTime.ToString("yyyy-MM-dd HH:mm") & "',"
                SQL &= "'" & tglJatuhTempo.DateTime.ToString("yyyy-MM-dd HH:mm") & "',"
                SQL &= txtKodeSupplier.EditValue & ","
                SQL &= "'" & tglSJ.DateTime.ToString("yyyy-MM-dd HH:mm") & "',"
                SQL &= "'" & FixApostropi(txtNoSJ.Text) & "',"
                SQL &= FixKoma(txtSubtotal.EditValue) & ","
                SQL &= FixKoma(txtDiscPersen.EditValue) & ","
                SQL &= FixKoma(txtDiscRp.EditValue) & ","
                SQL &= FixKoma(txtDiscTotal.EditValue) & ","
                SQL &= FixKoma(txtBiaya.EditValue) & ","
                SQL &= FixKoma(txtTotal.EditValue) & ","
                SQL &= FixKoma(txtBayar.EditValue) & ","
                SQL &= FixKoma(txtSisa.EditValue) & ","
                SQL &= NullTolong(txtGudang.EditValue) & ","
                SQL &= NullTolong(txtPenerima.EditValue) & ",'"
                SQL &= FixApostropi(txtPengirim.Text) & "','"
                SQL &= FixApostropi(txtCatatan.Text) & "'," & IDAdmin & ")"

                EksekusiSQL(SQL)
            Else
                SQL = "UPDATE MLPB SET "
                SQL &= "Dari=" & rbDari.SelectedIndex & ","
                SQL &= "Kode='" & FixApostropi(txtKode.Text) & "',"
                SQL &= "KodeReff='" & FixApostropi(txtKodeReff.Text) & "',"
                If Not Tgl.Properties.ReadOnly Or Not txtKode.Properties.ReadOnly Then
                    SQL &= "IDAdmin=" & IDAdmin & ","
                End If
                SQL &= "Tanggal='" & Tgl.DateTime.ToString("yyyy-MM-dd HH:mm") & "',"
                SQL &= "TanggalStock='" & tglStok.DateTime.ToString("yyyy-MM-dd HH:mm") & "',"
                SQL &= "JatuhTempo='" & tglJatuhTempo.DateTime.ToString("yyyy-MM-dd HH:mm") & "',"
                SQL &= "IDSupplier=" & txtKodeSupplier.EditValue & ","
                SQL &= "TanggalSJ='" & tglSJ.DateTime.ToString("yyyy-MM-dd HH:mm") & "',"
                SQL &= "NoSJ='" & FixApostropi(txtNoSJ.Text) & "',"
                SQL &= "IDGudangPenerima=" & NullTolong(txtGudang.EditValue) & ","
                SQL &= "IDBagPembelian=" & NullTolong(txtPenerima.EditValue) & ","
                SQL &= "Pengirim='" & FixApostropi(txtPengirim.Text) & "',"
                SQL &= "Keterangan='" & FixApostropi(txtCatatan.Text) & "',"
                SQL &= "SubTotal=" & FixKoma(txtSubtotal.EditValue) & ","
                SQL &= "DiskonNotaProsen=" & FixKoma(txtDiscPersen.EditValue) & ","
                SQL &= "DiskonNotaRp=" & FixKoma(txtDiscRp.EditValue) & ","
                SQL &= "DiskonNotaTotal=" & FixKoma(txtDiscTotal.EditValue) & ","
                SQL &= "Biaya=" & FixKoma(txtBiaya.EditValue) & ","
                SQL &= "Total=" & FixKoma(txtTotal.EditValue) & ","
                SQL &= "Bayar=" & FixKoma(txtBayar.EditValue) & ","
                SQL &= "Sisa=" & FixKoma(txtSisa.EditValue)
                SQL &= " WHERE NoID=" & NoID
                EksekusiSQL(SQL)
            End If
            Sukses = True
            If Sukses Then
                SQL = "UPDATE MLPBD SET IDGudang=" & NullTolong(txtGudang.EditValue) & " WHERE IDLPB=" & NoID
                EksekusiSQL(SQL)
            End If
        Catch ex As Exception
            PesanSalah = ex.Message
            Sukses = False
        Finally

        End Try
        Return Sukses
    End Function
    Private Sub LoadLayout()
        If Exists(folderLayouts & Me.Name & gvGudang.Name & ".xml") Then
            gvGudang.RestoreLayoutFromXml(folderLayouts & Me.Name & gvGudang.Name & ".xml")
        End If
        If Exists(folderLayouts & Me.Name & GV1.Name & ".xml") Then
            GV1.RestoreLayoutFromXml(folderLayouts & Me.Name & GV1.Name & ".xml")
        End If
        If Exists(folderLayouts & Me.Name & gvPenerima.Name & ".xml") Then
            gvPenerima.RestoreLayoutFromXml(folderLayouts & Me.Name & gvPenerima.Name & ".xml")
        End If

        If Exists(folderLayouts & Me.Name & LayoutControl1.Name & ".xml") Then
            LayoutControl1.RestoreLayoutFromXml(folderLayouts & Me.Name & LayoutControl1.Name & ".xml")
        End If
        If Exists(folderLayouts & Me.Name & LayoutControl1.Name & ".xml") Then
            LayoutControl1.RestoreLayoutFromXml(folderLayouts & Me.Name & LayoutControl1.Name & ".xml")
        End If
    End Sub
    Private Sub BarButtonItem3_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem3.ItemClick
        Dim xOtorisasi As New frmOtorisasiAdmin
        Try
            If XtraMessageBox.Show("Simpan Layout?", NamaAplikasi, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = Windows.Forms.DialogResult.Yes AndAlso xOtorisasi.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
                Ini.TulisIniPath(folderLayouts & Me.Name & ".ini", "Form", "Width", Me.Width)
                Ini.TulisIniPath(folderLayouts & Me.Name & ".ini", "Form", "Height", Me.Height)
                Me.Left = (Screen.PrimaryScreen.WorkingArea.Width - Me.Width) \ 2
                Me.Top = (Screen.PrimaryScreen.WorkingArea.Height - Me.Height) \ 2

                LayoutControl1.SaveLayoutToXml(folderLayouts & Me.Name & LayoutControl1.Name & ".xml")
                GV1.SaveLayoutToXml(folderLayouts & Me.Name & GV1.Name & ".xml")
                SearchLookUpEdit1View.SaveLayoutToXml(folderLayouts & Me.Name & SearchLookUpEdit1View.Name & ".xml")
                gvGudang.SaveLayoutToXml(folderLayouts & Me.Name & gvGudang.Name & ".xml")
                gvPenerima.SaveLayoutToXml(folderLayouts & Me.Name & gvPenerima.Name & ".xml")

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
            Dim IDDetil As Long = NullTolong(row("NoID"))
            If XtraMessageBox.Show("Item " & GV1.GetRowCellValue(GV1.FocusedRowHandle, "NamaStock") & " ini mau dihapus?", NamaAplikasi, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) = Windows.Forms.DialogResult.Yes Then
                EksekusiSQL("Delete From MLPBD where NoID=" & IDDetil.ToString)
                RefreshDetil()
            End If
            txtBarang.Focus()
        Catch ex As Exception
            XtraMessageBox.Show("Untuk mengedit pilih item satuan yang akan diedit terlebih dahulu lalu tekan tombol edit", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
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
            If rbDari.SelectedIndex = 0 Then
                TglAdd = NullTolong(EksekusiSQlSkalarNew("SELECT JatuhTempoSupplier FROM MAlamat WHERE NoID=" & NullTolong(txtKodeSupplier.EditValue)))
            Else
                TglAdd = NullTolong(EksekusiSQlSkalarNew("SELECT JatuhTempoCustomer FROM MAlamat WHERE NoID=" & NullTolong(txtKodeSupplier.EditValue)))
            End If
            If TglAdd = 0 Then
                tglJatuhTempo.DateTime = DateAdd(DateInterval.Month, 2, Tgl.DateTime)
            Else
                tglJatuhTempo.DateTime = DateAdd(DateInterval.Day, TglAdd, Tgl.DateTime)
            End If
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
                txtKode.Text = clsKode.MintaKodeBaru("STB", "MLPB", Tgl.DateTime, DefIDWilayah, 5)
            End If
        Catch ex As Exception

        End Try
    End Sub

    Private Sub txtSubtotal_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtSubtotal.LostFocus
        HitungTotal()
    End Sub

    Private Sub txtDiscPersen_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtDiscPersen.LostFocus
        HitungTotal()
    End Sub

    Private Sub txtDiscRp_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtDiscRp.LostFocus
        HitungTotal()
    End Sub

    Private Sub txtDiscTotal_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtDiscTotal.LostFocus
        HitungTotal()
    End Sub

    Private Sub txtBiaya_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtBiaya.LostFocus
        HitungTotal()
    End Sub

    Private Sub txtTotal_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtTotal.KeyDown
        If e.KeyCode = Keys.Enter Then
            cmdSave.Focus()
        End If
    End Sub

    Private Sub txtTotal_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtTotal.LostFocus
        HitungTotal()
    End Sub

    Private Sub txtBayar_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtBayar.LostFocus
        HitungTotal()
    End Sub

    Private Sub txtSisa_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtSisa.LostFocus
        HitungTotal()
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
    Private Function CariBarang(ByRef IDBarang As Long, ByRef NamaBarang As String, ByRef KodeBarang As String) As Boolean
        Dim x As Boolean = False
        Dim SQL As String = ""
        Dim oDS As New DataSet
        Try
            SQL = "SELECT NoID,Kode,Nama FROM MBarang WHERE IsActive=1 AND (ISNULL(KODE,'')<>'') AND (UPPER(Kode) = '" & NamaBarang.Replace("'", "''").ToUpper & "' OR UPPER(NAMA) = '" & NamaBarang.Replace("'", "''").ToUpper & "') ORDER BY Kode"
            oDS = ExecuteDataset("Tbl", SQL)
            If oDS.Tables("Tbl").Rows.Count >= 1 Then
                NamaBarang = NullToStr(oDS.Tables(0).Rows(0).Item("Nama"))
                KodeBarang = NullToStr(oDS.Tables(0).Rows(0).Item("Kode"))
                IDBarang = NullToLong(oDS.Tables(0).Rows(0).Item("NoID"))
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
        Dim IDDetil As Long = -1
        Dim frmEntri As New frmEntriLPBD
        Dim Konversi As Double = 0.0
        Try
            If txtGudang.Text = "" Then Exit Sub
            If CariBarang(IDBarang, NamaBarang, KodeBarang) Then
                If XtraMessageBox.Show("Ingin menerima item dengan Kode Stock " & KodeBarang & " Nama Stock " & NamaBarang, "Fast Entri Says", MessageBoxButtons.YesNo) = Windows.Forms.DialogResult.Yes Then
                    IDDetil = GetNewID("MLPBD", "NoID")
                    Konversi = NullToDbl(EksekusiSQlSkalarNew("SELECT Konversi FROM MBarangD WHERE IsBeli=1 AND IDBarang=" & IDBarang & " AND IDSatuan=" & DefIDSatuan))
                    If Konversi <> 0 Then
                        SQL = "INSERT INTO MLPBD (NoID,IDGudang,IDBarang,IDSatuan,Konversi,IDLPB,NoUrut,Tgl,Jam) VALUES " & vbCrLf
                        SQL &= "(" & IDDetil & "," & NullToLong(txtGudang.EditValue) & "," & IDBarang & "," & DefIDSatuan & "," & FixKoma(Konversi) & "," & NoID & "," & GetNewID("MLPBD", "NoUrut", " WHERE IDLPB=" & NoID) & ",GetDate(),GetDate())"
                    Else
                        SQL = "INSERT INTO MLPBD (NoID,IDGudang,IDBarang,IDLPB,NoUrut,Tgl,Jam) VALUES " & vbCrLf
                        SQL &= "(" & IDDetil & "," & NullToLong(txtGudang.EditValue) & "," & IDBarang & "," & NoID & "," & GetNewID("MLPBD", "NoUrut", " WHERE IDLPB=" & NoID) & ",GetDate(),GetDate())"
                    End If
                    EksekusiSQL(SQL)
                    frmEntri.IsNew = False
                    frmEntri.NoID = IDDetil
                    frmEntri.IDLPB = NoID
                    frmEntri.IDGudang = NullToLong(txtGudang.EditValue)
                    frmEntri.IDSupplier = NullToLong(txtKodeSupplier.EditValue)
                    frmEntri.IsFastEntri = True
                    frmEntri.FormPemanggil = Me
                    frmEntri.Show()
                    frmEntri.Focus()
                    'If frmEntri.ShowDialog(Me)= Windows.Forms.DialogResult.OK Then
                    '    RefreshDetil()
                    '    GV1.ClearSelection()
                    '    GV1.FocusedRowHandle = GV1.LocateByDisplayText(0, GV1.Columns("NoID"), IDDetil.ToString("#,##0"))
                    '    GV1.SelectRow(GV1.FocusedRowHandle)
                    'Else
                    '    SQL = "DELETE FROM MLPBD WHERE NoID=" & IDDetil
                    '    EksekusiSQL(SQL)
                    '    RefreshDetil()
                    '    GV1.ClearSelection()
                    '    GV1.FocusedRowHandle = GV1.LocateByDisplayText(0, GV1.Columns("NoID"), IDDetil.ToString("#,##0"))
                    '    GV1.SelectRow(GV1.FocusedRowHandle)
                    'End If
                End If
            End If
        Catch ex As Exception
            DevExpress.XtraEditors.XtraMessageBox.Show("Kesalahan : " & ex.Message, "Fast Entri Says", MessageBoxButtons.OK)
            'frmEntri.Dispose()
        End Try
    End Sub

    Private Sub cmdBAru_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdBAru.Click
        Dim frmEntri As New frmEntriLPBD
        Try
            Dim IDDetil As Long = -1
            frmEntri.NoID = IDDetil
            frmEntri.IsNew = True
            frmEntri.IDLPB = NoID
            If txtGudang.Text <> "" Then
                frmEntri.IDGudang = NullTolong(txtGudang.EditValue)
                frmEntri.IDSupplier = NullTolong(txtKodeSupplier.EditValue)
                frmEntri.NoID = IDDetil
                frmEntri.IsNew = True
                frmEntri.IDLPB = NoID
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
        Dim frmEntri As New frmEntriLPBD
        Try
            Dim row As System.Data.DataRow = view.GetDataRow(GV1.FocusedRowHandle)
            Dim dc As Integer = GV1.FocusedRowHandle
            Dim IDDetil As Long = NullTolong(row("NoID"))
            If txtGudang.Text <> "" Then
                frmEntri.IDGudang = NullTolong(txtGudang.EditValue)
                frmEntri.IDSupplier = NullTolong(txtKodeSupplier.EditValue)
                frmEntri.IsNew = False
                frmEntri.NoID = IDDetil
                frmEntri.IDLPB = NoID
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

    Private Sub rbDari_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rbDari.SelectedIndexChanged
        RubahDari()
    End Sub
    Private Sub RubahDari()
        RefreshDataKontak()
        If rbDari.SelectedIndex = 0 Then
            If pTipe = pStatus.Baru Then
                txtKodeSupplier.EditValue = DefIDSupplier
                txtGudang.EditValue = DefIDGudangSupplier
                RubahSupplier()
            End If
        Else
            If pTipe = pStatus.Baru Then
                txtKodeSupplier.EditValue = DefIDCustomer
                txtGudang.EditValue = DefIDGudangCustomer
                RubahSupplier()
            End If
        End If
    End Sub
    Private Sub txtGudang_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtGudang.EditValueChanged
        RubahGudang()
    End Sub
    Private Sub RubahGudang()
        txtWilayahGudang.Text = NullTostr(EksekusiSQlSkalarNew("SELECT MWilayah.Nama FROM MGudang LEFT JOIN MWilayah ON MWilayah.NOID=MGudang.IDWilayah WHERE MGudang.NoID=" & NullTolong(txtGudang.EditValue)))
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

    Private Sub txtKode_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtKode.EditValueChanged

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
        End If
    End Sub

    Private Sub txtGudang_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtGudang.KeyDown
        If e.KeyCode = Keys.F7 Then
            Dim x As New frmOtorisasiAdmin
            Try
                If txtGudang.Properties.ReadOnly Then
                    If x.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
                        IDAdmin = x.IDUserAdmin
                        txtGudang.Properties.ReadOnly = False
                    End If
                End If
            Catch ex As Exception
                XtraMessageBox.Show("Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End If
    End Sub
End Class