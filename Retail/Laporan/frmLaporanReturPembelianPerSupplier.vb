Imports System.Data.SqlClient 
Imports DevExpress.XtraEditors 
Imports DevExpress.XtraGrid.Views.Base 
Imports DevExpress.XtraEditors.Repository
Imports VPoint.mdlCetakCR 
Imports DevExpress.XtraGrid.Views.Grid.ViewInfo
Imports DevExpress.XtraGrid.Views.Grid 
Imports System.Data.OleDb

Public Class frmLaporanReturPembelianPerSupplier
    Public FormName As String = "ReturPembelianPerSupplierBulanan"
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
            TglSampai.DateTime = TanggalSystem
            Me.lbDaftar.Text = Me.Text
            RefreshLookUp()
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
        Dim SQL As String = ""
        Dim ds As New DataSet
        Try
            SQL = "SELECT NoID, Kode, Nama FROM MAlamat WHERE IsNull(IsActive,0)=1 AND IsNull(IsSupplier,0)=1"
            ds = ExecuteDataset("Data", SQL)
            txtSupplier.Properties.DataSource = ds.Tables("Data")
            txtSupplier.Properties.DisplayMember = "Nama"
            txtSupplier.Properties.ValueMember = "NoID"
        Catch ex As Exception
            XtraMessageBox.Show("Info Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
        Finally
            If Not ds Is Nothing Then
                ds.Dispose()
            End If
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



        If System.IO.File.Exists(FolderLayouts & FormName & ".xml") Then
            GV1.RestoreLayoutFromXml(FolderLayouts & FormName & ".xml")
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

            If rbMasuk.Checked Then
                strsql = "SELECT MReturBeli.NoID, MReturBeli.Kode, MReturBeli.Tanggal, MBayarHutang.TglKembali, MAlamat.Kode AS KodeSupplier, MAlamat.Nama AS NamaSupplier, MAlamat.Dept, MReturBeli.Total, (CASE WHEN MBayarHutang.TglKembali IS NULL THEN 'M' ELSE 'K' END) AS Ket, " & _
                         " (CASE WHEN IsNull(MAlamat.IsPKP,0)=1 THEN MReturBeli.Total ELSE 0 END) AS BKP, (CASE WHEN IsNull(MAlamat.IsPKP,0)=0 THEN MReturBeli.Total ELSE 0 END) AS NonBKP " & vbCrLf & _
                         " FROM MReturBeli " & vbCrLf & _
                         " LEFT JOIN MAlamat ON MAlamat.NoID=MReturBeli.IDSupplier " & vbCrLf & _
                         " LEFT JOIN (MBayarHutang INNER JOIN MBayarHutangDRetur ON MBayarHutang.NoID=MBayarHutangDRetur.IDBayarHutang) ON MReturBeli.NoID=MBayarHutangDRetur.IDReturBeli " & vbCrLf & _
                         " WHERE MReturBeli.IsPosted=1"
                If TglDari.Enabled AndAlso TglDari.Text <> "" Then
                    strsql &= " AND MReturBeli.Tanggal>='" & TglDari.DateTime.ToString("yyyy-MM-dd") & "' AND MReturBeli.Tanggal<'" & TglSampai.DateTime.AddDays(1).ToString("yyyy-MM-dd") & "'"
                End If
            Else
                strsql = "SELECT MReturBeli.NoID, MReturBeli.Kode, MReturBeli.Tanggal, MBayarHutang.TglKembali, MAlamat.Kode AS KodeSupplier, MAlamat.Nama AS NamaSupplier, MAlamat.Dept, MReturBeli.Total, (CASE WHEN MBayarHutang.TglKembali IS NULL THEN 'M' ELSE 'K' END) AS Ket, " & _
                         " (CASE WHEN IsNull(MAlamat.IsPKP,0)=1 THEN MReturBeli.Total ELSE 0 END) AS BKP, (CASE WHEN IsNull(MAlamat.IsPKP,0)=0 THEN MReturBeli.Total ELSE 0 END) AS NonBKP " & vbCrLf & _
                         " FROM MReturBeli " & vbCrLf & _
                         " INNER JOIN (MBayarHutang INNER JOIN MBayarHutangDRetur ON MBayarHutang.NoID=MBayarHutangDRetur.IDBayarHutang) ON MReturBeli.NoID=MBayarHutangDRetur.IDReturBeli " & vbCrLf & _
                         " LEFT JOIN MAlamat ON MAlamat.NoID=MReturBeli.IDSupplier " & vbCrLf & _
                         " WHERE MReturBeli.IsPosted=1 AND MBayarHutang.IsPosted=1 "
                If TglDari.Enabled AndAlso TglDari.Text <> "" Then
                    strsql &= " AND MBayarHutang.TglKembali>='" & TglDari.DateTime.ToString("yyyy-MM-dd") & "' AND MBayarHutang.TglKembali<'" & TglSampai.DateTime.AddDays(1).ToString("yyyy-MM-dd") & "'"
                End If
            End If
            If txtSupplier.Text <> "" Then
                strsql &= " AND MReturBeli.IDSupplier=" & NullToLong(txtSupplier.EditValue)
            End If

            ocmd2.Connection = cn
            ocmd2.CommandType = CommandType.Text
            ocmd2.CommandText = strsql

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
        'PrintPreview()
    End Sub
    Sub ExportExcel()
        Dim dlgsave As New SaveFileDialog
        dlgsave.Title = "Export Daftar ke Excel"
        dlgsave.Filter = "Excel Files|*.xls"
        If dlgsave.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
            If XtraTabControl1.SelectedTabPageIndex = 0 Then
                GV1.ExportToXls(dlgsave.FileName)
            End If
            BukaFile(dlgsave.FileName)
        End If
        dlgsave.Dispose()
    End Sub
    'Sub PrintPreview()
    '    Dim namafile As String
    '    namafile = Application.StartupPath & "\report\Omset1.rpt"
    '    CetakMRPTJual(IIf(EditReport, action_.Edit, action_.Preview), namafile)
    '    namafile = Application.StartupPath & "\report\Omset2.rpt"
    '    CetakMRPTJual(IIf(EditReport, action_.Edit, action_.Preview), namafile)
    '    namafile = Application.StartupPath & "\report\Omset3.rpt"
    '    CetakMRPTJual(IIf(EditReport, action_.Edit, action_.Preview), namafile)
    '    namafile = Application.StartupPath & "\report\Omset4.rpt"
    '    CetakMRPTJual(IIf(EditReport, action_.Edit, action_.Preview), namafile)
    'End Sub
    'Sub PrintPreviewDisc()
    '    Dim namafile As String
    '    namafile = Application.StartupPath & "\report\OmsetDisc1.rpt"
    '    CetakMRPTJual(IIf(EditReport, action_.Edit, action_.Preview), namafile)
    '    namafile = Application.StartupPath & "\report\OmsetDisc2.rpt"
    '    CetakMRPTJual(IIf(EditReport, action_.Edit, action_.Preview), namafile)
    '    namafile = Application.StartupPath & "\report\OmsetDisc3.rpt"
    '    CetakMRPTJual(IIf(EditReport, action_.Edit, action_.Preview), namafile)
    '    namafile = Application.StartupPath & "\report\OmsetDisc4.rpt"
    '    CetakMRPTJual(IIf(EditReport, action_.Edit, action_.Preview), namafile)

    'End Sub
    'Private Sub CetakMRPTJual(ByVal Action As action_, ByVal NamaFile As String, ByVal BolehkanKategoriUtama As Boolean)
    '    Dim strsql As String = ""
    '    Dim view As ColumnView = GC1.FocusedView
    '    Try

    '        If System.IO.File.Exists(NamaFile) Then
    '            If EditReport Then
    '                Action = action_.Edit
    '            Else
    '                Action = action_.Preview
    '            End If
    '            ViewReport(Me.ParentForm, Action, NamaFile, Me.Text, , , "IsPKP=" & IIf(rbKeluar.Checked, 0, IIf(rbMasuk.Checked, 1, 2)) & "&Periode=CDATE(" & TglPeriode.DateTime.ToString("yyyy,MM,01") & ")")
    '        Else
    '            DevExpress.XtraEditors.XtraMessageBox.Show("Nama File :" & NamaFile & " tidak ditemukan.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Stop)
    '        End If
    '    Catch EX As Exception
    '        XtraMessageBox.Show("Untuk mencetak, pilih item yang mau dicetak dan click cetak faktur.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
    '        'DevExpress.XtraEditors.XtraMessageBox.Show("Kesalahan:" & EX.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
    '    End Try
    'End Sub
    Private Sub SimpleButton4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SimpleButton4.Click
        ExportExcel()
    End Sub

    Private Sub CetakMRPTJual(ByVal Action As action_, ByVal NamaFile As String, ByVal BolehkanKategoriUtama As Boolean)
        Dim strsql As String = ""
        Dim view As ColumnView = GC1.FocusedView
        Try

            If System.IO.File.Exists(NamaFile) Then
                If EditReport Then
                    Action = action_.Edit
                Else
                    Action = action_.Preview
                End If
                ViewReport(Me.ParentForm, Action, NamaFile, Me.Text, "MONTH({MLapReturPembelianBulanan.Periode})=MONTH({@Periode}) AND YEAR({MLapReturPembelianBulanan.Periode})=YEAR({@Periode}) AND {MLapReturPembelianBulanan.IDUser}=" & IDUserAktif, , "Periode=CDATE(" & TglDari.DateTime.ToString("yyyy,MM,01") & ")")
            Else
                DevExpress.XtraEditors.XtraMessageBox.Show("Nama File :" & NamaFile & " tidak ditemukan.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Stop)
            End If
        Catch EX As Exception
            XtraMessageBox.Show("Untuk mencetak, pilih item yang mau dicetak dan click cetak faktur.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
            'DevExpress.XtraEditors.XtraMessageBox.Show("Kesalahan:" & EX.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub SimpleButton5_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SimpleButton5.Click
        Dim namafile As String
        namafile = Application.StartupPath & "\report\LaporanReturMasuk.rpt"
        If EditReport Then
            If txtSupplier.Text = "" Then
                ViewReport(Me.MdiParent, action_.Edit, namafile, "Laporan Retur Masuk", , , "TglDari=CDATE(" & TglDari.DateTime.ToString("yyyy,MM,dd") & ")&TglSampai=CDATE(" & TglSampai.DateTime.ToString("yyyy,MM,dd") & ")")
            Else
                ViewReport(Me.MdiParent, action_.Edit, namafile, "Laporan Retur Masuk", , , "TglDari=CDATE(" & TglDari.DateTime.ToString("yyyy,MM,dd") & ")&TglSampai=CDATE(" & TglSampai.DateTime.ToString("yyyy,MM,dd") & ")&IDSupplier=" & NullToLong(txtSupplier.EditValue))
            End If
        Else
            If txtSupplier.Text = "" Then
                ViewReport(Me.MdiParent, action_.Preview, namafile, "Laporan Retur Masuk", , , "TglDari=CDATE(" & TglDari.DateTime.ToString("yyyy,MM,dd") & ")&TglSampai=CDATE(" & TglSampai.DateTime.ToString("yyyy,MM,dd") & ")")
            Else
                ViewReport(Me.MdiParent, action_.Preview, namafile, "Laporan Retur Masuk", , , "TglDari=CDATE(" & TglDari.DateTime.ToString("yyyy,MM,dd") & ")&TglSampai=CDATE(" & TglSampai.DateTime.ToString("yyyy,MM,dd") & ")&IDSupplier=" & NullToLong(txtSupplier.EditValue))
            End If
        End If
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

    Private Sub SimpleButton6_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SimpleButton6.Click
        DialogResult = Windows.Forms.DialogResult.OK
        Me.Close()
        Me.Dispose()
    End Sub

    Private Sub SimpleButton1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        'HitungPerDeptBersih()
    End Sub
    'Sub HitungDisckondibebankanDept20()
    '    Dim Tgl As Integer
    '    Dim TglSampai As Date
    '    Dim strsql As String = ""
    '    Dim StrTglField As String = ""
    '    TglSampai = DateAdd(DateInterval.Day, (1 - TglPeriode.DateTime.Day), TglPeriode.DateTime)
    '    TglSampai = DateAdd(DateInterval.Month, 1, TglSampai)
    '    TglSampai = DateAdd(DateInterval.Day, -1, TglSampai)
    '    Dim x As DevExpress.Utils.WaitDialogForm = Nothing
    '    Try
    '        x = New DevExpress.Utils.WaitDialogForm("Sedang proses menghitung", NamaAplikasi)
    '        x.Show()
    '        ProgressBarControl1.Visible = True
    '        ProgressBarControl1.Position = 0
    '        Application.DoEvents()
    '        strsql = "delete from MRekapJual where Periode='" & Format(TglPeriode.DateTime, "yyyy-MM-1") & "'"
    '        EksekusiSQL(strsql)
    '        ProgressBarControl1.Position = 5
    '        Application.DoEvents()

    '        strsql = "insert into MRekapJual(IDKategori,Periode) Select NoID,'" & Format(TglPeriode.DateTime, "yyyy-MM-1") & "' as Periode   From MKategori  where IDParent >0 order by NoID"
    '        EksekusiSQL(strsql)
    '        'IDKategori=-1 DiGunakan Sebagai Diskon 
    '        strsql = "insert into MRekapJual(IDKategori,Periode) Values(-1,'" & Format(TglPeriode.DateTime, "yyyy-MM-1") & "') "
    '        EksekusiSQL(strsql)

    '        ProgressBarControl1.Position = 10
    '        Application.DoEvents()

    '        For Tgl = 1 To TglSampai.Day
    '            StrTglField = StrTglField & IIf(Tgl = 1, "", "+") & "IsNull([" & Format(Tgl, "00") & "],0) " & "+IsNull([" & Format(Tgl, "00") & "1],0) "
    '            ProgressBarControl1.Position = 10 + 90 * Tgl / TglSampai.Day
    '            Application.DoEvents()
    '            strsql = "update MRekapJual Set [" & Format(Tgl, "00") & "]=Z.Jumlah "
    '            strsql = strsql & "from MRekapJual inner join "
    '            strsql = strsql & "(SELECT  [NoID], SUM(isnull([Jumlah],0)) as Jumlah "
    '            strsql = strsql & "FROM [vRekapPenjualanPerDepartemen]  "
    '            strsql = strsql & "where tanggal='" & Format(TglPeriode.DateTime, "yyyy-MM-") & Tgl.ToString & "' group by [vRekapPenjualanPerDepartemen].NoID  "
    '            strsql = strsql & ") Z "
    '            strsql = strsql & "on MRekapJual.IDKategori=Z.NoID "
    '            strsql = strsql & "where MRekapJual.Periode='" & Format(TglPeriode.DateTime, "yyyy-MM-1") & "'"
    '            EksekusiSQL(strsql)
    '            Application.DoEvents()

    '            'Menghitung Jumlah Discount
    '            strsql = "update MRekapJual Set [" & Format(Tgl, "00") & "1]=0,[" & Format(Tgl, "00") & "]= -1*( select  SUM(isnull(MJual.DiskonNotaRp,0)+isnull(MJual.Pembulatan,0) )  from MJual where year(tanggal)=" & TglPeriode.DateTime.Year & " and month(tanggal)= " & TglPeriode.DateTime.Month & " and day(tanggal)= " & Tgl & ") "
    '            strsql = strsql & "from MRekapJual   "
    '            strsql = strsql & "where IDKategori=-1  and MRekapJual.Periode='" & Format(TglPeriode.DateTime, "yyyy-MM-1") & "'"
    '            EksekusiSQL(strsql)
    '            Application.DoEvents()
    '            'Menghitung Jumlah Discount dibebankan ke IDKategori 20
    '            strsql = "update MRekapJual Set [" & Format(Tgl, "00") & "1]= -1*( select  SUM(isnull(MJual.DiskonNotaRp,0)+isnull(MJual.Pembulatan,0) )  from MJual where year(tanggal)=" & TglPeriode.DateTime.Year & " and month(tanggal)= " & TglPeriode.DateTime.Month & " and day(tanggal)= " & Tgl & ") "
    '            strsql = strsql & "from MRekapJual   "
    '            strsql = strsql & "where IDKategori=20  and MRekapJual.Periode='" & Format(TglPeriode.DateTime, "yyyy-MM-1") & "'"
    '            EksekusiSQL(strsql)
    '            Application.DoEvents()
    '            ProgressBarControl1.Position = 10 + 90 * Tgl / TglSampai.Day
    '        Next
    '        strsql = "UPDATE MRekapJual Set [Total]=0"
    '        For i As Integer = 1 To 31
    '            strsql &= " + IsNull([" & i.ToString("00") & "],0) "
    '        Next
    '        strsql = strsql & " WHERE MRekapJual.Periode='" & Format(TglPeriode.DateTime, "yyyy-MM-1") & "'"
    '        EksekusiSQL(strsql)

    '        strsql = "update MRekapJual Set Total= " & StrTglField
    '        strsql = strsql & " from MRekapJual   "
    '        strsql = strsql & "where IDKategori=-1  and MRekapJual.Periode='" & Format(TglPeriode.DateTime, "yyyy-MM-1") & "'"
    '        EksekusiSQL(strsql)
    '        Application.DoEvents()
    '        strsql = "update MRekapJual Set Total= " & StrTglField
    '        strsql = strsql & " from MRekapJual   "
    '        strsql = strsql & "where IDKategori=20  and MRekapJual.Periode='" & Format(TglPeriode.DateTime, "yyyy-MM-1") & "'"
    '        EksekusiSQL(strsql)
    '        Application.DoEvents()
    '        'strsql = "update MRekapJual Set Total=Z.Jumlah "
    '        'strsql = strsql & "from MRekapJual inner join "
    '        'strsql = strsql & "(SELECT  [NoID], SUM(isnull([Jumlah],0)) as Jumlah "
    '        'strsql = strsql & "FROM [vRekapPenjualanPerDepartemen]  "
    '        'strsql = strsql & "where year(tanggal)=" & TglPeriode.DateTime.Year & " and month(tanggal)= " & TglPeriode.DateTime.Month & " group by [vRekapPenjualanPerDepartemen].NoID  "
    '        'strsql = strsql & ") Z "
    '        'strsql = strsql & "on MRekapJual.IDKategori=Z.NoID "
    '        'strsql = strsql & "where MRekapJual.Periode='" & Format(TglPeriode.DateTime, "yyyy-MM-1") & "'"
    '        'EksekusiSQL(strsql)


    '        lbDaftar.Text = Me.Text & " [Bruto]"
    '        x.Caption = "Proses Selesai"
    '        ProgressBarControl1.Visible = False
    '    Catch ex As Exception
    '        XtraMessageBox.Show("Info Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
    '    Finally
    '        x.Close()
    '        x.Dispose()
    '    End Try
    '    'If MsgBox("Yakin mau proses hitung ulang rekap?, mungkin memerlukan waktu beberapa saat!", MsgBoxStyle.Question + MsgBoxStyle.YesNo) = MsgBoxResult.Yes Then

    '    'End If
    'End Sub
    'Sub HitungPerDeptBersih()
    '    Dim StrSQL As String = ""
    '    Dim x As DevExpress.Utils.WaitDialogForm = Nothing
    '    Try
    '        x = New DevExpress.Utils.WaitDialogForm("Sedang proses menghitung", NamaAplikasi)
    '        x.Show()

    '        ProgressBarControl1.Visible = True
    '        ProgressBarControl1.Position = 0
    '        Application.DoEvents()

    '        StrSQL = "DELETE FROM [MLapPembelianBulanan] WHERE IDUser=" & IDUserAktif & " AND IsNull(IDKategori,0)<>0 AND MONTH(Periode)=" & Month(TglPeriode.DateTime) & " AND YEAR(Periode)=" & Year(TglPeriode.DateTime)
    '        EksekusiSQL(StrSQL)

    '        ProgressBarControl1.Position = 5
    '        Application.DoEvents()

    '        StrSQL = "INSERT INTO MLapPembelianBulanan(IDKategori,Periode,IDUser) SELECT MKategori.NoID,'" & TglPeriode.DateTime.ToString("yyyy-MM-01") & "' AS Periode, " & IDUserAktif & " FROM MKategori WHERE MKategori.IDParent>0 ORDER BY MKategori.NoID"
    '        EksekusiSQL(StrSQL)

    '        ProgressBarControl1.Position = 5
    '        Application.DoEvents()

    '        For Tgl As Integer = 1 To 31
    '            ProgressBarControl1.Position = 100 * Tgl / 31
    '            Application.DoEvents()
    '            StrSQL = "UPDATE MLapPembelianBulanan SET " & vbCrLf & _
    '                     " [" & Tgl & "]=IsNull(Z.Jumlah,0) " & vbCrLf & _
    '                     " FROM (MLapPembelianBulanan LEFT JOIN MKategori ON MKategori.NoID=MLapPembelianBulanan.IDKategori)" & vbCrLf & _
    '                     " LEFT JOIN (SELECT MKategori.NoID, SUM(MBeliD.Jumlah) AS Jumlah" & vbCrLf & _
    '                     " FROM (MBeli" & vbCrLf & _
    '                     " INNER JOIN MBeliD ON MBeli.NoID=MBeliD.IDBeli)" & vbCrLf & _
    '                     " INNER JOIN MBarang ON MBarang.NoID=MBeliD.IDBarang " & vbCrLf & _
    '                     " INNER JOIN MKategori ON MKategori.NoID=MBarang.IDKategori " & _
    '                     " WHERE MBeli.IsPosted=1 AND DAY(MBeli.Tanggal)=" & Tgl & " AND MONTH(MBeli.Tanggal)=" & Month(TglPeriode.DateTime) & " AND YEAR(MBeli.Tanggal)=" & Year(TglPeriode.DateTime) & " " & _
    '                     " GROUP BY MKategori.NoID " & _
    '                     ") Z ON Z.NoID=MKategori.NoID " & vbCrLf & _
    '                     " WHERE MLapPembelianBulanan.IDUser=" & IDUserAktif & " AND IsNull(MLapPembelianBulanan.IDSupplier,0)=0 AND MONTH(MLapPembelianBulanan.Periode)=" & Month(TglPeriode.DateTime) & " AND YEAR(MLapPembelianBulanan.Periode)=" & Year(TglPeriode.DateTime) & " "
    '            EksekusiSQL(StrSQL)
    '            Application.DoEvents()

    '            StrSQL = "UPDATE MLapPembelianBulanan SET " & vbCrLf & _
    '                     " [P" & Tgl & "]=IsNull(Z.Jumlah,0) " & vbCrLf & _
    '                     " FROM (MLapPembelianBulanan LEFT JOIN MKategori ON MKategori.NoID=MLapPembelianBulanan.IDKategori)" & vbCrLf & _
    '                     " LEFT JOIN (SELECT MKategori.NoID, SUM(MPOD.HargaJualPcs*MPOD.QtyPcs) AS Jumlah" & vbCrLf & _
    '                     " FROM (MBeli" & vbCrLf & _
    '                     " INNER JOIN MBeliD ON MBeli.NoID=MBeliD.IDBeli)" & vbCrLf & _
    '                     " INNER JOIN (MPOD INNER JOIN MPO ON MPO.NoID=MPOD.IDPO) ON MPOD.NoID=MBeliD.IDPOD " & vbCrLf & _
    '                     " INNER JOIN MBarang ON MBarang.NoID=MBeliD.IDBarang " & vbCrLf & _
    '                     " INNER JOIN MKategori ON MKategori.NoID=MBarang.IDKategori " & _
    '                     " WHERE MBeli.IsPosted=1 AND DAY(MBeli.Tanggal)=" & Tgl & " AND MONTH(MBeli.Tanggal)=" & Month(TglPeriode.DateTime) & " AND YEAR(MBeli.Tanggal)=" & Year(TglPeriode.DateTime) & " " & _
    '                     " GROUP BY MKategori.NoID " & _
    '                     ") Z ON Z.NoID=MKategori.NoID " & vbCrLf & _
    '                     " WHERE MLapPembelianBulanan.IDUser=" & IDUserAktif & " AND IsNull(MLapPembelianBulanan.IDSupplier,0)=0 AND MONTH(MLapPembelianBulanan.Periode)=" & Month(TglPeriode.DateTime) & " AND YEAR(MLapPembelianBulanan.Periode)=" & Year(TglPeriode.DateTime) & " "
    '            EksekusiSQL(StrSQL)
    '            Application.DoEvents()

    '            ProgressBarControl1.Position = 100 * Tgl / 31
    '        Next

    '        StrSQL = "UPDATE MLapPembelianBulanan SET Total="
    '        For i As Integer = 1 To 31
    '            StrSQL &= IIf(i = 1, "", " + ") & "IsNull([" & i & "],0)"
    '        Next
    '        StrSQL &= " WHERE MLapPembelianBulanan.IDUser=" & IDUserAktif & " AND IsNull(MLapPembelianBulanan.IDSupplier,0)=0 AND MONTH(MLapPembelianBulanan.Periode)=" & Month(TglPeriode.DateTime) & " AND YEAR(MLapPembelianBulanan.Periode)=" & Year(TglPeriode.DateTime) & " "
    '        EksekusiSQL(StrSQL)
    '        Application.DoEvents()

    '        StrSQL = "UPDATE MLapPembelianBulanan SET TotalP="
    '        For i As Integer = 1 To 31
    '            StrSQL &= IIf(i = 1, "", " + ") & "IsNull([P" & i & "],0)"
    '        Next
    '        StrSQL &= " WHERE MLapPembelianBulanan.IDUser=" & IDUserAktif & " AND IsNull(MLapPembelianBulanan.IDSupplier,0)=0 AND MONTH(MLapPembelianBulanan.Periode)=" & Month(TglPeriode.DateTime) & " AND YEAR(MLapPembelianBulanan.Periode)=" & Year(TglPeriode.DateTime) & " "
    '        EksekusiSQL(StrSQL)
    '        Application.DoEvents()

    '        '100*(case when Sum(MJualD.Jumlah)<>0 AND (Sum(ISNULL(MJualD.Qty,0) * ISNULL(MJualD.HargaPokok,0)))<>0 then (Sum(MJualD.Jumlah)/(Sum(ISNULL(MJualD.Qty,0) * ISNULL(MJualD.HargaPokok,0))))-1 else 0.0 end)
    '        StrSQL = "UPDATE MLapPembelianBulanan SET ProfitMargin=" & vbCrLf & _
    '                 " (CASE WHEN IsNull(Total,0)=0 THEN 0 ELSE ((TotalP/Total)-1)*100 END)" & vbCrLf & _
    '                 " WHERE MLapPembelianBulanan.IDUser=" & IDUserAktif & " AND IsNull(MLapPembelianBulanan.IDSupplier,0)=0 AND MONTH(MLapPembelianBulanan.Periode)=" & Month(TglPeriode.DateTime) & " AND YEAR(MLapPembelianBulanan.Periode)=" & Year(TglPeriode.DateTime) & " "
    '        EksekusiSQL(StrSQL)
    '        Application.DoEvents()

    '        x.Caption = "Proses Selesai"
    '        ProgressBarControl1.Visible = False
    '    Catch ex As Exception
    '        XtraMessageBox.Show("Info Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
    '    Finally
    '        x.Close()
    '        x.Dispose()
    '    End Try
    'End Sub
    Private Sub SimpleButton2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SimpleButton2.Click
        Dim namafile As String
        namafile = Application.StartupPath & "\report\LaporanReturKeluar.rpt"
        If EditReport Then
            If txtSupplier.Text = "" Then
                ViewReport(Me.MdiParent, action_.Edit, namafile, "Laporan Retur Keluar", , , "TglDari=CDATE(" & TglDari.DateTime.ToString("yyyy,MM,dd") & ")&TglSampai=CDATE(" & TglSampai.DateTime.ToString("yyyy,MM,dd") & ")")
            Else
                ViewReport(Me.MdiParent, action_.Edit, namafile, "Laporan Retur Keluar", , , "TglDari=CDATE(" & TglDari.DateTime.ToString("yyyy,MM,dd") & ")&TglSampai=CDATE(" & TglSampai.DateTime.ToString("yyyy,MM,dd") & ")&IDSupplier=" & NullToLong(txtSupplier.EditValue))
            End If
        Else
            If txtSupplier.Text = "" Then
                ViewReport(Me.MdiParent, action_.Preview, namafile, "Laporan Retur Keluar", , , "TglDari=CDATE(" & TglDari.DateTime.ToString("yyyy,MM,dd") & ")&TglSampai=CDATE(" & TglSampai.DateTime.ToString("yyyy,MM,dd") & ")")
            Else
                ViewReport(Me.MdiParent, action_.Preview, namafile, "Laporan Retur Keluar", , , "TglDari=CDATE(" & TglDari.DateTime.ToString("yyyy,MM,dd") & ")&TglSampai=CDATE(" & TglSampai.DateTime.ToString("yyyy,MM,dd") & ")&IDSupplier=" & NullToLong(txtSupplier.EditValue))
            End If
        End If
    End Sub

    Private Sub BarButtonItem1_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem1.ItemClick
        SimpleButton2.PerformClick()
    End Sub

    Private Sub SimpleButton3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SimpleButton3.Click
        Dim namafile As String
        'lbDaftar.Text = Me.Text & " [Netto]"
        If EditReport Then
            namafile = Application.StartupPath & "\report\ReturPembelianPerDept1.rpt"
            CetakMRPTJual(IIf(EditReport, action_.Edit, action_.Preview), namafile, True)
            namafile = Application.StartupPath & "\report\ReturPembelianPerDept2.rpt"
            CetakMRPTJual(IIf(EditReport, action_.Edit, action_.Preview), namafile, True)
            namafile = Application.StartupPath & "\report\ReturPembelianPerDept3.rpt"
            CetakMRPTJual(IIf(EditReport, action_.Edit, action_.Preview), namafile, True)
            namafile = Application.StartupPath & "\report\ReturPembelianPerDept4.rpt"
            CetakMRPTJual(IIf(EditReport, action_.Edit, action_.Preview), namafile, True)
        Else
            HitungPerDeptBersih()
            Application.DoEvents()
            RefreshData()
            Application.DoEvents()
            namafile = Application.StartupPath & "\report\ReturPembelianPerDept1.rpt"
            CetakMRPTJual(IIf(EditReport, action_.Edit, action_.Preview), namafile, True)
            namafile = Application.StartupPath & "\report\ReturPembelianPerDept2.rpt"
            CetakMRPTJual(IIf(EditReport, action_.Edit, action_.Preview), namafile, True)
            namafile = Application.StartupPath & "\report\ReturPembelianPerDept3.rpt"
            CetakMRPTJual(IIf(EditReport, action_.Edit, action_.Preview), namafile, True)
            namafile = Application.StartupPath & "\report\ReturPembelianPerDept4.rpt"
            CetakMRPTJual(IIf(EditReport, action_.Edit, action_.Preview), namafile, True)
        End If
    End Sub
    Sub HitungPerDeptBersih()
        Dim StrSQL As String = ""
        Dim x As DevExpress.Utils.WaitDialogForm = Nothing
        Dim TglPeriode As Date
        Try
            x = New DevExpress.Utils.WaitDialogForm("Sedang proses menghitung", NamaAplikasi)
            x.Show()

            ProgressBarControl1.Visible = True
            ProgressBarControl1.Position = 0
            Application.DoEvents()
            TglPeriode = CDate(TglDari.DateTime.ToString("yyyy,MM,01"))
            StrSQL = "DELETE FROM [MLapReturPembelianBulanan] WHERE IDUser=" & IDUserAktif & " AND IsNull(IDKategori,0)<>0 AND MONTH(Periode)=" & Month(TglPeriode) & " AND YEAR(Periode)=" & Year(TglPeriode)
            EksekusiSQL(StrSQL)

            ProgressBarControl1.Position = 5
            Application.DoEvents()

            'If NullToLong(txtKategori.EditValue) = 1 Then
            '    StrSQL = "INSERT INTO MLapPembelianBulanan(IDKategori,Periode,IDUser) SELECT MKategori.NoID,'" & TglPeriode.DateTime.ToString("yyyy-MM-01") & "' AS Periode, " & IDUserAktif & " FROM MKategori WHERE MKategori.Kode IN ('25', '27', '31', '32') AND MKategori.IDParent>0 ORDER BY MKategori.NoID"
            'ElseIf NullToLong(txtKategori.EditValue) = 2 Then
            '    StrSQL = "INSERT INTO MLapPembelianBulanan(IDKategori,Periode,IDUser) SELECT MKategori.NoID,'" & TglPeriode.DateTime.ToString("yyyy-MM-01") & "' AS Periode, " & IDUserAktif & " FROM MKategori WHERE MKategori.Kode NOT IN ('25', '27', '31', '32') AND MKategori.IDParent>0 ORDER BY MKategori.NoID"
            'Else 'Semua
            StrSQL = "INSERT INTO MLapReturPembelianBulanan(IDKategori,Periode,IDUser) SELECT MKategori.NoID,'" & TglPeriode.ToString("yyyy-MM-01") & "' AS Periode, " & IDUserAktif & " FROM MKategori WHERE MKategori.IDParent>0 ORDER BY MKategori.NoID"
            'End If
            EksekusiSQL(StrSQL)

            'StrSQL = "INSERT INTO MLapReturPembelianBulanan(IDKategori,Periode,IDUser) SELECT -1,'" & TglPeriode.ToString("yyyy-MM-01") & "' AS Periode, " & IDUserAktif & " "
            'EksekusiSQL(StrSQL)

            ProgressBarControl1.Position = 5
            Application.DoEvents()

            For Tgl As Integer = 1 To 31
                ProgressBarControl1.Position = 100 * Tgl / 31
                Application.DoEvents()
                StrSQL = "UPDATE MLapReturPembelianBulanan SET " & vbCrLf & _
                         " [" & Tgl & "]=IsNull(Z.Jumlah,0) " & vbCrLf & _
                         " FROM (MLapReturPembelianBulanan LEFT JOIN MKategori ON MKategori.NoID=MLapReturPembelianBulanan.IDKategori)" & vbCrLf & _
                         " LEFT JOIN (SELECT ReturBeli.NoID, SUM(ReturBeli.Jumlah) AS Jumlah FROM (SELECT MKategori.NoID, MReturBeliD.Jumlah AS Jumlah" & vbCrLf & _
                         " FROM (MReturBeli" & vbCrLf & _
                         " INNER JOIN MReturBeliD ON MReturBeli.NoID=MReturBeliD.IDReturBeli)" & vbCrLf & _
                         " INNER JOIN MBarang ON MBarang.NoID=MReturBeliD.IDBarang " & vbCrLf & _
                         " INNER JOIN MKategori ON MKategori.NoID=MBarang.IDKategori " & _
                         " WHERE IsNull(MReturBeli.IsTanpaBarang,0)=0 AND DAY(MReturBeli.Tanggal)=" & Tgl & " AND MONTH(MReturBeli.Tanggal)=" & Month(TglPeriode) & " AND YEAR(MReturBeli.Tanggal)=" & Year(TglPeriode) & "" & _
                         " UNION ALL " & _
                         " SELECT MReturBeli.IDKategori, MReturBeli.Total AS Jumlah" & vbCrLf & _
                         " FROM MReturBeli" & vbCrLf & _
                         " LEFT JOIN MKategori ON MKategori.NoID=MReturBeli.IDKategori " & _
                         " WHERE IsNull(MReturBeli.IsTanpaBarang,0)=1 AND DAY(MReturBeli.Tanggal)=" & Tgl & " AND MONTH(MReturBeli.Tanggal)=" & Month(TglPeriode) & " AND YEAR(MReturBeli.Tanggal)=" & Year(TglPeriode) & "" & _
                         " ) ReturBeli " & _
                         " GROUP BY ReturBeli.NoID " & _
                         ") Z ON Z.NoID=MKategori.NoID " & vbCrLf & _
                         " WHERE MLapReturPembelianBulanan.IDUser=" & IDUserAktif & " AND IsNull(MLapReturPembelianBulanan.IDSupplier,0)=0 AND MONTH(MLapReturPembelianBulanan.Periode)=" & Month(TglPeriode) & " AND YEAR(MLapReturPembelianBulanan.Periode)=" & Year(TglPeriode) & " "
                EksekusiSQL(StrSQL)
                Application.DoEvents()

                'StrSQL = "UPDATE MLapReturPembelianBulanan SET " & vbCrLf & _
                '         " [" & Tgl & "]=IsNull((SELECT SUM(-1*MReturBeli.DiskonNotaTotal) FROM MReturBeli WHERE DAY(MReturBeli.Tanggal)=" & Tgl & " AND MONTH(MReturBeli.Tanggal)=" & Month(TglPeriode) & " AND YEAR(MReturBeli.Tanggal)=" & Year(TglPeriode) & "),0) " & vbCrLf & _
                '         " FROM MLapReturPembelianBulanan " & vbCrLf & _
                '         " WHERE MLapReturPembelianBulanan.IDKategori=-1 AND MLapReturPembelianBulanan.IDUser=" & IDUserAktif & " AND IsNull(MLapReturPembelianBulanan.IDSupplier,0)=0 AND MONTH(MLapReturPembelianBulanan.Periode)=" & Month(TglPeriode) & " AND YEAR(MLapReturPembelianBulanan.Periode)=" & Year(TglPeriode) & " "
                'EksekusiSQL(StrSQL)
                'Application.DoEvents()

                'StrSQL = "UPDATE MLapReturPembelianBulanan SET " & vbCrLf & _
                '         " [P" & Tgl & "]=IsNull(Z.Jumlah,0) " & vbCrLf & _
                '         " FROM (MLapReturPembelianBulanan LEFT JOIN MKategori ON MKategori.NoID=MLapReturPembelianBulanan.IDKategori)" & vbCrLf & _
                '         " LEFT JOIN (SELECT ReturBeli.NoID, SUM(ReturBeli.Jumlah) AS Jumlah FROM (SELECT MKategori.NoID, MPOD.HargaJualPcs*MPOD.QtyPcs AS Jumlah" & vbCrLf & _
                '         " FROM (MBeli" & vbCrLf & _
                '         " INNER JOIN MBeliD ON MBeli.NoID=MBeliD.IDBeli)" & vbCrLf & _
                '         " INNER JOIN (MPOD INNER JOIN MPO ON MPO.NoID=MPOD.IDPO) ON MPOD.NoID=MBeliD.IDPOD " & vbCrLf & _
                '         " INNER JOIN MBarang ON MBarang.NoID=MBeliD.IDBarang " & vbCrLf & _
                '         " INNER JOIN MKategori ON MKategori.NoID=MBarang.IDKategori " & _
                '         " WHERE IsNull(MBeli.IsTanpaBarang,0)=0 AND DAY(MBeli.Tanggal)=" & Tgl & " AND MONTH(MBeli.Tanggal)=" & Month(TglPeriode.DateTime) & " AND YEAR(MBeli.Tanggal)=" & Year(TglPeriode.DateTime) & " " & _
                '         " UNION ALL " & vbCrLf & _
                '         " SELECT MBeli.IDKategori, MBeli.TotalJual AS Jumlah " & vbCrLf & _
                '         " FROM MBeli" & vbCrLf & _
                '         " LEFT JOIN MKategori ON MKategori.NoID=MBeli.IDKategori " & _
                '         " WHERE IsNull(MBeli.IsTanpaBarang,0)=1 AND DAY(MBeli.Tanggal)=" & Tgl & " AND MONTH(MBeli.Tanggal)=" & Month(TglPeriode.DateTime) & " AND YEAR(MBeli.Tanggal)=" & Year(TglPeriode.DateTime) & ") Beli " & _
                '         " GROUP BY Beli.NoID " & _
                '         ") Z ON Z.NoID=MKategori.NoID " & vbCrLf & _
                '         " WHERE MLapPembelianBulanan.IDUser=" & IDUserAktif & " AND IsNull(MLapPembelianBulanan.IDSupplier,0)=0 AND MONTH(MLapPembelianBulanan.Periode)=" & Month(TglPeriode.DateTime) & " AND YEAR(MLapPembelianBulanan.Periode)=" & Year(TglPeriode.DateTime) & " "
                'EksekusiSQL(StrSQL)
                'Application.DoEvents()

                ProgressBarControl1.Position = 100 * Tgl / 31
            Next

            StrSQL = "UPDATE MLapReturPembelianBulanan SET Total="
            For i As Integer = 1 To 31
                StrSQL &= IIf(i = 1, "", " + ") & "IsNull([" & i & "],0)"
            Next
            StrSQL &= " WHERE MLapReturPembelianBulanan.IDUser=" & IDUserAktif & " AND IsNull(MLapReturPembelianBulanan.IDSupplier,0)=0 AND MONTH(MLapReturPembelianBulanan.Periode)=" & Month(TglPeriode) & " AND YEAR(MLapReturPembelianBulanan.Periode)=" & Year(TglPeriode) & " "
            EksekusiSQL(StrSQL)
            Application.DoEvents()

            'StrSQL = "UPDATE MLapPembelianBulanan SET TotalP="
            'For i As Integer = 1 To 31
            '    StrSQL &= IIf(i = 1, "", " + ") & "IsNull([P" & i & "],0)"
            'Next
            'StrSQL &= " WHERE MLapPembelianBulanan.IDUser=" & IDUserAktif & " AND IsNull(MLapPembelianBulanan.IDSupplier,0)=0 AND MONTH(MLapPembelianBulanan.Periode)=" & Month(TglPeriode.DateTime) & " AND YEAR(MLapPembelianBulanan.Periode)=" & Year(TglPeriode.DateTime) & " "
            'EksekusiSQL(StrSQL)
            'Application.DoEvents()

            ''100*(case when Sum(MJualD.Jumlah)<>0 AND (Sum(ISNULL(MJualD.Qty,0) * ISNULL(MJualD.HargaPokok,0)))<>0 then (Sum(MJualD.Jumlah)/(Sum(ISNULL(MJualD.Qty,0) * ISNULL(MJualD.HargaPokok,0))))-1 else 0.0 end)
            'StrSQL = "UPDATE MLapPembelianBulanan SET ProfitMargin=" & vbCrLf & _
            '         " (CASE WHEN IsNull(Total,0)=0 THEN 0 ELSE ((TotalP/Total)-1)*100 END)" & vbCrLf & _
            '         " WHERE MLapPembelianBulanan.IDUser=" & IDUserAktif & " AND IsNull(MLapPembelianBulanan.IDSupplier,0)=0 AND MONTH(MLapPembelianBulanan.Periode)=" & Month(TglPeriode.DateTime) & " AND YEAR(MLapPembelianBulanan.Periode)=" & Year(TglPeriode.DateTime) & " "
            'EksekusiSQL(StrSQL)
            'Application.DoEvents()

            x.Caption = "Proses Selesai"
            ProgressBarControl1.Visible = False
        Catch ex As Exception
            XtraMessageBox.Show("Info Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
        Finally
            x.Close()
            x.Dispose()
        End Try
    End Sub
    Private Sub LabelControl5_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LabelControl5.Click
        TglDari.Enabled = Not TglDari.Enabled
        TglSampai.Enabled = TglDari.Enabled
    End Sub
End Class