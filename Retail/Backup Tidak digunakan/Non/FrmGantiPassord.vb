Imports System
Imports System.Windows.Forms
Imports DevExpress.XtraEditors
Imports VPOINT.Function.mPublic
Imports VPOINT.Function.Fungsi

Public Class FrmGantiPassord
    Public pStatus As VPOINT.Function.mPublic.ptipe
    Public NoID As Long

    Private Sub FrmGantiPassord_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        txtUserName.Text = NamaUserAktif
        SetCtlMe()
    End Sub

    Private Sub SetCtlMe()
        SetForm(Me)
        SetButton(BtnCansel, button_.cmdCancelSave)
        SetButton(btnSave, button_.cmdSave)
    End Sub
    Private Sub BtnCansel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnCansel.Click
        Close()
    End Sub

    Private Function IsValidasi() As Boolean
        Dim StrSQl As String
        'Dim Dss As DataSet

        If txtBaru.Text.Trim = "" Then
            FxMessage("Password Baru masih kosong.", NamaApplikasi)
            txtBaru.Focus()
            IsValidasi = False
            Exit Function
        ElseIf txtLama.Text.Trim = "" Then
            FxMessage("Password Lama masih kosong.", NamaApplikasi)
            txtLama.Focus()
            IsValidasi = False
            Exit Function
        ElseIf txtConfr.Text.Trim = "" Then
            FxMessage("Confirmasi Password masih kosong.", NamaApplikasi)
            txtConfr.Focus()
            IsValidasi = False
            Exit Function
        End If

        If (txtConfr.Text) <> (txtBaru.Text) Then
            FxMessage("Confirmasi Password Salah.", NamaApplikasi)
            txtConfr.Focus()
            IsValidasi = False
            Exit Function
        End If

        StrSQl = "SELECT Password from MUser where ID=" & IDUserAktif
        Dim PasswordLama As String = DecryptText(NullTostr(MyConn.EksekusiSQlSkalarNew(StrSQl)), "vpoint")
        If txtLama.Text <> PasswordLama Then
            FxMessage("Password Lama yang anda masukkan Salah.", NamaApplikasi)
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
                SqlStr = "update muser set password='" & _
                            Replace(EncryptText(txtBaru.Text, "vpoint"), "'", "''") & "'" & _
                            "where ID=" & IDUserAktif
                MyConn.EksekusiSQl(SqlStr)
                DialogResult = Windows.Forms.DialogResult.OK
                Return True
            Catch ex As Exception
                FxMessage(ex.Message, ".:: Pesan Kesalahan ::.", MessageBoxButtons.OK, MessageBoxIcon.Error, , , ex.StackTrace)
                Return False
            End Try
        End If
        Windows.Forms.Cursor.Current = CurentCursor
    End Function

    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If UpdatePassword() Then
            FxMessage("Password behasil diupdate.", NamaApplikasi)
        End If
    End Sub
End Class