
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
Imports System.Xml

Public Class frmDaftarFilterTanggal
    Public FormName As String = ""
    Public FormEntriName As String = ""
    Public TableName As String = ""
    Public TableMaster As String = ""
    Public NoID As Long = -1
    Dim oda2 As SqlDataAdapter
    Dim ds As New DataSet
    Dim BS As New BindingSource

    Dim repckedit As New RepositoryItemCheckEdit
    Dim repdateedit As New RepositoryItemDateEdit
    Dim reptextedit As New RepositoryItemTextEdit
    Dim reppicedit As New RepositoryItemPictureEdit

    Private Sub frmDaftar_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        GV1.SaveLayoutToXml(folderLayouts & FormName & ".xml")
    End Sub

    Private Sub ctrlDaftar_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If TableName = "MJual" Then
            lbCustomer.Visible = True
            lbsalesman.Visible = True
            txtCustomer.Visible = True
            txtSalesman.Visible = True
            refreshPendukung()
        Else
            lbCustomer.Visible = False
            lbsalesman.Visible = False
            txtCustomer.Visible = False
            txtSalesman.Visible = False
        End If
        TglDari.EditValue = TanggalSystem
        TglSampai.EditValue = TanggalSystem

        generateform()
        RestoreLayout()
        Me.lbDaftar.Text = Me.Text
        FungsiControl.SetForm(Me)
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
        Catch ex As Exception
            XtraMessageBox.Show("Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            ds.Dispose()
        End Try
    End Sub
    Private Sub RestoreLayout()
        If System.IO.File.Exists(folderLayouts & FormName & ".xml") Then
            GV1.RestoreLayoutFromXml(folderLayouts & FormName & ".xml")
        End If
    End Sub
    Sub generateform()
        Dim cn As New SqlConnection(StrKonSql)
        Dim ocmd2 As New SqlCommand
        Dim strsql As String = ""
        Dim Cur As Cursor = Windows.Forms.Cursor.Current
        Windows.Forms.Cursor.Current = Cursors.WaitCursor

        strsql = IIf(InStr(TableName, "select", CompareMethod.Text) > 0, TableName, "select * from " & TableName)
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

        Windows.Forms.Cursor.Current = Cur
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

    Private Sub SimpleButton7_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)

    End Sub

    Private Sub SimpleButton2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SimpleButton2.Click
        Edit()
    End Sub

    Private Sub SimpleButton3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SimpleButton3.Click
        Hapus()
    End Sub

    Private Sub SimpleButton1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SimpleButton1.Click
        Baru()
    End Sub
    Sub Baru()
        Select Case FormEntriName
            Case "EntriBarang"
                Dim Brg As New clsBarang
                Brg.FormName = FormEntriName
                Brg.isNew = True
                If Brg.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
                    RefreshData()
                    GV1.FocusedRowHandle = GV1.LocateByDisplayText(0, GV1.Columns("NoID"), NoID.ToString("#,##0"))

                End If
                Brg.Dispose()
            Case "EntriAlamat"
                Dim Kontak As New frmEntriKontak
                Kontak.FormName = FormEntriName
                Kontak.isNew = True
                If Kontak.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
                    RefreshData()
                    GV1.FocusedRowHandle = GV1.LocateByDisplayText(0, GV1.Columns("NoID"), NoID.ToString("#,##0"))
                End If
                Kontak.Dispose()
            Case "EntriPembelian", "EntriReturPembelian"

                Dim Kontak As New FrmEntriPembelian
                If FormEntriName = "EntriPembelian" Then

                    Kontak.TableNameD = "MBeliD"
                Else

                    Kontak.TableNameD = "MReturBeliD"
                End If
                Kontak.FormName = FormEntriName
                Kontak.isNew = True
                If Kontak.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
                    RefreshData()
                    GV1.FocusedRowHandle = GV1.LocateByDisplayText(0, GV1.Columns("NoID"), NoID.ToString("#,##0"))

                End If
                Kontak.Dispose()
            Case Else
                Dim x As New frmSimpleEntri
                x.FormName = FormEntriName
                x.isNew = True
                If x.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
                    RefreshData()
                    GV1.FocusedRowHandle = GV1.LocateByDisplayText(0, GV1.Columns("NoID"), NoID.ToString("#,##0"))

                End If
                x.Dispose()
        End Select
    End Sub
    Sub RefreshData()
        Dim cn As New SqlConnection(StrKonSql)
        Dim ocmd2 As New SqlCommand
        Dim Cur As Cursor = Windows.Forms.Cursor.Current
        Windows.Forms.Cursor.Current = Cursors.WaitCursor
        Dim strsql As String = ""
        strsql = IIf(InStr(TableName, "select", CompareMethod.Text) > 0, TableName, "select * from " & TableName)
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
        Windows.Forms.Cursor.Current = Cur
    End Sub
    Sub Edit()
        Dim x As frmSimpleEntri
        Dim Brg As clsBarang
        Dim Kontak As frmEntriKontak
        Dim Beli As FrmEntriPembelian
        Dim view As ColumnView = GC1.FocusedView
        Try
            Dim row As System.Data.DataRow = view.GetDataRow(GV1.FocusedRowHandle)
            Dim dc As Integer = GV1.FocusedRowHandle
            Dim NoID As Long = NullToLong(row("NoID"))
            If FormEntriName = "EntriBarang" Then
                Brg = New clsBarang
                Brg.FormName = FormEntriName
                Brg.isNew = False
                Brg.NoID = NoID
                If Brg.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
                    RefreshData()
                    GV1.ClearSelection()
                    GV1.FocusedRowHandle = GV1.LocateByDisplayText(0, GV1.Columns("NoID"), NoID.ToString("#,##0"))
                    GV1.SelectRow(GV1.FocusedRowHandle)
                End If
                Brg.Dispose()
            ElseIf FormEntriName = "EntriAlamat" Then
                Kontak = New frmEntriKontak
                Kontak.FormName = FormEntriName
                Kontak.isNew = False
                Kontak.NoID = NoID
                If Kontak.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
                    RefreshData()
                    GV1.ClearSelection()
                    GV1.FocusedRowHandle = GV1.LocateByDisplayText(0, GV1.Columns("NoID"), NoID.ToString("#,##0"))
                    GV1.SelectRow(GV1.FocusedRowHandle)
                End If
                Kontak.Dispose()
            ElseIf FormEntriName = "EntriPembelian" Or FormEntriName = "EntriReturPembelian" Then
                Beli = New FrmEntriPembelian
                Beli.FormName = FormEntriName
                If FormEntriName = "EntriPembelian" Then

                    Beli.TableNameD = "MBeliD"
                Else

                    Beli.TableNameD = "MReturBeliD"
                End If
                Beli.isNew = False
                Beli.NoID = NoID
                If Beli.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
                    RefreshData()
                    GV1.ClearSelection()
                    GV1.FocusedRowHandle = GV1.LocateByDisplayText(0, GV1.Columns("NoID"), NoID.ToString("#,##0"))
                    GV1.SelectRow(GV1.FocusedRowHandle)
                End If
                Beli.Dispose()
            Else
                x = New frmSimpleEntri
                x.FormName = FormEntriName
                x.isNew = False
                x.NoID = NoID
                If x.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
                    RefreshData()
                    GV1.ClearSelection()
                    GV1.FocusedRowHandle = GV1.LocateByDisplayText(0, GV1.Columns("NoID"), NoID.ToString("#,##0"))
                    GV1.SelectRow(GV1.FocusedRowHandle)
                End If
                x.Dispose()
            End If

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
            Dim NoID As Long = NullTolong(row("NoID"))

            If XtraMessageBox.Show("Yakin mau hapus data " & Me.Text & " dengan Kode " & NullTostr(row("Kode")), NamaAplikasi, MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                EksekusiSQL("update " & TableMaster & " set IsActive=0 where NoID= " & NoID.ToString)
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

    Private Sub LabelControl1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LabelControl1.Click
        TglDari.Enabled = Not TglDari.Enabled
        TglSampai.Enabled = TglDari.Enabled
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
    Private Sub mnFaktur_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles mnFaktur.ItemClick
        If frmMain.ckEditRpt.Checked Then
            CetakFaktur(action_.Edit)
        Else
            CetakFaktur(action_.Preview)
        End If
    End Sub

    Private Sub cmdFaktur_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdFaktur.Click
        mnFaktur.PerformClick()
    End Sub

    Private Sub lbCustomer_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lbCustomer.Click
        txtCustomer.Enabled = Not txtCustomer.Enabled

    End Sub

    Private Sub lbsalesman_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lbsalesman.Click
        txtSalesman.Enabled = Not txtSalesman.Enabled
    End Sub
End Class