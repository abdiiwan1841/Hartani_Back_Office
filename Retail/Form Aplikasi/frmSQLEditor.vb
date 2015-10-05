Imports System.Data
Imports System.Data.SqlClient
Imports System.Data.SQLite
Imports System.Data.OleDb
Imports DevExpress.XtraGrid
Imports DevExpress.XtraEditors
Imports DevExpress.XtraEditors.Repository

Public Class frmSQLEditor
    Dim strkoneksi As String = StrKonSql
    Dim con As SqlConnection = Nothing
    Dim com As SqlCommand = Nothing
    Dim oDA As SqlDataAdapter = Nothing
    Dim ds As DataSet = Nothing

    Dim repckedit As New RepositoryItemCheckEdit
    Dim repdateedit As New RepositoryItemDateEdit
    Dim reptextedit As New RepositoryItemTextEdit
    Dim reppicedit As New RepositoryItemPictureEdit

    Private Sub frmSQLEditor_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        Try
            If Not con Is Nothing Then
                con.Close()
                con.Dispose()
            End If
            If Not com Is Nothing Then
                com.Dispose()
            End If
            If Not oDA Is Nothing Then
                oDA.Dispose()
            End If
            If Not ds Is Nothing Then
                ds.Dispose()
            End If
            LayoutControl1.SaveLayoutToXml(FolderLayouts & "\" & Me.Name & LayoutControl1.Name & ".xml")

        Catch ex As Exception
            XtraMessageBox.Show("Info Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub frmSQLEditor_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If System.IO.File.Exists(FolderLayouts & "\" & Me.Name & LayoutControl1.Name & ".xml") Then
                LayoutControl1.RestoreLayoutFromXml(FolderLayouts & "\" & Me.Name & LayoutControl1.Name & ".xml")
            End If
            con = New SqlConnection(strkoneksi)
            con.Open()
            com = New SqlCommand
            com.Connection = con
            oDA = New SqlDataAdapter
            oDA.SelectCommand = com
            ds = New DataSet
        Catch ex As Exception
            XtraMessageBox.Show("Info Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub cmdSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSave.Click
        Dim SQL As String = txtSQL.SelectedText.ToLower
        Dim x As Integer = 0
        Try
            If txtSQL.SelectedText.Length > 0 Then
                SQL = txtSQL.SelectedText.ToLower
            Else
                SQL = txtSQL.Text.ToLower
            End If
            If SQL.Trim.Substring(0, 6).ToString.Replace(" ", "") = "SELECT".ToLower Then
                com.CommandText = SQL
                If Not ds.Tables("Data") Is Nothing Then
                    ds.Tables("Data").Clear()
                End If
                oDA.Fill(ds, "Data")
                GC1.DataSource = Nothing
                GC1.ResumeLayout()
                GV1.ClearColumnErrors()
                GV1.ClearColumnsFilter()
                GV1.ClearGrouping()
                GV1.ClearSorting()
                GV1.CloseEditor()
                GC1.DataSource = ds.Tables("Data")
                GC1.ResumeLayout()

                GV1.PopulateColumns()

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
                            Case "date"
                                .Columns(i).DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime
                                .Columns(i).DisplayFormat.FormatString = "dd-MM-yyyy"
                            Case "datetime"
                                .Columns(i).DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime
                                .Columns(i).DisplayFormat.FormatString = "dd-MM-yyyy HH:mm"
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
            ElseIf SQL.Trim.Substring(0, 6).ToString.Replace(" ", "") = "" Then

            Else
                com.CommandText = SQL
                x = com.ExecuteNonQuery()
                XtraMessageBox.Show("(" & x & " row(s) affected)", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
                GC1.DataSource = Nothing
                GC1.ResumeLayout()
                GV1.ClearColumnErrors()
                GV1.ClearColumnsFilter()
                GV1.ClearGrouping()
                GV1.ClearSorting()
                GV1.CloseEditor()
            End If
        Catch ex As Exception
            XtraMessageBox.Show("Info Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub SimpleButton1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SimpleButton1.Click
        DialogResult = Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub
End Class