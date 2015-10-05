Imports System.Data
Imports System.Data.SqlClient
Imports DevExpress.XtraEditors
Imports DevExpress.Utils

Public Class frmEntriRuleMutasi
    Public IsNew As Boolean = True
    Public NoID As Long = -1

    Dim oDS As New DataSet
    Dim SQL As String = ""
    Dim DefImageList As ImageList
    Dim IDPegawaiLama As Long = -1

    Private Sub IsiDefault()
        DefImageList = frmMain.ImageList1
        cmdSave.ImageList = DefImageList
        cmdSave.ImageIndex = 6

        cmdTutup.ImageList = DefImageList
        cmdTutup.ImageIndex = 3
        RefreshLookUp()
    End Sub
    Private Sub RefreshLookUp()
        Dim ds As New DataSet
        Try
            SQL = "SELECT MAlamat.NoID, MAlamat.Kode, MAlamat.Nama FROM MAlamat WHERE MAlamat.IsActive=1 AND MAlamat.IsPegawai=1 "
            ds = ExecuteDataset("MAlamat", SQL)
            txtKodePegawai.Properties.DataSource = ds.Tables("MAlamat")
            txtKodePegawai.Properties.ValueMember = "NoID"
            txtKodePegawai.Properties.DisplayMember = "Nama"
            InsertkanDetil()
        Catch ex As Exception
            XtraMessageBox.Show("Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            ds.Dispose()
        End Try
    End Sub
    
    Private Sub LoadData()
        Try
            SQL = "SELECT MRuleMutasi.*, MAlamat.Nama AS Pegawai "
            SQL &= " FROM MRuleMutasi "
            SQL &= " LEFT JOIN MAlamat ON MAlamat.NoID=MRuleMutasi.IDPegawai "
            SQL &= " WHERE MRuleMutasi.NoID= " & NoID
            oDS = New DataSet()
            oDS = ExecuteDataset("IDGudangAsal", SQL)
            If oDS.Tables(0).Rows.Count >= 1 Then
                With oDS.Tables("IDGudangAsal").Rows(0)
                    NoID = NullToLong(.Item("NoID"))
                    txtKodePegawai.EditValue = NullToLong(.Item("IDPegawai"))
                    IDPegawaiLama = NullToLong(.Item("IDPegawai"))
                    txtNamaPegawai.Text = NullToStr(.Item("Pegawai"))
                    txtCatatan.Text = NullToStr(.Item("Keterangan"))
                    ckActive.Checked = NullToBool(.Item("IsActive"))
                End With
            Else
                IsiDefault()
            End If
        Catch ex As Exception
            XtraMessageBox.Show("Kesalahan: " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            oDS.Dispose()
            InsertkanDetil()
        End Try
    End Sub
    Public Sub RefreshDetil()
        Dim strsql As String
        Dim curentcursor As Cursor = Windows.Forms.Cursor.Current
        Dim repChekEdit As New DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit
        Windows.Forms.Cursor.Current = Cursors.WaitCursor
        Try
            strsql = "SELECT MRuleMutasiD.*, MGudang.Nama AS GudangAsal, MWilayah.Nama AS Wilayah " & _
                     " FROM MRuleMutasiD " & _
                     " LEFT JOIN MGudang ON MGudang.NoID=MRuleMutasiD.IDGudangAsal" & _
                     " LEFT JOIN MWilayah ON MWilayah.NoID=MGudang.IDWilayah" & vbCrLf & _
                     " WHERE MRuleMutasiD.IDRuleMutasi=" & NoID & " AND MRuleMutasiD.IsAsal=1"
            ExecuteDBGrid(GC1, strsql, "NoID")
            For x As Integer = 0 To GV1.Columns.Count - 1
                Select Case GV1.Columns(x).ColumnType.Name.ToLower
                    Case "int32", "int64", "int"
                        GV1.Columns(x).DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
                        GV1.Columns(x).DisplayFormat.FormatString = "n0"
                    Case "decimal", "single", "money", "double"
                        GV1.Columns(x).DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
                        GV1.Columns(x).DisplayFormat.FormatString = "n2"
                    Case "string"
                        GV1.Columns(x).DisplayFormat.FormatType = DevExpress.Utils.FormatType.None
                        GV1.Columns(x).DisplayFormat.FormatString = ""
                    Case "date", "datetime"
                        If GV1.Columns(x).FieldName.Trim.ToLower = "jam" Then
                            GV1.Columns(x).DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime
                            GV1.Columns(x).DisplayFormat.FormatString = "HH:mm"
                        Else
                            GV1.Columns(x).DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime
                            GV1.Columns(x).DisplayFormat.FormatString = "dd-MM-yyyy"
                        End If
                    Case "boolean"
                        GV1.Columns(x).ColumnEdit = repChekEdit
                End Select
            Next
            GV1.HideFindPanel()
        Catch ex As Exception
            XtraMessageBox.Show(ex.Message, ".:: Pesan Kesalahan ::.", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
        Windows.Forms.Cursor.Current = curentcursor
        Application.DoEvents()
    End Sub
    Public Sub RefreshDetilTujuan()
        Dim strsql As String
        Dim curentcursor As Cursor = Windows.Forms.Cursor.Current
        Dim repChekEdit As New DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit
        Windows.Forms.Cursor.Current = Cursors.WaitCursor
        Try
            strsql = "SELECT MRuleMutasiD.*, MGudang.Nama AS GudangTujuan, MWilayah.Nama AS Wilayah " & _
                     " FROM MRuleMutasiD " & _
                     " LEFT JOIN MGudang ON MGudang.NoID=MRuleMutasiD.IDGudangTujuan" & _
                     " LEFT JOIN MWilayah ON MWilayah.NoID=MGudang.IDWilayah" & vbCrLf & _
                     " WHERE MRuleMutasiD.IDRuleMutasi=" & NoID & " AND MRuleMutasiD.IsAsal=0 AND MRuleMutasiD.IDGudangAsal=" & NullToLong(GV1.GetRowCellValue(GV1.FocusedRowHandle, "IDGudangAsal"))
            ExecuteDBGrid(GridControl1, strsql, "NoID")
            For x As Integer = 0 To GridView1.Columns.Count - 1
                Select Case GridView1.Columns(x).ColumnType.Name.ToLower
                    Case "int32", "int64", "int"
                        GridView1.Columns(x).DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
                        GridView1.Columns(x).DisplayFormat.FormatString = "n0"
                    Case "decimal", "single", "money", "double"
                        GridView1.Columns(x).DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
                        GridView1.Columns(x).DisplayFormat.FormatString = "n2"
                    Case "string"
                        GridView1.Columns(x).DisplayFormat.FormatType = DevExpress.Utils.FormatType.None
                        GridView1.Columns(x).DisplayFormat.FormatString = ""
                    Case "date", "datetime"
                        If GridView1.Columns(x).FieldName.Trim.ToLower = "jam" Then
                            GridView1.Columns(x).DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime
                            GridView1.Columns(x).DisplayFormat.FormatString = "HH:mm"
                        Else
                            GridView1.Columns(x).DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime
                            GridView1.Columns(x).DisplayFormat.FormatString = "dd-MM-yyyy"
                        End If
                    Case "boolean"
                        GridView1.Columns(x).ColumnEdit = repChekEdit
                End Select
            Next
            GridView1.HideFindPanel()
        Catch ex As Exception
            XtraMessageBox.Show(ex.Message, ".:: Pesan Kesalahan ::.", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
        Windows.Forms.Cursor.Current = curentcursor
        Application.DoEvents()
    End Sub
    Private Sub InsertkanDetil()
        Dim ds As New DataSet
        Try
            SQL = "INSERT INTO MRuleMutasiD (IDRuleMutasi, IsAsal, IDGudangAsal, IDGudangTujuan)" & _
                  " SELECT " & NoID & ", 1, NoID, NULL FROM MGudang WHERE IsActive=1 AND IDWilayah=" & DefIDWilayah & " AND NoID NOT IN (SELECT IDGudangAsal FROM MRuleMutasiD WHERE IDRuleMutasi=" & NoID & ")"
            EksekusiSQL(SQL)
            RefreshDetil()
        Catch ex As Exception
            XtraMessageBox.Show("Info : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK)
        Finally
            ds.Dispose()
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
        Dim IDLama As Long = -1
        Try
            IDLama = NoID
            If IsNew Then
                NoID = NullToLong(GetNewID("MRuleMutasi", "NoID"))
                SQL = "INSERT INTO MRuleMutasi (NoID,IDPegawai,IsActive, Keterangan) VALUES ("
                SQL &= NoID & ","
                SQL &= NullToLong(txtKodePegawai.EditValue) & ","
                SQL &= IIf(ckActive.Checked, 1, 0) & ","
                SQL &= "'" & FixApostropi(txtCatatan.EditValue) & "'"
                SQL &= ")"
            Else
                SQL = "UPDATE MRuleMutasi SET "
                SQL &= "IDPegawai=" & NullToLong(txtKodePegawai.EditValue) & ","
                SQL &= "IsActive=" & IIf(ckActive.Checked, 1, 0) & ","
                SQL &= "Keterangan='" & FixApostropi(txtCatatan.EditValue) & "'"
                SQL &= " WHERE NoID=" & NoID
            End If
            If EksekusiSQL(SQL) >= 1 Then
                EksekusiSQL("UPDATE MRuleMutasiD SET IDRuleMutasi=" & NoID & " WHERE IDRuleMutasi=" & IDLama)
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
        If txtKodePegawai.Text = "" Then
            XtraMessageBox.Show("Nama Pegawai masih kosong.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
            txtKodePegawai.Focus()
            Return False
            Exit Function
        End If
        If CekKodePegawai(NullToLong(txtKodePegawai.EditValue), IsNew) Then
            XtraMessageBox.Show("Nama Pegawai Ini Sudah ada di Rule Mutasi.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
            txtKodePegawai.Focus()
            Return False
            Exit Function
        End If
        Return True
    End Function
    Private Function CekKodePegawai(ByVal IDPegawai As Long, ByVal IsNew As Boolean) As Boolean
        Dim ds As New DataSet
        Try
            If IsNew Then
                SQL = "SELECT * FROM MRuleMutasi WHERE IDPegawai=" & NullToLong(IDPegawai)
            Else
                SQL = "SELECT * FROM MRuleMutasi WHERE IDPegawai=" & NullToLong(IDPegawai) & " AND IDPegawai<>" & NullToLong(IDPegawaiLama)
            End If
            ds = ExecuteDataset("MRuleMutasi", SQL)
            If ds.Tables("MRuleMutasi").Rows.Count >= 1 Then
                Return True
            Else
                Return False
            End If
        Catch ex As Exception
            Return True
        End Try
    End Function
    Private Sub cmdTutup_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdTutup.Click
        DialogResult = Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub BarButtonItem1_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles cmdSaveLayout.ItemClick
        Dim xOtorisasi As New frmOtorisasiAdmin
        Try
            If XtraMessageBox.Show("Simpan Layout?", NamaAplikasi, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = Windows.Forms.DialogResult.Yes AndAlso xOtorisasi.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
                Ini.TulisIniPath(folderLayouts & Me.Name & ".ini", "Form", "Width", Me.Width)
                Ini.TulisIniPath(folderLayouts & Me.Name & ".ini", "Form", "Height", Me.Height)
                Me.Left = (Screen.PrimaryScreen.WorkingArea.Width - Me.Width) \ 2
                Me.Top = (Screen.PrimaryScreen.WorkingArea.Height - Me.Height) \ 2

                LayoutControl1.SaveLayoutToXml(folderLayouts & Me.Name & LayoutControl1.Name & ".xml")
                gvPegawai.SaveLayoutToXml(FolderLayouts & Me.Name & gvPegawai.Name & ".xml")
                GV1.SaveLayoutToXml(FolderLayouts & Me.Name & GV1.Name & ".xml")
                GridView1.SaveLayoutToXml(FolderLayouts & Me.Name & GridView1.Name & ".xml")
            End If
        Catch ex As Exception
            XtraMessageBox.Show("Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            xOtorisasi.Dispose()
        End Try
    End Sub

    Private Sub frmEntriRuleMutasi_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        If DialogResult = Windows.Forms.DialogResult.Cancel AndAlso cmdSave.Enabled Then
            If XtraMessageBox.Show("Yakin ingin keluar?" & vbCrLf & "Cancel untuk membatalkan", NamaAplikasi, MessageBoxButtons.OKCancel, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) = Windows.Forms.DialogResult.Cancel Then
                e.Cancel = True
            End If
        End If
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
            If Not IsNew Then
                LoadData()
            End If
            Me.Width = Ini.BacaIniPath(folderLayouts & Me.Name & ".ini", "Form", "Width", Me.Width)
            Me.Height = Ini.BacaIniPath(folderLayouts & Me.Name & ".ini", "Form", "Height", Me.Height)

            Me.Left = (Screen.PrimaryScreen.WorkingArea.Width - Me.Width) \ 2
            Me.Top = (Screen.PrimaryScreen.WorkingArea.Height - Me.Height) \ 2

            If System.IO.File.Exists(folderLayouts & Me.Name & LayoutControl1.Name & ".xml") Then
                LayoutControl1.RestoreLayoutFromXml(folderLayouts & Me.Name & LayoutControl1.Name & ".xml")
            End If
            If System.IO.File.Exists(FolderLayouts & Me.Name & GV1.Name & ".xml") Then
                GV1.RestoreLayoutFromXml(FolderLayouts & Me.Name & GV1.Name & ".xml")
            End If
            If System.IO.File.Exists(FolderLayouts & Me.Name & GridView1.Name & ".xml") Then
                GridView1.RestoreLayoutFromXml(FolderLayouts & Me.Name & GridView1.Name & ".xml")
            End If
            FungsiControl.SetForm(Me)
            HighLightTxt()
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

    Private Sub txtBarang_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtKodePegawai.EditValueChanged
        Dim Ds As New DataSet
        Try
            SQL = "SELECT MAlamat.Nama AS Pegawai FROM MAlamat WHERE MAlamat.NoID=" & NullToLong(txtKodePegawai.EditValue)
            Ds = ExecuteDataset("Tabel", SQL)
            If Ds.Tables("Tabel").Rows.Count >= 1 Then
                txtNamaPegawai.Text = NullToStr(Ds.Tables(0).Rows(0).Item("Pegawai"))
            Else
                txtNamaPegawai.Text = ""
            End If
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

    Private Sub GV1_CellValueChanged(ByVal sender As Object, ByVal e As DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs) Handles GV1.CellValueChanged
        If GV1.FocusedColumn.FieldName.ToLower = "IsActive".ToLower Then
            SQL = "UPDATE MRuleMutasiD SET IsActive=" & NullTolInt(e.Value) & " WHERE NoID=" & NullToLong(GV1.GetRowCellValue(GV1.FocusedRowHandle, "NoID"))
            EksekusiSQL(SQL)
        End If
    End Sub
    Private Sub GridView1_CellValueChanged(ByVal sender As Object, ByVal e As DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs) Handles GridView1.CellValueChanged
        If GridView1.FocusedColumn.FieldName.ToLower = "IsActive".ToLower Then
            SQL = "UPDATE MRuleMutasiD SET IsActive=" & NullTolInt(e.Value) & " WHERE NoID=" & NullToLong(GridView1.GetRowCellValue(GridView1.FocusedRowHandle, "NoID"))
            EksekusiSQL(SQL)
        End If
    End Sub
    Private Sub GV1_FocusedRowChanged(ByVal sender As Object, ByVal e As DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs) Handles GV1.FocusedRowChanged
        Try
            Dim x As Long = NullToLong(GV1.GetRowCellValue(GV1.FocusedRowHandle, "IDGudangAsal"))
            InsertkanTujuan(x)
        Catch ex As Exception

        End Try
    End Sub
    Private Sub InsertkanTujuan(ByVal IDGudangAsal As Long)
        Dim ds As New DataSet
        Try
            SQL = "INSERT INTO MRuleMutasiD (IDRuleMutasi, IsAsal, IDGudangAsal, IDGudangTujuan)" & _
                  " SELECT " & NoID & ", 0, " & IDGudangAsal & ", NoID FROM MGudang WHERE IsActive=1 AND IDWilayah=" & DefIDWilayah & " AND NoID NOT IN (SELECT IDGudangTujuan FROM MRuleMutasiD WHERE IsNull(IsAsal,0)=0 AND IDRuleMutasi=" & NoID & " AND IDGudangAsal=" & IDGudangAsal & ")"
            EksekusiSQL(SQL)
            RefreshDetilTujuan()
        Catch ex As Exception
            XtraMessageBox.Show("Info : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Question)
        Finally
            ds.Dispose()
        End Try
    End Sub

    Private Sub GridView1_FocusedColumnChanged(ByVal sender As Object, ByVal e As DevExpress.XtraGrid.Views.Base.FocusedColumnChangedEventArgs) Handles GridView1.FocusedColumnChanged
        If GridView1.FocusedColumn.FieldName.ToLower = "IsActive".ToLower Then
            GridView1.OptionsBehavior.Editable = True
        Else
            GridView1.OptionsBehavior.Editable = False
        End If
    End Sub
    Private Sub GV1_FocusedColumnChanged(ByVal sender As Object, ByVal e As DevExpress.XtraGrid.Views.Base.FocusedColumnChangedEventArgs) Handles GV1.FocusedColumnChanged
        If GV1.FocusedColumn.FieldName.ToLower = "IsActive".ToLower Then
            GV1.OptionsBehavior.Editable = True
        Else
            GV1.OptionsBehavior.Editable = False
        End If
    End Sub
End Class