Imports System.Data.SqlClient 
Imports DevExpress.XtraEditors 
Imports DevExpress.XtraGrid.Views.Base 
Imports DevExpress.XtraEditors.Repository
Imports VPoint.mdlCetakCR 
Imports DevExpress.XtraGrid.Views.Grid.ViewInfo
Imports DevExpress.XtraGrid.Views.Grid 
Imports System.Data.OleDb

Public Class frmLaporanOmzetPerSupplierPerDepartemen
    Public FormName As String = "LaporanOmzetPerSupplierPerDepartemen"
    Public FormEntriName As String = ""
    Public TableName As String = ""
    Public TableNameD As String = ""
    Public TableMaster As String = ""
    Public NoID As Long = -1

    Dim oda2 As SqlDataAdapter
    Dim ds As New DataSet
    Dim BS As New BindingSource
    Dim repckedit As New RepositoryItemCheckEdit
    Dim reppicedit As New RepositoryItemPictureEdit


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

    Private Sub RefreshLookUp()
        Dim ds As New DataSet
        Dim SQL As String = ""
        Try
            SQL = "SELECT NoID, Kode, Nama FROM MKategori WHERE IsActive=1 and MKategori.IDParent>0"
            ds = ExecuteDataset("MKategori", SQL)
            txtKategori.Properties.DataSource = ds.Tables("MKategori")
            txtKategori.Properties.DisplayMember = "Kode"
            txtKategori.Properties.ValueMember = "NoID"

            SQL = "SELECT NoID, Kode, Nama FROM MAlamat WHERE IsActive=1 and IsSupplier=1"
            ds = ExecuteDataset("MSupplier", SQL)
            txtSupplier.Properties.DataSource = ds.Tables("MSupplier")
            txtSupplier.Properties.DisplayMember = "Kode"
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


        If System.IO.File.Exists(FolderLayouts & FormName & ".xml") Then
            GV1.RestoreLayoutFromXml(FolderLayouts & FormName & ".xml")
        End If
        If System.IO.File.Exists(FolderLayouts & FormName & GridView1.Name & ".xml") Then
            GridView1.RestoreLayoutFromXml(FolderLayouts & FormName & GridView1.Name & ".xml")
        End If

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
        Dim strsql As String = ""
        Dim Cur As Cursor = Windows.Forms.Cursor.Current
        Try
            Dim curentcursor As Cursor = Windows.Forms.Cursor.Current
            Windows.Forms.Cursor.Current = Cursors.WaitCursor
            dlg = New DevExpress.Utils.WaitDialogForm(String.Format("Sedang merefresh data.{0}MOHON TUNGGU ...", vbCrLf), Application.ProductName)
            dlg.TopMost = False
            dlg.Show()
            Windows.Forms.Cursor.Current = Cursors.WaitCursor

            strsql = "Select MBarang.NoID,MKategori.Nama Kategori,MBarang.Kode,MBarang.Nama NamaBarang,Mbarang.Barcode,MSatuan.Nama as Satuan,MBarang.IsActive Aktif,Omset.* From MBarang left join MSatuan On Mbarang.IDSatuan=MSatuan.NoID left Join MKategori On Mbarang.IDKategori=MKategori.NoID Left Join " & _
                     " (Select MJualD.IDBarang,Sum(MJualD.Qty*MJualD.Konversi) as QtyPenjualan, Sum(MJualD.Jumlah) as Penjualan, CASE WHEN IsNull(MKategori.DefProvitMargin,0)=0 THEN Sum(MJualD.Jumlah-IsNull(MJualD.JumlahDiscNotaRp,0)) - Sum(ISNULL(MJualD.Qty,0) * ISNULL(MJualD.HargaPokok,0)) ELSE Sum(MJualD.Jumlah-IsNull(MJualD.JumlahDiscNotaRp,0)) * MKategori.DefProvitMargin / 100 END as JumlahLaba, CASE WHEN IsNull(MKategori.DefProvitMargin,0)=0 THEN Sum(ISNULL(MJualD.Qty,0) * ISNULL(MJualD.HargaPokok,0)) ELSE Sum(MJualD.Jumlah-IsNull(MJualD.JumlahDiscNotaRp,0)) * (1-(MKategori.DefProvitMargin / 100)) END as JumlahHPP, CASE WHEN IsNull(MKategori.DefProvitMargin,0)=0 THEN 100*(case when Sum(MJualD.Jumlah-IsNull(MJualD.JumlahDiscNotaRp,0))<>0 AND (Sum(ISNULL(MJualD.Qty,0) * ISNULL(MJualD.HargaPokok,0)))<>0 then (Sum(MJualD.Jumlah-IsNull(MJualD.JumlahDiscNotaRp,0))/(Sum(ISNULL(MJualD.Qty,0) * ISNULL(MJualD.HargaPokok,0))))-1 else 0.0 end) ELSE MKategori.DefProvitMargin END as [MarginLaba (%)], Count(distinct MJualD.IDJual) as Pembeli From " & _
                     " (MJualD Inner Join MJual On MJualD.IDJual=MJual.NoID) INNER JOIN (MBarang INNER JOIN MKategori ON MKategori.NoID=MBarang.IDKategori) ON MBarang.NoID=MJualD.IDBarang WHERE MJual.Tanggal>='" & Format(TglDari.DateTime, "yyyy/MM/dd") & "' and MJual.Tanggal<'" & Format(TglSampai.DateTime.AddDays(1), "yyyy/MM/dd") & "' group by MJualD.IDBarang, MKategori.DefProvitMargin) Omset" & _
                     " On MBarang.NoID=Omset.IDBarang " & _
                     " WHERE MKategori.IsActive=1 "
            If txtKategori.Enabled And txtKategori.Text.Trim <> "" Then
                strsql &= " AND MBarang.IDKategori=" & NullToLong(txtKategori.EditValue)
            End If
            If txtSupplier.Enabled And txtSupplier.Text.Trim <> "" Then
                strsql &= " AND (MBarang.IDSupplier1=" & NullToLong(txtSupplier.EditValue) & vbCrLf & _
                          " OR MBarang.IDSupplier2=" & NullToLong(txtSupplier.EditValue) & vbCrLf & _
                          " OR MBarang.IDSupplier3=" & NullToLong(txtSupplier.EditValue) & vbCrLf & _
                          " OR MBarang.IDSupplier4=" & NullToLong(txtSupplier.EditValue) & vbCrLf & _
                          " OR MBarang.IDSupplier5=" & NullToLong(txtSupplier.EditValue) & ")"
            End If
            If RadioButton1.Checked Then
                strsql &= " order by Omset.QtyPenjualan Desc "
            ElseIf RadioButton2.Checked Then
                strsql &= " order by Omset.Penjualan Desc "
            ElseIf RadioButton3.Checked Then
                strsql &= " order by Omset.QtyPenjualan Desc, Omset.Penjualan Desc "
            End If
            ocmd2.Connection = cn
            ocmd2.CommandType = CommandType.Text
            ocmd2.CommandText = strsql

            cn.Open()
            oda2 = New SqlDataAdapter(ocmd2)
            oda2.TableMappings.Add("Tabel", "SalesTopTen")
            If ds.Tables("SalesTopTen") Is Nothing Then
            Else
                ds.Tables("SalesTopTen").Clear()
            End If
            oda2.Fill(ds, "SalesTopTen")
            BS.DataSource = ds.Tables("SalesTopTen")
            GC1.DataSource = BS.DataSource
            GridView1.PopulateColumns()
            If System.IO.File.Exists(FolderLayouts & Me.Name & XtraTabControl1.SelectedTabPage.Name & ".xml") Then
                GridView1.RestoreLayoutFromXml(FolderLayouts & Me.Name & XtraTabControl1.SelectedTabPage.Name & ".xml")
            End If
            GV1.OptionsDetail.SmartDetailExpandButtonMode = DetailExpandButtonMode.AlwaysEnabled
            For Each ctrl As GridView In GC1.Views
                With ctrl
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
                    Next
                End With
            Next

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
            'GV1.ShowFindPanel()
            'GridView1.ShowFindPanel()
            'lbDaftar.Text = "Laporan Laba Kotor (Provit) Per Barang"
            'Me.Text = lbDaftar.Text.ToString
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
        cmdPreview.PerformClick()
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
        CetakMRPTJual(IIf(EditReport, action_.Edit, action_.Preview))
    End Sub
    Private Sub CetakMRPTJual(ByVal Action As action_)
        Dim namafile As String
        Dim strsql As String = ""
        Dim view As ColumnView = GC1.FocusedView
        Try
            If RadioButton1.Checked Then
                namafile = Application.StartupPath & "\report\LaporanOmzetPerSupplierPerDepartemen1.rpt"
            ElseIf RadioButton1.Checked Then
                namafile = Application.StartupPath & "\report\LaporanOmzetPerSupplierPerDepartemen2.rpt"
            Else
                namafile = Application.StartupPath & "\report\LaporanOmzetPerSupplierPerDepartemen3.rpt"
            End If

            If System.IO.File.Exists(namafile) Then
                If EditReport Then
                    Action = action_.Edit
                Else
                    Action = action_.Preview
                End If
                If txtKategori.Text <> "" Then
                    ViewReport(Me.ParentForm, Action, namafile, Me.Text, , , "DariTanggal=cdate(" & TglDari.DateTime.ToString("yyyy,MM,dd") & ")&SampaiTanggal=cdate(" & TglSampai.DateTime.ToString("yyyy,MM,dd") & ")&IDKategori=" & NullToLong(txtKategori.EditValue) & "")
                Else
                    ViewReport(Me.ParentForm, Action, namafile, Me.Text, , , "DariTanggal=cdate(" & TglDari.DateTime.ToString("yyyy,MM,dd") & ")&SampaiTanggal=cdate(" & TglSampai.DateTime.ToString("yyyy,MM,dd") & ")" & "")
                End If
            Else
                DevExpress.XtraEditors.XtraMessageBox.Show("Nama File :" & namafile & " tidak ditemukan.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Stop)
            End If
        Catch EX As Exception
            XtraMessageBox.Show("Untuk mencetak, pilih item yang mau dicetak dan click cetak faktur.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
            'DevExpress.XtraEditors.XtraMessageBox.Show("Kesalahan:" & EX.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub
    Private Sub SimpleButton4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SimpleButton4.Click
        ExportExcel()
    End Sub

    'Private Sub SimpleButton5_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SimpleButton5.Click
    '    PrintPreview()
    'End Sub

    Private Sub BarButtonItem5_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem5.ItemClick
        ExportExcel()
    End Sub

    Private Sub LabelControl1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LabelControl1.Click
        TglDari.Enabled = Not TglDari.Enabled
        TglSampai.Enabled = TglDari.Enabled
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
                GridView1.SaveLayoutToXml(FolderLayouts & FormName & GridView1.Name & ".xml")
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




    Private Sub txtKodeBrg_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtSupplier.EditValueChanged
        Try
            txtNamaSupplier.Text = NullToStr(EksekusiSQlSkalarNew("SELECT Nama FROM MAlamat WHERE NoID=" & NullToLong(txtSupplier.EditValue)))
        Catch ex As Exception

        End Try
    End Sub



    Private Sub XtraTabControl1_SelectedPageChanging(ByVal sender As Object, ByVal e As DevExpress.XtraTab.TabPageChangingEventArgs) Handles XtraTabControl1.SelectedPageChanging
        If e.PrevPage Is Nothing Then
        Else
            GridView1.SaveLayoutToXml(FolderLayouts & Me.Name & e.PrevPage.Name & ".xml")
        End If

    End Sub

    Private Sub txtKategori_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtKategori.EditValueChanged
        txtNamaKategori.Text = NullToStr(EksekusiSQlSkalarNew("SELECT Nama FROM MKategori WHERE NoID=" & NullToLong(txtKategori.EditValue)))
    End Sub

    Private Sub SimpleButton6_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SimpleButton6.Click
        DialogResult = Windows.Forms.DialogResult.OK
        Me.Close()
        Me.Dispose()
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

    Private Sub cmdPreview_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdPreview.Click

    End Sub

    Private Sub BarButtonItem1_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem1.ItemClick
        Dim namafile As String
        Dim strsql As String = ""
        Dim view As ColumnView = GC1.FocusedView
        Dim Action As action_
        Try
            namafile = Application.StartupPath & "\report\LaporanOmzetPerSupplierPerDepartemen1.rpt"
            If System.IO.File.Exists(namafile) Then
                If EditReport Then
                    Action = action_.Edit
                Else
                    Action = action_.Preview
                    HitungTotal(1)
                End If
                If txtKategori.Text <> "" Then
                    ViewReport(Me.ParentForm, Action, namafile, Me.Text, , , IIf(txtSupplier.Text <> "", "IDSupplier=" & NullToLong(txtSupplier.EditValue) & "&NamaSupplier='" & txtSupplier.Text & " - " & txtNamaSupplier.Text & "'&", "") & "Periode=" & TglDari.DateTime.ToString("yyyy") & "&IDUser=" & IDUserAktif & "&IDKategori=" & NullToLong(txtKategori.EditValue), "")
                Else
                    ViewReport(Me.ParentForm, Action, namafile, Me.Text, , , IIf(txtSupplier.Text <> "", "IDSupplier=" & NullToLong(txtSupplier.EditValue) & "&NamaSupplier='" & txtSupplier.Text & " - " & txtNamaSupplier.Text & "'&", "") & "Periode=" & TglDari.DateTime.ToString("yyyy") & "&IDUser=" & IDUserAktif & "&IDKategori=-1", "")
                End If
            Else
                DevExpress.XtraEditors.XtraMessageBox.Show("Nama File :" & namafile & " tidak ditemukan.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Stop)
            End If
        Catch EX As Exception
            XtraMessageBox.Show("Untuk mencetak, pilih item yang mau dicetak dan click cetak faktur.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
            'DevExpress.XtraEditors.XtraMessageBox.Show("Kesalahan:" & EX.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub HitungTotal(ByVal TipeTigaBulan As Integer)
        Dim cn As New SqlConnection
        Dim com As New SqlCommand
        Dim ODa As New SqlDataAdapter
        Dim ds As New DataSet
        Dim SQL As String = "", TDari As Date, TSampai As Date

        Try
            cn.ConnectionString = StrKonSql
            cn.Open()
            com.Connection = cn
            ODa.SelectCommand = com
            Try
                SQL = " CREATE TABLE [dbo].[MLapOmzetPerDepPerSupplier](" & vbCrLf & _
                    " [NoID] [int] IDENTITY(1,1) NOT NULL," & vbCrLf & _
                    " [Tahun] [int] NULL," & vbCrLf & _
                    " [IDUser] [int] NULL," & vbCrLf & _
                    " [IDBarang] [bigint] NULL," & vbCrLf & _
                    " [1] [money] NULL," & vbCrLf & _
                    " [P1] [numeric](18, 0) NULL," & vbCrLf & _
                    " [2] [money] NULL," & vbCrLf & _
                    " [P2] [numeric](18, 0) NULL," & vbCrLf & _
                    " [3] [money] NULL," & vbCrLf & _
                    " [P3] [numeric](18, 0) NULL," & vbCrLf & _
                    " [4] [money] NULL," & vbCrLf & _
                    " [P4] [numeric](18, 0) NULL," & vbCrLf & _
                    " [5] [money] NULL," & vbCrLf & _
                    " [P5] [numeric](18, 0) NULL," & vbCrLf & _
                    " [6] [money] NULL," & vbCrLf & _
                    " [P6] [numeric](18, 0) NULL," & vbCrLf & _
                    " [7] [money] NULL," & vbCrLf & _
                    " [P7] [numeric](18, 0) NULL," & vbCrLf & _
                    " [8] [money] NULL," & vbCrLf & _
                    " [P8] [numeric](18, 0) NULL," & vbCrLf & _
                    " [9] [money] NULL," & vbCrLf & _
                    " [P9] [numeric](18, 0) NULL," & vbCrLf & _
                    " [10] [money] NULL," & vbCrLf & _
                    " [P10] [numeric](18, 0) NULL," & vbCrLf & _
                    " [11] [money] NULL," & vbCrLf & _
                    " [P11] [numeric](18, 0) NULL," & vbCrLf & _
                    " [12] [money] NULL," & vbCrLf & _
                    " [P12] [numeric](18, 0) NULL," & vbCrLf & _
                    " CONSTRAINT [PK_MLapOmzetPerDepPerSupplier] PRIMARY KEY CLUSTERED " & vbCrLf & _
                    " (" & vbCrLf & _
                    " [NoID] ASC" & vbCrLf & _
                    " )WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]" & vbCrLf & _
                    " ) ON [PRIMARY]"
                com.CommandText = SQL
                com.ExecuteNonQuery()
            Catch ex As Exception

            End Try

            SQL = "DELETE FROM MLapOmzetPerDepPerSupplier WHERE IDUser=" & IDUserAktif
            com.CommandText = SQL
            com.ExecuteNonQuery()

            SQL = "INSERT INTO MLapOmzetPerDepPerSupplier (Tahun, IDUser, IDBarang) SELECT " & TglDari.DateTime.ToString("yyyy") & "," & IDUserAktif & ", MBarang.NoID FROM MBarang INNER JOIN MBarangD ON MBarang.NoID=MBarangD.IDBarang WHERE MBarang.IsActive=1 AND MBarangD.IsActive=1" & IIf(txtSupplier.Text <> "", " AND (MBarang.IDSupplier1=" & NullToLong(txtSupplier.EditValue) & " OR MBarang.IDSupplier2=" & NullToLong(txtSupplier.EditValue) & " OR MBarang.IDSupplier3=" & NullToLong(txtSupplier.EditValue) & " OR MBarang.IDSupplier4=" & NullToLong(txtSupplier.EditValue) & " OR MBarang.IDSupplier5=" & NullToLong(txtSupplier.EditValue) & ")", "") & "" & IIf(txtKategori.Text <> "", " AND MBarang.IDKategori=" & NullToLong(txtKategori.EditValue), "") & " GROUP BY MBarang.NoID"
            com.CommandText = SQL
            com.ExecuteNonQuery()

            For i = (TipeTigaBulan * 4) - 3 To (TipeTigaBulan * 4)
                TDari = CDate(TglDari.DateTime.ToString("yyyy-" & i.ToString("00") & "-01"))
                If i = 12 Then
                    TSampai = CDate((NullToLong(TglDari.DateTime.ToString("yyyy")) + 1).ToString("0000") & "-01-01")
                Else
                    TSampai = CDate(TglDari.DateTime.ToString("yyyy-" & (i + 1).ToString("00") & "-01"))
                End If
                SQL = " UPDATE MLapOmzetPerDepPerSupplier SET " & vbCrLf & _
                    " [" & i & "]=IsNull(Jual.Jumlah,0)," & vbCrLf & _
                    " [P" & i & "]=IsNull(Jual.Pembeli,0)" & vbCrLf & _
                    " FROM MLapOmzetPerDepPerSupplier LEFT JOIN (" & vbCrLf & _
                    " SELECT MJualD.IDBarang, " & TDari.ToString("yyyy") & " AS Th, SUM(MJualD.Jumlah) AS Jumlah, SUM(MJualD.Qty*MJualD.Konversi) AS Pembeli FROM (MJual INNER JOIN MJualD ON MJual.NoID=MJualD.IDJual) INNER JOIN MBarang ON MBarang.NoID=MJualD.IDBarang " & vbCrLf & _
                    " WHERE MJual.IsPosted=1 AND MJual.IsPOS=1 AND MJual.Tanggal>='" & TDari.ToString("yyyy-MM-dd") & "' AND MJual.Tanggal<'" & TSampai.ToString("yyyy-MM-dd") & "'" & vbCrLf & _
                    IIf(txtSupplier.Text <> "", " AND (MBarang.IDSupplier1=" & NullToLong(txtSupplier.EditValue) & " OR MBarang.IDSupplier2=" & NullToLong(txtSupplier.EditValue) & " OR MBarang.IDSupplier3=" & NullToLong(txtSupplier.EditValue) & " OR MBarang.IDSupplier4=" & NullToLong(txtSupplier.EditValue) & " OR MBarang.IDSupplier5=" & NullToLong(txtSupplier.EditValue) & ")", "") & "" & IIf(txtKategori.Text <> "", " AND MBarang.IDKategori=" & NullToLong(txtKategori.EditValue), "") & vbCrLf & _
                    " GROUP BY MJualD.IDBarang" & vbCrLf & _
                    " ) AS Jual ON Jual.IDBarang=MLapOmzetPerDepPerSupplier.IDBarang AND Jual.Th=MLapOmzetPerDepPerSupplier.Tahun WHERE MLapOmzetPerDepPerSupplier.IDUser=" & IDUserAktif & " AND MLapOmzetPerDepPerSupplier.Tahun=" & TglDari.DateTime.ToString("yyyy")
                com.CommandText = SQL
                com.ExecuteNonQuery()
            Next
        Catch ex As Exception
            XtraMessageBox.Show("Info Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
        Finally
            If cn.State = ConnectionState.Open Then
                cn.Close()
            End If
            cn.Dispose()
            com.Dispose()
            ODa.Dispose()
            ds.Dispose()
        End Try
    End Sub

    Private Sub BarButtonItem2_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem2.ItemClick
        Dim namafile As String
        Dim strsql As String = ""
        Dim view As ColumnView = GC1.FocusedView
        Dim Action As action_
        Try
            namafile = Application.StartupPath & "\report\LaporanOmzetPerSupplierPerDepartemen2.rpt"
            If System.IO.File.Exists(namafile) Then
                If EditReport Then
                    Action = action_.Edit
                Else
                    Action = action_.Preview
                    HitungTotal(2)
                End If
                If txtKategori.Text <> "" Then
                    ViewReport(Me.ParentForm, Action, namafile, Me.Text, , , IIf(txtSupplier.Text <> "", "IDSupplier=" & NullToLong(txtSupplier.EditValue) & "&NamaSupplier='" & txtSupplier.Text & " - " & txtNamaSupplier.Text & "'&", "") & "Periode=" & TglDari.DateTime.ToString("yyyy") & "&IDUser=" & IDUserAktif & "&IDKategori=" & NullToLong(txtKategori.EditValue), "")
                Else
                    ViewReport(Me.ParentForm, Action, namafile, Me.Text, , , IIf(txtSupplier.Text <> "", "IDSupplier=" & NullToLong(txtSupplier.EditValue) & "&NamaSupplier='" & txtSupplier.Text & " - " & txtNamaSupplier.Text & "'&", "") & "Periode=" & TglDari.DateTime.ToString("yyyy") & "&IDUser=" & IDUserAktif & "&IDKategori=-1", "")
                End If
            Else
                DevExpress.XtraEditors.XtraMessageBox.Show("Nama File :" & namafile & " tidak ditemukan.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Stop)
            End If
        Catch EX As Exception
            XtraMessageBox.Show("Untuk mencetak, pilih item yang mau dicetak dan click cetak faktur.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
            'DevExpress.XtraEditors.XtraMessageBox.Show("Kesalahan:" & EX.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub
    Private Sub BarButtonItem3_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem3.ItemClick
        Dim namafile As String
        Dim strsql As String = ""
        Dim view As ColumnView = GC1.FocusedView
        Dim Action As action_
        Try
            namafile = Application.StartupPath & "\report\LaporanOmzetPerSupplierPerDepartemen3.rpt"
            If System.IO.File.Exists(namafile) Then
                If EditReport Then
                    Action = action_.Edit
                Else
                    Action = action_.Preview
                    HitungTotal(3)
                End If
                If txtKategori.Text <> "" Then
                    ViewReport(Me.ParentForm, Action, namafile, Me.Text, , , IIf(txtSupplier.Text <> "", "IDSupplier=" & NullToLong(txtSupplier.EditValue) & "&NamaSupplier='" & txtSupplier.Text & " - " & txtNamaSupplier.Text & "'&", "") & "Periode=" & TglDari.DateTime.ToString("yyyy") & "&IDUser=" & IDUserAktif & "&IDKategori=" & NullToLong(txtKategori.EditValue), "")
                Else
                    ViewReport(Me.ParentForm, Action, namafile, Me.Text, , , IIf(txtSupplier.Text <> "", "IDSupplier=" & NullToLong(txtSupplier.EditValue) & "&NamaSupplier='" & txtSupplier.Text & " - " & txtNamaSupplier.Text & "'&", "") & "Periode=" & TglDari.DateTime.ToString("yyyy") & "&IDUser=" & IDUserAktif & "&IDKategori=-1", "")
                End If
            Else
                DevExpress.XtraEditors.XtraMessageBox.Show("Nama File :" & namafile & " tidak ditemukan.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Stop)
            End If
        Catch EX As Exception
            XtraMessageBox.Show("Untuk mencetak, pilih item yang mau dicetak dan click cetak faktur.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
            'DevExpress.XtraEditors.XtraMessageBox.Show("Kesalahan:" & EX.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub
End Class