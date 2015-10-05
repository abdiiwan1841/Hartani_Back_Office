Imports DevExpress.XtraEditors

Public Class frmLogin

    Private Sub SimpleButton2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SimpleButton2.Click
        DialogResult = Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub mnLogin_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnLogin.Click
        If IsLoginOk(FixApostropi(txtKodeUser.Text.ToUpper), FixApostropi(EncryptText(txtPassword.Text.ToUpper, "vpoint"))) Then
            TambahkanKolom()
            MainAccounting()
            AmbilSetingPerusahaan()
            TambahMenu()
            Me.DialogResult = Windows.Forms.DialogResult.OK
            Me.Close()
        Else
            'Me.TopMost = False
            XtraMessageBox.Show("Masukkan Kode User dan Password dengan benar!", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
            'Me.TopMost = True
        End If
    End Sub

    Private Sub frmLogin_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'Me.TopMost = True
        FungsiControl.SetForm(Me)
    End Sub

    Private Sub txtPassword_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtPassword.EditValueChanged

    End Sub

    Private Sub txtPassword_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtPassword.KeyDown
        If txtPassword.Text <> "" AndAlso e.KeyCode = Keys.Enter Then
            mnLogin.PerformClick()
        End If
    End Sub

    Private Sub SimpleButton1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SimpleButton1.Click
        MsgBox(PingIPComputer(txtKodeUser.Text, 1000).ToString)
    End Sub
End Class