Imports VPOINT.Function.mPublic
Imports System.Windows.Forms
Imports VPOINT.Function.Fungsi
Imports VPOINT.Serialshield.Serial

Public Class frmServiceSQLServer
    Dim isLast As Boolean, IsDate As Date, ods As DataSet
    Public x As New Databases.SQLServer()
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
        x.SetKoneksi()
        If SQLServer.isDatabaseConnected Then
            Dim SQL As String = String.Format("if not exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[MNotify]') and OBJECTPROPERTY(id, N'IsUserTable') = 1){0} CREATE TABLE [dbo].[MNotify] ({0} [Info] [varchar] (150) COLLATE Latin1_General_CI_AS NULL ,{0} [Notify] [text] COLLATE Latin1_General_CI_AS NULL ,{0} [IsTampil] [bit] NULL ,{0} [IDCPU] [varchar] (50) COLLATE Latin1_General_CI_AS NULL {0} ) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]", vbCrLf)
            x.EksekusiSQl(SQL)
        End If
    End Sub
    Private Sub frmService_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        SetDatabase()
        NotifyIcon1.BalloonTipText = String.Format("Selamat Datang di {0}{1}{1}This Application Supported {1}by : VPoint .Framework  1.2 ™", NamaApplikasi(), vbCrLf)
        Hide()
        NotifyIcon1.ShowBalloonTip(10)
        isLast = False
        Timer2.Enabled = True
    End Sub

    Private Sub NotifyIcon1_MouseMove(ByVal sender As Object, ByVal e As MouseEventArgs) Handles NotifyIcon1.MouseMove
        If Not isLast Then
            NotifyIcon1.ShowBalloonTip(10)
            IsDate = Now
            isLast = True
            Timer1.Enabled = True
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

    Private Sub Timer2_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer2.Tick
        NotifyIcon1.BalloonTipText = String.Format("Selamat Datang di {0}{1}{1}This Application Supported {1}by : VPoint .Framework  1.2 ™", NamaApplikasi(), vbCrLf)
        'If PakaiDatabase = IsDatabase.SQLServer Then
        'x.SetKoneksi()
        If Not SQLServer.isDatabaseConnected Then
            NotifyIcon1.ShowBalloonTip(10, "Information center", String.Format("Applikasi tidak terhubung dengan server.{0}Mohon cek koneksi ke server.", vbCrLf), Windows.Forms.ToolTipIcon.Warning)
            x.SetKoneksi()
        Else
            Try
                ods = New DataSet
                'ods = (x.ExecuteDataset("tbl", String.Format("Select * from MNotify WHere IDCPU='{0}' AND IsTampil<>1", EncryptText(IDCPU, "vpoint"))))
                ods = x.ExecuteDataset("tbl", String.Format("Select * from MNotify WHere IDCPU='{0}' AND IsTampil<>1", NullTostr(IPLokal)))
                'Dim strNotify As String = 
                If ods.Tables(0).Rows.Count >= 1 Then
                    NotifyIcon1.ShowBalloonTip(20, NullTostr(ods.Tables(0).Rows(0).Item("Info")), _
                    String.Format("{0}{1}{1}This Application Supported {1}by : VPoint .Framework  1.2 ™", NullTostr(ods.Tables(0).Rows(0).Item("Notify")), vbCrLf) _
                    , ToolTipIcon.Info)
                    x.EksekusiSQl(String.Format("Update MNotify Set IsTampil=1 WHERE IDCPU='{0}'", NullTostr(IPLokal)))
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
        If VPOINT.Function.Fungsi.FxMessage(String.Format("Anda yakin ingin keluar dari {0} ?", NamaApplikasi()), NamaApplikasi, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) = Windows.Forms.DialogResult.Yes Then
            Application.ExitThread()
            Application.Exit()
        End If
    End Sub

    Private Sub CalculatorToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CalculatorToolStripMenuItem.Click
        VPOINT.Function.Fungsi.SendKeys("CALC")
    End Sub

    Private Sub CommandPromptToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CommandPromptToolStripMenuItem.Click
        VPOINT.Function.Fungsi.SendKeys("CMD")
    End Sub

    Private Sub WindowsExplorerToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles WindowsExplorerToolStripMenuItem.Click
        VPOINT.Function.Fungsi.SendKeys("EXPLORER")
    End Sub

    Private Sub TaskManagerToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TaskManagerToolStripMenuItem.Click
        VPOINT.Function.Fungsi.SendKeys("TASKMGR")
    End Sub

    Private Sub NotepadToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NotepadToolStripMenuItem.Click
        VPOINT.Function.Fungsi.SendKeys("NOTEPAD")
    End Sub

    Private Sub OnScreenKeyboardToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OnScreenKeyboardToolStripMenuItem.Click
        VPOINT.Function.Fungsi.SendKeys("OSK")
    End Sub

    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()
        NotifyIcon1.BalloonTipText = String.Format("Selamat Datang di {0}{1}{1}This Application Supported {1}by : VPoint .Framework  1.2 ™", NamaApplikasi(), vbCrLf)
        ' Add any initialization after the InitializeComponent() call.

    End Sub
End Class