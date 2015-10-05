Imports System.Data.SqlClient
Imports DevExpress.XtraEditors
Imports DevExpress.XtraGrid.Views.Base
Imports DevExpress.XtraEditors.Repository

Public Class frmLaporanSaldoHutangPerSupplier
    Public IsNew As Boolean
    Public NoID As Long
    Private IDAlamat As Long

    Dim repckedit As New RepositoryItemCheckEdit
    Dim repdateedit As New RepositoryItemDateEdit
    Dim reptextedit As New RepositoryItemTextEdit
    Dim reppicedit As New RepositoryItemPictureEdit

    Sub RefreshItem()
        Dim strsql As String = ""
        Dim oConn As New SqlConnection
        Dim ocmd As New SqlCommand
        Dim oDA As New SqlDataAdapter
        Dim oDS As New DataSet
        Dim dlg As DevExpress.Utils.WaitDialogForm = Nothing
        Dim TDari As Date = TanggalSystem, TSampai As Date = TanggalSystem
        Try
            Windows.Forms.Cursor.Current = Cursors.WaitCursor
            dlg = New DevExpress.Utils.WaitDialogForm("Sedang Merefresh Data ...", NamaAplikasi)
            dlg.TopMost = False
            dlg.Show()
            TDari = CDate(TglDari.DateTime.ToString("yyyy-MM-01"))
            TSampai = TDari.AddMonths(1)

            strsql = "SELECT MGroupSupplier.NoID, UPPER(LEFT(MGroupSupplier.Kode,1)) AS KodeGroup, MGroupSupplier.Kode AS GroupSupplier, MGroupSupplier.Nama, " & vbCrLf & _
                     " IsNull((SELECT SUM(IsNull(A.Adjustment,0)+IsNull(A.Pembelian,0)+IsNull(A.DN,0)+IsNull(A.Kekurangan,0)-IsNull(A.Pelunasan,0)-IsNull(A.Materai,0)-IsNull(A.Retur,0)-IsNull(A.Kelebihan,0)) FROM vKartuHutang A INNER JOIN MAlamat B ON A.IDSupplier=B.NoID INNER JOIN MGroupSupplierD D ON D.IDAlamat=B.NoID INNER JOIN MGroupSupplier C ON C.NoID=D.IDGroupSupplier WHERE A.Tanggal<'" & TDari.ToString("yyyy-MM-dd") & "' AND C.NoID=MGroupSupplier.NoID),0) AS SaldoAwal, " & vbCrLf & _
                     " IsNull(SUM(vKartuHutang.Adjustment),0) AS Adjustment, " & vbCrLf & _
                     " IsNull(SUM(vKartuHutang.Pembelian),0) AS Pembelian, " & vbCrLf & _
                     " IsNull(SUM(vKartuHutang.DN),0) AS DN, " & vbCrLf & _
                     " IsNull(SUM(vKartuHutang.Kekurangan),0) AS Kekurangan, " & vbCrLf & _
                     " IsNull(SUM(vKartuHutang.Pelunasan),0) AS Pelunasan, " & vbCrLf & _
                     " IsNull(SUM(vKartuHutang.Materai),0) AS Materai, " & vbCrLf & _
                     " IsNull(SUM(vKartuHutang.Retur),0) AS Retur, " & vbCrLf & _
                     " IsNull(SUM(vKartuHutang.Kelebihan),0) AS Kelebihan, " & vbCrLf & _
                     " IsNull((SELECT SUM(IsNull(A.Adjustment,0)+IsNull(A.Pembelian,0)+IsNull(A.DN,0)+IsNull(A.Kekurangan,0)-IsNull(A.Pelunasan,0)-IsNull(A.Materai,0)-IsNull(A.Retur,0)-IsNull(A.Kelebihan,0)) FROM vKartuHutang A INNER JOIN MAlamat B ON A.IDSupplier=B.NoID INNER JOIN MGroupSupplierD D ON D.IDAlamat=B.NoID INNER JOIN MGroupSupplier C ON C.NoID=D.IDGroupSupplier WHERE A.Tanggal<'" & TDari.ToString("yyyy-MM-dd") & "' AND C.NoID=MGroupSupplier.NoID),0)+" & vbCrLf & _
                     " IsNull(SUM(vKartuHutang.Adjustment),0) +" & vbCrLf & _
                     " IsNull(SUM(vKartuHutang.Pembelian),0) +" & vbCrLf & _
                     " IsNull(SUM(vKartuHutang.DN),0) +" & vbCrLf & _
                     " IsNull(SUM(vKartuHutang.Kekurangan),0) -" & vbCrLf & _
                     " IsNull(SUM(vKartuHutang.Pelunasan),0) -" & vbCrLf & _
                     " IsNull(SUM(vKartuHutang.Materai),0) -" & vbCrLf & _
                     " IsNull(SUM(vKartuHutang.Retur),0) -" & vbCrLf & _
                     " IsNull(SUM(vKartuHutang.Kelebihan),0)" & vbCrLf & _
                     " AS SaldoAkhir" & vbCrLf & _
                     " FROM (MAlamat " & vbCrLf & _
                     " INNER JOIN (MGroupSupplierD INNER JOIN MGroupSupplier ON MGroupSupplier.NoID=MGroupSupplierD.IDGroupSupplier) ON MAlamat.NoID=MGroupSupplierD.IDAlamat)" & vbCrLf & _
                     " LEFT JOIN vKartuHutang ON MAlamat.NoID=vKartuHutang.IDSupplier AND vKartuHutang.Tanggal>='" & TDari.ToString("yyyy-MM-dd") & "' AND vKartuHutang.Tanggal<'" & TSampai.ToString("yyyy-MM-dd") & "'" & vbCrLf & _
                     " WHERE MAlamat.IsSupplier=1 AND MAlamat.IsActive=1 AND UPPER(LEFT(MGroupSupplier.Kode,1))>='" & FixApostropi(txtSup1.Text.ToUpper) & "' AND UPPER(LEFT(MGroupSupplier.Kode,1))<='" & FixApostropi(txtSup2.Text.ToUpper) & "'" & vbCrLf & _
                     " GROUP BY MGroupSupplier.Kode, MGroupSupplier.NoID, MGroupSupplier.Nama " & vbCrLf & _
                     " ORDER BY MGroupSupplier.Kode"
            oConn.ConnectionString = StrKonSql
            oConn.Open()
            ocmd.Connection = oConn
            oDA.SelectCommand = ocmd
            ocmd.CommandText = strsql
            oDA.Fill(oDS, "LaporanPembayaranPersupplier")
            If Not oDS.Tables("LaporanPembayaranPersupplier") Is Nothing Then
                GridControl1.DataSource = oDS.Tables("LaporanPembayaranPersupplier")
            End If
            GridView1.OptionsCustomization.AllowSort = False
            GridView1.OptionsBehavior.Editable = False
            Application.DoEvents()

            With GridView1
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
                            If .Columns(i).FieldName.Trim.ToLower = "jam" Then
                                .Columns(i).DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime
                                .Columns(i).DisplayFormat.FormatString = "HH:mm"
                            Else
                                .Columns(i).DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime
                                .Columns(i).DisplayFormat.FormatString = "dd-MM-yyyy"
                            End If
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

            'HitungSaldo()
        Catch ex As Exception
            XtraMessageBox.Show(Me, ex.Message, ".:: Pesan Kesalahan ::.", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            If oConn.State = ConnectionState.Open Then
                oConn.Close()
            End If
            dlg.Close()
            dlg.Dispose()
            ocmd.Dispose()
            oConn.Dispose()
            oDS.Dispose()
            oDA.Dispose()
            Windows.Forms.Cursor.Current = Cursors.Default
        End Try
    End Sub

    Private Sub frBayarHutang_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        GridView1.SaveLayoutToXml(FolderLayouts & Me.Name & ".xml")
    End Sub
    Dim DefImageList As New ImageList
    Private Sub SetTombol()
        DefImageList = frmMain.ImageList1

        cmdRefresh.ImageList = DefImageList
        cmdRefresh.ImageIndex = 5

        cmdSave.ImageList = DefImageList
        cmdSave.ImageIndex = 6

        cmdTutup.ImageList = DefImageList
        cmdTutup.ImageIndex = 3
        cmdExcel.ImageList = DefImageList
        cmdExcel.ImageIndex = 11

        cmdPreview.ImageList = DefImageList
        cmdPreview.ImageIndex = 8
        cmdRekap.ImageList = DefImageList
        cmdRekap.ImageIndex = 7

    End Sub
    Private Sub frBayarHutang_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        TglDari.DateTime = CDate(TanggalSystem.ToString("yyyy,MM,01"))
        SetTombol()
        RefreshItem()
        'HitungSaldo()
        FungsiControl.SetForm(Me)
        TglDari.Properties.EditMask = "MMMM-yyyy"
        TglDari.Properties.Mask.MaskType = Mask.MaskType.DateTime
        TglDari.Properties.Mask.UseMaskAsDisplayFormat = True
        If Dir(FolderLayouts & Me.Name & ".xml") <> "" Then
            GridView1.RestoreLayoutFromXml(FolderLayouts & Me.Name & ".xml")
        End If
    End Sub



    Private Sub cmdTutup_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdTutup.Click
        DialogResult = Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub


    Private Sub cmdRefresh_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdRefresh.Click
        RefreshItem()
        'HitungSaldo()
    End Sub

    'Private Sub HitungSaldo()
    '    Dim SQL As String = ""
    '    Dim ds As New DataSet
    '    Dim SaldoAwal As Double = 0.0
    '    Dim SaldoAkhir As Double = 0.0
    '    Dim SampaiTanggal As Date
    '    Try
    '        SampaiTanggal = CDate(TglDari.DateTime.ToString("yyyy,MM,01")).AddMonths(1)
    '        SQL = "SELECT MBeli.Tanggal, MAlamat.Kode AS KodeSupplier, MAlamat.Nama AS Supplier, MBeli.Kode AS NoNota, 0 AS SaldoAwal, MBeli.Total AS Pembelian, 0 AS DN, 0 AS Kekurangan, 0 AS Pelunasan, 0 AS Materai, 0 AS Retur, 0 AS Kelebihan, 0 AS SaldoAkhir, 'Pembelian' AS Keterangan" & vbCrLf & _
    '             " FROM MBeli INNER JOIN MAlamat ON MAlamat.NoID=MBeli.IDSupplier" & vbCrLf & _
    '             " WHERE MBeli.IsPosted=1 AND MAlamat.NoID=" & IDAlamat & " AND MBeli.Tanggal<'" & SampaiTanggal.ToString("yyyy-MM-dd") & "'" & vbCrLf & _
    '             " UNION ALL" & vbCrLf & _
    '             " SELECT MBayarHutang.TglKembali, MAlamat.Kode AS KodeSupplier, MAlamat.Nama AS Supplier, MReturBeli.Kode AS NoNota, 0 AS SaldoAwal, 0 AS Pembelian, 0 AS DN, 0 AS Kekurangan, 0 AS Pelunasan, 0 AS Materai, MReturBeli.Total AS Retur, 0 AS Kelebihan, 0 AS SaldoAkhir, 'Retur Pembelian' AS Keterangan" & vbCrLf & _
    '             " FROM MReturBeli INNER JOIN MAlamat ON MAlamat.NoID=MReturBeli.IDSupplier" & vbCrLf & _
    '             " INNER JOIN (MBayarHutang INNER JOIN MBayarHutangDRetur ON MBayarHutang.NoID=MBayarHutangDRetur.IDBayarHutang) ON MBayarHutangDRetur.IDReturBeli=MReturBeli.NoID" & vbCrLf & _
    '             " WHERE MReturBeli.IsPosted=1 AND MAlamat.NoID=" & IDAlamat & " AND MBayarHutang.TglKembali<'" & SampaiTanggal.ToString("yyyy-MM-dd") & "'" & vbCrLf & _
    '             " UNION ALL " & vbCrLf & _
    '             " SELECT MBayarHutang.TglKembali, MAlamat.Kode AS KodeSupplier, MAlamat.Nama AS Supplier, MBayarHutang.Kode AS NoNota, 0 AS SaldoAwal, 0 AS Pembelian, 0 AS DN, 0 AS Kekurangan, MBayarHutang.Total AS Pelunasan, 0 AS Materai, 0 AS Retur, 0 AS Kelebihan, 0 AS SaldoAkhir, 'Pembayaran Hutang' AS Keterangan" & vbCrLf & _
    '             " FROM MBayarHutang INNER JOIN MAlamat ON MAlamat.NoID=MBayarHutang.IDAlamat" & vbCrLf & _
    '             " WHERE MBayarHutang.IsPosted=1 AND MAlamat.NoID=" & IDAlamat & " AND MBayarHutang.TglKembali<'" & SampaiTanggal.ToString("yyyy-MM-dd") & "'" & vbCrLf & _
    '             " UNION ALL" & vbCrLf & _
    '             " SELECT MBayarHutang.TglKembali, MAlamat.Kode AS KodeSupplier, MAlamat.Nama AS Supplier, MBayarHutang.Kode AS NoNota, 0 AS SaldoAwal, 0 AS Pembelian, 0 AS DN, SUM(MBayarHutangDDebet.Potong) AS Kekurangan, MBayarHutang.Total AS Pelunasan, 0 AS Materai, 0 AS Retur, 0 AS Kelebihan, 0 AS SaldoAkhir, 'DN Nota' AS Keterangan" & vbCrLf & _
    '             " FROM (MBayarHutang INNER JOIN MBayarHutangDDebet ON MBayarHutang.NoID=MBayarHutangDDebet.IDBayarHutang) INNER JOIN MAlamat ON MAlamat.NoID=MBayarHutang.IDAlamat" & vbCrLf & _
    '             " WHERE MBayarHutang.IsPosted=1 AND MAlamat.NoID=" & IDAlamat & " AND MBayarHutang.TglKembali<'" & SampaiTanggal.ToString("yyyy-MM-dd") & "'" & vbCrLf & _
    '             " GROUP BY MBayarHutang.TglKembali, MAlamat.Kode, MAlamat.Nama, MBayarHutang.Kode, MBayarHutang.DN" & vbCrLf & _
    '             " UNION ALL" & vbCrLf & _
    '             " SELECT MBayarHutang.TglKembali, MAlamat.Kode AS KodeSupplier, MAlamat.Nama AS Supplier, MBayarHutang.Kode AS NoNota, 0 AS SaldoAwal, 0 AS Pembelian, 0 AS DN, 0 AS Kekurangan, MBayarHutang.Total AS Pelunasan, 0 AS Materai, 0 AS Retur, ISNULL(SUM(MBayarHutangDKredit.Potong),0) AS Kelebihan, 0 AS SaldoAkhir, 'CN Nota' AS Keterangan" & vbCrLf & _
    '             " FROM (MBayarHutang INNER JOIN MBayarHutangDKredit ON MBayarHutang.NoID=MBayarHutangDKredit.IDBayarHutang) INNER JOIN MAlamat ON MAlamat.NoID=MBayarHutang.IDAlamat" & vbCrLf & _
    '             " WHERE MBayarHutang.IsPosted=1 AND MAlamat.NoID=" & IDAlamat & " AND MBayarHutang.TglKembali<'" & SampaiTanggal.ToString("yyyy-MM-dd") & "'" & vbCrLf & _
    '             " GROUP BY MBayarHutang.TglKembali, MAlamat.Kode, MAlamat.Nama, MBayarHutang.Kode, MBayarHutang.Potongan" & vbCrLf & _
    '             " UNION ALL" & vbCrLf & _
    '             " SELECT MBayarHutang.TglKembali, MAlamat.Kode AS KodeSupplier, MAlamat.Nama AS Supplier, MBayarHutang.Kode AS NoNota, 0 AS SaldoAwal, 0 AS Pembelian, 0 AS DN, 0 AS Kekurangan, MBayarHutang.Total AS Pelunasan, IsNull(MBayarHutang.Materai,0) AS Materai, 0 AS Retur, 0 AS Kelebihan, 0 AS SaldoAkhir, 'Materai Pembayaran' AS Keterangan" & vbCrLf & _
    '             " FROM MBayarHutang INNER JOIN MAlamat ON MAlamat.NoID=MBayarHutang.IDAlamat" & vbCrLf & _
    '             " WHERE MBayarHutang.IsPosted=1 AND MAlamat.NoID=" & IDAlamat & " AND MBayarHutang.TglKembali<'" & SampaiTanggal.ToString("yyyy-MM-dd") & "' AND IsNull(MBayarHutang.Materai,0)<>0" & vbCrLf & _
    '             " UNION ALL" & vbCrLf & _
    '             " SELECT MBayarHutang.TglKembali, MAlamat.Kode AS KodeSupplier, MAlamat.Nama AS Supplier, MBayarHutang.Kode AS NoNota, 0 AS SaldoAwal, 0 AS Pembelian, 0 AS DN, 0 AS Kekurangan, MBayarHutang.Total AS Pelunasan, 0 AS Materai, 0 AS Retur, IsNull(MBayarHutang.JumlahKwitansi,0) AS Kelebihan, 0 AS SaldoAkhir, 'Kwitansi Pembayaran' AS Keterangan" & vbCrLf & _
    '             " FROM MBayarHutang INNER JOIN MAlamat ON MAlamat.NoID=MBayarHutang.IDAlamat" & vbCrLf & _
    '             " WHERE MBayarHutang.IsPosted=1 AND MAlamat.NoID=" & IDAlamat & " AND AND MBayarHutang.TglKembali<'" & SampaiTanggal.ToString("yyyy-MM-dd") & "' AND IsNull(MBayarHutang.JumlahKwitansi,0)<>0" & vbCrLf
    '        SQL = "SELECT SUM(KartuHutang.Pembelian-KartuHutang.Retur-KartuHutang.Pelunasan+KartuHutang.DN+KartuHutang.Kekurangan-KartuHutang.Materai-KartuHutang.Retur-KartuHutang.Kelebihan) AS Saldo FROM (" & SQL & ") AS KartuHutang"
    '        ds = ExecuteDataset("Data", SQL)
    '        If ds.Tables("Data").Rows.Count >= 1 Then
    '            SaldoAwal = NullToDbl(ds.Tables("Data").Rows(0).Item("Saldo"))
    '            'txtSaldoAwal.EditValue = NullToDbl(ds.Tables("Data").Rows(0).Item("Saldo"))
    '            SaldoAkhir = SaldoAwal
    '            For i As Integer = 0 To GridView1.RowCount - 1
    '                SaldoAwal = SaldoAkhir
    '                SaldoAkhir = SaldoAwal + NullToDbl(GridView1.GetRowCellValue(i, "Masuk")) - NullToDbl(GridView1.GetRowCellValue(i, "Keluar"))
    '                GridView1.SetRowCellValue(i, "SaldoAwal", SaldoAwal)
    '                GridView1.SetRowCellValue(i, "SaldoAkhir", SaldoAkhir)
    '            Next
    '            txtSaldoAkhir.EditValue = SaldoAkhir
    '        Else
    '            txtSaldoAwal.EditValue = 0
    '            txtSaldoAkhir.EditValue = 0
    '        End If
    '    Catch ex As Exception
    '        XtraMessageBox.Show("Info Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
    '    Finally
    '        ds.Dispose()
    '    End Try
    'End Sub

    Private Sub BarButtonItem1_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem1.ItemClick
        cmdSave.PerformClick()
    End Sub

    Private Sub BarButtonItem6_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem6.ItemClick
        cmdRefresh.PerformClick()
    End Sub

    Private Sub BarButtonItem7_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem7.ItemClick
        cmdTutup.PerformClick()
    End Sub

    Private Sub BarButtonItem3_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem3.ItemClick

    End Sub

    Private Sub GridControl1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles GridControl1.Click

    End Sub

    'Private Sub GridView1_CellValueChanged(ByVal sender As Object, ByVal e As DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs) Handles GridView1.CellValueChanged
    '    Try
    '        If GridView1.FocusedColumn.FieldName.ToLower = "Keterangan".ToLower Then
    '            If GridView1.GetRowCellValue(GridView1.FocusedRowHandle, "Keterangan").ToString.Length <= 100 Then
    '                EksekusiSQL("UPDATE MTransferOutD SET Keterangan='" & FixApostropi(GridView1.GetRowCellValue(GridView1.FocusedRowHandle, "Keterangan").ToString) & "' WHERE NoID=" & NullTolong(GridView1.GetRowCellValue(GridView1.FocusedRowHandle, "NoID")))
    '            Else
    '                XtraMessageBox.Show("Keterangan terlalu panjang.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
    '            End If
    '        End If
    '    Catch ex As Exception
    '        XtraMessageBox.Show("Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
    '    End Try
    'End Sub
    Sub ExportExcel()
        Dim dlgsave As New SaveFileDialog
        dlgsave.Title = "Export Daftar ke Excel"
        dlgsave.Filter = "Excel Files|*.xls"
        If dlgsave.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
            GridView1.ExportToXls(dlgsave.FileName)
            BukaFile(dlgsave.FileName)
        End If
        dlgsave.Dispose()
    End Sub

    Sub PrintPreview()
        'GridControl1.ShowPrintPreview()
        Dim SQL As String = ""
        Dim TDari As Date = TanggalSystem, TSampai As Date = TanggalSystem
        Dim NamaFile As String = Application.StartupPath & "\Report\LaporanSaldoHutangDetil.rpt"
        Try
            TDari = CDate(TglDari.DateTime.ToString("yyyy-MM-01"))
            TSampai = TDari.AddMonths(1)

            If EditReport Then
                mdlCetakCR.ViewReport(Me.MdiParent, mdlCetakCR.action_.Edit, NamaFile, "Laporan Saldo Hutang Detil Per Supplier", , , "Dari='" & txtSup1.Text & "'&Sampai='" & txtSup2.Text & "'&IDUser=" & IDUserAktif & "&TglDari=cdate(" & TDari.ToString("yyyy,MM,dd") & ")&TglSampai=cdate(" & TSampai.ToString("yyyy,MM,dd") & ")")
            Else
                EksekusiSQL("DELETE FROM MLapSaldoHutang WHERE IDUser=" & IDUserAktif)

                SQL = "INSERT INTO [MLapSaldoHutang] ([IDUser],[IDSupplier],[KodeGroup],[GroupSupplier],[SaldoAwal],[Adjustment],[Pembelian],[DN],[Kekurangan],[Pelunasan],[Materai],[Retur],[Kelebihan],[SaldoAkhir]) " & vbCrLf & _
                      " SELECT " & IDUserAktif & ", MAlamat.NoID, UPPER(LEFT(MGroupSupplier.Kode,1)) AS KodeGroup, MGroupSupplier.Kode AS GroupSupplier, " & vbCrLf & _
                      " IsNull((SELECT SUM(IsNull(A.Adjustment,0)+IsNull(A.Pembelian,0)+IsNull(A.DN,0)+IsNull(A.Kekurangan,0)-IsNull(A.Pelunasan,0)-IsNull(A.Materai,0)-IsNull(A.Retur,0)-IsNull(A.Kelebihan,0)) FROM vKartuHutang A WHERE A.Tanggal<'" & TDari.ToString("yyyy-MM-dd") & "' AND A.IDSupplier=MAlamat.NoID),0) AS SaldoAwal, " & vbCrLf & _
                      " IsNull(SUM(vKartuHutang.Adjustment),0) AS Adjustment, " & vbCrLf & _
                      " IsNull(SUM(vKartuHutang.Pembelian),0) AS Pembelian, " & vbCrLf & _
                      " IsNull(SUM(vKartuHutang.DN),0) AS DN, " & vbCrLf & _
                      " IsNull(SUM(vKartuHutang.Kekurangan),0) AS Kekurangan, " & vbCrLf & _
                      " IsNull(SUM(vKartuHutang.Pelunasan),0) AS Pelunasan, " & vbCrLf & _
                      " IsNull(SUM(vKartuHutang.Materai),0) AS Materai, " & vbCrLf & _
                      " IsNull(SUM(vKartuHutang.Retur),0) AS Retur, " & vbCrLf & _
                      " IsNull(SUM(vKartuHutang.Kelebihan),0) AS Kelebihan, " & vbCrLf & _
                      " IsNull((SELECT SUM(IsNull(IsNull(A.Adjustment,0)+A.Pembelian,0)+IsNull(A.DN,0)+IsNull(A.Kekurangan,0)-IsNull(A.Pelunasan,0)-IsNull(A.Materai,0)-IsNull(A.Retur,0)-IsNull(A.Kelebihan,0)) FROM vKartuHutang A WHERE A.Tanggal<'" & TDari.ToString("yyyy-MM-dd") & "' AND A.IDSupplier=MAlamat.NoID),0)+" & vbCrLf & _
                      " IsNull(SUM(vKartuHutang.Adjustment),0) +" & vbCrLf & _
                      " IsNull(SUM(vKartuHutang.Pembelian),0) +" & vbCrLf & _
                      " IsNull(SUM(vKartuHutang.DN),0) +" & vbCrLf & _
                      " IsNull(SUM(vKartuHutang.Kekurangan),0) -" & vbCrLf & _
                      " IsNull(SUM(vKartuHutang.Pelunasan),0) -" & vbCrLf & _
                      " IsNull(SUM(vKartuHutang.Materai),0) -" & vbCrLf & _
                      " IsNull(SUM(vKartuHutang.Retur),0) -" & vbCrLf & _
                      " IsNull(SUM(vKartuHutang.Kelebihan),0)" & vbCrLf & _
                      " AS SaldoAkhir" & vbCrLf & _
                      " FROM (MAlamat " & vbCrLf & _
                      " INNER JOIN (MGroupSupplierD INNER JOIN MGroupSupplier ON MGroupSupplier.NoID=MGroupSupplierD.IDGroupSupplier) ON MAlamat.NoID=MGroupSupplierD.IDAlamat)" & vbCrLf & _
                      " LEFT JOIN vKartuHutang ON MAlamat.NoID=vKartuHutang.IDSupplier AND vKartuHutang.Tanggal>='" & TDari.ToString("yyyy-MM-dd") & "' AND vKartuHutang.Tanggal<'" & TSampai.ToString("yyyy-MM-dd") & "'" & vbCrLf & _
                      " WHERE MAlamat.IsSupplier=1 AND MAlamat.IsActive=1 AND UPPER(LEFT(MGroupSupplier.Kode,1))>='" & FixApostropi(txtSup1.Text.ToUpper) & "' AND UPPER(LEFT(MGroupSupplier.Kode,1))<='" & FixApostropi(txtSup2.Text.ToUpper) & "'" & vbCrLf & _
                      " GROUP BY MAlamat.NoID, MAlamat.Kode, MAlamat.Nama, MAlamat.Alamat, MAlamat.Dept, MGroupSupplier.Kode, UPPER(LEFT(MGroupSupplier.Kode,1))" & vbCrLf & _
                      " ORDER BY MGroupSupplier.Kode"
                EksekusiSQL(SQL)

                mdlCetakCR.ViewReport(Me.MdiParent, mdlCetakCR.action_.Preview, NamaFile, "Laporan Saldo Hutang Detil Per Supplier", , , "Dari='" & txtSup1.Text & "'&Sampai='" & txtSup2.Text & "'&IDUser=" & IDUserAktif & "&TglDari=cdate(" & TDari.ToString("yyyy,MM,dd") & ")&TglSampai=cdate(" & TSampai.ToString("yyyy,MM,dd") & ")")
            End If
        Catch ex As Exception
            XtraMessageBox.Show("Info Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
        End Try
    End Sub
    Private Sub cmdExcel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdExcel.Click
        ExportExcel()
    End Sub

    Private Sub cmdPreview_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdPreview.Click
        PrintPreview()
    End Sub

    Private Sub cmdSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSave.Click

    End Sub

    Private Sub LabelControl3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LabelControl3.Click
        'TglDari.Enabled = Not TglDari.Enabled
    End Sub

    Private Sub cmdRekap_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdRekap.Click
        'GridControl1.ShowPrintPreview()
        Dim SQL As String = ""
        Dim TDari As Date = TanggalSystem, TSampai As Date = TanggalSystem
        Dim NamaFile As String = Application.StartupPath & "\Report\LaporanSaldoHutangRekap.rpt"
        Try
            TDari = CDate(TglDari.DateTime.ToString("yyyy-MM-01"))
            TSampai = TDari.AddMonths(1)

            If EditReport Then
                mdlCetakCR.ViewReport(Me.MdiParent, mdlCetakCR.action_.Edit, NamaFile, "Laporan Saldo Hutang Rekap", , , "Dari='" & txtSup1.Text & "'&Sampai='" & txtSup2.Text & "'&IDUser=" & IDUserAktif & "&TglDari=cdate(" & TDari.ToString("yyyy,MM,dd") & ")&TglSampai=cdate(" & TSampai.ToString("yyyy,MM,dd") & ")")
            Else
                EksekusiSQL("DELETE FROM MLapSaldoHutang WHERE IDUser=" & IDUserAktif)

                SQL = "INSERT INTO [MLapSaldoHutang] ([IDUser],[IDSupplier],[KodeGroup],[GroupSupplier],[SaldoAwal],[Adjustment],[Pembelian],[DN],[Kekurangan],[Pelunasan],[Materai],[Retur],[Kelebihan],[SaldoAkhir]) " & vbCrLf & _
                      " SELECT " & IDUserAktif & ", MAlamat.NoID, UPPER(LEFT(MGroupSupplier.Kode,1)) AS KodeGroup, MGroupSupplier.Kode AS GroupSupplier, " & vbCrLf & _
                      " IsNull((SELECT SUM(IsNull(A.Adjustment,0)+IsNull(A.Pembelian,0)+IsNull(A.DN,0)+IsNull(A.Kekurangan,0)-IsNull(A.Pelunasan,0)-IsNull(A.Materai,0)-IsNull(A.Retur,0)-IsNull(A.Kelebihan,0)) FROM vKartuHutang A WHERE A.Tanggal<'" & TDari.ToString("yyyy-MM-dd") & "' AND A.IDSupplier=MAlamat.NoID),0) AS SaldoAwal, " & vbCrLf & _
                      " IsNull(SUM(vKartuHutang.Adjustment),0) AS Pembelian, " & vbCrLf & _
                      " IsNull(SUM(vKartuHutang.Pembelian),0) AS Pembelian, " & vbCrLf & _
                      " IsNull(SUM(vKartuHutang.DN),0) AS DN, " & vbCrLf & _
                      " IsNull(SUM(vKartuHutang.Kekurangan),0) AS Kekurangan, " & vbCrLf & _
                      " IsNull(SUM(vKartuHutang.Pelunasan),0) AS Pelunasan, " & vbCrLf & _
                      " IsNull(SUM(vKartuHutang.Materai),0) AS Materai, " & vbCrLf & _
                      " IsNull(SUM(vKartuHutang.Retur),0) AS Retur, " & vbCrLf & _
                      " IsNull(SUM(vKartuHutang.Kelebihan),0) AS Kelebihan, " & vbCrLf & _
                      " IsNull((SELECT SUM(IsNull(A.Adjustment,0)+IsNull(A.Pembelian,0)+IsNull(A.DN,0)+IsNull(A.Kekurangan,0)-IsNull(A.Pelunasan,0)-IsNull(A.Materai,0)-IsNull(A.Retur,0)-IsNull(A.Kelebihan,0)) FROM vKartuHutang A WHERE A.Tanggal<'" & TDari.ToString("yyyy-MM-dd") & "' AND A.IDSupplier=MAlamat.NoID),0)+" & vbCrLf & _
                      " IsNull(SUM(vKartuHutang.Adjustment),0) +" & vbCrLf & _
                      " IsNull(SUM(vKartuHutang.Pembelian),0) +" & vbCrLf & _
                      " IsNull(SUM(vKartuHutang.DN),0) +" & vbCrLf & _
                      " IsNull(SUM(vKartuHutang.Kekurangan),0) -" & vbCrLf & _
                      " IsNull(SUM(vKartuHutang.Pelunasan),0) -" & vbCrLf & _
                      " IsNull(SUM(vKartuHutang.Materai),0) -" & vbCrLf & _
                      " IsNull(SUM(vKartuHutang.Retur),0) -" & vbCrLf & _
                      " IsNull(SUM(vKartuHutang.Kelebihan),0)" & vbCrLf & _
                      " AS SaldoAkhir" & vbCrLf & _
                      " FROM (MAlamat " & vbCrLf & _
                      " INNER JOIN (MGroupSupplierD INNER JOIN MGroupSupplier ON MGroupSupplier.NoID=MGroupSupplierD.IDGroupSupplier) ON MAlamat.NoID=MGroupSupplierD.IDAlamat)" & vbCrLf & _
                      " LEFT JOIN vKartuHutang ON MAlamat.NoID=vKartuHutang.IDSupplier AND vKartuHutang.Tanggal>='" & TDari.ToString("yyyy-MM-dd") & "' AND vKartuHutang.Tanggal<'" & TSampai.ToString("yyyy-MM-dd") & "'" & vbCrLf & _
                      " WHERE MAlamat.IsSupplier=1 AND MAlamat.IsActive=1 AND UPPER(LEFT(MGroupSupplier.Kode,1))>='" & FixApostropi(txtSup1.Text.ToUpper) & "' AND UPPER(LEFT(MGroupSupplier.Kode,1))<='" & FixApostropi(txtSup2.Text.ToUpper) & "'" & vbCrLf & _
                      " GROUP BY MAlamat.NoID, MAlamat.Kode, MAlamat.Nama, MAlamat.Alamat, MAlamat.Dept, MGroupSupplier.Kode, UPPER(LEFT(MGroupSupplier.Kode,1))" & vbCrLf & _
                      " ORDER BY MGroupSupplier.Kode"
                EksekusiSQL(SQL)

                mdlCetakCR.ViewReport(Me.MdiParent, mdlCetakCR.action_.Preview, NamaFile, "Laporan Saldo Hutang Rekap", , , "Dari='" & txtSup1.Text & "'&Sampai='" & txtSup2.Text & "'&IDUser=" & IDUserAktif & "&TglDari=cdate(" & TDari.ToString("yyyy,MM,dd") & ")&TglSampai=cdate(" & TSampai.ToString("yyyy,MM,dd") & ")")
            End If
        Catch ex As Exception
            XtraMessageBox.Show("Info Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
        End Try
    End Sub
End Class