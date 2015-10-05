Imports System.Data
Imports System.Data.SqlClient
Imports DevExpress.XtraEditors
Imports DevExpress.Utils
Imports DevExpress.XtraGrid.Views.Base
Imports VPoint.SGI.Distributor.Program

Public Class frmEntriPO
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
    Public IsAlias As Boolean = False

    Public IsShow As Boolean = False
    Dim frmEntri As frmEntriPOD = VPOINT.Forms.EntriPOD.Instance
    Dim IDTypePajakLama As Long

    Private Sub RefreshDataKontak()
        Dim ds As New DataSet
        Try
            SQL = "SELECT NoID, Kode, Nama FROM MAlamat WHERE IsActive=1 AND IsSupplier=1"
            ds = ExecuteDataset("masterSupplier", SQL)
            txtKodeSupplier.Properties.DataSource = ds.Tables("masterSupplier")
            If System.IO.File.Exists(folderLayouts & Me.Name & SearchLookUpEdit1View.Name & ".xml") Then
                SearchLookUpEdit1View.RestoreLayoutFromXml(folderLayouts & Me.Name & SearchLookUpEdit1View.Name & ".xml")
            End If
            SQL = "SELECT NoID, Kode, Nama FROM MWilayah WHERE IsActive=1 "
            ds = ExecuteDataset("masterWilayah", SQL)
            txtWilayah.Properties.DataSource = ds.Tables("masterWilayah")
            txtWilayah.Properties.ValueMember = "NoID"
            txtWilayah.Properties.DisplayMember = "Nama"
            If System.IO.File.Exists(folderLayouts & Me.Name & GV1.Name & ".xml") Then
                GV1.RestoreLayoutFromXml(folderLayouts & Me.Name & GV1.Name & ".xml")
            End If
            SQL = "SELECT NoID, Kode, Nama,TarifProsen FROM MTypePajak WHERE IsActive=1 "
            ds = ExecuteDataset("masterPajak", SQL)
            TypePajak.Properties.DataSource = ds.Tables("masterPajak")
            TypePajak.Properties.ValueMember = "NoID"
            TypePajak.Properties.DisplayMember = "Nama"
            If System.IO.File.Exists(FolderLayouts & Me.Name & GVPajak.Name & ".xml") Then
                GVPajak.RestoreLayoutFromXml(FolderLayouts & Me.Name & GVPajak.Name & ".xml")
            End If
        Catch ex As Exception
            XtraMessageBox.Show("Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            ds.Dispose()
        End Try
    End Sub
    Dim DefImageList As New ImageList

    Private Sub frmEntriPO_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Activated
        If IsShow Then
            RefreshDetil()
            IsShow = False
        End If
    End Sub

    Private Sub frmEntriPO_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        Try
            If IsTempInsertBaru Then
                If XtraMessageBox.Show("Yakin ingin keluar?" & vbCrLf & "Ya, akan menghapus transaksi dan detil item.", NamaAplikasi, MessageBoxButtons.YesNo, MessageBoxIcon.Information, MessageBoxDefaultButton.Button2) = Windows.Forms.DialogResult.Yes Then
                    If Not NullToBool(EksekusiSQlSkalarNew("SELECT isnull(IsPosted,0) FROM  MPO WHERE NoID=" & NoID)) Then
                        EksekusiSQL("DELETE FROM MPO WHERE NoID=" & NoID)
                        EksekusiSQL("DELETE FROM MPOD WHERE IDPO=" & NoID)
                    Else
                        MsgBox("Data ini telah terposting, anda harus menyimpannya!", MsgBoxStyle.Information)
                    End If
                Else
                    e.Cancel = True
                End If
            End If
        Catch ex As Exception
            XtraMessageBox.Show("Info Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK)
        End Try
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
            If System.IO.File.Exists(folderLayouts & Me.Name & GV1.Name & IsAlias & ".xml") Then
                GV1.RestoreLayoutFromXml(folderLayouts & Me.Name & GV1.Name & IsAlias & ".xml")
            End If
            If System.IO.File.Exists(folderLayouts & Me.Name & SearchLookUpEdit1View.Name & ".xml") Then
                SearchLookUpEdit1View.RestoreLayoutFromXml(folderLayouts & Me.Name & SearchLookUpEdit1View.Name & ".xml")
            End If
            If IsAlias Then
                Me.Text = "Entri Purchase Order (Kode Alias)"
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
                SQL = "SELECT MPO.*, MUserEntri.Nama AS UserEntri, MUserEdit.Nama AS UserEdit, MUserPosting.Nama AS UserPosting, MAlamat.Alamat AS Alamat, MAlamat.Nama AS NamaAlamat, MAlamat.Kode as KodeAlamat " & _
                      " FROM MPO LEFT JOIN MAlamat ON MAlamat.NoID=MPO.IDSupplier " & _
                      " LEFT JOIN MUser MUserEntri ON MUserEntri.NoID=MPO.IDUserEntry " & _
                      " LEFT JOIN MUser MUserEdit ON MUserEdit.NoID=MPO.IDUserEdit " & _
                      " LEFT JOIN MUser MUserPosting ON MUserPosting.NoID=MPO.IDUserPosting " & _
                      " WHERE MPO.NoID = " & NoID
                DS = ExecuteDataset("Tabel", SQL)
                If DS.Tables("Tabel").Rows.Count >= 1 Then
                    txtKodeSupplier.EditValue = NullTolong(DS.Tables(0).Rows(0).Item("IDSupplier"))
                    txtWilayah.EditValue = NullTolong(DS.Tables(0).Rows(0).Item("IDWilayah"))
                    txtNamaSupplier.Text = NullTostr(DS.Tables(0).Rows(0).Item("NamaAlamat"))
                    txtAlamatSupplier.Text = NullTostr(DS.Tables(0).Rows(0).Item("Alamat"))
                    txtKode.Text = NullTostr(DS.Tables(0).Rows(0).Item("Kode"))
                    KodeLama = txtKode.Text
                    txtKodeReff.Text = NullTostr(DS.Tables(0).Rows(0).Item("KodeReff"))
                    Tgl.DateTime = NullToDate(DS.Tables(0).Rows(0).Item("Tanggal"))
                    tglStok.DateTime = NullToDate(DS.Tables(0).Rows(0).Item("TanggalStock"))
                    tglJatuhTempo.DateTime = NullToDate(DS.Tables(0).Rows(0).Item("JatuhTempo"))
                    tglSJ.DateTime = NullToDate(DS.Tables(0).Rows(0).Item("TanggalSJ"))
                    txtNoSJ.Text = NullToStr(DS.Tables(0).Rows(0).Item("NoSJ"))
                    txtSubtotal.EditValue = NullToDbl(DS.Tables(0).Rows(0).Item("SubTotal"))
                    txtDiscPersen.EditValue = NullToDbl(DS.Tables(0).Rows(0).Item("DiskonNotaProsen"))
                    txtDiscRp.EditValue = NullToDbl(DS.Tables(0).Rows(0).Item("DiskonNotaRp"))
                    txtDiscTotal.EditValue = NullToDbl(DS.Tables(0).Rows(0).Item("DiskonNotaTotal"))
                    txtBiaya.EditValue = NullToDbl(DS.Tables(0).Rows(0).Item("Biaya"))
                    txtTotal.EditValue = NullToDbl(DS.Tables(0).Rows(0).Item("Total"))
                    txtBayar.EditValue = NullToDbl(DS.Tables(0).Rows(0).Item("Bayar"))
                    txtSisa.EditValue = NullToDbl(DS.Tables(0).Rows(0).Item("Sisa"))
                    IsPosted = NullToBool(DS.Tables(0).Rows(0).Item("IsPosted"))
                    txtPKP.Text = IIf(NullToBool(DS.Tables(0).Rows(0).Item("IsPKP")), "PKP", "Non PKP")
                    txtDientriOleh.Text = NullToStr(DS.Tables(0).Rows(0).Item("UserEntri"))
                    txtDieditOleh.Text = NullToStr(DS.Tables(0).Rows(0).Item("UserEdit"))
                    txtDipostingOleh.Text = NullToStr(DS.Tables(0).Rows(0).Item("UserPosting"))
                    tglEntri.DateTime = NullToDate(DS.Tables(0).Rows(0).Item("TglEntri"))
                    tglEdit.DateTime = NullToDate(DS.Tables(0).Rows(0).Item("TglEdit"))
                    tglPosting.DateTime = NullToDate(DS.Tables(0).Rows(0).Item("TglPosting"))
                    txtTempo.EditValue = NullToLong(DS.Tables(0).Rows(0).Item("Tempo"))
                    TypePajak.EditValue = NullToLong(DS.Tables(0).Rows(0).Item("IDTypePajak"))
                    IDTypePajakLama = NullToLong(TypePajak.EditValue)
                    'TypePajak.Properties.ReadOnly = True
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
            strsql = "SELECT MPOD.*, MBarang.Catatan AS Keterangan, MBarang.CtnPcs AS IsiCtn, MBarangD.Barcode, MPOD.Disc1 AS DiscRp, MWilayah.Nama AS Wilayah, MBarang.KodeAlias AS Alias, MBarang.Kode as KodeBarang,MBarang.Nama as NamaBarang,MSatuan.Kode as Satuan,Mgudang.Kode as KodeGudang, " & vbCrLf & _
                     " MPOD.Jumlah AS TotalHargaBeli, MPOD.HargaPcs AS HargaBeliSatuan, " & _
                     " MPOD.HargaJualPcs*MPOD.Qty*MPOD.Konversi AS TotalHargaJual, MPOD.HargaJualPcs AS HargaJualSatuan, MPOD.Konversi AS Isi " & _
                     " FROM (MPOD Inner Join MPO On MPOD.IDPO=MPO.NoID) " & vbCrLf & _
                     " LEFT JOIN MBarangD ON MPOD.IDBarang=MBarangD.NoID " & vbCrLf & _
                     " LEFT JOIN MBarang ON MPOD.IDBarang=MBarang.NoID " & vbCrLf & _
                     " LEFT JOIN MSatuan ON MPOD.IDSatuan=MSatuan.NoID " & vbCrLf & _
                     " LEFT JOIN MGudang ON MPOD.IDGudang=MGudang.NoID " & vbCrLf & _
                     " LEFT JOIN MWilayah ON MPOD.IDWilayah=MWilayah.NoID " & vbCrLf & _
                     " WHERE MPOD.IDPO = " & NoID
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
        Dim ds As New DataSet
        Try
            For i = 0 To GV1.RowCount
                temp = temp + NullToDbl(GV1.GetRowCellValue(i, "Jumlah"))
            Next
            txtSubtotal.EditValue = temp
            txtDiscTotal.EditValue = (NullToDbl(txtDiscPersen.EditValue) * NullToDbl(txtSubtotal.EditValue) / 100) '+ txtDiscRp.EditValue
            txtSubtotal2.EditValue = NullToDbl(txtSubtotal.EditValue) - NullToDbl(txtDiscTotal.EditValue)
            txtTotal.EditValue = txtSubtotal.EditValue - txtDiscTotal.EditValue + txtBiaya.EditValue
            txtSisa.EditValue = txtTotal.EditValue - txtBayar.EditValue
            txtTotalCTN.EditValue = NullToDbl(EksekusiSQlSkalarNew("SELECT SUM(Ctn) FROM MPOD WHERE IDPO=" & NoID))

            'Hitung Target
            SQL = "SELECT MTargetSupplier.NoID, Malamat.Kode AS Supplier, REPLACE(CONVERT(Varchar, MTargetSupplier.TglDari, 103),'/','-') + ' s/d ' + REPLACE(CONVERT(Varchar, MTargetSupplier.TglSampai, 103),'/','-') AS Peride, MTargetSupplier.TglDari AS DariTanggal, MTargetSupplier.TglSampai AS SampaiTanggal, MTargetSupplier.[Target], " & vbCrLf & _
                  " (SELECT SUM(MPOD.Jumlah) FROM MPOD INNER JOIN MPO ON MPOD.IDPO=MPO.NoID WHERE MPO.NoID<>" & NoID & " AND MPO.IDSupplier=MTargetSupplier.IDSupplier AND MPO.Tanggal>=MTargetSupplier.TglDari AND MPO.Tanggal<=MTargetSupplier.TglSampai) AS TargetTerpenuhi, " & vbCrLf & _
                  " IsNull(MTargetSupplier.[Target],0)-IsNull((SELECT SUM(MPOD.Jumlah) FROM MPOD INNER JOIN MPO ON MPOD.IDPO=MPO.NoID WHERE MPO.NoID<>" & NoID & " AND MPO.IDSupplier=MTargetSupplier.IDSupplier AND MPO.Tanggal>=MTargetSupplier.TglDari AND MPO.Tanggal<=MTargetSupplier.TglSampai),0) AS SisaTarget " & vbCrLf & _
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
            XtraMessageBox.Show("Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            If Not ds Is Nothing Then
                ds.Dispose()
            End If
        End Try
    End Sub
    Private Sub IsiDefault()
        SetTombol()
        txtKodeSupplier.EditValue = DefIDSupplier
        RubahSupplier()
        txtWilayah.EditValue = DefIDWilayah
        Tgl.DateTime = TanggalSystem
        Tgl.Properties.ReadOnly = False
        tglJatuhTempo.DateTime = DateAdd(DateInterval.Month, 1, Tgl.DateTime)
        tglSJ.DateTime = TanggalSystem
        tglStok.DateTime = TanggalSystem
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

        'SimpleButton1.ImageList = DefImageList

        If pTipe = pStatus.Baru Or IsPosted Then
            cmdBAru.Enabled = False
            cmdEdit.Enabled = False
            cmdDelete.Enabled = False
            SimpleButton1.Enabled = False
        Else
            cmdBAru.Enabled = True
            cmdEdit.Enabled = True
            cmdDelete.Enabled = True
            SimpleButton1.Enabled = True
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
                        IsTempInsertBaru = True
                        'TypePajak.Properties.ReadOnly = True
                        'txtKodeSupplier.Properties.ReadOnly = True
                    Else
                        clsPostingPembelian.PostingPO(NoID)
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
                            frmEntri.MdiParent = Me.MdiParent
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
        If TypePajak.Text = "" Then
            XtraMessageBox.Show("Type Pajak masih kosong?", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Warning)
            TypePajak.Focus()
            Return False
            Exit Function
        End If
        'If pTipe = pStatus.Edit AndAlso Not IsTempInsertBaru AndAlso IDTypePajakLama = NullToLong(TypePajak.EditValue) Then
        '    XtraMessageBox.Show("Kembalikan Tipe Pajak ke ")
        'End If
        If NullToBool(EksekusiSQlSkalarNew("SELECT MPO.IsPosted FROM MPO WHERE MPO.NoID=" & NoID)) Then 'Nota DiPosting
            XtraMessageBox.Show("Nota Sudah Di Posting/Lock.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
            cmdTutup.Focus()
            Return False
            Exit Function
        End If
        If txtKodeSupplier.Text = "" Then
            XtraMessageBox.Show("Supplier masih kosong.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
            txtKodeSupplier.Focus()
            Return False
            Exit Function
        End If
        If txtWilayah.Text = "" Then
            XtraMessageBox.Show("Wilayah masih kosong.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
            txtWilayah.Focus()
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
        If txtDiscRp.EditValue < 0 Then
            XtraMessageBox.Show("Diskon rupiah masih dibawah 0.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
            txtDiscRp.Focus()
            Return False
            Exit Function
        ElseIf txtDiscRp.EditValue > txtSubtotal.EditValue Then
            XtraMessageBox.Show("Diskon rupiah melebihi subtotal.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
            txtDiscRp.Focus()
            Return False
            Exit Function
        End If
        If txtDiscPersen.EditValue > 100 Then
            XtraMessageBox.Show("Diskon tidak boleh diatas 100%.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
            txtDiscPersen.Focus()
            Return False
            Exit Function
        ElseIf txtDiscPersen.EditValue < 0 Then
            XtraMessageBox.Show("Diskon persen masih dibawah 0.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
            txtDiscPersen.Focus()
            Return False
            Exit Function
        End If
        If txtBayar.EditValue < 0 Then
            XtraMessageBox.Show("Bayar masih dibawah 0.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
            txtBayar.Focus()
            Return False
            Exit Function
        ElseIf txtBayar.EditValue > txtSubtotal.EditValue Then
            XtraMessageBox.Show("Pembayaran melebihi subtotal.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
            txtBayar.Focus()
            Return False
            Exit Function
        End If
        If txtBiaya.EditValue < 0 Then
            XtraMessageBox.Show("Biaya masih dibawah 0.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
            txtBiaya.Focus()
            Return False
            Exit Function
        ElseIf txtBiaya.EditValue > txtSubtotal.EditValue Then
            XtraMessageBox.Show("Biaya melebihi subtotal.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
            txtBiaya.Focus()
            Return False
            Exit Function
        End If
        If txtSisa.EditValue < 0 Then
            XtraMessageBox.Show("Total masih kurang dari 0.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
            txtSisa.Focus()
            Return False
            Exit Function
        End If
        If CekKodeValid(txtKode.Text, KodeLama, "MPO", "Kode", IIf(pTipe = pStatus.Edit, True, False)) Then
            XtraMessageBox.Show("Kode sudah dipakai.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
            If pTipe = pStatus.Baru Then
                If XtraMessageBox.Show("Lakukan pemberian kode baru?", NamaAplikasi, MessageBoxButtons.YesNo, MessageBoxIcon.Information) = Windows.Forms.DialogResult.Yes Then
                    txtKode.Text = clsKode.MintaKodeSPPBaru("SP", "MPO", Tgl.DateTime, NullToLong(txtWilayah.EditValue), 7)
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
    Public Sub SimpanTambahan()
        Try
            HitungTotal()
            SQL = "UPDATE MPO SET "
            SQL &= " SubTotal=" & FixKoma(txtSubtotal.EditValue) & ","
            SQL &= " DiskonNotaProsen=" & FixKoma(txtDiscPersen.EditValue) & ","
            SQL &= " DiskonNotaRp=" & FixKoma(txtDiscRp.EditValue) & ","
            SQL &= " DiskonNotaTotal=" & FixKoma(txtDiscTotal.EditValue) & ","
            SQL &= " Biaya=" & FixKoma(txtBiaya.EditValue) & ","
            SQL &= " Total=" & FixKoma(txtTotal.EditValue) & ","
            SQL &= " Bayar=" & FixKoma(txtBayar.EditValue) & ","
            SQL &= " Sisa=" & FixKoma(txtSisa.EditValue) & " "
            SQL &= " WHERE NoID=" & NoID
            EksekusiSQL(SQL)
        Catch ex As Exception

        End Try
    End Sub
    Public Function Simpan(ByRef PesanSalah As String) As Boolean
        Dim Sukses As Boolean = False
        HitungTotal()
        Try
            If pTipe = pStatus.Baru Then
                NoID = GetNewID("MPO")
                SQL = "INSERT INTO MPO (NoID,IDWilayah,Kode,KodeReff,Tanggal,TanggalStock,JatuhTempo,"
                SQL &= " IDSupplier,TanggalSJ,NoSJ,SubTotal,DiskonNotaProsen,DiskonNotaRp,DiskonNotaTotal,"
                SQL &= "  Biaya, Total, Bayar, Sisa,Tempo,IsPKP,IDTypePajak,IDUserEntry,IDUserEdit,IDUserPosting,TglEntri,IDAdmin) VALUES (" & vbCrLf
                SQL &= NoID & ","
                SQL &= NullToLong(txtWilayah.EditValue) & ","
                SQL &= "'" & FixApostropi(txtKode.Text) & "',"
                SQL &= "'" & FixApostropi(txtKodeReff.Text) & "',"
                SQL &= "'" & Tgl.DateTime.ToString("yyyy-MM-dd HH:mm") & "',"
                SQL &= "'" & tglStok.DateTime.ToString("yyyy-MM-dd HH:mm") & "',"
                SQL &= "'" & tglJatuhTempo.DateTime.ToString("yyyy-MM-dd HH:mm") & "',"
                SQL &= txtKodeSupplier.EditValue & ","
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
                SQL &= FixKoma(txtTempo.EditValue) & ","
                SQL &= IIf(txtPKP.Text = "PKP", "1", "0") & ","
                SQL &= NullToLong(TypePajak.EditValue) & ","
                SQL &= IDUserAktif & ","
                SQL &= "-1,"
                SQL &= "-1,"
                SQL &= "'" & TanggalSystem.ToString("yyyy-MM-dd HH:mm") & "'," & IDAdmin & ")"
            Else
                SQL = "UPDATE MPO SET "
                If Not Tgl.Properties.ReadOnly Or Not txtKode.Properties.ReadOnly Or Not txtWilayah.Properties.ReadOnly Then
                    SQL &= "IDAdmin=" & IDAdmin & ","
                End If
                SQL &= "IDWilayah=" & NullToLong(txtWilayah.EditValue) & ","
                SQL &= "Kode='" & FixApostropi(txtKode.Text) & "',"
                SQL &= "KodeReff='" & FixApostropi(txtKodeReff.Text) & "',"
                SQL &= "Tanggal='" & Tgl.DateTime.ToString("yyyy-MM-dd HH:mm") & "',"
                SQL &= "TanggalStock='" & tglStok.DateTime.ToString("yyyy-MM-dd HH:mm") & "',"
                SQL &= "JatuhTempo='" & tglJatuhTempo.DateTime.ToString("yyyy-MM-dd HH:mm") & "',"
                SQL &= "IDSupplier=" & txtKodeSupplier.EditValue & ","
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
                SQL &= "Tempo=" & FixKoma(txtTempo.EditValue) & ","
                SQL &= "IsPKP=" & IIf(txtPKP.Text = "PKP", "1", "0") & ","
                SQL &= "IDTypePajak=" & NullToLong(TypePajak.EditValue) & ","
                SQL &= "TglEdit='" & TanggalSystem.ToString("yyyy-MM-dd HH:mm") & "',"
                SQL &= "IDUserEdit=" & IDUserAktif
                SQL &= " WHERE NoID=" & NoID
            End If
            If EksekusiSQL(SQL) >= 1 Then
                Sukses = True
            Else
                Sukses = False
            End If
            If Sukses Then
                'Dim HargaBeli As Double = 0.0
                'Dim TotalHargaBeli As Double = 0.0
                ''RefreshDetil() Bikin Kisruh
                'For i As Integer = 0 To GV1.RowCount
                '    HargaBeli = IIf(NullToLong(TypePajak.EditValue) = 2, 1.1, 1.0) * (Bulatkan(NullToDbl(GV1.GetRowCellValue(i, "Harga")) * (1 - (NullToDbl(GV1.GetRowCellValue(i, "DiscPersen1")) / 100)) * (1 - (NullToDbl(GV1.GetRowCellValue(i, "DiscPersen2")) / 100)) * (1 - (NullToDbl(GV1.GetRowCellValue(i, "DiscPersen3")) / 100)), 2) - NullToDbl(GV1.GetRowCellValue(i, "Disc1"))) / IIf(NullToDbl(GV1.GetRowCellValue(i, "Konversi")) = 0, 1, NullToDbl(GV1.GetRowCellValue(i, "Konversi")))
                '    TotalHargaBeli = HargaBeli * NullToDbl(GV1.GetRowCellValue(i, "Qty"))
                '    SQL = "UPDATE MPOD SET HargaPcs=" & FixKoma(HargaBeli) & ", Jumlah=" & FixKoma(TotalHargaBeli) & " WHERE NoID=" & NullToLong(GV1.GetRowCellValue(i, "NoID"))
                '    EksekusiSQL(SQL)
                'Next
                'RefreshDetil()
                'HitungTotal()
            End If
            'If Sukses Then
            '    SQL = "UPDATE MPOD SET HargaNetto=HargaNetto-(" & FixKoma(txtDiscPersen.EditValue) & "/100*HargaNetto) WHERE IDPO=" & NoID
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
                Ini.TulisIniPath(FolderLayouts & Me.Name & ".ini", "Form", "Width", Me.Width)
                Ini.TulisIniPath(FolderLayouts & Me.Name & ".ini", "Form", "Height", Me.Height)
                Me.Left = (Screen.PrimaryScreen.WorkingArea.Width - Me.Width) \ 2
                Me.Top = (Screen.PrimaryScreen.WorkingArea.Height - Me.Height) \ 2

                LayoutControl1.SaveLayoutToXml(FolderLayouts & Me.Name & LayoutControl1.Name & ".xml")
                GV1.SaveLayoutToXml(FolderLayouts & Me.Name & GV1.Name & IsAlias & ".xml")
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
            If XtraMessageBox.Show("Item " & GV1.GetRowCellValue(GV1.FocusedRowHandle, "NamaBarang") & " ini mau dihapus?", NamaAplikasi, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) = Windows.Forms.DialogResult.Yes Then
                If NullToBool(EksekusiSQlSkalarNew("SELECT MPO.IsPosted FROM MPO WHERE MPO.NoID=" & NoID)) Then 'Nota DiPosting
                    XtraMessageBox.Show("Nota Sudah Di Posting/Lock.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
                    Exit Sub
                End If
                EksekusiSQL("Delete From MPOD where NoID=" & IDDetil.ToString)
                RefreshDetil()
                SimpanTambahan()
            End If
            txtBarang.Focus()
        Catch ex As Exception
            XtraMessageBox.Show("Untuk mengedit pilih item satuan yang akan diedit terlebih dahulu lalu tekan tombol edit", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub txtKodeSupplier_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtKodeSupplier.EditValueChanged
        RubahSupplier()
    End Sub
    Private Sub RubahSupplier()
        Dim TglAdd As Long = 0
        Dim ds As New DataSet
        Try
            txtNamaSupplier.Text = NullToStr(EksekusiSQlSkalarNew("SELECT Nama FROM MAlamat WHERE NoID=" & NullToLong(txtKodeSupplier.EditValue)))
            txtAlamatSupplier.Text = NullToStr(EksekusiSQlSkalarNew("SELECT Alamat FROM MAlamat WHERE NoID=" & NullToLong(txtKodeSupplier.EditValue)))
            TglAdd = NullToLong(EksekusiSQlSkalarNew("SELECT JatuhTempoSupplier FROM MAlamat WHERE NoID=" & NullToLong(txtKodeSupplier.EditValue)))
            If TglAdd = 0 Then
                tglJatuhTempo.DateTime = DateAdd(DateInterval.Month, 2, Tgl.DateTime)
            Else
                tglJatuhTempo.DateTime = DateAdd(DateInterval.Day, TglAdd, Tgl.DateTime)
            End If
            txtTempo.EditValue = TglAdd
            txtPKP.Text = IIf(NullToStr(EksekusiSQlSkalarNew("SELECT IsPKP FROM MAlamat WHERE NoID=" & NullToLong(txtKodeSupplier.EditValue))).ToUpper = "TRUE", "PKP", "NonPKP")

            SQL = "SELECT MTargetSupplier.NoID, Malamat.Kode AS Supplier, REPLACE(CONVERT(Varchar, MTargetSupplier.TglDari, 103),'/','-') + ' s/d ' + REPLACE(CONVERT(Varchar, MTargetSupplier.TglSampai, 103),'/','-') AS Peride, MTargetSupplier.TglDari AS DariTanggal, MTargetSupplier.TglSampai AS SampaiTanggal, MTargetSupplier.[Target]" & vbCrLf & _
                  " FROM MTargetSupplier " & _
                  " INNER JOIN MAlamat ON MAlamat.NoID=MTargetSupplier.IDSupplier" & vbCrLf & _
                  " WHERE MTargetSupplier.IDSupplier=" & NullToLong(txtKodeSupplier.EditValue)
            ds = ExecuteDataset("MBarang", SQL)
            txtTargetSupplier.Properties.DataSource = ds.Tables("MBarang")
            txtTargetSupplier.Properties.ValueMember = "NoID"
            txtTargetSupplier.Properties.DisplayMember = "Peride"

            'Set Target
            SQL &= vbCrLf & _
                   " AND '" & Tgl.DateTime.ToString("yyyy/MM/dd") & "'>=MTargetSupplier.TglDari AND '" & Tgl.DateTime.AddDays(1).ToString("yyyy/MM/dd") & "'<MTargetSupplier.TGlSampai "
            txtTargetSupplier.EditValue = NullToLong(EksekusiSQlSkalarNew(SQL))

        Catch ex As Exception

        Finally
            If Not ds Is Nothing Then
                ds.Dispose()
            End If
        End Try
    End Sub
    Private Sub Tgl_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Tgl.EditValueChanged
        Dim TglAdd As Long = 0
        Try
            'txtNamaSupplier.Text = NullTostr(SearchLookUpEdit1View.GetRowCellValue(SearchLookUpEdit1View.FocusedRowHandle, "Nama"))
            'txtAlamatSupplier.Text = NullTostr(EksekusiSQlSkalarNew("SELECT Alamat FROM MAlamat WHERE NoID=" & NullTolong(txtKodeSupplier.EditValue)))
            TglAdd = NullToLong(EksekusiSQlSkalarNew("SELECT JatuhTempoSupplier FROM MAlamat WHERE NoID=" & NullToLong(txtKodeSupplier.EditValue)))
            If TglAdd = 0 Then
                tglJatuhTempo.DateTime = DateAdd(DateInterval.Month, 2, Tgl.DateTime)
            Else
                tglJatuhTempo.DateTime = DateAdd(DateInterval.Day, TglAdd, Tgl.DateTime)
            End If
            If pTipe = pStatus.Baru Then
                txtKode.Text = clsKode.MintaKodeSPPBaru("SP", "MPO", Tgl.DateTime, NullToLong(txtWilayah.EditValue), 7)
            End If
        Catch ex As Exception

        End Try
    End Sub

    Private Sub txtSubtotal_EditValueChanging(ByVal sender As Object, ByVal e As DevExpress.XtraEditors.Controls.ChangingEventArgs) Handles txtSubtotal.EditValueChanging

    End Sub

    Private Sub txtSubtotal_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtSubtotal.LostFocus
        HitungTotal()
    End Sub

    Private Sub txtDiscPersen_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtDiscPersen.LostFocus
        txtDiscRp.EditValue = txtSubtotal.EditValue * txtDiscPersen.EditValue / 100
        HitungTotal()
    End Sub

    Private Sub txtDiscRp_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtDiscRp.KeyDown
        If e.KeyCode = Keys.F7 Then
            Dim x As New frmOtorisasiAdmin
            Try
                If txtDiscRp.Properties.ReadOnly Then
                    If x.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
                        IDAdmin = x.IDUserAdmin
                        txtDiscRp.Properties.ReadOnly = False
                    End If
                End If
            Catch ex As Exception
                XtraMessageBox.Show("Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End If
    End Sub

    Private Sub txtDiscRp_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtDiscRp.LostFocus
        txtDiscPersen.EditValue = txtDiscRp.EditValue / IIf(txtSubtotal.EditValue = 0, 1, txtSubtotal.EditValue) * 100
        HitungTotal()
    End Sub

    Private Sub txtDiscTotal_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtDiscTotal.LostFocus
        HitungTotal()
    End Sub

    Private Sub txtBiaya_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtBiaya.LostFocus
        HitungTotal()
    End Sub

    Private Sub txtTotal_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtTotal.KeyDown
        If e.KeyCode = Keys.Enter Then
            cmdSave.Focus()
        End If
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
    Private Function CariBarang(ByRef IDBarang As Long, ByRef NamaBarang As String, ByRef KodeBarang As String, ByRef IsPPn As Boolean, ByRef HargaJual As Double, ByRef HargaBeliPcs As Double, ByRef Disc1 As Double, ByRef Disc2 As Double, ByRef Disc3 As Double, ByRef HargaBeli As Double, ByRef CtnPcs As Double) As Boolean
        Dim x As Boolean = False
        Dim SQL As String = ""
        Dim oDS As New DataSet
        Try
            If NamaBarang <> "" Then
                SQL = "SELECT NoID,Kode,(CASE WHEN IsNull(Nama,'')='' THEN NamaAlias ELSE Nama END) AS Nama,IsPPN,HargaJual,DiscBeli1,DiscBeli2,DiscBeli3,HargaBeliPcs,HargaBeli,CtnPcs FROM MBarang WHERE IsActive=1 AND (ISNULL(KODE,'')<>'' OR ISNULL(KodeAlias,'')<>'') AND (UPPER(Kode) = '" & NamaBarang.Replace("'", "''").ToUpper & "' OR UPPER(NAMA) = '" & NamaBarang.Replace("'", "''").ToUpper & "' OR UPPER(KodeAlias) = '" & NamaBarang.Replace("'", "''").ToUpper & "' OR UPPER(NamaAlias) = '" & NamaBarang.Replace("'", "''").ToUpper & "') ORDER BY Kode"
                oDS = ExecuteDataset("Tbl", SQL)
                If oDS.Tables("Tbl").Rows.Count >= 1 Then
                    NamaBarang = NullToStr(oDS.Tables(0).Rows(0).Item("Nama"))
                    KodeBarang = NullToStr(oDS.Tables(0).Rows(0).Item("Kode"))
                    IDBarang = NullToLong(oDS.Tables(0).Rows(0).Item("NoID"))
                    IsPPn = NullToLong(oDS.Tables(0).Rows(0).Item("IsPPN"))
                    HargaJual = NullToDbl(oDS.Tables(0).Rows(0).Item("HargaJual"))
                    HargaBeliPcs = NullToDbl(oDS.Tables(0).Rows(0).Item("HargaBeliPcs"))
                    Disc1 = NullToDbl(oDS.Tables(0).Rows(0).Item("DiscBeli1"))
                    Disc2 = NullToDbl(oDS.Tables(0).Rows(0).Item("DiscBeli2"))
                    Disc3 = NullToDbl(oDS.Tables(0).Rows(0).Item("DiscBeli3"))
                    HargaBeli = NullToDbl(oDS.Tables(0).Rows(0).Item("HargaBeli"))
                    CtnPcs = NullToDbl(oDS.Tables(0).Rows(0).Item("CtnPcs"))

                    x = True
                Else
                    x = False
                End If
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
        Dim isPPn As Boolean = False
        Dim HargaJual As Double = 0.0
        Dim HargaBeliPcs As Double = 0.0
        Dim Disc1 As Double = 0.0
        Dim Disc2 As Double = 0.0
        Dim Disc3 As Double = 0.0
        Dim HargaBeli As Double = 0.0
        Dim CtnPcs As Double = 0.0
        Dim IDDetil As Long = -1
        'Dim frmEntri As New frmEntriPOD
        Dim Konversi As Double = 0.0
        Try
            If CariBarang(IDBarang, NamaBarang, KodeBarang, isPPn, HargaJual, HargaBeliPcs, Disc1, Disc2, Disc3, HargaBeli, CtnPcs) Then
                If XtraMessageBox.Show("Ingin menambah Item PO dengan Kode Barang " & KodeBarang & " Nama Barang " & NamaBarang, "Fast Entri Says", MessageBoxButtons.YesNo) = Windows.Forms.DialogResult.Yes Then
                    'IDDetil = GetNewID("MPOD", "NoID")
                    'Konversi = NullToDbl(EksekusiSQlSkalarNew("SELECT Konversi FROM MBarangD WHERE IsBeli=1 AND IDBarang=" & IDBarang & " AND IDSatuan=" & DefIDSatuan))
                    'If Konversi <> 0 Then
                    '    SQL = "INSERT INTO MPOD (NoID,IDBarang,IDSatuan,Konversi,IsPPN,HargaBeliPcs,Disc1,Disc2,Disc3,HargaBeli,CtnPcs,HargaJualPcs,IDWilayah,IDPO,NoUrut,Tgl,Jam) VALUES " & vbCrLf
                    '    SQL &= "(" & IDDetil & "," & IDBarang & "," & DefIDSatuan & "," & FixKoma(Konversi) & "," & NullTolInt(isPPn) & "," & FixKoma(HargaBeliPcs) & "," & FixKoma(Disc1) & "," & FixKoma(Disc2) & "," & FixKoma(Disc3) & ", " & FixKoma(HargaBeli) & "," & FixKoma(CtnPcs) & "," & FixKoma(HargaJual) & "," & NullToLong(txtWilayah.EditValue) & "," & NoID & "," & GetNewID("MPOD", "NoUrut", " WHERE IDPO=" & NoID) & ",GetDate(),GetDate())"
                    'Else
                    '    SQL = "INSERT INTO MPOD (NoID,IDBarang,IDWilayah,IsPPN, HargaBeliPcs, Disc1, Disc2, Disc3, HargaBeli, CtnPcs,HargaJualPcs,IDPO,NoUrut,Tgl,Jam) VALUES " & vbCrLf
                    '    SQL &= "(" & IDDetil & "," & IDBarang & "," & NullToLong(txtWilayah.EditValue) & "," & NullTolInt(isPPn) & "," & FixKoma(HargaBeliPcs) & "," & FixKoma(Disc1) & "," & FixKoma(Disc2) & "," & FixKoma(Disc3) & ", " & FixKoma(HargaBeli) & "," & FixKoma(CtnPcs) & "," & FixKoma(HargaJual) & "," & NoID & "," & GetNewID("MPOD", "NoUrut", " WHERE IDPO=" & NoID) & ",GetDate(),GetDate())"
                    'End If
                    'EksekusiSQL(SQL)
                    If VPOINT.Forms.EntriPOD.anInstance Is Nothing Then
                        frmEntri = VPOINT.Forms.EntriPOD.Instance
                    End If
                    frmEntri.IsNew = True
                    frmEntri.IDPO = NoID
                    frmEntri.IDWilayah = NullToLong(txtWilayah.EditValue)
                    frmEntri.IDTypePPN = NullTolInt(TypePajak.EditValue)
                    frmEntri.txtBarang.EditValue = IDBarang
                    frmEntri.IsFastEntri = False
                    frmEntri.formPemanggil = Me
                    frmEntri.Show()
                    frmEntri.Focus()
                    'If frmEntri.ShowDialog(Me)= Windows.Forms.DialogResult.OK Then
                    '    RefreshDetil()
                    '    GV1.ClearSelection()
                    '    GV1.FocusedRowHandle = GV1.LocateByDisplayText(0, GV1.Columns("NoID"), IDDetil.ToString("#,##0"))
                    '    GV1.SelectRow(GV1.FocusedRowHandle)
                    'Else
                    '    SQL = "DELETE FROM MPOD WHERE NoID=" & IDDetil
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

    Private Sub cmdBAru_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdBAru.Click
        'Dim frmEntri As New frmEntriPOD
        Try
            Dim IDDetil As Long = -1
            If VPOINT.Forms.EntriPOD.anInstance Is Nothing Then
                frmEntri = VPOINT.Forms.EntriPOD.Instance
            End If
            frmEntri.NoID = IDDetil
            frmEntri.IsNew = True
            frmEntri.IDPO = NoID
            frmEntri.IDTypePPN = NullToLong(TypePajak.EditValue)
            If txtKodeSupplier.Text <> "" Then
                If NullToBool(EksekusiSQlSkalarNew("SELECT MPO.IsPosted FROM MPO WHERE MPO.NoID=" & NoID)) Then 'Nota DiPosting
                    XtraMessageBox.Show("Nota Sudah Di Posting/Lock.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
                Else
                    frmEntri.IDSupplier = NullToLong(txtKodeSupplier.EditValue)
                    frmEntri.NoID = IDDetil
                    frmEntri.IsNew = True
                    frmEntri.IDPO = NoID
                    frmEntri.IDWilayah = NullToLong(txtWilayah.EditValue)
                    frmEntri.formPemanggil = Me
                    frmEntri.Show()
                    frmEntri.Focus()
                    'If frmEntri.ShowDialog(Me)= Windows.Forms.DialogResult.OK Then
                    '    RefreshDetil()
                    'End If
                    'txtBarang.Focus()
                End If
            Else
                XtraMessageBox.Show("Isi dulu supplier, lalu click baru.", NamaAplikasi, MessageBoxButtons.OK)
            End If
        Catch ex As Exception
            XtraMessageBox.Show("Untuk membuat baru tekan tombol baru", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            'frmEntri.Dispose()
        End Try
    End Sub

    Private Sub cmdEdit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdEdit.Click
        Dim view As ColumnView = GC1.FocusedView
        Try
            Dim row As System.Data.DataRow = view.GetDataRow(GV1.FocusedRowHandle)
            Dim dc As Integer = GV1.FocusedRowHandle
            Dim IDDetil As Long = NullToLong(row("NoID"))
            If txtKodeSupplier.Text <> "" Then
                If NullToBool(EksekusiSQlSkalarNew("SELECT MPO.IsPosted FROM MPO WHERE MPO.NoID=" & NoID)) Then 'Nota DiPosting
                    XtraMessageBox.Show("Nota Sudah Di Posting/Lock.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
                Else
                    If VPOINT.Forms.EntriPOD.anInstance Is Nothing Then
                        frmEntri = VPOINT.Forms.EntriPOD.Instance
                    End If
                    frmEntri.IDSupplier = NullToLong(txtKodeSupplier.EditValue)
                    frmEntri.IsNew = False
                    frmEntri.NoID = IDDetil
                    frmEntri.IDPO = NoID
                    frmEntri.IDWilayah = NullToLong(txtWilayah.EditValue)
                    frmEntri.IDTypePPN = NullToLong(TypePajak.EditValue)
                    frmEntri.formPemanggil = Me
                    frmEntri.Show()
                    frmEntri.Focus()
                    'If frmEntri.ShowDialog(Me)= Windows.Forms.DialogResult.OK Then
                    '    RefreshDetil()
                    'End If
                    'txtBarang.Focus()
                End If
            Else
                XtraMessageBox.Show("Isi dulu supplier, lalu click Edit.", NamaAplikasi, MessageBoxButtons.OK)
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

    Private Sub txtNamaSupplier_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtNamaSupplier.EditValueChanged

    End Sub

    Private Sub Tgl_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Tgl.KeyDown
        If e.KeyCode = Keys.F7 Then
            Dim x As New frmOtorisasiAdmin
            Try
                If Tgl.Properties.ReadOnly Then
                    If x.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
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
                'txtBarang.Focus()
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

    Private Sub txtSubtotal_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtSubtotal.EditValueChanged
        HitungTotal()
    End Sub

    Private Sub txtWilayah_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtWilayah.EditValueChanged
        If pTipe = pStatus.Baru Then
            txtKode.Text = clsKode.MintaKodeSPPBaru("SP", "MPO", Tgl.DateTime, NullToLong(txtWilayah.EditValue), 7)
        End If
    End Sub

    Private Sub txtDiscPersen_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtDiscPersen.EditValueChanged
        Diskon(True)
        HitungTotal()
    End Sub

    Private Sub txtDiscRp_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtDiscRp.EditValueChanged
        Diskon(False)
        HitungTotal()
    End Sub
    Private Sub Diskon(ByVal IsPersen As Boolean)
        If IsPersen Then
            txtDiscRp.EditValue = txtSubtotal.EditValue * txtDiscPersen.EditValue / 100
            'Else
            '    txtDiscPersen.EditValue = txtDiscRp.EditValue / IIf(txtSubtotal.EditValue = 0, 1, txtSubtotal.EditValue) * 100
        End If
    End Sub
    Private Sub txtSubtotal2_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtSubtotal2.EditValueChanged

    End Sub

    Private Sub txtSubtotal2_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtSubtotal2.LostFocus
        HitungTotal()
    End Sub

    Private Sub txtWilayah_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtWilayah.KeyDown
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

    Private Sub GC1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles GC1.Click

    End Sub

    Private Sub txtBiaya_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtBiaya.EditValueChanged
        HitungTotal()
    End Sub

    Private Sub TypePajak_ButtonClick(ByVal sender As Object, ByVal e As DevExpress.XtraEditors.Controls.ButtonPressedEventArgs) Handles TypePajak.ButtonClick
        If e.Button.Index = 1 Then
            Dim HargaBeli As Double = 0.0
            Dim TotalHargaBeli As Double = 0.0
            Dim SQL As String = ""
            Try
                RefreshDetil()
                If IsTempInsertBaru Or pTipe = pStatus.Edit Then
                    For i As Integer = 0 To GV1.RowCount
                        HargaBeli = IIf(NullToLong(TypePajak.EditValue) = 2, 1.1, 1.0) * (Bulatkan(NullToDbl(GV1.GetRowCellValue(i, "Harga")) * (1 - (NullToDbl(GV1.GetRowCellValue(i, "DiscPersen1")) / 100)) * (1 - (NullToDbl(GV1.GetRowCellValue(i, "DiscPersen2")) / 100)) * (1 - (NullToDbl(GV1.GetRowCellValue(i, "DiscPersen3")) / 100)), 2) - NullToDbl(GV1.GetRowCellValue(i, "Disc1")))
                        TotalHargaBeli = HargaBeli * NullToDbl(GV1.GetRowCellValue(i, "Qty"))
                        GV1.SetRowCellValue(i, "HargaBeliSatuan", HargaBeli)
                        GV1.SetRowCellValue(i, "HargaPcs", HargaBeli)

                        GV1.SetRowCellValue(i, "TotalHargaBeli", TotalHargaBeli)
                        GV1.SetRowCellValue(i, "Jumlah", TotalHargaBeli)

                        SQL = "UPDATE MPOD SET HargaPcs=" & FixKoma(HargaBeli) & ", Jumlah=" & FixKoma(TotalHargaBeli) & " WHERE NoID=" & NullToLong(GV1.GetRowCellValue(i, "NoID"))
                        EksekusiSQL(SQL)
                    Next
                    'RefreshDetil()
                End If
            Catch ex As Exception
            Finally
                HitungTotal()
            End Try
        End If
    End Sub

    Private Sub TypePajak_EditValueChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles TypePajak.EditValueChanged

    End Sub

    Private Sub SimpleButton1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SimpleButton1.Click
        Dim SQL As String
        Dim ds As New DataSet
        Dim dsBarang As New DataSet
        Dim Konversi As Double = 1
        Dim HargaSupplier As Double = 0
        Dim DiscProsen1 As Double = 0
        Dim DiscProsen2 As Double = 0
        Dim DiscProsen3 As Double = 0
        Dim DiscRp As Double = 0
        Dim HargaJual As Double = 0
        Dim HargaBeli As Double = 0
        Dim TotalBeli As Double = 0
        Dim TotalJual As Double = 0
        Try
            If Not IsPosted Then
                If XtraMessageBox.Show("Ingin mengupdate ulang harga menurut master Barang yang terbaru?", NamaAplikasi, MessageBoxButtons.YesNo, MessageBoxIcon.Information, MessageBoxDefaultButton.Button2) = Windows.Forms.DialogResult.Yes Then
                    SQL = "SELECT * FROM MPOD WHERE IDPO=" & NoID
                    ds = ExecuteDataset("MPOD", SQL)
                    For i As Integer = 0 To ds.Tables("MPOD").Rows.Count - 1
                        Konversi = NullToDbl(EksekusiSQlSkalarNew("Select Isi from (" & _
                                   " SELECT MSatuan.NoID, MSatuan.Kode, MSatuan.Nama,Mbarang.CtnPcs AS Isi FROM MSatuan inner join mbarang on MBarang.IDSatuanHarga=Msatuan.NoID where MBarang.NoID=" & NullToLong(ds.Tables("MPOD").Rows(i).Item("IDBarang")) & " " & _
                                   " UNION ALL SELECT MSatuan.NoID, MSatuan.Kode, MSatuan.Nama,1.0 AS Isi FROM MSatuan inner join mbarang on MBarang.IDSatuan=Msatuan.NoID where MBarang.NoID=" & NullToLong(ds.Tables("MPOD").Rows(i).Item("IDBarang")) & " ) X where NoID=" & NullToLong(ds.Tables("MPOD").Rows(i).Item("IDSatuan"))))
                        SQL = "SELECT (MBarang.HargaBeliPcsBruto*MBarang.CtnPcs/MBarang.Konversi) AS HargaBeliPcsBruto, (MBarang.HargaBeli*MBarang.CtnPcs/MBarang.Konversi) AS HargaBeli,MBarang.HargaJual,MBarang.CtnPcs,MBarang.DiscBeli1,MBarang.DiscBeli2,MBarang.DiscBeli3,Mbarang.DiscRp From Mbarang where NoID=" & NullToLong(ds.Tables("MPOD").Rows(i).Item("IDBarang"))
                        dsBarang = ExecuteDataset("TabelBarang", SQL)
                        If dsBarang.Tables("TabelBarang").Rows.Count >= 1 Then
                            'Konversi 1 Dianggap Harga PCS
                            If NullToDbl(Konversi) = 1.0 Then 'NullToDbl(dsBarang.Tables("TabelBarang").Rows(0).Item("CtnPcs")) Then
                                HargaSupplier = NullToDbl(dsBarang.Tables("TabelBarang").Rows(0).Item("HargaBeliPcsBruto"))
                            Else
                                HargaSupplier = NullToDbl(dsBarang.Tables("TabelBarang").Rows(0).Item("HargaBeli"))
                            End If
                            DiscProsen1 = NullToDbl(dsBarang.Tables("TabelBarang").Rows(0).Item("DiscBeli1"))
                            DiscProsen2 = NullToDbl(dsBarang.Tables("TabelBarang").Rows(0).Item("DiscBeli2"))
                            DiscProsen3 = NullToDbl(dsBarang.Tables("TabelBarang").Rows(0).Item("DiscBeli3"))
                            DiscRp = NullToDbl(dsBarang.Tables("TabelBarang").Rows(0).Item("DiscRp"))
                            HargaJual = NullToDbl(dsBarang.Tables("TabelBarang").Rows(0).Item("HargaJual"))
                        Else
                            HargaSupplier = 0
                            DiscProsen1 = 0
                            DiscProsen2 = 0
                            DiscProsen3 = 0
                            DiscRp = 0
                            HargaJual = 0
                        End If
                        HargaBeli = IIf(TypePajak.EditValue = 2, 1.1, 1.0) * (Bulatkan(HargaSupplier * (1 - (DiscProsen1 / 100)) * (1 - (DiscProsen2 / 100)) * (1 - (DiscProsen3 / 100)), 2) - DiscRp) / IIf(NullToDbl(Konversi) = 0, 1, NullToDbl(Konversi))
                        TotalBeli = HargaBeli * NullToDbl(ds.Tables("MPOD").Rows(i).Item("Qty"))
                        TotalBeli = HargaJual * NullToDbl(ds.Tables("MPOD").Rows(i).Item("Qty")) * NullToDbl(ds.Tables("MPOD").Rows(i).Item("Konversi"))

                        SQL = "UPDATE MPOD SET " & _
                              " Harga=" & FixKoma(HargaSupplier) & "," & _
                              " HargaPcs=" & FixKoma(HargaBeli) & "," & _
                              " DiscPersen1=" & FixKoma(DiscProsen1) & "," & _
                              " DiscPersen2=" & FixKoma(DiscProsen2) & "," & _
                              " DiscPersen3=" & FixKoma(DiscProsen3) & "," & _
                              " Disc1=" & FixKoma(DiscRp) & "," & _
                              " Jumlah=" & FixKoma(TotalBeli) & "," & _
                              " HargaNetto=" & FixKoma(HargaBeli) & "," & _
                              " HargaJualPcs=" & FixKoma(HargaJual) & _
                              " WHERE NoID=" & NullToLong(ds.Tables("MPOD").Rows(i).Item("NoID"))
                        EksekusiSQL(SQL)
                    Next
                    RefreshDetil()
                End If
            End If
        Catch ex As Exception
            XtraMessageBox.Show("Info Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            If Not ds Is Nothing Then
                ds.Dispose()
            End If
            If Not dsBarang Is Nothing Then
                dsBarang.Dispose()
            End If
        End Try
    End Sub

    Private Sub txtTargetSupplier_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtTargetSupplier.EditValueChanged
        HitungTotal()
    End Sub

    Private Sub mnGeneratePO_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles mnGeneratePO.Click
        Dim frm As New frmGeneratePO
        frm.IDPO = NoID
        frm.IDSupplier = NullToLong(txtAlamatSupplier.EditValue)
        If frm.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
            RefreshDetil()
            'Tambah Order
        End If
        frm.Dispose()
    End Sub

    Private Sub txtTotal_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtTotal.EditValueChanged

    End Sub
End Class