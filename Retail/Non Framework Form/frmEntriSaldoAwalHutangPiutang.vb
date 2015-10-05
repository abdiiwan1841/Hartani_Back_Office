Imports System.Data
Imports System.Data.SqlClient
Imports DevExpress.XtraEditors
Imports DevExpress.Utils
Imports DevExpress.XtraGrid.Views.Base

Public Class frmEntriSaldoAwalHutangPiutang
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
    Dim DefImageList As New ImageList

    ''For Back Action
    'Public FormNameDaftar As String = ""
    'Public TableNameDaftar As String = ""
    'Public TextDaftar As String = ""
    'Public FormEntriDaftar As String = ""
    'Public TableMasterDaftar As String = ""
    'Dim IsTempInsertBaru As Boolean = False
    ''Dim frmEntri As frmEntriBeliD = VPOINT.Forms.EntriBeliD.Instance

    Private Sub RefreshDataKontak()
        Dim ds As New DataSet
        Try
            SQL = "SELECT NoID, Kode, Nama FROM MAlamat WHERE IsActive=1 AND IsSupplier=1"
            ds = ExecuteDataset("master", SQL)
            txtKodeSupplier.Properties.DataSource = ds.Tables("master")
            If System.IO.File.Exists(folderLayouts & Me.Name & SearchLookUpEdit1View.Name & ".xml") Then
                SearchLookUpEdit1View.RestoreLayoutFromXml(folderLayouts & Me.Name & SearchLookUpEdit1View.Name & ".xml")
            End If
        Catch ex As Exception
            XtraMessageBox.Show("Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            ds.Dispose()
        End Try
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

            FungsiControl.SetForm(Me)
            Me.Text = "Entri Saldo Akhir Supplier"
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
    Private Sub RefreshData()
        Dim DS As New DataSet
        Try
            RefreshDataKontak()
            If pTipe = pStatus.Baru Then
                IsiDefault()
            Else
                SQL = "SELECT MSaldoAwalHutangPiutang.*, MAlamat.Alamat AS Alamat, MAlamat.Nama AS NamaAlamat, MAlamat.Kode AS KodeAlamat FROM MSaldoAwalHutangPiutang LEFT JOIN MAlamat ON MAlamat.NoID=MSaldoAwalHutangPiutang.IDAlamat WHERE MSaldoAwalHutangPiutang.NoID=" & NoID
                DS = ExecuteDataset("Tabel", SQL)
                If DS.Tables("Tabel").Rows.Count >= 1 Then
                    txtKodeSupplier.EditValue = NullToLong(DS.Tables(0).Rows(0).Item("IDAlamat"))
                    txtNamaSupplier.Text = NullToStr(DS.Tables(0).Rows(0).Item("NamaAlamat"))
                    txtAlamatSupplier.Text = NullTostr(DS.Tables(0).Rows(0).Item("Alamat"))
                    txtKode.Text = NullTostr(DS.Tables(0).Rows(0).Item("Kode"))
                    KodeLama = txtKode.Text
                    txtKodeReff.Text = NullTostr(DS.Tables(0).Rows(0).Item("KodeReff"))
                    Tgl.DateTime = NullToDate(DS.Tables(0).Rows(0).Item("Tanggal"))
                    tglJatuhTempo.DateTime = NullToDate(DS.Tables(0).Rows(0).Item("JatuhTempo"))
                    txtSaldoAkhir.EditValue = NullToDbl(DS.Tables(0).Rows(0).Item("SaldoAkhir"))
                    txtSaldoAwal.EditValue = NullToDbl(DS.Tables(0).Rows(0).Item("SaldoAwal"))
                    txtTotal.EditValue = NullToDbl(DS.Tables(0).Rows(0).Item("Debet"))
                    IsPosted = NullTobool(DS.Tables(0).Rows(0).Item("IsPosted"))
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
            DS.Dispose()
        End Try
    End Sub

    Private Sub HitungTotal()
        Try
            txtTotal.EditValue = txtSaldoAwal.EditValue - txtSaldoAkhir.EditValue
        Catch ex As Exception
            XtraMessageBox.Show("Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub
    Private Sub IsiDefault()
        Tgl.DateTime = Date.Now
        tglJatuhTempo.DateTime = DateAdd(DateInterval.Month, 1, Tgl.DateTime)
        SetTombol()

        txtKodeSupplier.EditValue = DefIDSupplier
        RubahSupplier()
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


        cmdSave.ImageList = DefImageList
        cmdSave.ImageIndex = 6

        cmdTutup.ImageList = DefImageList
        cmdTutup.ImageIndex = 3

    End Sub

    Private Sub cmdSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSave.Click
        Dim PesanSalah As String = ""
        If Not IsPosted Then
            If IsValidasi() Then
                If Simpan(PesanSalah) = True Then
                    clsPostingPembelian.PostingSaldoAwalHutangPiutang(NoID)
                    DialogResult = Windows.Forms.DialogResult.OK
                    Close()
                    Dispose()
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
        If Tgl.Text = "" Then
            XtraMessageBox.Show("Tanggal masih kosong.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
            Tgl.Focus()
            Return False
            Exit Function
        End If
        HitungTotal()
        If pTipe = pStatus.Baru AndAlso txtKode.Properties.ReadOnly Then
            txtKode.Text = clsKode.MintaKodeBaru("SA", "MSaldoAwalHutangPiutang", Tgl.DateTime, , 5)
        End If
        If CekKodeValid(txtKode.Text, KodeLama, "MSaldoAwalHutangPiutang", "Kode", IIf(pTipe = pStatus.Edit, True, False)) Then
            XtraMessageBox.Show("Kode sudah dipakai.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
            txtKode.Focus()
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
                NoID = GetNewID("MSaldoAwalHutangPiutang")
                SQL = "INSERT INTO [MSaldoAwalHutangPiutang] ([NoID],[IDWilayah],[IDAlamat],[DebetA],[KreditA],[Debet],[Kredit],[Kurs],[IDUser],[Tanggal],[JatuhTempo],[IsPosted],[IDUserEntry],[IDUserPosting],[SaldoAwal],[SaldoAkhir],[Keterangan],[Kode],[KodeReff]) VALUES (" & vbCrLf & _
                      NoID & "," & vbCrLf & _
                      DefIDWilayah & "," & vbCrLf & _
                      NullToLong(txtKodeSupplier.EditValue) & "," & vbCrLf & _
                      FixKoma(txtTotal.EditValue) & ", 0, " & vbCrLf & _
                      FixKoma(txtTotal.EditValue) & ", 0, 1, " & vbCrLf & _
                      IDUserAktif & ", " & vbCrLf & _
                      "'" & Tgl.DateTime.ToString("yyyy-MM-dd HH:mm") & "'," & vbCrLf & _
                      "'" & tglJatuhTempo.DateTime.ToString("yyyy-MM-dd HH:mm") & "', 0, " & IDUserAktif & ", -1, " & vbCrLf & _
                      FixKoma(txtSaldoAwal.EditValue) & ", " & vbCrLf & _
                      FixKoma(txtSaldoAkhir.EditValue) & ", 'Saldo Awal', " & vbCrLf & _
                      "'" & FixApostropi(txtKode.Text) & "'," & vbCrLf & _
                      "'" & FixApostropi(txtKodeReff.Text) & "')"
            Else
                SQL = "UPDATE [MSaldoAwalHutangPiutang] SET " & vbCrLf & _
                      " IDAlamat=" & NullToLong(txtKodeSupplier.EditValue) & "," & vbCrLf & _
                      " Debet=" & FixKoma(txtTotal.EditValue) & ", " & vbCrLf & _
                      " DebetA=" & FixKoma(txtTotal.EditValue) & ", " & vbCrLf & _
                      " IDUser=" & IDUserAktif & ", " & vbCrLf & _
                      " Tanggal='" & Tgl.DateTime.ToString("yyyy-MM-dd HH:mm") & "'," & vbCrLf & _
                      " JatuhTempo='" & tglJatuhTempo.DateTime.ToString("yyyy-MM-dd HH:mm") & "', " & vbCrLf & _
                      " SaldoAwal=" & FixKoma(txtSaldoAwal.EditValue) & ", " & vbCrLf & _
                      " SaldoAkhir=" & FixKoma(txtSaldoAkhir.EditValue) & ", " & vbCrLf & _
                      " Kode='" & FixApostropi(txtKode.Text) & "'," & vbCrLf & _
                      " KodeReff='" & FixApostropi(txtKodeReff.Text) & "' " & vbCrLf & _
                      " WHERE NoID=" & NoID
            End If
            If EksekusiSQL(SQL) >= 1 Then
                Sukses = True
            Else
                Sukses = False
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

                LayoutControl1.SaveLayoutToXml(FolderLayouts & Me.Name & LayoutControl1.Name & ".xml")

                SearchLookUpEdit1View.SaveLayoutToXml(FolderLayouts & Me.Name & SearchLookUpEdit1View.Name & ".xml")
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

    Private Function HitungSaldoAkhir() As Double
        Dim Hasil As Double
        Dim IDGroupSupplier As Long = -1
        Try
            IDGroupSupplier = NullToLong(EksekusiSQlSkalarNew("SELECT MGroupSupplier.NoID FROM MGroupSupplier INNER JOIN MGroupSupplierD ON MGroupSupplier.NoID=MGroupSupplierD.IDGroupSupplier WHERE MGroupSupplier.IsActive=1 AND MGroupSupplierD.IDAlamat=" & NullToLong(txtKodeSupplier.EditValue)))
            SQL = "SELECT MSaldoAwalHutangPiutang.SaldoAwal-0 AS Total" & vbCrLf & _
                  " FROM MSaldoAwalHutangPiutang INNER JOIN MAlamat ON MAlamat.NoID=MSaldoAwalHutangPiutang.IDAlamat" & vbCrLf & _
                  " WHERE MSaldoAwalHutangPiutang.IsPosted=1 AND MAlamat.NoID IN (SELECT MGroupSupplierD.IDAlamat FROM MGroupSupplierD WHERE MGroupSupplierD.IDGroupSupplier=" & IDGroupSupplier & ") AND MSaldoAwalHutangPiutang.Tanggal<'" & Tgl.DateTime.AddDays(1).ToString("yyyy-MM-dd") & "'" & vbCrLf & _
                  " UNION ALL " & vbCrLf & _
                  " SELECT MBeli.Total-0 AS Total" & vbCrLf & _
                  " FROM MBeli INNER JOIN MAlamat ON MAlamat.NoID=MBeli.IDSupplier" & vbCrLf & _
                  " WHERE MBeli.IsPosted=1 AND MAlamat.NoID IN (SELECT MGroupSupplierD.IDAlamat FROM MGroupSupplierD WHERE MGroupSupplierD.IDGroupSupplier=" & IDGroupSupplier & ") AND MBeli.Tanggal>='2012-12-01' AND MBeli.Tanggal<'" & Tgl.DateTime.AddDays(1).ToString("yyyy-MM-dd") & "'" & vbCrLf & _
                  " UNION ALL" & vbCrLf & _
                  " SELECT 0-MReturBeli.Total" & vbCrLf & _
                  " FROM MReturBeli INNER JOIN MAlamat ON MAlamat.NoID=MReturBeli.IDSupplier" & vbCrLf & _
                  " INNER JOIN (MBayarHutang INNER JOIN MBayarHutangDRetur ON MBayarHutang.NoID=MBayarHutangDRetur.IDBayarHutang) ON MBayarHutangDRetur.IDReturBeli=MReturBeli.NoID" & vbCrLf & _
                  " WHERE MReturBeli.IsPosted=1 AND MAlamat.NoID IN (SELECT MGroupSupplierD.IDAlamat FROM MGroupSupplierD WHERE MGroupSupplierD.IDGroupSupplier=" & IDGroupSupplier & ") AND MBayarHutang.TglKembali>='2012-12-01' AND MBayarHutang.TglKembali<'" & Tgl.DateTime.AddDays(1).ToString("yyyy-MM-dd") & "'" & vbCrLf & _
                  " UNION ALL " & vbCrLf & _
                  " SELECT 0-MBayarHutang.Total AS Masuk" & vbCrLf & _
                  " FROM MBayarHutang INNER JOIN MAlamat ON MAlamat.NoID=MBayarHutang.IDAlamat" & vbCrLf & _
                  " WHERE MBayarHutang.IsPosted=1 AND MAlamat.NoID IN (SELECT MGroupSupplierD.IDAlamat FROM MGroupSupplierD WHERE MGroupSupplierD.IDGroupSupplier=" & IDGroupSupplier & ") AND MBayarHutang.TglKembali>='2012-12-01' AND MBayarHutang.TglKembali<'" & Tgl.DateTime.AddDays(1).ToString("yyyy-MM-dd") & "'" & vbCrLf & _
                  " UNION ALL" & vbCrLf & _
                  " SELECT MBayarHutangDDebet.Potong-0 AS Masuk" & vbCrLf & _
                  " FROM (MBayarHutang INNER JOIN MBayarHutangDDebet ON MBayarHutang.NoID=MBayarHutangDDebet.IDBayarHutang) INNER JOIN MAlamat ON MAlamat.NoID=MBayarHutang.IDAlamat" & vbCrLf & _
                  " WHERE MBayarHutang.IsPosted=1 AND MAlamat.NoID IN (SELECT MGroupSupplierD.IDAlamat FROM MGroupSupplierD WHERE MGroupSupplierD.IDGroupSupplier=" & IDGroupSupplier & ") AND MBayarHutang.TglKembali>='2012-12-01' AND MBayarHutang.TglKembali<'" & Tgl.DateTime.AddDays(1).ToString("yyyy-MM-dd") & "'" & vbCrLf & _
                  " UNION ALL" & vbCrLf & _
                  " SELECT 0-MBayarHutangDKredit.Potong AS Keluar" & vbCrLf & _
                  " FROM (MBayarHutang INNER JOIN MBayarHutangDKredit ON MBayarHutang.NoID=MBayarHutangDKredit.IDBayarHutang) INNER JOIN MAlamat ON MAlamat.NoID=MBayarHutang.IDAlamat" & vbCrLf & _
                  " WHERE MBayarHutang.IsPosted=1 AND MAlamat.NoID IN (SELECT MGroupSupplierD.IDAlamat FROM MGroupSupplierD WHERE MGroupSupplierD.IDGroupSupplier=" & IDGroupSupplier & ") AND MBayarHutang.TglKembali>='2012-12-01' AND MBayarHutang.TglKembali<'" & Tgl.DateTime.AddDays(1).ToString("yyyy-MM-dd") & "'" & vbCrLf & _
                  " UNION ALL" & vbCrLf & _
                  " SELECT 0-IsNull(MBayarHutang.Materai,0) AS Keluar" & vbCrLf & _
                  " FROM MBayarHutang INNER JOIN MAlamat ON MAlamat.NoID=MBayarHutang.IDAlamat" & vbCrLf & _
                  " WHERE MBayarHutang.IsPosted=1 AND MAlamat.NoID IN (SELECT MGroupSupplierD.IDAlamat FROM MGroupSupplierD WHERE MGroupSupplierD.IDGroupSupplier=" & IDGroupSupplier & ") AND MBayarHutang.TglKembali>='2012-12-01' AND MBayarHutang.TglKembali<'" & Tgl.DateTime.AddDays(1).ToString("yyyy-MM-dd") & "' AND IsNull(MBayarHutang.Materai,0)<>0" & vbCrLf & _
                  " UNION ALL" & vbCrLf & _
                  " SELECT 0-IsNull(MBayarHutang.JumlahKwitansi,0) AS Keluar" & vbCrLf & _
                  " FROM MBayarHutang INNER JOIN MAlamat ON MAlamat.NoID=MBayarHutang.IDAlamat" & vbCrLf & _
                  " WHERE MBayarHutang.IsPosted=1 AND MAlamat.NoID IN (SELECT MGroupSupplierD.IDAlamat FROM MGroupSupplierD WHERE MGroupSupplierD.IDGroupSupplier=" & IDGroupSupplier & ") AND MBayarHutang.TglKembali>='2012-12-01' AND MBayarHutang.TglKembali<'" & Tgl.DateTime.AddDays(1).ToString("yyyy-MM-dd") & "' AND IsNull(MBayarHutang.JumlahKwitansi,0)<>0" & vbCrLf
            SQL = "SELECT SUM(KartuHutang.Total) AS Saldo FROM (" & SQL & ") AS KartuHutang"
            Hasil = NullToDbl(EksekusiSQlSkalarNew(SQL))
        Catch ex As Exception
            XtraMessageBox.Show("Info Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
            Hasil = 0
        End Try
        Return Hasil
    End Function

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
            txtSaldoAkhir.EditValue = HitungSaldoAkhir()
        Catch ex As Exception

        End Try
    End Sub

    Private Sub Tgl_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Tgl.EditValueChanged
        Dim TglAdd As Long = 0
        Try
            TglAdd = NullTolong(EksekusiSQlSkalarNew("SELECT JatuhTempoSupplier FROM MAlamat WHERE NoID=" & NullTolong(txtKodeSupplier.EditValue)))
            If TglAdd = 0 Then
                tglJatuhTempo.DateTime = DateAdd(DateInterval.Month, 2, Tgl.DateTime)
            Else
                tglJatuhTempo.DateTime = DateAdd(DateInterval.Day, TglAdd, Tgl.DateTime)
            End If
            If pTipe = pStatus.Baru Then
                txtKode.Text = clsKode.MintaKodeBaru("SA", "MSaldoAwalHutangPiutang", Tgl.DateTime, , 5)
            End If
            txtSaldoAkhir.EditValue = HitungSaldoAkhir()
        Catch ex As Exception

        End Try
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
                        Tgl.Properties.ReadOnly = False
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
                        txtKode.Properties.ReadOnly = False
                    End If
                End If

            Catch ex As Exception
                XtraMessageBox.Show("Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        ElseIf e.KeyCode = Keys.Enter Then
            If pTipe = pStatus.Baru Then
                If NullToLong(EksekusiSQlSkalarNew("select NoID from MSaldoAwalHutangPiutang where Kode='" & FixApostropi(txtKode.Text) & "'")) > 0 Then
                    MsgBox("Kode Sudah Ada!", MsgBoxStyle.Exclamation)
                End If
            Else
                If NullToLong(EksekusiSQlSkalarNew("select NoID from MSaldoAwalHutangPiutang where Kode<>'" & FixApostropi(KodeLama) & " and Kode='" & FixApostropi(txtKode.Text) & "'")) > 0 Then
                    MsgBox("Kode Sudah Ada!", MsgBoxStyle.Exclamation)
                End If
            End If
        End If
    End Sub

    Private Sub SearchLookUpEdit1View_DataSourceChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles SearchLookUpEdit1View.DataSourceChanged
        If System.IO.File.Exists(FolderLayouts & Me.Name & SearchLookUpEdit1View.Name & ".xml") Then
            SearchLookUpEdit1View.RestoreLayoutFromXml(FolderLayouts & Me.Name & SearchLookUpEdit1View.Name & ".xml")
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

    Private Sub txtSaldoAwal_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtSaldoAwal.EditValueChanged
        HitungTotal()
    End Sub

    Private Sub txtSaldoAkhir_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtSaldoAkhir.EditValueChanged
        HitungTotal()
    End Sub
End Class