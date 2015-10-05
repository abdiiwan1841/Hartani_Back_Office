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

Public Class frmLaporanRekapPenjualanPerCustomer
    Public FormName As String = "LaporanRekapPenjualanPerCustomer"
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
            TglDari.DateTime = CDate(TanggalSystem.ToString("yyyy/MM/dd"))
            TglSampai.DateTime = CDate(TanggalSystem.ToString("yyyy/MM/dd"))
            Me.lbDaftar.Text = Me.Text
            XtraTabPage1.Text = Me.Text
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
        Dim Cur As Cursor = Windows.Forms.Cursor.Current
        Dim FilterSQL As String = ""

        Try
            Dim curentcursor As Cursor = Windows.Forms.Cursor.Current
            Windows.Forms.Cursor.Current = Cursors.WaitCursor
            dlg = New DevExpress.Utils.WaitDialogForm(String.Format("Sedang merefresh data.{0}MOHON TUNGGU ...", vbCrLf), Application.ProductName)
            dlg.TopMost = False
            dlg.Show()
            Windows.Forms.Cursor.Current = Cursors.WaitCursor

            Dim strsql As String = ""
            If RadioButton2.Checked Then
                strsql = "Select MAlamat.Kode as KodeMember,MAlamat.Nama as Customer,MAlamat.Alamat,Malamat.Telpon,Sales.* From Malamat "
                strsql = strsql & " inner Join "
                strsql = strsql & " (Select Count(MJual.NoID) as Kunjungan,MJual.IDCustomer ,Sum(MJual.Subtotal) as SubTotal,Sum(MJual.DiskonNotaRp) as Diskon, Sum(MJual.Total) as Total, SUM(MJual.NilaiPoin) as Poin "
                strsql = strsql & " from MJual "
                strsql = strsql & " where MJual.IDCustomer>0 and Tanggal>='" & Format(TglDari.DateTime, "yyyy/MM/dd") & "' and Tanggal <'" & Format(TglSampai.DateTime.AddDays(1), "yyyy/MM/dd") & "' "
                strsql = strsql & " group by IDCustomer ) Sales "
                strsql = strsql & " on Sales.IDCustomer=MAlamat.NoID  Order by Sales.Total Desc"
            ElseIf RadioButton1.Checked Then
                strsql = "Select MAlamat.Kode as KodeMember,MAlamat.Nama as Customer,MAlamat.Alamat,Malamat.Telpon,Sales.* From Malamat "
                strsql = strsql & " inner Join "
                strsql = strsql & " (Select Count(MJual.NoID) as Kunjungan,MJual.IDCustomer ,Sum(MJual.Subtotal) as SubTotal,Sum(MJual.DiskonNotaRp) as Diskon, Sum(MJual.Total) as Total, SUM(MJual.NilaiPoin) as Poin "
                strsql = strsql & " from MJual "
                strsql = strsql & " where MJual.IDCustomer>0 and Tanggal>='" & Format(TglDari.DateTime, "yyyy/MM/dd") & "' and Tanggal <'" & Format(TglSampai.DateTime.AddDays(1), "yyyy/MM/dd") & "' "
                strsql = strsql & " group by IDCustomer ) Sales "
                strsql = strsql & " on Sales.IDCustomer=MAlamat.NoID   Order by Sales.Kunjungan Desc "
            End If
            'If TglDari.Enabled Then
            '    '                strsql = "select * from (SELECT count(distinct Mjual.NoID) as JumlahTaransaksi,mjual.IDWilayah,MKategori.Nama as Kategori,MBarang.Kode,Mbarang.Nama,Sum(MJualD.Qty*MJualD.Konversi) as QtyPcs, Sum((( MJualD.Harga*(100-ISNULL(DISCPERSEN1,0))*(100-ISNULL(DISCPERSEN2,0)))/10000-ISNULL(Disc3,0))*MJuald.Qty)as TotalRupiah from mjuald inner join mjual on mjuald.idjual=mjual.noid inner join mbarang  on mjuald.idbarang=mbarang.noid inner join mkategori on mbarang.idkategori=mkategori.noid where tanggal >='" & Format(TglDari.DateTime, "yyyy/MM/dd") & "' and tanggal<'" & Format(TglSampai.DateTime.AddDays(1), "yyyy/MM/dd") & "' " & whereInsert & " group by mjual.IDWilayah,MKategori.Nama,MBarang.Kode,Mbarang.Nama) MJual "
            'Else
            '    '               strsql = "select * from (SELECT count(distinct Mjual.NoID) as JumlahTaransaksi,mjual.IDWilayah,MKategori.Nama as Kategori,MBarang.Kode,Mbarang.Nama,Sum(MJualD.Qty*MJualD.Konversi) as QtyPcs, Sum((( MJualD.Harga*(100-ISNULL(DISCPERSEN1,0))*(100-ISNULL(DISCPERSEN2,0)))/10000-ISNULL(Disc3,0))*MJuald.Qty)as TotalRupiah from mjuald inner join mjual on mjuald.idjual=mjual.noid inner join mbarang  on mjuald.idbarang=mbarang.noid inner join mkategori on mbarang.idkategori=mkategori.noid " & whereInsert & " group by mjual.IDWilayah,MKategori.Nama,MBarang.Kode,Mbarang.Nama) MJual"
            'End If
            ds = ExecuteDataset("MKartuStok", strsql)
            GC1.DataSource = ds.Tables("MKartuStok")


        Catch ex As Exception
            XtraMessageBox.Show("Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            Windows.Forms.Cursor.Current = Cur
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
    Private Sub CetakMRPTJual(ByVal Action As action_)
        'Dim namafile As String
        'Dim strsql As String = ""
        'Dim view As ColumnView = GC1.FocusedView
        'Dim RefundFood As Double = 0.0 '46
        'Dim RefundNonFood As Double = 0.0 '47
        'Dim RefundFreshFood As Double = 0.0 '48
        'Try
        '    strsql = "Select Sum(MJualD.Harga*MJuald.Qty) as Jumlah From " & _
        '             "MJualD Inner Join MJual On MJualD.IDJual=MJual.NoID " & _
        '             "Inner Join MBarang On MJualD.IDBarang=MBarang.NoID " & _
        '             "Inner Join MKategori On MBarang.IDKategori=MKategori.NoID " & _
        '             "where MJualD.Transaksi='RTN' and MKategori.IDParent=46 and MJual.Tanggal>= " & Format(TglDari.DateTime, "yyyy/MM/dd") & " and MJual.Tanggal<" & Format(TglSampai.DateTime.AddDays(1), "yyyy/MM/dd")
        '    If txtSupplier.Enabled Then
        '        strsql = strsql & " and MJual.IDPos=" & NullTolInt(txtSupplier.EditValue)
        '    End If
        '    RefundFood = EksekusiSQLSkalar(strsql)
        '    strsql = "Select Sum(MJualD.Harga*MJuald.Qty) as Jumlah From " & _
        '             "MJualD Inner Join MJual On MJualD.IDJual=MJual.NoID " & _
        '             "Inner Join MBarang On MJualD.IDBarang=MBarang.NoID " & _
        '             "Inner Join MKategori On MBarang.IDKategori=MKategori.NoID " & _
        '             "where MJualD.Transaksi='RTN' and MKategori.IDParent=47 and MJual.Tanggal>= " & Format(TglDari.DateTime, "yyyy/MM/dd") & " and MJual.Tanggal<" & Format(TglSampai.DateTime.AddDays(1), "yyyy/MM/dd")
        '    If txtSupplier.Enabled Then
        '        strsql = strsql & " and MJual.IDPos=" & NullTolInt(txtSupplier.EditValue)
        '    End If
        '    RefundNonFood = EksekusiSQLSkalar(strsql)

        '    strsql = "Select Sum(MJualD.Harga*MJuald.Qty) as Jumlah From " & _
        '                                          "MJualD Inner Join MJual On MJualD.IDJual=MJual.NoID " & _
        '                                          "Inner Join MBarang On MJualD.IDBarang=MBarang.NoID " & _
        '                                          "Inner Join MKategori On MBarang.IDKategori=MKategori.NoID " & _
        '                                          "where MJualD.Transaksi='RTN' and MKategori.IDParent=48 and MJual.Tanggal>= " & Format(TglDari.DateTime, "yyyy/MM/dd") & " and MJual.Tanggal<" & Format(TglSampai.DateTime.AddDays(1), "yyyy/MM/dd")
        '    If txtSupplier.Enabled Then
        '        strsql = strsql & " and MJual.IDPos=" & NullTolInt(txtSupplier.EditValue)
        '    End If
        '    RefundFreshFood = EksekusiSQLSkalar(strsql)

        '    namafile = Application.StartupPath & "\report\Laporan" & TableMaster & ".rpt"
        '    If System.IO.File.Exists(namafile) Then
        '        If EditReport Then
        '            Action = action_.Edit
        '        Else
        '            Action = action_.Preview
        '        End If
        '        If txtSupplier.Text <> "" Then
        '            ViewReport(Me.ParentForm, Action, namafile, Me.Text, "cdate({vRekapPenjualanPerDepartemen.Tanggal})>={@DariTanggal} AND cdate({vRekapPenjualanPerDepartemen.Tanggal})<={@SampaiTanggal} AND {vRekapPenjualanPerDepartemen.IDPos}={@IDPOS}", , "DariTanggal=cdate(" & TglDari.DateTime.ToString("yyyy,MM,dd") & ")&SampaiTanggal=cdate(" & TglSampai.DateTime.ToString("yyyy,MM,dd") & ")&IDPOS=" & NullToLong(txtSupplier.EditValue) & "&RefundFood=" & CLng(RefundFood) & "&RefundNonFood=" & CLng(RefundNonFood) & "&RefundFreshFood=" & CLng(RefundFreshFood) & "")
        '        Else
        '            ViewReport(Me.ParentForm, Action, namafile, Me.Text, "cdate({vRekapPenjualanPerDepartemen.Tanggal})>={@DariTanggal} AND cdate({vRekapPenjualanPerDepartemen.Tanggal})<={@SampaiTanggal}", , "DariTanggal=cdate(" & TglDari.DateTime.ToString("yyyy,MM,dd") & ")&SampaiTanggal=cdate(" & TglSampai.DateTime.ToString("yyyy,MM,dd") & ")" & "&RefundFood=" & CLng(RefundFood) & "&RefundNonFood=" & CLng(RefundNonFood) & "&RefundFreshFood=" & CLng(RefundFreshFood) & "")
        '        End If
        '    Else
        '        DevExpress.XtraEditors.XtraMessageBox.Show("Nama File :" & namafile & " tidak ditemukan.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Stop)
        '    End If
        'Catch EX As Exception
        '    XtraMessageBox.Show("Untuk mencetak, pilih item yang mau dicetak dan click cetak faktur.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
        '    'DevExpress.XtraEditors.XtraMessageBox.Show("Kesalahan:" & EX.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        'End Try
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



    Private Sub GV1_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs)
        'Dim View As GridView = CType(sender, GridView)
        'If View Is Nothing Then Return
        '' obtaining hit info
        'Dim hitInfo As GridHitInfo = View.CalcHitInfo(New System.Drawing.Point(e.X, e.Y))
        'If (e.Button = Windows.Forms.MouseButtons.Right) And (hitInfo.InRow) And _
        '  (Not View.IsGroupRow(hitInfo.RowHandle)) Then
        '    PopupMenu1.ShowPopup(Control.MousePosition)
        'End If
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

    Private Sub SimpleButton1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SimpleButton1.Click
        Dim x As New frmLaporanDetilPenjualanPerCustomer
        x.IDCustomer = NullToLong(GV1.GetRowCellValue(GV1.FocusedRowHandle, "IDCustomer"))
        x.TglDari.DateTime = TglDari.DateTime
        x.TglSampai.DateTime = TglSampai.DateTime
        x.MdiParent = Me.MdiParent
        x.WindowState = FormWindowState.Normal
        x.Show()
        x.Focus()
    End Sub

    Private Sub mnHasilPosting_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles mnHasilPosting.ItemClick
        SimpleButton1.PerformClick()
    End Sub
End Class