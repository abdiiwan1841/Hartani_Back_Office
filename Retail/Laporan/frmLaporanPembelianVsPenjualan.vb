Imports System.Data.SqlClient 
Imports System.Windows.Forms
Imports DevExpress.XtraEditors
Imports DevExpress.XtraGrid.Views
Imports DevExpress.XtraGrid.Columns
Imports DevExpress.XtraGrid.Views.Base
Imports System.Drawing
Imports DevExpress.XtraEditors.Repository
Imports VPoint.mdlCetakCR 
Imports DevExpress.Utils
Imports DevExpress.XtraGrid.Views.Grid.ViewInfo
Imports DevExpress.XtraGrid.Views.Grid
Imports DevExpress.Utils.Menu
Imports DevExpress.XtraGrid.Localization 
Public Class frmLaporanPembelianvsPenjualan
    Public FormName As String = "LaporanPembelianvsPenjualan"
    Public FormEntriName As String = ""
    Public TableName As String = ""
    Public TableNameD As String = ""
    Public TableMaster As String = ""
    Public NoID As Long = -1
    Dim IDBarang As Long = -1
    Dim oda2 As SqlDataAdapter 

    Dim ds As New DataSet
    Dim BS As New BindingSource 

    Dim repckedit As New RepositoryItemCheckEdit
    Dim repdateedit As New RepositoryItemDateEdit
    Dim reptextedit As New RepositoryItemTextEdit
    Dim reppicedit As New RepositoryItemPictureEdit
    Public ShowNoID As Boolean = False
    Public DirectNoID As Long = -1
     
    Private Sub frmDaftarMasterDetil_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Activated
        If ShowNoID Then
            RefreshData()
            GV1.ClearSelection()
            GV1.FocusedRowHandle = GV1.LocateByDisplayText(0, GV1.Columns("NoID"), (DirectNoID).ToString("#,##0"))
            GV1.SelectRow(GV1.FocusedRowHandle)
            ShowNoID = False
        End If
    End Sub

    Private Sub ctrlDaftar_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim dlg As DevExpress.Utils.WaitDialogForm = Nothing
        Try
            Dim curentcursor As Cursor = Windows.Forms.Cursor.Current
            Windows.Forms.Cursor.Current = Cursors.WaitCursor
            dlg = New DevExpress.Utils.WaitDialogForm(String.Format("Creating component and analize database.{0}MOHON TUNGGU ...", vbCrLf), Application.ProductName)
            dlg.TopMost = False
            dlg.Show()
            Windows.Forms.Cursor.Current = Cursors.WaitCursor
            TglDari.DateTime = CDate(TanggalSystem.ToString("yyyy/MM/01"))
            TglSampai.DateTime = CDate(TanggalSystem.ToString("yyyy/MM/dd"))
            RefreshLookUp()
            Me.lbDaftar.Text = Me.Text
            XtraTabPage1.Text = Me.Text
            RefreshData()
            RefreshDataKumulatif()
            RestoreLayout()
            FungsiControl.SetForm(Me)

        Catch ex As Exception
            XtraMessageBox.Show(ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            Windows.Forms.Cursor.Current = Cursors.Default
            dlg.Close()
            dlg.Dispose()
        End Try
    End Sub

    Private Sub RefreshLookUp()
        Dim ds As New DataSet
        Dim SQL As String = ""
        Try
            SQL = "SELECT NoID, Kode, Nama FROM MKategori WHERE IsActive=1  "
            ds = ExecuteDataset("MKategori", SQL)
            txtKategori.Properties.DataSource = ds.Tables("MKategori")
            txtKategori.Properties.DisplayMember = "Nama"
            txtKategori.Properties.ValueMember = "NoID"
            SQL = "SELECT NoID, Kode, Nama FROM MAlamat  WHERE IsActive=1 and IsSupplier=1 "
            ds = ExecuteDataset("MSupplier", SQL)
            txtSupplier.Properties.DataSource = ds.Tables("MSupplier")
            txtSupplier.Properties.DisplayMember = "Nama"
            txtSupplier.Properties.ValueMember = "NoID"

        Catch ex As Exception
            XtraMessageBox.Show("Info : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
        Finally
            ds.Dispose()
        End Try
    End Sub
    Private Sub RestoreLayout()
        With GV1
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
                        ElseIf .Columns(i).FieldName.Trim.ToLower = "tanggalstart" Or .Columns(i).FieldName.Trim.ToLower = "tanggalend" Then
                            .Columns(i).DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime
                            .Columns(i).DisplayFormat.FormatString = "dd-MM-yyyy HH:mm"
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


        With gvKumulatif
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
                        ElseIf .Columns(i).FieldName.Trim.ToLower = "tanggalstart" Or .Columns(i).FieldName.Trim.ToLower = "tanggalend" Then
                            .Columns(i).DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime
                            .Columns(i).DisplayFormat.FormatString = "dd-MM-yyyy HH:mm"
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
        If System.IO.File.Exists(FolderLayouts & FormName & ".xml") Then
            GV1.RestoreLayoutFromXml(FolderLayouts & FormName & ".xml")
        End If

        If System.IO.File.Exists(FolderLayouts & FormName & "_Kumulatif.xml") Then
            gvKumulatif.RestoreLayoutFromXml(FolderLayouts & FormName & "_Kumulatif.xml")
        End If
    End Sub

    Private Sub SimpleButton6_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SimpleButton6.Click
        Tutup()
    End Sub

    Public Sub New()
        ' This call is required by the Windows Form Designer.
        InitializeComponent()
        ' Add any initialization after the InitializeComponent() call.
    End Sub

    Sub RefreshData()
        Dim dlg As DevExpress.Utils.WaitDialogForm = Nothing
        Dim cn As New SqlConnection(StrKonSql)
        Dim ocmd2 As New SqlCommand
        Dim Cur As Cursor = Windows.Forms.Cursor.Current
        Try
            Dim curentcursor As Cursor = Windows.Forms.Cursor.Current
            Windows.Forms.Cursor.Current = Cursors.WaitCursor
            dlg = New DevExpress.Utils.WaitDialogForm(String.Format("Sedang merefresh data.{0}MOHON TUNGGU ...", vbCrLf), Application.ProductName)
            dlg.TopMost = False
            dlg.Show()
            Windows.Forms.Cursor.Current = Cursors.WaitCursor

            Dim strsql As String = ""
            Dim SQL As String = ""
            Dim isperluwhere As Boolean = True
            strsql = "SELECT MBarangD.NoID, MBarangD.IDBarang, MBarang.Kode, MBarang.Nama, MBarangD.Barcode, MKategori.Nama AS Kategori, " & vbCrLf
            If TglDari.Enabled Then
                strsql &= " (SELECT SUM(MBeliD.Qty*MBeliD.Konversi) FROM MBeli INNER JOIN MBeliD ON MBeli.NoID=MBeliD.IDBeli WHERE MBeliD.IDBarang=MBarangD.IDBarang AND MBeliD.IDBarangD=MBarangD.NoID AND MBeli.Tanggal>='" & TglDari.DateTime.ToString("yyyy/MM/dd") & "' AND MBeli.Tanggal<'" & TglSampai.DateTime.AddDays(1).ToString("yyyy/MM/dd") & "') AS QtyPembelian," & vbCrLf & _
                          " (SELECT SUM(MJualD.Qty*MJualD.Konversi) FROM MJual INNER JOIN MJualD ON MJual.NoID=MJualD.IDJual WHERE MJualD.IDBarang=MBarangD.IDBarang AND MJualD.IDBarangD=MBarangD.NoID AND MJual.Tanggal>='" & TglDari.DateTime.ToString("yyyy/MM/dd") & "' AND MJual.Tanggal<'" & TglSampai.DateTime.AddDays(1).ToString("yyyy/MM/dd") & "') AS QtyPenjualan, " & vbCrLf
                strsql &= " (SELECT SUM(MBeliD.Jumlah) FROM MBeli INNER JOIN MBeliD ON MBeli.NoID=MBeliD.IDBeli WHERE MBeliD.IDBarang=MBarangD.IDBarang AND MBeliD.IDBarangD=MBarangD.NoID AND MBeli.Tanggal>='" & TglDari.DateTime.ToString("yyyy/MM/dd") & "' AND MBeli.Tanggal<'" & TglSampai.DateTime.AddDays(1).ToString("yyyy/MM/dd") & "') AS JumlahPembelian," & vbCrLf & _
                          " (SELECT SUM(MJualD.Jumlah) FROM MJual INNER JOIN MJualD ON MJual.NoID=MJualD.IDJual WHERE MJualD.IDBarang=MBarangD.IDBarang AND MJualD.IDBarangD=MBarangD.NoID AND MJual.Tanggal>='" & TglDari.DateTime.ToString("yyyy/MM/dd") & "' AND MJual.Tanggal<'" & TglSampai.DateTime.AddDays(1).ToString("yyyy/MM/dd") & "') AS JumlahPenjualan " & vbCrLf
            Else
                strsql &= " (SELECT SUM(MBeliD.Qty*MBeliD.Konversi) FROM MBeli INNER JOIN MBeliD ON MBeli.NoID=MBeliD.IDBeli WHERE MBeliD.IDBarang=MBarangD.IDBarang AND MBeliD.IDBarangD=MBarangD.NoID) AS QtyPembelian," & vbCrLf & _
                          " (SELECT SUM(MJualD.Qty*MJualD.Konversi) FROM MJual INNER JOIN MJualD ON MJual.NoID=MJualD.IDJual WHERE MJualD.IDBarang=MBarangD.IDBarang AND MJualD.IDBarangD=MBarangD.NoID) AS QtyPenjualan, " & vbCrLf
                strsql &= " (SELECT SUM(MBeliD.Jumlah) FROM MBeli INNER JOIN MBeliD ON MBeli.NoID=MBeliD.IDBeli WHERE MBeliD.IDBarang=MBarangD.IDBarang AND MBeliD.IDBarangD=MBarangD.NoID) AS JumlahPembelian," & vbCrLf & _
                          " (SELECT SUM(MJualD.Jumlah) FROM MJual INNER JOIN MJualD ON MJual.NoID=MJualD.IDJual WHERE MJualD.IDBarang=MBarangD.IDBarang AND MJualD.IDBarangD=MBarangD.NoID) AS JumlahPenjualan " & vbCrLf
            End If
            strsql &= " FROM MBarang " & vbCrLf & _
                      " INNER JOIN MBarangD ON MBarang.NoID=MBarangD.IDBarang" & vbCrLf & _
                      " INNER JOIN MKategori ON MKategori.NoID=MBarang.IDKategori"
            If txtKodeBarang.Text <> "" And txtKodeBarang.Enabled Then
                If isperluwhere Then
                    strsql &= " where"
                Else
                    strsql &= " AND"
                End If
                strsql &= " MBarang.NoID= " & NullToLong(IDBarang)
                isperluwhere = False
            End If
            If txtKategori.Enabled Then
                If isperluwhere Then
                    strsql &= " where"
                Else
                    strsql &= " AND"
                End If
                strsql = strsql & " MBarang.IDKategori= " & NullToLong(txtKategori.EditValue)
                isperluwhere = False
            End If
            If txtSupplier.Enabled Then
                If isperluwhere Then
                    strsql &= " where"
                Else
                    strsql &= " AND"
                End If
                strsql &= " (MBarang.IDSupplier1=" & NullToLong(txtSupplier.EditValue) & vbCrLf & _
                          " OR MBarang.IDSupplier2=" & NullToLong(txtSupplier.EditValue) & vbCrLf & _
                          " OR MBarang.IDSupplier3=" & NullToLong(txtSupplier.EditValue) & vbCrLf & _
                          " OR MBarang.IDSupplier4=" & NullToLong(txtSupplier.EditValue) & vbCrLf & _
                          " OR MBarang.IDSupplier5=" & NullToLong(txtSupplier.EditValue) & ")"
                isperluwhere = False
            End If

            ocmd2.Connection = cn
            ocmd2.CommandType = CommandType.Text
            ocmd2.CommandText = strsql
            SQL = strsql

            cn.Open()
            oda2 = New SqlDataAdapter(ocmd2)
            oda2.TableMappings.Add("Tabel", "Data")
            If ds.Tables("Data") Is Nothing Then
            Else
                ds.Tables("Data").Clear()
            End If
            oda2.Fill(ds, "Data")
            BS.DataSource = ds.Tables("Data")
            GC1.DataSource = BS.DataSource

        Catch ex As Exception
            XtraMessageBox.Show("Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            ocmd2.Dispose()
            cn.Close()
            cn.Dispose()
            Windows.Forms.Cursor.Current = Cur
            Windows.Forms.Cursor.Current = Cursors.Default
            dlg.Close()
            dlg.Dispose()

        End Try
    End Sub
    Sub RefreshDataKumulatif()
        Dim dlg As DevExpress.Utils.WaitDialogForm = Nothing
        Dim cn As New SqlConnection(StrKonSql)
        Dim ocmd2 As New SqlCommand

        Dim curentcursor As Cursor = Windows.Forms.Cursor.Current
        Try
            Windows.Forms.Cursor.Current = Cursors.WaitCursor
            dlg = New DevExpress.Utils.WaitDialogForm(String.Format("Sedang merefresh data.{0}MOHON TUNGGU ...", vbCrLf), Application.ProductName)
            dlg.TopMost = False
            dlg.Show()
            Windows.Forms.Cursor.Current = Cursors.WaitCursor

            Dim strsql As String = ""
            Dim SQL As String = ""
            Dim isperluwhere As Boolean = True
            strsql = "SELECT MLaporanBelivsJualD.*,MBarang.Kode, MBarang.Nama, MBarang.Barcode, MKategori.Nama AS Kategori  " & vbCrLf
            strsql &= " from MLaporanBelivsJualD left join MBarang on MLaporanBelivsJualD.IDbarang=Mbarang.NoID " & vbCrLf & _
                      " left join Mkategori on MBarang.IDkategori=MKategori.NoID where MLaporanBelivsJualD.IDUser=" & IDUserAktif

            ocmd2.Connection = cn
            ocmd2.CommandType = CommandType.Text
            ocmd2.CommandText = strsql
            SQL = strsql

            cn.Open()
            oda2 = New SqlDataAdapter(ocmd2)
            oda2.TableMappings.Add("Tabel", "DataKumulatif")
            If ds.Tables("DataKumulatif") Is Nothing Then
            Else
                ds.Tables("DataKumulatif").Clear()
            End If
            oda2.Fill(ds, "DataKumulatif")
            BS.DataSource = ds.Tables("DataKumulatif")
            GridControl1.DataSource = BS.DataSource

        Catch ex As Exception
            XtraMessageBox.Show("Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            ocmd2.Dispose()
            cn.Close()
            cn.Dispose()
            Windows.Forms.Cursor.Current = curentcursor
            dlg.Close()
            dlg.Dispose()

        End Try
    End Sub
    Private Sub BarButtonItem4_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem4.ItemClick
        Tutup()
    End Sub
    Sub Tutup()
        DialogResult = Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub SimpleButton8_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SimpleButton8.Click
        RefreshData()
        RefreshDataKumulatif()
    End Sub

    Private Sub BarButtonItem6_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem6.ItemClick
        PrintPreview()
    End Sub
    Sub ExportExcel()
        Dim dlgsave As New SaveFileDialog
        dlgsave.Title = "Export Daftar ke Excel"
        dlgsave.Filter = "Excel Files|*.xls"
        If dlgsave.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
            GV1.ExportToXls(dlgsave.FileName)
            BukaFile(dlgsave.FileName)
        End If
        dlgsave.Dispose()
    End Sub
    Sub PrintPreview()
        GC1.ShowPrintPreview()
    End Sub
    Private Sub CetakMRPTJual(ByVal Action As action_)

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

    Private Sub LabelControl1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LabelControl1.Click
        TglDari.Enabled = Not TglDari.Enabled
        TglSampai.Enabled = TglDari.Enabled
    End Sub

    Private Sub CetakFaktur(ByVal action As action_)
        Dim namafile As String
        Dim view As ColumnView = GC1.FocusedView
        Try
            namafile = Application.StartupPath & "\report\Faktur" & TableMaster & ".rpt"
            If System.IO.File.Exists(namafile) Then
                Dim row As System.Data.DataRow = view.GetDataRow(GV1.FocusedRowHandle)
                Dim dc As Integer = GV1.FocusedRowHandle
                If row Is Nothing Then XtraMessageBox.Show("Untuk mencetak, pilih item yang mau dicetak dan click cetak faktur.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information) : Exit Sub
                Dim NoID As Long = NullToLong(row("NoID"))
                'ViewReport(action, namafile, "Faktur Penjualan", "{MJual.NoID}=" & NoID)
                If EditReport Then
                    action = action_.Edit
                Else
                    action = action_.Preview
                End If
                If Not EditReport Then
                    'mnPosting.PerformClick()
                End If
                ViewReport(Me.ParentForm, action, namafile, Me.Text, "{" & TableMaster & ".NoID}=" & NoID & " AND {" & TableMaster & ".IsPosted}=True")
                'CetakCRViewer(action, Me.MdiParent, namafile, "Cetak " & Me.Text, "{MJual.NoID}=" & NoID)
            Else
                DevExpress.XtraEditors.XtraMessageBox.Show("Nama File :" & namafile & " tidak ditemukan.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Stop)
            End If
        Catch EX As Exception
            XtraMessageBox.Show("Untuk mencetak, pilih item yang mau dicetak dan click cetak faktur.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
            'DevExpress.XtraEditors.XtraMessageBox.Show("Kesalahan:" & EX.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub
    Private Sub CetakFakturPanjang(ByVal action As action_)
        Dim namafile As String
        Dim view As ColumnView = GC1.FocusedView
        Try
            namafile = Application.StartupPath & "\report\FakturPanjang" & TableMaster & ".rpt"
            If System.IO.File.Exists(namafile) Then
                Dim row As System.Data.DataRow = view.GetDataRow(GV1.FocusedRowHandle)
                If row Is Nothing Then XtraMessageBox.Show("Untuk mencetak, pilih item yang mau dicetak dan click cetak faktur.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information) : Exit Sub
                Dim dc As Integer = GV1.FocusedRowHandle
                Dim NoID As Long = NullToLong(row("NoID"))

                'ViewReport(action, namafile, "Faktur Penjualan", "{MJual.NoID}=" & NoID)
                If EditReport Then
                    action = action_.Edit
                Else
                    action = action_.Preview
                End If
                If Not EditReport Then
                    'mnPosting.PerformClick()
                End If
                ViewReport(Me.ParentForm, action, namafile, Me.Text, "{" & TableMaster & ".NoID}=" & NoID)
                'CetakCRViewer(action, Me.MdiParent, namafile, "Cetak " & Me.Text, "{MJual.NoID}=" & NoID)
            Else
                DevExpress.XtraEditors.XtraMessageBox.Show("Nama File :" & namafile & " tidak ditemukan.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Stop)
            End If
        Catch EX As Exception
            XtraMessageBox.Show("Untuk mencetak, pilih item yang mau dicetak dan click cetak faktur.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
            'DevExpress.XtraEditors.XtraMessageBox.Show("Kesalahan:" & EX.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub GV1_FocusedRowChanged(ByVal sender As Object, ByVal e As DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs) Handles GV1.FocusedRowChanged
        Try
            If TableMaster.ToUpper = "MBeli".ToUpper Then
                If IsShowStock Then
                    clsPostingKartuStok.LihatHasilPosting(NullToLong(GV1.GetRowCellValue(GV1.FocusedRowHandle, "IDBeli")), TableMaster)
                End If
            ElseIf TableMaster.ToUpper = "MReturBeli".ToUpper Then
                If IsShowStock Then
                    clsPostingKartuStok.LihatHasilPosting(NullToLong(GV1.GetRowCellValue(GV1.FocusedRowHandle, "IDReturBeli")), TableMaster)
                End If
            ElseIf TableMaster.ToUpper = "MRevisiHargaBeli".ToUpper Then
                If IsShowStock Then
                    clsPostingKartuStok.LihatHasilPosting(NullToLong(GV1.GetRowCellValue(GV1.FocusedRowHandle, "IDRevisiHargaBeli")), TableMaster)
                End If
            Else
                If IsShowHasilPostingan Then
                    clsPostingKartuStok.HasilPosting(NullToLong(GV1.GetRowCellValue(GV1.FocusedRowHandle, "NoID")), TableMaster)
                End If
            End If
        Catch ex As Exception
            XtraMessageBox.Show("Info : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
        End Try
    End Sub

    Private Sub GV1_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles GV1.MouseDown
        Dim View As GridView = CType(sender, GridView)
        If View Is Nothing Then Return
        ' obtaining hit info
        Dim hitInfo As GridHitInfo = View.CalcHitInfo(New System.Drawing.Point(e.X, e.Y))
        If (e.Button = Windows.Forms.MouseButtons.Right) And (hitInfo.InRow) And _
          (Not View.IsGroupRow(hitInfo.RowHandle)) Then
            PopupMenu1.ShowPopup(Control.MousePosition)
        End If
    End Sub

    Private Sub mnSaveLayouts_ItemClick(ByVal sender As Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles mnSaveLayouts.ItemClick
        Dim xOtorisasi As New frmOtorisasiAdmin
        Try
            If XtraMessageBox.Show("Simpan Layout?", NamaAplikasi, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = Windows.Forms.DialogResult.Yes AndAlso xOtorisasi.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
                GV1.SaveLayoutToXml(FolderLayouts & FormName & ".xml")
                gvKumulatif.SaveLayoutToXml(FolderLayouts & FormName & "_kumulatif.xml")
            End If
        Catch ex As Exception
            XtraMessageBox.Show("Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            xOtorisasi.Dispose()
        End Try
    End Sub

    Private Sub XtraTabControl1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles XtraTabControl1.Click

    End Sub


    Private Sub mnRefresh_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles mnRefresh.ItemClick
        SimpleButton8.PerformClick()
    End Sub


    Private Sub LabelControl2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LabelControl2.Click
        txtKategori.Enabled = Not txtKategori.Enabled
    End Sub

    Private Sub LabelControl4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LabelControl4.Click
        txtSupplier.Enabled = Not txtSupplier.Enabled
    End Sub

    Private Sub LabelControl6_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LabelControl6.Click
        txtKodeBarang.Enabled = Not txtKodeBarang.Enabled

    End Sub

    Private Sub txtKodeBarang_ButtonClick(ByVal sender As Object, ByVal e As DevExpress.XtraEditors.Controls.ButtonPressedEventArgs) Handles txtKodeBarang.ButtonClick
        If LookupBarangD Is Nothing Then
            LookupBarangD = New frLUBarangD
        End If
        If LookupBarangD.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
            txtKodeBarang.Text = LookupBarangD.Kode
            txtNamaBarang.Text = LookupBarangD.Nama
            IDBarang = LookupBarangD.IDBarang
        End If
    End Sub

    Private Sub txtKodeBarang_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtKodeBarang.EditValueChanged

    End Sub

    Private Sub LabelControl5_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LabelControl5.Click

    End Sub

    Private Sub SimpleButton2_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles SimpleButton2.Click
        EksekusiSQL("delete from MLaporanBelivsJualD where IDUser=" & IDUserAktif)
        RefreshDataKumulatif()
    End Sub

    Private Sub SimpleButton1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles SimpleButton1.Click
        Dim curentcursor As Cursor = Windows.Forms.Cursor.Current
        Try
            Windows.Forms.Cursor.Current = Cursors.WaitCursor
            Dim i As Long
            For i = 0 To ds.Tables("Data").Rows.Count - 1
                EksekusiSQL("delete MLaporanBelivsJualD where IDUser=" & IDUserAktif & " and IDBarang=" & ds.Tables("Data").Rows(i).Item("IDBarang"))
                EksekusiSQL("insert into MLaporanBelivsJualD(IDUser,IDBarang,QtyBeli,QtyJual,JumlahBeli,JumlahJual) values (" & _
                             IDUserAktif & "," & ds.Tables("Data").Rows(i).Item("IDBarang") & "," & _
                             FixKoma(NullToDbl(ds.Tables("Data").Rows(i).Item("QtyPembelian"))) & "," & _
                             FixKoma(NullToDbl(ds.Tables("Data").Rows(i).Item("QtyPenjualan"))) & "," & _
                             FixKoma(NullToDbl(ds.Tables("Data").Rows(i).Item("JumlahPembelian"))) & "," & _
                             FixKoma(NullToDbl(ds.Tables("Data").Rows(i).Item("JumlahPenjualan"))) & ")")
                ProgressBarControl1.Position = 100 * ((i + 1) / ds.Tables("Data").Rows.Count)
                Application.DoEvents()
            Next
            RefreshDataKumulatif()
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
        Windows.Forms.Cursor.Current = curentcursor
    End Sub
End Class