Imports System.Data
Imports System.Data.SqlClient
Imports DevExpress.XtraEditors
Imports DevExpress.Utils
Imports DevExpress.XtraGrid.Views.Base

Public Class frmEntriTransferOutMutasiWilayah
    Dim SQL As String = ""
    Public Enum pStatus
        Baru = 0
        Edit = 1
    End Enum
    Public pTipe As pStatus
    Public IsPosted As Boolean = False
    Public NoID As Long = -1
    Dim oda2 As SqlDataAdapter
    Dim IDAdmin As Long = -1
    Dim oDS As New DataSet
    Dim BS As New BindingSource
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
            SQL = "SELECT NoID, Kode, Nama FROM MAlamat WHERE IsActive=1 AND IsPegawai=1"
            ds = ExecuteDataset("master", SQL)
            txtPegawai.Properties.DataSource = ds.Tables("master")
            txtPegawai.Properties.ValueMember = "NoID"
            txtPegawai.Properties.DisplayMember = "Nama"
            If System.IO.File.Exists(folderLayouts & Me.Name & gvPegawai.Name & ".xml") Then
                gvPegawai.RestoreLayoutFromXml(folderLayouts & Me.Name & gvPegawai.Name & ".xml")
            End If

            SQL = "SELECT NoID, Kode, Nama FROM MWilayah WHERE IsActive=1 "
            ds = ExecuteDataset("master", SQL)
            txtWilayahDiminta.Properties.DataSource = ds.Tables("master")
            txtWilayahDiminta.Properties.ValueMember = "NoID"
            txtWilayahDiminta.Properties.DisplayMember = "Nama"
            If System.IO.File.Exists(folderLayouts & Me.Name & gvWilayahDiminta.Name & ".xml") Then
                gvWilayahDiminta.RestoreLayoutFromXml(folderLayouts & Me.Name & gvWilayahDiminta.Name & ".xml")
            End If

            SQL = "SELECT NoID, Kode, Nama FROM MWilayah WHERE IsActive=1 AND NoID<>" & NullTolong(txtWilayahDiminta.EditValue)
            ds = ExecuteDataset("master", SQL)
            txtWilayah.Properties.DataSource = ds.Tables("master")
            txtWilayah.Properties.ValueMember = "NoID"
            txtWilayah.Properties.DisplayMember = "Nama"
            If System.IO.File.Exists(folderLayouts & Me.Name & gvWilayah.Name & ".xml") Then
                gvWilayah.RestoreLayoutFromXml(folderLayouts & Me.Name & gvWilayah.Name & ".xml")
            End If

            SQL = "SELECT MGudang.NoID, MGudang.Kode, MGudang.Nama, MWilayah.Nama AS Wilayah FROM MGudang LEFT JOIN MWilayah ON MGudang.IDWilayah=MWilayah.NoID WHERE MGudang.InTransit=1"
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
    Private Sub RefreshDataWilayahDiminta()
        Dim ds As New DataSet
        Try
            SQL = "SELECT NoID, Kode, Nama FROM MWilayah WHERE IsActive=1 AND NoID<>" & NullTolong(txtWilayahDiminta.EditValue)
            ds = ExecuteDataset("master", SQL)
            txtWilayah.Properties.DataSource = ds.Tables("master")
            txtWilayah.Properties.ValueMember = "NoID"
            txtWilayah.Properties.DisplayMember = "Nama"
            If System.IO.File.Exists(folderLayouts & Me.Name & gvWilayah.Name & ".xml") Then
                gvWilayah.RestoreLayoutFromXml(folderLayouts & Me.Name & gvWilayah.Name & ".xml")
            End If
        Catch ex As Exception
            XtraMessageBox.Show("Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            ds.Dispose()
        End Try
    End Sub
    Dim DefImageList As New ImageList

    Private Sub frmEntriPO_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        If IsTempInsertBaru Then
            If XtraMessageBox.Show("Yakin ingin keluar?" & vbCrLf & "Ya, akan menghapus transaksi dan detil item.", NamaAplikasi, MessageBoxButtons.YesNo, MessageBoxIcon.Information, MessageBoxDefaultButton.Button2) = Windows.Forms.DialogResult.Yes Then
                EksekusiSQL("DELETE FROM MTransferOut WHERE NoID=" & NoID)
                EksekusiSQL("DELETE FROM MTransferOutD WHERE IDHeader=" & NoID)
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
                SQL = "SELECT MTransferOut.*, MUserEntri.Nama AS UserEntri, MUserEdit.Nama AS UserEdit, MUserPosting.Nama AS UserPosting, MAlamat.Alamat AS Alamat, MAlamat.Nama AS NamaAlamat, MAlamat.Kode as KodeAlamat "
                SQL &= " FROM MTransferOut LEFT JOIN MAlamat ON MAlamat.NoID=MTransferOut.IDPegawai "
                SQL &= " LEFT JOIN MUser MUserEntri ON MUserEntri.NoID=MTransferOut.IDUserEntry "
                SQL &= " LEFT JOIN MUser MUserEdit ON MUserEdit.NoID=MTransferOut.IDUserEdit "
                SQL &= " LEFT JOIN MUser MUserPosting ON MUserPosting.NoID=MTransferOut.IDUserPosting "
                SQL &= " WHERE MTransferOut.NoID = " & NoID
                DS = ExecuteDataset("Tabel", SQL)
                If DS.Tables("Tabel").Rows.Count >= 1 Then
                    txtKode.Text = NullTostr(DS.Tables(0).Rows(0).Item("Kode"))
                    KodeLama = txtKode.Text
                    txtKodeReff.Text = NullTostr(DS.Tables(0).Rows(0).Item("KodeReff"))
                    txtWilayahDiminta.EditValue = NullTolong(DS.Tables(0).Rows(0).Item("IDWilayahDari"))
                    txtWilayahDiminta.Properties.ReadOnly = False
                    txtWilayah.EditValue = NullTolong(DS.Tables(0).Rows(0).Item("IDWilayahUntuk"))
                    txtWilayah.Properties.ReadOnly = False
                    txtGudang.EditValue = NullTolong(DS.Tables(0).Rows(0).Item("IDGudangIntransit"))
                    txtPegawai.EditValue = NullTolong(DS.Tables(0).Rows(0).Item("IDPegawai"))
                    txtTgl.DateTime = NullToDate(DS.Tables(0).Rows(0).Item("Tanggal"))
                    txtJam.DateTime = NullToDate(DS.Tables(0).Rows(0).Item("Jam"))
                    IsPosted = NullTobool(DS.Tables(0).Rows(0).Item("IsPosted"))
                    txtDientriOleh.Text = NullTostr(DS.Tables(0).Rows(0).Item("UserEntri"))
                    txtDieditOleh.Text = NullTostr(DS.Tables(0).Rows(0).Item("UserEdit"))
                    txtDipostingOleh.Text = NullTostr(DS.Tables(0).Rows(0).Item("UserPosting"))
                    tglEntri.DateTime = NullToDate(DS.Tables(0).Rows(0).Item("TglEntri"))
                    tglEdit.DateTime = NullToDate(DS.Tables(0).Rows(0).Item("TglEdit"))
                    tglPosting.DateTime = NullToDate(DS.Tables(0).Rows(0).Item("TglPosting"))
                    If IsPosted Then
                        txtWilayah.Properties.ReadOnly = True
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
    Private Sub txt_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim txt As DevExpress.XtraEditors.TextEdit = TryCast(sender, DevExpress.XtraEditors.TextEdit)
        If txt.Properties.Mask.MaskType = Mask.MaskType.Numeric Then
            txt.SelectAll()
        End If
    End Sub
    Public Sub RefreshDetil()
        Dim strsql As String
        Dim curentcursor As Cursor = Windows.Forms.Cursor.Current
        Dim repChekEdit As New DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit
        'Dim ds As New DataSet
        Windows.Forms.Cursor.Current = Cursors.WaitCursor
        Try
            strsql = "SELECT MTransferOutD.*, MGudang.Nama AS DariGudang, MPackingMutasiWilayah.NoID AS IDPackingMutasi, MPackingMutasiWilayah.Kode AS NoPackingMutasi, MBarang.Nama AS NamaStock, MBarang.Kode AS KodeStock, MSatuan.Nama AS Satuan, MSatuan.Kode AS KodeSatuan " & vbCrLf & _
                     " FROM MTransferOutD " & vbCrLf & _
                     " LEFT JOIN (MPackingMutasiWilayahD LEFT JOIN MPackingMutasiWilayah ON MPackingMutasiWilayah.NoID=MPackingMutasiWilayahD.IDHeader) ON MPackingMutasiWilayahD.NoID=MTransferOutD.IDPackingMutasiWilayahD" & vbCrLf & _
                     " LEFT JOIN MGudang ON MGudang.NoID=MTransferOutD.IDGudangDari" & vbCrLf & _
                     " LEFT JOIN MBarang ON MBarang.NoID=MTransferOutD.IDBarang" & vbCrLf & _
                     " LEFT JOIN MSatuan ON MSatuan.NoID=MTransferOutD.IDSatuan" & vbCrLf & _
                     " WHERE MTransferOutD.IDHeader = " & NoID
            ExecuteDBGrid(GC1, strsql, "NoID")
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
                'If GV1.Columns(x).FieldName.ToLower = "Keterangan".ToLower Then
                '    GV1.Columns(x).OptionsColumn.AllowEdit = True
                'Else
                '    GV1.Columns(x).OptionsColumn.AllowEdit = False
                'End If
            Next
            GV1.OptionsBehavior.Editable = True
            GV1.HideFindPanel()
            RefreshLookUpPacking()
        Catch ex As Exception
            XtraMessageBox.Show(ex.Message, ".:: Pesan Kesalahan ::.", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
        Windows.Forms.Cursor.Current = curentcursor
        Application.DoEvents()
    End Sub
    Private Sub IsiDefault()
        SetTombol()
        txtWilayahDiminta.EditValue = DefIDWilayah
        RubahWilayah()
        txtGudang.EditValue = NullTolong(EksekusiSQlSkalarNew("SELECT IDGudangInTransit FROM MSETTING"))
        RubahGudang()
        txtTgl.DateTime = TanggalSystem
        txtJam.DateTime = TanggalSystem
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
            'cmdBAru.Enabled = False
            'cmdEdit.Enabled = False
            cmdDelete.Enabled = False
        Else
            'cmdBAru.Enabled = True
            'cmdEdit.Enabled = True
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
                        If GV1.RowCount = 0 Then
                            InsertIntoDetilTransferOut(txtBarang.EditValue)
                        End If
                        IsTempInsertBaru = True
                        txtWilayah.Properties.ReadOnly = True
                        'txtKodeSupplier.Properties.ReadOnly = True
                    Else
                        'UpdateDetail()
                        'KODE DIBAWAH UNTUK MEMBUKA FORM HANYA 1
                        Dim frmEntri As frmDaftarMutasiWilayah = Nothing
                        Dim F As Object
                        For Each F In MdiParent.MdiChildren
                            If TypeOf F Is frmDaftarMutasiWilayah Then
                                frmEntri = F
                                If frmEntri.FormName = FormNameDaftar Then
                                    Exit For
                                Else
                                    frmEntri = Nothing
                                End If
                            End If
                        Next
                        If frmEntri Is Nothing Then
                            frmEntri = New frmDaftarMutasiWilayah
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
    'Private Sub Updatedetail()
    '    Try
    '        For i As Integer = 0 To GV1.RowCount - 1

    '        Next
    '    Catch ex As Exception

    '    End Try
    'End Sub
    Private Function IsValidasi() As Boolean
        If txtKode.Text = "" Then
            XtraMessageBox.Show("Kode masih kosong.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
            txtKode.Focus()
            Return False
            Exit Function
        End If
        If txtPegawai.Text = "" Then
            XtraMessageBox.Show("Nama Pegawai yang meminta masih kosong.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
            txtPegawai.Focus()
            Return False
            Exit Function
        End If
        If txtWilayah.Text = "" Then
            XtraMessageBox.Show("Wilayah yg meminta masih kosong.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
            txtWilayah.Focus()
            Return False
            Exit Function
        End If
        If txtWilayahDiminta.Text = "" Then
            XtraMessageBox.Show("Wilayah yang dimintai masih kosong.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
            txtWilayahDiminta.Focus()
            Return False
            Exit Function
        End If
        If txtGudang.Text = "" Then
            XtraMessageBox.Show("Gudang setting dulu di Menu Setting atau tanyakan Administrator.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
            txtGudang.Focus()
            Return False
            Exit Function
        End If
        If txtPegawai.Text = "" Then
            XtraMessageBox.Show("Penanggung jawab gudang masih kosong.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
            txtPegawai.Focus()
            Return False
            Exit Function
        End If
        If pTipe = pStatus.Edit AndAlso txtKodeReff.Text = "" Then
            XtraMessageBox.Show("Kode reff masih kosong.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
            txtKodeReff.Focus()
            Return False
            Exit Function
        End If
        If txtTgl.Text = "" Then
            XtraMessageBox.Show("Tanggal masih kosong.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
            txtTgl.Focus()
            Return False
            Exit Function
        End If
        If txtJam.Text = "" Then
            XtraMessageBox.Show("Jam masih kosong.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
            txtTgl.Focus()
            Return False
            Exit Function
        End If
        'Dim Sukses As Boolean = True
        'For i As Integer = 0 To GV1.RowCount - 1
        '    If NullTolong(GV1.GetRowCellValue(i, "IDSPKMutasi")) <> NullTolong(txtKodeReff.EditValue) Then
        '        XtraMessageBox.Show("Item stok " & GV1.GetRowCellValue(i, "NamaStock").ToString & " tidak sesuai dengan Kode Reff.",NamaAplikasi,MessageBoxButtons.OK,MessageBoxIcon.Information)
        '        GV1.ClearSelection()
        '        GV1.FocusedRowHandle = i
        '        Sukses = False
        '        Exit For
        '    End If
        'Next
        'If Not Sukses Then
        '    Return False
        '    Exit Function
        'End If
        If CekKodeValid(txtKode.Text, KodeLama, "MTransferOut", "Kode", IIf(pTipe = pStatus.Edit, True, False)) Then
            XtraMessageBox.Show("Kode sudah dipakai.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
            If pTipe = pStatus.Baru Then
                If XtraMessageBox.Show("Lakukan pemberian kode baru?", NamaAplikasi, MessageBoxButtons.YesNo, MessageBoxIcon.Information) = Windows.Forms.DialogResult.Yes Then
                    txtKode.Text = clsKode.MintaKodeBaru("TO", "MTransferOut", txtTgl.DateTime, , 5)
                End If
            End If
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
    Dim KodeLama As String = ""
    Public Function Simpan(ByRef PesanSalah As String) As Boolean
        Dim Sukses As Boolean = False
        Try
            If pTipe = pStatus.Baru Then
                NoID = GetNewID("MTransferOut", "NoID")
                SQL = "INSERT INTO [MTransferOut] ([NoID],[Nomor],[Kode],[KodeReff],[IDWilayahDari],[IDWilayahUntuk],[Tanggal],[Jam],[IDGudangIntransit],[IDPegawai],[IDUserEntry],[TglEntri],[IDUserEdit],[IDUserPosting],[IDAdmin],[TglPosting],[IsPosted]) VALUES (" & vbCrLf & _
                      NoID & ", " & _
                      NullTolong(GetNewID("MTransferOut", "Nomor")) & ", " & _
                      "'" & FixApostropi(txtKode.Text) & "', " & _
                      "'" & FixApostropi(txtKodeReff.Text) & "', " & _
                      NullTolong(txtWilayahDiminta.EditValue) & "," & _
                      NullTolong(txtWilayah.EditValue) & "," & _
                      "'" & txtTgl.DateTime.ToString("yyyy-MM-dd ") & txtJam.DateTime.ToString("HH:mm") & "', " & _
                      "'" & txtJam.DateTime.ToString("HH:mm") & "', " & _
                      NullTolong(txtGudang.EditValue) & ", " & _
                      NullTolong(txtPegawai.EditValue) & "," & _
                      IDUserAktif & ", " & _
                      "Getdate()," & _
                      "NULL, " & _
                      "NULL, " & _
                      IIf(txtWilayahDiminta.Properties.ReadOnly = False Or txtKode.Properties.ReadOnly = False Or txtTgl.Properties.ReadOnly = False Or txtJam.Properties.ReadOnly = False, IDUserAktif, "NULL ") & ", " & _
                      "NULL, " & _
                      "0)"
            Else
                SQL = "UPDATE MTransferOut SET " & vbCrLf & _
                      "Kode='" & FixApostropi(txtKode.Text) & "', " & _
                      "KodeReff='" & FixApostropi(txtKodeReff.Text) & "', " & _
                      "IDWilayahDari=" & NullTolong(txtWilayahDiminta.EditValue) & "," & _
                      "IDWilayahUntuk=" & NullTolong(txtWilayah.EditValue) & "," & _
                      "Tanggal='" & txtTgl.DateTime.ToString("yyyy-MM-dd ") & txtJam.DateTime.ToString("HH:mm") & "', " & _
                      "Jam='" & txtJam.DateTime.ToString("HH:mm") & "', " & _
                      "IDGudangIntransit=" & NullTolong(txtGudang.EditValue) & ", " & _
                      "IDPegawai=" & NullTolong(txtPegawai.EditValue) & "," & _
                      "IDUserEdit=" & IDUserAktif & "," & _
                      "TglEdit=getdate() "
                If txtWilayahDiminta.Properties.ReadOnly = False Or txtKode.Properties.ReadOnly = False Or txtTgl.Properties.ReadOnly = False Or txtJam.Properties.ReadOnly = False Then
                    SQL &= ", IDAdmin=" & IDUserAktif
                End If
                SQL &= " WHERE NoID=" & NoID
            End If
            EksekusiSQL(SQL)
            Sukses = True
        Catch ex As Exception
            PesanSalah = ex.Message
            Sukses = False
        Finally

        End Try
        Return Sukses
    End Function

    Private Sub BarButtonItem3_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles mnSaveLayout.ItemClick
        Dim xOtorisasi As New frmOtorisasiAdmin
        Try
            If XtraMessageBox.Show("Simpan Layout?", NamaAplikasi, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = Windows.Forms.DialogResult.Yes AndAlso xOtorisasi.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
                Ini.TulisIniPath(folderLayouts & Me.Name & ".ini", "Form", "Width", Me.Width)
                Ini.TulisIniPath(folderLayouts & Me.Name & ".ini", "Form", "Height", Me.Height)
                Me.Left = (Screen.PrimaryScreen.WorkingArea.Width - Me.Width) \ 2
                Me.Top = (Screen.PrimaryScreen.WorkingArea.Height - Me.Height) \ 2

                LayoutControl1.SaveLayoutToXml(folderLayouts & Me.Name & LayoutControl1.Name & ".xml")
                GV1.SaveLayoutToXml(folderLayouts & Me.Name & GV1.Name & ".xml")
                gvPegawai.SaveLayoutToXml(folderLayouts & Me.Name & gvPegawai.Name & ".xml")
                gvWilayahDiminta.SaveLayoutToXml(folderLayouts & Me.Name & gvWilayahDiminta.Name & ".xml")
                gvWilayah.SaveLayoutToXml(folderLayouts & Me.Name & gvWilayah.Name & ".xml")
                gvGudang.SaveLayoutToXml(folderLayouts & Me.Name & gvGudang.Name & ".xml")
                gvBarang.SaveLayoutToXml(folderLayouts & Me.Name & gvBarang.Name & ".xml")
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
        Dim row As System.Data.DataRow = Nothing
        Dim IDDetil As Long = -1
        Try
            If XtraMessageBox.Show("Yakin data yang dipilih ini mau dihapus?", "Hapus Multiple", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) = Windows.Forms.DialogResult.Yes Then
                For i As Integer = 0 To GV1.SelectedRowsCount - 1
                    row = view.GetDataRow(i)
                    IDDetil = NullTolong(row("NoID"))
                    EksekusiSQL("Delete From MTransferOutD where NoID=" & IDDetil.ToString)
                Next
                txtBarang.Focus()
                RefreshDetil()
            End If
        Catch ex As Exception
            XtraMessageBox.Show("Untuk menghapus pilih item barang yang akan dihapus terlebih dahulu lalu tekan tombol hapus", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub Tgl_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtTgl.EditValueChanged
        Dim TglAdd As Long = 0
        Try
            If pTipe = pStatus.Baru Then
                txtKode.Text = clsKode.MintaKodeBaru("TO", "MTransferOut", txtTgl.DateTime, , 5)
            End If
        Catch ex As Exception

        End Try
    End Sub

    Private Sub txtBarang_ButtonClick(ByVal sender As Object, ByVal e As DevExpress.XtraEditors.Controls.ButtonPressedEventArgs) Handles txtBarang.ButtonClick
        If pTipe = pStatus.Baru Then Exit Sub
        Select Case e.Button.Index
            Case 1
                InsertIntoDetilTransferOut(txtBarang.EditValue)
                'txtBarang.EditValue = -1
                txtBarang.Focus()
            Case 2
                txtBarang.EditValue = -1
        End Select
    End Sub
    Private Function CariBarang(ByRef IDBarang As Long, ByRef NamaBarang As String) As Boolean
        Dim x As Boolean = False
        Dim SQL As String = ""
        Dim oDS As New DataSet
        Try
            SQL = "SELECT NoID,Kode,Nama FROM MBarang WHERE IsActive=1 AND (ISNULL(KODE,'')<>'') AND (UPPER(Kode) = '" & NamaBarang.Replace("'", "''").ToUpper & "' OR UPPER(NAMA) = '" & NamaBarang.Replace("'", "''").ToUpper & "') ORDER BY Kode"
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

    Private Sub Tgl_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtTgl.KeyDown
        If e.KeyCode = Keys.F7 Then
            Dim x As New frmOtorisasiAdmin
            Try
                If txtTgl.Properties.ReadOnly Then
                    If x.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
                        IDAdmin = x.IDUserAdmin
                        txtTgl.Properties.ReadOnly = False
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
                InsertIntoDetilTransferOut(txtBarang.EditValue)
                txtBarang.Focus()
            Case Keys.Escape
                txtBarang.EditValue = -1
        End Select
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

    Private Sub txtWilayahDiminta_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtWilayahDiminta.EditValueChanged
        RubahWilayah()
    End Sub

    Private Sub RubahWilayah()
        If pTipe = pStatus.Baru Then
            txtKode.Text = clsKode.MintaKodeBaru("TO", "MTransferOut", txtTgl.DateTime, , 5)
        End If
        RefreshDataWilayahDiminta()
        RefreshLookUpPacking()
    End Sub

    Private Sub txtWilayahDiminta_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtWilayahDiminta.KeyDown
        If e.KeyCode = Keys.F7 Then
            Dim x As New frmOtorisasiAdmin
            Try
                If txtWilayah.Properties.ReadOnly Then
                    If x.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
                        IDAdmin = x.IDUserAdmin
                        txtWilayah.Properties.ReadOnly = False
                    End If
                End If
            Catch ex As Exception
                XtraMessageBox.Show("Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End If
    End Sub

    Private Sub gvWilayahDiminta_DataSourceChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvWilayahDiminta.DataSourceChanged
        If System.IO.File.Exists(folderLayouts & Me.Name & gvWilayahDiminta.Name & ".xml") Then
            gvWilayahDiminta.RestoreLayoutFromXml(folderLayouts & Me.Name & gvWilayahDiminta.Name & ".xml")
        End If
        With gvWilayahDiminta
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

    Private Sub gvPegawai_DataSourceChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvPegawai.DataSourceChanged
        If System.IO.File.Exists(folderLayouts & Me.Name & gvWilayahDiminta.Name & ".xml") Then
            gvWilayahDiminta.RestoreLayoutFromXml(folderLayouts & Me.Name & gvWilayahDiminta.Name & ".xml")
        End If
        With gvWilayahDiminta
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

    Private Sub gvWilayah_DataSourceChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvWilayah.DataSourceChanged
        If System.IO.File.Exists(folderLayouts & Me.Name & gvWilayah.Name & ".xml") Then
            gvWilayah.RestoreLayoutFromXml(folderLayouts & Me.Name & gvWilayah.Name & ".xml")
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

    Private Sub txtJam_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtJam.EditValueChanged

    End Sub

    Private Sub txtJam_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtJam.KeyDown
        If e.KeyCode = Keys.F7 Then
            Dim x As New frmOtorisasiAdmin
            Try
                If txtJam.Properties.ReadOnly Then
                    If x.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
                        IDAdmin = x.IDUserAdmin
                        txtJam.Properties.ReadOnly = False
                    End If
                End If
            Catch ex As Exception
                XtraMessageBox.Show("Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End If
    End Sub

    'Private Sub RubahLookUp()
    '    Dim ds As New DataSet
    '    Try
    '        SQL = "SELECT NoID, Kode, Tanggal, SUM(Sisa) AS Sisa  " & vbCrLf & _
    '              " FROM" & vbCrLf & _
    '              " (SELECT MSPKMutasiWilayah.NoID, MSPKMutasiWilayah.Kode, MSPKMutasiWilayah.Tanggal, MSPKMutasiWilayahD.QtyPcs-IsNull((SELECT SUM(MTransferOutD.QtyPcs) FROM MTransferOutD WHERE MTransferOutD.IDSPKMutasiWilayahD=MSPKMutasiWilayahD.NoID),0) AS Sisa " & vbCrLf & _
    '              " FROM MSPKMutasiWilayahD LEFT JOIN MSPKMutasiWilayah ON MSPKMutasiWilayah.NoID=MSPKMutasiWilayahD.IDHeader WHERE MSPKMutasiWilayahD.QtyPcs-IsNull((SELECT SUM(MTransferOutD.QtyPcs) FROM MTransferOutD WHERE MTransferOutD.IDHeader<>" & NoID & " AND MTransferOutD.IDSPKMutasiWilayahD=MSPKMutasiWilayahD.NoID),0)>0 AND MSPKMutasiWilayah.IDWilayahDari=" & NullTolong(txtWilayahDiminta.EditValue) & " AND MSPKMutasiWilayah.IsPosted=1 AND (MSPKMutasiWilayah.IsSelesai=0 OR MSPKMutasiWilayah.IsSelesai IS NULL)" & vbCrLf & _
    '              " GROUP BY MSPKMutasiWilayah.NoID, MSPKMutasiWilayah.Kode, MSPKMutasiWilayah.Tanggal, MSPKMutasiWilayahD.NoID, MSPKMutasiWilayahD.QtyPcs) X " & vbCrLf & _
    '              " GROUP BY NoID, Kode, Tanggal"
    '        ds = ExecuteDataset("master", SQL)
    '        txtKodeReff.Properties.DataSource = ds.Tables("master")
    '        txtKodeReff.Properties.ValueMember = "NoID"
    '        txtKodeReff.Properties.DisplayMember = "Kode"
    '        If System.IO.File.Exists(folderLayouts &  Me.Name & gvKodeReff.Name & ".xml") Then
    '            gvKodeReff.RestoreLayoutFromXml(folderLayouts &  Me.Name & gvKodeReff.Name & ".xml")
    '        End If
    '    Catch ex As Exception
    '        XtraMessageBox.Show("Info " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
    '    Finally
    '        ds.Dispose()
    '    End Try
    'End Sub

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

    Private Sub gvPegawai_DataSourceChanged1(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvPegawai.DataSourceChanged
        If System.IO.File.Exists(folderLayouts & Me.Name & gvPegawai.Name & ".xml") Then
            gvPegawai.RestoreLayoutFromXml(folderLayouts & Me.Name & gvPegawai.Name & ".xml")
        End If
        With gvPegawai
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

    'Private Sub gvKodeReff_DataSourceChanged(ByVal sender As Object, ByVal e As System.EventArgs)
    '    If System.IO.File.Exists(folderLayouts &  Me.Name & gvKodeReff.Name & ".xml") Then
    '        gvKodeReff.RestoreLayoutFromXml(folderLayouts &  Me.Name & gvKodeReff.Name & ".xml")
    '    End If
    '    With gvKodeReff
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

    Private Sub txtGudang_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtGudang.EditValueChanged
        RubahGudang()
    End Sub

    Private Sub RubahGudang()
        Dim ds As New DataSet
        Try
            ds = ExecuteDataset("MGudang", "SELECT * FROM MGudang WHERE NoID=" & NullTolong(txtGudang.EditValue))
            If ds.Tables("MGudang").Rows.Count >= 1 Then
                txtPegawai.EditValue = NullTolong(ds.Tables("MGudang").Rows(0).Item("IDPenanggungJawab"))
            End If
        Catch ex As Exception
            XtraMessageBox.Show("Info " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
        Finally
            ds.Dispose()
        End Try
    End Sub

    Private Sub InsertIntoDetilTransferOut(ByVal IDReq As Long)
        Dim SQL As String = ""
        Dim ds As New DataSet
        Try
            If pTipe = pStatus.Edit Then
                SQL = "INSERT INTO MTransferOutD ([IDHeader],[IDPackingMutasiWilayahD],[IDPackingMutasiWilayah],[IDSPKMutasiWilayahD],[IDSPKMutasiWilayah],[IDRequestMutasiWilayahD],[IDRequestMutasiWilayah],[IDGudangDari],[IDBarang],[IDSatuan],[Konversi],[Qty],[QtyPCS],[Ctn],[Keterangan]) " & _
                      "SELECT " & NoID & ", MPackingMutasiWilayahD.NoID, MPackingMutasiWilayahD.IDHeader, MSPKMutasiWilayahD.NoID, MSPKMutasiWilayahD.IDHeader, MRequestMutasiWilayahD.NoID, MRequestMutasiWilayahD.IDHeader, MPackingMutasiWilayah.IDGudangDari, MPackingMutasiWilayahD.IDBarang, " & _
                      " MPackingMutasiWilayahD.IDSatuan,MPackingMutasiWilayahD.Konversi,MPackingMutasiWilayahD.Qty,MPackingMutasiWilayahD.QtyPCS,MPackingMutasiWilayahD.Ctn,MPackingMutasiWilayahD.Keterangan " & vbCrLf & _
                      " FROM MPackingMutasiWilayahD INNER JOIN MPackingMutasiWilayah ON MPackingMutasiWilayah.NoID=MPackingMutasiWilayahD.IDHeader" & vbCrLf & _
                      " LEFT JOIN MSPKMutasiWilayahD ON MSPKMutasiWilayahD.NoID=MPackingMutasiWilayahD.IDSPKMutasiWilayahD" & vbCrLf & _
                      " LEFT JOIN MRequestMutasiWilayahD ON MRequestMutasiWilayahD.NoID=MPackingMutasiWilayahD.IDRequestMutasiWilayahD" & vbCrLf & _
                      " WHERE MPackingMutasiWilayahD.IDHeader=" & IDReq & " AND (MPackingMutasiWilayahD.Qty*MPackingMutasiWilayahD.Konversi)-IsNull((SELECT SUM(A.Qty*A.Konversi) FROM MTransferOutD A WHERE A.IDPackingMutasiWilayahD=MPackingMutasiWilayahD.NoID),0)>0 "
                EksekusiSQL(SQL)
                'txtKodeReff.Text = txtKodeReff.Text & ", " & txtBarang.Text
                RefreshDetil()
                txtBarang.EditValue = -1
                RefreshLookUpPacking()

                txtKodeReff.Text = ""
                ds = ExecuteDataset("MKode", "SELECT MPackingMutasiWilayah.Kode FROM MPackingMutasiWilayah WHERE MPackingMutasiWilayah.NoID IN (SELECT MTransferOutD.IDPackingMutasiWilayah FROM MTransferOutD WHERE MTransferOutD.IDHeader=" & NoID & ")")
                If ds.Tables(0).Rows.Count >= 1 Then
                    For i As Integer = 0 To ds.Tables(0).Rows.Count - 1
                        txtKodeReff.Text = txtKodeReff.Text & IIf(txtKodeReff.Text.Length = 0, "", ", ") & NullTostr(ds.Tables(0).Rows(i).Item("Kode"))
                    Next
                End If
            End If
        Catch ex As Exception
            XtraMessageBox.Show("Kesalahan : " & ex.Message, "Fast Entri Says", MessageBoxButtons.OK)
        Finally
            ds.Dispose()
        End Try
    End Sub

    Private Sub txtWilayah_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtWilayah.EditValueChanged
        RefreshLookUpPacking()
    End Sub

    Private Sub txtWilayah_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtWilayah.LostFocus
        RefreshLookUpPacking()
    End Sub
    Private Sub RefreshLookUpPacking()
        Dim ds As New DataSet
        Try
            SQL = " SELECT NoID, NoPacking, Tanggal, SUM(Total) AS Total FROM (" & vbCrLf
            SQL &= " SELECT MPackingMutasiWilayah.NoID, MPackingMutasiWilayah.Kode AS NoPacking, MPackingMutasiWilayah.Tanggal, (MPackingMutasiWilayahD.Qty*MPackingMutasiWilayahD.Konversi)-IsNull((SELECT SUM(A.Qty*A.Konversi) FROM MTransferOutD A LEFT JOIN MTransferOut B ON B.NoID=A.IDHeader WHERE A.IDPackingMutasiWilayahD=MPackingMutasiWilayahD.NoID),0) AS Total" & vbCrLf
            SQL &= " FROM MPackingMutasiWilayahD INNER JOIN MPackingMutasiWilayah ON MPackingMutasiWilayah.NoID=MPackingMutasiWilayahD.IDHeader" & vbCrLf
            SQL &= " WHERE (MPackingMutasiWilayahD.Qty*MPackingMutasiWilayahD.Konversi)-IsNull((SELECT SUM(A.Qty*A.Konversi) FROM MTransferOutD A LEFT JOIN MTransferOut B ON B.NoID=A.IDHeader WHERE A.IDPackingMutasiWilayahD=MPackingMutasiWilayahD.NoID),0)>0 AND (MPackingMutasiWilayah.IsSelesai=0 OR MPackingMutasiWilayah.IsSelesai Is NULL) AND MPackingMutasiWilayah.IsPosted=1 AND MPackingMutasiWilayah.IDWilayahDari=" & NullTolong(txtWilayahDiminta.EditValue) & " AND MPackingMutasiWilayah.IDWilayahUntuk=" & NullTolong(txtWilayah.EditValue) & " ) X" & vbCrLf
            SQL &= " GROUP BY NoID, NoPacking, Tanggal"
            ds = ExecuteDataset("MSO", SQL)
            txtBarang.Properties.DataSource = ds.Tables("MSO")
            txtBarang.Properties.ValueMember = "NoID"
            txtBarang.Properties.DisplayMember = "NoPacking"
        Catch ex As Exception
            XtraMessageBox.Show("Info " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
        Finally
            ds.Dispose()
        End Try
    End Sub

    Private Sub GV1_CellValueChanged(ByVal sender As Object, ByVal e As DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs) Handles GV1.CellValueChanged
        Try
            If GV1.FocusedColumn.FieldName.ToLower = "Keterangan".ToLower Then
                If GV1.GetRowCellValue(GV1.FocusedRowHandle, "Keterangan").ToString.Length <= 100 Then
                    EksekusiSQL("UPDATE MTransferOutD SET Keterangan='" & FixApostropi(GV1.GetRowCellValue(GV1.FocusedRowHandle, "Keterangan").ToString) & "' WHERE NoID=" & NullTolong(GV1.GetRowCellValue(GV1.FocusedRowHandle, "NoID")))
                Else
                    XtraMessageBox.Show("Keterangan terlalu panjang.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
                End If
            End If
        Catch ex As Exception
            XtraMessageBox.Show("Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
        End Try
    End Sub

    Private Sub GV1_CellValueChanging(ByVal sender As Object, ByVal e As DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs) Handles GV1.CellValueChanging
        'If GV1.FocusedColumn.FieldName.ToLower = "Keterangan".ToLower Then
        '    GV1.OptionsBehavior.Editable = True
        'Else
        '    GV1.OptionsBehavior.Editable = False
        'End If
    End Sub

    Private Sub GV1_FocusedColumnChanged(ByVal sender As Object, ByVal e As DevExpress.XtraGrid.Views.Base.FocusedColumnChangedEventArgs) Handles GV1.FocusedColumnChanged
        If GV1.FocusedColumn.FieldName.ToLower = "Keterangan".ToLower Then
            GV1.OptionsBehavior.Editable = True
        Else
            GV1.OptionsBehavior.Editable = False
        End If
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

    Private Sub GC1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles GC1.Click

    End Sub
End Class