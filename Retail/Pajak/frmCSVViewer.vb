Public Class frmCSVViewer

    Private Sub ButtonEdit1_ButtonClick(ByVal sender As Object, ByVal e As DevExpress.XtraEditors.Controls.ButtonPressedEventArgs) Handles ButtonEdit1.ButtonClick
        Select Case e.Button.Index
            Case 0
                Dim x As New OpenFileDialog
                x.Filter = "CSV Files|*.csv"
                x.Title = "CSV Viewer"
                If x.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
                    ButtonEdit1.Text = x.FileName
                End If
                x.Dispose()
            Case 1
                RefreshData()
        End Select
    End Sub
    Private Sub RefreshData()
        Dim strKon As String = ""
        Dim SQL As String = ""
        Dim cn As New OleDb.OleDbConnection
        Dim com As New OleDb.OleDbCommand
        Dim ODa As New OleDb.OleDbDataAdapter
        Dim ds As New DataSet
        Dim fl As System.IO.FileInfo = Nothing
        Try
            If System.IO.File.Exists(ButtonEdit1.Text) Then
                fl = New System.IO.FileInfo(ButtonEdit1.Text)
                'strKon = "Data Source='" & fl.DirectoryName.ToString & "';Delimiter=';';Has Quotes=True;Skip Rows=0;Has Header=True;Comment Prefix='';Column Type=String,String,String,Int32,Boolean,String,String;Trim Spaces=False;Ignore Empty Lines=True;"
                'strKon = "Driver={Microsoft Text Driver (*.txt; *.csv)};Dbq='" & fl.DirectoryName.ToString & "';" & _
                '         "Extensions=asc,csv,tab,txt;"
                'strKon = "Dsn=SE;uid=;pwd="
                TulisSettinganIni(fl.DirectoryName.ToString, fl.Name)
                strKon = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source='" & fl.DirectoryName.ToString & "';Extended Properties='Text;HRD=Yes;FMT=Delimited(;)'"
                cn.ConnectionString = strKon
                cn.Open()
                com.Connection = cn
                ODa.SelectCommand = com
                SQL = "SELECT * FROM [" & fl.Name & "]"
                com.CommandText = SQL
                If Not ds.Tables("DATA") Is Nothing Then
                    ds.Tables("DATA").Clear()
                End If
                ODa.Fill(ds, "DATA")
            End If
            If Not ds.Tables("DATA") Is Nothing Then
                GridControl1.DataSource = ds.Tables("DATA")
            End If
            GridView1.PopulateColumns()
            HapusIni(fl.DirectoryName.ToString)
        Catch ex As Exception
            DevExpress.XtraEditors.XtraMessageBox.Show("Info Kesalahan : " & ex.Message, "CSV Viewer", MessageBoxButtons.OK, MessageBoxIcon.Information)
        Finally
            ds.Dispose()
            ODa.Dispose()
            com.Dispose()
            If cn.State = ConnectionState.Open Then
                cn.Close()
            End If
            cn.Dispose()
        End Try
    End Sub
    Private Sub RefreshData2()
        Dim strKon As String = ""
        Dim SQL As String = ""
        Dim cn As New OleDb.OleDbConnection
        Dim com As New OleDb.OleDbCommand
        Dim ODa As New OleDb.OleDbDataAdapter
        Dim ds As New DataSet
        Dim fl As System.IO.FileInfo = Nothing
        Try
            If System.IO.File.Exists(ButtonEdit2.Text) Then
                fl = New System.IO.FileInfo(ButtonEdit2.Text)
                'strKon = "Data Source='" & fl.DirectoryName.ToString & "';Delimiter=';';Has Quotes=True;Skip Rows=0;Has Header=True;Comment Prefix='';Column Type=String,String,String,Int32,Boolean,String,String;Trim Spaces=False;Ignore Empty Lines=True;"
                'strKon = "Driver={Microsoft Text Driver (*.txt; *.csv)};Dbq='" & fl.DirectoryName.ToString & "';" & _
                '         "Extensions=asc,csv,tab,txt;"
                'strKon = "Dsn=SE;uid=;pwd="
                TulisSettinganIni(fl.DirectoryName.ToString, fl.Name)
                strKon = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source='" & fl.DirectoryName.ToString & "';Extended Properties='Text;HRD=Yes;FMT=Delimited(;)'"
                cn.ConnectionString = strKon
                cn.Open()
                com.Connection = cn
                ODa.SelectCommand = com
                SQL = "SELECT * FROM [" & fl.Name & "]"
                com.CommandText = SQL
                If Not ds.Tables("DATA") Is Nothing Then
                    ds.Tables("DATA").Clear()
                End If
                ODa.Fill(ds, "DATA")
            End If
            If Not ds.Tables("DATA") Is Nothing Then
                GridControl2.DataSource = ds.Tables("DATA")
            End If
            GridView2.PopulateColumns()
            HapusIni(fl.DirectoryName.ToString)
        Catch ex As Exception
            DevExpress.XtraEditors.XtraMessageBox.Show("Info Kesalahan : " & ex.Message, "CSV Viewer", MessageBoxButtons.OK, MessageBoxIcon.Information)
        Finally
            ds.Dispose()
            ODa.Dispose()
            com.Dispose()
            If cn.State = ConnectionState.Open Then
                cn.Close()
            End If
            cn.Dispose()
        End Try
    End Sub
    Private Sub HapusIni(ByVal Path As String)
        Dim iniFile As String = Path & "\Schema.ini"
        Try
            If System.IO.File.Exists(iniFile) Then
                System.IO.File.Delete(iniFile)
            End If
        Catch ex As Exception

        End Try
    End Sub
    Private Sub TulisSettinganIni(ByVal Path As String, ByVal FileName As String)
        Dim iniFile As String = Path & "\Schema.ini"
        Try
            Ini.TulisIniPath(iniFile, FileName, "ColNameHeader", "True")
            Ini.TulisIniPath(iniFile, FileName, "Format", "Delimited(;)")
            Ini.TulisIniPath(iniFile, FileName, "MaxScanRows", "25")
            Ini.TulisIniPath(iniFile, FileName, "CharacterSet", "ANSI")
        Catch ex As Exception

        End Try
    End Sub
    Private Sub ButtonEdit1_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonEdit1.EditValueChanged

    End Sub

    Private Sub ButtonEdit1_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles ButtonEdit1.KeyDown

    End Sub

    Private Sub frmCSVViewer_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            FungsiControl.SetForm(Me)
        Catch ex As Exception

        End Try
    End Sub

    Private Sub ButtonEdit2_ButtonClick(ByVal sender As Object, ByVal e As DevExpress.XtraEditors.Controls.ButtonPressedEventArgs) Handles ButtonEdit2.ButtonClick
        Select Case e.Button.Index
            Case 0
                Dim x As New OpenFileDialog
                x.Filter = "CSV Files|*.csv"
                x.Title = "CSV Viewer"
                If x.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
                    ButtonEdit2.Text = x.FileName
                End If
                x.Dispose()
            Case 1
                RefreshData2()
        End Select
    End Sub

    Private Sub ButtonEdit2_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonEdit2.EditValueChanged

    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Dim PPN1 As Double = 0.0
        Dim PPN2 As Double = 0.0
        For i = 0 To GridView1.RowCount - 1
            PPN1 = NullToDbl(GridView1.GetRowCellValue(i, "PPN"))
            PPN2 = NullToDbl(GridView2.GetRowCellValue(i, "PPN"))

            If PPN1 <> PPN2 Then
                MessageBox.Show("Lihat " & GridView1.GetRowCellValue(i, "Nomor Faktur / Dokumen"))
            End If
        Next
    End Sub
End Class