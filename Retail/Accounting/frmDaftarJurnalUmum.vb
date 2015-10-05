Imports DevExpress.XtraEditors
Imports DevExpress.XtraGrid
Imports DevExpress.XtraGrid.Views.Grid
Imports DevExpress.XtraGrid.Views.Grid.ViewInfo
Imports DevExpress.XtraGrid.Views.Base
Imports DevExpress.XtraGrid.Columns
Imports VPoint.FungsiControl
Public Class frmDaftarJurnalUmum
    Dim oDS As New DataSet
    Dim ID As Long
    Dim SQL As String
    Public pStatus As mdlAccPublik.ptipe, BolehAmbilData As Boolean
    Public IDWilayah As Long

    Dim repckedit As New DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit
    Dim repdateedit As New DevExpress.XtraEditors.Repository.RepositoryItemDateEdit
    Dim reptextedit As New DevExpress.XtraEditors.Repository.RepositoryItemTextEdit
    Dim reppicedit As New DevExpress.XtraEditors.Repository.RepositoryItemPictureEdit
    Private Sub frmDaftarSTB_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        GridView1.SaveLayoutToXml(LayOutKu(Name & GridView1.Name))
    End Sub

    Private Sub frmDaftarSTB_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        SetCtlMe()
        cmdRefresh.PerformClick()
        LoadLayout()
    End Sub
    Private Sub LoadLayout()
        Try
            If System.IO.File.Exists(LayOutKu(Name & GridView1.Name)) Then
                GridView1.RestoreLayoutFromXml(LayOutKu(Name & GridView1.Name))
            End If
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
                            .Columns(i).DisplayFormat.FormatString = "dd-MM-yyyy"
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
        Catch ex As Exception
            XtraMessageBox.Show("Info Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button2)
        End Try
    End Sub
    Private Sub SetCtlMe()
        SetForm(Me)
        SetButton(cmdNew, button_.cmdNew)
        SetButton(cmdClose, button_.cmdExit)
        SetButton(cmdDelete, button_.cmdDelete)
        SetButton(cmdEdit, button_.cmdEdit)
        SetButton(cmdExport, button_.cmdExportXls)
        SetButton(cmdMark, button_.cmdMark)
        SetButton(cmdRefresh, button_.cmdRefresh)
        tglDari.DateTime = Today
        TglSampai.DateTime = Today
        cbStatus.SelectedIndex = 0
        If System.IO.File.Exists(LayOutKu(Me.Name & LayoutControl1.Name)) Then
            LayoutControl1.RestoreLayoutFromXml(LayOutKu(Me.Name & LayoutControl1.Name))
        End If
    End Sub
    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() : setme call.

    End Sub

    Protected Overrides Sub Finalize()
        MyBase.Finalize()
    End Sub

    Private Sub cmdClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdClose.Click
        Close()
    End Sub

    Private Sub cmdRefresh_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdRefresh.Click
        RefreshData()
    End Sub

    Private Sub RefreshData()
        Try
            Dim IsPerluWhere As Boolean = False
            SQL = "SELECT MJurnal.ID, MJurnal.Kode, MJurnal.Tanggal, MJurnal.Keterangan, MJurnal.IsPosting, MJurnal.KodeReff, MJurnal.TanggalEntry, MJurnal.TanggalPosting, SUM(MJurnalD.DebetA) AS Debet, SUM(MJurnalD.KreditA) AS Kredit " & vbCrLf
            SQL = SQL & " FROM MJurnal INNER JOIN MJurnalD ON MjurnalD.IDJurnal=Mjurnal.ID Where MJurnal.IDJenisTransaksi=1  " & vbCrLf
            'If IDWilayah >= 1 Then
            '    SQL = SQL & IIf(IsPerluWhere, " WHERE ", " AND ") & " MKontrakJual.IDWilayah=" & IDWilayah & vbCrLf
            '    IsPerluWhere = False
            'End If
            If tglDari.Enabled Then
                SQL = SQL & IIf(IsPerluWhere, " WHERE ", " AND ") & " MJurnal.Tanggal>='" & Format(tglDari.DateTime, "yyyy/MM/dd") & "' AND MJurnal.Tanggal<='" & Format(TglSampai.DateTime, "yyyy/MM/dd") & "' " & vbCrLf
                IsPerluWhere = False
            End If
            If cbStatus.SelectedIndex = 1 Then 'Posting
                SQL &= IIf(IsPerluWhere, " WHERE ", " AND ") & " IsNull(MJurnal.IsPosting,0)=1"
            ElseIf cbStatus.SelectedIndex = 2 Then 'Unposting
                SQL &= IIf(IsPerluWhere, " WHERE ", " AND ") & " IsNull(MJurnal.IsPosting,0)=0"
            End If
            SQL &= "GROUP BY MJurnal.ID, MJurnal.Kode, MJurnal.Tanggal, MJurnal.Keterangan, MJurnal.IsPosting, MJurnal.KodeReff, MJurnal.TanggalEntry, MJurnal.TanggalPosting"
            ExecuteDBGrid(GridControl1, SQL)
            SetGridView(GridControl1)
        Catch ex As Exception
            FxMessage("Error : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error, , , ex.StackTrace)
        End Try
    End Sub
    Private Sub cmdExport_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdExport.Click
        ExportGrid(GridControl1, ExportTo.Excel)
    End Sub

    Private Sub GridControl1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles GridControl1.Click

    End Sub

    Private Sub LabelControl5_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LabelControl5.Click
        tglDari.Enabled = Not tglDari.Enabled
        TglSampai.Enabled = tglDari.Enabled
    End Sub

    Private Sub cmdDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdDelete.Click
        Dim view As ColumnView = GridControl1.FocusedView
        'Try
        Dim row As System.Data.DataRow = view.GetDataRow(GridView1.FocusedRowHandle)
        Dim NoID As Long = NullToLong(row("ID"))
        Dim IsPosted As Boolean = NullToBool(row("IsPosting"))
        If Not IsPosted Then
            If FxMessage("Yakin Mau Hapus data ini?", "Hapus Data Jurnal", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                EksekusiSQL("DELETE FROM MJurnalD WHERE IDJurnal=" & NoID)
                EksekusiSQL("DELETE FROM Mjurnal WHERE ID=" & NoID)
                RefreshData()
            End If
        Else
            MsgBox("Silahkan Unposting terlebih dulu untuk menghapus data!", MsgBoxStyle.Information)
        End If
    End Sub

    Private Sub mnPosting_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles mnPosting.ItemClick
        Try
            Dim view As ColumnView = GridControl1.FocusedView
            Dim I As Integer
            For I = 0 To view.SelectedRowsCount - 1
                Dim row As System.Data.DataRow = view.GetDataRow(view.GetSelectedRows(I))
                ID = NullToLong(row("ID"))
                If NullToBool(row("IsPosting")) = False Then
                    EksekusiSQL("UPDATE MJurnal SET TanggalPosting=Tanggal, IsPosting=1, IDUserPosting=" & IDUserAktif & " where ID=" & ID)
                End If
            Next
            RefreshData()
        Catch ex As Exception
            FxMessage(ex.Message, ".:: Pesan Kesalahan ::.", MessageBoxButtons.OK, MessageBoxIcon.Error, , , ex.StackTrace)
        End Try
    End Sub

    Private Sub cmdEdit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdEdit.Click
        Dim view As ColumnView = GridControl1.FocusedView
        Dim x As New frmEntriJU
        Try
            Dim row As System.Data.DataRow = view.GetDataRow(GridView1.FocusedRowHandle)
            Dim NoID As Long = row("ID")
            x.NoID = NoID
            x.pStatus = mdlAccPublik.ptipe.Edit
            If NullToBool(row("IsPosting")) Then
                x.SimpleButton9.Enabled = False
            End If
            If x.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
                RefreshData()
                GridView1.ClearSelection()
                GridView1.FocusedRowHandle = GridView1.LocateByValue("ID", x.NoID)
                GridView1.SelectRow(GridView1.FocusedRowHandle)
            End If
        Catch ex As Exception
            FxMessage("Error : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error, , , ex.StackTrace)
        Finally
            x.Close()
            x.Dispose()
        End Try
    End Sub

    Private Sub cmdNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdNew.Click
        Dim view As ColumnView = GridControl1.FocusedView
        Dim x As New frmEntriJU
        Try
            x.NoID = -1
            x.pStatus = mdlAccPublik.ptipe.Baru
            x.TglDefault = TglSampai.DateTime
            If x.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
                RefreshData()
                GridView1.ClearSelection()
                GridView1.FocusedRowHandle = GridView1.LocateByValue("ID", x.NoID)
                GridView1.SelectRow(GridView1.FocusedRowHandle)
            End If
        Catch ex As Exception
            FxMessage("Error : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error, , , ex.StackTrace)
        Finally
            x.Close()
            x.Dispose()
        End Try
    End Sub

    Private Sub mnUnPosting_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles mnUnPosting.ItemClick
        Try
            Dim view As ColumnView = GridControl1.FocusedView
            Dim I As Integer
            For I = 0 To view.SelectedRowsCount - 1
                Dim row As System.Data.DataRow = view.GetDataRow(view.GetSelectedRows(I))
                ID = NullToLong(row("ID"))
                If NullToBool(row("IsPosting")) = True Then
                    EksekusiSQL("UPDATE MJurnal SET IsPosting=0,IDUserPosting=0 where ID=" & ID)
                End If
            Next
            RefreshData()
        Catch ex As Exception
            FxMessage(ex.Message, ".:: Pesan Kesalahan ::.", MessageBoxButtons.OK, MessageBoxIcon.Error, , , ex.StackTrace)
        End Try
    End Sub

    Private Sub GridControl1_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles GridControl1.DoubleClick
        If pStatus = mdlAccPublik.ptipe.LookUp Then
            'If BolehAmbilData Then
            '    AmbilData()
            'End If
        ElseIf pStatus = mdlAccPublik.ptipe.Lihat Then
            If BolehAmbilData Then
                cmdEdit.PerformClick()
            End If
        End If
    End Sub

    Private Sub GridControl1_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles GridControl1.KeyDown
        If e.KeyCode = Keys.Enter Then
            If pStatus = mdlAccPublik.ptipe.LookUp Then
                'AmbilData()
            ElseIf pStatus = mdlAccPublik.ptipe.Lihat Then
                cmdEdit.PerformClick()
            End If
        ElseIf e.KeyCode = Keys.Escape Then
            DialogResult = Windows.Forms.DialogResult.Cancel
            Close()
        End If
    End Sub

    Private Sub GridControl1_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles GridControl1.MouseDown
        Dim HI As New DevExpress.XtraGrid.Views.Grid.ViewInfo.GridHitInfo
        HI = GridView1.CalcHitInfo(e.X, e.Y)
        If HI.InRow Then
            BolehAmbilData = True
        Else
            BolehAmbilData = False
        End If
    End Sub

    Private Sub GridView1_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles GridView1.MouseDown
        Dim View As GridView = CType(sender, GridView)
        If View Is Nothing Then Return
        ' obtaining hit info
        Dim hitInfo As GridHitInfo = View.CalcHitInfo(New System.Drawing.Point(e.X, e.Y))
        If (e.Button = Windows.Forms.MouseButtons.Right) And (hitInfo.InRow) And _
          (Not View.IsGroupRow(hitInfo.RowHandle)) Then
            'If IsSupervisor Then
            PopupMenu1.ShowPopup(Control.MousePosition)
            'Else
            'PopupMenu2.ShowPopup(Control.MousePosition)
            'End If
        End If
    End Sub

    Private Sub mnFaktur_ItemClick(ByVal sender As Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles mnFaktur.ItemClick
        If frmMain.ckEditRpt.Checked = True Then
            CetakStruck(action_.Edit)
        Else
            CetakStruck(action_.Preview)
        End If
    End Sub
    Private Sub CetakStruck(ByVal action As action_)
        Dim namafile As String
        Dim view As ColumnView = GridControl1.FocusedView
        Dim row As System.Data.DataRow = view.GetDataRow(GridView1.FocusedRowHandle)
        Try
            namafile = Application.StartupPath & "\report\JurnalUmum.rpt"
            ID = NullToLong(row("ID"))
            mdlCetakCR.ViewReport(Me.MdiParent, action, namafile, "Cetak Bukti Jurnal Umum", , "ID", ID)
        Catch EX As Exception
            FxMessage(EX.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error, , , EX.StackTrace)
        End Try
    End Sub

    Private Sub mnPerbaikiData_ItemClick(ByVal sender As Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles mnPerbaikiData.ItemClick
        Try
            If XtraMessageBox.Show("Ingin melakukan perbaikan data?", NamaAplikasi, MessageBoxButtons.YesNo, MessageBoxIcon.Information) = Windows.Forms.DialogResult.Yes Then
                Dim view As ColumnView = GridControl1.FocusedView
                For i As Integer = 0 To view.SelectedRowsCount - 1
                    Dim row As System.Data.DataRow = view.GetDataRow(view.GetSelectedRows(i))
                    ID = NullToLong(row("ID"))
                    If NullToBool(row("IsPosting")) = False Then
                        EksekusiSQL("UPDATE MJurnalD SET DebetA=Debet, KreditA=Kredit WHERE IDJurnal=" & ID)
                    End If
                Next
                RefreshData()
            End If
        Catch ex As Exception
            FxMessage(ex.Message, ".:: Pesan Kesalahan ::.", MessageBoxButtons.OK, MessageBoxIcon.Error, , , ex.StackTrace)
        End Try
    End Sub
End Class