Imports System.Data.SqlClient
Imports System.Data.OleDb
Imports DevExpress.XtraEditors
Imports DevExpress.XtraGrid
Imports DevExpress.Utils
Imports DevExpress.XtraGrid.Views
Imports DevExpress.XtraGrid.Views.Base
Imports System.Drawing
'Imports Microsoft.VisualBasic

Module modSqlServer
    Public IsJurnalKasSatuLawanSatu As Boolean = True
    Public defAutoPosting As Boolean = True
    Public defIDAkunAntarKantor As Long = -1
    Public defIDMataUang As Integer = 1
    Public defNilaiTukarMataUang As Double = 1.0
    Public defNamaMataUang As String = "IDR"

    Public TypeBarcode As TypeBarcode_
    Public TypeKodeBarang As TypeKodeBarang_
    Public PanjangKodeBarang As Integer
    Public IDUserAktif As Long = -1
    Public NamaUserAktif As String
    Public KodeUserAktif As String
    Public DefIDDepartemen As Long = -1
    Public DefIDWilayah As Long = -1
    Public DefIDGudang As Long = -1

    Public DefIDGudangSupplier As Long = -1
    Public DefIDGudangCustomer As Long = -1

    Public DefIDSatuan As Long = -1
    Public DefIDPegawai As Long = -1
    Public DefIDCustomer As Long = -1
    Public DefIDSupplier As Long = -1
    Public DefTipeStock As DefTipeStock_
    Public DefIDKasBank As Long = -1
    Public DefNamaKasBank As String
    Public DefNamaGudang As String

    Public DefIDSatuanfrmBarang As Long = -1

    Public IsUpdateHargaBeli As Boolean = False

    Public IsSupervisor As Boolean = False
    Public IsAdministrator As Boolean = False
    Public IsEditLayout As Boolean = False
    Public IsKasir As Boolean = False
    Public IsAutoPosting As Boolean = False
    Public IsAccMutasi As Boolean = False
    'Public DSPublic As New DataSet
    'Public DSPublicKodeBarang As New DataSet

    Public Enum TypeBarcode_
        Ean13 = 0
        Ean8 = 1
        Codebar = 2
    End Enum

    Public Enum TypeKodeBarang_
        KodeMengikutiKategori = 0
        KategoriMengikutiKode = 1
        TidakBerhubungan = 2
    End Enum

    Public Enum DefTipeStock_
        Penuh = 0
        BongkarBarang = 1
        LihatStock = 2
    End Enum

    Public Function ExportExcelMutasiGudang(ByVal NoID As Long) As Boolean
        Dim ds As New DataSet
        Dim cn As New System.Data.OleDb.OleDbConnection
        Dim com As New System.Data.OleDb.OleDbCommand
        Dim xa As Boolean = False
        Dim ExcelFile As String = Application.StartupPath & "\system\engine\MutasiGudang.xls"
        Dim SQL As String = ""
        Try
            SQL = "SELECT MMutasiGudang.Kode, MMutasiGudang.Tanggal AS TglMutasi, MSatuan.Kode AS Satuan, MKategori.Kode + '-' + MKategori.Nama AS Kategori, MMutasiGudangD.*, MBarang.Kode AS KodeBarang, MBarang.Nama AS NamaBarang, MBarangD.Barcode, MBarangD.Varian" & vbCrLf & _
                  " FROM MMutasiGudang " & vbCrLf & _
                  " INNER JOIN MMutasiGudangD ON MMutasiGudang.NoID=MMutasiGudangD.IDMutasiGudang " & vbCrLf & _
                  " LEFT JOIN MBarang ON MBarang.NoID=MMutasiGudangD.IDBarang " & vbCrLf & _
                  " LEFT JOIN MBarangD ON MBarangD.NoID=MMutasiGudangD.IDBarangD " & vbCrLf & _
                  " LEFT JOIN MSatuan ON MSatuan.NoID=MMutasiGudangD.IDSatuan " & vbCrLf & _
                  " LEFT JOIN MKategori ON MKategori.NoID=MBarang.IDKategori " & vbCrLf & _
                  " WHERE MMutasiGudang.IsPosted=1 AND MMutasiGudang.NoID=" & NoID
            ds = ExecuteDataset("MMutasiGudang", SQL)
            If ds.Tables("MMutasiGudang").Rows.Count >= 1 Then
                System.IO.File.Copy(Application.StartupPath & "\system\engine\sysMutasiGudang.xls", ExcelFile, True)
                'Insert Sesuai Format P.Heru 25/06/2012, di Lombok :(
                cn.ConnectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" & ExcelFile & ";Extended Properties=Excel 8.0;"
                cn.Open()
                com.Connection = cn
                For i As Integer = 0 To ds.Tables("MMutasiGudang").Rows.Count - 1
                    With ds.Tables("MMutasiGudang").Rows(i)
                        SQL = "INSERT INTO [MMutasiGudang$] ([NoUrut], [NoReff], [Tanggal], [Barcode], [Kode], [Nama], [Kategori], [Varian/Ukuran], [Qty], [Satuan], [HargaJual], [Jumlah], [Keterangan], [IsLoad]) VALUES (" & vbCrLf & _
                              i + 1 & ", " & _
                              "'" & FixApostropi(.Item("Kode")) & "', " & _
                              "'" & NullToDate(.Item("TglMutasi")).ToString("dd-MMM-yyyy") & "', " & _
                              "'" & FixApostropi(.Item("Barcode")) & "', " & _
                              "'" & FixApostropi(.Item("KodeBarang")) & "', " & _
                              "'" & FixApostropi(.Item("NamaBarang")) & "', " & _
                              "'" & FixApostropi(.Item("Kategori")) & "', " & _
                              "'" & FixApostropi(.Item("Varian")) & "', " & _
                              FixKoma(.Item("Qty")) & ", " & _
                              "'" & FixApostropi(.Item("Satuan")) & "', " & _
                              FixKoma(.Item("HargaJual")) & ", " & _
                              FixKoma(.Item("Jumlah")) & ", " & _
                              "'" & FixApostropi(.Item("Keterangan")) & "', " & _
                              "0 )"
                        com.CommandText = SQL
                        com.ExecuteNonQuery()
                    End With
                    Application.DoEvents()
                Next
                xa = True
            End If
        Catch ex As Exception
            XtraMessageBox.Show("Info Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
            xa = False
        Finally
            If cn.State = ConnectionState.Open Then
                cn.Close()
            End If
            cn.Dispose()
            com.Dispose()
        End Try
        Return xa
    End Function
    Public Function ExportExcelMutasiGudangFormatPembelian(ByVal NoID As Long) As Boolean
        Dim ds As New DataSet
        Dim cn As New System.Data.OleDb.OleDbConnection
        Dim com As New System.Data.OleDb.OleDbCommand
        Dim xa As Boolean = False
        Dim ExcelFile As String = Application.StartupPath & "\system\engine\MutasiGudang.xls"
        Dim SQL As String = ""
        Try
            SQL = "SELECT MMutasiGudang.Kode, MMutasiGudang.Tanggal AS TglMutasi, MSatuan.Kode AS Satuan, MKategori.Nama AS Kategori, MMutasiGudangD.*, MBarang.Kode AS KodeBarang, MBarang.Nama AS NamaBarang, MBarangD.Barcode, MBarangD.Varian " & vbCrLf & _
                  " FROM MMutasiGudang " & vbCrLf & _
                  " INNER JOIN MMutasiGudangD ON MMutasiGudang.NoID=MMutasiGudangD.IDMutasiGudang " & vbCrLf & _
                  " LEFT JOIN MBarang ON MBarang.NoID=MMutasiGudangD.IDBarang " & vbCrLf & _
                  " LEFT JOIN MBarangD ON MBarangD.NoID=MMutasiGudangD.IDBarangD " & vbCrLf & _
                  " LEFT JOIN MSatuan ON MSatuan.NoID=MMutasiGudangD.IDSatuan " & vbCrLf & _
                  " LEFT JOIN MKategori ON MKategori.NoID=MBarang.IDKategori " & vbCrLf & _
                  " WHERE MMutasiGudang.IsPosted=1 AND MMutasiGudang.NoID=" & NoID
            ds = ExecuteDataset("MMutasiGudang", SQL)
            If ds.Tables("MMutasiGudang").Rows.Count >= 1 Then
                System.IO.File.Copy(Application.StartupPath & "\system\engine\sysPembelian.xls", ExcelFile, True)
                'P. Heru Minta disamakan dg format Pembelian
                cn.ConnectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" & ExcelFile & ";Extended Properties=Excel 8.0;"
                cn.Open()
                com.Connection = cn

                com.CommandText = "DELETE FROM [MMutasiGudang$]"
                com.ExecuteNonQuery()
                For i As Integer = 0 To ds.Tables("MMutasiGudang").Rows.Count - 1
                    With ds.Tables("MMutasiGudang").Rows(i)
                        SQL = "INSERT INTO [MMutasiGudang$] ([Kategori], [Nama], [Kode], [Ukuran], [HargaJual], [Jumlah], [HargaBeli]) VALUES (" & vbCrLf & _
                              "'" & FixApostropi(NullToStr(.Item("Kategori"))) & "', " & _
                              "'" & FixApostropi(NullToStr(.Item("Varian"))) & "', " & _
                              FixKoma(.Item("HargaJual") * .Item("Konversi")) & ", " & _
                              FixKoma(.Item("Jumlah") * .Item("Konversi")) & ", " & _
                              FixKoma(0) & ") "
                        com.CommandText = SQL
                        com.ExecuteNonQuery()
                    End With
                    Application.DoEvents()
                Next
                xa = True
            End If
        Catch ex As Exception
            XtraMessageBox.Show("Info Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
            xa = False
        Finally
            If cn.State = ConnectionState.Open Then
                cn.Close()
            End If
            cn.Dispose()
            com.Dispose()
        End Try
        Return xa
    End Function
    Public LookupBarangD As frLUBarangD
    'Public Function KirimDataKassa() As Boolean
    '    Dim cnOleDB As OleDbConnection = Nothing
    '    Dim cmOleDB As OleDbCommand = Nothing
    '    Dim SQL As String = ""
    '    Dim ds As New DataSet
    '    Dim dlg As DevExpress.Utils.WaitDialogForm = Nothing
    '    Dim FileSource As String = ""
    '    Dim FileDestination As String = ""
    '    Dim hasil As Boolean = False
    '    Try
    '        If IsSupervisor Then
    '            If XtraMessageBox.Show("Ingin mengirim data master ke Kassa ?" & vbCrLf & " Akan memerlukan sedikit waktu.", NamaAplikasi, MessageBoxButtons.YesNo, MessageBoxIcon.Information, MessageBoxDefaultButton.Button2) = Windows.Forms.DialogResult.Yes Then
    '                If System.IO.File.Exists(Application.StartupPath & "\System\engine\sysdbmaster.mdb") Then
    '                    Dim curentcursor As Cursor = Windows.Forms.Cursor.Current
    '                    Windows.Forms.Cursor.Current = Cursors.WaitCursor
    '                    dlg = New DevExpress.Utils.WaitDialogForm(String.Format("Sedang merefresh data.{0}MOHON TUNGGU ...", vbCrLf), Application.ProductName)
    '                    dlg.TopMost = False
    '                    dlg.Show()
    '                    Windows.Forms.Cursor.Current = Cursors.WaitCursor

    '                    'Provider=Microsoft.Jet.OLEDB.4.0;Data Source==D:\PROJECT\CITYTOYS\CitiToys\bin\Debug\System\engine\sysdbmaster.mdb
    '                    SQL = "SELECT MBarangD.*, MBarang.Kode AS KodeBarang, MBarang.Nama AS NamaBarang, MSatuan.Kode AS KodeSat, MBarang.HPP AS HargaPokok, " & vbCrLf & _
    '                    "MBarang.IsPPN,Mbarang.IsMember,MBarang.IsFamilyGroup,MBarang.QtyFamily,MBarang.HargaFamily,MBarang.TanggalDariFamily,MBarang.TanggalSampaiFamily " & vbCrLf & _
    '                          " FROM (MBarangD INNER JOIN MBarang ON MBarang.NoID=MBarangD.IDBarang) LEFT JOIN MSatuan ON MSatuan.NoID=MBarangD.IDSatuan" & vbCrLf & _
    '                          " WHERE IsNull(MBarang.Kode,'')<>'' AND MBarangD.IsActive=1 AND MBarang.IsActive=1 AND MBarangD.IsJualPOS=1"
    '                    ds = ExecuteDataset("MBarang", SQL)
    '                    With ds.Tables("MBarang")
    '                        If .Rows.Count >= 1 Then
    '                            cnOleDB = New OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" & Application.StartupPath & "\\System\\engine\\sysdbmaster.mdb")
    '                            cnOleDB.Open()
    '                            cmOleDB = New OleDbCommand("DELETE FROM TInv", cnOleDB)
    '                            cmOleDB.ExecuteNonQuery()
    '                            cmOleDB.Dispose()
    '                            'cnOleDB.Close()
    '                            'cnOleDB.Dispose()
    '                            For i As Integer = 0 To .Rows.Count - 1
    '                                SQL = "INSERT INTO TINV (NoID,IDInventor,Kode,Barcode,IsFamilyGroup,QtyFamily,HargaFamily,TanggalDariFamily,TanggalSampaiFamily,IsKelipatan,IsGrosir,QtyKelipatan,HargaKelipatan,Qty1,Harga1,Qty2,Harga2,Qty3,Harga3,IsAllowDisc,Nama,IDSatuan,KodeSat,Konversi,HargaJual,HargaPokok,HargaA,HargaB,HargaC,HargaD,HargaE,HargaF,HargaMinA,HargaMinB,HargaMinC,HargaMinD,HargaMinE,HargaMinF,DiscProsen1A,DiscProsen2A,DiscProsen1B,DiscProsen2B,DiscProsen1C,DiscProsen2C,DiscProsen1D,DiscProsen2D,DiscProsen1E,DiscProsen2E,DiscProsen1F,DiscProsen2F,DiscProsen,DiscRupiah,IsMember,IsOperator,IsOperator1,HargaMin) VALUES (" & vbCrLf & _
    '                                      NullToLong(.Rows(i).Item("NoID")) & "," & vbCrLf & _
    '                                      NullToLong(.Rows(i).Item("IDBarang")) & "," & vbCrLf & _
    '                                      "'" & FixApostropi(NullToStr(.Rows(i).Item("KodeBarang"))) & "'," & vbCrLf & _
    '                                      "'" & FixApostropi(NullToStr(.Rows(i).Item("Barcode"))) & "'," & vbCrLf & _
    '                                      IIf(NullToBool(.Rows(i).Item("IsFamilyGroup")), -1, 0) & "," & vbCrLf & _
    '                                      FixKoma(NullToDbl(.Rows(i).Item("Qty"))) & "," & vbCrLf & _
    '                                      FixKoma(NullToDbl(.Rows(i).Item("HargaFamily"))) & "," & vbCrLf & _
    '                                      NullToDateMDB(.Rows(i).Item("TanggalDariFamily")) & "," & vbCrLf & _
    '                                      NullToDateMDB(.Rows(i).Item("TanggalSampaiFamily")) & "," & vbCrLf & _
    '                                      IIf(NullToBool(.Rows(i).Item("IsKelipatan")), -1, 0) & "," & vbCrLf & _
    '                                      IIf(NullToBool(.Rows(i).Item("IsGrosir")), -1, 0) & "," & vbCrLf & _
    '                                      FixKoma(NullToDbl(.Rows(i).Item("QtyKelipatan"))) & "," & vbCrLf & _
    '                                      FixKoma(NullToDbl(.Rows(i).Item("HargaKelipatan"))) & "," & vbCrLf & _
    '                                      FixKoma(NullToDbl(.Rows(i).Item("Qty1"))) & "," & vbCrLf & _
    '                                      FixKoma(NullToDbl(.Rows(i).Item("Harga1"))) & "," & vbCrLf & _
    '                                      FixKoma(NullToDbl(.Rows(i).Item("Qty2"))) & "," & vbCrLf & _
    '                                      FixKoma(NullToDbl(.Rows(i).Item("Harga2"))) & "," & vbCrLf & _
    '                                      FixKoma(NullToDbl(.Rows(i).Item("Qty3"))) & "," & vbCrLf & _
    '                                      FixKoma(NullToDbl(.Rows(i).Item("Harga3"))) & "," & vbCrLf & _
    '                                      IIf(NullToBool(.Rows(i).Item("IsAllowDisc")), -1, 0) & "," & vbCrLf & _
    '                                      "'" & FixApostropi(NullToStr(.Rows(i).Item("NamaBarang"))) & "'," & vbCrLf & _
    '                                      NullToLong(.Rows(i).Item("IDSatuan")) & "," & vbCrLf & _
    '                                      "'" & FixApostropi(NullToStr(.Rows(i).Item("Satuan"))) & "'," & vbCrLf & _
    '                                      FixKoma(NullToDbl(.Rows(i).Item("Konversi"))) & "," & vbCrLf & _
    '                                      FixKoma(NullToDbl(.Rows(i).Item("HargaJualA"))) & "," & vbCrLf & _
    '                                      FixKoma(NullToDbl(.Rows(i).Item("HargaPokok"))) & "," & vbCrLf & _
    '                                      FixKoma(NullToDbl(.Rows(i).Item("HargaJualA"))) & "," & vbCrLf & _
    '                                      FixKoma(NullToDbl(.Rows(i).Item("HargaJualB"))) & "," & vbCrLf & _
    '                                      FixKoma(NullToDbl(.Rows(i).Item("HargaJualC"))) & "," & vbCrLf & _
    '                                      FixKoma(NullToDbl(.Rows(i).Item("HargaJualD"))) & "," & vbCrLf & _
    '                                      FixKoma(NullToDbl(.Rows(i).Item("HargaJualE"))) & "," & vbCrLf & _
    '                                      FixKoma(NullToDbl(.Rows(i).Item("HargaJualF"))) & "," & vbCrLf & _
    '                                      FixKoma(NullToDbl(.Rows(i).Item("HargaMinA"))) & "," & vbCrLf & _
    '                                      FixKoma(NullToDbl(.Rows(i).Item("HargaMinB"))) & "," & vbCrLf & _
    '                                      FixKoma(NullToDbl(.Rows(i).Item("HargaMinC"))) & "," & vbCrLf & _
    '                                      FixKoma(NullToDbl(.Rows(i).Item("HargaMinD"))) & "," & vbCrLf & _
    '                                      FixKoma(NullToDbl(.Rows(i).Item("HargaMinE"))) & "," & vbCrLf & _
    '                                      FixKoma(NullToDbl(.Rows(i).Item("HargaMinF"))) & "," & vbCrLf & _
    '                                      FixKoma(NullToDbl(.Rows(i).Item("DiscA1"))) & "," & vbCrLf & _
    '                                      FixKoma(NullToDbl(.Rows(i).Item("DiscA2"))) & "," & vbCrLf & _
    '                                      FixKoma(NullToDbl(.Rows(i).Item("DiscB1"))) & "," & vbCrLf & _
    '                                      FixKoma(NullToDbl(.Rows(i).Item("DiscB2"))) & "," & vbCrLf & _
    '                                      FixKoma(NullToDbl(.Rows(i).Item("DiscC1"))) & "," & vbCrLf & _
    '                                      FixKoma(NullToDbl(.Rows(i).Item("DiscC2"))) & "," & vbCrLf & _
    '                                      FixKoma(NullToDbl(.Rows(i).Item("DiscD1"))) & "," & vbCrLf & _
    '                                      FixKoma(NullToDbl(.Rows(i).Item("DiscD2"))) & "," & vbCrLf & _
    '                                      FixKoma(NullToDbl(.Rows(i).Item("DiscE1"))) & "," & vbCrLf & _
    '                                      FixKoma(NullToDbl(.Rows(i).Item("DiscE2"))) & "," & vbCrLf & _
    '                                      FixKoma(NullToDbl(.Rows(i).Item("DiscF1"))) & "," & vbCrLf & _
    '                                      FixKoma(NullToDbl(.Rows(i).Item("DiscF2"))) & "," & vbCrLf & _
    '                                      FixKoma(0) & "," & vbCrLf & _
    '                                      FixKoma(0) & "," & vbCrLf & _
    '                                      FixKoma(0) & "," & vbCrLf & _
    '                                      FixKoma(0) & "," & vbCrLf & _
    '                                      FixKoma(0) & "," & vbCrLf & _
    '                                      FixKoma(NullToDbl(.Rows(i).Item("HargaMinB"))) & ")"

    '                                cmOleDB = New OleDbCommand(SQL, cnOleDB)
    '                                cmOleDB.ExecuteNonQuery()
    '                                cmOleDB.Dispose()
    '                            Next
    '                        End If
    '                    End With
    '                    cnOleDB.Close()
    '                    cnOleDB.Dispose()

    '                    'Provider=Microsoft.Jet.OLEDB.4.0;Data Source==D:\PROJECT\CITYTOYS\CitiToys\bin\Debug\System\engine\sysdbmaster.mdb
    '                    SQL = "SELECT MUser.*, MAlamat.Kode AS KodeEmp, MAlamat.Nama AS NamaEmp, MAlamat.IsKasir AS IsKasirPeg, MAlamat.IsPengawas AS IsPengawasPeg, MAlamat.IsOperator AS IsOperatorPeg, MAlamat.IsOperator1 AS IsOperator1Peg, MAlamat.IsPengawasUtama AS IsPengawasUtamaPeg " & vbCrLf & _
    '                          " FROM MUser INNER JOIN MAlamat ON MAlamat.NoID=MUser.IDAlamat " & vbCrLf & _
    '                          " WHERE MAlamat.IsActive=1 AND MAlamat.IsPegawai=1 "
    '                    ds = ExecuteDataset("MUser", SQL)
    '                    With ds.Tables("MUser")
    '                        If .Rows.Count >= 1 Then
    '                            cnOleDB = New OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" & Application.StartupPath & "\\System\\engine\\sysdbmaster.mdb")
    '                            cnOleDB.Open()

    '                            cmOleDB = New OleDbCommand("DELETE FROM MEmp", cnOleDB)
    '                            cmOleDB.ExecuteNonQuery()
    '                            cmOleDB.Dispose()
    '                            'cnOleDB.Close()
    '                            'cnOleDB.Dispose()
    '                            For i As Integer = 0 To .Rows.Count - 1
    '                                SQL = "INSERT INTO MEMP ([NoID],[Kode],[Nama],[Nick],[Password],[IsKasir],[IsPengawas],[IsOperator],[IsOperator1],[IsPengawasUtama]) VALUES (" & vbCrLf & _
    '                                      NullToLong(.Rows(i).Item("NoID")) & "," & vbCrLf & _
    '                                      "'" & FixApostropi(NullToStr(.Rows(i).Item("Kode"))) & "'," & vbCrLf & _
    '                                      "'" & FixApostropi(NullToStr(.Rows(i).Item("NamaEmp"))) & "'," & vbCrLf & _
    '                                      "'" & FixApostropi(NullToStr(.Rows(i).Item("Nama"))) & "'," & vbCrLf & _
    '                                      "'" & FixApostropi(DecryptText(NullToStr(.Rows(i).Item("Pwd")), "vpoint")) & "'," & vbCrLf & _
    '                                      NullTolInt(.Rows(i).Item("IsKasirPeg")) & "," & vbCrLf & _
    '                                      NullTolInt(.Rows(i).Item("IsPengawasPeg")) & "," & vbCrLf & _
    '                                      NullTolInt(.Rows(i).Item("IsOperatorPeg")) & "," & vbCrLf & _
    '                                      NullTolInt(.Rows(i).Item("IsOperator1Peg")) & "," & vbCrLf & _
    '                                      NullTolInt(.Rows(i).Item("IsPengawasUtamaPeg")) & ")"

    '                                cmOleDB = New OleDbCommand(SQL, cnOleDB)
    '                                cmOleDB.ExecuteNonQuery()
    '                                cmOleDB.Dispose()
    '                            Next
    '                        End If
    '                    End With
    '                    cnOleDB.Close()
    '                    cnOleDB.Dispose()

    '                    'Provider=Microsoft.Jet.OLEDB.4.0;Data Source==D:\PROJECT\CITYTOYS\CitiToys\bin\Debug\System\engine\sysdbmaster.mdb
    '                    SQL = "SELECT MAlamat.* FROM MAlamat" & vbCrLf & _
    '                          " WHERE MAlamat.IsActive = 1 And MAlamat.IsCustomer = 1"
    '                    ds = ExecuteDataset("MCust", SQL)
    '                    With ds.Tables("MCust")
    '                        If .Rows.Count >= 1 Then
    '                            cnOleDB = New OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" & Application.StartupPath & "\\System\\engine\\sysdbmaster.mdb")
    '                            cnOleDB.Open()

    '                            cmOleDB = New OleDbCommand("DELETE FROM MCustomer", cnOleDB)
    '                            cmOleDB.ExecuteNonQuery()
    '                            cmOleDB.Dispose()
    '                            'cnOleDB.Close()
    '                            'cnOleDB.Dispose()
    '                            For i As Integer = 0 To .Rows.Count - 1
    '                                SQL = "INSERT INTO MCustomer ([NoID],[Kode],[Barcode],[Nama],[DOJ],[LimitHutang],[TipeHargaJual]) VALUES (" & vbCrLf & _
    '                                      NullToLong(.Rows(i).Item("NoID")) & "," & vbCrLf & _
    '                                      "'" & FixApostropi(NullToStr(.Rows(i).Item("Kode"))) & "'," & vbCrLf & _
    '                                      "'" & FixApostropi(NullToLong(.Rows(i).Item("NoID"))) & "'," & vbCrLf & _
    '                                      "'" & FixApostropi(NullToStr(.Rows(i).Item("Nama"))) & "'," & vbCrLf & _
    '                                      "#" & NullToDate(.Rows(i).Item("DOJ")).ToString("yyyy/MM/dd") & "#," & vbCrLf & _
    '                                      NullToDbl(.Rows(i).Item("LimitPiutang")) & "," & NullToLong(.Rows(i).Item("DefaultTipeHarga")) & ")"

    '                                cmOleDB = New OleDbCommand(SQL, cnOleDB)
    '                                cmOleDB.ExecuteNonQuery()
    '                                cmOleDB.Dispose()
    '                            Next
    '                        End If
    '                    End With
    '                    cnOleDB.Close()
    '                    cnOleDB.Dispose()

    '                    'Copy Database ke Kassa
    '                    SQL = "SELECT MPOS.* FROM MPOS WHERE MPOS.IsActive=1"
    '                    ds = ExecuteDataset("MPOS", SQL)
    '                    For i As Integer = 0 To ds.Tables("MPOS").Rows.Count - 1
    '                        If System.IO.Directory.Exists(NullToStr(ds.Tables("MPOS").Rows(i).Item("PathDbTemp"))) Then
    '                            Try
    '                                FileSource = Application.StartupPath & "\System\engine\sysdbmaster.mdb"
    '                                FileDestination = NullToStr(ds.Tables("MPOS").Rows(i).Item("PathDbTemp")) & "\Update\DBMaster.mdb"
    '                                If System.IO.File.Exists(FileDestination) Then
    '                                    System.IO.File.Delete(FileDestination)
    '                                End If
    '                                System.IO.File.Copy(FileSource, FileDestination)
    '                                Try
    '                                    FileDestination = NullToStr(ds.Tables("MPOS").Rows(i).Item("PathDbTemp")) & "\Database\DBMaster.mdb"
    '                                    System.IO.File.Copy(FileSource, FileDestination, True)
    '                                Catch ex As Exception

    '                                End Try
    '                                EksekusiSQL("Update MPOS SET DatabaseUpdate=getdate() WHERE NoID=" & NullToLong(ds.Tables("MPOS").Rows(i).Item("NoID")))
    '                            Catch ex As Exception
    '                                XtraMessageBox.Show("Info : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK)
    '                            End Try
    '                        End If
    '                    Next
    '                    hasil = True
    '                Else
    '                    XtraMessageBox.Show("File system tidak ditemukan.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Warning)
    '                    hasil = False
    '                End If
    '            End If
    '        End If
    '    Catch ex As Exception
    '        XtraMessageBox.Show("Gagal mengeksekusi SQL:" & SQL & vbCrLf & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
    '        hasil = False
    '    Finally
    '        Windows.Forms.Cursor.Current = Cursors.Default
    '        If Not cnOleDB Is Nothing Then
    '            cmOleDB.Dispose()
    '            cnOleDB.Close()
    '            cnOleDB.Dispose()
    '        End If
    '        If Not dlg Is Nothing Then
    '            dlg.Close()
    '            dlg.Dispose()
    '        End If
    '    End Try
    '    Return hasil
    'End Function
    Private Sub TambahMenuMei2013(ByRef strSQL As String, ByVal NoID As Long, ByVal IDMenuInternal As Long)
        Try
            'INSERT dbo.MMenu(noid, kode, idparent, caption, shortcut, KeyShortcut, icon, nourut, isactive, isawalgroup, objectrun, IDServerAsal, IsBarSubItem, IDBarSubItem) VALUES (116, N'LaporanSaldoStockPerVarian', 9, N'Laporan Saldo Stock Per Varian', NULL, NULL, 0, 2, 1, 0, N'LaporanSaldoStockPerVarian', NULL, 0, NULL)
            'INSERT dbo.MMenu(noid, kode, idparent, caption, shortcut, KeyShortcut, icon, nourut, isactive, isawalgroup, objectrun, IDServerAsal, IsBarSubItem, IDBarSubItem) VALUES (117, N'Kartu Stok Per Varian', 9, N'Laporan Kartu Stok Per Varian', NULL, NULL, 0, 1, 1, 0, N'DaftarKartuStokVarianPerGudang', NULL, 0, NULL)
            'INSERT dbo.MMenu(noid, kode, idparent, caption, shortcut, KeyShortcut, icon, nourut, isactive, isawalgroup, objectrun, IDServerAsal, IsBarSubItem, IDBarSubItem) VALUES (118, N'mnLaporanDiInternal', 9, N'Laporan-Laporan Internal', NULL, NULL, 0, 4, 1, 0, N'', NULL, 1, NULL)
            'INSERT dbo.MMenu(noid, kode, idparent, caption, shortcut, KeyShortcut, icon, nourut, isactive, isawalgroup, objectrun, IDServerAsal, IsBarSubItem, IDBarSubItem) VALUES (119, N'mnLaporanBarangMasuk', 9, N'Laporan Stock Barang Masuk', NULL, NULL, 0, 1, 1, 0, N'LaporanStockBarangMasuk', NULL, 0, 118)
            'INSERT dbo.MMenu(noid, kode, idparent, caption, shortcut, KeyShortcut, icon, nourut, isactive, isawalgroup, objectrun, IDServerAsal, IsBarSubItem, IDBarSubItem) VALUES (120, N'mnLaporanBarangKeluar', 9, N'Laporan Stock Barang Keluar', NULL, NULL, 0, 2, 1, 0, N'LaporanStockBarangKeluar', NULL, 0, 118)

            'ACC
            If NullToLong(EksekusiSQlSkalarNew("Select NoID from MMenu where UPPER(Kode)=UPPER('mnLaporanAccounting')")) > 0 Then
            Else
                NoID = GetNewID("MMenu")
                strSQL = "INSERT INTO [MMenu] ([noid],[kode],[idparent],[caption],[shortcut],[KeyShortcut],[icon],[nourut],[isactive],[isawalgroup],[objectrun],[IDServerAsal],[IsBarSubItem],[IDBarSubItem]) Values(" & _
                NoID & ",'mnLaporanAccounting', 9, 'Laporan-Laporan Accounting', NULL, NULL, 0, 3, 1, 1, NULL, 1, 1, NULL)"
                EksekusiSQL(strSQL)
            End If
            Dim IDAcc As Long = -1
            IDAcc = NullToLong(EksekusiSQlSkalarNew("SELECT NOID FROM MMenu WHERE UPPER(Kode)=UPPER('mnLaporanAccounting')"))
            If IDAcc >= 1 AndAlso NullToLong(EksekusiSQlSkalarNew("Select NoID from MMenu where UPPER(Kode)=UPPER('mnLaporanBukuBesar')")) > 0 Then
            Else
                NoID = GetNewID("MMenu")
                strSQL = "INSERT INTO [MMenu] ([noid],[kode],[idparent],[caption],[shortcut],[KeyShortcut],[icon],[nourut],[isactive],[isawalgroup],[objectrun],[IDServerAsal],[IsBarSubItem],[IDBarSubItem]) Values(" & _
                NoID & ", 'mnLaporanBukuBesar', 9, 'Laporan Buku Besar', NULL, NULL, 0, 1, 1, 0, 'LaporanBukuBesar', NULL, 0, " & IDAcc & ")"
                EksekusiSQL(strSQL)
            End If

            If IDAcc >= 1 AndAlso NullToLong(EksekusiSQlSkalarNew("Select NoID from MMenu where UPPER(Kode)=UPPER('mnLaporanLabaRugi')")) > 0 Then
            Else
                NoID = GetNewID("MMenu")
                strSQL = "INSERT INTO [MMenu] ([noid],[kode],[idparent],[caption],[shortcut],[KeyShortcut],[icon],[nourut],[isactive],[isawalgroup],[objectrun],[IDServerAsal],[IsBarSubItem],[IDBarSubItem]) Values(" & _
                NoID & ", 'mnLaporanLabaRugi', 9, 'Laporan Laba Rugi', NULL, NULL, 0, 2, 1, 0, 'LaporanLabaRugi', NULL, 0, " & FixKoma(IDAcc) & ")"
                EksekusiSQL(strSQL)
            End If

            If NullToLong(EksekusiSQlSkalarNew("Select NoID from MMenu where objectrun='LaporanCrew'")) > 0 Then
            Else
                NoID = GetNewID("MMenu")
                strSQL = "insert into MMenu(NoID,Kode,IDParent,Caption,icon,NoUrut,IsActive,IsAwalGroup,objectrun) Values(" & _
                NoID & ",'mnLaporanCrew',9,'Laporan Penjualan Crew',0, 2, 1, 0,'LaporanCrew')"
                EksekusiSQL(strSQL)
            End If

            If NullToLong(EksekusiSQlSkalarNew("Select NoID from MMenu where UPPER(Kode)=Upper('mnTipeCustomer')")) > 0 Then
            Else
                Try
                    NoID = GetNewID("MMenu")
                    IDMenuInternal = NoID
                    strSQL = "INSERT dbo.MMenu(noid, kode, idparent, caption, shortcut, KeyShortcut, icon, nourut, isactive, isawalgroup, objectrun, IDServerAsal, IsBarSubItem, IDBarSubItem) VALUES (" & NoID & ",'mnTipeCustomer', 1, 'Master Tipe &Member', NULL, NULL, 0, 18, 1, 1, 'DaftarTypeCustomer', NULL, 0, NULL)"
                    EksekusiSQL(strSQL)
                Catch ex As Exception

                End Try
            End If
            If NullToLong(EksekusiSQlSkalarNew("Select NoID from MMenu where UPPER(Kode)=Upper('mnPembelianTanpaBarang')")) > 0 Then
            Else
                NoID = GetNewID("MMenu")
                IDMenuInternal = NoID
                strSQL = "INSERT dbo.MMenu(noid, kode, idparent, caption, shortcut, KeyShortcut, icon, nourut, isactive, isawalgroup, objectrun, IDServerAsal, IsBarSubItem, IDBarSubItem) VALUES (" & NoID & ",'mnPembelianTanpaBarang',3, 'Pembelian (Tanpa Item Barang)', NULL, NULL, 0, 5, 1, 1, 'PembelianTanpaBarang', NULL, 0, NULL)"
                EksekusiSQL(strSQL)
            End If

            If NullToLong(EksekusiSQlSkalarNew("Select NoID from MMenu where UPPER(Kode)=Upper('mnReturPembelianTanpaBarang')")) > 0 Then
            Else
                NoID = GetNewID("MMenu")
                IDMenuInternal = NoID
                strSQL = "INSERT dbo.MMenu(noid, kode, idparent, caption, shortcut, KeyShortcut, icon, nourut, isactive, isawalgroup, objectrun, IDServerAsal, IsBarSubItem, IDBarSubItem) VALUES (" & NoID & ",'mnReturPembelianTanpaBarang',3, 'Retur Pembelian (Tanpa Item Barang)', NULL, NULL, 0, 7, 1, 1, 'ReturPembelianTanpaBarang', NULL, 0, NULL)"
                EksekusiSQL(strSQL)
            End If

            If NullToLong(EksekusiSQlSkalarNew("Select NoID from MMenu where UPPER(Kode)=Upper('mnLaporanJualRekapPerDepartemenTH')")) > 0 Then
            Else
                NoID = GetNewID("MMenu")
                IDMenuInternal = NoID
                strSQL = "INSERT dbo.MMenu(noid, kode, idparent, caption, shortcut, KeyShortcut, icon, nourut, isactive, isawalgroup, objectrun, IDServerAsal, IsBarSubItem, IDBarSubItem) VALUES (" & NoID & ",'mnLaporanJualRekapPerDepartemenTH',9, 'Rekap Penjualan per Departemen Tahunan', NULL, NULL, 0, 7, 8, 1, 'RekapPenjualanPerDepartemenTahunan', NULL, 0, NULL)"
                EksekusiSQL(strSQL)
            End If

            If NullToLong(EksekusiSQlSkalarNew("Select NoID from MMenu where UPPER(Kode)=Upper('mnLaporanPembelianPerDepartemenBulanan')")) > 0 Then
            Else
                NoID = GetNewID("MMenu")
                IDMenuInternal = NoID
                strSQL = "INSERT dbo.MMenu(noid, kode, idparent, caption, shortcut, KeyShortcut, icon, nourut, isactive, isawalgroup, objectrun, IDServerAsal, IsBarSubItem, IDBarSubItem) VALUES (" & NoID & ",'mnLaporanPembelianPerDepartemenBulanan', 9, 'Laporan Pembelian Per Departemen Bulanan', NULL, NULL, 0, 7, 8, 1, 'LaporanPembelianPerDepartemenBulanan', NULL, 0, NULL)"
                EksekusiSQL(strSQL)
            End If

            If NullToLong(EksekusiSQlSkalarNew("Select NoID from MMenu where UPPER(Kode)=Upper('mnLaporanOmzetAccBonus')")) > 0 Then
            Else
                NoID = GetNewID("MMenu")
                IDMenuInternal = NoID
                strSQL = "INSERT dbo.MMenu(noid, kode, idparent, caption, shortcut, KeyShortcut, icon, nourut, isactive, isawalgroup, objectrun, IDServerAsal, IsBarSubItem, IDBarSubItem) VALUES (" & NoID & ",'mnLaporanOmzetAccBonus', 9, 'Laporan Omzet Bulanan Accounting', NULL, NULL, 0, 7, 8, 1, 'LaporanOmzetAccBonus', NULL, 0, NULL)"
                EksekusiSQL(strSQL)
            End If

            If NullToLong(EksekusiSQlSkalarNew("Select NoID from MMenu where UPPER(Kode)=Upper('mnLaporanPembelianPerSupplierBulanan')")) > 0 Then
            Else
                NoID = GetNewID("MMenu")
                IDMenuInternal = NoID
                strSQL = "INSERT dbo.MMenu(noid, kode, idparent, caption, shortcut, KeyShortcut, icon, nourut, isactive, isawalgroup, objectrun, IDServerAsal, IsBarSubItem, IDBarSubItem) VALUES (" & NoID & ",'mnLaporanPembelianPerSupplierBulanan', 9, 'Laporan Pembelian Per Supplier Bulanan', NULL, NULL, 0, 7, 8, 1, 'LaporanPembelianPerSupplierBulanan', NULL, 0, NULL)"
                EksekusiSQL(strSQL)
            End If

            If NullToLong(EksekusiSQlSkalarNew("Select NoID from MMenu where UPPER(Kode)=Upper('mnmasterkartukredit')")) > 0 Then
            Else
                Try
                    NoID = GetNewID("MMenu")
                    IDMenuInternal = NoID
                    strSQL = "INSERT dbo.MMenu(noid, kode, idparent, caption, shortcut, KeyShortcut, icon, nourut, isactive, isawalgroup, objectrun, IDServerAsal, IsBarSubItem, IDBarSubItem) VALUES (" & NoID & ",'mnmasterkartukredit', 1, 'Master Kartu &Kredit', NULL, NULL, 0, 18, 1, 1, 'DaftarKartuKredit', NULL, 0, NULL)"
                    EksekusiSQL(strSQL)
                Catch ex As Exception

                End Try
            End If

            If NullToLong(EksekusiSQlSkalarNew("Select NoID from MMenu where UPPER(Kode)=Upper('mnmastergroupkategori')")) > 0 Then
            Else
                Try
                    NoID = GetNewID("MMenu")
                    IDMenuInternal = NoID
                    strSQL = "INSERT dbo.MMenu(noid, kode, idparent, caption, shortcut, KeyShortcut, icon, nourut, isactive, isawalgroup, objectrun, IDServerAsal, IsBarSubItem, IDBarSubItem) VALUES (" & NoID & ",'mnmastergroupkategori', 1, 'Master Group Supplier', NULL, NULL, 0, 18, 1, 1, 'DaftarGroupSupplier', NULL, 0, NULL)"
                    EksekusiSQL(strSQL)
                Catch ex As Exception

                End Try
            End If

            If NullToLong(EksekusiSQlSkalarNew("Select NoID from MMenu where UPPER(Kode)=Upper('mnDaftarRevisiTandaTerima')")) > 0 Then
            Else
                NoID = GetNewID("MMenu")
                IDMenuInternal = NoID
                strSQL = "INSERT dbo.MMenu(noid, kode, idparent, caption, shortcut, KeyShortcut, icon, nourut, isactive, isawalgroup, objectrun, IDServerAsal, IsBarSubItem, IDBarSubItem) VALUES (" & NoID & ", N'mnDaftarRevisiTandaTerima', 3, N'Daftar Revisi Tanda Terima Supplier', NULL, NULL, 0, 4, 1, 0, N'', NULL, 0, NULL)"
                EksekusiSQL(strSQL)
            End If
            If NullToLong(EksekusiSQlSkalarNew("Select NoID from MMenu where UPPER(Kode)=Upper('mnLaporanDiInternal')")) > 0 Then
            Else
                NoID = GetNewID("MMenu")
                IDMenuInternal = NoID
                strSQL = "INSERT dbo.MMenu(noid, kode, idparent, caption, shortcut, KeyShortcut, icon, nourut, isactive, isawalgroup, objectrun, IDServerAsal, IsBarSubItem, IDBarSubItem) VALUES (" & NoID & ", N'mnLaporanDiInternal', 9, N'Laporan-Laporan Internal', NULL, NULL, 0, 4, 1, 0, N'', NULL, 1, NULL)"
                EksekusiSQL(strSQL)
            End If
            If NullToLong(EksekusiSQlSkalarNew("Select NoID from MMenu where objectrun='LaporanStockBarangKeluar'")) > 0 Then
            Else
                NoID = GetNewID("MMenu")
                strSQL = "INSERT dbo.MMenu(noid, kode, idparent, caption, shortcut, KeyShortcut, icon, nourut, isactive, isawalgroup, objectrun, IDServerAsal, IsBarSubItem, IDBarSubItem) VALUES (" & NoID & ", N'mnLaporanBarangKeluar', 9, N'Laporan Stock Barang Keluar', NULL, NULL, 0, 2, 1, 0, N'LaporanStockBarangKeluar', NULL, 0, " & IDMenuInternal & ")"
                EksekusiSQL(strSQL)
            End If
            If NullToLong(EksekusiSQlSkalarNew("Select NoID from MMenu where objectrun='LaporanStockBarangMasuk'")) > 0 Then
            Else
                NoID = GetNewID("MMenu")
                strSQL = "INSERT dbo.MMenu(noid, kode, idparent, caption, shortcut, KeyShortcut, icon, nourut, isactive, isawalgroup, objectrun, IDServerAsal, IsBarSubItem, IDBarSubItem) VALUES (" & NoID & ", N'mnLaporanBarangMasuk', 9, N'Laporan Stock Barang Masuk', NULL, NULL, 0, 1, 1, 0, N'LaporanStockBarangMasuk', NULL, 0, " & IDMenuInternal & ")"
                EksekusiSQL(strSQL)
            End If
            If NullToLong(EksekusiSQlSkalarNew("Select NoID from MMenu where objectrun='LaporanSaldoStockPerVarian'")) > 0 Then
            Else
                NoID = GetNewID("MMenu")
                strSQL = "insert into MMenu(NoID,Kode,IDParent,Caption,icon,NoUrut,IsActive,IsAwalGroup,objectrun) Values(" & _
                NoID & ",'mnLaporanSaldoStockPerVarian',9,'Laporan Saldo Stock Per Varian',0, 2, 1, 0,'LaporanSaldoStockPerVarian')"
                EksekusiSQL(strSQL)
            End If
            If NullToLong(EksekusiSQlSkalarNew("Select NoID from MMenu where objectrun='DaftarPajakKeluaran'")) > 0 Then
            Else
                NoID = GetNewID("MMenu")
                strSQL = "INSERT INTO [MMenu] ([noid],[kode],[idparent],[caption],[shortcut],[KeyShortcut],[icon],[nourut],[isactive],[isawalgroup],[objectrun],[IDServerAsal],[IsBarSubItem],[IDBarSubItem]) Values(" & _
                NoID & ", 'mnDaftarPajakKeluaran', 4, 'Daftar Pajak Keluaran', NULL, NULL, 0, " & NullToLong(GetNewID("MMenu", "NoUrut", " WHERE MMenu.IDParent=4")) + 1 & ", 1, 1, 'DaftarPajakKeluaran', NULL, 0, NULL)"
                EksekusiSQL(strSQL)
            End If
            If NullToLong(EksekusiSQlSkalarNew("Select NoID from MMenu where objectrun='DaftarKartuStokVarianPerGudang'")) > 0 Then
            Else
                NoID = GetNewID("MMenu")
                strSQL = "insert into MMenu(NoID,Kode,IDParent,Caption,icon,NoUrut,IsActive,IsAwalGroup,objectrun) Values(" & _
                NoID & ",'mnLaporanKartuStokPerVarian',9,'Laporan Kartu Stok Per Varian',0, 1, 1, 0,'DaftarKartuStokVarianPerGudang')"
                EksekusiSQL(strSQL)
            End If

            If NullToLong(EksekusiSQlSkalarNew("Select NoID from MMenu where objectrun='LaporanStockOpnameD'")) > 0 Then
            Else
                NoID = GetNewID("MMenu")
                strSQL = "insert into MMenu(NoID,Kode,IDParent,Caption,icon,NoUrut,IsActive,IsAwalGroup,objectrun) Values(" & _
                NoID & ",'mnLaporanStockOpnameD',9,'Laporan Stock Opname',0, 10, 1, 0,'LaporanStockOpnameD')"
                EksekusiSQL(strSQL)
            End If
        Catch ex As Exception

        End Try
    End Sub

    Public Sub TambahMenu()
        Dim strSQL As String = ""
        Dim noid As Long = -1
        Dim IDMenuInternal As Long = -1
        Try
            If NullToLong(EksekusiSQlSkalarNew("Select NoID from MMenu where UPPER(Kode)=Upper('mnDaftarReedem')")) > 0 Then
            Else
                noid = GetNewID("MMenu")
                strSQL = "INSERT dbo.MMenu(noid, kode, idparent, caption, shortcut, KeyShortcut, icon, nourut, isactive, isawalgroup, objectrun, IDServerAsal, IsBarSubItem, IDBarSubItem) VALUES (" & noid & ", N'mnDaftarReedem', 1, N'Setting Reedem Poin Member', NULL, NULL, 0, 1, 1, 0, N'DaftarMReedem', NULL, 0, NULL)"
                EksekusiSQL(strSQL)
            End If
            'TambahMenuMei2013(strSQL, noid, IDMenuInternal)
        Catch ex As Exception

        End Try

    End Sub

    Private Sub TambahKolomMei2013(ByRef cn As SqlConnection, ByRef com As SqlCommand, ByRef SQL As String)
        Try
            SQL = "CREATE TABLE [dbo].[MLockFP](" & vbCrLf & _
                  "[NoID] [int] NOT NULL," & vbCrLf & _
                  "[Periode] [datetime] NULL," & vbCrLf & _
                  "[IDUserEntry] [int] NULL," & vbCrLf & _
                  "[IsLock] [bit] NULL," & vbCrLf & _
                  "[IDUserLock] [int] NULL," & vbCrLf & _
                  "[TglLock] [datetime] NULL" & vbCrLf & _
                  "CONSTRAINT [PK_MLockFP] PRIMARY KEY CLUSTERED " & vbCrLf & _
                  "([NoID] ASC" & vbCrLf & _
                  ")WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]" & vbCrLf & _
                  ") ON [PRIMARY]"
            com.CommandText = SQL
            com.ExecuteNonQuery()
        Catch ex As Exception

        End Try
        Try
            SQL = "ALTER TABLE MSetting ADD " & vbCrLf & _
                  " FormatFP Varchar(20) NULL"
            com.CommandText = SQL
            com.ExecuteNonQuery()
        Catch ex As Exception

        End Try
        Try
            SQL = "ALTER TABLE MKategori ADD " & vbCrLf & _
                  " DefProvitMargin Numeric(18,0) NULL"
            com.CommandText = SQL
            com.ExecuteNonQuery()
        Catch ex As Exception

        End Try
        Try
            SQL = "ALTER TABLE MBarang ADD " & vbCrLf & _
                  " IDSatuanObat INT NULL, " & vbCrLf & _
                  " KonversiObat Numeric(18,0) NULL "
            com.CommandText = SQL
            com.ExecuteNonQuery()
        Catch ex As Exception

        End Try
        Try
            SQL = "ALTER TABLE MLapOmzet ADD " & vbCrLf & _
                  " DPPPromosi Money NULL, " & vbCrLf & _
                  " PPNPromosi Money NULL, " & vbCrLf & _
                  " DPPObat Money NULL, " & vbCrLf & _
                  " PPNObat Money NULL "
            com.CommandText = SQL
            com.ExecuteNonQuery()
        Catch ex As Exception

        End Try
        Try
            SQL = "ALTER TABLE MJualD ADD " & vbCrLf & _
                  " IsDisc2 Bit NULL "
            com.CommandText = SQL
            com.ExecuteNonQuery()
        Catch ex As Exception

        End Try
        Try
            SQL = "ALTER TABLE MBarang ADD " & vbCrLf & _
                  " HargaPDP Money NULL "
            com.CommandText = SQL
            com.ExecuteNonQuery()
        Catch ex As Exception

        End Try
        Try
            SQL = "ALTER TABLE MBarang ADD " & vbCrLf & _
                  " DiscPDP Money NULL "
            com.CommandText = SQL
            com.ExecuteNonQuery()
        Catch ex As Exception

        End Try
        Try
            SQL = "ALTER TABLE MJualD ADD " & vbCrLf & _
                  " HargaNormal Money NULL "
            com.CommandText = SQL
            com.ExecuteNonQuery()
        Catch ex As Exception

        End Try
        Try
            SQL = "ALTER TABLE MJualD ADD " & vbCrLf & _
                  " IsPDP BIT NULL "
            com.CommandText = SQL
            com.ExecuteNonQuery()
        Catch ex As Exception

        End Try
        Try
            SQL = "ALTER TABLE MSETTINGPOIN ADD " & vbCrLf & _
                  " MinimumBelanjaDapatPDP Money NULL "
            com.CommandText = SQL
            com.ExecuteNonQuery()
        Catch ex As Exception

        End Try
        Try
            SQL = "ALTER TABLE MSETTINGPOIN ADD " & vbCrLf & _
                  " MinimumBelanjaDapatDiskon2 Money NULL "
            com.CommandText = SQL
            com.ExecuteNonQuery()
        Catch ex As Exception

        End Try
        Try
            SQL = "ALTER TABLE MBarang ADD " & vbCrLf & _
                  " QtyPDP numeric(18, 2) NULL "
            com.CommandText = SQL
            com.ExecuteNonQuery()
        Catch ex As Exception

        End Try
        Try
            SQL = "ALTER TABLE MLapOmzet ADD " & vbCrLf & _
                  " RumusSetoran Varchar(50) NULL "
            com.CommandText = SQL
            com.ExecuteNonQuery()
        Catch ex As Exception

        End Try
        Try
            SQL = "ALTER TABLE MReturBeli ADD " & vbCrLf & _
                  " TglFakturPajak Datetime NULL "
            com.CommandText = SQL
            com.ExecuteNonQuery()

            SQL = "UPDATE MReturBeli SET " & vbCrLf & _
                  " TglFakturPajak=Tanggal WHERE TglFakturPajak IS NULL"
            com.CommandText = SQL
            com.ExecuteNonQuery()
        Catch ex As Exception

        End Try
        Try
            SQL = "ALTER TABLE MBarang ADD " & vbCrLf & _
                  " TglDariDiskon2 Datetime NULL, " & vbCrLf & _
                  " TglSampaiDiskon2 Datetime NULL, " & vbCrLf & _
                  " DiscMemberProsen2 numeric(18, 2) NULL, " & vbCrLf & _
                  " DiscMemberRp2 Money NULL "
            com.CommandText = SQL
            com.ExecuteNonQuery()
        Catch ex As Exception

        End Try
        Try
            SQL = "ALTER TABLE MBarang ADD " & vbCrLf & _
                  " HargaNettoMember Money NULL "
            com.CommandText = SQL
            com.ExecuteNonQuery()
        Catch ex As Exception

        End Try
        'Try
        '    SQL = "ALTER TABLE MBarangD ADD " & vbCrLf & _
        '          " TglDariDiskon2 Datetime NULL, " & vbCrLf & _
        '          " TglSampaiDiskon2 Datetime NULL, " & vbCrLf & _
        '          " DiscMemberProsen2 numeric(18, 2) NULL, " & vbCrLf & _
        '          " DiscMemberRp2 Money NULL "
        '    com.CommandText = SQL
        '    com.ExecuteNonQuery()
        'Catch ex As Exception

        'End Try
        Try
            SQL = "ALTER TABLE MBeli ADD " & vbCrLf & _
                  " IDUserEditFP INT NULL, " & vbCrLf & _
                  " TglEditFP DateTime NULL"
            com.CommandText = SQL
            com.ExecuteNonQuery()
        Catch ex As Exception

        End Try
        Try
            SQL = "CREATE TABLE dbo.MLogExportCSV" & vbCrLf & _
                  " (NoID bigint NOT NULL IDENTITY (1, 1)," & vbCrLf & _
                  " IDUser int NULL," & vbCrLf & _
                  " Tanggal datetime NULL," & vbCrLf & _
                  " Filename varchar(255) NULL," & vbCrLf & _
                  " Pembetulan numeric(18, 0) NULL" & vbCrLf & _
                  " )  ON [PRIMARY]"
            com.CommandText = SQL
            com.ExecuteNonQuery()
            SQL = " ALTER TABLE dbo.MLogExportCSV ADD CONSTRAINT" & vbCrLf & _
                  " PK_MLogExportCSV PRIMARY KEY CLUSTERED " & vbCrLf & _
                  " (" & vbCrLf & _
                  " NoID " & vbCrLf & _
                  " ) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]"
            com.CommandText = SQL
            com.ExecuteNonQuery()
        Catch ex As Exception

        End Try
        Try
            SQL = " CREATE TABLE [dbo].[MLapProfitMarginPembelianBulanan](" & vbCrLf & _
                " [NoID] [int] IDENTITY(1,1) NOT NULL," & vbCrLf & _
                " [IDUser] [int] NULL," & vbCrLf & _
                " [Periode] [datetime] NULL," & vbCrLf & _
                " [Dept] [varchar](50) NULL," & vbCrLf & _
                " [NilaiPembelian] [money] NULL," & vbCrLf & _
                " [NilaiPenjualan] [money] NULL," & vbCrLf & _
                " [ProfitMargin] [numeric](18, 2) NULL," & vbCrLf & _
                "  CONSTRAINT [PK_MLapProfitMarginPembelianBulanan] PRIMARY KEY CLUSTERED " & vbCrLf & _
                " (" & vbCrLf & _
                " [NoID] ASC" & vbCrLf & _
                " )WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]" & vbCrLf & _
                " ) ON [PRIMARY]"
            com.CommandText = SQL
            com.ExecuteNonQuery()
        Catch ex As Exception

        End Try
        Try
            SQL = " CREATE TABLE [dbo].[MGroupSupplier](" & vbCrLf & _
                  " [NoID] [int] NOT NULL," & vbCrLf & _
                  " [Kode] [varchar](50) NULL," & vbCrLf & _
                  " [Nama] [varchar](50) NULL," & vbCrLf & _
                  " [Keterangan] [varchar](150) NULL," & vbCrLf & _
                  " [IsActive] [bit] NULL," & vbCrLf & _
                  "  CONSTRAINT [PK_MGroupSupplier] PRIMARY KEY CLUSTERED " & vbCrLf & _
                  " (" & vbCrLf & _
                  " [NoID] ASC" & vbCrLf & _
                  " )WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]" & vbCrLf & _
                  " ) ON [PRIMARY]"
            com.CommandText = SQL
            com.ExecuteNonQuery()
        Catch ex As Exception

        End Try
        Try
            SQL = " CREATE TABLE [dbo].[MGroupSupplierD](" & vbCrLf & _
                  " [NoID] [int] NOT NULL," & vbCrLf & _
                  " [IDGroupSupplier] [int] NULL," & vbCrLf & _
                  " [IDAlamat] [int] NULL," & vbCrLf & _
                  "  CONSTRAINT [PK_MGroupSupplierD] PRIMARY KEY CLUSTERED " & vbCrLf & _
                  " (" & vbCrLf & _
                  " [NoID] ASC" & vbCrLf & _
                  " )WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]" & vbCrLf & _
                  " ) ON [PRIMARY]"
            com.CommandText = SQL
            com.ExecuteNonQuery()
        Catch ex As Exception

        End Try
        Try
            SQL = " CREATE TABLE [MLapTopTen](" & vbCrLf & _
                " [NoID] [int] IDENTITY(1,1) NOT NULL," & vbCrLf & _
                " [IDUser] [int] NULL," & vbCrLf & _
                " [IP] [varchar](50) NULL," & vbCrLf & _
                " [IDBarang] [int] NULL," & vbCrLf & _
                " [Barcode] [varchar](50) NULL," & vbCrLf & _
                " [Kode] [varchar](50) NULL," & vbCrLf & _
                " [Nama] [varchar](50) NULL," & vbCrLf & _
                " [QtyTerjual] [numeric](18, 2) NULL," & vbCrLf & _
                " [JumlahPembeli] [numeric](18, 2) NULL," & vbCrLf & _
                " [TotalPenjualan] [money] NULL," & vbCrLf & _
                "  CONSTRAINT [PK_MLapTopTen] PRIMARY KEY CLUSTERED " & vbCrLf & _
                " (" & vbCrLf & _
                " [NoID] ASC" & vbCrLf & _
                " )WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]" & vbCrLf & _
                " ) ON [PRIMARY]"
            com.CommandText = SQL
            com.ExecuteNonQuery()
        Catch ex As Exception

        End Try
        Try
            SQL = "ALTER TABLE MLapPembelianBulanan ADD " & vbCrLf & _
                  " P1 Money NULL, " & vbCrLf & _
                  " P2 Money NULL, " & vbCrLf & _
                  " P3 Money NULL, " & vbCrLf & _
                  " P4 Money NULL, " & vbCrLf & _
                  " P5 Money NULL, " & vbCrLf & _
                  " P6 Money NULL, " & vbCrLf & _
                  " P7 Money NULL, " & vbCrLf & _
                  " P8 Money NULL, " & vbCrLf & _
                  " P9 Money NULL, " & vbCrLf & _
                  " P10 Money NULL, " & vbCrLf & _
                  " P11 Money NULL, " & vbCrLf & _
                  " P12 Money NULL, " & vbCrLf & _
                  " P13 Money NULL, " & vbCrLf & _
                  " P14 Money NULL, " & vbCrLf & _
                  " P15 Money NULL, " & vbCrLf & _
                  " P16 Money NULL, " & vbCrLf & _
                  " P17 Money NULL, " & vbCrLf & _
                  " P18 Money NULL, " & vbCrLf & _
                  " P19 Money NULL, " & vbCrLf & _
                  " P20 Money NULL, " & vbCrLf & _
                  " P21 Money NULL, " & vbCrLf & _
                  " P22 Money NULL, " & vbCrLf & _
                  " P23 Money NULL, " & vbCrLf & _
                  " P24 Money NULL, " & vbCrLf & _
                  " P25 Money NULL, " & vbCrLf & _
                  " P26 Money NULL, " & vbCrLf & _
                  " P27 Money NULL, " & vbCrLf & _
                  " P28 Money NULL, " & vbCrLf & _
                  " P29 Money NULL, " & vbCrLf & _
                  " P30 Money NULL, " & vbCrLf & _
                  " P31 Money NULL, " & vbCrLf & _
                  " TotalP Money NULL, " & vbCrLf & _
                  " ProfitMargin Numeric(18,2) NULL "
            com.CommandText = SQL
            com.ExecuteNonQuery()
        Catch ex As Exception

        End Try
        Try
            SQL = "ALTER TABLE MStockOpname ADD " & vbCrLf & _
                  " IsLock BIT NULL "
            com.CommandText = SQL
            com.ExecuteNonQuery()
        Catch ex As Exception

        End Try
        Try
            SQL = "ALTER TABLE MStockOpname ADD " & vbCrLf & _
                  " TglLock DateTime NULL "
            com.CommandText = SQL
            com.ExecuteNonQuery()
        Catch ex As Exception

        End Try
        Try
            SQL = "ALTER TABLE MStockOpname ADD " & vbCrLf & _
                  " IDUserLock INT NULL "
            com.CommandText = SQL
            com.ExecuteNonQuery()
        Catch ex As Exception

        End Try
        Try
            SQL = "ALTER TABLE MBayarHutang ADD " & vbCrLf & _
                  " IDBank Int NULL "
            com.CommandText = SQL
            com.ExecuteNonQuery()
        Catch ex As Exception

        End Try
        Try
            SQL = "ALTER TABLE MBayarHutangDGiro ADD " & vbCrLf & _
                  " IDBank Int NULL "
            com.CommandText = SQL
            com.ExecuteNonQuery()
        Catch ex As Exception

        End Try
        Try
            SQL = "ALTER TABLE MBayarHutangDGiro ADD " & vbCrLf & _
                  " TglJatuhTempo Datetime NULL, " & vbCrLf & _
                  " NoGiro Varchar(50) NULL "
            com.CommandText = SQL
            com.ExecuteNonQuery()
        Catch ex As Exception

        End Try
        Try
            SQL = "ALTER TABLE MBeli ADD " & vbCrLf & _
                  " NoBendelPajak Int NULL "
            com.CommandText = SQL
            com.ExecuteNonQuery()
        Catch ex As Exception

        End Try
        Try
            SQL = "ALTER TABLE MLapSaldoHutang ADD " & vbCrLf & _
                  " Adjustment Money NULL"
            com.CommandText = SQL
            com.ExecuteNonQuery()
        Catch ex As Exception

        End Try
        Try
            SQL = "ALTER TABLE MReturBeli ADD " & vbCrLf & _
                  " IDKategori INT NULL "
            com.CommandText = SQL
            com.ExecuteNonQuery()
        Catch ex As Exception

        End Try
        Try
            SQL = "ALTER TABLE MReturBeli ADD " & vbCrLf & _
                  " TotalJual Money NULL "
            com.CommandText = SQL
            com.ExecuteNonQuery()
        Catch ex As Exception

        End Try
        Try
            SQL = "ALTER TABLE MBeli ADD " & vbCrLf & _
                  " IDKategori Int NULL "
            com.CommandText = SQL
            com.ExecuteNonQuery()
        Catch ex As Exception

        End Try
        Try
            SQL = "ALTER TABLE MBeli ADD " & vbCrLf & _
                  " TotalJual Money NULL "
            com.CommandText = SQL
            com.ExecuteNonQuery()
        Catch ex As Exception

        End Try
        Try
            SQL = "ALTER TABLE MAlamat ADD " & vbCrLf & _
                  " Dept Varchar(5) NULL "
            com.CommandText = SQL
            com.ExecuteNonQuery()
        Catch ex As Exception

        End Try
        Try
            SQL = "ALTER TABLE MBeliD ADD " & vbCrLf & _
                  " IDTypePajak INT NULL "
            com.CommandText = SQL
            com.ExecuteNonQuery()
        Catch ex As Exception

        End Try
        Try
            SQL = "ALTER TABLE MReturBeli ADD " & vbCrLf & _
                  " IDAlamatDNPWP INT NULL "
            com.CommandText = SQL
            com.ExecuteNonQuery()
        Catch ex As Exception

        End Try
        Try
            SQL = "ALTER TABLE MReturBeli ADD " & vbCrLf & _
                  " IsProsesPajak BIT NULL "
            com.CommandText = SQL
            com.ExecuteNonQuery()
        Catch ex As Exception

        End Try
        Try
            SQL = "ALTER TABLE MUser ADD " & vbCrLf & _
                  " IDDashBoard1 INT NULL, " & vbCrLf & _
                  " IDDashBoard2 INT NULL, " & vbCrLf & _
                  " IDDashBoard3 INT NULL, " & vbCrLf & _
                  " IDDashBoard4 INT NULL "
            com.CommandText = SQL
            com.ExecuteNonQuery()
        Catch ex As Exception

        End Try

        Try
            SQL = "ALTER TABLE MReturBeliD ADD " & vbCrLf & _
                  " NamaStock varchar(100) NULL "
            com.CommandText = SQL
            com.ExecuteNonQuery()
        Catch ex As Exception

        End Try

        Try
            SQL = "CREATE TABLE [dbo].[MBayarHutangDGiro](" & vbCrLf & _
                  " [NoID] [int] NOT NULL," & vbCrLf & _
                  " [IDBayarHutang] [int] NULL," & vbCrLf & _
                  " [Tanggal] [datetime] NULL," & vbCrLf & _
                  " [Total] [money] NULL," & vbCrLf & _
                  " [Keterangan] [varchar](50) NULL," & vbCrLf & _
                  " CONSTRAINT [PK_MBayarHutangDGiro] PRIMARY KEY CLUSTERED " & vbCrLf & _
                  " (( [NoID] ASC)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]" & vbCrLf & _
                  " ) ON [PRIMARY]"
            com.CommandText = SQL
            com.ExecuteNonQuery()
        Catch ex As Exception

        End Try

        Try
            SQL = "ALTER TABLE MUser ADD " & _
                  " IsBagPajak Bit NULL "
            com.CommandText = SQL
            com.ExecuteNonQuery()
        Catch ex As Exception

        End Try
        Try
            SQL = "ALTER TABLE MReturBeli ADD " & _
                  " NoFPMasukkan Varchar(30) NULL, " & _
                  " TglFPMasukkan Datetime NULL "
            com.CommandText = SQL
            com.ExecuteNonQuery()
        Catch ex As Exception

        End Try
        Try
            SQL = "ALTER TABLE MReturBeli " & _
                  " ADD PPN Numeric(18,2) NULL, " & _
                  " DPP Money NULL "
            com.CommandText = SQL
            com.ExecuteNonQuery()
        Catch ex As Exception

        End Try
        Try
            SQL = "ALTER TABLE MReturBeli " & _
                  " ADD NilaiPPN Money NULL"
            com.CommandText = SQL
            com.ExecuteNonQuery()
        Catch ex As Exception

        End Try
        Try
            SQL = "ALTER TABLE MReturBeli " & _
                  " ADD IDTypePajak INT NULL"
            com.CommandText = SQL
            com.ExecuteNonQuery()
        Catch ex As Exception

        End Try
        Try
            SQL = "ALTER TABLE MBayarHutangD " & _
                  " ADD TglBeli Datetime NULL "
            com.CommandText = SQL
            com.ExecuteNonQuery()
            SQL = "UPDATE MBayarHutangD SET MBayarHutangD.TglBeli=MBeli.Tanggal" & vbCrLf & _
                  " FROM MBayarHutangD " & vbCrLf & _
                  " INNER JOIN MBeli ON MBeli.NoID=MBayarHutangD.IDBeli " & vbCrLf & _
                  " WHERE MBayarHutangD.TglBeli IS NULL"
            com.CommandText = SQL
            com.ExecuteNonQuery()
        Catch ex As Exception

        End Try
        Try
            SQL = "ALTER TABLE TBayarHutangD " & _
                  " ADD TglBeli Datetime NULL "
            com.CommandText = SQL
            com.ExecuteNonQuery()
        Catch ex As Exception

        End Try

        Try
            SQL = "ALTER TABLE MBayarHutangDRetur " & _
                  " ADD TglRetur Datetime NULL "
            com.CommandText = SQL
            com.ExecuteNonQuery()
            SQL = "UPDATE MBayarHutangDRetur SET MBayarHutangDRetur.TglBeli=MReturBeli.Tanggal" & vbCrLf & _
                  " FROM MBayarHutangDRetur " & vbCrLf & _
                  " INNER JOIN MReturBeli ON MReturBeli.NoID=MBayarHutangDRetur.IDReturBeli " & vbCrLf & _
                  " WHERE MBayarHutangDRetur.TglRetur IS NULL"
            com.CommandText = SQL
            com.ExecuteNonQuery()
        Catch ex As Exception

        End Try
        Try
            SQL = "ALTER TABLE TBayarHutangDRetur " & _
                  " ADD TglRetur Datetime NULL "
            com.CommandText = SQL
            com.ExecuteNonQuery()
        Catch ex As Exception

        End Try

        Try
            SQL = "ALTER TABLE MUser " & _
                  " ADD IsReminderHutang BIT NULL "
            com.CommandText = SQL
            com.ExecuteNonQuery()
        Catch ex As Exception

        End Try
        Try
            SQL = "ALTER TABLE MBayarHutang " & _
                  " ADD IDAkunKwitansi INT NULL, " & _
                  " IDAkunKwitansiD INT NULL, " & _
                  " NoKwitansi Varchar(30) NULL, " & _
                  " KetKwitansi Varchar(100) NULL "
            com.CommandText = SQL
            com.ExecuteNonQuery()
        Catch ex As Exception

        End Try
        Try
            SQL = "ALTER TABLE MBayarHutang " & _
                  " ADD TglKembali smalldatetime NULL "
            com.CommandText = SQL
            com.ExecuteNonQuery()
        Catch ex As Exception

        End Try
        Try
            SQL = "ALTER TABLE MBayarHutang " & _
                  " ADD DN Money NULL "
            com.CommandText = SQL
            com.ExecuteNonQuery()
        Catch ex As Exception

        End Try
        Try
            SQL = "ALTER TABLE MStockOpnameD " & _
                  " ADD Falg Bit NULL "
            com.CommandText = SQL
            com.ExecuteNonQuery()
        Catch ex As Exception

        End Try
        Try
            SQL = "ALTER TABLE MBeli " & _
            " ADD IsTT BIT NULL," & _
            " NoFaktur VARCHAR(20) NULL, " & _
            " IsAdaFP BIT NULL"
            com.CommandText = SQL
            com.ExecuteNonQuery()
        Catch ex As Exception

        End Try
        Try
            SQL = "ALTER TABLE MJual " & _
            " ADD NoFakturPajak Varchar(30) NULL "
            com.CommandText = SQL
            com.ExecuteNonQuery()
        Catch ex As Exception

        End Try
        Try
            SQL = "ALTER TABLE MJual " & _
            " ADD IDTypePajak INT NULL "
            com.CommandText = SQL
            com.ExecuteNonQuery()
        Catch ex As Exception

        End Try
        Try
            SQL = "ALTER TABLE MBarang " & _
            " ADD IDUserEdit INT NULL"
            com.CommandText = SQL
            com.ExecuteNonQuery()
        Catch ex As Exception

        End Try

        Try
            SQL = "ALTER TABLE MUser " & _
            " ADD IsMnVPOS BIT NULL"
            com.CommandText = SQL
            com.ExecuteNonQuery()
            SQL = "UPDATE MUser " & _
            " SET IsMnVPOS =1 "
            com.CommandText = SQL
            com.ExecuteNonQuery()
        Catch ex As Exception

        End Try
        Try
            SQL = "ALTER TABLE MUser " & _
            " ADD IsMnDatabase BIT NULL"
            com.CommandText = SQL
            com.ExecuteNonQuery()
            SQL = "UPDATE MUser " & _
            " SET IsMnDatabase =1 "
            com.CommandText = SQL
            com.ExecuteNonQuery()
        Catch ex As Exception

        End Try
        Try
            SQL = "ALTER TABLE MUser " & _
            " ADD IsMnSetting BIT NULL"
            com.CommandText = SQL
            com.ExecuteNonQuery()
            SQL = "UPDATE MUser " & _
            " SET IsMnSetting =1 "
            com.CommandText = SQL
            com.ExecuteNonQuery()
        Catch ex As Exception

        End Try
        Try
            SQL = "ALTER TABLE MUser " & _
            " ADD IsMnDeveloper BIT NULL"
            com.CommandText = SQL
            com.ExecuteNonQuery()
            SQL = "UPDATE MUser " & _
            " SET IsMnDeveloper =1 "
            com.CommandText = SQL
            com.ExecuteNonQuery()
        Catch ex As Exception

        End Try
        'Nambah MMerk
        'MTypeCustomer.DiscountCustomer,
        Try
            SQL = "ALTER TABLE MTypeCustomer " & _
            " ADD DiscountMarketing Decimal(10,2) NULL DEFAULT 0"
            com.CommandText = SQL
            com.ExecuteNonQuery()
        Catch ex As Exception

        End Try
        'Nambah MMerk
        Try
            SQL = "ALTER TABLE MBayarHutang " & _
            " ADD IDTT int NULL DEFAULT 0"
            com.CommandText = SQL
            com.ExecuteNonQuery()
        Catch ex As Exception

        End Try
        Try
            SQL = "ALTER TABLE MMenu " & _
            " ADD CaptionCetak1 Varchar(50) NULL, CaptionCetak2 Varchar(50) NULL"
            com.CommandText = SQL
            com.ExecuteNonQuery()
        Catch ex As Exception

        End Try
        'Nambah MMerk
        Try
            SQL = "ALTER TABLE MSetting " & _
            " ADD IsStockReturPerSupplier bit NULL DEFAULT 0"
            com.CommandText = SQL
            com.ExecuteNonQuery()
        Catch ex As Exception

        End Try
        'Nambah MMerk
        Try
            SQL = "Create Table MMerk(NoID int NOT NULL Primary Key," & _
            "Kode varchar(30) NULL, " & _
            "Nama varchar(50) NULL, " & _
             "Keterangan varchar(150) NULL," & _
             "IsActive bit NULL)"
            com.CommandText = SQL
            com.ExecuteNonQuery()
        Catch ex As Exception

        End Try
        'Nambah MMerk
        Try
            SQL = "ALTER TABLE MBarang " & _
            " ADD IDMerk int NULL DEFAULT 0"
            com.CommandText = SQL
            com.ExecuteNonQuery()
        Catch ex As Exception

        End Try
        'Tanpa Barang Retur Beli
        Try
            SQL = "ALTER TABLE dbo.MReturBeli " & _
                  " ADD IsTanpaBarang bit NULL DEFAULT 0"
            com.CommandText = SQL
            com.ExecuteNonQuery()
        Catch ex As Exception

        End Try
        'Tanpa Barang Beli
        Try
            SQL = "ALTER TABLE dbo.MBeli " & _
                  " ADD IsTanpaBarang bit NULL DEFAULT 0"
            com.CommandText = SQL
            com.ExecuteNonQuery()
        Catch ex As Exception

        End Try

        'MTT Produk Baru
        Try
            SQL = "ALTER TABLE dbo.MTT " & _
                  " ADD IsProdukBaru bit NULL DEFAULT 0"
            com.CommandText = SQL
            com.ExecuteNonQuery()
        Catch ex As Exception

        End Try



        Try
            SQL = "Create TABLE dbo.MGeneratePO( NoID Int PRIMARY KEY IDENTITY(1,1) NOT NULL,IDUser Int,IDBarang Int,IDBarangD Int,IDSatuan Int,Harga money,HargaPcs money,DiscPersen1 numeric(18,2),DiscPersen2 numeric(18,2),DiscPersen3 numeric(18,2),Disc1 money,Qty Numeric(18,2), Konversi Numeric(18,2),QtyPcs Numeric(18,2),QtyJualPcs Numeric(18,2),QtyAkhirPcs Numeric(18,2),QtyHitungOrderPcs Numeric(18,2),HargaJualPcs Money,JumlahBeli money, JumlahJual Money)"
            com.CommandText = SQL

            com.ExecuteNonQuery()

        Catch ex As Exception

        End Try



        'TBayarHutangD
        Try
            SQL = "ALTER TABLE dbo.MBeli " & _
                  " ADD IsPKP bit NULL DEFAULT 0"
            com.CommandText = SQL
            com.ExecuteNonQuery()
        Catch ex As Exception

        End Try

        'TBayarHutangD
        Try
            SQL = "ALTER TABLE dbo.MBeli " & _
                  " ADD Pending bit NULL DEFAULT 0"
            com.CommandText = SQL
            com.ExecuteNonQuery()
        Catch ex As Exception

        End Try

        Try
            SQL = "ALTER TABLE dbo.TBayarhutangD " & _
                  " ADD Pending bit NULL DEFAULT 0"
            com.CommandText = SQL
            com.ExecuteNonQuery()
        Catch ex As Exception

        End Try
        Try
            SQL = "ALTER TABLE dbo.MBayarHutang " & _
                  " ADD Materai Money NULL DEFAULT 0"
            com.CommandText = SQL
            com.ExecuteNonQuery()
        Catch ex As Exception

        End Try

        Try
            SQL = "Create TABLE dbo.MMaterai( NoID Int, Kode varchar(20), Nama Varchar (50), Nilai Money, IsActive bit)"
            com.CommandText = SQL
            com.ExecuteNonQuery()
            SQL = "insert into MMaterai(NoID,Kode,Nama,Nilai,IsActive) " & _
            "values(1,'M4000','Materai 3000',4000,1)"
            com.CommandText = SQL
            com.ExecuteNonQuery()
            SQL = "insert into MMaterai(NoID,Kode,Nama,Nilai,IsActive) " & _
             "values(2,'M8000','Materai 6000',8000,1)"
            com.CommandText = SQL
            com.ExecuteNonQuery()
        Catch ex As Exception

        End Try

        Try
            SQL = "ALTER TABLE dbo.MSetting" & _
                  " ADD IsStockPerSupplier bit NULL"
            com.CommandText = SQL
            com.ExecuteNonQuery()
        Catch ex As Exception

        End Try
        Try
            SQL = "ALTER TABLE dbo.MSetting" & _
                  " ADD IsStockPerSupplier bit NULL"
            com.CommandText = SQL
            com.ExecuteNonQuery()
        Catch ex As Exception

        End Try
        Try
            SQL = "ALTER TABLE dbo.MStockOpnameD " & _
                          " ADD IDBarangD bigint NULL"
            com.CommandText = SQL
            com.ExecuteNonQuery()

            SQL = "ALTER TABLE dbo.MPenyesuaianD " & _
              " ADD IDBarangD bigint NULL"
            com.CommandText = SQL
            com.ExecuteNonQuery()

            SQL = "ALTER TABLE dbo.MPemakaianD " & _
              " ADD IDBarangD bigint NULL"
            com.CommandText = SQL
            com.ExecuteNonQuery()
        Catch ex As Exception

        End Try
        Try
            SQL = "ALTER TABLE dbo.MJual" & _
                  " ADD IDAkunBiayaPembelian int NULL"
            com.CommandText = SQL
            com.ExecuteNonQuery()

            SQL = "EXEC sp_refreshview 'dbo.qMJual'"
            com.CommandText = SQL
            com.ExecuteNonQuery()

            SQL = "EXEC sp_refreshview 'dbo.vRekapPenjualanPerDepartemen'"
            com.CommandText = SQL
            com.ExecuteNonQuery()

            SQL = "EXEC sp_refreshview 'dbo.vRekapPenjualanPerDepartemenBersih'"
            com.CommandText = SQL
            com.ExecuteNonQuery()

            SQL = "ALTER TABLE dbo.MSetting" & _
                  " ADD IsStockReturPerSupplier bit NULL"
            com.CommandText = SQL
            com.ExecuteNonQuery()

            SQL = "UPDATE MSetting SET IsStockReturPerSupplier=1 "
            com.CommandText = SQL
            com.ExecuteNonQuery()

            SQL = "ALTER TABLE MReturBeliD" & _
                  " ADD HBeli money NULL, " & _
                  " HJual money NULL "
            com.CommandText = SQL
            com.ExecuteNonQuery()

            SQL = "UPDATE dbo.MMenu SET isawalgroup = 1 WHERE noid = 85"
            com.CommandText = SQL
            com.ExecuteNonQuery()
        Catch ex As Exception

        End Try

        Try
            SQL = "ALTER TABLE dbo.MReturBeliD ADD" & vbCrLf & _
            " IDBarangD Int NULL"
            com.CommandText = SQL
            com.ExecuteNonQuery()

            SQL = "ALTER TABLE dbo.MAlamat ADD" & vbCrLf & _
            " IDTypeCustomer Int NULL"
            com.CommandText = SQL

            com.ExecuteNonQuery()
        Catch ex As Exception

        End Try
        Try
            SQL = "ALTER TABLE dbo.TBayarHutangD ADD" & vbCrLf & _
                  " PotongRetur Money NULL"
            com.CommandText = SQL
            com.ExecuteNonQuery()
            SQL = "ALTER TABLE dbo.MReturBeli ADD" & vbCrLf & _
                 " IDBeli BigInt NULL"
            com.CommandText = SQL
            com.ExecuteNonQuery()
            SQL = "ALTER TABLE dbo.MSetting ADD" & vbCrLf & _
                 " [PanjangKodeBarang] int NULL,[TypeBarcode] int NULL,[TypeKodeBarang] int NULL"
            com.CommandText = SQL
            com.ExecuteNonQuery()
            SQL = "ALTER TABLE dbo.MSetting ADD" & vbCrLf & _
                 " UpdateHargaBeli bit NULL"
            com.CommandText = SQL
            com.ExecuteNonQuery()
        Catch ex As Exception

        End Try
    End Sub
    Public Sub TambahkanKolom()
        Dim SQL As String = ""
        Dim cn As SqlConnection = Nothing
        Dim com As SqlCommand = Nothing
        Try
            If cn Is Nothing Then
                cn = New SqlConnection(StrKonSql)
                cn.Open()
                com = New SqlCommand
                com.Connection = cn
            End If
            'TambahKolomMei2013(cn, com, SQL)
            Try
                SQL = "ALTER TABLE MSaldoAwalPersediaanD ADD IDBarangD BIGINT NULL"
                com.CommandText = SQL
                com.ExecuteNonQuery()
            Catch ex As Exception

            End Try
            Try
                SQL = "ALTER TABLE MJUAL ADD IDReedemPoin INT NULL"
                com.CommandText = SQL
                com.ExecuteNonQuery()
            Catch ex As Exception

            End Try
            Try
                SQL = "ALTER TABLE MJUAL ADD ReedemPoin Numeric(18,2) NULL"
                com.CommandText = SQL
                com.ExecuteNonQuery()
            Catch ex As Exception

            End Try
            Try
                SQL = "ALTER TABLE MJUAL ADD ReedemNilai Money NULL"
                com.CommandText = SQL
                com.ExecuteNonQuery()
            Catch ex As Exception

            End Try
            Try
                SQL = "ALTER TABLE MSetting ADD TglDitetapkanSO Datetime NULL"
                com.CommandText = SQL
                com.ExecuteNonQuery()
            Catch ex As Exception

            End Try
            Try
                SQL = "ALTER TABLE MStockOpname ADD TglDitetapkanSO Datetime NULL"
                com.CommandText = SQL
                com.ExecuteNonQuery()
            Catch ex As Exception

            End Try
            Try
                SQL = "ALTER TABLE MBarang ADD IsLockQtyMax BIT NULL"
                com.CommandText = SQL
                com.ExecuteNonQuery()
            Catch ex As Exception

            End Try
        Catch ex As Exception
        Finally
            If Not cn Is Nothing Then
                cn.Close()
                cn.Dispose()
            End If
        End Try
    End Sub
    Public Sub AmbilSetingPerusahaan()
        Dim sql As String
        Dim ds As New DataSet
        Try

            sql = "SELECT * FROM MSetting"
            ds = ExecuteDataset("MSetting", sql)
            If ds.Tables(0).Rows.Count >= 1 Then
                'txtAlamatPerusahaan.Text = NullToStr(ds.Tables(0).Rows(0).Item("AlamatPerusahaan"))
                'txtNamaPerusahaan.Text = NullToStr(ds.Tables(0).Rows(0).Item("NamaPerusahaan"))
                'txtNPWP.Text = NullToStr(ds.Tables(0).Rows(0).Item("NPWP"))
                'rbReturGudang.EditValue = IIf(NullToBool(ds.Tables(0).Rows(0).Item("IsGudangBSDiRetur")), "1", "0")
                'rbJenisBarang.EditValue = IIf(NullToBool(ds.Tables(0).Rows(0).Item("IsStockPerJenis")), "1", "0")
                'txtNoServer.EditValue = NullToLong(ds.Tables(0).Rows(0).Item("IDServerAsal"))
                'txtGudang.EditValue = NullToLong(ds.Tables(0).Rows(0).Item("IDGudangInTransit")) 
                'ckUpdateHargaBeli.Checked = NullToBool(ds.Tables(0).Rows(0).Item("UpdateHargaBeli"))
                FolderLayouts = NullToStr(ds.Tables(0).Rows(0).Item("PathLayouts"))
                If Not PathExists(FolderLayouts, 1) Then
                    FolderLayouts = Application.StartupPath & "\System\Layouts\"
                End If

                'If Not System.IO.Directory.Exists(FolderLayouts) Then
                '    FolderLayouts = Application.StartupPath & "\System\Layouts\"
                'End If
                FolderFoto = NullToStr(ds.Tables(0).Rows(0).Item("PathImage"))
                'If Not System.IO.Directory.Exists(FolderFoto) Then
                '    FolderFoto = Application.StartupPath & "\System\PathFoto\"
                'End If
                If Not PathExists(FolderFoto, 1) Then
                    FolderFoto = Application.StartupPath & "\System\PathFoto\"
                End If
                PanjangKodeBarang = NullTolInt(ds.Tables(0).Rows(0).Item("PanjangKodeBarang"))
                TypeBarcode = NullTolInt(ds.Tables(0).Rows(0).Item("TypeBarcode"))
                TypeKodeBarang = NullTolInt(ds.Tables(0).Rows(0).Item("TypeKodeBarang"))
            End If
        Catch ex As Exception
            DevExpress.XtraEditors.XtraMessageBox.Show("Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            ds.Dispose()
        End Try

    End Sub
    Sub TambahkolomSysDbmaster()
        Dim cnOleDB As OleDbConnection = Nothing
        Dim cmOleDB As OleDbCommand = Nothing
        cnOleDB = New OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" & Application.StartupPath & "\System\engine\sysdbmaster.mdb")
        cnOleDB.Open()
        cmOleDB = New OleDbCommand()
        cmOleDB.Connection = cnOleDB

        Try
            cmOleDB.CommandText = "CREATE TABLE MReedem (" & vbCrLf & _
                                  "NoID INTEGER Primary Key, " & vbCrLf & _
                                  "Nilai Currency, " & vbCrLf & _
                                  "Poin INTEGER, " & vbCrLf & _
                                  "IsActive BIT)"
            cmOleDB.ExecuteNonQuery()
        Catch ex As Exception
            '  MsgBox(ex.Message)
        End Try
        Try
            cmOleDB.CommandText = "ALTER TABLE TInv ADD DiscPDPMember Currency"
            cmOleDB.ExecuteNonQuery()
        Catch ex As Exception
            '  MsgBox(ex.Message)
        End Try
        Try
            cmOleDB.CommandText = "ALTER TABLE TInv ADD DiscPDP Currency"
            cmOleDB.ExecuteNonQuery()
        Catch ex As Exception
            '  MsgBox(ex.Message)
        End Try
        Try
            cmOleDB.CommandText = "ALTER TABLE MSettingPoin ADD MinimumBelanjaDapatPDP Currency "
            cmOleDB.ExecuteNonQuery()
        Catch ex As Exception
            '  MsgBox(ex.Message)
        End Try
        Try
            cmOleDB.CommandText = "ALTER TABLE MSettingPoin ADD MinimumBelanjaDapatPoin2 Currency "
            cmOleDB.ExecuteNonQuery()
        Catch ex As Exception
            '  MsgBox(ex.Message)
        End Try
        Try
            cmOleDB.CommandText = "ALTER TABLE MCustomer add DiscountMarketing double"
            cmOleDB.ExecuteNonQuery()
        Catch ex As Exception
            '  MsgBox(ex.Message)
        End Try

        Try
            cmOleDB.CommandText = "ALTER TABLE TInv ADD TglDariDiskon2 Datetime, TglSampaiDiskon2 Datetime, DiscMemberProsen2 Double, DiscMemberRp2 Currency"
            cmOleDB.ExecuteNonQuery()
        Catch ex As Exception
            'MsgBox(ex.Message)
        End Try

        cmOleDB.Dispose()
        cnOleDB.Close()
        cnOleDB.Dispose()
    End Sub
    Public Function KirimDataKassa(ByVal IDKassa As Integer) As Boolean 'All Kassa=0/-1
        Dim cnOleDB As OleDbConnection = Nothing
        Dim cmOleDB As OleDbCommand = Nothing
        Dim SQL As String = ""
        Dim ds As New DataSet
        Dim dlg As DevExpress.Utils.WaitDialogForm = Nothing
        Dim FileSource As String = ""
        Dim FileDestination As String = ""
        Dim hasil As Boolean = False
        Dim Rep As New Member.ImpMember, DaftarReedem As New Model.DaftarMReedem
        Try
            'If IsSupervisor Then
            If XtraMessageBox.Show("Ingin mengirim data master ke Kassa " & IIf(IDKassa >= 1, IDKassa.ToString, "") & " ?" & vbCrLf & " Akan memerlukan sedikit waktu.", NamaAplikasi, MessageBoxButtons.YesNo, MessageBoxIcon.Information, MessageBoxDefaultButton.Button2) = Windows.Forms.DialogResult.Yes Then
                If System.IO.File.Exists(Application.StartupPath & "\System\engine\sysdbmaster.mdb") Then
                    TambahkolomSysDbmaster()
                    Application.DoEvents()
                    Dim curentcursor As Cursor = Windows.Forms.Cursor.Current
                    Windows.Forms.Cursor.Current = Cursors.WaitCursor
                    dlg = New DevExpress.Utils.WaitDialogForm(String.Format("Sedang merefresh data.{0}MOHON TUNGGU ...", vbCrLf), Application.ProductName)
                    dlg.TopMost = False
                    dlg.Show()
                    Windows.Forms.Cursor.Current = Cursors.WaitCursor
                    IO.File.Copy(Application.StartupPath & "\System\engine\sysdbmaster.mdb", Application.StartupPath & "\System\engine\dbmaster.mdb", True)
                    'Provider=Microsoft.Jet.OLEDB.4.0;Data Source==D:\PROJECT\CITYTOYS\CitiToys\bin\Debug\System\engine\sysdbmaster.mdb
                    'TglDariDiskon2, TglSampaiDiskon2, DiscMemberProsen2, DiscMemberRp2
                    SQL = "SELECT MBarang.DiscPDP, MBarang.DiscPDPMember, MBarang.TglDariDiskon2, MBarang.TglSampaiDiskon2, MBarang.DiscMemberProsen2, MBarang.DiscMemberRp2, MBarang.TglDariDiskon, MBarang.TglSampaiDiskon, MBarangD.*, MBarang.Kode AS KodeBarang, MBarang.Nama AS NamaBarang, MSatuan.Kode AS Satuan, MBarang.HargaBeliPcs as  HargaPokok, MBarang.IsFamilyGroup, MBarang.HargaFamily, MBarang.TanggalDariFamily, MBarang.TanggalSampaiFamily, MBarang.IsPoin, 0 AS IsOperator, CASE WHEN IsNull(MBarang.IDPoinSupplier,0)>=1 THEN 1 ELSE 0 END AS IsPoinSupplier, MBarang.IDPoinSupplier, MBarang.BKP, MBarang.IsGroupQty, MBarang.HargaBeliPcs " & vbCrLf & _
                          " FROM (MBarangD INNER JOIN MBarang ON MBarang.NoID=MBarangD.IDBarang) INNER JOIN MSatuan ON MSatuan.NoID=MBarangD.IDSatuan  " & vbCrLf & _
                          " WHERE IsNull(MBarang.Kode,'')<>'' AND IsNull(MBarang.Nama,'')<>'' AND IsNull(MBarangD.Barcode,'')<>'' AND MBarangD.IsActive=1 AND MBarang.IsActive=1 AND MBarangD.IsJualPOS=1"
                    ds = ExecuteDataset("MBarang", SQL)
                    With ds.Tables("MBarang")
                        If .Rows.Count >= 1 Then
                            cnOleDB = New OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" & Application.StartupPath & "\System\engine\dbmaster.mdb")
                            cnOleDB.Open()
                            cmOleDB = New OleDbCommand("DELETE FROM TInv", cnOleDB)
                            cmOleDB.CommandText = "DELETE FROM TInv"
                            cmOleDB.ExecuteNonQuery()
                            For i As Integer = 0 To .Rows.Count - 1
                                Application.DoEvents()
                                SQL = "INSERT INTO TInv ( NoID, TglDariDiskon2, TglSampaiDiskon2, DiscMemberProsen2, DiscMemberRp2, IDInventor, Kode, Barcode, Nama, IDSatuan, KodeSat, Konversi, HargaJual, " & vbCrLf & _
                                      " HargaPokok, DiscExpired, DiscMulai, DiscProsen, DiscRupiah, IsPoin, IsOperator, IsPoinSupplier, IDPoinSupplier, " & _
                                      " BKP, HargaMin, IsKelipatan, QtyKelipatan, HargaKelipatan, Qty1, Harga1, Qty2, Harga2, Qty3, Harga3, " & vbCrLf & _
                                      " IsAllowDisc, IsFamilyGroup, QtyFamily, HargaFamily, TanggalDariFamily, TanggalSampaiFamily, IsGroupQty, HargaBeliTerakhir, DiscPDP, DiscPDPMember) VALUES (" & vbCrLf & _
                                      NullToLong(.Rows(i).Item("NoID")) & "," & vbCrLf & _
                                      "#" & NullToDate(.Rows(i).Item("TglDariDiskon2")).ToString("yyyy-MM-dd") & "#, " & vbCrLf & _
                                      "#" & NullToDate(.Rows(i).Item("TglSampaiDiskon2")).ToString("yyyy-MM-dd") & "#, " & vbCrLf & _
                                      FixKoma(NullToDbl(.Rows(i).Item("DiscMemberProsen2"))) & ", " & vbCrLf & _
                                      FixKoma(NullToDbl(.Rows(i).Item("DiscMemberRp2"))) & "," & vbCrLf & _
                                      NullToLong(.Rows(i).Item("IDBarang")) & "," & vbCrLf & _
                                      "'" & FixApostropi(NullToStr(.Rows(i).Item("KodeBarang"))) & "'," & vbCrLf & _
                                      "'" & FixApostropi(NullToStr(.Rows(i).Item("Barcode"))) & "'," & vbCrLf & _
                                      "'" & FixApostropi(IIf(NullToStr(.Rows(i).Item("Varian")).Length >= 1, NullToStr(.Rows(i).Item("NamaBarang")) & " " & NullToStr(.Rows(i).Item("Varian")), NullToStr(.Rows(i).Item("NamaBarang")))) & "'," & vbCrLf & _
                                      NullToLong(.Rows(i).Item("IDSatuan")) & "," & vbCrLf & _
                                      "'" & FixApostropi(NullToStr(.Rows(i).Item("Satuan"))) & "'," & vbCrLf & _
                                      FixKoma(NullToDbl(.Rows(i).Item("Konversi"))) & "," & vbCrLf & _
                                      FixKoma(NullToDbl(.Rows(i).Item("HargaJual"))) & "," & vbCrLf & _
                                      FixKoma(NullToDbl(.Rows(i).Item("HargaPokok"))) & "," & vbCrLf & _
                                      "#" & NullToDate(.Rows(i).Item("TglSampaiDiskon")).ToString("yyyy/MM/dd") & "#," & vbCrLf & _
                                      "#" & NullToDate(.Rows(i).Item("TglDariDiskon")).ToString("yyyy/MM/dd") & "#," & vbCrLf & _
                                      FixKoma(NullToDbl(.Rows(i).Item("PromoDiskonJual"))) & "," & vbCrLf & _
                                      FixKoma(NullToDbl(.Rows(i).Item("NilaiDiskon"))) & "," & vbCrLf & _
                                      IIf(NullToBool(.Rows(i).Item("IsPoin")), 1, 0) & "," & vbCrLf & _
                                      IIf(NullToBool(.Rows(i).Item("IsOperator")), 1, 0) & "," & vbCrLf & _
                                      IIf(NullToBool(.Rows(i).Item("IsPoinSupplier")), 1, 0) & "," & vbCrLf & _
                                      NullToLong(.Rows(i).Item("IDPoinSupplier")) & "," & vbCrLf & _
                                      IIf(NullToBool(.Rows(i).Item("BKP")), 1, 0) & "," & vbCrLf & _
                                      FixKoma(0) & "," & vbCrLf & _
                                      IIf(NullToBool(.Rows(i).Item("IsKelipatan")), 1, 0) & "," & vbCrLf & _
                                      FixKoma(NullToDbl(.Rows(i).Item("QtyKelipatan"))) & "," & vbCrLf & _
                                      FixKoma(NullToDbl(.Rows(i).Item("HargaKelipatan"))) & "," & vbCrLf & _
                                      FixKoma(NullToDbl(.Rows(i).Item("Qty1"))) & "," & vbCrLf & _
                                      FixKoma(NullToDbl(.Rows(i).Item("HargaJual"))) & "," & vbCrLf & _
                                      FixKoma(NullToDbl(.Rows(i).Item("Qty2"))) & "," & vbCrLf & _
                                      FixKoma(NullToDbl(.Rows(i).Item("HargaJual2"))) & "," & vbCrLf & _
                                      FixKoma(NullToDbl(.Rows(i).Item("Qty3"))) & "," & vbCrLf & _
                                      FixKoma(NullToDbl(.Rows(i).Item("HargaJual3"))) & "," & vbCrLf & _
                                      IIf(NullToBool(.Rows(i).Item("IsAllowDisc")), 1, 0) & "," & vbCrLf & _
                                      IIf(NullToBool(.Rows(i).Item("IsFamilyGroup")), 1, 0) & "," & vbCrLf & _
                                      FixKoma(NullToDbl(0)) & "," & vbCrLf & _
                                      FixKoma(NullToDbl(.Rows(i).Item("HargaFamily"))) & "," & vbCrLf & _
                                      "#" & NullToDate(.Rows(i).Item("TanggalDariFamily")).ToString("yyyy/MM/dd") & "#, " & vbCrLf & _
                                      "#" & NullToDate(.Rows(i).Item("TanggalSampaiFamily")).ToString("yyyy/MM/dd") & "#, " & vbCrLf & _
                                      IIf(NullToBool(.Rows(i).Item("IsGroupQty")), 1, 0) & ", " & vbCrLf & _
                                      FixKoma(NullToDbl(.Rows(i).Item("HargaBeliPcs"))) & ", " & vbCrLf & _
                                      FixKoma(NullToDbl(.Rows(i).Item("DiscPDP"))) & ", " & vbCrLf & _
                                      FixKoma(NullToDbl(.Rows(i).Item("DiscPDPMember"))) & " " & vbCrLf & _
                                      ")"

                                cmOleDB.CommandText = SQL
                                cmOleDB.ExecuteNonQuery()
                            Next
                        End If
                    End With

                    cmOleDB.CommandText = "DELETE FROM MReedem"
                    cmOleDB.ExecuteNonQuery()
                    DaftarReedem = Rep.DaftarMReedem("IsActive=1")
                    For i As Integer = 0 To DaftarReedem.Count - 1
                        Application.DoEvents()
                        SQL = "INSERT INTO MReedem ([NoID],[Poin],[Nilai],[IsActive]) VALUES (" & vbCrLf & _
                              FixKoma(DaftarReedem(i).NoID) & "," & vbCrLf & _
                              FixKoma(DaftarReedem(i).Poin) & "," & vbCrLf & _
                              FixKoma(DaftarReedem(i).Nilai) & "," & vbCrLf & _
                              IIf(DaftarReedem(i).IsActive, 1, 0) & ")"
                        cmOleDB.CommandText = SQL
                        cmOleDB.ExecuteNonQuery()
                    Next

                    'Provider=Microsoft.Jet.OLEDB.4.0;Data Source==D:\PROJECT\CITYTOYS\CitiToys\bin\Debug\System\engine\sysdbmaster.mdb
                    SQL = "SELECT MUser.* " & vbCrLf & _
                          " FROM MUser " & vbCrLf & _
                          " WHERE IsKasir=1 "
                    ds = ExecuteDataset("MUser", SQL)
                    With ds.Tables("MUser")
                        If .Rows.Count >= 1 Then
                            cmOleDB.CommandText = "DELETE FROM MEmp"
                            cmOleDB.ExecuteNonQuery()
                            For i As Integer = 0 To .Rows.Count - 1
                                Application.DoEvents()
                                SQL = "INSERT INTO MEMP ([NoID],[Kode],[Nama],[Nick],[Password],[IsKasir],[IsPengawas],[IsOperator],[IsOperator1],[IsPengawasUtama]) VALUES (" & vbCrLf & _
                                      NullToLong(.Rows(i).Item("NoID")) & "," & vbCrLf & _
                                      "'" & FixApostropi(NullToStr(.Rows(i).Item("Kode"))) & "'," & vbCrLf & _
                                      "'" & FixApostropi(NullToStr(.Rows(i).Item("Nama"))) & "'," & vbCrLf & _
                                      "'" & FixApostropi(NullToStr(.Rows(i).Item("Nama"))) & "'," & vbCrLf & _
                                      "'" & FixApostropi(DecryptText(NullToStr(.Rows(i).Item("Pwd")), "vpoint")) & "'," & vbCrLf & _
                                      NullTolInt(.Rows(i).Item("IsKasir")) & "," & vbCrLf & _
                                      NullTolInt(.Rows(i).Item("IsPengawasKasir")) & "," & vbCrLf & _
                                      NullTolInt(.Rows(i).Item("IsSupervisor")) & "," & vbCrLf & _
                                      NullTolInt(.Rows(i).Item("IsSupervisor")) & "," & vbCrLf & _
                                      NullTolInt(.Rows(i).Item("IsPengawasUtamaKasir")) & ")"

                                cmOleDB.CommandText = SQL
                                cmOleDB.ExecuteNonQuery()
                            Next
                        End If
                    End With

                    'Provider=Microsoft.Jet.OLEDB.4.0;Data Source==D:\PROJECT\CITYTOYS\CitiToys\bin\Debug\System\engine\sysdbmaster.mdb
                    SQL = "SELECT MAlamat.*,MTypeCustomer.DiscountCustomer,MTypeCustomer.DiscountMarketing,MTypeCustomer.SyaratMinimum,MTypeCustomer.AllowInputDiscount FROM MAlamat left join MTypeCustomer On MAlamat.IDTypeCustomer=MTypeCustomer.NoID " & vbCrLf & _
                          " WHERE MAlamat.IsActive = 1 And MAlamat.IsCustomer = 1"
                    ds = ExecuteDataset("MCust", SQL)
                    With ds.Tables("MCust")
                        If .Rows.Count >= 1 Then
                            cmOleDB.CommandText = "DELETE FROM MCustomer"
                            cmOleDB.ExecuteNonQuery()
                            For i As Integer = 0 To .Rows.Count - 1
                                Application.DoEvents()
                                SQL = "INSERT INTO MCustomer ([NoID],[Kode],[Barcode],[Nama],[DOJ],[DiscountCustomer],[DiscountMarketing],[SyaratMinimum],[AllowInputDiscount]) VALUES (" & vbCrLf & _
                                      NullToLong(.Rows(i).Item("NoID")) & "," & vbCrLf & _
                                      "'" & FixApostropi(NullToStr(.Rows(i).Item("Kode"))) & "'," & vbCrLf & _
                                      NullToLong(.Rows(i).Item("NoID")) & "," & vbCrLf & _
                                      "'" & FixApostropi(NullToStr(.Rows(i).Item("Nama"))) & "'," & vbCrLf & _
                                      "#" & NullToDate(.Rows(i).Item("DOJ")).ToString("yyyy/MM/dd") & "#," & _
                                       FixKoma(NullToDbl(.Rows(i).Item("DiscountCustomer"))) & "," & _
                                         FixKoma(NullToDbl(.Rows(i).Item("DiscountMarketing"))) & "," & _
                                       FixKoma(NullToDbl(.Rows(i).Item("SyaratMinimum"))) & "," & NullTolInt(.Rows(i).Item("AllowInputDiscount")) & ")"
                                cmOleDB.CommandText = SQL
                                cmOleDB.ExecuteNonQuery()
                            Next
                        End If
                    End With

                    'Provider=Microsoft.Jet.OLEDB.4.0;Data Source==D:\PROJECT\CITYTOYS\CitiToys\bin\Debug\System\engine\sysdbmaster.mdb
                    SQL = "SELECT MPoinSupplier.* FROM MPoinSupplier " & vbCrLf & _
                          " WHERE MPoinSupplier.IsActive = 1 "
                    ds = ExecuteDataset("MPoinSupplier", SQL)
                    With ds.Tables("MPoinSupplier")
                        If .Rows.Count >= 1 Then
                            cmOleDB.CommandText = "DELETE FROM MPoinSupplier"
                            cmOleDB.ExecuteNonQuery()
                            For i As Integer = 0 To .Rows.Count - 1
                                Application.DoEvents()
                                SQL = "INSERT INTO MPoinSupplier ([NoID],[Kode],[Nama],[Nilai1Poin],[IsKelipatan]) VALUES (" & vbCrLf & _
                                      NullToLong(.Rows(i).Item("NoID")) & "," & vbCrLf & _
                                      "'" & FixApostropi(NullToStr(.Rows(i).Item("Kode"))) & "'," & vbCrLf & _
                                      "'" & FixApostropi(NullToStr(.Rows(i).Item("Catatan"))) & "'," & vbCrLf & _
                                      FixKoma(NullToDbl(.Rows(i).Item("Nilai1Poin"))) & "," & IIf(NullToBool(.Rows(i).Item("IsKelipatan")), -1, 0) & ")"

                                cmOleDB.CommandText = SQL
                                cmOleDB.ExecuteNonQuery()
                            Next
                        End If
                    End With

                    'Provider=Microsoft.Jet.OLEDB.4.0;Data Source==D:\PROJECT\CITYTOYS\CitiToys\bin\Debug\System\engine\sysdbmaster.mdb
                    SQL = "SELECT MSettingPoin.* FROM MSettingPoin " & vbCrLf & _
                          " WHERE MSettingPoin.IsActive = 1 "
                    ds = ExecuteDataset("MSettingPoin", SQL)
                    With ds.Tables("MSettingPoin")
                        If .Rows.Count >= 1 Then
                            cmOleDB.CommandText = "DELETE FROM MSettingPoin"
                            cmOleDB.ExecuteNonQuery()
                            For i As Integer = 0 To .Rows.Count - 1
                                Application.DoEvents()
                                SQL = "INSERT INTO MSettingPoin ([NoID],[NilaiBelanja1Poin],[MinimumBelanjaDapatPoin],[MinimumBelanjaDapatPoin2],[NilaiDiskon],[CreditCardDapatDiskon],[MinimumBelanjaDapatPDP]) VALUES (" & vbCrLf & _
                                      NullToLong(.Rows(i).Item("NoID")) & "," & vbCrLf & _
                                      FixKoma(NullToDbl(.Rows(i).Item("NilaiBelanja1Poin"))) & "," & vbCrLf & _
                                      FixKoma(NullToDbl(.Rows(i).Item("MinimumBelanjaDapatDiskon"))) & "," & vbCrLf & _
                                      FixKoma(NullToDbl(.Rows(i).Item("MinimumBelanjaDapatDiskon2"))) & "," & vbCrLf & _
                                      FixKoma(NullToDbl(.Rows(i).Item("NilaiDiskon"))) & "," & vbCrLf & _
                                      IIf(NullToBool(.Rows(i).Item("CreditCardDapatDiskon")), 1, 0) & "," & vbCrLf & _
                                      FixKoma(NullToDbl(.Rows(i).Item("MinimumBelanjaDapatPDP"))) & ")"
                                cmOleDB.CommandText = SQL
                                cmOleDB.ExecuteNonQuery()
                            Next
                        End If
                    End With

                    'MBank
                    SQL = "SELECT MBank.* FROM MBank " & vbCrLf & _
                         " WHERE MBank.IsPos = 1 "
                    ds = ExecuteDataset("MBank", SQL)
                    With ds.Tables("MBank")
                        If .Rows.Count >= 1 Then
                            cmOleDB.CommandText = "DELETE FROM MBank"
                            cmOleDB.ExecuteNonQuery()
                            For i As Integer = 0 To .Rows.Count - 1
                                Application.DoEvents()
                                SQL = "INSERT INTO MBank([NoID],[Kode],[Nama],[IsKartuKredit],[IsKartuDebet]) VALUES (" & vbCrLf & _
                                      NullToLong(.Rows(i).Item("NoID")) & ",'" & _
                                      FixApostropi(NullToStr(.Rows(i).Item("Kode"))) & "','" & _
                                      FixApostropi(NullToStr(.Rows(i).Item("Nama"))) & "'," & _
                                      IIf(NullToBool(.Rows(i).Item("IsKartuKredit")), -1, 0) & "," & vbCrLf & _
                                      IIf(NullToBool(.Rows(i).Item("IsKartuDebet")), -1, 0) & ")"

                                cmOleDB.CommandText = SQL
                                cmOleDB.ExecuteNonQuery()
                            Next
                        End If
                    End With

                    'MJenisKartu
                    SQL = "SELECT MJenisKartu.* FROM MJenisKartu " & vbCrLf & _
                          " WHERE MJenisKartu.IsActive = 1 "
                    ds = ExecuteDataset("MJenisKartu", SQL)
                    With ds.Tables("MJenisKartu")
                        If .Rows.Count >= 1 Then
                            cmOleDB.CommandText = "DELETE FROM MJenisKartu"
                            cmOleDB.ExecuteNonQuery()
                            For i As Integer = 0 To .Rows.Count - 1
                                Application.DoEvents()
                                SQL = "INSERT INTO MJenisKartu([NoID],[Kode],[Nama],[IsKartuKredit],[IsKartuDebet],[Charge],[ChargeMember]) VALUES (" & vbCrLf & _
                                      NullToLong(.Rows(i).Item("NoID")) & ",'" & _
                                      FixApostropi(NullToStr(.Rows(i).Item("Kode"))) & "','" & _
                                      FixApostropi(NullToStr(.Rows(i).Item("Nama"))) & "'," & _
                                      IIf(NullToBool(.Rows(i).Item("IsKartuKredit")), -1, 0) & "," & vbCrLf & _
                                      IIf(NullToBool(.Rows(i).Item("IsKartuDebet")), -1, 0) & "," & vbCrLf & _
                                      FixKoma(NullToDbl(.Rows(i).Item("Charge"))) & "," & _
                                      FixKoma(NullToDbl(.Rows(i).Item("ChargeMember"))) & ")"

                                cmOleDB.CommandText = SQL
                                cmOleDB.ExecuteNonQuery()
                            Next
                        End If
                    End With

                    'PenerbitVoucher
                    SQL = "SELECT MPenerbitVoucher.* FROM MPenerbitVoucher " & vbCrLf & _
                          " WHERE MPenerbitVoucher.IsActive = 1 "
                    ds = ExecuteDataset("MPenerbitVoucher", SQL)
                    With ds.Tables("MPenerbitVoucher")
                        If .Rows.Count >= 1 Then
                            cmOleDB.CommandText = "DELETE FROM MPenerbitVoucher"
                            cmOleDB.ExecuteNonQuery()
                            For i As Integer = 0 To .Rows.Count - 1
                                Application.DoEvents()
                                SQL = "INSERT INTO MPenerbitVoucher ([NoID],[Kode],[Nama],[Nominal]) VALUES (" & vbCrLf & _
                                      NullToLong(.Rows(i).Item("NoID")) & "," & vbCrLf & _
                                      "'" & FixApostropi(NullToStr(.Rows(i).Item("Kode"))) & "'," & vbCrLf & _
                                      "'" & FixApostropi(NullToStr(.Rows(i).Item("Nama"))) & "'," & vbCrLf & _
                                      FixKoma(NullToDbl(.Rows(i).Item("Nominal"))) & ")"

                                cmOleDB.CommandText = SQL
                                cmOleDB.ExecuteNonQuery()
                            Next
                        End If
                    End With
                    'Tutup Database
                    If Not cnOleDB Is Nothing Then
                        cmOleDB.Dispose()
                        cnOleDB.Close()
                        cnOleDB.Dispose()
                    End If

                    'Copy Database ke Kassa
                    If IDKassa >= 1 Then
                        SQL = "SELECT MPOS.* FROM MPOS WHERE MPOS.IsActive=1 AND MPOS.NoID=" & IDKassa
                    Else
                        SQL = "SELECT MPOS.* FROM MPOS WHERE MPOS.IsActive=1"
                    End If
                    ds = ExecuteDataset("MPOS", SQL)
                    For i As Integer = 0 To ds.Tables("MPOS").Rows.Count - 1
                        Application.DoEvents()
                        If System.IO.Directory.Exists(NullToStr(ds.Tables("MPOS").Rows(i).Item("PathDbTemp"))) Then
                            Try
                                FileSource = Application.StartupPath & "\System\engine\dbmaster.mdb"
                                FileDestination = AppendBackSlash(NullToStr(ds.Tables("MPOS").Rows(i).Item("PathDbTemp"))) & "Update\DBMaster.mdb"
                                If System.IO.File.Exists(AppendBackSlash(NullToStr(ds.Tables("MPOS").Rows(i).Item("PathDbTemp"))) & "Update\temp.txt") Then
                                    System.IO.File.Delete(AppendBackSlash(NullToStr(ds.Tables("MPOS").Rows(i).Item("PathDbTemp"))) & "Update\temp.txt")
                                End If
                                If System.IO.File.Exists(FileDestination) Then
                                    System.IO.File.Delete(FileDestination)
                                End If
                                System.IO.File.Copy(FileSource, FileDestination)
                                Ini.TulisIniPath(Application.StartupPath & "\system\engine\temp.txt", "Database", "TglUpdate", TanggalSystem.ToString("dd-MM-yyyy HH:mm"))
                                System.IO.File.Copy(Application.StartupPath & "\system\engine\temp.txt", AppendBackSlash(NullToStr(ds.Tables("MPOS").Rows(i).Item("PathDbTemp"))) & "Update\temp.txt", True)
                                'Try
                                '    FileDestination = NullToStr(ds.Tables("MPOS").Rows(i).Item("PathDbTemp")) & "\Database\DBMaster.mdb"
                                '    System.IO.File.Copy(FileSource, FileDestination, True)
                                'Catch ex As Exception

                                'End Try
                                EksekusiSQL("Update MPOS SET DatabaseUpdate=getdate() WHERE NoID=" & NullToLong(ds.Tables("MPOS").Rows(i).Item("NoID")))
                            Catch ex As Exception
                                XtraMessageBox.Show("Info : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK)
                            End Try
                        Else
                            XtraMessageBox.Show("Direktori " & NullToStr(ds.Tables("MPOS").Rows(i).Item("PathDbTemp")) & " Tidak ditemukan!", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
                        End If
                    Next
                    hasil = True
                Else
                    XtraMessageBox.Show("File system tidak ditemukan.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Warning)
                    hasil = False
                End If
            End If
            'End If
        Catch ex As Exception
            XtraMessageBox.Show("Gagal mengeksekusi SQL:" & SQL & vbCrLf & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
            hasil = False
        Finally
            Windows.Forms.Cursor.Current = Cursors.Default
            If Not cnOleDB Is Nothing Then
                cmOleDB.Dispose()
                If cnOleDB.State = ConnectionState.Open Then
                    cnOleDB.Close()
                End If
                cnOleDB.Dispose()
            End If
            If Not dlg Is Nothing Then
                dlg.Close()
                dlg.Dispose()
            End If
        End Try
        Return hasil
    End Function

    Public Sub KirimDataKassaKeBO(ByVal IDPOS As Long, ByVal TGL As Date, ByVal PB1 As ProgressBarControl, ByVal IsReplace As Boolean, ByVal PathDB As String)
        Dim OleDBcn As OleDbConnection = Nothing
        Dim OleDBODA As OleDbDataAdapter = Nothing
        Dim OleDBocmd As OleDbCommand = Nothing
        Dim Trans As SqlTransaction = Nothing
        Dim Conn As SqlConnection = Nothing
        Dim Comm As SqlCommand = Nothing
        Dim ds As New DataSet
        Dim Eks As Integer = 0
        Try
            Application.DoEvents()
            'Provider=Microsoft.Jet.OLEDB.4.0;Data Source=\\serverName\shareName\folder\myDatabase.mdb;User Id=admin;Password=;
            Dim strsql As String = ""
            Dim SQL As String = ""
            Dim IDJual As Long
            Dim IDJualD As Long
            Dim IDSales As Long
            Dim IDgudang As Integer
            Dim IDWilayah As Integer
            Dim NamaKasir As String = ""
            Dim DiscPersen1 As Double
            Dim JumTransaksi As Integer, Sukses As Boolean = False
            'Buat Begin Tran
            Conn = New SqlConnection(StrKonSql)
            Conn.Open()
            Comm = Conn.CreateCommand()
            Comm.CommandTimeout = Conn.ConnectionTimeout
            Trans = Conn.BeginTransaction("MJualPOS")
            Comm.Connection = Conn
            Comm.Transaction = Trans
            If IsReplace Then
                SQL = "SELECT COUNT(MJualD.NoID) FROM MJualD INNER JOIN MJual ON MJual.NoID=MJualD.IDJual WHERE MJual.IsPosted=1 AND MJual.IsPOS=1 AND MJual.IDPOS=" & NullToLong(IDPOS) & " AND MJual.Tanggal>='" & TGL.ToString("yyyy-MM-dd") & "' AND MJual.Tanggal<'" & DateAdd(DateInterval.Day, 1, TGL).ToString("yyyy-MM-dd") & "'"
                Comm.CommandText = SQL
                If NullToLong(Comm.ExecuteScalar()) >= 1 Then
                    'XtraMessageBox.Show("Proses Download Gagal!" & vbCrLf & "Data Sudah ada / Diposting, Cek Terlebih Dahulu. ", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
                    Trans.Rollback("MJualPOS")
                    PB1.Position = 100
                    Application.DoEvents()
                    Exit Try
                Else
                    SQL = "DELETE FROM MKartuStok WHERE IDTransaksi IN (SELECT MJual.NoID FROM MJual WHERE MJual.IsPOS=1 AND MJual.IDPOS=" & NullToLong(IDPOS) & " AND MJual.Tanggal>='" & TGL.ToString("yyyy-MM-dd") & "' AND MJual.Tanggal<'" & DateAdd(DateInterval.Day, 1, TGL).ToString("yyyy-MM-dd") & "') AND IDJenisTransaksi=6"
                    Comm.CommandText = SQL
                    Comm.ExecuteNonQuery() 'Hapus Penjualan yg sudah terposting
                    Comm.CommandText = "Delete MJualD from MjualD inner join Mjual on Mjuald.idjual=mjual.noid Where mjual.Tanggal>='" & Format(TGL, "yyyy-MM-dd") & "' and Mjual.Tanggal<'" & Format(TGL.AddDays(1), "yyyy-MM-dd") & "' and MJual.IDPOS=" & NullToLong(IDPOS)
                    Comm.ExecuteNonQuery() 'Hapus di MJualD
                    Comm.CommandText = "Delete MJual from MJual WHERE MJual.Tanggal>='" & Format(TGL, "yyyy-MM-dd") & "' AND Mjual.Tanggal<'" & Format(TGL.AddDays(1), "yyyy-MM-dd") & "' and IDPOS=" & NullToLong(IDPOS)
                    Comm.ExecuteNonQuery() 'Hapus di MJual
                    Eks = 1
                End If
                Application.DoEvents()
            End If
            IDPOS = NullToLong(IDPOS)
            Comm.CommandText = "Select IDGudang From MPos Where NoID=" & NullToLong(IDPOS)
            IDgudang = NullToLong(Comm.ExecuteScalar())
            Comm.CommandText = "Select IDWilayah From MPos Where NoID=" & NullToLong(IDPOS)
            IDWilayah = NullToLong(Comm.ExecuteScalar())
            OleDBcn = New OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" & PathDB & IIf(Strings.Right(PathDB, 1) = "\", "", "\") & "database\Tempdb_" & Format(TGL, "yyyyMM") & ".mdb" & ";User Id=admin;Password=;")
            OleDBocmd = New OleDb.OleDbCommand
            OleDBocmd.Connection = OleDBcn
            OleDBocmd.CommandType = CommandType.Text
            OleDBcn.Open()
            Try
                strsql = "ALTER TABLE MSalesD ADD IsPDP BIT"
                OleDBocmd.ExecuteNonQuery()
            Catch ex As Exception
                'XtraMessageBox.Show("Info Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
            End Try
            strsql = "Select * from MSales Where Tanggal>=#" & Format(TGL, "MM/dd/yyyy") & "# and Tanggal<#" & Format(TGL.AddDays(1), "MM/dd/yyyy") & "# order by NoID"
            OleDBocmd.CommandText = strsql

            OleDBODA = New OleDbDataAdapter(OleDBocmd)
            OleDBODA.TableMappings.Add("Tabel", "Data")
            If ds.Tables("Data") Is Nothing Then
            Else
                ds.Tables("Data").Clear()
            End If
            OleDBODA.Fill(ds, "Data")
            JumTransaksi = NullToLong(ds.Tables("Data").Rows.Count)
            For i = 0 To JumTransaksi - 1
                PB1.Position = (((i + 1) * 100) \ JumTransaksi)
                Application.DoEvents()
                'Cek apakah Sudah Ada atau Belum
                TGL = ds.Tables("Data").Rows(i).Item("Tanggal")
                IDSales = NullToLong(ds.Tables("Data").Rows(i).Item("NoID"))
                Comm.CommandText = "Select COUNT(NoID) AS Ada FROM MJual WHERE MONTH(Tanggal)=" & TGL.Date.Month & " AND YEAR(Tanggal)=" & TGL.Date.Year & " AND DAY(Tanggal)=" & TGL.Date.Day & " AND IDPos=" & IDPOS & " AND NoIDPos=" & IDSales
                If NullToLong(Comm.ExecuteScalar()) = 0 Then 'Belum ada di server
                    Comm.CommandText = "SELECT MAX(MJual.NoID) FROM MJual"
                    IDJual = NullToLong(Comm.ExecuteScalar) + 1
                    Sukses = False
                    NamaKasir = NullToStr(EksekusiSQlSkalarNew("SELECT NAMA FROM MUser WHERE NoID=" & NullToLong(ds.Tables("Data").Rows(i).Item("IDUser"))))
                    SQL = "INSERT INTO MJual (IDGudang,IDWilayah,IsPOS,NoID,Kode,KodeReff,Tanggal,TanggalStock,JatuhTempo,"
                    SQL = SQL & " IDCustomer,TanggalSJ,NoSJ,SubTotal,DiskonNotaProsen,DiskonNotaRp,DiskonNotaTotal,"
                    SQL = SQL & " Biaya, Total, Bayar, Sisa,IDAdmin,IDPacking,Shift,NamaKasir,Pembulatan,IDBank,NoAcc,IDPOS,NoIDPOS,Kas,Bank,Voucher,Charge,TotalBKP," & _
                    "DPP, NilaiPPN,IDJenisKartu,TasKresekA,TasKresekB,TasKresekC,TasKresekD," & _
                    "BarangPoin, NilaiPoin, SisaPoin,KodeMarketing,FeeMarketing,FeeMarketingRp, IDReedemPoin, ReedemPoin, ReedemNilai)  VALUES (" & vbCrLf
                    SQL = SQL & IDgudang & "," & IDWilayah & ","
                    SQL = SQL & 1 & ","
                    SQL = SQL & IDJual & ","
                    SQL = SQL & "'" & Replace(ds.Tables("Data").Rows(i).Item("kode") & "/" & Format(IDPOS, "00") & "/" & Format(Now, "yyMM"), "'", "''") & "',"
                    SQL = SQL & "'" & Replace(ds.Tables("Data").Rows(i).Item("kode"), "'", "''") & "',"
                    SQL = SQL & "'" & Format(ds.Tables("Data").Rows(i).Item("TANGGAL"), "yyyy-MM-dd HH:mm:ss") & "',"
                    SQL = SQL & "'" & Format(ds.Tables("Data").Rows(i).Item("TANGGAL"), "yyyy-MM-dd HH:mm:ss") & "',"
                    SQL = SQL & "'" & Format(DateAdd("d", 30, CDate(ds.Tables("Data").Rows(i).Item("TANGGAL"))), "yyyy-MM-dd HH:mm:ss") & "',"
                    SQL = SQL & NullToLong(ds.Tables("Data").Rows(i).Item("IDMember")) & ","
                    SQL = SQL & "'" & Format(ds.Tables("Data").Rows(i).Item("TANGGAL"), "yyyy-MM-dd HH:mm:ss") & "',"
                    SQL = SQL & "'',"
                    SQL = SQL & FixKoma(ds.Tables("Data").Rows(i).Item("SubTotal")) & ","
                    SQL = SQL & FixKoma(0) & ","
                    SQL = SQL & FixKoma(ds.Tables("Data").Rows(i).Item("DiscIntern")) & ","
                    SQL = SQL & FixKoma(ds.Tables("Data").Rows(i).Item("DiscIntern") + NullToLong(ds.Tables("Data").Rows(i).Item("Pembulatan"))) & ","
                    SQL = SQL & FixKoma(ds.Tables("Data").Rows(i).Item("HargaTotal") - ds.Tables("Data").Rows(i).Item("SubTotal") - ds.Tables("Data").Rows(i).Item("Pembulatan") - ds.Tables("Data").Rows(i).Item("DiscIntern")) & ","
                    SQL = SQL & FixKoma(ds.Tables("Data").Rows(i).Item("SubTotal") - ds.Tables("Data").Rows(i).Item("Pembulatan") - ds.Tables("Data").Rows(i).Item("DiscIntern")) & ","
                    SQL = SQL & FixKoma(ds.Tables("Data").Rows(i).Item("UangMuka")) & ","
                    SQL = SQL & FixKoma(ds.Tables("Data").Rows(i).Item("Hargatotal") - ds.Tables("Data").Rows(i).Item("UangMuka")) & ","
                    SQL = SQL & NullToLong(ds.Tables("Data").Rows(i).Item("IDUser")) & ","
                    SQL = SQL & -1 & "," & Replace(ds.Tables("Data").Rows(i).Item("Shift"), "'", "''") & ", '" & Replace(NamaKasir, "'", "''") & "'," & FixKoma(NullToLong(ds.Tables("Data").Rows(i).Item("Pembulatan"))) & "," & NullToLong(ds.Tables("Data").Rows(i).Item("IDBank")) & ",'" & Replace(NullToStr(ds.Tables("Data").Rows(i).Item("NoAcc")), "'", "''") & "'," & IDPOS & "," & NullToLong(ds.Tables("Data").Rows(i).Item("NoID")) & "," & _
                    FixKoma(ds.Tables("Data").Rows(i).Item("UangMuka") - ds.Tables("Data").Rows(i).Item("Voucher") - ds.Tables("Data").Rows(i).Item("Bank")) & "," & FixKoma(ds.Tables("Data").Rows(i).Item("Bank")) & "," & FixKoma(ds.Tables("Data").Rows(i).Item("Voucher")) & "," & FixKoma(ds.Tables("Data").Rows(i).Item("Charge")) & "," & FixKoma(ds.Tables("Data").Rows(i).Item("TotalBKP")) & "," & _
                    FixKoma(ds.Tables("Data").Rows(i).Item("DPP")) & "," & FixKoma(ds.Tables("Data").Rows(i).Item("PPN")) & "," & NullToLong(ds.Tables("Data").Rows(i).Item("IDJenisKartu")) & "," & NullToLong(ds.Tables("Data").Rows(i).Item("TasKresekA")) & "," & NullToLong(ds.Tables("Data").Rows(i).Item("TasKresekB")) & "," & NullToLong(ds.Tables("Data").Rows(i).Item("TasKresekC")) & "," & NullToLong(ds.Tables("Data").Rows(i).Item("TasKresekD")) & "," & _
                    NullToLong(ds.Tables("Data").Rows(i).Item("BarangPoin")) & "," & NullToLong(ds.Tables("Data").Rows(i).Item("NilaiPoin")) & "," & NullToLong(ds.Tables("Data").Rows(i).Item("SisaPoin")) & ",'" & _
                    FixApostropi(NullToStr(ds.Tables("Data").Rows(i).Item("Sopir"))) & "'," & NullToDbl(ds.Tables("Data").Rows(i).Item("Komisi")) & "," & NullToLong(ds.Tables("Data").Rows(i).Item("KomisiRp")) & "," & FixKoma(NullToLong(ds.Tables("Data").Rows(i).Item("IDReedemPoin"))) & "," & FixKoma(NullToDbl(ds.Tables("Data").Rows(i).Item("ReedemPoin"))) & "," & FixKoma(NullToDbl(ds.Tables("Data").Rows(i).Item("NilaiReedemPoin"))) & ")"
                    Comm.CommandText = SQL
                    If Comm.ExecuteNonQuery() >= 1 Then
                        ''Yanto Tambahkan Untuk Ke Poin Member Tgl 12-09-2015
                        'Comm.CommandText = "SELECT ISNULL(MAX(NoID),0) FROM [dbo].[MKartuPoinMember]"
                        'SQL = "INSERT INTO [dbo].[MKartuPoinMember] ([NoID],[IDTransaksi],[IDJenisTransaksi],[KodeReff],[Tanggal],[IDAlamat],[PoinMasuk],[PoinKeluar])" & vbCrLf & _
                        '      " SELECT " & NullToLong(Comm.ExecuteScalar) + 1 & ", [NoID], 19, [Kode], [Tanggal], [IDCustomer], [NilaiPoin], 0 " & vbCrLf & _
                        '      " FROM MJual WHERE [NoID]=" & IDJual & " AND ISNULL([NilaiPoin],0)>=1"
                        'Comm.CommandText = SQL
                        'Comm.ExecuteNonQuery()
                        ''Untuk ReedemPoin di Poin Keluar
                        'Comm.CommandText = "SELECT ISNULL(MAX(NoID),0) FROM [dbo].[MKartuPoinMember]"
                        'SQL = "INSERT INTO [dbo].[MKartuPoinMember] ([NoID],[IDTransaksi],[IDJenisTransaksi],[KodeReff],[Tanggal],[IDAlamat],[PoinMasuk],[PoinKeluar])" & vbCrLf & _
                        '      " SELECT " & NullToLong(Comm.ExecuteScalar) + 1 & ", [NoID], 19, [Kode], [Tanggal], [IDCustomer], 0, [ReedemPoin] " & vbCrLf & _
                        '      " FROM MJual WHERE [NoID]=" & IDJual & " AND ISNULL([NilaiPoin],0)>=1"
                        'Comm.CommandText = SQL
                        'Comm.ExecuteNonQuery()
                        Sukses = True
                        Eks = 1
                        Application.DoEvents()
                        OleDBODA = New OleDbDataAdapter("Select * from MsalesD Where IDSales=" & IDSales, OleDBcn)
                        If ds.Tables("Detil") Is Nothing Then
                        Else
                            ds.Tables("Detil").Clear()
                        End If
                        OleDBODA.Fill(ds, "Detil")
                        For j = 0 To ds.Tables("Detil").Rows.Count - 1
                            Comm.CommandText = "SELECT MAX(MJualD.NoID) FROM MJualD"
                            IDJualD = NullToLong(Comm.ExecuteScalar) + 1
                            If ds.Tables("Detil").Rows(j).Item("HargaBruto") <> 0 Then
                                DiscPersen1 = (ds.Tables("Detil").Rows(j).Item("DiscRp") + ds.Tables("Detil").Rows(j).Item("DiscInternRp")) * 100 / ds.Tables("Detil").Rows(j).Item("HargaBruto")
                            Else
                                DiscPersen1 = 0
                            End If

                            SQL = "INSERT INTO MJualD (NoID,IDJual,IDPackingD,NoUrut,Tgl,Jam,IDBarang,IDBarangD,IDSatuan,Qty,QtyPcs," & _
                            "Harga,HargaPcs,HargaPokok,CTN,DiscPersen1,DiscPersen2,DiscPersen3,Disc1,Disc2,Disc3,Jumlah,Catatan,IDGudang," & _
                            "Konversi,IsPoin,IsPoinSupplier,IDPoinSupplier, BKP,Transaksi, IsPDP, IsDisc2, HargaNormal ) VALUES ("
                            SQL = SQL & IDJualD & ","
                            SQL = SQL & IDJual & ","
                            SQL = SQL & -1 & ","
                            SQL = SQL & i & ","
                            SQL = SQL & "GetDate(),"
                            SQL = SQL & "GetDate(),"
                            SQL = SQL & ds.Tables("Detil").Rows(j).Item("IdInventor") & ","
                            SQL = SQL & ds.Tables("Detil").Rows(j).Item("IDInvsat") & ","
                            SQL = SQL & ds.Tables("Detil").Rows(j).Item("idSatuan") & ","
                            SQL = SQL & FixKoma(ds.Tables("Detil").Rows(j).Item("QTY")) & ","
                            SQL = SQL & FixKoma(ds.Tables("Detil").Rows(j).Item("QTY") * ds.Tables("Detil").Rows(j).Item("Konversi")) & ","
                            SQL = SQL & FixKoma(ds.Tables("Detil").Rows(j).Item("HargaBruto")) & ","
                            SQL = SQL & FixKoma(ds.Tables("Detil").Rows(j).Item("Jumlah") / IIf(ds.Tables("Detil").Rows(j).Item("QTY") = 0, 1, ds.Tables("Detil").Rows(j).Item("QTY")) / IIf(ds.Tables("Detil").Rows(j).Item("Konversi") = 0, 1, ds.Tables("Detil").Rows(j).Item("Konversi"))) & ","
                            SQL = SQL & FixKoma(ds.Tables("Detil").Rows(j).Item("HargaPokok")) & "," 'HPP
                            SQL = SQL & "0" & ","
                            SQL = SQL & FixKoma(ds.Tables("Detil").Rows(j).Item("DiscInternProsen")) & ","
                            SQL = SQL & FixKoma(0) & ","
                            SQL = SQL & FixKoma(0) & ","
                            SQL = SQL & FixKoma(ds.Tables("Detil").Rows(j).Item("DiscInternRp")) & ","
                            SQL = SQL & FixKoma(0) & ","
                            SQL = SQL & FixKoma(0) & ","
                            SQL = SQL & FixKoma(ds.Tables("Detil").Rows(j).Item("Jumlah")) & ","
                            If ds.Tables("Detil").Rows(j).Item("Transaksi") = "PLU" Then
                                SQL = SQL & "'Penjualan POS',"
                            ElseIf ds.Tables("Detil").Rows(j).Item("Transaksi") = "VOD" Then
                                SQL = SQL & "'Void POS',"
                            Else
                                SQL = SQL & "'Returan POS',"
                            End If
                            SQL = SQL & IDgudang & ","
                            SQL = SQL & FixKoma(NullToLong(ds.Tables("Detil").Rows(j).Item("Konversi"))) & ","
                            SQL = SQL & NullToLong(ds.Tables("Detil").Rows(j).Item("IsPoin")) & ","
                            SQL = SQL & NullToLong(ds.Tables("Detil").Rows(j).Item("IsPoinSupplier")) & ","
                            SQL = SQL & NullToLong(ds.Tables("Detil").Rows(j).Item("IDPoinSupplier")) & ","
                            SQL = SQL & NullToLong(ds.Tables("Detil").Rows(j).Item("BKP")) & ",'"
                            SQL = SQL & NullToStr(ds.Tables("Detil").Rows(j).Item("Transaksi")) & "',"
                            SQL = SQL & IIf(NullToBool(ds.Tables("Detil").Rows(j).Item("IsPDP")), 1, 0) & ","
                            SQL = SQL & IIf(NullToBool(ds.Tables("Detil").Rows(j).Item("IsDisc2")), 1, 0) & ","
                            SQL = SQL & FixKoma(NullToLong(ds.Tables("Detil").Rows(j).Item("HargaNormal"))) & " "
                            SQL = SQL & ")"
                            Comm.CommandText = SQL
                            If Not Comm.ExecuteNonQuery() >= 1 Then
                                Sukses = False
                                Eks = 1
                                Exit For
                            End If
                        Next
                        If Eks = 1 Then
                            Trans.Commit()
                            Trans = Conn.BeginTransaction("MJualPOS")
                            Comm.Connection = Conn
                            Comm.Transaction = Trans
                            Eks = 0
                        End If
                        'Hartani Otomatis Langsung Posting
                        clsPostingPenjualan.PostingStokBarangPenjualan(IDJual)
                    Else
                        Sukses = False
                        Exit For
                    End If
                Else
                    Sukses = True
                End If
                If Eks = 1 Then
                    Trans.Commit()
                    Trans = Conn.BeginTransaction("MJualPOS")
                    Comm.Connection = Conn
                    Comm.Transaction = Trans
                    Eks = 0
                End If
            Next
            If Sukses Then
                'XtraMessageBox.Show("Proses Download Selesai!", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
                If Eks = 1 Then
                    Trans.Commit()
                End If
            Else
                'XtraMessageBox.Show("Proses Download Gagal!", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
                If Eks = 1 Then
                    Trans.Rollback("MJualPOS")
                End If
                PB1.Position = 100
            End If
            PB1.Position = 0
        Catch ex As Exception
            XtraMessageBox.Show("Proses Download Gagal!" & vbCrLf & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
            If Eks = 1 Then
                Trans.Rollback("MJualPOS")
            End If
        Finally
            If Not ds Is Nothing Then
                ds.Dispose()
            End If
            If Not OleDBODA Is Nothing Then
                OleDBODA.Dispose()
            End If
            If Not OleDBcn Is Nothing Then
                OleDBcn.Close()
                OleDBcn.Dispose()
            End If
            If Not OleDBocmd Is Nothing Then
                OleDBocmd.Dispose()
            End If
            If Not Comm Is Nothing Then
                Comm.Dispose()
            End If
            If Not Conn Is Nothing Then
                Conn.Close()
                Conn.Dispose()
            End If
        End Try
    End Sub

    Public Function KirimDataTimbangan() As Boolean
        Dim SQL As String = ""
        Dim Kalimat As String = ""
        Dim ds As New DataSet
        Dim dlg As DevExpress.Utils.WaitDialogForm = Nothing
        Dim FileSource As String = ""
        Dim FileDestination As String = ""
        Dim hasil As Boolean = False
        Dim namafileCSV As String = Application.StartupPath & "\System\engine\DataBaru.csv"
        Try
            If XtraMessageBox.Show("Ingin mengirim data master khusus ?" & vbCrLf & " Akan memerlukan sedikit waktu.", NamaAplikasi, MessageBoxButtons.YesNo, MessageBoxIcon.Information, MessageBoxDefaultButton.Button2) = Windows.Forms.DialogResult.Yes Then
                Dim curentcursor As Cursor = Windows.Forms.Cursor.Current
                Windows.Forms.Cursor.Current = Cursors.WaitCursor
                dlg = New DevExpress.Utils.WaitDialogForm(String.Format("Sedang merefresh data.{0}MOHON TUNGGU ...", vbCrLf), Application.ProductName)
                dlg.TopMost = False
                dlg.Show()
                Windows.Forms.Cursor.Current = Cursors.WaitCursor
                IO.File.Copy(Application.StartupPath & "\System\Engine\defData.csv", Application.StartupPath & "\System\Engine\DataBaru.csv", True)

                Microsoft.VisualBasic.FileOpen(1, namafileCSV, OpenMode.Output) ' Open file for output.
                'Kalimat = "Kode Pajak;Kode lampiran;Kode Status;Kode Dokumen;NPWP Lawan Transaksi;Nama Lawan Transaksi;Kode cabang;Digit Tahun/Kode Faktur;No Seri;Tanggal Faktur;Tanggal SSP;Masa Pajak;Tahun Pajak;Pembetulan;DPP;PPN;PPnBM."
                'Microsoft.VisualBasic.Print(1, Kalimat)

                SQL = "SELECT MBarangD.*, MBarang.Kode AS KodeBarang, MBarang.Nama AS NamaBarang, MSatuan.Kode AS Satuan, MBarang.HPP AS HargaPokok, MBarang.IsFamilyGroup, MBarang.HargaFamily, MBarang.TanggalDariFamily, MBarang.TanggalSampaiFamily, MBarang.IsPoin, 0 AS IsOperator, CASE WHEN IsNull(MBarang.IDPoinSupplier,0)>=1 THEN 1 ELSE 0 END AS IsPoinSupplier, MBarang.IDPoinSupplier, MBarang.BKP " & vbCrLf & _
                      " FROM (MBarangD INNER JOIN MBarang ON MBarang.NoID=MBarangD.IDBarang) LEFT JOIN MSatuan ON MSatuan.NoID=MBarangD.IDSatuan  " & vbCrLf & _
                      " WHERE (left(MBarang.Kode,2)='25' or left(MBarang.Kode,2)='27') AND MBarangD.IsActive=1 AND MBarang.IsActive=1 AND MBarangD.IsJualPOS=1"
                ds = ExecuteDataset("MBarang", SQL)
                With ds.Tables("MBarang")
                    If .Rows.Count >= 1 Then
                        For i As Integer = 0 To .Rows.Count - 1
                            Kalimat = i.ToString & "," & FixApostropi(NullToStr(.Rows(i).Item("NamaBarang"))) & "," & _
                            NullToStr(.Rows(i).Item("KodeBarang")) & "," & _
                            NullToStr(.Rows(i).Item("KodeBarang")) & "," & _
                            "7" & "," & _
                                  FixKoma(100 * NullToDbl(.Rows(i).Item("HargaJual"))) & "," & _
                                  "4," & _
                                  "2" & ",0" & _
                                  ",15,0,0,5,0,0,0,0,0,0" & vbCrLf
                            Microsoft.VisualBasic.Print(1, Kalimat)
                        Next
                    End If
                End With
                Microsoft.VisualBasic.FileClose(1)
            End If
            Shell(Application.StartupPath & "\CopyTimbangan.bat", AppWinStyle.NormalFocus)
        Catch ex As Exception
            XtraMessageBox.Show("Gagal mengeksekusi SQL:" & SQL & vbCrLf & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
            hasil = False
        Finally
            Windows.Forms.Cursor.Current = Cursors.Default

            If Not dlg Is Nothing Then
                dlg.Close()
                dlg.Dispose()
            End If
        End Try
        Return hasil
    End Function
    Public Function IsDatabaseConnected() As Boolean
        'Dim SQLConn As SQLConnection = Nothing
        Dim mcon As SqlConnection = Nothing
        Dim connect As Boolean
        Try
            Application.DoEvents()
            mcon = New SqlConnection(StrKonSql)
            mcon.Open()
            connect = True
        Catch ex As Exception
            connect = False
        Finally
            mcon.Close()
            mcon.Dispose()
        End Try
        Return connect
    End Function
    Public StrKonSql As String = "Data Source=" & "." & ";initial Catalog=" & "DBCITYTOYS" & ";Integrated Security=True;Connect Timeout=60"
    Public StrKonSqlServer2 As String = "Data Source=" & "." & ";initial Catalog=" & "DBCITYTOYS" & ";Integrated Security=True;Connect Timeout=60"
    Public Function IsiVariabelDef(ByVal strsql As String) As String
        Dim strsql1 As String = strsql
        If InStr(strsql, "{DefIDDepartemen}", CompareMethod.Text) > 0 Then
            strsql1 = Replace(strsql1, "{DefIDDepartemen}", DefIDDepartemen.ToString)
        End If
        If InStr(strsql1, "{DefIDWilayah}", CompareMethod.Text) > 0 Then
            strsql1 = Replace(strsql1, "{DefIDWilayah}", DefIDWilayah.ToString)
        End If
        If InStr(strsql1, "{DefIDGudang}", CompareMethod.Text) > 0 Then
            strsql1 = Replace(strsql1, "{DefIDGudang}", DefIDGudang.ToString)
        End If
        If InStr(strsql1, "{DefIDSatuan}", CompareMethod.Text) > 0 Then
            strsql1 = Replace(strsql1, "{DefIDSatuan}", DefIDSatuan.ToString)
        End If
        If InStr(strsql1, "{DefIDPegawai}", CompareMethod.Text) > 0 Then
            strsql1 = Replace(strsql1, "{DefIDPegawai}", DefIDPegawai.ToString)
        End If
        If InStr(strsql1, "{DefIDCustomer}", CompareMethod.Text) > 0 Then
            strsql1 = Replace(strsql1, "{DefIDCustomer}", DefIDCustomer.ToString)
        End If
        If InStr(strsql1, "{DefIDSatuanfrmBarang}", CompareMethod.Text) > 0 Then
            strsql1 = Replace(strsql1, "{DefIDSatuanfrmBarang}", DefIDSatuanfrmBarang.ToString)
        End If
        If InStr(strsql1, "{DefIDSupplier}", CompareMethod.Text) > 0 Then
            strsql1 = Replace(strsql1, "{DefIDSupplier}", DefIDSupplier.ToString)
        End If
        Return strsql1
    End Function
    Public Sub ExecuteDBGrid(ByRef dbgrid As GridControl, ByVal sql As String, Optional ByVal FieldNamePK As String = "ID")
        Dim curentcursor As Cursor = Windows.Forms.Cursor.Current
        Dim oDS As New DataSet
        Dim dlg As WaitDialogForm = New WaitDialogForm("Sedang merefresh data." & vbCrLf & "MOHON TUNGGU ...", NamaAplikasi)
        Try
            dlg.TopMost = True
            dlg.Show()
            Windows.Forms.Cursor.Current = Cursors.WaitCursor
            Const tbel As String = "NamaTabel"
            Dim cview As ColumnView = CType(dbgrid.DefaultView, ColumnView)
            Dim viewID As New DevExpress.XtraGrid.Views.Grid.GridView
            Dim x As Integer = 0
            viewID = CType(dbgrid.DefaultView, Views.Grid.GridView)
            If Not cview Is Nothing Then
                x = cview.FocusedRowHandle
            End If
            oDS = ExecuteDataset(tbel, sql)
            dbgrid.DataSource = oDS.Tables(tbel)
            'SetGridView(dbgrid)
            If Not viewID Is Nothing Then
                viewID.ClearSelection()
                viewID.ClearColumnsFilter()
                viewID.ClearColumnErrors()
                viewID.OptionsFilter.Reset()
                viewID.FocusedRowHandle = x
                viewID.SelectRow(viewID.FocusedRowHandle)
                viewID.ShowFindPanel()
            End If
        Catch ex As Exception
            XtraMessageBox.Show(ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            Windows.Forms.Cursor.Current = curentcursor
            dlg.Close()
            dlg.Dispose()
            If Not oDS Is Nothing Then
                oDS.Dispose()
            End If
        End Try
    End Sub
    Public Function ExecuteDataset(ByVal tbel As String, ByVal SQL As String) As DataSet
        Dim oConn As SqlConnection = Nothing
        Dim ocmd As SqlCommand = Nothing
        Dim ODA As SqlDataAdapter = Nothing
        Try
            oConn = New SqlConnection(StrKonSql)
            ocmd = New SqlCommand(SQL, oConn)
            oConn.Open()
            ocmd.CommandTimeout = oConn.ConnectionTimeout
            ODA = New SqlDataAdapter(ocmd)
            ExecuteDataset = New DataSet
            If Not ExecuteDataset.Tables(tbel) Is Nothing Then
                ExecuteDataset.Tables(tbel).Clear()
            End If
            ODA.Fill(ExecuteDataset, tbel)
        Catch ex As Exception
            ExecuteDataset = Nothing
        Finally
            If Not ODA Is Nothing Then
                ODA.Dispose()
            End If
            If Not ocmd Is Nothing Then
                ocmd.Dispose()
            End If
            If Not oConn Is Nothing Then
                oConn.Close()
                oConn.Dispose()
            End If
        End Try
    End Function
    Public Function GetNewID(ByVal tabel As String, Optional ByVal nmField As String = "NoID", Optional ByVal FilterButuhWhere As String = "") As Long
        Dim oConn As SqlConnection
        Dim ocmd As SqlCommand
        Dim NoID As Long
        Dim strsql As String
        strsql = "Select max(" & nmField & ") as NewNoID from " & tabel
        If FilterButuhWhere.Trim <> "" Then
            strsql &= FilterButuhWhere
        End If
        oConn = New SqlConnection(StrKonSql)
        ocmd = New SqlCommand(strsql, oConn)
        oConn.Open()
        NoID = NullToLong(ocmd.ExecuteScalar) + 1
        ocmd.Dispose()
        oConn.Close()
        oConn.Dispose()
        Return NoID
    End Function
    Public Function EksekusiSQlSkalarNew(ByVal strsql As String) As Object
        Dim oConn As SqlConnection = Nothing
        Dim ocmd As SqlCommand = Nothing
        Dim hasil As Object = ""
        Try
            oConn = New SqlConnection(StrKonSql)
            ocmd = New SqlCommand(strsql, oConn)
            oConn.Open()
            ocmd.CommandTimeout = oConn.ConnectionTimeout
            hasil = ocmd.ExecuteScalar() 'NullToStr(ocmd.ExecuteScalar())
        Catch ex As Exception
            'XtraMessageBox.Show()
        Finally
            If Not ocmd Is Nothing Then
                ocmd.Dispose()
            End If
            If Not oConn Is Nothing Then
                oConn.Close()
                oConn.Dispose()
            End If
        End Try
        Return hasil
    End Function
    Public Function EksekusiSQLSkalar(ByVal SQl As String) As Long
        Dim oConn As SqlConnection = Nothing
        Dim ocmd As SqlCommand = Nothing
        Dim NoID As Long
        Try
            oConn = New SqlConnection(StrKonSql)
            ocmd = New SqlCommand(SQl, oConn)
            oConn.Open()
            ocmd.CommandTimeout = oConn.ConnectionTimeout
            NoID = NullToLong(ocmd.ExecuteScalar)
            Return NoID
        Catch ex As Exception
            XtraMessageBox.Show("Gagal mengeksekusi SQL :" & SQl & vbCrLf & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
        Finally
            If Not ocmd Is Nothing Then
                ocmd.Dispose()
            End If
            If Not oConn Is Nothing Then
                oConn.Close()
                oConn.Dispose()
            End If
        End Try
    End Function
    Public Function EksekusiSQL(ByVal SQl As String) As Integer
        Dim oConn As SqlConnection = Nothing
        Dim ocmd As SqlCommand = Nothing
        Dim NoID As Long
        Try
            oConn = New SqlConnection(StrKonSql)
            ocmd = New SqlCommand(SQl, oConn)
            oConn.Open()
            ocmd.CommandTimeout = oConn.ConnectionTimeout
            NoID = NullToLong(ocmd.ExecuteNonQuery)
            Return NoID
        Catch ex As Exception
            XtraMessageBox.Show("Gagal mengeksekusi SQL :" & SQl & vbCrLf & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
        Finally
            If Not ocmd Is Nothing Then
                ocmd.Dispose()
            End If
            If Not oConn Is Nothing Then
                oConn.Close()
                oConn.Dispose()
            End If
        End Try
    End Function
    Public Function EksekusiSQLTCetak(ByVal SQl As String) As Integer
        Dim oConn As System.Data.OleDb.OleDbConnection = Nothing
        Dim ocmd As System.Data.OleDb.OleDbCommand = Nothing
        Dim NoID As Long
        Try
            oConn = New System.Data.OleDb.OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" & Application.StartupPath & "\temp\tCetak.mdb" & ";User Id=admin;Password=;")
            ocmd = New System.Data.OleDb.OleDbCommand(SQl, oConn)
            oConn.Open()
            ocmd.CommandTimeout = oConn.ConnectionTimeout
            NoID = NullToLong(ocmd.ExecuteNonQuery)
            Return NoID
        Catch ex As Exception
            XtraMessageBox.Show("Gagal mengeksekusi SQL :" & SQl & vbCrLf & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
        Finally
            If Not ocmd Is Nothing Then
                ocmd.Dispose()
            End If
            If Not oConn Is Nothing Then
                oConn.Close()
                oConn.Dispose()
            End If
        End Try
    End Function
    Public Function CekSupplierJual(ByVal IDSupplier As Long, Optional ByVal Filternya As String = "") As Boolean
        Dim x As Boolean
        Dim dbs As New DataSet
        Dim rs As String
        Try
            rs = "SELECT * FROM MBeli WHERE IDWilayah=" & DefIDWilayah & " AND IDSupplier=" & IDSupplier
            If Filternya <> "" Then
                rs &= " AND " & Filternya
            End If
            dbs = ExecuteDataset("MBeli", rs)
            If dbs.Tables("MBeli").Rows.Count >= 1 Then
                x = True
            Else
                x = False
            End If
            dbs.Dispose()
        Catch ex As Exception
            x = False
        End Try
        Return x
    End Function
    Public Function CekCustomerBeli(ByVal IDCustomer As Long, Optional ByVal Filternya As String = "") As Boolean
        Dim x As Boolean
        Dim dbs As New DataSet
        Dim rs As String
        Try
            rs = "SELECT * FROM MJual WHERE IDWilayah=" & DefIDWilayah & " AND IDCustomer=" & IDCustomer
            If Filternya <> "" Then
                rs &= " AND " & Filternya
            End If
            dbs = ExecuteDataset("MJual", rs)
            If dbs.Tables("MJual").Rows.Count >= 1 Then
                x = True
            Else
                x = False
            End If
            dbs.Dispose()
        Catch ex As Exception
            x = False
        End Try
        Return x
    End Function
    Public Function CekKodeValid(ByVal Kode As String, ByVal KodeOld As String, ByVal NamaTabel As String, ByVal Field As String, ByVal IsEdit As Boolean, Optional ByVal Filternya As String = "") As Boolean
        Dim x As Boolean
        Dim dbs As New DataSet
        Dim rs As String
        Try
            If IsEdit Then
                rs = "SELECT " & Field & " FROM " & NamaTabel & _
                            " WHERE " & Field & "='" & Replace(Kode, "'", "''") & "' and " & Field & "<>'" & Replace(KodeOld, "'", "''") & "'" & " " & Filternya
            Else
                rs = "SELECT " & Field & " FROM " & NamaTabel & _
                             " WHERE " & Field & "='" & Replace(Kode, "'", "''") & "'" & " " & Filternya
            End If
            dbs = ExecuteDataset(NamaTabel, rs)
            If dbs.Tables(NamaTabel).Rows.Count >= 1 Then
                x = True
            Else
                x = False
            End If
        Catch ex As Exception
            x = False
        Finally
            dbs.Dispose()
        End Try
        Return x
    End Function
    Public Function CekKoneksi() As Boolean
        Dim oConn As SqlConnection = Nothing
        Dim hasil As Boolean = False
        Try
            oConn = New SqlConnection(StrKonSql)
            oConn.Open()
            hasil = True
        Catch ex As Exception
            XtraMessageBox.Show("Koneksi gagal!" & vbCrLf & "Pesan:" & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
        Finally
            If Not oConn Is Nothing Then
                oConn.Close()
                oConn.Dispose()
            End If
        End Try
        Return hasil
    End Function
    Public Function DeleteRowByID(ByVal NamaTabel As String, ByVal NamaKolom As String, ByVal NoID As Long) As Integer
        Dim hsl As Long
        Try
            Dim oConn As SqlConnection
            Dim ocmd As SqlCommand
            oConn = New SqlConnection(StrKonSql)
            ocmd = New SqlCommand("delete from " & NamaTabel & " where " & NamaKolom & " = " & NoID.ToString, oConn)
            oConn.Open()
            hsl = NullToLong(ocmd.ExecuteNonQuery)
            ocmd.Dispose()
            oConn.Close()
            oConn.Dispose()
            Return hsl
        Catch ex As Exception
            MsgBox("gagal mengeksekusi sql:" & "delete from " & NamaTabel & " where " & NamaKolom & " = " & NoID.ToString & vbCrLf & ex.Message, MsgBoxStyle.Information)

        End Try
    End Function

    Public Function IsLoginOk(ByVal Kode As String, ByVal Password As String) As Boolean
        Dim oConn As SqlConnection = Nothing
        Dim ocmd As SqlCommand = Nothing
        Dim odr As SqlDataReader
        Dim strsql As String
        Dim IsOk As Boolean = False
        Dim ds As New DataSet
        Try
            strsql = "Select muser.*, MGudang.IDWilayah, MWilayah.IDDepartemen from muser LEFT JOIN (MGudang LEFT JOIN MWilayah ON MWilayah.NoID=MGudang.IDWilayah) ON MGudang.NoID=MUser.IDGudangDefault where Muser.IsActive=1 AND Muser.kode='" & Kode & "' and Muser.pwd='" & Password & "'"
            oConn = New SqlConnection(StrKonSql)
            ocmd = New SqlCommand(strsql, oConn)
            oConn.Open()
            odr = ocmd.ExecuteReader
            If odr.Read Then
                IDUserAktif = NullToLong(odr("NoID"))
                'strsql = "SELECT * FROM TLogUser WHERE TanggalEnd IS NULL AND IDUser=" & IDUserAktif
                'ds = ExecuteDataset("MUser", strsql)
                'If ds.Tables(0).Rows.Count >= 1 AndAlso IPLokal.ToUpper <> NullToStr(ds.Tables(0).Rows(0).Item("IP")).ToUpper AndAlso XtraMessageBox.Show("User " & NullToStr(odr("Nama")).ToUpper & " dipakai di Computer " & NullToStr(ds.Tables(0).Rows(0).Item("IP")) & vbCrLf & "Ingin melanjutkan untuk masuk ke applikasi?", NamaAplikasi, MessageBoxButtons.YesNo, MessageBoxIcon.Warning) = DialogResult.No Then
                '    IDUserAktif = -1
                '    IsOk = False
                'Else
                DefIDDepartemen = NullToLong(odr("IDDepartemen"))
                DefIDWilayah = NullToLong(odr("IDWilayah"))
                DefIDGudang = NullToLong(odr("IDGudangDefault"))
                DefIDGudangSupplier = NullToLong(odr("IDGudangPenerimaanSupplier"))
                DefIDGudangCustomer = NullToLong(odr("IDGudangPenerimaanCustomer"))
                NamaUserAktif = NullToStr(odr("Nama"))
                KodeUserAktif = NullToStr(odr("Kode"))
                IsSupervisor = NullToBool(odr("IsSupervisor"))
                IsEditLayout = NullToBool(odr("IsEditLayout"))
                IsKasir = NullToBool(odr("IsKasir"))
                IsAutoPosting = NullToBool(odr("IsAutoPosting"))
                IsAccMutasi = NullToBool(odr("IsAccMutasi"))

                DefIDSatuan = NullToLong(odr("IDSatuan"))
                DefIDPegawai = NullToLong(odr("IDAlamat"))
                DefIDCustomer = NullToLong(odr("IDPelanggan"))
                DefIDSupplier = NullToLong(odr("IDSupplier"))
                DefTipeStock = NullToLong(odr("Tipe"))


                IsOk = True
                strsql = "INSERT INTO TLogUser (IDUser,IP,TanggalStart) VALUES (" & IDUserAktif & ",'" & FixApostropi(IPLokal) & "',Getdate())"
                EksekusiSQL(strsql)
                'End If
            Else
                IDUserAktif = -1
                DefIDDepartemen = -1
                DefIDWilayah = -1
                DefIDGudang = -1
                DefIDGudangCustomer = -1
                DefIDGudangSupplier = -1
                DefIDSatuan = -1
                DefIDPegawai = -1
                DefIDCustomer = -1
                DefIDSupplier = -1
                NamaUserAktif = ""
                KodeUserAktif = ""
                IsSupervisor = False
                IsSupervisor = False
                IsEditLayout = False
                IsKasir = False
                IsAutoPosting = False
                IsAccMutasi = False
            End If
        Catch ex As Exception
            IsOk = False
        Finally
            If Not ocmd Is Nothing Then
                ocmd.Dispose()
            End If
            If Not oConn Is Nothing Then
                oConn.Close()
                oConn.Dispose()
            End If
        End Try
        Return IsOk
    End Function
End Module
