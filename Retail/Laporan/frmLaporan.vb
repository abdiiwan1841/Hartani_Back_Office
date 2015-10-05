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
Imports VPoint.clsPostingPenjualan
Imports DevExpress.Utils
Imports DevExpress.XtraGrid.Views.Grid.ViewInfo
Imports DevExpress.XtraGrid.Views.Grid
Imports DevExpress.Utils.Menu
Imports DevExpress.XtraGrid.Localization
Imports System.Data.OleDb

Public Class frmLaporan
    Public FormName As String = ""
    Public FormEntriName As String = ""
    Public TableName As String = ""
    Public TableNameD As String = ""
    Public TableMaster As String = ""
    Public NoID As Long = -1
    Dim oda2 As SqlDataAdapter
    Dim oda3 As SqlDataAdapter

    Dim OleDBoda As OleDbDataAdapter

    Dim ds As New DataSet
    Dim BS As New BindingSource
    Dim HargaPcs As Double

    Dim repckedit As New RepositoryItemCheckEdit
    Dim repdateedit As New RepositoryItemDateEdit
    Dim reptextedit As New RepositoryItemTextEdit
    Dim reppicedit As New RepositoryItemPictureEdit
    Public ShowNoID As Boolean = False
    Public DirectNoID As Long = -1

    Dim NamaFileDB As String = ""
    Private Sub frmDaftarMasterDetil_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Activated
        If ShowNoID Then
            RefreshData()
            GV1.ClearSelection()
            GV1.FocusedRowHandle = GV1.LocateByDisplayText(0, GV1.Columns("NoID"), (DirectNoID).ToString("#,##0"))
            GV1.SelectRow(GV1.FocusedRowHandle)
            ShowNoID = False
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
            TglDari.DateTime = CDate(TanggalSystem.ToString("yyyy/MM/01"))
            TglSampai.DateTime = CDate(TanggalSystem.ToString("yyyy/MM/dd"))
            RefreshLookUp()
            Me.lbDaftar.Text = Me.Text
            XtraTabPage1.Text = Me.Text
            If TableMaster.ToUpper = "MJual".ToUpper Then
                XtraTabPage9.PageVisible = True
                XtraTabControl1.SelectedTabPageIndex = 8
                If XtraTabControl1.SelectedTabPageIndex = 8 Then
                    lbCustomer.Visible = False
                    txtKodeCustomer.Visible = False
                    txtNamaCustomer.Visible = False
                    lbBrg.Visible = True
                    txtKodeBrg.Visible = True
                    txtNamaBrg.Visible = True
                    RefreshDataBarang()
                ElseIf XtraTabControl1.SelectedTabPageIndex = 0 Then
                    lbCustomer.Visible = True
                    txtKodeCustomer.Visible = True
                    txtNamaCustomer.Visible = True
                    lbBrg.Visible = False
                    txtKodeBrg.Visible = False
                    txtNamaBrg.Visible = False
                    RefreshDataKontak()
                End If
            ElseIf TableMaster.ToUpper = "VLapJLBL".ToUpper Then
                XtraTabPage9.PageVisible = False
                lbCustomer.Visible = False
                txtKodeCustomer.Visible = False
                txtNamaCustomer.Visible = False
                lbBrg.Visible = True
                txtKodeBrg.Visible = True
                txtNamaBrg.Visible = True
            Else
                XtraTabPage9.PageVisible = False
                lbCustomer.Visible = False
                txtKodeCustomer.Visible = False
                txtNamaCustomer.Visible = False
                lbBrg.Visible = False
                txtKodeBrg.Visible = False
                txtNamaBrg.Visible = False
            End If
            RefreshData()
            RestoreLayout()
            FungsiControl.SetForm(Me)
        Catch ex As Exception
            XtraMessageBox.Show(ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            Windows.Forms.Cursor.Current = Cursors.Default
            dlg.Close()
            dlg.Dispose()
        End Try
    End Sub

    Private Sub RefreshDataKontak()
        Dim ds As New DataSet
        Dim sql As String
        Try
            sql = "SELECT NoID, Kode, Nama FROM MAlamat WHERE IsActive=1 AND IsCustomer=1"
            ds = ExecuteDataset("master", sql)
            txtKodeCustomer.Properties.DataSource = ds.Tables("master")
            If System.IO.File.Exists(FolderLayouts & Me.Name & SearchLookUpEdit1View.Name & ".xml") Then
                SearchLookUpEdit1View.RestoreLayoutFromXml(FolderLayouts & Me.Name & SearchLookUpEdit1View.Name & ".xml")
            End If

        Catch ex As Exception
            XtraMessageBox.Show("Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            ds.Dispose()
        End Try
    End Sub
    Private Sub RefreshDataBarang()
        Dim ds As New DataSet
        Dim sql As String
        Try
            sql = "SELECT NoID, Kode, Nama FROM MBarang WHERE IsActive=1 "
            ds = ExecuteDataset("MBarang", sql)
            txtKodeBrg.Properties.DataSource = ds.Tables("MBarang")
            If System.IO.File.Exists(FolderLayouts & Me.Name & GridView9.Name & ".xml") Then
                SearchLookUpEdit1View.RestoreLayoutFromXml(FolderLayouts & Me.Name & GridView9.Name & ".xml")
            End If

        Catch ex As Exception
            XtraMessageBox.Show("Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            ds.Dispose()
        End Try
    End Sub
    Private Sub RefreshLookUp()
        Dim ds As New DataSet
        Dim SQL As String = ""
        Try
            SQL = "SELECT NoID, Kode, Nama AS Supplier FROM MAlamat WHERE IsActive=1 AND IsSupplier=1"
            ds = ExecuteDataset("MMaster", SQL)
            txtSupplier.Properties.DataSource = ds.Tables("MMaster")
            txtSupplier.Properties.DisplayMember = "Supplier"
            txtSupplier.Properties.ValueMember = "NoID"

            SQL = "SELECT NoID, IsNull(Kode,'') + ' ' + IsNull(KodeAlias,'') AS Kode, IsNull(Nama,'') + ' ' + IsNull(NamaAlias,'') AS Nama FROM MBarang WHERE IsActive=1"
            ds = ExecuteDataset("MMaster", SQL)
            txtBarang.Properties.DataSource = ds.Tables("MMaster")
            txtBarang.Properties.DisplayMember = "Kode"
            txtBarang.Properties.ValueMember = "NoID"

            Select Case TableMaster.ToUpper
                Case "vLapJLBL".ToUpper
                    SQL = "SELECT NoID, Kode, Nama FROM MAlamat WHERE IsActive=1 AND IsSupplier=1 "
                    ds = ExecuteDataset("MMaster", SQL)
                    txtSupplier.Properties.DataSource = ds.Tables("MMaster")
                    txtSupplier.Properties.DisplayMember = "Kode"
                    txtSupplier.Properties.ValueMember = "NoID"
                    LabelControl9.Text = "Supplier"

                    txtKodeBrg.Properties.DataSource = txtBarang.Properties.DataSource
                    txtKodeBrg.Properties.DisplayMember = txtBarang.Properties.DisplayMember
                    txtKodeBrg.Properties.ValueMember = txtBarang.Properties.ValueMember

                    RadioButton1.Visible = True
                    RadioButton2.Visible = True
                    RadioButton3.Visible = True

                Case "MPOS".ToUpper
                    'txtAlamat.Visible = True
                    'lbAlamat.Visible = True
                    'SQL = "SELECT NoID, Kode, Nama AS Customer FROM MAlamat WHERE IsActive=1 AND IsCustomer=1"
                    'ds = ExecuteDataset("MMaster", SQL)
                    'txtAlamat.Properties.DataSource = ds.Tables("MMaster")
                    'txtAlamat.Properties.DisplayMember = "Kode"
                    'txtAlamat.Properties.ValueMember = "NoID"

                    SQL = "SELECT NoID, Kode, PathDbTemp FROM MPOS WHERE ISActive=1 "
                    ds = ExecuteDataset("MMaster", SQL)
                    txtSupplier.Properties.DataSource = ds.Tables("MMaster")
                    txtSupplier.Properties.DisplayMember = "Kode"
                    txtSupplier.Properties.ValueMember = "NoID"
                    LabelControl9.Text = "Kassa"

                    'SQL = "SELECT NoID, Kode, Nama FROM MKategori WHERE IsActive=1 "
                    'ds = ExecuteDataset("MMaster", SQL)
                    'txtKategori.Properties.DataSource = ds.Tables("MMaster")
                    'txtKategori.Properties.DisplayMember = "Kode"
                    'txtKategori.Properties.ValueMember = "NoID"

                    txtSupplier.Visible = True
                    'lbKategori.Visible = True
                    'lbCustomer.Visible = True
                    'txtKategori.Visible = True
                Case "MRPTJual".ToUpper
                    SQL = "SELECT NoID, Kode, Nama FROM MPOS WHERE ISActive=1 "
                    ds = ExecuteDataset("MMaster", SQL)
                    txtSupplier.Properties.DataSource = ds.Tables("MMaster")
                    txtSupplier.Properties.DisplayMember = "Kode"
                    txtSupplier.Properties.ValueMember = "NoID"
                    lbCustomer.Text = "Kassa"
            End Select
        Catch ex As Exception
            XtraMessageBox.Show("Info : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
        Finally
            ds.Dispose()
        End Try
    End Sub
    Private Sub RestoreLayout()
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
                            .Columns(i).DisplayFormat.FormatString = "HH:mm"
                        ElseIf .Columns(i).FieldName.Trim.ToLower = "tanggalstart" Or .Columns(i).FieldName.Trim.ToLower = "tanggalend" Then
                            .Columns(i).DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime
                            .Columns(i).DisplayFormat.FormatString = "dd-MM-yyyy HH:mm"
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
                            .Columns(i).DisplayFormat.FormatString = "HH:mm"
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
                            .Columns(i).DisplayFormat.FormatString = "HH:mm"
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

        With GridView3
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
                            .Columns(i).DisplayFormat.FormatString = "HH:mm"
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

        With GridView4
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
                            .Columns(i).DisplayFormat.FormatString = "HH:mm"
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

        With GridView5
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
                            .Columns(i).DisplayFormat.FormatString = "HH:mm"
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

        With GridView6
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
                            .Columns(i).DisplayFormat.FormatString = "HH:mm"
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

        With GridView7
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
                            .Columns(i).DisplayFormat.FormatString = "HH:mm"
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

        If System.IO.File.Exists(FolderLayouts & FormName & ".xml") Then
            GV1.RestoreLayoutFromXml(FolderLayouts & FormName & ".xml")
        End If
        If System.IO.File.Exists(FolderLayouts & FormName & GridView1.Name & ".xml") Then
            GridView1.RestoreLayoutFromXml(FolderLayouts & FormName & GridView1.Name & ".xml")
        End If
        If System.IO.File.Exists(FolderLayouts & FormName & GridView2.Name & ".xml") Then
            GridView2.RestoreLayoutFromXml(FolderLayouts & FormName & GridView2.Name & ".xml")
        End If
        If System.IO.File.Exists(FolderLayouts & FormName & GridView3.Name & ".xml") Then
            GridView3.RestoreLayoutFromXml(FolderLayouts & FormName & GridView3.Name & ".xml")
        End If
        If System.IO.File.Exists(FolderLayouts & FormName & GridView4.Name & ".xml") Then
            GridView4.RestoreLayoutFromXml(FolderLayouts & FormName & GridView4.Name & ".xml")
        End If
        If System.IO.File.Exists(FolderLayouts & FormName & GridView5.Name & ".xml") Then
            GridView5.RestoreLayoutFromXml(FolderLayouts & FormName & GridView5.Name & ".xml")
        End If
        If System.IO.File.Exists(FolderLayouts & FormName & GridView6.Name & ".xml") Then
            GridView6.RestoreLayoutFromXml(FolderLayouts & FormName & GridView6.Name & ".xml")
        End If
        If System.IO.File.Exists(FolderLayouts & FormName & GridView7.Name & ".xml") Then
            GridView7.RestoreLayoutFromXml(FolderLayouts & FormName & GridView7.Name & ".xml")
        End If
    End Sub

    Private Function DiSPK() As String
        Return "isnull((SELECT SUM(isnull(A.Qty,0)*isnull(A.Konversi,0)) FROM MSPKD A WHERE A.IDSOD=MSOD.NoID),0)"
    End Function

    Private Function DiBeli() As String
        Return "isnull((SELECT SUM(isnull(A.Qty,0)*isnull(A.Konversi,0)) FROM MBeliD A WHERE A.IDPOD=MPOD.NoID),0)"
    End Function
    Private Function LPBDiBeli() As String
        Return "isnull((SELECT SUM(isnull(A.Qty,0)*isnull(A.Konversi,0)) FROM MBeliD A WHERE A.IDPOD=MPOD.NoID),0)"
    End Function
    Private Function SODiSPK() As String
        Return "isnull((SELECT SUM(isnull(A.Qty,0)*isnull(A.Konversi,0)) FROM MSPKD A WHERE A.IDSOD=MSOD.NoID),0)"
    End Function
    Private Function SPKDiPacking() As String
        Return "isnull((SELECT SUM(isnull(A.Qty,0)*isnull(A.Konversi,0)) FROM MPackingD A WHERE A.IDSPKD=MSPKD.NoID),0)"
    End Function
    Private Function PackingDiJual() As String
        Return "isnull((SELECT SUM(isnull(A.Qty,0)*isnull(A.Konversi,0)) FROM MJualD A WHERE A.IDPackingD=MPackingD.NoID),0)"
    End Function
    Private Function JualDiDO() As String
        Return "isnull((SELECT SUM(isnull(A.Qty,0)*isnull(A.Konversi,0)) FROM MDOD A WHERE A.IDJualD=MJualD.NoID),0)"
    End Function
    Private Function DiSTB() As String
        Return "isnull((SELECT SUM(isnull(A.Qty,0)*isnull(A.Konversi,0)) FROM MBeliD A WHERE A.IDLPBD=MLPBD.NoID),0)"
    End Function
    Private Sub SimpleButton6_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SimpleButton6.Click
        Tutup()
    End Sub

    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

    End Sub

    'Private Sub SimpleButton1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SimpleButton1.Click
    '    If NullTobool(Ini.BacaIni("Application", "UseFramework", False)) Then
    '        Baru()
    '    Else
    '        Select Case TableMaster.ToUpper
    '            Case "MLPB"
    '                Dim view As ColumnView = GC1.FocusedView
    '                Dim x As New frmEntriLPB
    '                Try
    '                    x.NoID = -1
    '                    x.pTipe = frmEntriLPB.pStatus.Baru
    '                    x.MdiParent = Me.MdiParent
    '                    x.WindowState = FormWindowState.Normal
    '                    x.Show()
    '                    x.Focus()
    '                    'If x.ShowDialog(Me)= Windows.Forms.DialogResult.OK Then
    '                    '    RefreshData()
    '                    '    GV1.ClearSelection()
    '                    '    GV1.FocusedRowHandle = GV1.LocateByDisplayText(0, GV1.Columns("NoID"), (x.NoID).ToString("#,##0"))
    '                    '    GV1.SelectRow(GV1.FocusedRowHandle)
    '                    'End If
    '                Catch ex As Exception
    '                    XtraMessageBox.Show("Error : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
    '                Finally
    '                    'x.Close()
    '                    'x.Dispose()
    '                End Try
    '            Case "MJUAL"
    '                Dim view As ColumnView = GC1.FocusedView
    '                Dim x As New frmEntriJual
    '                Try
    '                    x.NoID = -1
    '                    x.pTipe = frmEntriJual.pStatus.Baru
    '                    If x.ShowDialog(Me)= Windows.Forms.DialogResult.OK Then
    '                        RefreshData()
    '                        GV1.ClearSelection()
    '                        GV1.FocusedRowHandle = GV1.LocateByDisplayText(0, GV1.Columns("NoID"), (x.NoID).ToString("#,##0"))
    '                        GV1.SelectRow(GV1.FocusedRowHandle)
    '                    End If
    '                Catch ex As Exception
    '                    XtraMessageBox.Show("Error : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
    '                Finally
    '                    x.Close()
    '                    x.Dispose()
    '                End Try
    '            Case "MRETURJUAL"
    '                Dim view As ColumnView = GC1.FocusedView
    '                Dim x As New frmEntriReturJual
    '                Try
    '                    x.NoID = -1
    '                    x.pTipe = frmEntriReturJual.pStatus.Baru
    '                    If x.ShowDialog(Me)= Windows.Forms.DialogResult.OK Then
    '                        RefreshData()
    '                        GV1.ClearSelection()
    '                        GV1.FocusedRowHandle = GV1.LocateByDisplayText(0, GV1.Columns("NoID"), (x.NoID).ToString("#,##0"))
    '                        GV1.SelectRow(GV1.FocusedRowHandle)
    '                    End If
    '                Catch ex As Exception
    '                    XtraMessageBox.Show("Error : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
    '                Finally
    '                    x.Close()
    '                    x.Dispose()
    '                End Try
    '            Case "MPO"
    '                Dim view As ColumnView = GC1.FocusedView
    '                Dim x As New frmEntriPO
    '                Try
    '                    x.NoID = -1
    '                    x.pTipe = frmEntriPO.pStatus.Baru
    '                    If x.ShowDialog(Me)= Windows.Forms.DialogResult.OK Then
    '                        RefreshData()
    '                        GV1.ClearSelection()
    '                        GV1.FocusedRowHandle = GV1.LocateByDisplayText(0, GV1.Columns("NoID"), (x.NoID).ToString("#,##0"))
    '                        GV1.SelectRow(GV1.FocusedRowHandle)
    '                    End If
    '                Catch ex As Exception
    '                    XtraMessageBox.Show("Error : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
    '                Finally
    '                    x.Close()
    '                    x.Dispose()
    '                End Try
    '            Case "MSO"
    '                Dim view As ColumnView = GC1.FocusedView
    '                Dim x As New frmEntriSO
    '                Try
    '                    x.NoID = -1
    '                    x.pTipe = frmEntriSO.pStatus.Baru
    '                    If x.ShowDialog(Me)= Windows.Forms.DialogResult.OK Then
    '                        RefreshData()
    '                        GV1.ClearSelection()
    '                        GV1.FocusedRowHandle = GV1.LocateByDisplayText(0, GV1.Columns("NoID"), (x.NoID).ToString("#,##0"))
    '                        GV1.SelectRow(GV1.FocusedRowHandle)
    '                    End If
    '                Catch ex As Exception
    '                    XtraMessageBox.Show("Error : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
    '                Finally
    '                    x.Close()
    '                    x.Dispose()
    '                End Try
    '            Case "MBELI"
    '                Dim view As ColumnView = GC1.FocusedView
    '                Dim x As New frmEntriBeli
    '                Try
    '                    x.NoID = -1
    '                    x.pTipe = frmEntriBeli.pStatus.Baru
    '                    If x.ShowDialog(Me)= Windows.Forms.DialogResult.OK Then
    '                        RefreshData()
    '                        GV1.ClearSelection()
    '                        GV1.FocusedRowHandle = GV1.LocateByDisplayText(0, GV1.Columns("NoID"), (x.NoID).ToString("#,##0"))
    '                        GV1.SelectRow(GV1.FocusedRowHandle)
    '                    End If
    '                Catch ex As Exception
    '                    XtraMessageBox.Show("Error : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
    '                Finally
    '                    x.Close()
    '                    x.Dispose()
    '                End Try

    '            Case "MRETURBELI"
    '                Dim view As ColumnView = GC1.FocusedView
    '                Dim x As New frmEntriReturBeli
    '                Try
    '                    x.NoID = -1
    '                    x.pTipe = frmEntriReturBeli.pStatus.Baru
    '                    If x.ShowDialog(Me)= Windows.Forms.DialogResult.OK Then
    '                        RefreshData()
    '                        GV1.ClearSelection()
    '                        GV1.FocusedRowHandle = GV1.LocateByDisplayText(0, GV1.Columns("NoID"), (x.NoID).ToString("#,##0"))
    '                        GV1.SelectRow(GV1.FocusedRowHandle)
    '                    End If
    '                Catch ex As Exception
    '                    XtraMessageBox.Show("Error : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
    '                Finally
    '                    x.Close()
    '                    x.Dispose()
    '                End Try
    '        End Select
    '    End If
    'End Sub
    Sub RefreshData()
        Dim dlg As DevExpress.Utils.WaitDialogForm = Nothing
        Dim cn As New SqlConnection(StrKonSql)
        Dim ocmd2 As New SqlCommand
        Dim Cur As Cursor = Windows.Forms.Cursor.Current
        Dim JumlahWhere As String()
        Dim FilterSQL As String = ""
        Dim dsGudang As New DataSet
        Dim NamaGudang As String = ""

        Dim OleDBcn As OleDbConnection = Nothing
        Dim OleDBocmd As OleDbCommand = Nothing
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
            JumlahWhere = Split(strsql.ToLower, "where")
            If TableMaster.ToUpper = "MJual".ToUpper Then
                Dim whereInsert As String = ""
                If XtraTabControl1.SelectedTabPageIndex = 0 Then
                    If txtNamaCustomer.Text.Trim <> "" Then
                        whereInsert = ""
                        If TglDari.Enabled Then
                            whereInsert = "and Mjual.IDCustomer=" & txtKodeCustomer.EditValue.ToString & " "
                        Else
                            whereInsert = "where Mjual.IDCustomer=" & txtKodeCustomer.EditValue.ToString & " "
                        End If
                    Else

                    End If
                    If TglDari.Enabled Then
                        strsql = "select * from (SELECT count(distinct Mjual.NoID) as JumlahTaransaksi,mjual.IDWilayah,MKategori.Nama as Kategori,MBarang.Kode,Mbarang.Nama,Sum(MJualD.Qty*MJualD.Konversi) as QtyPcs, Sum((( MJualD.Harga*(100-ISNULL(DISCPERSEN1,0))*(100-ISNULL(DISCPERSEN2,0)))/10000-ISNULL(Disc3,0))*MJuald.Qty)as TotalRupiah from mjuald inner join mjual on mjuald.idjual=mjual.noid inner join mbarang  on mjuald.idbarang=mbarang.noid inner join mkategori on mbarang.idkategori=mkategori.noid where tanggal >='" & Format(TglDari.DateTime, "yyyy/MM/dd") & "' and tanggal<'" & Format(TglSampai.DateTime.AddDays(1), "yyyy/MM/dd") & "' " & whereInsert & " group by mjual.IDWilayah,MKategori.Nama,MBarang.Kode,Mbarang.Nama) MJual "
                    Else
                        strsql = "select * from (SELECT count(distinct Mjual.NoID) as JumlahTaransaksi,mjual.IDWilayah,MKategori.Nama as Kategori,MBarang.Kode,Mbarang.Nama,Sum(MJualD.Qty*MJualD.Konversi) as QtyPcs, Sum((( MJualD.Harga*(100-ISNULL(DISCPERSEN1,0))*(100-ISNULL(DISCPERSEN2,0)))/10000-ISNULL(Disc3,0))*MJuald.Qty)as TotalRupiah from mjuald inner join mjual on mjuald.idjual=mjual.noid inner join mbarang  on mjuald.idbarang=mbarang.noid inner join mkategori on mbarang.idkategori=mkategori.noid " & whereInsert & " group by mjual.IDWilayah,MKategori.Nama,MBarang.Kode,Mbarang.Nama) MJual"
                    End If
                Else

                    If txtKodeBrg.Text.Trim <> "" Then
                        whereInsert = ""
                        If TglDari.Enabled Then
                            whereInsert = "and MjualD.IDBarang=" & txtKodeBrg.EditValue.ToString & " "
                        Else
                            whereInsert = "where MjualD.IDBarang=" & txtKodeBrg.EditValue.ToString & " "
                        End If
                    Else

                    End If
                    If TglDari.Enabled Then
                        strsql = "select * from (SELECT count(distinct Mjual.NoID) as JumlahTaransaksi,MAlamat.Nama as Customer,mjual.IDWilayah,MKategori.Nama as Kategori,MBarang.Kode,Mbarang.Nama,Sum(MJualD.Qty*MJualD.Konversi) as QtyPcs, Sum(Jumlah)as TotalRupiah " & _
                        "from mjuald inner join mjual on mjuald.idjual=mjual.noid " & _
                        "inner join mbarang  on mjuald.idbarang=mbarang.noid " & _
                        "inner join mkategori on mbarang.idkategori=mkategori.noid " & _
                        "left join MAlamat on Mjual.IDCustomer=MAlamat.NoID " & _
                        "where tanggal >='" & Format(TglDari.DateTime, "yyyy/MM/dd") & "' and tanggal<'" & Format(TglSampai.DateTime.AddDays(1), "yyyy/MM/dd") & "' " & whereInsert & _
                        " group by mjual.IDWilayah,mjual.IDCustomer,MAlamat.Nama,MKategori.Nama,MBarang.Kode,Mbarang.Nama) MJual "
                    Else
                        strsql = "select * from (SELECT count(distinct Mjual.NoID) as JumlahTaransaksi,MAlamat.Nama as Customer,mjual.IDWilayah,MKategori.Nama as Kategori,MBarang.Kode,Mbarang.Nama,Sum(MJualD.Qty*MJualD.Konversi) as QtyPcs, Sum((( MJualD.Harga*(100-ISNULL(DISCPERSEN1,0))*(100-ISNULL(DISCPERSEN2,0)))/10000-ISNULL(Disc3,0))*MJuald.Qty)as TotalRupiah " & _
                        "from mjuald inner join mjual on mjuald.idjual=mjual.noid " & _
                        "inner join mbarang  on mjuald.idbarang=mbarang.noid " & _
                        "inner join mkategori on mbarang.idkategori=mkategori.noid " & _
                        "left join MAlamat on Mjual.IDCustomer=MAlamat.NoID " & _
                        "" & whereInsert & " group by mjual.IDWilayah,mjual.IDCustomer,MAlamat.Nama,MKategori.Nama,MBarang.Kode,Mbarang.Nama) MJual"
                    End If
                End If
            ElseIf TableMaster.ToUpper = "VLapJLBL".ToUpper Then
                strsql = "SELECT MBarangD.NoID, MBarangD.IDBarang, MBarang.Kode, MBarang.Nama, MBarangD.Barcode, MKategori.Nama AS Kategori, " & vbCrLf
                If TglDari.Enabled Then
                    strsql &= " (SELECT SUM(MBeliD.Qty*MBeliD.Konversi) FROM MBeli INNER JOIN MBeliD ON MBeli.NoID=MBeliD.IDBeli WHERE MBeliD.IDBarang=MBarangD.IDBarang AND MBeliD.IDBarangD=MBarangD.NoID AND MBeli.Tanggal>='" & TglDari.DateTime.ToString("yyyy/MM/dd") & "' AND MBeli.Tanggal<'" & TglSampai.DateTime.AddDays(1).ToString("yyyy/MM/dd") & "') AS QtyPembelian," & vbCrLf & _
                              " (SELECT SUM(MJualD.Qty*MJualD.Konversi) FROM MJual INNER JOIN MJualD ON MJual.NoID=MJualD.IDJual WHERE MJualD.IDBarang=MBarangD.IDBarang AND MJualD.IDBarangD=MBarangD.NoID AND MJual.Tanggal>='" & TglDari.DateTime.ToString("yyyy/MM/dd") & "' AND MJual.Tanggal<'" & TglSampai.DateTime.AddDays(1).ToString("yyyy/MM/dd") & "') AS QtyPenjualan, " & vbCrLf
                    strsql &= " (SELECT SUM(MBeliD.Jumlah) FROM MBeli INNER JOIN MBeliD ON MBeli.NoID=MBeliD.IDBeli WHERE MBeliD.IDBarang=MBarangD.IDBarang AND MBeliD.IDBarangD=MBarangD.NoID AND MBeli.Tanggal>='" & TglDari.DateTime.ToString("yyyy/MM/dd") & "' AND MBeli.Tanggal<'" & TglSampai.DateTime.AddDays(1).ToString("yyyy/MM/dd") & "') AS JumlahPembelian," & vbCrLf & _
                              " (SELECT SUM(MJualD.Jumlah) FROM MJual INNER JOIN MJualD ON MJual.NoID=MJualD.IDJual WHERE MJualD.IDBarang=MBarangD.IDBarang AND MJualD.IDBarangD=MBarangD.NoID AND MJual.Tanggal>='" & TglDari.DateTime.ToString("yyyy/MM/dd") & "' AND MJual.Tanggal<'" & TglSampai.DateTime.AddDays(1).ToString("yyyy/MM/dd") & "') AS JumlahPenjualan " & vbCrLf
                Else
                    strsql &= " (SELECT SUM(MBeliD.Qty*MBeliD.Konversi) FROM MBeli INNER JOIN MBeliD ON MBeli.NoID=MBeliD.IDBeli WHERE MBeliD.IDBarang=MBarangD.IDBarang AND MBeliD.IDBarangD=MBarangD.NoID) AS QtyPembelian," & vbCrLf & _
                              " (SELECT SUM(MJualD.Qty*MJualD.Konversi) FROM MJual INNER JOIN MJualD ON MJual.NoID=MJualD.IDJual WHERE MJualD.IDBarang=MBarangD.IDBarang AND MJualD.IDBarangD=MBarangD.NoID) AS QtyPenjualan, " & vbCrLf
                    strsql &= " (SELECT SUM(MBeliD.Jumlah) FROM MBeli INNER JOIN MBeliD ON MBeli.NoID=MBeliD.IDBeli WHERE MBeliD.IDBarang=MBarangD.IDBarang AND MBeliD.IDBarangD=MBarangD.NoID) AS JumlahPembelian," & vbCrLf & _
                              " (SELECT SUM(MJualD.Jumlah) FROM MJual INNER JOIN MJualD ON MJual.NoID=MJualD.IDJual WHERE MJualD.IDBarang=MBarangD.IDBarang AND MJualD.IDBarangD=MBarangD.NoID) AS JumlahPenjualan " & vbCrLf
                End If
                strsql &= " FROM MBarang " & vbCrLf & _
                          " INNER JOIN MBarangD ON MBarang.NoID=MBarangD.IDBarang" & vbCrLf & _
                          " INNER JOIN MKategori ON MKategori.NoID=MBarang.IDKategori" & vbCrLf & _
                          " WHERE 1=1 "
                If txtKodeBrg.Text <> "" Then
                    strsql &= " AND MBarang.NoID= " & NullToLong(txtKodeBrg.EditValue)
                End If
                If txtSupplier.Text <> "" Then
                    strsql &= " AND (MBarang.IDSupplier1=" & NullToLong(txtSupplier.EditValue) & vbCrLf & _
                              " OR MBarang.IDSupplier2=" & NullToLong(txtSupplier.EditValue) & vbCrLf & _
                              " OR MBarang.IDSupplier3=" & NullToLong(txtSupplier.EditValue) & vbCrLf & _
                              " OR MBarang.IDSupplier4=" & NullToLong(txtSupplier.EditValue) & vbCrLf & _
                              " OR MBarang.IDSupplier5=" & NullToLong(txtSupplier.EditValue) & ")"
                End If
                strsql = "SELECT X.*, " & _
                         " (CASE WHEN IsNull(X.QtyPembelian,0)=0 THEN 0 ELSE X.JumlahPembelian/X.QtyPembelian END) AS HargaPembelianRataRata, " & _
                         " (CASE WHEN IsNull(X.QtyPenjualan,0)=0 THEN 0 ELSE X.JumlahPenjualan/X.QtyPenjualan END) AS HargaPenjualanRataRata " & _
                         " FROM (" & strsql & ") X "
                If RadioButton1.Checked Then
                    strsql &= " ORDER BY X.QtyPembelian DESC, X.QtyPenjualan DESC"
                End If
                If RadioButton2.Checked Then
                    strsql &= " ORDER BY (CASE WHEN IsNull(X.QtyPembelian,0)=0 THEN 0 ELSE X.JumlahPembelian/X.QtyPembelian END) DESC, (CASE WHEN IsNull(X.QtyPenjualan,0)=0 THEN 0 ELSE X.JumlahPenjualan/X.QtyPenjualan END) DESC "
                End If
                If RadioButton3.Checked Then
                    strsql &= " ORDER BY X.QtyPembelian DESC, X.QtyPenjualan DESC, (CASE WHEN IsNull(X.QtyPembelian,0)=0 THEN 0 ELSE X.JumlahPembelian/X.QtyPembelian END) DESC, (CASE WHEN IsNull(X.QtyPenjualan,0)=0 THEN 0 ELSE X.JumlahPenjualan/X.QtyPenjualan END) DESC "
                End If
                GV1.OptionsCustomization.AllowSort = False
            ElseIf TableMaster.ToUpper = "MRPTJual".ToUpper Then
                strsql = "SELECT MKategori.NoID, MKategori.Kode, MKategori.Nama, MParent.Kode AS KodeParent, MParent.Nama AS NamaParent, SUM(vRekapPenjualanPerDepartemen.Jumlah) AS Jumlah" & vbCrLf & _
                         " FROM (MKategori INNER JOIN MKategori MParent ON MParent.NoID=MKategori.IDParent)" & vbCrLf & _
                         " LEFT JOIN vRekapPenjualanPerDepartemen ON vRekapPenjualanPerDepartemen.NoID=MKategori.NoID " & vbCrLf & _
                         IIf(TglDari.Enabled, "AND vRekapPenjualanPerDepartemen.Tanggal>='" & TglDari.DateTime.ToString("yyyy/MM/dd") & "' AND vRekapPenjualanPerDepartemen.Tanggal<'" & DateAdd(DateInterval.Day, 1, TglSampai.DateTime).ToString("yyyy/MM/dd") & "' ", "") & vbCrLf & _
                         IIf(txtSupplier.Text <> "", " AND vRekapPenjualanPerDepartemen.IDPOS=" & NullToLong(txtSupplier.EditValue), "") & vbCrLf & _
                         " GROUP BY MKategori.NoID, MKategori.Kode, MKategori.Nama, MParent.Kode, MParent.Nama" & vbCrLf & _
                         " ORDER BY MParent.Kode"
            ElseIf TableMaster.ToUpper = "MPOS".ToUpper Then
                'Provider=Microsoft.Jet.OLEDB.4.0;Data Source=\\serverName\shareName\folder\TempDB_201112.mdb;User Id=admin;Password=;
                If txtSupplier.Text <> "" Then
                    NamaFileDB = NullToStr(EksekusiSQlSkalarNew("SELECT PathDbTemp FROM MPOS WHERE NoID=" & NullToLong(txtSupplier.EditValue))) & "\Database\TempDB_" & TglDari.DateTime.ToString("yyyyMM") & ".mdb"
                Else
                    NamaFileDB = Application.StartupPath & "\system\engine\tempdb.mdb"
                End If
                If Not System.IO.File.Exists(NamaFileDB) Then
                    NamaFileDB = Application.StartupPath & "\system\engine\tempdb.mdb"
                End If
                strsql = "SELECT MSALES.*, MSales.Subtotal-MSales.DiscNota-MSales.Pembulatan AS Total, Msales.DiBayar-(MSales.Subtotal-MSales.DiscNota-MSales.Pembulatan) AS Kembali FROM MSALES WHERE 1=1"
                If TglDari.Enabled Then
                    strsql &= " AND MSALES.Tanggal>=#" & TglDari.DateTime.ToString("yyyy/MM/dd") & "# AND MSALES.Tanggal<#" & DateAdd(DateInterval.Day, 1, TglSampai.DateTime).ToString("yyyy/MM/dd") & "#"
                End If
                'If txtAlamat.Text <> "" Then
                '    strsql &= " AND MSALES.IDMember=" & NullToLong(txtAlamat.EditValue)
                'End If
                txtSupplier.Visible = True
                'lbCustomer.Visible = True
                mnSetPending.Visibility = DevExpress.XtraBars.BarItemVisibility.Always
                mnSetNonPending.Visibility = DevExpress.XtraBars.BarItemVisibility.Always
                mnDownload.Visibility = DevExpress.XtraBars.BarItemVisibility.Always
            ElseIf TableMaster.ToUpper = "MBARANG".ToUpper Then
                dsGudang = ExecuteDataset("MGudang", "SELECT CASE WHEN IsNULL(MGudang.IsBS,0)=0 THEN MGudang.Kode ELSE MGudang.Kode + '_BS' END AS Kode FROM MGudang WHERE MGudang.NoID IN (SELECT MUserDAksesGudang.IDGudang FROM MUserDAksesGudang WHERE MUserDAksesGudang.IsActive=1 AND MUserDAksesGudang.IDUser=" & IDUserAktif & ")")
                For i As Integer = 0 To dsGudang.Tables(0).Rows.Count - 1
                    NamaGudang = NamaGudang & IIf(i = 0, "", ", ") & NullToStr(dsGudang.Tables(0).Rows(i).Item("Kode"))
                Next
                If NamaGudang = "" Then
                    strsql = "SELECT MBarang.NoID, MBarang.Kode AS KodeBarang,MBarang.Barcode, MBarang.Nama AS NamaBarang,0 AS TotalQty" & vbCrLf & _
                             " FROM MBarang "
                ElseIf TglDari.Enabled Then
                    strsql = "Select XYZ.*,MAlamat.Nama as Supplier,MKategori.Nama as Kategori From (SELECT NoID,KodeBarang,IDSupplier,IDKategori,Barcode,NamaBarang,QtyMinimum,IDBarang,Wilayah "
                    For i As Integer = 0 To dsGudang.Tables(0).Rows.Count - 1
                        strsql = strsql & ", SUM(" & NullToStr(dsGudang.Tables(0).Rows(i).Item("Kode")) & ") AS " & NullToStr(dsGudang.Tables(0).Rows(i).Item("Kode"))
                    Next
                    strsql = strsql & ", SUM(TotalQty) AS TotalQty FROM (" & _
                             "SELECT MBarang.NoID, MBarang.IDKategori,MBarang.IDSupplier, MBarang.Kode AS KodeBarang,MBarang.Barcode, MBarang.Nama AS NamaBarang,MBarang.StockMin as QtyMinimum,p.*, (SELECT SUM((MKartuStok.QtyMasuk*MKartuStok.Konversi)-(MKartuStok.QtyKeluar*MKartuStok.Konversi)) FROM MKartuStok LEFT JOIN MGudang ON MGudang.NoID=MKartuStok.IDGudang WHERE MGudang.NoID IN (SELECT MUserDAksesGudang.IDGudang FROM MUserDAksesGudang WHERE MUserDAksesGudang.IsActive=1 AND MUserDAksesGudang.IDUser=" & IDUserAktif & ") AND MBarang.NoID=MKartuStok.IDBarang AND (MKartuStok.Tanggal>='" & Format(TglDari.DateTime, "yyyy/MM/dd") & "' and MKartuStok.Tanggal<'" & Format(DateAdd(DateInterval.Day, 1, TglSampai.DateTime), "yyyy/MM/dd") & "') ) AS TotalQty" & vbCrLf & _
                             " FROM MBarang LEFT JOIN " & vbCrLf & _
                             " (SELECT MKartuStok.IDBarang, MWilayah.Nama AS Wilayah, (CASE WHEN IsNULL(MGudang.IsBS,0)=0 THEN MGudang.Kode ELSE MGudang.Kode + '_BS' END) AS Gudang, (MKartuStok.QtyMasuk*MKartuStok.Konversi)-(MKartuStok.QtyKeluar*MKartuStok.Konversi) AS QtySisa FROM MKartuStok LEFT JOIN (MGudang LEFT JOIN MWilayah ON MWilayah.NoID=MGudang.IDWilayah) ON MGudang.NoID=MKartuStok.IDGudang WHERE MGudang.NoID IN (SELECT MUserDAksesGudang.IDGudang FROM MUserDAksesGudang WHERE MUserDAksesGudang.IsActive=1 AND MUserDAksesGudang.IDUser=" & IDUserAktif & ") AND (MKartuStok.Tanggal>='" & Format(TglDari.DateTime, "yyyy/MM/dd") & "' and MKartuStok.Tanggal<'" & Format(DateAdd(DateInterval.Day, 1, TglSampai.DateTime), "yyyy/MM/dd") & "')) AS pvtsource" & vbCrLf & _
                             " PIVOT" & vbCrLf & _
                             " (SUM(pvtsource.QtySisa) FOR pvtsource.Gudang IN(" & NamaGudang & ")" & vbCrLf & _
                             " ) AS p ON MBarang.NoID=p.IDBarang) MPBarang " & _
                             " GROUP BY NoID,KodeBarang,Barcode,NamaBarang,IDKategori,IDSupplier,QtyMinimum,IDBarang,Wilayah) XYZ " & _
                             " Left Join MSupplier On XYZ.IDSupplier=Malamat.NoID " & _
                             " Left Join MKategori On XYZ.IDKategori=MKategori.NoID "
                Else
                    strsql = "Select XYZ.*,MAlamat.Nama as Supplier,MKategori.Nama as Kategori From (SELECT NoID,KodeBarang,IDSupplier,IDKategori,Barcode,NamaBarang,QtyMinimum,IDBarang,Wilayah "
                    For i As Integer = 0 To dsGudang.Tables(0).Rows.Count - 1
                        strsql = strsql & ", SUM(" & NullToStr(dsGudang.Tables(0).Rows(i).Item("Kode")) & ") AS " & NullToStr(dsGudang.Tables(0).Rows(i).Item("Kode"))
                    Next
                    strsql = strsql & ", SUM(TotalQty) AS TotalQty FROM (" & _
                             "SELECT MBarang.NoID, MBarang.Kode AS KodeBarang,Mbarang.Barcode, MBarang.Nama AS NamaBarang,MBarang.StockMin as QtyMinimum, p.*, (SELECT SUM((MKartuStok.QtyMasuk*MKartuStok.Konversi)-(MKartuStok.QtyKeluar*MKartuStok.Konversi)) FROM MKartuStok LEFT JOIN MGudang ON MGudang.NoID=MKartuStok.IDGudang WHERE MGudang.NoID IN (SELECT MUserDAksesGudang.IDGudang FROM MUserDAksesGudang WHERE MUserDAksesGudang.IsActive=1 AND MUserDAksesGudang.IDUser=" & IDUserAktif & ") AND MBarang.NoID=MKartuStok.IDBarang) AS TotalQty" & vbCrLf & _
                             " FROM MBarang LEFT JOIN " & vbCrLf & _
                             " (SELECT MKartuStok.IDBarang, MWilayah.Nama AS Wilayah, (CASE WHEN IsNULL(MGudang.IsBS,0)=0 THEN MGudang.Kode ELSE MGudang.Kode + '_BS' END) AS Gudang, (MKartuStok.QtyMasuk*MKartuStok.Konversi)-(MKartuStok.QtyKeluar*MKartuStok.Konversi) AS QtySisa FROM MKartuStok LEFT JOIN (MGudang LEFT JOIN MWilayah ON MWilayah.NoID=MGudang.IDWilayah) ON MGudang.NoID=MKartuStok.IDGudang WHERE MGudang.NoID IN (SELECT MUserDAksesGudang.IDGudang FROM MUserDAksesGudang WHERE MUserDAksesGudang.IsActive=1 AND MUserDAksesGudang.IDUser=" & IDUserAktif & ")) AS pvtsource" & vbCrLf & _
                             " PIVOT" & vbCrLf & _
                             " (SUM(pvtsource.QtySisa) FOR pvtsource.Gudang IN(" & NamaGudang & ")" & vbCrLf & _
                             " ) AS p ON MBarang.NoID=p.IDBarang) MPBarang " & _
                             " GROUP BY NoID,KodeBarang,Barcode,NamaBarang,IDSupplier,IDKategori,QtyMinimum,IDBarang,Wilayah) XYZ " & _
                             " Left Join MSupplier On XYZ.IDSupplier=Malamat.NoID " & _
                             " Left Join MKategori On XYZ.IDKategori=MKategori.NoID "

                End If
                ''GridBand2.Visible = False
            ElseIf TableMaster.ToUpper = "MHutang".ToUpper Then
                If TglDari.Enabled Then
                    strsql = "SELECT MBeli.NoID, MBeli.Kode AS KodeTransaksi, MBeli.Tanggal, MAlamat.Kode AS KodeSupplier, MAlamat.Nama AS NamaSupplier, 0 AS Debet, MBeli.Total AS Kredit" & vbCrLf & _
                             " FROM MBeli LEFT JOIN MAlamat ON MAlamat.NoID=MBeli.IDSupplier WHERE MBeli.IDWilayah=" & DefIDWilayah & " AND (MBeli.Tanggal>='" & Format(TglDari.DateTime, "yyyy/MM/dd") & "' AND MBeli.Tanggal<'" & Format(DateAdd(DateInterval.Day, 1, TglSampai.DateTime), "yyyy/MM/dd") & "') " & vbCrLf & _
                             " UNION ALL " & vbCrLf & _
                             " SELECT MReturBeli.NoID, MReturBeli.Kode, MReturBeli.Tanggal, MAlamat.Kode AS KodeSupplier, MAlamat.Nama AS NamaSupplier, MReturBeli.Total AS Debet, 0 AS Kredit" & vbCrLf & _
                             " FROM MReturBeli LEFT JOIN MAlamat ON MAlamat.NoID=MReturBeli.IDSupplier WHERE MReturBeli.IDWilayah=" & DefIDWilayah & " AND (MReturBeli.Tanggal>='" & Format(TglDari.DateTime, "yyyy/MM/dd") & "' AND MReturBeli.Tanggal<'" & Format(DateAdd(DateInterval.Day, 1, TglSampai.DateTime), "yyyy/MM/dd") & "') " & vbCrLf & _
                             " UNION ALL" & vbCrLf & _
                             " SELECT MRevisiHargaBeli.NoID, MRevisiHargaBeli.Kode, MRevisiHargaBeli.Tanggal, MAlamat.Kode AS KodeSupplier, MAlamat.Nama AS NamaSupplier, " & vbCrLf & _
                             " (CASE WHEN SUM(MRevisiHargaBeliD.Jumlah-MRevisiHargaBeliD.JumlahBaru)<0 THEN 0 ELSE ABS(SUM(MRevisiHargaBeliD.Jumlah-MRevisiHargaBeliD.JumlahBaru)) END) AS Debet, " & vbCrLf & _
                             " (CASE WHEN SUM(MRevisiHargaBeliD.Jumlah-MRevisiHargaBeliD.JumlahBaru)>0 THEN 0 ELSE ABS(SUM(MRevisiHargaBeliD.Jumlah-MRevisiHargaBeliD.JumlahBaru)) END) AS Kredit " & vbCrLf & _
                             " FROM MRevisiHargaBeliD INNER JOIN MRevisiHargaBeli ON MRevisiHargaBeli.NoID=MRevisiHargaBeliD.IDRevisiHargaBeli LEFT JOIN MAlamat ON MAlamat.NoID=MRevisiHargaBeli.IDSupplier" & vbCrLf & _
                             " WHERE MRevisiHargaBeli.IsPosted=1 AND MRevisiHargaBeli.IDWilayah=" & DefIDWilayah & " AND (MRevisiHargaBeli.Tanggal>='" & Format(TglDari.DateTime, "yyyy/MM/dd") & "' AND MRevisiHargaBeli.Tanggal<'" & Format(DateAdd(DateInterval.Day, 1, TglSampai.DateTime), "yyyy/MM/dd") & "') " & vbCrLf & _
                             " GROUP BY MRevisiHargaBeli.IsPosted, MRevisiHargaBeli.NoID, MRevisiHargaBeli.IDWilayah, MRevisiHargaBeli.Kode, MRevisiHargaBeli.Tanggal, MAlamat.Kode, MAlamat.Nama"
                Else
                    strsql = "SELECT MBeli.NoID, MBeli.Kode AS KodeTransaksi, MBeli.Tanggal, MAlamat.Kode AS KodeSupplier, MAlamat.Nama AS NamaSupplier, 0 AS Debet, MBeli.Total AS Kredit" & vbCrLf & _
                             " FROM MBeli LEFT JOIN MAlamat ON MAlamat.NoID=MBeli.IDSupplier" & vbCrLf & _
                             " UNION ALL " & vbCrLf & _
                             " SELECT MReturBeli.NoID, MReturBeli.Kode, MReturBeli.Tanggal, MAlamat.Kode AS KodeSupplier, MAlamat.Nama AS NamaSupplier, MReturBeli.Total AS Debet, 0 AS Kredit" & vbCrLf & _
                             " FROM MReturBeli LEFT JOIN MAlamat ON MAlamat.NoID=MReturBeli.IDSupplier" & vbCrLf & _
                             " UNION ALL" & vbCrLf & _
                             " SELECT MRevisiHargaBeli.NoID, MRevisiHargaBeli.Kode, MRevisiHargaBeli.Tanggal, MAlamat.Kode AS KodeSupplier, MAlamat.Nama AS NamaSupplier, " & vbCrLf & _
                             " (CASE WHEN SUM(MRevisiHargaBeliD.Jumlah-MRevisiHargaBeliD.JumlahBaru)<0 THEN 0 ELSE ABS(SUM(MRevisiHargaBeliD.Jumlah-MRevisiHargaBeliD.JumlahBaru)) END) AS Debet, " & vbCrLf & _
                             " (CASE WHEN SUM(MRevisiHargaBeliD.Jumlah-MRevisiHargaBeliD.JumlahBaru)>0 THEN 0 ELSE ABS(SUM(MRevisiHargaBeliD.Jumlah-MRevisiHargaBeliD.JumlahBaru)) END) AS Kredit " & vbCrLf & _
                             " FROM MRevisiHargaBeliD INNER JOIN MRevisiHargaBeli ON MRevisiHargaBeli.NoID=MRevisiHargaBeliD.IDRevisiHargaBeli LEFT JOIN MAlamat ON MAlamat.NoID=MRevisiHargaBeli.IDSupplier" & vbCrLf & _
                             " WHERE MRevisiHargaBeli.IsPosted=1 AND MRevisiHargaBeli.IDWilayah=" & DefIDWilayah & "" & _
                             " GROUP BY MRevisiHargaBeli.IsPosted, MRevisiHargaBeli.IDWilayah, MRevisiHargaBeli.NoID, MRevisiHargaBeli.Kode, MRevisiHargaBeli.Tanggal, MAlamat.Kode, MAlamat.Nama"
                End If
                'GridBand2.Visible = False
            ElseIf TableMaster.ToUpper = "MPiutang".ToUpper Then
                If TglDari.Enabled Then
                    strsql = "SELECT MJual.NoID, MJual.Kode AS KodeTransaksi, MJual.Tanggal, MAlamat.Kode AS KodeCustomer, MAlamat.Nama AS NamaCustomer, MJual.Total-IsNull(MJual.Bayar,0) AS Debet, 0 AS Kredit" & vbCrLf & _
                             " FROM MJual LEFT JOIN MAlamat ON MAlamat.NoID=MJual.IDCustomer WHERE MJual.Total-IsNull(MJual.Bayar,0)>0 AND MJual.IDWilayah=" & DefIDWilayah & " AND (MJual.Tanggal>='" & Format(TglDari.DateTime, "yyyy/MM/dd") & "' AND MJual.Tanggal<'" & Format(DateAdd(DateInterval.Day, 1, TglSampai.DateTime), "yyyy/MM/dd") & "') " & vbCrLf & _
                             " UNION ALL " & vbCrLf & _
                             " SELECT MReturJual.NoID, MReturJual.Kode, MReturJual.Tanggal, MAlamat.Kode AS KodeCustomer, MAlamat.Nama AS NamaCustomer, 0 AS Debet, MReturJual.Total AS Kredit" & vbCrLf & _
                             " FROM MReturJual LEFT JOIN MAlamat ON MAlamat.NoID=MReturJual.IDCustomer WHERE MReturJual.IDWilayah=" & DefIDWilayah & " AND (MReturJual.Tanggal>='" & Format(TglDari.DateTime, "yyyy/MM/dd") & "' AND MReturJual.Tanggal<'" & Format(DateAdd(DateInterval.Day, 1, TglSampai.DateTime), "yyyy/MM/dd") & "') " & vbCrLf & _
                             " UNION ALL" & vbCrLf & _
                             " SELECT MRevisiHargaJual.NoID, MRevisiHargaJual.Kode, MRevisiHargaJual.Tanggal, MAlamat.Kode AS KodeCustomer, MAlamat.Nama AS NamaCustomer, " & vbCrLf & _
                             " (CASE WHEN SUM(MRevisiHargaJualD.Jumlah-MRevisiHargaJualD.JumlahBaru)>0 THEN 0 ELSE ABS(SUM(MRevisiHargaJualD.Jumlah-MRevisiHargaJualD.JumlahBaru)) END) AS Debet, " & vbCrLf & _
                             " (CASE WHEN SUM(MRevisiHargaJualD.Jumlah-MRevisiHargaJualD.JumlahBaru)<0 THEN 0 ELSE ABS(SUM(MRevisiHargaJualD.Jumlah-MRevisiHargaJualD.JumlahBaru)) END) AS Kredit " & vbCrLf & _
                             " FROM MRevisiHargaJualD INNER JOIN MRevisiHargaJual ON MRevisiHargaJual.NoID=MRevisiHargaJualD.IDRevisiHargaJual LEFT JOIN MAlamat ON MAlamat.NoID=MRevisiHargaJual.IDCustomer" & vbCrLf & _
                             " WHERE MRevisiHargaJual.IsPosted=1 AND MRevisiHargaJual.IDWilayah=" & DefIDWilayah & " AND (MRevisiHargaJual.Tanggal>='" & Format(TglDari.DateTime, "yyyy/MM/dd") & "' AND MRevisiHargaJual.Tanggal<'" & Format(DateAdd(DateInterval.Day, 1, TglSampai.DateTime), "yyyy/MM/dd") & "') " & vbCrLf & _
                             " GROUP BY MRevisiHargaJual.NoID, MRevisiHargaJual.IsPosted, MRevisiHargaJual.IDWilayah, MRevisiHargaJual.Kode, MRevisiHargaJual.Tanggal, MAlamat.Kode, MAlamat.Nama"
                Else
                    strsql = "SELECT MJual.NoID, MJual.Kode AS KodeTransaksi, MJual.Tanggal, MAlamat.Kode AS KodeCustomer, MAlamat.Nama AS NamaCustomer, MJual.Total AS Debet, 0 AS Kredit" & vbCrLf & _
                             " FROM MJual LEFT JOIN MAlamat ON MAlamat.NoID=MJual.IDCustomer" & vbCrLf & _
                             " UNION ALL " & vbCrLf & _
                             " SELECT MReturJual.NoID, MReturJual.Kode, MReturJual.Tanggal, MAlamat.Kode AS KodeCustomer, MAlamat.Nama AS NamaCustomer, 0 AS Debet, MReturJual.Total AS Kredit" & vbCrLf & _
                             " FROM MReturJual LEFT JOIN MAlamat ON MAlamat.NoID=MReturJual.IDCustomer" & vbCrLf & _
                             " UNION ALL" & vbCrLf & _
                             " SELECT MRevisiHargaJual.NoID, MRevisiHargaJual.Kode, MRevisiHargaJual.Tanggal, MAlamat.Kode AS KodeCustomer, MAlamat.Nama AS NamaCustomer, " & vbCrLf & _
                             " (CASE WHEN SUM(MRevisiHargaJualD.Jumlah-MRevisiHargaJualD.JumlahBaru)>0 THEN 0 ELSE ABS(SUM(MRevisiHargaJualD.Jumlah-MRevisiHargaJualD.JumlahBaru)) END) AS Debet, " & vbCrLf & _
                             " (CASE WHEN SUM(MRevisiHargaJualD.Jumlah-MRevisiHargaJualD.JumlahBaru)<0 THEN 0 ELSE ABS(SUM(MRevisiHargaJualD.Jumlah-MRevisiHargaJualD.JumlahBaru)) END) AS Kredit " & vbCrLf & _
                             " FROM MRevisiHargaJualD INNER JOIN MRevisiHargaJual ON MRevisiHargaJual.NoID=MRevisiHargaJualD.IDRevisiHargaJual LEFT JOIN MAlamat ON MAlamat.NoID=MRevisiHargaJual.IDCustomer" & vbCrLf & _
                             " WHERE MRevisiHargaJual.IsPosted=1 AND MRevisiHargaJual.IDWilayah=" & DefIDWilayah & _
                             " GROUP BY MRevisiHargaJual.NoID, , MRevisiHargaJual.IsPosted, MRevisiHargaJual.IDWilayah, MRevisiHargaJual.Kode, MRevisiHargaJual.Tanggal, MAlamat.Kode, MAlamat.Nama"
                End If
                'GridBand2.Visible = False
            ElseIf TableMaster.ToUpper = "TLogUser".ToUpper Then
                If TglDari.Enabled Then
                    strsql = "SELECT TLogUser.*, MUser.Nama AS [User] FROM TLogUser LEFT JOIN MUser ON MUser.NoID=TLogUser.IDUser " & vbCrLf & _
                             " WHERE (TLogUser.TanggalStart>='" & Format(TglDari.DateTime, "yyyy/MM/dd") & "' AND TLogUser.TanggalStart<'" & Format(DateAdd(DateInterval.Day, 1, TglSampai.DateTime), "yyyy/MM/dd") & "') "
                Else
                    strsql = "SELECT TLogUser.*, MUser.Nama AS [User] FROM TLogUser LEFT JOIN MUser ON MUser.NoID=TLogUser.IDUser "
                End If
                'GridBand2.Visible = False
            ElseIf TableMaster.ToUpper = "MBeli".ToUpper Then
                strsql = IIf(InStr(TableName, "select", CompareMethod.Text) > 0, TableName, "select * from " & TableName)
                strsql = strsql & " where " & TableMaster & ".IDWilayah=" & DefIDWilayah
                FilterSQL = FilterSQL & " WHERE " & TableMaster & ".IDWilayah=" & DefIDWilayah
                If TglDari.Enabled Then
                    If InStr(strsql.ToLower, "where", CompareMethod.Text) > 0 Then
                        strsql = strsql & " and (" & TableMaster & ".Tanggal>='" & Format(TglDari.DateTime, "yyyy/MM/dd") & "' and " & TableMaster & ".Tanggal<'" & Format(DateAdd(DateInterval.Day, 1, TglSampai.DateTime), "yyyy/MM/dd") & "') "
                        FilterSQL = FilterSQL & " and (" & TableMaster & ".Tanggal>='" & Format(TglDari.DateTime, "yyyy/MM/dd") & "' and " & TableMaster & ".Tanggal<'" & Format(DateAdd(DateInterval.Day, 1, TglSampai.DateTime), "yyyy/MM/dd") & "') "
                    Else
                        strsql = strsql & " where (" & TableMaster & ".Tanggal>='" & Format(TglDari.DateTime, "yyyy/MM/dd") & "' and " & TableMaster & ".Tanggal<'" & Format(DateAdd(DateInterval.Day, 1, TglSampai.DateTime), "yyyy/MM/dd") & "') "
                        FilterSQL = FilterSQL & " where (" & TableMaster & ".Tanggal>='" & Format(TglDari.DateTime, "yyyy/MM/dd") & "' and " & TableMaster & ".Tanggal<'" & Format(DateAdd(DateInterval.Day, 1, TglSampai.DateTime), "yyyy/MM/dd") & "') "
                    End If
                End If
                'GridBand2.Visible = False
            ElseIf TableMaster.ToUpper = "MReturBeli".ToUpper Then
                strsql = IIf(InStr(TableName, "select", CompareMethod.Text) > 0, TableName, "select * from " & TableName)
                strsql = strsql & " where " & TableMaster & ".IDWilayah=" & DefIDWilayah
                FilterSQL = FilterSQL & " WHERE " & TableMaster & ".IDWilayah=" & DefIDWilayah
                If TglDari.Enabled Then
                    If InStr(strsql.ToLower, "where", CompareMethod.Text) > 0 Then
                        strsql = strsql & " and (" & TableMaster & ".Tanggal>='" & Format(TglDari.DateTime, "yyyy/MM/dd") & "' and " & TableMaster & ".Tanggal<'" & Format(DateAdd(DateInterval.Day, 1, TglSampai.DateTime), "yyyy/MM/dd") & "') "
                        FilterSQL = FilterSQL & " and (" & TableMaster & ".Tanggal>='" & Format(TglDari.DateTime, "yyyy/MM/dd") & "' and " & TableMaster & ".Tanggal<'" & Format(DateAdd(DateInterval.Day, 1, TglSampai.DateTime), "yyyy/MM/dd") & "') "
                    Else
                        strsql = strsql & " where (" & TableMaster & ".Tanggal>='" & Format(TglDari.DateTime, "yyyy/MM/dd") & "' and " & TableMaster & ".Tanggal<'" & Format(DateAdd(DateInterval.Day, 1, TglSampai.DateTime), "yyyy/MM/dd") & "') "
                        FilterSQL = FilterSQL & " where (" & TableMaster & ".Tanggal>='" & Format(TglDari.DateTime, "yyyy/MM/dd") & "' and " & TableMaster & ".Tanggal<'" & Format(DateAdd(DateInterval.Day, 1, TglSampai.DateTime), "yyyy/MM/dd") & "') "
                    End If
                End If
                'GridBand2.Visible = False
            Else
                strsql = IIf(InStr(TableName, "select", CompareMethod.Text) > 0, TableName, "select * from " & TableName)
                JumlahWhere = Split(strsql.ToLower, "where")
                If JumlahWhere.Length >= 3 Then
                    strsql = strsql & " and " & TableMaster & ".IDWilayah=" & DefIDWilayah
                Else
                    strsql = strsql & " where " & TableMaster & ".IDWilayah=" & DefIDWilayah
                End If
                FilterSQL = FilterSQL & " WHERE " & TableMaster & ".IDWilayah=" & DefIDWilayah
                If TglDari.Enabled Then
                    If InStr(strsql.ToLower, "where", CompareMethod.Text) > 0 Then
                        strsql = strsql & " and (" & TableMaster & ".Tanggal>='" & Format(TglDari.DateTime, "yyyy/MM/dd") & "' and " & TableMaster & ".Tanggal<'" & Format(DateAdd(DateInterval.Day, 1, TglSampai.DateTime), "yyyy/MM/dd") & "') "
                        FilterSQL = FilterSQL & " and (" & TableMaster & ".Tanggal>='" & Format(TglDari.DateTime, "yyyy/MM/dd") & "' and " & TableMaster & ".Tanggal<'" & Format(DateAdd(DateInterval.Day, 1, TglSampai.DateTime), "yyyy/MM/dd") & "') "
                    Else
                        strsql = strsql & " where (" & TableMaster & ".Tanggal>='" & Format(TglDari.DateTime, "yyyy/MM/dd") & "' and " & TableMaster & ".Tanggal<'" & Format(DateAdd(DateInterval.Day, 1, TglSampai.DateTime), "yyyy/MM/dd") & "') "
                        FilterSQL = FilterSQL & " where (" & TableMaster & ".Tanggal>='" & Format(TglDari.DateTime, "yyyy/MM/dd") & "' and " & TableMaster & ".Tanggal<'" & Format(DateAdd(DateInterval.Day, 1, TglSampai.DateTime), "yyyy/MM/dd") & "') "
                    End If
                End If
                'GridBand2.Visible = False
            End If
            If TableMaster.ToUpper = "MPOS".ToUpper Then
                'Provider=Microsoft.Jet.OLEDB.4.0;Data Source=\\serverName\shareName\folder\myDatabase.mdb;User Id=admin;Password=;
                OleDBcn = New OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" & NamaFileDB & ";User Id=admin;Password=;")
                OleDBocmd = New OleDb.OleDbCommand
                OleDBocmd.Connection = OleDBcn
                OleDBocmd.CommandType = CommandType.Text
                OleDBocmd.CommandText = strsql
                SQL = strsql

                OleDBcn.Open()
                OleDBoda = New OleDbDataAdapter(OleDBocmd)
                OleDBoda.TableMappings.Add("Tabel", "Data")
                If ds.Tables("Data") Is Nothing Then
                Else
                    ds.Tables("Data").Clear()
                End If
                OleDBoda.Fill(ds, "Data")
                BS.DataSource = ds.Tables("Data")
                GC1.DataSource = BS.DataSource

                OleDBoda.Dispose()
                OleDBocmd.Dispose()
                OleDBcn.Close()
                OleDBcn.Dispose()
            Else
                ocmd2.Connection = cn
                ocmd2.CommandType = CommandType.Text
                ocmd2.CommandText = strsql
                SQL = strsql

                cn.Open()
                oda2 = New SqlDataAdapter(ocmd2)
                oda2.TableMappings.Add("Tabel", "Data")
                If ds.Tables("Data") Is Nothing Then
                Else
                    ds.Tables("Data").Clear()
                End If
                oda2.Fill(ds, "Data")
                BS.DataSource = ds.Tables("Data")
                GC1.DataSource = BS.DataSource
            End If
            '#Region "Tampilkan Detil"
            'If TableMaster.ToUpper = "MPO".ToUpper Then
            '    strsql = "SELECT MBeli.Tanggal, MBeli.Kode, MLPB.Kode AS NoPenerimaan, MAlamat.Nama AS Supplier, MGudang.Nama as Gudang, MWilayah.Nama as Wilayah, MBarang.Kode AS KodeStock, MBarang.Nama as NamaStock, MBeliD.Qty*MBeliD.Konversi as QtyPcs, MBeliD.Harga/MBeliD.Konversi AS HargaPcs, MBeliD.NoID AS No, MBeliD.IDPOD " & vbCrLf & _
            '             " FROM (((((MBeliD INNER JOIN MBeli ON MBeli.NoID=MBeliD.IDBeli)" & vbCrLf & _
            '             " LEFT JOIN MBarang ON MBarang.NoID=MBeliD.IDBarang) " & vbCrLf & _
            '             " LEFT JOIN MAlamat ON MAlamat.NoID=MBeli.IDSupplier) " & vbCrLf & _
            '             " LEFT JOIN (MLPBD INNER JOIN MLPB ON MLPB.NoID=MLPB.IDLPB) ON MLPB.NoID=MBeliD.IDLPBD) " & vbCrLf & _
            '             " LEFT JOIN (MGudang LEFT JOIN MWilayah ON MWilayah.NoID=MGudang.IDWilayah) ON MGudang.NoID=MBeliD.IDGudang) " & vbCrLf & _
            '             " WHERE MBeliD.IDPOD IN (SELECT MPOD.NoID FROM MPOD INNER JOIN MPO ON MPO.NoID=MPOD.IDPO " & FilterSQL & ")"
            '    oda3 = New SqlClient.SqlDataAdapter
            '    oda3.TableMappings.Add("Tabel", "MBeliD")
            '    cn = New SqlClient.SqlConnection(StrKonSql)
            '    cn.Open()
            '    ocmd2 = New SqlClient.SqlCommand(strsql, cn)
            '    oda3 = New SqlClient.SqlDataAdapter(ocmd2)
            '    If ds.Tables("MBeliD") Is Nothing Then
            '    Else
            '        ds.Tables("MBeliD").Clear()
            '    End If
            '    oda3.Fill(ds, "MBeliD")

            '    ' Create a DataRelation to link the two tables
            '    ' based on the SupplierID.
            '    Dim parentColumnutama As DataColumn = ds.Tables("Data").Columns("NoID")
            '    Dim childColumnutama As DataColumn = ds.Tables("MBeliD").Columns("IDPOD")
            '    Dim relationheader As New DataRelation("DetilPembelian", parentColumnutama, childColumnutama, False)
            '    If ds.Relations.Count >= 1 Then
            '        ds.Relations.Clear()
            '    End If
            '    ds.Relations.Add(relationheader)
            '    oda3.Dispose()
            '    ocmd2.Dispose()
            '    cn.Close()
            '    cn.Dispose()
            'ElseIf TableMaster.ToUpper = "MLPB".ToUpper Then
            '    strsql = "SELECT MBeli.Tanggal, MBeli.Kode, MPO.Kode AS NoPO, MAlamat.Nama AS Supplier, MGudang.Nama as Gudang, MWilayah.Nama as Wilayah, MBarang.Kode AS KodeStock, MBarang.Nama as NamaStock, MBeliD.Qty*MBeliD.Konversi as QtyPcs, MBeliD.Harga/MBeliD.Konversi AS HargaPcs, MBeliD.NoID AS No, MBeliD.IDLPBD " & vbCrLf & _
            '             " FROM (((((MBeliD INNER JOIN MBeli ON MBeli.NoID=MBeliD.IDBeli)" & vbCrLf & _
            '             " LEFT JOIN MBarang ON MBarang.NoID=MBeliD.IDBarang) " & vbCrLf & _
            '             " LEFT JOIN MAlamat ON MAlamat.NoID=MBeli.IDSupplier) " & vbCrLf & _
            '             " LEFT JOIN (MPOD INNER JOIN MPO ON MPO.NoID=MPO.IDPO) ON MPO.NoID=MBeliD.IDPOD) " & vbCrLf & _
            '             " LEFT JOIN (MGudang LEFT JOIN MWilayah ON MWilayah.NoID=MGudang.IDWilayah) ON MGudang.NoID=MBeliD.IDGudang) " & vbCrLf & _
            '             " WHERE MBeliD.IDLPBD IN (SELECT MLPBD.NoID FROM MLPBD INNER JOIN MLPB ON MLPB.NoID=MLPBD.IDLPB " & FilterSQL & ")"
            '    oda3 = New SqlClient.SqlDataAdapter
            '    oda3.TableMappings.Add("Tabel", "MBeliD")
            '    cn = New SqlClient.SqlConnection(StrKonSql)
            '    cn.Open()
            '    ocmd2 = New SqlClient.SqlCommand(strsql, cn)
            '    oda3 = New SqlClient.SqlDataAdapter(ocmd2)
            '    If ds.Tables("MBeliD") Is Nothing Then
            '    Else
            '        ds.Tables("MBeliD").Clear()
            '    End If
            '    oda3.Fill(ds, "MBeliD")

            '    ' Create a DataRelation to link the two tables
            '    ' based on the SupplierID.
            '    Dim parentColumnutama As DataColumn = ds.Tables("Data").Columns("NoID")
            '    Dim childColumnutama As DataColumn = ds.Tables("MBeliD").Columns("IDLPBD")
            '    Dim relationheader As New DataRelation("DetilPembelian", parentColumnutama, childColumnutama, False)
            '    If ds.Relations.Count >= 1 Then
            '        ds.Relations.Clear()
            '    End If
            '    ds.Relations.Add(relationheader)
            '    oda3.Dispose()
            '    ocmd2.Dispose()
            '    cn.Close()
            '    cn.Dispose()
            'ElseIf TableMaster.ToUpper = "MBeli".ToUpper Then
            '    strsql = "SELECT MReturBeli.Tanggal, MReturBeli.Kode, MPO.Kode AS NoPO, MLPB.Kode AS NoPenerimaan, MAlamat.Nama AS Supplier, MGudang.Nama as Gudang, MWilayah.Nama as Wilayah, MBarang.Kode AS KodeStock, MBarang.Nama as NamaStock, MReturBeliD.Qty*MReturBeliD.Konversi as QtyPcs, MReturBeliD.Harga/MReturBeliD.Konversi AS HargaPcs, MReturBeliD.NoID AS No, MReturBeliD.IDBeliD " & vbCrLf & _
            '             " FROM (((((MReturBeliD INNER JOIN MReturBeli ON MReturBeli.NoID=MReturBeliD.IDReturBeli)" & vbCrLf & _
            '             " LEFT JOIN MBarang ON MBarang.NoID=MReturBeliD.IDBarang) " & vbCrLf & _
            '             " LEFT JOIN MAlamat ON MAlamat.NoID=MReturBeli.IDSupplier) " & vbCrLf & _
            '             " LEFT JOIN (((MBeliD INNER JOIN MBeli ON MBeli.NoID=MBeliD.IDBeli) LEFT JOIN (MPOD INNER JOIN MPO ON MPO.NoID=MPOD.IDPO) ON MPOD.NoID=MBeliD.IDPOD) LEFT JOIN (MLPBD INNER JOIN MLPB ON MLPB.NoID=MLPBD.IDLPB) ON MLPBD.NoID=MBeliD.IDLPBD) ON MBeliD.NoID=MReturBeliD.IDBeliD) " & vbCrLf & _
            '             " LEFT JOIN (MGudang LEFT JOIN MWilayah ON MWilayah.NoID=MGudang.IDWilayah) ON MGudang.NoID=MReturBeliD.IDGudang) " & vbCrLf & _
            '             FilterSQL
            '    '" WHERE MReturBeliD.IDBeliD IN (SELECT MBeliD.NoID FROM MBeliD INNER JOIN MBeli ON MBeli.NoID=MBeliD.IDBeli " & FilterSQL & ")"
            '    oda3 = New SqlClient.SqlDataAdapter
            '    oda3.TableMappings.Add("Tabel", "MReturBeliD")
            '    cn = New SqlClient.SqlConnection(StrKonSql)
            '    cn.Open()
            '    ocmd2 = New SqlClient.SqlCommand(strsql, cn)
            '    oda3 = New SqlClient.SqlDataAdapter(ocmd2)
            '    If ds.Tables("MReturBeliD") Is Nothing Then
            '    Else
            '        ds.Tables("MReturBeliD").Clear()
            '    End If
            '    oda3.Fill(ds, "MReturBeliD")

            '    ' Create a DataRelation to link the two tables
            '    ' based on the SupplierID.
            '    Dim parentColumnutama As DataColumn = ds.Tables("Data").Columns("NoID")
            '    Dim childColumnutama As DataColumn = ds.Tables("MReturBeliD").Columns("IDBeliD")
            '    Dim relationheader As New DataRelation("DetilReturPembelian", parentColumnutama, childColumnutama, False)
            '    If ds.Relations.Count >= 2 Then
            '        ds.Relations.Clear()
            '    End If
            '    ds.Relations.Add(relationheader)
            '    oda3.Dispose()
            '    ocmd2.Dispose()
            '    cn.Close()
            '    cn.Dispose()

            '    'Revisi Harga
            '    strsql = "SELECT MRevisiHargaBeli.Tanggal, MRevisiHargaBeli.Kode, MPO.Kode AS NoPO, MLPB.Kode AS NoPenerimaan, MAlamat.Nama AS Supplier, MGudang.Nama as Gudang, MWilayah.Nama as Wilayah, MBarang.Kode AS KodeStock, MBarang.Nama as NamaStock, MRevisiHargaBeliD.Qty*MRevisiHargaBeliD.Konversi as QtyPcs, MRevisiHargaBeliD.Harga/MRevisiHargaBeliD.Konversi AS HargaPcs, MRevisiHargaBeliD.NoID AS No, MRevisiHargaBeliD.IDBeliD " & vbCrLf & _
            '             " FROM (((((MRevisiHargaBeliD INNER JOIN MRevisiHargaBeli ON MRevisiHargaBeli.NoID=MRevisiHargaBeliD.IDRevisiHargaBeli)" & vbCrLf & _
            '             " LEFT JOIN MBarang ON MBarang.NoID=MRevisiHargaBeliD.IDBarang) " & vbCrLf & _
            '             " LEFT JOIN MAlamat ON MAlamat.NoID=MRevisiHargaBeli.IDSupplier) " & vbCrLf & _
            '             " LEFT JOIN (((MBeliD INNER JOIN MBeli ON MBeli.NoID=MBeliD.IDBeli) LEFT JOIN (MPOD INNER JOIN MPO ON MPO.NoID=MPOD.IDPO) ON MPOD.NoID=MBeliD.IDPOD) LEFT JOIN (MLPBD INNER JOIN MLPB ON MLPB.NoID=MLPBD.IDLPB) ON MLPBD.NoID=MBeliD.IDLPBD) ON MBeliD.NoID=MRevisiHargaBeliD.IDBeliD) " & vbCrLf & _
            '             " LEFT JOIN (MGudang LEFT JOIN MWilayah ON MWilayah.NoID=MGudang.IDWilayah) ON MGudang.NoID=MRevisiHargaBeliD.IDGudang) " & vbCrLf & _
            '             FilterSQL
            '    '" WHERE MReturBeliD.IDBeliD IN (SELECT MBeliD.NoID FROM MBeliD INNER JOIN MBeli ON MBeli.NoID=MBeliD.IDBeli " & FilterSQL & ")"
            '    oda3 = New SqlClient.SqlDataAdapter
            '    oda3.TableMappings.Add("Tabel", "MRevisiBeliD")
            '    cn = New SqlClient.SqlConnection(StrKonSql)
            '    cn.Open()
            '    ocmd2 = New SqlClient.SqlCommand(strsql, cn)
            '    oda3 = New SqlClient.SqlDataAdapter(ocmd2)
            '    If ds.Tables("MRevisiBeliD") Is Nothing Then
            '    Else
            '        ds.Tables("MRevisiBeliD").Clear()
            '    End If
            '    oda3.Fill(ds, "MRevisiBeliD")

            '    ' Create a DataRelation to link the two tables
            '    ' based on the SupplierID.
            '    Dim parentColumnutama1 As DataColumn = ds.Tables("Data").Columns("NoID")
            '    Dim childColumnutama1 As DataColumn = ds.Tables("MRevisiBeliD").Columns("IDBeliD")
            '    Dim relationheader1 As New DataRelation("DetilRevisiPembelian", parentColumnutama1, childColumnutama1, False)
            '    If ds.Relations.Count >= 2 Then
            '        ds.Relations.Clear()
            '    End If
            '    ds.Relations.Add(relationheader1)
            '    oda3.Dispose()
            '    ocmd2.Dispose()
            '    cn.Close()
            '    cn.Dispose()
            'ElseIf TableMaster.ToUpper = "MSO".ToUpper Then
            '    strsql = "SELECT MSPK.Tanggal, MSPK.Kode, MAlamat.Nama AS Customer, MGudang.Nama as Gudang, MWilayah.Nama as Wilayah, MBarang.Kode AS KodeStock, MBarang.Nama as NamaStock, MSPKD.Qty*MSPKD.Konversi as QtyPcs, MSPKD.Harga/MSPKD.Konversi AS HargaPcs, MSPKD.NoID AS No, MSPKD.IDSOD " & vbCrLf & _
            '             " FROM ((((MSPKD INNER JOIN MSPK ON MSPK.NoID=MSPKD.IDSPK)" & vbCrLf & _
            '             " LEFT JOIN MBarang ON MBarang.NoID=MSPKD.IDBarang) " & vbCrLf & _
            '             " LEFT JOIN MAlamat ON MAlamat.NoID=MSPK.IDCustomer) " & vbCrLf & _
            '             " LEFT JOIN (MGudang LEFT JOIN MWilayah ON MWilayah.NoID=MGudang.IDWilayah) ON MGudang.NoID=MSPKD.IDGudang) " & vbCrLf & _
            '             " WHERE MSPKD.IDSOD IN (SELECT MSOD.NoID FROM MSOD INNER JOIN MSO ON MSO.NoID=MSOD.IDSO " & FilterSQL & ")"
            '    oda3 = New SqlClient.SqlDataAdapter
            '    oda3.TableMappings.Add("Tabel", "SPKD")
            '    cn = New SqlClient.SqlConnection(StrKonSql)
            '    cn.Open()
            '    ocmd2 = New SqlClient.SqlCommand(strsql, cn)
            '    oda3 = New SqlClient.SqlDataAdapter(ocmd2)
            '    If ds.Tables("SPKD") Is Nothing Then
            '    Else
            '        ds.Tables("MSPKD").Clear()
            '    End If
            '    oda3.Fill(ds, "MSPKD")

            '    ' Create a DataRelation to link the two tables
            '    ' based on the SupplierID.
            '    Dim parentColumnutama As DataColumn = ds.Tables("Data").Columns("NoID")
            '    Dim childColumnutama As DataColumn = ds.Tables("MSPKD").Columns("IDSOD")
            '    Dim relationheader As New DataRelation("DetilSPK", parentColumnutama, childColumnutama, False)
            '    If ds.Relations.Count >= 1 Then
            '        ds.Relations.Clear()
            '    End If
            '    ds.Relations.Add(relationheader)
            '    oda3.Dispose()
            '    ocmd2.Dispose()
            '    cn.Close()
            '    cn.Dispose()
            'ElseIf TableMaster.ToUpper = "MSPK".ToUpper Then
            '    strsql = "SELECT MPacking.Tanggal, MPacking.Kode, MAlamat.Nama AS Customer, MGudang.Nama as Gudang, MWilayah.Nama as Wilayah, MBarang.Kode AS KodeStock, MBarang.Nama as NamaStock, MPackingD.Qty*MPackingD.Konversi as QtyPcs, MPackingD.Harga/MPackingD.Konversi AS HargaPcs, MPackingD.NoID AS No, MPackingD.IDSPKD " & vbCrLf & _
            '             " FROM ((((MPackingD INNER JOIN MPacking ON MPacking.NoID=MPackingD.IDPacking)" & vbCrLf & _
            '             " LEFT JOIN MBarang ON MBarang.NoID=MPackingD.IDBarang) " & vbCrLf & _
            '             " LEFT JOIN MAlamat ON MAlamat.NoID=MPacking.IDCustomer) " & vbCrLf & _
            '             " LEFT JOIN (MGudang LEFT JOIN MWilayah ON MWilayah.NoID=MGudang.IDWilayah) ON MGudang.NoID=MPackingD.IDGudang) " & vbCrLf & _
            '             " WHERE MPackingD.IDSPKD IN (SELECT MSPKD.NoID FROM MSPKD INNER JOIN MSPK ON MSPK.NoID=MSPKD.IDSPK " & FilterSQL & ")"
            '    oda3 = New SqlClient.SqlDataAdapter
            '    oda3.TableMappings.Add("Tabel", "MPackingD")
            '    cn = New SqlClient.SqlConnection(StrKonSql)
            '    cn.Open()
            '    ocmd2 = New SqlClient.SqlCommand(strsql, cn)
            '    oda3 = New SqlClient.SqlDataAdapter(ocmd2)
            '    If ds.Tables("MPackingD") Is Nothing Then
            '    Else
            '        ds.Tables("MPackingD").Clear()
            '    End If
            '    oda3.Fill(ds, "MPackingD")

            '    ' Create a DataRelation to link the two tables
            '    ' based on the SupplierID.
            '    Dim parentColumnutama As DataColumn = ds.Tables("Data").Columns("NoID")
            '    Dim childColumnutama As DataColumn = ds.Tables("MPackingD").Columns("IDSPKD")
            '    Dim relationheader As New DataRelation("DetilPacking", parentColumnutama, childColumnutama, False)
            '    If ds.Relations.Count >= 1 Then
            '        ds.Relations.Clear()
            '    End If
            '    ds.Relations.Add(relationheader)
            '    oda3.Dispose()
            '    ocmd2.Dispose()
            '    cn.Close()
            '    cn.Dispose()
            'ElseIf TableMaster.ToUpper = "MPacking".ToUpper Then
            '    'Penjualan
            '    strsql = "SELECT MJual.Tanggal, MJual.Kode, MAlamat.Nama AS Customer, MGudang.Nama as Gudang, MWilayah.Nama as Wilayah, MBarang.Kode AS KodeStock, MBarang.Nama as NamaStock, MJualD.Qty*MJualD.Konversi as QtyPcs, MJualD.Harga/MJualD.Konversi AS HargaPcs, MJualD.NoID AS No, MJualD.IDPackingD " & vbCrLf & _
            '             " FROM ((((MJualD INNER JOIN MJual ON MJual.NoID=MJualD.IDJual)" & vbCrLf & _
            '             " LEFT JOIN MBarang ON MBarang.NoID=MJualD.IDBarang) " & vbCrLf & _
            '             " LEFT JOIN MAlamat ON MAlamat.NoID=MJual.IDCustomer) " & vbCrLf & _
            '             " LEFT JOIN (MGudang LEFT JOIN MWilayah ON MWilayah.NoID=MGudang.IDWilayah) ON MGudang.NoID=MJualD.IDGudang) " & vbCrLf & _
            '             " WHERE MJualD.IDPackingD IN (SELECT MPackingD.NoID FROM MPackingD INNER JOIN MPacking ON MPacking.NoID=MPackingD.IDPacking " & FilterSQL & ")"
            '    oda3 = New SqlClient.SqlDataAdapter
            '    oda3.TableMappings.Add("Tabel", "MJualD")
            '    cn = New SqlClient.SqlConnection(StrKonSql)
            '    cn.Open()
            '    ocmd2 = New SqlClient.SqlCommand(strsql, cn)
            '    oda3 = New SqlClient.SqlDataAdapter(ocmd2)
            '    If ds.Tables("MJualD") Is Nothing Then
            '    Else
            '        ds.Tables("MJualD").Clear()
            '    End If
            '    oda3.Fill(ds, "MJualD")

            '    ' Create a DataRelation to link the two tables
            '    ' based on the SupplierID.
            '    Dim parentColumnutama As DataColumn = ds.Tables("Data").Columns("NoID")
            '    Dim childColumnutama As DataColumn = ds.Tables("MJualD").Columns("IDPackingD")
            '    Dim relationheader As New DataRelation("DetilPenjualan", parentColumnutama, childColumnutama, False)
            '    If ds.Relations.Count >= 1 Then
            '        ds.Relations.Clear()
            '    End If
            '    ds.Relations.Add(relationheader)
            '    oda3.Dispose()
            '    ocmd2.Dispose()
            '    cn.Close()
            '    cn.Dispose()

            '    'Surat Jalan
            '    strsql = "SELECT MDO.Tanggal, MDO.Kode, MAlamat.Nama AS Customer, MGudang.Nama as Gudang, MWilayah.Nama as Wilayah, MBarang.Kode AS KodeStock, MBarang.Nama as NamaStock, MDOD.Qty*MDOD.Konversi as QtyPcs, MDOD.Harga/MDOD.Konversi AS HargaPcs, MDOD.NoID AS No, MDOD.IDPackingD " & vbCrLf & _
            '             " FROM ((((MDOD INNER JOIN MDO ON MDO.NoID=MDOD.IDDO)" & vbCrLf & _
            '             " LEFT JOIN MBarang ON MBarang.NoID=MDOD.IDBarang) " & vbCrLf & _
            '             " LEFT JOIN MAlamat ON MAlamat.NoID=MDO.IDCustomer) " & vbCrLf & _
            '             " LEFT JOIN (MGudang LEFT JOIN MWilayah ON MWilayah.NoID=MGudang.IDWilayah) ON MGudang.NoID=MDOD.IDGudang) " & vbCrLf & _
            '             " WHERE MDOD.IDPackingD IN (SELECT MPackingD.NoID FROM MPackingD INNER JOIN MPacking ON MPacking.NoID=MPackingD.IDPacking " & FilterSQL & ")"
            '    oda3 = New SqlClient.SqlDataAdapter
            '    oda3.TableMappings.Add("Tabel", "MDOD")
            '    cn = New SqlClient.SqlConnection(StrKonSql)
            '    cn.Open()
            '    ocmd2 = New SqlClient.SqlCommand(strsql, cn)
            '    oda3 = New SqlClient.SqlDataAdapter(ocmd2)
            '    If ds.Tables("MDOD") Is Nothing Then
            '    Else
            '        ds.Tables("MDOD").Clear()
            '    End If
            '    oda3.Fill(ds, "MDOD")

            '    ' Create a DataRelation to link the two tables
            '    ' based on the SupplierID.
            '    Dim parentColumnutama1 As DataColumn = ds.Tables("Data").Columns("NoID")
            '    Dim childColumnutama1 As DataColumn = ds.Tables("MDOD").Columns("IDPackingD")
            '    Dim relationheader1 As New DataRelation("DetilSuratJalan", parentColumnutama1, childColumnutama1, False)
            '    If ds.Relations.Count >= 2 Then
            '        ds.Relations.Clear()
            '    End If
            '    ds.Relations.Add(relationheader1)
            '    oda3.Dispose()
            '    ocmd2.Dispose()
            '    cn.Close()
            '    cn.Dispose()
            'ElseIf TableMaster.ToUpper = "MJual".ToUpper Then
            '    strsql = "SELECT MReturJual.Tanggal, MReturJual.Kode, MPacking.Kode AS KodePacking, MPackingD.NoPacking, MAlamat.Nama AS Customer, MGudang.Nama as Gudang, MWilayah.Nama as Wilayah, MBarang.Kode AS KodeStock, MBarang.Nama as NamaStock, MReturJualD.Qty*MReturJualD.Konversi as QtyPcs, MReturJualD.Harga/MReturJualD.Konversi AS HargaPcs, MReturJualD.NoID AS No, MReturJualD.IDJualD " & vbCrLf & _
            '             " FROM (((((MReturJualD INNER JOIN MReturJual ON MReturJual.NoID=MReturJualD.IDReturJual)" & vbCrLf & _
            '             " LEFT JOIN MBarang ON MBarang.NoID=MReturJualD.IDBarang) " & vbCrLf & _
            '             " LEFT JOIN MAlamat ON MAlamat.NoID=MReturJual.IDCustomer) " & vbCrLf & _
            '             " LEFT JOIN ((MJualD INNER JOIN MJual ON MJual.NoID=MJualD.IDJual) LEFT JOIN (MPackingD INNER JOIN MPacking ON MPacking.NoID=MPackingD.IDPacking) ON MPackingD.NoID=MJualD.IDPackingD) ON MJualD.NoID=MReturJualD.IDJualD) " & vbCrLf & _
            '             " LEFT JOIN (MGudang LEFT JOIN MWilayah ON MWilayah.NoID=MGudang.IDWilayah) ON MGudang.NoID=MReturJualD.IDGudang) " & vbCrLf & _
            '             FilterSQL
            '    oda3 = New SqlClient.SqlDataAdapter
            '    oda3.TableMappings.Add("Tabel", "MReturJualD")
            '    cn = New SqlClient.SqlConnection(StrKonSql)
            '    cn.Open()
            '    ocmd2 = New SqlClient.SqlCommand(strsql, cn)
            '    oda3 = New SqlClient.SqlDataAdapter(ocmd2)
            '    If ds.Tables("MReturJualD") Is Nothing Then
            '    Else
            '        ds.Tables("MReturJualD").Clear()
            '    End If
            '    oda3.Fill(ds, "MReturJualD")

            '    ' Create a DataRelation to link the two tables
            '    ' based on the SupplierID.
            '    Dim parentColumnutama As DataColumn = ds.Tables("Data").Columns("NoID")
            '    Dim childColumnutama As DataColumn = ds.Tables("MReturJualD").Columns("IDJualD")
            '    Dim relationheader As New DataRelation("DetilReturPenjualan", parentColumnutama, childColumnutama, False)
            '    If ds.Relations.Count >= 2 Then
            '        ds.Relations.Clear()
            '    End If
            '    ds.Relations.Add(relationheader)
            '    oda3.Dispose()
            '    ocmd2.Dispose()
            '    cn.Close()
            '    cn.Dispose()

            '    'Revisi Harga
            '    strsql = "SELECT MRevisiHargaJual.Tanggal, MRevisiHargaJual.Kode, MPackingD.NoPacking, MPacking.Kode AS KodePacking, MAlamat.Nama AS Customer, MGudang.Nama as Gudang, MWilayah.Nama as Wilayah, MBarang.Kode AS KodeStock, MBarang.Nama as NamaStock, MRevisiHargaJualD.Qty*MRevisiHargaJualD.Konversi as QtyPcs, MRevisiHargaJualD.Harga/MRevisiHargaJualD.Konversi AS HargaPcs, MRevisiHargaJualD.NoID AS No, MRevisiHargaJualD.IDJualD " & vbCrLf & _
            '             " FROM (((((MRevisiHargaJualD INNER JOIN MRevisiHargaJual ON MRevisiHargaJual.NoID=MRevisiHargaJualD.IDRevisiHargaJual)" & vbCrLf & _
            '             " LEFT JOIN MBarang ON MBarang.NoID=MRevisiHargaJualD.IDBarang) " & vbCrLf & _
            '             " LEFT JOIN MAlamat ON MAlamat.NoID=MRevisiHargaJual.IDCustomer) " & vbCrLf & _
            '             " LEFT JOIN ((MJualD INNER JOIN MJual ON MJual.NoID=MJualD.IDJual) LEFT JOIN (MPackingD INNER JOIN MPacking ON MPacking.NoID=MPackingD.IDPacking) ON MPackingD.NoID=MJualD.IDPackingD) ON MJualD.NoID=MRevisiHargaJualD.IDJualD) " & vbCrLf & _
            '             " LEFT JOIN (MGudang LEFT JOIN MWilayah ON MWilayah.NoID=MGudang.IDWilayah) ON MGudang.NoID=MRevisiHargaJualD.IDGudang) " & vbCrLf & _
            '             FilterSQL
            '    oda3 = New SqlClient.SqlDataAdapter
            '    oda3.TableMappings.Add("Tabel", "MRevisiJualD")
            '    cn = New SqlClient.SqlConnection(StrKonSql)
            '    cn.Open()
            '    ocmd2 = New SqlClient.SqlCommand(strsql, cn)
            '    oda3 = New SqlClient.SqlDataAdapter(ocmd2)
            '    If ds.Tables("MRevisiJualD") Is Nothing Then
            '    Else
            '        ds.Tables("MRevisiJualD").Clear()
            '    End If
            '    oda3.Fill(ds, "MRevisiJualD")

            '    ' Create a DataRelation to link the two tables
            '    ' based on the CustomerID.
            '    Dim parentColumnutama1 As DataColumn = ds.Tables("Data").Columns("NoID")
            '    Dim childColumnutama1 As DataColumn = ds.Tables("MRevisiJualD").Columns("IDJualD")
            '    Dim relationheader1 As New DataRelation("DetilRevisiPenjualan", parentColumnutama1, childColumnutama1, False)
            '    If ds.Relations.Count >= 2 Then
            '        ds.Relations.Clear()
            '    End If
            '    ds.Relations.Add(relationheader1)
            '    oda3.Dispose()
            '    ocmd2.Dispose()
            '    cn.Close()
            '    cn.Dispose()
            'End If
            '#End Region

            BS.DataSource = ds.Tables("Data")
            GC1.DataSource = BS.DataSource
            GridView1.PopulateColumns()
            If System.IO.File.Exists(FolderLayouts & Me.Name & XtraTabControl1.SelectedTabPage.Name & ".xml") Then
                GridView1.RestoreLayoutFromXml(FolderLayouts & Me.Name & XtraTabControl1.SelectedTabPage.Name & ".xml")
            End If
            'If System.IO.File.Exists(LayOutKu(Name & GridView1.Name & "_" & txtBarang.Text)) Then
            '    GridView1.RestoreLayoutFromXml(LayOutKu(Name & GridView1.Name & "_" & txtBarang.Text))
            'End If
            GV1.OptionsDetail.SmartDetailExpandButtonMode = DetailExpandButtonMode.AlwaysEnabled
            For Each ctrl As GridView In GC1.Views
                With ctrl
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
                                    .Columns(i).DisplayFormat.FormatString = "HH:mm"
                                ElseIf .Columns(i).FieldName.Trim.ToLower = "tanggalstart" Or .Columns(i).FieldName.Trim.ToLower = "tanggalend" Then
                                    .Columns(i).DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime
                                    .Columns(i).DisplayFormat.FormatString = "dd-MM-yyyy HH:mm"
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
            Next
            If TableMaster.ToUpper = "MJUAL".ToUpper Then
                GC1.Parent = XtraTabPage9
                SQL = "SELECT MJualD.Harga AS HargaBruto, MJual.Shift, MKategori.Kode + ' - ' + MKategori.Nama AS Kategori, (CASE WHEN MJualD.Qty=0 THEN MJualD.Harga ELSE MJualD.Harga-(MJualD.Harga*(((MJualD.Harga*MJualD.Qty)-MJualD.Jumlah)/(MJualD.Harga*MJualD.Qty)*100)/100) END) AS HargaNetto, MJualD.Harga*MJualD.Qty AS JumlahBruto, MJualD.Jumlah AS JumlahNetto, MJualD.Qty*(CASE WHEN MJualD.Qty=0 THEN 0 ELSE MJualD.Harga-(MJualD.Jumlah/MJualD.Qty) END) AS [TotalDiscRp], (CASE WHEN MJualD.Qty=0 THEN 0 ELSE MJualD.Harga-(MJualD.Jumlah/MJualD.Qty) END) AS [DiscRp], (CASE WHEN MJualD.Qty=0 THEN 0 ELSE ((MJualD.Harga*MJualD.Qty)-MJualD.Jumlah)/(MJualD.Harga*MJualD.Qty)*100 END) AS [Disc%], MJualD.*, MBarang.Kode AS KodeBarang, MBarang.Nama + ' ' + IsNull(MBarangD.Varian,'') AS NamaBarang, MBarangD.Varian AS [Varian/Ukuran], MSatuan.Kode AS Satuan, MJual.Kode, MJual.KodeReff, MJual.Tanggal, MJual.IDPos AS Kassa, MJual.NamaKasir" & vbCrLf & _
                      " FROM MJualD " & _
                      " INNER JOIN MJual ON MJualD.IDJual=MJual.NoID " & _
                      " LEFT JOIN MBarang ON MJualD.IDBarang=MBarang.NoID " & _
                      " LEFT JOIN MKategori ON MBarang.IDKategori=MKategori.NoID " & _
                      " LEFT JOIN MBarangD ON MJualD.IDBarangD=MBarangD.NoID " & _
                      " LEFT JOIN MSatuan ON MJualD.IDSatuan=MSatuan.NoID " & _
                      " WHERE 1=1 "
                If TglDari.Enabled Then
                    SQL &= " AND MJual.Tanggal>='" & TglDari.DateTime.ToString("yyyy/MM/dd") & "' AND MJual.Tanggal<'" & TglSampai.DateTime.AddDays(1).ToString("yyyy/MM/dd") & "' "
                End If
                If txtNamaBrg.Text <> "" Then
                    SQL &= " AND MJualD.IDBarang=" & NullToLong(txtBarang.EditValue)
                End If
                If txtNamaCustomer.Text <> "" Then
                    SQL &= " AND MJual.IDCustomer=" & NullToLong(txtKodeCustomer.EditValue)
                End If
                ocmd2.CommandText = SQL
                If Not ds.Tables("MJualDetail") Is Nothing Then
                    ds.Tables("MJualDetail").Clear()
                End If
                oda2.Fill(ds, "MJualDetail")
                GridControl7.DataSource = ds.Tables("MJualDetail")
                GridControl7.Parent = XtraTabPage1
            Else
                GC1.Parent = XtraTabControl1.SelectedTabPage
            End If

            'GridView7.PopulateColumns()
            If System.IO.File.Exists(FolderLayouts & FormName & GridView7.Name & ".xml") Then
                GridView7.RestoreLayoutFromXml(FolderLayouts & FormName & GridView7.Name & ".xml")
            End If
            GridView7.OptionsDetail.SmartDetailExpandButtonMode = DetailExpandButtonMode.AlwaysEnabled
            For Each ctrl As GridView In GridControl7.Views
                With ctrl
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
                                    .Columns(i).DisplayFormat.FormatString = "HH:mm"
                                ElseIf .Columns(i).FieldName.Trim.ToLower = "tanggalstart" Or .Columns(i).FieldName.Trim.ToLower = "tanggalend" Then
                                    .Columns(i).DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime
                                    .Columns(i).DisplayFormat.FormatString = "dd-MM-yyyy HH:mm"
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
            Next

            If TableMaster.ToUpper = "MLPB".ToUpper Then
                mnHasilPosting.Visibility = DevExpress.XtraBars.BarItemVisibility.Always
            ElseIf TableMaster.ToUpper = "MPO".ToUpper Then
                mnHasilPosting.Visibility = DevExpress.XtraBars.BarItemVisibility.Always
            ElseIf TableMaster.ToUpper = "MBeli".ToUpper Then
                mnHasilPosting.Visibility = DevExpress.XtraBars.BarItemVisibility.Always
            ElseIf TableMaster.ToUpper = "MReturBeli".ToUpper Then
                mnHasilPosting.Visibility = DevExpress.XtraBars.BarItemVisibility.Always
            ElseIf TableMaster.ToUpper = "MRevisiHargaBeli".ToUpper Then
                mnHasilPosting.Visibility = DevExpress.XtraBars.BarItemVisibility.Always
            ElseIf TableMaster.ToUpper = "MSO".ToUpper Then
                mnHasilPosting.Visibility = DevExpress.XtraBars.BarItemVisibility.Always
            ElseIf TableMaster.ToUpper = "MSPK".ToUpper Then
                mnHasilPosting.Visibility = DevExpress.XtraBars.BarItemVisibility.Always
            ElseIf TableMaster.ToUpper = "MPacking".ToUpper Then
                mnHasilPosting.Visibility = DevExpress.XtraBars.BarItemVisibility.Always
            ElseIf TableMaster.ToUpper = "MJual".ToUpper Then
                mnHasilPosting.Visibility = DevExpress.XtraBars.BarItemVisibility.Always
            ElseIf TableMaster.ToUpper = "MReturJual".ToUpper Then
                mnHasilPosting.Visibility = DevExpress.XtraBars.BarItemVisibility.Always
            ElseIf TableMaster.ToUpper = "MRevisiHargaJual".ToUpper Then
                mnHasilPosting.Visibility = DevExpress.XtraBars.BarItemVisibility.Always
            ElseIf TableMaster.ToUpper = "MMutasiGudang".ToUpper Then
                mnHasilPosting.Visibility = DevExpress.XtraBars.BarItemVisibility.Always
            Else
                mnHasilPosting.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
            End If

            If TableMaster.ToUpper = "MPO".ToUpper Then
                SQL = "SELECT MPOD.NoID, MPO.Kode AS NoPO, MGudang.Nama AS NamaGudang, MWilayah.Nama AS NamaWilayah, MBarang.Kode AS KodeBarang, MAlamat.Kode AS KodeSupplier, MAlamat.Nama AS NamaSupplier, MBarang.Nama AS NamaBarang, "
                SQL &= " (MPOD.Harga-isnull(MPOD.DISC1,0)-isnull(MPOD.DISC2,0)-isnull(MPOD.DISC3,0)"
                SQL &= " -(MPOD.Harga*isnull(MPOD.DiscPersen1,0)/100)"
                SQL &= " -(MPOD.Harga*isnull(MPOD.DiscPersen2,0)/100)"
                SQL &= " -(MPOD.Harga*isnull(MPOD.DiscPersen3,0)/100)"
                SQL &= " ) AS Harga , MPOD.Qty, MSatuan.Nama AS Satuan, MPOD.HargaPcs AS [Harga(Pcs)], MPOD.Qty*MPOD.Konversi AS [Qty(Pcs)], MPOD.Qty*MPOD.Konversi- " & DiBeli() & "  AS [SisaPO(Pcs)]"
                SQL &= " FROM MPOD LEFT JOIN MSatuan ON MSatuan.NoID=MPOD.IDSatuan "
                SQL &= " LEFT JOIN (MPO LEFT JOIN MAlamat ON MAlamat.NoID=MPO.IDSupplier) ON MPO.NoID=MPOD.IDPO "
                SQL &= " LEFT JOIN MBarang ON MBarang.NoID=MPOD.IDBarang "
                SQL &= " LEFT JOIN MGudang ON MGudang.NoID=MPOD.IDGudang "
                SQL &= " LEFT JOIN MWilayah ON MWilayah.NoID=MPOD.IDWilayah "
                SQL &= " WHERE (MPO.IsSelesai=0 OR MPO.IsSelesai IS NULL) AND MPO.IDWilayah=" & DefIDWilayah & " AND (MPOD.Qty*MPOD.Konversi - " & DiBeli() & ">0) AND MBarang.IsActive = 1 And MPO.IsPosted = 1 "
                dsT2 = ExecuteDataset("MPOD", SQL)
                GridControl1.DataSource = dsT2.Tables("MPOD")

                XtraTabPage2.PageVisible = True
                XtraTabPage2.Text = "Daftar PO Yang Menggantung"
                XtraTabPage3.PageVisible = False
                XtraTabPage4.PageVisible = False
            ElseIf TableMaster.ToUpper = "MLPB".ToUpper Then
                SQL = "SELECT MLPBD.NoID, MGudang.Nama AS NamaGudang, MWilayah.Nama AS NamaWilayah, MBarang.Kode AS KodeBarang, MBarang.Nama AS NamaBarang, MLPB.Kode AS NoLPB, MLPB.Tanggal, MAlamat.Nama AS Supplier, MBagPembelian.Nama AS Penerima, MLPBD.Qty * MLPBD.Konversi AS [QtyPcs], MLPBD.Qty*MLPBD.Konversi-" & DiSTB() & "  AS [Sisa(Pcs)] "
                SQL &= " FROM (MLPBD LEFT OUTER JOIN (MLPB LEFT OUTER JOIN MAlamat AS MBagPembelian ON MBagPembelian.NoID = MLPB.IDBagPembelian "
                SQL &= " LEFT OUTER JOIN MAlamat ON MLPB.IDSupplier = MAlamat.NoID)"
                SQL &= " ON MLPB.NoID=MLPBD.IDLPB) "
                SQL &= " LEFT OUTER JOIN MBarang ON MLPBD.IDBarang = MBarang.NoID "
                SQL &= " LEFT OUTER JOIN (MGudang LEFT OUTER JOIN MWilayah ON MWilayah.NoID=MGudang.IDWilayah) ON MGudang.NoID=MLPBD.IDGudang "
                SQL &= " WHERE MBarang.ISActive=1 AND (MLPBD.Qty*MLPBD.Konversi-" & DiSTB() & ">0) AND MLPB.IDWilayah=" & DefIDWilayah & " And MLPB.IsPosted = 1 "

                dsT2 = ExecuteDataset("MLPBD", SQL)
                GridControl1.DataSource = dsT2.Tables("MLPBD")
                XtraTabPage2.Text = "Daftar STB Yang Menggantung"
                XtraTabPage2.PageVisible = True
                XtraTabPage3.PageVisible = False
                XtraTabPage4.PageVisible = False
            ElseIf TableMaster.ToUpper = "MBeli".ToUpper Then
                'If JumlahWhere.Length >= 3 Then
                dsT2 = ExecuteDataset("MPOD", SQL & " and (ISNULL(MBeliD.IDPOD,0)=0 OR ISNULL(MBeliD.IDPOD,0)=-1)")
                'Else
                'dsT2 = ExecuteDataset("MPOD", SQL & " where (ISNULL(MBeliD.IDPOD,0)=0 OR ISNULL(MBeliD.IDPOD,0)=-1)")
                'End If
                GridControl1.DataSource = dsT2.Tables("MPOD")
                XtraTabPage2.Text = "Pembelian Tanpa Purchase Order"

                'If JumlahWhere.Length >= 2 Then
                dsT2 = ExecuteDataset("MLPBD", SQL & " and (ISNULL(MBeliD.IDLPBD,0)=0 OR ISNULL(MBeliD.IDLPBD,0)=-1)")
                'Else
                'dsT2 = ExecuteDataset("MLPBD", SQL & " where (ISNULL(MBeliD.IDLPBD,0)=0 OR ISNULL(MBeliD.IDLPBD,0)=-1)")
                'End If
                GridControl2.DataSource = dsT2.Tables("MLPBD")
                XtraTabPage3.Text = "Pembelian Tanpa STB"

                SQL = "SELECT MPO.Kode AS NoPO, MBeli.Kode AS NoPembelian, MBeli.Tanggal, MAlamat.Nama AS Supplier, MBarang.Nama AS NamaStock, MBarang.Kode AS KodeStock, MGudang.Nama AS Gudang, MWilayah.Nama AS Wilayah, SUM(MBeliD.Qty*MBeliD.Konversi) AS QtyPcs, SUM(MBeliD.Qty*MBeliD.Konversi)-(MPOD.Qty*MPOD.Konversi) AS SelisihPcs" & _
                      " FROM (MBeliD INNER JOIN MBeli ON MBeli.NoID=MBeliD.IDBeli) " & _
                      " INNER JOIN (MPOD INNER JOIN MPO ON MPO.NoID=MPOD.IDPO) ON MPOD.NoID=MBeliD.IDPOD" & _
                      " INNER JOIN MAlamat ON MBeli.IDSupplier = MAlamat.NoID" & _
                      " LEFT JOIN MBarang ON MBarang.NoID=MBeliD.IDBarang " & _
                      " LEFT JOIN (MGudang LEFT JOIN MWilayah ON MWilayah.NoID=MGudang.IDWilayah) ON MGudang.NoID=MBeliD.IDGudang" & _
                      " GROUP BY " & _
                      " MPOD.Qty, MPOD.Konversi, MPO.Kode, MBeli.IDWilayah, MBeli.Kode, MBeli.Tanggal, MAlamat.Nama, MBarang.Nama, MBarang.Kode, MGudang.Nama, MWilayah.Nama" & _
                      " HAVING " & _
                      " SUM(MBeliD.Qty*MBeliD.Konversi)-(MPOD.Qty*MPOD.Konversi)>0" & _
                      " AND MBeli.IDWilayah=" & DefIDWilayah
                If TglDari.Enabled Then
                    SQL &= " AND (MBeli.Tanggal>='" & Format(TglDari.DateTime, "yyyy/MM/dd") & "' AND MBeli.Tanggal<'" & Format(DateAdd(DateInterval.Day, 1, TglSampai.DateTime), "yyyy/MM/dd") & "') "
                End If
                dsT2 = ExecuteDataset("MPOD", SQL)
                GridControl3.DataSource = dsT2.Tables("MPOD")
                XtraTabPage4.Text = "Qty Pembelian Lebih Dari PO"

                SQL = "SELECT MLPB.Kode AS NoPenerimaan, MBeli.Kode AS NoPembelian, MBeli.Tanggal, MAlamat.Nama AS Supplier, MBarang.Nama AS NamaStock, MBarang.Kode AS KodeStock, MGudang.Nama AS Gudang, MWilayah.Nama AS Wilayah, SUM(MBeliD.Qty*MBeliD.Konversi) AS QtyPcs, SUM(MBeliD.Qty*MBeliD.Konversi)-(MLPBD.Qty*MLPBD.Konversi) AS SelisihPcs" & _
                      " FROM (MBeliD INNER JOIN MBeli ON MBeli.NoID=MBeliD.IDBeli) " & _
                      " INNER JOIN (MLPBD INNER JOIN MLPB ON MLPB.NoID=MLPBD.IDLPB) ON MLPBD.NoID=MBeliD.IDLPBD" & _
                      " INNER JOIN MAlamat ON MBeli.IDSupplier = MAlamat.NoID" & _
                      " LEFT JOIN MBarang ON MBarang.NoID=MBeliD.IDBarang " & _
                      " LEFT JOIN (MGudang LEFT JOIN MWilayah ON MWilayah.NoID=MGudang.IDWilayah) ON MGudang.NoID=MBeliD.IDGudang" & _
                      " GROUP BY " & _
                      " MLPBD.Qty, MLPBD.Konversi, MLPB.Kode, MBeli.IDWilayah, MBeli.Kode, MBeli.Tanggal, MAlamat.Nama, MBarang.Nama, MBarang.Kode, MGudang.Nama, MWilayah.Nama" & _
                      " HAVING " & _
                      " SUM(MBeliD.Qty*MBeliD.Konversi)-(MLPBD.Qty*MLPBD.Konversi)>0" & _
                      " AND MBeli.IDWilayah=" & DefIDWilayah
                If TglDari.Enabled Then
                    SQL &= " AND (MBeli.Tanggal>='" & Format(TglDari.DateTime, "yyyy/MM/dd") & "' AND MBeli.Tanggal<'" & Format(DateAdd(DateInterval.Day, 1, TglSampai.DateTime), "yyyy/MM/dd") & "') "
                End If
                dsT2 = ExecuteDataset("MLPB", SQL)
                GridControl4.DataSource = dsT2.Tables("MLPB")
                XtraTabPage5.Text = "Qty Pembelian Lebih Dari STB"

                SQL = "SELECT MPO.Kode AS NoPO, MPO.Tanggal AS TglPO, MAlamat.Nama AS Supplier, MBeli.Kode AS NoPembelian, MBeli.Tanggal, MBarang.Nama AS NamaStock, MBarang.Kode AS KodeStock, MBeliD.Qty, MBeliD.Konversi, MSatuan.Nama AS Satuan, MBeliD.HargaPcs-(MBeliD.HargaPcs*MBeli.DiskonNotaProsen/100) AS HargaNettoBeli, MPOD.HargaPcs-(MPOD.HargaPcs*MPO.DiskonNotaProsen/100) AS HargaNettoPO, CASE WHEN MBeliD.HargaPcs-MPOD.HargaPcs>0 THEN 'Lebih' WHEN MBeliD.HargaPcs-MPOD.HargaPcs<0 THEN 'Kurang' ELSE 'Sama' END AS Info" & _
                      " FROM (MBeliD INNER JOIN MBeli ON MBeli.NoID=MBeliD.IDBeli)" & _
                      " INNER JOIN (MPOD INNER JOIN MPO ON MPO.NoID=MPOD.IDPO) ON MBeliD.IDPOD=MPOD.NoID" & _
                      " LEFT JOIN MSatuan ON MSatuan.NoID=MBeliD.IDSatuan" & _
                      " LEFT JOIN MBarang ON MBarang.NoID=MBeliD.IDBarang" & _
                      " LEFT JOIN MAlamat ON MAlamat.NoID=MBeli.IDSupplier" & _
                      " WHERE MPOD.HargaPcs-(MPOD.HargaPcs*MPO.DiskonNotaProsen/100)<>MBeliD.HargaPcs-(MBeliD.HargaPcs*MBeli.DiskonNotaProsen/100) AND MBeli.IDWilayah=" & DefIDWilayah
                If TglDari.Enabled Then
                    SQL &= " AND (MBeli.Tanggal>='" & Format(TglDari.DateTime, "yyyy/MM/dd") & "' AND MBeli.Tanggal<'" & Format(DateAdd(DateInterval.Day, 1, TglSampai.DateTime), "yyyy/MM/dd") & "') "
                End If
                dsT2 = ExecuteDataset("MPOD", SQL)
                GridControl5.DataSource = dsT2.Tables("MPOD")
                XtraTabPage6.Text = "Harga Pembelian Tidak Sama Dengan PO"

                SQL = "SELECT MBeli.NoID, MPO.Kode AS NoPO, MAlamat.Nama AS Supplier, MBeli.Kode AS NoPembelian, MBeli.Tanggal, MBarang.Nama AS NamaStock, MBarang.Kode AS KodeStock, MBeliD.Qty, MBeliD.Konversi, MSatuan.Nama AS Satuan, MBeliD.HargaNetto-(MBeliD.HargaNetto*MBeli.DiskonNotaProsen/100) AS HargaNetto, " & _
                      " IsNull((SELECT TOP 1 MRevisiHargaBeliD.KoreksiBL FROM MRevisiHargaBeliD INNER JOIN MRevisiHargaBeli ON MRevisiHargaBeli.NoID=MRevisiHargaBeliD.IDRevisiHargaBeli WHERE MBeliD.NoID=MRevisiHargaBeliD.IDBeliD AND MBeli.NoID=MRevisiHargaBeliD.IDBeli AND MRevisiHargaBeli.IsPosted=1 ORDER BY MRevisiHargaBeli.Tanggal DESC),0) AS TotalPH, " & _
                      " (MBeliD.HargaNetto-(MBeliD.HargaNetto*MBeli.DiskonNotaProsen/100)-IsNull((SELECT TOP 1 MRevisiHargaBeliD.KoreksiBL FROM MRevisiHargaBeliD INNER JOIN MRevisiHargaBeli ON MRevisiHargaBeli.NoID=MRevisiHargaBeliD.IDRevisiHargaBeli WHERE MBeliD.NoID=MRevisiHargaBeliD.IDBeliD AND MBeli.NoID=MRevisiHargaBeliD.IDBeli AND MRevisiHargaBeli.IsPosted=1 ORDER BY MRevisiHargaBeli.Tanggal DESC),0))*MBeliD.Qty JumlahTotal " & _
                      " FROM (MBeliD INNER JOIN MBeli ON MBeli.NoID=MBeliD.IDBeli)" & _
                      " INNER JOIN (MPOD INNER JOIN MPO ON MPO.NoID=MPOD.IDPO) ON MBeliD.IDPOD=MPOD.NoID" & _
                      " LEFT JOIN MSatuan ON MSatuan.NoID=MBeliD.IDSatuan" & _
                      " LEFT JOIN MBarang ON MBarang.NoID=MBeliD.IDBarang" & _
                      " LEFT JOIN MAlamat ON MAlamat.NoID=MBeli.IDSupplier" & _
                      " WHERE MBeli.IDWilayah=" & DefIDWilayah
                If TglDari.Enabled Then
                    SQL &= " AND (MBeli.Tanggal>='" & Format(TglDari.DateTime, "yyyy/MM/dd") & "' AND MBeli.Tanggal<'" & Format(DateAdd(DateInterval.Day, 1, TglSampai.DateTime), "yyyy/MM/dd") & "') "
                End If
                If txtBarang.Text <> "" Then
                    SQL &= " AND MBeliD.IDBarang=" & NullToLong(txtBarang.EditValue)
                End If
                dsT2 = ExecuteDataset("MPOD", SQL)
                GridControl7.DataSource = dsT2.Tables("MPOD")
                XtraTabPage8.Text = "Pembelian Berdasarkan Datang Barang"

                SQL = "SELECT SUM((MKartuStok.QtyMasuk-MKartuStok.QtyKeluar)*MKartuStok.Konversi) FROM MKartuStok LEFT JOIN MGudang ON MGudang.NoID=MKartuStok.IDGudang WHERE MGudang.IsBS=0 AND MGudang.IDWilayah=" & DefIDWilayah
                If txtBarang.Text <> "" Then
                    SQL &= " AND MKartuStok.IDBarang=" & NullToLong(txtBarang.EditValue)
                End If
                txtStokSiapJual.EditValue = NullToDbl(EksekusiSQlSkalarNew(SQL))
                SQL = "SELECT SUM((MKartuStok.QtyMasuk-MKartuStok.QtyKeluar)*MKartuStok.Konversi) FROM MKartuStok LEFT JOIN MGudang ON MGudang.NoID=MKartuStok.IDGudang WHERE MGudang.IsBS=1 AND MGudang.IDWilayah=" & DefIDWilayah
                If txtBarang.Text <> "" Then
                    SQL &= " AND MKartuStok.IDBarang=" & NullToLong(txtBarang.EditValue)
                End If
                txtStokTidakSiapJual.EditValue = NullToDbl(EksekusiSQlSkalarNew(SQL))
                SQL = "SELECT SUM((MKartuStok.QtyMasuk-MKartuStok.QtyKeluar)*MKartuStok.Konversi) FROM MKartuStok LEFT JOIN MGudang ON MGudang.NoID=MKartuStok.IDGudang WHERE MGudang.IDWilayah=" & DefIDWilayah
                If txtBarang.Text <> "" Then
                    SQL &= " AND MKartuStok.IDBarang=" & NullToLong(txtBarang.EditValue)
                End If
                txtTotalStok.EditValue = NullToDbl(EksekusiSQlSkalarNew(SQL))

                SQL = "SELECT MBeli.NoID, MPO.Kode AS NoPO, MAlamat.Nama AS Supplier, MBeli.Kode AS NoPembelian, MBeli.Tanggal, MBarang.Nama AS NamaStock, MBarang.Kode AS KodeStock, MBeliD.Qty, MBeliD.Konversi, MSatuan.Nama AS Satuan, MBeliD.HargaNetto-(MBeliD.HargaNetto*MBeli.DiskonNotaProsen/100) AS HargaNetto, " & _
                      " IsNull((SELECT TOP 1 MRevisiHargaBeliD.KoreksiBL FROM MRevisiHargaBeliD INNER JOIN MRevisiHargaBeli ON MRevisiHargaBeli.NoID=MRevisiHargaBeliD.IDRevisiHargaBeli WHERE MBeliD.NoID=MRevisiHargaBeliD.IDBeliD AND MBeli.NoID=MRevisiHargaBeliD.IDBeli AND MRevisiHargaBeli.IsPosted=1 ORDER BY MRevisiHargaBeli.Tanggal DESC),0) AS TotalPH, " & _
                      " (MBeliD.HargaNetto-(MBeliD.HargaNetto*MBeli.DiskonNotaProsen/100)-IsNull((SELECT TOP 1 MRevisiHargaBeliD.KoreksiBL FROM MRevisiHargaBeliD INNER JOIN MRevisiHargaBeli ON MRevisiHargaBeli.NoID=MRevisiHargaBeliD.IDRevisiHargaBeli WHERE MBeliD.NoID=MRevisiHargaBeliD.IDBeliD AND MBeli.NoID=MRevisiHargaBeliD.IDBeli AND MRevisiHargaBeli.IsPosted=1 ORDER BY MRevisiHargaBeli.Tanggal DESC),0))*MBeliD.Qty JumlahTotal, " & _
                      " (SELECT SUM((MKartuStok.QtyMasuk-MKartuStok.QtyKeluar)*MKartuStok.Konversi) FROM MKartuStok LEFT JOIN MGudang ON MGudang.NoID=MKartuStok.IDGudang WHERE MGudang.IsBS=0 AND MGudang.IDWilayah=MBeli.IDWilayah AND MKartuStok.IDBarang=MBeliD.IDBarang) AS StokSiapJual, " & _
                      " (SELECT SUM((MKartuStok.QtyMasuk-MKartuStok.QtyKeluar)*MKartuStok.Konversi) FROM MKartuStok LEFT JOIN MGudang ON MGudang.NoID=MKartuStok.IDGudang WHERE MGudang.IsBS=1 AND MGudang.IDWilayah=MBeli.IDWilayah AND MKartuStok.IDBarang=MBeliD.IDBarang) AS StokTidakSiapJual, " & _
                      " (SELECT SUM((MKartuStok.QtyMasuk-MKartuStok.QtyKeluar)*MKartuStok.Konversi) FROM MKartuStok LEFT JOIN MGudang ON MGudang.NoID=MKartuStok.IDGudang WHERE MGudang.IDWilayah=MBeli.IDWilayah AND MKartuStok.IDBarang=MBeliD.IDBarang) AS TotalStok " & _
                      " FROM (MBeliD INNER JOIN MBeli ON MBeli.NoID=MBeliD.IDBeli)" & _
                      " INNER JOIN (MPOD INNER JOIN MPO ON MPO.NoID=MPOD.IDPO) ON MBeliD.IDPOD=MPOD.NoID" & _
                      " LEFT JOIN MSatuan ON MSatuan.NoID=MBeliD.IDSatuan" & _
                      " LEFT JOIN MBarang ON MBarang.NoID=MBeliD.IDBarang" & _
                      " LEFT JOIN MAlamat ON MAlamat.NoID=MBeli.IDSupplier" & _
                      " WHERE MBeli.IDWilayah=" & DefIDWilayah
                If TglDari.Enabled Then
                    SQL &= " AND (MBeli.Tanggal>='" & Format(TglDari.DateTime, "yyyy/MM/dd") & "' AND MBeli.Tanggal<'" & Format(DateAdd(DateInterval.Day, 1, TglSampai.DateTime), "yyyy/MM/dd") & "') "
                End If
                If txtSupplier.Text <> "" Then
                    SQL &= " AND MBeli.IDSupplier=" & NullToLong(txtSupplier.EditValue)
                End If

                dsT2 = ExecuteDataset("MPOD", SQL)
                GridControl6.DataSource = dsT2.Tables("MPOD")
                XtraTabPage7.Text = "Pembelian Berdasarkan Supplier"

                'XtraTabPage2.PageVisible = True
                'XtraTabPage3.PageVisible = True
                'XtraTabPage4.PageVisible = True
                'XtraTabPage5.PageVisible = True
                'XtraTabPage6.PageVisible = True
                'XtraTabPage7.PageVisible = True
                'XtraTabPage8.PageVisible = True
            ElseIf TableMaster.ToUpper = "MReturBeli".ToUpper Then
                If JumlahWhere.Length >= 1 Then
                    dsT2 = ExecuteDataset("MReturBeliD", SQL & " and (ISNULL(MReturBeliD.IDBeliD,0)=0 OR ISNULL(MReturBeliD.IDBeliD,0)=-1)")
                Else
                    dsT2 = ExecuteDataset("MReturBeliD", SQL & " where (ISNULL(MReturBeliD.IDBeliD,0)=0 OR ISNULL(MReturBeliD.IDBeliD,0)=-1)")
                End If
                GridControl1.DataSource = dsT2.Tables("MReturBeliD")
                XtraTabPage2.Text = "Returan Tanpa Pembelian"

                XtraTabPage2.PageVisible = True
                XtraTabPage3.PageVisible = False
                XtraTabPage4.PageVisible = False
            ElseIf TableMaster.ToUpper = "MSO".ToUpper Then
                SQL = "SELECT MSOD.NoID, MSO.Kode AS NoSO, MGudang.Nama AS NamaGudang, MBarang.KodeAlias AS KodeAlias, MBarang.NamaAlias, MWilayah.Nama AS NamaWilayah, MBarang.Kode AS KodeBarang, MAlamat.Kode AS KodeSupplier, MAlamat.Nama AS NamaSupplier, MBarang.Nama AS NamaBarang, "
                SQL &= " (MSOD.Harga-isnull(MSOD.DISC1,0)-isnull(MSOD.DISC2,0)-isnull(MSOD.DISC3,0)"
                SQL &= " -(MSOD.Harga*isnull(MSOD.DiscPersen1,0)/100)"
                SQL &= " -(MSOD.Harga*isnull(MSOD.DiscPersen2,0)/100)"
                SQL &= " -(MSOD.Harga*isnull(MSOD.DiscPersen3,0)/100)"
                SQL &= " ) AS Harga , MSOD.Qty, MSatuan.Nama AS Satuan, MSOD.HargaPcs AS [Harga(Pcs)], MSOD.Qty*MSOD.Konversi AS [Qty(Pcs)], MSOD.Qty*MSOD.Konversi- " & DiSPK() & "  AS [Sisa(Pcs)]"
                SQL &= " FROM MSOD LEFT JOIN MSatuan ON MSatuan.NoID=MSOD.IDSatuan "
                SQL &= " LEFT JOIN (MSO LEFT JOIN MAlamat ON MAlamat.NoID=MSO.IDCustomer) ON MSO.NoID=MSOD.IDSO "
                SQL &= " LEFT JOIN MBarang ON MBarang.NoID=MSOD.IDBarang "
                SQL &= " LEFT JOIN MGudang ON MGudang.NoID=MSOD.IDGudang "
                SQL &= " LEFT JOIN MWilayah ON MWilayah.NoID=MSOD.IDWilayah "
                SQL &= " WHERE MSO.IDWilayah=" & DefIDWilayah & " AND (MSOD.Qty*MSOD.Konversi - " & DiSPK() & ">0) AND MBarang.IsActive = 1 And MSO.IsPosted = 1 "
                dsT2 = ExecuteDataset("MSOD", SQL)
                GridControl1.DataSource = dsT2.Tables("MSOD")

                XtraTabPage2.PageVisible = True
                XtraTabPage2.Text = "SO Yang Menggantung"
                XtraTabPage3.PageVisible = False
                XtraTabPage4.PageVisible = False
            ElseIf TableMaster.ToUpper = "MSPK".ToUpper Then
                If JumlahWhere.Length >= 2 Then
                    dsT2 = ExecuteDataset("MSOD", SQL & " and (ISNULL(MSPKD.IDSOD,0)=0 OR ISNULL(MSPKD.IDSOD,0)=-1)")
                Else
                    dsT2 = ExecuteDataset("MSOD", SQL & " where (ISNULL(MSPKD.IDSOD,0)=0 OR ISNULL(MSPKD.IDSOD,0)=-1)")
                End If
                GridControl2.DataSource = dsT2.Tables("MSOD")
                XtraTabPage3.Text = "SPK Tanpa Sales Order"
                XtraTabPage3.PageVisible = True

                SQL = "SELECT MSPKD.NoID, MSPK.Kode AS NoSPK, MGudang.Nama AS NamaGudang, MBarang.KodeAlias AS KodeAlias, MBarang.NamaAlias, MWilayah.Nama AS NamaWilayah, MBarang.Kode AS KodeBarang, MAlamat.Kode AS KodeSupplier, MAlamat.Nama AS NamaSupplier, MBarang.Nama AS NamaBarang, "
                SQL &= " (MSPKD.Harga-isnull(MSPKD.DISC1,0)-isnull(MSPKD.DISC2,0)-isnull(MSPKD.DISC3,0)"
                SQL &= " -(MSPKD.Harga*isnull(MSPKD.DiscPersen1,0)/100)"
                SQL &= " -(MSPKD.Harga*isnull(MSPKD.DiscPersen2,0)/100)"
                SQL &= " -(MSPKD.Harga*isnull(MSPKD.DiscPersen3,0)/100)"
                SQL &= " ) AS Harga , MSPKD.Qty, MSatuan.Nama AS Satuan, MSPKD.HargaPcs AS [Harga(Pcs)], MSPKD.Qty*MSPKD.Konversi AS [Qty(Pcs)], MSPKD.Qty*MSPKD.Konversi- " & SPKDiPacking() & "  AS [Sisa(Pcs)]"
                SQL &= " FROM MSPKD LEFT JOIN MSatuan ON MSatuan.NoID=MSPKD.IDSatuan "
                SQL &= " LEFT JOIN (MSPK LEFT JOIN MAlamat ON MAlamat.NoID=MSPK.IDCustomer) ON MSPK.NoID=MSPKD.IDSPK "
                SQL &= " LEFT JOIN MBarang ON MBarang.NoID=MSPKD.IDBarang "
                SQL &= " LEFT JOIN MGudang ON MGudang.NoID=MSPK.IDGudang "
                SQL &= " LEFT JOIN MWilayah ON MWilayah.NoID=MSPK.IDWilayah "
                SQL &= " WHERE MSPK.IDWilayah=" & DefIDWilayah & " AND (MSPKD.Qty*MSPKD.Konversi - " & SPKDiPacking() & ">0) AND MBarang.IsActive = 1 And MSPK.IsPosted = 1 "
                dsT2 = ExecuteDataset("MSPKD", SQL)
                GridControl1.DataSource = dsT2.Tables("MSPKD")

                XtraTabPage2.PageVisible = True
                XtraTabPage2.Text = "SPK Yang Menggantung"
                XtraTabPage4.PageVisible = False
            ElseIf TableMaster.ToUpper = "MPacking".ToUpper Then
                If JumlahWhere.Length >= 4 Then
                    dsT2 = ExecuteDataset("MSPKD", SQL & " and (ISNULL(MPackingD.IDSPKD,0)=0 OR ISNULL(MPackingD.IDSPKD,0)=-1)")
                Else
                    dsT2 = ExecuteDataset("MSPKD", SQL & " where (ISNULL(MPackingD.IDSPKD,0)=0 OR ISNULL(MPackingD.IDSPKD,0)=-1)")
                End If
                GridControl2.DataSource = dsT2.Tables("MSPKD")
                XtraTabPage3.Text = "Packing Tanpa SPK"
                XtraTabPage3.PageVisible = True

                SQL = "SELECT MPackingD.NoID, MPacking.Kode AS NoPacking, MGudang.Nama AS NamaGudang, MBarang.KodeAlias AS KodeAlias, MBarang.NamaAlias, MWilayah.Nama AS NamaWilayah, MBarang.Kode AS KodeBarang, MAlamat.Kode AS KodeSupplier, MAlamat.Nama AS NamaSupplier, MBarang.Nama AS NamaBarang, "
                SQL &= " (MPackingD.Harga-isnull(MPackingD.DISC1,0)-isnull(MPackingD.DISC2,0)-isnull(MPackingD.DISC3,0)"
                SQL &= " -(MPackingD.Harga*isnull(MPackingD.DiscPersen1,0)/100)"
                SQL &= " -(MPackingD.Harga*isnull(MPackingD.DiscPersen2,0)/100)"
                SQL &= " -(MPackingD.Harga*isnull(MPackingD.DiscPersen3,0)/100)"
                SQL &= " ) AS Harga , MPackingD.Qty, MSatuan.Nama AS Satuan, MPackingD.HargaPcs AS [Harga(Pcs)], MPackingD.Qty*MPackingD.Konversi AS [Qty(Pcs)], MPackingD.Qty*MPackingD.Konversi- " & PackingDiJual() & "  AS [Sisa(Pcs)DiSJ]"
                SQL &= " FROM MPackingD LEFT JOIN MSatuan ON MSatuan.NoID=MPackingD.IDSatuan "
                SQL &= " LEFT JOIN (MPacking LEFT JOIN MAlamat ON MAlamat.NoID=MPacking.IDCustomer) ON MPacking.NoID=MPackingD.IDPacking "
                SQL &= " LEFT JOIN MBarang ON MBarang.NoID=MPackingD.IDBarang "
                SQL &= " LEFT JOIN MGudang ON MGudang.NoID=MPacking.IDGudang "
                SQL &= " LEFT JOIN MWilayah ON MWilayah.NoID=MPacking.IDWilayah "
                SQL &= " WHERE MPacking.IDWilayah=" & DefIDWilayah & " AND ((MPackingD.Qty*MPackingD.Konversi - " & PackingDiJual() & ">0)) AND MBarang.IsActive = 1 And MPacking.IsPosted = 1 "
                dsT2 = ExecuteDataset("MPackingD", SQL)
                GridControl1.DataSource = dsT2.Tables("MPackingD")

                XtraTabPage2.PageVisible = True
                XtraTabPage2.Text = "Packing Yang Menggantung Di Penjualan"

                'SQL = "SELECT MPacking.Kode AS KodePacking, MPacking.Tanggal, MGudang.Nama AS Gudang, MWilayah.Nama AS Wilayah, MAlamat.Nama AS Customer, " & _
                '      " MBarang.Kode AS KodeStock, MBarang.Nama AS NamaStock, SUM(MPackingD.Qty*MPackingD.Konversi) AS QtyPcs," & _
                '      " SUM(MPackingD.Qty*MPackingD.Konversi)-IsNull((SELECT SUM(MJualD.Qty*MJualD.Konversi) FROM MJualD INNER JOIN MJual ON MJual.NoID=MJualD.IDJual WHERE MJual.IDPacking=MPacking.NoID AND MJualD.IDBarang=MPackingD.IDBarang),0) AS SisaQty " & _
                '      " FROM MPackingD INNER JOIN MPacking ON MPacking.NoID=MPackingD.IDPacking " & _
                '      " LEFT JOIN MAlamat ON MAlamat.NoID=MPacking.IDCustomer " & _
                '      " LEFT JOIN (MGudang LEFT JOIN MWilayah ON MWilayah.NoID=MGudang.IDWilayah) ON MGudang.NoID=MPacking.IDGudang " & _
                '      " LEFT JOIN MBarang ON MBarang.NoID=MPackingD.IDBarang " & _
                '      " WHERE MPacking.IDWilayah = " & DefIDWilayah & " AND MBarang.IsActive=1 AND IsNull(MPacking.IsPosted, 0) <> 0 And IsNull(MPacking.IsSelesai, 0) = 0 " & _
                '      " GROUP BY MPacking.NoID, MPacking.IsPosted, MBarang.IsActive, MPacking.IsSelesai, MPackingD.IDBarang, MPacking.Kode, MPacking.Tanggal, MGudang.Nama, MWilayah.Nama, MAlamat.Nama, MBarang.Kode, MBarang.Nama, MPacking.IDWilayah " & _
                '      " HAVING SUM(MPackingD.Qty*MPackingD.Konversi)-IsNull((SELECT SUM(MJualD.Qty*MJualD.Konversi) FROM MJualD INNER JOIN MJual ON MJual.NoID=MJualD.IDJual WHERE MJual.IDPacking=MPacking.NoID AND MJualD.IDBarang=MPackingD.IDBarang),0)>0"
                'dsT2 = ExecuteDataset("MPackingD", SQL)
                'GridControl3.DataSource = dsT2.Tables("MPackingD")

                XtraTabPage4.PageVisible = False
                'XtraTabPage4.Text = "Daftar Packing Yang Menggantung Di Penjualan"

            ElseIf TableMaster.ToUpper = "MDO".ToUpper Then
                JumlahWhere = Split(strsql.ToLower, "where")
                If JumlahWhere.Length >= 2 Then
                    dsT2 = ExecuteDataset("MDOD", SQL & " and (ISNULL(MDOD.IDJualD,0)=0 OR ISNULL(MDOD.IDJualD,0)=-1)")
                Else
                    dsT2 = ExecuteDataset("MDOD", SQL & " where (ISNULL(MDOD.IDJualD,0)=0 OR ISNULL(MDOD.IDJualD,0)=-1)")
                End If
                GridControl1.DataSource = dsT2.Tables("MDOD")
                XtraTabPage2.Text = "SKB Tanpa Faktur Penjualan"
                XtraTabPage2.PageVisible = True

                XtraTabPage3.PageVisible = False
                XtraTabPage4.PageVisible = False
            ElseIf TableMaster.ToUpper = "MJual".ToUpper Then
                'If JumlahWhere.Length >= 2 Then
                '    dsT2 = ExecuteDataset("MPackingD", SQL & " and (ISNULL(MJualD.IDPackingD,0)=0 OR ISNULL(MJualD.IDPackingD,0)=-1)")
                'Else
                '    dsT2 = ExecuteDataset("MPackingD", SQL & " where (ISNULL(MJualD.IDPackingD,0)=0 OR ISNULL(MJualD.IDPackingD,0)=-1)")
                'End If
                'GridControl2.DataSource = dsT2.Tables("MPackingD")
                'XtraTabPage3.Text = "Penjualan Tanpa Packingan"
                'XtraTabPage3.PageVisible = True

                'SQL = "SELECT MJualD.NoID, MJual.Kode AS NoFaktur, MGudang.Nama AS NamaGudang, MBarang.KodeAlias AS KodeAlias, MBarang.NamaAlias, MWilayah.Nama AS NamaWilayah, MBarang.Kode AS KodeBarang, MAlamat.Kode AS KodeSupplier, MAlamat.Nama AS NamaSupplier, MBarang.Nama AS NamaBarang, "
                'SQL &= " (MJualD.Harga-isnull(MJualD.DISC1,0)-isnull(MJualD.DISC2,0)-isnull(MJualD.DISC3,0)"
                'SQL &= " -(MJualD.Harga*isnull(MJualD.DiscPersen1,0)/100)"
                'SQL &= " -(MJualD.Harga*isnull(MJualD.DiscPersen2,0)/100)"
                'SQL &= " -(MJualD.Harga*isnull(MJualD.DiscPersen3,0)/100)"
                'SQL &= " ) AS Harga , MJualD.Qty, MSatuan.Nama AS Satuan, MJualD.HargaPcs AS [Harga(Pcs)], MJualD.Qty*MJualD.Konversi AS [Qty(Pcs)], MJualD.Qty*MJualD.Konversi- " & JualDiDO() & "  AS [Sisa(Pcs)DiSJ]"
                'SQL &= " FROM MJualD LEFT JOIN MSatuan ON MSatuan.NoID=MJualD.IDSatuan "
                'SQL &= " LEFT JOIN (MJual LEFT JOIN MAlamat ON MAlamat.NoID=MJual.IDCustomer) ON MJual.NoID=MJualD.IDJual "
                'SQL &= " LEFT JOIN MBarang ON MBarang.NoID=MJualD.IDBarang "
                'SQL &= " LEFT JOIN MGudang ON MGudang.NoID=MJual.IDGudang "
                'SQL &= " LEFT JOIN MWilayah ON MWilayah.NoID=MJual.IDWilayah "
                'SQL &= " WHERE MJual.IDWilayah=" & DefIDWilayah & " AND ((MJualD.Qty*MJualD.Konversi - " & JualDiDO() & ">0)) AND MBarang.IsActive = 1 And MJual.IsPosted = 1 "
                'dsT2 = ExecuteDataset("MJualD", SQL)
                'GridControl1.DataSource = dsT2.Tables("MJualD")
                'XtraTabPage2.PageVisible = True
                XtraTabPage2.Text = "Penjualan Yang Menggantung Di SKB"

                XtraTabPage4.PageVisible = False
            ElseIf TableMaster.ToUpper = "MReturJual".ToUpper Then
                If JumlahWhere.Length >= 1 Then
                    dsT2 = ExecuteDataset("MReturJualD", SQL & " and (ISNULL(MReturJualD.IDJualD,0)=0 OR ISNULL(MReturJualD.IDJualD,0)=-1)")
                Else
                    dsT2 = ExecuteDataset("MReturJualD", SQL & " where (ISNULL(MReturJualD.IDJualD,0)=0 OR ISNULL(MReturJualD.IDJualD,0)=-1)")
                End If
                GridControl1.DataSource = dsT2.Tables("MReturJualD")
                XtraTabPage2.Text = "Returan Tanpa Penjualan"

                XtraTabPage2.PageVisible = True
                XtraTabPage3.PageVisible = False
                XtraTabPage4.PageVisible = False
            Else
                XtraTabPage2.PageVisible = False
                XtraTabPage3.PageVisible = False
                XtraTabPage4.PageVisible = False
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
            GV1.ShowFindPanel()
            GridView1.ShowFindPanel()
            GridView2.ShowFindPanel()
            GridView3.ShowFindPanel()
            GridView4.ShowFindPanel()
            GridView5.ShowFindPanel()
        End Try
    End Sub
    'Dim cn As New SqlConnection(StrKonSql)
    'Dim ocmd2 As New SqlCommand
    'Dim strsql As String = ""
    'Dim Cur As Cursor = Windows.Forms.Cursor.Current
    '    Windows.Forms.Cursor.Current = Cursors.WaitCursor
    'Dim SQL As String = ""
    'Dim dsT2 As New DataSet
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
    '' MsgBox(GV1.Columns(i).fieldname.ToString)
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
    '            Case "date"
    '                GV1.Columns(i).DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime
    '                GV1.Columns(i).DisplayFormat.FormatString = "dd-MM-yyyy"
    '            Case "datetime"
    '                GV1.Columns(i).DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime
    '                GV1.Columns(i).DisplayFormat.FormatString = "dd-MM-yyyy HH:mm"
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

    '    If TableMaster.ToUpper = "MSO".ToUpper Then
    '        mnGenerateSPK.Visibility = DevExpress.XtraBars.BarItemVisibility.Always
    '    ElseIf TableMaster.ToUpper = "MPO".ToUpper Then
    '        mnSetPOSelesai.Visibility = DevExpress.XtraBars.BarItemVisibility.Always
    '        mnSetPOBelumSelesai.Visibility = DevExpress.XtraBars.BarItemVisibility.Always
    '    Else
    '        mnSetPOBelumSelesai.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
    '        mnSetPOSelesai.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
    '        mnGenerateSPK.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
    '    End If

    '    ocmd2.Dispose()
    '    cn.Close()
    '    cn.Dispose()

    '    If TableMaster.ToUpper = "MBeli".ToUpper Then
    '        SQL = "SELECT MPOD.NoID, MPO.Kode, MBarang.Kode AS KodeBarang, MAlamat.Kode AS KodeSupplier, MAlamat.Nama AS NamaSupplier, MBarang.Nama AS NamaBarang, "
    '        SQL &= " (MPOD.Harga-isnull(MPOD.DISC1,0)-isnull(MPOD.DISC2,0)-isnull(MPOD.DISC3,0)"
    '        SQL &= " -(MPOD.Harga*isnull(MPOD.DiscPersen1,0)/100)"
    '        SQL &= " -(MPOD.Harga*isnull(MPOD.DiscPersen2,0)/100)"
    '        SQL &= " -(MPOD.Harga*isnull(MPOD.DiscPersen3,0)/100)"
    '        SQL &= " ) AS Harga , MPOD.Qty, MSatuan.Nama AS Satuan, MPOD.HargaPcs AS [Harga(Pcs)], MPOD.Qty*MPOD.Konversi AS [Qty(Pcs)], MPOD.Qty*MPOD.Konversi- " & DiBeli() & "  AS [Sisa(Pcs)]"
    '        SQL &= " FROM MPOD LEFT JOIN MSatuan ON MSatuan.NoID=MPOD.IDSatuan "
    '        SQL &= " LEFT JOIN (MPO LEFT JOIN MAlamat ON MAlamat.NoID=MPO.IDSupplier) ON MPO.NoID=MPOD.IDPO "
    '        SQL &= " LEFT JOIN MBarang ON MBarang.NoID=MPOD.IDBarang "
    '        SQL &= " WHERE (MPO.IsSelesai=0 OR MPO.IsSelesai IS NULL) AND (MPOD.Qty*MPOD.Konversi - " & DiBeli() & ">0) AND MBarang.IsActive = 1 And MPO.IsPosted = 1 "
    '        dsT2 = ExecuteDataset("MPOD", SQL)
    '        GridControl1.DataSource = dsT2.Tables("MPOD")
    '        For i As Integer = 0 To GridView1.Columns.Count - 1
    '' MsgBox(GV1.Columns(i).fieldname.ToString)
    '            Select Case GridView1.Columns(i).ColumnType.Name.ToLower

    '                Case "int32", "int64", "int"
    '                    GridView1.Columns(i).DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
    '                    GridView1.Columns(i).DisplayFormat.FormatString = "n0"
    '                Case "decimal", "single", "money", "double"
    '                    GridView1.Columns(i).DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
    '                    GridView1.Columns(i).DisplayFormat.FormatString = "n2"
    '                Case "string"
    '                    GridView1.Columns(i).DisplayFormat.FormatType = DevExpress.Utils.FormatType.None
    '                    GridView1.Columns(i).DisplayFormat.FormatString = ""
    '                Case "date"
    '                    GridView1.Columns(i).DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime
    '                    GridView1.Columns(i).DisplayFormat.FormatString = "dd-MM-yyyy"
    '                Case "datetime"
    '                    GridView1.Columns(i).DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime
    '                    GridView1.Columns(i).DisplayFormat.FormatString = "dd-MM-yyyy HH:mm"
    '                Case "byte[]"
    '                    reppicedit.SizeMode = DevExpress.XtraEditors.Controls.PictureSizeMode.Squeeze
    '                    GridView1.Columns(i).OptionsColumn.AllowGroup = False
    '                    GridView1.Columns(i).OptionsColumn.AllowSort = False
    '                    GridView1.Columns(i).OptionsFilter.AllowFilter = False
    '                    GridView1.Columns(i).ColumnEdit = reppicedit
    '                Case "boolean"
    '                    GridView1.Columns(i).ColumnEdit = repckedit
    '            End Select
    '            If GridView1.Columns(i).FieldName.Length >= 4 AndAlso GridView1.Columns(i).FieldName.Substring(0, 4).ToLower = "Kode".ToLower Then
    '                GridView1.Columns(i).Fixed = FixedStyle.Left
    '            ElseIf GridView1.Columns(i).FieldName.ToLower = "Nama".ToLower Then
    '                GridView1.Columns(i).Fixed = FixedStyle.Left
    '            End If
    '        Next

    '        SQL = "SELECT MLPBD.NoID, MBarang.Kode AS KodeBarang, MBarang.Nama AS NamaBarang, MLPB.Kode, MLPB.Tanggal, MAlamat.Nama AS Supplier, MBagPembelian.Nama AS Penerima, MLPBD.Qty * MLPBD.Konversi AS [QtyPcs], MLPBD.Qty*MLPBD.Konversi-" & DiSTB() & "  AS [Sisa(Pcs)] "
    '        SQL &= " FROM (MLPBD LEFT OUTER JOIN (MLPB LEFT OUTER JOIN MAlamat AS MBagPembelian ON MBagPembelian.NoID = MLPB.IDBagPembelian "
    '        SQL &= " LEFT OUTER JOIN MAlamat ON MLPB.IDSupplier = MAlamat.NoID)"
    '        SQL &= " ON MLPB.NoID=MLPBD.IDLPB) "
    '        SQL &= " LEFT OUTER JOIN MBarang ON MLPBD.IDBarang = MBarang.NoID "
    '        SQL &= " WHERE MBarang.ISActive=1 AND (MLPBD.Qty*MLPBD.Konversi-" & DiSTB() & ">0) And MLPB.IsPosted = 1 "

    '        dsT2 = ExecuteDataset("MLPBD", SQL)
    '        GridControl2.DataSource = dsT2.Tables("MLPBD")
    '        For i As Integer = 0 To GridView2.Columns.Count - 1
    '' MsgBox(GV1.Columns(i).fieldname.ToString)
    '            Select Case GridView2.Columns(i).ColumnType.Name.ToLower

    '                Case "int32", "int64", "int"
    '                    GridView2.Columns(i).DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
    '                    GridView2.Columns(i).DisplayFormat.FormatString = "n0"
    '                Case "decimal", "single", "money", "double"
    '                    GridView2.Columns(i).DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
    '                    GridView2.Columns(i).DisplayFormat.FormatString = "n2"
    '                Case "string"
    '                    GridView2.Columns(i).DisplayFormat.FormatType = DevExpress.Utils.FormatType.None
    '                    GridView2.Columns(i).DisplayFormat.FormatString = ""
    '                Case "date"
    '                    GridView2.Columns(i).DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime
    '                    GridView2.Columns(i).DisplayFormat.FormatString = "dd-MM-yyyy"
    '                Case "datetime"
    '                    GridView2.Columns(i).DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime
    '                    GridView2.Columns(i).DisplayFormat.FormatString = "dd-MM-yyyy HH:mm"
    '                Case "byte[]"
    '                    reppicedit.SizeMode = DevExpress.XtraEditors.Controls.PictureSizeMode.Squeeze
    '                    GridView2.Columns(i).OptionsColumn.AllowGroup = False
    '                    GridView2.Columns(i).OptionsColumn.AllowSort = False
    '                    GridView2.Columns(i).OptionsFilter.AllowFilter = False
    '                    GridView2.Columns(i).ColumnEdit = reppicedit
    '                Case "boolean"
    '                    GridView2.Columns(i).ColumnEdit = repckedit
    '            End Select
    '            If GridView2.Columns(i).FieldName.Length >= 4 AndAlso GridView2.Columns(i).FieldName.Substring(0, 4).ToLower = "Kode".ToLower Then
    '                GridView2.Columns(i).Fixed = FixedStyle.Left
    '            ElseIf GridView2.Columns(i).FieldName.ToLower = "Nama".ToLower Then
    '                GridView2.Columns(i).Fixed = FixedStyle.Left
    '            End If
    '        Next
    '    Else
    '        XtraTabPage2.PageVisible = False
    '    End If

    '    Windows.Forms.Cursor.Current = Cur

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
            If XtraTabControl1.SelectedTabPageIndex = 0 Then
                GV1.ExportToXls(dlgsave.FileName)
            ElseIf XtraTabControl1.SelectedTabPageIndex = 1 Then
                GridView1.ExportToXls(dlgsave.FileName)
            Else
                GridView1.ExportToXls(dlgsave.FileName)
            End If
            BukaFile(dlgsave.FileName)
        End If
        dlgsave.Dispose()
    End Sub

    Sub PrintPreview()
        If XtraTabControl1.SelectedTabPageIndex = 0 Then
            If TableMaster.ToUpper = "MRPTJual".ToUpper Then
                CetakMRPTJual(IIf(EditReport, action_.Edit, action_.Preview))
            ElseIf TableMaster.ToUpper = "vRekapPenjualan".ToUpper Then
                CetakvRekapPenjualan(IIf(EditReport, action_.Edit, action_.Preview))
            Else
                GC1.ShowPrintPreview()
            End If
        ElseIf XtraTabControl1.SelectedTabPageIndex = 1 Then
            GridControl1.ShowPrintPreview()
        Else
            GridControl2.ShowPrintPreview()
        End If
    End Sub
    Private Sub CetakMRPTJual(ByVal Action As action_)
        Dim namafile As String
        Dim strsql As String = ""
        Dim view As ColumnView = GC1.FocusedView
        Dim RefundFood As Double = 0.0 '46
        Dim RefundNonFood As Double = 0.0 '47
        Dim RefundFreshFood As Double = 0.0 '48
        Try
            strsql = "Select Sum(MJualD.Harga*MJuald.Qty) as Jumlah From " & _
                     "MJualD Inner Join MJual On MJualD.IDJual=MJual.NoID " & _
                     "Inner Join MBarang On MJualD.IDBarang=MBarang.NoID " & _
                     "Inner Join MKategori On MBarang.IDKategori=MKategori.NoID " & _
                     "where MJualD.Transaksi='RTN' and MKategori.IDParent=46 and MJual.Tanggal>= " & Format(TglDari.DateTime, "yyyy/MM/dd") & " and MJual.Tanggal<" & Format(TglSampai.DateTime.AddDays(1), "yyyy/MM/dd")
            If txtSupplier.Enabled Then
                strsql = strsql & " and MJual.IDPos=" & NullTolInt(txtSupplier.EditValue)
            End If
            RefundFood = EksekusiSQLSkalar(strsql)
            strsql = "Select Sum(MJualD.Harga*MJuald.Qty) as Jumlah From " & _
                     "MJualD Inner Join MJual On MJualD.IDJual=MJual.NoID " & _
                     "Inner Join MBarang On MJualD.IDBarang=MBarang.NoID " & _
                     "Inner Join MKategori On MBarang.IDKategori=MKategori.NoID " & _
                     "where MJualD.Transaksi='RTN' and MKategori.IDParent=47 and MJual.Tanggal>= " & Format(TglDari.DateTime, "yyyy/MM/dd") & " and MJual.Tanggal<" & Format(TglSampai.DateTime.AddDays(1), "yyyy/MM/dd")
            If txtSupplier.Enabled Then
                strsql = strsql & " and MJual.IDPos=" & NullTolInt(txtSupplier.EditValue)
            End If
            RefundNonFood = EksekusiSQLSkalar(strsql)

            strsql = "Select Sum(MJualD.Harga*MJuald.Qty) as Jumlah From " & _
                                                  "MJualD Inner Join MJual On MJualD.IDJual=MJual.NoID " & _
                                                  "Inner Join MBarang On MJualD.IDBarang=MBarang.NoID " & _
                                                  "Inner Join MKategori On MBarang.IDKategori=MKategori.NoID " & _
                                                  "where MJualD.Transaksi='RTN' and MKategori.IDParent=48 and MJual.Tanggal>= " & Format(TglDari.DateTime, "yyyy/MM/dd") & " and MJual.Tanggal<" & Format(TglSampai.DateTime.AddDays(1), "yyyy/MM/dd")
            If txtSupplier.Enabled Then
                strsql = strsql & " and MJual.IDPos=" & NullTolInt(txtSupplier.EditValue)
            End If
            RefundFreshFood = EksekusiSQLSkalar(strsql)

            namafile = Application.StartupPath & "\report\Laporan" & TableMaster & ".rpt"
            If System.IO.File.Exists(namafile) Then
                If EditReport Then
                    Action = action_.Edit
                Else
                    Action = action_.Preview
                End If
                If txtSupplier.Text <> "" Then
                    ViewReport(Me.ParentForm, Action, namafile, Me.Text, "cdate({vRekapPenjualanPerDepartemen.Tanggal})>={@DariTanggal} AND cdate({vRekapPenjualanPerDepartemen.Tanggal})<={@SampaiTanggal} AND {vRekapPenjualanPerDepartemen.IDPos}={@IDPOS}", , "DariTanggal=cdate(" & TglDari.DateTime.ToString("yyyy,MM,dd") & ")&SampaiTanggal=cdate(" & TglSampai.DateTime.ToString("yyyy,MM,dd") & ")&IDPOS=" & NullToLong(txtSupplier.EditValue) & "&RefundFood=" & CLng(RefundFood) & "&RefundNonFood=" & CLng(RefundNonFood) & "&RefundFreshFood=" & CLng(RefundFreshFood) & "")
                Else
                    ViewReport(Me.ParentForm, Action, namafile, Me.Text, "cdate({vRekapPenjualanPerDepartemen.Tanggal})>={@DariTanggal} AND cdate({vRekapPenjualanPerDepartemen.Tanggal})<={@SampaiTanggal}", , "DariTanggal=cdate(" & TglDari.DateTime.ToString("yyyy,MM,dd") & ")&SampaiTanggal=cdate(" & TglSampai.DateTime.ToString("yyyy,MM,dd") & ")" & "&RefundFood=" & CLng(RefundFood) & "&RefundNonFood=" & CLng(RefundNonFood) & "&RefundFreshFood=" & CLng(RefundFreshFood) & "")
                End If
            Else
                DevExpress.XtraEditors.XtraMessageBox.Show("Nama File :" & namafile & " tidak ditemukan.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Stop)
            End If
        Catch EX As Exception
            XtraMessageBox.Show("Untuk mencetak, pilih item yang mau dicetak dan click cetak faktur.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
            'DevExpress.XtraEditors.XtraMessageBox.Show("Kesalahan:" & EX.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub
    Private Sub CetakvRekapPenjualan(ByVal Action As action_)
        Dim namafile As String = ""
        Dim view As ColumnView = GC1.FocusedView
        Try
            namafile = Application.StartupPath & "\report\Laporan" & TableMaster & ".rpt"
            If System.IO.File.Exists(namafile) Then
                If EditReport Then
                    Action = action_.Edit
                Else
                    Action = action_.Preview
                End If
                If txtSupplier.Text <> "" Then
                    ViewReport(Me.ParentForm, Action, namafile, Me.Text, "cdate({vRekapPenjualan.Tanggal})>={@DariTanggal} AND cdate({vRekapPenjualan.Tanggal})<={@SampaiTanggal} AND {vRekapPenjualan.Kassa}={@IDPOS}", , "DariTanggal=cdate(" & TglDari.DateTime.ToString("yyyy,MM,dd") & ")&SampaiTanggal=cdate(" & TglSampai.DateTime.ToString("yyyy,MM,dd") & ")&IDPOS=" & NullToLong(txtSupplier.EditValue))
                Else
                    ViewReport(Me.ParentForm, Action, namafile, Me.Text, "cdate({vRekapPenjualan.Tanggal})>={@DariTanggal} AND cdate({vRekapPenjualan.Tanggal})<={@SampaiTanggal}", , "DariTanggal=cdate(" & TglDari.DateTime.ToString("yyyy,MM,dd") & ")&SampaiTanggal=cdate(" & TglSampai.DateTime.ToString("yyyy,MM,dd") & ")")
                End If
            Else
                DevExpress.XtraEditors.XtraMessageBox.Show("Nama File :" & namafile & " tidak ditemukan.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Stop)
            End If
        Catch EX As Exception
            XtraMessageBox.Show("Untuk mencetak, pilih item yang mau dicetak dan click cetak faktur.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
            'DevExpress.XtraEditors.XtraMessageBox.Show("Kesalahan:" & EX.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
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

    Private Sub LabelControl1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LabelControl1.Click
        TglDari.Enabled = Not TglDari.Enabled
        TglSampai.Enabled = TglDari.Enabled
    End Sub

    Private Sub CetakFaktur(ByVal action As action_)
        Dim namafile As String
        Dim view As ColumnView = GC1.FocusedView
        Try
            namafile = Application.StartupPath & "\report\Faktur" & TableMaster & ".rpt"
            If System.IO.File.Exists(namafile) Then
                Dim row As System.Data.DataRow = view.GetDataRow(GV1.FocusedRowHandle)
                Dim dc As Integer = GV1.FocusedRowHandle
                If row Is Nothing Then XtraMessageBox.Show("Untuk mencetak, pilih item yang mau dicetak dan click cetak faktur.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information) : Exit Sub
                Dim NoID As Long = NullToLong(row("NoID"))
                'ViewReport(action, namafile, "Faktur Penjualan", "{MJual.NoID}=" & NoID)
                If EditReport Then
                    action = action_.Edit
                Else
                    action = action_.Preview
                End If
                If Not EditReport Then
                    'mnPosting.PerformClick()
                End If
                ViewReport(Me.ParentForm, action, namafile, Me.Text, "{" & TableMaster & ".NoID}=" & NoID & " AND {" & TableMaster & ".IsPosted}=True")
                'CetakCRViewer(action, Me.MdiParent, namafile, "Cetak " & Me.Text, "{MJual.NoID}=" & NoID)
            Else
                DevExpress.XtraEditors.XtraMessageBox.Show("Nama File :" & namafile & " tidak ditemukan.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Stop)
            End If
        Catch EX As Exception
            XtraMessageBox.Show("Untuk mencetak, pilih item yang mau dicetak dan click cetak faktur.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
            'DevExpress.XtraEditors.XtraMessageBox.Show("Kesalahan:" & EX.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub
    Private Sub CetakFakturPanjang(ByVal action As action_)
        Dim namafile As String
        Dim view As ColumnView = GC1.FocusedView
        Try
            namafile = Application.StartupPath & "\report\FakturPanjang" & TableMaster & ".rpt"
            If System.IO.File.Exists(namafile) Then
                Dim row As System.Data.DataRow = view.GetDataRow(GV1.FocusedRowHandle)
                If row Is Nothing Then XtraMessageBox.Show("Untuk mencetak, pilih item yang mau dicetak dan click cetak faktur.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information) : Exit Sub
                Dim dc As Integer = GV1.FocusedRowHandle
                Dim NoID As Long = NullToLong(row("NoID"))

                'ViewReport(action, namafile, "Faktur Penjualan", "{MJual.NoID}=" & NoID)
                If EditReport Then
                    action = action_.Edit
                Else
                    action = action_.Preview
                End If
                If Not EditReport Then
                    'mnPosting.PerformClick()
                End If
                ViewReport(Me.ParentForm, action, namafile, Me.Text, "{" & TableMaster & ".NoID}=" & NoID)
                'CetakCRViewer(action, Me.MdiParent, namafile, "Cetak " & Me.Text, "{MJual.NoID}=" & NoID)
            Else
                DevExpress.XtraEditors.XtraMessageBox.Show("Nama File :" & namafile & " tidak ditemukan.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Stop)
            End If
        Catch EX As Exception
            XtraMessageBox.Show("Untuk mencetak, pilih item yang mau dicetak dan click cetak faktur.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
            'DevExpress.XtraEditors.XtraMessageBox.Show("Kesalahan:" & EX.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub GV1_FocusedRowChanged(ByVal sender As Object, ByVal e As DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs) Handles GV1.FocusedRowChanged
        Try
            If TableMaster.ToUpper = "MBeli".ToUpper Then
                If IsShowStock Then
                    clsPostingKartuStok.LihatHasilPosting(NullToLong(GV1.GetRowCellValue(GV1.FocusedRowHandle, "IDBeli")), TableMaster)
                End If
            ElseIf TableMaster.ToUpper = "MReturBeli".ToUpper Then
                If IsShowStock Then
                    clsPostingKartuStok.LihatHasilPosting(NullToLong(GV1.GetRowCellValue(GV1.FocusedRowHandle, "IDReturBeli")), TableMaster)
                End If
            ElseIf TableMaster.ToUpper = "MRevisiHargaBeli".ToUpper Then
                If IsShowStock Then
                    clsPostingKartuStok.LihatHasilPosting(NullToLong(GV1.GetRowCellValue(GV1.FocusedRowHandle, "IDRevisiHargaBeli")), TableMaster)
                End If
            Else
                If IsShowHasilPostingan Then
                    clsPostingKartuStok.HasilPosting(NullToLong(GV1.GetRowCellValue(GV1.FocusedRowHandle, "NoID")), TableMaster)
                End If
            End If
        Catch ex As Exception
            XtraMessageBox.Show("Info : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
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

    Private Sub mnSaveLayouts_ItemClick(ByVal sender As Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles mnSaveLayouts.ItemClick
        Dim xOtorisasi As New frmOtorisasiAdmin
        Try
            If XtraMessageBox.Show("Simpan Layout?", NamaAplikasi, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = Windows.Forms.DialogResult.Yes AndAlso xOtorisasi.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
                GV1.SaveLayoutToXml(FolderLayouts & FormName & ".xml")
                GridView1.SaveLayoutToXml(FolderLayouts & FormName & GridView1.Name & ".xml")
                GridView2.SaveLayoutToXml(FolderLayouts & FormName & GridView2.Name & ".xml")
                GridView3.SaveLayoutToXml(FolderLayouts & FormName & GridView3.Name & ".xml")
                GridView4.SaveLayoutToXml(FolderLayouts & FormName & GridView4.Name & ".xml")
                GridView5.SaveLayoutToXml(FolderLayouts & FormName & GridView5.Name & ".xml")
                GridView6.SaveLayoutToXml(FolderLayouts & FormName & GridView6.Name & ".xml")
                GridView7.SaveLayoutToXml(FolderLayouts & FormName & GridView7.Name & ".xml")
            End If
        Catch ex As Exception
            XtraMessageBox.Show("Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            xOtorisasi.Dispose()
        End Try
    End Sub

    Private Sub XtraTabControl1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles XtraTabControl1.Click

    End Sub

    Private Sub XtraTabControl1_SelectedPageChanged(ByVal sender As Object, ByVal e As DevExpress.XtraTab.TabPageChangedEventArgs) Handles XtraTabControl1.SelectedPageChanged

        If XtraTabControl1.SelectedTabPageIndex = 8 Then
            lbCustomer.Visible = False
            txtKodeCustomer.Visible = False
            txtNamaCustomer.Visible = False
            lbBrg.Visible = True
            txtKodeBrg.Visible = True
            txtNamaBrg.Visible = True
            RefreshDataBarang()
        ElseIf XtraTabControl1.SelectedTabPageIndex = 0 Then
            lbCustomer.Visible = True
            txtKodeCustomer.Visible = True
            txtNamaCustomer.Visible = True
            lbBrg.Visible = False
            txtKodeBrg.Visible = False
            txtNamaBrg.Visible = False
            RefreshDataKontak()
        End If
    End Sub

    Private Sub mnRefresh_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles mnRefresh.ItemClick
        SimpleButton8.PerformClick()
    End Sub

    Private Sub mnHasilPosting_ItemClick(ByVal sender As Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles mnHasilPosting.ItemClick
        Try
            If TableMaster.ToUpper = "MBeli".ToUpper Then
                clsPostingKartuStok.LihatHasilPosting(NullToLong(GV1.GetRowCellValue(GV1.FocusedRowHandle, "IDBeli")), TableMaster)
            ElseIf TableMaster.ToUpper = "MReturBeli".ToUpper Then
                clsPostingKartuStok.LihatHasilPosting(NullToLong(GV1.GetRowCellValue(GV1.FocusedRowHandle, "IDReturBeli")), TableMaster)
            ElseIf TableMaster.ToUpper = "MRevisiHargaBeli".ToUpper Then
                clsPostingKartuStok.LihatHasilPosting(NullToLong(GV1.GetRowCellValue(GV1.FocusedRowHandle, "IDRevisiHargaBeli")), TableMaster)
            ElseIf TableMaster.ToUpper = "MJual".ToUpper Then
                clsPostingKartuStok.LihatHasilPosting(NullToLong(GV1.GetRowCellValue(GV1.FocusedRowHandle, "IDJual")), TableMaster)
            ElseIf TableMaster.ToUpper = "MReturJual".ToUpper Then
                clsPostingKartuStok.LihatHasilPosting(NullToLong(GV1.GetRowCellValue(GV1.FocusedRowHandle, "IDReturJual")), TableMaster)
            ElseIf TableMaster.ToUpper = "MRevisiHargaJual".ToUpper Then
                clsPostingKartuStok.LihatHasilPosting(NullToLong(GV1.GetRowCellValue(GV1.FocusedRowHandle, "IDRevisiHargaJual")), TableMaster)
            Else
                clsPostingKartuStok.HasilPosting(NullToLong(GV1.GetRowCellValue(GV1.FocusedRowHandle, "NoID")), TableMaster)
            End If
        Catch ex As Exception
            XtraMessageBox.Show("Info : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
        End Try
    End Sub

    Private Sub txtKodeCustomer_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtKodeCustomer.EditValueChanged
        'Dim TglAdd As Long = 0
        Try
            txtNamaCustomer.Text = NullToStr(EksekusiSQlSkalarNew("SELECT Nama FROM MAlamat WHERE NoID=" & NullToLong(txtKodeCustomer.EditValue)))
            'txtAlamatCustomer.Text = NullToStr(EksekusiSQlSkalarNew("SELECT Alamat FROM MAlamat WHERE NoID=" & NullToLong(txtKodeCustomer.EditValue)))
            'TglAdd = NullToLong(EksekusiSQlSkalarNew("SELECT JatuhTempoCustomer FROM MAlamat WHERE NoID=" & NullToLong(txtKodeCustomer.EditValue)))
            'If TglAdd = 0 Then
            '    tglJatuhTempo.DateTime = DateAdd(DateInterval.Month, 2, Tgl.DateTime)
            'Else
            '    tglJatuhTempo.DateTime = DateAdd(DateInterval.Day, TglAdd, Tgl.DateTime)
            'End If
            'RefreshDataPacking()
        Catch ex As Exception

        End Try
    End Sub

    Private Sub txtKodeBrg_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtKodeBrg.EditValueChanged
        Try
            txtNamaBrg.Text = NullToStr(EksekusiSQlSkalarNew("SELECT Nama FROM MBarang WHERE NoID=" & NullToLong(txtKodeBrg.EditValue)))
        Catch ex As Exception
        End Try
    End Sub



    Private Sub XtraTabControl1_SelectedPageChanging(ByVal sender As Object, ByVal e As DevExpress.XtraTab.TabPageChangingEventArgs) Handles XtraTabControl1.SelectedPageChanging
        If e.PrevPage Is Nothing Then
        Else
            GridView1.SaveLayoutToXml(FolderLayouts & Me.Name & e.PrevPage.Name & ".xml")
        End If

    End Sub

    Private Sub mnSetPending_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles mnSetPending.ItemClick
        Dim x As New frmOtorisasiAdmin
        Try
            If x.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
                For Each i In GV1.GetSelectedRows
                    SetPending(NullToLong(GV1.GetRowCellValue(i, "NoID")), NamaFileDB, True)
                Next
                RefreshData()
            End If
        Catch ex As Exception
            XtraMessageBox.Show("Info Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
        End Try
    End Sub
    Private Sub SetPending(ByVal NoID As Long, ByVal PathDB As String, ByVal IsPending As Boolean)
        Dim oConn As OleDbConnection = Nothing
        Dim ocmd As OleDbCommand = Nothing
        Dim SQL As String = ""
        'Provider=Microsoft.Jet.OLEDB.4.0;Data Source=\\serverName\shareName\folder\myDatabase.mdb;User Id=admin;Password=;
        Try
            SQL = "UPDATE MSales SET IsPending=" & IIf(IsPending, 1, 0) & " WHERE NoID=" & NullToLong(NoID)
            oConn = New OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" & PathDB & ";User Id=admin;Password=;")
            ocmd = New OleDbCommand(SQL, oConn)
            oConn.Open()
            NoID = NullToLong(ocmd.ExecuteNonQuery)
        Catch ex As Exception
            XtraMessageBox.Show("Gagal mengeksekusi SQL :" & SQL & vbCrLf & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
            'InsertErrorLogDB("Gagal mengeksekusi SQL :" & SQL & vbCrLf & ex.Message)
        Finally
            If Not ocmd Is Nothing Then
                ocmd.Dispose()
            End If
            If Not oConn Is Nothing Then
                oConn.Close()
                oConn.Dispose()
            End If
        End Try
    End Sub
    Private Sub DownloadItemKassa(ByVal NoID As Long, ByVal PathDB As String)
        Dim oConn As OleDbConnection = Nothing
        Dim ocmd As OleDbCommand = Nothing
        Dim SQL As String = ""
        'Provider=Microsoft.Jet.OLEDB.4.0;Data Source=\\serverName\shareName\folder\myDatabase.mdb;User Id=admin;Password=;
        Try
            SQL = "UPDATE MSales SET IsSend=1, IsUpload=1 WHERE NoID=" & NullToLong(NoID)
            oConn = New OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" & PathDB & ";User Id=admin;Password=;")
            ocmd = New OleDbCommand(SQL, oConn)
            oConn.Open()
            NoID = NullToLong(ocmd.ExecuteNonQuery)
        Catch ex As Exception
            XtraMessageBox.Show("Gagal mengeksekusi SQL :" & SQL & vbCrLf & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
            'InsertErrorLogDB("Gagal mengeksekusi SQL :" & SQL & vbCrLf & ex.Message)
        Finally
            If Not ocmd Is Nothing Then
                ocmd.Dispose()
            End If
            If Not oConn Is Nothing Then
                oConn.Close()
                oConn.Dispose()
            End If
        End Try
    End Sub
    Private Sub mnDownload_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles mnDownload.ItemClick
        Dim x As New frmOtorisasiAdmin
        Try
            If x.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
                For Each i In GV1.GetSelectedRows
                    DownloadItemKassa(NullToLong(GV1.GetRowCellValue(i, "NoID")), NamaFileDB)
                Next
                RefreshData()
            End If
        Catch ex As Exception
            XtraMessageBox.Show("Info Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
        End Try
    End Sub

    Private Sub mnSetNonPending_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles mnSetNonPending.ItemClick
        Dim x As New frmOtorisasiAdmin
        Try
            If x.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
                For Each i In GV1.GetSelectedRows
                    SetPending(NullToLong(GV1.GetRowCellValue(i, "NoID")), NamaFileDB, False)
                Next
                RefreshData()
            End If
        Catch ex As Exception
            XtraMessageBox.Show("Info Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
        End Try
    End Sub

    Private Sub RadioButton1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles RadioButton1.Click
        RefreshData()
    End Sub
    Private Sub RadioButton2_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles RadioButton2.Click
        RefreshData()
    End Sub
    Private Sub RadioButton3_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles RadioButton3.Click
        RefreshData()
    End Sub

End Class