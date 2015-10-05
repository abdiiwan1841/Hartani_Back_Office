Imports System.Data
Imports System.Data.SqlClient
Imports DevExpress.XtraEditors
Imports DevExpress.Utils
Imports System.IO.File

Public Class frmEntriReturBeliD
    Public IsNew As Boolean = True
    Public IsPosted As Boolean = False
    Public NoID As Long = -1
    Public IDReturBeli As Long = -1
    Public FormPemanggil As frmEntriReturBeli
    Public IDSupplier As Long = -1
    Public IDGudang As Long = -1

    Dim QtySisa As Double = 0.0
    Dim oDS As New DataSet
    Dim SQL As String = ""
    Dim DefImageList As ImageList
    Dim HargaPcs As Double = 0.0
    Dim Konversi As Double = 0.0
    Dim KonversiBeli As Double = 0.0
    Public IsFastEntri As Boolean = False
    Dim IsStockPerSupplier As Boolean = False

    Private Sub IsiDefault()
        DefImageList = frmMain.ImageList1
        cmdSave.ImageList = DefImageList
        cmdSave.ImageIndex = 6

        cmdTutup.ImageList = DefImageList
        cmdTutup.ImageIndex = 3
        RefreshLookUp()
        Konversi = 1.0
        HargaPcs = 0.0
        QtySisa = 0.0
        txtGudang.EditValue = IDGudang
        RubahGudang()
    End Sub
    Private Function DiRetur() As String
        Return "isnull((SELECT SUM(isnull(A.Qty,0)*isnull(A.Konversi,0)) FROM MReturBeliD A WHERE A.NoID <> " & NoID & " AND A.IDBeliD=MBeliD.NoID),0)"
    End Function
    Private Sub RefreshLookUp()
        RefreshBarcode(False)
        Dim ds As New DataSet
        Try
            IsStockPerSupplier = NullToBool(EksekusiSQlSkalarNew("SELECT MSetting.IsStockReturPerSupplier FROM MSetting"))

            SQL = "SELECT MBeliD.NoID, MWilayah.Nama AS Wilayah, MGudang.Nama AS Gudang, MBeli.Kode AS KodeBeli, MBarang.Kode AS KodeStock, MBeliD.Qty AS QtyBeli, MSatuan.Nama AS SatuanBeli, "
            SQL &= " (MBeliD.Jumlah/isnull(MBeliD.Konversi,1)/isnull(MBeliD.Qty,1))-(IsNull(MBeli.DiskonNotaProsen,0)/100*(MBeliD.Jumlah/isnull(MBeliD.Konversi,1)/isnull(MBeliD.Qty,1)))-IsNull((SELECT SUM(MRevisiHargaBeliD.Koreksi*MRevisiHargaBeliD.Konversi) FROM MRevisiHargaBeliD LEFT JOIN MRevisiHargaBeli ON MRevisiHargaBeli.NoID=MRevisiHargaBeliD.IDRevisiHargaBeli WHERE MRevisiHargaBeliD.IDBeli=MBeli.NoID AND MRevisiHargaBeliD.IDBeliD=MBeliD.NoID AND MRevisiHargaBeli.IsPosted=1),0) AS [Harga (Pcs)],"
            SQL &= " (MBeliD.Qty*MBeliD.Konversi)-isnull(" & DiRetur() & ",0) AS [Qty Sisa (Pcs)] "
            SQL &= " FROM MBeliD "
            SQL &= " LEFT JOIN (MGudang LEFT JOIN MWilayah ON MWilayah.NoID=MGudang.IDWilayah) ON MGudang.NoID=MBeliD.IDGudang "
            SQL &= " LEFT JOIN MBeli ON MBeli.NoID=MBeliD.IDBeli LEFT JOIN MBarang ON MBarang.NoID=MBeliD.IDBarang LEFT JOIN MSatuan ON MSatuan.NoID=MBeliD.IDSatuan "
            SQL &= " WHERE MWilayah.NoID=" & DefIDWilayah & " AND (MBeliD.Qty*MBeliD.Konversi)-isnull(" & DiRetur() & ",0)>0 AND MBarang.IsActive=1 AND MBeli.IsPosted=1 AND MBeli.IDSupplier=" & IDSupplier
            ds = ExecuteDataset("MBeliD", SQL)
            txtBeli.Properties.DataSource = ds.Tables("MBeliD")
            txtBeli.Properties.ValueMember = "NoID"
            txtBeli.Properties.DisplayMember = "KodeBeli"

            SQL = "SELECT MBarang.NoID, MBarang.Kode, MBarang.Barcode, MBarang.Nama, MSatuan.Nama AS Satuan FROM MBarang LEFT JOIN MSatuan ON MSatuan.NoID=MBarang.IDSatuan WHERE MBarang.IsActive=1 AND NOT (MBarang.KODE='' OR MBarang.KODE IS NULL)" & vbCrLf & _
                  IIf(IsStockPerSupplier, " AND (MBarang.IDSupplier1=" & NullToLong(IDSupplier) & " OR MBarang.IDSupplier2=" & NullToLong(IDSupplier) & " OR MBarang.IDSupplier3=" & NullToLong(IDSupplier) & " OR MBarang.IDSupplier4=" & NullToLong(IDSupplier) & " OR MBarang.IDSupplier5=" & NullToLong(IDSupplier) & ")", "")
            ds = ExecuteDataset("MBarang", SQL)
            txtBarang.Properties.DataSource = ds.Tables("MBarang")
            txtBarang.Properties.ValueMember = "NoID"
            txtBarang.Properties.DisplayMember = "Kode"

            QtyGudang()
        Catch ex As Exception
            XtraMessageBox.Show("Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            ds.Dispose()
        End Try
    End Sub
    Sub RefreshBarcode(ByVal IsForce As Boolean)
        Dim dsPublic As New DataSet
        'If IsForce Then
        '    If DSPublic.Tables("MBarangD") Is Nothing Then 'blm ada data
        '        SQL = "SELECT MBarangD.Barcode,MBarang.Kode,MBarang.Nama,MSatuan.Kode as Satuan,MBarangD.NoID,MBarangD.IDSatuan,MBarangD.IDBarang from MbarangD inner join MBarang On MBarangD.IDBarang=MBarang.NoID inner join MSatuan On MBarangD.IDSatuan=MSatuan.NoID"
        '        DSPublic = ExecuteDataset("MBarangD", SQL)
        '    Else
        '        SQL = "SELECT MBarangD.NoID,MBarangD.Barcode,MBarang.Kode,MBarang.Nama,MSatuan.Kode as Satuan,MBarangD.IDSatuan,MBarangD.IDBarang from MbarangD inner join MBarang On MBarangD.IDBarang=MBarang.NoID inner join MSatuan On MBarangD.IDSatuan=MSatuan.NoID"
        '        DSPublic = ExecuteDataset("MBarangD", SQL)

        '    End If
        'Else
        '    If Not DSPublic Is Nothing AndAlso Not DSPublic.Tables("MBarangD") Is Nothing Then 'sdh ada data
        '        If DSPublic.Tables("MBarangD").Rows.Count = 0 Then
        '            SQL = "SELECT MBarangD.NoID,MBarangD.Barcode,MBarang.Kode,MBarang.Nama,MSatuan.Kode as Satuan,MBarangD.IDSatuan,MBarangD.IDBarang from MbarangD inner join MBarang On MBarangD.IDBarang=MBarang.NoID inner join MSatuan On MBarangD.IDSatuan=MSatuan.NoID"
        '            DSPublic = ExecuteDataset("MBarangD", SQL)
        '        End If
        '    Else
        '        SQL = "SELECT MBarangD.NoID,MBarangD.Barcode,MBarang.Kode,MBarang.Nama,MSatuan.Kode as Satuan,MBarangD.IDSatuan,MBarangD.IDBarang from MbarangD inner join MBarang On MBarangD.IDBarang=MBarang.NoID inner join MSatuan On MBarangD.IDSatuan=MSatuan.NoID"
        '        DSPublic = ExecuteDataset("MBarangD", SQL)
        '    End If
        'End If
        IsStockPerSupplier = NullToBool(EksekusiSQlSkalarNew("SELECT MSetting.IsStockReturPerSupplier FROM MSetting"))

        SQL = "SELECT MBarangD.NoID,MBarangD.Barcode,MBarang.Kode,MBarang.Nama + ' ' + ISNULL(MBarangD.Varian,'') AS Nama,MMerk.Nama as Merk, MSatuan.Kode as Satuan,MBarangD.IDSatuan,MBarangD.IDBarang FROM MBarangD inner join MBarang On MBarangD.IDBarang=MBarang.NoID inner join MSatuan On MBarangD.IDSatuan=MSatuan.NoID left join mmerk on MBarang.IDMerk=MMerk.NoID WHERE MBarang.IsActive=1 AND MBarangD.IsActive=1" & vbCrLf & _
              IIf(IsStockPerSupplier, " AND (MBarang.IDSupplier1=" & NullToLong(IDSupplier) & " OR MBarang.IDSupplier2=" & NullToLong(IDSupplier) & " OR MBarang.IDSupplier3=" & NullToLong(IDSupplier) & " OR MBarang.IDSupplier4=" & NullToLong(IDSupplier) & " OR MBarang.IDSupplier5=" & NullToLong(IDSupplier) & ")", "")
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
    Private Sub RefreshLookUpBeli()
        Dim ds As New DataSet
        Try
            SQL = "SELECT MBeliD.NoID, MWilayah.Nama AS Wilayah, MGudang.Nama AS Gudang, MBeli.Kode AS KodeBeli, MBarang.Kode AS KodeStock, MBeliD.Qty AS QtyBeli, MSatuan.Nama AS SatuanBeli, " & _
                  " (MBeliD.HargaNetto/isnull(MBeliD.Konversi,1))-(MBeliD.HargaNetto/isnull(MBeliD.Konversi,1)*IsNull(MBeli.DiskonNotaProsen,0)/100) AS [Harga (Pcs)], IsNull((SELECT TOP 1 MRevisiHargaBeliD.KoreksiBL*MRevisiHargaBeliD.Konversi FROM MRevisiHargaBeliD INNER JOIN MRevisiHargaBeli ON MRevisiHargaBeli.NoID=MRevisiHargaBeliD.IDRevisiHargaBeli WHERE MRevisiHargaBeliD.IDBeli=MBeli.NoID AND MRevisiHargaBeliD.IDBeliD=MBeliD.NoID AND MRevisiHargaBeli.IsPosted=1 ORDER BY MRevisiHargaBeli.Tanggal DESC),0) AS [TotalPH], " & _
                  " (MBeliD.Qty*MBeliD.Konversi)-isnull(" & DiRetur() & ",0) AS [Qty Sisa (Pcs)] " & _
                  " FROM MBeliD " & _
                  " LEFT JOIN (MGudang LEFT JOIN MWilayah ON MWilayah.NoID=MGudang.IDWilayah) ON MGudang.NoID=MBeliD.IDGudang " & _
                  " LEFT JOIN MBeli ON MBeli.NoID=MBeliD.IDBeli LEFT JOIN MBarang ON MBarang.NoID=MBeliD.IDBarang LEFT JOIN MSatuan ON MSatuan.NoID=MBeliD.IDSatuan " & _
                  " WHERE MWilayah.NoID=" & DefIDWilayah & " AND (MBeliD.Qty*MBeliD.Konversi)-isnull(" & DiRetur() & ",0)>0 AND MBarang.NoID=" & NullToLong(txtBarang.EditValue) & " AND MBarang.IsActive=1 AND MBeli.IsPosted=1 AND MBeli.IDSupplier=" & IDSupplier
            ds = ExecuteDataset("MBeliD", SQL)
            txtBeli.Properties.DataSource = ds.Tables("MBeliD")
            txtBeli.Properties.ValueMember = "NoID"
            txtBeli.Properties.DisplayMember = "KodeBeli"
            txtBeli.EditValue = EksekusiSQlSkalarNew("SELECT max(MBeliD.NoID) " & _
                 " FROM MBeliD " & _
                 " LEFT JOIN MBeli ON MBeli.NoID=MBeliD.IDBeli LEFT JOIN MBarang ON MBarang.NoID=MBeliD.IDBarang LEFT JOIN MSatuan ON MSatuan.NoID=MBeliD.IDSatuan " & _
                 " WHERE (MBeliD.Qty*MBeliD.Konversi)-isnull(" & DiRetur() & ",0)>0 AND MBarang.NoID=" & NullToLong(txtBarang.EditValue) & " AND  MBeli.IsPosted=1 AND MBeli.IDSupplier=" & IDSupplier)

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
    Private Sub QtyGudang()
        Dim ds As New DataSet
        Try
            SQL = "SELECT MGudang.NoID, MGudang.Kode, MGudang.Kode + ' - ' + MGudang.Nama AS Nama, MWilayah.Nama AS Wilayah FROM MGudang LEFT JOIN MWilayah ON MGudang.IDWilayah=MWilayah.NoID WHERE MGudang.IsActive=1 AND MGudang.InTransit=0 AND MGudang.IDWilayah=" & DefIDWilayah
            If NullTobool(EksekusiSQlSkalarNew("SELECT IsGudangBSDiRetur FROM MSetting")) Then
                SQL &= " AND MGudang.IsBS=1 "
            End If
            ds = ExecuteDataset("MGudang", SQL)
            txtGudang.Properties.DataSource = ds.Tables("MGudang")
            txtGudang.Properties.ValueMember = "NoID"
            txtGudang.Properties.DisplayMember = "Nama"
            
        Catch ex As Exception
            XtraMessageBox.Show("Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            ds.Dispose()
        End Try
    End Sub
    Private Sub LoadData()
        Try
            SQL = "SELECT MReturBeliD.*, MBeli.DiskonNotaProsen, MBeliD.Konversi AS KonversiAsal, (MBeliD.Harga/MBeliD.Konversi) AS HargaAsal, (MBeliD.Qty*MBeliD.Konversi) AS QtyAsal, MBarang.Kode AS KodeBarang, MBarang.Nama AS NamaBarang, MGudang.Kode AS KodeGudang, MGudang.Nama AS NamaGudang, MSatuan.Nama AS NamaSatuan "
            SQL &= " FROM (((MReturBeliD LEFT JOIN MGudang ON MGudang.NoID=MReturBeliD.IDGudang) "
            SQL &= " LEFT JOIN MBarang ON MBarang.NoID=MReturBeliD.IDBarang)"
            SQL &= " LEFT JOIN MSatuan ON MSatuan.NoID=MReturBeliD.IDSatuan) "
            SQL &= " LEFT JOIN (MBeliD LEFT JOIN MBeli ON MBeli.NoID=MBeliD.IDBeli) ON MBeliD.NoID=MReturBeliD.IDBeliD "
            SQL &= " WHERE MReturBeliD.NoID= " & NoID
            oDS = New DataSet()
            oDS = ExecuteDataset("MReturBeliD", SQL)
            If oDS.Tables(0).Rows.Count >= 1 Then
                With oDS.Tables("MReturBeliD").Rows(0)
                    NoID = NullTolong(.Item("NoID"))
                    IDReturBeli = NullTolong(.Item("IDReturBeli"))
                    txtBeli.EditValue = NullToLong(.Item("IDBeliD"))
                    txtBarcode.EditValue = NullToLong(.Item("IDBarangD"))
                    'txtBeli.Text = NullTostr(.Item("KodeBarang"))
                    txtBarang.EditValue = NullToLong(.Item("IDBarang"))
                    If NullToStr(.Item("NamaStock")) <> "" Then
                        txtNamaStock.EditValue = NullToStr(.Item("NamaStock"))
                    End If
                    'txtBarang.Text = NullTostr(.Item("KodeBarang"))
                    txtSatuan.EditValue = NullToLong(.Item("IDSatuan"))
                    'txtSatuan.Text = NullTostr(.Item("KodeSatuan"))
                    txtGudang.EditValue = IDGudang
                    'txtGudang.Text = NullTostr(.Item("KodeGudang"))
                    txtQty.EditValue = NullToDbl(.Item("Qty"))
                    HargaPcs = NullToDbl(.Item("HargaAsal"))
                    txtHargaJual.EditValue = NullToDbl(.Item("HJual"))
                    txtHargaBeli.EditValue = NullToDbl(.Item("HBeli"))
                    KonversiBeli = NullToDbl(.Item("KonversiAsal"))
                    QtySisa = NullToDbl(EksekusiSQlSkalarNew("Select ((MBeliD.Qty*MBeliD.Konversi)-" & DiRetur() & ") AS QtySisa FROM MBeliD WHERE NoID=" & NullToLong(.Item("IDBeliD"))))
                    txtHarga.EditValue = NullToDbl(.Item("Harga"))
                    txtCtn.EditValue = NullToDbl(.Item("CTN"))
                    txtDiscPersen1.EditValue = NullToDbl(.Item("DiscPersen1"))
                    txtDiscPersen2.EditValue = NullToDbl(.Item("DiscPersen2"))
                    txtDiscPersen3.EditValue = NullToDbl(.Item("DiscPersen3"))
                    txtDiscRp1.EditValue = NullToDbl(.Item("Disc1"))
                    txtDiscRp2.EditValue = NullToDbl(.Item("Disc2"))
                    txtDiscRp3.EditValue = NullToDbl(.Item("Disc3"))
                    'txtDiscPOBawah.EditValue = NullToDbl(.Item("DiskonNotaProsen"))
                    txtJUmlah.EditValue = NullToDbl(.Item("Jumlah"))
                    txtCatatan.Text = NullToStr(.Item("Catatan"))
                    txtKonversi.EditValue = NullToDbl(.Item("Konversi"))
                    Konversi = txtKonversi.EditValue
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
                SQL = "INSERT INTO MReturBeliD (NoID,IDReturBeli,IDBarangD,NoUrut,Tgl,Jam,IDBeliD,IDBarang,IDSatuan,Qty,QtyPcs,Harga,HargaPcs,CTN,DiscPersen1,DiscPersen2,DiscPersen3,Disc1,Disc2,Disc3,Jumlah,Catatan,IDGudang,Konversi,HBeli,HJual,NamaStock) VALUES ("
                SQL &= NullTolong(GetNewID("MReturBeliD", "NoID")) & ","
                SQL &= IDReturBeli & ","
                SQL &= NullToLong(txtBarcode.EditValue) & ","
                SQL &= GetNewID("MReturBeliD", "NoUrut", " WHERE IDReturBeli=" & IDReturBeli) & ","
                SQL &= "GetDate(),"
                SQL &= "GetDate(),"
                SQL &= NullTolong(txtBeli.EditValue) & ","
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
                SQL &= FixKoma(txtHargaBeli.EditValue) & ","
                SQL &= FixKoma(txtHargaJual.EditValue) & ","
                SQL &= "'" & FixApostropi(txtNamaStock.Text) & "' "
                SQL &= ")"
            Else
                SQL = "UPDATE MReturBeliD SET "
                SQL &= " Tgl=GetDate(),"
                SQL &= " Jam=GetDate(),"
                SQL &= " IDReturBeli=" & IDReturBeli & ","
                SQL &= " IDBeliD=" & NullTolong(txtBeli.EditValue) & ","
                SQL &= " IDBarangD=" & NullToLong(txtBarcode.EditValue) & ","
                SQL &= " IDBarang=" & NullToLong(txtBarang.EditValue) & ","
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
                SQL &= " IDGudang=" & NullToLong(txtGudang.EditValue) & ","
                SQL &= " HBeli=" & FixKoma(txtHargaBeli.EditValue) & ","
                SQL &= " HJual=" & FixKoma(txtHargaJual.EditValue) & ","
                SQL &= " NamaStock='" & FixApostropi(txtNamaStock.Text) & "',"
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
        If txtBeli.Text = "" Then
            If XtraMessageBox.Show("Yakin melakukan retur tanpa adanya pembelian ?", NamaAplikasi, MessageBoxButtons.YesNo, MessageBoxIcon.Warning) = Windows.Forms.DialogResult.No Then
                txtBeli.Focus()
                Return False
                Exit Function
            End If
        End If
        If txtBeli.Text <> "" Then
            QtySisa = NullToDbl(EksekusiSQlSkalarNew("Select ((MBeliD.Qty*MBeliD.Konversi)-" & DiRetur() & ") AS QtySisa FROM MBeliD WHERE NoID=" & NullToLong(txtBeli.EditValue)))
            If QtySisa < (txtQty.EditValue * txtKonversi.EditValue) Then
                Dim Jwb As DialogResult
                Jwb = XtraMessageBox.Show("Qty melebihi standart Pembelian." & vbCrLf & "Mau Pakasa Lanjut?", NamaAplikasi, MessageBoxButtons.YesNo, MessageBoxIcon.Information)
                If Jwb = Windows.Forms.DialogResult.No Then
                    txtQty.Focus()
                    Return False
                    Exit Function
                Else
                    Return True
                End If
                
            End If
        End If
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
        'If SisaQty() < txtQty.EditValue * txtKonversi.EditValue Then
        '    XtraMessageBox.Show("Qty melebihi stok Gudang " & txtGudang.Text & " .", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
        '    txtQty.Focus()
        '    Return False
        '    Exit Function
        'End If
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
        If txtDiscPersen1.EditValue < 0 Then
            XtraMessageBox.Show("Disc persen 1 tidak boleh kurang dari 0.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
            txtDiscPersen1.Focus()
            Return False
            Exit Function
        End If
        If txtDiscPersen2.EditValue < 0 Then
            XtraMessageBox.Show("Disc persen 1 tidak boleh kurang dari 0.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
            txtDiscPersen2.Focus()
            Return False
            Exit Function
        End If
        If txtDiscPersen3.EditValue < 0 Then
            XtraMessageBox.Show("Disc persen 1 tidak boleh kurang dari 0.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
            txtDiscPersen2.Focus()
            Return False
            Exit Function
        End If
        If txtDiscRp1.EditValue < 0 Then
            XtraMessageBox.Show("Disc rupiah 1 tidak boleh kurang dari 0.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
            txtDiscRp1.Focus()
            Return False
            Exit Function
        End If
        If txtDiscRp2.EditValue < 0 Then
            XtraMessageBox.Show("Disc rupiah 2 tidak boleh kurang dari 0.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
            txtDiscRp2.Focus()
            Return False
            Exit Function
        End If
        If txtDiscRp3.EditValue < 0 Then
            XtraMessageBox.Show("Disc rupiah 3 tidak boleh kurang dari 0.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
            txtDiscRp3.Focus()
            Return False
            Exit Function
        End If
        If txtHarga.EditValue <= 0 Then
            If txtHarga.EditValue = 0 AndAlso XtraMessageBox.Show("Harga masih kurang dari atau sama dengan 0, lanjutkan penyimpanan?", NamaAplikasi, MessageBoxButtons.YesNo, MessageBoxIcon.Information, MessageBoxDefaultButton.Button2) = Windows.Forms.DialogResult.No Then
                txtHarga.Focus()
                Return False
                Exit Function
            ElseIf txtHarga.EditValue < 0 Then
                txtHarga.Focus()
                Return False
                Exit Function
            End If
        End If
        Return True
    End Function
    Private Function SisaQty() As Double
        Dim ds As New DataSet
        Try
            SQL = "SELECT SUM((isnull(QtyMasuk,0)*isnull(Konversi,0))-(isnull(QtyKeluar,0)*isnull(Konversi,0))) AS QtySisa FROM MKartuStok WHERE IDBarang=" & NullTolong(txtBarang.EditValue) & " AND IDGudang=" & NullTolong(txtGudang.EditValue) & " AND (IsSPK=0 OR IsSPK Is NULL)"
            ds = ExecuteDataset("Qty", SQL)
            If ds.Tables(0).Rows.Count >= 1 Then
                Return NullToDbl(ds.Tables(0).Rows(0).Item("QtySisa"))
            Else
                Return 0
            End If
        Catch ex As Exception
            XtraMessageBox.Show("Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return 0
        Finally
            oDS.Dispose()
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

                If FormPemanggil.IsRevisi Then
                    LayoutControl1.SaveLayoutToXml(FolderLayouts & Me.Name & LayoutControl1.Name & FormPemanggil.IsRevisi & ".xml")
                Else
                    LayoutControl1.SaveLayoutToXml(FolderLayouts & Me.Name & LayoutControl1.Name & ".xml")
                End If
                gvBarcode.SaveLayoutToXml(FolderLayouts & Me.Name & gvBarcode.Name & ".xml")
                gvBarang.SaveLayoutToXml(folderLayouts & Me.Name & gvBarang.Name & ".xml")
                gvGudang.SaveLayoutToXml(folderLayouts & Me.Name & gvGudang.Name & ".xml")
                gvSatuan.SaveLayoutToXml(folderLayouts & Me.Name & gvSatuan.Name & ".xml")
                gvBeli.SaveLayoutToXml(folderLayouts & Me.Name & gvBeli.Name & ".xml")
            End If
        Catch ex As Exception
            XtraMessageBox.Show("Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            xOtorisasi.Dispose()
        End Try
    End Sub
    Private Sub LoadLayout()
        'If Exists(folderLayouts &  Me.Name & gvBarang.Name & ".xml") Then
        '    gvBarang.RestoreLayoutFromXml(folderLayouts &  Me.Name & gvBarang.Name & ".xml")
        'End If
        'If Exists(folderLayouts &  Me.Name & gvGudang.Name & ".xml") Then
        '    gvGudang.RestoreLayoutFromXml(folderLayouts &  Me.Name & gvGudang.Name & ".xml")
        'End If
        'If Exists(folderLayouts &  Me.Name & gvSatuan.Name & ".xml") Then
        '    gvSatuan.RestoreLayoutFromXml(folderLayouts &  Me.Name & gvSatuan.Name & ".xml")
        'End If
        'If Exists(folderLayouts &  Me.Name & gvBeli.Name & ".xml") Then
        '    gvBeli.RestoreLayoutFromXml(folderLayouts &  Me.Name & gvBeli.Name & ".xml")
        'End If
        If FormPemanggil.IsRevisi Then
            If Exists(FolderLayouts & Me.Name & LayoutControl1.Name & FormPemanggil.IsRevisi.ToString & ".xml") Then
                LayoutControl1.RestoreLayoutFromXml(FolderLayouts & Me.Name & LayoutControl1.Name & FormPemanggil.IsRevisi.ToString & ".xml")
            Else
                If Exists(FolderLayouts & Me.Name & LayoutControl1.Name & ".xml") Then
                    LayoutControl1.RestoreLayoutFromXml(FolderLayouts & Me.Name & LayoutControl1.Name & ".xml")
                End If
            End If
        Else
            If Exists(FolderLayouts & Me.Name & LayoutControl1.Name & ".xml") Then
                LayoutControl1.RestoreLayoutFromXml(FolderLayouts & Me.Name & LayoutControl1.Name & ".xml")
            End If
        End If
    End Sub

    Private Sub frmEntriReturBeliD_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        Try
            If DialogResult = Windows.Forms.DialogResult.OK Then
                FormPemanggil.RefreshDetil()
                FormPemanggil.Show()
                FormPemanggil.Focus()
                FormPemanggil.SimpanTambahan()
                FormPemanggil.txtBarang.Focus()
            Else
                If IsFastEntri Then
                    EksekusiSQL("DELETE FROM MReturBeliD WHERE NoID=" & NoID)
                End If
                FormPemanggil.Show()
                FormPemanggil.Focus()
            End If
            Me.Dispose()
        Catch ex As Exception

        End Try
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
            If Not IsNew Then
                LoadData()
            End If
            Me.Width = Ini.BacaIniPath(folderLayouts & Me.Name & ".ini", "Form", "Width", Me.Width)
            Me.Height = Ini.BacaIniPath(folderLayouts & Me.Name & ".ini", "Form", "Height", Me.Height)

            Me.Left = (Screen.PrimaryScreen.WorkingArea.Width - Me.Width) \ 2
            Me.Top = (Screen.PrimaryScreen.WorkingArea.Height - Me.Height) \ 2
            LoadLayout()
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
    Private Sub HitungJumlah()
        Dim ds As New DataSet
        Dim IsiKarton As Double = 0.0
        Try
            'Dim Harga As Double = 0.0
            'If txtBeli.Text <> "" Then
            '    Harga = IIf(HargaPcs = 0, txtHarga.EditValue, HargaPcs) * IIf(txtKonversi.EditValue = 0, 1, txtKonversi.EditValue)
            'Else
            '    Harga = txtHarga.EditValue
            'End If
            'Dim SubTotal As Double = (Harga - txtDiscRp1.EditValue - txtDiscRp2.EditValue - txtDiscRp3.EditValue)
            'Dim DiscA As Double = Harga * txtQty.EditValue * txtDiscPersen1.EditValue / 100
            'Dim DiscB As Double = Harga * txtQty.EditValue * txtDiscPersen2.EditValue / 100
            'Dim DiscC As Double = Harga * txtQty.EditValue * txtDiscPersen3.EditValue / 100
            'txtHarga.EditValue = Harga
            'txtJUmlah.EditValue = (SubTotal * txtQty.EditValue) - DiscA - DiscB - DiscC
            '            txtJUmlah.EditValue = txtQty.EditValue * (Bulatkan((Harga * (1 - (txtDiscPersen1.EditValue / 100)) * (1 - (txtDiscPersen2.EditValue / 100)) * (1 - (txtDiscPersen3.EditValue / 100))), 0) - txtDiscRp1.EditValue - txtDiscRp2.EditValue - txtDiscRp3.EditValue)
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
            End If
            If FormPemanggil.IsRevisi Then
                txtHarga.EditValue = txtHargaBeli.EditValue
            End If
            txtJUmlah.EditValue = Bulatkan(txtQty.EditValue * ((txtHarga.EditValue * (1 - (txtDiscPersen1.EditValue / 100)) * (1 - (txtDiscPersen2.EditValue / 100)) * (1 - (txtDiscPersen3.EditValue / 100))) - txtDiscRp1.EditValue - txtDiscRp2.EditValue - txtDiscRp3.EditValue), 0)
            'txtJUmlah.EditValue = txtJUmlah.EditValue - (txtJUmlah.EditValue * txtDiscPOBawah.EditValue / 100)
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
    Private Sub txtDiscPersen1_EditValueChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtDiscPersen1.EditValueChanged
        HitungJumlah()
    End Sub
    Private Sub txtDiscPersen2_EditValueChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtDiscPersen2.EditValueChanged
        HitungJumlah()
    End Sub
    Private Sub txtDiscPersen3_EditValueChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtDiscPersen3.EditValueChanged
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
            SQL = "SELECT MBarang.Nama AS NamaBarang FROM MBarang WHERE MBarang.NoID=" & NullTolong(txtBarang.EditValue)
            Ds = ExecuteDataset("Tabel", SQL)
            If Ds.Tables("Tabel").Rows.Count >= 1 Then
                'txtSatuan.EditValue = NullTolong(Ds.Tables(0).Rows(0).Item("IDSatuan"))
                txtNamaStock.Text = NullTostr(Ds.Tables(0).Rows(0).Item("NamaBarang"))
                'txtKonversi.EditValue = NullToDbl(Ds.Tables(0).Rows(0).Item("Konversi"))
                RefreshLookUpSatuan()
                RefreshLookUpBeli()
                If IsNew Then
                    txtSatuan.EditValue = DefIDSatuan
                End If
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
        Dim IsiKarton As Double = 0.0
        Dim strHargaBeli As String = Ini.BacaIni(Me.Name, "TampilkanHargaBeli", " MBarang.HargaBeliPcs ")
        Dim strHargaJual As String = Ini.BacaIni(Me.Name, "TampilkanHargaJual", " MBarangD.HargaNetto ")
        Try
            If Not FormPemanggil Is Nothing AndAlso Not FormPemanggil.IsRevisi Then
                SQL = "SELECT MBarangD.Konversi, MBarang.CtnPcs, " & strHargaBeli & " AS HargaBeli, " & strHargaJual & " AS HargaJual " & _
                      " FROM MBarangD LEFT JOIN MBarang ON MBarang.NoID=MBarangD.IDBarang LEFT JOIN MSatuan ON MSatuan.NoID=MBarangD.IDSatuan WHERE MBarangD.NoID=" & NullToLong(txtBarcode.EditValue) & " AND MSatuan.NoID=" & NullToLong(txtSatuan.EditValue)
                Ds = ExecuteDataset("Tabel", SQL)
                If Ds.Tables("Tabel").Rows.Count >= 1 Then
                    txtKonversi.EditValue = NullToDbl(Ds.Tables(0).Rows(0).Item("Konversi"))
                    IsiKarton = NullToDbl(Ds.Tables(0).Rows(0).Item("CtnPcs"))
                    Konversi = txtKonversi.EditValue
                    If IsiKarton = 0 Then
                        txtCtn.EditValue = 0
                    Else
                        txtCtn.EditValue = txtQty.EditValue / IsiKarton * txtKonversi.EditValue
                    End If
                    If txtBeli.Text <> "" AndAlso (IsNew Or IsFastEntri) Then
                        txtHarga.EditValue = HargaPcs * Konversi
                    ElseIf txtHarga.EditValue = 0 Or IsNew Or IsFastEntri Then
                        'HargaBeliPcsBruto
                        'HargaBeliPcsBruto
                        'txtHarga.EditValue = NullToDbl(EksekusiSQlSkalarNew("SELECT TOP 1 MBeliD.Harga/MBeliD.Konversi FROM MBeliD INNER JOIN MBeli ON MBeli.NoID=MBeliD.IDBeli WHERE IsNull(MBeliD.Harga,0)<>0 AND MBeli.IDSupplier=" & IDSupplier & " AND MBeliD.IDBarang=" & NullToLong(txtBarang.EditValue) & " Order By MBeli.Tanggal Desc, MBeliD.Tgl Desc")) * IIf(txtKonversi.EditValue = 0, 1, txtKonversi.EditValue)
                        txtHarga.EditValue = NullToDbl(EksekusiSQlSkalarNew("SELECT TOP 1 MBeliD.HargaPcs FROM MBeliD INNER JOIN MBeli ON MBeli.NoID=MBeliD.IDBeli WHERE MBeliD.IDBarang=" & NullToLong(txtBarang.EditValue) & " Order By MBeli.Tanggal Desc, MBeliD.Tgl Desc")) * txtKonversi.EditValue
                    End If

                    txtHargaBeli.EditValue = NullToDbl(Ds.Tables(0).Rows(0).Item("HargaBeli")) * NullToDbl(txtKonversi.EditValue)
                    'If txtHargaBeli.EditValue > 0 AndAlso NullToLong(txtBeli.EditValue) <= 0 Then
                    txtHarga.EditValue = txtHargaBeli.EditValue
                    txtDiscPersen1.EditValue = 0
                    txtDiscPersen2.EditValue = 0
                    txtDiscPersen3.EditValue = 0
                    txtDiscRp1.EditValue = 0
                    txtDiscRp2.EditValue = 0
                    txtDiscRp3.EditValue = 0
                    'End If
                    txtHargaJual.EditValue = NullToDbl(Ds.Tables(0).Rows(0).Item("HargaJual"))
                    Ini.TulisIni(Me.Name, "TampilkanHargaBeli", strHargaBeli.ToString.ToUpper)
                    Ini.TulisIni(Me.Name, "TampilkanHargaJual", strHargaJual.ToString.ToUpper)
                End If
                txtHargaBeli.Properties.ReadOnly = True
                txtHargaJual.Properties.ReadOnly = True
            Else
                txtHargaBeli.Properties.ReadOnly = False
                txtHargaJual.Properties.ReadOnly = False
            End If
            HitungJumlah()
        Catch ex As Exception
            XtraMessageBox.Show("Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            Ds.Dispose()
        End Try
    End Sub

    'Private Sub txtBeli_ButtonClick(ByVal sender As Object, ByVal e As DevExpress.XtraEditors.Controls.ButtonPressedEventArgs) Handles txtBeli.ButtonClick
    '    With gvBeli
    '        For x As Integer = 0 To .Columns.Count - 1
    '            Select Case .Columns(x).ColumnType.Name.ToLower
    '                Case "int32", "int64", "int"
    '                    .Columns(x).DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
    '                    .Columns(x).DisplayFormat.FormatString = "n0"
    '                Case "decimal", "single", "money", "double"
    '                    .Columns(x).DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
    '                    .Columns(x).DisplayFormat.FormatString = "n2"
    '                Case "string"
    '                    .Columns(x).DisplayFormat.FormatType = DevExpress.Utils.FormatType.None
    '                    .Columns(x).DisplayFormat.FormatString = ""
    '                Case "date", "datetime"
    '                    If .Columns(x).FieldName.Trim.ToLower = "jam" Then
    '                        .Columns(x).DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime
    '                        .Columns(x).DisplayFormat.FormatString = "HH:mm"
    '                    Else
    '                        .Columns(x).DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime
    '                        .Columns(x).DisplayFormat.FormatString = "dd-MM-yyyy"
    '                    End If
    '            End Select
    '        Next
    '    End With
    'End Sub

    Private Sub txtBeli_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtBeli.EditValueChanged
        Dim Ds As New DataSet
        Try
            If IsNew Or IsFastEntri Then
                SQL = "SELECT MBeliD.*, MBeli.DiskonNotaProsen, (MBeliD.Qty*MBeliD.Konversi)-" & DiRetur() & " AS QtySisa, MBarang.Kode AS KodeBarang, MBarang.Nama AS NamaBarang, MBarang.NamaAlias, MGudang.Kode AS KodeGudang, MGudang.Nama AS NamaGudang, MSatuan.Nama AS NamaSatuan, "
                SQL &= " (MBeliD.Jumlah/isnull(MBeliD.Konversi,1)/isnull(MBeliD.Qty,1))-(IsNull(MBeli.DiskonNotaProsen,0)/100*(MBeliD.Jumlah/isnull(MBeliD.Konversi,1)/isnull(MBeliD.Qty,1))) AS [HargaPcsFix], IsNull(MBeli.DiskonNotaProsen,0) AS DiscProsenBawah, IsNull((SELECT TOP 1 MRevisiHargaBeliD.KoreksiBL*MRevisiHargaBeliD.Konversi FROM MRevisiHargaBeliD INNER JOIN MRevisiHargaBeli ON MRevisiHargaBeli.NoID=MRevisiHargaBeliD.IDRevisiHargaBeli WHERE MBeliD.NoID=MRevisiHargaBeliD.IDBeliD AND MBeli.NoID=MRevisiHargaBeliD.IDBeli AND MRevisiHargaBeli.IsPosted=1 ORDER BY MRevisiHargaBeli.Tanggal DESC),0) AS TotalPH "
                SQL &= " FROM (((MBeliD INNER JOIN MBeli ON MBeli.NoID=MBeliD.IDBeli) LEFT JOIN MGudang ON MGudang.NoID=MBeliD.IDGudang) "
                SQL &= " LEFT JOIN MBarang ON MBarang.NoID=MBeliD.IDBarang)"
                SQL &= " LEFT JOIN MSatuan ON MSatuan.NoID=MBeliD.IDSatuan "
                SQL &= " WHERE MBeliD.NoID= " & NullToLong(txtBeli.EditValue)
                Ds = New DataSet()
                Ds = ExecuteDataset("MBeliD", SQL)
                If Ds.Tables(0).Rows.Count >= 1 Then
                    With Ds.Tables("MBeliD").Rows(0)
                        gvBeli.SaveLayoutToXml(FolderLayouts & Me.Name & gvBeli.Name & ".xml")
                        txtBarang.EditValue = NullToLong(.Item("IDBarang"))
                        'txtBarang.Text = NullTostr(.Item("KodeBarang"))
                        txtSatuan.EditValue = NullToLong(.Item("IDSatuan"))
                        'txtSatuan.Text = NullTostr(.Item("KodeSatuan"))
                        txtGudang.EditValue = IDGudang
                        'txtGudang.Text = NullTostr(.Item("KodeGudang"))
                        QtySisa = NullToDbl(.Item("QtySisa"))
                        Konversi = IIf(NullToDbl(.Item("Konversi")) = 0, 1, NullToDbl(.Item("Konversi")))
                        HargaPcs = ((NullToDbl(.Item("Harga")) / Konversi) - (NullToDbl(.Item("Harga")) / Konversi / 100 * NullToDbl(.Item("DiscProsenBawah")))) - (NullToDbl(.Item("TotalPH")) / Konversi)

                        txtQty.EditValue = NullToDbl(.Item("QtySisa")) / Konversi

                        'txtHarga.EditValue = (NullToDbl(.Item("Harga")) - (NullToDbl(.Item("Harga")) / 100 * NullToDbl(.Item("DiscProsenBawah")))) - NullToDbl(.Item("TotalPH"))
                        txtHarga.EditValue = (NullToDbl(.Item("Harga")) / Konversi) * txtKonversi.EditValue 'Hartani 28/05/2012

                        txtCtn.EditValue = NullToDbl(.Item("CTN"))
                        txtDiscPersen1.EditValue = NullToDbl(.Item("DiscPersen1"))
                        txtDiscPersen2.EditValue = NullToDbl(.Item("DiscPersen2"))
                        txtDiscPersen3.EditValue = NullToDbl(.Item("DiscPersen3"))
                        txtDiscRp1.EditValue = NullToDbl(.Item("Disc1"))
                        txtDiscRp2.EditValue = NullToDbl(.Item("Disc2"))
                        txtDiscRp3.EditValue = NullToDbl(.Item("Disc3"))
                        'txtDiscPOBawah.EditValue = NullToDbl(.Item("DiskonNotaProsen"))
                        txtJUmlah.EditValue = NullToDbl(.Item("Jumlah"))
                        txtCatatan.Text = NullToStr(.Item("Catatan"))
                        txtKonversi.EditValue = Konversi
                        KonversiBeli = Konversi
                        HitungJumlah()
                    End With
                Else
                    IsiDefault()
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

    Private Sub txtHarga_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtHarga.LostFocus
        HitungJumlah()
    End Sub

    'Private Sub txtBeli_EditValueChanging(ByVal sender As Object, ByVal e As DevExpress.XtraEditors.Controls.ChangingEventArgs) Handles txtBeli.EditValueChanging
    '    If System.IO.File.Exists(folderLayouts &  Me.Name & gvBeli.Name & ".xml") Then
    '        gvBeli.RestoreLayoutFromXml(folderLayouts &  Me.Name & gvBeli.Name & ".xml")
    '    End If
    '    With gvBeli
    '        For x As Integer = 0 To .Columns.Count - 1
    '            Select Case .Columns(x).ColumnType.Name.ToLower
    '                Case "int32", "int64", "int"
    '                    .Columns(x).DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
    '                    .Columns(x).DisplayFormat.FormatString = "n0"
    '                Case "decimal", "single", "money", "double"
    '                    .Columns(x).DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
    '                    .Columns(x).DisplayFormat.FormatString = "n2"
    '                Case "string"
    '                    .Columns(x).DisplayFormat.FormatType = DevExpress.Utils.FormatType.None
    '                    .Columns(x).DisplayFormat.FormatString = ""
    '                Case "date", "datetime"
    '                    If .Columns(x).FieldName.Trim.ToLower = "jam" Then
    '                        .Columns(x).DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime
    '                        .Columns(x).DisplayFormat.FormatString = "HH:mm"
    '                    Else
    '                        .Columns(x).DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime
    '                        .Columns(x).DisplayFormat.FormatString = "dd-MM-yyyy"
    '                    End If
    '            End Select
    '        Next
    '    End With
    'End Sub

    Private Sub gvBeli_DataSourceChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvBeli.DataSourceChanged
        If System.IO.File.Exists(folderLayouts & Me.Name & gvBeli.Name & ".xml") Then
            gvBeli.RestoreLayoutFromXml(folderLayouts & Me.Name & gvBeli.Name & ".xml")
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
            RefreshLookUp()
        Catch ex As Exception
            XtraMessageBox.Show("Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            Ds.Dispose()
        End Try
    End Sub

    Private Sub txtQty_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtQty.EditValueChanged
        HitungJumlah()
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

    Private Sub txtKonversi_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtKonversi.LostFocus
        HitungJumlah()
    End Sub

    Private Sub txtHarga_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtHarga.EditValueChanged
        HitungJumlah()
    End Sub

    Private Sub txtSatuan_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtSatuan.LostFocus
        RubahSatuan()
    End Sub

    Private Sub txtBarcode_ButtonClick(ByVal sender As Object, ByVal e As DevExpress.XtraEditors.Controls.ButtonPressedEventArgs) Handles txtBarcode.ButtonClick
        If e.Button.Index = 1 Then
            RefreshBarcode(True)
        End If
    End Sub

    Private Sub txtBarcode_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtBarcode.EditValueChanged
        Dim Ds As New DataSet
        Try
            SQL = "SELECT MBarangD.NoID,MBarangD.Varian,MBarangD.IDBarang,MBarangD.IDSatuan,MBarang.Kode,MBarang.IDSatuan IDSatuanBase,MBarang.Nama AS NamaBarang, MBarang.NamaAlias " & _
                  " FROM MBarangD inner join MBarang On MBarangD.IDbarang=MBarang.NoID WHERE MBarangD.NoID=" & NullToLong(txtBarcode.EditValue)
            Ds = ExecuteDataset("Tabel", SQL)
            If Ds.Tables("Tabel").Rows.Count >= 1 Then
                'txtSatuan.EditValue = NullTolong(Ds.Tables(0).Rows(0).Item("IDSatuan"))
                'txtKonversi.EditValue = NullToDbl(Ds.Tables(0).Rows(0).Item("Konversi"))
                txtBarang.EditValue = NullToLong(Ds.Tables(0).Rows(0).Item("IDBarang"))
                'txtVarian.Text = NullToStr(Ds.Tables(0).Rows(0).Item("Varian"))
                txtNamaStock.Text = NullToStr(Ds.Tables(0).Rows(0).Item("NamaBarang"))
                'txtNamaAlias.Text = NullToStr(Ds.Tables(0).Rows(0).Item("NamaAlias"))
                RefreshLookUpSatuan()
                txtSatuan.EditValue = NullToLong(Ds.Tables(0).Rows(0).Item("IDSatuan")) 'DefIDSatuan
                'RubahSatuan()
                'RefreshLookUpSTB()
                'RefreshLookUpPO()
            End If
        Catch ex As Exception
            XtraMessageBox.Show("Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            'RefreshLookUpSTB()
            Ds.Dispose()
        End Try
    End Sub
End Class