Imports System.Data
Imports System.Data.SqlClient
Imports DevExpress.XtraEditors
Imports DevExpress.Utils
Imports System.IO.File
Public Class frmEntriJualD
    Public IsNew As Boolean = True
    Public IsPosted As Boolean = False
    Public NoID As Long = -1
    Public IDJual As Long = -1
    Public IDCustomer As Long = -1
    Public IDPacking As Long = -1
    'Public IDGudang As Long = DefIDGudang
    Public IsFastEntri As Boolean = False

    'Dim QtySisa As Double = 0.0
    Dim oDS As New DataSet
    Dim SQL As String = ""
    Dim DefImageList As ImageList
    Dim HargaPcs As Double = 0.0
    Dim Konversi As Double = 0.0
    Public FormPemanggil As frmEntriJual

    Private Sub IsiDefault()
        DefImageList = frmMain.ImageList1
        cmdSave.ImageList = DefImageList
        cmdSave.ImageIndex = 6

        cmdTutup.ImageList = DefImageList
        cmdTutup.ImageIndex = 3
        RefreshLookUp()
        Konversi = 1.0
        HargaPcs = 0.0
        txtGudang.EditValue = DefIDGudang
        RubahGudang()
    End Sub
    Private Function DiJual() As String
        Return "isnull((SELECT SUM(isnull(A.Qty,0)*isnull(A.Konversi,0)) FROM MJualD A WHERE A.NoID <> " & NoID & " AND A.IDPackingD=MPackingD.NoID),0)"
    End Function
    Private Sub RefreshLookUp()
        Dim ds As New DataSet
        Try
            RefreshBarcode(False)
            RefreshKodeBarang(False)
            SQL = "SELECT MGudang.NoID, MGudang.Kode, MGudang.Nama, MWilayah.Nama AS Wilayah FROM MGudang LEFT JOIN MWilayah ON MWilayah.NoID=MGudang.IDWilayah WHERE MGudang.IsActive=1 AND MGudang.IsBS=0 "
            ds = ExecuteDataset("MGudang", SQL)
            txtGudang.Properties.DataSource = ds.Tables("MGudang")
            txtGudang.Properties.ValueMember = "NoID"
            txtGudang.Properties.DisplayMember = "Nama"
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

            'SQL = "SELECT MBarang.NoID, MBarang.Kode, MBarang.Nama, MSatuan.Nama AS Satuan FROM MBarang LEFT JOIN MSatuan ON MSatuan.NoID=MBarang.IDSatuan WHERE MBarang.IsActive=1 AND NOT (MBarang.KODE='' OR MBarang.KODE IS NULL)"
            'ds = ExecuteDataset("MBarang", SQL)
            'txtBarang.Properties.DataSource = ds.Tables("MBarang")
            'txtBarang.Properties.ValueMember = "NoID"
            'txtBarang.Properties.DisplayMember = "Kode"

            SQL = "SELECT NoID, Kode, Nama FROM MAlamat WHERE IsActive=1 AND IsPegawai=1"
            ds = ExecuteDataset("master", SQL)
            txtSalesman.Properties.DataSource = ds.Tables("master")
            txtSalesman.Properties.ValueMember = "NoID"
            txtSalesman.Properties.DisplayMember = "Nama"
            If System.IO.File.Exists(FolderLayouts & Me.Name & gvSalesman.Name & ".xml") Then
                gvSalesman.RestoreLayoutFromXml(FolderLayouts & Me.Name & gvSalesman.Name & ".xml")
            End If

            'SQL = "SELECT MSatuan.NoID, MSatuan.Kode, MSatuan.Nama FROM MSatuan WHERE MSatuan.IsActive=1 AND MSatuan.NoID IN (SELECT MBarangD.IDSatuan FROM MBarangD WHERE MBarangD.IsJual=1 AND MBarangD.IsActive=1 AND MBarangD.IDBarang=" & NullTolong(txtBarang.EditValue) & ")"
            'ds = ExecuteDataset("MSatuan", SQL)
            'txtSatuan.Properties.DataSource = ds.Tables("MSatuan")
            'txtSatuan.Properties.ValueMember = "NoID"
            'txtSatuan.Properties.DisplayMember = "Kode"
            'If System.IO.File.Exists(folderLayouts &  Me.Name & gvSatuan.Name & ".xml") Then
            '    gvSatuan.RestoreLayoutFromXml(folderLayouts &  Me.Name & gvSatuan.Name & ".xml")
            'End If
            'With gvSatuan
            '    For x As Integer = 0 To .Columns.Count - 1
            '        Select Case .Columns(x).ColumnType.Name.ToLower
            '            Case "int32", "int64", "int"
            '                .Columns(x).DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
            '                .Columns(x).DisplayFormat.FormatString = "n0"
            '            Case "decimal", "single", "money", "double"
            '                .Columns(x).DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
            '                .Columns(x).DisplayFormat.FormatString = "n2"
            '            Case "string"
            '                .Columns(x).DisplayFormat.FormatType = DevExpress.Utils.FormatType.None
            '                .Columns(x).DisplayFormat.FormatString = ""
            '            Case "date", "datetime"
            '                If .Columns(x).FieldName.Trim.ToLower = "jam" Then
            '                    .Columns(x).DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime
            '                    .Columns(x).DisplayFormat.FormatString = "HH:mm"
            '                Else
            '                    .Columns(x).DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime
            '                    .Columns(x).DisplayFormat.FormatString = "dd-MM-yyyy"
            '                End If
            '        End Select
            '    Next
            'End With

            'SQL = "SELECT MBarangD.NoID,MBarangD.Barcode,MBarang.Kode,MBarang.Nama,MSatuan.Kode as Satuan,MBarangD.IDSatuan,MBarangD.IDBarang from MbarangD inner join MBarang On MBarangD.IDBarang=MBarang.NoID inner join MSatuan On MBarangD.IDSatuan=MSatuan.NoID"
            'ds = ExecuteDataset("MBarangD", SQL)
            'txtBarcode.Properties.DataSource = ds.Tables("MBarangD")
            'txtBarcode.Properties.ValueMember = "NoID"
            'txtBarcode.Properties.DisplayMember = "Barcode"
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
        SQL = "SELECT MBarangD.NoID,MBarangD.Barcode,MBarang.Kode,MBarang.Nama + ISNULL(MBarangD.Varian,'') AS Nama,MMerk.Nama as Merk, MSatuan.Kode as Satuan,MBarangD.IDSatuan,MBarangD.IDBarang from MbarangD inner join MBarang On MBarangD.IDBarang=MBarang.NoID inner join MSatuan On MBarangD.IDSatuan=MSatuan.NoID Left Join MMerk on MBarang.IDMerk=MMerk.NoID where MBarangD.Isactive=1 and MBarang.IsActive=1"
        dsPublic = ExecuteDataset("MBarangD", SQL)
        txtBarcode.Properties.DataSource = DSPublic.Tables("MBarangD")
        txtBarcode.Properties.ValueMember = "NoID"
        txtBarcode.Properties.DisplayMember = "Barcode"
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
    Sub RefreshKodeBarang(ByVal IsForce As Boolean)
        Dim dsPublicKodeBarang As DataSet

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
        SQL = "SELECT MBarang.NoID, (CASE WHEN ISNULL(MBarang.Kode,'')='' THEN MBarang.KodeAlias ELSE MBarang.Kode END) AS Kode,MBarang.Barcode,MBarang.HPP, MBarang.Nama, MBarang.NamaAlias, MSatuan.Nama AS Satuan FROM MBarang LEFT JOIN MSatuan ON MSatuan.NoID=MBarang.IDSatuan WHERE MBarang.IsActive=1 AND NOT ((MBarang.KODE='' OR MBarang.KODE IS NULL) AND (MBarang.KodeAlias='' OR MBarang.KodeAlias IS NULL))"
        dsPublicKodeBarang = ExecuteDataset("MBarang", SQL)
        txtBarang.Properties.DataSource = DSPublicKodeBarang.Tables("MBarang")
        txtBarang.Properties.ValueMember = "NoID"
        txtBarang.Properties.DisplayMember = "Kode"
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
    Private Sub RefreshLookUpPacking()
        Dim ds As New DataSet
        Try
            SQL = "SELECT MPackingD.NoID, MPacking.Kode, MGudang.Nama AS Gudang, MWilayah.Nama AS Wilayah, MBarang.Kode AS KodeBarang, MBarang.Nama AS NamaBarang, " & vbCrLf & _
                  " MPackingD.Qty, MSatuan.Nama AS Satuan, MPackingD.Qty*MPackingD.Konversi AS [Qty(Pcs)], MPackingD.Qty*MPackingD.Konversi- " & DiJual() & "  AS [Sisa(Pcs)]" & vbCrLf & _
                  " FROM MPackingD LEFT JOIN MSatuan ON MSatuan.NoID=MPackingD.IDSatuan " & vbCrLf & _
                  " LEFT JOIN MPacking ON MPacking.NoID=MPackingD.IDPacking " & vbCrLf & _
                  " LEFT JOIN (MGudang LEFT JOIN MWilayah ON MWilayah.NoID=MGudang.IDWilayah) ON MGudang.NoID=MPackingD.IDGudang " & vbCrLf & _
                  " LEFT JOIN MBarang ON MBarang.NoID=MPackingD.IDBarang " & vbCrLf & _
                  " WHERE (MPacking.IsSelesai=0 OR MPacking.IsSelesai IS NULL) AND MPackingD.IsPacking=1 AND (MPackingD.Qty*MPackingD.Konversi- " & DiJual() & ">0) AND MBarang.IsActive = 1 And MPacking.IsPosted = 1"
            ds = ExecuteDataset("MPackingD", SQL)
            txtPacking.Properties.DataSource = ds.Tables("MPackingD")
            txtPacking.Properties.ValueMember = "NoID"
            txtPacking.Properties.DisplayMember = "Kode"
        Catch ex As Exception
            XtraMessageBox.Show("Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            ds.Dispose()
        End Try
    End Sub
    Private Sub LoadData()
        Try
            SQL = "SELECT MJualD.*, MPackingD.Konversi AS KonversiAsal, (MPackingD.Harga/MPackingD.Konversi) AS HargaAsal, (MPackingD.Qty*MPackingD.Konversi) AS QtyAsal, MBarang.Kode AS KodeBarang, MBarang.Nama AS NamaBarang, MGudang.Kode AS KodeGudang, MGudang.Nama AS NamaGudang, MSatuan.Nama AS NamaSatuan "
            SQL &= " FROM (((MJualD LEFT JOIN MGudang ON MGudang.NoID=MJualD.IDGudang) "
            SQL &= " LEFT JOIN MBarang ON MBarang.NoID=MJualD.IDBarang)"
            SQL &= " LEFT JOIN MPackingD ON MPackingD.NoID=MJualD.IDPackingD)"
            SQL &= " LEFT JOIN MSatuan ON MSatuan.NoID=MJualD.IDSatuan "
            SQL &= " WHERE MJualD.NoID= " & NoID
            oDS = New DataSet()
            oDS = ExecuteDataset("MJualD", SQL)
            If oDS.Tables(0).Rows.Count >= 1 Then
                With oDS.Tables("MJualD").Rows(0)
                    NoID = NullToLong(.Item("NoID"))
                    IDJual = NullToLong(.Item("IDJual"))
                    txtBarcode.EditValue = NullToLong(.Item("IDBarangD"))
                    txtBarang.EditValue = NullToLong(.Item("IDBarang"))
                    txtPacking.Text = NullToStr(.Item("IDPackingD"))
                    txtSatuan.EditValue = NullToLong(.Item("IDSatuan"))
                    'txtSatuan.Text = NullTostr(.Item("KodeSatuan"))
                    txtGudang.EditValue = NullToLong(.Item("IDGudang"))
                    'txtGudang.EditValue = IDGudang
                    'txtGudang.Text = NullTostr(.Item("KodeGudang"))
                    txtQty.EditValue = NullToDbl(.Item("Qty"))
                    HargaPcs = NullToDbl(.Item("HargaAsal"))
                    Konversi = NullToDbl(.Item("KonversiAsal"))
                    txtHarga.EditValue = NullToDbl(.Item("Harga"))
                    txtCtn.EditValue = NullToDbl(.Item("CTN"))
                    txtDiscPersen1.EditValue = NullToDbl(.Item("DiscPersen1"))
                    txtDiscPersen2.EditValue = NullToDbl(.Item("DiscPersen2"))
                    txtDiscPersen3.EditValue = NullToDbl(.Item("DiscPersen3"))
                    txtDiscRp1.EditValue = NullToDbl(.Item("Disc1"))
                    txtDiscRp2.EditValue = NullToDbl(.Item("Disc2"))
                    txtDiscRp3.EditValue = NullToDbl(.Item("Disc3"))
                    txtJUmlah.EditValue = NullToDbl(.Item("Jumlah"))
                    txtPPN.EditValue = NullToDbl(.Item("PPN"))
                    txtCatatan.Text = NullToStr(.Item("Catatan"))
                    txtKonversi.EditValue = NullToDbl(.Item("Konversi"))
                    'RefreshLookUpSTB()
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
                SQL = "INSERT INTO MJualD (IDSalesman,NoID,IDJual,IDPackingD,NoUrut,Tgl,Jam,IDBarangD,IDBarang,IDSatuan,Qty,QtyPcs,Harga,HargaPcs,CTN,DiscPersen1,DiscPersen2,DiscPersen3,Disc1,Disc2,Disc3,Jumlah,PPN,Catatan,IDGudang,Konversi,HargaPokok) VALUES ("
                SQL &= NullToLong(txtSalesman.EditValue) & ","
                SQL &= NullToLong(GetNewID("MJualD", "NoID")) & ","
                SQL &= IDJual & ","
                SQL &= NullToLong(txtPacking.EditValue) & ","
                SQL &= GetNewID("MJualD", "NoUrut", " WHERE IDJual=" & IDJual) & ","
                SQL &= "GetDate(),"
                SQL &= "GetDate(),"
                SQL &= NullToLong(txtBarcode.EditValue) & ","
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
                SQL &= FixKoma(txtPPN.EditValue) & ","
                SQL &= "'" & FixApostropi(txtCatatan.Text) & "',"
                SQL &= NullToLong(txtGudang.EditValue) & ","
                SQL &= FixKoma(txtKonversi.EditValue) & ","
                SQL &= FixKoma(EksekusiSQlSkalarNew("SELECT MBarang.HargaBeliPcs FROM MBarang WHERE MBarang.NoID=" & NullToLong(txtBarang.EditValue)))
                SQL &= ")"
            Else
                SQL = "UPDATE MJualD SET "
                SQL &= " IDSalesman=" & NullToLong(txtSalesman.EditValue) & ","
                SQL &= " Tgl=GetDate(),"
                SQL &= " Jam=GetDate(),"
                SQL &= " IDJual=" & IDJual & ","
                SQL &= " IDPackingD=" & NullToLong(txtPacking.EditValue) & ","
                SQL &= " IDBarangD=" & NullToLong(txtBarcode.EditValue) & ","
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
                SQL &= " PPN=" & FixKoma(txtPPN.EditValue) & ","
                SQL &= " Catatan='" & FixApostropi(txtCatatan.Text) & "',"
                SQL &= " IDGudang=" & NullToLong(txtGudang.EditValue) & ","
                SQL &= " Konversi=" & FixKoma(txtKonversi.EditValue) & ","
                SQL &= " HargaPokok=" & FixKoma(EksekusiSQlSkalarNew("SELECT MBarang.HargaBeliPcs FROM MBarang WHERE MBarang.NoID=" & NullToLong(txtBarang.EditValue)))
                SQL &= " WHERE NoID=" & NoID
            End If
            If EksekusiSQL(SQL) >= 1 Then
                SQL = "UPDATE MJualD SET HargaPokok=" & FixKoma(clsPostingPenjualan.GetHargaPokok(NullToLong(txtBarcode.EditValue))) & " WHERE NoID=" & NoID
                EksekusiSQL(SQL)
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
        Else
            If txtQty.EditValue * txtKonversi.EditValue < SisaStokGudang Then
                XtraMessageBox.Show("Stok " & txtBarang.Text & " melebihi stok Gudang " & txtGudang.Text, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
                Return False
                Exit Function
            End If
        End If
        If txtQty.EditValue = 0 Then
            XtraMessageBox.Show("Qty masih 0.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
            txtQty.Focus()
            Return False
            Exit Function
        End If
        If txtPacking.Text <> "" Then
            'If QtySisa() < txtQty.EditValue * txtKonversi.EditValue Then
            '    XtraMessageBox.Show("Qty melebihi Sisa Qty Packing.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
            '    txtQty.Focus()
            '    Return False
            '    Exit Function
            'End If
            If QtySisa() - (txtQty.EditValue * txtKonversi.EditValue) <> 0 Then
                XtraMessageBox.Show("Qty harus sama dengan Qty packing.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
                txtQty.Focus()
                Return False
                Exit Function
            End If
        End If
        Return True
    End Function
    Private Function SisaStokGudang() As Double
        Dim x As Double = 0.0
        Try
            x = NullToDbl(EksekusiSQlSkalarNew("SELECT SUM((MKartuStok.QtyMasuk-MKartuStok.QtyMasuk)*MKartuStok.Konversi) FROM MKartuStok WHERE MKartuStok.IDGudang=" & NullToLong(txtGudang.EditValue) & " AND MKartuStok.IDBarang=" & NullToLong(txtBarang.EditValue)))
        Catch ex As Exception

        End Try
        Return x
    End Function
    Private Function QtySisa() As Double
        Dim x As Double = 0.0
        Try
            x = NullToDbl(EksekusiSQlSkalarNew("Select ((MPackingD.Qty*MPackingD.Konversi)-" & DiJual() & ") AS QtySisa FROM MPackingD WHERE NoID=" & NullToLong(txtPacking.EditValue)))
        Catch ex As Exception

        End Try
        Return x
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
        If Exists(FolderLayouts & Me.Name & LayoutControl1.Name & ".xml") Then
            LayoutControl1.RestoreLayoutFromXml(FolderLayouts & Me.Name & LayoutControl1.Name & ".xml")
        End If
    End Sub
    Private Sub cmdSaveLayout_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles cmdSaveLayout.ItemClick
        Dim xOtorisasi As New frmOtorisasiAdmin
        Try
            If XtraMessageBox.Show("Simpan Layout?", NamaAplikasi, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = Windows.Forms.DialogResult.Yes AndAlso xOtorisasi.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
                Ini.TulisIniPath(FolderLayouts & Me.Name & ".ini", "Form", "Width", Me.Width)
                Ini.TulisIniPath(FolderLayouts & Me.Name & ".ini", "Form", "Height", Me.Height)
                Me.Left = (Screen.PrimaryScreen.WorkingArea.Width - Me.Width) \ 2
                Me.Top = (Screen.PrimaryScreen.WorkingArea.Height - Me.Height) \ 2

                LayoutControl1.SaveLayoutToXml(FolderLayouts & Me.Name & LayoutControl1.Name & ".xml")
                gvBarcode.SaveLayoutToXml(FolderLayouts & Me.Name & gvBarcode.Name & ".xml")
                gvBarang.SaveLayoutToXml(FolderLayouts & Me.Name & gvBarang.Name & ".xml")
                gvGudang.SaveLayoutToXml(FolderLayouts & Me.Name & gvGudang.Name & ".xml")
                gvSatuan.SaveLayoutToXml(FolderLayouts & Me.Name & gvSatuan.Name & ".xml")
                gvPacking.SaveLayoutToXml(FolderLayouts & Me.Name & gvPacking.Name & ".xml")
                gvSalesman.SaveLayoutToXml(FolderLayouts & Me.Name & gvSalesman.Name & ".xml")
            End If
        Catch ex As Exception
            XtraMessageBox.Show("Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            xOtorisasi.Dispose()
        End Try
    End Sub

    Private Sub frmEntriJualD_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        Try
            If DialogResult = Windows.Forms.DialogResult.OK Then
                FormPemanggil.RefreshDetil()
                FormPemanggil.Show()
                FormPemanggil.Focus()
                FormPemanggil.txtBarang.Focus()
            Else
                If IsFastEntri Then
                    EksekusiSQL("DELETE FROM MJUALD WHERE NoID=" & NoID)
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
            Me.Width = Ini.BacaIniPath(FolderLayouts & Me.Name & ".ini", "Form", "Width", Me.Width)
            Me.Height = Ini.BacaIniPath(FolderLayouts & Me.Name & ".ini", "Form", "Height", Me.Height)

            Me.Left = (Screen.PrimaryScreen.WorkingArea.Width - Me.Width) \ 2
            Me.Top = (Screen.PrimaryScreen.WorkingArea.Height - Me.Height) \ 2
            LoadLayout()
            FungsiControl.SetForm(Me)
            HighLightTxt()
            txtCatatan.Properties.CharacterCasing = CharacterCasing.Normal
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
        Dim PPN As Double = 0.0
        Try
            'Dim Harga As Double = 0.0
            ''If txtPacking.Text <> "" Then
            ''    Harga = IIf(HargaPcs = 0, txtHarga.EditValue, HargaPcs) * IIf(txtKonversi.EditValue = 0, 1, txtKonversi.EditValue)
            ''Else
            'Harga = txtHarga.EditValue
            ''End If

            'Dim SubTotal As Double = (Harga - txtDiscRp1.EditValue - txtDiscRp2.EditValue - txtDiscRp3.EditValue)
            'Dim DiscA As Double = Harga * txtQty.EditValue * txtDiscPersen1.EditValue / 100
            'Dim DiscB As Double = Harga * txtQty.EditValue * txtDiscPersen2.EditValue / 100
            'Dim DiscC As Double = Harga * txtQty.EditValue * txtDiscPersen3.EditValue / 100
            'txtHarga.EditValue = Harga
            SQL = "SELECT MBarangD.Konversi, MBarang.CtnPcs, 0 AS PPN FROM MBarangD LEFT JOIN MBarang ON MBarang.NoID=MBarangD.IDBarang LEFT JOIN MSatuan ON MSatuan.NoID=MBarangD.IDSatuan WHERE MBarangD.IDBarang=" & NullToLong(txtBarang.EditValue) & " AND MSatuan.NoID=" & NullToLong(txtSatuan.EditValue)
            ds = ExecuteDataset("Tabel", SQL)
            If ds.Tables("Tabel").Rows.Count >= 1 Then
                txtKonversi.EditValue = NullToDbl(ds.Tables(0).Rows(0).Item("Konversi"))
                IsiKarton = NullToDbl(ds.Tables(0).Rows(0).Item("CtnPcs"))
                PPN = NullToDbl(ds.Tables(0).Rows(0).Item("PPN"))
                If IsiKarton = 0 Then
                    txtCtn.EditValue = 0
                Else
                    txtCtn.EditValue = txtQty.EditValue / IsiKarton * txtKonversi.EditValue
                End If
            End If
            txtJUmlah.EditValue = txtQty.EditValue * (Bulatkan((txtHarga.EditValue * (1 - (txtDiscPersen1.EditValue / 100)) * (1 - (txtDiscPersen2.EditValue / 100)) * (1 - (txtDiscPersen3.EditValue / 100))), 0) - txtDiscRp1.EditValue - txtDiscRp2.EditValue - txtDiscRp3.EditValue)
            txtPPN.EditValue = Bulatkan(txtJUmlah.EditValue * PPN / 100, 0)
        Catch ex As Exception
        Finally
            ds.Dispose()
        End Try
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
                    txtHarga.EditValue = clsPostingPenjualan.HargaJual(NullToLong(txtBarcode.EditValue), NullToDbl(txtQty.EditValue * txtKonversi.EditValue), txtDiscPersen1.EditValue)
                End If

            End If
            HitungJumlah()
        Catch ex As Exception
            XtraMessageBox.Show("Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            Ds.Dispose()
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
    Private Sub RefreshLookUpSatuan()
        Dim ds As New DataSet
        Try
            SQL = "SELECT MSatuan.NoID, MSatuan.Kode, MSatuan.Nama FROM MSatuan WHERE MSatuan.IsActive=1 AND MSatuan.NoID IN (SELECT MBarangD.IDSatuan FROM MBarangD WHERE MBarangD.IsJual=1 AND MBarangD.IsActive=1 AND MBarangD.IDBarang=" & NullToLong(txtBarang.EditValue) & ")"
            ds = ExecuteDataset("MSatuan", SQL)
            txtSatuan.Properties.DataSource = ds.Tables("MSatuan")
            txtSatuan.Properties.ValueMember = "NoID"
            txtSatuan.Properties.DisplayMember = "Kode"
            'SQL = "SELECT MBarangD.NoID,MBarangD.Barcode,MBarang.Kode,MBarang.Nama,MSatuan.Kode as Satuan,MBarangD.IDSatuan,MBarangD.IDBarang from MbarangD inner join MBarang On MBarangD.IDBarang=MBarang.NoID inner join MSatuan On MBarangD.IDSatuan=MSatuan.NoID"
            'ds = ExecuteDataset("MBarangD", SQL)
            'txtBarcode.Properties.DataSource = ds.Tables("MBarangD")
            'txtBarcode.Properties.ValueMember = "NoID"
            'txtBarcode.Properties.DisplayMember = "Barcode"

        Catch ex As Exception
            XtraMessageBox.Show("Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            ds.Dispose()
        End Try
    End Sub
    Private Sub txtBarang_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtBarang.EditValueChanged
        Dim Ds As New DataSet
        Try
            SQL = "SELECT MBarang.Nama FROM MBarang WHERE MBarang.NoID=" & NullToLong(txtBarang.EditValue)
            Ds = ExecuteDataset("Tabel", SQL)
            If Ds.Tables("Tabel").Rows.Count >= 1 Then
                'txtSatuan.EditValue = NullTolong(Ds.Tables(0).Rows(0).Item("IDSatuan"))
                'txtKonversi.EditValue = NullToDbl(Ds.Tables(0).Rows(0).Item("Konversi"))
                txtNamaStock.Text = NullToStr(Ds.Tables(0).Rows(0).Item("Nama"))
                RefreshLookUpSatuan()
                RefreshLookUpPacking()
                If IsNew Then
                    txtSatuan.EditValue = DefIDSatuan
                End If
                If IsNew Or IsFastEntri Then
                End If
            End If
            refreshStock()
        Catch ex As Exception
            XtraMessageBox.Show("Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            Ds.Dispose()
        End Try
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
            'MWilayah.Nama AS Wilayah, 
            SQL = "SELECT MBarang.Nama AS NamaBarang, MBarang.Kode AS KodeBarang, MGudang.Nama AS Gudang, SUM((MKartuStok.QtyMasuk*MKartuStok.Konversi)-(MKartuStok.QtyKeluar*MKartuStok.Konversi)) AS QtySisa " & _
                  " FROM MKartuStok LEFT JOIN MBarang ON MBarang.NoID=MKartuStok.IDBarang " & _
                  " LEFT JOIN (MGudang LEFT JOIN MWilayah ON MWilayah.NoID=MGUdang.IDWilayah) ON MGudang.NoID=MKartuStok.IDGudang " & _
                  " WHERE MWilayah.NoID=" & DefIDWilayah & " AND MKartuStok.IDBarang=" & NullToLong(txtBarang.EditValue) & vbCrLf & _
                  " GROUP BY MWilayah.Nama, MGudang.Nama, MBarang.Nama, MBarang.Kode "
            odsT2 = ExecuteDataset("vBrg", SQL)
            GcStok.DataSource = odsT2.Tables("vBrg")
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

    Private Sub txtSatuan_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtSatuan.EditValueChanged
        RubahSatuan()
    End Sub

    Private Sub BarButtonItem2_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem2.ItemClick
        cmdTutup.PerformClick()
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

    Private Sub txtPO_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtPacking.EditValueChanged
        Dim Ds As New DataSet
        Try
            If IsNew Or IsFastEntri Then
                SQL = "SELECT MPackingD.*, (MPackingD.Qty*MPackingD.Konversi)-" & DiJual() & " AS QtySisa, MBarang.Kode AS KodeBarang, MBarang.Nama AS NamaBarang, MGudang.Kode AS KodeGudang, MGudang.Nama AS NamaGudang, MSatuan.Nama AS NamaSatuan "
                SQL &= " FROM ((MPackingD LEFT JOIN MGudang ON MGudang.NoID=MPackingD.IDGudang) "
                SQL &= " LEFT JOIN MBarang ON MBarang.NoID=MPackingD.IDBarang)"
                SQL &= " LEFT JOIN MSatuan ON MSatuan.NoID=MPackingD.IDSatuan "
                SQL &= " WHERE MPackingD.NoID= " & NullToLong(txtPacking.EditValue)
                Ds = New DataSet()
                Ds = ExecuteDataset("MPackingD", SQL)
                If Ds.Tables(0).Rows.Count >= 1 Then
                    With Ds.Tables("MPackingD").Rows(0)
                        gvPacking.SaveLayoutToXml(FolderLayouts & Me.Name & gvPacking.Name & ".xml")
                        txtBarang.EditValue = NullToLong(.Item("IDBarang"))
                        'txtBarang.Text = NullTostr(.Item("KodeBarang"))
                        txtSatuan.EditValue = NullToLong(.Item("IDSatuan"))
                        'txtSatuan.Text = NullTostr(.Item("KodeSatuan"))
                        txtGudang.EditValue = NullToLong(.Item("IDGudang"))
                        'txtGudang.Text = NullTostr(.Item("KodeGudang"))
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
                        txtPPN.EditValue = NullToDbl(.Item("PPN"))
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

    Private Sub gvPO_DataSourceChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvPacking.DataSourceChanged
        If System.IO.File.Exists(FolderLayouts & Me.Name & gvPacking.Name & ".xml") Then
            gvPacking.RestoreLayoutFromXml(FolderLayouts & Me.Name & gvPacking.Name & ".xml")
        End If
        With gvPacking
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

    Private Sub txtQty_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtQty.EditValueChanged
        RubahSatuan()
        HitungJumlah()
    End Sub

    Private Sub txtGudang_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtGudang.EditValueChanged
        RubahGudang()
    End Sub
    Private Sub RubahGudang()
        Dim Ds As New DataSet
        Try
            SQL = "SELECT MWilayah.Nama AS Wilayah FROM MGudang LEFT JOIN MWilayah ON MWilayah.NoID=MGudang.IDWilayah WHERE MGudang.NoID=" & NullToLong(txtGudang.EditValue)
            Ds = ExecuteDataset("Tabel", SQL)
            If Ds.Tables("Tabel").Rows.Count >= 1 Then
                txtWilayah.Text = NullToStr(Ds.Tables(0).Rows(0).Item("Wilayah"))
            End If
            RefreshLookUpPacking()
        Catch ex As Exception
            XtraMessageBox.Show("Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            Ds.Dispose()
        End Try
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
    Private Sub gvSalesman_DataSourceChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvSalesman.DataSourceChanged
        If System.IO.File.Exists(FolderLayouts & Me.Name & gvSalesman.Name & ".xml") Then
            gvSalesman.RestoreLayoutFromXml(FolderLayouts & Me.Name & gvSalesman.Name & ".xml")
        End If
        With gvSalesman
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

    Private Sub gvSatuan_DataSourceChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvSatuan.DataSourceChanged
        If System.IO.File.Exists(FolderLayouts & Me.Name & gvSatuan.Name & ".xml") Then
            gvSatuan.RestoreLayoutFromXml(FolderLayouts & Me.Name & gvSatuan.Name & ".xml")
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

    Private Sub txtHarga_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtHarga.EditValueChanged
        HitungJumlah()
    End Sub

    Private Sub txtPPN_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtPPN.EditValueChanged

    End Sub

    Private Sub txtBarcode_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtBarcode.EditValueChanged
        Dim Ds As New DataSet
        Try
            SQL = "SELECT MBarangD.NoID,MBarangD.IDBarang,MBarangD.IDSatuan,MBarang.Kode,MBarang.IDSatuan IDSatuanBase,MBarang.Nama AS NamaBarang, MBarang.NamaAlias " & _
            "FROM MBarangD inner join MBarang On MBarangD.IDbarang=MBarang.NoID WHERE MBarangD.NoID=" & NullToLong(txtBarcode.EditValue)
            Ds = ExecuteDataset("Tabel", SQL)
            If Ds.Tables("Tabel").Rows.Count >= 1 Then
                'txtSatuan.EditValue = NullTolong(Ds.Tables(0).Rows(0).Item("IDSatuan"))
                'txtKonversi.EditValue = NullToDbl(Ds.Tables(0).Rows(0).Item("Konversi"))
                txtBarang.EditValue = NullToLong(Ds.Tables(0).Rows(0).Item("IDBarang"))
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
End Class