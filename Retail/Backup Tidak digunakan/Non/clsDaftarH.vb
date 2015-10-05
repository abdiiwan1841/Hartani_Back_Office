Imports DevExpress.XtraGrid
Imports DevExpress.XtraGrid.Views.Base
Imports DevExpress.XtraEditors

Imports ClassLibrary1
Public Class clsDaftarH
    Dim WithEvents x As New ctrlDaftar
    Dim WithEvents grd As GridControl = x.gridcontrol1
    Private Sub frmEntri_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        pnlGrid.Controls.Add(x)
        x.Dock = DockStyle.Fill

    End Sub

    Private Sub grd_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles grd.DoubleClick
        Try
            Dim view As ColumnView = grd.FocusedView
            Dim row As System.Data.DataRow = view.GetDataRow(x.gridview1.FocusedRowHandle)
            Dim frm As New clsSimpleEntri
            frm.isNew = False
            frm.NoID = row("NoID")
            frm.ShowDialog()
            frm.Dispose()
        Catch ex As Exception
            XtraMessageBox.Show(Me, ex.Message, ".:: Pesan Kesalahan ::.", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try

    End Sub
End Class