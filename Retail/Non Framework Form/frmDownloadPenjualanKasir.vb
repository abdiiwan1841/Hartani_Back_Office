Imports System
Imports System.Windows.Forms
Imports DevExpress.XtraEditors
Imports System.Data.OleDb
Imports System.Data.SqlClient

Public Class frmDownloadPenjualanKasir
    Public NoID As Long

    Private Sub FrmGantiPassord_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        RefreshPendukung()
        TglDari.EditValue = Today
        TglSampai.EditValue = Today
        txtKassa.EditValue = 1
    End Sub

    Private Sub SetCtlMe()
        'SetButton(BtnCansel, button_.cmdCancelSave)
        'SetButton(btnSave, button_.cmdSave)
    End Sub

    Sub RefreshPendukung()
        Dim ds As New DataSet
        Dim sql As String
        Try
            sql = "SELECT NoID, Kode, Nama FROM MPos WHERE IsActive=1"
            ds = ExecuteDataset("POS", sql)
            txtKassa.Properties.DataSource = ds.Tables("POS")
            If System.IO.File.Exists(FolderLayouts & Me.Name & gvKassa.Name & ".xml") Then
                gvKassa.RestoreLayoutFromXml(FolderLayouts & Me.Name & gvKassa.Name & ".xml")
            End If
        Catch ex As Exception
            XtraMessageBox.Show("Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            ds.Dispose()
        End Try
    End Sub

    Private Sub txtKassa_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtKassa.EditValueChanged
        txtPath.Text = NullToStr(EksekusiSQlSkalarNew("Select PathDBTemp from MPos Where NoID=" & NullTolInt(txtKassa.EditValue)))
    End Sub

    Private Sub cmdSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSave.Click
        Dim Tgl As Date = TglDari.DateTime, i As Integer = 0
        Dim SQL As String = "", ds As New DataSet
        If XtraMessageBox.Show("Yakin Mau Proses Download?, Lebih baik jika Koneksi jaringan sudah di cek!" & vbCrLf & _
                   "Dan proses ini dijalankan saat kasir tidak bekerja atau autoposting penjualan pos sudah dimatikan.", NamaAplikasi, MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.No Then Exit Sub
        Try
            cmdSave.Enabled = False
            Do While Tgl <= TglSampai.DateTime
                ProgressBarControl1.Position = i / IIf(DateDiff(DateInterval.Day, TglDari.DateTime, TglSampai.DateTime) = 0, 1, DateDiff(DateInterval.Day, TglDari.DateTime, TglSampai.DateTime)) * 100
                KirimDataKassaKeBO(NullToLong(txtKassa.EditValue), Tgl, PB1, CheckEdit1.Checked, txtPath.Text)
                i = i + 1
                Tgl = Tgl.AddDays(1)
            Loop
            ProgressBarControl1.Position = 0
            PB1.Position = 0
            Application.DoEvents()
        Catch ex As Exception
        Finally
            cmdSave.Enabled = True
        End Try

        Try 'Posting
            cmdSave.Enabled = False
            If txtKassa.Text = "" Then 'ALL Kassa
                SQL = "SELECT * FROM MJual WHERE IsPosted=0 AND Tanggal>='" & FixApostropi(TglDari.DateTime.ToString("yyyy/MM/dd")) & "' AND Tanggal<'" & FixApostropi(TglSampai.DateTime.AddDays(1).ToString("yyyy/MM/dd")) & "'"
            Else
                SQL = "SELECT * FROM MJual WHERE IDPOS=" & NullToLong(txtKassa.EditValue) & " AND IsPosted=0 AND Tanggal>='" & FixApostropi(TglDari.DateTime.ToString("yyyy/MM/dd")) & "' AND Tanggal<'" & FixApostropi(TglSampai.DateTime.AddDays(1).ToString("yyyy/MM/dd")) & "'"
            End If
            ds = ExecuteDataset("MJual", SQL)
            For x As Integer = 0 To ds.Tables("MJual").Rows.Count - 1
                ProgressBarControl1.Position = (x + 1) / NullToDbl(ds.Tables("MJual").Rows.Count) * 100
                PB1.Position = 100
                clsPostingPenjualan.PostingStokBarangPenjualan(NullToLong(ds.Tables("MJual").Rows(x).Item("NoID")))
                Application.DoEvents()
            Next
            ProgressBarControl1.Position = 0
            PB1.Position = 0
            Application.DoEvents()
        Catch ex As Exception

        Finally
            ds.Dispose()
            cmdSave.Enabled = True
        End Try
    End Sub

    Private Sub cmdTutup_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdTutup.Click
        DialogResult = Windows.Forms.DialogResult.Cancel
        Me.Close()
        Me.Dispose()
    End Sub
End Class