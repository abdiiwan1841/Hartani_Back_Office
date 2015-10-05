
Imports System.Data.SqlClient
Imports System.Data.SQLite
Imports System.Windows.Forms
Imports DevExpress.XtraEditors
Imports DevExpress.XtraGrid.Views
Imports DevExpress.XtraGrid.Columns
Imports DevExpress.XtraGrid.Views.Base
Imports System.Drawing
Imports DevExpress.XtraEditors.Repository

Public Class frmDaftarTree
    Public FormName As String = ""
    Public FormEntriName As String = ""
    Public TableName As String = ""
    Public TableMaster As String = ""
    Public NoID As Long = -1
    Dim oda2 As SqlDataAdapter
    Dim ds As New DataSet
    Dim BS As New BindingSource


    Dim repckedit As New RepositoryItemCheckEdit
    Dim repdateedit As New RepositoryItemDateEdit
    Dim reptextedit As New RepositoryItemTextEdit

    Private Sub ctrlDaftar_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        RefreshData()
        RestoreLayout()
        lbDaftar.Text = Me.Text
        FungsiControl.SetForm(Me)
    End Sub

    Private Sub RestoreLayout()
        If System.IO.File.Exists(folderLayouts & FormName & ".xml") Then
            TreeList1.RestoreLayoutFromXml(folderLayouts & FormName & ".xml")
        End If
        Dim sizefrm As New Size
        'If System.IO.File.Exists(Application.StartupPath & "\System\Layouts\setting_" & FormName & ".dat") Then
        '    sizefrm.Height = bacain
        'End If
    End Sub
    'Sub generateform()
    '    Dim Cur As Cursor = Windows.Forms.Cursor.Current
    '    Windows.Forms.Cursor.Current = Cursors.WaitCursor
    '    Dim cn As New SqlConnection(StrKonSql)
    '    Dim ocmd2 As New SqlCommand

    '    'Dim sysSQLconnect As New SQLite.SQLiteConnection()
    '    'Dim sysSQLcommand As SQLiteCommand
    '    'Dim sysSQLoda As SQLite.SQLiteDataAdapter
    '    'sysSQLconnect.ConnectionString = "Data Source=" & Application.StartupPath & "\\System\engine\syssgi.sqlite" & ";"
    '    'sysSQLconnect.Open()
    '    'sysSQLcommand = sysSQLconnect.CreateCommand
    '    'sysSQLcommand.CommandText = "SELECT * FROM sysform where formname='" & FormName & "'"

    '    ocmd2.Connection = cn
    '    ocmd2.CommandType = CommandType.Text
    '    ' TableName = GetTableNamebyFormname(FormName)
    '    ocmd2.CommandText = IIf(InStr(TableName, "select", CompareMethod.Text) > 0, TableName, "select * from " & TableName) & IIf(ckAll.Checked, "", " WHERE " & TableName & ".IsActive=1")
    '    cn.Open()
    '    'sysSQLoda = New SQLite.SQLiteDataAdapter(sysSQLcommand)
    '    'sysSQLoda.Fill(ds, "Master")

    '    oda2 = New SqlDataAdapter(ocmd2)
    '    oda2.Fill(ds, "Data")
    '    BS.DataSource = ds.Tables("Data")
    '    TreeList1.DataSource = BS.DataSource
    '    TreeList1.ExpandAll()
    '    For i As Integer = 0 To TreeList1.Columns.Count - 1
    '        ' MsgBox(TreeList1.Columns(i).fieldname.ToString)
    '        Select Case TreeList1.Columns(i).ColumnType.Name.ToLower

    '            Case "int32", "int64", "int"
    '                TreeList1.Columns(i).Format.FormatType = DevExpress.Utils.FormatType.Numeric
    '                TreeList1.Columns(i).Format.FormatString = "n0"
    '            Case 2
    '            Case "decimal", "single", "money", "double"
    '                TreeList1.Columns(i).Format.FormatType = DevExpress.Utils.FormatType.Numeric
    '                TreeList1.Columns(i).Format.FormatString = "n2"
    '            Case "string"
    '                TreeList1.Columns(i).Format.FormatType = DevExpress.Utils.FormatType.None
    '                TreeList1.Columns(i).Format.FormatString = ""
    '            Case "date", "datetime"
    '                TreeList1.Columns(i).Format.FormatType = DevExpress.Utils.FormatType.DateTime
    '                TreeList1.Columns(i).Format.FormatString = "dd-MM-yyyy"
    '            Case "boolean"
    '                TreeList1.Columns(i).ColumnEdit = repckedit
    '        End Select
    '        If TreeList1.Columns(i).FieldName.ToLower = "Kode".ToLower Then
    '            TreeList1.Columns(i).Fixed = FixedStyle.Left
    '        ElseIf TreeList1.Columns(i).FieldName.ToLower = "Nama".ToLower Then
    '            TreeList1.Columns(i).Fixed = FixedStyle.Left
    '        End If
    '    Next
    '    'For i = 0 To ds.Tables("Master").Rows.Count - 1

    '    '    If NullTostr(ds.Tables("Master").Rows(i).Item("fieldname")).Trim = "" Then
    '    '        Dim unbColumn As GridColumn = TreeList1.Columns.AddField(NullTostr(ds.Tables("Master").Rows(i).Item("nama")))
    '    '        unbColumn.VisibleIndex = TreeList1.Columns.Count
    '    '        Select Case NullTostr(ds.Tables("Master").Rows(i).Item("Tipe"))

    '    '            Case "string"
    '    '                unbColumn.UnboundType = DevExpress.Data.UnboundColumnType.String

    '    '            Case "int", "bigint", "real", "money", "real", "Decimal", "float"
    '    '                unbColumn.UnboundType = DevExpress.Data.UnboundColumnType.Decimal
    '    '                ' Specify format settings.
    '    '                unbColumn.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
    '    '                unbColumn.DisplayFormat.FormatString = ds.Tables("Master").Rows(i).Item("format")
    '    '                unbColumn.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
    '    '            Case "date", "time", "datetime"
    '    '                unbColumn.UnboundType = DevExpress.Data.UnboundColumnType.DateTime
    '    '                ' Specify format settings.
    '    '                unbColumn.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime
    '    '                unbColumn.DisplayFormat.FormatString = ds.Tables("Master").Rows(i).Item("format")
    '    '        End Select

    '    '        ' Disable editing.
    '    '        unbColumn.OptionsColumn.AllowEdit = False

    '    '        ' Customize the appearance settings.
    '    '        unbColumn.AppearanceCell.BackColor = Color.LemonChiffon
    '    '    Else
    '    '        Dim bndColumn As GridColumn = TreeList1.Columns(NullTostr(ds.Tables("Master").Rows(i).Item("fieldname"))) ' TreeList1.Columns.AddField(NullTostr(ds.Tables("Master").Rows(i).Item("fieldname")))
    '    '        bndColumn.Caption = ds.Tables("Master").Rows(i).Item("caption")
    '    '        bndColumn.Visible = NullTobool(ds.Tables("Master").Rows(i).Item("visible"))
    '    '        If bndColumn.Visible Then
    '    '            bndColumn.VisibleIndex = TreeList1.Columns.Count
    '    '        End If
    '    '        ' TreeList1.Columns.AddField(ds.Tables("Master").Rows(i).Item("fieldname").ToString)
    '    '        Select Case ds.Tables("Master").Rows(i).Item("control")
    '    '            Case "checkedit"
    '    '                bndColumn.ColumnEdit = repckedit
    '    '            Case "textedit"
    '    '                bndColumn.ColumnEdit = reptextedit
    '    '            Case "dateedit"
    '    '                repdateedit.Mask.EditMask = ds.Tables("Master").Rows(i).Item("format").ToString
    '    '                repdateedit.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.DateTime
    '    '                repdateedit.Mask.UseMaskAsDisplayFormat = True
    '    '                bndColumn.ColumnEdit = repdateedit
    '    '            Case "lookupedit"
    '    '            Case "string"
    '    '                bndColumn.DisplayFormat.FormatType = DevExpress.Utils.FormatType.None
    '    '            Case "numeric"
    '    '                bndColumn.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
    '    '                bndColumn.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
    '    '                bndColumn.DisplayFormat.FormatString = ds.Tables("Master").Rows(i).Item("format")
    '    '            Case "int", "bigint", "real", "money", "real", "Decimal", "float"
    '    '                bndColumn.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
    '    '                bndColumn.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
    '    '                bndColumn.DisplayFormat.FormatString = ds.Tables("Master").Rows(i).Item("format")
    '    '            Case "date"
    '    '                bndColumn.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime
    '    '                bndColumn.DisplayFormat.FormatString = ds.Tables("Master").Rows(i).Item("format")
    '    '        End Select
    '    '    End If

    '    'Next
    '    ocmd2.Dispose()
    '    cn.Close()
    '    cn.Dispose()
    '    Windows.Forms.Cursor.Current = Cur
    'End Sub

    Private Sub SimpleButton6_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SimpleButton6.Click
        Tutup()
    End Sub

    Private Sub SimpleButton9_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SimpleButton9.Click
        DialogResult = Windows.Forms.DialogResult.OK
        Me.Close()

    End Sub

    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

    End Sub

    Private Sub SimpleButton7_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)

    End Sub

    Private Sub SimpleButton2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SimpleButton2.Click
        SimpleButton2.Enabled = False
        Edit()
        SimpleButton2.Enabled = True

    End Sub

    Private Sub SimpleButton3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SimpleButton3.Click
        Hapus()
    End Sub

    Private Sub SimpleButton1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SimpleButton1.Click
        SimpleButton1.Enabled = False
        Baru()
        SimpleButton1.Enabled = True
    End Sub
    Sub Baru()
        Select Case FormEntriName
            Case "EntriBarang"
                Dim Brg As New clsBarang
                Brg.FormName = FormEntriName
                Brg.isNew = True
                If Brg.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
                    RefreshData()
                    TreeList1.FocusedNode = TreeList1.FindNodeByFieldValue("NoID", Brg.NoID)
                End If
                Brg.Dispose()
            Case Else
                Dim x As New frmSimpleEntri
                x.FormName = FormEntriName
                x.isNew = True
                If x.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
                    RefreshData()
                    TreeList1.FocusedNode = TreeList1.FindNodeByFieldValue("NoID", x.NoID)
                End If
                x.Dispose()
        End Select
    End Sub
    Sub RefreshData()
        Dim Cur As Cursor = Windows.Forms.Cursor.Current
        Windows.Forms.Cursor.Current = Cursors.WaitCursor
        Dim cn As New SqlConnection(StrKonSql)
        Dim ocmd2 As New SqlCommand
        ocmd2.Connection = cn
        ocmd2.CommandType = CommandType.Text
        ocmd2.CommandText = IIf(InStr(TableName, "select", CompareMethod.Text) > 0, TableName, "select * from " & TableName) & IIf(ckAll.Checked, "", " WHERE " & TableMaster & ".IsActive=1")
        cn.Open()
        oda2 = New SqlDataAdapter(ocmd2)
        If ds.Tables("Data") Is Nothing Then
        Else
            ds.Tables("Data").Clear()
        End If
        oda2.Fill(ds, "Data")
        BS.DataSource = ds.Tables("Data")
        TreeList1.DataSource = BS.DataSource
        TreeList1.ExpandAll()
        For i As Integer = 0 To TreeList1.Columns.Count - 1
            ' MsgBox(TreeList1.Columns(i).fieldname.ToString)
            Select Case TreeList1.Columns(i).ColumnType.Name.ToLower

                Case "int32", "int64", "int"
                    TreeList1.Columns(i).Format.FormatType = DevExpress.Utils.FormatType.Numeric
                    TreeList1.Columns(i).Format.FormatString = "n0"
                Case 2
                Case "decimal", "single", "money", "double"
                    TreeList1.Columns(i).Format.FormatType = DevExpress.Utils.FormatType.Numeric
                    TreeList1.Columns(i).Format.FormatString = "n2"
                Case "string"
                    TreeList1.Columns(i).Format.FormatType = DevExpress.Utils.FormatType.None
                    TreeList1.Columns(i).Format.FormatString = ""
                Case "date", "datetime"
                    TreeList1.Columns(i).Format.FormatType = DevExpress.Utils.FormatType.DateTime
                    TreeList1.Columns(i).Format.FormatString = "dd-MM-yyyy"
                Case "boolean"
                    TreeList1.Columns(i).ColumnEdit = repckedit
            End Select
            If TreeList1.Columns(i).FieldName.ToLower = "Kode".ToLower Then
                TreeList1.Columns(i).Fixed = FixedStyle.Left
            ElseIf TreeList1.Columns(i).FieldName.ToLower = "Nama".ToLower Then
                TreeList1.Columns(i).Fixed = FixedStyle.Left
            End If
        Next

        ocmd2.Dispose()
        cn.Close()
        cn.Dispose()
        Windows.Forms.Cursor.Current = Cur
    End Sub
    Sub Edit()
        Dim x As frmSimpleEntri
        Dim Brg As clsBarang
        'Try
        Dim NoID As Long = NullToLong(TreeList1.FocusedNode.Item("NoID"))
        If FormEntriName = "EntriBarang" Then
            Brg = New clsBarang
            Brg.FormName = FormEntriName
            Brg.isNew = False
            Brg.NoID = NoID
            If Brg.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
                RefreshData()
                TreeList1.FocusedNode = TreeList1.FindNodeByFieldValue("NoID", Brg.NoID)
            End If
            Brg.Dispose()
        Else
            x = New frmSimpleEntri
            x.FormName = FormEntriName
            x.isNew = False
            x.NoID = NoID
            If x.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
                RefreshData()
                TreeList1.FocusedNode = TreeList1.FindNodeByFieldValue("NoID", x.NoID)
            End If
            x.Dispose()
        End If

        'Catch ex As Exception
        '    XtraMessageBox.Show("Untuk menghapus pilih data yang akan dihapus terlebih dahulu lalu tekan tombol hapus!", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        'Finally

        'End Try
    End Sub
    Sub Hapus()
        Try

            Dim NoID As Long = NullTolong(TreeList1.FocusedNode.Item("NoID"))

            If XtraMessageBox.Show("Yakin mau hapus data " & Me.Text & " dengan " & IIf(TableMaster.ToLower = "mrulemutasi", "rule mutasi ", "Kode ") & IIf(TableMaster.ToLower = "mrulemutasi", NullTostr(TreeList1.FocusedNode.Item("KodeAsal")) & " ke " & NullTostr(TreeList1.FocusedNode.Item("KodeTujuan")), NullTostr(TreeList1.FocusedNode.Item("Kode"))), NamaAplikasi, MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                EksekusiSQL("update " & TableMaster & " set IsActive=0 where noid= " & NoID.ToString)
                RefreshData()
            End If

        Catch ex As Exception
            XtraMessageBox.Show("Untuk menghapus pilih data yang akan dihapus terlebih dahulu lalu tekan tombol hapus!", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
        End Try
    End Sub

    Private Sub BarButtonItem1_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem1.ItemClick
        Baru()
    End Sub

    Private Sub BarButtonItem2_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem2.ItemClick
        Edit()
    End Sub

    Private Sub BarButtonItem3_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem3.ItemClick
        Hapus()
    End Sub

    Private Sub BarButtonItem4_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem4.ItemClick
        Tutup()
    End Sub
    Sub Tutup()
        DialogResult = Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub SimpleButton8_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SimpleButton8.Click
        SimpleButton8.Enabled = False
        RefreshData()
        SimpleButton8.Enabled = True
    End Sub

    Private Sub BarButtonItem6_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem6.ItemClick
        PrintPreview()
    End Sub
    Sub ExportExcel()
        Dim dlgsave As New SaveFileDialog
        dlgsave.Title = "Export Daftar ke Excel"
        dlgsave.Filter = "Excel Files|*.xls"
        If dlgsave.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
            TreeList1.ExportToXls(dlgsave.FileName)
            BukaFile(dlgsave.FileName)
        End If
        dlgsave.Dispose()
    End Sub
    Sub PrintPreview()

        TreeList1.ShowPrintPreview()

    End Sub

    Private Sub SimpleButton4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SimpleButton4.Click
        ExportExcel()
    End Sub

    Private Sub SimpleButton5_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SimpleButton5.Click
        PrintPreview()
    End Sub

    Private Sub BarButtonItem5_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem5.ItemClick
        ExportExcel()
    End Sub

    Private Sub mnrefresh_ItemClick(ByVal sender As Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles mnrefresh.ItemClick
        SimpleButton8.PerformClick()
    End Sub

    Private Sub ckAll_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ckAll.CheckedChanged
        SimpleButton8.PerformClick()
    End Sub

    Private Sub mnSaveLayouts_ItemClick(ByVal sender As Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles mnSaveLayouts.ItemClick
        Dim xOtorisasi As New frmOtorisasiAdmin
        Try
            If XtraMessageBox.Show("Simpan Layout?", NamaAplikasi, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = Windows.Forms.DialogResult.Yes AndAlso xOtorisasi.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
                TreeList1.SaveLayoutToXml(folderLayouts & FormName & ".xml")
            End If
        Catch ex As Exception
            XtraMessageBox.Show("Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            xOtorisasi.Dispose()
        End Try
    End Sub
End Class