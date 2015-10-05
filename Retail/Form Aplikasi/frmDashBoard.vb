Imports System.IO
Imports DevExpress.XtraEditors

Public Class frmDashBoard
    Dim FileGambar As String = Application.StartupPath & "\System\Image\BG.jpg"

    Public Sub TampilkanLinkBerdasarkanUser()
        Dim SQL As String = ""
        Dim IDMenu1 As Long = -1
        Dim IDMenu2 As Long = -1
        Dim IDMenu3 As Long = -1
        Dim IDMenu4 As Long = -1
        Dim ds As New DataSet
        Try
            ds = ExecuteDataset("MDashBoard", "SELECT * FROM MUser WHERE NoID=" & IDUserAktif)
            If Not ds.Tables("MDashBoard") Is Nothing AndAlso ds.Tables(0).Rows.Count >= 1 Then
                IDMenu1 = NullToLong(ds.Tables("MDashBoard").Rows(0).Item("IDDashBoard1"))
                IDMenu2 = NullToLong(ds.Tables("MDashBoard").Rows(0).Item("IDDashBoard2"))
                IDMenu3 = NullToLong(ds.Tables("MDashBoard").Rows(0).Item("IDDashBoard3"))
                IDMenu4 = NullToLong(ds.Tables("MDashBoard").Rows(0).Item("IDDashBoard4"))
            Else
                IDMenu1 = -1
                IDMenu2 = -1
                IDMenu3 = -1
                IDMenu4 = -1
            End If

            SQL = "SELECT MMenu.*" & vbCrLf & _
                  " FROM MMenu " & vbCrLf & _
                  " WHERE MMenu.NoID=" & IDMenu1 & " AND MMenu.isactive = 1 And IsNull(MMenu.IsBarSubItem, 0) = 0" & vbCrLf & _
                  " AND MMenu.noid NOT IN (SELECT A.idparent FROM MMenu A)" & vbCrLf & _
                  " AND MMenu.noid IN (SELECT MUserD.IDMenu FROM MUser INNER JOIN MUserD ON MUser.NoID=MUserD.IDUser WHERE MUserD.[Enable]=1 AND MUserD.[Visible]=1 AND MUser.NoID=31)"
            ds = ExecuteDataset("MDashBoard", SQL)
            If ds.Tables(0).Rows.Count >= 1 Then
                BUTTON1.Tag = NullToLong(ds.Tables(0).Rows(0).Item("NoID"))
                BUTTON1.Text = NullToStr(ds.Tables(0).Rows(0).Item("Caption"))
            Else
                BUTTON1.Tag = ""
                BUTTON1.Text = ""
            End If

            SQL = "SELECT MMenu.*" & vbCrLf & _
                  " FROM MMenu " & vbCrLf & _
                  " WHERE MMenu.NoID=" & IDMenu2 & " AND MMenu.isactive = 1 And IsNull(MMenu.IsBarSubItem, 0) = 0" & vbCrLf & _
                  " AND MMenu.noid NOT IN (SELECT A.idparent FROM MMenu A)" & vbCrLf & _
                  " AND MMenu.noid IN (SELECT MUserD.IDMenu FROM MUser INNER JOIN MUserD ON MUser.NoID=MUserD.IDUser WHERE MUserD.[Enable]=1 AND MUserD.[Visible]=1 AND MUser.NoID=31)"
            ds = ExecuteDataset("MDashBoard", SQL)
            If ds.Tables(0).Rows.Count >= 1 Then
                BUTTON2.Tag = NullToLong(ds.Tables(0).Rows(0).Item("NoID"))
                BUTTON2.Text = NullToStr(ds.Tables(0).Rows(0).Item("Caption"))
            Else
                BUTTON2.Tag = ""
                BUTTON2.Text = ""
            End If

            SQL = "SELECT MMenu.*" & vbCrLf & _
                  " FROM MMenu " & vbCrLf & _
                  " WHERE MMenu.NoID=" & IDMenu3 & " AND MMenu.isactive = 1 And IsNull(MMenu.IsBarSubItem, 0) = 0" & vbCrLf & _
                  " AND MMenu.noid NOT IN (SELECT A.idparent FROM MMenu A)" & vbCrLf & _
                  " AND MMenu.noid IN (SELECT MUserD.IDMenu FROM MUser INNER JOIN MUserD ON MUser.NoID=MUserD.IDUser WHERE MUserD.[Enable]=1 AND MUserD.[Visible]=1 AND MUser.NoID=31)"
            ds = ExecuteDataset("MDashBoard", SQL)
            If ds.Tables(0).Rows.Count >= 1 Then
                BUTTON3.Tag = NullToLong(ds.Tables(0).Rows(0).Item("NoID"))
                BUTTON3.Text = NullToStr(ds.Tables(0).Rows(0).Item("Caption"))
            Else
                BUTTON3.Tag = ""
                BUTTON3.Text = ""
            End If

            SQL = "SELECT MMenu.*" & vbCrLf & _
                  " FROM MMenu " & vbCrLf & _
                  " WHERE MMenu.NoID=" & IDMenu4 & " AND MMenu.isactive = 1 And IsNull(MMenu.IsBarSubItem, 0) = 0" & vbCrLf & _
                  " AND MMenu.noid NOT IN (SELECT A.idparent FROM MMenu A)" & vbCrLf & _
                  " AND MMenu.noid IN (SELECT MUserD.IDMenu FROM MUser INNER JOIN MUserD ON MUser.NoID=MUserD.IDUser WHERE MUserD.[Enable]=1 AND MUserD.[Visible]=1 AND MUser.NoID=31)"
            ds = ExecuteDataset("MDashBoard", SQL)
            If ds.Tables(0).Rows.Count >= 1 Then
                BUTTON4.Tag = NullToLong(ds.Tables(0).Rows(0).Item("NoID"))
                BUTTON4.Text = NullToStr(ds.Tables(0).Rows(0).Item("Caption"))
            Else
                BUTTON4.Tag = ""
                BUTTON4.Text = ""
            End If

            If BUTTON1.Tag.ToString = "" Then
                BUTTON1.Enabled = False
                BUTTON1.Text = ""
            Else
                BUTTON1.Enabled = True
            End If
            If BUTTON2.Tag.ToString = "" Then
                BUTTON2.Enabled = False
                BUTTON2.Text = ""
            Else
                BUTTON2.Enabled = True
            End If
            If BUTTON3.Tag.ToString = "" Then
                BUTTON3.Enabled = False
                BUTTON3.Text = ""
            Else
                BUTTON3.Enabled = True
            End If
            If BUTTON4.Tag.ToString = "" Then
                BUTTON4.Enabled = False
                BUTTON4.Text = ""
            Else
                BUTTON4.Enabled = True
            End If
        Catch ex As Exception
            XtraMessageBox.Show("Info Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
        Finally
            ds.Dispose()
        End Try
    End Sub

    Private Sub frmDashBoard_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        FungsiControl.SetForm(Me)
        FileGambar = Ini.BacaIni("Application", "Background", Application.StartupPath & "\System\Image\BG.Jpg")
        If File.Exists(FileGambar) Then
            Me.BackgroundImage = Image.FromFile(FileGambar)
            Me.BackgroundImageLayout = ImageLayout.Stretch
            Me.PictureEdit1.Image = Image.FromFile(FileGambar)
            Me.PictureEdit1.Properties.SizeMode = DevExpress.XtraEditors.Controls.PictureSizeMode.Squeeze
        Else
            If File.Exists(Application.StartupPath & "\System\Image\BG.Jpg") Then
                Me.BackgroundImage = Image.FromFile(FileGambar)
                Me.BackgroundImageLayout = ImageLayout.Stretch
                Me.PictureEdit1.Image = Image.FromFile(FileGambar)
                Me.PictureEdit1.Properties.SizeMode = DevExpress.XtraEditors.Controls.PictureSizeMode.Squeeze
            End If
        End If
        If File.Exists(FolderLayouts & Me.Name & LayoutControl1.Name & ".xml") Then
            LayoutControl1.RestoreLayoutFromXml(FolderLayouts & Me.Name & LayoutControl1.Name & ".xml")
        End If
        If IsLogin Then
            TampilkanLinkBerdasarkanUser()
        End If
        LayoutControl1.BackColor = Color.Transparent
        PanelControl1.BackColor = Color.Transparent
    End Sub

    Private Sub mnSaveLayouts_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles mnSaveLayouts.ItemClick
        Dim xOtorisasi As New frmOtorisasiAdmin
        If XtraMessageBox.Show("Simpan Layout?", NamaAplikasi, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = Windows.Forms.DialogResult.Yes AndAlso xOtorisasi.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
            Ini.TulisIniPath(FolderLayouts & Me.Name & ".ini", "Form", "Width", Me.Width)
            Ini.TulisIniPath(FolderLayouts & Me.Name & ".ini", "Form", "Height", Me.Height)
            LayoutControl1.SaveLayoutToXml(FolderLayouts & Me.Name & LayoutControl1.Name & ".xml")
        End If
        xOtorisasi.Dispose()
    End Sub

    Private Sub mnCustomLayouts_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles mnCustomLayouts.ItemClick
        If IsEditLayout Then
            LayoutControl1.ShowCustomizationForm()
        End If
    End Sub

    Private Sub BUTTON1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BUTTON1.Click
        Dim str As String = sender.tag
        Try
            If IsLogin Then
                str = NullToStr(EksekusiSQlSkalarNew("SELECT Kode FROM MMenu WHERE NoID=" & NullToLong(sender.tag)))
                TryCast(CType(Me.MdiParent, frmMain).BarManager1.Items(str), DevExpress.XtraBars.BarButtonItem).PerformClick()
            End If
        Catch ex As Exception

        End Try
    End Sub

    Private Sub BUTTON2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BUTTON2.Click
        Dim str As String = sender.tag
        Try
            If IsLogin Then
                str = NullToStr(EksekusiSQlSkalarNew("SELECT Kode FROM MMenu WHERE NoID=" & NullToLong(sender.tag)))
                TryCast(CType(Me.MdiParent, frmMain).BarManager1.Items(str), DevExpress.XtraBars.BarButtonItem).PerformClick()
            End If
        Catch ex As Exception

        End Try
    End Sub

    Private Sub BUTTON3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BUTTON3.Click
        Dim str As String = sender.tag
        Try
            If IsLogin Then
                str = NullToStr(EksekusiSQlSkalarNew("SELECT Kode FROM MMenu WHERE NoID=" & NullToLong(sender.tag)))
                TryCast(CType(Me.MdiParent, frmMain).BarManager1.Items(str), DevExpress.XtraBars.BarButtonItem).PerformClick()
            End If
        Catch ex As Exception

        End Try
    End Sub

    Private Sub BUTTON4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BUTTON4.Click
        Dim str As String = sender.tag
        Try
            If IsLogin Then
                str = NullToStr(EksekusiSQlSkalarNew("SELECT Kode FROM MMenu WHERE NoID=" & NullToLong(sender.tag)))
                TryCast(CType(Me.MdiParent, frmMain).BarManager1.Items(str), DevExpress.XtraBars.BarButtonItem).PerformClick()
            End If
        Catch ex As Exception

        End Try
    End Sub
End Class