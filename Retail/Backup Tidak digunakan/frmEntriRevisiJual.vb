Imports System.Data
Imports System.Data.SqlClient
Imports DevExpress.XtraEditors
Imports DevExpress.Utils
Imports System.IO.File

Public Class frmEntriRevisiJual
    Public IsNew As Boolean = True
    Public IsPosted As Boolean = False
    Public NoID As Long = -1
    Dim oDS As New DataSet
    Dim SQL As String = ""
    Dim DefImageList As ImageList
    Dim HargaPcs As Double = 0.0
    Dim Konversi As Double = 0.0
    Dim IDAdmin As Long = -1
    Dim IDJual As Long = -1

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
        RefreshLookUp()
        Konversi = 1.0
        HargaPcs = 0.0
        txtGudang.EditValue = DefIDGudang
        Tgl.DateTime = TanggalSystem
    End Sub
    Private Sub RefreshLookUp()
        Dim ds As New DataSet
        Try
            SQL = "SELECT MJualD.NoID, MWilayah.Nama AS Wilayah, MGudang.Nama AS Gudang, MJual.Kode AS KodeJual, MBarang.Kode AS KodeStock, MJualD.Qty AS QtyJual, MSatuan.Nama AS SatuanJual, "
            SQL &= " (MJualD.Harga-isnull(MJualD.DISC1,0)-isnull(MJualD.DISC2,0)-isnull(MJualD.DISC3,0)"
            SQL &= " -(MJualD.Harga*isnull(MJualD.DiscPersen1,0)/100)"
            SQL &= " -(MJualD.Harga*isnull(MJualD.DiscPersen2,0)/100)"
            SQL &= " -(MJualD.Harga*isnull(MJualD.DiscPersen3,0)/100)"
            SQL &= " ) AS HargaJual, (MJualD.Qty*MJualD.Konversi) AS [Qty (Pcs)], MJualD.HargaPcs AS [Harga (Pcs)] "
            SQL &= " FROM MJualD "
            SQL &= " LEFT JOIN (MGudang LEFT JOIN MWilayah ON MWilayah.NoID=MGudang.IDWilayah) ON MGudang.NoID=MJualD.IDGudang "
            SQL &= " LEFT JOIN MJual ON MJual.NoID=MJualD.IDJual LEFT JOIN MBarang ON MBarang.NoID=MJualD.IDBarang LEFT JOIN MSatuan ON MSatuan.NoID=MJualD.IDSatuan "
            SQL &= " WHERE MWilayah.NoID=" & DefIDWilayah & " AND MBarang.IsActive=1 AND MJual.IsPosted=1"
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
            SQL = "SELECT MGudang.NoID, MGudang.Kode, MGudang.Nama, MWilayah.Nama AS Wilayah FROM MGudang LEFT JOIN MWilayah ON MWilayah.NoID=MGudang.IDWilayah WHERE MGudang.IDWilayah=" & DefIDWilayah & " AND MGudang.IsActive=1 "
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
            SQL = "SELECT MRevisiHargaJual.* FROM MRevisiHargaJual WHERE MRevisiHargaJual.NoID=" & NoID
            oDS = New DataSet()
            oDS = ExecuteDataset("MRevisiHargaJual", SQL)
            If oDS.Tables(0).Rows.Count >= 1 Then
                With oDS.Tables("MRevisiHargaJual").Rows(0)
                    NoID = NullTolong(.Item("NoID"))
                    txtJual.EditValue = NullTolong(.Item("IDJualD"))
                    IDJual = NullTolong(.Item("IDJual"))
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
                    txtCatatan.Text = NullTostr(.Item("Catatan"))
                    txtKonversi.EditValue = NullToDbl(.Item("Konversi"))
                    txtTgl.DateTime = NullToDate(.Item("TanggalJual"))

                    'Revisi
                    KodeLama = NullTostr(.Item("Kode"))
                    txtKode.Text = NullTostr(.Item("Kode"))
                    txtKodeReff.Text = NullTostr(.Item("KodeReff"))
                    Tgl.DateTime = NullToDate(.Item("Tanggal"))
                    txtHargaRevisi.EditValue = NullToDbl(.Item("HargaBaru"))
                    txtDiscPersen1Revisi.EditValue = NullToDbl(.Item("DiscPersen1Baru"))
                    txtDiscPersen2Revisi.EditValue = NullToDbl(.Item("DiscPersen2Baru"))
                    txtDiscPersen3Revisi.EditValue = NullToDbl(.Item("DiscPersen3Baru"))
                    txtDiscRp1Revisi.EditValue = NullToDbl(.Item("Disc1Baru"))
                    txtDiscRp2Revisi.EditValue = NullToDbl(.Item("Disc2Baru"))
                    txtDiscRp3Revisi.EditValue = NullToDbl(.Item("Disc3Baru"))
                    txtJumlahRevisi.EditValue = NullToDbl(.Item("JumlahBaru"))
                    txtSelisihPcsRevisi.EditValue = NullToDbl(.Item("Koreksi"))
                    txtCatatanRevisi.Text = NullTostr(.Item("Keterangan"))
                    HitungJumlah()
                    If NullTobool(.Item("IsPosted")) Then
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
                Close()
                Dispose()
            End If
        End If
    End Sub
    Private Function Simpan() As Boolean
        Try
            If IsNew Then
                SQL = "INSERT INTO MRevisiHargaJual ([NoID],[Kode],[KodeReff],[IDJualD]," & _
                      "[IDJual],[IDBarang],[Tanggal],[Keterangan],[IDSatuan],[IDGudang]," & _
                      "[Ctn],[Qty],[Harga],[RevisiHarga],[DiscPersen1],[DiscPersen2],[DiscPersen3]," & _
                      "[Disc1],[Disc2],[Disc3],[Jumlah],[IDPackingD],[IDSPKD],[IDAkunPersediaan],[Catatan]," & _
                      "[NoUrut],[HargaPcs],[QtyPcs],[IDAsal],[Konversi],[IDServerAsal]," & _
                      "[HargaBaru],[DiscPersen1Baru],[DiscPersen2Baru],[DiscPersen3Baru],[Disc1Baru],[Disc2Baru],[Disc3Baru],[JumlahBaru],[Koreksi],[IsPosted],[TanggalJual],[IDUserPosting],[TglPosting],[IDWilayah],[IDUserEntry],[IDUserEdit],[IDAdmin],[IDCustomer]) " & vbCrLf & _
                      " SELECT " & NullTolong(GetNewID("MRevisiHargaJual", "NoID")) & "," & vbCrLf & _
                      "'" & FixApostropi(txtKode.Text) & "'," & vbCrLf & _
                      "'" & FixApostropi(txtKodeReff.Text) & "'," & vbCrLf & _
                      "MJualD.NoID," & _
                      "MJualD.IDJual," & _
                      "MJualD.IDBarang," & _
                      "'" & Tgl.DateTime.ToString("yyyy-MM-dd HH:mm") & "'," & vbCrLf & _
                      "'" & FixApostropi(txtCatatanRevisi.Text) & "'," & _
                      "MJualD.IDSatuan," & _
                      "MJualD.IDGudang," & _
                      "MJualD.Ctn," & _
                      "MJualD.Qty," & _
                      "MJualD.Harga," & _
                      FixKoma(txtHargaRevisi.EditValue) & "," & vbCrLf & _
                      "MJualD.DiscPersen1," & _
                      "MJualD.DiscPersen2," & _
                      "MJualD.DiscPersen3," & _
                      "MJualD.Disc1," & _
                      "MJualD.Disc2," & _
                      "MJualD.Disc3," & _
                      "MJualD.Jumlah," & _
                      "MJualD.IDPackingD," & _
                      "MJualD.IDSPKD," & _
                      "MJualD.IDAkunPersediaan," & _
                      "MJualD.Catatan," & _
                      "MJualD.NoUrut," & _
                      "MJualD.HargaPcs,MJualD.QtyPcs,MJualD.IDAsal,MJualD.Konversi,MJualD.IDServerAsal," & _
                      FixKoma(txtHargaRevisi.EditValue) & "," & FixKoma(txtDiscPersen1Revisi.EditValue) & ", " & _
                      FixKoma(txtDiscPersen2Revisi.EditValue) & "," & FixKoma(txtDiscPersen3Revisi.EditValue) & "," & _
                      FixKoma(txtDiscRp2Revisi.EditValue) & "," & FixKoma(txtDiscRp2Revisi.EditValue) & "," & FixKoma(txtDiscRp3Revisi.EditValue) & "," & _
                      FixKoma(txtJumlahRevisi.EditValue) & "," & FixKoma(txtSelisihPcsRevisi.EditValue) & ",0,MJual.Tanggal,NULL,NULL," & DefIDWilayah & "," & IDUserAktif & ",NULL," & IDAdmin & ", MJual.IDCustomer " & _
                      " FROM MJualD LEFT JOIN MJual ON MJual.NoID=MJualD.IDJual WHERE MJualD.NoID=" & NullTolong(txtJual.EditValue)
            Else
                EksekusiSQL("UPDATE MRevisiHargaJual SET IDJualD=" & NullTolong(txtJual.EditValue) & ", IDJual=" & IDJual & " WHERE NoID=" & NoID)
                SQL = "UPDATE MRevisiHargaJual SET " & _
                      "Kode='" & FixApostropi(txtKode.Text) & "'," & vbCrLf & _
                      "KodeReff='" & FixApostropi(txtKodeReff.Text) & "'," & vbCrLf & _
                      "IDJualD=MJualD.NoID," & _
                      "IDJual=MJualD.IDJual," & _
                      "IDBarang=MJualD.IDBarang," & _
                      "IDCustomer=MJual.IDCustomer," & _
                      "Tanggal='" & Tgl.DateTime.ToString("yyyy-MM-dd HH:mm") & "'," & vbCrLf & _
                      "Keterangan='" & FixApostropi(txtCatatanRevisi.Text) & "'," & _
                      "IDSatuan=MJualD.IDSatuan," & _
                      "IDGudang=MJualD.IDGudang," & _
                      "Ctn=MJualD.Ctn," & _
                      "Qty=MJualD.Qty," & _
                      "Harga=MJualD.Harga," & _
                      "RevisiHarga=" & FixKoma(txtHargaRevisi.EditValue) & "," & vbCrLf & _
                      "DiscPersen1=MJualD.DiscPersen1," & _
                      "DiscPersen2=MJualD.DiscPersen2," & _
                      "DiscPersen3=MJualD.DiscPersen3," & _
                      "Disc1=MJualD.Disc1," & _
                      "Disc2=MJualD.Disc2," & _
                      "Disc3=MJualD.Disc3," & _
                      "Jumlah=MJualD.Jumlah," & _
                      "IDPackingD=MJualD.IDPackingD," & _
                      "IDSPKD=MJualD.IDSPKD," & _
                      "IDAkunPersediaan=MJualD.IDAkunPersediaan," & _
                      "Catatan=MJualD.Catatan," & _
                      "NoUrut=MJualD.NoUrut," & _
                      "HargaPcs=MJualD.HargaPcs,QtyPcs=MJualD.QtyPcs,IDAsal=MJualD.IDAsal,Konversi=MJualD.Konversi,IDServerAsal=MJualD.IDServerAsal," & _
                      "HargaBaru=" & FixKoma(txtHargaRevisi.EditValue) & ",DiscPersen1Baru=" & FixKoma(txtDiscPersen1Revisi.EditValue) & ", " & _
                      "DiscPersen2Baru=" & FixKoma(txtDiscPersen2Revisi.EditValue) & ",DiscPersen3Baru=" & FixKoma(txtDiscPersen3Revisi.EditValue) & "," & _
                      "Disc1Baru=" & FixKoma(txtDiscRp2Revisi.EditValue) & ",Disc2Baru=" & FixKoma(txtDiscRp2Revisi.EditValue) & ",Disc3Baru=" & FixKoma(txtDiscRp3Revisi.EditValue) & "," & _
                      "JumlahBaru=" & FixKoma(txtJumlahRevisi.EditValue) & ",Koreksi=" & FixKoma(txtSelisihPcsRevisi.EditValue) & ",TanggalJual=MJual.Tanggal,IDWilayah=" & DefIDWilayah & ",IDUserEdit=" & IDUserAktif & _
                      IIf(txtKode.Properties.ReadOnly = False Or txtTgl.Properties.ReadOnly = False, ", IDAdmin=" & IDAdmin, "") & _
                      " FROM (MRevisiHargaJual LEFT JOIN MJualD ON MJualD.NoID=MRevisiHargaJual.IDJualD) " & _
                      " LEFT JOIN MJual ON MJual.NoID=MJualD.IDJual WHERE MRevisiHargaJual.NoID=" & NoID
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
        If txtJual.Text = "" Then
            XtraMessageBox.Show("Faktur Penjualan masih kosong.", NamaAplikasi, MessageBoxButtons.YesNo, MessageBoxIcon.Information)
            txtJual.Focus()
            Return False
            Exit Function
        End If
        If txtKode.Text = "" Then
            XtraMessageBox.Show("Kode masih kosong.", NamaAplikasi, MessageBoxButtons.YesNo, MessageBoxIcon.Information)
            txtKode.Focus()
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
        If CekKodeValid(txtKode.Text, KodeLama, "MRevisiHargaJual", "Kode", Not IsNew) Then
            XtraMessageBox.Show("Kode sudah dipakai.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
            txtKode.Focus()
            Return False
            Exit Function
        End If
        Return True
    End Function
    Dim KodeLama As String = ""
    Private Sub cmdTutup_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdTutup.Click
        DialogResult = Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub BarButtonItem1_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles cmdSaveLayout.ItemClick
        Dim xOtorisasi As New frmOtorisasiAdmin
        Try
            If XtraMessageBox.Show("Simpan Layout?", NamaAplikasi, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = Windows.Forms.DialogResult.Yes AndAlso xOtorisasi.ShowDialog = Windows.Forms.DialogResult.OK Then
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
            Else
                txtKode.Text = clsKode.MintaKodeBaru("RHJL", "MRevisiHargaJual", Tgl.DateTime, DefIDWilayah, 5)
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
        Try
            Dim Harga As Double = 0.0
            If txtJual.Text <> "" Then
                Harga = IIf(HargaPcs = 0, txtHarga.EditValue, HargaPcs) * IIf(txtKonversi.EditValue = 0, 1, txtKonversi.EditValue)
            Else
                Harga = txtHarga.EditValue
            End If
            'Dim SubTotal As Double = (Harga - txtDiscRp1.EditValue - txtDiscRp2.EditValue - txtDiscRp3.EditValue)
            'Dim DiscA As Double = Harga * txtQty.EditValue * txtDiscPersen1.EditValue / 100
            'Dim DiscB As Double = Harga * txtQty.EditValue * txtDiscPersen2.EditValue / 100
            'Dim DiscC As Double = Harga * txtQty.EditValue * txtDiscPersen3.EditValue / 100
            txtHarga.EditValue = Harga
            'txtJUmlah.EditValue = (SubTotal * txtQty.EditValue) - DiscA - DiscB - DiscC
            txtJUmlah.EditValue = txtQty.EditValue * (Bulatkan((Harga * (1 - (txtDiscPersen1.EditValue / 100)) * (1 - (txtDiscPersen2.EditValue / 100)) * (1 - (txtDiscPersen3.EditValue / 100))), 0) - txtDiscRp1.EditValue - txtDiscRp2.EditValue - txtDiscRp3.EditValue)
            txtJumlahRevisi.EditValue = txtQty.EditValue * (Bulatkan((txtHargaRevisi.EditValue * (1 - (txtDiscPersen1Revisi.EditValue / 100)) * (1 - (txtDiscPersen2Revisi.EditValue / 100)) * (1 - (txtDiscPersen3Revisi.EditValue / 100))), 0) - txtDiscRp1.EditValue - txtDiscRp2.EditValue - txtDiscRp3.EditValue)
            txtSelisihPcsRevisi.EditValue = ((Harga - txtDiscRp1.EditValue - txtDiscRp2.EditValue - txtDiscRp3.EditValue) * (1 - (txtDiscPersen1.EditValue / 100)) * (1 - (txtDiscPersen2.EditValue / 100)) * (1 - (txtDiscPersen3.EditValue / 100))) - ((txtHargaRevisi.EditValue - txtDiscRp1Revisi.EditValue - txtDiscRp2Revisi.EditValue - txtDiscRp3Revisi.EditValue) * (1 - (txtDiscPersen1Revisi.EditValue / 100)) * (1 - (txtDiscPersen2Revisi.EditValue / 100)) * (1 - (txtDiscPersen3Revisi.EditValue / 100)))
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
                txtNamaStock.Text = NullTostr(Ds.Tables(0).Rows(0).Item("NamaBarang"))
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
            SQL = "SELECT MBarangD.Konversi, MBarang.Ctn_Pcs FROM MBarangD LEFT JOIN MBarang ON MBarang.NoID=MBarangD.IDBarang LEFT JOIN MSatuan ON MSatuan.NoID=MBarangD.IDSatuan WHERE MBarangD.IDBarang=" & NullTolong(txtBarang.EditValue) & " AND MSatuan.NoID=" & NullTolong(txtSatuan.EditValue)
            Ds = ExecuteDataset("Tabel", SQL)
            If Ds.Tables("Tabel").Rows.Count >= 1 Then
                txtKonversi.EditValue = NullToDbl(Ds.Tables(0).Rows(0).Item("Konversi"))
                IsiKarton = NullToDbl(Ds.Tables(0).Rows(0).Item("Ctn_Pcs"))
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
            SQL = "SELECT MJualD.*, MJual.Tanggal AS TglLama, MJual.Kode AS KodeJual, MAlamat.Nama AS NamaCustomer, MBarang.Kode AS KodeBarang, MBarang.Nama AS NamaBarang, MGudang.Kode AS KodeGudang, MGudang.Nama AS NamaGudang, MSatuan.Nama AS NamaSatuan "
            SQL &= " FROM (((MJualD LEFT JOIN MGudang ON MGudang.NoID=MJualD.IDGudang) "
            SQL &= " LEFT JOIN MBarang ON MBarang.NoID=MJualD.IDBarang)"
            SQL &= " LEFT JOIN (MJual LEFT JOIN MAlamat ON MAlamat.NoID=MJual.IDCustomer) ON MJual.NoID=MJualD.IDJual)"
            SQL &= " LEFT JOIN MSatuan ON MSatuan.NoID=MJualD.IDSatuan "
            SQL &= " WHERE MJualD.NoID= " & NullTolong(txtJual.EditValue)
            Ds = New DataSet()
            Ds = ExecuteDataset("MJualD", SQL)
            If Ds.Tables(0).Rows.Count >= 1 Then
                With Ds.Tables("MJualD").Rows(0)
                    IDJual = NullTolong(.Item("IDJual"))
                    txtBarang.EditValue = NullTolong(.Item("IDBarang"))
                    txtSatuan.EditValue = NullTolong(.Item("IDSatuan"))
                    txtGudang.EditValue = NullTolong(.Item("IDGudang"))
                    txtTgl.DateTime = NullToDate(.Item("TglLama"))
                    Konversi = IIf(NullToDbl(.Item("Konversi")) = 0, 1, NullToDbl(.Item("Konversi")))
                    HargaPcs = NullToDbl(.Item("Harga")) / Konversi
                    txtCustomer.Text = NullTostr(.Item("NamaCustomer"))
                    txtQty.EditValue = NullToDbl(.Item("Qty"))
                    txtHarga.EditValue = NullToDbl(.Item("Harga"))
                    txtHargaRevisi.EditValue = NullToDbl(.Item("Harga"))
                    txtCtn.EditValue = NullToDbl(.Item("CTN"))
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
                    txtCatatan.Text = NullTostr(.Item("Catatan"))
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
                    If x.ShowDialog = Windows.Forms.DialogResult.OK Then
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

    End Sub

    Private Sub txtDiscPersen3_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtDiscPersen3.EditValueChanged

    End Sub

    Private Sub Tgl_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Tgl.EditValueChanged
        Dim TglAdd As Long = 0
        Try
            If IsNew Then
                txtKode.Text = clsKode.MintaKodeBaru("RHJL", "MRevisiHargaJual", Tgl.DateTime, DefIDWilayah, 5)
            End If
        Catch ex As Exception

        End Try
    End Sub

    Private Sub Tgl_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Tgl.KeyDown
        If e.KeyCode = Keys.F7 Then
            Dim x As New frmOtorisasiAdmin
            Try
                If Tgl.Properties.ReadOnly Then
                    If x.ShowDialog = Windows.Forms.DialogResult.OK Then
                        IDAdmin = x.IDUserAdmin
                        Tgl.Properties.ReadOnly = False
                    End If
                End If
            Catch ex As Exception
                XtraMessageBox.Show("Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End If
    End Sub

    Private Sub txtKode_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtKode.EditValueChanged

    End Sub

    Private Sub txtKode_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtKode.KeyDown
        If e.KeyCode = Keys.F7 Then
            Dim x As New frmOtorisasiAdmin
            Try
                If Tgl.Properties.ReadOnly Then
                    If x.ShowDialog = Windows.Forms.DialogResult.OK Then
                        IDAdmin = x.IDUserAdmin
                        txtKode.Properties.ReadOnly = False
                    End If
                End If
            Catch ex As Exception
                XtraMessageBox.Show("Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End If
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
End Class