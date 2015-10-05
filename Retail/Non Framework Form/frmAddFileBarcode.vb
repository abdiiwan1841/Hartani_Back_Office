Imports System.Data
Imports DevExpress.XtraExport
Imports DevExpress.XtraEditors
Imports DevExpress.XtraEditors.Repository
Imports DevExpress.XtraGrid.Views.Base
Imports DevExpress.XtraReports.UI

Public Class frmAddFileBarcode
    Dim SQL As String
    Dim DS As New DataSet
    Dim repckedit As New RepositoryItemCheckEdit
    Dim repdateedit As New RepositoryItemDateEdit
    Dim reptextedit As New RepositoryItemTextEdit
    Dim reppicedit As New RepositoryItemPictureEdit
    Dim NoID As Long = -1
    Private Sub RefreshData()
        Try
            SQL = "SELECT * FROM MTipeCetakBarcodeV2"
            DS = ExecuteDataset("MData", SQL)
            GridControl1.DataSource = DS.Tables("MData")
            GridView1.OptionsBehavior.Editable = False
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
                        Case "boolean"
                            .Columns(i).ColumnEdit = repckedit
                    End Select
                    'If LayoutView1.Columns(i).FieldName.Length >= 4 AndAlso LayoutView1.Columns(i).FieldName.Substring(0, 4).ToLower = "Kode".ToLower Then
                    '    LayoutView1.Columns(i).Fixed = FixedStyle.Left
                    'ElseIf LayoutView1.Columns(i).FieldName.ToLower = "Nama".ToLower Then
                    '    LayoutView1.Columns(i).Fixed = FixedStyle.Left
                    'End If
                Next
            End With
        Catch ex As Exception
            XtraMessageBox.Show("Info Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
        Finally
            If Not DS Is Nothing Then
                DS.Dispose()
            End If
        End Try
    End Sub

    Private Sub cmdClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdClose.Click
        DialogResult = Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub frmAddFileBarcode_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        RefreshData()
        If System.IO.File.Exists(FolderLayouts & Me.Name & GridView1.Name & ".xml") Then
            GridView1.RestoreLayoutFromXml(FolderLayouts & Me.Name & GridView1.Name & ".xml")
        End If
        If System.IO.File.Exists(FolderLayouts & Me.Name & LayoutControl1.Name & ".xml") Then
            LayoutControl1.RestoreLayoutFromXml(FolderLayouts & Me.Name & LayoutControl1.Name & ".xml")
        End If
    End Sub

    Private Sub mnSaveLayouts_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles mnSaveLayouts.ItemClick
        Dim xOtorisasi As New frmOtorisasiAdmin
        Try
            If XtraMessageBox.Show("Simpan Layout?", NamaAplikasi, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = Windows.Forms.DialogResult.Yes AndAlso xOtorisasi.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
                GridView1.SaveLayoutToXml(FolderLayouts & Me.Name & GridView1.Name & ".xml")
                LayoutControl1.SaveLayoutToXml(FolderLayouts & Me.Name & LayoutControl1.Name & ".xml")
            End If
        Catch ex As Exception
            XtraMessageBox.Show("Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            xOtorisasi.Dispose()
        End Try
    End Sub

    Private Sub cmdHapus_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdHapus.Click
        Dim view As ColumnView = GridControl1.FocusedView
        Dim row As System.Data.DataRow
        Try
            row = view.GetDataRow(GridView1.FocusedRowHandle)
            Dim NoID As Long = NullToLong(row("NoID"))
            If XtraMessageBox.Show(Me, "Yakin mau hapus data Tipe Barcode dengan Tipe " & NullToStr(row("Kode")), NamaAplikasi, MessageBoxButtons.YesNo) = Windows.Forms.DialogResult.Yes Then
                EksekusiSQL("DELETE FROM MTipeCetakBarcodeV2 WHERE NoID= " & NoID.ToString)
                RefreshData()
                GridView1.ClearSelection()
                GridView1.FocusedRowHandle = GridView1.LocateByDisplayText(0, GridView1.Columns("NoID"), (NoID).ToString("#,##0"))
                GridView1.SelectRow(GridView1.FocusedRowHandle)
            End If
        Catch ex As Exception
            XtraMessageBox.Show("Untuk menghapus pilih data yang akan dihapus terlebih dahulu lalu tekan tombol hapus!", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally

        End Try
    End Sub

    Private Sub cmdBaru_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdBaru.Click
        Dim Report As XtraReport = Nothing
        Dim NamaFile As String = ""
        Dim DS As New DataSet
        Dim ID As Long = -1
        Try
            If IsValidasi() Then
                NamaFile = txtNamaFile.Text.ToUpper.Replace(".REPX", "") & ".Repx"
                ID = GetNewID("MTipeCetakBarcodeV2", "NoID")
                EksekusiSQL("INSERT INTO MTipeCetakBarcodeV2 (NoID, Kode, NamaFile) VALUES (" & ID & ",'" & FixApostropi(txtKode.Text) & "','" & FixApostropi(NamaFile) & "')")
                Report = New XtraReport
                Report.Margins.Top = 0.0
                Report.Margins.Left = 0.0
                Report.Margins.Right = 0.0
                Report.Margins.Bottom = 0.0
                Report.Name = txtKode.Text
                Report.PaperKind = System.Drawing.Printing.PaperKind.Custom
                Report.ReportUnit = ReportUnit.TenthsOfAMillimeter
                Report.PageHeight = txtHeight.EditValue
                Report.PageWidth = txtWidth.EditValue
                Report.PaperName = "Custom"
                Report.PrinterName = ""
                Report.DisplayName = NamaFile
                Report.SaveLayout(Application.StartupPath & "\Report\" & NamaFile)
                Report.ShowDesignerDialog()
                RefreshData()
                GridView1.ClearSelection()
                GridView1.FocusedRowHandle = GridView1.LocateByDisplayText(0, GridView1.Columns("NoID"), (ID).ToString("#,##0"))
                GridView1.SelectRow(GridView1.FocusedRowHandle)
            End If
        Catch ex As Exception
            XtraMessageBox.Show("Info Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
        Finally
            If Not Report Is Nothing Then
                Report.Dispose()
            End If
            If Not DS Is Nothing Then
                DS.Dispose()
            End If
        End Try
    End Sub

    Private Function IsValidasi() As Boolean
        Dim NamaFile As String = ""
        If txtKode.Text = "" Then
            XtraMessageBox.Show("Kode Masih Kosong.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
            txtKode.Focus()
            Return False
            Exit Function
        End If
        If NullToLong(EksekusiSQlSkalarNew("SELECT COUNT(MTipeCetakBarcodeV2.NoID) FROM MTipeCetakBarcodeV2 WHERE UPPER(MTipeCetakBarcodeV2.Kode)=UPPER('" & FixApostropi(txtKode.Text) & "')")) >= 1 Then
            XtraMessageBox.Show("Kode sudah dipakai.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
            txtKode.Focus()
            Return False
            Exit Function
        End If
        If txtNamaFile.Text = "" Then
            XtraMessageBox.Show("Nama File Masih Kosong.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
            txtNamaFile.Focus()
            Return False
            Exit Function
        End If
        NamaFile = txtNamaFile.Text.ToUpper.Replace(".REPX", "") & ".repx"
        If txtNamaFile.Text <> "" AndAlso System.IO.File.Exists(Application.StartupPath & "\Report\" & NamaFile) Then
            If XtraMessageBox.Show("Nama File Sudah Ada." & vbCrLf & "Yakin untuk mereplace?", NamaAplikasi, MessageBoxButtons.YesNo, MessageBoxIcon.Information) = Windows.Forms.DialogResult.No Then
                txtNamaFile.Focus()
                Return False
                Exit Function
            Else
                If NullToLong(EksekusiSQlSkalarNew("SELECT COUNT(MTipeCetakBarcodeV2.NoID) FROM MTipeCetakBarcodeV2 WHERE UPPER(MTipeCetakBarcodeV2.NamaFile)=UPPER('" & FixApostropi(NamaFile) & "')")) >= 1 Then
                    XtraMessageBox.Show("Nama File Sudah didatabasekan.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
                    txtNamaFile.Focus()
                    Return False
                    Exit Function
                End If
            End If
        End If
        If txtNamaFile.Text = "" Then
            XtraMessageBox.Show("Nama File Masih Kosong.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
            txtNamaFile.Focus()
            Return False
            Exit Function
        End If
        Return True
    End Function

    Private Sub txtKode_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtKode.EditValueChanged

    End Sub

    Private Sub GridView1_FocusedRowChanged(ByVal sender As Object, ByVal e As DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs) Handles GridView1.FocusedRowChanged
        If GridView1.RowCount >= 1 Then
            NoID = NullToLong(GridView1.GetRowCellValue(GridView1.FocusedRowHandle, "NoID"))
            txtNamaFile.Text = GridView1.GetRowCellValue(GridView1.FocusedRowHandle, "NamaFile")
            txtKode.Text = GridView1.GetRowCellValue(GridView1.FocusedRowHandle, "Kode")
        End If
    End Sub

    Private Sub cmdUpdate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdUpdate.Click
        Try
            EksekusiSQL("UPDATE MTipeCetakBarcodeV2 SET Kode='" & FixApostropi(txtKode.Text) & "', NamaFile='" & FixApostropi(txtNamaFile.Text.ToUpper.Replace(".REPX", "") & ".Repx") & "' WHERE NoID=" & NoID)
            RefreshData()
            GridView1.ClearSelection()
            GridView1.FocusedRowHandle = GridView1.LocateByDisplayText(0, GridView1.Columns("NoID"), (NoID).ToString("#,##0"))
            GridView1.SelectRow(GridView1.FocusedRowHandle)
        Catch ex As Exception
            XtraMessageBox.Show("Info Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
        End Try
    End Sub
End Class