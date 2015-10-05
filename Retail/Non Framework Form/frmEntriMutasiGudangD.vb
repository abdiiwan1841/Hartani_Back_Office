Imports System.Data
Imports System.Data.SqlClient
Imports DevExpress.XtraEditors
Imports DevExpress.Utils
Imports System.IO.File
Public Class frmEntriMutasiGudangD
    Dim IsLoading As Boolean = False
    Public IsNew As Boolean = True
    Public IsPosted As Boolean = False
    Public NoID As Long = -1
    Public IDMutasiGudang As Long = -1
    Public IDGudang As Long = -1
    Public Tgl As Date = Today
    Dim oDS As New DataSet
    Dim SQL As String = ""
    Dim DefImageList As ImageList

    Public IsFastEntri As Boolean = False
    Public FormPemanggil As frmEntriMutasiGudang

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
            RefreshBarcode(False)
            RefreshKodeBarang(False)
            'SQL = "SELECT MBarang.NoID, MBarang.Kode, MBarang.Nama, MSatuan.Nama AS Satuan, IsNull(X.QtySisa,0) AS [QtySisa(Pcs)] "
            'SQL &= " FROM MBarang "
            'SQL &= " LEFT JOIN MSatuan ON MSatuan.NoID=MBarang.IDSatuan "
            'SQL &= " LEFT JOIN (SELECT MKartuStok.IDBarang, SUM((IsNull(MKartuStok.QtyMasuk,0)*IsNull(MKartuStok.Konversi,0))-(IsNull(MKartuStok.QtyKeluar,0)*IsNull(MKartuStok.Konversi,0))) AS QtySisa "
            'SQL &= " FROM MKartuStok WHERE MKartuStok.IDGudang=" & IDGudang
            'SQL &= " GROUP BY MKartuStok.IDBarang) X ON X.IDBarang=MBarang.NoID"
            'SQL &= " WHERE MBarang.IsActive=1 "
            'ds = ExecuteDataset("MBarang", SQL)
            'txtBarang.Properties.DataSource = ds.Tables("MBarang")
            'txtBarang.Properties.ValueMember = "NoID"
            'txtBarang.Properties.DisplayMember = "Kode"

            SQL = "SELECT MSatuan.NoID, MSatuan.Kode, MSatuan.Nama FROM MSatuan WHERE MSatuan.IsActive=1 AND MSatuan.NoID IN (SELECT MBarangD.IDSatuan FROM MBarangD WHERE MBarangD.IsJual=1 AND MBarangD.IsActive=1 AND MBarangD.IDBarang=" & NullToLong(txtBarang.EditValue) & ")"
            ds = ExecuteDataset("MSatuan", SQL)
            txtSatuan.Properties.DataSource = ds.Tables("MSatuan")
            txtSatuan.Properties.ValueMember = "NoID"
            txtSatuan.Properties.DisplayMember = "Kode"
            If System.IO.File.Exists(FolderLayouts & Me.Name & gvSatuan.Name & ".xml") Then
                gvSatuan.RestoreLayoutFromXml(FolderLayouts & Me.Name & gvSatuan.Name & ".xml")
            End If
            With gvSatuan
                For x As Integer = 0 To .Columns.Count - 1
                    Select Case .Columns(x).ColumnType.Name.ToLower
                        Case "int32", "int64", "int"
                            .Columns(x).DisplayFormat.FormatType = FormatType.Numeric
                            .Columns(x).DisplayFormat.FormatString = "n0"
                        Case "decimal", "single", "money", "double"
                            .Columns(x).DisplayFormat.FormatType = FormatType.Numeric
                            .Columns(x).DisplayFormat.FormatString = "n2"
                        Case "string"
                            .Columns(x).DisplayFormat.FormatType = FormatType.None
                            .Columns(x).DisplayFormat.FormatString = ""
                        Case "date", "datetime"
                            If .Columns(x).FieldName.Trim.ToLower = "jam" Then
                                .Columns(x).DisplayFormat.FormatType = FormatType.DateTime
                                .Columns(x).DisplayFormat.FormatString = "HH:mm"
                            Else
                                .Columns(x).DisplayFormat.FormatType = FormatType.DateTime
                                .Columns(x).DisplayFormat.FormatString = "dd-MM-yyyy"
                            End If
                    End Select
                Next
            End With


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

        SQL = "SELECT MBarangD.NoID,MBarangD.Barcode,MBarang.Kode,MBarang.Nama,MSatuan.Kode as Satuan,MBarangD.IDSatuan,MBarangD.IDBarang from MbarangD inner join MBarang On MBarangD.IDBarang=MBarang.NoID inner join MSatuan On MBarangD.IDSatuan=MSatuan.NoID"
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
        Dim dsPublicKodeBarang As New DataSet

        'If IsForce Then
        '    If DSPublicKodeBarang.Tables("MBarang") Is Nothing Then 'blm ada data
        '        SQL = "SELECT MBarang.NoID, (CASE WHEN ISNULL(MBarang.Kode,'')='' THEN MBarang.Alias ELSE MBarang.Kode END) AS Kode,MBarang.Barcode,MBarang.HPP, MBarang.Nama, MBarang.NamaAlias, MSatuan.Nama AS Satuan FROM MBarang LEFT JOIN MSatuan ON MSatuan.NoID=MBarang.IDSatuan WHERE MBarang.IsActive=1 AND NOT ((MBarang.KODE='' OR MBarang.KODE IS NULL) AND (MBarang.Alias='' OR MBarang.Alias IS NULL))"
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
        DSPublicKodeBarang = ExecuteDataset("MBarang", SQL)
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
    Private Sub LoadData()
        Try
            SQL = "SELECT MMutasiGudangD.*,MBarang.Nama  FROM MMutasiGudangD left join MBarang on MMutasiGudangD.IDBarang=MBarang.NoID WHERE MMutasiGudangD.NoID= " & NoID
            oDS = New DataSet()
            oDS = ExecuteDataset("MMutasiGudangD", SQL)
            If oDS.Tables(0).Rows.Count >= 1 Then
                With oDS.Tables("MMutasiGudangD").Rows(0)
                    txtBarcode.EditValue = NullToLong(.Item("IDBarangD"))
                    txtBarang.EditValue = NullToLong(.Item("IDBarang"))
                    'RubahBarang()
                    RefreshLookUpSatuan()
                    txtSatuan.EditValue = NullToLong(.Item("IDSatuan"))
                    txtKonversi.EditValue = NullToDbl(.Item("Konversi"))
                    txtQty.EditValue = NullToDbl(.Item("Qty"))
                    txtSaldoGudang.EditValue = NullToDbl(.Item("QtyGudangPcs"))
                    txtHargaJual.EditValue = NullToDbl(.Item("HargaJual"))
                    txtJumlah.EditValue = NullToDbl(.Item("Jumlah"))
                    txtCtn.EditValue = NullToDbl(.Item("Ctn"))
                    txtCatatan.Text = NullToStr(.Item("Keterangan"))
                    txtNamaStock.Text = NullToStr(.Item("Nama"))
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
        Try
            If IsNew Then
                SQL = "INSERT INTO MMutasiGudangD (NoID,NoUrut,IDMutasiGudang,IDBarangD,IDBarang,Tanggal,Jam,IDSatuan,Konversi,Qty,HargaJual,Jumlah,QtyPcs,QtyGudangPcs,CTN,Operator,Keterangan,IDOperator) VALUES ("
                SQL &= NullToLong(GetNewID("MMutasiGudangD", "NoID")) & ","
                SQL &= NullToLong(GetNewID("MMutasiGudangD", "NoUrut", " WHERE IDMutasiGudang=" & IDMutasiGudang)) & ","
                SQL &= IDMutasiGudang & ","
                SQL &= NullToLong(txtBarcode.EditValue) & ","
                SQL &= NullToLong(txtBarang.EditValue) & ","
                SQL &= "GetDate(),"
                SQL &= "GetDate(),"
                SQL &= NullToLong(txtSatuan.EditValue) & ","
                SQL &= FixKoma(txtKonversi.EditValue) & ","
                SQL &= FixKoma(txtQty.EditValue) & ","
                SQL &= FixKoma(txtHargaJual.EditValue) & ","
                SQL &= FixKoma(txtJumlah.EditValue) & ","
                SQL &= FixKoma(txtQty.EditValue * txtKonversi.EditValue) & ","
                SQL &= FixKoma(txtSaldoGudang.EditValue) & ","
                SQL &= FixKoma(txtCtn.EditValue) & ","
                SQL &= "'" & NullToStr(NamaUserAktif) & "',"
                SQL &= "'" & NullToStr(txtCatatan.Text) & "',"
                SQL &= NullToLong(IDUserAktif)
                SQL &= ")"
            Else
                SQL = "UPDATE MMutasiGudangD SET "
                SQL &= " IDMutasiGudang=" & IDMutasiGudang & ","
                SQL &= " IDBarangD=" & NullToLong(txtBarcode.EditValue) & ","
                SQL &= " IDBarang=" & NullToLong(txtBarang.EditValue) & ","
                SQL &= " Tanggal=GetDate(),"
                SQL &= " Jam=GetDate(),"
                SQL &= " IDSatuan=" & NullToLong(txtSatuan.EditValue) & ","
                SQL &= " Konversi=" & FixKoma(txtKonversi.EditValue) & ","
                SQL &= " Qty=" & FixKoma(txtQty.EditValue) & ","
                SQL &= " HargaJual=" & FixKoma(txtHargaJual.EditValue) & ","
                SQL &= " Jumlah=" & FixKoma(txtJumlah.EditValue) & ","
                SQL &= " QtyPcs=" & FixKoma(txtQty.EditValue * txtKonversi.EditValue) & ","
                SQL &= " CTN=" & FixKoma(txtCtn.EditValue) & ","
                SQL &= " Operator='" & NullToStr(NamaUserAktif) & "',"
                SQL &= " Keterangan='" & NullToStr(txtCatatan.Text) & "',"
                SQL &= " IDOperator=" & NullToLong(IDUserAktif)
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
        If txtQty.EditValue = 0 Then
            XtraMessageBox.Show("Qty masih 0.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
            txtQty.Focus()
            Return False
            Exit Function
        End If
        'SisaQty = NullToDbl(EksekusiSQlSkalarNew("SELECT (MKartuStok.QtyMasuk-MKartuStok.QtyKeluar)*MKartuStok.Konversi FROM MKartuStok WHERE IDGudang=" & NullToLong(txtBeli.EditValue)))
        If txtSaldoGudang.EditValue < txtQty.EditValue Then  'SisaQty() < txtQty.EditValue Then
            If XtraMessageBox.Show("Qty Melebihi sisa Stok Gudang!" & vbCrLf & "Mau tetap menyimpan?", "Informasi.", MessageBoxButtons.YesNo) = Windows.Forms.DialogResult.No Then
                'XtraMessageBox.Show("Qty melebihi stok " & NullToStr(EksekusiSQlSkalarNew("SELECT Nama FROM MGudang WHERE NoID=" & IDGudang)) & " .", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
                txtQty.Focus()
                Return False
                Exit Function
            End If
        End If
        Return True
    End Function
    Private Function SisaQty() As Double
        Dim ds As New DataSet
        Try
            SQL = "SELECT SUM((isnull(QtyMasuk,0)*isnull(Konversi,0))-(isnull(QtyKeluar,0)*isnull(Konversi,0))) AS QtySisa FROM MKartuStok WHERE IDBarang=" & NullToLong(txtBarang.EditValue) & " AND IDGudang=" & IDGudang & " AND (IsSPK=0 OR IsSPK Is NULL)"
            'SQL = "SELECT MWilayah.Nama AS Wilayah, MGudang.Nama AS Gudang, MBarang.Nama, SUM(MKartuStok.QtyMasuk*MkartuStok.Konversi) AS Stok, SUM((MKartuStok.QtyMasuk*MkartuStok.Konversi)-(MKartuStok.QtyKeluar*MkartuStok.Konversi))-IsNull(TSPK.DiSPK,0) AS QtySisa" & vbCrLf
            'SQL &= " FROM MKartuStok" & vbCrLf
            'SQL &= " LEFT JOIN (MGudang LEFT JOIN MWilayah ON MWilayah.NoID=MGudang.IDWilayah) ON MGudang.NoID=MKartuStok.IDGudang" & vbCrLf
            'SQL &= " LEFT JOIN MBarang ON MBarang.NoID=MKartuStok.IDBarang" & vbCrLf
            'SQL &= " Left Join" & vbCrLf
            'SQL &= " (SELECT X.IDBarang, X.IDGudang, SUM(X.DiSPK) AS DiSPK FROM" & vbCrLf
            'SQL &= " (SELECT MSPKD.IDBarang, MSPKD.IDGudang, (MSPKD.Qty*MSPKD.Konversi) AS SPK, " & vbCrLf
            'SQL &= " (MSPKD.Qty*MSPKD.Konversi)-IsNull(" & vbCrLf
            'SQL &= " (SELECT SUM(B.Qty*B.Konversi) FROM MJualD B WHERE B.IDBarang=" & NullTolong(txtBarang.EditValue) & " AND B.IDGudang=" & NullTolong(IDGudang) & " AND B.IDSPKD<>" & NoID & " AND B.IDSPKD=MSPKD.NoID),0) AS DiSPK " & vbCrLf
            'SQL &= " FROM MSPKD WHERE MSPKD.IDBarang=" & NullTolong(txtBarang.EditValue) & " AND MSPKD.IDGudang=" & NullTolong(IDGudang) & " AND MSPKD.NoID<>" & NoID & ") X" & vbCrLf
            'SQL &= " GROUP BY X.IDBarang, X.IDGudang) TSPK ON MKartuStok.IDBarang=TSPK.IDBarang AND MKartuStok.IDGudang=TSPK.IDGudang" & vbCrLf
            'SQL &= " WHERE MKartuStok.IDBarang=" & NullTolong(txtBarang.EditValue) & " AND MKartuStok.IDGudang=" & NullTolong(IDGudang) & " AND (MKartuStok.IsSPK=0 OR MKartuStok.IsSPK Is NULL)"
            'SQL &= " GROUP BY TSPK.DiSPK, MWilayah.Nama, MGudang.Nama, MBarang.Nama" & vbCrLf
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
                Ini.TulisIniPath(FolderLayouts & Me.Name & ".ini", "Form", "Width", Me.Width)
                Ini.TulisIniPath(FolderLayouts & Me.Name & ".ini", "Form", "Height", Me.Height)
                Me.Left = (Screen.PrimaryScreen.WorkingArea.Width - Me.Width) \ 2
                Me.Top = (Screen.PrimaryScreen.WorkingArea.Height - Me.Height) \ 2

                LayoutControl1.SaveLayoutToXml(FolderLayouts & Me.Name & LayoutControl1.Name & ".xml")
                gvBarcode.SaveLayoutToXml(FolderLayouts & Me.Name & gvBarcode.Name & ".xml")
                gvBarang.SaveLayoutToXml(FolderLayouts & Me.Name & gvBarang.Name & ".xml")
                gvSatuan.SaveLayoutToXml(FolderLayouts & Me.Name & gvSatuan.Name & ".xml")
            End If
        Catch ex As Exception
            XtraMessageBox.Show("Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            xOtorisasi.Dispose()
        End Try
    End Sub

    Private Sub frmEntriMutasiGudangD_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        Try
            If DialogResult = Windows.Forms.DialogResult.OK Then
                FormPemanggil.RefreshDetil()
                FormPemanggil.Show()
                FormPemanggil.Focus()
                FormPemanggil.txtBarang.Focus()
            Else
                If IsFastEntri Then
                    EksekusiSQL("DELETE FROM MMutasiGudangD WHERE NoID=" & NoID)
                End If
                FormPemanggil.Show()
                FormPemanggil.Focus()
            End If
            Me.Dispose()
        Catch ex As Exception

        End Try
    End Sub

    Private Sub frmEntriPOD_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Dim dlg As WaitDialogForm = Nothing
        IsLoading = True
        Try
            Dim curentcursor As Cursor = Windows.Forms.Cursor.Current
            Windows.Forms.Cursor.Current = Cursors.WaitCursor
            dlg = New WaitDialogForm(String.Format("Creating component and analize database.{0}MOHON TUNGGU ...", vbCrLf), Application.ProductName)
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
        Catch ex As Exception
            XtraMessageBox.Show(ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            Windows.Forms.Cursor.Current = Cursors.Default
            dlg.Close()
            dlg.Dispose()
        End Try
        IsLoading = False
    End Sub
    Private Sub LoadLayout()
        If Exists(FolderLayouts & Me.Name & LayoutControl1.Name & ".xml") Then
            LayoutControl1.RestoreLayoutFromXml(FolderLayouts & Me.Name & LayoutControl1.Name & ".xml")
        End If
    End Sub

    Private Sub txtBarang_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtBarang.EditValueChanged
        'If Not IsLoading Then
        '    RubahBarang()
        'End If
    End Sub
    Private Sub RubahBarang()
        Dim Ds As New DataSet
        Try
            'SQL = "SELECT MBarang.Nama,MBarang.IDSatuan, MSatuan.Konversi FROM MBarang LEFT JOIN MSatuan ON MSatuan.NoID=MBarang.IDSatuan WHERE MBarang.NoID=" & NullToLong(txtBarang.EditValue)
            'Ds = ExecuteDataset("Tabel", SQL)
            'If Ds.Tables("Tabel").Rows.Count >= 1 Then
            '    txtNamaStock.EditValue = NullToStr(Ds.Tables(0).Rows(0).Item("Nama"))
            '    'txtKonversi.EditValue = NullToDbl(Ds.Tables(0).Rows(0).Item("Konversi"))
            '    'txtSatuan.Text = 'NullToLong(DefIDSatuan)
            'End If
            'RefreshLookUp()
        Catch ex As Exception
            XtraMessageBox.Show("Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            Ds.Dispose()
        End Try
    End Sub
    Private Sub txtSatuan_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtSatuan.EditValueChanged
        If Not IsLoading Then

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
                End If
            Catch ex As Exception
                XtraMessageBox.Show("Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
            Finally
                Ds.Dispose()
            End Try
        End If
    End Sub

    Private Sub BarButtonItem1_ItemClick_1(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles mnSave.ItemClick
        cmdSave.PerformClick()
    End Sub

    Private Sub BarButtonItem2_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem2.ItemClick
        cmdTutup.PerformClick()
    End Sub

    Private Sub gvBarang_DataSourceChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvBarang.DataSourceChanged
        If System.IO.File.Exists(FolderLayouts & Me.Name & gvBarang.Name & ".xml") Then
            gvBarang.RestoreLayoutFromXml(FolderLayouts & Me.Name & gvBarang.Name & ".xml")
        End If
        With gvBarang
            For x As Integer = 0 To .Columns.Count - 1
                Select Case .Columns(x).ColumnType.Name.ToLower
                    Case "int32", "int64", "int"
                        .Columns(x).DisplayFormat.FormatType = FormatType.Numeric
                        .Columns(x).DisplayFormat.FormatString = "n0"
                    Case "decimal", "single", "money", "double"
                        .Columns(x).DisplayFormat.FormatType = FormatType.Numeric
                        .Columns(x).DisplayFormat.FormatString = "n2"
                    Case "string"
                        .Columns(x).DisplayFormat.FormatType = FormatType.None
                        .Columns(x).DisplayFormat.FormatString = ""
                    Case "date", "datetime"
                        If .Columns(x).FieldName.Trim.ToLower = "jam" Then
                            .Columns(x).DisplayFormat.FormatType = FormatType.DateTime
                            .Columns(x).DisplayFormat.FormatString = "HH:mm"
                        Else
                            .Columns(x).DisplayFormat.FormatType = FormatType.DateTime
                            .Columns(x).DisplayFormat.FormatString = "dd-MM-yyyy"
                        End If
                End Select
            Next
        End With
    End Sub

    Private Sub txtQty_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtQty.EditValueChanged
        HitungJumlah()
    End Sub
    Private Sub HitungJumlah()
        Dim ds As New DataSet
        Dim IsiKarton As Double = 0.0
        Try
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
            txtJumlah.EditValue = Bulatkan(NullToDbl(txtQty.EditValue) * NullToDbl(txtHargaJual.EditValue), 2)
        Catch ex As Exception
        Finally
            ds.Dispose()
        End Try
    End Sub

    Private Sub txtQty_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtQty.LostFocus
        HitungJumlah()
    End Sub

    Private Sub txtKonversi_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtKonversi.EditValueChanged
        HitungJumlah()
    End Sub

    Private Sub txtKonversi_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtKonversi.LostFocus
        HitungJumlah()
    End Sub

    Private Sub txtBarcode_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtBarcode.EditValueChanged
        If Not IsLoading Then

            Dim Ds As New DataSet
            Try
                SQL = "SELECT MBarangD.NoID,MBarangD.IDBarang,MBarangD.IDSatuan,MBarang.Kode, MBarang.IDSatuan IDSatuanBase,MBarang.Nama + ' ' + IsNull(MBarangD.Varian,'') AS NamaBarang, MBarang.NamaAlias, MBarangD.HargaJual " & _
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
                    txtHargaJual.EditValue = NullToDbl(Ds.Tables(0).Rows(0).Item("HargaJual"))
                    txtSaldoGudang.EditValue = (clsPostingKartuStok.CekSaldoStockVerian(IDGudang, NullToLong(txtBarcode.EditValue), Tgl))
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

        End If
    End Sub
    Private Sub RefreshLookUpSatuan()
        Dim ds As New DataSet
        Try
            SQL = "SELECT MSatuan.NoID, MSatuan.Kode, MSatuan.Nama FROM MSatuan WHERE MSatuan.IsActive=1 AND MSatuan.NoID IN (SELECT MBarangD.IDSatuan FROM MBarangD WHERE MBarangD.IsJual=1 AND MBarangD.IsActive=1 AND MBarangD.IDBarang=" & NullToLong(txtBarang.EditValue) & ")"
            ds = ExecuteDataset("MSatuan", SQL)
            txtSatuan.Properties.DataSource = ds.Tables("MSatuan")
            txtSatuan.Properties.ValueMember = "NoID"
            txtSatuan.Properties.DisplayMember = "Kode"
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
        Catch ex As Exception
            XtraMessageBox.Show("Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            ds.Dispose()
        End Try
    End Sub

    Private Sub frmEntriMutasiGudangD_Shown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Shown
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
End Class