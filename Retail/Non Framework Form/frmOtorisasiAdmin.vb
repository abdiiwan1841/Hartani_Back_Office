Imports System.Data
Imports DevExpress.XtraEditors

Public Class frmOtorisasiAdmin
    Public IDUserAdmin As Long = -1
    Public NamaUserAdmin As String = ""
    Public IsForceSystem As Boolean = False
    Public UID As String = "sa"
    Public PWD As String = "sys"

    Private Sub cmdSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSave.Click
        If IsForceSystem AndAlso txtUserID.Text.ToUpper = UID.ToUpper AndAlso txtPwd.Text.ToUpper = PWD.ToUpper Then
            IDUserAdmin = IDUserAktif
            NamaUserAdmin = UID.ToUpper
            DialogResult = Windows.Forms.DialogResult.OK
            Me.Close()
        Else
            save()
        End If
    End Sub

    Public Sub save()
        Dim ds As New DataSet
        Try
            ds = ExecuteDataset("MUser", "SELECT * FROM MUser WHERE IsSupervisor=1 AND Kode='" & FixApostropi(txtUserID.Text) & "' AND Pwd='" & FixApostropi(EncryptText(txtPwd.Text.ToUpper, "vpoint")) & "'")
            If ds.Tables("MUser").Rows.Count >= 1 Then
                IDUserAdmin = NullToLong(ds.Tables("MUser").Rows(0).Item("NoID"))
                NamaUserAdmin = NullToStr(ds.Tables("MUser").Rows(0).Item("Nama"))
                DialogResult = Windows.Forms.DialogResult.OK
                Me.Close()
            Else
                XtraMessageBox.Show("User atau password yang anda masukkan salah.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Warning)
            End If
        Catch ex As Exception
            XtraMessageBox.Show("User atau password yang anda masukkan salah.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Warning)
        Finally
            ds.Dispose()
        End Try
    End Sub
    Private Sub SimpleButton1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SimpleButton1.Click
        DialogResult = Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub frmOtorisasiAdmin_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        FungsiControl.SetForm(Me)
    End Sub
End Class