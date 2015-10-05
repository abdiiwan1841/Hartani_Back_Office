Imports System.Data.SqlClient
Imports System.Data.SQLite
Imports System.Windows.Forms
Imports DevExpress.XtraEditors
Imports DevExpress.XtraGrid.Views
Imports DevExpress.XtraGrid.Columns
Imports DevExpress.XtraGrid.Views.Base
Imports System.Drawing
Imports DevExpress.XtraEditors.Repository
Imports VPoint.mdlCetakCR
Imports DevExpress.XtraGrid.Views.Grid.ViewInfo
Imports DevExpress.XtraGrid.Views.Grid
Imports DevExpress.Utils.Menu
Imports DevExpress.XtraGrid.Localization
Imports System.Data.OleDb

Public Class frmLaporanPenjualanPromoDiskon
    Public FormName As String = "LaporanPenjualanPromoDiskon"
    Public FormEntriName As String = ""
    Public TableName As String = ""
    Public TableNameD As String = ""
    Public TableMaster As String = ""
    Public NoID As Long = -1

    Dim ds As New DataSet
    Dim BS As New BindingSource
    Dim HargaPcs As Double

    Dim repckedit As New RepositoryItemCheckEdit
    Dim repdateedit As New RepositoryItemDateEdit
    Dim reptextedit As New RepositoryItemTextEdit
    Dim reppicedit As New RepositoryItemPictureEdit
    Public ShowNoID As Boolean = False
    Public DirectNoID As Long = -1

    Dim NamaFileDB As String = ""

    Private Sub ctrlDaftar_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim dlg As DevExpress.Utils.WaitDialogForm = Nothing
        Dim curentcursor As Cursor = Windows.Forms.Cursor.Current
        Try
            Windows.Forms.Cursor.Current = Cursors.WaitCursor
            dlg = New DevExpress.Utils.WaitDialogForm(String.Format("Creating component and analize database.{0}MOHON TUNGGU ...", vbCrLf), Application.ProductName)
            dlg.TopMost = False
            dlg.Show()
            Windows.Forms.Cursor.Current = Cursors.WaitCursor
            TglDari.DateTime = CDate(TanggalSystem.ToString("yyyy/MM/dd"))
            TglSampai.DateTime = CDate(TanggalSystem.ToString("yyyy/MM/dd"))
            Me.lbDaftar.Text = Me.Text
            XtraTabPage1.Text = Me.Text
            RefreshDataLookUp()
            RefreshData()
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


        If System.IO.File.Exists(FolderLayouts & FormName & ".xml") Then
            GV1.RestoreLayoutFromXml(FolderLayouts & FormName & ".xml")
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
        Dim strsql As String = ""
        Try
            dlg = New DevExpress.Utils.WaitDialogForm(String.Format("Sedang merefresh data.{0}MOHON TUNGGU ...", vbCrLf), Application.ProductName)
            dlg.TopMost = False
            dlg.Show()
            Windows.Forms.Cursor.Current = Cursors.WaitCursor
            strsql = "SELECT MJualD.NoID, MJual.Tanggal, MJualD.IDJual, MPOS.Kode AS Kassa, MUser.Nama AS Kasir, MBarang.Kode AS KodeBarang, MBarangD.Barcode, " & vbCrLf & _
                     " MBarang.Nama AS NamaBarang, MKategori.Nama AS Kategori, MJualD.Konversi, MSatuan.Kode AS Satuan, MJualD.Qty, MAlamat.Kode AS KodeCustomer, MAlamat.Nama AS NamaCustomer, MJualD.Transaksi, " & vbCrLf & _
                     " MJualD.Harga, CASE WHEN IsNull(MJualD.IsDisc2,0)=0 THEN MJualD.DiscPersen1 ELSE 0 END AS DiscProsenAll, " & vbCrLf & _
                     " MJualD.Qty*(CASE WHEN IsNull(MJualD.IsDisc2,0)=1 THEN MJualD.Disc1 ELSE 0 END) AS DiscRpMember, " & vbCrLf & _
                     " MJualD.Qty*(CASE WHEN IsNull(MJualD.IsDisc2,0)=0 THEN MJualD.Disc1 ELSE 0 END) AS DiscRpAll," & vbCrLf & _
                     " CASE WHEN IsNull(MJualD.IsDisc2,0)=1 THEN MJualD.DiscPersen1 ELSE 0 END AS DiscProsenMember, MJualD.DiscNotaProsen, " & vbCrLf & _
                     " MJualD.JumlahDiscNotaRp, MJual.DiskonNotaTotal, MJualD.Qty*(IsNull(MJualD.Harga,0)-IsNull(MJualD.Disc1,0)-IsNull(MJualD.JumlahDiscNotaRp,0)) AS Jumlah, IsNull(MJualD.HargaNormal,0) AS HargaNormal, CASE WHEN IsNull(MJualD.HargaNormal,0)>=1 THEN IsNull(MJualD.HargaNormal,0)-(IsNull(MJualD.Harga,0)-IsNull(MJualD.Disc1,0)-IsNull(MJualD.JumlahDiscNotaRp,0)) ELSE 0 END AS DiscPDP " & vbCrLf & _
                     " FROM MJual" & vbCrLf & _
                     " INNER JOIN MJualD ON MJual.NoID=MJualD.IDJual" & vbCrLf & _
                     " LEFT JOIN (MBarang INNER JOIN MKategori ON MKategori.NoID=MBarang.IDKategori) ON MBarang.NoID=MJualD.IDBarang" & vbCrLf & _
                     " LEFT JOIN MBarangD ON MBarangD.NoID=MJualD.IDBarangD" & vbCrLf & _
                     " LEFT JOIN MSatuan ON MSatuan.NoID=MJualD.IDSatuan" & vbCrLf & _
                     " LEFT JOIN MAlamat ON MAlamat.NoID=MJual.IDCustomer" & vbCrLf & _
                     " LEFT JOIN MPOS ON MPOS.NoID=MJual.IDPos" & vbCrLf & _
                     " LEFT JOIN MUser ON MUser.NoID=MJual.IDUserEntry" & vbCrLf & _
                     " WHERE MJual.IsPOS=1 AND MJual.Tanggal>='" & TglDari.DateTime.ToString("yyyy-MM-dd") & "' AND MJual.Tanggal<'" & TglSampai.DateTime.AddDays(1).ToString("yyyy-MM-dd") & "'"
            If txtBarang.Text <> "" Then
                strsql &= " AND MBarang.NoID=" & NullToLong(txtBarang.EditValue)
            End If
            If txtKategori.Text <> "" Then
                strsql &= " AND MKategori.NoID=" & NullToLong(txtKategori.EditValue)
            End If
            If txtKassa.Text <> "" Then
                strsql &= " AND MPOS.NoID=" & NullToLong(txtKassa.EditValue)
            End If
            If RadioButton2.Checked Then
                strsql &= " AND IsNull(MJualD.IsDisc2,0)=0 AND IsNull(MJualD.DiscPersen1,0)<>0 "
            End If
            If RadioButton3.Checked Then
                strsql &= " AND IsNull(MJualD.IsDisc2,0)=1 "
            End If
            If RadioButton4.Checked Then
                strsql &= " AND IsNull(MJualD.HargaNormal,0)<>0 "
            End If
            If RadioButton5.Checked Then
                strsql &= " AND IsNull(MJualD.DiscNotaProsen,0)<>0 "
            End If
            ds = ExecuteDataset("MKartuStok", strsql)
            GC1.DataSource = ds.Tables("MKartuStok")

            GV1.OptionsCustomization.AllowSort = True
        Catch ex As Exception
            XtraMessageBox.Show("Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            Windows.Forms.Cursor.Current = Cursors.Default
            dlg.Close()
            dlg.Dispose()
        End Try
    End Sub
    Sub RefreshDataLookUp()
        Dim dlg As DevExpress.Utils.WaitDialogForm = Nothing
        Dim strsql As String = ""
        Try
            dlg = New DevExpress.Utils.WaitDialogForm(String.Format("Sedang merefresh data.{0}MOHON TUNGGU ...", vbCrLf), Application.ProductName)
            dlg.TopMost = False
            dlg.Show()
            Windows.Forms.Cursor.Current = Cursors.WaitCursor
            strsql = "SELECT MBarang.NoID, MBarang.Kode, MBarang.Nama " & vbCrLf & _
                     " FROM MBarang INNER JOIN MBarangD ON MBarang.NoID=MBarangD.IDBarang " & _
                     " WHERE MBarang.IsActive=1 AND MBarangD.IsActive=1 " & vbCrLf & _
                     " GROUP BY MBarang.NoID, MBarang.Kode, MBarang.Nama ORDER BY MBarang.Kode"
            ds = ExecuteDataset("MKartuStok", strsql)
            txtBarang.Properties.DataSource = ds.Tables("MKartuStok")

            strsql = "SELECT MPOS.NoID, MPOS.Kode, MPOS.Nama " & vbCrLf & _
                     " FROM MPOS " & _
                     " WHERE MPOS.IsActive=1 "
            ds = ExecuteDataset("MKartuStok", strsql)
            txtKassa.Properties.DataSource = ds.Tables("MKartuStok")

            strsql = "SELECT MKategori.NoID, MKategori.Kode, MKategori.Nama " & vbCrLf & _
                     " FROM MKategori WHERE MKategori.IsActive=1 AND IsNull(MKategori.IDParent,0) NOT IN (SELECT IsNull(A.NoID,0) FROM MKategori A WHERE A.IsActive=1) "
            ds = ExecuteDataset("MKartuStok", strsql)
            txtKategori.Properties.DataSource = ds.Tables("MKartuStok")
        Catch ex As Exception
            XtraMessageBox.Show("Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            Windows.Forms.Cursor.Current = Cursors.Default
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

    Private Sub mnSaveLayouts_ItemClick(ByVal sender As Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles mnSaveLayouts.ItemClick
        Dim xOtorisasi As New frmOtorisasiAdmin
        Try
            If XtraMessageBox.Show("Simpan Layout?", NamaAplikasi, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = Windows.Forms.DialogResult.Yes AndAlso xOtorisasi.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
                GV1.SaveLayoutToXml(FolderLayouts & FormName & ".xml")
            End If
        Catch ex As Exception
            XtraMessageBox.Show("Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            xOtorisasi.Dispose()
        End Try
    End Sub

    Private Sub mnRefresh_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles mnRefresh.ItemClick
        SimpleButton8.PerformClick()
    End Sub
    Private Sub RadioButton1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles RadioButton1.Click
        RefreshData()
    End Sub
    Private Sub RadioButton2_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles RadioButton2.Click
        RefreshData()
    End Sub

    Private Sub RadioButton3_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles RadioButton3.Click
        RefreshData()
    End Sub

    Private Sub RadioButton4_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles RadioButton4.Click
        RefreshData()
    End Sub

    Private Sub RadioButton5_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles RadioButton5.Click
        RefreshData()
    End Sub
End Class