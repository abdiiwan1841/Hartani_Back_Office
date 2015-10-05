Public Class frmCetakXtraReport
    Private Sub frmCetakMDI_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Activated
        Label2.Text = Me.Text
    End Sub
    Private Sub frmCetakMDI_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        LayoutControl1.SaveLayoutToXml(folderLayouts & Me.Name & ".xml")
    End Sub

    Private Sub frmCetakMDI_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        SetCtlMe()
        FungsiControl.SetForm(Me)

    End Sub
    Private Sub SetCtlMe()
        Label2.Text = Me.Text
        If System.IO.File.Exists(folderLayouts & Me.Name & ".xml") Then
            LayoutControl1.RestoreLayoutFromXml(folderLayouts & Me.Name & ".xml")
        End If
    End Sub
End Class