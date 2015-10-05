Imports System.Data.SqlClient
Imports DevExpress.XtraEditors
Imports DevExpress.XtraReports
Imports System.Data.Odbc

Public Class frmCetakBarcodeVer2
    'Dim oDS As New DataSet
    Dim SQL As String = ""
    Dim DefImageList As ImageList
    'Dim btd As BarTender.Application
    'Dim btformat As New BarTender.Format
    'Dim btMessage As New BarTender.Messages

    Dim OleDBcn As New OdbcConnection
    Dim OleDBcm As New OdbcCommand

    Private Sub IsiDefault()
        DefImageList = frmMain.ImageList1
        cmdSave.ImageList = DefImageList
        cmdSave.ImageIndex = 6

        cmdTutup.ImageList = DefImageList
        cmdTutup.ImageIndex = 3

        cmdDesain.ImageList = DefImageList
        cmdDesain.ImageIndex = 2

        RefreshLookUp()
        'EksekusiSQL("DELETE FROM TCetakBarcode WHERE IDUser=" & IDUserAktif & " AND IP='" & FixApostropi(IPLokal) & "'")
        RefreshDetil()
    End Sub
    Private Sub RefreshLookUpTipeCetakan()
        Dim ds As New DataSet
        Try
            SQL = "SELECT MTipeCetakBarcode.NoID, MTipeCetakBarcode.Kode, MTipeCetakBarcode.NamaFile FROM MTipeCetakBarcodeV2 MTipeCetakBarcode"
            ds = ExecuteDataset("MTipeCetakBarcode", SQL)
            txtTipeCetakkan.Properties.DataSource = ds.Tables("MTipeCetakBarcode")
            txtTipeCetakkan.Properties.ValueMember = "NoID"
            txtTipeCetakkan.Properties.DisplayMember = "NamaFile"
        Catch ex As Exception
            XtraMessageBox.Show("Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            ds.Dispose()
        End Try
    End Sub
    Private Sub RefreshLookUp()
        Dim ds As New DataSet
        Try
            SQL = "SELECT MBarangD.NoID,MBarangD.Barcode, MBarang.Kode , MBarang.Nama + ' ' + IsNull(MBarangD.Varian,'') AS Nama, MSatuan.Nama AS Satuan, MBarangD.HargaJual-ISNULL(MBarangD.NilaiDiskon,0) as HargaJual FROM MBarangD Inner Join  MBarang ON MBarangD.IDbarang=Mbarang.NoID LEFT JOIN MSatuan ON MBarangD.IDSatuan=MSatuan.NoID where MBarang.IsActive=1 and MBarangD.IsActive=1"
            ds = ExecuteDataset("MBarang", SQL)
            txtBarang.Properties.DataSource = ds.Tables("MBarang")
            txtBarang.Properties.ValueMember = "NoID"
            txtBarang.Properties.DisplayMember = "Barcode"

            SQL = "SELECT MKategori.NoID, MKategori.Kode, MKategori.Kode + ' ' + MKategori.Nama AS Nama FROM MKategori where MKategori.IsActive=1 "
            ds = ExecuteDataset("MBarang", SQL)
            txtKategori.Properties.DataSource = ds.Tables("MBarang")
            txtKategori.Properties.ValueMember = "NoID"
            txtKategori.Properties.DisplayMember = "Nama"

            RefreshLookUpTipeCetakan()
        Catch ex As Exception
            XtraMessageBox.Show("Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            ds.Dispose()
        End Try
    End Sub

    Private Sub cmdSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSave.Click
        Dim ds As New DataSet
        Dim SQL As String = ""
        Try
            If Simpan() Then
                'If System.IO.File.Exists(NamaFile) Then

                'btformat = btd.Formats.Open(NamaFile, False, "")
                'For i As Integer = 1 To btformat.Databases.Count
                '    'btformat.Databases.Item(i).
                '    btformat.Databases.Item(i).ODBC.UserId = Ini.BacaIni("odbcconfig", "Username", "sa")
                '    btformat.Databases.Item(i).ODBC.Password = Ini.BacaIni("odbcconfig", "Password", "elliteserv")
                '    btformat.Databases.Item(i).ODBC.SQLStatement = "SELECT * FROM MCetakBarcode WHERE IP='" & FixApostropi(IPLokal) & "' AND IDUser=" & IDUserAktif
                '    'MsgBox(btformat.Databases.QueryPrompts.Count)
                'Next
                'btformat.EnablePrompting = True
                ''btformat.SetNamedSubStringValue("IP", IPLokal)
                ''btformat.SetNamedSubStringValue("IDUser", IDUserAktif)
                ''btformat.Print("CityToys", True, -1, btMessage)
                'btformat.PrintOut(True, True)

                'Using DevExpress
                'Cari Jika Ada Query di Text
                If System.IO.File.Exists(Application.StartupPath & "\Report\SQL\" & txtTipeCetakkan.Text.ToUpper.Replace(".REPX", ".SQL")) Then
                    SQL = GetTextFile(Application.StartupPath & "\Report\SQL\" & txtTipeCetakkan.Text.ToUpper.Replace(".REPX", ".SQL"))
                Else
                    SQL = "SELECT * FROM MCetakBarcode "
                    SaveTextFile("SELECT * FROM MCetakBarcode ", Application.StartupPath & "\Report\SQL\" & txtTipeCetakkan.Text.ToUpper.Replace(".REPX", ".SQL"))
                End If
                ds = ExecuteDataset("MCetakBarcode", SQL & " WHERE IP='" & FixApostropi(IPLokal) & "' AND IDUser=" & IDUserAktif)
                clsCetakReportDevExpress.ViewXtraReport(Me, mdlCetakCR.action_.Preview, NamaFile, txtTipeCetakkan.Text, txtTipeCetakkan.Text, ds)
                'End If
            Else
                'XtraMessageBox.Show("Info : file " & NamaFile.ToLower & " tidak ditemukan.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If
        Catch ex As Exception
            XtraMessageBox.Show("Info : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
        Finally
            'btformat.Close(BarTender.BtSaveOptions.btSaveChanges)
            If Not ds Is Nothing Then
                ds.Dispose()
            End If
        End Try
    End Sub

    Private Function Simpan() As Boolean
        Dim dlg As DevExpress.Utils.WaitDialogForm = Nothing
        Dim curentcursor As Cursor = Windows.Forms.Cursor.Current
        Dim DS As New DataSet
        Dim IDAccess As Long = -1
        Dim NewBarcode As String = ""
        Try
            'OleDBcn = New OleDbConnection
            'OleDBcm = New OleDbCommand

            dlg = New DevExpress.Utils.WaitDialogForm(String.Format("Sedang merefresh data.{0}MOHON TUNGGU ...", vbCrLf), System.Windows.Forms.Application.ProductName)
            dlg.TopMost = False
            dlg.Show()
            Windows.Forms.Cursor.Current = Cursors.WaitCursor
            EksekusiSQL("DELETE FROM MCetakBarcode WHERE IP='" & FixApostropi(IPLokal) & "'")
            IDAccess = 0
            For i As Integer = 0 To GV1.RowCount - 1
                SQL = "SELECT TCetakBarcode.*, MBarang.Nama AS NamaBarang, MBarangD.Barcode AS MBarangBarcode, MBarangD.HargaJual-ISNULL(MBarangD.NilaiDiskon,0) AS HargaBarcode, MBarang.Kode AS KDBarang, MBarangD.HargaJual-ISNULL(MBarangD.NilaiDiskon,0) AS HargaBarcode FROM TCetakBarcode LEFT JOIN MBarangD on TCetakbarcode.IDBarang=MBarangD.NoID left join MBarang ON MBarang.NoID=MBarangD.IDBarang WHERE TCetakBarcode.NoID=" & NullToLong(GV1.GetRowCellValue(i, "NoID"))
                DS = ExecuteDataset("MTemp", SQL)
                With DS.Tables("MTemp")
                    If .Rows.Count >= 1 Then
                        For x As Integer = 0 To NullToDbl(.Rows(0).Item("Qty")) - 1
                            SQL = "INSERT INTO MCetakBarcode ( NoID, IDBarang, IDUser, IP, KodeBarcode, NamaBarang, Barcode, HargaBarcode,HargaRp ) VALUES (" & _
                                  GetNewID("MCetakBarcode", "NoID") & "," & NullToLong(.Rows(0).Item("IDBarang")) & "," & IDUserAktif & ",'" & IPLokal & "','" & FixApostropi(NullToStr(.Rows(0).Item("KDBarang"))) & "', '" & FixApostropi(NullToStr(.Rows(0).Item("NamaBarang"))) & "', '" & FixApostropi(NullToStr(.Rows(0).Item("MBarangBarcode"))) & "', " & FixKoma(NullToDbl(.Rows(0).Item("HargaBarcode"))) & ",'Rp. " & Strings.Format(NullToDbl(.Rows(0).Item("HargaBarcode")), "###,###,##0") & "')"
                            EksekusiSQL(SQL)
                            ' NewBarcode = Append_EAN13_Checksum("9" & NullToLong(.Rows(0).Item("IDBarang")).ToString("00000000000")).ToString
                            'EksekusiSQL("UPDATE MBarang set Barcode='" & FixApostropi(NewBarcode) & "' WHERE NoID=" & NullToLong(.Rows(0).Item("IDBarang")))
                            'IDAccess = IDAccess + 1
                        Next
                    End If
                End With
            Next
            Return True
        Catch ex As Exception
            XtraMessageBox.Show("Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return False
        Finally
            DS.Dispose()
            dlg.Dispose()
            Windows.Forms.Cursor.Current = curentcursor
        End Try
    End Function

    Private Sub BarButtonItem1_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles cmdSaveLayout.ItemClick
        Dim xOtorisasi As New frmOtorisasiAdmin
        Try
            If XtraMessageBox.Show("Simpan Layout?", NamaAplikasi, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = Windows.Forms.DialogResult.Yes AndAlso xOtorisasi.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
                Ini.TulisIniPath(FolderLayouts & Me.Name & ".ini", "Form", "Width", Me.Width)
                Ini.TulisIniPath(FolderLayouts & Me.Name & ".ini", "Form", "Height", Me.Height)
                Me.Left = (Screen.PrimaryScreen.WorkingArea.Width - Me.Width) \ 2
                Me.Top = (Screen.PrimaryScreen.WorkingArea.Height - Me.Height) \ 2

                LayoutControl1.SaveLayoutToXml(FolderLayouts & Me.Name & LayoutControl1.Name & ".xml")
                gvBarang.SaveLayoutToXml(FolderLayouts & Me.Name & gvBarang.Name & ".xml")
                GV1.SaveLayoutToXml(FolderLayouts & Me.Name & GV1.Name & ".xml")
                gvTipeCetakan.SaveLayoutToXml(FolderLayouts & Me.Name & gvTipeCetakan.Name & ".xml")
            End If
        Catch ex As Exception
            XtraMessageBox.Show("Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            xOtorisasi.Dispose()
        End Try
    End Sub

    Private Sub frmCetakBarcode_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        'btd.Quit()
    End Sub

    Private Sub frmEntriPOD_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim dlg As DevExpress.Utils.WaitDialogForm = Nothing
        Dim curentcursor As Cursor = Windows.Forms.Cursor.Current
        Try
            Windows.Forms.Cursor.Current = Cursors.WaitCursor
            dlg = New DevExpress.Utils.WaitDialogForm(String.Format("Creating component and analize database.{0}MOHON TUNGGU ...", vbCrLf), System.Windows.Forms.Application.ProductName)
            dlg.TopMost = False
            dlg.Show()
            Windows.Forms.Cursor.Current = Cursors.WaitCursor
            IsiDefault()

            Me.Width = Ini.BacaIniPath(FolderLayouts & Me.Name & ".ini", "Form", "Width", Me.Width)
            Me.Height = Ini.BacaIniPath(FolderLayouts & Me.Name & ".ini", "Form", "Height", Me.Height)

            Me.Left = (Screen.PrimaryScreen.WorkingArea.Width - Me.Width) \ 2
            Me.Top = (Screen.PrimaryScreen.WorkingArea.Height - Me.Height) \ 2

            If System.IO.File.Exists(FolderLayouts & Me.Name & LayoutControl1.Name & ".xml") Then
                LayoutControl1.RestoreLayoutFromXml(FolderLayouts & Me.Name & LayoutControl1.Name & ".xml")
            End If
            If System.IO.File.Exists(FolderLayouts & Me.Name & GV1.Name & ".xml") Then
                GV1.RestoreLayoutFromXml(FolderLayouts & Me.Name & GV1.Name & ".xml")
            End If
            'btd = New BarTender.Application

            FungsiControl.SetForm(Me)
            tglDari.EditValue = Today
            TglSampai.EditValue = Today

        Catch ex As Exception
            XtraMessageBox.Show(ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            Windows.Forms.Cursor.Current = curentcursor
            dlg.Close()
            dlg.Dispose()
        End Try
    End Sub

    Public Sub RefreshDetil()
        Dim Ds As New DataSet
        Try
            SQL = "SELECT TCetakBarcode.NoID, TCetakBarcode.Qty, TCetakBarcode.NoUrut, MBarangD.Barcode, MBarang.Kode AS KodeBarang, MBarang.Nama + ' ' + isnull(mbarangd.Varian,'') AS NamaBarang , MBarangD.HargaJual-ISNULL(MBarangD.NilaiDiskon,0) as HargaJual,MSatuan.Nama as Satuan " & _
            "FROM TCetakBarcode LEFT JOIN MBarangD On TCetakbarcode.IDbarang=MbarangD.NoID inner Join MBarang ON MBarangD.IDBarang=MBarang.NoID inner join MSatuan On MBarangD.IDSatuan=MSatuan.NoID WHERE TCetakBarcode.IP='" & FixApostropi(IPLokal) & "' AND TCetakBarcode.IDUser=" & IDUserAktif
            SQL &= " ORDER BY NoUrut"
            Ds = ExecuteDataset("Tabel", SQL)
            GC1.DataSource = Ds.Tables("Tabel")
            With GV1
                For x As Integer = 0 To .Columns.Count - 1
                    Select Case .Columns(x).ColumnType.Name.ToLower
                        Case "int32", "int64", "int"
                            .Columns(x).DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
                            .Columns(x).DisplayFormat.FormatString = "n0"
                        Case "decimal", "single", "money", "double"
                            .Columns(x).DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
                            .Columns(x).DisplayFormat.FormatString = "n2"
                        Case "string"
                            .Columns(x).DisplayFormat.FormatType = DevExpress.Utils.FormatType.None
                            .Columns(x).DisplayFormat.FormatString = ""
                        Case "date", "datetime"
                            If .Columns(x).FieldName.Trim.ToLower = "jam" Then
                                .Columns(x).DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime
                                .Columns(x).DisplayFormat.FormatString = "HH:mm"
                            Else
                                .Columns(x).DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime
                                .Columns(x).DisplayFormat.FormatString = "dd-MM-yyyy"
                            End If
                    End Select
                Next
            End With
        Catch ex As Exception
            XtraMessageBox.Show("Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            Ds.Dispose()
        End Try
    End Sub

    Private Sub txtBarang_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtBarang.EditValueChanged
        Dim Ds As New DataSet
        Try
            SQL = "SELECT MBarangD.Barcode,MBarang.Kode,MBarang.Nama + ' ' + IsNull(MBarangD.Varian,'') AS NamaBarang, MBarangD.HargaJual-ISNULL(MBarangD.NilaiDiskon,0) AS HargaJual,MSatuan.Nama as Satuan FROM MBarangD INNER JOIN MBarang ON MBarangD.IDBarang = MBarang.NoID inner join MSatuan on MBarangD.IDSatuan = MSatuan.NoID WHERE  MBarangD.NoID=" & NullToLong(txtBarang.EditValue)
            Ds = ExecuteDataset("Tabel", SQL)
            If Ds.Tables("Tabel").Rows.Count >= 1 Then
                txtkode.Text = NullToStr(Ds.Tables(0).Rows(0).Item("Kode"))
                txtNamaStock.Text = NullToStr(Ds.Tables(0).Rows(0).Item("NamaBarang"))
                txtHargaJual.EditValue = NullToDbl(Ds.Tables(0).Rows(0).Item("HargaJual"))
            End If
        Catch ex As Exception
            XtraMessageBox.Show("Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            Ds.Dispose()
        End Try
    End Sub
    Private Sub BarButtonItem1_ItemClick_1(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem1.ItemClick
        cmdSave.PerformClick()
    End Sub

    Private Sub BarButtonItem2_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem2.ItemClick
        cmdTutup.PerformClick()
    End Sub

    Private Sub gvBarang_DataSourceChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvBarang.DataSourceChanged
        If System.IO.File.Exists(FolderLayouts & Me.Name & gvBarang.Name & ".xml") Then
            gvBarang.RestoreLayoutFromXml(FolderLayouts & Me.Name & gvBarang.Name & ".xml")
        End If
        With gvBarang
            For x As Integer = 0 To .Columns.Count - 1
                Select Case .Columns(x).ColumnType.Name.ToLower
                    Case "int32", "int64", "int"
                        .Columns(x).DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
                        .Columns(x).DisplayFormat.FormatString = "n0"
                    Case "decimal", "single", "money", "double"
                        .Columns(x).DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
                        .Columns(x).DisplayFormat.FormatString = "n2"
                    Case "string"
                        .Columns(x).DisplayFormat.FormatType = DevExpress.Utils.FormatType.None
                        .Columns(x).DisplayFormat.FormatString = ""
                    Case "date", "datetime"
                        If .Columns(x).FieldName.Trim.ToLower = "jam" Then
                            .Columns(x).DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime
                            .Columns(x).DisplayFormat.FormatString = "HH:mm"
                        Else
                            .Columns(x).DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime
                            .Columns(x).DisplayFormat.FormatString = "dd-MM-yyyy"
                        End If
                End Select
            Next
        End With
    End Sub

    Private Sub GV1_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles GV1.KeyDown
        If e.KeyCode = Keys.F5 Then
            RefreshDetil()
        ElseIf e.KeyCode = Keys.Delete Then
            Try
                For Each i In GV1.GetSelectedRows
                    EksekusiSQL("DELETE FROM TCetakBarcode WHERE NoID=" & GV1.GetRowCellValue(i, "NoID"))
                Next
                RefreshDetil()
            Catch ex As Exception
                XtraMessageBox.Show("Untuk menghapus pilih item barang lalu tekan Delete.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
            End Try
        End If
    End Sub

    Private Sub txtQty_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtQty.KeyDown
        Dim NoID As Long = -1
        Try
            If e.KeyCode = Keys.Enter Then
                If isvalidasi() Then
                    NoID = GetNewID("TCetakBarcode", "NoID")
                    EksekusiSQL("INSERT INTO TCetakBarcode ([NoID],[NoUrut],[IDBarang],[NamaBarang],[KodeBarcode],[Barcode],[IDUser],[IP],[Qty]) VALUES (" & _
                                 NoID & ", " & GetNewID("TCetakBarcode", "NoUrut", " WHERE NoID=" & NoID) & ", " & NullToLong(txtBarang.EditValue) & ",'" & FixApostropi(txtNamaStock.Text) & "', " & _
                                "'', '', " & IDUserAktif & ", '" & FixApostropi(IPLokal) & "', " & FixKoma(txtQty.EditValue) & ")")
                    RefreshDetil()
                    txtBarang.EditValue = -1
                    txtNamaStock.Text = ""
                    txtQty.EditValue = 0
                    txtHargaJual.EditValue = 0
                    txtBarang.Focus()
                End If
            End If
        Catch ex As Exception
            XtraMessageBox.Show("Info " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
        End Try
    End Sub
    Private Function isvalidasi() As Boolean
        If txtBarang.Text.Trim = "" Then
            XtraMessageBox.Show("Kode / Nama Barang masih kosong?", NamaAplikasi, MessageBoxButtons.OK)
            txtBarang.Focus()
            Return False
        End If
        If txtQty.EditValue <= 0 Then
            XtraMessageBox.Show("Qty masih kosong?", NamaAplikasi, MessageBoxButtons.OK)
            txtQty.Focus()
            Return False
        End If

        Return True
    End Function

    Private Sub cmdDesain_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdDesain.Click
        Dim DS As New DataSet
        Dim SQL As String = ""
        Try
            If Simpan() Then
                'If System.IO.File.Exists(NamaFile) Then
                'btformat = btd.Formats.Open(System.Windows.Forms.Application.StartupPath & "\Report\CTBarcodeV2.btw", False, "")
                'For i As Integer = 1 To btformat.Databases.Count
                '    btformat.Databases.Item(i).ODBC.UserId = Ini.BacaIni("odbcconfig", "Username", "sa")
                '    btformat.Databases.Item(i).ODBC.Password = Ini.BacaIni("odbcconfig", "Password", "elliteserv")
                'Next
                'btformat.EnablePrompting = True
                'btformat.PrintOut(False, True)

                'BukaFile(NamaFile)
                'Cari Jika Ada Query di Text
                If System.IO.File.Exists(Application.StartupPath & "\Report\SQL\" & txtTipeCetakkan.Text.ToUpper.Replace(".REPX", ".SQL")) Then
                    SQL = GetTextFile(Application.StartupPath & "\Report\SQL\" & txtTipeCetakkan.Text.ToUpper.Replace(".REPX", ".SQL"))
                Else
                    SQL = "SELECT * FROM MCetakBarcode WHERE IP='" & FixApostropi(IPLokal) & "' AND IDUser=" & IDUserAktif
                    SaveTextFile("SELECT * FROM MCetakBarcode", Application.StartupPath & "\Report\SQL\" & txtTipeCetakkan.Text.ToUpper.Replace(".REPX", ".SQL"))
                End If
                DS = ExecuteDataset("MCetakBarcode", SQL)
                clsCetakReportDevExpress.ViewXtraReport(Me, mdlCetakCR.action_.Edit, NamaFile, txtTipeCetakkan.Text, txtTipeCetakkan.Text, DS)
                'Else
                '    XtraMessageBox.Show("Info : file " & NamaFile.ToLower & " tidak ditemukan.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
                'End If
            End If
        Catch ex As Exception
            XtraMessageBox.Show("Info : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
            'Finally
            '    btformat.Close()
        Finally
            If Not DS Is Nothing Then
                DS.Dispose()
            End If
        End Try
    End Sub
    Private Function NamaFile() As String
        Return System.Windows.Forms.Application.StartupPath & "\Report\" & txtTipeCetakkan.Text
    End Function

    Private Sub gvTipeCetakan_DataSourceChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvTipeCetakan.DataSourceChanged
        If System.IO.File.Exists(FolderLayouts & Me.Name & gvTipeCetakan.Name & ".xml") Then
            gvTipeCetakan.RestoreLayoutFromXml(FolderLayouts & Me.Name & gvTipeCetakan.Name & ".xml")
        End If
        With gvTipeCetakan
            For x As Integer = 0 To .Columns.Count - 1
                Select Case .Columns(x).ColumnType.Name.ToLower
                    Case "int32", "int64", "int"
                        .Columns(x).DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
                        .Columns(x).DisplayFormat.FormatString = "n0"
                    Case "decimal", "single", "money", "double"
                        .Columns(x).DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
                        .Columns(x).DisplayFormat.FormatString = "n2"
                    Case "string"
                        .Columns(x).DisplayFormat.FormatType = DevExpress.Utils.FormatType.None
                        .Columns(x).DisplayFormat.FormatString = ""
                    Case "date", "datetime"
                        If .Columns(x).FieldName.Trim.ToLower = "jam" Then
                            .Columns(x).DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime
                            .Columns(x).DisplayFormat.FormatString = "HH:mm"
                        Else
                            .Columns(x).DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime
                            .Columns(x).DisplayFormat.FormatString = "dd-MM-yyyy"
                        End If
                End Select
            Next
        End With
    End Sub

    Private Sub txtQty_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtQty.EditValueChanged

    End Sub

    Private Sub SimpleButton1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SimpleButton1.Click
        Dim strsql As String
        Dim oconn As SqlConnection
        Dim ocmd As SqlCommand
        Dim odr As SqlDataReader
        Dim NoID As Integer
        Dim IDBarangD As Integer
        Try
            strsql = "select MBarangD.NoID from MBarangD where ((MBarangD.LastUpdated>='" & tglDari.DateTime.ToString("yyyy-MM-dd") & "' AND MBarangD.LastUpdated<'" & TglSampai.DateTime.AddDays(1).ToString("yyyy-MM-dd") & "') OR (MBarang.TerakhirUpdate>='" & tglDari.DateTime.ToString("yyyy-MM-dd") & "' AND MBarang.TerakhirUpdate<'" & TglSampai.DateTime.AddDays(1).ToString("yyyy-MM-dd") & "')) AND MBarang.IsActive=1 AND MBarangD.IsActive=1 ORDER BY MBarangD.Barcode "
            oconn = New SqlConnection(StrKonSql)
            ocmd = New SqlCommand(strsql, oconn)
            oConn.Open()

            odr = ocmd.ExecuteReader
            Do While odr.Read
                IDBarangD = NullTolInt(odr.GetValue(0))
                NoID = GetNewID("TCetakBarcode", "NoID")
                EksekusiSQL("INSERT INTO TCetakBarcode ([NoID],[NoUrut],[IDBarang],[NamaBarang],[KodeBarcode],[Barcode],[IDUser],[IP],[Qty]) " & _
                             " Select " & NoID & ", " & GetNewID("TCetakBarcode", "NoUrut", " WHERE NoID=" & NoID) & ", " & _
                             "MBarangD.NoID,MBarang.Nama,MBarang.Kode,MBarangD.Barcode," & _
                             IDUserAktif & ", '" & FixApostropi(IPLokal) & "', 1 FROM MBarangD inner join MBarang on MBarangD.IDbarang=MBarang.NoID where MBarangD.NoID=" & IDBarangD)
            Loop
            ocmd.Dispose()
            oconn.Close()
            oconn.Dispose()
            RefreshDetil()
        Catch ex As Exception
        End Try

        'txtBarang.EditValue = -1
        'txtNamaStock.Text = ""
        'txtQty.EditValue = 0
        'txtHargaJual.EditValue = 0
        'txtBarang.Focus()
    End Sub
    Sub DeleteAll()
        EksekusiSQL("DELETE FROM TCetakBarcode WHERE IDUser=" & IDUserAktif & " AND IP='" & FixApostropi(IPLokal) & "'")
        RefreshDetil()
    End Sub

    Private Sub cmdTutup_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdTutup.Click
        DialogResult = Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub SimpleButton2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SimpleButton2.Click
        DeleteAll()
    End Sub

    Private Sub SimpleButton3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SimpleButton3.Click
        Try
            For Each i In GV1.GetSelectedRows
                EksekusiSQL("DELETE FROM TCetakBarcode WHERE NoID=" & GV1.GetRowCellValue(i, "NoID"))
            Next
            RefreshDetil()
        Catch ex As Exception
            XtraMessageBox.Show("Untuk menghapus pilih item barang lalu tekan Delete.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
        End Try
    End Sub

    Private Sub GC1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles GC1.Click

    End Sub

    Private Sub txtTipeCetakkan_ButtonClick(ByVal sender As Object, ByVal e As DevExpress.XtraEditors.Controls.ButtonPressedEventArgs) Handles txtTipeCetakkan.ButtonClick
        If e.Button.Index = 1 Then
            Dim x As New frmAddFileBarcode
            Dim frmOtorisasi As New frmOtorisasiAdmin
            If frmOtorisasi.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
                x.StartPosition = FormStartPosition.CenterParent
                x.ShowDialog(Me)
                RefreshLookUpTipeCetakan()
            End If
            frmOtorisasi.Dispose()
            x.Dispose()
        End If
    End Sub

    Private Sub txtKategori_ButtonClick(ByVal sender As Object, ByVal e As DevExpress.XtraEditors.Controls.ButtonPressedEventArgs) Handles txtKategori.ButtonClick
        Dim ds As New DataSet
        Dim NoID As Long = -1
        Try
            If e.Button.Index = 1 AndAlso txtKategori.Text <> "" AndAlso XtraMessageBox.Show("Ingin menggenerate kategori " & txtKategori.Text & "?", NamaAplikasi, MessageBoxButtons.YesNo, MessageBoxIcon.Information) = Windows.Forms.DialogResult.Yes Then
                ds = ExecuteDataset("MKategori", "SELECT MBarangD.NoID, MbarangD.IDBarang, MBarangD.Barcode, MBarang.Kode, MBarang.Nama + ' ' + IsNull(MBarangD.Varian,'') AS NamaBarang, MBarangD.IDSatuan, MBarangD.Konversi FROM MBarang INNER JOIN MBarangD ON MBarang.NoID=MBarangD.IDBarang WHERE MBarangD.IsActive=1 AND MBarang.IsActive=1 AND MBarangD.IsJualPOS=1 AND MBarang.IDKategori=" & NullToLong(txtKategori.EditValue))
                For i As Integer = 0 To ds.Tables("MKategori").Rows.Count - 1
                    NoID = GetNewID("TCetakBarcode", "NoID")
                    EksekusiSQL("INSERT INTO TCetakBarcode ([NoID],[NoUrut],[IDBarang],[NamaBarang],[KodeBarcode],[Barcode],[IDUser],[IP],[Qty]) VALUES (" & _
                                NoID & ", " & GetNewID("TCetakBarcode", "NoUrut", " WHERE NoID=" & NoID) & ", " & NullToLong(ds.Tables("MKategori").Rows(i).Item("NoID")) & ",'" & FixApostropi(ds.Tables("MKategori").Rows(i).Item("NamaBarang")) & "', " & _
                                "'', '', " & IDUserAktif & ", '" & FixApostropi(IPLokal) & "', " & FixKoma(1) & ")")
                Next
                RefreshDetil()
                txtKategori.EditValue = -1
                txtBarang.EditValue = -1
                txtNamaStock.Text = ""
                txtQty.EditValue = 0
                txtHargaJual.EditValue = 0
                txtBarang.Focus()
            End If
        Catch ex As Exception
            XtraMessageBox.Show("Info Keslahaan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
        Finally
            If Not ds Is Nothing Then
                ds.Dispose()
            End If
        End Try
    End Sub

    Private Sub txtKategori_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtKategori.EditValueChanged

    End Sub
End Class