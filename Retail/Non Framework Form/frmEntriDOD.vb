Imports System.Data
Imports System.Data.SqlClient
Imports DevExpress.XtraEditors
Imports DevExpress.Utils

Public Class frmEntriDOD
    Public IsNew As Boolean = True
    Public IsPosted As Boolean = False
    Public NoID As Long = -1
    Public IDDO As Long = -1
    Public IDCustomer As Long = -1
    Public IDGudang As Long = -1
    Public IsDariSupplier As Boolean = False
    Public IDSPK As Long = -1
    Public IsFastEntri As Boolean = False

    'Dim QtySisaSPK As Double = 0.0
    Dim oDS As New DataSet
    Dim SQL As String = ""
    Dim DefImageList As ImageList
    Dim HargaPcs As Double = 0.0
    Dim Konversi As Double = 0.0
    Public FormPemanggil As frmEntriDO

    Private Sub IsiDefault()
        DefImageList = frmMain.ImageList1
        cmdSave.ImageList = DefImageList
        cmdSave.ImageIndex = 6

        cmdTutup.ImageList = DefImageList
        cmdTutup.ImageIndex = 3
        RefreshLookUp()
        txtGudang.EditValue = IDGudang
    End Sub
    Private Sub RefreshLookUp()
        Dim ds As New DataSet
        Try
            SQL = "SELECT MGudang.NoID, MGudang.Kode, MGudang.Kode + ' - ' + MGudang.Nama AS Nama, MWilayah.Nama AS Wilayah FROM MGudang LEFT JOIN MWilayah ON MGudang.IDWilayah=MWilayah.NoID WHERE MGudang.IsActive=1 AND MGudang.InTransit=0 AND MGudang.IDWilayah=" & DefIDWilayah
            ds = ExecuteDataset("MGudang", SQL)
            txtGudang.Properties.DataSource = ds.Tables("MGudang")
            txtGudang.Properties.ValueMember = "NoID"
            txtGudang.Properties.DisplayMember = "Nama"

            SQL = "SELECT MBarang.NoID, MBarang.Kode, MBarang.Nama, MSatuan.Nama AS Satuan FROM MBarang LEFT JOIN MSatuan ON MSatuan.NoID=MBarang.IDSatuan WHERE MBarang.IsActive=1 AND NOT (MBarang.KODE='' OR MBarang.KODE IS NULL)"
            If NullTobool(EksekusiSQlSkalarNew("SELECT IsStockPerJenis FROM MSetting")) Then
                SQL &= " AND MBarang.IDJenis IN (SELECT MAlamatD.IDJenisBarang FROM MAlamatD WHERE MAlamatD.IDAlamat=" & IDCustomer & ") "
            End If
            'If IDCustomer >= 1 Then
            '    SQL &= " AND (MBarang.IDCustomer1=" & IDCustomer
            '    SQL &= " OR MBarang.IDCustomer2=" & IDCustomer
            '    SQL &= " OR MBarang.IDCustomer3=" & IDCustomer
            '    SQL &= " OR MBarang.IDCustomer4=" & IDCustomer
            '    SQL &= " OR MBarang.IDCustomer5=" & IDCustomer & ")"
            'End If
            ds = ExecuteDataset("MBarang", SQL)
            txtBarang.Properties.DataSource = ds.Tables("MBarang")
            txtBarang.Properties.ValueMember = "NoID"
            txtBarang.Properties.DisplayMember = "Kode"

        Catch ex As Exception
            XtraMessageBox.Show("Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            ds.Dispose()
        End Try
    End Sub
    Private Sub RefreshLookUpJualRetur()
        Dim ds As New DataSet
        Try
            If IsDariSupplier Then
                SQL = "SELECT MReturBeliD.NoID, MReturBeli.Kode, MGudang.Nama AS Gudang, MWilayah.Nama AS Wilayah, MBarang.Kode AS KodeBarang, MBarang.Nama AS NamaBarang, "
                SQL &= " (MReturBeliD.Harga-isnull(MReturBeliD.DISC1,0)-isnull(MReturBeliD.DISC2,0)-isnull(MReturBeliD.DISC3,0)"
                SQL &= " -(MReturBeliD.Harga*isnull(MReturBeliD.DiscPersen1,0)/100)"
                SQL &= " -(MReturBeliD.Harga*isnull(MReturBeliD.DiscPersen2,0)/100)"
                SQL &= " -(MReturBeliD.Harga*isnull(MReturBeliD.DiscPersen3,0)/100)"
                SQL &= " ) AS Harga , MReturBeliD.Qty, MSatuan.Nama AS Satuan, MReturBeliD.HargaPcs AS [Harga(Pcs)], MReturBeliD.Qty*MReturBeliD.Konversi AS [Qty(Pcs)], MReturBeliD.Qty*MReturBeliD.Konversi- " & DiSJ("MReturBeliD") & "  AS [Sisa(Pcs)]"
                SQL &= " FROM MReturBeliD LEFT JOIN MSatuan ON MSatuan.NoID=MReturBeliD.IDSatuan "
                SQL &= " LEFT JOIN MReturBeli ON MReturBeli.NoID=MReturBeliD.IDReturBeli "
                SQL &= " LEFT JOIN (MGudang LEFT JOIN MWilayah ON MWilayah.NoID=MGudang.IDWilayah) ON MGudang.NoID=MReturBeliD.IDGudang "
                SQL &= " LEFT JOIN MBarang ON MBarang.NoID=MReturBeliD.IDBarang "
                SQL &= " WHERE (MReturBeliD.Qty*MReturBeliD.Konversi- " & DiSJ("MReturBeliD") & ">0) AND MGudang.NoID=" & IDGudang & " AND MBarang.IsActive = 1 AND MBarang.NoID=" & NullToLong(txtBarang.EditValue) & " AND  MReturBeli.IsPosted = 1 And MReturBeli.NoID = " & IDSPK
            Else
                SQL = "SELECT MJualD.NoID, MJual.Kode, MGudang.Nama AS Gudang, MWilayah.Nama AS Wilayah, MBarang.Kode AS KodeBarang, MBarang.Nama AS NamaBarang, "
                SQL &= " (MJualD.Harga-isnull(MJualD.DISC1,0)-isnull(MJualD.DISC2,0)-isnull(MJualD.DISC3,0)"
                SQL &= " -(MJualD.Harga*isnull(MJualD.DiscPersen1,0)/100)"
                SQL &= " -(MJualD.Harga*isnull(MJualD.DiscPersen2,0)/100)"
                SQL &= " -(MJualD.Harga*isnull(MJualD.DiscPersen3,0)/100)"
                SQL &= " ) AS Harga , MJualD.Qty, MSatuan.Nama AS Satuan, MJualD.HargaPcs AS [Harga(Pcs)], MJualD.Qty*MJualD.Konversi AS [Qty(Pcs)], MJualD.Qty*MJualD.Konversi- " & DiSJ("MJualD") & "  AS [Sisa(Pcs)]"
                SQL &= " FROM MJualD LEFT JOIN MSatuan ON MSatuan.NoID=MJualD.IDSatuan "
                SQL &= " LEFT JOIN MJual ON MJual.NoID=MJualD.IDJual "
                SQL &= " LEFT JOIN (MGudang LEFT JOIN MWilayah ON MWilayah.NoID=MGudang.IDWilayah) ON MGudang.NoID=MJualD.IDGudang "
                SQL &= " LEFT JOIN MBarang ON MBarang.NoID=MJualD.IDBarang "
                SQL &= " WHERE (MJualD.Qty*MJualD.Konversi- " & DiSJ("MJualD") & ">0) AND MGudang.NoID=" & IDGudang & " AND MBarang.IsActive = 1 AND MBarang.NoID=" & NullToLong(txtBarang.EditValue) & " AND MJual.IsPosted = 1 And MJual.NoID = " & IDSPK
            End If
            ds = ExecuteDataset("MPOD", SQL)
            txtSPK.Properties.DataSource = ds.Tables("MPOD")
            txtSPK.Properties.ValueMember = "NoID"
            txtSPK.Properties.DisplayMember = "Kode"
        Catch ex As Exception
            XtraMessageBox.Show("Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            ds.Dispose()
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
        txt.SelectAll()
    End Sub
    Private Function DiSJ(ByVal tabel As String) As String
        Return "isnull((SELECT SUM(isnull(A.Qty,0)*isnull(A.Konversi,0)) FROM MDOD A WHERE A.NoID <> " & NoID & " AND A.IDJualD=" & tabel & ".NoID),0)"
    End Function
    Private Sub LoadData()
        Try
            SQL = "SELECT MDOD.*, MBarang.Kode AS KodeBarang, MBarang.Nama AS NamaBarang, MGudang.Kode AS KodeGudang, MGudang.Nama AS NamaGudang, MSatuan.Nama AS NamaSatuan "
            SQL &= " FROM ((MDOD LEFT JOIN MGudang ON MGudang.NoID=MDOD.IDGudang) "
            SQL &= " LEFT JOIN MBarang ON MBarang.NoID=MDOD.IDBarang)"
            SQL &= " LEFT JOIN MSatuan ON MSatuan.NoID=MDOD.IDSatuan "
            SQL &= " WHERE MDOD.NoID= " & NoID
            oDS = New DataSet()
            oDS = ExecuteDataset("MDOD", SQL)
            If oDS.Tables(0).Rows.Count >= 1 Then
                With oDS.Tables("MDOD").Rows(0)
                    NoID = NullToLong(.Item("NoID"))
                    IDDO = NullToLong(.Item("IDDO"))
                    txtSPK.EditValue = NullToLong(.Item("IDJualD"))
                    txtBarang.EditValue = NullToLong(.Item("IDBarang"))
                    'txtBarang.Text = NullTostr(.Item("KodeBarang"))
                    txtSatuan.EditValue = NullToLong(.Item("IDSatuan"))
                    'txtSatuan.Text = NullTostr(.Item("KodeSatuan"))
                    'txtGudang.EditValue = NullToLong(.Item("IDGudang"))
                    txtGudang.EditValue = IDGudang
                    'txtGudang.Text = NullTostr(.Item("KodeGudang"))
                    txtQty.EditValue = NullToDbl(.Item("Qty"))
                    txtHarga.EditValue = NullToDbl(.Item("Harga"))
                    txtCtn.EditValue = NullToDbl(.Item("CTN"))
                    txtDiscPersen1.EditValue = NullToDbl(.Item("DiscPersen1"))
                    txtDiscPersen2.EditValue = NullToDbl(.Item("DiscPersen2"))
                    txtDiscPersen3.EditValue = NullToDbl(.Item("DiscPersen3"))
                    txtDiscRp1.EditValue = NullToDbl(.Item("Disc1"))
                    txtDiscRp2.EditValue = NullToDbl(.Item("Disc2"))
                    txtDiscRp3.EditValue = NullToDbl(.Item("Disc3"))
                    txtJUmlah.EditValue = NullToDbl(.Item("Jumlah"))
                    txtCatatan.Text = NullToStr(.Item("Catatan"))
                    txtKonversi.EditValue = NullToDbl(.Item("Konversi"))
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
                SQL = "INSERT INTO MDOD (NoID,IDDO,IDJualD,NoUrut,Tgl,Jam,IDBarang,IDSatuan,Qty,QtyPcs,Harga,HargaPcs,CTN,DiscPersen1,DiscPersen2,DiscPersen3,Disc1,Disc2,Disc3,Jumlah,Catatan,IDGudang,Konversi) VALUES ("
                SQL &= NullToLong(GetNewID("MDOD", "NoID")) & ","
                SQL &= IDDO & ","
                SQL &= NullToLong(txtSPK.EditValue) & ","
                SQL &= GetNewID("MDOD", "NoUrut", " WHERE IDDO=" & IDDO) & ","
                SQL &= "GetDate(),"
                SQL &= "GetDate(),"
                SQL &= NullToLong(txtBarang.EditValue) & ","
                SQL &= NullToLong(txtSatuan.EditValue) & ","
                SQL &= FixKoma(txtQty.EditValue) & ","
                SQL &= FixKoma(txtQty.EditValue * txtKonversi.EditValue) & ","
                SQL &= FixKoma(txtHarga.EditValue) & ","
                SQL &= FixKoma(txtJUmlah.EditValue / IIf(txtQty.EditValue = 0, 1, txtQty.EditValue) / IIf(txtKonversi.EditValue = 0, 1, txtKonversi.EditValue)) & ","
                SQL &= FixKoma(txtCtn.EditValue) & ","
                SQL &= FixKoma(txtDiscPersen1.EditValue) & ","
                SQL &= FixKoma(txtDiscPersen2.EditValue) & ","
                SQL &= FixKoma(txtDiscPersen3.EditValue) & ","
                SQL &= FixKoma(txtDiscRp1.EditValue) & ","
                SQL &= FixKoma(txtDiscRp2.EditValue) & ","
                SQL &= FixKoma(txtDiscRp3.EditValue) & ","
                SQL &= FixKoma(txtJUmlah.EditValue) & ","
                SQL &= "'" & FixApostropi(txtCatatan.Text) & "',"
                SQL &= NullToLong(txtGudang.EditValue) & ","
                SQL &= FixKoma(txtKonversi.EditValue) & ""
                SQL &= ")"
            Else
                SQL = "UPDATE MDOD SET "
                SQL &= " Tgl=GetDate(),"
                SQL &= " Jam=GetDate(),"
                SQL &= " IDDO=" & IDDO & ","
                SQL &= " IDJualD=" & NullToLong(txtSPK.EditValue) & ","
                SQL &= " IDBarang=" & NullToLong(txtBarang.EditValue) & ","
                SQL &= " IDSatuan=" & NullToLong(txtSatuan.EditValue) & ","
                SQL &= " Qty=" & FixKoma(txtQty.EditValue) & ","
                SQL &= " QtyPcs=" & FixKoma(txtQty.EditValue * txtKonversi.EditValue) & ","
                SQL &= " Harga=" & FixKoma(txtHarga.EditValue) & ","
                SQL &= " HargaPcs=" & FixKoma(txtJUmlah.EditValue / IIf(txtQty.EditValue = 0, 1, txtQty.EditValue) / IIf(txtKonversi.EditValue = 0, 1, txtKonversi.EditValue)) & ","
                SQL &= " CTN=" & FixKoma(txtCtn.EditValue) & ","
                SQL &= " DiscPersen1=" & FixKoma(txtDiscPersen1.EditValue) & ","
                SQL &= " DiscPersen2=" & FixKoma(txtDiscPersen2.EditValue) & ","
                SQL &= " DiscPersen3=" & FixKoma(txtDiscPersen3.EditValue) & ","
                SQL &= " Disc1=" & FixKoma(txtDiscRp1.EditValue) & ","
                SQL &= " Disc2=" & FixKoma(txtDiscRp2.EditValue) & ","
                SQL &= " Disc3=" & FixKoma(txtDiscRp3.EditValue) & ","
                SQL &= " Jumlah=" & FixKoma(txtJUmlah.EditValue) & ","
                SQL &= " Catatan='" & FixApostropi(txtCatatan.Text) & "',"
                SQL &= " IDGudang=" & NullToLong(txtGudang.EditValue) & ","
                SQL &= " Konversi=" & FixKoma(txtKonversi.EditValue) & ""
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
        If txtGudang.Text = "" Then
            XtraMessageBox.Show("Gudang masih kosong.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
            txtGudang.Focus()
            Return False
            Exit Function
        End If
        If txtQty.EditValue = 0 Then
            XtraMessageBox.Show("Qty masih 0.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
            txtQty.Focus()
            Return False
            Exit Function
        End If
        'If txtSPK.Text = "" Then
        '    XtraMessageBox.Show("Item SPK harus diisi.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
        '    txtSPK.Focus()
        '    Return False
        '    Exit Function
        'End If
        If txtSPK.Text <> "" Then
            If QtySisa() - txtQty.EditValue * txtKonversi.EditValue <> 0 Then
                XtraMessageBox.Show("Qty harus sama dengan Qty " & IIf(IsDariSupplier, "retur pembelian", "Penjualan") & ".", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
                txtQty.Focus()
                Return False
                Exit Function
            End If
        End If
        Return True
    End Function
    Private Function QtySisa() As Double
        Dim x As Double = 0.0
        Try
            x = NullToDbl(EksekusiSQlSkalarNew("Select ((MJualD.Qty*MJualD.Konversi)-" & DiSJ(IIf(IsDariSupplier, "MReturBeliD", "MJualD")) & ") AS QtySisa FROM MJualD WHERE NoID=" & NullToLong(txtSPK.EditValue)))
        Catch ex As Exception

        End Try
        Return x
    End Function
    'Private Function DiSJ() As String
    '    Return "isnull((SELECT SUM(isnull(A.Qty,0)*isnull(A.Konversi,0)) FROM MDOD A WHERE A.NoID <> " & NoID & " AND A.IDJualD=MDOD.NoID),0)"
    'End Function
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
                gvSatuan.SaveLayoutToXml(FolderLayouts & Me.Name & gvSatuan.Name & ".xml")
                gvSPK.SaveLayoutToXml(FolderLayouts & Me.Name & gvSPK.Name & ".xml")
            End If
        Catch ex As Exception
            XtraMessageBox.Show("Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            xOtorisasi.Dispose()
        End Try
    End Sub

    Private Sub frmEntriDOD_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        Try
            If DialogResult = Windows.Forms.DialogResult.OK Then
                FormPemanggil.LoadDataJualRetur()
                FormPemanggil.RefreshDetil()
                FormPemanggil.Show()
                FormPemanggil.Focus()
                FormPemanggil.txtBarang.Focus()
            Else
                If IsFastEntri Then
                    EksekusiSQL("DELETE FROM MDOD WHERE NoID=" & NoID)
                End If
                FormPemanggil.Show()
                FormPemanggil.Focus()
            End If
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
            If Not IsNew Then
                LoadData()
            End If
            Me.Width = Ini.BacaIniPath(FolderLayouts & Me.Name & ".ini", "Form", "Width", Me.Width)
            Me.Height = Ini.BacaIniPath(FolderLayouts & Me.Name & ".ini", "Form", "Height", Me.Height)

            Me.Left = (Screen.PrimaryScreen.WorkingArea.Width - Me.Width) \ 2
            Me.Top = (Screen.PrimaryScreen.WorkingArea.Height - Me.Height) \ 2

            If System.IO.File.Exists(FolderLayouts & Me.Name & LayoutControl1.Name & ".xml") Then
                LayoutControl1.RestoreLayoutFromXml(FolderLayouts & Me.Name & LayoutControl1.Name & ".xml")
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
            Dim SubTotal As Double = (txtHarga.EditValue - txtDiscRp1.EditValue - txtDiscRp2.EditValue - txtDiscRp3.EditValue)
            Dim DiscA As Double = txtHarga.EditValue * txtQty.EditValue * txtDiscPersen1.EditValue / 100
            Dim DiscB As Double = txtHarga.EditValue * txtQty.EditValue * txtDiscPersen2.EditValue / 100
            Dim DiscC As Double = txtHarga.EditValue * txtQty.EditValue * txtDiscPersen3.EditValue / 100
            txtJUmlah.EditValue = (SubTotal * txtQty.EditValue) - DiscA - DiscB - DiscC
        Catch ex As Exception

        End Try
    End Sub

    Private Sub txtQty_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtQty.LostFocus
        RubahSatuan()
        HitungJumlah()
    End Sub

    Private Sub txtDiscPersen1_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtDiscPersen1.LostFocus
        HitungJumlah()
    End Sub
    Private Sub txtDiscPersen2_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtDiscPersen2.LostFocus
        HitungJumlah()
    End Sub
    Private Sub txtDiscPersen3_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtDiscPersen3.LostFocus
        HitungJumlah()
    End Sub
    Private Sub txtDisc1_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtDiscRp1.LostFocus
        HitungJumlah()
    End Sub
    Private Sub txtDisc2_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtDiscRp2.LostFocus
        HitungJumlah()
    End Sub
    Private Sub txtDisc3_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtDiscRp3.LostFocus
        HitungJumlah()
    End Sub
    Private Sub txtDiscPersen1_EditValueChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtDiscPersen1.EditValueChanged
        HitungJumlah()
    End Sub
    Private Sub txtDiscPersen2_EditValueChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtDiscPersen2.EditValueChanged
        HitungJumlah()
    End Sub
    Private Sub txtDiscPersen3_EditValueChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtDiscPersen3.EditValueChanged
        HitungJumlah()
    End Sub
    Private Sub txtDisc1_EditValueChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtDiscRp1.EditValueChanged
        HitungJumlah()
    End Sub
    Private Sub txtDisc2_EditValueChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtDiscRp2.EditValueChanged
        HitungJumlah()
    End Sub
    Private Sub txtDisc3_EditValueChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtDiscRp3.EditValueChanged
        HitungJumlah()
    End Sub
    Private Sub txtBarang_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtBarang.EditValueChanged
        Dim Ds As New DataSet
        Try
            SQL = "SELECT MBarang.Nama AS NamaBarang FROM MBarang WHERE MBarang.NoID=" & NullToLong(txtBarang.EditValue)
            Ds = ExecuteDataset("Tabel", SQL)
            If Ds.Tables("Tabel").Rows.Count >= 1 Then
                txtNamaStock.Text = NullToStr(Ds.Tables(0).Rows(0).Item("NamaBarang"))
                RefreshLookUpSatuan()
                RefreshLookUpJualRetur()
                If IsNew Then
                    txtSatuan.EditValue = DefIDSatuan
                End If
                If IsNew Or IsFastEntri Then
                    'txtHarga.EditValue = clsPostingPenjualan.HargaJual(NullToLong(txtBarang.EditValue), NullToLong(txtSatuan.EditValue), IDCustomer, txtDiscPersen1.EditValue, txtDiscPersen2.EditValue)
                End If
            End If
        Catch ex As Exception
            XtraMessageBox.Show("Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            Ds.Dispose()
        End Try
    End Sub
    Private Sub RefreshLookUpSatuan()
        Dim ds As New DataSet
        Try
            SQL = "SELECT MSatuan.NoID, MSatuan.Kode, MSatuan.Nama FROM MSatuan WHERE MSatuan.IsActive=1 AND MSatuan.NoID IN (SELECT MBarangD.IDSatuan FROM MBarangD WHERE MBarangD.IsJual=1 AND MBarangD.IsActive=1 AND MBarangD.IDBarang=" & NullToLong(txtBarang.EditValue) & ")"
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
    Private Sub txtSatuan_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtSatuan.EditValueChanged
        RubahSatuan()
    End Sub
    Private Sub RubahSatuan()
        Dim Ds As New DataSet
        Dim IsiKarton As Double = 0.0
        Try
            SQL = "SELECT MBarangD.Konversi, MBarang.CtnPcs FROM MBarangD LEFT JOIN MBarang ON MBarang.NoID=MBarangD.IDBarang LEFT JOIN MSatuan ON MSatuan.NoID=MBarangD.IDSatuan WHERE MBarangD.IDBarang=" & NullToLong(txtBarang.EditValue) & " AND MSatuan.NoID=" & NullToLong(txtSatuan.EditValue)
            Ds = ExecuteDataset("Tabel", SQL)
            If Ds.Tables("Tabel").Rows.Count >= 1 Then
                txtKonversi.EditValue = NullToDbl(Ds.Tables(0).Rows(0).Item("Konversi"))
                IsiKarton = NullToDbl(Ds.Tables(0).Rows(0).Item("CtnPcs"))
                If IsiKarton = 0 Then
                    txtCtn.EditValue = 0
                Else
                    txtCtn.EditValue = txtQty.EditValue / IsiKarton * txtKonversi.EditValue
                End If

                If txtHarga.EditValue = 0 Or IsNew Or IsFastEntri Then
                    'txtHarga.EditValue = clsPostingPenjualan.HargaJual(NullToLong(txtBarang.EditValue), NullToLong(txtSatuan.EditValue), IDCustomer, txtDiscPersen1.EditValue, txtDiscPersen2.EditValue)
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

    Private Sub gvSPK_DataSourceChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvSPK.DataSourceChanged
        If System.IO.File.Exists(FolderLayouts & Me.Name & gvSPK.Name & ".xml") Then
            gvSPK.RestoreLayoutFromXml(FolderLayouts & Me.Name & gvSPK.Name & ".xml")
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
        Try
            If IsDariSupplier Then
                SQL = "SELECT MReturBeliD.*, (MReturBeliD.Qty*MReturBeliD.Konversi)-" & DiSJ(IIf(IsDariSupplier, "MReturBeliD", "MJualD")) & " AS QtySisa, MBarang.Kode AS KodeBarang, MBarang.Nama AS NamaBarang, MGudang.Kode AS KodeGudang, MGudang.Nama AS NamaGudang, MSatuan.Nama AS NamaSatuan "
                SQL &= " FROM ((MReturBeliD LEFT JOIN MGudang ON MGudang.NoID=MReturBeliD.IDGudang) "
                SQL &= " LEFT JOIN MBarang ON MBarang.NoID=MReturBeliD.IDBarang)"
                SQL &= " LEFT JOIN MSatuan ON MSatuan.NoID=MReturBeliD.IDSatuan "
                SQL &= " WHERE MReturBeliD.NoID= " & NullToLong(txtSPK.EditValue)
            Else
                SQL = "SELECT MJualD.*, (MJualD.Qty*MJualD.Konversi)-" & DiSJ(IIf(IsDariSupplier, "MReturBeliD", "MJualD")) & " AS QtySisa, MBarang.Kode AS KodeBarang, MBarang.Nama AS NamaBarang, MGudang.Kode AS KodeGudang, MGudang.Nama AS NamaGudang, MSatuan.Nama AS NamaSatuan "
                SQL &= " FROM ((MJualD LEFT JOIN MGudang ON MGudang.NoID=MJualD.IDGudang) "
                SQL &= " LEFT JOIN MBarang ON MBarang.NoID=MJualD.IDBarang)"
                SQL &= " LEFT JOIN MSatuan ON MSatuan.NoID=MJualD.IDSatuan "
                SQL &= " WHERE MJualD.NoID= " & NullToLong(txtSPK.EditValue)
            End If
            Ds = New DataSet()
            Ds = ExecuteDataset("MPOD", SQL)
            If Ds.Tables(0).Rows.Count >= 1 Then
                With Ds.Tables("MPOD").Rows(0)
                    'gvSPK.SaveLayoutToXml(folderLayouts &  Me.Name & gvSPK.Name & ".xml")
                    txtBarang.EditValue = NullToLong(.Item("IDBarang"))
                    'txtBarang.Text = NullTostr(.Item("KodeBarang"))
                    txtSatuan.EditValue = NullToLong(.Item("IDSatuan"))
                    'txtSatuan.Text = NullTostr(.Item("KodeSatuan"))
                    txtGudang.EditValue = NullToLong(.Item("IDGudang"))
                    'txtGudang.Text = NullTostr(.Item("KodeGudang"))
                    'QtySisaSPK = NullToDbl(.Item("QtySisa"))
                    Konversi = IIf(NullToDbl(.Item("Konversi")) = 0, 1, NullToDbl(.Item("Konversi")))
                    HargaPcs = NullToDbl(.Item("Harga")) / Konversi

                    txtQty.EditValue = NullToDbl(.Item("QtySisa")) / Konversi
                    txtHarga.EditValue = NullToDbl(.Item("Harga"))
                    txtCtn.EditValue = NullToDbl(.Item("CTN"))
                    txtDiscPersen1.EditValue = NullToDbl(.Item("DiscPersen1"))
                    txtDiscPersen2.EditValue = NullToDbl(.Item("DiscPersen2"))
                    txtDiscPersen3.EditValue = NullToDbl(.Item("DiscPersen3"))
                    txtDiscRp1.EditValue = NullToDbl(.Item("Disc1"))
                    txtDiscRp2.EditValue = NullToDbl(.Item("Disc2"))
                    txtDiscRp3.EditValue = NullToDbl(.Item("Disc3"))
                    txtJUmlah.EditValue = NullToDbl(.Item("Jumlah"))
                    txtCatatan.Text = NullToStr(.Item("Catatan"))
                    txtKonversi.EditValue = NullToDbl(.Item("Konversi"))
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

    Private Sub txtQty_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtQty.EditValueChanged
        HitungJumlah()
    End Sub

    Private Sub txtHarga_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtHarga.EditValueChanged
        HitungJumlah()
    End Sub
End Class