Imports System
Imports System.Windows.Forms
Imports DevExpress.XtraEditors

Public Class FrmGantiPassword
    Public NoID As Long

    Private Sub FrmGantiPassord_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        txtUserName.Text = NamaUserAktif
        SetCtlMe()
        FungsiControl.SetForm(Me)
    End Sub

    Private Sub SetCtlMe()
        'SetButton(BtnCansel, button_.cmdCancelSave)
        'SetButton(btnSave, button_.cmdSave)
    End Sub
    Private Sub BtnCansel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnCansel.Click
        Close()
    End Sub

    Private Function IsValidasi() As Boolean
        Dim StrSQl As String
        'Dim Dss As DataSet

        If txtBaru.Text.Trim = "" Then
            XtraMessageBox.Show("Password Baru masih kosong.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
            txtBaru.Focus()
            IsValidasi = False
            Exit Function
            'ElseIf txtLama.Text.Trim = "" Then
            '    XtraMessageBox.Show("Password Lama masih kosong.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
            '    txtLama.Focus()
            '    IsValidasi = False
            '    Exit Function
        ElseIf txtConfr.Text.Trim = "" Then
            XtraMessageBox.Show("Confirmasi Password masih kosong.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
            txtConfr.Focus()
            IsValidasi = False
            Exit Function
        End If
        If (txtConfr.Text) <> (txtBaru.Text) Then
            XtraMessageBox.Show("Confirmasi Password Salah.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
            txtConfr.Focus()
            IsValidasi = False
            Exit Function
        End If

        StrSQl = "SELECT Pwd from MUser where NoID=" & IDUserAktif
        Dim PasswordLama As String = DecryptText(NullTostr(EksekusiSQlSkalarNew(StrSQl)), "vpoint")
        If txtLama.Text <> PasswordLama Then
            XtraMessageBox.Show("Password Lama yang anda masukkan Salah.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
            txtLama.Text = ""
            txtLama.Focus()
            Exit Function
        End If
        IsValidasi = True
    End Function

    Private Function UpdatePassword() As Boolean
        Dim CurentCursor As Cursor = Windows.Forms.Cursor.Current
        Windows.Forms.Cursor.Current = Cursors.WaitCursor
        Dim SqlStr As String
        If IsValidasi() Then
            Try
                SqlStr = "UPDATE MUSER SET PWD='" & FixApostropi(EncryptText(txtBaru.Text, "vpoint")) & "'" & _
                         " WHERE NoID=" & IDUserAktif
                EksekusiSQL(SqlStr)
                DialogResult = Windows.Forms.DialogResult.OK
                Return True
            Catch ex As Exception
                XtraMessageBox.Show(ex.Message, ".:: Pesan Kesalahan ::.", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return False
            End Try
        End If
        Windows.Forms.Cursor.Current = CurentCursor
    End Function

    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If UpdatePassword() Then
            XtraMessageBox.Show("Password behasil diupdate.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If
    End Sub
End Class