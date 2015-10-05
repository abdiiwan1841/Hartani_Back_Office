Imports System.Data.SQLite
Imports System.Data
Imports DevExpress.XtraEditors

Public Class frmSettingApplikasi
    Dim SQL As String
    Dim ds As New DataSet
    
    Dim strKonSQLite As String = "Data Source=" & Application.StartupPath & "\System\engine\syssgi.sqlite" & ";"
    Dim con As SQLiteConnection = Nothing
    Dim com As SQLiteCommand = Nothing
    Dim oDA As SQLiteDataAdapter = Nothing

    Private Sub SimpleButton6_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SimpleButton6.Click
        DialogResult = Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub SimpleButton9_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SimpleButton9.Click
        Try
            SQL = "DELETE FROM sysSettingSoftware"
            com.CommandText = SQL
            com.ExecuteNonQuery()

            SQL = "INSERT INTO sysSettingSoftware (NoID,SoftwareName,IDSoftware,IDSerial,IsPostingBukuJurnal,IsPostingKartuHutang)" & vbCrLf & _
                  " VALUES (1, '" & FixApostropi(NamaAplikasi) & "', '12345', '12345', " & NullToLong(rbPostingBukuJurnal.EditValue) & ", " & NullToLong(rbPostingKartuHutang.EditValue) & ")"
            com.CommandText = SQL
            If NullToLong(com.ExecuteNonQuery) >= 1 Then
                DialogResult = Windows.Forms.DialogResult.OK
                Me.Close()
            End If
        Catch ex As Exception
            DevExpress.XtraEditors.XtraMessageBox.Show("Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub frmSettingApplikasi_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        Try
            Ini.TulisIniPath(FolderLayouts & Me.Name & ".ini", "Form", "Width", Me.Width)
            Ini.TulisIniPath(FolderLayouts & Me.Name & ".ini", "Form", "Height", Me.Height)

            LayoutControl1.SaveLayoutToXml(FolderLayouts & Me.Name & LayoutControl1.Name & ".xml")

            con.Close()
            con.Dispose()
            com.Dispose()
            oDA.Dispose()
            ds.Dispose()
        Catch ex As Exception

        End Try
    End Sub

    Private Sub frmSettingPerusahaan_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            Me.Width = Ini.BacaIniPath(FolderLayouts & Me.Name & ".ini", "Form", "Width", Me.Width)
            Me.Height = Ini.BacaIniPath(FolderLayouts & Me.Name & ".ini", "Form", "Height", Me.Height)

            Me.Left = (Screen.PrimaryScreen.WorkingArea.Width - Me.Width) \ 2
            Me.Top = (Screen.PrimaryScreen.WorkingArea.Height - Me.Height) \ 2

            If System.IO.File.Exists(FolderLayouts & Me.Name & LayoutControl1.Name & ".xml") Then
                LayoutControl1.RestoreLayoutFromXml(FolderLayouts & Me.Name & LayoutControl1.Name & ".xml")
            End If

            If con Is Nothing Then
                con = New SQLiteConnection(strKonSQLite)
                con.Open()
                com = New SQLiteCommand
                com.Connection = con
                oDA = New SQLiteDataAdapter
                oDA.SelectCommand = com
            End If
            Try 'Create Table
                SQL = "CREATE TABLE sysSettingSoftware (" & _
                      " NoID INTEGER," & _
                      " SoftwareName TEXT(50)," & _
                      " IDSoftware TEXT(25), " & _
                      " IDSerial TEXT(25), " & _
                      " IsPostingKartuHutang INTEGER, " & _
                      " IsPostingBukuJurnal INTEGER," & _
                      " PRIMARY KEY(NoID ASC) ) "
                com.CommandText = SQL
                com.ExecuteNonQuery()
            Catch ex As Exception

            End Try
            If Not ds.Tables("sysSettingSoftware") Is Nothing Then
                ds.Tables("sysSettingSoftware").Clear()
            End If
            com.CommandText = "SELECT * FROM sysSettingSoftware WHERE NoID=1"
            oDA.Fill(ds, "sysSettingSoftware")
            
            If ds.Tables("sysSettingSoftware").Rows.Count >= 1 Then
                rbPostingBukuJurnal.EditValue = CShort(NullToLong(ds.Tables("sysSettingSoftware").Rows(0).Item("IsPostingBukuJurnal")))
                rbPostingKartuHutang.EditValue = CShort(NullToLong(ds.Tables("sysSettingSoftware").Rows(0).Item("IsPostingKartuHutang")))
            End If

            FungsiControl.SetForm(Me)
        Catch ex As Exception
            DevExpress.XtraEditors.XtraMessageBox.Show("Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            ds.Dispose()
        End Try
    End Sub
End Class