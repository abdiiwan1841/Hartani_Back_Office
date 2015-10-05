Imports System.Data
Imports System.Data.SqlClient
Imports DevExpress.XtraEditors

Public Class frmBackupDatabase
    Public Enum TypeDB
        Backup = 0
        Restore = 1
    End Enum
    Public tipe As TypeDB

    Private Sub txtKodeReff_ButtonClick(ByVal sender As Object, ByVal e As DevExpress.XtraEditors.Controls.ButtonPressedEventArgs) Handles txtKodeReff.ButtonClick
        If e.Button.Index = 0 Then
            If tipe = TypeDB.Backup Then
                Dim dlg As New SaveFileDialog
                Try
                    dlg.Title = "File Backup Databse"
                    dlg.Filter = "Database Backup Files|*.BAK"
                    If dlg.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
                        txtKodeReff.Text = dlg.FileName
                    End If
                Catch ex As Exception
                    XtraMessageBox.Show("Info Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
                End Try
            Else
                Dim dlg As New OpenFileDialog
                Try
                    dlg.Title = "File Restore Databse"
                    dlg.Filter = "Database Backup Files|*.BAK"
                    If dlg.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
                        txtKodeReff.Text = dlg.FileName
                    End If
                Catch ex As Exception
                    XtraMessageBox.Show("Info Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
                End Try
            End If
        End If
    End Sub

    Private Sub cmdTutup_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdTutup.Click
        DialogResult = Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub cmdSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSave.Click
        If tipe = TypeDB.Backup Then
            BackupDB()
        Else
            RestoreDB()
        End If
    End Sub

    Private Sub BackupDB()
        Dim filename As String, SQL As String
        Dim con As New SqlConnection
        Dim com As New SqlCommand
        Dim dlg As DevExpress.Utils.WaitDialogForm = Nothing
        Try
            If txtKodeReff.Text <> "" Then
                dlg = New DevExpress.Utils.WaitDialogForm("Sedang Membackup data ...", NamaAplikasi)
                dlg.Show()
                filename = txtKodeReff.Text
                SQL = "BACKUP DATABASE [" & Ini.BacaIni("DBConfig", "Database", "HARTANI") & "] TO  DISK = N'C:\DBPAA.BAK' WITH NOFORMAT, NOINIT,  NAME = N'" & Ini.BacaIni("DBConfig", "Database", "HARTANI") & "-Full Database Backup', SKIP, NOREWIND, NOUNLOAD,  STATS = 10"
                con.ConnectionString = "Data Source=" & Ini.BacaIni("dbconfig", "Server", "localhost") & _
                                       ";initial Catalog=" & Ini.BacaIni("dbconfig", "Database", "dbcityoys") & _
                                       ";Integrated Security=True;Connect Timeout=250"
                con.Open()
                com.Connection = con
                com.CommandText = SQL
                com.CommandTimeout = 250
                com.ExecuteNonQuery()
                System.IO.File.Copy("C:\DBPAA.BAK", filename, True)
                dlg.Close()
                DialogResult = Windows.Forms.DialogResult.OK
                Me.Close()
            End If
        Catch ex As Exception
            XtraMessageBox.Show("Info Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            con.Close()
            con.Dispose()
            com.Dispose()
            If Not dlg Is Nothing Then
                dlg.Dispose()
            End If
        End Try
    End Sub

    Private Sub RestoreDB()
        Dim filename As String, SQL As String
        Dim con As New SqlConnection
        Dim com As New SqlCommand
        Dim dlg As DevExpress.Utils.WaitDialogForm = Nothing
        Dim DriveDB As String = Application.StartupPath & "\DATABASE\"
        Try
            If Not System.IO.Directory.Exists(DriveDB) Then
                System.IO.Directory.CreateDirectory(DriveDB)
            End If
            If txtKodeReff.Text <> "" AndAlso System.IO.File.Exists(txtKodeReff.Text) Then
                dlg = New DevExpress.Utils.WaitDialogForm("Sedang Merestore data ...", NamaAplikasi)
                dlg.Show()
                filename = txtKodeReff.Text
                SQL = "RESTORE DATABASE [" & Ini.BacaIni("DBConfig", "Database", "HARTANI") & "] FROM  DISK = N'" & FixApostropi(filename) & "' WITH  FILE = 1,  MOVE N'" & Ini.BacaIni("DBConfig", "Database", "HARTANI") & "_Data'  TO N'" & DriveDB & "\" & Ini.BacaIni("DBConfig", "Database", "HARTANI") & "_Data.MDF',  MOVE N'" & Ini.BacaIni("DBConfig", "Database", "HARTANI") & "_Log' TO N'" & DriveDB & "\" & Ini.BacaIni("DBConfig", "Database", "HARTANI") & "_Log.LDF',  NOUNLOAD,  REPLACE,  STATS = 10"
                con.ConnectionString = "Data Source=" & Ini.BacaIni("dbconfig", "Server", "localhost") & _
                                       ";initial Catalog=" & Ini.BacaIni("dbconfig", "Database", "dbcityoys") & _
                                       ";Integrated Security=True;Connect Timeout=250"
                con.Open()
                com.Connection = con
                com.CommandText = SQL
                com.CommandTimeout = 250
                com.ExecuteNonQuery()
                dlg.Close()
                DialogResult = Windows.Forms.DialogResult.OK
                Me.Close()
            End If
        Catch ex As Exception
            XtraMessageBox.Show("Info Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            con.Close()
            con.Dispose()
            com.Dispose()
            If Not dlg Is Nothing Then
                dlg.Dispose()
            End If
        End Try
    End Sub

    Private Sub frmBackupDatabase_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        If tipe = TypeDB.Backup Then
            Me.Text = "Backup Database"
        Else
            Me.Text = "Restore Database"
        End If
    End Sub
End Class