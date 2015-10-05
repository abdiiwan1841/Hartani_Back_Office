
Imports System.Data.SqlClient
Imports System.Data.SQLite
Imports System.Windows.Forms
Imports DevExpress.XtraEditors
Imports DevExpress.XtraGrid.Views
Imports DevExpress.XtraGrid.Columns
Imports DevExpress.XtraGrid.Views.Base
Imports System.Drawing
Imports DevExpress.XtraEditors.Repository
Imports VPoint.mdlCetakCR
Imports VPoint.clsPostingPembelian
Imports DevExpress.Utils
Imports DevExpress.XtraGrid.Views.Grid.ViewInfo
Imports DevExpress.XtraGrid.Views.Grid

Public Class frmDaftarJenisBarang
    Public FormName As String = ""
    Public FormEntriName As String = ""
    Public TableName As String = ""
    Public TableNameD As String = ""
    Public TableMaster As String = ""
    Public NoID As Long = -1
    Dim oda2 As SqlDataAdapter
    Dim ds As New DataSet
    Dim BS As New BindingSource

    Dim repckedit As New RepositoryItemCheckEdit
    Dim repdateedit As New RepositoryItemDateEdit
    Dim reptextedit As New RepositoryItemTextEdit
    Dim reppicedit As New RepositoryItemPictureEdit

    Private Sub ctrlDaftar_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim dlg As DevExpress.Utils.WaitDialogForm = Nothing
        Try
            Dim curentcursor As Cursor = Windows.Forms.Cursor.Current
            Windows.Forms.Cursor.Current = Cursors.WaitCursor
            dlg = New DevExpress.Utils.WaitDialogForm(String.Format("Creating component and analize database.{0}MOHON TUNGGU ...", vbCrLf), Application.ProductName)
            dlg.TopMost = False
            dlg.Show()
            Windows.Forms.Cursor.Current = Cursors.WaitCursor
            RefreshData()
            RestoreLayout()
            Me.lbDaftar.Text = Me.Text
            FungsiControl.SetForm(Me)
        Catch ex As Exception
            XtraMessageBox.Show(ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            Windows.Forms.Cursor.Current = Cursors.Default
            dlg.Close()
            dlg.Dispose()
        End Try
    End Sub

    Private Sub RestoreLayout()
        If System.IO.File.Exists(folderLayouts & Me.Name & ".xml") Then
            GV1.RestoreLayoutFromXml(folderLayouts & Me.Name & ".xml")
        End If
    End Sub
    'Sub generateform()
    '    Dim cn As New SqlConnection(StrKonSql)
    '    Dim ocmd2 As New SqlCommand
    '    Dim strsql As String = ""
    '    Dim Cur As Cursor = Windows.Forms.Cursor.Current
    '    Windows.Forms.Cursor.Current = Cursors.WaitCursor

    '    strsql = IIf(InStr(TableName, "select", CompareMethod.Text) > 0, TableName, "select * from " & TableName)

    '    ocmd2.Connection = cn
    '    ocmd2.CommandType = CommandType.Text
    '    ocmd2.CommandText = strsql
    '    cn.Open()

    '    oda2 = New SqlDataAdapter(ocmd2)
    '    oda2.Fill(ds, "Data")
    '    BS.DataSource = ds.Tables("Data")
    '    GC1.DataSource = BS.DataSource
    '    For i As Integer = 0 To GV1.Columns.Count - 1
    '        ' MsgBox(GV1.Columns(i).fieldname.ToString)
    '        Select Case GV1.Columns(i).ColumnType.Name.ToLower

    '            Case "int32", "int64", "int"
    '                GV1.Columns(i).DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
    '                GV1.Columns(i).DisplayFormat.FormatString = "n0"
    '            Case "decimal", "single", "money", "double"
    '                GV1.Columns(i).DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
    '                GV1.Columns(i).DisplayFormat.FormatString = "n2"
    '            Case "string"
    '                GV1.Columns(i).DisplayFormat.FormatType = DevExpress.Utils.FormatType.None
    '                GV1.Columns(i).DisplayFormat.FormatString = ""
    'Case "date"
    '    GV1.Columns(i).DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime
    '    GV1.Columns(i).DisplayFormat.FormatString = "dd-MM-yyyy"
    'Case "datetime"
    '    GV1.Columns(i).DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime
    '    GV1.Columns(i).DisplayFormat.FormatString = "dd-MM-yyyy HH:mm"
    '            Case "byte[]"
    '                reppicedit.SizeMode = DevExpress.XtraEditors.Controls.PictureSizeMode.Squeeze
    '                GV1.Columns(i).OptionsColumn.AllowGroup = False
    '                GV1.Columns(i).OptionsColumn.AllowSort = False
    '                GV1.Columns(i).OptionsFilter.AllowFilter = False
    '                GV1.Columns(i).ColumnEdit = reppicedit
    '            Case "boolean"
    '                GV1.Columns(i).ColumnEdit = repckedit
    '        End Select
    '        If GV1.Columns(i).FieldName.Length >= 4 AndAlso GV1.Columns(i).FieldName.Substring(0, 4).ToLower = "Kode".ToLower Then
    '            GV1.Columns(i).Fixed = FixedStyle.Left
    '        ElseIf GV1.Columns(i).FieldName.ToLower = "Nama".ToLower Then
    '            GV1.Columns(i).Fixed = FixedStyle.Left
    '        End If
    '    Next
    '    'For i = 0 To ds.Tables("Master").Rows.Count - 1

    '    '    If NullTostr(ds.Tables("Master").Rows(i).Item("fieldname")).Trim = "" Then
    '    '        Dim unbColumn As GridColumn = GV1.Columns.AddField(NullTostr(ds.Tables("Master").Rows(i).Item("nama")))
    '    '        unbColumn.VisibleIndex = GV1.Columns.Count
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
    '    '        Dim bndColumn As GridColumn = GV1.Columns(NullTostr(ds.Tables("Master").Rows(i).Item("fieldname"))) ' GV1.Columns.AddField(NullTostr(ds.Tables("Master").Rows(i).Item("fieldname")))
    '    '        bndColumn.Caption = ds.Tables("Master").Rows(i).Item("caption")
    '    '        bndColumn.Visible = NullTobool(ds.Tables("Master").Rows(i).Item("visible"))
    '    '        If bndColumn.Visible Then
    '    '            bndColumn.VisibleIndex = GV1.Columns.Count
    '    '        End If
    '    '        ' GV1.Columns.AddField(ds.Tables("Master").Rows(i).Item("fieldname").ToString)
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

    Private Sub SimpleButton2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SimpleButton2.Click
        Dim x As New frmSimpleEntri
        x = New frmSimpleEntri
        x.FormName = "JenisBarang"
        x.isNew = False
        x.NoID = GV1.GetRowCellValue(GV1.FocusedRowHandle, "NoID")
        If x.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
            RefreshData()
            GV1.ClearSelection()
            GV1.FocusedRowHandle = GV1.LocateByDisplayText(0, GV1.Columns("NoID"), x.NoID.ToString("#,##0"))
            GV1.SelectRow(GV1.FocusedRowHandle)
        End If
        x.Dispose()
    End Sub

    Private Sub SimpleButton3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SimpleButton3.Click
        Hapus()
    End Sub

    Private Sub SimpleButton1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SimpleButton1.Click
        Dim x As New frmSimpleEntri
        x.FormName = "JenisBarang"
        x.isNew = True
        If x.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
            RefreshData()
            GV1.FocusedRowHandle = GV1.LocateByDisplayText(0, GV1.Columns("NoID"), x.NoID.ToString("#,##0"))
        End If
        x.Dispose()
    End Sub
    Sub Baru()

        Dim SQLconnect As New SQLite.SQLiteConnection()
        Dim SQLcommand As SQLiteCommand
        Dim odr As SQLite.SQLiteDataReader
        SQLconnect.ConnectionString = "Data Source=" & Application.StartupPath & "\System\engine\syssgi.sqlite" & ";"
        SQLconnect.Open()
        SQLcommand = SQLconnect.CreateCommand
        SQLcommand.CommandText = "SELECT idtipeform,namaform,namatabel,namatabeldetil,caption,namaformentri,namatabelmaster FROM sysformheader where namaform='" & FormEntriName & "'"
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
                RefreshData()
                GV1.FocusedRowHandle = GV1.LocateByDisplayText(0, GV1.Columns("NoID"), NoID.ToString("#,##0"))

            End If
            frENTRI.Dispose()
        End If
    End Sub
    Sub RefreshData()
        Dim dlg As DevExpress.Utils.WaitDialogForm = Nothing
        Try
            Dim curentcursor As Cursor = Windows.Forms.Cursor.Current
            Windows.Forms.Cursor.Current = Cursors.WaitCursor
            dlg = New DevExpress.Utils.WaitDialogForm(String.Format("Sedang merefresh data.{0}MOHON TUNGGU ...", vbCrLf), Application.ProductName)
            dlg.TopMost = False
            dlg.Show()
            Windows.Forms.Cursor.Current = Cursors.WaitCursor

            Dim cn As New SqlConnection(StrKonSql)
            Dim ocmd2 As New SqlCommand
            Dim strsql As String = "SELECT * FROM MJenisBarang " & IIf(ckAll.Checked, "", " WHERE MJenisBarang.IsActive=1")
            ocmd2.Connection = cn
            ocmd2.CommandType = CommandType.Text
            ocmd2.CommandText = strsql
            cn.Open()
            oda2 = New SqlDataAdapter(ocmd2)
            If ds.Tables("Data") Is Nothing Then
            Else
                ds.Tables("Data").Clear()
            End If
            oda2.Fill(ds, "Data")
            BS.DataSource = ds.Tables("Data")
            GC1.DataSource = BS.DataSource
            ocmd2.Dispose()
            cn.Close()
            cn.Dispose()
            For i As Integer = 0 To GV1.Columns.Count - 1
                Select Case GV1.Columns(i).ColumnType.Name.ToLower
                    Case "int32", "int64", "int"
                        GV1.Columns(i).DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
                        GV1.Columns(i).DisplayFormat.FormatString = "n0"
                    Case "decimal", "single", "money", "double"
                        GV1.Columns(i).DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
                        GV1.Columns(i).DisplayFormat.FormatString = "n2"
                    Case "string"
                        GV1.Columns(i).DisplayFormat.FormatType = DevExpress.Utils.FormatType.None
                        GV1.Columns(i).DisplayFormat.FormatString = ""
                    Case "date"
                        GV1.Columns(i).DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime
                        GV1.Columns(i).DisplayFormat.FormatString = "dd-MM-yyyy"
                    Case "datetime"
                        GV1.Columns(i).DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime
                        GV1.Columns(i).DisplayFormat.FormatString = "dd-MM-yyyy HH:mm"
                    Case "byte[]"
                        reppicedit.SizeMode = DevExpress.XtraEditors.Controls.PictureSizeMode.Squeeze
                        GV1.Columns(i).OptionsColumn.AllowGroup = False
                        GV1.Columns(i).OptionsColumn.AllowSort = False
                        GV1.Columns(i).OptionsFilter.AllowFilter = False
                        GV1.Columns(i).ColumnEdit = reppicedit
                    Case "boolean"
                        GV1.Columns(i).ColumnEdit = repckedit
                End Select
                If GV1.Columns(i).FieldName.Length >= 4 AndAlso GV1.Columns(i).FieldName.Substring(0, 4).ToLower = "Kode".ToLower Then
                    GV1.Columns(i).Fixed = FixedStyle.Left
                ElseIf GV1.Columns(i).FieldName.ToLower = "Nama".ToLower Then
                    GV1.Columns(i).Fixed = FixedStyle.Left
                End If
            Next
        Catch ex As Exception
            XtraMessageBox.Show(ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            Windows.Forms.Cursor.Current = Cursors.Default
            dlg.Close()
            dlg.Dispose()
        End Try
    End Sub
    Sub Edit()
        Dim view As ColumnView = GC1.FocusedView
        Try
            Dim row As System.Data.DataRow = view.GetDataRow(GV1.FocusedRowHandle)
            Dim dc As Integer = GV1.FocusedRowHandle
            Dim NoID As Long = NullToLong(row("NoID"))
            Dim SQLconnect As New SQLite.SQLiteConnection()
            Dim SQLcommand As SQLiteCommand
            Dim odr As SQLite.SQLiteDataReader
            SQLconnect.ConnectionString = "Data Source=" & Application.StartupPath & "\System\engine\syssgi.sqlite" & ";"
            SQLconnect.Open()
            SQLcommand = SQLconnect.CreateCommand
            SQLcommand.CommandText = "SELECT idtipeform,namaform,namatabel,namatabeldetil,caption,namaformentri,namatabelmaster FROM sysformheader where namaform='" & FormEntriName & "'"
            odr = SQLcommand.ExecuteReader
            If odr.Read Then
                Dim frENTRI As New FrmEntriMasterDetil
                frENTRI.FormName = NullToStr(odr.GetValue(1))
                frENTRI.TableName = NullToStr(odr.GetValue(2))
                frENTRI.SqlDetil = NullToStr(odr.GetValue(3))
                frENTRI.Text = NullToStr(odr.GetValue(4))
                frENTRI.FormEntriName = NullToStr(odr.GetValue(5))
                frENTRI.TableNameD = NullToStr(odr.GetValue(6))

                frENTRI.isNew = False
                frENTRI.NoID = NoID
                If frENTRI.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
                    RefreshData()
                    GV1.ClearSelection()
                    GV1.FocusedRowHandle = GV1.LocateByDisplayText(0, GV1.Columns("NoID"), NoID.ToString("#,##0"))
                    GV1.SelectRow(GV1.FocusedRowHandle)
                End If
                frENTRI.Dispose()
            End If

            odr.Close()
            SQLcommand.Dispose()
            SQLconnect.Close()
            SQLconnect.Dispose()
        Catch ex As Exception
            XtraMessageBox.Show("Untuk menghapus pilih data yang akan dihapus terlebih dahulu lalu tekan tombol hapus!", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally

        End Try
    End Sub
    Sub Hapus()
        Dim view As ColumnView = GC1.FocusedView
        Try
            Dim row As System.Data.DataRow = view.GetDataRow(GV1.FocusedRowHandle)
            Dim dc As Integer = GV1.FocusedRowHandle
            Dim NoID As Long = NullTolong(row("NoID"))
            If XtraMessageBox.Show(Me, "Yakin mau hapus data " & Me.Text & " dengan Kode " & NullTostr(row("Kode")), NamaAplikasi, MessageBoxButtons.YesNo) = Windows.Forms.DialogResult.Yes Then
                EksekusiSQL("update MJenisBarang set IsActive=0 where NoID= " & NoID.ToString)
                RefreshData()
                GV1.ClearSelection()
                GV1.FocusedRowHandle = GV1.LocateByDisplayText(0, GV1.Columns("NoID"), (NoID).ToString("#,##0"))
                GV1.SelectRow(GV1.FocusedRowHandle)
            End If
        Catch ex As Exception
            XtraMessageBox.Show("Untuk menghapus pilih data yang akan dihapus terlebih dahulu lalu tekan tombol hapus!", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
        End Try
    End Sub

    Private Sub mnBaru_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles mnBaru.ItemClick
        SimpleButton1.PerformClick()
    End Sub

    Private Sub BarButtonItem2_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem2.ItemClick
        SimpleButton2.PerformClick()
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
        RefreshData()
    End Sub

    Private Sub BarButtonItem6_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem6.ItemClick
        PrintPreview()
    End Sub
    Sub ExportExcel()
        Dim dlgsave As New SaveFileDialog
        dlgsave.Title = "Export Daftar ke Excel"
        dlgsave.Filter = "Excel Files|*.xls"
        If dlgsave.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
            GC1.ExportToXls(dlgsave.FileName)
            BukaFile(dlgsave.FileName)
        End If
        dlgsave.Dispose()
    End Sub
    Sub PrintPreview()

        GC1.ShowPrintPreview()

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

    Private Sub cmdFaktur_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdFaktur.Click
        mnPindahJenisBarang.PerformClick()
    End Sub
    Private Sub CetakFaktur(ByVal action As action_)
        Dim namafile As String
        Dim view As ColumnView = GC1.FocusedView
        Try
            namafile = Application.StartupPath & "\report\Faktur" & TableMaster & ".rpt"

            Dim row As System.Data.DataRow = view.GetDataRow(GV1.FocusedRowHandle)
            Dim dc As Integer = GV1.FocusedRowHandle
            Dim NoID As Long = NullTolong(row("NoID"))

            'ViewReport(action, namafile, "Faktur Penjualan", "{MJual.NoID}=" & NoID)
            If EditReport Then
                action = action_.Edit
            Else
                action = action_.Preview
            End If
            ViewReport(Me.ParentForm, action, namafile, Me.Text, "{" & TableMaster & ".NoID}=" & NoID)
            'CetakCRViewer(action, Me.MdiParent, namafile, "Cetak " & Me.Text, "{MJual.NoID}=" & NoID)
        Catch EX As Exception
            DevExpress.XtraEditors.XtraMessageBox.Show("Kesalahan:" & EX.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    'Private Sub mnPosting_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs)
    '    Dim dlg As New WaitDialogForm("Sedang Proses Posting...", "Mohon Tunggu Sebentar.")
    '    Dim NoID As Long = -1
    '    Try
    '        Enabled = False
    '        dlg.Show()
    '        dlg.Owner = Me
    '        dlg.TopMost = True
    '        Dim jumItem As Integer = GV1.SelectedRowsCount
    '        For Each i In GV1.GetSelectedRows
    '            NoID = CStr(GV1.GetDataRow(i).Item("NoID"))
    '            Select Case TableMaster.ToUpper
    '                Case "MLPB".ToUpper
    '                    PostingLPB(NoID)
    '                Case "MPO".ToUpper
    '                    PostingPO(NoID)
    '                Case "MBELI".ToUpper
    '                    PostingStokBarangPembelian(NoID)
    '                Case "MJUAL".ToUpper
    '                    PostingStokBarangPenjualan(NoID)
    '                Case "MReturJUAL".ToUpper
    '                    PostingStokBarangReturPenjualan(NoID)
    '                Case "MReturBeli".ToUpper
    '                    PostingStokBarangReturPembelian(NoID)
    '                Case "MMUTASIGUDANG".ToUpper
    '                    PostingStokBarangMutasiGudang(NoID)
    '            End Select
    '            Enabled = True
    '            RefreshData()
    '            Application.DoEvents()
    '        Next
    '    Catch ex As Exception
    '        XtraMessageBox.Show("Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
    '    Finally
    '        dlg.Close()
    '        dlg.Dispose()
    '        Enabled = True
    '    End Try
    'End Sub

    'Private Sub mnUnPosting_ItemClick(ByVal sender As Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs)
    '    Dim dlg As New WaitDialogForm("Proses UnPosting diproses...", "Mohon Tunggu Sebentar.")
    '    Dim NoID As Long = -1
    '    Try
    '        Enabled = False
    '        dlg.Show()
    '        dlg.Owner = Me
    '        dlg.TopMost = True
    '        Dim jumItem As Integer = GV1.SelectedRowsCount
    '        For Each i In GV1.GetSelectedRows
    '            NoID = CStr(GV1.GetDataRow(i).Item("NoID"))
    '            Select Case TableMaster.ToUpper
    '                Case "MLPB".ToUpper
    '                    UnPostingLPB(NoID)
    '                Case "MPO".ToUpper
    '                    UnPostingPO(NoID)
    '                Case "MBELI".ToUpper
    '                    UnPostingStokBarangPembelian(NoID)
    '                Case "MJUAL".ToUpper
    '                    UnPostingStokBarangPenjualan(NoID)
    '                Case "MMutasiGudang".ToUpper
    '                    UnPostingStokBarangMutasiGudang(NoID)
    '            End Select
    '            Enabled = True
    '            RefreshData()
    '            Application.DoEvents()
    '        Next
    '    Catch ex As Exception
    '        XtraMessageBox.Show("Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
    '    Finally
    '        dlg.Close()
    '        dlg.Dispose()
    '        Enabled = True
    '    End Try

    'End Sub
    Private Sub GV1_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles GV1.MouseDown
        Dim View As GridView = CType(sender, GridView)
        If View Is Nothing Then Return
        ' obtaining hit info
        Dim hitInfo As GridHitInfo = View.CalcHitInfo(New System.Drawing.Point(e.X, e.Y))
        If (e.Button = Windows.Forms.MouseButtons.Right) And (hitInfo.InRow) And _
          (Not View.IsGroupRow(hitInfo.RowHandle)) Then
            PopupMenu1.ShowPopup(Control.MousePosition)
        End If
    End Sub

    Private Sub mnPindahJenisBarang_ItemClick(ByVal sender As Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles mnPindahJenisBarang.ItemClick
        Dim x As New frmRubahJenisBarang
        Try
            If x.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then

            End If
        Catch ex As Exception
            XtraMessageBox.Show("Kesalahan : " & ex.Message)
        Finally
            x.Dispose()
        End Try
    End Sub

    Private Sub mnRefresh_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles mnRefresh.ItemClick
        RefreshData()
    End Sub

    Private Sub mnSaveLayouts_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles mnSaveLayouts.ItemClick
        Dim xOtorisasi As New frmOtorisasiAdmin
        Try
            If XtraMessageBox.Show("Simpan Layout?", NamaAplikasi, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = Windows.Forms.DialogResult.Yes AndAlso xOtorisasi.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
                GV1.SaveLayoutToXml(folderLayouts & Me.Name & ".xml")
            End If
        Catch ex As Exception
            XtraMessageBox.Show("Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            xOtorisasi.Dispose()
        End Try
    End Sub

    Private Sub ckAll_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ckAll.CheckedChanged
        SimpleButton8.PerformClick()
    End Sub

    Private Sub GC1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles GC1.Click

    End Sub
End Class