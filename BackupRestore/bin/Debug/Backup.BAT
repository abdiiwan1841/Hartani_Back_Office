SQLCMD -S "(local)" -U "sa" -P "sahasystem"  -Q "DBCC SHRINKDATABASE(N'AdResto1' )"
SQLCMD -S "(local)" -U "sa" -P "sahasystem"  -Q "BACKUP DATABASE [AdResto1] TO  DISK = N'F:\HARTANI\Atum30Juli2012\BackupRestore\bin\Debug\AUTO_12012013.BAK' WITH NOFORMAT, INIT, NAME = N'AdResto1-Full Database Backup', SKIP, NOREWIND, NOUNLOAD,  STATS = 10"
"C:\Program Files\WinRar\\winrar.exe" a -m5 -v500m -df -r "AUTO BACKUP\12012013.RAR" "AUTO_12012013.BAK"
COPY "F:\HARTANI\Atum30Juli2012\BackupRestore\bin\Debug\AUTO BACKUP\12012013.RAR" "C:\Users\Yanto\Videos\12012013.rar"
Pause