
Imports System.Data.SqlClient
Imports System.Data.SQLite
Imports System.Windows.Forms
Imports DevExpress.XtraEditors
Imports DevExpress.XtraGrid.Views
Imports DevExpress.XtraGrid.Columns
Imports DevExpress.XtraGrid.Views.Base
Imports System.Drawing
Imports DevExpress.XtraEditors.Repository
Imports CitiToys.mdlCetakCR
Imports CitiToys.clsPostingStock
Imports DevExpress.Utils
Imports DevExpress.XtraGrid.Views.Grid.ViewInfo
Imports DevExpress.XtraGrid.Views.Grid
Imports CitiToys.FungsiControl

Public Class frmDaftarPembelian
    Public NoID As Long = -1
    Dim oda2 As SqlDataAdapter
    Dim ds As New DataSet
    Dim BS As New BindingSource

    Dim repckedit As New RepositoryItemCheckEdit
    Dim repdateedit As New RepositoryItemDateEdit
    Dim reptextedit As New RepositoryItemTextEdit
    Dim reppicedit As New RepositoryItemPictureEdit

    Private Sub frmDaftar_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        GV1.SaveLayoutToXml(Application.StartupPath & "\system\layouts\" & Name & GV1.Name & ".xml")
    End Sub

    Private Sub ctrlDaftar_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim dlg As DevExpress.Utils.WaitDialogForm = Nothing
        Try
            Dim curentcursor As Cursor = Windows.Forms.Cursor.Current
            Windows.Forms.Cursor.Current = Cursors.WaitCursor
            dlg = New DevExpress.Utils.WaitDialogForm(String.Format("Creating component and analize data. {0}MOHON TUNGGU ...", vbCrLf), Application.ProductName)
            dlg.TopMost = False
            dlg.Show()
            Windows.Forms.Cursor.Current = Cursors.WaitCursor
            TglDari.EditValue = Today
            TglSampai.EditValue = Today

            SetCtlMe()
            cmdRefresh.PerformClick()
            If System.IO.File.Exists(Application.StartupPath & "\system\layouts\" & Name & GV1.Name & ".xml") Then
                GV1.RestoreLayoutFromXml(Application.StartupPath & "\system\layouts\" & Name & GV1.Name & ".xml")
            End If

            RefreshData()
            RestoreLayout()
            Me.lbDaftar.Text = Me.Text
        Catch ex As Exception
            XtraMessageBox.Show(ex.Message, "Retail System", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            Windows.Forms.Cursor.Current = Cursors.Default
            dlg.Close()
            dlg.Dispose()
        End Try
    End Sub
    Private Sub SetCtlMe()
        Dim ds As New DataSet
        'ds = ExecuteDataset("Button", "SELECT * FROM MShortcut")
        'If ds.Tables(0).Rows.Count >= 1 Then
        cmdBaru.Image = Image.FromFile(Application.StartupPath & "\system\image\F1.png")
        cmdEdit.Image = Image.FromFile(Application.StartupPath & "\system\image\F2.png")
        cmdHapus.Image = Image.FromFile(Application.StartupPath & "\system\image\F4.png")
        cmdExcel.Image = Image.FromFile(Application.StartupPath & "\system\image\F1.png")
        cmdBaru.Image = Image.FromFile(Application.StartupPath & "\system\image\F1.png")
        cmdBaru.Image = Image.FromFile(Application.StartupPath & "\system\image\F1.png")
        cmdBaru.Image = Image.FromFile(Application.StartupPath & "\system\image\F1.png")
        cmdBaru.Image = Image.FromFile(Application.StartupPath & "\system\image\F1.png")
        cmdBaru.Image = Image.FromFile(Application.StartupPath & "\system\image\F1.png")
        'End If
        AddHandler cmdBaru.MouseHover, AddressOf SimpleButton_MouseHover
        AddHandler cmdBaru.MouseLeave, AddressOf SimpleButton_MouseLeave
        AddHandler cmdEdit.MouseHover, AddressOf SimpleButton_MouseHover
        AddHandler cmdEdit.MouseLeave, AddressOf SimpleButton_MouseLeave
        AddHandler cmdHapus.MouseHover, AddressOf SimpleButton_MouseHover
        AddHandler cmdHapus.MouseLeave, AddressOf SimpleButton_MouseLeave
        AddHandler cmdExcel.MouseHover, AddressOf SimpleButton_MouseHover
        AddHandler cmdExcel.MouseLeave, AddressOf SimpleButton_MouseLeave
        AddHandler cmdPreview.MouseHover, AddressOf SimpleButton_MouseHover
        AddHandler cmdPreview.MouseLeave, AddressOf SimpleButton_MouseLeave
        AddHandler cmdFaktur.MouseHover, AddressOf SimpleButton_MouseHover
        AddHandler cmdFaktur.MouseLeave, AddressOf SimpleButton_MouseLeave
        AddHandler cmdRefresh.MouseHover, AddressOf SimpleButton_MouseHover
        AddHandler cmdRefresh.MouseLeave, AddressOf SimpleButton_MouseLeave
        AddHandler cmdOK.MouseHover, AddressOf SimpleButton_MouseHover
        AddHandler cmdOK.MouseLeave, AddressOf SimpleButton_MouseLeave
        AddHandler cmdTutup.MouseHover, AddressOf SimpleButton_MouseHover
        AddHandler cmdTutup.MouseLeave, AddressOf SimpleButton_MouseLeave

    End Sub
    Private Sub RestoreLayout()
        If System.IO.File.Exists(Application.StartupPath & "\System\Layouts\" & Me.Name & GV1.Name & ".xml") Then
            GV1.RestoreLayoutFromXml(Application.StartupPath & "\System\Layouts\" & Me.Name & GV1.Name & ".xml")
        End If
    End Sub
    'Sub RefreshItem()
    '    Dim cn As New SqlConnection(StrKonSql)
    '    Dim ocmd2 As New SqlCommand
    '    Dim strsql As String = ""
    '    Dim Cur As Cursor = Windows.Forms.Cursor.Current
    '    Windows.Forms.Cursor.Current = Cursors.WaitCursor

    '    strsql = IIf(InStr(TableName, "select", CompareMethod.Text) > 0, TableName, "select * from " & TableName)
    '    If TglDari.Enabled Then
    '        If InStr(strsql.ToLower, "where", CompareMethod.Text) > 0 Then
    '            strsql = strsql & " and (" & TableMaster & ".Tanggal>='" & Format(TglDari.DateTime, "yyyy/MM/dd") & "' and " & TableMaster & ".Tanggal<'" & Format(DateAdd(DateInterval.Day, 1, TglSampai.DateTime), "yyyy/MM/dd") & "') "
    '        Else
    '            strsql = strsql & " where (" & TableMaster & ".Tanggal>='" & Format(TglDari.DateTime, "yyyy/MM/dd") & "' and " & TableMaster & ".Tanggal<'" & Format(DateAdd(DateInterval.Day, 1, TglSampai.DateTime), "yyyy/MM/dd") & "') "
    '        End If
    '    End If

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
    '            Case "date", "datetime"
    '                GV1.Columns(i).DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime
    '                GV1.Columns(i).DisplayFormat.FormatString = "dd-MM-yyyy"
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

    Private Sub SimpleButton6_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdTutup.Click
        Tutup()
    End Sub

    Private Sub SimpleButton9_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdOK.Click
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

    Private Sub SimpleButton2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdEdit.Click
        Edit()
    End Sub

    Private Sub SimpleButton3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdHapus.Click
        Hapus()
    End Sub

    Private Sub SimpleButton1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdBaru.Click
        Baru()
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
            frENTRI.FormName = NullTostr(odr.GetValue(1))
            frENTRI.TableName = NullTostr(odr.GetValue(2))
            frENTRI.SqlDetil = NullTostr(odr.GetValue(3))
            frENTRI.Text = NullTostr(odr.GetValue(4))
            frENTRI.FormEntriName = NullTostr(odr.GetValue(5))
            frENTRI.TableNameD = NullTostr(odr.GetValue(6))

            frENTRI.isNew = True
            If frENTRI.ShowDialog() = Windows.Forms.DialogResult.OK Then
                RefreshData()
                GV1.FocusedRowHandle = GV1.LocateByDisplayText(0, GV1.Columns("NoID"), NoID.ToString("#,##0"))

            End If
            frENTRI.Dispose()
        End If
    End Sub
    Sub RefreshData()
        Dim cn As New SqlConnection(StrKonSql)
        Dim ocmd2 As New SqlCommand
        Dim Cur As Cursor = Windows.Forms.Cursor.Current
        Windows.Forms.Cursor.Current = Cursors.WaitCursor
        Dim strsql As String = ""
        strsql = "SELECT dbo.MBeli.*, dbo.MAlamat.Kode AS KodeSupplier, dbo.MAlamat.Nama AS NamaSupplier FROM dbo.MBeli INNER JOIN dbo.MAlamat ON dbo.MBeli.IDSupplier = dbo.MAlamat.NoID "
        If TglDari.Enabled Then
            If InStr(strsql.ToLower, "where", CompareMethod.Text) > 0 Then
                strsql = strsql & " and (mbeli.Tanggal>='" & Format(TglDari.DateTime, "yyyy/MM/dd") & "' and mbeli.Tanggal<'" & Format(DateAdd(DateInterval.Day, 1, TglSampai.DateTime), "yyyy/MM/dd") & "') "
            Else
                strsql = strsql & " where (mbeli.Tanggal>='" & Format(TglDari.DateTime, "yyyy/MM/dd") & "' and mbeli.Tanggal<'" & Format(DateAdd(DateInterval.Day, 1, TglSampai.DateTime), "yyyy/MM/dd") & "') "
            End If
        End If

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

        For i As Integer = 0 To GV1.Columns.Count - 1
            ' MsgBox(GV1.Columns(i).fieldname.ToString)
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
                Case "date", "datetime"
                    GV1.Columns(i).DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime
                    GV1.Columns(i).DisplayFormat.FormatString = "dd-MM-yyyy"
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

        ocmd2.Dispose()
        cn.Close()
        cn.Dispose()
        Windows.Forms.Cursor.Current = Cur
    End Sub
    Sub Edit()
        Dim view As ColumnView = GC1.FocusedView
        Try
            Dim row As System.Data.DataRow = view.GetDataRow(GV1.FocusedRowHandle)
            Dim dc As Integer = GV1.FocusedRowHandle
            Dim NoID As Long = NullTolong(row("NoID"))
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
                frENTRI.FormName = NullTostr(odr.GetValue(1))
                frENTRI.TableName = NullTostr(odr.GetValue(2))
                frENTRI.SqlDetil = NullTostr(odr.GetValue(3))
                frENTRI.Text = NullTostr(odr.GetValue(4))
                frENTRI.FormEntriName = NullTostr(odr.GetValue(5))
                frENTRI.TableNameD = NullTostr(odr.GetValue(6))

                frENTRI.isNew = False
                frENTRI.NoID = NoID
                If frENTRI.ShowDialog() = Windows.Forms.DialogResult.OK Then
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
            MsgBox("Untuk mengedit pilih data yang akan diedit terlebih dahulu lalu tekan tombol edit", MessageBoxButtons.OK + MessageBoxIcon.Error)
        Finally

        End Try
    End Sub
    Sub Hapus()
        Dim view As ColumnView = GC1.FocusedView
        Try
            Dim row As System.Data.DataRow = view.GetDataRow(GV1.FocusedRowHandle)
            Dim dc As Integer = GV1.FocusedRowHandle
            Dim Kode As Long = NullTostr(row("Kode"))
            Dim NoID As Long = NullTolong(row("NoID"))

            If MsgBox("Yakin mau hapus data " & Me.Text & " dengan kode " & Kode.ToString, MsgBoxStyle.Question + MsgBoxStyle.YesNo) = MsgBoxResult.Yes Then
                If Not NullTobool(EksekusiSQlSkalarNew("SELECT IsPosted FROM MBeli WHERE NoID=" & NoID)) Then
                    EksekusiSQL("DELETE FROM MBeli where NoID= " & NoID.ToString)
                    cmdRefresh.PerformClick()
                Else
                    XtraMessageBox.Show("Data yang sudah diposting tidak dapat dihapus.", "Admin Says", MessageBoxButtons.OK, MessageBoxIcon.Information)
                End If
                GV1.ClearSelection()
                GV1.FocusedRowHandle = GV1.LocateByDisplayText(0, GV1.Columns("NoID"), (NoID).ToString("#,##0"))
                GV1.SelectRow(GV1.FocusedRowHandle)
            End If
        Catch ex As Exception
            MsgBox("Untuk menghapus pilih data yang akan dihapus terlebih dahulu lalu tekan tombol hapus!", MessageBoxButtons.OK + MessageBoxIcon.Error)
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

    Private Sub SimpleButton8_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdRefresh.Click
        RefreshData()
    End Sub

    Private Sub BarButtonItem6_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem6.ItemClick
        PrintPreview()
    End Sub
    Sub ExportExcel()
        Dim dlgsave As New SaveFileDialog
        dlgsave.Title = "Export Daftar ke Excel"
        dlgsave.Filter = "Excel Files|*.xls"
        If dlgsave.ShowDialog = Windows.Forms.DialogResult.OK Then
            GC1.ExportToXls(dlgsave.FileName)
            BukaFile(dlgsave.FileName)
        End If
        dlgsave.Dispose()
    End Sub
    Sub PrintPreview()

        GC1.ShowPrintPreview()

    End Sub

    Private Sub SimpleButton4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdExcel.Click
        ExportExcel()
    End Sub

    Private Sub SimpleButton5_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdPreview.Click
        PrintPreview()
    End Sub

    Private Sub BarButtonItem5_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem5.ItemClick
        ExportExcel()
    End Sub

    Private Sub LabelControl1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LabelControl1.Click
        TglDari.Enabled = Not TglDari.Enabled
        TglSampai.Enabled = TglDari.Enabled
        cmdRefresh.PerformClick()
    End Sub

    Private Sub cmdFaktur_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdFaktur.Click
        mnFaktur.PerformClick()
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
            DevExpress.XtraEditors.XtraMessageBox.Show("Kesalahan:" & EX.Message, "RETAIL SYSTEM", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub
    Private Sub mnFaktur_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles mnFaktur.ItemClick
        If frmMain.ckEditRpt.Checked Then
            CetakFaktur(action_.Edit)
        Else
            CetakFaktur(action_.Preview)
        End If
    End Sub

    Private Sub mnPosting_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles mnPosting.ItemClick
        Dim dlg As New WaitDialogForm("Proses Posting diproses...", "Mohon Tunggu Sebentar.")
        Dim NoID As Long = -1
        Try
            Enabled = False
            dlg.Show()
            dlg.Owner = Me
            dlg.TopMost = True
            Dim jumItem As Integer = GV1.SelectedRowsCount
            For Each i In GV1.GetSelectedRows
                NoID = CStr(GV1.GetDataRow(i).Item("NoID"))
                Select Case TableMaster.ToUpper
                    Case "MBELI".ToUpper
                        PostingStokBarangPembelian(NoID)
                    Case "MJUAL".ToUpper
                        PostingStokBarangPenjualan(NoID)
                    Case "MReturJUAL".ToUpper
                        PostingStokBarangReturPenjualan(NoID)
                    Case "MReturBeli".ToUpper
                        PostingStokBarangReturPembelian(NoID)
                    Case "MMUTASIGUDANG".ToUpper
                        PostingStokBarangMutasiGudang(NoID)
                End Select
                Application.DoEvents()
            Next
        Catch ex As Exception
            XtraMessageBox.Show("Kesalahan : " & ex.Message, "RETAIL SYSTEM", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            dlg.Close()
            dlg.Dispose()
            Enabled = True
        End Try
    End Sub

    Private Sub mnUnPosting_ItemClick(ByVal sender As Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles mnUnPosting.ItemClick
        Dim dlg As New WaitDialogForm("Proses UnPosting diproses...", "Mohon Tunggu Sebentar.")
        Dim NoID As Long = -1
        Try
            Enabled = False
            dlg.Show()
            dlg.Owner = Me
            dlg.TopMost = True
            Dim jumItem As Integer = GV1.SelectedRowsCount
            For Each i In GV1.GetSelectedRows
                NoID = CStr(GV1.GetDataRow(i).Item("NoID"))
                Select Case TableMaster.ToUpper
                    Case "MBELI".ToUpper
                        UnPostingStokBarangPembelian(NoID)
                    Case "MJUAL".ToUpper
                        UnPostingStokBarangPenjualan(NoID)
                    Case "MMutasiGudang".ToUpper
                        UnPostingStokBarangMutasiGudang(NoID)
                End Select
                Application.DoEvents()
            Next
        Catch ex As Exception
            XtraMessageBox.Show("Kesalahan : " & ex.Message, "RETAIL SYSTEM", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            dlg.Close()
            dlg.Dispose()
            Enabled = True
        End Try

    End Sub

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
End Class