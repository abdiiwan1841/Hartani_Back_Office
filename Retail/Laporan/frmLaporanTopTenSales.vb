Imports System.Data.SqlClient 
Imports DevExpress.XtraEditors 
Imports DevExpress.XtraGrid.Views.Base 
Imports DevExpress.XtraEditors.Repository
Imports VPoint.mdlCetakCR 
Imports DevExpress.XtraGrid.Views.Grid.ViewInfo
Imports DevExpress.XtraGrid.Views.Grid 
Imports System.Data.OleDb

Public Class frmLaporanTopTenSales
    Public FormName As String = "LaporanTopTenSales"
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

            strsql = "Select MBarang.NoID, MBarang.IsNewItem AS ProdukBaru, MKategori.Nama Kategori, MBarang.Kode, MBarang.Nama NamaBarang, CASE WHEN IsNull((SELECT COUNT(MBarangD.NoID) FROM MBarangD WHERE MBarangD.IDBarang=MBarang.NoID AND MBarangD.IsActive=1), 0)<=1 THEN IsNull((SELECT TOP 1 MBarangD.Barcode FROM MBarangD WHERE MBarangD.IDBarang=MBarang.NoID AND MBarangD.IsActive=1),'') ELSE MBarang.Barcode END AS Barcode, MSatuan.Nama AS Satuan, MBarang.IsActive Aktif, Omset.* " & vbCrLf & _
                     " From MBarang left join MSatuan On Mbarang.IDSatuan=MSatuan.NoID left Join MKategori On Mbarang.IDKategori=MKategori.NoID " & vbCrLf & _
                     " Left Join (Select MJualD.IDBarang,Sum(MJualD.Qty*MJualD.Konversi) as QtyPenjualan, Sum(MJualD.Jumlah) as Penjualan,Count(distinct MJualD.IDJual) as Pembeli " & vbCrLf & _
                     " From " & _
                     " MJualD Inner Join MJual On MJualD.IDJual=MJual.NoID where MJual.Tanggal>='" & Format(TglDari.DateTime, "yyyy/MM/dd") & "' and MJual.Tanggal<'" & Format(TglSampai.DateTime.AddDays(1), "yyyy/MM/dd") & "' group by MJualD.IDBarang) Omset" & _
                     " On MBarang.NoID=Omset.IDBarang " & _
                     " WHERE MKategori.IsActive=1 "
            If Not ckAll.Checked Then
                strsql &= " AND MBarang.IsActive=1 "
            End If
            If ckProdukBaru.Checked Then
                strsql &= " AND MBarang.IsNewItem=1 "
            End If
            If txtKategori.Enabled And txtKategori.Text.Trim <> "" Then
                strsql = strsql & " AND MBarang.IDKategori=" & NullToLong(txtKategori.EditValue)
            End If
            If RadioButton1.Checked Then
                strsql = strsql & " order by Omset.QtyPenjualan Desc "
            ElseIf RadioButton2.Checked Then
                strsql = strsql & " order by Omset.Penjualan Desc "
            ElseIf RadioButton3.Checked Then
                strsql = strsql & " order by Omset.Pembeli Desc "
            ElseIf RadioButton4.Checked Then
                strsql = strsql & " order by MBarang.Kode "
            ElseIf RadioButton5.Checked Then
                strsql = strsql & " order by MBarang.Nama "
            ElseIf RadioButton6.Checked Then
                strsql = strsql & " order by MBarang.Barcode"
            ElseIf RadioButton7.Checked Then
                strsql = strsql & " order by Omset.QtyPenjualan"
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
    Sub InsertkanKeTable()
        Dim SQL As String = ""
        Dim ds As New DataSet
        Try
            SQL = "DELETE FROM MLapTopTen WHERE [IDUser]=" & IDUserAktif
            EksekusiSQL(SQL)

            SQL = "INSERT INTO [MLapTopTen] ([IDUser],[IP],[IDBarang],[Barcode],[Kode],[Nama],[QtyTerjual],[TotalPenjualan],[JumlahPembeli])" & vbCrLf & _
                  " Select " & IDUserAktif & ", '" & FixKoma(IPLokal) & "', MBarang.NoID, CASE WHEN IsNull((SELECT COUNT(MBarangD.NoID) FROM MBarangD WHERE MBarangD.IDBarang=MBarang.NoID AND MBarangD.IsActive=1), 0)<=1 THEN IsNull((SELECT TOP 1 MBarangD.Barcode FROM MBarangD WHERE MBarangD.IDBarang=MBarang.NoID AND MBarangD.IsActive=1),'') ELSE MBarang.Barcode END AS Barcode, " & vbCrLf & _
                  " MBarang.Kode, MBarang.Nama NamaBarang, Omset.QtyPenjualan, Omset.Penjualan, Omset.Pembeli " & vbCrLf & _
                  " FROM MBarang LEFT JOIN MSatuan ON MBarang.IDSatuan=MSatuan.NoID left Join MKategori On Mbarang.IDKategori=MKategori.NoID " & vbCrLf & _
                  " Left Join (Select MJualD.IDBarang,Sum(MJualD.Qty*MJualD.Konversi) as QtyPenjualan, Sum(MJualD.Jumlah) as Penjualan, Count(distinct MJualD.IDJual) as Pembeli " & vbCrLf & _
                  " From " & _
                  " MJualD Inner Join MJual On MJualD.IDJual=MJual.NoID where MJual.Tanggal>='" & Format(TglDari.DateTime, "yyyy/MM/dd") & "' and MJual.Tanggal<'" & Format(TglSampai.DateTime.AddDays(1), "yyyy/MM/dd") & "' group by MJualD.IDBarang) Omset" & _
                  " On MBarang.NoID=Omset.IDBarang " & _
                  " WHERE MKategori.IsActive=1 "
            If Not ckAll.Checked Then
                SQL &= " AND MBarang.IsActive=1 "
            End If
            If ckProdukBaru.Checked Then
                SQL &= " AND MBarang.IsNewItem=1 "
            End If
            If txtKategori.Enabled And txtKategori.Text.Trim <> "" Then
                SQL &= " AND MBarang.IDKategori=" & NullToLong(txtKategori.EditValue)
            End If
            If RadioButton1.Checked Then
                SQL &= " order by Omset.QtyPenjualan Desc "
            ElseIf RadioButton2.Checked Then
                SQL &= " order by Omset.Penjualan Desc "
            ElseIf RadioButton3.Checked Then
                SQL &= " order by Omset.Pembeli Desc "
            ElseIf RadioButton4.Checked Then
                SQL &= " order by MBarang.Kode "
            ElseIf RadioButton5.Checked Then
                SQL &= " order by MBarang.Nama "
            ElseIf RadioButton6.Checked Then
                SQL &= " order by MBarang.Barcode"
            ElseIf RadioButton7.Checked Then
                SQL &= " order by Omset.QtyPenjualan"
            End If
            EksekusiSQL(SQL)
        Catch ex As Exception
            XtraMessageBox.Show("Info Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK)
        End Try
    End Sub
    Sub PrintPreview()
        Dim NamaFile As String = ""
        Dim Judul As String = ""
        Try
            NamaFile = Application.StartupPath & "\Report\LaporanTopTen.rpt"
            If System.IO.File.Exists(NamaFile) Then
                If Not EditReport Then
                    InsertkanKeTable()
                End If
                If RadioButton1.Checked Then
                    Judul = RadioButton1.Text.Replace("Berdasarkan ", "")
                ElseIf RadioButton2.Checked Then
                    Judul = RadioButton2.Text.Replace("Berdasarkan ", "")
                ElseIf RadioButton3.Checked Then
                    Judul = RadioButton3.Text.Replace("Berdasarkan ", "")
                ElseIf RadioButton4.Checked Then
                    Judul = RadioButton4.Text.Replace("Berdasarkan ", "")
                ElseIf RadioButton5.Checked Then
                    Judul = RadioButton5.Text.Replace("Berdasarkan ", "")
                ElseIf RadioButton6.Checked Then
                    Judul = RadioButton6.Text.Replace("Berdasarkan ", "")
                ElseIf RadioButton7.Checked Then
                    Judul = RadioButton7.Text.Replace("Berdasarkan ", "")
                End If
                If ckProdukBaru.Checked Then
                    Judul &= " (Produk Baru)"
                End If
                If txtKategori.Enabled And txtKategori.Text.Trim <> "" Then
                    ViewReport(Me.MdiParent, IIf(EditReport, action_.Edit, action_.Preview), Application.StartupPath & "\Report\LaporanTopTen.rpt", "Laporan Penjualan " & Judul, , , "Judul='" & Judul & "'&IP='" & IPLokal.ToString & "'&IDUser=" & IDUserAktif & "&TglDari=CDATE(" & TglDari.DateTime.ToString("yyyy,MM,dd") & ")&TglSampai=CDATE(" & TglSampai.DateTime.ToString("yyyy,MM,dd") & ")&Kategori='" & txtNamaKategori.Text & "'")
                Else
                    ViewReport(Me.MdiParent, IIf(EditReport, action_.Edit, action_.Preview), Application.StartupPath & "\Report\LaporanTopTen.rpt", "Laporan Penjualan " & Judul, , , "Judul='" & Judul & "'&IP='" & IPLokal.ToString & "'&IDUser=" & IDUserAktif & "&TglDari=CDATE(" & TglDari.DateTime.ToString("yyyy,MM,dd") & ")&TglSampai=CDATE(" & TglSampai.DateTime.ToString("yyyy,MM,dd") & ")")
                End If
            Else
                XtraMessageBox.Show("File Report " & NamaFile & " tidak ditemukan.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If
        Catch ex As Exception
            XtraMessageBox.Show("Info Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
        End Try
    End Sub
    Private Sub CetakMRPTJual(ByVal Action As action_)
        Dim namafile As String
        Dim strsql As String = ""
        Dim view As ColumnView = GC1.FocusedView
        Try

            namafile = Application.StartupPath & "\report\LaporanToptenSales.rpt"
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

    Private Sub RadioButton4_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles RadioButton4.Click
        RefreshData()
    End Sub

    Private Sub RadioButton5_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles RadioButton5.Click
        RefreshData()
    End Sub

    Private Sub RadioButton6_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles RadioButton6.Click
        RefreshData()
    End Sub

    Private Sub RadioButton7_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles RadioButton7.Click
        RefreshData()
    End Sub

    Private Sub ckAll_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ckAll.CheckedChanged
        RefreshData()
    End Sub
End Class