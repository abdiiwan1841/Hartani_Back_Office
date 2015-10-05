Imports System.IO
Imports DevExpress.XtraEditors

Public Class frmOpsiDiscPromo

    'Private Sub txtFileName_ButtonClick(ByVal sender As Object, ByVal e As DevExpress.XtraEditors.Controls.ButtonPressedEventArgs)
    '    If e.Button.Index = 0 Then
    '        Dim dlg As New SaveFileDialog
    '        Try
    '            dlg.Title = "Export Data PPN Masukkan CSV"
    '            dlg.Filter = "CSV PPN Masukkan Files|*.csv"
    '            If dlg.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
    '                txtUserEdit.Text = dlg.FileName
    '            End If
    '        Catch ex As Exception
    '            XtraMessageBox.Show("Info Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
    '        Finally
    '            dlg.Dispose()
    '        End Try
    '    End If
    'End Sub

    Private Sub cmdBatal_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdBatal.Click
        DialogResult = Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub cmdSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSave.Click
        Try
            If DateDiff(DateInterval.Day, NullToDate(txtTglSampai.DateTime), NullToDate(txtTglDari.DateTime)) >= 1 Then
                XtraMessageBox.Show("Cek Tanggal Periode. Periode Salah.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Stop)
                txtTglSampai.Focus()
            End If
            If NullToDbl(txtDiscProsen.EditValue) <> 0 Or NullToDbl(txtDiscRp.EditValue) <> 0 Then
                DialogResult = Windows.Forms.DialogResult.OK
                Me.Close()
            Else
                DialogResult = Windows.Forms.DialogResult.OK
                Me.Close()
            End If
        Catch ex As Exception
            XtraMessageBox.Show("Info Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
        End Try
    End Sub

    Private Sub frmOpsiDiscPromo_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        txtTglDari.DateTime = TanggalSystem
        txtTglSampai.DateTime = TanggalSystem
        txtTglEdit.DateTime = TanggalSystem
        txtUserEdit.Text = NamaUserAktif
    End Sub

    Private Sub LabelControl4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LabelControl4.Click
        txtDiscProsen.Properties.ReadOnly = False
        txtDiscRp.Properties.ReadOnly = True
        txtDiscRp2.Properties.ReadOnly = True
        txtDiscRp.EditValue = 0
        txtDiscRp2.EditValue = 0
    End Sub

    Private Sub LabelControl1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LabelControl1.Click
        txtDiscRp2.Properties.ReadOnly = False
        txtDiscRp.Properties.ReadOnly = False
        txtDiscProsen.Properties.ReadOnly = True
        txtDiscProsen.EditValue = 0
    End Sub
End Class