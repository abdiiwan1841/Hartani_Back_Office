Imports DevExpress.XtraEditors

Public Class frmCetakMDI
    Dim DefImageList As New ImageList
    Private Sub frmCetakMDI_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Activated
        Label2.Text = Me.Text
    End Sub

    Private Sub frmCetakMDI_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        SetCtlMe()
        FungsiControl.SetForm(Me)

        DefImageList = frmMain.ImageList1

        cmdExport.ImageList = DefImageList
        cmdExport.ImageIndex = 11

        cmdPrint.ImageList = DefImageList
        cmdPrint.ImageIndex = 8

        cmdTutup.ImageList = DefImageList
        cmdTutup.ImageIndex = 3
    End Sub
    Private Sub SetCtlMe()
        Label2.Text = Me.Text
        If System.IO.File.Exists(FolderLayouts & Me.Name & ".xml") Then
            LayoutControl1.RestoreLayoutFromXml(FolderLayouts & Me.Name & ".xml")
        End If
    End Sub

    Private Sub cmdTutup_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdTutup.Click
        DialogResult = Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub cmdPreview_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdPrint.Click
        Try
            CrViewer.PrintReport()
        Catch ex As Exception

        End Try
    End Sub

    Private Sub cmdExcel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdExport.Click
        Try
            CrViewer.ExportReport()
        Catch ex As Exception

        End Try
    End Sub

    Private Sub mnExport_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles mnExport.ItemClick
        cmdExport.PerformClick()
    End Sub

    Private Sub mnPrint_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles mnPrint.ItemClick
        cmdPrint.PerformClick()
    End Sub

    Private Sub mnExit_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles mnExit.ItemClick
        cmdTutup.PerformClick()
    End Sub

    Private Sub mnSaveLayouts_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles mnSaveLayouts.ItemClick
        Dim xOtorisasi As New frmOtorisasiAdmin
        Try
            If XtraMessageBox.Show("Simpan Layout?", NamaAplikasi, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = Windows.Forms.DialogResult.Yes AndAlso xOtorisasi.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
                Ini.TulisIniPath(FolderLayouts & Me.Name & ".ini", "Form", "Width", Me.Width)
                Ini.TulisIniPath(FolderLayouts & Me.Name & ".ini", "Form", "Height", Me.Height)
                Me.Left = (Screen.PrimaryScreen.WorkingArea.Width - Me.Width) \ 2
                Me.Top = (Screen.PrimaryScreen.WorkingArea.Height - Me.Height) \ 2

                LayoutControl1.SaveLayoutToXml(FolderLayouts & Me.Name & ".xml")
            End If
        Catch ex As Exception
            XtraMessageBox.Show("Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            xOtorisasi.Dispose()
        End Try
    End Sub
End Class