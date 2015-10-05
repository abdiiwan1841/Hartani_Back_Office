
SQLCMD -S "(local)" -U "sa" -P "sahasystem"  -Q "msdb.dbo.sp_delete_database_backuphistory @database_name = N'AdResto1'"
SQLCMD -S "(local)" -U "sa" -P "sahasystem"  -Q "DROP DATABASE [AdResto1]"
SQLCMD -S "(local)" -U "sa" -P "sahasystem"  -Q "RESTORE DATABASE [AdResto1] FROM  DISK = N'C:\Users\Yanto\Videos\AUTO_12012013.BAK' WITH  FILE = 1,  MOVE N'AdResto1_Data' TO N'D:\\AdResto1_Data.mdf',  MOVE N'AdResto1_log' TO N'D:\\AdResto1_Log.LDF',  NOUNLOAD,  REPLACE,  STATS = 10"

Pause