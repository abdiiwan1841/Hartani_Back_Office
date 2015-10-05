Imports System.Data
Imports System.Data.SqlClient
Imports DevExpress.XtraEditors
Imports DevExpress.Utils
Imports DevExpress.XtraGrid.Views.Base
Imports System.Xml
Imports VPoint.mdlCetakCR

Public Class frmEntriJualPOS
    Dim SQL As String = ""
    Public Enum pStatus
        Baru = 0
        Edit = 1
    End Enum
    Public pTipe As pStatus
    Public IsPosted As Boolean = False
    Public IsPOS As Boolean = False
    Public NoID As Long = -1
    Public LimitHutang As Double
    Dim oda2 As SqlDataAdapter
    Dim KodeLama As String = ""
    Dim FakturPajakLama As String = ""
    Dim oDS As New DataSet
    Dim BS As New BindingSource
    Dim IDPackingOld As Long = -1
    Dim defIDKassa As Long = 1
    'For Back Action
    Public FormNameDaftar As String = ""
    Public TableNameDaftar As String = ""
    Public TextDaftar As String = ""
    Public FormEntriDaftar As String = ""
    Public TableMasterDaftar As String = ""
    Dim IsTempInsertBaru As Boolean = False

    Private Sub RefreshDataKontak()
        Dim ds As New DataSet
        Dim dsSalesman As New DataSet
        Try
            SQL = "SELECT NoID, Kode, Nama FROM MAlamat WHERE IsActive=1 AND IsCustomer=1"
            ds = ExecuteDataset("master", SQL)
            txtKodeCustomer.Properties.DataSource = ds.Tables("master")
            If System.IO.File.Exists(FolderLayouts & Me.Name & SearchLookUpEdit1View.Name & ".xml") Then
                SearchLookUpEdit1View.RestoreLayoutFromXml(FolderLayouts & Me.Name & SearchLookUpEdit1View.Name & ".xml")
            End If
            SQL = "SELECT NoID, Kode, Nama FROM MAlamat WHERE IsActive=1 AND IsPegawai=1"
            dsSalesman = ExecuteDataset("MSalesman", SQL)
            txtKodeSalesman.Properties.DataSource = dsSalesman.Tables("MSalesman")
            If System.IO.File.Exists(FolderLayouts & Me.Name & GVSalesman.Name & ".xml") Then
                GVSalesman.RestoreLayoutFromXml(FolderLayouts & Me.Name & GVSalesman.Name & ".xml")
            End If

        Catch ex As Exception
            XtraMessageBox.Show("Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            ds.Dispose()
            dsSalesman.Dispose()
        End Try
    End Sub
    Private Sub RefreshDataPacking()
        Dim ds As New DataSet
        Dim repcheckEdit As New Repository.RepositoryItemCheckEdit
        Try
            'SQL = "SELECT MPacking.NoID, MPacking.Kode AS NoPacking, MPacking.Tanggal " & vbCrLf & _
            '      " FROM (MPackingD " & vbCrLf & _
            '      " INNER JOIN MPacking ON MPacking.NoID=MPackingD.IDPacking) " & vbCrLf & _
            '      " WHERE ((MPackingD.Qty*MPackingD.Konversi)-IsNull((SELECT SUM(MJualD.Qty*MJualD.Konversi) FROM MJualD INNER JOIN MJual ON MJual.NoID=MJualD.IDJual WHERE MJualD.IDPackingD=MPackingD.NoID),0))>0 AND IsNull(MPacking.IsSelesai,0)=0 AND IsNull(MPacking.IsPosted,0)=1 AND MPacking.IDWilayah=" & NullToLong(DefIDWilayah) & " AND MPacking.IDCustomer=" & NullToLong(txtKodeCustomer.EditValue) & vbCrLf & _
            '      " GROUP BY MPacking.NoID, MPacking.Kode, MPacking.Tanggal, MPacking.IsSelesai, MPacking.IsPosted, MPacking.IDWilayah, MPacking.IDCustomer"

            'SQL = "SELECT X.NoID, X.NoPacking, X.Tanggal FROM (SELECT MPacking.NoID, MPacking.Kode AS NoPacking, MPacking.Tanggal " & _
            '      " FROM (MPackingD INNER JOIN MPacking ON MPacking.NoID=MPackingD.IDPacking) " & _
            '      " WHERE IsNull(MPacking.IsSelesai, 0) = 0 And IsNull(MPacking.IsPosted, 0) = 1 And MPacking.IDGudang = " & NullToLong(txtGudang.EditValue) & " And MPacking.IDCustomer = " & NullToLong(txtKodeCustomer.EditValue) & " " & _
            '      " GROUP BY MPackingD.IDBarang, MPacking.NoID, MPacking.Kode, MPacking.Tanggal, MPacking.IsSelesai, MPacking.IsPosted, MPacking.IDWilayah, MPacking.IDCustomer " & _
            '      " HAVING (SUM(MPackingD.Qty*MPackingD.Konversi)-IsNull((SELECT SUM(MJualD.Qty*MJualD.Konversi) FROM MJualD INNER JOIN MJual ON MJual.NoID=MJualD.IDJual WHERE MJual.NoID<>" & NoID & " AND MJual.IDPacking=MPacking.NoID AND MJualD.IDBarang=MPackingD.IDBarang),0))>0) X" & _
            '      " GROUP BY X.NoID, X.NoPacking, X.Tanggal "

            'SQL = "SELECT NoID, Kode, Tanggal, SUM(QtySisa) AS QtySisa FROM (SELECT MPacking.NoID, MPacking.Kode, MPacking.Tanggal, (MPackingD.Qty*MPackingD.Konversi)-IsNull((SELECT SUM(MJualD.Qty*MJualD.Konversi) FROM MJualD INNER JOIN MJual ON MJual.NoID=MJualD.IDJual WHERE MJual.NoID<>" & NoID & " AND MJualD.IDPackingD=MPackingD.NoID),0) AS QtySisa FROM MPacking INNER JOIN MPackingD ON MPackingD.IDPacking=MPacking.NoID "
            'SQL &= " WHERE MPackingD.IsPacking = 1 And MPacking.IsPosted = 1 And MPacking.IDCustomer=" & NullToLong(txtKodeCustomer.EditValue) & " AND (MPackingD.Qty*MPackingD.Konversi)-IsNull((SELECT SUM(MJualD.Qty*MJualD.Konversi) FROM MJualD INNER JOIN MJual ON MJual.NoID=MJualD.IDJual WHERE MJual.NoID<>" & NoID & " AND MJualD.IDPackingD=MPackingD.NoID),0)>0 AND MPacking.NoID<>" & NullToLong(txtPacking.EditValue)
            'SQL &= " ) X GROUP BY NoID, Kode, Tanggal"

            'ds = ExecuteDataset("master", SQL)
            'txtPacking.Properties.DataSource = ds.Tables("master")
            'txtPacking.Properties.DisplayMember = "NoPacking"
            'txtPacking.Properties.ValueMember = "NoID"

            'SQL = "SELECT NoID, Kode, Nama,TarifProsen FROM MTypePajak WHERE IsActive=1 "
            'ds = ExecuteDataset("masterPajak", SQL)
            'TypePajak.Properties.DataSource = ds.Tables("masterPajak")
            'TypePajak.Properties.ValueMember = "NoID"
            'TypePajak.Properties.DisplayMember = "Nama"
            'If System.IO.File.Exists(FolderLayouts & Me.Name & GVPajak.Name & ".xml") Then
            '    GVPajak.RestoreLayoutFromXml(FolderLayouts & Me.Name & GVPajak.Name & ".xml")
            'End If

        Catch ex As Exception
            XtraMessageBox.Show("Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            ds.Dispose()
        End Try
    End Sub
    Dim DefImageList As New ImageList

    Private Sub frmEntriJual_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        Dim ds As New DataSet
        Try
            'If Not IsTempInsertBaru AndAlso pTipe = pStatus.Edit Then
            '    SQL = "SELECT MPackingD.Qty*MPackingD.Konversi-IsNull((SELECT SUM(MJualD.Qty*MjualD.Konversi) FROM MJualD WHERE MJualD.IDJual=" & NoID & " AND MJualD.IDPackingD=MPackingD.NoID),0) AS QtySisa " & _
            '          " FROM MPackingD INNER JOIN MPacking ON MPacking.NoID=MPackingD.IDPacking " & _
            '          " WHERE  MPacking.NoID IN (SELECT MPackingD.IDPacking FROM MJualD LEFT JOIN MPackingD ON MPackingD.NoID=MJualD.IDPackingD WHERE MJualD.IDJual=" & NoID & ")"
            '    ds = ExecuteDataset("MJual", SQL)
            '    If ds.Tables("MJual").Rows.Count >= 1 Then
            '        For i As Integer = 0 To ds.Tables("MJual").Rows.Count - 1
            '            If NullToDbl(ds.Tables("MJual").Rows(i).Item("QtySisa")) > 1 Then
            '                XtraMessageBox.Show("Qty penjualan tidak sama dengan Qty Packing", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
            '                e.Cancel = True
            '            End If
            '        Next
            '    End If
            'End If
            If Not IsPending AndAlso IsTempInsertBaru Then
                If XtraMessageBox.Show("Yakin ingin keluar?" & vbCrLf & "Ya, akan menghapus transaksi dan detil item.", NamaAplikasi, MessageBoxButtons.YesNo, MessageBoxIcon.Information, MessageBoxDefaultButton.Button2) = Windows.Forms.DialogResult.Yes Then
                    EksekusiSQL("DELETE FROM MJual WHERE NoID=" & NoID)
                    EksekusiSQL("DELETE FROM MJualD WHERE IDJual=" & NoID)
                Else
                    e.Cancel = True
                End If
            End If
        Catch ex As Exception
        Finally
            ds.Dispose()
        End Try
    End Sub

    Dim IsPending As Boolean = False

    Private Sub LoadData()
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
            If System.IO.File.Exists(FolderLayouts & Me.Name & GVSalesman.Name & ".xml") Then
                GVSalesman.RestoreLayoutFromXml(FolderLayouts & Me.Name & GVSalesman.Name & ".xml")
            End If
            If System.IO.File.Exists(FolderLayouts & Me.Name & gvPacking.Name & ".xml") Then
                gvPacking.RestoreLayoutFromXml(FolderLayouts & Me.Name & gvPacking.Name & ".xml")
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

    Private Sub frmEntriJual_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        LoadData()
    End Sub
    Private Sub RefreshData()
        Dim DS As New DataSet
        Try
            RefreshDataKontak()
            If pTipe = pStatus.Baru Then
                IsiDefault()
            Else
                SQL = "SELECT MJual.*, MAlamat.Nama AS NamaAlamat, MAlamat.Kode as KodeAlamat,MSalesman.Nama as Salesman " & _
                "FROM MJual LEFT JOIN MAlamat ON MAlamat.NoID=MJual.IDCustomer " & _
                "LEFT JOIN MAlamat MSalesman ON MJual.IDBagPembelian=MSalesman.NoID WHERE MJual.NoID=" & NoID
                DS = ExecuteDataset("Tabel", SQL)
                If DS.Tables("Tabel").Rows.Count >= 1 Then
                    RefreshDetil()
                    txtKodeCustomer.EditValue = NullToLong(DS.Tables(0).Rows(0).Item("IDCustomer"))
                    txtKodeSalesman.EditValue = NullToLong(DS.Tables(0).Rows(0).Item("IDBagPembelian"))
                    'TypePajak.EditValue = NullToLong(DS.Tables(0).Rows(0).Item("IDTypePajak"))
                    txtNamaSalesman.Text = NullToStr(DS.Tables(0).Rows(0).Item("Salesman"))
                    'txtSalesman.EditValue = NullToLong(DS.Tables(0).Rows(0).Item("IDBagPenjualan"))
                    'txtGudang.EditValue = NullToLong(DS.Tables(0).Rows(0).Item("IDGudang"))
                    'RubahGudang()
                    'txtKodeCustomer.Text = NullTostr(DS.Tables(0).Rows(0).Item("KodeAlamat"))
                    txtNamaCustomer.Text = NullToStr(DS.Tables(0).Rows(0).Item("NamaAlamat"))
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
                    txtKeterangan.Text = NullToStr(DS.Tables(0).Rows(0).Item("Keterangan"))
                    txtSubtotal.EditValue = NullToDbl(DS.Tables(0).Rows(0).Item("SubTotal"))
                    txtDiscPersen.EditValue = NullToDbl(DS.Tables(0).Rows(0).Item("DiskonNotaProsen"))
                    txtDiscRp.EditValue = NullToDbl(DS.Tables(0).Rows(0).Item("DiskonNotaRp"))
                    txtDiscTotal.EditValue = NullToDbl(DS.Tables(0).Rows(0).Item("DiskonNotaTotal"))
                    'txtBiaya.EditValue = NullToDbl(DS.Tables(0).Rows(0).Item("Biaya"))
                    txtTotal.EditValue = NullToDbl(DS.Tables(0).Rows(0).Item("Total"))
                    txtBayar.EditValue = NullToDbl(DS.Tables(0).Rows(0).Item("Bayar"))
                    txtSisa.EditValue = NullToDbl(DS.Tables(0).Rows(0).Item("Sisa"))
                    txtPPNProsen.EditValue = NullToDbl(DS.Tables(0).Rows(0).Item("PPN"))
                    txtPPN.EditValue = NullToDbl(DS.Tables(0).Rows(0).Item("NilaiPPN"))
                    txtDPP.EditValue = NullToDbl(DS.Tables(0).Rows(0).Item("DPP"))
                    IsPosted = NullToBool(DS.Tables(0).Rows(0).Item("IsPosted"))
                    ckCash.Checked = NullToBool(DS.Tables(0).Rows(0).Item("IsCash"))
                    IDPackingOld = NullToLong(DS.Tables(0).Rows(0).Item("IDPacking"))
                    txtPacking.EditValue = NullToLong(DS.Tables(0).Rows(0).Item("IDPacking"))
                    IsPOS = NullToBool(DS.Tables(0).Rows(0).Item("IsPOS"))

                    lbKassa.Text = "Kassa : " & NullToLong(DS.Tables(0).Rows(0).Item("IDPOS")).ToString("00")

                    HitungTotal()
                    cmdSave.Text = "&Bayar"
                    'If IsPOS Then
                    '    cmdSave.Enabled = False
                    '    cmdBAru.Enabled = False
                    '    cmdEdit.Enabled = False
                    '    cmdDelete.Enabled = False
                    'End If
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
            strsql = "select MBarangD.Barcode,MJualD.*,MJualD.DiscPersen1 AS [Disc1(%)], MJualD.DiscPersen2 AS [Disc2(%)], MJualD.DiscPersen3 AS [Disc3(%)], MJualD.Disc1 AS [Disc1(Rp)], MGudang.Nama AS Gudang, MBarang.CtnPcs AS IsiCtn, MBarang.Kode as KodeStock,MMerk.Nama as Merk,MBarang.Nama as NamaStock,MSatuan.Kode as Satuan,Mgudang.Kode as KodeGudang, MPacking.Kode AS KodePacking, MPackingD.NoPacking, " & vbCrLf
            strsql &= " ROUND((MJualD.Harga*(1-(MJualD.DiscPersen1/100))*(1-(MJualD.DiscPersen2/100))*(1-(MJualD.DiscPersen3/100)))-MJualD.Disc1-MJualD.Disc2-MJualD.Disc3 ,0) AS HargaNetto "
            strsql &= " From (MJualD Inner Join MJual On MJualD.IDJual=MJual.NoID) " & vbCrLf
            strsql &= " LEFT JOIN MBarangD ON MJualD.IDBarangD=MBarangD.NoID " & vbCrLf
            strsql &= " LEFT JOIN MBarang ON MJualD.IDBarang=MBarang.NoID " & vbCrLf
            strsql &= " LEFT JOIN MMerk ON MBarang.IDMerk=MMerk.NoID " & vbCrLf
            strsql &= " LEFT JOIN (MPackingD INNER JOIN MPacking ON MPacking.NoID=MPackingD.IDPacking) ON MJualD.IDPackingD=MPackingD.NoID " & vbCrLf
            strsql &= " LEFT JOIN MSatuan ON MJualD.IDSatuan=MSatuan.NoID " & vbCrLf
            strsql &= " LEFT JOIN MGudang ON MJualD.IDGudang=MGudang.NoID " & vbCrLf
            strsql &= " where MJualD.IDJual = " & NoID
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
            txtBarang.Focus()
            HitungTotal()
            GV1.HideFindPanel()
            'If pTipe = pStatus.Edit Then
            '    RefreshDataPacking()
            'End If
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
            txtDiscPersen.EditValue = txtDiscRp.EditValue * 100 / IIf(txtSubtotal.EditValue = 0, 1, txtSubtotal.EditValue)
            txtDiscTotal.EditValue = txtDiscRp.EditValue '(NullToDbl(txtDiscPersen.EditValue) * NullToDbl(txtSubtotal.EditValue) / 100) + txtDiscRp.EditValue
            txtSubtotal2.EditValue = txtSubtotal.EditValue - txtDiscTotal.EditValue
            'txtPPN.EditValue = NullToDbl(EksekusiSQlSkalarNew("Select Sum(PPN) from MJualD where IDJual=" & NoID))

            'Select Case NullToLong(TypePajak.EditValue)
            '    Case 1 'Sudah Termasuk pajak
            '        txtPPNProsen.EditValue = NullToDbl(EksekusiSQlSkalarNew("SELECT TarifProsen FROM MTypePajak WHERE NoID=" & NullToLong(TypePajak.EditValue)))
            '        txtDPP.EditValue = Bulatkan(NullToDbl(txtSubtotal.EditValue) / (1 + (txtPPNProsen.EditValue / 100)), 0)
            '        txtPPN.EditValue = NullToDbl(txtSubtotal.EditValue) - NullToDbl(txtDPP.EditValue)
            '        txtTotal.EditValue = txtSubtotal2.EditValue + txtBiaya.EditValue
            '    Case 2 'Belum Termasuk Pajak
            '        txtDPP.EditValue = NullToDbl(txtSubtotal.EditValue)
            '        txtPPNProsen.EditValue = NullToDbl(EksekusiSQlSkalarNew("SELECT TarifProsen FROM MTypePajak WHERE NoID=" & NullToLong(TypePajak.EditValue)))
            '        txtPPN.EditValue = Bulatkan(NullToDbl(txtSubtotal.EditValue) * NullToDbl(txtPPNProsen.EditValue) / 100, 0)
            '        txtTotal.EditValue = txtSubtotal2.EditValue + txtPPN.EditValue + txtBiaya.EditValue
            '    Case Else 'Non BKP
            '        txtDPP.EditValue = 0
            '        txtPPN.EditValue = 0
            '        txtPPNProsen.EditValue = 0
            '        txtTotal.EditValue = txtSubtotal2.EditValue - txtPPN.EditValue + txtBiaya.EditValue
            'End Select

            txtTotal.EditValue = txtSubtotal2.EditValue + txtBiaya.EditValue
            If ckCash.Checked Then
                txtBayar.EditValue = txtTotal.EditValue
            End If
            txtSisa.EditValue = txtTotal.EditValue - txtBayar.EditValue
            txtTotalCTN.EditValue = NullToDbl(EksekusiSQlSkalarNew("SELECT SUM(Ctn) FROM MJualD WHERE IDJual=" & NoID))
            txtTotalBKP.EditValue = txtDPP.EditValue + txtPPN.EditValue
            'txtKodeReff.Text = ""
            'SQL = "SELECT MPacking.Kode FROM MJualD LEFT JOIN (MPackingD INNER JOIN MPacking ON MPacking.NoID=MPackingD.IDPacking) ON MPackingD.NoID=MJualD.IDPackingD WHERE MJualD.IDJual=" & NoID & " GROUP BY MPacking.Kode ORDER BY MPacking.Kode"
            'ds = ExecuteDataset("MPacking", SQL)
            'For i As Integer = 0 To ds.Tables("MPacking").Rows.Count - 1
            '    txtKodeReff.Text = txtKodeReff.Text & IIf(i = 0, "", ", ") & NullToStr(ds.Tables("MPacking").Rows(i).Item("Kode"))
            'Next

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
        IsPending = False
        IsTempInsertBaru = False
        lbKassa.Text = "Kassa : " & defIDKassa.ToString("00")
        'txtGudang.EditValue = DefIDGudang
        'RubahGudang()
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

        cmdPending.ImageList = DefImageList
        cmdPending.ImageIndex = 13

        If pTipe = pStatus.Baru Or IsPosted Then
            cmdBAru.Enabled = False
            cmdEdit.Enabled = False
            cmdDelete.Enabled = False
            cmdSave.Enabled = True
        Else
            cmdBAru.Enabled = True
            cmdEdit.Enabled = True
            cmdDelete.Enabled = True
            cmdSave.Enabled = True
        End If
    End Sub
    Private Sub cmdSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSave.Click
        Dim PesanSalah As String = ""
        Dim ds As New DataSet
        Dim Harga As Double = 0.0
        Dim Disc1 As Double = 0.0
        Dim Disc2 As Double = 0.0
        Dim Disc3 As Double = 0.0
        Dim DiscRp1 As Double = 0.0
        Dim DiscRp2 As Double = 0.0
        Dim DiscRp3 As Double = 0.0
        Dim Jumlah As Double = 0.0
        If Not IsPosted Then
            If IsValidasi() Then
                If Simpan(PesanSalah) = True Then
                    If pTipe = pStatus.Baru Then
                        KodeLama = txtKode.Text
                        FakturPajakLama = txtNoFakturPajak.Text
                        pTipe = pStatus.Edit
                        SetTombol()
                        txtBarang.Focus()
                        cmdSave.Text = "&Bayar"

                        EksekusiSQL("DELETE FROM MJualD WHERE IDJual=" & NoID)
                        IDPackingOld = NullToLong(txtPacking.EditValue)
                        SQL = "SELECT MPackingD.*, MSO.IDBagPenjualan AS IDSalesman, MSO.DiskonNotaProsen, MSOD.HargaPcs AS HargaPcsSO, MSOD.Harga AS HargaSO, MSOD.Disc1 AS Disc1SO, MSOD.Disc2 AS Disc2SO, MSOD.Disc3 AS Disc3SO, MSOD.DiscPersen1 AS DiscPersen1SO, MSOD.DiscPersen2 AS DiscPersen2SO, MSOD.DiscPersen3 AS DiscPersen3SO, MBarang.CtnPcs, MSatuanBase.NoID AS IDSatuanBase, MSatuanBase.Konversi AS KonversiBase, (MPackingD.Qty*MPackingD.Konversi)-IsNull((SELECT SUM(MJualD.Qty*MJualD.Konversi) FROM MJualD INNER JOIN MJual ON MJual.NoID=MJualD.IDJual WHERE MJualD.IDPackingD=MPackingD.NoID),0) AS SisaQty " & _
                              " FROM MPackingD " & _
                              " LEFT JOIN MBarang ON MBarang.NoID=MPackingD.IDBarang " & _
                              " LEFT JOIN ((MSPKD LEFT JOIN (MSOD INNER JOIN MSO ON MSO.NoID=MSOD.IDSO) ON MSOD.NoID=MSPKD.IDSOD) INNER JOIN MSPK ON MSPK.NoID=MSPKD.IDSPK) ON MSPKD.NoID=MPackingD.IDSPKD " & _
                              " LEFT JOIN (MSatuan LEFT JOIN MSatuan MSatuanBase ON MSatuanBase.NoID=MSatuan.IDBase) ON MSatuan.NoID=MPackingD.IDSatuan " & _
                              " WHERE MPackingD.IDPacking=" & NullToLong(txtPacking.EditValue) & _
                              " AND (MPackingD.Qty*MPackingD.Konversi)-IsNull((SELECT SUM(MJualD.Qty*MJualD.Konversi) FROM MJualD INNER JOIN MJual ON MJual.NoID=MJualD.IDJual WHERE MJualD.IDPackingD=MPackingD.NoID),0)>0" & _
                              " ORDER BY MPackingD.NoPacking "
                        ds = ExecuteDataset("MPackingD", SQL)
                        With ds.Tables("MPackingD")
                            For i As Integer = 0 To .Rows.Count - 1
                                'txtQty.EditValue / IsiKarton * txtKonversi.EditValue
                                If NullToDbl(.Rows(i).Item("Konversi")) <= NullToDbl(.Rows(i).Item("SisaQty")) Then
                                    Harga = NullToDbl(.Rows(i).Item("Harga")) / NullToDbl(.Rows(i).Item("Konversi"))
                                    Disc1 = NullToDbl(.Rows(i).Item("DiscPersen1SO"))
                                    Disc2 = NullToDbl(.Rows(i).Item("DiscPersen2SO"))
                                    Disc3 = NullToDbl(.Rows(i).Item("DiscPersen3SO"))
                                    DiscRp1 = NullToDbl(.Rows(i).Item("Disc1SO")) / NullToDbl(.Rows(i).Item("Konversi"))
                                    DiscRp2 = NullToDbl(.Rows(i).Item("Disc2SO")) / NullToDbl(.Rows(i).Item("Konversi"))
                                    DiscRp3 = NullToDbl(.Rows(i).Item("Disc3SO")) / NullToDbl(.Rows(i).Item("Konversi"))
                                    If Harga <= 0 Then
                                        'Harga = clsPostingPenjualan.HargaJual(NullToLong(txtKodeCustomer.EditValue), NullToLong(.Rows(i).Item("IDBarangD")), NullToDbl(.Rows(i).Item("Qty")) * NullToDbl(.Rows(i).Item("Konversi")), Disc1)
                                        Harga = clsPostingPenjualan.HargaJual(NullToLong(.Rows(i).Item("IDBarangD")), NullToDbl(.Rows(i).Item("Qty")) * NullToDbl(.Rows(i).Item("Konversi")), Disc1)
                                    End If
                                    Jumlah = (Bulatkan((Harga * (1 - (Disc1 / 100)) * (1 - (Disc2 / 100)) * (1 - (Disc3 / 100))), 0) - DiscRp1 - DiscRp2 - DiscRp3)
                                    Jumlah = (NullToDbl(.Rows(i).Item("SisaQty")) / NullToDbl(.Rows(i).Item("Konversi"))) * Jumlah
                                    SQL = "INSERT INTO [MJualD] ([IDSalesman], [NoID] ,[Tgl] ,[Jam] ,[IDJual] ,[IDBarang] ,[IDSatuan] ,[IDGudang] ,[Ctn] ,[Qty] ,[Harga] ,[DiscPersen1] ,[DiscPersen2] ,[DiscPersen3] ,[Disc1] ,[Disc2] ,[Disc3] ,[Jumlah] ,[IDDOD] ,[IDAkunPersediaan] ,[Catatan] ,[NoUrut] ,[HargaPcs] ,[QtyPcs] ,[IDAsal] ,[Konversi] ,[KodeGolongan] ,[HargaAsal] ,[RupiahAsal] ,[Selisih] ,[SelisihGlobal] ,[HargaPokok] ,[KodeJenis] ,[IDServerAsal] ,[IDPackingD])" & _
                                          " VALUES (" & NullToLong(.Rows(i).Item("IDSalesman")) & "," & NullToLong(GetNewID("MJualD", "NoID")) & " ,getdate() ,getdate() ," & NoID & " ," & NullToLong(.Rows(i).Item("IDBarang")) & " ," & NullToLong(.Rows(i).Item("IDSatuan")) & " ," & NullToLong(.Rows(i).Item("IDGudang")) & " ," & FixKoma(IIf(NullToDbl(.Rows(i).Item("CtnPcs")) = 0, 0, NullToDbl(.Rows(i).Item("SisaQty")) / NullToDbl(.Rows(i).Item("CtnPcs")))) & " ," & FixKoma(NullToDbl(.Rows(i).Item("SisaQty")) / IIf(NullToDbl(.Rows(i).Item("Konversi")) = 0, 1, NullToDbl(.Rows(i).Item("Konversi")))) & " ," & FixKoma(Harga) & " ," & FixKoma(Disc1) & _
                                          " ," & FixKoma(Disc2) & " ," & FixKoma(Disc3) & " ," & FixKoma(DiscRp1) & " ," & FixKoma(DiscRp2) & " ," & FixKoma(DiscRp3) & " ," & FixKoma(Jumlah) & " ,-1 ,-1 ,'" & FixApostropi(NullToStr(.Rows(i).Item("Catatan"))) & "' ," & _
                                          NullToLong(GetNewID("MJualD", "NoUrut", " WHERE MJualD.IDJual=" & NoID)) & " ," & FixKoma(Harga / IIf(NullToDbl(.Rows(i).Item("Konversi")) = 0, 1, NullToDbl(.Rows(i).Item("Konversi")))) & " ," & FixKoma(NullToDbl(.Rows(i).Item("SisaQty"))) & " ," & FixKoma(NullToDbl(.Rows(i).Item("IDAsal"))) & " ," & FixKoma(NullToDbl(.Rows(i).Item("Konversi"))) & " ,NULL ,NULL ,NULL ,NULL ,NULL ,NULL ,NULL ,NULL ," & NullToLong(.Rows(i).Item("NoID")) & ")"
                                Else
                                    Harga = NullToDbl(.Rows(i).Item("Harga")) / NullToDbl(.Rows(i).Item("KonversiBase"))
                                    Disc1 = NullToDbl(.Rows(i).Item("DiscPersen1SO"))
                                    Disc2 = NullToDbl(.Rows(i).Item("DiscPersen2SO"))
                                    Disc3 = NullToDbl(.Rows(i).Item("DiscPersen3SO"))
                                    DiscRp1 = NullToDbl(.Rows(i).Item("Disc1SO")) / NullToDbl(.Rows(i).Item("KonversiBase"))
                                    DiscRp2 = NullToDbl(.Rows(i).Item("Disc2SO")) / NullToDbl(.Rows(i).Item("KonversiBase"))
                                    DiscRp3 = NullToDbl(.Rows(i).Item("Disc3SO")) / NullToDbl(.Rows(i).Item("KonversiBase"))
                                    If Harga <= 0 Then
                                        'Harga = clsPostingPenjualan.HargaJual(NullToLong(txtKodeCustomer.EditValue), NullToLong(.Rows(i).Item("IDBarangD")), NullToDbl(.Rows(i).Item("Qty")) * NullToDbl(.Rows(i).Item("KonversiBase")), Disc1)
                                        Harga = clsPostingPenjualan.HargaJual(NullToLong(.Rows(i).Item("IDBarangD")), NullToDbl(.Rows(i).Item("Qty")) * NullToDbl(.Rows(i).Item("KonversiBase")), Disc1)
                                    End If
                                    Jumlah = (Bulatkan((Harga * (1 - (Disc1 / 100)) * (1 - (Disc2 / 100)) * (1 - (Disc3 / 100))), 0) - DiscRp1 - DiscRp2 - DiscRp3)
                                    Jumlah = (NullToDbl(.Rows(i).Item("SisaQty")) / NullToDbl(.Rows(i).Item("KonversiBase"))) * Jumlah
                                    SQL = "INSERT INTO [MJualD] ([IDSalesman], [NoID] ,[Tgl] ,[Jam] ,[IDJual] ,[IDBarang] ,[IDSatuan] ,[IDGudang] ,[Ctn] ,[Qty] ,[Harga] ,[DiscPersen1] ,[DiscPersen2] ,[DiscPersen3] ,[Disc1] ,[Disc2] ,[Disc3] ,[Jumlah] ,[IDDOD] ,[IDAkunPersediaan] ,[Catatan] ,[NoUrut] ,[HargaPcs] ,[QtyPcs] ,[IDAsal] ,[Konversi] ,[KodeGolongan] ,[HargaAsal] ,[RupiahAsal] ,[Selisih] ,[SelisihGlobal] ,[HargaPokok] ,[KodeJenis] ,[IDServerAsal] ,[IDPackingD])" & _
                                    " VALUES (" & NullToLong(.Rows(i).Item("IDSalesman")) & "," & NullToLong(GetNewID("MJualD", "NoID")) & " ,getdate() ,getdate() ," & NoID & " ," & NullToLong(.Rows(i).Item("IDBarang")) & " ," & NullToLong(.Rows(i).Item("IDSatuanBase")) & " ," & NullToLong(.Rows(i).Item("IDGudang")) & " ," & FixKoma(IIf(NullToDbl(.Rows(i).Item("CtnPcs")) = 0, 0, NullToDbl(.Rows(i).Item("SisaQty")) / NullToDbl(.Rows(i).Item("CtnPcs")))) & " ," & FixKoma(NullToDbl(.Rows(i).Item("SisaQty")) / IIf(NullToDbl(.Rows(i).Item("KonversiBase")) = 0, 1, NullToDbl(.Rows(i).Item("KonversiBase")))) & " ," & FixKoma(Harga) & " ," & FixKoma(Disc1) & _
                                    " ," & FixKoma(Disc2) & " ," & FixKoma(Disc3) & " ," & FixKoma(DiscRp1) & " ," & FixKoma(DiscRp2) & " ," & FixKoma(DiscRp3) & " ," & FixKoma(Jumlah) & " ,-1 ,-1 ,'" & FixApostropi(NullToStr(.Rows(i).Item("Catatan"))) & "' ," & _
                                    NullToLong(GetNewID("MJualD", "NoUrut", " WHERE MJualD.IDJual=" & NoID)) & " ," & FixKoma(Harga / IIf(NullToDbl(.Rows(i).Item("KonversiBase")) = 0, 1, NullToDbl(.Rows(i).Item("KonversiBase")))) & " ," & FixKoma(NullToDbl(.Rows(i).Item("SisaQty"))) & " ," & FixKoma(NullToDbl(.Rows(i).Item("IDAsal"))) & " ," & FixKoma(NullToDbl(.Rows(i).Item("KonversiBase"))) & " ,NULL ,NULL ,NULL ,NULL ,NULL ,NULL ,NULL ,NULL ," & NullToLong(.Rows(i).Item("NoID")) & ")"
                                End If
                                EksekusiSQL(SQL)
                            Next
                        End With

                        'EksekusiSQL("DELETE FROM MJualD WHERE IDJual=" & NoID)
                        'IDPackingOld = NullToLong(txtPacking.EditValue)
                        'SQL = "SELECT MPacking.NoID, MPackingD.IDBarang, MSO.DiskonNotaProsen, MPackingD.IDSatuan, MPackingD.Konversi, MBarang.CtnPcs, MSatuanBase.NoID AS IDSatuanBase, MSatuanBase.Konversi AS KonversiBase, SUM(MPackingD.Qty*MPackingD.Konversi) AS Qty," & vbCrLf & _
                        '      " IsNull((SELECT SUM(MJualD.Qty*MJualD.Konversi) FROM MJualD INNER JOIN MJual ON MJual.NoID=MJualD.IDJual WHERE MJual.IDPacking=MPacking.NoID AND MJualD.IDBarang=MPackingD.IDBarang),0) AS DiJual, AVG(MSOD.DiscPersen1) AS DiscPersen1, AVG(MSOD.DiscPersen2) AS DiscPersen2, AVG(MSOD.DiscPersen3) AS DiscPersen3, AVG(MSOD.Disc1) AS Disc1, AVG(MSOD.Disc2) AS Disc2, AVG(MSOD.Disc3) AS Disc3, AVG(MSOD.Harga) AS Harga, AVG(MSOD.HargaPcs) AS HargaPcs" & vbCrLf & _
                        '      " FROM MPackingD INNER JOIN MPacking ON MPacking.NoID=MPackingD.IDPacking " & vbCrLf & _
                        '      " LEFT JOIN ((MSPKD LEFT JOIN (MSOD INNER JOIN MSO ON MSO.NoID=MSOD.IDSO) ON MSOD.NoID=MSPKD.IDSOD) INNER JOIN MSPK ON MSPK.NoID=MSPKD.IDSPK) ON MSPKD.NoID=MPackingD.IDSPKD " & vbCrLf & _
                        '      " LEFT JOIN MBarang ON MBarang.NoID=MPackingD.IDBarang  " & vbCrLf & _
                        '      " LEFT JOIN (MSatuan LEFT JOIN MSatuan MSatuanBase ON MSatuanBase.NoID=MSatuan.IDBase) ON MSatuan.NoID=MPackingD.IDSatuan  " & vbCrLf & _
                        '      " WHERE MPackingD.IDGudang = " & NullToLong(txtGudang.EditValue) & " AND MPacking.NoID=" & NullToLong(txtPacking.EditValue) & vbCrLf & _
                        '      " GROUP BY MSO.DiskonNotaProsen, MPacking.NoID, MPackingD.IDBarang, MPackingD.IDSatuan, MPackingD.Konversi, MBarang.CtnPcs, MSatuanBase.NoID, MPackingD.IDGudang, MSatuanBase.Konversi" & _
                        '      " HAVING SUM(MPackingD.Qty*MPackingD.Konversi)-IsNull((SELECT SUM(MJualD.Qty*MJualD.Konversi) FROM MJualD INNER JOIN MJual ON MJual.NoID=MJualD.IDJual WHERE MJual.IDPacking=MPacking.NoID AND MJualD.IDBarang=MPackingD.IDBarang),0)>0"
                        'ds = ExecuteDataset("MPackingD", SQL)
                        'With ds.Tables("MPackingD")
                        '    For i As Integer = 0 To .Rows.Count - 1
                        '        'txtQty.EditValue / IsiKarton * txtKonversi.EditValue
                        '        If NullToDbl(.Rows(i).Item("Konversi")) <= NullToDbl(.Rows(i).Item("Qty")) - NullToDbl(.Rows(i).Item("DiJual")) Then
                        '            Harga = NullToDbl(.Rows(i).Item("Harga"))
                        '            Harga = Harga - (Harga * NullToDbl(.Rows(i).Item("DiskonNotaProsen")) / 100)
                        '            Disc1 = NullToDbl(.Rows(i).Item("DiscPersen1"))
                        '            Disc2 = NullToDbl(.Rows(i).Item("DiscPersen2"))
                        '            Disc3 = NullToDbl(.Rows(i).Item("DiscPersen3"))
                        '            DiscRp1 = NullToDbl(.Rows(i).Item("Disc1"))
                        '            DiscRp2 = NullToDbl(.Rows(i).Item("Disc2"))
                        '            DiscRp3 = NullToDbl(.Rows(i).Item("Disc3"))
                        '            If Harga <= 0 Then
                        '                Harga = clsPostingPenjualan.HargaJual(NullToLong(.Rows(i).Item("IDBarang")), NullToLong(.Rows(i).Item("IDSatuan")), NullToLong(txtKodeCustomer.EditValue), Disc1, Disc2)
                        '            End If
                        '            Jumlah = ((NullToDbl(.Rows(i).Item("Qty")) - NullToDbl(.Rows(i).Item("DiJual"))) / NullToDbl(.Rows(i).Item("Konversi"))) * (Bulatkan((Harga * (1 - (Disc1 / 100)) * (1 - (Disc2 / 100)) * (1 - (Disc3 / 100))), 0) - DiscRp1 - DiscRp2 - DiscRp3)
                        '            SQL = "INSERT INTO [MJualD] ([NoID] ,[Tgl] ,[Jam] ,[IDJual] ,[IDBarang] ,[IDSatuan] ,[IDGudang] ,[Ctn] ,[Qty] ,[Harga] ,[DiscPersen1] ,[DiscPersen2] ,[DiscPersen3] ,[Disc1] ,[Disc2] ,[Disc3] ,[Jumlah] ,[IDDOD] ,[IDAkunPersediaan] ,[Catatan] ,[NoUrut] ,[HargaPcs] ,[QtyPcs] ,[IDAsal] ,[Konversi] ,[KodeGolongan] ,[HargaAsal] ,[RupiahAsal] ,[Selisih] ,[SelisihGlobal] ,[HargaPokok] ,[KodeJenis] ,[IDServerAsal] ,[IDPackingD])" & _
                        '            " VALUES (" & NullToLong(GetNewID("MJualD", "NoID")) & " ,getdate() ,getdate() ," & NoID & " ," & NullToLong(.Rows(i).Item("IDBarang")) & " ," & NullToLong(.Rows(i).Item("IDSatuan")) & " ," & NullToLong(txtGudang.EditValue) & " ," & FixKoma(IIf(NullToDbl(.Rows(i).Item("CtnPcs")) = 0, 0, (NullToDbl(.Rows(i).Item("Qty")) - NullToDbl(.Rows(i).Item("DiJual"))) / NullToDbl(.Rows(i).Item("CtnPcs")))) & " ," & FixKoma((NullToDbl(.Rows(i).Item("Qty")) - NullToDbl(.Rows(i).Item("DiJual"))) / IIf(NullToDbl(.Rows(i).Item("Konversi")) = 0, 1, NullToDbl(.Rows(i).Item("Konversi")))) & " ," & FixKoma(Harga) & " ," & FixKoma(Disc1) & _
                        '            " ," & FixKoma(Disc2) & " ," & FixKoma(Disc3) & " ," & FixKoma(DiscRp1) & " ," & FixKoma(DiscRp2) & " ," & FixKoma(DiscRp3) & " ," & FixKoma(Jumlah) & " ,-1 ,-1 ,'' ," & _
                        '            NullToLong(GetNewID("MJualD", "NoUrut", " WHERE MJualD.IDJual=" & NoID)) & " ," & FixKoma(Harga / IIf(NullToDbl(.Rows(i).Item("Konversi")) = 0, 1, NullToDbl(.Rows(i).Item("Konversi")))) & " ," & FixKoma((NullToDbl(.Rows(i).Item("Qty")) - NullToDbl(.Rows(i).Item("DiJual")))) & " ," & -1 & " ," & FixKoma(NullToDbl(.Rows(i).Item("Konversi"))) & " ,NULL ,NULL ,NULL ,NULL ,NULL ,NULL ,NULL ,NULL ," & -1 & ")"
                        '        Else
                        '            Harga = NullToDbl(.Rows(i).Item("Harga"))
                        '            Harga = Harga - (Harga * NullToDbl(.Rows(i).Item("DiskonNotaProsen")) / 100)
                        '            Disc1 = NullToDbl(.Rows(i).Item("DiscPersen1"))
                        '            Disc2 = NullToDbl(.Rows(i).Item("DiscPersen2"))
                        '            Disc3 = NullToDbl(.Rows(i).Item("DiscPersen3"))
                        '            DiscRp1 = NullToDbl(.Rows(i).Item("Disc1"))
                        '            DiscRp2 = NullToDbl(.Rows(i).Item("Disc2"))
                        '            DiscRp3 = NullToDbl(.Rows(i).Item("Disc3"))
                        '            If Harga <= 0 Then
                        '                Harga = clsPostingPenjualan.HargaJual(NullToLong(.Rows(i).Item("IDBarang")), NullToLong(.Rows(i).Item("IDSatuanBase")), NullToLong(txtKodeCustomer.EditValue), Disc1, Disc2)
                        '            End If
                        '            Jumlah = ((NullToDbl(.Rows(i).Item("Qty")) - NullToDbl(.Rows(i).Item("DiJual"))) / NullToDbl(.Rows(i).Item("KonversiBase"))) * (Bulatkan((Harga * (1 - (Disc1 / 100)) * (1 - (Disc2 / 100)) * (1 - (Disc3 / 100))), 0) - DiscRp1 - DiscRp2 - DiscRp3)
                        '            SQL = "INSERT INTO [MJualD] ([NoID] ,[Tgl] ,[Jam] ,[IDJual] ,[IDBarang] ,[IDSatuan] ,[IDGudang] ,[Ctn] ,[Qty] ,[Harga] ,[DiscPersen1] ,[DiscPersen2] ,[DiscPersen3] ,[Disc1] ,[Disc2] ,[Disc3] ,[Jumlah] ,[IDDOD] ,[IDAkunPersediaan] ,[Catatan] ,[NoUrut] ,[HargaPcs] ,[QtyPcs] ,[IDAsal] ,[Konversi] ,[KodeGolongan] ,[HargaAsal] ,[RupiahAsal] ,[Selisih] ,[SelisihGlobal] ,[HargaPokok] ,[KodeJenis] ,[IDServerAsal] ,[IDPackingD])" & _
                        '            " VALUES (" & NullToLong(GetNewID("MJualD", "NoID")) & " ,getdate() ,getdate() ," & NoID & " ," & NullToLong(.Rows(i).Item("IDBarang")) & " ," & NullToLong(.Rows(i).Item("IDSatuanBase")) & " ," & NullToLong(txtGudang.EditValue) & " ," & FixKoma(IIf(NullToDbl(.Rows(i).Item("CtnPcs")) = 0, 0, (NullToDbl(.Rows(i).Item("Qty")) - NullToDbl(.Rows(i).Item("DiJual"))) / NullToDbl(.Rows(i).Item("CtnPcs")))) & " ," & FixKoma((NullToDbl(.Rows(i).Item("Qty")) - NullToDbl(.Rows(i).Item("DiJual"))) / IIf(NullToDbl(.Rows(i).Item("KonversiBase")) = 0, 1, NullToDbl(.Rows(i).Item("KonversiBase")))) & " ," & FixKoma(Harga) & " ," & FixKoma(Disc1) & _
                        '            " ," & FixKoma(Disc2) & " ," & FixKoma(Disc3) & " ," & FixKoma(DiscRp1) & " ," & FixKoma(DiscRp2) & " ," & FixKoma(DiscRp3) & " ," & FixKoma(Jumlah) & " ,-1 ,-1 ,'' ," & _
                        '            NullToLong(GetNewID("MJualD", "NoUrut", " WHERE MJualD.IDJual=" & NoID)) & " ," & FixKoma(Harga / IIf(NullToDbl(.Rows(i).Item("KonversiBase")) = 0, 1, NullToDbl(.Rows(i).Item("KonversiBase")))) & " ," & FixKoma((NullToDbl(.Rows(i).Item("Qty")) - NullToDbl(.Rows(i).Item("DiJual")))) & " ," & -1 & " ," & FixKoma(NullToDbl(.Rows(i).Item("KonversiBase"))) & " ,NULL ,NULL ,NULL ,NULL ,NULL ,NULL ,NULL ,NULL ," & -1 & ")"
                        '        End If
                        '        EksekusiSQL(SQL)
                        '    Next
                        'End With

                        'SQL = "SELECT MSO.IDBagPenjualan" & _
                        '      " FROM MPacking " & _
                        '      " LEFT JOIN ((MSPKD LEFT JOIN (MSOD INNER JOIN MSO ON MSO.NoID=MSOD.IDSO) ON MSOD.NoID=MSPKD.IDSOD) INNER JOIN MSPK ON MSPK.NoID=MSPKD.IDSPK) ON MSPK.NoID=MPacking.IDSPK" & _
                        '      " WHERE MPacking.NoID = " & NullToLong(txtPacking.EditValue)
                        'txtSalesman.EditValue = NullToLong(EksekusiSQlSkalarNew(SQL))
                        RefreshDetil()
                        IsTempInsertBaru = True
                    Else
                        clsPostingPenjualan.PostingStokBarangPenjualan(NoID)
                        PrintStruck()
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

                        'For i As Integer = 0 To GV1.RowCount - 1
                        '    SQL = "UPDATE MJualD SET IDGudang=" & NullToLong(txtGudang.EditValue) & " WHERE NoID=" & NullToLong(GV1.GetRowCellValue(i, "NoID"))
                        '    EksekusiSQL(SQL)
                        'Next
                        IsTempInsertBaru = False
                        'DialogResult = Windows.Forms.DialogResult.OK
                        Close()
                        Dispose()
                    End If
                Else
                    If Not IsPOS Then
                        XtraMessageBox.Show("Info Kesalahan : " & vbCrLf & PesanSalah, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                    End If
                End If
            End If
        End If
    End Sub
    Private Sub PrintStruck()
        Dim namafile As String
        Try
            namafile = Application.StartupPath & "\report\FakturStrukMJual.rpt"
            If System.IO.File.Exists(namafile) Then
                If EditReport Then
                    PrintReport(action_.Edit, namafile, Me.Text, "{MJual.NoID}=" & NoID & " AND {MJual.IsPosted}=True")
                Else
                    PrintReport(action_.Print, namafile, Me.Text, "{MJual.NoID}=" & NoID & " AND {MJual.IsPosted}=True")
                End If
            Else
                DevExpress.XtraEditors.XtraMessageBox.Show("Nama File :" & namafile & " tidak ditemukan.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Stop)
            End If
        Catch EX As Exception
            XtraMessageBox.Show("Untuk mencetak, pilih item yang mau dicetak dan click cetak faktur.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
        End Try
    End Sub
    Private Function IsValidasi() As Boolean
        Dim SisaQty As Double = 0.0
        Dim ds As New DataSet
        Try
            HitungTotal()
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
            'If NullToDbl(EksekusiSQlSkalarNew("SELECT SUM(Modal) FROM MReset WHERE Tanggal>='" & Tgl.DateTime.ToString("yyyy/MM/dd") & "' AND Tanggal<'" & Tgl.DateTime.AddDays(1).ToString("yyyy/MM/dd") & "'")) <= 0 Then
            '    Dim frModal As New frmModal
            '    If Not frModal.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
            '        XtraMessageBox.Show("Harap isikan mdal terlebih dahulu.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
            '        Return False
            '        Exit Function
            '    End If
            'End If
            'If TypePajak.Text = "" Then
            '    XtraMessageBox.Show("Type Pajak masih kosong.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
            '    TypePajak.Focus()
            '    Return False
            '    Exit Function
            'End If
            'If txtGudang.Text = "" Then
            '    XtraMessageBox.Show("Gudang masih kosong.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
            '    txtKode.Focus()
            '    Return False
            '    Exit Function
            'End If
            If Tgl.Text = "" Then
                XtraMessageBox.Show("Tanggal masih kosong.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
                Tgl.Focus()
                Return False
                Exit Function
            End If
            If CekKodeValid(txtKode.Text, KodeLama, "MJual", "Kode", IIf(pTipe = pStatus.Edit, True, False)) Then
                XtraMessageBox.Show("Kode sudah dipakai.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
                txtKode.Focus()
                Return False
                Exit Function
            End If
            'If Not IsPOS AndAlso CekKodeValid(txtNoFakturPajak.Text, FakturPajakLama, "MJual", "NoFakturPajak", IIf(pTipe = pStatus.Edit, True, False)) Then
            '    XtraMessageBox.Show("No Faktur Pajak sudah dipakai.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
            '    txtNoFakturPajak.Focus()
            '    Return False
            '    Exit Function
            'End If
            'LimitHutang = NullToDbl(EksekusiSQlSkalarNew("Select sum(LimitPiutang) as x from MAlamat where NoID=" & NullToLong(txtKodeCustomer.EditValue)))
            'If Not IsPOS AndAlso LimitHutang > 0 Then
            '    Dim SaldoHutang As Double
            '    SaldoHutang = NullToDbl(EksekusiSQlSkalarNew("Select Sum(ISNULL(MJual.Sisa,0)-ISNULL(B.JumBayar,0)) as Saldo " & _
            '                                                 "from MJual left join (Select IDBeli,SUM(ISNULL(MBayarHutangD.Bayar,0))+SUM(ISNULL(MBayarHutangD.Retur,0)) as JumBayar from MBayarHutangD Group by IDBeli ) B " & _
            '                                                 "On MJual.NoID=B.IDBeli where MJual.NoID<>" & NoID & " and MJual.IDCustomer=" & NullToLong(txtKodeCustomer.EditValue)))
            '    If SaldoHutang + txtSisa.EditValue > LimitHutang Then
            '        MsgBox("Hutang Customer ini telah melebihi limit!" & vbCrLf & "Limit Hutang:" & Format(LimitHutang, "#.##0") & vbCrLf & _
            '                "Saldo Hutang sebelumnya:" & Format(SaldoHutang, "#,##0") & vbCrLf & _
            '                "Saldo Hutang dengan Nota Ini:" & Format(SaldoHutang + txtSisa.EditValue, "#,##0"), MsgBoxStyle.Information)
            '        Return False
            '        Exit Function
            '    End If
            'End If
            'If pTipe = pStatus.Edit AndAlso txtPacking.Text = "" Then
            '    If XtraMessageBox.Show("Item Packing masih kosong." & vbCrLf & "Ingin melanjutkan penyimpanan?", NamaAplikasi, MessageBoxButtons.YesNo, MessageBoxIcon.Information) = Windows.Forms.DialogResult.No Then
            '        txtPacking.Focus()
            '        Return False
            '        Exit Function
            '    End If
            'End If
            'If txtSisa.EditValue <> 0 Then
            '    XtraMessageBox.Show("Jumlah Pembayaran Belum Sesuai.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
            '    txtBayar.Focus()
            '    Return False
            '    Exit Function
            'End If
            'If txtBayar.EditValue < 0 Then
            '    Dim x As New frmOtorisasiAdmin
            '    If Not x.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
            '        x.Dispose()
            '        Return False
            '        Exit Function
            '    Else
            '        x.Dispose()
            '    End If
            'End If
            If pTipe = pStatus.Edit AndAlso GV1.RowCount <= 0 Then
                XtraMessageBox.Show("Item detil masih kosong." & vbCrLf & "Isi item detil atau tutup bila ingin membatailkan.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
                Return False
                Exit Function
            End If
            'If pTipe = pStatus.Edit Then
            '    For i As Integer = 0 To GV1.RowCount - 1
            '        SisaQty = NullToDbl(EksekusiSQlSkalarNew("SELECT SUM((QtyMasuk-QtyKeluar)*Konversi) FROM MKartuStok WHERE IDBarang=" & NullToLong(GV1.GetRowCellValue(i, "IDBarang")) & " AND IDGudang=" & NullToLong(GV1.GetRowCellValue(i, "IDGUdang"))))
            '        If SisaQty < 0 Then
            '            If XtraMessageBox.Show("Qty " & GV1.GetRowCellValue(i, "NamaStock") & " melebihi sisa stock digudang " & NullToStr(GV1.GetRowCellValue(i, "Gudang")) & "." & vbCrLf & "Yakin ingin melanjutkan penyimpanan?", NamaAplikasi, MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) = Windows.Forms.DialogResult.No Then
            '                Return False
            '                Exit Function
            '            End If
            '        End If
            '    Next
            '    SQL = "SELECT MPackingD.Qty*MPackingD.Konversi-IsNull((SELECT SUM(MJualD.Qty*MjualD.Konversi) FROM MJualD WHERE MJualD.IDJual=" & NoID & " AND MJualD.IDPackingD=MPackingD.NoID),0) AS QtySisa " & _
            '          " FROM MPackingD INNER JOIN MPacking ON MPacking.NoID=MPackingD.IDPacking " & _
            '          " WHERE  MPacking.NoID IN (SELECT MPackingD.IDPacking FROM MJualD LEFT JOIN MPackingD ON MPackingD.NoID=MJualD.IDPackingD WHERE MJualD.IDJual=" & NoID & ")"
            '    ds = ExecuteDataset("MJual", SQL)
            '    If ds.Tables("MJual").Rows.Count >= 1 Then
            '        For i As Integer = 0 To ds.Tables("MJual").Rows.Count - 1
            '            If NullToDbl(ds.Tables("MJual").Rows(i).Item("QtySisa")) > 1 Then
            '                XtraMessageBox.Show("Qty penjualan tidak sama dengan Qty Packing", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
            '                Return False
            '                Exit Function
            '            End If
            '        Next
            '    End If
            'End If
            Return True
        Catch ex As Exception
            Return False
        Finally
            ds.Dispose()
        End Try
    End Function
    Public Function Simpan(ByRef PesanSalah As String) As Boolean
        Dim Sukses As Boolean = False
        HitungTotal()
        Dim PPN As Double = 0, DPP As Double = 0
        Dim DS As New DataSet
        Dim xBayar As New frmBayar
        Dim ChargeProsen As Double = 0.0, Temp As Double = 0.0
        Try
            If pTipe = pStatus.Baru Then
                NoID = GetNewID("MJual")
                SQL = "INSERT INTO MJual (IDBagPembelian,IDBagPenjualan,IDWilayah,IsCash,NoID,Kode,KodeReff,Tanggal,TanggalStock,JatuhTempo,"
                SQL &= " IDCustomer,TanggalSJ,NoSJ,SubTotal,DiskonNotaProsen,DiskonNotaRp,DiskonNotaTotal,"
                SQL &= " Biaya, Total, Bayar, Sisa,PPN,NilaiPPN,DPP,IDAdmin,IDPacking,Keterangan,NoFakturPajak,IDTypePajak,IsPOS,IDPOS,Kas,IsPending) VALUES (" & vbCrLf
                SQL &= NullTolInt(txtKodeSalesman.EditValue) & "," & IDUserAktif & "," & DefIDWilayah & ","
                SQL &= IIf(ckCash.Checked, 1, 0) & ","
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
                SQL &= FixKoma(txtPPNProsen.EditValue) & ","
                SQL &= FixKoma(txtPPN.EditValue) & ","
                SQL &= FixKoma(txtDPP.EditValue) & ","
                SQL &= IDAdmin & ","
                SQL &= NullToLong(txtPacking.EditValue) & ",'"
                SQL &= FixApostropi(txtKeterangan.Text) & "', '" & FixApostropi(txtNoFakturPajak.Text) & "'," & NullToLong(0) & "," & IIf(IsPOS, 1, 0) & ", " & defIDKassa & "," & FixKoma(txtTotal.EditValue) & ",0 )"
                If EksekusiSQL(SQL) Then
                    Sukses = True
                Else
                    Sukses = False
                End If
            Else
                xBayar.txtSubtotal.EditValue = txtTotal.EditValue
                If xBayar.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
                    'For i = 0 To GV1.RowCount
                    '    temp = temp + NullToDbl(GV1.GetRowCellValue(i, "Jumlah"))
                    'Next
                    'txtSubtotal.EditValue = temp
                    'txtDiscTotal.EditValue = (NullToDbl(txtDiscPersen.EditValue) * NullToDbl(txtSubtotal.EditValue) / 100) '+ txtDiscRp.EditValue
                    'txtSubtotal2.EditValue = txtSubtotal.EditValue - txtDiscTotal.EditValue
                    'txtTotal.EditValue = txtSubtotal2.EditValue + txtBiaya.EditValue
                    'If ckCash.Checked Then
                    '    txtBayar.EditValue = txtTotal.EditValue
                    'End If
                    txtBayar.EditValue = xBayar.txtTunai.EditValue + Bulatkan((xBayar.txtSubtotal.EditValue + xBayar.txtRounding.EditValue - xBayar.txtTunai.EditValue) * (1 + (xBayar.txtChargeCC.EditValue / 100)) * (1 + (xBayar.txtChargeDC.EditValue / 100)), 0)
                    HitungTotal()
                    txtTotal.EditValue = txtSubtotal2.EditValue + xBayar.txtRounding.EditValue + txtBiaya.EditValue
                    txtSisa.EditValue = Bulatkan((txtTotal.EditValue * (1 + (xBayar.txtChargeCC.EditValue / 100)) * (1 + (xBayar.txtChargeDC.EditValue / 100))), 0) - txtBayar.EditValue
                    'txtTotalCTN.EditValue = NullToDbl(EksekusiSQlSkalarNew("SELECT SUM(Ctn) FROM MJualD WHERE IDJual=" & NoID))

                    SQL = "UPDATE MJual SET "
                    If Not Tgl.Properties.ReadOnly Or Not txtKode.Properties.ReadOnly Then
                        SQL &= " IDAdmin=" & IDAdmin & ","
                    End If
                    SQL &= " IDBagPenjualan=" & IDUserAktif & ","
                    SQL &= " IDBagPembelian=" & NullTolInt(txtKodeSalesman.EditValue) & ","
                    SQL &= " IDWilayah=" & DefIDWilayah & ","
                    SQL &= " IDPacking=" & NullToLong(txtPacking.EditValue) & ","
                    SQL &= " IsCash=" & IIf(ckCash.Checked, 1, 0) & ","
                    SQL &= " IsPOS=" & IIf(IsPOS, 1, 0) & ","
                    SQL &= " Kode='" & FixApostropi(txtKode.Text) & "',"
                    SQL &= " NoFakturPajak='" & FixApostropi(txtNoFakturPajak.Text) & "',"
                    SQL &= " IDTypePajak=" & NullToLong(0) & ","
                    SQL &= " KodeReff='" & FixApostropi(txtKodeReff.Text) & "',"
                    SQL &= " Tanggal='" & Tgl.DateTime.ToString("yyyy-MM-dd HH:mm") & "',"
                    SQL &= " TanggalStock='" & tglStok.DateTime.ToString("yyyy-MM-dd HH:mm") & "',"
                    SQL &= " JatuhTempo='" & tglJatuhTempo.DateTime.ToString("yyyy-MM-dd HH:mm") & "',"
                    SQL &= " IDCustomer=" & txtKodeCustomer.EditValue & ","
                    SQL &= " TanggalSJ='" & tglSJ.DateTime.ToString("yyyy-MM-dd HH:mm") & "',"
                    SQL &= " NoSJ='" & FixApostropi(txtNoSJ.Text) & "',"
                    SQL &= " Keterangan='" & FixApostropi(txtKeterangan.Text) & "',"
                    SQL &= " SubTotal=" & FixKoma(txtSubtotal.EditValue) & ","
                    SQL &= " DiskonNotaProsen=" & FixKoma(txtDiscPersen.EditValue) & ","
                    SQL &= " DiskonNotaRp=" & FixKoma(txtDiscRp.EditValue) & ","
                    SQL &= " DiskonNotaTotal=" & FixKoma(txtDiscTotal.EditValue) & ","
                    SQL &= " Biaya=" & FixKoma(txtBiaya.EditValue) & ","
                    SQL &= " Total=" & FixKoma(txtTotal.EditValue) & ","
                    SQL &= " Bayar=" & FixKoma(txtBayar.EditValue) & ","
                    SQL &= " Pembulatan=" & FixKoma(xBayar.txtRounding.EditValue) & ","
                    SQL &= " Kas=" & FixKoma(IIf(xBayar.txtSubtotal.EditValue + xBayar.txtRounding.EditValue - xBayar.txtTunai.EditValue = 0, xBayar.txtTunai.EditValue, xBayar.txtTunai.EditValue - (xBayar.txtSubtotal.EditValue + xBayar.txtRounding.EditValue - xBayar.txtCC.EditValue - xBayar.txtDC.EditValue))) & ","
                    SQL &= " xBayar=" & FixKoma(xBayar.txtTunai.EditValue) & ","
                    SQL &= " xKembali=" & FixKoma(xBayar.txtKembali.EditValue) & ","
                    If xBayar.txtBankCC.Text <> "" AndAlso xBayar.txtBankDC.Text = "" Then 'CC
                        ChargeProsen = NullToDbl(EksekusiSQlSkalarNew("SELECT Charge FROM MJenisKartu WHERE NoID=" & NullToLong(xBayar.txtBankCC.EditValue)))
                        SQL &= " Bank=" & FixKoma(xBayar.txtCC.EditValue * (1 + (ChargeProsen / 100))) & ","
                        SQL &= " IDBank=" & NullToLong(xBayar.txtBankCC.EditValue) & ","
                        SQL &= " Charge=" & FixKoma(xBayar.txtCC.EditValue * (ChargeProsen / 100)) & ","
                    ElseIf xBayar.txtBankCC.Text = "" AndAlso xBayar.txtBankDC.Text <> "" Then 'DC
                        ChargeProsen = NullToDbl(EksekusiSQlSkalarNew("SELECT Charge FROM MJenisKartu WHERE NoID=" & NullToLong(xBayar.txtBankDC.EditValue)))
                        SQL &= " Bank=" & FixKoma(xBayar.txtDC.EditValue * (1 + (ChargeProsen / 100))) & ","
                        SQL &= " IDBank=" & NullToLong(xBayar.txtBankDC.EditValue) & ","
                        SQL &= " Charge=" & FixKoma(xBayar.txtDC.EditValue * (ChargeProsen / 100)) & ","
                    Else
                        SQL &= " Bank=0,"
                        SQL &= " IDBank=NULL,"
                        SQL &= " Charge=0,"
                    End If
                    SQL &= " NilaiPPN=" & FixKoma(txtPPN.EditValue) & ","
                    SQL &= " PPN=" & FixKoma(txtPPNProsen.EditValue) & ","
                    SQL &= " DPP=" & FixKoma(txtDPP.EditValue) & ","
                    SQL &= " IsPending=0,"
                    SQL &= " Sisa=" & FixKoma(txtSisa.EditValue)
                    SQL &= " WHERE NoID=" & NoID
                    If EksekusiSQL(SQL) Then
                        Sukses = True
                    Else
                        Sukses = False
                    End If
                Else
                    Sukses = False
                End If
            End If
        Catch ex As Exception
            PesanSalah = ex.Message
            Sukses = False
        Finally
            xBayar.Dispose()
        End Try
        Return Sukses
    End Function

    Private Sub BarButtonItem3_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles mnSaveLayOuts.ItemClick
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
                GVSalesman.SaveLayoutToXml(FolderLayouts & Me.Name & GVSalesman.Name & ".xml")
                gvPacking.SaveLayoutToXml(FolderLayouts & Me.Name & gvPacking.Name & ".xml")
                'GVPajak.SaveLayoutToXml(FolderLayouts & Me.Name & GVPajak.Name & ".xml")
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
            If XtraMessageBox.Show("Item ini mau dihapus?", NamaAplikasi, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) = Windows.Forms.DialogResult.Yes Then
                EksekusiSQL("Delete From MJualD where NoID=" & IDDetil.ToString)
                RefreshDetil()
            End If
        Catch ex As Exception
            XtraMessageBox.Show("Untuk mengedit pilih item satuan yang akan diedit terlebih dahulu lalu tekan tombol edit", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub txtKodeCustomer_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtKodeCustomer.EditValueChanged
        Dim TglAdd As Long = 0
        Try
            txtNamaCustomer.Text = NullToStr(EksekusiSQlSkalarNew("SELECT Nama FROM MAlamat WHERE NoID=" & NullToLong(txtKodeCustomer.EditValue)))
            txtAlamatCustomer.Text = NullToStr(EksekusiSQlSkalarNew("SELECT Alamat FROM MAlamat WHERE NoID=" & NullToLong(txtKodeCustomer.EditValue)))
            TglAdd = NullToLong(EksekusiSQlSkalarNew("SELECT JatuhTempoCustomer FROM MAlamat WHERE NoID=" & NullToLong(txtKodeCustomer.EditValue)))
            If TglAdd = 0 Then
                tglJatuhTempo.DateTime = DateAdd(DateInterval.Month, 2, Tgl.DateTime)
            Else
                tglJatuhTempo.DateTime = DateAdd(DateInterval.Day, TglAdd, Tgl.DateTime)
            End If
            RefreshDataPacking()
        Catch ex As Exception

        End Try
    End Sub

    Private Sub Tgl_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Tgl.EditValueChanged
        Dim TglAdd As Long = 0
        Try
            'txtNamaCustomer.Text = NullTostr(SearchLookUpEdit1View.GetRowCellValue(SearchLookUpEdit1View.FocusedRowHandle, "Nama"))
            TglAdd = NullToLong(EksekusiSQlSkalarNew("SELECT JatuhTempoCustomer FROM MAlamat WHERE NoID=" & NullToLong(txtKodeCustomer.EditValue)))
            If TglAdd = 0 Then
                tglJatuhTempo.DateTime = DateAdd(DateInterval.Month, 2, Tgl.DateTime)
            Else
                tglJatuhTempo.DateTime = DateAdd(DateInterval.Day, TglAdd, Tgl.DateTime)
            End If
            If pTipe = pStatus.Baru Then
                If ckCash.Checked Then
                    txtKode.Text = clsKode.MintaKodeBaru("JLT", "MJual", Tgl.DateTime, DefIDWilayah, 5, " MJual.IsCash=1 ")
                Else
                    If IsPOS Then
                        txtKode.Text = clsKode.MintaKodeBaru(defIDKassa.ToString("00"), "MJual", Tgl.DateTime, DefIDWilayah, 5, " IsNull(MJual.IsCash,0)=0 AND IsNull(MJual.IsPOS,0)=1 AND IsNull(MJual.IDPOS,0)= " & defIDKassa)
                    Else
                        txtKode.Text = clsKode.MintaKodeBaru("JL", "MJual", Tgl.DateTime, DefIDWilayah, 5, " (MJual.IsCash=0 OR MJual.IsCash IS NULL) ")
                    End If
                End If
                txtNoFakturPajak.Text = "000.000-00." & clsKode.MintaKodeBaruFP("MJual", "NoFakturPajak", Tgl.DateTime, 12, 8)
            End If
        Catch ex As Exception

        End Try
    End Sub

    Private Sub txtSubtotal_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtSubtotal.EditValueChanged

    End Sub

    Private Sub txtDiscPersen_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtDiscPersen.EditValueChanged
        'txtDiscRp.EditValue = txtSubtotal.EditValue * txtDiscPersen.EditValue / 100
        HitungTotal()
    End Sub

    Private Sub txtSubtotal_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtSubtotal.LostFocus
        HitungTotal()
    End Sub

    Private Sub txtDiscPersen_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtDiscPersen.LostFocus
        'txtDiscRp.EditValue = txtSubtotal.EditValue * txtDiscPersen.EditValue / 100
        HitungTotal()
    End Sub

    Private Sub txtDiscRp_EditValueChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtDiscRp.EditValueChanged
        'txtDiscPersen.EditValue = txtDiscRp.EditValue / IIf(txtSubtotal.EditValue = 0, 1, txtSubtotal.EditValue) * 100
        HitungTotal()
    End Sub

    Private Sub txtDiscRp_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtDiscRp.LostFocus
        'txtDiscPersen.EditValue = txtDiscRp.EditValue / IIf(txtSubtotal.EditValue = 0, 1, txtSubtotal.EditValue) * 100
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

    Private Sub txtBarang_ButtonClick(ByVal sender As Object, ByVal e As DevExpress.XtraEditors.Controls.ButtonPressedEventArgs)
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
    Private Function CariBarang(ByRef IDBarangD As Long, ByRef IDBarang As Long, ByRef NamaBarang As String, ByRef KodeBarang As String, ByRef Barcode As String, ByRef Konversi As Double, ByRef IDSatuan As Integer, ByRef HargaJual As Double) As Boolean
        Dim x As Boolean = False
        Dim SQL As String = ""
        Dim oDS As New DataSet
        Try
            SQL = "SELECT MBarangD.NoID, MBarangD.IDBarang, MBarangD.IDSatuan,MBarangD.Konversi,MBarangD.HargaJual,MBarangD.Barcode,MBarang.Kode,MBarang.Nama FROM MBarangD inner join MBarang  on MBarangD.IDBarang=Mbarang.NoID WHERE MBarang.IsActive=1 and MBarangD.IsActive=1 and (MBarangD.Barcode = '" & NamaBarang.Replace("'", "''").ToUpper & "' OR MBarang.Nama LIKE '" & NamaBarang.Replace("'", "''").ToUpper & "%' OR MBarang.Kode = '" & NamaBarang.Replace("'", "''").ToUpper & "') ORDER BY MBarang.Kode,MBarangD.Konversi ASC"
            oDS = ExecuteDataset("Tbl", SQL)
            If oDS.Tables("Tbl").Rows.Count >= 1 Then
                NamaBarang = NullToStr(oDS.Tables(0).Rows(0).Item("Nama"))
                KodeBarang = NullToStr(oDS.Tables(0).Rows(0).Item("Kode"))
                IDBarangD = NullToLong(oDS.Tables(0).Rows(0).Item("NoID"))
                IDBarang = NullToLong(oDS.Tables(0).Rows(0).Item("IDBarang"))
                IDSatuan = NullToLong(oDS.Tables(0).Rows(0).Item("IDSatuan"))
                Konversi = NullToDbl(oDS.Tables(0).Rows(0).Item("Konversi"))
                HargaJual = NullToDbl(oDS.Tables(0).Rows(0).Item("HargaJual"))
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
    'Private Function CariBarang(ByRef IDBarang As Long, ByRef NamaBarang As String, ByRef KodeBarang As String) As Boolean
    '    Dim x As Boolean = False
    '    Dim SQL As String = ""
    '    Dim oDS As New DataSet
    '    Try
    '        SQL = "SELECT NoID,Kode,Nama FROM MBarang WHERE IsActive=1 AND (ISNULL(KODE,'')<>'') AND (UPPER(Kode) = '" & NamaBarang.Replace("'", "''").ToUpper & "' OR UPPER(NAMA) = '" & NamaBarang.Replace("'", "''").ToUpper & "') ORDER BY Kode"
    '        oDS = ExecuteDataset("Tbl", SQL)
    '        If oDS.Tables("Tbl").Rows.Count >= 1 Then
    '            NamaBarang = NullToStr(oDS.Tables(0).Rows(0).Item("Nama"))
    '            KodeBarang = NullToStr(oDS.Tables(0).Rows(0).Item("Kode"))
    '            IDBarang = NullToLong(oDS.Tables(0).Rows(0).Item("NoID"))
    '            x = True
    '        Else
    '            x = False
    '        End If
    '    Catch ex As Exception
    '        x = False
    '    End Try
    '    Return x
    'End Function

    Private Sub InsertIntoDetil()
        'Dim SQL As String = ""
        'Dim NamaBarang As String = txtBarang.Text
        'Dim KodeBarang As String = ""
        'Dim IDBarang As Long = -1
        'Dim IDBarangD As Long = -1
        'Dim IDSatuan As Long = -1
        'Dim HargaJual As Double = 0.0
        'Dim Barcode As String = ""
        'Dim IDDetil As Long = -1
        'Dim frmEntri As New frmEntriJualDPOS
        'Dim Konversi As Double = 0.0
        'Try
        '    If CariBarang(IDBarangD, IDBarang, NamaBarang, KodeBarang, Barcode, Konversi, IDSatuan, HargaJual) Then
        '        frmEntri.IsNew = True
        '        frmEntri.NoID = IDDetil
        '        frmEntri.IDJual = NoID
        '        frmEntri.IDCustomer = NullToLong(txtKodeCustomer.EditValue)
        '        frmEntri.IDPacking = NullToLong(txtPacking.EditValue)
        '        frmEntri.txtBarcode.EditValue = NullToLong(IDBarangD)
        '        frmEntri.txtBarang.EditValue = NullToLong(IDBarang)
        '        If frmEntri.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
        '            RefreshDetil()
        '            GV1.ClearSelection()
        '            GV1.FocusedRowHandle = GV1.LocateByDisplayText(0, GV1.Columns("NoID"), frmEntri.NoID.ToString("##0"))
        '            GV1.SelectRow(GV1.FocusedRowHandle)
        '        End If
        '    End If
        'Catch ex As Exception
        '    XtraMessageBox.Show("Kesalahan : " & ex.Message, "Fast Entri Says", MessageBoxButtons.OK)
        'Finally
        '    frmEntri.Dispose()
        'End Try
    End Sub

    Private Sub cmdBAru_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdBAru.Click
        'Dim frmEntri As New frmEntriJualDPOS
        'Try
        '    Dim IDDetil As Long = -1
        '    frmEntri.NoID = IDDetil
        '    frmEntri.IsNew = True
        '    frmEntri.IDJual = NoID
        '    If txtKodeCustomer.Text <> "" Then
        '        frmEntri.IDCustomer = NullToLong(txtKodeCustomer.EditValue)
        '        frmEntri.IDPacking = NullToLong(txtPacking.EditValue)
        '        'frmEntri.IDGudang = NullToLong(txtGudang.EditValue)
        '        frmEntri.NoID = IDDetil
        '        frmEntri.IsNew = True
        '        If frmEntri.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
        '            RefreshDetil()
        '            GV1.ClearSelection()
        '            GV1.FocusedRowHandle = GV1.LocateByDisplayText(0, GV1.Columns("NoID"), frmEntri.NoID.ToString("##0"))
        '            GV1.SelectRow(GV1.FocusedRowHandle)
        '        End If
        '    Else
        '        XtraMessageBox.Show("Isi dulu Customer, lalu click baru.", NamaAplikasi, MessageBoxButtons.OK)
        '    End If
        'Catch ex As Exception
        '    XtraMessageBox.Show("Untuk membuat baru tekan tombol baru", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        'Finally
        '    frmEntri.Dispose()
        'End Try
    End Sub

    Private Sub cmdEdit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdEdit.Click
        'Dim view As ColumnView = GC1.FocusedView
        'Dim frmEntri As New frmEntriJualDPOS
        'Try
        '    Dim row As System.Data.DataRow = view.GetDataRow(GV1.FocusedRowHandle)
        '    Dim dc As Integer = GV1.FocusedRowHandle
        '    Dim IDDetil As Long = NullToLong(row("NoID"))
        '    If txtKodeCustomer.Text <> "" Then
        '        frmEntri.IDCustomer = NullToLong(txtKodeCustomer.EditValue)
        '        frmEntri.IDPacking = NullToLong(txtPacking.EditValue)
        '        'frmEntri.IDGudang = NullToLong(txtGudang.EditValue)
        '        frmEntri.IsNew = False
        '        frmEntri.NoID = IDDetil
        '        If frmEntri.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
        '            RefreshDetil()
        '            GV1.ClearSelection()
        '            GV1.FocusedRowHandle = GV1.LocateByDisplayText(0, GV1.Columns("NoID"), frmEntri.NoID.ToString("##0"))
        '            GV1.SelectRow(GV1.FocusedRowHandle)
        '        End If
        '    Else
        '        XtraMessageBox.Show("Isi dulu Customer, lalu click Edit.", NamaAplikasi, MessageBoxButtons.OK)
        '    End If
        'Catch ex As Exception
        '    XtraMessageBox.Show("Untuk mengedit pilih item satuan yang akan diedit terlebih dahulu lalu tekan tombol edit", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        'Finally
        '    frmEntri.Dispose()
        'End Try
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
    Dim IDAdmin As Long
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

    Private Sub txtPacking_ButtonClick(ByVal sender As Object, ByVal e As DevExpress.XtraEditors.Controls.ButtonPressedEventArgs) Handles txtPacking.ButtonClick
        If e.Button.Index = 1 Then
            InsertPacking()
        End If
    End Sub

    Private Sub txtPacking_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtPacking.EditValueChanged

    End Sub
    Private Sub InsertPacking()
        Dim Ds As New DataSet
        Dim Harga As Double = 0.0
        Dim Disc1 As Double = 0.0
        Dim Disc2 As Double = 0.0
        Dim Disc3 As Double = 0.0
        Dim DiscRp1 As Double = 0.0
        Dim DiscRp2 As Double = 0.0
        Dim DiscRp3 As Double = 0.0

        Dim Jumlah As Double = 0.0
        Try
            If pTipe = pStatus.Edit AndAlso IsPosted = False Then
                'If IDPackingOld <> NullToLong(txtPacking.EditValue) AndAlso XtraMessageBox.Show("Bersihkan item penjualan dan ambil data dari Packing?", NamaAplikasi, MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                '    EksekusiSQL("DELETE FROM MJualD WHERE IDJual=" & NoID)
                'ElseIf IDPackingOld = NullToLong(txtPacking.EditValue) Then

                'Else
                '    txtPacking.EditValue = IDPackingOld
                '    Exit Sub
                'End If
                IDPackingOld = NullToLong(txtPacking.EditValue)
                SQL = "SELECT MPackingD.*, MSO.IDBagPenjualan AS IDSalesman, MSO.DiskonNotaProsen, MSOD.HargaPcs AS HargaPcsSO, MSOD.Harga AS HargaSO, MSOD.Disc1 AS Disc1SO, MSOD.Disc2 AS Disc2SO, MSOD.Disc3 AS Disc3SO, MSOD.DiscPersen1 AS DiscPersen1SO, MSOD.DiscPersen2 AS DiscPersen2SO, MSOD.DiscPersen3 AS DiscPersen3SO, MBarang.CtnPcs, MSatuanBase.NoID AS IDSatuanBase, MSatuanBase.Konversi AS KonversiBase, (MPackingD.Qty*MPackingD.Konversi)-IsNull((SELECT SUM(MJualD.Qty*MJualD.Konversi) FROM MJualD INNER JOIN MJual ON MJual.NoID=MJualD.IDJual WHERE MJualD.IDPackingD=MPackingD.NoID),0) AS SisaQty " & _
                              " FROM MPackingD " & _
                              " LEFT JOIN MBarang ON MBarang.NoID=MPackingD.IDBarang " & _
                              " LEFT JOIN ((MSPKD LEFT JOIN (MSOD INNER JOIN MSO ON MSO.NoID=MSOD.IDSO) ON MSOD.NoID=MSPKD.IDSOD) INNER JOIN MSPK ON MSPK.NoID=MSPKD.IDSPK) ON MSPKD.NoID=MPackingD.IDSPKD " & _
                              " LEFT JOIN (MSatuan LEFT JOIN MSatuan MSatuanBase ON MSatuanBase.NoID=MSatuan.IDBase) ON MSatuan.NoID=MPackingD.IDSatuan " & _
                              " WHERE MPackingD.IDPacking=" & NullToLong(txtPacking.EditValue) & _
                              " AND (MPackingD.Qty*MPackingD.Konversi)-IsNull((SELECT SUM(MJualD.Qty*MJualD.Konversi) FROM MJualD INNER JOIN MJual ON MJual.NoID=MJualD.IDJual WHERE MJualD.IDPackingD=MPackingD.NoID),0)>0" & _
                              " ORDER BY MPackingD.NoPacking "
                Ds = ExecuteDataset("MPackingD", SQL)
                With Ds.Tables("MPackingD")
                    For i As Integer = 0 To .Rows.Count - 1
                        'txtQty.EditValue / IsiKarton * txtKonversi.EditValue
                        If NullToDbl(.Rows(i).Item("Konversi")) <= NullToDbl(.Rows(i).Item("SisaQty")) Then
                            Harga = NullToDbl(.Rows(i).Item("Harga")) / NullToDbl(.Rows(i).Item("Konversi"))
                            Disc1 = NullToDbl(.Rows(i).Item("DiscPersen1SO"))
                            Disc2 = NullToDbl(.Rows(i).Item("DiscPersen2SO"))
                            Disc3 = NullToDbl(.Rows(i).Item("DiscPersen3SO"))
                            DiscRp1 = NullToDbl(.Rows(i).Item("Disc1SO")) / NullToDbl(.Rows(i).Item("Konversi"))
                            DiscRp2 = NullToDbl(.Rows(i).Item("Disc2SO")) / NullToDbl(.Rows(i).Item("Konversi"))
                            DiscRp3 = NullToDbl(.Rows(i).Item("Disc3SO")) / NullToDbl(.Rows(i).Item("Konversi"))
                            If Harga <= 0 Then
                                'Harga = clsPostingPenjualan.HargaJual(NullToLong(txtKodeCustomer.EditValue), NullToLong(.Rows(i).Item("IDBarangD")), NullToDbl(.Rows(i).Item("Qty")) * NullToDbl(.Rows(i).Item("Konversi")), Disc1)
                                Harga = clsPostingPenjualan.HargaJual(NullToLong(.Rows(i).Item("IDBarangD")), NullToDbl(.Rows(i).Item("Qty")) * NullToDbl(.Rows(i).Item("Konversi")), Disc1)
                            End If
                            Jumlah = Bulatkan((Harga * (1 - (Disc1 / 100)) * (1 - (Disc2 / 100)) * (1 - (Disc3 / 100))), 0) - DiscRp1 - DiscRp2 - DiscRp3
                            Jumlah = (NullToDbl(.Rows(i).Item("SisaQty")) / NullToDbl(.Rows(i).Item("Konversi"))) * Jumlah
                            SQL = "INSERT INTO [MJualD] ([IDSalesman], [NoID] ,[Tgl] ,[Jam] ,[IDJual] ,[IDBarang] ,[IDSatuan] ,[IDGudang] ,[Ctn] ,[Qty] ,[Harga] ,[DiscPersen1] ,[DiscPersen2] ,[DiscPersen3] ,[Disc1] ,[Disc2] ,[Disc3] ,[Jumlah] ,[IDDOD] ,[IDAkunPersediaan] ,[Catatan] ,[NoUrut] ,[HargaPcs] ,[QtyPcs] ,[IDAsal] ,[Konversi] ,[KodeGolongan] ,[HargaAsal] ,[RupiahAsal] ,[Selisih] ,[SelisihGlobal] ,[HargaPokok] ,[KodeJenis] ,[IDServerAsal] ,[IDPackingD])" & _
                                  " VALUES (" & NullToLong(.Rows(i).Item("IDSalesman")) & "," & NullToLong(GetNewID("MJualD", "NoID")) & " ,getdate() ,getdate() ," & NoID & " ," & NullToLong(.Rows(i).Item("IDBarang")) & " ," & NullToLong(.Rows(i).Item("IDSatuan")) & " ," & NullToLong(.Rows(i).Item("IDGudang")) & " ," & FixKoma(IIf(NullToDbl(.Rows(i).Item("CtnPcs")) = 0, 0, NullToDbl(.Rows(i).Item("SisaQty")) / NullToDbl(.Rows(i).Item("CtnPcs")))) & " ," & FixKoma(NullToDbl(.Rows(i).Item("SisaQty")) / IIf(NullToDbl(.Rows(i).Item("Konversi")) = 0, 1, NullToDbl(.Rows(i).Item("Konversi")))) & " ," & FixKoma(Harga) & " ," & FixKoma(Disc1) & _
                                  " ," & FixKoma(Disc2) & " ," & FixKoma(Disc3) & " ," & FixKoma(DiscRp1) & " ," & FixKoma(DiscRp2) & " ," & FixKoma(DiscRp3) & " ," & FixKoma(Jumlah) & " ,-1 ,-1 ,'" & FixApostropi(NullToStr(.Rows(i).Item("Catatan"))) & "' ," & _
                                  NullToLong(GetNewID("MJualD", "NoUrut", " WHERE MJualD.IDJual=" & NoID)) & " ," & FixKoma(Harga / IIf(NullToDbl(.Rows(i).Item("Konversi")) = 0, 1, NullToDbl(.Rows(i).Item("Konversi")))) & " ," & FixKoma(NullToDbl(.Rows(i).Item("SisaQty"))) & " ," & FixKoma(NullToDbl(.Rows(i).Item("IDAsal"))) & " ," & FixKoma(NullToDbl(.Rows(i).Item("Konversi"))) & " ,NULL ,NULL ,NULL ,NULL ,NULL ,NULL ,NULL ,NULL ," & NullToLong(.Rows(i).Item("NoID")) & ")"
                        Else
                            Harga = NullToDbl(.Rows(i).Item("Harga")) / NullToDbl(.Rows(i).Item("KonversiBase"))
                            Disc1 = NullToDbl(.Rows(i).Item("DiscPersen1SO"))
                            Disc2 = NullToDbl(.Rows(i).Item("DiscPersen2SO"))
                            Disc3 = NullToDbl(.Rows(i).Item("DiscPersen3SO"))
                            DiscRp1 = NullToDbl(.Rows(i).Item("Disc1SO")) / NullToDbl(.Rows(i).Item("KonversiBase"))
                            DiscRp2 = NullToDbl(.Rows(i).Item("Disc2SO")) / NullToDbl(.Rows(i).Item("KonversiBase"))
                            DiscRp3 = NullToDbl(.Rows(i).Item("Disc3SO")) / NullToDbl(.Rows(i).Item("KonversiBase"))
                            If Harga <= 0 Then
                                'Harga = clsPostingPenjualan.HargaJual(NullToLong(txtKodeCustomer.EditValue), NullToLong(.Rows(i).Item("IDBarangD")), NullToDbl(.Rows(i).Item("Qty")) * NullToDbl(.Rows(i).Item("KonversiBase")), Disc1)
                                Harga = clsPostingPenjualan.HargaJual(NullToLong(.Rows(i).Item("IDBarangD")), NullToDbl(.Rows(i).Item("Qty")) * NullToDbl(.Rows(i).Item("Konversi")), Disc1)
                            End If
                            Jumlah = (Bulatkan((Harga * (1 - (Disc1 / 100)) * (1 - (Disc2 / 100)) * (1 - (Disc3 / 100))), 0) - DiscRp1 - DiscRp2 - DiscRp3)
                            Jumlah = (NullToDbl(.Rows(i).Item("SisaQty")) / NullToDbl(.Rows(i).Item("KonversiBase"))) * Jumlah
                            SQL = "INSERT INTO [MJualD] ([IDSalesman], [NoID] ,[Tgl] ,[Jam] ,[IDJual] ,[IDBarang] ,[IDSatuan] ,[IDGudang] ,[Ctn] ,[Qty] ,[Harga] ,[DiscPersen1] ,[DiscPersen2] ,[DiscPersen3] ,[Disc1] ,[Disc2] ,[Disc3] ,[Jumlah] ,[IDDOD] ,[IDAkunPersediaan] ,[Catatan] ,[NoUrut] ,[HargaPcs] ,[QtyPcs] ,[IDAsal] ,[Konversi] ,[KodeGolongan] ,[HargaAsal] ,[RupiahAsal] ,[Selisih] ,[SelisihGlobal] ,[HargaPokok] ,[KodeJenis] ,[IDServerAsal] ,[IDPackingD])" & _
                            " VALUES (" & NullToLong(.Rows(i).Item("IDSalesman")) & "," & NullToLong(GetNewID("MJualD", "NoID")) & " ,getdate() ,getdate() ," & NoID & " ," & NullToLong(.Rows(i).Item("IDBarang")) & " ," & NullToLong(.Rows(i).Item("IDSatuanBase")) & " ," & NullToLong(.Rows(i).Item("IDGudang")) & " ," & FixKoma(IIf(NullToDbl(.Rows(i).Item("CtnPcs")) = 0, 0, NullToDbl(.Rows(i).Item("SisaQty")) / NullToDbl(.Rows(i).Item("CtnPcs")))) & " ," & FixKoma(NullToDbl(.Rows(i).Item("SisaQty")) / IIf(NullToDbl(.Rows(i).Item("KonversiBase")) = 0, 1, NullToDbl(.Rows(i).Item("KonversiBase")))) & " ," & FixKoma(Harga) & " ," & FixKoma(Disc1) & _
                            " ," & FixKoma(Disc2) & " ," & FixKoma(Disc3) & " ," & FixKoma(DiscRp1) & " ," & FixKoma(DiscRp2) & " ," & FixKoma(DiscRp3) & " ," & FixKoma(Jumlah) & " ,-1 ,-1 ,'" & FixApostropi(NullToStr(.Rows(i).Item("Catatan"))) & "' ," & _
                            NullToLong(GetNewID("MJualD", "NoUrut", " WHERE MJualD.IDJual=" & NoID)) & " ," & FixKoma(Harga / IIf(NullToDbl(.Rows(i).Item("KonversiBase")) = 0, 1, NullToDbl(.Rows(i).Item("KonversiBase")))) & " ," & FixKoma(NullToDbl(.Rows(i).Item("SisaQty"))) & " ," & FixKoma(NullToDbl(.Rows(i).Item("IDAsal"))) & " ," & FixKoma(NullToDbl(.Rows(i).Item("KonversiBase"))) & " ,NULL ,NULL ,NULL ,NULL ,NULL ,NULL ,NULL ,NULL ," & NullToLong(.Rows(i).Item("NoID")) & ")"
                        End If
                        EksekusiSQL(SQL)
                    Next
                End With

                'SQL = "SELECT MPacking.NoID, MPackingD.IDBarang, MSO.DiskonNotaProsen, MPackingD.IDSatuan, MPackingD.Konversi, MBarang.CtnPcs, MSatuanBase.NoID AS IDSatuanBase, MSatuanBase.Konversi AS KonversiBase, SUM(MPackingD.Qty*MPackingD.Konversi) AS Qty," & vbCrLf & _
                '      " IsNull((SELECT SUM(MJualD.Qty*MJualD.Konversi) FROM MJualD INNER JOIN MJual ON MJual.NoID=MJualD.IDJual WHERE MJual.IDPacking=MPacking.NoID AND MJualD.IDBarang=MPackingD.IDBarang),0) AS DiJual, AVG(MSOD.DiscPersen1) AS DiscPersen1, AVG(MSOD.DiscPersen2) AS DiscPersen2, AVG(MSOD.DiscPersen3) AS DiscPersen3, AVG(MSOD.Disc1) AS Disc1, AVG(MSOD.Disc2) AS Disc2, AVG(MSOD.Disc3) AS Disc3, AVG(MSOD.Harga) AS Harga, AVG(MSOD.HargaPcs) AS HargaPcs" & vbCrLf & _
                '      " FROM MPackingD INNER JOIN MPacking ON MPacking.NoID=MPackingD.IDPacking " & vbCrLf & _
                '      " LEFT JOIN ((MSPKD LEFT JOIN (MSOD INNER JOIN MSO ON MSO.NoID=MSOD.IDSO) ON MSOD.NoID=MSPKD.IDSOD) INNER JOIN MSPK ON MSPK.NoID=MSPKD.IDSPK) ON MSPKD.NoID=MPackingD.IDSPKD " & vbCrLf & _
                '      " LEFT JOIN MBarang ON MBarang.NoID=MPackingD.IDBarang  " & vbCrLf & _
                '      " LEFT JOIN (MSatuan LEFT JOIN MSatuan MSatuanBase ON MSatuanBase.NoID=MSatuan.IDBase) ON MSatuan.NoID=MPackingD.IDSatuan  " & vbCrLf & _
                '      " WHERE MPackingD.IDGudang = " & NullToLong(txtGudang.EditValue) & " AND MPacking.NoID=" & NullToLong(txtPacking.EditValue) & vbCrLf & _
                '      " GROUP BY MSO.DiskonNotaProsen, MPacking.NoID, MPackingD.IDBarang, MPackingD.IDSatuan, MPackingD.Konversi, MBarang.CtnPcs, MSatuanBase.NoID, MPackingD.IDGudang, MSatuanBase.Konversi" & _
                '      " HAVING SUM(MPackingD.Qty*MPackingD.Konversi)-IsNull((SELECT SUM(MJualD.Qty*MJualD.Konversi) FROM MJualD INNER JOIN MJual ON MJual.NoID=MJualD.IDJual WHERE MJual.IDPacking=MPacking.NoID AND MJualD.IDBarang=MPackingD.IDBarang),0)>0"
                'Ds = ExecuteDataset("MPackingD", SQL)
                'With Ds.Tables("MPackingD")
                '    For i As Integer = 0 To .Rows.Count - 1
                '        'txtQty.EditValue / IsiKarton * txtKonversi.EditValue
                '        If NullToDbl(.Rows(i).Item("Konversi")) <= NullToDbl(.Rows(i).Item("Qty")) - NullToDbl(.Rows(i).Item("DiJual")) Then
                '            Harga = NullToDbl(.Rows(i).Item("Harga"))
                '            Harga = Harga - (Harga * NullToDbl(.Rows(i).Item("DiskonNotaProsen")) / 100)
                '            Disc1 = NullToDbl(.Rows(i).Item("DiscPersen1"))
                '            Disc2 = NullToDbl(.Rows(i).Item("DiscPersen2"))
                '            Disc3 = NullToDbl(.Rows(i).Item("DiscPersen3"))
                '            DiscRp1 = NullToDbl(.Rows(i).Item("Disc1"))
                '            DiscRp2 = NullToDbl(.Rows(i).Item("Disc2"))
                '            DiscRp3 = NullToDbl(.Rows(i).Item("Disc3"))
                '            If Harga <= 0 Then
                '                Harga = clsPostingPenjualan.HargaJual(NullToLong(.Rows(i).Item("IDBarang")), NullToLong(.Rows(i).Item("IDSatuan")), NullToLong(txtKodeCustomer.EditValue), Disc1, Disc2)
                '            End If
                '            Jumlah = ((NullToDbl(.Rows(i).Item("Qty")) - NullToDbl(.Rows(i).Item("DiJual"))) / NullToDbl(.Rows(i).Item("Konversi"))) * (Bulatkan((Harga * (1 - (Disc1 / 100)) * (1 - (Disc2 / 100)) * (1 - (Disc3 / 100))), 0) - DiscRp1 - DiscRp2 - DiscRp3)
                '            SQL = "INSERT INTO [MJualD] ([NoID] ,[Tgl] ,[Jam] ,[IDJual] ,[IDBarang] ,[IDSatuan] ,[IDGudang] ,[Ctn] ,[Qty] ,[Harga] ,[DiscPersen1] ,[DiscPersen2] ,[DiscPersen3] ,[Disc1] ,[Disc2] ,[Disc3] ,[Jumlah] ,[IDDOD] ,[IDAkunPersediaan] ,[Catatan] ,[NoUrut] ,[HargaPcs] ,[QtyPcs] ,[IDAsal] ,[Konversi] ,[KodeGolongan] ,[HargaAsal] ,[RupiahAsal] ,[Selisih] ,[SelisihGlobal] ,[HargaPokok] ,[KodeJenis] ,[IDServerAsal] ,[IDPackingD])" & _
                '            " VALUES (" & NullToLong(GetNewID("MJualD", "NoID")) & " ,getdate() ,getdate() ," & NoID & " ," & NullToLong(.Rows(i).Item("IDBarang")) & " ," & NullToLong(.Rows(i).Item("IDSatuan")) & " ," & NullToLong(txtGudang.EditValue) & " ," & FixKoma(IIf(NullToDbl(.Rows(i).Item("CtnPcs")) = 0, 0, (NullToDbl(.Rows(i).Item("Qty")) - NullToDbl(.Rows(i).Item("DiJual"))) / NullToDbl(.Rows(i).Item("CtnPcs")))) & " ," & FixKoma((NullToDbl(.Rows(i).Item("Qty")) - NullToDbl(.Rows(i).Item("DiJual"))) / IIf(NullToDbl(.Rows(i).Item("Konversi")) = 0, 1, NullToDbl(.Rows(i).Item("Konversi")))) & " ," & FixKoma(Harga) & " ," & FixKoma(Disc1) & _
                '            " ," & FixKoma(Disc2) & " ," & FixKoma(Disc3) & " ," & FixKoma(DiscRp1) & " ," & FixKoma(DiscRp2) & " ," & FixKoma(DiscRp3) & " ," & FixKoma(Jumlah) & " ,-1 ,-1 ,'' ," & _
                '            NullToLong(GetNewID("MJualD", "NoUrut", " WHERE MJualD.IDJual=" & NoID)) & " ," & FixKoma(Harga / IIf(NullToDbl(.Rows(i).Item("Konversi")) = 0, 1, NullToDbl(.Rows(i).Item("Konversi")))) & " ," & FixKoma((NullToDbl(.Rows(i).Item("Qty")) - NullToDbl(.Rows(i).Item("DiJual")))) & " ," & -1 & " ," & FixKoma(NullToDbl(.Rows(i).Item("Konversi"))) & " ,NULL ,NULL ,NULL ,NULL ,NULL ,NULL ,NULL ,NULL ," & -1 & ")"
                '        Else
                '            Harga = NullToDbl(.Rows(i).Item("Harga"))
                '            Harga = Harga - (Harga * NullToDbl(.Rows(i).Item("DiskonNotaProsen")) / 100)
                '            Disc1 = NullToDbl(.Rows(i).Item("DiscPersen1"))
                '            Disc2 = NullToDbl(.Rows(i).Item("DiscPersen2"))
                '            Disc3 = NullToDbl(.Rows(i).Item("DiscPersen3"))
                '            DiscRp1 = NullToDbl(.Rows(i).Item("Disc1"))
                '            DiscRp2 = NullToDbl(.Rows(i).Item("Disc2"))
                '            DiscRp3 = NullToDbl(.Rows(i).Item("Disc3"))
                '            If Harga <= 0 Then
                '                Harga = clsPostingPenjualan.HargaJual(NullToLong(.Rows(i).Item("IDBarang")), NullToLong(.Rows(i).Item("IDSatuanBase")), NullToLong(txtKodeCustomer.EditValue), Disc1, Disc2)
                '            End If
                '            Jumlah = ((NullToDbl(.Rows(i).Item("Qty")) - NullToDbl(.Rows(i).Item("DiJual"))) / NullToDbl(.Rows(i).Item("KonversiBase"))) * (Bulatkan((Harga * (1 - (Disc1 / 100)) * (1 - (Disc2 / 100)) * (1 - (Disc3 / 100))), 0) - DiscRp1 - DiscRp2 - DiscRp3)
                '            SQL = "INSERT INTO [MJualD] ([NoID] ,[Tgl] ,[Jam] ,[IDJual] ,[IDBarang] ,[IDSatuan] ,[IDGudang] ,[Ctn] ,[Qty] ,[Harga] ,[DiscPersen1] ,[DiscPersen2] ,[DiscPersen3] ,[Disc1] ,[Disc2] ,[Disc3] ,[Jumlah] ,[IDDOD] ,[IDAkunPersediaan] ,[Catatan] ,[NoUrut] ,[HargaPcs] ,[QtyPcs] ,[IDAsal] ,[Konversi] ,[KodeGolongan] ,[HargaAsal] ,[RupiahAsal] ,[Selisih] ,[SelisihGlobal] ,[HargaPokok] ,[KodeJenis] ,[IDServerAsal] ,[IDPackingD])" & _
                '            " VALUES (" & NullToLong(GetNewID("MJualD", "NoID")) & " ,getdate() ,getdate() ," & NoID & " ," & NullToLong(.Rows(i).Item("IDBarang")) & " ," & NullToLong(.Rows(i).Item("IDSatuanBase")) & " ," & NullToLong(txtGudang.EditValue) & " ," & FixKoma(IIf(NullToDbl(.Rows(i).Item("CtnPcs")) = 0, 0, (NullToDbl(.Rows(i).Item("Qty")) - NullToDbl(.Rows(i).Item("DiJual"))) / NullToDbl(.Rows(i).Item("CtnPcs")))) & " ," & FixKoma((NullToDbl(.Rows(i).Item("Qty")) - NullToDbl(.Rows(i).Item("DiJual"))) / IIf(NullToDbl(.Rows(i).Item("KonversiBase")) = 0, 1, NullToDbl(.Rows(i).Item("KonversiBase")))) & " ," & FixKoma(Harga) & " ," & FixKoma(Disc1) & _
                '            " ," & FixKoma(Disc2) & " ," & FixKoma(Disc3) & " ," & FixKoma(DiscRp1) & " ," & FixKoma(DiscRp2) & " ," & FixKoma(DiscRp3) & " ," & FixKoma(Jumlah) & " ,-1 ,-1 ,'' ," & _
                '            NullToLong(GetNewID("MJualD", "NoUrut", " WHERE MJualD.IDJual=" & NoID)) & " ," & FixKoma(Harga / IIf(NullToDbl(.Rows(i).Item("KonversiBase")) = 0, 1, NullToDbl(.Rows(i).Item("KonversiBase")))) & " ," & FixKoma((NullToDbl(.Rows(i).Item("Qty")) - NullToDbl(.Rows(i).Item("DiJual")))) & " ," & -1 & " ," & FixKoma(NullToDbl(.Rows(i).Item("KonversiBase"))) & " ,NULL ,NULL ,NULL ,NULL ,NULL ,NULL ,NULL ,NULL ," & -1 & ")"
                '        End If
                '        EksekusiSQL(SQL)
                '    Next
                'End With

                'SQL = "SELECT MSO.IDBagPenjualan" & _
                '      " FROM MPacking " & _
                '      " LEFT JOIN ((MSPKD LEFT JOIN (MSOD INNER JOIN MSO ON MSO.NoID=MSOD.IDSO) ON MSOD.NoID=MSPKD.IDSOD) INNER JOIN MSPK ON MSPK.NoID=MSPKD.IDSPK) ON MSPK.NoID=MPacking.IDSPK" & _
                '      " WHERE MPacking.NoID = " & NullToLong(txtPacking.EditValue)
                'txtSalesman.EditValue = NullToLong(EksekusiSQlSkalarNew(SQL))
                RefreshDetil()
            Else
                txtPacking.EditValue = IDPackingOld
            End If
        Catch ex As Exception
            XtraMessageBox.Show("Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            'RefreshLookUpSTB()
            Ds.Dispose()
        End Try
    End Sub
    Private Sub gvPacking_DataSourceChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvPacking.DataSourceChanged
        If System.IO.File.Exists(FolderLayouts & Me.Name & gvPacking.Name & ".xml") Then
            gvPacking.RestoreLayoutFromXml(FolderLayouts & Me.Name & gvPacking.Name & ".xml")
        End If
        With gvPacking
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
    Private Sub ckCash_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ckCash.CheckedChanged
        If pTipe = pStatus.Baru Then
            If ckCash.Checked Then
                txtKode.Text = clsKode.MintaKodeBaru("JLT", "MJual", Tgl.DateTime, DefIDWilayah, 5, " MJual.IsCash=1 ")
            Else
                If IsPOS Then
                    txtKode.Text = clsKode.MintaKodeBaru(defIDKassa.ToString("00"), "MJual", Tgl.DateTime, DefIDWilayah, 5, " IsNull(MJual.IsCash,0)=0 AND IsNull(MJual.IsPOS,0)=1 AND IsNull(MJual.IDPOS,0)= " & defIDKassa)
                Else
                    txtKode.Text = clsKode.MintaKodeBaru("JL", "MJual", Tgl.DateTime, DefIDWilayah, 5, " (MJual.IsCash=0 OR MJual.IsCash IS NULL) ")
                End If
            End If
            txtNoFakturPajak.Text = "000.000-00." & clsKode.MintaKodeBaruFP("MJual", "NoFakturPajak", Tgl.DateTime, 12, 8)
        End If
    End Sub
    'Private Sub gvGudang_DataSourceChanged(ByVal sender As Object, ByVal e As System.EventArgs)
    '    If System.IO.File.Exists(FolderLayouts & Me.Name & gvGudang.Name & ".xml") Then
    '        gvGudang.RestoreLayoutFromXml(FolderLayouts & Me.Name & gvGudang.Name & ".xml")
    '    End If
    '    With gvGudang
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

    'Private Sub txtGudang_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
    '    RubahGudang()
    'End Sub
    'Private Sub RubahGudang()
    '    RefreshDataPacking()
    '    txtWilayah.Text = NullToStr(EksekusiSQlSkalarNew("SELECT MWilayah.Nama FROM MGudang LEFT JOIN MWilayah ON MWilayah.NOID=MGudang.IDWilayah WHERE MGudang.NoID=" & NullToLong(txtGudang.EditValue)))
    'End Sub

    Private Sub txtPacking_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtPacking.KeyDown
        If e.KeyCode = Keys.Enter Then
            InsertPacking()
        End If
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
        txt.SelectAll()
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

    Private Sub txtBarang_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtBarang.EditValueChanged

    End Sub

    Private Sub tglJatuhTempo_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tglJatuhTempo.EditValueChanged

    End Sub

    Private Sub txtKode_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtKode.EditValueChanged

    End Sub

    Private Sub SimpleButton1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SimpleButton1.Click
        'EksekusiSQL("Update MJualD set PPN=Jumlah*" & FixKoma(txtPPNProsen.EditValue) & "/100 where IDJual=" & NoID)
        'RefreshDetil()
        HitungTotal()
    End Sub

    Private Sub txtTotal_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtTotal.EditValueChanged

    End Sub

    Private Sub txtPPNProsen_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtPPNProsen.EditValueChanged

    End Sub

    Private Sub txtPPNProsen_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtPPNProsen.KeyDown
        If e.KeyCode = Keys.Enter Then

            'If MsgBox("PPN diset menjadi " & txtPPNProsen.Text & "?", MsgBoxStyle.YesNo + MsgBoxStyle.Question, "Seting PPN") = MsgBoxResult.Yes Then
            'EksekusiSQL("Update MJualD set PPN=Jumlah*" & FixKoma(txtPPNProsen.EditValue) & "/100 where IDJual=" & NoID)
            'RefreshDetil()
            'HitungTotal()
            'End If

        End If
    End Sub

    Private Sub txtKodeSalesman_EditValueChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtKodeSalesman.EditValueChanged
        txtNamaSalesman.Text = NullToStr(EksekusiSQlSkalarNew("SELECT Nama FROM MAlamat WHERE NoID=" & NullToLong(txtKodeSalesman.EditValue)))

    End Sub

    'Private Sub GVPajak_DataSourceChanged(ByVal sender As Object, ByVal e As System.EventArgs)
    '    If System.IO.File.Exists(FolderLayouts & Me.Name & GVPajak.Name & ".xml") Then
    '        GVPajak.RestoreLayoutFromXml(FolderLayouts & Me.Name & GVPajak.Name & ".xml")
    '    End If
    '    With GVPajak
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

    Private Sub TypePajak_EditValueChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        HitungTotal()
    End Sub

    Private Sub cmdPending_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdPending.Click
        Try
            If pTipe = pStatus.Edit AndAlso Not IsPosted Then
                SQL = "UPDATE MJual SET IsPending=1 WHERE NoID=" & NoID
                EksekusiSQL(SQL)
                'Print Tidak Option

                pTipe = pStatus.Baru
                NoID = -1
                LoadData()
                txtKodeCustomer.Focus()
                cmdSave.Text = "&Baru"
            End If
        Catch ex As Exception

        End Try
    End Sub

    Private Sub mnPending_ItemClick(ByVal sender As Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles mnPending.ItemClick
        cmdPending.PerformClick()
    End Sub

    Private Sub txtPPN_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtPPN.EditValueChanged

    End Sub
End Class