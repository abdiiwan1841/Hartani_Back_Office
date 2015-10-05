Imports System.Data.SqlClient
Imports System.Data
Imports DevExpress.XtraEditors
Imports DevExpress.Utils

Public Class frmSettingAkun
    Dim SQL As String = ""
    Dim ds As New DataSet

    Private Sub frmSettingAkun_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        RefreshData()

    End Sub

    Private Sub RefreshLookUp()
        Try
            SQL = "SELECT MAkun.ID,Makun.IDMataUang, MMataUang.Nama AS NamaMataUang, MAkun.Kode,MAkun.Nama,MSubklasAkun.Nama as SubKlasifikasi,MKlasAkun.Nama as Klasifikasi,MKlasAkun.IsDebet " & _
                     " FROM ((MAkun LEFT JOIN MSubKlasAkun On MAkun.IDSubklasAkun=MSubKlasAkun.ID)" & _
                     " LEFT JOIN MKlasAkun On MSubKlasAkun.IDKlasAkun=MKlasAkun.ID)" & _
                     " LEFT JOIN MMataUang On MMataUang.ID=MAkun.IDMataUang"
            ds = ExecuteDataset("MSettingAkun", SQL)
            If Not ds.Tables("MSettingAkun") Is Nothing Then

            End If
        Catch ex As Exception
            XtraMessageBox.Show("Info Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK)
        End Try
    End Sub

    Private Sub RefreshData()
        Try
            SQL = "SELECT * FROM MSettingAkun "
            ds = ExecuteDataset("MSettingAkun", SQL)
            If ds.Tables("MSettingAkun").Rows.Count >= 1 Then

            Else

            End If
        Catch ex As Exception
            XtraMessageBox.Show("Info Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK)
        End Try
    End Sub
End Class