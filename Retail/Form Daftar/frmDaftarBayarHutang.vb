Imports System.Data.SqlClient
Imports DevExpress.XtraGrid.Views.Base
Imports DevExpress.XtraEditors
Imports DevExpress.XtraEditors.Repository
Imports DevExpress.XtraGrid.Views.Grid
Imports DevExpress.XtraGrid.Views.Grid.ViewInfo
Imports DevExpress.XtraGrid.Menu
Imports VPoint.mdlCetakCR

Public Class frmDaftarBayarHutang
    Dim NoID As Long
    'Dim IDAlamat As Long
    'Dim IDKasBank As Long
    Dim ISALLOWDBLCLICK As Boolean
    Public IsHutang As Boolean = True
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
        Dim isPerluWhere As Boolean
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
            If IsHutang Then
                'strsql = "Select MBayarHutang.*, MBayarHutang.Catatan AS NoBPK, MTT.Kode AS NoTT, MBank.Kode as KodeKas, MBank.Nama as KasBank, MAlamat.Nama as Supplier, MAlamat.Kode as KodeSupplier, Malamat.Alamat, MKwitansi.NoKwitansi " & _
                '         " FROM ((MBayarHutang LEFT JOIN MAlamat ON MBayarHutang.IDAlamat=MAlamat.NoID) LEFT JOIN MKasIN MKwitansi ON MKwitansi.ID=MBayarHutang.IDKwitansi " & _
                '         " LEFT JOIN MBank On MBayarHutang.IDAkunKas=MBank.NoID) LEFT JOIN MTT ON MTT.NoID=MBayarHutang.IDTT Where (MBayarHutang.IsJual=0 OR MBayarHutang.IsJual IS NULL) "
                strsql = "SELECT MBayarHutang.*, MBayarHutang.Catatan AS NoBPK, MAkun.Kode + ' - ' + MAkun.Nama AS AkunKas, MTT.Kode AS NoTT, MBank.Kode AS KodeBank, MBank.Nama AS KasBank, MAlamat.Nama as Supplier, MAlamat.Kode as KodeSupplier, Malamat.Alamat, MKwitansi.NoKwitansi " & _
                         " FROM ((MBayarHutang LEFT JOIN MAlamat ON MBayarHutang.IDAlamat=MAlamat.NoID) LEFT JOIN MKasIN MKwitansi ON MKwitansi.ID=MBayarHutang.IDKwitansi) " & _
                         " LEFT JOIN MTT ON MTT.NoID=MBayarHutang.IDTT " & vbCrLf & _
                         " LEFT JOIN MBank ON MBank.NoID=MBayarHutang.IDBank " & vbCrLf & _
                         " LEFT JOIN MAkun ON MAkun.ID=MBayarHutang.IDAkunKas " & vbCrLf & _
                         " WHERE IsNull(MBayarHutang.IsJual,0)=0 "
            Else
                'strsql = "Select MBayarHutang.*, MTT.Kode AS NoTT, MBank.Kode as KodeKas, MBank.Nama as KasBank, MAlamat.Nama as Supplier, MAlamat.Kode as KodeSupplier, Malamat.Alamat, MKwitansi.NoKwitansi " & _
                '         " FROM ((MBayarHutang LEFT JOIN MAlamat ON MBayarHutang.IDAlamat=MAlamat.NoID) LEFT JOIN MKasIN MKwitansi ON MKwitansi.ID=MBayarHutang.IDKwitansi " & _
                '         " LEFT JOIN MBank On MBayarHutang.IDAkunKas=MBank.NoID) LEFT JOIN MTT ON MTT.NoID=MBayarHutang.IDTT Where (MBayarHutang.IsJual=1) "
                strsql = "SELECT MBayarHutang.*, MAkun.Kode + ' - ' + MAkun.Nama AS AkunKas, MTT.Kode AS NoTT, MBank.Kode AS KodeBank, MBank.Nama AS KasBank, MAlamat.Nama as Supplier, MAlamat.Kode as KodeSupplier, Malamat.Alamat, MKwitansi.NoKwitansi " & _
                         " FROM ((MBayarHutang LEFT JOIN MAlamat ON MBayarHutang.IDAlamat=MAlamat.NoID) LEFT JOIN MKasIN MKwitansi ON MKwitansi.ID=MBayarHutang.IDKwitansi) " & _
                         " LEFT JOIN MTT ON MTT.NoID=MBayarHutang.IDTT " & vbCrLf & _
                         " LEFT JOIN MBank ON MBank.NoID=MBayarHutang.IDBank " & vbCrLf & _
                         " LEFT JOIN MAkun ON MAkun.ID=MBayarHutang.IDAkunKas " & vbCrLf & _
                         " WHERE IsNull(MBayarHutang.IsJual,0)=1 "
            End If
            isPerluWhere = False
            If txtAlamat.Text <> "" Then
                If isPerluWhere Then
                    strsql = strsql & " WHERE MBayarHutang.IDAlamat=" & NullToLong(txtAlamat.EditValue)
                Else
                    strsql = strsql & " AND MBayarHutang.IDAlamat=" & NullToLong(txtAlamat.EditValue)
                End If
                isPerluWhere = False
            End If
            If txtKasBank.Text <> "" Then
                If isPerluWhere Then
                    strsql = strsql & " WHERE IDAKunKas=" & NullToLong(txtKasBank.EditValue)
                Else
                    strsql = strsql & " AND IDAKunKas=" & NullToLong(txtKasBank.EditValue)
                End If
                isPerluWhere = False
            End If
            If tglDari.Enabled = True Then
                If isPerluWhere Then
                    strsql = strsql & " WHERE MBayarHutang.Tanggal>='" & Format(tglDari.DateTime, "yyyy/MM/dd") & "' and  MBayarHutang.Tanggal<'" & Format(TglSampai.DateTime.AddDays(1), "yyyy/MM/dd") & "' "
                Else
                    strsql = strsql & " AND MBayarHutang.Tanggal>='" & Format(tglDari.DateTime, "yyyy/MM/dd") & "' and  MBayarHutang.Tanggal<'" & Format(TglSampai.DateTime.AddDays(1), "yyyy/MM/dd") & "' "
                End If
                isPerluWhere = False
            End If
            If TglKembaliDari.Enabled = True Then
                If isPerluWhere Then
                    strsql = strsql & " WHERE MBayarHutang.TglKembali>='" & Format(TglKembaliDari.DateTime, "yyyy/MM/dd") & "' and  MBayarHutang.TglKembali<'" & Format(TglKembaliSampai.DateTime.AddDays(1), "yyyy/MM/dd") & "' "
                Else
                    strsql = strsql & " AND MBayarHutang.TglKembali>='" & Format(TglKembaliDari.DateTime, "yyyy/MM/dd") & "' and  MBayarHutang.TglKembali<'" & Format(TglKembaliSampai.DateTime.AddDays(1), "yyyy/MM/dd") & "' "
                End If
                isPerluWhere = False
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
            If IsHutang Then
                SQL = "SELECT NoID, Kode, Nama, Alamat FROM MAlamat WHERE IsSupplier=1 "
                ds = ExecuteDataset("MAlamat", SQL)
                txtAlamat.Properties.DataSource = ds.Tables("MAlamat")
                txtAlamat.Properties.ValueMember = "NoID"
                txtAlamat.Properties.DisplayMember = "Nama"
                LabelControl1.Text = "Supplier"
            Else
                SQL = "SELECT NoID, Kode, Nama, Alamat FROM MAlamat WHERE IsCustomer=1 "
                ds = ExecuteDataset("MAlamat", SQL)
                txtAlamat.Properties.DataSource = ds.Tables("MAlamat")
                txtAlamat.Properties.ValueMember = "NoID"
                txtAlamat.Properties.DisplayMember = "Nama"
                LabelControl1.Text = "Customer"
            End If
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
            TglKembaliDari.EditValue = TanggalSystem
            TglKembaliSampai.EditValue = TanggalSystem
            RefreshLookUp()
            TampilData()
            RefreshLookUp()
            If IsHutang Then
                Me.Text = "Daftar Pembayaran Hutang"
            Else
                Me.Text = "Daftar Pembayaran Piutang"
            End If
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
            If System.IO.File.Exists(folderLayouts & Me.Name & GridView1.Name & IsHutang & ".xml") Then
                GridView1.RestoreLayoutFromXml(folderLayouts & Me.Name & GridView1.Name & IsHutang & ".xml")
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

    Private Sub GridView1_CellValueChanged(ByVal sender As Object, ByVal e As DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs) Handles GridView1.CellValueChanged
        Dim cn As New SqlConnection
        Dim com As New SqlCommand
        Try
            If CheckEdit1.Checked AndAlso GridView1.FocusedColumn.FieldName.ToUpper = "NoBPK".ToUpper Then
                cn.ConnectionString = StrKonSql
                cn.Open()
                com.Connection = cn
                com.CommandText = "UPDATE MBayarHutang SET Catatan='" & FixApostropi(NullToStr(e.Value)) & "', Keterangan='" & FixApostropi(NullToStr(e.Value)) & "' WHERE NoID=" & NullToLong(GridView1.GetRowCellValue(e.RowHandle, "NoID"))
                com.ExecuteNonQuery()
            End If
        Catch ex As Exception
            XtraMessageBox.Show("Info Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
        Finally
            If cn.State = ConnectionState.Open Then
                cn.Close()
            End If
            cn.Dispose()
            com.Dispose()
        End Try
    End Sub

    Private Sub GridView1_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles GridView1.DoubleClick
        If ISALLOWDBLCLICK Then
            cmdEdit.PerformClick()
        End If
    End Sub

    Private Sub GridView1_FocusedColumnChanged(ByVal sender As Object, ByVal e As DevExpress.XtraGrid.Views.Base.FocusedColumnChangedEventArgs) Handles GridView1.FocusedColumnChanged
        If CheckEdit1.Checked AndAlso GridView1.FocusedColumn.FieldName.ToUpper = "NoBPK".ToUpper Then
            GridView1.OptionsBehavior.Editable = True
        Else
            GridView1.OptionsBehavior.Editable = False
        End If
    End Sub

    Private Sub GridView1_FocusedRowChanged(ByVal sender As Object, ByVal e As DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs) Handles GridView1.FocusedRowChanged
        If CheckEdit1.Checked AndAlso GridView1.FocusedColumn.FieldName.ToUpper = "NoBPK".ToUpper Then
            GridView1.OptionsBehavior.Editable = True
        Else
            GridView1.OptionsBehavior.Editable = False
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
        If IsHutang Then
            NewHutang()
        Else
            NewPiutang()
        End If
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
        Try
            Dim view As ColumnView = GridControl1.FocusedView
            Dim row As System.Data.DataRow = view.GetDataRow(GridView1.FocusedRowHandle)
            NoID = NullToLong(row("NoID"))
            If NullToBool(row("IsPosted")) = False Then
                If XtraMessageBox.Show("Apakah anda mau menghapus Nota no: " & NullToStr(row("Kode")) & " ini?", NamaAplikasi, MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                    'If EksekusiSQL("Delete From MBayarHutangD where IDBayarHutang=" & NoID) Then
                    EksekusiSQL("DELETE FROM MBayarHutangD WHERE IDBayarHutang=" & NoID)
                    EksekusiSQL("DELETE FROM MBayarHutang WHERE NoID=" & NoID)
                    EksekusiSQL("DELETE FROM MBayarHutangDRetur WHERE IDBayarHutang=" & NoID)
                    EksekusiSQL("DELETE FROM MBayarHutangDPH WHERE IDBayarHutang=" & NoID)
                    EksekusiSQL("DELETE FROM MBayarHutangDDebet WHERE IDBayarHutang=" & NoID)
                    EksekusiSQL("DELETE FROM MBayarHutangDKredit WHERE IDBayarHutang=" & NoID)
                    EksekusiSQL("DELETE FROM MBayarHutangDGiro WHERE IDBayarHutang=" & NoID)
                    TampilData()
                    'End If
                End If
            End If
        Catch ex As Exception
            XtraMessageBox.Show(Me, ex.Message, ".:: Pesan Kesalahan ::.", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub SimpleButton2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdEdit.Click
        If IsHutang Then
            EditHutang()
        Else
            EditPiutang()
        End If
    End Sub

    Private Sub EditHutang()
        Dim x As New frmEntriPembayaranHutangOK
        Try
            Dim view As ColumnView = GridControl1.FocusedView
            Dim row As System.Data.DataRow = view.GetDataRow(GridView1.FocusedRowHandle)
            NoID = NullToLong(row("NoID"))
            If NullToBool(row("IsPosted")) Then
                x.cmdSave.Enabled = False
            End If
            x.IsNew = False
            x.pTipe = frmEntriPembayaranHutangOK.pStatus.Edit
            x.NoID = NoID
            x.IsJual = False
            x.pTrans = frmEntriPembayaranHutangOK.pTransaksi.Pembelian
            If x.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
                NoID = x.NoID
                TampilData()
                GridView1.ClearSelection()
                GridView1.FocusedRowHandle = GridView1.LocateByDisplayText(0, GridView1.Columns("NoID"), (NoID).ToString("#,##0"))
                GridView1.SelectRow(GridView1.FocusedRowHandle)
            End If
        Catch ex As Exception
            XtraMessageBox.Show(Me, ex.Message, ".:: Pesan Kesalahan ::.", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub
    Private Sub EditPiutang()
        Dim x As New frmEntriPembayaranPiutangOK
        Try
            Dim view As ColumnView = GridControl1.FocusedView
            Dim row As System.Data.DataRow = view.GetDataRow(GridView1.FocusedRowHandle)
            NoID = NullToLong(row("NoID"))
            If NullToBool(row("IsPosted")) Then
                x.cmdSave.Enabled = False
            End If
            x.IsNew = False
            x.pTipe = frmEntriPembayaranPiutangOK.pStatus.Edit
            x.NoID = NoID
            x.IsJual = True
            x.pTrans = frmEntriPembayaranPiutangOK.pTransaksi.Penjualan
            If x.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
                NoID = x.NoID
                TampilData()
                GridView1.ClearSelection()
                GridView1.FocusedRowHandle = GridView1.LocateByDisplayText(0, GridView1.Columns("NoID"), (NoID).ToString("#,##0"))
                GridView1.SelectRow(GridView1.FocusedRowHandle)
            End If
        Catch ex As Exception
            XtraMessageBox.Show(Me, ex.Message, ".:: Pesan Kesalahan ::.", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub
    Private Sub NewHutang()
        Dim x As New frmEntriPembayaranHutangOK
        Try
            x.IsNew = True
            x.pTipe = frmEntriPembayaranHutangOK.pStatus.Baru
            x.NoID = -1
            x.IsJual = False
            x.pTrans = frmEntriPembayaranHutangOK.pTransaksi.Pembelian
            If x.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
                NoID = x.NoID
                TampilData()
                GridView1.ClearSelection()
                GridView1.FocusedRowHandle = GridView1.LocateByDisplayText(0, GridView1.Columns("NoID"), (NoID).ToString("#,##0"))
                GridView1.SelectRow(GridView1.FocusedRowHandle)
            End If
        Catch ex As Exception
            XtraMessageBox.Show(Me, ex.Message, ".:: Pesan Kesalahan ::.", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub
    Private Sub NewPiutang()
        Dim x As New frmEntriPembayaranPiutangOK
        Try
            x.IsNew = True
            x.pTipe = frmEntriPembayaranPiutangOK.pStatus.Baru
            x.NoID = -1
            x.IsJual = True
            x.pTrans = frmEntriPembayaranPiutangOK.pTransaksi.Penjualan
            If x.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
                NoID = x.NoID
                TampilData()
                GridView1.ClearSelection()
                GridView1.FocusedRowHandle = GridView1.LocateByDisplayText(0, GridView1.Columns("NoID"), (NoID).ToString("#,##0"))
                GridView1.SelectRow(GridView1.FocusedRowHandle)
            End If
        Catch ex As Exception
            XtraMessageBox.Show(Me, ex.Message, ".:: Pesan Kesalahan ::.", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub
    Private Sub SimpleButton6_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdTutup.Click
        DialogResult = Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub mnPosting_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles mnPosting.ItemClick
        Try
            Dim view As ColumnView = GridControl1.FocusedView
            Dim I As Integer
            Dim row As System.Data.DataRow
            For I = 0 To view.SelectedRowsCount - 1
                row = view.GetDataRow(view.GetSelectedRows(I))
                NoID = NullToLong(row("NoID"))
                If NullToBool(row("IsPosted")) = False Then
                    EksekusiSQL("UPDATE MBayarHutang SET IsPosted=1 where NoID=" & NoID)
                End If
            Next
            TampilData()
        Catch ex As Exception
            XtraMessageBox.Show(Me, ex.Message, ".:: Pesan Kesalahan ::.", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub mnUnPosting_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles mnUnPosting.ItemClick
        Try
            Dim view As ColumnView = GridControl1.FocusedView
            Dim I As Integer
            Dim row As System.Data.DataRow
            For I = 0 To view.SelectedRowsCount - 1
                row = view.GetDataRow(view.GetSelectedRows(I))
                NoID = NullToLong(row("NoID"))
                If NullToBool(row("IsPosted")) = True Then
                    EksekusiSQL("UPDATE MBayarHutang SET IsPosted=0 where NoID=" & NoID)
                End If
            Next
            TampilData()
        Catch ex As Exception
            XtraMessageBox.Show(Me, ex.Message, ".:: Pesan Kesalahan ::.", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
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
                GridView1.SaveLayoutToXml(FolderLayouts & Me.Name & GridView1.Name & IsHutang & ".xml")

                gvKasBank.SaveLayoutToXml(FolderLayouts & Me.Name & gvKasBank.Name & IsHutang & ".xml")
                gvSupplier.SaveLayoutToXml(FolderLayouts & Me.Name & gvSupplier.Name & IsHutang & ".xml")
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
        If System.IO.File.Exists(FolderLayouts & Me.Name & gvKasBank.Name & IsHutang & ".xml") Then
            gvKasBank.RestoreLayoutFromXml(FolderLayouts & Me.Name & gvKasBank.Name & IsHutang & ".xml")
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
        If System.IO.File.Exists(FolderLayouts & Me.Name & gvSupplier.Name & IsHutang & ".xml") Then
            gvSupplier.RestoreLayoutFromXml(FolderLayouts & Me.Name & gvSupplier.Name & IsHutang & ".xml")
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
            If IsHutang Then
                namafile = Application.StartupPath & "\report\FakturMBayarHutang.rpt"
            Else
                namafile = Application.StartupPath & "\report\FakturMBayarPiutang.rpt"
            End If
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
                If IsHutang Then
                    'InsertTemporary(True, NoID, NullTolong(row("IDAlamat")))
                    ViewReport(Me.ParentForm, action, namafile, Me.Text, "{MBayarHutang.NoID}=" & NoID & " AND {MBayarHutang.IsPosted}=True ")
                    'CetakCRViewer(action, Me.MdiParent, namafile, "Cetak " & Me.Text, "{MJual.NoID}=" & NoID)
                Else
                    'InsertTemporary(False, NoID, NullTolong(row("IDAlamat")))
                    ViewReport(Me.ParentForm, action, namafile, Me.Text, "{MBayarPiutang.NoID}=" & NoID & " AND {MBayarPiutang.IsPosted}=True ")
                    'CetakCRViewer(action, Me.MdiParent, namafile, "Cetak " & Me.Text, "{MJual.NoID}=" & NoID)
                End If
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
            If IsHutang Then
                namafile = Application.StartupPath & "\report\FakturPanjangMBayarHutang.rpt"
            Else
                namafile = Application.StartupPath & "\report\FakturPanjangMBayarPiutang.rpt"
            End If
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
                If IsHutang Then
                    'InsertTemporary(True, NoID, NullTolong(row("IDAlamat")))
                    ViewReport(Me.ParentForm, action, namafile, Me.Text, "{MBayarHutang.NoID}=" & NoID & " AND {MBayarHutang.IsPosted}=True ")
                Else
                    'InsertTemporary(False, NoID, NullTolong(row("IDAlamat")))
                    ViewReport(Me.ParentForm, action, namafile, Me.Text, "{MBayarPiutang.NoID}=" & NoID & " AND {MBayarPiutang.IsPosted}=True ")
                End If
                'CetakCRViewer(action, Me.MdiParent, namafile, "Cetak " & Me.Text, "{MJual.NoID}=" & NoID)
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
        Dim NamaFile As String = Application.StartupPath & "\Report\DaftarBayarHutang.rpt"
        Try
            If EditReport Then
                ViewReport(Me.MdiParent, action_.Edit, NamaFile, "Daftar Pembayaran Hutang", , , "TglDari=cdate(" & tglDari.DateTime.ToString("yyyy,MM,dd") & ")&TglSampai=cdate(" & TglSampai.DateTime.ToString("yyyy,MM,dd") & ")&IDSupplier=" & IIf(txtAlamat.Text = "", -1, NullToLong(txtAlamat.EditValue)))
            Else
                If tglDari.Enabled AndAlso Not TglKembaliDari.Enabled Then 'Hanya Tanggal
                    ViewReport(Me.MdiParent, action_.Preview, NamaFile, "Pembayaran Hutang Berdasarkan Tgl Entri", , , _
                               "IsTanggal=True&IsTglKembali=False&TglDari=cdate(" & tglDari.DateTime.ToString("yyyy,MM,dd") & ")&TglSampai=cdate(" & TglSampai.DateTime.ToString("yyyy,MM,dd") & ")&TglKembaliDari=CDATE(" & TglKembaliDari.DateTime.ToString("yyyy,MM,dd") & ")&TglKembaliSampai=CDATE(" & TglKembaliSampai.DateTime.ToString("yyyy,MM,dd") & ")&IDSupplier=" & IIf(txtAlamat.Text = "", -1, NullToLong(txtAlamat.EditValue)))
                ElseIf Not tglDari.Enabled AndAlso TglKembaliDari.Enabled Then 'Hanya Tanggal Kembali
                    ViewReport(Me.MdiParent, action_.Preview, NamaFile, "Pembayaran Hutang Berdasarkan Tgl Kembali", , , _
                               "IsTanggal=False&IsTglKembali=True&TglDari=cdate(" & tglDari.DateTime.ToString("yyyy,MM,dd") & ")&TglSampai=cdate(" & TglSampai.DateTime.ToString("yyyy,MM,dd") & ")&TglKembaliDari=CDATE(" & TglKembaliDari.DateTime.ToString("yyyy,MM,dd") & ")&TglKembaliSampai=CDATE(" & TglKembaliSampai.DateTime.ToString("yyyy,MM,dd") & ")&IDSupplier=" & IIf(txtAlamat.Text = "", -1, NullToLong(txtAlamat.EditValue)))
                Else 'Dua-duanya
                    ViewReport(Me.MdiParent, action_.Preview, NamaFile, "Pembayaran Hutang Berdasarkan Tgl Entri dan Tgl Kembali", , , _
                               "IsTanggal=True&IsTglKembali=True&TglDari=cdate(" & tglDari.DateTime.ToString("yyyy,MM,dd") & ")&TglSampai=cdate(" & TglSampai.DateTime.ToString("yyyy,MM,dd") & ")&TglKembaliDari=CDATE(" & TglKembaliDari.DateTime.ToString("yyyy,MM,dd") & ")&TglKembaliSampai=CDATE(" & TglKembaliSampai.DateTime.ToString("yyyy,MM,dd") & ")&IDSupplier=" & IIf(txtAlamat.Text = "", -1, NullToLong(txtAlamat.EditValue)))
                End If
            End If
        Catch ex As Exception
            XtraMessageBox.Show("Info Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
        End Try
    End Sub

    Private Sub LabelControl6_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LabelControl6.Click
        TglKembaliDari.Enabled = Not TglKembaliDari.Enabled
        TglKembaliSampai.Enabled = TglKembaliDari.Enabled
        TampilData()
    End Sub

    Private Sub tglDari_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tglDari.EditValueChanged

    End Sub

    Private Sub GridControl1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles GridControl1.Click

    End Sub
End Class