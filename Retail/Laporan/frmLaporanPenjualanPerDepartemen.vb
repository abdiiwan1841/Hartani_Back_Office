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
Imports System.Data.OleDb

Public Class frmLaporanPenjualanPerDepartemen
    Public FormName As String = ""
    Public FormEntriName As String = ""
    Public TableName As String = ""
    Public TableNameD As String = ""
    Public TableMaster As String = "MRPTJual"
    Public NoID As Long = -1
    Public ShowNoID As Boolean = False
    Public DirectNoID As Long = -1


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
            FungsiControl.SetForm(Me)
            RefreshLookUp()
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
            SQL = "SELECT NoID, Kode, Nama FROM MPos WHERE IsActive=1"
            ds = ExecuteDataset("MPos", SQL)
            txtSupplier.Properties.DataSource = ds.Tables("MPos")
            txtSupplier.Properties.DisplayMember = "Nama"
            txtSupplier.Properties.ValueMember = "NoID"

        Catch ex As Exception
            XtraMessageBox.Show("Info : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
        Finally
            ds.Dispose()
        End Try
    End Sub
    Private Sub SimpleButton6_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SimpleButton6.Click
        Tutup()
    End Sub

    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

    End Sub


    Private Sub BarButtonItem4_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem4.ItemClick
        Tutup()
    End Sub
    Sub Tutup()
        DialogResult = Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub


    Private Sub BarButtonItem6_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem6.ItemClick
        PrintPreview()
    End Sub
    Sub PrintPreview()
        CetakMRPTJualBersih(IIf(EditReport, action_.Edit, action_.Preview))
    End Sub
    Private Sub CetakMRPTJual(ByVal Action As action_)
        Dim namafile As String
        Dim strsql As String = ""
        Dim RefundFood As Double = 0.0 '46
        Dim RefundNonFood As Double = 0.0 '47
        Dim RefundFreshFood As Double = 0.0 '48
        Try
            strsql = "Select Sum(MJualD.Harga*MJuald.Qty) as Jumlah From " & _
                     "MJualD Inner Join MJual On MJualD.IDJual=MJual.NoID " & _
                     "Inner Join MBarang On MJualD.IDBarang=MBarang.NoID " & _
                     "Inner Join MKategori On MBarang.IDKategori=MKategori.NoID " & _
                     "where MJualD.Transaksi='RTN' and MKategori.IDParent=46 and MJual.Tanggal>= " & Format(TglDari.DateTime, "yyyy/MM/dd") & " and MJual.Tanggal<" & Format(TglSampai.DateTime.AddDays(1), "yyyy/MM/dd")
            If txtSupplier.Enabled Then
                strsql = strsql & " and MJual.IDPos=" & NullTolInt(txtSupplier.EditValue)
            End If
            RefundFood = EksekusiSQLSkalar(strsql)
            strsql = "Select Sum(MJualD.Harga*MJuald.Qty) as Jumlah From " & _
                     "MJualD Inner Join MJual On MJualD.IDJual=MJual.NoID " & _
                     "Inner Join MBarang On MJualD.IDBarang=MBarang.NoID " & _
                     "Inner Join MKategori On MBarang.IDKategori=MKategori.NoID " & _
                     "where MJualD.Transaksi='RTN' and MKategori.IDParent=47 and MJual.Tanggal>= " & Format(TglDari.DateTime, "yyyy/MM/dd") & " and MJual.Tanggal<" & Format(TglSampai.DateTime.AddDays(1), "yyyy/MM/dd")
            If txtSupplier.Enabled Then
                strsql = strsql & " and MJual.IDPos=" & NullTolInt(txtSupplier.EditValue)
            End If
            RefundNonFood = EksekusiSQLSkalar(strsql)

            strsql = "Select Sum(MJualD.Harga*MJuald.Qty) as Jumlah From " & _
                                                  "MJualD Inner Join MJual On MJualD.IDJual=MJual.NoID " & _
                                                  "Inner Join MBarang On MJualD.IDBarang=MBarang.NoID " & _
                                                  "Inner Join MKategori On MBarang.IDKategori=MKategori.NoID " & _
                                                  "where MJualD.Transaksi='RTN' and MKategori.IDParent=48 and MJual.Tanggal>= " & Format(TglDari.DateTime, "yyyy/MM/dd") & " and MJual.Tanggal<" & Format(TglSampai.DateTime.AddDays(1), "yyyy/MM/dd")
            If txtSupplier.Enabled Then
                strsql = strsql & " and MJual.IDPos=" & NullTolInt(txtSupplier.EditValue)
            End If
            RefundFreshFood = EksekusiSQLSkalar(strsql)

            namafile = Application.StartupPath & "\report\Laporan" & TableMaster & ".rpt"
            If System.IO.File.Exists(namafile) Then
                If EditReport Then
                    Action = action_.Edit
                Else
                    Action = action_.Preview
                End If
                If txtSupplier.Text <> "" Then
                    ViewReport(Me.ParentForm, Action, namafile, Me.Text, "cdate({vRekapPenjualanPerDepartemen.Tanggal})>={@DariTanggal} AND cdate({vRekapPenjualanPerDepartemen.Tanggal})<={@SampaiTanggal} AND {vRekapPenjualanPerDepartemen.IDPos}={@IDPOS}", , "DariTanggal=cdate(" & TglDari.DateTime.ToString("yyyy,MM,dd") & ")&SampaiTanggal=cdate(" & TglSampai.DateTime.ToString("yyyy,MM,dd") & ")&IDPOS=" & NullToLong(txtSupplier.EditValue) & "&RefundFood=" & CLng(RefundFood) & "&RefundNonFood=" & CLng(RefundNonFood) & "&RefundFreshFood=" & CLng(RefundFreshFood) & "")
                Else
                    ViewReport(Me.ParentForm, Action, namafile, Me.Text, "cdate({vRekapPenjualanPerDepartemen.Tanggal})>={@DariTanggal} AND cdate({vRekapPenjualanPerDepartemen.Tanggal})<={@SampaiTanggal}", , "DariTanggal=cdate(" & TglDari.DateTime.ToString("yyyy,MM,dd") & ")&SampaiTanggal=cdate(" & TglSampai.DateTime.ToString("yyyy,MM,dd") & ")" & "&RefundFood=" & CLng(RefundFood) & "&RefundNonFood=" & CLng(RefundNonFood) & "&RefundFreshFood=" & CLng(RefundFreshFood) & "")
                End If
            Else
                DevExpress.XtraEditors.XtraMessageBox.Show("Nama File :" & namafile & " tidak ditemukan.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Stop)
            End If
        Catch EX As Exception
            XtraMessageBox.Show("Untuk mencetak, pilih item yang mau dicetak dan click cetak faktur.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
            'DevExpress.XtraEditors.XtraMessageBox.Show("Kesalahan:" & EX.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub


    Private Sub CetakMRPTJualBersih(ByVal Action As action_)
        Dim namafile As String
        Dim strsql As String = ""
        Dim RefundFood As Double = 0.0 '46
        Dim RefundNonFood As Double = 0.0 '47
        Dim RefundFreshFood As Double = 0.0 '48
        Dim RefundObat As Double = 0.0 '50
        Try
            strsql = "Select Sum(MJualD.Jumlah-ISNULL(MJualD.JumlahDiscNotaRp,0)) as Jumlah From " & _
                     "MJualD Inner Join MJual On MJualD.IDJual=MJual.NoID " & _
                     "Inner Join MBarang On MJualD.IDBarang=MBarang.NoID " & _
                     "Inner Join MKategori On MBarang.IDKategori=MKategori.NoID " & _
                     "where MJualD.Transaksi='RTN' and MKategori.IDParent=46 and MJual.Tanggal>= " & Format(TglDari.DateTime, "yyyy/MM/dd") & " and MJual.Tanggal<" & Format(TglSampai.DateTime.AddDays(1), "yyyy/MM/dd")
            If txtSupplier.Enabled Then
                strsql = strsql & " and MJual.IDPos=" & NullTolInt(txtSupplier.EditValue)
            End If
            RefundFood = EksekusiSQLSkalar(strsql)
            strsql = "Select Sum(MJualD.Jumlah-ISNULL(MJualD.JumlahDiscNotaRp,0)) as Jumlah From " & _
                     "MJualD Inner Join MJual On MJualD.IDJual=MJual.NoID " & _
                     "Inner Join MBarang On MJualD.IDBarang=MBarang.NoID " & _
                     "Inner Join MKategori On MBarang.IDKategori=MKategori.NoID " & _
                     "where MJualD.Transaksi='RTN' and MKategori.IDParent=47 and MJual.Tanggal>= " & Format(TglDari.DateTime, "yyyy/MM/dd") & " and MJual.Tanggal<" & Format(TglSampai.DateTime.AddDays(1), "yyyy/MM/dd")
            If txtSupplier.Enabled Then
                strsql = strsql & " and MJual.IDPos=" & NullTolInt(txtSupplier.EditValue)
            End If
            RefundNonFood = EksekusiSQLSkalar(strsql)

            strsql = "Select Sum(MJualD.Jumlah-ISNULL(MJualD.JumlahDiscNotaRp,0)) as Jumlah From " & _
                     "MJualD Inner Join MJual On MJualD.IDJual=MJual.NoID " & _
                     "Inner Join MBarang On MJualD.IDBarang=MBarang.NoID " & _
                     "Inner Join MKategori On MBarang.IDKategori=MKategori.NoID " & _
                     "where MJualD.Transaksi='RTN' and MKategori.IDParent=48 and MJual.Tanggal>= " & Format(TglDari.DateTime, "yyyy/MM/dd") & " and MJual.Tanggal<" & Format(TglSampai.DateTime.AddDays(1), "yyyy/MM/dd")
            If txtSupplier.Enabled Then
                strsql = strsql & " and MJual.IDPos=" & NullTolInt(txtSupplier.EditValue)
            End If
            RefundFreshFood = EksekusiSQLSkalar(strsql)
            strsql = "Select Sum(MJualD.Jumlah-ISNULL(MJualD.JumlahDiscNotaRp,0)) as Jumlah From " & _
                     "MJualD Inner Join MJual On MJualD.IDJual=MJual.NoID " & _
                     "Inner Join MBarang On MJualD.IDBarang=MBarang.NoID " & _
                     "Inner Join MKategori On MBarang.IDKategori=MKategori.NoID " & _
                     "where MJualD.Transaksi='RTN' and MKategori.IDParent=50 and MJual.Tanggal>= " & Format(TglDari.DateTime, "yyyy/MM/dd") & " and MJual.Tanggal<" & Format(TglSampai.DateTime.AddDays(1), "yyyy/MM/dd")
            If txtSupplier.Enabled Then
                strsql = strsql & " and MJual.IDPos=" & NullTolInt(txtSupplier.EditValue)
            End If
            RefundObat = EksekusiSQLSkalar(strsql)

            namafile = Application.StartupPath & "\report\LaporanPenjualanPerDepartemenBersih.rpt"
            If System.IO.File.Exists(namafile) Then
                If EditReport Then
                    Action = action_.Edit
                Else
                    Action = action_.Preview
                End If
                If txtSupplier.Text <> "" Then
                    ViewReport(Me.ParentForm, Action, namafile, Me.Text, "cdate({vRekapPenjualanPerDepartemenBersih.Tanggal})>={@DariTanggal} AND cdate({vRekapPenjualanPerDepartemenBersih.Tanggal})<={@SampaiTanggal} AND CSTR({vRekapPenjualanPerDepartemenBersih.KodeKategori})<>'34' AND {vRekapPenjualanPerDepartemenBersih.IDPos}={@IDPOS}", , "DariTanggal=cdate(" & TglDari.DateTime.ToString("yyyy,MM,dd") & ")&SampaiTanggal=cdate(" & TglSampai.DateTime.ToString("yyyy,MM,dd") & ")&IDPOS=" & NullToLong(txtSupplier.EditValue) & "&RefundObat=" & CLng(RefundObat) & "&RefundFood=" & CLng(RefundFood) & "&RefundNonFood=" & CLng(RefundNonFood) & "&RefundFreshFood=" & CLng(RefundFreshFood) & "")
                Else
                    ViewReport(Me.ParentForm, Action, namafile, Me.Text, "cdate({vRekapPenjualanPerDepartemenBersih.Tanggal})>={@DariTanggal} AND cdate({vRekapPenjualanPerDepartemenBersih.Tanggal})<={@SampaiTanggal} AND CSTR({vRekapPenjualanPerDepartemenBersih.KodeKategori})<>'34'", , "DariTanggal=cdate(" & TglDari.DateTime.ToString("yyyy,MM,dd") & ")&SampaiTanggal=cdate(" & TglSampai.DateTime.ToString("yyyy,MM,dd") & ")" & "&RefundObat=" & CLng(RefundObat) & "&RefundFood=" & CLng(RefundFood) & "&RefundNonFood=" & CLng(RefundNonFood) & "&RefundFreshFood=" & CLng(RefundFreshFood) & "")
                End If
            Else
                DevExpress.XtraEditors.XtraMessageBox.Show("Nama File :" & namafile & " tidak ditemukan.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Stop)
            End If
        Catch EX As Exception
            XtraMessageBox.Show("Untuk mencetak, pilih item yang mau dicetak dan click cetak faktur.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
            'DevExpress.XtraEditors.XtraMessageBox.Show("Kesalahan:" & EX.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub
    Private Sub CetakMRPTJualBersihDenganDiskon(ByVal Action As action_)
        Dim namafile As String
        Dim strsql As String = ""
        Dim RefundFood As Double = 0.0 '46
        Dim RefundNonFood As Double = 0.0 '47
        Dim RefundFreshFood As Double = 0.0 '48
        Dim RefundObat As Double = 0.0 '50
        Try
            strsql = "Select Sum(MJualD.Jumlah-ISNULL(MJualD.JumlahDiscNotaRp,0)) as Jumlah From " & _
                     "MJualD Inner Join MJual On MJualD.IDJual=MJual.NoID " & _
                     "Inner Join MBarang On MJualD.IDBarang=MBarang.NoID " & _
                     "Inner Join MKategori On MBarang.IDKategori=MKategori.NoID " & _
                     "where MJualD.Transaksi='RTN' and MKategori.IDParent=46 and MJual.Tanggal>= " & Format(TglDari.DateTime, "yyyy/MM/dd") & " and MJual.Tanggal<" & Format(TglSampai.DateTime.AddDays(1), "yyyy/MM/dd")
            If txtSupplier.Enabled Then
                strsql = strsql & " and MJual.IDPos=" & NullTolInt(txtSupplier.EditValue)
            End If
            RefundFood = EksekusiSQLSkalar(strsql)
            strsql = "Select Sum(MJualD.Jumlah-ISNULL(MJualD.JumlahDiscNotaRp,0)) as Jumlah From " & _
                     "MJualD Inner Join MJual On MJualD.IDJual=MJual.NoID " & _
                     "Inner Join MBarang On MJualD.IDBarang=MBarang.NoID " & _
                     "Inner Join MKategori On MBarang.IDKategori=MKategori.NoID " & _
                     "where MJualD.Transaksi='RTN' and MKategori.IDParent=47 and MJual.Tanggal>= " & Format(TglDari.DateTime, "yyyy/MM/dd") & " and MJual.Tanggal<" & Format(TglSampai.DateTime.AddDays(1), "yyyy/MM/dd")
            If txtSupplier.Enabled Then
                strsql = strsql & " and MJual.IDPos=" & NullTolInt(txtSupplier.EditValue)
            End If
            RefundNonFood = EksekusiSQLSkalar(strsql)

            strsql = "Select Sum(MJualD.Jumlah-ISNULL(MJualD.JumlahDiscNotaRp,0)) as Jumlah From " & _
                     "MJualD Inner Join MJual On MJualD.IDJual=MJual.NoID " & _
                     "Inner Join MBarang On MJualD.IDBarang=MBarang.NoID " & _
                     "Inner Join MKategori On MBarang.IDKategori=MKategori.NoID " & _
                     "where MJualD.Transaksi='RTN' and MKategori.IDParent=48 and MJual.Tanggal>= " & Format(TglDari.DateTime, "yyyy/MM/dd") & " and MJual.Tanggal<" & Format(TglSampai.DateTime.AddDays(1), "yyyy/MM/dd")
            If txtSupplier.Enabled Then
                strsql = strsql & " and MJual.IDPos=" & NullTolInt(txtSupplier.EditValue)
            End If
            RefundFreshFood = EksekusiSQLSkalar(strsql)
            strsql = "Select Sum(MJualD.Jumlah-ISNULL(MJualD.JumlahDiscNotaRp,0)) as Jumlah From " & _
                     "MJualD Inner Join MJual On MJualD.IDJual=MJual.NoID " & _
                     "Inner Join MBarang On MJualD.IDBarang=MBarang.NoID " & _
                     "Inner Join MKategori On MBarang.IDKategori=MKategori.NoID " & _
                     "where MJualD.Transaksi='RTN' and MKategori.IDParent=50 and MJual.Tanggal>= " & Format(TglDari.DateTime, "yyyy/MM/dd") & " and MJual.Tanggal<" & Format(TglSampai.DateTime.AddDays(1), "yyyy/MM/dd")
            If txtSupplier.Enabled Then
                strsql = strsql & " and MJual.IDPos=" & NullTolInt(txtSupplier.EditValue)
            End If
            RefundObat = EksekusiSQLSkalar(strsql)

            namafile = Application.StartupPath & "\report\LaporanPenjualanPerDepartemenBersihDgDiscMember.rpt"
            If System.IO.File.Exists(namafile) Then
                If EditReport Then
                    Action = action_.Edit
                Else
                    Action = action_.Preview
                End If
                If txtSupplier.Text <> "" Then
                    ViewReport(Me.ParentForm, Action, namafile, Me.Text, "cdate({vRekapPenjualanPerDepartemenBersih.Tanggal})>={@DariTanggal} AND cdate({vRekapPenjualanPerDepartemenBersih.Tanggal})<={@SampaiTanggal} AND CSTR({vRekapPenjualanPerDepartemenBersih.KodeKategori})<>'34' AND {vRekapPenjualanPerDepartemenBersih.IDPos}={@IDPOS}", , "DariTanggal=cdate(" & TglDari.DateTime.ToString("yyyy,MM,dd") & ")&SampaiTanggal=cdate(" & TglSampai.DateTime.ToString("yyyy,MM,dd") & ")&IDPOS=" & NullToLong(txtSupplier.EditValue) & "&RefundObat=" & CLng(RefundObat) & "&RefundFood=" & CLng(RefundFood) & "&RefundNonFood=" & CLng(RefundNonFood) & "&RefundFreshFood=" & CLng(RefundFreshFood) & "")
                Else
                    ViewReport(Me.ParentForm, Action, namafile, Me.Text, "cdate({vRekapPenjualanPerDepartemenBersih.Tanggal})>={@DariTanggal} AND cdate({vRekapPenjualanPerDepartemenBersih.Tanggal})<={@SampaiTanggal} AND CSTR({vRekapPenjualanPerDepartemenBersih.KodeKategori})<>'34'", , "DariTanggal=cdate(" & TglDari.DateTime.ToString("yyyy,MM,dd") & ")&SampaiTanggal=cdate(" & TglSampai.DateTime.ToString("yyyy,MM,dd") & ")" & "&RefundObat=" & CLng(RefundObat) & "&RefundFood=" & CLng(RefundFood) & "&RefundNonFood=" & CLng(RefundNonFood) & "&RefundFreshFood=" & CLng(RefundFreshFood) & "")
                End If
            Else
                DevExpress.XtraEditors.XtraMessageBox.Show("Nama File :" & namafile & " tidak ditemukan.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Stop)
            End If
        Catch EX As Exception
            XtraMessageBox.Show("Untuk mencetak, pilih item yang mau dicetak dan click cetak faktur.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
            'DevExpress.XtraEditors.XtraMessageBox.Show("Kesalahan:" & EX.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub
    Private Sub SimpleButton5_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SimpleButton5.Click
        If IsValidasi() Then
            HitungSebarkanDiscountMemberPerBarang()
            Application.DoEvents()
            PrintPreview()
        End If
    End Sub

    Private Function IsValidasi() As Boolean
        Dim SQL As String = ""
        If txtSupplier.Text <> "" Then
            SQL = "SELECT COUNT(MJual.NoID) FROM MJual WHERE IsNull(MJual.IsPosted,0)=0 AND MJual.IDPos=" & NullToLong(txtSupplier.EditValue) & " AND MJual.Tanggal>='" & TglDari.DateTime.ToString("yyyy-MM-dd") & "' AND MJual.Tanggal<'" & TglSampai.DateTime.AddDays(1).ToString("yyyy-MM-dd") & "'"
        Else
            SQL = "SELECT COUNT(MJual.NoID) FROM MJual WHERE IsNull(MJual.IsPosted,0)=0 AND MJual.Tanggal>='" & TglDari.DateTime.ToString("yyyy-MM-dd") & "' AND MJual.Tanggal<'" & TglSampai.DateTime.AddDays(1).ToString("yyyy-MM-dd") & "'"
        End If
        If NullToLong(EksekusiSQlSkalarNew(SQL)) >= 1 Then
            XtraMessageBox.Show("Masih ada data yg kurang seimbang, lakukan download penjualan terlebih dahulu.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
            Return False
            Exit Function
        End If
        Return True
    End Function
    Sub HitungSebarkanDiscountMemberPerBarang()
        'Dim Prosen As Double
        'Dim JumlahDiskonNota As Double
        'Dim JumlahDiskonDetil As Double
        ProgressBarControl1.Position = 0
        Dim ds As New DataSet
        Dim dsdetil As New DataSet
        Dim SQL As String = ""
        Dim IDJual_ As Long
        Dim IDJualD_ As Long
        Dim DiscNota As Double
        Dim DiscItemRp As Double
        Dim DiscNotaProsen As Double
        Dim JumlahDiscTerpakai As Double
        Dim i As Integer
        Try

            Dim strsql As String
            strsql = "SELECT  MJual.NoID,  round( (mjual.DiskonNotaRp*100/ mjual.BarangPoin ),2)  as Discprosen,mjual.DiskonNotaRp " & _
                     "FROM [MJual] " & _
                     "where IsNull(MJual.DiskonNotaRp,0) <> 0 and mjual.tanggal>='" & Format(TglDari.DateTime, "yyyy/MM/dd") & "' and MJual.Tanggal<'" & Format(TglSampai.DateTime.AddDays(1), "yyyy/MM/dd") & "'"
            ds = ExecuteDataset("Jual", strsql)
            For i = 0 To ds.Tables("Jual").Rows.Count - 1
                ProgressBarControl1.Position = (i + 1) / ds.Tables("Jual").Rows.Count * 100
                Application.DoEvents()
                IDJual_ = ds.Tables("Jual").Rows(i).Item("NoID")
                DiscNota = NullToDbl(ds.Tables("Jual").Rows(i).Item("DiskonNotaRp"))
                DiscNotaProsen = NullToDbl(ds.Tables("Jual").Rows(i).Item("Discprosen"))
                strsql = "SELECT MJualD.NoID,Jumlah " & _
                         " FROM [MJualD] " & _
                         " WHERE mjualD.IDJual=" & IDJual_ & " and MjualD.IsPoin=1 order by NoID"
                dsdetil = ExecuteDataset("JualD", strsql)
                JumlahDiscTerpakai = 0
                For j = 0 To dsdetil.Tables("JualD").Rows.Count - 1
                    IDJualD_ = NullToLong(dsdetil.Tables("JualD").Rows(j).Item("NoID"))
                    If j = dsdetil.Tables("JualD").Rows.Count - 1 Then 'terakhir
                        DiscItemRp = DiscNota - JumlahDiscTerpakai
                    Else
                        DiscItemRp = Math.Round(NullToDbl(dsdetil.Tables("JualD").Rows(j).Item("Jumlah")) * DiscNotaProsen / 100, 0)
                        JumlahDiscTerpakai = JumlahDiscTerpakai + DiscItemRp
                    End If
                    EksekusiSQL("Update MJualD set DiscNotaProsen=" & FixKoma(DiscNotaProsen) & ", JumlahDiscNotaRp=" & FixKoma(DiscItemRp) & " where NoID=" & IDJualD_)
                Next
            Next
            dsdetil.Dispose()
            ds.Dispose()
        Catch
        End Try
    End Sub
    Private Sub LabelControl1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LabelControl1.Click
        TglDari.Enabled = Not TglDari.Enabled
        TglSampai.Enabled = TglDari.Enabled
    End Sub


    Private Sub mnSaveLayouts_ItemClick(ByVal sender As Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles mnSaveLayouts.ItemClick
        Dim xOtorisasi As New frmOtorisasiAdmin
        Try
            If XtraMessageBox.Show("Simpan Layout?", NamaAplikasi, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = Windows.Forms.DialogResult.Yes AndAlso xOtorisasi.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
                GridView4.SaveLayoutToXml(FolderLayouts & FormName & GridView4.Name & ".xml")
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

    Private Sub LabelControl9_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LabelControl9.Click

    End Sub

    Private Sub SimpleButton1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SimpleButton1.Click
        If IsValidasi() Then
            HitungSebarkanDiscountMemberPerBarang()
            Application.DoEvents()
            CetakMRPTJualBersihDenganDiskon(IIf(EditReport, action_.Edit, action_.Preview))
        End If
    End Sub

    Private Sub SimpleButton8_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SimpleButton8.Click

    End Sub
End Class