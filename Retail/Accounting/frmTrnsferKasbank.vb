 
Imports DevExpress.XtraGrid.Views.Base 
Public Class frmTransferKasBank
    Public NoID As Long

    Private IDAkun, IDKasBank As Long
    Private oldTanggal As DateTime
    Private oldNota As String
    Private oldNosp As String
    Dim KodeLama As String
    Public pStatus As mdlAccPublik.Ptipe
    Dim IDGudang As Long


    Private Sub SimpleButton1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SimpleButton1.Click
        DialogResult = Windows.Forms.DialogResult.Cancel
        EksekusiSQL("DELETE FROM TKasIND WHERE IDTerminal=" & IDUserAktif)
        Me.Close()
    End Sub

    Private Sub SimpleButton9_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SimpleButton9.Click
        'HitungTotal()
        If Simpan() Then
            DialogResult = Windows.Forms.DialogResult.OK
            Me.Close()
        End If
    End Sub
    Private Sub HitungTotal()
        Dim dbs As New DataSet
        Dim str As String
        str = "Select SUM(Debet) AS JumDebet, SUM(ISNULL(Kurs, 0) * ISNULL(Kredit, 0)) AS JumKredit FROM TKasIND WHERE IDTerminal=" & IDUserAktif
        Try
            ' EksekusiSQl("Update TKasIND SET Kurs=" & FixKoma(txtNilaiTukar.EditValue) & " WHERE IDTerminal=" & IDUserAktif)
            dbs = ExecuteDataset("tabel", str)
            txtJumlah.EditValue = NullToDbl(dbs.Tables("tabel").Rows(0).Item("JumDebet"))
            dbs.Dispose()
        Catch ex As Exception
            FxMessage(ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error, , , ex.StackTrace)
            dbs.Dispose()
        End Try
    End Sub
    Function isValidasi() As Boolean
        If Trim(txtKode.Text) = "" Then
            msgbox("Kode Harus diisi!", "Kode harus ada")
            isValidasi = False
            Exit Function
        End If

        If CekKodeValid(txtKode.Text, KodeLama, "MKasIn", "Kode", IIf(pStatus = mdlAccPublik.ptipe.Baru, False, True)) Then
            MsgBox("Kode sudah dipakai.", MsgBoxStyle.Exclamation, NamaAplikasi)
            isValidasi = False
            Exit Function
        End If
        isValidasi = True
    End Function
    Function Simpan() As Boolean
        Dim curentcursor As Cursor = Windows.Forms.Cursor.Current
        Windows.Forms.Cursor.Current = Cursors.WaitCursor
        Dim x As Boolean
        Try
            Dim SQL As String = ""
            Dim view As ColumnView = GridControl1.FocusedView
            HitungTotal()
            If isValidasi() Then
                If pStatus = mdlAccPublik.ptipe.Baru Then
                    NoID = GetNewID("MKasIN", "ID")
                    SQL = "Insert Into MKasIN (IDGudang,ID,Tanggal,Kode,Keterangan,Jumlah," & _
                          "IsPosted,IDAkunKas,IDKas,KodeReff) Values(" & _
                          IDGudang & "," & NoID & ",'" & Format(Tgl.DateTime, "yyyy/MM/dd") & "','" & FixApostropi(txtKode.Text) & "','" & FixApostropi(txtCatatan.Text) & "'," & _
                          FixKoma(txtJumlah.EditValue) & ",0," & IDAkun & "," & IDKasBank & ",'" & FixApostropi(txtKodeReff.Text) & "')"
                ElseIf pStatus = mdlAccPublik.ptipe.Edit Then
                    SQL = "update MKasIN SET " & _
                          " Tanggal='" & Format(Tgl.DateTime, "yyyy/MM/dd") & "'," & _
                          " Kode='" & FixApostropi(txtKode.Text) & "'," & _
                          " KodeReff='" & FixApostropi(txtKodeReff.Text) & "'," & _
                          " Keterangan='" & FixApostropi(txtCatatan.Text) & "'," & _
                          " Jumlah=" & FixKoma(txtJumlah.EditValue) & ", " & _
                          " IDAkunKas= " & IDAkun & "," & _
                          " IDKas= " & IDKasBank & "," & _
                          " IDGudang= " & IDGudang & " " & _
                           " WHERE ID=" & NoID
                End If
                EksekusiSQL(SQL)
                InsertkanDetil()

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

    Private Sub txtKas_ButtonClick(ByVal sender As Object, ByVal e As DevExpress.XtraEditors.Controls.ButtonPressedEventArgs) Handles txtKas.ButtonClick
        Select Case e.Button.Index
            Case 0
                Dim x As New frmLUKas : x.pStatus = mdlAccPublik.ptipe.LookUp
                If x.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
                    IDKasBank = x.NoID
                    IDAkun = x.IDAKun
                    txtKas.Text = x.Nama '& " - " & x.SubKlas
                    IDGudang = x.IDGudang
                    If pStatus = mdlAccPublik.ptipe.Baru Then
                        NotaBaru()
                    End If
                End If
                x.Dispose()
            Case 1
                IDKasBank = -1
                IDAkun = -1
                txtKas.Text = ""
                IDGudang = -1
        End Select
        NotaBaru()
    End Sub

    Sub RefreshItem()
        Dim strsql As String
        strsql = "SELECT" & vbCrLf
        strsql = strsql & " TKasIND.ID," & vbCrLf
        strsql = strsql & " TKasIND.IDKasIN," & vbCrLf
        strsql = strsql & " MGudang.Nama AS NamaGudang," & vbCrLf
        strsql = strsql & " TKasIND.IDAkun," & vbCrLf
        strsql = strsql & " TKasIND.NamaAkun," & vbCrLf
        strsql = strsql & " TKasIND.Debet," & vbCrLf
        strsql = strsql & " TKasIND.Kredit," & vbCrLf
        strsql = strsql & " TKasIND.IDTransaksi," & vbCrLf
        strsql = strsql & " TKasIND.IDTerminal," & vbCrLf
        strsql = strsql & " TKasIND.Keterangan," & vbCrLf
        strsql = strsql & " TKasIND.NoPOD," & vbCrLf
        strsql = strsql & " makun.Nama AS Perkiraan" & vbCrLf
        strsql = strsql & " FROM" & vbCrLf
        strsql = strsql & " TKasIND " & vbCrLf
        strsql = strsql & " LEFT JOIN mgudang ON TKasIND.idgudang = mgudang.NoID" & vbCrLf
        strsql = strsql & " LEFT JOIN makun ON TKasIND.IDAkun = makun.ID where TKasIND.idterminal=" & IDUserAktif
        Try
            ExecuteDBGrid(GridControl1, strsql)
        Catch ex As Exception
            FxMessage(ex.Message, ".:: Pesan Kesalahan ::.", MessageBoxButtons.OK, MessageBoxIcon.Error, , , ex.StackTrace)
        End Try
        HitungTotal()

    End Sub
    Private Sub frBayarHutang_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        GridView1.SaveLayoutToXml(Application.StartupPath & "\system\layouts\" & (Me.Name & GridView1.Name) & ".xml")
    End Sub
    Private Sub SetCtlMe()


        If txtKas.Text = "" Then
            IDKasBank = DefIDKasBank
            txtKas.Text = DefNamaKasBank
            IDGudang = DefIDGudang
        End If
        If Not SimpleButton9.Enabled Then
            cmdNew.Enabled = False
            cmdEdit.Enabled = False
            cmdDelete.Enabled = False
        End If

    End Sub
    Private Sub frBayarHutang_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        SetCtlMe()
        TampilData()
        If System.IO.File.Exists(Application.StartupPath & "\system\layouts\" & (Me.Name & GridView1.Name) & ".xml") Then
            GridView1.RestoreLayoutFromXml(Application.StartupPath & "\system\layouts\" & (Me.Name & GridView1.Name) & ".xml")
        End If
    End Sub

    Sub IsiDefault()
        Tgl.EditValue = Today
        pStatus = mdlAccPublik.ptipe.Baru
        IDAkun = -1
        'IDKasBank = -1
        'txtKas.Text = ""
    End Sub

    'Sub NotaBaru()
    '    txtKode.Text = GetKodeKasIN(Tgl.DateTime, IDAkun)
    'End Sub
    Sub NotaBaru()
        If pStatus = mdlAccPublik.ptipe.Baru Then
            'Dim IsKas As Boolean = NullTobool(EksekusiSQlSkalarNew("SELECT IsKas FROM MKas WHERE ID=" & IDKasBank))
            'txtKode.Text = VPOINT.Posting.PostingKode.GetKode("MKasIN", IIf(IsKas, "KasMasuk", "BankMasuk"), NullToDate(Tgl.DateTime), IDAlamat, IDAlamat, IDAlamat, IDGudang, DefIDWilayah, DefIDDepartemen, , "(IsKembaliUM=0 OR IsKembaliUM IS NULL)")
        Else
            If oldTanggal.Year = Tgl.DateTime.Year AndAlso oldTanggal.Month = Tgl.DateTime.Month Then
                txtKode.Text = oldNota
            Else
                'Dim IsKas As Boolean = NullTobool(EksekusiSQlSkalarNew("SELECT IsKas FROM MKas WHERE ID=" & IDKasBank))
                'txtKode.Text = VPOINT.Posting.PostingKode.GetKode("MKasIN", IIf(IsKas, "KasMasuk", "BankMasuk"), NullToDate(Tgl.DateTime), IDAlamat, IDAlamat, IDAlamat, IDGudang, DefIDWilayah, DefIDDepartemen, , "(IsKembaliUM=0 OR IsKembaliUM IS NULL)")
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
            strsql = "SELECT MKasIN.*,MAkun.Kode AS KodeAkun, MKas.Kode AS KodeKas, MKas.Nama AS NamaKas , MAkun.Nama AS NamaAkun, mklasakun.Nama AS NamaSubKlasAkun " & vbCrLf
            strsql = strsql & " FROM (MKasIN LEFT JOIN " & vbCrLf
            strsql = strsql & " (MAkun LEFT JOIN mklasakun ON makun.IDSubKlasAkun = mklasakun.ID) ON MKasIN.IDAkunKas = MAkun.ID) LEFT JOIN " & vbCrLf
            strsql = strsql & " MBank MKas ON MKasIN.IDKas = Mkas.NoID  Where MKasIN.ID= " & NoID.ToString
            Try
                oDS = ExecuteDataset("MTable", strsql)
                If oDS.Tables("MTable").Rows.Count = 0 Then
                    IsiDefault()
                Else
                    IDKasBank = NullToLong(oDS.Tables("MTable").Rows(0).Item("IDKas"))
                    IDAkun = NullToLong(oDS.Tables("MTable").Rows(0).Item("IDAkunKas"))
                    txtKas.Text = NullToStr(oDS.Tables("MTable").Rows(0).Item("KodeKas")) & " - " & NullToStr(oDS.Tables("MTable").Rows(0).Item("NamaKas"))
                    txtKode.Text = NullToStr(oDS.Tables("MTable").Rows(0).Item("Kode"))
                    oldNota = NullToStr(oDS.Tables("MTable").Rows(0).Item("Kode"))
                    Tgl.EditValue = oDS.Tables("MTable").Rows(0).Item("Tanggal")
                    txtKodeReff.Text = NullToStr(oDS.Tables("MTable").Rows(0).Item("KodeReff"))
                    txtCatatan.Text = NullToStr(oDS.Tables("MTable").Rows(0).Item("Keterangan"))
                    oldTanggal = oDS.Tables("MTable").Rows(0).Item("Tanggal")
                    txtJumlah.EditValue = NullToDbl(oDS.Tables("MTable").Rows(0).Item("Jumlah"))
                    KodeLama = txtKode.Text
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
            EksekusiSQL("DELETE FROM MKasIND WHERE IDKasIN=" & NoID)
            SQL = "INSERT INTO MKasIND " & vbCrLf
            SQL = SQL & " (IDKasIN, IDGudang, IDAkun, KodeAkun, NamaAkun,  Debet, Kredit,IDTransaksi, IDTerminal, " & vbCrLf
            SQL = SQL & " Keterangan, NoPOD)" & vbCrLf
            SQL = SQL & " SELECT " & NoID & ", IDGudang, IDAkun, KodeAkun, NamaAkun,  Debet, Kredit, IDTransaksi, " & IDUserAktif & ", " & vbCrLf
            SQL = SQL & " Keterangan, NoPOD " & vbCrLf
            SQL = SQL & " FROM TKasIND "
            SQL = SQL & " WHERE TKasIND.IDTerminal = " & IDUserAktif & " ORDER BY TKasIND.ID"
            EksekusiSQL(SQL)
            EksekusiSQL("DELETE FROM TKasIND WHERE IDTerminal=" & IDUserAktif)
        Catch ex As Exception
            FxMessage("Error : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error, , , ex.StackTrace)
        End Try
    End Sub
    Private Sub TampilkankanDetil()
        Dim SQL As String = ""
        Dim oDS As New DataSet
        Try
            SQL = "SELECT * FROM TKasIND WHERE IDTerminal=" & IDUserAktif
            oDS = ExecuteDataset("tbl", SQL)
            If oDS.Tables(0).Rows.Count >= 1 Then
                If FxMessage("Masih ada data temporary, ingin ditampilkan ?", NamaAplikasi, MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.No Then
                    EksekusiSQL("DELETE FROM TKasIND WHERE IDTerminal=" & IDUserAktif)
                End If
            End If
            SQL = "INSERT INTO TKasIND " & vbCrLf
            SQL = SQL & " (IDKasIN, IDGudang, IDAkun, KodeAkun, NamaAkun, Debet, Kredit,IDTransaksi, IDTerminal, " & vbCrLf
            SQL = SQL & " Keterangan, NoPOD)" & vbCrLf
            SQL = SQL & " SELECT " & NoID & ", IDGudang, IDAkun, KodeAkun, NamaAkun,  Debet, Kredit, IDTransaksi, " & IDUserAktif & ", " & vbCrLf
            SQL = SQL & " Keterangan, NoPOD " & vbCrLf
            SQL = SQL & " FROM MKasIND "
            SQL = SQL & " WHERE MKasIND.IDKasIN = " & NoID & " ORDER BY MKasIND.ID"
            EksekusiSQL(SQL)
        Catch ex As Exception
            FxMessage("Error : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error, , , ex.StackTrace)
        Finally
            oDS.Dispose()
            RefreshItem()
        End Try
    End Sub

    Sub GenerateJumlah()
        'If msgbox("Apakah anda yakin mau generate pembayaran per nota?", NamaApplikasi,MessageBoxButtons.YesNo,MessageBoxIcon.Question) =Windows.Forms.DialogResult.Yes Then
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
        Return KodeNota & Format(CStr(Id), "0000") & "/" & Format(CStr(Bulan), "00") & "/" & Format(CStr(Tahun), "0000")
    End Function

    Private Sub Tgl_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Tgl.EditValueChanged

    End Sub

    Private Sub cmdNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdNew.Click
        Try
            Dim IDKasD As Long
            Dim x As New frmEntriKasIND
            IDKasD = 0
            x.IDAkun = IDAkun
            x.pStatus = mdlAccPublik.ptipe.Baru
            x.Tgl = Tgl.DateTime
            If x.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
                RefreshItem()
                txtCatatan.Text = ""
                For i As Integer = 0 To GridView1.RowCount - 1
                    txtCatatan.Text &= IIf(i <> 0, ", ", "") & GridView1.GetRowCellValue(i, "Keterangan").ToString.Trim
                Next
            End If
            x.Close()
            x.Dispose()
        Catch ex As Exception
            FxMessage(ex.Message, ".:: Pesan Kesalahan ::.", MessageBoxButtons.OK, MessageBoxIcon.Error, , , ex.StackTrace)
        End Try
    End Sub


    Private Sub cmdEdit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdEdit.Click
        Try
            Dim IDKasD As Long
            Dim view As ColumnView = GridControl1.FocusedView
            Dim row As System.Data.DataRow = view.GetDataRow(GridView1.FocusedRowHandle)
            Dim x As New frmEntriKasIND
            IDKasD = NullToLong(row("ID"))
            x.ID = IDKasD
            'x.IDAlamat = IIf(IDAlamat >= 1000, IDAlamat - 1000, IDAlamat)
            'x.NamaAlamat = txtAlamat.Text
            x.Tgl = Tgl.DateTime
            x.pStatus = mdlAccPublik.ptipe.Edit

            If x.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
                RefreshItem()
                txtCatatan.Text = ""
                For i As Integer = 0 To GridView1.RowCount - 1
                    txtCatatan.Text &= IIf(i <> 0, ", ", "") & GridView1.GetRowCellValue(i, "Keterangan").ToString.Trim
                Next
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



    Private Sub PanelControl1_Paint(ByVal sender As System.Object, ByVal e As System.Windows.Forms.PaintEventArgs) Handles PanelControl1.Paint

    End Sub

    Private Sub txtAlamat_ButtonPressed(ByVal sender As Object, ByVal e As DevExpress.XtraEditors.Controls.ButtonPressedEventArgs)

    End Sub

    Private Sub txtAlamat_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)

    End Sub

    Private Sub cmdDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdDelete.Click
        Try
            Dim IDKasD As Long
            Dim view As ColumnView = GridControl1.FocusedView
            Dim row As System.Data.DataRow = view.GetDataRow(GridView1.FocusedRowHandle)
            IDKasD = NullToLong(row("ID"))
            If MsgBox("Apa item ini akan dihapus?", MessageBoxButtons.YesNo, "Hapus Item") = Windows.Forms.DialogResult.Yes Then
                EksekusiSQL("DELETE FROM TKasIND WHERE ID=" & IDKasD)
                RefreshItem()
                txtCatatan.Text = ""
                For i As Integer = 0 To GridView1.RowCount - 1
                    txtCatatan.Text &= IIf(i <> 0, ", ", "") & GridView1.GetRowCellValue(i, "Keterangan").ToString.Trim
                Next
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

    Private Sub txtMataUang_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)

    End Sub



    Private Sub txtAlamat_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        NotaBaru()
    End Sub

    Private Sub txtKas_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtKas.EditValueChanged

    End Sub
End Class