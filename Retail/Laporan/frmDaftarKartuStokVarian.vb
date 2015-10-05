Imports System.Data.SqlClient
Imports System.Data
Imports System.Windows.Forms
Imports DevExpress.XtraEditors
Imports DevExpress.XtraGrid.Views
Imports DevExpress.XtraGrid.Columns
Imports DevExpress.XtraGrid.Views.Base
Imports System.Drawing
Imports DevExpress.XtraEditors.Repository
'Imports FastReport
'Imports FastReport.Utils

Public Class frmDaftarKartuStokVarian
    Dim oConn As SqlConnection = New SqlConnection(StrKonSql)
    Dim ocmd As SqlCommand = New SqlCommand()
    Dim frmImage As frmShowImage
    Dim ds As New DataSet
    Dim BS As New BindingSource
    Dim oDA As New SqlDataAdapter
    Dim repckedit As New RepositoryItemCheckEdit
    Dim reppicedit As New RepositoryItemPictureEdit
    Dim IsShow As Boolean = True
    Dim IDBarangD As Long = -1
    Private Sub ctrlDaftar_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        ocmd.Connection = oConn
        oConn.Open()
        RefreshLookUpBarang()
        RefreshLookUpGudang()
        TglDari.DateTime = CDate(DateAdd(DateInterval.Month, -3, TanggalSystem).ToString("yyyy/MM/01"))
        TglSampai.DateTime = TanggalSystem
        'txtGudang.EditValue = DefIDGudang
        RefreshData()
        IsShow = False
        RestoreLayout()
        Me.lbDaftar.Text = Me.Text
        FungsiControl.SetForm(Me)
    End Sub
    Private Sub RefreshLookUpBarang()
        Dim ds As New DataSet
        Dim SQL As String = ""
        Try
            SQL = "SELECT MBarangD.NoID, MBarangD.Barcode,MBarang.Kode, MBarang.Nama + ' ' + IsNull(MBarangD.Varian,'') AS Nama, MSatuan.Nama Satuan " & _
                  " FROM MBarang INNER JOIN MBarangD ON MBarang.NoID=MBarangD.IDBarang Left Join MSatuan On MBarangD.IDSatuan= MSatuan.NoID " & _
                  " WHERE MBarang.IsActive=1 AND MBarangD.IsActive=1"
            ds = ExecuteDataset("MBarang", SQL)
            txtBarang.Properties.DataSource = ds.Tables("MBarang")
            txtBarang.Properties.DisplayMember = "Kode"
            txtBarang.Properties.ValueMember = "NoID"
            If System.IO.File.Exists(folderLayouts & Me.Name & gvBarang.Name & ".xml") Then
                gvBarang.RestoreLayoutFromXml(folderLayouts & Me.Name & gvBarang.Name & ".xml")
            End If
        Catch ex As Exception
            XtraMessageBox.Show("Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            ds.Dispose()
        End Try
    End Sub
    Private Sub RestoreLayout()
        For i As Integer = 0 To GV1.Columns.Count - 1
            Select Case GV1.Columns(i).ColumnType.Name.ToLower
                Case "int32", "int64", "int"
                    GV1.Columns(i).DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
                    GV1.Columns(i).DisplayFormat.FormatString = "n0"
                Case "decimal", "single", "money", "double"
                    GV1.Columns(i).DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
                    GV1.Columns(i).DisplayFormat.FormatString = "n2"
                Case "string"
                    GV1.Columns(i).DisplayFormat.FormatType = DevExpress.Utils.FormatType.None
                    GV1.Columns(i).DisplayFormat.FormatString = ""
                Case "date", "datetime"
                    GV1.Columns(i).DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime
                    GV1.Columns(i).DisplayFormat.FormatString = "dd-MM-yyyy"
                Case "byte[]"
                    reppicedit.SizeMode = DevExpress.XtraEditors.Controls.PictureSizeMode.Squeeze
                    GV1.Columns(i).OptionsColumn.AllowGroup = False
                    GV1.Columns(i).OptionsColumn.AllowSort = False
                    GV1.Columns(i).OptionsFilter.AllowFilter = False
                    GV1.Columns(i).ColumnEdit = reppicedit
                Case "boolean"
                    GV1.Columns(i).ColumnEdit = repckedit

            End Select
            'If GV1.Columns(i).FieldName.Length >= 4 AndAlso GV1.Columns(i).FieldName.Substring(0, 4).ToLower = "Kode".ToLower Then
            '    GV1.Columns(i).Fixed = FixedStyle.Left
            'ElseIf GV1.Columns(i).FieldName.ToLower = "Nama".ToLower Then
            '    GV1.Columns(i).Fixed = FixedStyle.Left
            'End If
        Next
        If System.IO.File.Exists(folderLayouts & Me.Name & GV1.Name & ".xml") Then
            GV1.RestoreLayoutFromXml(folderLayouts & Me.Name & GV1.Name & ".xml")
        End If
        Me.Width = Ini.BacaIniPath(folderLayouts & Me.Name & ".ini", "Form", "Width", Me.Width)
        Me.Height = Ini.BacaIniPath(folderLayouts & Me.Name & ".ini", "Form", "Height", Me.Height)
        Me.Left = (Screen.PrimaryScreen.WorkingArea.Width - Me.Width) \ 2
        Me.Top = (Screen.PrimaryScreen.WorkingArea.Height - Me.Height) \ 2
        If System.IO.File.Exists(folderLayouts & Me.Name & gvGudang.Name & ".xml") Then
            gvGudang.RestoreLayoutFromXml(folderLayouts & Me.Name & gvGudang.Name & ".xml")
        End If
        If System.IO.File.Exists(folderLayouts & Me.Name & gvBarang.Name & ".xml") Then
            gvBarang.RestoreLayoutFromXml(folderLayouts & Me.Name & gvBarang.Name & ".xml")
        End If
    End Sub
    Sub RefreshData()
        'Dim cn As New SqlConnection(StrKonSql)
        'Dim ocmd2 As New SqlCommand
        'If NullTolong(txtBarang.EditValue) = 0 Then Exit Sub ':XtraMessageBox.Show("Isi nama barang terlebih dahulu.", NamaAplikasi, MessageBoxButtons.OK) : Exit Sub
        Dim Cur As Cursor = Windows.Forms.Cursor.Current
        Dim SQL As String = ""
        Dim IsPerluWhere As Boolean = True
        Dim ds As New DataSet
        Dim Saldo As Double = 0.0, SaldoAkhir As Double = 0.0
        Dim saldox As Double
        Windows.Forms.Cursor.Current = Cursors.WaitCursor
        Try
            'If IsShow = False AndAlso txtGudang.Text = "" AndAlso XtraMessageBox.Show("Gudang masih kosong, ingin melanjutkan ?", NamaAplikasi, MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) = Windows.Forms.DialogResult.No Then Exit Try
            'If IsShow = False Then Exit Try
            'If TglDari.Enabled Then
            '    clsPostingKartuStok.TriggerStok(TglDari.DateTime, TglSampai.DateTime, NullToLong(txtBarang.EditValue))
            'Else
            '    clsPostingKartuStok.TriggerStok(CDate(Date.Today.ToString("yyyy-MM-01")), CDate(Date.Today.ToString("yyyy-MM-dd")), NullToLong(txtBarang.EditValue))
            'End If
            If TglDari.Enabled Then
                If txtGudang.Text <> "" Then
                    SQL = "SELECT SUM((MKartuStok.QtyMasuk-MKartuStok.QtyKeluar)*MKartuStok.Konversi) AS Saldo" & vbCrLf & _
                          " FROM MKartuStok " & vbCrLf & _
                          " INNER JOIN (MBarang INNER JOIN MBarangD ON MBarang.NoID=MBarangD.IDBarang) ON MBarang.NoID=MKartuStok.IDBarang AND MBarangD.NoID=MKartuStok.IDBarangD  AND MBarang.NoID=MKartuSTok.IDBarang " & vbCrLf & _
                          " INNER JOIN MGudang ON MGudang.NoID=MKartuStok.IDGudang " & vbCrLf & _
                          " WHERE MKartuStok.Tanggal<'" & TglDari.DateTime.ToString("yyyy-MM-dd") & "' AND MGudang.NoID=" & NullToLong(txtGudang.EditValue) & " AND MBarangD.NoID=" & NullToLong(IDBarangD)
                Else
                    SQL = "SELECT SUM((MKartuStok.QtyMasuk-MKartuStok.QtyKeluar)*MKartuStok.Konversi) AS Saldo" & vbCrLf & _
                          " FROM MKartuStok " & vbCrLf & _
                          " INNER JOIN (MBarang INNER JOIN MBarangD ON MBarang.NoID=MBarangD.IDBarang) ON MBarang.NoID=MKartuStok.IDBarang AND MBarangD.NoID=MKartuStok.IDBarangD AND MBarang.NoID=MKartuSTok.IDBarang " & vbCrLf & _
                          " INNER JOIN MGudang ON MGudang.NoID=MKartuStok.IDGudang " & vbCrLf & _
                          " WHERE MKartuStok.Tanggal<'" & TglDari.DateTime.ToString("yyyy-MM-dd") & "' AND MBarangD.NoID=" & NullToLong(IDBarangD)
                End If
                Saldo = NullToDbl(EksekusiSQlSkalarNew(SQL))
            Else
                Saldo = 0
            End If

            SQL = "SELECT MKartuStok.IDSatuan,MBarangD.Barcode, MGudang.Kode AS Gudang, MWilayah.Kode AS Wilayah, MKartuStok.NoID, MKartuStok.Kode AS KodeTransaksi, MKartuStok.Tanggal, MKartuStok.Keterangan, MBarang.Kode AS KodeBarang, RTRIM(MBarang.Nama + ' ' + ISNULL(MBarangD.Varian,'')) as Nama ,  MKartuStok.QtyMasuk AS QtyMasuk, MKartuStok.QtyKeluar AS QtyKeluar, MKartuStok.QtyMasukA AS [QtyMasuk (Pcs)], MKartuStok.QtyKeluarA AS [QtyKeluar (Pcs)], MSatuan.Nama AS Satuan, mjenisbarang.Nama AS Jenis,Malamat.Nama as Supplier, " & vbCrLf
            SQL &= " MKategori.Nama AS Kategori, MKartuStok.HargaBeli/MKartuStok.Konversi AS HargaPcs,MBarang.IsActive, MJenisTransaksi.Nama AS Transaksi" & vbCrLf & _
                   ", 0.00 AS SaldoAwal" & vbCrLf & _
                   ", 0.00+(MKartuStok.Konversi*(MKartuStok.QtyMasuk-MKartuStok.QtyKeluar)) AS SaldoAkhir " & vbCrLf
            'SQL &= ", (SELECT SUM(TKartuStok.SaldoAwal) FROM TKartuStok WHERE TKartuStok.IDUSer=" & IDUserAktif & " AND TKartuStok.IP='" & FixApostropi(IPLokal) & "' AND TKartuStok.IDKartuStok=MKartuStok.NoID) AS SaldoAwal"
            'SQL &= ", (SELECT SUM(TKartuStok.SaldoAkhir) FROM TKartuStok WHERE TKartuStok.IDUSer=" & IDUserAktif & " AND TKartuStok.IP='" & FixApostropi(IPLokal) & "' AND TKartuStok.IDKartuStok=MKartuStok.NoID) AS SaldoAkhir"
            'SQL &= ", (SELECT SUM(TKartuStok.SaldoAkhir) FROM TKartuStok LEFT JOIN MGudang ON MGudang.NoID=TKartuStok.IDGudang WHERE (MGudang.IsBS=0 OR MGudang.IsBS IS NULL) AND TKartuStok.IDUser=" & IDUserAktif & " AND TKartuStok.IP='" & FixApostropi(IPLokal) & "' AND TKartuStok.IDKartuStok=MKartuStok.NoID) AS StockSiapJual"
            'SQL &= ", (SELECT SUM(TKartuStok.SaldoAkhir) FROM TKartuStok LEFT JOIN MGudang ON MGudang.NoID=TKartuStok.IDGudang WHERE MGudang.IsBS=1 AND TKartuStok.IDUser=" & IDUserAktif & " AND TKartuStok.IP='" & FixApostropi(IPLokal) & "' AND TKartuStok.IDKartuStok=MKartuStok.NoID) AS StockTidakSiapJual"
            SQL &= " FROM MKartuStok INNER JOIN ((MBarangD inner join MBarang on MBarangD.IDBarang=MBarang.NoID) LEFT OUTER JOIN" & vbCrLf
            SQL &= " MKategori ON MBarang.IDKategori = MKategori.NoID LEFT OUTER JOIN" & vbCrLf
            SQL &= " mjenisbarang ON MBarang.IDJenis = mjenisbarang.NoID) ON MBarangD.NoID=MKartuStok.IDBarangD AND MBarang.NoID=MKartuStok.IDBarang " & vbCrLf
            SQL &= " LEFT OUTER JOIN MSatuan ON MKartuStok.IDSatuan = MSatuan.NoID "
            SQL &= " LEFT OUTER JOIN MAlamat ON ((MKartuStok.idjenistransaksi=2 or mkartustok.idjenistransaksi=3) and mkartustok.idalamat=malamat.noid) "
            SQL &= " LEFT JOIN MJenisTransaksi ON MJenisTransaksi.ID=MKartuStok.IDJenisTransaksi" & vbCrLf
            SQL &= " LEFT JOIN (MGudang LEFT JOIN MWilayah ON MWilayah.NoID=MGudang.IDWilayah) ON MGudang.NoID=MKartuStok.IDGudang" & vbCrLf
            SQL &= IIf(IsPerluWhere, " WHERE ", " AND ") & " MWilayah.NoID=" & DefIDWilayah & vbCrLf
            IsPerluWhere = Not IsPerluWhere
            If Not IsSupervisor Then
                SQL &= IIf(IsPerluWhere, " WHERE ", " AND") & " MWilayah.NoID=" & DefIDWilayah & vbCrLf
                IsPerluWhere = False
            End If
            If TglDari.Enabled Then
                SQL &= IIf(IsPerluWhere, " WHERE ", " AND") & " (MKartuStok.Tanggal>='" & TglDari.DateTime.ToString("yyyy/MM/dd") & "' AND MKartuStok.Tanggal<'" & DateAdd(DateInterval.Day, 1, TglSampai.DateTime).ToString("yyyy/MM/dd") & "')" & vbCrLf
                IsPerluWhere = False
            End If
            If ckBS.EditValue = "0" Or ckBS.EditValue = "1" Then
                SQL &= IIf(IsPerluWhere, " WHERE ", " AND") & " (MGudang.IsBS=" & ckBS.EditValue & ")" & vbCrLf
                IsPerluWhere = False
            End If
            If ckIsSPK.Checked Then
                SQL &= IIf(IsPerluWhere, " WHERE ", " AND") & " (MKartuStok.IsSPK=0 OR MKartuStok.IsSPK Is NULL )" & vbCrLf
                IsPerluWhere = False
            End If
            If txtGudang.Text <> "" Then
                SQL &= IIf(IsPerluWhere, " WHERE ", " AND") & " (MGudang.NoID=" & NullTolong(txtGudang.EditValue) & ")" & vbCrLf
                IsPerluWhere = False
            End If
            'If IDBarangD > 0 Then
            SQL &= IIf(IsPerluWhere, " WHERE ", " AND") & " (MKartuStok.IDBarangD=" & IDBarangD & ")" & vbCrLf
            'IsPerluWhere = False
            'End If
            'If txtBarang.Text <> "" Then
            'SQL &= IIf(IsPerluWhere, " WHERE ", " AND") & " (MBarang.NoID=" & NullToLong(txtBarang.EditValue) & ")" & vbCrLf
            IsPerluWhere = False
            'End If
            'SQL &= " ORDER BY MKartuStok.Tanggal, MJenisTransaksi.NoUrut, MkartuStok.NoID"
            SQL &= " ORDER BY DATEADD(dd, 0, DATEDIFF(dd, 0, MKartuStok.Tanggal)), MJenisTransaksi.NoUrut, MKartuStok.NoID "
            'ocmd2.CommandText = SQL
            'cn.Open()
            ds = modSqlServer.ExecuteDataset("Data", SQL)
            'BS.DataSource = Nothing
            'BS.DataSource = ds
            'GC1.DataSource = Nothing
            GC1.DataSource = ds.Tables("Data")

            saldox = Saldo
            For i As Integer = 0 To GV1.RowCount - 1
                SaldoAkhir = saldox + NullToDbl(GV1.GetRowCellValue(i, "SaldoAkhir"))
                GV1.SetRowCellValue(i, "SaldoAwal", saldox)
                GV1.SetRowCellValue(i, "SaldoAkhir", SaldoAkhir)
                saldox = SaldoAkhir
            Next
            txtSaldoAwal.EditValue = Saldo
            txtSaldoAkhir.EditValue = SaldoAkhir

            'IsPerluWhere = False
            'SQL = "SELECT SUM((IsNull(MKartustok.QTYMasuk,0)*IsNull(MKartustok.Konversi,0))-(IsNull(MKartustok.QTYKeluar,0)*IsNull(MKartustok.Konversi,0))) AS QtyStok "
            'SQL &= " FROM MKartuStok LEFT JOIN (MGudang LEFT JOIN MWilayah ON MWilayah.NoID=MGudang.IDWilayah) ON MGudang.NoID=MKartuStok.IDGudang "
            'SQL &= " LEFT JOIN MSatuan ON MSatuan.NoID=MKartuStok.IDSatuan WHERE MKartuStok.IDJenisTransaksi<>11 AND MKartuStok.IDJenisTransaksi<>15 AND MWilayah.NoID=" & DefIDWilayah
            'If TglDari.Enabled Then
            '    SQL &= IIf(IsPerluWhere, " WHERE ", " AND") & " MKartuStok.Tanggal<'" & DateAdd(DateInterval.Day, 1, TglDari.DateTime).ToString("yyyy/MM/dd") & "' " & vbCrLf
            '    IsPerluWhere = False
            'End If
            'If txtGudang.Text <> "" Then
            '    SQL &= IIf(IsPerluWhere, " WHERE ", " AND") & " (MGudang.NoID=" & NullToLong(txtGudang.EditValue) & ")" & vbCrLf
            '    IsPerluWhere = False
            'End If
            'If ckIsSPK.Checked Then
            '    SQL &= IIf(IsPerluWhere, " WHERE ", " AND") & " (MKartuStok.IsSPK=0 OR MKartuStok.IsSPK Is NULL )" & vbCrLf
            '    IsPerluWhere = False
            'End If
            'If IDBarangD > 0 Then
            '    SQL &= IIf(IsPerluWhere, " WHERE ", " AND") & " MKartuStok.IDBarangD=" & IDBarangD
            '    IsPerluWhere = False
            'End If
            'If txtBarang.Text <> "" Then
            '    SQL &= IIf(IsPerluWhere, " WHERE ", " AND") & " MKartuStok.IDBarang=" & NullToLong(txtBarang.EditValue)
            '    IsPerluWhere = False
            'Else
            '    SQL &= IIf(IsPerluWhere, " WHERE ", " AND") & " MKartuStok.IDBarang=" & NullToLong(txtBarang.EditValue)
            '    IsPerluWhere = False
            'End If
            'ds = ExecuteDataset("Saldo", SQL)
            'If ds.Tables(0).Rows.Count >= 1 Then
            '    txtSaldoAwal.EditValue = NullToDbl(ds.Tables(0).Rows(0).Item("QtyStok"))
            'Else
            '    txtSaldoAwal.EditValue = 0
            'End If

            'IsPerluWhere = False
            'SQL = "SELECT SUM((IsNull(MKartustok.QTYMasuk,0)*IsNull(MKartustok.Konversi,0))-(IsNull(MKartustok.QTYKeluar,0)*IsNull(MKartustok.Konversi,0))) AS QtyStok "
            'SQL &= " FROM MKartuStok LEFT JOIN (MGudang LEFT JOIN MWilayah ON MWilayah.NoID=MGudang.IDWilayah) ON MGudang.NoID=MKartuStok.IDGudang "
            'SQL &= " LEFT JOIN MSatuan ON MSatuan.NoID=MKartuStok.IDSatuan  WHERE MKartuStok.IDJenisTransaksi<>11 AND MKartuStok.IDJenisTransaksi<>15 AND MWilayah.NoID=" & DefIDWilayah
            'If TglSampai.Enabled Then
            '    SQL &= IIf(IsPerluWhere, " WHERE ", " AND") & " MKartuStok.Tanggal<'" & DateAdd(DateInterval.Day, 1, TglSampai.DateTime).ToString("yyyy/MM/dd") & "' " & vbCrLf
            '    IsPerluWhere = False
            'End If
            'If txtGudang.Text <> "" Then
            '    SQL &= IIf(IsPerluWhere, " WHERE ", " AND") & " (MGudang.NoID=" & NullToLong(txtGudang.EditValue) & ")" & vbCrLf
            '    IsPerluWhere = False
            'End If
            'If ckIsSPK.Checked Then
            '    SQL &= IIf(IsPerluWhere, " WHERE ", " AND") & " (MKartuStok.IsSPK=0 OR MKartuStok.IsSPK Is NULL )" & vbCrLf
            '    IsPerluWhere = False
            'End If
            'If IDBarangD > 0 Then
            '    SQL &= IIf(IsPerluWhere, " WHERE ", " AND") & " MKartuStok.IDBarangD=" & IDBarangD
            '    IsPerluWhere = False
            'End If
            'If txtBarang.Text <> "" Then
            '    SQL &= IIf(IsPerluWhere, " WHERE ", " AND") & " MKartuStok.IDBarang=" & NullToLong(txtBarang.EditValue)
            '    IsPerluWhere = False
            'Else
            '    SQL &= IIf(IsPerluWhere, " WHERE ", " AND") & " MKartuStok.IDBarang=" & NullToLong(txtBarang.EditValue)
            '    IsPerluWhere = False
            'End If
            'ds = ExecuteDataset("Saldo", SQL)
            'If ds.Tables(0).Rows.Count >= 1 Then
            '    txtSaldoAkhir.EditValue = NullToDbl(ds.Tables(0).Rows(0).Item("QtyStok"))
            'Else
            '    txtSaldoAkhir.EditValue = 0
            'End If
        Catch ex As Exception
            XtraMessageBox.Show("Error : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK)
        Finally
            ds.Dispose()
            oDA.Dispose()
            'ocmd2.Dispose()
            'cn.Close()
            'cn.Dispose()
            Windows.Forms.Cursor.Current = Cur
        End Try
    End Sub

    Private Sub SimpleButton6_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SimpleButton6.Click
        Tutup()
    End Sub

    Private Sub SimpleButton9_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SimpleButton9.Click
        DialogResult = Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub

    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

    End Sub


    Private Sub BarButtonItem4_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem4.ItemClick
        Tutup()
    End Sub
    Sub Tutup()
        DialogResult = Windows.Forms.DialogResult.Cancel
        Me.Close()
        Me.Dispose()
    End Sub

    Private Sub SimpleButton8_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SimpleButton8.Click
        RefreshData()
    End Sub

    Private Sub BarButtonItem6_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem6.ItemClick
        PrintPreview()
    End Sub
    Sub ExportExcel()
        Dim dlgsave As New SaveFileDialog
        dlgsave.Title = "Export Daftar ke Excel"
        dlgsave.Filter = "Excel Files|*.xls"
        If dlgsave.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
            GC1.ExportToXls(dlgsave.FileName)
            BukaFile(dlgsave.FileName)
        End If
        dlgsave.Dispose()
    End Sub
    Sub PrintPreview()

        'GC1.ShowPrintPreview()

        Dim namafile As String = Application.StartupPath & "\Report\DaftarKartuStokVarian.rpt"
        Try
            RefreshData()
            If EditReport Then
                mdlCetakCR.ViewReport(Me.MdiParent, mdlCetakCR.action_.Edit, namafile, "Daftar Kartu Stok", , , "SaldoAwal=" & txtSaldoAwal.EditValue & "&IDBarangD=" & IDBarangD & "&TglDari=cdate(" & TglDari.DateTime.ToString("yyyy,MM,dd") & ")&TglSampai=cdate(" & TglSampai.DateTime.ToString("yyyy,MM,dd") & ")")
            Else
                mdlCetakCR.ViewReport(Me.MdiParent, mdlCetakCR.action_.Preview, namafile, "Daftar Kartu Stok", , , "SaldoAwal=" & txtSaldoAwal.EditValue & "&IDBarangD=" & IDBarangD & "&TglDari=cdate(" & TglDari.DateTime.ToString("yyyy,MM,dd") & ")&TglSampai=cdate(" & TglSampai.DateTime.ToString("yyyy,MM,dd") & ")")
            End If
        Catch ex As Exception
            XtraMessageBox.Show("Info Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK)
        End Try

    End Sub

    Private Sub SimpleButton4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SimpleButton4.Click
        ExportExcel()
    End Sub

    Private Sub SimpleButton5_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SimpleButton5.Click
        PrintPreview()
    End Sub

    Private Sub BarButtonItem5_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem5.ItemClick
        ExportExcel()
    End Sub

    Private Sub mnRefresh_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles mnRefresh.ItemClick
        RefreshData()
    End Sub

    Private Sub LabelControl1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LabelControl1.Click
        TglDari.Enabled = Not TglDari.Enabled
        TglSampai.Enabled = TglDari.Enabled
        RefreshData()
    End Sub

    Private Sub ckBS_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ckBS.EditValueChanged
        RefreshLookUpGudang()
        'RefreshData()
    End Sub
    Private Sub RefreshLookUpGudang()
        Dim ds As New DataSet
        Dim SQL As String = ""
        Try
            SQL = "SELECT MGudang.NoID, MGudang.Kode, MGudang.Nama, MWilayah.Nama AS Wilayah," & _
                  " CASE WHEN MGudang.IsBS = 0 OR" & _
                  " MGudang.IsBS IS NULL THEN 'Siap Jual' ELSE 'Tidak Siap Jual' END AS TipeGudang " & _
                  " FROM MGudang LEFT OUTER JOIN" & _
                  " MWilayah ON MGudang.IDWilayah = MWilayah.NoID" & _
                  " WHERE MGudang.IDWilayah=" & DefIDWilayah & " AND MGudang.IsActive=1 " & IIf(ckBS.EditValue = "0" Or ckBS.EditValue = "1", " AND MGudang.IsBS=" & ckBS.EditValue, "")
            ds = ExecuteDataset("MGudang", SQL)
            txtGudang.Properties.DataSource = ds.Tables("MGudang")
            txtGudang.Properties.DisplayMember = "Nama"
            txtGudang.Properties.ValueMember = "NoID"
            If System.IO.File.Exists(folderLayouts & Me.Name & gvBarang.Name & ".xml") Then
                gvBarang.RestoreLayoutFromXml(folderLayouts & Me.Name & gvBarang.Name & ".xml")
            End If
        Catch ex As Exception
            XtraMessageBox.Show("Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            ds.Dispose()
        End Try
    End Sub

    Private Sub GC1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles GC1.Click

    End Sub

    Private Sub barButtonItem1_ItemClick(ByVal sender As Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles barButtonItem1.ItemClick
        SimpleButton9.PerformClick()
    End Sub

    Private Sub txtBarang_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtBarang.EditValueChanged
        txtNamaBarang.Text = NullToStr(EksekusiSQlSkalarNew("SELECT MBarang.Nama FROM MBarangD inner join Mbarang on MbarangD.IDBarang =Mbarang.NoID WHERE MBarangD.NoID=" & NullToLong(txtBarang.EditValue)))
        TextEdit1.Text = NullToStr(EksekusiSQlSkalarNew("SELECT MBarangD.Barcode FROM MBarangD  WHERE MBarangD.NoID=" & NullToLong(txtBarang.EditValue)))
        IDBarangD = NullToLong(txtBarang.EditValue)
        'RefreshData()
    End Sub

    Private Sub mnSimpanLayouts_ItemClick(ByVal sender As Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles mnSimpanLayouts.ItemClick
        Dim xOtorisasi As New frmOtorisasiAdmin
        Try
            Me.TopMost = False
            If XtraMessageBox.Show("Simpan Layout?", NamaAplikasi, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = Windows.Forms.DialogResult.Yes AndAlso xOtorisasi.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
                GV1.SaveLayoutToXml(folderLayouts & Me.Name & GV1.Name & ".xml")
                gvBarang.SaveLayoutToXml(folderLayouts & Me.Name & gvBarang.Name & ".xml")
                gvGudang.SaveLayoutToXml(folderLayouts & Me.Name & gvGudang.Name & ".xml")
                Ini.TulisIniPath(folderLayouts & Me.Name & ".ini", "Form", "Width", Me.Width)
                Ini.TulisIniPath(folderLayouts & Me.Name & ".ini", "Form", "Height", Me.Height)
                Me.Left = (Screen.PrimaryScreen.WorkingArea.Width - Me.Width) \ 2
                Me.Top = (Screen.PrimaryScreen.WorkingArea.Height - Me.Height) \ 2
            End If
        Catch ex As Exception
            XtraMessageBox.Show("Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            xOtorisasi.Dispose()
            Me.TopMost = True
        End Try
    End Sub

    Private Sub gvBarang_DataSourceChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvBarang.DataSourceChanged
        If System.IO.File.Exists(folderLayouts & Me.Name & gvBarang.Name & ".xml") Then
            gvBarang.RestoreLayoutFromXml(folderLayouts & Me.Name & gvBarang.Name & ".xml")
        End If
        With gvBarang
            For i As Integer = 0 To .Columns.Count - 1
                Select Case .Columns(i).ColumnType.Name.ToLower
                    Case "int32", "int64", "int"
                        .Columns(i).DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
                        .Columns(i).DisplayFormat.FormatString = "n0"
                    Case "decimal", "single", "money", "double"
                        .Columns(i).DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
                        .Columns(i).DisplayFormat.FormatString = "n2"
                    Case "string"
                        .Columns(i).DisplayFormat.FormatType = DevExpress.Utils.FormatType.None
                        .Columns(i).DisplayFormat.FormatString = ""
                    Case "date", "datetime"
                        .Columns(i).DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime
                        .Columns(i).DisplayFormat.FormatString = "dd-MM-yyyy"
                    Case "byte[]"
                        reppicedit.SizeMode = DevExpress.XtraEditors.Controls.PictureSizeMode.Squeeze
                        .Columns(i).OptionsColumn.AllowGroup = False
                        .Columns(i).OptionsColumn.AllowSort = False
                        .Columns(i).OptionsFilter.AllowFilter = False
                        .Columns(i).ColumnEdit = reppicedit
                    Case "boolean"
                        .Columns(i).ColumnEdit = repckedit
                End Select
            Next
        End With
    End Sub

    Private Sub gvGudang_DataSourceChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvGudang.DataSourceChanged
        If System.IO.File.Exists(folderLayouts & Me.Name & gvGudang.Name & ".xml") Then
            gvGudang.RestoreLayoutFromXml(folderLayouts & Me.Name & gvGudang.Name & ".xml")
        End If
        With gvGudang
            For i As Integer = 0 To .Columns.Count - 1
                Select Case .Columns(i).ColumnType.Name.ToLower
                    Case "int32", "int64", "int"
                        .Columns(i).DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
                        .Columns(i).DisplayFormat.FormatString = "n0"
                    Case "decimal", "single", "money", "double"
                        .Columns(i).DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
                        .Columns(i).DisplayFormat.FormatString = "n2"
                    Case "string"
                        .Columns(i).DisplayFormat.FormatType = DevExpress.Utils.FormatType.None
                        .Columns(i).DisplayFormat.FormatString = ""
                    Case "date", "datetime"
                        .Columns(i).DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime
                        .Columns(i).DisplayFormat.FormatString = "dd-MM-yyyy"
                    Case "byte[]"
                        reppicedit.SizeMode = DevExpress.XtraEditors.Controls.PictureSizeMode.Squeeze
                        .Columns(i).OptionsColumn.AllowGroup = False
                        .Columns(i).OptionsColumn.AllowSort = False
                        .Columns(i).OptionsFilter.AllowFilter = False
                        .Columns(i).ColumnEdit = reppicedit
                    Case "boolean"
                        .Columns(i).ColumnEdit = repckedit
                End Select
            Next
        End With
    End Sub

    Private Sub TglDari_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TglDari.EditValueChanged

    End Sub

    Private Sub ckIsSPK_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ckIsSPK.CheckedChanged

    End Sub

    Private Sub txtGudang_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtGudang.EditValueChanged

    End Sub

    Private Sub TextEdit1_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextEdit1.EditValueChanged

    End Sub

    Private Sub TextEdit1_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles TextEdit1.KeyDown
        If e.KeyCode = Keys.Delete Then
            TextEdit1.Text = ""
        ElseIf e.KeyCode = Keys.Enter Then
            IDBarangD = NullToLong(EksekusiSQlSkalarNew("Select MBarangD.NoID from MBarangD where barcode='" & FixApostropi(TextEdit1.Text) & "'"))
            txtBarang.EditValue = IDBarangD ' NullToLong(EksekusiSQlSkalarNew("Select MBarangD.IDBarang from MBarangD where barcode='" & FixApostropi(TextEdit1.Text) & "'"))
        End If
    End Sub
End Class