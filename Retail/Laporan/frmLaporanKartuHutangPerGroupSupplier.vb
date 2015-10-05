Imports System.Data.SqlClient
Imports DevExpress.XtraEditors
Imports DevExpress.XtraGrid.Views.Base
Imports DevExpress.XtraEditors.Repository

Public Class frmLaporanKartuHutangPerGroupSupplier
    Public IsNew As Boolean
    Public NoID As Long
    Private IDAlamat As Long

    Dim repckedit As New RepositoryItemCheckEdit
    Dim repdateedit As New RepositoryItemDateEdit
    Dim reptextedit As New RepositoryItemTextEdit
    Dim reppicedit As New RepositoryItemPictureEdit

    Private Sub ButtonEdit1_ButtonClick(ByVal sender As Object, ByVal e As DevExpress.XtraEditors.Controls.ButtonPressedEventArgs) Handles txtAlamat.ButtonClick
        'Select Case e.Button.Index
        '    Case 0
        '        Dim x As New frLUAlamat
        '        x.IsSupplier = True
        '        If x.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
        '            IDAlamat = x.NoID
        '            txtAlamat.Text = x.Nama & " - " & x.Kontak
        '        End If
        '        x.Dispose()
        '    Case 1
        '        IDAlamat = -1
        '        txtAlamat.Text = ""
        'End Select
        'RefreshItem()
    End Sub

    Private Sub RefreshLookUp()
        Dim ds As New DataSet
        Try
            ds = ExecuteDataset("Data", "SELECT NoID, Kode, Nama, ISNULL((SELECT COUNT(MGroupSupplierD.NoID) FROM MGroupSupplierD WHERE MGroupSupplierD.IDGroupSupplier=MGroupSupplier.NoID),0) AS JumlahSupplier FROM MGroupSupplier WHERE IsActive=1")
            txtAlamat.Properties.DataSource = ds.Tables("Data")
            txtAlamat.Properties.DisplayMember = "Nama"
            txtAlamat.Properties.ValueMember = "NoID"
        Catch ex As Exception
            XtraMessageBox.Show("Info Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
        Finally
            If Not ds Is Nothing Then
                ds.Dispose()
            End If
        End Try
    End Sub
    Sub RefreshItem()
        Dim strsql As String = ""
        Dim oConn As New SqlConnection
        Dim ocmd As New SqlCommand
        Dim oDA As New SqlDataAdapter
        Dim oDS As New DataSet
        Dim dlg As DevExpress.Utils.WaitDialogForm = Nothing

        Try
            Windows.Forms.Cursor.Current = Cursors.WaitCursor
            dlg = New DevExpress.Utils.WaitDialogForm("Sedang Merefresh Data ...", NamaAplikasi)
            dlg.TopMost = False
            dlg.Show()
            IDAlamat = NullToLong(txtAlamat.EditValue)
            strsql = "SELECT 18 AS IDJenisTransaksi, DATEADD(dd, 0, DATEDIFF(dd, 0, MSaldoAwalHutangPiutang.Tanggal)) AS Tanggal, MAlamat.Kode AS KodeSupplier, MAlamat.Nama AS Supplier, MSaldoAwalHutangPiutang.Kode AS NoNota, MSaldoAwalHutangPiutang.SaldoAwal AS Masuk, 0 AS Keluar, 'Saldo Awal' AS Keterangan" & vbCrLf & _
                     " FROM MSaldoAwalHutangPiutang INNER JOIN MAlamat ON MAlamat.NoID=MSaldoAwalHutangPiutang.IDAlamat" & vbCrLf & _
                     " WHERE MSaldoAwalHutangPiutang.IsPosted=1 AND MAlamat.NoID IN (SELECT MGroupSupplierD.IDAlamat FROM MGroupSupplierD WHERE MGroupSupplierD.IDGroupSupplier=" & IDAlamat & ") AND MSaldoAwalHutangPiutang.Tanggal>='" & TglDari.DateTime.ToString("yyyy-MM-dd") & "' AND MSaldoAwalHutangPiutang.Tanggal<'" & TglSampai.DateTime.AddDays(1).ToString("yyyy-MM-dd") & "'" & vbCrLf & _
                     " UNION ALL" & vbCrLf & _
                     " SELECT 2 AS IDJenisTransaksi, DATEADD(dd, 0, DATEDIFF(dd, 0, MBeli.Tanggal)) AS Tanggal, MAlamat.Kode AS KodeSupplier, MAlamat.Nama AS Supplier, MBeli.Kode AS NoNota, MBeli.Total AS Masuk, 0 AS Keluar, 'Pembelian' AS Keterangan" & vbCrLf & _
                     " FROM MBeli INNER JOIN MAlamat ON MAlamat.NoID=MBeli.IDSupplier" & vbCrLf & _
                     " WHERE MBeli.IsPosted=1 AND MAlamat.NoID IN (SELECT MGroupSupplierD.IDAlamat FROM MGroupSupplierD WHERE MGroupSupplierD.IDGroupSupplier=" & IDAlamat & ") AND MBeli.Tanggal>='2012-12-01' AND MBeli.Tanggal>='" & TglDari.DateTime.ToString("yyyy-MM-dd") & "' AND MBeli.Tanggal<'" & TglSampai.DateTime.AddDays(1).ToString("yyyy-MM-dd") & "'" & vbCrLf & _
                     " UNION ALL" & vbCrLf & _
                     " SELECT 3 AS IDJenisTransaksi, DATEADD(dd, 0, DATEDIFF(dd, 0, MBayarHutang.TglKembali)) AS Tanggal, MAlamat.Kode AS KodeSupplier, MAlamat.Nama AS Supplier, MReturBeli.Kode AS NoNota, 0, MReturBeli.Total AS Keluar, 'Retur Pembelian' AS Keterangan" & vbCrLf & _
                     " FROM MReturBeli INNER JOIN MAlamat ON MAlamat.NoID=MReturBeli.IDSupplier" & vbCrLf & _
                     " INNER JOIN (MBayarHutang INNER JOIN MBayarHutangDRetur ON MBayarHutang.NoID=MBayarHutangDRetur.IDBayarHutang) ON MBayarHutangDRetur.IDReturBeli=MReturBeli.NoID" & vbCrLf & _
                     " WHERE MReturBeli.IsPosted=1 AND MAlamat.NoID IN (SELECT MGroupSupplierD.IDAlamat FROM MGroupSupplierD WHERE MGroupSupplierD.IDGroupSupplier=" & IDAlamat & ") AND MBayarHutang.TglKembali>='2012-12-01' AND MBayarHutang.TglKembali>='" & TglDari.DateTime.ToString("yyyy-MM-dd") & "' AND MBayarHutang.TglKembali<'" & TglSampai.DateTime.AddDays(1).ToString("yyyy-MM-dd") & "'" & vbCrLf & _
                     " UNION ALL " & vbCrLf & _
                     " SELECT 17 AS IDJenisTransaksi, DATEADD(dd, 0, DATEDIFF(dd, 0, MBayarHutang.TglKembali)) AS Tanggal, MAlamat.Kode AS KodeSupplier, MAlamat.Nama AS Supplier, MBayarHutang.Kode AS NoNota, 0, MBayarHutang.Total AS Masuk, 'Pembayaran Hutang' AS Keterangan" & vbCrLf & _
                     " FROM MBayarHutang INNER JOIN MAlamat ON MAlamat.NoID=MBayarHutang.IDAlamat" & vbCrLf & _
                     " WHERE MBayarHutang.IsPosted=1 AND MAlamat.NoID IN (SELECT MGroupSupplierD.IDAlamat FROM MGroupSupplierD WHERE MGroupSupplierD.IDGroupSupplier=" & IDAlamat & ") AND MBayarHutang.TglKembali>='2012-12-01' AND MBayarHutang.TglKembali>='" & TglDari.DateTime.ToString("yyyy-MM-dd") & "' AND MBayarHutang.TglKembali<'" & TglSampai.DateTime.AddDays(1).ToString("yyyy-MM-dd") & "'" & vbCrLf & _
                     " UNION ALL" & vbCrLf & _
                     " SELECT 17 AS IDJenisTransaksi, DATEADD(dd, 0, DATEDIFF(dd, 0, MBayarHutang.TglKembali)) AS Tanggal, MAlamat.Kode AS KodeSupplier, MAlamat.Nama AS Supplier, MBayarHutang.Kode AS NoNota, SUM(MBayarHutangDDebet.Potong) AS Masuk, 0, 'DN Nota' AS Keterangan" & vbCrLf & _
                     " FROM (MBayarHutang INNER JOIN MBayarHutangDDebet ON MBayarHutang.NoID=MBayarHutangDDebet.IDBayarHutang) INNER JOIN MAlamat ON MAlamat.NoID=MBayarHutang.IDAlamat" & vbCrLf & _
                     " WHERE MBayarHutang.IsPosted=1 AND MAlamat.NoID IN (SELECT MGroupSupplierD.IDAlamat FROM MGroupSupplierD WHERE MGroupSupplierD.IDGroupSupplier=" & IDAlamat & ") AND MBayarHutang.TglKembali>='2012-12-01' AND MBayarHutang.TglKembali>='" & TglDari.DateTime.ToString("yyyy-MM-dd") & "' AND MBayarHutang.TglKembali<'" & TglSampai.DateTime.AddDays(1).ToString("yyyy-MM-dd") & "'" & vbCrLf & _
                     " GROUP BY MBayarHutang.TglKembali, MAlamat.Kode, MAlamat.Nama, MBayarHutang.Kode, MBayarHutang.DN" & vbCrLf & _
                     " UNION ALL" & vbCrLf & _
                     " SELECT 17 AS IDJenisTransaksi, DATEADD(dd, 0, DATEDIFF(dd, 0, MBayarHutang.TglKembali)) AS Tanggal, MAlamat.Kode AS KodeSupplier, MAlamat.Nama AS Supplier, MBayarHutang.Kode AS NoNota, 0, ISNULL(SUM(MBayarHutangDKredit.Potong),0) AS Keluar, 'CN Nota' AS Keterangan" & vbCrLf & _
                     " FROM (MBayarHutang INNER JOIN MBayarHutangDKredit ON MBayarHutang.NoID=MBayarHutangDKredit.IDBayarHutang) INNER JOIN MAlamat ON MAlamat.NoID=MBayarHutang.IDAlamat" & vbCrLf & _
                     " WHERE MBayarHutang.IsPosted=1 AND MAlamat.NoID IN (SELECT MGroupSupplierD.IDAlamat FROM MGroupSupplierD WHERE MGroupSupplierD.IDGroupSupplier=" & IDAlamat & ") AND MBayarHutang.TglKembali>='2012-12-01' AND MBayarHutang.TglKembali>='" & TglDari.DateTime.ToString("yyyy-MM-dd") & "' AND MBayarHutang.TglKembali<'" & TglSampai.DateTime.AddDays(1).ToString("yyyy-MM-dd") & "'" & vbCrLf & _
                     " GROUP BY MBayarHutang.TglKembali, MAlamat.Kode, MAlamat.Nama, MBayarHutang.Kode, MBayarHutang.Potongan" & vbCrLf & _
                     " UNION ALL" & vbCrLf & _
                     " SELECT 17 AS IDJenisTransaksi, DATEADD(dd, 0, DATEDIFF(dd, 0, MBayarHutang.TglKembali)) AS Tanggal, MAlamat.Kode AS KodeSupplier, MAlamat.Nama AS Supplier, MBayarHutang.Kode AS NoNota, 0, IsNull(MBayarHutang.Materai,0) AS Keluar, 'Materai Pembayaran' AS Keterangan" & vbCrLf & _
                     " FROM MBayarHutang INNER JOIN MAlamat ON MAlamat.NoID=MBayarHutang.IDAlamat" & vbCrLf & _
                     " WHERE MBayarHutang.IsPosted=1 AND MAlamat.NoID IN (SELECT MGroupSupplierD.IDAlamat FROM MGroupSupplierD WHERE MGroupSupplierD.IDGroupSupplier=" & IDAlamat & ") AND MBayarHutang.TglKembali>='2012-12-01' AND MBayarHutang.TglKembali>='" & TglDari.DateTime.ToString("yyyy-MM-dd") & "' AND MBayarHutang.TglKembali<'" & TglSampai.DateTime.AddDays(1).ToString("yyyy-MM-dd") & "' AND IsNull(MBayarHutang.Materai,0)<>0" & vbCrLf & _
                     " UNION ALL" & vbCrLf & _
                     " SELECT 17 AS IDJenisTransaksi, DATEADD(dd, 0, DATEDIFF(dd, 0, MBayarHutang.TglKembali)) AS Tanggal, MAlamat.Kode AS KodeSupplier, MAlamat.Nama AS Supplier, MBayarHutang.Kode AS NoNota, 0, IsNull(MBayarHutang.JumlahKwitansi,0) AS Keluar, 'Kwitansi Pembayaran' AS Keterangan" & vbCrLf & _
                     " FROM MBayarHutang INNER JOIN MAlamat ON MAlamat.NoID=MBayarHutang.IDAlamat" & vbCrLf & _
                     " WHERE MBayarHutang.IsPosted=1 AND MAlamat.NoID IN (SELECT MGroupSupplierD.IDAlamat FROM MGroupSupplierD WHERE MGroupSupplierD.IDGroupSupplier=" & IDAlamat & ") AND MBayarHutang.TglKembali>='2012-12-01' AND MBayarHutang.TglKembali>='" & TglDari.DateTime.ToString("yyyy-MM-dd") & "' AND MBayarHutang.TglKembali<'" & TglSampai.DateTime.AddDays(1).ToString("yyyy-MM-dd") & "' AND IsNull(MBayarHutang.JumlahKwitansi,0)<>0" & vbCrLf
            strsql = "SELECT KartuHutang.*, 0.0000 AS SaldoAwal, 0.0000 AS SaldoAkhir, MJenisTransaksi.Nama AS JenisTransaksi FROM (" & strsql & ") AS KartuHutang LEFT JOIN MJenisTransaksi ON MJenisTransaksi.ID=KartuHutang.IDJenisTransaksi ORDER BY DATEADD(dd, 0, DATEDIFF(dd, 0, KartuHutang.Tanggal)), MJenisTransaksi.NoUrut"

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
                    'If .Columns(i).FieldName.Length >= 4 AndAlso .Columns(i).FieldName.Substring(0, 4).ToLower = "Kode".ToLower Then
                    '    .Columns(i).Fixed = FixedStyle.Left
                    'ElseIf (.Columns(i).FieldName.Length >= 3 AndAlso .Columns(i).FieldName.Substring(0, 3).ToLower = "Qty".ToLower) Or .Columns(i).FieldName.Length >= 5 AndAlso .Columns(i).FieldName.Substring(0, 5).ToLower = "Total".ToLower Then
                    '    .Columns(i).GroupFormat.FormatType = FormatType.Numeric
                    '    .Columns(i).GroupFormat.FormatString = "{0:n2}"
                    'ElseIf .Columns(i).FieldName.ToLower = "Nama".ToLower Then
                    '    .Columns(i).Fixed = FixedStyle.Left
                    'End If
                Next
            End With

            HitungSaldo()
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

    End Sub
    Private Sub frBayarHutang_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        TglDari.DateTime = CDate(TanggalSystem.ToString("yyyy,MM,01"))
        TglSampai.DateTime = TanggalSystem
        SetTombol()
        RefreshLookUp()
        RefreshItem()
        'HitungSaldo()
        FungsiControl.SetForm(Me)
        TglDari.Properties.EditMask = "dd-MM-yyyy"
        TglDari.Properties.Mask.MaskType = Mask.MaskType.DateTime
        TglDari.Properties.Mask.UseMaskAsDisplayFormat = True
        TglSampai.Properties.EditMask = "dd-MM-yyyy"
        TglSampai.Properties.Mask.MaskType = Mask.MaskType.DateTime
        TglSampai.Properties.Mask.UseMaskAsDisplayFormat = True
        lbDaftar.Text = "Laporan Kartu Hutang Per Group Supplier"
        If Dir(FolderLayouts & "\LaporanBayarHutangPerSupplier.xml") <> "" Then
            GridView1.RestoreLayoutFromXml(FolderLayouts & "\LaporanBayarHutangPerSupplier.xml")
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

    Private Sub HitungSaldo()
        Dim SQL As String = ""
        Dim ds As New DataSet
        Dim SaldoAwal As Double = 0.0
        Dim SaldoAkhir As Double = 0.0
        Try
            IDAlamat = NullToLong(txtAlamat.EditValue)
            SQL = "SELECT MSaldoAwalHutangPiutang.SaldoAwal-0 AS Total" & vbCrLf & _
                  " FROM MSaldoAwalHutangPiutang INNER JOIN MAlamat ON MAlamat.NoID=MSaldoAwalHutangPiutang.IDAlamat" & vbCrLf & _
                  " WHERE MSaldoAwalHutangPiutang.IsPosted=1 AND MAlamat.NoID IN (SELECT MGroupSupplierD.IDAlamat FROM MGroupSupplierD WHERE MGroupSupplierD.IDGroupSupplier=" & IDAlamat & ") AND MSaldoAwalHutangPiutang.Tanggal<'" & TglDari.DateTime.ToString("yyyy-MM-dd") & "'" & vbCrLf & _
                  " UNION ALL " & vbCrLf & _
                  " SELECT MBeli.Total-0 AS Total" & vbCrLf & _
                  " FROM MBeli INNER JOIN MAlamat ON MAlamat.NoID=MBeli.IDSupplier" & vbCrLf & _
                  " WHERE MBeli.IsPosted=1 AND MAlamat.NoID IN (SELECT MGroupSupplierD.IDAlamat FROM MGroupSupplierD WHERE MGroupSupplierD.IDGroupSupplier=" & IDAlamat & ") AND MBeli.Tanggal>='2012-12-01' AND MBeli.Tanggal<'" & TglDari.DateTime.ToString("yyyy-MM-dd") & "'" & vbCrLf & _
                  " UNION ALL" & vbCrLf & _
                  " SELECT 0-MReturBeli.Total" & vbCrLf & _
                  " FROM MReturBeli INNER JOIN MAlamat ON MAlamat.NoID=MReturBeli.IDSupplier" & vbCrLf & _
                  " INNER JOIN (MBayarHutang INNER JOIN MBayarHutangDRetur ON MBayarHutang.NoID=MBayarHutangDRetur.IDBayarHutang) ON MBayarHutangDRetur.IDReturBeli=MReturBeli.NoID" & vbCrLf & _
                  " WHERE MReturBeli.IsPosted=1 AND MAlamat.NoID IN (SELECT MGroupSupplierD.IDAlamat FROM MGroupSupplierD WHERE MGroupSupplierD.IDGroupSupplier=" & IDAlamat & ") AND MBayarHutang.TglKembali>='2012-12-01' AND MBayarHutang.TglKembali<'" & TglDari.DateTime.ToString("yyyy-MM-dd") & "'" & vbCrLf & _
                  " UNION ALL " & vbCrLf & _
                  " SELECT 0-MBayarHutang.Total AS Masuk" & vbCrLf & _
                  " FROM MBayarHutang INNER JOIN MAlamat ON MAlamat.NoID=MBayarHutang.IDAlamat" & vbCrLf & _
                  " WHERE MBayarHutang.IsPosted=1 AND MAlamat.NoID IN (SELECT MGroupSupplierD.IDAlamat FROM MGroupSupplierD WHERE MGroupSupplierD.IDGroupSupplier=" & IDAlamat & ") AND MBayarHutang.TglKembali>='2012-12-01' AND MBayarHutang.TglKembali<'" & TglDari.DateTime.ToString("yyyy-MM-dd") & "'" & vbCrLf & _
                  " UNION ALL" & vbCrLf & _
                  " SELECT MBayarHutangDDebet.Potong-0 AS Masuk" & vbCrLf & _
                  " FROM (MBayarHutang INNER JOIN MBayarHutangDDebet ON MBayarHutang.NoID=MBayarHutangDDebet.IDBayarHutang) INNER JOIN MAlamat ON MAlamat.NoID=MBayarHutang.IDAlamat" & vbCrLf & _
                  " WHERE MBayarHutang.IsPosted=1 AND MAlamat.NoID IN (SELECT MGroupSupplierD.IDAlamat FROM MGroupSupplierD WHERE MGroupSupplierD.IDGroupSupplier=" & IDAlamat & ") AND MBayarHutang.TglKembali>='2012-12-01' AND MBayarHutang.TglKembali<'" & TglDari.DateTime.ToString("yyyy-MM-dd") & "'" & vbCrLf & _
                  " UNION ALL" & vbCrLf & _
                  " SELECT 0-MBayarHutangDKredit.Potong AS Keluar" & vbCrLf & _
                  " FROM (MBayarHutang INNER JOIN MBayarHutangDKredit ON MBayarHutang.NoID=MBayarHutangDKredit.IDBayarHutang) INNER JOIN MAlamat ON MAlamat.NoID=MBayarHutang.IDAlamat" & vbCrLf & _
                  " WHERE MBayarHutang.IsPosted=1 AND MAlamat.NoID IN (SELECT MGroupSupplierD.IDAlamat FROM MGroupSupplierD WHERE MGroupSupplierD.IDGroupSupplier=" & IDAlamat & ") AND MBayarHutang.TglKembali>='2012-12-01' AND MBayarHutang.TglKembali<'" & TglDari.DateTime.ToString("yyyy-MM-dd") & "'" & vbCrLf & _
                  " UNION ALL" & vbCrLf & _
                  " SELECT 0-IsNull(MBayarHutang.Materai,0) AS Keluar" & vbCrLf & _
                  " FROM MBayarHutang INNER JOIN MAlamat ON MAlamat.NoID=MBayarHutang.IDAlamat" & vbCrLf & _
                  " WHERE MBayarHutang.IsPosted=1 AND MAlamat.NoID IN (SELECT MGroupSupplierD.IDAlamat FROM MGroupSupplierD WHERE MGroupSupplierD.IDGroupSupplier=" & IDAlamat & ") AND MBayarHutang.TglKembali>='2012-12-01' AND MBayarHutang.TglKembali<'" & TglDari.DateTime.ToString("yyyy-MM-dd") & "' AND IsNull(MBayarHutang.Materai,0)<>0" & vbCrLf & _
                  " UNION ALL" & vbCrLf & _
                  " SELECT 0-IsNull(MBayarHutang.JumlahKwitansi,0) AS Keluar" & vbCrLf & _
                  " FROM MBayarHutang INNER JOIN MAlamat ON MAlamat.NoID=MBayarHutang.IDAlamat" & vbCrLf & _
                  " WHERE MBayarHutang.IsPosted=1 AND MAlamat.NoID IN (SELECT MGroupSupplierD.IDAlamat FROM MGroupSupplierD WHERE MGroupSupplierD.IDGroupSupplier=" & IDAlamat & ") AND MBayarHutang.TglKembali>='2012-12-01' AND MBayarHutang.TglKembali<'" & TglDari.DateTime.ToString("yyyy-MM-dd") & "' AND IsNull(MBayarHutang.JumlahKwitansi,0)<>0" & vbCrLf
            SQL = "SELECT SUM(KartuHutang.Total) AS Saldo FROM (" & SQL & ") AS KartuHutang"
            ds = ExecuteDataset("Data", SQL)
            If ds.Tables("Data").Rows.Count >= 1 Then
                SaldoAwal = NullToDbl(ds.Tables("Data").Rows(0).Item("Saldo"))
                txtSaldoAwal.EditValue = NullToDbl(ds.Tables("Data").Rows(0).Item("Saldo"))
                SaldoAkhir = SaldoAwal
                For i As Integer = 0 To GridView1.RowCount - 1
                    SaldoAwal = SaldoAkhir
                    SaldoAkhir = SaldoAwal + NullToDbl(GridView1.GetRowCellValue(i, "Masuk")) - NullToDbl(GridView1.GetRowCellValue(i, "Keluar"))
                    GridView1.SetRowCellValue(i, "SaldoAwal", SaldoAwal)
                    GridView1.SetRowCellValue(i, "SaldoAkhir", SaldoAkhir)
                Next
                txtSaldoAkhir.EditValue = SaldoAkhir
            Else
                txtSaldoAwal.EditValue = 0
                txtSaldoAkhir.EditValue = 0
            End If
        Catch ex As Exception
            XtraMessageBox.Show("Info Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
        Finally
            ds.Dispose()
        End Try
    End Sub

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
        Dim xOtorisasi As New frmOtorisasiAdmin
        Try
            If XtraMessageBox.Show("Simpan Layout?", NamaAplikasi, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = Windows.Forms.DialogResult.Yes AndAlso xOtorisasi.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
                GridView1.SaveLayoutToXml(FolderLayouts & "\LaporanBayarHutangPerSupplier.xml")
                gv1.SaveLayoutToXml(FolderLayouts & Me.Name & gv1.Name & ".xml")
            End If
        Catch ex As Exception
            XtraMessageBox.Show("Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            xOtorisasi.Dispose()
        End Try
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
        GridControl1.ShowPrintPreview()
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
        TglDari.Enabled = Not TglDari.Enabled
        TglSampai.Enabled = TglDari.Enabled
    End Sub

    Private Sub CheckEdit1_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckEdit1.CheckedChanged
        RefreshItem()
    End Sub

    Private Sub gv1_DataSourceChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gv1.DataSourceChanged
        If System.IO.File.Exists(FolderLayouts & Me.Name & gv1.Name & ".xml") Then
            gv1.RestoreLayoutFromXml(FolderLayouts & Me.Name & gv1.Name & ".xml")
        End If
        With gv1
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

    Private Sub txtAlamat_EditValueChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtAlamat.EditValueChanged
        RefreshItem()
    End Sub
End Class