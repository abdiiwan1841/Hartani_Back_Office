Imports System.Data.SQLite
Imports DevExpress.Utils
Imports DevExpress.XtraEditors
Imports DevExpress.XtraGrid
Imports DevExpress.XtraEditors.Repository
Imports System.Net.Dns
Imports System.Net
Imports System.IO
Imports System.Security.Cryptography
Imports System.Threading

Module Util
    Public TanggalSystem As DateTime = Date.Today
    Public Parser As New modParser
    Public IPLokal As String = GetIPAddress()

    'Untuk Lihat Hasil Posting
    Public IsShowStock As Boolean = False
    Public frmHasilPosted As frmHasilPosting

    'Untuk Lihat Hasil Posting
    Public IsShowHasilPostingan As Boolean = False
    Public frmHasilPostingan As frmHasilPostingNonStock

    'Untuk Layouts
    Public FolderLayouts As String = Application.StartupPath & "\System\Layouts\"
    Public FolderFoto As String = Application.StartupPath & "\System\PathFoto\"
    Public TglDitetapkanSO As Date = TanggalSystem

    Public NamaPerusahaan As String = ""
    Public AlamatPerusahaan As String = ""
    Public KotaPerusahaan As String = "Surabaya"
    Public FormatFP As String = "010.000.YY.00000000"
    'Public NamaApplikasi As String = "Sistem Informasi Accounting"

    Public Function PingIPComputer(ByVal IP As String, ByVal TimeOut As Long, Optional ByRef PesanError As String = "") As Boolean
        Try
            If My.Computer.Network.Ping(IP, TimeOut) Then
                'MsgBox("Server pinged successfully.")
                Return True
            Else
                'MsgBox("Ping request timed out.")
                Return False
            End If
        Catch ex As Exception
            PesanError = ex.Message
            Return False
        End Try
    End Function
    Public Function FileExists(ByVal xFile As String, ByVal timeout As Integer) As Boolean
        Dim exists As Boolean = True
        Dim t As New Thread(DirectCast(Function() CheckFileFunction(xFile), ThreadStart))
        t.Start()
        Dim completed As Boolean = t.Join(timeout)
        If Not completed Then
            exists = False
            t.Abort()
        End If
        Return exists
    End Function
    Public Function PathExists(ByVal path As String, ByVal timeout As Integer) As Boolean
        Dim exists As Boolean = True
        Dim t As New Thread(DirectCast(Function() CheckPathFunction(path), ThreadStart))
        t.Start()
        Dim completed As Boolean = t.Join(timeout)
        If Not completed Then
            exists = False
            t.Abort()
        End If
        Return exists
    End Function
    Public Function CheckFileFunction(ByVal xFile As String) As Boolean
        Return System.IO.File.Exists(xFile)
    End Function
    Public Function CheckPathFunction(ByVal path As String) As Boolean
        Return System.IO.Directory.Exists(path)
    End Function

    Public Function EAN8_Checksum(ByVal EAN8_Barcode As String) As String
        'http://www.barcodeisland.com/ean8.phtml

        Dim ChecksumCalculation As Integer
        ChecksumCalculation = 0
        Dim Position As Integer
        Position = 1
        Dim i As Integer
        For i = Len(EAN8_Barcode) - 1 To 0 Step -1
            If Position Mod 2 = 1 Then
                'odd position
                ChecksumCalculation = ChecksumCalculation + CLng(Mid(EAN8_Barcode, i + 1, 1)) * 3
            Else
                'even position
                ChecksumCalculation = ChecksumCalculation + CLng(Mid(EAN8_Barcode, i + 1, 1)) * 1
            End If
            Position = Position + 1
        Next

        Dim Checksum As Integer
        Checksum = (10 - (ChecksumCalculation Mod 10)) Mod 10
        If Checksum = 10 Then
            Checksum = 0
        End If
        If EAN8_Barcode.Length >= 7 Then
            Return EAN8_Barcode.Substring(0, 7) & Format$(Checksum, "0")
        Else
            Return EAN8_Barcode.ToString() & Format$(Checksum, "0")
        End If
    End Function

    Public Function Append_EAN13_Checksum(ByVal RawString As String) As String
        Dim Position As Integer
        Dim Checksum As Integer

        Checksum = 0
        For Position = 2 To 12 Step 2
            Checksum = Checksum + Val(Mid$(RawString, Position, 1))
        Next Position
        Checksum = Checksum * 3
        For Position = 1 To 11 Step 2
            Checksum = Checksum + Val(Mid$(RawString, Position, 1))
        Next Position
        Checksum = Checksum Mod 10
        Checksum = 10 - Checksum
        If Checksum = 10 Then
            Checksum = 0
        End If
        If RawString.Length >= 12 Then
            Return RawString.Substring(0, 12) & Format$(Checksum, "0")
        Else
            Return RawString.ToString() & Format$(Checksum, "0")
        End If
    End Function

    Public Function GetIPAddress() As String
        Dim IP1 As String = ""
        'Dim localEntry As Net.IPHostEntry = Dns.GetHostEntry(System.Net.Dns.GetHostName)
        'For Each address As Net.IPAddress In localEntry.AddressList
        '    If IP1 = "" Then
        '        IP1 = IP1 & address.ToString
        '    Else
        '        IP1 = address.IsIPv6SiteLocal.ToString & ":" & IP1
        '    End If
        'Next
        Dim IP As System.Net.IPAddress
        With Dns.GetHostByName(Dns.GetHostName())
            IP = New System.Net.IPAddress(.AddressList(0).Address)
            IP1 = IP.ToString
        End With
        Return IP1
    End Function

    'And to implement It you would need to call it like this,you can do whatever you want
    'with it as its very flexible
    Public Sub MYIPandHOST()
        Dim host As String = System.Net.Dns.GetHostName
        XtraMessageBox.Show("Name of the System is: " & host)
        XtraMessageBox.Show("Your IP address is: " & GetIPAddress())
    End Sub
    Public Sub BukaFile(ByVal nmfile As String)
        Try
            Dim p As New System.Diagnostics.ProcessStartInfo
            p.Verb = "Open"
            p.WindowStyle = ProcessWindowStyle.Normal
            p.FileName = nmfile
            p.UseShellExecute = True
            System.Diagnostics.Process.Start(p)
        Catch ex As Exception
            XtraMessageBox.Show("Ada Kesalahan :" & vbCrLf & ex.Message & vbCrLf & "File : " & nmfile, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
        End Try
    End Sub
    Public Sub PrintFile(ByVal nmfile As String)
        Try
            Dim p As New System.Diagnostics.ProcessStartInfo
            p.Verb = "Print"
            p.WindowStyle = ProcessWindowStyle.Normal
            p.FileName = nmfile
            p.UseShellExecute = True
            System.Diagnostics.Process.Start(p)
        Catch ex As Exception
            XtraMessageBox.Show("Ada Kesalahan :" & vbCrLf & ex.Message & vbCrLf & "File : " & nmfile, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
        End Try
    End Sub

    Public Function ComTerbilang(ByVal Angka As Double) As String
        Dim x As String = ""
        Dim Bilang As New CRUFLTBL.CRUFLTBG
        Try
            x = NullToStr(Bilang.UFLTerbilang(Angka))
        Catch ex As Exception
            XtraMessageBox.Show("Ada Kesalahan :" & vbCrLf & ex.Message & vbCrLf & Angka, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
        End Try
        Return x
    End Function
    Public Function NullToDbl(ByVal Value As Object) As Double
        If IsDBNull(Value) Then
            Return 0.0
        Else
            If IsNumeric(Value) Then
                Return Value
            Else
                Return 0.0
            End If

        End If
    End Function
    Public Function NullToBool(ByVal Value As Object) As Boolean
        If IsDBNull(Value) Then
            Return False
        ElseIf Value Is Nothing Then
            Return False
        ElseIf Value.ToString = "" Then
            Return False
        Else
            Return CBool(Value)
        End If
    End Function
    Public Function getValueFromLookup(ByVal sender As Object, ByVal fieldname As String) As String
        Dim lu As DevExpress.XtraEditors.SearchLookUpEdit = CType(sender, DevExpress.XtraEditors.SearchLookUpEdit)
        Dim strtablefield As String() = Split(fieldname, ".")
        Dim row As System.Data.DataRow
        Dim val As String = ""
        Try
            If lu.Properties.View.Columns Is Nothing Or lu.Properties.View.DataRowCount = 0 Then
                If strtablefield(0) <> "" And strtablefield(1) <> "" Then
                    val = NullToStr(EksekusiSQlSkalarNew("SELECT " & strtablefield(1).ToString & " from " & strtablefield(0).ToString & " where " & lu.Properties.ValueMember.ToString & "=" & lu.EditValue))
                End If
            Else
                row = lu.Properties.View.GetDataRow(lu.Properties.View.FocusedRowHandle)
                val = NullToStr(row(strtablefield(1)))
            End If
        Catch ex As Exception
            val = ""
        End Try
        Return val
    End Function
    Public Function NullToStr(ByVal Value As Object) As String
        If IsDBNull(Value) Then
            Return ""
        ElseIf Value Is Nothing Then
            Return ""
        Else
            Return Value
        End If
    End Function
    Public Function AppendBackSlash(ByVal str As String) As String
        If Right(str, 1) = "\" Then
            Return str
        Else
            Return str & "\"
        End If
    End Function
    Public Function NullToLong(ByVal Value As Object) As Long
        If IsDBNull(Value) Then
            Return 0
        Else
            If IsNumeric(Value) Then
                Return Value
            Else
                Return 0
            End If
        End If
    End Function
    Public Function NullTolInt(ByVal Value As Object) As Integer
        If IsDBNull(Value) Then
            Return 0
        Else
            If IsNumeric(Value) Then
                Return Value
            Else
                Return 0
            End If
        End If
    End Function
    Public Function NullToDate(ByVal X As Object) As Date
        If TypeOf X Is Date Then
            Return CDate(X)
        Else
            Return CDate("1/1/1900")
        End If
    End Function
    Public Function NullToDateMDB(ByVal X As Object) As String
        If TypeOf X Is Date Then
            Return "#" & Format(CDate(X), "MM/dd/yyyy") & "#"
        Else
            Return "NULL"
        End If
    End Function

    Public Function GetTextFile(ByVal FullPath As String, _
   Optional ByRef ErrInfo As String = "") As String
        Dim strContents As String = ""
        Dim objReader As StreamReader
        Try
            objReader = New StreamReader(FullPath)
            strContents = objReader.ReadToEnd()
            objReader.Close()
        Catch Ex As Exception
            ErrInfo = Ex.Message
        End Try
        Return strContents
    End Function

    Public Function SaveTextFile(ByVal strData As String, _
     ByVal FullPath As String, _
       Optional ByVal ErrInfo As String = "") As Boolean
        Dim Contents As String = ""
        Dim bAns As Boolean = False
        Dim objReader As StreamWriter
        Dim InfoFile As FileInfo = Nothing
        Try
            InfoFile = New FileInfo(FullPath)
            If Not InfoFile.Directory.Exists Then
                InfoFile.Directory.Create()
            End If
            objReader = New StreamWriter(FullPath)
            objReader.Write(strData)
            objReader.Close()
            bAns = True
        Catch Ex As Exception
            ErrInfo = Ex.Message
        End Try
        Return bAns
    End Function

    Public Sub SetGridView(ByRef GridControl1 As DevExpress.XtraGrid.GridControl)
        'Set Format Gridview Here
        Dim repChekEdit As New RepositoryItemCheckEdit
        Try
            For i As Integer = 0 To GridControl1.ViewCollection.Count - 1
                Dim view As DevExpress.XtraGrid.Views.Base.ColumnView
                If i = 0 Then
                    view = CType(GridControl1.DefaultView, Views.Base.ColumnView)
                Else
                    view = CType(GridControl1.ViewCollection(i), Views.Base.ColumnView)
                End If
                For x As Integer = 0 To view.Columns.Count - 1
                    Select Case view.Columns(i).ColumnType.Name.ToLower
                        Case "int32", "int64", "int"
                            view.Columns(i).DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
                            view.Columns(i).DisplayFormat.FormatString = "n0"
                        Case "decimal", "single", "money", "double"
                            view.Columns(i).DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
                            view.Columns(i).DisplayFormat.FormatString = "n2"
                        Case "string"
                            view.Columns(i).DisplayFormat.FormatType = DevExpress.Utils.FormatType.None
                            view.Columns(i).DisplayFormat.FormatString = ""
                        Case "date"
                            view.Columns(i).DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime
                            view.Columns(i).DisplayFormat.FormatString = "dd-MM-yyyy"
                        Case "datetime"
                            view.Columns(i).DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime
                            view.Columns(i).DisplayFormat.FormatString = "dd-MM-yyyy HH:mm"
                        Case "boolean"
                            view.Columns(i).ColumnEdit = repChekEdit
                    End Select
                Next
            Next
        Catch ex As Exception

        End Try
    End Sub
    'Public Sub SetColumnView(ByVal c As DevExpress.XtraGrid.Columns.GridColumn)
    '    Dim repChekEdit As New RepositoryItemCheckEdit
    '    Try
    '        With c
    '            Select Case .ColumnType.Name.ToUpper
    '                Case Is = "Int32".ToUpper
    '                    .DisplayFormat.FormatString = "n0"
    '                    .DisplayFormat.FormatType = FormatType.Numeric
    '                    .AppearanceCell.TextOptions.HAlignment = HorzAlignment.Far
    '                    .AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Far
    '                Case Is = "Int16".ToUpper
    '                    .DisplayFormat.FormatString = "n0"
    '                    .DisplayFormat.FormatType = FormatType.Numeric
    '                    .AppearanceCell.TextOptions.HAlignment = HorzAlignment.Far
    '                    .AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Far
    '                Case Is = "Boolean".ToUpper
    '                    .DisplayFormat.FormatType = FormatType.None
    '                    .AppearanceCell.TextOptions.HAlignment = HorzAlignment.Center
    '                    .AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Center
    '                    repChekEdit = New DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit
    '                    repChekEdit.NullStyle = DevExpress.XtraEditors.Controls.StyleIndeterminate.Unchecked
    '                    .ColumnEdit = repChekEdit
    '                Case Is = "Datetime".ToUpper
    '                    .DisplayFormat.FormatString = "dd/MM/yyyy"
    '                    .DisplayFormat.FormatType = FormatType.DateTime
    '                    .AppearanceCell.TextOptions.HAlignment = HorzAlignment.Default
    '                    .AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Default
    '                Case Is = "Numeric".ToUpper
    '                    .DisplayFormat.FormatString = "n2"
    '                    .DisplayFormat.FormatType = FormatType.Numeric
    '                    .AppearanceCell.TextOptions.HAlignment = HorzAlignment.Far
    '                    .AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Far
    '                Case Is = "Decimal".ToUpper
    '                    .DisplayFormat.FormatString = "n2"
    '                    .DisplayFormat.FormatType = FormatType.Numeric
    '                    .AppearanceCell.TextOptions.HAlignment = HorzAlignment.Far
    '                    .AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Far
    '                Case Is = "Double".ToUpper
    '                    .DisplayFormat.FormatString = "n2"
    '                    .DisplayFormat.FormatType = FormatType.Numeric
    '                    .AppearanceCell.TextOptions.HAlignment = HorzAlignment.Far
    '                    .AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Far
    '                Case Is = "Money".ToUpper
    '                    .DisplayFormat.FormatString = "n2"
    '                    .DisplayFormat.FormatType = FormatType.Numeric
    '                    .AppearanceCell.TextOptions.HAlignment = HorzAlignment.Far
    '                    .AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Far

    '                Case Else
    '                    .DisplayFormat.FormatType = FormatType.None
    '                    .AppearanceCell.TextOptions.HAlignment = HorzAlignment.Default
    '                    .AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Default
    '            End Select
    '        End With
    '    Catch ex As Exception
    '        XtraMessageBox.Show("Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
    '    Finally
    '        repChekEdit.Dispose()
    '    End Try
    'End Sub
    Public Function GetTableNamebyFormname(ByVal Value As Object, ByRef strCaption As String) As String
        Dim SQLconnect As New SQLite.SQLiteConnection()
        Dim SQLcommand As SQLiteCommand
        Dim odr As SQLite.SQLiteDataReader
        Dim hasil As String = ""
        SQLconnect.ConnectionString = "Data Source=" & Application.StartupPath & "\\System\engine\syssgi.sqlite" & ";"
        SQLconnect.Open()
        SQLcommand = SQLconnect.CreateCommand
        SQLcommand.CommandText = "SELECT namatabel,namatabeldetil,caption FROM sysformheader where namaform='" & Value & "'"
        odr = SQLcommand.ExecuteReader
        hasil = NullToStr(odr.GetValue(0)) & IIf(NullToStr(odr.GetValue(1)).Trim = "", "", "," & NullToStr(odr.GetValue(1)))
        strCaption = NullToStr(odr.GetValue(2))
        odr.Close()
        SQLcommand.Dispose()
        SQLconnect.Close()
        SQLconnect.Dispose()

        Return hasil
    End Function
    Public Function Bulatkan(ByVal x As Double, ByVal Koma As Integer) As Double
        If Koma >= 0 Then
            Bulatkan = System.Math.Round(x, CInt(Koma))
            If System.Math.Round(x - Bulatkan, CInt(Koma + 5)) >= 0.5 / (10 ^ Koma) Then Bulatkan = Bulatkan + 1 / (10 ^ Koma)
        Else
            Bulatkan = x
        End If
    End Function
    Public Function Evaluate(ByVal kalimat As String) As Double
        Dim DecSep As String
        Dim Nfi As System.Globalization.NumberFormatInfo = System.Globalization.CultureInfo.InstalledUICulture.NumberFormat
        DecSep = Nfi.NumberDecimalSeparator
        kalimat = kalimat.Replace(".", DecSep).Replace(",", DecSep)
        Parser.Function = kalimat
        Parser.BuildFunctionTree()
        Return Parser.Result
    End Function
    Public IsLogin As Boolean = False
    Public NamaAplikasi As String = Application.ProductName.ToString
    Public Function FixApostropi(ByVal obj As Object) As String
        Dim x As String = ""
        Try
            x = obj.ToString.Replace("'", "''")
        Catch ex As Exception
            x = ""
        End Try
        Return x
    End Function
    Public Function FixKoma(ByVal obj As Object) As String
        Dim x As String = ""
        Try
            x = obj.ToString.Replace(",", ".")
        Catch ex As Exception
            x = ""
        End Try
        Return x
    End Function
    'Public Function FixDate(ByVal obj As DateTime) As String
    '    Dim x As String = ""
    '    Try
    '        x = "CONVERT(DATETIME," & obj.Date
    '    Catch ex As Exception
    '        x = ""
    '    End Try
    '    Return x
    'End Function
    'Encrypt or decrypt a file, saving the results in another
    'file.
    Public Sub EncryptFile(ByVal password As String, ByVal _
        in_file As String, ByVal out_file As String)
        CryptFile(password, in_file, out_file, True)
    End Sub
    Public Sub DecryptFile(ByVal password As String, ByVal _
        in_file As String, ByVal out_file As String)
        CryptFile(password, in_file, out_file, False)
    End Sub
    Public Sub CryptFile(ByVal password As String, ByVal _
        in_file As String, ByVal out_file As String, ByVal _
        encrypt As Boolean)
        ' Create input and output file streams.
        Using in_stream As New FileStream(in_file, _
            FileMode.Open, FileAccess.Read)
            Using out_stream As New FileStream(out_file, _
                FileMode.Create, FileAccess.Write)
                ' Encrypt/decrypt the input stream into the
                ' output stream.
                CryptStream(password, in_stream, out_stream, _
                    encrypt)
            End Using
        End Using
    End Sub
    'Encrypt the data in the input stream into the output
    'stream.
    Public Sub CryptStream(ByVal password As String, ByVal _
        in_stream As Stream, ByVal out_stream As Stream, ByVal _
        encrypt As Boolean)
        ' Make an AES service provider.
        Dim aes_provider As New DESCryptoServiceProvider()

        ' Find a valid key size for this provider.
        Dim key_size_bits As Integer = 0
        For i As Integer = 1024 To 1 Step -1
            If (aes_provider.ValidKeySize(i)) Then
                key_size_bits = i
                Exit For
            End If
        Next i
        Debug.Assert(key_size_bits > 0)
        Console.WriteLine("Key size: " & key_size_bits)

        ' Get the block size for this provider.
        Dim block_size_bits As Integer = aes_provider.BlockSize

        ' Generate the key and initialization vector.
        Dim key() As Byte = Nothing
        Dim iv() As Byte = Nothing
        Dim salt() As Byte = {&H0, &H0, &H1, &H2, &H3, &H4, _
            &H5, &H6, &HF1, &HF0, &HEE, &H21, &H22, &H45}
        MakeKeyAndIV(password, salt, key_size_bits, _
            block_size_bits, key, iv)

        ' Make the encryptor or decryptor.
        Dim crypto_transform As ICryptoTransform
        If (encrypt) Then
            crypto_transform = _
                aes_provider.CreateEncryptor(key, iv)
        Else
            crypto_transform = _
                aes_provider.CreateDecryptor(key, iv)
        End If

        ' Attach a crypto stream to the output stream.
        ' Closing crypto_stream sometimes throws an
        ' exception if the decryption didn't work
        ' (e.g. if we use the wrong password).
        Try
            Using crypto_stream As New CryptoStream(out_stream, _
                crypto_transform, CryptoStreamMode.Write)
                ' Encrypt or decrypt the file.
                Const block_size As Integer = 1024
                Dim buffer(block_size) As Byte
                Dim bytes_read As Integer
                Do
                    ' Read some bytes.
                    bytes_read = in_stream.Read(buffer, 0, _
                        block_size)
                    If (bytes_read = 0) Then Exit Do

                    ' Write the bytes into the CryptoStream.
                    crypto_stream.Write(buffer, 0, bytes_read)
                Loop
            End Using
        Catch
        End Try

        crypto_transform.Dispose()
    End Sub
    Public Sub MakeKeyAndIV(ByVal password As String, ByVal _
        salt() As Byte, ByVal key_size_bits As Integer, ByVal _
        block_size_bits As Integer, ByRef key() As Byte, ByRef _
        iv() As Byte)
        Dim derive_bytes As New Rfc2898DeriveBytes(password, _
            salt, 1000)
        key = derive_bytes.GetBytes(CInt(key_size_bits / 8))
        iv = derive_bytes.GetBytes(CInt(block_size_bits / 8))
    End Sub
    Public Function GrabFile(ByVal file_name As String) As String
        Try
            Using stream_reader As New IO.StreamReader(file_name)
                Dim txt As String = stream_reader.ReadToEnd()
                stream_reader.Close()
                Return txt
            End Using
        Catch exc As System.IO.FileNotFoundException
            ' Ignore this error.
            Return ""
        Catch exc As Exception
            ' Report other errors.
            DevExpress.XtraEditors.XtraMessageBox.Show(exc.Message, "Read Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return ""
        End Try
    End Function
    Public Function EncryptText(ByVal strText As String, ByVal strPwd As String) As String
        Dim i As Integer, c As Integer
        Dim strBuff As String = Nothing

#If Not CASE_SENSITIVE_PASSWORD Then

        'Convert password to upper case
        'if not case-sensitive
        strPwd = UCase$(strPwd)

#End If

        'Encrypt string
        If CBool(Len(strPwd)) Then
            For i = 1 To Len(strText)
                c = Asc(Mid$(strText, i, 1))
                c = c + Asc(Mid$(strPwd, (i Mod Len(strPwd)) + 1, 1))
                strBuff = strBuff & Chr(c And &HFF)
            Next i
        Else
            strBuff = strText
        End If
        Return strBuff
    End Function
    Public Function DecryptText(ByVal strText As String, ByVal strPwd As String) As String
        Dim i As Integer, c As Integer
        Dim strBuff As String = Nothing

#If Not CASE_SENSITIVE_PASSWORD Then

        'Convert password to upper case
        'if not case-sensitive
        strPwd = UCase$(strPwd)

#End If

        'Decrypt string
        If CBool(Len(strPwd)) Then
            For i = 1 To Len(strText)
                c = Asc(Mid$(strText, i, 1))
                c = c - Asc(Mid$(strPwd, (i Mod Len(strPwd)) + 1, 1))
                strBuff = strBuff & Chr(c And &HFF)
            Next i
        Else
            strBuff = strText
        End If
        Return strBuff
    End Function
End Module
