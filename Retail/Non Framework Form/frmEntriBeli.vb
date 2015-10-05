Imports System.Data
Imports System.Data.SqlClient
Imports DevExpress.XtraEditors
Imports DevExpress.Utils
Imports DevExpress.XtraGrid.Views.Base

Public Class frmEntriBeli
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
    Dim IDWilayah As Long = DefIDWilayah
    Dim IDPOOld As Long = -1

    'For Back Action
    Public FormNameDaftar As String = ""
    Public TableNameDaftar As String = ""
    Public TextDaftar As String = ""
    Public FormEntriDaftar As String = ""
    Public TableMasterDaftar As String = ""
    Dim IsTempInsertBaru As Boolean = False
    'Dim frmEntri As frmEntriBeliD = VPOINT.Forms.EntriBeliD.Instance
    Public IsRevisi As Boolean = False
    Dim IsLoad As Boolean = True

    Public Sub SimpanTambahan()
        Try
            HitungTotal()
            SQL = "UPDATE MBeli SET "
            SQL &= " SubTotal=" & FixKoma(txtSubtotal.EditValue) & ","
            SQL &= " DiskonNotaProsen=" & FixKoma(txtDiscPersen.EditValue) & ","
            SQL &= " DiskonNotaRp=" & FixKoma(txtDiscRp.EditValue) & ","
            SQL &= " DiskonNotaTotal=" & FixKoma(txtDiscTotal.EditValue) & ","
            SQL &= " Biaya=" & FixKoma(txtBiaya.EditValue) & ","
            SQL &= " Total=" & FixKoma(txtTotal.EditValue) & ","
            'SQL &= " PPN=" & FixKoma(txtPPN.EditValue) & ","
            SQL &= " Bayar=" & FixKoma(txtBayar.EditValue) & ","
            SQL &= " Sisa=" & FixKoma(txtSisa.EditValue)
            SQL &= " WHERE NoID=" & NoID
            EksekusiSQL(SQL)
        Catch ex As Exception

        End Try
    End Sub
    Private Sub RefreshDataKontak()
        Dim ds As New DataSet
        Try
            SQL = "SELECT NoID, Kode, Nama FROM MAlamat WHERE IsActive=1 AND IsSupplier=1"
            ds = ExecuteDataset("master", SQL)
            txtKodeSupplier.Properties.DataSource = ds.Tables("master")
            If System.IO.File.Exists(FolderLayouts & Me.Name & SearchLookUpEdit1View.Name & ".xml") Then
                SearchLookUpEdit1View.RestoreLayoutFromXml(FolderLayouts & Me.Name & SearchLookUpEdit1View.Name & ".xml")
            End If

            SQL = "SELECT MGudang.NoID, MGudang.Kode, MGudang.Kode + ' - ' + MGudang.Nama AS Nama, MWilayah.Nama AS Wilayah FROM MGudang LEFT JOIN MWilayah ON MGudang.IDWilayah=MWilayah.NoID WHERE MGudang.IsActive=1 AND MGudang.InTransit=0 AND MGudang.IDWilayah=" & DefIDWilayah
            ds = ExecuteDataset("master", SQL)
            txtGudang.Properties.DataSource = ds.Tables("master")
            txtGudang.Properties.ValueMember = "NoID"
            txtGudang.Properties.DisplayMember = "Nama"
            If System.IO.File.Exists(FolderLayouts & Me.Name & gvGudang.Name & ".xml") Then
                gvGudang.RestoreLayoutFromXml(FolderLayouts & Me.Name & gvGudang.Name & ".xml")
            End If
        Catch ex As Exception
            XtraMessageBox.Show("Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            ds.Dispose()
        End Try
    End Sub

    Private Sub frmEntriBeli_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        If IsTempInsertBaru Then
            If XtraMessageBox.Show("Yakin ingin keluar?" & vbCrLf & "Ya, akan menghapus transaksi dan detil item.", NamaAplikasi, MessageBoxButtons.YesNo, MessageBoxIcon.Information, MessageBoxDefaultButton.Button2) = Windows.Forms.DialogResult.Yes Then
                If Not NullToBool(EksekusiSQlSkalarNew("SELECT isnull(IsPosted,0) FROM  MBeli  WHERE NoID=" & NoID)) Then
                    EksekusiSQL("DELETE FROM MBeli WHERE NoID=" & NoID)
                    EksekusiSQL("DELETE FROM MBeliD WHERE IDBeli=" & NoID)
                Else
                    MsgBox("Data ini telah terposting, anda harus menyimpannya!", MsgBoxStyle.Information)
                End If
            Else
                e.Cancel = True
            End If
        Else

        End If
    End Sub

    'Private Sub RefreshDataSTB()
    '    Dim ds As New DataSet
    '    Try
    '        SQL = "SELECT     MLPB.NoID, MLPB.Kode, MLPB.Tanggal, MAlamat.Nama AS Supplier, MAlamat_1.Nama AS Penerima"
    '        SQL &= " FROM MAlamat AS MAlamat_1 RIGHT OUTER JOIN"
    '        SQL &= " MLPB ON MAlamat_1.NoID = MLPB.IDBagPembelian LEFT OUTER JOIN"
    '        SQL &= " MAlamat ON MLPB.IDSupplier = MAlamat.NoID "
    '        SQL &= " WHERE MLPB.IDSupplier=" & NullTolong(txtKodeSupplier.EditValue)
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
            If System.IO.File.Exists(folderLayouts & Me.Name & GV1.Name & ".xml") Then
                GV1.RestoreLayoutFromXml(folderLayouts & Me.Name & GV1.Name & ".xml")
            End If
            If System.IO.File.Exists(folderLayouts & Me.Name & SearchLookUpEdit1View.Name & ".xml") Then
                SearchLookUpEdit1View.RestoreLayoutFromXml(folderLayouts & Me.Name & SearchLookUpEdit1View.Name & ".xml")
            End If

            FungsiControl.SetForm(Me)

            HighLightTxt()
            Me.Text = "Entri BPB"
            IsLoad = False
            If IsRevisi AndAlso NullToDbl(txtDiscTotal.EditValue) = 0 AndAlso System.Math.Round(txtSubtotal.EditValue - txtDiscTotal.EditValue) <> txtTotal.EditValue Then
                txtDiscRp.EditValue = (txtSubtotal.EditValue + txtDiscTotal.EditValue) - System.Math.Round(txtSubtotal.EditValue)
            End If
            If IsRevisi Then
                HitungTotal()
                txtTotal2.Properties.ReadOnly = False
                txtTotal2.Focus()
                txtTotal2.EditValue = txtTotal.EditValue

                txtSubtotal.Properties.Mask.EditMask = "n3"
                txtTotal.Properties.Mask.EditMask = "n3"
                txtTotal2.Properties.Mask.EditMask = "n3"
                txtDiscRp.Properties.Mask.EditMask = "n3"
                txtDiscTotal.Properties.Mask.EditMask = "n3"
                txtSubtotal2.Properties.Mask.EditMask = "n3"
            End If
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
                SQL = "SELECT MBeli.*, MAlamat.Alamat AS Alamat, MAlamat.Nama AS NamaAlamat, MAlamat.Kode as KodeAlamat FROM MBeli LEFT JOIN MAlamat ON MAlamat.NoID=MBeli.IDSupplier WHERE MBeli.NoID=" & NoID
                DS = ExecuteDataset("Tabel", SQL)
                If DS.Tables("Tabel").Rows.Count >= 1 Then
                    txtKodeSupplier.EditValue = NullTolong(DS.Tables(0).Rows(0).Item("IDSupplier"))
                    'txtKodeSupplier.Text = NullTostr(DS.Tables(0).Rows(0).Item("KodeAlamat"))
                    txtGudang.EditValue = NullToLong(DS.Tables(0).Rows(0).Item("IDGudang"))
                    RubahGudang()
                    IDWilayah = NullToLong(DS.Tables(0).Rows(0).Item("IDWilayah"))
                    txtNamaSupplier.Text = NullToStr(DS.Tables(0).Rows(0).Item("NamaAlamat"))
                    txtAlamatSupplier.Text = NullTostr(DS.Tables(0).Rows(0).Item("Alamat"))
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
                    txtDiscTotal.EditValue = NullToDbl(DS.Tables(0).Rows(0).Item("DiskonNotaTotal"))
                    txtBiaya.EditValue = NullToDbl(DS.Tables(0).Rows(0).Item("Biaya"))
                    'HitungTotal()
                    txtTotal.EditValue = NullToDbl(DS.Tables(0).Rows(0).Item("Total"))
                    'txtPPN.EditValue = NullToDbl(DS.Tables(0).Rows(0).Item("PPN"))
                    txtBayar.EditValue = NullToDbl(DS.Tables(0).Rows(0).Item("Bayar"))
                    txtSisa.EditValue = NullToDbl(DS.Tables(0).Rows(0).Item("Sisa"))
                    IsPosted = NullToBool(DS.Tables(0).Rows(0).Item("IsPosted"))
                    txtDiscRp.EditValue = NullToDbl(DS.Tables(0).Rows(0).Item("DiskonNotaRp"))
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
    Public Sub RefreshDetil()
        Dim strsql As String
        Dim curentcursor As Cursor = Windows.Forms.Cursor.Current
        Dim repChekEdit As New DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit
        Windows.Forms.Cursor.Current = Cursors.WaitCursor
        Try
            strsql = "select MBeliD.*, MBeliD.Jumlah+MBeliD.Biaya as Total,MBarangD.Barcode,MMerk.Nama as Merk, MBarang.CtnPcs AS IsiCtn, MBeliD.Disc1 AS DiscRp, MLPB.Kode AS KodePenerimaan, MLPB.Tanggal AS TglPenerimaan, MPO.Kode AS KodePO, MPO.Tanggal AS TglPO, MBarang.Kode as KodeBarang,MWilayah.Nama AS Wilayah, MBarang.KodeAlias, MBarang.NamaAlias, MBarang.Nama + ' ' + MBarangD.Varian AS NamaBarang,MSatuan.Kode as Satuan,Mgudang.Kode as KodeGudang, MBarangD.HargaJualNetto AS HargaJual, MBarangD.HargaJualNetto*MBeliD.Qty AS NilaiJual " & vbCrLf
            strsql &= " FROM (MBeliD Inner Join MBeli On MBeliD.IDBeli=MBeli.NoID) " & vbCrLf
            strsql &= " LEFT JOIN MBarangD ON MBeliD.IDBarangD=MBarangD.NoID " & vbCrLf
            strsql &= " LEFT JOIN MBarang ON MBeliD.IDBarang=MBarang.NoID " & vbCrLf
            strsql &= " LEFT JOIN MSatuan ON MBeliD.IDSatuan=MSatuan.NoID " & vbCrLf
            strsql &= " LEFT JOIN MMerk ON MBarang.IDMerk=MMerk.NoID " & vbCrLf
            strsql &= " LEFT JOIN (MPOD LEFT JOIN MPO ON MPO.NoID=MPOD.IDPO) ON MBeliD.IDPOD=MPOD.NoID " & vbCrLf
            strsql &= " LEFT JOIN (MLPBD LEFT JOIN MLPB ON MLPB.NoID=MLPBD.IDLPB) ON MBeliD.IDLPBD=MLPBD.NoID " & vbCrLf
            strsql &= " LEFT JOIN (MGudang LEFT JOIN MWilayah ON MWilayah.NoID=MGudang.IDWilayah) ON MBeliD.IDGudang=MGudang.NoID " & vbCrLf
            strsql &= " where MBeliD.IDBeli = " & NoID
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
            HitungTotal()
            If pTipe = pStatus.Edit Then
                RefreshPO()
            End If
            GV1.HideFindPanel()
        Catch ex As Exception
            XtraMessageBox.Show(ex.Message, ".:: Pesan Kesalahan ::.", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
        Windows.Forms.Cursor.Current = curentcursor
        Application.DoEvents()
    End Sub
    Private Sub HitungTotal(Optional ByVal IsHitungTotal2 As Boolean = True)
        Dim temp As Double = 0.0
        Try
            For i = 0 To GV1.RowCount
                temp = temp + NullToDbl(GV1.GetRowCellValue(i, "Jumlah"))
            Next
            txtSubtotal.EditValue = temp
            txtDiscTotal.EditValue = txtDiscRp.EditValue '(NullToDbl(txtDiscPersen.EditValue) * NullToDbl(txtSubtotal.EditValue) / 100) '+ txtDiscRp.EditValue
            txtSubtotal2.EditValue = NullToDbl(txtSubtotal.EditValue) - NullToDbl(txtDiscTotal.EditValue)
            'txtPPN.EditValue = Bulatkan(NullToDbl(EksekusiSQlSkalarNew("Select SUM(PPN) from MBeliD where IDBeli=" & NoID)), 0)
            txtTotal.EditValue = txtSubtotal.EditValue - txtDiscTotal.EditValue + txtBiaya.EditValue
            'If IsHitungTotal2 Then
            '    txtTotal2.EditValue = txtTotal.EditValue
            'End If
            txtSisa.EditValue = txtTotal.EditValue - txtBayar.EditValue
            txtTotalCTN.EditValue = NullToDbl(EksekusiSQlSkalarNew("SELECT SUM(Ctn) FROM MBeliD WHERE IDBeli=" & NoID))

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

        txtKodeSupplier.EditValue = DefIDSupplier
        txtGudang.EditValue = DefIDGudang
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

        If pTipe = pStatus.Baru AndAlso Not IsRevisi Then
            cmdBAru.Enabled = False
            cmdEdit.Enabled = False
            cmdDelete.Enabled = False
        ElseIf pTipe = pStatus.Edit AndAlso IsPosted Then
            If IsRevisi Then
                cmdBAru.Enabled = False
                cmdEdit.Enabled = True
                cmdDelete.Enabled = False
            Else
                cmdBAru.Enabled = False
                cmdEdit.Enabled = False
                cmdDelete.Enabled = False
            End If
        Else
            cmdBAru.Enabled = True
            cmdEdit.Enabled = True
            cmdDelete.Enabled = True
        End If
    End Sub

    Private Sub IsCekSisaPO()
        Dim ds As New DataSet
        Try
            SQL = "SELECT MPOD.NoID, (MPOD.Qty*MPOD.Konversi)-ISNULL((SELECT SUM(MBeliD.Qty*MBeliD.Konversi) FROM MBeliD WHERE MBeliD.IDPOD=MPOD.NoID),0) AS Sisa" & vbCrLf & _
                  " FROM MPOD WHERE (MPOD.Qty*MPOD.Konversi)-ISNULL((SELECT SUM(MBeliD.Qty*MBeliD.Konversi) FROM MBeliD WHERE MBeliD.IDPOD=MPOD.NoID),0)>1 AND MPOD.NoID IN (SELECT MBeliD.IDPOD FROM MBeliD WHERE MBeliD.IDBeli=" & NoID & ")"
            ds = ExecuteDataset("MPOD", SQL)
            If ds.Tables("MPOD").Rows.Count >= 1 Then
                If XtraMessageBox.Show("Ada Sisa PO, apakah sisa-sisa PO ini dianggap selesai?", NamaAplikasi, MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                    For i As Integer = 0 To ds.Tables("MPOD").Rows.Count - 1
                        EksekusiSQL("UPDATE MPOD SET IsSelesai=1, IDUserSelesai=" & IDUserAktif & ", TglSelesai=getdate() WHERE NoID=" & NullToLong(ds.Tables("MPOD").Rows(i).Item("NoID")))
                    Next
                End If
            End If
        Catch ex As Exception
            XtraMessageBox.Show("Info Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
        Finally
            ds.Dispose()
        End Try
    End Sub

    Private Sub cmdSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSave.Click
        Dim PesanSalah As String = ""
        If (Not IsPosted) Or (IsPosted AndAlso IsRevisi) Then
            'If pTipe = pStatus.Baru Then
            '    txtKode.Text = clsKode.MintaKodeBaru("BL", "MBeli", Tgl.DateTime, IDWilayah, 5)
            'End If
            If IsValidasi() Then
                If Simpan(PesanSalah) = True Then
                    If pTipe = pStatus.Baru Then
                        KodeLama = txtKode.Text
                        pTipe = pStatus.Edit
                        SetTombol()
                        txtBarang.Focus()
                        IsTempInsertBaru = True
                        RefreshPO()
                        'txtKodeSupplier.Properties.ReadOnly = True
                    Else
                        IsCekSisaPO()
                        clsPostingPembelian.PostingStokBarangPembelian(NoID)
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
        If txtKodeSupplier.Text = "" Then
            XtraMessageBox.Show("Supplier masih kosong.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
            txtKodeSupplier.Focus()
            Return False
            Exit Function
        End If
        If txtGudang.Text = "" Then
            XtraMessageBox.Show("Gudang masih kosong.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
            txtGudang.Focus()
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
        'If Not IsRevisi AndAlso txtDiscRp.EditValue < 0 Then
        '    XtraMessageBox.Show("Diskon rupiah masih dibawah 0.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
        '    txtDiscRp.Focus()
        '    Return False
        '    Exit Function
        'ElseIf txtDiscRp.EditValue > txtSubtotal.EditValue Then
        '    XtraMessageBox.Show("Diskon rupiah melebihi subtotal.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
        '    txtDiscRp.Focus()
        '    Return False
        '    Exit Function
        'End If
        'If txtDiscPersen.EditValue > 100 Then
        '    XtraMessageBox.Show("Diskon tidak boleh diatas 100%.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
        '    txtDiscPersen.Focus()
        '    Return False
        '    Exit Function
        'ElseIf txtDiscPersen.EditValue < 0 Then
        '    XtraMessageBox.Show("Diskon persen masih dibawah 0.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
        '    txtDiscPersen.Focus()
        '    Return False
        '    Exit Function
        'End If
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
        If CekKodeValid(txtKode.Text, KodeLama, "MBeli", "Kode", IIf(pTipe = pStatus.Edit, True, False)) Then
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
        HitungTotal()
        Try
            If pTipe = pStatus.Baru Then
                NoID = GetNewID("MBeli")
                SQL = "INSERT INTO MBeli (NoID,IDPO,IDGudang,IDWilayah,Kode,KodeReff,Tanggal,TanggalStock,JatuhTempo,"
                SQL &= " IDSupplier,TanggalSJ,NoSJ,SubTotal,DiskonNotaProsen,DiskonNotaRp,DiskonNotaTotal,"
                SQL &= "  Biaya, Total, Bayar, Sisa, IDAdmin) VALUES (" & vbCrLf
                SQL &= NoID & ","
                SQL &= NullToLong(txtGudang.EditValue) & ","
                SQL &= NullToLong(txtPO.EditValue) & ","
                SQL &= IDWilayah & ","
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
                SQL &= FixKoma(txtSisa.EditValue) & "," & IDAdmin & ")"
            Else
                SQL = "UPDATE MBeli SET "
                If Not Tgl.Properties.ReadOnly Or Not txtKode.Properties.ReadOnly Then
                    SQL &= "IDAdmin=" & IDAdmin & ","
                End If
                SQL &= "IDGudang=" & NullToLong(txtGudang.EditValue) & ","
                SQL &= "IDPO=" & NullToLong(txtPO.EditValue) & ","
                SQL &= "IDWilayah=" & IDWilayah & ","
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
                'SQL &= "PPN=" & FixKoma(txtPPN.EditValue) & ","
                SQL &= "Bayar=" & FixKoma(txtBayar.EditValue) & ","
                SQL &= "Sisa=" & FixKoma(txtSisa.EditValue)
                SQL &= " WHERE NoID=" & NoID
            End If
            If EksekusiSQL(SQL) >= 1 Then
                Sukses = True
            Else
                Sukses = False
            End If
            If Sukses Then
                SQL = "UPDATE MBeliD SET IDGudang=" & NullToLong(txtGudang.EditValue) & " WHERE IDBeli=" & NoID
                EksekusiSQL(SQL)
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

                GV1.SaveLayoutToXml(folderLayouts & Me.Name & GV1.Name & ".xml")
                SearchLookUpEdit1View.SaveLayoutToXml(folderLayouts & Me.Name & SearchLookUpEdit1View.Name & ".xml")
                gvGudang.SaveLayoutToXml(FolderLayouts & Me.Name & gvGudang.Name & ".xml")
                gvPO.SaveLayoutToXml(FolderLayouts & Me.Name & gvPO.Name & ".xml")
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
                EksekusiSQL("Delete From MBeliD where NoID=" & IDDetil.ToString)
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
        Try
            txtNamaSupplier.Text = NullTostr(EksekusiSQlSkalarNew("SELECT Nama FROM MAlamat WHERE NoID=" & NullTolong(txtKodeSupplier.EditValue)))
            txtAlamatSupplier.Text = NullTostr(EksekusiSQlSkalarNew("SELECT Alamat FROM MAlamat WHERE NoID=" & NullTolong(txtKodeSupplier.EditValue)))
            TglAdd = NullTolong(EksekusiSQlSkalarNew("SELECT JatuhTempoSupplier FROM MAlamat WHERE NoID=" & NullTolong(txtKodeSupplier.EditValue)))
            If TglAdd = 0 Then
                tglJatuhTempo.DateTime = DateAdd(DateInterval.Month, 2, Tgl.DateTime)
            Else
                tglJatuhTempo.DateTime = DateAdd(DateInterval.Day, TglAdd, Tgl.DateTime)
            End If
            If pTipe = pStatus.Edit Then
                RefreshPO()
            End If
        Catch ex As Exception

        End Try
    End Sub
    Private Sub RefreshPO()
        Dim ds As New DataSet
        Try
            SQL = "SELECT MPO.NoID, MPO.Kode AS NoPO, MPO.Tanggal " & vbCrLf & _
                  " FROM (MPOD " & vbCrLf & _
                  " INNER JOIN MPO ON MPO.NoID=MPOD.IDPO) " & vbCrLf & _
                  " WHERE ((MPOD.Qty*MPOD.Konversi)-IsNull((SELECT SUM(MBeliD.Qty*MBeliD.Konversi) FROM MBeliD INNER JOIN MBeli ON MBeli.NoID=MBeliD.IDBeli WHERE MBeliD.IDPOD=MPOD.NoID),0))>0 AND IsNull(MPOD.IsSelesai,0)=0 AND IsNull(MPO.IsPosted,0)=1 AND MPO.IDWilayah=" & DefIDWilayah & " AND MPO.IDSupplier=" & NullToLong(txtKodeSupplier.EditValue) & vbCrLf & _
                  " GROUP BY MPO.NoID, MPO.Kode, MPO.Tanggal, MPO.IsPosted, MPO.IDWilayah, MPO.IDSupplier"
            ds = ExecuteDataset("MPO", SQL)
            txtPO.Properties.DataSource = ds.Tables("MPO")
            txtPO.Properties.ValueMember = "NoID"
            txtPO.Properties.DisplayMember = "NoPO"
        Catch ex As Exception
            XtraMessageBox.Show("Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
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
            TglAdd = NullTolong(EksekusiSQlSkalarNew("SELECT JatuhTempoSupplier FROM MAlamat WHERE NoID=" & NullTolong(txtKodeSupplier.EditValue)))
            If TglAdd = 0 Then
                tglJatuhTempo.DateTime = DateAdd(DateInterval.Month, 2, Tgl.DateTime)
            Else
                tglJatuhTempo.DateTime = DateAdd(DateInterval.Day, TglAdd, Tgl.DateTime)
            End If
            If pTipe = pStatus.Baru Then
                txtKode.Text = clsKode.MintaKodeBaru("BL", "MBeli", Tgl.DateTime, IDWilayah, 5, "ISNULL(MBELI.IsTanpaBarang,0)=0")
            End If
        Catch ex As Exception

        End Try
    End Sub

    Private Sub txtSubtotal_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtSubtotal.EditValueChanged

    End Sub

    Private Sub txtSubtotal_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtSubtotal.LostFocus
        HitungTotal()
    End Sub

    Private Sub txtDiscPersen_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtDiscPersen.LostFocus
        'txtDiscRp.EditValue = txtSubtotal.EditValue * txtDiscPersen.EditValue / 100
        'HitungTotal()
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
        'txtDiscPersen.EditValue = txtDiscRp.EditValue / IIf(txtSubtotal.EditValue = 0, 1, txtSubtotal.EditValue) * 100
        'HitungTotal()
    End Sub

    Private Sub txtDiscTotal_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtDiscTotal.LostFocus
        HitungTotal()
    End Sub

    Private Sub txtDiscTotal_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtDiscTotal.EditValueChanged

    End Sub

    Private Sub txtBiaya_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtBiaya.EditValueChanged
        HitungTotal()
    End Sub

    Private Sub txtBiaya_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtBiaya.LostFocus
        HitungTotal()
    End Sub

    Private Sub txtTotal_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtTotal.EditValueChanged

    End Sub

    Private Sub txtTotal_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtTotal.KeyDown
        If e.KeyCode = Keys.Enter Then
            cmdSave.PerformClick()
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
    Private Function CariBarang(ByRef IDBarangD As Long, ByRef IDBarang As Long, ByRef NamaBarang As String, ByRef KodeBarang As String, ByRef Barcode As String, ByRef Konversi As Double, ByRef IDSatuan As Integer) As Boolean
        Dim x As Boolean = False
        Dim SQL As String = ""
        Dim oDS As New DataSet
        Try
            SQL = "SELECT MBarangD.NoID,MBarangD.IDBarang,MBarangD.IDSatuan,MBarangD.Konversi,MBarangD.Barcode,MBarang.Kode,MBarang.Nama FROM MBarangD inner join MBarang  on MBarangD.IDBarang=Mbarang.NoID WHERE MBarang.IsActive=1 and MBarangD.IsActive=1 AND (MBarangD.Barcode='" & NamaBarang.Replace("'", "''").ToUpper & "' OR MBarang.Kode = '" & NamaBarang.Replace("'", "''").ToUpper & "') ORDER BY Kode,Konversi ASC"
            oDS = ExecuteDataset("Tbl", SQL)
            If oDS.Tables("Tbl").Rows.Count >= 1 Then
                NamaBarang = NullToStr(oDS.Tables(0).Rows(0).Item("Nama"))
                KodeBarang = NullToStr(oDS.Tables(0).Rows(0).Item("Kode"))
                IDBarangD = NullToLong(oDS.Tables(0).Rows(0).Item("NoID"))
                IDBarang = NullToLong(oDS.Tables(0).Rows(0).Item("IDBarang"))
                IDSatuan = NullToLong(oDS.Tables(0).Rows(0).Item("IDSatuan"))
                Konversi = NullToDbl(oDS.Tables(0).Rows(0).Item("Konversi"))
                Barcode = NullToStr(oDS.Tables(0).Rows(0).Item("Barcode"))
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
        Dim Barcode As String = ""
        Dim IDBarangD As Long = -1
        Dim IDBarang As Long = -1
        Dim IDSatuan As Long = -1
        Dim IDDetil As Long = -1
        Dim frmEntri As New frmEntriBeliD
        Dim Konversi As Double = 0.0
        Try
            If txtKodeSupplier.Text = "" Or txtGudang.Text = "" Then Exit Sub
            If CariBarang(IDBarangD, IDBarang, NamaBarang, KodeBarang, Barcode, Konversi, IDSatuan) Then
                ''If XtraMessageBox.Show("Ingin membeli item dengan Kode Stock " & KodeBarang & " Nama Stock " & NamaBarang, "Fast Entri Says", MessageBoxButtons.YesNo) = Windows.Forms.DialogResult.Yes Then
                'IDDetil = GetNewID("MBeliD", "NoID")
                ''Konversi = NullToDbl(EksekusiSQlSkalarNew("SELECT Konversi FROM MBarangD WHERE IsBeli=1 AND IDBarang=" & IDBarang & " AND IDSatuan=" & DefIDSatuan))
                'If Konversi = 0 Then Konversi = 1
                ''If Konversi <> 0 Then
                'SQL = "INSERT INTO MBeliD (NoID,IDBarangD,IDBarang,IDSatuan,Konversi,IDGudang,IDBeli,NoUrut,Tgl,Jam) VALUES " & vbCrLf
                'SQL &= "(" & IDDetil & "," & IDBarangD & "," & IDBarang & "," & IDSatuan & "," & FixKoma(Konversi) & "," & NullToLong(txtGudang.EditValue) & "," & NoID & "," & GetNewID("MBeliD", "NoUrut", " WHERE IDBeli=" & NoID) & ",GetDate(),GetDate())"
                ''Else
                ''    SQL = "INSERT INTO MBeliD (NoID,IDBarang,IDGudang,IDBeli,NoUrut,Tgl,Jam) VALUES " & vbCrLf
                ''   SQL &= "(" & IDDetil & "," & IDBarang & "," & NullToLong(txtGudang.EditValue) & "," & NoID & "," & GetNewID("MBeliD", "NoUrut", " WHERE IDBeli=" & NoID) & ",GetDate(),GetDate())"
                ''End If
                'EksekusiSQL(SQL)
                If VPoint.Forms.EntriBeliD.adInstance Is Nothing Then
                    frmEntri = VPoint.Forms.EntriBeliD.Instance
                End If
                frmEntri.IsNew = True
                frmEntri.NoID = IDDetil
                frmEntri.IDBeli = NoID
                frmEntri.IDSupplier = NullToLong(txtKodeSupplier.EditValue)
                frmEntri.txtBarcode.EditValue = IDBarangD
                frmEntri.IsFastEntri = False
                frmEntri.FormPemanggil = Me
                frmEntri.Show()
                frmEntri.Focus()
                'If frmEntri.ShowDialog(Me)= Windows.Forms.DialogResult.OK Then
                '    RefreshDetil()
                '    GV1.ClearSelection()
                '    GV1.FocusedRowHandle = GV1.LocateByDisplayText(0, GV1.Columns("NoID"), IDDetil.ToString("#,##0"))
                '    GV1.SelectRow(GV1.FocusedRowHandle)
                'Else
                '    SQL = "DELETE FROM MBeliD WHERE NoID=" & IDDetil
                '    EksekusiSQL(SQL)
                '    RefreshDetil()
                '    GV1.ClearSelection()
                '    GV1.FocusedRowHandle = GV1.LocateByDisplayText(0, GV1.Columns("NoID"), IDDetil.ToString("#,##0"))
                '    GV1.SelectRow(GV1.FocusedRowHandle)
                'End If
                'End If
            End If
            'txtBarang.Focus()
        Catch ex As Exception
            XtraMessageBox.Show("Kesalahan : " & ex.Message, "Fast Entri Says", MessageBoxButtons.OK)
            'frmEntri.Dispose()
        End Try
    End Sub

    Private Sub cmdBAru_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdBAru.Click
        Dim frmEntri As New frmEntriBeliD
        Try
            Dim IDDetil As Long = -1
            frmEntri.NoID = IDDetil
            frmEntri.IsNew = True
            frmEntri.IDBeli = NoID
            If txtKodeSupplier.Text <> "" Then
                frmEntri.IDSupplier = NullToLong(txtKodeSupplier.EditValue)
                frmEntri.IDGudang = NullToLong(txtGudang.EditValue)
                frmEntri.NoID = IDDetil
                frmEntri.IsNew = True
                frmEntri.IDBeli = NoID
                frmEntri.FormPemanggil = Me
                frmEntri.Show()
                frmEntri.Focus()
                'If frmEntri.ShowDialog(Me)= Windows.Forms.DialogResult.OK Then
                '    RefreshDetil()
                'End If
                'txtBarang.Focus()
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
        Dim frmEntri As New frmEntriBeliD
        Dim view As ColumnView = GC1.FocusedView
        Try
            Dim row As System.Data.DataRow = view.GetDataRow(GV1.FocusedRowHandle)
            Dim dc As Integer = GV1.FocusedRowHandle
            Dim IDDetil As Long = NullToLong(row("NoID"))
            If txtKodeSupplier.Text <> "" Then
                frmEntri.IDSupplier = NullToLong(txtKodeSupplier.EditValue)
                frmEntri.IDGudang = NullToLong(txtGudang.EditValue)
                frmEntri.IsNew = False
                frmEntri.NoID = IDDetil
                frmEntri.IDBeli = NoID
                frmEntri.FormPemanggil = Me
                frmEntri.Show()
                frmEntri.Focus()
                'If frmEntri.ShowDialog(Me)= Windows.Forms.DialogResult.OK Then
                '    RefreshDetil()
                'End If
                'txtBarang.Focus()
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

    Private Sub txtGudang_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtGudang.EditValueChanged
        RubahGudang()
    End Sub
    Private Sub RubahGudang()
        Dim Ds As New DataSet
        Try
            SQL = "SELECT MWilayah.NoID AS IDWilayah, MWilayah.Nama AS Wilayah FROM MGudang LEFT JOIN MWilayah ON MWilayah.NoID=MGudang.IDWilayah WHERE MGudang.NOID=" & NullToLong(txtGudang.EditValue)
            Ds = ExecuteDataset("Tabel", SQL)
            If Ds.Tables("Tabel").Rows.Count >= 1 Then
                txtNamaWilayah.Text = NullToStr(Ds.Tables(0).Rows(0).Item("Wilayah"))
                IDWilayah = NullToLong(Ds.Tables(0).Rows(0).Item("IDWilayah"))
            End If
        Catch ex As Exception
            XtraMessageBox.Show("Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            Ds.Dispose()
        End Try
    End Sub

    Private Sub gvGudang_DataSourceChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvGudang.DataSourceChanged
        If System.IO.File.Exists(FolderLayouts & Me.Name & gvGudang.Name & ".xml") Then
            gvGudang.RestoreLayoutFromXml(FolderLayouts & Me.Name & gvGudang.Name & ".xml")
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

    Private Sub txtDiscPersen_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtDiscPersen.EditValueChanged
        If Not IsLoad Then
            txtDiscRp.EditValue = txtSubtotal.EditValue * txtDiscPersen.EditValue / 100
        End If
        HitungTotal()
    End Sub

    Private Sub txtDiscRp_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtDiscRp.EditValueChanged
        If Not IsLoad Then
            txtDiscPersen.EditValue = txtDiscRp.EditValue / IIf(txtSubtotal.EditValue = 0, 1, txtSubtotal.EditValue) * 100
        End If
        HitungTotal()
    End Sub

    Private Sub TextEdit1_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtPPN.EditValueChanged

    End Sub

    Private Sub gvPO_DataSourceChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvPO.DataSourceChanged
        If System.IO.File.Exists(FolderLayouts & Me.Name & gvPO.Name & ".xml") Then
            gvPO.RestoreLayoutFromXml(FolderLayouts & Me.Name & gvPO.Name & ".xml")
        End If
        With gvPO
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

    Private Sub txtPO_ButtonClick(ByVal sender As Object, ByVal e As DevExpress.XtraEditors.Controls.ButtonPressedEventArgs) Handles txtPO.ButtonClick
        If e.Button.Index = 1 Then
            AmbilDataPO()
        End If
    End Sub

    Private Sub txtPO_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtPO.KeyDown
        If e.KeyCode = Keys.Enter Then
            AmbilDataPO()
        End If
    End Sub

    Private Sub AmbilDataPO()
        Dim Ds As New DataSet
        Try
            If pTipe = pStatus.Edit AndAlso IsPosted = False Then
                IDPOOld = NullToLong(txtPO.EditValue)
                SQL = "SELECT MPOD.*,MPO.IDTypePajak " & _
                      " FROM MPOD INNER JOIN MPO ON MPO.NoID=MPOD.IDPO " & _
                      " WHERE MPOD.IDPO=" & NullToLong(txtPO.EditValue) & _
                      " AND (MPOD.Qty*MPOD.Konversi)-IsNull((SELECT SUM(MBeliD.Qty*MBeliD.Konversi) FROM MBeliD INNER JOIN MBeli ON MBeli.NoID=MBeliD.IDBeli WHERE MBeliD.IDPOD=MPOD.NoID),0)>0 " & vbCrLf & _
                      " ORDER BY MPOD.NoUrut"
                Ds = ExecuteDataset("MPOD", SQL)
                With Ds.Tables("MPOD")
                    If .Rows.Count >= 1 Then
                        For i As Integer = 0 To .Rows.Count - 1
                            'txtQty.EditValue / IsiKarton * txtKonversi.EditValue
                            'Total sudah include PPN (jika Ada)
                            SQL = "INSERT INTO MBeliD (NoID,IDBeli,IDBarangD,IDPOD,IDLPBD,NoUrut,Tgl,Jam,ExpiredDate,IDBarang," & _
                                 " IDSatuan,Qty,QtyPcs,Harga,Biaya,HargaPcs,CTN,DiscPersen1,DiscPersen2,DiscPersen3,Disc1,Disc2,Disc3,Jumlah,Catatan,IDGudang,Konversi,PPN,HargaNetto,ProsenMargin,HitungJual,HargaJual,IDTypePajak)" & vbCrLf & _
                                 " SELECT " & GetNewID("MBeliD", "NoID") & ", " & NoID & ", MPOD.IDBarangD, MPOD.NoID, NULL, " & GetNewID("MBeliD", "NoUrut", " WHERE MBeliD.IDBeli=" & NoID) & ", MPOD.Tgl, MPOD.Jam, NULL, MPOD.IDBarang, " & _
                                 " MPOD.IDSatuan, MPOD.Qty, MPOD.QtyPcs, MPOD.Harga, 0, MPOD.HargaPcs, MPOD.Ctn, MPOD.DiscPersen1, MPOD.DiscPersen2, MPOD.DiscPersen3, MPOD.Disc1, MPOD.Disc2, MPOD.Disc3, MPOD.Jumlah, MPOD.Catatan, " & NullToLong(txtGudang.EditValue) & ", MPOD.Konversi,0, MPOD.HargaNetto, 0, 0, 0, MPO.IDTypePajak " & vbCrLf & _
                                 " FROM MPOD INNER JOIN MPO ON MPO.NoID=MPOD.IDPO WHERE MPOD.NoID=" & NullToLong(.Rows(i).Item("NoID"))
                            EksekusiSQL(SQL)
                        Next
                    End If
                End With
                RefreshDetil()
                If NullToBool(EksekusiSQlSkalarNew("select IsPKP from MAlamat where NoID=" & NullToLong(txtAlamatSupplier.EditValue))) Then
                    'PKP
                Else
                End If
            Else
                txtPO.EditValue = IDPOOld
            End If
        Catch ex As Exception
            XtraMessageBox.Show("Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            'RefreshLookUpSTB()
            Ds.Dispose()
        End Try
    End Sub

    Private Sub txtPO_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtPO.EditValueChanged

    End Sub

    Private Sub txtKode_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtKode.EditValueChanged

    End Sub

    Private Sub cmdImport_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdImport.Click
        Dim x As New frmImportDataPembelianBarang
        x.IDBeli = NoID
        x.IDGudang = NullTolInt(txtGudang.EditValue)
        If x.ShowDialog(Me) Then
            RefreshDetil()
        End If
        x.Dispose()
    End Sub

    Private Sub txtTotal2_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtTotal2.EditValueChanged
        Try
            'IsLoad = False
            txtDiscRp.EditValue = 0
            txtDiscPersen.EditValue = 0
            HitungTotal(False)
            Application.DoEvents()
            txtDiscRp.EditValue = (txtSubtotal.EditValue + txtBiaya.EditValue) - txtTotal2.EditValue
            txtDiscPersen.EditValue = txtDiscRp.EditValue / IIf(txtSubtotal.EditValue = 0, 1, txtSubtotal.EditValue) * 100
            Application.DoEvents()
            HitungTotal(False)
            'IsLoad = True
        Catch ex As Exception

        End Try
    End Sub

    Private Sub txtTotal2_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtTotal2.LostFocus
        'txtDiscRp.EditValue = txtTotal2.EditValue - txtSubtotal.EditValue + txtDiscTotal.EditValue - txtBiaya.EditValue
        'HitungTotal()
    End Sub
End Class