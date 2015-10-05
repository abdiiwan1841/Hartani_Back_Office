Imports DevExpress.XtraEditors
Imports DevExpress.XtraGrid
Imports DevExpress.XtraGrid.Views.Grid.ViewInfo
Imports DevExpress.XtraGrid.Views.Grid

Public Class frmRubahJenisBarang
    Dim IsLoad As Boolean = True
    Dim DefImageList As ImageList
    Dim repLkEdit As New DevExpress.XtraEditors.Repository.RepositoryItemSearchLookUpEdit

    Private Sub GV1_FocusedColumnChanged(ByVal sender As Object, ByVal e As DevExpress.XtraGrid.Views.Base.FocusedColumnChangedEventArgs) Handles GV1.FocusedColumnChanged
        If GV1.FocusedColumn.FieldName.ToLower = "NamaJenisKe".ToLower Then
            GV1.OptionsBehavior.Editable = True
        Else
            GV1.OptionsBehavior.Editable = False
        End If
    End Sub
    Private Sub GridView1_FocusedColumnChanged(ByVal sender As Object, ByVal e As DevExpress.XtraGrid.Views.Base.FocusedColumnChangedEventArgs) Handles GridView1.FocusedColumnChanged
        If GridView1.FocusedColumn.FieldName.ToLower = "KeJenisBarang".ToLower Then
            GridView1.OptionsBehavior.Editable = True
        Else
            GridView1.OptionsBehavior.Editable = False
        End If
    End Sub
    Private Sub cmdSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSave.Click
        Dim dlg As DevExpress.Utils.WaitDialogForm = Nothing
        Dim IDJenisBaru As Long = -1, SQL As String = ""
        Dim curentcursor As Cursor = Windows.Forms.Cursor.Current
        Try
            If txtJenisDari.Text <> "" And txtJenisKe.Text <> "" Then
                If XtraMessageBox.Show("Yakin ingin melakukan pemindahan data Master Stock dari Jenis " & txtJenisDari.Text & " ke Jenis " & txtJenisKe.Text & "?", NamaAplikasi, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) = Windows.Forms.DialogResult.Yes Then
                    Windows.Forms.Cursor.Current = Cursors.WaitCursor
                    dlg = New DevExpress.Utils.WaitDialogForm(String.Format("Sedang update data.{0}MOHON TUNGGU ...", vbCrLf), Application.ProductName)
                    dlg.TopMost = False
                    dlg.Show()
                    EksekusiSQL("UPDATE MBarang SET IDJenis=" & NullTolong(txtJenisKe.EditValue) & " WHERE IDJenis=" & NullTolong(txtJenisDari.EditValue))
                    XtraMessageBox.Show("Update jenis seluruh barang " & txtJenisDari.Text & " ke " & txtJenisKe.Text & " sukses.")
                    RefreshDetil(1)
                    RefreshDetil(2)
                    RefreshDetil(3)
                End If
            ElseIf txtJenisDari.Text <> "" And txtJenisKe.Text = "" Then
                Windows.Forms.Cursor.Current = Cursors.WaitCursor
                dlg = New DevExpress.Utils.WaitDialogForm(String.Format("Sedang update data.{0}MOHON TUNGGU ...", vbCrLf), Application.ProductName)
                dlg.TopMost = False
                dlg.Show()
                For i As Integer = 0 To GV1.RowCount - 1
                    If NullTolong(GV1.GetRowCellValue(i, "NamaJenisKe")) <> 0 Then
                        'IDJenisBaru = NullTolong(EksekusiSQlSkalarNew("SELECT NoID FROM MJenisBarang WHERE Kode='" & NullTostr(GV1.GetRowCellValue(i, "NamaJenisKe")) & "'"))
                        SQL = "UPDATE MBarang SET IDJenis=" & NullTolong(GV1.GetRowCellValue(i, "NamaJenisKe")) & " WHERE NoID=" & NullTolong(GV1.GetRowCellValue(i, "NoID"))
                        EksekusiSQL(SQL)
                    End If
                Next
                XtraMessageBox.Show("Update jenis seluruh barang " & txtJenisDari.Text & " sukses.")
                RefreshDetil(1)
                RefreshDetil(2)
                RefreshDetil(3)
            ElseIf GridView1.RowCount >= 1 Then
                Windows.Forms.Cursor.Current = Cursors.WaitCursor
                dlg = New DevExpress.Utils.WaitDialogForm(String.Format("Sedang update data.{0}MOHON TUNGGU ...", vbCrLf), Application.ProductName)
                dlg.TopMost = False
                dlg.Show()
                For i As Integer = 0 To GridView1.RowCount - 1
                    If NullToLong(GridView1.GetRowCellValue(i, "KeJenisBarang")) <> 0 Then
                        SQL = "UPDATE MBarang SET IDJenis=" & NullToLong(GridView1.GetRowCellValue(i, "KeJenisBarang")) & " WHERE NoID=" & NullToLong(GridView1.GetRowCellValue(i, "IDBarang"))
                        EksekusiSQL(SQL)
                    End If
                Next
                XtraMessageBox.Show("Update jenis seluruh barang sukses.")
                RefreshDetil(1)
                RefreshDetil(2)
                RefreshDetil(3)
            End If
        Catch ex As Exception
            XtraMessageBox.Show(ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            Windows.Forms.Cursor.Current = Cursors.Default
            If Not dlg Is Nothing Then
                dlg.Close()
                dlg.Dispose()
            End If
        End Try
    End Sub
    'Private Sub GV1_CellValueChanged(ByVal sender As Object, ByVal e As DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs) Handles GV1.CellValueChanged
    '    Try
    '        If GV1.FocusedColumn.FieldName.ToLower = "NamaJenisKe".ToLower Then
    '            If GV1.GetRowCellValue(GV1.FocusedRowHandle, "NamaJenisKe").ToString.Length <> 0 Then
    '                GV1.SetRowCellValue(GV1.FocusedRowHandle, "IDJenisKe", repLkEdit.ValueMember)
    '            End If
    '        End If
    '    Catch ex As Exception
    '        XtraMessageBox.Show("Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
    '    End Try
    'End Sub
    Private Sub frmRubahJenisBarang_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim DS As New DataSet
        DS = ExecuteDataset("Tbl", "SELECT NoID, Kode, Nama FROM MJenisBarang")
        txtJenisDari.Properties.DataSource = DS.Tables("Tbl")
        txtJenisKe.Properties.DataSource = DS.Tables("Tbl")
        repLkEdit.DataSource = DS.Tables("Tbl")
        repLkEdit.DisplayMember = "Kode"
        repLkEdit.ValueMember = "NoID"
        DS.Dispose()
        FungsiControl.SetForm(Me)
        IsLoad = False
        EksekusiSQL("DELETE FROM TJenisBarang WHERE IDUSer=" & IDUserAktif & " AND IP='" & FixApostropi(IPLokal) & "'")
        RefreshDetil(1)
        RefreshDetil(2)
        RefreshDetil(3)

        Me.Width = Ini.BacaIniPath(folderLayouts & Me.Name & ".ini", "Form", "Width", Me.Width)
        Me.Height = Ini.BacaIniPath(folderLayouts & Me.Name & ".ini", "Form", "Height", Me.Height)

        Me.Left = (Screen.PrimaryScreen.WorkingArea.Width - Me.Width) \ 2
        Me.Top = (Screen.PrimaryScreen.WorkingArea.Height - Me.Height) \ 2

        If System.IO.File.Exists(folderLayouts & Me.Name & LayoutControl1.Name & ".xml") Then
            LayoutControl1.RestoreLayoutFromXml(folderLayouts & Me.Name & LayoutControl1.Name & ".xml")
        End If
        If System.IO.File.Exists(folderLayouts & Me.Name & GV1.Name & ".xml") Then
            GV1.RestoreLayoutFromXml(folderLayouts & Me.Name & GV1.Name & ".xml")
        End If
        If System.IO.File.Exists(folderLayouts & Me.Name & GV2.Name & ".xml") Then
            GV2.RestoreLayoutFromXml(folderLayouts & Me.Name & GV2.Name & ".xml")
        End If
        If System.IO.File.Exists(FolderLayouts & Me.Name & GridView1.Name & ".xml") Then
            GridView1.RestoreLayoutFromXml(FolderLayouts & Me.Name & GridView1.Name & ".xml")
        End If
        SetTombol()
    End Sub
    Private Sub SetTombol()
        DefImageList = frmMain.ImageList1

        cmdSave.ImageList = DefImageList
        cmdSave.ImageIndex = 6

        SimpleButton1.ImageList = DefImageList
        SimpleButton1.ImageIndex = 3
    End Sub
    Private Sub SimpleButton1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SimpleButton1.Click
        DialogResult = Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub txtJenisDari_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtJenisDari.EditValueChanged
        RefreshDetil(1)
    End Sub

    Private Sub txtJenisKe_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtJenisKe.EditValueChanged
        RefreshDetil(2)
    End Sub

    Private Sub RefreshDetil(ByVal tipe As Integer)
        If IsLoad Then Exit Sub
        Dim strsql As String
        Dim ds As New DataSet
        Dim curentcursor As Cursor = Windows.Forms.Cursor.Current
        Dim repChekEdit As New DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit
        Windows.Forms.Cursor.Current = Cursors.WaitCursor
        Try
            strsql = "SELECT MJenisKe.Nama AS NamaJenisKe, MBarang.NoID, MBarang.IsNonStock AS TidakDikontrol, MBarang.Kode, MBarang.Nama, MBarang.Barcode, MSatuan.Nama AS Satuan, mjenisbarang.Nama AS Jenis, MBarang.Alias AS KodeAlias, MBarang.NamaAlias, " & vbCrLf
            strsql &= " MKategori.Nama AS Kategori, MBarang.HargaPasar, MBarang.HargaJualA, MBarang.HargaJualB, MBarang.HargaJualC, " & vbCrLf
            strsql &= " MBarang.HargaJualD, MBarang.HargaJualE, MBarang.KodeDuz, MBarang.Ctn_Duz, MBarang.CtnPcs, MBarang.IsActive, MBarang.IsNewItem AS NewItem, " & vbCrLf
            strsql &= " MBarang.HargaJualF, MBarang.HPP, " & vbCrLf
            strsql &= " MSupplier1.Nama AS Supplier1, " & vbCrLf
            strsql &= " MSupplier2.Nama AS Supplier2, " & vbCrLf
            strsql &= " MSupplier3.Nama AS Supplier3, " & vbCrLf
            strsql &= " MSupplier4.Nama AS Supplier4, " & vbCrLf
            strsql &= " MSupplier5.Nama AS Supplier5 " & vbCrLf
            strsql &= " FROM MBarang LEFT OUTER JOIN" & vbCrLf
            strsql &= " MSatuan ON MBarang.IDSatuan = MSatuan.NoID LEFT OUTER JOIN" & vbCrLf
            strsql &= " MAlamat MSupplier1 ON MBarang.IDSupplier1 = MSupplier1.NoID LEFT OUTER JOIN" & vbCrLf
            strsql &= " MAlamat MSupplier2 ON MBarang.IDSupplier2 = MSupplier2.NoID LEFT OUTER JOIN" & vbCrLf
            strsql &= " MAlamat MSupplier3 ON MBarang.IDSupplier3 = MSupplier3.NoID LEFT OUTER JOIN" & vbCrLf
            strsql &= " MAlamat MSupplier4 ON MBarang.IDSupplier4 = MSupplier4.NoID LEFT OUTER JOIN" & vbCrLf
            strsql &= " MAlamat MSupplier5 ON MBarang.IDSupplier5 = MSupplier5.NoID LEFT OUTER JOIN" & vbCrLf
            strsql &= " MKategori ON MBarang.IDKategori = MKategori.NoID LEFT OUTER JOIN" & vbCrLf
            strsql &= " mjenisbarang ON MBarang.IDJenis = mjenisbarang.NoID LEFT OUTER JOIN" & vbCrLf
            strsql &= " mjenisbarang MJenisKe ON MBarang.IDJenis = MJenisKe.NoID" & vbCrLf
            If tipe = 1 Then
                strsql &= " WHERE MBarang.IDJenis = " & NullTolong(txtJenisDari.EditValue)
                ExecuteDBGrid(GC1, strsql, "NoID")
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
                            Case "boolean"
                                .Columns(x).ColumnEdit = repChekEdit
                        End Select
                        If .Columns(x).FieldName.ToLower = "NamaJenisKe".ToLower Then
                            .Columns(x).ColumnEdit = repLkEdit
                            .Columns(x).Caption = "Ke Jenis Barang"
                        End If
                    Next
                End With
                GV1.HideFindPanel()
            ElseIf tipe = 2 Then
                strsql &= " WHERE MBarang.IDJenis = " & NullTolong(txtJenisKe.EditValue)
                ExecuteDBGrid(GC2, strsql, "NoID")
                With GV2
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
                            Case "boolean"
                                .Columns(x).ColumnEdit = repChekEdit
                        End Select
                    Next
                End With
                GV2.HideFindPanel()
            ElseIf tipe = 3 Then
                strsql = "SELECT TJenisBarang.*, MBarang.Nama AS NamaStock, MBarang.Kode AS KodeStock, MJenisBarang.Nama AS JenisBarang, MJenisKe.Kode AS KeJenisBarang " & vbCrLf & _
                         " FROM (TJenisBarang LEFT JOIN MBarang ON MBarang.NoID=TJenisBarang.IDBarang)" & vbCrLf & _
                         " LEFT JOIN MJenisBarang ON MJenisBarang.NoID=MBarang.IDJenis" & vbCrLf & _
                         " LEFT JOIN MJenisBarang MJenisKe ON MJenisKe.NoID=TJenisBarang.IDJenisBaru" & vbCrLf & _
                         " WHERE TJenisBarang.IDUSer=" & IDUserAktif & " AND TJenisBarang.IP='" & FixApostropi(IPLokal) & "'"
                ExecuteDBGrid(GridControl1, strsql, "NoID")
                With GridView1
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
                            Case "boolean"
                                .Columns(x).ColumnEdit = repChekEdit
                        End Select
                        If .Columns(x).FieldName.ToLower = "KeJenisBarang".ToLower Then
                            .Columns(x).ColumnEdit = repLkEdit
                            .Columns(x).Caption = "Ke Jenis Barang"
                        End If
                    Next
                End With
                GridView1.HideFindPanel()
            End If
        Catch ex As Exception
            XtraMessageBox.Show(ex.Message, ".:: Pesan Kesalahan ::.", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
        Windows.Forms.Cursor.Current = curentcursor
        Application.DoEvents()
    End Sub

    Private Sub BarButtonItem1_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem1.ItemClick
        cmdSave.PerformClick()
    End Sub

    Private Sub BarButtonItem3_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem3.ItemClick
        Dim xOtorisasi As New frmOtorisasiAdmin
        Try
            If XtraMessageBox.Show("Simpan Layout?", NamaAplikasi, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = Windows.Forms.DialogResult.Yes AndAlso xOtorisasi.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
                Ini.TulisIniPath(folderLayouts & Me.Name & ".ini", "Form", "Width", Me.Width)
                Ini.TulisIniPath(folderLayouts & Me.Name & ".ini", "Form", "Height", Me.Height)
                Me.Left = (Screen.PrimaryScreen.WorkingArea.Width - Me.Width) \ 2
                Me.Top = (Screen.PrimaryScreen.WorkingArea.Height - Me.Height) \ 2

                LayoutControl1.SaveLayoutToXml(folderLayouts & Me.Name & LayoutControl1.Name & ".xml")
                GV1.SaveLayoutToXml(folderLayouts & Me.Name & GV1.Name & ".xml")
                GV2.SaveLayoutToXml(FolderLayouts & Me.Name & GV2.Name & ".xml")
                GridView1.SaveLayoutToXml(FolderLayouts & Me.Name & GridView1.Name & ".xml")
            End If
        Catch ex As Exception
            XtraMessageBox.Show("Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            xOtorisasi.Dispose()
        End Try
    End Sub

    Private Sub BarButtonItem6_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem6.ItemClick
        RefreshDetil(1)
        RefreshDetil(2)
    End Sub

    Private Sub mnCancel_ItemClick(ByVal sender As Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles mnCancel.ItemClick
        SimpleButton1.PerformClick()
    End Sub

    Private Sub txtBarang_ButtonClick(ByVal sender As Object, ByVal e As DevExpress.XtraEditors.Controls.ButtonPressedEventArgs) Handles txtBarang.ButtonClick
        Select Case e.Button.Index
            Case 0
                InsertIntoDetil(txtBarang.Text)
                txtBarang.Text = ""
                txtBarang.Focus()
                Tgl.Text = ""
            Case 1
                txtBarang.Text = ""
        End Select
    End Sub

    Private Sub InsertIntoDetil(ByVal text As String)
        Dim ID As Long = -1
        Dim x As Boolean = False
        Dim SQL As String = ""
        Dim oDS As New DataSet
        Dim ds As New DataSet
        Try
            'Cari Nama Barang
            SQL = "SELECT NoID,Kode,Nama,IDJenis FROM MBarang WHERE NOT (IsNULL(MBarang.KODE,'')='') AND (UPPER(MBarang.Kode) = '" & text.Replace("'", "''").ToUpper & "' OR UPPER(MBarang.NAMA) = '" & text.Replace("'", "''").ToUpper & "')"
            oDS = ExecuteDataset("Tbl", SQL)
            For i As Integer = 0 To oDS.Tables(0).Rows.Count - 1
                ds = ExecuteDataset("Data", "SELECT TJenisBarang.* FROM TJenisBarang WHERE TJenisBarang.IDBarang=" & NullToLong(oDS.Tables(0).Rows(i).Item("NoID")) & " AND TJenisBarang.IDUser=" & IDUserAktif & " AND TJenisBarang.IP='" & FixApostropi(IPLokal) & "'")
                If ds.Tables(0).Rows.Count >= 1 Then
                Else
                    SQL = "INSERT INTO TJenisBarang (IDJenis,IDBarang,IDUser,IP) VALUES " & vbCrLf & _
                          " (" & NullToLong(oDS.Tables(0).Rows(i).Item("IDJenis")) & "," & NullToLong(oDS.Tables(0).Rows(i).Item("NoID")) & "," & NullToLong(IDUserAktif) & ",'" & FixApostropi(IPLokal) & "')"
                    EksekusiSQL(SQL)
                End If
            Next
            'Cari Jenis Barang
            SQL = "SELECT MBarang.NoID,MBarang.Kode,MBarang.Nama,MBarang.IDJenis FROM MBarang LEFT JOIN MJenisBarang ON MJenisBarang.NoID=MBarang.IDJenis WHERE NOT (IsNULL(MBarang.KODE,'')='') AND (UPPER(MJenisBarang.Kode) = '" & text.Replace("'", "''").ToUpper & "' OR UPPER(MJenisBarang.NAMA) = '" & text.Replace("'", "''").ToUpper & "')"
            oDS = ExecuteDataset("Tbl", SQL)
            For i As Integer = 0 To oDS.Tables(0).Rows.Count - 1
                ds = ExecuteDataset("Data", "SELECT TJenisBarang.* FROM TJenisBarang WHERE TJenisBarang.IDBarang=" & NullToLong(oDS.Tables(0).Rows(i).Item("NoID")) & " AND TJenisBarang.IDUser=" & IDUserAktif & " AND TJenisBarang.IP='" & FixApostropi(IPLokal) & "'")
                If ds.Tables(0).Rows.Count >= 1 Then
                Else
                    SQL = "INSERT INTO TJenisBarang (IDJenis,IDBarang,IDUser,IP) VALUES " & vbCrLf & _
                          " (" & NullToLong(oDS.Tables(0).Rows(i).Item("IDJenis")) & "," & NullToLong(oDS.Tables(0).Rows(i).Item("NoID")) & "," & NullToLong(IDUserAktif) & ",'" & FixApostropi(IPLokal) & "')"
                    EksekusiSQL(SQL)
                End If
            Next
            'Cari Kategori Barang
            SQL = "SELECT MBarang.NoID,MBarang.Kode,MBarang.Nama,MBarang.IDJenis FROM MBarang LEFT JOIN MKategori ON MKategori.NoID=MBarang.IDKategori WHERE NOT (IsNULL(MBarang.KODE,'')='') AND (UPPER(MKategori.Kode) = '" & text.Replace("'", "''").ToUpper & "' OR UPPER(MKategori.NAMA) = '" & text.Replace("'", "''").ToUpper & "')"
            oDS = ExecuteDataset("Tbl", SQL)
            For i As Integer = 0 To oDS.Tables(0).Rows.Count - 1
                ds = ExecuteDataset("Data", "SELECT TJenisBarang.* FROM TJenisBarang WHERE TJenisBarang.IDBarang=" & NullToLong(oDS.Tables(0).Rows(i).Item("NoID")) & " AND TJenisBarang.IDUser=" & IDUserAktif & " AND TJenisBarang.IP='" & FixApostropi(IPLokal) & "'")
                If ds.Tables(0).Rows.Count >= 1 Then
                Else
                    SQL = "INSERT INTO TJenisBarang (IDJenis,IDBarang,IDUser,IP) VALUES " & vbCrLf & _
                          " (" & NullToLong(oDS.Tables(0).Rows(i).Item("IDJenis")) & "," & NullToLong(oDS.Tables(0).Rows(i).Item("NoID")) & "," & NullToLong(IDUserAktif) & ",'" & FixApostropi(IPLokal) & "')"
                    EksekusiSQL(SQL)
                End If
            Next
            'Cari Pembelian
            SQL = "SELECT MBarang.NoID,MBarang.Kode,MBarang.Nama,MBarang.IDJenis FROM (MBeliD INNER JOIN MBeli ON MBeli.NoID=MBeliD.IDBeli) LEFT JOIN (MBarang LEFT JOIN MJenisBarang ON MJenisBarang.NoID=MBarang.IDJenis) ON MBarang.NoID=MBeliD.IDBarang WHERE (UPPER(MBeli.Kode) = '" & text.Replace("'", "''").ToUpper & "')" & vbCrLf & _
                  " GROUP BY MBarang.NoID,MBarang.Kode,MBarang.Nama,MBarang.IDJenis"
            oDS = ExecuteDataset("Tbl", SQL)
            For i As Integer = 0 To oDS.Tables(0).Rows.Count - 1
                ds = ExecuteDataset("Data", "SELECT TJenisBarang.* FROM TJenisBarang WHERE TJenisBarang.IDBarang=" & NullToLong(oDS.Tables(0).Rows(i).Item("NoID")) & " AND TJenisBarang.IDUser=" & IDUserAktif & " AND TJenisBarang.IP='" & FixApostropi(IPLokal) & "'")
                If ds.Tables(0).Rows.Count >= 1 Then
                Else
                    SQL = "INSERT INTO TJenisBarang (IDJenis,IDBarang,IDUser,IP) VALUES " & vbCrLf & _
                      " (" & NullToLong(oDS.Tables(0).Rows(i).Item("IDJenis")) & "," & NullToLong(oDS.Tables(0).Rows(i).Item("NoID")) & "," & NullToLong(IDUserAktif) & ",'" & FixApostropi(IPLokal) & "')"
                    EksekusiSQL(SQL)
                End If
            Next
            'Cari Tanggal Pembelian
            If Tgl.Text <> "" Then
                SQL = "SELECT MBarang.NoID,MBarang.Kode,MBarang.Nama,MBarang.IDJenis FROM (MBeliD INNER JOIN MBeli ON MBeli.NoID=MBeliD.IDBeli) LEFT JOIN (MBarang LEFT JOIN MJenisBarang ON MJenisBarang.NoID=MBarang.IDJenis) ON MBarang.NoID=MBeliD.IDBarang WHERE (MBeli.Tanggal >= '" & Tgl.DateTime.ToString("yyyy/MM/dd") & "' AND MBeli.Tanggal<'" & DateAdd(DateInterval.Day, 1, Tgl.DateTime).ToString("yyyy/MM/dd") & "')" & vbCrLf & _
                      " GROUP BY MBarang.NoID,MBarang.Kode,MBarang.Nama,MBarang.IDJenis"
                oDS = ExecuteDataset("Tbl", SQL)
                For i As Integer = 0 To oDS.Tables(0).Rows.Count - 1
                    ds = ExecuteDataset("Data", "SELECT TJenisBarang.* FROM TJenisBarang WHERE TJenisBarang.IDBarang=" & NullToLong(oDS.Tables(0).Rows(i).Item("NoID")) & " AND TJenisBarang.IDUser=" & IDUserAktif & " AND TJenisBarang.IP='" & FixApostropi(IPLokal) & "'")
                    If ds.Tables(0).Rows.Count >= 1 Then
                    Else
                        SQL = "INSERT INTO TJenisBarang (IDJenis,IDBarang,IDUser,IP) VALUES " & vbCrLf & _
                              " (" & NullToLong(oDS.Tables(0).Rows(i).Item("IDJenis")) & "," & NullToLong(oDS.Tables(0).Rows(i).Item("NoID")) & "," & NullToLong(IDUserAktif) & ",'" & FixApostropi(IPLokal) & "')"
                        EksekusiSQL(SQL)
                    End If
                Next
            End If
            RefreshDetil(3)
        Catch ex As Exception
            XtraMessageBox.Show("Info : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
        Finally
            oDS.Dispose()
            ds.Dispose()
        End Try
    End Sub

    Private Sub txtBarang_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtBarang.KeyDown
        If e.KeyCode = Keys.Enter Then
            InsertIntoDetil(txtBarang.Text)
            txtBarang.Text = ""
            Tgl.Text = ""
            txtBarang.Focus()
        ElseIf e.KeyCode = Keys.Escape Then
            txtBarang.Text = ""
        End If
    End Sub

    Private Sub Tgl_ButtonClick(ByVal sender As Object, ByVal e As DevExpress.XtraEditors.Controls.ButtonPressedEventArgs) Handles Tgl.ButtonClick
        Select Case e.Button.Index
            Case 1
                EksekusiSQL("DELETE FROM TJenisBarang WHERE IDUser=" & IDUserAktif & " AND IP='" & FixApostropi(IPLokal) & "'")
                InsertIntoDetil(txtBarang.Text)
                txtBarang.Text = ""
                txtBarang.Focus()
                Tgl.Text = ""
        End Select
    End Sub

    Private Sub Tgl_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Tgl.KeyDown
        If e.KeyCode = Keys.Enter Then
            InsertIntoDetil(txtBarang.Text)
            txtBarang.Text = ""
            Tgl.Text = ""
            txtBarang.Focus()
        ElseIf e.KeyCode = Keys.Escape Then
            txtBarang.Text = ""
        End If
    End Sub

    Private Sub mnRubahKejenis_ItemClick(ByVal sender As Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles mnRubahKejenis.ItemClick
        Dim frLookUp As New frmLookup
        'Dim KodeJenis As String = ""
        frLookUp.Strsql = "SELECT MJenisBarang.NoID, MJenisBarang.Kode, MJenisBarang.Nama FROM MJenisBarang WHERE IsActive=1"
        frLookUp.FormName = "frmLookUpJenisBarang"
        If frLookUp.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
            'KodeJenis = NullToStr(EksekusiSQlSkalarNew("SELECT Kode FROM MJenisBarang WHERE NoID=" & NullToLong(frLookUp.NoID)))
            For Each i In GridView1.GetSelectedRows
                GridView1.SetRowCellValue(i, "KeJenisBarang", NullToLong(frLookUp.NoID))
            Next
        End If
        frLookUp.Dispose()
    End Sub

    Private Sub GridView1_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles GridView1.MouseDown
        Dim View As GridView = CType(sender, GridView)
        If View Is Nothing Then Return
        ' obtaining hit info
        Dim hitInfo As GridHitInfo = View.CalcHitInfo(New System.Drawing.Point(e.X, e.Y))
        If (e.Button = Windows.Forms.MouseButtons.Right) And (hitInfo.InRow) And _
          (Not View.IsGroupRow(hitInfo.RowHandle)) Then
            PopupMenu1.ShowPopup(Control.MousePosition)
        End If
    End Sub

    Private Sub GridControl1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles GridControl1.Click

    End Sub

    Private Sub txtBarang_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtBarang.EditValueChanged

    End Sub
End Class