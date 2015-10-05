Imports System.Data
Imports System.Data.SqlClient
Imports DevExpress.XtraEditors
Imports DevExpress.Utils
Imports DevExpress.XtraGrid.Views.Base
Imports System.IO.File

Public Class frmEntriDO
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
    Dim IDSPKOld As Long = -1

    'For Back Action
    Public FormNameDaftar As String = ""
    Public TableNameDaftar As String = ""
    Public TextDaftar As String = ""
    Public FormEntriDaftar As String = ""
    Public TableMasterDaftar As String = ""
    Dim IsTempInsertBaru As Boolean = False
    Dim IsLoad As Boolean = True

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
            txtKodeCustomer.Properties.DataSource = ds.Tables("master")
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
            RefreshDataSPKRetur()
            ds.Dispose()
        End Try
    End Sub
    Private Sub RefreshDataSPKRetur()
        Dim ds As New DataSet
        Try
            If rbDari.SelectedIndex = 1 Then 'Customer (SPK)
                SQL = "SELECT MJual.NoID, MJual.Kode AS NoReff, MJual.Tanggal " & vbCrLf & _
                      " FROM (MJualD " & vbCrLf & _
                      " INNER JOIN MJual ON MJual.NoID=MJualD.IDJual) " & vbCrLf & _
                      " WHERE ((MJualD.Qty*MJualD.Konversi)-IsNull((SELECT SUM(MDOD.Qty*MDOD.Konversi) FROM MDOD INNER JOIN MDO ON MDO.NoID=MDOD.IDDO WHERE MDOD.IDJualD=MJualD.NoID),0))>0 AND IsNull(MJual.IsPosted,0)=1 AND MJualD.IDGudang=" & NullToLong(txtGudang.EditValue) & " AND MJual.IDCustomer=" & NullToLong(txtKodeCustomer.EditValue) & vbCrLf & _
                      " GROUP BY MJual.NoID, MJual.Kode, MJual.Tanggal, MJual.IsPosted, MJual.IDWilayah, MJual.IDCustomer"

                'SQL = "SELECT NoID, Kode, Tanggal, SUM(QtySisa) AS QtySisa FROM (SELECT MJual.NoID, MJual.Kode, MJual.Tanggal, (MJualD.Qty*MJualD.Konversi)-IsNull((SELECT SUM(MDOD.Qty*MDOD.Konversi) FROM MDOD WHERE MDOD.IDDO<>" & NoID & " AND MDOD.IDJualD=MJualD.NoID),0) AS QtySisa FROM MJual INNER JOIN MJualD ON MJualD.IDJual=MJual.NoID "
                'SQL &= " WHERE MJualD.IsJual = 1 And (MJual.IsSelesai = 0 OR MJual.IsSelesai IS NULL) And MJual.IsPosted = 1 And MJualD.IDGudang=" & NullToLong(txtGudang.EditValue) & " AND MJual.IDCustomer=" & NullToLong(txtKodeCustomer.EditValue) & " AND (MJualD.Qty*MJualD.Konversi)-IsNull((SELECT SUM(MDOD.Qty*MDOD.Konversi) FROM MDOD WHERE MDOD.IDDO<>" & NoID & " AND MDOD.IDJualD=MJualD.NoID),0)>0"
                'SQL &= " ) X GROUP BY NoID, Kode, Tanggal"
            Else 'Supplier (Retur Beli)
                'SQL = "SELECT NoID, Kode, Tanggal, SUM(QtySisa) AS QtySisa FROM (SELECT MReturBeli.NoID, MReturBeli.Kode, MReturBeli.Tanggal, (MReturBeliD.Qty*MReturBeliD.Konversi)-IsNull((SELECT SUM(MDOD.Qty*MDOD.Konversi) FROM MDOD WHERE MDOD.IDDO<>" & NoID & " AND MDOD.IDJualD=MReturBeliD.NoID),0) AS QtySisa FROM MReturBeli INNER JOIN MReturBeliD ON MReturBeliD.IDReturBeli=MReturBeli.NoID "
                'SQL &= " WHERE MReturBeli.IsPosted = 1 And MReturBeliD.IDGudang=" & NullToLong(txtGudang.EditValue) & " And MReturBeli.IDSupplier=" & NullToLong(txtKodeCustomer.EditValue) & " AND (MReturBeliD.Qty*MReturBeliD.Konversi)-IsNull((SELECT SUM(MDOD.Qty*MDOD.Konversi) FROM MDOD WHERE MDOD.IDDO<>" & NoID & " AND MDOD.IDJualD=MReturBeliD.NoID),0)>0"
                'SQL &= " ) X GROUP BY NoID, Kode, Tanggal"
                SQL = "SELECT MReturBeli.NoID, MReturBeli.Kode AS NoReff, MReturBeli.Tanggal " & vbCrLf & _
                      " FROM (MReturBeliD " & vbCrLf & _
                      " INNER JOIN MReturBeli ON MReturBeli.NoID=MReturBeliD.IDReturBeli) " & vbCrLf & _
                      " WHERE ((MReturBeliD.Qty*MReturBeliD.Konversi)-IsNull((SELECT SUM(MDOD.Qty*MDOD.Konversi) FROM MDOD INNER JOIN MDO ON MDO.NoID=MDOD.IDDO WHERE MDOD.IDJualD=MReturBeliD.NoID),0))>0 AND IsNull(MReturBeli.IsSelesai,0)=0 AND IsNull(MReturBeli.IsPosted,0)=1 AND MReturBeli.IDGudang=" & NullToLong(txtGudang.EditValue) & " AND MReturBeli.IDSupplier=" & NullToLong(txtKodeCustomer.EditValue) & vbCrLf & _
                      " GROUP BY MReturBeli.NoID, MReturBeli.Kode, MReturBeli.Tanggal, MReturBeli.IsSelesai, MReturBeli.IsPosted, MReturBeli.IDWilayah, MReturBeli.IDSupplier"
            End If
            ds = ExecuteDataset("MSPKD", SQL)
            txtSPK.Properties.DataSource = ds.Tables("MSPKD")
            txtSPK.Properties.ValueMember = "NoID"
            txtSPK.Properties.DisplayMember = "NoReff"
        Catch ex As Exception
            XtraMessageBox.Show("Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            ds.Dispose()
        End Try
    End Sub
    Dim DefImageList As New ImageList

    Private Sub frmEntriDO_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        If IsTempInsertBaru Then
            If XtraMessageBox.Show("Yakin ingin keluar?" & vbCrLf & "Ya, akan menghapus transaksi dan detil item.", NamaAplikasi, MessageBoxButtons.YesNo, MessageBoxIcon.Information, MessageBoxDefaultButton.Button2) = Windows.Forms.DialogResult.Yes Then
                EksekusiSQL("DELETE FROM MDO WHERE NoID=" & NoID)
                EksekusiSQL("DELETE FROM MDOD WHERE IDDO=" & NoID)
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
            If System.IO.File.Exists(FolderLayouts & Me.Name & SearchLookUpEdit1View.Name & ".xml") Then
                SearchLookUpEdit1View.RestoreLayoutFromXml(FolderLayouts & Me.Name & SearchLookUpEdit1View.Name & ".xml")
            End If
            LoadLayout()
            FungsiControl.SetForm(Me)
            HighLightTxt()
            IsLoad = False
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
                SQL = "SELECT MDO.*, MAlamat.Alamat AS Alamat, MAlamat.Nama AS NamaAlamat, MAlamat.Kode as KodeAlamat, MWilayah.Nama AS Wilayah FROM MDO LEFT JOIN MAlamat ON MAlamat.NoID=MDO.IDCustomer LEFT JOIN (MGudang LEFT JOIN MWilayah ON MWilayah.NoID=MGudang.IDWilayah) ON MGudang.NoID=MDO.IDGudangPengiriman WHERE MDO.NoID=" & NoID
                DS = ExecuteDataset("Tabel", SQL)
                If DS.Tables("Tabel").Rows.Count >= 1 Then
                    rbDari.SelectedIndex = NullToLong(DS.Tables(0).Rows(0).Item("Dari"))
                    RefreshDataKontak()
                    txtKodeCustomer.EditValue = NullToLong(DS.Tables(0).Rows(0).Item("IDCustomer"))
                    'txtKodeCustomer.Text = NullTostr(DS.Tables(0).Rows(0).Item("KodeAlamat"))
                    txtNamaCustomer.Text = NullToStr(DS.Tables(0).Rows(0).Item("NamaAlamat"))
                    txtAlamatCustomer.Text = NullToStr(DS.Tables(0).Rows(0).Item("Alamat"))
                    txtKode.Text = NullToStr(DS.Tables(0).Rows(0).Item("Kode"))
                    KodeLama = txtKode.Text
                    txtKodeReff.Text = NullToStr(DS.Tables(0).Rows(0).Item("KodeReff"))
                    Tgl.DateTime = NullToDate(DS.Tables(0).Rows(0).Item("Tanggal"))
                    tglStok.DateTime = NullToDate(DS.Tables(0).Rows(0).Item("TanggalStock"))
                    tglJatuhTempo.DateTime = NullToDate(DS.Tables(0).Rows(0).Item("JatuhTempo"))
                    tglSJ.DateTime = NullToDate(DS.Tables(0).Rows(0).Item("TanggalSJ"))
                    txtNoSJ.Text = NullToStr(DS.Tables(0).Rows(0).Item("NoSJ"))
                    txtSubtotal.EditValue = NullToDbl(DS.Tables(0).Rows(0).Item("SubTotal"))
                    txtDiscPersen.EditValue = NullToDbl(DS.Tables(0).Rows(0).Item("DiskonNotaProsen"))
                    txtDiscRp.EditValue = NullToDbl(DS.Tables(0).Rows(0).Item("DiskonNotaRp"))
                    txtDiscTotal.EditValue = NullToDbl(DS.Tables(0).Rows(0).Item("DiskonNotaTotal"))
                    txtBiaya.EditValue = NullToDbl(DS.Tables(0).Rows(0).Item("Biaya"))
                    txtTotal.EditValue = NullToDbl(DS.Tables(0).Rows(0).Item("Total"))
                    txtBayar.EditValue = NullToDbl(DS.Tables(0).Rows(0).Item("Bayar"))
                    txtSisa.EditValue = NullToDbl(DS.Tables(0).Rows(0).Item("Sisa"))
                    IsPosted = NullToBool(DS.Tables(0).Rows(0).Item("IsPosted"))
                    txtGudang.EditValue = NullToLong(DS.Tables(0).Rows(0).Item("IDGudangPengiriman"))
                    txtPenerima.EditValue = NullToLong(DS.Tables(0).Rows(0).Item("IDBagPenjualan"))
                    txtPengirim.Text = NullToStr(DS.Tables(0).Rows(0).Item("Pengirim"))
                    txtCatatan.Text = NullToStr(DS.Tables(0).Rows(0).Item("Keterangan"))
                    txtWilayahGudang.Text = NullToStr(DS.Tables(0).Rows(0).Item("Wilayah"))
                    IDSPKOld = NullToLong(DS.Tables(0).Rows(0).Item("IDJual"))
                    RefreshDataSPKRetur()
                    txtSPK.EditValue = IDSPKOld
                    If IsPosted Then
                        txtKodeCustomer.Properties.ReadOnly = True
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
            If rbDari.SelectedIndex = 0 Then
                strsql = "select '' AS KodePacking, '' AS NoPacking, MDOD.*, MBarang.CtnPcs AS IsiCtn, MBarang.Kode as KodeStock,MBarang.Nama as NamaStock,MSatuan.Kode as Satuan,Mgudang.Kode as KodeGudang, MReturBeli.Kode AS KodeReff, MReturBeliD.NoUrut " & vbCrLf
                strsql &= " From (MDOD Inner Join MDO On MDOD.IDDO=MDO.NoID) " & vbCrLf
                strsql &= " LEFT JOIN MBarang ON MDOD.IDBarang=MBarang.NoID " & vbCrLf
                strsql &= " LEFT JOIN (MReturBeliD INNER JOIN MReturBeli ON MReturBeli.NoID=MReturBeliD.IDReturBeli) ON MDOD.IDJualD=MReturBeliD.NoID " & vbCrLf
                strsql &= " LEFT JOIN MSatuan ON MDOD.IDSatuan=MSatuan.NoID " & vbCrLf
                strsql &= " LEFT JOIN MGudang ON MDOD.IDGudang=MGudang.NoID " & vbCrLf
                strsql &= " where MDOD.IDDO = " & NoID
            Else
                strsql = "select MPacking.Kode AS KodePacking, MPackingD.NoPacking, MDOD.*, MBarang.CtnPcs AS IsiCtn, MBarang.Kode as KodeStock,MBarang.Nama as NamaStock,MSatuan.Kode as Satuan,Mgudang.Kode as KodeGudang, MJual.Kode AS KodeReff, MJualD.NoUrut " & vbCrLf
                strsql &= " From (MDOD Inner Join MDO On MDOD.IDDO=MDO.NoID) " & vbCrLf
                strsql &= " LEFT JOIN MBarang ON MDOD.IDBarang=MBarang.NoID " & vbCrLf
                strsql &= " LEFT JOIN ((MJualD LEFT JOIN (MPackingD INNER JOIN MPacking ON MPacking.NoID=MPackingD.IDPacking) ON MJualD.IDPackingD=MPackingD.NoID) INNER JOIN MJual ON MJual.NoID=MJualD.IDJual) ON MDOD.IDJualD=MJualD.NoID " & vbCrLf
                strsql &= " LEFT JOIN MSatuan ON MDOD.IDSatuan=MSatuan.NoID " & vbCrLf
                strsql &= " LEFT JOIN MGudang ON MDOD.IDGudang=MGudang.NoID " & vbCrLf
                strsql &= " where MDOD.IDDO = " & NoID
            End If
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
            If pTipe = pStatus.Edit Then
                RefreshDataSPKRetur()
            End If
        Catch ex As Exception
            XtraMessageBox.Show(ex.Message, ".:: Pesan Kesalahan ::.", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
        Windows.Forms.Cursor.Current = curentcursor
        Application.DoEvents()
    End Sub
    Private Sub HitungTotal()
        Dim temp As Double = 0.0
        Dim ds As New DataSet
        Try
            For i = 0 To GV1.RowCount
                temp = temp + NullToDbl(GV1.GetRowCellValue(i, "Jumlah"))
            Next
            txtSubtotal.EditValue = temp
            txtDiscTotal.EditValue = (NullToDbl(txtDiscPersen.EditValue) * NullToDbl(txtSubtotal.EditValue) / 100) '+ txtDiscRp.EditValue
            txtTotal.EditValue = txtSubtotal.EditValue - txtDiscTotal.EditValue + txtBiaya.EditValue
            txtSisa.EditValue = txtTotal.EditValue - txtBayar.EditValue
            txtTotalCTN.EditValue = NullToDbl(EksekusiSQlSkalarNew("SELECT SUM(Ctn) FROM MDOD WHERE IDDO=" & NoID))
            txtKodeReff.Text = ""
            SQL = "SELECT MJual.Kode FROM MDOD LEFT JOIN (MJualD INNER JOIN MJual ON MJual.NoID=MJualD.IDJual) ON MJualD.NoID=MDOD.IDJualD WHERE MDOD.IDDO=" & NoID & " GROUP BY MJual.Kode ORDER BY MJual.Kode"
            ds = ExecuteDataset("MSPK", SQL)
            For i As Integer = 0 To ds.Tables("MSPK").Rows.Count - 1
                txtKodeReff.Text = txtKodeReff.Text & IIf(i = 0, "", ", ") & NullToStr(ds.Tables("MSPK").Rows(i).Item("Kode"))
            Next
        Catch ex As Exception
            XtraMessageBox.Show("Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            ds.Dispose()
        End Try
    End Sub
    Private Sub IsiDefault()
        Tgl.DateTime = TanggalSystem
        tglJatuhTempo.DateTime = DateAdd(DateInterval.Month, 1, Tgl.DateTime)
        tglSJ.DateTime = TanggalSystem
        tglStok.DateTime = TanggalSystem
        SetTombol()

        txtGudang.EditValue = DefIDGudang
        RubahGudang()
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
        Dim ds As New DataSet
        Dim Harga As Double = 0.0
        Dim Disc1 As Double = 0.0
        Dim Disc2 As Double = 0.0
        Dim Jumlah As Double = 0.0
        If Not IsPosted Then
            If pTipe = pStatus.Baru Then
                txtKode.Text = clsKode.MintaKodeBaru("SJ", "MDO", Tgl.DateTime, DefIDWilayah, 5)
            End If
            If IsValidasi() Then
                If Simpan(PesanSalah) = True Then
                    If pTipe = pStatus.Baru Then
                        KodeLama = txtKode.Text
                        pTipe = pStatus.Edit
                        SetTombol()
                        txtBarang.Focus()
                        'txtKodeCustomer.Properties.ReadOnly = True
                        IsTempInsertBaru = True

                        IDSPKOld = NullToLong(txtSPK.EditValue)
                        If rbDari.SelectedIndex = 0 Then
                            SQL = "SELECT MReturBeliD.*, MBarang.CtnPcs, MSatuanBase.NoID AS IDSatuanBase, MSatuanBase.Konversi AS KonversiBase, (MReturBeliD.Qty*MReturBeliD.Konversi)-IsNull((SELECT SUM(MDOD.Qty*MDOD.Konversi) FROM MDOD INNER JOIN MDO ON MDO.NoID=MDOD.IDDO WHERE MDOD.IDJualD=MReturBeliD.NoID),0) AS SisaQty " & _
                                  " FROM MReturBeliD " & _
                                  " LEFT JOIN MBarang ON MBarang.NoID=MReturBeliD.IDBarang " & _
                                  " LEFT JOIN (MSatuan LEFT JOIN MSatuan MSatuanBase ON MSatuanBase.NoID=MSatuan.IDBase) ON MSatuan.NoID=MReturBeliD.IDSatuan " & _
                                  " WHERE MReturBeliD.IDGudang=" & NullToLong(txtGudang.EditValue) & " AND MReturBeliD.IDReturBeli=" & NullToLong(txtSPK.EditValue) & _
                                  " AND (MReturBeliD.Qty*MReturBeliD.Konversi)-IsNull((SELECT SUM(MDOD.Qty*MDOD.Konversi) FROM MDOD INNER JOIN MDO ON MDO.NoID=MDOD.IDDO WHERE MDOD.IDJualD=MReturBeliD.NoID),0)>0"
                        Else
                            SQL = "SELECT MJualD.*, MBarang.CtnPcs, MSatuanBase.NoID AS IDSatuanBase, MSatuanBase.Konversi AS KonversiBase, (MJualD.Qty*MJualD.Konversi)-IsNull((SELECT SUM(MDOD.Qty*MDOD.Konversi) FROM MDOD INNER JOIN MDO ON MDO.NoID=MDOD.IDDO WHERE MDOD.IDJualD=MJualD.NoID),0) AS SisaQty " & _
                                  " FROM MJualD " & _
                                  " LEFT JOIN MBarang ON MBarang.NoID=MJualD.IDBarang " & _
                                  " LEFT JOIN (MSatuan LEFT JOIN MSatuan MSatuanBase ON MSatuanBase.NoID=MSatuan.IDBase) ON MSatuan.NoID=MJualD.IDSatuan " & _
                                  " WHERE MJualD.IDGudang=" & NullToLong(txtGudang.EditValue) & " AND MJualD.IDJual=" & NullToLong(txtSPK.EditValue) & _
                                  " AND (MJualD.Qty*MJualD.Konversi)-IsNull((SELECT SUM(MDOD.Qty*MDOD.Konversi) FROM MDOD INNER JOIN MDO ON MDO.NoID=MDOD.IDDO WHERE MDOD.IDJualD=MJualD.NoID),0)>0" & _
                                  " ORDER BY MJualD.NoUrut "
                        End If
                        ds = ExecuteDataset("MJualD", SQL)
                        With ds.Tables("MJualD")
                            For i As Integer = 0 To .Rows.Count - 1
                                'txtQty.EditValue / IsiKarton * txtKonversi.EditValue
                                If NullToDbl(.Rows(i).Item("Konversi")) <= NullToDbl(.Rows(i).Item("SisaQty")) Then
                                    'Harga = clsPostingPenjualan.HargaJual(NullToLong(.Rows(i).Item("IDBarang")), NullToLong(.Rows(i).Item("IDSatuan")), NullToLong(txtKodeCustomer.EditValue), Disc1, Disc2)
                                    Jumlah = NullToDbl(.Rows(i).Item("Qty")) * (Bulatkan((Harga * (1 - (Disc1 / 100)) * (1 - (Disc2 / 100)) * (1 - (0 / 100))), 0) - 0 - 0 - 0)
                                    SQL = "INSERT INTO [MDOD] ([NoID] ,[Tgl] ,[Jam] ,[IDDO] ,[IDBarang] ,[IDSatuan] ,[IDGudang] ,[Ctn] ,[Qty] ,[Harga] ,[DiscPersen1] ,[DiscPersen2] ,[DiscPersen3] ,[Disc1] ,[Disc2] ,[Disc3] ,[Jumlah],[Catatan] ,[NoUrut] ,[HargaPcs] ,[QtyPcs] ,[IDAsal] ,[Konversi] ,[IDJualD])" & _
                                    " VALUES (" & NullToLong(GetNewID("MDOD", "NoID")) & " ,getdate() ,getdate() ," & NoID & " ," & NullToLong(.Rows(i).Item("IDBarang")) & " ," & NullToLong(.Rows(i).Item("IDSatuan")) & " ," & NullToLong(txtGudang.EditValue) & " ," & FixKoma(IIf(NullToDbl(.Rows(i).Item("CtnPcs")) = 0, 0, NullToDbl(.Rows(i).Item("SisaQty")) / NullToDbl(.Rows(i).Item("CtnPcs")))) & " ," & FixKoma(NullToDbl(.Rows(i).Item("SisaQty")) / IIf(NullToDbl(.Rows(i).Item("Konversi")) = 0, 1, NullToDbl(.Rows(i).Item("Konversi")))) & " ," & FixKoma(Harga) & " ," & FixKoma(Disc1) & _
                                    " ," & FixKoma(Disc2) & " ," & FixKoma(0) & " ," & FixKoma(0) & " ," & FixKoma(0) & " ," & FixKoma(0) & " ," & FixKoma(Jumlah) & " ,'" & FixApostropi(NullToStr(.Rows(i).Item("Catatan"))) & "' ," & _
                                    NullToLong(GetNewID("MDOD", "NoUrut", " WHERE MDOD.IDDO=" & NoID)) & " ," & FixKoma(Harga / IIf(NullToDbl(.Rows(i).Item("Konversi")) = 0, 1, NullToDbl(.Rows(i).Item("Konversi")))) & " ," & FixKoma(NullToDbl(.Rows(i).Item("SisaQty"))) & " ," & FixKoma(NullToDbl(.Rows(i).Item("IDAsal"))) & " ," & FixKoma(NullToDbl(.Rows(i).Item("Konversi"))) & " ," & NullToLong(.Rows(i).Item("NoID")) & ")"
                                Else
                                    'Harga = clsPostingPenjualan.HargaJual(NullToLong(.Rows(i).Item("IDBarang")), NullToLong(.Rows(i).Item("IDSatuanBase")), NullToLong(txtKodeCustomer.EditValue), Disc1, Disc2)
                                    Jumlah = NullToDbl(.Rows(i).Item("Qty")) * (Bulatkan((Harga * (1 - (Disc1 / 100)) * (1 - (Disc2 / 100)) * (1 - (0 / 100))), 0) - 0 - 0 - 0)
                                    SQL = "INSERT INTO [MDOD] ([NoID] ,[Tgl] ,[Jam] ,[IDDO] ,[IDBarang] ,[IDSatuan] ,[IDGudang] ,[Ctn] ,[Qty] ,[Harga] ,[DiscPersen1] ,[DiscPersen2] ,[DiscPersen3] ,[Disc1] ,[Disc2] ,[Disc3] ,[Jumlah] ,[Catatan] ,[NoUrut] ,[HargaPcs] ,[QtyPcs] ,[IDAsal] ,[Konversi] ,[IDJualD])" & _
                                    " VALUES (" & NullToLong(GetNewID("MDOD", "NoID")) & " ,getdate() ,getdate() ," & NoID & " ," & NullToLong(.Rows(i).Item("IDBarang")) & " ," & NullToLong(.Rows(i).Item("IDSatuanBase")) & " ," & NullToLong(txtGudang.EditValue) & " ," & FixKoma(IIf(NullToDbl(.Rows(i).Item("CtnPcs")) = 0, 0, NullToDbl(.Rows(i).Item("SisaQty")) / NullToDbl(.Rows(i).Item("CtnPcs")))) & " ," & FixKoma(NullToDbl(.Rows(i).Item("SisaQty")) / IIf(NullToDbl(.Rows(i).Item("KonversiBase")) = 0, 1, NullToDbl(.Rows(i).Item("KonversiBase")))) & " ," & FixKoma(Harga) & " ," & FixKoma(Disc1) & _
                                    " ," & FixKoma(Disc2) & " ," & FixKoma(0) & " ," & FixKoma(0) & " ," & FixKoma(0) & " ," & FixKoma(0) & " ," & FixKoma(Jumlah) & " ,'" & FixApostropi(NullToStr(.Rows(i).Item("Catatan"))) & "' ," & _
                                    NullToLong(GetNewID("MDOD", "NoUrut", " WHERE MDOD.IDDO=" & NoID)) & " ," & FixKoma(Harga / IIf(NullToDbl(.Rows(i).Item("KonversiBase")) = 0, 1, NullToDbl(.Rows(i).Item("KonversiBase")))) & " ," & FixKoma(NullToDbl(.Rows(i).Item("SisaQty"))) & " ," & FixKoma(NullToDbl(.Rows(i).Item("IDAsal"))) & " ," & FixKoma(NullToDbl(.Rows(i).Item("KonversiBase"))) & " ," & NullToLong(.Rows(i).Item("NoID")) & ")"
                                End If
                                EksekusiSQL(SQL)
                            Next
                        End With
                        RefreshDetil()
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

                        'DialogResult = Windows.Forms.DialogResult.OK
                        IsTempInsertBaru = False
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
        If txtKodeCustomer.Text = "" Then
            XtraMessageBox.Show("Customer / supplier masih kosong.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
            txtKodeCustomer.Focus()
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
        If CekKodeValid(txtKode.Text, KodeLama, "MDO", "Kode", IIf(pTipe = pStatus.Edit, True, False)) Then
            XtraMessageBox.Show("Kode sudah dipakai.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
            txtKode.Focus()
            Return False
            Exit Function
        End If
        'If pTipe = pStatus.Edit AndAlso txtSPK.Text = "" Then
        '    If XtraMessageBox.Show("Item " & IIf(rbDari.SelectedIndex = 0, "Retur Beli", "Jual") & " masih kosong." & vbCrLf & "Ingin melanjutkan penyimpanan?", NamaAplikasi, MessageBoxButtons.YesNo, MessageBoxIcon.Information) = Windows.Forms.DialogResult.No Then
        '        txtSPK.Focus()
        '        Return False
        '        Exit Function
        '    End If
        'End If
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
                NoID = GetNewID("MDO")
                SQL = "INSERT INTO MDO (Dari,IDJual,NoID,IDWilayah,Kode,KodeReff,Tanggal,TanggalStock,JatuhTempo,"
                SQL &= " IDCustomer,TanggalSJ,NoSJ,SubTotal,DiskonNotaProsen,DiskonNotaRp,DiskonNotaTotal,"
                SQL &= "  Biaya, Total, Bayar, Sisa,IDGudangPengiriman,IDBagPenjualan,Pengirim,Keterangan,IDAdmin) VALUES (" & vbCrLf
                SQL &= rbDari.SelectedIndex & "," & NullToLong(txtSPK.EditValue) & ","
                SQL &= NoID & ","
                SQL &= DefIDWilayah & ","
                SQL &= "'" & FixApostropi(txtKode.Text) & "',"
                SQL &= "'" & FixApostropi(txtKodeReff.Text) & "',"
                SQL &= "'" & Tgl.DateTime.ToString("yyyy-MM-dd HH:mm") & "',"
                SQL &= "'" & tglStok.DateTime.ToString("yyyy-MM-dd HH:mm") & "',"
                SQL &= "'" & tglJatuhTempo.DateTime.ToString("yyyy-MM-dd HH:mm") & "',"
                SQL &= txtKodeCustomer.EditValue & ","
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
                SQL &= NullToLong(txtGudang.EditValue) & ","
                SQL &= NullToLong(txtPenerima.EditValue) & ",'"
                SQL &= FixApostropi(txtPengirim.Text) & "','"
                SQL &= FixApostropi(txtCatatan.Text) & "'," & IDAdmin & ")"

                EksekusiSQL(SQL)
            Else
                SQL = "UPDATE MDO SET "
                SQL &= "Dari=" & rbDari.SelectedIndex & ","
                SQL &= "IDJual=" & NullToLong(txtSPK.EditValue) & ","
                SQL &= "Kode='" & FixApostropi(txtKode.Text) & "',"
                SQL &= "KodeReff='" & FixApostropi(txtKodeReff.Text) & "',"
                If Not Tgl.Properties.ReadOnly Or Not txtKode.Properties.ReadOnly Then
                    SQL &= "IDAdmin=" & IDAdmin & ","
                End If
                SQL &= "Tanggal='" & Tgl.DateTime.ToString("yyyy-MM-dd HH:mm") & "',"
                SQL &= "TanggalStock='" & tglStok.DateTime.ToString("yyyy-MM-dd HH:mm") & "',"
                SQL &= "JatuhTempo='" & tglJatuhTempo.DateTime.ToString("yyyy-MM-dd HH:mm") & "',"
                SQL &= "IDCustomer=" & txtKodeCustomer.EditValue & ","
                SQL &= "TanggalSJ='" & tglSJ.DateTime.ToString("yyyy-MM-dd HH:mm") & "',"
                SQL &= "NoSJ='" & FixApostropi(txtNoSJ.Text) & "',"
                SQL &= "IDGudangPengiriman=" & NullToLong(txtGudang.EditValue) & ","
                SQL &= "IDBagPenjualan=" & NullToLong(txtPenerima.EditValue) & ","
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
                SQL = "UPDATE MDOD SET IDGudang=" & NullToLong(txtGudang.EditValue) & " WHERE IDDO=" & NoID
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
        If Exists(FolderLayouts & Me.Name & gvGudang.Name & ".xml") Then
            gvGudang.RestoreLayoutFromXml(FolderLayouts & Me.Name & gvGudang.Name & ".xml")
        End If
        If Exists(FolderLayouts & Me.Name & GV1.Name & ".xml") Then
            GV1.RestoreLayoutFromXml(FolderLayouts & Me.Name & GV1.Name & ".xml")
        End If
        If Exists(FolderLayouts & Me.Name & gvPenerima.Name & ".xml") Then
            gvPenerima.RestoreLayoutFromXml(FolderLayouts & Me.Name & gvPenerima.Name & ".xml")
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
                SearchLookUpEdit1View.SaveLayoutToXml(FolderLayouts & Me.Name & SearchLookUpEdit1View.Name & ".xml")
                gvGudang.SaveLayoutToXml(FolderLayouts & Me.Name & gvGudang.Name & ".xml")
                gvPenerima.SaveLayoutToXml(FolderLayouts & Me.Name & gvPenerima.Name & ".xml")
                gvSPK.SaveLayoutToXml(FolderLayouts & Me.Name & gvSPK.Name & ".xml")
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
                EksekusiSQL("Delete From MDOD where NoID=" & IDDetil.ToString)
                RefreshDetil()
            End If
            txtBarang.Focus()
        Catch ex As Exception
            XtraMessageBox.Show("Untuk mengedit pilih item satuan yang akan diedit terlebih dahulu lalu tekan tombol edit", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub txtKodeCustomer_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtKodeCustomer.EditValueChanged
        Dim TglAdd As Long = 0
        Try
            txtNamaCustomer.Text = NullToStr(SearchLookUpEdit1View.GetRowCellValue(SearchLookUpEdit1View.FocusedRowHandle, "Nama"))
            txtAlamatCustomer.Text = NullToStr(EksekusiSQlSkalarNew("SELECT Alamat FROM MAlamat WHERE NoID=" & NullToLong(txtKodeCustomer.EditValue)))
            TglAdd = NullToLong(EksekusiSQlSkalarNew("SELECT JatuhTempoCustomer FROM MAlamat WHERE NoID=" & NullToLong(txtKodeCustomer.EditValue)))
            If TglAdd = 0 Then
                tglJatuhTempo.DateTime = DateAdd(DateInterval.Month, 2, Tgl.DateTime)
            Else
                tglJatuhTempo.DateTime = DateAdd(DateInterval.Day, TglAdd, Tgl.DateTime)
            End If
            RefreshDataSPKRetur()
        Catch ex As Exception

        End Try
    End Sub

    Private Sub Tgl_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Tgl.EditValueChanged
        Dim TglAdd As Long = 0
        Try
            'txtNamaCustomer.Text = NullTostr(SearchLookUpEdit1View.GetRowCellValue(SearchLookUpEdit1View.FocusedRowHandle, "Nama"))
            'txtAlamatCustomer.Text = NullTostr(EksekusiSQlSkalarNew("SELECT Alamat FROM MAlamat WHERE NoID=" & NullTolong(txtKodeCustomer.EditValue)))
            TglAdd = NullToLong(EksekusiSQlSkalarNew("SELECT JatuhTempoCustomer FROM MAlamat WHERE NoID=" & NullToLong(txtKodeCustomer.EditValue)))
            If TglAdd = 0 Then
                tglJatuhTempo.DateTime = DateAdd(DateInterval.Month, 2, Tgl.DateTime)
            Else
                tglJatuhTempo.DateTime = DateAdd(DateInterval.Day, TglAdd, Tgl.DateTime)
            End If
            If pTipe = pStatus.Baru Then
                txtKode.Text = clsKode.MintaKodeBaru("SJ", "MDO", Tgl.DateTime, DefIDWilayah, 5)
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
            SQL = "SELECT NoID,Kode,Nama FROM MBarang WHERE IsActive=1 AND NOT (MBarang.KODE='' OR MBarang.KODE IS NULL) AND (UPPER(Kode) = '" & NamaBarang.Replace("'", "''").ToUpper & "' OR UPPER(NAMA) = '" & NamaBarang.Replace("'", "''").ToUpper & "' OR UPPER(Alias) = '" & NamaBarang.Replace("'", "''").ToUpper & "' OR UPPER(NamaAlias) = '" & NamaBarang.Replace("'", "''").ToUpper & "') ORDER BY Kode"
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
        Dim frmEntri As New frmEntriDOD
        Dim Konversi As Double = 0.0
        Try
            If txtGudang.Text = "" Then Exit Sub
            If CariBarang(IDBarang, NamaBarang, KodeBarang) Then
                If XtraMessageBox.Show("Ingin menambah Item Surat Jalan dengan Kode Stock " & KodeBarang & " Nama Stock " & NamaBarang, "Fast Entri Says", MessageBoxButtons.YesNo) = Windows.Forms.DialogResult.Yes Then
                    IDDetil = GetNewID("MDOD", "NoID")
                    Konversi = NullToDbl(EksekusiSQlSkalarNew("SELECT Konversi FROM MBarangD WHERE IsJual=1 AND IDBarang=" & IDBarang & " AND IDSatuan=" & DefIDSatuan))
                    If Konversi <> 0 Then
                        SQL = "INSERT INTO MDOD (NoID,IDGudang,IDBarang,IDSatuan,Konversi,IDDO,NoUrut,Tgl,Jam) VALUES " & vbCrLf
                        SQL &= "(" & IDDetil & "," & NullToLong(txtGudang.EditValue) & "," & IDBarang & "," & DefIDSatuan & "," & FixKoma(Konversi) & "," & NoID & "," & GetNewID("MDOD", "NoUrut", " WHERE IDDO=" & NoID) & ",GetDate(),GetDate())"
                    Else
                        SQL = "INSERT INTO MDOD (NoID,IDGudang,IDBarang,IDDO,NoUrut,Tgl,Jam) VALUES " & vbCrLf
                        SQL &= "(" & IDDetil & "," & NullToLong(txtGudang.EditValue) & "," & IDBarang & "," & NoID & "," & GetNewID("MDOD", "NoUrut", " WHERE IDDO=" & NoID) & ",GetDate(),GetDate())"
                    End If
                    EksekusiSQL(SQL)
                    frmEntri.IsNew = False
                    frmEntri.NoID = IDDetil
                    frmEntri.IDDO = NoID
                    frmEntri.IDGudang = NullToLong(txtGudang.EditValue)
                    frmEntri.IDSPK = NullToLong(txtSPK.EditValue)
                    frmEntri.IDCustomer = NullToLong(txtKodeCustomer.EditValue)
                    frmEntri.IsFastEntri = True
                    frmEntri.IsDariSupplier = IIf(rbDari.SelectedIndex = 0, True, False)
                    frmEntri.FormPemanggil = Me
                    frmEntri.Show()
                    frmEntri.Focus()
                    'If frmEntri.ShowDialog(Me)= Windows.Forms.DialogResult.OK Then
                    '    RefreshDetil()
                    '    GV1.ClearSelection()
                    '    GV1.FocusedRowHandle = GV1.LocateByDisplayText(0, GV1.Columns("NoID"), IDDetil.ToString("#,##0"))
                    '    GV1.SelectRow(GV1.FocusedRowHandle)
                    'Else
                    '    SQL = "DELETE FROM MDOD WHERE NoID=" & IDDetil
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
        Dim frmEntri As New frmEntriDOD
        Try
            Dim IDDetil As Long = -1
            frmEntri.NoID = IDDetil
            frmEntri.IsNew = True
            frmEntri.IDDO = NoID
            If txtGudang.Text <> "" Then
                frmEntri.IDGudang = NullToLong(txtGudang.EditValue)
                frmEntri.IDCustomer = NullToLong(txtKodeCustomer.EditValue)
                frmEntri.IDSPK = NullToLong(txtSPK.EditValue)
                frmEntri.IsDariSupplier = IIf(rbDari.SelectedIndex = 0, True, False)
                frmEntri.NoID = IDDetil
                frmEntri.IsNew = True
                frmEntri.IDDO = NoID
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
        Dim frmEntri As New frmEntriDOD
        Try
            Dim row As System.Data.DataRow = view.GetDataRow(GV1.FocusedRowHandle)
            Dim dc As Integer = GV1.FocusedRowHandle
            Dim IDDetil As Long = NullToLong(row("NoID"))
            If txtGudang.Text <> "" Then
                frmEntri.IDGudang = NullToLong(txtGudang.EditValue)
                frmEntri.IDCustomer = NullToLong(txtKodeCustomer.EditValue)
                frmEntri.IDSPK = NullToLong(txtSPK.EditValue)
                frmEntri.IsDariSupplier = IIf(rbDari.SelectedIndex = 0, True, False)

                frmEntri.IsNew = False
                frmEntri.NoID = IDDetil
                frmEntri.IDDO = NoID
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
        RefreshDataKontak()
    End Sub

    Private Sub txtGudang_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtGudang.EditValueChanged
        RubahGudang()
    End Sub
    Private Sub RubahGudang()
        Try
            txtWilayahGudang.Text = NullToStr(EksekusiSQlSkalarNew("SELECT MWilayah.Nama FROM MGudang LEFT JOIN MWilayah ON MWilayah.NOID=MGudang.IDWilayah WHERE MGudang.NoID=" & NullToLong(txtGudang.EditValue)))
            RefreshDataSPKRetur()
        Catch ex As Exception
            XtraMessageBox.Show("Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
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

    Private Sub gvSPK_DataSourceChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvSPK.DataSourceChanged
        If System.IO.File.Exists(FolderLayouts & Me.Name & gvSPK.Name & ".xml") Then
            gvSPK.RestoreLayoutFromXml(FolderLayouts & Me.Name & gvSPK.Name & ".xml")
        End If
        With gvSPK
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
    Sub HighLightTxt()
        For Each ctrl In LayoutControl1.Controls
            If TypeOf ctrl Is DevExpress.XtraEditors.TextEdit Then
                AddHandler TryCast(ctrl, DevExpress.XtraEditors.TextEdit).GotFocus, AddressOf txt_GotFocus
            End If
        Next
    End Sub
    Private Sub txt_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim txt As DevExpress.XtraEditors.TextEdit = TryCast(sender, DevExpress.XtraEditors.TextEdit)
        txt.SelectAll()
    End Sub
    Private Sub txtSPK_ButtonClick(ByVal sender As Object, ByVal e As DevExpress.XtraEditors.Controls.ButtonPressedEventArgs) Handles txtSPK.ButtonClick
        If e.Button.Index = 1 Then
            AmbilDataJualRetur()
        End If
    End Sub

    Private Sub txtSPK_EditValueChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtSPK.EditValueChanged

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

    Private Sub txtSPK_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtSPK.KeyDown
        If e.KeyCode = Keys.Enter Then
            AmbilDataJualRetur()
        End If
    End Sub
    'Private Sub AmbilDataJualRetur()
    '    Dim Ds As New DataSet
    '    Try
    '        If pTipe = pStatus.Edit AndAlso IsPosted = False AndAlso IDSPKOld <> NullTolong(txtSPK.EditValue) Then
    '            If XtraMessageBox.Show("Bersihkan item kirim barang dan ambil data dari " & IIf(rbDari.SelectedIndex = 0, "Retur Pembelian", "Jualan") & " ?", NamaAplikasi, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) = Windows.Forms.DialogResult.Yes Then
    '                EksekusiSQL("DELETE FROM MDOD WHERE IDDO=" & NoID)
    '                IDSPKOld = txtSPK.EditValue
    '                If rbDari.SelectedIndex = 1 Then
    '                    Ds = ExecuteDataset("MSPKD", "SELECT MJualD.* FROM MJualD WHERE MJualD.IDGudang=" & NullTolong(txtGudang.EditValue) & " AND MJualD.IDJual=" & NullTolong(txtSPK.EditValue) & " AND MJualD.IsJual=1 ORDER BY MJualD.NoUrut")
    '                Else
    '                    Ds = ExecuteDataset("MSPKD", "SELECT MReturBeliD.* FROM MReturBeliD WHERE MReturBeliD.IDGudang=" & NullTolong(txtGudang.EditValue) & " AND MReturBeliD.IDReturBeli=" & NullTolong(txtSPK.EditValue) & " ORDER BY MReturBeliD.NoUrut")
    '                End If
    '                With Ds.Tables("MSPKD")
    '                    If .Rows.Count >= 1 Then
    '                        For i As Integer = 0 To .Rows.Count - 1
    '                            SQL = "INSERT INTO [MDOD] ([NoID] ,[Tgl] ,[Jam] ,[IDDO] ,[IDBarang] ,[IDSatuan] ,[IDGudang] ,[Ctn] ,[Qty] ,[Harga] ,[DiscPersen1] ,[DiscPersen2] ,[DiscPersen3] ,[Disc1] ,[Disc2] ,[Disc3] ,[Jumlah] ,[IDAkunPersediaan] ,[Catatan] ,[NoUrut] ,[HargaPcs] ,[QtyPcs] ,[IDAsal] ,[Konversi] ,[IDServerAsal] ,[IDJualD] )"
    '                            SQL &= " VALUES (" & NullTolong(GetNewID("MDOD", "NoID")) & " ,getdate() ,getdate() ," & NoID & " ," & NullTolong(.Rows(i).Item("IDBarang")) & " ," & NullTolong(.Rows(i).Item("IDSatuan")) & " ," & NullTolong(.Rows(i).Item("IDGudang")) & " ," & FixKoma(NullToDbl(.Rows(i).Item("Ctn"))) & " ," & FixKoma(NullToDbl(.Rows(i).Item("Qty"))) & " ," & FixKoma(NullToDbl(.Rows(i).Item("Harga"))) & " ," & FixKoma(NullToDbl(.Rows(i).Item("DiscPersen1"))) & _
    '                            " ," & FixKoma(NullToDbl(.Rows(i).Item("DiscPersen2"))) & " ," & FixKoma(NullToDbl(.Rows(i).Item("DiscPersen3"))) & " ," & FixKoma(NullToDbl(.Rows(i).Item("Disc1"))) & " ," & FixKoma(NullToDbl(.Rows(i).Item("Disc2"))) & " ," & FixKoma(NullToDbl(.Rows(i).Item("Disc3"))) & " ," & FixKoma(NullToDbl(.Rows(i).Item("Jumlah"))) & " ,-1 ,'" & FixApostropi(NullTostr(.Rows(i).Item("Catatan"))) & "' ," & _
    '                            NullTolong(GetNewID("MDOD", "NoUrut", " WHERE MDOD.IDDO=" & NoID)) & " ," & FixKoma(NullToDbl(.Rows(i).Item("HargaPcs"))) & " ," & FixKoma(NullToDbl(.Rows(i).Item("QtyPcs"))) & " ," & FixKoma(NullToDbl(.Rows(i).Item("IDAsal"))) & " ," & FixKoma(NullToDbl(.Rows(i).Item("Konversi"))) & " ," & NullTolong(.Rows(i).Item("IDServerAsal")) & "," & NullTolong(.Rows(i).Item("NoID")) & ")"
    '                            EksekusiSQL(SQL)
    '                        Next
    '                    End If
    '                End With
    '                RefreshDetil()
    '            End If
    '        Else
    '            txtSPK.EditValue = IDSPKOld
    '        End If
    '    Catch ex As Exception
    '        XtraMessageBox.Show("Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
    '    Finally
    '        'RefreshLookUpSTB()
    '        Ds.Dispose()
    '    End Try
    'End Sub
    Private Sub AmbilDataJualRetur()
        Dim Ds As New DataSet
        Dim Harga As Double = 0.0
        Dim Disc1 As Double = 0.0
        Dim Disc2 As Double = 0.0
        Dim Jumlah As Double = 0.0
        Try
            If pTipe = pStatus.Edit AndAlso IsPosted = False Then
                'If IDSPKOld <> NullToLong(txtSPK.EditValue) AndAlso XtraMessageBox.Show("Bersihkan item Surat Jalan dan ambil data dari " & IIf(rbDari.SelectedIndex = 0, "Retur Pembelian", "Jual") & " ?", NamaAplikasi, MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                '    EksekusiSQL("DELETE FROM MDOD WHERE IDDO=" & NoID)
                'ElseIf IDSPKOld = NullToLong(txtSPK.EditValue) Then

                'Else
                '    txtSPK.EditValue = IDSPKOld
                '    Exit Sub
                'End If
                IDSPKOld = NullToLong(txtSPK.EditValue)
                If rbDari.SelectedIndex = 0 Then
                    SQL = "SELECT MReturBeliD.*, MBarang.CtnPcs, MSatuanBase.NoID AS IDSatuanBase, MSatuanBase.Konversi AS KonversiBase, (MReturBeliD.Qty*MReturBeliD.Konversi)-IsNull((SELECT SUM(MDOD.Qty*MDOD.Konversi) FROM MDOD INNER JOIN MDO ON MDO.NoID=MDOD.IDDO WHERE MDOD.IDJualD=MReturBeliD.NoID),0) AS SisaQty " & _
                          " FROM MReturBeliD " & _
                          " LEFT JOIN MBarang ON MBarang.NoID=MReturBeliD.IDBarang " & _
                          " LEFT JOIN (MSatuan LEFT JOIN MSatuan MSatuanBase ON MSatuanBase.NoID=MSatuan.IDBase) ON MSatuan.NoID=MReturBeliD.IDSatuan " & _
                          " WHERE MReturBeliD.IDGudang=" & NullToLong(txtGudang.EditValue) & " AND MReturBeliD.IDReturBeli=" & NullToLong(txtSPK.EditValue) & _
                          " AND (MReturBeliD.Qty*MReturBeliD.Konversi)-IsNull((SELECT SUM(MDOD.Qty*MDOD.Konversi) FROM MDOD INNER JOIN MDO ON MDO.NoID=MDOD.IDDO WHERE MDOD.IDJualD=MReturBeliD.NoID),0)>0"
                Else
                    SQL = "SELECT MJualD.*, MBarang.CtnPcs, MSatuanBase.NoID AS IDSatuanBase, MSatuanBase.Konversi AS KonversiBase, (MJualD.Qty*MJualD.Konversi)-IsNull((SELECT SUM(MDOD.Qty*MDOD.Konversi) FROM MDOD INNER JOIN MDO ON MDO.NoID=MDOD.IDDO WHERE MDOD.IDJualD=MJualD.NoID),0) AS SisaQty " & _
                          " FROM MJualD " & _
                          " LEFT JOIN MBarang ON MBarang.NoID=MJualD.IDBarang " & _
                          " LEFT JOIN (MSatuan LEFT JOIN MSatuan MSatuanBase ON MSatuanBase.NoID=MSatuan.IDBase) ON MSatuan.NoID=MJualD.IDSatuan " & _
                          " WHERE MJualD.IDGudang=" & NullToLong(txtGudang.EditValue) & " AND MJualD.IDJual=" & NullToLong(txtSPK.EditValue) & _
                          " AND (MJualD.Qty*MJualD.Konversi)-IsNull((SELECT SUM(MDOD.Qty*MDOD.Konversi) FROM MDOD INNER JOIN MDO ON MDO.NoID=MDOD.IDDO WHERE MDOD.IDJualD=MJualD.NoID),0)>0" & _
                          " ORDER BY MJualD.NoUrut "
                End If
                Ds = ExecuteDataset("MJualD", SQL)
                With Ds.Tables("MJualD")
                    For i As Integer = 0 To .Rows.Count - 1
                        'txtQty.EditValue / IsiKarton * txtKonversi.EditValue
                        If NullToDbl(.Rows(i).Item("Konversi")) <= NullToDbl(.Rows(i).Item("SisaQty")) Then
                            'Harga = clsPostingPenjualan.HargaJual(NullToLong(.Rows(i).Item("IDBarang")), NullToLong(.Rows(i).Item("IDSatuan")), , Disc1)
                            Jumlah = NullToDbl(.Rows(i).Item("Qty")) * (Bulatkan((Harga * (1 - (Disc1 / 100)) * (1 - (Disc2 / 100)) * (1 - (0 / 100))), 0) - 0 - 0 - 0)
                            SQL = "INSERT INTO [MDOD] ([NoID] ,[Tgl] ,[Jam] ,[IDDO] ,[IDBarang] ,[IDSatuan] ,[IDGudang] ,[Ctn] ,[Qty] ,[Harga] ,[DiscPersen1] ,[DiscPersen2] ,[DiscPersen3] ,[Disc1] ,[Disc2] ,[Disc3] ,[Jumlah],[Catatan] ,[NoUrut] ,[HargaPcs] ,[QtyPcs] ,[IDAsal] ,[Konversi] ,[IDJualD])" & _
                            " VALUES (" & NullToLong(GetNewID("MDOD", "NoID")) & " ,getdate() ,getdate() ," & NoID & " ," & NullToLong(.Rows(i).Item("IDBarang")) & " ," & NullToLong(.Rows(i).Item("IDSatuan")) & " ," & NullToLong(txtGudang.EditValue) & " ," & FixKoma(IIf(NullToDbl(.Rows(i).Item("CtnPcs")) = 0, 0, NullToDbl(.Rows(i).Item("SisaQty")) / NullToDbl(.Rows(i).Item("CtnPcs")))) & " ," & FixKoma(NullToDbl(.Rows(i).Item("SisaQty")) / IIf(NullToDbl(.Rows(i).Item("Konversi")) = 0, 1, NullToDbl(.Rows(i).Item("Konversi")))) & " ," & FixKoma(Harga) & " ," & FixKoma(Disc1) & _
                            " ," & FixKoma(Disc2) & " ," & FixKoma(0) & " ," & FixKoma(0) & " ," & FixKoma(0) & " ," & FixKoma(0) & " ," & FixKoma(Jumlah) & " ,'" & FixApostropi(NullToStr(.Rows(i).Item("Catatan"))) & "' ," & _
                            NullToLong(GetNewID("MDOD", "NoUrut", " WHERE MDOD.IDDO=" & NoID)) & " ," & FixKoma(Harga / IIf(NullToDbl(.Rows(i).Item("Konversi")) = 0, 1, NullToDbl(.Rows(i).Item("Konversi")))) & " ," & FixKoma(NullToDbl(.Rows(i).Item("SisaQty"))) & " ," & FixKoma(NullToDbl(.Rows(i).Item("IDAsal"))) & " ," & FixKoma(NullToDbl(.Rows(i).Item("Konversi"))) & " ," & NullToLong(.Rows(i).Item("NoID")) & ")"
                        Else
                            'Harga = clsPostingPenjualan.HargaJual(NullToLong(.Rows(i).Item("IDBarang")), NullToLong(.Rows(i).Item("IDSatuanBase")), NullToLong(txtKodeCustomer.EditValue), Disc1)
                            Jumlah = NullToDbl(.Rows(i).Item("Qty")) * (Bulatkan((Harga * (1 - (Disc1 / 100)) * (1 - (Disc2 / 100)) * (1 - (0 / 100))), 0) - 0 - 0 - 0)
                            SQL = "INSERT INTO [MDOD] ([NoID] ,[Tgl] ,[Jam] ,[IDDO] ,[IDBarang] ,[IDSatuan] ,[IDGudang] ,[Ctn] ,[Qty] ,[Harga] ,[DiscPersen1] ,[DiscPersen2] ,[DiscPersen3] ,[Disc1] ,[Disc2] ,[Disc3] ,[Jumlah] ,[Catatan] ,[NoUrut] ,[HargaPcs] ,[QtyPcs] ,[IDAsal] ,[Konversi] ,[IDJualD])" & _
                            " VALUES (" & NullToLong(GetNewID("MDOD", "NoID")) & " ,getdate() ,getdate() ," & NoID & " ," & NullToLong(.Rows(i).Item("IDBarang")) & " ," & NullToLong(.Rows(i).Item("IDSatuanBase")) & " ," & NullToLong(txtGudang.EditValue) & " ," & FixKoma(IIf(NullToDbl(.Rows(i).Item("CtnPcs")) = 0, 0, NullToDbl(.Rows(i).Item("SisaQty")) / NullToDbl(.Rows(i).Item("CtnPcs")))) & " ," & FixKoma(NullToDbl(.Rows(i).Item("SisaQty")) / IIf(NullToDbl(.Rows(i).Item("KonversiBase")) = 0, 1, NullToDbl(.Rows(i).Item("KonversiBase")))) & " ," & FixKoma(Harga) & " ," & FixKoma(Disc1) & _
                            " ," & FixKoma(Disc2) & " ," & FixKoma(0) & " ," & FixKoma(0) & " ," & FixKoma(0) & " ," & FixKoma(0) & " ," & FixKoma(Jumlah) & " ,'" & FixApostropi(NullToStr(.Rows(i).Item("Catatan"))) & "' ," & _
                            NullToLong(GetNewID("MDOD", "NoUrut", " WHERE MDOD.IDDO=" & NoID)) & " ," & FixKoma(Harga / IIf(NullToDbl(.Rows(i).Item("KonversiBase")) = 0, 1, NullToDbl(.Rows(i).Item("KonversiBase")))) & " ," & FixKoma(NullToDbl(.Rows(i).Item("SisaQty"))) & " ," & FixKoma(NullToDbl(.Rows(i).Item("IDAsal"))) & " ," & FixKoma(NullToDbl(.Rows(i).Item("KonversiBase"))) & " ," & NullToLong(.Rows(i).Item("NoID")) & ")"
                        End If
                        EksekusiSQL(SQL)
                    Next
                End With
                RefreshDetil()
            Else
                txtSPK.EditValue = IDSPKOld
            End If
        Catch ex As Exception
            XtraMessageBox.Show("Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            'RefreshLookUpSTB()
            Ds.Dispose()
        End Try
    End Sub
    Public Sub LoadDataJualRetur()
        Dim Ds As New DataSet
        Dim Harga As Double = 0.0
        Dim Disc1 As Double = 0.0
        Dim Disc2 As Double = 0.0
        Dim Jumlah As Double = 0.0
        Try
            If pTipe = pStatus.Edit AndAlso IsPosted = False Then
                'If IDSPKOld <> NullToLong(txtSPK.EditValue) AndAlso XtraMessageBox.Show("Bersihkan item Surat Jalan dan ambil data dari " & IIf(rbDari.SelectedIndex = 0, "Retur Pembelian", "Jual") & " ?", NamaAplikasi, MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                '    EksekusiSQL("DELETE FROM MDOD WHERE IDDO=" & NoID)
                'ElseIf IDSPKOld = NullToLong(txtSPK.EditValue) Then

                'Else
                '    txtSPK.EditValue = IDSPKOld
                '    Exit Sub
                'End If
                'IDSPKOld = NullToLong(txtSPK.EditValue)
                If rbDari.SelectedIndex = 0 Then
                    SQL = "SELECT MReturBeliD.*, MBarang.CtnPcs, MSatuanBase.NoID AS IDSatuanBase, MSatuanBase.Konversi AS KonversiBase, (MReturBeliD.Qty*MReturBeliD.Konversi)-IsNull((SELECT SUM(MDOD.Qty*MDOD.Konversi) FROM MDOD INNER JOIN MDO ON MDO.NoID=MDOD.IDDO WHERE MDOD.IDJualD=MReturBeliD.NoID),0) AS SisaQty " & _
                          " FROM MReturBeliD " & _
                          " LEFT JOIN MBarang ON MBarang.NoID=MReturBeliD.IDBarang " & _
                          " LEFT JOIN (MSatuan LEFT JOIN MSatuan MSatuanBase ON MSatuanBase.NoID=MSatuan.IDBase) ON MSatuan.NoID=MReturBeliD.IDSatuan " & _
                          " WHERE MReturBeliD.IDGudang=" & NullToLong(txtGudang.EditValue) & " AND MReturBeliD.IDReturBeli IN (SELECT MReturBeliD.IDReturBeli FROM MDOD LEFT JOIN (MReturBeliD INNER JOIN MReturBeli ON MReturBeli.NoID=MReturBeliD.IDReturBeli) ON MDOD.IDJualD=MReturBeliD.NoID WHERE MDOD.IDDO=" & NoID & ") " & _
                          " AND (MReturBeliD.Qty*MReturBeliD.Konversi)-IsNull((SELECT SUM(MDOD.Qty*MDOD.Konversi) FROM MDOD INNER JOIN MDO ON MDO.NoID=MDOD.IDDO WHERE MDOD.IDJualD=MReturBeliD.NoID),0)>0"
                Else
                    SQL = "SELECT MJualD.*, MBarang.CtnPcs, MSatuanBase.NoID AS IDSatuanBase, MSatuanBase.Konversi AS KonversiBase, (MJualD.Qty*MJualD.Konversi)-IsNull((SELECT SUM(MDOD.Qty*MDOD.Konversi) FROM MDOD INNER JOIN MDO ON MDO.NoID=MDOD.IDDO WHERE MDOD.IDJualD=MJualD.NoID),0) AS SisaQty " & _
                          " FROM MJualD " & _
                          " LEFT JOIN MBarang ON MBarang.NoID=MJualD.IDBarang " & _
                          " LEFT JOIN (MSatuan LEFT JOIN MSatuan MSatuanBase ON MSatuanBase.NoID=MSatuan.IDBase) ON MSatuan.NoID=MJualD.IDSatuan " & _
                          " WHERE MJualD.IDGudang=" & NullToLong(txtGudang.EditValue) & " AND MJualD.IDJual IN (SELECT MJualD.IDJual FROM MDOD LEFT JOIN (MJualD INNER JOIN MJual ON MJual.NoID=MJualD.IDJual) ON MDOD.IDJualD=MJualD.NoID WHERE MDOD.IDDO=" & NoID & ") " & _
                          " AND (MJualD.Qty*MJualD.Konversi)-IsNull((SELECT SUM(MDOD.Qty*MDOD.Konversi) FROM MDOD INNER JOIN MDO ON MDO.NoID=MDOD.IDDO WHERE MDOD.IDJualD=MJualD.NoID),0)>0" & _
                          " ORDER BY MJualD.NoUrut "
                End If
                Ds = ExecuteDataset("MJualD", SQL)
                With Ds.Tables("MJualD")
                    For i As Integer = 0 To .Rows.Count - 1
                        'txtQty.EditValue / IsiKarton * txtKonversi.EditValue
                        If NullToDbl(.Rows(i).Item("Konversi")) <= NullToDbl(.Rows(i).Item("SisaQty")) Then
                            'Harga = clsPostingPenjualan.HargaJual(NullToLong(.Rows(i).Item("IDBarang")), NullToLong(.Rows(i).Item("IDSatuan")), NullToLong(txtKodeCustomer.EditValue), Disc1, Disc2)
                            Jumlah = NullToDbl(.Rows(i).Item("Qty")) * (Bulatkan((Harga * (1 - (Disc1 / 100)) * (1 - (Disc2 / 100)) * (1 - (0 / 100))), 0) - 0 - 0 - 0)
                            SQL = "INSERT INTO [MDOD] ([NoID] ,[Tgl] ,[Jam] ,[IDDO] ,[IDBarang] ,[IDSatuan] ,[IDGudang] ,[Ctn] ,[Qty] ,[Harga] ,[DiscPersen1] ,[DiscPersen2] ,[DiscPersen3] ,[Disc1] ,[Disc2] ,[Disc3] ,[Jumlah],[Catatan] ,[NoUrut] ,[HargaPcs] ,[QtyPcs] ,[IDAsal] ,[Konversi] ,[IDJualD])" & _
                            " VALUES (" & NullToLong(GetNewID("MDOD", "NoID")) & " ,getdate() ,getdate() ," & NoID & " ," & NullToLong(.Rows(i).Item("IDBarang")) & " ," & NullToLong(.Rows(i).Item("IDSatuan")) & " ," & NullToLong(txtGudang.EditValue) & " ," & FixKoma(IIf(NullToDbl(.Rows(i).Item("CtnPcs")) = 0, 0, NullToDbl(.Rows(i).Item("SisaQty")) / NullToDbl(.Rows(i).Item("CtnPcs")))) & " ," & FixKoma(NullToDbl(.Rows(i).Item("SisaQty")) / IIf(NullToDbl(.Rows(i).Item("Konversi")) = 0, 1, NullToDbl(.Rows(i).Item("Konversi")))) & " ," & FixKoma(Harga) & " ," & FixKoma(Disc1) & _
                            " ," & FixKoma(Disc2) & " ," & FixKoma(0) & " ," & FixKoma(0) & " ," & FixKoma(0) & " ," & FixKoma(0) & " ," & FixKoma(Jumlah) & " ,'" & FixApostropi(NullToStr(.Rows(i).Item("Catatan"))) & "' ," & _
                            NullToLong(GetNewID("MDOD", "NoUrut", " WHERE MDOD.IDDO=" & NoID)) & " ," & FixKoma(Harga / IIf(NullToDbl(.Rows(i).Item("Konversi")) = 0, 1, NullToDbl(.Rows(i).Item("Konversi")))) & " ," & FixKoma(NullToDbl(.Rows(i).Item("SisaQty"))) & " ," & FixKoma(NullToDbl(.Rows(i).Item("IDAsal"))) & " ," & FixKoma(NullToDbl(.Rows(i).Item("Konversi"))) & " ," & NullToLong(.Rows(i).Item("NoID")) & ")"
                        Else
                            'Harga = clsPostingPenjualan.HargaJual(NullToLong(.Rows(i).Item("IDBarang")), NullToLong(.Rows(i).Item("IDSatuanBase")), NullToLong(txtKodeCustomer.EditValue), Disc1, Disc2)
                            Jumlah = NullToDbl(.Rows(i).Item("Qty")) * (Bulatkan((Harga * (1 - (Disc1 / 100)) * (1 - (Disc2 / 100)) * (1 - (0 / 100))), 0) - 0 - 0 - 0)
                            SQL = "INSERT INTO [MDOD] ([NoID] ,[Tgl] ,[Jam] ,[IDDO] ,[IDBarang] ,[IDSatuan] ,[IDGudang] ,[Ctn] ,[Qty] ,[Harga] ,[DiscPersen1] ,[DiscPersen2] ,[DiscPersen3] ,[Disc1] ,[Disc2] ,[Disc3] ,[Jumlah] ,[Catatan] ,[NoUrut] ,[HargaPcs] ,[QtyPcs] ,[IDAsal] ,[Konversi] ,[IDJualD])" & _
                            " VALUES (" & NullToLong(GetNewID("MDOD", "NoID")) & " ,getdate() ,getdate() ," & NoID & " ," & NullToLong(.Rows(i).Item("IDBarang")) & " ," & NullToLong(.Rows(i).Item("IDSatuanBase")) & " ," & NullToLong(txtGudang.EditValue) & " ," & FixKoma(IIf(NullToDbl(.Rows(i).Item("CtnPcs")) = 0, 0, NullToDbl(.Rows(i).Item("SisaQty")) / NullToDbl(.Rows(i).Item("CtnPcs")))) & " ," & FixKoma(NullToDbl(.Rows(i).Item("SisaQty")) / IIf(NullToDbl(.Rows(i).Item("KonversiBase")) = 0, 1, NullToDbl(.Rows(i).Item("KonversiBase")))) & " ," & FixKoma(Harga) & " ," & FixKoma(Disc1) & _
                            " ," & FixKoma(Disc2) & " ," & FixKoma(0) & " ," & FixKoma(0) & " ," & FixKoma(0) & " ," & FixKoma(0) & " ," & FixKoma(Jumlah) & " ,'" & FixApostropi(NullToStr(.Rows(i).Item("Catatan"))) & "' ," & _
                            NullToLong(GetNewID("MDOD", "NoUrut", " WHERE MDOD.IDDO=" & NoID)) & " ," & FixKoma(Harga / IIf(NullToDbl(.Rows(i).Item("KonversiBase")) = 0, 1, NullToDbl(.Rows(i).Item("KonversiBase")))) & " ," & FixKoma(NullToDbl(.Rows(i).Item("SisaQty"))) & " ," & FixKoma(NullToDbl(.Rows(i).Item("IDAsal"))) & " ," & FixKoma(NullToDbl(.Rows(i).Item("KonversiBase"))) & " ," & NullToLong(.Rows(i).Item("NoID")) & ")"
                        End If
                        EksekusiSQL(SQL)
                    Next
                End With
                RefreshDetil()
            Else
                txtSPK.EditValue = IDSPKOld
            End If
        Catch ex As Exception
            XtraMessageBox.Show("Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            'RefreshLookUpSTB()
            Ds.Dispose()
        End Try
    End Sub
    Private Sub gvGudang_DataSourceChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvGudang.DataSourceChanged
        If System.IO.File.Exists(FolderLayouts & Me.Name & gvGudang.Name & ".xml") Then
            gvGudang.RestoreLayoutFromXml(FolderLayouts & Me.Name & gvGudang.Name & ".xml")
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

    Private Sub gvPenerima_DataSourceChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvPenerima.DataSourceChanged
        If System.IO.File.Exists(FolderLayouts & Me.Name & gvPenerima.Name & ".xml") Then
            gvPenerima.RestoreLayoutFromXml(FolderLayouts & Me.Name & gvPenerima.Name & ".xml")
        End If
        With gvPenerima
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

    Private Sub GC1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles GC1.Click

    End Sub

    Private Sub txtGudang_EditValueChanging(ByVal sender As Object, ByVal e As DevExpress.XtraEditors.Controls.ChangingEventArgs) Handles txtGudang.EditValueChanging
        If Not IsLoad AndAlso (IsTempInsertBaru Or (pTipe = pStatus.Edit AndAlso Not IsPosted)) Then
            If XtraMessageBox.Show("Bila gudang dirubah item detil akan dibersihkan?" & vbCrLf & "Ingin melanjutkan?", NamaAplikasi, MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) = Windows.Forms.DialogResult.Yes Then
                EksekusiSQL("DELETE FROM MDOD WHERE IDDO=" & NoID)
                RefreshDetil()
            Else
                e.Cancel = True
            End If
        End If
    End Sub
End Class