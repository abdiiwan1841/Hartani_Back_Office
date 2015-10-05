'Imports SGI.Function.mPublic
'Imports System.Windows.Forms
'Imports SGI.Function.Fungsi
'Imports SGI.Serialshield.Serial
Imports VPoint.Ini

Public Class frmServiceSQLServer
    Dim isLast As Boolean, IsDate As Date, ods As DataSet
    'Public x As New Databases.SQLServer()
    Private Sub frmService_FormClosing(ByVal sender As Object, ByVal e As FormClosingEventArgs) Handles Me.FormClosing
        DeleteIconFromTray()
    End Sub
    Public Sub DeleteIconFromTray()
        'Call Shell_NotifyIcon(DeleteIcon, Data)
        'NotifyIcon1.Dispose()
        NotifyIcon1.Visible = False
        'Application.Exit()
    End Sub
    Private Sub SetDatabase()
        If BacaIni("dbconfig", "Login windows", "1") = "1" Then 'Login windows
            StrKonSql = "Data Source=" & Ini.BacaIni("dbconfig", "Server", "localhost") & _
                        ";initial Catalog=" & BacaIni("dbconfig", "Database", "dbcityoys") & _
                        ";Integrated Security=True;Connect Timeout=" & BacaIni("dbconfig", "Timeout", "15")
        Else
            StrKonSql = "Data Source=" & BacaIni("dbconfig", "Server", "localhost") & _
                        ";initial Catalog=" & BacaIni("dbconfig", "Database", "dbcityoys") & _
                        ";User ID=" & BacaIni("dbconfig", "Username", "sa") & _
                        ";Password=" & BacaIni("dbconfig", "Password", "sahasystem") & ";Connect Timeout=" & BacaIni("dbconfig", "Timeout", "15")
        End If
        If isDatabaseConnected() Then
            Dim SQL As String = String.Format("if not exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[MNotify]') and OBJECTPROPERTY(id, N'IsUserTable') = 1){0} CREATE TABLE [dbo].[MNotify] ({0} [Info] [varchar] (150) COLLATE Latin1_General_CI_AS NULL ,{0} [Notify] [text] COLLATE Latin1_General_CI_AS NULL ,{0} [IsTampil] [bit] NULL ,{0} [IDCPU] [varchar] (50) COLLATE Latin1_General_CI_AS NULL {0} ) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]", vbCrLf)
            EksekusiSQL(SQL)
        End If
    End Sub
    Private Sub frmService_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'SetDatabase()
        NotifyIcon1.BalloonTipText = String.Format("Selamat Datang di {0}{1}{1}This Application Supported {1}by : SGI .Framework  1.2 ™", NamaAplikasi, vbCrLf)
        Hide()
        NotifyIcon1.ShowBalloonTip(10)
        isLast = False
        'Timer2.Enabled = True
        NotifyIcon1.Icon = frmMain.Icon
    End Sub

    Private Sub NotifyIcon1_MouseMove(ByVal sender As Object, ByVal e As MouseEventArgs) Handles NotifyIcon1.MouseMove
        If Not isLast Then
            NotifyIcon1.ShowBalloonTip(10)
            IsDate = Now
            isLast = True
            Timer1.Enabled = True
        Else
            NotifyIcon1.Visible = False
        End If
    End Sub

    Private Sub Timer1_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer1.Tick
        If DateDiff(DateInterval.Second, IsDate, Now) <= 5 Then
            isLast = True
        Else
            isLast = False
            Timer1.Enabled = False
        End If
    End Sub

    Private Sub Timer2_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs)
        NotifyIcon1.BalloonTipText = String.Format("Selamat Datang di {0}{1}{1}This Application Supported {1}by : SGI .Framework  1.2 ™", NamaAplikasi, vbCrLf)
        'If PakaiDatabase = IsDatabase.SQLServer Then
        'x.SetKoneksi()
        If Not isDatabaseConnected() Then
            NotifyIcon1.ShowBalloonTip(10, "Information center", String.Format("Applikasi tidak terhubung dengan server.{0}Mohon cek koneksi ke server.", vbCrLf), Windows.Forms.ToolTipIcon.Warning)
            'SetKoneksi()
        Else
            Try
                ods = New DataSet
                'ods = (x.ExecuteDataset("tbl", String.Format("Select * from MNotify WHere IDCPU='{0}' AND IsTampil<>1", EncryptText(IDCPU, "SGI"))))
                ods = ExecuteDataset("tbl", String.Format("Select * from MNotify WHere IDCPU='{0}' AND IsTampil<>1", NullToStr(IPLokal)))
                'Dim strNotify As String = 
                If ods.Tables(0).Rows.Count >= 1 Then
                    NotifyIcon1.ShowBalloonTip(20, NullToStr(ods.Tables(0).Rows(0).Item("Info")), _
                    String.Format("{0}{1}{1}This Application Supported {1}by : SGI .Framework  1.2 ™", NullToStr(ods.Tables(0).Rows(0).Item("Notify")), vbCrLf) _
                    , ToolTipIcon.Info)
                    EksekusiSQL(String.Format("Update MNotify Set IsTampil=1 WHERE IDCPU='{0}'", NullToStr(IPLokal)))
                End If
            Catch ex As Exception
                Application.DoEvents()
                Exit Sub
            End Try
        End If
        'End If
        Application.DoEvents()
    End Sub

    Private Sub ExitToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExitToolStripMenuItem.Click
        If DevExpress.XtraEditors.XtraMessageBox.Show("Anda yakin ingin keluar dari " & NamaAplikasi & " ?", NamaAplikasi, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) = Windows.Forms.DialogResult.Yes Then
            Application.ExitThread()
            Application.Exit()
        End If
    End Sub

    Private Sub CalculatorToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CalculatorToolStripMenuItem.Click
        FungsiControl.SendKeys("CALC")
    End Sub

    Private Sub CommandPromptToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CommandPromptToolStripMenuItem.Click
        FungsiControl.SendKeys("CMD")
    End Sub

    Private Sub WindowsExplorerToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles WindowsExplorerToolStripMenuItem.Click
        FungsiControl.SendKeys("EXPLORER")
    End Sub

    Private Sub TaskManagerToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TaskManagerToolStripMenuItem.Click
        FungsiControl.SendKeys("TASKMGR")
    End Sub

    Private Sub NotepadToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NotepadToolStripMenuItem.Click
        FungsiControl.SendKeys("NOTEPAD")
    End Sub

    Private Sub OnScreenKeyboardToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OnScreenKeyboardToolStripMenuItem.Click
        FungsiControl.SendKeys("OSK")
    End Sub

    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()
        NotifyIcon1.BalloonTipText = String.Format("Selamat Datang di {0}{1}{1}This Application Supported {1}by : SGI .Framework  1.2 ™", NamaAplikasi, vbCrLf)
        ' Add any initialization after the InitializeComponent() call.

    End Sub
End Class