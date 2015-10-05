Imports System
Imports System.Windows.Forms
Imports DevExpress.XtraEditors
Imports System.Data.SqlClient

Public Class frmMutasiData
    Private Sub frmMutasiData_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        TglMutasiDataBarang.DateTime = DateAdd(DateInterval.Day, -1, TanggalSystem)
        DateEdit1.DateTime = DateAdd(DateInterval.Day, -1, TanggalSystem)
    End Sub

    Private Sub cmdSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSave.Click
        If XtraMessageBox.Show("Yakin akan memproses Memindahkan Saldo Stok Barang sampai tanggal " & TglMutasiDataBarang.DateTime.ToString("dd-MM-yyyy") & "?" & vbCrLf & _
                               "Lihat skenario pemindahan saldo stok.", NamaAplikasi, MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) + Windows.Forms.DialogResult.Yes Then
            cmdSave.Enabled = False
            RepMutasiData.MutasiSaldoStok(TglMutasiDataBarang.DateTime, PB1)
            cmdSave.Enabled = True
        End If
    End Sub

    Private Sub SimpleButton1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SimpleButton1.Click
        If XtraMessageBox.Show("Yakin akan memproses Menghapus data Penjualan Kasir sampai tanggal " & DateEdit1.DateTime.ToString("dd-MM-yyyy") & "?" & vbCrLf & _
                               "Lihat skenario penghapusan data Penjualan Kasir.", NamaAplikasi, MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) + Windows.Forms.DialogResult.Yes Then
            SimpleButton1.Enabled = False
            RepMutasiData.HapusDataPenjualan(DateEdit1.DateTime, ProgressBarControl1)
            SimpleButton1.Enabled = True
        End If
    End Sub
End Class


