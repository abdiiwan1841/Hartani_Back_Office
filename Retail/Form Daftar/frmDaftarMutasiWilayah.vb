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
Imports VPoint.clsPostingMutasiWilayah
Imports DevExpress.Utils
Imports DevExpress.XtraGrid.Views.Grid.ViewInfo
Imports DevExpress.XtraGrid.Views.Grid
Imports DevExpress.Utils.Menu
Imports DevExpress.XtraGrid.Localization

Public Class frmDaftarMutasiWilayah
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

    Private Sub frmDaftarMasterDetil_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Activated
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
    Private Function DiBeli() As String
        Return "isnull((SELECT SUM(isnull(A.Qty,0)*isnull(A.Konversi,0)) FROM MBeliD A WHERE A.IDPOD=MPOD.NoID),0)"
    End Function
    Private Function RequestDiSPK() As String
        Return "isnull((SELECT SUM(isnull(A.Qty,0)*isnull(A.Konversi,0)) FROM MSPKMutasiWilayahD A WHERE A.IDRequestMutasiWilayahD=MRequestMutasiWilayahD.NoID),0)"
    End Function
    Private Function SPKWDiPacking() As String
        Return "isnull((SELECT SUM(isnull(A.Qty,0)*isnull(A.Konversi,0)) FROM MPackingMutasiWilayahD A WHERE A.IDSPKMutasiWilayahD=MSPKMutasiWilayahD.NoID),0)"
    End Function
    Private Function PackingDiTO() As String
        Return "isnull((SELECT SUM(isnull(A.Qty,0)*isnull(A.Konversi,0)) FROM MTransferOutD A WHERE A.IDPackingMutasiWilayahD=MPackingMutasiWilayahD.NoID),0)"
    End Function
    Private Function TODiTI() As String
        Return "isnull((SELECT SUM(isnull(A.Qty,0)*isnull(A.Konversi,0)) FROM MTransferIND A WHERE A.IDTransferOutD=MTransferOutD.NoID),0)"
    End Function
    Private Function SODiSPK() As String
        Return "isnull((SELECT SUM(isnull(A.Qty,0)*isnull(A.Konversi,0)) FROM MSPKD A WHERE A.IDSOD=MSOD.NoID),0)"
    End Function
    Private Function SPKDiPacking() As String
        Return "isnull((SELECT SUM(isnull(A.Qty,0)*isnull(A.Konversi,0)) FROM MPackingD A WHERE A.IDSPKD=MSPKD.NoID),0)"
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

    Private Sub SimpleButton2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SimpleButton2.Click
        If NullToBool(Ini.BacaIni("Application", "UseFramework", False)) Then
            Edit()
        Else
            Select Case TableMaster.ToUpper
                Case "MTransferKode".ToUpper
                    Dim view As ColumnView = GC1.FocusedView
                    Dim x As New frmEntriTransferKodeBarang
                    Try
                        Dim row As System.Data.DataRow = view.GetDataRow(GV1.FocusedRowHandle)
                        Dim dc As Integer = GV1.FocusedRowHandle
                        Dim IDDetil As Long = NullToLong(row("NoID"))
                        x.NoID = IDDetil
                        x.pTipe = frmEntriTransferKodeBarang.pStatus.Edit
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
                Case "MStockOpname".ToUpper
                    Dim view As ColumnView = GC1.FocusedView
                    Dim x As New frmEntriStockOpname
                    Try
                        Dim row As System.Data.DataRow = view.GetDataRow(GV1.FocusedRowHandle)
                        Dim dc As Integer = GV1.FocusedRowHandle
                        Dim IDDetil As Long = NullToLong(row("NoID"))
                        x.NoID = IDDetil
                        x.pTipe = frmEntriStockOpname.pStatus.Edit
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
                Case "MPEMAKAIAN"
                    Dim view As ColumnView = GC1.FocusedView
                    Dim x As New frmEntriPemakaianBarang
                    Try
                        Dim row As System.Data.DataRow = view.GetDataRow(GV1.FocusedRowHandle)
                        Dim dc As Integer = GV1.FocusedRowHandle
                        Dim IDDetil As Long = NullToLong(row("NoID"))
                        x.NoID = IDDetil
                        x.pTipe = frmEntriPemakaianBarang.pStatus.Edit
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
                Case "MPENYESUAIAN"
                    Dim view As ColumnView = GC1.FocusedView
                    Dim x As New frmEntriPenyesuaianBarang
                    Try
                        Dim row As System.Data.DataRow = view.GetDataRow(GV1.FocusedRowHandle)
                        Dim dc As Integer = GV1.FocusedRowHandle
                        Dim IDDetil As Long = NullToLong(row("NoID"))
                        x.NoID = IDDetil
                        x.pTipe = frmEntriPenyesuaianBarang.pStatus.Edit
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
                Case "MSaldoAwalPersediaan".ToUpper
                    Dim view As ColumnView = GC1.FocusedView
                    Dim x As New frmEntriSaldoAwalPersediaan
                    Try
                        Dim row As System.Data.DataRow = view.GetDataRow(GV1.FocusedRowHandle)
                        Dim dc As Integer = GV1.FocusedRowHandle
                        Dim IDDetil As Long = NullToLong(row("NoID"))
                        x.NoID = IDDetil
                        x.pTipe = frmEntriSaldoAwalPersediaan.pStatus.Edit
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
                Case "MTRANSFERIN"
                    Dim view As ColumnView = GC1.FocusedView
                    Dim x As New frmEntriTransferINMutasiWilayah
                    Try
                        Dim row As System.Data.DataRow = view.GetDataRow(GV1.FocusedRowHandle)
                        Dim dc As Integer = GV1.FocusedRowHandle
                        Dim IDDetil As Long = NullToLong(row("NoID"))
                        x.NoID = IDDetil
                        x.pTipe = frmEntriTransferINMutasiWilayah.pStatus.Edit
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
                Case "MTRANSFEROUT"
                    Dim view As ColumnView = GC1.FocusedView
                    Dim x As New frmEntriTransferOutMutasiWilayah
                    Try
                        Dim row As System.Data.DataRow = view.GetDataRow(GV1.FocusedRowHandle)
                        Dim dc As Integer = GV1.FocusedRowHandle
                        Dim IDDetil As Long = NullToLong(row("NoID"))
                        x.NoID = IDDetil
                        x.pTipe = frmEntriTransferOutMutasiWilayah.pStatus.Edit
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
                Case "MPACKINGMUTASIWILAYAH"
                    Dim view As ColumnView = GC1.FocusedView
                    Dim x As New frmEntriPackingMutasiWilayah
                    Try
                        Dim row As System.Data.DataRow = view.GetDataRow(GV1.FocusedRowHandle)
                        Dim dc As Integer = GV1.FocusedRowHandle
                        Dim IDDetil As Long = NullToLong(row("NoID"))
                        x.NoID = IDDetil
                        x.pTipe = frmEntriPackingMutasiWilayah.pStatus.Edit
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
                Case "MSPKMUTASIWILAYAH"
                    Dim view As ColumnView = GC1.FocusedView
                    Dim x As New frmEntriSPKMutasiWilayah
                    Try
                        Dim row As System.Data.DataRow = view.GetDataRow(GV1.FocusedRowHandle)
                        Dim dc As Integer = GV1.FocusedRowHandle
                        Dim IDDetil As Long = NullToLong(row("NoID"))
                        x.NoID = IDDetil
                        x.pTipe = frmEntriSPKMutasiWilayah.pStatus.Edit
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
                Case "MREQUESTMUTASIWILAYAH"
                    Dim view As ColumnView = GC1.FocusedView
                    Dim x As New frmEntriRequestMutasiWilayah
                    Try
                        Dim row As System.Data.DataRow = view.GetDataRow(GV1.FocusedRowHandle)
                        Dim dc As Integer = GV1.FocusedRowHandle
                        Dim IDDetil As Long = NullToLong(row("NoID"))
                        x.NoID = IDDetil
                        x.pTipe = frmEntriRequestMutasiWilayah.pStatus.Edit
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
                    Dim view As ColumnView = GC1.FocusedView
                    Dim x As New frmEntriSO
                    Try
                        Dim row As System.Data.DataRow = view.GetDataRow(GV1.FocusedRowHandle)
                        Dim dc As Integer = GV1.FocusedRowHandle
                        Dim IDDetil As Long = NullToLong(row("NoID"))
                        x.NoID = IDDetil
                        x.pTipe = frmEntriSO.pStatus.Edit
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
                        XtraMessageBox.Show("Untuk mengedit pilih data yang akan diedit terlebih dahulu lalu tekan tombol edit!", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Warning)
                    End Try
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
                Case "MBELI"
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
                Case "MRETURBELI"
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
                Case "MRevisiHargaBeli".ToUpper
                    Dim view As ColumnView = GC1.FocusedView
                    Dim x As New frmEntriRevisiBeliD
                    Try
                        Dim row As System.Data.DataRow = view.GetDataRow(GV1.FocusedRowHandle)
                        Dim dc As Integer = GV1.FocusedRowHandle
                        Dim IDDetil As Long = NullToLong(row("NoID"))
                        x.NoID = IDDetil
                        x.IsNew = False
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
            If TableMaster.ToLower = "MRequestMutasiWilayah".ToLower Then
                If InStr(strsql.ToLower, "where", CompareMethod.Text) > 0 Then
                    strsql = strsql & " and " & TableMaster & ".IDWilayahUntuk=" & DefIDWilayah
                Else
                    strsql = strsql & " where " & TableMaster & ".IDWilayahUntuk=" & DefIDWilayah
                End If
            ElseIf TableMaster.ToLower = "MSPKMutasiWilayah".ToLower Or TableMaster.ToLower = "MPackingMutasiWilayah".ToLower Then
                If InStr(strsql.ToLower, "where", CompareMethod.Text) > 0 Then
                    strsql = strsql & " and " & TableMaster & ".IDWilayahDari=" & DefIDWilayah
                Else
                    strsql = strsql & " where " & TableMaster & ".IDWilayahDari=" & DefIDWilayah
                End If
            ElseIf TableMaster.ToLower = "MTransferOut".ToLower Then
                If InStr(strsql.ToLower, "where", CompareMethod.Text) > 0 Then
                    strsql = strsql & " and " & TableMaster & ".IDWilayahDari=" & DefIDWilayah
                Else
                    strsql = strsql & " where " & TableMaster & ".IDWilayahDari=" & DefIDWilayah
                End If
            ElseIf TableMaster.ToLower = "MTransferIN".ToLower Then
                If InStr(strsql.ToLower, "where", CompareMethod.Text) > 0 Then
                    strsql = strsql & " and " & TableMaster & ".IDWilayahDari=" & DefIDWilayah
                Else
                    strsql = strsql & " where " & TableMaster & ".IDWilayahDari=" & DefIDWilayah
                End If
            Else
                If InStr(strsql.ToLower, "where", CompareMethod.Text) > 0 Then
                    strsql = strsql & " and " & TableMaster & ".IDWilayah=" & DefIDWilayah
                Else
                    strsql = strsql & " where " & TableMaster & ".IDWilayah=" & DefIDWilayah
                End If
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

            If TableMaster.ToUpper = "MSO".ToUpper Then
                mnGenerateSPK.Visibility = DevExpress.XtraBars.BarItemVisibility.Always
                mnPacking.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
                mnSetPOSelesai.Visibility = DevExpress.XtraBars.BarItemVisibility.Always
                mnSetPOBelumSelesai.Visibility = DevExpress.XtraBars.BarItemVisibility.Always
                If FormName.ToLower = "daftarpermintaanbarang" Then
                    mnSetPOBelumSelesai.Caption = "Set Permintaan Belum Selesai"
                    mnSetPOSelesai.Caption = "Set Permintaan Selesai"
                Else
                    mnSetPOBelumSelesai.Caption = "Set SO Belum Selesai"
                    mnSetPOSelesai.Caption = "Set SO Selesai"
                End If
                mnHasilPosting.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
            ElseIf TableMaster.ToUpper = "MPO".ToUpper Then
                mnGenerateSPK.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
                mnSetPOSelesai.Visibility = DevExpress.XtraBars.BarItemVisibility.Always
                mnSetPOBelumSelesai.Visibility = DevExpress.XtraBars.BarItemVisibility.Always
                mnPacking.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
                mnHasilPosting.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
            ElseIf TableMaster.ToUpper = "MSPK".ToUpper Then
                mnSetPOSelesai.Visibility = DevExpress.XtraBars.BarItemVisibility.Always
                mnSetPOBelumSelesai.Visibility = DevExpress.XtraBars.BarItemVisibility.Always
                mnSetPOBelumSelesai.Caption = "Set SPK Belum Selesai"
                mnSetPOSelesai.Caption = "Set SPK Selesai"
                mnGenerateSPK.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
                mnPacking.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
                mnHasilPosting.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
            ElseIf TableMaster.ToUpper = "MPacking".ToUpper Then
                mnSetPOSelesai.Visibility = DevExpress.XtraBars.BarItemVisibility.Always
                mnSetPOBelumSelesai.Visibility = DevExpress.XtraBars.BarItemVisibility.Always
                mnSetPOBelumSelesai.Caption = "Set Packing Belum Selesai"
                mnSetPOSelesai.Caption = "Set Packing Selesai"
                mnGenerateSPK.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
                mnPacking.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
                mnHasilPosting.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
            ElseIf TableMaster.ToUpper = "MRequestMutasiWilayah".ToUpper Then
                mnSetPOSelesai.Visibility = DevExpress.XtraBars.BarItemVisibility.Always
                mnSetPOBelumSelesai.Visibility = DevExpress.XtraBars.BarItemVisibility.Always
                mnSetPOBelumSelesai.Caption = "Set Request Belum Selesai"
                mnSetPOSelesai.Caption = "Set Request Selesai"
                mnGenerateSPK.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
                mnPacking.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
                mnHasilPosting.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
                If IsAccMutasi Then
                    mnAccPusat.Visibility = DevExpress.XtraBars.BarItemVisibility.Always
                    mnUnAccPusat.Visibility = DevExpress.XtraBars.BarItemVisibility.Always
                End If
            ElseIf TableMaster.ToUpper = "MSPKMutasiWilayah".ToUpper Then
                mnSetPOSelesai.Visibility = DevExpress.XtraBars.BarItemVisibility.Always
                mnSetPOBelumSelesai.Visibility = DevExpress.XtraBars.BarItemVisibility.Always
                mnSetPOBelumSelesai.Caption = "Set SPK Belum Selesai"
                mnSetPOSelesai.Caption = "Set SPK Selesai"
                mnGenerateSPK.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
                mnPacking.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
                mnHasilPosting.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
            ElseIf TableMaster.ToUpper = "MPackingMutasiWilayah".ToUpper Then
                mnSetPOSelesai.Visibility = DevExpress.XtraBars.BarItemVisibility.Always
                mnSetPOBelumSelesai.Visibility = DevExpress.XtraBars.BarItemVisibility.Always
                mnSetPOBelumSelesai.Caption = "Set Packing Belum Selesai"
                mnSetPOSelesai.Caption = "Set Packing Selesai"
                mnGenerateSPK.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
                mnPacking.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
                mnHasilPosting.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
            ElseIf TableMaster.ToUpper = "MTransferIN".ToUpper Then
                mnHasilPosting.Visibility = DevExpress.XtraBars.BarItemVisibility.Always
                mnSetPOBelumSelesai.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
                mnSetPOSelesai.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
                mnGenerateSPK.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
                mnPacking.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
            ElseIf TableMaster.ToUpper = "MTransferOut".ToUpper Then
                mnSetPOBelumSelesai.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
                mnSetPOSelesai.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
                mnGenerateSPK.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
                mnPacking.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
                mnHasilPosting.Visibility = DevExpress.XtraBars.BarItemVisibility.Always
            ElseIf TableMaster.ToUpper = "MSaldoAwalPersediaan".ToUpper Then
                mnSetPOBelumSelesai.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
                mnSetPOSelesai.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
                mnGenerateSPK.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
                mnPacking.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
                mnHasilPosting.Visibility = DevExpress.XtraBars.BarItemVisibility.Always
            ElseIf TableMaster.ToUpper = "MPemakaian".ToUpper Then
                mnSetPOBelumSelesai.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
                mnSetPOSelesai.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
                mnGenerateSPK.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
                mnPacking.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
                mnHasilPosting.Visibility = DevExpress.XtraBars.BarItemVisibility.Always
            ElseIf TableMaster.ToUpper = "MStockOpname".ToUpper Then
                mnSetPOBelumSelesai.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
                mnSetPOSelesai.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
                mnGenerateSPK.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
                mnPacking.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
                mnHasilPosting.Visibility = DevExpress.XtraBars.BarItemVisibility.Always
                mnStockOpname.Visibility = DevExpress.XtraBars.BarItemVisibility.Always
            Else
                mnSetPOBelumSelesai.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
                mnSetPOSelesai.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
                mnGenerateSPK.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
                mnPacking.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
                mnHasilPosting.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
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
                SQL &= " LEFT JOIN MGudang ON MGudang.NoID=MPOD.IDGudang "
                SQL &= " LEFT JOIN MWilayah ON MWilayah.NoID=MPOD.IDWilayah "
                SQL &= " WHERE (MPO.IsSelesai=0 OR MPO.IsSelesai IS NULL) AND MPO.IDWilayah=" & DefIDWilayah & " AND (MPOD.Qty*MPOD.Konversi - " & DiBeli() & ">0) AND MBarang.IsActive = 1 And MPO.IsPosted = 1 "
                dsT2 = ExecuteDataset("MPOD", SQL)
                GridControl1.DataSource = dsT2.Tables("MPOD")

                SQL = "SELECT MLPBD.NoID, MGudang.Nama AS NamaGudang, MWilayah.Nama AS NamaWilayah, MBarang.Kode AS KodeBarang, MBarang.Nama AS NamaBarang, MLPB.Kode AS NoLPB, MLPB.Tanggal, MAlamat.Nama AS Supplier, MBagPembelian.Nama AS Penerima, MLPBD.Qty * MLPBD.Konversi AS [QtyPcs], MLPBD.Qty*MLPBD.Konversi-" & DiSTB() & "  AS [Sisa(Pcs)] "
                SQL &= " FROM (MLPBD LEFT OUTER JOIN (MLPB LEFT OUTER JOIN MAlamat AS MBagPembelian ON MBagPembelian.NoID = MLPB.IDBagPembelian "
                SQL &= " LEFT OUTER JOIN MAlamat ON MLPB.IDSupplier = MAlamat.NoID)"
                SQL &= " ON MLPB.NoID=MLPBD.IDLPB) "
                SQL &= " LEFT OUTER JOIN MBarang ON MLPBD.IDBarang = MBarang.NoID "
                SQL &= " LEFT OUTER JOIN (MGudang LEFT OUTER JOIN MWilayah ON MWilayah.NoID=MGudang.IDWilayah) ON MGudang.NoID=MLPBD.IDGudang "
                SQL &= " WHERE MBarang.ISActive=1 AND (MLPBD.Qty*MLPBD.Konversi-" & DiSTB() & ">0) AND MLPB.IDWilayah=" & DefIDWilayah & " And MLPB.IsPosted = 1 "

                dsT2 = ExecuteDataset("MLPBD", SQL)
                GridControl2.DataSource = dsT2.Tables("MLPBD")

                XtraTabPage2.PageVisible = True
                XtraTabPage3.PageVisible = True
            ElseIf TableMaster.ToUpper = "MSPK".ToUpper Then
                SQL = "SELECT MSOD.NoID, MSO.Kode AS NoSO, MGudang.Nama AS NamaGudang, MWilayah.Nama AS NamaWilayah, MBarang.Kode AS KodeBarang, MAlamat.Kode AS KodeSupplier, MAlamat.Nama AS NamaSupplier, MBarang.Nama AS NamaBarang, " & _
                      " (MSOD.Harga-isnull(MSOD.DISC1,0)-isnull(MSOD.DISC2,0)-isnull(MSOD.DISC3,0)" & _
                      " -(MSOD.Harga*isnull(MSOD.DiscPersen1,0)/100)" & _
                      " -(MSOD.Harga*isnull(MSOD.DiscPersen2,0)/100)" & _
                      " -(MSOD.Harga*isnull(MSOD.DiscPersen3,0)/100)" & _
                      " ) AS Harga , MSOD.Qty, MSatuan.Nama AS Satuan, MSOD.HargaPcs AS [Harga(Pcs)], MSOD.Qty*MSOD.Konversi AS [Qty(Pcs)], MSOD.Qty*MSOD.Konversi- " & SODiSPK() & "  AS [Sisa(Pcs)]" & _
                      " FROM MSOD LEFT JOIN MSatuan ON MSatuan.NoID=MSOD.IDSatuan " & _
                      " LEFT JOIN (MSO LEFT JOIN MAlamat ON MAlamat.NoID=MSO.IDCustomer) ON MSO.NoID=MSOD.IDSO " & _
                      " LEFT JOIN MBarang ON MBarang.NoID=MSOD.IDBarang " & _
                      " LEFT JOIN (MGudang LEFT JOIN MWilayah ON MWilayah.NoID=MGudang.IDWilayah) ON MGudang.NoID=MSOD.IDGudang " & _
                      " WHERE (MSO.IsSelesai=0 OR MSO.IsSelesai IS NULL) AND MSO.IDWilayah=" & DefIDWilayah & " AND (MSOD.Qty*MSOD.Konversi - " & SODiSPK() & ">0) AND MBarang.IsActive = 1 And MSO.IsPosted = 1 "
                dsT2 = ExecuteDataset("MPOD", SQL)
                GridControl1.DataSource = dsT2.Tables("MPOD")
                XtraTabPage2.Text = "SO Belum di SPK"

                XtraTabPage2.PageVisible = True
                XtraTabPage3.PageVisible = False
            ElseIf TableMaster.ToUpper = "MPacking".ToUpper Then
                SQL = "SELECT MSPKD.NoID, MSPK.Kode AS NoSPK, MGudang.Nama AS NamaGudang, MWilayah.Nama AS NamaWilayah, MBarang.Kode AS KodeBarang, MAlamat.Kode AS KodeSupplier, MAlamat.Nama AS NamaSupplier, MBarang.Nama AS NamaBarang, " & _
                      " (MSPKD.Harga-isnull(MSPKD.DISC1,0)-isnull(MSPKD.DISC2,0)-isnull(MSPKD.DISC3,0)" & _
                      " -(MSPKD.Harga*isnull(MSPKD.DiscPersen1,0)/100)" & _
                      " -(MSPKD.Harga*isnull(MSPKD.DiscPersen2,0)/100)" & _
                      " -(MSPKD.Harga*isnull(MSPKD.DiscPersen3,0)/100)" & _
                      " ) AS Harga , MSPKD.Qty, MSatuan.Nama AS Satuan, MSPKD.HargaPcs AS [Harga(Pcs)], MSPKD.Qty*MSPKD.Konversi AS [Qty(Pcs)], MSPKD.Qty*MSPKD.Konversi- " & SPKDiPacking() & "  AS [Sisa(Pcs)]" & _
                      " FROM MSPKD LEFT JOIN MSatuan ON MSatuan.NoID=MSPKD.IDSatuan " & _
                      " LEFT JOIN (MSPK LEFT JOIN MAlamat ON MAlamat.NoID=MSPK.IDCustomer) ON MSPK.NoID=MSPKD.IDSPK " & _
                      " LEFT JOIN MBarang ON MBarang.NoID=MSPKD.IDBarang " & _
                      " LEFT JOIN (MGudang LEFT JOIN MWilayah ON MWilayah.NoID=MGudang.IDWilayah) ON MGudang.NoID=MSPKD.IDGudang " & _
                      " WHERE (MSPK.IsSelesai=0 OR MSPK.IsSelesai IS NULL) AND MSPK.IDWilayah=" & DefIDWilayah & " AND (MSPKD.Qty*MSPKD.Konversi - " & SPKDiPacking() & ">0) AND MBarang.IsActive = 1 And MSPK.IsPosted = 1 "
                dsT2 = ExecuteDataset("MPOD", SQL)
                GridControl1.DataSource = dsT2.Tables("MPOD")
                XtraTabPage2.Text = "SPK Belum di Packing"

                XtraTabPage2.PageVisible = True
                XtraTabPage3.PageVisible = False
            ElseIf TableMaster.ToUpper = "MSPKMutasiWilayah".ToUpper Then
                SQL = "SELECT MRequestMutasiWilayahD.NoID, MRequestMutasiWilayah.Kode AS NoRequest, MWilayahUntuk.Nama AS WilayahYgMeminta, MBarang.Kode AS KodeBarang, MAlamat.Nama AS YgMeminta, MBarang.Nama AS NamaBarang, " & _
                      " MWilayahDari.Nama AS WilayahYgDimintai, MRequestMutasiWilayah.*, MRequestMutasiWilayahD.Qty, MRequestMutasiWilayahD.Konversi, MRequestMutasiWilayahD.QtyPcs, MSatuan.Nama AS Satuan, " & _
                      " MRequestMutasiWilayahD.Qty*MRequestMutasiWilayahD.Konversi- " & RequestDiSPK() & "  AS [Sisa(Pcs)]" & _
                      " FROM MRequestMutasiWilayahD LEFT JOIN MSatuan ON MSatuan.NoID=MRequestMutasiWilayahD.IDSatuan " & vbCrLf & _
                      " LEFT JOIN (((MRequestMutasiWilayah " & _
                      " LEFT JOIN MAlamat ON MAlamat.NoID=MRequestMutasiWilayah.IDPegawaiRequest) " & _
                      " LEFT JOIN MWilayah MWilayahDari ON MWilayahDari.NoID=MRequestMutasiWilayah.IDWilayahDari) " & _
                      " LEFT JOIN MWilayah MWilayahUntuk ON MWilayahUntuk.NoID=MRequestMutasiWilayah.IDWilayahUntuk) ON MRequestMutasiWilayah.NoID=MRequestMutasiWilayahD.IDHeader " & vbCrLf & _
                      " LEFT JOIN MBarang ON MBarang.NoID=MRequestMutasiWilayahD.IDBarang " & vbCrLf & _
                      " WHERE MRequestMutasiWilayah.IsAccPusat=1 AND (MRequestMutasiWilayah.IsSelesai=0 OR MRequestMutasiWilayah.IsSelesai IS NULL) AND MRequestMutasiWilayah.IDWilayahDari=" & DefIDWilayah & " AND (MRequestMutasiWilayahD.Qty*MRequestMutasiWilayahD.Konversi - " & RequestDiSPK() & ">0) AND MBarang.IsActive = 1 And MRequestMutasiWilayah.IsPosted = 1 "
                dsT2 = ExecuteDataset("MPOD", SQL)
                GridControl1.DataSource = dsT2.Tables("MPOD")

                XtraTabPage2.Text = "Request belum di SPK Wilayah"
                XtraTabPage2.PageVisible = True
                XtraTabPage3.PageVisible = False
            ElseIf TableMaster.ToUpper = "MPackingMutasiWilayah".ToUpper Then
                SQL = "SELECT MSPKMutasiWilayahD.NoID, MSPKMutasiWilayah.Kode AS NoSPK, MWilayahUntuk.Nama AS WilayahYgMeminta, MBarang.Kode AS KodeBarang, MAlamat.Nama AS YgMeminta, MBarang.Nama AS NamaBarang, " & _
                      " MWilayahDari.Nama AS WilayahYgDimintai, MSPKMutasiWilayah.*, MSPKMutasiWilayahD.Qty, MSPKMutasiWilayahD.Konversi, MSPKMutasiWilayahD.QtyPcs, MSatuan.Nama AS Satuan, " & _
                      " MSPKMutasiWilayahD.Qty*MSPKMutasiWilayahD.Konversi- " & SPKWDiPacking() & "  AS [Sisa(Pcs)]" & _
                      " FROM MSPKMutasiWilayahD LEFT JOIN MSatuan ON MSatuan.NoID=MSPKMutasiWilayahD.IDSatuan " & vbCrLf & _
                      " LEFT JOIN (((MSPKMutasiWilayah " & _
                      " LEFT JOIN MAlamat ON MAlamat.NoID=MSPKMutasiWilayah.IDPegawai) " & _
                      " LEFT JOIN MWilayah MWilayahDari ON MWilayahDari.NoID=MSPKMutasiWilayah.IDWilayahDari) " & _
                      " LEFT JOIN MWilayah MWilayahUntuk ON MWilayahUntuk.NoID=MSPKMutasiWilayah.IDWilayahUntuk) ON MSPKMutasiWilayah.NoID=MSPKMutasiWilayahD.IDHeader " & vbCrLf & _
                      " LEFT JOIN MBarang ON MBarang.NoID=MSPKMutasiWilayahD.IDBarang " & vbCrLf & _
                      " WHERE (MSPKMutasiWilayah.IsSelesai=0 OR MSPKMutasiWilayah.IsSelesai IS NULL) AND MSPKMutasiWilayah.IDWilayahDari=" & DefIDWilayah & " AND (MSPKMutasiWilayahD.Qty*MSPKMutasiWilayahD.Konversi - " & SPKWDiPacking() & ">0) AND MBarang.IsActive = 1 And MSPKMutasiWilayah.IsPosted = 1 "
                dsT2 = ExecuteDataset("MPOD", SQL)
                GridControl1.DataSource = dsT2.Tables("MPOD")

                XtraTabPage2.Text = "SPK Wilayah belum di Packing"
                XtraTabPage2.PageVisible = True
                XtraTabPage3.PageVisible = False
            ElseIf TableMaster.ToUpper = "MTransferOut".ToUpper Then
                SQL = "SELECT MPackingMutasiWilayahD.NoID, MPackingMutasiWilayah.Kode AS NoPacking, MWilayahUntuk.Nama AS WilayahYgMeminta, MBarang.Kode AS KodeBarang, MAlamat.Nama AS YgMeminta, MBarang.Nama AS NamaBarang, " & _
                      " MWilayahDari.Nama AS WilayahYgDimintai, MPackingMutasiWilayah.*, MPackingMutasiWilayahD.Qty, MPackingMutasiWilayahD.Konversi, MPackingMutasiWilayahD.QtyPcs, MSatuan.Nama AS Satuan, " & _
                      " MPackingMutasiWilayahD.Qty*MPackingMutasiWilayahD.Konversi- " & PackingDiTO() & "  AS [Sisa(Pcs)]" & _
                      " FROM MPackingMutasiWilayahD LEFT JOIN MSatuan ON MSatuan.NoID=MPackingMutasiWilayahD.IDSatuan " & vbCrLf & _
                      " LEFT JOIN (((MPackingMutasiWilayah " & _
                      " LEFT JOIN MAlamat ON MAlamat.NoID=MPackingMutasiWilayah.IDPegawai) " & _
                      " LEFT JOIN MWilayah MWilayahDari ON MWilayahDari.NoID=MPackingMutasiWilayah.IDWilayahDari) " & _
                      " LEFT JOIN MWilayah MWilayahUntuk ON MWilayahUntuk.NoID=MPackingMutasiWilayah.IDWilayahUntuk) ON MPackingMutasiWilayah.NoID=MPackingMutasiWilayahD.IDHeader " & vbCrLf & _
                      " LEFT JOIN MBarang ON MBarang.NoID=MPackingMutasiWilayahD.IDBarang " & vbCrLf & _
                      " WHERE (MPackingMutasiWilayah.IsSelesai=0 OR MPackingMutasiWilayah.IsSelesai IS NULL) AND MPackingMutasiWilayah.IDWilayahDari=" & DefIDWilayah & " AND (MPackingMutasiWilayahD.Qty*MPackingMutasiWilayahD.Konversi - " & PackingDiTO() & ">0) AND MBarang.IsActive = 1 And MPackingMutasiWilayah.IsPosted = 1 "
                dsT2 = ExecuteDataset("MPOD", SQL)
                GridControl1.DataSource = dsT2.Tables("MPOD")

                XtraTabPage2.Text = "Packing Wilayah belum di Transfer Out"
                XtraTabPage2.PageVisible = True
                XtraTabPage3.PageVisible = False
            ElseIf TableMaster.ToUpper = "MTransferIN".ToUpper Then
                SQL = "SELECT MTransferOutD.NoID, MGudang.Nama AS GudangIntransit, MTransferOut.Kode AS NoTransferOut, MWilayahUntuk.Nama AS UntukWilayah, MBarang.Kode AS KodeBarang, MAlamat.Nama AS PenanggungJawab, MBarang.Nama AS NamaBarang, " & _
                      " MWilayahDari.Nama AS DariWilayah, MTransferOut.*, MTransferOutD.Qty, MTransferOutD.Konversi, MTransferOutD.QtyPcs, MSatuan.Nama AS Satuan, " & _
                      " MTransferOutD.Qty*MTransferOutD.Konversi- " & TODiTI() & "  AS [Sisa(Pcs)]" & _
                      " FROM MTransferOutD LEFT JOIN MSatuan ON MSatuan.NoID=MTransferOutD.IDSatuan " & vbCrLf & _
                      " LEFT JOIN ((((MTransferOut " & _
                      " LEFT JOIN MAlamat ON MAlamat.NoID=MTransferOut.IDPegawai) " & _
                      " LEFT JOIN MGudang ON MGudang.NoID=MTransferOut.IDGudangIntransit) " & _
                      " LEFT JOIN MWilayah MWilayahDari ON MWilayahDari.NoID=MTransferOut.IDWilayahDari) " & _
                      " LEFT JOIN MWilayah MWilayahUntuk ON MWilayahUntuk.NoID=MTransferOut.IDWilayahUntuk) ON MTransferOut.NoID=MTransferOutD.IDHeader " & vbCrLf & _
                      " LEFT JOIN MBarang ON MBarang.NoID=MTransferOutD.IDBarang " & vbCrLf & _
                      " WHERE (MTransferOut.IsSelesai=0 OR MTransferOut.IsSelesai IS NULL) AND (MTransferOutD.Qty*MTransferOutD.Konversi - " & TODiTI() & ">0) AND MBarang.IsActive = 1 And MTransferOut.IsPosted = 1 "
                dsT2 = ExecuteDataset("MPOD", SQL)
                GridControl1.DataSource = dsT2.Tables("MPOD")

                XtraTabPage2.Text = "Transfer Out Yang Menggantung"
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
        Try
            Dim row As System.Data.DataRow = view.GetDataRow(GV1.FocusedRowHandle)
            Dim dc As Integer = GV1.FocusedRowHandle
            Dim NoID As Long = NullToLong(row("NoID"))
            Select Case TableMaster.ToUpper
                Case "MBELI", "MLPB", "MPO", "MRETURBELI", "MJUAL", "MRETURJUAL", "MSO", "MSPK", "MPACKING", "MDO", "MMUTASIGUDANG"
                    If Not NullToBool(EksekusiSQlSkalarNew("SELECT isnull(IsPosted,0) FROM " & TableMaster.ToString & " WHERE NoID=" & NoID)) Then
                        If XtraMessageBox.Show(Me, "Yakin mau hapus data " & Text & " dengan Kode " & GV1.GetRowCellValue(GV1.FocusedRowHandle, "Kode"), NamaAplikasi, MessageBoxButtons.YesNo) = Windows.Forms.DialogResult.Yes Then
                            EksekusiSQL("DELETE FROM " & TableMaster & " where NoID= " & NoID.ToString)
                            'TableMaster = "MBeli"
                            'If TableMaster.ToUpper = "MTRANSFEROUT" Or TableMaster.ToUpper = "MTRANSFERIN" Or TableMaster.ToUpper = "MSaldoAwalPersediaan" Then
                            '    EksekusiSQL("DELETE FROM " & TableMaster & "D where IDHEADER= " & NoID.ToString)
                            'Else
                            EksekusiSQL("DELETE FROM " & TableMaster & "D where ID" & TableMaster.Substring(1) & "= " & NoID.ToString)
                            'End If
                            RefreshData()
                            GV1.ClearSelection()
                            GV1.FocusedRowHandle = GV1.LocateByDisplayText(0, GV1.Columns("NoID"), (NoID).ToString("#,##0"))
                            GV1.SelectRow(GV1.FocusedRowHandle)
                        End If
                    Else
                        XtraMessageBox.Show("Data yang sudah diposting tidak boleh dihapus.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Warning)
                    End If
                Case "MREQUESTMUTASIWILAYAH", "MSPKMUTASIWILAYAH", "MPACKINGMUTASIWILAYAH", "MTRANSFEROUT", "MTRANSFERIN", "MPENYESUAIAN", "MPEMAKAIAN", "MSALDOAWALPERSEDIAAN", "MSTOCKOPNAME", "MTRANSFERKODE"
                    If Not NullToBool(EksekusiSQlSkalarNew("SELECT isnull(IsPosted,0) FROM " & TableMaster.ToString & " WHERE NoID=" & NoID)) Then
                        If XtraMessageBox.Show(Me, "Yakin mau hapus data " & Text & " dengan Kode " & GV1.GetRowCellValue(GV1.FocusedRowHandle, "Kode"), NamaAplikasi, MessageBoxButtons.YesNo) = Windows.Forms.DialogResult.Yes Then
                            EksekusiSQL("DELETE FROM " & TableMaster & " where NoID= " & NoID.ToString)
                            'TableMaster = "MBeli"
                            EksekusiSQL("DELETE FROM " & TableMaster & "D where IDHeader= " & NoID.ToString)
                            RefreshData()
                            GV1.ClearSelection()
                            GV1.FocusedRowHandle = GV1.LocateByDisplayText(0, GV1.Columns("NoID"), (NoID).ToString("#,##0"))
                            GV1.SelectRow(GV1.FocusedRowHandle)
                        End If
                    Else
                        XtraMessageBox.Show("Data yang sudah diposting tidak boleh dihapus.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Warning)
                    End If
                Case "MREVISIHARGABELI"
                    If Not NullToBool(EksekusiSQlSkalarNew("SELECT isnull(IsPosted,0) FROM " & TableMaster.ToString & " WHERE NoID=" & NoID)) Then
                        If XtraMessageBox.Show(Me, "Yakin mau hapus data " & Text & " dengan Kode " & GV1.GetRowCellValue(GV1.FocusedRowHandle, "Kode"), NamaAplikasi, MessageBoxButtons.YesNo) = Windows.Forms.DialogResult.Yes Then
                            EksekusiSQL("DELETE FROM " & TableMaster & " where NoID= " & NoID.ToString)
                            RefreshData()
                            GV1.ClearSelection()
                            GV1.FocusedRowHandle = GV1.LocateByDisplayText(0, GV1.Columns("NoID"), (NoID).ToString("#,##0"))
                            GV1.SelectRow(GV1.FocusedRowHandle)
                        End If
                    Else
                        XtraMessageBox.Show("Data yang sudah diposting tidak boleh dihapus.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Warning)
                    End If
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
                    mnPosting.PerformClick()
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
                    mnPosting.PerformClick()
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
        Dim NoID As Long = -1
        Try
            Enabled = False
            dlg.Owner = Me.MdiParent
            dlg.TopMost = True
            dlg.Show()
            Dim jumItem As Integer = GV1.SelectedRowsCount
            For Each i In GV1.GetSelectedRows
                NoID = NullToLong(GV1.GetDataRow(i).Item("NoID"))
                Select Case TableMaster.ToUpper
                    Case "MLPB".ToUpper
                        PostingLPB(NoID)
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
                    Case "MREQUESTMUTASIWILAYAH".ToUpper
                        PostingRequest(NoID)
                    Case "MSPKMUTASIWILAYAH".ToUpper
                        PostingSPKWilayah(NoID)
                    Case "MPACKINGMUTASIWILAYAH".ToUpper
                        PostingPackingWilayah(NoID)
                    Case "MTRANSFEROUT".ToUpper
                        PostingTransferOut(NoID)
                    Case "MTRANSFERIN".ToUpper
                        PostingTransferIN(NoID)
                    Case "MPenyesuaian".ToUpper
                        PostingPenyesuaian(NoID)
                    Case "MPemakaian".ToUpper
                        PostingPemakaian(NoID)
                    Case "MSaldoAwalPersediaan".ToUpper
                        PostingSaldoAwalPersediaan(NoID)
                    Case "MStockOpname".ToUpper
                        PostingStockOpname(NoID)
                    Case "MTransferKode".ToUpper
                        PostingTransferKode(NoID)
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
            dlg.Owner = Me.MdiParent
            dlg.TopMost = True
            dlg.Show()
            Dim jumItem As Integer = GV1.SelectedRowsCount
            For Each i In GV1.GetSelectedRows
                NoID = NullToLong(GV1.GetDataRow(i).Item("NoID"))
                Select Case TableMaster.ToUpper
                    Case "MLPB".ToUpper
                        UnPostingLPB(NoID)
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
                    Case "MREQUESTMUTASIWILAYAH".ToUpper
                        UnPostingRequest(NoID)
                    Case "MSPKMUTASIWILAYAH".ToUpper
                        UnPostingSPKWilayah(NoID)
                    Case "MPACKINGMUTASIWILAYAH".ToUpper
                        UnPostingPackingWilayah(NoID)
                    Case "MTRANSFEROUT".ToUpper
                        UnPostingTransferOut(NoID)
                    Case "MTRANSFERIN".ToUpper
                        UnPostingTransferIN(NoID)
                    Case "MSaldoAwalPenyesuaian".ToUpper
                        UnPostingPenyesuaian(NoID)
                    Case "MPemakaian".ToUpper
                        UnPostingPemakaian(NoID)
                    Case "MPenyesuaian".ToUpper
                        UnPostingPenyesuaian(NoID)
                    Case "MSaldoAwalPersediaan".ToUpper
                        UnPostingSaldoAwalPersediaan(NoID)
                    Case "MStockOpname".ToUpper
                        UnPostingStockOpname(NoID)
                    Case "MTransferKode".ToUpper
                        UnPostingTransferKode(NoID)
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
                    Case "MRequestMutasiWilayah".ToUpper
                        EksekusiSQL("Update MRequestMutasiWilayah Set IsSelesai=1 WHERE NoID=" & NoID)
                    Case "MSPKMutasiWilayah".ToUpper
                        EksekusiSQL("Update MSPKMutasiWilayah Set IsSelesai=1 WHERE NoID=" & NoID)
                    Case "MPackingMutasiWilayah".ToUpper
                        EksekusiSQL("Update MPackingMutasiWilayah Set IsSelesai=1 WHERE NoID=" & NoID)
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
                    Case "MRequestMutasiWilayah".ToUpper
                        EksekusiSQL("Update MRequestMutasiWilayah Set IsSelesai=0 WHERE NoID=" & NoID)
                    Case "MSPKMutasiWilayah".ToUpper
                        EksekusiSQL("Update MSPKMutasiWilayah Set IsSelesai=0 WHERE NoID=" & NoID)
                    Case "MPackingMutasiWilayah".ToUpper
                        EksekusiSQL("Update MPackingMutasiWilayah Set IsSelesai=0 WHERE NoID=" & NoID)
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

    Private Sub SimpleButton1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SimpleButton1.Click
        If NullToBool(Ini.BacaIni("Application", "UseFramework", False)) Then
            Baru()
        Else
            Select Case TableMaster.ToUpper
                Case "MTransferKode".ToUpper
                    Dim view As ColumnView = GC1.FocusedView
                    Dim x As New frmEntriTransferKodeBarang
                    Try
                        x.NoID = -1
                        x.pTipe = frmEntriTransferKodeBarang.pStatus.Baru
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
                Case "MStockOpname".ToUpper
                    Dim view As ColumnView = GC1.FocusedView
                    Dim x As New frmEntriStockOpname
                    Try
                        x.NoID = -1
                        x.pTipe = frmEntriStockOpname.pStatus.Baru
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
                Case "MPEMAKAIAN"
                    Dim view As ColumnView = GC1.FocusedView
                    Dim x As New frmEntriPemakaianBarang
                    Try
                        x.NoID = -1
                        x.pTipe = frmEntriPemakaianBarang.pStatus.Baru
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
                Case "MPenyesuaian".ToUpper
                    Dim view As ColumnView = GC1.FocusedView
                    Dim x As New frmEntriPenyesuaianBarang
                    Try
                        x.NoID = -1
                        x.pTipe = frmEntriPenyesuaianBarang.pStatus.Baru
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
                Case "MSaldoAwalPersediaan".ToUpper
                    Dim view As ColumnView = GC1.FocusedView
                    Dim x As New frmEntriSaldoAwalPersediaan
                    Try
                        x.NoID = -1
                        x.pTipe = frmEntriSaldoAwalPersediaan.pStatus.Baru
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
                Case "MTRANSFERIN"
                    Dim view As ColumnView = GC1.FocusedView
                    Dim x As New frmEntriTransferINMutasiWilayah
                    Try
                        x.NoID = -1
                        x.pTipe = frmEntriTransferINMutasiWilayah.pStatus.Baru
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
                Case "MTRANSFEROUT"
                    Dim view As ColumnView = GC1.FocusedView
                    Dim x As New frmEntriTransferOutMutasiWilayah
                    Try
                        x.NoID = -1
                        x.pTipe = frmEntriTransferOutMutasiWilayah.pStatus.Baru
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
                Case "MPACKINGMUTASIWILAYAH"
                    Dim view As ColumnView = GC1.FocusedView
                    Dim x As New frmEntriPackingMutasiWilayah
                    Try
                        x.NoID = -1
                        x.pTipe = frmEntriPackingMutasiWilayah.pStatus.Baru
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
                Case "MSPKMUTASIWILAYAH"
                    Dim view As ColumnView = GC1.FocusedView
                    Dim x As New frmEntriSPKMutasiWilayah
                    Try
                        x.NoID = -1
                        x.pTipe = frmEntriSPKMutasiWilayah.pStatus.Baru
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
                Case "MREQUESTMUTASIWILAYAH"
                    Dim view As ColumnView = GC1.FocusedView
                    Dim x As New frmEntriRequestMutasiWilayah
                    Try
                        x.NoID = -1
                        x.pTipe = frmEntriRequestMutasiWilayah.pStatus.Baru
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
                Case "MBELI"
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

                Case "MRETURBELI"
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
                Case "MRevisiHargaBeli".ToUpper
                    Dim view As ColumnView = GC1.FocusedView
                    Dim x As New frmEntriRevisiBeliD
                    Try
                        x.NoID = -1
                        x.IsNew = True
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
        SimpleButton8.PerformClick()
    End Sub

    Private Sub mnHasilPosting_ItemClick(ByVal sender As Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles mnHasilPosting.ItemClick
        Try
            clsPostingKartuStok.LihatHasilPosting(NullToLong(GV1.GetRowCellValue(GV1.FocusedRowHandle, "NoID")), TableMaster)
        Catch ex As Exception
            XtraMessageBox.Show("Info : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
        End Try
    End Sub

    Private Sub mnAccPusat_ItemClick(ByVal sender As Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles mnAccPusat.ItemClick
        Dim dlg As New WaitDialogForm("Sedang Proses Acc Pusat...", "Mohon Tunggu Sebentar.")
        Dim NoID As Long = -1
        Try
            Enabled = False
            dlg.Owner = Me.MdiParent
            dlg.TopMost = True
            dlg.Show()
            Dim jumItem As Integer = GV1.SelectedRowsCount
            For Each i In GV1.GetSelectedRows
                NoID = NullToLong(GV1.GetDataRow(i).Item("NoID"))
                Select Case TableMaster.ToUpper
                    Case "MREQUESTMUTASIWILAYAH".ToUpper
                        RequestDiAccPusat(NoID)
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
    Private Sub mnUnAccPusat_ItemClick(ByVal sender As Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles mnUnAccPusat.ItemClick
        Dim dlg As New WaitDialogForm("Sedang Proses UnAcc Pusat...", "Mohon Tunggu Sebentar.")
        Dim NoID As Long = -1
        Try
            Enabled = False
            dlg.Owner = Me.MdiParent
            dlg.TopMost = True
            dlg.Show()
            Dim jumItem As Integer = GV1.SelectedRowsCount
            For Each i In GV1.GetSelectedRows
                NoID = NullToLong(GV1.GetDataRow(i).Item("NoID"))
                Select Case TableMaster.ToUpper
                    Case "MREQUESTMUTASIWILAYAH".ToUpper
                        UnAccPusatRequest(NoID)
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

    Private Sub mnUnLockStockOpname_ItemClick(ByVal sender As Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles mnunLockStockOpname.ItemClick
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