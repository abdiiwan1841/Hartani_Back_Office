﻿Imports System.Data
Imports System.Data.SqlClient
Imports DevExpress.XtraEditors
Imports DevExpress.Utils
Imports DevExpress.XtraGrid.Views.Base

Public Class frmEntriSPKMutasiWilayah
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

            SQL = "SELECT MGudang.NoID, MGudang.Kode, MGudang.Nama, MWilayah.Nama AS Wilayah FROM MGudang LEFT JOIN MWilayah ON MGudang.IDWilayah=MWilayah.NoID WHERE MGudang.IsActive=1 AND MGudang.InTransit=0 AND MGudang.IDWilayah=" & NullTolong(txtWilayahDiminta.EditValue)
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

            SQL = "SELECT MGudang.NoID, MGudang.Kode, MGudang.Nama, MWilayah.Nama AS Wilayah FROM MGudang LEFT JOIN MWilayah ON MGudang.IDWilayah=MWilayah.NoID WHERE MGudang.IsActive=1 AND MGudang.InTransit=0 AND MGudang.IDWilayah=" & NullTolong(txtWilayahDiminta.EditValue)
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

    Private Sub frmEntriPO_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        'Dim Sukses As Boolean = True
        'For i As Integer = 0 To GV1.RowCount - 1
        '    If NullTolong(GV1.GetRowCellValue(i, "IDRequestMutasi")) <> NullTolong(txtKodeReff.EditValue) Then
        '        XtraMessageBox.Show("Item stok " & GV1.GetRowCellValue(i, "NamaStock").ToString & " tidak sesuai dengan Kode Reff.",NamaAplikasi,MessageBoxButtons.OK,MessageBoxIcon.Information)
        '        GV1.ClearSelection()
        '        GV1.FocusedRowHandle = i
        '        Sukses = False
        '        Exit For
        '    End If
        'Next
        'If Not Sukses Then
        '    e.Cancel = True
        '    Exit Sub
        'End If
        If IsTempInsertBaru Then
            If XtraMessageBox.Show("Yakin ingin keluar?" & vbCrLf & "Ya, akan menghapus transaksi dan detil item.", NamaAplikasi, MessageBoxButtons.YesNo, MessageBoxIcon.Information, MessageBoxDefaultButton.Button2) = Windows.Forms.DialogResult.Yes Then
                EksekusiSQL("DELETE FROM MSPKMutasiWilayah WHERE NoID=" & NoID)
                EksekusiSQL("DELETE FROM MSPKMutasiWilayahD WHERE IDHeader=" & NoID)
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
                SQL = "SELECT MSPKMutasiWilayah.*, MUserEntri.Nama AS UserEntri, MUserEdit.Nama AS UserEdit, MUserPosting.Nama AS UserPosting, MAlamat.Alamat AS Alamat, MAlamat.Nama AS NamaAlamat, MAlamat.Kode as KodeAlamat "
                SQL &= " FROM MSPKMutasiWilayah LEFT JOIN MAlamat ON MAlamat.NoID=MSPKMutasiWilayah.IDPegawai "
                SQL &= " LEFT JOIN MUser MUserEntri ON MUserEntri.NoID=MSPKMutasiWilayah.IDUserEntry "
                SQL &= " LEFT JOIN MUser MUserEdit ON MUserEdit.NoID=MSPKMutasiWilayah.IDUserEdit "
                SQL &= " LEFT JOIN MUser MUserPosting ON MUserPosting.NoID=MSPKMutasiWilayah.IDUserPosting "
                SQL &= " WHERE MSPKMutasiWilayah.NoID = " & NoID
                DS = ExecuteDataset("Tabel", SQL)
                If DS.Tables("Tabel").Rows.Count >= 1 Then
                    txtKode.Text = NullTostr(DS.Tables(0).Rows(0).Item("Kode"))
                    KodeLama = txtKode.Text
                    txtWilayahDiminta.EditValue = NullTolong(DS.Tables(0).Rows(0).Item("IDWilayahDari"))
                    txtWilayah.EditValue = NullTolong(DS.Tables(0).Rows(0).Item("IDWilayahUntuk"))
                    txtKodeReff.EditValue = NullTolong(DS.Tables(0).Rows(0).Item("IDRequestMutasiWilayah"))
                    txtGudang.EditValue = NullTolong(DS.Tables(0).Rows(0).Item("IDGudangDari"))
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
        Windows.Forms.Cursor.Current = Cursors.WaitCursor
        Try
            strsql = "SELECT MSPKMutasiWilayahD.*, MRequestMutasiWilayahD.QtyPcs AS QtyRequest, MSPKMutasiWilayahD.SisaQtyPCS AS SisaRequest, MRequestMutasiWilayah.NoID AS IDRequestMutasi, MRequestMutasiWilayah.Kode AS NoRequestMutasi, MBarang.Nama AS NamaStock, MBarang.Kode AS KodeStock, MSatuan.Nama AS Satuan, MSatuan.Kode AS KodeSatuan " & vbCrLf & _
                     " FROM MSPKMutasiWilayahD " & vbCrLf & _
                     " LEFT JOIN (MRequestMutasiWilayahD LEFT JOIN MRequestMutasiWilayah ON MRequestMutasiWilayah.NoID=MRequestMutasiWilayahD.IDHeader) ON MRequestMutasiWilayahD.NoID=MSPKMutasiWilayahD.IDRequestMutasiWilayahD" & vbCrLf & _
                     " LEFT JOIN MBarang ON MBarang.NoID=MSPKMutasiWilayahD.IDBarang" & vbCrLf & _
                     " LEFT JOIN MSatuan ON MSatuan.NoID=MSPKMutasiWilayahD.IDSatuan" & vbCrLf & _
                     " WHERE MSPKMutasiWilayahD.IDHeader = " & NoID
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
            Next
            GV1.HideFindPanel()
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
                        If GV1.RowCount = 0 Then
                            InsertIntoDetilSPk(txtKodeReff.EditValue)
                        End If
                        IsTempInsertBaru = True
                        'txtKodeSupplier.Properties.ReadOnly = True
                    Else
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
    Private Function IsValidasi() As Boolean
        If txtKode.Text = "" Then
            XtraMessageBox.Show("Kode masih kosong.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
            txtKode.Focus()
            Return False
            Exit Function
        End If
        If txtPegawai.Text = "" Then
            XtraMessageBox.Show("Nama Penanggung Jawab Gudang masih kosong.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
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
            XtraMessageBox.Show("Gudang masih kosong.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
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
        If txtKodeReff.Text = "" Then
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
        Dim Sukses As Boolean = True
        For i As Integer = 0 To GV1.RowCount - 1
            If NullTolong(GV1.GetRowCellValue(i, "IDRequestMutasi")) <> NullTolong(txtKodeReff.EditValue) Then
                XtraMessageBox.Show("Item stok " & GV1.GetRowCellValue(i, "NamaStock").ToString & " tidak sesuai dengan Kode Reff.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
                GV1.ClearSelection()
                GV1.FocusedRowHandle = i
                Sukses = False
                Exit For
            End If
        Next
        If Not Sukses Then
            Return False
            Exit Function
        End If
        If CekKodeValid(txtKode.Text, KodeLama, "MSPKMutasiWilayah", "Kode", IIf(pTipe = pStatus.Edit, True, False)) Then
            XtraMessageBox.Show("Kode sudah dipakai.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
            If pTipe = pStatus.Baru Then
                If XtraMessageBox.Show("Lakukan pemberian kode baru?", NamaAplikasi, MessageBoxButtons.YesNo, MessageBoxIcon.Information) = Windows.Forms.DialogResult.Yes Then
                    txtKode.Text = clsKode.MintaKodeBaru("SPW", "MSPKMutasiWilayah", txtTgl.DateTime, NullTolong(txtWilayahDiminta.EditValue), 5)
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
                NoID = GetNewID("MSPKMutasiWilayah", "NoID")
                SQL = "INSERT INTO [MSPKMutasiWilayah] ([NoID],[IDRequestMutasiWilayah],[Nomor],[Kode],[KodeReff],[IDWilayahDari],[IDWilayahUntuk],[Tanggal],[Jam],[IDGudangDari],[IDPegawai],[IDUserEntry],[TglEntri],[IDUserEdit],[IDUserPosting],[IDAdmin],[TglPosting],[IsPosted]) VALUES (" & vbCrLf & _
                      NoID & ", " & _
                      NullTolong(txtKodeReff.EditValue) & "," & _
                      NullTolong(GetNewID("MSPKMutasiWilayah", "Nomor")) & ", " & _
                      "'" & FixApostropi(txtKode.Text) & "', " & _
                      "'" & FixApostropi(txtKodeReff.Text) & "', " & _
                      NullTolong(txtWilayahDiminta.EditValue) & "," & _
                      NullTolong(txtWilayah.EditValue) & "," & _
                      "'" & txtTgl.DateTime.ToString("yyyy-MM-dd") & "', " & _
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
                SQL = "UPDATE MSPKMutasiWilayah SET " & vbCrLf & _
                      "Kode='" & FixApostropi(txtKode.Text) & "', " & _
                      "IDRequestMutasiWilayah=" & NullTolong(txtKodeReff.EditValue) & ", " & _
                      "KodeReff='" & FixApostropi(txtKodeReff.Text) & "', " & _
                      "IDWilayahDari=" & NullTolong(txtWilayahDiminta.EditValue) & "," & _
                      "IDWilayahUntuk=" & NullTolong(txtWilayah.EditValue) & "," & _
                      "Tanggal='" & txtTgl.DateTime.ToString("yyyy-MM-dd") & "', " & _
                      "Jam='" & txtJam.DateTime.ToString("HH:mm") & "', " & _
                      "IDGudangDari=" & NullTolong(txtGudang.EditValue) & ", " & _
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
                gvKodeReff.SaveLayoutToXml(folderLayouts & Me.Name & gvKodeReff.Name & ".xml")
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
                EksekusiSQL("Delete From MSPKMutasiWilayahD where NoID=" & IDDetil.ToString)
                RefreshDetil()
            End If
            txtBarang.Focus()
        Catch ex As Exception
            XtraMessageBox.Show("Untuk menghapus pilih item barang yang akan dihapus terlebih dahulu lalu tekan tombol hapus", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub Tgl_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtTgl.EditValueChanged
        Dim TglAdd As Long = 0
        Try
            If pTipe = pStatus.Baru Then
                txtKode.Text = clsKode.MintaKodeBaru("SPW", "MSPKMutasiWilayah", txtTgl.DateTime, NullTolong(txtWilayahDiminta.EditValue), 5)
            End If
        Catch ex As Exception

        End Try
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
        Dim frmEntri As New frmEntriSPKMutasiWilayahD
        Dim Konversi As Double = 0.0
        Try
            If txtWilayahDiminta.Text <> "" Then
                If CariBarang(IDBarang, NamaBarang, KodeBarang) Then
                    If XtraMessageBox.Show("Ingin SPK Mutasi Kode Barang " & KodeBarang & " dengan Nama Barang " & NamaBarang, "Fast Entri Says", MessageBoxButtons.YesNo) = Windows.Forms.DialogResult.Yes Then
                        IDDetil = GetNewID("MSPKMutasiWilayahD", "NoID")
                        Konversi = NullToDbl(EksekusiSQlSkalarNew("SELECT Konversi FROM MBarangD WHERE IDBarang=" & IDBarang & " AND IDSatuan=" & DefIDSatuan))
                        If Konversi <> 0 Then
                            SQL = "INSERT INTO MSPKMutasiWilayahD (NoID,IDHeader,IDBarang,IDSatuan,Konversi) VALUES " & vbCrLf
                            SQL &= "(" & IDDetil & "," & NoID & "," & IDBarang & "," & DefIDSatuan & "," & FixKoma(Konversi) & ")"
                        Else
                            SQL = "INSERT INTO MSPKMutasiWilayahD (NoID,IDHeader,IDBarang) VALUES " & vbCrLf
                            SQL &= "(" & IDDetil & "," & NoID & "," & IDBarang & ")"
                        End If
                        EksekusiSQL(SQL)
                        frmEntri.IsNew = False
                        frmEntri.NoID = IDDetil
                        frmEntri.IDHeader = NoID
                        frmEntri.IDRequest = NullToLong(txtKodeReff.EditValue)
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
                        '    SQL = "DELETE FROM MSPKMutasiWilayahD WHERE NoID=" & IDDetil
                        '    EksekusiSQL(SQL)
                        '    RefreshDetil()
                        '    GV1.ClearSelection()
                        '    GV1.FocusedRowHandle = GV1.LocateByDisplayText(0, GV1.Columns("NoID"), IDDetil.ToString("#,##0"))
                        '    GV1.SelectRow(GV1.FocusedRowHandle)
                        'End If
                    End If
                End If
            Else
                XtraMessageBox.Show("Isi dulu wilayah yang dimintai, lalu click baru.", NamaAplikasi, MessageBoxButtons.OK)
            End If
        Catch ex As Exception
            XtraMessageBox.Show("Kesalahan : " & ex.Message, "Fast Entri Says", MessageBoxButtons.OK)
            'frmEntri.Dispose()
        End Try
    End Sub

    Private Sub cmdBAru_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdBAru.Click
        Dim frmEntri As New frmEntriSPKMutasiWilayahD
        Try
            Dim IDDetil As Long = -1
            frmEntri.NoID = IDDetil
            frmEntri.IsNew = True
            frmEntri.IDHeader = NoID
            If txtWilayahDiminta.Text <> "" Then
                frmEntri.IDRequest = NullTolong(txtKodeReff.EditValue)
                frmEntri.NoID = IDDetil
                frmEntri.IsNew = True
                frmEntri.FormPemanggil = Me
                frmEntri.Show()
                frmEntri.Focus()
                'If frmEntri.ShowDialog(Me)= Windows.Forms.DialogResult.OK Then
                '    RefreshDetil()
                'End If
                'txtBarang.Focus()
            Else
                XtraMessageBox.Show("Isi dulu wilayah yang dimintai, lalu click baru.", NamaAplikasi, MessageBoxButtons.OK)
            End If
        Catch ex As Exception
            XtraMessageBox.Show("Untuk membuat baru tekan tombol baru", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            'frmEntri.Dispose()
        End Try
    End Sub

    Private Sub cmdEdit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdEdit.Click
        Dim view As ColumnView = GC1.FocusedView
        Dim frmEntri As New frmEntriSPKMutasiWilayahD
        Try
            Dim row As System.Data.DataRow = view.GetDataRow(GV1.FocusedRowHandle)
            Dim dc As Integer = GV1.FocusedRowHandle
            Dim IDDetil As Long = NullTolong(row("NoID"))
            If txtWilayahDiminta.Text <> "" Then
                frmEntri.IDRequest = NullTolong(txtKodeReff.EditValue)
                frmEntri.IsNew = False
                frmEntri.NoID = IDDetil
                frmEntri.IDHeader = NoID
                frmEntri.FormPemanggil = Me
                frmEntri.Show()
                frmEntri.Focus()
                'If frmEntri.ShowDialog(Me)= Windows.Forms.DialogResult.OK Then
                '    RefreshDetil()
                'End If
                'txtBarang.Focus()
            Else
                XtraMessageBox.Show("Isi dulu wilayah yang dimintai, lalu click Edit.", NamaAplikasi, MessageBoxButtons.OK)
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
                InsertIntoDetil()
                txtBarang.Text = ""
                'txtBarang.Focus()
            Case Keys.Escape
                txtBarang.Text = ""
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
            txtKode.Text = clsKode.MintaKodeBaru("SPW", "MSPKMutasiWilayah", txtTgl.DateTime, NullTolong(txtWilayahDiminta.EditValue), 5)
        End If
        RefreshDataWilayahDiminta()
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

    Private Sub txtWilayah_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtWilayah.EditValueChanged
        RubahLookUp()
    End Sub

    Private Sub txtWilayah_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtWilayah.LostFocus
        RubahLookUp()
    End Sub
    Private Sub RubahLookUp()
        Dim ds As New DataSet
        Try
            SQL = "SELECT NoID, Kode, Tanggal, SUM(Sisa) AS Sisa " & _
                  " FROM (SELECT MRequestMutasiWilayah.NoID, MRequestMutasiWilayah.Kode, MRequestMutasiWilayah.Tanggal, " & _
                  " MRequestMutasiWilayahD.QtyPcs-IsNull((SELECT SUM(MSPKMutasiWilayahD.QtyPcs) FROM MSPKMutasiWilayahD WHERE MSPKMutasiWilayahD.IDRequestMutasiWilayahD=MRequestMutasiWilayahD.NoID),0) AS Sisa " & vbCrLf & _
                  " FROM MRequestMutasiWilayahD LEFT JOIN MRequestMutasiWilayah ON MRequestMutasiWilayah.NoID=MRequestMutasiWilayahD.IDHeader " & _
                  " WHERE MRequestMutasiWilayahD.QtyPcs-IsNull((SELECT SUM(MSPKMutasiWilayahD.QtyPcs) FROM MSPKMutasiWilayahD WHERE MSPKMutasiWilayahD.IDHeader<>" & NoID & " AND MSPKMutasiWilayahD.IDRequestMutasiWilayahD=MRequestMutasiWilayahD.NoID),0)>0 AND MRequestMutasiWilayah.IDWilayahDari=" & NullToLong(txtWilayahDiminta.EditValue) & " AND MRequestMutasiWilayah.IDWilayahUntuk=" & NullToLong(txtWilayah.EditValue) & " AND MRequestMutasiWilayah.IsAccPusat=1 AND MRequestMutasiWilayah.IsPosted=1 AND (MRequestMutasiWilayah.IsSelesai=0 OR MRequestMutasiWilayah.IsSelesai IS NULL) " & _
                  " GROUP BY MRequestMutasiWilayah.IsAccPusat, MRequestMutasiWilayah.NoID, MRequestMutasiWilayah.Kode, MRequestMutasiWilayah.Tanggal, MRequestMutasiWilayahD.NoID, MRequestMutasiWilayahD.QtyPcs) X " & vbCrLf & _
                  " GROUP BY NoID, Kode, Tanggal"
            ds = ExecuteDataset("master", SQL)
            txtKodeReff.Properties.DataSource = ds.Tables("master")
            txtKodeReff.Properties.ValueMember = "NoID"
            txtKodeReff.Properties.DisplayMember = "Kode"
            If System.IO.File.Exists(folderLayouts & Me.Name & gvKodeReff.Name & ".xml") Then
                gvKodeReff.RestoreLayoutFromXml(folderLayouts & Me.Name & gvKodeReff.Name & ".xml")
            End If
        Catch ex As Exception
            XtraMessageBox.Show("Info " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
        Finally
            ds.Dispose()
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

    Private Sub gvKodeReff_DataSourceChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvKodeReff.DataSourceChanged
        If System.IO.File.Exists(folderLayouts & Me.Name & gvKodeReff.Name & ".xml") Then
            gvKodeReff.RestoreLayoutFromXml(folderLayouts & Me.Name & gvKodeReff.Name & ".xml")
        End If
        With gvKodeReff
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

    Private Sub txtKodeReff_ButtonClick(ByVal sender As Object, ByVal e As DevExpress.XtraEditors.Controls.ButtonPressedEventArgs) Handles txtKodeReff.ButtonClick
        If pTipe = pStatus.Baru Then Exit Sub
        Select Case e.Button.Index
            Case 1
                InsertIntoDetilSPk(NullTolong(txtKodeReff.EditValue))
                RefreshDetil()
                txtKodeReff.Focus()
        End Select
    End Sub

    Private Sub txtKodeReff_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtKodeReff.EditValueChanged

    End Sub

    Private Sub txtKodeReff_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtKodeReff.KeyDown
        If pTipe = pStatus.Baru Then Exit Sub
        Select Case e.KeyCode
            Case Keys.Enter
                InsertIntoDetilSPK(NullTolong(txtKodeReff.EditValue))
                RefreshDetil()
                txtKodeReff.Focus()
        End Select
    End Sub

    Private Sub InsertIntoDetilSPk(ByVal IDReq As Long)
        Dim SQL As String = ""
        Dim ds As New DataSet
        Try
            SQL = "SELECT MRequestMutasiWilayahD.*, (MRequestMutasiWilayahD.Qty*MRequestMutasiWilayahD.Konversi)-IsNull((SELECT SUM(A.Qty*A.Konversi) FROM MSPKMutasiWilayahD A WHERE A.IDRequestMutasiWilayahD=MRequestMutasiWilayahD.NoID),0) AS SisaQty "
            SQL &= " FROM MRequestMutasiWilayahD WHERE MRequestMutasiWilayahD.IDHeader=" & IDReq & " AND (MRequestMutasiWilayahD.Qty*MRequestMutasiWilayahD.Konversi)-IsNull((SELECT SUM(A.Qty*A.Konversi) FROM MSPKMutasiWilayahD A WHERE A.IDRequestMutasiWilayahD=MRequestMutasiWilayahD.NoID),0)>0 "
            ds = ExecuteDataset("MRequestMutasiWilayah", SQL)
            With ds.Tables("MRequestMutasiWilayah")
                If .Rows.Count >= 1 Then
                    For i As Integer = 0 To .Rows.Count - 1
                        SQL = "INSERT INTO MSPKMutasiWilayahD (NoID ,IDHeader ,SisaQtyPCS ,IDRequestMutasiWilayahD ,IDBarang ,IDSatuan ,Konversi ,Qty ,QtyPCS ,Ctn ,Keterangan) VALUES (" & _
                              NullTolong(GetNewID("MSPKMutasiWilayahD", "NoID")) & ", " & NoID & ", " & FixKoma(0) & "," & NullTolong(.Rows(i).Item("NoID")) & ", " & NullTolong(.Rows(i).Item("IDBarang")) & ", " & NullTolong(.Rows(i).Item("IDSatuan")) & ", " & _
                              FixKoma(NullToDbl(.Rows(i).Item("Konversi"))) & ", " & FixKoma(NullToDbl(.Rows(i).Item("SisaQty")) / IIf(NullToDbl(.Rows(i).Item("Konversi")) = 0, 1, NullToDbl(.Rows(i).Item("Konversi")))) & ", " & FixKoma(NullToDbl(.Rows(i).Item("SisaQty"))) & ", " & FixKoma(NullToDbl(.Rows(i).Item("Ctn"))) & ", '" & FixApostropi(NullTostr(.Rows(i).Item("Keterangan"))) & "')"
                        EksekusiSQL(SQL)
                    Next
                End If
            End With
            RefreshDetil()
        Catch ex As Exception
            XtraMessageBox.Show("Kesalahan : " & ex.Message, "Fast Entri Says", MessageBoxButtons.OK)
        Finally
            ds.Dispose()
        End Try
    End Sub
End Class