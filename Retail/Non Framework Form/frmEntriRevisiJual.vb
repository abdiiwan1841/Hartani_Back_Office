Imports System.Data
Imports System.Data.SqlClient
Imports DevExpress.XtraEditors
Imports DevExpress.Utils
Imports DevExpress.XtraGrid.Views.Base

Public Class frmEntriRevisiJual
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
    Dim DefImageList As New ImageList
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
            SQL = "SELECT NoID, Kode, Nama FROM MAlamat WHERE IsActive=1 AND IsCustomer=1"
            ds = ExecuteDataset("master", SQL)
            txtKodeCustomer.Properties.DataSource = ds.Tables("master")
            If System.IO.File.Exists(FolderLayouts & Me.Name & SearchLookUpEdit1View.Name & ".xml") Then
                SearchLookUpEdit1View.RestoreLayoutFromXml(FolderLayouts & Me.Name & SearchLookUpEdit1View.Name & ".xml")
            End If
        Catch ex As Exception
            XtraMessageBox.Show("Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            ds.Dispose()
        End Try
    End Sub

    Private Sub frmEntriJual_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        If IsTempInsertBaru Then
            If XtraMessageBox.Show("Yakin ingin keluar?" & vbCrLf & "Ya, akan menghapus transaksi dan detil item.", NamaAplikasi, MessageBoxButtons.YesNo, MessageBoxIcon.Information, MessageBoxDefaultButton.Button2) = Windows.Forms.DialogResult.Yes Then
                EksekusiSQL("DELETE FROM MRevisiHargaJual WHERE NoID=" & NoID)
                EksekusiSQL("DELETE FROM MRevisiHargaJualD WHERE IDRevisiHargaJual=" & NoID)
            Else
                e.Cancel = True
            End If
        End If
    End Sub
    'Private Sub RefreshDataSTB()
    '    Dim ds As New DataSet
    '    Try
    '        SQL = "SELECT     MLPB.NoID, MLPB.Kode, MLPB.Tanggal, MAlamat.Nama AS Customer, MAlamat_1.Nama AS Penerima"
    '        SQL &= " FROM MAlamat AS MAlamat_1 RIGHT OUTER JOIN"
    '        SQL &= " MLPB ON MAlamat_1.NoID = MLPB.IDBagPeMRevisiHargaJualan LEFT OUTER JOIN"
    '        SQL &= " MAlamat ON MLPB.IDCustomer = MAlamat.NoID "
    '        SQL &= " WHERE MLPB.IDCustomer=" & NullTolong(txtKodeCustomer.EditValue)
    '        ds = ExecuteDataset("master", SQL)
    '        txtSTB.Properties.DataSource = ds.Tables("master")
    '        If System.IO.File.Exists(folderLayouts &  Me.Name & gvSTB.Name & ".xml") Then
    '            gvSTB.RestoreLayoutFromXml(folderLayouts &  Me.Name & gvSTB.Name & ".xml")
    '        End If
    '    Catch ex As Exception
    '        XtraMessageBox.Show("Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
    '    Finally
    '        ds.Dispose()
    '    End Try
    'End Sub

    Private Sub frmEntriJual_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
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
    Private Sub RefreshData()
        Dim DS As New DataSet
        Try
            RefreshDataKontak()
            'RefreshDataSTB()
            If pTipe = pStatus.Baru Then
                IsiDefault()
            Else
                SQL = "SELECT MRevisiHargaJual.*, MAlamat.Alamat AS Alamat, MAlamat.Nama AS NamaAlamat, MAlamat.Kode as KodeAlamat FROM MRevisiHargaJual LEFT JOIN MAlamat ON MAlamat.NoID=MRevisiHargaJual.IDCustomer WHERE MRevisiHargaJual.NoID=" & NoID
                DS = ExecuteDataset("Tabel", SQL)
                If DS.Tables("Tabel").Rows.Count >= 1 Then
                    txtKodeCustomer.EditValue = NullToLong(DS.Tables(0).Rows(0).Item("IDCustomer"))
                    'txtKodeCustomer.Text = NullTostr(DS.Tables(0).Rows(0).Item("KodeAlamat"))
                    txtNamaCustomer.Text = NullToStr(DS.Tables(0).Rows(0).Item("NamaAlamat"))
                    txtAlamatCustomer.Text = NullToStr(DS.Tables(0).Rows(0).Item("Alamat"))
                    txtKode.Text = NullToStr(DS.Tables(0).Rows(0).Item("Kode"))
                    KodeLama = txtKode.Text
                    txtKodeReff.Text = NullToStr(DS.Tables(0).Rows(0).Item("KodeReff"))
                    Tgl.DateTime = NullToDate(DS.Tables(0).Rows(0).Item("Tanggal"))
                    IsPosted = NullToBool(DS.Tables(0).Rows(0).Item("IsPosted"))
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
            strsql = "select MRevisiHargaJualD.*, MBarang.CtnPcs AS IsiCtn, MRevisiHargaJualD.Kode AS FakturJual, (MRevisiHargaJualD.Jumlah-MRevisiHargaJualD.Disc1-MRevisiHargaJualD.Disc2-MRevisiHargaJualD.Disc3)/MRevisiHargaJualD.Qty AS HargaNetto, (MRevisiHargaJualD.JumlahBaru-MRevisiHargaJualD.Disc1Baru-MRevisiHargaJualD.Disc2Baru-MRevisiHargaJualD.Disc3Baru)/MRevisiHargaJualD.Qty AS HargaNettoBaru, MRevisiHargaJualD.Jumlah-MRevisiHargaJualD.JumlahBaru AS [Total PH], MRevisiHargaJualD.Disc1 AS DiscRp, MRevisiHargaJualD.Disc1 AS DiscRpBaru, MBarang.Kode as KodeStock,MWilayah.Nama AS Wilayah, MBarang.Nama as NamaStock,MSatuan.Kode as Satuan,Mgudang.Kode as KodeGudang " & vbCrLf & _
            " From (MRevisiHargaJualD Inner Join MRevisiHargaJual On MRevisiHargaJualD.IDRevisiHargaJual=MRevisiHargaJual.NoID) " & vbCrLf & _
            " LEFT JOIN MBarang ON MRevisiHargaJualD.IDBarang=MBarang.NoID " & vbCrLf & _
            " LEFT JOIN MSatuan ON MRevisiHargaJualD.IDSatuan=MSatuan.NoID " & vbCrLf & _
            " LEFT JOIN (MGudang LEFT JOIN MWilayah ON MWilayah.NoID=MGudang.IDWilayah) ON MRevisiHargaJualD.IDGudang=MGudang.NoID " & vbCrLf & _
            " where MRevisiHargaJualD.IDRevisiHargaJual= " & NoID
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
            txtTotalCTN.EditValue = NullToDbl(EksekusiSQlSkalarNew("SELECT SUM(Ctn) FROM MRevisiHargaBeliD WHERE IDRevisiHargaBeli=" & NoID))
        Catch ex As Exception
            XtraMessageBox.Show(ex.Message, ".:: Pesan Kesalahan ::.", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
        Windows.Forms.Cursor.Current = curentcursor
        Application.DoEvents()
    End Sub

    Private Sub IsiDefault()
        Tgl.DateTime = TanggalSystem
        SetTombol()

        txtKodeCustomer.EditValue = DefIDCustomer
        RubahCustomer()
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
            If pTipe = pStatus.Baru Then
                txtKode.Text = clsKode.MintaKodeBaru("RHJL", "MRevisiHargaJual", Tgl.DateTime, DefIDWilayah, 5)
            End If
            If IsValidasi() Then
                If Simpan(PesanSalah) = True Then
                    If pTipe = pStatus.Baru Then
                        KodeLama = txtKode.Text
                        pTipe = pStatus.Edit
                        SetTombol()
                        txtBarang.Focus()
                        IsTempInsertBaru = True
                        'txtKodeCustomer.Properties.ReadOnly = True
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
        If txtKodeCustomer.Text = "" Then
            XtraMessageBox.Show("Customer masih kosong.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
            txtKodeCustomer.Focus()
            Return False
            Exit Function
        End If
        If Tgl.Text = "" Then
            XtraMessageBox.Show("Tanggal masih kosong.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
            Tgl.Focus()
            Return False
            Exit Function
        End If

        If CekKodeValid(txtKode.Text, KodeLama, "MRevisiHargaJual", "Kode", IIf(pTipe = pStatus.Edit, True, False)) Then
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

        Try
            If pTipe = pStatus.Baru Then
                NoID = GetNewID("MRevisiHargaJual")
                SQL = "INSERT INTO MRevisiHargaJual (NoID,IDWilayah,Kode,KodeReff,Tanggal,"
                SQL &= " IDCustomer) VALUES (" & vbCrLf
                SQL &= NoID & ","
                SQL &= DefIDWilayah & ","
                SQL &= "'" & FixApostropi(txtKode.Text) & "',"
                SQL &= "'" & FixApostropi(txtKodeReff.Text) & "',"
                SQL &= "'" & Tgl.DateTime.ToString("yyyy-MM-dd HH:mm") & "',"
                SQL &= txtKodeCustomer.EditValue & ") "
                EksekusiSQL(SQL)
            Else
                SQL = "UPDATE MRevisiHargaJual SET "
                If Not Tgl.Properties.ReadOnly Or Not txtKode.Properties.ReadOnly Then
                    SQL &= "IDAdmin=" & IDAdmin & ","
                End If
                SQL &= "Kode='" & FixApostropi(txtKode.Text) & "',"
                SQL &= "KodeReff='" & FixApostropi(txtKodeReff.Text) & "',"
                SQL &= "Tanggal='" & Tgl.DateTime.ToString("yyyy-MM-dd HH:mm") & "',"
                SQL &= "IDCustomer=" & txtKodeCustomer.EditValue & " "
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
            If XtraMessageBox.Show("Simpan Layout?", NamaAplikasi, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = Windows.Forms.DialogResult.Yes AndAlso xOtorisasi.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
                Ini.TulisIniPath(FolderLayouts & Me.Name & ".ini", "Form", "Width", Me.Width)
                Ini.TulisIniPath(FolderLayouts & Me.Name & ".ini", "Form", "Height", Me.Height)
                Me.Left = (Screen.PrimaryScreen.WorkingArea.Width - Me.Width) \ 2
                Me.Top = (Screen.PrimaryScreen.WorkingArea.Height - Me.Height) \ 2

                LayoutControl1.SaveLayoutToXml(FolderLayouts & Me.Name & LayoutControl1.Name & ".xml")
                GV1.SaveLayoutToXml(FolderLayouts & Me.Name & GV1.Name & ".xml")
                SearchLookUpEdit1View.SaveLayoutToXml(FolderLayouts & Me.Name & SearchLookUpEdit1View.Name & ".xml")
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
            If XtraMessageBox.Show("Item " & GV1.GetRowCellValue(GV1.FocusedRowHandle, "NamaStock") & " ini mau dihapus?", NamaAplikasi, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) = Windows.Forms.DialogResult.Yes Then
                EksekusiSQL("Delete From MRevisiHargaJualD where NoID=" & IDDetil.ToString)
                RefreshDetil()
            End If
            txtBarang.Focus()
        Catch ex As Exception
            XtraMessageBox.Show("Untuk mengedit pilih item satuan yang akan diedit terlebih dahulu lalu tekan tombol edit", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub txtKodeCustomer_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtKodeCustomer.EditValueChanged
        RubahCustomer()
    End Sub
    Private Sub RubahCustomer()
        Dim TglAdd As Long = 0
        Try
            txtNamaCustomer.Text = NullToStr(EksekusiSQlSkalarNew("SELECT Nama FROM MAlamat WHERE NoID=" & NullToLong(txtKodeCustomer.EditValue)))
            txtAlamatCustomer.Text = NullToStr(EksekusiSQlSkalarNew("SELECT Alamat FROM MAlamat WHERE NoID=" & NullToLong(txtKodeCustomer.EditValue)))
        Catch ex As Exception

        End Try
    End Sub
    Private Sub Tgl_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Tgl.EditValueChanged
        Try
            If pTipe = pStatus.Baru Then
                txtKode.Text = clsKode.MintaKodeBaru("RHJL", "MRevisiHargaJual", Tgl.DateTime, DefIDWilayah, 5)
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
        Dim frmEntri As New frmEntriRevisiJualD
        Dim Konversi As Double = 0.0
        Try
            If txtKodeCustomer.Text = "" Then Exit Sub
            If CariBarang(IDBarang, NamaBarang, KodeBarang) Then
                If XtraMessageBox.Show("Ingin Merubah Harga Kode Barang " & KodeBarang & " Nama Barang " & NamaBarang, "Fast Entri Says", MessageBoxButtons.YesNo) = Windows.Forms.DialogResult.Yes Then
                    IDDetil = GetNewID("MRevisiHargaJualD", "NoID")
                    Konversi = NullToDbl(EksekusiSQlSkalarNew("SELECT Konversi FROM MBarangD WHERE IsJual=1 AND IDBarang=" & IDBarang & " AND IDSatuan=" & DefIDSatuan))
                    If Konversi <> 0 Then
                        SQL = "INSERT INTO MRevisiHargaJualD (NoID,IDBarang,IDSatuan,Konversi,IDGudang,IDRevisiHargaJual,NoUrut) VALUES " & vbCrLf
                        SQL &= "(" & IDDetil & "," & IDBarang & "," & DefIDSatuan & "," & FixKoma(Konversi) & "," & DefIDGudang & "," & NoID & "," & GetNewID("MRevisiHargaJualD", "NoUrut", " WHERE IDRevisiHargaJual=" & NoID) & ")"
                    Else
                        SQL = "INSERT INTO MRevisiHargaJualD (NoID,IDBarang,IDGudang,IDRevisiHargaJual,NoUrut) VALUES " & vbCrLf
                        SQL &= "(" & IDDetil & "," & IDBarang & "," & DefIDGudang & "," & NoID & "," & GetNewID("MRevisiHargaJualD", "NoUrut", " WHERE IDRevisiHargaJual=" & NoID) & ")"
                    End If
                    EksekusiSQL(SQL)
                    frmEntri.IsNew = False
                    frmEntri.NoID = IDDetil
                    frmEntri.IDRevisiHargaJual = NoID
                    frmEntri.IDCustomer = NullToLong(txtKodeCustomer.EditValue)
                    frmEntri.IsFastEntri = True
                    frmEntri.IDBarangDef = IDBarang
                    frmEntri.FormPemanggil = Me
                    frmEntri.Show()
                    frmEntri.Focus()
                    'If frmEntri.ShowDialog(Me)= Windows.Forms.DialogResult.OK Then
                    '    RefreshDetil()
                    '    GV1.ClearSelection()
                    '    GV1.FocusedRowHandle = GV1.LocateByDisplayText(0, GV1.Columns("NoID"), IDDetil.ToString("#,##0"))
                    '    GV1.SelectRow(GV1.FocusedRowHandle)
                    'Else
                    '    SQL = "DELETE FROM MRevisiHargaJualD WHERE NoID=" & IDDetil
                    '    EksekusiSQL(SQL)
                    '    RefreshDetil()
                    '    GV1.ClearSelection()
                    '    GV1.FocusedRowHandle = GV1.LocateByDisplayText(0, GV1.Columns("NoID"), IDDetil.ToString("#,##0"))
                    '    GV1.SelectRow(GV1.FocusedRowHandle)
                    'End If
                End If
            End If
            'txtBarang.Focus()
        Catch ex As Exception
            XtraMessageBox.Show("Kesalahan : " & ex.Message, "Fast Entri Says", MessageBoxButtons.OK)
            'frmEntri.Dispose()
        End Try
    End Sub

    Private Sub cmdBAru_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdBAru.Click
        Dim frmEntri As New frmEntriRevisiJualD
        Try
            Dim IDDetil As Long = -1
            frmEntri.NoID = IDDetil
            frmEntri.IsNew = True
            frmEntri.IDRevisiHargaJual = NoID
            If txtKodeCustomer.Text <> "" Then
                frmEntri.IDCustomer = NullToLong(txtKodeCustomer.EditValue)
                frmEntri.NoID = IDDetil
                frmEntri.IsNew = True
                frmEntri.IDRevisiHargaJual = NoID
                frmEntri.FormPemanggil = Me
                frmEntri.Show()
                frmEntri.Focus()
                'If frmEntri.ShowDialog(Me)= Windows.Forms.DialogResult.OK Then
                '    RefreshDetil()
                'End If
                'txtBarang.Focus()
            Else
                XtraMessageBox.Show("Isi dulu Customer, lalu click baru.", NamaAplikasi, MessageBoxButtons.OK)
            End If
        Catch ex As Exception
            XtraMessageBox.Show("Untuk membuat baru tekan tombol baru", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            'frmEntri.Dispose()
        End Try
    End Sub

    Private Sub cmdEdit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdEdit.Click
        Dim view As ColumnView = GC1.FocusedView
        Dim frmEntri As New frmEntriRevisiJualD
        Try
            Dim row As System.Data.DataRow = view.GetDataRow(GV1.FocusedRowHandle)
            Dim dc As Integer = GV1.FocusedRowHandle
            Dim IDDetil As Long = NullToLong(row("NoID"))
            If txtKodeCustomer.Text <> "" Then
                frmEntri.IDCustomer = NullToLong(txtKodeCustomer.EditValue)
                frmEntri.IsNew = False
                frmEntri.NoID = IDDetil
                frmEntri.IDRevisiHargaJual = NoID
                frmEntri.FormPemanggil = Me
                frmEntri.Show()
                frmEntri.Focus()
                'If frmEntri.ShowDialog(Me)= Windows.Forms.DialogResult.OK Then
                '    RefreshDetil()
                'End If
                'txtBarang.Focus()
            Else
                XtraMessageBox.Show("Isi dulu Customer, lalu click Edit.", NamaAplikasi, MessageBoxButtons.OK)
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

    Private Sub Tgl_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Tgl.KeyDown
        If e.KeyCode = Keys.F7 Then
            Dim x As New frmOtorisasiAdmin
            Try
                If Tgl.Properties.ReadOnly Then
                    If x.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
                        IDAdmin = x.IDUserAdmin
                        Tgl.Properties.ReadOnly = False
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
End Class