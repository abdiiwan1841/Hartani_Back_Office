Imports System.Data.SqlClient
Imports System.Data.SQLite
Imports System.Windows.Forms
Imports DevExpress.XtraEditors
Imports DevExpress.XtraEditors.Repository
Imports VPoint.mdlCetakCR
Imports DevExpress.Utils
Imports DevExpress.XtraGrid.Views.Base
Imports DevExpress.XtraGrid.Views.Grid.ViewInfo
Imports DevExpress.XtraGrid.Views.Grid

Public Class frmDaftarTransaksiAccounting
    Public FormName As String = ""
    Public FormEntriName As String = ""
    Public TableName As String = ""
    Public TableNameD As String = ""
    Public TableMaster As String = ""

    Public NoID As Long = -1
    Dim oda2 As SqlDataAdapter
    Dim ds As New DataSet
    Dim BS As New BindingSource
    Dim HargaPcs As Double

    Dim repckedit As New RepositoryItemCheckEdit
    Dim repdateedit As New RepositoryItemDateEdit
    Dim reptextedit As New RepositoryItemTextEdit
    Dim reppicedit As New RepositoryItemPictureEdit
    Public ShowNoID As Boolean = False
    Public DirectNoID As Long = -1

    Private Sub frmDaftarTransaksiAccounting_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Activated
        If IsSupervisor Then
            mnUnPosting.Visibility = DevExpress.XtraBars.BarItemVisibility.Always
        Else
            mnUnPosting.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
        End If
        If ShowNoID Then
            RefreshData()
            GV1.ClearSelection()
            GV1.FocusedRowHandle = GV1.LocateByDisplayText(0, GV1.Columns("NoID"), (DirectNoID).ToString("#,##0"))
            GV1.SelectRow(GV1.FocusedRowHandle)
            ShowNoID = False
        End If
    End Sub
    Private Sub frmDaftar_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing

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
            If FormName = "DaftarPembelianAccounting" Then
                lbCustomer.Visible = True
                txtCustomer.Visible = True
                lbCustomer.Text = "Supplier"
                RefreshPendukung()
            ElseIf FormName = "DaftarReturPembelianAccounting" Then
                lbCustomer.Visible = True
                txtCustomer.Visible = True
                lbCustomer.Text = "Supplier"
                RefreshPendukung()
            ElseIf FormName = "DaftarPenjualanAccounting" Then
                lbCustomer.Visible = True
                txtCustomer.Visible = True
                lbCustomer.Text = "Customer"
                RefreshPendukung()
            ElseIf FormName = "DaftarReturPenjualanAccounting" Then
                lbCustomer.Visible = True
                txtCustomer.Visible = True
                lbCustomer.Text = "Customer"
                RefreshPendukung()
            Else
                lbCustomer.Visible = False
                txtCustomer.Visible = False
            End If
            TglDari.EditValue = TanggalSystem
            TglSampai.EditValue = TanggalSystem

            Me.lbDaftar.Text = Me.Text
            XtraTabPage1.Text = Me.Text
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
    Private Sub RefreshPendukung()
        Dim ds As New DataSet
        Dim SQL As String
        Try
            Select Case FormName.ToUpper
                Case "DaftarPembelianAccounting".ToUpper, "DaftarReturPembelianAccounting".ToUpper
                    SQL = "SELECT NoID, Kode, Nama FROM MAlamat WHERE IsActive=1 AND IsSupplier=1"
                    ds = ExecuteDataset("master", SQL)
                    txtCustomer.Properties.DataSource = ds.Tables("master")
                    txtCustomer.Properties.DisplayMember = "Kode"
                    txtCustomer.Properties.ValueMember = "NoID"
                    If System.IO.File.Exists(FolderLayouts & Me.Name & gvCustomer.Name & ".xml") Then
                        gvCustomer.RestoreLayoutFromXml(FolderLayouts & Me.Name & gvCustomer.Name & ".xml")
                    End If
                    lbCustomer.Text = "Supplier"
                    lbCustomer.Visible = True
                    txtCustomer.Visible = True
                Case "DaftarPenjualanAccounting".ToUpper, "DaftarReturPenjualanAccounting".ToUpper
                    SQL = "SELECT NoID, Kode, Nama FROM MAlamat WHERE IsActive=1 AND IsCustomer=1"
                    ds = ExecuteDataset("master", SQL)
                    txtCustomer.Properties.DataSource = ds.Tables("master")
                    txtCustomer.Properties.DisplayMember = "Kode"
                    txtCustomer.Properties.ValueMember = "NoID"
                    If System.IO.File.Exists(FolderLayouts & Me.Name & gvCustomer.Name & ".xml") Then
                        gvCustomer.RestoreLayoutFromXml(FolderLayouts & Me.Name & gvCustomer.Name & ".xml")
                    End If
                    lbCustomer.Text = "Customer"
                    lbCustomer.Visible = True
                    txtCustomer.Visible = True
                Case Else
                    lbCustomer.Text = "Supplier"
                    lbCustomer.Visible = False
                    txtCustomer.Visible = False
            End Select
        Catch ex As Exception
            XtraMessageBox.Show("Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
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

        If System.IO.File.Exists(FolderLayouts & FormName & ".xml") Then
            GV1.RestoreLayoutFromXml(FolderLayouts & FormName & ".xml")
        End If
        If System.IO.File.Exists(FolderLayouts & FormName & GridView1.Name & ".xml") Then
            GridView1.RestoreLayoutFromXml(FolderLayouts & FormName & GridView1.Name & ".xml")
        End If
        If System.IO.File.Exists(FolderLayouts & FormName & GridView2.Name & ".xml") Then
            GridView2.RestoreLayoutFromXml(FolderLayouts & FormName & GridView2.Name & ".xml")
        End If
    End Sub

    Private Sub SimpleButton6_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SimpleButton6.Click
        Tutup()
    End Sub

    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

    End Sub

    Private Sub SimpleButton2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SimpleButton2.Click
        If NullToBool(Ini.BacaIni("Application", "UseFramework", False)) Then
            Edit()
        Else
            Select Case TableMaster.ToUpper
                Case "MJUAL"
                    Dim view As ColumnView = GC1.FocusedView
                    Dim x As New frmEntriJual
                    Try
                        Dim row As System.Data.DataRow = view.GetDataRow(GV1.FocusedRowHandle)
                        Dim dc As Integer = GV1.FocusedRowHandle
                        Dim IDDetil As Long = NullToLong(row("NoID"))
                        x.NoID = IDDetil
                        x.pTipe = frmEntriJual.pStatus.Edit
                        x.MdiParent = Me.MdiParent
                        x.WindowState = FormWindowState.Maximized

                        'For Back Action
                        x.FormNameDaftar = FormName
                        x.TableNameDaftar = TableName
                        x.TextDaftar = Text
                        x.FormEntriDaftar = FormEntriName
                        x.TableMasterDaftar = TableMaster

                        x.Show()
                        x.Focus()
                    Catch ex As Exception
                        XtraMessageBox.Show("Untuk mengedit pilih data yang akan diedit terlebih dahulu lalu tekan tombol edit!", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Warning)
                    End Try
                Case "MRETURJUAL"
                    Dim view As ColumnView = GC1.FocusedView
                    Dim x As New frmEntriReturJual
                    Try
                        Dim row As System.Data.DataRow = view.GetDataRow(GV1.FocusedRowHandle)
                        Dim dc As Integer = GV1.FocusedRowHandle
                        Dim IDDetil As Long = NullToLong(row("NoID"))
                        x.NoID = IDDetil
                        x.pTipe = frmEntriReturJual.pStatus.Edit
                        x.MdiParent = Me.MdiParent
                        x.WindowState = FormWindowState.Maximized

                        'For Back Action
                        x.FormNameDaftar = FormName
                        x.TableNameDaftar = TableName
                        x.TextDaftar = Text
                        x.FormEntriDaftar = FormEntriName
                        x.TableMasterDaftar = TableMaster

                        x.Show()
                        x.Focus()
                    Catch ex As Exception
                        XtraMessageBox.Show("Untuk mengedit pilih data yang akan diedit terlebih dahulu lalu tekan tombol edit!", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Warning)
                    End Try
                Case "MBELI", "vDaftarPembelianAccounting".ToUpper
                    If FormEntriName.ToUpper = "ENTRIPEMBELIANTANPABARANG" Then
                        Dim view As ColumnView = GC1.FocusedView
                        Dim x As New frmEntriBeliTanpaBarang
                        Try
                            Dim row As System.Data.DataRow = view.GetDataRow(GV1.FocusedRowHandle)
                            Dim dc As Integer = GV1.FocusedRowHandle
                            Dim IDDetil As Long = NullToLong(row("NoID"))
                            x.NoID = IDDetil
                            x.pTipe = frmEntriBeli.pStatus.Edit
                            x.MdiParent = Me.MdiParent
                            x.WindowState = FormWindowState.Maximized

                            'For Back Action
                            x.FormNameDaftar = FormName
                            x.TableNameDaftar = TableName
                            x.TextDaftar = Text
                            x.FormEntriDaftar = FormEntriName
                            x.TableMasterDaftar = TableMaster

                            x.Show()
                            x.Focus()
                        Catch ex As Exception
                            XtraMessageBox.Show("Untuk mengedit pilih data yang akan diedit terlebih dahulu lalu tekan tombol edit!", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Warning)
                        End Try
                    Else
                        Dim view As ColumnView = GC1.FocusedView
                        Dim x As New frmEntriBeli
                        Try
                            Dim row As System.Data.DataRow = view.GetDataRow(GV1.FocusedRowHandle)
                            Dim dc As Integer = GV1.FocusedRowHandle
                            Dim IDDetil As Long = NullToLong(row("NoID"))
                            x.NoID = IDDetil
                            x.pTipe = frmEntriBeli.pStatus.Edit
                            x.MdiParent = Me.MdiParent
                            x.WindowState = FormWindowState.Maximized

                            'For Back Action
                            x.FormNameDaftar = FormName
                            x.TableNameDaftar = TableName
                            x.TextDaftar = Text
                            x.FormEntriDaftar = FormEntriName
                            x.TableMasterDaftar = TableMaster

                            x.Show()
                            x.Focus()
                        Catch ex As Exception
                            XtraMessageBox.Show("Untuk mengedit pilih data yang akan diedit terlebih dahulu lalu tekan tombol edit!", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Warning)
                        End Try
                    End If
                Case "MRETURBELI"
                    If FormEntriName.ToUpper = "ENTRIRETURBELITANPABARANG" Then
                        Dim view As ColumnView = GC1.FocusedView
                        Dim x As New frmEntriReturBeliTanpaBarang
                        Try
                            Dim row As System.Data.DataRow = view.GetDataRow(GV1.FocusedRowHandle)
                            Dim dc As Integer = GV1.FocusedRowHandle
                            Dim IDDetil As Long = NullToLong(row("NoID"))
                            x.NoID = IDDetil
                            x.pTipe = frmEntriReturBeli.pStatus.Edit
                            x.MdiParent = Me.MdiParent
                            x.WindowState = FormWindowState.Maximized

                            'For Back Action
                            x.FormNameDaftar = FormName
                            x.TableNameDaftar = TableName
                            x.TextDaftar = Text
                            x.FormEntriDaftar = FormEntriName
                            x.TableMasterDaftar = TableMaster

                            x.Show()
                            x.Focus()
                        Catch ex As Exception
                            XtraMessageBox.Show("Untuk mengedit pilih data yang akan diedit terlebih dahulu lalu tekan tombol edit!", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Warning)
                        End Try
                    Else
                        Dim view As ColumnView = GC1.FocusedView
                        Dim x As New frmEntriReturBeli
                        Try
                            Dim row As System.Data.DataRow = view.GetDataRow(GV1.FocusedRowHandle)
                            Dim dc As Integer = GV1.FocusedRowHandle
                            Dim IDDetil As Long = NullToLong(row("NoID"))
                            x.NoID = IDDetil
                            x.pTipe = frmEntriReturBeli.pStatus.Edit
                            x.MdiParent = Me.MdiParent
                            x.WindowState = FormWindowState.Maximized

                            'For Back Action
                            x.FormNameDaftar = FormName
                            x.TableNameDaftar = TableName
                            x.TextDaftar = Text
                            x.FormEntriDaftar = FormEntriName
                            x.TableMasterDaftar = TableMaster

                            x.Show()
                            x.Focus()
                        Catch ex As Exception
                            XtraMessageBox.Show("Untuk mengedit pilih data yang akan diedit terlebih dahulu lalu tekan tombol edit!", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Warning)
                        End Try
                    End If

                Case "MRevisiHargaBeli".ToUpper
                    'Dim view As ColumnView = GC1.FocusedView
                    'Dim x As New frmEntriRevisiBeli
                    'Try
                    '    Dim row As System.Data.DataRow = view.GetDataRow(GV1.FocusedRowHandle)
                    '    Dim dc As Integer = GV1.FocusedRowHandle
                    '    Dim IDDetil As Long = NullToLong(row("NoID"))
                    '    x.NoID = IDDetil
                    '    x.pTipe = frmEntriRevisiBeli.pStatus.Edit
                    '    x.MdiParent = Me.MdiParent
                    '    x.WindowState = FormWindowState.Maximized

                    '    'For Back Action
                    '    x.FormNameDaftar = FormName
                    '    x.TableNameDaftar = TableName
                    '    x.TextDaftar = Text
                    '    x.FormEntriDaftar = FormEntriName
                    '    x.TableMasterDaftar = TableMaster

                    '    x.Show()
                    '    x.Focus()
                    'Catch ex As Exception
                    '    XtraMessageBox.Show("Untuk mengedit pilih data yang akan diedit terlebih dahulu lalu tekan tombol edit!", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Warning)
                    'End Try
                Case "MRevisiHargaJual".ToUpper
                    'Dim view As ColumnView = GC1.FocusedView
                    'Dim x As New frmEntriRevisiJual
                    'Try
                    '    Dim row As System.Data.DataRow = view.GetDataRow(GV1.FocusedRowHandle)
                    '    Dim dc As Integer = GV1.FocusedRowHandle
                    '    Dim IDDetil As Long = NullToLong(row("NoID"))
                    '    x.NoID = IDDetil
                    '    x.pTipe = frmEntriRevisiJual.pStatus.Edit
                    '    x.MdiParent = Me.MdiParent
                    '    x.WindowState = FormWindowState.Maximized

                    '    'For Back Action
                    '    x.FormNameDaftar = FormName
                    '    x.TableNameDaftar = TableName
                    '    x.TextDaftar = Text
                    '    x.FormEntriDaftar = FormEntriName
                    '    x.TableMasterDaftar = TableMaster

                    '    x.Show()
                    '    x.Focus()
                    'Catch ex As Exception
                    '    XtraMessageBox.Show("Untuk mengedit pilih data yang akan diedit terlebih dahulu lalu tekan tombol edit!", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Warning)
                    'End Try
                Case "MTT".ToUpper
                    Dim view As ColumnView = GC1.FocusedView
                    Dim x As New frmEntriTandaTerima
                    Try
                        Dim row As System.Data.DataRow = view.GetDataRow(GV1.FocusedRowHandle)
                        Dim dc As Integer = GV1.FocusedRowHandle
                        Dim IDDetil As Long = NullToLong(row("NoID"))
                        x.NoID = IDDetil
                        x.pTipe = frmEntriTandaTerima.pStatus.Edit
                        x.MdiParent = Me.MdiParent
                        x.WindowState = FormWindowState.Maximized
                        If FormName.ToUpper = "DaftarRevisiTandaTerimaSupplier".ToUpper Then
                            x.IsRevisi = True
                        Else
                            x.IsRevisi = False
                        End If

                        'For Back Action
                        x.FormNameDaftar = FormName
                        x.TableNameDaftar = TableName
                        x.TextDaftar = Text
                        x.FormEntriDaftar = FormEntriName
                        x.TableMasterDaftar = TableMaster

                        x.Show()
                        x.Focus()
                    Catch ex As Exception
                        XtraMessageBox.Show("Untuk mengedit pilih data yang akan diedit terlebih dahulu lalu tekan tombol edit!", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Warning)
                    End Try
            End Select
        End If

    End Sub

    Private Sub SimpleButton3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SimpleButton3.Click
        Hapus()
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
            frENTRI.MdiParent = Me.MdiParent
            frENTRI.WindowState = FormWindowState.Normal
            frENTRI.Show()
            frENTRI.Focus()

            'If frENTRI.ShowDialog(me) = Windows.Forms.DialogResult.OK Then
            '    RefreshData()
            '    GV1.FocusedRowHandle = GV1.LocateByDisplayText(0, GV1.Columns("NoID"), NoID.ToString("#,##0"))
            'End If
            'frENTRI.Dispose()
        End If
    End Sub
    Sub RefreshData()
        Dim dlg As DevExpress.Utils.WaitDialogForm = Nothing
        Dim cn As New SqlConnection(StrKonSql)
        Dim ocmd2 As New SqlCommand
        Dim Cur As Cursor = Windows.Forms.Cursor.Current
        Dim JumlahWhere As String()
        Try
            Dim curentcursor As Cursor = Windows.Forms.Cursor.Current
            Windows.Forms.Cursor.Current = Cursors.WaitCursor
            dlg = New DevExpress.Utils.WaitDialogForm(String.Format("Sedang merefresh data.{0}MOHON TUNGGU ...", vbCrLf), Application.ProductName)
            dlg.TopMost = False
            dlg.Show()
            Windows.Forms.Cursor.Current = Cursors.WaitCursor

            Dim SQL As String = ""
            Dim dsT2 As New DataSet
            Select Case TableMaster.ToUpper
                Case "vDaftarPembelianAccounting".ToUpper
                    SQL = " SELECT MAlamat.NoID, MAlamat.Kode AS KodeSupplier, MAlamat.Nama AS NamaSupplier, MAlamat.IsPKP AS [PKP / Non PKP], SUM(X.BKP) AS BKP, SUM(X.NBKP) AS NonBKP, SUM(X.BKP)+SUM(X.NBKP) AS Total, " & vbCrLf & _
                          " (SELECT MJurnal.Tanggal FROM MJurnal WHERE MJurnal.IDJenisTransaksi=2 AND MJurnal.IDAlamat=MAlamat.NoID AND MONTH(MJurnal.Tanggal)=" & TglDari.DateTime.Month & " AND YEAR(MJurnal.Tanggal)=" & TglDari.DateTime.Year & ") AS TglPosting, " & vbCrLf & _
                          " (SELECT MUser.Nama FROM MJurnal INNER JOIN MUser ON MUser.NoID=MJurnal.IDUserEntry WHERE MJurnal.IDJenisTransaksi=2 AND MJurnal.IDAlamat=MAlamat.NoID AND MONTH(MJurnal.Tanggal)=" & TglDari.DateTime.Month & " AND YEAR(MJurnal.Tanggal)=" & TglDari.DateTime.Year & ") AS UserPosting " & vbCrLf & _
                          " FROM MAlamat" & vbCrLf & _
                          " LEFT JOIN " & vbCrLf & _
                          " (SELECT MBarang.NoID, MBeli.IDSupplier, MBarang.IDKategori," & vbCrLf & _
                          " SUM(CASE WHEN MBarang.IDKategori IN (25,27,31,32) THEN 0 ELSE MBeliD.Jumlah END) AS BKP, SUM(CASE WHEN MBarang.IDKategori NOT IN (25,27,31,32) THEN 0 ELSE MBeliD.Jumlah END) AS NBKP" & vbCrLf & _
                          " FROM MBeli " & vbCrLf & _
                          " INNER JOIN MBeliD ON MBeli.NoID=MBeliD.IDBeli" & vbCrLf & _
                          " INNER JOIN MBarang ON MBarang.NoID=MBeliD.IDBarang" & vbCrLf & _
                          " WHERE MBeli.IsPosted=1 AND IsNull(MBeli.IsTanpaBarang,0)=0 AND MBeli.Tanggal>='" & TglDari.DateTime.ToString("yyyy-MM-dd") & "' AND MBeli.Tanggal<'" & TglSampai.DateTime.AddDays(1).ToString("yyyy-MM-dd") & "'" & vbCrLf & _
                          " GROUP BY MBarang.NoID, MBeli.IDSupplier, MBarang.IDKategori" & vbCrLf & _
                          " UNION ALL" & vbCrLf & _
                          " SELECT -1, MBeli.IDSupplier, MBeli.IDKategori, " & vbCrLf & _
                          " (CASE WHEN MBeli.IDKategori IN (25,27,31,32) THEN 0 ELSE MBeli.Total END) AS BKP," & vbCrLf & _
                          " (CASE WHEN MBeli.IDKategori NOT IN (25,27,31,32) THEN 0 ELSE MBeli.Total END) AS NBKP" & vbCrLf & _
                          " FROM MBeli " & vbCrLf & _
                          " LEFT JOIN MKategori ON MKategori.NoID=MBeli.IDKategori" & vbCrLf & _
                          " WHERE MBeli.IsPosted=1 AND IsNull(MBeli.IsTanpaBarang,0)=1 AND MBeli.Total>0 AND MBeli.Tanggal>='" & TglDari.DateTime.ToString("yyyy-MM-dd") & "' AND MBeli.Tanggal<'" & TglSampai.DateTime.AddDays(1).ToString("yyyy-MM-dd") & "'" & vbCrLf & _
                          " UNION ALL" & vbCrLf & _
                          " SELECT -1, MBeli.IDSupplier, -1, -1*MBeli.DiskonNotaTotal, 0" & vbCrLf & _
                          " FROM MBeli" & vbCrLf & _
                          " WHERE MBeli.IsPosted=1 AND IsNull(MBeli.IsTanpaBarang,0)=0 AND MBeli.Tanggal>='" & TglDari.DateTime.ToString("yyyy-MM-dd") & "' AND MBeli.Tanggal<'" & TglSampai.DateTime.AddDays(1).ToString("yyyy-MM-dd") & "'" & vbCrLf & _
                          " ) X ON X.IDSupplier=MAlamat.NoID " & vbCrLf & _
                          " WHERE MAlamat.IsSupplier=1 AND MAlamat.IsActive=1" & vbCrLf & _
                          " GROUP BY MAlamat.NoID, MAlamat.Kode, MAlamat.Nama, MAlamat.IsPKP "
                    SQL = "SELECT vBeli.* FROM (" & SQL & ") vBeli WHERE 1=1 "
                    If txtTampilkan.EditValue = CStr(1) Then 'Sudah diposting
                        SQL &= " AND NOT vBeli.TglPosting IS NULL "
                    ElseIf txtTampilkan.EditValue = CStr(2) Then 'Belum diposting
                        SQL &= " AND vBeli.TglPosting IS NULL "
                    End If
                    If txtCustomer.Enabled AndAlso txtCustomer.Text <> "" Then
                        SQL &= " AND vBeli.NoID=" & NullToLong(txtCustomer.EditValue)
                    End If
                Case "vDaftarReturPembelianAccounting".ToUpper
                    SQL = "SELECT MAlamat.NoID, MAlamat.Kode AS KodeSupplier, MAlamat.Nama AS NamaSupplier, MAlamat.IsPKP AS [PKP / Non PKP], SUM(X.BKP) AS BKP, SUM(X.NBKP) AS NonBKP, SUM(X.BKP)+SUM(X.NBKP) AS Total, " & vbCrLf & _
                          " (SELECT MJurnal.Tanggal FROM MJurnal WHERE MJurnal.IDJenisTransaksi=3 AND MJurnal.IDAlamat=MAlamat.NoID AND MONTH(MJurnal.Tanggal)=" & Month(TglDari.DateTime) & " AND YEAR(MJurnal.Tanggal)=" & Year(TglDari.DateTime) & ") AS TglPosting, " & vbCrLf & _
                          " (SELECT MUser.Nama FROM MJurnal INNER JOIN MUser ON MUser.NoID=MJurnal.IDUserEntry WHERE MJurnal.IDJenisTransaksi=3 AND MJurnal.IDAlamat=MAlamat.NoID AND MONTH(MJurnal.Tanggal)=" & Month(TglDari.DateTime) & " AND YEAR(MJurnal.Tanggal)=" & Year(TglDari.DateTime) & ") AS UserPosting " & vbCrLf & _
                          " FROM MAlamat" & vbCrLf & _
                          " LEFT JOIN " & vbCrLf & _
                          " (SELECT MBarang.NoID, MReturBeli.IDSupplier, MReturBeli.IsProsesPajak," & vbCrLf & _
                          " SUM(CASE WHEN IsNull(MReturBeli.IsProsesPajak,0) = 0 THEN 0 ELSE MReturBeliD.Jumlah END) AS BKP, SUM(CASE WHEN IsNull(MReturBeli.IsProsesPajak,0) = 1 THEN 0 ELSE MReturBeliD.Jumlah END) AS NBKP" & vbCrLf & _
                          " FROM MReturBeli " & vbCrLf & _
                          " INNER JOIN MReturBeliD ON MReturBeli.NoID=MReturBeliD.IDReturBeli" & vbCrLf & _
                          " INNER JOIN MBarang ON MBarang.NoID=MReturBeliD.IDBarang" & vbCrLf & _
                          " WHERE MReturBeli.IsPosted=1 AND IsNull(MReturBeli.IsTanpaBarang,0)=0 AND MReturBeli.Tanggal>='" & TglDari.DateTime.ToString("yyyy-MM-dd") & "' AND MReturBeli.Tanggal<'" & TglSampai.DateTime.AddDays(1).ToString("yyyy-MM-dd") & "'" & vbCrLf & _
                          " GROUP BY MBarang.NoID, MReturBeli.IDSupplier, MReturBeli.IsProsesPajak" & vbCrLf & _
                          " UNION ALL" & vbCrLf & _
                          " SELECT -1, MReturBeli.IDSupplier, MReturBeli.IsProsesPajak, " & vbCrLf & _
                          " (CASE WHEN IsNull(MReturBeli.IsProsesPajak,0) = 0 THEN 0 ELSE MReturBeli.Total END) AS BKP," & vbCrLf & _
                          " (CASE WHEN IsNull(MReturBeli.IsProsesPajak,0) = 1 THEN 0 ELSE MReturBeli.Total END) AS NBKP" & vbCrLf & _
                          " FROM MReturBeli " & vbCrLf & _
                          " LEFT JOIN MKategori ON MKategori.NoID=MReturBeli.IDKategori" & vbCrLf & _
                          " WHERE MReturBeli.IsPosted=1 AND IsNull(MReturBeli.IsTanpaBarang,0)=1 AND MReturBeli.Total>0 AND MReturBeli.Tanggal>='" & TglDari.DateTime.ToString("yyyy-MM-dd") & "' AND MReturBeli.Tanggal<'" & TglSampai.DateTime.AddDays(1).ToString("yyyy-MM-dd") & "'" & vbCrLf & _
                          " UNION ALL" & vbCrLf & _
                          " SELECT -1, MReturBeli.IDSupplier, -1, -1*MReturBeli.DiskonNotaTotal, 0" & vbCrLf & _
                          " FROM MReturBeli" & vbCrLf & _
                          " WHERE MReturBeli.IsPosted=1 AND IsNull(MReturBeli.IsTanpaBarang,0)=0 AND MReturBeli.Tanggal>='" & TglDari.DateTime.ToString("yyyy-MM-dd") & "' AND MReturBeli.Tanggal<'" & TglSampai.DateTime.AddDays(1).ToString("yyyy-MM-dd") & "'" & vbCrLf & _
                          " ) X ON X.IDSupplier=MAlamat.NoID " & vbCrLf & _
                          " WHERE MAlamat.IsSupplier=1 AND MAlamat.IsActive=1" & vbCrLf & _
                          " GROUP BY MAlamat.NoID, MAlamat.Kode, MAlamat.Nama, MAlamat.IsPKP "
                    SQL = "SELECT vReturBeli.* FROM (" & SQL & ") vReturBeli WHERE 1=1 "
                    If txtTampilkan.EditValue = CStr(1) Then 'Sudah diposting
                        SQL &= " AND NOT vReturBeli.TglPosting IS NULL "
                    ElseIf txtTampilkan.EditValue = CStr(2) Then 'Belum diposting
                        SQL &= " AND vReturBeli.TglPosting IS NULL "
                    End If
                    If txtCustomer.Enabled AndAlso txtCustomer.Text <> "" Then
                        SQL &= " AND vReturBeli.NoID=" & NullToLong(txtCustomer.EditValue)
                    End If
                Case Else
                    SQL = IIf(InStr(TableName, "select", CompareMethod.Text) > 0, TableName, "select * from " & TableName)
                    JumlahWhere = Split(SQL.ToLower, "WHERE")

                    If TglDari.Enabled Then
                        If InStr(SQL.ToLower, "where", CompareMethod.Text) > 0 Then
                            SQL &= " AND (" & TableMaster & ".Tanggal>='" & Format(TglDari.DateTime, "yyyy/MM/dd") & "' AND " & TableMaster & ".Tanggal<'" & Format(DateAdd(DateInterval.Day, 1, TglSampai.DateTime), "yyyy/MM/dd") & "') "
                        Else
                            SQL &= " where (" & TableMaster & ".Tanggal>='" & Format(TglDari.DateTime, "yyyy/MM/dd") & "' and " & TableMaster & ".Tanggal<'" & Format(DateAdd(DateInterval.Day, 1, TglSampai.DateTime), "yyyy/MM/dd") & "') "
                        End If
                    End If
                    If txtTampilkan.EditValue.ToString.Trim = "1" Then 'Posting
                        If InStr(SQL.ToLower, "where", CompareMethod.Text) > 0 Then
                            SQL &= " AND " & TableMaster & ".PostingJurnal=1"
                        Else
                            SQL &= " WHERE " & TableMaster & ".PostingJurnal=1"
                        End If
                    ElseIf txtTampilkan.EditValue.ToString.Trim = "2" Then 'Belum Posting
                        If InStr(SQL.ToLower, "where", CompareMethod.Text) > 0 Then
                            SQL &= " AND IsNull(" & TableMaster & ".PostingJurnal,0)=0"
                        Else
                            SQL &= " WHERE IsNull(" & TableMaster & ".PostingJurnal,0)=0"
                        End If
                    End If
                    If txtCustomer.Enabled Then
                        Select Case FormName.ToUpper
                            Case "DaftarPembelianAccounting".ToUpper, "DaftarReturPembelianAccounting".ToUpper
                                If InStr(SQL.ToLower, "where", CompareMethod.Text) > 0 Then
                                    SQL &= " AND (" & TableMaster & ".IDSupplier=" & NullTolInt(txtCustomer.EditValue) & ") "
                                Else
                                    SQL &= " where (" & TableMaster & ".IDSupplier=" & NullTolInt(txtCustomer.EditValue) & ") "
                                End If
                            Case "DaftarPenjualanAccounting".ToUpper, "DaftarReturPenjualanAccounting".ToUpper
                                If InStr(SQL.ToLower, "where", CompareMethod.Text) > 0 Then
                                    SQL &= " AND (" & TableMaster & ".IDCustomer=" & NullTolInt(txtCustomer.EditValue) & ") "
                                Else
                                    SQL &= " where (" & TableMaster & ".IDCustomer=" & NullTolInt(txtCustomer.EditValue) & ") "
                                End If
                            Case Else
                                SQL &= " "
                        End Select
                    End If
            End Select

            ocmd2.Connection = cn
            ocmd2.CommandType = CommandType.Text
            ocmd2.CommandText = SQL
            cn.Open()
            oda2 = New SqlDataAdapter(ocmd2)
            If ds.Tables("Data") Is Nothing Then
            Else
                ds.Tables("Data").Clear()
            End If
            oda2.Fill(ds, "Data")
            BS.DataSource = ds.Tables("Data")
            GC1.DataSource = BS.DataSource
            GV1.ShowFindPanel()

            XtraTabPage2.PageVisible = False
            XtraTabPage3.PageVisible = False
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
                frENTRI.MdiParent = Me.MdiParent
                frENTRI.WindowState = FormWindowState.Normal
                frENTRI.Show()
                frENTRI.Focus()

                'If frENTRI.ShowDialog(me) = Windows.Forms.DialogResult.OK Then
                '    RefreshData()
                '    GV1.ClearSelection()
                '    GV1.FocusedRowHandle = GV1.LocateByDisplayText(0, GV1.Columns("NoID"), NoID.ToString("#,##0"))
                '    GV1.SelectRow(GV1.FocusedRowHandle)
                'End If
                'frENTRI.Dispose()
            End If

            odr.Close()
            SQLcommand.Dispose()
            SQLconnect.Close()
            SQLconnect.Dispose()
        Catch ex As Exception
            XtraMessageBox.Show("Untuk mengedit pilih data yang akan diedit terlebih dahulu lalu tekan tombol edit", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally

        End Try
    End Sub
    Sub Hapus()
        Dim view As ColumnView = GC1.FocusedView
        Dim x As frmOtorisasiAdmin = Nothing
        Try
            Dim row As System.Data.DataRow = view.GetDataRow(GV1.FocusedRowHandle)
            Dim dc As Integer = GV1.FocusedRowHandle
            Dim NoID As Long = NullToLong(row("NoID"))
            Select Case TableMaster.ToUpper
                Case "MDEBETNOTE", "MCREDITNOTE", "MTITIPAN", "MTRANSFERPOIN"
                    If Not NullToBool(EksekusiSQlSkalarNew("SELECT isnull(IsPosted,0) FROM " & TableMaster.ToString & " WHERE NoID=" & NoID)) Then
                        If XtraMessageBox.Show(Me, "Yakin mau hapus data " & Text & " dengan Kode " & GV1.GetRowCellValue(GV1.FocusedRowHandle, "Kode"), NamaAplikasi, MessageBoxButtons.YesNo) = Windows.Forms.DialogResult.Yes Then
                            EksekusiSQL("DELETE FROM " & TableMaster & " WHERE NoID= " & NoID.ToString)
                            RefreshData()
                            GV1.ClearSelection()
                            GV1.FocusedRowHandle = GV1.LocateByDisplayText(0, GV1.Columns("NoID"), (NoID).ToString("#,##0"))
                            GV1.SelectRow(GV1.FocusedRowHandle)
                        End If
                    End If
                Case "MBELI", "MLPB", "MPO", "MRETURBELI", "MJUAL", "MRETURJUAL", "MSO", "MSPK", "MPACKING", "MDO", "MMUTASIGUDANG", "MREVISIHARGABELI", "MREVISIHARGAJUAL", "MTT"
                    If Not NullToBool(EksekusiSQlSkalarNew("SELECT isnull(IsPosted,0) FROM " & TableMaster.ToString & " WHERE NoID=" & NoID)) Then
                        If TableMaster.ToUpper = "MJual".ToUpper AndAlso NullToBool(row("IsPOS")) Then
                            If XtraMessageBox.Show(Me, "Yakin mau hapus data " & Text & " dengan Kode " & GV1.GetRowCellValue(GV1.FocusedRowHandle, "Kode"), NamaAplikasi, MessageBoxButtons.YesNo) = Windows.Forms.DialogResult.Yes Then
                                x = New frmOtorisasiAdmin
                                If x.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
                                    For Each i In GV1.GetSelectedRows
                                        NoID = NullToLong(GV1.GetDataRow(i).Item("NoID"))
                                        If EksekusiSQL("DELETE FROM " & TableMaster & " WHERE IsNull(IsPosted,0)=0 AND NoID= " & NoID.ToString) >= 1 Then
                                            'TableMaster = "MBeli"
                                            EksekusiSQL("DELETE FROM " & TableMaster & "D where ID" & TableMaster.Substring(1) & "= " & NoID.ToString)
                                        End If
                                    Next
                                    RefreshData()
                                    GV1.ClearSelection()
                                    GV1.FocusedRowHandle = GV1.LocateByDisplayText(0, GV1.Columns("NoID"), (NoID).ToString("#,##0"))
                                    GV1.SelectRow(GV1.FocusedRowHandle)
                                End If
                                x.Dispose()
                            End If
                        Else
                            If XtraMessageBox.Show(Me, "Yakin mau hapus data " & Text & " dengan Kode " & GV1.GetRowCellValue(GV1.FocusedRowHandle, "Kode"), NamaAplikasi, MessageBoxButtons.YesNo) = Windows.Forms.DialogResult.Yes Then
                                EksekusiSQL("DELETE FROM " & TableMaster & " where NoID= " & NoID.ToString)
                                'TableMaster = "MBeli"
                                EksekusiSQL("DELETE FROM " & TableMaster & "D where ID" & TableMaster.Substring(1) & "= " & NoID.ToString)
                                RefreshData()
                                GV1.ClearSelection()
                                GV1.FocusedRowHandle = GV1.LocateByDisplayText(0, GV1.Columns("NoID"), (NoID).ToString("#,##0"))
                                GV1.SelectRow(GV1.FocusedRowHandle)
                            End If
                        End If
                    Else
                        XtraMessageBox.Show("Data yang sudah diposting tidak boleh dihapus.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Warning)
                    End If
                    'Case "MREVISIHARGABELI", "MREVISIHARGAJUAL"
                    '    If Not NullTobool(EksekusiSQlSkalarNew("SELECT isnull(IsPosted,0) FROM " & TableMaster.ToString & " WHERE NoID=" & NoID)) Then
                    '        If XtraMessageBox.Show(Me, "Yakin mau hapus data " & Text & " dengan Kode " & GV1.GetRowCellValue(GV1.FocusedRowHandle, "Kode"), NamaAplikasi, MessageBoxButtons.YesNo) = Windows.Forms.DialogResult.Yes Then
                    '            EksekusiSQL("DELETE FROM " & TableMaster & " where NoID= " & NoID.ToString)
                    '            RefreshData()
                    '            GV1.ClearSelection()
                    '            GV1.FocusedRowHandle = GV1.LocateByDisplayText(0, GV1.Columns("NoID"), (NoID).ToString("#,##0"))
                    '            GV1.SelectRow(GV1.FocusedRowHandle)
                    '        End If
                    '    Else
                    '        XtraMessageBox.Show("Data yang sudah diposting tidak boleh dihapus.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Warning)
                    '    End If
                Case Else
                    If XtraMessageBox.Show(Me, "Yakin mau hapus data " & Me.Text & " dengan Kode " & GV1.GetRowCellValue(GV1.FocusedRowHandle, "Kode"), NamaAplikasi, MessageBoxButtons.YesNo) = Windows.Forms.DialogResult.Yes Then

                        EksekusiSQL("update " & TableMaster & " set IsActive=0 where NoID= " & NoID.ToString)
                        RefreshData()
                        GV1.ClearSelection()
                        GV1.FocusedRowHandle = GV1.LocateByDisplayText(0, GV1.Columns("NoID"), (NoID).ToString("#,##0"))
                        GV1.SelectRow(GV1.FocusedRowHandle)
                    End If

            End Select
        Catch ex As Exception
            XtraMessageBox.Show("Untuk menghapus pilih data yang akan dihapus terlebih dahulu lalu tekan tombol hapus!", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Warning)
        Finally
        End Try
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
            GC1.ShowPrintPreview()
        ElseIf XtraTabControl1.SelectedTabPageIndex = 1 Then
            GridControl1.ShowPrintPreview()
        Else
            GridControl2.ShowPrintPreview()
        End If
    End Sub

    Private Sub SimpleButton4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SimpleButton4.Click
        ExportExcel()
    End Sub

    Private Sub SimpleButton5_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SimpleButton5.Click
        Try
            Select Case TableMaster.ToUpper
                Case "MBeli".ToUpper, "vDaftarPembelianAccounting".ToUpper
                    clsPostingAccounting.LihatHasilPosting(NullToLong(GV1.GetRowCellValue(GV1.FocusedRowHandle, "NoID")), 2, TglDari.DateTime, Me)
                Case "MReturBeli".ToUpper, "vDaftarReturPembelianAccounting".ToUpper
                    clsPostingAccounting.LihatHasilPosting(NullToLong(GV1.GetRowCellValue(GV1.FocusedRowHandle, "NoID")), 3, TglDari.DateTime, Me)
                Case "MJual".ToUpper, "vDaftarPenjualanAccounting".ToUpper
                    clsPostingAccounting.LihatHasilPosting(NullToLong(GV1.GetRowCellValue(GV1.FocusedRowHandle, "NoID")), 6, TglDari.DateTime, Me)
                Case "MReturJual".ToUpper, "vDaftarReturPenjualanAccounting".ToUpper
                    clsPostingAccounting.LihatHasilPosting(NullToLong(GV1.GetRowCellValue(GV1.FocusedRowHandle, "NoID")), 7, TglDari.DateTime, Me)
                Case "MReturJual".ToUpper, "vDaftarPembayaranHutangAccounting".ToUpper
                    clsPostingAccounting.LihatHasilPosting(NullToLong(GV1.GetRowCellValue(GV1.FocusedRowHandle, "NoID")), 17, TglDari.DateTime, Me)
            End Select
        Catch ex As Exception
            XtraMessageBox.Show("Info Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
        End Try
    End Sub

    Private Sub BarButtonItem5_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem5.ItemClick
        ExportExcel()
    End Sub

    Private Sub LabelControl1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LabelControl1.Click
        TglDari.Enabled = Not TglDari.Enabled
        TglSampai.Enabled = TglDari.Enabled
    End Sub

    Private Sub cmdFaktur_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdFaktur.Click
        mnFaktur.PerformClick()
    End Sub
    Private Sub CetakFaktur(ByVal action As action_)
        Dim namafile As String
        Dim view As ColumnView = GC1.FocusedView
        Dim ds As New DataSet
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
                If CekMasihDiedit(NoID) Then Exit Try
                If Not EditReport Then
                    mnPosting.PerformClick()
                End If
                If TableMaster.ToLower = "MSPK".ToLower Then
                    ds = ExecuteDataset("MSPK", "SELECT IDGudang FROM MSPKD WHERE IDSPK=" & NoID & " GROUP BY IDGudang")
                    For i As Integer = 0 To ds.Tables("MSPK").Rows.Count - 1
                        ViewReport(Me.ParentForm, action, namafile, Me.Text, "{" & TableMaster & ".NoID}=" & NoID & " AND {" & TableMaster & ".IsPosted}=True AND {" & TableMaster & "D.IDGudang}=" & NullToLong(ds.Tables("MSPK").Rows(i).Item("IDGudang")))
                    Next
                Else
                    ViewReport(Me.ParentForm, action, namafile, Me.Text, "{" & TableMaster & ".NoID}=" & NoID & " AND {" & TableMaster & ".IsPosted}=True")
                End If
                'CetakCRViewer(action, Me.MdiParent, namafile, "Cetak " & Me.Text, "{MJual.NoID}=" & NoID)
            Else
                DevExpress.XtraEditors.XtraMessageBox.Show("Nama File :" & namafile & " tidak ditemukan.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Stop)
            End If
        Catch EX As Exception
            XtraMessageBox.Show("Untuk mencetak, pilih item yang mau dicetak dan click cetak faktur.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
            'DevExpress.XtraEditors.XtraMessageBox.Show("Kesalahan:" & EX.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            ds.Dispose()
        End Try
    End Sub
    Private Function CekMasihDiedit(ByVal NoID As Long) As Boolean
        Dim SQL As String = ""
        Dim ds As New DataSet
        Dim SudahDiposting As Boolean = False
        Try
            If TableMaster.ToUpper = "vDaftarPembelianAccounting".ToUpper Or TableMaster.ToUpper = "vDaftarReturPembelianAccounting".ToUpper Then
                Return False
            Else
                SQL = "SELECT * FROM " & TableMaster & " WHERE NoID=" & NoID
                ds = ExecuteDataset(TableMaster, SQL)
                If TableMaster.ToUpper = "vDaftarPembayaranHutangAccounting".ToUpper Then
                    SudahDiposting = NullToBool(ds.Tables(TableMaster).Rows(0).Item("PostingHutang"))
                Else
                    SudahDiposting = NullToBool(ds.Tables(TableMaster).Rows(0).Item("PostingStock"))
                End If
                If Not SudahDiposting Then 'sebelumnya PostingStok 2013-02-16
                    XtraMessageBox.Show("Transaksi ini belum cek.", NamaAplikasi, MessageBoxButtons.OK)
                    Return True
                Else
                    Return False
                End If
            End If
        Catch ex As Exception
            XtraMessageBox.Show("Info Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
            Return True
        Finally
            ds.Dispose()
        End Try
    End Function
    Private Sub CetakFakturPanjang(ByVal action As action_)
        Dim namafile As String
        Dim view As ColumnView = GC1.FocusedView
        Dim ds As New DataSet
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
                If CekMasihDiedit(NoID) Then Exit Try
                If Not EditReport Then
                    mnPosting.PerformClick()
                End If
                If TableMaster.ToLower = "MSPK".ToLower Then
                    ds = ExecuteDataset("MSPK", "SELECT IDGudang FROM MSPKD WHERE IDSPK=" & NoID & " GROUP BY IDGudang")
                    For i As Integer = 0 To ds.Tables("MSPK").Rows.Count - 1
                        ViewReport(Me.ParentForm, action, namafile, Me.Text, "{" & TableMaster & ".NoID}=" & NoID & " AND {" & TableMaster & ".IsPosted}=True AND {" & TableMaster & "D.IDGudang}=" & NullToLong(ds.Tables("MSPK").Rows(i).Item("IDGudang")))
                    Next
                Else
                    ViewReport(Me.ParentForm, action, namafile, Me.Text, "{" & TableMaster & ".NoID}=" & NoID & " AND {" & TableMaster & ".IsPosted}=True")
                End If
                'CetakCRViewer(action, Me.MdiParent, namafile, "Cetak " & Me.Text, "{MJual.NoID}=" & NoID)
            Else
                DevExpress.XtraEditors.XtraMessageBox.Show("Nama File :" & namafile & " tidak ditemukan.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Stop)
            End If
        Catch EX As Exception
            XtraMessageBox.Show("Untuk mencetak, pilih item yang mau dicetak dan click cetak faktur.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
            'DevExpress.XtraEditors.XtraMessageBox.Show("Kesalahan:" & EX.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            ds.Dispose()
        End Try
    End Sub
    Private Sub mnFaktur_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles mnFaktur.ItemClick
        If frmMain.ckEditRpt.Checked Then
            CetakFaktur(action_.Edit)
        ElseIf frmMain.ckLangsungCetak.Checked Then
            CetakFaktur(action_.Print)
        Else
            CetakFaktur(action_.Preview)
        End If
    End Sub

    Private Sub mnPosting_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles mnPosting.ItemClick
        Dim dlg As New WaitDialogForm("Sedang Proses Posting...", "Mohon Tunggu Sebentar.")
        Dim NoID As Long = -1, x As Integer = 0
        Try
            Enabled = False
            dlg.Owner = Me.MdiParent
            dlg.TopMost = True
            dlg.Show()
            Dim jumItem As Integer = GV1.SelectedRowsCount
            ProgressBarControl1.Visible = True
            For Each i In GV1.GetSelectedRows
                x += 1
                ProgressBarControl1.Position = x / IIf(GV1.GetSelectedRows.Length = 0, 1, GV1.GetSelectedRows.Length) * 100
                Application.DoEvents()
                NoID = NullToLong(GV1.GetDataRow(i).Item("NoID"))
                If CekMasihDiedit(NoID) Then Exit For
                Select Case TableMaster.ToUpper
                    Case "MBeli".ToUpper, "vDaftarPembelianAccounting".ToUpper
                        clsPostingAccounting.PostingJurnalPembelian(NoID, TglDari.DateTime)
                    Case "MReturBeli".ToUpper, "vDaftarReturPembelianAccounting".ToUpper
                        clsPostingAccounting.PostingJurnalReturPembelian(NoID, TglDari.DateTime)
                    Case "MJual".ToUpper, "vDaftarPenjualanAccounting".ToUpper
                        'clsPostingAccounting.PostingJurnalPenjualan(NoID)
                    Case "MBayarhutang".ToUpper, "VDAFTARPEMBAYARANHUTANGACCOUNTING".ToUpper
                        'clsPostingAccounting.PostingJurnalBayarHutang(NoID)
                    Case "MReturJual".ToUpper, "vDaftarReturPenjualanAccounting".ToUpper
                        'clsPostingAccounting.PostingJurnalPembelian(NoID)
                End Select
                Application.DoEvents()
            Next
            Enabled = True
            RefreshData()
            GV1.ClearSelection()
            GV1.FocusedRowHandle = GV1.LocateByDisplayText(0, GV1.Columns("NoID"), (NoID).ToString("#,##0"))
            GV1.SelectRow(GV1.FocusedRowHandle)
        Catch ex As Exception
            XtraMessageBox.Show("Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            dlg.Close()
            dlg.Dispose()
            Enabled = True
            ProgressBarControl1.Visible = False
            MdiParent.Focus()
            Focus()
        End Try
    End Sub

    Private Sub mnUnPosting_ItemClick(ByVal sender As Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles mnUnPosting.ItemClick
        Dim dlg As New WaitDialogForm("Proses UnPosting diproses...", "Mohon Tunggu Sebentar.")
        Dim NoID As Long = -1
        Dim x As Integer = 0
        Try
            Enabled = False
            dlg.TopMost = True
            dlg.Owner = Me.MdiParent
            dlg.Show()
            Dim jumItem As Integer = GV1.SelectedRowsCount
            ProgressBarControl1.Visible = True
            For Each i In GV1.GetSelectedRows
                x += 1
                ProgressBarControl1.Position = x / IIf(GV1.GetSelectedRows.Length = 0, 1, GV1.GetSelectedRows.Length) * 100
                Application.DoEvents()
                NoID = NullToLong(GV1.GetDataRow(i).Item("NoID"))
                Select Case TableMaster.ToUpper
                    Case "Mbeli".ToUpper, "vDaftarPembelianAccounting".ToUpper
                        clsPostingAccounting.UnPostingJurnalPembelian(NoID, TglDari.DateTime)
                    Case "MReturBeli".ToUpper, "vDaftarReturPembelianAccounting".ToUpper
                        clsPostingAccounting.UnPostingJurnalReturPembelian(NoID, TglDari.DateTime)
                    Case "MJual".ToUpper, "vDaftarPenjualanAccounting".ToUpper
                        'clsPostingAccounting.UnPostingJurnalPenjualan(NoID)
                    Case "MBayarhutang".ToUpper, "VDAFTARPEMBAYARANHUTANGACCOUNTING".ToUpper
                        'clsPostingAccounting.UnPostingJurnalBayarHutang(NoID)
                    Case "MReturJual".ToUpper, "vDaftarReturPenjualanAccounting".ToUpper
                        'clsPostingAccounting.UnPostingJurnalPembelian(NoID)
                End Select
                Application.DoEvents()
            Next
            Enabled = True
            RefreshData()
            GV1.ClearSelection()
            GV1.FocusedRowHandle = GV1.LocateByDisplayText(0, GV1.Columns("NoID"), (NoID).ToString("#,##0"))
            GV1.SelectRow(GV1.FocusedRowHandle)
        Catch ex As Exception
            XtraMessageBox.Show("Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            dlg.Close()
            dlg.Dispose()
            Enabled = True
            ProgressBarControl1.Visible = False
            MdiParent.Focus()
            Focus()
        End Try

    End Sub

    Private Sub GV1_FocusedRowChanged(ByVal sender As Object, ByVal e As DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs) Handles GV1.FocusedRowChanged
        'Try
        '    If IsShowStock Then
        '        clsPostingKartuStok.LihatHasilPosting(NullToLong(GV1.GetRowCellValue(GV1.FocusedRowHandle, "NoID")), TableMaster)
        '    End If
        'Catch ex As Exception
        '    XtraMessageBox.Show("Info : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
        'End Try
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

    Private Sub mnFakturPanjang_ItemClick(ByVal sender As Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles mnFakturPanjang.ItemClick
        If frmMain.ckEditRpt.Checked Then
            CetakFakturPanjang(action_.Edit)
        ElseIf frmMain.ckLangsungCetak.Checked Then
            CetakFakturPanjang(action_.Print)
        Else
            CetakFakturPanjang(action_.Preview)
        End If
    End Sub

    Private Sub cmdFakturPanjang_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdFakturPanjang.Click
        mnFakturPanjang.PerformClick()
    End Sub

    Private Sub SimpleButton1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SimpleButton1.Click
        If NullToBool(Ini.BacaIni("Application", "UseFramework", False)) Then
            Baru()
        Else
            Select Case TableMaster.ToUpper
                Case "MTITIPAN"
                    'Dim view As ColumnView = GC1.FocusedView
                    'Dim x As New frmEntriTitipan
                    'Try
                    '    x.NoID = -1
                    '    x.pTipe = frmEntriTitipan.pStatus.Baru
                    '    x.TabelMDI = Me.MdiParent
                    '    'x.MdiParent = Me.MdiParent
                    '    'x.WindowState = FormWindowState.Maximized

                    '    'For Back Action
                    '    x.FormNameDaftar = FormName
                    '    x.TableNameDaftar = TableName
                    '    x.TextDaftar = Text
                    '    x.FormEntriDaftar = FormEntriName
                    '    x.TableMasterDaftar = TableMaster

                    '    x.Show()
                    '    x.Focus()
                    'Catch ex As Exception
                    '    XtraMessageBox.Show("Error : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
                    'End Try
                Case "MCREDITNOTE"
                    'Dim view As ColumnView = GC1.FocusedView
                    'Dim x As New frmEntriNotaKredit
                    'Try
                    '    x.NoID = -1
                    '    x.pTipe = frmEntriNotaKredit.pStatus.Baru
                    '    x.TabelMDI = Me.MdiParent
                    '    'x.MdiParent = Me.MdiParent
                    '    'x.WindowState = FormWindowState.Maximized

                    '    'For Back Action
                    '    x.FormNameDaftar = FormName
                    '    x.TableNameDaftar = TableName
                    '    x.TextDaftar = Text
                    '    x.FormEntriDaftar = FormEntriName
                    '    x.TableMasterDaftar = TableMaster

                    '    x.Show()
                    '    x.Focus()
                    'Catch ex As Exception
                    '    XtraMessageBox.Show("Error : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
                    'End Try
                Case "MDEBETNOTE"
                    'Dim view As ColumnView = GC1.FocusedView
                    'Dim x As New frmEntriNotaDebet
                    'Try
                    '    x.NoID = -1
                    '    x.pTipe = frmEntriNotaDebet.pStatus.Baru
                    '    x.TabelMDI = Me.MdiParent
                    '    'x.MdiParent = Me.MdiParent
                    '    'x.WindowState = FormWindowState.Maximized

                    '    'For Back Action
                    '    x.FormNameDaftar = FormName
                    '    x.TableNameDaftar = TableName
                    '    x.TextDaftar = Text
                    '    x.FormEntriDaftar = FormEntriName
                    '    x.TableMasterDaftar = TableMaster

                    '    x.Show()
                    '    x.Focus()
                    'Catch ex As Exception
                    '    XtraMessageBox.Show("Error : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
                    'End Try
                Case "MMUTASIGUDANG"
                    Dim view As ColumnView = GC1.FocusedView
                    Dim x As New frmEntriMutasiGudang
                    Try
                        x.NoID = -1
                        x.pTipe = frmEntriMutasiGudang.pStatus.Baru
                        x.MdiParent = Me.MdiParent
                        x.WindowState = FormWindowState.Maximized

                        'For Back Action
                        x.FormNameDaftar = FormName
                        x.TableNameDaftar = TableName
                        x.TextDaftar = Text
                        x.FormEntriDaftar = FormEntriName
                        x.TableMasterDaftar = TableMaster

                        x.Show()
                        x.Focus()
                    Catch ex As Exception
                        XtraMessageBox.Show("Error : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
                    End Try
                Case "MLPB"
                    Dim view As ColumnView = GC1.FocusedView
                    Dim x As New frmEntriLPB
                    Try
                        x.NoID = -1
                        x.pTipe = frmEntriLPB.pStatus.Baru
                        x.MdiParent = Me.MdiParent
                        x.WindowState = FormWindowState.Maximized

                        'For Back Action
                        x.FormNameDaftar = FormName
                        x.TableNameDaftar = TableName
                        x.TextDaftar = Text
                        x.FormEntriDaftar = FormEntriName
                        x.TableMasterDaftar = TableMaster

                        x.Show()
                        x.Focus()
                    Catch ex As Exception
                        XtraMessageBox.Show("Error : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
                    End Try
                Case "MJUAL"
                    Dim view As ColumnView = GC1.FocusedView
                    Dim x As New frmEntriJual
                    Try
                        x.NoID = -1
                        x.pTipe = frmEntriJual.pStatus.Baru
                        x.MdiParent = Me.MdiParent
                        x.WindowState = FormWindowState.Maximized

                        'For Back Action
                        x.FormNameDaftar = FormName
                        x.TableNameDaftar = TableName
                        x.TextDaftar = Text
                        x.FormEntriDaftar = FormEntriName
                        x.TableMasterDaftar = TableMaster

                        x.Show()
                        x.Focus()
                    Catch ex As Exception
                        XtraMessageBox.Show("Error : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
                    End Try
                Case "MTT"
                    Dim view As ColumnView = GC1.FocusedView
                    Dim x As New frmEntriTandaTerima
                    Try
                        x.NoID = -1
                        x.pTipe = frmEntriTandaTerima.pStatus.Baru
                        x.MdiParent = Me.MdiParent
                        x.WindowState = FormWindowState.Maximized
                        If FormName.ToUpper = "DaftarRevisiTandaTerimaSupplier".ToUpper Then
                            x.IsRevisi = True
                        Else
                            x.IsRevisi = False
                        End If
                        'For Back Action
                        x.FormNameDaftar = FormName
                        x.TableNameDaftar = TableName
                        x.TextDaftar = Text
                        x.FormEntriDaftar = FormEntriName
                        x.TableMasterDaftar = TableMaster

                        x.pTrans = frmEntriTandaTerima.pTransaksi.Supplier
                        x.Show()
                        x.Focus()
                    Catch ex As Exception
                        XtraMessageBox.Show("Error : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
                    End Try
                Case "MRETURJUAL"
                    Dim view As ColumnView = GC1.FocusedView
                    Dim x As New frmEntriReturJual
                    Try
                        x.NoID = -1
                        x.pTipe = frmEntriReturJual.pStatus.Baru
                        x.MdiParent = Me.MdiParent
                        x.WindowState = FormWindowState.Maximized

                        'For Back Action
                        x.FormNameDaftar = FormName
                        x.TableNameDaftar = TableName
                        x.TextDaftar = Text
                        x.FormEntriDaftar = FormEntriName
                        x.TableMasterDaftar = TableMaster

                        x.Show()
                        x.Focus()
                    Catch ex As Exception
                        XtraMessageBox.Show("Error : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
                    End Try
                Case "MPO"
                    If FormName.ToLower = "daftarpurchaseorder" Then
                        Dim view As ColumnView = GC1.FocusedView
                        Dim x As New frmEntriPO
                        Try
                            x.NoID = -1
                            x.pTipe = frmEntriPO.pStatus.Baru
                            x.MdiParent = Me.MdiParent
                            x.WindowState = FormWindowState.Maximized

                            'For Back Action
                            x.FormNameDaftar = FormName
                            x.TableNameDaftar = TableName
                            x.TextDaftar = Text
                            x.FormEntriDaftar = FormEntriName
                            x.TableMasterDaftar = TableMaster

                            x.Show()
                            x.Focus()
                        Catch ex As Exception
                            XtraMessageBox.Show("Error : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
                        End Try
                    Else
                        Dim view As ColumnView = GC1.FocusedView
                        Dim x As New frmEntriPO
                        Try
                            x.NoID = -1
                            x.pTipe = frmEntriPO.pStatus.Baru
                            x.MdiParent = Me.MdiParent
                            x.WindowState = FormWindowState.Maximized

                            'For Back Action
                            x.FormNameDaftar = FormName
                            x.TableNameDaftar = TableName
                            x.TextDaftar = Text
                            x.FormEntriDaftar = FormEntriName
                            x.TableMasterDaftar = TableMaster

                            x.Show()
                            x.Focus()
                        Catch ex As Exception
                            XtraMessageBox.Show("Error : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
                        End Try
                    End If
                Case "MSO"
                    Dim view As ColumnView = GC1.FocusedView
                    Dim x As New frmEntriSO
                    Try
                        x.NoID = -1
                        x.pTipe = frmEntriSO.pStatus.Baru
                        x.MdiParent = Me.MdiParent
                        x.WindowState = FormWindowState.Maximized

                        'For Back Action
                        x.FormNameDaftar = FormName
                        If FormName.ToLower = "DaftarSalesOrder".ToLower Then
                            x.IsSO = True
                        Else
                            x.IsSO = False
                        End If
                        x.TableNameDaftar = TableName
                        x.TextDaftar = Text
                        x.FormEntriDaftar = FormEntriName
                        x.TableMasterDaftar = TableMaster

                        x.Show()
                        x.Focus()
                    Catch ex As Exception
                        XtraMessageBox.Show("Error : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
                    End Try
                Case "MSPK"
                    'Dim view As ColumnView = GC1.FocusedView
                    'Dim x As New frmEntriSPK
                    'Try
                    '    x.NoID = -1
                    '    x.pTipe = frmEntriSPK.pStatus.Baru
                    '    x.MdiParent = Me.MdiParent
                    '    x.WindowState = FormWindowState.Maximized

                    '    'For Back Action
                    '    x.FormNameDaftar = FormName
                    '    x.TableNameDaftar = TableName
                    '    x.TextDaftar = Text
                    '    x.FormEntriDaftar = FormEntriName
                    '    x.TableMasterDaftar = TableMaster

                    '    x.Show()
                    '    x.Focus()
                    'Catch ex As Exception
                    '    XtraMessageBox.Show("Error : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
                    'End Try
                Case "MPacking".ToUpper
                    'Dim view As ColumnView = GC1.FocusedView
                    'Dim x As New frmEntriPacking
                    'Try
                    '    x.NoID = -1
                    '    x.pTipe = frmEntriPacking.pStatus.Baru
                    '    x.MdiParent = Me.MdiParent
                    '    x.WindowState = FormWindowState.Maximized

                    '    'For Back Action
                    '    x.FormNameDaftar = FormName
                    '    x.TableNameDaftar = TableName
                    '    x.TextDaftar = Text
                    '    x.FormEntriDaftar = FormEntriName
                    '    x.TableMasterDaftar = TableMaster

                    '    x.Show()
                    '    x.Focus()
                    'Catch ex As Exception
                    '    XtraMessageBox.Show("Error : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
                    'End Try
                Case "MDO"
                    Dim view As ColumnView = GC1.FocusedView
                    Dim x As New frmEntriDO
                    Try
                        x.NoID = -1
                        x.pTipe = frmEntriDO.pStatus.Baru
                        x.MdiParent = Me.MdiParent
                        x.WindowState = FormWindowState.Maximized

                        'For Back Action
                        x.FormNameDaftar = FormName
                        x.TableNameDaftar = TableName
                        x.TextDaftar = Text
                        x.FormEntriDaftar = FormEntriName
                        x.TableMasterDaftar = TableMaster

                        x.Show()
                        x.Focus()
                    Catch ex As Exception
                        XtraMessageBox.Show("Error : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
                    End Try
                Case "MTransferPoin".ToUpper
                    Dim view As ColumnView = GC1.FocusedView
                    Dim x As New frmEntriTransferPoin
                    Try
                        x.NoID = -1
                        x.pTipe = frmEntriTransferPoin.pStatus.Baru

                        ''For Back Action
                        'x.FormNameDaftar = FormName
                        'x.TableNameDaftar = TableName
                        'x.TextDaftar = Text
                        'x.FormEntriDaftar = FormEntriName
                        'x.TableMasterDaftar = TableMaster

                        x.WindowState = FormWindowState.Normal
                        x.StartPosition = FormStartPosition.CenterParent

                        If x.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
                            RefreshData()
                            GV1.ClearSelection()
                            GV1.FocusedRowHandle = GV1.LocateByDisplayText(0, GV1.Columns("NoID"), x.NoID.ToString("##0"))
                            GV1.SelectRow(GV1.FocusedRowHandle)
                        End If
                    Catch ex As Exception
                        XtraMessageBox.Show("Error : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
                    End Try
                Case "MBELI"
                    If FormEntriName.ToUpper = "ENTRIPEMBELIANTANPABARANG" Then
                        Dim view As ColumnView = GC1.FocusedView
                        Dim x As New frmEntriBeliTanpaBarang
                        Try
                            x.NoID = -1
                            x.pTipe = frmEntriBeli.pStatus.Baru
                            x.MdiParent = Me.MdiParent
                            x.WindowState = FormWindowState.Maximized

                            'For Back Action
                            x.FormNameDaftar = FormName
                            x.TableNameDaftar = TableName
                            x.TextDaftar = Text
                            x.FormEntriDaftar = FormEntriName
                            x.TableMasterDaftar = TableMaster

                            x.Show()
                            x.Focus()
                        Catch ex As Exception
                            XtraMessageBox.Show("Error : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
                        End Try
                    Else
                        Dim view As ColumnView = GC1.FocusedView
                        Dim x As New frmEntriBeli
                        Try
                            x.NoID = -1
                            x.pTipe = frmEntriBeli.pStatus.Baru
                            x.MdiParent = Me.MdiParent
                            x.WindowState = FormWindowState.Maximized

                            'For Back Action
                            x.FormNameDaftar = FormName
                            x.TableNameDaftar = TableName
                            x.TextDaftar = Text
                            x.FormEntriDaftar = FormEntriName
                            x.TableMasterDaftar = TableMaster

                            x.Show()
                            x.Focus()
                        Catch ex As Exception
                            XtraMessageBox.Show("Error : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
                        End Try
                    End If


                Case "MRETURBELI"
                    If FormEntriName.ToUpper = "ENTRIRETURBELITANPABARANG" Then
                        Dim view As ColumnView = GC1.FocusedView
                        Dim x As New frmEntriReturBeliTanpaBarang
                        Try
                            x.NoID = -1
                            x.pTipe = frmEntriReturBeli.pStatus.Baru
                            x.MdiParent = Me.MdiParent
                            x.WindowState = FormWindowState.Maximized

                            'For Back Action
                            x.FormNameDaftar = FormName
                            x.TableNameDaftar = TableName
                            x.TextDaftar = Text
                            x.FormEntriDaftar = FormEntriName
                            x.TableMasterDaftar = TableMaster

                            x.Show()
                            x.Focus()
                        Catch ex As Exception
                            XtraMessageBox.Show("Error : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
                        End Try
                    Else
                        Dim view As ColumnView = GC1.FocusedView
                        Dim x As New frmEntriReturBeli
                        Try
                            x.NoID = -1
                            x.pTipe = frmEntriReturBeli.pStatus.Baru
                            x.MdiParent = Me.MdiParent
                            x.WindowState = FormWindowState.Maximized

                            'For Back Action
                            x.FormNameDaftar = FormName
                            x.TableNameDaftar = TableName
                            x.TextDaftar = Text
                            x.FormEntriDaftar = FormEntriName
                            x.TableMasterDaftar = TableMaster

                            x.Show()
                            x.Focus()
                        Catch ex As Exception
                            XtraMessageBox.Show("Error : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
                        End Try
                    End If

                Case "MRevisiHargaBeli".ToUpper
                    'Dim view As ColumnView = GC1.FocusedView
                    'Dim x As New frmEntriRevisiBeli
                    'Try
                    '    x.NoID = -1
                    '    x.pTipe = frmEntriRevisiBeli.pStatus.Baru
                    '    x.MdiParent = Me.MdiParent
                    '    x.WindowState = FormWindowState.Maximized

                    '    'For Back Action
                    '    x.FormNameDaftar = FormName
                    '    x.TableNameDaftar = TableName
                    '    x.TextDaftar = Text
                    '    x.FormEntriDaftar = FormEntriName
                    '    x.TableMasterDaftar = TableMaster

                    '    x.Show()
                    '    x.Focus()
                    'Catch ex As Exception
                    '    XtraMessageBox.Show("Error : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
                    'End Try
                Case "MRevisiHargaJual".ToUpper
                    'Dim view As ColumnView = GC1.FocusedView
                    'Dim x As New frmEntriRevisiJual
                    'Try
                    '    x.NoID = -1
                    '    x.pTipe = frmEntriRevisiJual.pStatus.Baru
                    '    x.MdiParent = Me.MdiParent
                    '    x.WindowState = FormWindowState.Maximized

                    '    'For Back Action
                    '    x.FormNameDaftar = FormName
                    '    x.TableNameDaftar = TableName
                    '    x.TextDaftar = Text
                    '    x.FormEntriDaftar = FormEntriName
                    '    x.TableMasterDaftar = TableMaster

                    '    x.Show()
                    '    x.Focus()
                    'Catch ex As Exception
                    '    XtraMessageBox.Show("Error : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
                    'End Try
            End Select
        End If
    End Sub

    Private Sub mnSaveLayouts_ItemClick(ByVal sender As Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles mnSaveLayouts.ItemClick
        Dim xOtorisasi As New frmOtorisasiAdmin
        Try
            If XtraMessageBox.Show("Simpan Layout?", NamaAplikasi, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = Windows.Forms.DialogResult.Yes AndAlso xOtorisasi.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
                GV1.SaveLayoutToXml(FolderLayouts & FormName & ".xml")
                GridView1.SaveLayoutToXml(FolderLayouts & FormName & GridView1.Name & ".xml")
                GridView2.SaveLayoutToXml(FolderLayouts & FormName & GridView2.Name & ".xml")
            End If
        Catch ex As Exception
            XtraMessageBox.Show("Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            xOtorisasi.Dispose()
        End Try
    End Sub

    Private Sub mnRefresh_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles mnRefresh.ItemClick
        'SimpleButton8.PerformClick()
        RefreshData()
    End Sub

    Private Sub mnHasilPosting_ItemClick(ByVal sender As Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles mnHasilPosting.ItemClick
        SimpleButton5.PerformClick()
    End Sub

    Private Sub lbCustomer_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lbCustomer.Click
        txtCustomer.Enabled = Not txtCustomer.Enabled
    End Sub

    Private Sub txtCustomer_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtCustomer.EditValueChanged
        Try
            txtNamaCustomer.Text = NullToStr(EksekusiSQlSkalarNew("SELECT Nama FROM MAlamat WHERE NoID=" & NullToLong(txtCustomer.EditValue)))
        Catch ex As Exception

        End Try
    End Sub

    Private Sub mnViewJurnalRekap_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles mnViewJurnalRekap.ItemClick
        Dim x As New frmHasilPostingAccounting
        Try
            If TableMaster.ToUpper = "vDaftarPembelianAccounting".ToUpper Then
                x.SQL = "SELECT MAkun.Kode, MAkun.Nama, MSubKlasAkun.Kode + ' - ' + MSubKlasAkun.Nama AS SubKlas, " & vbCrLf & _
                        " IsNull(MKlasAkun.Kode,'') + ' - ' + IsNull(MKlasAkun.Nama,'') AS KlasAkun, SUM(MJurnalD.Debet) AS Debet, SUM(MJurnalD.Kredit) AS Kredit " & vbCrLf & _
                        " FROM MAkun  " & vbCrLf & _
                        " INNER JOIN (MJurnal INNER JOIN MJurnalD ON MJurnal.ID=MJurnalD.IDJurnal) ON MAkun.ID=MJurnalD.IDAkun " & vbCrLf & _
                        " LEFT JOIN (MSubKlasAkun LEFT JOIN MKlasAkun ON MKlasAkun.ID=MSubKlasAkun.IDKlasAkun) ON MSubKlasAkun.ID=MAkun.IDSubKlasAkun " & vbCrLf & _
                        " WHERE MJurnal.IDJenisTransaksi=2 AND MONTH(MJurnal.Tanggal)=" & Month(TglDari.DateTime) & " And Year(MJurnal.Tanggal) = " & Year(TglDari.DateTime) & vbCrLf & _
                        " GROUP BY MAkun.Kode, MAkun.Nama, MSubKlasAkun.Kode + ' - ' + MSubKlasAkun.Nama,  " & vbCrLf & _
                        " IsNull(MKlasAkun.Kode,'') + ' - ' + IsNull(MKlasAkun.Nama,'')"
                x.FormName = "Pembelian"
                x.Periode = TglDari.DateTime
            End If
            x.ShowDialog(Me)
        Catch ex As Exception
            XtraMessageBox.Show("Info Kesalahn : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
        Finally
            x.Dispose()
        End Try
    End Sub
End Class