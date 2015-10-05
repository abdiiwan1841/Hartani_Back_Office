Imports System.Data
Imports System.Data.SqlClient
Imports DevExpress.XtraEditors
Imports DevExpress.Utils
Imports System.IO.File

Public Class frmEntriRevisiBeliD
    Public IsNew As Boolean = True
    Public IsPosted As Boolean = False
    Public NoID As Long = -1
    Public IDRevisiHargaBeli As Long = -1
    Public IDSupplier As Long = -1
    Public FormPemanggil As frmEntriRevisiBeli
    Public IsFastEntri As Boolean = False
    Public IDBarangDef As Long = -1

    Dim oDS As New DataSet
    Dim SQL As String = ""
    Dim DefImageList As ImageList
    Dim HargaPcs As Double = 0.0
    Dim Konversi As Double = 0.0
    Dim IDAdmin As Long = -1
    Dim IDBeli As Long = -1
    Dim SelisihBeli As Double = 0.0

    'For Back Action
    Public FormNameDaftar As String = ""
    Public TableNameDaftar As String = ""
    Public TextDaftar As String = ""
    Public FormEntriDaftar As String = ""
    Public TableMasterDaftar As String = ""

    Private Sub IsiDefault()
        DefImageList = frmMain.ImageList1
        cmdSave.ImageList = DefImageList
        cmdSave.ImageIndex = 6

        cmdTutup.ImageList = DefImageList
        cmdTutup.ImageIndex = 3
        TglDari.DateTime = CDate(TanggalSystem.ToString("yyyy/MM/01"))
        tglSampai.DateTime = CDate(TanggalSystem.ToString("yyyy/MM/dd"))
        RefreshLookUp()
        Konversi = 1.0
        HargaPcs = 0.0
        txtGudang.EditValue = DefIDGudang
    End Sub
    Private Sub RefreshLookUp()
        Dim ds As New DataSet
        Try
            SQL = "SELECT MBeliD.NoID, MWilayah.Nama AS Wilayah, MGudang.Nama AS Gudang, MBeli.Kode AS KodeBeli, MBarang.Kode AS KodeStock, MBeliD.Qty AS QtyBeli, MSatuan.Nama AS SatuanBeli, "
            SQL &= " (MBeliD.HargaNetto/isnull(MBeliD.Konversi,1))-(MBeliD.HargaNetto/isnull(MBeliD.Konversi,1)*IsNull(MBeli.DiskonNotaProsen,0)/100) AS [Harga (Pcs)], "
            SQL &= " (MBeliD.Qty*MBeliD.Konversi) AS [Qty (Pcs)]"
            SQL &= " FROM MBeliD "
            SQL &= " LEFT JOIN (MGudang LEFT JOIN MWilayah ON MWilayah.NoID=MGudang.IDWilayah) ON MGudang.NoID=MBeliD.IDGudang "
            SQL &= " LEFT JOIN MBeli ON MBeli.NoID=MBeliD.IDBeli LEFT JOIN MBarang ON MBarang.NoID=MBeliD.IDBarang LEFT JOIN MSatuan ON MSatuan.NoID=MBeliD.IDSatuan "
            SQL &= " WHERE MBeli.IDSupplier=" & IDSupplier & " AND MWilayah.NoID=" & DefIDWilayah & " AND MBarang.IsActive=1 AND MBeli.IsPosted=1 "
            If IDBarangDef >= 1 Then
                SQL &= " AND MBeliD.IDBarang=" & IDBarangDef
            End If
            If TglDari.Text <> "" And tglSampai.Text <> "" Then
                SQL &= " AND (MBeli.Tanggal>='" & Format(TglDari.DateTime, "yyyy/MM/dd") & "' AND MBeli.Tanggal<'" & Format(DateAdd(DateInterval.Day, 1, tglSampai.DateTime), "yyyy/MM/dd") & "') "
            End If
            ds = ExecuteDataset("MBeliD", SQL)
            txtBeli.Properties.DataSource = ds.Tables("MBeliD")
            txtBeli.Properties.ValueMember = "NoID"
            txtBeli.Properties.DisplayMember = "KodeBeli"

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
            SQL = "SELECT MRevisiHargaBeliD.* FROM MRevisiHargaBeliD WHERE MRevisiHargaBeliD.NoID=" & NoID
            oDS = New DataSet()
            oDS = ExecuteDataset("MRevisiHargaBeliD", SQL)
            If oDS.Tables(0).Rows.Count >= 1 Then
                With oDS.Tables("MRevisiHargaBeliD").Rows(0)
                    NoID = NullTolong(.Item("NoID"))
                    txtBeli.EditValue = NullTolong(.Item("IDBeliD"))
                    IDBeli = NullTolong(.Item("IDBeli"))
                    txtBarang.EditValue = NullTolong(.Item("IDBarang"))
                    txtSatuan.EditValue = NullTolong(.Item("IDSatuan"))
                    txtGudang.EditValue = NullTolong(.Item("IDGudang"))
                    txtQty.EditValue = NullToDbl(.Item("Qty"))
                    txtHarga.EditValue = NullToDbl(.Item("Harga"))
                    Konversi = IIf(NullToDbl(.Item("Konversi")) = 0, 1, NullToDbl(.Item("Konversi")))
                    HargaPcs = NullToDbl(.Item("Harga")) / Konversi
                    txtCtn.EditValue = NullToDbl(.Item("CTN"))
                    txtDiscPersen1.EditValue = NullToDbl(.Item("DiscPersen1"))
                    txtDiscPersen2.EditValue = NullToDbl(.Item("DiscPersen2"))
                    txtDiscPersen3.EditValue = NullToDbl(.Item("DiscPersen3"))
                    txtDiscRp1.EditValue = NullToDbl(.Item("Disc1"))
                    txtDiscRp2.EditValue = NullToDbl(.Item("Disc2"))
                    txtDiscRp3.EditValue = NullToDbl(.Item("Disc3"))
                    txtJUmlah.EditValue = NullToDbl(.Item("Jumlah"))
                    txtCatatan.Text = NullToStr(.Item("Catatan"))
                    txtKonversi.EditValue = NullToDbl(.Item("Konversi"))
                    txtTgl.DateTime = NullToDate(.Item("TanggalBeli"))

                    'Revisi
                    txtHargaRevisi.EditValue = NullToDbl(.Item("HargaBaru"))
                    txtDiscPersen1Revisi.EditValue = NullToDbl(.Item("DiscPersen1Baru"))
                    txtDiscPersen2Revisi.EditValue = NullToDbl(.Item("DiscPersen2Baru"))
                    txtDiscPersen3Revisi.EditValue = NullToDbl(.Item("DiscPersen3Baru"))
                    txtDiscRp1Revisi.EditValue = NullToDbl(.Item("Disc1Baru"))
                    txtDiscRp2Revisi.EditValue = NullToDbl(.Item("Disc2Baru"))
                    txtDiscRp3Revisi.EditValue = NullToDbl(.Item("Disc3Baru"))
                    txtJumlahRevisi.EditValue = NullToDbl(.Item("JumlahBaru"))
                    txtSelisihPcsRevisi.EditValue = NullToDbl(.Item("Koreksi"))
                    txtCatatanRevisi.Text = NullToStr(.Item("Keterangan"))
                    HitungJumlah()
                    If NullToBool(.Item("IsPosted")) Then
                        cmdSave.Enabled = False
                    End If
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
                SQL = "INSERT INTO MRevisiHargaBeliD ([NoID],[Kode],[KodeReff],[IDBeliD],[IDRevisiHargaBeli]," & _
                      "[IDBeli],[IDBarang],[Keterangan],[IDSatuan],[IDGudang]," & _
                      "[Ctn],[Qty],[Harga],[RevisiHarga],[DiscPersen1],[DiscPersen2],[DiscPersen3]," & _
                      "[Disc1],[Disc2],[Disc3],[Jumlah],[IDLPBD],[IDPOD],[IDAkunPersediaan],[Catatan]," & _
                      "[NoUrut],[HargaPcs],[HargaPcsBaru],[QtyPcs],[IDAsal],[Konversi],[IDServerAsal]," & _
                      "[HargaBaru],[DiscPersen1Baru],[DiscPersen2Baru],[DiscPersen3Baru],[Disc1Baru],[Disc2Baru],[Disc3Baru],[JumlahBaru],[Koreksi],[IsPosted],[TanggalBeli],[IDUserPosting],[TglPosting],[IDWilayah],[IDUserEntry],[IDUserEdit],[IDAdmin],[IDSupplier],[KoreksiBL],[HargaNettoLama],[HargaNettoBaru]) " & vbCrLf & _
                      " SELECT " & NullToLong(GetNewID("MRevisiHargaBeliD", "NoID")) & "," & vbCrLf & _
                      "MBeli.Kode," & _
                      "'" & FixApostropi(txtKodeReff.Text) & "'," & _
                      "MBeliD.NoID," & _
                      IDRevisiHargaBeli & "," & _
                      "MBeliD.IDBeli," & _
                      "MBeliD.IDBarang," & _
                      "'" & FixApostropi(txtCatatanRevisi.Text) & "'," & _
                      "MBeliD.IDSatuan," & _
                      "MBeliD.IDGudang," & _
                      "MBeliD.Ctn," & _
                      "MBeliD.Qty," & _
                      "" & FixKoma(txtHarga.EditValue) & "," & _
                      FixKoma(txtHargaRevisi.EditValue) & "," & vbCrLf & _
                      "" & FixKoma(txtDiscPersen1.EditValue) & "," & _
                      "" & FixKoma(txtDiscPersen2.EditValue) & "," & _
                      "" & FixKoma(txtDiscPersen3.EditValue) & "," & _
                      "" & FixKoma(txtDiscRp1.EditValue) & "," & _
                      "" & FixKoma(txtDiscRp2.EditValue) & "," & _
                      "" & FixKoma(txtDiscRp3.EditValue) & "," & _
                      "" & FixKoma(txtJUmlah.EditValue) & "," & _
                      "MBeliD.IDLPBD," & _
                      "MBeliD.IDPOD," & _
                      "MBeliD.IDAkunPersediaan," & _
                      "MBeliD.Catatan," & _
                      "MBeliD.NoUrut," & _
                      FixKoma(txtJUmlah.EditValue / IIf(txtQty.EditValue = 0, 1, txtQty.EditValue) / IIf(txtKonversi.EditValue = 0, 1, txtKonversi.EditValue)) & "," & _
                      FixKoma(txtJumlahRevisi.EditValue / IIf(txtQty.EditValue = 0, 1, txtQty.EditValue) / IIf(txtKonversi.EditValue = 0, 1, txtKonversi.EditValue)) & ",MBeliD.QtyPcs,MBeliD.IDAsal,MBeliD.Konversi,MBeliD.IDServerAsal," & _
                      FixKoma(txtHargaRevisi.EditValue) & "," & FixKoma(txtDiscPersen1Revisi.EditValue) & ", " & _
                      FixKoma(txtDiscPersen2Revisi.EditValue) & "," & FixKoma(txtDiscPersen3Revisi.EditValue) & "," & _
                      FixKoma(txtDiscRp1Revisi.EditValue) & "," & FixKoma(txtDiscRp2Revisi.EditValue) & "," & FixKoma(txtDiscRp3Revisi.EditValue) & "," & _
                      FixKoma(txtJumlahRevisi.EditValue) & "," & FixKoma(txtSelisihPcsRevisi.EditValue) & ",0,MBeli.Tanggal,NULL,NULL," & DefIDWilayah & "," & IDUserAktif & ",NULL," & IDAdmin & ", MBeli.IDSupplier," & FixKoma(SelisihBeli) & ", " & _
                      FixKoma(Bulatkan((txtHarga.EditValue * (1 - (txtDiscPersen1.EditValue / 100)) * (1 - (txtDiscPersen2.EditValue / 100)) * (1 - (txtDiscPersen3.EditValue / 100))), 0) - txtDiscRp1.EditValue - txtDiscRp2.EditValue - txtDiscRp3.EditValue) & "," & _
                      FixKoma(Bulatkan((txtHargaRevisi.EditValue * (1 - (txtDiscPersen1Revisi.EditValue / 100)) * (1 - (txtDiscPersen3Revisi.EditValue / 100)) * (1 - (txtDiscPersen3Revisi.EditValue / 100))), 0) - txtDiscRp1Revisi.EditValue - txtDiscRp2Revisi.EditValue - txtDiscRp3Revisi.EditValue) & _
                      " FROM MBeliD LEFT JOIN MBeli ON MBeli.NoID=MBeliD.IDBeli WHERE MBeliD.NoID=" & NullToLong(txtBeli.EditValue)
            Else
                EksekusiSQL("UPDATE MRevisiHargaBeliD SET IDBeliD=" & NullTolong(txtBeli.EditValue) & ", IDBeli=" & IDBeli & " WHERE NoID=" & NoID)
                SQL = "UPDATE MRevisiHargaBeliD SET " & _
                      "Kode=MBeli.Kode," & _
                      "KodeReff='" & FixApostropi(txtKodeReff.Text) & "'," & _
                      "IDBeliD=MBeliD.NoID," & _
                      "IDBeli=MBeliD.IDBeli," & _
                      "IDBarang=MBeliD.IDBarang," & _
                      "IDSupplier=MBeli.IDSupplier," & _
                      "Keterangan='" & FixApostropi(txtCatatanRevisi.Text) & "'," & _
                      "IDSatuan=MBeliD.IDSatuan," & _
                      "IDGudang=MBeliD.IDGudang," & _
                      "Ctn=MBeliD.Ctn," & _
                      "Qty=MBeliD.Qty," & _
                      "Harga=" & FixKoma(txtHarga.EditValue) & "," & _
                      "RevisiHarga=" & FixKoma(txtHargaRevisi.EditValue) & "," & vbCrLf & _
                      "DiscPersen1=" & FixKoma(txtDiscPersen1.EditValue) & "," & _
                      "DiscPersen2=" & FixKoma(txtDiscPersen2.EditValue) & "," & _
                      "DiscPersen3=" & FixKoma(txtDiscPersen3.EditValue) & "," & _
                      "Disc1=" & FixKoma(txtDiscRp1.EditValue) & "," & _
                      "Disc2=" & FixKoma(txtDiscRp2.EditValue) & "," & _
                      "Disc3=" & FixKoma(txtDiscRp3.EditValue) & "," & _
                      "Jumlah=" & FixKoma(txtJUmlah.EditValue) & "," & _
                      "IDLPBD=MBeliD.IDLPBD," & _
                      "IDPOD=MBeliD.IDPOD," & _
                      "IDAkunPersediaan=MBeliD.IDAkunPersediaan," & _
                      "Catatan=MBeliD.Catatan," & _
                      "NoUrut=MBeliD.NoUrut," & _
                      "HargaPcs=" & FixKoma(txtJUmlah.EditValue / IIf(txtQty.EditValue = 0, 1, txtQty.EditValue) / IIf(txtKonversi.EditValue = 0, 1, txtKonversi.EditValue)) & ",HargaPcsBaru=" & FixKoma(txtJumlahRevisi.EditValue / IIf(txtQty.EditValue = 0, 1, txtQty.EditValue) / IIf(txtKonversi.EditValue = 0, 1, txtKonversi.EditValue)) & ",QtyPcs=MBeliD.QtyPcs,IDAsal=MBeliD.IDAsal,Konversi=MBeliD.Konversi,IDServerAsal=MBeliD.IDServerAsal," & _
                      "HargaBaru=" & FixKoma(txtHargaRevisi.EditValue) & ",DiscPersen1Baru=" & FixKoma(txtDiscPersen1Revisi.EditValue) & ", " & _
                      "DiscPersen2Baru=" & FixKoma(txtDiscPersen2Revisi.EditValue) & ",DiscPersen3Baru=" & FixKoma(txtDiscPersen3Revisi.EditValue) & "," & _
                      "Disc1Baru=" & FixKoma(txtDiscRp1Revisi.EditValue) & ",Disc2Baru=" & FixKoma(txtDiscRp2Revisi.EditValue) & ",Disc3Baru=" & FixKoma(txtDiscRp3Revisi.EditValue) & "," & _
                      "JumlahBaru=" & FixKoma(txtJumlahRevisi.EditValue) & ",KoreksiBL=" & FixKoma(SelisihBeli) & ",Koreksi=" & FixKoma(txtSelisihPcsRevisi.EditValue) & ",TanggalBeli=MBeli.Tanggal,IDWilayah=" & DefIDWilayah & ",IDUserEdit=" & IDUserAktif & "," & _
                      "HargaNettoLama=" & FixKoma(Bulatkan((txtHarga.EditValue * (1 - (txtDiscPersen1.EditValue / 100)) * (1 - (txtDiscPersen2.EditValue / 100)) * (1 - (txtDiscPersen3.EditValue / 100))), 0) - txtDiscRp1.EditValue - txtDiscRp2.EditValue - txtDiscRp3.EditValue) & "," & _
                      "HargaNettoBaru=" & FixKoma(Bulatkan((txtHargaRevisi.EditValue * (1 - (txtDiscPersen1Revisi.EditValue / 100)) * (1 - (txtDiscPersen3Revisi.EditValue / 100)) * (1 - (txtDiscPersen3Revisi.EditValue / 100))), 0) - txtDiscRp1Revisi.EditValue - txtDiscRp2Revisi.EditValue - txtDiscRp3Revisi.EditValue) & _
                      IIf(txtTgl.Properties.ReadOnly = False, ", IDAdmin=" & IDAdmin, "") & _
                      " FROM (MRevisiHargaBeliD LEFT JOIN MBeliD ON MbeliD.NoID=MRevisiHargaBeliD.IDBeliD) " & _
                      " LEFT JOIN MBeli ON MBeli.NoID=MBeliD.IDBeli WHERE MRevisiHargaBeliD.NoID=" & NoID
            End If
            If EksekusiSQL(SQL) >= 1 Then
                Return True
            Else
                Return False
            End If
        Catch ex As Exception
            XtraMessageBox.Show("Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
            Return False
        End Try
    End Function
    Private Function IsValidasi() As Boolean
        If txtBeli.Text = "" Then
            XtraMessageBox.Show("Faktur Pembelian masih kosong.", NamaAplikasi, MessageBoxButtons.YesNo, MessageBoxIcon.Information)
            txtBeli.Focus()
            Return False
            Exit Function
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
        If txtDiscPersen1Revisi.EditValue < 0 Then
            XtraMessageBox.Show("Disc persen 1 tidak boleh kurang dari 0.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
            txtDiscPersen1Revisi.Focus()
            Return False
            Exit Function
        End If
        If txtDiscPersen2Revisi.EditValue < 0 Then
            XtraMessageBox.Show("Disc persen 1 tidak boleh kurang dari 0.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
            txtDiscPersen2Revisi.Focus()
            Return False
            Exit Function
        End If
        If txtDiscPersen3Revisi.EditValue < 0 Then
            XtraMessageBox.Show("Disc persen 1 tidak boleh kurang dari 0.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
            txtDiscPersen2Revisi.Focus()
            Return False
            Exit Function
        End If
        If txtDiscRp1Revisi.EditValue < 0 Then
            XtraMessageBox.Show("Disc rupiah 1 tidak boleh kurang dari 0.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
            txtDiscRp1Revisi.Focus()
            Return False
            Exit Function
        End If
        If txtDiscRp2Revisi.EditValue < 0 Then
            XtraMessageBox.Show("Disc rupiah 2 tidak boleh kurang dari 0.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
            txtDiscRp2Revisi.Focus()
            Return False
            Exit Function
        End If
        If txtDiscRp3Revisi.EditValue < 0 Then
            XtraMessageBox.Show("Disc rupiah 3 tidak boleh kurang dari 0.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
            txtDiscRp3Revisi.Focus()
            Return False
            Exit Function
        End If
        If txtHargaRevisi.EditValue <= 0 Then
            If txtHargaRevisi.EditValue = 0 AndAlso XtraMessageBox.Show("Harga masih kurang dari atau sama dengan 0, lanjutkan penyimpanan?", NamaAplikasi, MessageBoxButtons.YesNo, MessageBoxIcon.Information, MessageBoxDefaultButton.Button2) = Windows.Forms.DialogResult.No Then
                txtHargaRevisi.Focus()
                Return False
                Exit Function
            ElseIf txtHargaRevisi.EditValue < 0 Then
                txtHargaRevisi.Focus()
                Return False
                Exit Function
            End If
        End If
        Return True
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
                gvBeli.SaveLayoutToXml(folderLayouts & Me.Name & gvBeli.Name & ".xml")
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
        'If Exists(folderLayouts &  Me.Name & gvBeli.Name & ".xml") Then
        '    gvBeli.RestoreLayoutFromXml(folderLayouts &  Me.Name & gvBeli.Name & ".xml")
        'End If
        If Exists(folderLayouts & Me.Name & LayoutControl1.Name & ".xml") Then
            LayoutControl1.RestoreLayoutFromXml(folderLayouts & Me.Name & LayoutControl1.Name & ".xml")
        End If
    End Sub

    Private Sub frmEntriRevisiBeliD_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        Try
            If DialogResult = Windows.Forms.DialogResult.OK Then
                FormPemanggil.RefreshDetil()
                FormPemanggil.Show()
                FormPemanggil.Focus()
                FormPemanggil.txtBarang.Focus()
            Else
                If IsFastEntri Then
                    EksekusiSQL("DELETE FROM MRevisiHargaBeliD WHERE NoID=" & NoID)
                End If
                FormPemanggil.Show()
                FormPemanggil.Focus()
            End If
            Me.Dispose()
        Catch ex As Exception

        End Try
    End Sub
    Private Sub frmEntriBeliD_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
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
    Private Sub HitungJumlah()
        Dim HargaBeli As Double = 0.0
        Try
            'Dim Harga As Double = 0.0
            'If txtBeli.Text <> "" Then
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
            txtJUmlah.EditValue = txtQty.EditValue * (Bulatkan((txtHarga.EditValue * (1 - (txtDiscPersen1.EditValue / 100)) * (1 - (txtDiscPersen2.EditValue / 100)) * (1 - (txtDiscPersen3.EditValue / 100))), 0) - txtDiscRp1.EditValue - txtDiscRp2.EditValue - txtDiscRp3.EditValue)
            txtJumlahRevisi.EditValue = txtQty.EditValue * (Bulatkan((txtHargaRevisi.EditValue * (1 - (txtDiscPersen1Revisi.EditValue / 100)) * (1 - (txtDiscPersen2Revisi.EditValue / 100)) * (1 - (txtDiscPersen3Revisi.EditValue / 100))), 0) - txtDiscRp1Revisi.EditValue - txtDiscRp2Revisi.EditValue - txtDiscRp3Revisi.EditValue)
            SelisihBeli = NullToDbl(EksekusiSQlSkalarNew("SELECT HargaPcs*Konversi FROM MBeliD WHERE NoID=" & NullToLong(txtBeli.EditValue))) - (Bulatkan((txtHargaRevisi.EditValue * (1 - (txtDiscPersen1Revisi.EditValue / 100)) * (1 - (txtDiscPersen2Revisi.EditValue / 100)) * (1 - (txtDiscPersen3Revisi.EditValue / 100))), 0) - txtDiscRp1Revisi.EditValue - txtDiscRp2Revisi.EditValue - txtDiscRp3Revisi.EditValue)
            txtSelisihPcsRevisi.EditValue = (Bulatkan((txtHarga.EditValue * (1 - (txtDiscPersen1.EditValue / 100)) * (1 - (txtDiscPersen2.EditValue / 100)) * (1 - (txtDiscPersen3.EditValue / 100))), 0) - txtDiscRp1.EditValue - txtDiscRp2.EditValue - txtDiscRp3.EditValue) - (Bulatkan((txtHargaRevisi.EditValue * (1 - (txtDiscPersen1Revisi.EditValue / 100)) * (1 - (txtDiscPersen2Revisi.EditValue / 100)) * (1 - (txtDiscPersen3Revisi.EditValue / 100))), 0) - txtDiscRp1Revisi.EditValue - txtDiscRp2Revisi.EditValue - txtDiscRp3Revisi.EditValue)
            txtSelisihTotalRevisi.EditValue = txtSelisihPcsRevisi.EditValue * txtQty.EditValue

        Catch ex As Exception

        End Try
    End Sub

    Private Sub txtQty_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtQty.LostFocus
        RubahSatuan()
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
    Private Sub txtDisc1_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtDiscRp1.LostFocus
        HitungJumlah()
    End Sub
    Private Sub txtDisc2_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtDiscRp2.LostFocus
        HitungJumlah()
    End Sub
    Private Sub txtDisc3_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtDiscRp3.LostFocus
        HitungJumlah()
    End Sub

    Private Sub txtBarang_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtBarang.EditValueChanged
        Dim Ds As New DataSet
        Try
            SQL = "SELECT MBarang.Nama AS NamaBarang FROM MBarang WHERE MBarang.NoID=" & NullTolong(txtBarang.EditValue)
            Ds = ExecuteDataset("Tabel", SQL)
            If Ds.Tables("Tabel").Rows.Count >= 1 Then
                'txtSatuan.EditValue = NullTolong(Ds.Tables(0).Rows(0).Item("IDSatuan"))
                txtNamaStock.Text = NullToStr(Ds.Tables(0).Rows(0).Item("NamaBarang"))
                'txtKonversi.EditValue = NullToDbl(Ds.Tables(0).Rows(0).Item("Konversi"))
                RefreshLookUpSatuan()
                txtSatuan.EditValue = DefIDSatuan
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
            HitungJumlah()
        Catch ex As Exception
            XtraMessageBox.Show("Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            Ds.Dispose()
        End Try
    End Sub

    'Private Sub txtBeli_ButtonClick(ByVal sender As Object, ByVal e As DevExpress.XtraEditors.Controls.ButtonPressedEventArgs) Handles txtBeli.ButtonClick
    '    With gvBeli
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

    Private Sub txtBeli_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtBeli.EditValueChanged
        Dim Ds As New DataSet
        Dim dsRevisiHarga As New DataSet
        Try
            SQL = "SELECT MBeliD.*, MBeli.Tanggal AS TglLama, MBeli.Kode AS KodeBeli, MAlamat.Nama AS NamaSupplier, MBarang.Kode AS KodeBarang, MBarang.Nama AS NamaBarang, MGudang.Kode AS KodeGudang, MGudang.Nama AS NamaGudang, MSatuan.Nama AS NamaSatuan "
            SQL &= " FROM (((MBeliD LEFT JOIN MGudang ON MGudang.NoID=MBeliD.IDGudang) "
            SQL &= " LEFT JOIN MBarang ON MBarang.NoID=MBeliD.IDBarang)"
            SQL &= " LEFT JOIN (MBeli LEFT JOIN MAlamat ON MAlamat.NoID=MBeli.IDSupplier) ON MBeli.NoID=MBeliD.IDBeli)"
            SQL &= " LEFT JOIN MSatuan ON MSatuan.NoID=MBeliD.IDSatuan "
            SQL &= " WHERE MBeliD.NoID= " & NullTolong(txtBeli.EditValue)
            Ds = New DataSet()
            Ds = ExecuteDataset("MBeliD", SQL)
            If Ds.Tables(0).Rows.Count >= 1 Then
                With Ds.Tables("MBeliD").Rows(0)
                    IDBeli = NullTolong(.Item("IDBeli"))
                    txtBarang.EditValue = NullTolong(.Item("IDBarang"))
                    txtSatuan.EditValue = NullTolong(.Item("IDSatuan"))
                    txtGudang.EditValue = NullTolong(.Item("IDGudang"))
                    txtTgl.DateTime = NullToDate(.Item("TglLama"))
                    Konversi = IIf(NullToDbl(.Item("Konversi")) = 0, 1, NullToDbl(.Item("Konversi")))
                    HargaPcs = NullToDbl(.Item("Harga")) / Konversi
                    txtSupplier.Text = NullTostr(.Item("NamaSupplier"))
                    txtQty.EditValue = NullToDbl(.Item("Qty"))
                    txtCtn.EditValue = NullToDbl(.Item("CTN"))

                    dsRevisiHarga = ExecuteDataset("MRevisiHargaBeli", "SELECT TOP 1 MRevisiHargaBeliD.*, MRevisiHargaBeli.Kode AS NoReff FROM MRevisiHargaBeliD INNER JOIN MRevisiHargaBeli ON MRevisiHargaBeli.NoID=MRevisiHargaBeliD.IDRevisiHargaBeli WHERE MRevisiHargaBeli.IsPosted=1 AND MRevisiHargaBeliD.IDBeliD=" & NullToLong(txtBeli.EditValue) & " ORDER BY MRevisiHargaBeliD.NoID DESC")
                    If dsRevisiHarga.Tables(0).Rows.Count >= 1 Then
                        txtHarga.EditValue = NullToDbl(dsRevisiHarga.Tables("MRevisiHargaBeli").Rows(0).Item("HargaBaru"))
                        txtHargaRevisi.EditValue = NullToDbl(dsRevisiHarga.Tables(0).Rows(0).Item("HargaBaru"))
                        txtDiscPersen1.EditValue = NullToDbl(dsRevisiHarga.Tables(0).Rows(0).Item("DiscPersen1Baru"))
                        txtDiscPersen1Revisi.EditValue = NullToDbl(dsRevisiHarga.Tables(0).Rows(0).Item("DiscPersen1Baru"))
                        txtDiscPersen2.EditValue = NullToDbl(dsRevisiHarga.Tables(0).Rows(0).Item("DiscPersen2Baru"))
                        txtDiscPersen2Revisi.EditValue = NullToDbl(dsRevisiHarga.Tables(0).Rows(0).Item("DiscPersen2Baru"))
                        txtDiscPersen3.EditValue = NullToDbl(dsRevisiHarga.Tables(0).Rows(0).Item("DiscPersen3Baru"))
                        txtDiscPersen3.EditValue = NullToDbl(dsRevisiHarga.Tables(0).Rows(0).Item("DiscPersen3Baru"))
                        txtDiscRp1.EditValue = NullToDbl(dsRevisiHarga.Tables(0).Rows(0).Item("Disc1Baru"))
                        txtDiscRp1Revisi.EditValue = NullToDbl(dsRevisiHarga.Tables(0).Rows(0).Item("Disc1Baru"))
                        txtDiscRp2.EditValue = NullToDbl(dsRevisiHarga.Tables(0).Rows(0).Item("Disc2Baru"))
                        txtDiscRp2Revisi.EditValue = NullToDbl(dsRevisiHarga.Tables(0).Rows(0).Item("Disc2Baru"))
                        txtDiscRp3.EditValue = NullToDbl(dsRevisiHarga.Tables(0).Rows(0).Item("Disc3Baru"))
                        txtDiscRp3Revisi.EditValue = NullToDbl(dsRevisiHarga.Tables(0).Rows(0).Item("Disc3Baru"))
                        txtJUmlah.EditValue = NullToDbl(dsRevisiHarga.Tables(0).Rows(0).Item("JumlahBaru"))
                        txtJumlahRevisi.EditValue = NullToDbl(dsRevisiHarga.Tables(0).Rows(0).Item("JumlahBaru"))
                        txtKodeReff.Text = NullToStr(dsRevisiHarga.Tables(0).Rows(0).Item("NoReff"))
                    Else
                        txtHarga.EditValue = NullToDbl(.Item("Harga"))
                        txtHargaRevisi.EditValue = NullToDbl(.Item("Harga"))
                        txtDiscPersen1.EditValue = NullToDbl(.Item("DiscPersen1"))
                        txtDiscPersen1Revisi.EditValue = NullToDbl(.Item("DiscPersen1"))
                        txtDiscPersen2.EditValue = NullToDbl(.Item("DiscPersen2"))
                        txtDiscPersen2Revisi.EditValue = NullToDbl(.Item("DiscPersen2"))
                        txtDiscPersen3.EditValue = NullToDbl(.Item("DiscPersen3"))
                        txtDiscPersen3.EditValue = NullToDbl(.Item("DiscPersen3"))
                        txtDiscRp1.EditValue = NullToDbl(.Item("Disc1"))
                        txtDiscRp1Revisi.EditValue = NullToDbl(.Item("Disc1"))
                        txtDiscRp2.EditValue = NullToDbl(.Item("Disc2"))
                        txtDiscRp2Revisi.EditValue = NullToDbl(.Item("Disc2"))
                        txtDiscRp3.EditValue = NullToDbl(.Item("Disc3"))
                        txtDiscRp3Revisi.EditValue = NullToDbl(.Item("Disc3"))
                        txtJUmlah.EditValue = NullToDbl(.Item("Jumlah"))
                        txtJumlahRevisi.EditValue = NullToDbl(.Item("Jumlah"))
                        txtKodeReff.Text = ""
                    End If
                    txtCatatan.Text = NullToStr(.Item("Catatan"))
                    txtKonversi.EditValue = Konversi
                    HitungJumlah()
                End With
            Else
                IsiDefault()
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

    'Private Sub txtBeli_EditValueChanging(ByVal sender As Object, ByVal e As DevExpress.XtraEditors.Controls.ChangingEventArgs) Handles txtBeli.EditValueChanging
    '    If System.IO.File.Exists(folderLayouts &  Me.Name & gvBeli.Name & ".xml") Then
    '        gvBeli.RestoreLayoutFromXml(folderLayouts &  Me.Name & gvBeli.Name & ".xml")
    '    End If
    '    With gvBeli
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

    Private Sub gvBeli_DataSourceChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvBeli.DataSourceChanged
        If System.IO.File.Exists(folderLayouts & Me.Name & gvBeli.Name & ".xml") Then
            gvBeli.RestoreLayoutFromXml(folderLayouts & Me.Name & gvBeli.Name & ".xml")
        End If
        With gvBeli
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
        Catch ex As Exception
            XtraMessageBox.Show("Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            Ds.Dispose()
        End Try
    End Sub

    Private Sub txtQty_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtQty.EditValueChanged

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

    Private Sub txtHargaRevisi_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtHargaRevisi.EditValueChanged

    End Sub

    Private Sub txtHargaRevisi_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtHargaRevisi.LostFocus
        HitungJumlah()
    End Sub

    Private Sub txtDiscPersen1Revisi_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtDiscPersen1Revisi.EditValueChanged
        HitungJumlah()
    End Sub
    Private Sub txtDiscPersen2Revisi_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtDiscPersen2Revisi.EditValueChanged
        HitungJumlah()
    End Sub
    Private Sub txtDiscPersen3Revisi_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtDiscPersen3Revisi.EditValueChanged
        HitungJumlah()
    End Sub

    Private Sub txtDiscRp1Revisi_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtDiscRp1Revisi.EditValueChanged
        HitungJumlah()
    End Sub
    Private Sub txtDiscRp2Revisi_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtDiscRp2Revisi.EditValueChanged
        HitungJumlah()
    End Sub
    Private Sub txtDiscRp3Revisi_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtDiscRp3Revisi.EditValueChanged
        HitungJumlah()
    End Sub

    Private Sub TglDari_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TglDari.EditValueChanged
        RefreshLookUp()
    End Sub

    Private Sub tglSampai_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tglSampai.EditValueChanged
        RefreshLookUp()
    End Sub
End Class