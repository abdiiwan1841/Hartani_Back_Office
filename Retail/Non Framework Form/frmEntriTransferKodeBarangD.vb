Imports System.Data
Imports System.Data.SqlClient
Imports DevExpress.XtraEditors
Imports DevExpress.Utils

Public Class frmEntriTransferKodeBarangD
    Public IsNew As Boolean = True
    Public IsPosted As Boolean = False
    Public NoID As Long = -1
    Public IDHeader As Long = -1

    Dim oDS As New DataSet
    Dim SQL As String = ""
    Dim DefImageList As ImageList
    Dim SisaTransferOut As Double = 0.0
    Public IsFastEntri As Boolean = False

    Private Sub IsiDefault()
        DefImageList = frmMain.ImageList1
        cmdSave.ImageList = DefImageList
        cmdSave.ImageIndex = 6
        txtGudangLama.EditValue = DefIDGudang
        txtGudangBaru.EditValue = DefIDGudang

        cmdTutup.ImageList = DefImageList
        cmdTutup.ImageIndex = 3
        RefreshLookUp()
    End Sub
    Private Sub RefreshLookUp()
        Dim ds As New DataSet
        Try
            SQL = "SELECT MBarang.NoID, MBarang.Kode, MBarang.Nama, MSatuan.Nama AS Satuan FROM MBarang LEFT JOIN MSatuan ON MSatuan.NoID=MBarang.IDSatuan WHERE MBarang.IsActive=1 AND NOT (MBarang.KODE='' OR MBarang.KODE IS NULL)"
            ds = ExecuteDataset("MBarang", SQL)
            txtBarangLama.Properties.DataSource = ds.Tables("MBarang")
            txtBarangLama.Properties.ValueMember = "NoID"
            txtBarangLama.Properties.DisplayMember = "Kode"

            SQL = "SELECT MBarang.NoID, MBarang.Kode, MBarang.Nama, MSatuan.Nama AS Satuan FROM MBarang LEFT JOIN MSatuan ON MSatuan.NoID=MBarang.IDSatuan WHERE MBarang.IsActive=1 AND NOT (MBarang.KODE='' OR MBarang.KODE IS NULL)"
            ds = ExecuteDataset("MBarang", SQL)
            txtBarangBaru.Properties.DataSource = ds.Tables("MBarang")
            txtBarangBaru.Properties.ValueMember = "NoID"
            txtBarangBaru.Properties.DisplayMember = "Kode"

            SQL = "SELECT MGudang.NoID, MGudang.Kode, MGudang.Nama, MWilayah.Nama AS Wilayah FROM MGudang LEFT JOIN MWilayah ON MWilayah.NoID=MGudang.IDWilayah WHERE MGudang.IsActive=1 AND MGudang.InTransit=0 AND MGudang.IDWilayah=" & DefIDWilayah
            ds = ExecuteDataset("MWilayah", SQL)
            txtGudangLama.Properties.DataSource = ds.Tables("MWilayah")
            txtGudangLama.Properties.ValueMember = "NoID"
            txtGudangLama.Properties.DisplayMember = "Kode"

            SQL = "SELECT MGudang.NoID, MGudang.Kode, MGudang.Nama, MWilayah.Nama AS Wilayah FROM MGudang LEFT JOIN MWilayah ON MWilayah.NoID=MGudang.IDWilayah WHERE MGudang.IsActive=1 AND MGudang.InTransit=0 AND MGudang.IDWilayah=" & DefIDWilayah
            ds = ExecuteDataset("MWilayah", SQL)
            txtGudangBaru.Properties.DataSource = ds.Tables("MWilayah")
            txtGudangBaru.Properties.ValueMember = "NoID"
            txtGudangBaru.Properties.DisplayMember = "Kode"

        Catch ex As Exception
            XtraMessageBox.Show("Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            ds.Dispose()
        End Try
    End Sub
    Private Sub RefreshLookUpSatuan()
        Dim ds As New DataSet
        Try
            SQL = "SELECT MSatuan.NoID, MSatuan.Kode, MSatuan.Nama FROM MSatuan WHERE MSatuan.IsActive=1 AND MSatuan.NoID IN (SELECT MBarangD.IDSatuan FROM MBarangD WHERE MBarangD.IsJual=1 AND MBarangD.IsActive=1 AND MBarangD.IDBarang=" & NullTolong(txtBarangLama.EditValue) & ")"
            ds = ExecuteDataset("MSatuan", SQL)
            txtSatuanLama.Properties.DataSource = ds.Tables("MSatuan")
            txtSatuanLama.Properties.ValueMember = "NoID"
            txtSatuanLama.Properties.DisplayMember = "Kode"

            SQL = "SELECT MSatuan.NoID, MSatuan.Kode, MSatuan.Nama FROM MSatuan WHERE MSatuan.IsActive=1 AND MSatuan.NoID IN (SELECT MBarangD.IDSatuan FROM MBarangD WHERE MBarangD.IsJual=1 AND MBarangD.IsActive=1 AND MBarangD.IDBarang=" & NullToLong(txtBarangBaru.EditValue) & ")"
            ds = ExecuteDataset("MSatuan", SQL)
            txtSatuanBaru.Properties.DataSource = ds.Tables("MSatuan")
            txtSatuanBaru.Properties.ValueMember = "NoID"
            txtSatuanBaru.Properties.DisplayMember = "Kode"
        Catch ex As Exception
            XtraMessageBox.Show("Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            ds.Dispose()
        End Try
    End Sub
    Private Sub LoadData()
        Try
            SQL = "SELECT MTransferKodeD.* "
            SQL &= " FROM MTransferKodeD "
            SQL &= " WHERE MTransferKodeD.NoID= " & NoID
            oDS = New DataSet()
            oDS = ExecuteDataset("MPOD", SQL)
            If oDS.Tables(0).Rows.Count >= 1 Then
                With oDS.Tables("MPOD").Rows(0)
                    NoID = NullTolong(.Item("NoID"))
                    IDHeader = NullTolong(.Item("IDHeader"))

                    txtBarangLama.EditValue = NullToLong(.Item("IDBarangLama"))
                    txtSatuanLama.EditValue = NullToLong(.Item("IDSatuanLama"))
                    txtGudangLama.EditValue = NullToLong(.Item("IDGudangLama"))
                    txtQtyLama.EditValue = NullToDbl(.Item("QtyLama"))

                    txtBarangBaru.EditValue = NullToLong(.Item("IDBarangBaru"))
                    txtSatuanBaru.EditValue = NullToLong(.Item("IDSatuanBaru"))
                    txtGudangBaru.EditValue = NullToLong(.Item("IDGudangBaru"))
                    txtQtyBaru.EditValue = NullToDbl(.Item("QtyBaru"))

                    txtHargaModal.EditValue = NullToDbl(.Item("HargaModal"))
                    txtCatatan.Text = NullTostr(.Item("Keterangan"))
                    txtKonversiLama.EditValue = NullToDbl(.Item("KonversiLama"))
                    txtKonversiLama.EditValue = NullToDbl(.Item("KonversiBaru"))
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
            If Simpan() Then
                DialogResult = Windows.Forms.DialogResult.OK
                Me.Close()
            End If
        End If
    End Sub
    Private Function Simpan() As Boolean
        Dim IsiKartonLama As Double = 0.0
        Dim IsiKartonBaru As Double = 0.0
        Dim CtnLama As Double = 0.0
        Dim CtnBaru As Double = 0.0

        Try
            IsiKartonLama = NullToDbl(EksekusiSQlSkalarNew("SELECT CtnPcs FROM MBarang WHERE NoID=" & NullToLong(txtBarangLama.EditValue)))
            IsiKartonBaru = NullToDbl(EksekusiSQlSkalarNew("SELECT CtnPcs FROM MBarang WHERE NoID=" & NullToLong(txtBarangBaru.EditValue)))
            If IsiKartonLama = 0 Then
                CtnLama = 0
            Else
                CtnLama = txtQtyLama.EditValue / IsiKartonLama * txtKonversiLama.EditValue
            End If
            If IsiKartonBaru = 0 Then
                CtnBaru = 0
            Else
                CtnBaru = txtQtyBaru.EditValue / IsiKartonBaru * txtKonversiBaru.EditValue
            End If

            If IsNew Then
                SQL = "INSERT INTO MTransferKodeD" & _
                      " (NoID, IDHeader, IDBarangLama, IDBarangBaru, IDGudangLama, IDGudangBaru, IDSatuanLama, QtyLama, KonversiLama, CtnLama, IDSatuanBaru, QtyBaru, " & _
                      " KonversiBaru, CtnBaru, Keterangan, HargaModal) " & _
                      " VALUES     (" & _
                      NullToLong(GetNewID("MTransferKodeD", "NoID")) & "," & _
                      IDHeader & "," & _
                      NullToLong(txtBarangLama.EditValue) & "," & _
                      NullToLong(txtBarangBaru.EditValue) & "," & _
                      NullToLong(txtGudangLama.EditValue) & "," & _
                      NullToLong(txtGudangBaru.EditValue) & "," & _
                      NullToLong(txtSatuanLama.EditValue) & "," & _
                      FixKoma(txtQtyLama.EditValue) & "," & _
                      FixKoma(txtKonversiLama.EditValue) & "," & _
                      FixKoma(CtnLama) & "," & _
                      NullToLong(txtSatuanBaru.EditValue) & "," & _
                      FixKoma(txtQtyBaru.EditValue) & "," & _
                      FixKoma(txtKonversiBaru.EditValue) & "," & _
                      FixKoma(CtnBaru) & "," & _
                      "'" & FixApostropi(txtCatatan.Text) & "'," & _
                      FixKoma(txtHargaModal.EditValue) & _
                      ")"
            Else
                SQL = "UPDATE MTransferKodeD SET " & _
                      " IDBarangLama=" & NullToLong(txtBarangLama.EditValue) & "," & _
                      " IDBarangBaru=" & NullToLong(txtBarangBaru.EditValue) & "," & _
                      " IDGudangLama=" & NullToLong(txtGudangLama.EditValue) & "," & _
                      " IDGudangBaru=" & NullToLong(txtGudangBaru.EditValue) & "," & _
                      " IDSatuanLama=" & NullToLong(txtSatuanLama.EditValue) & "," & _
                      " QtyLama=" & FixKoma(txtQtyLama.EditValue) & "," & _
                      " KonversiLama=" & FixKoma(txtKonversiLama.EditValue) & "," & _
                      " CtnLama=" & FixKoma(CtnLama) & "," & _
                      " IDSatuanBaru=" & NullToLong(txtSatuanBaru.EditValue) & "," & _
                      " QtyBaru=" & FixKoma(txtQtyBaru.EditValue) & "," & _
                      " KonversiBaru=" & FixKoma(txtKonversiBaru.EditValue) & "," & _
                      " CtnBaru=" & FixKoma(CtnBaru) & "," & _
                      " Keterangan='" & FixApostropi(txtCatatan.Text) & "'," & _
                      " HargaModal=" & FixKoma(txtHargaModal.EditValue) & _
                      " WHERE NoID=" & NoID
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
        If txtBarangLama.Text = "" Then
            XtraMessageBox.Show("Barang Lama masih kosong.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
            txtBarangLama.Focus()
            Return False
            Exit Function
        End If
        If txtSatuanLama.Text = "" Then
            XtraMessageBox.Show("Satuan Lama masih kosong.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
            txtSatuanLama.Focus()
            Return False
            Exit Function
        End If
        If txtQtyLama.EditValue <= 0 Then
            XtraMessageBox.Show("Qty Lama masih kurang dari atau sama dengan 0.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
            txtQtyLama.Focus()
            Return False
            Exit Function
        End If
        If txtGudangLama.Text = "" Then
            XtraMessageBox.Show("Gudang Lama masih kosong.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
            txtGudangLama.Focus()
            Return False
            Exit Function
        End If

        If txtBarangBaru.Text = "" Then
            XtraMessageBox.Show("Barang Baru masih kosong.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
            txtBarangBaru.Focus()
            Return False
            Exit Function
        End If
        If txtSatuanBaru.Text = "" Then
            XtraMessageBox.Show("Satuan Baru masih kosong.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
            txtSatuanBaru.Focus()
            Return False
            Exit Function
        End If
        If txtQtyBaru.EditValue <= 0 Then
            XtraMessageBox.Show("Qty Baru masih kurang dari atau sama dengan 0.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
            txtQtyBaru.Focus()
            Return False
            Exit Function
        End If
        If txtGudangBaru.Text = "" Then
            XtraMessageBox.Show("Gudang Baru masih kosong.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
            txtGudangBaru.Focus()
            Return False
            Exit Function
        End If

        If txtKonversiLama.EditValue < 0 Then
            XtraMessageBox.Show("Konversi Lama tidak boleh kurang dari 0.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
            txtKonversiLama.Focus()
            Return False
            Exit Function
        End If
        If txtKonversiBaru.EditValue < 0 Then
            XtraMessageBox.Show("Konversi Baru tidak boleh kurang dari 0.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
            txtKonversiLama.Focus()
            Return False
            Exit Function
        End If

        If Not (SisaStock() = txtQtyBaru.EditValue * txtKonversiBaru.EditValue) Then
            If XtraMessageBox.Show("Qty lama dan baru belum seimbang." & vbCrLf & "Ingin tetap melakukan penyimpanan?", NamaAplikasi, MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) = Windows.Forms.DialogResult.No Then
                txtKonversiLama.Focus()
                Return False
                Exit Function
            End If
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
    Private Function SisaStock() As Double
        Dim SQL As String = ""
        Dim ds As New DataSet
        Try
            SQL = "SELECT SUM((MKartuStok.QtyMasuk-MKartuStok.QtyKeluar)*MKartuStok.Konversi) AS QtyPcs " & _
                  " FROM MKartuStok WHERE MKartuStok.IDBarang=" & NullToLong(txtBarangLama.EditValue) & " AND MKartuStok.IDGudang= " & NullToLong(txtGudangLama.EditValue)
            ds = ExecuteDataset("MStock", SQL)
            If ds.Tables("MStock").Rows.Count >= 1 Then
                Return NullToDbl(ds.Tables(0).Rows(0).Item("QtyPcs"))
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
                gvBarangLama.SaveLayoutToXml(folderLayouts & Me.Name & gvBarangLama.Name & ".xml")
                gvSatuanLama.SaveLayoutToXml(FolderLayouts & Me.Name & gvSatuanLama.Name & ".xml")
                gvGudangLama.SaveLayoutToXml(folderLayouts & Me.Name & gvGudangLama.Name & ".xml")
                gvBarangBaru.SaveLayoutToXml(FolderLayouts & Me.Name & gvBarangBaru.Name & ".xml")
                gvSatuanBaru.SaveLayoutToXml(FolderLayouts & Me.Name & gvSatuanBaru.Name & ".xml")
                gvGudangBaru.SaveLayoutToXml(FolderLayouts & Me.Name & gvGudangBaru.Name & ".xml")
            End If
        Catch ex As Exception
            XtraMessageBox.Show("Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            xOtorisasi.Dispose()
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
    Private Sub txtBarang_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtBarangLama.EditValueChanged
        Dim Ds As New DataSet
        Try
            SQL = "SELECT MBarang.Nama AS NamaBarang, MBarang.HPP AS HargaModal FROM MBarang WHERE MBarang.NoID=" & NullToLong(txtBarangLama.EditValue)
            Ds = ExecuteDataset("Tabel", SQL)
            If Ds.Tables("Tabel").Rows.Count >= 1 Then
                txtNamaStockLama.Text = NullTostr(Ds.Tables(0).Rows(0).Item("NamaBarang"))
                RefreshLookUpSatuan()
                If txtSatuanLama.Text = "" Then
                    txtSatuanLama.EditValue = DefIDSatuan
                End If
                If txtHargaModal.EditValue = 0 Then
                    txtHargaModal.EditValue = NullToDbl(Ds.Tables(0).Rows(0).Item("HargaModal"))
                End If
            End If
        Catch ex As Exception
            XtraMessageBox.Show("Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            Ds.Dispose()
        End Try
    End Sub
    Private Sub txtSatuan_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtSatuanLama.EditValueChanged
        RubahSatuanLama()
    End Sub
    Private Sub RubahSatuanLama()
        Dim Ds As New DataSet
        Dim SisaQty As Double = 0.0
        Try
            SQL = "SELECT " & _
                  " MBarangD.Konversi, MBarang.HPP AS HargaModal " & _
                  " FROM MBarangD " & _
                  " INNER JOIN MBarang ON MBarang.NoID=MBarangD.IDBarang " & _
                  " LEFT JOIN MSatuan ON MSatuan.NoID=MBarangD.IDSatuan " & _
                  " WHERE MBarangD.IDBarang = " & NullToLong(txtBarangLama.EditValue) & " And MSatuan.NoID = " & NullToLong(txtSatuanLama.EditValue)
            Ds = ExecuteDataset("Tabel", SQL)
            If Ds.Tables("Tabel").Rows.Count >= 1 Then
                txtKonversiLama.EditValue = NullToDbl(Ds.Tables(0).Rows(0).Item("Konversi"))
                SisaQty = NullToDbl(EksekusiSQlSkalarNew("SELECT SUM((MKartuStok.QtyMasuk-MKartuStok.QtyKeluar)*MKartuStok.Konversi) AS QtyPcs FROM MKartuStok WHERE IDBarang=" & NullToLong(txtBarangLama.EditValue) & " AND IDGudang=" & NullToLong(txtGudangLama.EditValue)))
                If txtKonversiLama.EditValue <> 0 Then
                    txtQtyLama.EditValue = SisaQty / txtKonversiLama.EditValue
                Else
                    txtQtyLama.EditValue = 0
                End If
                If NullToDbl(txtHargaModal.EditValue) = 0 Or IsNew Or IsFastEntri Then
                    txtHargaModal.EditValue = NullToDbl(Ds.Tables("Tabel").Rows(0).Item("HargaModal")) * txtKonversiLama.EditValue
                End If
            End If
        Catch ex As Exception
            XtraMessageBox.Show("Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            Ds.Dispose()
        End Try
    End Sub
    Private Sub RubahSatuanBaru()
        Dim Ds As New DataSet
        Try
            SQL = "SELECT " & _
                  " MBarangD.Konversi, MBarang.HPP AS HargaModal " & _
                  " FROM MBarangD " & _
                  " LEFT JOIN MBarang ON MBarang.NoID=MBarangD.IDBarang " & _
                  " LEFT JOIN MSatuan ON MSatuan.NoID=MBarangD.IDSatuan " & _
                  " WHERE MBarangD.IDBarang = " & NullToLong(txtBarangBaru.EditValue) & " And MSatuan.NoID = " & NullToLong(txtSatuanBaru.EditValue)
            Ds = ExecuteDataset("Tabel", SQL)
            If Ds.Tables("Tabel").Rows.Count >= 1 Then
                txtKonversiBaru.EditValue = NullToDbl(Ds.Tables(0).Rows(0).Item("Konversi"))
                If txtKonversiBaru.EditValue <> 0 Or IsNew Then
                    txtQtyBaru.EditValue = txtQtyLama.EditValue * txtKonversiLama.EditValue / IIf(txtKonversiBaru.EditValue = 0, 1, txtKonversiBaru.EditValue)
                Else
                    txtQtyBaru.EditValue = 0
                End If
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
    Private Sub gvBarang_DataSourceChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvBarangLama.DataSourceChanged
        If System.IO.File.Exists(folderLayouts & Me.Name & gvBarangLama.Name & ".xml") Then
            gvBarangLama.RestoreLayoutFromXml(folderLayouts & Me.Name & gvBarangLama.Name & ".xml")
        End If
        With gvBarangLama
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
    Private Sub gvBarangBaru_DataSourceChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvBarangBaru.DataSourceChanged
        If System.IO.File.Exists(FolderLayouts & Me.Name & gvBarangBaru.Name & ".xml") Then
            gvBarangBaru.RestoreLayoutFromXml(FolderLayouts & Me.Name & gvBarangBaru.Name & ".xml")
        End If
        With gvBarangBaru
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
    Private Sub gvSatuan_DataSourceChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvSatuanLama.DataSourceChanged
        If System.IO.File.Exists(folderLayouts & Me.Name & gvSatuanLama.Name & ".xml") Then
            gvSatuanLama.RestoreLayoutFromXml(folderLayouts & Me.Name & gvSatuanLama.Name & ".xml")
        End If
        With gvSatuanLama
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
    Private Sub gvSatuanBaru_DataSourceChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvSatuanBaru.DataSourceChanged
        If System.IO.File.Exists(FolderLayouts & Me.Name & gvSatuanBaru.Name & ".xml") Then
            gvSatuanBaru.RestoreLayoutFromXml(FolderLayouts & Me.Name & gvSatuanBaru.Name & ".xml")
        End If
        With gvSatuanBaru
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
    Private Sub txtKonversi_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtKonversiLama.KeyDown
        If e.KeyCode = Keys.F7 Then
            Dim x As New frmOtorisasiAdmin
            Try
                If txtKonversiLama.Properties.ReadOnly Then
                    If x.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
                        txtKonversiLama.Properties.ReadOnly = False
                    End If
                End If
            Catch ex As Exception
                XtraMessageBox.Show("Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End If
    End Sub
    Private Sub txtKonversiBaru_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtKonversiBaru.KeyDown
        If e.KeyCode = Keys.F7 Then
            Dim x As New frmOtorisasiAdmin
            Try
                If txtKonversiBaru.Properties.ReadOnly Then
                    If x.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
                        txtKonversiBaru.Properties.ReadOnly = False
                    End If
                End If
            Catch ex As Exception
                XtraMessageBox.Show("Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End If
    End Sub
    Private Sub txtSatuan_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtSatuanLama.LostFocus
        RubahSatuanLama()
    End Sub
    Private Sub gvGudang_DataSourceChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvGudangLama.DataSourceChanged
        If System.IO.File.Exists(FolderLayouts & Me.Name & gvGudangLama.Name & ".xml") Then
            gvGudangLama.RestoreLayoutFromXml(FolderLayouts & Me.Name & gvGudangLama.Name & ".xml")
        End If
        With gvGudangLama
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
    Private Sub gvGudangBaru_DataSourceChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvGudangBaru.DataSourceChanged
        If System.IO.File.Exists(FolderLayouts & Me.Name & gvGudangBaru.Name & ".xml") Then
            gvGudangBaru.RestoreLayoutFromXml(FolderLayouts & Me.Name & gvGudangBaru.Name & ".xml")
        End If
        With gvGudangBaru
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
    Private Sub txtBarangBaru_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtBarangBaru.EditValueChanged
        Dim Ds As New DataSet
        Try
            SQL = "SELECT MBarang.Nama AS NamaBarang, MBarang.HPP AS HargaModal FROM MBarang WHERE MBarang.NoID=" & NullToLong(txtBarangBaru.EditValue)
            Ds = ExecuteDataset("Tabel", SQL)
            If Ds.Tables("Tabel").Rows.Count >= 1 Then
                txtNamaStockBaru.Text = NullToStr(Ds.Tables(0).Rows(0).Item("NamaBarang"))
                RefreshLookUpSatuan()
                If txtSatuanBaru.Text = "" Then
                    txtSatuanBaru.EditValue = DefIDSatuan
                End If
            End If
        Catch ex As Exception
            XtraMessageBox.Show("Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            Ds.Dispose()
        End Try
    End Sub
    Private Sub txtSatuanBaru_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtSatuanBaru.EditValueChanged
        RubahSatuanBaru()
    End Sub
    Private Sub txtSatuanBaru_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtSatuanBaru.LostFocus
        RubahSatuanBaru()
    End Sub

    Private Sub txtGudangLama_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtGudangLama.EditValueChanged
        RubahSatuanLama()
    End Sub

    Private Sub txtGudangBaru_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtGudangBaru.EditValueChanged
        RubahSatuanBaru()
    End Sub

    Private Sub txtBarangLama_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtBarangLama.LostFocus
        RubahSatuanLama()
    End Sub
End Class