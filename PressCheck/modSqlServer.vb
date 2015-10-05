Imports System.Data.SqlClient
Imports System.Data.OleDb
Imports DevExpress.XtraEditors
Imports DevExpress.XtraGrid
Imports DevExpress.Utils
Imports DevExpress.XtraGrid.Views
Imports DevExpress.XtraGrid.Views.Base
Imports Microsoft.VisualBasic
Module modSqlServer
    Public IDUserAktif As Long = -1
    Public NamaUserAktif As String
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

    Public DefIDSatuanfrmBarang As Long = -1

    Public IsSupervisor As Boolean = False
    Public IsEditLayout As Boolean = False
    Public IsKasir As Boolean = False
    Public IsAutoPosting As Boolean = False
    Public IsAccMutasi As Boolean = False
    'Public DSPublic As New DataSet
    'Public DSPublicKodeBarang As New DataSet
    Public Enum DefTipeStock_
        Penuh = 0
        BongkarBarang = 1
        LihatStock = 2
    End Enum
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
    Public Function KirimDataKassa(ByVal IDKassa As Integer) As Boolean 'All Kassa=0/-1
        Dim cnOleDB As OleDbConnection = Nothing
        Dim cmOleDB As OleDbCommand = Nothing
        Dim SQL As String = ""
        Dim ds As New DataSet
        Dim dlg As DevExpress.Utils.WaitDialogForm = Nothing
        Dim FileSource As String = ""
        Dim FileDestination As String = ""
        Dim hasil As Boolean = False
        Try
            If IsSupervisor Then
                If XtraMessageBox.Show("Ingin mengirim data master ke Kassa " & IIf(IDKassa >= 1, IDKassa.ToString, "") & " ?" & vbCrLf & " Akan memerlukan sedikit waktu.", NamaAplikasi, MessageBoxButtons.YesNo, MessageBoxIcon.Information, MessageBoxDefaultButton.Button2) = Windows.Forms.DialogResult.Yes Then
                    If System.IO.File.Exists(Application.StartupPath & "\System\engine\sysdbmaster.mdb") Then
                        Dim curentcursor As Cursor = Windows.Forms.Cursor.Current
                        Windows.Forms.Cursor.Current = Cursors.WaitCursor
                        dlg = New DevExpress.Utils.WaitDialogForm(String.Format("Sedang merefresh data.{0}MOHON TUNGGU ...", vbCrLf), Application.ProductName)
                        dlg.TopMost = False
                        dlg.Show()
                        Windows.Forms.Cursor.Current = Cursors.WaitCursor
                        IO.File.Copy(Application.StartupPath & "\System\engine\sysdbmaster.mdb", Application.StartupPath & "\System\engine\dbmaster.mdb", True)
                        'Provider=Microsoft.Jet.OLEDB.4.0;Data Source==D:\PROJECT\CITYTOYS\CitiToys\bin\Debug\System\engine\sysdbmaster.mdb
                        SQL = "SELECT MBarang.TglDariDiskon,MBarang.TglSampaiDiskon, MBarangD.*, MBarang.Kode AS KodeBarang, MBarang.Nama AS NamaBarang, MSatuan.Kode AS Satuan, MBarang.HargaBeliPcs as  HargaPokok, MBarang.IsFamilyGroup, MBarang.HargaFamily, MBarang.TanggalDariFamily, MBarang.TanggalSampaiFamily, MBarang.IsPoin, 0 AS IsOperator, CASE WHEN IsNull(MBarang.IDPoinSupplier,0)>=1 THEN 1 ELSE 0 END AS IsPoinSupplier, MBarang.IDPoinSupplier, MBarang.BKP, MBarang.IsGroupQty " & vbCrLf & _
                              " FROM (MBarangD INNER JOIN MBarang ON MBarang.NoID=MBarangD.IDBarang) INNER JOIN MSatuan ON MSatuan.NoID=MBarangD.IDSatuan  " & vbCrLf & _
                              " WHERE IsNull(MBarang.Kode,'')<>'' AND IsNull(MBarang.Nama,'')<>'' AND IsNull(MBarangD.Barcode,'')<>'' AND MBarangD.IsActive=1 AND MBarang.IsActive=1 AND MBarangD.IsJualPOS=1"
                        ds = ExecuteDataset("MBarang", SQL)
                        With ds.Tables("MBarang")
                            If .Rows.Count >= 1 Then
                                cnOleDB = New OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" & Application.StartupPath & "\\System\\engine\\dbmaster.mdb")
                                cnOleDB.Open()
                                cmOleDB = New OleDbCommand("DELETE FROM TInv", cnOleDB)
                                cmOleDB.CommandText = "DELETE FROM TInv"
                                cmOleDB.ExecuteNonQuery()
                                For i As Integer = 0 To .Rows.Count - 1
                                    SQL = "INSERT INTO TInv ( NoID, IDInventor, Kode, Barcode, Nama, IDSatuan, KodeSat, Konversi, HargaJual, " & vbCrLf & _
                                          "  HargaPokok, DiscExpired, DiscMulai, DiscProsen, DiscRupiah, IsPoin, IsOperator, IsPoinSupplier, IDPoinSupplier, " & _
                                          " BKP, HargaMin, IsKelipatan, QtyKelipatan, HargaKelipatan, Qty1, Harga1, Qty2, Harga2, Qty3, Harga3, " & vbCrLf & _
                                          " IsAllowDisc, IsFamilyGroup, QtyFamily, HargaFamily, TanggalDariFamily, TanggalSampaiFamily, IsGroupQty) VALUES (" & vbCrLf & _
                                          NullToLong(.Rows(i).Item("NoID")) & "," & vbCrLf & _
                                          NullToLong(.Rows(i).Item("IDBarang")) & "," & vbCrLf & _
                                          "'" & FixApostropi(NullToStr(.Rows(i).Item("KodeBarang"))) & "'," & vbCrLf & _
                                          "'" & FixApostropi(NullToStr(.Rows(i).Item("Barcode"))) & "'," & vbCrLf & _
                                          "'" & FixApostropi(NullToStr(.Rows(i).Item("NamaBarang"))) & "'," & vbCrLf & _
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
                                          FixKoma(NullToDbl(.Rows(i).Item("Harga1"))) & "," & vbCrLf & _
                                          FixKoma(NullToDbl(.Rows(i).Item("Qty2"))) & "," & vbCrLf & _
                                          FixKoma(NullToDbl(.Rows(i).Item("Harga2"))) & "," & vbCrLf & _
                                          FixKoma(NullToDbl(.Rows(i).Item("Qty3"))) & "," & vbCrLf & _
                                          FixKoma(NullToDbl(.Rows(i).Item("Harga3"))) & "," & vbCrLf & _
                                          IIf(NullToBool(.Rows(i).Item("IsAllowDisc")), 1, 0) & "," & vbCrLf & _
                                          IIf(NullToBool(.Rows(i).Item("IsFamilyGroup")), 1, 0) & "," & vbCrLf & _
                                          FixKoma(NullToDbl(0)) & "," & vbCrLf & _
                                          FixKoma(NullToDbl(.Rows(i).Item("HargaFamily"))) & "," & vbCrLf & _
                                          "#" & NullToDate(.Rows(i).Item("TanggalDariFamily")).ToString("yyyy/MM/dd") & "#, " & vbCrLf & _
                                          "#" & NullToDate(.Rows(i).Item("TanggalSampaiFamily")).ToString("yyyy/MM/dd") & "#, " & vbCrLf & _
                                          IIf(NullToBool(.Rows(i).Item("IsGroupQty")), 1, 0) & " " & vbCrLf & _
                                          ")"

                                    cmOleDB.CommandText = SQL
                                    cmOleDB.ExecuteNonQuery()
                                Next
                            End If
                        End With

                        'Provider=Microsoft.Jet.OLEDB.4.0;Data Source==D:\PROJECT\CITYTOYS\CitiToys\bin\Debug\System\engine\sysdbmaster.mdb
                        SQL = "SELECT MUser.*, MAlamat.Kode AS KodeEmp, MAlamat.Nama AS NamaEmp, MAlamat.IsKasir AS IsKasirPeg, MAlamat.IsOperator AS IsOperatorPeg, MAlamat.IsOperator1 AS IsOperator1Peg " & vbCrLf & _
                              " FROM MUser INNER JOIN MAlamat ON MAlamat.NoID=MUser.IDAlamat " & vbCrLf & _
                              " WHERE MAlamat.IsActive=1 AND MAlamat.IsPegawai=1 "
                        ds = ExecuteDataset("MUser", SQL)
                        With ds.Tables("MUser")
                            If .Rows.Count >= 1 Then
                                cmOleDB.CommandText = "DELETE FROM MEmp"
                                cmOleDB.ExecuteNonQuery()
                                For i As Integer = 0 To .Rows.Count - 1
                                    SQL = "INSERT INTO MEMP ([NoID],[Kode],[Nama],[Nick],[Password],[IsKasir],[IsPengawas],[IsOperator],[IsOperator1],[IsPengawasUtama]) VALUES (" & vbCrLf & _
                                          NullToLong(.Rows(i).Item("NoID")) & "," & vbCrLf & _
                                          "'" & FixApostropi(NullToStr(.Rows(i).Item("Kode"))) & "'," & vbCrLf & _
                                          "'" & FixApostropi(NullToStr(.Rows(i).Item("NamaEmp"))) & "'," & vbCrLf & _
                                          "'" & FixApostropi(NullToStr(.Rows(i).Item("Nama"))) & "'," & vbCrLf & _
                                          "'" & FixApostropi(DecryptText(NullToStr(.Rows(i).Item("Pwd")), "vpoint")) & "'," & vbCrLf & _
                                          NullTolInt(.Rows(i).Item("IsKasirPeg")) & "," & vbCrLf & _
                                          NullTolInt(.Rows(i).Item("IsPengawasKasir")) & "," & vbCrLf & _
                                          NullTolInt(.Rows(i).Item("IsOperatorPeg")) & "," & vbCrLf & _
                                          NullTolInt(.Rows(i).Item("IsOperator1Peg")) & "," & vbCrLf & _
                                          NullTolInt(.Rows(i).Item("IsPengawasUtamaKasir")) & ")"

                                    cmOleDB.CommandText = SQL
                                    cmOleDB.ExecuteNonQuery()
                                Next
                            End If
                        End With

                        'Provider=Microsoft.Jet.OLEDB.4.0;Data Source==D:\PROJECT\CITYTOYS\CitiToys\bin\Debug\System\engine\sysdbmaster.mdb
                        SQL = "SELECT MAlamat.* FROM MAlamat" & vbCrLf & _
                              " WHERE MAlamat.IsActive = 1 And MAlamat.IsCustomer = 1"
                        ds = ExecuteDataset("MCust", SQL)
                        With ds.Tables("MCust")
                            If .Rows.Count >= 1 Then
                                cmOleDB.CommandText = "DELETE FROM MCustomer"
                                cmOleDB.ExecuteNonQuery()
                                For i As Integer = 0 To .Rows.Count - 1
                                    SQL = "INSERT INTO MCustomer ([NoID],[Kode],[Barcode],[Nama],[DOJ]) VALUES (" & vbCrLf & _
                                          NullToLong(.Rows(i).Item("NoID")) & "," & vbCrLf & _
                                          "'" & FixApostropi(NullToStr(.Rows(i).Item("Kode"))) & "'," & vbCrLf & _
                                          NullToLong(.Rows(i).Item("NoID")) & "," & vbCrLf & _
                                          "'" & FixApostropi(NullToStr(.Rows(i).Item("Nama"))) & "'," & vbCrLf & _
                                          "#" & NullToDate(.Rows(i).Item("DOJ")).ToString("yyyy/MM/dd") & "#)"

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
                                    SQL = "INSERT INTO MPoinSupplier ([NoID],[Kode],[Nama],[Nilai1Poin]) VALUES (" & vbCrLf & _
                                          NullToLong(.Rows(i).Item("NoID")) & "," & vbCrLf & _
                                          "'" & FixApostropi(NullToStr(.Rows(i).Item("Kode"))) & "'," & vbCrLf & _
                                          "'" & FixApostropi(NullToStr(.Rows(i).Item("Nama"))) & "'," & vbCrLf & _
                                          FixKoma(NullToDbl(.Rows(i).Item("Nilai1Poin"))) & ")"

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
                                    SQL = "INSERT INTO MSettingPoin ([NoID],[NilaiBelanja1Poin],[MinimumBelanjaDapatPoin],[NilaiDiskon],[CreditCardDapatDiskon]) VALUES (" & vbCrLf & _
                                          NullToLong(.Rows(i).Item("NoID")) & "," & vbCrLf & _
                                          FixKoma(NullToDbl(.Rows(i).Item("NilaiBelanja1Poin"))) & "," & vbCrLf & _
                                          FixKoma(NullToDbl(.Rows(i).Item("MinimumBelanjaDapatDiskon"))) & "," & vbCrLf & _
                                          FixKoma(NullToDbl(.Rows(i).Item("NilaiDiskon"))) & "," & vbCrLf & _
                                          IIf(NullToBool(.Rows(i).Item("CreditCardDapatDiskon")), 1, 0) & ")"

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

                        'Copy Database ke Kassa
                        If IDKassa >= 1 Then
                            SQL = "SELECT MPOS.* FROM MPOS WHERE MPOS.IsActive=1 AND MPOS.NoID=" & IDKassa
                        Else
                            SQL = "SELECT MPOS.* FROM MPOS WHERE MPOS.IsActive=1"
                        End If
                        ds = ExecuteDataset("MPOS", SQL)
                        For i As Integer = 0 To ds.Tables("MPOS").Rows.Count - 1
                            If System.IO.Directory.Exists(NullToStr(ds.Tables("MPOS").Rows(i).Item("PathDbTemp"))) Then
                                Try
                                    FileSource = Application.StartupPath & "\System\engine\dbmaster.mdb"
                                    FileDestination = NullToStr(ds.Tables("MPOS").Rows(i).Item("PathDbTemp")) & "\Update\DBMaster.mdb"
                                    If System.IO.File.Exists(NullToStr(ds.Tables("MPOS").Rows(i).Item("PathDbTemp")) & "\Update\temp.txt") Then
                                        System.IO.File.Delete(NullToStr(ds.Tables("MPOS").Rows(i).Item("PathDbTemp")) & "\Update\temp.txt")
                                    End If
                                    If System.IO.File.Exists(FileDestination) Then
                                        System.IO.File.Delete(FileDestination)
                                    End If
                                    System.IO.File.Copy(FileSource, FileDestination)
                                    Ini.TulisIniPath(Application.StartupPath & "\system\engine\temp.txt", "Database", "TglUpdate", TanggalSystem.ToString("dd-MM-yyyy HH:mm"))
                                    System.IO.File.Copy(Application.StartupPath & "\system\engine\temp.txt", NullToStr(ds.Tables("MPOS").Rows(i).Item("PathDbTemp")) & "\Update\temp.txt", True)
                                    'Try
                                    '    FileDestination = NullToStr(ds.Tables("MPOS").Rows(i).Item("PathDbTemp")) & "\Database\DBMaster.mdb"
                                    '    System.IO.File.Copy(FileSource, FileDestination, True)
                                    'Catch ex As Exception

                                    'End Try
                                    EksekusiSQL("Update MPOS SET DatabaseUpdate=getdate() WHERE NoID=" & NullToLong(ds.Tables("MPOS").Rows(i).Item("NoID")))
                                Catch ex As Exception
                                    XtraMessageBox.Show("Info : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK)
                                End Try
                            End If
                        Next
                        hasil = True
                    Else
                        XtraMessageBox.Show("File system tidak ditemukan.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Warning)
                        hasil = False
                    End If
                End If
            End If
        Catch ex As Exception
            XtraMessageBox.Show("Gagal mengeksekusi SQL:" & SQL & vbCrLf & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
            hasil = False
        Finally
            Windows.Forms.Cursor.Current = Cursors.Default
            If Not cnOleDB Is Nothing Then
                cmOleDB.Dispose()
                cnOleDB.Close()
                cnOleDB.Dispose()
            End If
            If Not dlg Is Nothing Then
                dlg.Close()
                dlg.Dispose()
            End If
        End Try
        Return hasil
    End Function
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
                IO.File.Copy(Application.StartupPath & "\System\engine\defData.csv", Application.StartupPath & "\System\engine\DataBaru.csv", True)
                
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
    Public Function isDatabaseConnected() As Boolean
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
        Dim hasil As String = ""
        Try
            oConn = New SqlConnection(StrKonSql)
            ocmd = New SqlCommand(strsql, oConn)
            oConn.Open()
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

                FolderLayouts = NullToStr(EksekusiSQlSkalarNew("select PathLayouts from msetting"))
                If Not System.IO.Directory.Exists(FolderLayouts) Then
                    FolderLayouts = Application.StartupPath & "\System\Layouts\"
                End If
                FolderFoto = NullToStr(EksekusiSQlSkalarNew("select PathImage from msetting"))
                If Not System.IO.Directory.Exists(FolderLayouts) Then
                    FolderFoto = Application.StartupPath & "\System\PathFoto\"
                End If
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
