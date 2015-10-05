Imports DevExpress.XtraEditors
Imports System.Data.SqlClient
Imports System.Data.SQLite

Public Class frmLockFP
    Inherits frmSimpleEntri
    Dim Periode As Date
    Dim txtNama As TextEdit
    Dim txtKode As TextEdit
    Dim IsProsesLoad As Boolean = True

    Private Sub frmLockFP_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Private Sub frmLockFP_Shown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Shown
        For Each ctl As Control In LC1.Controls
            If TypeOf ctl Is DateEdit Then
                AddHandler dtEdit.EditValueChanged, AddressOf DateEdit_EditValueChanged
            ElseIf TypeOf ctl Is TextEdit Then
                If ctl.Name.ToLower = "txtkode".ToLower Then
                    txtKode = ctl
                ElseIf ctl.Name.ToLower = "txtnama".ToLower Then
                    txtNama = ctl
                End If
            End If
        Next
        IsProsesLoad = False
    End Sub

    Private Sub DateEdit_EditValueChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Try
            If FormName.ToLower <> "" AndAlso IsProsesLoad Then Exit Sub
            If TryCast(sender, DateEdit).Name.ToString.ToLower = "txtPeriode".ToLower Then
                txtKode.Text = Format(TryCast(sender, DateEdit).EditValue, "01MMyyyy")
                txtNama.Text = Format(TryCast(sender, DateEdit).EditValue, "01MMyyyy")
                'For Each ctl In LC1.Controls
                '    If TypeOf ctl Is TextEdit AndAlso ctl.name.ToString.ToLower = "txtKode".ToLower Then
                '        TryCast(ctl, TextEdit).Text = Format(TryCast(sender, DateEdit).EditValue, "01MMyyyy")
                '    ElseIf TypeOf ctl Is TextEdit AndAlso ctl.name.ToString.ToLower = "txtNama".ToLower Then
                '        TryCast(ctl, TextEdit).Text = Format(TryCast(sender, DateEdit).EditValue, "01MMyyyy")
                '    End If
                'Next
            End If
        Catch ex As Exception
            XtraMessageBox.Show("Info Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
        End Try
    End Sub
End Class
