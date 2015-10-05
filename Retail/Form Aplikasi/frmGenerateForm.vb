Imports System.Data.SQLite
Imports DevExpress.XtraEditors

Public Class frmGenerateForm
    Dim ds As New DataSet
    Dim BS As New BindingSource

    Private Sub SimpleButton3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SimpleButton3.Click
        Dim nmtabel As String
        nmtabel = InputBox("Nama Tabel yang akan digenerate?:", "")
        If nmtabel <> "" Then
            GeneratefromTable(nmtabel)
            SimpleButton7.PerformClick()
        End If
    End Sub
    Sub GeneratefromTable(ByVal NmTable As String)
        Dim SQL As String = "", IDForm As Long = -1, i As Integer = 0
        Dim cn As New SqlClient.SqlConnection
        Dim com As New SqlClient.SqlCommand
        Dim oDA As New SqlClient.SqlDataAdapter
        Dim ds As New DataSet

        Dim scn As New SQLite.SQLiteConnection
        Dim scom As New SQLite.SQLiteCommand
        Try
            scn.ConnectionString = "Data Source=" & Application.StartupPath & "\System\engine\syssgi.sqlite" & ";"
            scn.Open()
            scom.Connection = scn

            SQL = "SELECT * FROM " & NmTable
            cn.ConnectionString = StrKonSql
            cn.Open()
            com.Connection = cn
            oDA.SelectCommand = com
            com.CommandText = SQL
            If Not ds.Tables(NmTable) Is Nothing Then
                ds.Tables(NmTable).Clear()
            End If
            oDA.Fill(ds, NmTable)
            For Each col As System.Data.DataColumn In ds.Tables(NmTable).Columns
                Select Case col.DataType.Name.ToUpper
                    Case "Int32".ToUpper, "Int16".ToUpper, "Single".ToUpper
                        scom.CommandText = "SELECT MAX(NoID) FROM sysForm"
                        IDForm = NullToLong(scom.ExecuteScalar()) + 1
                        SQL = "INSERT INTO sysForm (noid, formname, nama, caption, fieldname, urut, format, control, tipe, tablename) VALUES (" & vbCrLf & _
                              IDForm & ", '" & FixApostropi(txtformname.Text) & "', '" & FixApostropi(col.ColumnName) & "', " & _
                              " '" & FixApostropi(col.Caption) & "', '" & FixApostropi(col.ColumnName) & "', " & i & ", 'n0', 'TextEdit', 'Int', '" & "m" & FixApostropi(txtformname.Text.ToLower) & "')"
                    Case "Double".ToUpper, "Decimal".ToUpper
                        scom.CommandText = "SELECT MAX(NoID) FROM sysForm"
                        IDForm = NullToLong(scom.ExecuteScalar()) + 1
                        SQL = "INSERT INTO sysForm (noid, formname, nama, caption, fieldname, urut, format, control, tipe, tablename) VALUES (" & vbCrLf & _
                              IDForm & ", '" & FixApostropi(txtformname.Text) & "', '" & FixApostropi(col.ColumnName) & "', " & _
                              " '" & FixApostropi(col.Caption) & "', '" & FixApostropi(col.ColumnName) & "', " & i & ", 'n2', 'CalcEdit', 'Numeric', '" & "m" & FixApostropi(txtformname.Text.ToLower) & "')"
                    Case "Money".ToUpper
                        scom.CommandText = "SELECT MAX(NoID) FROM sysForm"
                        IDForm = NullToLong(scom.ExecuteScalar()) + 1
                        SQL = "INSERT INTO sysForm (noid, formname, nama, caption, fieldname, urut, format, control, tipe, tablename, [default]) VALUES (" & vbCrLf & _
                              IDForm & ", '" & FixApostropi(txtformname.Text) & "', '" & FixApostropi(col.ColumnName) & "', " & _
                              " '" & FixApostropi(col.Caption) & "', '" & FixApostropi(col.ColumnName) & "', " & i & ", 'n2', 'CalcEdit', 'Money', '" & "m" & FixApostropi(txtformname.Text.ToLower) & "','0')"
                    Case "Date".ToUpper, "Datetime".ToUpper, "Time".ToUpper
                        scom.CommandText = "SELECT MAX(NoID) FROM sysForm"
                        IDForm = NullToLong(scom.ExecuteScalar()) + 1
                        SQL = "INSERT INTO sysForm (noid, formname, nama, caption, fieldname, urut, format, control, tipe, tablename, [default]) VALUES (" & vbCrLf & _
                              IDForm & ", '" & FixApostropi(txtformname.Text) & "', '" & FixApostropi(col.ColumnName) & "', " & _
                              " '" & FixApostropi(col.Caption) & "', '" & FixApostropi(col.ColumnName) & "', " & i & ", 'dd-MM-yyyy', 'DateEdit', 'Datetime', '" & "m" & FixApostropi(txtformname.Text.ToLower) & "','Now')"
                    Case "String".ToUpper, "Varchar".ToUpper
                        scom.CommandText = "SELECT MAX(NoID) FROM sysForm"
                        IDForm = NullToLong(scom.ExecuteScalar()) + 1
                        SQL = "INSERT INTO sysForm (noid, formname, nama, caption, fieldname, urut, format, control, tipe, tablename, [default]) VALUES (" & vbCrLf & _
                              IDForm & ", '" & FixApostropi(txtformname.Text) & "', '" & FixApostropi(col.ColumnName) & "', " & _
                              " '" & FixApostropi(col.Caption) & "', '" & FixApostropi(col.ColumnName) & "', " & i & ", '', 'TextEdit', 'Varchar', '" & "m" & FixApostropi(txtformname.Text.ToLower) & "','')"
                    Case "Bit".ToUpper, "Boolean".ToUpper
                        scom.CommandText = "SELECT MAX(NoID) FROM sysForm"
                        IDForm = NullToLong(scom.ExecuteScalar()) + 1
                        SQL = "INSERT INTO sysForm (noid, formname, nama, caption, fieldname, urut, format, control, tipe, tablename, [default]) VALUES (" & vbCrLf & _
                              IDForm & ", '" & FixApostropi(txtformname.Text) & "', '" & FixApostropi(col.ColumnName) & "', " & _
                              " '" & FixApostropi(col.Caption) & "', '" & FixApostropi(col.ColumnName) & "', " & i & ", '', 'CheckEdit', 'Bit', '" & "m" & FixApostropi(txtformname.Text.ToLower) & "','1')"
                    Case Else
                        SQL = ""
                End Select
                scom.CommandText = "SELECT 1 FROM sysForm WHERE UPPER(FormName)='" & FixApostropi(txtformname.Text.ToUpper) & "' AND UPPER(FieldName)='" & FixApostropi(col.ColumnName.ToUpper) & "'"
                If SQL <> "" AndAlso Not NullToBool(scom.ExecuteScalar()) Then
                    scom.CommandText = SQL
                    scom.ExecuteNonQuery()
                    i = i + 1
                End If
            Next
        Catch ex As Exception
            XtraMessageBox.Show("Info Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
        Finally
            If scn.State = ConnectionState.Open Then
                scn.Close()
            End If
            scn.Dispose()
            scom.Dispose()
            If cn.State = ConnectionState.Open Then
                cn.Close()
            End If
            cn.Dispose()
            com.Dispose()
            oDA.Dispose()
            ds.Dispose()
        End Try
    End Sub

    Private Sub SimpleButton2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SimpleButton2.Click
        If MsgBox("Yakin Mau menutup Form Generator ini?", MsgBoxStyle.Question + MsgBoxStyle.YesNo) = MsgBoxResult.Yes Then
            Me.Close()
        End If
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
        SQLcommand.CommandText = "SELECT * FROM sysform where formname='" & txtformname.Text & "'"
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
            SQLcommand.CommandText = "SELECT * FROM sysform where formname='" & txtformname.Text & "'"
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
            DevExpress.XtraEditors.XtraMessageBox.Show("Data Tersimpan")
            'Me.Close()

        Catch ex As Exception
            XtraMessageBox.Show("ada kesalahan :" & vbCr & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try

    End Sub

    Private Sub SimpleButton6_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SimpleButton6.Click

    End Sub

    Private Sub SimpleButton5_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SimpleButton5.Click
        Dim frentri As New frmSimpleEntri
        frentri.FormName = txtformname.Text
        frentri.isNew = True
        frentri.ShowDialog(Me)
        frentri.Dispose()
    End Sub

    Private Sub SimpleButton8_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SimpleButton8.Click
        Evaluate("")
    End Sub

    Private Sub GridControl1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles GridControl1.Click

    End Sub

    Private Sub GridView1_CellValueChanged(ByVal sender As Object, ByVal e As DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs) Handles GridView1.CellValueChanged
        Try
            If e.Column.FieldName = "fieldname" AndAlso NullToStr(GridView1.GetFocusedRowCellValue("nama")) = "" Then
                GridView1.SetFocusedRowCellValue("nama", GridView1.GetFocusedRowCellValue("fieldname").ToString.Substring(0, 1).ToUpper + GridView1.GetFocusedRowCellValue("fieldname").ToString.Substring(1).ToLower)
            End If
            If e.Column.FieldName = "fieldname" AndAlso NullToStr(GridView1.GetFocusedRowCellValue("caption")) = "" Then
                GridView1.SetFocusedRowCellValue("caption", GridView1.GetFocusedRowCellValue("fieldname").ToString.Substring(0, 1).ToUpper + GridView1.GetFocusedRowCellValue("fieldname").ToString.Substring(1).ToLower)
            End If
        Catch ex As Exception

        End Try
    End Sub

    Private Sub GridView1_ColumnChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles GridView1.ColumnChanged

    End Sub

    Private Sub GridView1_InitNewRow(ByVal sender As Object, ByVal e As DevExpress.XtraGrid.Views.Grid.InitNewRowEventArgs) Handles GridView1.InitNewRow
        GridView1.SetRowCellValue(e.RowHandle, "formname", txtformname.Text)
        GridView1.SetRowCellValue(e.RowHandle, "tablename", "m" & txtformname.Text.ToLower)
        GridView1.SetRowCellValue(e.RowHandle, "visible", 1)
    End Sub

    Private Sub frmGenerateForm_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        GridView1.SaveLayoutToXml(Application.StartupPath & "\System\engine\GenerateForm.xml")
    End Sub

    Private Sub frmGenerateForm_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If System.IO.File.Exists(Application.StartupPath & "\System\engine\GenerateForm.xml") Then
            GridView1.RestoreLayoutFromXml(Application.StartupPath & "\System\engine\GenerateForm.xml")
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

    Private Sub SimpleButton9_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SimpleButton9.Click

        Dim SQLconnect As New SQLite.SQLiteConnection()
        Dim SQLcommand As SQLiteCommand
        Dim odr As SQLite.SQLiteDataReader
        SQLconnect.ConnectionString = "Data Source=" & Application.StartupPath & "\System\engine\syssgi.sqlite" & ";"
        SQLconnect.Open()
        SQLcommand = SQLconnect.CreateCommand
        SQLcommand.CommandText = "SELECT idtipeform,namaform,namatabel,namatabeldetil,caption,namaformentri,namatabelmaster FROM sysformheader where namaform='" & txtformname.Text & "'"
        odr = SQLcommand.ExecuteReader
        If odr.Read Then
            Dim frENTRI As New FrmEntriMasterDetil
            frENTRI.FormName = NullToStr(odr.GetValue(1))
            frENTRI.TableName = NullToStr(odr.GetValue(2))
            frENTRI.SqlDetil = NullToStr(odr.GetValue(3))
            frENTRI.Text = NullToStr(odr.GetValue(4))
            frENTRI.FormEntriName = NullToStr(odr.GetValue(5))
            frENTRI.TableNameD = NullToStr(odr.GetValue(6))

            frENTRI.isNew = True
            If frENTRI.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
            End If
            frENTRI.Dispose()
        End If
    End Sub

    Private Sub txtformname_ButtonClick(ByVal sender As Object, ByVal e As DevExpress.XtraEditors.Controls.ButtonPressedEventArgs) Handles txtformname.ButtonClick
        Dim frheader As New frmGenerateFormH
        If frheader.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
            txtformname.Text = frheader.FormName
            txttable.Text = frheader.Tablename
            txtcaption.Text = frheader.FormCaption
            RefreshData()
        End If
        frheader.Dispose()
    End Sub

    Private Sub txtformname_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtformname.EditValueChanged
       
    End Sub
End Class