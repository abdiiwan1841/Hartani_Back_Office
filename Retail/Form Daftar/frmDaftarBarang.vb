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
Imports System.Data.OleDb

Public Class frmDaftarBarang
    Public TableNameD As String = ""
    Public TableMaster As String = ""
    Public NoID As Long = -1
    Dim oda2 As SqlDataAdapter
    Dim ds As New DataSet
    Dim BS As New BindingSource
    Dim frmImage As New frmShowImage

    Dim repckedit As New RepositoryItemCheckEdit
    Dim repdateedit As New RepositoryItemDateEdit
    Dim reptextedit As New RepositoryItemTextEdit
    Dim reppicedit As New RepositoryItemPictureEdit
    Public ShowNoID As Boolean = False
    Public DirectNoID As Long = -1
    Dim frmlookup As frmLookUpPembelianDetil
    Dim frmSaldoGudang As frmSaldoPerGudang
    Public IsShowStock As Boolean = False
    Public FocusRowColumn As Long = -1

    Private Sub frmDaftarBarang_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Activated
        If ShowNoID Then
            RefreshData()
            If XtraTabControl1.SelectedTabPageIndex = 0 Then
                GV1.ClearSelection()
                GV1.FocusedRowHandle = GV1.LocateByDisplayText(0, GV1.Columns("NoID"), (DirectNoID).ToString("#,##0"))
                If GV1.FocusedRowHandle = 0 Then
                    If FocusRowColumn >= 1 Then
                        GV1.FocusedRowHandle = FocusRowColumn
                    End If
                End If
                GV1.SelectRow(GV1.FocusedRowHandle)
                FocusRowColumn = -1
            ElseIf XtraTabControl1.SelectedTabPageIndex = 1 Then
                GridView1.ClearSelection()
                GridView1.FocusedRowHandle = GridView1.LocateByDisplayText(0, GridView1.Columns("NoID"), (DirectNoID).ToString("#,##0"))
                GridView1.SelectRow(GridView1.FocusedRowHandle)

            Else
                LayoutView1.ClearSelection()
                LayoutView1.FocusedRowHandle = LayoutView1.LocateByDisplayText(0, LayoutView1.Columns("NoID"), (DirectNoID).ToString("#,##0"))
                LayoutView1.SelectRow(LayoutView1.FocusedRowHandle)
            End If
            ShowNoID = False
            'If KirimDataKassa() Then
            '    XtraMessageBox.Show("Pengiriman berhasil.", NamaAplikasi, MessageBoxButtons.OK)
            'End If
        End If
        txtSearch.Focus()
    End Sub

    Private Sub frmDaftar_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing

    End Sub
    Sub refreshLookUp()
        Dim ds As New DataSet
        Dim strsql As String
        Try
            strsql = "SELECT NoID, Kode, Nama FROM MAlamat WHERE IsActive=1 AND IsSupplier=1"
            ds = ExecuteDataset("MasterSupplier", strsql)
            txtSupplier.Properties.DataSource = ds.Tables("MasterSupplier")
            If System.IO.File.Exists(FolderLayouts & Me.Name & gvSupplier.Name & ".xml") Then
                gvSupplier.RestoreLayoutFromXml(FolderLayouts & Me.Name & gvSupplier.Name & ".xml")
            End If

            strsql = "SELECT  NoID, Kode, Nama FROM MKategori "
            ds = ExecuteDataset("masterKategori", strsql)
            txtKategori.Properties.DataSource = ds.Tables("masterKategori")
            txtKategori.Properties.ValueMember = "NoID"
            txtKategori.Properties.DisplayMember = "Nama"
            If System.IO.File.Exists(FolderLayouts & Me.Name & gvKategori.Name & ".xml") Then
                gvKategori.RestoreLayoutFromXml(FolderLayouts & Me.Name & gvKategori.Name & ".xml")
            End If

            strsql = "SELECT  NoID, Kode, Nama FROM MMerk "
            ds = ExecuteDataset("masterMerk", strsql)
            txtMerk.Properties.DataSource = ds.Tables("masterMerk")
            txtMerk.Properties.ValueMember = "NoID"
            txtMerk.Properties.DisplayMember = "Nama"
            If System.IO.File.Exists(FolderLayouts & Me.Name & gvMerk.Name & ".xml") Then
                gvMerk.RestoreLayoutFromXml(FolderLayouts & Me.Name & gvMerk.Name & ".xml")
            End If
        Catch ex As Exception
            XtraMessageBox.Show("Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            ds.Dispose()
        End Try
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
            TglDari.DateTime = TanggalSystem
            txtMerk.Enabled = False
            txtKategori.Enabled = False
            txtSupplier.Enabled = False
            TambahMenu()
            refreshLookUp()
            RefreshData()
            RestoreLayout()
            Me.lbDaftar.Text = Me.Text
            FungsiControl.SetForm(Me)
            If DefTipeStock = DefTipeStock_.Penuh AndAlso IsSupervisor Then
                mnQtyMin.Visibility = DevExpress.XtraBars.BarItemVisibility.Always
                mnQtyMax.Visibility = DevExpress.XtraBars.BarItemVisibility.Always
            Else
                mnQtyMin.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
                mnQtyMax.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
            End If
            If IsAccMutasi Then
                mnSetAkun.Visibility = DevExpress.XtraBars.BarItemVisibility.Always
            Else
                mnSetAkun.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
            End If
        Catch ex As Exception
            XtraMessageBox.Show(ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            Windows.Forms.Cursor.Current = Cursors.Default
            dlg.Close()
            dlg.Dispose()
        End Try
    End Sub
    Private Sub TambahMenu()
        Dim SQL As String = ""
        Dim ds As New DataSet
        Dim mnButton As DevExpress.XtraBars.BarButtonItem = Nothing
        Try
            SQL = "SELECT * FROM MPOS WHERE IsActive=1"
            ds = ExecuteDataset("MPOS", SQL)
            For i As Integer = 0 To ds.Tables("MPOS").Rows.Count - 1
                mnButton = New DevExpress.XtraBars.BarButtonItem
                mnButton.Name = "mnButton" & NullToStr(ds.Tables("MPOS").Rows(i).Item("Kode"))
                mnButton.Caption = "Kirim Data ke " & NullToStr(ds.Tables("MPOS").Rows(i).Item("Kode"))
                mnButton.Tag = NullToLong(ds.Tables("MPOS").Rows(i).Item("NoID"))
                AddHandler mnButton.ItemClick, AddressOf mnKirimBarangKeKassack_ItemClick
                mnSubKirimDataMaster.ItemLinks.Add(mnButton, IIf(i = 0, True, False))
            Next
        Catch ex As Exception
            XtraMessageBox.Show("Info Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
        Finally
            If Not ds Is Nothing Then
                ds.Dispose()
            End If
        End Try
    End Sub
    Private Sub RestoreLayout()
        'If System.IO.File.Exists(FolderLayouts & Me.Name & IDUserAktif & ".xml") Then
        '    GV1.RestoreLayoutFromXml(FolderLayouts & Me.Name & IDUserAktif & ".xml")
        'Else
        If System.IO.File.Exists(FolderLayouts & Me.Name & DefTipeStock.ToString & ".xml") Then
            GV1.RestoreLayoutFromXml(FolderLayouts & Me.Name & DefTipeStock.ToString & ".xml")
        Else
            If System.IO.File.Exists(FolderLayouts & Me.Name & ".xml") Then
                GV1.RestoreLayoutFromXml(FolderLayouts & Me.Name & ".xml")
            End If
        End If
        'End If
        'If System.IO.File.Exists(FolderLayouts & Me.Name & GridView1.Name & IDUserAktif & ".xml") Then
        '    GridView1.RestoreLayoutFromXml(FolderLayouts & Me.Name & GridView1.Name & IDUserAktif & ".xml")
        'Else
        If System.IO.File.Exists(FolderLayouts & Me.Name & GridView1.Name & DefTipeStock.ToString & ".xml") Then
            GridView1.RestoreLayoutFromXml(FolderLayouts & Me.Name & GridView1.Name & DefTipeStock.ToString & ".xml")
        Else
            If System.IO.File.Exists(FolderLayouts & Me.Name & GridView1.Name & ".xml") Then
                GridView1.RestoreLayoutFromXml(FolderLayouts & Me.Name & GridView1.Name & ".xml")
            End If
        End If
        'End If
        'If System.IO.File.Exists(FolderLayouts & Me.Name & LayoutView1.Name & IDUserAktif & ".xml") Then
        '    LayoutView1.RestoreLayoutFromXml(FolderLayouts & Me.Name & LayoutView1.Name & IDUserAktif & ".xml")
        'Else
        If System.IO.File.Exists(FolderLayouts & Me.Name & LayoutView1.Name & DefTipeStock.ToString & ".xml") Then
            LayoutView1.RestoreLayoutFromXml(FolderLayouts & Me.Name & LayoutView1.Name & DefTipeStock.ToString & ".xml")
        Else
            If System.IO.File.Exists(FolderLayouts & Me.Name & LayoutView1.Name & ".xml") Then
                LayoutView1.RestoreLayoutFromXml(FolderLayouts & Me.Name & LayoutView1.Name & ".xml")
            End If
        End If
        'End If
        If System.IO.File.Exists(FolderLayouts & Me.Name & LayoutControl1.Name & ".xml") Then
            LayoutControl1.RestoreLayoutFromXml(FolderLayouts & Me.Name & LayoutControl1.Name & ".xml")
        End If

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
                    Case "date"
                        .Columns(i).DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime
                        .Columns(i).DisplayFormat.FormatString = "dd-MM-yyyy"
                    Case "datetime"
                        .Columns(i).DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime
                        .Columns(i).DisplayFormat.FormatString = "dd-MM-yyyy HH:mm"
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
                    Case "date"
                        .Columns(i).DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime
                        .Columns(i).DisplayFormat.FormatString = "dd-MM-yyyy"
                    Case "datetime"
                        .Columns(i).DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime
                        .Columns(i).DisplayFormat.FormatString = "dd-MM-yyyy HH:mm"
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
        Dim x As New clsBarang
        x = New clsBarang
        x.FormName = "EntriBarang"
        x.isNew = False
        frmImage.TopMost = False
        frmImage.Close()
        frmImage.Dispose()
        x.NoID = NullToLong(GV1.GetRowCellValue(GV1.FocusedRowHandle, "NoID"))
        x.TipeEntri = clsBarang.TipeEntry.All
        x.MdiParent = Me.MdiParent
        x.WindowState = FormWindowState.Normal
        x.FocusRowColumn = NullToLong(GV1.FocusedRowHandle)
        x.Show()
        x.Focus()
    End Sub

    Private Sub SimpleButton3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SimpleButton3.Click
        Hapus()
    End Sub

    Private Sub SimpleButton1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SimpleButton1.Click
        Dim x As New clsBarang
        x.FormName = "EntriBarang"
        x.isNew = True
        frmImage.TopMost = False
        frmImage.Close()
        frmImage.Dispose()
        x.MdiParent = Me.MdiParent
        x.WindowState = FormWindowState.Normal

        x.Show()
        x.Focus()

        'If x.ShowDialog(me) = Windows.Forms.DialogResult.OK Then
        '    RefreshData()
        '    GV1.FocusedRowHandle = GV1.LocateByDisplayText(0, GV1.Columns("NoID"), x.NoID.ToString("#,##0"))
        'End If
        'x.Dispose()
    End Sub
    Sub RefreshData()
        Dim dlg As DevExpress.Utils.WaitDialogForm = Nothing
        Dim odsT2 As New DataSet
        Dim strsql As String
        Try
            Dim curentcursor As Cursor = Windows.Forms.Cursor.Current
            Windows.Forms.Cursor.Current = Cursors.WaitCursor
            dlg = New DevExpress.Utils.WaitDialogForm(String.Format("Sedang merefresh data.{0}MOHON TUNGGU ...", vbCrLf), Application.ProductName)
            dlg.TopMost = False
            dlg.Show()
            Windows.Forms.Cursor.Current = Cursors.WaitCursor

            'strsql = "SELECT MBarang.NoID, MBarang.IsNonStock AS TidakDikontrol, MBarang.Kode, MBarang.Nama, MBarang.Barcode, MSatuan.Nama AS Satuan, mjenisbarang.Nama AS Jenis, MBarang.Alias AS KodeAlias, MBarang.NamaAlias, " & vbCrLf
            'strsql &= " MKategori.Nama AS Kategori, MBarang.HargaPasar, MBarang.HargaJualA, MBarang.HargaJualB, MBarang.HargaJualC, " & vbCrLf
            'strsql &= " MBarang.HargaJualD, MBarang.HargaJualE, MBarang.KodeDuz, MBarang.Ctn_Duz, MBarang.CtnPcs, MBarang.IsActive, MBarang.IsNewItem AS NewItem, " & vbCrLf
            'strsql &= " MBarang.HargaJualF, MBarang.HPP, " & vbCrLf
            'If mnCkShowGallery.Checked Then
            '    strsql &= " MBarang.Foto, " & vbCrLf
            'End If
            'strsql &= " MSupplier1.Nama AS Supplier1, " & vbCrLf
            'strsql &= " MSupplier2.Nama AS Supplier2, " & vbCrLf
            'strsql &= " MSupplier3.Nama AS Supplier3, " & vbCrLf
            'strsql &= " MSupplier4.Nama AS Supplier4, " & vbCrLf
            'strsql &= " MSupplier5.Nama AS Supplier5, " & vbCrLf
            'strsql &= " (SELECT SUM((MKartuStok.QtyMasuk*MKartuStok.Konversi)-(MKartuStok.QtyKeluar*MKartuStok.Konversi)) FROM MKartuStok WHERE MKartuStok.Tanggal<'" & DateAdd(DateInterval.Day, 1, TanggalSystem).ToString("yyyy-MM-dd") & "' AND MKartuStok.IDBarang=MBarang.NoID) AS StokAkhir, " & vbCrLf
            'strsql &= " (SELECT SUM((MKartuStok.QtyMasuk*MKartuStok.Konversi)-(MKartuStok.QtyKeluar*MKartuStok.Konversi)) FROM MKartuStok LEFT JOIN MGudang ON MGudang.NoID=MKartuStok.IDGudang WHERE MGudang.IsBS=1 AND MKartuStok.Tanggal<'" & DateAdd(DateInterval.Day, 1, TanggalSystem).ToString("yyyy-MM-dd") & "' AND MKartuStok.IDBarang=MBarang.NoID) AS StokTdkSiapJual, " & vbCrLf
            'strsql &= " (SELECT SUM((MKartuStok.QtyMasuk*MKartuStok.Konversi)-(MKartuStok.QtyKeluar*MKartuStok.Konversi)) FROM MKartuStok LEFT JOIN MGudang ON MGudang.NoID=MKartuStok.IDGudang WHERE (MGudang.IsBS=0 OR MGudang.IsBS Is Null) AND MKartuStok.Tanggal<'" & DateAdd(DateInterval.Day, 1, TanggalSystem).ToString("yyyy-MM-dd") & "' AND MKartuStok.IDBarang=MBarang.NoID) AS StokSiapJual " & vbCrLf
            'strsql &= " FROM MBarang LEFT OUTER JOIN" & vbCrLf
            'strsql &= " MSatuan ON MBarang.IDSatuan = MSatuan.NoID LEFT OUTER JOIN" & vbCrLf
            'strsql &= " MAlamat MSupplier1 ON MBarang.IDSupplier1 = MSupplier1.NoID LEFT OUTER JOIN" & vbCrLf
            'strsql &= " MAlamat MSupplier2 ON MBarang.IDSupplier2 = MSupplier2.NoID LEFT OUTER JOIN" & vbCrLf
            'strsql &= " MAlamat MSupplier3 ON MBarang.IDSupplier3 = MSupplier3.NoID LEFT OUTER JOIN" & vbCrLf
            'strsql &= " MAlamat MSupplier4 ON MBarang.IDSupplier4 = MSupplier4.NoID LEFT OUTER JOIN" & vbCrLf
            'strsql &= " MAlamat MSupplier5 ON MBarang.IDSupplier5 = MSupplier5.NoID LEFT OUTER JOIN" & vbCrLf
            'strsql &= " MKategori ON MBarang.IDKategori = MKategori.NoID LEFT OUTER JOIN" & vbCrLf
            'strsql &= " mjenisbarang ON MBarang.IDJenis = mjenisbarang.NoID" & vbCrLf
            'If Not ckAll.Checked Then
            '    strsql &= " WHERE MBarang.IsActive=1 "
            'End If

            '#REGION SPBU99
            ''SPBU 99
            'strsql = "SELECT MBarang.NoID, MBarang.Kode, MBarang.Nama, MBarangD.Barcode, MSatuan.Nama AS Satuan," & vbCrLf
            'strsql &= " MKategori.Nama AS Kategori, MBarang.IsFamilyGroup,MBarang.HargaFamily," & vbCrLf
            'strsql &= "  MBarang.Qty as QtyFamily,MBarang.TanggalDariFamily,MBarang.TanggalSampaiFamily," & vbCrLf
            'strsql &= "  MBarang.HargaBeli,MBarang.CtnPcs as IsiKarton,MBarangD.HargaJualA as HargaJual,MBarangD.IsGrosir," & vbCrLf
            'strsql &= "  MBarangD.Qty3 as QtyGrosir,MBarangD.Harga3 as HargaGrosir,MBarangD.IsActive,MBarangD.LastUpdated " & vbCrLf
            'strsql &= " FROM MBarangD inner join Mbarang On MbarangD.IDBarang=MBarang.NoID LEFT OUTER JOIN" & vbCrLf
            'strsql &= " MSatuan ON MBarangD.IDSatuan = MSatuan.NoID LEFT OUTER JOIN" & vbCrLf
            'strsql &= " MKategori ON MBarang.IDKategori = MKategori.NoID  " & vbCrLf
            '#END REGION SPBU99

            'PLANET SWALAYAN
            'strsql = "SELECT MBarangD.NoID IDDetil,MBarang.NoID, MBarang.Kode, MBarang.Nama + ' ' + MBarangD.Varian AS Nama, MBarangD.Barcode, MSatuan.Nama AS Satuan," & vbCrLf & _
            '         " MKategori.Nama AS Kategori, MBarangD.HargaJual, MBarangD.HargaJual2, MBarangD.HargaJual3, " & vbCrLf & _
            '         " MBarangD.Qty1, MBarangD.Qty2, MBarangD.Qty3," & vbCrLf & _
            '         " MBarang.BKP,MBarang.IsPoin,MBarang.IDPoinSupplier," & vbCrLf & _
            '         " MBarang.HargaBeli,MBarang.HargaBeliPcs,MBarang.DiscBeli1,MBarang.DiscBeli2,MBarang.DiscBeli3,MBarang.CtnPcs as IsiKarton, " & vbCrLf & _
            '         " FROM MBarang left join MbarangD On MbarangD.IDBarang=MBarang.NoID LEFT OUTER JOIN" & vbCrLf & _
            '         " MSatuan ON MBarangD.IDSatuan = MSatuan.NoID LEFT OUTER JOIN" & vbCrLf & _
            '         " MKategori ON MBarang.IDKategori = MKategori.NoID  " & vbCrLf
            'strsql &= " MBarang.HargaJualD, MBarang.HargaJualE, MBarang.KodeDuz, MBarang.Ctn_Duz, MBarang.CtnPcs, MBarang.IsActive, MBarang.IsNewItem AS NewItem, " & vbCrLf
            'strsql &= " MBarang.HargaJualF, MBarang.HPP, " & vbCrLf
            'If mnCkShowGallery.Checked Then
            '    strsql &= " MBarang.Foto, " & vbCrLf
            'End If
            'strsql &= " MSupplier1.Nama AS Supplier1, " & vbCrLf
            'strsql &= " MSupplier2.Nama AS Supplier2, " & vbCrLf
            'strsql &= " MSupplier3.Nama AS Supplier3, " & vbCrLf
            'strsql &= " MSupplier4.Nama AS Supplier4, " & vbCrLf
            'strsql &= " MSupplier5.Nama AS Supplier5, " & vbCrLf
            'strsql &= " (SELECT SUM((MKartuStok.QtyMasuk*MKartuStok.Konversi)-(MKartuStok.QtyKeluar*MKartuStok.Konversi)) FROM MKartuStok WHERE MKartuStok.Tanggal<'" & DateAdd(DateInterval.Day, 1, TanggalSystem).ToString("yyyy-MM-dd") & "' AND MKartuStok.IDBarang=MBarang.NoID) AS StokAkhir, " & vbCrLf
            'strsql &= " (SELECT SUM((MKartuStok.QtyMasuk*MKartuStok.Konversi)-(MKartuStok.QtyKeluar*MKartuStok.Konversi)) FROM MKartuStok LEFT JOIN MGudang ON MGudang.NoID=MKartuStok.IDGudang WHERE MGudang.IsBS=1 AND MKartuStok.Tanggal<'" & DateAdd(DateInterval.Day, 1, TanggalSystem).ToString("yyyy-MM-dd") & "' AND MKartuStok.IDBarang=MBarang.NoID) AS StokTdkSiapJual, " & vbCrLf
            'strsql &= " (SELECT SUM((MKartuStok.QtyMasuk*MKartuStok.Konversi)-(MKartuStok.QtyKeluar*MKartuStok.Konversi)) FROM MKartuStok LEFT JOIN MGudang ON MGudang.NoID=MKartuStok.IDGudang WHERE (MGudang.IsBS=0 OR MGudang.IsBS Is Null) AND MKartuStok.Tanggal<'" & DateAdd(DateInterval.Day, 1, TanggalSystem).ToString("yyyy-MM-dd") & "' AND MKartuStok.IDBarang=MBarang.NoID) AS StokSiapJual " & vbCrLf
            'strsql &= " FROM MBarang LEFT OUTER JOIN" & vbCrLf
            'strsql &= " MSatuan ON MBarang.IDSatuan = MSatuan.NoID LEFT OUTER JOIN" & vbCrLf
            'strsql &= " MAlamat MSupplier1 ON MBarang.IDSupplier1 = MSupplier1.NoID LEFT OUTER JOIN" & vbCrLf
            'strsql &= " MAlamat MSupplier2 ON MBarang.IDSupplier2 = MSupplier2.NoID LEFT OUTER JOIN" & vbCrLf
            'strsql &= " MAlamat MSupplier3 ON MBarang.IDSupplier3 = MSupplier3.NoID LEFT OUTER JOIN" & vbCrLf
            'strsql &= " MAlamat MSupplier4 ON MBarang.IDSupplier4 = MSupplier4.NoID LEFT OUTER JOIN" & vbCrLf
            'strsql &= " MAlamat MSupplier5 ON MBarang.IDSupplier5 = MSupplier5.NoID LEFT OUTER JOIN" & vbCrLf
            'strsql &= " MKategori ON MBarang.IDKategori = MKategori.NoID LEFT OUTER JOIN" & vbCrLf
            'strsql &= " mjenisbarang ON MBarang.IDJenis = mjenisbarang.NoID" & vbCrLf

            RefreshDataBarang("")
            If mnCkShowGallery.Checked Then
                GridControl2.DataSource = BS.DataSource
                If System.IO.File.Exists(FolderLayouts & Me.Name & LayoutView1.Name & IDUserAktif & ".xml") Then
                    LayoutView1.RestoreLayoutFromXml(FolderLayouts & Me.Name & LayoutView1.Name & IDUserAktif & ".xml")
                End If
                With LayoutView1
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
                            Case "date"
                                .Columns(i).DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime
                                .Columns(i).DisplayFormat.FormatString = "dd-MM-yyyy"
                            Case "datetime"
                                .Columns(i).DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime
                                .Columns(i).DisplayFormat.FormatString = "dd-MM-yyyy HH:mm"
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
            End If

            If DefTipeStock = DefTipeStock_.Penuh Then
                strsql = "SELECT MBarang.NoID, MBarang.Kode, MBarang.Nama, MBarang.Barcode, MSatuan.Nama AS Satuan, mjenisbarang.Nama AS Jenis, MBarang.KodeAlias AS KodeAlias, " & vbCrLf
                strsql &= " MKategori.Nama AS Kategori, MBarang.HargaJual," & vbCrLf
                strsql &= " MBarang.KodeDuz, MBarang.CtnPcs, MBarang.IsActive, MBarang.IsNewItem AS NewItem, " & vbCrLf
                strsql &= " MBarang.HPP, " & vbCrLf
                'If mnCkShowGallery.Checked Then
                '    strsql &= " MBarang.Foto, " & vbCrLf
                'End If
                strsql &= " MSupplier1.Nama AS Supplier1, " & vbCrLf
                strsql &= " MSupplier2.Nama AS Supplier2, " & vbCrLf
                strsql &= " MSupplier3.Nama AS Supplier3, " & vbCrLf
                strsql &= " MSupplier4.Nama AS Supplier4, " & vbCrLf
                strsql &= " MSupplier5.Nama AS Supplier5 " & vbCrLf
                'strsql &= " (SELECT SUM((MKartuStok.QtyMasuk*MKartuStok.Konversi)-(MKartuStok.QtyKeluar*MKartuStok.Konversi)) FROM MKartuStok WHERE MKartuStok.Tanggal<'" & DateAdd(DateInterval.Day, 1, TanggalSystem).ToString("yyyy-MM-dd") & "' AND MKartuStok.IDBarang=MBarang.NoID) AS StokAkhir, " & vbCrLf
                'strsql &= " (SELECT SUM((MKartuStok.QtyMasuk*MKartuStok.Konversi)-(MKartuStok.QtyKeluar*MKartuStok.Konversi)) FROM MKartuStok LEFT JOIN MGudang ON MGudang.NoID=MKartuStok.IDGudang WHERE MGudang.IsBS=1 AND MKartuStok.Tanggal<'" & DateAdd(DateInterval.Day, 1, TanggalSystem).ToString("yyyy-MM-dd") & "' AND MKartuStok.IDBarang=MBarang.NoID) AS StokTdkSiapJual, " & vbCrLf
                'strsql &= " (SELECT SUM((MKartuStok.QtyMasuk*MKartuStok.Konversi)-(MKartuStok.QtyKeluar*MKartuStok.Konversi)) FROM MKartuStok LEFT JOIN MGudang ON MGudang.NoID=MKartuStok.IDGudang WHERE (MGudang.IsBS=0 OR MGudang.IsBS Is Null) AND MKartuStok.Tanggal<'" & DateAdd(DateInterval.Day, 1, TanggalSystem).ToString("yyyy-MM-dd") & "' AND MKartuStok.IDBarang=MBarang.NoID) AS StokSiapJual " & vbCrLf
                strsql &= " FROM MBarang LEFT OUTER JOIN" & vbCrLf
                strsql &= " MSatuan ON MBarang.IDSatuan = MSatuan.NoID LEFT OUTER JOIN" & vbCrLf
                strsql &= " MAlamat MSupplier1 ON MBarang.IDSupplier1 = MSupplier1.NoID LEFT OUTER JOIN" & vbCrLf
                strsql &= " MAlamat MSupplier2 ON MBarang.IDSupplier2 = MSupplier2.NoID LEFT OUTER JOIN" & vbCrLf
                strsql &= " MAlamat MSupplier3 ON MBarang.IDSupplier3 = MSupplier3.NoID LEFT OUTER JOIN" & vbCrLf
                strsql &= " MAlamat MSupplier4 ON MBarang.IDSupplier4 = MSupplier4.NoID LEFT OUTER JOIN" & vbCrLf
                strsql &= " MAlamat MSupplier5 ON MBarang.IDSupplier5 = MSupplier5.NoID LEFT OUTER JOIN" & vbCrLf
                strsql &= " MKategori ON MBarang.IDKategori = MKategori.NoID LEFT OUTER JOIN" & vbCrLf
                strsql &= " mjenisbarang ON MBarang.IDJenis = mjenisbarang.NoID" & vbCrLf
                strsql &= " INNER JOIN (MBeliD LEFT JOIN MBeli ON MBeli.NoID=MBeliD.IDBeli) ON MBarang.NoID=MBeliD.IDBarang "
                If TglDari.Enabled Then
                    strsql &= " WHERE (MBeli.Tanggal>='" & Format(TglDari.DateTime, "yyyy-MM-dd") & "' AND MBeli.Tanggal<'" & Format(DateAdd(DateInterval.Day, 1, TglDari.DateTime), "yyyy-MM-dd") & "')"
                Else
                    strsql &= " WHERE 1=1"
                End If

                If Not ckAll.Checked Then
                    strsql &= " AND MBarang.IsActive=1 "
                End If
                If txtSupplier.Enabled Then
                    strsql &= " AND (MBarang.IDSupplier1= " & NullToLong(txtSupplier.EditValue) & " or mbarang.IDSupplier5=" & NullToLong(txtSupplier.EditorTypeName) & ")"
                End If
                If txtKategori.Enabled Then
                    strsql &= " AND MBarang.IDKategori= " & NullToLong(txtKategori.EditValue) & " "
                End If
                If txtMerk.Enabled Then
                    strsql &= " AND MBarang.IDMerk= " & NullToLong(txtMerk.EditValue) & ""
                End If
                odsT2 = ExecuteDataset("Tbl", strsql)
                GridControl1.DataSource = odsT2.Tables("Tbl")
                XtraTabPage2.PageVisible = True
            Else
                XtraTabPage2.PageVisible = False
            End If

            Application.DoEvents()
            GV1.ShowFindPanel()
            GridView1.ShowFindPanel()
            'FocusRowColumn = -1
        Catch ex As Exception
            XtraMessageBox.Show(ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            Windows.Forms.Cursor.Current = Cursors.Default
            odsT2.Dispose()
            dlg.Close()
            dlg.Dispose()
        End Try
    End Sub
    Sub RefreshDataBarang(ByVal filter As String)
        Dim dlg As DevExpress.Utils.WaitDialogForm = Nothing
        Dim odsT2 As New DataSet
        Dim strsql As String
        Dim TimeStart As DateTime = Nothing
        Try
            dlg = New DevExpress.Utils.WaitDialogForm(String.Format("Sedang merefresh data.{0}MOHON TUNGGU ...", vbCrLf), Application.ProductName)
            dlg.TopMost = True
            dlg.Show()

            strsql = "SELECT MBarang.DiscMemberProsen2 AS [Disc Member (%)], MBarang.DiscPDPMember, MBarangD.HargaJual-IsNull(MBarang.DiscPDPMember,0) AS HargaPDPMember, MBarang.DiscPDP, MBarangD.HargaJual-IsNull(MBarang.DiscPDP,0) AS HargaPDP, MBarang.DiscMemberRp2 AS NilaiDiskonMember, MBarang.TglDariDiskon2 AS TglDariDiscMember, MBarang.TglSampaiDiskon2 AS TglSampaiDiscMember, MBarang.HargaNettoMember, MBarang.IsNewItem AS ProdukBaru, MUser.Nama AS UserEdit, MBarang.TerakhirUpdate, MBarangD.NoID IDDetil,MBarang.NoID, MBarang.Kode, rtrim(MBarang.Nama + ' ' + isnull(MBarangD.Varian,'')) AS Nama, MBarangD.Barcode, MSatuan.Nama AS Satuan," & vbCrLf & _
                     " MKategori.Nama AS Kategori,MBarang.StockMin,MBarang.StockMax, MBarangD.HargaJual, MBarangD.HargaJual2, MBarangD.HargaJual3, " & vbCrLf & _
                     " MBarangD.Qty1, MBarangD.Qty2, MBarangD.Qty3," & vbCrLf & _
                     " MBarang.BKP,MBarang.IsPoin,MBarang.IDPoinSupplier," & vbCrLf & _
                     " MBarang.HargaBeli,MBarang.HargaBeliPcs,MBarang.DiscBeli1,MBarang.DiscBeli2,MBarang.DiscBeli3,MBarang.CtnPcs as IsiKarton," & vbCrLf & _
                     " MBarangD.IsActive,MBarangD.LastUpdated,MBarangD.PromoDiskonJual,MBarangD.NilaiDiskon,MBarangD.HargaNetto,MBarangD.TglDariDiskon as DariTgl,MBarangD.TglSampaiDiskon as SampaiTgl, " & vbCrLf & _
                     " MMerk.Nama as Merk,MSupplier1.Kode + ' - ' + MSupplier1.Nama AS Supplier1, MSupplier2.Kode + ' - ' + MSupplier2.Nama AS Supplier2, MSupplier3.Kode + ' - ' + MSupplier3.Nama AS Supplier3, MSupplier4.Kode + ' - ' + MSupplier4.Nama AS Supplier4, MSupplier5.Kode + ' - ' + MSupplier5.Nama AS Supplier5, " & vbCrLf & _
                     " MAkunPersediaan.Nama AS AkunPersediaan, MAkunHPP.Nama AS AkunHPP, MAkunPenjualan.Nama AS AkunPenjualan, MAkunHPPRetur.Nama AS AkunHPPRetur, MAkunReturPenjualan.Nama AS AkunReturPenjualan, MTypePajak.Kode AS TypePajak " & vbCrLf & _
                     " FROM MBarang " & vbCrLf & _
                     " LEFT JOIN MUser On MBarang.IDUserEdit=MUser.NoID " & vbCrLf & _
                     " LEFT JOIN MMerk On MBarang.IDMerk=MMerk.NoID " & vbCrLf & _
                     " LEFT JOIN MTypePajak On MTypePajak.NoID=MBarang.IDTypePajak " & vbCrLf & _
                     " LEFT JOIN MAlamat MSupplier1 On MSupplier1.NoID=MBarang.IDSupplier1 " & vbCrLf & _
                     " LEFT JOIN MAlamat MSupplier2 On MSupplier2.NoID=MBarang.IDSupplier2 " & vbCrLf & _
                     " LEFT JOIN MAlamat MSupplier3 On MSupplier3.NoID=MBarang.IDSupplier3 " & vbCrLf & _
                     " LEFT JOIN MAlamat MSupplier4 On MSupplier4.NoID=MBarang.IDSupplier4 " & vbCrLf & _
                     " LEFT JOIN MAlamat MSupplier5 On MSupplier5.NoID=MBarang.IDSupplier5 " & vbCrLf & _
                     " LEFT JOIN MBarangD On MbarangD.IDBarang=MBarang.NoID " & vbCrLf & _
                     " LEFT OUTER JOIN MSatuan ON MBarangD.IDSatuan = MSatuan.NoID " & vbCrLf & _
                     " LEFT OUTER JOIN MKategori ON MBarang.IDKategori = MKategori.NoID " & vbCrLf & _
                     " LEFT JOIN MAkun MAkunPersediaan ON MAkunPersediaan.ID=MBarang.IDAkunPersediaan " & vbCrLf & _
                     " LEFT JOIN MAkun MAkunHPP ON MAkunHPP.ID=MBarang.IDAkunHPP " & vbCrLf & _
                     " LEFT JOIN MAkun MAkunPenjualan ON MAkunPenjualan.ID=MBarang.IDAkunPenjualan " & vbCrLf & _
                     " LEFT JOIN MAkun MAkunHPPRetur ON MAkunHPPRetur.ID=MBarang.IDAkunHPPRetur " & vbCrLf & _
                     " LEFT JOIN MAkun MAkunReturPenjualan ON MAkunReturPenjualan.ID=MBarang.IDAkunRetur " & vbCrLf
            If Not ckAll.Checked Then
                strsql &= " WHERE MBarang.IsActive=1 and MBarangD.IsActive=1"
            End If
            If filter <> "" Then
                strsql &= IIf(Not ckAll.Checked, " AND ", " WHERE ") & " (UPPER(MBarang.Nama + ' ' + MBarangD.Varian) LIKE '" & FixApostropi(filter.ToUpper) & "%' OR UPPER(MBarang.Kode) LIKE '" & FixApostropi(filter.ToUpper) & "%' OR UPPER(MBarangD.Barcode) LIKE '" & FixApostropi(filter.ToUpper) & "%') "
            End If
            If txtSupplier.Enabled Then
                strsql &= " AND (MBarang.IDSupplier1= " & NullToLong(txtSupplier.EditValue) & " or mbarang.IDSupplier5=" & NullToLong(txtSupplier.EditorTypeName) & ")"
            End If
            If txtKategori.Enabled Then
                strsql &= " AND MBarang.IDKategori= " & NullToLong(txtKategori.EditValue) & " "
            End If
            If txtMerk.Enabled Then
                strsql &= " AND MBarang.IDMerk= " & NullToLong(txtMerk.EditValue) & ""
            End If

            'TimeStart = Date.Now()
            'TryCast(Me.MdiParent, frmMain).mnStatusUser.Caption = "Start : " & TimeStart.ToString("HH:mm:ss")
            odsT2 = ExecuteDataset("vBrg", strsql)
            If ds.Tables("vBrg") Is Nothing Then
            Else
                ds.Tables("vBrg").Clear()
            End If
            ds = odsT2
            BS.DataSource = ds.Tables("vBrg")
            GC1.DataSource = BS.DataSource
            'TryCast(Me.MdiParent, frmMain).mnStatusUser.Caption &= ", End : " & DateDiff(DateInterval.Second, TimeStart, Date.Now()) & " s"
        Catch ex As Exception
            XtraMessageBox.Show(ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            odsT2.Dispose()
            dlg.Close()
            dlg.Dispose()
            txtSearch.Text = ""
            txtSearch.Focus()
        End Try
    End Sub
    Sub Hapus()
        Dim view As ColumnView = GC1.FocusedView
        Try
            Dim row As System.Data.DataRow

            If XtraTabControl1.SelectedTabPageIndex = 0 Then
                row = view.GetDataRow(GV1.FocusedRowHandle)
            ElseIf XtraTabControl1.SelectedTabPageIndex = 1 Then
                row = view.GetDataRow(GridView1.FocusedRowHandle)
            Else
                row = view.GetDataRow(LayoutView1.FocusedRowHandle)
            End If
            Dim NoID As Long = NullToLong(row("NoID"))
            If XtraMessageBox.Show(Me, "Yakin mau hapus data " & Me.Text & " dengan Kode " & NullToStr(row("Kode")), NamaAplikasi, MessageBoxButtons.YesNo) = Windows.Forms.DialogResult.Yes Then
                EksekusiSQL("update MBarang set IsActive=0 where NoID= " & NoID.ToString)
                RefreshData()
                If XtraTabControl1.SelectedTabPageIndex = 0 Then
                    GV1.ClearSelection()
                    GV1.FocusedRowHandle = GV1.LocateByDisplayText(0, GV1.Columns("NoID"), (NoID).ToString("#,##0"))
                    GV1.SelectRow(GV1.FocusedRowHandle)
                ElseIf XtraTabControl1.SelectedTabPageIndex = 1 Then
                    GridView1.ClearSelection()
                    GridView1.FocusedRowHandle = GridView1.LocateByDisplayText(0, GridView1.Columns("NoID"), (NoID).ToString("#,##0"))
                    GridView1.SelectRow(GridView1.FocusedRowHandle)
                Else
                    LayoutView1.ClearSelection()
                    LayoutView1.FocusedRowHandle = LayoutView1.LocateByDisplayText(0, LayoutView1.Columns("NoID"), (NoID).ToString("#,##0"))
                    LayoutView1.SelectRow(LayoutView1.FocusedRowHandle)
                End If
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
                GC1.ExportToXls(dlgsave.FileName)
            ElseIf XtraTabControl1.SelectedTabPageIndex = 1 Then
                GridView1.ExportToXls(dlgsave.FileName)
            Else
                LayoutView1.ExportToXls(dlgsave.FileName)
            End If
            BukaFile(dlgsave.FileName)
        End If
        dlgsave.Dispose()
    End Sub
    Sub PrintPreview()
        'If XtraTabControl1.SelectedTabPageIndex = 0 Then
        '    GC1.ShowPrintPreview()
        'ElseIf XtraTabControl1.SelectedTabPageIndex = 1 Then
        '    GridControl1.ShowPrintPreview()
        'Else
        '    GridControl2.ShowPrintPreview()
        'End If
        Dim NamaFile As String = Application.StartupPath & "\Report\DaftarMasterStock.rpt"
        Try
            If System.IO.File.Exists(NamaFile) Then
                ViewReport(Me.MdiParent, IIf(EditReport, action_.Edit, action_.Preview), NamaFile, "Daftar Master Stok", , , IIf(txtMerk.Text <> "", "IDMerk=" & NullToLong(txtMerk.EditValue) & "&", ""))
            Else
                XtraMessageBox.Show("Nama File Report : " & NamaFile & " tidak ditemukan.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If
        Catch ex As Exception
            XtraMessageBox.Show("Info Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
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

    Private Sub cmdFaktur_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdFaktur.Click
        mnHistory.PerformClick()
    End Sub
    Private Sub ShowStock()
        Try
            If IsShowStock AndAlso Not frmlookup Is Nothing Then
                frmlookup.TglDari.DateTime = NullToDate(GV1.GetRowCellValue(GV1.FocusedRowHandle, "DariTgl"))
                frmlookup.TglSampai.DateTime = NullToDate(GV1.GetRowCellValue(GV1.FocusedRowHandle, "SampaiTgl"))
                frmlookup.txtBarang.EditValue = NullToLong(GV1.GetRowCellValue(GV1.FocusedRowHandle, "NoID"))
                'frmlookup.RefreshData()
            End If
            If IsShowStock AndAlso Not frmSaldoGudang Is Nothing Then
                frmSaldoGudang.IDBarang = NullToLong(GV1.GetRowCellValue(GV1.FocusedRowHandle, "NoID"))
                frmSaldoGudang.RefreshData()
            End If
        Catch ex As Exception

        End Try
    End Sub
    Private Sub GV1_FocusedRowChanged(ByVal sender As Object, ByVal e As DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs) Handles GV1.FocusedRowChanged
        Dim oConn As New SqlConnection
        Dim ocmd As New SqlCommand
        Dim ODA As New SqlDataAdapter
        Try
            ShowStock()
            'If mnCkShowGallery.Checked Then
            oConn = New SqlConnection(StrKonSql)
            ocmd = New SqlCommand("SELECT Foto from MBarang where NoID=" & NullToLong(GV1.GetDataRow(GV1.FocusedRowHandle).Item("NoID")), oConn)
            oConn.Open()
            ODA = New SqlDataAdapter(ocmd)
            If Not ds.Tables("Gambar") Is Nothing Then
                ds.Tables("Gambar").Clear()
            End If
            ODA.Fill(ds, "Gambar")
            'End If
        Catch ex As Exception
            'VPOINT.Function.Fungsi.FxMessage(ex.Message,NamaAplikasi,MessageBoxButtons.OK,MessageBoxIcon.Information)
        Finally
            ocmd.Dispose()
            oConn.Close()
            oConn.Dispose()
            ODA.Dispose()
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
        Dim strsql As String = "SELECT * FROM MJenisBarang WHERE IsActive=1 "
        Dim frmlookup As New frmLookup
        Dim IDDetil As Long = -1
        Try
            frmlookup.Strsql = strsql
            frmlookup.FormName = "JenisBarang"
            If frmlookup.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
                Dim dlg As New WaitDialogForm("Proses sedang dilakukan...", "Mohon Tunggu Sebentar.")
                dlg.Show()
                dlg.Owner = Me
                dlg.TopMost = True
                If XtraTabControl1.SelectedTabPageIndex = 0 Then
                    For Each i In GV1.GetSelectedRows
                        IDDetil = CStr(GV1.GetDataRow(i).Item("NoID"))
                        EksekusiSQL("UPDATE MBarang set IDJenis=" & NullToLong(frmlookup.NoID) & " WHERE NoID=" & IDDetil)
                    Next
                ElseIf XtraTabControl1.SelectedTabPageIndex = 1 Then
                    For Each i In GridView1.GetSelectedRows
                        IDDetil = CStr(GridView1.GetDataRow(i).Item("NoID"))
                        EksekusiSQL("UPDATE MBarang set IDJenis=" & NullToLong(frmlookup.NoID) & " WHERE NoID=" & IDDetil)
                    Next
                Else
                    For Each i In LayoutView1.GetSelectedRows
                        IDDetil = CStr(LayoutView1.GetDataRow(i).Item("NoID"))
                        EksekusiSQL("UPDATE MBarang set IDJenis=" & NullToLong(frmlookup.NoID) & " WHERE NoID=" & IDDetil)
                    Next
                End If
                RefreshData()
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
        Dim strsql As String = "SELECT * FROM MKategori WHERE IsActive=1 "
        Dim frmlookup As New frmLookup
        Dim IDDetil As Long = -1
        Try
            frmlookup.Strsql = strsql
            frmlookup.FormName = "KategoriBarang"
            If frmlookup.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
                Dim dlg As New WaitDialogForm("Proses sedang dilakukan...", "Mohon Tunggu Sebentar.")
                dlg.Show()
                dlg.Owner = Me
                dlg.TopMost = True
                If XtraTabControl1.SelectedTabPageIndex = 0 Then
                    For Each i In GV1.GetSelectedRows
                        IDDetil = CStr(GV1.GetDataRow(i).Item("NoID"))
                        EksekusiSQL("UPDATE MBarang set IDKategori=" & NullToLong(frmlookup.NoID) & " WHERE NoID=" & IDDetil)
                    Next
                ElseIf XtraTabControl1.SelectedTabPageIndex = 1 Then
                    For Each i In GridView1.GetSelectedRows
                        IDDetil = CStr(GridView1.GetDataRow(i).Item("NoID"))
                        EksekusiSQL("UPDATE MBarang set IDKategori=" & NullToLong(frmlookup.NoID) & " WHERE NoID=" & IDDetil)
                    Next
                Else
                    For Each i In LayoutView1.GetSelectedRows
                        IDDetil = CStr(LayoutView1.GetDataRow(i).Item("NoID"))
                        EksekusiSQL("UPDATE MBarang set IDKategori=" & NullToLong(frmlookup.NoID) & " WHERE NoID=" & IDDetil)
                    Next
                End If
                RefreshData()
                dlg.Close()
                dlg.Dispose()
            End If
        Catch ex As Exception
            XtraMessageBox.Show("Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK)
        Finally
            frmlookup.Dispose()
        End Try
    End Sub

    Private Sub mnNewItem_ItemClick(ByVal sender As Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles mnNewItem.ItemClick
        Dim IDDetil As Long = -1
        Try
            If XtraMessageBox.Show("Konfirmasi set Produk Baru?", NamaAplikasi, MessageBoxButtons.YesNo) = Windows.Forms.DialogResult.Yes Then
                Dim dlg As New WaitDialogForm("Permintaan seedang diproses...", "Mohon Tunggu Sebentar.")
                dlg.Show()
                dlg.Owner = Me
                dlg.TopMost = True
                If XtraTabControl1.SelectedTabPageIndex = 0 Then
                    For Each i In GV1.GetSelectedRows
                        IDDetil = CStr(GV1.GetDataRow(i).Item("NoID"))
                        EksekusiSQL("UPDATE MBarang set IsNewItem=1 WHERE NoID=" & IDDetil)
                    Next
                ElseIf XtraTabControl1.SelectedTabPageIndex = 1 Then
                    For Each i In GridView1.GetSelectedRows
                        IDDetil = CStr(GridView1.GetDataRow(i).Item("NoID"))
                        EksekusiSQL("UPDATE MBarang set IsNewItem=1 WHERE NoID=" & IDDetil)
                    Next
                Else
                    For Each i In LayoutView1.GetSelectedRows
                        IDDetil = CStr(LayoutView1.GetDataRow(i).Item("NoID"))
                        EksekusiSQL("UPDATE MBarang set IsNewItem=1 WHERE NoID=" & IDDetil)
                    Next
                End If
                RefreshData()
                dlg.Close()
                dlg.Dispose()
            End If
        Catch ex As Exception
            XtraMessageBox.Show("Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK)
        End Try
    End Sub

    Private Sub mnUsetNewItem_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles mnUsetNewItem.ItemClick
        Dim IDDetil As Long = -1
        Try
            If XtraMessageBox.Show("Konfirmasi bukan Produk Baru?", NamaAplikasi, MessageBoxButtons.YesNo) = Windows.Forms.DialogResult.Yes Then
                Dim dlg As New WaitDialogForm("Permintaan seedang diproses...", "Mohon Tunggu Sebentar.")
                dlg.Show()
                dlg.Owner = Me
                dlg.TopMost = True
                If XtraTabControl1.SelectedTabPageIndex = 0 Then
                    For Each i In GV1.GetSelectedRows
                        IDDetil = CStr(GV1.GetDataRow(i).Item("NoID"))
                        EksekusiSQL("UPDATE MBarang set IsNewItem=0 WHERE NoID=" & IDDetil)
                    Next
                ElseIf XtraTabControl1.SelectedTabPageIndex = 1 Then
                    For Each i In GridView1.GetSelectedRows
                        IDDetil = CStr(GridView1.GetDataRow(i).Item("NoID"))
                        EksekusiSQL("UPDATE MBarang set IsNewItem=0 WHERE NoID=" & IDDetil)
                    Next
                Else
                    For Each i In LayoutView1.GetSelectedRows
                        IDDetil = CStr(LayoutView1.GetDataRow(i).Item("NoID"))
                        EksekusiSQL("UPDATE MBarang set IsNewItem=0 WHERE NoID=" & IDDetil)
                    Next
                End If
                RefreshData()
                dlg.Close()
                dlg.Dispose()
            End If
        Catch ex As Exception
            XtraMessageBox.Show("Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK)
        End Try
    End Sub

    Private Sub mnShowImage_ItemClick(ByVal sender As Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles mnShowImage.ItemClick
        Try
            frmImage.Dispose()
            If Not ds.Tables("Gambar") Is Nothing Then
                frmImage = New frmShowImage
                frmImage.PictureEdit1.DataBindings.Add("EditValue", ds.Tables("Gambar"), "Foto")
                frmImage.TopMost = True
                frmImage.Show()
            End If
        Catch ex As Exception
            XtraMessageBox.Show("Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub mnGenerateBarcode_ItemClick(ByVal sender As Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles mnGenerateBarcode.ItemClick
        If MsgBox("Untuk menghindari kesalahan Generate, maka barcode kosong saja yang bisa digenerate." & vbCrLf & "Mau lanjut?", MsgBoxStyle.Question + MsgBoxStyle.YesNo) = MsgBoxResult.No Then Exit Sub
        Dim IDDetil As Long = -1
        Dim NewBarcode As String = ""
        Dim dlg As WaitDialogForm = Nothing
        Try
            dlg = New WaitDialogForm("Perintah sedang diproses...", "Mohon Tunggu Sebentar.")
            dlg.Show()
            dlg.Owner = Me
            dlg.TopMost = True
            If XtraTabControl1.SelectedTabPageIndex = 0 Then
                For Each i In GV1.GetSelectedRows
                    IDDetil = CStr(GV1.GetDataRow(i).Item("IDDetil"))
                    If CStr(GV1.GetDataRow(i).Item("Barcode")).Trim = "" Then
                        NewBarcode = Append_EAN13_Checksum("8" & CLng(GV1.GetDataRow(i).Item("Kode")).ToString("00000000000"))
                        'NewBarcode = Append_EAN13_Checksum("9" & IDDetil.ToString("00000000000")).ToString
                        EksekusiSQL("UPDATE MBarangD set Barcode='" & FixApostropi(NewBarcode) & "' WHERE NoID=" & IDDetil)
                        EksekusiSQL("UPDATE MBarang set Barcode='" & FixApostropi(NewBarcode) & "' WHERE NoID=" & CStr(GV1.GetDataRow(i).Item("NoID")))
                    End If
                Next
            ElseIf XtraTabControl1.SelectedTabPageIndex = 1 Then
                For Each i In GridView1.GetSelectedRows
                    IDDetil = CStr(GridView1.GetDataRow(i).Item("IDDetil"))
                    'NewBarcode = Append_EAN13_Checksum("9" & IDDetil.ToString("00000000000")).ToString
                    'EksekusiSQL("UPDATE MBarang set Barcode='" & FixApostropi(NewBarcode) & "' WHERE NoID=" & IDDetil)
                Next
            Else
                For Each i In LayoutView1.GetSelectedRows
                    IDDetil = CStr(LayoutView1.GetDataRow(i).Item("IDDetil"))
                    'NewBarcode = Append_EAN13_Checksum("9" & IDDetil.ToString("00000000000")).ToString
                    'EksekusiSQL("UPDATE MBarang set Barcode='" & FixApostropi(NewBarcode) & "' WHERE NoID=" & IDDetil)
                Next
            End If
            RefreshData()
            GV1.ClearSelection()
            GV1.FocusedRowHandle = GV1.LocateByDisplayText(0, GV1.Columns("IDDetil"), IDDetil.ToString("#,##0"))
            GV1.SelectRow(GV1.FocusedRowHandle)
        Catch ex As Exception
            XtraMessageBox.Show("Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK)
        Finally
            If Not dlg Is Nothing Then
                dlg.Close()
                dlg.Dispose()
            End If
        End Try
    End Sub

    Private Sub mnCetakBarcode_ItemClick(ByVal sender As Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles mnCetakBarcode.ItemClick
        Dim IDDetil As Long = -1
        Dim NewBarcode As String = ""
        Try
            Dim dlg As New WaitDialogForm("Perintah sedang diproses...", "Mohon Tunggu Sebentar.")
            dlg.Show()
            dlg.Owner = Me
            dlg.TopMost = True
            EksekusiSQL("UPDATE MBarang set IsPrint=0")
            If XtraTabControl1.SelectedTabPageIndex = 0 Then
                For Each i In GV1.GetSelectedRows
                    IDDetil = CStr(GV1.GetDataRow(i).Item("NoID"))
                    EksekusiSQL("UPDATE MBarang set IsPrint=1 WHERE NoID=" & IDDetil)
                Next
            ElseIf XtraTabControl1.SelectedTabPageIndex = 1 Then
                For Each i In GridView1.GetSelectedRows
                    IDDetil = CStr(GridView1.GetDataRow(i).Item("NoID"))
                    EksekusiSQL("UPDATE MBarang set IsPrint=1 WHERE NoID=" & IDDetil)
                Next
            Else
                For Each i In LayoutView1.GetSelectedRows
                    IDDetil = CStr(LayoutView1.GetDataRow(i).Item("NoID"))
                    EksekusiSQL("UPDATE MBarang set IsPrint=1 WHERE NoID=" & IDDetil)
                Next
            End If
            If System.IO.File.Exists(Application.StartupPath & "\Report\CTBarcode.btw") Then
                BukaFile(Application.StartupPath & "\Report\CTBarcode.btw")
            End If
            RefreshData()
            dlg.Close()
            dlg.Dispose()
        Catch ex As Exception
            XtraMessageBox.Show("Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK)
        End Try
    End Sub

    Private Sub mnSettingSupplier1_ItemClick(ByVal sender As Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles mnSettingSupplier1.ItemClick
        Dim strsql As String = "SELECT NoID,Kode,Nama,ALamat FROM MAlamat WHERE IsActive=1 AND IsSupplier=1"
        Dim frmlookup As New frmLookup
        Dim IDDetil As Long = -1
        Try
            frmlookup.Strsql = strsql
            frmlookup.FormName = "DaftarSupplier"
            If frmlookup.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
                Dim dlg As New WaitDialogForm("Proses sedang dilakukan...", "Mohon Tunggu Sebentar.")
                dlg.Show()
                dlg.Owner = Me
                dlg.TopMost = True
                If XtraTabControl1.SelectedTabPageIndex = 0 Then
                    For Each i In GV1.GetSelectedRows
                        IDDetil = CStr(GV1.GetDataRow(i).Item("NoID"))
                        EksekusiSQL("UPDATE MBarang set IDSupplier1=" & NullToLong(frmlookup.NoID) & " WHERE NoID=" & IDDetil)
                    Next
                ElseIf XtraTabControl1.SelectedTabPageIndex = 1 Then
                    For Each i In GridView1.GetSelectedRows
                        IDDetil = CStr(GridView1.GetDataRow(i).Item("NoID"))
                        EksekusiSQL("UPDATE MBarang set IDSupplier1=" & NullToLong(frmlookup.NoID) & " WHERE NoID=" & IDDetil)
                    Next
                Else
                    For Each i In LayoutView1.GetSelectedRows
                        IDDetil = CStr(LayoutView1.GetDataRow(i).Item("NoID"))
                        EksekusiSQL("UPDATE MBarang set IDSupplier1=" & NullToLong(frmlookup.NoID) & " WHERE NoID=" & IDDetil)
                    Next
                End If
                RefreshData()
                dlg.Close()
                dlg.Dispose()
            End If
        Catch ex As Exception
            XtraMessageBox.Show("Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK)
        Finally
            frmlookup.Dispose()
        End Try
    End Sub

    Private Sub mnSettingSupplier2_ItemClick(ByVal sender As Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles mnSettingSupplier2.ItemClick
        Dim strsql As String = "SELECT NoID,Kode,Nama,ALamat FROM MAlamat WHERE IsActive=1 AND IsSupplier=1"
        Dim frmlookup As New frmLookup
        Dim IDDetil As Long = -1
        Try
            frmlookup.Strsql = strsql
            frmlookup.FormName = "DaftarSupplier"
            If frmlookup.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
                Dim dlg As New WaitDialogForm("Proses sedang dilakukan...", "Mohon Tunggu Sebentar.")
                dlg.Show()
                dlg.Owner = Me
                dlg.TopMost = True
                If XtraTabControl1.SelectedTabPageIndex = 0 Then
                    For Each i In GV1.GetSelectedRows
                        IDDetil = CStr(GV1.GetDataRow(i).Item("NoID"))
                        EksekusiSQL("UPDATE MBarang set IDSupplier2=" & NullToLong(frmlookup.NoID) & " WHERE NoID=" & IDDetil)
                    Next
                ElseIf XtraTabControl1.SelectedTabPageIndex = 1 Then
                    For Each i In GridView1.GetSelectedRows
                        IDDetil = CStr(GridView1.GetDataRow(i).Item("NoID"))
                        EksekusiSQL("UPDATE MBarang set IDSupplier2=" & NullToLong(frmlookup.NoID) & " WHERE NoID=" & IDDetil)
                    Next
                Else
                    For Each i In LayoutView1.GetSelectedRows
                        IDDetil = CStr(LayoutView1.GetDataRow(i).Item("NoID"))
                        EksekusiSQL("UPDATE MBarang set IDSupplier2=" & NullToLong(frmlookup.NoID) & " WHERE NoID=" & IDDetil)
                    Next
                End If
                RefreshData()
                dlg.Close()
                dlg.Dispose()
            End If
        Catch ex As Exception
            XtraMessageBox.Show("Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK)
        Finally
            frmlookup.Dispose()
        End Try

    End Sub

    Private Sub mnSettingSupplier3_ItemClick(ByVal sender As Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles mnSettingSupplier3.ItemClick
        Dim strsql As String = "SELECT NoID,Kode,Nama,ALamat FROM MAlamat WHERE IsActive=1 AND IsSupplier=1"
        Dim frmlookup As New frmLookup
        Dim IDDetil As Long = -1
        Try
            frmlookup.Strsql = strsql
            frmlookup.FormName = "DaftarSupplier"
            If frmlookup.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
                Dim dlg As New WaitDialogForm("Proses sedang dilakukan...", "Mohon Tunggu Sebentar.")
                dlg.Show()
                dlg.Owner = Me
                dlg.TopMost = True

                If XtraTabControl1.SelectedTabPageIndex = 0 Then
                    For Each i In GV1.GetSelectedRows
                        IDDetil = CStr(GV1.GetDataRow(i).Item("NoID"))
                        EksekusiSQL("UPDATE MBarang set IDSupplier3=" & NullToLong(frmlookup.NoID) & " WHERE NoID=" & IDDetil)
                    Next
                ElseIf XtraTabControl1.SelectedTabPageIndex = 1 Then
                    For Each i In GridView1.GetSelectedRows
                        IDDetil = CStr(GridView1.GetDataRow(i).Item("NoID"))
                        EksekusiSQL("UPDATE MBarang set IDSupplier3=" & NullToLong(frmlookup.NoID) & " WHERE NoID=" & IDDetil)
                    Next
                Else
                    For Each i In LayoutView1.GetSelectedRows
                        IDDetil = CStr(LayoutView1.GetDataRow(i).Item("NoID"))
                        EksekusiSQL("UPDATE MBarang set IDSupplier3=" & NullToLong(frmlookup.NoID) & " WHERE NoID=" & IDDetil)
                    Next
                End If
                RefreshData()
                dlg.Close()
                dlg.Dispose()
            End If
        Catch ex As Exception
            XtraMessageBox.Show("Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK)
        Finally
            frmlookup.Dispose()
        End Try

    End Sub

    Private Sub mnSettingSupplier4_ItemClick(ByVal sender As Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles mnSettingSupplier4.ItemClick
        Dim strsql As String = "SELECT NoID,Kode,Nama,ALamat FROM MAlamat WHERE IsActive=1 AND IsSupplier=1"
        Dim frmlookup As New frmLookup
        Dim IDDetil As Long = -1
        Try
            frmlookup.Strsql = strsql
            frmlookup.FormName = "DaftarSupplier"
            If frmlookup.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
                Dim dlg As New WaitDialogForm("Proses sedang dilakukan...", "Mohon Tunggu Sebentar.")
                dlg.Show()
                dlg.Owner = Me
                dlg.TopMost = True

                If XtraTabControl1.SelectedTabPageIndex = 0 Then
                    For Each i In GV1.GetSelectedRows
                        IDDetil = CStr(GV1.GetDataRow(i).Item("NoID"))
                        EksekusiSQL("UPDATE MBarang set IDSupplier4=" & NullToLong(frmlookup.NoID) & " WHERE NoID=" & IDDetil)
                    Next
                ElseIf XtraTabControl1.SelectedTabPageIndex = 1 Then
                    For Each i In GridView1.GetSelectedRows
                        IDDetil = CStr(GridView1.GetDataRow(i).Item("NoID"))
                        EksekusiSQL("UPDATE MBarang set IDSupplier4=" & NullToLong(frmlookup.NoID) & " WHERE NoID=" & IDDetil)
                    Next
                Else
                    For Each i In LayoutView1.GetSelectedRows
                        IDDetil = CStr(LayoutView1.GetDataRow(i).Item("NoID"))
                        EksekusiSQL("UPDATE MBarang set IDSupplier4=" & NullToLong(frmlookup.NoID) & " WHERE NoID=" & IDDetil)
                    Next
                End If
                RefreshData()
                dlg.Close()
                dlg.Dispose()
            End If
        Catch ex As Exception
            XtraMessageBox.Show("Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK)
        Finally
            frmlookup.Dispose()
        End Try

    End Sub

    Private Sub mnSettingSupplier5_ItemClick(ByVal sender As Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles mnSettingSupplier5.ItemClick
        Dim strsql As String = "SELECT NoID,Kode,Nama,ALamat FROM MAlamat WHERE IsActive=1 AND IsSupplier=1"
        Dim frmlookup As New frmLookup
        Dim IDDetil As Long = -1
        Try
            frmlookup.Strsql = strsql
            frmlookup.FormName = "DaftarSupplier"
            If frmlookup.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
                Dim dlg As New WaitDialogForm("Proses sedang dilakukan...", "Mohon Tunggu Sebentar.")
                dlg.Show()
                dlg.Owner = Me
                dlg.TopMost = True

                If XtraTabControl1.SelectedTabPageIndex = 0 Then
                    For Each i In GV1.GetSelectedRows
                        IDDetil = CStr(GV1.GetDataRow(i).Item("NoID"))
                        EksekusiSQL("UPDATE MBarang set IDSupplier5=" & NullToLong(frmlookup.NoID) & " WHERE NoID=" & IDDetil)
                    Next
                ElseIf XtraTabControl1.SelectedTabPageIndex = 1 Then
                    For Each i In GridView1.GetSelectedRows
                        IDDetil = CStr(GridView1.GetDataRow(i).Item("NoID"))
                        EksekusiSQL("UPDATE MBarang set IDSupplier5=" & NullToLong(frmlookup.NoID) & " WHERE NoID=" & IDDetil)
                    Next
                Else
                    For Each i In LayoutView1.GetSelectedRows
                        IDDetil = CStr(LayoutView1.GetDataRow(i).Item("NoID"))
                        EksekusiSQL("UPDATE MBarang set IDSupplier5=" & NullToLong(frmlookup.NoID) & " WHERE NoID=" & IDDetil)
                    Next
                End If
                RefreshData()
                dlg.Close()
                dlg.Dispose()
            End If
        Catch ex As Exception
            XtraMessageBox.Show("Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK)
        Finally
            frmlookup.Dispose()
        End Try

    End Sub

    Private Sub mnHistory_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles mnHistory.ItemClick
        Try
            If Not frmlookup Is Nothing Then
                frmlookup.Dispose()
            End If
            frmlookup = New frmLookUpPembelianDetil
            frmlookup.StartPosition = FormStartPosition.WindowsDefaultLocation
            frmlookup.WindowState = FormWindowState.Normal
            If XtraTabControl1.SelectedTabPageIndex = 0 Then
                frmlookup.IDBarang = NullToLong(GV1.GetRowCellValue(GV1.FocusedRowHandle, "NoID"))
            ElseIf XtraTabControl1.SelectedTabPageIndex = 1 Then
                frmlookup.IDBarang = NullToLong(GridView1.GetRowCellValue(GridView1.FocusedRowHandle, "NoID"))
            Else
                frmlookup.IDBarang = NullToLong(LayoutView1.GetRowCellValue(LayoutView1.FocusedRowHandle, "NoID"))
            End If
            frmlookup.LookUp = frmLookUpPembelianDetil.JenisLookUp.HistoryAlamat
            frmlookup.TopMost = True
            frmlookup.ParentMDIForm = Me.MdiParent
            IsShowStock = True
            frmlookup.Show()
        Catch ex As Exception
            XtraMessageBox.Show("Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub TglDari_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TglDari.EditValueChanged

    End Sub

    Private Sub mnRefresh_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles mnRefresh.ItemClick
        SimpleButton8.PerformClick()
    End Sub

    Private Sub mnCkShowGallery_CheckedChanged(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles mnCkShowGallery.CheckedChanged
        Dim dlg As DevExpress.Utils.WaitDialogForm = Nothing
        Dim strsql As String = ""
        Dim dsT2 As New DataSet
        Try
            Dim curentcursor As Cursor = Windows.Forms.Cursor.Current
            Windows.Forms.Cursor.Current = Cursors.WaitCursor
            dlg = New DevExpress.Utils.WaitDialogForm(String.Format("Sedang merefresh data.{0}MOHON TUNGGU ...", vbCrLf), Application.ProductName)
            dlg.TopMost = False
            dlg.Show()
            Windows.Forms.Cursor.Current = Cursors.WaitCursor

            If mnCkShowGallery.Checked Then
                strsql = "SELECT MBarang.NoID, MBarang.Kode, MBarang.Nama, MBarang.Barcode, MSatuan.Nama AS Satuan, mjenisbarang.Nama AS Jenis, " & vbCrLf
                strsql &= " MKategori.Nama AS Kategori, MBarang.HPP, MBarang.HargaJualA, MBarang.HargaJualB, MBarang.HargaJualC, " & vbCrLf
                strsql &= " MBarang.HargaJualD, MBarang.HargaJualE, MBarang.KodeDuz, MBarang.Ctn_Duz, MBarang.IsActive, MBarang.IsNewItem AS NewItem, " & vbCrLf
                strsql &= " MBarang.HargaJualF, " & vbCrLf
                strsql &= " MBarang.Foto, " & vbCrLf
                strsql &= " MSupplier1.Nama AS Supplier1, " & vbCrLf
                strsql &= " MSupplier2.Nama AS Supplier2, " & vbCrLf
                strsql &= " MSupplier3.Nama AS Supplier3, " & vbCrLf
                strsql &= " MSupplier4.Nama AS Supplier4, " & vbCrLf
                strsql &= " MSupplier5.Nama AS Supplier5 " & vbCrLf
                strsql &= " FROM MBarang LEFT OUTER JOIN" & vbCrLf
                strsql &= " MSatuan ON MBarang.IDSatuan = MSatuan.NoID LEFT OUTER JOIN" & vbCrLf
                strsql &= " MAlamat MSupplier1 ON MBarang.IDSupplier1 = MSupplier1.NoID LEFT OUTER JOIN" & vbCrLf
                strsql &= " MAlamat MSupplier2 ON MBarang.IDSupplier2 = MSupplier2.NoID LEFT OUTER JOIN" & vbCrLf
                strsql &= " MAlamat MSupplier3 ON MBarang.IDSupplier3 = MSupplier3.NoID LEFT OUTER JOIN" & vbCrLf
                strsql &= " MAlamat MSupplier4 ON MBarang.IDSupplier4 = MSupplier4.NoID LEFT OUTER JOIN" & vbCrLf
                strsql &= " MAlamat MSupplier5 ON MBarang.IDSupplier5 = MSupplier5.NoID LEFT OUTER JOIN" & vbCrLf
                strsql &= " MKategori ON MBarang.IDKategori = MKategori.NoID LEFT OUTER JOIN" & vbCrLf
                strsql &= " mjenisbarang ON MBarang.IDJenis = mjenisbarang.NoID" & vbCrLf
                dsT2 = ExecuteDataset("Tbl", strsql)
                GridControl2.DataSource = dsT2.Tables("Tbl")
                For i As Integer = 0 To LayoutView1.Columns.Count - 1
                    Select Case LayoutView1.Columns(i).ColumnType.Name.ToLower
                        Case "int32", "int64", "int"
                            LayoutView1.Columns(i).DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
                            LayoutView1.Columns(i).DisplayFormat.FormatString = "n0"
                        Case "decimal", "single", "money", "double"
                            LayoutView1.Columns(i).DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
                            LayoutView1.Columns(i).DisplayFormat.FormatString = "n2"
                        Case "string"
                            LayoutView1.Columns(i).DisplayFormat.FormatType = DevExpress.Utils.FormatType.None
                            LayoutView1.Columns(i).DisplayFormat.FormatString = ""
                        Case "date"
                            LayoutView1.Columns(i).DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime
                            LayoutView1.Columns(i).DisplayFormat.FormatString = "dd-MM-yyyy"
                        Case "datetime"
                            LayoutView1.Columns(i).DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime
                            LayoutView1.Columns(i).DisplayFormat.FormatString = "dd-MM-yyyy HH:mm"
                        Case "byte[]"
                            reppicedit.SizeMode = DevExpress.XtraEditors.Controls.PictureSizeMode.Squeeze
                            LayoutView1.Columns(i).OptionsColumn.AllowGroup = False
                            LayoutView1.Columns(i).OptionsColumn.AllowSort = False
                            LayoutView1.Columns(i).OptionsFilter.AllowFilter = False
                            LayoutView1.Columns(i).ColumnEdit = reppicedit
                        Case "boolean"
                            LayoutView1.Columns(i).ColumnEdit = repckedit
                    End Select
                    'If LayoutView1.Columns(i).FieldName.Length >= 4 AndAlso LayoutView1.Columns(i).FieldName.Substring(0, 4).ToLower = "Kode".ToLower Then
                    '    LayoutView1.Columns(i).Fixed = FixedStyle.Left
                    'ElseIf LayoutView1.Columns(i).FieldName.ToLower = "Nama".ToLower Then
                    '    LayoutView1.Columns(i).Fixed = FixedStyle.Left
                    'End If
                Next
                If System.IO.File.Exists(FolderLayouts & Me.Name & LayoutView1.Name & IDUserAktif & ".xml") Then
                    LayoutView1.RestoreLayoutFromXml(FolderLayouts & Me.Name & LayoutView1.Name & IDUserAktif & ".xml")
                End If
                XtraTabPage3.PageVisible = True
                XtraTabControl1.SelectedTabPage = XtraTabPage3
            Else
                XtraTabPage3.PageVisible = False
                XtraTabControl1.SelectedTabPage = XtraTabPage1
            End If
        Catch ex As Exception
            XtraMessageBox.Show("Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            dsT2.Dispose()
            Windows.Forms.Cursor.Current = Cursors.Default
            dlg.Close()
            dlg.Dispose()
        End Try
    End Sub

    Private Sub GC1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles GC1.Click

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

    Private Sub mnSaveLayout_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles mnSaveLayout.ItemClick
        Dim xOtorisasi As New frmOtorisasiAdmin
        Try
            If XtraMessageBox.Show("Simpan Layout?", NamaAplikasi, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = Windows.Forms.DialogResult.Yes AndAlso xOtorisasi.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
                GV1.SaveLayoutToXml(FolderLayouts & Me.Name & IDUserAktif & ".xml")
                gvSupplier.SaveLayoutToXml(FolderLayouts & Me.Name & gvSupplier.Name & ".xml")
                gvKategori.SaveLayoutToXml(FolderLayouts & Me.Name & gvKategori.Name & ".xml")
                gvMerk.SaveLayoutToXml(FolderLayouts & Me.Name & gvMerk.Name & ".xml")
                LayoutControl1.SaveLayoutToXml(FolderLayouts & Me.Name & LayoutControl1.Name & ".xml")

                'If XtraTabPage2.PageVisible Then
                '    GridView1.SaveLayoutToXml(FolderLayouts & Me.Name & GridView1.Name & IDUserAktif & ".xml")
                'End If
                'If XtraTabPage3.PageVisible Then
                '    LayoutView1.SaveLayoutToXml(FolderLayouts & Me.Name & LayoutView1.Name & IDUserAktif & ".xml")
                'End If
                'If XtraMessageBox.Show("Simpan layouts juga sebagai default untuk akses " & DefTipeStock.ToString & "?", NamaAplikasi, MessageBoxButtons.YesNo, MessageBoxIcon.Question) Then
                GV1.SaveLayoutToXml(FolderLayouts & Me.Name & DefTipeStock.ToString & ".xml")
                If XtraTabPage2.PageVisible Then
                    GridView1.SaveLayoutToXml(FolderLayouts & Me.Name & GridView1.Name & DefTipeStock.ToString & ".xml")
                End If
                If XtraTabPage3.PageVisible Then
                    LayoutView1.SaveLayoutToXml(FolderLayouts & Me.Name & LayoutView1.Name & DefTipeStock.ToString & ".xml")
                End If
                'End If
            End If
        Catch ex As Exception
            XtraMessageBox.Show("Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            xOtorisasi.Dispose()
        End Try
    End Sub

    Private Sub mnSaldoPerGudang_ItemClick(ByVal sender As Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles mnSaldoPerGudang.ItemClick
        Try
            If Not frmSaldoGudang Is Nothing Then
                frmSaldoGudang.Dispose()
            End If
            frmSaldoGudang = New frmSaldoPerGudang
            frmSaldoGudang.StartPosition = FormStartPosition.WindowsDefaultLocation
            frmSaldoGudang.WindowState = FormWindowState.Normal
            If XtraTabControl1.SelectedTabPageIndex = 0 Then
                frmSaldoGudang.IDBarang = NullToLong(GV1.GetRowCellValue(GV1.FocusedRowHandle, "NoID"))
            ElseIf XtraTabControl1.SelectedTabPageIndex = 1 Then
                frmSaldoGudang.IDBarang = NullToLong(GridView1.GetRowCellValue(GridView1.FocusedRowHandle, "NoID"))
            Else
                frmSaldoGudang.IDBarang = NullToLong(LayoutView1.GetRowCellValue(LayoutView1.FocusedRowHandle, "NoID"))
            End If
            frmSaldoGudang.TopMost = True
            frmSaldoGudang.ParentMDIForm = Me.MdiParent
            IsShowStock = True
            frmSaldoGudang.RefreshData()
            frmSaldoGudang.Show()
        Catch ex As Exception
            XtraMessageBox.Show("Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub ckAll_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ckAll.CheckedChanged
        SimpleButton8.PerformClick()
    End Sub

    Private Sub LabelControl1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LabelControl1.Click
        TglDari.Enabled = Not TglDari.Enabled
    End Sub

    Private Sub mnSettingHarga_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles mnSettingHarga.ItemClick
        Dim x As New clsBarang
        x = New clsBarang
        x.FormName = "EntriBarang"
        x.isNew = False
        frmImage.TopMost = False
        frmImage.Close()
        frmImage.Dispose()
        If XtraTabControl1.SelectedTabPageIndex = 0 Then
            x.NoID = NullToLong(GV1.GetRowCellValue(GV1.FocusedRowHandle, "NoID"))
        ElseIf XtraTabControl1.SelectedTabPageIndex = 1 Then
            x.NoID = NullToLong(GridView1.GetRowCellValue(GridView1.FocusedRowHandle, "NoID"))
        Else
            x.NoID = NullToLong(LayoutView1.GetRowCellValue(LayoutView1.FocusedRowHandle, "NoID"))
        End If
        x.TipeEntri = clsBarang.TipeEntry.SettingHarga
        x.MdiParent = Me.MdiParent
        x.WindowState = FormWindowState.Normal
        x.Show()
        x.Focus()
    End Sub

    Private Sub mnKirimDataKassa(Optional ByVal IDKassa As Long = -1)
        If KirimDataKassa(IDKassa) Then
            XtraMessageBox.Show("Pengiriman berhasil.", NamaAplikasi, MessageBoxButtons.OK)
        End If
    End Sub

    Private Sub mnKirimBarangKeKassack_ItemClick(ByVal sender As Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs)
        Try
            mnKirimDataKassa(NullToLong(e.Item.Tag)) 'PerKassa
        Catch ex As Exception

        End Try
    End Sub
    Private Sub mnKirimBarangKeKassa_ItemClick(ByVal sender As Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles mnKirimBarangKeKassa.ItemClick
        mnKirimDataKassa() 'All
    End Sub

    Private Sub mnTimbangan_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles mnTimbangan.ItemClick
        If KirimDataTimbangan() Then
            XtraMessageBox.Show("Pengiriman berhasil.", NamaAplikasi, MessageBoxButtons.OK)
        End If
    End Sub

    Private Sub mnPromoDiskon_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles mnPromoDiskon.ItemClick
        SimpleButton7.PerformClick()
    End Sub

    Private Sub SimpleButton7_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SimpleButton7.Click
        Try
            'KODE DIBAWAH UNTUK MEMBUKA FORM HANYA 1
            frmlookup = New frmLookUpPembelianDetil
            Dim F As Object
            For Each F In Me.MdiParent.MdiChildren
                If TypeOf F Is frmLookUpPembelianDetil Then
                    frmlookup = F
                    Exit For
                End If
            Next
            If frmlookup Is Nothing Then
                frmlookup = New frmLookUpPembelianDetil
            End If
            frmlookup.WindowState = FormWindowState.Maximized
            frmlookup.MdiParent = Me.MdiParent
            If XtraTabControl1.SelectedTabPageIndex = 0 Then
                frmlookup.IDBarang = NullToLong(GV1.GetRowCellValue(GV1.FocusedRowHandle, "NoID"))
                frmlookup.TglDari.DateTime = NullToDate(GV1.GetRowCellValue(GV1.FocusedRowHandle, "DariTgl"))
                frmlookup.TglSampai.DateTime = NullToDate(GV1.GetRowCellValue(GV1.FocusedRowHandle, "SampaiTgl"))
            ElseIf XtraTabControl1.SelectedTabPageIndex = 1 Then
                frmlookup.IDBarang = NullToLong(GridView1.GetRowCellValue(GridView1.FocusedRowHandle, "NoID"))
                'frmlookup.TglDari.DateTime = NullToDate(GridView1.GetRowCellValue(GridView1.FocusedRowHandle, "Tang"))
                'frmlookup.TglSampai.DateTime = NullToDate(GridView1.GetRowCellValue(GridView1.FocusedRowHandle, "Tang"))
            Else
                frmlookup.IDBarang = NullToLong(LayoutView1.GetRowCellValue(LayoutView1.FocusedRowHandle, "NoID"))
                'frmlookup.TglDari.DateTime = NullToDate(LayoutView1.GetRowCellValue(LayoutView1.FocusedRowHandle, "NoID"))
                'frmlookup.TglSampai.DateTime = NullToDate(LayoutView1.GetRowCellValue(LayoutView1.FocusedRowHandle, "NoID"))
            End If
            frmlookup.LookUp = frmLookUpPembelianDetil.JenisLookUp.HistoryPenjualanPromo
            frmlookup.Show()
            frmlookup.Focus()
            IsShowStock = True

            'If Not frmlookup Is Nothing Then
            '    frmlookup.Dispose()
            'End If
            'frmlookup = New frmLookUpPembelianDetil
            'frmlookup.StartPosition = FormStartPosition.WindowsDefaultLocation
            'frmlookup.WindowState = FormWindowState.Normal
            'If XtraTabControl1.SelectedTabPageIndex = 0 Then
            '    frmlookup.IDBarang = NullToLong(GV1.GetRowCellValue(GV1.FocusedRowHandle, "NoID"))
            '    frmlookup.TglDari.DateTime = NullToDate(GV1.GetRowCellValue(GV1.FocusedRowHandle, "DariTgl"))
            '    frmlookup.TglSampai.DateTime = NullToDate(GV1.GetRowCellValue(GV1.FocusedRowHandle, "SampaiTgl"))
            'ElseIf XtraTabControl1.SelectedTabPageIndex = 1 Then
            '    frmlookup.IDBarang = NullToLong(GridView1.GetRowCellValue(GridView1.FocusedRowHandle, "NoID"))
            '    'frmlookup.TglDari.DateTime = NullToDate(GridView1.GetRowCellValue(GridView1.FocusedRowHandle, "Tang"))
            '    'frmlookup.TglSampai.DateTime = NullToDate(GridView1.GetRowCellValue(GridView1.FocusedRowHandle, "Tang"))
            'Else
            '    frmlookup.IDBarang = NullToLong(LayoutView1.GetRowCellValue(LayoutView1.FocusedRowHandle, "NoID"))
            '    'frmlookup.TglDari.DateTime = NullToDate(LayoutView1.GetRowCellValue(LayoutView1.FocusedRowHandle, "NoID"))
            '    'frmlookup.TglSampai.DateTime = NullToDate(LayoutView1.GetRowCellValue(LayoutView1.FocusedRowHandle, "NoID"))
            'End If
            'frmlookup.LookUp = frmLookUpPembelianDetil.JenisLookUp.HistoryPenjualanPromo
            ''frmlookup.TopMost = True
            'frmlookup.ParentMDIForm = Me.MdiParent
            'IsShowStock = True
            'frmlookup.Show()
            'frmlookup.Focus()
        Catch ex As Exception
            XtraMessageBox.Show("Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub mnQtyMin_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles mnQtyMin.ItemClick
        Dim x As New frmGetQty
        Dim IDDetil As Long = -1
        Dim iData As Long = -1
        Try
            x.Text = "Set Qty Minimum"
            x.lbQty.Text = "Qty Min"
            If x.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
                Dim dlg As New WaitDialogForm("Proses sedang dilakukan...", "Mohon Tunggu Sebentar.")
                dlg.Show()
                dlg.Owner = Me
                dlg.TopMost = True
                If XtraTabControl1.SelectedTabPageIndex = 0 Then
                    For Each i In GV1.GetSelectedRows
                        IDDetil = CStr(GV1.GetDataRow(i).Item("NoID"))
                        EksekusiSQL("UPDATE MBarang SET StockMin=" & FixKoma(x.Qty) & " WHERE NoID=" & IDDetil)
                        GV1.SetRowCellValue(i, "StockMin", FixKoma(x.Qty))
                    Next
                End If
                dlg.Close()
                dlg.Dispose()
            End If
            x.Dispose()
        Catch ex As Exception
            XtraMessageBox.Show("Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK)
        Finally
            'frmlookup.Dispose()
        End Try
    End Sub

    Private Sub BarButtonItem8_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem8.ItemClick

    End Sub

    Private Sub mnQtyMax_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles mnQtyMax.ItemClick
        Dim x As New frmGetQty
        Dim IDDetil As Long = -1
        Try
            x.Text = "Set Qty Maksimum"
            x.lbQty.Text = "Qty Max"

            If x.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
                Dim dlg As New WaitDialogForm("Proses sedang dilakukan...", "Mohon Tunggu Sebentar.")
                dlg.Show()
                dlg.Owner = Me
                dlg.TopMost = True
                If XtraTabControl1.SelectedTabPageIndex = 0 Then
                    For Each i In GV1.GetSelectedRows
                        IDDetil = CStr(GV1.GetDataRow(i).Item("NoID"))
                        EksekusiSQL("UPDATE MBarang set StockMax=" & FixKoma(x.Qty) & " WHERE NoID=" & IDDetil)
                        GV1.SetRowCellValue(i, "StockMax", FixKoma(x.Qty))
                    Next
                End If
                dlg.Close()
                dlg.Dispose()
            End If

            x.Dispose()
        Catch ex As Exception
            XtraMessageBox.Show("Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK)
        Finally
            'frmlookup.Dispose()
        End Try
    End Sub

    Private Sub txtSearch_ButtonClick(ByVal sender As Object, ByVal e As DevExpress.XtraEditors.Controls.ButtonPressedEventArgs) Handles txtSearch.ButtonClick
        If e.Button.Index = 0 Then
            RefreshDataBarang(txtSearch.Text)
        End If
    End Sub

    Private Sub txtSearch_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtSearch.EditValueChanged

    End Sub

    Private Sub txtSearch_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtSearch.KeyDown
        Try
            If e.KeyCode = Keys.Enter Then
                RefreshDataBarang(txtSearch.Text)
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

    Private Sub mnGabungKeKdoeBaranglain_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles mnGabungKeKdoeBaranglain.ItemClick
        Dim x As New frmPindahKodeBarang
        Dim IDDetil As Integer
        IDDetil = NullToLong(GV1.GetRowCellValue(GV1.FocusedRowHandle, "IDDetil"))
        x.IDBarangDLama = NullToLong(GV1.GetRowCellValue(GV1.FocusedRowHandle, "IDDetil"))
        x.IDBarangLama = NullToLong(GV1.GetRowCellValue(GV1.FocusedRowHandle, "NoID"))
        x.txtBarcode.Text = NullToStr(GV1.GetRowCellValue(GV1.FocusedRowHandle, "Barcode"))
        x.txtKode.Text = NullToStr(GV1.GetRowCellValue(GV1.FocusedRowHandle, "Kode"))
        x.txtNama.Text = NullToStr(GV1.GetRowCellValue(GV1.FocusedRowHandle, "Nama"))
        If x.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
            x.Dispose()
            RefreshData()
            GV1.FocusedRowHandle = GV1.LocateByDisplayText(0, GV1.Columns("IDDetil"), (IDDetil).ToString("#,##0"))

        Else
            x.Dispose()
        End If

    End Sub

    Private Sub LayoutControlItem1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LayoutControlItem1.Click
        txtSupplier.Enabled = Not txtSupplier.Enabled
    End Sub

    Private Sub LayoutControlItem3_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LayoutControlItem3.Click
        txtKategori.Enabled = Not txtKategori.Enabled
    End Sub

    Private Sub LayoutControlItem4_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LayoutControlItem4.Click
        txtMerk.Enabled = Not txtMerk.Enabled
    End Sub

    Sub SetAkun(ByVal nmfield As String)
        Dim frmlookup As New frmLUAkun
        Dim IDBarang As Long = -1
        Try

            If frmlookup.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
                Dim dlg As New WaitDialogForm("Proses sedang dilakukan...", "Mohon Tunggu Sebentar.")
                dlg.Show()
                dlg.Owner = Me
                dlg.TopMost = True
                If XtraTabControl1.SelectedTabPageIndex = 0 Then
                    For Each i In GV1.GetSelectedRows
                        IDBarang = CStr(GV1.GetDataRow(i).Item("NoID"))
                        EksekusiSQL("UPDATE MBarang set " & nmfield & "=" & NullToLong(frmlookup.NoID) & " WHERE NoID=" & IDBarang)
                    Next

                End If
                RefreshData()
                dlg.Close()
                dlg.Dispose()
            End If
        Catch ex As Exception
            XtraMessageBox.Show("Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK)
        Finally
            frmlookup.Dispose()
        End Try
    End Sub

    Private Sub mnAkunPersediaan_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles mnAkunPersediaan.ItemClick
        SetAkun("IDAkunPersediaan")
    End Sub


    Private Sub mnAkunReturPenjualan_ItemClick(ByVal sender As Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles mnAkunReturPenjualan.ItemClick
        SetAkun("IDAkunRetur")
    End Sub

    Private Sub mnAkunPenjualan_ItemClick(ByVal sender As Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles mnAkunPenjualan.ItemClick
        SetAkun("IDAkunPenjualan")
    End Sub

    Private Sub mnAkunHPP_ItemClick(ByVal sender As Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles mnAkunHPP.ItemClick
        SetAkun("IDAkunHPP")
    End Sub

    Private Sub mnAkunHPPReturPenjualan_ItemClick(ByVal sender As Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles mnAkunHPPReturPenjualan.ItemClick
        SetAkun("IDAkunHPPRetur")
    End Sub

    Sub SetDiscPromo(ByVal DiscBerapa As Integer)
        Dim frmlookup As New frmOpsiDiscPromo
        Dim IDBarang As Long = -1
        Dim SQL As String = ""
        Dim ds As New DataSet
        Dim DiscProsen As Double = 0.0, DiscRp As Double = 0.0, HargaJual As Double = 0.0, HargaNetto As Double = 0.0, Konversi As Double = 0.0
        Try
            frmlookup.LabelControl1.ToolTip = "Klik disini untuk merubah By Rupiah"
            frmlookup.LabelControl4.ToolTip = "Klik disini untuk merubah By Prosen"
            If DiscBerapa = 1 Then 'Untuk Disc Biasanya
                frmlookup.Text = "Opsi Disc / Promo"
                frmlookup.LabelControl1.Text = "Disc Promo (Rp)"
                frmlookup.LabelControl4.Text = "Disc Promo (%)"
            ElseIf DiscBerapa = 2 Then
                frmlookup.Text = "Opsi Disc Khusus Member"
                frmlookup.LabelControl1.Text = "Disc Khusus (Rp)"
                frmlookup.LabelControl4.Text = "Disc Khusus (%)"
                frmlookup.txtQty.Properties.ReadOnly = False
                frmlookup.txtDiscRp2.Properties.ReadOnly = False
            Else
                frmlookup.LabelControl3.Visible = False
                frmlookup.txtTglDari.Visible = False
                frmlookup.txtTglSampai.Visible = False
                frmlookup.LabelControl4.Visible = False
                frmlookup.txtDiscProsen.Visible = False
                frmlookup.LabelControl5.Visible = False
                frmlookup.LabelControl7.Visible = False
                frmlookup.txtQty.Visible = False
                frmlookup.Text = "Opsi Disc Barang-barang PDP"
                frmlookup.LabelControl1.Text = "Harga Khusus (Rp)" & vbCrLf & "Member"
                frmlookup.LabelControl4.Text = "Disc Khusus (%)"
                frmlookup.txtQty.Properties.ReadOnly = False
                frmlookup.txtDiscRp2.Visible = True
                frmlookup.txtDiscRp2.Properties.ReadOnly = True
            End If
            If frmlookup.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
                Dim dlg As New WaitDialogForm("Proses sedang dilakukan...", "Mohon Tunggu Sebentar.")
                dlg.Show()
                dlg.Owner = Me
                dlg.TopMost = True
                If XtraTabControl1.SelectedTabPageIndex = 0 Then
                    For Each i In GV1.GetSelectedRows
                        IDBarang = CStr(GV1.GetDataRow(i).Item("NoID"))
                        Select Case DiscBerapa
                            Case 3 'Artinya HargaKhusus PDP
                                If frmlookup.txtDiscRp.EditValue = 0 Then 'Kembalikan ke HargaJual
                                    'MBarang
                                    SQL = "UPDATE MBarang SET " & vbCrLf & _
                                          " DiscPDP=" & FixKoma(Bulatkan(0, 0)) & ", " & vbCrLf & _
                                          " TerakhirUpdate='" & frmlookup.txtTglEdit.DateTime.ToString("yyyy-MM-dd HH:mm") & "', IDUserEdit=" & NullToLong(IDUserAktif) & vbCrLf & _
                                          " WHERE NoID=" & IDBarang
                                    EksekusiSQL(SQL)
                                Else
                                    'MBarang
                                    HargaJual = NullToDbl(GV1.GetDataRow(i).Item("HargaJual"))
                                    HargaNetto = NullToDbl(frmlookup.txtDiscRp.EditValue)
                                    DiscRp = Bulatkan(HargaJual - HargaNetto, 0)
                                    SQL = "UPDATE MBarang SET " & vbCrLf & _
                                          " DiscPDP=" & FixKoma(Bulatkan(DiscRp, 0)) & ", " & vbCrLf & _
                                          " TerakhirUpdate='" & frmlookup.txtTglEdit.DateTime.ToString("yyyy-MM-dd HH:mm") & "', IDUserEdit=" & NullToLong(IDUserAktif) & vbCrLf & _
                                          " WHERE NoID=" & IDBarang
                                    EksekusiSQL(SQL)
                                End If
                                If frmlookup.txtDiscRp2.EditValue = 0 Then 'Kembalikan ke HargaJual
                                    'MBarang
                                    SQL = "UPDATE MBarang SET " & vbCrLf & _
                                          " DiscPDPMember=" & FixKoma(Bulatkan(0, 0)) & ", " & vbCrLf & _
                                          " TerakhirUpdate='" & frmlookup.txtTglEdit.DateTime.ToString("yyyy-MM-dd HH:mm") & "', IDUserEdit=" & NullToLong(IDUserAktif) & vbCrLf & _
                                          " WHERE NoID=" & IDBarang
                                    EksekusiSQL(SQL)
                                Else
                                    'MBarang
                                    HargaJual = NullToDbl(GV1.GetDataRow(i).Item("HargaJual"))
                                    HargaNetto = NullToDbl(frmlookup.txtDiscRp2.EditValue)
                                    DiscRp = Bulatkan(HargaJual - HargaNetto, 0)
                                    SQL = "UPDATE MBarang SET " & vbCrLf & _
                                          " DiscPDPMember=" & FixKoma(Bulatkan(DiscRp, 0)) & ", " & vbCrLf & _
                                          " TerakhirUpdate='" & frmlookup.txtTglEdit.DateTime.ToString("yyyy-MM-dd HH:mm") & "', IDUserEdit=" & NullToLong(IDUserAktif) & vbCrLf & _
                                          " WHERE NoID=" & IDBarang
                                    EksekusiSQL(SQL)
                                End If
                            Case 1 'Artinya Disc 1 / Promo Yg Berjalan Lama
                                SQL = "SELECT MBarangD.*, MBarang.HargaJual AS HJual FROM MBarangD INNER JOIN MBarang ON MBarang.NoID=MBarangD.IDBarang WHERE MBarangD.IsActive=1 AND MBarangD.IDBarang=" & IDBarang
                                ds = ExecuteDataset("MBarangD", SQL)
                                With ds.Tables("MBarangD")
                                    For iMBarangD = 0 To ds.Tables("MBarangD").Rows.Count - 1
                                        Konversi = NullToDbl(ds.Tables("MBarangD").Rows(iMBarangD).Item("Konversi"))
                                        HargaJual = NullToDbl(ds.Tables("MBarangD").Rows(iMBarangD).Item("HJual"))
                                        If Not frmlookup.txtDiscRp.Properties.ReadOnly Then 'By Rp
                                            DiscRp = NullToDbl(frmlookup.txtDiscRp.EditValue)
                                            If HargaJual = 0 Then
                                                DiscProsen = 0.0
                                            Else
                                                DiscProsen = NullToDbl(DiscRp) * 100 / NullToDbl(HargaJual)
                                            End If
                                            HargaNetto = Bulatkan(HargaJual, 0) - Bulatkan(DiscRp, 0)
                                        Else 'By Prosen
                                            DiscProsen = NullToDbl(frmlookup.txtDiscProsen.EditValue)
                                            DiscRp = Bulatkan((DiscProsen / 100) * HargaJual, 0)
                                            HargaNetto = Bulatkan(HargaJual, 0) - Bulatkan(DiscRp, 0)
                                        End If
                                        If frmlookup.txtTglDari.Text = "" Or frmlookup.txtTglSampai.Text = "" Then
                                            SQL = "UPDATE MBarangD SET " & vbCrLf & _
                                                  " NilaiDiskon=0, " & vbCrLf & _
                                                  " PromoDiskonJual=0, " & vbCrLf & _
                                                  " HargaJualNetto=" & FixKoma(Bulatkan(HargaJual * Konversi, 0)) & ", " & vbCrLf & _
                                                  " LastUpdated='" & frmlookup.txtTglEdit.DateTime.ToString("yyyy-MM-dd HH:mm") & "' " & vbCrLf & _
                                                  " WHERE NoID=" & NullToLong(ds.Tables("MBarangD").Rows(iMBarangD).Item("NoID"))
                                            EksekusiSQL(SQL)

                                            'MBarang
                                            SQL = "UPDATE MBarang SET " & vbCrLf & _
                                                  " DiscPromoRp=0, " & vbCrLf & _
                                                  " DiscPromo=0, " & vbCrLf & _
                                                  " HargaJualNetto=" & FixKoma(Bulatkan(HargaJual, 0)) & ", " & vbCrLf & _
                                                  " TglDariDiskon=NULL, " & vbCrLf & _
                                                  " TglSampaiDiskon=NULL, " & vbCrLf & _
                                                  " TerakhirUpdate='" & frmlookup.txtTglEdit.DateTime.ToString("yyyy-MM-dd HH:mm") & "', IDUserEdit=" & NullToLong(IDUserAktif) & vbCrLf & _
                                                  " WHERE NoID=" & NullToLong(ds.Tables("MBarangD").Rows(iMBarangD).Item("IDBarang"))
                                            EksekusiSQL(SQL)
                                        Else
                                            SQL = "UPDATE MBarangD SET " & vbCrLf & _
                                                  " NilaiDiskon=" & FixKoma(Bulatkan(DiscRp * Konversi, 0)) & ", " & vbCrLf & _
                                                  " PromoDiskonJual=" & FixKoma(Bulatkan(DiscProsen, 0)) & ", " & vbCrLf & _
                                                  " HargaJualNetto=" & FixKoma(Bulatkan(HargaNetto * Konversi, 0)) & ", " & vbCrLf & _
                                                  " LastUpdated='" & frmlookup.txtTglEdit.DateTime.ToString("yyyy-MM-dd HH:mm") & "' " & vbCrLf & _
                                                  " WHERE NoID=" & NullToLong(ds.Tables("MBarangD").Rows(iMBarangD).Item("NoID"))
                                            EksekusiSQL(SQL)

                                            'MBarang
                                            SQL = "UPDATE MBarang SET " & vbCrLf & _
                                                  " DiscPromoRp=" & FixKoma(Bulatkan(DiscRp, 0)) & ", " & vbCrLf & _
                                                  " DiscPromo=" & FixKoma(Bulatkan(DiscProsen, 0)) & ", " & vbCrLf & _
                                                  " HargaJualNetto=" & FixKoma(Bulatkan(HargaNetto, 0)) & ", " & vbCrLf & _
                                                  " TglDariDiskon='" & frmlookup.txtTglDari.DateTime.ToString("yyyy-MM-dd HH:mm") & "', " & vbCrLf & _
                                                  " TglSampaiDiskon='" & frmlookup.txtTglSampai.DateTime.ToString("yyyy-MM-dd HH:mm") & "', " & vbCrLf & _
                                                  " TerakhirUpdate='" & frmlookup.txtTglEdit.DateTime.ToString("yyyy-MM-dd HH:mm") & "', IDUserEdit=" & NullToLong(IDUserAktif) & vbCrLf & _
                                                  " WHERE NoID=" & NullToLong(ds.Tables("MBarangD").Rows(iMBarangD).Item("IDBarang"))
                                            EksekusiSQL(SQL)
                                        End If
                                    Next
                                End With
                            Case 2 'Artinya Disc 2 / Promo Yg Berjalan Pengganti Voucher
                                SQL = "SELECT MBarangD.*, MBarang.HargaJual AS HJual FROM MBarangD INNER JOIN MBarang ON MBarang.NoID=MBarangD.IDBarang WHERE MBarangD.IsActive=1 AND MBarangD.IDBarang=" & IDBarang
                                ds = ExecuteDataset("MBarangD", SQL)
                                With ds.Tables("MBarangD")
                                    For iMBarangD = 0 To ds.Tables("MBarangD").Rows.Count - 1
                                        Konversi = NullToDbl(ds.Tables("MBarangD").Rows(iMBarangD).Item("Konversi"))
                                        HargaJual = NullToDbl(ds.Tables("MBarangD").Rows(iMBarangD).Item("HJual"))
                                        If Not frmlookup.txtDiscRp.Properties.ReadOnly Then 'By Rp
                                            DiscRp = NullToDbl(frmlookup.txtDiscRp.EditValue)
                                            If HargaJual = 0 Then
                                                DiscProsen = 0.0
                                            Else
                                                DiscProsen = NullToDbl(DiscRp) * 100 / NullToDbl(HargaJual)
                                            End If
                                            HargaNetto = Bulatkan(HargaJual, 0) - Bulatkan(DiscRp, 0)
                                        Else 'By Prosen
                                            DiscProsen = NullToDbl(frmlookup.txtDiscProsen.EditValue)
                                            DiscRp = Bulatkan((DiscProsen / 100) * HargaJual, 0)
                                            HargaNetto = Bulatkan(HargaJual, 0) - Bulatkan(DiscRp, 0)
                                        End If
                                        If frmlookup.txtTglDari.Text = "" Or frmlookup.txtTglSampai.Text = "" Then
                                            'SQL = "UPDATE MBarangD SET " & vbCrLf & _
                                            '      " NilaiDiskon=" & FixKoma(Bulatkan(DiscRp * Konversi, 0)) & ", " & vbCrLf & _
                                            '      " PromoDiskonJual=" & FixKoma(Bulatkan(DiscProsen, 0)) & ", " & vbCrLf & _
                                            '      " HargaJualNetto=" & FixKoma(Bulatkan(HargaNetto * Konversi, 0)) & ", " & vbCrLf & _
                                            '      " LastUpdated='" & frmlookup.txtTglEdit.DateTime.ToString("yyyy-MM-dd HH:mm") & "' " & vbCrLf & _
                                            '      " WHERE NoID=" & NullToLong(ds.Tables("MBarangD").Rows(iMBarangD).Item("NoID"))
                                            'EksekusiSQL(SQL)

                                            'MBarang
                                            SQL = "UPDATE MBarang SET " & vbCrLf & _
                                                  " DiscMemberRp2=0, " & vbCrLf & _
                                                  " DiscMemberProsen2=0, " & vbCrLf & _
                                                  " QtyPDP=0, " & vbCrLf & _
                                                  " HargaNettoMember=" & FixKoma(Bulatkan(HargaJual, 0)) & ", " & vbCrLf & _
                                                  " TglDariDiskon2=NULL, " & vbCrLf & _
                                                  " TglSampaiDiskon2=NULL, " & vbCrLf & _
                                                  " TerakhirUpdate='" & frmlookup.txtTglEdit.DateTime.ToString("yyyy-MM-dd HH:mm") & "', IDUserEdit=" & NullToLong(IDUserAktif) & vbCrLf & _
                                                  " WHERE NoID=" & NullToLong(ds.Tables("MBarangD").Rows(iMBarangD).Item("IDBarang"))
                                            EksekusiSQL(SQL)
                                        Else
                                            'SQL = "UPDATE MBarangD SET " & vbCrLf & _
                                            '      " NilaiDiskon=" & FixKoma(Bulatkan(DiscRp * Konversi, 0)) & ", " & vbCrLf & _
                                            '      " PromoDiskonJual=" & FixKoma(Bulatkan(DiscProsen, 0)) & ", " & vbCrLf & _
                                            '      " HargaJualNetto=" & FixKoma(Bulatkan(HargaNetto * Konversi, 0)) & ", " & vbCrLf & _
                                            '      " LastUpdated='" & frmlookup.txtTglEdit.DateTime.ToString("yyyy-MM-dd HH:mm") & "' " & vbCrLf & _
                                            '      " WHERE NoID=" & NullToLong(ds.Tables("MBarangD").Rows(iMBarangD).Item("NoID"))
                                            'EksekusiSQL(SQL)

                                            'MBarang
                                            SQL = "UPDATE MBarang SET " & vbCrLf & _
                                                  " DiscMemberRp2=" & FixKoma(Bulatkan(DiscRp, 0)) & ", " & vbCrLf & _
                                                  " DiscMemberProsen2=" & FixKoma(Bulatkan(DiscProsen, 0)) & ", " & vbCrLf & _
                                                  " QtyPDP=" & FixKoma(frmlookup.txtQty.EditValue) & "," & vbCrLf & _
                                                  " HargaNettoMember=" & FixKoma(Bulatkan(HargaNetto, 0)) & ", " & vbCrLf & _
                                                  " TglDariDiskon2='" & frmlookup.txtTglDari.DateTime.ToString("yyyy-MM-dd HH:mm") & "', " & vbCrLf & _
                                                  " TglSampaiDiskon2='" & frmlookup.txtTglSampai.DateTime.ToString("yyyy-MM-dd HH:mm") & "', " & vbCrLf & _
                                                  " TerakhirUpdate='" & frmlookup.txtTglEdit.DateTime.ToString("yyyy-MM-dd HH:mm") & "', IDUserEdit=" & NullToLong(IDUserAktif) & vbCrLf & _
                                                  " WHERE NoID=" & NullToLong(ds.Tables("MBarangD").Rows(iMBarangD).Item("IDBarang"))
                                            EksekusiSQL(SQL)
                                        End If
                                    Next
                                End With
                        End Select
                    Next
                End If
                RefreshData()
                dlg.Close()
                dlg.Dispose()
            End If
        Catch ex As Exception
            XtraMessageBox.Show("Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK)
        Finally
            frmlookup.Dispose()
            ds.Dispose()
        End Try
    End Sub

    Private Sub mnDiscPromo1_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles mnDiscPromo1.ItemClick
        SetDiscPromo(1)
    End Sub

    Private Sub mnDiscPromo2_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles mnDiscPromo2.ItemClick
        SetDiscPromo(2)
    End Sub

    Private Sub mnPDP_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles mnPDP.ItemClick
        SetDiscPromo(3)
    End Sub

    Private Sub frmDaftarBarang_MouseWheel(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles Me.MouseWheel

    End Sub
End Class