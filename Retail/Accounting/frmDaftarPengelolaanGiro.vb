Imports System.Data.SqlClient
Imports DevExpress.XtraGrid.Views.Base
Imports DevExpress.XtraEditors
Imports DevExpress.XtraEditors.Repository
Imports DevExpress.XtraGrid.Views.Grid
Imports DevExpress.XtraGrid.Views.Grid.ViewInfo
Imports DevExpress.XtraGrid.Menu
Imports VPoint.mdlCetakCR

Public Class frmDaftarPengelolaanGiro
    Dim NoID As Long
    'Dim IDAlamat As Long
    'Dim IDKasBank As Long
    Dim ISALLOWDBLCLICK As Boolean
    Dim repckedit As New RepositoryItemCheckEdit
    Dim repdateedit As New RepositoryItemDateEdit
    Dim reptextedit As New RepositoryItemTextEdit
    Dim reppicedit As New RepositoryItemPictureEdit

    Sub TampilData()
        Dim oConn As New SqlConnection
        Dim ocmd As SqlCommand
        Dim oDA As SqlDataAdapter
        Dim oDS As New DataSet
        Dim strsql As String
        'Dim isPerluWhere As Boolean
        'Dim isAda As Boolean
        Dim dlg As DevExpress.Utils.WaitDialogForm = Nothing
        Dim curentcursor As Cursor = Windows.Forms.Cursor.Current
        Windows.Forms.Cursor.Current = Cursors.WaitCursor
        dlg = New DevExpress.Utils.WaitDialogForm(String.Format("Sedang merefresh data.{0}MOHON TUNGGU ...", vbCrLf), Application.ProductName)
        dlg.TopMost = False
        dlg.Show()
        Windows.Forms.Cursor.Current = Cursors.WaitCursor
        Dim CurrentPotition As Long
        Try
            If Not ckLunas.Checked Then
                strsql = "SELECT MBayarHutangDGiro.*, MAkun.Kode + ' - ' + MAkun.Nama AS AkunBank, MBank.Kode + ' - ' + MBank.Nama AS Bank,  MAlamat.Kode AS KodeSupplier, MAlamat.Nama AS Supplier, MBayarHutang.Kode, MBayarHutang.Tanggal, MBayarHutang.TglKembali AS [Jatuh Tempo Pelunasan] " & vbCrLf & _
                         " FROM MBayarHutangDGiro " & vbCrLf & _
                         " INNER JOIN MBayarHutang ON MBayarHutangDGiro.IDBayarHutang=MBayarHutang.NoID " & vbCrLf & _
                         " LEFT JOIN MAlamat ON MAlamat.NoID=MBayarHutang.IDAlamat " & vbCrLf & _
                         " LEFT JOIN (MBank LEFT JOIN MAkun ON MAkun.ID=MBank.IDAkun) ON MBank.NoID=MBayarHutangDGiro.IDBank " & vbCrLf & _
                         " WHERE IsNull(MBayarHutangDGiro.IsCair,0)=0 "
            Else
                strsql = "SELECT MBayarHutangDGiro.*, MAkun.Kode + ' - ' + MAkun.Nama AS AkunBank, MBank.Kode + ' - ' + MBank.Nama AS Bank,  MAlamat.Kode AS KodeSupplier, MAlamat.Nama AS Supplier, MBayarHutang.Kode, MBayarHutang.Tanggal, MBayarHutang.TglKembali AS [Jatuh Tempo Pelunasan] " & vbCrLf & _
                         " FROM MBayarHutangDGiro " & vbCrLf & _
                         " INNER JOIN MBayarHutang ON MBayarHutangDGiro.IDBayarHutang=MBayarHutang.NoID " & vbCrLf & _
                         " LEFT JOIN MAlamat ON MAlamat.NoID=MBayarHutang.IDAlamat " & vbCrLf & _
                         " LEFT JOIN (MBank LEFT JOIN MAkun ON MAkun.ID=MBank.IDAkun) ON MBank.NoID=MBayarHutangDGiro.IDBank " & vbCrLf & _
                         " WHERE (IsNull(MBayarHutangDGiro.IsCair,0)=0 OR IsNull(MBayarHutangDGiro.IsCair,0)=1) "
            End If
            If txtAlamat.Text <> "" Then
                strsql &= " AND MBayarHutang.IDAlamat=" & NullToLong(txtAlamat.EditValue)
            End If
            If txtKasBank.Text <> "" Then
                strsql &= " AND MBayarHutangDGiro.IDBank=" & NullToLong(txtKasBank.EditValue)
            End If
            If tglDari.Enabled = True Then
                strsql &= " AND MBayarHutangDGiro.Tanggal>='" & Format(tglDari.DateTime, "yyyy/MM/dd") & "' AND MBayarHutangDGiro.Tanggal<'" & Format(TglSampai.DateTime.AddDays(1), "yyyy/MM/dd") & "' "
            End If

            oConn.ConnectionString = StrKonSql
            ocmd = New SqlCommand(strsql, oConn)
            oConn.Open()
            oDA = New SqlDataAdapter(ocmd)
            oDA.Fill(oDS, "MBayarHutang")

            CurrentPotition = GridView1.FocusedRowHandle
            GridControl1.DataSource = oDS.Tables("MBayarHutang")
            GridView1.OptionsView.ShowFooter = True
            GridView1.ClearSelection()
            GridView1.FocusedRowHandle = CurrentPotition
            GridView1.SelectRow(GridView1.FocusedRowHandle)
        Catch ex As Exception
            XtraMessageBox.Show(Me, ex.Message, ".:: Pesan Kesalahan ::.", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            oConn.Close()
            If Not dlg Is Nothing Then
                dlg.Close()
                dlg.Dispose()
            End If
            Windows.Forms.Cursor.Current = curentcursor
            Application.DoEvents()
        End Try
    End Sub

    Private Sub DateEdit2_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TglSampai.EditValueChanged

    End Sub

    Private Sub LabelControl3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LabelControl3.Click
        tglDari.Enabled = Not tglDari.Enabled
        TglSampai.Enabled = tglDari.Enabled
        TampilData()
    End Sub

    Private Sub ButtonEdit1_ButtonClick(ByVal sender As Object, ByVal e As DevExpress.XtraEditors.Controls.ButtonPressedEventArgs) Handles txtAlamat.ButtonClick
        'Select Case e.Button.Index
        '    Case 0
        '        Dim x As New frLUAlamat
        '        x.IsCustomer = True
        '        If x.ShowDialog(Me)= Windows.Forms.DialogResult.OK Then
        '            IDAlamat = x.NoID
        '            txtAlamat.Text = x.Nama & " - " & x.Kontak
        '        End If
        '        x.Dispose()
        '    Case 1
        '        IDAlamat = -1
        '        txtAlamat.Text = ""
        'End Select
        'TampilData()
    End Sub

    Private Sub txtKasBank_ButtonClick(ByVal sender As Object, ByVal e As DevExpress.XtraEditors.Controls.ButtonPressedEventArgs) Handles txtKasBank.ButtonClick
        'Select Case e.Button.Index
        '    Case 0
        '        Dim x As New frLUKasBank
        '        If x.ShowDialog(Me)= Windows.Forms.DialogResult.OK Then
        '            IDKasBank = x.NoID
        '            txtKasBank.Text = x.Nama '& " - " & x.SubKlas
        '        End If
        '        x.Dispose()
        '    Case 1
        '        IDKasBank = -1
        '        txtKasBank.Text = ""
        'End Select
        'TampilData()
    End Sub
    Private Sub RefreshLookUp()
        Dim ds As New DataSet
        Dim SQL As String = ""
        Try
            SQL = "SELECT NoID, Kode, Nama, Alamat FROM MAlamat WHERE IsActive=1 "
            ds = ExecuteDataset("MAlamat", SQL)
            txtAlamat.Properties.DataSource = ds.Tables("MAlamat")
            txtAlamat.Properties.ValueMember = "NoID"
            txtAlamat.Properties.DisplayMember = "Nama"
            LabelControl1.Text = "Supplier"

            SQL = "SELECT NoID, Kode, Nama FROM MBank WHERE IsActive=1 "
            ds = ExecuteDataset("MAlamat", SQL)
            txtKasBank.Properties.DataSource = ds.Tables("MAlamat")
            txtKasBank.Properties.ValueMember = "NoID"
            txtKasBank.Properties.DisplayMember = "Nama"
        Catch ex As Exception
            XtraMessageBox.Show("Info : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
        Finally
            ds.Dispose()
        End Try
    End Sub

    Private Sub frmDaftarBayarHutang_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim dlg As DevExpress.Utils.WaitDialogForm = Nothing
        Try
            Dim curentcursor As Cursor = Windows.Forms.Cursor.Current
            Windows.Forms.Cursor.Current = Cursors.WaitCursor
            dlg = New DevExpress.Utils.WaitDialogForm(String.Format("Creating component and analize database.{0}MOHON TUNGGU ...", vbCrLf), Application.ProductName)
            dlg.TopMost = False
            dlg.Show()
            Windows.Forms.Cursor.Current = Cursors.WaitCursor
            tglDari.EditValue = TanggalSystem
            TglSampai.EditValue = TanggalSystem
            RefreshLookUp()
            TampilData()
            RefreshLookUp()

            Me.lbDaftar.Text = Me.Text
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
            If System.IO.File.Exists(FolderLayouts & Me.Name & GridView1.Name & ".xml") Then
                GridView1.RestoreLayoutFromXml(FolderLayouts & Me.Name & GridView1.Name & ".xml")
            End If
            FungsiControl.SetForm(Me)
        Catch ex As Exception
            XtraMessageBox.Show(ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            Windows.Forms.Cursor.Current = Cursors.Default
            dlg.Close()
            dlg.Dispose()
        End Try
    End Sub

    Private Sub txtAlamat_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtAlamat.EditValueChanged
        cmdRefresh.PerformClick()
    End Sub

    Private Sub GridView1_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles GridView1.DoubleClick
        If ISALLOWDBLCLICK Then
            cmdEdit.PerformClick()
        End If
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
        If (e.Button = Windows.Forms.MouseButtons.Right) And (hitInfo.InRow) And _
          (Not View.IsGroupRow(hitInfo.RowHandle)) Then
            ISALLOWDBLCLICK = True
        Else
            ISALLOWDBLCLICK = False
        End If
    End Sub

    Private Sub SimpleButton8_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdRefresh.Click
        TampilData()
    End Sub

    Private Sub SimpleButton1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdBaru.Click
        'If IsHutang Then
        '    NewHutang()
        'Else
        '    NewPiutang()
        'End If
    End Sub

    Private Sub SimpleButton4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdExcel.Click
        ExportExcel()
    End Sub
    Sub ExportExcel()
        Dim dlgsave As New SaveFileDialog
        dlgsave.Title = "Export Daftar ke Excel"
        dlgsave.Filter = "Excel Files|*.xls"
        If dlgsave.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
            GridView1.ExportToXls(dlgsave.FileName)
            BukaFile(dlgsave.FileName)
        End If
        dlgsave.Dispose()
    End Sub
    Private Sub SimpleButton3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdHapus.Click
        'Try
        '    Dim view As ColumnView = GridControl1.FocusedView
        '    Dim row As System.Data.DataRow = view.GetDataRow(GridView1.FocusedRowHandle)
        '    NoID = NullToLong(row("NoID"))
        '    If NullToBool(row("IsPosted")) = False Then
        '        If XtraMessageBox.Show("Apakah anda mau menghapus Nota no: " & NullToStr(row("Kode")) & " ini?", NamaAplikasi, MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
        '            'If EksekusiSQL("Delete From MBayarHutangD where IDBayarHutang=" & NoID) Then
        '            If TypePembayaran = TypePembayaran_.Komplit Then
        '                EksekusiSQL("DELETE FROM MBayarHutangD WHERE IDBayarHutang=" & NoID)
        '                EksekusiSQL("DELETE FROM MBayarHutang WHERE NoID=" & NoID)
        '                EksekusiSQL("DELETE FROM MBayarHutangDRetur WHERE IDBayarHutang=" & NoID)
        '                EksekusiSQL("DELETE FROM MBayarHutangDPH WHERE IDBayarHutang=" & NoID)
        '                EksekusiSQL("DELETE FROM MBayarHutangDDebet WHERE IDBayarHutang=" & NoID)
        '                EksekusiSQL("DELETE FROM MBayarHutangDKredit WHERE IDBayarHutang=" & NoID)
        '                EksekusiSQL("DELETE FROM MBayarHutangDGiro WHERE IDBayarHutang=" & NoID)
        '            Else
        '                EksekusiSQL("DELETE FROM MBayarHutangD WHERE IDBayarHutang=" & NoID)
        '                EksekusiSQL("DELETE FROM MBayarHutang WHERE NoID=" & NoID)
        '            End If
        '            TampilData()
        '            'End If
        '        End If
        '    End If
        'Catch ex As Exception
        '    XtraMessageBox.Show(Me, ex.Message, ".:: Pesan Kesalahan ::.", MessageBoxButtons.OK, MessageBoxIcon.Error)
        'End Try
    End Sub

    Private Sub SimpleButton2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdEdit.Click
        EditGiro()
    End Sub

    Private Sub EditGiro()
        Dim x As New frmSimpleEntri
        Try
            NoID = NullToLong(GridView1.GetRowCellValue(GridView1.FocusedRowHandle, "NoID"))
            x.FormName = "EntriGiro"
            x.isNew = False
            x.NoID = NoID
            If x.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
                TampilData()
                GridView1.ClearSelection()
                GridView1.FocusedRowHandle = GridView1.LocateByDisplayText(0, GridView1.Columns("NoID"), NoID.ToString("#,##0"))
                GridView1.SelectRow(GridView1.FocusedRowHandle)
            End If
            x.Dispose()
        Catch ex As Exception
            XtraMessageBox.Show("Info Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
        Finally
            x.Dispose()
        End Try
    End Sub

    Private Sub SimpleButton6_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdTutup.Click
        DialogResult = Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub mnPosting_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles mnPosting.ItemClick
        'Try
        '    Dim view As ColumnView = GridControl1.FocusedView
        '    Dim I As Integer
        '    Dim row As System.Data.DataRow
        '    For I = 0 To view.SelectedRowsCount - 1
        '        row = view.GetDataRow(view.GetSelectedRows(I))
        '        NoID = NullToLong(row("NoID"))
        '        If NullToBool(row("IsPosted")) = False Then
        '            EksekusiSQL("UPDATE MBayarHutang SET IsPosted=1 where NoID=" & NoID)
        '        End If
        '    Next
        '    TampilData()
        'Catch ex As Exception
        '    XtraMessageBox.Show(Me, ex.Message, ".:: Pesan Kesalahan ::.", MessageBoxButtons.OK, MessageBoxIcon.Error)
        'End Try
    End Sub

    Private Sub mnUnPosting_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles mnUnPosting.ItemClick
        'Try
        '    Dim view As ColumnView = GridControl1.FocusedView
        '    Dim I As Integer
        '    Dim row As System.Data.DataRow
        '    For I = 0 To view.SelectedRowsCount - 1
        '        row = view.GetDataRow(view.GetSelectedRows(I))
        '        NoID = NullToLong(row("NoID"))
        '        If NullToBool(row("IsPosted")) = True Then
        '            EksekusiSQL("UPDATE MBayarHutang SET IsPosted=0 where NoID=" & NoID)
        '        End If
        '    Next
        '    TampilData()
        'Catch ex As Exception
        '    XtraMessageBox.Show(Me, ex.Message, ".:: Pesan Kesalahan ::.", MessageBoxButtons.OK, MessageBoxIcon.Error)
        'End Try
    End Sub

    Private Sub BarButtonItem1_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem1.ItemClick
        cmdBaru.PerformClick()
    End Sub

    Private Sub BarButtonItem2_ItemClick(ByVal sender As Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem2.ItemClick
        cmdEdit.PerformClick()
    End Sub

    Private Sub BarButtonItem3_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem3.ItemClick
        cmdHapus.PerformClick()
    End Sub

    Private Sub BarButtonItem5_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem5.ItemClick
        cmdExcel.PerformClick()
    End Sub

    Private Sub BarButtonItem6_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem6.ItemClick
        cmdPreview.PerformClick()
    End Sub

    Private Sub mnRefresh_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles mnRefresh.ItemClick
        cmdRefresh.PerformClick()
    End Sub

    Private Sub BarButtonItem4_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem4.ItemClick
        cmdTutup.PerformClick()
    End Sub

    Private Sub mnSaveLayout_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles mnSaveLayout.ItemClick
        Dim xOtorisasi As New frmOtorisasiAdmin
        Try
            If XtraMessageBox.Show("Simpan Layout?", NamaAplikasi, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = Windows.Forms.DialogResult.Yes AndAlso xOtorisasi.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
                GridView1.SaveLayoutToXml(FolderLayouts & Me.Name & GridView1.Name & ".xml")

                gvKasBank.SaveLayoutToXml(FolderLayouts & Me.Name & gvKasBank.Name & ".xml")
                gvSupplier.SaveLayoutToXml(FolderLayouts & Me.Name & gvSupplier.Name & ".xml")
            End If
        Catch ex As Exception
            XtraMessageBox.Show("Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            xOtorisasi.Dispose()
        End Try
    End Sub

    Private Sub mnFaktur_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles mnFaktur.ItemClick
        cmdFaktur.PerformClick()
    End Sub

    Private Sub mnFakturPanjang_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles mnFakturPanjang.ItemClick
        cmdFakturPanjang.PerformClick()
    End Sub

    Private Sub txtKasBank_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtKasBank.EditValueChanged
        cmdRefresh.PerformClick()
    End Sub

    Private Sub gvKasBank_DataSourceChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvKasBank.DataSourceChanged
        If System.IO.File.Exists(FolderLayouts & Me.Name & gvKasBank.Name & ".xml") Then
            gvKasBank.RestoreLayoutFromXml(FolderLayouts & Me.Name & gvKasBank.Name & ".xml")
        End If
        With gvKasBank
            For x As Integer = 0 To .Columns.Count - 1
                Select Case .Columns(x).ColumnType.Name.ToLower
                    Case "int32", "int64", "int"
                        .Columns(x).DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
                        .Columns(x).DisplayFormat.FormatString = "n0"
                    Case "decimal", "single", "money", "double"
                        .Columns(x).DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
                        .Columns(x).DisplayFormat.FormatString = "n2"
                    Case "string"
                        .Columns(x).DisplayFormat.FormatType = DevExpress.Utils.FormatType.None
                        .Columns(x).DisplayFormat.FormatString = ""
                    Case "date", "datetime"
                        If .Columns(x).FieldName.Trim.ToLower = "jam" Then
                            .Columns(x).DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime
                            .Columns(x).DisplayFormat.FormatString = "HH:mm"
                        Else
                            .Columns(x).DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime
                            .Columns(x).DisplayFormat.FormatString = "dd-MM-yyyy"
                        End If
                End Select
            Next
        End With
    End Sub

    Private Sub gvSupplier_DataSourceChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvSupplier.DataSourceChanged
        If System.IO.File.Exists(FolderLayouts & Me.Name & gvSupplier.Name & ".xml") Then
            gvSupplier.RestoreLayoutFromXml(FolderLayouts & Me.Name & gvSupplier.Name & ".xml")
        End If
        With gvSupplier
            For x As Integer = 0 To .Columns.Count - 1
                Select Case .Columns(x).ColumnType.Name.ToLower
                    Case "int32", "int64", "int"
                        .Columns(x).DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
                        .Columns(x).DisplayFormat.FormatString = "n0"
                    Case "decimal", "single", "money", "double"
                        .Columns(x).DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
                        .Columns(x).DisplayFormat.FormatString = "n2"
                    Case "string"
                        .Columns(x).DisplayFormat.FormatType = DevExpress.Utils.FormatType.None
                        .Columns(x).DisplayFormat.FormatString = ""
                    Case "date", "datetime"
                        If .Columns(x).FieldName.Trim.ToLower = "jam" Then
                            .Columns(x).DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime
                            .Columns(x).DisplayFormat.FormatString = "HH:mm"
                        Else
                            .Columns(x).DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime
                            .Columns(x).DisplayFormat.FormatString = "dd-MM-yyyy"
                        End If
                End Select
            Next
        End With
    End Sub
    Private Sub CetakFaktur(ByVal action As action_)
        Dim namafile As String
        Dim view As ColumnView = GridControl1.FocusedView
        Try
            namafile = Application.StartupPath & "\report\FakturMBayarHutangDGiro.rpt"
            If System.IO.File.Exists(namafile) Then
                Dim row As System.Data.DataRow = view.GetDataRow(GridView1.FocusedRowHandle)
                Dim dc As Integer = GridView1.FocusedRowHandle
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
                ViewReport(Me.ParentForm, action, namafile, Me.Text, "{MBayarHutangDGiro.NoID}=" & NoID & " AND {MBayarHutangDGiro.IsCair}=True ")
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
        Dim view As ColumnView = GridControl1.FocusedView
        Try
            namafile = Application.StartupPath & "\report\FakturPanjangMBayarHutangDGiro.rpt"
            If System.IO.File.Exists(namafile) Then
                Dim row As System.Data.DataRow = view.GetDataRow(GridView1.FocusedRowHandle)
                Dim dc As Integer = GridView1.FocusedRowHandle
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
                ViewReport(Me.ParentForm, action, namafile, Me.Text, "{MBayarHutangDGiro.NoID}=" & NoID & " AND {MBayarHutangDGiro.IsCair}=True ")
            Else
                DevExpress.XtraEditors.XtraMessageBox.Show("Nama File :" & namafile & " tidak ditemukan.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Stop)
            End If
        Catch EX As Exception
            XtraMessageBox.Show("Untuk mencetak, pilih item yang mau dicetak dan click cetak faktur.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
            'DevExpress.XtraEditors.XtraMessageBox.Show("Kesalahan:" & EX.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub
    Private Sub cmdFaktur_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdFaktur.Click
        If frmMain.ckEditRpt.Checked Then
            CetakFaktur(action_.Edit)
        ElseIf frmMain.ckLangsungCetak.Checked Then
            CetakFaktur(action_.Print)
        Else
            CetakFaktur(action_.Preview)
        End If
    End Sub

    Private Sub cmdFakturPanjang_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdFakturPanjang.Click
        If frmMain.ckEditRpt.Checked Then
            CetakFakturPanjang(action_.Edit)
        ElseIf frmMain.ckLangsungCetak.Checked Then
            CetakFakturPanjang(action_.Print)
        Else
            CetakFakturPanjang(action_.Preview)
        End If
    End Sub

    Private Sub cmdPreview_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdPreview.Click
        'Dim NamaFile As String = Application.StartupPath & "\Report\DaftarBayarHutang.rpt"
        'Try
        '    If EditReport Then
        '        ViewReport(Me.MdiParent, action_.Edit, NamaFile, "Daftar Pembayaran Hutang", , , "TglDari=cdate(" & tglDari.DateTime.ToString("yyyy,MM,dd") & ")&TglSampai=cdate(" & TglSampai.DateTime.ToString("yyyy,MM,dd") & ")&IDSupplier=" & IIf(txtAlamat.Text = "", -1, NullToLong(txtAlamat.EditValue)))
        '    Else
        '        ViewReport(Me.MdiParent, action_.Preview, NamaFile, "Daftar Pembayaran Hutang", , , "TglDari=cdate(" & tglDari.DateTime.ToString("yyyy,MM,dd") & ")&TglSampai=cdate(" & TglSampai.DateTime.ToString("yyyy,MM,dd") & ")&IDSupplier=" & IIf(txtAlamat.Text = "", -1, NullToLong(txtAlamat.EditValue)))
        '    End If
        'Catch ex As Exception
        '    XtraMessageBox.Show("Info Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
        'End Try
        GridControl1.ShowPrintPreview()
    End Sub

    Private Sub GridControl1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles GridControl1.Click

    End Sub
End Class