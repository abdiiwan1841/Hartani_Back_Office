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

Public Class frmImportDataPembelianBarang
    Public FormName As String = "ImportDataPembelianBarang"
    Public FormEntriName As String = ""
    Public TableName As String = ""
    Public TableNameD As String = ""
    Public TableMaster As String = ""
    Public IDBeli As Long = -1 
    Public IDGudang As Long = -1
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
        'If ShowNoID Then
        '    RefreshData()
        '    GV1.ClearSelection()
        '    GV1.FocusedRowHandle = GV1.LocateByDisplayText(0, GV1.Columns("NoID"), (DirectNoID).ToString("#,##0"))
        '    GV1.SelectRow(GV1.FocusedRowHandle)
        '    ShowNoID = False
        'End If
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
            Me.lbDaftar.Text = Me.Text
            XtraTabPage1.Text = Me.Text
            'RefreshData()

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


        'If System.IO.File.Exists(FolderLayouts & FormName & ".xml") Then
        '    GV1.RestoreLayoutFromXml(FolderLayouts & FormName & ".xml")
        'End If

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
        'RefreshData()
        If Strings.Right(txtFileName.Text, 3).ToLower = "mdb" Then
            refreshDataAccess()
        ElseIf Strings.Right(txtFileName.Text, 3).ToLower = "xls" Then
            refreshDataExcel()
        Else
            MsgBox("Silahkan Pilih file excel atau file access!", MsgBoxStyle.Information)
        End If
        'RestoreLayout()
    End Sub

    Sub refreshDataAccess()
        If Not String.IsNullOrEmpty(txtFileName.Text) Then
            Try
                Dim OleDBcn As OleDbConnection
                Dim OleDBocmd As OleDbCommand
                Dim OleDBoda As OleDbDataAdapter
                'Provider=Microsoft.Jet.OLEDB.4.0;Data Source=\\serverName\shareName\folder\myDatabase.mdb;User Id=admin;Password=;
                OleDBcn = New OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" & txtFileName.Text & ";User Id=admin;Password=;")
                OleDBocmd = New OleDb.OleDbCommand
                OleDBocmd.Connection = OleDBcn
                OleDBocmd.CommandType = CommandType.Text
                OleDBocmd.CommandText = "Select * from Barang"
                OleDBcn.Open()
                OleDBoda = New OleDbDataAdapter(OleDBocmd)
                OleDBoda.TableMappings.Add("Tabel", "Barang")
                If ds.Tables("Barang") Is Nothing Then
                Else
                    ds.Tables("Barang").Clear()
                End If
                OleDBoda.Fill(ds, "Barang")
                BS.DataSource = ds.Tables("Barang")
                GC1.DataSource = BS.DataSource

                OleDBoda.Dispose()
                OleDBocmd.Dispose()
                OleDBcn.Close()
                OleDBcn.Dispose()
                RestoreLayout()
            Catch ex As Exception

            Finally
            End Try
        End If
    End Sub

    Sub refreshDataExcel()
        If Not String.IsNullOrEmpty(txtFileName.Text) Then
            Try
                Dim OExcelHandler As New ExcelHandler()
                Dim ds As DataSet = OExcelHandler.GetDataFromExcel(txtFileName.Text.Trim())

                If ds IsNot Nothing Then
                    'dgvExcelData.SelectionMode = DataGridViewSelectionMode.FullRowSelect
                    'dgvExcelData.EditMode = DataGridViewEditMode.EditProgrammatically
                    'dgvExcelData.DataSource = ds.Tables(0)
                    GC1.DataSource = ds.Tables(0)
                End If

            Catch ex As Exception

            Finally
            End Try
        End If
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
    Private Sub RadioButton1_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        RefreshData()
    End Sub
    Private Sub RadioButton2_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        RefreshData()
    End Sub



    Private Sub mnHasilPosting_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles mnHasilPosting.ItemClick
        SimpleButton1.PerformClick()
    End Sub

    Private Sub ButtonEdit1_ButtonClick(ByVal sender As Object, ByVal e As DevExpress.XtraEditors.Controls.ButtonPressedEventArgs) Handles txtFileName.ButtonClick
        Dim dlg As New OpenFileDialog
        dlg.Filter = "Microsoft Acess Files (Recomended)|*.mdb|Microsoft Excel Files|*.xls"
        dlg.Title = "Silahkan pilih file data barang dalam format Access atau Excel..."
        If dlg.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
            txtFileName.Text = dlg.FileName
        End If
        dlg.Dispose()
        SimpleButton8.PerformClick()
    End Sub

    Private Sub ButtonEdit1_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtFileName.EditValueChanged

    End Sub

    Private Sub SimpleButton2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SimpleButton2.Click
        If MsgBox("Yakin Mau Proses Import Data ini Ke Pembelian Barang" & "Sebelum Proses Cek dulu Kode,Ukuran dan Jumlah sudah benar!", MsgBoxStyle.Question + MsgBoxStyle.YesNo) = MsgBoxResult.Yes Then
            ProsesImportPembelian()
            EksekusiSQL("update mbelid set idbarang=mbarangd.idbarang from mbelid inner join mbarangd on mbelid.idbarangd=mbarangd.noid where mbelid.idbeli=" & IDBeli)
            Me.Close()
        End If
    End Sub
    Sub ProsesImportPembelian()
        Dim i As Long
        Dim Kode As String
        Dim Ukuran As String
        Dim Nama As String
        Dim Kategori As String
        Dim HargaJual As Double
        Dim HargaBeli As Double
        Dim NoIDBarangD As Long
        Dim NoIDBarang As Long
        Dim Qty As Double
        Dim sql As String
        Dim margin As Double

        ProgressBarControl1.Position = 0
        Dim JumData As Long
        JumData = ds.Tables(0).Rows.Count
        If HargaBeli = 0 Then
            margin = 0
        Else
            margin = 100 * (HargaJual - HargaBeli) / HargaBeli
        End If

        For i = 0 To ds.Tables(0).Rows.Count - 1
            ProgressBarControl1.Position = ((i + 1) / JumData) * 100
            Kode = NullToStr(ds.Tables(0).Rows(i).Item("Kode"))
            Ukuran = NullToStr(ds.Tables(0).Rows(i).Item("Ukuran"))
            Nama = NullToStr(ds.Tables(0).Rows(i).Item("Nama"))
            Kategori = NullToStr(ds.Tables(0).Rows(i).Item("Kategori"))
            HargaJual = NullToDbl(ds.Tables(0).Rows(i).Item("HargaJual"))
            HargaBeli = NullToDbl(ds.Tables(0).Rows(i).Item("HargaBeli"))
            Qty = NullToDbl(ds.Tables(0).Rows(i).Item("Jumlah"))
            NoIDBarangD = EksekusiSQLSkalar("Select MBarangD.NoID from MBarangD inner join mbarang on MbarangD.IDBarang=MBarang.NoID where ltrim(rtrim(MBarang.Kode))='" & FixApostropi(Kode.Trim) & "' and ltrim(rtrim(MBarangD.Varian))='" & FixApostropi(Ukuran.Trim) & "'")
            If NoIDBarangD <= 0 Then 'tidak ada
                ''maka cek di barang apakah sudah ada atau belum
                'NoIDBarang = EksekusiSQLSkalar("Select MBarang.NoID from  MBarang where ltrim(rtrim(MBarang.Kode))='" & FixApostropi(Kode.Trim) & "'")
                'If NoIDBarang <= 0 Then 'Barang belum ada
                '    NoIDBarang = GetNewID("MBarang")
                '    EksekusiSQL("Insert into MBarang(NoID,IDkategori,Kode,Nama,IDSatuan,IDSatuanHarga,Konversi,HargaBeli,HargaBeliPcs,HargaBeliPcsBruto,Isactive,IdTypePajak,CtnPcs,Barcode,HargaJual,HargaJualNetto) values(" & _
                '               NoIDBarang & "," & IDKategori & ",'" & FixApostropi(Kode.Trim) & "','" & FixApostropi(Nama.Trim) & "',1,1,1," & FixKoma(HargaBeli) & "," & FixKoma(HargaBeli) & "," & FixKoma(HargaBeli) & ",1,0,1,'" & FixApostropi(Kode.Trim & IIf(Ukuran.Trim = "", "", "-") & Ukuran.Trim) & "'," & FixKoma(HargaJual) & "," & FixKoma(HargaJual) & ")")

                '    NoIDBarangD = GetNewID("MBarangD")
                '    EksekusiSQL("Insert into MBarangD(NoID,IDBarang,IDSatuan,Konversi,HargaJual,HargaNetto,IsActive,IsBeli,IsJual,IsJualPos,Varian,Barcode) values(" & _
                '                NoIDBarangD & "," & NoIDBarang & ",1,1," & FixKoma(HargaJual) & "," & FixKoma(HargaJual) & ",1,1,1,1,'" & FixApostropi(Ukuran.Trim) & "','" & FixApostropi(Kode.Trim & IIf(Ukuran.Trim = "", "", "-") & Ukuran.Trim) & "')")




                'Else 'Barang sudah ada tetapi barang detil/ukuran belum ada
                '    NoIDBarangD = GetNewID("MBarangD")
                '    EksekusiSQL("Insert into MBarangD(NoID,IDBarang,IDSatuan,Konversi,HargaJual,HargaNetto,IsActive,IsBeli,IsJual,IsJualPos,Varian,Barcode) values(" & _
                '                NoIDBarangD & "," & NoIDBarang & ",1,1," & FixKoma(HargaJual) & "," & FixKoma(HargaJual) & ",1,1,1,1,'" & FixApostropi(Ukuran.Trim) & "','" & FixApostropi(Kode.Trim & IIf(Ukuran.Trim = "", "", "-") & Ukuran.Trim) & "')")

                'End If
            Else 'Barang Detil ada dan Barang Ada
                Sql = "INSERT INTO MBeliD (NoID,IDBeli,IDBarangD,IDPOD,IDLPBD,NoUrut,Tgl,Jam,ExpiredDate," & _
                "IDBarang,IDSatuan,Qty,QtyPcs,Harga,Biaya,HargaPcs,CTN,DiscPersen1,DiscPersen2,DiscPersen3,Disc1,Disc2,Disc3,Jumlah,Catatan,IDGudang,Konversi,PPN,HargaNetto,ProsenMargin,HitungJual,HargaJual) VALUES ("
                Sql &= NullToLong(GetNewID("MBeliD", "NoID")) & ","
                Sql &= IDBeli & ","
                sql &= NullToLong(NoIDBarangD) & ","
                sql &= NullToLong(0) & ","
                sql &= NullToLong(0) & ","
                Sql &= GetNewID("MBeliD", "NoUrut", " WHERE IDBeli=" & IDBeli) & ","
                Sql &= "GetDate(),"
                Sql &= "GetDate(),"
                sql &= "null,"
                sql &= NullToLong(NoIDBarang) & ","
                sql &= NullToLong(1) & ","
                sql &= FixKoma(Qty) & ","
                sql &= FixKoma(Qty) & ","
                sql &= FixKoma(HargaBeli) & ","
                sql &= FixKoma(0) & ","
                sql &= FixKoma(HargaBeli) & ","
                sql &= FixKoma(1) & ","
                sql &= FixKoma(0) & ","
                sql &= FixKoma(0) & ","
                sql &= FixKoma(0) & ","
                sql &= FixKoma(0) & ","
                sql &= FixKoma(0) & ","
                sql &= FixKoma(0) & ","
                sql &= FixKoma(Qty * HargaBeli) & ","
                sql &= "'" & FixApostropi("") & "',"
                sql &= NullToLong(IDGudang) & ","
                sql &= FixKoma(1) & ","
                sql &= FixKoma(0) & ","
                sql &= FixKoma(HargaBeli) & ","
                sql &= FixKoma(Margin) & ","
                sql &= FixKoma(HargaJual) & ","
                sql &= FixKoma(HargaJual)
                sql &= ")"
                EksekusiSQL(sql)
                'EksekusiSQL("Update MBarang Set IsActive=1, Nama='" & FixApostropi(Nama.Trim) & "',HargaJual=" & FixKoma(HargaJual) & ",HargaJualNetto=" & FixKoma(HargaJual) & " where NoID=" & NoIDBarang)
                'EksekusiSQL("Update MBarangD Set  IsActive=1,IsJual=1,IsJualPos=1,Varian='" & FixApostropi(Ukuran.Trim) & "',HargaJual=" & FixKoma(HargaJual) & ",HargaNetto=" & FixKoma(HargaJual) & " where NoID=" & NoIDBarangD)

            End If
            Application.DoEvents()
        Next
        MsgBox("Proses Selesai", MsgBoxStyle.Information)
    End Sub

    Sub ProsesImport()
        Dim i As Long
        Dim Kode As String
        Dim Ukuran As String
        Dim Nama As String
        Dim Kategori As String
        Dim HargaJual As Double
        Dim HargaBeli As Double
        Dim NoIDBarangD As Long
        Dim NoIDBarang As Long
        Dim IDKategori As Long
        Dim Tanggal As Date
        ProgressBarControl1.Position = 0
        Dim JumData As Long
        JumData = ds.Tables(0).Rows.Count
        For i = 0 To ds.Tables(0).Rows.Count - 1
            ProgressBarControl1.Position = ((i + 1) / JumData) * 100
            Kode = NullToStr(ds.Tables(0).Rows(i).Item("Kode"))
            Ukuran = NullToStr(ds.Tables(0).Rows(i).Item("Ukuran"))
            Nama = NullToStr(ds.Tables(0).Rows(i).Item("Nama"))
            Kategori = NullToStr(ds.Tables(0).Rows(i).Item("Kategori"))
            HargaJual = NullToDbl(ds.Tables(0).Rows(i).Item("HargaJual"))
            HargaBeli = NullToDbl(ds.Tables(0).Rows(i).Item("HargaBeli"))
            If TypeOf ds.Tables(0).Rows(i).Item("Tanggal") Is DBNull Then
                Tanggal = Today
            Else
                Tanggal = ds.Tables(0).Rows(i).Item("Tanggal")
            End If
            IDKategori = EksekusiSQLSkalar("Select MKategori.NoID from MKategori  where ltrim(rtrim(MKategori.Nama))='" & FixApostropi(Kategori.Trim) & "'")
            If IDKategori <= 0 Then
                IDKategori = GetNewID("MKategori")
                EksekusiSQL("Insert Into MKategori(NoID,IDParent,Kode,Nama,IsActive) values(" & _
                            IDKategori & ",-1,'" & Format(IDKategori, "000") & "','" & FixApostropi(Nama.Trim) & "',1)")

            End If
            NoIDBarangD = EksekusiSQLSkalar("Select MBarangD.NoID from MBarangD inner join mbarang on MbarangD.IDBarang=MBarang.NoID where ltrim(rtrim(MBarang.Kode))='" & FixApostropi(Kode.Trim) & "' and ltrim(rtrim(MBarangD.Varian))='" & FixApostropi(Ukuran.Trim) & "'")
            If NoIDBarangD <= 0 Then 'tidak ada
                'maka cek di barang apakah sudah ada atau belum
                NoIDBarang = EksekusiSQLSkalar("Select MBarang.NoID from  MBarang where ltrim(rtrim(MBarang.Kode))='" & FixApostropi(Kode.Trim) & "'")
                If NoIDBarang <= 0 Then 'Barang belum ada
                    NoIDBarang = GetNewID("MBarang")
                    EksekusiSQL("Insert into MBarang(NoID,IDkategori,Kode,Nama,IDSatuan,IDSatuanHarga,Konversi,HargaBeli,HargaBeliPcs,HargaBeliPcsBruto,Isactive,IdTypePajak,CtnPcs,Barcode,HargaJual,HargaJualNetto) values(" & _
                               NoIDBarang & "," & IDKategori & ",'" & FixApostropi(Kode.Trim) & "','" & FixApostropi(Nama.Trim) & "',1,1,1," & FixKoma(HargaBeli) & "," & FixKoma(HargaBeli) & "," & FixKoma(HargaBeli) & ",1,0,1,'" & FixApostropi(Kode.Trim & IIf(Ukuran.Trim = "", "", "-") & Ukuran.Trim) & "'," & FixKoma(HargaJual) & "," & FixKoma(HargaJual) & ")")

                    NoIDBarangD = GetNewID("MBarangD")
                    EksekusiSQL("Insert into MBarangD(NoID,IDBarang,IDSatuan,Konversi,HargaJual,HargaNetto,IsActive,IsBeli,IsJual,IsJualPos,Varian,Barcode) values(" & _
                                NoIDBarangD & "," & NoIDBarang & ",1,1," & FixKoma(HargaJual) & "," & FixKoma(HargaJual) & ",1,1,1,1,'" & FixApostropi(Ukuran.Trim) & "','" & FixApostropi(Kode.Trim & IIf(Ukuran.Trim = "", "", "-") & Ukuran.Trim) & "')")




                Else 'Barang sudah ada tetapi barang detil/ukuran belum ada
                    NoIDBarangD = GetNewID("MBarangD")
                    EksekusiSQL("Insert into MBarangD(NoID,IDBarang,IDSatuan,Konversi,HargaJual,HargaNetto,IsActive,IsBeli,IsJual,IsJualPos,Varian,Barcode) values(" & _
                                NoIDBarangD & "," & NoIDBarang & ",1,1," & FixKoma(HargaJual) & "," & FixKoma(HargaJual) & ",1,1,1,1,'" & FixApostropi(Ukuran.Trim) & "','" & FixApostropi(Kode.Trim & IIf(Ukuran.Trim = "", "", "-") & Ukuran.Trim) & "')")

                End If
            Else 'Barang Detil ada dan Barang Ada
                EksekusiSQL("Update MBarang Set IsActive=1, Nama='" & FixApostropi(Nama.Trim) & "',HargaJual=" & FixKoma(HargaJual) & ",HargaJualNetto=" & FixKoma(HargaJual) & " where NoID=" & NoIDBarang)
                EksekusiSQL("Update MBarangD Set  IsActive=1,IsJual=1,IsJualPos=1,Varian='" & FixApostropi(Ukuran.Trim) & "',HargaJual=" & FixKoma(HargaJual) & ",HargaNetto=" & FixKoma(HargaJual) & " where NoID=" & NoIDBarangD)

            End If
            Application.DoEvents()
        Next
        MsgBox("Proses Selesai", MsgBoxStyle.Information)
    End Sub
End Class