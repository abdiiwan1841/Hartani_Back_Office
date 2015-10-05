Imports System.Data
Imports System.Data.SqlClient
Imports DevExpress.XtraEditors
Imports DevExpress.Utils

Public Class frmEntriPackingMutasiWilayahD
    Public IsNew As Boolean = True
    Public IsPosted As Boolean = False
    Public NoID As Long = -1
    Public IDHeader As Long = -1
    'Public IDWilayahDari As Long = -1
    Public IDSPK As Long = -1

    Dim oDS As New DataSet
    Dim SQL As String = ""
    Dim DefImageList As ImageList
    Dim IsiKarton As Double = 0.0
    Dim SisaSPK As Double = 0.0

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
            SQL = "SELECT MBarang.NoID, MBarang.Kode, MBarang.Nama, MSatuan.Nama AS Satuan FROM MBarang LEFT JOIN MSatuan ON MSatuan.NoID=MBarang.IDSatuan WHERE MBarang.IsActive=1 AND NOT (MBarang.KODE='' OR MBarang.KODE IS NULL)"
            ds = ExecuteDataset("MBarang", SQL)
            txtBarang.Properties.DataSource = ds.Tables("MBarang")
            txtBarang.Properties.ValueMember = "NoID"
            txtBarang.Properties.DisplayMember = "Kode"

            SQL = "SELECT MSPKMutasiWilayahD.NoID, MSPKMutasiWilayah.Kode, MBarang.Kode AS KodeStock, MBarang.Nama AS NamaStock, MSatuan.Nama AS Satuan, MSPKMutasiWilayahD.Qty,  MSPKMutasiWilayahD.QtyPcs, MSPKMutasiWilayahD.QtyPcs-" & _
                  " IsNull((SELECT SUM(MPackingMutasiwilayahD.QtyPcs) FROM MPackingMutasiwilayahD WHERE MPackingMutasiwilayahD.NoID<>" & NoID & " AND MPackingMutasiwilayahD.IDSPKMutasiWilayahD=MSPKMutasiWilayahD.NoID),0) AS Sisa  " & vbCrLf & _
                  " FROM MSPKMutasiWilayahD " & vbCrLf & _
                  " LEFT JOIN MSPKMutasiWilayah ON MSPKMutasiWilayah.NoID=MSPKMutasiWilayahD.IDHeader " & vbCrLf & _
                  " LEFT JOIN MBarang ON MBarang.NoID=MSPKMutasiWilayahD.IDBarang " & vbCrLf & _
                  " LEFT JOIN MSatuan ON MSatuan.NoID=MSPKMutasiWilayahD.IDSatuan " & vbCrLf & _
                  " WHERE MSPKMutasiWilayah.NoID=" & IDSPK & " AND MSPKMutasiWilayah.IsPosted=1 AND (MSPKMutasiWilayah.IsSelesai=0 OR MSPKMutasiWilayah.IsSelesai IS NULL) AND MBarang.IsActive=1 AND MSPKMutasiWilayahD.QtyPcs-IsNull((SELECT SUM(MPackingMutasiwilayahD.QtyPcs) FROM MPackingMutasiwilayahD WHERE MPackingMutasiwilayahD.NoID<>" & NoID & " AND MPackingMutasiwilayahD.IDSPKMutasiWilayahD=MSPKMutasiWilayahD.NoID),0)>0"
            ds = ExecuteDataset("MBarang", SQL)
            txtSPK.Properties.DataSource = ds.Tables("MBarang")
            txtSPK.Properties.ValueMember = "NoID"
            txtSPK.Properties.DisplayMember = "Kode"
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
            SQL = "SELECT MPackingMutasiWilayahD.*, MBarang.CtnPcs, MBarang.Kode AS KodeBarang, MBarang.Nama AS NamaBarang, MSatuan.Nama AS NamaSatuan "
            SQL &= " FROM (MPackingMutasiWilayahD LEFT JOIN MBarang ON MBarang.NoID=MPackingMutasiWilayahD.IDBarang) "
            SQL &= " LEFT JOIN MSatuan ON MSatuan.NoID=MPackingMutasiWilayahD.IDSatuan "
            SQL &= " WHERE MPackingMutasiWilayahD.NoID= " & NoID
            oDS = New DataSet()
            oDS = ExecuteDataset("MPOD", SQL)
            If oDS.Tables(0).Rows.Count >= 1 Then
                With oDS.Tables("MPOD").Rows(0)
                    NoID = NullTolong(.Item("NoID"))
                    IDHeader = NullTolong(.Item("IDHeader"))
                    txtSPK.EditValue = NullTolong(.Item("IDSPKMutasiWilayahD"))
                    txtBarang.EditValue = NullTolong(.Item("IDBarang"))
                    txtSatuan.EditValue = NullTolong(.Item("IDSatuan"))
                    txtQty.EditValue = NullToDbl(.Item("Qty"))
                    txtCtn.EditValue = NullToDbl(.Item("CTN"))
                    txtQtyPcs.EditValue = NullToDbl(.Item("QtyPcs"))
                    txtNoPacking.Text = NullTostr(.Item("NoPacking"))
                    txtCatatan.Text = NullTostr(.Item("Keterangan"))
                    txtKonversi.EditValue = NullToDbl(.Item("Konversi"))
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
                SQL = "INSERT INTO [MPackingMutasiWilayahD] ([NoID],[IDSPKMutasiWilayahD],[IDRequestMutasiWilayahD],[IDHeader],[IDBarang],[IDSatuan],[Konversi],[Qty],[QtyPCS],[Ctn],[NoPacking],[Keterangan]) VALUES ("
                SQL &= NullTolong(GetNewID("MPackingMutasiWilayahD", "NoID")) & ","
                SQL &= NullTolong(txtSPK.EditValue) & ","
                SQL &= NullTolong(EksekusiSQlSkalarNew("SELECT IDRequestMutasiWilayahD FROM MSPKMutasiWilayahD WHERE NoID=" & NullTolong(txtSPK.EditValue))) & ","
                SQL &= IDHeader & ","
                SQL &= NullTolong(txtBarang.EditValue) & ","
                SQL &= NullTolong(txtSatuan.EditValue) & ","
                SQL &= FixKoma(txtKonversi.EditValue) & ","
                SQL &= FixKoma(txtQty.EditValue) & ","
                SQL &= FixKoma(txtQtyPcs.EditValue) & ","
                SQL &= FixKoma(txtCtn.EditValue) & ","
                SQL &= "'" & FixApostropi(txtNoPacking.Text) & "',"
                SQL &= "'" & FixApostropi(txtCatatan.Text) & "'"
                SQL &= ")"
            Else
                SQL = "UPDATE MPackingMutasiWilayahD SET "
                SQL &= " IDSPKMutasiWilayahD=" & NullTolong(txtSPK.EditValue) & ","
                SQL &= " IDRequestMutasiWilayahD=" & NullTolong(EksekusiSQlSkalarNew("SELECT IDRequestMutasiWilayahD FROM MSPKMutasiWilayahD WHERE NoID=" & NullTolong(txtSPK.EditValue))) & ","
                SQL &= " IDBarang=" & NullTolong(txtBarang.EditValue) & ","
                SQL &= " IDSatuan=" & NullTolong(txtSatuan.EditValue) & ","
                SQL &= " Konversi=" & FixKoma(txtKonversi.EditValue) & ","
                SQL &= " Qty=" & FixKoma(txtQty.EditValue) & ","
                SQL &= " QtyPcs=" & FixKoma(txtQtyPcs.EditValue) & ","
                SQL &= " NoPacking='" & FixApostropi(txtNoPacking.Text) & "',"
                SQL &= " Keterangan='" & FixApostropi(txtCatatan.Text) & "',"
                SQL &= " Ctn=" & FixKoma(txtCtn.EditValue)
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
        If txtQty.EditValue <= 0 Then
            XtraMessageBox.Show("Qty masih kurang dari atau sama dengan 0.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
            txtQty.Focus()
            Return False
            Exit Function
        End If
        If txtCtn.EditValue < 0 Then
            XtraMessageBox.Show("Ctn tidak boleh kurang dari 0.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
            txtCtn.Focus()
            Return False
            Exit Function
        End If
        If txtKonversi.EditValue < 0 Then
            XtraMessageBox.Show("Konversi tidak boleh kurang dari 0.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
            txtKonversi.Focus()
            Return False
            Exit Function
        End If
        If txtQtyPcs.EditValue < 0 Then
            XtraMessageBox.Show("Disc persen 1 tidak boleh kurang dari 0.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
            txtQtyPcs.Focus()
            Return False
            Exit Function
        End If
        If txtNoPacking.Text = "" Then
            XtraMessageBox.Show("No Packing masih kosong.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
            txtNoPacking.Focus()
            Return False
            Exit Function
        End If
        If txtSPK.Text <> "" Then
            If SisaStockSPK() < txtQtyPcs.EditValue Then
                XtraMessageBox.Show("Nama stock " & txtNamaStock.Text & " melebihi stock item SPK " & txtSPK.Text, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
                txtQty.Focus()
                Return False
                Exit Function
            End If
        Else
            XtraMessageBox.Show("Item SPK masih kosong.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
            txtSPK.Focus()
            Return False
            Exit Function
        End If
        Return True
    End Function
    'Private Function SisaStockWilayah() As Double
    '    Dim SQL As String = ""
    '    Dim ds As New DataSet
    '    Try
    '        SQL = "SELECT IsNull(SUM(MKartuStok.QtyMasuk*MKartuStok.Konversi)-SUM(MKartuStok.QtyKeluar*MKartuStok.Konversi),0) AS QtyAkhir " & _
    '              " FROM MKartuStok" & _
    '              " LEFT JOIN MGudang ON MGudang.NoID=MKartuStok.IDGudang" & _
    '              " WHERE IDBarang=" & NullTolong(txtBarang.EditValue) & " AND MGudang.IDWilayah=" & IDWilayahDari
    '        ds = ExecuteDataset("MStock", SQL)
    '        If ds.Tables("MStock").Rows.Count >= 1 Then
    '            Return NullToDbl(ds.Tables(0).Rows(0).Item("QtyAkhir"))
    '        Else
    '            Return 0
    '        End If
    '    Catch ex As Exception
    '        Return 0
    '    Finally
    '        ds.Dispose()
    '    End Try
    'End Function
    Private Function SisaStockSPK() As Double
        Dim SQL As String = ""
        Dim ds As New DataSet
        Try
            SQL = "SELECT MSPKMutasiwilayahD.QtyPcs-IsNull((SELECT SUM(MPackingMutasiWilayahD.QtyPcs) FROM MPackingMutasiWilayahD WHERE MPackingMutasiWilayahD.IDSPKMutasiWilayahD=MSPKMutasiwilayahD.NoID AND MPackingMutasiWilayahD.NoID<>" & NoID & "),0) AS QtyAkhir " & _
                  " FROM MSPKMutasiwilayahD " & _
                  " WHERE MSPKMutasiwilayahD.NoID=" & NullTolong(txtSPK.EditValue)
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
                gvBarang.SaveLayoutToXml(folderLayouts & Me.Name & gvBarang.Name & ".xml")
                gvSPK.SaveLayoutToXml(folderLayouts & Me.Name & gvSPK.Name & ".xml")
                gvSatuan.SaveLayoutToXml(folderLayouts & Me.Name & gvSatuan.Name & ".xml")
            End If
        Catch ex As Exception
            XtraMessageBox.Show("Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            xOtorisasi.Dispose()
        End Try
    End Sub
    'Sub RefreshStock()
    '    Dim dlg As DevExpress.Utils.WaitDialogForm = Nothing
    '    Dim odsT2 As New DataSet
    '    Dim strsql As String
    '    Dim repckedit As New Repository.RepositoryItemCheckEdit
    '    Try
    '        Dim curentcursor As Cursor = Windows.Forms.Cursor.Current
    '        Windows.Forms.Cursor.Current = Cursors.WaitCursor
    '        dlg = New DevExpress.Utils.WaitDialogForm(String.Format("Sedang merefresh data.{0}MOHON TUNGGU ...", vbCrLf), Application.ProductName)
    '        dlg.TopMost = False
    '        dlg.Show()
    '        Windows.Forms.Cursor.Current = Cursors.WaitCursor

    '        strsql = "SELECT MBarang.NoID, MBarang.IsNonStock AS TidakDikontrol, MBarang.Kode, MBarang.Nama, MBarang.Barcode, MSatuan.Nama AS Satuan, mjenisbarang.Nama AS Jenis, " & vbCrLf & _
    '                 " MKategori.Nama AS Kategori, MBarang.HPP, MBarang.HargaJualA, MBarang.HargaJualB, MBarang.HargaJualC, " & vbCrLf & _
    '                 " MBarang.HargaJualD, MBarang.HargaJualE, MBarang.KodeDuz, MBarang.Ctn_Duz, MBarang.CtnPcs, MBarang.IsActive, MBarang.IsNewItem AS NewItem, " & vbCrLf & _
    '                 " MBarang.HargaJualF, " & vbCrLf & _
    '                 " MSupplier1.Nama AS Supplier1, " & vbCrLf & _
    '                 " MSupplier2.Nama AS Supplier2, " & vbCrLf & _
    '                 " MSupplier3.Nama AS Supplier3, " & vbCrLf & _
    '                 " MSupplier4.Nama AS Supplier4, " & vbCrLf & _
    '                 " MSupplier5.Nama AS Supplier5, " & vbCrLf & _
    '                 " (SELECT SUM((MKartuStok.QtyMasuk*MKartuStok.Konversi)-(MKartuStok.QtyKeluar*MKartuStok.Konversi)) FROM MKartuStok LEFT JOIN MGudang ON MGudang.NoID=MKartuStok.IDGudang WHERE MGudang.IDwilayah=" & IDWilayahDari & " AND MKartuStok.Tanggal<'" & DateAdd(DateInterval.Day, 1, TanggalSystem).ToString("yyyy-MM-dd") & "' AND MKartuStok.IDBarang=MBarang.NoID) AS StokAkhir, " & vbCrLf & _
    '                 " (SELECT SUM((MKartuStok.QtyMasuk*MKartuStok.Konversi)-(MKartuStok.QtyKeluar*MKartuStok.Konversi)) FROM MKartuStok LEFT JOIN MGudang ON MGudang.NoID=MKartuStok.IDGudang WHERE MGudang.IDwilayah=" & IDWilayahDari & " AND MGudang.IsBS=1 AND MKartuStok.Tanggal<'" & DateAdd(DateInterval.Day, 1, TanggalSystem).ToString("yyyy-MM-dd") & "' AND MKartuStok.IDBarang=MBarang.NoID) AS StokTdkSiapJual, " & vbCrLf & _
    '                 " (SELECT SUM((MKartuStok.QtyMasuk*MKartuStok.Konversi)-(MKartuStok.QtyKeluar*MKartuStok.Konversi)) FROM MKartuStok LEFT JOIN MGudang ON MGudang.NoID=MKartuStok.IDGudang WHERE MGudang.IDwilayah=" & IDWilayahDari & " AND (MGudang.IsBS=0 OR MGudang.IsBS Is Null) AND MKartuStok.Tanggal<'" & DateAdd(DateInterval.Day, 1, TanggalSystem).ToString("yyyy-MM-dd") & "' AND MKartuStok.IDBarang=MBarang.NoID) AS StokSiapJual " & vbCrLf & _
    '                 " FROM MBarang LEFT OUTER JOIN" & vbCrLf & _
    '                 " MSatuan ON MBarang.IDSatuan = MSatuan.NoID LEFT OUTER JOIN" & vbCrLf & _
    '                 " MAlamat MSupplier1 ON MBarang.IDSupplier1 = MSupplier1.NoID LEFT OUTER JOIN" & vbCrLf & _
    '                 " MAlamat MSupplier2 ON MBarang.IDSupplier2 = MSupplier2.NoID LEFT OUTER JOIN" & vbCrLf & _
    '                 " MAlamat MSupplier3 ON MBarang.IDSupplier3 = MSupplier3.NoID LEFT OUTER JOIN" & vbCrLf & _
    '                 " MAlamat MSupplier4 ON MBarang.IDSupplier4 = MSupplier4.NoID LEFT OUTER JOIN" & vbCrLf & _
    '                 " MAlamat MSupplier5 ON MBarang.IDSupplier5 = MSupplier5.NoID LEFT OUTER JOIN" & vbCrLf & _
    '                 " MKategori ON MBarang.IDKategori = MKategori.NoID LEFT OUTER JOIN" & vbCrLf & _
    '                 " mjenisbarang ON MBarang.IDJenis = mjenisbarang.NoID" & vbCrLf & _
    '                 " WHERE MBarang.IsActive=1"
    '        odsT2 = ExecuteDataset("vBrg", strsql)
    '        GridControl1.DataSource = odsT2.Tables("vBrg")
    '        If System.IO.File.Exists(folderLayouts &  Me.Name & gvSPK.Name & ".xml") Then
    '            gvSPK.RestoreLayoutFromXml(folderLayouts &  Me.Name & gvSPK.Name & ".xml")
    '        End If
    '        With gvSPK
    '            For i As Integer = 0 To .Columns.Count - 1
    '                Select Case .Columns(i).ColumnType.Name.ToLower
    '                    Case "int32", "int64", "int"
    '                        .Columns(i).DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
    '                        .Columns(i).DisplayFormat.FormatString = "n0"
    '                    Case "decimal", "single", "money", "double"
    '                        .Columns(i).DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
    '                        .Columns(i).DisplayFormat.FormatString = "n2"
    '                    Case "string"
    '                        .Columns(i).DisplayFormat.FormatType = DevExpress.Utils.FormatType.None
    '                        .Columns(i).DisplayFormat.FormatString = ""
    '                    Case "date"
    '                        .Columns(i).DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime
    '                        .Columns(i).DisplayFormat.FormatString = "dd-MM-yyyy"
    '                    Case "datetime"
    '                        .Columns(i).DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime
    '                        .Columns(i).DisplayFormat.FormatString = "dd-MM-yyyy HH:mm"
    '                    Case "boolean"
    '                        .Columns(i).ColumnEdit = repckedit
    '                End Select
    '                If .Columns(i).FieldName.Length >= 4 AndAlso .Columns(i).FieldName.Substring(0, 4).ToLower = "Kode".ToLower Then
    '                    .Columns(i).Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left
    '                ElseIf .Columns(i).FieldName.ToLower = "StokAkhir".ToLower Then
    '                    .Columns(i).Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left
    '                ElseIf .Columns(i).FieldName.ToLower = "Nama".ToLower Then
    '                    .Columns(i).Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left
    '                End If
    '            Next
    '        End With
    '        Application.DoEvents()
    '    Catch ex As Exception
    '        XtraMessageBox.Show(ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
    '    Finally
    '        Windows.Forms.Cursor.Current = Cursors.Default
    '        odsT2.Dispose()
    '        dlg.Close()
    '        dlg.Dispose()
    '    End Try
    'End Sub
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
        RubahSatuan()
        HitungJumlah()
    End Sub

    Private Sub txtQtypcs_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtQtyPcs.LostFocus
        HitungJumlah()
    End Sub

    Private Sub txtBarang_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtBarang.EditValueChanged
        Dim Ds As New DataSet
        Try
            SQL = "SELECT MBarang.Nama AS NamaBarang FROM MBarang WHERE MBarang.NoID=" & NullTolong(txtBarang.EditValue)
            Ds = ExecuteDataset("Tabel", SQL)
            If Ds.Tables("Tabel").Rows.Count >= 1 Then
                txtNamaStock.Text = NullTostr(Ds.Tables(0).Rows(0).Item("NamaBarang"))
                RefreshLookUpSatuan()
                txtSatuan.EditValue = DefIDSatuan
            End If
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
                txtQtyPcs.EditValue = txtQty.EditValue * txtKonversi.EditValue
                If IsiKarton = 0 Then
                    txtCtn.EditValue = 0
                Else
                    txtCtn.EditValue = txtQty.EditValue / IsiKarton * txtKonversi.EditValue
                End If
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

    Private Sub txtCtn_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtCtn.LostFocus
        HitungJumlah()
    End Sub

    Private Sub txtSatuan_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtSatuan.LostFocus
        RubahSatuan()
    End Sub

    Private Sub txtKonversi_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtKonversi.EditValueChanged

    End Sub

    Private Sub gvSPK_DataSourceChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvSPK.DataSourceChanged
        If System.IO.File.Exists(folderLayouts & Me.Name & gvSPK.Name & ".xml") Then
            gvSPK.RestoreLayoutFromXml(folderLayouts & Me.Name & gvSPK.Name & ".xml")
        End If
        With gvSPK
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

    Private Sub txtSPK_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtSPK.EditValueChanged
        Dim Ds As New DataSet
        Dim SisaQty As Double = 0.0
        Try
            SQL = "SELECT MSPKMutasiWilayahD.*, MBarang.Kode AS KodeBarang, MBarang.Nama AS NamaBarang, MSatuan.Nama AS NamaSatuan "
            SQL &= " FROM (MSPKMutasiWilayahD LEFT JOIN MBarang ON MBarang.NoID=MSPKMutasiWilayahD.IDBarang)"
            SQL &= " LEFT JOIN MSatuan ON MSatuan.NoID=MSPKMutasiWilayahD.IDSatuan "
            SQL &= " WHERE MSPKMutasiWilayahD.NoID= " & NullTolong(txtSPK.EditValue)
            Ds = New DataSet()
            Ds = ExecuteDataset("Master", SQL)
            If Ds.Tables(0).Rows.Count >= 1 Then
                With Ds.Tables("Master").Rows(0)
                    txtBarang.EditValue = NullTolong(.Item("IDBarang"))
                    txtSatuan.EditValue = NullTolong(.Item("IDSatuan"))
                    txtKonversi.EditValue = NullToDbl(.Item("Konversi"))
                    SisaQty = NullToDbl(SisaStockSPK)
                    txtQty.EditValue = SisaQty / txtKonversi.EditValue
                    txtQtyPcs.EditValue = SisaQty
                    RubahSatuan()
                    txtCatatan.Text = NullTostr(.Item("Keterangan"))
                    HitungJumlah()
                End With
            Else
                IsiDefault()
            End If
        Catch ex As Exception
            XtraMessageBox.Show("Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            'RefreshLookUpSTB()
            Ds.Dispose()
        End Try
    End Sub

    Private Sub txtQty_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtQty.EditValueChanged

    End Sub
End Class