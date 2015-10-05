Imports System.Data
Imports System.Data.SqlClient
Imports DevExpress.XtraEditors

Public Class clsPostingAccounting
    Public Shared Sub LihatHasilPosting(ByVal IDTransaksi As Long, ByVal IDJenisTransaksi As Integer, ByVal Periode As Date, ByVal FormPemanggil As XtraForm)
        Dim frmHasil As New frmViewJurnal
        Try
            Select Case IDJenisTransaksi
                Case 2, 3
                    frmHasil.IDTransaksi = NullToLong(EksekusiSQlSkalarNew("SELECT TOP 1 ID FROM MJurnal WHERE IDJenisTransaksi=" & IDJenisTransaksi & " AND IDAlamat=" & IDTransaksi & " AND MONTH(Tanggal)=" & Periode.Month & " AND YEAR(Tanggal)=" & Periode.Year))
                Case Else
                    frmHasil.IDTransaksi = IDTransaksi
            End Select
            frmHasil.IDTypeTransaksi = IDJenisTransaksi
            frmHasil.StartPosition = FormStartPosition.CenterParent
            frmHasil.WindowState = FormWindowState.Normal
            frmHasil.ShowDialog(FormPemanggil)
        Catch ex As Exception
            XtraMessageBox.Show("Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            frmHasil.Dispose()
        End Try
    End Sub

    Public Shared Function JurnalSudahDiKunci(ByVal IDTransaksi As Long, ByVal IDJenisTransaksi As Long, ByVal Periode As Date) As Boolean
        Dim SQL As String = ""
        Dim hasil As Boolean = True
        Try
            Select Case IDJenisTransaksi
                Case 2, 3
                    hasil = NullToBool(EksekusiSQlSkalarNew("SELECT IsPosting FROM MJurnal WHERE MONTH(Tanggal)=" & Month(Periode) & "' AND YEAR(Tanggal)=" & Year(Periode) & " AND IDJenisTransaksi=" & IDJenisTransaksi & " AND IDAlamat=" & IDTransaksi))
                Case Else
                    hasil = NullToBool(EksekusiSQlSkalarNew("SELECT IsPosting FROM MJurnal WHERE IDTransaksi=" & IDTransaksi & " AND IDJenisTransaksi=" & IDJenisTransaksi))
            End Select
        Catch ex As Exception
            XtraMessageBox.Show("Info Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
        End Try
        Return hasil
    End Function
    Function GetKode(ByVal NmTable As String, ByVal KodeNota As String, ByVal Bulan As Integer, ByVal Tahun As Integer) As String
        Dim curentcursor As Cursor = Windows.Forms.Cursor.Current
        Windows.Forms.Cursor.Current = Cursors.WaitCursor
        Dim Id As Long
        Id = NullToLong(EksekusiSQLSkalar("Select count(ID) as x from " & NmTable & " where month(tanggal)=" & Bulan & " and year(tanggal)=" & Tahun)) + 1
        Windows.Forms.Cursor.Current = curentcursor
        Return KodeNota & Format(Id, "0000") & "/" & Format(Bulan, "00") & "/" & Format(Tahun, "0000")
    End Function
    'Public Shared Function PostingJurnalPembelian(ByVal NoID As Long, ByVal Periode As Date, Optional ByVal IsForce As Boolean = False) As Boolean
    '    Dim cn As New SqlConnection
    '    Dim com As New SqlCommand
    '    Dim ds As New DataSet
    '    Dim ODa As New SqlDataAdapter
    '    Dim SQL As String = ""

    '    Dim IDJurnal As Long = -1
    '    Dim IDJurnalD As Long = -1
    '    Dim DiscNotaProsen As Double = 0
    '    Dim SelisihHisBalancing As Double = 0
    '    Dim IDAkunBalancing As Long = -1
    '    Dim IDAkunPembelian As Long = -1
    '    Dim IDAkunReturPembelian As Long = -1
    '    Dim IDAkunPenjualan As Long = -1
    '    Dim IDAkunReturPenjualan As Long = -1

    '    Dim IDAkunKasPembelian As Long = -1
    '    Dim IDAkunBiayaPembelian As Long = -1
    '    Try
    '        If Not JurnalSudahDiKunci(NoID, 2, Periode) Then
    '            cn.ConnectionString = StrKonSql
    '            cn.Open()
    '            com.Connection = cn
    '            ODa.SelectCommand = com
    '            'Persiapan
    '            IDAkunBalancing = NullToLong(EksekusiSQlSkalarNew("SELECT IDAkunSelisihBalancing FROM MSettingAkun"))
    '            IDAkunPembelian = NullToLong(EksekusiSQlSkalarNew("SELECT IDAkunPembelian FROM MSettingAkun"))
    '            IDAkunReturPembelian = NullToLong(EksekusiSQlSkalarNew("SELECT IDAkunReturPembelian FROM MSettingAkun"))
    '            IDAkunPenjualan = NullToLong(EksekusiSQlSkalarNew("SELECT IDAkunPenjualan FROM MSettingAkun"))
    '            IDAkunReturPenjualan = NullToLong(EksekusiSQlSkalarNew("SELECT IDAkunReturPenjualan FROM MSettingAkun"))
    '            IDAkunKasPembelian = NullToLong(EksekusiSQlSkalarNew("SELECT IDAkunKasPembelian FROM MSettingAkun"))
    '            IDAkunBiayaPembelian = NullToLong(EksekusiSQlSkalarNew("SELECT IDAkunBiayaPembelian FROM MSettingAkun"))

    '            'Mulai
    '            SQL = "SELECT IsNull(MBeli.DiskonNotaProsen,0) AS Diskon FROM MBeli WHERE MBeli.NoID=" & NoID
    '            com.CommandText = SQL
    '            DiscNotaProsen = NullToDbl(com.ExecuteScalar())
    '            If DiscNotaProsen <> 0 Then 'Updatekan diskon Prosen jika ada
    '                com.CommandText = "UPDATE MBeliD SET DiskonNotaProsen=" & FixKoma(DiscNotaProsen) & " WHERE IDBeli=" & NoID
    '                com.ExecuteNonQuery()
    '            End If

    '            SQL = "DELETE FROM MJurnalD WHERE IDJurnal IN (SELECT ID FROM MJurnal WHERE IDJenisTransaksi=2 AND IDTransaksi=" & NoID & ")"
    '            com.CommandText = SQL
    '            com.ExecuteNonQuery()

    '            SQL = "DELETE FROM MJurnal WHERE IDJenisTransaksi=2 AND IDTransaksi=" & NoID
    '            com.CommandText = SQL
    '            com.ExecuteNonQuery()

    '            IDJurnal = GetNewID("MJurnal", "ID")
    '            SQL = "INSERT INTO MJurnal (ID, IDDepartemenUser,Tanggal, Kode, Keterangan, IDJenisTransaksi,IDTransaksi, IDUSerEntry,IDUserPosting ) " & _
    '                  " SELECT " & IDJurnal & " AS ID,IDDepartemenUser, MBeli.Tanggal, MBeli.Kode, MBeli.Keterangan, 2 AS IDJenisTransaksi,NoID, IDUser," & IDUserAktif & " AS IDUserPosting " & _
    '                  " FROM MBeli " & _
    '                  " WHERE MBeli.NoID=" & NoID
    '            com.CommandText = SQL
    '            com.ExecuteNonQuery()

    '            com.CommandText = "SELECT MAlamat.Nama AS NamaSupplier, MBeli.Kode, MBarang.IDAkunPersediaan, 1 AS IDDepartemen, SUM((MBeliD.Jumlah*(1-IsNull(MBeliD.DiskonNotaProsen,0)/100))-IsNull(MBeliD.DiskonNotaRp,0)) AS Total " & _
    '                              " FROM MBeliD INNER JOIN MBeli ON MBeli.NoID=MBeliD.IDBeli " & _
    '                              " LEFT JOIN MAlamat ON MAlamat.NoID=MBeli.IDSupplier " & _
    '                              " LEFT JOIN MBarang ON MBarang.NoID=MBeliD.IDBarang " & _
    '                              " GROUP BY MAlamat.Nama, MBeli.Kode, MBeliD.IDBeli, MBarang.IDAkunPersediaan " & _
    '                              " HAVING MBeliD.IDBeli=" & NoID
    '            If Not ds.Tables("MBeliD") Is Nothing Then
    '                ds.Tables("MBeliD").Clear()
    '            End If
    '            ODa.Fill(ds, "MBeliD")
    '            For i As Integer = 0 To ds.Tables("MBeliD").Rows.Count - 1
    '                'Persediaan
    '                SQL = "INSERT INTO MJurnalD ( IDJurnal, IDDepartemen, IDAkun, " & _
    '                      "Debet, DebetA, Kurs, IDMataUang, Kredit, KreditA, IsBalancing,Keterangan ) " & _
    '                      "Values(" & IDJurnal & "," & _
    '                      NullToLong(ds.Tables("MBeliD").Rows(i).Item("IDDepartemen")) & "," & _
    '                      NullToLong(ds.Tables("MBeliD").Rows(i).Item("IDAkunPersediaan")) & "," & _
    '                      Bulatkan(NullToDbl(ds.Tables("MBeliD").Rows(i).Item("Total")), 0) & "," & _
    '                      Bulatkan(NullToDbl(ds.Tables("MBeliD").Rows(i).Item("Total")), 0) & ",1," & _
    '                      defIDMataUang & "," & _
    '                      "0," & _
    '                      "0," & _
    '                      "0,'" & "Pembelian : " & FixApostropi(NullToStr(ds.Tables("MBeliD").Rows(i).Item("NamaSupplier"))) & ", No :" & FixApostropi(NullToStr(ds.Tables("MBeliD").Rows(i).Item("Kode"))) & "')"
    '                com.CommandText = SQL
    '                com.ExecuteNonQuery()

    '                'Balancing
    '                SelisihHisBalancing = Bulatkan(NullToDbl(ds.Tables("MBeliD").Rows(i).Item("Total")), 2) - Bulatkan(NullToDbl(ds.Tables("MBeliD").Rows(i).Item("Total")), 0)
    '                If SelisihHisBalancing < 0 Then
    '                    SQL = "INSERT INTO MJurnalD ( IDJurnal, IDDepartemen, IDAkun, " & _
    '                          "Debet, DebetA, Kurs, IDMataUang, Kredit, KreditA, IsBalancing,Keterangan ) " & _
    '                          "Values(" & IDJurnal & "," & _
    '                          NullToLong(ds.Tables("MBeliD").Rows(i).Item("IDDepartemen")) & "," & _
    '                          IDAkunBalancing & "," & _
    '                          FixKoma(-1.0# * SelisihHisBalancing) & "," & _
    '                          FixKoma(-1.0# * SelisihHisBalancing) & "," & _
    '                          "1," & _
    '                          defIDMataUang & "," & _
    '                          "0," & _
    '                          "0," & _
    '                          "0,'" & "Pembelian : " & FixApostropi(NullToStr(ds.Tables("MBeliD").Rows(i).Item("NamaSupplier"))) & ", No :" & FixApostropi(NullToStr(ds.Tables("MBeliD").Rows(i).Item("Kode"))) & "')"
    '                    com.CommandText = SQL
    '                    com.ExecuteNonQuery()
    '                ElseIf SelisihHisBalancing > 0 Then
    '                    SQL = "INSERT INTO MJurnalD ( IDJurnal, IDDepartemen, IDAkun, " & _
    '                          "Debet, DebetA, Kurs, IDMataUang, Kredit, KreditA, IsBalancing,Keterangan ) " & _
    '                          "Values(" & IDJurnal & "," & _
    '                          NullToLong(ds.Tables("MBeliD").Rows(i).Item("IDDepartemen")) & "," & _
    '                          IDAkunBalancing & "," & _
    '                          "0," & _
    '                          "0," & _
    '                          "1," & _
    '                          defIDMataUang & "," & _
    '                          FixKoma(SelisihHisBalancing) & "," & _
    '                          FixKoma(SelisihHisBalancing) & "," & _
    '                          "0,'" & "Pembelian : " & FixApostropi(NullToStr(ds.Tables("MBeliD").Rows(i).Item("NamaSupplier"))) & ", No :" & FixApostropi(NullToStr(ds.Tables("MBeliD").Rows(i).Item("Kode"))) & "')"
    '                    com.CommandText = SQL
    '                    com.ExecuteNonQuery()
    '                End If
    '            Next

    '            'Hutang Supplier
    '            com.CommandText = "SELECT MAlamat.Nama AS NamaSupplier, MBeli.Kode, MBeli.Total, MBeli.Bayar, MBeli.Biaya, MBeli.Sisa, MBeli.IDDepartemen, MAlamat.IDAkunHutang, MBeli.IDAkunKas " & _
    '                              " FROM MBeli " & _
    '                              " LEFT JOIN MAlamat ON MAlamat.NoID=MBeli.IDSupplier " & _
    '                              " WHERE MBeli.NoID=" & NoID
    '            If Not ds.Tables("MBeli") Is Nothing Then
    '                ds.Tables("MBeli").Clear()
    '            End If
    '            ODa.Fill(ds, "MBeli")
    '            If ds.Tables("MBeli").Rows.Count >= 1 Then
    '                If NullToDbl(ds.Tables("MBeli").Rows(0).Item("Bayar")) > 0 Then 'Dibayar
    '                    SQL = "INSERT INTO MJurnalD ( IDJurnal, IDDepartemen, IDAkun, " & _
    '                          "Debet, DebetA, Kurs, IDMataUang, Kredit, KreditA,IsBalancing,Keterangan ) " & _
    '                          "Values(" & IDJurnal & "," & _
    '                          NullToLong(ds.Tables("MBeli").Rows(0).Item("IDDepartemen")) & "," & _
    '                          NullToLong(IDAkunKasPembelian) & "," & _
    '                          "0," & _
    '                          "0," & FixKoma(1) & "," & _
    '                          defIDMataUang & "," & _
    '                          FixKoma(Bulatkan(NullToDbl(ds.Tables("MBeli").Rows(0).Item("Bayar")), 0)) & "," & _
    '                          FixKoma(Bulatkan(NullToDbl(ds.Tables("MBeli").Rows(0).Item("Bayar")), 0)) & "," & _
    '                          "0,'" & "Pembelian : " & FixApostropi(NullToStr(ds.Tables("MBeli").Rows(0).Item("NamaSupplier"))) & ", No :" & FixApostropi(NullToStr(ds.Tables("MBeli").Rows(0).Item("Kode"))) & "')"
    '                    com.CommandText = SQL
    '                    com.ExecuteNonQuery()
    '                End If
    '                If NullToDbl(ds.Tables("MBeli").Rows(0).Item("Sisa")) > 0 Then 'Hutang ke supplier
    '                    SQL = "INSERT INTO MJurnalD ( IDJurnal, IDDepartemen, IDAkun, " & _
    '                          "Debet, DebetA, Kurs, IDMataUang, Kredit, KreditA,IsBalancing,Keterangan ) " & _
    '                          "Values(" & IDJurnal & "," & _
    '                          NullToLong(ds.Tables("MBeli").Rows(0).Item("IDDepartemen")) & "," & _
    '                          NullToLong(ds.Tables("MBeli").Rows(0).Item("IDAkunHutang")) & "," & _
    '                          "0," & _
    '                          "0," & FixKoma(1) & "," & _
    '                          defIDMataUang & "," & _
    '                          FixKoma(Bulatkan(NullToDbl(ds.Tables("MBeli").Rows(0).Item("Sisa")), 0)) & "," & _
    '                          FixKoma(Bulatkan(NullToDbl(ds.Tables("MBeli").Rows(0).Item("Sisa")), 0)) & "," & _
    '                          "0,'" & "Pembelian : " & FixApostropi(NullToStr(ds.Tables("MBeli").Rows(0).Item("NamaSupplier"))) & ", No :" & FixApostropi(NullToStr(ds.Tables("MBeli").Rows(0).Item("Kode"))) & "')"
    '                    com.CommandText = SQL
    '                    com.ExecuteNonQuery()
    '                End If
    '                If NullToDbl(ds.Tables("MBeli").Rows(0).Item("Biaya")) > 0 Then 'Biaya
    '                    SQL = "INSERT INTO MJurnalD ( IDJurnal, IDDepartemen, IDAkun, " & _
    '                          "Debet, DebetA, Kurs, IDMataUang, Kredit, KreditA,IsBalancing,Keterangan ) " & _
    '                          "Values(" & IDJurnal & "," & _
    '                          NullToLong(ds.Tables("MBeli").Rows(0).Item("IDDepartemen")) & "," & _
    '                          NullToLong(IDAkunBiayaPembelian) & "," & _
    '                          "0," & _
    '                          "0," & FixKoma(1) & "," & _
    '                          defIDMataUang & "," & _
    '                          FixKoma(Bulatkan(NullToDbl(ds.Tables("MBeli").Rows(0).Item("Biaya")), 0)) & "," & _
    '                          FixKoma(Bulatkan(NullToDbl(ds.Tables("MBeli").Rows(0).Item("Biaya")), 0)) & "," & _
    '                          "0,'" & "Pembelian : " & FixApostropi(NullToStr(ds.Tables("MBeli").Rows(0).Item("NamaSupplier"))) & ", No :" & FixApostropi(NullToStr(ds.Tables("MBeli").Rows(0).Item("Kode"))) & "')"
    '                    com.CommandText = SQL
    '                    com.ExecuteNonQuery()
    '                End If
    '            End If

    '            SQL = "UPDATE MBeli SET IsPostedJurnal=1, TglPostedJurnal=Getdate(), IDUserPostedJurnal=" & IDUserAktif & " WHERE NoID=" & NoID
    '            com.CommandText = SQL
    '            com.ExecuteNonQuery()
    '        End If
    '    Catch ex As Exception
    '        XtraMessageBox.Show("Info Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
    '    Finally
    '        If Not ds Is Nothing Then
    '            ds.Dispose()
    '        End If
    '        If ODa Is Nothing Then
    '            ODa.Dispose()
    '        End If
    '        If cn.State = ConnectionState.Open Then
    '            cn.Close()
    '        End If
    '        cn.Dispose()
    '        com.Dispose()
    '    End Try
    'End Function
    Public Shared Function PostingJurnalPembelian(ByVal IDSupplier As Long, ByVal Periode As Date, Optional ByVal IsForce As Boolean = False) As Boolean
        Dim cn As New SqlConnection
        Dim com As New SqlCommand
        Dim ds As New DataSet
        Dim ODa As New SqlDataAdapter
        Dim Tran As SqlTransaction = Nothing
        Dim SQL As String = ""

        Dim IsPKP As Boolean = False
        Dim IDJurnal As Long = -1
        Dim IDJurnalD As Long = -1
        Dim Kode As String = ""
        'Dim DiscNotaProsen As Double = 0

        Dim BKP As Double = 0.0
        Dim NBKP As Double = 0.0
        Dim PPN As Double = 0.0

        Dim SelisihHisBalancing As Double = 0
        Dim IDAkunBalancing As Long = -1
        Dim IDAkunPembelian As Long = -1
        Dim IDAkunBiayaPembelian As Long = -1
        Try
            If Not JurnalSudahDiKunci(IDSupplier, 2, Periode) Then
                cn.ConnectionString = StrKonSql
                cn.Open()
                com.Connection = cn
                ODa.SelectCommand = com
                Tran = cn.BeginTransaction("MJurnal")
                com.Transaction = Tran
                'Persiapan
                IDAkunBalancing = NullToLong(EksekusiSQlSkalarNew("SELECT IDAkunSelisihBalancing FROM MSettingAkun"))
                IDAkunPembelian = NullToLong(EksekusiSQlSkalarNew("SELECT IDAkunPembelian FROM MSettingAkun"))
                IDAkunBiayaPembelian = NullToLong(EksekusiSQlSkalarNew("SELECT IDAkunBiayaPembelian FROM MSettingAkun"))

                'Mulai
                'SQL = "SELECT MBeli.IDSupplier, Malamat.Kode AS KodeSupplier, MAlamat.Nama AS Supplier, MAlamat.IDAkunHutang, IsNull(MAlamat.IsPKP,0) AS PKP, SUM(MBeli.Total) AS Total, SUM(ROUND(MBeli.Total/1.1,0)) AS DPP, SUM(MBeli.Total-ROUND(MBeli.Total/1.1,0)) AS PPN " & vbCrLf & _
                '      " FROM MBeli INNER JOIN MAlamat ON MAlamat.NoID=MBeli.IDSupplier " & vbCrLf & _
                '      " WHERE MBeli.IsPosted=1 AND MONTH(MBeli.Tanggal)=" & FixKoma(Periode.Month) & " AND YEAR(MBeli.Tanggal)=" & FixKoma(Periode.Year) & " AND MBeli.IDSupplier=" & IDSupplier & vbCrLf & _
                '      " GROUP BY MBeli.IDSupplier, MAlamat.IDAkunHutang, Malamat.Kode, MAlamat.Nama, IsNull(MAlamat.IsPKP,0)"

                SQL = " SELECT MAlamat.NoID, MAlamat.Kode, MAlamat.IDAkunHutang, MAlamat.Nama AS Supplier, MAlamat.IsPKP AS [PKP / Non PKP], SUM(X.BKP) AS BKP, SUM(X.NBKP) AS NonBKP, SUM(X.BKP)+SUM(X.NBKP) AS Total" & vbCrLf & _
                      " FROM MAlamat" & vbCrLf & _
                      " LEFT JOIN " & vbCrLf & _
                      " (SELECT MBarang.NoID, MBeli.IDSupplier, MBarang.IDKategori," & vbCrLf & _
                      " SUM(CASE WHEN MBarang.IDKategori IN (25,27,31,32) THEN 0 ELSE MBeliD.Jumlah END) AS BKP, SUM(CASE WHEN MBarang.IDKategori NOT IN (25,27,31,32) THEN 0 ELSE MBeliD.Jumlah END) AS NBKP" & vbCrLf & _
                      " FROM MBeli " & vbCrLf & _
                      " INNER JOIN MBeliD ON MBeli.NoID=MBeliD.IDBeli" & vbCrLf & _
                      " INNER JOIN MBarang ON MBarang.NoID=MBeliD.IDBarang" & vbCrLf & _
                      " WHERE MBeli.IsPosted=1 AND IsNull(MBeli.IsTanpaBarang,0)=0 AND MBeli.IDSupplier=" & IDSupplier & " AND MONTH(MBeli.Tanggal)=" & FixKoma(Periode.Month) & " AND YEAR(MBeli.Tanggal)=" & FixKoma(Periode.Year) & vbCrLf & _
                      " GROUP BY MBarang.NoID, MBeli.IDSupplier, MBarang.IDKategori" & vbCrLf & _
                      " UNION ALL" & vbCrLf & _
                      " SELECT -1, MBeli.IDSupplier, MBeli.IDKategori, " & vbCrLf & _
                      " (CASE WHEN MBeli.IDKategori IN (25,27,31,32) THEN 0 ELSE MBeli.Total END) AS BKP," & vbCrLf & _
                      " (CASE WHEN MBeli.IDKategori NOT IN (25,27,31,32) THEN 0 ELSE MBeli.Total END) AS NBKP" & vbCrLf & _
                      " FROM MBeli " & vbCrLf & _
                      " LEFT JOIN MKategori ON MKategori.NoID=MBeli.IDKategori" & vbCrLf & _
                      " WHERE MBeli.IsPosted=1 AND IsNull(MBeli.IsTanpaBarang,0)=1 AND MBeli.Total>0 AND MBeli.IDSupplier=" & IDSupplier & " AND MONTH(MBeli.Tanggal)=" & FixKoma(Periode.Month) & " AND YEAR(MBeli.Tanggal)=" & FixKoma(Periode.Year) & vbCrLf & _
                      " UNION ALL" & vbCrLf & _
                      " SELECT -1, MBeli.IDSupplier, -1, -1*MBeli.DiskonNotaTotal, 0" & vbCrLf & _
                      " FROM MBeli" & vbCrLf & _
                      " WHERE MBeli.IsPosted=1 AND IsNull(MBeli.IsTanpaBarang,0)=0 AND MBeli.IDSupplier=" & IDSupplier & " AND MONTH(MBeli.Tanggal)=" & FixKoma(Periode.Month) & " AND YEAR(MBeli.Tanggal)=" & FixKoma(Periode.Year) & vbCrLf & _
                      " ) X ON X.IDSupplier=MAlamat.NoID" & vbCrLf & _
                      " WHERE MAlamat.IsSupplier=1 AND MAlamat.NoID=" & IDSupplier & " AND MAlamat.IsActive=1" & vbCrLf & _
                      " GROUP BY MAlamat.NoID, MAlamat.Kode, MAlamat.IDAkunHutang, MAlamat.Nama, MAlamat.IsPKP" & vbCrLf & _
                      " HAVING SUM(X.BKP)+SUM(X.NBKP)<>0"
                com.CommandText = SQL
                If Not ds.Tables("MBeli") Is Nothing Then
                    ds.Tables("MBeli").Clear()
                End If
                ODa.Fill(ds, "MBeli")
                For i As Integer = 0 To ds.Tables("MBeli").Rows.Count - 1
                    SQL = "DELETE FROM MJurnalD WHERE IDJurnal IN (SELECT ID FROM MJurnal WHERE IDJenisTransaksi=2 AND IDAlamat=" & IDSupplier & " AND MONTH(Tanggal)=" & FixKoma(Periode.Month) & " AND YEAR(Tanggal)=" & FixKoma(Periode.Year) & ")"
                    com.CommandText = SQL
                    com.ExecuteNonQuery()

                    SQL = "DELETE FROM MJurnal WHERE IDJenisTransaksi=2 AND IDAlamat=" & IDSupplier & " AND MONTH(Tanggal)=" & FixKoma(Periode.Month) & " AND YEAR(Tanggal)=" & FixKoma(Periode.Year)
                    com.CommandText = SQL
                    com.ExecuteNonQuery()

                    SQL = "SELECT count(ID) as x from Mjurnal where month(tanggal)=" & Month(Periode) & " and year(tanggal)=" & Year(Periode)
                    com.CommandText = SQL
                    Kode = "BL" & Format(NullToLong(com.ExecuteScalar()) + 1, "0000") & "/" & Format(Month(Periode), "00") & "/" & Format(Year(Periode), "0000")
                    SQL = "SELECT MAX(MJurnal.ID) FROM MJurnal "
                    com.CommandText = SQL
                    IDJurnal = NullToLong(com.ExecuteScalar) + 1
                    SQL = "INSERT INTO MJurnal (ID, IDAlamat, IDDepartemenUser, Tanggal, Kode, Keterangan, IDJenisTransaksi, IDTransaksi, IDUserEntry, IDUserPosting) VALUES (" & _
                          IDJurnal & "," & IDSupplier & "," & DefIDDepartemen & ", '" & CDate(Periode.ToString("yyyy/MM/01")).AddMonths(1).AddDays(-1).ToString("yyyy/MM/dd") & "', '" & FixApostropi(Kode) & "', 'Pembelian " & FixApostropi(NullToStr(ds.Tables("MBeli").Rows(i).Item("Supplier"))) & ", " & Periode.ToString("MMM yyyy") & "' , 2, -1, " & IDUserAktif & "," & IDUserAktif & ")"
                    com.CommandText = SQL
                    com.ExecuteNonQuery()
                    IsPKP = NullToBool(ds.Tables("MBeli").Rows(i).Item("PKP / Non PKP"))

                    BKP = Bulatkan(NullToDbl(ds.Tables("MBeli").Rows(i).Item("BKP")) / 1.1, 0)
                    NBKP = NullToDbl(ds.Tables("MBeli").Rows(i).Item("NonBKP"))
                    PPN = NullToDbl(ds.Tables("MBeli").Rows(i).Item("BKP")) - Bulatkan(NullToDbl(ds.Tables("MBeli").Rows(i).Item("BKP")) / 1.1, 0)

                    If BKP <> 0 Then 'Persediaan/Pembelian BKP
                        SQL = "INSERT INTO MJurnalD ( IDJurnal, IDDepartemen, IDAkun, " & _
                              "Debet, DebetA, Kurs, IDMataUang, Kredit, KreditA, IsBalancing,Keterangan ) " & _
                              "Values(" & IDJurnal & "," & _
                              NullToLong(DefIDDepartemen) & "," & _
                              NullToLong(IDAkunPembelian) & "," & _
                              Bulatkan(BKP, 0) & "," & _
                              Bulatkan(BKP, 0) & ",1," & _
                              defIDMataUang & "," & _
                              "0," & _
                              "0," & _
                              "0,'" & "Pembelian BKP : " & FixApostropi(NullToStr(ds.Tables("MBeli").Rows(i).Item("Supplier"))) & ", Periode :" & Periode.ToString("MMM yyyy") & "')"
                        com.CommandText = SQL
                        com.ExecuteNonQuery()

                        'Balancing
                        SelisihHisBalancing = Bulatkan(BKP, 2) - Bulatkan(BKP, 0)
                        If SelisihHisBalancing < 0 Then
                            SQL = "INSERT INTO MJurnalD ( IDJurnal, IDDepartemen, IDAkun, " & _
                                  "Debet, DebetA, Kurs, IDMataUang, Kredit, KreditA, IsBalancing,Keterangan ) " & _
                                  "Values(" & IDJurnal & "," & _
                                  NullToLong(DefIDDepartemen) & "," & _
                                  IDAkunBalancing & "," & _
                                  FixKoma(-1.0# * SelisihHisBalancing) & "," & _
                                  FixKoma(-1.0# * SelisihHisBalancing) & "," & _
                                  "1," & _
                                  defIDMataUang & "," & _
                                  "0," & _
                                  "0," & _
                                  "0,'" & "Pembelian BKP : " & FixApostropi(NullToStr(ds.Tables("MBeli").Rows(i).Item("Supplier"))) & ", Periode :" & Periode.ToString("MMM yyyy") & "')"
                            com.CommandText = SQL
                            com.ExecuteNonQuery()
                        ElseIf SelisihHisBalancing > 0 Then
                            SQL = "INSERT INTO MJurnalD ( IDJurnal, IDDepartemen, IDAkun, " & _
                                  "Debet, DebetA, Kurs, IDMataUang, Kredit, KreditA, IsBalancing,Keterangan ) " & _
                                  "Values(" & IDJurnal & "," & _
                                  NullToLong(DefIDDepartemen) & "," & _
                                  IDAkunBalancing & "," & _
                                  "0," & _
                                  "0," & _
                                  "1," & _
                                  defIDMataUang & "," & _
                                  FixKoma(SelisihHisBalancing) & "," & _
                                  FixKoma(SelisihHisBalancing) & "," & _
                                  "0,'" & "Pembelian BKP : " & FixApostropi(NullToStr(ds.Tables("MBeli").Rows(i).Item("Supplier"))) & ", Periode :" & Periode.ToString("MMM yyyy") & "')"
                            com.CommandText = SQL
                            com.ExecuteNonQuery()
                        End If
                    End If

                    If PPN <> 0 Then 'PPN
                        SQL = "INSERT INTO MJurnalD ( IDJurnal, IDDepartemen, IDAkun, " & _
                              "Debet, DebetA, Kurs, IDMataUang, Kredit, KreditA, IsBalancing,Keterangan ) " & _
                              "Values(" & IDJurnal & "," & _
                              NullToLong(DefIDDepartemen) & "," & _
                              NullToLong(IDAkunBiayaPembelian) & "," & _
                              Bulatkan(PPN, 0) & "," & _
                              Bulatkan(PPN, 0) & ",1," & _
                              defIDMataUang & "," & _
                              "0," & _
                              "0," & _
                              "0,'" & "PPN : " & FixApostropi(NullToStr(ds.Tables("MBeli").Rows(i).Item("Supplier"))) & ", Periode :" & Periode.ToString("MMM yyyy") & "')"
                        com.CommandText = SQL
                        com.ExecuteNonQuery()

                        'Balancing
                        SelisihHisBalancing = Bulatkan(PPN, 2) - Bulatkan(PPN, 0)
                        If SelisihHisBalancing < 0 Then
                            SQL = "INSERT INTO MJurnalD ( IDJurnal, IDDepartemen, IDAkun, " & _
                                  "Debet, DebetA, Kurs, IDMataUang, Kredit, KreditA, IsBalancing,Keterangan ) " & _
                                  "Values(" & IDJurnal & "," & _
                                  NullToLong(DefIDDepartemen) & "," & _
                                  IDAkunBalancing & "," & _
                                  FixKoma(-1.0# * SelisihHisBalancing) & "," & _
                                  FixKoma(-1.0# * SelisihHisBalancing) & "," & _
                                  "1," & _
                                  defIDMataUang & "," & _
                                  "0," & _
                                  "0," & _
                                  "0,'" & "PPN : " & FixApostropi(NullToStr(ds.Tables("MBeli").Rows(i).Item("Supplier"))) & ", Periode :" & Periode.ToString("MMM yyyy") & "')"
                            com.CommandText = SQL
                            com.ExecuteNonQuery()
                        ElseIf SelisihHisBalancing > 0 Then
                            SQL = "INSERT INTO MJurnalD ( IDJurnal, IDDepartemen, IDAkun, " & _
                                  "Debet, DebetA, Kurs, IDMataUang, Kredit, KreditA, IsBalancing,Keterangan ) " & _
                                  "Values(" & IDJurnal & "," & _
                                  NullToLong(DefIDDepartemen) & "," & _
                                  IDAkunBalancing & "," & _
                                  "0," & _
                                  "0," & _
                                  "1," & _
                                  defIDMataUang & "," & _
                                  FixKoma(SelisihHisBalancing) & "," & _
                                  FixKoma(SelisihHisBalancing) & "," & _
                                  "0,'" & "PPN : " & FixApostropi(NullToStr(ds.Tables("MBeli").Rows(i).Item("Supplier"))) & ", Periode :" & Periode.ToString("MMM yyyy") & "')"
                            com.CommandText = SQL
                            com.ExecuteNonQuery()
                        End If
                    End If

                    If NBKP <> 0 Then 'Persediaan/Pembelian NBKP
                        SQL = "INSERT INTO MJurnalD ( IDJurnal, IDDepartemen, IDAkun, " & _
                              "Debet, DebetA, Kurs, IDMataUang, Kredit, KreditA, IsBalancing,Keterangan ) " & _
                              "Values(" & IDJurnal & "," & _
                              NullToLong(DefIDDepartemen) & "," & _
                              NullToLong(IDAkunPembelian) & "," & _
                              Bulatkan(NBKP, 0) & "," & _
                              Bulatkan(NBKP, 0) & ",1," & _
                              defIDMataUang & "," & _
                              "0," & _
                              "0," & _
                              "0,'" & "Pembelian Non BKP : " & FixApostropi(NullToStr(ds.Tables("MBeli").Rows(i).Item("Supplier"))) & ", Periode :" & Periode.ToString("MMM yyyy") & "')"
                        com.CommandText = SQL
                        com.ExecuteNonQuery()

                        'Balancing
                        SelisihHisBalancing = Bulatkan(NBKP, 2) - Bulatkan(NBKP, 0)
                        If SelisihHisBalancing < 0 Then
                            SQL = "INSERT INTO MJurnalD ( IDJurnal, IDDepartemen, IDAkun, " & _
                                  "Debet, DebetA, Kurs, IDMataUang, Kredit, KreditA, IsBalancing,Keterangan ) " & _
                                  "Values(" & IDJurnal & "," & _
                                  NullToLong(DefIDDepartemen) & "," & _
                                  IDAkunBalancing & "," & _
                                  FixKoma(-1.0# * SelisihHisBalancing) & "," & _
                                  FixKoma(-1.0# * SelisihHisBalancing) & "," & _
                                  "1," & _
                                  defIDMataUang & "," & _
                                  "0," & _
                                  "0," & _
                                  "0,'" & "Pembelian BKP : " & FixApostropi(NullToStr(ds.Tables("MBeli").Rows(i).Item("Supplier"))) & ", Periode :" & Periode.ToString("MMM yyyy") & "')"
                            com.CommandText = SQL
                            com.ExecuteNonQuery()
                        ElseIf SelisihHisBalancing > 0 Then
                            SQL = "INSERT INTO MJurnalD ( IDJurnal, IDDepartemen, IDAkun, " & _
                                  "Debet, DebetA, Kurs, IDMataUang, Kredit, KreditA, IsBalancing,Keterangan ) " & _
                                  "Values(" & IDJurnal & "," & _
                                  NullToLong(DefIDDepartemen) & "," & _
                                  IDAkunBalancing & "," & _
                                  "0," & _
                                  "0," & _
                                  "1," & _
                                  defIDMataUang & "," & _
                                  FixKoma(SelisihHisBalancing) & "," & _
                                  FixKoma(SelisihHisBalancing) & "," & _
                                  "0,'" & "Pembelian Non BKP : " & FixApostropi(NullToStr(ds.Tables("MBeli").Rows(i).Item("Supplier"))) & ", Periode :" & Periode.ToString("MMM yyyy") & "')"
                            com.CommandText = SQL
                            com.ExecuteNonQuery()
                        End If
                    End If

                    'Hutang
                    SQL = "INSERT INTO MJurnalD ( IDJurnal, IDDepartemen, IDAkun, " & _
                          "Debet, DebetA, Kurs, IDMataUang, Kredit, KreditA, IsBalancing,Keterangan ) " & _
                          "Values(" & IDJurnal & "," & _
                          NullToLong(DefIDDepartemen) & "," & _
                          NullToLong(ds.Tables("MBeli").Rows(i).Item("IDAkunHutang")) & "," & _
                          "0," & _
                          "0," & _
                          "1," & _
                          defIDMataUang & "," & _
                          Bulatkan(NullToDbl(ds.Tables("MBeli").Rows(i).Item("Total")), 0) & "," & _
                          Bulatkan(NullToDbl(ds.Tables("MBeli").Rows(i).Item("Total")), 0) & "," & _
                          "0,'" & "Pembelian : " & FixApostropi(NullToStr(ds.Tables("MBeli").Rows(i).Item("Supplier"))) & ", Periode :" & Periode.ToString("MMM yyyy") & "')"
                    com.CommandText = SQL
                    com.ExecuteNonQuery()

                    'Balancing
                    SelisihHisBalancing = Bulatkan(NullToDbl(ds.Tables("MBeli").Rows(i).Item("Total")), 2) - Bulatkan(NullToDbl(ds.Tables("MBeli").Rows(i).Item("Total")), 0)
                    If SelisihHisBalancing > 0 Then
                        SQL = "INSERT INTO MJurnalD ( IDJurnal, IDDepartemen, IDAkun, " & _
                              "Debet, DebetA, Kurs, IDMataUang, Kredit, KreditA, IsBalancing,Keterangan ) " & _
                              "Values(" & IDJurnal & "," & _
                              NullToLong(DefIDDepartemen) & "," & _
                              IDAkunBalancing & "," & _
                              FixKoma(SelisihHisBalancing) & "," & _
                              FixKoma(SelisihHisBalancing) & "," & _
                              "1," & _
                              defIDMataUang & "," & _
                              "0," & _
                              "0," & _
                              "0,'" & "Pembelian : " & FixApostropi(NullToStr(ds.Tables("MBeli").Rows(i).Item("Supplier"))) & ", Periode :" & Periode.ToString("MMM yyyy") & "')"
                        com.CommandText = SQL
                        com.ExecuteNonQuery()
                    ElseIf SelisihHisBalancing < 0 Then
                        SQL = "INSERT INTO MJurnalD ( IDJurnal, IDDepartemen, IDAkun, " & _
                              "Debet, DebetA, Kurs, IDMataUang, Kredit, KreditA, IsBalancing,Keterangan ) " & _
                              "Values(" & IDJurnal & "," & _
                              NullToLong(DefIDDepartemen) & "," & _
                              IDAkunBalancing & "," & _
                              "0," & _
                              "0," & _
                              "1," & _
                              defIDMataUang & "," & _
                              FixKoma(-1.0# * SelisihHisBalancing) & "," & _
                              FixKoma(-1.0# * SelisihHisBalancing) & "," & _
                              "0,'" & "Pembelian : " & FixApostropi(NullToStr(ds.Tables("MBeli").Rows(i).Item("Supplier"))) & ", Periode :" & Periode.ToString("MMM yyyy") & "')"
                        com.CommandText = SQL
                        com.ExecuteNonQuery()
                    End If
                Next

                SQL = "UPDATE MBeli SET IsPostedJurnal=1, TglPostedJurnal=Getdate(), IDUserPostedJurnal=" & IDUserAktif & " WHERE IDSupplier=" & IDSupplier & " AND MONTH(MBeli.Tanggal)=" & Periode.Month & " AND YEAR(MBeli.Tanggal)=" & Periode.Year
                com.CommandText = SQL
                com.ExecuteNonQuery()

                Tran.Commit()
            End If
        Catch ex As Exception
            XtraMessageBox.Show("Info Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
            If Not Tran Is Nothing Then
                Tran.Rollback()
            End If
        Finally
            If Not ds Is Nothing Then
                ds.Dispose()
            End If
            If ODa Is Nothing Then
                ODa.Dispose()
            End If
            If cn.State = ConnectionState.Open Then
                cn.Close()
            End If
            Tran = Nothing
            cn.Dispose()
            com.Dispose()
        End Try
    End Function
    Public Shared Function PostingJurnalBayarHutang(ByVal NoID As Long, Optional ByVal IsForce As Boolean = False) As Boolean
        Dim cn As New SqlConnection
        Dim com As New SqlCommand
        Dim ds As New DataSet
        Dim ODa As New SqlDataAdapter
        Dim SQL As String = ""
        Dim Tran As SqlTransaction = Nothing

        Dim IDJurnal As Long = -1
        Dim IDJurnalD As Long = -1
        'Dim DiscNotaProsen As Double = 0
        Dim SelisihHisBalancing As Double = 0
        Dim IDAkunBalancing As Long = -1
        'Dim IDAkunPembelian As Long = -1
        'Dim IDAkunReturPembelian As Long = -1
        'Dim IDAkunPenjualan As Long = -1
        'Dim IDAkunReturPenjualan As Long = -1

        'Dim IDAkunKasPembelian As Long = -1
        'Dim IDAkunBiayaPembelian As Long = -1
        Try
            If Not JurnalSudahDiKunci(NoID, 17, TanggalSystem) Then
                cn.ConnectionString = StrKonSql
                cn.Open()
                com.Connection = cn
                ODa.SelectCommand = com

                ''Persiapan
                IDAkunBalancing = NullToLong(EksekusiSQlSkalarNew("SELECT IDAkunSelisihBalancing FROM MSettingAkun"))
                'IDAkunPembelian = NullToLong(EksekusiSQlSkalarNew("SELECT IDAkunPembelian FROM MSettingAkun"))
                'IDAkunReturPembelian = NullToLong(EksekusiSQlSkalarNew("SELECT IDAkunReturPembelian FROM MSettingAkun"))
                ''IDAkunPenjualan = NullToLong(EksekusiSQlSkalarNew("SELECT IDAkunPenjualan FROM MSettingAkun"))
                ''IDAkunReturPenjualan = NullToLong(EksekusiSQlSkalarNew("SELECT IDAkunReturPenjualan FROM MSettingAkun"))
                'IDAkunKasPembelian = NullToLong(EksekusiSQlSkalarNew("SELECT IDAkunKasPembelian FROM MSettingAkun"))
                'IDAkunBiayaPembelian = NullToLong(EksekusiSQlSkalarNew("SELECT IDAkunBiayaPembelian FROM MSettingAkun"))

                Tran = cn.BeginTransaction("MBayarHutang")
                com.Transaction = Tran

                'Mulai
                SQL = "DELETE FROM MJurnalD WHERE IDJurnal IN (SELECT ID FROM MJurnal WHERE IDJenisTransaksi=17 AND IDTransaksi=" & NoID & ")"
                com.CommandText = SQL
                com.ExecuteNonQuery()

                SQL = "DELETE FROM MJurnal WHERE IDJenisTransaksi=17 AND IDTransaksi=" & NoID
                com.CommandText = SQL
                com.ExecuteNonQuery()

                com.CommandText = "SELECT MAX(ID) FROM MJurnal"
                IDJurnal = NullToLong(com.ExecuteScalar()) + 1 'GetNewID("MJurnal", "ID")
                SQL = "INSERT INTO MJurnal (ID, IDDepartemenUser,Tanggal, Kode, Keterangan, IDJenisTransaksi,IDTransaksi, IDUSerEntry,IDUserPosting ) " & _
                      " SELECT " & IDJurnal & " AS ID, " & DefIDDepartemen & ", MBayarHutang.TglKembali, MBayarHutang.Kode, MBayarHutang.Keterangan, 17 AS IDJenisTransaksi,NoID, IDUserEntry," & IDUserAktif & " AS IDUserPosting " & _
                      " FROM MBayarHutang " & _
                      " WHERE MBayarHutang.NoID=" & NoID
                com.CommandText = SQL
                com.ExecuteNonQuery()


                SQL = "SELECT MAlamat.Nama AS NamaSupplier, " & DefIDDepartemen & " AS IDDepartemen, MAlamat.IDAkunHutang, MBayarHutang.Kode, MBayarHutang.Total, MBank.IDAkun AS IDAKunBank FROM (MBayarHutang INNER JOIN MAlamat ON MAlamat.NoID=MBayarHutang.IDAlamat) LEFT JOIN MBank ON MBayarHutang.IDBank=MBank.NoID WHERE MBayarHutang.NoID=" & NoID
                com.CommandText = SQL
                If Not ds.Tables("MBayarHutangD") Is Nothing Then
                    ds.Tables("MBayarHutangD").Clear()
                End If
                ODa.Fill(ds, "MBayarHutangD")
                For i As Integer = 0 To ds.Tables("MBayarHutangD").Rows.Count - 1
                    'Pembayaran Hutang
                    SQL = "INSERT INTO MJurnalD ( IDJurnal, IDDepartemen, IDAkun, " & _
                          "Debet, DebetA, Kurs, IDMataUang, Kredit, KreditA, IsBalancing,Keterangan ) " & _
                          "Values(" & IDJurnal & "," & _
                          NullToLong(ds.Tables("MBayarHutangD").Rows(i).Item("IDDepartemen")) & "," & _
                          NullToLong(ds.Tables("MBayarHutangD").Rows(i).Item("IDAkunHutang")) & "," & _
                          Bulatkan(NullToDbl(ds.Tables("MBayarHutangD").Rows(i).Item("Total")), 0) & "," & _
                          Bulatkan(NullToDbl(ds.Tables("MBayarHutangD").Rows(i).Item("Total")), 0) & ",1," & _
                          defIDMataUang & "," & _
                          "0," & _
                          "0," & _
                          "0,'" & "Pembayaran Hutang : " & FixApostropi(NullToStr(ds.Tables("MBayarHutangD").Rows(i).Item("NamaSupplier"))) & ", No :" & FixApostropi(NullToStr(ds.Tables("MBayarHutangD").Rows(i).Item("Kode"))) & "')"
                    com.CommandText = SQL
                    com.ExecuteNonQuery()

                    'Balancing
                    SelisihHisBalancing = Bulatkan(NullToDbl(ds.Tables("MBayarHutangD").Rows(i).Item("Total")), 2) - Bulatkan(NullToDbl(ds.Tables("MBayarHutangD").Rows(i).Item("Total")), 0)
                    If SelisihHisBalancing < 0 Then
                        SQL = "INSERT INTO MJurnalD ( IDJurnal, IDDepartemen, IDAkun, " & _
                              "Debet, DebetA, Kurs, IDMataUang, Kredit, KreditA, IsBalancing,Keterangan ) " & _
                              "Values(" & IDJurnal & "," & _
                              NullToLong(ds.Tables("MBeliD").Rows(i).Item("IDDepartemen")) & "," & _
                              IDAkunBalancing & "," & _
                              FixKoma(-1.0# * SelisihHisBalancing) & "," & _
                              FixKoma(-1.0# * SelisihHisBalancing) & "," & _
                              "1," & _
                              defIDMataUang & "," & _
                              "0," & _
                              "0," & _
                              "0,'" & "Pembayaran Hutang : " & FixApostropi(NullToStr(ds.Tables("MBayarHutangD").Rows(i).Item("NamaSupplier"))) & ", No :" & FixApostropi(NullToStr(ds.Tables("MBayarHutangD").Rows(i).Item("Kode"))) & "')"
                        com.CommandText = SQL
                        com.ExecuteNonQuery()
                    ElseIf SelisihHisBalancing > 0 Then
                        SQL = "INSERT INTO MJurnalD ( IDJurnal, IDDepartemen, IDAkun, " & _
                              "Debet, DebetA, Kurs, IDMataUang, Kredit, KreditA, IsBalancing,Keterangan ) " & _
                              "Values(" & IDJurnal & "," & _
                              NullToLong(ds.Tables("MBeliD").Rows(i).Item("IDDepartemen")) & "," & _
                              IDAkunBalancing & "," & _
                              "0," & _
                              "0," & _
                              "1," & _
                              defIDMataUang & "," & _
                              FixKoma(SelisihHisBalancing) & "," & _
                              FixKoma(SelisihHisBalancing) & "," & _
                              "0,'" & "Pembayaran Hutang : " & FixApostropi(NullToStr(ds.Tables("MBayarHutangD").Rows(i).Item("NamaSupplier"))) & ", No :" & FixApostropi(NullToStr(ds.Tables("MBayarHutangD").Rows(i).Item("Kode"))) & "')"
                        com.CommandText = SQL
                        com.ExecuteNonQuery()
                    End If

                    'Kas Bank
                    SQL = "INSERT INTO MJurnalD ( IDJurnal, IDDepartemen, IDAkun, " & _
                          "Debet, DebetA, Kurs, IDMataUang, Kredit, KreditA, IsBalancing,Keterangan ) " & _
                          "Values(" & IDJurnal & "," & _
                          NullToLong(ds.Tables("MBayarHutangD").Rows(i).Item("IDDepartemen")) & "," & _
                          NullToLong(ds.Tables("MBayarHutangD").Rows(i).Item("IDAkunBank")) & "," & _
                          "0," & _
                          "0,1," & _
                          defIDMataUang & "," & _
                          Bulatkan(NullToDbl(ds.Tables("MBayarHutangD").Rows(i).Item("Total")), 0) & "," & _
                          Bulatkan(NullToDbl(ds.Tables("MBayarHutangD").Rows(i).Item("Total")), 0) & "," & _
                          "0,'" & "Pembayaran Hutang : " & FixApostropi(NullToStr(ds.Tables("MBayarHutangD").Rows(i).Item("NamaSupplier"))) & ", No :" & FixApostropi(NullToStr(ds.Tables("MBayarHutangD").Rows(i).Item("Kode"))) & "')"
                    com.CommandText = SQL
                    com.ExecuteNonQuery()
                Next

                SQL = "UPDATE MBayarHutang SET IsPostedJurnal=1, TglPostedJurnal=Getdate(), IDUserPostedJurnal=" & IDUserAktif & " WHERE NoID=" & NoID
                com.CommandText = SQL
                com.ExecuteNonQuery()

                If Not Tran Is Nothing Then
                    Tran.Commit()
                End If
            End If
        Catch ex As Exception
            XtraMessageBox.Show("Info Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
            If Not Tran Is Nothing Then
                Tran.Rollback()
            End If
        Finally
            Tran.Dispose()
            If Not ds Is Nothing Then
                ds.Dispose()
            End If
            If ODa Is Nothing Then
                ODa.Dispose()
            End If
            If cn.State = ConnectionState.Open Then
                cn.Close()
            End If
            cn.Dispose()
            com.Dispose()
        End Try
    End Function
    Public Shared Function UnPostingJurnalBayarHutang(ByVal NoID As Long, Optional ByVal IsForce As Boolean = False) As Boolean
        Dim cn As New SqlConnection
        Dim com As New SqlCommand
        Dim SQL As String = ""
        Try
            If Not JurnalSudahDiKunci(NoID, 17, TanggalSystem) Then
                cn.ConnectionString = StrKonSql
                cn.Open()
                com.Connection = cn

                'Mulai
                SQL = "DELETE FROM MJurnalD WHERE IDJurnal IN (SELECT ID FROM MJurnal WHERE IDJenisTransaksi=17 AND IDTransaksi=" & NoID & ")"
                com.CommandText = SQL
                com.ExecuteNonQuery()

                SQL = "DELETE FROM MJurnal WHERE IDJenisTransaksi=17 AND IDTransaksi=" & NoID
                com.CommandText = SQL
                com.ExecuteNonQuery()

                SQL = "UPDATE MBayarHutang SET IsPostedJurnal=0, TglPostedJurnal=NULL, IDUserPostedJurnal=NULL WHERE NoID=" & NoID
                com.CommandText = SQL
                com.ExecuteNonQuery()
            End If
        Catch ex As Exception
            XtraMessageBox.Show("Info Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
        Finally
            If cn.State = ConnectionState.Open Then
                cn.Close()
            End If
            cn.Dispose()
            com.Dispose()
        End Try
    End Function
    Public Shared Function UnPostingJurnalPembelian(ByVal IDSupplier As Long, ByVal Periode As Date, Optional ByVal IsForce As Boolean = False) As Boolean
        Dim cn As New SqlConnection
        Dim com As New SqlCommand
        Dim SQL As String = ""
        Try
            If Not JurnalSudahDiKunci(IDSupplier, 2, TanggalSystem) Then
                cn.ConnectionString = StrKonSql
                cn.Open()
                com.Connection = cn

                'Mulai
                SQL = "DELETE FROM MJurnalD WHERE IDJurnal IN (SELECT ID FROM MJurnal WHERE IDJenisTransaksi=2 AND IDAlamat=" & IDSupplier & " AND MONTH(Tanggal)=" & FixKoma(Periode.Month) & " AND YEAR(Tanggal)=" & FixKoma(Periode.Year) & ")"
                com.CommandText = SQL
                com.ExecuteNonQuery()

                SQL = "DELETE FROM MJurnal WHERE IDJenisTransaksi=2 AND IDAlamat=" & IDSupplier & " AND MONTH(Tanggal)=" & FixKoma(Periode.Month) & " AND YEAR(Tanggal)=" & FixKoma(Periode.Year)
                com.CommandText = SQL
                com.ExecuteNonQuery()

                SQL = "UPDATE MBeli SET IsPostedJurnal=0, TglPostedJurnal=NULL, IDUserPostedJurnal=NULL WHERE IDSupplier=" & IDSupplier & " AND MONTH(MBeli.Tanggal)=" & Periode.Month & " AND YEAR(MBeli.Tanggal)=" & Periode.Year
                com.CommandText = SQL
                com.ExecuteNonQuery()
            End If
        Catch ex As Exception
            XtraMessageBox.Show("Info Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
        Finally
            If cn.State = ConnectionState.Open Then
                cn.Close()
            End If
            cn.Dispose()
            com.Dispose()
        End Try
    End Function
    Public Shared Function PostingJurnalReturPembelian(ByVal IDSupplier As Long, ByVal Periode As Date, Optional ByVal IsForce As Boolean = False) As Boolean
        Dim cn As New SqlConnection
        Dim com As New SqlCommand
        Dim ds As New DataSet
        Dim ODa As New SqlDataAdapter
        Dim SQL As String = ""
        Dim Tran As SqlTransaction = Nothing

        Dim IsPKP As Boolean = False
        Dim IDJurnal As Long = -1
        Dim IDJurnalD As Long = -1
        Dim Kode As String = ""
        'Dim DiscNotaProsen As Double = 0

        Dim BKP As Double = 0.0
        Dim NBKP As Double = 0.0
        Dim PPN As Double = 0.0

        Dim SelisihHisBalancing As Double = 0
        Dim IDAkunBalancing As Long = -1
        Dim IDAkunReturPembelian As Long = -1
        Dim IDAkunBiayaReturPembelian As Long = -1
        Try
            If Not JurnalSudahDiKunci(IDSupplier, 3, Periode) Then
                cn.ConnectionString = StrKonSql
                cn.Open()
                com.Connection = cn
                ODa.SelectCommand = com
                Tran = cn.BeginTransaction("MJurnal")
                com.Transaction = Tran

                'Persiapan
                IDAkunBalancing = NullToLong(EksekusiSQlSkalarNew("SELECT IDAkunSelisihBalancing FROM MSettingAkun"))
                IDAkunReturPembelian = NullToLong(EksekusiSQlSkalarNew("SELECT IDAkunReturPembelian FROM MSettingAkun"))
                IDAkunBiayaReturPembelian = NullToLong(EksekusiSQlSkalarNew("SELECT IDAkunBiayaReturPembelian FROM MSettingAkun"))

                'Mulai
                SQL = "SELECT MAlamat.NoID, MAlamat.IDAkunHutang, MAlamat.Kode AS KodeSupplier, MAlamat.Nama AS Supplier, MAlamat.IsPKP AS [PKP / Non PKP], SUM(X.BKP) AS BKP, SUM(X.NBKP) AS NonBKP, SUM(X.BKP)+SUM(X.NBKP) AS Total, " & vbCrLf & _
                      " (SELECT MJurnal.Tanggal FROM MJurnal WHERE MJurnal.IDJenisTransaksi=3 AND MJurnal.IDAlamat=MAlamat.NoID AND MONTH(MJurnal.Tanggal)=" & Month(Periode) & " AND YEAR(MJurnal.Tanggal)=" & Year(Periode) & ") AS TglPosting, " & vbCrLf & _
                      " (SELECT MUser.Nama FROM MJurnal INNER JOIN MUser ON MUser.NoID=MJurnal.IDUserEntry WHERE MJurnal.IDJenisTransaksi=3 AND MJurnal.IDAlamat=MAlamat.NoID AND MONTH(MJurnal.Tanggal)=" & Month(Periode) & " AND YEAR(MJurnal.Tanggal)=" & Year(Periode) & ") AS UserPosting " & vbCrLf & _
                      " FROM MAlamat" & vbCrLf & _
                      " LEFT JOIN " & vbCrLf & _
                      " (SELECT MBarang.NoID, MReturBeli.IDSupplier, MReturBeli.IsProsesPajak," & vbCrLf & _
                      " SUM(CASE WHEN IsNull(MReturBeli.IsProsesPajak,0) = 0 THEN 0 ELSE MReturBeliD.Jumlah END) AS BKP, SUM(CASE WHEN IsNull(MReturBeli.IsProsesPajak,0) = 1 THEN 0 ELSE MReturBeliD.Jumlah END) AS NBKP" & vbCrLf & _
                      " FROM MReturBeli " & vbCrLf & _
                      " INNER JOIN MReturBeliD ON MReturBeli.NoID=MReturBeliD.IDReturBeli" & vbCrLf & _
                      " INNER JOIN MBarang ON MBarang.NoID=MReturBeliD.IDBarang" & vbCrLf & _
                      " WHERE MReturBeli.IDSupplier=" & IDSupplier & " AND MReturBeli.IsPosted=1 AND IsNull(MReturBeli.IsTanpaBarang,0)=0 AND MONTH(MReturBeli.Tanggal)=" & Month(Periode) & " AND YEAR(MReturBeli.Tanggal)=" & Year(Periode) & vbCrLf & _
                      " GROUP BY MBarang.NoID, MReturBeli.IDSupplier, MReturBeli.IsProsesPajak" & vbCrLf & _
                      " UNION ALL" & vbCrLf & _
                      " SELECT -1, MReturBeli.IDSupplier, MReturBeli.IsProsesPajak, " & vbCrLf & _
                      " (CASE WHEN IsNull(MReturBeli.IsProsesPajak,0) = 0 THEN 0 ELSE MReturBeli.Total END) AS BKP," & vbCrLf & _
                      " (CASE WHEN IsNull(MReturBeli.IsProsesPajak,0) = 1 THEN 0 ELSE MReturBeli.Total END) AS NBKP" & vbCrLf & _
                      " FROM MReturBeli " & vbCrLf & _
                      " LEFT JOIN MKategori ON MKategori.NoID=MReturBeli.IDKategori" & vbCrLf & _
                      " WHERE MReturBeli.IDSupplier=" & IDSupplier & " AND MReturBeli.IsPosted=1 AND IsNull(MReturBeli.IsTanpaBarang,0)=1 AND MReturBeli.Total>0 AND MONTH(MReturBeli.Tanggal)=" & Month(Periode) & " AND YEAR(MReturBeli.Tanggal)=" & Year(Periode) & vbCrLf & _
                      " UNION ALL" & vbCrLf & _
                      " SELECT -1, MReturBeli.IDSupplier, -1, -1*MReturBeli.DiskonNotaTotal, 0" & vbCrLf & _
                      " FROM MReturBeli" & vbCrLf & _
                      " WHERE MReturBeli.IDSupplier=" & IDSupplier & " AND MReturBeli.IsPosted=1 AND IsNull(MReturBeli.IsTanpaBarang,0)=0 AND MONTH(MReturBeli.Tanggal)=" & Month(Periode) & " AND YEAR(MReturBeli.Tanggal)=" & Year(Periode) & vbCrLf & _
                      " ) X ON X.IDSupplier=MAlamat.NoID " & vbCrLf & _
                      " WHERE MAlamat.NoID=" & IDSupplier & " AND MAlamat.IsSupplier=1 AND MAlamat.IsActive=1" & vbCrLf & _
                      " GROUP BY MAlamat.NoID, MAlamat.IDAkunHutang, MAlamat.Kode, MAlamat.Nama, MAlamat.IsPKP "
                com.CommandText = SQL
                If Not ds.Tables("MReturBeli") Is Nothing Then
                    ds.Tables("MReturBeli").Clear()
                End If
                ODa.Fill(ds, "MReturBeli")
                For i As Integer = 0 To ds.Tables("MReturBeli").Rows.Count - 1
                    SQL = "DELETE FROM MJurnalD WHERE IDJurnal IN (SELECT ID FROM MJurnal WHERE IDJenisTransaksi=3 AND IDAlamat=" & IDSupplier & " AND MONTH(Tanggal)=" & FixKoma(Periode.Month) & " AND YEAR(Tanggal)=" & FixKoma(Periode.Year) & ")"
                    com.CommandText = SQL
                    com.ExecuteNonQuery()

                    SQL = "DELETE FROM MJurnal WHERE IDJenisTransaksi=3 AND IDAlamat=" & IDSupplier & " AND MONTH(Tanggal)=" & FixKoma(Periode.Month) & " AND YEAR(Tanggal)=" & FixKoma(Periode.Year)
                    com.CommandText = SQL
                    com.ExecuteNonQuery()

                    SQL = "SELECT count(ID) as x from Mjurnal where month(tanggal)=" & Month(Periode) & " and year(tanggal)=" & Year(Periode)
                    com.CommandText = SQL
                    Kode = "RB" & Format(NullToLong(com.ExecuteScalar()) + 1, "0000") & "/" & Format(Month(Periode), "00") & "/" & Format(Year(Periode), "0000")
                    SQL = "SELECT MAX(MJurnal.ID) FROM MJurnal "
                    com.CommandText = SQL
                    IDJurnal = NullToLong(com.ExecuteScalar) + 1
                    SQL = "INSERT INTO MJurnal (ID, IDAlamat, IDDepartemenUser, Tanggal, Kode, Keterangan, IDJenisTransaksi, IDTransaksi, IDUserEntry, IDUserPosting) VALUES (" & _
                          IDJurnal & "," & IDSupplier & "," & DefIDDepartemen & ", '" & CDate(Periode.ToString("yyyy/MM/01")).AddMonths(1).AddDays(-1).ToString("yyyy/MM/dd") & "', '" & FixApostropi(Kode) & "', 'Retur Pembelian " & FixApostropi(NullToStr(ds.Tables("MReturBeli").Rows(i).Item("Supplier"))) & ", " & Periode.ToString("MMM yyyy") & "' , 3, -1, " & IDUserAktif & "," & IDUserAktif & ")"
                    com.CommandText = SQL
                    com.ExecuteNonQuery()
                    IsPKP = NullToBool(ds.Tables("MReturBeli").Rows(i).Item("PKP / Non PKP"))

                    BKP = Bulatkan(NullToDbl(ds.Tables("MReturBeli").Rows(i).Item("BKP")) / 1.1, 0)
                    NBKP = NullToDbl(ds.Tables("MReturBeli").Rows(i).Item("NonBKP"))
                    PPN = NullToDbl(ds.Tables("MReturBeli").Rows(i).Item("BKP")) - Bulatkan(NullToDbl(ds.Tables("MReturBeli").Rows(i).Item("BKP")) / 1.1, 0)

                    If BKP <> 0 Then 'Persediaan/Retur Pembelian BKP
                        SQL = "INSERT INTO MJurnalD ( IDJurnal, IDDepartemen, IDAkun, " & _
                              "Debet, DebetA, Kurs, IDMataUang, Kredit, KreditA, IsBalancing,Keterangan ) " & _
                              "Values(" & IDJurnal & "," & _
                              NullToLong(DefIDDepartemen) & "," & _
                              NullToLong(IDAkunReturPembelian) & "," & _
                              "0," & _
                              "0,1," & _
                              defIDMataUang & "," & _
                              Bulatkan(BKP, 0) & "," & _
                              Bulatkan(BKP, 0) & "," & _
                              "0,'" & "Retur Pembelian BKP : " & FixApostropi(NullToStr(ds.Tables("MReturBeli").Rows(i).Item("Supplier"))) & ", Periode :" & Periode.ToString("MMM yyyy") & "')"
                        com.CommandText = SQL
                        com.ExecuteNonQuery()

                        'Balancing
                        SelisihHisBalancing = Bulatkan(BKP, 2) - Bulatkan(BKP, 0)
                        If SelisihHisBalancing < 0 Then
                            SQL = "INSERT INTO MJurnalD ( IDJurnal, IDDepartemen, IDAkun, " & _
                                  "Debet, DebetA, Kurs, IDMataUang, Kredit, KreditA, IsBalancing,Keterangan ) " & _
                                  "Values(" & IDJurnal & "," & _
                                  NullToLong(DefIDDepartemen) & "," & _
                                  IDAkunBalancing & "," & _
                                  "0," & _
                                  "0," & _
                                  "1," & _
                                  defIDMataUang & "," & _
                                  FixKoma(-1.0# * SelisihHisBalancing) & "," & _
                                  FixKoma(-1.0# * SelisihHisBalancing) & "," & _
                                  "0,'" & "Retur Pembelian BKP : " & FixApostropi(NullToStr(ds.Tables("MReturBeli").Rows(i).Item("Supplier"))) & ", Periode :" & Periode.ToString("MMM yyyy") & "')"
                            com.CommandText = SQL
                            com.ExecuteNonQuery()
                        ElseIf SelisihHisBalancing > 0 Then
                            SQL = "INSERT INTO MJurnalD ( IDJurnal, IDDepartemen, IDAkun, " & _
                                  "Debet, DebetA, Kurs, IDMataUang, Kredit, KreditA, IsBalancing,Keterangan ) " & _
                                  "Values(" & IDJurnal & "," & _
                                  NullToLong(DefIDDepartemen) & "," & _
                                  IDAkunBalancing & "," & _
                                  FixKoma(SelisihHisBalancing) & "," & _
                                  FixKoma(SelisihHisBalancing) & "," & _
                                  "1," & _
                                  defIDMataUang & "," & _
                                  "0," & _
                                  "0," & _
                                  "0,'" & "Retur Pembelian BKP : " & FixApostropi(NullToStr(ds.Tables("MReturBeli").Rows(i).Item("Supplier"))) & ", Periode :" & Periode.ToString("MMM yyyy") & "')"
                            com.CommandText = SQL
                            com.ExecuteNonQuery()
                        End If
                    End If

                    If PPN <> 0 Then 'PPN
                        SQL = "INSERT INTO MJurnalD ( IDJurnal, IDDepartemen, IDAkun, " & _
                              "Debet, DebetA, Kurs, IDMataUang, Kredit, KreditA, IsBalancing,Keterangan ) " & _
                              "Values(" & IDJurnal & "," & _
                              NullToLong(DefIDDepartemen) & "," & _
                              NullToLong(IDAkunBiayaReturPembelian) & "," & _
                              "0," & _
                              "0," & _
                              "1," & _
                              defIDMataUang & "," & _
                              Bulatkan(PPN, 0) & "," & _
                              Bulatkan(PPN, 0) & "," & _
                              "0,'" & "PPN : " & FixApostropi(NullToStr(ds.Tables("MReturBeli").Rows(i).Item("Supplier"))) & ", Periode :" & Periode.ToString("MMM yyyy") & "')"
                        com.CommandText = SQL
                        com.ExecuteNonQuery()

                        'Balancing
                        SelisihHisBalancing = Bulatkan(PPN, 2) - Bulatkan(PPN, 0)
                        If SelisihHisBalancing < 0 Then
                            SQL = "INSERT INTO MJurnalD ( IDJurnal, IDDepartemen, IDAkun, " & _
                                  "Debet, DebetA, Kurs, IDMataUang, Kredit, KreditA, IsBalancing,Keterangan ) " & _
                                  "Values(" & IDJurnal & "," & _
                                  NullToLong(DefIDDepartemen) & "," & _
                                  IDAkunBalancing & "," & _
                                  "0," & _
                                  "0," & _
                                  "1," & _
                                  defIDMataUang & "," & _
                                  FixKoma(-1.0# * SelisihHisBalancing) & "," & _
                                  FixKoma(-1.0# * SelisihHisBalancing) & "," & _
                                  "0,'" & "PPN : " & FixApostropi(NullToStr(ds.Tables("MReturBeli").Rows(i).Item("Supplier"))) & ", Periode :" & Periode.ToString("MMM yyyy") & "')"
                            com.CommandText = SQL
                            com.ExecuteNonQuery()
                        ElseIf SelisihHisBalancing > 0 Then
                            SQL = "INSERT INTO MJurnalD ( IDJurnal, IDDepartemen, IDAkun, " & _
                                  "Debet, DebetA, Kurs, IDMataUang, Kredit, KreditA, IsBalancing,Keterangan ) " & _
                                  "Values(" & IDJurnal & "," & _
                                  NullToLong(DefIDDepartemen) & "," & _
                                  IDAkunBalancing & "," & _
                                  FixKoma(SelisihHisBalancing) & "," & _
                                  FixKoma(SelisihHisBalancing) & "," & _
                                  "1," & _
                                  defIDMataUang & "," & _
                                  "0," & _
                                  "0," & _
                                  "0,'" & "PPN : " & FixApostropi(NullToStr(ds.Tables("MReturBeli").Rows(i).Item("Supplier"))) & ", Periode :" & Periode.ToString("MMM yyyy") & "')"
                            com.CommandText = SQL
                            com.ExecuteNonQuery()
                        End If
                    End If

                    If NBKP <> 0 Then 'Persediaan/Retur Pembelian NBKP
                        SQL = "INSERT INTO MJurnalD ( IDJurnal, IDDepartemen, IDAkun, " & _
                              "Debet, DebetA, Kurs, IDMataUang, Kredit, KreditA, IsBalancing,Keterangan ) " & _
                              "Values(" & IDJurnal & "," & _
                              NullToLong(DefIDDepartemen) & "," & _
                              NullToLong(IDAkunReturPembelian) & "," & _
                              "0," & _
                              "0," & _
                              "1," & _
                              defIDMataUang & "," & _
                              Bulatkan(NBKP, 0) & "," & _
                              Bulatkan(NBKP, 0) & "," & _
                              "0,'" & "Retur Pembelian Non BKP : " & FixApostropi(NullToStr(ds.Tables("MReturBeli").Rows(i).Item("Supplier"))) & ", Periode :" & Periode.ToString("MMM yyyy") & "')"
                        com.CommandText = SQL
                        com.ExecuteNonQuery()

                        'Balancing
                        SelisihHisBalancing = Bulatkan(NBKP, 2) - Bulatkan(NBKP, 0)
                        If SelisihHisBalancing < 0 Then
                            SQL = "INSERT INTO MJurnalD ( IDJurnal, IDDepartemen, IDAkun, " & _
                                  "Debet, DebetA, Kurs, IDMataUang, Kredit, KreditA, IsBalancing,Keterangan ) " & _
                                  "Values(" & IDJurnal & "," & _
                                  NullToLong(DefIDDepartemen) & "," & _
                                  IDAkunBalancing & "," & _
                                  "0," & _
                                  "0," & _
                                  "1," & _
                                  defIDMataUang & "," & _
                                  FixKoma(-1.0# * SelisihHisBalancing) & "," & _
                                  FixKoma(-1.0# * SelisihHisBalancing) & "," & _
                                  "0,'" & "Retur Pembelian BKP : " & FixApostropi(NullToStr(ds.Tables("MReturBeli").Rows(i).Item("Supplier"))) & ", Periode :" & Periode.ToString("MMM yyyy") & "')"
                            com.CommandText = SQL
                            com.ExecuteNonQuery()
                        ElseIf SelisihHisBalancing > 0 Then
                            SQL = "INSERT INTO MJurnalD ( IDJurnal, IDDepartemen, IDAkun, " & _
                                  "Debet, DebetA, Kurs, IDMataUang, Kredit, KreditA, IsBalancing,Keterangan ) " & _
                                  "Values(" & IDJurnal & "," & _
                                  NullToLong(DefIDDepartemen) & "," & _
                                  IDAkunBalancing & "," & _
                                  FixKoma(SelisihHisBalancing) & "," & _
                                  FixKoma(SelisihHisBalancing) & "," & _
                                  "1," & _
                                  defIDMataUang & "," & _
                                  "0," & _
                                  "0," & _
                                  "0,'" & "Retur Pembelian Non BKP : " & FixApostropi(NullToStr(ds.Tables("MReturBeli").Rows(i).Item("Supplier"))) & ", Periode :" & Periode.ToString("MMM yyyy") & "')"
                            com.CommandText = SQL
                            com.ExecuteNonQuery()
                        End If
                    End If

                    'Hutang
                    SQL = "INSERT INTO MJurnalD ( IDJurnal, IDDepartemen, IDAkun, " & _
                          "Debet, DebetA, Kurs, IDMataUang, Kredit, KreditA, IsBalancing,Keterangan ) " & _
                          "Values(" & IDJurnal & "," & _
                          NullToLong(DefIDDepartemen) & "," & _
                          NullToLong(ds.Tables("MReturBeli").Rows(i).Item("IDAkunHutang")) & "," & _
                          Bulatkan(NullToDbl(ds.Tables("MReturBeli").Rows(i).Item("Total")), 0) & "," & _
                          Bulatkan(NullToDbl(ds.Tables("MReturBeli").Rows(i).Item("Total")), 0) & "," & _
                          "1," & _
                          defIDMataUang & "," & _
                          "0," & _
                          "0," & _
                          "0,'" & "Retur Pembelian : " & FixApostropi(NullToStr(ds.Tables("MReturBeli").Rows(i).Item("Supplier"))) & ", Periode :" & Periode.ToString("MMM yyyy") & "')"
                    com.CommandText = SQL
                    com.ExecuteNonQuery()

                    'Balancing
                    SelisihHisBalancing = Bulatkan(NullToDbl(ds.Tables("MReturBeli").Rows(i).Item("Total")), 2) - Bulatkan(NullToDbl(ds.Tables("MReturBeli").Rows(i).Item("Total")), 0)
                    If SelisihHisBalancing > 0 Then
                        SQL = "INSERT INTO MJurnalD ( IDJurnal, IDDepartemen, IDAkun, " & _
                              "Debet, DebetA, Kurs, IDMataUang, Kredit, KreditA, IsBalancing,Keterangan ) " & _
                              "Values(" & IDJurnal & "," & _
                              NullToLong(DefIDDepartemen) & "," & _
                              IDAkunBalancing & "," & _
                              FixKoma(SelisihHisBalancing) & "," & _
                              FixKoma(SelisihHisBalancing) & "," & _
                              "1," & _
                              defIDMataUang & "," & _
                              "0," & _
                              "0," & _
                              "0,'" & "Retur Pembelian : " & FixApostropi(NullToStr(ds.Tables("MReturBeli").Rows(i).Item("Supplier"))) & ", Periode :" & Periode.ToString("MMM yyyy") & "')"
                        com.CommandText = SQL
                        com.ExecuteNonQuery()
                    ElseIf SelisihHisBalancing < 0 Then
                        SQL = "INSERT INTO MJurnalD ( IDJurnal, IDDepartemen, IDAkun, " & _
                              "Debet, DebetA, Kurs, IDMataUang, Kredit, KreditA, IsBalancing,Keterangan ) " & _
                              "Values(" & IDJurnal & "," & _
                              NullToLong(DefIDDepartemen) & "," & _
                              IDAkunBalancing & "," & _
                              "0," & _
                              "0," & _
                              "1," & _
                              defIDMataUang & "," & _
                              FixKoma(-1.0# * SelisihHisBalancing) & "," & _
                              FixKoma(-1.0# * SelisihHisBalancing) & "," & _
                              "0,'" & "Retur Pembelian : " & FixApostropi(NullToStr(ds.Tables("MReturBeli").Rows(i).Item("Supplier"))) & ", Periode :" & Periode.ToString("MMM yyyy") & "')"
                        com.CommandText = SQL
                        com.ExecuteNonQuery()
                    End If
                Next

                SQL = "UPDATE MReturBeli SET IsPostedJurnal=1, TglPostedJurnal=Getdate(), IDUserPostedJurnal=" & IDUserAktif & " WHERE IDSupplier=" & IDSupplier & " AND MONTH(MReturBeli.Tanggal)=" & Periode.Month & " AND YEAR(MReturBeli.Tanggal)=" & Periode.Year
                com.CommandText = SQL
                com.ExecuteNonQuery()

                Tran.Commit()
            End If
        Catch ex As Exception
            XtraMessageBox.Show("Info Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
            If Not Tran Is Nothing Then
                Tran.Rollback()
            End If
        Finally
            If Not ds Is Nothing Then
                ds.Dispose()
            End If
            If ODa Is Nothing Then
                ODa.Dispose()
            End If
            If cn.State = ConnectionState.Open Then
                cn.Close()
            End If
            Tran = Nothing
            cn.Dispose()
            com.Dispose()
        End Try
    End Function
    Public Shared Function UnPostingJurnalReturPembelian(ByVal IDSupplier As Long, ByVal Periode As Date, Optional ByVal IsForce As Boolean = False) As Boolean
        Dim cn As New SqlConnection
        Dim com As New SqlCommand
        Dim SQL As String = ""
        Try
            If Not JurnalSudahDiKunci(IDSupplier, 3, TanggalSystem) Then
                cn.ConnectionString = StrKonSql
                cn.Open()
                com.Connection = cn

                'Mulai
                SQL = "DELETE FROM MJurnalD WHERE IDJurnal IN (SELECT ID FROM MJurnal WHERE IDJenisTransaksi=3 AND IDAlamat=" & IDSupplier & " AND MONTH(Tanggal)=" & FixKoma(Periode.Month) & " AND YEAR(Tanggal)=" & FixKoma(Periode.Year) & ")"
                com.CommandText = SQL
                com.ExecuteNonQuery()

                SQL = "DELETE FROM MJurnal WHERE IDJenisTransaksi=3 AND IDAlamat=" & IDSupplier & " AND MONTH(Tanggal)=" & FixKoma(Periode.Month) & " AND YEAR(Tanggal)=" & FixKoma(Periode.Year)
                com.CommandText = SQL
                com.ExecuteNonQuery()

                SQL = "UPDATE MBeli SET IsPostedJurnal=0, TglPostedJurnal=NULL, IDUserPostedJurnal=NULL WHERE IDSupplier=" & IDSupplier & " AND MONTH(MBeli.Tanggal)=" & Periode.Month & " AND YEAR(MBeli.Tanggal)=" & Periode.Year
                com.CommandText = SQL
                com.ExecuteNonQuery()
            End If
        Catch ex As Exception
            XtraMessageBox.Show("Info Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
        Finally
            If cn.State = ConnectionState.Open Then
                cn.Close()
            End If
            cn.Dispose()
            com.Dispose()
        End Try
    End Function
    Public Shared Function PostingJurnalPenjualan(ByVal NoID As Long, Optional ByVal IsForce As Boolean = False) As Boolean
        Dim cn As New SqlConnection
        Dim com As New SqlCommand
        Dim ds As New DataSet
        Dim ODa As New SqlDataAdapter
        Dim SQL As String = ""
        Dim Tran As SqlTransaction = Nothing

        Dim IDJurnal As Long = -1
        Dim IDJurnalD As Long = -1
        Dim DiscNotaProsen As Double = 0
        Dim DiscNotaRp As Double = 0
        Dim SelisihHisBalancing As Double = 0
        Dim IDAkunBalancing As Long = -1
        Dim IDAkunPenjualan As Long = -1
        Dim IDAkunPotonganPenjualan As Long = -1

        Dim IDAkunKasPenjualan As Long = -1
        Dim IDAkunBiayaPenjualan As Long = -1
        Try
            If Not JurnalSudahDiKunci(NoID, 6, TanggalSystem) Then
                cn.ConnectionString = StrKonSql
                cn.Open()
                com.Connection = cn
                ODa.SelectCommand = com
                'Persiapan
                IDAkunBalancing = NullToLong(EksekusiSQlSkalarNew("SELECT IDAkunSelisihBalancing FROM MSettingAkun"))
                IDAkunPenjualan = NullToLong(EksekusiSQlSkalarNew("SELECT IDAkunPenjualan FROM MSettingAkun"))
                IDAkunKasPenjualan = NullToLong(EksekusiSQlSkalarNew("SELECT IDAkunKasPenjualan FROM MSettingAkun"))
                IDAkunBiayaPenjualan = NullToLong(EksekusiSQlSkalarNew("SELECT IDAkunBiayaPenjualan FROM MSettingAkun"))
                IDAkunPotonganPenjualan = NullToLong(EksekusiSQlSkalarNew("SELECT IDAkunPotonganPenjualan FROM MSettingAkun"))

                Tran = cn.BeginTransaction("MJual")
                com.Transaction = Tran

                SQL = "DELETE FROM MJurnalD WHERE IDJurnal IN (SELECT ID FROM MJurnal WHERE IDJenisTransaksi=6 AND IDTransaksi=" & NoID & ")"
                com.CommandText = SQL
                com.ExecuteNonQuery()

                SQL = "DELETE FROM MJurnal WHERE IDJenisTransaksi=6 AND IDTransaksi=" & NoID
                com.CommandText = SQL
                com.ExecuteNonQuery()

                com.CommandText = "SELECT MAX(ID) FROM MJurnal"
                IDJurnal = NullToLong(com.ExecuteScalar()) + 1 'GetNewID("MJurnal", "ID")
                SQL = "INSERT INTO MJurnal (ID, IDDepartemenUser,Tanggal, Kode, Keterangan, IDJenisTransaksi,IDTransaksi, IDUSerEntry,IDUserPosting ) " & _
                      " SELECT " & IDJurnal & " AS ID,IDDepartemenUser, MJual.Tanggal, CASE WHEN IsNull(IsPOS,0)=1 THEN MJual.KodeReff ELSE MJual.Kode END, MJual.Keterangan, 6 AS IDJenisTransaksi,NoID, IDUser," & IDUserAktif & " AS IDUserPosting " & _
                      " FROM MJual " & _
                      " WHERE MJual.NoID=" & NoID
                com.CommandText = SQL
                com.ExecuteNonQuery()

                'Persediaan Penjualan Di Group
                com.CommandText = "SELECT MAlamat.Nama AS NamaCustomer, MJual.Kode, MBarang.IDAkunPersediaan, SUM(MJualD.HargaPokok * MJualD.Qty * MJualD.Konversi) AS Total " & _
                                  " FROM (MJual INNER JOIN MJualD ON Mjual.NoID=MJualD.IDJUal) " & _
                                  " LEFT JOIN MAlamat ON MAlamat.NoID=MJual.IDCustomer " & _
                                  " LEFT JOIN MBarang ON MBarang.NoID=MJualD.IDBArang " & _
                                  " WHERE MJual.NoID=" & NoID & _
                                  " GROUP BY MAlamat.Nama, MJual.Kode, MBarang.IDAkunPersediaan"
                If Not ds.Tables("MJualD") Is Nothing Then
                    ds.Tables("MJualD").Clear()
                End If
                ODa.Fill(ds, "MJualD")
                For i As Integer = 0 To ds.Tables("MJualD").Rows.Count - 1
                    'Persediaan
                    SQL = "INSERT INTO MJurnalD ( IDJurnal, IDDepartemen, IDAkun, " & _
                          "Debet, DebetA, Kurs, IDMataUang, Kredit, KreditA, IsBalancing,Keterangan ) " & _
                          "Values(" & IDJurnal & "," & _
                          NullToLong(DefIDDepartemen) & "," & _
                          NullToLong(ds.Tables("MJualD").Rows(i).Item("IDAkunPersediaan")) & "," & _
                          "0," & _
                          "0,1," & _
                          defIDMataUang & "," & _
                          FixKoma(Bulatkan(NullToDbl(ds.Tables("MJualD").Rows(i).Item("Total")), 0)) & ", " & _
                          FixKoma(Bulatkan(NullToDbl(ds.Tables("MJualD").Rows(i).Item("Total")), 0)) & ", " & _
                          "0,'" & "Penjualan : " & FixApostropi(NullToStr(ds.Tables("MJualD").Rows(i).Item("NamaCustomer"))) & ", No :" & FixApostropi(NullToStr(ds.Tables("MJualD").Rows(i).Item("Kode"))) & "')"
                    com.CommandText = SQL
                    com.ExecuteNonQuery()

                    'Balancing
                    SelisihHisBalancing = Bulatkan(NullToDbl(ds.Tables("MjualD").Rows(i).Item("Total")), 2) - Bulatkan(NullToDbl(ds.Tables("MjualD").Rows(i).Item("Total")), 0)
                    If SelisihHisBalancing > 0 Then
                        SQL = "INSERT INTO MJurnalD ( IDJurnal, IDDepartemen, IDAkun, " & _
                              "Debet, DebetA, Kurs, IDMataUang, Kredit, KreditA, IsBalancing,Keterangan ) " & _
                              "Values(" & IDJurnal & "," & _
                              NullToLong(DefIDDepartemen) & "," & _
                              IDAkunBalancing & "," & _
                              FixKoma(SelisihHisBalancing) & "," & _
                              FixKoma(SelisihHisBalancing) & "," & _
                              "1," & _
                              defIDMataUang & "," & _
                              "0," & _
                              "0," & _
                              "0,'" & "Penjualan : " & FixApostropi(NullToStr(ds.Tables("MjualD").Rows(i).Item("NamaCustomer"))) & ", No :" & FixApostropi(NullToStr(ds.Tables("MJualD").Rows(i).Item("Kode"))) & "')"
                        com.CommandText = SQL
                        com.ExecuteNonQuery()
                    ElseIf SelisihHisBalancing < 0 Then
                        SQL = "INSERT INTO MJurnalD ( IDJurnal, IDDepartemen, IDAkun, " & _
                              "Debet, DebetA, Kurs, IDMataUang, Kredit, KreditA, IsBalancing,Keterangan ) " & _
                              "Values(" & IDJurnal & "," & _
                              NullToLong(DefIDDepartemen) & "," & _
                              IDAkunBalancing & "," & _
                              "0," & _
                              "0," & _
                              "1," & _
                              defIDMataUang & "," & _
                              FixKoma(-1.0# * SelisihHisBalancing) & "," & _
                              FixKoma(-1.0# * SelisihHisBalancing) & "," & _
                              "0,'" & "Penjualan : " & FixApostropi(NullToStr(ds.Tables("MjualD").Rows(i).Item("NamaCustomer"))) & ", No :" & FixApostropi(NullToStr(ds.Tables("MJualD").Rows(i).Item("Kode"))) & "')"
                        com.CommandText = SQL
                        com.ExecuteNonQuery()
                    End If
                Next

                'HPP Penjualan di Group
                com.CommandText = "SELECT MAlamat.Nama AS NamaCustomer, MJual.Kode, MBarang.IDAkunHPP, SUM(MJualD.HargaPokok * MJualD.Qty * MJualD.Konversi) AS Total " & _
                                  " FROM (MJual INNER JOIN MJualD ON Mjual.NoID=MJualD.IDJUal) " & _
                                  " LEFT JOIN MAlamat ON MAlamat.NoID=MJual.IDCustomer " & _
                                  " LEFT JOIN MBarang ON MBarang.NoID=MJualD.IDBArang " & _
                                  " WHERE MJual.NoID=" & NoID & _
                                  " GROUP BY MAlamat.Nama, MJual.Kode, MBarang.IDAkunHPP"
                If Not ds.Tables("MJualD") Is Nothing Then
                    ds.Tables("MJualD").Clear()
                End If
                ODa.Fill(ds, "MJualD")
                For i As Integer = 0 To ds.Tables("MJualD").Rows.Count - 1
                    SQL = "INSERT INTO MJurnalD ( IDJurnal, IDDepartemen, IDAkun, " & _
                      "Debet, DebetA, Kurs, IDMataUang, Kredit, KreditA, IsBalancing,Keterangan ) " & _
                      "Values(" & IDJurnal & "," & _
                      NullToLong(DefIDDepartemen) & "," & _
                      NullToLong(ds.Tables("MJualD").Rows(i).Item("IDAkunHPP")) & "," & _
                      FixKoma(Bulatkan(NullToDbl(ds.Tables("MJualD").Rows(i).Item("Total")), 0)) & "," & _
                      FixKoma(Bulatkan(NullToDbl(ds.Tables("MJualD").Rows(i).Item("Total")), 0)) & ",1," & _
                      defIDMataUang & "," & _
                      "0, " & _
                      "0, " & _
                      "0,'" & "Penjualan : " & FixApostropi(NullToStr(ds.Tables("MJualD").Rows(i).Item("NamaCustomer"))) & ", No :" & FixApostropi(NullToStr(ds.Tables("MJualD").Rows(i).Item("Kode"))) & "')"
                    com.CommandText = SQL
                    com.ExecuteNonQuery()

                    'Balancing
                    SelisihHisBalancing = Bulatkan(NullToDbl(ds.Tables("MjualD").Rows(i).Item("Total")), 2) - Bulatkan(NullToDbl(ds.Tables("MjualD").Rows(i).Item("Total")), 0)
                    If SelisihHisBalancing < 0 Then
                        SQL = "INSERT INTO MJurnalD ( IDJurnal, IDDepartemen, IDAkun, " & _
                              "Debet, DebetA, Kurs, IDMataUang, Kredit, KreditA, IsBalancing,Keterangan ) " & _
                              "Values(" & IDJurnal & "," & _
                              NullToLong(DefIDDepartemen) & "," & _
                              IDAkunBalancing & "," & _
                              FixKoma(-1.0# * SelisihHisBalancing) & "," & _
                              FixKoma(-1.0# * SelisihHisBalancing) & "," & _
                              "1," & _
                              defIDMataUang & "," & _
                              "0," & _
                              "0," & _
                              "0,'" & "Penjualan : " & FixApostropi(NullToStr(ds.Tables("MjualD").Rows(i).Item("NamaCustomer"))) & ", No :" & FixApostropi(NullToStr(ds.Tables("MJualD").Rows(i).Item("Kode"))) & "')"
                        com.CommandText = SQL
                        com.ExecuteNonQuery()
                    ElseIf SelisihHisBalancing > 0 Then
                        SQL = "INSERT INTO MJurnalD ( IDJurnal, IDDepartemen, IDAkun, " & _
                              "Debet, DebetA, Kurs, IDMataUang, Kredit, KreditA, IsBalancing,Keterangan ) " & _
                              "Values(" & IDJurnal & "," & _
                              NullToLong(DefIDDepartemen) & "," & _
                              IDAkunBalancing & "," & _
                              "0," & _
                              "0," & _
                              "1," & _
                              defIDMataUang & "," & _
                              FixKoma(SelisihHisBalancing) & "," & _
                              FixKoma(SelisihHisBalancing) & "," & _
                              "0,'" & "Penjualan : " & FixApostropi(NullToStr(ds.Tables("MjualD").Rows(i).Item("NamaCustomer"))) & ", No :" & FixApostropi(NullToStr(ds.Tables("MJualD").Rows(i).Item("Kode"))) & "')"
                        com.CommandText = SQL
                        com.ExecuteNonQuery()
                    End If
                Next

                'Penjualan
                com.CommandText = "SELECT MAlamat.Nama AS NamaCustomer, MJual.Kode, MJual.Total+IsNull(MJual.Biaya,0)+IsNull(MJual.DiskonNotaTotal,0) AS Total, MJual.Bayar, MJual.DiskonNotaTotal, MJual.Kas, MJual.Bank, MJual.IDBank, MJual.Sisa, MJual.Biaya, MJual.IDDepartemen, Mjual.DiskonNotaTotal, MAlamat.IDAkunPiutang, MBank.IDAkun AS IDAkunBank " & _
                                  " FROM MJual " & _
                                  " LEFT JOIN MAlamat ON MAlamat.NoID=MJual.IDCustomer " & _
                                  " LEFT JOIN MBank ON MBank.NoID=MJual.IDBank " & _
                                  " WHERE MJual.NoID=" & NoID
                If Not ds.Tables("MJualD") Is Nothing Then
                    ds.Tables("MJualD").Clear()
                End If
                ODa.Fill(ds, "MJualD")
                For i As Integer = 0 To ds.Tables("MJualD").Rows.Count - 1
                    'Penjualan
                    SQL = "INSERT INTO MJurnalD ( IDJurnal, IDDepartemen, IDAkun, " & _
                          "Debet, DebetA, Kurs, IDMataUang, Kredit, KreditA, IsBalancing,Keterangan ) " & _
                          "Values(" & IDJurnal & "," & _
                          NullToLong(DefIDDepartemen) & "," & _
                          NullToLong(IDAkunPenjualan) & "," & _
                          "0," & _
                          "0,1," & _
                          defIDMataUang & "," & _
                          FixKoma(Bulatkan(NullToDbl(ds.Tables("MJualD").Rows(i).Item("Total")), 0)) & ", " & _
                          FixKoma(Bulatkan(NullToDbl(ds.Tables("MJualD").Rows(i).Item("Total")), 0)) & ", " & _
                          "0,'" & "Penjualan : " & FixApostropi(NullToStr(ds.Tables("MJualD").Rows(i).Item("NamaCustomer"))) & ", No :" & FixApostropi(NullToStr(ds.Tables("MJualD").Rows(i).Item("Kode"))) & "')"
                    com.CommandText = SQL
                    com.ExecuteNonQuery()

                    'Potongan Jika Ada DiscNota>=1
                    If NullToDbl(ds.Tables("MJualD").Rows(0).Item("DiskonNotaTotal")) > 0 Then 'Disc Nota Debet
                        SQL = "INSERT INTO MJurnalD ( IDJurnal, IDDepartemen, IDAkun, " & _
                              "Debet, DebetA, Kurs, IDMataUang, Kredit, KreditA,IsBalancing,Keterangan ) " & _
                              "Values(" & IDJurnal & "," & _
                              NullToLong(ds.Tables("MJualD").Rows(0).Item("IDDepartemen")) & "," & _
                              NullToLong(ds.Tables("MJualD").Rows(0).Item("IDAkunPiutang")) & "," & _
                              FixKoma(Bulatkan(NullToDbl(ds.Tables("MJualD").Rows(0).Item("DiskonNotaTotal")), 0)) & "," & _
                              FixKoma(Bulatkan(NullToDbl(ds.Tables("MJualD").Rows(0).Item("DiskonNotaTotal")), 0)) & "," & FixKoma(1) & "," & _
                              defIDMataUang & "," & _
                              "0," & _
                              "0," & _
                              "0,'" & "Penjualan : " & FixApostropi(NullToStr(ds.Tables("MJualD").Rows(0).Item("NamaCustomer"))) & ", No :" & FixApostropi(NullToStr(ds.Tables("MJualD").Rows(0).Item("Kode"))) & "')"
                        com.CommandText = SQL
                        com.ExecuteNonQuery()
                    End If

                    'Piutang Jika Ada Sisa>=1 
                    If NullToDbl(ds.Tables("MJualD").Rows(0).Item("Sisa")) > 0 Then 'Piutang Debet
                        SQL = "INSERT INTO MJurnalD ( IDJurnal, IDDepartemen, IDAkun, " & _
                              "Debet, DebetA, Kurs, IDMataUang, Kredit, KreditA,IsBalancing,Keterangan ) " & _
                              "Values(" & IDJurnal & "," & _
                              NullToLong(ds.Tables("MJualD").Rows(0).Item("IDDepartemen")) & "," & _
                              NullToLong(ds.Tables("MJualD").Rows(0).Item("IDAkunPiutang")) & "," & _
                              FixKoma(Bulatkan(NullToDbl(ds.Tables("MJualD").Rows(0).Item("Sisa")), 0)) & "," & _
                              FixKoma(Bulatkan(NullToDbl(ds.Tables("MJualD").Rows(0).Item("Sisa")), 0)) & "," & FixKoma(1) & "," & _
                              defIDMataUang & "," & _
                              "0," & _
                              "0," & _
                              "0,'" & "Penjualan : " & FixApostropi(NullToStr(ds.Tables("MJualD").Rows(0).Item("NamaCustomer"))) & ", No :" & FixApostropi(NullToStr(ds.Tables("MJualD").Rows(0).Item("Kode"))) & "')"
                        com.CommandText = SQL
                        com.ExecuteNonQuery()
                    End If

                    'Biaya Jika Ada Biaya>=1 
                    If NullToDbl(ds.Tables("MJualD").Rows(0).Item("Biaya")) > 0 Then 'Biaya Debet
                        SQL = "INSERT INTO MJurnalD ( IDJurnal, IDDepartemen, IDAkun, " & _
                              "Debet, DebetA, Kurs, IDMataUang, Kredit, KreditA,IsBalancing,Keterangan ) " & _
                              "Values(" & IDJurnal & "," & _
                              NullToLong(ds.Tables("MJualD").Rows(0).Item("IDDepartemen")) & "," & _
                              NullToLong(IDAkunBiayaPenjualan) & "," & _
                              FixKoma(Bulatkan(NullToDbl(ds.Tables("MJualD").Rows(0).Item("Biaya")), 0)) & "," & _
                              FixKoma(Bulatkan(NullToDbl(ds.Tables("MJualD").Rows(0).Item("Biaya")), 0)) & "," & FixKoma(1) & "," & _
                              defIDMataUang & "," & _
                              "0," & _
                              "0," & _
                              "0,'" & "Penjualan : " & FixApostropi(NullToStr(ds.Tables("MJualD").Rows(0).Item("NamaCustomer"))) & ", No :" & FixApostropi(NullToStr(ds.Tables("MJualD").Rows(0).Item("Kode"))) & "')"
                        com.CommandText = SQL
                        com.ExecuteNonQuery()
                    End If

                    'Kas/Tunai Jika Ada Kas>=1 
                    If NullToDbl(ds.Tables("MJualD").Rows(0).Item("Kas")) > 0 Then 'Kas Debet
                        SQL = "INSERT INTO MJurnalD ( IDJurnal, IDDepartemen, IDAkun, " & _
                              "Debet, DebetA, Kurs, IDMataUang, Kredit, KreditA,IsBalancing,Keterangan ) " & _
                              "Values(" & IDJurnal & "," & _
                              NullToLong(ds.Tables("MJualD").Rows(0).Item("IDDepartemen")) & "," & _
                              NullToLong(IDAkunKasPenjualan) & "," & _
                              FixKoma(Bulatkan(NullToDbl(ds.Tables("MJualD").Rows(0).Item("Kas")), 0)) & "," & _
                              FixKoma(Bulatkan(NullToDbl(ds.Tables("MJualD").Rows(0).Item("Kas")), 0)) & "," & FixKoma(1) & "," & _
                              defIDMataUang & "," & _
                              "0," & _
                              "0," & _
                              "0,'" & "Penjualan : " & FixApostropi(NullToStr(ds.Tables("MJualD").Rows(0).Item("NamaCustomer"))) & ", No :" & FixApostropi(NullToStr(ds.Tables("MJualD").Rows(0).Item("Kode"))) & "')"
                        com.CommandText = SQL
                        com.ExecuteNonQuery()
                    End If

                    'Bank Jika Ada Bank>=1 
                    If NullToDbl(ds.Tables("MJualD").Rows(0).Item("Bank")) > 0 Then 'Bank Debet
                        SQL = "INSERT INTO MJurnalD ( IDJurnal, IDDepartemen, IDAkun, " & _
                              "Debet, DebetA, Kurs, IDMataUang, Kredit, KreditA,IsBalancing,Keterangan ) " & _
                              "Values(" & IDJurnal & "," & _
                              NullToLong(ds.Tables("MJualD").Rows(0).Item("IDDepartemen")) & "," & _
                              NullToLong(ds.Tables("MJualD").Rows(0).Item("IDAkunBank")) & "," & _
                              FixKoma(Bulatkan(NullToDbl(ds.Tables("MJualD").Rows(0).Item("Bank")), 0)) & "," & _
                              FixKoma(Bulatkan(NullToDbl(ds.Tables("MJualD").Rows(0).Item("Bank")), 0)) & "," & FixKoma(1) & "," & _
                              defIDMataUang & "," & _
                              "0," & _
                              "0," & _
                              "0,'" & "Penjualan : " & FixApostropi(NullToStr(ds.Tables("MJualD").Rows(0).Item("NamaCustomer"))) & ", No :" & FixApostropi(NullToStr(ds.Tables("MJualD").Rows(0).Item("Kode"))) & "')"
                        com.CommandText = SQL
                        com.ExecuteNonQuery()
                    End If

                    'Balancing
                    SelisihHisBalancing = Bulatkan(NullToDbl(ds.Tables("MJualD").Rows(i).Item("Total")), 2) - Bulatkan(NullToDbl(ds.Tables("MJualD").Rows(i).Item("Total")), 0)
                    If SelisihHisBalancing < 0 Then
                        SQL = "INSERT INTO MJurnalD ( IDJurnal, IDDepartemen, IDAkun, " & _
                              "Debet, DebetA, Kurs, IDMataUang, Kredit, KreditA, IsBalancing,Keterangan ) " & _
                              "Values(" & IDJurnal & "," & _
                              NullToLong(DefIDDepartemen) & "," & _
                              IDAkunBalancing & "," & _
                              FixKoma(-1.0# * SelisihHisBalancing) & "," & _
                              FixKoma(-1.0# * SelisihHisBalancing) & "," & _
                              "1," & _
                              defIDMataUang & "," & _
                              "0," & _
                              "0," & _
                              "0,'" & "Penjualan : " & FixApostropi(NullToStr(ds.Tables("MjualD").Rows(i).Item("NamaCustomer"))) & ", No :" & FixApostropi(NullToStr(ds.Tables("MJualD").Rows(i).Item("Kode"))) & "')"
                        com.CommandText = SQL
                        com.ExecuteNonQuery()
                    ElseIf SelisihHisBalancing > 0 Then
                        SQL = "INSERT INTO MJurnalD ( IDJurnal, IDDepartemen, IDAkun, " & _
                              "Debet, DebetA, Kurs, IDMataUang, Kredit, KreditA, IsBalancing,Keterangan ) " & _
                              "Values(" & IDJurnal & "," & _
                              NullToLong(DefIDDepartemen) & "," & _
                              IDAkunBalancing & "," & _
                              "0," & _
                              "0," & _
                              "1," & _
                              defIDMataUang & "," & _
                              FixKoma(SelisihHisBalancing) & "," & _
                              FixKoma(SelisihHisBalancing) & "," & _
                              "0,'" & "Penjualan : " & FixApostropi(NullToStr(ds.Tables("MjualD").Rows(i).Item("NamaCustomer"))) & ", No :" & FixApostropi(NullToStr(ds.Tables("MJualD").Rows(i).Item("Kode"))) & "')"
                        com.CommandText = SQL
                        com.ExecuteNonQuery()
                    End If
                Next

                SQL = "UPDATE MJual SET IsPostedJurnal=1, TglPostedJurnal=Getdate(), IDUserPostedJurnal=" & IDUserAktif & " WHERE NoID=" & NoID
                com.CommandText = SQL
                com.ExecuteNonQuery()
                If Not Tran Is Nothing Then
                    Tran.Commit()
                End If
            End If
        Catch ex As Exception
            XtraMessageBox.Show("Info Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
            If Not Tran Is Nothing Then
                Tran.Rollback()
            End If
        Finally
            Tran.Dispose()
            If Not ds Is Nothing Then
                ds.Dispose()
            End If
            If ODa Is Nothing Then
                ODa.Dispose()
            End If
            If cn.State = ConnectionState.Open Then
                cn.Close()
            End If
            cn.Dispose()
            com.Dispose()
        End Try
    End Function
    Public Shared Function UnPostingJurnalPenjualan(ByVal NoID As Long, Optional ByVal IsForce As Boolean = False) As Boolean
        Dim cn As New SqlConnection
        Dim com As New SqlCommand
        Dim SQL As String = ""
        Try
            If Not JurnalSudahDiKunci(NoID, 6, TanggalSystem) Then
                cn.ConnectionString = StrKonSql
                cn.Open()
                com.Connection = cn

                'Mulai
                SQL = "DELETE FROM MJurnalD WHERE IDJurnal IN (SELECT ID FROM MJurnal WHERE IDJenisTransaksi=6 AND IDTransaksi=" & NoID & ")"
                com.CommandText = SQL
                com.ExecuteNonQuery()

                SQL = "DELETE FROM MJurnal WHERE IDJenisTransaksi=6 AND IDTransaksi=" & NoID
                com.CommandText = SQL
                com.ExecuteNonQuery()

                SQL = "UPDATE MJual SET IsPostedJurnal=0, TglPostedJurnal=NULL, IDUserPostedJurnal=NULL WHERE NoID=" & NoID
                com.CommandText = SQL
                com.ExecuteNonQuery()
            End If
        Catch ex As Exception
            XtraMessageBox.Show("Info Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
        Finally
            If cn.State = ConnectionState.Open Then
                cn.Close()
            End If
            cn.Dispose()
            com.Dispose()
        End Try
    End Function
End Class
