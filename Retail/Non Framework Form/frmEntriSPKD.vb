Imports System.Data
Imports System.Data.SqlClient
Imports DevExpress.XtraEditors
Imports DevExpress.Utils

Public Class frmEntriSPKD
    Public IsNew As Boolean = True
    Public IsPosted As Boolean = False
    Public NoID As Long = -1
    Public IDSPK As Long = -1
    Public IDCustomer As Long = -1
    'Public IDSO As Long = -1

    Dim QtySisaSO As Double = 0.0
    Dim oDS As New DataSet
    Dim SQL As String = ""
    Dim DefImageList As ImageList
    Dim HargaPcs As Double = 0.0
    Dim Konversi As Double = 0.0
    Public IsFastEntri As Boolean = False
    Public FormPemanggil As frmEntriSPK

    Public Enum SPK
        IsSPK = 0
        IsPacking = 1
    End Enum

    Public pSPK As SPK
    'Public IDGudang As Long = -1

    Private Sub IsiDefault()
        DefImageList = frmMain.ImageList1
        cmdSave.ImageList = DefImageList
        cmdSave.ImageIndex = 6

        cmdTutup.ImageList = DefImageList
        cmdTutup.ImageIndex = 3
        RefreshLookUp()

        txtGudang.EditValue = DefIDGudang
        RubahGudang()
    End Sub
    Private Sub RefreshLookUp()
        Dim ds As New DataSet
        Try
            SQL = "SELECT MGudang.NoID, MGudang.Kode, MGudang.Kode + ' - ' + MGudang.Nama AS Nama, MWilayah.Nama AS Wilayah FROM MGudang LEFT JOIN MWilayah ON MGudang.IDWilayah=MWilayah.NoID WHERE MGudang.IsActive=1 AND MGudang.InTransit=0 AND MGudang.IDWilayah=" & DefIDWilayah & " "
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
    Private Sub RefreshLookUpSO()
        Dim ds As New DataSet
        Try
            SQL = "SELECT MSOD.NoID, MSO.Kode, MGudang.Nama AS Gudang, MWilayah.Nama AS Wilayah, MBarang.Kode AS KodeBarang, MBarang.Nama AS NamaBarang, "
            SQL &= " (MSOD.Jumlah/isnull(MSOD.Konversi,1)/isnull(MSOD.Qty,1)) AS [Harga (Pcs)],"
            SQL &= " MSOD.Qty, MSatuan.Nama AS Satuan, MSOD.Qty*MSOD.Konversi AS [Qty(Pcs)], MSOD.Qty*MSOD.Konversi- " & DiSPK() & "  AS [Sisa(Pcs)]"
            SQL &= " FROM MSOD LEFT JOIN MSatuan ON MSatuan.NoID=MSOD.IDSatuan "
            SQL &= " LEFT JOIN MSO ON MSO.NoID=MSOD.IDSO "
            SQL &= " LEFT JOIN (MGudang LEFT JOIN MWilayah ON MWilayah.NoID=MGudang.IDWilayah) ON MGudang.NoID=MSOD.IDGudang "
            SQL &= " LEFT JOIN MBarang ON MBarang.NoID=MSOD.IDBarang "
            SQL &= " WHERE (MSOD.Qty*MSOD.Konversi- " & DiSPK() & ">0) AND MBarang.IsActive = 1 And MSO.IsPosted = 1 AND MBarang.NoID=" & NullToLong(txtBarang.EditValue) & " AND (IsSelesai=0 OR IsSelesai Is Null)"
            ds = ExecuteDataset("MPOD", SQL)
            txtSO.Properties.DataSource = ds.Tables("MPOD")
            txtSO.Properties.ValueMember = "NoID"
            txtSO.Properties.DisplayMember = "Kode"
        Catch ex As Exception
            XtraMessageBox.Show("Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            ds.Dispose()
        End Try
    End Sub
    Private Function DiSPK() As String
        Return "isnull((SELECT SUM(isnull(A.Qty,0)*isnull(A.Konversi,0)) FROM MSPKD A INNER JOIN MSPK B ON B.NoID=A.IDSPK WHERE A.NoID <> " & NoID & " AND A.IDSOD=MSOD.NoID),0)"
    End Function
    Private Sub LoadData()
        Try
            SQL = "SELECT MSPKD.*, MBarang.Kode AS KodeBarang, MBarang.Nama AS NamaBarang, MGudang.Kode AS KodeGudang, MGudang.Nama AS NamaGudang, MSatuan.Nama AS NamaSatuan "
            SQL &= " FROM ((MSPKD LEFT JOIN MGudang ON MGudang.NoID=MSPKD.IDGudang) "
            SQL &= " LEFT JOIN MBarang ON MBarang.NoID=MSPKD.IDBarang)"
            SQL &= " LEFT JOIN MSatuan ON MSatuan.NoID=MSPKD.IDSatuan "
            SQL &= " WHERE MSPKD.NoID= " & NoID
            oDS = New DataSet()
            oDS = ExecuteDataset("MSPKD", SQL)
            If oDS.Tables(0).Rows.Count >= 1 Then
                With oDS.Tables("MSPKD").Rows(0)
                    NoID = NullTolong(.Item("NoID"))
                    IDSPK = NullTolong(.Item("IDSPK"))
                    txtSO.EditValue = NullToLong(.Item("IDSOD"))
                    txtBarang.EditValue = NullToLong(.Item("IDBarang"))
                    'txtBarang.Text = NullTostr(.Item("KodeBarang"))
                    txtSatuan.EditValue = NullTolong(.Item("IDSatuan"))
                    'txtSatuan.Text = NullTostr(.Item("KodeSatuan"))
                    txtGudang.EditValue = NullToLong(.Item("IDGudang"))
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
                    txtCatatan.Text = NullTostr(.Item("Catatan"))
                    txtKonversi.EditValue = NullToDbl(.Item("Konversi"))
                    txtQtyKarton.EditValue = NullToDbl(.Item("QtyKarton"))
                    txtNoPacking.Text = NullTostr(.Item("NoPacking"))
                    ckPacking.Checked = NullTostr(.Item("IsPacking"))
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
                SQL = "INSERT INTO MSPKD (NoID,IDSOD,IDSPK,NoUrut,Tgl,Jam,IDBarang,IDSatuan,Qty,QtyPcs,Harga,HargaPcs,CTN,DiscPersen1,DiscPersen2,DiscPersen3,Disc1,Disc2,Disc3,Jumlah,Catatan,IDGudang,Konversi,IsPacking,QtyKarton,NoPacking) VALUES ("
                SQL &= NullTolong(GetNewID("MSPKD", "NoID")) & ","
                SQL &= NullTolong(txtSO.EditValue) & ","
                SQL &= IDSPK & ","
                SQL &= GetNewID("MSPKD", "NoUrut", " WHERE IDSPK=" & IDSPK) & ","
                SQL &= "GetDate(),"
                SQL &= "GetDate(),"
                SQL &= NullTolong(txtBarang.EditValue) & ","
                SQL &= NullTolong(txtSatuan.EditValue) & ","
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
                SQL &= NullTolong(txtGudang.EditValue) & ","
                SQL &= FixKoma(txtKonversi.EditValue) & ","
                SQL &= IIf(ckPacking.Checked, 1, 0) & ","
                SQL &= FixKoma(txtQtyKarton.EditValue) & ","
                SQL &= "'" & FixApostropi(txtNoPacking.Text) & "'"
                SQL &= ")"
            Else
                SQL = "UPDATE MSPKD SET "
                SQL &= " Tgl=GetDate(),"
                SQL &= " Jam=GetDate(),"
                SQL &= " IDSPK=" & IDSPK & ","
                SQL &= " IDSOD=" & NullTolong(txtSO.EditValue) & ","
                SQL &= " IDBarang=" & NullTolong(txtBarang.EditValue) & ","
                SQL &= " IDSatuan=" & NullTolong(txtSatuan.EditValue) & ","
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
                SQL &= " IDGudang=" & NullTolong(txtGudang.EditValue) & ","
                SQL &= " Konversi=" & FixKoma(txtKonversi.EditValue) & ","
                SQL &= " IsPacking=" & IIf(ckPacking.Checked, 1, 0) & ","
                SQL &= " QtyKarton=" & FixKoma(txtQtyKarton.EditValue) & ","
                SQL &= " NoPacking='" & FixApostropi(txtNoPacking.Text) & "'"
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
        If txtQty.EditValue * txtKonversi.EditValue > SisaQty() Then
            If XtraMessageBox.Show("Qty melebihi stok " & txtGudang.Text & " ." & vbCrLf & "Ingin melanjutkan penyimpanan?", NamaAplikasi, MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) = Windows.Forms.DialogResult.No Then
                txtQty.Focus()
                Return False
                Exit Function
            End If
        End If
        If txtSO.Text <> "" Then
            QtySisaSO = NullToDbl(EksekusiSQlSkalarNew("Select ((MSOD.Qty*MSOD.Konversi)-" & DiSPK() & ") AS QtySisa FROM MSOD WHERE NoID=" & NullToLong(txtSO.EditValue)))
            If QtySisaSO < txtQty.EditValue * txtKonversi.EditValue Then
                XtraMessageBox.Show("Qty melebihi standart Sales Order.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
                txtQty.Focus()
                Return False
                Exit Function
            End If
        End If
        If pSPK = SPK.IsSPK AndAlso ckPacking.Checked Then
            XtraMessageBox.Show("Barang yang sudah dipacking tidak boleh diedit.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
            Return False
            Exit Function
        End If
        Return True
    End Function
    Private Function SisaQty() As Double
        'Dim ds As New DataSet
        Dim Qty As Double = 0.0
        Try
            'SQL = "SELECT SUM((isnull(QtyMasuk,0)*isnull(Konversi,0))-(isnull(QtyKeluar,0)*isnull(Konversi,0))) AS QtySisa FROM MKartuStok WHERE IDBarang=" & NullTolong(txtBarang.EditValue) & " AND IDGudang=" & NullTolong(txtGudang.EditValue) & " AND (IsSPK=0 OR IsSPK Is NULL)"
            'SQL = "SELECT MWilayah.Nama AS Wilayah, MGudang.Nama AS Gudang, MBarang.Nama, SUM(MKartuStok.QtyMasuk*MkartuStok.Konversi) AS Stok, SUM((MKartuStok.QtyMasuk*MkartuStok.Konversi)-(MKartuStok.QtyKeluar*MkartuStok.Konversi))-IsNull(TSPK.DiSPK,0) AS QtySisa" & vbCrLf
            'SQL &= " FROM MKartuStok" & vbCrLf
            'SQL &= " LEFT JOIN (MGudang LEFT JOIN MWilayah ON MWilayah.NoID=MGudang.IDWilayah) ON MGudang.NoID=MKartuStok.IDGudang" & vbCrLf
            'SQL &= " LEFT JOIN MBarang ON MBarang.NoID=MKartuStok.IDBarang" & vbCrLf
            'SQL &= " Left Join" & vbCrLf
            'SQL &= " (SELECT X.IDBarang, X.IDGudang, SUM(X.DiSPK) AS DiSPK FROM" & vbCrLf
            'SQL &= " (SELECT MSPKD.IDBarang, MSPKD.IDGudang, (MSPKD.Qty*MSPKD.Konversi) AS SPK, " & vbCrLf
            'SQL &= " (MSPKD.Qty*MSPKD.Konversi)-IsNull(" & vbCrLf
            'SQL &= " (SELECT SUM(B.Qty*B.Konversi) FROM MJualD B WHERE B.IDBarang=" & NullTolong(txtBarang.EditValue) & " AND B.IDGudang=" & NullTolong(txtGudang.EditValue) & " AND B.IDSPKD<>" & NoID & " AND B.IDSPKD=MSPKD.NoID),0) AS DiSPK " & vbCrLf
            'SQL &= " FROM MSPKD WHERE MSPKD.IDBarang=" & NullTolong(txtBarang.EditValue) & " AND MSPKD.IDGudang=" & NullTolong(txtGudang.EditValue) & " AND MSPKD.NoID<>" & NoID & ") X" & vbCrLf
            'SQL &= " GROUP BY X.IDBarang, X.IDGudang) TSPK ON MKartuStok.IDBarang=TSPK.IDBarang AND MKartuStok.IDGudang=TSPK.IDGudang" & vbCrLf
            'SQL &= " WHERE MKartuStok.IDBarang=" & NullTolong(txtBarang.EditValue) & " AND MKartuStok.IDGudang=" & NullTolong(txtGudang.EditValue) & " AND (MKartuStok.IsSPK=0 OR MKartuStok.IsSPK Is NULL)"
            'SQL &= " GROUP BY TSPK.DiSPK, MWilayah.Nama, MGudang.Nama, MBarang.Nama" & vbCrLf

            Qty = NullToDbl(EksekusiSQlSkalarNew("SELECT SUM((QtyMasuk-QtyKeluar)*Konversi) FROM MKartuStok WHERE IDBarang=" & NullToLong(txtBarang.EditValue) & " AND IDGudang=" & NullToLong(txtGudang.EditValue)))
            SQL = "SELECT SUM(Jumlah) FROM (SELECT (MSPKD.Qty*MSPKD.Konversi)-IsNull((SELECT SUM(MPackingD.Qty*MPackingD.Konversi) FROM MPackingD INNER JOIN MPacking ON MPacking.NoID=MPackingD.IDPacking WHERE MPackingD.IDSPKD=MSPKD.NoID),0) AS Jumlah FROM MSPKD INNER JOIN MSPK ON MSPK.NoID=MSPKD.IDSPK WHERE MSPKD.IDBarang=" & NullToLong(txtBarang.EditValue) & " AND MSPKD.IDGudang=" & NullToLong(txtGudang.EditValue) & ") X"
            Qty = Qty - NullToDbl(EksekusiSQlSkalarNew(SQL))
            Return Qty
            'ds = ExecuteDataset("Qty", SQL)
            'If ds.Tables(0).Rows.Count >= 1 Then
            '    Return NullToDbl(ds.Tables(0).Rows(0).Item("QtySisa"))
            'Else
            '    Return 0
            'End If
        Catch ex As Exception
            XtraMessageBox.Show("Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return 0
            'Finally
            '    oDS.Dispose()
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
                gvGudang.SaveLayoutToXml(folderLayouts & Me.Name & gvGudang.Name & ".xml")
                gvSatuan.SaveLayoutToXml(folderLayouts & Me.Name & gvSatuan.Name & ".xml")
                GridView1.SaveLayoutToXml(folderLayouts & Me.Name & GridView1.Name & ".xml")
                gvSO.SaveLayoutToXml(folderLayouts & Me.Name & gvSO.Name & ".xml")
            End If
        Catch ex As Exception
            XtraMessageBox.Show("Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            xOtorisasi.Dispose()
        End Try
    End Sub

    Private Sub frmEntriSPKD_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        Try
            If DialogResult = Windows.Forms.DialogResult.OK Then
                FormPemanggil.LoadDetilSO()
                FormPemanggil.RefreshDetil()
                FormPemanggil.Show()
                FormPemanggil.Focus()
                FormPemanggil.txtBarang.Focus()
            Else
                If IsFastEntri Then
                    EksekusiSQL("DELETE FROM MSPKD WHERE NoID=" & NoID)
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
            Else
                RefreshStok()
            End If
            If pSPK = SPK.IsPacking Then
                txtBarang.Properties.ReadOnly = True
                txtCatatan.Properties.ReadOnly = True
                txtCtn.Properties.ReadOnly = True
                txtDiscPersen1.Properties.ReadOnly = True
                txtDiscPersen2.Properties.ReadOnly = True
                txtDiscPersen3.Properties.ReadOnly = True
                txtDiscRp1.Properties.ReadOnly = True
                txtDiscRp2.Properties.ReadOnly = True
                txtDiscRp3.Properties.ReadOnly = True
                txtGudang.Properties.ReadOnly = True
                txtHarga.Properties.ReadOnly = True
                txtJUmlah.Properties.ReadOnly = True
                txtKonversi.Properties.ReadOnly = True
                txtQty.Properties.ReadOnly = True
                txtSatuan.Properties.ReadOnly = True
                txtSO.Properties.ReadOnly = True

                txtNoPacking.Properties.ReadOnly = False
                txtQtyKarton.Properties.ReadOnly = False
                ckPacking.Properties.ReadOnly = False
                ckPacking.Checked = True
            Else
                txtNoPacking.Properties.ReadOnly = True
                txtQtyKarton.Properties.ReadOnly = True
                ckPacking.Properties.ReadOnly = True
                'ckPacking.Checked = False
            End If

            Me.Width = Ini.BacaIniPath(folderLayouts & Me.Name & ".ini", "Form", "Width", Me.Width)
            Me.Height = Ini.BacaIniPath(folderLayouts & Me.Name & ".ini", "Form", "Height", Me.Height)

            Me.Left = (Screen.PrimaryScreen.WorkingArea.Width - Me.Width) \ 2
            Me.Top = (Screen.PrimaryScreen.WorkingArea.Height - Me.Height) \ 2

            If System.IO.File.Exists(folderLayouts & Me.Name & LayoutControl1.Name & ".xml") Then
                LayoutControl1.RestoreLayoutFromXml(folderLayouts & Me.Name & LayoutControl1.Name & ".xml")
            End If
            If System.IO.File.Exists(folderLayouts & Me.Name & GridView1.Name & ".xml") Then
                GridView1.RestoreLayoutFromXml(folderLayouts & Me.Name & GridView1.Name & ".xml")
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

    Private Sub HitungJumlah()
        Dim ds As New DataSet
        Dim IsiKarton As Double = 0.0
        Try
            'Dim SubTotal As Double = (txtHarga.EditValue - txtDiscRp1.EditValue - txtDiscRp2.EditValue - txtDiscRp3.EditValue)
            'Dim DiscA As Double = txtHarga.EditValue * txtQty.EditValue * txtDiscPersen1.EditValue / 100
            'Dim DiscB As Double = txtHarga.EditValue * txtQty.EditValue * txtDiscPersen2.EditValue / 100
            'Dim DiscC As Double = txtHarga.EditValue * txtQty.EditValue * txtDiscPersen3.EditValue / 100
            'txtJUmlah.EditValue = (SubTotal * txtQty.EditValue) - DiscA - DiscB - DiscC
            SQL = "SELECT MBarangD.Konversi, MBarang.CtnPcs FROM MBarangD LEFT JOIN MBarang ON MBarang.NoID=MBarangD.IDBarang LEFT JOIN MSatuan ON MSatuan.NoID=MBarangD.IDSatuan WHERE MBarangD.IDBarang=" & NullToLong(txtBarang.EditValue) & " AND MSatuan.NoID=" & NullToLong(txtSatuan.EditValue)
            ds = ExecuteDataset("Tabel", SQL)
            If ds.Tables("Tabel").Rows.Count >= 1 Then
                txtKonversi.EditValue = NullToDbl(ds.Tables(0).Rows(0).Item("Konversi"))
                IsiKarton = NullToDbl(ds.Tables(0).Rows(0).Item("CtnPcs"))
                If IsiKarton = 0 Then
                    txtCtn.EditValue = 0
                Else
                    txtCtn.EditValue = txtQty.EditValue / IsiKarton * txtKonversi.EditValue
                End If
            End If
            txtJUmlah.EditValue = txtQty.EditValue * (Bulatkan((txtHarga.EditValue * (1 - (txtDiscPersen1.EditValue / 100)) * (1 - (txtDiscPersen2.EditValue / 100)) * (1 - (txtDiscPersen3.EditValue / 100))), 0) - txtDiscRp1.EditValue - txtDiscRp2.EditValue - txtDiscRp3.EditValue)
        Catch ex As Exception
        Finally
            ds.Dispose()
        End Try
    End Sub

    Private Sub txtQty_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtQty.LostFocus
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
            SQL = "SELECT MBarang.Nama FROM MBarang WHERE MBarang.NoID=" & NullTolong(txtBarang.EditValue)
            Ds = ExecuteDataset("Tabel", SQL)
            If Ds.Tables("Tabel").Rows.Count >= 1 Then
                'txtSatuan.EditValue = NullTolong(Ds.Tables(0).Rows(0).Item("IDSatuan"))
                'txtKonversi.EditValue = NullToDbl(Ds.Tables(0).Rows(0).Item("Konversi"))
                txtNamaStock.Text = NullTostr(Ds.Tables(0).Rows(0).Item("Nama"))
                RefreshLookUpSatuan()
                RefreshLookUpSO()
                txtSatuan.EditValue = DefIDSatuan
                If IsNew Then
                    'txtHarga.EditValue = clsPostingPenjualan.HargaJual(NullToLong(txtBarang.EditValue), NullToLong(txtSatuan.EditValue), IDCustomer, txtDiscPersen1.EditValue, txtDiscPersen2.EditValue)
                End If
                RefreshStok()
            End If
        Catch ex As Exception
            XtraMessageBox.Show("Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            Ds.Dispose()
        End Try
    End Sub
    Private Sub RefreshStok()
        Dim ds As New DataSet
        Try
            'SQL = "SELECT MWilayah.Nama AS Wilayah, MGudang.Nama AS Gudang, SUM((MKartuStok.QtyMasuk*MKartuStok.Konversi)-(MKartuStok.QtyKeluar*MKartuStok.Konversi)) AS SisaStok "
            'SQL &= " FROM MKartuStok "
            'SQL &= " LEFT JOIN (MGudang LEFT JOIN MWilayah ON MWilayah.NoID=MGudang.IDWilayah) ON MGudang.NoID=MKartuStok.IDGudang "
            'SQL &= " WHERE MKartuStok.IDBarang=" & NullTolong(txtBarang.EditValue)
            'SQL &= " GROUP BY MKartuStok.IDGudang, MWilayah.Nama, MGudang.Nama "

            'SQL = " SELECT *, IsNull(X.Stok,0)-IsNull(A.DiSPK,0) AS SisaStok FROM " & vbCrLf
            'SQL &= " (SELECT MBarang.NoID, MWilayah.Nama AS Wilayah, MGudang.Nama AS Gudang, MBarang.Nama AS Barang, SUM((MKartuStok.QtyMasuk*MKartuStok.Konversi)-(MKartuStok.QtyKeluar*MKartuStok.Konversi)) AS Stok" & vbCrLf
            'SQL &= " FROM MKartuStok " & vbCrLf
            'SQL &= " LEFT JOIN MGudang ON MGudang.NoID=MKartuStok.IDGudang" & vbCrLf
            'SQL &= " LEFT JOIN MWilayah ON MGudang.IDWilayah=MWilayah.NoID" & vbCrLf
            'SQL &= " LEFT JOIN MBarang ON MKartuStok.IDBarang=MBarang.NoID" & vbCrLf
            'SQL &= " WHERE MKartuStok.IDBarang=" & NullTolong(txtBarang.EditValue) & vbCrLf
            'SQL &= " GROUP BY MKartuStok.IDGudang, MGudang.Nama, MWilayah.Nama, MBarang.NoID, MBarang.Nama) X" & vbCrLf
            'SQL &= " LEFT JOIN (" & vbCrLf
            'SQL &= " SELECT MSPKD.IDBarang, MSPKD.NoID AS IDSKPD, SUM(MSPKD.Qty*MSPKD.Konversi) AS DiSPK " & vbCrLf
            'SQL &= " FROM MSPKD" & vbCrLf
            'SQL &= " INNER JOIN MSPK ON MSPK.NoID=MSPKD.IDSPK" & vbCrLf
            'SQL &= " WHERE MSPKD.IDBarang=" & NullTolong(txtBarang.EditValue) & vbCrLf
            'SQL &= " GROUP BY MSPKD.IDBarang, MSPKD.NoID" & vbCrLf
            'SQL &= " ) A ON X.NoID=A.IDBarang" & vbCrLf
            'SQL &= " LEFT JOIN (" & vbCrLf
            'SQL &= " SELECT MJualD.IDBarang, SUM(MJualD.Qty*MJualD.Konversi) AS DiJual " & vbCrLf
            'SQL &= " FROM MJualD INNER JOIN MJual ON MJual.NoID=MJualD.IDJual " & vbCrLf
            'SQL &= " WHERE MJualD.IDBarang=" & NullTolong(txtBarang.EditValue) & vbCrLf
            'SQL &= " GROUP BY MJualD.IDBarang) B ON A.IDSKPD=B.IDSKPD "

            'SQL = "SELECT MWilayah.Nama AS Wilayah, MGudang.Nama AS Gudang, MBarang.Kode AS KodeStock, MBarang.Nama AS NamaStock, SUM((MKartuStok.QtyMasuk*MkartuStok.Konversi)-(MKartuStok.QtyKeluar*MkartuStok.Konversi)) AS Stok, SUM((MKartuStok.QtyMasuk*MkartuStok.Konversi)-(MKartuStok.QtyKeluar*MkartuStok.Konversi))-IsNull(TSPK.DiSPK,0) AS QtySisa" & vbCrLf
            'SQL &= " FROM MKartuStok" & vbCrLf
            'SQL &= " LEFT JOIN (MGudang LEFT JOIN MWilayah ON MWilayah.NoID=MGudang.IDWilayah) ON MGudang.NoID=MKartuStok.IDGudang" & vbCrLf
            'SQL &= " LEFT JOIN MBarang ON MBarang.NoID=MKartuStok.IDBarang" & vbCrLf
            'SQL &= " Left Join" & vbCrLf
            'SQL &= " (SELECT X.IDBarang, X.IDGudang, SUM(X.DiSPK) AS DiSPK FROM" & vbCrLf
            'SQL &= " (SELECT MSPKD.IDBarang, MSPKD.IDGudang, (MSPKD.Qty*MSPKD.Konversi) AS SPK, " & vbCrLf
            'SQL &= " (MSPKD.Qty*MSPKD.Konversi)-IsNull(" & vbCrLf
            'SQL &= " (SELECT SUM(B.Qty*B.Konversi) FROM MJualD B WHERE B.IDSPKD<>" & NoID & " AND B.IDSPKD=MSPKD.NoID),0) AS DiSPK " & vbCrLf
            'SQL &= " FROM MSPKD WHERE MSPKD.NoID<>" & NoID & ") X" & vbCrLf
            'SQL &= " GROUP BY X.IDBarang, X.IDGudang) TSPK ON MKartuStok.IDBarang=TSPK.IDBarang AND MKartuStok.IDGudang=TSPK.IDGudang" & vbCrLf
            'SQL &= " WHERE MWilayah.NoID=" & DefIDWilayah & " AND MKartuStok.IDBarang=" & NullTolong(txtBarang.EditValue) & vbCrLf
            'SQL &= " GROUP BY TSPK.DiSPK, MWilayah.Nama, MGudang.Nama, MBarang.Nama, MBarang.Kode" & vbCrLf
            SQL = "SELECT MBarang.Nama AS NamaStock, MBarang.Kode AS KodeStock, MGudang.Nama AS Gudang, MWilayah.Nama AS Wilayah, X.IDGudang, X.IDBarang, SUM(X.Masuk) AS QtyStock, SUM(X.Masuk-X.Keluar) AS SisaQty FROM " & vbCrLf & _
                  " (SELECT MKartuStok.IDGudang, MKartuStok.IDBarang, (MKartuStok.QtyMasuk-MKartuStok.QtyKeluar)*MKartuStok.Konversi AS Masuk , 0 AS Keluar" & vbCrLf & _
                  " FROM MKartuStok LEFT JOIN MGudang ON MGudang.NoID=MKartuStok.IDGudang" & vbCrLf & _
                  " WHERE MGudang.IDWilayah = " & DefIDWilayah & " And MKartuStok.IDBarang = " & NullToLong(txtBarang.EditValue) & " " & vbCrLf & _
                  " UNION ALL  " & vbCrLf & _
                  " SELECT MSPKD.IDGudang, MSPKD.IDBarang, 0, ((MSPKD.Qty*MSPKD.Konversi)-IsNull((SELECT SUM(MPackingD.Qty*MPackingD.Konversi) FROM MPackingD INNER JOIN MPacking ON MPacking.NoID=MPackingD.IDPacking WHERE MPackingD.IDSPKD=MSPKD.NoID),0)) AS DiSPK " & vbCrLf & _
                  " FROM MSPKD INNER JOIN MSPK ON MSPK.NoID=MSPKD.IDSPK  " & vbCrLf & _
                  " WHERE MSPK.IDWilayah=" & DefIDWilayah & " AND MSPKD.IDBarang=" & NullToLong(txtBarang.EditValue) & " AND ((MSPKD.Qty*MSPKD.Konversi)-IsNull((SELECT SUM(MPackingD.Qty*MPackingD.Konversi) FROM MPackingD INNER JOIN MPacking ON MPacking.NoID=MPackingD.IDPacking WHERE MPackingD.IDSPKD=MSPKD.NoID),0))>0) X " & vbCrLf & _
                  " LEFT JOIN (MGudang LEFT JOIN MWilayah ON MWilayah.NoID=MGudang.IDWilayah) ON MGudang.NoID=X.IDGudang " & vbCrLf & _
                  " LEFT JOIN MBarang ON MBarang.NoID=X.IDBarang " & vbCrLf & _
                  " GROUP BY X.IDGUdang, X.IDBarang, MBarang.Nama, MBarang.Kode, MGudang.Nama, MWilayah.Nama"

            ds = ExecuteDataset("MStok", SQL)
            GridControl1.DataSource = ds.Tables(0)
            'If System.IO.File.Exists(folderLayouts &  Me.Name & GridView1.Name & ".xml") Then
            '    GridView1.RestoreLayoutFromXml(folderLayouts &  Me.Name & GridView1.Name & ".xml")
            'End If
            With GridView1
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
            SQL = "SELECT MBarangD.Konversi, MBarang.CtnPcs FROM MBarangD LEFT JOIN MBarang ON MBarang.NoID=MBarangD.IDBarang LEFT JOIN MSatuan ON MSatuan.NoID=MBarangD.IDSatuan WHERE MBarangD.IDBarang=" & NullTolong(txtBarang.EditValue) & " AND MSatuan.NoID=" & NullTolong(txtSatuan.EditValue)
            Ds = ExecuteDataset("Tabel", SQL)
            If Ds.Tables("Tabel").Rows.Count >= 1 Then
                txtKonversi.EditValue = NullToDbl(Ds.Tables(0).Rows(0).Item("Konversi"))
                IsiKarton = NullToDbl(Ds.Tables(0).Rows(0).Item("CtnPcs"))
                If IsiKarton = 0 Then
                    txtCtn.EditValue = 0
                Else
                    txtCtn.EditValue = txtQty.EditValue / IsiKarton * txtKonversi.EditValue
                End If
            End If
            If IsNew AndAlso txtSO.Text = "" Then
                'txtHarga.EditValue = clsPostingPenjualan.HargaJual(NullToLong(txtBarang.EditValue), NullToLong(txtSatuan.EditValue), IDCustomer, txtDiscPersen1.EditValue, txtDiscPersen2.EditValue)
            Else
                If txtSO.Text <> "" AndAlso IsNew = True Then
                    txtHarga.EditValue = HargaPcs * Konversi
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

    Private Sub gvSO_DataSourceChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvSO.DataSourceChanged
        If System.IO.File.Exists(folderLayouts & Me.Name & gvSO.Name & ".xml") Then
            gvSO.RestoreLayoutFromXml(folderLayouts & Me.Name & gvSO.Name & ".xml")
        End If
        With gvSO
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

    Private Sub txtSO_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtSO.EditValueChanged
        Dim Ds As New DataSet
        Try
            If IsNew Or IsFastEntri Then
                SQL = "SELECT MSOD.*, (MSOD.Qty*MSOD.Konversi)-" & DiSPK() & " AS QtySisa, MBarang.Kode AS KodeBarang, MBarang.Nama AS NamaBarang, MGudang.Kode AS KodeGudang, MGudang.Nama AS NamaGudang, MSatuan.Nama AS NamaSatuan "
                SQL &= " FROM ((MSOD LEFT JOIN MGudang ON MGudang.NoID=MSOD.IDGudang) "
                SQL &= " LEFT JOIN MBarang ON MBarang.NoID=MSOD.IDBarang)"
                SQL &= " LEFT JOIN MSatuan ON MSatuan.NoID=MSOD.IDSatuan "
                SQL &= " WHERE MSOD.NoID= " & NullToLong(txtSO.EditValue)
                Ds = New DataSet()
                Ds = ExecuteDataset("MPOD", SQL)
                If Ds.Tables(0).Rows.Count >= 1 Then
                    With Ds.Tables("MPOD").Rows(0)
                        'gvSO.SaveLayoutToXml(folderLayouts &  Me.Name & gvSO.Name & ".xml")
                        txtBarang.EditValue = NullToLong(.Item("IDBarang"))
                        'txtBarang.Text = NullTostr(.Item("KodeBarang"))
                        txtSatuan.EditValue = NullToLong(.Item("IDSatuan"))
                        'txtSatuan.Text = NullTostr(.Item("KodeSatuan"))
                        'txtGudang.EditValue = NullTolong(.Item("IDGudang"))
                        'txtGudang.Text = NullTostr(.Item("KodeGudang"))
                        QtySisaSO = NullToDbl(.Item("QtySisa"))
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

    Private Sub txtGudang_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtGudang.EditValueChanged
        RubahGudang()
    End Sub

    Private Sub RubahGudang()
        Dim Ds As New DataSet
        Try
            SQL = "SELECT MWilayah.Nama AS Wilayah FROM MGudang LEFT JOIN MWilayah ON MWilayah.NoID=MGudang.IDWilayah WHERE MGudang.NoID=" & NullTolong(txtGudang.EditValue)
            Ds = ExecuteDataset("Tabel", SQL)
            If Ds.Tables("Tabel").Rows.Count >= 1 Then
                txtWilayah.Text = NullTostr(Ds.Tables(0).Rows(0).Item("Wilayah"))
            End If
        Catch ex As Exception
            XtraMessageBox.Show("Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            Ds.Dispose()
        End Try
    End Sub

    Private Sub txtHarga_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtHarga.EditValueChanged
        HitungJumlah()
    End Sub

    Private Sub txtHarga_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtHarga.LostFocus
        HitungJumlah()
    End Sub

    Private Sub txtKonversi_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtKonversi.EditValueChanged

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

    Private Sub txtQty_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtQty.EditValueChanged
        HitungJumlah()
    End Sub

    Private Sub frmEntriSPKD_Shown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Shown
        Try
            txtSO.Focus()
        Catch ex As Exception

        End Try
    End Sub
End Class