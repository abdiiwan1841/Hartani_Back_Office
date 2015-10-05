Imports VPoint.Ini
Imports System.Data
Imports System.Data.SqlClient
Imports DevExpress.XtraEditors
Imports DevExpress.Utils
Imports VPoint.clsPostingKartuStok

Public Class clsPostingPenjualan
    'Public Enum JenisTransaksi
    '    Penjualan = 2
    '    Penjualan = 6
    '    MutasiGudang = 4
    'End Enum
    Public Shared Function GetHargaPokok(ByVal IDBarangD As Long) As Double
        Dim SQL As String = ""
        Dim DS As New DataSet
        Dim Hasil As Double = 0.0
        Try
            DS = ExecuteDataset("MBarang", "SELECT MBarang.HargaBeliPcs AS HPP, MBarang.CtnPcs AS IsiKarton, MBarang.IDKategori FROM MBarang INNER JOIN MBarangD ON MBarang.NoID=MBarangD.IDBarang WHERE MBarangD.NoID=" & IDBarangD)
            If Not DS.Tables("MBarang") Is Nothing Then
                If DS.Tables("MBarang").Rows.Count >= 1 Then
                    If NullToLong(DS.Tables("MBarang").Rows(0).Item("IDKategori")) = 34 Then 'Khusus Dept 34 Dikali IsiKarton
                        Hasil = NullToDbl(DS.Tables("MBarang").Rows(0).Item("HPP")) * NullToDbl(DS.Tables("MBarang").Rows(0).Item("IsiKarton"))
                    Else
                        Hasil = NullToDbl(DS.Tables("MBarang").Rows(0).Item("HPP"))
                    End If
                Else
                    Hasil = 0.0
                End If
            Else
                Hasil = 0.0
            End If
        Catch ex As Exception
            XtraMessageBox.Show("Info Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
        Finally
            If Not DS.Tables("MBarang") Is Nothing Then
                DS.Dispose()
            End If
        End Try
    End Function
    Public Shared Function HargaJual(ByVal IDBarangD As Long, ByVal Qty As Double, ByRef Disc As Double) As Double
        Dim SQL As String = ""
        Dim TipeHarga As Integer = -1
        Dim DS As New DataSet
        Try
            ''Perhitungan Baru
            'SQL = " SELECT CASE WHEN MAlamatD.IDJenisHarga=1 THEN MBarangD.HargaJualA  " & _
            '      " WHEN MAlamatD.IDJenisHarga=2 THEN MBarangD.HargaJualB  " & _
            '      " WHEN MAlamatD.IDJenisHarga=3 THEN MBarangD.HargaJualC  " & _
            '      " WHEN MAlamatD.IDJenisHarga=4 THEN MBarangD.HargaJualD  " & _
            '      " WHEN MAlamatD.IDJenisHarga=5 THEN MBarangD.HargaJualE  " & _
            '      " WHEN MAlamatD.IDJenisHarga=6 THEN MBarangD.HargaJualF  " & _
            '      " ELSE MBarangD.HargaJualF" & _
            '      " END AS HargaJual" & _
            '      " FROM (MBarangD LEFT JOIN MBarang ON MBarang.NoID=MBarangD.IDBarang) INNER JOIN MAlamatD ON MAlamatD.IDJenisBarang=MBarang.IDJenis" & _
            '      " WHERE MBarangD.IDSatuan = " & IDSatuan & " AND MBarang.NoID = " & IDBarang & " And MAlamatD.IDAlamat = " & IDAlamat

            ''Perhitungan Lama
            'TipeHarga = NullToLong(EksekusiSQlSkalarNew("SELECT DefaultTipeHarga FROM MAlamat WHERE NoID=" & IDAlamat))
            'SQL = " SELECT CASE WHEN " & TipeHarga & "=1 THEN MBarangD.HargaJualA  " & _
            '      " WHEN " & TipeHarga & "=2 THEN MBarangD.HargaJualB  " & _
            '      " WHEN " & TipeHarga & "=3 THEN MBarangD.HargaJualC  " & _
            '      " WHEN " & TipeHarga & "=4 THEN MBarangD.HargaJualD  " & _
            '      " WHEN " & TipeHarga & "=5 THEN MBarangD.HargaJualE  " & _
            '      " WHEN " & TipeHarga & "=6 THEN MBarangD.HargaJualF  " & _
            '      " ELSE MBarangD.HargaJualD" & _
            '      " END AS HargaJual, " & _
            '      " CASE WHEN " & TipeHarga & "=1 THEN MBarang.DiscA1  " & _
            '      " WHEN " & TipeHarga & "=2 THEN MBarang.DiscB1  " & _
            '      " WHEN " & TipeHarga & "=3 THEN MBarang.DiscC1  " & _
            '      " WHEN " & TipeHarga & "=4 THEN MBarang.DiscD1  " & _
            '      " WHEN " & TipeHarga & "=5 THEN MBarang.DiscE1  " & _
            '      " WHEN " & TipeHarga & "=6 THEN MBarang.DiscF1  " & _
            '      " ELSE MBarang.DiscD1 " & _
            '      " END AS Disc1, " & _
            '      " CASE WHEN " & TipeHarga & "=1 THEN MBarang.DiscA2  " & _
            '      " WHEN " & TipeHarga & "=2 THEN MBarang.DiscB2  " & _
            '      " WHEN " & TipeHarga & "=3 THEN MBarang.DiscC2  " & _
            '      " WHEN " & TipeHarga & "=4 THEN MBarang.DiscD2  " & _
            '      " WHEN " & TipeHarga & "=5 THEN MBarang.DiscE2  " & _
            '      " WHEN " & TipeHarga & "=6 THEN MBarang.DiscF2  " & _
            '      " ELSE MBarang.DiscD2 " & _
            '      " END AS Disc2" & _
            '      " FROM MBarangD LEFT JOIN MBarang ON MBarang.NoID=MBarangD.IDBarang " & _
            '      " WHERE MBarangD.IDSatuan = " & IDBarangD & " AND MBarang.NoID = " & IDBarang
            'TETAP JAYA PER QTY
            SQL = "SELECT " & vbCrLf & _
                  " CASE WHEN " & FixKoma(Qty) & ">=isnull(MBarangD.Qty3,0) and isnull(MBarangD.Qty3,0) <>0  THEN " & vbCrLf & _
                  " MBarangD.HargaJual3 " & vbCrLf & _
                  " WHEN " & FixKoma(Qty) & ">=isnull(MBarangD.Qty2,0) and isnull(MBarangD.Qty2,0)<>0 THEN " & vbCrLf & _
                  " MBarangD.HargaJual2 " & vbCrLf & _
                  " ELSE " & vbCrLf & _
                  " MBarangD.HargaJual " & vbCrLf & _
                  " END AS Harga, " & vbCrLf & _
                  " CASE WHEN " & FixKoma(Qty) & ">=isnull(MBarangD.Qty3,0) and isnull(MBarangD.Qty3,0) <>0  THEN " & vbCrLf & _
                  " MBarangD.DiscPromo3 " & vbCrLf & _
                  " WHEN " & FixKoma(Qty) & ">=isnull(MBarangD.Qty2,0) and isnull(MBarangD.Qty2,0)<>0 THEN " & vbCrLf & _
                  " MBarangD.DiscPromo2 " & vbCrLf & _
                  " ELSE " & vbCrLf & _
                  " MBarangD.DiscPromo " & vbCrLf & _
                  " END AS Disc " & vbCrLf & _
                  " FROM MBarangD INNER JOIN MBarang ON MBarang.NoID=MBarangD.IDBarang " & vbCrLf & _
                  " WHERE MBarangD.NoID=" & IDBarangD
            DS = ExecuteDataset("MHarga", SQL)
            If DS.Tables(0).Rows.Count >= 1 Then
                Disc = NullToDbl(DS.Tables(0).Rows(0).Item("Disc"))
                Return NullToDbl(DS.Tables(0).Rows(0).Item("Harga"))
            Else
                Return 0.0
            End If
        Catch ex As Exception
            XtraMessageBox.Show("Info : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
            Return 0.0
        Finally
            DS.Dispose()
        End Try
    End Function
    Private Shared Function AdaDiReturJual(ByVal IDJual As Long) As Boolean
        Dim ds As New DataSet
        Try
            ds = ExecuteDataset("NamaTabel", "SELECT MReturJualD.* FROM (MReturJualD LEFT JOIN (MJualD LEFT JOIN MJual ON MJual.NoID=MJualD.IDJual) ON MJualD.NoID=MReturJualD.IDJualD) WHERE MJual.NoID=" & IDJual)
            If ds.Tables(0).Rows.Count >= 1 Then
                Return True
            Else
                Return False
            End If
        Catch ex As Exception
            Return False
        End Try

    End Function
    Private Shared Function AdaDiSuratJalan(ByVal IDJual As Long) As Boolean
        Dim ds As New DataSet
        Try
            ds = ExecuteDataset("NamaTabel", "SELECT MDOD.* FROM ((MDOD INNER JOIN MDO ON MDO.NoID=MDOD.IDDO) LEFT JOIN (MJualD INNER JOIN MJual ON MJual.NoID=MJualD.IDJual) ON MJualD.NoID=MDOD.IDJualD) WHERE MDO.Dari=1 AND MJual.NoID=" & IDJual)
            If ds.Tables(0).Rows.Count >= 1 Then
                Return True
            Else
                Return False
            End If
        Catch ex As Exception
            Return False
        End Try

    End Function
    Private Shared Function AdaDiRubahHargaJual(ByVal IDJual As Long) As Boolean
        Dim ds As New DataSet
        Try
            ds = ExecuteDataset("NamaTabel", "SELECT MRevisiHargaJual.* FROM (MRevisiHargaJual LEFT JOIN (MJualD LEFT JOIN MJual ON MJual.NoID=MJualD.IDJual) ON MJualD.NoID=MRevisiHargaJual.IDJualD) WHERE MJual.NoID=" & IDJual)
            If ds.Tables(0).Rows.Count >= 1 Then
                Return True
            Else
                Return False
            End If
        Catch ex As Exception
            Return False
        End Try

    End Function
    Private Shared Function AdaDiPenjualan(ByVal IDDO As Long) As Boolean
        Dim ds As New DataSet
        Try
            ds = ExecuteDataset("NamaTabel", "SELECT MJualD.* FROM (MJualD LEFT JOIN (MDOD LEFT JOIN MDO ON MDO.NoID=MDOD.IDDO) ON MDOD.NoID=MJualD.IDDOD) WHERE MDO.NoID=" & IDDO)
            If ds.Tables(0).Rows.Count >= 1 Then
                Return True
            Else
                Return False
            End If
        Catch ex As Exception
            Return False
        End Try
    End Function
    Private Shared Function SOAdaDiSPK(ByVal IDSO As Long) As Boolean
        Dim ds As New DataSet
        Try
            ds = ExecuteDataset("NamaTabel", "SELECT MSPKD.* FROM (MSPKD LEFT JOIN (MSOD LEFT JOIN MSO ON MSO.NoID=MSOD.IDSO) ON MSOD.NoID=MSPKD.IDSOD) WHERE MSO.NoID=" & IDSO)
            If ds.Tables(0).Rows.Count >= 1 Then
                Return True
            Else
                Return False
            End If
        Catch ex As Exception
            Return False
        End Try

    End Function
    Private Shared Function SPKAdaDiPacking(ByVal IDSPK As Long) As Boolean
        Dim ds As New DataSet
        Try
            ds = ExecuteDataset("NamaTabel", "SELECT MPackingD.* FROM MPackingD LEFT JOIN (MSPKD INNER JOIN MSPK ON MSPK.NoID=MSPKD.IDSPK) ON MSPKD.NoID=MPackingD.IDSPKD WHERE MSPK.NoID=" & IDSPK)
            If ds.Tables(0).Rows.Count >= 1 Then
                Return True
            Else
                Return False
            End If
        Catch ex As Exception
            Return False
        End Try
    End Function
    Private Shared Function PackingAdaDiPenjualan(ByVal IDPacking As Long) As Boolean
        Dim ds As New DataSet
        'Dim diSJ As Boolean = False
        Dim diJL As Boolean = False
        Try
            'ds = ExecuteDataset("NamaTabel", "SELECT MJualD.* FROM MJualD LEFT JOIN (MPackingD INNER JOIN MPacking ON MPacking.NoID=MPackingD.IDPacking) ON MPackingD.NoID=MJualD.IDPackingD WHERE MPacking.NoID=" & IDPacking)
            'If ds.Tables(0).Rows.Count >= 1 Then
            '    diJual = True
            'Else
            '    diJual = False
            'End If
            ds = ExecuteDataset("NamaTabel", "SELECT MJualD.* FROM (MJualD INNER JOIN MJual ON MJual.NoID=MJualD.IDJual) LEFT JOIN (MPackingD INNER JOIN MPacking ON MPacking.NoID=MPackingD.IDPacking) ON MPackingD.NoID=MJualD.IDPackingD WHERE MPacking.NoID=" & IDPacking)
            If ds.Tables(0).Rows.Count >= 1 Then
                diJL = True
            Else
                diJL = False
            End If

            'If Not diJL AndAlso Not diSJ Then
            '    Return False
            'Else
            '    Return True
            'End If
            Return diJL
        Catch ex As Exception
            Return False
        End Try
    End Function
    Public Shared Sub PostingSO(ByVal IDSO As Long)
        Dim ds As New DataSet
        Dim SQL As String = ""
        Try
            ds = modSqlServer.ExecuteDataset("MSOD", "SELECT MSOD.*, MSO.IsPosted, MSO.Tanggal AS TGLPO FROM MSOD LEFT JOIN MSO ON MSO.NoID=MSOD.IDSO WHERE MSOD.IDSO=" & IDSO)
            If ds.Tables("MSOD").Rows.Count >= 1 Then
                If Not NullTobool(ds.Tables("MSOD").Rows(0).Item("IsPosted")) Then
                    EksekusiSQL("UPDATE MSO SET IsPosted=1, IDUserPosting=" & IDUserAktif & ", TglPosting=getdate()  WHERE NoID=" & IDSO)
                End If
            End If
        Catch ex As Exception
            DevExpress.XtraEditors.XtraMessageBox.Show("Kesalahan perhitugan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
        Finally
            ds.Dispose()
        End Try
    End Sub
    Public Shared Sub UnPostingSO(ByVal IDSO As Long)
        Dim ds As New DataSet
        Try
            If Not SOAdaDiSPK(IDSO) Then
                EksekusiSQL("UPDATE MSO SET IsPosted=0, IDUserPosting=NULL, TglPosting=NULL WHERE NoID=" & IDSO)
            End If
        Catch ex As Exception
            DevExpress.XtraEditors.XtraMessageBox.Show("Kesalahan perhitugan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
        Finally
            ds.Dispose()
        End Try
    End Sub
    Public Shared Sub PostingSPK(ByVal IDSPK As Long)
        Dim ds As New DataSet
        Dim SQL As String = ""
        Try
            ds = modSqlServer.ExecuteDataset("MSPKD", "SELECT MSPKD.*, MSPK.IsPosted, MSPK.IsPacking, MSPK.Tanggal AS TGLSPK FROM MSPKD LEFT JOIN MSPK ON MSPK.NoID=MSPKD.IDSPK WHERE MSPKD.IDSPK=" & IDSPK)
            If ds.Tables("MSPKD").Rows.Count >= 1 Then
                If Not NullTobool(ds.Tables("MSPKD").Rows(0).Item("IsPosted")) Then
                    EksekusiSQL("UPDATE MSPK SET IsPacking=1, IsPosted=1, IDUserPosting=" & IDUserAktif & ", TglPosting=getdate() WHERE NoID=" & IDSPK)
                End If
            End If
        Catch ex As Exception
            DevExpress.XtraEditors.XtraMessageBox.Show("Kesalahan perhitugan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
        Finally
            ds.Dispose()
        End Try
    End Sub
    Public Shared Sub PostingPacking(ByVal IDPacking As Long)
        Dim ds As New DataSet
        Dim SQL As String = ""
        Try
            ds = modSqlServer.ExecuteDataset("MPackingD", "SELECT MPackingD.*, MPacking.IsPosted, MPacking.IsPacking, MPacking.Tanggal FROM MPackingD LEFT JOIN MPacking ON MPacking.NoID=MPackingD.IDPacking WHERE MPackingD.IDPacking=" & IDPacking)
            If ds.Tables("MPackingD").Rows.Count >= 1 Then
                If Not NullTobool(ds.Tables("MPackingD").Rows(0).Item("IsPosted")) Then
                    EksekusiSQL("UPDATE MPacking SET IsPacking=1, IsPosted=1, IDUserPosting=" & IDUserAktif & ", TglPosting=getdate() WHERE NoID=" & IDPacking)
                    EksekusiSQL("UPDATE MPackingD SET IsPacking=1 WHERE IDPacking=" & IDPacking)
                End If
            End If
        Catch ex As Exception
            DevExpress.XtraEditors.XtraMessageBox.Show("Kesalahan perhitugan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
        Finally
            ds.Dispose()
        End Try
    End Sub
    Public Shared Sub UnPostingSPK(ByVal IDSPK As Long)
        Dim ds As New DataSet
        Try
            If Not SPKAdaDiPacking(IDSPK) Then
                EksekusiSQL("UPDATE MSPK SET IsPacking=0, IsPosted=0, IDUserPosting=NULL, TglPosting=NULL WHERE NoID=" & IDSPK)
            End If
        Catch ex As Exception
            DevExpress.XtraEditors.XtraMessageBox.Show("Kesalahan perhitugan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
        Finally
            ds.Dispose()
        End Try
    End Sub
    Public Shared Sub UnPostingPacking(ByVal IDPacking As Long)
        Dim ds As New DataSet
        Try
            If Not PackingAdaDiPenjualan(IDPacking) Then
                EksekusiSQL("UPDATE MPacking SET IsPacking=0, IsPosted=0, IDUserPosting=NULL, TglPosting=NULL WHERE NoID=" & IDPacking)
                EksekusiSQL("UPDATE MPackingD SET IsPacking=0 WHERE IDPacking=" & IDPacking)
            End If
        Catch ex As Exception
            DevExpress.XtraEditors.XtraMessageBox.Show("Kesalahan perhitugan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
        Finally
            ds.Dispose()
        End Try
    End Sub
    Public Shared Sub PostingDO(ByVal IDDO As Long)
        Dim ds As New DataSet
        Dim SQL As String = ""
        Try
            ds = modSqlServer.ExecuteDataset("MDOD", "SELECT MDOD.*, MDO.IsPosted, MDO.Tanggal AS TGLDO FROM MDOD LEFT JOIN MDO ON MDO.NoID=MDOD.IDDO WHERE MDOD.IDDO=" & IDDO)
            If ds.Tables("MDOD").Rows.Count >= 1 Then
                If Not NullTobool(ds.Tables("MDOD").Rows(0).Item("IsPosted")) Then
                    EksekusiSQL("UPDATE MDO SET IsPosted=1, IDUserPosting=" & IDUserAktif & ", TglPosting=Getdate() WHERE NoID=" & IDDO)
                End If
            End If
        Catch ex As Exception
            DevExpress.XtraEditors.XtraMessageBox.Show("Kesalahan perhitugan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
        Finally
            ds.Dispose()
        End Try
    End Sub
    Public Shared Sub UnPostingDO(ByVal IDDO As Long)
        Dim ds As New DataSet
        Try
            If Not AdaDiPenjualan(IDDO) Then
                EksekusiSQL("UPDATE MDO SET IsPosted=0, IDUserPosting=NULL, TglPosting=NULL  WHERE NoID=" & IDDO)
            End If
        Catch ex As Exception
            DevExpress.XtraEditors.XtraMessageBox.Show("Kesalahan perhitugan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
        Finally
            ds.Dispose()
        End Try
    End Sub
    Public Shared Sub PostingStokBarangPenjualan(ByVal IDJual As Long)
        Dim SQL As String = ""
        Dim SaldoStockAkhir As Double = 0.0
        Dim HPP As Double = 0.0
        Dim IDKartuStok As Long = -1, IDDepartemen As Long = -1, IDJenisBarang As Long = -1, IDCustomer As Long = -1

        Dim con As New SqlConnection
        Dim com As New SqlCommand
        Dim oDA As New SqlDataAdapter
        Dim Trans As SqlTransaction = Nothing
        Dim ds As New DataSet
        Try
            con.ConnectionString = StrKonSql
            con.Open()
            com.Connection = con
            oDA.SelectCommand = com
            Trans = con.BeginTransaction("MJual")
            com.Transaction = Trans
            'Transaction Terbuat
            If Not ds.Tables("MJualD") Is Nothing Then
                ds.Tables("MJualD").Clear()
            End If
            com.CommandText = "SELECT MJualD.*, MJual.Kode AS NoJual, MJual.IsPosted, MJual.Tanggal AS TGLJual, (MJualD.Jumlah/MJualD.Qty)-((MJualD.Jumlah/MJualD.Qty)*MJual.DiskonNotaProsen/100) AS HargaJual FROM MJualD LEFT JOIN MJual ON MJual.NoID=MJualD.IDJual WHERE MJualD.Qty<>0 and MJualD.IDJual=" & IDJual
            oDA.Fill(ds, "MJualD")
            If ds.Tables("MJualD").Rows.Count >= 1 Then
                If Not NullToBool(ds.Tables("MJualD").Rows(0).Item("IsPosted")) Then
                    com.CommandText = "DELETE FROM MKartuStok WHERE IDTransaksi=" & IDJual & " AND IDJenisTransaksi=6"
                    com.ExecuteNonQuery()
                    For i As Integer = 0 To ds.Tables("MJualD").Rows.Count - 1
                        SaldoStockAkhir = 0 'GetStockAkhirGudang(NullToLong(ds.Tables("MJualD").Rows(i).Item("IDBarang")), NullToLong(ds.Tables("MJualD").Rows(i).Item("IDGudang")), NullToDate(ds.Tables("MJualD").Rows(i).Item("TGLJual")))
                        com.CommandText = "SELECT IsBarangPaket FROM MBarang WHERE NoID=" & NullToLong(ds.Tables(0).Rows(i).Item("IDBarang"))
                        If NullToBool(com.ExecuteScalar()) Then
                            SQL = "SELECT MBarangDPaket.*, MBarang.NoID AS IDBarangHeader, MBarang.IsBarangPaket, MBarang.IsBarangPaket, MBarangD.Konversi, MBarangD.IDSatuan " & _
                                  " FROM MBarangDPaket LEFT JOIN (MBarang INNER JOIN MBarangD ON MBarang.NoID=MBarangD.IDBarang) ON MBarangD.NoID=MBarangDPaket.IDBarangD  WHERE MBarangDPaket.IDBarang=" & NullToLong(ds.Tables(0).Rows(i).Item("IDBarang"))
                            If Not ds.Tables("MBarangDPaket") Is Nothing Then
                                ds.Tables("MBarangDPaket").Clear()
                            End If
                            oDA.Fill(ds, "MBarangDPaket")
                            For x As Integer = 0 To ds.Tables("MBarangDPaket").Rows.Count - 1
                                If NullToBool(ds.Tables("MBarangDPaket").Rows(x).Item("IsBarangPaket")) Then
                                    SQL = "SELECT MBarangDPaket.*, MBarang.NoID AS IDBarangHeader, MBarang.IsBarangPaket, MBarang.IsBarangPaket, MBarangD.Konversi, MBarangD.IDSatuan " & _
                                          " FROM MBarangDPaket LEFT JOIN (MBarang INNER JOIN MBarangD ON MBarang.NoID=MBarangD.IDBarang) ON MBarangD.NoID=MBarangDPaket.IDBarangD  WHERE MBarangDPaket.IDBarang=" & NullToLong(ds.Tables("MBarangDPaket").Rows(x).Item("IDBarangHeader"))
                                    com.CommandText = SQL
                                    If Not ds.Tables("MJualDPaket") Is Nothing Then
                                        ds.Tables("MJualDPaket").Clear()
                                    End If
                                    oDA.Fill(ds, "MJualDPaket")
                                    For a As Integer = 0 To ds.Tables("MJualDPaket").Rows.Count - 1
                                        com.CommandText = "SELECT IDCustomer FROM MJual WHERE NoID=" & NullToLong(IDJual)
                                        IDCustomer = NullToLong(com.ExecuteScalar())
                                        com.CommandText = "SELECT MBarang.IDJenis FROM MBarang WHERE MBarang.NoID=" & NullToLong(ds.Tables("MJualDPaket").Rows(a).Item("IDBarangHeader"))
                                        IDJenisBarang = NullToLong(com.ExecuteScalar())
                                        com.CommandText = "SELECT MWilayah.IDDepartemen FROM MWilayah INNER JOIN MJual ON MJual.IDWilayah=MWilayah.NoID WHERE MJual.NoID=" & NullToLong(IDJual)
                                        IDDepartemen = NullToLong(com.ExecuteScalar())
                                        com.CommandText = "SELECT MAX(MKartuStok.NoID) FROM MKartuStok"
                                        IDKartuStok = NullToLong(com.ExecuteScalar()) + 1
                                        SQL = " INSERT INTO [dbo].[MKartuStok]"
                                        SQL &= " ([NoID]"
                                        SQL &= " ,[Kode]"
                                        SQL &= " ,[IDBarang]"
                                        SQL &= " ,[IDBarangD]"
                                        SQL &= " ,[IDDepartemen]"
                                        SQL &= " ,[IDGudang]"
                                        SQL &= " ,[Tanggal]"
                                        SQL &= " ,[IDJenisTransaksi]"
                                        SQL &= " ,[IDTransaksi]"
                                        SQL &= " ,[IDTransaksiDetil]"
                                        SQL &= " ,[Keterangan]"
                                        SQL &= " ,[QtyKeluarA]"
                                        SQL &= " ,[QtyKeluar]"
                                        SQL &= " ,[HgBeliPeritem]"
                                        SQL &= " ,[JumlahBeli]"
                                        SQL &= " ,[QtyMasukA]"
                                        SQL &= " ,[QtyMasuk]"
                                        SQL &= " ,[HargaJualPerItem]"
                                        SQL &= " ,[JumlahJual]"
                                        SQL &= " ,[HPPKeluar]"
                                        SQL &= " ,[JumHPPKeluar]"
                                        SQL &= " ,[QtyAkhir]"
                                        SQL &= " ,[HgBeliRtRtPeritem]"
                                        SQL &= " ,[TotHPPRataRata]"
                                        SQL &= " ,[IDJenis]"
                                        SQL &= " ,[IDSatuan]"
                                        SQL &= " ,[Konversi]"
                                        SQL &= " ,[IDAlamat]"
                                        SQL &= " ,[Ctn_Duz]) VALUES (" & vbCrLf
                                        SQL &= IDKartuStok & ", "
                                        SQL &= " '" & FixApostropi(ds.Tables("MJualD").Rows(i).Item("NoJual")) & "', "
                                        SQL &= NullToLong(ds.Tables("MJualDPaket").Rows(a).Item("IDBarangHeader")) & ", "
                                        SQL &= NullToLong(ds.Tables("MJualDPaket").Rows(a).Item("IDBarangD")) & ", "
                                        SQL &= NullToLong(IDDepartemen) & ", "
                                        SQL &= NullToLong(ds.Tables("MJualD").Rows(i).Item("IDGudang")) & ", "
                                        SQL &= "'" & NullToDate(ds.Tables("MJualD").Rows(i).Item("TglJual")).ToString("yyyy/MM/dd HH:mm") & "'" & ", 6, " & IDJual & ", "
                                        SQL &= NullToLong(ds.Tables("MJualD").Rows(i).Item("NoID")) & ", "
                                        SQL &= "'" & FixApostropi(NullToStr(ds.Tables("MJualD").Rows(i).Item("Catatan"))) & "', "
                                        SQL &= FixKoma(NullToDbl(ds.Tables("MJualDPaket").Rows(a).Item("Qty")) * NullToDbl(ds.Tables("MJualDPaket").Rows(a).Item("Konversi"))) & ", 0, "
                                        SQL &= FixKoma(NullToDbl(ds.Tables("MJualDPaket").Rows(a).Item("Qty"))) & ", "
                                        SQL &= FixKoma(0) & ", "
                                        SQL &= FixKoma(0) & ", 0, 0, "
                                        SQL &= " 0,0,0,0, "
                                        SQL &= FixKoma(SaldoStockAkhir - NullToDbl(ds.Tables("MJualDPaket").Rows(a).Item("Qty")) * NullToDbl(ds.Tables("MJualDPaket").Rows(a).Item("Konversi"))) & ", "
                                        SQL &= " 0, "
                                        SQL &= NullToLong(IDJenisBarang) & ", "
                                        SQL &= NullToLong(ds.Tables("MJualDPaket").Rows(a).Item("IDSatuan")) & ", "
                                        SQL &= FixKoma(NullToDbl(ds.Tables("MJualDPaket").Rows(a).Item("Konversi"))) & ", "
                                        SQL &= NullToLong(IDCustomer) & ", 0)"
                                        com.CommandText = SQL
                                        com.ExecuteNonQuery()

                                        com.CommandText = "SELECT HPP FROM MBarang WHERE NoID=" & NullToLong(ds.Tables("MJualDPaket").Rows(a).Item("IDBarangHeader"))
                                        HPP = NullToDbl(com.ExecuteScalar())
                                        
                                        SQL = "UPDATE MKartuStok SET HPP=" & FixKoma(HPP) & ", HargaBeli=" & FixKoma(NullToDbl(ds.Tables("MJualD").Rows(i).Item("HargaJual"))) & " WHERE NoID=" & IDKartuStok
                                        com.CommandText = SQL
                                        com.ExecuteNonQuery()
                                    Next
                                Else
                                    com.CommandText = "SELECT IDCustomer FROM MJual WHERE NoID=" & NullToLong(IDJual)
                                    IDCustomer = NullToLong(com.ExecuteScalar())
                                    com.CommandText = "SELECT MBarang.IDJenis FROM MBarang WHERE MBarang.NoID=" & NullToLong(ds.Tables("MBarangDPaket").Rows(x).Item("IDBarangHeader"))
                                    IDJenisBarang = NullToLong(com.ExecuteScalar())
                                    com.CommandText = "SELECT MWilayah.IDDepartemen FROM MWilayah INNER JOIN MJual ON MJual.IDWilayah=MWilayah.NoID WHERE MJual.NoID=" & NullToLong(IDJual)
                                    IDDepartemen = NullToLong(com.ExecuteScalar())
                                    com.CommandText = "SELECT MAX(MKartuStok.NoID) FROM MKartuStok"
                                    IDKartuStok = NullToLong(com.ExecuteScalar()) + 1
                                    SQL = " INSERT INTO [dbo].[MKartuStok]"
                                    SQL &= " ([NoID]"
                                    SQL &= " ,[Kode]"
                                    SQL &= " ,[IDBarang]"
                                    SQL &= " ,[IDBarangD]"
                                    SQL &= " ,[IDDepartemen]"
                                    SQL &= " ,[IDGudang]"
                                    SQL &= " ,[Tanggal]"
                                    SQL &= " ,[IDJenisTransaksi]"
                                    SQL &= " ,[IDTransaksi]"
                                    SQL &= " ,[IDTransaksiDetil]"
                                    SQL &= " ,[Keterangan]"
                                    SQL &= " ,[QtyKeluarA]"
                                    SQL &= " ,[QtyKeluar]"
                                    SQL &= " ,[HgBeliPeritem]"
                                    SQL &= " ,[JumlahBeli]"
                                    SQL &= " ,[QtyMasukA]"
                                    SQL &= " ,[QtyMasuk]"
                                    SQL &= " ,[HargaJualPerItem]"
                                    SQL &= " ,[JumlahJual]"
                                    SQL &= " ,[HPPKeluar]"
                                    SQL &= " ,[JumHPPKeluar]"
                                    SQL &= " ,[QtyAkhir]"
                                    SQL &= " ,[HgBeliRtRtPeritem]"
                                    SQL &= " ,[TotHPPRataRata]"
                                    SQL &= " ,[IDJenis]"
                                    SQL &= " ,[IDSatuan]"
                                    SQL &= " ,[Konversi]"
                                    SQL &= " ,[IDAlamat]"
                                    SQL &= " ,[Ctn_Duz]) VALUES (" & vbCrLf
                                    SQL &= IDKartuStok & ", "
                                    SQL &= " '" & FixApostropi(ds.Tables(0).Rows(i).Item("NoJual")) & "', "
                                    SQL &= NullToLong(ds.Tables("MBarangDPaket").Rows(x).Item("IDBarangHeader")) & ", "
                                    SQL &= NullToLong(ds.Tables("MBarangDPaket").Rows(x).Item("IDBarangD")) & ", "
                                    SQL &= NullToLong(IDDepartemen) & ", "
                                    SQL &= NullToLong(ds.Tables("MJualD").Rows(i).Item("IDGudang")) & ", "
                                    SQL &= "'" & NullToDate(ds.Tables("MJualD").Rows(i).Item("TglJual")).ToString("yyyy/MM/dd HH:mm") & "'" & ", 6, " & IDJual & ", "
                                    SQL &= NullToLong(ds.Tables("MJualD").Rows(i).Item("NoID")) & ", "
                                    SQL &= "'" & FixApostropi(NullToStr(ds.Tables("MJualD").Rows(i).Item("Catatan"))) & "', "
                                    SQL &= FixKoma(NullToDbl(ds.Tables("MBarangDPaket").Rows(x).Item("Qty")) * NullToDbl(ds.Tables("MBarangDPaket").Rows(x).Item("Konversi"))) & ", 0, "
                                    SQL &= FixKoma(NullToDbl(ds.Tables("MBarangDPaket").Rows(x).Item("Qty"))) & ", "
                                    SQL &= FixKoma(0) & ", "
                                    SQL &= FixKoma(0) & ", 0, 0, "
                                    SQL &= " 0,0,0,0, "
                                    SQL &= FixKoma(SaldoStockAkhir - NullToDbl(ds.Tables("MBarangDPaket").Rows(x).Item("Qty")) * NullToDbl(ds.Tables("MBarangDPaket").Rows(x).Item("Konversi"))) & ", "
                                    SQL &= " 0, "
                                    SQL &= NullToLong(IDJenisBarang) & ", "
                                    SQL &= NullToLong(ds.Tables("MBarangDPaket").Rows(x).Item("IDSatuan")) & ", "
                                    SQL &= FixKoma(NullToDbl(ds.Tables("MBarangDPaket").Rows(x).Item("Konversi"))) & ", "
                                    SQL &= NullToLong(IDCustomer) & ", 0)"
                                    com.CommandText = SQL
                                    com.ExecuteNonQuery()
                                    
                                    com.CommandText = "SELECT HPP FROM MBarang WHERE NoID=" & NullToLong(ds.Tables("MBarangDPaket").Rows(x).Item("IDBarangHeader"))
                                    HPP = NullToDbl(com.ExecuteScalar())

                                    SQL = "UPDATE MKartuStok SET HPP=" & FixKoma(HPP) & ", HargaBeli=" & FixKoma(NullToDbl(ds.Tables("MJualD").Rows(i).Item("HargaJual"))) & " WHERE NoID=" & IDKartuStok
                                    com.CommandText = SQL
                                    com.ExecuteNonQuery()
                                End If
                            Next
                        Else
                            'com.CommandText = "SELECT IDCustomer FROM MJual WHERE NoID=" & NullToLong(IDJual)
                            'IDCustomer = NullToLong(com.ExecuteScalar())
                            'com.CommandText = "SELECT MBarang.IDJenis FROM MBarang WHERE MBarang.NoID=" & NullToLong(ds.Tables("MJualD").Rows(i).Item("IDBarangHeader"))
                            'IDJenisBarang = NullToLong(com.ExecuteScalar())
                            com.CommandText = "SELECT MWilayah.IDDepartemen FROM MWilayah INNER JOIN MJual ON MJual.IDWilayah=MWilayah.NoID WHERE MJual.NoID=" & NullToLong(IDJual)
                            IDDepartemen = NullToLong(com.ExecuteScalar())
                            com.CommandText = "SELECT MAX(MKartuStok.NoID) FROM MKartuStok"
                            IDKartuStok = NullToLong(com.ExecuteScalar()) + 1
                            SQL = " INSERT INTO [dbo].[MKartuStok]"
                            SQL &= " ([NoID]"
                            SQL &= " ,[Kode]"
                            SQL &= " ,[IDBarang]"
                            SQL &= " ,[IDBarangD]"
                            SQL &= " ,[IDDepartemen]"
                            SQL &= " ,[IDGudang]"
                            SQL &= " ,[Tanggal]"
                            SQL &= " ,[IDJenisTransaksi]"
                            SQL &= " ,[IDTransaksi]"
                            SQL &= " ,[IDTransaksiDetil]"
                            SQL &= " ,[Keterangan]"
                            SQL &= " ,[QtyKeluarA]"
                            SQL &= " ,[QtyKeluar]"
                            SQL &= " ,[HgBeliPeritem]"
                            SQL &= " ,[JumlahBeli]"
                            SQL &= " ,[QtyMasukA]"
                            SQL &= " ,[QtyMasuk]"
                            SQL &= " ,[HargaJualPerItem]"
                            SQL &= " ,[JumlahJual]"
                            SQL &= " ,[HPPKeluar]"
                            SQL &= " ,[JumHPPKeluar]"
                            SQL &= " ,[QtyAkhir]"
                            SQL &= " ,[HgBeliRtRtPeritem]"
                            SQL &= " ,[TotHPPRataRata]"
                            SQL &= " ,[IDJenis]"
                            SQL &= " ,[IDSatuan]"
                            SQL &= " ,[Konversi]"
                            SQL &= " ,[IDAlamat]"
                            SQL &= " ,[Ctn_Duz])" & vbCrLf
                            SQL &= " SELECT " & IDKartuStok & " AS NoID, MJual.Kode, MJualD.IDBarang, MJualD.IDBarangD,  "
                            SQL &= " MWilayah.IDDepartemen, MJualD.IDGudang, "
                            SQL &= " MJual.Tanggal, 6 AS IDJenisTransaksi,  MJual.NoID AS IDTransaksi, "
                            SQL &= " MJualD.NoID AS IDTransaksiDetil, MJualD.Catatan, ISNULL(MJualD.Qty,0)*IsNull(MJualD.Konversi,0) AS Masuk, MJualD.Qty AS MasukA, "
                            SQL &= " MJualD.Harga AS HrgBeliPerItem, ISNULL(MJualD.Jumlah,0) AS JumlahBeli, 0 AS Keluar, 0 AS KeluarA, "
                            SQL &= " 0,0,0 AS HPPKeluar, "
                            SQL &= " 0 AS JumHPPKeluar, " & Replace(SaldoStockAkhir, ",", ".") & "+(ISNULL(MJualD.Qty,0)*IsNull(MJualD.Konversi,0)) as QtyAkhir, 0,"
                            SQL &= " 0, MBarang.IDJenis, MJualD.IDSatuan, "
                            SQL &= " IsNull(MJualD.Konversi,0), MJual.IDCustomer, MJualD.Ctn "
                            SQL &= " FROM MJualD LEFT JOIN MJual ON MJual.NoID=MJualD.IDJual"
                            SQL &= " LEFT JOIN (MGudang LEFT JOIN MWilayah ON MWilayah.NoID=MGudang.IDWilayah) ON MJualD.IDGudang=MGudang.NoID"
                            SQL &= " LEFT JOIN MBarang ON MBarang.NoID=MJualD.IDBarang"
                            SQL &= " WHERE MJualD.NoID=" & NullToLong(ds.Tables("MJualD").Rows(i).Item("NoID"))
                            com.CommandText = SQL
                            com.ExecuteNonQuery()

                            com.CommandText = "SELECT HPP FROM MBarang WHERE NoID=" & NullToLong(ds.Tables("MJualD").Rows(i).Item("IDBarang"))
                            HPP = NullToDbl(com.ExecuteScalar())

                            SQL = "UPDATE MBarang SET HPP=" & FixKoma(HPP) & " WHERE MBarang.NoID=" & NullToLong(ds.Tables("MJualD").Rows(i).Item("IDBarang"))
                            com.CommandText = SQL
                            com.ExecuteNonQuery()
                            SQL = "UPDATE MKartuStok SET HPP=" & FixKoma(HPP) & ", HargaBeli=" & FixKoma(NullToDbl(ds.Tables("MJualD").Rows(i).Item("HargaJual"))) & " WHERE NoID=" & IDKartuStok
                            com.CommandText = SQL
                            com.ExecuteNonQuery()
                        End If
                    Next
                    com.CommandText = "UPDATE MJual SET IsPosted=1 WHERE NoID=" & IDJual
                    com.ExecuteNonQuery()

                    Trans.Commit()
                    ''U/ Posting Kartu Hutang
                    'If IsPostingKartuHutang Then
                    '    SQL = "SELECT MJual.Kode, MJual.Tanggal, MJual.JatuhTempo, " & _
                    '          " MJual.IDCustomer AS IDAlamat, MJual.IDWilayah, MJual.Keterangan, " & _
                    '          " MJual.Subtotal AS Debet, 0 AS Kredit, MJual.DiskonNotaTotal AS Diskon, " & _
                    '          " MJual.Biaya, MJual.Bayar AS Dibayar, MJual.Sisa, MJual.IsPosted, 0 AS IsSaldoAwal " & _
                    '          " FROM MJual " & _
                    '          " WHERE NoID=" & IDTransaksi
                    '    If Not ds.Tables("MHutang") Is Nothing Then
                    '        ds.Tables("MHutang").Clear()
                    '    End If
                    '    com.CommandText = SQL
                    '    oDA.Fill(ds, "MHutang")
                    '    If ds.Tables("MHutang").Rows.Count >= 1 Then
                    '        If Not NullToBool(ds.Tables("MHutang").Rows(0).Item("IsPosted")) Then
                    '            com.CommandText = "DELETE FROM MHutang WHERE IDTransaksi=" & IDTransaksi & " AND IDJenisTransaksi=6"
                    '            com.ExecuteNonQuery()
                    '            For i As Integer = 0 To ds.Tables("MHutang").Rows.Count - 1
                    '                IDHutang = NullToLong(GetNewID("MHutang", "NoID"))
                    '                SQL = "INSERT INTO MHutang (NoID, Kode, Tanggal, JatuhTempo, IDAlamat, IDWilayah, IDTransaksi, IDJenisTransaksi, Keterangan, Debet, DebetA, Kredit, KreditA, Diskon, Biaya, Dibayar, Sisa, IsSaldoAwal) VALUES (" & _
                    '                      IDHutang & ", " & _
                    '                      "'" & FixApostropi(NullToStr(ds.Tables("MHutang").Rows(i).Item("Kode"))) & "', " & _
                    '                      "'" & NullToDate(ds.Tables("MHutang").Rows(i).Item("Tanggal")).ToString("yyyy-MM-dd HH:mm") & "', " & _
                    '                      "'" & NullToDate(ds.Tables("MHutang").Rows(i).Item("JatuhTempo")).ToString("yyyy-MM-dd HH:mm") & "', " & _
                    '                      NullToLong(ds.Tables("MHutang").Rows(i).Item("IDAlamat")) & ", " & _
                    '                      NullToLong(ds.Tables("MHutang").Rows(i).Item("IDWilayah")) & ", " & _
                    '                      IDTransaksi & ", " & _
                    '                      6 & ", " & _
                    '                      "'" & FixApostropi(NullToStr(ds.Tables("MHutang").Rows(i).Item("Keterangan"))) & "', " & _
                    '                      FixKoma(NullToDbl(ds.Tables("MHutang").Rows(i).Item("Debet"))) & ", " & _
                    '                      FixKoma(NullToDbl(ds.Tables("MHutang").Rows(i).Item("Debet"))) & ", " & _
                    '                      FixKoma(NullToDbl(ds.Tables("MHutang").Rows(i).Item("Debet"))) & ", " & _
                    '                      FixKoma(NullToDbl(ds.Tables("MHutang").Rows(i).Item("Debet"))) & ", " & _
                    '                      FixKoma(NullToDbl(ds.Tables("MHutang").Rows(i).Item("Diskon"))) & ", " & _
                    '                      FixKoma(NullToDbl(ds.Tables("MHutang").Rows(i).Item("Biaya"))) & ", " & _
                    '                      FixKoma(NullToDbl(ds.Tables("MHutang").Rows(i).Item("Dibayar"))) & ", " & _
                    '                      FixKoma(NullToDbl(ds.Tables("MHutang").Rows(i).Item("Sisa"))) & ", " & _
                    '                      NullToLong(ds.Tables("MHutang").Rows(i).Item("IsSaldoAwal")) & ")"
                    '                com.CommandText = SQL
                    '                com.ExecuteNonQuery()
                    '            Next
                    '        End If
                    '    End If
                    'End If


                End If
            Else
                com.CommandText = "UPDATE MJual SET IsPosted=1 WHERE NoID=" & IDJual
                com.ExecuteNonQuery()
                Trans.Commit()
            End If
        Catch ex As Exception
            DevExpress.XtraEditors.XtraMessageBox.Show("Kesalahan perhitugan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
            If Not Trans Is Nothing Then
                Trans.Rollback()
            End If
        Finally
            If con.State = ConnectionState.Open Then
                con.Close()
            End If
            con.Dispose()
            com.Dispose()
            oDA.Dispose()
            Trans = Nothing
            ds.Dispose()
        End Try
    End Sub
    Public Shared Sub UnPostingStokBarangPenjualan(ByVal IDJual As Long)
        Dim ds As New DataSet
        Try
            ds = ExecuteDataset("NamaTabel", "SELECT MKartuStok.* FROM MKartuStok WHERE (MKartuStok.IsPosted=0 OR MKartuStok.IsPosted Is Null) AND MKartuStok.IDTransaksi=" & IDJual & " AND MKartuStok.IDJenisTransaksi=6 ")
            If ds.Tables(0).Rows.Count >= 1 Then
                If Not NullToBool(ds.Tables(0).Rows(0).Item("IsPosted")) Then
                    If Not AdaDiSuratJalan(IDJual) AndAlso Not AdaDiReturJual(IDJual) AndAlso Not AdaDiRubahHargaJual(IDJual) Then
                        EksekusiSQL("DELETE FROM MKartuStok WHERE IDTransaksi=" & IDJual & " AND IDJenisTransaksi=6")
                        EksekusiSQL("UPDATE MJual SET IsPosted=0 WHERE NoID=" & IDJual)
                    End If
                End If
            ElseIf ds.Tables(0).Rows.Count = 0 Then
                EksekusiSQL("UPDATE MJual SET IsPosted=0 WHERE NoID=" & IDJual)
            End If
        Catch ex As Exception
            DevExpress.XtraEditors.XtraMessageBox.Show("Kesalahan perhitugan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
        Finally
            ds.Dispose()
        End Try
    End Sub
    Public Shared Sub PostingStokBarangReturPenjualan(ByVal IDRetur As Long)
        Dim ds As New DataSet
        Dim SQL As String = ""
        Dim SaldoStockAkhir As Double = 0.0
        Dim HPP As Double = 0.0
        Dim IDKartuStok As Long = -1
        Try
            ds = modSqlServer.ExecuteDataset("MJualD", "SELECT MReturJualD.*, MReturJual.IsPosted, MReturJual.Tanggal AS TGLJual, (MReturJualD.Jumlah/MReturJualD.Qty)-((MReturJualD.Jumlah/MReturJualD.Qty)*MReturJual.DiskonNotaProsen/100) AS HargaReturJual FROM MReturJualD LEFT JOIN MReturJual ON MReturJual.NoID=MReturJualD.IDReturJual WHERE MReturJualD.IDReturJual=" & IDRetur)
            If ds.Tables("MJualD").Rows.Count >= 1 Then
                If Not NullTobool(ds.Tables("MJualD").Rows(0).Item("IsPosted")) Then
                    EksekusiSQL("DELETE FROM MKartuStok WHERE IDTransaksi=" & IDRetur & " AND IDJenisTransaksi=7")
                    For i As Integer = 0 To ds.Tables("MJualD").Rows.Count - 1
                        SaldoStockAkhir = 0 'GetStockAkhirGudang(NullToLong(ds.Tables("MJualD").Rows(i).Item("IDBarang")), NullToLong(ds.Tables("MJualD").Rows(i).Item("IDGudang")), NullToDate(ds.Tables("MJualD").Rows(i).Item("TGLJual")))
                        IDKartuStok = GetNewID("MKartuStok", "NoID")
                        SQL = " INSERT INTO [dbo].[MKartuStok]"
                        SQL &= " ([NoID]"
                        SQL &= " ,[Kode]"
                        SQL &= " ,[IDBarang]"
                        SQL &= " ,[IDDepartemen]"
                        SQL &= " ,[IDGudang]"
                        SQL &= " ,[Tanggal]"
                        SQL &= " ,[IDJenisTransaksi]"
                        SQL &= " ,[IDTransaksi]"
                        SQL &= " ,[IDTransaksiDetil]"
                        SQL &= " ,[Keterangan]"
                        SQL &= " ,[QtyMasukA]"
                        SQL &= " ,[QtyMasuk]"
                        SQL &= " ,[HargaJualPerItem]"
                        SQL &= " ,[JumlahJual]"
                        SQL &= " ,[QtyKeluarA]"
                        SQL &= " ,[QtyKeluar]"
                        SQL &= " ,[HgBeliPeritem]"
                        SQL &= " ,[JumlahBeli]"
                        SQL &= " ,[HPPKeluar]"
                        SQL &= " ,[JumHPPKeluar]"
                        SQL &= " ,[QtyAkhir]"
                        SQL &= " ,[HgBeliRtRtPeritem]"
                        SQL &= " ,[TotHPPRataRata]"
                        SQL &= " ,[IDJenis]"
                        SQL &= " ,[IDSatuan]"
                        SQL &= " ,[Konversi]"
                        SQL &= " ,[IDAlamat])" & vbCrLf
                        SQL &= " SELECT " & IDKartuStok & " AS NoID, MReturJual.Kode, MReturJualD.IDBarang, "
                        SQL &= " MWilayah.IDDepartemen, MReturJualD.IDGudang, "
                        SQL &= " MReturJual.Tanggal, 7 AS IDJenisTransaksi, MReturJual.NoID AS IDTransaksi, "
                        SQL &= " MReturJualD.NoID AS IDTransaksiDetil, MReturJualD.Catatan, (ISNULL(MReturJualD.Qty,0)*ISNULL(MReturJualD.Konversi,0)) AS Keluar, MReturJualD.Qty AS KeluarA,"
                        SQL &= " MReturJualD.Harga AS HrgJualPerItem, ISNULL(MReturJualD.Jumlah,0), 0 AS Masuk,0 AS MasukA,"
                        SQL &= " 0,0,0 AS HPPKeluar, "
                        SQL &= " 0 AS JumHPPKeluar, " & Replace(SaldoStockAkhir, ",", ".") & "-(ISNULL(MReturJualD.Qty,0)*ISNULL(MReturJualD.Konversi,0)) as QtyAkhir, 0,"
                        SQL &= " 0, MBarang.IDJenis, MReturJualD.IDSatuan, "
                        SQL &= " Isnull(MReturJualD.Konversi,0), MReturJual.IDCustomer "
                        SQL &= " FROM MReturJualD LEFT JOIN MReturJual ON MReturJual.NoID=MReturJualD.IDReturJual"
                        SQL &= " LEFT JOIN (MGudang LEFT JOIN MWilayah ON MWilayah.NoID=MGudang.IDWilayah) ON MReturJualD.IDGudang=MGudang.NoID"
                        SQL &= " LEFT JOIN MBarang ON MBarang.NoID=MReturJualD.IDBarang"
                        SQL &= " WHERE MReturJualD.NoID=" & NullTolong(ds.Tables("MJualD").Rows(i).Item("NoID"))
                        EksekusiSQL(SQL)

                        HPP = NullToDbl(EksekusiSQlSkalarNew("SELECT HPP FROM MBarang WHERE NoID=" & NullToLong(ds.Tables(0).Rows(i).Item("IDBarang"))))

                        SQL = "UPDATE MBarang SET HPP=" & FixKoma(HPP) & " WHERE MBarang.NoID=" & NullToLong(ds.Tables(0).Rows(i).Item("IDBarang"))
                        EksekusiSQL(SQL)
                        SQL = "UPDATE MKartuStok SET HPP=" & FixKoma(HPP) & ", HargaBeli=" & FixKoma(NullToDbl(ds.Tables(0).Rows(i).Item("HargaReturJual"))) & " WHERE NoID=" & IDKartuStok
                        EksekusiSQL(SQL)
                    Next
                    EksekusiSQL("UPDATE MReturJual SET IsPosted=1 WHERE NoID=" & IDRetur)
                End If
            End If
        Catch ex As Exception
            DevExpress.XtraEditors.XtraMessageBox.Show("Kesalahan perhitugan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
        Finally
            ds.Dispose()
        End Try
    End Sub
    Public Shared Sub UnPostingStokBarangReturPenjualan(ByVal IDJual As Long)
        Dim ds As New DataSet
        Try
            ds = ExecuteDataset("NamaTabel", "SELECT MKartuStok.* FROM MKartuStok WHERE (MKartuStok.IsPosted=0 OR MKartuStok.IsPosted Is Null) AND MKartuStok.IDTransaksi=" & IDJual & " AND MKartuStok.IDJenisTransaksi=7 ")
            If ds.Tables(0).Rows.Count >= 1 Then
                If Not NullTobool(ds.Tables(0).Rows(0).Item("IsPosted")) Then
                    EksekusiSQL("DELETE FROM MKartuStok WHERE IDTransaksi=" & IDJual & " AND MKartuStok.IDJenisTransaksi=7")
                    EksekusiSQL("UPDATE MReturJual SET IsPosted=0 WHERE NoID=" & IDJual)
                End If
            ElseIf ds.Tables(0).Rows.Count = 0 Then
                EksekusiSQL("UPDATE MReturJual SET IsPosted=0 WHERE NoID=" & IDJual)
            End If
        Catch ex As Exception
            DevExpress.XtraEditors.XtraMessageBox.Show("Kesalahan perhitugan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
        Finally
            ds.Dispose()
        End Try
    End Sub
End Class
