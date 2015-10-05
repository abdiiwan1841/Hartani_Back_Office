Imports System.Data
Imports System.Data.SqlClient
Imports DevExpress.XtraEditors
Imports DevExpress.Utils
Imports System.IO.File
Public Class frmEntriBeliD
    Public IsNew As Boolean = True
    Public FormPemanggil As frmEntriBeli
    Public IsPosted As Boolean = False
    Public NoID As Long = -1
    Public IDBeli As Long = -1
    Public IDSupplier As Long = -1
    Public IDGudang As Long = DefIDGudang

    Dim QtySisa As Double = 0.0
    Dim QtySisaSTB As Double = 0.0
    Dim oDS As New DataSet
    Dim SQL As String = ""
    Dim DefImageList As ImageList
    Dim HargaPcs As Double = 0.0
    Dim Konversi As Double = 0.0
    Dim KonversiPO As Double = 0.0
    Dim IsLoad As Boolean = True
    Public IsFastEntri As Boolean = False

    Private Sub IsiDefault()
        DefImageList = frmMain.ImageList1
        cmdSave.ImageList = DefImageList
        cmdSave.ImageIndex = 6

        cmdTutup.ImageList = DefImageList
        cmdTutup.ImageIndex = 3
        RefreshLookUp()
        txtSatuan.EditValue = 1
        Konversi = 1.0
        HargaPcs = 0.0
        QtySisa = 0.0
        QtySisaSTB = 0.0
        txtBiaya.EditValue = 0.0
        txtGudang.EditValue = IDGudang
        RubahGudang()
    End Sub
    Private Function DiBeli() As String
        Return "isnull((SELECT SUM(isnull(A.Qty,0)*isnull(A.Konversi,0)) FROM MBeliD A INNER JOIN MBeli B ON B.NoID=A.IDBeli WHERE A.NoID <> " & NoID & " AND A.IDPOD=MPOD.NoID),0)"
    End Function
    Private Function DiSTB() As String
        Return "isnull((SELECT SUM(isnull(A.Qty,0)*isnull(A.Konversi,0)) FROM MBeliD A INNER JOIN MBeli B ON B.NoID=A.IDBeli WHERE A.NoID <> " & NoID & " AND A.IDLPBD=MLPBD.NoID),0)"
    End Function
    Private Sub RefreshLookUpSatuan()
        Dim ds As New DataSet
        Try
            'SQL = "SELECT MSatuan.NoID, MSatuan.Kode, MSatuan.Nama FROM MSatuan WHERE MSatuan.IsActive=1 AND MSatuan.NoID IN (SELECT MBarangD.IDSatuan FROM MBarangD WHERE MBarangD.IsJual=1 AND MBarangD.IsActive=1 AND MBarangD.IDBarang=" & NullToLong(txtBarang.EditValue) & ")"
            'BUKA ALL
            ' SQL = "SELECT MSatuan.NoID, MSatuan.Kode, MSatuan.Nama FROM MSatuan WHERE MSatuan.IsActive=1 "
            SQL = "Select distinct * from ("
            SQL = SQL & "SELECT MSatuan.NoID, MSatuan.Kode, MSatuan.Nama,Mbarang.CtnPcs AS Isi FROM MSatuan inner join mbarang on MBarang.IDSatuanHarga=Msatuan.NoID where MBarang.NoID=" & NullToLong(txtBarang.EditValue) & " "
            SQL = SQL & " UNION ALL SELECT MSatuan.NoID, MSatuan.Kode, MSatuan.Nama,1.0 AS Isi FROM MSatuan inner join mbarang on MBarang.IDSatuan=Msatuan.NoID where MBarang.NoID=" & NullToLong(txtBarang.EditValue)
            SQL = SQL & " UNION ALL SELECT MSatuan.NoID, MSatuan.Kode, MSatuan.Nama,MBarangd.Konversi AS Isi FROM MSatuan inner join mbarangd on MBarangd.IDSatuan=Msatuan.NoID where MBarangd.IDBarang=" & NullToLong(txtBarang.EditValue) & " ) X "
            ds = ExecuteDataset("MSatuan", SQL)
            txtSatuan.Properties.DataSource = ds.Tables("MSatuan")
            txtSatuan.Properties.ValueMember = "NoID"
            txtSatuan.Properties.DisplayMember = "Kode"
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
        Catch ex As Exception
            XtraMessageBox.Show("Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            ds.Dispose()
        End Try
    End Sub
    Private Sub RefreshLookUpPO()
        Dim ds As New DataSet
        Try
            SQL = "SELECT MPOD.NoID, MPO.Kode, MGudang.Nama AS Gudang, MWilayah.Nama AS Wilayah, MBarang.Kode AS KodeBarang, MBarang.Nama AS NamaBarang, "
            SQL &= " (MPOD.HargaNetto/isnull(MPOD.Konversi,1))-(MPOD.HargaNetto/isnull(MPOD.Konversi,1)*IsNull(MPO.DiskonNotaProsen,0)/100) AS [Harga (Pcs)],"
            SQL &= " MPOD.Qty, MSatuan.Nama AS Satuan, MPOD.Qty*MPOD.Konversi AS [Qty(Pcs)], MPOD.Qty*MPOD.Konversi- " & DiBeli() & "  AS [Sisa(Pcs)]"
            SQL &= " FROM MPOD LEFT JOIN MSatuan ON MSatuan.NoID=MPOD.IDSatuan "
            SQL &= " LEFT JOIN MPO ON MPO.NoID=MPOD.IDPO "
            SQL &= " LEFT JOIN MGudang ON MGudang.NoID=MPOD.IDGudang "
            SQL &= " LEFT JOIN MWilayah ON MWilayah.NoID=MPOD.IDWilayah "
            SQL &= " LEFT JOIN MBarang ON MBarang.NoID=MPOD.IDBarang "
            SQL &= " WHERE MPOD.IDBarang=" & NullTolong(txtBarang.EditValue) & " AND MWilayah.NoID=" & DefIDWilayah & " AND (MPO.IsSelesai=0 OR MPO.IsSelesai IS NULL) AND (MPOD.Qty*MPOD.Konversi- " & DiBeli() & ">0) AND MBarang.IsActive = 1 And MPO.IsPosted = 1 And MPO.IDSupplier = " & IDSupplier
            ds = ExecuteDataset("MPOD", SQL)
            txtPO.Properties.DataSource = ds.Tables("MPOD")
            txtPO.Properties.ValueMember = "NoID"
            txtPO.Properties.DisplayMember = "Kode"
        Catch ex As Exception
            XtraMessageBox.Show("Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            ds.Dispose()
        End Try
    End Sub
    Private Sub RefreshLookUp()
        Dim ds As New DataSet
        Try

            RefreshBarcode(False)
            RefreshKodeBarang(False)

            SQL = "SELECT MGudang.NoID, MGudang.Kode, MGudang.Nama, MWilayah.Nama AS Wilayah FROM MGudang LEFT JOIN MWilayah ON MWilayah.NoID=MGudang.IDWilayah WHERE MWilayah.NoID=" & DefIDWilayah & " AND MGudang.IsActive=1 AND MGudang.IsBS=0 "
            ds = ExecuteDataset("MGudang", SQL)
            txtGudang.Properties.DataSource = ds.Tables("MGudang")
            txtGudang.Properties.ValueMember = "NoID"
            txtGudang.Properties.DisplayMember = "Nama"

            SQL = "SELECT NoID, Kode, Nama FROM MTypePajak WHERE IsActive=1 "
            ds = ExecuteDataset("Data", SQL)
            txtTypePajak.Properties.DataSource = ds.Tables("Data")
            txtTypePajak.Properties.ValueMember = "NoID"
            txtTypePajak.Properties.DisplayMember = "Nama"

            RefreshLookUpPO()
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
        SQL = "SELECT MBarangD.NoID,MBarangD.Barcode,MBarang.Kode,MBarang.KodeAlias AS Alias, MBarang.Nama + ' ' + MBarangD.Varian AS Nama, MBarang.Catatan AS Keterangan, MMerk.Nama as Merk, MSatuan.Kode as Satuan,MBarangD.IDSatuan,MBarangD.IDBarang FROM MBarangD inner join MBarang On MBarangD.IDBarang=MBarang.NoID inner join MSatuan On MBarangD.IDSatuan=MSatuan.NoID Left Join MMerk On MBarang.IDMerk=MMerk.NoID WHERE MBarang.IsActive=1 AND MBarangD.IsActive=1"
        If IDSupplier >= 1 AndAlso NullToBool(EksekusiSQlSkalarNew("SELECT IsStockPerSupplier FROM MSETTING")) Then
            SQL &= " AND (MBarang.IDSupplier1=" & IDSupplier
            SQL &= " OR MBarang.IDSupplier2=" & IDSupplier
            SQL &= " OR MBarang.IDSupplier3=" & IDSupplier
            SQL &= " OR MBarang.IDSupplier4=" & IDSupplier
            SQL &= " OR MBarang.IDSupplier5=" & IDSupplier & ")"
        End If
        dsPublic = ExecuteDataset("MBarangD", SQL)
        txtBarcode.Properties.DataSource = dsPublic.Tables("MBarangD")
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
    Sub RefreshKodeBarang(ByVal IsForce As Boolean)
        Dim dsPublicKodeBarang As New DataSet
        'If IsForce Then
        '    If DSPublicKodeBarang.Tables("MBarang") Is Nothing Then 'blm ada data
        '        SQL = "SELECT MBarang.NoID, (CASE WHEN ISNULL(MBarang.Kode,'')='' THEN MBarang.KodeAlias ELSE MBarang.Kode END) AS Kode,MBarang.Barcode,MBarang.HPP, MBarang.Nama, MBarang.NamaAlias, MSatuan.Nama AS Satuan FROM MBarang LEFT JOIN MSatuan ON MSatuan.NoID=MBarang.IDSatuan WHERE MBarang.IsActive=1 AND NOT ((MBarang.KODE='' OR MBarang.KODE IS NULL) AND (MBarang.KodeAlias='' OR MBarang.KodeAlias IS NULL))"
        '        DSPublicKodeBarang = ExecuteDataset("MBarang", SQL)
        '    Else
        '        SQL = "SELECT MBarang.NoID, (CASE WHEN ISNULL(MBarang.Kode,'')='' THEN MBarang.KodeAlias ELSE MBarang.Kode END) AS Kode,MBarang.Barcode,MBarang.HPP, MBarang.Nama, MBarang.NamaAlias, MSatuan.Nama AS Satuan FROM MBarang LEFT JOIN MSatuan ON MSatuan.NoID=MBarang.IDSatuan WHERE MBarang.IsActive=1 AND NOT ((MBarang.KODE='' OR MBarang.KODE IS NULL) AND (MBarang.KodeAlias='' OR MBarang.KodeAlias IS NULL))"
        '        DSPublicKodeBarang = ExecuteDataset("MBarang", SQL)
        '    End If
        'Else
        '    If Not DSPublicKodeBarang Is Nothing AndAlso Not DSPublicKodeBarang.Tables("MBarang") Is Nothing Then 'blm ada data
        '        If DSPublicKodeBarang.Tables("MBarang").Rows.Count = 0 Then
        '            SQL = "SELECT MBarang.NoID, (CASE WHEN ISNULL(MBarang.Kode,'')='' THEN MBarang.KodeAlias ELSE MBarang.Kode END) AS Kode,MBarang.Barcode,MBarang.HPP, MBarang.Nama, MBarang.NamaAlias, MSatuan.Nama AS Satuan FROM MBarang LEFT JOIN MSatuan ON MSatuan.NoID=MBarang.IDSatuan WHERE MBarang.IsActive=1 AND NOT ((MBarang.KODE='' OR MBarang.KODE IS NULL) AND (MBarang.KodeAlias='' OR MBarang.KodeAlias IS NULL))"
        '            DSPublicKodeBarang = ExecuteDataset("MBarang", SQL)
        '        End If
        '    Else 'sudah ada data dan tidak dipaksa maka biarkan
        '        SQL = "SELECT MBarang.NoID, (CASE WHEN ISNULL(MBarang.Kode,'')='' THEN MBarang.KodeAlias ELSE MBarang.Kode END) AS Kode,MBarang.Barcode,MBarang.HPP, MBarang.Nama, MBarang.NamaAlias, MSatuan.Nama AS Satuan FROM MBarang LEFT JOIN MSatuan ON MSatuan.NoID=MBarang.IDSatuan WHERE MBarang.IsActive=1 AND NOT ((MBarang.KODE='' OR MBarang.KODE IS NULL) AND (MBarang.KodeAlias='' OR MBarang.KodeAlias IS NULL))"
        '        DSPublicKodeBarang = ExecuteDataset("MBarang", SQL)
        '    End If
        'End If
        SQL = "SELECT MBarang.NoID, (CASE WHEN ISNULL(MBarang.Kode,'')='' THEN MBarang.KodeAlias ELSE MBarang.Kode END) AS Kode,MBarang.Barcode,MBarang.HPP, MBarang.Nama, MBarang.NamaAlias, MSatuan.Nama AS Satuan FROM MBarang LEFT JOIN MSatuan ON MSatuan.NoID=MBarang.IDSatuan WHERE MBarang.IsActive=1 "
        If IDSupplier >= 1 AndAlso NullToBool(EksekusiSQlSkalarNew("SELECT IsStockPerSupplier FROM MSETTING")) Then
            SQL &= " AND (MBarang.IDSupplier1=" & IDSupplier
            SQL &= " OR MBarang.IDSupplier2=" & IDSupplier
            SQL &= " OR MBarang.IDSupplier3=" & IDSupplier
            SQL &= " OR MBarang.IDSupplier4=" & IDSupplier
            SQL &= " OR MBarang.IDSupplier5=" & IDSupplier & ")"
        End If
        dsPublicKodeBarang = ExecuteDataset("MBarang", SQL)
        txtBarang.Properties.DataSource = DSPublicKodeBarang.Tables("MBarang")
        txtBarang.Properties.ValueMember = "NoID"
        txtBarang.Properties.DisplayMember = "Kode"
        If System.IO.File.Exists(FolderLayouts & "frmEntriBeliD" & gvBarang.Name & ".xml") Then
            gvBarang.RestoreLayoutFromXml(FolderLayouts & "frmEntriBeliD" & gvBarang.Name & ".xml")
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
    Private Sub RefreshLookUpSTB()
        Dim ds As New DataSet
        Try
            SQL = "SELECT MLPBD.NoID, MWilayah.Nama AS Wilayah, MGudang.Nama AS Gudang, MLPB.Kode, MLPB.Tanggal, MAlamat.Nama AS Supplier, MBagPembelian.Nama AS Penerima, MLPBD.Qty * MLPBD.Konversi AS [QtyPcs], MLPBD.Qty*MLPBD.Konversi-" & DiSTB() & "  AS [Sisa(Pcs)] "
            SQL &= " FROM (MLPBD LEFT OUTER JOIN (MLPB LEFT OUTER JOIN MAlamat AS MBagPembelian ON MBagPembelian.NoID = MLPB.IDBagPembelian "
            SQL &= " LEFT OUTER JOIN MAlamat ON MLPB.IDSupplier = MAlamat.NoID)"
            SQL &= " ON MLPB.NoID=MLPBD.IDLPB) "
            SQL &= " LEFT OUTER JOIN MBarang ON MLPBD.IDBarang = MBarang.NoID "
            SQL &= " LEFT OUTER JOIN (MGudang LEFT JOIN MWilayah ON MWilayah.NoID=MGudang.IDWilayah) ON MLPB.IDGudangPenerima = MGudang.NoID "
            SQL &= " WHERE MLPB.IsPosted = 1 AND MBarang.IsActive=1 AND (MLPBD.Qty*MLPBD.Konversi-" & DiSTB() & ">0) AND MLPBD.IDGudang= " & NullTolong(txtGudang.EditValue) & " AND MLPB.IDSupplier= " & NullTolong(IDSupplier) & " AND MLPBD.IDBarang= " & NullTolong(txtBarang.EditValue)

            ds = ExecuteDataset("MLPBD", SQL)
            txtSTB.Properties.DataSource = ds.Tables("MLPBD")
            txtSTB.Properties.ValueMember = "NoID"
            txtSTB.Properties.DisplayMember = "Kode"
            If System.IO.File.Exists(FolderLayouts & Me.Name & gvSTB.Name & ".xml") Then
                gvSTB.RestoreLayoutFromXml(FolderLayouts & Me.Name & gvSTB.Name & ".xml")
            End If
            With gvSTB
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
    Private Sub LoadData()
        Try
            SQL = "SELECT MBeliD.*, MPO.DiskonNotaProsen, MPOD.Konversi AS KonversiAsal, (MPOD.Harga/MPOD.Konversi) AS HargaAsal, (MPOD.Qty*MPOD.Konversi) AS QtyAsal, MBarang.Kode AS KodeBarang, MBarang.Nama AS NamaBarang, MGudang.Kode AS KodeGudang, MGudang.Nama AS NamaGudang, MSatuan.Nama AS NamaSatuan "
            SQL &= " FROM (((MBeliD LEFT JOIN MGudang ON MGudang.NoID=MBeliD.IDGudang) "
            SQL &= " LEFT JOIN MBarang ON MBarang.NoID=MBeliD.IDBarang)"
            SQL &= " LEFT JOIN (MPOD INNER JOIN MPO ON MPO.NoID=MPOD.IDPO) ON MPOD.NoID=MBeliD.IDPOD)"
            SQL &= " LEFT JOIN MSatuan ON MSatuan.NoID=MBeliD.IDSatuan "
            SQL &= " WHERE MBeliD.NoID= " & NoID
            oDS = New DataSet()
            oDS = ExecuteDataset("MBeliD", SQL)
            If oDS.Tables(0).Rows.Count >= 1 Then
                With oDS.Tables("MBeliD").Rows(0)
                    NoID = NullTolong(.Item("NoID"))
                    IDBeli = NullToLong(.Item("IDBeli"))
                    TglExpired.EditValue = (.Item("ExpiredDate"))
                    txtPO.EditValue = NullToLong(.Item("IDPOD"))
                    'txtDiscPOBawah.EditValue = NullToDbl(.Item("DiskonNotaProsen"))
                    txtSTB.EditValue = NullToLong(.Item("IDLPBD"))
                    'If NullTolong(txtSTB.EditValue) <> 0 Or txtSTB.Text <> "" Then
                    '    txtSatuan.Properties.ReadOnly = True
                    '    txtQty.Properties.ReadOnly = True
                    'Else
                    '    txtSatuan.Properties.ReadOnly = False
                    '    txtQty.Properties.ReadOnly = False
                    'End If
                    txtBarang.EditValue = NullToLong(.Item("IDBarang"))
                    txtBarcode.EditValue = NullToLong(.Item("IDBarangD"))
                    txtSatuan.EditValue = NullTolong(.Item("IDSatuan"))
                    'RefreshLookUpSTB()
                    'txtSatuan.Text = NullTostr(.Item("KodeSatuan"))
                    txtGudang.EditValue = IDGudang
                    'txtGudang.Text = NullTostr(.Item("KodeGudang"))
                    txtQty.EditValue = NullToDbl(.Item("Qty"))
                    HargaPcs = NullToDbl(.Item("HargaAsal"))
                    KonversiPO = NullToDbl(.Item("KonversiAsal"))
                    'QtySisa = NullToDbl(EksekusiSQlSkalarNew("Select ((MPOD.Qty*MPOD.Konversi)-" & DiBeli() & ") AS QtySisa FROM MPOD WHERE NoID=" & NullTolong(.Item("IDPOD"))))
                    'QtySisaSTB = NullToDbl(EksekusiSQlSkalarNew("Select ((MLPBD.Qty*MLPBD.Konversi)-" & DiSTB() & ") AS QtySisa FROM MLPBD WHERE NoID=" & NullTolong(.Item("IDLPBD"))))
                    txtHarga.EditValue = NullToDbl(.Item("Harga"))
                    txtCtn.EditValue = NullToDbl(.Item("CTN"))
                    txtDiscPersen1.EditValue = NullToDbl(.Item("DiscPersen1"))
                    txtDiscPersen2.EditValue = NullToDbl(.Item("DiscPersen2"))
                    txtDiscPersen3.EditValue = NullToDbl(.Item("DiscPersen3"))
                    txtDiscRp1.EditValue = NullToDbl(.Item("Disc1"))
                    txtDiscRp2.EditValue = NullToDbl(.Item("Disc2"))
                    txtDiscRp3.EditValue = NullToDbl(.Item("Disc3"))
                    txtJumlah.EditValue = NullToDbl(.Item("Jumlah"))
                    txtTypePajak.EditValue = NullToLong(.Item("IDTypePajak"))
                    txtPPN.EditValue = NullToDbl(.Item("PPN"))
                    txtBiaya.EditValue = NullToDbl(.Item("Biaya"))
                    txtCatatan.Text = NullToStr(.Item("Catatan"))
                    Konversi = NullToDbl(.Item("Konversi"))
                    txtKonversi.EditValue = NullToDbl(.Item("Konversi"))
                    txtMargin.EditValue = NullToDbl(.Item("ProsenMargin"))
                    txtHitungHargaJual.EditValue = NullToDbl(.Item("HitungJual"))
                    txtHargaJual.EditValue = NullToDbl(.Item("HargaJual"))
                    '
                    ',ProsenMargin,HitungJual,HargaJual
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
                SQL = "INSERT INTO MBeliD (NoID,IDBeli,IDBarangD,IDPOD,IDLPBD,NoUrut,Tgl,Jam,ExpiredDate," & _
                "IDBarang,IDSatuan,Qty,QtyPcs,Harga,Biaya,HargaPcs,CTN,DiscPersen1,DiscPersen2,DiscPersen3,Disc1,Disc2,Disc3,Jumlah,Catatan,IDGudang,Konversi,PPN,HargaNetto,ProsenMargin,HitungJual,HargaJual,IDTypePajak) VALUES ("
                SQL &= NullTolong(GetNewID("MBeliD", "NoID")) & ","
                SQL &= IDBeli & ","
                SQL &= NullToLong(txtBarcode.EditValue) & ","
                SQL &= NullTolong(txtPO.EditValue) & ","
                SQL &= NullTolong(txtSTB.EditValue) & ","
                SQL &= GetNewID("MBeliD", "NoUrut", " WHERE IDBeli=" & IDBeli) & ","
                SQL &= "GetDate(),"
                SQL &= "GetDate(),"
                SQL &= "" & IIf(TglExpired.Text = "", "NULL", "'" & Format(TglExpired.DateTime, "yyyy-MM-dd") & "'") & ","
                SQL &= NullTolong(txtBarang.EditValue) & ","
                SQL &= NullTolong(txtSatuan.EditValue) & ","
                SQL &= FixKoma(txtQty.EditValue) & ","
                SQL &= FixKoma(txtQty.EditValue * txtKonversi.EditValue) & ","
                SQL &= FixKoma(txtHarga.EditValue) & ","
                SQL &= FixKoma(txtBiaya.EditValue) & ","
                SQL &= FixKoma(txtHargaBeliPcs.EditValue) & ","
                SQL &= FixKoma(txtCtn.EditValue) & ","
                SQL &= FixKoma(txtDiscPersen1.EditValue) & ","
                SQL &= FixKoma(txtDiscPersen2.EditValue) & ","
                SQL &= FixKoma(txtDiscPersen3.EditValue) & ","
                SQL &= FixKoma(txtDiscRp1.EditValue) & ","
                SQL &= FixKoma(txtDiscRp2.EditValue) & ","
                SQL &= FixKoma(txtDiscRp3.EditValue) & ","
                SQL &= FixKoma(txtJumlah.EditValue) & ","
                SQL &= "'" & FixApostropi(txtCatatan.Text) & "',"
                SQL &= NullTolong(txtGudang.EditValue) & ","
                SQL &= FixKoma(txtKonversi.EditValue) & ","
                SQL &= FixKoma(txtPPN.EditValue) & ","
                SQL &= FixKoma(txtHargaBeliPcs.EditValue * txtKonversi.EditValue) & ","
                SQL &= FixKoma(txtMargin.EditValue) & ","
                SQL &= FixKoma(txtHitungHargaJual.EditValue) & ","
                SQL &= FixKoma(txtHargaJual.EditValue) & "," & NullToLong(txtTypePajak.EditValue)
                SQL &= ")"
            Else
                SQL = "UPDATE MBeliD SET "
                SQL &= " Tgl=GetDate(),"
                SQL &= " Jam=GetDate(),"
                SQL &= " ExpiredDate=" & IIf(TglExpired.Text = "", "NULL", "'" & Format(TglExpired.DateTime, "yyyy-MM-dd") & "'") & ","
                SQL &= " IDBeli=" & IDBeli & ","
                SQL &= " IDPOD=" & NullTolong(txtPO.EditValue) & ","
                SQL &= " IDLPBD=" & NullTolong(txtSTB.EditValue) & ","
                SQL &= " IDBarangD=" & NullToLong(txtBarcode.EditValue) & ","
                SQL &= " IDBarang=" & NullToLong(txtBarang.EditValue) & ","
                SQL &= " IDSatuan=" & NullTolong(txtSatuan.EditValue) & ","
                SQL &= " Qty=" & FixKoma(txtQty.EditValue) & ","
                SQL &= " QtyPcs=" & FixKoma(txtQty.EditValue * txtKonversi.EditValue) & ","
                SQL &= " Harga=" & FixKoma(txtHarga.EditValue) & ","
                SQL &= " Biaya=" & FixKoma(txtBiaya.EditValue) & ","
                SQL &= " HargaPcs=" & FixKoma(txtHargaBeliPcs.EditValue) & ","
                SQL &= " CTN=" & FixKoma(txtCtn.EditValue) & ","
                SQL &= " DiscPersen1=" & FixKoma(txtDiscPersen1.EditValue) & ","
                SQL &= " DiscPersen2=" & FixKoma(txtDiscPersen2.EditValue) & ","
                SQL &= " DiscPersen3=" & FixKoma(txtDiscPersen3.EditValue) & ","
                SQL &= " Disc1=" & FixKoma(txtDiscRp1.EditValue) & ","
                SQL &= " Disc2=" & FixKoma(txtDiscRp2.EditValue) & ","
                SQL &= " Disc3=" & FixKoma(txtDiscRp3.EditValue) & ","
                SQL &= " Jumlah=" & FixKoma(txtJumlah.EditValue) & ","
                SQL &= " Catatan='" & FixApostropi(txtCatatan.Text) & "',"
                SQL &= " IDGudang=" & NullToLong(txtGudang.EditValue) & ","
                SQL &= " PPN=" & FixKoma(txtPPN.EditValue) & ","
                SQL &= " HargaNetto=" & FixKoma(txtHargaBeliPcs.EditValue * txtKonversi.EditValue) & ","
                SQL &= " ProsenMargin=" & FixKoma(txtMargin.EditValue) & ","
                SQL &= " HitungJual=" & FixKoma(txtHitungHargaJual.EditValue) & ","
                SQL &= " HargaJual=" & FixKoma(txtHargaJual.EditValue) & ","
                SQL &= " IDTypePajak=" & NullToLong(txtTypePajak.EditValue) & ", "
                SQL &= " Konversi=" & FixKoma(txtKonversi.EditValue) & ""
                SQL &= " WHERE NoID=" & NoID
                ',ProsenMargin,HitungJual,HargaJual
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
        If txtQty.EditValue <= 0 Then
            XtraMessageBox.Show("Qty masih kurang dari atau sama dengan 0.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
            txtQty.Focus()
            Return False
            Exit Function
        End If
        'If txtCtn.EditValue < 0 Then
        '    XtraMessageBox.Show("Ctn tidak boleh kurang dari 0.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
        '    txtCtn.Focus()
        '    Return False
        '    Exit Function
        'End If
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
        ElseIf txtDiscPersen1.EditValue > 100 Then
            XtraMessageBox.Show("Disc persen 1 tidak boleh lebih dari 100.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
            txtDiscPersen1.Focus()
            Return False
            Exit Function
        End If
        If txtDiscPersen2.EditValue < 0 Then
            XtraMessageBox.Show("Disc persen 1 tidak boleh kurang dari 0.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
            txtDiscPersen2.Focus()
            Return False
            Exit Function
        ElseIf txtDiscPersen2.EditValue > 100 Then
            XtraMessageBox.Show("Disc persen 2 tidak boleh lebih dari 100.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
            txtDiscPersen2.Focus()
            Return False
            Exit Function
        End If
        If txtDiscPersen3.EditValue < 0 Then
            XtraMessageBox.Show("Disc persen 1 tidak boleh kurang dari 0.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
            txtDiscPersen2.Focus()
            Return False
            Exit Function
        ElseIf txtDiscPersen3.EditValue > 100 Then
            XtraMessageBox.Show("Disc persen 3 tidak boleh lebih dari 100.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
            txtDiscPersen3.Focus()
            Return False
            Exit Function
        End If
        If txtDiscRp1.EditValue < 0 Then
            XtraMessageBox.Show("Disc rupiah 1 tidak boleh kurang dari 0.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
            txtDiscRp1.Focus()
            Return False
            Exit Function
        ElseIf txtDiscRp1.EditValue > txtHarga.EditValue Then
            XtraMessageBox.Show("Disc rupiah 1 tidak boleh lebih dari harga.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
            txtDiscRp1.Focus()
            Return False
            Exit Function
        End If
        If txtDiscRp2.EditValue < 0 Then
            XtraMessageBox.Show("Disc rupiah 2 tidak boleh kurang dari 0.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
            txtDiscRp2.Focus()
            Return False
            Exit Function
        ElseIf txtDiscRp2.EditValue > txtHarga.EditValue Then
            XtraMessageBox.Show("Disc rupiah 2 tidak boleh lebih dari harga.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
            txtDiscRp2.Focus()
            Return False
            Exit Function
        End If
        If txtDiscRp3.EditValue < 0 Then
            XtraMessageBox.Show("Disc rupiah 3 tidak boleh kurang dari 0.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
            txtDiscRp3.Focus()
            Return False
            Exit Function
        ElseIf txtDiscRp3.EditValue > txtHarga.EditValue Then
            XtraMessageBox.Show("Disc rupiah 3 tidak boleh lebih dari harga.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
            txtDiscRp3.Focus()
            Return False
            Exit Function
        End If
        txtJumlah.EditValue = txtQty.EditValue * ((txtHarga.EditValue - txtDiscRp1.EditValue - txtDiscRp2.EditValue - txtDiscRp3.EditValue) * (1 - (txtDiscPersen1.EditValue / 100)) * (1 - (txtDiscPersen2.EditValue / 100)) * (1 - (txtDiscPersen3.EditValue / 100)))
        If txtJumlah.EditValue > txtHarga.EditValue * txtQty.EditValue Then
            XtraMessageBox.Show("Total discount melebihi harga.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
            txtHarga.Focus()
            Return False
            Exit Function
        End If
        If txtHarga.EditValue <= 0 Then
            If txtHarga.EditValue = 0 AndAlso XtraMessageBox.Show("Harga masih 0, lanjutkan penyimpanan?", NamaAplikasi, MessageBoxButtons.YesNo, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1) = Windows.Forms.DialogResult.No Then
                txtHarga.Focus()
                Return False
                Exit Function
            ElseIf txtHarga.EditValue < 0 Then
                XtraMessageBox.Show("Harga masih kurang dari 0.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
                txtHarga.Focus()
                Return False
                Exit Function
            End If
        End If
        If txtPO.Text <> "" Then
            QtySisa = NullToDbl(EksekusiSQlSkalarNew("Select ((MPOD.Qty*MPOD.Konversi)-" & DiBeli() & ") AS QtySisa FROM MPOD WHERE NoID=" & NullTolong(txtPO.EditValue)))
            If QtySisa < txtQty.EditValue * txtKonversi.EditValue Then
                If XtraMessageBox.Show("Qty melebihi standart di PO." & vbCrLf & "Ingin melanjutkan penyimpanan?", NamaAplikasi, MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) = Windows.Forms.DialogResult.No Then
                    txtQty.Focus()
                    Return False
                    Exit Function
                End If
            End If
            If txtHarga.EditValue / txtKonversi.EditValue < HargaPcs Then
                If XtraMessageBox.Show("Harga beli dibawah standart harga di PO " & txtPO.Text & vbCrLf & "Yakin ingin melanjutkan penyimpanan?", NamaAplikasi, MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) = Windows.Forms.DialogResult.No Then
                    txtHarga.Focus()
                    Return False
                    Exit Function
                End If
            End If
        End If
        'If txtSTB.Text = "" Then
        '    If XtraMessageBox.Show("Yakin ingin melakukan pembelian tanpa Penerimaan Barang?" & vbCrLf & "Yes untuk melanjutkan, No untuk membatalkan penyimpanan.", NamaAplikasi, MessageBoxButtons.YesNo, MessageBoxIcon.Warning) = Windows.Forms.DialogResult.No Then
        '        txtSTB.Focus()
        '        Return False
        '        Exit Function
        '    End If
        '    'Else
        '    'QtySisaSTB = NullToDbl(EksekusiSQlSkalarNew("Select ((MLPBD.Qty*MLPBD.Konversi)-" & DiSTB() & ") AS QtySisa FROM MLPBD WHERE NoID=" & NullTolong(txtSTB.EditValue)))
        '    'If txtSTB.Text <> "" AndAlso (QtySisaSTB < txtQty.EditValue * txtKonversi.EditValue) Then
        '    '    XtraMessageBox.Show("Qty melebihi qty di penerimaan barang.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
        '    '    txtQty.Focus()
        '    '    Return False
        '    '    Exit Function
        '    'End If
        'End If
        Return True
    End Function
    Private Sub cmdTutup_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdTutup.Click
        DialogResult = Windows.Forms.DialogResult.Cancel
        Me.Close()
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
        'If Exists(folderLayouts &  Me.Name & gvPO.Name & ".xml") Then
        '    gvPO.RestoreLayoutFromXml(folderLayouts &  Me.Name & gvPO.Name & ".xml")
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
    Private Sub cmdSaveLayout_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles cmdSaveLayout.ItemClick
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
                gvBarang.SaveLayoutToXml(FolderLayouts & Me.Name & gvBarang.Name & ".xml")
                gvGudang.SaveLayoutToXml(folderLayouts & Me.Name & gvGudang.Name & ".xml")
                gvSatuan.SaveLayoutToXml(folderLayouts & Me.Name & gvSatuan.Name & ".xml")
                gvPO.SaveLayoutToXml(folderLayouts & Me.Name & gvPO.Name & ".xml")
                gvSTB.SaveLayoutToXml(folderLayouts & Me.Name & gvSTB.Name & ".xml")
            End If
        Catch ex As Exception
            XtraMessageBox.Show("Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            xOtorisasi.Dispose()
        End Try
    End Sub

    Private Sub frmEntriBeliD_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        Try
            If DialogResult = Windows.Forms.DialogResult.OK Then
                FormPemanggil.RefreshDetil()
                FormPemanggil.Show()
                FormPemanggil.Focus()
                FormPemanggil.SimpanTambahan()
                FormPemanggil.txtBarang.Focus()
            Else
                If IsFastEntri Then
                    EksekusiSQL("DELETE FROM MBeliD WHERE NoID=" & NoID)
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
            IsLoad = False
            Me.Text = "Entri Detil BPB"
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
    Private Sub HitungJumlah()
        Dim DS As New DataSet
        'Dim IsiKarton As Double = 0.0
        'Dim PPN As Double = 0.0
        Try
            'Dim SubTotal As Double = (Harga - txtDiscRp1.EditValue - txtDiscRp2.EditValue - txtDiscRp3.EditValue)
            'Dim DiscA As Double = Harga * txtQty.EditValue * txtDiscPersen1.EditValue / 100
            'Dim DiscB As Double = Harga * txtQty.EditValue * txtDiscPersen2.EditValue / 100
            'Dim DiscC As Double = Harga * txtQty.EditValue * txtDiscPersen3.EditValue / 100
            'txtHarga.EditValue = Harga
            'txtJUmlah.EditValue = (SubTotal * txtQty.EditValue) - DiscA - DiscB - DiscC
            'txtJUmlah.EditValue = txtQty.EditValue * ((txtHarga.EditValue - txtDiscRp1.EditValue - txtDiscRp2.EditValue - txtDiscRp3.EditValue) * (1 - (txtDiscPersen1.EditValue / 100)) * (1 - (txtDiscPersen2.EditValue / 100)) * (1 - (txtDiscPersen3.EditValue / 100)))

            'SQL = "SELECT MBarangD.Konversi, MBarang.CtnPcs,MBarang.PPN FROM MBarangD LEFT JOIN MBarang ON MBarang.NoID=MBarangD.IDBarang LEFT JOIN MSatuan ON MSatuan.NoID=MBarangD.IDSatuan WHERE MBarangD.IDBarang=" & NullToLong(txtBarang.EditValue) & " AND MSatuan.NoID=" & NullToLong(txtSatuan.EditValue)
            'Ds = ExecuteDataset("Tabel", SQL)
            'If Ds.Tables("Tabel").Rows.Count >= 1 Then
            '    txtKonversi.EditValue = NullToDbl(Ds.Tables(0).Rows(0).Item("Konversi"))
            '    Konversi = txtKonversi.EditValue
            '    IsiKarton = NullToDbl(DS.Tables(0).Rows(0).Item("CtnPcs"))
            '    PPN = NullToDbl(DS.Tables(0).Rows(0).Item("PPN"))

            '    If IsiKarton = 0 Then
            '        txtCtn.EditValue = 0
            '    Else
            '        txtCtn.EditValue = txtQty.EditValue / IsiKarton * txtKonversi.EditValue
            '    End If
            'End If
            'txtPPN.EditValue = Bulatkan(txtJUmlah.EditValue * PPN / 100, 0)
            'txtHargaBeli.EditValue = IIf(IDTypePPN = 2, 1.1, 1.0) * (Bulatkan(txtHargaSupplier.EditValue * (1 - (txtDiscPersen1.EditValue / 100)) * (1 - (txtDiscPersen2.EditValue / 100)) * (1 - (txtDiscPersen3.EditValue / 100)), 2) - txtDiscRp1.EditValue) / IIf(NullToDbl(txtKonversi.EditValue) = 0, 1, NullToDbl(txtKonversi.EditValue))
            'txtHargaBeli.EditValue = IIf(IDTypePPN = 2, 1.1, 1.0) * txtHargaBeli.EditValue / IIf(NullToDbl(txtKonversi.EditValue) = 0, 1, NullToDbl(txtKonversi.EditValue))

            'txtTotalBeli.EditValue = IIf(IDTypePPN = 2, 1.1, 1.0) * (Bulatkan(txtHargaSupplier.EditValue * (1 - (txtDiscPersen1.EditValue / 100)) * (1 - (txtDiscPersen2.EditValue / 100)) * (1 - (txtDiscPersen3.EditValue / 100)), 2) - txtDiscRp1.EditValue) * txtQty.EditValue
            'If txtPO.Text <> "" Then
            '    txtJumlah.EditValue = IIf(txtPPN.EditValue = 0, 1, txtPPN.EditValue) * (Bulatkan(txtHarga.EditValue * (1 - (txtDiscPersen1.EditValue / 100)) * (1 - (txtDiscPersen2.EditValue / 100)) * (1 - (txtDiscPersen3.EditValue / 100)), 2) - txtDiscRp1.EditValue) * txtQty.EditValue
            'Else
            '    txtJumlah.EditValue = Bulatkan(txtQty.EditValue * (Bulatkan((txtHarga.EditValue * (1 - (txtDiscPersen1.EditValue / 100)) * (1 - (txtDiscPersen2.EditValue / 100)) * (1 - (txtDiscPersen3.EditValue / 100))), 0) - txtDiscRp3.EditValue) * (1 + txtPPN.EditValue / 100), 0)
            'End If

            If txtPO.Text <> "" Then 'Perhitungan Sudah Include di Harga Jika Ada
                txtPPN.EditValue = 0
            Else
                txtPPN.EditValue = IIf(NullToLong(txtTypePajak.EditValue) = 2, 0.1, 0.0) * (Bulatkan(txtHarga.EditValue * (1 - (txtDiscPersen1.EditValue / 100)) * (1 - (txtDiscPersen2.EditValue / 100)) * (1 - (txtDiscPersen3.EditValue / 100)), 2) - txtDiscRp1.EditValue) * txtQty.EditValue
            End If
            txtJumlah.EditValue = IIf(NullToLong(txtTypePajak.EditValue) = 2, 1.1, 1.0) * (Bulatkan(txtHarga.EditValue * (1 - (txtDiscPersen1.EditValue / 100)) * (1 - (txtDiscPersen2.EditValue / 100)) * (1 - (txtDiscPersen3.EditValue / 100)), 2) - txtDiscRp1.EditValue) * txtQty.EditValue

            txtTotal.EditValue = txtJumlah.EditValue + txtBiaya.EditValue
            If (txtQty.EditValue * txtKonversi.EditValue) > 0 Then
                txtHargaBeliPcs.EditValue = txtTotal.EditValue / (txtQty.EditValue * txtKonversi.EditValue)
            Else
                txtHargaBeliPcs.EditValue = 0.0
            End If
            txtHitungHargaJual.EditValue = txtHargaBeliPcs.EditValue * (1.0 + txtMargin.EditValue / 100)
            'txtJumlah.EditValue.EditValue = txtJumlah.EditValue + txtBiaya.EditValue * txtQty.EditValue
            'txtHargaBeliPcs.EditValue = FixKoma((Bulatkan((txtHarga.EditValue * (1 - (txtDiscPersen1.EditValue / 100)) * (1 - (txtDiscPersen2.EditValue / 100)) * (1 - (txtDiscPersen3.EditValue / 100))), 0) - txtDiscRp1.EditValue - txtDiscRp2.EditValue - txtDiscRp3.EditValue))

            'txtJUmlah.EditValue = txtJUmlah.EditValue - (txtDiscPOBawah.EditValue / 100 * txtJUmlah.EditValue)
        Catch ex As Exception

        Finally
            DS.Dispose()
        End Try
    End Sub

    Private Sub txtQty_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtQty.KeyDown
        If e.KeyCode = Keys.F7 Then
            Dim x As New frmOtorisasiAdmin
            Try
                If txtQty.Properties.ReadOnly Then
                    If x.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
                        txtQty.Properties.ReadOnly = False
                    End If
                End If
            Catch ex As Exception
                XtraMessageBox.Show("Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End If
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

    Private Sub txtDisc1_EditValueChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtDiscRp1.EditValueChanged
        HitungJumlah()
    End Sub
    Private Sub txtDisc2_EditValueChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtDiscRp2.EditValueChanged
        HitungJumlah()
    End Sub
    Private Sub txtDisc3_EditValueChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtDiscRp3.EditValueChanged
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

    Private Sub txtBarang_ButtonClick(ByVal sender As Object, ByVal e As DevExpress.XtraEditors.Controls.ButtonPressedEventArgs) Handles txtBarang.ButtonClick
        If e.Button.Index = 1 Then
            RefreshKodeBarang(True)
        End If
    End Sub

    Private Sub txtBarang_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtBarang.EditValueChanged
        Dim Ds As New DataSet
        Try
            SQL = "SELECT MBarang.Nama AS NamaBarang, MBarang.NamaAlias FROM MBarang WHERE MBarang.NoID=" & NullToLong(txtBarang.EditValue)
            Ds = ExecuteDataset("Tabel", SQL)
            If Ds.Tables("Tabel").Rows.Count >= 1 Then
                'txtSatuan.EditValue = NullTolong(Ds.Tables(0).Rows(0).Item("IDSatuan"))
                'txtKonversi.EditValue = NullToDbl(Ds.Tables(0).Rows(0).Item("Konversi"))
                txtNamaStock.Text = NullToStr(Ds.Tables(0).Rows(0).Item("NamaBarang"))
                txtNamaAlias.Text = NullToStr(Ds.Tables(0).Rows(0).Item("NamaAlias"))
                RefreshLookUpSatuan()
                txtSatuan.EditValue = DefIDSatuan
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

    Private Sub txtSatuan_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtSatuan.EditValueChanged
        RubahSatuan()
    End Sub
    Private Sub RubahSatuan()
        Dim Ds As New DataSet
        Dim IsiKarton As Double = 0.0
        Try
            If Not FormPemanggil Is Nothing AndAlso Not FormPemanggil.IsRevisi Then
                SQL = "Select distinct * from ("
                SQL = SQL & "SELECT MSatuan.NoID, MSatuan.Kode, MSatuan.Nama,Mbarang.CtnPcs AS Isi FROM MSatuan inner join mbarang on MBarang.IDSatuanHarga=Msatuan.NoID where MBarang.NoID=" & NullToLong(txtBarang.EditValue) & " "
                SQL = SQL & " UNION ALL SELECT MSatuan.NoID, MSatuan.Kode, MSatuan.Nama,1.0 AS Isi FROM MSatuan inner join mbarang on MBarang.IDSatuan=Msatuan.NoID where MBarang.NoID=" & NullToLong(txtBarang.EditValue)
                SQL = SQL & " UNION ALL SELECT MSatuan.NoID, MSatuan.Kode, MSatuan.Nama,MBarangd.Konversi AS Isi FROM MSatuan inner join mbarangd on MBarangd.IDSatuan=Msatuan.NoID where MBarangd.IDBarang=" & NullToLong(txtBarang.EditValue) & " ) X "

                SQL = "select Isi from (" & SQL & ") Z where NoID=" & NullToLong(txtSatuan.EditValue)
                txtKonversi.EditValue = NullToDbl(EksekusiSQlSkalarNew(SQL))
                SQL = "SELECT (MBarang.HargaBeliPcsBruto) AS HargaBeliPcsBruto, (MBarang.HargaBeli) AS HargaBeli,MBarang.HargaJual,MBarang.CtnPcs,MBarang.DiscBeli1,MBarang.DiscBeli2,MBarang.DiscBeli3,Mbarang.DiscRp From Mbarang where NoID=" & NullToLong(txtBarang.EditValue)
                Ds = ExecuteDataset("TabelBarang", SQL)
                If Ds.Tables("TabelBarang").Rows.Count >= 1 Then
                    'Konversi 1 dianggap harga PCs
                    'If NullToDbl(txtKonversi.EditValue) = 1.0 Then 'NullToDbl(Ds.Tables("TabelBarang").Rows(0).Item("CtnPcs")) Then
                    If txtPO.Text = "" Then
                        txtHarga.EditValue = NullToDbl(Ds.Tables("TabelBarang").Rows(0).Item("HargaBeliPcsBruto")) * NullToDbl(txtKonversi.EditValue)
                        'HargaSupplier = txtHargaSupplier.EditValue / IIf(NullToDbl(txtKonversi.EditValue) = 0, 1, NullToDbl(txtKonversi.EditValue))
                        txtDiscPersen1.EditValue = NullToDbl(Ds.Tables("TabelBarang").Rows(0).Item("DiscBeli1"))
                        txtDiscPersen2.EditValue = NullToDbl(Ds.Tables("TabelBarang").Rows(0).Item("DiscBeli2"))
                        txtDiscPersen3.EditValue = NullToDbl(Ds.Tables("TabelBarang").Rows(0).Item("DiscBeli3"))
                        txtDiscRp1.EditValue = NullToDbl(Ds.Tables("TabelBarang").Rows(0).Item("DiscRp"))
                        txtHargaJual.EditValue = NullToDbl(Ds.Tables("TabelBarang").Rows(0).Item("HargaJual"))
                    End If
                    'HargaJual = txtHargaJual.EditValue
                End If
                ' SQL = "SELECT MBarangD.Konversi, MBarang.CtnPcs FROM MBarangD LEFT JOIN MBarang ON MBarang.NoID=MBarangD.IDBarang LEFT JOIN MSatuan ON MSatuan.NoID=MBarangD.IDSatuan WHERE MBarangD.IDBarang=" & NullToLong(txtBarang.EditValue) & " AND MSatuan.NoID=" & NullToLong(txtSatuan.EditValue)
                'Ds = ExecuteDataset("Tabel", SQL)
                'If Ds.Tables("Tabel").Rows.Count >= 1 Then
                '    txtKonversi.EditValue = NullToDbl(Ds.Tables(0).Rows(0).Item("Konversi"))
                '    Konversi = txtKonversi.EditValue
                '    IsiKarton = NullToDbl(Ds.Tables(0).Rows(0).Item("CtnPcs"))
                '    If IsiKarton = 0 Then
                '        txtCtn.EditValue = 0
                '    Else
                '        txtCtn.EditValue = txtQty.EditValue / IsiKarton * txtKonversi.EditValue
                '    End If
                'If txtPO.Text <> "" AndAlso (IsNew Or IsFastEntri) Then
                '    txtHarga.EditValue = HargaPcs * Konversi
                'ElseIf txtHarga.EditValue = 0 Or IsNew Or IsFastEntri Then
                '    'txtHarga.EditValue = NullToDbl(EksekusiSQlSkalarNew("SELECT TOP 1 MBeliD.Harga/MBeliD.Konversi FROM MBeliD INNER JOIN MBeli ON MBeli.NoID=MBeliD.IDBeli WHERE IsNull(MBeliD.Harga,0)<>0 AND MBeli.IDSupplier=" & IDSupplier & " AND MBeliD.IDBarang=" & NullToLong(txtBarang.EditValue) & " Order By MBeli.Tanggal Desc, MBeliD.Tgl Desc")) * IIf(txtKonversi.EditValue = 0, 1, txtKonversi.EditValue)
                '    txtHarga.EditValue = System.Math.Round(clsPostingPembelian.GetHargaBeliterakhir(NullToLong(txtBarang.EditValue))) * txtKonversi.EditValue
                'End If
                'End If
            End If
            HitungJumlah()
        Catch ex As Exception
            XtraMessageBox.Show("Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            Ds.Dispose()
        End Try
    End Sub
    Private Sub BarButtonItem2_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem2.ItemClick
        cmdTutup.PerformClick()
    End Sub

    Private Sub txtPO_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtPO.Click

    End Sub

    'Private Sub txtPO_ButtonClick(ByVal sender As Object, ByVal e As DevExpress.XtraEditors.Controls.ButtonPressedEventArgs) Handles txtPO.ButtonClick
    '    'If System.IO.File.Exists(folderLayouts &  Me.Name & gvPO.Name & ".xml") Then
    '    '    gvPO.RestoreLayoutFromXml(folderLayouts &  Me.Name & gvPO.Name & ".xml")
    '    'End If
    '    With gvPO
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

    Private Sub txtPO_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtPO.EditValueChanged
        Dim Ds As New DataSet
        Try
            If IsNew Or IsFastEntri Then
                SQL = "SELECT MPO.DiskonNotaProsen, MPOD.*, MPO.DiskonNotaProsen, (MPOD.Qty*MPOD.Konversi)-" & DiBeli() & " AS QtySisa, MBarang.Kode AS KodeBarang, MBarang.Nama AS NamaBarang, MGudang.Kode AS KodeGudang, MGudang.Nama AS NamaGudang, MSatuan.Nama AS NamaSatuan "
                SQL &= " FROM (((MPOD INNER JOIN MPO ON MPO.NoID=MPOD.IDPO) LEFT JOIN MGudang ON MGudang.NoID=MPOD.IDGudang) "
                SQL &= " LEFT JOIN MBarang ON MBarang.NoID=MPOD.IDBarang)"
                SQL &= " LEFT JOIN MSatuan ON MSatuan.NoID=MPOD.IDSatuan "
                SQL &= " WHERE MPOD.NoID= " & NullToLong(txtPO.EditValue)
                Ds = New DataSet()
                Ds = ExecuteDataset("MPOD", SQL)
                If Ds.Tables(0).Rows.Count >= 1 Then
                    With Ds.Tables("MPOD").Rows(0)
                        'txtBarang.EditValue = NullTolong(.Item("IDBarang"))
                        'txtBarang.Text = NullTostr(.Item("KodeBarang"))
                        txtSatuan.EditValue = NullToLong(.Item("IDSatuan"))
                        'txtSatuan.Text = NullTostr(.Item("KodeSatuan"))
                        'txtGudang.EditValue = NullTolong(.Item("IDGudang"))
                        'RubahGudang()
                        'txtGudang.Text = NullTostr(.Item("KodeGudang"))
                        QtySisa = NullToDbl(.Item("QtySisa"))
                        Konversi = IIf(NullToDbl(.Item("Konversi")) = 0, 1, NullToDbl(.Item("Konversi")))
                        'HargaPcs = (NullToDbl(.Item("Harga")) / Konversi) - ((NullToDbl(.Item("Harga")) / Konversi) * (NullToDbl(.Item("DiskonNotaProsen")) / 100))
                        HargaPcs = NullToDbl(.Item("Harga")) / Konversi

                        txtQty.EditValue = NullToDbl(.Item("QtySisa")) / Konversi
                        'txtHarga.EditValue = NullToDbl(.Item("Harga")) - ((NullToDbl(.Item("Harga"))) * (NullToDbl(.Item("DiskonNotaProsen"))) / 100)
                        txtHarga.EditValue = NullToDbl(.Item("Harga"))
                        'txtDiscPOBawah.EditValue = NullToDbl(.Item("DiskonNotaProsen"))
                        txtCtn.EditValue = NullToDbl(.Item("CTN"))
                        txtDiscPersen1.EditValue = NullToDbl(.Item("DiscPersen1"))
                        txtDiscPersen2.EditValue = NullToDbl(.Item("DiscPersen2"))
                        txtDiscPersen3.EditValue = NullToDbl(.Item("DiscPersen3"))
                        txtDiscRp1.EditValue = NullToDbl(.Item("Disc1"))
                        txtDiscRp2.EditValue = NullToDbl(.Item("Disc2"))
                        txtDiscRp3.EditValue = NullToDbl(.Item("Disc3"))
                        txtJumlah.EditValue = NullToDbl(.Item("Jumlah"))
                        txtCatatan.Text = NullToStr(.Item("Catatan"))
                        txtKonversi.EditValue = Konversi
                        KonversiPO = Konversi
                        RefreshLookUpSTB()
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

    Private Sub txtHarga_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtHarga.LostFocus
        HitungJumlah()
    End Sub

    Private Sub BarButtonItem1_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem1.ItemClick
        cmdSave.PerformClick()
    End Sub

    'Private Sub txtPO_EditValueChanging(ByVal sender As Object, ByVal e As DevExpress.XtraEditors.Controls.ChangingEventArgs) Handles txtPO.EditValueChanging
    '    'If System.IO.File.Exists(folderLayouts &  Me.Name & gvPO.Name & ".xml") Then
    '    '    gvPO.RestoreLayoutFromXml(folderLayouts &  Me.Name & gvPO.Name & ".xml")
    '    'End If
    '    With gvPO
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

    Private Sub gvPO_DataSourceChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvPO.DataSourceChanged
        If System.IO.File.Exists(folderLayouts & Me.Name & gvPO.Name & ".xml") Then
            gvPO.RestoreLayoutFromXml(folderLayouts & Me.Name & gvPO.Name & ".xml")
        End If
        With gvPO
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

    'Private Sub txtSTB_ButtonClick(ByVal sender As Object, ByVal e As DevExpress.XtraEditors.Controls.ButtonPressedEventArgs) Handles txtSTB.ButtonClick
    '    'If System.IO.File.Exists(folderLayouts &  Me.Name & gvSTB.Name & ".xml") Then
    '    '    gvSTB.RestoreLayoutFromXml(folderLayouts &  Me.Name & gvSTB.Name & ".xml")
    '    'End If
    '    With gvSTB
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

    Private Sub txtSTB_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtSTB.EditValueChanged
        Dim Ds As New DataSet
        Try
            If IsNew Or IsFastEntri Then
                SQL = "SELECT MLPBD.NoID, MLPB.Kode, MLPB.Tanggal, MAlamat.Nama AS Supplier, MBagPembelian.Nama AS Penerima, MLPBD.Qty, MLPBD.Konversi, MLPBD.IDSatuan, MLPBD.Qty * MLPBD.Konversi AS [QtyPcs], MLPBD.Qty*MLPBD.Konversi-" & DiSTB() & "  AS [QtySisa] "
                SQL &= " FROM MLPBD LEFT OUTER JOIN (MLPB LEFT OUTER JOIN MAlamat AS MBagPembelian ON MBagPembelian.NoID = MLPB.IDBagPembelian "
                SQL &= " LEFT OUTER JOIN MAlamat ON MLPB.IDSupplier = MAlamat.NoID)"
                SQL &= " ON MLPB.NoID=MLPBD.IDLPB "
                SQL &= " WHERE MLPBD.NoID= " & NullToLong(txtSTB.EditValue)
                Ds = New DataSet()
                Ds = ExecuteDataset("MLPBD", SQL)
                If Ds.Tables(0).Rows.Count >= 1 Then
                    With Ds.Tables("MLPBD").Rows(0)
                        QtySisaSTB = NullToDbl(.Item("QtySisa"))
                        Konversi = IIf(NullToDbl(.Item("Konversi")) = 0, 1, NullToDbl(.Item("Konversi")))
                        txtQty.EditValue = NullToDbl(.Item("QtySisa")) / Konversi
                        txtSatuan.EditValue = NullToLong(.Item("IDSatuan"))
                        If txtPO.Text <> "" Then
                            txtHarga.EditValue = HargaPcs * Konversi
                        End If
                    End With
                Else
                    'IsiDefault()
                End If
                'If Not IsLoad AndAlso txtSTB.Text <> "" Then
                '    txtSatuan.Properties.ReadOnly = True
                '    txtQty.Properties.ReadOnly = True
                'Else
                '    txtSatuan.Properties.ReadOnly = False
                '    txtQty.Properties.ReadOnly = False
                'End If
            End If
        Catch ex As Exception
            XtraMessageBox.Show("Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            Ds.Dispose()
        End Try
    End Sub

    'Private Sub txtSTB_EditValueChanging(ByVal sender As Object, ByVal e As DevExpress.XtraEditors.Controls.ChangingEventArgs) Handles txtSTB.EditValueChanging
    '    If System.IO.File.Exists(folderLayouts &  Me.Name & gvSTB.Name & ".xml") Then
    '        gvSTB.RestoreLayoutFromXml(folderLayouts &  Me.Name & gvSTB.Name & ".xml")
    '    End If
    '    With gvSTB
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

    Private Sub gvSTB_DataSourceChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvSTB.DataSourceChanged
        If System.IO.File.Exists(folderLayouts & Me.Name & gvSTB.Name & ".xml") Then
            gvSTB.RestoreLayoutFromXml(folderLayouts & Me.Name & gvSTB.Name & ".xml")
        End If
        With gvSTB
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
            SQL = "SELECT MWilayah.Nama AS Wilayah FROM MGudang LEFT JOIN MWilayah ON MWilayah.NoID=MGudang.IDWilayah WHERE MGudang.NOID=" & NullTolong(txtGudang.EditValue)
            Ds = ExecuteDataset("Tabel", SQL)
            If Ds.Tables("Tabel").Rows.Count >= 1 Then
                txtWilayah.Text = NullTostr(Ds.Tables(0).Rows(0).Item("Wilayah"))
            End If
        Catch ex As Exception
            XtraMessageBox.Show("Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            RefreshLookUpSTB()
            Ds.Dispose()
        End Try
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

    Private Sub txtKonversi_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtKonversi.LostFocus
        HitungJumlah()
    End Sub

    Private Sub txtGudang_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtGudang.LostFocus
        RubahGudang()
    End Sub

    Private Sub txtSatuan_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtSatuan.KeyDown
        If e.KeyCode = Keys.F7 Then
            Dim x As New frmOtorisasiAdmin
            Try
                If txtSatuan.Properties.ReadOnly Then
                    If x.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
                        txtSatuan.Properties.ReadOnly = False
                    End If
                End If
            Catch ex As Exception
                XtraMessageBox.Show("Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End If
    End Sub
    Private Sub txtSatuan_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtSatuan.LostFocus
        RubahSatuan()
        'Dim Harga As Double = 0.0
        'If txtPO.Text <> "" Then
        '    Harga = IIf(HargaPcs = 0, txtHarga.EditValue, HargaPcs) * IIf(txtKonversi.EditValue = 0, 1, txtKonversi.EditValue)
        'Else
        '    Harga = txtHarga.EditValue
        'End If
    End Sub

    Private Sub txtQty_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtQty.EditValueChanged
        HitungJumlah()
    End Sub

    Private Sub txtKonversi_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtKonversi.EditValueChanged

    End Sub

    Private Sub txtHarga_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtHarga.EditValueChanged
        HitungJumlah()
    End Sub

    Private Sub txtBarcode_ButtonClick(ByVal sender As Object, ByVal e As DevExpress.XtraEditors.Controls.ButtonPressedEventArgs) Handles txtBarcode.ButtonClick
        If e.Button.Index = 1 Then
            RefreshBarcode(True)
        End If
    End Sub

    Private Sub txtBarcode_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtBarcode.EditValueChanged
        Dim Ds As New DataSet
        Try
            SQL = "SELECT MBarangD.NoID,MBarangD.Varian, MBarang.ProsenUp, MBarang.HargaBeli, MBarang.HargaJual, MBarang.DiscBeli1, MBarang.DiscBeli2, MBarang.DiscBeli3, MBarang.DiscRp, MBarang.CtnPcs, MBarang.Konversi, MBarangD.IDBarang,MBarangD.IDSatuan,MBarangD.Konversi AS KonversiBarang, MBarang.Kode,MBarang.IDSatuanHarga IDSatuanBase,MBarang.Nama AS NamaBarang, MBarang.NamaAlias " & _
            "FROM MBarangD inner join MBarang On MBarangD.IDbarang=MBarang.NoID WHERE MBarangD.NoID=" & NullToLong(txtBarcode.EditValue)
            Ds = ExecuteDataset("Tabel", SQL)
            If Ds.Tables("Tabel").Rows.Count >= 1 Then
                'txtSatuan.EditValue = NullTolong(Ds.Tables(0).Rows(0).Item("IDSatuan"))
                'txtKonversi.EditValue = NullToDbl(Ds.Tables(0).Rows(0).Item("Konversi"))
                txtBarang.EditValue = NullToLong(Ds.Tables(0).Rows(0).Item("IDBarang"))
                txtVarian.Text = NullToStr(Ds.Tables(0).Rows(0).Item("Varian"))
                txtNamaStock.Text = NullToStr(Ds.Tables(0).Rows(0).Item("NamaBarang"))
                txtNamaAlias.Text = NullToStr(Ds.Tables(0).Rows(0).Item("NamaAlias"))

                RefreshLookUpSatuan()
                txtSatuan.EditValue = NullToLong(Ds.Tables(0).Rows(0).Item("IDSatuanBase")) 'DefIDSatuan
                txtKonversi.EditValue = NullToDbl(Ds.Tables(0).Rows(0).Item("CtnPcs"))
                'RubahSatuan()
                RefreshLookUpSTB()
                RefreshLookUpPO()

                txtDiscPersen1.EditValue = NullToDbl(Ds.Tables(0).Rows(0).Item("DiscBeli1"))
                txtDiscPersen2.EditValue = NullToDbl(Ds.Tables(0).Rows(0).Item("DiscBeli2"))
                txtDiscPersen3.EditValue = NullToDbl(Ds.Tables(0).Rows(0).Item("DiscBeli3"))
                txtDiscRp1.EditValue = NullToDbl(Ds.Tables(0).Rows(0).Item("DiscRp"))
                txtHarga.EditValue = NullToDbl(Ds.Tables(0).Rows(0).Item("HargaBeli"))
                txtMargin.EditValue = NullToDbl(Ds.Tables(0).Rows(0).Item("ProsenUp"))

                HitungJumlah()
            End If
        Catch ex As Exception
            XtraMessageBox.Show("Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            'RefreshLookUpSTB()
            Ds.Dispose()
        End Try
    End Sub

    Private Sub txtBiaya_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtBiaya.EditValueChanged
        HitungJumlah()
    End Sub

    Private Sub txtMargin_EditValueChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtMargin.EditValueChanged
        HitungJumlah()
    End Sub

    Private Sub txtMargin_EditValueChanging(ByVal sender As Object, ByVal e As DevExpress.XtraEditors.Controls.ChangingEventArgs) Handles txtMargin.EditValueChanging

    End Sub

    Private Sub txtPPN_EditValueChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtPPN.EditValueChanged
        HitungJumlah()
    End Sub

    Private Sub frmEntriBeliDDel_Shown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Shown
        If IsFastEntri Then
            txtQty.Focus()
        End If
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

    Private Sub txtTypePajak_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtTypePajak.EditValueChanged
        HitungJumlah()
    End Sub
End Class