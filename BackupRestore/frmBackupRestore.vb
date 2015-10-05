Imports System.Data
Imports System.Data.SqlClient
Imports System.Windows.Forms
Imports BackupRestore.Ini

Public Class frmBackupRestore
    Dim appIni As String = Application.StartupPath & "\Setting.ini"
    Private Sub frmBackupRestore_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            txtServer.Text = Ini.BacaIniPath(appIni, "DBConfig", "Server", "(local)\SQLEXPRESS")
            txtUID.Text = Ini.BacaIniPath(appIni, "DBConfig", "UID", "sa")
            txtPwd.Text = Ini.BacaIniPath(appIni, "DBConfig", "PWD", "sgi")
            CekKoneksi()
            cmbDatabase.Text = Ini.BacaIniPath(appIni, "DBConfig", "Database", "Hartani")
        Catch ex As Exception
            MessageBox.Show("Info Kesalahan : " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End Try
    End Sub

    Private Sub txtServer_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtServer.TextChanged

    End Sub

    Private Sub txtUID_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtUID.TextChanged

    End Sub

    Private Sub txtPwd_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtPwd.KeyDown
        If e.KeyCode = Keys.Enter Then
            CekKoneksi()
        End If
    End Sub

    Private Sub txtPwd_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtPwd.TextChanged

    End Sub

    Private Sub CekKoneksi()
        Dim con As New SqlConnection
        Dim com As New SqlCommand
        Dim oDA As New SqlDataAdapter
        Dim ds As New DataSet
        Try
            con.ConnectionString = "Data Source=" & txtServer.Text & _
                                   ";initial Catalog=master" & _
                                   ";User ID=" & txtUID.Text & _
                                   ";Password=" & txtPwd.Text & _
                                   ";Connect Timeout=30"
            con.Open()
            Ini.TulisIniPath(appIni, "DBConfig", "Server", txtServer.Text)
            Ini.TulisIniPath(appIni, "DBConfig", "UID", txtUID.Text)
            Ini.TulisIniPath(appIni, "DBConfig", "PWD", txtPwd.Text)

            com.Connection = con
            oDA.SelectCommand = com
            com.CommandText = "SELECT * FROM sysdatabases"
            oDA.Fill(ds, "Data")
            cmbDatabase.Items.Clear()
            For i As Integer = 0 To ds.Tables("Data").Rows.Count - 1
                cmbDatabase.Items.Add(ds.Tables("Data").Rows(i).Item(0))
            Next
            If cmbDatabase.Items.Count >= 1 Then
                cmbDatabase.Text = Ini.BacaIniPath(appIni, "DBConfig", "Database", "Hartani")
            End If
        Catch ex As Exception
            MessageBox.Show("Info Kesalahan : " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Information)
            cmbDatabase.Items.Clear()
            cmbDatabase.Text = ""
        Finally
            If con.State = ConnectionState.Open Then
                con.Close()
            End If
            con.Dispose()
            com.Dispose()
            oDA.Dispose()
            ds.Dispose()
        End Try
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Dim x As New SaveFileDialog
        x.Filter = "WinRar Archive|*.rar"
        x.Title = "Backup Database " & cmbDatabase.Text.ToUpper
        If cmbDatabase.Items.Count >= 1 AndAlso x.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
            Ini.TulisIniPath(appIni, "DBConfig", "Database", cmbDatabase.Text)
            BackupDatabaseManual(x.FileName)
        End If
    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        Dim x As System.Windows.Forms.DialogResult = MessageBox.Show("Dalam proses restore data dapat terjadi kesalahan." & "Untuk menjaga data sebelumnya akan dilakukan backup secara otomatis. Ya : Membackup data sebelumnya, No : Tidak membeckup, Cancel : Membatalkan", "Warning !!!", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button3)
        If x = Windows.Forms.DialogResult.Yes Then
            Button1.PerformClick()
            MessageBox.Show("Tunggu sampai selesai, dan clik restore lalu pilih No Backup.", "Warning !!!", MessageBoxButtons.OK, MessageBoxIcon.Information)
        ElseIf x = Windows.Forms.DialogResult.No Then
            Ini.TulisIniPath(appIni, "DBConfig", "Database", cmbDatabase.Text)
            Restore()
        End If
    End Sub

    Private Sub Restore()
        'MessageBox.Show("Untuk Restore Menyusul ya.", "AUTOR : YH", MessageBoxButtons.OK, MessageBoxIcon.Stop)
        Dim x As New OpenFileDialog
        Dim FileBak As System.IO.FileInfo = Nothing
        Try
            If Not System.IO.Directory.Exists(txtPathDB.Text) Then
                MessageBox.Show("Info Kesalahan : Path Database belum ditemukan.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information)
            ElseIf cmbDatabase.Items.Count >= 1 AndAlso System.IO.Directory.Exists(txtPathDB.Text) Then
                x.Filter = "BAK File|*.bak"
                x.Title = "Restore Database " & cmbDatabase.Text.ToUpper
                If x.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
                    FileBak = New System.IO.FileInfo(x.FileName)
                    If FileBak.Extension.ToUpper = ".RAR" Then
                        RestoreDatabaseManual(x.FileName, txtPathDB.Text, FileBak.Name.ToUpper.Replace(".RAR", ".BAK").Replace("AUTO_", ""), True)
                    ElseIf FileBak.Extension.ToUpper = ".BAK" Then
                        RestoreDatabaseManual(x.FileName, txtPathDB.Text, FileBak.Name.ToUpper.Replace("AUTO_", ""), False)
                    End If
                End If
            End If
        Catch ex As Exception
            MessageBox.Show("Info Kesalahan : " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Information)
        Finally
            x.Dispose()
        End Try
    End Sub

    Private Sub cmdBrowseDir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdBrowseDir.Click
        Dim x As New FolderBrowserDialog
        x.ShowNewFolderButton = True
        If x.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
            txtPathDB.Text = x.SelectedPath
            'Else
            '    txtPathDB.Text = ""
        End If
        x.Dispose()
    End Sub
End Class