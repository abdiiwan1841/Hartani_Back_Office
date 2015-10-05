Imports DevExpress.XtraEditors
Module mdlAccEnum
    Public Enum cTypetransaksi
        SaldoAwal = 1
        Purchase = 2
        PurchaseReturn = 3
        MutasiKeluar = 4
        MutasiMasuk = 5
        Sales = 6
        SalesReturn = 7
        PemakaianBarang = 8
        StokOpname = 9
        RubahHargaBeli = 11
        TransferOut = 12
        TransferIN = 13
        PenyesuaianBarang = 14
        RubahHargaJual = 15
        TransferKode = 16
        MutasiKeluarInternal = 17
        MutasiMasukInternal = 18
        NotaDebet = 19
        NotaKredit = 20
        TransferUang = 21
        PembayaranHutang = 22
        PembayaranPiutang = 23
        PengembalianTU = 24
        MutasiKeluarIntransit = 25
        MutasiMasukIntransit = 26
        RubahHPP = 27
    End Enum
    Public TypeTransaksi As cTypetransaksi
    '    1	Saldo Awal
    '2	Purchase
    '3	Purchase Return
    '4	Mutasi Keluar
    '5	Mutasi Masuk
    '6	Sales
    '7	Sales Return
    '8	Pemakaian Barang
    '9	Stok Opname
    '11	Rubah Harga Beli
    '12	Transfer Out
    '13	Transfer IN
    '14	Penyesuaian Barang
    '15	Rubah Harga Jual
    '16	Transfer Kode
    '17	Mutasi Keluar (Internal)
    '18	Mutasi Masuk (Internal)
    '19	Nota Debet
    '20	Nota Kredit
    '21	Transfer Uang
    '22	Pembayaran Hutang
    '23	Pembayaran Piutang
    '24	Pengembalian TU
    Public Function GetHargaNetto(ByVal IDBarang As Long, ByVal Tgl As Date) As Double
        Dim x As Double = 0.0
        Try
            x = NullToDbl(EksekusiSQlSkalarNew("SELECT Top 1 MKartuStok.HargaNetto/MKartuStok.Konversi " & _
                                               " FROM MKartuStok " & _
                                               " LEFT JOIN MBarang ON MBarang.NoID=MKartuStok.IDBarang " & _
                                               " LEFT JOIN MSatuan ON MSatuan.NoID=MKartuStok.IDSatuan " & _
                                               " LEFT JOIN MGudang ON MGudang.NoID=MKartuStok.IDGUdang " & _
                                               " LEFT JOIN MJenisTransaksi ON MJenisTransaksi.ID=MKartuStok.IDJenisTransaksi " & _
                                               " WHERE IsNull(MKartuStok.HargaNetto,0)<>0 AND MKartuStok.Tanggal<'" & Tgl.ToString("yyyy-MM-dd") & "' AND MBarang.NoID=" & IDBarang & _
                                               " ORDER BY MKartuStok.IDBarang, YEAR(MKartuStok.Tanggal) DESC, MONTH(MKartuStok.Tanggal) DESC, DAY(MKartuStok.Tanggal) DESC, MJenisTransaksi.NoUrut DESC"))
            If x = 0 Then
                x = GetHargaBeliNettoTerakhirPcs(IDBarang, Tgl)
            End If
        Catch ex As Exception
            XtraMessageBox.Show("Info Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End Try
        Return x
    End Function
    Public Function GetHargaBeliNettoTerakhirPcs(ByVal IDBarang As Long, ByVal TglSebelum As Date) As Double
        Dim x As Double = 0
        Dim SQL As String = ""
        Dim ds As New DataSet
        Dim ds1 As New DataSet
        Dim Harga As Double = 0.0, Konversi As Double = 0.0
        Dim DP1 As Double = 0.0, DR1 As Double = 0.0
        Dim DP2 As Double = 0.0, DR2 As Double = 0.0
        Dim DP3 As Double = 0.0, DR3 As Double = 0.0
        Try
            SQL = "SELECT TOP 1 MBeliD.NoID, MBeliD.Harga/MBeliD.Konversi AS Harga, MBeliD.Konversi, MBeliD.DiscPersen1, MBeliD.DiscPersen2, MBeliD.DiscPersen3, MBeliD.Disc1, MBeliD.Disc2, MBeliD.Disc3, MBeli.DiskonNotaProsen, MBeli.DiskonNotaRp FROM MBeliD INNER JOIN MBeli ON MBeli.NoID=MBeliD.IDBeli WHERE MBeli.Tanggal<'" & TglSebelum.AddDays(1).ToString("yyyy-MM-dd") & "' AND MBeliD.IDBarang=" & IDBarang & " Order By MBeli.Tanggal Desc, MBeliD.Tgl Desc"
            ds = ExecuteDataset("MBeli", SQL)
            If ds.Tables("MBeli").Rows.Count >= 1 Then
                SQL = "SELECT TOP 1 MRevisiHargaBeliD.NoID, MRevisiHargaBeliD.HargaBaru/MRevisiHargaBeliD.Konversi AS Harga, MRevisiHargaBeliD.DiscPersen1, MRevisiHargaBeliD.DiscPersen2, MRevisiHargaBeliD.DiscPersen3, MRevisiHargaBeliD.Disc1, MRevisiHargaBeliD.Disc2, MRevisiHargaBeliD.Disc3 " & _
                      " FROM (MRevisiHargaBeliD INNER JOIN MRevisiHargaBeli ON MRevisiHargaBeli.NoID=MRevisiHargaBeliD.IDRevisiHargaBeli) LEFT JOIN (MBeliD INNER JOIN MBeli ON MBeli.NoID=MBeliD.IDBeli) ON MBeliD.NoID=MRevisiHargaBeliD.IDBeliD AND MBeli.NoID=MRevisiHargaBeliD.IDBeli " & _
                      " WHERE MBeliD.NOID=" & NullToLong(ds.Tables("MBeli").Rows(0).Item("NoID")) & _
                      " Order By MRevisiHargaBeli.Tanggal Desc, MRevisiHargaBeliD.NoID Desc"
                ds1 = ExecuteDataset("MRevisiHargaBeli", SQL)
                If ds1.Tables("MRevisiHargaBeli").Rows.Count >= 1 Then
                    x = NullToDbl(ds1.Tables("MRevisiHargaBeli").Rows(0).Item("Harga"))
                    DP1 = NullToDbl(ds1.Tables("MRevisiHargaBeli").Rows(0).Item("DiscPersen1"))
                    DP2 = NullToDbl(ds1.Tables("MRevisiHargaBeli").Rows(0).Item("DiscPersen2"))
                    DP3 = NullToDbl(ds1.Tables("MRevisiHargaBeli").Rows(0).Item("DiscPersen3"))
                    DR1 = NullToDbl(ds1.Tables("MRevisiHargaBeli").Rows(0).Item("Disc1")) + NullToDbl(ds1.Tables("MRevisiHargaBeli").Rows(0).Item("Disc2")) + NullToDbl(ds1.Tables("MRevisiHargaBeli").Rows(0).Item("Disc3"))
                    DR2 = 0
                    DR3 = 0
                    x = (Bulatkan((Harga * Konversi) * (1 - (DP1 / 100)) * (1 - (DP2 / 100)) * (1 - (DP3 / 100)), 0) - DR1 - DR2 - DR3)
                    x = (x * (1 - (NullToDbl(ds.Tables("MBeli").Rows(0).Item("DiskonNotaProsen")) / 100))) - NullToDbl(ds.Tables("MBeli").Rows(0).Item("DiskonNotaRp"))
                    x = x / Konversi
                Else
                    If NullToDbl(ds.Tables("MBeli").Rows(0).Item("DiskonNotaProsen")) > 0 Or _
                    NullToDbl(ds.Tables("MBeli").Rows(0).Item("DiskonNotaRp")) > 0 Then
                        'Netto 17/01/2012
                        'txtJUmlah.EditValue = txtQty.EditValue * (Bulatkan((txtHarga.EditValue * (1 - (txtDiscPersen1.EditValue / 100)) * (1 - (txtDiscPersen2.EditValue / 100)) * (1 - (txtDiscPersen3.EditValue / 100))), 0) - txtDiscRp1.EditValue - txtDiscRp2.EditValue - txtDiscRp3.EditValue)
                        Harga = NullToDbl(ds.Tables("MBeli").Rows(0).Item("Harga"))
                        Konversi = NullToDbl(ds.Tables("MBeli").Rows(0).Item("Konversi"))
                        DP1 = NullToDbl(ds.Tables("MBeli").Rows(0).Item("DiscPersen1"))
                        DP2 = NullToDbl(ds.Tables("MBeli").Rows(0).Item("DiscPersen2"))
                        DP3 = NullToDbl(ds.Tables("MBeli").Rows(0).Item("DiscPersen3"))
                        DR1 = NullToDbl(ds.Tables("MBeli").Rows(0).Item("Disc1"))
                        DR2 = NullToDbl(ds.Tables("MBeli").Rows(0).Item("Disc2"))
                        DR3 = NullToDbl(ds.Tables("MBeli").Rows(0).Item("Disc3"))
                        x = (Bulatkan((Harga * Konversi) * (1 - (DP1 / 100)) * (1 - (DP2 / 100)) * (1 - (DP3 / 100)), 0) - DR1 - DR2 - DR3)
                        x = (x * (1 - (NullToDbl(ds.Tables("MBeli").Rows(0).Item("DiskonNotaProsen")) / 100))) - NullToDbl(ds.Tables("MBeli").Rows(0).Item("DiskonNotaRp"))
                        x = x / Konversi
                    Else
                        Harga = NullToDbl(ds.Tables("MBeli").Rows(0).Item("Harga"))
                        Konversi = NullToDbl(ds.Tables("MBeli").Rows(0).Item("Konversi"))
                        DP1 = NullToDbl(ds.Tables("MBeli").Rows(0).Item("DiscPersen1"))
                        DP2 = NullToDbl(ds.Tables("MBeli").Rows(0).Item("DiscPersen2"))
                        DP3 = NullToDbl(ds.Tables("MBeli").Rows(0).Item("DiscPersen3"))
                        DR1 = NullToDbl(ds.Tables("MBeli").Rows(0).Item("Disc1")) + NullToDbl(ds.Tables("MBeli").Rows(0).Item("Disc2")) + NullToDbl(ds.Tables("MBeli").Rows(0).Item("Disc3"))
                        DR2 = 0
                        DR3 = 0
                        x = (Bulatkan((Harga * Konversi) * (1 - (DP1 / 100)) * (1 - (DP2 / 100)) * (1 - (DP3 / 100)), 0) - DR1 - DR2 - DR3)
                        x = (x * (1 - (NullToDbl(ds.Tables("MBeli").Rows(0).Item("DiskonNotaProsen")) / 100))) - NullToDbl(ds.Tables("MBeli").Rows(0).Item("DiskonNotaRp"))
                        x = x / Konversi
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
    Public Function GetTotHPP(ByVal IdBarang As Long, ByVal Tanggal As Date, ByVal ID As Long) As Double
        Dim ds As New DataSet
        Dim StrCekRecord As String
        Try
            StrCekRecord = "SELECT TOP 1 MKartuStok.HPP " & _
                           " From MKartuStok INNER JOIN MJenisTransaksi ON MJenisTransaksi.ID=MKartuStok.IDJenisTransaksi " & _
                           " WHERE IsNull(MKartuStok.HPP,0)>0 AND (MKartuStok.NoID<=" & ID & ") AND (MKartuStok.Tanggal <= CONVERT(DATETIME, '" & Tanggal.AddDays(1).ToString("yyyy/MM/dd") & "', 101)) AND (MKartuStok.IDBarang = " & IdBarang & ") " & _
                           " ORDER BY MKartuStok.Tanggal DESC, MKartuStok.NoID DESC, MJenisTransaksi.NoUrut DESC"
            ds = ExecuteDataset("MHPP", StrCekRecord)
            If ds.Tables("MHPP").Rows.Count = 0 Then
                GetTotHPP = 0
            Else
                If NullToDbl(ds.Tables("MHPP").Rows(0).Item("HPP")) < 0 Then
                    GetTotHPP = 0
                Else
                    GetTotHPP = NullToDbl(ds.Tables("MHPP").Rows(0).Item("HPP"))
                End If
            End If
        Catch ex As Exception
            MsgBox("Info Kesalahan : " & ex.Message, MsgBoxStyle.Critical, NamaAplikasi)
        Finally
            ds.Dispose()
        End Try
    End Function
    Public Function GetTotSaldoAkhir(ByVal IdBarang As Long, ByVal Tanggal As Date, ByVal ID As Long) As Double
        Dim ds As New DataSet
        Dim StrCekRecord As String
        Try
            StrCekRecord = "SELECT TOP 1 MKartuStok.QtyAkhir " & _
                           " From MKartuStok INNER JOIN MJenisTransaksi ON MJenisTransaksi.ID=MKartuStok.IDJenisTransaksi " & _
                           " WHERE (MKartuStok.NoID<=" & ID & ") AND (MKartuStok.Tanggal <= CONVERT(DATETIME, '" & Tanggal.AddDays(1).ToString("yyyy/MM/dd") & "', 101)) AND (MKartuStok.IDBarang = " & IdBarang & ") " & _
                           " ORDER BY MKartuStok.Tanggal DESC, MKartuStok.NoID DESC, MJenisTransaksi.NoUrut DESC"
            ds = ExecuteDataset("MHPP", StrCekRecord)
            If ds.Tables("MHPP").Rows.Count = 0 Then
                GetTotSaldoAkhir = 0
            Else
                GetTotSaldoAkhir = NullToDbl(ds.Tables("MHPP").Rows(0).Item("QtyAkhir"))
            End If
        Catch ex As Exception
            MsgBox("Info Kesalahan : " & ex.Message, MsgBoxStyle.Critical, NamaAplikasi)
        Finally
            ds.Dispose()
        End Try
    End Function
End Module
