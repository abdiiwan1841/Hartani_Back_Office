Imports System.Data
Imports System.Data.SqlClient
Imports DevExpress.XtraEditors
Imports System.Data.SQLite

Module mdlPostingAccounting
    Dim SQL As String = ""
    Public Sub TambahKolomTableViewPostinganAcc()
        Dim cn As New SqlConnection
        Dim com As New SqlCommand
        Try
            cn.ConnectionString = StrKonSql
            cn.Open()
            com.Connection = cn
            Try
                SQL = "CREATE VIEW vKartuJurnal AS" & vbCrLf & _
                      " SELECT MJurnalD.ID, MJurnalD.IDJurnal, CONVERT(DATE, MJurnal.Tanggal, 101) AS Tanggal, MJurnalD.IDDepartemen, MJurnalD.IDAkun, MJurnalD.IDMataUang, MJurnalD.Kurs, MJurnalD.Debet, MJurnalD.Kredit, " & vbCrLf & _
                      " ISNULL((SELECT SUM(B.Debet-B.Kredit) FROM MJurnalD B INNER JOIN MJurnal A ON A.ID=B.IDJurnal AND B.IDAkun=MJurnalD.IDAkun WHERE A.IDJenisTransaksi<>0 AND (CONVERT(DATE, A.Tanggal,101)<CONVERT(DATE, MJurnal.Tanggal, 101) OR (CONVERT(DATE, A.Tanggal,101)<=CONVERT(DATE, MJurnal.Tanggal, 101) AND B.ID<MJurnalD.ID))),0) AS SaldoAwal" & vbCrLf & _
                      " FROM MJurnalD " & vbCrLf & _
                      " INNER JOIN MJurnal ON MJurnal.ID=MJurnalD.IDJurnal" & vbCrLf & _
                      " WHERE MJurnal.IDJenisTransaksi<>0"
                com.CommandText = SQL
                com.ExecuteNonQuery()
            Catch ex As Exception

            End Try
            Try
                SQL = "ALTER TABLE MJurnal ADD " & vbCrLf & _
                      " IDAlamat INT NULL"
                com.CommandText = SQL
                com.ExecuteNonQuery()
            Catch ex As Exception

            End Try
            Try
                SQL = "ALTER TABLE MJurnalD ADD " & vbCrLf & _
                      " IDAlamat INT NULL"
                com.CommandText = SQL
                com.ExecuteNonQuery()
            Catch ex As Exception

            End Try
            Try
                SQL = "ALTER TABLE MKasOutD ADD " & vbCrLf & _
                      " IDKasKeluar INT NULL"
                com.CommandText = SQL
                com.ExecuteNonQuery()
            Catch ex As Exception

            End Try
            Try
                SQL = "ALTER TABLE TKasOutD ADD " & vbCrLf & _
                      " IDKasKeluar INT NULL"
                com.CommandText = SQL
                com.ExecuteNonQuery()
            Catch ex As Exception

            End Try
            Try
                SQL = "ALTER TABLE MReturBeli ADD " & vbCrLf & _
                      " IDAkunBiayaPembelian INT NULL"
                com.CommandText = SQL
                com.ExecuteNonQuery()
            Catch ex As Exception

            End Try
            Try
                SQL = "ALTER TABLE MReturBeliD ADD " & vbCrLf & _
                      " DiskonNotaProsen Numeric(18,2) NULL, " & _
                      " DiskonNotaRp Money NULL"
                com.CommandText = SQL
                com.ExecuteNonQuery()
            Catch ex As Exception

            End Try
            Try
                SQL = "ALTER TABLE MBeliD ADD " & vbCrLf & _
                      " DiskonNotaProsen Numeric(18,2) NULL, " & _
                      " DiskonNotaRp Money NULL"
                com.CommandText = SQL
                com.ExecuteNonQuery()
            Catch ex As Exception

            End Try
            Try
                SQL = "ALTER TABLE MSettingAkun ADD " & vbCrLf & _
                      " IDAkunLRBerjalan INT NULL "
                com.CommandText = SQL
                com.ExecuteNonQuery()
            Catch ex As Exception

            End Try
            Try
                SQL = "CREATE TABLE [MSettingAkun]( " & vbCrLf & _
                      " [IDAkunPembelian] [int] NULL," & vbCrLf & _
                      " [IDAkunReturPembelian] [int] NULL," & vbCrLf & _
                      " [IDAkunSelisihBalancing] [int] NULL," & vbCrLf & _
                      " [IDAkunPenjualan] [int] NULL," & vbCrLf & _
                      " [IDAkunReturPenjualan] [int] NULL," & vbCrLf & _
                      " [IDAkunBiayaPembelian] [int] NULL, " & vbCrLf & _
                      " [IDAkunBiayaPenjualan] [int] NULL," & vbCrLf & _
                      " [IDAkunKasPembelian] [int] NULL," & vbCrLf & _
                      " [IDAkunKasPenjualan] [int] NULL" & vbCrLf & _
                      " ) ON [PRIMARY]"
                com.CommandText = SQL
                com.ExecuteNonQuery()
            Catch ex As Exception

            End Try
            Try
                SQL = "ALTER TABLE [MSettingAkun] ADD " & vbCrLf & _
                      " [IDAkunPembelian] [int] NULL " & vbCrLf & _
                      ""
                com.CommandText = SQL
                com.ExecuteNonQuery()
            Catch ex As Exception

            End Try
            Try
                SQL = "ALTER TABLE [MSettingAkun] ADD " & vbCrLf & _
                      " [IDAkunReturPembelian] [int] NULL," & vbCrLf & _
                      ""
                com.CommandText = SQL
                com.ExecuteNonQuery()
            Catch ex As Exception

            End Try
            Try
                SQL = "ALTER TABLE [MSettingAkun] ADD " & vbCrLf & _
                      " [IDAkunSelisihBalancing] [int] NULL," & vbCrLf & _
                      ""
                com.CommandText = SQL
                com.ExecuteNonQuery()
            Catch ex As Exception

            End Try
            Try
                SQL = "ALTER TABLE [MSettingAkun] ADD " & vbCrLf & _
                      " [IDAkunPenjualan] [int] NULL," & vbCrLf & _
                      ""
                com.CommandText = SQL
                com.ExecuteNonQuery()
            Catch ex As Exception

            End Try
            Try
                SQL = "ALTER TABLE [MSettingAkun] ADD " & vbCrLf & _
                      " [IDAkunReturPenjualan] [int] NULL, " & vbCrLf & _
                      ""
                com.CommandText = SQL
                com.ExecuteNonQuery()
            Catch ex As Exception

            End Try
            Try
                SQL = "ALTER TABLE [MSettingAkun] ADD " & vbCrLf & _
                      " [IDAkunBiayaPembelian] [int] NULL, " & vbCrLf & _
                      ""
                com.CommandText = SQL
                com.ExecuteNonQuery()
            Catch ex As Exception

            End Try
            Try
                SQL = "ALTER TABLE [MSettingAkun] ADD " & vbCrLf & _
                      " [IDAkunBiayaPenjualan] [int] NULL," & vbCrLf & _
                      ""
                com.CommandText = SQL
                com.ExecuteNonQuery()
            Catch ex As Exception

            End Try
            Try
                SQL = "ALTER TABLE [MSettingAkun] ADD " & vbCrLf & _
                      " [IDAkunKasPembelian] [int] NULL," & vbCrLf & _
                      ""
                com.CommandText = SQL
                com.ExecuteNonQuery()
            Catch ex As Exception

            End Try
            Try
                SQL = "ALTER TABLE [MSettingAkun] ADD " & vbCrLf & _
                      " [IDAkunKasPenjualan] [int] NULL" & vbCrLf & _
                      ""
                com.CommandText = SQL
                com.ExecuteNonQuery()
            Catch ex As Exception

            End Try
            Try
                SQL = "ALTER TABLE MBayarHutang ADD " & _
                      " IsPostedJurnal BIT NULL, " & _
                      " IDUserPostedJurnal INT NULL, " & _
                      " TglPostedJurnal Datetime NULL"
                com.CommandText = SQL
                com.ExecuteNonQuery()
            Catch ex As Exception

            End Try
            Try
                SQL = "ALTER TABLE MBeli ADD " & _
                      " IsPostedJurnal BIT NULL, " & _
                      " IDUserPostedJurnal INT NULL, " & _
                      " TglPostedJurnal Datetime NULL"
                com.CommandText = SQL
                com.ExecuteNonQuery()
            Catch ex As Exception

            End Try
            Try
                SQL = "ALTER TABLE MReturBeli ADD " & _
                      " IsPostedJurnal BIT NULL, " & _
                      " IDUserPostedJurnal INT NULL, " & _
                      " TglPostedJurnal Datetime NULL"
                com.CommandText = SQL
                com.ExecuteNonQuery()
            Catch ex As Exception

            End Try
            Try
                SQL = "ALTER TABLE MJual ADD " & _
                      " IsPostedJurnal BIT NULL, " & _
                      " IDUserPostedJurnal INT NULL, " & _
                      " TglPostedJurnal Datetime NULL"
                com.CommandText = SQL
                com.ExecuteNonQuery()
            Catch ex As Exception

            End Try
            Try
                SQL = "ALTER TABLE MReturJual ADD " & _
                      " IsPostedJurnal BIT NULL, " & _
                      " IDUserPostedJurnal INT NULL, " & _
                      " TglPostedJurnal Datetime NULL"
                com.CommandText = SQL
                com.ExecuteNonQuery()
            Catch ex As Exception

            End Try
            Try
                SQL = "ALTER TABLE MBayarHutang ADD " & _
                      " IsPostedJurnal BIT NULL, " & _
                      " IDUserPostedJurnal INT NULL, " & _
                      " TglPostedJurnal Datetime NULL"
                com.CommandText = SQL
                com.ExecuteNonQuery()
            Catch ex As Exception

            End Try
            Try
                SQL = " CREATE VIEW [vDaftarPembayaranHutangAccounting] AS " & vbCrLf & _
                      " SELECT MBayarHutang.NoID, MBayarHutang.Kode, MBayarHutang.Tanggal, MBayarHutang.TglKembali AS JatuhTempo, " & vbCrLf & _
                      " MBayarHutang.SubTotal, MBayarHutang.DN, MBayarHutang.Potongan AS CN, MBayarHutang.Materai, " & vbCrLf & _
                      " MBayarHutang.JumlahKwitansi, MBayarHutang.Total, MBayarHutang.IsPosted AS PostingHutang, MSupplier.Kode + ' - ' + MSupplier.Nama AS Supplier, MUserAcc.Nama AS UserPostingJurnal, MBayarHutang.TglPostedJurnal AS TglPostingJurnal, MBayarHutang.IsPostedJurnal AS PostingJurnal " & vbCrLf & _
                      " FROM MBayarHutang" & vbCrLf & _
                      " LEFT JOIN MAlamat MSupplier ON MSupplier.NoID=MBayarHutang.IDAlamat " & vbCrLf & _
                      " LEFT JOIN MUser MUserAcc ON MUserAcc.NoID=MBayarHutang.IDUserPostedJurnal " & vbCrLf & _
                      " WHERE MBayarHUtang.IsPosted=1"
                com.CommandText = SQL
                com.ExecuteNonQuery()
            Catch ex As Exception

            End Try
            Try
                SQL = " CREATE VIEW [vDaftarPembelianAccounting] AS " & vbCrLf & _
                      " SELECT MBeli.NoID, MBeli.Kode AS NoBPB, (SELECT TOP 1 MPO.Kode FROM MPO INNER JOIN MPOD ON MPO.NoID=MPOD.IDPO INNER JOIN MBeliD ON MBeliD.IDPOD=MPOD.NoID WHERE MBeliD.IDBeli=MBeli.NoID) AS NoSPP, MBeli.Tanggal, MBeli.JatuhTempo, MBeli.Total, MBeli.IsPosted AS PostingStock, MSupplier.Kode + ' - ' + MSupplier.Nama AS Supplier, MUserAcc.Nama AS UserPostingJurnal, MBeli.TglPostedJurnal AS TglPostingJurnal, MBeli.IsPostedJurnal AS PostingJurnal FROM MBeli  " & vbCrLf & _
                      " LEFT JOIN MAlamat MSupplier ON MSupplier.NoID=MBeli.IDSupplier  " & vbCrLf & _
                      " LEFT JOIN MUser MUserAcc ON MUserAcc.NoID=MBeli.IDUserPostedJurnal  " & vbCrLf & _
                      " WHERE MBeli.IsPosted=1 "
                com.CommandText = SQL
                com.ExecuteNonQuery()
            Catch ex As Exception

            End Try
            Try
                SQL = " CREATE VIEW [vDaftarReturPembelianAccounting] AS " & vbCrLf & _
                      " SELECT MReturBeli.NoID, MReturBeli.Kode AS NoRetur, MReturBeli.KodeReff, MReturBeli.Tanggal, MReturBeli.JatuhTempo, MReturBeli.Total, MReturBeli.IsPosted AS PostingStock, MSupplier.Kode + ' - ' + MSupplier.Nama AS Supplier, MUserAcc.Nama AS UserPostingJurnal, MReturBeli.TglPostedJurnal AS TglPostingJurnal, MReturBeli.IsPostedJurnal AS PostingJurnal FROM MReturBeli  " & vbCrLf & _
                      " LEFT JOIN MAlamat MSupplier ON MSupplier.NoID=MReturBeli.IDSupplier  " & vbCrLf & _
                      " LEFT JOIN MUser MUserAcc ON MUserAcc.NoID=MReturBeli.IDUserPostedJurnal  " & vbCrLf & _
                      " WHERE MReturBeli.IsPosted=1 "
                com.CommandText = SQL
                com.ExecuteNonQuery()
            Catch ex As Exception

            End Try
            Try
                SQL = " CREATE VIEW [vDaftarPenjualanAccounting] AS " & vbCrLf & _
                      " SELECT MJual.NoID, MJual.Kode AS NoRetur, MJual.KodeReff, MJual.Tanggal, MJual.JatuhTempo, MJual.Total, MJual.IsPosted AS PostingStock, MCustomer.Kode + ' - ' + MCustomer.Nama AS Customer, MUserAcc.Nama AS UserPostingJurnal, MJual.TglPostedJurnal AS TglPostingJurnal, MJual.IsPostedJurnal AS PostingJurnal FROM MJual  " & vbCrLf & _
                      " LEFT JOIN MAlamat MCustomer ON MCustomer.NoID=MJual.IDCustomer  " & vbCrLf & _
                      " LEFT JOIN MUser MUserAcc ON MUserAcc.NoID=MJual.IDUserPostedJurnal  " & vbCrLf & _
                      " WHERE MJual.IsPosted=1 "
                com.CommandText = SQL
                com.ExecuteNonQuery()
            Catch ex As Exception

            End Try
            Try
                SQL = " CREATE VIEW [vDaftarReturPenjualanAccounting] AS " & vbCrLf & _
                      " SELECT MReturJual.NoID, MReturJual.Kode AS NoRetur, MReturJual.KodeReff, MReturJual.Tanggal, MReturJual.JatuhTempo, MReturJual.Total, MReturJual.IsPosted AS PostingStock, MCustomer.Kode + ' - ' + MCustomer.Nama AS Customer, MUserAcc.Nama AS UserPostingJurnal, MReturJual.TglPostedJurnal AS TglPostingJurnal, MReturJual.IsPostedJurnal AS PostingJurnal FROM MReturJual  " & vbCrLf & _
                      " LEFT JOIN MAlamat MCustomer ON MCustomer.NoID=MReturJual.IDCustomer  " & vbCrLf & _
                      " LEFT JOIN MUser MUserAcc ON MUserAcc.NoID=MReturJual.IDUserPostedJurnal  " & vbCrLf & _
                      " WHERE MReturJual.IsPosted=1 "
                com.CommandText = SQL
                com.ExecuteNonQuery()
            Catch ex As Exception

            End Try
            Try
                SQL = "IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE ID = OBJECT_ID(N'[dbo].[vDaftarPembelianAccounting]') AND OBJECTPROPERTY(id, N'IsView') = 1)" & vbCrLf & _
                      " BEGIN " & vbCrLf & _
                      " CREATE VIEW [vDaftarPembelianAccounting] AS " & vbCrLf & _
                      " SELECT MBeli.NoID, MBeli.Kode AS NoBPB, (SELECT TOP 1 MPO.Kode FROM MPO INNER JOIN MPOD ON MPO.NoID=MPOD.IDPO INNER JOIN MBeliD ON MBeliD.IDPOD=MPOD.NoID WHERE MBeliD.IDBeli=MBeli.NoID) AS KodeReff, MBeli.Tanggal, MBeli.JatuhTempo, MBeli.Total, MBeli.IsPosted AS PostingStock, MSupplier.Kode + ' - ' + MSupplier.Nama AS Supplier, MUserAcc.Nama AS UserPostingJurnal, MBeli.TglPostedJurnal AS TglPostingJurnal, MBeli.IsPostedJurnal AS PostingJurnal FROM MBeli  " & vbCrLf & _
                      " LEFT JOIN MAlamat MSupplier ON MSupplier.NoID=MBeli.IDSupplier  " & vbCrLf & _
                      " LEFT JOIN MUser MUserAcc ON MUserAcc.NoID=MBeli.IDUserPostedJurnal  " & vbCrLf & _
                      " WHERE MBeli.IsPosted=1 " & vbCrLf & _
                      " END "
                com.CommandText = SQL
                com.ExecuteNonQuery()
            Catch ex As Exception

            End Try
        Catch ex As Exception
            XtraMessageBox.Show("Info Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
        End Try
    End Sub

    Public Sub TambahMenuPostinganAcc()
        Dim NoID As Long = -1
        Dim strSQL As String = ""
        Dim IDAcc As Long = -1
        Try
            If NullToLong(EksekusiSQlSkalarNew("Select NoID from MMenu where UPPER(Kode)=UPPER('mnkodesubklasakun')")) > 0 Then
            Else
                NoID = GetNewID("MMenu")
                strSQL = "INSERT INTO [MMenu] ([noid],[kode],[idparent],[caption],[shortcut],[KeyShortcut],[icon],[nourut],[isactive],[isawalgroup],[objectrun],[IDServerAsal],[IsBarSubItem]) Values(" & _
                NoID & ", 'mnkodesubklasakun', 7, 'Kode Subklasifikasi Rekening', NULL, NULL, 0, 3, 1, 0, 'DaftarSubKlasAkun', NULL, 0)"
                EksekusiSQL(strSQL)
            End If

            If NullToLong(EksekusiSQlSkalarNew("Select NoID from MMenu where UPPER(Kode)=UPPER('mnkodeklasakun')")) > 0 Then
            Else
                NoID = GetNewID("MMenu")
                strSQL = "INSERT INTO [MMenu] ([noid],[kode],[idparent],[caption],[shortcut],[KeyShortcut],[icon],[nourut],[isactive],[isawalgroup],[objectrun],[IDServerAsal],[IsBarSubItem]) Values(" & _
                NoID & ", 'mnkodeklasakun', 7, 'Kode Klasifikasi Rekening', NULL, NULL, 0, 3, 1, 0, 'DaftarKlasAkun', NULL, 0)"
                EksekusiSQL(strSQL)
            End If

            IDAcc = NullToLong(EksekusiSQlSkalarNew("SELECT NOID FROM MMenu WHERE UPPER(Kode)=UPPER('mnLaporanAccounting')"))
            If IDAcc >= 1 AndAlso NullToLong(EksekusiSQlSkalarNew("Select NoID from MMenu where UPPER(Kode)=UPPER('mnLaporanNeraca')")) > 0 Then
            Else
                NoID = GetNewID("MMenu")
                strSQL = "INSERT INTO [MMenu] ([noid],[kode],[idparent],[caption],[shortcut],[KeyShortcut],[icon],[nourut],[isactive],[isawalgroup],[objectrun],[IDServerAsal],[IsBarSubItem],[IDBarSubItem]) Values(" & _
                NoID & ", 'mnLaporanNeraca', 9, 'Laporan Neraca', NULL, NULL, 0, 1, 1, 0, 'LaporanNeraca', NULL, 0, " & IDAcc & ")"
                EksekusiSQL(strSQL)
            End If

            IDAcc = NullToLong(EksekusiSQlSkalarNew("SELECT NOID FROM MMenu WHERE UPPER(Kode)=UPPER('mnLaporanAccounting')"))
            If IDAcc >= 1 AndAlso NullToLong(EksekusiSQlSkalarNew("Select NoID from MMenu where UPPER(Kode)=UPPER('mnLaporanNeracaPercobaan')")) > 0 Then
            Else
                NoID = GetNewID("MMenu")
                strSQL = "INSERT INTO [MMenu] ([noid],[kode],[idparent],[caption],[shortcut],[KeyShortcut],[icon],[nourut],[isactive],[isawalgroup],[objectrun],[IDServerAsal],[IsBarSubItem],[IDBarSubItem]) Values(" & _
                NoID & ", 'mnLaporanNeracaPercobaan', 9, 'Laporan Neraca Percobaan', NULL, NULL, 0, 1, 1, 0, 'LaporanNeracaPercobaan', NULL, 0, " & IDAcc & ")"
                EksekusiSQL(strSQL)
            End If

            IDAcc = NullToLong(EksekusiSQlSkalarNew("SELECT NOID FROM MMenu WHERE UPPER(Kode)=UPPER('mnLaporanAccounting')"))
            If IDAcc >= 1 AndAlso NullToLong(EksekusiSQlSkalarNew("Select NoID from MMenu where UPPER(Kode)=UPPER('mnLaporanJurnalKosong')")) > 0 Then
            Else
                NoID = GetNewID("MMenu")
                strSQL = "INSERT INTO [MMenu] ([noid],[kode],[idparent],[caption],[shortcut],[KeyShortcut],[icon],[nourut],[isactive],[isawalgroup],[objectrun],[IDServerAsal],[IsBarSubItem],[IDBarSubItem]) Values(" & _
                NoID & ", 'mnLaporanJurnalKosong', 9, 'Laporan Jurnal Kosong', NULL, NULL, 0, 1, 1, 0, 'LaporanJurnalKosong', NULL, 0, " & IDAcc & ")"
                EksekusiSQL(strSQL)
            End If

            IDAcc = NullToLong(EksekusiSQlSkalarNew("SELECT NOID FROM MMenu WHERE UPPER(Kode)=UPPER('mnLaporanAccounting')"))
            If IDAcc >= 1 AndAlso NullToLong(EksekusiSQlSkalarNew("Select NoID from MMenu where UPPER(Kode)=UPPER('mnLaporanJurnalTidakBalance')")) > 0 Then
            Else
                NoID = GetNewID("MMenu")
                strSQL = "INSERT INTO [MMenu] ([noid],[kode],[idparent],[caption],[shortcut],[KeyShortcut],[icon],[nourut],[isactive],[isawalgroup],[objectrun],[IDServerAsal],[IsBarSubItem],[IDBarSubItem]) Values(" & _
                NoID & ", 'mnLaporanJurnalTidakBalance', 9, 'Laporan Jurnal Tidak Balance', NULL, NULL, 0, 1, 1, 0, 'LaporanJurnalTidakBalance', NULL, 0, " & IDAcc & ")"
                EksekusiSQL(strSQL)
            End If

            'ACC
            If NullToLong(EksekusiSQlSkalarNew("Select NoID from MMenu where UPPER(Kode)=UPPER('mnDaftarTransaksiAccounting')")) > 0 Then
            Else
                NoID = GetNewID("MMenu")
                strSQL = "INSERT INTO [MMenu] ([noid],[kode],[idparent],[caption],[shortcut],[KeyShortcut],[icon],[nourut],[isactive],[isawalgroup],[objectrun],[IDServerAsal],[IsBarSubItem],[IDBarSubItem]) Values(" & _
                NoID & ",'mnDaftarTransaksiAccounting', 7, 'Daftar Transaksi (Accounting)', NULL, NULL, 0, 1, 1, 1, NULL, 1, 1, NULL)"
                EksekusiSQL(strSQL)
            End If

            IDAcc = NullToLong(EksekusiSQlSkalarNew("SELECT NOID FROM MMenu WHERE UPPER(Kode)=UPPER('mnDaftarTransaksiAccounting')"))
            If IDAcc >= 1 Then
                If Not NullToLong(EksekusiSQlSkalarNew("Select NoID from MMenu where UPPER(Kode)=UPPER('mnDaftarPembayaranHutangAccounting')")) > 0 Then
                    NoID = GetNewID("MMenu")
                    strSQL = "INSERT INTO [MMenu] ([noid],[kode],[idparent],[caption],[shortcut],[KeyShortcut],[icon],[nourut],[isactive],[isawalgroup],[objectrun],[IDServerAsal],[IsBarSubItem],[IDBarSubItem]) Values(" & _
                    NoID & ", 'mnDaftarPembayaranHutangAccounting', 7, 'Daftar Pembayaran Hutang', NULL, NULL, 0, 1, 1, 0, 'DaftarPembayaranHutangAccounting', NULL, 0, " & IDAcc & ")"
                    EksekusiSQL(strSQL)
                End If
                If Not NullToLong(EksekusiSQlSkalarNew("Select NoID from MMenu where UPPER(Kode)=UPPER('mnDaftarPembayaranPiutangAccounting')")) > 0 Then
                    NoID = GetNewID("MMenu")
                    strSQL = "INSERT INTO [MMenu] ([noid],[kode],[idparent],[caption],[shortcut],[KeyShortcut],[icon],[nourut],[isactive],[isawalgroup],[objectrun],[IDServerAsal],[IsBarSubItem],[IDBarSubItem]) Values(" & _
                    NoID & ", 'mnDaftarPembayaranPiutangAccounting', 7, 'Daftar Pembayaran Piutang', NULL, NULL, 0, 1, 1, 0, 'DaftarPembayaranPiutangAccounting', NULL, 0, " & IDAcc & ")"
                    EksekusiSQL(strSQL)
                End If
                If Not NullToLong(EksekusiSQlSkalarNew("Select NoID from MMenu where UPPER(Kode)=UPPER('mnDaftarPembelianAccounting')")) > 0 Then
                    NoID = GetNewID("MMenu")
                    strSQL = "INSERT INTO [MMenu] ([noid],[kode],[idparent],[caption],[shortcut],[KeyShortcut],[icon],[nourut],[isactive],[isawalgroup],[objectrun],[IDServerAsal],[IsBarSubItem],[IDBarSubItem]) Values(" & _
                    NoID & ", 'mnDaftarPembelianAccounting', 7, 'Daftar Pembelian', NULL, NULL, 0, 1, 1, 0, 'DaftarPembelianAccounting', NULL, 0, " & IDAcc & ")"
                    EksekusiSQL(strSQL)
                End If
                If Not NullToLong(EksekusiSQlSkalarNew("Select NoID from MMenu where UPPER(Kode)=UPPER('mnDaftarReturPembelianAccounting')")) > 0 Then
                    NoID = GetNewID("MMenu")
                    strSQL = "INSERT INTO [MMenu] ([noid],[kode],[idparent],[caption],[shortcut],[KeyShortcut],[icon],[nourut],[isactive],[isawalgroup],[objectrun],[IDServerAsal],[IsBarSubItem],[IDBarSubItem]) Values(" & _
                    NoID & ", 'mnDaftarReturPembelianAccounting', 7, 'Daftar Retur Pembelian', NULL, NULL, 0, 2, 1, 0, 'DaftarReturPembelianAccounting', NULL, 0, " & IDAcc & ")"
                    EksekusiSQL(strSQL)
                End If
                If Not NullToLong(EksekusiSQlSkalarNew("Select NoID from MMenu where UPPER(Kode)=UPPER('mnDaftarPenjualanAccounting')")) > 0 Then
                    NoID = GetNewID("MMenu")
                    strSQL = "INSERT INTO [MMenu] ([noid],[kode],[idparent],[caption],[shortcut],[KeyShortcut],[icon],[nourut],[isactive],[isawalgroup],[objectrun],[IDServerAsal],[IsBarSubItem],[IDBarSubItem]) Values(" & _
                    NoID & ", 'mnDaftarPenjualanAccounting', 7, 'Daftar Penjualan', NULL, NULL, 0, 3, 1, 0, 'DaftarPenjualanAccounting', NULL, 0, " & IDAcc & ")"
                    EksekusiSQL(strSQL)
                End If
                If Not NullToLong(EksekusiSQlSkalarNew("Select NoID from MMenu where UPPER(Kode)=UPPER('mnDaftarReturPenjualanAccounting')")) > 0 Then
                    NoID = GetNewID("MMenu")
                    strSQL = "INSERT INTO [MMenu] ([noid],[kode],[idparent],[caption],[shortcut],[KeyShortcut],[icon],[nourut],[isactive],[isawalgroup],[objectrun],[IDServerAsal],[IsBarSubItem],[IDBarSubItem]) Values(" & _
                    NoID & ", 'mnDaftarReturPenjualanAccounting', 7, 'Daftar Retur Penjualan', NULL, NULL, 0, 4, 1, 0, 'DaftarReturPenjualanAccounting', NULL, 0, " & IDAcc & ")"
                    EksekusiSQL(strSQL)
                End If
            End If
        Catch ex As Exception

        End Try
    End Sub

    Public Sub TambahMenuSQLite()
        Dim con As New SQLiteConnection
        Dim com As New SQLiteCommand
        Dim NoID As Long = -1
        Try
            con.ConnectionString = "Data Source=" & Application.StartupPath & "\System\engine\syssgi.sqlite" & ";"
            con.Open()
            com.Connection = con

            SQL = "SELECT NoID FROM sysFormHeader WHERE UPPER(NamaForm)=UPPER('daftarPembayaranHutangAccounting')"
            com.CommandText = SQL
            NoID = NullToLong(com.ExecuteScalar())
            If NoID <= 0 Then 'Tambah
                SQL = "SELECT MAX(NoID) FROM sysFormHeader"
                com.CommandText = SQL
                NoID = NullToLong(com.ExecuteScalar()) + 1
                SQL = "INSERT INTO sysFormHeader (noid,namaform,namatabel,namatabeldetil,caption,namaformentri,idtipeform,sql,namatabelmaster) VALUES ( " & vbCrLf & _
                      NoID & ", 'DaftarPembayaranHutangAccounting', 'SELECT * FROM vDaftarPembayaranHutangAccounting', '', 'Daftar Pembayaran Hutang Accounting', '', 7, '', 'vDaftarPembayaranHutangAccounting')"
                com.CommandText = SQL
                com.ExecuteNonQuery()
            End If

            SQL = "SELECT NoID FROM sysFormHeader WHERE UPPER(NamaForm)=UPPER('daftarPembelianAccounting')"
            com.CommandText = SQL
            NoID = NullToLong(com.ExecuteScalar())
            If NoID <= 0 Then 'Tambah
                SQL = "SELECT MAX(NoID) FROM sysFormHeader"
                com.CommandText = SQL
                NoID = NullToLong(com.ExecuteScalar()) + 1
                SQL = "INSERT INTO sysFormHeader (noid,namaform,namatabel,namatabeldetil,caption,namaformentri,idtipeform,sql,namatabelmaster) VALUES ( " & vbCrLf & _
                      NoID & ", 'DaftarPembelianAccounting', 'SELECT * FROM vDaftarPembelianAccounting', '', 'Daftar Pembelian Accounting', '', 7, '', 'vDaftarPembelianAccounting')"
                com.CommandText = SQL
                com.ExecuteNonQuery()
            End If

            SQL = "SELECT NoID FROM sysFormHeader WHERE UPPER(NamaForm)=UPPER('DaftarReturPembelianAccounting')"
            com.CommandText = SQL
            NoID = NullToLong(com.ExecuteScalar())
            If NoID <= 0 Then 'Tambah
                SQL = "SELECT MAX(NoID) FROM sysFormHeader"
                com.CommandText = SQL
                NoID = NullToLong(com.ExecuteScalar()) + 1
                SQL = "INSERT INTO sysFormHeader (noid,namaform,namatabel,namatabeldetil,caption,namaformentri,idtipeform,sql,namatabelmaster) VALUES ( " & vbCrLf & _
                      NoID & ", 'DaftarReturPembelianAccounting', 'SELECT * FROM vDaftarReturPembelianAccounting', '', 'Daftar Retur Pembelian Accounting', '', 7, '', 'vDaftarReturPembelianAccounting')"
                com.CommandText = SQL
                com.ExecuteNonQuery()
            End If

            SQL = "SELECT NoID FROM sysFormHeader WHERE UPPER(NamaForm)=UPPER('DaftarPenjualanAccounting')"
            com.CommandText = SQL
            NoID = NullToLong(com.ExecuteScalar())
            If NoID <= 0 Then 'Tambah
                SQL = "SELECT MAX(NoID) FROM sysFormHeader"
                com.CommandText = SQL
                NoID = NullToLong(com.ExecuteScalar()) + 1
                SQL = "INSERT INTO sysFormHeader (noid,namaform,namatabel,namatabeldetil,caption,namaformentri,idtipeform,sql,namatabelmaster) VALUES ( " & vbCrLf & _
                      NoID & ", 'DaftarPenjualanAccounting', 'SELECT * FROM vDaftarPenjualanAccounting', '', 'Daftar Penjualan Accounting', '', 7, '', 'vDaftarPenjualanAccounting')"
                com.CommandText = SQL
                com.ExecuteNonQuery()
            End If

            SQL = "SELECT NoID FROM sysFormHeader WHERE UPPER(NamaForm)=UPPER('DaftarReturPenjualanAccounting')"
            com.CommandText = SQL
            NoID = NullToLong(com.ExecuteScalar())
            If NoID <= 0 Then 'Tambah
                SQL = "SELECT MAX(NoID) FROM sysFormHeader"
                com.CommandText = SQL
                NoID = NullToLong(com.ExecuteScalar()) + 1
                SQL = "INSERT INTO sysFormHeader (noid,namaform,namatabel,namatabeldetil,caption,namaformentri,idtipeform,sql,namatabelmaster) VALUES ( " & vbCrLf & _
                      NoID & ", 'DaftarReturPenjualanAccounting', 'SELECT * FROM vDaftarReturPenjualanAccounting', '', 'Daftar Retur Penjualan Accounting', '', 7, '', 'vDaftarReturPenjualanAccounting')"
                com.CommandText = SQL
                com.ExecuteNonQuery()
            End If

            SQL = "SELECT NoID FROM sysFormHeader WHERE UPPER(NamaForm)=UPPER('DaftarReturPenjualanAccounting')"
            com.CommandText = SQL
            NoID = NullToLong(com.ExecuteScalar())
            If NoID <= 0 Then 'Tambah
                SQL = "SELECT MAX(NoID) FROM sysFormHeader"
                com.CommandText = SQL
                NoID = NullToLong(com.ExecuteScalar()) + 1
                SQL = "INSERT INTO sysFormHeader (noid,namaform,namatabel,namatabeldetil,caption,namaformentri,idtipeform,sql,namatabelmaster) VALUES ( " & vbCrLf & _
                      NoID & ", 'DaftarReturPenjualanAccounting', 'SELECT * FROM vDaftarReturPenjualanAccounting', '', 'Daftar Retur Penjualan Accounting', '', 7, '', 'vDaftarReturPenjualanAccounting')"
                com.CommandText = SQL
                com.ExecuteNonQuery()
            End If

        Catch ex As Exception
            XtraMessageBox.Show("Info Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
        Finally
            If con.State = ConnectionState.Open Then
                con.Close()
            End If
            con.Dispose()
            com.Dispose()
        End Try
    End Sub

    Public Sub MainAccounting()
        TambahKolomTableViewPostinganAcc()
        TambahMenuPostinganAcc()
        TambahMenuSQLite()
        AmbilSettingAccounting()
    End Sub

    Public Sub AmbilSettingAccounting()
        Dim sql As String
        Dim ds As New DataSet
        Try

            sql = "SELECT * FROM MSettingAkun"
            ds = ExecuteDataset("MSetting", sql)
            If ds.Tables(0).Rows.Count >= 1 Then
                mdlAccPublik.defIDAkunLabaBerjalan = NullTolInt(ds.Tables(0).Rows(0).Item("IDAkunLRBerjalan"))
            End If
            defIDMataUang = NullToLong(EksekusiSQlSkalarNew("SELECT ID FROM MMataUang"))

        Catch ex As Exception
            DevExpress.XtraEditors.XtraMessageBox.Show("Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            ds.Dispose()
        End Try
    End Sub
End Module
