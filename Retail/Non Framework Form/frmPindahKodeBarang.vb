Imports System.Data.SqlClient
Public Class frmPindahKodeBarang
    Public IDBarangLama As Long
    Public IDBarangDLama As Long
    Private Sub cmdTutup_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdTutup.Click
        Me.Close()

    End Sub
    Private Sub RefreshLookUp()
        Dim ds As New DataSet
        Dim sql As String
        Try
            sql = "SELECT MBarang.NoID, MBarang.Kode, MBarang.Nama, MSatuan.Nama AS Satuan FROM MBarang LEFT JOIN MSatuan ON MSatuan.NoID=MBarang.IDSatuan WHERE MBarang.IsActive=1"
            ds = ExecuteDataset("MBarang", sql)
            txtBarang.Properties.DataSource = ds.Tables("MBarang")
            txtBarang.Properties.ValueMember = "NoID"
        Catch ex As Exception
            MsgBox("Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK + MessageBoxIcon.Error)
        Finally
            ds.Dispose()
        End Try
    End Sub

    Private Sub cmdSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSave.Click
        If PindahBarang() Then
            DialogResult = Windows.Forms.DialogResult.OK
            Me.Close()
        End If
    End Sub
    Function PindahBarang() As Boolean
        Dim hasil As Boolean
        hasil = False
        Try
            If NullToLong(txtBarang.EditValue) <= 0 Then
                MsgBox("Barang Tujuan harus diisi!", MsgBoxStyle.Critical)
            Else
                EksekusiSQL("Update MKartuStok set IDBarang=" & NullToLong(txtBarang.EditValue) & " where IDBarangD=" & IDBarangDLama)
                EksekusiSQL("Update MBeliD set IDBarang=" & NullToLong(txtBarang.EditValue) & " where IDBarangD=" & IDBarangDLama)
                EksekusiSQL("Update MReturBeliD set IDBarang=" & NullToLong(txtBarang.EditValue) & " where IDBarangD=" & IDBarangDLama)
                EksekusiSQL("Update MJualD set IDBarang=" & NullToLong(txtBarang.EditValue) & " where IDBarangD=" & IDBarangDLama)
                EksekusiSQL("Update MReturJualD set IDBarang=" & NullToLong(txtBarang.EditValue) & " where IDBarangD=" & IDBarangDLama)
                EksekusiSQL("Update MPemakaianD set IDBarang=" & NullToLong(txtBarang.EditValue) & " where IDBarangD=" & IDBarangDLama)
                EksekusiSQL("Update MPenyesuaianD set IDBarang=" & NullToLong(txtBarang.EditValue) & " where IDBarangD=" & IDBarangDLama)
                EksekusiSQL("Update MMutasiGudangD Set IDBarang=" & NullToLong(txtBarang.EditValue) & " where IDBarangD=" & IDBarangDLama)
                EksekusiSQL("Update MStockOpnameD set IDBarang=" & NullToLong(txtBarang.EditValue) & " where IDBarangD=" & IDBarangDLama)
                EksekusiSQL("Update MBarangD set IDBarang=" & NullToLong(txtBarang.EditValue) & " where NoID=" & IDBarangDLama)

                If NullToLong(EksekusiSQlSkalarNew("select count(MbarangD.NoID) from MbarangD where IsActive=1 and IDBarang=" & IDBarangLama)) = 0 Then
                    EksekusiSQL("Update MBarang set IsActive=0  where NoID=" & IDBarangLama)
                End If
            End If
            hasil = True
        Catch ex As Exception
            MsgBox("Ada kesalahan " & ex.Message, MsgBoxStyle.Information)
        End Try
        Return hasil
    End Function

    Private Sub XtraForm1_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        RefreshLookUp()
    End Sub
End Class