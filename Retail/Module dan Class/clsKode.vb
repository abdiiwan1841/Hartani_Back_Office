Imports System.Data
Imports System.Data.SqlClient
Imports DevExpress.XtraEditors

Public Class clsKode
    Public Enum StrKode
        PO = 0
        Beli = 1
        ReturBeli = 2
        SO = 3
        Jual = 4
        ReturJual = 5
    End Enum
    Public Enum IsSaldo
        none = 0
        termasuk = 1
        tidaktermasuk = 2
    End Enum
    Private Shared Function fnRomawi(ByVal input As Integer) As String
        Select Case input
            Case 1
                Return "I"
            Case 2
                Return "II"
            Case 3
                Return "III"
            Case 4
                Return "IV"
            Case 5
                Return "V"
            Case 6
                Return "VI"
            Case 7
                Return "VII"
            Case 8
                Return "VIII"
            Case 9
                Return "IX"
            Case 10
                Return "X"
            Case 11
                Return "XI"
            Case 12
                Return "XII"
            Case Else
                Return "XX"
        End Select
    End Function
    Public Shared Function NotaDiPosting(ByVal NamaTabel As String, ByVal NoID As Long) As Boolean
        Dim SQL As String = ""
        Dim hasil As Boolean = False
        Try
            SQL = "SELECT IsPosted FROM " & NamaTabel & " WHERE NoID=" & NoID
            hasil = NullToBool(EksekusiSQlSkalarNew(SQL))
        Catch ex As Exception
            XtraMessageBox.Show("Info : " & ex.Message & vbCrLf & SQL, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
        End Try
        Return hasil
    End Function
    Private Shared Sub getComponentKode(ByVal KodeSetting As StrKode, ByRef Digit As String, ByRef KodeDepan As String, _
    ByRef IsBarang As Boolean, ByRef IsGudang As Boolean, ByRef IsWilayah As Boolean, ByRef IsCabang As Boolean, ByRef IsAlamat As Boolean, ByRef IsMonth As Boolean, ByRef IsYear As Boolean)
        Dim oDS As New DataSet
        Try
            oDS = ExecuteDataset(NamaAplikasi, "SELECT * FROM MSettingKode WHERE upper(Kode)='" & FixApostropi(KodeSetting.ToString).ToUpper & "'")
            If oDS.Tables(0).Rows.Count >= 1 Then
                KodeDepan = NullTostr(oDS.Tables(0).Rows(0).Item("Prefix"))
                IsBarang = NullTobool(oDS.Tables(0).Rows(0).Item("IsBarang"))
                IsGudang = NullTobool(oDS.Tables(0).Rows(0).Item("IsGudang"))
                IsWilayah = NullTobool(oDS.Tables(0).Rows(0).Item("IsWilayah"))
                IsCabang = NullTobool(oDS.Tables(0).Rows(0).Item("IsCabang"))
                IsAlamat = NullTobool(oDS.Tables(0).Rows(0).Item("IsAlamat"))
                IsMonth = NullTobool(oDS.Tables(0).Rows(0).Item("IsMonth"))
                IsYear = NullTobool(oDS.Tables(0).Rows(0).Item("IsYear"))
                For i As Integer = 1 To CInt(NullToDbl(oDS.Tables(0).Rows(0).Item("Digit")))
                    Digit = Digit & "X"
                Next
            Else
                KodeDepan = KodeDepan
                IsBarang = False
                IsGudang = False
                IsWilayah = False
                IsCabang = False
                IsAlamat = False
                IsMonth = True
                IsYear = True
                Digit = "XXXX"
            End If
        Catch ex As Exception
        Finally
            oDS.Dispose()
        End Try
    End Sub
    Public Shared Function GetKode(ByVal NamaTabel As String, ByVal KodeSetting As StrKode, ByVal dt As Date, Optional ByVal IDBarang As Long = -1, Optional ByVal IDAlamat As Long = -1, Optional ByVal IDGudang As Long = -1, Optional ByVal IDWilayah As Long = -1, Optional ByVal IDCabang As Long = -1, Optional ByVal IsSaldoAwal As IsSaldo = IsSaldo.none, Optional ByVal FilterTambahan As String = "") As String
        Dim SQL As String = "", oDS As New DataSet
        Dim KodeAlamat As String = ""
        Dim KodeGudang As String = ""
        Dim KodeWilayah As String = ""
        Dim KodeCabang As String = ""
        Dim KodeBarang As String = ""

        Dim IsGudang As Boolean = False
        Dim IsWilayah As Boolean = False
        Dim IsCabang As Boolean = False
        Dim IsAlamat As Boolean = False
        Dim IsBarang As Boolean = False
        Dim IsMonth As Boolean = False
        Dim IsYear As Boolean = False

        Dim KodeDepan As String = ""
        Dim Digit As String = ""
        Dim x As String = ""
        Dim KodeA As Long = 1
        Dim Filter As String = ""
        Try
            getComponentKode(KodeSetting, Digit, KodeDepan, IsBarang, IsGudang, IsWilayah, IsCabang, IsAlamat, IsMonth, IsYear)
            If IsBarang Then
                SQL = "SELECT Kode FROM MBarang WHERE NoID=" & IDBarang
                KodeBarang = NullTostr(EksekusiSQlSkalarNew(SQL))
            End If
            If IsAlamat Then
                SQL = "SELECT Kode FROM MAlamat WHERE NoID=" & IDAlamat
                KodeAlamat = NullTostr(EksekusiSQlSkalarNew(SQL))
            End If
            If IsGudang Then
                SQL = "SELECT Kode FROM MGudang WHERE NoID=" & IDGudang
                KodeGudang = NullTostr(EksekusiSQlSkalarNew(SQL))
            End If
            If IsWilayah Then
                SQL = "SELECT Kode FROM MWilayah WHERE NoID=" & IDWilayah
                KodeWilayah = NullTostr(EksekusiSQlSkalarNew(SQL))
            End If
            If IsCabang Then
                SQL = "SELECT Kode FROM MDepartemen WHERE NoID=" & IDCabang
                KodeCabang = NullTostr(EksekusiSQlSkalarNew(SQL))
            End If

            Filter = " 1=1 "
            Select Case NamaTabel
                Case "MPO".ToUpper, "MBeli".ToUpper, "MReturBeli".ToUpper
                    Filter &= " AND " & CStr(IIf(IsAlamat, NamaTabel & ".IDSupplier=" & IDAlamat, " 1=1 "))
                Case "MSO".ToUpper, "MJual".ToUpper, "MReturJual".ToUpper
                    Filter &= " AND " & CStr(IIf(IsAlamat, NamaTabel & ".IDCustomer=" & IDAlamat, " 1=1 "))
                Case Else
                    Filter &= " AND " & CStr(IIf(IsAlamat, NamaTabel & ".IDAlamat=" & IDAlamat, " 1=1 "))
            End Select

            'Cabang
            Filter &= " AND " & CStr(IIf(IsCabang, NamaTabel & ".IDDepartemen=" & IDCabang, " 1=1 "))

            'Wilayah
            Filter &= " AND " & CStr(IIf(IsWilayah, NamaTabel & ".IDWilayah=" & IDWilayah, " 1=1 "))

            'Gudang
            Filter &= " AND " & CStr(IIf(IsGudang, NamaTabel & ".IDGudang=" & IDGudang, " 1=1 "))

            Filter &= " AND " & CStr(IIf(IsMonth, " MONTH(" & NamaTabel & ".Tanggal)=" & Format(dt, "MM"), " 1=1 "))
            Filter &= " AND " & CStr(IIf(IsYear, " YEAR(" & NamaTabel & ".Tanggal)=" & Format(dt, "yyyy"), " 1=1 "))

            If IsSaldoAwal = IsSaldo.termasuk Then
                Filter &= " AND " & NamaTabel & ".IsSaldoAwal=1"
            ElseIf IsSaldoAwal = IsSaldo.tidaktermasuk Then
                Filter &= " AND (" & NamaTabel & ".IsSaldoAwal=0 OR " & NamaTabel & ".IsSaldoAwal IS NULL)"
            End If

            If FilterTambahan.Trim <> "" Then
                Filter &= " AND " & FilterTambahan
            End If

            KodeA = NullTolong(GetNewKodeTablebyFilter(NamaTabel, "Kode", Filter, Len(KodeDepan) + 1, Digit.Length))
            If IsSaldoAwal = IsSaldo.termasuk Then
                x = "SA"
            Else
                x = KodeDepan
            End If
            x &= Format(KodeA, "0" & Replace(Digit.Substring(1), "X", "#"))

            If IsBarang AndAlso KodeBarang.Trim <> "" Then
                x &= "/" & KodeBarang
            End If
            If IsAlamat AndAlso KodeAlamat.Trim <> "" Then
                x &= "/" & KodeAlamat
            End If
            If IsGudang AndAlso KodeGudang.Trim <> "" Then
                x &= "/" & KodeGudang
            End If
            If IsWilayah AndAlso KodeWilayah.Trim <> "" Then
                x &= "/" & KodeWilayah
            End If
            If IsCabang AndAlso KodeCabang.Trim <> "" Then
                x &= "/" & KodeCabang
            End If

            If IsMonth And IsYear Then
                x &= "/" & Format(dt, "MM/yy")
            ElseIf IsMonth = False And IsYear Then
                x &= "/" & Format(dt, "yyyy")
            ElseIf IsMonth And IsYear = False Then
                x &= "/" & Format(dt, "MM")
            End If

        Catch ex As Exception
            XtraMessageBox.Show("Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
        Return x
    End Function
    Public Shared Function GetNewKodeTablebyFilter(ByVal NamaTabel As String, ByVal field As String, ByVal Filter As String, ByVal iStart As Integer, ByVal iLeght As Integer) As Object
        Dim oDs As New DataSet
        Dim StrCekRecord As String, x As Object
        Try
            Select Case NamaTabel
                Case "MJurnal", "MKasIn", "MKasOut"
                    StrCekRecord = "select max(cast((CASE WHEN IsNumeric(Substring(" & NamaTabel & "." & field & "," & iStart & "," & iLeght & "))=1 THEN Substring(" & NamaTabel & "." & field & "," & iStart & "," & iLeght & ") ELSE 1 END) as int)) As idMax from " & NamaTabel & "  WHERE isnumeric(Substring(" & NamaTabel & "." & field & "," & iStart & "," & iLeght & "))=1 AND  " & Filter
                Case Else
                    StrCekRecord = "select " & vbCrLf
                    StrCekRecord = StrCekRecord & " max(" & vbCrLf
                    StrCekRecord = StrCekRecord & " cast(" & vbCrLf
                    StrCekRecord = StrCekRecord & " (CASE WHEN IsNumeric(Substring(" & NamaTabel & "." & field & "," & iStart & "," & iLeght & "))=1 THEN Substring(" & NamaTabel & "." & field & "," & iStart & "," & iLeght & ") ELSE 1 END" & vbCrLf
                    StrCekRecord = StrCekRecord & " ) as int)) As idMax from " & NamaTabel & " WHERE " & Filter & vbCrLf
            End Select

            oDs = ExecuteDataset(NamaTabel, StrCekRecord)
            If oDs.Tables(NamaTabel).Rows.Count = 0 Then
                x = 1
            Else
                x = NullTolong(oDs.Tables(NamaTabel).Rows(0).Item(0)) + 1
            End If
        Catch ex As Exception
            XtraMessageBox.Show("Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
            x = 1
        Finally
            oDs.Dispose()
        End Try
        Return x
    End Function

    Public Shared Function MintaKodeBaru(ByVal KodeDepan As String, ByVal Tabel As String, ByVal Tanggal As Date, Optional ByVal IDWilayah As Long = -1, Optional ByVal Digit As Integer = 5, Optional ByVal FilterTambahanTidakPakaiAND As String = "")
        Dim TmpKode As String = ""
        Dim TmpKodeDepan As String = KodeDepan
        Dim xFormat As String = ""
        Dim SQL As String = ""
        Dim ds As New DataSet
        Try
            SQL = " 1=1 "
            If Tabel.ToLower = "MRequestMutasiWilayah".ToLower Then
                SQL &= " AND " & Tabel & ".IDWilayahUntuk=" & IDWilayah
            ElseIf Tabel.ToLower = "MSPKMutasiWilayah".ToLower Or Tabel.ToLower = "MPackingMutasiWilayah".ToLower Then
                SQL &= " AND " & Tabel & ".IDWilayahDari=" & IDWilayah
            ElseIf Tabel.ToLower = "MSaldoAwalHutangPiutang".ToLower Or Tabel.ToLower = "MTransferKode".ToLower Or Tabel.ToLower = "MTransferOut".ToLower Or Tabel.ToLower = "MTransferIN".ToLower Or Tabel.ToLower = "MSaldoAwalPersediaan".ToLower Then
                SQL &= ""
            Else
                SQL &= " AND " & Tabel & ".IDWilayah=" & IDWilayah
            End If
            SQL &= " AND MONTH(" & Tabel & ".Tanggal)=" & Format(Tanggal, "MM")
            SQL &= " AND YEAR(" & Tabel & ".Tanggal)=" & Format(Tanggal, "yyyy")
            If FilterTambahanTidakPakaiAND <> "" Then
                SQL &= " AND " & FilterTambahanTidakPakaiAND
            End If
            If Tabel.ToLower = "MTransferOut".ToLower Or Tabel.ToLower = "MTransferOut".ToLower Then
                TmpKodeDepan = TmpKodeDepan
            Else
                TmpKodeDepan = TmpKodeDepan & NullTostr(EksekusiSQlSkalarNew("SELECT Pengkodean FROM MWilayah WHERE NoID=" & IDWilayah))
            End If
            TmpKodeDepan = TmpKodeDepan & "/" & Tanggal.ToString("yyyy") & "/" & fnRomawi(CInt(Tanggal.ToString("MM"))) & "/"
            TmpKode = NullTolong(GetNewKodeTablebyFilter(Tabel, "Kode", SQL, Len(TmpKodeDepan) + 1, Digit))
            xFormat = ""
            For i As Integer = 1 To Digit
                If i = 1 Then
                    xFormat = "0"
                Else
                    xFormat &= "#"
                End If
            Next
            TmpKode = TmpKodeDepan & Format(CDbl(TmpKode), xFormat)
        Catch ex As Exception
            XtraMessageBox.Show("Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            If Not ds Is Nothing Then
                ds.Dispose()
            End If
        End Try
        Return TmpKode
    End Function

    Public Shared Function MintaKodeBaruFP(ByVal Tabel As String, ByVal NamaKolom As String, ByVal MassaPajak As Date, Optional ByVal StartDigit As Integer = 8, Optional ByVal Digit As Integer = 8, Optional ByVal FilterTambahanTidakPakaiAND As String = "")
        Dim TmpKode As String = ""
        'Dim TmpKodeDepan As String = ""
        Dim xFormat As String = ""
        Dim SQL As String = ""
        Dim ds As New DataSet
        Try
            SQL = " 1=1 "
            SQL &= " AND YEAR(" & Tabel & ".Tanggal)=" & Format(MassaPajak, "yyyy")
            If FilterTambahanTidakPakaiAND <> "" Then
                SQL &= " AND " & FilterTambahanTidakPakaiAND
            End If
            TmpKode = NullToLong(GetNewKodeTablebyFilter(Tabel, NamaKolom, SQL, StartDigit, Digit))
            xFormat = ""
            For i As Integer = 1 To Digit
                If i = 1 Then
                    xFormat = "0"
                Else
                    xFormat &= "#"
                End If
            Next
            TmpKode = Format(CDbl(TmpKode), xFormat)
        Catch ex As Exception
            XtraMessageBox.Show("Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            If Not ds Is Nothing Then
                ds.Dispose()
            End If
        End Try
        Return TmpKode
    End Function

    Public Shared Function MintaKodeBaruPajakKeluaran(ByVal Tabel As String, ByVal NamaKolom As String, ByVal Tanggal As Date, Optional ByVal StartDigit As Integer = 1, Optional ByVal Digit As Integer = 5, Optional ByVal FilterTambahanTidakPakaiAND As String = "")
        Dim TmpKode As String = ""
        'Dim TmpKodeDepan As String = ""
        Dim xFormat As String = ""
        Dim SQL As String = ""
        Dim ds As New DataSet
        Try
            SQL = " 1=1 "
            SQL &= " AND YEAR(" & Tabel & ".Tanggal)=" & Format(Tanggal, "yyyy")
            If FilterTambahanTidakPakaiAND <> "" Then
                SQL &= " AND " & FilterTambahanTidakPakaiAND
            End If
            TmpKode = NullToLong(GetNewKodeTablebyFilter(Tabel, NamaKolom, SQL, StartDigit, Digit))
            xFormat = ""
            For i As Integer = 1 To Digit
                If i = 1 Then
                    xFormat = "0"
                Else
                    xFormat &= "#"
                End If
            Next
            TmpKode = Format(CDbl(TmpKode), xFormat) & "/" & fnRomawi(Tanggal.Month) & "/" & Tanggal.ToString("yy") & "/P"
        Catch ex As Exception
            XtraMessageBox.Show("Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            If Not ds Is Nothing Then
                ds.Dispose()
            End If
        End Try
        Return TmpKode
    End Function

    Public Shared Function MintaKodeSPPBaru(ByVal KodeDepan As String, ByVal Tabel As String, ByVal Tanggal As Date, Optional ByVal IDWilayah As Long = -1, Optional ByVal Digit As Integer = 5, Optional ByVal FilterTambahanTidakPakaiAND As String = "", Optional ByVal PakaiTambahanKode As Boolean = True)
        Dim TmpKode As String = ""
        Dim TmpKodeDepan As String = KodeDepan
        Dim xFormat As String = ""
        Dim SQL As String = ""
        Dim ds As New DataSet
        Try
            SQL = " 1=1 "
            If Tabel.ToLower = "MRequestMutasiWilayah".ToLower Then
                SQL &= " AND " & Tabel & ".IDWilayahUntuk=" & IDWilayah
            ElseIf Tabel.ToLower = "MSPKMutasiWilayah".ToLower Or Tabel.ToLower = "MPackingMutasiWilayah".ToLower Then
                SQL &= " AND " & Tabel & ".IDWilayahDari=" & IDWilayah
            ElseIf Tabel.ToLower = "MTransferKode".ToLower Or Tabel.ToLower = "MTransferOut".ToLower Or Tabel.ToLower = "MTransferIN".ToLower Or Tabel.ToLower = "MSaldoAwalPersediaan".ToLower Then
                SQL &= ""
            Else
                SQL &= " AND " & Tabel & ".IDWilayah=" & IDWilayah
            End If
            SQL &= " AND YEAR(" & Tabel & ".Tanggal)=" & Format(Tanggal, "yyyy")
            If FilterTambahanTidakPakaiAND <> "" Then
                SQL &= " AND " & FilterTambahanTidakPakaiAND
            End If
            If Tabel.ToLower = "MTransferOut".ToLower Or Tabel.ToLower = "MTransferOut".ToLower Then
                TmpKodeDepan = TmpKodeDepan
            Else
                If PakaiTambahanKode Then
                    TmpKodeDepan = TmpKodeDepan & NullToStr(EksekusiSQlSkalarNew("SELECT Pengkodean FROM MWilayah WHERE NoID=" & IDWilayah))
                Else
                    TmpKodeDepan = TmpKodeDepan
                End If

            End If
            TmpKodeDepan = TmpKodeDepan & "/" & Tanggal.ToString("yyyy") & "/"
            TmpKode = NullToLong(GetNewKodeTablebyFilter(Tabel, "Kode", SQL, Len(TmpKodeDepan) + 1, Digit))
            xFormat = ""
            For i As Integer = 1 To Digit
                If i = 1 Then
                    xFormat = "0"
                Else
                    xFormat &= "#"
                End If
            Next
            TmpKode = TmpKodeDepan & Format(CDbl(TmpKode), xFormat)
        Catch ex As Exception
            XtraMessageBox.Show("Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            If Not ds Is Nothing Then
                ds.Dispose()
            End If
        End Try
        Return TmpKode
    End Function

    Public Shared Function MintaKodeReturBeliBaru(ByVal Tabel As String, ByVal Tanggal As Date, ByVal IDSupplier As Long, Optional ByVal IDWilayah As Long = -1, Optional ByVal Digit As Integer = 5, Optional ByVal FilterTambahanTidakPakaiAND As String = "", Optional ByVal PakaiTambahanKode As Boolean = True)
        Dim TmpKode As String = ""
        'Dim TmpKodeDepan As String = KodeDepan
        Dim xFormat As String = ""
        Dim SQL As String = ""
        Dim ds As New DataSet
        Try
            SQL = " 1=1 "
            SQL &= " AND " & Tabel & ".IDWilayah=" & IDWilayah
            SQL &= " AND YEAR(" & Tabel & ".Tanggal)=" & Format(Tanggal, "yyyy")
            If FilterTambahanTidakPakaiAND <> "" Then
                SQL &= " AND " & FilterTambahanTidakPakaiAND
            End If
            TmpKode = NullToLong(GetNewKodeTablebyFilter(Tabel, "Kode", SQL, 1, Digit))
            xFormat = ""
            For i As Integer = 1 To Digit
                If i = 1 Then
                    xFormat = "0"
                Else
                    xFormat &= "#"
                End If
            Next
            If NullToBool(EksekusiSQlSkalarNew("SELECT IsPKP FROM MAlamat WHERE NoID=" & IDSupplier)) Then
                TmpKode = Format(CDbl(TmpKode), xFormat) & "/" & fnRomawi(CInt(Tanggal.ToString("MM"))) & Tanggal.ToString("/yyyy/P")
            Else
                TmpKode = Format(CDbl(TmpKode), xFormat) & "/" & fnRomawi(CInt(Tanggal.ToString("MM"))) & Tanggal.ToString("/yyyy/N")
            End If
        Catch ex As Exception
            XtraMessageBox.Show("Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            If Not ds Is Nothing Then
                ds.Dispose()
            End If
        End Try
        Return TmpKode
    End Function
End Class
