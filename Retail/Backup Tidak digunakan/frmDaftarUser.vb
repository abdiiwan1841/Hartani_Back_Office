
Imports System.Data.SqlClient
Imports System.Data.SQLite
Imports System.Windows.Forms
Imports DevExpress.XtraEditors
Imports DevExpress.XtraGrid.Views
Imports DevExpress.XtraGrid.Columns
Imports DevExpress.XtraGrid.Views.Base
Imports System.Drawing
Imports DevExpress.XtraEditors.Repository
Imports CitiToys.frmEntriUser

Public Class frmDaftarUser
    Public FormName As String = "Daftar User"
    Public FormEntriName As String = "EntriUser"
    Public TableName As String = "MUser"
    Public NoID As Long = -1
    Dim oda2 As SqlDataAdapter
    Dim ds As New DataSet
    Dim BS As New BindingSource

    Dim repckedit As New RepositoryItemCheckEdit
    Dim repdateedit As New RepositoryItemDateEdit
    Dim reptextedit As New RepositoryItemTextEdit

    Private Sub frmDaftar_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        GV1.SaveLayoutToXml(Application.StartupPath & "\System\Layouts\" & FormName & ".xml")
    End Sub

    Private Sub ctrlDaftar_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        generateform()
        RestoreLayout()
    End Sub

    Private Sub RestoreLayout()
        If System.IO.File.Exists(Application.StartupPath & "\System\Layouts\" & FormName & ".xml") Then
            GV1.RestoreLayoutFromXml(Application.StartupPath & "\System\Layouts\" & FormName & ".xml")
        End If
    End Sub
    Sub generateform()
        Dim cn As New SqlConnection(StrKonSql)
        Dim ocmd2 As New SqlCommand

        'Dim sysSQLconnect As New SQLite.SQLiteConnection()
        'Dim sysSQLcommand As SQLiteCommand
        'Dim sysSQLoda As SQLite.SQLiteDataAdapter
        'sysSQLconnect.ConnectionString = "Data Source=" & Application.StartupPath & "\\System\engine\syssgi.sqlite" & ";"
        'sysSQLconnect.Open()
        'sysSQLcommand = sysSQLconnect.CreateCommand
        'sysSQLcommand.CommandText = "SELECT * FROM sysform where formname='" & FormName & "'"

        ocmd2.Connection = cn
        ocmd2.CommandType = CommandType.Text
        ' TableName = GetTableNamebyFormname(FormName)
        ocmd2.CommandText = IIf(InStr(TableName, "select", CompareMethod.Text) > 0, TableName, "select * from " & TableName)
        cn.Open()
        'sysSQLoda = New SQLite.SQLiteDataAdapter(sysSQLcommand)
        'sysSQLoda.Fill(ds, "Master")

        oda2 = New SqlDataAdapter(ocmd2)
        oda2.Fill(ds, "Data")
        BS.DataSource = ds.Tables("Data")
        GC1.DataSource = BS.DataSource
        For i As Integer = 0 To GV1.Columns.Count - 1
            ' MsgBox(GV1.Columns(i).fieldname.ToString)
            Select Case GV1.Columns(i).ColumnType.Name.ToLower

                Case "int32", "int64", "int"
                    GV1.Columns(i).DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
                    GV1.Columns(i).DisplayFormat.FormatString = "n0"
                Case 2
                Case "decimal", "single", "money", "double"
                    GV1.Columns(i).DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
                    GV1.Columns(i).DisplayFormat.FormatString = "n2"
                Case "string"
                    GV1.Columns(i).DisplayFormat.FormatType = DevExpress.Utils.FormatType.None
                    GV1.Columns(i).DisplayFormat.FormatString = ""
                Case "date", "datetime"
                    GV1.Columns(i).DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime
                    GV1.Columns(i).DisplayFormat.FormatString = "dd-MM-yyyy"
                Case "boolean"
                    GV1.Columns(i).ColumnEdit = repckedit

            End Select

        Next
        'For i = 0 To ds.Tables("Master").Rows.Count - 1

        '    If NullTostr(ds.Tables("Master").Rows(i).Item("fieldname")).Trim = "" Then
        '        Dim unbColumn As GridColumn = GV1.Columns.AddField(NullTostr(ds.Tables("Master").Rows(i).Item("nama")))
        '        unbColumn.VisibleIndex = GV1.Columns.Count
        '        Select Case NullTostr(ds.Tables("Master").Rows(i).Item("Tipe"))

        '            Case "string"
        '                unbColumn.UnboundType = DevExpress.Data.UnboundColumnType.String

        '            Case "int", "bigint", "real", "money", "real", "Decimal", "float"
        '                unbColumn.UnboundType = DevExpress.Data.UnboundColumnType.Decimal
        '                ' Specify format settings.
        '                unbColumn.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
        '                unbColumn.DisplayFormat.FormatString = ds.Tables("Master").Rows(i).Item("format")
        '                unbColumn.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
        '            Case "date", "time", "datetime"
        '                unbColumn.UnboundType = DevExpress.Data.UnboundColumnType.DateTime
        '                ' Specify format settings.
        '                unbColumn.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime
        '                unbColumn.DisplayFormat.FormatString = ds.Tables("Master").Rows(i).Item("format")
        '        End Select

        '        ' Disable editing.
        '        unbColumn.OptionsColumn.AllowEdit = False

        '        ' Customize the appearance settings.
        '        unbColumn.AppearanceCell.BackColor = Color.LemonChiffon
        '    Else
        '        Dim bndColumn As GridColumn = GV1.Columns(NullTostr(ds.Tables("Master").Rows(i).Item("fieldname"))) ' GV1.Columns.AddField(NullTostr(ds.Tables("Master").Rows(i).Item("fieldname")))
        '        bndColumn.Caption = ds.Tables("Master").Rows(i).Item("caption")
        '        bndColumn.Visible = NullTobool(ds.Tables("Master").Rows(i).Item("visible"))
        '        If bndColumn.Visible Then
        '            bndColumn.VisibleIndex = GV1.Columns.Count
        '        End If
        '        ' GV1.Columns.AddField(ds.Tables("Master").Rows(i).Item("fieldname").ToString)
        '        Select Case ds.Tables("Master").Rows(i).Item("control")
        '            Case "checkedit"
        '                bndColumn.ColumnEdit = repckedit
        '            Case "textedit"
        '                bndColumn.ColumnEdit = reptextedit
        '            Case "dateedit"
        '                repdateedit.Mask.EditMask = ds.Tables("Master").Rows(i).Item("format").ToString
        '                repdateedit.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.DateTime
        '                repdateedit.Mask.UseMaskAsDisplayFormat = True
        '                bndColumn.ColumnEdit = repdateedit
        '            Case "lookupedit"
        '            Case "string"
        '                bndColumn.DisplayFormat.FormatType = DevExpress.Utils.FormatType.None
        '            Case "numeric"
        '                bndColumn.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
        '                bndColumn.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
        '                bndColumn.DisplayFormat.FormatString = ds.Tables("Master").Rows(i).Item("format")
        '            Case "int", "bigint", "real", "money", "real", "Decimal", "float"
        '                bndColumn.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
        '                bndColumn.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
        '                bndColumn.DisplayFormat.FormatString = ds.Tables("Master").Rows(i).Item("format")
        '            Case "date"
        '                bndColumn.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime
        '                bndColumn.DisplayFormat.FormatString = ds.Tables("Master").Rows(i).Item("format")
        '        End Select
        '    End If

        'Next
        ocmd2.Dispose()
        cn.Close()
        cn.Dispose()

    End Sub

    Private Sub SimpleButton6_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SimpleButton6.Click
        Tutup()
    End Sub

    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

    End Sub


    Private Sub SimpleButton1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SimpleButton1.Click
        Baru()
    End Sub

    Private Sub SimpleButton2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SimpleButton2.Click
        Edit()
        
    End Sub

    Private Sub SimpleButton3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SimpleButton3.Click
        Hapus()
    End Sub

    Sub Baru()
        Dim x As New frmEntriUser
        x.FormName = FormEntriName
        x.isNew = True
        If x.ShowDialog() = Windows.Forms.DialogResult.OK Then
            RefreshData()
            GV1.FocusedRowHandle = GV1.LocateByDisplayText(0, GV1.Columns("NoID"), NoID.ToString("#,##0"))
        End If
        x.Dispose()
    End Sub
    Sub RefreshData()
        Dim dlg As DevExpress.Utils.WaitDialogForm = Nothing
        Dim cn As New SqlConnection(StrKonSql)
        Dim ocmd2 As New SqlCommand
        Dim Cur As Cursor = Windows.Forms.Cursor.Current
        Try
            Dim curentcursor As Cursor = Windows.Forms.Cursor.Current
            Windows.Forms.Cursor.Current = Cursors.WaitCursor
            dlg = New DevExpress.Utils.WaitDialogForm(String.Format("Sedang merefresh data.{0}MOHON TUNGGU ...", vbCrLf), Application.ProductName)
            dlg.TopMost = False
            dlg.Show()
            Windows.Forms.Cursor.Current = Cursors.WaitCursor

            Dim strsql As String = ""
            Dim SQL As String = ""
            Dim dsT2 As New DataSet
            If TableMaster.ToLower = "mlpb" Then
                strsql = IIf(InStr(TableName, "select", CompareMethod.Text) > 0, TableName, "select * from " & TableName)
            Else
                strsql = IIf(InStr(TableName, "select", CompareMethod.Text) > 0, TableName, "select * from " & TableName)
            End If
            If TglDari.Enabled Then
                If InStr(strsql.ToLower, "where", CompareMethod.Text) > 0 Then
                    strsql = strsql & " and (" & TableMaster & ".Tanggal>='" & Format(TglDari.DateTime, "yyyy/MM/dd") & "' and " & TableMaster & ".Tanggal<'" & Format(DateAdd(DateInterval.Day, 1, TglSampai.DateTime), "yyyy/MM/dd") & "') "
                Else
                    strsql = strsql & " where (" & TableMaster & ".Tanggal>='" & Format(TglDari.DateTime, "yyyy/MM/dd") & "' and " & TableMaster & ".Tanggal<'" & Format(DateAdd(DateInterval.Day, 1, TglSampai.DateTime), "yyyy/MM/dd") & "') "
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
                                .Columns(i).DisplayFormat.FormatString = "hh:mm"
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
                    'If .Columns(i).FieldName.Length >= 4 AndAlso .Columns(i).FieldName.Substring(0, 4).ToLower = "Kode".ToLower Then
                    '    .Columns(i).Fixed = FixedStyle.Left
                    'ElseIf (.Columns(i).FieldName.Length >= 3 AndAlso .Columns(i).FieldName.Substring(0, 3).ToLower = "Qty".ToLower) Or .Columns(i).FieldName.Length >= 5 AndAlso .Columns(i).FieldName.Substring(0, 5).ToLower = "Total".ToLower Then
                    '    .Columns(i).GroupFormat.FormatType = FormatType.Numeric
                    '    .Columns(i).GroupFormat.FormatString = "{0:n2}"
                    'ElseIf .Columns(i).FieldName.ToLower = "Nama".ToLower Then
                    '    .Columns(i).Fixed = FixedStyle.Left
                    'End If
                Next
            End With


            If TableMaster.ToUpper = "MSO".ToUpper Then
                mnGenerateSPK.Visibility = DevExpress.XtraBars.BarItemVisibility.Always
            ElseIf TableMaster.ToUpper = "MPO".ToUpper Then
                mnSetPOSelesai.Visibility = DevExpress.XtraBars.BarItemVisibility.Always
                mnSetPOBelumSelesai.Visibility = DevExpress.XtraBars.BarItemVisibility.Always
            Else
                mnSetPOBelumSelesai.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
                mnSetPOSelesai.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
                mnGenerateSPK.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
            End If

            If TableMaster.ToUpper = "MBeli".ToUpper Then
                SQL = "SELECT MPOD.NoID, MPO.Kode AS NoPO, MGudang.Nama AS NamaGudang, MWilayah.Nama AS NamaWilayah, MBarang.Kode AS KodeBarang, MAlamat.Kode AS KodeSupplier, MAlamat.Nama AS NamaSupplier, MBarang.Nama AS NamaBarang, "
                SQL &= " (MPOD.Harga-isnull(MPOD.DISC1,0)-isnull(MPOD.DISC2,0)-isnull(MPOD.DISC3,0)"
                SQL &= " -(MPOD.Harga*isnull(MPOD.DiscPersen1,0)/100)"
                SQL &= " -(MPOD.Harga*isnull(MPOD.DiscPersen2,0)/100)"
                SQL &= " -(MPOD.Harga*isnull(MPOD.DiscPersen3,0)/100)"
                SQL &= " ) AS Harga , MPOD.Qty, MSatuan.Nama AS Satuan, MPOD.HargaPcs AS [Harga(Pcs)], MPOD.Qty*MPOD.Konversi AS [Qty(Pcs)], MPOD.Qty*MPOD.Konversi- " & DiBeli() & "  AS [Sisa(Pcs)]"
                SQL &= " FROM MPOD LEFT JOIN MSatuan ON MSatuan.NoID=MPOD.IDSatuan "
                SQL &= " LEFT JOIN (MPO LEFT JOIN MAlamat ON MAlamat.NoID=MPO.IDSupplier) ON MPO.NoID=MPOD.IDPO "
                SQL &= " LEFT JOIN MBarang ON MBarang.NoID=MPOD.IDBarang "
                SQL &= " LEFT JOIN (MGudang LEFT JOIN MWilayah ON MWilayah.NoID=MGudang.IDWilayah) ON MGudang.NoID=MPOD.IDGudang "
                SQL &= " WHERE (MPO.IsSelesai=0 OR MPO.IsSelesai IS NULL) AND (MPOD.Qty*MPOD.Konversi - " & DiBeli() & ">0) AND MBarang.IsActive = 1 And MPO.IsPosted = 1 "
                dsT2 = ExecuteDataset("MPOD", SQL)
                GridControl1.DataSource = dsT2.Tables("MPOD")
                With GridView1
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
                                    .Columns(i).DisplayFormat.FormatString = "hh:mm"
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
                        'If .Columns(i).FieldName.Length >= 4 AndAlso .Columns(i).FieldName.Substring(0, 4).ToLower = "Kode".ToLower Then
                        '    .Columns(i).Fixed = FixedStyle.Left
                        'ElseIf (.Columns(i).FieldName.Length >= 3 AndAlso .Columns(i).FieldName.Substring(0, 3).ToLower = "Qty".ToLower) Or .Columns(i).FieldName.Length >= 5 AndAlso .Columns(i).FieldName.Substring(0, 5).ToLower = "Total".ToLower Then
                        '    .Columns(i).GroupFormat.FormatType = FormatType.Numeric
                        '    .Columns(i).GroupFormat.FormatString = "{0:n2}"
                        'ElseIf .Columns(i).FieldName.ToLower = "Nama".ToLower Then
                        '    .Columns(i).Fixed = FixedStyle.Left
                        'End If
                    Next
                End With

                SQL = "SELECT MLPBD.NoID, MGudang.Nama AS NamaGudang, MWilayah.Nama AS NamaWilayah, MBarang.Kode AS KodeBarang, MBarang.Nama AS NamaBarang, MLPB.Kode AS NoLPB, MLPB.Tanggal, MAlamat.Nama AS Supplier, MBagPembelian.Nama AS Penerima, MLPBD.Qty * MLPBD.Konversi AS [QtyPcs], MLPBD.Qty*MLPBD.Konversi-" & DiSTB() & "  AS [Sisa(Pcs)] "
                SQL &= " FROM (MLPBD LEFT OUTER JOIN (MLPB LEFT OUTER JOIN MAlamat AS MBagPembelian ON MBagPembelian.NoID = MLPB.IDBagPembelian "
                SQL &= " LEFT OUTER JOIN MAlamat ON MLPB.IDSupplier = MAlamat.NoID)"
                SQL &= " ON MLPB.NoID=MLPBD.IDLPB) "
                SQL &= " LEFT OUTER JOIN MBarang ON MLPBD.IDBarang = MBarang.NoID "
                SQL &= " LEFT OUTER JOIN (MGudang LEFT OUTER JOIN MWilayah ON MWilayah.NoID=MGudang.IDWilayah) ON MGudang.NoID=MLPBD.IDGudang "
                SQL &= " WHERE MBarang.ISActive=1 AND (MLPBD.Qty*MLPBD.Konversi-" & DiSTB() & ">0) And MLPB.IsPosted = 1 "

                dsT2 = ExecuteDataset("MLPBD", SQL)
                GridControl2.DataSource = dsT2.Tables("MLPBD")
                With GridView2
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
                                    .Columns(i).DisplayFormat.FormatString = "hh:mm"
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
                        'If .Columns(i).FieldName.Length >= 4 AndAlso .Columns(i).FieldName.Substring(0, 4).ToLower = "Kode".ToLower Then
                        '    .Columns(i).Fixed = FixedStyle.Left
                        'ElseIf (.Columns(i).FieldName.Length >= 3 AndAlso .Columns(i).FieldName.Substring(0, 3).ToLower = "Qty".ToLower) Or .Columns(i).FieldName.Length >= 5 AndAlso .Columns(i).FieldName.Substring(0, 5).ToLower = "Total".ToLower Then
                        '    .Columns(i).GroupFormat.FormatType = FormatType.Numeric
                        '    .Columns(i).GroupFormat.FormatString = "{0:n2}"
                        'ElseIf .Columns(i).FieldName.ToLower = "Nama".ToLower Then
                        '    .Columns(i).Fixed = FixedStyle.Left
                        'End If
                    Next
                End With

            Else
                XtraTabPage2.PageVisible = False
            End If
        Catch ex As Exception
            XtraMessageBox.Show("Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            ocmd2.Dispose()
            cn.Close()
            cn.Dispose()
            Windows.Forms.Cursor.Current = Cur
            Windows.Forms.Cursor.Current = Cursors.Default
            dlg.Close()
            dlg.Dispose()
        End Try
    End Sub
    Sub Edit()
        Dim x As New frmEntriUser
        Dim view As ColumnView = GC1.FocusedView
        Try
            Dim row As System.Data.DataRow = view.GetDataRow(GV1.FocusedRowHandle)
            Dim dc As Integer = GV1.FocusedRowHandle
            Dim NoID As Long = NullTolong(row("NoID"))
            x.FormName = FormEntriName
            x.isNew = False
            x.NoID = NoID
            If x.ShowDialog() = Windows.Forms.DialogResult.OK Then
                RefreshData()
                GV1.ClearSelection()
                GV1.FocusedRowHandle = GV1.LocateByDisplayText(0, GV1.Columns("NoID"), NoID.ToString("#,##0"))
                GV1.SelectRow(GV1.FocusedRowHandle)
            End If
        Catch ex As Exception
            MsgBox("Untuk mengedit pilih data yang akan diedit terlebih dahulu lalu tekan tombol edit", MessageBoxButtons.OK + MessageBoxIcon.Error)
        Finally
            x.Dispose()
        End Try
    End Sub
    Sub Hapus()
        Dim SQL As String = ""
        Try
            If XtraMessageBox.Show("Yakin mau hapus data " & FormName & " dengan noid " & GV1.GetRowCellValue(GV1.FocusedRowHandle, "Nama").ToString) Then
                SQL = "DELETE FROM MUser WHERE NoID=" & NullTolong(GV1.GetRowCellValue(GV1.FocusedRowHandle, "NoID"))
                EksekusiSQL(SQL)
            End If
        Catch ex As Exception
            XtraMessageBox.Show("Pilih item terlebih dahulu, lalu click Hapus.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
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

    Private Sub SimpleButton4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SimpleButton4.Click
        RefreshData()
    End Sub

    Private Sub SimpleButton7_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SimpleButton7.Click
        GC1.ShowPrintPreview()
    End Sub

    Private Sub mnPreview_ItemClick(ByVal sender As Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles mnPreview.ItemClick
        SimpleButton7.PerformClick()
    End Sub

    Private Sub mnRefresh_ItemClick(ByVal sender As Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles mnRefresh.ItemClick
        SimpleButton4.PerformClick()
    End Sub
End Class