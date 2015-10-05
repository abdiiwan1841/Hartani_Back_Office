Imports System.IO

Module mdlPublic
    Dim NamaServer As String = ""
    Dim NamaDB As String = ""
    Dim UID As String = ""
    Dim PWD As String = ""
    Dim BakFile As String = ""
    Dim RarDirectory As String = ""
    Public Sub Main()
        Try
            Application.EnableVisualStyles()
            Application.SetCompatibleTextRenderingDefault(False)
            Application.Run(New frmBackupRestore())
        Catch ex As Exception
            MessageBox.Show("Info Kesalahan : " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End Try
    End Sub

    Public Sub BackupDatabase()
        Dim FileText As String = ""
        Dim oWrite As StreamWriter = Nothing
        Try
            NamaServer = Ini.BacaIniPath(Application.StartupPath & "\Setting.ini", "DBConfig", "Server", "(local)\SQLEXPRESS")
            NamaDB = Ini.BacaIniPath(Application.StartupPath & "\Setting.ini", "DBConfig", "Database", "HARTANI")
            UID = Ini.BacaIniPath(Application.StartupPath & "\Setting.ini", "DBConfig", "UID", "sa")
            PWD = Ini.BacaIniPath(Application.StartupPath & "\Setting.ini", "DBConfig", "PWD", "sgi")
            RarDirectory = Ini.BacaIniPath(Application.StartupPath & "\Setting.ini", "Application", "Winrar Path", "C:\Program Files\WinRar\")
            BakFile = Today.ToString("ddMMyyyy")
            If Not IO.Directory.Exists(RarDirectory) Then
                RarDirectory = Ini.BacaIniPath(Application.StartupPath & "\Setting.ini", "Application", "Winrar Path", "C:\Program Files (x86)\WinRAR\")
            End If
            If Not System.IO.Directory.Exists(Application.StartupPath & "\AUTO BACKUP\") Then
                System.IO.Directory.CreateDirectory(Application.StartupPath & "\AUTO BACKUP\")
            End If
            FileText = Application.StartupPath & "\Backup.BAT"
            If System.IO.File.Exists(FileText) Then
                System.IO.File.Delete(FileText)
            End If
            oWrite = File.CreateText(FileText)
            oWrite.Write("SQLCMD -S """ & NamaServer & """ -U """ & UID & """ -P """ & PWD & """ " & _
                         " -Q ""DBCC SHRINKDATABASE(N'" & NamaDB & "' )""")
            oWrite.WriteLine()
            oWrite.Write("SQLCMD -S """ & NamaServer & """ -U """ & UID & """ -P """ & PWD & """ " & _
                         " -Q ""BACKUP DATABASE [" & NamaDB & "] TO  DISK = N'" & Application.StartupPath & "\AUTO_" & BakFile & ".BAK' WITH NOFORMAT, INIT, NAME = N'" & NamaDB & "-Full Database Backup', SKIP, NOREWIND, NOUNLOAD,  STATS = 10""")
            oWrite.WriteLine()
            If System.IO.File.Exists(RarDirectory & "\Winrar.exe") Then
                oWrite.Write("""" & RarDirectory & "\winrar.exe"" a -m5 -v500m -df -r """ & Application.StartupPath & "\AUTO BACKUP\" & BakFile & ".RAR"" """ & Application.StartupPath & "\AUTO_" & BakFile & ".BAK""")
            End If
            oWrite.WriteLine()
            oWrite.Write("Pause")
            oWrite.Close()

            'BackFeed
            Ini.TulisIniPath(Application.StartupPath & "\Setting.ini", "DBConfig", "Server", NamaServer)
            Ini.TulisIniPath(Application.StartupPath & "\Setting.ini", "DBConfig", "Database", NamaDB)
            Ini.TulisIniPath(Application.StartupPath & "\Setting.ini", "DBConfig", "UID", UID)
            Ini.TulisIniPath(Application.StartupPath & "\Setting.ini", "DBConfig", "PWD", PWD)
            Ini.TulisIniPath(Application.StartupPath & "\Setting.ini", "Application", "Winrar Path", RarDirectory)


            BukaFile(FileText)
        Catch ex As Exception
            MessageBox.Show("Info Kesalahan : " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Information)
        Finally
            If Not oWrite Is Nothing Then
                oWrite.Dispose()
            End If
        End Try
    End Sub
    Public Sub BackupDatabaseManual(ByVal NamaFile As String)
        Dim FileText As String = ""
        Dim oWrite As StreamWriter = Nothing
        Dim IsAdaRar As Boolean = False
        Try
            NamaServer = Ini.BacaIniPath(Application.StartupPath & "\Setting.ini", "DBConfig", "Server", "(local)\SQLEXPRESS")
            NamaDB = Ini.BacaIniPath(Application.StartupPath & "\Setting.ini", "DBConfig", "Database", "HARTANI")
            UID = Ini.BacaIniPath(Application.StartupPath & "\Setting.ini", "DBConfig", "UID", "sa")
            PWD = Ini.BacaIniPath(Application.StartupPath & "\Setting.ini", "DBConfig", "PWD", "sgi")
            RarDirectory = Ini.BacaIniPath(Application.StartupPath & "\Setting.ini", "Application", "Winrar Path", "C:\Program Files\WinRar\")
            BakFile = Today.ToString("ddMMyyyy")
            If Not IO.Directory.Exists(RarDirectory) Then
                RarDirectory = Ini.BacaIniPath(Application.StartupPath & "\Setting.ini", "Application", "Winrar Path", "C:\Program Files (x86)\WinRAR\")
            End If
            If Not System.IO.Directory.Exists(Application.StartupPath & "\AUTO BACKUP\") Then
                System.IO.Directory.CreateDirectory(Application.StartupPath & "\AUTO BACKUP\")
            End If
            FileText = Application.StartupPath & "\Backup.BAT"
            If System.IO.File.Exists(FileText) Then
                System.IO.File.Delete(FileText)
            End If
            oWrite = File.CreateText(FileText)
            oWrite.Write("SQLCMD -S """ & NamaServer & """ -U """ & UID & """ -P """ & PWD & """ " & _
                         " -Q ""DBCC SHRINKDATABASE(N'" & NamaDB & "' )""")
            oWrite.WriteLine()
            oWrite.Write("SQLCMD -S """ & NamaServer & """ -U """ & UID & """ -P """ & PWD & """ " & _
                         " -Q ""BACKUP DATABASE [" & NamaDB & "] TO  DISK = N'" & Application.StartupPath & "\AUTO_" & BakFile & ".BAK' WITH NOFORMAT, INIT, NAME = N'" & NamaDB & "-Full Database Backup', SKIP, NOREWIND, NOUNLOAD,  STATS = 10""")
            oWrite.WriteLine()
            If System.IO.File.Exists(RarDirectory & "\Winrar.exe") Then
                oWrite.Write("""" & RarDirectory & "\winrar.exe"" a -m5 -v500m -df -r ""AUTO BACKUP\" & BakFile & ".RAR"" ""AUTO_" & BakFile & ".BAK""")
                IsAdaRar = True
            Else
                IsAdaRar = False
            End If
            If IsAdaRar Then
                oWrite.WriteLine()
                'COPY "BACKUP DATA\.Rar" "APPAPP"
                oWrite.Write("COPY """ & Application.StartupPath & "\AUTO BACKUP\" & BakFile & ".RAR"" """ & NamaFile & """")
            Else
                oWrite.Write("COPY """ & Application.StartupPath & "\AUTO_" & BakFile & ".BAK"" """ & NamaFile.ToUpper.Replace(".RAR", ".BAK") & """")
            End If
            oWrite.WriteLine()
            oWrite.Write("Pause")
            oWrite.Close()

            'BackFeed
            Ini.TulisIniPath(Application.StartupPath & "\Setting.ini", "DBConfig", "Server", NamaServer)
            Ini.TulisIniPath(Application.StartupPath & "\Setting.ini", "DBConfig", "Database", NamaDB)
            Ini.TulisIniPath(Application.StartupPath & "\Setting.ini", "DBConfig", "UID", UID)
            Ini.TulisIniPath(Application.StartupPath & "\Setting.ini", "DBConfig", "PWD", PWD)
            Ini.TulisIniPath(Application.StartupPath & "\Setting.ini", "Application", "Winrar Path", RarDirectory)


            BukaFile(FileText)
        Catch ex As Exception
            MessageBox.Show("Info Kesalahan : " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Information)
        Finally
            If Not oWrite Is Nothing Then
                oWrite.Dispose()
            End If
        End Try
    End Sub
    Public Sub RestoreDatabaseManual(ByVal NamaFile As String, ByVal PathDB As String, ByVal FileBak As String, ByVal IsRar As Boolean)
        Dim FileText As String = ""
        Dim oWrite As StreamWriter = Nothing
        Dim IsAdaRar As Boolean = False
        Try
            NamaServer = Ini.BacaIniPath(Application.StartupPath & "\Setting.ini", "DBConfig", "Server", "(local)\SQLEXPRESS")
            NamaDB = Ini.BacaIniPath(Application.StartupPath & "\Setting.ini", "DBConfig", "Database", "HARTANI")
            UID = Ini.BacaIniPath(Application.StartupPath & "\Setting.ini", "DBConfig", "UID", "sa")
            PWD = Ini.BacaIniPath(Application.StartupPath & "\Setting.ini", "DBConfig", "PWD", "sgi")
            RarDirectory = Ini.BacaIniPath(Application.StartupPath & "\Setting.ini", "Application", "Winrar Path", "C:\Program Files\WinRar\")
            'BakFile = Today.ToString("ddMMyyyy")
            If Not IO.Directory.Exists(RarDirectory) Then
                RarDirectory = Ini.BacaIniPath(Application.StartupPath & "\Setting.ini", "Application", "Winrar Path", "C:\Program Files (x86)\WinRAR\")
            End If
            If Not System.IO.Directory.Exists(Application.StartupPath & "\AUTO BACKUP\") Then
                System.IO.Directory.CreateDirectory(Application.StartupPath & "\AUTO BACKUP\")
            End If
            FileText = Application.StartupPath & "\Restore.BAT"
            If System.IO.File.Exists(FileText) Then
                System.IO.File.Delete(FileText)
            End If
            oWrite = File.CreateText(FileText)
            If Not System.IO.File.Exists(NamaFile) Then
                MessageBox.Show("File Tidak ditemukan.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Exit Try
            End If
            If IsRar Then
                If System.IO.File.Exists(RarDirectory & "\Winrar.exe") Then
                    oWrite.Write("""" & RarDirectory & "\winrar.exe"" e """ & NamaFile & """ ""*.BAK""")
                    IsAdaRar = True
                Else
                    IsAdaRar = False
                End If
            End If
            oWrite.WriteLine()
            oWrite.Write("SQLCMD -S """ & NamaServer & """ -U """ & UID & """ -P """ & PWD & """ " & _
                         " -Q ""msdb.dbo.sp_delete_database_backuphistory @database_name = N'" & NamaDB & "'""")
            oWrite.WriteLine()
            oWrite.Write("SQLCMD -S """ & NamaServer & """ -U """ & UID & """ -P """ & PWD & """ " & _
                         " -Q ""DROP DATABASE [" & NamaDB & "]""")
            oWrite.WriteLine()
            If IsAdaRar Then
                oWrite.Write("SQLCMD -S """ & NamaServer & """ -U """ & UID & """ -P """ & PWD & """ " & _
                             " -Q ""RESTORE DATABASE [" & NamaDB & "] FROM  DISK = N'" & Application.StartupPath & "\" & FileBak & "' WITH  FILE = 1,  MOVE N'" & NamaDB & "_Data' TO N'" & PathDB & "\" & NamaDB & "_Data.mdf',  MOVE N'" & NamaDB & "_log' TO N'" & NamaDB & "\" & NamaDB & "_Log.LDF',  NOUNLOAD,  REPLACE,  STATS = 10""")
            Else
                oWrite.Write("SQLCMD -S """ & NamaServer & """ -U """ & UID & """ -P """ & PWD & """ " & _
                             " -Q ""RESTORE DATABASE [" & NamaDB & "] FROM  DISK = N'" & NamaFile & "' WITH  FILE = 1,  MOVE N'" & NamaDB & "_Data' TO N'" & PathDB & "\" & NamaDB & "_Data.mdf',  MOVE N'" & NamaDB & "_log' TO N'" & PathDB & "\" & NamaDB & "_Log.LDF',  NOUNLOAD,  REPLACE,  STATS = 10""")
            End If
            oWrite.WriteLine()

            oWrite.WriteLine()
            oWrite.Write("Pause")
            oWrite.Close()

            'BackFeed
            Ini.TulisIniPath(Application.StartupPath & "\Setting.ini", "DBConfig", "Server", NamaServer)
            Ini.TulisIniPath(Application.StartupPath & "\Setting.ini", "DBConfig", "Database", NamaDB)
            Ini.TulisIniPath(Application.StartupPath & "\Setting.ini", "DBConfig", "UID", UID)
            Ini.TulisIniPath(Application.StartupPath & "\Setting.ini", "DBConfig", "PWD", PWD)
            Ini.TulisIniPath(Application.StartupPath & "\Setting.ini", "Application", "Winrar Path", RarDirectory)


            BukaFile(FileText)
        Catch ex As Exception
            MessageBox.Show("Info Kesalahan : " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Information)
        Finally
            If Not oWrite Is Nothing Then
                oWrite.Dispose()
            End If
        End Try
    End Sub
    Public Sub BukaFile(ByVal nmfile As String)
        Try
            Dim p As New System.Diagnostics.ProcessStartInfo
            p.Verb = "Open"
            p.WindowStyle = ProcessWindowStyle.Normal
            p.FileName = nmfile
            p.UseShellExecute = True
            System.Diagnostics.Process.Start(p)
        Catch ex As Exception
            MessageBox.Show("Ada Kesalahan :" & vbCrLf & ex.Message & vbCrLf & "File : " & nmfile, "Error", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End Try
    End Sub
End Module