Public Class RepMutasiData
    Public Shared Sub MutasiSaldoStok(ByVal TglSampai As Date, Optional ByRef PB As ProgressBarControl = Nothing)
        Using cn As New SqlConnection(StrKonSql)
            Dim com As New SqlCommand
            Dim oDA As New SqlDataAdapter
            Dim ds As New DataSet
            Dim Tran As SqlTransaction = Nothing
            Dim SQL As String = "", IDSaldoAwal As Long = -1, IDSaldoAwalD As Long = -1, IDKartuStok As Long = -1
            Try
                If Not PB Is Nothing Then
                    PB.Visible = True
                    PB.Position = 0
                End If
                cn.Open()
                com.Connection = cn
                com.CommandTimeout = cn.ConnectionTimeout
                Tran = cn.BeginTransaction("Mutasi Data")
                com.Transaction = Tran

                oDA.SelectCommand = com

                SQL = "SELECT ROW_NUMBER() OVER(ORDER BY MKartuStok.IDBarangD) AS NoID" & vbCrLf & _
                      " ,MKartuStok.[IDBarang]" & vbCrLf & _
                      " ,1 AS IDSatuan" & vbCrLf & _
                      " ,MKartuStok.[IDGudang]" & vbCrLf & _
                      " ,1 AS Konversi" & vbCrLf & _
                      " ,SUM(MKartuStok.Konversi*(MKartuStok.QtyMasuk-MKartuStok.QtyKeluar)) AS [Qty]" & vbCrLf & _
                      " ,SUM(MKartuStok.Konversi*(MKartuStok.QtyMasuk-MKartuStok.QtyKeluar)) AS [QtyPCS]" & vbCrLf & _
                      " ,MBarang.HargaBeliPcs AS [HargaPokok]" & vbCrLf & _
                      " ,SUM(MKartuStok.Konversi*(MKartuStok.QtyMasuk-MKartuStok.QtyKeluar))*MBarang.HargaBeliPcs AS [Jumlah]" & vbCrLf & _
                      " ,'Potong Saldo Stok " & TglSampai.ToString("dd-MM-yyyy") & "' AS [Keterangan]" & vbCrLf & _
                      " ,MKartuStok.[IDBarangD]" & vbCrLf & _
                      " FROM MKartuStok (NOLOCK) " & vbCrLf & _
                      " INNER JOIN MBarang (NOLOCK) ON MKartuStok.IDBarang=MBarang.NoID" & vbCrLf & _
                      " INNER JOIN MBarangD (NOLOCK) ON MKartuStok.IDBarangD=MBarangD.NoID AND MKartuStok.IDBarang=MBarangD.IDBarang" & vbCrLf & _
                      " WHERE MKartuStok.Tanggal<'" & TglSampai.ToString("yyyy-MM-dd") & "'" & vbCrLf & _
                      " GROUP BY MKartuStok.IDBarang, MKartuStok.IDBarangD, MKartuStok.IDGudang, MBarang.HargaBeliPcs" & vbCrLf & _
                      " ORDER BY MKartuStok.IDBarangD"
                com.CommandText = SQL
                oDA.Fill(ds, "Data")

                If Not PB Is Nothing Then
                    PB.Position = 1 / (ds.Tables("Data").Rows.Count + 3) * 100
                End If
                Application.DoEvents()

                SQL = "DELETE K" & vbCrLf & _
                      " FROM MKartuStok (NOLOCK) K" & vbCrLf & _
                      " INNER JOIN MBarang (NOLOCK) ON K.IDBarang=MBarang.NoID" & vbCrLf & _
                      " INNER JOIN MBarangD (NOLOCK) ON K.IDBarangD=MBarangD.NoID AND K.IDBarang=MBarangD.IDBarang" & vbCrLf & _
                      " WHERE K.Tanggal<'" & TglSampai.ToString("yyyy-MM-dd") & "'"
                com.CommandText = SQL
                com.ExecuteNonQuery()

                If Not PB Is Nothing Then
                    PB.Position = 2 / (ds.Tables("Data").Rows.Count + 3) * 100
                End If
                Application.DoEvents()

                com.CommandText = "SELECT ISNULL(MAX([NoID]),0)+1 FROM [dbo].[MSaldoAwalPersediaan]"
                IDSaldoAwal = NullToLong(com.ExecuteScalar())
                SQL = "INSERT INTO INSERT INTO [dbo].[MSaldoAwalPersediaan] ([NoID],[Nomor],[Kode],[KodeReff],[Tanggal],[Jam],[IDWilayah],[IDPegawai],[IDUserEntry],[IDUserEdit],[IDUserPosting],[Total],[IsPosted],[TglEntri],[TglEdit],[TglPosting],[IDAdmin],[IDServerAsal],[IsSelesai],[Keterangan]) VALUES (" & vbCrLf & _
                      IDSaldoAwal & "," & IDSaldoAwal & ",'SA-" & TglSampai.ToString("yyMMdd") & "','','" & TglSampai.ToString("yyyy-MM-dd HH:mm:ss") & "','" & TglSampai.ToString("yyyy-MM-dd HH:mm:ss") & "',1," & IDUserAktif & "," & IDUserAktif & ",NULL," & IDUserAktif & ",0,1,GETDATE(),NULL,GETADTE()," & IDUserAktif & ",NULL,NULL,'Potong Saldo Stok " & TglSampai.ToString("dd-MM-yyyy") & "')"
                com.CommandText = SQL
                com.ExecuteNonQuery()
                If Not PB Is Nothing Then
                    PB.Position = 3 / (ds.Tables("Data").Rows.Count + 3) * 100
                End If
                Application.DoEvents()

                For i As Integer = 0 To ds.Tables("Data").Rows.Count - 1
                    If i = 0 Then
                        com.CommandText = "SELECT ISNULL(MAX([NoID]),0)+1 FROM [dbo].[MSaldoAwalPersediaanD]"
                        IDSaldoAwalD = NullToLong(com.ExecuteScalar)
                    Else
                        IDSaldoAwalD += 1
                    End If
                    com.CommandText = "INSERT INTO [dbo].[MSaldoAwalPersediaanD]" & vbCrLf & _
                                      "([NoID],[IDHeader],[IDBarang],[IDBarangD],[IDSatuan],[IDGudang],[Konversi],[Qty],[QtyPCS],[HargaPokok],[Jumlah],[Keterangan])" & vbCrLf & _
                                      NullToLong(com.ExecuteScalar) + 1 & "," & IDSaldoAwal & vbCrLf & _
                                      NullToLong(ds.Tables("Data").Rows(i).Item("IDBarang")) & "," & vbCrLf & _
                                      NullToLong(ds.Tables("Data").Rows(i).Item("IDBarangD")) & "," & vbCrLf & _
                                      NullToLong(ds.Tables("Data").Rows(i).Item("IDSatuan")) & "," & vbCrLf & _
                                      NullToLong(ds.Tables("Data").Rows(i).Item("IDGudang")) & "," & vbCrLf & _
                                      NullTolInt(ds.Tables("Data").Rows(i).Item("Konversi")) & "," & vbCrLf & _
                                      FixKoma(NullToDbl(ds.Tables("Data").Rows(i).Item("Qty"))) & "," & vbCrLf & _
                                      FixKoma(NullToDbl(ds.Tables("Data").Rows(i).Item("QtyPcs"))) & "," & vbCrLf & _
                                      FixKoma(NullToDbl(ds.Tables("Data").Rows(i).Item("HargaPokok"))) & "," & vbCrLf & _
                                      FixKoma(NullToDbl(ds.Tables("Data").Rows(i).Item("Jumlah"))) & "," & vbCrLf & _
                                      "'" & FixApostropi(NullToStr(ds.Tables("Data").Rows(i).Item("Keterangan"))) & "'" & vbCrLf & _
                                      ")"
                    com.CommandText = SQL
                    com.ExecuteNonQuery()

                    'Langsung Posting
                    If i = 0 Then
                        com.CommandText = "SELECT ISNULL(MAX([NoID]),0)+1 FROM [dbo].[MKartuStok]"
                        IDKartuStok = NullToLong(com.ExecuteScalar)
                    Else
                        IDKartuStok += 1
                    End If
                    SQL = "INSERT INTO [dbo].[MKartuStok]"
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
                    If NullToDbl(ds.Tables("Data").Rows(i).Item("Qty")) < 0 Then
                        SQL &= " ,[QtyKeluarA]"
                        SQL &= " ,[QtyKeluar]"
                    Else
                        SQL &= " ,[QtyMasukA]"
                        SQL &= " ,[QtyMasuk]"
                    End If
                    SQL &= " ,[HargaJualPerItem]"
                    SQL &= " ,[JumlahJual]"
                    If NullToDbl(ds.Tables("Data").Rows(i).Item("Qty")) < 0 Then
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
                    SQL &= " SELECT " & IDKartuStok & " AS NoID, MSaldoAwalPersediaan.Kode, MSaldoAwalPersediaanD.IDBarang, "
                    SQL &= " MWilayah.IDDepartemen, MSaldoAwalPersediaanD.IDGudang, "
                    SQL &= " MSaldoAwalPersediaan.Tanggal, 1 AS IDJenisTransaksi,  MSaldoAwalPersediaan.NoID AS IDTransaksi, "
                    SQL &= " MSaldoAwalPersediaanD.NoID AS IDTransaksiDetil, MSaldoAwalPersediaanD.Keterangan, (ISNULL(ABS(MSaldoAwalPersediaanD.Qty),0)*ISNULL(MSaldoAwalPersediaanD.Konversi,0)) AS KeluarA, ABS(MSaldoAwalPersediaanD.Qty) AS Keluar,"
                    SQL &= " 0 AS HrgJualPerItem, 0 AS Jumlah, 0 AS Masuk,0 AS MasukA,"
                    SQL &= " 0,0,0 AS HPPKeluar, "
                    SQL &= " 0 AS JumHPPKeluar, (ISNULL(MSaldoAwalPersediaanD.Qty,0)*ISNULL(MSaldoAwalPersediaanD.Konversi,0)) as QtyAkhir, 0,"
                    SQL &= " 0, MBarang.IDJenis, MSaldoAwalPersediaanD.IDSatuan, "
                    SQL &= " IsNull(MSaldoAwalPersediaanD.Konversi,0), 0 AS IDAlamat, MSaldoAwalPersediaanD.IDBarangD "
                    SQL &= " FROM MSaldoAwalPersediaanD LEFT JOIN MSaldoAwalPersediaan ON MSaldoAwalPersediaan.NoID=MSaldoAwalPersediaanD.IDHeader "
                    SQL &= " LEFT JOIN (MGudang LEFT JOIN MWilayah ON MWilayah.NoID=MGudang.IDWilayah) ON MSaldoAwalPersediaanD.IDGudang=MGudang.NoID"
                    SQL &= " LEFT JOIN MBarang ON MBarang.NoID=MSaldoAwalPersediaanD.IDBarang"
                    SQL &= " WHERE MSaldoAwalPersediaanD.NoID=" & IDSaldoAwalD
                    com.CommandText = SQL
                    com.ExecuteNonQuery()

                    If Not PB Is Nothing Then
                        PB.Position = (i + 4) / (ds.Tables("Data").Rows.Count + 3) * 100
                    End If
                    i += 1
                    Application.DoEvents()
                Next
            Catch ex As Exception
                XtraMessageBox.Show("Info Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
                If Not Tran Is Nothing Then
                    Tran.Rollback()
                    Tran = Nothing
                End If
            Finally
                If Not Tran Is Nothing Then
                    Tran.Commit()
                    Tran = Nothing
                End If
                com.Dispose()
                oDA.Dispose()
                ds.Dispose()
                If Not PB Is Nothing Then
                    PB.Visible = False
                End If
            End Try
        End Using
    End Sub
    Public Shared Sub HapusDataPenjualan(ByVal TglSampai As Date, Optional ByRef PB As ProgressBarControl = Nothing)
        Using cn As New SqlConnection(StrKonSql)
            Dim com As New SqlCommand
            Dim Tran As SqlTransaction = Nothing
            Dim SQL As String = "", IDSaldoAwal As Long = -1, IDSaldoAwalD As Long = -1, IDKartuStok As Long = -1
            Try
                If Not PB Is Nothing Then
                    PB.Visible = True
                    PB.Position = 0
                End If
                cn.Open()
                com.Connection = cn
                com.CommandTimeout = cn.ConnectionTimeout
                Tran = cn.BeginTransaction("Mutasi Data")
                com.Transaction = Tran

                com.CommandText = "EXEC sp_HapusPenjualanKasir '" & TglSampai.ToString("yyyy-MM-dd") & "'"
                com.CommandText = SQL

                If Not PB Is Nothing Then
                    PB.Visible = True
                    PB.Position = 100
                End If
                Application.DoEvents()
            Catch ex As Exception
                XtraMessageBox.Show("Info Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
                If Not Tran Is Nothing Then
                    Tran.Rollback()
                    Tran = Nothing
                End If
            Finally
                If Not Tran Is Nothing Then
                    Tran.Commit()
                    Tran = Nothing
                End If
                com.Dispose()
                If Not PB Is Nothing Then
                    PB.Visible = False
                End If
            End Try
        End Using
    End Sub
End Class