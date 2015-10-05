Imports System.IO
Imports DevExpress.XtraEditors

Public Class frmOpsiExportFP
    Private Sub txtFileName_ButtonClick(ByVal sender As Object, ByVal e As DevExpress.XtraEditors.Controls.ButtonPressedEventArgs) Handles txtFileName.ButtonClick
        If e.Button.Index = 0 Then
            Dim dlg As New SaveFileDialog
            Try
                dlg.Title = "Export Data PPN Masukkan CSV"
                dlg.Filter = "CSV PPN Masukkan Files|*.csv"
                If dlg.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
                    txtFileName.Text = dlg.FileName
                End If
            Catch ex As Exception
                XtraMessageBox.Show("Info Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
            Finally
                dlg.Dispose()
            End Try
        End If
    End Sub

    Private Sub txtFileName_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtFileName.EditValueChanged

    End Sub

    Private Sub SimpleButton6_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SimpleButton6.Click
        DialogResult = Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub cmdRefresh_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdRefresh.Click
        Dim dir As FileInfo = Nothing
        Try
            dir = New FileInfo(txtFileName.Text)
            If Not dir.Directory.Exists Then
                XtraMessageBox.Show("Folder Tidak ditemukan.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
                txtFileName.Focus()
            End If
            If NullToLong(txtPembetulan.EditValue) > 100 Then
                If XtraMessageBox.Show("Pembetulan sudah lebih dari 100." & vbCrLf & "Ingin melanjutkan penyimpanan?", NamaAplikasi, MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) = Windows.Forms.DialogResult.No Then
                    txtPembetulan.Focus()
                End If
            End If
            DialogResult = Windows.Forms.DialogResult.OK
            Me.Close()
        Catch ex As Exception
            XtraMessageBox.Show("Info Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
        Finally
            dir = Nothing
        End Try
    End Sub
End Class