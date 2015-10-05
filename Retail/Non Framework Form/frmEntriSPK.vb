Imports System.Data
Imports System.Data.SqlClient
Imports DevExpress.XtraEditors
Imports DevExpress.Utils
Imports DevExpress.XtraGrid.Views.Base

Public Class frmEntriSPK
    Dim SQL As String = ""
    Public Enum pStatus
        Baru = 0
        Edit = 1
    End Enum
    Dim IDadmin As Long = -1
    Public Enum SPK
        IsSPK = 0
        IsPacking = 1
    End Enum
    Public pSPK As SPK

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
    Dim IsTempInsertBaru As Boolean = False
    Dim IsLoad As Boolean = True

    Private Sub RefreshDataKontak()
        Dim ds As New DataSet
        Try
            SQL = "SELECT NoID, Kode, Nama FROM MAlamat WHERE IsActive=1 AND IsCustomer=1"
            ds = ExecuteDataset("master", SQL)
            txtKodeCustomer.Properties.DataSource = ds.Tables("master")
            If System.IO.File.Exists(folderLayouts & Me.Name & SearchLookUpEdit1View.Name & ".xml") Then
                SearchLookUpEdit1View.RestoreLayoutFromXml(folderLayouts & Me.Name & SearchLookUpEdit1View.Name & ".xml")
            End If
            'SQL = "SELECT MGudang.NoID, MGudang.Kode, MGudang.Kode + ' - ' + MGudang.Nama AS Nama, MWilayah.Nama AS Wilayah FROM MGudang LEFT JOIN MWilayah ON MGudang.IDWilayah=MWilayah.NoID WHERE MGudang.IsActive=1 AND MGudang.InTransit=0 AND MGudang.IDWilayah=" & DefIDWilayah
            'ds = ExecuteDataset("master", SQL)
            'txtGudang.Properties.DataSource = ds.Tables("master")
            'txtGudang.Properties.ValueMember = "NoID"
            'txtGudang.Properties.DisplayMember = "Nama"
            'If System.IO.File.Exists(FolderLayouts & Me.Name & gvGudang.Name & ".xml") Then
            '    gvGudang.RestoreLayoutFromXml(FolderLayouts & Me.Name & gvGudang.Name & ".xml")
            'End If
        Catch ex As Exception
            XtraMessageBox.Show("Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            ds.Dispose()
        End Try
    End Sub
    Dim DefImageList As New ImageList

    Private Sub frmEntriSPK_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        If IsTempInsertBaru Then
            If XtraMessageBox.Show("Yakin ingin keluar?" & vbCrLf & "Ya, akan menghapus transaksi dan detil item.", NamaAplikasi, MessageBoxButtons.YesNo, MessageBoxIcon.Information, MessageBoxDefaultButton.Button2) = Windows.Forms.DialogResult.Yes Then
                EksekusiSQL("DELETE FROM MSPK WHERE NoID=" & NoID)
                EksekusiSQL("DELETE FROM MSPKD WHERE IDSPK=" & NoID)
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
            RefreshStok()
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
            If System.IO.File.Exists(folderLayouts & Me.Name & SearchLookUpEdit1View.Name & ".xml") Then
                SearchLookUpEdit1View.RestoreLayoutFromXml(folderLayouts & Me.Name & SearchLookUpEdit1View.Name & ".xml")
            End If
            If System.IO.File.Exists(folderLayouts & Me.Name & GridView1.Name & ".xml") Then
                GridView1.RestoreLayoutFromXml(folderLayouts & Me.Name & GridView1.Name & ".xml")
            End If
            FungsiControl.SetForm(Me)
        Catch ex As Exception
            XtraMessageBox.Show(ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            IsLoad = False
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
                SQL = "SELECT MSPK.*, MWilayah.Nama AS Wilayah, MUserEntri.Nama AS UserEntri, MUserEdit.Nama AS UserEdit, MUserPosting.Nama AS UserPosting, MAlamat.Nama AS NamaAlamat, MAlamat.Kode as KodeAlamat "
                SQL &= " FROM MSPK LEFT JOIN MAlamat ON MAlamat.NoID=MSPK.IDCustomer "
                SQL &= " LEFT JOIN MUser MUserEntri ON MUserEntri.NoID=MSPK.IDUserEntry "
                SQL &= " LEFT JOIN (MGudang LEFT JOIN MWilayah ON MWilayah.NoID=MGudang.IDWilayah) ON MGudang.NoID=MSPK.IDGudang "
                SQL &= " LEFT JOIN MUser MUserEdit ON MUserEdit.NoID=MSPK.IDUserEdit "
                SQL &= " LEFT JOIN MUser MUserPosting ON MUserPosting.NoID=MSPK.IDUserPosting "
                SQL &= " WHERE MSPK.NoID = " & NoID
                DS = ExecuteDataset("Tabel", SQL)
                If DS.Tables("Tabel").Rows.Count >= 1 Then
                    txtKodeCustomer.EditValue = NullTolong(DS.Tables(0).Rows(0).Item("IDCustomer"))
                    'txtGudang.EditValue = NullToLong(DS.Tables(0).Rows(0).Item("IDGudang"))
                    'txtWilayah.Text = NullToStr(DS.Tables(0).Rows(0).Item("Wilayah"))
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
                    RefreshSO()
                    'txtSO.EditValue = NullTolong(DS.Tables(0).Rows(0).Item("IDSO"))
                    txtDientriOleh.Text = NullTostr(DS.Tables(0).Rows(0).Item("UserEntri"))
                    txtDieditOleh.Text = NullTostr(DS.Tables(0).Rows(0).Item("UserEdit"))
                    txtDipostingOleh.Text = NullTostr(DS.Tables(0).Rows(0).Item("UserPosting"))
                    tglEntri.DateTime = NullToDate(DS.Tables(0).Rows(0).Item("TglEntri"))
                    tglEdit.DateTime = NullToDate(DS.Tables(0).Rows(0).Item("TglEdit"))
                    tglPosting.DateTime = NullToDate(DS.Tables(0).Rows(0).Item("TglPosting"))
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
            strsql = "select MSPKD.*, MGudang.Nama AS Gudang, MBarang.CtnPcs AS IsiCtn, MSO.Kode AS NoSO, MWilayah.Nama AS Wilayah, MBarang.Kode as KodeStock,MBarang.Nama as NamaStock,MSatuan.Kode as Satuan,Mgudang.Kode as KodeGudang " & vbCrLf
            strsql &= " From (MSPKD Inner Join MSPK On MSPKD.IDSPK=MSPK.NoID) " & vbCrLf
            strsql &= " LEFT JOIN MBarang ON MSPKD.IDBarang=MBarang.NoID " & vbCrLf
            strsql &= " LEFT JOIN MSatuan ON MSPKD.IDSatuan=MSatuan.NoID " & vbCrLf
            strsql &= " LEFT JOIN (MSOD LEFT JOIN MSO ON MSO.NoID=MSOD.IDSO) ON MSPKD.IDSOD=MSOD.NoID " & vbCrLf
            strsql &= " LEFT JOIN (MGudang LEFT JOIN MWilayah ON MWilayah.NoID=MGudang.IDWilayah) ON MSPKD.IDGudang=MGudang.NoID " & vbCrLf
            strsql &= " where MSPKD.IDSPK = " & NoID
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
            RefreshStok()
            GV1.HideFindPanel()
            If pTipe = pStatus.Edit Then
                RefreshSO()
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
            txtTotalCTN.EditValue = NullToDbl(EksekusiSQlSkalarNew("SELECT SUM(Ctn) FROM MSPKD WHERE IDSPK=" & NoID))
            txtKodeReff.Text = ""
            SQL = "SELECT MSO.Kode FROM MSPKD LEFT JOIN (MSOD INNER JOIN MSO ON MSO.NoID=MSOD.IDSO) ON MSOD.NoID=MSPKD.IDSOD WHERE MSPKD.IDSPK=" & NoID & " GROUP BY MSO.Kode ORDER BY MSO.Kode"
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
        'txtGudang.EditValue = DefIDGudang
        'RubahGudang()
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

        If pSPK = SPK.IsPacking Then
            If pTipe = pStatus.Baru Or IsPosted Then
                cmdBAru.Enabled = False
                cmdEdit.Enabled = False
                cmdDelete.Enabled = False
            Else
                cmdBAru.Enabled = False
                cmdEdit.Enabled = True
                cmdDelete.Enabled = False
            End If
        Else
            If pTipe = pStatus.Baru Or IsPosted Then
                cmdBAru.Enabled = False
                cmdEdit.Enabled = False
                cmdDelete.Enabled = False
            Else
                cmdBAru.Enabled = True
                cmdEdit.Enabled = True
                cmdDelete.Enabled = True
            End If
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
                        RefreshSO()
                        txtSO.Focus()
                        'txtKodeCustomer.Properties.ReadOnly = True
                        'txtGudang.Properties.ReadOnly = True
                        InsertIntoDetilSO(NullToLong(txtSO.EditValue))
                        RefreshSO()
                        txtSO.EditValue = -1
                        txtSO.Focus()
                        IsTempInsertBaru = True
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
        Dim SisaQty As Double = 0.0
        If txtKode.Text = "" Then
            XtraMessageBox.Show("Kode masih kosong.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
            txtKode.Focus()
            Return False
            Exit Function
        End If
        'If txtGudang.Text = "" Then
        '    XtraMessageBox.Show("Gudang masih kosong.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
        '    txtGudang.Focus()
        '    Return False
        '    Exit Function
        'End If
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
        If CekKodeValid(txtKode.Text, KodeLama, "MSPK", "Kode", IIf(pTipe = pStatus.Edit, True, False)) Then
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
        If pTipe = pStatus.Edit Then
            RefreshDetil()
            For i As Integer = 0 To GV1.RowCount - 1
                SisaQty = NullToDbl(EksekusiSQlSkalarNew("SELECT SUM((QtyMasuk-QtyKeluar)*Konversi) FROM MKartuStok WHERE IDBarang=" & NullToLong(GV1.GetRowCellValue(i, "IDBarang")) & " AND IDGudang=" & NullToLong(GV1.GetRowCellValue(i, "IDGudang"))))
                SQL = "SELECT SUM(Jumlah) FROM (SELECT (MSPKD.Qty*MSPKD.Konversi)-IsNull((SELECT SUM(MPackingD.Qty*MPackingD.Konversi) FROM MPackingD INNER JOIN MPacking ON MPacking.NoID=MPackingD.IDPacking WHERE MPackingD.IDSPKD=MSPKD.NoID),0) AS Jumlah FROM MSPKD INNER JOIN MSPK ON MSPK.NoID=MSPKD.IDSPK WHERE MSPKD.IDBarang=" & NullToLong(GV1.GetRowCellValue(i, "IDBarang")) & " AND MSPKD.IDGudang=" & NullToLong(GV1.GetRowCellValue(i, "IDGudang")) & ") X"
                SisaQty = SisaQty - NullToDbl(EksekusiSQlSkalarNew(SQL))
                If SisaQty < 0 Then
                    If XtraMessageBox.Show("Qty " & GV1.GetRowCellValue(i, "NamaStock") & " melebihi sisa stock digudang " & NullToStr(GV1.GetRowCellValue(i, "Gudang")) & "." & vbCrLf & "Yakin ingin melanjutkan penyimpanan?", NamaAplikasi, MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) = Windows.Forms.DialogResult.No Then
                        Return False
                        Exit Function
                    End If
                End If
            Next
        End If
        Return True
    End Function
    Dim KodeLama As String = ""
    Public Function Simpan(ByRef PesanSalah As String) As Boolean
        Dim Sukses As Boolean = False
        HitungTotal()
        Try
            If pTipe = pStatus.Baru Then
                NoID = GetNewID("MSPK")
                SQL = "INSERT INTO MSPK (IDWilayah,NoID,Kode,KodeReff,Tanggal,TanggalStock,JatuhTempo,"
                SQL &= " IDCustomer,TanggalSJ,NoSJ,SubTotal,DiskonNotaProsen,DiskonNotaRp,DiskonNotaTotal,"
                SQL &= "  Biaya, Total, Bayar, Sisa,IDUserEntry,IDUserEdit,IDUserPosting,TglEntri,IDAdmin) VALUES (" & vbCrLf
                SQL &= DefIDWilayah & ","
                'SQL &= NullTolong(txtGudang.EditValue) & ","
                SQL &= NoID & ","
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
                SQL &= "'" & TanggalSystem.ToString("yyyy-MM-dd HH:mm") & "'," & IDadmin & ")"
                EksekusiSQL(SQL)
            Else
                SQL = "UPDATE MSPK SET "
                If Not Tgl.Properties.ReadOnly Or Not txtKode.Properties.ReadOnly Then
                    SQL &= "IDAdmin=" & IDadmin & ","
                End If
                SQL &= "Kode='" & FixApostropi(txtKode.Text) & "',"
                'SQL &= "IDGudang=" & NullTolong(txtGudang.EditValue) & ","
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
            'If Sukses Then
            '    SQL = "UPDATE MSPKD SET IDGudang=" & NullToLong(txtGudang.EditValue) & " WHERE IDSPK=" & NoID
            '    EksekusiSQL(SQL)
            'End If
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

                LayoutControl1.SaveLayoutToXml(folderLayouts & Me.Name & LayoutControl1.Name & ".xml")
                GV1.SaveLayoutToXml(folderLayouts & Me.Name & GV1.Name & ".xml")
                SearchLookUpEdit1View.SaveLayoutToXml(folderLayouts & Me.Name & SearchLookUpEdit1View.Name & ".xml")
                GridView1.SaveLayoutToXml(folderLayouts & Me.Name & GridView1.Name & ".xml")
                gvSO.SaveLayoutToXml(folderLayouts & Me.Name & gvSO.Name & ".xml")
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
                EksekusiSQL("Delete From MSPKD where NoID=" & IDDetil.ToString)
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
            txtNamaCustomer.Text = NullTostr(EksekusiSQlSkalarNew("SELECT Nama FROM MAlamat WHERE NoID=" & NullTolong(txtKodeCustomer.EditValue)))
            txtAlamatCustomer.Text = NullTostr(EksekusiSQlSkalarNew("SELECT Alamat FROM MAlamat WHERE NoID=" & NullTolong(txtKodeCustomer.EditValue)))
            TglAdd = NullTolong(EksekusiSQlSkalarNew("SELECT JatuhTempoCustomer FROM MAlamat WHERE NoID=" & NullTolong(txtKodeCustomer.EditValue)))
            'If pTipe = pStatus.Edit Then
            RefreshSO()
            'End If
            If TglAdd = 0 Then
                tglJatuhTempo.DateTime = DateAdd(DateInterval.Month, 2, Tgl.DateTime)
            Else
                tglJatuhTempo.DateTime = DateAdd(DateInterval.Day, TglAdd, Tgl.DateTime)
            End If
        Catch ex As Exception

        End Try
    End Sub
    Private Sub RefreshSO()
        Dim ds As New DataSet
        Try
            SQL = " SELECT NoID, NoSO, Tanggal, SUM(Total) AS Total FROM (" & vbCrLf
            SQL &= " SELECT MSO.NoID, MSO.Kode AS NoSO, MSO.Tanggal, (MSOD.Qty*MSOD.Konversi)-IsNull((SELECT SUM(A.Qty*A.Konversi) FROM MSPKD A LEFT JOIN MSPK B ON B.NoID=A.IDSPK WHERE A.IDSOD=MSOD.NoID),0) AS Total" & vbCrLf
            SQL &= " FROM MSOD INNER JOIN MSO ON MSO.NoID=MSOD.IDSO" & vbCrLf
            SQL &= " WHERE (MSOD.Qty*MSOD.Konversi)-IsNull((SELECT SUM(A.Qty*A.Konversi) FROM MSPKD A LEFT JOIN MSPK B ON B.NoID=A.IDSPK WHERE A.IDSOD=MSOD.NoID),0)>0 AND (MSO.IsSelesai=0 OR MSO.IsSelesai Is NULL) AND MSO.IsPosted=1 AND MSO.IDWilayah=" & DefIDWilayah & " AND MSO.IDCustomer=" & NullTolong(txtKodeCustomer.EditValue) & " ) X" & vbCrLf
            SQL &= " GROUP BY NoID, NoSO, Tanggal"
            ds = ExecuteDataset("MSO", SQL)
            txtSO.Properties.DataSource = ds.Tables("MSO")
            txtSO.Properties.ValueMember = "NoID"
            txtSO.Properties.DisplayMember = "NoSO"
        Catch ex As Exception
            XtraMessageBox.Show("Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            ds.Dispose()
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
                txtKode.Text = clsKode.MintaKodeBaru("SPK", "MSPK", Tgl.DateTime, DefIDWilayah, 5)
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
        Dim frmEntri As New frmEntriSPKD
        Dim Konversi As Double = 0.0

        Try
            If pSPK = SPK.IsPacking Then XtraMessageBox.Show("Packing tidak boleh menambah baru.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error) : Exit Sub
            If txtKodeCustomer.Text = "" Then XtraMessageBox.Show("Customer masih kosong.", NamaBarang) : txtKodeCustomer.Focus() : Exit Sub
            If CariBarang(IDBarang, NamaBarang, KodeBarang) Then
                If XtraMessageBox.Show("Ingin menambah Item SPK dengan Kode Stock " & KodeBarang & " Nama Stock " & NamaBarang, "Fast Entri Says", MessageBoxButtons.YesNo) = Windows.Forms.DialogResult.Yes Then
                    IDDetil = GetNewID("MSPKD", "NoID")
                    Konversi = NullToDbl(EksekusiSQlSkalarNew("SELECT Konversi FROM MBarangD WHERE IsJual=1 AND IDBarang=" & IDBarang & " AND IDSatuan=" & DefIDSatuan))
                    If Konversi <> 0 Then
                        SQL = "INSERT INTO MSPKD (NoID,IDBarang,IDSatuan,IDGudang,Konversi,IDSPK,NoUrut,Tgl,Jam) VALUES " & vbCrLf
                        SQL &= "(" & IDDetil & "," & IDBarang & "," & DefIDSatuan & "," & NullToLong(-1) & "," & FixKoma(Konversi) & "," & NoID & "," & GetNewID("MSPKD", "NoUrut", " WHERE IDSPK=" & NoID) & ",GetDate(),GetDate())"
                    Else
                        SQL = "INSERT INTO MSPKD (NoID,IDBarang,IDGudang,IDSPK,NoUrut,Tgl,Jam) VALUES " & vbCrLf
                        SQL &= "(" & IDDetil & "," & IDBarang & "," & NoID & "," & NullToLong(-1) & "," & GetNewID("MSPKD", "NoUrut", " WHERE IDSPK=" & NoID) & ",GetDate(),GetDate())"
                    End If
                    EksekusiSQL(SQL)
                    frmEntri.IsNew = False
                    frmEntri.NoID = IDDetil
                    frmEntri.IDSPK = NoID
                    frmEntri.IDCustomer = NullToLong(txtKodeCustomer.EditValue)
                    'frmEntri.IDGudang = NullToLong(txtGudang.EditValue)
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
                    '    SQL = "DELETE FROM MSPKD WHERE NoID=" & IDDetil
                    '    EksekusiSQL(SQL)
                    '    RefreshDetil()
                    '    GV1.ClearSelection()
                    '    GV1.FocusedRowHandle = GV1.LocateByDisplayText(0, GV1.Columns("NoID"), IDDetil.ToString("#,##0"))
                    '    GV1.SelectRow(GV1.FocusedRowHandle)
                    'End If
                End If
            End If
        Catch ex As Exception
            XtraMessageBox.Show("Kesalahan : " & ex.Message, "Fast Entri Says", MessageBoxButtons.OK)
            'frmEntri.Dispose()
        End Try
    End Sub

    Private Sub InsertIntoDetilSO(ByVal IDSO As Long)
        Dim SQL As String = ""
        Dim ds As New DataSet
        Dim IDGudang As Long = -1
        Dim Qty As Double = 0
        Try
            If pSPK = SPK.IsPacking Then XtraMessageBox.Show("Packing tidak boleh menambah baru.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error) : Exit Sub
            If pTipe = pStatus.Edit AndAlso Not IsPosted Then
                SQL = "SELECT MSOD.*, MBarang.CtnPcs, (MSOD.Qty*MSOD.Konversi)-IsNull((SELECT SUM(A.Qty*A.Konversi) FROM MSPKD A WHERE A.IDSOD=MSOD.NoID),0) AS SisaQty "
                SQL &= " FROM MSOD LEFT JOIN MBarang ON MBarang.NoID=MSOD.IDBarang WHERE MSOD.IDSO=" & IDSO & " AND (MSOD.Qty*MSOD.Konversi)-IsNull((SELECT SUM(A.Qty*A.Konversi) FROM MSPKD A WHERE A.IDSOD=MSOD.NoID),0)>0 "
                ds = ExecuteDataset("MSO", SQL)
                With ds.Tables("MSO")
                    If .Rows.Count >= 1 Then
                        For i As Integer = 0 To .Rows.Count - 1
                            'txtQty.EditValue / IsiKarton * txtKonversi.EditValue
                            Qty = NullToDbl(.Rows(i).Item("SisaQty"))
                            'CariGudang(IDGudang, Qty)
                            SQL = "INSERT INTO MSPKD ([NoID],[Tgl],[Jam],[IDSPK],[IDBarang],[IDSatuan],[IDGudang],[Ctn],[Qty],[Harga],[DiscPersen1],[DiscPersen2],[DiscPersen3],[Disc1],[Disc2],[Disc3],[Jumlah]," & _
                                 "[IDSOD],[Catatan],[NoUrut],[HargaPcs],[QtyPcs],[Konversi],[IsPacking],[NoPacking],[QtyKarton]) VALUES (" & _
                                  NullToLong(GetNewID("MSPKD", "NoID")) & ", getdate(), getdate(), " & NoID & ", " & NullToLong(.Rows(i).Item("IDBarang")) & ", " & NullToLong(.Rows(i).Item("IDSatuan")) & ", " & _
                                  NullToLong(-1) & ", " & FixKoma(IIf(NullToDbl(.Rows(i).Item("CtnPcs")) = 0, 0, NullToDbl(.Rows(i).Item("SisaQty")) / NullToDbl(.Rows(i).Item("CtnPcs")))) & ", " & FixKoma(NullToDbl(.Rows(i).Item("SisaQty")) / IIf(NullToDbl(.Rows(i).Item("Konversi")) = 0, 1, NullToDbl(.Rows(i).Item("Konversi")))) & ", " & FixKoma(NullToDbl(.Rows(i).Item("Harga"))) & ", " & _
                                  FixKoma(NullToDbl(.Rows(i).Item("DiscPersen1"))) & ", " & FixKoma(NullToDbl(.Rows(i).Item("DiscPersen2"))) & ", " & FixKoma(NullToDbl(.Rows(i).Item("DiscPersen3"))) & ", " & _
                                  FixKoma(NullToDbl(.Rows(i).Item("Disc1"))) & ", " & FixKoma(NullToDbl(.Rows(i).Item("Disc2"))) & ", " & FixKoma(NullToDbl(.Rows(i).Item("Disc3"))) & ", " & _
                                  FixKoma(NullToDbl(.Rows(i).Item("Jumlah")) / NullToDbl(.Rows(i).Item("Qty")) * NullToDbl(.Rows(i).Item("SisaQty")) / NullToDbl(.Rows(i).Item("Konversi"))) & ", " & NullToLong(.Rows(i).Item("NoID")) & ", '" & FixApostropi(NullToStr(.Rows(i).Item("Catatan"))) & "', " & NullToLong(GetNewID("MSPKD", "NoUrut", " WHERE IDSPK=" & NoID)) & ", " & _
                                  FixKoma(NullToDbl(.Rows(i).Item("HargaPcs"))) & ", " & FixKoma(NullToDbl(.Rows(i).Item("QtyPcs"))) & ", " & FixKoma(NullToDbl(.Rows(i).Item("Konversi"))) & ", 0,'',0)"
                            EksekusiSQL(SQL)
                        Next
                    End If
                End With
                RefreshDetil()
            End If
        Catch ex As Exception
            XtraMessageBox.Show("Kesalahan : " & ex.Message, "Fast Entri Says", MessageBoxButtons.OK)
        Finally
            ds.Dispose()
        End Try
    End Sub
    Public Sub LoadDetilSO()
        Dim SQL As String = ""
        Dim ds As New DataSet
        Try
            'If pSPK = SPK.IsPacking Then XtraMessageBox.Show("Packing tidak boleh menambah baru.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error) : Exit Sub
            If pTipe = pStatus.Edit AndAlso Not IsPosted Then
                SQL = "SELECT MSOD.*, MBarang.CtnPcs, (MSOD.Qty*MSOD.Konversi)-IsNull((SELECT SUM(A.Qty*A.Konversi) FROM MSPKD A WHERE A.IDSOD=MSOD.NoID),0) AS SisaQty "
                SQL &= " FROM MSOD LEFT JOIN MBarang ON MBarang.NoID=MSOD.IDBarang WHERE MSOD.IDSO IN (SELECT MSOD.IDSO FROM MSPKD INNER JOIN MSOD ON MSOD.NoID=MSPKD.IDSOD) AND (MSOD.Qty*MSOD.Konversi)-IsNull((SELECT SUM(A.Qty*A.Konversi) FROM MSPKD A WHERE A.IDSOD=MSOD.NoID),0)>0 "
                ds = ExecuteDataset("MSO", SQL)
                With ds.Tables("MSO")
                    If .Rows.Count >= 1 Then
                        For i As Integer = 0 To .Rows.Count - 1
                            'txtQty.EditValue / IsiKarton * txtKonversi.EditValue
                            SQL = "INSERT INTO MSPKD ([NoID],[Tgl],[Jam],[IDSPK],[IDBarang],[IDSatuan],[IDGudang],[Ctn],[Qty],[Harga],[DiscPersen1],[DiscPersen2],[DiscPersen3],[Disc1],[Disc2],[Disc3],[Jumlah]," & _
                                 "[IDSOD],[Catatan],[NoUrut],[HargaPcs],[QtyPcs],[Konversi],[IsPacking],[NoPacking],[QtyKarton]) VALUES (" & _
                                  NullToLong(GetNewID("MSPKD", "NoID")) & ", getdate(), getdate(), " & NoID & ", " & NullToLong(.Rows(i).Item("IDBarang")) & ", " & NullToLong(.Rows(i).Item("IDSatuan")) & ", " & _
                                  NullToLong(-1) & ", " & FixKoma(IIf(NullToDbl(.Rows(i).Item("CtnPcs")) = 0, 0, NullToDbl(.Rows(i).Item("SisaQty")) / NullToDbl(.Rows(i).Item("CtnPcs")))) & ", " & FixKoma(NullToDbl(.Rows(i).Item("SisaQty")) / IIf(NullToDbl(.Rows(i).Item("Konversi")) = 0, 1, NullToDbl(.Rows(i).Item("Konversi")))) & ", " & FixKoma(NullToDbl(.Rows(i).Item("Harga"))) & ", " & _
                                  FixKoma(NullToDbl(.Rows(i).Item("DiscPersen1"))) & ", " & FixKoma(NullToDbl(.Rows(i).Item("DiscPersen2"))) & ", " & FixKoma(NullToDbl(.Rows(i).Item("DiscPersen3"))) & ", " & _
                                  FixKoma(NullToDbl(.Rows(i).Item("Disc1"))) & ", " & FixKoma(NullToDbl(.Rows(i).Item("Disc2"))) & ", " & FixKoma(NullToDbl(.Rows(i).Item("Disc3"))) & ", " & _
                                  FixKoma(NullToDbl(.Rows(i).Item("Jumlah")) / NullToDbl(.Rows(i).Item("Qty")) * NullToDbl(.Rows(i).Item("SisaQty")) / NullToDbl(.Rows(i).Item("Konversi"))) & ", " & NullToLong(.Rows(i).Item("NoID")) & ", '" & FixApostropi(NullToStr(.Rows(i).Item("Catatan"))) & "', " & NullToLong(GetNewID("MSPKD", "NoUrut", " WHERE IDSPK=" & NoID)) & ", " & _
                                  FixKoma(NullToDbl(.Rows(i).Item("HargaPcs"))) & ", " & FixKoma(NullToDbl(.Rows(i).Item("QtyPcs"))) & ", " & FixKoma(NullToDbl(.Rows(i).Item("Konversi"))) & ", 0,'',0)"
                            EksekusiSQL(SQL)
                        Next
                    End If
                End With
                'RefreshDetil()
            End If
        Catch ex As Exception
            XtraMessageBox.Show("Kesalahan : " & ex.Message, "Fast Entri Says", MessageBoxButtons.OK)
        Finally
            ds.Dispose()
        End Try
    End Sub
    Private Sub cmdBAru_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdBAru.Click
        Dim frmEntri As New frmEntriSPKD
        Try
            Dim IDDetil As Long = -1
            frmEntri.NoID = IDDetil
            frmEntri.IsNew = True
            frmEntri.IDSPK = NoID
            'If txtGudang.Text <> "" Then
            If txtKodeCustomer.Text <> "" Then
                frmEntri.IDCustomer = NullToLong(txtKodeCustomer.EditValue)
                frmEntri.NoID = IDDetil
                frmEntri.IsNew = True
                frmEntri.IDSPK = NoID
                'frmEntri.IDGudang = NullToLong(txtGudang.EditValue)
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
            'Else
            'XtraMessageBox.Show("Isi dahulu gudang.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information) : txtGudang.Focus()
            'End If
        Catch ex As Exception
            XtraMessageBox.Show("Untuk membuat baru tekan tombol baru", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            'frmEntri.Dispose()
        End Try
    End Sub

    Private Sub cmdEdit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdEdit.Click
        Dim view As ColumnView = GC1.FocusedView
        Dim frmEntri As New frmEntriSPKD
        Try
            Dim row As System.Data.DataRow = view.GetDataRow(GV1.FocusedRowHandle)
            Dim dc As Integer = GV1.FocusedRowHandle
            Dim IDDetil As Long = NullTolong(row("NoID"))
            'If txtGudang.Text <> "" Then
            If txtKodeCustomer.Text <> "" Then
                frmEntri.IDCustomer = NullToLong(txtKodeCustomer.EditValue)
                frmEntri.IsNew = False
                frmEntri.NoID = IDDetil
                frmEntri.IDSPK = NoID
                'frmEntri.IDGudang = NullTolong(txtGudang.EditValue)
                frmEntri.pSPK = pSPK
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
            'Else
            'XtraMessageBox.Show("Isi dahulu gudang.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information) : txtGudang.Focus()
            'End If
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

    Private Sub txtNamaCustomer_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtNamaCustomer.EditValueChanged

    End Sub

    Private Sub Tgl_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Tgl.KeyDown
        If e.KeyCode = Keys.F7 Then
            Dim x As New frmOtorisasiAdmin
            Try
                If Tgl.Properties.ReadOnly Then
                    If x.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
                        IDadmin = x.IDUserAdmin
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

    Private Sub GC1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles GC1.Click

    End Sub

    Private Sub GV1_FocusedRowChanged(ByVal sender As Object, ByVal e As DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs) Handles GV1.FocusedRowChanged
        RefreshStok()
    End Sub
    Private Sub RefreshStok()
        Dim ds As New DataSet
        Dim repcheckedit As New DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit

        Dim dlg As DevExpress.Utils.WaitDialogForm = Nothing
        Dim Cur As Cursor = Windows.Forms.Cursor.Current
        Try
            Dim curentcursor As Cursor = Windows.Forms.Cursor.Current
            Windows.Forms.Cursor.Current = Cursors.WaitCursor
            dlg = New DevExpress.Utils.WaitDialogForm(String.Format("Sedang merefresh data.{0}MOHON TUNGGU ...", vbCrLf), Application.ProductName)
            dlg.TopMost = True
            dlg.Show()
            Windows.Forms.Cursor.Current = Cursors.WaitCursor

            'SQL = "SELECT MWilayah.Nama AS Wilayah, MGudang.Nama AS Gudang, MBarang.Kode AS KodeStock, MBarang.Nama AS NamaStock, SUM((MKartuStok.QtyMasuk*MkartuStok.Konversi)-(MKartuStok.QtyKeluar*MkartuStok.Konversi)) AS Stok, SUM((MKartuStok.QtyMasuk*MkartuStok.Konversi)-(MKartuStok.QtyKeluar*MkartuStok.Konversi))-IsNull(TSPK.DiSPK,0) AS QtySisa" & vbCrLf
            'SQL &= " FROM MKartuStok" & vbCrLf
            'SQL &= " LEFT JOIN (MGudang LEFT JOIN MWilayah ON MWilayah.NoID=MGudang.IDWilayah) ON MGudang.NoID=MKartuStok.IDGudang" & vbCrLf
            'SQL &= " LEFT JOIN MBarang ON MBarang.NoID=MKartuStok.IDBarang" & vbCrLf
            'SQL &= " Left Join" & vbCrLf
            'SQL &= " (SELECT X.IDBarang, X.IDGudang, SUM(X.DiSPK) AS DiSPK FROM" & vbCrLf
            'SQL &= " (SELECT MSPKD.IDBarang, MSPKD.IDGudang, (MSPKD.Qty*MSPKD.Konversi) AS SPK, " & vbCrLf
            'SQL &= " (MSPKD.Qty*MSPKD.Konversi)-IsNull(" & vbCrLf
            'SQL &= " (SELECT SUM(B.Qty*B.Konversi) FROM MJualD B WHERE B.IDSPKD=MSPKD.NoID),0) AS DiSPK " & vbCrLf
            'SQL &= " FROM MSPKD ) X" & vbCrLf
            'SQL &= " GROUP BY X.IDBarang, X.IDGudang) TSPK ON MKartuStok.IDBarang=TSPK.IDBarang AND MKartuStok.IDGudang=TSPK.IDGudang" & vbCrLf
            'SQL &= " WHERE MWilayah.NoID=" & DefIDWilayah & " AND MKartuStok.IDBarang=" & NullTolong(GV1.GetRowCellValue(GV1.FocusedRowHandle, "IDBarang")) & vbCrLf
            'SQL &= " GROUP BY TSPK.DiSPK, MWilayah.Nama, MGudang.Nama, MBarang.Nama, MBarang.Kode" & vbCrLf

            SQL = "SELECT MBarang.Nama AS NamaStock, MBarang.Kode AS KodeStock, MGudang.Nama AS Gudang, MWilayah.Nama AS Wilayah, X.IDGudang, X.IDBarang, SUM(X.Masuk) AS QtyStock, SUM(X.Masuk-X.Keluar) AS SisaQty FROM " & vbCrLf & _
                  " (SELECT MKartuStok.IDGudang, MKartuStok.IDBarang, (MKartuStok.QtyMasuk-MKartuStok.QtyKeluar)*MKartuStok.Konversi AS Masuk , 0 AS Keluar" & vbCrLf & _
                  " FROM MKartuStok LEFT JOIN MGudang ON MGudang.NoID=MKartuStok.IDGudang" & vbCrLf & _
                  " WHERE MGudang.IDWilayah = " & DefIDWilayah & " And MKartuStok.IDBarang = " & NullToLong(GV1.GetRowCellValue(GV1.FocusedRowHandle, "IDBarang")) & " " & vbCrLf & _
                  " UNION ALL  " & vbCrLf & _
                  " SELECT MSPKD.IDGudang, MSPKD.IDBarang, 0, ((MSPKD.Qty*MSPKD.Konversi)-IsNull((SELECT SUM(MPackingD.Qty*MPackingD.Konversi) FROM MPackingD INNER JOIN MPacking ON MPacking.NoID=MPackingD.IDPacking WHERE MPackingD.IDSPKD=MSPKD.NoID),0)) AS DiSPK " & vbCrLf & _
                  " FROM MSPKD INNER JOIN MSPK ON MSPK.NoID=MSPKD.IDSPK  " & vbCrLf & _
                  " WHERE MSPK.IDWilayah=" & DefIDWilayah & " AND MSPKD.IDBarang=" & NullToLong(GV1.GetRowCellValue(GV1.FocusedRowHandle, "IDBarang")) & " AND ((MSPKD.Qty*MSPKD.Konversi)-IsNull((SELECT SUM(MPackingD.Qty*MPackingD.Konversi) FROM MPackingD INNER JOIN MPacking ON MPacking.NoID=MPackingD.IDPacking WHERE MPackingD.IDSPKD=MSPKD.NoID),0))>0) X " & vbCrLf & _
                  " LEFT JOIN (MGudang LEFT JOIN MWilayah ON MWilayah.NoID=MGudang.IDWilayah) ON MGudang.NoID=X.IDGudang " & vbCrLf & _
                  " LEFT JOIN MBarang ON MBarang.NoID=X.IDBarang " & vbCrLf & _
                  " GROUP BY X.IDGUdang, X.IDBarang, MBarang.Nama, MBarang.Kode, MGudang.Nama, MWilayah.Nama"

            ds = ExecuteDataset("MStok", SQL)
            GridControl1.DataSource = ds.Tables(0)
            With GridView1
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
                        Case "boolean"
                            .Columns(x).ColumnEdit = repcheckedit
                    End Select
                Next
            End With
        Catch ex As Exception
            XtraMessageBox.Show("Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            ds.Dispose()
            Windows.Forms.Cursor.Current = Cur
            Windows.Forms.Cursor.Current = Cursors.Default
            dlg.Close()
            dlg.Dispose()
        End Try
    End Sub

    Private Sub txtSO_ButtonClick(ByVal sender As Object, ByVal e As DevExpress.XtraEditors.Controls.ButtonPressedEventArgs) Handles txtSO.ButtonClick
        If pTipe = pStatus.Baru Then Exit Sub
        Select Case e.Button.Index
            Case 1
                InsertIntoDetilSO(NullTolong(txtSO.EditValue))
                RefreshSO()
                txtSO.EditValue = -1
                txtSO.Focus()
        End Select
    End Sub

    Private Sub txtSO_EditValueChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtSO.EditValueChanged

    End Sub

    Private Sub gvSO_DataSourceChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvSO.DataSourceChanged
        If System.IO.File.Exists(folderLayouts & Me.Name & gvSO.Name & ".xml") Then
            gvSO.RestoreLayoutFromXml(folderLayouts & Me.Name & gvSO.Name & ".xml")
        End If
        With gvSO
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

    Private Sub txtTglKirim_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtTglKirim.EditValueChanged

    End Sub

    Private Sub txtKode_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtKode.EditValueChanged

    End Sub

    Private Sub txtKode_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtKode.KeyDown
        If e.KeyCode = Keys.F7 Then
            Dim x As New frmOtorisasiAdmin
            Try
                If txtKode.Properties.ReadOnly Then
                    If x.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
                        IDadmin = x.IDUserAdmin
                        txtKode.Properties.ReadOnly = False
                    End If
                End If
            Catch ex As Exception
                XtraMessageBox.Show("Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End If
    End Sub

    Private Sub txtSO_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtSO.KeyDown
        If pTipe = pStatus.Baru Then Exit Sub
        Select Case e.KeyCode
            Case Keys.Enter
                InsertIntoDetilSO(NullToLong(txtSO.EditValue))
                RefreshSO()
                txtSO.EditValue = -1
                txtSO.Focus()
            Case Keys.Escape
                txtSO.EditValue = -1
        End Select
    End Sub

    'Private Sub RubahGudang()
    '    Try
    '        txtWilayah.Text = NullToStr(EksekusiSQlSkalarNew("SELECT MWilayah.Nama FROM MGudang LEFT JOIN MWilayah ON MWilayah.NOID=MGudang.IDWilayah WHERE MGudang.NoID=" & NullToLong(txtGudang.EditValue)))
    '        If Not IsLoad AndAlso Not IsPosted Then
    '            EksekusiSQL("UPDATE MSPKD SET IDGudang=" & NullToLong(txtGudang.EditValue) & " WHERE IDSPK=" & NoID)
    '            RefreshDetil()
    '        End If
    '    Catch ex As Exception
    '        XtraMessageBox.Show("Info kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
    '    End Try
    'End Sub
End Class