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
Imports VPoint.clsPostingPembelian
Imports VPoint.clsPostingPenjualan
Imports DevExpress.Utils
Imports DevExpress.XtraGrid.Views.Grid.ViewInfo
Imports DevExpress.XtraGrid.Views.Grid
Imports DevExpress.Utils.Menu
Imports DevExpress.XtraGrid.Localization

Public Class frmHasilPosting
    Public NoID As Long = -1
    Dim ds As New DataSet
    Dim HargaPcs As Double

    Dim repckedit As New RepositoryItemCheckEdit
    Dim repdateedit As New RepositoryItemDateEdit
    Dim reptextedit As New RepositoryItemTextEdit
    Dim reppicedit As New RepositoryItemPictureEdit

    Public IDTransaksi As Long = -1
    Public IDJenisTransaksi As Long = -1
    Public TransaksiHeader As String = ""

    Private Sub frmHasilPosting_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        IsShowStock = False
        'Dispose()
    End Sub

    Private Sub ctrlDaftar_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim dlg As DevExpress.Utils.WaitDialogForm = Nothing
        Try
            Dim curentcursor As Cursor = Windows.Forms.Cursor.Current
            Windows.Forms.Cursor.Current = Cursors.WaitCursor
            dlg = New DevExpress.Utils.WaitDialogForm(String.Format("Creating component and analize database.{0}MOHON TUNGGU ...", vbCrLf), Application.ProductName)
            dlg.TopMost = False
            dlg.Show()
            RefreshData()
            RestoreLayout()
            FungsiControl.SetForm(Me)
            Me.Text = Me.Text & TransaksiHeader

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

        With GridView2
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

        If System.IO.File.Exists(folderLayouts & Me.Name & ".xml") Then
            GV1.RestoreLayoutFromXml(folderLayouts & Me.Name & ".xml")
        End If
        If System.IO.File.Exists(folderLayouts & Me.Name & GridView1.Name & ".xml") Then
            GridView1.RestoreLayoutFromXml(folderLayouts & Me.Name & GridView1.Name & ".xml")
        End If
        If System.IO.File.Exists(folderLayouts & Me.Name & GridView2.Name & ".xml") Then
            GridView2.RestoreLayoutFromXml(folderLayouts & Me.Name & GridView2.Name & ".xml")
        End If

        Me.Width = Ini.BacaIniPath(folderLayouts & Me.Name & ".ini", "Form", "Width", Me.Width)
        Me.Height = Ini.BacaIniPath(folderLayouts & Me.Name & ".ini", "Form", "Height", Me.Height)

        Me.Left = (Screen.PrimaryScreen.WorkingArea.Width - Me.Width) \ 2
        Me.Top = (Screen.PrimaryScreen.WorkingArea.Height - Me.Height) \ 2
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
        Try
            Dim curentcursor As Cursor = Windows.Forms.Cursor.Current
            Windows.Forms.Cursor.Current = Cursors.WaitCursor
            dlg = New DevExpress.Utils.WaitDialogForm(String.Format("Sedang merefresh data.{0}MOHON TUNGGU ...", vbCrLf), Application.ProductName)
            dlg.TopMost = False
            dlg.Show()
            Windows.Forms.Cursor.Current = Cursors.WaitCursor

            Dim SQL As String = "SELECT MKartuStok.* FROM MKartuStok WHERE IDJenisTransaksi=" & IDJenisTransaksi & " AND IDTransaksi=" & IDTransaksi
            ds = ExecuteDataset("MKartuStok", SQL)
            With ds.Tables("MKartuStok")
                'If .Rows.Count >= 1 Then
                '    clsPostingKartuStok.TriggerStok(NullToDate(.Rows(0).Item("Tanggal")), NullToDate(.Rows(0).Item("Tanggal")), -1)
                'End If
                SQL = "SELECT MKartuStok.IDSatuan, MGudang.Kode AS Gudang, MWilayah.Kode AS Wilayah, MKartuStok.NoID, MKartuStok.Kode AS KodeTransaksi, MKartuStok.Tanggal, MKartuStok.Keterangan, MBarang.Kode AS KodeBarang, MBarang.Nama, MBarang.Barcode, MKartuStok.QtyMasuk AS QtyMasuk, MKartuStok.QtyKeluar AS QtyKeluar, MKartuStok.QtyMasukA AS [QtyMasuk (Pcs)], MKartuStok.QtyKeluarA AS [QtyKeluar (Pcs)], MSatuan.Nama AS Satuan, mjenisbarang.Nama AS Jenis, " & vbCrLf
                SQL &= " MKartuStok.HargaBeli/MKartuStok.Konversi AS HargaPcs, MKategori.Nama AS Kategori, MBarang.KodeDuz, MKartuStok.Ctn_Duz AS CtnDuz, MBarang.IsActive, MJenisTransaksi.Nama AS Transaksi," & vbCrLf
                SQL &= " MAlamat.Kode as KodeKontak, MAlamat.Nama as NamaKontak, " & vbCrLf
                SQL &= " MBarangD.Varian AS [Varian/Ukuran] "
                'SQL &= ", (SELECT SUM(TKartuStok.SaldoAwal) FROM TKartuStok WHERE TKartuStok.IDUSer=" & IDUserAktif & " AND TKartuStok.IP='" & FixApostropi(IPLokal) & "' AND TKartuStok.IDKartuStok=MKartuStok.NoID) AS SaldoAwal"
                'SQL &= ", (SELECT SUM(TKartuStok.SaldoAkhir) FROM TKartuStok WHERE TKartuStok.IDUSer=" & IDUserAktif & " AND TKartuStok.IP='" & FixApostropi(IPLokal) & "' AND TKartuStok.IDKartuStok=MKartuStok.NoID) AS SaldoAkhir"
                SQL &= " FROM MKartuStok INNER JOIN (MBarang LEFT OUTER JOIN" & vbCrLf
                SQL &= " MKategori ON MBarang.IDKategori = MKategori.NoID LEFT OUTER JOIN" & vbCrLf
                SQL &= " mjenisbarang ON MBarang.IDJenis = mjenisbarang.NoID) ON MBarang.NoID=MKartuStok.IDBarang " & vbCrLf
                SQL &= " LEFT OUTER JOIN MSatuan ON MKartuStok.IDSatuan = MSatuan.NoID "
                SQL &= " LEFT OUTER JOIN MAlamat ON MKartuStok.IDAlamat = MAlamat.NoID "
                SQL &= " LEFT OUTER JOIN MBarangD ON MKartuStok.IDBarangD = MBarangD.NoID "
                SQL &= " LEFT JOIN MJenisTransaksi ON MJenisTransaksi.ID=MKartuStok.IDJenisTransaksi" & vbCrLf
                SQL &= " LEFT JOIN (MGudang LEFT JOIN MWilayah ON MWilayah.NoID=MGudang.IDWilayah) ON MGudang.NoID=MKartuStok.IDGudang" & vbCrLf
                If IDJenisTransaksi = 4 Or IDJenisTransaksi = 5 Then
                    SQL &= " WHERE (MKartuStok.IDJenisTransaksi=" & 4 & " OR MKartuStok.IDJenisTransaksi=" & 5 & ") AND MkartuStok.IDTransaksi=" & IDTransaksi
                Else
                    SQL &= " WHERE MKartuStok.IDJenisTransaksi=" & IDJenisTransaksi & " AND MkartuStok.IDTransaksi=" & IDTransaksi
                End If
                SQL &= " ORDER BY MKartuStok.Tanggal, MJenisTransaksi.NoUrut, MKartuStok.NoID"
            End With

            ds = ExecuteDataset("MKartuStok", SQL)
            GC1.DataSource = ds.Tables("MKartuStok")

            XtraTabPage2.PageVisible = False
            XtraTabPage3.PageVisible = False

            'Jurnal
            SQL = "SELECT MJurnal.Kode, MJurnalD.IDAkun, MJurnal.Tanggal, MJurnal.KodeReff, MJurnalD.Debet, MJurnalD.Kredit, MJurnalD.SaldoAwal, MJurnalD.SaldoAkhir, MAkun.Nama AS Perkiraan, MJurnalD.Keterangan" & vbCrLf & _
                  " FROM MJurnal " & vbCrLf & _
                  " INNER JOIN MJurnalD ON MJurnal.ID=MJurnalD.IDJurnal " & vbCrLf & _
                  " LEFT JOIN MAkun ON MAkun.ID=MJurnalD.IDAkun " & vbCrLf & _
                  " WHERE MJurnal.IDJenisTransaksi=" & IDJenisTransaksi & " AND MJurnal.IDTransaksi=" & IDTransaksi
            ds = ExecuteDataset("MJurnal", SQL)
            GridControl3.DataSource = ds.Tables("MJurnal")
            XtraTabPage4.PageVisible = True
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
            If XtraTabControl1.SelectedTabPageIndex = 0 Then
                GV1.ExportToXls(dlgsave.FileName)
            ElseIf XtraTabControl1.SelectedTabPageIndex = 1 Then
                GridView1.ExportToXls(dlgsave.FileName)
            Else
                GridView1.ExportToXls(dlgsave.FileName)
            End If
            BukaFile(dlgsave.FileName)
        End If
        dlgsave.Dispose()
    End Sub
    Sub PrintPreview()
        If XtraTabControl1.SelectedTabPageIndex = 0 Then
            GC1.ShowPrintPreview()
        ElseIf XtraTabControl1.SelectedTabPageIndex = 1 Then
            GridControl1.ShowPrintPreview()
        Else
            GridControl2.ShowPrintPreview()
        End If
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
            Me.TopMost = False
            If XtraMessageBox.Show("Simpan Layout?", NamaAplikasi, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = Windows.Forms.DialogResult.Yes AndAlso xOtorisasi.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
                GV1.SaveLayoutToXml(folderLayouts & Me.Name & ".xml")
                GridView1.SaveLayoutToXml(folderLayouts & Me.Name & GridView1.Name & ".xml")
                GridView2.SaveLayoutToXml(folderLayouts & Me.Name & GridView2.Name & ".xml")

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

    Private Sub XtraTabControl1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles XtraTabControl1.Click

    End Sub

    Private Sub XtraTabControl1_SelectedPageChanged(ByVal sender As Object, ByVal e As DevExpress.XtraTab.TabPageChangedEventArgs) Handles XtraTabControl1.SelectedPageChanged
        If XtraTabControl1.SelectedTabPageIndex <> 0 Then
            PanelControl3.Visible = False
        Else
            PanelControl3.Visible = True
        End If
    End Sub

    Private Sub mnRefresh_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles mnRefresh.ItemClick
        SimpleButton8.PerformClick()
    End Sub

    Private Sub GC1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles GC1.Click

    End Sub
End Class