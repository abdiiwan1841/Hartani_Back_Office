Imports VPoint.Ini
Imports System.Data
Imports System.Data.SqlClient
Imports DevExpress.XtraEditors
Imports DevExpress.Utils

Public Class clsPostingKartuStok
    'Public Enum JenisTransaksi
    '    Pembelian = 2
    '    Penjualan = 6
    '    MutasiGudang = 4
    'End Enum
    Public Shared Function GetStockAkhirGudang(ByVal IDBarang As Long, ByVal IDGudang As Long, ByVal bln As Date) As Double
        Dim ds As New DataSet
        Dim SQL As String = ""
        Try
            SQL = "SELECT SUM((IsNull(QtyMasuk,0)*IsNull(Konversi,0))-(IsNull(QtyKeluar,0)*IsNull(Konversi,0))) AS Saldo " & _
                   " From MKartuStok " & _
                   " WHERE Month(Tanggal)=" & Month(bln) & " AND Year(Tanggal)=" & Year(bln) & " " & _
                   " GROUP BY IDGudang, IDBarang " & _
                   " Having (IDGudang = " & IDGudang & ") And (IDBarang = " & IDBarang & " )"
            ds = modSqlServer.ExecuteDataset("MStock", SQL)
            If ds.Tables("MStock").Rows.Count >= 1 Then
                Return NullToDbl(ds.Tables("MStock").Rows(0).Item("Saldo"))
            Else
                Return 0
            End If
        Catch ex As Exception
            DevExpress.XtraEditors.XtraMessageBox.Show("Kesalahan perhitugan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
        End Try
    End Function
    'Public Shared Function CekSaldoStock(ByVal IDGudang As Long, ByVal IDBarang As Long, ByVal Tgl As Date) As Double
    '    Dim SQL As String = ""
    '    Try
    '        SQL = "SELECT SUM((IsNull(MKartuStok.QtyMasuk,0)-IsNull(MKartuStok.QtyKeluar,0))*IsNull(MKartuStok.Konversi,0)) AS Saldo" & vbCrLf & _
    '              " FROM MKartuStok LEFT JOIN MGudang ON MGudang.NoID=MKartuStok.IDGudang " & vbCrLf & _
    '              " WHERE MKartuStok.IDBarang=" & IDBarang & " AND MGudang.NoID=" & IDGudang & " AND MGudang.IDWilayah=" & DefIDWilayah & " AND MKartuStok.Tanggal<'" & DateAdd(DateInterval.Day, 1, Tgl).ToString("yyyy-MM-dd") & "'"
    '        Return NullToDbl(EksekusiSQlSkalarNew(SQL))
    '    Catch ex As Exception
    '        DevExpress.XtraEditors.XtraMessageBox.Show("Kesalahan perhitugan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
    '    End Try
    'End Function
    Public Shared Function CekSaldoStockVerian(ByVal IDGudang As Long, ByVal IDBarangD As Long, ByVal Tgl As Date) As Double
        Dim SQL As String = ""
        Try
            SQL = "SELECT SUM((IsNull(MKartuStok.QtyMasuk,0)-IsNull(MKartuStok.QtyKeluar,0))*IsNull(MKartuStok.Konversi,0)) AS Saldo" & vbCrLf & _
                  " FROM MKartuStok LEFT JOIN MGudang ON MGudang.NoID=MKartuStok.IDGudang " & vbCrLf & _
                  " WHERE MKartuStok.IDBarangD=" & IDBarangD & " AND MGudang.NoID=" & IDGudang & " AND MGudang.IDWilayah=" & DefIDWilayah & " AND MKartuStok.Tanggal<'" & DateAdd(DateInterval.Day, 1, Tgl).ToString("yyyy-MM-dd") & "'"
            Return NullToDbl(EksekusiSQlSkalarNew(SQL))
        Catch ex As Exception
            DevExpress.XtraEditors.XtraMessageBox.Show("Kesalahan perhitugan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
        End Try
    End Function
    Public Shared Function GetHPP(ByVal IdBarang As Long, ByVal Tanggal As String) As Double
        Dim ds As New DataSet
        Dim SQL As String = ""
        Try
            SQL = "SELECT TOP 1 HgBeliRtRtPeritem " & _
                  " From MKartuStok " & _
                  " WHERE (Tanggal <= CONVERT(DATETIME, '" & Tanggal & "', 102)) AND (IDBarang = " & IdBarang & ") " & _
                  " ORDER BY Tanggal DESC,NoID DESC,IDJenisTransaksi DESC " ' "ORDER BY Tanggal DESC,ID DESC,IDJenisTransaksi DESC "
            ds = modSqlServer.ExecuteDataset("MHPP", SQL)
            If ds.Tables("MHPP").Rows.Count >= 1 Then
                Return NullToDbl(ds.Tables("MHPP").Rows(0).Item("HgBeliRtRtPeritem"))
            Else
                Return 0
            End If
        Catch ex As Exception
            DevExpress.XtraEditors.XtraMessageBox.Show("Kesalahan perhitugan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
        End Try
    End Function
    Public Shared Sub TriggerStok(ByVal TglDari As DateTime, ByVal TglSampai As DateTime, ByVal IDBarang As Long)
        Dim SQL As String = ""
        Dim ds As New DataSet
        Dim SaldoAwal As Double = 0.0
        Try
            SQL = "DELETE FROM TKartuStok WHERE IDUser=" & IDUserAktif & " AND IP='" & FixApostropi(IPLokal) & "'"
            EksekusiSQL(SQL)
            If IDBarang = -1 Then
                SQL = "SELECT MKartuStok.* FROM MKartuStok LEFT JOIN MGudang ON MGudang.NoID=MKartuStok.IDGudang LEFT JOIN MJenisTransaksi ON MJenisTransaksi.ID=MKartuStok.IDJenisTransaksi WHERE (MKartuStok.Tanggal>='" & TglDari.ToString("yyyy/MM/dd") & "' AND MKartuStok.Tanggal<'" & DateAdd(DateInterval.Day, 1, TglSampai).ToString("yyyy/MM/dd") & "') AND MGudang.IDWilayah=" & DefIDWilayah & "  ORDER BY MKartuStok.Tanggal, MJenisTransaksi.NoUrut, MKartuStok.NoID"
            Else
                SQL = "SELECT MKartuStok.* FROM MKartuStok LEFT JOIN MGudang ON MGudang.NoID=MKartuStok.IDGudang LEFT JOIN MJenisTransaksi ON MJenisTransaksi.ID=MKartuStok.IDJenisTransaksi WHERE (MKartuStok.Tanggal>='" & TglDari.ToString("yyyy/MM/dd") & "' AND MKartuStok.Tanggal<'" & DateAdd(DateInterval.Day, 1, TglSampai).ToString("yyyy/MM/dd") & "') AND MGudang.IDWilayah=" & DefIDWilayah & " AND MKartuStok.IDBarang=" & IDBarang & " ORDER BY MKartuStok.Tanggal, MJenisTransaksi.NoUrut, MKartuStok.NoID"
            End If
            ds = ExecuteDataset("MKartuStok", SQL)
            If ds.Tables("MKartuStok").Rows.Count >= 1 Then
                For i As Long = 0 To ds.Tables(0).Rows.Count - 1
                    SQL = "SELECT SUM((IsNull(QtyMasuk,0)*IsNull(Konversi,0))-(IsNull(QtyKeluar,0)*IsNull(Konversi,0))) " & _
                          " FROM MKartuStok " & _
                          " LEFT JOIN MGudang ON MGudang.NoID=MKartustok.IDGudang " & _
                          " WHERE (MKartuStok.Tanggal<'" & NullToDate(ds.Tables(0).Rows(i).Item("Tanggal")).ToString("yyyy-MM-dd HH:mm") & "' AND MKartuStok.IDBarang=" & NullToLong(ds.Tables(0).Rows(i).Item("IDBarang")) & " AND MGudang.IDWilayah=" & DefIDWilayah & ") OR (MKartuStok.Tanggal<='" & NullToDate(ds.Tables(0).Rows(i).Item("Tanggal")).ToString("yyyy-MM-dd HH:mm") & "' AND MKartuStok.IDBarang=" & NullToLong(ds.Tables(0).Rows(i).Item("IDBarang")) & " AND MGudang.IDWilayah=" & DefIDWilayah & " AND MKartuStok.NoID<" & NullToLong(ds.Tables(0).Rows(i).Item("NoID")) & ")"
                    SaldoAwal = NullToDbl(EksekusiSQlSkalarNew(SQL))
                    SQL = "INSERT INTO TKartuStok ([NoID],[IDKartuStok],[IDBarang],[IDGudang],[IDSatuan],[Konversi],[SaldoAwal],[QtyMasuk],[QtyMasukA],[QtyKeluar],[QtyKeluarA],[SaldoAkhir],[IDUser],[IP]) VALUES ("
                    SQL &= NullToLong(GetNewID("TKartuStok", "NoID")) & ","
                    SQL &= NullToLong(ds.Tables(0).Rows(i).Item("NoID")) & ","
                    SQL &= NullToLong(ds.Tables(0).Rows(i).Item("IDBarang")) & ","
                    SQL &= NullToLong(ds.Tables(0).Rows(i).Item("IDGudang")) & ","
                    SQL &= NullToLong(ds.Tables(0).Rows(i).Item("IDSatuan")) & ","
                    SQL &= FixKoma(NullToDbl(ds.Tables(0).Rows(i).Item("Konversi"))) & ","
                    SQL &= FixKoma(SaldoAwal) & ","
                    SQL &= FixKoma(NullToDbl(ds.Tables(0).Rows(i).Item("QtyMasuk"))) & ","
                    SQL &= FixKoma(NullToDbl(ds.Tables(0).Rows(i).Item("QtyMasuk")) * NullToDbl(ds.Tables(0).Rows(i).Item("Konversi"))) & ","
                    SQL &= FixKoma(NullToDbl(ds.Tables(0).Rows(i).Item("QtyKeluar"))) & ","
                    SQL &= FixKoma(NullToDbl(ds.Tables(0).Rows(i).Item("QtyKeluar")) * NullToDbl(ds.Tables(0).Rows(i).Item("Konversi"))) & ","
                    SQL &= FixKoma(SaldoAwal + (NullToDbl(ds.Tables(0).Rows(i).Item("QtyMasuk")) * NullToDbl(ds.Tables(0).Rows(i).Item("Konversi"))) - NullToDbl(ds.Tables(0).Rows(i).Item("QtyKeluar")) * NullToDbl(ds.Tables(0).Rows(i).Item("Konversi"))) & ","
                    SQL &= IDUserAktif & ",'" & FixApostropi(IPLokal) & "'"
                    SQL &= ")"
                    EksekusiSQL(SQL)
                Next
            End If
        Catch ex As Exception
            XtraMessageBox.Show("Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            ds.Dispose()
        End Try
    End Sub
    Public Shared Sub LihatHasilPosting(ByVal IDTransaksi As Long, ByVal TabelMaster As String)
        Try
            If IsShowStock AndAlso frmHasilPosted Is Nothing Then
                frmHasilPosted = New frmHasilPosting
                frmHasilPosted.TopMost = True
                frmHasilPosted.Show()
                frmHasilPosted.Focus()
                IsShowStock = True
            ElseIf Not IsShowStock AndAlso Not frmHasilPosted Is Nothing Then
                frmHasilPosted.Close()
                frmHasilPosted.Dispose()
                frmHasilPosted = New frmHasilPosting
                frmHasilPosted.TopMost = True
                frmHasilPosted.Show()
                frmHasilPosted.Focus()
                IsShowStock = True
            ElseIf Not IsShowStock AndAlso frmHasilPosted Is Nothing Then
                frmHasilPosted = New frmHasilPosting
                frmHasilPosted.TopMost = True
                frmHasilPosted.Show()
                frmHasilPosted.Focus()
                IsShowStock = True
            Else
                IsShowStock = True
            End If
            With frmHasilPosted
                Select Case TabelMaster.ToUpper
                    Case "MBELI".ToUpper
                        .IDJenisTransaksi = 2
                    Case "MReturBeli".ToUpper
                        .IDJenisTransaksi = 3
                    Case "MMUTASIGUDANG".ToUpper
                        .IDJenisTransaksi = 4
                    Case "MJUAL".ToUpper
                        .IDJenisTransaksi = 6
                    Case "MReturJUAL".ToUpper
                        .IDJenisTransaksi = 7
                    Case "MPemakaian".ToUpper
                        .IDJenisTransaksi = 8
                    Case "MStockOpname".ToUpper
                        .IDJenisTransaksi = 9
                    Case "MREVISIHARGABELI".ToUpper
                        .IDJenisTransaksi = 11
                    Case "MTRANSFEROUT".ToUpper
                        .IDJenisTransaksi = 12
                    Case "MTRANSFERIN".ToUpper
                        .IDJenisTransaksi = 13
                    Case "MPenyesuaian".ToUpper
                        .IDJenisTransaksi = 14
                    Case "MREVISIHARGAJUAL".ToUpper
                        .IDJenisTransaksi = 15
                    Case "MSaldoAwalPersediaan".ToUpper
                        .IDJenisTransaksi = 1
                    Case Else
                        .IDJenisTransaksi = -1
                End Select
                .IDTransaksi = IDTransaksi
                .RefreshData()
            End With
        Catch ex As Exception
            XtraMessageBox.Show("Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub
    Public Shared Sub HasilPosting(ByVal IDTransaksi As Long, ByVal TabelMaster As String)
        Try
            If IsShowHasilPostingan AndAlso frmHasilPostingan Is Nothing Then
                frmHasilPostingan = New frmHasilPostingNonStock
                frmHasilPostingan.TableMaster = TabelMaster
                frmHasilPostingan.TopMost = True
                frmHasilPostingan.Show()
                frmHasilPostingan.Focus()
                IsShowHasilPostingan = True
            ElseIf Not IsShowHasilPostingan AndAlso Not frmHasilPostingan Is Nothing Then
                frmHasilPostingan.Close()
                frmHasilPostingan.Dispose()
                frmHasilPostingan = New frmHasilPostingNonStock
                frmHasilPostingan.TableMaster = TabelMaster
                frmHasilPostingan.TopMost = True
                frmHasilPostingan.Show()
                frmHasilPostingan.Focus()
                IsShowHasilPostingan = True
            ElseIf Not IsShowHasilPostingan AndAlso frmHasilPostingan Is Nothing Then
                frmHasilPostingan = New frmHasilPostingNonStock
                frmHasilPostingan.TableMaster = TabelMaster
                frmHasilPostingan.TopMost = True
                frmHasilPostingan.Show()
                frmHasilPostingan.Focus()
                IsShowHasilPostingan = True
            ElseIf IsShowHasilPostingan AndAlso Not frmHasilPostingan Is Nothing Then
                If frmHasilPostingan.TableMaster <> TabelMaster Then
                    frmHasilPostingan.Close()
                    frmHasilPostingan.Dispose()
                    frmHasilPostingan = New frmHasilPostingNonStock
                    frmHasilPostingan.TopMost = True
                    frmHasilPostingan.TableMaster = TabelMaster
                    frmHasilPostingan.Show()
                    frmHasilPostingan.Focus()
                End If
                IsShowHasilPostingan = True
            Else
                frmHasilPostingan.TableMaster = TabelMaster
                IsShowHasilPostingan = True
            End If
            With frmHasilPostingan
                Select Case TabelMaster.ToUpper
                    Case "MPO".ToUpper
                        .SQLString = "SELECT MBeliD.NoID, MBeliD.IDBeli, MBeli.Kode, MBeli.Tanggal, MAlamat.Nama AS Supplier, " & vbCrLf & _
                                     " MGudang.Nama AS Gudang, MWilayah.Nama AS Wilayah, MBarang.Kode AS KodeStock, MBarang.Nama AS NamaStock, MBarang.KodeAlias AS KodeAlias, MBarang.NamaAlias, " & vbCrLf & _
                                     " MBeliD.Qty, MBeliD.Ctn, MSatuan.Nama AS Satuan, MBeliD.Konversi, MBeliD.Harga, MBeliD.Harga/MBeliD.Konversi AS [HargaPcs], MBeliD.Qty*MBeliD.Konversi AS [QtyPcs]" & vbCrLf & _
                                     " FROM (MBeliD INNER JOIN MBeli ON MBeli.NoID=MBeliD.IDBeli)" & vbCrLf & _
                                     " LEFT JOIN MSatuan ON MSatuan.NoID=MBeliD.IDSatuan" & vbCrLf & _
                                     " LEFT JOIN MBarang ON MBarang.NoID=MBeliD.IDBarang" & vbCrLf & _
                                     " LEFT JOIN (MGudang LEFT JOIN MWilayah ON MWilayah.NoID=Mgudang.IDWilayah) ON MGudang.NoID=MBeliD.IDGudang" & vbCrLf & _
                                     " LEFT JOIN MAlamat ON MAlamat.NoID=MBeli.IDSupplier " & vbCrLf & _
                                     " WHERE MBeliD.IDPOD=" & IDTransaksi
                        .TransaksiHeader = "PO di Pembelian"
                    Case "MLPB".ToUpper
                        .SQLString = "SELECT MBeliD.NoID, MBeliD.IDBeli, MBeli.Kode, MBeli.Tanggal, MAlamat.Nama AS Supplier, " & vbCrLf & _
                                     " MGudang.Nama AS Gudang, MWilayah.Nama AS Wilayah, MBarang.Kode AS KodeStock, MBarang.Nama AS NamaStock, MBarang.KodeAlias AS KodeAlias, MBarang.NamaAlias, " & vbCrLf & _
                                     " MBeliD.Qty, MBeliD.Ctn, MSatuan.Nama AS Satuan, MBeliD.Konversi, MBeliD.Harga, MBeliD.Harga/MBeliD.Konversi AS [HargaPcs], MBeliD.Qty*MBeliD.Konversi AS [QtyPcs]" & vbCrLf & _
                                     " FROM (MBeliD INNER JOIN MBeli ON MBeli.NoID=MBeliD.IDBeli)" & vbCrLf & _
                                     " LEFT JOIN MSatuan ON MSatuan.NoID=MBeliD.IDSatuan" & vbCrLf & _
                                     " LEFT JOIN MBarang ON MBarang.NoID=MBeliD.IDBarang" & vbCrLf & _
                                     " LEFT JOIN (MGudang LEFT JOIN MWilayah ON MWilayah.NoID=Mgudang.IDWilayah) ON MGudang.NoID=MBeliD.IDGudang" & vbCrLf & _
                                     " LEFT JOIN MAlamat ON MAlamat.NoID=MBeli.IDSupplier " & vbCrLf & _
                                     " WHERE MBeliD.IDLPBD=" & IDTransaksi
                        .TransaksiHeader = "STB di Pembelian"
                    Case "MSO".ToUpper
                        .SQLString = "SELECT MSPKD.NoID, MSPKD.IDSPK, MSPK.Kode, MSPK.Tanggal, MAlamat.Nama AS Supplier, " & vbCrLf & _
                                     " MGudang.Nama AS Gudang, MWilayah.Nama AS Wilayah, MBarang.Kode AS KodeStock, MBarang.Nama AS NamaStock, MBarang.KodeAlias AS KodeAlias, MBarang.NamaAlias, " & vbCrLf & _
                                     " MSPKD.Qty, MSPKD.Ctn, MSatuan.Nama AS Satuan, MSPKD.Konversi, MSPKD.Harga, MSPKD.Harga/MSPKD.Konversi AS [HargaPcs], MSPKD.Qty*MSPKD.Konversi AS [QtyPcs]" & vbCrLf & _
                                     " FROM (MSPKD INNER JOIN MSPK ON MSPK.NoID=MSPKD.IDSPK)" & vbCrLf & _
                                     " LEFT JOIN MSatuan ON MSatuan.NoID=MSPKD.IDSatuan" & vbCrLf & _
                                     " LEFT JOIN MBarang ON MBarang.NoID=MSPKD.IDBarang" & vbCrLf & _
                                     " LEFT JOIN (MGudang LEFT JOIN MWilayah ON MWilayah.NoID=MGudang.IDWilayah) ON MGudang.NoID=MSPKD.IDGudang" & vbCrLf & _
                                     " LEFT JOIN MAlamat ON MAlamat.NoID=MSPK.IDCustomer " & vbCrLf & _
                                     " WHERE MSPKD.IDSOD=" & IDTransaksi
                        .TransaksiHeader = "SO di Surat Perintah Kerja"
                    Case "MSPK".ToUpper
                        .SQLString = "SELECT MPackingD.NoID, MPackingD.IDPacking, MPacking.Kode, MPacking.Tanggal, MAlamat.Nama AS Supplier, " & vbCrLf & _
                                     " MGudang.Nama AS Gudang, MWilayah.Nama AS Wilayah, MBarang.Kode AS KodeStock, MBarang.Nama AS NamaStock, MBarang.KodeAlias AS KodeAlias, MBarang.NamaAlias, " & vbCrLf & _
                                     " MPackingD.Qty, MPackingD.Ctn, MSatuan.Nama AS Satuan, MPackingD.Konversi, MPackingD.Harga, MPackingD.Harga/MPackingD.Konversi AS [HargaPcs], MPackingD.Qty*MPackingD.Konversi AS [QtyPcs]" & vbCrLf & _
                                     " FROM (MPackingD INNER JOIN MPacking ON MPacking.NoID=MPackingD.IDPacking)" & vbCrLf & _
                                     " LEFT JOIN MSatuan ON MSatuan.NoID=MPackingD.IDSatuan" & vbCrLf & _
                                     " LEFT JOIN MBarang ON MBarang.NoID=MPackingD.IDBarang" & vbCrLf & _
                                     " LEFT JOIN (MGudang LEFT JOIN MWilayah ON MWilayah.NoID=MGudang.IDWilayah) ON MGudang.NoID=MPackingD.IDGudang" & vbCrLf & _
                                     " LEFT JOIN MAlamat ON MAlamat.NoID=MPacking.IDCustomer " & vbCrLf & _
                                     " WHERE MPackingD.IDSPKD=" & IDTransaksi
                        .TransaksiHeader = "SPK di Surat Packing"
                    Case "MPacking".ToUpper
                        .SQLString = "SELECT MJualD.NoID, MJualD.IDJual, MJual.Kode, MJual.Tanggal, MAlamat.Nama AS Supplier, " & vbCrLf & _
                                     " MGudang.Nama AS Gudang, MWilayah.Nama AS Wilayah, MBarang.Kode AS KodeStock, MBarang.Nama AS NamaStock, MBarang.KodeAlias AS KodeAlias, MBarang.NamaAlias, " & vbCrLf & _
                                     " MJualD.Qty, MJualD.Ctn, MSatuan.Nama AS Satuan, MJualD.Konversi, MJualD.Harga, MJualD.Harga/MJualD.Konversi AS [HargaPcs], MJualD.Qty*MJualD.Konversi AS [QtyPcs]" & vbCrLf & _
                                     " FROM (MJualD INNER JOIN MJual ON MJual.NoID=MJualD.IDJual)" & vbCrLf & _
                                     " LEFT JOIN MSatuan ON MSatuan.NoID=MJualD.IDSatuan" & vbCrLf & _
                                     " LEFT JOIN MBarang ON MBarang.NoID=MJualD.IDBarang" & vbCrLf & _
                                     " LEFT JOIN (MGudang LEFT JOIN MWilayah ON MWilayah.NoID=MGudang.IDWilayah) ON MGudang.NoID=MJualD.IDGudang" & vbCrLf & _
                                     " LEFT JOIN MAlamat ON MAlamat.NoID=MJual.IDCustomer " & vbCrLf & _
                                     " WHERE MJualD.IDPackingD=" & IDTransaksi
                        .TransaksiHeader = "SPK di Surat Jual"
                End Select
                .RefreshData()
            End With
        Catch ex As Exception
            XtraMessageBox.Show("Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub
    Public Shared Sub CariGudangSPK(ByRef IDGudang As Long, ByRef QtyPcs As Double)
        Try

        Catch ex As Exception

        End Try
    End Sub
End Class
