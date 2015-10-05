Imports System.Data.SqlClient 
Imports DevExpress.XtraEditors 
Imports DevExpress.XtraGrid.Views.Base 
Imports DevExpress.XtraEditors.Repository
Imports VPoint.mdlCetakCR 
Imports DevExpress.XtraGrid.Views.Grid.ViewInfo
Imports DevExpress.XtraGrid.Views.Grid 
Imports System.Data.OleDb

Public Class frmLaporanRekapPenjualanPerDepartemen
    Public FormName As String = "RekapJualPerDepartemen"
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
            TglPeriode.DateTime = CDate(TanggalSystem.ToString("yyyy/MM/01"))
            TglDari.DateTime = CDate(TanggalSystem.ToString("yyyy/MM/01"))
            TglSampai.DateTime = CDate(TanggalSystem.ToString("yyyy/MM/dd"))

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
        Dim Temp As Double = 0.0
        Dim i As Integer = 0
        Try
            Dim curentcursor As Cursor = Windows.Forms.Cursor.Current
            Windows.Forms.Cursor.Current = Cursors.WaitCursor
            dlg = New DevExpress.Utils.WaitDialogForm(String.Format("Sedang merefresh data.{0}MOHON TUNGGU ...", vbCrLf), Application.ProductName)
            dlg.TopMost = False
            dlg.Show()
            Windows.Forms.Cursor.Current = Cursors.WaitCursor

            strsql = "SELECT MKategori.Kode, MKategori.Nama Departemen, MParent.Nama AS Grup, MRekapJual.* FROM " & _
                     " MRekapJual Left Join MKategori on MRekapJual.IDKategori=MKategori.NoID " & _
                     " left join MKategori MParent on Mkategori.IDParent=MParent.NoID where Periode='" & Format(TglPeriode.DateTime, "yyyy-MM-1") & "'"
            ocmd2.Connection = cn
            ocmd2.CommandType = CommandType.Text
            ocmd2.CommandText = strsql

            cn.Open()
            oda2 = New SqlDataAdapter(ocmd2)
            oda2.TableMappings.Add("Tabel", "RekapJualPerDepartemenBulanan")
            If ds.Tables("RekapJualPerDepartemenBulanan") Is Nothing Then
            Else
                ds.Tables("RekapJualPerDepartemenBulanan").Clear()
            End If
            oda2.Fill(ds, "RekapJualPerDepartemenBulanan")
            BS.DataSource = ds.Tables("RekapJualPerDepartemenBulanan")
            GC1.DataSource = BS.DataSource

            strsql = "SELECT CONVERT(Varchar(2),CONVERT(DATE,MJual.Tanggal),103) AS Tanggal, SUM(MJual.Total) AS Total" & vbCrLf & _
                     " FROM MJual " & vbCrLf & _
                     " WHERE MJual.IsPOS=1 AND MJual.Tanggal>='" & Format(TglPeriode.DateTime, "yyyy-MM-1") & "' AND MJual.Tanggal<'" & CDate(Format(TglPeriode.DateTime, "yyyy,MM,01")).AddMonths(1).ToString("yyyy-MM-01") & "' " & vbCrLf & _
                     " GROUP BY CONVERT(DATE,MJual.Tanggal) " & vbCrLf & _
                     " ORDER BY CONVERT(DATE,MJual.Tanggal)"
            'If ds.Tables("MRekapBulanan") Is Nothing Then
            'Else
            '    ds.Tables("MRekapBulanan").Clear()
            'End If
            'oda2.Fill(ds, "MRekapBulanan")
            ds = ExecuteDataset("MRekapBulanan", strsql)
            If Not ds Is Nothing Then
                ChartControl1.DataSource = ds.Tables("MRekapBulanan")
                ChartControl1.Series.Item(0).ArgumentDataMember = "Tanggal"
            Else
                ChartControl1.DataSource = Nothing
            End If
            ChartControl1.RefreshData()
            Temp = 0.0
            For i = 0 To ds.Tables("MRekapBulanan").Rows.Count - 1
                Temp = Temp + NullToDbl(ds.Tables("MRekapBulanan").Rows(i).Item("Total"))
            Next
            txtHargaJual.EditValue = Temp / (i + 1)
            GV1.OptionsDetail.SmartDetailExpandButtonMode = DetailExpandButtonMode.AlwaysEnabled
            For Each ctrl As GridView In GC1.Views
                With ctrl
                    For i = 0 To .Columns.Count - 1
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
            End If
            BukaFile(dlgsave.FileName)
        End If
        dlgsave.Dispose()
    End Sub
    Sub PrintPreview()
        Dim namafile As String
        namafile = Application.StartupPath & "\report\Omset1.rpt"
        CetakMRPTJual(IIf(EditReport, action_.Edit, action_.Preview), namafile)
        namafile = Application.StartupPath & "\report\Omset2.rpt"
        CetakMRPTJual(IIf(EditReport, action_.Edit, action_.Preview), namafile)
        namafile = Application.StartupPath & "\report\Omset3.rpt"
        CetakMRPTJual(IIf(EditReport, action_.Edit, action_.Preview), namafile)
        namafile = Application.StartupPath & "\report\Omset4.rpt"
        CetakMRPTJual(IIf(EditReport, action_.Edit, action_.Preview), namafile)

    End Sub
    Sub PrintPreviewDisc()
        Dim namafile As String
        namafile = Application.StartupPath & "\report\OmsetDisc1.rpt"
        CetakMRPTJual(IIf(EditReport, action_.Edit, action_.Preview), namafile)
        namafile = Application.StartupPath & "\report\OmsetDisc2.rpt"
        CetakMRPTJual(IIf(EditReport, action_.Edit, action_.Preview), namafile)
        namafile = Application.StartupPath & "\report\OmsetDisc3.rpt"
        CetakMRPTJual(IIf(EditReport, action_.Edit, action_.Preview), namafile)
        namafile = Application.StartupPath & "\report\OmsetDisc4.rpt"
        CetakMRPTJual(IIf(EditReport, action_.Edit, action_.Preview), namafile)

    End Sub
    Private Sub CetakMRPTJual(ByVal Action As action_, ByVal NamaFile As String)
        Dim strsql As String = ""
        Dim view As ColumnView = GC1.FocusedView
        Try

            If System.IO.File.Exists(namafile) Then
                If EditReport Then
                    Action = action_.Edit
                Else
                    Action = action_.Preview
                End If

                ViewReport(Me.ParentForm, Action, NamaFile, Me.Text, , , "Periode=cdate(" & Format(TglPeriode.DateTime, "yyyy,MM") & ",1)")
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
        lbDaftar.Text = Me.Text & " [Bruto]"
        If EditReport Then
            PrintPreview()
        Else
            HitungDisckondibebankanDept20()
            Application.DoEvents()
            RefreshData()
            Application.DoEvents()
            PrintPreview()
        End If
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

    Private Sub RadioButton1_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)

    End Sub

    Private Sub RadioButton1_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        RefreshData()
    End Sub

    Private Sub RadioButton2_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        RefreshData()
    End Sub

    Private Sub RadioButton3_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        RefreshData()
    End Sub

    Private Sub RadioButton4_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        RefreshData()
    End Sub

    Private Sub RadioButton5_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        RefreshData()
    End Sub

    Private Sub RadioButton6_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        RefreshData()
    End Sub

    Private Sub SimpleButton1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SimpleButton1.Click
        HitungPerDeptBersih()
    End Sub
    Sub HitungDisckondibebankanDept20()
        Dim Tgl As Integer
        Dim TglSampai As Date
        Dim strsql As String = ""
        Dim StrTglField As String = ""
        TglSampai = DateAdd(DateInterval.Day, (1 - TglPeriode.DateTime.Day), TglPeriode.DateTime)
        TglSampai = DateAdd(DateInterval.Month, 1, TglSampai)
        TglSampai = DateAdd(DateInterval.Day, -1, TglSampai)
        Dim x As DevExpress.Utils.WaitDialogForm = Nothing
        Try
            x = New DevExpress.Utils.WaitDialogForm("Sedang proses menghitung", NamaAplikasi)
            x.Show()
            ProgressBarControl1.Visible = True
            ProgressBarControl1.Position = 0
            Application.DoEvents()
            strsql = "delete from MRekapJual where Periode='" & Format(TglPeriode.DateTime, "yyyy-MM-1") & "'"
            EksekusiSQL(strsql)
            ProgressBarControl1.Position = 5
            Application.DoEvents()

            'strsql = "insert into MRekapJual(IDKategori,Periode) Select NoID,'" & Format(TglPeriode.DateTime, "yyyy-MM-1") & "' as Periode   From MKategori  where IDParent >0 order by NoID"
            'EksekusiSQL(strsql)
            strsql = "insert into MRekapJual(IDKategori,Periode) Select NoID,'" & Format(TglPeriode.DateTime, "yyyy-MM-1") & "' as Periode   From MKategori where NoID<>50 AND NoID<>34 AND IDParent >0 order by NoID"
            EksekusiSQL(strsql)
            'IDKategori=-1 DiGunakan Sebagai Diskon 
            strsql = "insert into MRekapJual(IDKategori,Periode) Values(-1,'" & Format(TglPeriode.DateTime, "yyyy-MM-1") & "') "
            EksekusiSQL(strsql)

            ProgressBarControl1.Position = 10
            Application.DoEvents()

            For Tgl = 1 To TglSampai.Day
                StrTglField = StrTglField & IIf(Tgl = 1, "", "+") & "IsNull([" & Format(Tgl, "00") & "],0) " & "+IsNull([" & Format(Tgl, "00") & "1],0) "
                ProgressBarControl1.Position = 10 + 90 * Tgl / TglSampai.Day
                Application.DoEvents()
                strsql = "update MRekapJual Set [" & Format(Tgl, "00") & "]=Z.Jumlah "
                strsql = strsql & "from MRekapJual inner join "
                strsql = strsql & "(SELECT  [NoID], SUM(isnull([Jumlah],0)) as Jumlah "
                strsql = strsql & "FROM [vRekapPenjualanPerDepartemen]  "
                strsql = strsql & "where tanggal='" & Format(TglPeriode.DateTime, "yyyy-MM-") & Tgl.ToString & "' group by [vRekapPenjualanPerDepartemen].NoID  "
                strsql = strsql & ") Z "
                strsql = strsql & "on MRekapJual.IDKategori=Z.NoID "
                strsql = strsql & "where MRekapJual.Periode='" & Format(TglPeriode.DateTime, "yyyy-MM-1") & "'"
                EksekusiSQL(strsql)
                Application.DoEvents()

                'Menghitung Jumlah Discount
                strsql = "update MRekapJual Set [" & Format(Tgl, "00") & "1]=0,[" & Format(Tgl, "00") & "]= -1*( select  SUM(isnull(MJual.DiskonNotaRp,0)+isnull(MJual.Pembulatan,0) )  from MJual where year(tanggal)=" & TglPeriode.DateTime.Year & " and month(tanggal)= " & TglPeriode.DateTime.Month & " and day(tanggal)= " & Tgl & ") "
                strsql = strsql & "from MRekapJual   "
                strsql = strsql & "where IDKategori=-1 and MRekapJual.Periode='" & Format(TglPeriode.DateTime, "yyyy-MM-1") & "'"
                EksekusiSQL(strsql)
                Application.DoEvents()

                'Menghitung Jumlah Discount dibebankan ke IDKategori 20
                strsql = "update MRekapJual Set [" & Format(Tgl, "00") & "1]= -1*( select  SUM(isnull(MJual.DiskonNotaRp,0)+isnull(MJual.Pembulatan,0) )  from MJual where year(tanggal)=" & TglPeriode.DateTime.Year & " and month(tanggal)= " & TglPeriode.DateTime.Month & " and day(tanggal)= " & Tgl & ") "
                strsql = strsql & "from MRekapJual   "
                strsql = strsql & "where IDKategori=20  and MRekapJual.Periode='" & Format(TglPeriode.DateTime, "yyyy-MM-1") & "'"
                EksekusiSQL(strsql)
                Application.DoEvents()
                ProgressBarControl1.Position = 10 + 90 * Tgl / TglSampai.Day
            Next
            strsql = "UPDATE MRekapJual Set [Total]=0"
            For i As Integer = 1 To 31
                strsql &= " + IsNull([" & i.ToString("00") & "],0) "
            Next
            strsql = strsql & " WHERE MRekapJual.Periode='" & Format(TglPeriode.DateTime, "yyyy-MM-1") & "'"
            EksekusiSQL(strsql)

            strsql = "update MRekapJual Set Total= " & StrTglField
            strsql = strsql & " from MRekapJual   "
            strsql = strsql & "where IDKategori=-1  and MRekapJual.Periode='" & Format(TglPeriode.DateTime, "yyyy-MM-1") & "'"
            EksekusiSQL(strsql)
            Application.DoEvents()
            strsql = "update MRekapJual Set Total= " & StrTglField
            strsql = strsql & " from MRekapJual   "
            strsql = strsql & "where IDKategori=20  and MRekapJual.Periode='" & Format(TglPeriode.DateTime, "yyyy-MM-1") & "'"
            EksekusiSQL(strsql)
            Application.DoEvents()
            'strsql = "update MRekapJual Set Total=Z.Jumlah "
            'strsql = strsql & "from MRekapJual inner join "
            'strsql = strsql & "(SELECT  [NoID], SUM(isnull([Jumlah],0)) as Jumlah "
            'strsql = strsql & "FROM [vRekapPenjualanPerDepartemen]  "
            'strsql = strsql & "where year(tanggal)=" & TglPeriode.DateTime.Year & " and month(tanggal)= " & TglPeriode.DateTime.Month & " group by [vRekapPenjualanPerDepartemen].NoID  "
            'strsql = strsql & ") Z "
            'strsql = strsql & "on MRekapJual.IDKategori=Z.NoID "
            'strsql = strsql & "where MRekapJual.Periode='" & Format(TglPeriode.DateTime, "yyyy-MM-1") & "'"
            'EksekusiSQL(strsql)


            lbDaftar.Text = Me.Text & " [Bruto]"
            x.Caption = "Proses Selesai"
            ProgressBarControl1.Visible = False
        Catch ex As Exception
            XtraMessageBox.Show("Info Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
        Finally
            x.Close()
            x.Dispose()
        End Try
        'If MsgBox("Yakin mau proses hitung ulang rekap?, mungkin memerlukan waktu beberapa saat!", MsgBoxStyle.Question + MsgBoxStyle.YesNo) = MsgBoxResult.Yes Then

        'End If
    End Sub
    Sub HitungPerDeptBersih()
        Dim Tgl As Integer
        Dim strsql As String = ""
        Dim StrTglField As String = ""
        Dim TglSampai As Date
        TglSampai = DateAdd(DateInterval.Day, (1 - TglPeriode.DateTime.Day), TglPeriode.DateTime)
        TglSampai = DateAdd(DateInterval.Month, 1, TglSampai)
        TglSampai = DateAdd(DateInterval.Day, -1, TglSampai)
        Dim x As DevExpress.Utils.WaitDialogForm = Nothing
        Try
            x = New DevExpress.Utils.WaitDialogForm("Sedang proses menghitung", NamaAplikasi)
            x.Show()

            ProgressBarControl1.Visible = True
            ProgressBarControl1.Position = 0
            Application.DoEvents()
            strsql = "delete from MRekapJual where Periode='" & Format(TglPeriode.DateTime, "yyyy-MM-1") & "'"
            EksekusiSQL(strsql)
            ProgressBarControl1.Position = 5
            Application.DoEvents()

            'strsql = "insert into MRekapJual(IDKategori,Periode) Select NoID,'" & Format(TglPeriode.DateTime, "yyyy-MM-1") & "' as Periode   From MKategori  where IDParent >0 order by NoID"
            'EksekusiSQL(strsql)
            strsql = "insert into MRekapJual(IDKategori,Periode) Select NoID,'" & Format(TglPeriode.DateTime, "yyyy-MM-1") & "' as Periode   From MKategori where NoID<>50 AND NoID<>34 AND IDParent>0 order by NoID"
            EksekusiSQL(strsql)
            'IDKategori=-1 DiGunakan Sebagai Diskon 
            strsql = "insert into MRekapJual(IDKategori,Periode) Values(-1,'" & Format(TglPeriode.DateTime, "yyyy-MM-1") & "') "
            EksekusiSQL(strsql)

            ProgressBarControl1.Position = 10
            Application.DoEvents()

            For Tgl = 1 To TglSampai.Day
                StrTglField = StrTglField & IIf(Tgl = 1, "", "+") & "IsNull([" & Format(Tgl, "00") & "],0) " & "+IsNull([" & Format(Tgl, "00") & "1],0) "
                ProgressBarControl1.Position = 10 + 90 * Tgl / TglSampai.Day
                Application.DoEvents()
                strsql = "update MRekapJual Set [" & Format(Tgl, "00") & "]=Z.Jumlah "
                strsql = strsql & "from MRekapJual inner join "
                strsql = strsql & "(SELECT  [NoID], SUM(isnull([Jumlah],0)) as Jumlah "
                strsql = strsql & "FROM [vRekapPenjualanPerDepartemenBersih]  "
                strsql = strsql & "where tanggal='" & Format(TglPeriode.DateTime, "yyyy-MM-") & Tgl.ToString & "' group by [vRekapPenjualanPerDepartemenBersih].NoID  "
                strsql = strsql & ") Z "
                strsql = strsql & "on MRekapJual.IDKategori=Z.NoID "
                strsql = strsql & "where MRekapJual.Periode='" & Format(TglPeriode.DateTime, "yyyy-MM-1") & "'"
                EksekusiSQL(strsql)
                Application.DoEvents()

                ''Menghitung Jumlah Discount
                'strsql = "update MRekapJual Set [" & Format(Tgl, "00") & "1]=0,[" & Format(Tgl, "00") & "]= -1*( select  SUM(isnull(MJual.DiskonNotaRp,0)+isnull(MJual.Pembulatan,0) )  from MJual where year(tanggal)=" & TglPeriode.DateTime.Year & " and month(tanggal)= " & TglPeriode.DateTime.Month & " and day(tanggal)= " & Tgl & ") "
                'strsql = strsql & "from MRekapJual   "
                'strsql = strsql & "where IDKategori=-1  and MRekapJual.Periode='" & Format(TglPeriode.DateTime, "yyyy-MM-1") & "'"
                'EksekusiSQL(strsql)
                'Application.DoEvents()
                ''Menghitung Jumlah Discount dibebankan ke IDKategori 20
                'strsql = "update MRekapJual Set [" & Format(Tgl, "00") & "1]= -1*( select  SUM(isnull(MJual.DiskonNotaRp,0)+isnull(MJual.Pembulatan,0) )  from MJual where year(tanggal)=" & TglPeriode.DateTime.Year & " and month(tanggal)= " & TglPeriode.DateTime.Month & " and day(tanggal)= " & Tgl & ") "
                'strsql = strsql & "from MRekapJual   "
                'strsql = strsql & "where IDKategori=20  and MRekapJual.Periode='" & Format(TglPeriode.DateTime, "yyyy-MM-1") & "'"
                'EksekusiSQL(strsql)
                Application.DoEvents()
                ProgressBarControl1.Position = 10 + 90 * Tgl / TglSampai.Day
            Next
            strsql = "update MRekapJual Set Total= " & StrTglField
            strsql = strsql & " from MRekapJual   "
            strsql = strsql & "where IDKategori=-1  and MRekapJual.Periode='" & Format(TglPeriode.DateTime, "yyyy-MM-1") & "'"
            EksekusiSQL(strsql)
            Application.DoEvents()
            strsql = "update MRekapJual Set Total= " & StrTglField
            strsql = strsql & " from MRekapJual   "
            strsql = strsql & "where IDKategori=20  and MRekapJual.Periode='" & Format(TglPeriode.DateTime, "yyyy-MM-1") & "'"
            EksekusiSQL(strsql)
            Application.DoEvents()

            'strsql = "UPDATE MRekapJual Set [Total]=Z.Jumlah "
            'strsql = strsql & "from MRekapJual inner join "
            'strsql = strsql & "(SELECT  [NoID], SUM(isnull([Jumlah],0)) as Jumlah "
            'strsql = strsql & "FROM [vRekapPenjualanPerDepartemen]  "
            'strsql = strsql & "where year(tanggal)=" & TglPeriode.DateTime.Year & " and month(tanggal)= " & TglPeriode.DateTime.Month & " group by [vRekapPenjualanPerDepartemen].NoID  "
            'strsql = strsql & ") Z "
            'strsql = strsql & "on MRekapJual.IDKategori=Z.NoID "
            'strsql = strsql & "where MRekapJual.Periode='" & Format(TglPeriode.DateTime, "yyyy-MM-1") & "'"
            'EksekusiSQL(strsql)

            strsql = "UPDATE MRekapJual Set [Total]=0"
            For i As Integer = 1 To 31
                strsql &= " + IsNull([" & i.ToString("00") & "],0) "
            Next
            strsql = strsql & " WHERE MRekapJual.Periode='" & Format(TglPeriode.DateTime, "yyyy-MM-1") & "'"
            EksekusiSQL(strsql)

            lbDaftar.Text = Me.Text & " [Netto]"
            x.Caption = "Proses Selesai"
            ProgressBarControl1.Visible = False
        Catch ex As Exception
            XtraMessageBox.Show("Info Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
        Finally
            x.Close()
            x.Dispose()
        End Try
    End Sub
    Private Sub SimpleButton2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SimpleButton2.Click
        lbDaftar.Text = Me.Text & " [Bruto Dengan Diskon]"
        If EditReport Then
            PrintPreviewDisc()
        Else
            HitungDisckondibebankanDept20()
            Application.DoEvents()
            RefreshData()
            Application.DoEvents()
            PrintPreviewDisc()
        End If

    End Sub

    Private Sub BarButtonItem1_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem1.ItemClick
        SimpleButton2.PerformClick()
    End Sub

    Private Sub SimpleButton3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SimpleButton3.Click
        Dim namafile As String
        lbDaftar.Text = Me.Text & " [Netto]"
        If EditReport Then
            namafile = Application.StartupPath & "\report\OmsetNetto1.rpt"
            CetakMRPTJual(IIf(EditReport, action_.Edit, action_.Preview), namafile)
            namafile = Application.StartupPath & "\report\OmsetNetto2.rpt"
            CetakMRPTJual(IIf(EditReport, action_.Edit, action_.Preview), namafile)
            namafile = Application.StartupPath & "\report\OmsetNetto3.rpt"
            CetakMRPTJual(IIf(EditReport, action_.Edit, action_.Preview), namafile)
            namafile = Application.StartupPath & "\report\OmsetNetto4.rpt"
            CetakMRPTJual(IIf(EditReport, action_.Edit, action_.Preview), namafile)
        Else
            HitungPerDeptBersih()
            Application.DoEvents()
            RefreshData()
            Application.DoEvents()
            namafile = Application.StartupPath & "\report\OmsetNetto1.rpt"
            CetakMRPTJual(IIf(EditReport, action_.Edit, action_.Preview), namafile)
            namafile = Application.StartupPath & "\report\OmsetNetto2.rpt"
            CetakMRPTJual(IIf(EditReport, action_.Edit, action_.Preview), namafile)
            namafile = Application.StartupPath & "\report\OmsetNetto3.rpt"
            CetakMRPTJual(IIf(EditReport, action_.Edit, action_.Preview), namafile)
            namafile = Application.StartupPath & "\report\OmsetNetto4.rpt"
            CetakMRPTJual(IIf(EditReport, action_.Edit, action_.Preview), namafile)
        End If
    End Sub
End Class