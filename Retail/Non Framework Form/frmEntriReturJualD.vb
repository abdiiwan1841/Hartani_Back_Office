Imports System.Data
Imports System.Data.SqlClient
Imports DevExpress.XtraEditors
Imports DevExpress.Utils
Imports System.IO.File

Public Class frmEntriReturJualD
    Public IsNew As Boolean = True
    Public IsPosted As Boolean = False
    Public NoID As Long = -1
    Public IDReturJual As Long = -1
    Public FormPemanggil As frmEntriReturJual
    Public IDCustomer As Long = -1
    Public IDGudang As Long = -1

    Dim QtySisa As Double = 0.0
    Dim oDS As New DataSet
    Dim SQL As String = ""
    Dim DefImageList As ImageList
    Dim HargaPcs As Double = 0.0
    Dim Konversi As Double = 0.0
    Dim KonversiJual As Double = 0.0
    Public IsFastEntri As Boolean = False

    Private Sub IsiDefault()
        DefImageList = frmMain.ImageList1
        cmdSave.ImageList = DefImageList
        cmdSave.ImageIndex = 6

        cmdTutup.ImageList = DefImageList
        cmdTutup.ImageIndex = 3
        RefreshLookUp()
        Konversi = 1.0
        HargaPcs = 0.0
        QtySisa = 0.0
        txtGudang.EditValue = IDGudang
        RubahGudang()
    End Sub
    Private Function DiRetur() As String
        Return "isnull((SELECT SUM(isnull(A.Qty,0)*isnull(A.Konversi,0)) FROM MReturJualD A WHERE A.NoID <> " & NoID & " AND A.IDJualD=MJualD.NoID),0)"
    End Function
    Private Sub RefreshLookUp()
        Dim ds As New DataSet
        Try
            SQL = "SELECT MJualD.NoID, MWilayah.Nama AS Wilayah, MGudang.Nama AS Gudang, MJual.Kode AS KodeJual, MBarang.Kode AS KodeStock, MJualD.Qty AS QtyJual, MSatuan.Nama AS SatuanJual, "
            SQL &= " (MJualD.Jumlah/isnull(MJualD.Konversi,1)/isnull(MJualD.Qty,1))-(IsNull(MJual.DiskonNotaProsen,0)/100*(MJualD.Jumlah/isnull(MJualD.Konversi,1)/isnull(MJualD.Qty,1)))-IsNull((SELECT SUM(MRevisiHargaJualD.Koreksi*MRevisiHargaJualD.Konversi) FROM MRevisiHargaJualD LEFT JOIN MRevisiHargaJual ON MRevisiHargaJual.NoID=MRevisiHargaJualD.IDRevisiHargaJual WHERE MRevisiHargaJualD.IDJual=MJual.NoID AND MRevisiHargaJualD.IDJualD=MJualD.NoID AND MRevisiHargaJual.IsPosted=1),0) AS [Harga (Pcs)],"
            SQL &= " (MJualD.Qty*MJualD.Konversi)-isnull(" & DiRetur() & ",0) AS [Qty Sisa (Pcs)] "
            SQL &= " FROM MJualD "
            SQL &= " LEFT JOIN (MGudang LEFT JOIN MWilayah ON MWilayah.NoID=MGudang.IDWilayah) ON MGudang.NoID=MJualD.IDGudang "
            SQL &= " LEFT JOIN MJual ON MJual.NoID=MJualD.IDJual LEFT JOIN MBarang ON MBarang.NoID=MJualD.IDBarang LEFT JOIN MSatuan ON MSatuan.NoID=MJualD.IDSatuan "
            SQL &= " WHERE MWilayah.NoID=" & DefIDWilayah & " AND (MJualD.Qty*MJualD.Konversi)-isnull(" & DiRetur() & ",0)>0 AND MBarang.IsActive=1 AND MJual.IsPosted=1 AND MJual.IDCustomer=" & IDCustomer
            ds = ExecuteDataset("MJualD", SQL)
            txtJual.Properties.DataSource = ds.Tables("MJualD")
            txtJual.Properties.ValueMember = "NoID"
            txtJual.Properties.DisplayMember = "KodeJual"

            SQL = "SELECT MBarang.NoID, MBarang.Kode, MBarang.Nama, MSatuan.Nama AS Satuan FROM MBarang LEFT JOIN MSatuan ON MSatuan.NoID=MBarang.IDSatuan WHERE MBarang.IsActive=1 AND NOT (MBarang.KODE='' OR MBarang.KODE IS NULL)"
            ds = ExecuteDataset("MBarang", SQL)
            txtBarang.Properties.DataSource = ds.Tables("MBarang")
            txtBarang.Properties.ValueMember = "NoID"
            txtBarang.Properties.DisplayMember = "Kode"
            
            QtyGudang()
        Catch ex As Exception
            XtraMessageBox.Show("Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            ds.Dispose()
        End Try
    End Sub
    Private Sub RefreshLookUpJual()
        Dim ds As New DataSet
        Try
            SQL = "SELECT MJualD.NoID, MWilayah.Nama AS Wilayah, MGudang.Nama AS Gudang, MJual.Kode AS KodeJual, MBarang.Kode AS KodeStock, MJualD.Qty AS QtyJual, MSatuan.Nama AS SatuanJual, " & _
                  " (MJualD.Jumlah/isnull(MJualD.Konversi,1)/isnull(MJualD.Qty,1))-(IsNull(MJual.DiskonNotaProsen,0)/100*(MJualD.Jumlah/isnull(MJualD.Konversi,1)/isnull(MJualD.Qty,1))) AS [Harga (Pcs)], IsNull((SELECT TOP 1 MRevisiHargaJualD.KoreksiJL*MRevisiHargaJualD.Konversi FROM MRevisiHargaJualD INNER JOIN MRevisiHargaJual ON MRevisiHargaJual.NoID=MRevisiHargaJualD.IDRevisiHargaJual WHERE MRevisiHargaJualD.IDJual=MJual.NoID AND MRevisiHargaJualD.IDJualD=MJualD.NoID AND MRevisiHargaJual.IsPosted=1 ORDER BY MRevisiHargaJual.Tanggal DESC),0) AS [TotalPH], " & _
                  " (MJualD.Qty*MJualD.Konversi)-isnull(" & DiRetur() & ",0) AS [Qty Sisa (Pcs)] " & _
                  " FROM MJualD " & _
                  " LEFT JOIN (MGudang LEFT JOIN MWilayah ON MWilayah.NoID=MGudang.IDWilayah) ON MGudang.NoID=MJualD.IDGudang " & _
                  " LEFT JOIN MJual ON MJual.NoID=MJualD.IDJual LEFT JOIN MBarang ON MBarang.NoID=MJualD.IDBarang LEFT JOIN MSatuan ON MSatuan.NoID=MJualD.IDSatuan " & _
                  " WHERE MWilayah.NoID=" & DefIDWilayah & " AND (MJualD.Qty*MJualD.Konversi)-isnull(" & DiRetur() & ",0)>0 AND MBarang.NoID=" & NullToLong(txtBarang.EditValue) & " AND MBarang.IsActive=1 AND MJual.IsPosted=1 AND MJual.IDCustomer=" & IDCustomer
            ds = ExecuteDataset("MJualD", SQL)
            txtJual.Properties.DataSource = ds.Tables("MJualD")
            txtJual.Properties.ValueMember = "NoID"
            txtJual.Properties.DisplayMember = "KodeJual"

        Catch ex As Exception
            XtraMessageBox.Show("Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            ds.Dispose()
        End Try
    End Sub
    Private Sub RefreshLookUpSatuan()
        Dim ds As New DataSet
        Try
            SQL = "SELECT MSatuan.NoID, MSatuan.Kode, MSatuan.Nama FROM MSatuan WHERE MSatuan.IsActive=1 AND MSatuan.NoID IN (SELECT MBarangD.IDSatuan FROM MBarangD WHERE MBarangD.IsJual=1 AND MBarangD.IsActive=1 AND MBarangD.IDBarang=" & NullTolong(txtBarang.EditValue) & ")"
            ds = ExecuteDataset("MSatuan", SQL)
            txtSatuan.Properties.DataSource = ds.Tables("MSatuan")
            txtSatuan.Properties.ValueMember = "NoID"
            txtSatuan.Properties.DisplayMember = "Kode"
        Catch ex As Exception
            XtraMessageBox.Show("Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            ds.Dispose()
        End Try
    End Sub
    Private Sub QtyGudang()
        Dim ds As New DataSet
        Try
            SQL = "SELECT MGudang.NoID, MGudang.Kode, MGudang.Kode + ' - ' + MGudang.Nama AS Nama, MWilayah.Nama AS Wilayah FROM MGudang LEFT JOIN MWilayah ON MGudang.IDWilayah=MWilayah.NoID WHERE MGudang.IsActive=1 AND MGudang.InTransit=0 AND MGudang.IDWilayah=" & DefIDWilayah
            If NullTobool(EksekusiSQlSkalarNew("SELECT IsGudangBSDiRetur FROM MSetting")) Then
                SQL &= " AND MGudang.IsBS=1 "
            End If
            ds = ExecuteDataset("MGudang", SQL)
            txtGudang.Properties.DataSource = ds.Tables("MGudang")
            txtGudang.Properties.ValueMember = "NoID"
            txtGudang.Properties.DisplayMember = "Nama"
            
        Catch ex As Exception
            XtraMessageBox.Show("Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            ds.Dispose()
        End Try
    End Sub
    Private Sub LoadData()
        Try
            SQL = "SELECT MReturJualD.*, MJualD.Konversi AS KonversiAsal, MJualD.HargaPcs AS HargaAsal, (MJualD.Qty*MJualD.Konversi) AS QtyAsal, MBarang.Kode AS KodeBarang, MBarang.Nama AS NamaBarang, MGudang.Kode AS KodeGudang, MGudang.Nama AS NamaGudang, MSatuan.Nama AS NamaSatuan "
            SQL &= " FROM (((MReturJualD LEFT JOIN MGudang ON MGudang.NoID=MReturJualD.IDGudang) "
            SQL &= " LEFT JOIN MBarang ON MBarang.NoID=MReturJualD.IDBarang)"
            SQL &= " LEFT JOIN MSatuan ON MSatuan.NoID=MReturJualD.IDSatuan) "
            SQL &= " LEFT JOIN (MJualD LEFT JOIN MJual ON MJual.NoID=MJualD.IDJual) ON MJualD.NoID=MReturJualD.IDJualD "
            SQL &= " WHERE MReturJualD.NoID= " & NoID
            oDS = New DataSet()
            oDS = ExecuteDataset("MReturJualD", SQL)
            If oDS.Tables(0).Rows.Count >= 1 Then
                With oDS.Tables("MReturJualD").Rows(0)
                    NoID = NullTolong(.Item("NoID"))
                    IDReturJual = NullTolong(.Item("IDReturJual"))
                    txtJual.EditValue = NullTolong(.Item("IDJualD"))
                    'txtJual.Text = NullTostr(.Item("KodeBarang"))
                    txtBarang.EditValue = NullTolong(.Item("IDBarang"))
                    'txtBarang.Text = NullTostr(.Item("KodeBarang"))
                    txtSatuan.EditValue = NullTolong(.Item("IDSatuan"))
                    'txtSatuan.Text = NullTostr(.Item("KodeSatuan"))
                    txtGudang.EditValue = IDGudang
                    'txtGudang.Text = NullTostr(.Item("KodeGudang"))
                    txtQty.EditValue = NullToDbl(.Item("Qty"))
                    HargaPcs = NullToDbl(.Item("HargaAsal"))
                    KonversiJual = NullToDbl(.Item("KonversiAsal"))
                    QtySisa = NullToDbl(EksekusiSQlSkalarNew("Select ((MJualD.Qty*MJualD.Konversi)-" & DiRetur() & ") AS QtySisa FROM MJualD WHERE NoID=" & NullTolong(.Item("IDJualD"))))
                    txtHarga.EditValue = NullToDbl(.Item("Harga"))
                    txtCtn.EditValue = NullToDbl(.Item("CTN"))
                    txtDiscPersen1.EditValue = NullToDbl(.Item("DiscPersen1"))
                    txtDiscPersen2.EditValue = NullToDbl(.Item("DiscPersen2"))
                    txtDiscPersen3.EditValue = NullToDbl(.Item("DiscPersen3"))
                    txtDiscRp1.EditValue = NullToDbl(.Item("Disc1"))
                    txtDiscRp2.EditValue = NullToDbl(.Item("Disc2"))
                    txtDiscRp3.EditValue = NullToDbl(.Item("Disc3"))
                    txtJUmlah.EditValue = NullToDbl(.Item("Jumlah"))
                    txtCatatan.Text = NullTostr(.Item("Catatan"))
                    txtKonversi.EditValue = NullToDbl(.Item("Konversi"))
                    Konversi = txtKonversi.EditValue
                End With
            Else
                IsiDefault()
            End If
        Catch ex As Exception
            XtraMessageBox.Show("Kesalahan: " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            oDS.Dispose()
        End Try
    End Sub

    Private Sub cmdSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSave.Click
        If IsValidasi() Then
            HitungJumlah()
            If Simpan() Then
                DialogResult = Windows.Forms.DialogResult.OK
                Me.Close()
            End If
        End If
    End Sub
    Private Function Simpan() As Boolean
        Try
            If IsNew Then
                SQL = "INSERT INTO MReturJualD (NoID,IDReturJual,NoUrut,Tgl,Jam,IDJualD,IDBarang,IDSatuan,Qty,QtyPcs,Harga,HargaPcs,CTN,DiscPersen1,DiscPersen2,DiscPersen3,Disc1,Disc2,Disc3,Jumlah,Catatan,IDGudang,Konversi) VALUES ("
                SQL &= NullTolong(GetNewID("MReturJualD", "NoID")) & ","
                SQL &= IDReturJual & ","
                SQL &= GetNewID("MReturJualD", "NoUrut", " WHERE IDReturJual=" & IDReturJual) & ","
                SQL &= "GetDate(),"
                SQL &= "GetDate(),"
                SQL &= NullTolong(txtJual.EditValue) & ","
                SQL &= NullTolong(txtBarang.EditValue) & ","
                SQL &= NullTolong(txtSatuan.EditValue) & ","
                SQL &= FixKoma(txtQty.EditValue) & ","
                SQL &= FixKoma(txtQty.EditValue * txtKonversi.EditValue) & ","
                SQL &= FixKoma(txtHarga.EditValue) & ","
                SQL &= FixKoma(txtJUmlah.EditValue / IIf(txtQty.EditValue = 0, 1, txtQty.EditValue) / IIf(txtKonversi.EditValue = 0, 1, txtKonversi.EditValue)) & ","
                SQL &= FixKoma(txtCtn.EditValue) & ","
                SQL &= FixKoma(txtDiscPersen1.EditValue) & ","
                SQL &= FixKoma(txtDiscPersen2.EditValue) & ","
                SQL &= FixKoma(txtDiscPersen3.EditValue) & ","
                SQL &= FixKoma(txtDiscRp1.EditValue) & ","
                SQL &= FixKoma(txtDiscRp2.EditValue) & ","
                SQL &= FixKoma(txtDiscRp3.EditValue) & ","
                SQL &= FixKoma(txtJUmlah.EditValue) & ","
                SQL &= "'" & FixApostropi(txtCatatan.Text) & "',"
                SQL &= NullTolong(txtGudang.EditValue) & ","
                SQL &= FixKoma(txtKonversi.EditValue) & ""
                SQL &= ")"
            Else
                SQL = "UPDATE MReturJualD SET "
                SQL &= " Tgl=GetDate(),"
                SQL &= " Jam=GetDate(),"
                SQL &= " IDReturJual=" & IDReturJual & ","
                SQL &= " IDJualD=" & NullTolong(txtJual.EditValue) & ","
                SQL &= " IDBarang=" & NullTolong(txtBarang.EditValue) & ","
                SQL &= " IDSatuan=" & NullTolong(txtSatuan.EditValue) & ","
                SQL &= " Qty=" & FixKoma(txtQty.EditValue) & ","
                SQL &= " QtyPcs=" & FixKoma(txtQty.EditValue * txtKonversi.EditValue) & ","
                SQL &= " Harga=" & FixKoma(txtHarga.EditValue) & ","
                SQL &= " HargaPcs=" & FixKoma(txtJUmlah.EditValue / IIf(txtQty.EditValue = 0, 1, txtQty.EditValue) / IIf(txtKonversi.EditValue = 0, 1, txtKonversi.EditValue)) & ","
                SQL &= " CTN=" & FixKoma(txtCtn.EditValue) & ","
                SQL &= " DiscPersen1=" & FixKoma(txtDiscPersen1.EditValue) & ","
                SQL &= " DiscPersen2=" & FixKoma(txtDiscPersen2.EditValue) & ","
                SQL &= " DiscPersen3=" & FixKoma(txtDiscPersen3.EditValue) & ","
                SQL &= " Disc1=" & FixKoma(txtDiscRp1.EditValue) & ","
                SQL &= " Disc2=" & FixKoma(txtDiscRp2.EditValue) & ","
                SQL &= " Disc3=" & FixKoma(txtDiscRp3.EditValue) & ","
                SQL &= " Jumlah=" & FixKoma(txtJUmlah.EditValue) & ","
                SQL &= " Catatan='" & FixApostropi(txtCatatan.Text) & "',"
                SQL &= " IDGudang=" & NullTolong(txtGudang.EditValue) & ","
                SQL &= " Konversi=" & FixKoma(txtKonversi.EditValue) & ""
                SQL &= " WHERE NoID=" & NoID
            End If
            If EksekusiSQL(SQL) >= 1 Then
                Return True
            Else
                Return False
            End If
        Catch ex As Exception
            XtraMessageBox.Show("Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return False
        End Try
    End Function
    Private Function IsValidasi() As Boolean
        If txtJual.Text = "" Then
            If XtraMessageBox.Show("Yakin melakukan retur tanpa adanya penjualan ?", NamaAplikasi, MessageBoxButtons.YesNo, MessageBoxIcon.Warning) = Windows.Forms.DialogResult.No Then
                txtJual.Focus()
                Return False
                Exit Function
            End If
        End If
        If txtJual.Text <> "" Then
            QtySisa = NullToDbl(EksekusiSQlSkalarNew("Select ((MJualD.Qty*MJualD.Konversi)-" & DiRetur() & ") AS QtySisa FROM MJualD WHERE NoID=" & NullTolong(txtJual.EditValue)))
            If QtySisa < (txtQty.EditValue * txtKonversi.EditValue) Then
                XtraMessageBox.Show("Qty melebihi standart penjualan.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
                txtQty.Focus()
                Return False
                Exit Function
            End If
        End If
        If txtBarang.Text = "" Then
            XtraMessageBox.Show("Barang masih kosong.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
            txtBarang.Focus()
            Return False
            Exit Function
        End If
        If txtSatuan.Text = "" Then
            XtraMessageBox.Show("Satuan masih kosong.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
            txtSatuan.Focus()
            Return False
            Exit Function
        End If
        If txtGudang.Text = "" Then
            XtraMessageBox.Show("Gudang masih kosong.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
            txtGudang.Focus()
            Return False
            Exit Function
        End If
        'If SisaQty() < txtQty.EditValue * txtKonversi.EditValue Then
        '    XtraMessageBox.Show("Qty melebihi stok Gudang " & txtGudang.Text & " .", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
        '    txtQty.Focus()
        '    Return False
        '    Exit Function
        'End If
        If txtQty.EditValue <= 0 Then
            XtraMessageBox.Show("Qty masih kurang dari atau sama dengan 0.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
            txtQty.Focus()
            Return False
            Exit Function
        End If
        If txtCtn.EditValue < 0 Then
            XtraMessageBox.Show("Ctn tidak boleh kurang dari 0.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
            txtCtn.Focus()
            Return False
            Exit Function
        End If
        If txtKonversi.EditValue < 0 Then
            XtraMessageBox.Show("Konversi tidak boleh kurang dari 0.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
            txtKonversi.Focus()
            Return False
            Exit Function
        End If
        If txtDiscPersen1.EditValue < 0 Then
            XtraMessageBox.Show("Disc persen 1 tidak boleh kurang dari 0.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
            txtDiscPersen1.Focus()
            Return False
            Exit Function
        End If
        If txtDiscPersen2.EditValue < 0 Then
            XtraMessageBox.Show("Disc persen 1 tidak boleh kurang dari 0.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
            txtDiscPersen2.Focus()
            Return False
            Exit Function
        End If
        If txtDiscPersen3.EditValue < 0 Then
            XtraMessageBox.Show("Disc persen 1 tidak boleh kurang dari 0.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
            txtDiscPersen2.Focus()
            Return False
            Exit Function
        End If
        If txtDiscRp1.EditValue < 0 Then
            XtraMessageBox.Show("Disc rupiah 1 tidak boleh kurang dari 0.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
            txtDiscRp1.Focus()
            Return False
            Exit Function
        End If
        If txtDiscRp2.EditValue < 0 Then
            XtraMessageBox.Show("Disc rupiah 2 tidak boleh kurang dari 0.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
            txtDiscRp2.Focus()
            Return False
            Exit Function
        End If
        If txtDiscRp3.EditValue < 0 Then
            XtraMessageBox.Show("Disc rupiah 3 tidak boleh kurang dari 0.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
            txtDiscRp3.Focus()
            Return False
            Exit Function
        End If
        If txtHarga.EditValue <= 0 Then
            If txtHarga.EditValue = 0 AndAlso XtraMessageBox.Show("Harga masih kurang dari atau sama dengan 0, lanjutkan penyimpanan?", NamaAplikasi, MessageBoxButtons.YesNo, MessageBoxIcon.Information, MessageBoxDefaultButton.Button2) = Windows.Forms.DialogResult.No Then
                txtHarga.Focus()
                Return False
                Exit Function
            ElseIf txtHarga.EditValue < 0 Then
                txtHarga.Focus()
                Return False
                Exit Function
            End If
        End If
        Return True
    End Function
    Private Function SisaQty() As Double
        Dim ds As New DataSet
        Try
            SQL = "SELECT SUM((isnull(QtyMasuk,0)*isnull(Konversi,0))-(isnull(QtyKeluar,0)*isnull(Konversi,0))) AS QtySisa FROM MKartuStok WHERE IDBarang=" & NullTolong(txtBarang.EditValue) & " AND IDGudang=" & NullTolong(txtGudang.EditValue) & " AND (IsSPK=0 OR IsSPK Is NULL)"
            ds = ExecuteDataset("Qty", SQL)
            If ds.Tables(0).Rows.Count >= 1 Then
                Return NullToDbl(ds.Tables(0).Rows(0).Item("QtySisa"))
            Else
                Return 0
            End If
        Catch ex As Exception
            XtraMessageBox.Show("Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return 0
        Finally
            oDS.Dispose()
        End Try
    End Function

    Private Sub cmdTutup_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdTutup.Click
        DialogResult = Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub BarButtonItem1_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles cmdSaveLayout.ItemClick
        Dim xOtorisasi As New frmOtorisasiAdmin
        Try
            If XtraMessageBox.Show("Simpan Layout?", NamaAplikasi, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = Windows.Forms.DialogResult.Yes AndAlso xOtorisasi.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
                Ini.TulisIniPath(folderLayouts & Me.Name & ".ini", "Form", "Width", Me.Width)
                Ini.TulisIniPath(folderLayouts & Me.Name & ".ini", "Form", "Height", Me.Height)
                Me.Left = (Screen.PrimaryScreen.WorkingArea.Width - Me.Width) \ 2
                Me.Top = (Screen.PrimaryScreen.WorkingArea.Height - Me.Height) \ 2

                LayoutControl1.SaveLayoutToXml(folderLayouts & Me.Name & LayoutControl1.Name & ".xml")
                gvBarang.SaveLayoutToXml(folderLayouts & Me.Name & gvBarang.Name & ".xml")
                gvGudang.SaveLayoutToXml(folderLayouts & Me.Name & gvGudang.Name & ".xml")
                gvSatuan.SaveLayoutToXml(folderLayouts & Me.Name & gvSatuan.Name & ".xml")
                gvJual.SaveLayoutToXml(folderLayouts & Me.Name & gvJual.Name & ".xml")
            End If
        Catch ex As Exception
            XtraMessageBox.Show("Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            xOtorisasi.Dispose()
        End Try
    End Sub
    Private Sub LoadLayout()
        'If Exists(folderLayouts &  Me.Name & gvBarang.Name & ".xml") Then
        '    gvBarang.RestoreLayoutFromXml(folderLayouts &  Me.Name & gvBarang.Name & ".xml")
        'End If
        'If Exists(folderLayouts &  Me.Name & gvGudang.Name & ".xml") Then
        '    gvGudang.RestoreLayoutFromXml(folderLayouts &  Me.Name & gvGudang.Name & ".xml")
        'End If
        'If Exists(folderLayouts &  Me.Name & gvSatuan.Name & ".xml") Then
        '    gvSatuan.RestoreLayoutFromXml(folderLayouts &  Me.Name & gvSatuan.Name & ".xml")
        'End If
        'If Exists(folderLayouts &  Me.Name & gvJual.Name & ".xml") Then
        '    gvJual.RestoreLayoutFromXml(folderLayouts &  Me.Name & gvJual.Name & ".xml")
        'End If
        If Exists(folderLayouts & Me.Name & LayoutControl1.Name & ".xml") Then
            LayoutControl1.RestoreLayoutFromXml(folderLayouts & Me.Name & LayoutControl1.Name & ".xml")
        End If
    End Sub

    Private Sub frmEntriReturJualD_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        Try
            If DialogResult = Windows.Forms.DialogResult.OK Then
                FormPemanggil.RefreshDetil()
                FormPemanggil.Show()
                FormPemanggil.Focus()
                FormPemanggil.txtBarang.Focus()
            Else
                If IsFastEntri Then
                    EksekusiSQL("DELETE FROM MReturJualD WHERE NoID=" & NoID)
                End If
                FormPemanggil.Show()
                FormPemanggil.Focus()
            End If
            Me.Dispose()
        Catch ex As Exception

        End Try
    End Sub
    Private Sub frmEntriJualD_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim dlg As DevExpress.Utils.WaitDialogForm = Nothing
        Try
            Dim curentcursor As Cursor = Windows.Forms.Cursor.Current
            Windows.Forms.Cursor.Current = Cursors.WaitCursor
            dlg = New DevExpress.Utils.WaitDialogForm(String.Format("Creating component and analize database.{0}MOHON TUNGGU ...", vbCrLf), Application.ProductName)
            dlg.TopMost = False
            dlg.Show()
            Windows.Forms.Cursor.Current = Cursors.WaitCursor
            IsiDefault()
            If Not IsNew Then
                LoadData()
            End If
            Me.Width = Ini.BacaIniPath(folderLayouts & Me.Name & ".ini", "Form", "Width", Me.Width)
            Me.Height = Ini.BacaIniPath(folderLayouts & Me.Name & ".ini", "Form", "Height", Me.Height)

            Me.Left = (Screen.PrimaryScreen.WorkingArea.Width - Me.Width) \ 2
            Me.Top = (Screen.PrimaryScreen.WorkingArea.Height - Me.Height) \ 2
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
    Private Sub HitungJumlah()
        Dim ds As New DataSet
        Dim IsiKarton As Double = 0.0
        Try
            'Dim Harga As Double = 0.0
            'If txtJual.Text <> "" Then
            '    Harga = IIf(HargaPcs = 0, txtHarga.EditValue, HargaPcs) * IIf(txtKonversi.EditValue = 0, 1, txtKonversi.EditValue)
            'Else
            '    Harga = txtHarga.EditValue
            'End If
            'Dim SubTotal As Double = (Harga - txtDiscRp1.EditValue - txtDiscRp2.EditValue - txtDiscRp3.EditValue)
            'Dim DiscA As Double = Harga * txtQty.EditValue * txtDiscPersen1.EditValue / 100
            'Dim DiscB As Double = Harga * txtQty.EditValue * txtDiscPersen2.EditValue / 100
            'Dim DiscC As Double = Harga * txtQty.EditValue * txtDiscPersen3.EditValue / 100
            'txtHarga.EditValue = Harga
            'txtJUmlah.EditValue = (SubTotal * txtQty.EditValue) - DiscA - DiscB - DiscC
            '            txtJUmlah.EditValue = txtQty.EditValue * (Bulatkan((Harga * (1 - (txtDiscPersen1.EditValue / 100)) * (1 - (txtDiscPersen2.EditValue / 100)) * (1 - (txtDiscPersen3.EditValue / 100))), 0) - txtDiscRp1.EditValue - txtDiscRp2.EditValue - txtDiscRp3.EditValue)
            SQL = "SELECT MBarangD.Konversi, MBarang.CtnPcs FROM MBarangD LEFT JOIN MBarang ON MBarang.NoID=MBarangD.IDBarang LEFT JOIN MSatuan ON MSatuan.NoID=MBarangD.IDSatuan WHERE MBarangD.IDBarang=" & NullToLong(txtBarang.EditValue) & " AND MSatuan.NoID=" & NullToLong(txtSatuan.EditValue)
            Ds = ExecuteDataset("Tabel", SQL)
            If Ds.Tables("Tabel").Rows.Count >= 1 Then
                txtKonversi.EditValue = NullToDbl(Ds.Tables(0).Rows(0).Item("Konversi"))
                IsiKarton = NullToDbl(Ds.Tables(0).Rows(0).Item("CtnPcs"))
                If IsiKarton = 0 Then
                    txtCtn.EditValue = 0
                Else
                    txtCtn.EditValue = txtQty.EditValue / IsiKarton * txtKonversi.EditValue
                End If
            End If
            txtJUmlah.EditValue = txtQty.EditValue * (Bulatkan((txtHarga.EditValue * (1 - (txtDiscPersen1.EditValue / 100)) * (1 - (txtDiscPersen2.EditValue / 100)) * (1 - (txtDiscPersen3.EditValue / 100))), 0) - txtDiscRp1.EditValue - txtDiscRp2.EditValue - txtDiscRp3.EditValue)
        Catch ex As Exception
        Finally
            ds.Dispose()
        End Try
    End Sub

    Private Sub txtQty_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtQty.LostFocus
        HitungJumlah()
    End Sub

    Private Sub txtDiscPersen1_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtDiscPersen1.LostFocus
        HitungJumlah()
    End Sub
    Private Sub txtDiscPersen2_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtDiscPersen2.LostFocus
        HitungJumlah()
    End Sub
    Private Sub txtDiscPersen3_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtDiscPersen3.LostFocus
        HitungJumlah()
    End Sub
    Private Sub txtDiscPersen1_EditValueChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtDiscPersen1.EditValueChanged
        HitungJumlah()
    End Sub
    Private Sub txtDiscPersen2_EditValueChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtDiscPersen2.EditValueChanged
        HitungJumlah()
    End Sub
    Private Sub txtDiscPersen3_EditValueChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtDiscPersen3.EditValueChanged
        HitungJumlah()
    End Sub

    Private Sub txtDisc1_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtDiscRp1.LostFocus
        HitungJumlah()
    End Sub
    Private Sub txtDisc2_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtDiscRp2.LostFocus
        HitungJumlah()
    End Sub
    Private Sub txtDisc3_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtDiscRp3.LostFocus
        HitungJumlah()
    End Sub
    Private Sub txtDisc1_EditValueChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtDiscRp1.EditValueChanged
        HitungJumlah()
    End Sub
    Private Sub txtDisc2_EditValueChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtDiscRp2.EditValueChanged
        HitungJumlah()
    End Sub
    Private Sub txtDisc3_EditValueChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtDiscRp3.EditValueChanged
        HitungJumlah()
    End Sub

    Private Sub txtBarang_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtBarang.EditValueChanged
        Dim Ds As New DataSet
        Try
            SQL = "SELECT MBarang.Nama AS NamaBarang FROM MBarang WHERE MBarang.NoID=" & NullTolong(txtBarang.EditValue)
            Ds = ExecuteDataset("Tabel", SQL)
            If Ds.Tables("Tabel").Rows.Count >= 1 Then
                'txtSatuan.EditValue = NullTolong(Ds.Tables(0).Rows(0).Item("IDSatuan"))
                txtNamaStock.Text = NullTostr(Ds.Tables(0).Rows(0).Item("NamaBarang"))
                'txtKonversi.EditValue = NullToDbl(Ds.Tables(0).Rows(0).Item("Konversi"))
                RefreshLookUpSatuan()
                RefreshLookUpJual()
                If IsNew Then
                    txtSatuan.EditValue = DefIDSatuan
                End If
            End If
        Catch ex As Exception
            XtraMessageBox.Show("Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            Ds.Dispose()
        End Try
    End Sub

    Private Sub txtSatuan_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtSatuan.EditValueChanged
        RubahSatuan()
    End Sub
    Private Sub RubahSatuan()
        Dim Ds As New DataSet
        Dim IsiKarton As Double = 0.0
        Try
            SQL = "SELECT MBarangD.Konversi, MBarang.CtnPcs FROM MBarangD LEFT JOIN MBarang ON MBarang.NoID=MBarangD.IDBarang LEFT JOIN MSatuan ON MSatuan.NoID=MBarangD.IDSatuan WHERE MBarangD.IDBarang=" & NullTolong(txtBarang.EditValue) & " AND MSatuan.NoID=" & NullTolong(txtSatuan.EditValue)
            Ds = ExecuteDataset("Tabel", SQL)
            If Ds.Tables("Tabel").Rows.Count >= 1 Then
                txtKonversi.EditValue = NullToDbl(Ds.Tables(0).Rows(0).Item("Konversi"))
                IsiKarton = NullToDbl(Ds.Tables(0).Rows(0).Item("CtnPcs"))
                Konversi = txtKonversi.EditValue
                If IsiKarton = 0 Then
                    txtCtn.EditValue = 0
                Else
                    txtCtn.EditValue = txtQty.EditValue / IsiKarton * txtKonversi.EditValue
                End If
                If txtJual.Text <> "" AndAlso (IsNew Or IsFastEntri) Then
                    txtHarga.EditValue = HargaPcs * Konversi
                ElseIf txtHarga.EditValue = 0 Or IsNew Or IsFastEntri Then
                    'txtHarga.EditValue = clsPostingPenjualan.HargaJual(NullToLong(txtBarang.EditValue), NullToLong(txtSatuan.EditValue), IDCustomer, txtDiscPersen1.EditValue, txtDiscPersen2.EditValue)
                End If
            End If
            HitungJumlah()
        Catch ex As Exception
            XtraMessageBox.Show("Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            Ds.Dispose()
        End Try
    End Sub

    'Private Sub txtJual_ButtonClick(ByVal sender As Object, ByVal e As DevExpress.XtraEditors.Controls.ButtonPressedEventArgs) Handles txtJual.ButtonClick
    '    With gvJual
    '        For x As Integer = 0 To .Columns.Count - 1
    '            Select Case .Columns(x).ColumnType.Name.ToLower
    '                Case "int32", "int64", "int"
    '                    .Columns(x).DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
    '                    .Columns(x).DisplayFormat.FormatString = "n0"
    '                Case "decimal", "single", "money", "double"
    '                    .Columns(x).DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
    '                    .Columns(x).DisplayFormat.FormatString = "n2"
    '                Case "string"
    '                    .Columns(x).DisplayFormat.FormatType = DevExpress.Utils.FormatType.None
    '                    .Columns(x).DisplayFormat.FormatString = ""
    '                Case "date", "datetime"
    '                    If .Columns(x).FieldName.Trim.ToLower = "jam" Then
    '                        .Columns(x).DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime
    '                        .Columns(x).DisplayFormat.FormatString = "HH:mm"
    '                    Else
    '                        .Columns(x).DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime
    '                        .Columns(x).DisplayFormat.FormatString = "dd-MM-yyyy"
    '                    End If
    '            End Select
    '        Next
    '    End With
    'End Sub

    Private Sub txtJual_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtJual.EditValueChanged
        Dim Ds As New DataSet
        Try
            If IsNew Or IsFastEntri Then
                SQL = "SELECT MJualD.*, (MJualD.Qty*MJualD.Konversi)-" & DiRetur() & " AS QtySisa, MBarang.Kode AS KodeBarang, MBarang.Nama AS NamaBarang, MGudang.Kode AS KodeGudang, MGudang.Nama AS NamaGudang, MSatuan.Nama AS NamaSatuan, "
                SQL &= " (MJualD.Jumlah/isnull(MJualD.Konversi,1)/isnull(MJualD.Qty,1))-(IsNull(MJual.DiskonNotaProsen,0)/100*(MJualD.Jumlah/isnull(MJualD.Konversi,1)/isnull(MJualD.Qty,1))) AS [HargaPcsFix], IsNull(MJual.DiskonNotaProsen,0) AS DiscProsenBawah, IsNull((SELECT TOP 1 MRevisiHargaJualD.KoreksiJL*MRevisiHargaJualD.Konversi FROM MRevisiHargaJualD INNER JOIN MRevisiHargaJual ON MRevisiHargaJual.NoID=MRevisiHargaJualD.IDRevisiHargaJual WHERE MJualD.NoID=MRevisiHargaJualD.IDJualD AND MJual.NoID=MRevisiHargaJualD.IDJual AND MRevisiHargaJual.IsPosted=1 ORDER BY MRevisiHargaJual.Tanggal DESC),0) AS TotalPH "
                SQL &= " FROM (((MJualD INNER JOIN MJual ON MJual.NoID=MJualD.IDJual) LEFT JOIN MGudang ON MGudang.NoID=MJualD.IDGudang) "
                SQL &= " LEFT JOIN MBarang ON MBarang.NoID=MJualD.IDBarang)"
                SQL &= " LEFT JOIN MSatuan ON MSatuan.NoID=MJualD.IDSatuan "
                SQL &= " WHERE MJualD.NoID= " & NullToLong(txtJual.EditValue)
                Ds = New DataSet()
                Ds = ExecuteDataset("MJualD", SQL)
                If Ds.Tables(0).Rows.Count >= 1 Then
                    With Ds.Tables("MJualD").Rows(0)
                        gvJual.SaveLayoutToXml(FolderLayouts & Me.Name & gvJual.Name & ".xml")
                        txtBarang.EditValue = NullToLong(.Item("IDBarang"))
                        'txtBarang.Text = NullTostr(.Item("KodeBarang"))
                        txtSatuan.EditValue = NullToLong(.Item("IDSatuan"))
                        'txtSatuan.Text = NullTostr(.Item("KodeSatuan"))
                        txtGudang.EditValue = NullToLong(.Item("IDGudang"))
                        'txtGudang.Text = NullTostr(.Item("KodeGudang"))
                        QtySisa = NullToDbl(.Item("QtySisa"))
                        Konversi = IIf(NullToDbl(.Item("Konversi")) = 0, 1, NullToDbl(.Item("Konversi")))
                        HargaPcs = ((NullToDbl(.Item("Harga")) / Konversi) - (NullToDbl(.Item("Harga")) / Konversi / 100 * NullToDbl(.Item("DiscProsenBawah")))) - (NullToDbl(.Item("TotalPH")) / Konversi)

                        txtQty.EditValue = NullToDbl(.Item("QtySisa")) / Konversi
                        txtHarga.EditValue = (NullToDbl(.Item("Harga")) - (NullToDbl(.Item("Harga")) / 100 * NullToDbl(.Item("DiscProsenBawah")))) - NullToDbl(.Item("TotalPH"))

                        txtCtn.EditValue = NullToDbl(.Item("CTN"))
                        txtDiscPersen1.EditValue = NullToDbl(.Item("DiscPersen1"))
                        txtDiscPersen2.EditValue = NullToDbl(.Item("DiscPersen2"))
                        txtDiscPersen3.EditValue = NullToDbl(.Item("DiscPersen3"))
                        txtDiscRp1.EditValue = NullToDbl(.Item("Disc1"))
                        txtDiscRp2.EditValue = NullToDbl(.Item("Disc2"))
                        txtDiscRp3.EditValue = NullToDbl(.Item("Disc3"))
                        txtJUmlah.EditValue = NullToDbl(.Item("Jumlah"))
                        txtCatatan.Text = NullToStr(.Item("Catatan"))
                        txtKonversi.EditValue = Konversi
                        KonversiJual = Konversi
                        HitungJumlah()
                    End With
                Else
                    IsiDefault()
                End If
            End If
        Catch ex As Exception
            XtraMessageBox.Show("Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            Ds.Dispose()
        End Try
    End Sub

    Private Sub BarButtonItem1_ItemClick_1(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem1.ItemClick
        cmdSave.PerformClick()
    End Sub

    Private Sub BarButtonItem2_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem2.ItemClick
        cmdTutup.PerformClick()
    End Sub

    Private Sub txtHarga_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtHarga.LostFocus
        HitungJumlah()
    End Sub

    'Private Sub txtJual_EditValueChanging(ByVal sender As Object, ByVal e As DevExpress.XtraEditors.Controls.ChangingEventArgs) Handles txtJual.EditValueChanging
    '    If System.IO.File.Exists(folderLayouts &  Me.Name & gvJual.Name & ".xml") Then
    '        gvJual.RestoreLayoutFromXml(folderLayouts &  Me.Name & gvJual.Name & ".xml")
    '    End If
    '    With gvJual
    '        For x As Integer = 0 To .Columns.Count - 1
    '            Select Case .Columns(x).ColumnType.Name.ToLower
    '                Case "int32", "int64", "int"
    '                    .Columns(x).DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
    '                    .Columns(x).DisplayFormat.FormatString = "n0"
    '                Case "decimal", "single", "money", "double"
    '                    .Columns(x).DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
    '                    .Columns(x).DisplayFormat.FormatString = "n2"
    '                Case "string"
    '                    .Columns(x).DisplayFormat.FormatType = DevExpress.Utils.FormatType.None
    '                    .Columns(x).DisplayFormat.FormatString = ""
    '                Case "date", "datetime"
    '                    If .Columns(x).FieldName.Trim.ToLower = "jam" Then
    '                        .Columns(x).DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime
    '                        .Columns(x).DisplayFormat.FormatString = "HH:mm"
    '                    Else
    '                        .Columns(x).DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime
    '                        .Columns(x).DisplayFormat.FormatString = "dd-MM-yyyy"
    '                    End If
    '            End Select
    '        Next
    '    End With
    'End Sub

    Private Sub gvJual_DataSourceChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvJual.DataSourceChanged
        If System.IO.File.Exists(folderLayouts & Me.Name & gvJual.Name & ".xml") Then
            gvJual.RestoreLayoutFromXml(folderLayouts & Me.Name & gvJual.Name & ".xml")
        End If
        With gvJual
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

    Private Sub txtGudang_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtGudang.EditValueChanged
        RubahGudang()
    End Sub
    Private Sub RubahGudang()
        Dim Ds As New DataSet
        Try
            SQL = "SELECT MWilayah.Nama AS Wilayah FROM MGudang LEFT JOIN MWilayah ON MWilayah.NoID=MGudang.IDWilayah WHERE MGudang.NoID=" & NullTolong(txtGudang.EditValue)
            Ds = ExecuteDataset("Tabel", SQL)
            If Ds.Tables("Tabel").Rows.Count >= 1 Then
                txtWilayah.Text = NullTostr(Ds.Tables(0).Rows(0).Item("Wilayah"))
            End If
            RefreshLookUp()
        Catch ex As Exception
            XtraMessageBox.Show("Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            Ds.Dispose()
        End Try
    End Sub

    Private Sub txtQty_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtQty.EditValueChanged
        HitungJumlah()
    End Sub

    Private Sub gvBarang_DataSourceChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvBarang.DataSourceChanged
        If System.IO.File.Exists(folderLayouts & Me.Name & gvBarang.Name & ".xml") Then
            gvBarang.RestoreLayoutFromXml(folderLayouts & Me.Name & gvBarang.Name & ".xml")
        End If
        With gvBarang
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

    Private Sub gvSatuan_DataSourceChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvSatuan.DataSourceChanged
        If System.IO.File.Exists(folderLayouts & Me.Name & gvSatuan.Name & ".xml") Then
            gvSatuan.RestoreLayoutFromXml(folderLayouts & Me.Name & gvSatuan.Name & ".xml")
        End If
        With gvSatuan
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

    Private Sub txtKonversi_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtKonversi.EditValueChanged

    End Sub

    Private Sub txtKonversi_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtKonversi.KeyDown
        If e.KeyCode = Keys.F7 Then
            Dim x As New frmOtorisasiAdmin
            Try
                If txtKonversi.Properties.ReadOnly Then
                    If x.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
                        txtKonversi.Properties.ReadOnly = False
                    End If
                End If
            Catch ex As Exception
                XtraMessageBox.Show("Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End If
    End Sub

    Private Sub txtKonversi_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtKonversi.LostFocus
        HitungJumlah()
    End Sub

    Private Sub txtHarga_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtHarga.EditValueChanged
        HitungJumlah()
    End Sub

    Private Sub txtSatuan_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtSatuan.LostFocus
        RubahSatuan()
    End Sub
End Class