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

Public Class frmDaftarMasterDetil
    Public FormName As String = ""
    Public FormEntriName As String = ""
    Public TableName As String = ""
    Public TableNameD As String = ""
    Public TableMaster As String = ""
    Public CaptionCetak1 As String = ""
    Public CaptionCetak2 As String = ""

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

    Private Sub frmDaftarMasterDetil_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Activated
        If Not IsSupervisor AndAlso (TableMaster.ToUpper = "MBeli".ToUpper) Then
            mnUnPosting.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
        Else
            mnUnPosting.Visibility = DevExpress.XtraBars.BarItemVisibility.Always
        End If
        If ShowNoID Then
            RefreshData()
            GV1.ClearSelection()
            GV1.FocusedRowHandle = GV1.LocateByDisplayText(0, GV1.Columns("NoID"), (DirectNoID).ToString("#,##0"))
            GV1.SelectRow(GV1.FocusedRowHandle)
            ShowNoID = False
        End If
        If CaptionCetak1 = "" Then
            CaptionCetak1 = "Cetak Faktur"
        End If
        If CaptionCetak2 = "" Then
            CaptionCetak2 = "Cetak Panjang"
        End If
        cmdFaktur.Text = CaptionCetak1
        cmdFakturPanjang.Text = CaptionCetak2
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
            If FormName = "DaftarPenjualan" Then
                lbCustomer.Visible = True
                lbsalesman.Visible = True
                txtCustomer.Visible = True
                txtSalesman.Visible = True
                txtKassa.Visible = True
                lbKassa.Visible = True
                RefreshPendukung()
            ElseIf FormName = "DaftarPenjualanPos" Then
                lbCustomer.Visible = False
                lbsalesman.Visible = False
                txtCustomer.Visible = False
                txtSalesman.Visible = False
                txtKassa.Visible = True
                lbKassa.Visible = True
                XtraTabPage2.PageVisible = False
                XtraTabPage3.PageVisible = False
                RefreshPendukung()
            Else
                lbCustomer.Visible = False
                lbsalesman.Visible = False
                txtCustomer.Visible = False
                txtSalesman.Visible = False
                txtKassa.Visible = False
                lbKassa.Visible = False
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
        Dim sql As String
        Try
            sql = "SELECT NoID, Kode, Nama FROM MAlamat WHERE IsActive=1 AND IsCustomer=1"
            ds = ExecuteDataset("master", sql)
            txtCustomer.Properties.DataSource = ds.Tables("master")
            If System.IO.File.Exists(FolderLayouts & Me.Name & gvCustomer.Name & ".xml") Then
                gvCustomer.RestoreLayoutFromXml(FolderLayouts & Me.Name & gvCustomer.Name & ".xml")
            End If

            sql = "SELECT NoID, Kode, Nama FROM MAlamat WHERE IsActive=1 AND IsPegawai=1"
            ds = ExecuteDataset("Pegawai", sql)
            txtSalesman.Properties.DataSource = ds.Tables("Pegawai")
            If System.IO.File.Exists(FolderLayouts & Me.Name & GvSalesman.Name & ".xml") Then
                GvSalesman.RestoreLayoutFromXml(FolderLayouts & Me.Name & GvSalesman.Name & ".xml")
            End If

            sql = "SELECT NoID, Kode, Nama FROM MPos WHERE IsActive=1"
            ds = ExecuteDataset("POS", sql)
            txtKassa.Properties.DataSource = ds.Tables("POS")
            If System.IO.File.Exists(FolderLayouts & Me.Name & gvKassa.Name & ".xml") Then
                gvKassa.RestoreLayoutFromXml(FolderLayouts & Me.Name & gvKassa.Name & ".xml")
            End If
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
                If TableMaster.ToUpper = "MBeli".ToUpper AndAlso (.Columns(i).FieldName.ToUpper = "SubTotal".ToUpper Or .Columns(i).FieldName.ToUpper = "DiscNotaTotal".ToUpper Or .Columns(i).FieldName.ToUpper = "Total".ToUpper) Then
                    .Columns(i).DisplayFormat.FormatString = "n3"
                End If
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

    Private Sub SimpleButton6_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdTutup.Click
        Tutup()
    End Sub

    Private Sub SimpleButton9_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        'DialogResult = Windows.Forms.DialogResult.OK
        'Me.Close()

    End Sub

    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

    End Sub

    Private Sub SimpleButton2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdEdit.Click
        If NullToBool(Ini.BacaIni("Application", "UseFramework", False)) Then
            Edit()
        Else
            Select Case TableMaster.ToUpper
                Case "MSaldoAwalHutangPiutang".ToUpper
                    Dim view As ColumnView = GC1.FocusedView
                    Dim x As New frmEntriSaldoAwalHutangPiutang
                    Try
                        Dim row As System.Data.DataRow = view.GetDataRow(GV1.FocusedRowHandle)
                        Dim dc As Integer = GV1.FocusedRowHandle
                        Dim IDDetil As Long = NullToLong(row("NoID"))
                        x.NoID = IDDetil
                        x.pTipe = frmEntriSaldoAwalHutangPiutang.pStatus.Edit
                        x.WindowState = FormWindowState.Normal
                        x.StartPosition = FormStartPosition.CenterParent
                        If x.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
                            RefreshData()
                            GV1.ClearSelection()
                            GV1.FocusedRowHandle = GV1.LocateByDisplayText(0, GV1.Columns("NoID"), x.NoID.ToString("#,##0"))
                            GV1.SelectRow(GV1.FocusedRowHandle)
                        End If
                    Catch ex As Exception
                        XtraMessageBox.Show("Error : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Finally
                        x.Dispose()
                    End Try
                Case "MTITIPAN"
                    'Dim view As ColumnView = GC1.FocusedView
                    'Dim x As New frmEntriTitipan
                    'Try
                    '    Dim row As System.Data.DataRow = view.GetDataRow(GV1.FocusedRowHandle)
                    '    Dim dc As Integer = GV1.FocusedRowHandle
                    '    Dim IDDetil As Long = NullToLong(row("NoID"))
                    '    x.NoID = IDDetil
                    '    x.pTipe = frmEntriTitipan.pStatus.Edit
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
                    '    XtraMessageBox.Show("Untuk mengedit pilih data yang akan diedit terlebih dahulu lalu tekan tombol edit!", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Warning)
                    'End Try
                Case "MCREDITNOTE"
                    'Dim view As ColumnView = GC1.FocusedView
                    'Dim x As New frmEntriNotaKredit
                    'Try
                    '    Dim row As System.Data.DataRow = view.GetDataRow(GV1.FocusedRowHandle)
                    '    Dim dc As Integer = GV1.FocusedRowHandle
                    '    Dim IDDetil As Long = NullToLong(row("NoID"))
                    '    x.NoID = IDDetil
                    '    x.pTipe = frmEntriNotaKredit.pStatus.Edit
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
                    '    XtraMessageBox.Show("Untuk mengedit pilih data yang akan diedit terlebih dahulu lalu tekan tombol edit!", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Warning)
                    'End Try
                Case "MDEBETNOTE"
                    'Dim view As ColumnView = GC1.FocusedView
                    'Dim x As New frmEntriNotaDebet
                    'Try
                    '    Dim row As System.Data.DataRow = view.GetDataRow(GV1.FocusedRowHandle)
                    '    Dim dc As Integer = GV1.FocusedRowHandle
                    '    Dim IDDetil As Long = NullToLong(row("NoID"))
                    '    x.NoID = IDDetil
                    '    x.pTipe = frmEntriNotaDebet.pStatus.Edit
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
                    '    XtraMessageBox.Show("Untuk mengedit pilih data yang akan diedit terlebih dahulu lalu tekan tombol edit!", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Warning)
                    'End Try
                Case "MPO"
                    Dim view As ColumnView = GC1.FocusedView
                    Dim x As New frmEntriPO
                    Try
                        Dim row As System.Data.DataRow = view.GetDataRow(GV1.FocusedRowHandle)
                        Dim dc As Integer = GV1.FocusedRowHandle
                        Dim IDDetil As Long = NullToLong(row("NoID"))
                        x.NoID = IDDetil
                        x.pTipe = frmEntriPO.pStatus.Edit
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
                Case "MMUTASIGUDANG"
                    Dim view As ColumnView = GC1.FocusedView
                    Dim x As New frmEntriMutasiGudang
                    Try
                        Dim row As System.Data.DataRow = view.GetDataRow(GV1.FocusedRowHandle)
                        Dim dc As Integer = GV1.FocusedRowHandle
                        Dim IDDetil As Long = NullToLong(row("NoID"))
                        x.NoID = IDDetil
                        x.pTipe = frmEntriMutasiGudang.pStatus.Edit
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
                Case "MSO"
                    'Dim view As ColumnView = GC1.FocusedView
                    'Dim x As New frmEntriSO
                    'Try
                    '    Dim row As System.Data.DataRow = view.GetDataRow(GV1.FocusedRowHandle)
                    '    Dim dc As Integer = GV1.FocusedRowHandle
                    '    Dim IDDetil As Long = NullToLong(row("NoID"))
                    '    x.NoID = IDDetil
                    '    x.pTipe = frmEntriSO.pStatus.Edit
                    '    x.MdiParent = Me.MdiParent
                    '    x.WindowState = FormWindowState.Maximized

                    '    'For Back Action
                    '    x.FormNameDaftar = FormName
                    '    If FormName.ToLower = "DaftarSalesOrder".ToLower Then
                    '        x.IsSO = True
                    '    Else
                    '        x.IsSO = False
                    '    End If
                    '    x.TableNameDaftar = TableName
                    '    x.TextDaftar = Text
                    '    x.FormEntriDaftar = FormEntriName
                    '    x.TableMasterDaftar = TableMaster

                    '    x.Show()
                    '    x.Focus()
                    'Catch ex As Exception
                    '    XtraMessageBox.Show("Untuk mengedit pilih data yang akan diedit terlebih dahulu lalu tekan tombol edit!", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Warning)
                    'End Try
                Case "MSPK"
                    Dim view As ColumnView = GC1.FocusedView
                    Dim x As New frmEntriSPK
                    Try
                        Dim row As System.Data.DataRow = view.GetDataRow(GV1.FocusedRowHandle)
                        Dim dc As Integer = GV1.FocusedRowHandle
                        Dim IDDetil As Long = NullToLong(row("NoID"))
                        x.NoID = IDDetil
                        x.pTipe = frmEntriSPK.pStatus.Edit
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
                Case "MPacking".ToUpper
                    Dim view As ColumnView = GC1.FocusedView
                    Dim x As New frmEntriPacking
                    Try
                        Dim row As System.Data.DataRow = view.GetDataRow(GV1.FocusedRowHandle)
                        Dim dc As Integer = GV1.FocusedRowHandle
                        Dim IDDetil As Long = NullToLong(row("NoID"))
                        x.NoID = IDDetil
                        x.pTipe = frmEntriPacking.pStatus.Edit
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
                Case "MDO"
                    Dim view As ColumnView = GC1.FocusedView
                    Dim x As New frmEntriDO
                    Try
                        Dim row As System.Data.DataRow = view.GetDataRow(GV1.FocusedRowHandle)
                        Dim dc As Integer = GV1.FocusedRowHandle
                        Dim IDDetil As Long = NullToLong(row("NoID"))
                        x.NoID = IDDetil
                        x.pTipe = frmEntriDO.pStatus.Edit
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
                Case "MJUAL"
                    If FormName.ToUpper = "DaftarPenjualanPOS".ToUpper Or FormName.ToUpper = "DAFTARPENJUALAN".ToUpper Then
                        Dim view As ColumnView = GC1.FocusedView
                        Dim x As New frmEntriJualPOS
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

                            If FormName.ToUpper = "DaftarPenjualanPOS".ToUpper Then
                                x.IsPosted = True
                            Else
                                x.IsPOS = False
                            End If

                            x.Show()
                            x.Focus()
                        Catch ex As Exception
                            XtraMessageBox.Show("Untuk mengedit pilih data yang akan diedit terlebih dahulu lalu tekan tombol edit!", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Warning)
                        End Try
                    Else
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
                    End If
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
                Case "MLPB"
                    Dim view As ColumnView = GC1.FocusedView
                    Dim x As New frmEntriLPB
                    Try
                        Dim row As System.Data.DataRow = view.GetDataRow(GV1.FocusedRowHandle)
                        Dim dc As Integer = GV1.FocusedRowHandle
                        Dim IDDetil As Long = NullToLong(row("NoID"))
                        x.NoID = IDDetil
                        x.pTipe = frmEntriLPB.pStatus.Edit
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
                Case "MTransferPoin".ToUpper
                    Dim view As ColumnView = GC1.FocusedView
                    Dim x As New frmEntriTransferPoin
                    Try
                        Dim row As System.Data.DataRow = view.GetDataRow(GV1.FocusedRowHandle)
                        Dim dc As Integer = GV1.FocusedRowHandle
                        Dim IDDetil As Long = NullToLong(row("NoID"))
                        x.NoID = IDDetil
                        x.pTipe = frmEntriTransferPoin.pStatus.Edit

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
                    Dim view As ColumnView = GC1.FocusedView
                    Dim x As New frmEntriRevisiBeli
                    Try
                        Dim row As System.Data.DataRow = view.GetDataRow(GV1.FocusedRowHandle)
                        Dim dc As Integer = GV1.FocusedRowHandle
                        Dim IDDetil As Long = NullToLong(row("NoID"))
                        x.NoID = IDDetil
                        x.pTipe = frmEntriRevisiBeli.pStatus.Edit
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
                Case "MRevisiHargaJual".ToUpper
                    Dim view As ColumnView = GC1.FocusedView
                    Dim x As New frmEntriRevisiJual
                    Try
                        Dim row As System.Data.DataRow = view.GetDataRow(GV1.FocusedRowHandle)
                        Dim dc As Integer = GV1.FocusedRowHandle
                        Dim IDDetil As Long = NullToLong(row("NoID"))
                        x.NoID = IDDetil
                        x.pTipe = frmEntriRevisiJual.pStatus.Edit
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
                Case "MTT".ToUpper
                    Dim view As ColumnView = GC1.FocusedView
                    Dim x As New frmEntriTandaTerima
                    Try
                        Dim row As System.Data.DataRow = view.GetDataRow(GV1.FocusedRowHandle)
                        Dim dc As Integer = GV1.FocusedRowHandle
                        Dim IDDetil As Long = NullToLong(row("NoID"))
                        x.NoID = IDDetil
                        x.pTipe = frmEntriRevisiJual.pStatus.Edit
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

    Private Sub SimpleButton3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdHapus.Click
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

            Dim strsql As String = ""
            Dim SQL As String = ""
            Dim dsT2 As New DataSet
            If TableMaster.ToLower = "mlpb" Then
                strsql = IIf(InStr(TableName, "select", CompareMethod.Text) > 0, TableName, "select * from " & TableName)
            Else
                strsql = IIf(InStr(TableName, "select", CompareMethod.Text) > 0, TableName, "select * from " & TableName)
            End If
            JumlahWhere = Split(strsql.ToLower, "where")
            If JumlahWhere.Length >= 3 And TableMaster.ToUpper <> "MBELI" Then
                strsql = strsql & " and " & TableMaster & ".IDWilayah=" & DefIDWilayah
            ElseIf TableMaster.ToUpper = "MBELI" Then
                strsql = strsql & " WHERE " & TableMaster & ".IDWilayah=" & DefIDWilayah
            ElseIf JumlahWhere.Length >= 2 And (TableMaster.ToUpper = "MPO" Or TableMaster.ToUpper = "MSO" Or TableMaster.ToUpper = "MJUAL" Or TableMaster.ToUpper = "MTT") Then
                strsql = strsql & " and " & TableMaster & ".IDWilayah=" & DefIDWilayah
            Else
                strsql = strsql & " where " & TableMaster & ".IDWilayah=" & DefIDWilayah
            End If

            If TglDari.Enabled Then
                If InStr(strsql.ToLower, "where", CompareMethod.Text) > 0 Then
                    strsql = strsql & " and (" & TableMaster & ".Tanggal>='" & Format(TglDari.DateTime, "yyyy/MM/dd") & "' and " & TableMaster & ".Tanggal<'" & Format(DateAdd(DateInterval.Day, 1, TglSampai.DateTime), "yyyy/MM/dd") & "') "
                Else
                    strsql = strsql & " where (" & TableMaster & ".Tanggal>='" & Format(TglDari.DateTime, "yyyy/MM/dd") & "' and " & TableMaster & ".Tanggal<'" & Format(DateAdd(DateInterval.Day, 1, TglSampai.DateTime), "yyyy/MM/dd") & "') "
                End If
            End If
            If txtCustomer.Enabled Then
                If InStr(strsql.ToLower, "where", CompareMethod.Text) > 0 Then
                    strsql = strsql & " and (" & TableMaster & ".IDCustomer=" & NullTolInt(txtCustomer.EditValue) & ") "
                Else
                    strsql = strsql & " where (" & TableMaster & ".IDCustomer=" & NullTolInt(txtCustomer.EditValue) & ") "
                End If
            End If
            If txtSalesman.Enabled Then
                If InStr(strsql.ToLower, "where", CompareMethod.Text) > 0 Then
                    strsql = strsql & " and (" & TableMaster & ".IDBagPembelian=" & NullTolInt(txtSalesman.EditValue) & ") "
                Else
                    strsql = strsql & " where (" & TableMaster & ".IDBagPembelian=" & NullTolInt(txtSalesman.EditValue) & ") "
                End If
            End If
            If txtKassa.Text <> "" Then
                If InStr(strsql.ToLower, "where", CompareMethod.Text) > 0 Then
                    strsql = strsql & " and (" & TableMaster & ".IDPos=" & NullTolInt(txtKassa.EditValue) & ") "
                Else
                    strsql = strsql & " where (" & TableMaster & ".IDPos=" & NullTolInt(txtKassa.EditValue) & ") "
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
            GV1.ShowFindPanel()
            If TableMaster.ToUpper = "MSO".ToUpper Then
                mnGenerateSPK.Visibility = DevExpress.XtraBars.BarItemVisibility.Always
                mnPacking.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
                mnSetPOSelesai.Visibility = DevExpress.XtraBars.BarItemVisibility.Always
                mnSetPOBelumSelesai.Visibility = DevExpress.XtraBars.BarItemVisibility.Always
                mnHasilPosting.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
                mnExportExcel.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
                If FormName.ToLower = "daftarpermintaanbarang" Then
                    mnSetPOBelumSelesai.Caption = "Set Permintaan Belum Selesai"
                    mnSetPOSelesai.Caption = "Set Permintaan Selesai"
                Else
                    mnSetPOBelumSelesai.Caption = "Set SO Belum Selesai"
                    mnSetPOSelesai.Caption = "Set SO Selesai"
                End If
            ElseIf TableMaster.ToUpper = "MPO".ToUpper Then
                mnGenerateSPK.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
                mnSetPOSelesai.Visibility = DevExpress.XtraBars.BarItemVisibility.Always
                mnSetPOBelumSelesai.Visibility = DevExpress.XtraBars.BarItemVisibility.Always
                mnPacking.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
                mnHasilPosting.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
                mnExportExcel.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
            ElseIf TableMaster.ToUpper = "MSPK".ToUpper Then
                mnSetPOSelesai.Visibility = DevExpress.XtraBars.BarItemVisibility.Always
                mnSetPOBelumSelesai.Visibility = DevExpress.XtraBars.BarItemVisibility.Always
                mnSetPOBelumSelesai.Caption = "Set SPK Belum Selesai"
                mnSetPOSelesai.Caption = "Set SPK Selesai"
                mnGenerateSPK.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
                mnPacking.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
                mnHasilPosting.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
                mnExportExcel.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
            ElseIf TableMaster.ToUpper = "MPacking".ToUpper Then
                mnSetPOSelesai.Visibility = DevExpress.XtraBars.BarItemVisibility.Always
                mnSetPOBelumSelesai.Visibility = DevExpress.XtraBars.BarItemVisibility.Always
                mnSetPOBelumSelesai.Caption = "Set Packing Belum Selesai"
                mnSetPOSelesai.Caption = "Set Packing Selesai"
                mnGenerateSPK.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
                mnPacking.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
                mnHasilPosting.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
                mnExportExcel.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
            ElseIf TableMaster.ToUpper = "MBeli".ToUpper Then
                mnSetPOSelesai.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
                mnSetPOBelumSelesai.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
                mnGenerateSPK.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
                mnPacking.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
                mnHasilPosting.Visibility = DevExpress.XtraBars.BarItemVisibility.Always
                mnExportExcel.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
                If IsAccMutasi Then
                    mnRevisi.Visibility = DevExpress.XtraBars.BarItemVisibility.Always
                Else
                    mnRevisi.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
                End If
            ElseIf TableMaster.ToUpper = "MJual".ToUpper Then
                mnSetPOSelesai.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
                mnSetPOBelumSelesai.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
                mnGenerateSPK.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
                mnPacking.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
                mnHasilPosting.Visibility = DevExpress.XtraBars.BarItemVisibility.Always
                mnExportExcel.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
                mnCetakFPKosong.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
                mnCetakFPBlanko.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
                If FormName.ToUpper = "DaftarPenjualan".ToUpper Then
                    'ckAutoPosting.Visible = True

                ElseIf FormName.ToUpper = "DAFTARPAJAKKELUARAN".ToUpper Then
                    cmdFaktur.Text = "Format Blanko"
                    cmdFakturPanjang.Text = "Format Kosong"

                    cmdFaktur.Visible = False
                    cmdFakturPanjang.Visible = False

                    mnFPObat.Visibility = DevExpress.XtraBars.BarItemVisibility.Always
                    mnFPPromosi.Visibility = DevExpress.XtraBars.BarItemVisibility.Always
                End If
            ElseIf TableMaster.ToUpper = "MReturBeli".ToUpper Then
                mnSetPOSelesai.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
                mnSetPOBelumSelesai.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
                mnGenerateSPK.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
                mnPacking.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
                mnHasilPosting.Visibility = DevExpress.XtraBars.BarItemVisibility.Always
                mnExportExcel.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
                If IsAccMutasi Then
                    mnRevisi.Visibility = DevExpress.XtraBars.BarItemVisibility.Always
                    mnEntriFakturPajakRetur.Visibility = DevExpress.XtraBars.BarItemVisibility.Always
                Else
                    mnRevisi.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
                    mnEntriFakturPajakRetur.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
                End If
                mnCetakFPKosong.Visibility = DevExpress.XtraBars.BarItemVisibility.Always
                mnCetakFPBlanko.Visibility = DevExpress.XtraBars.BarItemVisibility.Always
            ElseIf TableMaster.ToUpper = "MReturJual".ToUpper Then
                mnSetPOSelesai.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
                mnSetPOBelumSelesai.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
                mnGenerateSPK.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
                mnPacking.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
                mnHasilPosting.Visibility = DevExpress.XtraBars.BarItemVisibility.Always
                mnExportExcel.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
            ElseIf TableMaster.ToUpper = "MMutasiGudang".ToUpper Then
                mnSetPOSelesai.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
                mnSetPOBelumSelesai.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
                mnGenerateSPK.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
                mnPacking.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
                mnHasilPosting.Visibility = DevExpress.XtraBars.BarItemVisibility.Always
                mnExportExcel.Visibility = DevExpress.XtraBars.BarItemVisibility.Always
            ElseIf TableMaster.ToUpper = "MRevisiHargaBeli".ToUpper Then
                mnSetPOBelumSelesai.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
                mnSetPOSelesai.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
                mnGenerateSPK.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
                mnPacking.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
                mnHasilPosting.Visibility = DevExpress.XtraBars.BarItemVisibility.Always
                mnExportExcel.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
            Else
                mnSetPOBelumSelesai.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
                mnSetPOSelesai.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
                mnGenerateSPK.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
                mnPacking.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
                mnHasilPosting.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
                mnExportExcel.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
            End If

            If TableMaster.ToUpper = "MBeli".ToUpper Then
                '    SQL = "SELECT MPOD.NoID, MPO.Kode AS NoPO, MGudang.Nama AS NamaGudang, MBarang.Alias AS KodeAlias, MBarang.NamaAlias, MWilayah.Nama AS NamaWilayah, MBarang.Kode AS KodeBarang, MAlamat.Kode AS KodeSupplier, MAlamat.Nama AS NamaSupplier, MBarang.Nama AS NamaBarang, "
                '    SQL &= " (MPOD.Harga-isnull(MPOD.DISC1,0)-isnull(MPOD.DISC2,0)-isnull(MPOD.DISC3,0)"
                '    SQL &= " -(MPOD.Harga*isnull(MPOD.DiscPersen1,0)/100)"
                '    SQL &= " -(MPOD.Harga*isnull(MPOD.DiscPersen2,0)/100)"
                '    SQL &= " -(MPOD.Harga*isnull(MPOD.DiscPersen3,0)/100)"
                'SQL &= " ) AS Harga , MPOD.Qty, MSatuan.Nama AS Satuan, MPOD.HargaPcs AS [Harga(Pcs)], MPOD.Qty*MPOD.Konversi AS [Qty(Pcs)], MPOD.Qty*MPOD.Konversi- " & PODiBeli() & "  AS [Sisa(Pcs)]"
                '    SQL &= " FROM MPOD LEFT JOIN MSatuan ON MSatuan.NoID=MPOD.IDSatuan "
                '    SQL &= " LEFT JOIN (MPO LEFT JOIN MAlamat ON MAlamat.NoID=MPO.IDSupplier) ON MPO.NoID=MPOD.IDPO "
                '    SQL &= " LEFT JOIN MBarang ON MBarang.NoID=MPOD.IDBarang "
                '    SQL &= " LEFT JOIN MGudang ON MGudang.NoID=MPOD.IDGudang "
                '    SQL &= " LEFT JOIN MWilayah ON MWilayah.NoID=MPOD.IDWilayah "
                'SQL &= " WHERE (MPO.IsSelesai=0 OR MPO.IsSelesai IS NULL) AND MPO.IDWilayah=" & DefIDWilayah & " AND (MPOD.Qty*MPOD.Konversi - " & PODiBeli() & ">0) AND MBarang.IsActive = 1 And MPO.IsPosted = 1 "
                '    dsT2 = ExecuteDataset("MPOD", SQL)
                '    GridControl1.DataSource = dsT2.Tables("MPOD")
                '    XtraTabPage2.Text = "PO Yang Menggantung"

                'SQL = "SELECT MLPBD.NoID, MGudang.Nama AS NamaGudang, MWilayah.Nama AS NamaWilayah, MBarang.Kode AS KodeBarang, MBarang.Nama AS NamaBarang, MLPB.Kode AS NoLPB, MLPB.Tanggal, MAlamat.Nama AS Supplier, MBagPembelian.Nama AS Penerima, MLPBD.Qty * MLPBD.Konversi AS [QtyPcs], MLPBD.Qty*MLPBD.Konversi-" & STBDiBeli() & "  AS [Sisa(Pcs)] "
                '    SQL &= " FROM (MLPBD LEFT OUTER JOIN (MLPB LEFT OUTER JOIN MAlamat AS MBagPembelian ON MBagPembelian.NoID = MLPB.IDBagPembelian "
                '    SQL &= " LEFT OUTER JOIN MAlamat ON MLPB.IDSupplier = MAlamat.NoID)"
                '    SQL &= " ON MLPB.NoID=MLPBD.IDLPB) "
                '    SQL &= " LEFT OUTER JOIN MBarang ON MLPBD.IDBarang = MBarang.NoID "
                '    SQL &= " LEFT OUTER JOIN (MGudang LEFT OUTER JOIN MWilayah ON MWilayah.NoID=MGudang.IDWilayah) ON MGudang.NoID=MLPBD.IDGudang "
                'SQL &= " WHERE MBarang.ISActive=1 AND (MLPBD.Qty*MLPBD.Konversi-" & STBDiBeli() & ">0) AND MLPB.IDWilayah=" & DefIDWilayah & " And MLPB.IsPosted = 1 "

                '    dsT2 = ExecuteDataset("MLPBD", SQL)
                '    GridControl2.DataSource = dsT2.Tables("MLPBD")
                '    XtraTabPage3.Text = "Surat Terima Barang Yang Menggantung"

                XtraTabPage2.PageVisible = False
                XtraTabPage3.PageVisible = False
            ElseIf TableMaster.ToUpper = "MSPK".ToUpper Then
                'SQL = "SELECT MSOD.NoID, MSO.Tanggal AS TglSO, MSO.Kode AS NoSO, MGudang.Nama AS NamaGudang, MWilayah.Nama AS NamaWilayah, MBarang.Kode AS KodeBarang, MAlamat.Kode AS KodeCustomer, MAlamat.Nama AS NamaCustomer, MBarang.Nama AS NamaBarang, " & _
                '      " (MSOD.Harga-isnull(MSOD.DISC1,0)-isnull(MSOD.DISC2,0)-isnull(MSOD.DISC3,0)" & _
                '      " -(MSOD.Harga*isnull(MSOD.DiscPersen1,0)/100)" & _
                '      " -(MSOD.Harga*isnull(MSOD.DiscPersen2,0)/100)" & _
                '      " -(MSOD.Harga*isnull(MSOD.DiscPersen3,0)/100)" & _
                '      " ) AS Harga , MSOD.Qty, MSatuan.Nama AS Satuan, MSOD.HargaPcs AS [Harga(Pcs)], MSOD.Qty*MSOD.Konversi AS [Qty(Pcs)], MSOD.Qty*MSOD.Konversi- " & SODiSPK() & "  AS [Sisa(Pcs)]" & _
                '      " FROM MSOD LEFT JOIN MSatuan ON MSatuan.NoID=MSOD.IDSatuan " & _
                '      " LEFT JOIN (MSO LEFT JOIN MAlamat ON MAlamat.NoID=MSO.IDCustomer) ON MSO.NoID=MSOD.IDSO " & _
                '      " LEFT JOIN MBarang ON MBarang.NoID=MSOD.IDBarang " & _
                '      " LEFT JOIN MGudang ON MGudang.NoID=MSOD.IDGudang " & _
                '      " LEFT JOIN MWilayah ON MWilayah.NoID=MSO.IDWilayah " & _
                '      " WHERE (MSO.IsSelesai=0 OR MSO.IsSelesai IS NULL) AND MSO.IDWilayah=" & DefIDWilayah & " AND (MSOD.Qty*MSOD.Konversi - " & SODiSPK() & ">0) AND MBarang.IsActive = 1 And MSO.IsPosted = 1 "
                'dsT2 = ExecuteDataset("MPOD", SQL)
                'GridControl1.DataSource = dsT2.Tables("MPOD")
                'XtraTabPage2.Text = "SO Yang Menggantung"

                XtraTabPage2.PageVisible = True
                XtraTabPage3.PageVisible = False
            ElseIf TableMaster.ToUpper = "MPacking".ToUpper Then
                'SQL = "SELECT MSPKD.NoID, MSPK.Kode AS NoSPK, MSPK.Tanggal AS TglSPK, MGudang.Nama AS NamaGudang, MWilayah.Nama AS NamaWilayah, MBarang.Kode AS KodeBarang, MAlamat.Kode AS KodeCustomer, MAlamat.Nama AS NamaCustomer, MBarang.Nama AS NamaBarang, " & _
                '      " (MSPKD.Harga-isnull(MSPKD.DISC1,0)-isnull(MSPKD.DISC2,0)-isnull(MSPKD.DISC3,0)" & _
                '      " -(MSPKD.Harga*isnull(MSPKD.DiscPersen1,0)/100)" & _
                '      " -(MSPKD.Harga*isnull(MSPKD.DiscPersen2,0)/100)" & _
                '      " -(MSPKD.Harga*isnull(MSPKD.DiscPersen3,0)/100)" & _
                '      " ) AS Harga , MSPKD.Qty, MSatuan.Nama AS Satuan, MSPKD.HargaPcs AS [Harga(Pcs)], MSPKD.Qty*MSPKD.Konversi AS [Qty(Pcs)], MSPKD.Qty*MSPKD.Konversi- " & SPKDiPacking() & "  AS [Sisa(Pcs)]" & _
                '      " FROM MSPKD LEFT JOIN MSatuan ON MSatuan.NoID=MSPKD.IDSatuan " & _
                '      " LEFT JOIN (MSPK LEFT JOIN MAlamat ON MAlamat.NoID=MSPK.IDCustomer) ON MSPK.NoID=MSPKD.IDSPK " & _
                '      " LEFT JOIN MBarang ON MBarang.NoID=MSPKD.IDBarang " & _
                '      " LEFT JOIN (MGudang LEFT JOIN MWilayah ON MWilayah.NoID=MGudang.IDWilayah) ON MGudang.NoID=MSPKD.IDGudang " & _
                '      " WHERE (MSPK.IsSelesai=0 OR MSPK.IsSelesai IS NULL) AND MSPK.IDWilayah=" & DefIDWilayah & " AND (MSPKD.Qty*MSPKD.Konversi - " & SPKDiPacking() & ">0) AND MBarang.IsActive = 1 And MSPK.IsPosted = 1 "
                'dsT2 = ExecuteDataset("MPOD", SQL)
                'GridControl1.DataSource = dsT2.Tables("MPOD")
                'XtraTabPage2.Text = "SPK Yang Menggantung"

                XtraTabPage2.PageVisible = True
                XtraTabPage3.PageVisible = False
            ElseIf TableMaster.ToUpper = "MJual".ToUpper Then
                'dsT2 = ExecuteDataset("MJualPOS", Replace(strsql, "IsNull(" & TableMaster & ".IsPOS,0)=0", "IsNull(" & TableMaster & ".IsPOS,0)=1"))
                'GridControl2.DataSource = dsT2.Tables("MJualPOS")
                'XtraTabPage3.Text = "Daftar Penjualan POS"

                If FormName.ToUpper = "DaftarPenjualanPOS".ToUpper Then
                    lbKassa.Enabled = True
                    txtKassa.Enabled = True

                    SQL = strsql & " AND IsNull(MJual.IDCustomer,0)>=1"
                    GridControl1.DataSource = ExecuteDataset("MJualMember", SQL).Tables("MJualMember")
                    XtraTabPage2.PageVisible = True
                    XtraTabPage2.Text = "Penjualan Member"
                ElseIf FormName.ToUpper = "DaftarPenjualan".ToUpper Then
                    lbKassa.Visible = True
                    txtKassa.Visible = True

                    XtraTabPage2.PageVisible = False
                    XtraTabPage3.PageVisible = False
                Else
                    lbKassa.Enabled = False
                    txtKassa.Enabled = False
                    lbKassa.Visible = False
                    txtKassa.Visible = False
                    'SQL = "SELECT MPacking.Tanggal, MPackingD.NoID, MPacking.Kode AS KodePacking, MPackingD.NoPacking, MGudang.Nama AS Gudang, MWilayah.Nama AS Wilayah, MBarang.Kode AS KodeBarang, MAlamat.Kode AS KodeCustomer, MAlamat.Nama AS NamaCustomer, MBarang.Nama AS NamaBarang, " & _
                    '      " (MPackingD.Harga-isnull(MPackingD.DISC1,0)-isnull(MPackingD.DISC2,0)-isnull(MPackingD.DISC3,0)" & _
                    '      " -(MPackingD.Harga*isnull(MPackingD.DiscPersen1,0)/100)" & _
                    '      " -(MPackingD.Harga*isnull(MPackingD.DiscPersen2,0)/100)" & _
                    '      " -(MPackingD.Harga*isnull(MPackingD.DiscPersen3,0)/100)" & _
                    '      " ) AS Harga , MPackingD.Qty, MSatuan.Nama AS Satuan, MPackingD.HargaPcs AS [Harga(Pcs)], MPackingD.Qty*MPackingD.Konversi AS [Qty(Pcs)], (MPackingD.Qty*MPackingD.Konversi)- " & PackingDiJual() & "  AS [Sisa(Pcs)]" & _
                    '      " FROM MPackingD LEFT JOIN MSatuan ON MSatuan.NoID=MPackingD.IDSatuan " & _
                    '      " LEFT JOIN (MPacking LEFT JOIN MAlamat ON MAlamat.NoID=MPacking.IDCustomer) ON MPacking.NoID=MPackingD.IDPacking " & _
                    '      " LEFT JOIN MBarang ON MBarang.NoID=MPackingD.IDBarang " & _
                    '      " LEFT JOIN (MGudang LEFT JOIN MWilayah ON MWilayah.NoID=MGudang.IDWilayah) ON MGudang.NoID=MPackingD.IDGudang " & _
                    '      " WHERE (MPacking.IsSelesai=0 OR MPacking.IsSelesai IS NULL) AND MPacking.IDWilayah=" & DefIDWilayah & " AND ((MPackingD.Qty*MPackingD.Konversi) - " & PackingDiJual() & ">0) AND MBarang.IsActive = 1 And MPacking.IsPosted = 1 "
                    ''SQL = "SELECT MPacking.Kode AS KodePacking, MPacking.Tanggal, MGudang.Nama AS Gudang, MWilayah.Nama AS Wilayah, MAlamat.Kode AS KodeCustomer, MAlamat.Nama AS NamaCustomer, " & _
                    ''      " MBarang.Kode AS KodeStock, MBarang.Nama AS NamaStock, SUM(MPackingD.Qty*MPackingD.Konversi) AS QtyPcs," & _
                    ''      " SUM(MPackingD.Qty*MPackingD.Konversi)-IsNull((SELECT SUM(MJualD.Qty*MJualD.Konversi) FROM MJualD INNER JOIN MJual ON MJual.NoID=MJualD.IDJual WHERE MJual.IDPacking=MPacking.NoID AND MJualD.IDBarang=MPackingD.IDBarang),0) AS SisaQty " & _
                    ''      " FROM MPackingD INNER JOIN MPacking ON MPacking.NoID=MPackingD.IDPacking " & _
                    ''      " LEFT JOIN MAlamat ON MAlamat.NoID=MPacking.IDCustomer " & _
                    ''      " LEFT JOIN (MGudang LEFT JOIN MWilayah ON MWilayah.NoID=MGudang.IDWilayah) ON MGudang.NoID=MPacking.IDGudang " & _
                    ''      " LEFT JOIN MBarang ON MBarang.NoID=MPackingD.IDBarang " & _
                    ''      " WHERE MPacking.IDWilayah = " & DefIDWilayah & " AND MBarang.IsActive=1 AND IsNull(MPacking.IsPosted, 0) <> 0 And IsNull(MPacking.IsSelesai, 0) = 0 " & _
                    ''      " GROUP BY MPacking.NoID, MPacking.IsPosted, MBarang.IsActive, MPacking.IsSelesai, MPackingD.IDBarang, MPacking.Kode, MPacking.Tanggal, MGudang.Nama, MWilayah.Nama, MAlamat.Kode, MAlamat.Nama, MBarang.Kode, MBarang.Nama, MPacking.IDWilayah " & _
                    ''      " HAVING SUM(MPackingD.Qty*MPackingD.Konversi)-IsNull((SELECT SUM(MJualD.Qty*MJualD.Konversi) FROM MJualD INNER JOIN MJual ON MJual.NoID=MJualD.IDJual WHERE MJual.IDPacking=MPacking.NoID AND MJualD.IDBarang=MPackingD.IDBarang),0)>0"

                    'dsT2 = ExecuteDataset("MPOD", SQL)
                    'GridControl1.DataSource = dsT2.Tables("MPOD")
                    'XtraTabPage2.Text = "Packing Yang Menggantung Di Penjualan"
                    'XtraTabPage2.PageVisible = True
                    XtraTabPage2.PageVisible = False
                    XtraTabPage3.PageVisible = False
                End If
                XtraTabPage3.PageVisible = False
            ElseIf TableMaster.ToUpper = "MDO".ToUpper Then
                'SQL = "SELECT MJualD.NoID, MJual.Kode AS NoFaktur, MJual.Tanggal AS Tanggal, MJualD.NoUrut, MGudang.Nama AS Gudang, MWilayah.Nama AS Wilayah, MBarang.Kode AS KodeBarang, MAlamat.Kode AS KodeCustomer, MAlamat.Nama AS NamaCustomer, MBarang.Nama AS NamaBarang, " & _
                '      " MJualD.Qty, MSatuan.Nama AS Satuan, MJualD.HargaPcs AS [Harga(Pcs)], MJualD.Qty*MJualD.Konversi AS [Qty(Pcs)], (MJualD.Qty*MJualD.Konversi)- " & JualDiSKB() & "  AS [Sisa(Pcs)]" & _
                '      " FROM MJualD LEFT JOIN MSatuan ON MSatuan.NoID=MJualD.IDSatuan " & _
                '      " LEFT JOIN (MJual LEFT JOIN MAlamat ON MAlamat.NoID=MJual.IDCustomer) ON MJual.NoID=MJualD.IDJual " & _
                '      " LEFT JOIN MBarang ON MBarang.NoID=MJualD.IDBarang " & _
                '      " LEFT JOIN (MGudang LEFT JOIN MWilayah ON MWilayah.NoID=MGudang.IDWilayah) ON MGudang.NoID=MJualD.IDGudang " & _
                '      " WHERE MJual.Tanggal>='2011/10/30' AND IsNull(MJual.IsPOS,0)=0 AND MJual.IDWilayah=" & DefIDWilayah & " AND (MJualD.Qty*MJualD.Konversi - " & JualDiSKB() & ">0) AND MBarang.IsActive = 1 And MJual.IsPosted = 1 "
                'dsT2 = ExecuteDataset("MPOD", SQL)
                'GridControl1.DataSource = dsT2.Tables("MPOD")
                'XtraTabPage2.Text = "Penjualan Yang Menggantung Di Surat Jalan"

                XtraTabPage2.PageVisible = True
                XtraTabPage3.PageVisible = False
            Else
                XtraTabPage2.PageVisible = False
                XtraTabPage3.PageVisible = False
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
                Case "MDEBETNOTE", "MCREDITNOTE", "MTITIPAN", "MTRANSFERPOIN", "MSALDOAWALHUTANGPIUTANG"
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
                                            ''12-09-2015 Poin
                                            'EksekusiSQL("DELETE FROM MKartuPoinMember where IDJenisTransaksi=6 AND ID" & TableMaster.Substring(1) & "= " & NoID.ToString)

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

    Private Sub BarButtonItem1_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem1.ItemClick
        cmdBaru.PerformClick()
    End Sub

    Private Sub BarButtonItem2_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem2.ItemClick
        cmdEdit.PerformClick()
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
        cmdPreview.PerformClick()
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
            If TableMaster.ToUpper = "MReturBeli".ToUpper Then
                CetakCrystalReport(Me.ParentForm, Me.Text, Application.StartupPath & "\Report\Rekap" & TableMaster & ".rpt", "{" & TableMaster & ".Tanggal}>={@DariTanggal} AND {" & TableMaster & ".Tanggal}<={@SampaiTanggal}", "DariTanggal=CDate(" & TglDari.DateTime.ToString("yyyy,MM,dd") & ")&SampaiTanggal=CDATE(" & TglSampai.DateTime.ToString("yyyy,MM,dd") & ")&UserPrint='" & NamaUserAktif & "'")
            Else
                GC1.ShowPrintPreview()
            End If
        ElseIf XtraTabControl1.SelectedTabPageIndex = 1 Then
            GridControl1.ShowPrintPreview()
        Else
            GridControl2.ShowPrintPreview()
        End If
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
    End Sub

    Private Sub cmdFaktur_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdFaktur.Click
        mnFaktur.PerformClick()
    End Sub
    Private Sub CetakRekapPenjualanMember(ByVal action As action_)
        Dim namafile As String
        Dim view As ColumnView = GC1.FocusedView
        Dim ds As New DataSet
        Try
            namafile = Application.StartupPath & "\report\RekapPenjualanMemberHarian.rpt"
            If System.IO.File.Exists(namafile) Then
                Dim row As System.Data.DataRow = view.GetDataRow(GV1.FocusedRowHandle)
                Dim dc As Integer = GV1.FocusedRowHandle
                If row Is Nothing Then XtraMessageBox.Show("Untuk mencetak, pilih item yang mau dicetak dan click cetak faktur.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information) : Exit Sub
                Dim NoID As Long = NullToLong(row("NoID"))
                If EditReport Then
                    action = action_.Edit
                Else
                    action = action_.Preview
                End If
                ViewReport(Me.ParentForm, action, namafile, Me.Text, , , "TglDari=CDATE(" & TglDari.DateTime.ToString("yyyy,MM,dd") & ")&TglSampai=CDATE(" & TglSampai.DateTime.ToString("yyyy,MM,dd") & ")&IDCustomer=" & NullToLong(txtCustomer.EditValue) & "&IDKassa=" & NullToLong(txtKassa.EditValue))
            Else
                DevExpress.XtraEditors.XtraMessageBox.Show("Nama File :" & namafile & " tidak ditemukan.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Stop)
            End If
        Catch EX As Exception
            XtraMessageBox.Show("Untuk mencetak, pilih item yang mau dicetak dan click cetak faktur.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
        Finally
            ds.Dispose()
        End Try
    End Sub
    Private Sub CetakDetilPenjualanMember(ByVal action As action_)
        Dim namafile As String
        Dim view As ColumnView = GC1.FocusedView
        Dim ds As New DataSet
        Try
            namafile = Application.StartupPath & "\report\DetilPenjualanMemberHarian.Rpt"
            If System.IO.File.Exists(namafile) Then
                Dim row As System.Data.DataRow = view.GetDataRow(GV1.FocusedRowHandle)
                Dim dc As Integer = GV1.FocusedRowHandle
                If row Is Nothing Then XtraMessageBox.Show("Untuk mencetak, pilih item yang mau dicetak dan click cetak faktur.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information) : Exit Sub
                Dim NoID As Long = NullToLong(row("NoID"))
                If EditReport Then
                    action = action_.Edit
                Else
                    action = action_.Preview
                End If
                ViewReport(Me.ParentForm, action, namafile, Me.Text, , , "TglDari=CDATE(" & TglDari.DateTime.ToString("yyyy,MM,dd") & ")&TglSampai=CDATE(" & TglSampai.DateTime.ToString("yyyy,MM,dd") & ")&IDCustomer=" & NullToLong(txtCustomer.EditValue) & "&IDKassa=" & NullToLong(txtKassa.EditValue))
            Else
                DevExpress.XtraEditors.XtraMessageBox.Show("Nama File :" & namafile & " tidak ditemukan.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Stop)
            End If
        Catch EX As Exception
            XtraMessageBox.Show("Untuk mencetak, pilih item yang mau dicetak dan click cetak faktur.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
        Finally
            ds.Dispose()
        End Try
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
    Private Sub CetakFakturPajakObat(ByVal action As action_, ByVal Tipe As Integer)
        Dim namafile As String
        Dim view As ColumnView = GC1.FocusedView
        Dim ds As New DataSet
        Try
            namafile = Application.StartupPath & "\report\FakturPajak" & TableMaster & ".rpt"
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
                'If Not EditReport Then
                '    mnPosting.PerformClick()
                'End If
                If TableMaster.ToLower = "MSPK".ToLower Then
                    ds = ExecuteDataset("MSPK", "SELECT IDGudang FROM MSPKD WHERE IDSPK=" & NoID & " GROUP BY IDGudang")
                    For i As Integer = 0 To ds.Tables("MSPK").Rows.Count - 1
                        ViewReport(Me.ParentForm, action, namafile, Me.Text, "{" & TableMaster & ".NoID}=" & NoID & " AND {" & TableMaster & "D.IDGudang}=" & NullToLong(ds.Tables("MSPK").Rows(i).Item("IDGudang")))
                    Next
                Else
                    ViewReport(Me.ParentForm, action, namafile, Me.Text, "{" & TableMaster & ".NoID}=" & NoID, , "Tipe=" & Tipe)
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
    Private Sub CetakFakturPajakPromosi(ByVal action As action_, ByVal Tipe As Integer)
        Dim namafile As String
        Dim view As ColumnView = GC1.FocusedView
        Dim ds As New DataSet
        Try
            namafile = Application.StartupPath & "\report\FakturPajak" & TableMaster & ".rpt"
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
                'If Not EditReport Then
                '    mnPosting.PerformClick()
                'End If
                If TableMaster.ToLower = "MSPK".ToLower Then
                    ds = ExecuteDataset("MSPK", "SELECT IDGudang FROM MSPKD WHERE IDSPK=" & NoID & " GROUP BY IDGudang")
                    For i As Integer = 0 To ds.Tables("MSPK").Rows.Count - 1
                        ViewReport(Me.ParentForm, action, namafile, Me.Text, "{" & TableMaster & ".NoID}=" & NoID & " AND {" & TableMaster & "D.IDGudang}=" & NullToLong(ds.Tables("MSPK").Rows(i).Item("IDGudang")))
                    Next
                Else
                    ViewReport(Me.ParentForm, action, namafile, Me.Text, "{" & TableMaster & ".NoID}=" & NoID, , "Tipe=" & Tipe)
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
        Try
            SQL = "SELECT * FROM " & TableMaster & " WHERE NoID=" & NoID
            ds = ExecuteDataset(TableMaster, SQL)
            If NullToBool(ds.Tables(TableMaster).Rows(0).Item("IsEdit")) Then
                XtraMessageBox.Show("Transaksi ini masih diedit oleh " & NullToStr(ds.Tables(TableMaster).Rows(0).Item("UserEdit")), NamaAplikasi, MessageBoxButtons.OK)
                Return True
            Else
                Return False
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
    Private Sub CetakFakturPajakBlankoObat(ByVal action As action_, ByVal Tipe As Integer)
        Dim namafile As String
        Dim view As ColumnView = GC1.FocusedView
        Dim ds As New DataSet
        Try
            namafile = Application.StartupPath & "\report\FakturPajakBlanko" & TableMaster & ".rpt"
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
                'If Not EditReport Then
                '    mnPosting.PerformClick()
                'End If
                If TableMaster.ToLower = "MSPK".ToLower Then
                    ds = ExecuteDataset("MSPK", "SELECT IDGudang FROM MSPKD WHERE IDSPK=" & NoID & " GROUP BY IDGudang")
                    For i As Integer = 0 To ds.Tables("MSPK").Rows.Count - 1
                        ViewReport(Me.ParentForm, action, namafile, Me.Text, "{" & TableMaster & ".NoID}=" & NoID & " AND {" & TableMaster & "D.IDGudang}=" & NullToLong(ds.Tables("MSPK").Rows(i).Item("IDGudang")))
                    Next
                Else
                    ViewReport(Me.ParentForm, action, namafile, Me.Text, "{" & TableMaster & ".NoID}=" & NoID, , "Tipe=" & Tipe)
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
    Private Sub CetakFakturPajakBlankoPromosi(ByVal action As action_, ByVal Tipe As Integer)
        Dim namafile As String
        Dim view As ColumnView = GC1.FocusedView
        Dim ds As New DataSet
        Try
            namafile = Application.StartupPath & "\report\FakturPajakBlankoPromosi" & TableMaster & ".rpt"
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
                'If Not EditReport Then
                '    mnPosting.PerformClick()
                'End If
                If TableMaster.ToLower = "MSPK".ToLower Then
                    ds = ExecuteDataset("MSPK", "SELECT IDGudang FROM MSPKD WHERE IDSPK=" & NoID & " GROUP BY IDGudang")
                    For i As Integer = 0 To ds.Tables("MSPK").Rows.Count - 1
                        ViewReport(Me.ParentForm, action, namafile, Me.Text, "{" & TableMaster & ".NoID}=" & NoID & " AND {" & TableMaster & "D.IDGudang}=" & NullToLong(ds.Tables("MSPK").Rows(i).Item("IDGudang")))
                    Next
                Else
                    ViewReport(Me.ParentForm, action, namafile, Me.Text, "{" & TableMaster & ".NoID}=" & NoID, , "Tipe=" & Tipe)
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
        If FormName.ToUpper = "DAFTARPAJAKKELUARAN" AndAlso TableMaster.ToUpper = "MJual".ToUpper Then
            'If frmMain.ckEditRpt.Checked Then
            '    CetakFakturPajakBlanko(action_.Edit)
            'ElseIf frmMain.ckLangsungCetak.Checked Then
            '    CetakFakturPajakBlanko(action_.Print)
            'Else
            '    CetakFakturPajakBlanko(action_.Preview)
            'End If
        ElseIf FormName.ToUpper = "DAFTARPENJUALAN" AndAlso TableMaster.ToUpper = "MJual".ToUpper Then
            If frmMain.ckEditRpt.Checked Then
                CetakRekapPenjualanMember(action_.Edit)
            ElseIf frmMain.ckLangsungCetak.Checked Then
                CetakRekapPenjualanMember(action_.Print)
            Else
                CetakRekapPenjualanMember(action_.Preview)
            End If
        Else
            If frmMain.ckEditRpt.Checked Then
                CetakFaktur(action_.Edit)
            ElseIf frmMain.ckLangsungCetak.Checked Then
                CetakFaktur(action_.Print)
            Else
                CetakFaktur(action_.Preview)
            End If
        End If
    End Sub

    Private Sub mnPosting_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles mnPosting.ItemClick
        Dim dlg As New WaitDialogForm("Sedang Proses Posting...", "Mohon Tunggu Sebentar.")
        Dim NoID As Long = -1
        Try
            Enabled = False
            dlg.Owner = Me.MdiParent
            dlg.TopMost = True
            dlg.Show()
            Dim jumItem As Integer = GV1.SelectedRowsCount
            For Each i In GV1.GetSelectedRows
                NoID = NullToLong(GV1.GetDataRow(i).Item("NoID"))
                If CekMasihDiedit(NoID) Then Exit For
                Select Case TableMaster.ToUpper
                    Case "MSaldoAwalHutangPiutang".ToUpper
                        PostingSaldoAwalHutangPiutang(NoID)
                    Case "MTransferPoin".ToUpper
                        PostingTransferPoin(NoID)
                    Case "MLPB".ToUpper
                        PostingLPB(NoID)
                    Case "MTT".ToUpper
                        PostingTandaTerimaSupplier(NoID)
                    Case "MPO".ToUpper
                        PostingPO(NoID)
                    Case "MSO".ToUpper
                        PostingSO(NoID)
                    Case "MSPK".ToUpper
                        PostingSPK(NoID)
                    Case "MPacking".ToUpper
                        PostingPacking(NoID)
                    Case "MDO".ToUpper
                        PostingDO(NoID)
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
                    Case "MREVISIHARGABELI".ToUpper
                        PostingStokBarangRevisiHargaPembelian(NoID)
                    Case "MREVISIHARGAJUAL".ToUpper
                        PostingStokBarangRevisiHargaPenjualan(NoID)
                    Case "MCREDITNOTE".ToUpper
                        PostingNotaKredit(NoID)
                    Case "MDEBETNOTE".ToUpper
                        PostingNotaDebet(NoID)
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
            MdiParent.Focus()
            Focus()
        End Try
    End Sub

    Private Sub mnUnPosting_ItemClick(ByVal sender As Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles mnUnPosting.ItemClick
        Dim dlg As New WaitDialogForm("Proses UnPosting diproses...", "Mohon Tunggu Sebentar.")
        Dim NoID As Long = -1
        Try
            Enabled = False
            dlg.TopMost = False
            dlg.Owner = Me.MdiParent
            dlg.Show()
            Dim jumItem As Integer = GV1.SelectedRowsCount
            For Each i In GV1.GetSelectedRows
                NoID = NullToLong(GV1.GetDataRow(i).Item("NoID"))
                Select Case TableMaster.ToUpper
                    Case "MSaldoAwalHutangPiutang".ToUpper
                        UnPostingSaldoAwalHutangPiutang(NoID)
                    Case "MTransferPoin".ToUpper
                        UnPostingTransferPoin(NoID)
                    Case "MLPB".ToUpper
                        UnPostingLPB(NoID)
                    Case "MTT".ToUpper
                        UnPostingTandaTerimaSupplier(NoID)
                    Case "MPO".ToUpper
                        UnPostingPO(NoID)
                    Case "MSO".ToUpper
                        UnPostingSO(NoID)
                    Case "MSPK".ToUpper
                        UnPostingSPK(NoID)
                    Case "MPacking".ToUpper
                        UnPostingPacking(NoID)
                    Case "MDO".ToUpper
                        UnPostingDO(NoID)
                    Case "MBELI".ToUpper
                        UnPostingStokBarangPembelian(NoID)
                    Case "MJUAL".ToUpper
                        UnPostingStokBarangPenjualan(NoID)
                    Case "MReturJUAL".ToUpper
                        UnPostingStokBarangReturPenjualan(NoID)
                    Case "MReturBeli".ToUpper
                        UnPostingStokBarangReturPembelian(NoID)
                    Case "MMUTASIGUDANG".ToUpper
                        UnPostingStokBarangMutasiGudang(NoID)
                    Case "MREVISIHARGABELI".ToUpper
                        UnPostingStokBarangRevisiHargaPembelian(NoID)
                    Case "MREVISIHARGAJUAL".ToUpper
                        UnPostingStokBarangRevisiHargaPenjualan(NoID)
                    Case "MCREDITNOTE".ToUpper
                        UnPostingNotaKredit(NoID)
                    Case "MDEBETNOTE".ToUpper
                        UnPostingNotaDebet(NoID)
                    Case "MPemakaian".ToUpper
                        clsPostingMutasiWilayah.UnPostingPemakaian(NoID)
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
            MdiParent.Focus()
            Focus()
        End Try

    End Sub

    Private Sub GV1_FocusedRowChanged(ByVal sender As Object, ByVal e As DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs) Handles GV1.FocusedRowChanged
        Try
            If IsShowStock Then
                clsPostingKartuStok.LihatHasilPosting(NullToLong(GV1.GetRowCellValue(GV1.FocusedRowHandle, "NoID")), TableMaster)
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

    Private Sub mnFakturPanjang_ItemClick(ByVal sender As Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles mnFakturPanjang.ItemClick
        If FormName.ToUpper = "DAFTARPAJAKKELUARAN" AndAlso TableMaster.ToUpper = "MJual".ToUpper Then
            'If frmMain.ckEditRpt.Checked Then
            '    CetakFakturPajak(action_.Edit)
            'ElseIf frmMain.ckLangsungCetak.Checked Then
            '    CetakFakturPajak(action_.Print)
            'Else
            '    CetakFakturPajak(action_.Preview)
            'End If
        ElseIf FormName.ToUpper = "DAFTARPENJUALAN" AndAlso TableMaster.ToUpper = "MJual".ToUpper Then
            If frmMain.ckEditRpt.Checked Then
                CetakDetilPenjualanMember(action_.Edit)
            ElseIf frmMain.ckLangsungCetak.Checked Then
                CetakDetilPenjualanMember(action_.Print)
            Else
                CetakDetilPenjualanMember(action_.Preview)
            End If
        Else
            If frmMain.ckEditRpt.Checked Then
                CetakFakturPanjang(action_.Edit)
            ElseIf frmMain.ckLangsungCetak.Checked Then
                CetakFakturPanjang(action_.Print)
            Else
                CetakFakturPanjang(action_.Preview)
            End If
        End If
    End Sub

    Private Sub cmdFakturPanjang_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdFakturPanjang.Click
        mnFakturPanjang.PerformClick()
    End Sub

    Private Sub mnSetPOSelesai_ItemClick(ByVal sender As Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles mnSetPOSelesai.ItemClick
        Dim dlg As New WaitDialogForm("Proses Penyelesaian PO ...", "Mohon Tunggu Sebentar.")
        Dim NoID As Long = -1
        Try
            Enabled = False
            dlg.Show()
            dlg.Owner = Me
            dlg.TopMost = True
            Dim jumItem As Integer = GV1.SelectedRowsCount
            For Each i In GV1.GetSelectedRows
                NoID = NullToLong(GV1.GetDataRow(i).Item("NoID"))
                Select Case TableMaster.ToUpper
                    Case "MPO".ToUpper
                        EksekusiSQL("Update MPO Set IsSelesai=1 WHERE NoID=" & NoID)
                    Case "MSO".ToUpper
                        EksekusiSQL("Update MSO Set IsSelesai=1 WHERE NoID=" & NoID)
                    Case "MSPK".ToUpper
                        EksekusiSQL("Update MSPK Set IsSelesai=1 WHERE NoID=" & NoID)
                    Case "MPacking".ToUpper
                        EksekusiSQL("Update MPacking Set IsSelesai=1 WHERE NoID=" & NoID)
                End Select
                Enabled = True
                RefreshData()
                GV1.ClearSelection()
                GV1.FocusedRowHandle = GV1.LocateByDisplayText(0, GV1.Columns("NoID"), (NoID).ToString("#,##0"))
                GV1.SelectRow(GV1.FocusedRowHandle)
                Application.DoEvents()
            Next
        Catch ex As Exception
            XtraMessageBox.Show("Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            dlg.Close()
            dlg.Dispose()
            Enabled = True
        End Try
    End Sub

    Private Sub mnSetPOBelumSelesai_ItemClick(ByVal sender As Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles mnSetPOBelumSelesai.ItemClick
        Dim dlg As New WaitDialogForm("Proses Penyelesaian PO ...", "Mohon Tunggu Sebentar.")
        Dim NoID As Long = -1
        Try
            Enabled = False
            dlg.Show()
            dlg.Owner = Me
            dlg.TopMost = True
            Dim jumItem As Integer = GV1.SelectedRowsCount
            For Each i In GV1.GetSelectedRows
                NoID = NullToLong(GV1.GetDataRow(i).Item("NoID"))
                Select Case TableMaster.ToUpper
                    Case "MPO".ToUpper
                        EksekusiSQL("Update MPO Set IsSelesai=0 WHERE NoID=" & NoID)
                    Case "MSO".ToUpper
                        EksekusiSQL("Update MSO Set IsSelesai=0 WHERE NoID=" & NoID)
                    Case "MSPK".ToUpper
                        EksekusiSQL("Update MSPK Set IsSelesai=0 WHERE NoID=" & NoID)
                    Case "MPacking".ToUpper
                        EksekusiSQL("Update MPacking Set IsSelesai=0 WHERE NoID=" & NoID)
                End Select
                Enabled = True
                RefreshData()
                GV1.ClearSelection()
                GV1.FocusedRowHandle = GV1.LocateByDisplayText(0, GV1.Columns("NoID"), (NoID).ToString("#,##0"))
                GV1.SelectRow(GV1.FocusedRowHandle)
                Application.DoEvents()
            Next
        Catch ex As Exception
            XtraMessageBox.Show("Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            dlg.Close()
            dlg.Dispose()
            Enabled = True
        End Try
    End Sub

    Private Sub SimpleButton1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdBaru.Click
        If NullToBool(Ini.BacaIni("Application", "UseFramework", False)) Then
            Baru()
        Else
            Select Case TableMaster.ToUpper
                Case "MSaldoAwalHutangPiutang".ToUpper
                    Dim view As ColumnView = GC1.FocusedView
                    Dim x As New frmEntriSaldoAwalHutangPiutang
                    Try
                        x.NoID = -1
                        x.pTipe = frmEntriSaldoAwalHutangPiutang.pStatus.Baru
                        x.WindowState = FormWindowState.Normal
                        x.StartPosition = FormStartPosition.CenterParent
                        If x.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
                            RefreshData()
                            GV1.ClearSelection()
                            GV1.FocusedRowHandle = GV1.LocateByDisplayText(0, GV1.Columns("NoID"), x.NoID.ToString("#,##0"))
                            GV1.SelectRow(GV1.FocusedRowHandle)
                        End If
                    Catch ex As Exception
                        XtraMessageBox.Show("Error : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Finally
                        x.Dispose()
                    End Try
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
                    If FormName.ToUpper = "DaftarPenjualanPOS".ToUpper Or FormName.ToUpper = "DAFTARPENJUALAN".ToUpper Then
                        'Dim x As New frmEntriJualPOS
                        'Try
                        '    Dim row As System.Data.DataRow = view.GetDataRow(GV1.FocusedRowHandle)
                        '    Dim dc As Integer = GV1.FocusedRowHandle
                        '    Dim IDDetil As Long = NullToLong(row("NoID"))
                        '    x.NoID = IDDetil
                        '    x.pTipe = frmEntriJual.pStatus.Edit
                        '    x.MdiParent = Me.MdiParent
                        '    x.WindowState = FormWindowState.Maximized

                        '    'For Back Action
                        '    x.FormNameDaftar = FormName
                        '    x.TableNameDaftar = TableName
                        '    x.TextDaftar = Text
                        '    x.FormEntriDaftar = FormEntriName
                        '    x.TableMasterDaftar = TableMaster

                        '    If FormName.ToUpper = "DaftarPenjualanPOS".ToUpper Then
                        '        x.IsPosted = True
                        '    Else
                        '        x.IsPOS = False
                        '    End If

                        '    x.Show()
                        '    x.Focus()
                        'Catch ex As Exception
                        '    XtraMessageBox.Show("Untuk mengedit pilih data yang akan diedit terlebih dahulu lalu tekan tombol edit!", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Warning)
                        'End Try
                        XtraMessageBox.Show("Fitur baru hanya di mesin kasir.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
                    Else
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
                    End If
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
                    Dim view As ColumnView = GC1.FocusedView
                    Dim x As New frmEntriSPK
                    Try
                        x.NoID = -1
                        x.pTipe = frmEntriSPK.pStatus.Baru
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
                Case "MPacking".ToUpper
                    Dim view As ColumnView = GC1.FocusedView
                    Dim x As New frmEntriPacking
                    Try
                        x.NoID = -1
                        x.pTipe = frmEntriPacking.pStatus.Baru
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
                    Dim view As ColumnView = GC1.FocusedView
                    Dim x As New frmEntriRevisiBeli
                    Try
                        x.NoID = -1
                        x.pTipe = frmEntriRevisiBeli.pStatus.Baru
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
                Case "MRevisiHargaJual".ToUpper
                    Dim view As ColumnView = GC1.FocusedView
                    Dim x As New frmEntriRevisiJual
                    Try
                        x.NoID = -1
                        x.pTipe = frmEntriRevisiJual.pStatus.Baru
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
            End Select
        End If
    End Sub

    Private Sub mnPacking_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles mnPacking.ItemClick
        Dim view As ColumnView = GC1.FocusedView
        Dim x As New frmEntriSPK
        Try
            Dim row As System.Data.DataRow = view.GetDataRow(GV1.FocusedRowHandle)
            Dim dc As Integer = GV1.FocusedRowHandle
            Dim IDDetil As Long = NullToLong(row("NoID"))
            x.NoID = IDDetil
            x.pTipe = frmEntriSPK.pStatus.Edit
            x.MdiParent = Me.MdiParent
            x.WindowState = FormWindowState.Normal
            x.pSPK = frmEntriSPK.SPK.IsPacking

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

    Private Sub XtraTabControl1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles XtraTabControl1.Click

    End Sub

    Private Sub XtraTabControl1_SelectedPageChanged(ByVal sender As Object, ByVal e As DevExpress.XtraTab.TabPageChangedEventArgs) Handles XtraTabControl1.SelectedPageChanged
        If XtraTabControl1.SelectedTabPageIndex <> 0 Then
            PanelControl3.Visible = False
        Else
            PanelControl3.Visible = True
        End If
    End Sub

    Private Sub mnRefresh_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles mnRefresh.ItemClick
        'SimpleButton8.PerformClick()
        RefreshData()
    End Sub

    Private Sub mnHasilPosting_ItemClick(ByVal sender As Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles mnHasilPosting.ItemClick
        Try
            clsPostingKartuStok.LihatHasilPosting(NullToLong(GV1.GetRowCellValue(GV1.FocusedRowHandle, "NoID")), TableMaster)
        Catch ex As Exception
            XtraMessageBox.Show("Info : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
        End Try
    End Sub

    Private Sub GC1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles GC1.Click

    End Sub

    Private Sub ckAutoPosting_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ckAutoPosting.CheckedChanged
        If ckAutoPosting.Checked Then
            Timer1.Interval = 1000
            Interval = 1
            Timer1.Enabled = True
        Else
            Timer1.Enabled = False
        End If
    End Sub
    Dim Interval As Integer = 10

    Private Sub Timer1_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer1.Tick
        Dim SQL As String = ""
        Dim ds As New DataSet
        Try
            If Interval = 10 Then
                If TableMaster.ToUpper = "MJual".ToUpper Then
                    SQL = "SELECT * FROM MJual WHERE IsNull(MJual.IsPosted,0)=0 AND MJual.IsPOS=1 AND MJual.IDWilayah=" & DefIDWilayah & " AND (MJual.Tanggal>='" & Format(TglDari.DateTime, "yyyy/MM/dd") & "' AND MJual.Tanggal<'" & Format(DateAdd(DateInterval.Day, 1, TglSampai.DateTime), "yyyy/MM/dd") & "')"
                    ds = ExecuteDataset("MJual", SQL)
                    For i As Integer = 0 To ds.Tables("MJual").Rows.Count - 1
                        PostingStokBarangPenjualan(NullToLong(ds.Tables("MJual").Rows(i).Item("NoID")))
                    Next
                    RefreshData()
                    Interval = 1
                End If
            Else
                Interval = Interval + 1
            End If
        Catch ex As Exception
            Interval = 1
        End Try
    End Sub
    Private Sub lbCustomer_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lbCustomer.Click
        txtCustomer.Enabled = Not txtCustomer.Enabled
    End Sub

    Private Sub lbsalesman_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lbsalesman.Click
        txtSalesman.Enabled = Not txtSalesman.Enabled
    End Sub

    Private Sub PanelControl3_Paint(ByVal sender As System.Object, ByVal e As System.Windows.Forms.PaintEventArgs) Handles PanelControl3.Paint

    End Sub

    Private Sub lbKassa_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lbKassa.Click
        txtKassa.Enabled = Not txtKassa.Enabled
    End Sub

    Private Sub mnExportExcel_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles mnExportExcel.ItemClick
        Dim dlg As New SaveFileDialog
        Dim FileName As String
        Try
            If TableMaster.ToUpper = "MMUTASIGUDANG".ToUpper Then
                If XtraMessageBox.Show("Anda ingin mengexport Soft Copy Mutasi Barang dengan Kode " & NullToStr(GV1.GetRowCellValue(GV1.FocusedRowHandle, "Kode")) & " ?", NamaAplikasi, MessageBoxButtons.YesNo) = Windows.Forms.DialogResult.Yes Then
                    dlg.Title = "Soft Copy Mutasi Barang Antar Gudang"
                    dlg.Filter = "Excel Format|*.xls"
                    If dlg.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
                        FileName = dlg.FileName
                        If ExportExcelMutasiGudangFormatPembelian(NullToLong(GV1.GetRowCellValue(GV1.FocusedRowHandle, "NoID"))) Then
                            System.IO.File.Copy(Application.StartupPath & "\system\engine\MutasiGudang.xls", FileName, True)
                            XtraMessageBox.Show("Sukses Mengexport Data." & vbCrLf & "File Location : " & FileName, NamaAplikasi, MessageBoxButtons.OK)
                        End If
                    End If
                End If
            End If
        Catch ex As Exception
            XtraMessageBox.Show("Info Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
        Finally
            dlg.Dispose()
        End Try
    End Sub

    Private Sub mnCetakFPBlanko_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles mnCetakFPBlanko.ItemClick
        If frmMain.ckEditRpt.Checked Then
            CetakFakturPajakBlankoObat(action_.Edit, 1)
        ElseIf frmMain.ckLangsungCetak.Checked Then
            CetakFakturPajakBlankoObat(action_.Print, 1)
        Else
            CetakFakturPajakBlankoObat(action_.Preview, 1)
        End If
    End Sub

    Private Sub mnCetakFakturPajakKosong_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles mnCetakFPKosong.ItemClick
        If frmMain.ckEditRpt.Checked Then
            CetakFakturPajakObat(action_.Edit, 1)
        ElseIf frmMain.ckLangsungCetak.Checked Then
            CetakFakturPajakObat(action_.Print, 1)
        Else
            CetakFakturPajakObat(action_.Preview, 1)
        End If
    End Sub

    Private Sub txtfilter_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)

    End Sub

    Private Sub mnRevisi_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles mnRevisi.ItemClick
        If NullToBool(Ini.BacaIni("Application", "UseFramework", False)) Then
            Edit()
        Else
            Select Case TableMaster.ToUpper
                Case "MBELI"
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
                        Dim frmO As New frmOtorisasiAdmin
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

                            If Not clsPostingPembelian.PembelianAdaDiPembayaranHutang(IDDetil) Then
                                x.IsRevisi = True
                                x.Show()
                                x.Focus()
                            Else
                                XtraMessageBox.Show(Me, "Informasi : Data Sudah Masuk ke Pembayaran Hutang.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
                                If frmO.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
                                    x.IsRevisi = True
                                    x.Show()
                                    x.Focus()
                                Else
                                    x.IsRevisi = False
                                End If
                            End If
                        Catch ex As Exception
                            XtraMessageBox.Show("Untuk mengedit pilih data yang akan diedit terlebih dahulu lalu tekan tombol edit!", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Warning)
                        Finally
                            frmO.Dispose()
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
                        Dim frmO As New frmOtorisasiAdmin
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
                            If Not clsPostingPembelian.ReturAdaDiPembayaranHutang(IDDetil) Then
                                x.IsRevisi = True
                                x.Show()
                                x.Focus()
                            Else
                                XtraMessageBox.Show(Me, "Informasi : Data Sudah Masuk ke Pembayaran Hutang.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
                                If frmO.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
                                    x.IsRevisi = True
                                    x.Show()
                                    x.Focus()
                                Else
                                    x.IsRevisi = False
                                End If
                            End If
                        Catch ex As Exception
                            XtraMessageBox.Show("Untuk mengedit pilih data yang akan diedit terlebih dahulu lalu tekan tombol edit!", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Warning)
                        Finally
                            frmO.Dispose()
                        End Try
                    End If
            End Select
        End If
    End Sub

    Private Sub BarButtonItem7_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem7.ItemClick
        If frmMain.ckEditRpt.Checked Then
            CetakFakturPajakBlankoObat(action_.Edit, 1)
        ElseIf frmMain.ckLangsungCetak.Checked Then
            CetakFakturPajakBlankoObat(action_.Print, 1)
        Else
            CetakFakturPajakBlankoObat(action_.Preview, 1)
        End If
    End Sub

    Private Sub BarButtonItem8_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem8.ItemClick
        If frmMain.ckEditRpt.Checked Then
            CetakFakturPajakBlankoObat(action_.Edit, 2)
        ElseIf frmMain.ckLangsungCetak.Checked Then
            CetakFakturPajakBlankoObat(action_.Print, 2)
        Else
            CetakFakturPajakBlankoObat(action_.Preview, 2)
        End If
    End Sub

    Private Sub BarButtonItem9_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem9.ItemClick
        If frmMain.ckEditRpt.Checked Then
            CetakFakturPajakBlankoObat(action_.Edit, 3)
        ElseIf frmMain.ckLangsungCetak.Checked Then
            CetakFakturPajakBlankoObat(action_.Print, 3)
        Else
            CetakFakturPajakBlankoObat(action_.Preview, 3)
        End If
    End Sub

    Private Sub BarButtonItem10_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem10.ItemClick
        If frmMain.ckEditRpt.Checked Then
            CetakFakturPajakBlankoObat(action_.Edit, 4)
        ElseIf frmMain.ckLangsungCetak.Checked Then
            CetakFakturPajakBlankoObat(action_.Print, 4)
        Else
            CetakFakturPajakBlankoObat(action_.Preview, 4)
        End If
    End Sub

    Private Sub BarButtonItem11_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem11.ItemClick
        If frmMain.ckEditRpt.Checked Then
            CetakFakturPajakObat(action_.Edit, 1)
        ElseIf frmMain.ckLangsungCetak.Checked Then
            CetakFakturPajakObat(action_.Print, 1)
        Else
            CetakFakturPajakObat(action_.Preview, 1)
        End If
    End Sub

    Private Sub BarButtonItem12_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem12.ItemClick
        If frmMain.ckEditRpt.Checked Then
            CetakFakturPajakObat(action_.Edit, 2)
        ElseIf frmMain.ckLangsungCetak.Checked Then
            CetakFakturPajakObat(action_.Print, 2)
        Else
            CetakFakturPajakObat(action_.Preview, 2)
        End If
    End Sub

    Private Sub BarButtonItem13_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem13.ItemClick
        If frmMain.ckEditRpt.Checked Then
            CetakFakturPajakObat(action_.Edit, 3)
        ElseIf frmMain.ckLangsungCetak.Checked Then
            CetakFakturPajakObat(action_.Print, 3)
        Else
            CetakFakturPajakObat(action_.Preview, 3)
        End If
    End Sub

    Private Sub BarButtonItem14_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem14.ItemClick
        If frmMain.ckEditRpt.Checked Then
            CetakFakturPajakObat(action_.Edit, 4)
        ElseIf frmMain.ckLangsungCetak.Checked Then
            CetakFakturPajakObat(action_.Print, 4)
        Else
            CetakFakturPajakObat(action_.Preview, 4)
        End If
    End Sub

    Private Sub BarButtonItem15_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem15.ItemClick
        If frmMain.ckEditRpt.Checked Then
            CetakFakturPajakBlankoPromosi(action_.Edit, 1)
        ElseIf frmMain.ckLangsungCetak.Checked Then
            CetakFakturPajakBlankoPromosi(action_.Print, 1)
        Else
            CetakFakturPajakBlankoPromosi(action_.Preview, 1)
        End If
    End Sub

    Private Sub BarButtonItem16_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem16.ItemClick
        If frmMain.ckEditRpt.Checked Then
            CetakFakturPajakBlankoPromosi(action_.Edit, 2)
        ElseIf frmMain.ckLangsungCetak.Checked Then
            CetakFakturPajakBlankoPromosi(action_.Print, 2)
        Else
            CetakFakturPajakBlankoPromosi(action_.Preview, 2)
        End If
    End Sub

    Private Sub BarButtonItem17_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem17.ItemClick
        If frmMain.ckEditRpt.Checked Then
            CetakFakturPajakBlankoPromosi(action_.Edit, 3)
        ElseIf frmMain.ckLangsungCetak.Checked Then
            CetakFakturPajakBlankoPromosi(action_.Print, 3)
        Else
            CetakFakturPajakBlankoPromosi(action_.Preview, 3)
        End If
    End Sub

    Private Sub BarButtonItem18_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem18.ItemClick
        If frmMain.ckEditRpt.Checked Then
            CetakFakturPajakBlankoPromosi(action_.Edit, 4)
        ElseIf frmMain.ckLangsungCetak.Checked Then
            CetakFakturPajakBlankoPromosi(action_.Print, 4)
        Else
            CetakFakturPajakBlankoPromosi(action_.Preview, 4)
        End If
    End Sub

    Private Sub BarButtonItem19_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem19.ItemClick
        If frmMain.ckEditRpt.Checked Then
            CetakFakturPajakPromosi(action_.Edit, 1)
        ElseIf frmMain.ckLangsungCetak.Checked Then
            CetakFakturPajakPromosi(action_.Print, 1)
        Else
            CetakFakturPajakPromosi(action_.Preview, 1)
        End If
    End Sub

    Private Sub BarButtonItem20_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem20.ItemClick
        If frmMain.ckEditRpt.Checked Then
            CetakFakturPajakPromosi(action_.Edit, 2)
        ElseIf frmMain.ckLangsungCetak.Checked Then
            CetakFakturPajakPromosi(action_.Print, 2)
        Else
            CetakFakturPajakPromosi(action_.Preview, 2)
        End If
    End Sub

    Private Sub BarButtonItem21_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem21.ItemClick
        If frmMain.ckEditRpt.Checked Then
            CetakFakturPajakPromosi(action_.Edit, 3)
        ElseIf frmMain.ckLangsungCetak.Checked Then
            CetakFakturPajakPromosi(action_.Print, 3)
        Else
            CetakFakturPajakPromosi(action_.Preview, 3)
        End If
    End Sub

    Private Sub BarButtonItem22_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem22.ItemClick
        If frmMain.ckEditRpt.Checked Then
            CetakFakturPajakPromosi(action_.Edit, 4)
        ElseIf frmMain.ckLangsungCetak.Checked Then
            CetakFakturPajakPromosi(action_.Print, 4)
        Else
            CetakFakturPajakPromosi(action_.Preview, 4)
        End If
    End Sub

    Private Sub mnEntriFakturPajakRetur_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles mnEntriFakturPajakRetur.ItemClick
        If NullToBool(Ini.BacaIni("Application", "UseFramework", False)) Then
            Edit()
        Else
            Select Case TableMaster.ToUpper
                Case "MRETURBELI"
                    If FormEntriName.ToUpper = "ENTRIRETURBELITANPABARANG" Then
                        Dim view As ColumnView = GC1.FocusedView
                        Dim x As New frmEntriReturBeliTanpaBarang
                        Try
                            Dim row As System.Data.DataRow = view.GetDataRow(GV1.FocusedRowHandle)
                            Dim dc As Integer = GV1.FocusedRowHandle
                            Dim IDDetil As Long = NullToLong(row("NoID"))
                            Dim MasaPajak As Date = NullToDate(row("MasaPajak"))
                            If Not clsPostingPembelian.IsLockPeriodeFP(MasaPajak) Then
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
                            Else
                                XtraMessageBox.Show("Masa Pajak " & MasaPajak.ToString("MMMM-yyyy") & " sudah dikunci.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
                            End If
                        Catch ex As Exception
                            XtraMessageBox.Show("Untuk mengedit pilih data yang akan diedit terlebih dahulu lalu tekan tombol edit!", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Warning)
                        End Try
                    Else
                        Dim view As ColumnView = GC1.FocusedView
                        Dim x As New frmEntriReturBeli
                        Dim frmO As New frmOtorisasiAdmin
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
                            If Not clsPostingPembelian.ReturAdaDiPembayaranHutang(IDDetil) Then
                                x.IsRevisi = True
                                x.IsEntriFakturPajak = True
                                x.Show()
                                x.Focus()
                            Else
                                XtraMessageBox.Show(Me, "Informasi : Data Sudah Masuk ke Pembayaran Hutang.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
                                If frmO.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
                                    x.IsRevisi = True
                                    x.IsEntriFakturPajak = True
                                    x.Show()
                                    x.Focus()
                                Else
                                    x.IsRevisi = False
                                    x.IsEntriFakturPajak = False
                                End If
                            End If
                        Catch ex As Exception
                            XtraMessageBox.Show("Untuk mengedit pilih data yang akan diedit terlebih dahulu lalu tekan tombol edit!", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Warning)
                        Finally
                            frmO.Dispose()
                        End Try
                    End If
            End Select
        End If
    End Sub

    Private Sub mnLockStockOpname_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles mnLockStockOpname.ItemClick
        Try
            If TableMaster.ToUpper = "MStockOpname".ToUpper Then
                If XtraMessageBox.Show("Yakin ingin mengunci stock opname ini?", NamaAplikasi, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) = Windows.Forms.DialogResult.Yes Then
                    For Each i As Integer In GV1.GetSelectedRows
                        clsPostingMutasiWilayah.PostingStockOpname(NullToLong(GV1.GetRowCellValue(i, "NoID")))
                        EksekusiSQL("UPDATE MStockOpname SET IDUserLock=" & IDUserAktif & ", TglLock='" & TanggalSystem.ToString("yyyy-MM-dd") & "', IsLock=1 WHERE NoID=" & NullToLong(GV1.GetRowCellValue(i, "NoID")))
                    Next
                End If
            End If
        Catch ex As Exception
            XtraMessageBox.Show("Info Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
        End Try
    End Sub

    Private Sub mnUnLockStockOpname_ItemClick(ByVal sender As Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles mnUnLockStockOpname.ItemClick
        Try
            If TableMaster.ToUpper = "MStockOpname".ToUpper Then
                If XtraMessageBox.Show("Yakin ingin membuka kunci stock opname ini?", NamaAplikasi, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) = Windows.Forms.DialogResult.Yes Then
                    For Each i As Integer In GV1.GetSelectedRows
                        'clsPostingMutasiWilayah.PostingStockOpname(NullToLong(GV1.GetRowCellValue(i, "NoID")))
                        EksekusiSQL("UPDATE MStockOpname SET IDUserLock=NULL, TglLock=NULL, IsLock=0 WHERE NoID=" & NullToLong(GV1.GetRowCellValue(i, "NoID")))
                    Next
                End If
            End If
        Catch ex As Exception
            XtraMessageBox.Show("Info Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
        End Try
    End Sub
End Class