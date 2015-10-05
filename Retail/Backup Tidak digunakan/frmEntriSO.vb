Imports System.Data
Imports System.Data.SqlClient
Imports DevExpress.XtraEditors
Imports DevExpress.Utils
Imports DevExpress.XtraGrid.Views.Base

Public Class frmEntriSO
    Dim SQL As String = ""
    Public Enum pStatus
        Baru = 0
        Edit = 1
    End Enum
    Public pTipe As pStatus
    Public IsPosted As Boolean = False
    Public NoID As Long = -1
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
    Public IsSO As Boolean = True

    Private Sub RefreshDataKontak()
        Dim ds As New DataSet
        Try
            SQL = "SELECT NoID, Kode, Nama FROM MAlamat WHERE IsActive=1 AND IsCustomer=1"
            ds = ExecuteDataset("master", SQL)
            txtKodeCustomer.Properties.DataSource = ds.Tables("master")
            If System.IO.File.Exists(Application.StartupPath & "\system\layouts\" & Me.Name & SearchLookUpEdit1View.Name & ".xml") Then
                SearchLookUpEdit1View.RestoreLayoutFromXml(Application.StartupPath & "\system\layouts\" & Me.Name & SearchLookUpEdit1View.Name & ".xml")
            End If
        Catch ex As Exception
            XtraMessageBox.Show("Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            ds.Dispose()
        End Try
    End Sub
    Dim DefImageList As New ImageList

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
            Me.Width = Ini.BacaIniPath(Application.StartupPath & "\system\Layouts\" & Me.Name & ".ini", "Form", "Width", Me.Width)
            Me.Height = Ini.BacaIniPath(Application.StartupPath & "\system\Layouts\" & Me.Name & ".ini", "Form", "Height", Me.Height)

            Me.Left = (Screen.PrimaryScreen.WorkingArea.Width - Me.Width) \ 2
            Me.Top = (Screen.PrimaryScreen.WorkingArea.Height - Me.Height) \ 2

            If System.IO.File.Exists(Application.StartupPath & "\System\Layouts\" & Me.Name & LayoutControl1.Name & IsSO & ".xml") Then
                LayoutControl1.RestoreLayoutFromXml(Application.StartupPath & "\System\Layouts\" & Me.Name & LayoutControl1.Name & IsSO & ".xml")
            End If
            If System.IO.File.Exists(Application.StartupPath & "\System\Layouts\" & Me.Name & GV1.Name & IsSO & ".xml") Then
                GV1.RestoreLayoutFromXml(Application.StartupPath & "\System\Layouts\" & Me.Name & GV1.Name & IsSO & ".xml")
            End If
            If System.IO.File.Exists(Application.StartupPath & "\System\Layouts\" & Me.Name & SearchLookUpEdit1View.Name & IsSO & ".xml") Then
                SearchLookUpEdit1View.RestoreLayoutFromXml(Application.StartupPath & "\System\Layouts\" & Me.Name & SearchLookUpEdit1View.Name & IsSO & ".xml")
            End If
            If Not IsSO Then
                Me.Text = "Entri Permintaan Barang"
            End If
            FungsiControl.SetForm(Me)
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
                SQL = "SELECT MSO.*, MUserEntri.Nama AS UserEntri, MUserEdit.Nama AS UserEdit, MUserPosting.Nama AS UserPosting, MAlamat.Nama AS NamaAlamat, MAlamat.Kode as KodeAlamat "
                SQL &= " FROM MSO LEFT JOIN MAlamat ON MAlamat.NoID=MSO.IDCustomer "
                SQL &= " LEFT JOIN MUser MUserEntri ON MUserEntri.NoID=MSO.IDUserEntry "
                SQL &= " LEFT JOIN MUser MUserEdit ON MUserEdit.NoID=MSO.IDUserEdit "
                SQL &= " LEFT JOIN MUser MUserPosting ON MUserPosting.NoID=MSO.IDUserPosting "
                SQL &= " WHERE MSO.NoID = " & NoID
                DS = ExecuteDataset("Tabel", SQL)
                If DS.Tables("Tabel").Rows.Count >= 1 Then
                    txtKodeCustomer.EditValue = NullTolong(DS.Tables(0).Rows(0).Item("IDCustomer"))
                    'txtKodeCustomer.Text = NullTostr(DS.Tables(0).Rows(0).Item("KodeAlamat"))
                    txtNamaCustomer.Text = NullTostr(DS.Tables(0).Rows(0).Item("NamaAlamat"))
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

                    txtDientriOleh.Text = NullTostr(DS.Tables(0).Rows(0).Item("UserEntri"))
                    txtDieditOleh.Text = NullTostr(DS.Tables(0).Rows(0).Item("UserEdit"))
                    txtDipostingOleh.Text = NullTostr(DS.Tables(0).Rows(0).Item("UserPosting"))
                    tglEntri.DateTime = NullToDate(DS.Tables(0).Rows(0).Item("TglEntri"))
                    tglEdit.DateTime = NullToDate(DS.Tables(0).Rows(0).Item("TglEdit"))
                    tglPosting.DateTime = NullToDate(DS.Tables(0).Rows(0).Item("TglPosting"))
                    txtCatatan.Text = NullTostr(DS.Tables(0).Rows(0).Item("Catatan"))
                    IsSO = NullTobool(DS.Tables(0).Rows(0).Item("IsSO"))
                    If IsPosted Then
                        txtKodeCustomer.Properties.ReadOnly = True
                        cmdSave.Enabled = False
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
    Private Sub RefreshDetil()
        Dim strsql As String
        Dim curentcursor As Cursor = Windows.Forms.Cursor.Current
        Dim repChekEdit As New DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit
        Windows.Forms.Cursor.Current = Cursors.WaitCursor
        Try
            strsql = "select MSOD.*,MWilayah.Nama AS Wilayah, MBarang.Kode as KodeStock,MBarang.Nama as NamaStock,MSatuan.Kode as Satuan,Mgudang.Kode as KodeGudang " & vbCrLf
            strsql &= " From (MSOD Inner Join MSO On MSOD.IDSO=MSO.NoID) " & vbCrLf
            strsql &= " LEFT JOIN MBarang ON MSOD.IDBarang=MBarang.NoID " & vbCrLf
            strsql &= " LEFT JOIN MSatuan ON MSOD.IDSatuan=MSatuan.NoID " & vbCrLf
            strsql &= " LEFT JOIN (MGudang LEFT JOIN MWilayah ON MWilayah.NoID=MGudang.IDWilayah) ON MSOD.IDGudang=MGudang.NoID " & vbCrLf
            strsql &= " where MSOD.IDSO = " & NoID
            ExecuteDBGrid(GC1, strsql, "NoID")
            'SetGridView(GC1)
            For x As Integer = 0 To GV1.Columns.Count - 1
                Select Case GV1.Columns(x).ColumnType.Name.ToLower
                    Case "int32", "int64", "int"
                        GV1.Columns(x).DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
                        GV1.Columns(x).DisplayFormat.FormatString = "n0"
                    Case "decimal", "single", "money", "double"
                        GV1.Columns(x).DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
                        GV1.Columns(x).DisplayFormat.FormatString = "n2"
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
            txtDiscTotal.EditValue = (txtDiscPersen.EditValue * txtSubtotal.EditValue / 100) + txtDiscRp.EditValue
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
            If IsValidasi() Then
                If Simpan(PesanSalah) = True Then
                    If pTipe = pStatus.Baru Then
                        KodeLama = txtKode.Text
                        pTipe = pStatus.Edit
                        SetTombol()
                        txtBarang.Focus()
                        txtKodeCustomer.Properties.ReadOnly = True
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
            XtraMessageBox.Show("Kode masih kosong.", NamaAplikasi)
            txtKode.Focus()
            Return False
            Exit Function
        End If
        If txtKodeCustomer.Text = "" Then
            XtraMessageBox.Show("Customer masih kosong.", NamaAplikasi)
            txtKodeCustomer.Focus()
            Return False
            Exit Function
        End If
        If Tgl.Text = "" Then
            XtraMessageBox.Show("Tanggal masih kosong.", NamaAplikasi)
            Tgl.Focus()
            Return False
            Exit Function
        End If
        If CekKodeValid(txtKode.Text, KodeLama, "MSO", "Kode", IIf(pTipe = pStatus.Edit, True, False), " AND IsSO=" & IIf(IsSO, 1, 0)) Then
            XtraMessageBox.Show("Kode sudah dipakai.", NamaAplikasi)
            txtKode.Focus()
            Return False
            Exit Function
        End If
        Return True
    End Function
    Dim KodeLama As String = ""
    Public Function Simpan(ByRef PesanSalah As String) As Boolean
        Dim Sukses As Boolean = False
        HitungTotal()
        Try
            If pTipe = pStatus.Baru Then
                NoID = GetNewID("MSO")
                SQL = "INSERT INTO MSO (IsSO,IDWilayah,NoID,Kode,KodeReff,Tanggal,TanggalStock,JatuhTempo,"
                SQL &= " IDCustomer,TanggalSJ,NoSJ,SubTotal,DiskonNotaProsen,DiskonNotaRp,DiskonNotaTotal,"
                SQL &= "  Biaya, Total, Bayar, Sisa,IDUserEntry,IDUserEdit,IDUserPosting,TglEntri,IDAdmin,Catatan) VALUES (" & vbCrLf
                SQL &= IIf(IsSO, 1, 0) & "," & DefIDWilayah & "," & NoID & ","
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
                SQL &= IDUserAktif & ","
                SQL &= "-1,"
                SQL &= "-1,"
                SQL &= "'" & TanggalSystem.ToString("yyyy-MM-dd HH:mm") & "'," & IDAdmin & ",'" & FixApostropi(txtCatatan.Text) & "')"

                EksekusiSQL(SQL)
            Else
                SQL = "UPDATE MSO SET "
                If Not Tgl.Properties.ReadOnly Or Not txtKode.Properties.ReadOnly Then
                    SQL &= "IDAdmin=" & IDAdmin & ","
                End If
                SQL &= "Kode='" & FixApostropi(txtKode.Text) & "',"
                SQL &= "Catatan='" & FixApostropi(txtCatatan.Text) & "',"
                SQL &= "IsSO=" & IIf(IsSO, 1, 0) & ","
                SQL &= "KodeReff='" & FixApostropi(txtKodeReff.Text) & "',"
                SQL &= "Tanggal='" & Tgl.DateTime.ToString("yyyy-MM-dd HH:mm") & "',"
                SQL &= "TanggalStock='" & tglStok.DateTime.ToString("yyyy-MM-dd HH:mm") & "',"
                SQL &= "JatuhTempo='" & tglJatuhTempo.DateTime.ToString("yyyy-MM-dd HH:mm") & "',"
                SQL &= "IDCustomer=" & txtKodeCustomer.EditValue & ","
                SQL &= "TanggalSJ='" & tglSJ.DateTime.ToString("yyyy-MM-dd HH:mm") & "',"
                SQL &= "NoSJ='" & FixApostropi(txtNoSJ.Text) & "',"
                SQL &= "SubTotal=" & FixKoma(txtSubtotal.EditValue) & ","
                SQL &= "DiskonNotaProsen=" & FixKoma(txtDiscPersen.EditValue) & ","
                SQL &= "DiskonNotaRp=" & FixKoma(txtDiscRp.EditValue) & ","
                SQL &= "DiskonNotaTotal=" & FixKoma(txtDiscTotal.EditValue) & ","
                SQL &= "Biaya=" & FixKoma(txtBiaya.EditValue) & ","
                SQL &= "Total=" & FixKoma(txtTotal.EditValue) & ","
                SQL &= "Bayar=" & FixKoma(txtBayar.EditValue) & ","
                SQL &= "Sisa=" & FixKoma(txtSisa.EditValue) & ","
                SQL &= "TglEdit='" & TanggalSystem.ToString("yyyy-MM-dd HH:mm") & "',"
                SQL &= "IDUserEdit=" & IDUserAktif
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
    Private Sub BarButtonItem3_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem3.ItemClick
        Dim xOtorisasi As New frmOtorisasiAdmin
        Try
            If XtraMessageBox.Show("Simpan Layout?", NamaAplikasi, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = Windows.Forms.DialogResult.Yes AndAlso xOtorisasi.ShowDialog = Windows.Forms.DialogResult.OK Then
                Ini.TulisIniPath(Application.StartupPath & "\system\Layouts\" & Me.Name & ".ini", "Form", "Width", Me.Width)
                Ini.TulisIniPath(Application.StartupPath & "\system\Layouts\" & Me.Name & ".ini", "Form", "Height", Me.Height)
                Me.Left = (Screen.PrimaryScreen.WorkingArea.Width - Me.Width) \ 2
                Me.Top = (Screen.PrimaryScreen.WorkingArea.Height - Me.Height) \ 2

                LayoutControl1.SaveLayoutToXml(Application.StartupPath & "\System\Layouts\" & Me.Name & LayoutControl1.Name & IsSO & ".xml")
                GV1.SaveLayoutToXml(Application.StartupPath & "\System\Layouts\" & Me.Name & GV1.Name & IsSO & ".xml")
                SearchLookUpEdit1View.SaveLayoutToXml(Application.StartupPath & "\System\Layouts\" & Me.Name & SearchLookUpEdit1View.Name & IsSO & ".xml")
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
            If XtraMessageBox.Show("Item ini mau dihapus?", NamaAplikasi, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) = Windows.Forms.DialogResult.Yes Then
                EksekusiSQL("Delete From MSOD where NoID=" & IDDetil.ToString)
                RefreshDetil()
            End If
            txtBarang.Focus()
        Catch ex As Exception
            XtraMessageBox.Show("Untuk menghapus pilih item yang akan dihapus terlebih dahulu lalu tekan tombol hapus.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub txtKodeCustomer_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtKodeCustomer.EditValueChanged
        Dim TglAdd As Long = 0
        Try
            txtNamaCustomer.Text = NullTostr(SearchLookUpEdit1View.GetRowCellValue(SearchLookUpEdit1View.FocusedRowHandle, "Nama"))
            txtAlamatCustomer.Text = NullTostr(EksekusiSQlSkalarNew("SELECT Alamat FROM MAlamat WHERE NoID=" & NullTolong(txtKodeCustomer.EditValue)))
            TglAdd = NullTolong(EksekusiSQlSkalarNew("SELECT JatuhTempoCustomer FROM MAlamat WHERE NoID=" & NullTolong(txtKodeCustomer.EditValue)))
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
            'txtNamaCustomer.Text = NullTostr(SearchLookUpEdit1View.GetRowCellValue(SearchLookUpEdit1View.FocusedRowHandle, "Nama"))
            'txtAlamatCustomer.Text = NullTostr(EksekusiSQlSkalarNew("SELECT Alamat FROM MAlamat WHERE NoID=" & NullTolong(txtKodeCustomer.EditValue)))
            TglAdd = NullTolong(EksekusiSQlSkalarNew("SELECT JatuhTempoCustomer FROM MAlamat WHERE NoID=" & NullTolong(txtKodeCustomer.EditValue)))
            If TglAdd = 0 Then
                tglJatuhTempo.DateTime = DateAdd(DateInterval.Month, 2, Tgl.DateTime)
            Else
                tglJatuhTempo.DateTime = DateAdd(DateInterval.Day, TglAdd, Tgl.DateTime)
            End If
            If pTipe = pStatus.Baru Then
                txtKode.Text = clsKode.MintaKodeBaru("SO", "MSO", Tgl.DateTime, DefIDWilayah, 5)
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
    Private Function CariBarang(ByRef IDBarang As Long, ByRef NamaBarang As String) As Boolean
        Dim x As Boolean = False
        Dim SQL As String = ""
        Dim oDS As New DataSet
        Try
            SQL = "SELECT NoID,Kode,Nama FROM MBarang WHERE " & IIf(IsSO, "", " IsNonStock=1 AND ") & " MBarang.IsActive=1 AND (UPPER(Kode) = '" & NamaBarang.Replace("'", "''").ToUpper & "' OR UPPER(NAMA) = '" & NamaBarang.Replace("'", "''").ToUpper & "') ORDER BY Kode"
            oDS = ExecuteDataset("Tbl", SQL)
            If oDS.Tables("Tbl").Rows.Count >= 1 Then
                NamaBarang = NullTostr(oDS.Tables(0).Rows(0).Item("Nama"))
                IDBarang = NullTolong(oDS.Tables(0).Rows(0).Item("NoID"))
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
        Dim IDBarang As Long = -1
        Dim IDDetil As Long = -1
        Dim frmEntri As New frmEntriSOD
        Dim Konversi As Double = 0.0
        Try
            If txtKodeCustomer.Text = "" Then XtraMessageBox.Show("Customer masih kosong.", NamaBarang) : txtKodeCustomer.Focus() : Exit Sub
            If CariBarang(IDBarang, NamaBarang) Then
                If XtraMessageBox.Show("Ingin menambah barang " & NamaBarang, "Fast Entri Says", MessageBoxButtons.YesNo) = Windows.Forms.DialogResult.Yes Then
                    IDDetil = GetNewID("MSOD", "NoID")
                    Konversi = NullToDbl(EksekusiSQlSkalarNew("SELECT Konversi FROM MBarangD WHERE IsJual=1 AND IDBarang=" & IDBarang & " AND IDSatuan=" & DefIDSatuan))
                    If Konversi <> 0 Then
                        SQL = "INSERT INTO MSOD (NoID,IDBarang,IDSatuan,Konversi,IDSO,NoUrut,Tgl,Jam) VALUES " & vbCrLf
                        SQL &= "(" & IDDetil & "," & IDBarang & "," & DefIDSatuan & "," & FixKoma(Konversi) & "," & NoID & "," & GetNewID("MSOD", "NoUrut", " WHERE IDSO=" & NoID) & ",GetDate(),GetDate())"
                    Else
                        SQL = "INSERT INTO MSOD (NoID,IDBarang,IDSO,NoUrut,Tgl,Jam) VALUES " & vbCrLf
                        SQL &= "(" & IDDetil & "," & IDBarang & "," & NoID & "," & GetNewID("MSOD", "NoUrut", " WHERE IDSO=" & NoID) & ",GetDate(),GetDate())"
                    End If
                    EksekusiSQL(SQL)
                    frmEntri.IsNew = False
                    frmEntri.NoID = IDDetil
                    frmEntri.IDSO = NoID
                    frmEntri.IDCustomer = NullTolong(txtKodeCustomer.EditValue)
                    If frmEntri.ShowDialog = Windows.Forms.DialogResult.OK Then
                        RefreshDetil()
                        GV1.ClearSelection()
                        GV1.FocusedRowHandle = GV1.LocateByDisplayText(0, GV1.Columns("NoID"), IDDetil.ToString("#,##0"))
                        GV1.SelectRow(GV1.FocusedRowHandle)
                    Else
                        SQL = "DELETE FROM MSOD WHERE NoID=" & IDDetil
                        EksekusiSQL(SQL)
                        RefreshDetil()
                        GV1.ClearSelection()
                        GV1.FocusedRowHandle = GV1.LocateByDisplayText(0, GV1.Columns("NoID"), IDDetil.ToString("#,##0"))
                        GV1.SelectRow(GV1.FocusedRowHandle)
                    End If
                End If
            End If
        Catch ex As Exception
            XtraMessageBox.Show("Kesalahan : " & ex.Message, "Fast Entri Says", MessageBoxButtons.OK)
            frmEntri.Dispose()
        End Try
    End Sub

    Private Sub cmdBAru_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdBAru.Click
        Dim frmEntri As New frmEntriSOD
        Try
            Dim IDDetil As Long = -1
            frmEntri.NoID = IDDetil
            frmEntri.IsNew = True
            frmEntri.IDSO = NoID
            frmEntri.IsSO = IsSO
            If Not IsSO Then
                frmEntri.Text = "Entri Detil Permintaan Barang"
            End If
            If txtKodeCustomer.Text <> "" Then
                frmEntri.IDCustomer = NullTolong(txtKodeCustomer.EditValue)
                frmEntri.NoID = IDDetil
                frmEntri.IsNew = True
                frmEntri.IDSO = NoID
                If frmEntri.ShowDialog = Windows.Forms.DialogResult.OK Then
                    RefreshDetil()
                End If
                txtBarang.Focus()
            Else
                XtraMessageBox.Show("Isi dulu Customer, lalu click baru.", NamaAplikasi, MessageBoxButtons.OK)
            End If
        Catch ex As Exception
            XtraMessageBox.Show("Untuk membuat baru tekan tombol baru", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            frmEntri.Dispose()
        End Try
    End Sub

    Private Sub cmdEdit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdEdit.Click
        Dim view As ColumnView = GC1.FocusedView
        Dim frmEntri As New frmEntriSOD
        Try
            Dim row As System.Data.DataRow = view.GetDataRow(GV1.FocusedRowHandle)
            Dim dc As Integer = GV1.FocusedRowHandle
            Dim IDDetil As Long = NullTolong(row("NoID"))
            If txtKodeCustomer.Text <> "" Then
                frmEntri.IDCustomer = NullTolong(txtKodeCustomer.EditValue)
                frmEntri.IsNew = False
                frmEntri.NoID = IDDetil
                frmEntri.IDSO = NoID
                frmEntri.IsSO = IsSO
                If Not IsSO Then
                    frmEntri.Text = "Entri Detil Permintaan Barang"
                End If
                If frmEntri.ShowDialog = Windows.Forms.DialogResult.OK Then
                    RefreshDetil()
                End If
                txtBarang.Focus()
            Else
                XtraMessageBox.Show("Isi dulu Customer, lalu click Edit.", NamaAplikasi, MessageBoxButtons.OK)
            End If
        Catch ex As Exception
            XtraMessageBox.Show("Untuk mengedit pilih item satuan yang akan diedit terlebih dahulu lalu tekan tombol edit", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            frmEntri.Dispose()
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

    Private Sub txtNamaCustomer_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtNamaCustomer.EditValueChanged

    End Sub

    Private Sub Tgl_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Tgl.KeyDown
        If e.KeyCode = Keys.F7 Then
            Dim x As New frmOtorisasiAdmin
            Try
                If Tgl.Properties.ReadOnly Then
                    If x.ShowDialog = Windows.Forms.DialogResult.OK Then
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
                txtBarang.Focus()
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
End Class