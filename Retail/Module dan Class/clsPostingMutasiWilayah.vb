Imports VPoint.Ini
Imports System.Data
Imports System.Data.SqlClient
Imports DevExpress.XtraEditors
Imports DevExpress.Utils
Imports VPoint.clsPostingKartuStok

Public Class clsPostingMutasiWilayah
    Private Shared Function RequestDiACC(ByVal IDSO As Long) As Boolean
        Dim ds As New DataSet
        Try
            ds = ExecuteDataset("NamaTabel", "SELECT MRequestMutasiWilayah.* FROM MRequestMutasiWilayah WHERE MRequestMutasiWilayah.IsAccPusat=1 AND MRequestMutasiWilayah.NoID=" & IDSO)
            If ds.Tables(0).Rows.Count >= 1 Then
                Return True
            Else
                Return False
            End If
        Catch ex As Exception
            Return False
        End Try

    End Function
    Private Shared Function RequestAdaDiSPK(ByVal IDSO As Long) As Boolean
        Dim ds As New DataSet
        Try
            ds = ExecuteDataset("NamaTabel", "SELECT MSPKMutasiWilayah.* FROM MSPKMutasiWilayah WHERE MSPKMutasiWilayah.IDRequestMutasiWilayah=" & IDSO)
            If ds.Tables(0).Rows.Count >= 1 Then
                Return True
            Else
                Return False
            End If
        Catch ex As Exception
            Return False
        End Try

    End Function
    Private Shared Function SPKAdaDiPacking(ByVal IDSO As Long) As Boolean
        Dim ds As New DataSet
        Try
            ds = ExecuteDataset("NamaTabel", "SELECT MPackingMutasiWilayah.* FROM MPackingMutasiWilayah WHERE MPackingMutasiWilayah.IDSPKMutasiWilayah=" & IDSO)
            If ds.Tables(0).Rows.Count >= 1 Then
                Return True
            Else
                Return False
            End If
        Catch ex As Exception
            Return False
        End Try

    End Function
    Private Shared Function TOAdaDiTI(ByVal IDSO As Long) As Boolean
        Dim ds As New DataSet
        Try
            ds = ExecuteDataset("NamaTabel", "SELECT MTransferIND.* FROM MTransferIND INNER JOIN MTransferOut ON MTransferIN.NoID=MTransferIND.IDHeader WHERE MTransferIN.IDTransferOut=" & IDSO)
            If ds.Tables(0).Rows.Count >= 1 Then
                Return True
            Else
                Return False
            End If
        Catch ex As Exception
            Return False
        End Try

    End Function
    Private Shared Function PackingAdaDiTransferOut(ByVal IDSO As Long) As Boolean
        Dim ds As New DataSet
        Try
            ds = ExecuteDataset("NamaTabel", "SELECT MTransferOutD.* FROM MTransferOutD WHERE MTransferOutD.IDPackingMutasiWilayah=" & IDSO)
            If ds.Tables(0).Rows.Count >= 1 Then
                Return True
            Else
                Return False
            End If
        Catch ex As Exception
            Return False
        End Try

    End Function
    Public Shared Sub PostingRequest(ByVal IDSO As Long)
        Dim ds As New DataSet
        Dim SQL As String = ""
        Try
            ds = modSqlServer.ExecuteDataset("MREQUEST", "SELECT MRequestMutasiWilayahD.*, MRequestMutasiWilayah.IsPosted, MRequestMutasiWilayah.Tanggal FROM MRequestMutasiWilayahD LEFT JOIN MRequestMutasiWilayah ON MRequestMutasiWilayah.NoID=MRequestMutasiWilayahD.IDHeader WHERE MRequestMutasiWilayah.NoID=" & IDSO)
            If ds.Tables("MREQUEST").Rows.Count >= 1 Then
                If Not NullToBool(ds.Tables("MREQUEST").Rows(0).Item("IsPosted")) Then
                    EksekusiSQL("UPDATE MRequestMutasiWilayah SET IsPosted=1, IDUserPosting=" & IDUserAktif & ", TanggalPosting=getdate(), JamPosting=Getdate() WHERE NoID=" & IDSO)
                End If
            End If
        Catch ex As Exception
            DevExpress.XtraEditors.XtraMessageBox.Show("Kesalahan perhitugan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
        Finally
            ds.Dispose()
        End Try
    End Sub
    Public Shared Sub RequestDiAccPusat(ByVal IDSO As Long)
        Dim ds As New DataSet
        Dim SQL As String = ""
        Try
            ds = modSqlServer.ExecuteDataset("MREQUEST", "SELECT MRequestMutasiWilayahD.*, MRequestMutasiWilayah.IsPosted, MRequestMutasiWilayah.Tanggal FROM MRequestMutasiWilayahD LEFT JOIN MRequestMutasiWilayah ON MRequestMutasiWilayah.NoID=MRequestMutasiWilayahD.IDHeader WHERE MRequestMutasiWilayah.NoID=" & IDSO)
            If ds.Tables("MREQUEST").Rows.Count >= 1 Then
                If NullToBool(ds.Tables("MREQUEST").Rows(0).Item("IsPosted")) Then
                    EksekusiSQL("UPDATE MRequestMutasiWilayah SET IsAccPusat=1, IDUserAcc=" & IDUserAktif & ", TanggalAcc=getdate(), JamAcc=Getdate() WHERE NoID=" & IDSO)
                End If
            End If
        Catch ex As Exception
            DevExpress.XtraEditors.XtraMessageBox.Show("Kesalahan perhitugan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
        Finally
            ds.Dispose()
        End Try
    End Sub
    Public Shared Sub UnPostingRequest(ByVal IDSO As Long)
        Dim ds As New DataSet
        Try
            If Not RequestDiACC(IDSO) AndAlso Not RequestAdaDiSPK(IDSO) Then
                EksekusiSQL("UPDATE MRequestMutasiWilayah SET IsPosted=0, IDUserPosting=NULL, TanggalPosting=NULL, JamPosting=NULL WHERE NoID=" & IDSO)
            End If
        Catch ex As Exception
            DevExpress.XtraEditors.XtraMessageBox.Show("Kesalahan perhitugan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
        Finally
            ds.Dispose()
        End Try
    End Sub
    Public Shared Sub UnAccPusatRequest(ByVal IDSO As Long)
        Dim ds As New DataSet
        Try
            If Not RequestAdaDiSPK(IDSO) Then
                EksekusiSQL("UPDATE MRequestMutasiWilayah SET IsAccPusat=0, IDUserAcc=NULL, TanggalAcc=NULL, JamAcc=NULL WHERE NoID=" & IDSO)
            End If
        Catch ex As Exception
            DevExpress.XtraEditors.XtraMessageBox.Show("Kesalahan perhitugan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
        Finally
            ds.Dispose()
        End Try
    End Sub
    Public Shared Sub PostingSPKWilayah(ByVal IDSO As Long)
        Dim ds As New DataSet
        Dim SQL As String = ""
        Try
            ds = modSqlServer.ExecuteDataset("MSPK", "SELECT MSPKMutasiWilayahD.*, MSPKMutasiWilayah.IsPosted, MSPKMutasiWilayah.Tanggal FROM MSPKMutasiWilayahD LEFT JOIN MSPKMutasiWilayah ON MSPKMutasiWilayah.NoID=MSPKMutasiWilayahD.IDHeader WHERE MSPKMutasiWilayah.NoID=" & IDSO)
            If ds.Tables("MSPK").Rows.Count >= 1 Then
                If Not NullToBool(ds.Tables("MSPK").Rows(0).Item("IsPosted")) Then
                    EksekusiSQL("UPDATE MSPKMutasiWilayah SET IsPosted=1, IDUserPosting=" & IDUserAktif & ", TglPosting=getdate() WHERE NoID=" & IDSO)
                End If
            End If
        Catch ex As Exception
            DevExpress.XtraEditors.XtraMessageBox.Show("Kesalahan perhitugan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
        Finally
            ds.Dispose()
        End Try
    End Sub
    Public Shared Sub UnPostingSPKWilayah(ByVal IDSO As Long)
        Dim ds As New DataSet
        Try
            If Not SPKAdaDiPacking(IDSO) Then
                EksekusiSQL("UPDATE MSPKMutasiWilayah SET IsPosted=0, IDUserPosting=NULL, TglPosting=NULL WHERE NoID=" & IDSO)
            End If
        Catch ex As Exception
            DevExpress.XtraEditors.XtraMessageBox.Show("Kesalahan perhitugan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
        Finally
            ds.Dispose()
        End Try
    End Sub
    'Public Shared Sub PostingPackingWilayah(ByVal IDSO As Long)
    '    Dim ds As New DataSet
    '    Dim SQL As String = ""
    '    Try
    '        ds = modSqlServer.ExecuteDataset("MPacking", "SELECT MPackingMutasiWilayahD.*, MPackingMutasiWilayah.IsPosted, MPackingMutasiWilayah.Tanggal FROM MPackingMutasiWilayahD LEFT JOIN MPackingMutasiWilayah ON MPackingMutasiWilayah.NoID=MPackingMutasiWilayahD.IDHeader WHERE MPackingMutasiWilayah.NoID=" & IDSO)
    '        If ds.Tables("MPacking").Rows.Count >= 1 Then
    '            If Not NullToBool(ds.Tables("MPacking").Rows(0).Item("IsPosted")) Then
    '                EksekusiSQL("UPDATE MPackingMutasiWilayah SET IsPosted=1, IDUserPosting=" & IDUserAktif & ", TglPosting=getdate() WHERE NoID=" & IDSO)
    '            End If
    '        End If
    '    Catch ex As Exception
    '        DevExpress.XtraEditors.XtraMessageBox.Show("Kesalahan perhitugan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
    '    Finally
    '        ds.Dispose()
    '    End Try
    'End Sub
    'Public Shared Sub UnPostingPackingWilayah(ByVal IDSO As Long)
    '    Dim ds As New DataSet
    '    Try
    '        If Not PackingAdaDiTransferOut(IDSO) Then
    '            EksekusiSQL("UPDATE MPackingMutasiWilayah SET IsPosted=0, IDUserPosting=NULL, TglPosting=NULL WHERE NoID=" & IDSO)
    '        End If
    '    Catch ex As Exception
    '        DevExpress.XtraEditors.XtraMessageBox.Show("Kesalahan perhitugan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
    '    Finally
    '        ds.Dispose()
    '    End Try
    'End Sub
    Public Shared Sub PostingTransferOut(ByVal IDMutasi As Long)
        Dim ds As New DataSet
        Dim SQL As String = ""
        Dim SaldoStockAkhir As Double = 0.0
        Dim HPPPerItem As Double = 0.0
        Try
            ds = modSqlServer.ExecuteDataset("MMutasiGudangD", "SELECT MTransferOutD.*, MTransferOut.IsPosted, MTransferOut.Tanggal AS TGLMutasi, MTransferOut.IDGudangIntransit AS IDGudangTujuan, MTransferOutD.IDGudangDari AS IDGudangAsal FROM MTransferOutD LEFT JOIN MTransferOut ON MTransferOut.NoID=MTransferOutD.IDHeader WHERE MTransferOut.NoID=" & IDMutasi)
            If ds.Tables("MMutasiGudangD").Rows.Count >= 1 Then
                If Not NullToBool(ds.Tables("MMutasiGudangD").Rows(0).Item("IsPosted")) Then
                    EksekusiSQL("DELETE FROM MKartuStok WHERE IDTransaksi=" & IDMutasi & " AND IDJenisTransaksi=12")
                    For i As Integer = 0 To ds.Tables("MMutasiGudangD").Rows.Count - 1
                        'Keluarkan
                        SaldoStockAkhir = 0 'GetStockAkhirGudang(NullToLong(ds.Tables("MMutasiGudangD").Rows(i).Item("IDBarang")), NullToLong(ds.Tables("MMutasiGudangD").Rows(i).Item("IDGudangAsal")), NullToDate(ds.Tables("MMutasiGudangD").Rows(i).Item("TGLMutasi")))
                        'HPPPerItem = GetHPP(NullTolong(ds.Tables("MMutasiGudangD").Rows(i).Item("IDBarang")), NullToDate(ds.Tables("MMutasiGudangD").Rows(i).Item("TGLMutasi")))
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
                        SQL &= " SELECT " & GetNewID("MKartuStok", "NoID") & " AS NoID, MTransferOut.Kode, MTransferOutD.IDBarang, "
                        SQL &= " MWilayah.IDDepartemen, MTransferOutD.IDGudangDari, "
                        SQL &= " MTransferOut.Tanggal, 12 AS IDJenisTransaksi,  MTransferOut.NoID AS IDTransaksi, "
                        SQL &= " MTransferOutD.NoID AS IDTransaksiDetil, MTransferOutD.Keterangan, (ISNULL(MTransferOutD.Qty,0)*ISNULL(MTransferOutD.Konversi,0)) AS KeluarA, MTransferOutD.Qty AS Keluar,"
                        SQL &= " 0 AS HrgJualPerItem, 0 AS Jumlah, 0 AS Masuk,0 AS MasukA,"
                        SQL &= " 0,0,0 AS HPPKeluar, "
                        SQL &= " 0 AS JumHPPKeluar, " & Replace(SaldoStockAkhir, ",", ".") & "-(ISNULL(MTransferOutD.Qty,0)*ISNULL(MTransferOutD.Konversi,0)) as QtyAkhir, 0,"
                        SQL &= " 0, MBarang.IDJenis, MTransferOutD.IDSatuan, "
                        SQL &= " isnull(MTransferOutD.Konversi,0), 0 AS IDAlamat "
                        SQL &= " FROM MTransferOutD LEFT JOIN MTransferOut ON MTransferOut.NoID=MTransferOutD.IDHeader "
                        SQL &= " LEFT JOIN (MGudang LEFT JOIN MWilayah ON MWilayah.NoID=MGudang.IDWilayah) ON MTransferOutD.IDGudangDari=MGudang.NoID"
                        SQL &= " LEFT JOIN MBarang ON MBarang.NoID=MTransferOutD.IDBarang"
                        SQL &= " WHERE MTransferOutD.NoID=" & NullToLong(ds.Tables("MMutasiGudangD").Rows(i).Item("NoID"))
                        EksekusiSQL(SQL)

                        'Masukkan
                        SaldoStockAkhir = 0 'GetStockAkhirGudang(NullToLong(ds.Tables("MMutasiGudangD").Rows(i).Item("IDBarang")), NullToLong(ds.Tables("MMutasiGudangD").Rows(i).Item("IDGudangTujuan")), NullToDate(ds.Tables("MMutasiGudangD").Rows(i).Item("TGLMutasi")))
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
                        SQL &= " SELECT " & GetNewID("MKartuStok", "NoID") & " AS NoID, MTransferOut.Kode, MTransferOutD.IDBarang, "
                        SQL &= " MWilayah.IDDepartemen, MTransferOut.IDGudangIntransit, "
                        SQL &= " MTransferOut.Tanggal, 12 AS IDJenisTransaksi,  MTransferOut.NoID AS IDTransaksi, "
                        SQL &= " MTransferOutD.NoID AS IDTransaksiDetil, MTransferOutD.Keterangan, "
                        SQL &= " ISNULL(MTransferOutD.Qty,0)*ISNULL(MTransferOutD.Konversi,0) AS MasukA, MTransferOutD.Qty AS Masuk, 0 AS HrgBeliPerItem, 0 AS JumlahBeli, "
                        SQL &= " 0 AS KeluarA,0 AS Keluar, 0 AS HrgJualPerItem, 0 AS JumlahJual, "
                        SQL &= " 0 AS HPPKeluar, 0 AS JumHPPKeluar, "
                        SQL &= Replace(SaldoStockAkhir, ",", ".") & "+(ISNULL(MTransferOutD.Qty,0)*ISNULL(MTransferOutD.Konversi,0)) as QtyAkhir, 0,"
                        SQL &= " 0, MBarang.IDJenis, MTransferOutD.IDSatuan, "
                        SQL &= " MTransferOutD.Konversi, 0 AS IDAlamat "
                        SQL &= " FROM MTransferOutD LEFT JOIN MTransferOut ON MTransferOut.NoID=MTransferOutD.IDHeader"
                        SQL &= " LEFT JOIN (MGudang LEFT JOIN MWilayah ON MWilayah.NoID=MGudang.IDWilayah) ON MTransferOut.IDGudangIntransit=MGudang.NoID"
                        SQL &= " LEFT JOIN MBarang ON MBarang.NoID=MTransferOutD.IDBarang"
                        SQL &= " WHERE MTransferOutD.NoID=" & NullToLong(ds.Tables("MMutasiGudangD").Rows(i).Item("NoID"))
                        EksekusiSQL(SQL)
                    Next

                    EksekusiSQL("UPDATE MTransferOut SET IsPosted=1, IDUSerPosting=" & IDUserAktif & ", TglPosting=getdate() WHERE NoID=" & IDMutasi)
                End If
            End If
        Catch ex As Exception
            DevExpress.XtraEditors.XtraMessageBox.Show("Kesalahan perhitugan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
        Finally
            ds.Dispose()
        End Try
    End Sub
    Public Shared Sub UnPostingTransferOut(ByVal IDMutasiGudang As Long)
        Dim ds As New DataSet
        Try
            ds = ExecuteDataset("NamaTabel", "SELECT MKartuStok.* FROM MKartuStok LEFT JOIN MTransferOut ON MTransferOut.NoID=MKartuStok.IDTransaksi WHERE (MKartuStok.IsPosted=0 OR MKartuStok.IsPosted Is Null) AND MTransferOut.NoID=" & IDMutasiGudang & " AND MKartuStok.IDJenisTransaksi=12 ")
            If ds.Tables(0).Rows.Count >= 1 AndAlso Not TOAdaDiTI(IDMutasiGudang) Then
                If Not NullToBool(ds.Tables(0).Rows(0).Item("IsPosted")) Then
                    EksekusiSQL("DELETE FROM MKartuStok WHERE IDTransaksi=" & IDMutasiGudang & " AND IDJenisTransaksi=12 ")
                    EksekusiSQL("UPDATE MTransferOut SET IsPosted=0, IDUserPosting=NULL, TglPosting=NULL WHERE NoID=" & IDMutasiGudang)
                End If
            ElseIf ds.Tables(0).Rows.Count = 0 Then
                EksekusiSQL("UPDATE MTransferOut SET IsPosted=0, IDUserPosting=NULL, TglPosting=NULL WHERE NoID=" & IDMutasiGudang)
            End If
        Catch ex As Exception
            DevExpress.XtraEditors.XtraMessageBox.Show("Kesalahan perhitugan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
        Finally
            ds.Dispose()
        End Try
    End Sub
    Public Shared Sub PostingPackingWilayah(ByVal IDMutasi As Long)
        Dim ds As New DataSet
        Dim SQL As String = ""
        Dim SaldoStockAkhir As Double = 0.0
        Dim HPPPerItem As Double = 0.0
        Try
            ds = modSqlServer.ExecuteDataset("MMutasiGudangD", "SELECT MPackingMutasiWilayahD.*, MPackingMutasiWilayah.IsPosted, MPackingMutasiWilayah.Tanggal AS TGLMutasi, MPackingMutasiWilayah.IDGudangDari AS IDGudangAsal, MWilayah.IDGudangTampungan AS IDGudangTujuan FROM (MPackingMutasiWilayahD LEFT JOIN MPackingMutasiWilayah ON MPackingMutasiWilayah.NoID=MPackingMutasiWilayahD.IDHeader) LEFT JOIN MWilayah ON MWilayah.NoID=MPackingMutasiWilayah.IDWilayahDari WHERE MPackingMutasiWilayah.NoID=" & IDMutasi)
            If ds.Tables("MMutasiGudangD").Rows.Count >= 1 Then
                If Not NullToBool(ds.Tables("MMutasiGudangD").Rows(0).Item("IsPosted")) Then
                    EksekusiSQL("DELETE FROM MKartuStok WHERE IDTransaksi=" & IDMutasi & " AND (IDJenisTransaksi=5 OR IDJenisTransaksi=4)")
                    For i As Integer = 0 To ds.Tables("MMutasiGudangD").Rows.Count - 1
                        'Keluarkan
                        SaldoStockAkhir = 0 'GetStockAkhirGudang(NullToLong(ds.Tables("MMutasiGudangD").Rows(i).Item("IDBarang")), NullToLong(ds.Tables("MMutasiGudangD").Rows(i).Item("IDGudangAsal")), NullToDate(ds.Tables("MMutasiGudangD").Rows(i).Item("TGLMutasi")))
                        'HPPPerItem = GetHPP(NullTolong(ds.Tables("MMutasiGudangD").Rows(i).Item("IDBarang")), NullToDate(ds.Tables("MMutasiGudangD").Rows(i).Item("TGLMutasi")))
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
                        SQL &= " SELECT " & GetNewID("MKartuStok", "NoID") & " AS NoID, MPackingMutasiWilayah.Kode, MPackingMutasiWilayahD.IDBarang, "
                        SQL &= " MPackingMutasiWilayah.IDDepartemen, MPackingMutasiWilayah.IDGudangDari, "
                        SQL &= " MPackingMutasiWilayah.Tanggal, 17 AS IDJenisTransaksi,  MPackingMutasiWilayah.NoID AS IDTransaksi, "
                        SQL &= " MPackingMutasiWilayahD.NoID AS IDTransaksiDetil, MPackingMutasiWilayahD.Keterangan, (ISNULL(MPackingMutasiWilayahD.Qty,0)*ISNULL(MPackingMutasiWilayahD.Konversi,0)) AS KeluarA, MPackingMutasiWilayahD.Qty AS Keluar,"
                        SQL &= " 0 AS HrgJualPerItem, 0 AS Jumlah, 0 AS Masuk,0 AS MasukA,"
                        SQL &= " 0,0,0 AS HPPKeluar, "
                        SQL &= " 0 AS JumHPPKeluar, " & Replace(SaldoStockAkhir, ",", ".") & "-(ISNULL(MPackingMutasiWilayahD.Qty,0)*ISNULL(MPackingMutasiWilayahD.Konversi,0)) as QtyAkhir, 0,"
                        SQL &= " 0, MBarang.IDJenis, MPackingMutasiWilayahD.IDSatuan, "
                        SQL &= " isnull(MPackingMutasiWilayahD.Konversi,0), 0 AS IDAlamat "
                        SQL &= " FROM MPackingMutasiWilayahD LEFT JOIN MPackingMutasiWilayah ON MPackingMutasiWilayah.NoID=MPackingMutasiWilayahD.IDHeader"
                        SQL &= " LEFT JOIN (MGudang LEFT JOIN MWilayah ON MWilayah.NoID=MGudang.IDWilayah) ON MPackingMutasiWilayah.IDGudangDari=MGudang.NoID"
                        SQL &= " LEFT JOIN MBarang ON MBarang.NoID=MPackingMutasiWilayahD.IDBarang"
                        SQL &= " WHERE MPackingMutasiWilayahD.NoID=" & NullToLong(ds.Tables("MMutasiGudangD").Rows(i).Item("NoID"))
                        EksekusiSQL(SQL)

                        'Masukkan
                        SaldoStockAkhir = 0 'GetStockAkhirGudang(NullToLong(ds.Tables("MMutasiGudangD").Rows(i).Item("IDBarang")), NullToLong(ds.Tables("MMutasiGudangD").Rows(i).Item("IDGudangTujuan")), NullToDate(ds.Tables("MMutasiGudangD").Rows(i).Item("TGLMutasi")))
                        'HPPPerItem = GetHPP(NullTolong(ds.Tables("MMutasiGudangD").Rows(i).Item("IDBarang")), NullToDate(ds.Tables("MMutasiGudangD").Rows(i).Item("TGLMutasi")))
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
                        SQL &= " SELECT " & GetNewID("MKartuStok", "NoID") & " AS NoID, MPackingMutasiWilayah.Kode, MPackingMutasiWilayahD.IDBarang, "
                        SQL &= " MPackingMutasiWilayah.IDDepartemen, MWilayah.IDGudangTampungan, "
                        SQL &= " MPackingMutasiWilayah.Tanggal, 18 AS IDJenisTransaksi,  MPackingMutasiWilayah.NoID AS IDTransaksi, "
                        SQL &= " MPackingMutasiWilayahD.NoID AS IDTransaksiDetil, MPackingMutasiWilayahD.Keterangan, (ISNULL(MPackingMutasiWilayahD.Qty,0)*ISNULL(MPackingMutasiWilayahD.Konversi,0)) AS KeluarA, MPackingMutasiWilayahD.Qty AS Keluar,"
                        SQL &= " 0 AS HrgJualPerItem, 0 AS Jumlah, 0 AS Masuk,0 AS MasukA,"
                        SQL &= " 0,0,0 AS HPPKeluar, "
                        SQL &= " 0 AS JumHPPKeluar, " & Replace(SaldoStockAkhir, ",", ".") & "-(ISNULL(MPackingMutasiWilayahD.Qty,0)*ISNULL(MPackingMutasiWilayahD.Konversi,0)) as QtyAkhir, 0,"
                        SQL &= " 0, MBarang.IDJenis, MPackingMutasiWilayahD.IDSatuan, "
                        SQL &= " isnull(MPackingMutasiWilayahD.Konversi,0), 0 AS IDAlamat "
                        SQL &= " FROM MPackingMutasiWilayahD LEFT JOIN MPackingMutasiWilayah ON MPackingMutasiWilayah.NoID=MPackingMutasiWilayahD.IDHeader"
                        SQL &= " LEFT JOIN (MGudang LEFT JOIN MWilayah ON MWilayah.NoID=MGudang.IDWilayah) ON MPackingMutasiWilayah.IDGudangDari=MGudang.NoID"
                        SQL &= " LEFT JOIN MBarang ON MBarang.NoID=MPackingMutasiWilayahD.IDBarang"
                        SQL &= " WHERE MPackingMutasiWilayahD.NoID=" & NullToLong(ds.Tables("MMutasiGudangD").Rows(i).Item("NoID"))
                        EksekusiSQL(SQL)
                    Next
                    EksekusiSQL("UPDATE MPackingMutasiWilayah SET IsPosted=1 WHERE NoID=" & IDMutasi)
                End If
            End If
        Catch ex As Exception
            DevExpress.XtraEditors.XtraMessageBox.Show("Kesalahan perhitugan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
        Finally
            ds.Dispose()
        End Try
    End Sub
    Public Shared Sub UnPostingPackingWilayah(ByVal IDMutasiGudang As Long)
        Dim ds As New DataSet
        Try
            ds = ExecuteDataset("NamaTabel", "SELECT MKartuStok.* FROM MKartuStok LEFT JOIN MPackingMutasiWilayah ON MPackingMutasiWilayah.NoID=MKartuStok.IDTransaksi WHERE (MKartuStok.IsPosted=0 OR MKartuStok.IsPosted Is Null) AND MPackingMutasiWilayah.NoID=" & IDMutasiGudang & " AND (MKartuStok.IDJenisTransaksi=17 OR MKartuStok.IDJenisTransaksi=18)")
            If ds.Tables(0).Rows.Count >= 1 Then
                If Not NullToBool(ds.Tables(0).Rows(0).Item("IsPosted")) Then
                    EksekusiSQL("DELETE FROM MKartuStok WHERE IDTransaksi=" & IDMutasiGudang & " AND (IDJenisTransaksi=17 OR IDJenisTransaksi=18)")
                    EksekusiSQL("UPDATE MPackingMutasiWilayah SET IsPosted=0 WHERE NoID=" & IDMutasiGudang)
                End If
            ElseIf ds.Tables(0).Rows.Count = 0 Then
                EksekusiSQL("UPDATE MPackingMutasiWilayah SET IsPosted=0 WHERE NoID=" & IDMutasiGudang)
            End If
        Catch ex As Exception
            DevExpress.XtraEditors.XtraMessageBox.Show("Kesalahan perhitugan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
        Finally
            ds.Dispose()
        End Try
    End Sub
    Public Shared Sub PostingTransferIN(ByVal IDMutasi As Long)
        Dim ds As New DataSet
        Dim SQL As String = ""
        Dim SaldoStockAkhir As Double = 0.0
        Dim HPPPerItem As Double = 0.0
        Try
            ds = modSqlServer.ExecuteDataset("MMutasiGudangD", "SELECT MTransferIND.*, MTransferIN.IsPosted, MTransferIN.Tanggal AS TGLMutasi, MTransferIN.IDGudangIntransit AS IDGudangAsal, MTransferIND.IDGudang AS IDGudangTujuan FROM MTransferIND LEFT JOIN MTransferIN ON MTransferIN.NoID=MTransferIND.IDHeader WHERE MTransferIN.NoID=" & IDMutasi)
            If ds.Tables("MMutasiGudangD").Rows.Count >= 1 Then
                If Not NullToBool(ds.Tables("MMutasiGudangD").Rows(0).Item("IsPosted")) Then
                    EksekusiSQL("DELETE FROM MKartuStok WHERE IDTransaksi=" & IDMutasi & " AND IDJenisTransaksi=13")
                    For i As Integer = 0 To ds.Tables("MMutasiGudangD").Rows.Count - 1
                        'Keluarkan
                        SaldoStockAkhir = 0 'GetStockAkhirGudang(NullToLong(ds.Tables("MMutasiGudangD").Rows(i).Item("IDBarang")), NullToLong(ds.Tables("MMutasiGudangD").Rows(i).Item("IDGudangAsal")), NullToDate(ds.Tables("MMutasiGudangD").Rows(i).Item("TGLMutasi")))
                        'HPPPerItem = GetHPP(NullTolong(ds.Tables("MMutasiGudangD").Rows(i).Item("IDBarang")), NullToDate(ds.Tables("MMutasiGudangD").Rows(i).Item("TGLMutasi")))
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
                        SQL &= " SELECT " & GetNewID("MKartuStok", "NoID") & " AS NoID, MTransferIN.Kode, MTransferIND.IDBarang, "
                        SQL &= " MWilayah.IDDepartemen, MTransferIN.IDGudangIntransit, "
                        SQL &= " MTransferIN.Tanggal, 13 AS IDJenisTransaksi,  MTransferIN.NoID AS IDTransaksi, "
                        SQL &= " MTransferIND.NoID AS IDTransaksiDetil, MTransferIND.Keterangan, (ISNULL(MTransferIND.Qty,0)*ISNULL(MTransferIND.Konversi,0)) AS KeluarA, MTransferIND.Qty AS Keluar,"
                        SQL &= " 0 AS HrgJualPerItem, 0 AS Jumlah, 0 AS Masuk,0 AS MasukA,"
                        SQL &= " 0,0,0 AS HPPKeluar, "
                        SQL &= " 0 AS JumHPPKeluar, " & Replace(SaldoStockAkhir, ",", ".") & "-(ISNULL(MTransferIND.Qty,0)*ISNULL(MTransferIND.Konversi,0)) as QtyAkhir, 0,"
                        SQL &= " 0, MBarang.IDJenis, MTransferIND.IDSatuan, "
                        SQL &= " isnull(MTransferIND.Konversi,0), 0 AS IDAlamat "
                        SQL &= " FROM MTransferIND LEFT JOIN MTransferIN ON MTransferIN.NoID=MTransferIND.IDHeader "
                        SQL &= " LEFT JOIN (MGudang LEFT JOIN MWilayah ON MWilayah.NoID=MGudang.IDWilayah) ON MTransferIN.IDGudangIntransit=MGudang.NoID"
                        SQL &= " LEFT JOIN MBarang ON MBarang.NoID=MTransferIND.IDBarang"
                        SQL &= " WHERE MTransferIND.NoID=" & NullToLong(ds.Tables("MMutasiGudangD").Rows(i).Item("NoID"))
                        EksekusiSQL(SQL)

                        'Masukkan
                        SaldoStockAkhir = 0 'GetStockAkhirGudang(NullToLong(ds.Tables("MMutasiGudangD").Rows(i).Item("IDBarang")), NullToLong(ds.Tables("MMutasiGudangD").Rows(i).Item("IDGudangTujuan")), NullToDate(ds.Tables("MMutasiGudangD").Rows(i).Item("TGLMutasi")))
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
                        SQL &= " SELECT " & GetNewID("MKartuStok", "NoID") & " AS NoID, MTransferIN.Kode, MTransferIND.IDBarang, "
                        SQL &= " MWilayah.IDDepartemen, MTransferIND.IDGudang, "
                        SQL &= " MTransferIN.Tanggal, 13 AS IDJenisTransaksi,  MTransferIN.NoID AS IDTransaksi, "
                        SQL &= " MTransferIND.NoID AS IDTransaksiDetil, MTransferIND.Keterangan, "
                        SQL &= " ISNULL(MTransferIND.Qty,0)*ISNULL(MTransferIND.Konversi,0) AS MasukA, MTransferIND.Qty AS Masuk, 0 AS HrgBeliPerItem, 0 AS JumlahBeli, "
                        SQL &= " 0 AS KeluarA,0 AS Keluar, 0 AS HrgJualPerItem, 0 AS JumlahJual, "
                        SQL &= " 0 AS HPPKeluar, 0 AS JumHPPKeluar, "
                        SQL &= Replace(SaldoStockAkhir, ",", ".") & "+(ISNULL(MTransferIND.Qty,0)*ISNULL(MTransferIND.Konversi,0)) as QtyAkhir, 0,"
                        SQL &= " 0, MBarang.IDJenis, MTransferIND.IDSatuan, "
                        SQL &= " MTransferIND.Konversi, 0 AS IDAlamat "
                        SQL &= " FROM MTransferIND LEFT JOIN MTransferIN ON MTransferIN.NoID=MTransferIND.IDHeader"
                        SQL &= " LEFT JOIN (MGudang LEFT JOIN MWilayah ON MWilayah.NoID=MGudang.IDWilayah) ON MTransferIND.IDGudang=MGudang.NoID"
                        SQL &= " LEFT JOIN MBarang ON MBarang.NoID=MTransferIND.IDBarang"
                        SQL &= " WHERE MTransferIND.NoID=" & NullToLong(ds.Tables("MMutasiGudangD").Rows(i).Item("NoID"))
                        EksekusiSQL(SQL)
                    Next

                    EksekusiSQL("UPDATE MTransferIN SET IsPosted=1, IDUSerPosting=" & IDUserAktif & ", TglPosting=getdate() WHERE NoID=" & IDMutasi)
                End If
            End If
        Catch ex As Exception
            DevExpress.XtraEditors.XtraMessageBox.Show("Kesalahan perhitugan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
        Finally
            ds.Dispose()
        End Try
    End Sub
    Public Shared Sub UnPostingTransferIN(ByVal IDMutasiGudang As Long)
        Dim ds As New DataSet
        Try
            ds = ExecuteDataset("NamaTabel", "SELECT MKartuStok.* FROM MKartuStok LEFT JOIN MTransferIN ON MTransferIN.NoID=MKartuStok.IDTransaksi WHERE (MKartuStok.IsPosted=0 OR MKartuStok.IsPosted Is Null) AND MTransferIN.NoID=" & IDMutasiGudang & " AND MKartuStok.IDJenisTransaksi=13 ")
            If ds.Tables(0).Rows.Count >= 1 Then
                If Not NullToBool(ds.Tables(0).Rows(0).Item("IsPosted")) Then
                    EksekusiSQL("DELETE FROM MKartuStok WHERE IDTransaksi=" & IDMutasiGudang & " AND IDJenisTransaksi=13 ")
                    EksekusiSQL("UPDATE MTransferIN SET IsPosted=0, IDUserPosting=NULL, TglPosting=NULL WHERE NoID=" & IDMutasiGudang)
                End If
            ElseIf ds.Tables(0).Rows.Count = 0 Then
                EksekusiSQL("UPDATE MTransferIN SET IsPosted=0, IDUserPosting=NULL, TglPosting=NULL WHERE NoID=" & IDMutasiGudang)
            End If
        Catch ex As Exception
            DevExpress.XtraEditors.XtraMessageBox.Show("Kesalahan perhitugan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
        Finally
            ds.Dispose()
        End Try
    End Sub
    Public Shared Sub PostingPenyesuaian(ByVal IDMutasi As Long)
        Dim ds As New DataSet
        Dim SQL As String = ""
        Dim SaldoStockAkhir As Double = 0.0
        Dim HPPPerItem As Double = 0.0
        Try
            ds = modSqlServer.ExecuteDataset("MMutasiGudangD", "SELECT MPenyesuaianD.*, MPenyesuaian.IsPosted, MPenyesuaian.Tanggal AS TGLMutasi, MPenyesuaianD.IDGudang FROM MPenyesuaianD LEFT JOIN MPenyesuaian ON MPenyesuaian.NoID=MPenyesuaianD.IDHeader WHERE MPenyesuaian.NoID=" & IDMutasi)
            If ds.Tables("MMutasiGudangD").Rows.Count >= 1 Then
                If Not NullToBool(ds.Tables("MMutasiGudangD").Rows(0).Item("IsPosted")) Then
                    EksekusiSQL("DELETE FROM MKartuStok WHERE IDTransaksi=" & IDMutasi & " AND IDJenisTransaksi=14")
                    For i As Integer = 0 To ds.Tables("MMutasiGudangD").Rows.Count - 1
                        '- Keluarkan, + Masuk
                        SaldoStockAkhir = 0 'GetStockAkhirGudang(NullToLong(ds.Tables("MMutasiGudangD").Rows(i).Item("IDBarang")), NullToLong(ds.Tables("MMutasiGudangD").Rows(i).Item("IDGudang")), NullToDate(ds.Tables("MMutasiGudangD").Rows(i).Item("TGLMutasi")))
                        'HPPPerItem = GetHPP(NullTolong(ds.Tables("MMutasiGudangD").Rows(i).Item("IDBarang")), NullToDate(ds.Tables("MMutasiGudangD").Rows(i).Item("TGLMutasi")))
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
                        If NullToDbl(ds.Tables("MMutasiGudangD").Rows(i).Item("Qty")) < 0 Then
                            SQL &= " ,[QtyKeluarA]"
                            SQL &= " ,[QtyKeluar]"
                        Else
                            SQL &= " ,[QtyMasukA]"
                            SQL &= " ,[QtyMasuk]"
                        End If
                        SQL &= " ,[HargaJualPerItem]"
                        SQL &= " ,[JumlahJual]"
                        If NullToDbl(ds.Tables("MMutasiGudangD").Rows(i).Item("Qty")) < 0 Then
                            SQL &= " ,[QtyMasukA]"
                            SQL &= " ,[QtyMasuk]"
                        Else
                            SQL &= " ,[QtyKeluarA]"
                            SQL &= " ,[QtyKeluar]"
                        End If
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
                        SQL &= " ,[IDAlamat],[IDBarangD])" & vbCrLf
                        SQL &= " SELECT " & GetNewID("MKartuStok", "NoID") & " AS NoID, MPenyesuaian.Kode, MPenyesuaianD.IDBarang, "
                        SQL &= " MWilayah.IDDepartemen, MPenyesuaianD.IDGudang, "
                        SQL &= " MPenyesuaian.Tanggal, 14 AS IDJenisTransaksi,  MPenyesuaian.NoID AS IDTransaksi, "
                        SQL &= " MPenyesuaianD.NoID AS IDTransaksiDetil, MPenyesuaianD.Keterangan, (ISNULL(ABS(MPenyesuaianD.Qty),0)*ISNULL(MPenyesuaianD.Konversi,0)) AS KeluarA, ABS(MPenyesuaianD.Qty) AS Keluar,"
                        SQL &= " 0 AS HrgJualPerItem, 0 AS Jumlah, 0 AS Masuk,0 AS MasukA,"
                        SQL &= " 0,0,0 AS HPPKeluar, "
                        SQL &= " 0 AS JumHPPKeluar, " & Replace(SaldoStockAkhir, ",", ".") & "-(ISNULL(MPenyesuaianD.Qty,0)*ISNULL(MPenyesuaianD.Konversi,0)) as QtyAkhir, 0,"
                        SQL &= " 0, MBarang.IDJenis, MPenyesuaianD.IDSatuan, "
                        SQL &= " isnull(MPenyesuaianD.Konversi,0), 0 AS IDAlamat, MPenyesuaianD.IDBarangD "
                        SQL &= " FROM MPenyesuaianD LEFT JOIN MPenyesuaian ON MPenyesuaian.NoID=MPenyesuaianD.IDHeader "
                        SQL &= " LEFT JOIN (MGudang LEFT JOIN MWilayah ON MWilayah.NoID=MGudang.IDWilayah) ON MPenyesuaianD.IDGudang=MGudang.NoID"
                        SQL &= " LEFT JOIN MBarang ON MBarang.NoID=MPenyesuaianD.IDBarang"
                        SQL &= " WHERE MPenyesuaianD.NoID=" & NullToLong(ds.Tables("MMutasiGudangD").Rows(i).Item("NoID"))
                        EksekusiSQL(SQL)
                    Next

                    EksekusiSQL("UPDATE MPenyesuaian SET IsPosted=1, IDUSerPosting=" & IDUserAktif & ", TglPosting=getdate() WHERE NoID=" & IDMutasi)
                End If
            End If
        Catch ex As Exception
            DevExpress.XtraEditors.XtraMessageBox.Show("Kesalahan perhitugan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
        Finally
            ds.Dispose()
        End Try
    End Sub
    Public Shared Sub PostingTransferKode(ByVal IDTransaksi As Long)
        Dim ds As New DataSet
        Dim SQL As String = ""
        Dim SaldoStockAkhir As Double = 0.0
        Dim HPPPerItem As Double = 0.0
        Dim IDKartuStok As Long = -1
        Dim HPP As Double = 0.0

        Try
            ds = modSqlServer.ExecuteDataset("MTransferKodeD", "SELECT MTransferKodeD.*, MTransferKode.IsPosted, MTransferKode.Tanggal AS TGLMutasi, MTransferKodeD.IDGudangLama, MTransferKodeD.IDGudangBaru, MTransferKodeD.HargaModal FROM MTransferKodeD LEFT JOIN MTransferKode ON MTransferKode.NoID=MTransferKodeD.IDHeader WHERE MTransferKode.NoID=" & IDTransaksi)
            If ds.Tables("MTransferKodeD").Rows.Count >= 1 Then
                If Not NullToBool(ds.Tables("MTransferKodeD").Rows(0).Item("IsPosted")) Then
                    EksekusiSQL("DELETE FROM MKartuStok WHERE IDTransaksi=" & IDTransaksi & " AND IDJenisTransaksi=16")
                    For i As Integer = 0 To ds.Tables("MTransferKodeD").Rows.Count - 1
                        'OUT
                        SaldoStockAkhir = 0 'GetStockAkhirGudang(NullToLong(ds.Tables("MTransferKodeD").Rows(i).Item("IDBarangLama")), NullToLong(ds.Tables("MTransferKodeD").Rows(i).Item("IDGudangLama")), NullToDate(ds.Tables("MTransferKodeD").Rows(i).Item("TGLMutasi")))
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
                        SQL &= " SELECT " & IDKartuStok & " AS NoID, MTransferKode.Kode, MTransferKodeD.IDBarangLama, "
                        SQL &= " MWilayah.IDDepartemen, MTransferKodeD.IDGudangLama, "
                        SQL &= " MTransferKode.Tanggal, 16 AS IDJenisTransaksi,  MTransferKode.NoID AS IDTransaksi, "
                        SQL &= " MTransferKodeD.NoID AS IDTransaksiDetil, MTransferKodeD.Keterangan, ISNULL(MTransferKodeD.QtyLama,0)*IsNull(MTransferKodeD.KonversiLama,0) AS Masuk, MTransferKodeD.QtyLama AS MasukA, "
                        SQL &= " MTransferKodeD.HargaModal AS HrgBeliPerItem, ISNULL(MTransferKodeD.HargaModal*MTransferKodeD.QtyLama,0) AS JumlahBeli, 0 AS Keluar, 0 AS KeluarA, "
                        SQL &= " 0,0,0 AS HPPKeluar, "
                        SQL &= " 0 AS JumHPPKeluar, " & Replace(SaldoStockAkhir, ",", ".") & "+(ISNULL(MTransferKodeD.QtyLama,0)*IsNull(MTransferKodeD.KonversiLama,0)) as QtyAkhir, 0,"
                        SQL &= " 0, MBarang.IDJenis, MTransferKodeD.IDSatuanLama, "
                        SQL &= " IsNull(MTransferKodeD.KonversiLama,0), " & IDUserAktif & ", MTransferKodeD.CtnLama "
                        SQL &= " FROM MTransferKodeD INNER JOIN MTransferKode ON MTransferKode.NoID=MTransferKodeD.IDHeader LEFT JOIN (MGudang LEFT JOIN MWilayah ON MWilayah.NoID=MGudang.IDWilayah) ON MTransferKodeD.IDGudangLama=MGudang.NoID"
                        SQL &= " LEFT JOIN MBarang ON MBarang.NoID=MTransferKodeD.IDBarangLama"
                        SQL &= " WHERE MTransferKodeD.NoID=" & NullToLong(ds.Tables("MTransferKodeD").Rows(i).Item("NoID"))
                        EksekusiSQL(SQL)

                        HPP = NullToDbl(EksekusiSQlSkalarNew("SELECT HPP FROM MBarang WHERE NoID=" & NullToLong(ds.Tables(0).Rows(i).Item("IDBarangLama"))))

                        SQL = "UPDATE MBarang SET HPP=" & FixKoma(HPP) & " WHERE MBarang.NoID=" & NullToLong(ds.Tables(0).Rows(i).Item("IDBarangLama"))
                        EksekusiSQL(SQL)
                        SQL = "UPDATE MKartuStok SET HPP=" & FixKoma(HPP) & ", HargaBeli=" & FixKoma(NullToDbl(ds.Tables(0).Rows(i).Item("HargaModal"))) & " WHERE NoID=" & IDKartuStok
                        EksekusiSQL(SQL)

                        'IN
                        SaldoStockAkhir = 0 'GetStockAkhirGudang(NullToLong(ds.Tables("MTransferKodeD").Rows(i).Item("IDBarangBaru")), NullToLong(ds.Tables("MTransferKodeD").Rows(i).Item("IDGudangBaru")), NullToDate(ds.Tables("MTransferKodeD").Rows(i).Item("TGLMutasi")))
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
                        SQL &= " SELECT " & IDKartuStok & " AS NoID, MTransferKode.Kode, MTransferKodeD.IDBarangBaru, "
                        SQL &= " MWilayah.IDDepartemen, MTransferKodeD.IDGudangBaru, "
                        SQL &= " MTransferKode.Tanggal, 16 AS IDJenisTransaksi,  MTransferKode.NoID AS IDTransaksi, "
                        SQL &= " MTransferKodeD.NoID AS IDTransaksiDetil, MTransferKodeD.Keterangan, ISNULL(MTransferKodeD.QtyBaru,0)*IsNull(MTransferKodeD.KonversiBaru,0) AS Masuk, MTransferKodeD.QtyBaru AS MasukA, "
                        SQL &= " MTransferKodeD.HargaModal AS HrgBeliPerItem, ISNULL(MTransferKodeD.HargaModal*MTransferKodeD.QtyBaru,0) AS JumlahBeli, 0 AS Keluar, 0 AS KeluarA, "
                        SQL &= " 0,0,0 AS HPPKeluar, "
                        SQL &= " 0 AS JumHPPKeluar, " & Replace(SaldoStockAkhir, ",", ".") & "+(ISNULL(MTransferKodeD.QtyBaru,0)*IsNull(MTransferKodeD.KonversiBaru,0)) as QtyAkhir, 0,"
                        SQL &= " 0, MBarang.IDJenis, MTransferKodeD.IDSatuanBaru, "
                        SQL &= " IsNull(MTransferKodeD.KonversiBaru,0), " & IDUserAktif & ", MTransferKodeD.CtnBaru "
                        SQL &= " FROM MTransferKodeD INNER JOIN MTransferKode ON MTransferKode.NoID=MTransferKodeD.IDHeader LEFT JOIN (MGudang LEFT JOIN MWilayah ON MWilayah.NoID=MGudang.IDWilayah) ON MTransferKodeD.IDGudangBaru=MGudang.NoID"
                        SQL &= " LEFT JOIN MBarang ON MBarang.NoID=MTransferKodeD.IDBarangBaru"
                        SQL &= " WHERE MTransferKodeD.NoID=" & NullToLong(ds.Tables("MTransferKodeD").Rows(i).Item("NoID"))
                        EksekusiSQL(SQL)

                        HPP = NullToDbl(EksekusiSQlSkalarNew("SELECT HPP FROM MBarang WHERE NoID=" & NullToLong(ds.Tables(0).Rows(i).Item("IDBarangBaru"))))

                        SQL = "UPDATE MBarang SET HPP=" & FixKoma(HPP) & " WHERE MBarang.NoID=" & NullToLong(ds.Tables(0).Rows(i).Item("IDBarangBaru"))
                        EksekusiSQL(SQL)
                        SQL = "UPDATE MKartuStok SET HPP=" & FixKoma(HPP) & ", HargaBeli=" & FixKoma(NullToDbl(ds.Tables(0).Rows(i).Item("HargaModal"))) & " WHERE NoID=" & IDKartuStok
                        EksekusiSQL(SQL)
                    Next

                    EksekusiSQL("UPDATE MTransferKode SET IsPosted=1, IDUSerPosting=" & IDUserAktif & ", TglPosting=getdate() WHERE NoID=" & IDTransaksi)
                End If
            End If
        Catch ex As Exception
            DevExpress.XtraEditors.XtraMessageBox.Show("Kesalahan perhitugan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
        Finally
            ds.Dispose()
        End Try
    End Sub
    Public Shared Sub UnPostingTransferKode(ByVal IDTransaksi As Long)
        Dim ds As New DataSet
        Try
            ds = ExecuteDataset("NamaTabel", "SELECT MKartuStok.* FROM MKartuStok LEFT JOIN MTransferKode ON MTransferKode.NoID=MKartuStok.IDTransaksi WHERE (MKartuStok.IsPosted=0 OR MKartuStok.IsPosted Is Null) AND MTransferKode.NoID=" & IDTransaksi & " AND MKartuStok.IDJenisTransaksi=16 ")
            If ds.Tables(0).Rows.Count >= 1 Then
                If Not NullToBool(ds.Tables(0).Rows(0).Item("IsPosted")) Then
                    EksekusiSQL("DELETE FROM MKartuStok WHERE IDTransaksi=" & IDTransaksi & " AND IDJenisTransaksi=16 ")
                    EksekusiSQL("UPDATE MTransferKode SET IsPosted=0, IDUserPosting=NULL, TglPosting=NULL WHERE NoID=" & IDTransaksi)
                End If
            ElseIf ds.Tables(0).Rows.Count = 0 Then
                EksekusiSQL("UPDATE MTransferKode SET IsPosted=0, IDUserPosting=NULL, TglPosting=NULL WHERE NoID=" & IDTransaksi)
            End If
        Catch ex As Exception
            DevExpress.XtraEditors.XtraMessageBox.Show("Kesalahan perhitugan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
        Finally
            ds.Dispose()
        End Try
    End Sub
    Public Shared Sub PostingStockOpname(ByVal IDMutasi As Long)
        Dim ds As New DataSet
        Dim SQL As String = ""
        Dim SaldoStockAkhir As Double = 0.0
        Dim HPPPerItem As Double = 0.0
        Try
            ds = modSqlServer.ExecuteDataset("MMutasiGudangD", "SELECT MStockOpnameD.*, MStockOpname.TglDitetapkanSO, MStockOpname.IsPosted, MStockOpname.Tanggal AS TGLMutasi, MStockOpnameD.IDGudang FROM MStockOpnameD LEFT JOIN MStockOpname ON MStockOpname.NoID=MStockOpnameD.IDHeader WHERE MStockOpname.NoID=" & IDMutasi)
            If ds.Tables("MMutasiGudangD").Rows.Count >= 1 Then
                HitungSelisihStockOpname(IDMutasi, NullToDate(ds.Tables(0).Rows(0).Item("TglDitetapkanSO")))
                If Not NullToBool(ds.Tables("MMutasiGudangD").Rows(0).Item("IsPosted")) Then
                    EksekusiSQL("DELETE FROM MKartuStok WHERE IDTransaksi=" & IDMutasi & " AND IDJenisTransaksi=9")
                    For i As Integer = 0 To ds.Tables("MMutasiGudangD").Rows.Count - 1
                        '- Keluarkan, + Masuk
                        SaldoStockAkhir = 0  'GetStockAkhirGudang(NullToLong(ds.Tables("MMutasiGudangD").Rows(i).Item("IDBarang")), NullToLong(ds.Tables("MMutasiGudangD").Rows(i).Item("IDGudang")), NullToDate(ds.Tables("MMutasiGudangD").Rows(i).Item("TGLMutasi")))
                        'HPPPerItem = GetHPP(NullTolong(ds.Tables("MMutasiGudangD").Rows(i).Item("IDBarang")), NullToDate(ds.Tables("MMutasiGudangD").Rows(i).Item("TGLMutasi")))
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
                        If NullToDbl(ds.Tables("MMutasiGudangD").Rows(i).Item("Qty")) < 0 Then
                            SQL &= " ,[QtyKeluarA]"
                            SQL &= " ,[QtyKeluar]"
                        Else
                            SQL &= " ,[QtyMasukA]"
                            SQL &= " ,[QtyMasuk]"
                        End If
                        SQL &= " ,[HargaJualPerItem]"
                        SQL &= " ,[JumlahJual]"
                        If NullToDbl(ds.Tables("MMutasiGudangD").Rows(i).Item("Qty")) < 0 Then
                            SQL &= " ,[QtyMasukA]"
                            SQL &= " ,[QtyMasuk]"
                        Else
                            SQL &= " ,[QtyKeluarA]"
                            SQL &= " ,[QtyKeluar]"
                        End If
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
                        SQL &= " ,[IDAlamat],[IDBarangD])" & vbCrLf
                        SQL &= " SELECT " & GetNewID("MKartuStok", "NoID") & " AS NoID, MStockOpname.Kode, MStockOpnameD.IDBarang, "
                        SQL &= " MWilayah.IDDepartemen, MStockOpnameD.IDGudang, "
                        SQL &= " MStockOpname.Tanggal, 9 AS IDJenisTransaksi,  MStockOpname.NoID AS IDTransaksi, "
                        SQL &= " MStockOpnameD.NoID AS IDTransaksiDetil, MStockOpnameD.Keterangan, (ISNULL(ABS(MStockOpnameD.Qty),0)*ISNULL(MStockOpnameD.Konversi,0)) AS KeluarA, ABS(MStockOpnameD.Qty) AS Keluar,"
                        SQL &= " 0 AS HrgJualPerItem, 0 AS Jumlah, 0 AS Masuk,0 AS MasukA,"
                        SQL &= " 0,0,0 AS HPPKeluar, "
                        SQL &= " 0 AS JumHPPKeluar, " & Replace(SaldoStockAkhir, ",", ".") & "-(ISNULL(MStockOpnameD.Qty,0)*ISNULL(MStockOpnameD.Konversi,0)) as QtyAkhir, 0,"
                        SQL &= " 0, MBarang.IDJenis, MStockOpnameD.IDSatuan, "
                        SQL &= " isnull(MStockOpnameD.Konversi,0), 0 AS IDAlamat, MStockOpnameD.IDBarangD "
                        SQL &= " FROM MStockOpnameD LEFT JOIN MStockOpname ON MStockOpname.NoID=MStockOpnameD.IDHeader "
                        SQL &= " LEFT JOIN (MGudang LEFT JOIN MWilayah ON MWilayah.NoID=MGudang.IDWilayah) ON MStockOpnameD.IDGudang=MGudang.NoID"
                        SQL &= " LEFT JOIN MBarang ON MBarang.NoID=MStockOpnameD.IDBarang"
                        SQL &= " WHERE MStockOpnameD.NoID=" & NullToLong(ds.Tables("MMutasiGudangD").Rows(i).Item("NoID"))
                        EksekusiSQL(SQL)
                    Next

                    EksekusiSQL("UPDATE MStockOpname SET IsPosted=1, IDUSerPosting=" & IDUserAktif & ", TglPosting=getdate() WHERE NoID=" & IDMutasi)
                End If
            End If
        Catch ex As Exception
            DevExpress.XtraEditors.XtraMessageBox.Show("Kesalahan perhitugan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
        Finally
            ds.Dispose()
        End Try
    End Sub
    Public Shared Sub HitungSelisihStockOpname(ByVal IDMutasi As Long, ByVal TglDitetapkanSO As Date)
        Dim ds As New DataSet
        Dim SQL As String = ""
        Dim SaldoStockAkhir As Double = 0.0
        Dim Konversi As Double = 0.0
        Dim QtySebelum As Double = 0.0
        Dim QtySebelumSatuNota As Double = 0.0
        Try
            ds = modSqlServer.ExecuteDataset("MMutasiGudangD", "SELECT MStockOpnameD.*, MStockOpname.IsPosted, MStockOpname.Jam AS JamSO, MStockOpname.Tanggal AS TGLMutasi, MStockOpnameD.IDGudang FROM MStockOpnameD LEFT JOIN MStockOpname ON MStockOpname.NoID=MStockOpnameD.IDHeader WHERE MStockOpname.NoID=" & IDMutasi)
            If ds.Tables("MMutasiGudangD").Rows.Count >= 1 Then
                If Not NullToBool(ds.Tables("MMutasiGudangD").Rows(0).Item("IsPosted")) Then
                    Konversi = NullToDbl(ds.Tables("MMutasiGudangD").Rows(0).Item("Konversi"))
                    If Konversi = 0 Then Konversi = 1
                    For i As Integer = 0 To ds.Tables("MMutasiGudangD").Rows.Count - 1
                        'Jika Komulatif untuk sementara
                        'If NullToLong(ds.Tables(0).Rows(i).Item("IDBArangD")) = 19613 Then
                        '    XtraMessageBox.Show("")
                        'End If
                        QtySebelumSatuNota = NullToDbl(EksekusiSQlSkalarNew("SELECT COUNT(MStockOpnameD.NoID) FROM MStockOpnameD INNER JOIN MStockOpname ON MStockOpname.NoID=MStockOpnameD.IDHeader WHERE MStockOpname.NoID=" & NullToLong(ds.Tables(0).Rows(i).Item("IDHeader")) & " AND MStockOpnameD.NoID<" & NullToLong(ds.Tables(0).Rows(i).Item("NoID")) & " AND MStockOpnameD.IDBarangD=" & NullToLong(ds.Tables("MMutasiGudangD").Rows(i).Item("IDBarangD"))))
                        QtySebelum = NullToDbl(EksekusiSQlSkalarNew("SELECT COUNT(MStockOpnameD.NoID) FROM MStockOpnameD INNER JOIN MStockOpname ON MStockOpname.NoID=MStockOpnameD.IDHeader WHERE MStockOpname.Tanggal>='" & TglDitetapkanSO.ToString("yyyy-MM-dd") & "' AND MStockOpname.Tanggal<'" & FixApostropi(CDate(ds.Tables(0).Rows(i).Item("TglMutasi")).ToString("yyyy/MM/dd HH:mm")) & "' AND MStockOpnameD.IDBarangD=" & NullToLong(ds.Tables("MMutasiGudangD").Rows(i).Item("IDBarangD"))))
                        If QtySebelum = 0 And QtySebelumSatuNota = 0 Then
                            SaldoStockAkhir = NullToDbl(EksekusiSQlSkalarNew("SELECT SUM((MKartuStok.Qtymasuk*MKartuStok.Konversi)-(MKartuStok.QtyKeluar*MKartuStok.Konversi)) AS Qty " & _
                                                                             " FROM MKartuStok " & _
                                                                             " WHERE IDGudang=" & NullToLong(ds.Tables("MMutasiGudangD").Rows(i).Item("IDGudang")) & " " & _
                                                                             " AND IDBarangD=" & NullToLong(ds.Tables("MMutasiGudangD").Rows(i).Item("IDBarangD")) & " " & _
                                                                             " AND IDBarang=" & NullToLong(ds.Tables("MMutasiGudangD").Rows(i).Item("IDBarang")) & " " & _
                                                                             " AND Tanggal>='" & TglDitetapkanSO.ToString("yyyy/MM/dd") & "' AND Tanggal<='" & Format(NullToDate(ds.Tables("MMutasiGudangD").Rows(i).Item("TGLMutasi")), "yyyy/MM/dd") & " " & Format(NullToDate(ds.Tables("MMutasiGudangD").Rows(i).Item("JamSO")), "HH:mm") & "'")) / Konversi
                        ElseIf QtySebelumSatuNota <> 0 Then 'Belum Ada di Stock Opname Maka Carikan Saldo di MKartuStok 20-08-2012
                            SaldoStockAkhir = 0
                        Else
                            SaldoStockAkhir = 0
                        End If
                        'If NullToLong(ds.Tables("MMutasiGudangD").Rows(i).Item("IDBarang")) = 4218 Then
                        '    MsgBox("")
                        'End If
                        EksekusiSQL("Update MStockOpnameD SET Qty=" & FixKoma(NullToDbl(ds.Tables("MMutasiGudangD").Rows(i).Item("QtyFisik")) - SaldoStockAkhir) & "," & _
                                    " QtyKomputer=" & FixKoma(SaldoStockAkhir) & "," & _
                                    " QtyPcs=" & FixKoma(Konversi * (NullToDbl(ds.Tables("MMutasiGudangD").Rows(i).Item("QtyFisik")) - SaldoStockAkhir)) & ", " & _
                                    " Jumlah=HargaPokok*QtyFisik*Konversi " & _
                                    " WHERE NoID=" & NullToLong(ds.Tables("MMutasiGudangD").Rows(i).Item("NoID")))
                        Application.DoEvents()
                    Next
                    SQL = "UPDATE MStockOpname SET Total=IsNull((SELECT SUM(MStockOpnameD.Jumlah) FROM MStockOpnameD INNER JOIN MStockOpname ON MStockOpname.NoID=MStockOpnameD.IDHeader WHERE MStockOpname.NoID=" & IDMutasi & "),0) WHERE MStockOpname.NoID=" & IDMutasi
                    EksekusiSQL(SQL)
                End If
            End If
        Catch ex As Exception
            DevExpress.XtraEditors.XtraMessageBox.Show("Kesalahan perhitugan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
        Finally
            ds.Dispose()
        End Try
    End Sub
    Public Shared Sub UnPostingPenyesuaian(ByVal IDMutasiGudang As Long)
        Dim ds As New DataSet
        Try
            ds = ExecuteDataset("NamaTabel", "SELECT MKartuStok.* FROM MKartuStok LEFT JOIN MPenyesuaian ON MPenyesuaian.NoID=MKartuStok.IDTransaksi WHERE (MKartuStok.IsPosted=0 OR MKartuStok.IsPosted Is Null) AND MPenyesuaian.NoID=" & IDMutasiGudang & " AND MKartuStok.IDJenisTransaksi=14 ")
            If ds.Tables(0).Rows.Count >= 1 Then
                If Not NullToBool(ds.Tables(0).Rows(0).Item("IsPosted")) Then
                    EksekusiSQL("DELETE FROM MKartuStok WHERE IDTransaksi=" & IDMutasiGudang & " AND IDJenisTransaksi=14 ")
                    EksekusiSQL("UPDATE MPenyesuaian SET IsPosted=0, IDUserPosting=NULL, TglPosting=NULL WHERE NoID=" & IDMutasiGudang)
                End If
            Else
                EksekusiSQL("UPDATE MPenyesuaian SET IsPosted=0, IDUserPosting=NULL, TglPosting=NULL WHERE NoID=" & IDMutasiGudang)
            End If
        Catch ex As Exception
            DevExpress.XtraEditors.XtraMessageBox.Show("Kesalahan perhitugan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
        Finally
            ds.Dispose()
        End Try
    End Sub
    Public Shared Sub UnPostingStockOpname(ByVal IDMutasiGudang As Long)
        Dim ds As New DataSet
        Try
            ds = ExecuteDataset("NamaTabel", "SELECT MKartuStok.* FROM MKartuStok LEFT JOIN MStockOpname ON MStockOpname.NoID=MKartuStok.IDTransaksi WHERE (MKartuStok.IsPosted=0 OR MKartuStok.IsPosted Is Null) AND MStockOpname.NoID=" & IDMutasiGudang & " AND MKartuStok.IDJenisTransaksi=9 ")
            If ds.Tables(0).Rows.Count >= 1 Then
                If Not NullToBool(ds.Tables(0).Rows(0).Item("IsPosted")) AndAlso Not StockOpnameIsLock(IDMutasiGudang) Then
                    EksekusiSQL("DELETE FROM MKartuStok WHERE IDTransaksi=" & IDMutasiGudang & " AND IDJenisTransaksi=9 ")
                    EksekusiSQL("UPDATE MStockOpname SET IsPosted=0, IDUserPosting=NULL, TglPosting=NULL WHERE NoID=" & IDMutasiGudang)
                End If
            ElseIf ds.Tables(0).Rows.Count = 0 Then
                EksekusiSQL("UPDATE MStockOpname SET IsPosted=0, IDUserPosting=NULL, TglPosting=NULL WHERE NoID=" & IDMutasiGudang)
            End If
        Catch ex As Exception
            DevExpress.XtraEditors.XtraMessageBox.Show("Kesalahan perhitugan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
        Finally
            ds.Dispose()
        End Try
    End Sub
    Public Shared Function StockOpnameIsLock(ByVal IDStockOpname As Long) As Boolean
        Dim ds As New DataSet
        Try
            ds = ExecuteDataset("NamaTabel", "SELECT MStockOpname.* FROM MStockOpname WHERE MStockOpname.NoID=" & IDStockOpname)
            If ds.Tables(0).Rows.Count >= 1 Then
                If NullToBool(ds.Tables(0).Rows.Item("IsLock")) Then
                    Return True
                Else
                    Return False
                End If
            Else
                Return False
            End If
        Catch ex As Exception
            Return False
        End Try
    End Function
    Public Shared Sub PostingPemakaian(ByVal IDMutasi As Long)
        Dim ds As New DataSet
        Dim SQL As String = ""
        Dim SaldoStockAkhir As Double = 0.0
        Dim HPPPerItem As Double = 0.0
        Try
            ds = modSqlServer.ExecuteDataset("MMutasiGudangD", "SELECT MPemakaianD.*, MPemakaian.IsPosted, MPemakaian.Tanggal AS TGLMutasi, MPemakaianD.IDGudang FROM MPemakaianD LEFT JOIN MPemakaian ON MPemakaian.NoID=MPemakaianD.IDHeader WHERE MPemakaian.NoID=" & IDMutasi)
            If ds.Tables("MMutasiGudangD").Rows.Count >= 1 Then
                If Not NullToBool(ds.Tables("MMutasiGudangD").Rows(0).Item("IsPosted")) Then
                    EksekusiSQL("DELETE FROM MKartuStok WHERE IDTransaksi=" & IDMutasi & " AND IDJenisTransaksi=8")
                    For i As Integer = 0 To ds.Tables("MMutasiGudangD").Rows.Count - 1
                        '- Keluarkan, + Masuk
                        SaldoStockAkhir = 0 'GetStockAkhirGudang(NullToLong(ds.Tables("MMutasiGudangD").Rows(i).Item("IDBarang")), NullToLong(ds.Tables("MMutasiGudangD").Rows(i).Item("IDGudang")), NullToDate(ds.Tables("MMutasiGudangD").Rows(i).Item("TGLMutasi")))
                        'HPPPerItem = GetHPP(NullTolong(ds.Tables("MMutasiGudangD").Rows(i).Item("IDBarang")), NullToDate(ds.Tables("MMutasiGudangD").Rows(i).Item("TGLMutasi")))
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
                        SQL &= " ,[IDAlamat],[IDBarangD])" & vbCrLf
                        SQL &= " SELECT " & GetNewID("MKartuStok", "NoID") & " AS NoID, MPemakaian.Kode, MPemakaianD.IDBarang, "
                        SQL &= " MWilayah.IDDepartemen, MPemakaianD.IDGudang, "
                        SQL &= " MPemakaian.Tanggal, 8 AS IDJenisTransaksi,  MPemakaian.NoID AS IDTransaksi, "
                        SQL &= " MPemakaianD.NoID AS IDTransaksiDetil, MPemakaianD.Keterangan, (ISNULL(MPemakaianD.Qty,0)*ISNULL(MPemakaianD.Konversi,0)) AS KeluarA, MPemakaianD.Qty AS Keluar,"
                        SQL &= " 0 AS HrgJualPerItem, 0 AS Jumlah, 0 AS Masuk,0 AS MasukA,"
                        SQL &= " 0,0,0 AS HPPKeluar, "
                        SQL &= " 0 AS JumHPPKeluar, " & Replace(SaldoStockAkhir, ",", ".") & "-(ISNULL(MPemakaianD.Qty,0)*ISNULL(MPemakaianD.Konversi,0)) as QtyAkhir, 0,"
                        SQL &= " 0, MBarang.IDJenis, MPemakaianD.IDSatuan, "
                        SQL &= " isnull(MPemakaianD.Konversi,0), 0 AS IDAlamat, MPemakaianD.IDBarangD "
                        SQL &= " FROM MPemakaianD LEFT JOIN MPemakaian ON MPemakaian.NoID=MPemakaianD.IDHeader "
                        SQL &= " LEFT JOIN (MGudang LEFT JOIN MWilayah ON MWilayah.NoID=MGudang.IDWilayah) ON MPemakaianD.IDGudang=MGudang.NoID"
                        SQL &= " LEFT JOIN MBarang ON MBarang.NoID=MPemakaianD.IDBarang"
                        SQL &= " WHERE MPemakaianD.NoID=" & NullToLong(ds.Tables("MMutasiGudangD").Rows(i).Item("NoID"))
                        EksekusiSQL(SQL)
                    Next

                    EksekusiSQL("UPDATE MPemakaian SET IsPosted=1, IDUSerPosting=" & IDUserAktif & ", TglPosting=getdate() WHERE NoID=" & IDMutasi)
                End If
            End If
        Catch ex As Exception
            DevExpress.XtraEditors.XtraMessageBox.Show("Kesalahan perhitugan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
        Finally
            ds.Dispose()
        End Try
    End Sub
    Public Shared Sub UnPostingPemakaian(ByVal IDMutasiGudang As Long)
        Dim ds As New DataSet
        Try
            ds = ExecuteDataset("NamaTabel", "SELECT MKartuStok.* FROM MKartuStok LEFT JOIN MPemakaian ON MPemakaian.NoID=MKartuStok.IDTransaksi WHERE (MKartuStok.IsPosted=0 OR MKartuStok.IsPosted Is Null) AND MPemakaian.NoID=" & IDMutasiGudang & " AND MKartuStok.IDJenisTransaksi=8 ")
            If ds.Tables(0).Rows.Count >= 1 Then
                If Not NullToBool(ds.Tables(0).Rows(0).Item("IsPosted")) Then
                    EksekusiSQL("DELETE FROM MKartuStok WHERE IDTransaksi=" & IDMutasiGudang & " AND IDJenisTransaksi=8 ")
                    EksekusiSQL("UPDATE MPemakaian SET IsPosted=0, IDUserPosting=NULL, TglPosting=NULL WHERE NoID=" & IDMutasiGudang)
                End If
            Else
                EksekusiSQL("UPDATE MPemakaian SET IsPosted=0, IDUserPosting=NULL, TglPosting=NULL WHERE NoID=" & IDMutasiGudang)
            End If
        Catch ex As Exception
            DevExpress.XtraEditors.XtraMessageBox.Show("Kesalahan perhitugan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
        Finally
            ds.Dispose()
        End Try
    End Sub
    Public Shared Sub PostingSaldoAwalPersediaan(ByVal IDMutasi As Long)
        Dim ds As New DataSet
        Dim SQL As String = ""
        Dim SaldoStockAkhir As Double = 0.0
        Dim HPPPerItem As Double = 0.0
        Try
            ds = modSqlServer.ExecuteDataset("MMutasiGudangD", "SELECT MSaldoAwalPersediaanD.*, MSaldoAwalPersediaan.IsPosted, MSaldoAwalPersediaan.Tanggal AS TGLMutasi, MSaldoAwalPersediaanD.IDGudang FROM MSaldoAwalPersediaanD LEFT JOIN MSaldoAwalPersediaan ON MSaldoAwalPersediaan.NoID=MSaldoAwalPersediaanD.IDHeader WHERE MSaldoAwalPersediaan.NoID=" & IDMutasi)
            If ds.Tables("MMutasiGudangD").Rows.Count >= 1 Then
                If Not NullToBool(ds.Tables("MMutasiGudangD").Rows(0).Item("IsPosted")) Then
                    EksekusiSQL("DELETE FROM MKartuStok WHERE IDTransaksi=" & IDMutasi & " AND IDJenisTransaksi=1")
                    For i As Integer = 0 To ds.Tables("MMutasiGudangD").Rows.Count - 1
                        '- Keluarkan, + Masuk
                        SaldoStockAkhir = 0 'GetStockAkhirGudang(NullToLong(ds.Tables("MMutasiGudangD").Rows(i).Item("IDBarang")), NullToLong(ds.Tables("MMutasiGudangD").Rows(i).Item("IDGudang")), NullToDate(ds.Tables("MMutasiGudangD").Rows(i).Item("TGLMutasi")))
                        'HPPPerItem = GetHPP(NullTolong(ds.Tables("MMutasiGudangD").Rows(i).Item("IDBarang")), NullToDate(ds.Tables("MMutasiGudangD").Rows(i).Item("TGLMutasi")))
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
                        If NullToDbl(ds.Tables("MMutasiGudangD").Rows(i).Item("Qty")) < 0 Then
                            SQL &= " ,[QtyKeluarA]"
                            SQL &= " ,[QtyKeluar]"
                        Else
                            SQL &= " ,[QtyMasukA]"
                            SQL &= " ,[QtyMasuk]"
                        End If
                        SQL &= " ,[HargaJualPerItem]"
                        SQL &= " ,[JumlahJual]"
                        If NullToDbl(ds.Tables("MMutasiGudangD").Rows(i).Item("Qty")) < 0 Then
                            SQL &= " ,[QtyMasukA]"
                            SQL &= " ,[QtyMasuk]"
                        Else
                            SQL &= " ,[QtyKeluarA]"
                            SQL &= " ,[QtyKeluar]"
                        End If
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
                        SQL &= " SELECT " & GetNewID("MKartuStok", "NoID") & " AS NoID, MSaldoAwalPersediaan.Kode, MSaldoAwalPersediaanD.IDBarang, "
                        SQL &= " MWilayah.IDDepartemen, MSaldoAwalPersediaanD.IDGudang, "
                        SQL &= " MSaldoAwalPersediaan.Tanggal, 1 AS IDJenisTransaksi,  MSaldoAwalPersediaan.NoID AS IDTransaksi, "
                        SQL &= " MSaldoAwalPersediaanD.NoID AS IDTransaksiDetil, MSaldoAwalPersediaanD.Keterangan, (ISNULL(ABS(MSaldoAwalPersediaanD.Qty),0)*ISNULL(MSaldoAwalPersediaanD.Konversi,0)) AS KeluarA, ABS(MSaldoAwalPersediaanD.Qty) AS Keluar,"
                        SQL &= " 0 AS HrgJualPerItem, 0 AS Jumlah, 0 AS Masuk,0 AS MasukA,"
                        SQL &= " 0,0,0 AS HPPKeluar, "
                        SQL &= " 0 AS JumHPPKeluar, " & Replace(SaldoStockAkhir, ",", ".") & "-(ISNULL(MSaldoAwalPersediaanD.Qty,0)*ISNULL(MSaldoAwalPersediaanD.Konversi,0)) as QtyAkhir, 0,"
                        SQL &= " 0, MBarang.IDJenis, MSaldoAwalPersediaanD.IDSatuan, "
                        SQL &= " isnull(MSaldoAwalPersediaanD.Konversi,0), 0 AS IDAlamat "
                        SQL &= " FROM MSaldoAwalPersediaanD LEFT JOIN MSaldoAwalPersediaan ON MSaldoAwalPersediaan.NoID=MSaldoAwalPersediaanD.IDHeader "
                        SQL &= " LEFT JOIN (MGudang LEFT JOIN MWilayah ON MWilayah.NoID=MGudang.IDWilayah) ON MSaldoAwalPersediaanD.IDGudang=MGudang.NoID"
                        SQL &= " LEFT JOIN MBarang ON MBarang.NoID=MSaldoAwalPersediaanD.IDBarang"
                        SQL &= " WHERE MSaldoAwalPersediaanD.NoID=" & NullToLong(ds.Tables("MMutasiGudangD").Rows(i).Item("NoID"))
                        EksekusiSQL(SQL)
                        SQL = "UPDATE MBarang SET HPP=" & NullToDbl(ds.Tables("MMutasiGudangD").Rows(i).Item("HPP")) & " WHERE NoID=" & NullToLong(ds.Tables("MMutasiGudangD").Rows(i).Item("IDBarang"))
                        EksekusiSQL(SQL)
                    Next
                    EksekusiSQL("UPDATE MSaldoAwalPersediaan SET IsPosted=1, IDUSerPosting=" & IDUserAktif & ", TglPosting=getdate() WHERE NoID=" & IDMutasi)
                End If
            End If
        Catch ex As Exception
            DevExpress.XtraEditors.XtraMessageBox.Show("Kesalahan perhitugan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
        Finally
            ds.Dispose()
        End Try
    End Sub
    Public Shared Sub UnPostingSaldoAwalPersediaan(ByVal IDMutasiGudang As Long)
        Dim ds As New DataSet
        Try
            ds = ExecuteDataset("NamaTabel", "SELECT MKartuStok.* FROM MKartuStok LEFT JOIN MSaldoAwalPersediaan ON MSaldoAwalPersediaan.NoID=MKartuStok.IDTransaksi WHERE (MKartuStok.IsPosted=0 OR MKartuStok.IsPosted Is Null) AND MSaldoAwalPersediaan.NoID=" & IDMutasiGudang & " AND MKartuStok.IDJenisTransaksi=1 ")
            If ds.Tables(0).Rows.Count >= 1 Then
                If Not NullToBool(ds.Tables(0).Rows(0).Item("IsPosted")) AndAlso Not AdaTransaksi(NullToLong(ds.Tables(0).Rows(0).Item("IDBarang")), NullToLong(ds.Tables(0).Rows(0).Item("IDGudang"))) Then
                    EksekusiSQL("DELETE FROM MKartuStok WHERE IDTransaksi=" & IDMutasiGudang & " AND IDJenisTransaksi=1 ")
                    EksekusiSQL("UPDATE MSaldoAwalPersediaan SET IsPosted=0, IDUserPosting=NULL, TglPosting=NULL WHERE NoID=" & IDMutasiGudang)
                End If
            ElseIf ds.Tables(0).Rows.Count = 0 Then
                EksekusiSQL("UPDATE MSaldoAwalPersediaan SET IsPosted=0, IDUserPosting=NULL, TglPosting=NULL WHERE NoID=" & IDMutasiGudang)
            End If
        Catch ex As Exception
            DevExpress.XtraEditors.XtraMessageBox.Show("Kesalahan perhitugan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
        Finally
            ds.Dispose()
        End Try
    End Sub
    Private Shared Function AdaTransaksi(ByVal IDBarang As Long, ByVal IDGudang As Long) As Boolean
        Dim ds As New DataSet
        Try
            ds = ExecuteDataset("MKartuStok", "SELECT * FROM MKartuStok WHERE IDBarang=" & IDBarang & " AND IDGudang=" & IDGudang & " AND IDJenisTransaksi<>1")
            If ds.Tables(0).Rows.Count >= 1 Then
                Return True
            Else
                Return False
            End If
        Catch ex As Exception
            XtraMessageBox.Show("Info : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
            Return False
        Finally
            ds.Dispose()
        End Try
    End Function
End Class
