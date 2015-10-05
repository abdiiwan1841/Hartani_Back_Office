
Imports System.Data.SqlClient
Imports DevExpress.XtraEditors
Imports DevExpress.XtraGrid.Columns
Imports DevExpress.XtraGrid.Views.Base
Imports DevExpress.XtraEditors.Repository
Imports DevExpress.XtraGrid.Views.Grid.ViewInfo
Imports DevExpress.XtraGrid.Views.Grid
Imports DevExpress.Utils

Public Class frmDaftarAlamat
    Public TableNameD As String = ""
    Public TableMaster As String = ""
    Public NoID As Long = -1
    Dim oda2 As SqlDataAdapter
    Dim ds As New DataSet
    Dim BS As New BindingSource
    Dim frmImage As frmShowImage

    Dim repckedit As New RepositoryItemCheckEdit
    Dim repdateedit As New RepositoryItemDateEdit
    Dim reptextedit As New RepositoryItemTextEdit
    Dim reppicedit As New RepositoryItemPictureEdit

    Public ShowNoID As Boolean = False
    Public DirectNoID As Long = -1
    Dim frmlookup As New frmLookUpKartuStok
    Public IsShowStock As Boolean = False

    Private Sub frmDaftarAlamat_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Activated
        If ShowNoID Then
            RefreshData()
            If XtraTabControl1.SelectedTabPageIndex = 0 Then
                GV1.ClearSelection()
                GV1.FocusedRowHandle = GV1.LocateByDisplayText(0, GV1.Columns("NoID"), (DirectNoID).ToString("#,##0"))
                GV1.SelectRow(GV1.FocusedRowHandle)
            ElseIf XtraTabControl1.SelectedTabPageIndex = 1 Then
                GridView1.ClearSelection()
                GridView1.FocusedRowHandle = GridView1.LocateByDisplayText(0, GridView1.Columns("NoID"), (DirectNoID).ToString("#,##0"))
                GridView1.SelectRow(GridView1.FocusedRowHandle)
            ElseIf XtraTabControl1.SelectedTabPageIndex = 2 Then
                GridView2.ClearSelection()
                GridView2.FocusedRowHandle = GridView2.LocateByDisplayText(0, GridView2.Columns("NoID"), (DirectNoID).ToString("#,##0"))
                GridView2.SelectRow(GridView2.FocusedRowHandle)
            ElseIf XtraTabControl1.SelectedTabPageIndex = 3 Then
                GridView3.ClearSelection()
                GridView3.FocusedRowHandle = GridView3.LocateByDisplayText(0, GridView3.Columns("NoID"), (DirectNoID).ToString("#,##0"))
                GridView3.SelectRow(GridView3.FocusedRowHandle)
            End If
            ShowNoID = False
            'If KirimDataKassa() Then
            '    XtraMessageBox.Show("Pengiriman berhasil.", NamaAplikasi, MessageBoxButtons.OK)
            'End If
        End If
    End Sub

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
        If System.IO.File.Exists(FolderLayouts & Me.Name & IDUserAktif & ".xml") Then
            GV1.RestoreLayoutFromXml(FolderLayouts & Me.Name & IDUserAktif & ".xml")
        Else
            If System.IO.File.Exists(FolderLayouts & Me.Name & ".xml") Then
                GV1.RestoreLayoutFromXml(FolderLayouts & Me.Name & ".xml")
            End If
        End If

        If System.IO.File.Exists(FolderLayouts & Me.Name & IDUserAktif & GridView1.Name & ".xml") Then
            GridView1.RestoreLayoutFromXml(FolderLayouts & Me.Name & IDUserAktif & GridView1.Name & ".xml")
        Else
            If System.IO.File.Exists(FolderLayouts & Me.Name & ".xml") Then
                GridView1.RestoreLayoutFromXml(FolderLayouts & Me.Name & ".xml")
            End If
        End If

        If System.IO.File.Exists(FolderLayouts & Me.Name & IDUserAktif & GridView2.Name & ".xml") Then
            GridView2.RestoreLayoutFromXml(FolderLayouts & Me.Name & IDUserAktif & GridView2.Name & ".xml")
        Else
            If System.IO.File.Exists(FolderLayouts & Me.Name & ".xml") Then
                GridView2.RestoreLayoutFromXml(FolderLayouts & Me.Name & ".xml")
            End If
        End If

        If System.IO.File.Exists(FolderLayouts & Me.Name & IDUserAktif & GridView3.Name & ".xml") Then
            GridView3.RestoreLayoutFromXml(FolderLayouts & Me.Name & IDUserAktif & GridView3.Name & ".xml")
        Else
            If System.IO.File.Exists(FolderLayouts & Me.Name & ".xml") Then
                GridView3.RestoreLayoutFromXml(FolderLayouts & Me.Name & ".xml")
            End If
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
        Dim Kontak As New frmEntriKontak() With {.FormName = "EntriAlamat", .isNew = False, .MdiParent = Me.MdiParent, .WindowState = FormWindowState.Normal}
        If XtraTabControl1.SelectedTabPageIndex = 1 Then
            Kontak.IsSupplier = True
            Kontak.NoID = NullToLong(GridView1.GetRowCellValue(GridView1.FocusedRowHandle, "NoID"))
        ElseIf XtraTabControl1.SelectedTabPageIndex = 2 Then
            Kontak.IsPegawai = True
            Kontak.NoID = NullToLong(GridView2.GetRowCellValue(GridView2.FocusedRowHandle, "NoID"))
        ElseIf XtraTabControl1.SelectedTabPageIndex = 3 Then
            Kontak.IsMember = True
            Kontak.NoID = NullToLong(GridView3.GetRowCellValue(GridView3.FocusedRowHandle, "NoID"))
        Else
            Kontak.NoID = NullToLong(GV1.GetRowCellValue(GV1.FocusedRowHandle, "NoID"))
        End If
        Kontak.Show()
        Kontak.Focus()
        'If Kontak.ShowDialog(me) = Windows.Forms.DialogResult.OK Then
        '    RefreshData()
        '    GV1.ClearSelection()
        '    GV1.FocusedRowHandle = GV1.LocateByDisplayText(0, GV1.Columns("NoID"), NoID.ToString("#,##0"))
        '    GV1.SelectRow(GV1.FocusedRowHandle)
        'End If
        'Kontak.Dispose()
    End Sub

    Private Sub SimpleButton3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdHapus.Click
        Hapus()
    End Sub

    Private Sub SimpleButton1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SimpleButton1.Click
        Dim Kontak As New frmEntriKontak() With {.FormName = "EntriAlamat", .isNew = True, .MdiParent = Me.MdiParent, .WindowState = FormWindowState.Normal}
        If XtraTabControl1.SelectedTabPageIndex = 1 Then
            Kontak.IsSupplier = True
        ElseIf XtraTabControl1.SelectedTabPageIndex = 2 Then
            Kontak.IsPegawai = True
        ElseIf XtraTabControl1.SelectedTabPageIndex = 3 Then
            Kontak.IsMember = True
        End If
        Kontak.Show()
        Kontak.Focus()

        'If Kontak.ShowDialog(me) = Windows.Forms.DialogResult.OK Then
        '    RefreshData()
        '    GV1.FocusedRowHandle = GV1.LocateByDisplayText(0, GV1.Columns("NoID"), Kontak.NoID.ToString("#,##0"))
        'End If
        'Kontak.Dispose()
    End Sub
    Sub RefreshData(Optional ByVal StrCari As String = "")
        Dim dlg As DevExpress.Utils.WaitDialogForm = Nothing
        Dim odsT2 As New DataSet
        Try
            Dim curentcursor As Cursor = Windows.Forms.Cursor.Current
            Windows.Forms.Cursor.Current = Cursors.WaitCursor
            dlg = New DevExpress.Utils.WaitDialogForm(String.Format("Sedang merefresh data.{0}MOHON TUNGGU ...", vbCrLf), Application.ProductName)
            dlg.TopMost = False
            dlg.Show()
            Windows.Forms.Cursor.Current = Cursors.WaitCursor

            Dim strsql As String

            strsql = "select malamat.*, (SELECT TOP 1 MGroupSupplier.Kode FROM MGroupSupplier INNER JOIN MGroupSupplierD ON MGroupSupplier.NoID=MGroupSupplierD.IDGroupSupplier WHERE MGroupSupplier.IsActive=1 AND MGroupSupplierD.IDAlamat=MAlamat.NoID) AS GroupSupplier, MAkunHutang.Kode + ' - ' + MAkunHutang.Nama AS AkunHutang, MAkunPiutang.Kode + ' - ' + MAkunPiutang.Nama AS AkunPiutang, NamaRekening1 AS [BG Atas Nama], MKota.Nama as Kota,MTypeCustomer.Nama as TipeMember from malamat left join Mkota on MAlamat.IDKota=Mkota.NoID left join MTypeCustomer on MAlamat.IDTypeCustomer=MTypeCustomer.NoID" & vbCrLf & _
                     " LEFT JOIN MAkun MAkunHutang ON MAkunHutang.ID=MAlamat.IDAkunHutang " & _
                     " LEFT JOIN MAkun MAkunPiutang ON MAkunPiutang.ID=MAlamat.IDAkunPiutang "
            If Not ckAll.Checked Then
                strsql &= " WHERE MAlamat.IsActive=1 "
            Else
                strsql &= " WHERE 1=1 "
            End If
            If StrCari <> "" Then
                strsql &= " AND (UPPER(MAlamat.Kode) LIKE '" & FixApostropi(StrCari.ToUpper) & "%' OR UPPER(MAlamat.Nama) LIKE '" & FixApostropi(StrCari.ToUpper) & "%' OR UPPER(MAlamat.Alamat) LIKE '" & FixApostropi(StrCari.ToUpper) & "%') "
            End If

            odsT2 = ExecuteDataset("vBrg", strsql)

            If ds.Tables("vBrg") Is Nothing Then
            Else
                ds.Tables("vBrg").Clear()
            End If
            ds = odsT2
            BS.DataSource = ds.Tables("vBrg")
            GC1.DataSource = odsT2.Tables("vBrg")

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

            'Supplier
            odsT2 = ExecuteDataset("vBrg", strsql & " AND MAlamat.IsSupplier=1 ")
            GridControl1.DataSource = odsT2.Tables("vBrg")

            For i As Integer = 0 To GridView1.Columns.Count - 1
                Select Case GridView1.Columns(i).ColumnType.Name.ToLower
                    Case "int32", "int64", "int"
                        GridView1.Columns(i).DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
                        GridView1.Columns(i).DisplayFormat.FormatString = "n0"
                    Case "decimal", "single", "money", "double"
                        GridView1.Columns(i).DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
                        GridView1.Columns(i).DisplayFormat.FormatString = "n2"
                    Case "string"
                        GridView1.Columns(i).DisplayFormat.FormatType = DevExpress.Utils.FormatType.None
                        GridView1.Columns(i).DisplayFormat.FormatString = ""
                    Case "date"
                        GridView1.Columns(i).DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime
                        GridView1.Columns(i).DisplayFormat.FormatString = "dd-MM-yyyy"
                    Case "datetime"
                        GridView1.Columns(i).DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime
                        GridView1.Columns(i).DisplayFormat.FormatString = "dd-MM-yyyy HH:mm"
                    Case "byte[]"
                        reppicedit.SizeMode = DevExpress.XtraEditors.Controls.PictureSizeMode.Squeeze
                        GridView1.Columns(i).OptionsColumn.AllowGroup = False
                        GridView1.Columns(i).OptionsColumn.AllowSort = False
                        GridView1.Columns(i).OptionsFilter.AllowFilter = False
                        GridView1.Columns(i).ColumnEdit = reppicedit
                    Case "boolean"
                        GridView1.Columns(i).ColumnEdit = repckedit
                End Select
                If GridView1.Columns(i).FieldName.Length >= 4 AndAlso GridView1.Columns(i).FieldName.Substring(0, 4).ToLower = "Kode".ToLower Then
                    GridView1.Columns(i).Fixed = FixedStyle.Left
                ElseIf GV1.Columns(i).FieldName.ToLower = "Nama".ToLower Then
                    GridView1.Columns(i).Fixed = FixedStyle.Left
                End If
            Next

            'Pegawai
            odsT2 = ExecuteDataset("vBrg", strsql & " AND MAlamat.IsPegawai=1 ")
            GridControl2.DataSource = odsT2.Tables("vBrg")

            For i As Integer = 0 To GridView2.Columns.Count - 1
                Select Case GridView2.Columns(i).ColumnType.Name.ToLower
                    Case "int32", "int64", "int"
                        GridView2.Columns(i).DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
                        GridView2.Columns(i).DisplayFormat.FormatString = "n0"
                    Case "decimal", "single", "money", "double"
                        GridView2.Columns(i).DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
                        GridView2.Columns(i).DisplayFormat.FormatString = "n2"
                    Case "string"
                        GridView2.Columns(i).DisplayFormat.FormatType = DevExpress.Utils.FormatType.None
                        GridView2.Columns(i).DisplayFormat.FormatString = ""
                    Case "date"
                        GridView2.Columns(i).DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime
                        GridView2.Columns(i).DisplayFormat.FormatString = "dd-MM-yyyy"
                    Case "datetime"
                        GridView2.Columns(i).DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime
                        GridView2.Columns(i).DisplayFormat.FormatString = "dd-MM-yyyy HH:mm"
                    Case "byte[]"
                        reppicedit.SizeMode = DevExpress.XtraEditors.Controls.PictureSizeMode.Squeeze
                        GridView2.Columns(i).OptionsColumn.AllowGroup = False
                        GridView2.Columns(i).OptionsColumn.AllowSort = False
                        GridView2.Columns(i).OptionsFilter.AllowFilter = False
                        GridView2.Columns(i).ColumnEdit = reppicedit
                    Case "boolean"
                        GridView2.Columns(i).ColumnEdit = repckedit
                End Select
                If GridView2.Columns(i).FieldName.Length >= 4 AndAlso GridView2.Columns(i).FieldName.Substring(0, 4).ToLower = "Kode".ToLower Then
                    GridView2.Columns(i).Fixed = FixedStyle.Left
                ElseIf GV1.Columns(i).FieldName.ToLower = "Nama".ToLower Then
                    GridView2.Columns(i).Fixed = FixedStyle.Left
                End If
            Next

            'Customer
            odsT2 = ExecuteDataset("vBrg", strsql & " AND MAlamat.IsCustomer=1 ")
            GridControl3.DataSource = odsT2.Tables("vBrg")

            For i As Integer = 0 To GridView3.Columns.Count - 1
                Select Case GridView3.Columns(i).ColumnType.Name.ToLower
                    Case "int32", "int64", "int"
                        GridView3.Columns(i).DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
                        GridView3.Columns(i).DisplayFormat.FormatString = "n0"
                    Case "decimal", "single", "money", "double"
                        GridView3.Columns(i).DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
                        GridView3.Columns(i).DisplayFormat.FormatString = "n2"
                    Case "string"
                        GridView3.Columns(i).DisplayFormat.FormatType = DevExpress.Utils.FormatType.None
                        GridView3.Columns(i).DisplayFormat.FormatString = ""
                    Case "date"
                        GridView3.Columns(i).DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime
                        GridView3.Columns(i).DisplayFormat.FormatString = "dd-MM-yyyy"
                    Case "datetime"
                        GridView3.Columns(i).DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime
                        GridView3.Columns(i).DisplayFormat.FormatString = "dd-MM-yyyy HH:mm"
                    Case "byte[]"
                        reppicedit.SizeMode = DevExpress.XtraEditors.Controls.PictureSizeMode.Squeeze
                        GridView3.Columns(i).OptionsColumn.AllowGroup = False
                        GridView3.Columns(i).OptionsColumn.AllowSort = False
                        GridView3.Columns(i).OptionsFilter.AllowFilter = False
                        GridView3.Columns(i).ColumnEdit = reppicedit
                    Case "boolean"
                        GridView3.Columns(i).ColumnEdit = repckedit
                End Select
                If GridView3.Columns(i).FieldName.Length >= 4 AndAlso GridView3.Columns(i).FieldName.Substring(0, 4).ToLower = "Kode".ToLower Then
                    GridView3.Columns(i).Fixed = FixedStyle.Left
                ElseIf GV1.Columns(i).FieldName.ToLower = "Nama".ToLower Then
                    GridView3.Columns(i).Fixed = FixedStyle.Left
                End If
            Next
            GV1.ShowFindPanel()
            GridView1.ShowFindPanel()
            GridView2.ShowFindPanel()
            GridView3.ShowFindPanel()
            txtSearch.Text = ""
            If IsSupervisor Then
                cmdHapus.Enabled = True
            Else
                cmdHapus.Enabled = False
            End If
        Catch ex As Exception
            XtraMessageBox.Show(ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            Windows.Forms.Cursor.Current = Cursors.Default
            odsT2.Dispose()
            dlg.Close()
            dlg.Dispose()
        End Try
    End Sub
    Sub Hapus()
        Dim view As ColumnView = GC1.FocusedView
        Try
            Dim row As System.Data.DataRow
            If XtraTabControl1.SelectedTabPageIndex = 1 Then
                view = GridControl1.FocusedView
                row = view.GetDataRow(GridView1.FocusedRowHandle)
            ElseIf XtraTabControl1.SelectedTabPageIndex = 2 Then
                view = GridControl2.FocusedView
                row = view.GetDataRow(GridView2.FocusedRowHandle)
            ElseIf XtraTabControl1.SelectedTabPageIndex = 3 Then
                view = GridControl3.FocusedView
                row = view.GetDataRow(GridView3.FocusedRowHandle)
            Else
                row = view.GetDataRow(GV1.FocusedRowHandle)
            End If

            Dim NoID As Long = NullToLong(row("NoID"))
            If XtraMessageBox.Show(Me, "Yakin mau hapus data " & Me.Text & " dengan Kode " & NullToStr(row("Kode")), NamaAplikasi, MessageBoxButtons.YesNo) = Windows.Forms.DialogResult.Yes Then
                EksekusiSQL("Update MAlamat set IsActive=0 where NoID= " & NoID.ToString)
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

    Private Sub BarButtonItem1_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem1.ItemClick
        SimpleButton1.PerformClick()
    End Sub

    Private Sub BarButtonItem2_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem2.ItemClick
        SimpleButton2.PerformClick()
    End Sub

    Private Sub BarButtonItem3_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem3.ItemClick
        cmdHapus.PerformClick()
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
        Dim dlgsave As New SaveFileDialog() With {.Title = "Export Daftar ke Excel", .Filter = "Excel Files|*.xls"}
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
        Try
            frmlookup.Dispose()
            frmlookup = New frmLookUpKartuStok() With {.StartPosition = FormStartPosition.WindowsDefaultLocation, .WindowState = FormWindowState.Normal, .IDSupplier = NullToLong(GV1.GetRowCellValue(GV1.FocusedRowHandle, "NoID")), .LookUp = frmLookUpKartuStok.JenisLookUp.HistoryBarang, .TopMost = True, .ParentMDIForm = Me.MdiParent}
            IsShowStock = True
            frmlookup.Show()
        Catch ex As Exception
            XtraMessageBox.Show("Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub
    Private Sub ShowStock()
        Try
            If IsShowStock AndAlso Not frmlookup Is Nothing Then
                frmlookup.txtBarang.EditValue = NullToLong(GV1.GetRowCellValue(GV1.FocusedRowHandle, "NoID"))
                'frmlookup.RefreshData()
            End If
        Catch ex As Exception
            XtraMessageBox.Show("Info : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
        End Try
    End Sub

    Private Sub GV1_FocusedRowChanged(ByVal sender As Object, ByVal e As DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs) Handles GV1.FocusedRowChanged
        ShowStock()
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

    Private Sub mnHistory_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles mnHistory.ItemClick
        cmdFaktur.PerformClick()
    End Sub

    Private Sub mnRefresh_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles mnRefresh.ItemClick
        SimpleButton8.PerformClick()
    End Sub

    Private Sub mnSaveLayouts_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles mnSaveLayouts.ItemClick
        Dim xOtorisasi As New frmOtorisasiAdmin
        Try
            If XtraMessageBox.Show("Simpan Layout?", NamaAplikasi, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = Windows.Forms.DialogResult.Yes AndAlso xOtorisasi.Showdialog(Me) = Windows.Forms.DialogResult.OK Then
                GV1.SaveLayoutToXml(FolderLayouts & Me.Name & IDUserAktif & ".xml")
                GridView1.SaveLayoutToXml(FolderLayouts & Me.Name & IDUserAktif & GridView1.Name & ".xml")
                GridView2.SaveLayoutToXml(FolderLayouts & Me.Name & IDUserAktif & GridView2.Name & ".xml")
                GridView3.SaveLayoutToXml(FolderLayouts & Me.Name & IDUserAktif & GridView3.Name & ".xml")
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
    Sub SetAkun(ByVal nmfield As String)
        Dim frmlookup As New frmLUAkun
        Dim IDAlamat As Long = -1
        Try
            If frmlookup.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
                Dim dlg As New WaitDialogForm("Proses sedang dilakukan...", "Mohon Tunggu Sebentar.")
                dlg.Show()
                dlg.Owner = Me
                dlg.TopMost = True
                If XtraTabControl1.SelectedTabPageIndex = 0 Then
                    For Each i In GV1.GetSelectedRows
                        IDAlamat = NullToLong(GV1.GetDataRow(i).Item("NoID"))
                        EksekusiSQL("UPDATE MAlamat SET " & nmfield & "=" & NullToLong(frmlookup.NoID) & " WHERE NoID=" & IDAlamat)
                    Next
                ElseIf XtraTabControl1.SelectedTabPageIndex = 1 Then
                    For Each i In GridView1.GetSelectedRows
                        IDAlamat = NullToLong(GridView1.GetDataRow(i).Item("NoID"))
                        EksekusiSQL("UPDATE MAlamat SET " & nmfield & "=" & NullToLong(frmlookup.NoID) & " WHERE NoID=" & IDAlamat)
                    Next
                ElseIf XtraTabControl1.SelectedTabPageIndex = 2 Then
                    For Each i In GridView2.GetSelectedRows
                        IDAlamat = NullToLong(GridView2.GetDataRow(i).Item("NoID"))
                        EksekusiSQL("UPDATE MAlamat SET " & nmfield & "=" & NullToLong(frmlookup.NoID) & " WHERE NoID=" & IDAlamat)
                    Next
                ElseIf XtraTabControl1.SelectedTabPageIndex = 3 Then
                    For Each i In GridView3.GetSelectedRows
                        IDAlamat = NullToLong(GridView3.GetDataRow(i).Item("NoID"))
                        EksekusiSQL("UPDATE MAlamat SET " & nmfield & "=" & NullToLong(frmlookup.NoID) & " WHERE NoID=" & IDAlamat)
                    Next
                End If
                RefreshData()
                If XtraTabControl1.SelectedTabPageIndex = 0 Then
                    GV1.ClearSelection()
                    GV1.FocusedRowHandle = GV1.LocateByDisplayText(0, GV1.Columns("NoID"), IDAlamat)
                    GV1.SelectRow(GV1.FocusedRowHandle)
                ElseIf XtraTabControl1.SelectedTabPageIndex = 1 Then
                    GridView1.ClearSelection()
                    GridView1.FocusedRowHandle = GridView1.LocateByDisplayText(0, GridView1.Columns("NoID"), IDAlamat)
                    GridView1.SelectRow(GridView1.FocusedRowHandle)
                ElseIf XtraTabControl1.SelectedTabPageIndex = 2 Then
                    GridView2.ClearSelection()
                    GridView2.FocusedRowHandle = GridView2.LocateByDisplayText(0, GridView2.Columns("NoID"), IDAlamat)
                    GridView2.SelectRow(GridView2.FocusedRowHandle)
                ElseIf XtraTabControl1.SelectedTabPageIndex = 3 Then
                    GridView3.ClearSelection()
                    GridView3.FocusedRowHandle = GridView3.LocateByDisplayText(0, GridView3.Columns("NoID"), IDAlamat)
                    GridView3.SelectRow(GridView3.FocusedRowHandle)
                End If
                dlg.Close()
                dlg.Dispose()
            End If
        Catch ex As Exception
            XtraMessageBox.Show("Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK)
        Finally
            frmlookup.Dispose()
        End Try
    End Sub
    Private Sub BarButtonItem7_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem7.ItemClick
        SetAkun("IDAkunHutang")
    End Sub

    Private Sub GridView1_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles GridView1.MouseDown
        Dim View As GridView = CType(sender, GridView)
        If View Is Nothing Then Return
        ' obtaining hit info
        Dim hitInfo As GridHitInfo = View.CalcHitInfo(New System.Drawing.Point(e.X, e.Y))
        If (e.Button = Windows.Forms.MouseButtons.Right) And (hitInfo.InRow) And _
          (Not View.IsGroupRow(hitInfo.RowHandle)) Then
            PopupMenu1.ShowPopup(Control.MousePosition)
        End If
    End Sub

    Private Sub GridView2_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles GridView2.MouseDown
        Dim View As GridView = CType(sender, GridView)
        If View Is Nothing Then Return
        ' obtaining hit info
        Dim hitInfo As GridHitInfo = View.CalcHitInfo(New System.Drawing.Point(e.X, e.Y))
        If (e.Button = Windows.Forms.MouseButtons.Right) And (hitInfo.InRow) And _
          (Not View.IsGroupRow(hitInfo.RowHandle)) Then
            PopupMenu1.ShowPopup(Control.MousePosition)
        End If
    End Sub

    Private Sub GridView3_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles GridView3.MouseDown
        Dim View As GridView = CType(sender, GridView)
        If View Is Nothing Then Return
        ' obtaining hit info
        Dim hitInfo As GridHitInfo = View.CalcHitInfo(New System.Drawing.Point(e.X, e.Y))
        If (e.Button = Windows.Forms.MouseButtons.Right) And (hitInfo.InRow) And _
          (Not View.IsGroupRow(hitInfo.RowHandle)) Then
            PopupMenu1.ShowPopup(Control.MousePosition)
        End If
    End Sub

    Private Sub mnKirimDataKassa(Optional ByVal IDKassa As Long = -1)
        If KirimDataKassa(IDKassa) Then
            XtraMessageBox.Show("Pengiriman berhasil.", NamaAplikasi, MessageBoxButtons.OK)
        End If
    End Sub
    Private Sub mnKirimDataKeKassa_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles mnKirimDataKeKassa.ItemClick
        mnKirimDataKassa()
    End Sub

    Private Sub BarButtonItem8_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem8.ItemClick
        SetAkun("IDAkunPiutang")
    End Sub

    Private Sub txtSearch_ButtonClick(ByVal sender As Object, ByVal e As DevExpress.XtraEditors.Controls.ButtonPressedEventArgs) Handles txtSearch.ButtonClick
        If e.Button.Index = 0 Then
            RefreshData(txtSearch.Text)
        End If
    End Sub

    Private Sub txtSearch_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtSearch.EditValueChanged

    End Sub

    Private Sub txtSearch_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtSearch.KeyDown
        Try
            If e.KeyCode = Keys.Enter Then
                RefreshData(txtSearch.Text)
            ElseIf e.KeyCode = Keys.Up Then
                GV1.ClearSelection()
                GV1.FocusedRowHandle = GV1.FocusedRowHandle - 1
                GV1.SelectRow(GV1.FocusedRowHandle)
            ElseIf e.KeyCode = Keys.Down Then
                GV1.ClearSelection()
                GV1.FocusedRowHandle = GV1.FocusedRowHandle + 1
                GV1.SelectRow(GV1.FocusedRowHandle)
            End If
        Catch ex As Exception

        End Try
    End Sub

    Private Sub txtSearch_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles txtSearch.MouseDown

    End Sub
End Class