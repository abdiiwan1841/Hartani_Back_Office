Imports System.Data
Imports System.Data.SqlClient
Imports DevExpress.XtraEditors
Imports DevExpress.Utils
Imports System.Xml

Public Class frmEntriPOD
    Public IsNew As Boolean = True
    Public IsPosted As Boolean = False
    Public NoID As Long = -1
    Public IDPO As Long = -1
    Public IDSupplier As Long = -1
    Public IDWilayah As Long = -1
    Public formPemanggil As frmEntriPO

    Dim oDS As New DataSet
    Dim SQL As String = ""
    Dim DefImageList As ImageList
    Public IsAlias As Boolean = False
    Public IsFastEntri As Boolean = False
    Public IDTypePPN As Integer = 0 '0=Non,1=Include,2=Exclude
    Public frParent As New XtraForm

    Dim HargaBeli As Double, HargaJual As Double, HargaSupplier As Double

    Private Sub IsiDefault()
        DefImageList = frmMain.ImageList1
        cmdSave.ImageList = DefImageList
        cmdSave.ImageIndex = 6

        cmdTutup.ImageList = DefImageList
        cmdTutup.ImageIndex = 3

        txtKonversi.EditValue = 1
        RefreshLookUp()
        txtWilayah.EditValue = IDWilayah
        TglDari.DateTime = formPemanggil.Tgl.DateTime.AddDays(-30)
        TglSampai.DateTime = formPemanggil.Tgl.DateTime
    End Sub
    Private Sub RefreshLookUp()
        Dim ds As New DataSet
        Try
            SQL = "SELECT MWilayah.NoID, MWilayah.Kode, MWilayah.Nama FROM MWilayah WHERE MWilayah.IsActive=1 "
            ds = ExecuteDataset("MGudang", SQL)
            txtWilayah.Properties.DataSource = ds.Tables("MGudang")
            txtWilayah.Properties.ValueMember = "NoID"
            txtWilayah.Properties.DisplayMember = "Nama"

            SQL = "SELECT MBarangD.NoID,MBarangD.Barcode,MBarang.Kode,MBarang.Nama + ' ' + MBarangD.Varian AS Nama,MSatuan.Kode as Satuan,MBarangD.IDSatuan,MBarangD.IDBarang FROM MBarangD inner join MBarang On MBarangD.IDBarang=MBarang.NoID inner join MSatuan On MBarangD.IDSatuan=MSatuan.NoID WHERE MBarang.IsActive=1 AND MBarangD.IsActive=1 "
            If IDSupplier >= 1 AndAlso NullToBool(EksekusiSQlSkalarNew("SELECT IsStockPerSupplier FROM MSETTING")) Then
                SQL &= " AND (MBarang.IDSupplier1=" & IDSupplier
                SQL &= " OR MBarang.IDSupplier2=" & IDSupplier
                SQL &= " OR MBarang.IDSupplier3=" & IDSupplier
                SQL &= " OR MBarang.IDSupplier4=" & IDSupplier
                SQL &= " OR MBarang.IDSupplier5=" & IDSupplier & ")"
            End If
            ds = ExecuteDataset("MBarangD", SQL)
            txtBarangD.Properties.DataSource = ds.Tables("MBarangD")
            txtBarangD.Properties.ValueMember = "NoID"
            txtBarangD.Properties.DisplayMember = "Barcode"

            SQL = "SELECT MBarang.NoID, MBarang.Kode, MBarang.KodeAlias AS KodeAlias, MBarang.Nama, MBarang.NamaAlias, MSatuan.Nama AS Satuan FROM MBarang LEFT JOIN MSatuan ON MSatuan.NoID=MBarang.IDSatuan WHERE MBarang.IsActive=1 AND " & IIf(IsAlias, " NOT (MBarang.Alias='' OR MBarang.Alias IS NULL)", " NOT (MBarang.KODE='' OR MBarang.KODE IS NULL)")
            If IDSupplier >= 1 AndAlso NullToBool(EksekusiSQlSkalarNew("SELECT IsStockPerSupplier FROM MSETTING")) Then
                SQL &= " AND (MBarang.IDSupplier1=" & IDSupplier
                SQL &= " OR MBarang.IDSupplier2=" & IDSupplier
                SQL &= " OR MBarang.IDSupplier3=" & IDSupplier
                SQL &= " OR MBarang.IDSupplier4=" & IDSupplier
                SQL &= " OR MBarang.IDSupplier5=" & IDSupplier & ")"
            End If
            ds = ExecuteDataset("MBarang", SQL)
            txtBarang.Properties.DataSource = ds.Tables("MBarang")
            txtBarang.Properties.ValueMember = "NoID"

            SQL = "SELECT MTargetSupplier.NoID, Malamat.Kode AS Supplier, REPLACE(CONVERT(Varchar, MTargetSupplier.TglDari, 103),'/','-') + ' s/d ' + REPLACE(CONVERT(Varchar, MTargetSupplier.TglSampai, 103),'/','-') AS Peride, MTargetSupplier.TglDari AS DariTanggal, MTargetSupplier.TglSampai AS SampaiTanggal, MTargetSupplier.[Target]" & vbCrLf & _
                  " FROM MTargetSupplier " & _
                  " INNER JOIN MAlamat ON MAlamat.NoID=MTargetSupplier.IDSupplier" & vbCrLf & _
                  " WHERE MTargetSupplier.IDSupplier=" & IDSupplier
            ds = ExecuteDataset("MBarang", SQL)
            txtTargetSupplier.Properties.DataSource = ds.Tables("MBarang")
            txtTargetSupplier.Properties.ValueMember = "NoID"
            txtTargetSupplier.Properties.DisplayMember = "Peride"

            'Set Target
            SQL &= vbCrLf & _
                   " AND '" & formPemanggil.Tgl.DateTime.ToString("yyyy/MM/dd") & "'>=MTargetSupplier.TglDari AND '" & formPemanggil.Tgl.DateTime.AddDays(1).ToString("yyyy/MM/dd") & "'<MTargetSupplier.TGlSampai "
            txtTargetSupplier.EditValue = NullToLong(EksekusiSQlSkalarNew(SQL))

            If IsAlias Then
                txtBarang.Properties.DisplayMember = "KodeAlias"
            Else
                txtBarang.Properties.DisplayMember = "Kode"
            End If
        Catch ex As Exception
            XtraMessageBox.Show("Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            ds.Dispose()
        End Try
    End Sub
    Private Sub RefreshLookUpSatuan()
        Dim ds As New DataSet
        Try
            'diambil dari MBarangD
            'SQL = "SELECT MSatuan.NoID, MSatuan.Kode, MSatuan.Nama,MbarangD.Konversi AS Isi FROM MSatuan left join mbarangd on mbarangd.idsatuan=msatuan.noid WHERE MSatuan.IsActive=1 AND MSatuan.NoID IN (SELECT MBarangD.IDSatuan FROM MBarangD WHERE  MBarangD.IsActive=1 AND MBarangD.IDBarang=" & NullToLong(txtBarang.EditValue) & ") and (MBarangD.IDBarang=" & NullToLong(txtBarang.EditValue) & ") "
            'Diambil dari MBarang, antara IDSatuan atau IDSatuanHarga-->DIpakai satuan harga beli
            '
            SQL = "Select distinct * from ("
            SQL = SQL & "SELECT MSatuan.NoID, MSatuan.Kode, MSatuan.Nama,Mbarang.CtnPcs AS Isi FROM MSatuan inner join mbarang on MBarang.IDSatuanHarga=Msatuan.NoID where MBarang.NoID=" & NullToLong(txtBarang.EditValue) & " "
            SQL = SQL & " UNION ALL SELECT MSatuan.NoID, MSatuan.Kode, MSatuan.Nama,1.0 AS Isi FROM MSatuan inner join mbarang on MBarang.IDSatuan=Msatuan.NoID where MBarang.NoID=" & NullToLong(txtBarang.EditValue)
            SQL = SQL & " UNION ALL SELECT MSatuan.NoID, MSatuan.Kode, MSatuan.Nama,MBarangd.Konversi AS Isi FROM MSatuan inner join mbarangd on MBarangd.IDSatuan=Msatuan.NoID where MBarangd.IDBarang=" & NullToLong(txtBarang.EditValue) & " ) X "
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
    Private Sub LoadData()
        Try
            SQL = "SELECT MPOD.*, MBarang.Kode AS KodeBarang, MBarang.Nama AS NamaBarang, MGudang.Kode AS KodeGudang, MGudang.Nama AS NamaGudang, MSatuan.Nama AS NamaSatuan " & _
                  " FROM ((MPOD LEFT JOIN MGudang ON MGudang.NoID=MPOD.IDGudang) " & _
                  " LEFT JOIN MBarang ON MBarang.NoID=MPOD.IDBarang)" & _
                  " LEFT JOIN MSatuan ON MSatuan.NoID=MPOD.IDSatuan " & _
                  " WHERE MPOD.NoID= " & NoID
            oDS = New DataSet()
            oDS = ExecuteDataset("MPOD", SQL)
            If oDS.Tables(0).Rows.Count >= 1 Then
                With oDS.Tables("MPOD").Rows(0)
                    NoID = NullToLong(.Item("NoID"))
                    IDPO = NullToLong(.Item("IDPO"))

                    txtBarangD.EditValue = NullToLong(.Item("IDBarangD"))
                    txtBarang.EditValue = NullToLong(.Item("IDBarang"))
                    'txtBarang.Text = NullTostr(.Item("KodeBarang"))
                    txtSatuan.EditValue = NullToLong(.Item("IDSatuan"))
                    'txtSatuan.Text = NullTostr(.Item("KodeSatuan"))
                    txtWilayah.EditValue = IDWilayah
                    'txtGudang.Text = NullTostr(.Item("KodeGudang"))
                    txtKonversi.EditValue = NullToDbl(.Item("Konversi"))
                    txtQty.EditValue = NullToDbl(.Item("Qty"))
                    txtHargaSupplier.EditValue = NullToDbl(.Item("Harga"))
                    HargaSupplier = txtHargaSupplier.EditValue / IIf(NullToDbl(.Item("Konversi")) = 0, 1, NullToDbl(.Item("Konversi")))
                    txtDiscPersen1.EditValue = NullToDbl(.Item("DiscPersen1"))
                    txtDiscPersen2.EditValue = NullToDbl(.Item("DiscPersen2"))
                    txtDiscPersen3.EditValue = NullToDbl(.Item("DiscPersen3"))
                    txtDiscRp1.EditValue = NullToDbl(.Item("Disc1"))
                    txtTotalBeli.EditValue = NullToDbl(.Item("Jumlah"))
                    txtCatatan.Text = NullToStr(.Item("Catatan"))
                    txtHargaBeli.EditValue = NullToDbl(.Item("HargaPcs"))
                    HargaBeli = txtHargaBeli.EditValue
                    txtHargaJual.EditValue = NullToDbl(.Item("HargaJualPcs"))
                    HargaJual = txtHargaJual.EditValue
                    HitungJumlah()
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

                SQL = "INSERT INTO MPOD (NoID,IDPO,NoUrut,Tgl,Jam,IDBarang,IDBarangD,IDSatuan,Qty,QtyPcs,Harga,HargaPcs,DiscPersen1,DiscPersen2,DiscPersen3,Disc1,Jumlah,Catatan,IDWilayah,Konversi,HargaNetto,HargaJualPcs) VALUES ("
                SQL &= NullToLong(GetNewID("MPOD", "NoID")) & ","
                SQL &= IDPO & ","
                SQL &= GetNewID("MPOD", "NoUrut", " WHERE IDPO=" & IDPO) & ","
                SQL &= "GetDate(),"
                SQL &= "GetDate(),"
                SQL &= NullToLong(txtBarang.EditValue) & ","
                SQL &= NullToLong(txtBarangD.EditValue) & ","
                SQL &= NullToLong(txtSatuan.EditValue) & ","
                SQL &= FixKoma(txtQty.EditValue) & ","
                SQL &= FixKoma(txtQty.EditValue * txtKonversi.EditValue) & ","
                SQL &= FixKoma(txtHargaSupplier.EditValue) & ","
                SQL &= FixKoma(txtHargaBeli.EditValue) & ","
                SQL &= FixKoma(txtDiscPersen1.EditValue) & ","
                SQL &= FixKoma(txtDiscPersen2.EditValue) & ","
                SQL &= FixKoma(txtDiscPersen3.EditValue) & ","
                SQL &= FixKoma(txtDiscRp1.EditValue) & ","
                SQL &= FixKoma(txtTotalBeli.EditValue) & ","
                SQL &= "'" & FixApostropi(txtCatatan.Text) & "',"
                SQL &= NullToLong(txtWilayah.EditValue) & ","
                SQL &= FixKoma(txtKonversi.EditValue) & ","
                SQL &= FixKoma(txtHargaBeli.EditValue) & ","
                SQL &= FixKoma(NullToDbl(txtHargaJual.EditValue))
                SQL &= ")"
            Else
                SQL = "UPDATE MPOD SET "
                SQL &= " Tgl=GetDate(),"
                SQL &= " Jam=GetDate(),"
                SQL &= " IDPO=" & IDPO & ","
                SQL &= " IDBarang=" & NullToLong(txtBarang.EditValue) & ","
                SQL &= " IDBarangD=" & NullToLong(txtBarangD.EditValue) & ","
                SQL &= " IDSatuan=" & NullToLong(txtSatuan.EditValue) & ","
                SQL &= " Qty=" & FixKoma(txtQty.EditValue) & ","
                SQL &= " QtyPcs=" & FixKoma(txtQty.EditValue * txtKonversi.EditValue) & ","
                SQL &= " Harga=" & FixKoma(txtHargaSupplier.EditValue) & ","
                SQL &= " HargaPcs=" & FixKoma(txtHargaBeli.EditValue) & ","
                SQL &= " DiscPersen1=" & FixKoma(txtDiscPersen1.EditValue) & ","
                SQL &= " DiscPersen2=" & FixKoma(txtDiscPersen2.EditValue) & ","
                SQL &= " DiscPersen3=" & FixKoma(txtDiscPersen3.EditValue) & ","
                SQL &= " Disc1=" & FixKoma(txtDiscRp1.EditValue) & ","
                SQL &= " Jumlah=" & FixKoma(txtTotalBeli.EditValue) & ","
                SQL &= " Catatan='" & FixApostropi(txtCatatan.Text) & "',"
                SQL &= " IDWilayah=" & NullToLong(txtWilayah.EditValue) & ","
                SQL &= " Konversi=" & FixKoma(txtKonversi.EditValue) & ","
                SQL &= " HargaNetto=" & FixKoma(txtHargaBeli.EditValue) & ","
                SQL &= " HargaJualPcs=" & FixKoma(txtHargaJual.EditValue)
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
        If txtBarangD.Text = "" Then
            XtraMessageBox.Show("Barcode / Kode masih kosong.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
            txtBarangD.Focus()
            Return False
            Exit Function
        End If
        If TextEdit4.EditValue + (txtQty.EditValue * txtKonversi.EditValue) > TextEdit3.EditValue Then ' Qty Max
            XtraMessageBox.Show("Qty melebihi standart maksimum stock.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Warning)
            txtQty.Focus()
            Return False
            Exit Function
        End If
        If txtBarang.Text = "" Then
            XtraMessageBox.Show("Barang masih kosong.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
            txtBarang.Focus()
            Return False
            Exit Function
        End If
        If NullToBool(EksekusiSQlSkalarNew("SELECT MPO.IsPosted FROM MPO WHERE MPO.NoID=" & IDPO)) Then 'Nota DiPosting
            XtraMessageBox.Show("Nota Sudah Di Posting/Lock.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
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
        If txtWilayah.Text = "" Then
            XtraMessageBox.Show("Wilayah masih kosong.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
            txtWilayah.Focus()
            Return False
            Exit Function
        End If
        If txtQty.EditValue <= 0 Then
            XtraMessageBox.Show("Qty masih kurang dari atau sama dengan 0.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
            txtQty.Focus()
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
        ElseIf txtDiscPersen1.EditValue > 100 Then
            XtraMessageBox.Show("Disc persen 1 tidak boleh lebih dari 100.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
            txtDiscPersen1.Focus()
            Return False
            Exit Function
        End If
        If txtDiscPersen2.EditValue < 0 Then
            XtraMessageBox.Show("Disc persen 1 tidak boleh kurang dari 0.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
            txtDiscPersen2.Focus()
            Return False
            Exit Function
        ElseIf txtDiscPersen2.EditValue > 100 Then
            XtraMessageBox.Show("Disc persen 2 tidak boleh lebih dari 100.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
            txtDiscPersen2.Focus()
            Return False
            Exit Function
        End If
        If txtDiscPersen3.EditValue < 0 Then
            XtraMessageBox.Show("Disc persen 1 tidak boleh kurang dari 0.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
            txtDiscPersen2.Focus()
            Return False
            Exit Function
        ElseIf txtDiscPersen3.EditValue > 100 Then
            XtraMessageBox.Show("Disc persen 3 tidak boleh lebih dari 100.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
            txtDiscPersen3.Focus()
            Return False
            Exit Function
        End If
        If txtDiscRp1.EditValue < 0 Then
            XtraMessageBox.Show("Disc rupiah 1 tidak boleh kurang dari 0.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
            txtDiscRp1.Focus()
            Return False
            Exit Function
        ElseIf txtDiscRp1.EditValue > txtHargaSupplier.EditValue Then
            XtraMessageBox.Show("Disc rupiah 1 tidak boleh lebih dari harga.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
            txtDiscRp1.Focus()
            Return False
            Exit Function
        End If
        txtTotalBeli.EditValue = txtQty.EditValue * (txtHargaBeli.EditValue)
        'If txtTotalBeli.EditValue > txtHargaSupplier.EditValue * txtQty.EditValue Then
        '    XtraMessageBox.Show("Total discount melebihi harga.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
        '    txtHargaSupplier.Focus()
        '    Return False
        '    Exit Function
        'End If

        'If txtHargaSupplier.EditValue <> HargaSupplier Or txtHargaBeli.EditValue <> HargaBeli Or txtHargaJual.EditValue <> HargaJual Then
        '    Dim x As New frmOtorisasiAdmin
        '    If Not x.ShowDialog(Me)= Windows.Forms.DialogResult.OK Then
        '        XtraMessageBox.Show("Harga tidak sesuai dengan master, masukkan Password untuk merubah harga.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
        '        txtHargaSupplier.Focus()
        '        x.Dispose()
        '        Return False
        '        Exit Function
        '    Else
        '        x.Dispose()
        '    End If
        'End If

        If txtHargaSupplier.EditValue <= 0 Then
            If txtHargaSupplier.EditValue = 0 AndAlso XtraMessageBox.Show("Harga masih 0, lanjutkan penyimpanan?", NamaAplikasi, MessageBoxButtons.YesNo, MessageBoxIcon.Information, MessageBoxDefaultButton.Button2) = Windows.Forms.DialogResult.No Then
                txtHargaSupplier.Focus()
                Return False
                Exit Function
            ElseIf txtHargaSupplier.EditValue < 0 Then
                XtraMessageBox.Show("Harga masih kurang dari 0.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
                txtHargaSupplier.Focus()
                Return False
                Exit Function
            End If
        End If
        If IsAdaItemDiTransaksi(NullToLong(txtBarang.EditValue)) Then
            If XtraMessageBox.Show("Item sudah dientri." & vbCrLf & "Cek terlebih dahulu, Untuk melanjutkan tekan Yes atau No untuk membatalkan.", NamaAplikasi, MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) = Windows.Forms.DialogResult.No Then
                txtBarangD.Focus()
                Return False
                Exit Function
            End If
        End If
        Return True
    End Function
    Private Function IsAdaItemDiTransaksi(ByVal IDBarang As Long) As Boolean
        Dim x As Boolean = False
        Try
            SQL = "SELECT COUNT(MPOD.NoID) FROM MPOD WHERE MPOD.NoID<>" & NoID & " AND MPOD.IDBarang=" & IDBarang & " AND MPOD.IDPO=" & IDPO
            If NullToLong(EksekusiSQlSkalarNew(SQL)) >= 1 Then
                x = True
            Else
                x = False
            End If
        Catch ex As Exception
            XtraMessageBox.Show("Info Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
            x = False
        End Try
        Return x
    End Function
    Private Sub cmdTutup_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdTutup.Click
        DialogResult = Windows.Forms.DialogResult.Cancel
        Me.Close()
        'me.dispose()
    End Sub

    Private Sub BarButtonItem1_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles cmdSaveLayout.ItemClick
        Dim xOtorisasi As New frmOtorisasiAdmin
        Try
            If XtraMessageBox.Show("Simpan Layout?", NamaAplikasi, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = Windows.Forms.DialogResult.Yes AndAlso xOtorisasi.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
                Ini.TulisIniPath(FolderLayouts & Me.Name & ".ini", "Form", "Width", Me.Width)
                Ini.TulisIniPath(FolderLayouts & Me.Name & ".ini", "Form", "Height", Me.Height)
                Me.Left = (Screen.PrimaryScreen.WorkingArea.Width - Me.Width) \ 2
                Me.Top = (Screen.PrimaryScreen.WorkingArea.Height - Me.Height) \ 2

                LayoutControl1.SaveLayoutToXml(FolderLayouts & Me.Name & LayoutControl1.Name & ".xml")
                gvBarang.SaveLayoutToXml(FolderLayouts & Me.Name & gvBarang.Name & ".xml")
                gvBarangD.SaveLayoutToXml(FolderLayouts & Me.Name & gvBarangD.Name & ".xml")
                gvWilayah.SaveLayoutToXml(FolderLayouts & Me.Name & gvWilayah.Name & ".xml")
                gvSatuan.SaveLayoutToXml(FolderLayouts & Me.Name & gvSatuan.Name & ".xml")
            End If
        Catch ex As Exception
            XtraMessageBox.Show("Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            xOtorisasi.Dispose()
        End Try
    End Sub

    Private Sub frmEntriPOD_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        Try
            If DialogResult = Windows.Forms.DialogResult.OK Then
                formPemanggil.RefreshDetil()
                formPemanggil.Show()
                formPemanggil.Focus()
                formPemanggil.SimpanTambahan()
                formPemanggil.txtBarang.Focus()
            Else
                If IsFastEntri Then
                    EksekusiSQL("DELETE FROM MPOD WHERE NoID=" & NoID)
                End If
                formPemanggil.Show()
                formPemanggil.Focus()
            End If
            VPOINT.Forms.EntriPOD.anInstance = Nothing
            'VPOINT.Forms.EntriPOD.anInstance.Dispose()
        Catch ex As Exception

        End Try
    End Sub

    Private Sub frmEntriPOD_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
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
            Me.Width = Ini.BacaIniPath(FolderLayouts & Me.Name & ".ini", "Form", "Width", Me.Width)
            Me.Height = Ini.BacaIniPath(FolderLayouts & Me.Name & ".ini", "Form", "Height", Me.Height)

            Me.Left = (Screen.PrimaryScreen.WorkingArea.Width - Me.Width) \ 2
            Me.Top = (Screen.PrimaryScreen.WorkingArea.Height - Me.Height) \ 2

            If System.IO.File.Exists(FolderLayouts & Me.Name & LayoutControl1.Name & ".xml") Then
                LayoutControl1.RestoreLayoutFromXml(FolderLayouts & Me.Name & LayoutControl1.Name & ".xml")
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


    Private Sub HitungJumlah()
        Dim ds As New DataSet
        Try
            txtHargaBeli.EditValue = IIf(IDTypePPN = 2, 1.1, 1.0) * (Bulatkan(txtHargaSupplier.EditValue * (1 - (txtDiscPersen1.EditValue / 100)) * (1 - (txtDiscPersen2.EditValue / 100)) * (1 - (txtDiscPersen3.EditValue / 100)), 2) - txtDiscRp1.EditValue) / IIf(NullToDbl(txtKonversi.EditValue) = 0, 1, NullToDbl(txtKonversi.EditValue))
            'txtHargaBeli.EditValue = IIf(IDTypePPN = 2, 1.1, 1.0) * txtHargaBeli.EditValue / IIf(NullToDbl(txtKonversi.EditValue) = 0, 1, NullToDbl(txtKonversi.EditValue))
            txtTotalBeli.EditValue = IIf(IDTypePPN = 2, 1.1, 1.0) * (Bulatkan(txtHargaSupplier.EditValue * (1 - (txtDiscPersen1.EditValue / 100)) * (1 - (txtDiscPersen2.EditValue / 100)) * (1 - (txtDiscPersen3.EditValue / 100)), 2) - txtDiscRp1.EditValue) * txtQty.EditValue
            'HargaJualPcs = Bulatkan((txtHarga.EditValue * (1 - (txtDiscPersen1.EditValue / 100)) * (1 - (txtDiscPersen2.EditValue / 100)) * (1 - (txtDiscPersen3.EditValue / 100))), 0) - txtDiscRp1.EditValue - txtDiscRp2.EditValue - txtDiscRp3.EditValue
            'HargaJualPcs = HargaJualPcs + (HargaJualPcs * (NullToDbl(EksekusiSQlSkalarNew("SELECT PROSENUP FROM MBarang WHERE NoID=" & NullToLong(txtBarang.EditValue))) / 100))
            'txtHargaJual.EditValue = HargaJualPcs
            txtTotalHargaJual.EditValue = txtHargaJual.EditValue * txtQty.EditValue * txtKonversi.EditValue

            'Hitung Target
            SQL = "SELECT MTargetSupplier.NoID, Malamat.Kode AS Supplier, REPLACE(CONVERT(Varchar, MTargetSupplier.TglDari, 103),'/','-') + ' s/d ' + REPLACE(CONVERT(Varchar, MTargetSupplier.TglSampai, 103),'/','-') AS Peride, MTargetSupplier.TglDari AS DariTanggal, MTargetSupplier.TglSampai AS SampaiTanggal, MTargetSupplier.[Target], " & vbCrLf & _
                  " (SELECT SUM(MPOD.Jumlah) FROM MPOD INNER JOIN MPO ON MPOD.IDPO=MPO.NoID WHERE MPOD.NoID<>" & NoID & " AND MPO.IDSupplier=MTargetSupplier.IDSupplier AND MPO.Tanggal>=MTargetSupplier.TglDari AND MPO.Tanggal<=MTargetSupplier.TglSampai) AS TargetTerpenuhi, " & vbCrLf & _
                  " IsNull(MTargetSupplier.[Target],0)-IsNull((SELECT SUM(MPOD.Jumlah) FROM MPOD INNER JOIN MPO ON MPOD.IDPO=MPO.NoID WHERE MPOD.NoID<>" & NoID & " AND MPO.IDSupplier=MTargetSupplier.IDSupplier AND MPO.Tanggal>=MTargetSupplier.TglDari AND MPO.Tanggal<=MTargetSupplier.TglSampai),0) AS SisaTarget " & vbCrLf & _
                  " FROM MTargetSupplier  " & vbCrLf & _
                  " INNER JOIN MAlamat ON MAlamat.NoID=MTargetSupplier.IDSupplier" & vbCrLf & _
                  " WHERE MTargetSupplier.NoID=" & NullToLong(txtTargetSupplier.EditValue)
            ds = ExecuteDataset("MPOD", SQL)
            If ds.Tables("MPOD").Rows.Count >= 1 Then
                txtTarget.EditValue = NullToDbl(ds.Tables("MPOD").Rows(0).Item("Target"))
                txtTargetTerpenuhi.EditValue = NullToDbl(ds.Tables("MPOD").Rows(0).Item("TargetTerpenuhi"))
                txtSisaTarget.EditValue = NullToDbl(ds.Tables("MPOD").Rows(0).Item("SisaTarget"))
            Else
                txtTarget.EditValue = 0
                txtTargetTerpenuhi.EditValue = 0
                txtSisaTarget.EditValue = 0
            End If
        Catch ex As Exception

        Finally
            If Not ds Is Nothing Then
                ds.Dispose()
            End If
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
    Private Sub txtDisc1_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtDiscRp1.LostFocus
        HitungJumlah()
    End Sub
    Private Sub txtDisc2_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        HitungJumlah()
    End Sub
    Private Sub txtDisc3_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        HitungJumlah()
    End Sub

    Private Sub txtBarang_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtBarang.EditValueChanged
        Dim Ds As New DataSet
        Try
            SQL = "SELECT MBarang.Nama AS NamaBarang, MBarang.NamaAlias, MBarang.IsPPN,MBarang.HargaJual FROM MBarang WHERE MBarang.NoID=" & NullToLong(txtBarang.EditValue)
            Ds = ExecuteDataset("Tabel", SQL)
            If Ds.Tables("Tabel").Rows.Count >= 1 Then
                'txtSatuan.EditValue = NullTolong(Ds.Tables(0).Rows(0).Item("IDSatuan"))
                'If IsAlias Then
                txtNamaAlias.Text = NullToStr(Ds.Tables(0).Rows(0).Item("NamaAlias"))
                'Else
                txtNamaStock.Text = NullToStr(Ds.Tables(0).Rows(0).Item("NamaBarang"))
                'End If
                'txtKonversi.EditValue = NullToDbl(Ds.Tables(0).Rows(0).Item("Konversi"))
                txtHargaJual.EditValue = NullToBool(Ds.Tables(0).Rows(0).Item("HargaJual"))
                HargaJual = txtHargaJual.EditValue
                RefreshLookUpSatuan()
                txtSatuan.EditValue = DefIDSatuan
                PerhitunganEOQ(NullToLong(txtBarang.EditValue), NullToLong(txtBarangD.EditValue))
                HitungQtyPenjualan()
                'RubahSatuan()
            End If
        Catch ex As Exception
            XtraMessageBox.Show("Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            Ds.Dispose()
        End Try
    End Sub
    Private Sub PerhitunganEOQ(ByVal IDBarang As Long, ByVal IDBarangD As Long)
        Dim SQL As String
        Try
            SQL = "SELECT StockMax FROM MBarang WHERE NoID=" & IDBarang
            TextEdit3.EditValue = NullToDbl(EksekusiSQlSkalarNew(SQL))

            SQL = "SELECT StockMin FROM MBarang WHERE NoID=" & IDBarang
            TextEdit1.EditValue = NullToDbl(EksekusiSQlSkalarNew(SQL))

            SQL = "SELECT TOP 1 MJual.Tanggal FROM MJual INNER JOIN MJualD ON MJual.NoID=MJualD.IDJual WHERE MJualD.IDBarang=" & IDBarang & " ORDER BY MJual.Tanggal DESC"
            Tgl.DateTime = NullToDate(EksekusiSQlSkalarNew(SQL))

            SQL = "SELECT AVG(MJualD.Qty*MJualD.Konversi) FROM MJual INNER JOIN MJualD ON MJual.NoID=MJualD.IDJual WHERE MJualD.IDBarang=9 AND MJual.Tanggal>='" & TanggalSystem.Date.AddDays(-30).ToString("yyyy/MM/dd") & "' AND MJual.Tanggal<'" & TanggalSystem.Date.AddDays(1).ToString("yyyy/MM/dd") & "'"
            TextEdit5.EditValue = NullToDbl(EksekusiSQlSkalarNew(SQL))

            SQL = "SELECT TOP 1 DATEDIFF(D, MBeli.Tanggal, MPO.Tanggal) AS LeadTime" & _
                  " FROM MPO " & _
                  " INNER JOIN MPOD ON MPOD.IDPO=MPO.NoID" & _
                  " INNER JOIN (MBeliD INNER JOIN MBeli ON MBeli.NoID=MBeliD.IDBeli) ON MBeliD.IDBeli=MBeli.NoID" & _
                  " WHERE MPO.IDBarang=" & IDBarang & _
                  " ORDER BY MPO.Tanggal DESC"
            TextEdit9.EditValue = NullToDbl(EksekusiSQlSkalarNew(SQL))

            SQL = "SELECT TOP 1 DATEDIFF(D, MPO.Tanggal, MBeli.Tanggal) AS LeadTime" & _
                  " FROM MPO " & _
                  " INNER JOIN MPOD ON MPOD.IDPO=MPO.NoID" & _
                  " INNER JOIN (MBeliD INNER JOIN MBeli ON MBeli.NoID=MBeliD.IDBeli) ON MBeliD.IDPOD=MPOD.NoID" & _
                  " WHERE MPOD.IDBarang=" & IDBarang & _
                  " ORDER BY MPO.Tanggal DESC"
            TextEdit9.EditValue = NullToDbl(EksekusiSQlSkalarNew(SQL))

            SQL = "SELECT TOP 1 DATEDIFF(D, IsNull((SELECT TOP 1 MPO.Tanggal FROM MPOD INNER JOIN MPO ON MPO.NoID=MPOD.IDPO WHERE MPOD.IDBarang=MPODNew.IDBarang AND MPOD.NoID<MPODNew.NoID ORDER BY MPO.Tanggal DESC),MPONew.Tanggal), MPONew.Tanggal) AS LeadTime" & _
                  " FROM MPO MPONew " & _
                  " INNER JOIN MPOD MPODNew ON MPODNew.IDPO=MPONew.NoID" & _
                  " WHERE MPODNew.IDBarang = " & IDBarang & _
                  " ORDER BY MPONew.Tanggal DESC"
            TextEdit6.EditValue = NullToDbl(EksekusiSQlSkalarNew(SQL))

            SQL = "SELECT SUM((MKartuStok.QtyMasuk-MKartuStok.QtyKeluar)*MKartuStok.Konversi) FROM MKartuStok WHERE MKartuStok.IDBarangD=" & IDBarangD & " AND MKartuStok.IDBarang=" & IDBarang
            TextEdit4.EditValue = NullToDbl(EksekusiSQlSkalarNew(SQL))

            TextEdit8.EditValue = TextEdit5.EditValue * (TextEdit9.EditValue + TextEdit6.EditValue) - TextEdit4.EditValue - TextEdit1.EditValue
        Catch ex As Exception
            XtraMessageBox.Show("Info Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
        End Try
    End Sub
    Private Sub txtSatuan_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtSatuan.EditValueChanged
        RubahSatuan()
    End Sub
    Private Sub RubahSatuan()
        Dim Ds As New DataSet
        Dim IsiKarton As Double = 0.0
        Try
            'gvSatuan.FocusedRowHandle = gvSatuan.LocateByDisplayText(-1, gvSatuan.Columns("NoID"), NullToLong(txtSatuan.EditValue).ToString("###,##0"))
            'txtKonversi.EditValue = NullToDbl(gvSatuan.GetFocusedRowCellValue("Isi"))
            SQL = "select Isi From( Select distinct * from ("
            SQL = SQL & "SELECT MSatuan.NoID, MSatuan.Kode, MSatuan.Nama,Mbarang.CtnPcs AS Isi FROM MSatuan inner join mbarang on MBarang.IDSatuanHarga=Msatuan.NoID where MBarang.NoID=" & NullToLong(txtBarang.EditValue) & " "
            SQL = SQL & " UNION ALL SELECT MSatuan.NoID, MSatuan.Kode, MSatuan.Nama,1.0 AS Isi FROM MSatuan inner join mbarang on MBarang.IDSatuan=Msatuan.NoID where MBarang.NoID=" & NullToLong(txtBarang.EditValue)
            SQL = SQL & " UNION ALL SELECT MSatuan.NoID, MSatuan.Kode, MSatuan.Nama,MBarangd.Konversi AS Isi FROM MSatuan inner join mbarangd on MBarangd.IDSatuan=Msatuan.NoID where MBarangd.IDBarang=" & NullToLong(txtBarang.EditValue) & " ) X  ) Z where NoID=" & NullToLong(txtSatuan.EditValue)

            txtKonversi.EditValue = NullToDbl(EksekusiSQlSkalarNew(SQL))

            'txtKonversi.EditValue = NullToDbl(EksekusiSQlSkalarNew("SELECT Isi FROM (" & _
            '                        "SELECT MSatuan.NoID, MSatuan.Kode, MSatuan.Nama,Mbarang.CtnPcs AS Isi FROM MSatuan inner join mbarang on MBarang.IDSatuanHarga=Msatuan.NoID where MBarang.NoID=" & NullToLong(txtBarang.EditValue) & " " & _
            '                        " UNION ALL SELECT MSatuan.NoID, MSatuan.Kode, MSatuan.Nama,1.0 AS Isi FROM MSatuan inner join mbarang on MBarang.IDSatuan=Msatuan.NoID where MBarang.NoID=" & NullToLong(txtBarang.EditValue) & " ) X where NoID=" & NullToLong(txtSatuan.EditValue)))
            'Ds = ExecuteDataset("MSatuan", SQL)

            SQL = "SELECT (MBarang.HargaBeliPcsBruto) AS HargaBeliPcsBruto, (MBarang.HargaBeli) AS HargaBeli,MBarang.HargaJual,MBarang.CtnPcs,MBarang.DiscBeli1,MBarang.DiscBeli2,MBarang.DiscBeli3,Mbarang.DiscRp From Mbarang where NoID=" & NullToLong(txtBarang.EditValue)
            Ds = ExecuteDataset("TabelBarang", SQL)
            If Ds.Tables("TabelBarang").Rows.Count >= 1 Then
                'Konversi 1 dianggap harga PCs
                'If NullToDbl(txtKonversi.EditValue) = 1.0 Then 'NullToDbl(Ds.Tables("TabelBarang").Rows(0).Item("CtnPcs")) Then
                txtHargaSupplier.EditValue = NullToDbl(Ds.Tables("TabelBarang").Rows(0).Item("HargaBeliPcsBruto")) * NullToDbl(txtKonversi.EditValue)
                'Else
                '    txtHargaSupplier.EditValue = NullToDbl(Ds.Tables("TabelBarang").Rows(0).Item("HargaBeli")) / IIf(txtKonversi.EditValue = 0, 1, txtKonversi.EditValue)
                'End If
                HargaSupplier = txtHargaSupplier.EditValue / IIf(NullToDbl(txtKonversi.EditValue) = 0, 1, NullToDbl(txtKonversi.EditValue))
                txtDiscPersen1.EditValue = NullToDbl(Ds.Tables("TabelBarang").Rows(0).Item("DiscBeli1"))
                txtDiscPersen2.EditValue = NullToDbl(Ds.Tables("TabelBarang").Rows(0).Item("DiscBeli2"))
                txtDiscPersen3.EditValue = NullToDbl(Ds.Tables("TabelBarang").Rows(0).Item("DiscBeli3"))
                txtDiscRp1.EditValue = NullToDbl(Ds.Tables("TabelBarang").Rows(0).Item("DiscRp"))
                txtHargaJual.EditValue = NullToDbl(Ds.Tables("TabelBarang").Rows(0).Item("HargaJual"))
                HargaJual = txtHargaJual.EditValue
            End If
            HitungJumlah()
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

    Private Sub gvWilayah_DataSourceChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvWilayah.DataSourceChanged
        If System.IO.File.Exists(FolderLayouts & Me.Name & gvWilayah.Name & ".xml") Then
            gvWilayah.RestoreLayoutFromXml(FolderLayouts & Me.Name & gvWilayah.Name & ".xml")
        End If
        With gvWilayah
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

    Private Sub gvBarang_DataSourceChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvBarang.DataSourceChanged
        If System.IO.File.Exists(FolderLayouts & Me.Name & gvBarang.Name & ".xml") Then
            gvBarang.RestoreLayoutFromXml(FolderLayouts & Me.Name & gvBarang.Name & ".xml")
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
        If System.IO.File.Exists(FolderLayouts & Me.Name & gvSatuan.Name & ".xml") Then
            gvSatuan.RestoreLayoutFromXml(FolderLayouts & Me.Name & gvSatuan.Name & ".xml")
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

    Private Sub txtQty_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtQty.EditValueChanged
        HitungJumlah()
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

    Private Sub txtHarga_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtHargaSupplier.LostFocus
        'txtDisc1_LostFocus(sender, e)
        'txtDisc2_LostFocus(sender, e)
        'txtDisc3_LostFocus(sender, e)
        'txtDiscPersen1_LostFocus(sender, e)
        'txtDiscPersen2_LostFocus(sender, e)
        'txtDiscPersen3_LostFocus(sender, e)
        HitungJumlah()
    End Sub

    Private Sub txtCtn_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)

    End Sub

    Private Sub txtCtn_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        HitungJumlah()
    End Sub

    Private Sub txtSatuan_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtSatuan.LostFocus
        RubahSatuan()
    End Sub

    Private Sub txtDiscPersen1_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtDiscPersen1.EditValueChanged
        HitungJumlah()
    End Sub
    Private Sub txtDiscPersen2_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtDiscPersen2.EditValueChanged
        HitungJumlah()
    End Sub
    Private Sub txtDiscPersen3_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtDiscPersen3.EditValueChanged
        HitungJumlah()
    End Sub

    Private Sub txtHarga_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtHargaSupplier.EditValueChanged
        HitungJumlah()
    End Sub
 

    Private Sub txtDiscRp1_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtDiscRp1.EditValueChanged
        HitungJumlah()
    End Sub
    Private Sub txtDiscRp2_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        HitungJumlah()
    End Sub
    Private Sub txtDiscRp3_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        HitungJumlah()
    End Sub

    Private Sub txtHargaJual_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtHargaJual.EditValueChanged
        HitungJumlah()
    End Sub

    Private Sub ckPPN_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        HitungJumlah()
    End Sub

    Private Sub txtBarangD_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtBarangD.EditValueChanged
        Dim Ds As New DataSet
        Try
            SQL = "SELECT MBarangD.NoID,MBarangD.Varian,MBarangD.IDBarang,MBarangD.IDSatuan,MBarang.Kode,MBarang.IDSatuan IDSatuanBase,MBarang.Nama AS NamaBarang, MBarang.NamaAlias " & _
                  "FROM MBarangD inner join MBarang On MBarangD.IDbarang=MBarang.NoID WHERE MBarangD.NoID=" & NullToLong(txtBarangD.EditValue)
            Ds = ExecuteDataset("Tabel", SQL)
            If Ds.Tables("Tabel").Rows.Count >= 1 Then
                txtBarang.EditValue = NullToLong(Ds.Tables(0).Rows(0).Item("IDBarang"))
                txtNamaStock.Text = NullToStr(Ds.Tables(0).Rows(0).Item("NamaBarang"))
                txtNamaAlias.Text = NullToStr(Ds.Tables(0).Rows(0).Item("NamaAlias"))
                RefreshLookUpSatuan()
                txtSatuan.EditValue = NullToLong(Ds.Tables(0).Rows(0).Item("IDSatuan")) 'DefIDSatuan
                RubahSatuan()
            End If
        Catch ex As Exception
            XtraMessageBox.Show("Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            'RefreshLookUpSTB()
            Ds.Dispose()
        End Try
    End Sub

    Private Sub gvBarangD_DataSourceChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvBarangD.DataSourceChanged
        If System.IO.File.Exists(FolderLayouts & Me.Name & gvBarangD.Name & ".xml") Then
            gvBarangD.RestoreLayoutFromXml(FolderLayouts & Me.Name & gvBarangD.Name & ".xml")
        End If
        With gvBarangD
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

    Private Sub LabelControl1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LabelControl1.Click

    End Sub

    Private Sub txtKonversi_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtKonversi.EditValueChanged
        'Dim kon As Double = txtKonversi.EditValue
        'RubahSatuan()
        'txtKonversi.EditValue = kon
        'txtHargaSupplier.EditValue = HargaSupplier / IIf(txtKonversi.EditValue = 0, 1, txtKonversi.EditValue)
        txtHargaSupplier.EditValue = HargaSupplier * txtKonversi.EditValue
        HitungJumlah()
    End Sub

    Private Sub txtTotalPenjualan_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtTotalPenjualan.EditValueChanged
        HitungQtyPenjualan()
    End Sub

    Private Sub HitungQtyPenjualan()
        Try
            txtTotalPenjualan.EditValue = NullToDbl(EksekusiSQlSkalarNew("SELECT SUM(MJualD.Qty*MJualD.Konversi) FROM MJual INNER JOIN MJualD ON MJualD.IDJual=MJual.NoID WHERE MJual.Tanggal>='" & TglDari.DateTime.ToString("yyyy/MM/dd") & "' AND MJual.Tanggal<'" & TglSampai.DateTime.AddDays(1).ToString("yyyy/MM/dd") & "' AND MJualD.IDBarang=" & NullToLong(txtBarang.EditValue) & " AND MJualD.IDBarangD=" & NullToLong(txtBarangD.EditValue)))
            txtQty.EditValue = (txtTotalPenjualan.EditValue - TextEdit1.EditValue - TextEdit4.EditValue) / IIf(txtKonversi.EditValue = 0, 1, txtKonversi.EditValue)
        Catch ex As Exception
            XtraMessageBox.Show("Info Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK)
        End Try
    End Sub

    Private Sub TglDari_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TglDari.EditValueChanged
        HitungQtyPenjualan()
    End Sub

    Private Sub TglSampai_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TglSampai.EditValueChanged
        HitungQtyPenjualan()
    End Sub

    Private Sub txtTargetSupplier_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtTargetSupplier.EditValueChanged
        HitungJumlah()
    End Sub
End Class