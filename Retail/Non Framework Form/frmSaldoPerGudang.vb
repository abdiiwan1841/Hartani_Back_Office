Imports System.Data.SqlClient
Imports System.Data
Imports System.Windows.Forms
Imports DevExpress.XtraEditors
Imports DevExpress.XtraGrid.Views
Imports DevExpress.XtraGrid.Columns
Imports DevExpress.XtraGrid.Views.Base
Imports System.Drawing
Imports DevExpress.XtraEditors.Repository
'Imports FastReport
'Imports FastReport.Utils

Public Class frmSaldoPerGudang
    Dim oConn As SqlConnection = New SqlConnection(StrKonSql)
    Dim ocmd As SqlCommand = New SqlCommand()
    Dim ds As New DataSet
    Dim BS As New BindingSource
    Dim oDA As New SqlDataAdapter
    Dim repckedit As New RepositoryItemCheckEdit
    Dim reppicedit As New RepositoryItemPictureEdit
    Dim IsShow As Boolean = True
    Public IDBarang As Long = -1
    Public ParentMDIForm As XtraForm = Nothing
    Private Sub frmDaftar_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        'KODE DIBAWAH UNTUK MEMBUKA FORM HANYA 1
        Dim frmEntri As frmDaftarBarang = Nothing
        Dim F As Object
        For Each F In ParentMDIForm.MdiChildren
            If TypeOf F Is frmDaftarBarang Then
                frmEntri = F
                Exit For
            End If
        Next
        If frmEntri Is Nothing Then
            'frmEntri = New frmDaftarBarang
            'frmEntri.WindowState = FormWindowState.Maximized
            'frmEntri.MdiParent = ParentMDIForm
        Else
            frmEntri.IsShowStock = False
        End If

        'KODE DIBAWAH UNTUK MEMBUKA FORM HANYA 1
        Dim frmAlamat As frmDaftarAlamat = Nothing
        Dim G As Object
        For Each G In ParentMDIForm.MdiChildren
            If TypeOf G Is frmDaftarAlamat Then
                frmAlamat = G
                Exit For
            End If
        Next
        If frmAlamat Is Nothing Then
            'frmEntri = New frmDaftarBarang
            'frmEntri.WindowState = FormWindowState.Maximized
            'frmEntri.MdiParent = ParentMDIForm
        Else
            frmAlamat.IsShowStock = False
        End If
    End Sub

    Private Sub ctrlDaftar_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        ocmd.Connection = oConn
        oConn.Open()
        RefreshLookUpWilayah()
        txtWilayah.EditValue = DefIDWilayah
        RefreshData()
        IsShow = False
        RestoreLayout()
        Me.lbDaftar.Text = Me.Text
        FungsiControl.SetForm(Me)
    End Sub
    Private Sub RestoreLayout()
        For i As Integer = 0 To GV1.Columns.Count - 1
            Select Case GV1.Columns(i).ColumnType.Name.ToLower
                Case "int32", "int64", "int"
                    GV1.Columns(i).DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
                    GV1.Columns(i).DisplayFormat.FormatString = "n0"
                Case "decimal", "single", "money", "double"
                    GV1.Columns(i).DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
                    GV1.Columns(i).DisplayFormat.FormatString = "n2"
                Case "string"
                    GV1.Columns(i).DisplayFormat.FormatType = DevExpress.Utils.FormatType.None
                    GV1.Columns(i).DisplayFormat.FormatString = ""
                Case "date"
                    GV1.Columns(i).DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime
                    GV1.Columns(i).DisplayFormat.FormatString = "dd-MM-yyyy"
                Case "datetime"
                    GV1.Columns(i).DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime
                    GV1.Columns(i).DisplayFormat.FormatString = "dd-MM-yyyy HH:mm"
                Case "byte[]"
                    reppicedit.SizeMode = DevExpress.XtraEditors.Controls.PictureSizeMode.Squeeze
                    GV1.Columns(i).OptionsColumn.AllowGroup = False
                    GV1.Columns(i).OptionsColumn.AllowSort = False
                    GV1.Columns(i).OptionsFilter.AllowFilter = False
                    GV1.Columns(i).ColumnEdit = reppicedit
                Case "boolean"
                    GV1.Columns(i).ColumnEdit = repckedit
            End Select
        Next
        If System.IO.File.Exists(folderLayouts & Me.Name & GV1.Name & ".xml") Then
            GV1.RestoreLayoutFromXml(folderLayouts & Me.Name & GV1.Name & ".xml")
        End If

    End Sub
    Sub RefreshData()
        Dim Cur As Cursor = Windows.Forms.Cursor.Current
        Dim SQL As String = ""
        Dim IsPerluWhere As Boolean = False
        Dim ds As New DataSet
        Windows.Forms.Cursor.Current = Cursors.WaitCursor
        Try
            If IsShow = False AndAlso txtWilayah.Text = "" Then
                If XtraMessageBox.Show("Wilayah masih kosong, ingin melanjutkan ?", NamaAplikasi, MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) = Windows.Forms.DialogResult.No Then Exit Try
            End If
            SQL = "SELECT (CASE WHEN MGudang.IsBS=1 THEN 'Tidak Siap Jual' ELSE 'Siap Jual' END) AS Status, MGudang.Nama AS Gudang, MWilayah.Nama AS Wilayah, SUM((MKartustok.QTYMasuk*MKartustok.Konversi)-(MKartustok.QTYKeluar*MKartustok.Konversi)) AS QtyStok"
            SQL &= " FROM MKartuStok LEFT JOIN (MGudang LEFT JOIN MWilayah ON MWilayah.NoID=MGudang.IDWilayah) ON MGudang.NoID=MKartuStok.IDGudang "
            SQL &= " LEFT JOIN MSatuan ON MSatuan.NoID=MKartuStok.IDSatuan "
            SQL &= " WHERE MKartustok.Tanggal<'" & Format(DateAdd(DateInterval.Day, 1, TanggalSystem), "yyyy-MM-dd") & "' AND MKartuStok.IDBarang=" & IDBarang
            If Not IsSupervisor Then
                SQL &= IIf(IsPerluWhere, " WHERE ", " AND") & " MWilayah.NoID=" & DefIDWilayah & vbCrLf
                IsPerluWhere = False
                txtWilayah.Properties.ReadOnly = False
            Else
                txtWilayah.Properties.ReadOnly = True
            End If
            If txtWilayah.Text <> "" Then
                SQL &= IIf(IsPerluWhere, " WHERE ", " AND") & " MWilayah.NoID=" & NullToLong(txtWilayah.EditValue) & vbCrLf
                IsPerluWhere = False
            End If
            SQL &= " GROUP BY MGudang.Nama, MWilayah.Nama, MGudang.IsBS "
            ds = modSqlServer.ExecuteDataset("Data", SQL)
            GC1.DataSource = ds.Tables("Data")
        Catch ex As Exception
            XtraMessageBox.Show("Error : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK)
        Finally
            ds.Dispose()
            oDA.Dispose()
            'ocmd2.Dispose()
            'cn.Close()
            'cn.Dispose()
            Windows.Forms.Cursor.Current = Cur
        End Try
    End Sub

    Private Sub SimpleButton6_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SimpleButton6.Click
        Tutup()
    End Sub

    Private Sub SimpleButton9_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SimpleButton9.Click
        DialogResult = Windows.Forms.DialogResult.OK
        Me.Close()
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
        Me.Dispose()
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
            GC1.ExportToXls(dlgsave.FileName)
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

    Private Sub mnRefresh_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles mnRefresh.ItemClick
        RefreshData()
    End Sub

    Private Sub RefreshLookUpWilayah()
        Dim ds As New DataSet
        Dim SQL As String = ""
        Try
            SQL = "SELECT MWilayah.NoID, MWilayah.Kode, MWilayah.Nama " & _
                  " FROM MWilayah WHERE MWilayah.IsActive=1 "
            ds = ExecuteDataset("MWilayah", SQL)
            txtWilayah.Properties.DataSource = ds.Tables("MWilayah")
            txtWilayah.Properties.DisplayMember = "Nama"
            txtWilayah.Properties.ValueMember = "NoID"
        Catch ex As Exception
            XtraMessageBox.Show("Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            ds.Dispose()
        End Try
    End Sub

    Private Sub GC1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles GC1.Click

    End Sub

    Private Sub barButtonItem1_ItemClick(ByVal sender As Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs)
        SimpleButton9.PerformClick()
    End Sub

    Private Sub txtWilayah_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtWilayah.EditValueChanged
        RefreshData()
    End Sub

    Private Sub txtWilayah_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtWilayah.KeyDown
        If e.KeyCode = Keys.F7 Then
            Dim x As New frmOtorisasiAdmin
            Try
                Me.TopMost = False
                If txtWilayah.Properties.ReadOnly Then
                    If x.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
                        txtWilayah.Properties.ReadOnly = False
                    End If
                End If
            Catch ex As Exception
                XtraMessageBox.Show("Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
            Finally
                x.Dispose()
                Me.TopMost = True
            End Try
        End If
    End Sub

    Private Sub mnSimpanLayouts_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles mnSimpanLayouts.ItemClick
        Dim xOtorisasi As New frmOtorisasiAdmin
        Try
            Me.TopMost = False
            If XtraMessageBox.Show("Simpan Layout?", NamaAplikasi, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = Windows.Forms.DialogResult.Yes AndAlso xOtorisasi.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
                GV1.SaveLayoutToXml(folderLayouts & Me.Name & GV1.Name & ".xml")
                gvWilayah.SaveLayoutToXml(folderLayouts & Me.Name & gvWilayah.Name & ".xml")
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

    Private Sub gvWilayah_DataSourceChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvWilayah.DataSourceChanged
        If System.IO.File.Exists(folderLayouts & Me.Name & gvWilayah.Name & ".xml") Then
            gvWilayah.RestoreLayoutFromXml(folderLayouts & Me.Name & gvWilayah.Name & ".xml")
        End If
        With gvWilayah
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
                        .Columns(i).DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime
                        .Columns(i).DisplayFormat.FormatString = "dd-MM-yyyy"
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
    End Sub
End Class