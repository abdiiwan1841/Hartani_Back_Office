Imports DevExpress.XtraGrid
Imports System.Windows

Module ModPublik

    Public ReminderHutang As Boolean = False
    Public ShowReminder As Boolean = False
    Public TglUpdateReminder As Date = Today
    
    Public EditReport As Boolean = False
    Public Enum ExportTo
        Excel = 0
        Rtf = 1
        Pdf = 2
    End Enum

    'Public IsKhususAcc As Boolean = False

    Public Function LayOutKu(ByVal frm As String) As String
        Dim FolderLayout As String = Application.StartupPath ' VPOINT.Serialshield.Ini.BacaIni("Application", "LayoutSource", Application.StartupPath)
        If Not System.IO.Directory.Exists(FolderLayout) Then
            System.IO.Directory.CreateDirectory(FolderLayout)
        End If
        LayOutKu = FolderLayout & "\system\layouts\" & frm.ToString & ".xml"
    End Function
    Public Function FxMessage(ByVal msg As String, Optional ByVal title As String = "Admin Says", Optional ByVal btn As Windows.Forms.MessageBoxButtons = MessageBoxButtons.OK, _
    Optional ByVal ico As Windows.Forms.MessageBoxButtons = MessageBoxIcon.Information, Optional ByVal DefaultBtn As MessageBoxDefaultButton = MessageBoxDefaultButton.Button1, Optional ByVal CustomIcon As Icon = Nothing, Optional ByVal stackoverflow As String = "", Optional ByVal CreateLog As Boolean = False, Optional ByVal LogMessage As String = "") As Windows.Forms.DialogResult
        Try

            Return MsgBox(msg, btn + ico, title)

        Catch ex As Exception
            'VPOINT.Interface.ErrorLogger.WriteToErrorLog(ex.Message, ex.StackTrace, mPublic.NamaApplikasi)
        End Try
    End Function
    Public Sub ExportGrid(ByRef GridControl As GridControl, ByVal export As ExportTo)
        Using dlg As New SaveFileDialog()
            Dim x As String = ""
            Try
                dlg.Title = "Export file ke " & export.ToString
                If export = ExportTo.Excel Then
                    dlg.Filter = "Excel Files|*.xls"
                ElseIf export = ExportTo.Pdf Then
                    dlg.Filter = "Pdf Files|*.pdf"
                ElseIf export = ExportTo.Rtf Then
                    dlg.Filter = "Rtf Files|*.rtf"
                Else
                    Exit Sub
                End If
                dlg.FileName = GridControl.Parent.Name.ToString & "_" & export.ToString
                If dlg.ShowDialog = DialogResult.OK Then
                    x = dlg.FileName
                    If System.IO.File.Exists(x) Then System.IO.File.Delete(x)
                    If export = ExportTo.Excel Then
                        GridControl.DefaultView.ExportToXls(x)
                    ElseIf export = ExportTo.Pdf Then
                        GridControl.DefaultView.ExportToPdf(x)
                    ElseIf export = ExportTo.Rtf Then
                        GridControl.DefaultView.ExportToRtf(x)
                    Else
                        Exit Sub
                    End If
                    BukaFile(x)
                End If
            Catch ex As Exception
                FxMessage("Kesalahan : " & ex.Message, "Pesan:", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End Try
        End Using
    End Sub
End Module
