Imports System.IO
Imports DevExpress.XtraEditors
Imports DevExpress.XtraEditors.Repository

Public Class frmExportDataKasir

    Dim repckedit As New RepositoryItemCheckEdit
    Dim repdateedit As New RepositoryItemDateEdit
    Dim reptextedit As New RepositoryItemTextEdit
    Dim reppicedit As New RepositoryItemPictureEdit

    Private Sub frmExportDataKasir_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        Try
            GV1.SaveLayoutToXml(FolderLayouts & "\" & Me.Name & GV1.Name & ".xml")
        Catch ex As Exception

        End Try
    End Sub

    Private Sub frmExportDataKasir_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        TglDari.DateTime = TanggalSystem
        txtFileName.Text = Application.StartupPath & "\"
        RefreshKassa()
        If File.Exists(FolderLayouts & "\" & Me.Name & GV1.Name & ".xml") Then
            GV1.RestoreLayoutFromXml(FolderLayouts & "\" & Me.Name & GV1.Name & ".xml")
        End If
    End Sub

    Private Sub RefreshKassa()
        Dim ds As New DataSet
        Dim SQL As String = ""
        Try
            SQL = "SELECT NoID, Kode, Nama, PathDBTemp, CONVERT(BIT, 1) AS [Pilih] FROM MPOS WHERE IsActive=1"
            ds = ExecuteDataset("MPOS", SQL)
            GC1.DataSource = ds.Tables("MPOS")
            With GV1
                For i As Integer = 0 To .Columns.Count - 1
                    Select Case .Columns(i).ColumnType.Name.ToLower
                        Case "int32", "int64", "int"
                            .Columns(i).DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
                            .Columns(i).DisplayFormat.FormatString = "n0"
                        Case "decimal", "single", "money", "double"
                            .Columns(i).DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
                            .Columns(i).DisplayFormat.FormatString = "n2"
                        Case "string"
                            .Columns(i).DisplayFormat.FormatType = DevExpress.Utils.FormatType.None
                            .Columns(i).DisplayFormat.FormatString = ""
                        Case "date", "datetime"
                            If .Columns(i).FieldName.Trim.ToLower = "jam" Then
                                .Columns(i).DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime
                                .Columns(i).DisplayFormat.FormatString = "HH:mm"
                            Else
                                .Columns(i).DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime
                                .Columns(i).DisplayFormat.FormatString = "dd-MM-yyyy"
                            End If
                        Case "byte[]"
                            reppicedit.SizeMode = DevExpress.XtraEditors.Controls.PictureSizeMode.Squeeze
                            .Columns(i).OptionsColumn.AllowGroup = False
                            .Columns(i).OptionsColumn.AllowSort = False
                            .Columns(i).OptionsFilter.AllowFilter = False
                            .Columns(i).ColumnEdit = reppicedit
                        Case "boolean"
                            .Columns(i).ColumnEdit = repckedit
                    End Select
                Next
            End With
        Catch ex As Exception
            XtraMessageBox.Show("Info Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
        Finally
            If Not ds Is Nothing Then
                ds.Dispose()
            End If
        End Try
    End Sub

    Private Sub txtFileName_ButtonClick(ByVal sender As Object, ByVal e As DevExpress.XtraEditors.Controls.ButtonPressedEventArgs) Handles txtFileName.ButtonClick
        Dim dlg As New FolderBrowserDialog
        Try
            If e.Button.Index = 0 Then
                If dlg.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
                    txtFileName.Text = dlg.SelectedPath
                End If
            End If
        Catch ex As Exception
            XtraMessageBox.Show("Info Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK)
        Finally
            dlg.Dispose()
        End Try
    End Sub

    Private Sub SimpleButton6_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SimpleButton6.Click
        Me.Close()
    End Sub

    Private Sub SimpleButton1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SimpleButton1.Click
        Dim FileDestination As String = "", FileSource As String = ""
        Try
            If XtraMessageBox.Show("Cek ulang jika benar, klik Yes/OK.", NamaAplikasi, MessageBoxButtons.YesNo, MessageBoxIcon.Warning) = Windows.Forms.DialogResult.Yes Then
                For i As Integer = 0 To GV1.RowCount - 1
                    If NullToBool(GV1.GetRowCellValue(i, "Pilih")) Then
                        FileSource = NullToStr(GV1.GetRowCellValue(i, "PathDBTemp")) & "\Database\TempDB_" & TglDari.DateTime.ToString("yyyyMM") & ".mdb"
                        If Not System.IO.Directory.Exists(txtFileName.Text & "\BACKUP_KASSA_" & TglDari.DateTime.ToString("MM_yyyy") & "\" & NullToStr(GV1.GetRowCellValue(i, "Kode")).ToUpper) Then
                            System.IO.Directory.CreateDirectory(txtFileName.Text & "\BACKUP_KASSA_" & TglDari.DateTime.ToString("MM_yyyy") & "\" & NullToStr(GV1.GetRowCellValue(i, "Kode")).ToUpper)
                        End If
                        FileDestination = txtFileName.Text & "\BACKUP_KASSA_" & TglDari.DateTime.ToString("MM_yyyy") & "\" & NullToStr(GV1.GetRowCellValue(i, "Kode")).ToUpper & "\TempDB_" & TglDari.DateTime.ToString("yyyyMM") & ".mdb"
                        Try
                            System.IO.File.Copy(FileSource, FileDestination, True)
                        Catch ex As Exception
                            XtraMessageBox.Show("Gagal mengcopy file " & NullToStr(GV1.GetRowCellValue(i, "Kode")) & " : " & FileSource, NamaAplikasi)
                        End Try
                    End If
                Next
                XtraMessageBox.Show("Berhasil mengcopy data.", NamaAplikasi)
            End If
        Catch ex As Exception
            XtraMessageBox.Show("Info Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
        End Try
    End Sub

    Private Sub GV1_FocusedColumnChanged(ByVal sender As Object, ByVal e As DevExpress.XtraGrid.Views.Base.FocusedColumnChangedEventArgs) Handles GV1.FocusedColumnChanged
        If e.FocusedColumn.Name.ToUpper = "Pilih".ToUpper Then
            GV1.OptionsBehavior.Editable = True
        Else
            GV1.OptionsBehavior.Editable = False
        End If
    End Sub
End Class