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
Imports DevExpress.Utils
Imports DevExpress.XtraGrid.Views.Grid.ViewInfo
Imports DevExpress.XtraGrid.Views.Grid

Public Class frmDaftarUser
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

    Private Sub frmDaftarUser_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Activated
        If ShowNoID Then
            RefreshData()
            GV1.ClearSelection()
            GV1.FocusedRowHandle = GV1.LocateByDisplayText(0, GV1.Columns("NoID"), (DirectNoID).ToString("#,##0"))
            GV1.SelectRow(GV1.FocusedRowHandle)
            ShowNoID = False
            'If KirimDataKassa() Then
            '    XtraMessageBox.Show("Pengiriman berhasil.", NamaAplikasi, MessageBoxButtons.OK)
            'End If
        End If
    End Sub
    Private Sub frmDaftar_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        GV1.SaveLayoutToXml(folderLayouts & Me.Name & GV1.Name & ".xml")
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

            FormName = "Daftar User"
            TableName = "MUser"
            Text = "Daftar User"
            FormEntriName = "EntriMUser"
            TableMaster = "MUser"

            Me.lbDaftar.Text = Me.Text
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
        If System.IO.File.Exists(folderLayouts & Me.Name & GV1.Name & ".xml") Then
            GV1.RestoreLayoutFromXml(folderLayouts & Me.Name & GV1.Name & ".xml")
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
    End Sub

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
        If NullTobool(Ini.BacaIni("Application", "UseFramework", False)) Then
            Edit()
        Else
            Dim view As ColumnView = GC1.FocusedView
            Dim x As New frmEntriUser
            Try
                Dim row As System.Data.DataRow = view.GetDataRow(GV1.FocusedRowHandle)
                Dim dc As Integer = GV1.FocusedRowHandle
                Dim IDDetil As Long = NullTolong(row("NoID"))
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
                XtraMessageBox.Show("Error : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End If

    End Sub

    Private Sub SimpleButton3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SimpleButton3.Click
        Hapus()
    End Sub

    Private Sub SimpleButton1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SimpleButton1.Click
        If NullTobool(Ini.BacaIni("Application", "UseFramework", False)) Then
            Baru()
        Else
            Dim view As ColumnView = GC1.FocusedView
            Dim x As New frmEntriUser
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
        End If
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
            If frENTRI.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
                RefreshData()
                GV1.FocusedRowHandle = GV1.LocateByDisplayText(0, GV1.Columns("NoID"), NoID.ToString("#,##0"))

            End If
            frENTRI.Dispose()
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
            strsql = "SELECT MUser.NoID, MUser.Kode, MUser.Nama, MUser.IsSupervisor, MUser.IsKasir, MUser.IsAutoPosting, MUser.IsEditLayout, MUser.IDGudangDefault, "
            strsql &= " MUser.IDAlamat, MUser.IDSupplier, MUser.IDPelanggan, MUser.IDBarang, MUser.IDSatuan, MUser.IsActive, "
            strsql &= " MUser.Keterangan, MGudang.Nama AS Gudang, MWilayah.Nama AS Wilayah, MDepartemen.Nama AS Departemen, "
            strsql &= " MSatuan.Nama AS Satuan, MPegawai.Nama AS Pegawai, MPegawai.Alamat AS AlamatPegawai, MCustomer.Nama AS Customer, MCustomer.Alamat AS AlamatCustomer, MSupplier.Nama AS Supplier, MSupplier.Alamat AS AlamatSupplier, MUser.IsAccMutasi, MUser.IsReminderHutang AS PengingatHutang, MUser.IsAccMutasi AS BagAccounting "
            strsql &= " FROM MUser " & vbCrLf
            strsql &= " LEFT OUTER JOIN (MGudang LEFT JOIN (MWilayah LEFT JOIN MDepartemen ON MDepartemen.NoID=MWilayah.IDDepartemen) ON MWilayah.NoID=MGudang.IDWilayah) ON MUser.IDGudangDefault = MGudang.NoID" & vbCrLf
            strsql &= " LEFT OUTER JOIN MSatuan ON MSatuan.NoID=MUser.IDSatuan" & vbCrLf
            strsql &= " LEFT OUTER JOIN MAlamat MPegawai ON MPegawai.NoID=MUser.IDAlamat" & vbCrLf
            strsql &= " LEFT OUTER JOIN MAlamat MSupplier ON MSupplier.NoID=MUser.IDSupplier" & vbCrLf
            strsql &= " LEFT OUTER JOIN MAlamat MCustomer ON MCustomer.NoID=MUser.IDPelanggan" & vbCrLf
            strsql &= " LEFT OUTER JOIN MBarang ON MBarang.NoID=MUser.IDBarang" & vbCrLf
            If KodeUserAktif.ToUpper = "ADM".ToUpper Then
            Else
                strsql &= " WHERE MUser.Kode<>'ADM' " & vbCrLf
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
            If XtraMessageBox.Show("Yakin mau hapus data User dengan Nama " & NullToStr(GV1.GetRowCellValue(GV1.FocusedRowHandle, "Nama")), NamaAplikasi, MessageBoxButtons.YesNo) = Windows.Forms.DialogResult.Yes Then
                EksekusiSQL("DELETE FROM MUser WHERE NoID= " & NoID.ToString)
                EksekusiSQL("DELETE FROM MUserD WHERE IDUser= " & NoID.ToString)
                EksekusiSQL("DELETE FROM MUserDAksesGudang WHERE IDUser= " & NoID.ToString)
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

    Private Sub mnRefresh_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles mnRefresh.ItemClick
        SimpleButton8.PerformClick()
    End Sub
End Class