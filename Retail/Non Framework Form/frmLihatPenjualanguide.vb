Imports System.Data
Imports System.Data.SqlClient
Imports DevExpress.XtraEditors
Imports DevExpress.Utils
Imports System.IO.File
Imports System
Imports VPoint.mdlCetakCR
Imports System.Xml

Public Class frmLihatPenjualanGuide
    Public IsNew As Boolean = True
    Public IsPosted As Boolean = False
    Public NoID As Long = -1
    Public IDTandaTerima As Long = -1
    Public IDSupplier As Long = -1
    Public FormPemanggil As frmEntriTandaTerima
    Public IsFastEntri As Boolean = False

    Dim oDS As New DataSet
    Dim SQL As String = ""
    Dim DefImageList As ImageList

    Private Sub IsiDefault()

        RefreshLookUp()
    End Sub
    Private Sub RefreshLookUp()
        Dim ds As New DataSet
        Try
            SQL = "SELECT MAlamat.NoID, MAlamat.Kode, MAlamat.Nama, MTypeCustomer.DiscountMarketing as Fee FROM Malamat inner join MTypeCustomer On MAlamat.IDTypeCustomer=MTypeCustomer.NoID WHERE MAlamat.IsCustomer=1 and MAlamat.IsActive=1"
            ds = ExecuteDataset("MMarketing", SQL)
            txtKodeCustomer1.Properties.DataSource = ds.Tables("MMarketing")
            txtKodeCustomer1.Properties.ValueMember = "NoID"
            txtKodeCustomer1.Properties.DisplayMember = "Kode"

            txtKodeCustomer2.Properties.DataSource = ds.Tables("MMarketing")
            txtKodeCustomer2.Properties.ValueMember = "NoID"
            txtKodeCustomer2.Properties.DisplayMember = "Kode"

            txtKodeCustomer3.Properties.DataSource = ds.Tables("MMarketing")
            txtKodeCustomer3.Properties.ValueMember = "NoID"
            txtKodeCustomer3.Properties.DisplayMember = "Kode"

            txtKodeCustomer4.Properties.DataSource = ds.Tables("MMarketing")
            txtKodeCustomer4.Properties.ValueMember = "NoID"
            txtKodeCustomer4.Properties.DisplayMember = "Kode"

            
        Catch ex As Exception
            XtraMessageBox.Show("Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            ds.Dispose()
        End Try
    End Sub
    Private Sub LoadData()
        Dim sql1 As String
        Dim sql2 As String
        Dim sql3 As String
        Dim sql4 As String

        Try

            If NullToLong(txtKodeCustomer1.EditValue) > 0 Then
                sql1 = "SELECT MJual.NoID,MJual.Tanggal,MJual.Kode,MJual.Total,MJual.FeeMarketing,MJual.FeeMarketingRp from MJual " & _
                "WHERE MJual.Tanggal>='" & Format(TglDari.DateTime, "yyyy/MM/dd") & "' and MJual.Tanggal<'" & Format(TglSampai.DateTime.AddDays(1), "yyyy/MM/dd") & "' and MJual.KodeMarketing='" & FixApostropi(NullToStr(txtKodeCustomer1.Text)) & "'"
            Else
                sql1 = "SELECT MJual.NoID,MJual.Tanggal,MJual.Kode,MJual.Total,MJual.FeeMarketing,MJual.FeeMarketingRp from MJual " & _
            "WHERE MJual.Tanggal>='" & Format(TglDari.DateTime, "yyyy/MM/dd") & "' and MJual.Tanggal<'" & Format(TglSampai.DateTime.AddDays(1), "yyyy/MM/dd") & "' and MJual.noid=-2"
            End If

            If NullToLong(txtKodeCustomer2.EditValue) > 0 Then
                sql2 = "SELECT MJual.NoID,MJual.Tanggal,MJual.Kode,MJual.Total,MJual.FeeMarketing,MJual.FeeMarketingRp from MJual " & _
                "WHERE MJual.Tanggal>='" & Format(TglDari.DateTime, "yyyy/MM/dd") & "' and MJual.Tanggal<'" & Format(TglSampai.DateTime.AddDays(1), "yyyy/MM/dd") & "' and MJual.KodeMarketing='" & FixApostropi(NullToStr(txtKodeCustomer2.Text)) & "'"
            Else
                sql2 = "SELECT MJual.NoID,MJual.Tanggal,MJual.Kode,MJual.Total,MJual.FeeMarketing,MJual.FeeMarketingRp from MJual " & _
            "WHERE MJual.Tanggal>='" & Format(TglDari.DateTime, "yyyy/MM/dd") & "' and MJual.Tanggal<'" & Format(TglSampai.DateTime.AddDays(1), "yyyy/MM/dd") & "' and MJual.noid=-2"
            End If

            If NullToLong(txtKodeCustomer3.EditValue) > 0 Then
                sql3 = "SELECT MJual.NoID,MJual.Tanggal,MJual.Kode,MJual.Total,MJual.FeeMarketing,MJual.FeeMarketingRp from MJual " & _
                "WHERE MJual.Tanggal>='" & Format(TglDari.DateTime, "yyyy/MM/dd") & "' and MJual.Tanggal<'" & Format(TglSampai.DateTime.AddDays(1), "yyyy/MM/dd") & "' and MJual.KodeMarketing='" & FixApostropi(NullToStr(txtKodeCustomer3.Text)) & "'"
            Else
                sql3 = "SELECT MJual.NoID,MJual.Tanggal,MJual.Kode,MJual.Total,MJual.FeeMarketing,MJual.FeeMarketingRp from MJual " & _
            "WHERE MJual.Tanggal>='" & Format(TglDari.DateTime, "yyyy/MM/dd") & "' and MJual.Tanggal<'" & Format(TglSampai.DateTime.AddDays(1), "yyyy/MM/dd") & "' and MJual.noid=-2"
            End If

            If NullToLong(txtKodeCustomer4.EditValue) > 0 Then
                sql4 = "SELECT MJual.NoID,MJual.Tanggal,MJual.Kode,MJual.Total,MJual.FeeMarketing,MJual.FeeMarketingRp from MJual " & _
                "WHERE MJual.Tanggal>='" & Format(TglDari.DateTime, "yyyy/MM/dd") & "' and MJual.Tanggal<'" & Format(TglSampai.DateTime.AddDays(1), "yyyy/MM/dd") & "' and MJual.KodeMarketing='" & FixApostropi(NullToStr(txtKodeCustomer4.Text)) & "'"
            Else
                sql4 = "SELECT MJual.NoID,MJual.Tanggal,MJual.Kode,MJual.Total,MJual.FeeMarketing,MJual.FeeMarketingRp from MJual " & _
                "WHERE MJual.Tanggal>='" & Format(TglDari.DateTime, "yyyy/MM/dd") & "' and MJual.Tanggal<'" & Format(TglSampai.DateTime.AddDays(1), "yyyy/MM/dd") & "' and MJual.noid=-2"
            End If


            oDS = New DataSet()
            oDS = ExecuteDataset("JUAL1", sql1)
            GC1.DataSource = oDS.Tables("JUAL1")
            oDS = ExecuteDataset("JUAL2", sql2)
            GC2.DataSource = oDS.Tables("JUAL2")
            oDS = ExecuteDataset("JUAL3", sql3)
            GC3.DataSource = oDS.Tables("JUAL3")
            oDS = ExecuteDataset("JUAL4", sql4)
            GC4.DataSource = oDS.Tables("JUAL4")
            txtTotal1.EditValue = GV1.Columns("Total").SummaryItem.SummaryValue
            txtTotal2.EditValue = GV2.Columns("Total").SummaryItem.SummaryValue
            txtTotal3.EditValue = GV3.Columns("Total").SummaryItem.SummaryValue
            txtTotal4.EditValue = GV4.Columns("Total").SummaryItem.SummaryValue
        Catch ex As Exception
            XtraMessageBox.Show("Kesalahan: " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            oDS.Dispose()
        End Try
    End Sub

    Private Sub cmdSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSave.Click
        LoadData()
    End Sub
    Private Sub cmdTutup_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdTutup.Click
        DialogResult = Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub BarButtonItem1_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles cmdSaveLayout.ItemClick
        Dim xOtorisasi As New frmOtorisasiAdmin
        Try
            If XtraMessageBox.Show("Simpan Layout?", NamaAplikasi, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = Windows.Forms.DialogResult.Yes AndAlso xOtorisasi.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
                Ini.TulisIniPath(FolderLayouts & Me.Name & ".ini", "Form", "Width", Me.Width)
                Ini.TulisIniPath(FolderLayouts & Me.Name & ".ini", "Form", "Height", Me.Height)
                Me.Left = (Screen.PrimaryScreen.WorkingArea.Width - Me.Width) \ 2
                Me.Top = (Screen.PrimaryScreen.WorkingArea.Height - Me.Height) \ 2

                LayoutControl1.SaveLayoutToXml(FolderLayouts & Me.Name & LayoutControl1.Name & ".xml")
                GV1.SaveLayoutToXml(FolderLayouts & Me.Name & GV1.Name & ".xml")
                GV2.SaveLayoutToXml(FolderLayouts & Me.Name & GV2.Name & ".xml")
                GV3.SaveLayoutToXml(FolderLayouts & Me.Name & GV3.Name & ".xml")
                GV4.SaveLayoutToXml(FolderLayouts & Me.Name & GV4.Name & ".xml")
            End If
        Catch ex As Exception
            XtraMessageBox.Show("Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            xOtorisasi.Dispose()
        End Try
    End Sub

    Private Sub frmEntriLPBD_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        Try
            Me.Dispose()
        Catch ex As Exception

        End Try
    End Sub

    Private Sub frmEntriPOD_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim dlg As DevExpress.Utils.WaitDialogForm = Nothing
        Try
            Dim curentcursor As Cursor = Windows.Forms.Cursor.Current
            Windows.Forms.Cursor.Current = Cursors.WaitCursor
            dlg = New DevExpress.Utils.WaitDialogForm(String.Format("Creating component and analize database.{0}MOHON TUNGGU ...", vbCrLf), Application.ProductName)
            dlg.TopMost = False
            dlg.Show()
            Windows.Forms.Cursor.Current = Cursors.WaitCursor
            IsiDefault()
            TglDari.EditValue = Today
            TglSampai.EditValue = Today
            If Not IsNew Then
                LoadData()
            End If
            Me.Width = Ini.BacaIniPath(FolderLayouts & Me.Name & ".ini", "Form", "Width", Me.Width)
            Me.Height = Ini.BacaIniPath(FolderLayouts & Me.Name & ".ini", "Form", "Height", Me.Height)

            Me.Left = (Screen.PrimaryScreen.WorkingArea.Width - Me.Width) \ 2
            Me.Top = (Screen.PrimaryScreen.WorkingArea.Height - Me.Height) \ 2
            LoadLayout()
            HighLightTxt()
            FungsiControl.SetForm(Me)
        Catch ex As Exception
            XtraMessageBox.Show(ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            Windows.Forms.Cursor.Current = Cursors.Default
            dlg.Close()
            dlg.Dispose()
        End Try
    End Sub
    Sub HighLightTxt()
        For Each ctrl In LayoutControl1.Controls
            If TypeOf ctrl Is DevExpress.XtraEditors.TextEdit Then
                AddHandler TryCast(ctrl, DevExpress.XtraEditors.TextEdit).GotFocus, AddressOf txt_GotFocus
            End If
        Next
    End Sub
    Private Sub txt_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim txt As DevExpress.XtraEditors.TextEdit = TryCast(sender, DevExpress.XtraEditors.TextEdit)
        If txt.Properties.Mask.MaskType = Mask.MaskType.Numeric Then
            txt.SelectAll()
        End If
    End Sub
    Private Sub LoadLayout()
        If Exists(FolderLayouts & Me.Name & LayoutControl1.Name & ".xml") Then
            LayoutControl1.RestoreLayoutFromXml(FolderLayouts & Me.Name & LayoutControl1.Name & ".xml")
        End If
    End Sub
    

    Private Sub GV1_DataSourceChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles GV1.DataSourceChanged
        If System.IO.File.Exists(FolderLayouts & Me.Name & GV1.Name & ".xml") Then
            GV1.RestoreLayoutFromXml(FolderLayouts & Me.Name & GV1.Name & ".xml")
        End If
        With GV1
            For x As Integer = 0 To .Columns.Count - 1
                Select Case .Columns(x).ColumnType.Name.ToLower
                    Case "int32", "int64", "int"
                        .Columns(x).DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
                        .Columns(x).DisplayFormat.FormatString = "n0"
                    Case "decimal", "single", "money", "double"
                        .Columns(x).DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
                        .Columns(x).DisplayFormat.FormatString = "n0"
                    Case "string"
                        .Columns(x).DisplayFormat.FormatType = DevExpress.Utils.FormatType.None
                        .Columns(x).DisplayFormat.FormatString = ""
                    Case "date", "datetime"
                        If .Columns(x).FieldName.Trim.ToLower = "tanggal" Then
                            .Columns(x).DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime
                            .Columns(x).DisplayFormat.FormatString = "dd-MM-yyyy HH:mm"
                        Else
                            .Columns(x).DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime
                            .Columns(x).DisplayFormat.FormatString = "dd-MM-yyyy"
                        End If
                End Select
            Next
        End With
    End Sub

    Private Sub GV2_DataSourceChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles GV2.DataSourceChanged
        If System.IO.File.Exists(FolderLayouts & Me.Name & GV2.Name & ".xml") Then
            GV2.RestoreLayoutFromXml(FolderLayouts & Me.Name & GV2.Name & ".xml")
        End If
        With GV2
            For x As Integer = 0 To .Columns.Count - 1
                Select Case .Columns(x).ColumnType.Name.ToLower
                    Case "int32", "int64", "int"
                        .Columns(x).DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
                        .Columns(x).DisplayFormat.FormatString = "n0"
                    Case "decimal", "single", "money", "double"
                        .Columns(x).DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
                        .Columns(x).DisplayFormat.FormatString = "n0"
                    Case "string"
                        .Columns(x).DisplayFormat.FormatType = DevExpress.Utils.FormatType.None
                        .Columns(x).DisplayFormat.FormatString = ""
                    Case "date", "datetime"
                        If .Columns(x).FieldName.Trim.ToLower = "tanggal" Then
                            .Columns(x).DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime
                            .Columns(x).DisplayFormat.FormatString = "dd-MM-yyyy HH:mm"
                        Else
                            .Columns(x).DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime
                            .Columns(x).DisplayFormat.FormatString = "dd-MM-yyyy"
                        End If
                End Select
            Next
        End With
    End Sub

    Private Sub GV3_DataSourceChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles GV3.DataSourceChanged
        If System.IO.File.Exists(FolderLayouts & Me.Name & GV3.Name & ".xml") Then
            GV3.RestoreLayoutFromXml(FolderLayouts & Me.Name & GV3.Name & ".xml")
        End If
        With GV3
            For x As Integer = 0 To .Columns.Count - 1
                Select Case .Columns(x).ColumnType.Name.ToLower
                    Case "int32", "int64", "int"
                        .Columns(x).DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
                        .Columns(x).DisplayFormat.FormatString = "n0"
                    Case "decimal", "single", "money", "double"
                        .Columns(x).DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
                        .Columns(x).DisplayFormat.FormatString = "n0"
                    Case "string"
                        .Columns(x).DisplayFormat.FormatType = DevExpress.Utils.FormatType.None
                        .Columns(x).DisplayFormat.FormatString = ""
                    Case "date", "datetime"
                        If .Columns(x).FieldName.Trim.ToLower = "tanggal" Then
                            .Columns(x).DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime
                            .Columns(x).DisplayFormat.FormatString = "dd-MM-yyyy HH:mm"
                        Else
                            .Columns(x).DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime
                            .Columns(x).DisplayFormat.FormatString = "dd-MM-yyyy"
                        End If
                End Select
            Next
        End With
    End Sub

    Private Sub GV4_DataSourceChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles GV4.DataSourceChanged
        If System.IO.File.Exists(FolderLayouts & Me.Name & GV4.Name & ".xml") Then
            GV4.RestoreLayoutFromXml(FolderLayouts & Me.Name & GV4.Name & ".xml")
        End If
        With GV4
            For x As Integer = 0 To .Columns.Count - 1
                Select Case .Columns(x).ColumnType.Name.ToLower
                    Case "int32", "int64", "int"
                        .Columns(x).DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
                        .Columns(x).DisplayFormat.FormatString = "n0"
                    Case "decimal", "single", "money", "double"
                        .Columns(x).DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
                        .Columns(x).DisplayFormat.FormatString = "n0"
                    Case "string"
                        .Columns(x).DisplayFormat.FormatType = DevExpress.Utils.FormatType.None
                        .Columns(x).DisplayFormat.FormatString = ""
                    Case "date", "datetime"
                        If .Columns(x).FieldName.Trim.ToLower = "tanggal" Then
                            .Columns(x).DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime
                            .Columns(x).DisplayFormat.FormatString = "dd-MM-yyyy HH:mm"
                        Else
                            .Columns(x).DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime
                            .Columns(x).DisplayFormat.FormatString = "dd-MM-yyyy"
                        End If
                End Select
            Next
        End With
    End Sub

    Private Sub txtKodeCustomer1_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtKodeCustomer1.EditValueChanged
        txtNama1.Text = NullToStr(EksekusiSQlSkalarNew("select Nama from MAlamat where NoID=" & NullToLong(txtKodeCustomer1.EditValue)))

    End Sub

    Private Sub txtKodeCustomer2_EditValueChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtKodeCustomer2.EditValueChanged
        txtNama2.Text = NullToStr(EksekusiSQlSkalarNew("select Nama from MAlamat where NoID=" & NullToLong(txtKodeCustomer2.EditValue)))
    End Sub

    Private Sub txtKodeCustomer3_EditValueChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtKodeCustomer3.EditValueChanged
        txtNama3.Text = NullToStr(EksekusiSQlSkalarNew("select Nama from MAlamat where NoID=" & NullToLong(txtKodeCustomer3.EditValue)))
    End Sub

    Private Sub txtKodeCustomer4_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtKodeCustomer4.EditValueChanged
        txtNama4.Text = NullToStr(EksekusiSQlSkalarNew("select Nama from MAlamat where NoID=" & NullToLong(txtKodeCustomer4.EditValue)))
    End Sub

    Private Sub CetakMRPTJual(ByVal Action As action_, ByVal Loket As Integer)
        Dim namafile As String
        Dim strsql As String = ""
        Try

            namafile = Application.StartupPath & "\report\TandaTerimaFee.rpt"
            If System.IO.File.Exists(namafile) Then
                If EditReport Then
                    Action = action_.Edit
                Else
                    Action = action_.Preview
                End If
                Select Case Loket
                    Case 1
                        ViewReport(Me.ParentForm, Action, namafile, Me.Text, , , "DariTanggal=cdate(" & TglDari.DateTime.ToString("yyyy,MM,dd") & ")&SampaiTanggal=cdate(" & TglSampai.DateTime.ToString("yyyy,MM,dd") & ")&NoCrew='" & FixApostropi(NullToStr(txtKodeCustomer1.Text)) & "'&Total=" & FixKoma(txtTotal1.EditValue) & "&Penerima='" & FixApostropi(NullToStr(txtNama1.Text)) & "'&Fee=" & NullToLong(GV1.Columns("FeeMarketingRp").SummaryItem.SummaryValue))
                    Case 2
                        ViewReport(Me.ParentForm, Action, namafile, Me.Text, , , "DariTanggal=cdate(" & TglDari.DateTime.ToString("yyyy,MM,dd") & ")&SampaiTanggal=cdate(" & TglSampai.DateTime.ToString("yyyy,MM,dd") & ")&NoCrew='" & FixApostropi(NullToStr(txtKodeCustomer2.Text)) & "'&Total=" & FixKoma(txtTotal2.EditValue) & "&Penerima='" & FixApostropi(NullToStr(txtNama2.Text)) & "'&Fee=" & NullToLong(GV2.Columns("FeeMarketingRp").SummaryItem.SummaryValue))
                    Case 3
                        ViewReport(Me.ParentForm, Action, namafile, Me.Text, , , "DariTanggal=cdate(" & TglDari.DateTime.ToString("yyyy,MM,dd") & ")&SampaiTanggal=cdate(" & TglSampai.DateTime.ToString("yyyy,MM,dd") & ")&NoCrew='" & FixApostropi(NullToStr(txtKodeCustomer3.Text)) & "'&Total=" & FixKoma(txtTotal3.EditValue) & "&Penerima='" & FixApostropi(NullToStr(txtNama3.Text)) & "'&Fee=" & NullToLong(GV3.Columns("FeeMarketingRp").SummaryItem.SummaryValue))
                    Case 4
                        ViewReport(Me.ParentForm, Action, namafile, Me.Text, , , "DariTanggal=cdate(" & TglDari.DateTime.ToString("yyyy,MM,dd") & ")&SampaiTanggal=cdate(" & TglSampai.DateTime.ToString("yyyy,MM,dd") & ")&NoCrew='" & FixApostropi(NullToStr(txtKodeCustomer4.Text)) & "'&Total=" & FixKoma(txtTotal4.EditValue) & "&Penerima='" & FixApostropi(NullToStr(txtNama4.Text)) & "'&Fee=" & NullToLong(GV4.Columns("FeeMarketingRp").SummaryItem.SummaryValue))
                End Select

            Else
                DevExpress.XtraEditors.XtraMessageBox.Show("Nama File :" & namafile & " tidak ditemukan.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Stop)
            End If
        Catch EX As Exception
            XtraMessageBox.Show("Untuk mencetak, pilih item yang mau dicetak dan click cetak faktur.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
            'DevExpress.XtraEditors.XtraMessageBox.Show("Kesalahan:" & EX.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub SimpleButton4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SimpleButton4.Click
        CetakMRPTJual(action_.Preview, 4)
    End Sub

    Private Sub SimpleButton2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SimpleButton2.Click
        CetakMRPTJual(action_.Preview, 2)
    End Sub

    Private Sub SimpleButton1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SimpleButton1.Click
        CetakMRPTJual(action_.Preview, 1)
    End Sub

    Private Sub SimpleButton3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SimpleButton3.Click
        CetakMRPTJual(action_.Preview, 3)
    End Sub

    Private Sub Timer1_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer1.Tick
        LoadData()
    End Sub
End Class