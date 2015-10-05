Imports System.Data.SQLite
Imports DevExpress.XtraGrid.Views.Base
Imports DevExpress.XtraEditors

Public Class frmGenerateFormH
    Public FormName As String = ""
    Public Tablename As String = ""
    Public FormCaption As String = ""

    Dim ds As New DataSet
    Dim BS As New BindingSource

    Private Sub SimpleButton2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SimpleButton2.Click
        DialogResult = Windows.Forms.DialogResult.Cancel
        Me.Close()

    End Sub

    Private Sub SimpleButton7_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SimpleButton7.Click
        RefreshData()
    End Sub
    Sub RefreshData()
        'Dim f As New OpenFileDialog
        'f.Filter = "SQLite 3 (*.db3)|*.db3|All Files|*.*"
        '  If f.ShowDialog(me) = DialogResult.OK Then
        Dim SQLconnect As New SQLite.SQLiteConnection()
        Dim SQLcommand As SQLiteCommand
        Dim oda As SQLite.SQLiteDataAdapter
        SQLconnect.ConnectionString = "Data Source=" & Application.StartupPath & "\System\engine\syssgi.sqlite" & ";"
        SQLconnect.Open()
        SQLcommand = SQLconnect.CreateCommand
        SQLcommand.CommandText = "SELECT * FROM sysformheader"
        oda = New SQLite.SQLiteDataAdapter(SQLcommand)
        If ds.Tables("sysform") Is Nothing Then
        Else
            ds.Tables("sysform").Clear()
        End If
        oda.Fill(ds, "sysform")

        BS.DataSource = ds.Tables("sysform")
        GridControl1.DataSource = BS
        SQLcommand.Dispose()
        SQLconnect.Close()
        SQLconnect.Dispose()
        oda.Dispose()

    End Sub

    Private Sub SimpleButton1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SimpleButton1.Click
        Try
            Dim ODC As SQLite.SQLiteCommandBuilder
            Dim SQLconnect As New SQLite.SQLiteConnection()
            Dim SQLcommand As SQLiteCommand
            Dim oda As SQLite.SQLiteDataAdapter
            SQLconnect.ConnectionString = "Data Source=" & Application.StartupPath & "\System\engine\syssgi.sqlite" & ";"
            SQLconnect.Open()
            SQLcommand = SQLconnect.CreateCommand
            SQLcommand.CommandText = "SELECT * FROM sysformheader"
            oda = New SQLite.SQLiteDataAdapter(SQLcommand)
            ' oda.TableMappings.Add("Table", "Contacts")


            ' If isNew Then
            'Me.Validate()
            'BS.EndEdit()
            ODC = New SQLite.SQLiteCommandBuilder(oda)
            oda.Update(ds.Tables("sysform"))
            '   End If

            SQLcommand.Dispose()
            SQLconnect.Close()
            SQLconnect.Dispose()
            oda.Dispose()
            XtraMessageBox.Show("Data Tersimpan")
            'Me.Close()

        Catch ex As Exception
            XtraMessageBox.Show("ada kesalahan :" & vbCr & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try

    End Sub



    Private Sub GridView1_CellValueChanged(ByVal sender As Object, ByVal e As DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs) Handles GridView1.CellValueChanged
        If e.Column.FieldName = "fieldname" AndAlso NullTostr(GridView1.GetFocusedRowCellValue("nama")) = "" Then
            GridView1.SetFocusedRowCellValue("nama", GridView1.GetFocusedRowCellValue("fieldname").ToString.Substring(0, 1).ToUpper + GridView1.GetFocusedRowCellValue("fieldname").ToString.Substring(1).ToLower)
        End If
        If e.Column.FieldName = "fieldname" AndAlso NullTostr(GridView1.GetFocusedRowCellValue("caption")) = "" Then
            GridView1.SetFocusedRowCellValue("caption", GridView1.GetFocusedRowCellValue("fieldname").ToString.Substring(0, 1).ToUpper + GridView1.GetFocusedRowCellValue("fieldname").ToString.Substring(1).ToLower)
        End If
    End Sub

    Private Sub GridView1_ColumnChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles GridView1.ColumnChanged

    End Sub


    Private Sub frmGenerateForm_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        GridView1.SaveLayoutToXml(Application.StartupPath & "\System\engine\GenerateFormH.xml")
    End Sub

    Private Sub frmGenerateForm_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        RefreshData()
        If System.IO.File.Exists(Application.StartupPath & "\System\engine\GenerateFormH.xml") Then
            GridView1.RestoreLayoutFromXml(Application.StartupPath & "\System\engine\GenerateFormH.xml")
        End If

    End Sub



    Private Sub GridView1_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles GridView1.MouseDown
        If e.Button = Windows.Forms.MouseButtons.Right Then
            Dim HI As New DevExpress.XtraGrid.Views.Grid.ViewInfo.GridHitInfo
            If e.Button = Windows.Forms.MouseButtons.Right Then
                HI = GridView1.CalcHitInfo(e.X, e.Y)
                If HI.InRow Then
                    PopupMenu1.ShowPopup(Control.MousePosition)
                End If
            End If
        End If
    End Sub

    Private Sub BarButtonItem1_ItemClick_1(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem1.ItemClick
        If MsgBox("Yain mau hapus object pada baris in?", MsgBoxStyle.Question + MsgBoxStyle.YesNo) = MsgBoxResult.Yes Then
            GridView1.DeleteRow(GridView1.FocusedRowHandle)
        End If
    End Sub



    Private Sub SimpleButton5_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SimpleButton5.Click
        Dim view As ColumnView = GridControl1.FocusedView
        Try
            Dim row As System.Data.DataRow = view.GetDataRow(GridView1.FocusedRowHandle)

            FormName = NullTostr(row("namaform"))
            Tablename = NullTostr(row("namatabel"))
            FormCaption = NullTostr(row("caption"))
            DialogResult = Windows.Forms.DialogResult.OK
            Me.Close()

        Catch ex As Exception
            XtraMessageBox.Show("Silahkan pilih data terlebih dahulu lalu tekan tombol OK", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally

        End Try
    End Sub
End Class