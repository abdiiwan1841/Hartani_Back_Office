Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Windows.Forms
Imports VPoint.Ini
Imports DevExpress.XtraEditors

Namespace SGI.Distributor
    Friend Class Program
        <STAThread()> _
        Shared Sub Main()
            Try
                If BacaIni("dbconfig", "Login windows", "1") = "1" Then 'Login windows
                    StrKonSql = "Data Source=" & BacaIni("dbconfig", "Server", "localhost") & _
                                ";initial Catalog=" & BacaIni("dbconfig", "Database", "dbcityoys") & _
                                ";Integrated Security=True;Connect Timeout=" & BacaIni("dbconfig", "Timeout", "15")
                Else
                    StrKonSql = "Data Source=" & BacaIni("dbconfig", "Server", "localhost") & _
                                ";initial Catalog=" & BacaIni("dbconfig", "Database", "dbcityoys") & _
                                ";User ID=" & BacaIni("dbconfig", "Username", "sa") & _
                                ";Password=" & BacaIni("dbconfig", "Password", "sahasystem") & ";Connect Timeout=" & BacaIni("dbconfig", "Timeout", "15")
                End If
                If BacaIni("dbconfig2", "Login windows", "1") = "1" Then 'Login windows
                    StrKonSqlServer2 = "Data Source=" & BacaIni("dbconfig2", "Server", "localhost") & _
                                       ";initial Catalog=" & BacaIni("dbconfig2", "Database", "dbcityoys") & _
                                       ";Integrated Security=True;Connect Timeout=" & BacaIni("dbconfig2", "Timeout", "15")
                Else
                    StrKonSqlServer2 = "Data Source=" & BacaIni("dbconfig2", "Server", "localhost") & _
                                       ";initial Catalog=" & BacaIni("dbconfig2", "Database", "dbcityoys") & _
                                       ";User ID=" & BacaIni("dbconfig2", "Username", "sa") & _
                                       ";Password=" & BacaIni("dbconfig2", "Password", "sahasystem") & ";Connect Timeout=" & BacaIni("dbconfig2", "Timeout", "15")
                End If

                DevExpress.UserSkins.OfficeSkins.Register()
                DevExpress.UserSkins.BonusSkins.Register()
                Application.EnableVisualStyles()
                Application.SetCompatibleTextRenderingDefault(False)
                Application.Run(New frmMain())

            Catch ex As Exception
                XtraMessageBox.Show("System error : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End Sub
    End Class
End Namespace
