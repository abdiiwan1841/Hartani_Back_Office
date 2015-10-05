'Imports MySql.Data.MySqlClient
Imports DevExpress.XtraEditors
Imports DevExpress.XtraGrid.Views.Base
Imports DevExpress.XtraEditors.Repository
Imports VPoint.FungsiControl

Public Class frmEntriJU
    Public NoID As Long
    Private oldTanggal As DateTime
    Private oldNota As String
    Private oldNosp As String
    Dim KodeLama As String = ""
    Dim KodeReffLama As String = ""
    Public pStatus As mdlAccPublik.ptipe
    Public TglDefault As DateTime = TanggalSystem

    Dim repckedit As New RepositoryItemCheckEdit
    Dim repdateedit As New RepositoryItemDateEdit
    Dim reptextedit As New RepositoryItemTextEdit
    Dim reppicedit As New RepositoryItemPictureEdit

    Private Sub SimpleButton1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SimpleButton1.Click
        DialogResult = Windows.Forms.DialogResult.Cancel
        EksekusiSQL("DELETE FROM TJurnalD WHERE IDTerminal=" & IDUserAktif)
        Me.Close()
    End Sub

    Private Sub SimpleButton9_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SimpleButton9.Click
        If Simpan() Then
            DialogResult = Windows.Forms.DialogResult.OK
            Me.Close()
        End If
    End Sub
    Function isValidasi() As Boolean
        If Trim(txtKode.Text) = "" Then
            FxMessage("Kode Harus diisi!", "Kode harus ada")
            isValidasi = False
            Exit Function
        End If
        If CekKodeValid(txtKode.Text, KodeLama, "MJurnal", "Kode", IIf(pStatus = mdlAccPublik.ptipe.Baru, False, True)) Then
            FxMessage("Kode sudah dipakai.", NamaAplikasi)
            isValidasi = False
            Exit Function
        End If
        If CekKodeValid(txtKodeReff.Text, KodeReffLama, "MJurnal", "KodeReff", IIf(pStatus = mdlAccPublik.ptipe.Baru, False, True)) AndAlso XtraMessageBox.Show("Kode Reff sudah dipakai.", NamaAplikasi, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) = Windows.Forms.DialogResult.No Then
            'FxMessage("Kode Reff sudah dipakai.", NamaAplikasi)
            txtKodeReff.Focus()
            isValidasi = False
            Exit Function
        End If
        isValidasi = True
        If EksekusiSQlSkalarNew("select sum(DebetA)-Sum(KreditA) as Selisih from TJurnalD where IDTerminal=" & IDUserAktif) <> 0 Then
            If FxMessage("Jurnal masih belum balance!", NamaAplikasi, MessageBoxButtons.YesNo) = Windows.Forms.DialogResult.No Then
                isValidasi = False
                Exit Function
            End If
        End If
    End Function
    Function Simpan() As Boolean
        Dim curentcursor As Cursor = Windows.Forms.Cursor.Current
        Windows.Forms.Cursor.Current = Cursors.WaitCursor
        Dim x As Boolean = False
        Try
            Dim SQL As String = ""
            Dim view As ColumnView = GridControl1.FocusedView
            If isValidasi() Then
                If pStatus = mdlAccPublik.ptipe.Baru Then
                    NoID = GetNewID("Mjurnal", "ID")
                    SQL = "Insert Into MJurnal(ID,Tanggal,Kode,Keterangan," & _
                          "IDDepartemenuser,IDJenisTransaksi,IDUserEntry,IDUserPosting,IDTransaksi,IsPosting,KodeReff) Values(" & _
                          NoID & ",'" & Format(Tgl.DateTime, "yyyy/MM/dd") & "','" & FixApostropi(txtKode.Text) & "','" & FixApostropi(txtCatatan.Text) & "'," & _
                          DefIDDepartemen & ",1," & IDUserAktif & ",-1," & NoID & ",0,'" & FixApostropi(txtKodeReff.Text) & "')"
                ElseIf pStatus = mdlAccPublik.ptipe.Edit Then
                    SQL = "update MJurnal SET " & _
                          " Tanggal='" & Format(Tgl.DateTime, "yyyy/MM/dd") & "'," & _
                          " Kode='" & FixApostropi(txtKode.Text) & "'," & _
                          " KodeReff='" & FixApostropi(txtKodeReff.Text) & "'," & _
                          " Keterangan='" & FixApostropi(txtCatatan.Text) & "'," & _
                          " IDUserEntry=" & IDUserAktif & ", " & _
                          " IDDepartemenuser=" & DefIDDepartemen & " " & _
                          " WHERE ID=" & NoID
                End If
                EksekusiSQL(SQL)
                InsertkanDetil()
                If defAutoPosting Then
                    SQL = "update MJurnal SET " & _
                          " IDUserPosting=" & IDUserAktif & ", TanggalPosting=Tanggal," & _
                          " IsPosting=1 " & _
                          " WHERE ID=" & NoID
                    EksekusiSQL(SQL)
                End If
                x = True
            Else
                x = False
            End If
        Catch Ex As Exception
            FxMessage(Ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error, , , Ex.StackTrace)
            x = False
        End Try
        Windows.Forms.Cursor.Current = curentcursor
        Return x
    End Function

    Sub RefreshItem()
        Dim strcekrecord As String
        Dim DS As New DataSet
        Dim namaTabel As String = "TJurnalD"
        Try
            strcekrecord = "SELECT " & namaTabel & ".*,MAkun.Kode as KodeAkun,MAkun.Nama as NamaAkun,MMataUang.Kode as MataUang, " & _
                           " MGudang.Nama AS Gudang, MWilayah.Nama AS Wilayah," & _
                           " MDepartemen.Nama AS Departemen " & _
                           " FROM ((" & namaTabel & vbCrLf & _
                           " LEFT JOIN MAkun ON " & namaTabel & ".IDAkun=MAkun.ID " & vbCrLf & _
                           " LEFT JOIN MMataUang ON " & namaTabel & ".IDMataUang=MMataUang.ID " & vbCrLf & _
                           " LEFT JOIN MGudang ON MGudang.NoID=" & namaTabel & ".IDGudang) " & vbCrLf & _
                           " LEFT JOIN MWilayah ON MWilayah.NoID=" & namaTabel & ".IDWilayah) " & vbCrLf & _
                           " LEFT JOIN MDepartemen ON MDepartemen.NoID=" & namaTabel & ".IDDepartemen " & vbCrLf & _
                           " WHERE " & namaTabel & ".IDTerminal = " & IDUserAktif
            DS = ExecuteDataset(namaTabel, strcekrecord)
            GridControl1.DataSource = DS.Tables(namaTabel)
            GridView1.ShowFindPanel()
            With GridView1
                For i As Integer = 0 To .Columns.Count - 1
                    Select Case .Columns(i).ColumnType.Name.ToLower
                        Case "int32", "int64", "int"
                            .Columns(i).DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
                            .Columns(i).DisplayFormat.FormatString = "n0"
                        Case "decimal", "single", "money", "double"
                            .Columns(i).DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
                            .Columns(i).DisplayFormat.FormatString = "n2"
                        Case "string"
                            .Columns(i).DisplayFormat.FormatType = DevExpress.Utils.FormatType.None
                            .Columns(i).DisplayFormat.FormatString = ""
                        Case "date", "datetime"
                            If .Columns(i).FieldName.Trim.ToLower = "jam" Then
                                .Columns(i).DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime
                                .Columns(i).DisplayFormat.FormatString = "HH:mm"
                            Else
                                .Columns(i).DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime
                                .Columns(i).DisplayFormat.FormatString = "dd-MM-yyyy"
                            End If
                        Case "byte[]"
                            reppicedit.SizeMode = DevExpress.XtraEditors.Controls.PictureSizeMode.Squeeze
                            .Columns(i).OptionsColumn.AllowGroup = False
                            .Columns(i).OptionsColumn.AllowSort = False
                            .Columns(i).OptionsFilter.AllowFilter = False
                            .Columns(i).ColumnEdit = reppicedit
                        Case "boolean"
                            .Columns(i).ColumnEdit = repckedit
                    End Select

                    If .Columns(i).FieldName.Trim.ToLower = "debet" Or .Columns(i).FieldName.Trim.ToLower = "kredit" Then
                        .Columns(i).SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum
                        .Columns(i).SummaryItem.DisplayFormat = "{0:n2}"
                        .Columns(i).SummaryItem.Tag = 0
                    End If
                Next
            End With
        Catch ex As Exception
            FxMessage(ex.Message, ".:: Pesan Kesalahan ::.", MessageBoxButtons.OK, MessageBoxIcon.Error, , , ex.StackTrace)
        End Try
    End Sub
    Private Sub frBayarHutang_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        GridView1.SaveLayoutToXml(LayOutKu(Me.Name & GridView1.Name))
    End Sub
    Private Sub SetCtlMe()
        SetForm(Me)
        SetButton(cmdNew, button_.cmdNew)
        SetButton(cmdEdit, button_.cmdEdit)
        SetButton(cmdDelete, button_.cmdDelete)
        SetButton(SimpleButton9, button_.cmdSave)
        SetButton(SimpleButton1, button_.cmdCancelSave)
        SetButton(cmdRefresh, button_.cmdRefresh)
        'If pStatus = publik.ptipe.Baru Then
        '    cbDari.Enabled = True
        '    txtAlamat.Enabled = True
        'Else
        '    cbDari.Enabled = False
        '    txtAlamat.Enabled = False
        'End If
        If Not SimpleButton9.Enabled Then
            cmdNew.Enabled = False
            cmdEdit.Enabled = False
            cmdDelete.Enabled = False
        End If

    End Sub
    Private Sub frBayarHutang_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        SetCtlMe()
        TampilData()
        SetForm(Me)
        If System.IO.File.Exists(LayOutKu(Me.Name & GridView1.Name)) Then
            GridView1.RestoreLayoutFromXml(LayOutKu(Me.Name & GridView1.Name))
        End If
    End Sub

    Sub IsiDefault()
        Tgl.DateTime = TglDefault
        pStatus = mdlAccPublik.ptipe.Baru
    End Sub

    'Sub NotaBaru()
    '    txtKode.Text = GetKodeKasIN(Tgl.DateTime, IDAkun)
    'End Sub
    Sub NotaBaru()
        If pStatus = mdlAccPublik.ptipe.Baru Then
            txtKode.Text = GetKode("MJurnal", "JU", Month(Tgl.DateTime), Year(Tgl.DateTime))
        Else
            If oldTanggal.Year = Tgl.DateTime.Year AndAlso oldTanggal.Month = Tgl.DateTime.Month Then
                txtKode.Text = oldNota
            Else
                txtKode.Text = GetKode("MJurnal", "JU", Month(Tgl.DateTime), Year(Tgl.DateTime))
            End If
        End If
    End Sub
    Sub TampilData()
        If pStatus = mdlAccPublik.ptipe.Baru Then
            IsiDefault()
            NotaBaru()
            TampilkankanDetil()
        ElseIf pStatus = mdlAccPublik.ptipe.Edit Then
            Dim oDS As New DataSet
            Dim strsql As String
            Dim curentcursor As Windows.Forms.Cursor = Windows.Forms.Cursor.Current
            Dim dlg As DevExpress.Utils.WaitDialogForm
            dlg = New DevExpress.Utils.WaitDialogForm("Mempersiapkan data." & vbCrLf & "Mohon tunggu beberapa saat ...", NamaAplikasi)
            dlg.TopMost = True
            dlg.Show()
            Windows.Forms.Cursor.Current = Windows.Forms.Cursors.WaitCursor
            strsql = "SELECT MJurnal.*  " & _
            "From MJurnal Where MJurnal.ID= " & NoID.ToString
            Try
                oDS = ExecuteDataset("MTable", strsql)
                If oDS.Tables("MTable").Rows.Count = 0 Then
                    IsiDefault()
                Else
                    txtKode.Text = NullToStr(oDS.Tables("MTable").Rows(0).Item("Kode"))
                    oldNota = NullToStr(oDS.Tables("MTable").Rows(0).Item("Kode"))
                    Tgl.EditValue = oDS.Tables("MTable").Rows(0).Item("Tanggal")
                    txtKodeReff.Text = NullToStr(oDS.Tables("MTable").Rows(0).Item("KodeReff"))
                    txtCatatan.Text = NullToStr(oDS.Tables("MTable").Rows(0).Item("Keterangan"))
                    oldTanggal = oDS.Tables("MTable").Rows(0).Item("Tanggal")
                    KodeLama = txtKode.Text
                    KodeReffLAma = txtKodeReff.Text
                End If
                TampilkankanDetil()
            Catch ex As Exception
                FxMessage(ex.Message, ".:: Pesan Kesalahan ::.", Windows.Forms.MessageBoxButtons.OK + Windows.Forms.MessageBoxButtons.OK, Windows.Forms.MessageBoxIcon.Error)
            Finally
                oDS.Dispose()
            End Try
            Windows.Forms.Cursor.Current = curentcursor
            dlg.Close()
            dlg.Dispose()
        End If
    End Sub

    Private Sub InsertkanDetil()
        Dim SQL As String = ""
        Try
            EksekusiSQL("DELETE FROM MJurnalD WHERE IDJurnal=" & NoID)
            SQL = "INSERT INTO MJurnalD " & vbCrLf
            SQL = SQL & " (IDJurnal, IDGudang, IDWilayah,IDDepartemen, IDAkun, IDMataUang, Kurs, Debet, Kredit, DebetA, KreditA, IDTransaksi, " & vbCrLf
            SQL = SQL & " Keterangan)" & vbCrLf
            SQL = SQL & " SELECT " & NoID & ", IDGudang,IDWilayah,IDDepartemen, IDAkun, IDMataUang, Kurs, Debet, Kredit, DebetA, KreditA, IDTransaksi, " & vbCrLf
            SQL = SQL & " Keterangan " & vbCrLf
            SQL = SQL & " FROM TJurnalD"
            SQL = SQL & " WHERE TJurnalD.IDTerminal = " & IDUserAktif & " ORDER BY TJurnalD.ID"
            EksekusiSQL(SQL)
            EksekusiSQL("DELETE FROM TJurnalD WHERE IDTerminal=" & IDUserAktif)
        Catch ex As Exception
            FxMessage("Error : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error, , , ex.StackTrace)
        End Try
    End Sub
    Private Sub TampilkankanDetil()
        Dim SQL As String = ""
        Dim oDS As New DataSet
        Try
            SQL = "SELECT * FROM TJurnalD WHERE IDTerminal=" & IDUserAktif
            oDS = ExecuteDataset("tbl", SQL)
            If oDS.Tables(0).Rows.Count >= 1 Then
                If FxMessage("Masih ada data temporary, ingin ditampilkan ?", NamaAplikasi, MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.No Then
                    EksekusiSQL("DELETE FROM TJurnalD WHERE IDTerminal=" & IDUserAktif)
                End If
            End If
            SQL = "INSERT INTO TJurnalD " & vbCrLf
            SQL = SQL & " (IDGudang, IDDepartemen, IDWilayah, IDAkun, IDMataUang,  Kurs, Debet, Kredit, DebetA, KreditA, IDTransaksi, IDTerminal, " & vbCrLf
            SQL = SQL & " Keterangan)" & vbCrLf
            SQL = SQL & " SELECT IDGudang, IDDepartemen, IDWilayah, IDAkun, IDMataUang, Kurs, Debet, Kredit, DebetA, KreditA, IDTransaksi, " & IDUserAktif & ", " & vbCrLf
            SQL = SQL & " Keterangan" & vbCrLf
            SQL = SQL & " FROM MJurnalD "
            SQL = SQL & " WHERE MJurnalD.IDJurnal= " & NoID & " ORDER BY MJurnalD.ID"
            EksekusiSQL(SQL)
        Catch ex As Exception
            FxMessage("Error : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error, , , ex.StackTrace)
        Finally
            oDS.Dispose()
            RefreshItem()
        End Try
    End Sub

    Sub GenerateJumlah()
        'If FxMessage("Apakah anda yakin mau generate pembayaran per nota?", NamaApplikasi,MessageBoxButtons.YesNo,MessageBoxIcon.Question) =Windows.Forms.DialogResult.Yes Then
        '    Dim i As Integer = 0
        '    Dim Sisa As Double
        '    Dim DiscountRp As Double
        '    Dim DiscountProsen As Double
        '    Dim view As ColumnView = GridControl1.FocusedView
        '    Sisa = Jumlah.EditValue
        '    DiscountRp = txtDiscount.EditValue
        '    If Sisa = 0 Then DiscountProsen = 100 Else DiscountProsen = DiscountRp * 100 / (Sisa + DiscountRp)

        '    While i < view.RowCount 'Sisa > 0 And
        '        If view.GetRowCellValue(i, "Sisa") > (Sisa + DiscountRp) Then
        '            view.SetRowCellValue(i, "Bayar", Sisa)
        '            view.SetRowCellValue(i, "Retur", DiscountRp)
        '            Sisa = 0
        '            DiscountRp = 0
        '        Else
        '            view.SetRowCellValue(i, "Retur", view.GetRowCellValue(i, "Sisa") * DiscountProsen / 100)
        '            view.SetRowCellValue(i, "Bayar", view.GetRowCellValue(i, "Sisa") - view.GetRowCellValue(i, "Sisa") * DiscountProsen / 100)

        '            Sisa = Sisa - view.GetRowCellValue(i, "Bayar")
        '            DiscountRp = DiscountRp - view.GetRowCellValue(i, "Retur")
        '        End If
        '        i = i + 1
        '    End While
        'End If
    End Sub

    Function GetKode(ByVal NmTable As String, ByVal KodeNota As String, ByVal Bulan As Integer, ByVal Tahun As Integer) As String
        Dim curentcursor As Cursor = Windows.Forms.Cursor.Current
        Windows.Forms.Cursor.Current = Cursors.WaitCursor
        Dim Id As Long
        Id = NullToLong(EksekusiSQLSkalar("Select count(ID) as x from " & NmTable & " where month(tanggal)=" & Bulan & " and year(tanggal)=" & Tahun)) + 1
        Windows.Forms.Cursor.Current = curentcursor
        Return KodeNota & Format(Id, "0000") & "/" & Format(Bulan, "00") & "/" & Format(Tahun, "0000")
    End Function

    Private Sub Tgl_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Tgl.EditValueChanged

    End Sub

    Private Sub cmdNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdNew.Click
        Try
            Dim x As New frmEntriJUD
            x.pStatus = mdlAccPublik.ptipe.Baru
            x.txtCatatan.Text = txtCatatan.Text
            If x.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
                RefreshItem()
                GridView1.ClearSelection()
                GridView1.FocusedRowHandle = GridView1.LocateByDisplayText(0, GridView1.Columns("IDAkun"), x.IDAkun.ToString("#,##0"))
                GridView1.SelectRow(GridView1.FocusedRowHandle)

                'txtCatatan.Text = ""
                'For i As Integer = 0 To GridView1.RowCount - 1
                '    txtCatatan.Text &= IIf(i <> 0, ", ", "") & GridView1.GetRowCellValue(i, "Keterangan").ToString.Trim
                'Next
            End If
            x.Close()
            x.Dispose()
        Catch ex As Exception
            FxMessage(ex.Message, ".:: Pesan Kesalahan ::.", MessageBoxButtons.OK, MessageBoxIcon.Error, , , ex.StackTrace)
        End Try
    End Sub


    Private Sub cmdEdit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdEdit.Click
        Try
            Dim IDJurnalD As Long
            Dim view As ColumnView = GridControl1.FocusedView
            Dim row As System.Data.DataRow = view.GetDataRow(GridView1.FocusedRowHandle)
            Dim x As New frmEntriJUD
            IDJurnalD = NullToLong(row("ID"))
            x.ID = IDJurnalD
            x.pStatus = mdlAccPublik.ptipe.Edit
            If x.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
                RefreshItem()
                GridView1.ClearSelection()
                GridView1.FocusedRowHandle = GridView1.LocateByDisplayText(0, GridView1.Columns("ID"), x.ID.ToString("#,##0"))
                GridView1.SelectRow(GridView1.FocusedRowHandle)
                'txtCatatan.Text = ""
                'For i As Integer = 0 To GridView1.RowCount - 1
                '    txtCatatan.Text &= IIf(i <> 0, ", ", "") & GridView1.GetRowCellValue(i, "Keterangan").ToString.Trim
                'Next
            End If
            x.Close()
            x.Dispose()
        Catch ex As Exception
            FxMessage(ex.Message, ".:: Pesan Kesalahan ::.", MessageBoxButtons.OK, MessageBoxIcon.Error, , , ex.StackTrace)
        End Try
    End Sub

    Private Sub cmdRefresh_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdRefresh.Click
        RefreshItem()
    End Sub

    Private Sub cmdDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdDelete.Click
        Try
            Dim IDJurnalD As Long
            Dim view As ColumnView = GridControl1.FocusedView
            Dim row As System.Data.DataRow = view.GetDataRow(GridView1.FocusedRowHandle)
            IDJurnalD = NullToLong(row("ID"))
            If FxMessage("Apa item ini akan dihapus?", NamaAplikasi, MessageBoxButtons.YesNo) = Windows.Forms.DialogResult.Yes Then
                EksekusiSQL("DELETE FROM TJurnalD WHERE ID=" & IDJurnalD)
                RefreshItem()
                'txtCatatan.Text = ""
                'For i As Integer = 0 To GridView1.RowCount - 1
                '    txtCatatan.Text &= IIf(i <> 0, ", ", "") & GridView1.GetRowCellValue(i, "Keterangan").ToString.Trim
                'Next
            End If
        Catch ex As Exception
            FxMessage(ex.Message, ".:: Pesan Kesalahan ::.", MessageBoxButtons.OK, MessageBoxIcon.Error, , , ex.StackTrace)
        End Try
    End Sub

    Private Sub ckBon_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        If pStatus = mdlAccPublik.ptipe.Baru Then
            NotaBaru()
        End If
    End Sub

    Private Sub Tgl_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles Tgl.LostFocus
        NotaBaru()
    End Sub
    Private Sub txtAlamat_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        NotaBaru()
    End Sub
End Class