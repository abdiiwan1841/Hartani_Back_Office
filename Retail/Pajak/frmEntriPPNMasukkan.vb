Imports System.Data
Imports System.Data.SqlClient
Imports DevExpress.XtraEditors
Imports DevExpress.Utils
Imports System.IO.File

Public Class frmEntriPPNMasukkan
    Public IsNew As Boolean = True
    Public IsPosted As Boolean = False
    Public NoID As Long = -1 
    'Public IDSupplier As Long = -1 

    Private Sub IsiDefault()
        cmdSave.ImageList = frmMain.ImageList1
        cmdSave.ImageIndex = 6

        cmdTutup.ImageList = frmMain.ImageList1
        cmdTutup.ImageIndex = 3

        'TgMasapajak.DateTime = CDate(Format(TanggalSystem, "yyyy,MM,01"))
        TglFP.DateTime = TanggalSystem
    End Sub
    Private Sub RefreshLookUp()
        Dim ds As New DataSet
        Dim SQL As String = ""
        Try

            SQL = "SELECT MAlamatDNPWP.NoID, MAlamatDNPWP.NPWP,MAlamat.Kode,MAlamatDNPWP.NamaWP,MAlamatDNPWP.IDAlamat from MAlamat inner join MAlamatDNPWP on MAlamatDNPWP.IDAlamat=MAlamat.NoID "
            SQL &= " WHERE MAlamatDNPWP.IsActive=1 "
            ds = ExecuteDataset("MNPWP", SQL)

            txtNPWP.Properties.DataSource = ds.Tables("MNPWP")
            txtNPWP.Properties.ValueMember = "NoID"
            txtNPWP.Properties.DisplayMember = "NPWP"

        Catch ex As Exception
            XtraMessageBox.Show("Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            ds.Dispose()
        End Try
    End Sub
    Sub RefreshNota()
        Dim ds As New DataSet
        Dim SQL As String
        Try
            SQL = "SELECT MBeli.NoID,MBeli.NoFaktur, MBeli.Kode NoBPB, CASE WHEN IsTanpaBarang=1 THEN MBeli.Kode ELSE (SELECT TOP 1 MPO.Kode FROM MPO INNER JOIN MPOD ON MPO.NoID=MPOD.IDPO INNER JOIN MBeliD ON MBeliD.IDPOD=MPOD.NoID WHERE MBeliD.IDBeli=MBeli.NoID) END AS NoSPP, MBeli.Tanggal, MBeli.Total, MBeli.NoFakturPajak, RIGHT(CONVERT(VARCHAR(10), MBeli.MasaPajak, 105),7) AS MassaPajak " & _
                  " From MBeli WHERE MBeli.IsPosted=1 "
            If Not ckAll.Checked Then
                If IsNew Then
                    SQL &= " AND ISNULL(MBeli.IsTerimaFakturPajak,0)=0"
                Else
                    SQL &= " AND (ISNULL(MBeli.IsTerimaFakturPajak,0)=0 OR ISNULL(MBeli.NoID,0)= " & NoID & ")"
                End If
            End If
            ds = ExecuteDataset("MBeli", Sql)
            txtNoFaktur.Properties.DataSource = ds.Tables("MBeli")
            txtNoFaktur.Properties.ValueMember = "NoID"
            txtNoFaktur.Properties.DisplayMember = "NoSPP"
        Catch ex As Exception
            XtraMessageBox.Show("Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            ds.Dispose()
        End Try
    End Sub
    Private Sub LoadData()
        Dim ods As New DataSet

        Dim sql As String
        Try
            sql = "SELECT MBeli.* From MBeli WHERE NoID= " & NoID
            oDS = New DataSet()
            oDS = ExecuteDataset("MBeliOK", Sql)
            If oDS.Tables(0).Rows.Count >= 1 Then
                With oDS.Tables("MBeliOK").Rows(0)
                    NoID = NullToLong(.Item("NoID"))
                    txtNPWP.EditValue = NullToLong(.Item("IDAlamatDNPWP"))
                    txtNoFaktur.EditValue = NoID
                    TgMasapajak.DateTime = NullToDate(.Item("MasaPajak"))
                    TglFP.DateTime = NullToDate(.Item("TglFakturPajak"))
                    tglTerimaFP.DateTime = NullToDate(.Item("TglTerimaFakturPajak"))

                    txtDPP.EditValue = NullToDbl(.Item("DPP"))
                    txtPPN.EditValue = NullToDbl(.Item("PPN"))
                    txtTotal.EditValue = NullToDbl(.Item("TotalPajak"))
                    txtBendel.EditValue = NullToDbl(.Item("NoBendelPajak"))
                    txtCatatan.Text = NullToStr(.Item("CatatanFP"))
                    'txtTotal.EditValue = Bulatkan(NullToDbl(.Item("Total")), 0)
                    'HitungTotal()

                    txtNoDokumen1.Text = NullToStr(.Item("NoFakturPajak")).Substring(0, 3)
                    txtNoDokumen2.Text = NullToStr(.Item("NoFakturPajak")).Substring(4, 3)
                    txtNoDokumen3.Text = NullToStr(.Item("NoFakturPajak")).Substring(8, 2)
                    txtNoDokumen4.Text = NullToStr(.Item("NoFakturPajak")).Substring(11, 8)
                    txtNoFP.Text = NullToStr(.Item("NoFakturPajak"))
                End With
            Else
                IsiDefault()
            End If
            ods.Dispose()
        Catch ex As Exception
            XtraMessageBox.Show("Kesalahan: " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            oDS.Dispose()
        End Try
    End Sub

    Private Sub cmdSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSave.Click
        If IsValidasi() Then
            If Simpan() Then
                DialogResult = Windows.Forms.DialogResult.OK
                Me.Close()
            End If
        End If
    End Sub
    Private Function Simpan() As Boolean
        Dim SQL As String
        Try
            SQL = "UPDATE MBeli SET "
            SQL &= " IsTerimaFakturPajak=1,"
            SQL &= " IDUserEditFP=" & IDUserAktif & ","
            SQL &= " TglEditFP='" & TanggalSystem.ToString("yyyy-MM-dd HH:mm") & "',"
            SQL &= " MasaPajak='" & Format(TgMasapajak.DateTime, "yyyy-MM-1") & "', "
            SQL &= " TglFakturPajak='" & Format(TglFP.DateTime, "yyyy-MM-dd") & "', "
            SQL &= " TglTerimaFakturPajak='" & Format(tglTerimaFP.DateTime, "yyyy-MM-dd") & "', "
            SQL &= " NoFakturPajak='" & FixApostropi(txtNoFP.Text) & "', "
            SQL &= " IDAlamatDNPWP=" & NullToLong(txtNPWP.EditValue) & ","
            SQL &= " DPP=" & FixKoma(txtDPP.EditValue) & ","
            SQL &= " PPN=" & FixKoma(txtPPN.EditValue) & ","
            SQL &= " NoBendelPajak=" & FixKoma(txtBendel.EditValue) & ","
            SQL &= " TotalPajak=" & FixKoma(txtTotal.EditValue) & ","
            SQL &= " CatatanFP='" & FixApostropi(txtCatatan.Text) & "' "
            SQL &= " WHERE NoID=" & NoID
            If EksekusiSQL(SQL) >= 1 Then
                Return True
            Else
                Return False
            End If
        Catch ex As Exception
            XtraMessageBox.Show("Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return False
        End Try
    End Function
    Private Function IsValidasi() As Boolean
        If CInt(txtNoDokumen3.Text) - CInt(TgMasapajak.DateTime.ToString("yy")) < -1 Or CInt(txtNoDokumen3.Text) - CInt(TgMasapajak.DateTime.ToString("yy")) > 0 Then
            If XtraMessageBox.Show("No Faktur Pajak dan Massa Pajak tidak sesuai." & vbCrLf & "Ingin melanjutkan penyimpanan?", NamaAplikasi, MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) = Windows.Forms.DialogResult.No Then
                txtNoDokumen3.Focus()
                Return False
                Exit Function
            End If
        End If
        If clsPostingPembelian.IsLockPeriodeFP(TgMasapajak.DateTime) Then
            XtraMessageBox.Show("Masa Pajak " & TgMasapajak.DateTime.ToString("MMMM-yyyy") & " sudah dikunci.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
            TgMasapajak.Focus()
            Return False
            Exit Function
        End If
        If txtBendel.EditValue <= 0 Then
            If XtraMessageBox.Show("No bendel masih belum diisi." & vbCrLf & "Ingin melanjutkan penyimpanan?", NamaAplikasi, MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) = Windows.Forms.DialogResult.No Then
                txtBendel.Focus()
                Return False
                Exit Function
            End If
        End If
        If txtNPWP.Text = "" Then
            XtraMessageBox.Show("NPWP masih kosong.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
            txtNPWP.Focus()
            Return False
            Exit Function
        End If
        If txtNoFaktur.Text = "" Then
            XtraMessageBox.Show("No Faktur masih kosong.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
            txtNPWP.Focus()
            Return False
            Exit Function
        End If
        If txtNoFP.Text = "" Then
            XtraMessageBox.Show("No Faktur Pajak masih kosong.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
            txtNPWP.Focus()
            Return False
            Exit Function
        End If
        If txtPPN.EditValue <= 0 Then
            If XtraMessageBox.Show("PPN masih kurang atau nol." & vbCrLf & "Ingin melanjutkan penyimpanan?", NamaAplikasi, MessageBoxButtons.YesNo, MessageBoxIcon.Error, MessageBoxDefaultButton.Button2) = Windows.Forms.DialogResult.No Then
                txtPPN.Focus()
                Return False
                Exit Function
            End If
        End If
        If txtDPP.EditValue <= 0 Then
            If XtraMessageBox.Show("DPP masih kurang atau nol." & vbCrLf & "Ingin melanjutkan penyimpanan?", NamaAplikasi, MessageBoxButtons.YesNo, MessageBoxIcon.Error, MessageBoxDefaultButton.Button2) = Windows.Forms.DialogResult.No Then
                txtDPP.Focus()
                Return False
                Exit Function
            End If
        End If
        If IsNew AndAlso NullToBool(EksekusiSQlSkalarNew("SELECT IsTerimaFakturPajak FROM MBeli WHERE NoID=" & NoID)) Then
            XtraMessageBox.Show("No Faktur Pajak Sudah Dientri.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Warning)
            txtNoFaktur.Focus()
            Return False
            Exit Function
        End If
        Return True
    End Function
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
                gvBarang.SaveLayoutToXml(FolderLayouts & Me.Name & gvBarang.Name & ".xml")
                gvGudang.SaveLayoutToXml(FolderLayouts & Me.Name & gvGudang.Name & ".xml")
                gvBeli.SaveLayoutToXml(FolderLayouts & Me.Name & gvBeli.Name & ".xml")
            End If
        Catch ex As Exception
            XtraMessageBox.Show("Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            xOtorisasi.Dispose()
        End Try
    End Sub
    Private Sub LoadLayout()
        If Exists(FolderLayouts & Me.Name & LayoutControl1.Name & ".xml") Then
            LayoutControl1.RestoreLayoutFromXml(FolderLayouts & Me.Name & LayoutControl1.Name & ".xml")
        End If
    End Sub

    Private Sub frmEntriBeliD_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim dlg As DevExpress.Utils.WaitDialogForm = Nothing
        Try
            Dim curentcursor As Cursor = Windows.Forms.Cursor.Current
            Windows.Forms.Cursor.Current = Cursors.WaitCursor
            dlg = New DevExpress.Utils.WaitDialogForm(String.Format("Creating component and analize database.{0}MOHON TUNGGU ...", vbCrLf), Application.ProductName)
            dlg.TopMost = False
            dlg.Show()
            Windows.Forms.Cursor.Current = Cursors.WaitCursor
            IsiDefault()
            RefreshNota()
            RefreshLookUp()
            If Not IsNew Then
                LoadData()
            End If
            Me.Width = Ini.BacaIniPath(FolderLayouts & Me.Name & ".ini", "Form", "Width", Me.Width)
            Me.Height = Ini.BacaIniPath(FolderLayouts & Me.Name & ".ini", "Form", "Height", Me.Height)

            Me.Left = (Screen.PrimaryScreen.WorkingArea.Width - Me.Width) \ 2
            Me.Top = (Screen.PrimaryScreen.WorkingArea.Height - Me.Height) \ 2
            LoadLayout()
            FungsiControl.SetForm(Me)
            TgMasapajak.Properties.EditMask = "MMMM-yyyy"
            HighLightTxt()
        Catch ex As Exception
            XtraMessageBox.Show(ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            Windows.Forms.Cursor.Current = Cursors.Default
            dlg.Close()
            dlg.Dispose()
        End Try
    End Sub
    Private Sub HighLightTxt()
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
    Private Sub txtBarang_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtNPWP.EditValueChanged
        Dim Ds As New DataSet
        Try
            'IDSupplier = NullToLong(EksekusiSQlSkalarNew("Select IDAlamat from MAlamatDNPWP where NoID=" & NullToLong(txtNPWP.EditValue)))
            txtNamaWP.Text = NullToStr(EksekusiSQlSkalarNew("Select NamaWP from MAlamatDNPWP where NoID=" & NullToLong(txtNPWP.EditValue)))
            'RefreshNota()
        Catch ex As Exception
            XtraMessageBox.Show("Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            Ds.Dispose()
        End Try
    End Sub

    
    Private Sub txtNoFaktur_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtNoFaktur.EditValueChanged
        '0:      Non(BKP)
        '1:      Include()
        '2:      Exclude()
        Dim Ds As New DataSet
        Dim Sql As String
        Try
            If IsNew Then
                Sql = "SELECT MBeli.*, MPO.IDTypePajak AS IDTypePajakPO " & vbCrLf & _
                      " FROM (MBeli LEFT JOIN MBeliD ON MBeli.NoID=MBeliD.IDBeli) LEFT JOIN (MPO INNER JOIN MPOD ON MPO.NoID=MPOD.IDPO) ON MBeliD.IDPOD=MPOD.NoID " & vbCrLf & _
                      " WHERE MBeli.NoID= " & NullToLong(txtNoFaktur.EditValue)
                Ds = New DataSet()
                Ds = ExecuteDataset("MBeliD", Sql)
                If Ds.Tables(0).Rows.Count >= 1 Then
                    With Ds.Tables("MBeliD").Rows(0)
                        If NullToBool(.Item("IsTerimaFakturPajak")) Then
                            XtraMessageBox.Show("No Faktur Pajak Sudah Di Entri." & vbCrLf & "No Faktur Pajak : " & NullToStr(.Item("NoFakturPajak")) & ", Massa Pajak : " & NullToDate(.Item("MasaPajak")).ToString("MMMM-yyyy"), NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Warning)
                            txtDPP.EditValue = 0
                            txtPPN.EditValue = 0
                            txtTotal.EditValue = 0
                            cmdSave.Enabled = False
                        ElseIf DateDiff(DateInterval.Month, CDate(NullToDate(.Item("Tanggal")).ToString("yyyy-MM-01")), CDate(TgMasapajak.DateTime.ToString("yyyy-MM-01"))) > 3 Then
                            XtraMessageBox.Show("Masa Pajak Sudah Melebihi 3 Bulan.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Warning)
                            txtDPP.EditValue = 0
                            txtPPN.EditValue = 0
                            txtTotal.EditValue = 0
                            cmdSave.Enabled = False
                        ElseIf DateDiff(DateInterval.Month, CDate(NullToDate(.Item("Tanggal")).ToString("yyyy-MM-01")), CDate(TgMasapajak.DateTime.ToString("yyyy-MM-01"))) < 0 Then
                            XtraMessageBox.Show("Masa Pajak belum saatnya.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Warning)
                            txtDPP.EditValue = 0
                            txtPPN.EditValue = 0
                            txtTotal.EditValue = 0
                            cmdSave.Enabled = False
                        Else
                            gvBeli.SaveLayoutToXml(FolderLayouts & Me.Name & gvBeli.Name & ".xml")
                            NoID = NullToLong(.Item("NoID"))
                            txtNPWP.EditValue = NullToLong(EksekusiSQlSkalarNew("SELECT TOP 1 NoID FROM MAlamatDNPWP WHERE IDAlamat=" & NullToLong(.Item("IDSupplier")) & " AND IsActive=1"))
                            If NullToLong(.Item("IDTypePajakPO")) = 1 Then '1 Include
                                txtDPP.EditValue = Bulatkan(NullToDbl(.Item("Total")) / 1.1, 0)
                                txtPPN.EditValue = Bulatkan(NullToDbl(.Item("Total")), 0) - txtDPP.EditValue
                            ElseIf NullToLong(.Item("IDTypePajakPO")) = 2 Then '2 exclude
                                txtDPP.EditValue = Bulatkan(NullToDbl(.Item("Total")), 0)
                                txtPPN.EditValue = Bulatkan(txtDPP.EditValue * 0.1, 0)
                            Else
                                txtDPP.EditValue = Bulatkan(NullToDbl(.Item("Total")), 0)
                                txtPPN.EditValue = 0
                            End If
                            txtTotal.EditValue = Bulatkan(NullToDbl(.Item("Total")), 0)
                            cmdSave.Enabled = True
                        End If
                    End With
                Else

                End If
            End If
            'RefreshLookUp()
        Catch ex As Exception
            XtraMessageBox.Show("Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            Ds.Dispose()
        End Try
    End Sub

    Private Sub BarButtonItem1_ItemClick_1(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem1.ItemClick
        cmdSave.PerformClick()
    End Sub

    Private Sub BarButtonItem2_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem2.ItemClick
        cmdTutup.PerformClick()
    End Sub


    Private Sub gvBeli_DataSourceChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvBeli.DataSourceChanged
        If System.IO.File.Exists(FolderLayouts & Me.Name & gvBeli.Name & ".xml") Then
            gvBeli.RestoreLayoutFromXml(FolderLayouts & Me.Name & gvBeli.Name & ".xml")
        End If
        With gvBeli
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

    
    Private Sub gvBarang_DataSourceChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvBarang.DataSourceChanged
        If System.IO.File.Exists(FolderLayouts & Me.Name & gvBarang.Name & ".xml") Then
            gvBarang.RestoreLayoutFromXml(FolderLayouts & Me.Name & gvBarang.Name & ".xml")
        End If
        With gvBarang
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

    Private Sub gvGudang_DataSourceChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvGudang.DataSourceChanged
        If System.IO.File.Exists(FolderLayouts & Me.Name & gvGudang.Name & ".xml") Then
            gvGudang.RestoreLayoutFromXml(FolderLayouts & Me.Name & gvGudang.Name & ".xml")
        End If
        With gvGudang
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

    Private Sub TgMasapajak_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TgMasapajak.EditValueChanged
        RefreshNota()
        If IsNew Then
            txtNoDokumen1.Text = "010"
            txtNoDokumen2.Text = "000"
            txtNoDokumen3.Text = TgMasapajak.DateTime.ToString("yy")

        Else
            txtNoDokumen3.Text = TgMasapajak.DateTime.ToString("yy")
        End If
    End Sub

    Private Sub txtDPP_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtDPP.EditValueChanged
        txtPPN.EditValue = System.Math.Round(txtDPP.EditValue * 10 / 100, 0)
        'HitungTotal()
    End Sub

    Private Sub txtDPP_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtDPP.KeyDown
        Dim x As New frmOtorisasiAdmin
        Try
            If NullToBool(sender.properties.readonly) AndAlso e.KeyCode = Keys.F7 AndAlso x.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
                txtDPP.Properties.ReadOnly = Not txtDPP.Properties.ReadOnly
                txtPPN.Properties.ReadOnly = Not txtPPN.Properties.ReadOnly
                txtTotal.Properties.ReadOnly = Not txtTotal.Properties.ReadOnly
            End If
        Catch ex As Exception
            XtraMessageBox.Show("Info Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
        Finally
            x.Dispose()
        End Try
    End Sub

    Private Sub txtPPN_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtPPN.KeyDown
        Dim x As New frmOtorisasiAdmin
        Try
            If NullToBool(sender.properties.readonly) AndAlso e.KeyCode = Keys.F7 AndAlso x.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
                txtDPP.Properties.ReadOnly = Not txtDPP.Properties.ReadOnly
                txtPPN.Properties.ReadOnly = Not txtPPN.Properties.ReadOnly
                txtTotal.Properties.ReadOnly = Not txtTotal.Properties.ReadOnly
            End If
        Catch ex As Exception
            XtraMessageBox.Show("Info Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
        Finally
            x.Dispose()
        End Try
    End Sub

    Private Sub txtPPN_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtPPN.EditValueChanged
        'HitungTotal()
    End Sub

    'Private Sub HitungTotal()
    '    Try
    '        txtTotal.EditValue = Bulatkan(txtDPP.EditValue + txtPPN.EditValue, 0)
    '    Catch ex As Exception

    '    End Try
    'End Sub

    Private Sub ckAll_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ckAll.CheckedChanged
        RefreshNota()
    End Sub

    Private Sub txtNPWP_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtNPWP.LostFocus
        'txtNoDokumen4.Focus()
    End Sub

    Private Sub txtNoDokumen1_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtNoDokumen1.EditValueChanged
        BuildNoFP()
    End Sub

    Private Sub txtNoDokumen2_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtNoDokumen2.EditValueChanged
        BuildNoFP()
    End Sub

    Private Sub txtNoDokumen3_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtNoDokumen3.EditValueChanged
        BuildNoFP()
    End Sub

    Private Sub txtNoDokumen4_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtNoDokumen4.EditValueChanged
        BuildNoFP()
    End Sub

    Private Sub BuildNoDokumen()
        Try
            txtNoFP.Text = txtNoDokumen1.Text & "." & txtNoDokumen2.Text & "-" & txtNoDokumen3.Text & "." & txtNoDokumen4.Text
        Catch ex As Exception

        End Try
    End Sub

    Private Sub BuildNoFP()
        'Try
        '    txtNoFP.Text = txtNoDokumen1.Text & "." & txtNoDokumen2.Text & "-" & txtNoDokumen3.Text & "." & txtNoDokumen4.Text

        'Catch ex As Exception

        'End Try
    End Sub

    Private Sub txtNoFP_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtNoFP.EditValueChanged

    End Sub
End Class