Imports VPoint.Ini
Imports System.Data
Imports System.Data.SqlClient
Imports DevExpress.XtraEditors
Imports DevExpress.Utils
Imports VPoint.clsPostingKartuStok

Public Class clsPostingPembelian
    'Public Enum JenisTransaksi
    '    Pembelian = 2
    '    Penjualan = 6
    '    MutasiGudang = 4
    'End Enum

    Public Shared Function IsLockPeriodeFP(ByVal Periode As Date) As Boolean
        Dim x As Boolean = False
        Dim SQL As String = ""
        Dim ds As New DataSet
        Try
            SQL = "SELECT * FROM MLockFP WHERE MONTH(Periode)=" & Periode.Month & " AND YEAR(Periode)=" & Periode.Year
            ds = ExecuteDataset("MLockFP", SQL)
            If ds.Tables("MLockFP").Rows.Count >= 1 Then
                x = NullToBool(ds.Tables("MLockFP").Rows(0).Item("IsLock"))
            Else
                x = False
            End If
        Catch ex As Exception
            XtraMessageBox.Show("Info Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
            x = False
        Finally
            If Not ds Is Nothing Then
                ds.Dispose()
            End If
        End Try
        Return x
    End Function
    Public Shared Function GetHargaBeliterakhir(ByVal IDBarang As Long)
        Dim x As Double = 0
        Dim SQL As String = ""
        Dim ds As New DataSet
        Dim ds1 As New DataSet
        Try
            If IsUpdateHargaBeli Then
                SQL = "SELECT MBarang.* FROM MBarang WHERE MBarang.NOID=" & IDBarang
                ds = ExecuteDataset("MBarang", SQL)
                If ds.Tables("MBarang").Rows.Count >= 1 Then
                    x = NullToDbl(ds.Tables("MBarang").Rows(0).Item("HargaBeli"))
                End If
            Else
                SQL = "SELECT TOP 1 MBeliD.NoID, ROUND(MBeliD.HargaPcs-(MBeliD.HargaPcs*MBeli.DiskonNotaProsen/100),0) AS HargaNetto, MBeli.DiskonNotaProsen FROM MBeliD INNER JOIN MBeli ON MBeli.NoID=MBeliD.IDBeli WHERE MBeliD.IDBarang=" & IDBarang & " Order By MBeli.Tanggal Desc, MBeliD.Tgl Desc"
                ds = ExecuteDataset("MBeli", SQL)
                If ds.Tables("MBeli").Rows.Count >= 1 Then
                    SQL = "SELECT TOP 1 MRevisiHargaBeliD.NoID, ROUND(MRevisiHargaBeliD.HargaPcsBaru-(MRevisiHargaBeliD.HargaPcsBaru*MBeli.DiskonNotaProsen/100),0) AS HargaNetto, MBeli.DiskonNotaProsen " & _
                          " FROM (MRevisiHargaBeliD INNER JOIN MRevisiHargaBeli ON MRevisiHargaBeli.NoID=MRevisiHargaBeliD.IDRevisiHargaBeli) LEFT JOIN (MBeliD INNER JOIN MBeli ON MBeli.NoID=MBeliD.IDBeli) ON MBeliD.NoID=MRevisiHargaBeliD.IDBeliD AND MBeli.NoID=MRevisiHargaBeliD.IDBeli " & _
                          " WHERE MBeliD.NOID=" & NullToLong(ds.Tables("MBeli").Rows(0).Item("NoID")) & _
                          " Order By MRevisiHargaBeli.Tanggal Desc, MRevisiHargaBeliD.NoID Desc"
                    ds1 = ExecuteDataset("MRevisiHargaBeli", SQL)
                    If ds1.Tables("MRevisiHargaBeli").Rows.Count >= 1 Then
                        x = NullToDbl(ds1.Tables("MRevisiHargaBeli").Rows(0).Item("HargaNetto"))
                    Else
                        x = NullToDbl(ds.Tables("MBeli").Rows(0).Item("HargaNetto"))
                    End If
                End If
            End If
        Catch ex As Exception
            XtraMessageBox.Show("Info : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
        Finally
            ds.Dispose()
        End Try
        Return x
    End Function
    Private Shared Function PembelianAdaDiReturBeli(ByVal IDBeli As Long) As Boolean
        Dim ds As New DataSet
        Try
            'per item barang
            'ds = ExecuteDataset("NamaTabel", "SELECT MReturBelid.* FROM (MReturBeliD LEFT JOIN (MBeliD LEFT JOIN MBeli ON MBeli.NoID=MBeliD.IDBeli) ON MBeliD.NoID=MReturBeliD.IDBeliD) WHERE MBeli.NoID=" & IDBeli)
            'potong 1 nota saja
            ds = ExecuteDataset("NamaTabel", "SELECT MReturBeli.* FROM MReturBeli where MReturBeli.IDBeli=" & IDBeli)

            If ds.Tables(0).Rows.Count >= 1 Then
                Return True
            Else
                Return False
            End If
        Catch ex As Exception
            Return False
        Finally
            ds.Dispose()
        End Try
    End Function
    Private Shared Function PembelianAdaDiRevisiHargabeli(ByVal IDBeli As Long) As Boolean
        Dim ds As New DataSet
        Try
            ds = ExecuteDataset("NamaTabel", "SELECT MRevisiHargaBeli.* FROM (MRevisiHargaBeli LEFT JOIN (MBeliD LEFT JOIN MBeli ON MBeli.NoID=MBeliD.IDBeli) ON MBeliD.NoID=MRevisiHargaBeli.IDBeliD) WHERE MBeli.NoID=" & IDBeli)
            If ds.Tables(0).Rows.Count >= 1 Then
                Return True
            Else
                Return False
            End If
        Catch ex As Exception
            Return False
        End Try

    End Function
    Private Shared Function TransaksiAdaDiTT(ByVal IDBeli As Long, ByVal IDJenisTransaksi As Long) As Boolean
        Dim ds As New DataSet
        Try
            ds = ExecuteDataset("NamaTabel", "SELECT MTTD.* FROM MTTD INNER JOIN MTT ON MTT.NoID=MTTD.IDTT WHERE MTTD.IDJenisTransaksi=" & IDJenisTransaksi & " AND MTTD.IDTransaksi=" & IDBeli)
            If ds.Tables(0).Rows.Count >= 1 Then
                Return True
            Else
                Return False
            End If
        Catch ex As Exception
            Return False
        Finally
            ds.Dispose()
        End Try
    End Function
    Private Shared Function TTAdaDiPembayaranHutang(ByVal IDTT As Long) As Boolean
        Dim ds As New DataSet
        Try
            ds = ExecuteDataset("NamaTabel", "SELECT MBayarHutang.* FROM MBayarHutang WHERE MBayarHutang.IDTT=" & IDTT)
            If ds.Tables(0).Rows.Count >= 1 Then
                Return True
            Else
                Return False
            End If
        Catch ex As Exception
            Return False
        Finally
            ds.Dispose()
        End Try
    End Function
    Public Shared Function ReturAdaDiPembayaranHutang(ByVal IDRetur As Long) As Boolean
        Dim ds As New DataSet
        Try
            ds = ExecuteDataset("NamaTabel", "SELECT MBayarHutangDRetur.* FROM MBayarHutangDRetur INNER JOIN MBayarHutang ON MBayarHutang.NoID=MBayarHutangDRetur.IDBayarHutang WHERE MBayarHutangDRetur.IDReturBeli=" & IDRetur)
            If ds.Tables(0).Rows.Count >= 1 Then
                Return True
            Else
                Return False
            End If
        Catch ex As Exception
            Return False
        Finally
            ds.Dispose()
        End Try
    End Function
    Public Shared Function PembelianAdaDiPembayaranHutang(ByVal IDBeli As Long) As Boolean
        Dim ds As New DataSet
        Try
            ds = ExecuteDataset("NamaTabel", "SELECT MBayarHutangD.* FROM MBayarHutangD INNER JOIN MBayarHutang ON MBayarHutang.NoID=MBayarHutangD.IDBayarHutang WHERE MBayarHutangD.IDBeli=" & IDBeli)
            If ds.Tables(0).Rows.Count >= 1 Then
                Return True
            Else
                Return False
            End If
        Catch ex As Exception
            Return False
        Finally
            ds.Dispose()
        End Try
    End Function
    Private Shared Function POAdaDiPembelian(ByVal IDPO As Long) As Boolean
        Dim ds As New DataSet
        Try
            ds = ExecuteDataset("NamaTabel", "SELECT MBeliD.* FROM (MBeliD LEFT JOIN (MPOD LEFT JOIN MPO ON MPO.NoID=MPOD.IDPO) ON MPOD.NoID=MBeliD.IDPOD) WHERE MPO.NoID=" & IDPO)
            If ds.Tables(0).Rows.Count >= 1 Then
                Return True
            Else
                Return False
            End If
        Catch ex As Exception
            Return False
        Finally
            ds.Dispose()
        End Try
    End Function
    Private Shared Function PeneriaanAdaDiPembelian(ByVal IDLPB As Long) As Boolean
        Dim ds As New DataSet
        Try
            ds = ExecuteDataset("NamaTabel", "SELECT MBeliD.* FROM (MBeliD LEFT JOIN (MLPBD LEFT JOIN MLPB ON MLPB.NoID=MLPBD.IDLPB) ON MLPBD.NoID=MBeliD.IDLPBD) WHERE MLPB.NoID=" & IDLPB)
            If ds.Tables(0).Rows.Count >= 1 Then
                Return True
            Else
                Return False
            End If
        Catch ex As Exception
            Return False
        Finally
            ds.Dispose()
        End Try
    End Function
    Public Shared Sub PostingTandaTerimaSupplier(ByVal NoID As Long)
        Dim ds As New DataSet
        Dim SQL As String = ""
        Try
            ds = modSqlServer.ExecuteDataset("MTTD", "SELECT MTTD.*, MTT.IsPosted FROM MTTD INNER JOIN MTT ON MTT.NoID=MTTD.IDTT WHERE MTTD.IDTT=" & NoID)
            If ds.Tables("MTTD").Rows.Count >= 1 Then
                If Not NullToBool(ds.Tables("MTTD").Rows(0).Item("IsPosted")) Then
                    For i As Integer = 0 To ds.Tables("MTTD").Rows.Count - 1
                        SQL = "UPDATE MBeli SET IsTT=1,NoFaktur='" & FixApostropi(NullToStr(ds.Tables("MTTD").Rows(i).Item("NoFaktur"))) & "',IsAdaFP=" & IIf(NullToBool(ds.Tables("MTTD").Rows(i).Item("IsAdaFP")), "1", "0") & " WHERE NoID=" & NullToLong(ds.Tables("MTTD").Rows(i).Item("IDTransaksi"))
                        EksekusiSQL(SQL)
                        SQL = "UPDATE MPO SET IsTT=1 WHERE NoID=" & NullToLong(ds.Tables("MTTD").Rows(i).Item("IDPO"))
                        EksekusiSQL(SQL)
                        SQL = "UPDATE MReturBeli SET IsTT=1 WHERE NoID=" & NullToLong(ds.Tables("MTTD").Rows(i).Item("IDRetur"))
                        EksekusiSQL(SQL)
                    Next
                    EksekusiSQL("UPDATE MTT SET IsPosted=1, IDUserPosted=" & IDUserAktif & ", TglPosted=getdate() WHERE NoID=" & NoID)
                End If
            End If
        Catch ex As Exception
            DevExpress.XtraEditors.XtraMessageBox.Show("Kesalahan perhitugan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
        Finally
            ds.Dispose()
        End Try
    End Sub
    Public Shared Sub UnPostingTandaTerimaSupplier(ByVal NoID As Long)
        Dim ds As New DataSet
        Try
            If Not NullToBool(EksekusiSQlSkalarNew("SELECT IsRevisi FROM MTT WHERE NoID=" & NoID)) AndAlso Not TTAdaDiPembayaranHutang(NoID) Then
                EksekusiSQL("UPDATE MTT SET IsPosted=0, IDUserPosted=NULL, TglPosted=NULL WHERE NoID=" & NoID)
                EksekusiSQL("UPDATE MBeli SET IsTT=0 WHERE NoID IN (SELECT IDTransaksi FROM MTTD WHERE IDTT=" & NoID & ")")
                EksekusiSQL("UPDATE MPO SET IsTT=0 WHERE NoID IN (SELECT IDPO FROM MTTD WHERE IDTT=" & NoID & ")")
                EksekusiSQL("UPDATE MReturBeli SET IsTT=0 WHERE NoID IN (SELECT IDRetur FROM MTTD WHERE IDTT=" & NoID & ")")
            End If
        Catch ex As Exception
            DevExpress.XtraEditors.XtraMessageBox.Show("Kesalahan perhitugan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
        Finally
            ds.Dispose()
        End Try
    End Sub
    Public Shared Sub PostingSaldoAwalHutangPiutang(ByVal NoID As Long)
        Dim ds As New DataSet
        Dim SQL As String = ""
        Try
            ds = modSqlServer.ExecuteDataset("MSaldoAwalHutangPiutang", "SELECT MSaldoAwalHutangPiutang.* FROM MSaldoAwalHutangPiutang WHERE MSaldoAwalHutangPiutang.NoID=" & NoID)
            If ds.Tables("MSaldoAwalHutangPiutang").Rows.Count >= 1 Then
                If Not NullToBool(ds.Tables("MSaldoAwalHutangPiutang").Rows(0).Item("IsPosted")) Then
                    EksekusiSQL("UPDATE MSaldoAwalHutangPiutang SET IsPosted=1, IDUserPosting=" & IDUserAktif & " WHERE NoID=" & NoID)
                End If
            End If
        Catch ex As Exception
            DevExpress.XtraEditors.XtraMessageBox.Show("Kesalahan perhitugan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
        Finally
            ds.Dispose()
        End Try
    End Sub
    Public Shared Sub UnPostingSaldoAwalHutangPiutang(ByVal NoID As Long)
        Dim ds As New DataSet
        Dim SQL As String = ""
        Try
            ds = modSqlServer.ExecuteDataset("MSaldoAwalHutangPiutang", "SELECT MSaldoAwalHutangPiutang.* FROM MSaldoAwalHutangPiutang WHERE MSaldoAwalHutangPiutang.NoID=" & NoID)
            If ds.Tables("MSaldoAwalHutangPiutang").Rows.Count >= 1 Then
                EksekusiSQL("UPDATE MSaldoAwalHutangPiutang SET IsPosted=0, IDUserPosting=NULL WHERE NoID=" & NoID)
            End If
        Catch ex As Exception
            DevExpress.XtraEditors.XtraMessageBox.Show("Kesalahan perhitugan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
        Finally
            ds.Dispose()
        End Try
    End Sub
    Public Shared Sub PostingPO(ByVal IDPO As Long)
        Dim ds As New DataSet
        Dim SQL As String = ""
        Try
            ds = modSqlServer.ExecuteDataset("MPOD", "SELECT MPOD.*, MPO.IsPosted, MPO.Tanggal AS TGLPO FROM MPOD LEFT JOIN MPO ON MPO.NoID=MPOD.IDPO WHERE MPOD.IDPO=" & IDPO)
            If ds.Tables("MPOD").Rows.Count >= 1 Then
                If Not NullTobool(ds.Tables("MPOD").Rows(0).Item("IsPosted")) Then
                    EksekusiSQL("UPDATE MPO SET IsPosted=1, IDUSerPosting=" & IDUserAktif & ", TglPosting=getdate() WHERE NoID=" & IDPO)
                End If
            End If
        Catch ex As Exception
            DevExpress.XtraEditors.XtraMessageBox.Show("Kesalahan perhitugan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
        Finally
            ds.Dispose()
        End Try
    End Sub
    Public Shared Sub UnPostingPO(ByVal IDPO As Long)
        Dim ds As New DataSet
        Try
            If Not POAdaDiPembelian(IDPO) Then
                EksekusiSQL("UPDATE MPO SET IsPosted=0, IDUserPosting=NULL, TglPosting=NULL WHERE NoID=" & IDPO)
            End If
        Catch ex As Exception
            DevExpress.XtraEditors.XtraMessageBox.Show("Kesalahan perhitugan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
        Finally
            ds.Dispose()
        End Try
    End Sub
    Public Shared Sub UnPostingTransferPoin(ByVal NoID As Long)
        Dim ds As New DataSet
        Try
            EksekusiSQL("DELETE FROM MCustomerPoin WHERE IDJenisTransaksi=61 AND IDTransaksi=" & NoID)

            EksekusiSQL("UPDATE MTransferPoin SET IsPosted=0 WHERE NoID=" & NoID)
        Catch ex As Exception
            DevExpress.XtraEditors.XtraMessageBox.Show("Kesalahan perhitugan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
        Finally
            ds.Dispose()
        End Try
    End Sub
    Public Shared Sub PostingLPB(ByVal IDLPB As Long)
        Dim ds As New DataSet
        Dim SQL As String = ""
        Try
            'ds = modSqlServer.ExecuteDataset("MPOD", "SELECT MPOD.*, MPO.IsPosted, MPO.Tanggal AS TGLPO FROM MPOD LEFT JOIN MPO ON MPO.NoID=MPOD.IDPO WHERE MPOD.IDPO=" & IDPO)
            'If ds.Tables("MPOD").Rows.Count >= 1 Then
            '    If Not NullTobool(ds.Tables("MPOD").Rows(0).Item("IsPosted")) Then
            EksekusiSQL("UPDATE MLPB SET IsPosted=1, IDUserPosting=" & IDUserAktif & ", TglPosting=Getdate() WHERE NoID=" & IDLPB)
            '    End If
            'End If
        Catch ex As Exception
            DevExpress.XtraEditors.XtraMessageBox.Show("Kesalahan perhitugan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
        Finally
            ds.Dispose()
        End Try
    End Sub
    Public Shared Sub UnPostingLPB(ByVal IDLPB As Long)
        Dim ds As New DataSet
        Try
            If Not PeneriaanAdaDiPembelian(IDLPB) Then
                EksekusiSQL("UPDATE MLPB SET IsPosted=0, IDuserPosting=NULL, TglPosting=NULL WHERE NoID=" & IDLPB)
            End If
        Catch ex As Exception
            DevExpress.XtraEditors.XtraMessageBox.Show("Kesalahan perhitugan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
        Finally
            ds.Dispose()
        End Try
    End Sub
    Public Shared Sub PostingNotaDebet(ByVal IDBeli As Long)
        Dim ds As New DataSet
        Dim SQL As String = ""
        Try
            ds = modSqlServer.ExecuteDataset("MDebetNote", "SELECT MDebetNote.* FROM MDebetNote WHERE MDebetNote.NoID=" & IDBeli)
            If ds.Tables("MDebetNote").Rows.Count >= 1 Then
                If Not NullToBool(ds.Tables("MDebetNote").Rows(0).Item("IsPosted")) Then
                    InsertMHutang(IDBeli, 19)
                    EksekusiSQL("UPDATE MDebetNote SET IsPosted=1 WHERE NoID=" & IDBeli)
                End If
            End If
        Catch ex As Exception
            DevExpress.XtraEditors.XtraMessageBox.Show("Kesalahan perhitugan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
        Finally
            ds.Dispose()
        End Try
    End Sub
    Public Shared Sub PostingNotaKredit(ByVal IDBeli As Long)
        Dim ds As New DataSet
        Dim SQL As String = ""
        Try
            ds = modSqlServer.ExecuteDataset("MCreditNote", "SELECT MCreditNote.* FROM MCreditNote WHERE MCreditNote.NoID=" & IDBeli)
            If ds.Tables("MCreditNote").Rows.Count >= 1 Then
                If Not NullToBool(ds.Tables("MCreditNote").Rows(0).Item("IsPosted")) Then
                    InsertMHutang(IDBeli, 20)
                    EksekusiSQL("UPDATE MCreditNote SET IsPosted=1 WHERE NoID=" & IDBeli)
                End If
            End If
        Catch ex As Exception
            DevExpress.XtraEditors.XtraMessageBox.Show("Kesalahan perhitugan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
        Finally
            ds.Dispose()
        End Try
    End Sub
    Public Shared Sub PostingTitipan(ByVal IDBeli As Long)
        Dim ds As New DataSet
        Dim SQL As String = ""
        Try
            ds = modSqlServer.ExecuteDataset("MTitipan", "SELECT MTitipan.* FROM MTitipan WHERE MTitipan.NoID=" & IDBeli)
            If ds.Tables("MTitipan").Rows.Count >= 1 Then
                If Not NullToBool(ds.Tables("MTitipan").Rows(0).Item("IsPosted")) Then
                    InsertMHutang(IDBeli, 21)
                    EksekusiSQL("UPDATE MTitipan SET IsPosted=1 WHERE NoID=" & IDBeli)
                End If
            End If
        Catch ex As Exception
            DevExpress.XtraEditors.XtraMessageBox.Show("Kesalahan perhitugan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
        Finally
            ds.Dispose()
        End Try
    End Sub
    Public Shared Sub UnPostingNotaKredit(ByVal IDBeli As Long)
        Dim ds As New DataSet
        Dim SQL As String = ""
        Try
            DeleteMHutang(IDBeli, 20, DefIDWilayah)
            EksekusiSQL("UPDATE MCreditNote SET IsPosted=0 WHERE NoID=" & IDBeli)
        Catch ex As Exception
            DevExpress.XtraEditors.XtraMessageBox.Show("Kesalahan perhitugan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
        Finally
            ds.Dispose()
        End Try
    End Sub
    Public Shared Sub UnPostingNotaDebet(ByVal IDBeli As Long)
        Dim ds As New DataSet
        Dim SQL As String = ""
        Try
            DeleteMHutang(IDBeli, 19, DefIDWilayah)
            EksekusiSQL("UPDATE MDebetNote SET IsPosted=0 WHERE NoID=" & IDBeli)
        Catch ex As Exception
            DevExpress.XtraEditors.XtraMessageBox.Show("Kesalahan perhitugan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
        Finally
            ds.Dispose()
        End Try
    End Sub
    Public Shared Sub PostingStokBarangPembelian(ByVal IDBeli As Long)
        Dim ds As New DataSet
        Dim con As New SqlClient.SqlConnection
        Dim comm As New SqlClient.SqlCommand
        Dim oDA As New SqlClient.SqlDataAdapter
        Dim Trans As SqlTransaction = Nothing
        Dim SQL As String = ""
        Dim SaldoStockAkhir As Double = 0.0
        Dim SaldoAkhir As Double = 0.0
        Dim HPPSekarang As Double = 0.0
        Dim HPP As Double = 0.0
        Dim IDKartuStok As Long = -1
        Try
            con.ConnectionString = StrKonSql
            con.Open()
            Trans = con.BeginTransaction("TransPostingMBeli")
            comm.Connection = con
            comm.Transaction = Trans
            'Trans Terbuat
            comm.CommandText = "SELECT MBeliD.*, MBeli.IsPosted, MBeli.Tanggal AS TGLBeli, MBeli.DiskonNotaProsen, (MBeliD.Jumlah/MBeliD.Qty)-((MBeliD.Jumlah/MBeliD.Qty)*MBeli.DiskonNotaProsen/100) AS HargaBeli FROM MBeliD LEFT JOIN MBeli ON MBeli.NoID=MBeliD.IDBeli WHERE MBeliD.IDBeli=" & IDBeli
            oDA.SelectCommand = comm
            If Not ds.Tables("MBeliD") Is Nothing Then
                ds.Tables("MBeliD").Clear()
            End If
            oDA.Fill(ds, "MBeliD")
            If ds.Tables("MBeliD").Rows.Count >= 1 Then
                If Not NullToBool(ds.Tables("MBeliD").Rows(0).Item("IsPosted")) Then
                    comm.CommandText = "DELETE FROM MKartuStok WHERE IDTransaksi=" & IDBeli & " AND IDJenisTransaksi=2"
                    comm.ExecuteNonQuery()
                    For i As Integer = 0 To ds.Tables("MBeliD").Rows.Count - 1
                        SaldoStockAkhir = 0 'GetStockAkhirGudang(NullTolong(ds.Tables("MBeliD").Rows(i).Item("IDBarang")), NullTolong(ds.Tables("MBeliD").Rows(i).Item("IDGudang")), NullToDate(ds.Tables("MBeliD").Rows(i).Item("TGLBeli")))
                        comm.CommandText = "SELECT MAX(NoID) AS NewNoID FROM MKartuStok"
                        IDKartuStok = NullToLong(comm.ExecuteScalar) + 1
                        SQL = " INSERT INTO [dbo].[MKartuStok]" & vbCrLf & _
                              " ([NoID]" & _
                              " ,[Kode]" & _
                              " ,[IDBarang]" & _
                              " ,[IDBarangD]" & _
                              " ,[IDDepartemen]" & _
                              " ,[IDGudang]" & _
                              " ,[Tanggal]" & _
                              " ,[IDJenisTransaksi]" & _
                              " ,[IDTransaksi]" & _
                              " ,[IDTransaksiDetil]" & _
                              " ,[Keterangan]" & _
                              " ,[QtyMasukA]" & _
                              " ,[QtyMasuk]" & _
                              " ,[HgBeliPeritem]" & _
                              " ,[JumlahBeli]" & _
                              " ,[QtyKeluarA]" & _
                              " ,[QtyKeluar]" & _
                              " ,[HargaJualPerItem]" & _
                              " ,[JumlahJual]" & _
                              " ,[HPPKeluar]" & _
                              " ,[JumHPPKeluar]" & _
                              " ,[QtyAkhir]" & _
                              " ,[HgBeliRtRtPeritem]" & _
                              " ,[TotHPPRataRata]" & _
                              " ,[IDJenis]" & _
                              " ,[IDSatuan]" & _
                              " ,[Konversi]" & _
                              " ,[IDAlamat]" & _
                              " ,[Ctn_Duz])" & vbCrLf & _
                              " SELECT " & IDKartuStok & " AS NoID, MBeli.Kode, MBeliD.IDBarang, MBeliD.IDBarangD, " & _
                              " MWilayah.IDDepartemen, MBeliD.IDGudang, " & _
                              " MBeli.Tanggal, 2 AS IDJenisTransaksi,  MBeli.NoID AS IDTransaksi, " & _
                              " MBeliD.NoID AS IDTransaksiDetil, MBeliD.Catatan, ISNULL(MBeliD.Qty,0)*IsNull(MBeliD.Konversi,0) AS Masuk, MBeliD.Qty AS MasukA, " & _
                              " MBeliD.Harga AS HrgBeliPerItem, ISNULL(MBeliD.Jumlah,0) AS JumlahBeli, 0 AS Keluar, 0 AS KeluarA, " & _
                              " 0,0,0 AS HPPKeluar, " & _
                              " 0 AS JumHPPKeluar, " & Replace(SaldoStockAkhir, ",", ".") & "+(ISNULL(MBeliD.Qty,0)*IsNull(MBeliD.Konversi,0)) as QtyAkhir, 0," & _
                              " 0, MBarang.IDJenis, MBeliD.IDSatuan, " & _
                              " IsNull(MBeliD.Konversi,0), MBeli.IDSupplier, MBeliD.Ctn " & _
                              " FROM MBeliD LEFT JOIN MBeli ON MBeli.NoID=MBeliD.IDBeli" & _
                              " LEFT JOIN (MGudang LEFT JOIN MWilayah ON MWilayah.NoID=MGudang.IDWilayah) ON MBeliD.IDGudang=MGudang.NoID" & _
                              " LEFT JOIN MBarang ON MBarang.NoID=MBeliD.IDBarang" & _
                              " WHERE MBeliD.NoID=" & NullToLong(ds.Tables("MBeliD").Rows(i).Item("NoID"))
                        comm.CommandText = SQL
                        If NullToLong(comm.ExecuteNonQuery()) >= 1 Then
                            If IsUpdateHargaBeli Then
                                ''Update Harga Beli Master Stock
                                'SQL = "UPDATE MBarang SET TerakhirBeli=MBeli.Tanggal, HargaBeli=MBeliD.HargaPcs*MBarang.CtnPcs, HargaBeliPcs=MBeliD.HargaPcs,IDSupplier5=MBeli.IDSupplier " & _
                                '      " FROM (MBeliD " & _
                                '      " LEFT JOIN MBeli ON MBeli.NoID=MBeliD.IDBeli) " & _
                                '      " LEFT JOIN MBarang ON MBarang.NoID=MBeliD.IDBarang " & _
                                '      " WHERE MBarang.NoID=" & NullToLong(ds.Tables("MBeliD").Rows(i).Item("IDBarang")) & " AND MBeliD.NoID=" & NullToLong(ds.Tables("MBeliD").Rows(i).Item("NoID"))
                                'comm.CommandText = SQL
                                'comm.ExecuteNonQuery()
                            End If
                            ''Update Harga Jual Di MBarangD
                            'SQL = "UPDATE MBarangD SET HargaJual=MBeliD.HargaJual, HargaBeliNetto=MBeliD.HargaNetto " & _
                            '      " FROM (MBeliD " & _
                            '      " LEFT JOIN MBeli ON MBeli.NoID=MBeliD.IDBeli) " & _
                            '      " LEFT JOIN MBarangD ON MBarangD.NoID=MBeliD.IDBarangD " & _
                            '      " WHERE  MBeliD.NoID=" & NullToLong(ds.Tables("MBeliD").Rows(i).Item("NoID"))
                            'comm.CommandText = SQL
                            'comm.ExecuteNonQuery()

                            'Update HPP (pembelian) : StockAkhir*HPP+QtyBeli*HargaBeli/StockAkhir+QtyBeli
                            comm.CommandText = "SELECT SUM((MKartuStok.QtyMasuk-MKartuStok.QtyKeluar)*MKartuStok.Konversi) FROM MKartuStok WHERE MKartuStok.IDBarang=" & NullToLong(ds.Tables(0).Rows(i).Item("IDBarang"))
                            SaldoAkhir = NullToDbl(comm.ExecuteScalar)
                            comm.CommandText = "SELECT HargaBeliPcs FROM MBarang WHERE NoID=" & NullToLong(ds.Tables(0).Rows(i).Item("IDBarang"))
                            HPPSekarang = NullToDbl(comm.ExecuteScalar)
                            HPP = (SaldoAkhir * HPPSekarang) + (NullToDbl(ds.Tables(0).Rows(i).Item("Qty")) * NullToDbl(ds.Tables(0).Rows(i).Item("Konversi")) * NullToDbl(ds.Tables(0).Rows(i).Item("HargaBeli")))
                            If ((SaldoAkhir + (NullToDbl(ds.Tables(0).Rows(i).Item("Qty")) * NullToDbl(ds.Tables(0).Rows(i).Item("Konversi"))))) = 0 Then
                                HPP = NullToDbl(ds.Tables(0).Rows(i).Item("HargaBeli"))
                            Else
                                HPP = HPP / (SaldoAkhir + (NullToDbl(ds.Tables(0).Rows(i).Item("Qty")) * NullToDbl(ds.Tables(0).Rows(i).Item("Konversi"))))
                            End If
                            SQL = "UPDATE MBarang SET HPP=" & FixKoma(HPP) & " WHERE MBarang.NoID=" & NullToLong(ds.Tables("MBeliD").Rows(i).Item("IDBarang"))
                            comm.CommandText = SQL
                            comm.ExecuteNonQuery()
                            SQL = "UPDATE MKartuStok SET HPP=" & FixKoma(HPP) & ", TerakhirBeli=Tanggal, HargaBeli=" & FixKoma(NullToDbl(ds.Tables(0).Rows(i).Item("HargaBeli"))) & " WHERE NoID=" & IDKartuStok
                            comm.CommandText = SQL
                            comm.ExecuteNonQuery()
                        End If
                    Next
                   
                End If
            End If
            'InsertMHutang(IDBeli, 2)
            SQL = "UPDATE MBeli SET IsPosted=1, IDuserPosting=" & IDUserAktif & ", TglPosting=getdate() WHERE NoID=" & IDBeli
            comm.CommandText = SQL
            comm.ExecuteNonQuery()
            Trans.Commit()
        Catch ex As Exception
            DevExpress.XtraEditors.XtraMessageBox.Show("Info Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
            Trans.Rollback("TransPostingMBeli")
        Finally
            If Not oDA Is Nothing Then
                oDA.Dispose()
            End If
            If Not comm Is Nothing Then
                comm.Dispose()
            End If
            If Not con Is Nothing Then
                con.Close()
                con.Dispose()
            End If
            If Not ds Is Nothing Then
                ds.Dispose()
            End If
        End Try
    End Sub
    Public Shared Sub PostingTransferPoin(ByVal NoID As Long)
        Dim ds As New DataSet
        Dim con As New SqlClient.SqlConnection
        Dim comm As New SqlClient.SqlCommand
        Dim oDA As New SqlClient.SqlDataAdapter
        Dim Trans As SqlTransaction = Nothing
        Dim SQL As String = ""
        Dim SaldoPoinCustomerBaru As Double = 0.0
        Dim IDTukarPoin As Long = -1
        Try
            con.ConnectionString = StrKonSql
            con.Open()
            Trans = con.BeginTransaction("TransPosting")
            comm.Connection = con
            comm.CommandTimeout = con.ConnectionTimeout
            comm.Transaction = Trans
            'Trans Terbuat
            comm.CommandText = "SELECT MTransferPoin.* FROM MTransferPoin WHERE MTransferPoin.NoID=" & NoID
            oDA.SelectCommand = comm
            If Not ds.Tables("MTransferPoin") Is Nothing Then
                ds.Tables("MTransferPoin").Clear()
            End If
            oDA.Fill(ds, "MTransferPoin")
            For i As Integer = 0 To ds.Tables("MTransferPoin").Rows.Count - 1
                With ds.Tables("MTransferPoin").Rows(i)
                    'comm.CommandText = "DELETE FROM MTukarPoin WHERE IDKasir=" & NoID & " AND IDKassa=-1"
                    'comm.ExecuteNonQuery()

                    ''Poin dikeluarkan dari member lama
                    'comm.CommandText = "SELECT MAX(NoID) AS NewNoID FROM MTukarPoin"
                    'IDTukarPoin = NullToLong(comm.ExecuteScalar) + 1
                    'SQL = "INSERT INTO [dbo].[MTukarPoin] ([NoID],[Tanggal],[Jam],[IDMember],[NoMember],[IDKassa],[IDKasir],[JumlahPoin],[Kredit],[Saldo],[Keterangan]) " & vbCrLf & _
                    '      " SELECT " & IDTukarPoin & ", MTransferPoin.Tanggal, MTransferPoin.Jam, MTransferPoin.IDCustomerLama, LEFT(MAlamat.Nama, 20) AS NoMember, -1, " & NoID & ", MTransferPoin.JumlahPoin, MTransferPoin.PoinTransfer, MTransferPoin.JumlahPoin-MTransferPoin.PoinTransfer, MTransferPoin.Keterangan " & vbCrLf & _
                    '      " FROM MTransferPoin INNER JOIN MAlamat ON MAlamat.NoID=MTransferPoin.IDCustomerLama WHERE MTransferPoin.NoID=" & NoID
                    'comm.CommandText = SQL
                    'comm.ExecuteNonQuery()

                    ''Poin dimasukkan ke member baru di kredit kali minus
                    'comm.CommandText = "SELECT MAX(NoID) AS NewNoID FROM MTukarPoin"
                    'IDTukarPoin = NullToLong(comm.ExecuteScalar) + 1
                    ''SQL = "SELECT " & _
                    ''      " IsNull((SELECT SUM(MJual.NilaiPoin) FROM MJual WHERE MJual.IDCustomer=" & NullToLong(.Item("IDCustomerBaru")) & "),0)-" & _
                    ''      " IsNull((SELECT SUM(MTukarPoin.Kredit) FROM MTukarPoin WHERE MTukarPoin.IDMember=" & NullToLong(.Item("IDCustomerBaru")) & "),0)" & _
                    ''      " AS POIN"
                    'SQL = "SELECT vSaldoPoin.SaldoPoin FROM vSaldoPoin WHERE vSaldoPoin.IDCustomer=" & NullToLong(.Item("IDCustomerBaru"))
                    'comm.CommandText = SQL
                    'SaldoPoinCustomerBaru = NullToDbl(comm.ExecuteScalar())
                    'SQL = "INSERT INTO [dbo].[MTukarPoin] ([NoID],[Tanggal],[Jam],[IDMember],[NoMember],[IDKassa],[IDKasir],[JumlahPoin],[Kredit],[Saldo],[Keterangan]) " & vbCrLf & _
                    '      " SELECT " & IDTukarPoin & ", MTransferPoin.Tanggal, MTransferPoin.Jam, MTransferPoin.IDCustomerBaru, LEFT(MAlamat.Nama, 20) AS NoMember, -1, " & NoID & ", " & FixKoma(SaldoPoinCustomerBaru) & ", MTransferPoin.PoinTransfer*-1, " & FixKoma(SaldoPoinCustomerBaru) & "-(MTransferPoin.PoinTransfer*-1), MTransferPoin.Keterangan " & vbCrLf & _
                    '      " FROM MTransferPoin INNER JOIN MAlamat ON MAlamat.NoID=MTransferPoin.IDCustomerBaru WHERE MTransferPoin.NoID=" & NoID
                    'comm.CommandText = SQL
                    'comm.ExecuteNonQuery()

                    'Keluarkan dulu
                    SQL = "INSERT INTO [MCustomerPoin]" & vbCrLf & _
                          " ([IDTransaksi]" & vbCrLf & _
                          " ,[IDJenisTransaksi]" & vbCrLf & _
                          " ,[IDCustomer]" & vbCrLf & _
                          " ,[Tanggal]" & vbCrLf & _
                          " ,[NoReff]" & vbCrLf & _
                          " ,[Debet]" & vbCrLf & _
                          " ,[Kredit])" & vbCrLf & _
                          " SELECT i.NoID, 61 AS IDJenisTransaksi, i.IDCustomerLama, i.Tanggal, i.Kode AS Kode, 0 AS Debet, ISNULL(i.Kredit,0) AS Kredit" & vbCrLf & _
                          " FROM MTransferPoin i WHERE i.NoID=" & NoID
                    comm.CommandText = SQL
                    comm.ExecuteNonQuery()

                    'Masukkan ke member baru
                    SQL = "INSERT INTO [MCustomerPoin]" & vbCrLf & _
                          " ([IDTransaksi]" & vbCrLf & _
                          " ,[IDJenisTransaksi]" & vbCrLf & _
                          " ,[IDCustomer]" & vbCrLf & _
                          " ,[Tanggal]" & vbCrLf & _
                          " ,[NoReff]" & vbCrLf & _
                          " ,[Debet]" & vbCrLf & _
                          " ,[Kredit])" & vbCrLf & _
                          " SELECT i.NoID, 61 AS IDJenisTransaksi, i.IDCustomerBaru, i.Tanggal, i.Kode AS Kode, ISNULL(i.Kredit,0) AS Debet, 0 AS Kredit" & vbCrLf & _
                          " FROM MTransferPoin i WHERE i.NoID=" & NoID
                    comm.CommandText = SQL
                    comm.ExecuteNonQuery()
                End With
            Next
            'Update MTransferPoin
            SQL = "UPDATE MTransferPoin SET IsPosted=1 WHERE NoID=" & NoID
            comm.CommandText = SQL
            comm.ExecuteNonQuery()
            Trans.Commit()
        Catch ex As Exception
            DevExpress.XtraEditors.XtraMessageBox.Show("Info Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
            Trans.Rollback("TransPosting")
        Finally
            If Not oDA Is Nothing Then
                oDA.Dispose()
            End If
            If Not comm Is Nothing Then
                comm.Dispose()
            End If
            If Not con Is Nothing Then
                con.Close()
                con.Dispose()
            End If
            If Not ds Is Nothing Then
                ds.Dispose()
            End If
        End Try
    End Sub

    Public Shared Sub InsertMHutang(ByVal IDTransaksi As Long, ByVal IDJenisTransaksi As Long)
        Dim ds As New DataSet
        Dim SQL As String = ""
        Try
            If IDJenisTransaksi = 2 Then
                SQL = "SELECT MBeli.Kode, MBeli.Tanggal, MBeli.JatuhTempo, " & _
                      " MBeli.IDSupplier AS IDAlamat, MBeli.IDWilayah, MBeli.Keterangan, " & _
                      " 0 AS Debet, MBeli.Subtotal AS Kredit, MBeli.DiskonNotaTotal AS Diskon, " & _
                      " MBeli.Biaya, MBeli.Bayar AS Dibayar, MBeli.Sisa, MBeli.IsPosted, 0 AS IsSaldoAwal " & _
                      " FROM MBeli " & _
                      " WHERE NoID=" & IDTransaksi
            ElseIf IDJenisTransaksi = 3 Then
                SQL = "SELECT MReturBeli.Kode, MReturBeli.Tanggal, MReturBeli.JatuhTempo, " & _
                      " MReturBeli.IDSupplier AS IDAlamat, MReturBeli.IDWilayah, MReturBeli.Keterangan, " & _
                      " 0 AS Kredit, MReturBeli.Subtotal AS Debet, MReturBeli.DiskonNotaTotal AS Diskon, " & _
                      " MReturBeli.Biaya, MReturBeli.Bayar AS Dibayar, MReturBeli.Sisa, MReturBeli.IsPosted, 0 AS IsSaldoAwal " & _
                      " FROM MReturBeli " & _
                      " WHERE NoID=" & IDTransaksi
            ElseIf IDJenisTransaksi = 11 Then
                SQL = "SELECT MRevisiHargaBeli.Kode, MRevisiHargaBeli.Tanggal, NULL AS JatuhTempo, " & _
                      " MRevisiHargaBeli.IDSupplier AS IDAlamat, MRevisiHargaBeli.IDWilayah, MRevisiHargaBeli.Keterangan, " & _
                      " ABS(CASE WHEN SUM(MRevisiHargaBeliD.Jumlah-MRevisiHargaBeliD.JumlahBaru)>=0 THEN 0 ELSE SUM(MRevisiHargaBeliD.Jumlah-MRevisiHargaBeliD.JumlahBaru) END) AS Kredit, " & _
                      " ABS(CASE WHEN SUM(MRevisiHargaBeliD.Jumlah-MRevisiHargaBeliD.JumlahBaru)>=0 THEN SUM(MRevisiHargaBeliD.Jumlah-MRevisiHargaBeliD.JumlahBaru) ELSE 0 END) AS Debet, " & _
                      " 0 AS Diskon,  0 AS Biaya, 0 AS Dibayar, " & _
                      " SUM(MRevisiHargaBeliD.Jumlah-MRevisiHargaBeliD.JumlahBaru)AS Sisa, MRevisiHargaBeli.IsPosted, 0 AS IsSaldoAwal " & _
                      " FROM MRevisiHargaBeliD INNER JOIN MRevisiHargaBeli ON MRevisiHargaBeli.NoID=MRevisiHargaBeliD.IDRevisiHargaBeli" & _
                      " WHERE MRevisiHargaBeli.NoID=" & IDTransaksi & _
                      " GROUP BY MRevisiHargaBeli.Kode, MRevisiHargaBeli.Tanggal, MRevisiHargaBeli.IDSupplier, MRevisiHargaBeli.IDWilayah, MRevisiHargaBeli.Keterangan, MRevisiHargaBeli.IsPosted"
            ElseIf IDJenisTransaksi = 19 Then
                SQL = "SELECT MDebetNote.Kode, MDebetNote.Tanggal, MDebetNote.JatuhTempo, " & _
                      " MDebetNote.IDAlamat, MDebetNote.IDWilayah, MDebetNote.Keterangan, " & _
                      " MDebetNote.Jumlah AS Debet, " & _
                      " 0 AS Kredit, " & _
                      " 0 AS Diskon,  0 AS Biaya, 0 AS Dibayar, " & _
                      " MDebetNote.Jumlah AS Sisa, MDebetNote.IsPosted, 0 AS IsSaldoAwal " & _
                      " FROM MDebetNote " & _
                      " WHERE MDebetNote.NoID=" & IDTransaksi
            ElseIf IDJenisTransaksi = 20 Then
                SQL = "SELECT MCreditNote.Kode, MCreditNote.Tanggal, MCreditNote.JatuhTempo, " & _
                      " MCreditNote.IDAlamat, MCreditNote.IDWilayah, MCreditNote.Keterangan, " & _
                      " MCreditNote.Jumlah AS Kredit, " & _
                      " 0 AS Debet, " & _
                      " 0 AS Diskon,  0 AS Biaya, 0 AS Dibayar, " & _
                      " MCreditNote.Jumlah AS Sisa, MCreditNote.IsPosted, 0 AS IsSaldoAwal " & _
                      " FROM MCreditNote " & _
                      " WHERE MCreditNote.NoID=" & IDTransaksi
            ElseIf IDJenisTransaksi = 21 Then
                SQL = "SELECT MCreditNote.Kode, MCreditNote.Tanggal, MCreditNote.JatuhTempo, " & _
                      " MCreditNote.IDAlamat, MCreditNote.IDWilayah, MCreditNote.Keterangan, " & _
                      " MCreditNote.Jumlah AS Kredit, " & _
                      " 0 AS Debet, " & _
                      " 0 AS Diskon,  0 AS Biaya, 0 AS Dibayar, " & _
                      " MCreditNote.Jumlah AS Sisa, MDebetNote.IsPosted, 0 AS IsSaldoAwal " & _
                      " FROM MCreditNote " & _
                      " WHERE MCreditNote.NoID=" & IDTransaksi
            End If
            ds = modSqlServer.ExecuteDataset("MHutang", SQL)
            If ds.Tables("MHutang").Rows.Count >= 1 Then
                If Not NullToBool(ds.Tables("MHutang").Rows(0).Item("IsPosted")) Then
                    EksekusiSQL("DELETE FROM MHutang WHERE IDTransaksi=" & IDTransaksi & " AND IDJenisTransaksi=" & IDJenisTransaksi)
                    For i As Integer = 0 To ds.Tables("MHutang").Rows.Count - 1
                        SQL = "INSERT INTO MHutang (NoID, Kode, Tanggal, JatuhTempo, IDAlamat, IDWilayah, IDTransaksi, IDJenisTransaksi, Keterangan, Debet, DebetA, Kredit, KreditA, Diskon, Biaya, Dibayar, Sisa, IsSaldoAwal) VALUES (" & _
                              NullToLong(GetNewID("MHutang", "NoID")) & ", " & _
                              "'" & FixApostropi(NullToStr(ds.Tables("MHutang").Rows(i).Item("Kode"))) & "', " & _
                              "'" & NullToDate(ds.Tables("MHutang").Rows(i).Item("Tanggal")).ToString("yyyy-MM-dd HH:mm") & "', " & _
                              "'" & NullToDate(ds.Tables("MHutang").Rows(i).Item("JatuhTempo")).ToString("yyyy-MM-dd HH:mm") & "', " & _
                              NullToLong(ds.Tables("MHutang").Rows(i).Item("IDAlamat")) & ", " & _
                              NullToLong(ds.Tables("MHutang").Rows(i).Item("IDWilayah")) & ", " & _
                              IDTransaksi & ", " & _
                              IDJenisTransaksi & ", " & _
                              "'" & FixApostropi(NullToStr(ds.Tables("MHutang").Rows(i).Item("Keterangan"))) & "', " & _
                              FixKoma(NullToDbl(ds.Tables("MHutang").Rows(i).Item("Debet"))) & ", " & _
                              FixKoma(NullToDbl(ds.Tables("MHutang").Rows(i).Item("Debet"))) & ", " & _
                              FixKoma(NullToDbl(ds.Tables("MHutang").Rows(i).Item("Debet"))) & ", " & _
                              FixKoma(NullToDbl(ds.Tables("MHutang").Rows(i).Item("Debet"))) & ", " & _
                              FixKoma(NullToDbl(ds.Tables("MHutang").Rows(i).Item("Diskon"))) & ", " & _
                              FixKoma(NullToDbl(ds.Tables("MHutang").Rows(i).Item("Biaya"))) & ", " & _
                              FixKoma(NullToDbl(ds.Tables("MHutang").Rows(i).Item("Dibayar"))) & ", " & _
                              FixKoma(NullToDbl(ds.Tables("MHutang").Rows(i).Item("Sisa"))) & ", " & _
                              NullToLong(ds.Tables("MHutang").Rows(i).Item("IsSaldoAwal")) & ")"
                        EksekusiSQL(SQL)
                    Next
                End If
            End If
        Catch ex As Exception
            DevExpress.XtraEditors.XtraMessageBox.Show("Kesalahan perhitugan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
        Finally
            ds.Dispose()
        End Try
    End Sub
    Public Shared Sub DeleteMHutang(ByVal IDTransaksi As Long, ByVal IDJenisTransaksi As Long, ByVal IDWilayah As Long)
        Dim SQL As String = ""
        Try
            SQL = "DELETE FROM MHutang WHERE IDTransaksi=" & IDTransaksi & " AND IDJenisTransaksi=" & IDJenisTransaksi & " AND IDWilayah=" & IDWilayah
            EksekusiSQL(SQL)
        Catch ex As Exception
            DevExpress.XtraEditors.XtraMessageBox.Show("Kesalahan perhitugan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
        End Try
    End Sub
    Public Shared Sub PostingStokBarangRevisiHargaPembelian(ByVal IDRevisiHarga As Long)
        Dim ds As New DataSet
        Dim SQL As String = ""
        Dim SaldoStockAkhir As Double = 0.0
        Dim HPP As Double = 0.0
        Dim IDKartuStok As Long = -1
        Try
            ds = modSqlServer.ExecuteDataset("MRevisiHargaBeli", "SELECT MRevisiHargaBeliD.*, MRevisiHargaBeli.Tanggal AS TGLBeli, (MRevisiHargaBeliD.Jumlah/MRevisiHargaBeliD.Qty) AS HargaOut, (MRevisiHargaBeliD.JumlahBaru/MRevisiHargaBeliD.Qty) AS HargaIN FROM MRevisiHargaBeliD INNER JOIN MRevisiHargaBeli ON MRevisiHargaBeli.NoID=MRevisiHargaBeliD.IDRevisiHargaBeli WHERE MRevisiHargaBeli.NoID=" & IDRevisiHarga)
            If ds.Tables("MRevisiHargaBeli").Rows.Count >= 1 Then
                If Not NullTobool(ds.Tables("MRevisiHargaBeli").Rows(0).Item("IsPosted")) Then
                    EksekusiSQL("DELETE FROM MKartuStok WHERE IDTransaksi=" & IDRevisiHarga & " AND IDJenisTransaksi=11")
                    For i As Integer = 0 To ds.Tables("MRevisiHargaBeli").Rows.Count - 1
                        'OUT
                        SaldoStockAkhir = 0 'GetStockAkhirGudang(NullToLong(ds.Tables("MRevisiHargaBeli").Rows(i).Item("IDBarang")), NullToLong(ds.Tables("MRevisiHargaBeli").Rows(i).Item("IDGudang")), NullToDate(ds.Tables("MRevisiHargaBeli").Rows(i).Item("TGLBeli")))
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
                        SQL &= " SELECT " & IDKartuStok & " AS NoID, MRevisiHargaBeli.Kode, MRevisiHargaBeliD.IDBarang, "
                        SQL &= " MWilayah.IDDepartemen, MRevisiHargaBeliD.IDGudang, "
                        SQL &= " MRevisiHargaBeli.Tanggal, 11 AS IDJenisTransaksi,  MRevisiHargaBeli.NoID AS IDTransaksi, "
                        SQL &= " MRevisiHargaBeliD.NoID AS IDTransaksiDetil, MRevisiHargaBeliD.Keterangan, ISNULL(MRevisiHargaBeliD.Qty,0)*IsNull(MRevisiHargaBeliD.Konversi,0) AS Masuk, MRevisiHargaBeliD.Qty AS MasukA, "
                        SQL &= " MRevisiHargaBeliD.Harga AS HrgBeliPerItem, ISNULL(MRevisiHargaBeliD.Jumlah,0) AS JumlahBeli, 0 AS Keluar, 0 AS KeluarA, "
                        SQL &= " 0,0,0 AS HPPKeluar, "
                        SQL &= " 0 AS JumHPPKeluar, " & Replace(SaldoStockAkhir, ",", ".") & "+(ISNULL(MRevisiHargaBeliD.Qty,0)*IsNull(MRevisiHargaBeliD.Konversi,0)) as QtyAkhir, 0,"
                        SQL &= " 0, MBarang.IDJenis, MRevisiHargaBeliD.IDSatuan, "
                        SQL &= " IsNull(MRevisiHargaBeliD.Konversi,0), MRevisiHargaBeliD.IDSupplier, MRevisiHargaBeliD.Ctn "
                        SQL &= " FROM MRevisiHargaBeliD INNER JOIN MRevisiHargaBeli ON MRevisiHargaBeli.NoID=MRevisiHargaBeliD.IDRevisiHargaBeli LEFT JOIN (MGudang LEFT JOIN MWilayah ON MWilayah.NoID=MGudang.IDWilayah) ON MRevisiHargaBeliD.IDGudang=MGudang.NoID"
                        SQL &= " LEFT JOIN MBarang ON MBarang.NoID=MRevisiHargaBeliD.IDBarang"
                        SQL &= " WHERE MRevisiHargaBeliD.NoID=" & NullTolong(ds.Tables("MRevisiHargaBeli").Rows(i).Item("NoID"))
                        EksekusiSQL(SQL)

                        HPP = NullToDbl(EksekusiSQlSkalarNew("SELECT HPP FROM MBarang WHERE NoID=" & NullToLong(ds.Tables(0).Rows(i).Item("IDBarang"))))

                        SQL = "UPDATE MBarang SET HPP=" & FixKoma(HPP) & " WHERE MBarang.NoID=" & NullToLong(ds.Tables(0).Rows(i).Item("IDBarang"))
                        EksekusiSQL(SQL)
                        SQL = "UPDATE MKartuStok SET HPP=" & FixKoma(HPP) & ", HargaBeli=" & FixKoma(NullToDbl(ds.Tables(0).Rows(i).Item("HargaOut"))) & " WHERE NoID=" & IDKartuStok
                        EksekusiSQL(SQL)

                        'IN
                        SaldoStockAkhir = 0 'GetStockAkhirGudang(NullToLong(ds.Tables("MRevisiHargaBeli").Rows(i).Item("IDBarang")), NullToLong(ds.Tables("MRevisiHargaBeli").Rows(i).Item("IDGudang")), NullToDate(ds.Tables("MRevisiHargaBeli").Rows(i).Item("TGLBeli")))
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
                        SQL &= " ,[HgBeliPeritem]"
                        SQL &= " ,[JumlahBeli]"
                        SQL &= " ,[QtyKeluarA]"
                        SQL &= " ,[QtyKeluar]"
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
                        SQL &= " SELECT " & IDKartuStok & " AS NoID, MRevisiHargaBeli.Kode, MRevisiHargaBeliD.IDBarang, "
                        SQL &= " MWilayah.IDDepartemen, MRevisiHargaBeliD.IDGudang, "
                        SQL &= " MRevisiHargaBeli.Tanggal, 11 AS IDJenisTransaksi,  MRevisiHargaBeli.NoID AS IDTransaksi, "
                        SQL &= " MRevisiHargaBeliD.NoID AS IDTransaksiDetil, MRevisiHargaBeliD.Keterangan, ISNULL(MRevisiHargaBeliD.Qty,0)*IsNull(MRevisiHargaBeliD.Konversi,0) AS Masuk, MRevisiHargaBeliD.Qty AS MasukA, "
                        SQL &= " MRevisiHargaBeliD.Harga AS HrgBeliPerItem, ISNULL(MRevisiHargaBeliD.Jumlah,0) AS JumlahBeli, 0 AS Keluar, 0 AS KeluarA, "
                        SQL &= " 0,0,0 AS HPPKeluar, "
                        SQL &= " 0 AS JumHPPKeluar, " & Replace(SaldoStockAkhir, ",", ".") & "+(ISNULL(MRevisiHargaBeliD.Qty,0)*IsNull(MRevisiHargaBeliD.Konversi,0)) as QtyAkhir, 0,"
                        SQL &= " 0, MBarang.IDJenis, MRevisiHargaBeliD.IDSatuan, "
                        SQL &= " IsNull(MRevisiHargaBeliD.Konversi,0), MRevisiHargaBeliD.IDSupplier, MRevisiHargaBeliD.Ctn "
                        SQL &= " FROM MRevisiHargaBeliD INNER JOIN MRevisiHargaBeli ON MRevisiHargaBeli.NoID=MRevisiHargaBeliD.IDRevisiHargaBeli LEFT JOIN (MGudang LEFT JOIN MWilayah ON MWilayah.NoID=MGudang.IDWilayah) ON MRevisiHargaBeliD.IDGudang=MGudang.NoID"
                        SQL &= " LEFT JOIN MBarang ON MBarang.NoID=MRevisiHargaBeliD.IDBarang"
                        SQL &= " WHERE MRevisiHargaBeliD.NoID=" & NullTolong(ds.Tables("MRevisiHargaBeli").Rows(i).Item("NoID"))
                        EksekusiSQL(SQL)

                        HPP = NullToDbl(EksekusiSQlSkalarNew("SELECT HPP FROM MBarang WHERE NoID=" & NullToLong(ds.Tables(0).Rows(i).Item("IDBarang"))))

                        SQL = "UPDATE MBarang SET HPP=" & FixKoma(HPP) & " WHERE MBarang.NoID=" & NullToLong(ds.Tables(0).Rows(i).Item("IDBarang"))
                        EksekusiSQL(SQL)
                        SQL = "UPDATE MKartuStok SET HPP=" & FixKoma(HPP) & ", HargaBeli=" & FixKoma(NullToDbl(ds.Tables(0).Rows(i).Item("HargaIN"))) & " WHERE NoID=" & IDKartuStok
                        EksekusiSQL(SQL)
                    Next
                    InsertMHutang(IDRevisiHarga, 11)
                    EksekusiSQL("UPDATE MRevisiHargaBeli SET IsPosted=1, IDuserPosting=" & IDUserAktif & ", TglPosting=getdate() WHERE NoID=" & IDRevisiHarga)
                End If
            End If
        Catch ex As Exception
            DevExpress.XtraEditors.XtraMessageBox.Show("Kesalahan perhitugan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
        Finally
            ds.Dispose()
        End Try
    End Sub
    Public Shared Sub PostingStokBarangRevisiHargaPenjualan(ByVal IDRevisiHarga As Long)
        Dim ds As New DataSet
        Dim SQL As String = ""
        Dim SaldoStockAkhir As Double = 0.0
        Dim HPP As Double = 0.0
        Dim IDKartuStok As Long = -1
        Try
            ds = modSqlServer.ExecuteDataset("MRevisiHargaJual", "SELECT MRevisiHargaJualD.*, MRevisiHargaJual.Tanggal AS TGLJual, (MRevisiHargaJualD.JumlahBaru/MRevisiHargaJualD.Qty) AS HargaOut, (MRevisiHargaJualD.Jumlah/MRevisiHargaJualD.Qty) AS HargaIN FROM MRevisiHargaJualD INNER JOIN MRevisiHargaJual ON MRevisiHargaJual.NoID=MRevisiHargaJualD.IDRevisiHargaJual WHERE MRevisiHargaJual.NoID=" & IDRevisiHarga)
            If ds.Tables("MRevisiHargaJual").Rows.Count >= 1 Then
                If Not NullToBool(ds.Tables("MRevisiHargaJual").Rows(0).Item("IsPosted")) Then
                    EksekusiSQL("DELETE FROM MKartuStok WHERE IDTransaksi=" & IDRevisiHarga & " AND IDJenisTransaksi=15")
                    For i As Integer = 0 To ds.Tables("MRevisiHargaJual").Rows.Count - 1
                        'IN
                        SaldoStockAkhir = 0 'GetStockAkhirGudang(NullToLong(ds.Tables("MRevisiHargaJual").Rows(i).Item("IDBarang")), NullToLong(ds.Tables("MRevisiHargaJual").Rows(i).Item("IDGudang")), NullToDate(ds.Tables("MRevisiHargaJual").Rows(i).Item("TGLJual")))
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
                        SQL &= " ,[HgbeliPeritem]"
                        SQL &= " ,[JumlahBeli]"
                        SQL &= " ,[QtyKeluarA]"
                        SQL &= " ,[QtyKeluar]"
                        SQL &= " ,[HargaJualPerItem]"
                        SQL &= " ,[JumlahJual]"
                        SQL &= " ,[HPPKeluar]"
                        SQL &= " ,[JumHPPKeluar]"
                        SQL &= " ,[QtyAkhir]"
                        SQL &= " ,[HgbeliRtRtPeritem]"
                        SQL &= " ,[TotHPPRataRata]"
                        SQL &= " ,[IDJenis]"
                        SQL &= " ,[IDSatuan]"
                        SQL &= " ,[Konversi]"
                        SQL &= " ,[IDAlamat]"
                        SQL &= " ,[Ctn_Duz])" & vbCrLf
                        SQL &= " SELECT " & IDKartuStok & " AS NoID, MRevisiHargaJual.Kode, MRevisiHargaJualD.IDBarang, "
                        SQL &= " MWilayah.IDDepartemen, MRevisiHargaJualD.IDGudang, "
                        SQL &= " MRevisiHargaJual.Tanggal, 15 AS IDJenisTransaksi,  MRevisiHargaJual.NoID AS IDTransaksi, "
                        SQL &= " MRevisiHargaJualD.NoID AS IDTransaksiDetil, MRevisiHargaJualD.Keterangan, ISNULL(MRevisiHargaJualD.Qty,0)*IsNull(MRevisiHargaJualD.Konversi,0) AS Masuk, MRevisiHargaJualD.Qty AS MasukA, "
                        SQL &= " MRevisiHargaJualD.Harga AS HrgJualPerItem, ISNULL(MRevisiHargaJualD.Jumlah,0) AS JumlahJual, 0 AS Keluar, 0 AS KeluarA, "
                        SQL &= " 0,0,0 AS HPPKeluar, "
                        SQL &= " 0 AS JumHPPKeluar, " & Replace(SaldoStockAkhir, ",", ".") & "+(ISNULL(MRevisiHargaJualD.Qty,0)*IsNull(MRevisiHargaJualD.Konversi,0)) as QtyAkhir, 0,"
                        SQL &= " 0, MBarang.IDJenis, MRevisiHargaJualD.IDSatuan, "
                        SQL &= " IsNull(MRevisiHargaJualD.Konversi,0), MRevisiHargaJualD.IDCustomer, MRevisiHargaJualD.Ctn "
                        SQL &= " FROM MRevisiHargaJualD INNER JOIN MRevisiHargaJual ON MRevisiHargaJual.NoID=MRevisiHargaJualD.IDRevisiHargaJual LEFT JOIN (MGudang LEFT JOIN MWilayah ON MWilayah.NoID=MGudang.IDWilayah) ON MRevisiHargaJualD.IDGudang=MGudang.NoID"
                        SQL &= " LEFT JOIN MBarang ON MBarang.NoID=MRevisiHargaJualD.IDBarang"
                        SQL &= " WHERE MRevisiHargaJualD.NoID=" & NullToLong(ds.Tables("MRevisiHargaJual").Rows(i).Item("NoID"))
                        EksekusiSQL(SQL)

                        HPP = NullToDbl(EksekusiSQlSkalarNew("SELECT HPP FROM MBarang WHERE NoID=" & NullToLong(ds.Tables(0).Rows(i).Item("IDBarang"))))

                        SQL = "UPDATE MBarang SET HPP=" & FixKoma(HPP) & " WHERE MBarang.NoID=" & NullToLong(ds.Tables(0).Rows(i).Item("IDBarang"))
                        EksekusiSQL(SQL)
                        SQL = "UPDATE MKartuStok SET HPP=" & FixKoma(HPP) & ", HargaBeli=" & FixKoma(NullToDbl(ds.Tables(0).Rows(i).Item("HargaIN"))) & " WHERE NoID=" & IDKartuStok
                        EksekusiSQL(SQL)

                        'OUT
                        SaldoStockAkhir = 0 'GetStockAkhirGudang(NullToLong(ds.Tables("MRevisiHargaJual").Rows(i).Item("IDBarang")), NullToLong(ds.Tables("MRevisiHargaJual").Rows(i).Item("IDGudang")), NullToDate(ds.Tables("MRevisiHargaJual").Rows(i).Item("TGLJual")))
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
                        SQL &= " ,[QtyKeluarA]"
                        SQL &= " ,[QtyKeluar]"
                        SQL &= " ,[HgbeliPeritem]"
                        SQL &= " ,[JumlahBeli]"
                        SQL &= " ,[QtyMasukA]"
                        SQL &= " ,[QtyMasuk]"
                        SQL &= " ,[HargaJualPerItem]"
                        SQL &= " ,[JumlahJual]"
                        SQL &= " ,[HPPKeluar]"
                        SQL &= " ,[JumHPPKeluar]"
                        SQL &= " ,[QtyAkhir]"
                        SQL &= " ,[HgbeliRtRtPeritem]"
                        SQL &= " ,[TotHPPRataRata]"
                        SQL &= " ,[IDJenis]"
                        SQL &= " ,[IDSatuan]"
                        SQL &= " ,[Konversi]"
                        SQL &= " ,[IDAlamat]"
                        SQL &= " ,[Ctn_Duz])" & vbCrLf
                        SQL &= " SELECT " & IDKartuStok & " AS NoID, MRevisiHargaJual.Kode, MRevisiHargaJualD.IDBarang, "
                        SQL &= " MWilayah.IDDepartemen, MRevisiHargaJualD.IDGudang, "
                        SQL &= " MRevisiHargaJual.Tanggal, 15 AS IDJenisTransaksi,  MRevisiHargaJual.NoID AS IDTransaksi, "
                        SQL &= " MRevisiHargaJualD.NoID AS IDTransaksiDetil, MRevisiHargaJualD.Keterangan, ISNULL(MRevisiHargaJualD.Qty,0)*IsNull(MRevisiHargaJualD.Konversi,0) AS Masuk, MRevisiHargaJualD.Qty AS MasukA, "
                        SQL &= " MRevisiHargaJualD.Harga AS HrgJualPerItem, ISNULL(MRevisiHargaJualD.Jumlah,0) AS JumlahJual, 0 AS Keluar, 0 AS KeluarA, "
                        SQL &= " 0,0,0 AS HPPKeluar, "
                        SQL &= " 0 AS JumHPPKeluar, " & Replace(SaldoStockAkhir, ",", ".") & "+(ISNULL(MRevisiHargaJualD.Qty,0)*IsNull(MRevisiHargaJualD.Konversi,0)) as QtyAkhir, 0,"
                        SQL &= " 0, MBarang.IDJenis, MRevisiHargaJualD.IDSatuan, "
                        SQL &= " IsNull(MRevisiHargaJualD.Konversi,0), MRevisiHargaJualD.IDCustomer, MRevisiHargaJualD.Ctn "
                        SQL &= " FROM MRevisiHargaJualD INNER JOIN MRevisiHargaJual ON MRevisiHargaJual.NoID=MRevisiHargaJualD.IDRevisiHargaJual LEFT JOIN (MGudang LEFT JOIN MWilayah ON MWilayah.NoID=MGudang.IDWilayah) ON MRevisiHargaJualD.IDGudang=MGudang.NoID"
                        SQL &= " LEFT JOIN MBarang ON MBarang.NoID=MRevisiHargaJualD.IDBarang"
                        SQL &= " WHERE MRevisiHargaJualD.NoID=" & NullToLong(ds.Tables("MRevisiHargaJual").Rows(i).Item("NoID"))
                        EksekusiSQL(SQL)

                        HPP = NullToDbl(EksekusiSQlSkalarNew("SELECT HPP FROM MBarang WHERE NoID=" & NullToLong(ds.Tables(0).Rows(i).Item("IDBarang"))))

                        SQL = "UPDATE MBarang SET HPP=" & FixKoma(HPP) & " WHERE MBarang.NoID=" & NullToLong(ds.Tables(0).Rows(i).Item("IDBarang"))
                        EksekusiSQL(SQL)
                        SQL = "UPDATE MKartuStok SET HPP=" & FixKoma(HPP) & ", HargaBeli=" & FixKoma(NullToDbl(ds.Tables(0).Rows(i).Item("HargaOut"))) & " WHERE NoID=" & IDKartuStok
                        EksekusiSQL(SQL)
                    Next
                    EksekusiSQL("UPDATE MRevisiHargaJual SET IsPosted=1, IDuserPosting=" & IDUserAktif & ", TglPosting=getdate() WHERE NoID=" & IDRevisiHarga)
                End If
            End If
        Catch ex As Exception
            DevExpress.XtraEditors.XtraMessageBox.Show("Kesalahan perhitugan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
        Finally
            ds.Dispose()
        End Try
    End Sub
    Public Shared Sub UnPostingStokBarangPembelian(ByVal IDBeli As Long)
        Dim ds As New DataSet
        Dim dstemp As New DataSet
        Dim SaldoAkhir As Double = 0.0
        Dim HPPSekarang As Double = 0.0
        Dim HPP As Double = 0.0
        Dim HargaBeli As Double = 0.0

        Dim x As New frmOtorisasiAdmin
        Dim SQL As String = ""
        Try
            ds = ExecuteDataset("NamaTabel", "SELECT MKartuStok.* FROM MKartuStok WHERE (MKartuStok.IsPosted=0 OR MKartuStok.IsPosted Is Null) AND MKartuStok.IDTransaksi=" & IDBeli & " AND MKartuStok.IDJenisTransaksi=2 ")
            If ds.Tables(0).Rows.Count >= 1 Then
                If Not NullToBool(ds.Tables(0).Rows(0).Item("IsPosted")) Then
                    If x.ShowDialog(frmMain) = DialogResult.OK Then
                        If Not PembelianAdaDiReturBeli(IDBeli) AndAlso Not PembelianAdaDiRevisiHargabeli(IDBeli) And Not TransaksiAdaDiTT(IDBeli, 2) Then
                            EksekusiSQL("DELETE FROM MKartuStok WHERE IDTransaksi=" & IDBeli & " AND IDJenisTransaksi=2")
                            EksekusiSQL("UPDATE MBeli SET IsPosted=0, IDUserPosting=NULL, TglPosting=NULL WHERE NoID=" & IDBeli)
                            DeleteMHutang(IDBeli, 2, DefIDWilayah)

                            If IsUpdateHargaBeli Then
                                'Update HPP (pembelian) : StockAkhir*HPP+QtyBeli*HargaBeli/StockAkhir+QtyBeli
                                For i As Integer = 0 To ds.Tables(0).Rows.Count - 1
                                    SQL = "SELECT TOP 1 MKartuStok.* FROM MKartuStok LEFT JOIN MJenisTransaksi ON MJenisTransaksi.ID=MkartuStok.IDJenisTransaksi WHERE Not(MKartuStok.HPP=0 OR MKartuStok.HPP IS NULL) AND MKartuStok.IDBarang=" & NullToLong(ds.Tables(0).Rows(i).Item("IDBarang")) & "  ORDER BY MKartuStok.Tanggal DESC, MJenisTransaksi.NoUrut DESC, MKartuStok.NoID DESC"
                                    dstemp = ExecuteDataset("MKartuStok", SQL)
                                    If dstemp.Tables(0).Rows.Count >= 1 Then
                                        If FixKoma(NullToDbl(dstemp.Tables(0).Rows(0).Item("HargaBeli"))) = 0 Then
                                            SQL = "UPDATE MBarang SET HPP=" & FixKoma(NullToDbl(dstemp.Tables(0).Rows(0).Item("HPP"))) & " WHERE NoID=" & NullToLong(ds.Tables(0).Rows(i).Item("IDBarang"))
                                        Else
                                            SQL = "UPDATE MBarang SET HPP=" & FixKoma(NullToDbl(dstemp.Tables(0).Rows(0).Item("HPP"))) & ", TerakhirBeli='" & Format(NullToDate(dstemp.Tables(0).Rows(0).Item("Tanggal")), "yyyy-MM-dd HH:mm") & "', HargaBeli=" & FixKoma(NullToDbl(dstemp.Tables(0).Rows(0).Item("HargaBeli"))) & " WHERE NoID=" & NullToLong(ds.Tables(0).Rows(i).Item("IDBarang"))
                                        End If
                                        EksekusiSQL(SQL)
                                    End If
                                Next
                            End If
                        End If
                    End If
                End If
            ElseIf ds.Tables(0).Rows.Count = 0 Then
                EksekusiSQL("UPDATE MBeli SET IsPosted=0, IDUserPosting=NULL, TglPosting=NULL WHERE NoID=" & IDBeli)
                DeleteMHutang(IDBeli, 2, DefIDWilayah)
            End If
        Catch ex As Exception
            DevExpress.XtraEditors.XtraMessageBox.Show("Kesalahan perhitungan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
        Finally
            x.Dispose()
            dstemp.Dispose()
            ds.Dispose()
        End Try
    End Sub
    Public Shared Sub PostingStokBarangReturPembelian(ByVal IDRetur As Long, Optional ByVal IsForce As Boolean = False)
        Dim ds As New DataSet
        Dim SQL As String = ""
        Dim SaldoStockAkhir As Double = 0.0
        Dim SaldoAkhir As Double = 0.0
        Dim HPPSekarang As Double = 0.0
        Dim HPP As Double = 0.0
        Dim IDKartuStok As Long = -1
        Try
            ds = modSqlServer.ExecuteDataset("MJualD", "SELECT MReturBeliD.*, MReturBeli.IsPosted, MReturBeli.Tanggal AS TGLJual, MReturBeli.DiskonNotaProsen, (MReturBeliD.Jumlah/MReturBeliD.Qty)-((MReturBeliD.Jumlah/MReturBeliD.Qty)*MReturBeli.DiskonNotaProsen/100) AS HargaRetur FROM MReturBeliD LEFT JOIN MReturBeli ON MReturBeli.NoID=MReturBeliD.IDReturBeli WHERE MReturBeliD.IDReturBeli=" & IDRetur)
            If ds.Tables("MJualD").Rows.Count >= 1 Then
                If (Not NullToBool(ds.Tables("MJualD").Rows(0).Item("IsPosted"))) Or (IsForce AndAlso NullToBool(ds.Tables("MJualD").Rows(0).Item("IsPosted"))) Then
                    EksekusiSQL("DELETE FROM MKartuStok WHERE IDTransaksi=" & IDRetur & " AND IDJenisTransaksi=3")
                    For i As Integer = 0 To ds.Tables("MJualD").Rows.Count - 1
                        SaldoStockAkhir = 0 'GetStockAkhirGudang(NullTolong(ds.Tables("MJualD").Rows(i).Item("IDBarang")), NullTolong(ds.Tables("MJualD").Rows(i).Item("IDGudang")), NullToDate(ds.Tables("MJualD").Rows(i).Item("TGLJual")))
                        IDKartuStok = GetNewID("MKartuStok", "NoID")
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
                        SQL &= " ,[IDAlamat])" & vbCrLf
                        SQL &= " SELECT " & IDKartuStok & " AS NoID, MReturBeli.Kode, MReturBeliD.IDBarang,MReturBeliD.IDBarangD, "
                        SQL &= " MWilayah.IDDepartemen, MReturBeliD.IDGudang, "
                        SQL &= " MReturBeli.Tanggal, 3 AS IDJenisTransaksi, MReturBeli.NoID AS IDTransaksi, "
                        SQL &= " MReturBeliD.NoID AS IDTransaksiDetil, MReturBeliD.Catatan, (ISNULL(MReturBeliD.Qty,0)*ISNULL(MReturBeliD.Konversi,0)) AS Keluar, MReturBeliD.Qty AS KeluarA,"
                        SQL &= " MReturBeliD.Harga AS HrgJualPerItem, ISNULL(MReturBeliD.Jumlah,0), 0 AS Masuk,0 AS MasukA,"
                        SQL &= " 0,0,0 AS HPPKeluar, "
                        SQL &= " 0 AS JumHPPKeluar, " & Replace(SaldoStockAkhir, ",", ".") & "-(ISNULL(MReturBeliD.Qty,0)*ISNULL(MReturBeliD.Konversi,0)) as QtyAkhir, 0,"
                        SQL &= " 0, MBarang.IDJenis, MReturBeliD.IDSatuan, "
                        SQL &= " Isnull(MReturBeliD.Konversi,0), MReturBeli.IDSupplier "
                        SQL &= " FROM MReturBeliD LEFT JOIN MReturBeli ON MReturBeli.NoID=MReturBeliD.IDReturBeli"
                        SQL &= " LEFT JOIN (MGudang LEFT JOIN MWilayah ON MWilayah.NoID=MGudang.IDWilayah) ON MReturBeliD.IDGudang=MGudang.NoID"
                        SQL &= " LEFT JOIN MBarang ON MBarang.NoID=MReturBeliD.IDBarang"
                        SQL &= " WHERE MReturBeliD.NoID=" & NullToLong(ds.Tables("MJualD").Rows(i).Item("NoID"))
                        EksekusiSQL(SQL)

                        If IsUpdateHargaBeli Then
                            ''Update Harga Beli Master Stock
                            'SQL = "UPDATE MBarang SET TerakhirBeli=MBeli.Tanggal, HargaBeli=(MBeliD.Jumlah/MBeliD.Qty)-((MBeliD.Jumlah/MBeliD.Qty)*MBeli.DiskonNotaProsen/100)" & _
                            '" FROM (MBeliD " & _
                            '" LEFT JOIN MBeli ON MBeli.NoID=MBeliD.IDBeli) " & _
                            '" LEFT JOIN MBarang ON MBarang.NoID=MBeliD.IDBarang " & _
                            '" WHERE MBarang.NoID=" & NullToLong(ds.Tables("MBeliD").Rows(i).Item("IDBarang")) & " AND MBeliD.NoID=" & NullToLong(ds.Tables("MBeliD").Rows(i).Item("NoID"))
                            'EksekusiSQL(SQL)

                            'Update HPP (pembelian) : StockAkhir*HPP+QtyBeli*HargaBeli/StockAkhir+QtyBeli
                            SaldoAkhir = NullToDbl(EksekusiSQlSkalarNew("SELECT (MKartuStok.QtyMasuk*MKartuStok.Konversi)-(MKartuStok.QtyKeluar*MKartuStok.Konversi) WHERE MKartuStok.IDBarang=" & NullToLong(ds.Tables(0).Rows(i).Item("IDBarang"))))
                            'If SaldoAkhir - (NullToDbl(ds.Tables(0).Rows(i).Item("Qty")) * NullToDbl(ds.Tables(0).Rows(i).Item("Konversi"))) <= 0 Then
                            '    If XtraMessageBox.Show("Stock sama dengan kosong ingin mengupdatekan HPP?", NamaAplikasi, MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) = DialogResult.No Then
                            '        Exit For
                            '    End If
                            'End If

                            HPPSekarang = NullToDbl(EksekusiSQlSkalarNew("SELECT HPP FROM MBarang WHERE NoID=" & NullToLong(ds.Tables(0).Rows(i).Item("IDBarang"))))
                            HPP = (SaldoAkhir * HPPSekarang) - (NullToDbl(ds.Tables(0).Rows(i).Item("Qty")) * NullToDbl(ds.Tables(0).Rows(i).Item("Konversi")) * NullToDbl(ds.Tables(0).Rows(i).Item("HargaRetur")))
                            If ((SaldoAkhir - (NullToDbl(ds.Tables(0).Rows(i).Item("Qty")) * NullToDbl(ds.Tables(0).Rows(i).Item("Konversi"))))) = 0 Then
                                HPP = NullToDbl(ds.Tables(0).Rows(i).Item("HargaRetur"))
                            Else
                                HPP = HPP / (SaldoAkhir - (NullToDbl(ds.Tables(0).Rows(i).Item("Qty")) * NullToDbl(ds.Tables(0).Rows(i).Item("Konversi"))))
                            End If

                            SQL = "UPDATE MBarang SET HPP=" & FixKoma(HPP) & " WHERE MBarang.NoID=" & NullToLong(ds.Tables(0).Rows(i).Item("IDBarang"))
                            EksekusiSQL(SQL)
                            SQL = "UPDATE MKartuStok SET HPP=" & FixKoma(HPP) & ", HargaBeli=" & FixKoma(NullToDbl(ds.Tables(0).Rows(i).Item("HargaRetur"))) & " WHERE NoID=" & IDKartuStok
                            EksekusiSQL(SQL)
                        End If
                    Next
                    InsertMHutang(IDRetur, 3)
                End If
            End If
            EksekusiSQL("UPDATE MReturBeli SET IsPosted=1, TglPosting=getdate(), IDUserPosting=" & IDUserAktif & " WHERE NoID=" & IDRetur)

        Catch ex As Exception
            DevExpress.XtraEditors.XtraMessageBox.Show("Kesalahan perhitugan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
        Finally
            ds.Dispose()
        End Try
    End Sub
    Public Shared Sub UnPostingStokBarangReturPembelian(ByVal IDJual As Long)
        Dim ds As New DataSet
        Dim dstemp As New DataSet
        Dim SQL As String = ""
        Try
            ds = ExecuteDataset("NamaTabel", "SELECT MKartuStok.* FROM MKartuStok WHERE (MKartuStok.IsPosted=0 OR MKartuStok.IsPosted Is Null) AND MKartuStok.IDTransaksi=" & IDJual & " AND MKartuStok.IDJenisTransaksi=3 ")
            If ds.Tables(0).Rows.Count >= 1 Then
                If Not NullToBool(ds.Tables(0).Rows(0).Item("IsPosted")) AndAlso Not TransaksiAdaDiTT(IDJual, 3) AndAlso Not ReturAdaDiPembayaranHutang(IDJual) Then
                    EksekusiSQL("DELETE FROM MKartuStok WHERE IDTransaksi=" & IDJual & " AND MKartuStok.IDJenisTransaksi=3")
                    EksekusiSQL("UPDATE MReturBeli SET IsPosted=0, IDUserPosting=NULL, TglPosting=NULL WHERE NoID=" & IDJual)
                    DeleteMHutang(IDJual, 3, DefIDWilayah)
                    'Update HPP (pembelian) : StockAkhir*HPP+QtyBeli*HargaBeli/StockAkhir+QtyBeli
                    If IsUpdateHargaBeli Then
                        For i As Integer = 0 To ds.Tables(0).Rows.Count - 1
                            SQL = "SELECT TOP 1 MKartuStok.* FROM MKartuStok LEFT JOIN MJenisTransaksi ON MJenisTransaksi.ID=MkartuStok.IDJenisTransaksi WHERE Not(MKartuStok.HPP=0 OR MKartuStok.HPP IS NULL) AND MKartuStok.IDBarang=" & NullToLong(ds.Tables(0).Rows(i).Item("IDBarang")) & "  ORDER BY MKartuStok.Tanggal DESC, MJenisTransaksi.NoUrut DESC, MKartuStok.NoID DESC"
                            dstemp = ExecuteDataset("MKartuStok", SQL)
                            If dstemp.Tables(0).Rows.Count >= 1 Then
                                If FixKoma(NullToDbl(dstemp.Tables(0).Rows(0).Item("HargaBeli"))) = 0 Then
                                    SQL = "UPDATE MBarang SET HPP=" & FixKoma(NullToDbl(dstemp.Tables(0).Rows(0).Item("HPP"))) & " WHERE NoID=" & NullToLong(ds.Tables(0).Rows(i).Item("IDBarang"))
                                Else
                                    SQL = "UPDATE MBarang SET HPP=" & FixKoma(NullToDbl(dstemp.Tables(0).Rows(0).Item("HPP"))) & ", TerakhirBeli='" & Format(NullToDate(dstemp.Tables(0).Rows(0).Item("Tanggal")), "yyyy-MM-dd HH:mm") & "', HargaBeli=" & FixKoma(NullToDbl(dstemp.Tables(0).Rows(0).Item("HargaBeli"))) & " WHERE NoID=" & NullToLong(ds.Tables(0).Rows(i).Item("IDBarang"))
                                End If
                                EksekusiSQL(SQL)
                            End If
                        Next
                    End If
                End If
            ElseIf ds.Tables(0).Rows.Count = 0 Then
                EksekusiSQL("UPDATE MReturBeli SET IsPosted=0, IDUserPosting=NULL, TglPosting=NULL WHERE NoID=" & IDJual)
                DeleteMHutang(IDJual, 3, DefIDWilayah)
            End If
        Catch ex As Exception
            DevExpress.XtraEditors.XtraMessageBox.Show("Kesalahan perhitugan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
        Finally
            dstemp.Dispose()
            ds.Dispose()
        End Try
    End Sub
    Public Shared Sub UnPostingStokBarangRevisiHargaPembelian(ByVal IDRevisi As Long)
        Dim ds As New DataSet
        Try
            ds = ExecuteDataset("NamaTabel", "SELECT MKartuStok.* FROM MKartuStok WHERE (MKartuStok.IsPosted=0 OR MKartuStok.IsPosted Is Null) AND MKartuStok.IDTransaksi=" & IDRevisi & " AND MKartuStok.IDJenisTransaksi=11 ")
            If ds.Tables(0).Rows.Count >= 1 Then
                If Not NullToBool(ds.Tables(0).Rows(0).Item("IsPosted")) Then
                    If Not KodeReffDipakaiDiRevisiHarga(IDRevisi) Then
                        EksekusiSQL("DELETE FROM MKartuStok WHERE IDTransaksi=" & IDRevisi & " AND MKartuStok.IDJenisTransaksi=11")
                        EksekusiSQL("UPDATE MRevisiHargaBeli SET IsPosted=0, IDUserPosting=NULL, TglPosting=NULL WHERE NoID=" & IDRevisi)
                        DeleteMHutang(IDRevisi, 11, DefIDWilayah)
                    End If
                End If
            ElseIf ds.Tables(0).Rows.Count = 0 Then
                EksekusiSQL("UPDATE MRevisiHargaBeli SET IsPosted=0, IDUserPosting=NULL, TglPosting=NULL WHERE NoID=" & IDRevisi)
                DeleteMHutang(IDRevisi, 11, DefIDWilayah)
            End If
        Catch ex As Exception
            DevExpress.XtraEditors.XtraMessageBox.Show("Kesalahan perhitugan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
        Finally
            ds.Dispose()
        End Try
    End Sub
    Private Shared Function KodeReffDipakaiDiRevisiHarga(ByVal IDRevisi As Long) As Boolean
        Dim ds As New DataSet
        Dim SQL As String = ""
        Try
            SQL = "SELECT MRevisiHargaBeliD.* FROM MRevisiHargaBeliD WHERE MRevisiHargaBeliD.KodeReff<>'' AND MRevisiHargaBeliD.KodeReff IN " & vbCrLf & _
            " (SELECT B.Kode FROM MRevisiHargaBeliD A INNER JOIN MRevisiHargaBeli B ON B.NoID=A.IDRevisiHargaBeli WHERE B.NoID=" & IDRevisi & ")"
            ds = ExecuteDataset("MData", SQL)
            If ds.Tables(0).Rows.Count >= 1 Then
                Return True
            Else
                Return False
            End If
        Catch ex As Exception
            XtraMessageBox.Show("Info : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
            Return True
        Finally
            ds.Dispose()
        End Try
    End Function
    Private Shared Function KodeReffDipakaiDiRevisiHargaJual(ByVal IDRevisi As Long) As Boolean
        Dim ds As New DataSet
        Dim SQL As String = ""
        Try
            SQL = "SELECT MRevisiHargaJualD.* FROM MRevisiHargaJualD WHERE MRevisiHargaJualD.KodeReff<>'' AND MRevisiHargaJualD.KodeReff IN " & vbCrLf & _
            " (SELECT B.Kode FROM MRevisiHargaJualD A INNER JOIN MRevisiHargaJual B ON B.NoID=A.IDRevisiHargaJual WHERE B.NoID=" & IDRevisi & ")"
            ds = ExecuteDataset("MData", SQL)
            If ds.Tables(0).Rows.Count >= 1 Then
                Return True
            Else
                Return False
            End If
        Catch ex As Exception
            XtraMessageBox.Show("Info : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
            Return True
        Finally
            ds.Dispose()
        End Try
    End Function
    Public Shared Sub UnPostingStokBarangRevisiHargaPenjualan(ByVal IDRevisi As Long)
        Dim ds As New DataSet
        Try
            ds = ExecuteDataset("NamaTabel", "SELECT MKartuStok.* FROM MKartuStok WHERE (MKartuStok.IsPosted=0 OR MKartuStok.IsPosted Is Null) AND MKartuStok.IDTransaksi=" & IDRevisi & " AND MKartuStok.IDJenisTransaksi=15 ")
            If ds.Tables(0).Rows.Count >= 1 Then
                If Not NullToBool(ds.Tables(0).Rows(0).Item("IsPosted")) Then
                    If Not KodeReffDipakaiDiRevisiHargaJual(IDRevisi) Then
                        EksekusiSQL("DELETE FROM MKartuStok WHERE IDTransaksi=" & IDRevisi & " AND MKartuStok.IDJenisTransaksi=15")
                        EksekusiSQL("UPDATE MRevisiHargaJual SET IsPosted=0, IDUserPosting=NULL, TglPosting=NULL WHERE NoID=" & IDRevisi)
                    End If
                End If
            ElseIf ds.Tables(0).Rows.Count = 0 Then
                EksekusiSQL("UPDATE MRevisiHargaJual SET IsPosted=0, IDUserPosting=NULL, TglPosting=NULL WHERE NoID=" & IDRevisi)
            End If
        Catch ex As Exception
            DevExpress.XtraEditors.XtraMessageBox.Show("Kesalahan perhitugan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
        Finally
            ds.Dispose()
        End Try
    End Sub
    Public Shared Sub PostingStokBarangMutasiGudang(ByVal IDMutasi As Long)
        Dim ds As New DataSet
        Dim SQL As String = ""
        Dim SaldoStockAkhir As Double = 0.0
        Dim HPPPerItem As Double = 0.0
        Try
            ds = modSqlServer.ExecuteDataset("MMutasiGudangD", "SELECT MMutasiGudangD.*, MMutasiGudang.IsPosted, MMutasiGudang.Tanggal AS TGLMutasi, MMutasiGudang.IDGudangAsal, MMutasiGudang.IDGudangTujuan FROM MMutasiGudangD LEFT JOIN MMutasiGudang ON MMutasiGudang.NoID=MMutasiGudangD.IDMutasiGudang WHERE MMutasiGudang.NoID=" & IDMutasi)
            If ds.Tables("MMutasiGudangD").Rows.Count >= 1 Then
                If Not NullTobool(ds.Tables("MMutasiGudangD").Rows(0).Item("IsPosted")) Then
                    EksekusiSQL("DELETE FROM MKartuStok WHERE IDTransaksi=" & IDMutasi & " AND (IDJenisTransaksi=5 OR IDJenisTransaksi=4)")
                    For i As Integer = 0 To ds.Tables("MMutasiGudangD").Rows.Count - 1
                        'Keluarkan
                        SaldoStockAkhir = 0 'GetStockAkhirGudang(NullTolong(ds.Tables("MMutasiGudangD").Rows(i).Item("IDBarang")), NullTolong(ds.Tables("MMutasiGudangD").Rows(i).Item("IDGudangAsal")), NullToDate(ds.Tables("MMutasiGudangD").Rows(i).Item("TGLMutasi")))
                        'HPPPerItem = GetHPP(NullTolong(ds.Tables("MMutasiGudangD").Rows(i).Item("IDBarang")), NullToDate(ds.Tables("MMutasiGudangD").Rows(i).Item("TGLMutasi")))
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
                        SQL &= " ,[HargaJualPerItem]"
                        SQL &= " ,[JumlahJual]"
                        SQL &= " ,[QtyMasukA]"
                        SQL &= " ,[QtyMasuk]"
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
                        SQL &= " SELECT " & GetNewID("MKartuStok", "NoID") & " AS NoID, MMutasiGudang.Kode, MMutasiGudangD.IDBarang, MMUTasiGudangD.IDBarangD, "
                        SQL &= " MWilayah.IDDepartemen, MMutasiGudang.IDGudangAsal, "
                        SQL &= " MMUtasiGudang.Tanggal, 4 AS IDJenisTransaksi,  MMutasiGudang.NoID AS IDTransaksi, "
                        SQL &= " MMutasiGudangD.NoID AS IDTransaksiDetil, MMutasiGudang.Keterangan, (ISNULL(MMutasiGudangD.Qty,0)*ISNULL(MMutasiGudangD.Konversi,0)) AS KeluarA, MMutasiGudangD.Qty AS Keluar,"
                        SQL &= " 0 AS HrgJualPerItem, 0 AS Jumlah, 0 AS Masuk,0 AS MasukA,"
                        SQL &= " 0,0,0 AS HPPKeluar, "
                        SQL &= " 0 AS JumHPPKeluar, " & Replace(SaldoStockAkhir, ",", ".") & "-(ISNULL(MMutasiGudangD.Qty,0)*ISNULL(MMutasiGudangD.Konversi,0)) as QtyAkhir, 0,"
                        SQL &= " 0, MBarang.IDJenis, MMutasiGudangD.IDSatuan, "
                        SQL &= " isnull(MMutasiGudangD.Konversi,0), 0 AS IDAlamat "
                        SQL &= " FROM MMutasiGudangD LEFT JOIN MMutasiGudang ON MMutasiGudang.NoID=MMutasiGudangD.IDMutasiGudang"
                        SQL &= " LEFT JOIN (MGudang LEFT JOIN MWilayah ON MWilayah.NoID=MGudang.IDWilayah) ON MMutasiGudang.IDGudangAsal=MGudang.NoID"
                        SQL &= " LEFT JOIN MBarang ON MBarang.NoID=MMutasiGudangD.IDBarang"
                        SQL &= " WHERE MMutasiGudangD.NoID=" & NullTolong(ds.Tables("MMutasiGudangD").Rows(i).Item("NoID"))
                        EksekusiSQL(SQL)

                        'Masukkan
                        SaldoStockAkhir = 0 'GetStockAkhirGudang(NullTolong(ds.Tables("MMutasiGudangD").Rows(i).Item("IDBarang")), NullTolong(ds.Tables("MMutasiGudangD").Rows(i).Item("IDGudangTujuan")), NullToDate(ds.Tables("MMutasiGudangD").Rows(i).Item("TGLMutasi")))
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
                        SQL &= " ,[HgBeliPeritem]"
                        SQL &= " ,[JumlahBeli]"
                        SQL &= " ,[QtyKeluarA]"
                        SQL &= " ,[QtyKeluar]"
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
                        SQL &= " ,[IDAlamat])" & vbCrLf
                        SQL &= " SELECT " & GetNewID("MKartuStok", "NoID") & " AS NoID, MMutasiGudang.Kode, MMutasiGudangD.IDBarang, "
                        SQL &= " MWilayah.IDDepartemen, MMutasiGudang.IDGudangTujuan, "
                        SQL &= " MMUtasiGudang.Tanggal, 5 AS IDJenisTransaksi,  MMutasiGudang.NoID AS IDTransaksi, "
                        SQL &= " MMutasiGudangD.NoID AS IDTransaksiDetil, MMutasiGudang.Keterangan, "
                        SQL &= " ISNULL(MMutasiGudangD.Qty,0)*ISNULL(MMutasiGudangD.Konversi,0) AS MasukA, MMutasiGudangD.Qty AS Masuk, 0 AS HrgBeliPerItem, 0 AS JumlahBeli, "
                        SQL &= " 0 AS KeluarA,0 AS Keluar, 0 AS HrgJualPerItem, 0 AS JumlahJual, "
                        SQL &= " 0 AS HPPKeluar, 0 AS JumHPPKeluar, "
                        SQL &= Replace(SaldoStockAkhir, ",", ".") & "+(ISNULL(MMutasiGudangD.Qty,0)*ISNULL(MMutasiGudangD.Konversi,0)) as QtyAkhir, 0,"
                        SQL &= " 0, MBarang.IDJenis, MMutasiGudangD.IDSatuan, "
                        SQL &= " MMutasiGudangD.Konversi, 0 AS IDAlamat "
                        SQL &= " FROM MMutasiGudangD LEFT JOIN MMutasiGudang ON MMutasiGudang.NoID=MMutasiGudangD.IDMutasiGudang"
                        SQL &= " LEFT JOIN (MGudang LEFT JOIN MWilayah ON MWilayah.NoID=MGudang.IDWilayah) ON MMutasiGudang.IDGudangTujuan=MGudang.NoID"
                        SQL &= " LEFT JOIN MBarang ON MBarang.NoID=MMutasiGudangD.IDBarang"
                        SQL &= " WHERE MMutasiGudangD.NoID=" & NullTolong(ds.Tables("MMutasiGudangD").Rows(i).Item("NoID"))
                        EksekusiSQL(SQL)

                    Next

                    EksekusiSQL("UPDATE MMutasiGudang SET IsPosted=1 WHERE NoID=" & IDMutasi)
                End If
            End If
        Catch ex As Exception
            DevExpress.XtraEditors.XtraMessageBox.Show("Kesalahan perhitugan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
        Finally
            ds.Dispose()
        End Try
    End Sub
    Public Shared Sub UnPostingStokBarangMutasiGudang(ByVal IDMutasiGudang As Long)
        Dim ds As New DataSet
        Try
            ds = ExecuteDataset("NamaTabel", "SELECT MKartuStok.* FROM MKartuStok LEFT JOIN MMutasiGudang ON MMutasiGudang.NoID=MKartuStok.IDTransaksi WHERE (MKartuStok.IsPosted=0 OR MKartuStok.IsPosted Is Null) AND MMutasiGudang.NoID=" & IDMutasiGudang & " AND (MKartuStok.IDJenisTransaksi=4 OR MKartuStok.IDJenisTransaksi=5)")
            If ds.Tables(0).Rows.Count >= 1 Then
                If Not NullTobool(ds.Tables(0).Rows(0).Item("IsPosted")) Then
                    EksekusiSQL("DELETE FROM MKartuStok WHERE IDTransaksi=" & IDMutasiGudang & " AND (IDJenisTransaksi=4 OR IDJenisTransaksi=5)")
                    EksekusiSQL("UPDATE MMutasiGudang SET IsPosted=0 WHERE NoID=" & IDMutasiGudang)
                End If
            ElseIf ds.Tables(0).Rows.Count = 0 Then
                EksekusiSQL("UPDATE MMutasiGudang SET IsPosted=0 WHERE NoID=" & IDMutasiGudang)
            End If
        Catch ex As Exception
            DevExpress.XtraEditors.XtraMessageBox.Show("Kesalahan perhitugan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
        Finally
            ds.Dispose()
        End Try
    End Sub
End Class
