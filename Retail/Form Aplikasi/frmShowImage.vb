Public Class frmShowImage 

    Private Sub frmShowImage_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        Ini.TulisIniPath(folderLayouts & "FotoBarang" & ".ini", "Form", "Width", Me.Width)
        Ini.TulisIniPath(folderLayouts & "FotoBarang" & ".ini", "Form", "Height", Me.Height)
        Ini.TulisIniPath(folderLayouts & "FotoBarang" & ".ini", "Form", "Left", Me.Left)
        Ini.TulisIniPath(folderLayouts & "FotoBarang" & ".ini", "Form", "Top", Me.Top)

        LayoutControl1.SaveLayoutToXml(folderLayouts & "FotoBarang" & ".xml")

    End Sub

    Private Sub frmShowImage_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        FungsiControl.SetForm(Me)
    End Sub
End Class