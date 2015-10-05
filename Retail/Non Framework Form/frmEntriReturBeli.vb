Imports System.Data
Imports System.Data.SqlClient
Imports DevExpress.XtraEditors
Imports DevExpress.Utils
Imports DevExpress.XtraGrid.Views.Base

Public Class frmEntriReturBeli
    Dim SQL As String = ""
    Public Enum pStatus
        Baru = 0
        Edit = 1
    End Enum
    Public pTipe As pStatus
    Public IsPosted As Boolean = False
    Public NoID As Long = -1
    Public IDWilayah As Long = DefIDWilayah

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
    Dim IsTempInsertBaru As Boolean = False
    Dim IDBeli As Long = -1
    Public IsRevisi As Boolean = False
    Public IsEntriFakturPajak As Boolean = False
    Public IsDariDaftarPPN As Boolean = False

    Private Sub RefreshNotaJual()
        Dim repChekEdit As New DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit
        Dim ds As New DataSet
        Try
            SQL = "select BL.*,ISNULL(RB.NilaiRetur,0) as Retur,ISNULL(BYR.NilaiBayar,0) as Bayar,BL.NilaiNota-ISNULL(RB.NilaiRetur,0)-ISNULL(BYR.NilaiBayar,0) as Sisa  from "
            SQL = SQL & "(select mbeli.Kode,NoID,mbeli.IDSupplier,mbeli.tanggal,mbeli.Total as NilaiNota from mbeli where isposted=1) BL "
            SQL = SQL & "left join (Select IDBeli,Sum(MReturBeli.Total) as NilaiRetur from MReturBeli "
            SQL = SQL & "group by IDBeli) RB on BL.NoID=RB.IDBeli "
            SQL = SQL & "left join (Select IDBeli,Sum(MbayarHutangD.Bayar) as NilaiBayar from MBayarHutangD inner join MBayarHutang "
            SQL = SQL & "on MBayarHutangD.IDbayarHutang=MbayarHutang.NoID where MBayarHutang.IsJual=0 "
            SQL = SQL & "group by IDBeli ) BYR on BL.NoID=BYR.IDBeli "
            SQL = SQL & "where (BL.NoID=" & IDBeli & " or BL.NilaiNota-ISNULL(RB.NilaiRetur,0)-ISNULL(BYR.NilaiBayar,0)<>0) "
            SQL = SQL & "and BL.IDSupplier= " & NullToLong(txtKodeSupplier.EditValue)
            ds = ExecuteDataset("MJual", SQL)
            txtNotaJual.Properties.DataSource = ds.Tables("MJual")

            For x As Integer = 0 To gvNotaJual.Columns.Count - 1
                Select Case gvNotaJual.Columns(x).ColumnType.Name.ToLower
                    Case "int32", "int64", "int"
                        gvNotaJual.Columns(x).DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
                        gvNotaJual.Columns(x).DisplayFormat.FormatString = "n0"
                    Case "decimal", "single", "money", "double"
                        gvNotaJual.Columns(x).DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
                        gvNotaJual.Columns(x).DisplayFormat.FormatString = "n2"
                    Case "string"
                        gvNotaJual.Columns(x).DisplayFormat.FormatType = DevExpress.Utils.FormatType.None
                        gvNotaJual.Columns(x).DisplayFormat.FormatString = ""
                    Case "date", "datetime"
                        If gvNotaJual.Columns(x).FieldName.Trim.ToLower = "jam" Then
                            gvNotaJual.Columns(x).DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime
                            gvNotaJual.Columns(x).DisplayFormat.FormatString = "HH:mm"
                        Else
                            gvNotaJual.Columns(x).DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime
                            gvNotaJual.Columns(x).DisplayFormat.FormatString = "dd-MM-yyyy"
                        End If
                    Case "boolean"
                        gvNotaJual.Columns(x).ColumnEdit = repChekEdit
                End Select
            Next
            'If System.IO.File.Exists(FolderLayouts & Me.Name & gvNotaJual.Name & ".xml") Then
            '    gvNotaJual.RestoreLayoutFromXml(FolderLayouts & Me.Name & gvNotaJual.Name & ".xml")
            'End If

        Catch ex As Exception
            XtraMessageBox.Show("Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            ds.Dispose()
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

            SQL = "SELECT MAlamatDNPWP.NoID, MAlamat.Kode, MAlamatDNPWP.NamaWP, MAlamatDNPWP.NPWP FROM MAlamat INNER JOIN MAlamatDNPWP ON MAlamat.NoID=MAlamatDNPWP.IDAlamat WHERE MAlamat.NoID=" & NullToLong(txtKodeSupplier.EditValue) & vbCrLf & _
                  " AND MAlamatDNPWP.IsActive=1 "
            ds = ExecuteDataset("master", SQL)
            txtWP.Properties.DataSource = ds.Tables("master")
            txtWP.Properties.DisplayMember = "NPWP"
            txtWP.Properties.ValueMember = "NoID"
            If System.IO.File.Exists(FolderLayouts & Me.Name & gvWP.Name & ".xml") Then
                gvWP.RestoreLayoutFromXml(FolderLayouts & Me.Name & gvWP.Name & ".xml")
            End If

            SQL = "SELECT MGudang.NoID, MGudang.Kode, MGudang.Kode + ' - ' + MGudang.Nama AS Nama, MWilayah.Nama AS Wilayah FROM MGudang LEFT JOIN MWilayah ON MGudang.IDWilayah=MWilayah.NoID WHERE MGudang.IsActive=1 AND MGudang.InTransit=0 AND MGudang.IDWilayah=" & DefIDWilayah
            ds = ExecuteDataset("master", SQL)
            txtGudang.Properties.DataSource = ds.Tables("master")
            txtGudang.Properties.ValueMember = "NoID"
            txtGudang.Properties.DisplayMember = "Nama"
            If System.IO.File.Exists(FolderLayouts & Me.Name & gvGudang.Name & ".xml") Then
                gvGudang.RestoreLayoutFromXml(FolderLayouts & Me.Name & gvGudang.Name & ".xml")
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

    Private Sub frmEntriReturBeli_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        If IsTempInsertBaru Then
            If XtraMessageBox.Show("Yakin ingin keluar?" & vbCrLf & "Ya, akan menghapus transaksi dan detil item.", NamaAplikasi, MessageBoxButtons.YesNo, MessageBoxIcon.Information, MessageBoxDefaultButton.Button2) = Windows.Forms.DialogResult.Yes Then
                If Not NullToBool(EksekusiSQlSkalarNew("SELECT isnull(IsPosted,0) FROM  MBeli  WHERE NoID=" & NoID)) Then
                    EksekusiSQL("DELETE FROM MReturBeli WHERE NoID=" & NoID)
                    EksekusiSQL("DELETE FROM MReturBeliD WHERE IDReturBeli=" & NoID)
                Else
                    MsgBox("Data ini telah terposting, anda harus menyimpannya!", MsgBoxStyle.Information)
                End If
            Else
                e.Cancel = True
            End If
        End If
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
            If System.IO.File.Exists(FolderLayouts & Me.Name & gvGudang.Name & ".xml") Then
                gvGudang.RestoreLayoutFromXml(FolderLayouts & Me.Name & gvGudang.Name & ".xml")
            End If
            If System.IO.File.Exists(FolderLayouts & Me.Name & gvNotaJual.Name & ".xml") Then
                gvNotaJual.RestoreLayoutFromXml(FolderLayouts & Me.Name & gvNotaJual.Name & ".xml")
            End If
            FungsiControl.SetForm(Me)
            HighLightTxt()

            'Paten
            TypePajak.EditValue = 1
            TypePajak.Properties.ReadOnly = False
            txtMasaPajak.Properties.Mask.EditMask = "MMMM-yyyy"
            If IsEntriFakturPajak Then
                ckAktif.Checked = True
            End If
        Catch ex As Exception
            XtraMessageBox.Show(ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            Windows.Forms.Cursor.Current = Cursors.Default
            dlg.Close()
            dlg.Dispose()
        End Try
    End Sub
    Private Sub HighLightTxt()
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
                SQL = "SELECT MReturBeli.*, MWilayah.Nama AS Wilayah, MAlamat.Alamat, MAlamat.Nama AS NamaAlamat, MAlamat.Kode as KodeAlamat FROM MReturBeli LEFT JOIN (MGudang LEFT JOIN MWilayah ON MWilayah.NoID=MGudang.IDWilayah) ON MGudang.NoID=MReturBeli.IDGudang LEFT JOIN MAlamat ON MAlamat.NoID=MReturBeli.IDSupplier WHERE MReturBeli.NoID=" & NoID
                DS = ExecuteDataset("Tabel", SQL)
                If DS.Tables("Tabel").Rows.Count >= 1 Then
                    txtKodeSupplier.EditValue = NullToLong(DS.Tables(0).Rows(0).Item("IDSupplier"))
                    'txtKodeSupplier.Text = NullTostr(DS.Tables(0).Rows(0).Item("KodeAlamat"))
                    txtGudang.EditValue = NullToLong(DS.Tables(0).Rows(0).Item("IDGudang"))
                    TypePajak.EditValue = NullToLong(DS.Tables(0).Rows(0).Item("IDTypePajak"))
                    RubahGudang()
                    txtNamaSupplier.Text = NullToStr(DS.Tables(0).Rows(0).Item("NamaAlamat"))
                    txtAlamatSupplier.Text = NullToStr(DS.Tables(0).Rows(0).Item("Alamat"))
                    txtKode.Text = NullToStr(DS.Tables(0).Rows(0).Item("Kode"))
                    txtNoFakturPajak.Text = NullToStr(DS.Tables(0).Rows(0).Item("NoFakturPajak"))
                    KodeLama = txtKode.Text
                    FakturPajakLama = txtNoFakturPajak.Text
                    txtKodeReff.Text = NullToStr(DS.Tables(0).Rows(0).Item("KodeReff"))
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
                    txtPPN.EditValue = NullToDbl(DS.Tables(0).Rows(0).Item("NilaiPPN"))
                    txtPPNProsen.EditValue = NullToDbl(DS.Tables(0).Rows(0).Item("PPN"))
                    txtDPP.EditValue = NullToDbl(DS.Tables(0).Rows(0).Item("DPP"))
                    txtTotal.EditValue = NullToDbl(DS.Tables(0).Rows(0).Item("Total"))
                    txtBayar.EditValue = NullToDbl(DS.Tables(0).Rows(0).Item("Bayar"))
                    txtSisa.EditValue = NullToDbl(DS.Tables(0).Rows(0).Item("Sisa"))
                    IsPosted = NullToBool(DS.Tables(0).Rows(0).Item("IsPosted"))
                    txtKeterangan.Text = NullToStr(DS.Tables(0).Rows(0).Item("Keterangan"))
                    txtNotaJual.EditValue = NullToLong(DS.Tables(0).Rows(0).Item("IDBeli"))
                    IDBeli = NullToLong(DS.Tables(0).Rows(0).Item("IDBeli"))

                    If NullToStr(DS.Tables(0).Rows(0).Item("TglFakturPajak")) = "" Then
                        txtTglFP.DateTime = NullToDate(DS.Tables(0).Rows(0).Item("Tanggal"))
                    Else
                        txtTglFP.DateTime = NullToDate(DS.Tables(0).Rows(0).Item("TglFakturPajak"))
                    End If
                    txtFPMasukkan.Text = NullToStr(DS.Tables(0).Rows(0).Item("NoFPMasukkan"))
                    tglFPMasukkan.DateTime = NullToDate(DS.Tables(0).Rows(0).Item("TglFPMasukkan"))
                    txtMasaPajak.DateTime = NullToDate(DS.Tables(0).Rows(0).Item("MasaPajak"))
                    ckAktif.Checked = NullToBool(DS.Tables(0).Rows(0).Item("IsProsesPajak"))
                    txtWP.EditValue = NullToLong(DS.Tables(0).Rows(0).Item("IDAlamatDNPWP"))
                    'txtGudang.EditValue = NullTolong(DS.Tables(0).Rows(0).Item("IDGudang"))
                    'txtWilayah.Text = NullTostr(DS.Tables(0).Rows(0).Item("Wilayah"))
                    HitungTotal()
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
        Dim strHargaBeli As String = Ini.BacaIni(Me.Name, "TampilkanHargaBeli", " MBarang.HargaBeliPcs ")
        Dim strHargaJual As String = Ini.BacaIni(Me.Name, "TampilkanHargaJual", " MBarangD.HargaNetto ")
        Try
            strsql = "select MBarangD.Barcode,MBarang.CtnPcs AS IsiCtn, MReturBeliD.Disc1 AS DiscRp, MWilayah.Nama AS Wilayah, MBarang.Kode as KodeStock, CASE WHEN IsNull(MReturBeliD.NamaStock,'')='' THEN MBarang.Nama ELSE MReturBeliD.NamaStock END as NamaStock,MSatuan.Kode as Satuan,Mgudang.Kode as KodeGudang, MBeli.Kode AS NoFaktur, MBeli.Tanggal AS TglFaktur, MReturBeliD.*, " & vbCrLf
            strsql &= strHargaBeli & " AS HargaBeli, " & strHargaJual & " AS HargaJual, " & strHargaBeli & "*MReturBeliD.Qty AS NilaiBeli, " & strHargaJual & "*MReturBeliD.Qty AS NilaiJual " & vbCrLf
            strsql &= " From (MReturBeliD Inner Join MReturBeli On MReturBeliD.IDReturBeli=MReturBeli.NoID) " & vbCrLf
            strsql &= " LEFT JOIN MBarang ON MReturBeliD.IDBarang=MBarang.NoID " & vbCrLf
            strsql &= " LEFT JOIN MBarangD ON MReturBeliD.IDBarangD=MBarangD.NoID " & vbCrLf
            strsql &= " LEFT JOIN MSatuan ON MReturBeliD.IDSatuan=MSatuan.NoID " & vbCrLf
            strsql &= " LEFT JOIN (MBeliD LEFT JOIN MBeli ON MBeli.NoID=MBeliD.IDBeli) ON MReturBeliD.IDBeliD=MBeliD.NoID " & vbCrLf
            strsql &= " LEFT JOIN (MGudang LEFT JOIN MWilayah ON MWilayah.NoID=MGudang.IDWilayah) ON MReturBeliD.IDGudang=MGudang.NoID " & vbCrLf
            strsql &= " where MReturBeliD.IDReturBeli = " & NoID
            ExecuteDBGrid(GC1, strsql, "NoID")
            Ini.TulisIni(Me.Name, "TampilkanHargaBeli", strHargaBeli.ToString.ToUpper)
            Ini.TulisIni(Me.Name, "TampilkanHargaJual", strHargaJual.ToString.ToUpper)
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
            txtDiscTotal.EditValue = (NullToDbl(txtDiscPersen.EditValue) * NullToDbl(txtSubtotal.EditValue) / 100) '+ txtDiscRp.EditValue
            txtSubtotal2.EditValue = NullToDbl(txtSubtotal.EditValue) - NullToDbl(txtDiscTotal.EditValue)

            Select Case NullToLong(TypePajak.EditValue)
                Case 1 'Sudah Termasuk pajak
                    txtPPNProsen.EditValue = NullToDbl(EksekusiSQlSkalarNew("SELECT TarifProsen FROM MTypePajak WHERE NoID=" & NullToLong(TypePajak.EditValue)))
                    txtDPP.EditValue = Bulatkan(NullToDbl(txtSubtotal.EditValue) / (1 + (txtPPNProsen.EditValue / 100)), 0)
                    txtPPN.EditValue = NullToDbl(txtSubtotal.EditValue) - NullToDbl(txtDPP.EditValue)
                    txtTotal.EditValue = txtSubtotal2.EditValue + txtBiaya.EditValue
                Case 2 'Belum Termasuk Pajak
                    txtDPP.EditValue = NullToDbl(txtSubtotal.EditValue)
                    txtPPNProsen.EditValue = NullToDbl(EksekusiSQlSkalarNew("SELECT TarifProsen FROM MTypePajak WHERE NoID=" & NullToLong(TypePajak.EditValue)))
                    txtPPN.EditValue = Bulatkan(NullToDbl(txtSubtotal.EditValue) * NullToDbl(txtPPNProsen.EditValue) / 100, 0)
                    txtTotal.EditValue = txtSubtotal2.EditValue + txtPPN.EditValue + txtBiaya.EditValue
                Case Else 'Non BKP
                    txtDPP.EditValue = 0
                    txtPPN.EditValue = 0
                    txtPPNProsen.EditValue = 0
                    txtTotal.EditValue = txtSubtotal2.EditValue - txtPPN.EditValue + txtBiaya.EditValue
            End Select

            'txtTotal.EditValue = txtSubtotal.EditValue - txtDiscTotal.EditValue + txtBiaya.EditValue
            txtSisa.EditValue = txtTotal.EditValue - txtBayar.EditValue
            txtTotalCTN.EditValue = NullToDbl(EksekusiSQlSkalarNew("SELECT SUM(Ctn) FROM MReturBeliD WHERE IDReturBeli=" & NoID))
        Catch ex As Exception
            XtraMessageBox.Show("Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub
    Private Sub IsiDefault()
        Tgl.DateTime = TanggalSystem
        tglJatuhTempo.DateTime = DateAdd(DateInterval.Month, 1, Tgl.DateTime)
        tglSJ.DateTime = TanggalSystem
        tglStok.DateTime = TanggalSystem
        tglFPMasukkan.DateTime = TanggalSystem
        SetTombol()
        txtKodeSupplier.EditValue = DefIDSupplier
        RubahSupplier()
        txtGudang.EditValue = DefIDGudang
        RubahGudang()
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

    Private Sub cmdSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSave.Click
        Dim PesanSalah As String = ""
        If (Not IsPosted) Or (IsPosted AndAlso IsRevisi) Or (IsPosted AndAlso IsEntriFakturPajak) Then
            If pTipe = pStatus.Baru Then
                txtKode.Text = clsKode.MintaKodeReturBeliBaru("MReturBeli", Tgl.DateTime, NullToLong(txtKodeSupplier.EditValue), IDWilayah, 5, "ISNULL(MReturBeli.IsTanpaBarang,0)=0")
            End If
            If IsValidasi() Then
                If Simpan(PesanSalah) = True Then
                    If pTipe = pStatus.Baru Then
                        KodeLama = txtKode.Text
                        FakturPajakLama = txtNoFakturPajak.Text
                        pTipe = pStatus.Edit
                        SetTombol()
                        txtBarang.Focus()
                        IsTempInsertBaru = True
                    Else
                        clsPostingPembelian.PostingStokBarangReturPembelian(NoID, IsRevisi)
                        If IsDariDaftarPPN Then
                            'KODE DIBAWAH UNTUK MEMBUKA FORM HANYA 1
                            Dim frmEntri As frmDaftarPPNMasukkan = Nothing
                            Dim F As Object
                            For Each F In MdiParent.MdiChildren
                                If TypeOf F Is frmDaftarPPNMasukkan Then
                                    frmEntri = F
                                    If frmEntri.FormName = FormNameDaftar Then
                                        Exit For
                                    Else
                                        frmEntri = Nothing
                                    End If
                                End If
                            Next
                            If frmEntri Is Nothing Then
                                frmEntri = New frmDaftarPPNMasukkan
                                frmEntri.WindowState = FormWindowState.Maximized
                                frmEntri.MdiParent = Me
                                frmEntri.FormName = FormNameDaftar
                                frmEntri.TableName = TableNameDaftar
                                frmEntri.Text = TextDaftar
                                frmEntri.FormEntriName = FormEntriDaftar
                                frmEntri.TableMaster = TableMasterDaftar
                            End If
                            frmEntri.Show()
                            frmEntri.RefreshData()
                            frmEntri.GV1.ClearSelection()
                            frmEntri.GV1.FocusedRowHandle = frmEntri.GV1.LocateByDisplayText(0, frmEntri.GV1.Columns("NoID"), NoID.ToString("#,##0"))
                            frmEntri.GV1.SelectRow(frmEntri.GV1.FocusedRowHandle)
                            frmEntri.Focus()
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
                        End If

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
        If txtDiscRp.EditValue < 0 Then
            XtraMessageBox.Show("Diskon rupiah masih dibawah 0.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
            txtDiscRp.Focus()
            Return False
            Exit Function
        End If
        If txtDiscPersen.EditValue < 0 Then
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
        End If
        If txtBiaya.EditValue < 0 Then
            XtraMessageBox.Show("Biaya masih dibawah 0.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
            txtBiaya.Focus()
            Return False
            Exit Function
        End If

        If CekKodeValid(txtKode.Text, KodeLama, "MReturBeli", "Kode", IIf(pTipe = pStatus.Edit, True, False)) Then
            XtraMessageBox.Show("Kode sudah dipakai.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
            txtKode.Focus()
            Return False
            Exit Function
        End If
        If Not CekSupplierJual(NullToLong(txtKodeSupplier.EditValue)) Then
            If XtraMessageBox.Show("Belum ada transaksi pembelian ke Supplier " & txtKodeSupplier.Text & "." & vbCrLf & "Ingin melanjutkan penyimpanan?", NamaAplikasi, MessageBoxButtons.YesNo, MessageBoxIcon.Information) = Windows.Forms.DialogResult.No Then
                txtKodeSupplier.Focus()
                Return False
                Exit Function
            Else
                Dim x As New frmOtorisasiAdmin
                Try
                    If x.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
                    Else
                        txtKodeSupplier.Focus()
                        Return False
                        Exit Function
                    End If
                Catch ex As Exception
                    XtraMessageBox.Show("En", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
                End Try
            End If
        End If
        If pTipe = pStatus.Edit AndAlso GV1.RowCount <= 0 Then
            XtraMessageBox.Show("Item detil masih kosong." & vbCrLf & "Isi item detil atau tutup bila ingin membatailkan.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
            Return False
            Exit Function
        End If

        'Untuk Pajak
        If IsEntriFakturPajak Then
            If txtNoFakturPajak.Text = "" Then
                If XtraMessageBox.Show("Faktur Pajak Masih Kosong." & vbCrLf & "Ingin meneruskan penyimpanan?", NamaAplikasi, MessageBoxButtons.YesNo, MessageBoxIcon.Information) = Windows.Forms.DialogResult.No Then
                    txtNoFakturPajak.Focus()
                    Return False
                    Exit Function
                End If
            End If
            If txtWP.Text = "" Then
                If XtraMessageBox.Show("No NPWP Masih kosong." & vbCrLf & "Ingin meneruskan penyimpanan?", NamaAplikasi, MessageBoxButtons.YesNo, MessageBoxIcon.Information) = Windows.Forms.DialogResult.No Then
                    txtWP.Focus()
                    Return False
                    Exit Function
                End If
            End If
            If ckAktif.Checked AndAlso txtMasaPajak.Text = "" Then
                XtraMessageBox.Show("Masa Pajak Masih Kosong.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
                txtMasaPajak.Focus()
                Return False
                Exit Function
            End If
            If clsPostingPembelian.IsLockPeriodeFP(txtMasaPajak.DateTime) Then
                XtraMessageBox.Show("Masa Pajak " & txtMasaPajak.DateTime.ToString("MMMM-yyyy") & " sudah dikunci.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
                txtMasaPajak.Focus()
                Return False
                Exit Function
            End If
            If IsRevisi AndAlso txtWP.Text <> "" AndAlso Not ckAktif.Checked Then
                If XtraMessageBox.Show("No WP Sudah terisi tapi checklist Aktif masih belum checklist." & vbCrLf & "Ingin meneruskan penyimpanan?", NamaAplikasi, MessageBoxButtons.YesNo, MessageBoxIcon.Information) = Windows.Forms.DialogResult.No Then
                    ckAktif.Focus()
                    Return False
                    Exit Function
                End If
            End If
            If ckAktif.Checked AndAlso DateDiff(DateInterval.Month, CDate(txtTglFP.DateTime.ToString("yyyy-MM-01")), CDate(txtMasaPajak.DateTime.ToString("yyyy-MM-01"))) > 3 Then
                XtraMessageBox.Show("Masa Pajak Sudah Melebihi 3 Bulan.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Warning)
                txtMasaPajak.Focus()
                Return False
                Exit Function
            End If
            If ckAktif.Checked AndAlso DateDiff(DateInterval.Month, CDate(txtTglFP.DateTime.ToString("yyyy-MM-01")), CDate(txtMasaPajak.DateTime.ToString("yyyy-MM-01"))) < 0 Then
                XtraMessageBox.Show("Masa Pajak belum saatnya.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Warning)
                txtMasaPajak.Focus()
                Return False
                Exit Function
            End If
            If CekKodeValid(txtNoFakturPajak.Text, FakturPajakLama, "MReturBeli", "NoFakturPajak", IIf(pTipe = pStatus.Edit, True, False)) Then
                If XtraMessageBox.Show("Faktur Pajak sudah dipakai." & vbCrLf & "Ingin meneruskan penyimpanan?", NamaAplikasi, MessageBoxButtons.YesNo, MessageBoxIcon.Information) = Windows.Forms.DialogResult.No Then
                    txtNoFakturPajak.Focus()
                    Return False
                    Exit Function
                End If
            End If
        End If

        Return True
    End Function
    Dim KodeLama As String = ""
    Dim FakturPajakLama As String = ""
    Public Sub SimpanTambahan()
        Try
            HitungTotal()
            SQL = "UPDATE MReturBeli SET "
            SQL &= "SubTotal=" & FixKoma(txtSubtotal.EditValue) & ","
            SQL &= "DiskonNotaProsen=" & FixKoma(txtDiscPersen.EditValue) & ","
            SQL &= "DiskonNotaRp=" & FixKoma(txtDiscRp.EditValue) & ","
            SQL &= "DiskonNotaTotal=" & FixKoma(txtDiscTotal.EditValue) & ","
            SQL &= "Biaya=" & FixKoma(txtBiaya.EditValue) & ","
            SQL &= "Total=" & FixKoma(txtTotal.EditValue) & ","
            SQL &= "Bayar=" & FixKoma(txtBayar.EditValue) & ","
            SQL &= "Sisa=" & FixKoma(txtSisa.EditValue)
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
                NoID = GetNewID("MReturBeli")
                SQL = "INSERT INTO MReturBeli (NoID,IDGudang,IDWilayah,IDAdmin,Kode,KodeReff,Tanggal,TanggalStock,JatuhTempo,"
                SQL &= " IDSupplier,TanggalSJ,NoSJ,SubTotal,DiskonNotaProsen,DiskonNotaRp,DiskonNotaTotal,"
                SQL &= " Biaya, Total, Bayar, Sisa, Keterangan, IDBeli" & IIf(IsEntriFakturPajak AndAlso ckAktif.Checked, ",NoFakturPajak,NilaiPPN,PPN,DPP,IDTypePajak,NoFPMasukkan,TglFPMasukkan,IsProsesPajak,IDAlamatDNPWP", "") & ") VALUES (" & vbCrLf
                SQL &= NoID & ","
                SQL &= NullToLong(txtGudang.EditValue) & ","
                SQL &= IDWilayah & ","
                SQL &= IDAdmin & ","
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
                SQL &= "'" & FixApostropi(txtKeterangan.Text) & "',"
                SQL &= NullToLong(txtNotaJual.EditValue) & " " & _
                IIf(IsRevisi, ",'" & FixApostropi(txtNoFakturPajak.Text) & "'," & _
                FixKoma(txtPPN.EditValue) & "," & vbCrLf & _
                FixKoma(txtPPNProsen.EditValue) & "," & vbCrLf & _
                FixKoma(txtDPP.EditValue) & "," & vbCrLf & _
                NullToLong(TypePajak.EditValue) & "," & _
                "'" & FixApostropi(txtFPMasukkan.Text) & "', " & _
                "'" & tglFPMasukkan.DateTime.ToString("yyyy-MM-dd HH:mm") & "'," & IIf(ckAktif.Checked, 1, 0) & ", " & NullToLong(txtWP.EditValue), "") & _
                ")"
                'SQL &= NullTolong(txtGudang.EditValue) & ",)"
                EksekusiSQL(SQL)
            Else
                SQL = "UPDATE MReturBeli SET "
                If Not Tgl.Properties.ReadOnly Or Not txtKode.Properties.ReadOnly Then
                    SQL &= "IDAdmin=" & IDAdmin & ","
                End If
                SQL &= "IDGudang=" & NullToLong(txtGudang.EditValue) & ","
                SQL &= "IDWilayah=" & IDWilayah & ","
                SQL &= "Kode='" & FixApostropi(txtKode.Text) & "',"
                SQL &= "KodeReff='" & FixApostropi(txtKodeReff.Text) & "',"
                SQL &= "Tanggal='" & Tgl.DateTime.ToString("yyyy-MM-dd HH:mm") & "',"
                SQL &= "TanggalStock='" & tglStok.DateTime.ToString("yyyy-MM-dd HH:mm") & "',"
                SQL &= "JatuhTempo='" & tglJatuhTempo.DateTime.ToString("yyyy-MM-dd HH:mm") & "',"
                SQL &= "IDSupplier=" & txtKodeSupplier.EditValue & ","
                'SQL &= "IDGudang=" & txtGudang.EditValue & ","
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
                SQL &= "IDBeli=" & NullToLong(txtNotaJual.EditValue) & ","
                SQL &= " Keterangan='" & FixApostropi(txtKeterangan.Text) & "' "
                If IsEntriFakturPajak AndAlso ckAktif.Checked Then
                    SQL &= ", IDTypePajak=" & NullToLong(TypePajak.EditValue) & "," & _
                           " IDAlamatDNPWP=" & NullToLong(txtWP.EditValue) & "," & vbCrLf & _
                           " NoFakturPajak='" & FixApostropi(txtNoFakturPajak.Text) & "'," & _
                           " NilaiPPN=" & FixKoma(txtPPN.EditValue) & "," & vbCrLf & _
                           " PPN=" & FixKoma(txtPPNProsen.EditValue) & "," & vbCrLf & _
                           " DPP=" & FixKoma(txtDPP.EditValue) & "," & vbCrLf & _
                           " NoFPMasukkan='" & FixApostropi(txtFPMasukkan.Text) & "', " & _
                           " TglFakturPajak='" & txtTglFP.DateTime.ToString("yyyy-MM-dd HH:mm") & "', " & _
                           " TglFPMasukkan='" & tglFPMasukkan.DateTime.ToString("yyyy-MM-dd HH:mm") & "', " & _
                           " MasaPajak='" & txtMasaPajak.DateTime.ToString("yyyy-MM-dd HH:mm") & "'," & vbCrLf & _
                           " IsProsesPajak=" & IIf(ckAktif.Checked, 1, 0)
                ElseIf IsEntriFakturPajak AndAlso Not ckAktif.Checked Then
                    SQL &= ", IsProsesPajak=" & IIf(ckAktif.Checked, 1, 0)
                End If
                SQL &= " WHERE NoID=" & NoID
                EksekusiSQL(SQL)
            End If
            Sukses = True
            If Sukses Then
                SQL = "UPDATE MReturBeliD SET IDGudang=" & NullToLong(txtGudang.EditValue) & " WHERE IDReturBeli=" & NoID
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
                Ini.TulisIniPath(FolderLayouts & Me.Name & ".ini", "Form", "Width", Me.Width)
                Ini.TulisIniPath(FolderLayouts & Me.Name & ".ini", "Form", "Height", Me.Height)
                Me.Left = (Screen.PrimaryScreen.WorkingArea.Width - Me.Width) \ 2
                Me.Top = (Screen.PrimaryScreen.WorkingArea.Height - Me.Height) \ 2

                LayoutControl1.SaveLayoutToXml(FolderLayouts & Me.Name & LayoutControl1.Name & ".xml")
                GV1.SaveLayoutToXml(FolderLayouts & Me.Name & GV1.Name & ".xml")
                SearchLookUpEdit1View.SaveLayoutToXml(FolderLayouts & Me.Name & SearchLookUpEdit1View.Name & ".xml")
                gvGudang.SaveLayoutToXml(FolderLayouts & Me.Name & gvGudang.Name & ".xml")
                gvNotaJual.SaveLayoutToXml(FolderLayouts & Me.Name & gvNotaJual.Name & ".xml")
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
                EksekusiSQL("Delete From MReturBeliD where NoID=" & IDDetil.ToString)
                RefreshDetil()
                SimpanTambahan()
            End If
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
            txtNamaSupplier.Text = NullToStr(EksekusiSQlSkalarNew("SELECT Nama FROM MAlamat WHERE NoID=" & NullToLong(txtKodeSupplier.EditValue)))
            txtAlamatSupplier.Text = NullToStr(EksekusiSQlSkalarNew("SELECT Alamat FROM MAlamat WHERE NoID=" & NullToLong(txtKodeSupplier.EditValue)))
            TglAdd = NullToLong(EksekusiSQlSkalarNew("SELECT JatuhTempoSupplier FROM MAlamat WHERE NoID=" & NullToLong(txtKodeSupplier.EditValue)))
            If TglAdd = 0 Then
                tglJatuhTempo.DateTime = DateAdd(DateInterval.Month, 2, Tgl.DateTime)
            Else
                tglJatuhTempo.DateTime = DateAdd(DateInterval.Day, TglAdd, Tgl.DateTime)
            End If
            If pTipe = pStatus.Baru Then
                txtKode.Text = clsKode.MintaKodeReturBeliBaru("MReturBeli", Tgl.DateTime, NullToLong(txtKodeSupplier.EditValue), IDWilayah, 5, "ISNULL(MReturBeli.IsTanpaBarang,0)=0")
                'txtNoFakturPajak.Text = "PA/" & Tgl.DateTime.ToString("yy") & "/" & clsKode.MintaKodeBaruFP("MReturBeli", "NoFakturPajak", Tgl.DateTime, 7, 8)
            End If
            RefreshDataKontak()
            RefreshNotaJual()
        Catch ex As Exception

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
                txtKode.Text = clsKode.MintaKodeReturBeliBaru("MReturBeli", Tgl.DateTime, NullToLong(txtKodeSupplier.EditValue), IDWilayah, 5, "ISNULL(MReturBeli.IsTanpaBarang,0)=0")
                'txtNoFakturPajak.Text = "PA/" & Tgl.DateTime.ToString("yy") & "/" & clsKode.MintaKodeBaruFP("MReturBeli", "NoFakturPajak", Tgl.DateTime, 7, 8)
            End If

        Catch ex As Exception

        End Try
    End Sub

    Private Sub txtSubtotal_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtSubtotal.EditValueChanged

    End Sub

    Private Sub txtDiscPersen_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtDiscPersen.EditValueChanged
        txtDiscRp.EditValue = txtSubtotal.EditValue * txtDiscPersen.EditValue / 100
        HitungTotal()
    End Sub

    Private Sub txtSubtotal_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtSubtotal.LostFocus
        HitungTotal()
    End Sub

    Private Sub txtDiscPersen_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtDiscPersen.LostFocus
        txtDiscRp.EditValue = txtSubtotal.EditValue * txtDiscPersen.EditValue / 100
        HitungTotal()
    End Sub

    Private Sub txtDiscRp_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtDiscRp.EditValueChanged
        txtDiscPersen.EditValue = txtDiscRp.EditValue / IIf(txtSubtotal.EditValue = 0, 1, txtSubtotal.EditValue) * 100
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

    Private Sub txtTotal_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtTotal.LostFocus
        HitungTotal()
    End Sub

    Private Sub txtBayar_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtBayar.EditValueChanged

    End Sub

    Private Sub txtBayar_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtBayar.LostFocus
        HitungTotal()
    End Sub

    Private Sub txtSisa_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtSisa.EditValueChanged

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
        Dim IsStockPerSupplier As Boolean = False
        Try
            IsStockPerSupplier = NullToBool(EksekusiSQlSkalarNew("SELECT MSetting.IsStockReturPerSupplier FROM MSetting"))
            SQL = "SELECT NoID,Kode,Nama FROM MBarang WHERE IsActive=1 AND (ISNULL(KODE,'')<>'') AND (UPPER(Kode) = '" & NamaBarang.Replace("'", "''").ToUpper & "' OR UPPER(NAMA) = '" & NamaBarang.Replace("'", "''").ToUpper & "') " & _
                  IIf(IsStockPerSupplier, " AND (MBarang.IDSupplier1=" & NullToLong(txtKodeSupplier.EditValue) & " OR MBarang.IDSupplier2=" & NullToLong(txtKodeSupplier.EditValue) & " OR MBarang.IDSupplier3=" & NullToLong(txtKodeSupplier.EditValue) & " OR MBarang.IDSupplier4=" & NullToLong(txtKodeSupplier.EditValue) & " OR MBarang.IDSupplier5=" & NullToLong(txtKodeSupplier.EditValue) & ")", "") & _
                  " ORDER BY Kode"
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
        Dim frmEntri As New frmEntriReturBeliD
        Dim Konversi As Double = 0.0
        Try
            If txtKodeSupplier.Text = "" Then Exit Sub
            If CariBarang(IDBarang, NamaBarang, KodeBarang) Then
                If XtraMessageBox.Show("Ingin Meretur Pembelian dengan Kode Stock " & KodeBarang & " Nama Stock " & NamaBarang, "Fast Entri Says", MessageBoxButtons.YesNo) = Windows.Forms.DialogResult.Yes Then
                    IDDetil = GetNewID("MReturBeliD", "NoID")
                    Konversi = NullToDbl(EksekusiSQlSkalarNew("SELECT Konversi FROM MBarangD WHERE IsBeli=1 AND IDBarang=" & IDBarang & " AND IDSatuan=" & DefIDSatuan))
                    If Konversi <> 0 Then
                        SQL = "INSERT INTO MReturBeliD (NoID,IDBarang,IDSatuan,Konversi,IDGudang,IDReturBeli,NoUrut,Tgl,Jam) VALUES " & vbCrLf
                        SQL &= "(" & IDDetil & "," & IDBarang & "," & DefIDSatuan & "," & FixKoma(Konversi) & "," & NullToLong(txtGudang.EditValue) & "," & NoID & "," & GetNewID("MReturBeliD", "NoUrut", " WHERE IDReturBeli= " & NoID) & ",GetDate(),GetDate())"
                    Else
                        SQL = "INSERT INTO MReturBeliD (NoID,IDBarang,IDGudang,IDReturBeli,NoUrut,Tgl,Jam) VALUES " & vbCrLf
                        SQL &= "(" & IDDetil & "," & IDBarang & "," & NullToLong(txtGudang.EditValue) & "," & NoID & "," & GetNewID("MReturBeliD", "NoUrut", " WHERE IDReturBeli= " & NoID) & ",GetDate(),GetDate())"
                    End If
                    EksekusiSQL(SQL)
                    frmEntri.IsNew = False
                    frmEntri.NoID = IDDetil
                    frmEntri.IDReturBeli = NoID
                    frmEntri.IDSupplier = NullToLong(txtKodeSupplier.EditValue)
                    frmEntri.IDGudang = NullToLong(txtGudang.EditValue)
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
                    '    SQL = "DELETE FROM MReturBeliD WHERE NoID=" & IDDetil
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
        Dim frmEntri As New frmEntriReturBeliD
        Try
            Dim IDDetil As Long = -1
            If txtKodeSupplier.Text <> "" Then
                frmEntri.IDSupplier = NullToLong(txtKodeSupplier.EditValue)
                frmEntri.IDGudang = NullToLong(txtGudang.EditValue)
                frmEntri.NoID = IDDetil
                frmEntri.IsNew = True
                frmEntri.IDReturBeli = NoID
                frmEntri.FormPemanggil = Me
                frmEntri.Show()
                frmEntri.Focus()
                'If frmEntri.ShowDialog(Me)= Windows.Forms.DialogResult.OK Then
                '    RefreshDetil()
                'End If
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
        Dim frmEntri As New frmEntriReturBeliD
        Try
            Dim row As System.Data.DataRow = view.GetDataRow(GV1.FocusedRowHandle)
            Dim dc As Integer = GV1.FocusedRowHandle
            Dim IDDetil As Long = NullToLong(row("NoID"))
            If txtKodeSupplier.Text <> "" Then
                frmEntri.IDSupplier = NullToLong(txtKodeSupplier.EditValue)
                frmEntri.IDGudang = NullToLong(txtGudang.EditValue)
                frmEntri.IsNew = False
                frmEntri.NoID = IDDetil
                frmEntri.IDReturBeli = NoID
                frmEntri.FormPemanggil = Me
                frmEntri.Show()
                frmEntri.Focus()
                'If frmEntri.ShowDialog(Me)= Windows.Forms.DialogResult.OK Then
                '    RefreshDetil()
                'End If
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

    Private Sub txtSubtotal2_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtSubtotal2.EditValueChanged

    End Sub

    Private Sub txtSubtotal2_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtSubtotal2.LostFocus
        HitungTotal()
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
                txtWilayah.Text = NullToStr(Ds.Tables(0).Rows(0).Item("Wilayah"))
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

    Private Sub gvNotaJual_DataSourceChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvNotaJual.DataSourceChanged
        If System.IO.File.Exists(FolderLayouts & Me.Name & gvNotaJual.Name & ".xml") Then
            SearchLookUpEdit1View.RestoreLayoutFromXml(FolderLayouts & Me.Name & gvNotaJual.Name & ".xml")
        End If
        With gvNotaJual
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

    Private Sub TypePajak_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TypePajak.EditValueChanged
        HitungTotal()
    End Sub

    Private Sub GV1_CellValueChanged(ByVal sender As Object, ByVal e As DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs) Handles GV1.CellValueChanged
        Dim cn As New SqlConnection
        Dim com As New SqlCommand
        Try
            If GV1.FocusedColumn.FieldName.ToUpper = "NamaStock".ToUpper Then
                cn.ConnectionString = StrKonSql
                cn.Open()
                com.Connection = cn
                SQL = "UPDATE MReturBeliD SET NamaStock='" & FixApostropi(NullToStr(e.Value)) & "' WHERE NoID=" & NullToLong(GV1.GetRowCellValue(e.RowHandle, "NoID"))
                com.CommandText = SQL
                com.ExecuteNonQuery()
            End If
        Catch ex As Exception
        Finally
            If cn.State = ConnectionState.Open Then
                cn.Close()
            End If
            cn.Dispose()
            com.Dispose()
        End Try
    End Sub

    Private Sub GV1_ColumnChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles GV1.ColumnChanged
        'If IsRevisi AndAlso GV1.Columns("NamaStock").Name.ToUpper = "NamaStock".ToUpper Then
        '    GV1.OptionsBehavior.Editable = True
        'Else
        '    GV1.OptionsBehavior.Editable = False
        'End If
    End Sub

    Private Sub GV1_FocusedColumnChanged(ByVal sender As Object, ByVal e As DevExpress.XtraGrid.Views.Base.FocusedColumnChangedEventArgs) Handles GV1.FocusedColumnChanged
        If IsRevisi AndAlso GV1.FocusedColumn.FieldName.ToUpper = "NamaStock".ToUpper Then
            GV1.OptionsBehavior.Editable = True
        Else
            GV1.OptionsBehavior.Editable = False
        End If
    End Sub

    Private Sub tglFPMasukkan_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tglFPMasukkan.EditValueChanged
        
    End Sub

    Private Sub txtWP_EditValueChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtWP.EditValueChanged
        If txtWP.Text <> "" Then
            ckAktif.Checked = True
        Else
            ckAktif.Checked = False
        End If
    End Sub

    Private Sub txtMasaPajak_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtMasaPajak.EditValueChanged
        RubahNoFP()
    End Sub

    Private Sub RubahNoFP()
        If IsEntriFakturPajak AndAlso FakturPajakLama = "" Then
            txtNoFakturPajak.Text = "PA/" & txtMasaPajak.DateTime.ToString("yy") & "/" & clsKode.MintaKodeBaruFP("MReturBeli", "NoFakturPajak", txtMasaPajak.DateTime, 7, 8)
        End If
    End Sub

    Private Sub ckAktif_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ckAktif.CheckedChanged
        RubahNoFP()
    End Sub

    Private Sub txtTglFP_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtTglFP.EditValueChanged

    End Sub

    Private Sub txtTglFP_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtTglFP.KeyDown
        Dim x As New frmOtorisasiAdmin
        Try
            If e.KeyCode = Keys.F7 AndAlso x.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
                txtTglFP.Properties.ReadOnly = False
            End If
        Catch ex As Exception
            XtraMessageBox.Show("Info Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
        End Try
    End Sub
End Class