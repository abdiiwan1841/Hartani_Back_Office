Imports System.Data.SqlClient
Imports DevExpress.XtraEditors
Imports BarTender
Imports System.Data.Odbc

Public Class frmGetQty
    Public Qty As Double
    Private Sub cmdTutup_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdTutup.Click
        DialogResult = Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub cmdOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdOK.Click
        Qty = txtQty.EditValue
        DialogResult = Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub
End Class