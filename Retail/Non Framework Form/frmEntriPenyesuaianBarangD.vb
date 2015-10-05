Imports System.Data
Imports System.Data.SqlClient
Imports DevExpress.XtraEditors
Imports DevExpress.Utils

Public Class frmEntriPenyesuaianBarangD
    Public IsNew As Boolean = True
    Public IsPosted As Boolean = False
    Public NoID As Long = -1
    Public IDHeader As Long = -1
    Public IDWilayah As Long = -1

    Dim oDS As New DataSet
    Dim SQL As String = ""
    Dim DefImageList As ImageList
    Dim IsiKarton As Double = 0.0

    Private Sub IsiDefault()
        DefImageList = frmMain.ImageList1
        cmdSave.ImageList = DefImageList
        cmdSave.ImageIndex = 6

        cmdTutup.ImageList = DefImageList
        cmdTutup.ImageIndex = 3
        RefreshBarcode(True)
        RefreshLookUp()
    End Sub
    Private Sub gvBarcode_DataSourceChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvBarcode.DataSourceChanged
        If System.IO.File.Exists(FolderLayouts & Me.Name & gvBarcode.Name & ".xml") Then
            gvBarcode.RestoreLayoutFromXml(FolderLayouts & Me.Name & gvBarcode.Name & ".xml")
        End If
        With gvBarcode
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
    Sub RefreshBarcode(ByVal IsForce As Boolean)
        Dim dsPublic As New DataSet
        SQL = "SELECT MBarangD.NoID,MBarangD.Barcode,MBarang.Kode,MBarang.Nama + ' ' + MBarangD.Varian AS Nama,MSatuan.Kode as Satuan,MBarangD.IDSatuan,MBarangD.IDBarang FROM MBarangD inner join MBarang On MBarangD.IDBarang=MBarang.NoID inner join MSatuan On MBarangD.IDSatuan=MSatuan.NoID WHERE MBarang.IsActive=1 AND MBarangD.IsActive=1"
        dsPublic = ExecuteDataset("MBarangD", SQL)
        txtBarcode.Properties.DataSource = DSPublic.Tables("MBarangD")
        txtBarcode.Properties.ValueMember = "NoID"
        txtBarcode.Properties.DisplayMember = "Barcode"
        If System.IO.File.Exists(FolderLayouts & "frmEntriBeliD" & gvBarcode.Name & ".xml") Then
            gvBarcode.RestoreLayoutFromXml(FolderLayouts & "frmEntriBeliD" & gvBarcode.Name & ".xml")
        End If
        With gvBarcode
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
    Private Sub RefreshLookUp()
        Dim ds As New DataSet
        Try
            SQL = "SELECT MBarang.NoID, MBarang.Kode, MBarang.Nama, MSatuan.Nama AS Satuan FROM MBarang LEFT JOIN MSatuan ON MSatuan.NoID=MBarang.IDSatuan WHERE MBarang.IsActive=1 AND NOT (MBarang.KODE='' OR MBarang.KODE IS NULL)"
            ds = ExecuteDataset("MBarang", SQL)
            txtBarang.Properties.DataSource = ds.Tables("MBarang")
            txtBarang.Properties.ValueMember = "NoID"
            txtBarang.Properties.DisplayMember = "Kode"

            SQL = "SELECT MGudang.NoID, MGudang.Kode, MGudang.Nama, MWilayah.Nama AS Wilayah FROM MGudang LEFT JOIN MWilayah ON MWilayah.NoID=MGUdang.IDWilayah WHERE MGudang.IsActive=1 AND MWilayah.NoID=" & IDWilayah
            ds = ExecuteDataset("MWilayah", SQL)
            txtGudang.Properties.DataSource = ds.Tables("MWilayah")
            txtGudang.Properties.ValueMember = "NoID"
            txtGudang.Properties.DisplayMember = "Kode"
        Catch ex As Exception
            XtraMessageBox.Show("Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            ds.Dispose()
        End Try
    End Sub
    Private Sub RefreshLookUpSatuan()
        Dim ds As New DataSet
        Try
            SQL = "SELECT MSatuan.NoID, MSatuan.Kode, MSatuan.Nama FROM MSatuan WHERE MSatuan.IsActive=1 AND MSatuan.NoID IN (SELECT MBarangD.IDSatuan FROM MBarangD WHERE MBarangD.IsJual=1 AND MBarangD.IsActive=1 AND MBarangD.IDBarang=" & NullTolong(txtBarang.EditValue) & ")"
            ds = ExecuteDataset("MSatuan", SQL)
            txtSatuan.Properties.DataSource = ds.Tables("MSatuan")
            txtSatuan.Properties.ValueMember = "NoID"
            txtSatuan.Properties.DisplayMember = "Kode"
        Catch ex As Exception
            XtraMessageBox.Show("Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            ds.Dispose()
        End Try
    End Sub
    Private Sub LoadData()
        Try
            SQL = "SELECT MPenyesuaianD.*, MBarang.CtnPcs, MBarang.Kode AS KodeBarang, MBarang.Nama AS NamaBarang, MSatuan.Nama AS NamaSatuan "
            SQL &= " FROM (MPenyesuaianD LEFT JOIN MBarang ON MBarang.NoID=MPenyesuaianD.IDBarang) "
            SQL &= " LEFT JOIN MSatuan ON MSatuan.NoID=MPenyesuaianD.IDSatuan "
            SQL &= " WHERE MPenyesuaianD.NoID= " & NoID
            oDS = New DataSet()
            oDS = ExecuteDataset("MPOD", SQL)
            If oDS.Tables(0).Rows.Count >= 1 Then
                With oDS.Tables("MPOD").Rows(0)
                    NoID = NullTolong(.Item("NoID"))
                    IDHeader = NullToLong(.Item("IDHeader"))
                    txtBarcode.EditValue = NullToLong(.Item("IDBarangD"))
                    txtBarang.EditValue = NullTolong(.Item("IDBarang"))
                    txtSatuan.EditValue = NullTolong(.Item("IDSatuan"))
                    txtgudang.EditValue = NullTolong(.Item("IDGudang"))
                    txtQty.EditValue = NullToDbl(.Item("Qty"))
                    txtQtyPcs.EditValue = NullToDbl(.Item("QtyPcs"))
                    txtCatatan.Text = NullTostr(.Item("Keterangan"))
                    txtKonversi.EditValue = NullToDbl(.Item("Konversi"))
                    txtHargaPokok.EditValue = NullToDbl(.Item("HargaPokok"))
                    txtJumlah.EditValue = NullToDbl(.Item("Jumlah"))
                    IsiKarton = NullToDbl(.Item("CtnPcs"))
                    HitungJumlah()
                End With

            Else
                IsiDefault()
            End If
        Catch ex As Exception
            XtraMessageBox.Show("Kesalahan: " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            oDS.Dispose()
        End Try
    End Sub

    Private Sub cmdSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSave.Click
        If IsValidasi() Then
            HitungJumlah()
            If Simpan() Then
                DialogResult = Windows.Forms.DialogResult.OK
                Me.Close()
            End If
        End If
    End Sub
    Private Function Simpan() As Boolean
        Try
            If IsNew Then
                SQL = "INSERT INTO [MPenyesuaianD] ([NoID],[IDHeader],[IDBarang],[IDGudang],[IDSatuan],[Konversi],[Qty],[QtyPCS],[Keterangan],[HargaPokok],[Jumlah],[IDBarangD]) VALUES ("
                SQL &= NullTolong(GetNewID("MPenyesuaianD", "NoID")) & ","
                SQL &= IDHeader & ","
                SQL &= NullTolong(txtBarang.EditValue) & ","
                SQL &= NullTolong(txtGudang.EditValue) & ","
                SQL &= NullTolong(txtSatuan.EditValue) & ","
                SQL &= FixKoma(txtKonversi.EditValue) & ","
                SQL &= FixKoma(txtQty.EditValue) & ","
                SQL &= FixKoma(txtQtyPcs.EditValue) & ","
                SQL &= "'" & FixApostropi(txtCatatan.Text) & "',"
                SQL &= FixKoma(txtHargaPokok.EditValue) & ","
                SQL &= FixKoma(txtJumlah.EditValue) & ","
                SQL &= NullToLong(txtBarcode.EditValue) & ""
                SQL &= ")"
            Else
                SQL = "UPDATE MPenyesuaianD SET "
                SQL &= " IDBarang=" & NullTolong(txtBarang.EditValue) & ","
                SQL &= " IDGudang=" & NullTolong(txtGudang.EditValue) & ","
                SQL &= " IDSatuan=" & NullTolong(txtSatuan.EditValue) & ","
                SQL &= " Konversi=" & FixKoma(txtKonversi.EditValue) & ","
                SQL &= " Qty=" & FixKoma(txtQty.EditValue) & ","
                SQL &= " QtyPcs=" & FixKoma(txtQtyPcs.EditValue) & ","
                SQL &= " Keterangan='" & FixApostropi(txtCatatan.Text) & "',"
                SQL &= " HargaPokok=" & FixKoma(txtHargaPokok.EditValue) & ","
                SQL &= " Jumlah=" & FixKoma(txtJumlah.EditValue) & ","
                SQL &= " IDBarangD=" & NullToLong(txtBarcode.EditValue) & ""
                SQL &= " WHERE NoID=" & NoID
            End If
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
        If txtBarang.Text = "" Then
            XtraMessageBox.Show("Barang masih kosong.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
            txtBarang.Focus()
            Return False
            Exit Function
        End If
        If txtSatuan.Text = "" Then
            XtraMessageBox.Show("Satuan masih kosong.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
            txtSatuan.Focus()
            Return False
            Exit Function
        End If
        'If txtQty.EditValue <= 0 Then
        '    XtraMessageBox.Show("Qty masih kurang dari atau sama dengan 0.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
        '    txtQty.Focus()
        '    Return False
        '    Exit Function
        'End If
        If txtKonversi.EditValue < 0 Then
            XtraMessageBox.Show("Konversi tidak boleh kurang dari 0.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
            txtKonversi.Focus()
            Return False
            Exit Function
        End If
        If txtHargaPokok.EditValue <= 0 Then
            If XtraMessageBox.Show("Harga pokok masih kurang dari 0." & vbCrLf & "Yakin ingin melanjutkan penyimpanan?", NamaAplikasi, MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) = Windows.Forms.DialogResult.No Then
                txtHargaPokok.Focus()
                Return False
                Exit Function
            End If
        End If
        If txtGudang.Text = "" Then
            XtraMessageBox.Show("Gudang masih kosong.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
            txtGudang.Focus()
            Return False
            Exit Function
        End If
        If txtBarcode.Text = "" Then
            XtraMessageBox.Show("Kode Barang / Barcode masih kosong.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
            txtBarcode.Focus()
            Return False
            Exit Function
        End If
        'Qty Melebihi Stock Gudang
        If SisaStockGudang() < txtQtyPcs.EditValue Then
            If XtraMessageBox.Show("Nama stock " & txtNamaStock.Text & " melebihi stock gudang " & NullTostr(txtGudang.Text) & vbCrLf & "Yakin ingin melanjutkan penyimpanan?", NamaAplikasi, MessageBoxButtons.YesNo, MessageBoxIcon.Warning) = Windows.Forms.DialogResult.No Then
                txtQty.Focus()
                Return False
                Exit Function
            End If
        End If
        Return True
    End Function
    Private Function SisaStockGudang() As Double
        Dim SQL As String = ""
        Dim ds As New DataSet
        Try
            SQL = "SELECT IsNull(SUM(MKartuStok.QtyMasuk*MKartuStok.Konversi)-SUM(MKartuStok.QtyKeluar*MKartuStok.Konversi),0) AS QtyAkhir " & _
                  " FROM MKartuStok" & _
                  " LEFT JOIN MGudang ON MGudang.NoID=MKartuStok.IDGudang" & _
                  " WHERE IDBarang=" & NullTolong(txtBarang.EditValue) & " AND MGudang.NoID=" & NullTolong(txtGudang.EditValue)
            ds = ExecuteDataset("MStock", SQL)
            If ds.Tables("MStock").Rows.Count >= 1 Then
                Return NullToDbl(ds.Tables(0).Rows(0).Item("QtyAkhir"))
            Else
                Return 0
            End If
        Catch ex As Exception
            Return 0
        Finally
            ds.Dispose()
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
                gvBarang.SaveLayoutToXml(FolderLayouts & Me.Name & gvBarang.Name & ".xml")
                gvBarcode.SaveLayoutToXml(FolderLayouts & Me.Name & gvBarcode.Name & ".xml")
                GridView1.SaveLayoutToXml(folderLayouts & Me.Name & GridView1.Name & ".xml")
                gvSatuan.SaveLayoutToXml(folderLayouts & Me.Name & gvSatuan.Name & ".xml")
                gvGudang.SaveLayoutToXml(folderLayouts & Me.Name & gvGudang.Name & ".xml")
            End If
        Catch ex As Exception
            XtraMessageBox.Show("Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            xOtorisasi.Dispose()
        End Try
    End Sub

    Private Sub frmEntriPOD_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing

    End Sub
    Sub RefreshStock()
        Dim dlg As DevExpress.Utils.WaitDialogForm = Nothing
        Dim odsT2 As New DataSet
        Dim repckedit As New Repository.RepositoryItemCheckEdit
        Try
            Dim curentcursor As Cursor = Windows.Forms.Cursor.Current
            Windows.Forms.Cursor.Current = Cursors.WaitCursor
            dlg = New DevExpress.Utils.WaitDialogForm(String.Format("Sedang merefresh data.{0}MOHON TUNGGU ...", vbCrLf), Application.ProductName)
            dlg.TopMost = False
            dlg.Show()
            Windows.Forms.Cursor.Current = Cursors.WaitCursor

            SQL = "SELECT MWilayah.Nama AS Wilayah, MGudang.Nama AS Gudang, MBarang.Nama AS NamaStock, MBarang.Kode AS KodeStock, SUM((MKartuStok.QtyMasuk*MKartuStok.Konversi)-(MKartuStok.QtyKeluar*MKartuStok.Konversi)) AS QtySisa " & _
                  " FROM MKartuStok LEFT JOIN MBarang ON MBarang.NoID=MKartuStok.IDBarang " & _
                  " LEFT JOIN (MGudang LEFT JOIN MWilayah ON MWilayah.NoID=MGUdang.IDWilayah) ON MGudang.NoID=MKartuStok.IDGudang " & _
                  " WHERE MWilayah.NoID=" & IDWilayah & " AND MKartuStok.IDBarang=" & NullTolong(txtBarang.EditValue) & vbCrLf & _
                  " GROUP BY MWilayah.Nama, MGudang.Nama, MBarang.Nama, MBarang.Kode "
            odsT2 = ExecuteDataset("vBrg", SQL)
            GridControl1.DataSource = odsT2.Tables("vBrg")
            If System.IO.File.Exists(folderLayouts & Me.Name & GridView1.Name & ".xml") Then
                GridView1.RestoreLayoutFromXml(folderLayouts & Me.Name & GridView1.Name & ".xml")
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
                            .Columns(i).DisplayFormat.FormatString = "dd-MM-yyyy HH:mm"
                        Case "boolean"
                            .Columns(i).ColumnEdit = repckedit
                    End Select
                    If .Columns(i).FieldName.Length >= 4 AndAlso .Columns(i).FieldName.Substring(0, 4).ToLower = "Kode".ToLower Then
                        .Columns(i).Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left
                    ElseIf .Columns(i).FieldName.ToLower = "StokAkhir".ToLower Then
                        .Columns(i).Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left
                    ElseIf .Columns(i).FieldName.ToLower = "Nama".ToLower Then
                        .Columns(i).Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left
                    End If
                Next
            End With
            Application.DoEvents()
        Catch ex As Exception
            XtraMessageBox.Show(ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            Windows.Forms.Cursor.Current = Cursors.Default
            odsT2.Dispose()
            dlg.Close()
            dlg.Dispose()
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
            If Not IsNew Then
                LoadData()
            End If
            refreshStock()
            Me.Width = Ini.BacaIniPath(folderLayouts & Me.Name & ".ini", "Form", "Width", Me.Width)
            Me.Height = Ini.BacaIniPath(folderLayouts & Me.Name & ".ini", "Form", "Height", Me.Height)

            Me.Left = (Screen.PrimaryScreen.WorkingArea.Width - Me.Width) \ 2
            Me.Top = (Screen.PrimaryScreen.WorkingArea.Height - Me.Height) \ 2

            If System.IO.File.Exists(folderLayouts & Me.Name & LayoutControl1.Name & ".xml") Then
                LayoutControl1.RestoreLayoutFromXml(folderLayouts & Me.Name & LayoutControl1.Name & ".xml")
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

    Private Sub HitungJumlah()
        Try
            txtQtyPcs.EditValue = txtQty.EditValue * txtKonversi.EditValue
            txtJumlah.EditValue = txtQtyPcs.EditValue * txtHargaPokok.EditValue
        Catch ex As Exception

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
    Private Sub txtQty_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtQty.LostFocus
        HitungJumlah()
    End Sub

    Private Sub txtQtypcs_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtQtyPcs.LostFocus
        HitungJumlah()
    End Sub

    Private Sub txtBarang_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtBarang.EditValueChanged
        Dim Ds As New DataSet
        Dim strHPP As String = Ini.BacaIni(Me.Name, "TampilkanHPP", " MBarang.HargaBeliPcs ")
        Try
            SQL = "SELECT MBarang.Nama AS NamaBarang, " & strHPP & " AS HPP FROM MBarang WHERE MBarang.NoID=" & NullToLong(txtBarang.EditValue)
            Ds = ExecuteDataset("Tabel", SQL)
            If Ds.Tables("Tabel").Rows.Count >= 1 Then
                txtNamaStock.Text = NullTostr(Ds.Tables(0).Rows(0).Item("NamaBarang"))
                txtHargaPokok.EditValue = NullToDbl(Ds.Tables(0).Rows(0).Item("HPP"))
                RefreshLookUpSatuan()
                txtSatuan.EditValue = DefIDSatuan
                Ini.TulisIni(Me.Name, "TampilkanHPP", strHPP.ToUpper)
            End If
            RefreshStock()
        Catch ex As Exception
            XtraMessageBox.Show("Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            Ds.Dispose()
        End Try
    End Sub

    Private Sub txtSatuan_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtSatuan.EditValueChanged
        RubahSatuan()
    End Sub
    Private Sub RubahSatuan()
        Dim Ds As New DataSet
        Try
            SQL = "SELECT MBarangD.Konversi, MBarang.CtnPcs FROM MBarangD LEFT JOIN MBarang ON MBarang.NoID=MBarangD.IDBarang LEFT JOIN MSatuan ON MSatuan.NoID=MBarangD.IDSatuan WHERE MBarangD.IDBarang=" & NullTolong(txtBarang.EditValue) & " AND MSatuan.NoID=" & NullTolong(txtSatuan.EditValue)
            Ds = ExecuteDataset("Tabel", SQL)
            If Ds.Tables("Tabel").Rows.Count >= 1 Then
                txtKonversi.EditValue = NullToDbl(Ds.Tables(0).Rows(0).Item("Konversi"))
                IsiKarton = NullToDbl(Ds.Tables(0).Rows(0).Item("CtnPcs"))
                If IsiKarton = 0 Then
                    IsiKarton = 1
                End If
                txtQtyPcs.EditValue = txtQty.EditValue * txtKonversi.EditValue
                'txtCtn.EditValue = txtQty.EditValue / IsiKarton * txtKonversi.EditValue
            End If
            HitungJumlah()
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

    Private Sub gvBarang_DataSourceChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvBarang.DataSourceChanged
        If System.IO.File.Exists(folderLayouts & Me.Name & gvBarang.Name & ".xml") Then
            gvBarang.RestoreLayoutFromXml(folderLayouts & Me.Name & gvBarang.Name & ".xml")
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

    Private Sub gvSatuan_DataSourceChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvSatuan.DataSourceChanged
        If System.IO.File.Exists(folderLayouts & Me.Name & gvSatuan.Name & ".xml") Then
            gvSatuan.RestoreLayoutFromXml(folderLayouts & Me.Name & gvSatuan.Name & ".xml")
        End If
        With gvSatuan
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

    Private Sub txtKonversi_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtKonversi.KeyDown
        If e.KeyCode = Keys.F7 Then
            Dim x As New frmOtorisasiAdmin
            Try
                If txtKonversi.Properties.ReadOnly Then
                    If x.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
                        txtKonversi.Properties.ReadOnly = False
                    End If
                End If
            Catch ex As Exception
                XtraMessageBox.Show("Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End If
    End Sub

    Private Sub txtKonversi_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtKonversi.LostFocus
        HitungJumlah()
    End Sub

    Private Sub txtCtn_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        HitungJumlah()
    End Sub

    Private Sub txtSatuan_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtSatuan.LostFocus
        RubahSatuan()
    End Sub

    Private Sub txtKonversi_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtKonversi.EditValueChanged

    End Sub

    Private Sub txtQty_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtQty.EditValueChanged

    End Sub

    Private Sub txtNamaStock_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtNamaStock.EditValueChanged

    End Sub

    Private Sub txtBarang_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtBarang.LostFocus
        'RefreshStock()
    End Sub

    Private Sub gvGudang_DataSourceChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvGudang.DataSourceChanged
        If System.IO.File.Exists(folderLayouts & Me.Name & gvGudang.Name & ".xml") Then
            gvGudang.RestoreLayoutFromXml(folderLayouts & Me.Name & gvGudang.Name & ".xml")
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

    Private Sub txtHargaPokok_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtHargaPokok.EditValueChanged

    End Sub

    Private Sub txtHargaPokok_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtHargaPokok.KeyDown
        If e.KeyCode = Keys.F7 Then
            Dim x As New frmOtorisasiAdmin
            Try
                If txtHargaPokok.Properties.ReadOnly Then
                    If x.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
                        txtHargaPokok.Properties.ReadOnly = False
                    End If
                End If
            Catch ex As Exception
                XtraMessageBox.Show("Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End If
    End Sub

    Private Sub txtHargaPokok_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtHargaPokok.LostFocus
        HitungJumlah()
    End Sub

    Private Sub txtJumlah_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtJumlah.EditValueChanged

    End Sub

    Private Sub txtJumlah_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtJumlah.LostFocus
        HitungJumlah()
    End Sub

    Private Sub txtBarcode_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtBarcode.EditValueChanged
        Dim Ds As New DataSet
        Try
            SQL = "SELECT MBarangD.NoID, MBarangD.IDBarang, MBarangD.IDSatuan, MBarang.Kode, MBarang.IDSatuan " & _
                  " FROM MBarangD inner join MBarang On MBarangD.IDbarang=MBarang.NoID WHERE MBarangD.NoID=" & NullToLong(txtBarcode.EditValue)
            Ds = ExecuteDataset("Tabel", SQL)
            If Ds.Tables("Tabel").Rows.Count >= 1 Then
                txtBarang.EditValue = NullToLong(Ds.Tables(0).Rows(0).Item("IDBarang"))
                'txtNamaStock.Text = NullToStr(Ds.Tables(0).Rows(0).Item("NamaBarang"))
                RefreshLookUpSatuan()
                txtSatuan.EditValue = NullToLong(Ds.Tables(0).Rows(0).Item("IDSatuan")) 'DefIDSatuan
            End If
        Catch ex As Exception
            XtraMessageBox.Show("Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            Ds.Dispose()
        End Try
    End Sub
End Class