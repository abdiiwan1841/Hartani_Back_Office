'Imports MySql.Data.MySqlClient
Imports DevExpress.XtraEditors
Imports DevExpress.XtraGrid.Views.Base 

Public Class frmEntriKasOut
    Public NoID As Long 
    Private IDMataUang As Long 
    Private IDKasBank As Long
    Private IDKasBankOld As Long
    Private IDAkun As Long
    Private oldTanggal As DateTime
    Private oldNota As String
    Private oldNosp As String
    Dim KodeLama As String
    Public pStatus As mdlAccPublik.Ptipe
    Dim JumlahA As Double
    Public IDGudang As Long

    Private Sub SimpleButton1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SimpleButton1.Click
        DialogResult = Windows.Forms.DialogResult.Cancel
        EksekusiSQL("DELETE FROM TKasOutD WHERE IDTerminal=" & IDUserAktif)
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
        str = "Select SUM(ISNULL(Kurs, 0) * ISNULL(Debet, 0)) AS JumDebet, SUM(Debet) AS JumKredit FROM TKasOutD WHERE IDTerminal=" & IDUserAktif
        Try
            ' EksekusiSQl("Update TKasOutD SET Kurs=" & FixKoma(txtNilaiTukar.EditValue) & " WHERE IDTerminal=" & IDUserAktif)
            dbs = ExecuteDataset("tabel", str)
            If dbs.Tables("tabel").Rows.Count = 0 Then
                'JumDebet = 0
                JumlahA = 0
            Else
                '  JumDebet.Value = IIf(IsNull(RsCekRecord.Fields("JumDebet")), 0, RsCekRecord.Fields("JumDebet"))
                txtJumlah.EditValue = IIf(IsNumeric(dbs.Tables("tabel").Rows(0).Item("JumKredit")), dbs.Tables("tabel").Rows(0).Item("JumKredit"), 0)
                'txtJumlah.EditValue = JumlahA
                ' JumDebet.Value = IIf(IsNull(RsCekRecord.Fields("JumKredit")), 0, RsCekRecord.Fields("JumKredit"))
            End If
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


        If CekKodeValid(txtKode.Text, KodeLama, "MKasOut", "Kode", IIf(pStatus = mdlAccPublik.ptipe.Baru, False, True)) Then
            MsgBox("Kode sudah dipakai.", MsgBoxStyle.Information, NamaAplikasi)
            isValidasi = False
            Exit Function
        End If
        isValidasi = True
    End Function
    Private Sub RefreshDataKontak()
        Dim ds As New DataSet
        Dim strsql
        Try
            strsql = "SELECT NoID, Kode, Nama,IsSupplier,IsPegawai FROM MAlamat WHERE IsActive=1 "
            ds = ExecuteDataset("Alamat", strsql)
            txtKodeSupplier.Properties.DataSource = ds.Tables("Alamat")
            If System.IO.File.Exists(FolderLayouts & Me.Name & SearchLookUpEdit1View.Name & ".xml") Then
                SearchLookUpEdit1View.RestoreLayoutFromXml(FolderLayouts & Me.Name & SearchLookUpEdit1View.Name & ".xml")
            End If


        Catch ex As Exception
            XtraMessageBox.Show("Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            ds.Dispose()
        End Try
    End Sub
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
                    NoID = GetNewID("MKasOut", "ID")
                    SQL = "Insert Into MKasOut (IDGudang,ID,Tanggal,Kode,Keterangan,Jumlah," & _
                          "IsPosted,IDAkunKas,IDKas,KodeReff,IsGiro,NoGiro,TanggalJTGiro,NamaPenerimaGiro,IDAlamat,IDWilayah) Values(" & _
                            IDGudang & "," & NoID & ",'" & Format(Tgl.DateTime, "yyyy/MM/dd") & "','" & FixApostropi(txtKode.Text) & "','" & FixApostropi(txtCatatan.Text) & "'," & _
                          FixKoma(txtJumlah.EditValue) & "" & _
                          ",0," & IDAkun & "," & IDKasBank & ",'" & FixApostropi(txtKodeReff.Text) & "'," & _
                          IIf(chkGiro.Checked, 1, 0) & ",'" & FixApostropi(txtNoGiro.Text) & "'," & IIf(NullToStr(TglJTGiro.Text) = "", "NULL", "'" & Format(TglJTGiro.DateTime, "yyyy/MM/dd") & "'") & ",'" & FixApostropi(txtPenerimaGiro.Text) & "'," & NullToLong(txtKodeSupplier.EditValue) & "," & DefIDWilayah & ")"
                ElseIf pStatus = mdlAccPublik.ptipe.Edit Then
                    SQL = "update MKasOut SET " & _
                            " Tanggal='" & Format(Tgl.DateTime, "yyyy/MM/dd") & "'," & _
                          " Kode='" & FixApostropi(txtKode.Text) & "'," & _
                          " KodeReff='" & FixApostropi(txtKodeReff.Text) & "'," & _
                           " Keterangan='" & FixApostropi(txtCatatan.Text) & "'," & _
                          " Jumlah=" & FixKoma(txtJumlah.EditValue) & ", " & _
                          " IDAkunKas= " & IDAkun & "," & _
                          " IDKas= " & IDKasBank & "," & _
                          " IsGiro= " & IIf(chkGiro.Checked, 1, 0) & "," & _
                          " NoGiro= '" & FixApostropi(txtNoGiro.Text) & "'," & _
                          " TanggalJTGiro=" & IIf(NullToStr(TglJTGiro.Text) = "", "NULL", "'" & Format(TglJTGiro.DateTime, "yyyy/MM/dd") & "'") & "," & _
                          " NamaPenerimaGiro= '" & FixApostropi(txtPenerimaGiro.Text) & "'," & _
                          " IDAlamat= " & NullToLong(txtKodeSupplier.EditValue) & "," & _
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
                Dim x As New frmLUKas
                x.IDGudang = DefIDGudang : x.pStatus = mdlAccPublik.ptipe.LookUp
                If x.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
                    IDKasBank = x.NoID
                    IDAkun = x.IDAKun
                    txtKas.Text = x.Nama '& " - " & x.SubKlas
                    IDGudang = x.IDGudang
                End If
                x.Dispose()
            Case 1
                IDKasBank = -1
                IDAkun = -1
                txtKas.Text = ""
                IDGudang = -1
        End Select
        NotaBaru()
        CariKode()
    End Sub

    Sub RefreshItem()
        Dim strsql As String
        strsql = "SELECT" & vbCrLf
        strsql = strsql & " TKasOutD.ID," & vbCrLf
        strsql = strsql & " TKasOutD.IDKasOut," & vbCrLf
        strsql = strsql & " TKasOutD.IDAkun," & vbCrLf
        strsql = strsql & " TKasOutD.NamaAkun," & vbCrLf
        strsql = strsql & " TKasOutD.IDMataUang," & vbCrLf
        strsql = strsql & " TKasOutD.Debet," & vbCrLf
        strsql = strsql & " TKasOutD.Kredit," & vbCrLf
        strsql = strsql & " TKasOutD.IDTransaksi," & vbCrLf
        strsql = strsql & " TKasOutD.IDTerminal," & vbCrLf
        strsql = strsql & " TKasOutD.Keterangan," & vbCrLf
        strsql = strsql & " TKasOutD.NoPOD," & vbCrLf
        strsql = strsql & " MGUdang.Nama AS NamaGudang," & vbCrLf
        strsql = strsql & " makun.Nama AS Perkiraan" & vbCrLf
        strsql = strsql & " FROM" & vbCrLf
        strsql = strsql & " TKasOutD " & vbCrLf
        strsql = strsql & " LEFT JOIN mgudang ON TKasOutD.idgudang = mgudang.NoID" & vbCrLf
        strsql = strsql & " LEFT JOIN makun ON TKasOutD.IDAkun = makun.ID where TKasOutD.idterminal=" & IDUserAktif
        Try
            ExecuteDBGrid(GridControl1, strsql)
        Catch ex As Exception
            FxMessage(ex.Message, ".:: Pesan Kesalahan ::.", MessageBoxButtons.OK, MessageBoxIcon.Error, , , ex.StackTrace)
        End Try
        HitungTotal()

    End Sub
    Private Sub frBayarHutang_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        GridView1.SaveLayoutToXml(Application.StartupPath & "\System\Layouts\" & Me.Name & GridView1.Name & ".xml")
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
        RefreshDataKontak()
        SetCtlMe()
        TampilData()
        If System.IO.File.Exists(Application.StartupPath & "\System\Layouts\" & Me.Name & GridView1.Name & ".xml") Then
            GridView1.RestoreLayoutFromXml(Application.StartupPath & "\System\Layouts\" & Me.Name & GridView1.Name & ".xml")
        End If
    End Sub

    Sub IsiDefault()
        Tgl.EditValue = Today
        txtJumlah.EditValue = 0 'NullToDbl(oDS.Tables("MBayarHutang").Rows(0).Item("Retur"))
        pStatus = mdlAccPublik.ptipe.Baru
        IDAkun = -1

        'IDKasBank = -1
        'txtKas.Text = ""
    End Sub

    'Sub NotaBaru()
    '    txtKode.Text = GetKodeKasOut(Tgl.DateTime)
    'End Sub
    Sub NotaBaru()
        If pStatus = mdlAccPublik.ptipe.Baru Then
            ' Dim IsKas As Boolean = NullTobool(EksekusiSQlSkalarNew("SELECT IsKas FROM MKas WHERE ID=" & IDKasBank))
            'txtKode.Text = GetKode("MKasOut", IIf(IsKas, "KasKeluar", "BankKeluar"), NullToDate(Tgl.DateTime), IDAlamat, IDAlamat, IDAlamat, IDGudang, DefIDWilayah, DefIDDepartemen, , "(IsKembaliUM=0 OR IsKembaliUM IS NULL)")
        Else
            If oldTanggal.Year = Tgl.DateTime.Year AndAlso oldTanggal.Month = Tgl.DateTime.Month Then
                txtKode.Text = oldNota
            Else
                'Dim IsKas As Boolean = NullTobool(EksekusiSQlSkalarNew("SELECT IsKas FROM MKas WHERE ID=" & IDKasBank))
                'txtKode.Text = GetKode("MKasOut", IIf(IsKas, "KasKeluar", "BankKeluar"), NullToDate(Tgl.DateTime), IDAlamat, IDAlamat, IDAlamat, IDGudang, DefIDWilayah, DefIDDepartemen, , "(IsKembaliUM=0 OR IsKembaliUM IS NULL)")
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
            strsql = "SELECT MKasOut.*,  MAkun.Kode AS KodeAkun, MKas.Kode AS KodeKas, MKas.Nama AS NamaKas , MAkun.Nama AS NamaAkun, mklasakun.Nama AS NamaSubKlasAkun " & vbCrLf
            strsql = strsql & " FROM ( MKasOut LEFT JOIN " & vbCrLf
            strsql = strsql & " (MAkun LEFT JOIN mklasakun ON makun.IDSubKlasAkun = mklasakun.ID) ON MKasOut.IDAkunKas = MAkun.ID) LEFT JOIN " & vbCrLf
            strsql = strsql & " MBank MKas ON MKasOut.IDKas = Mkas.NoID  Where MKasOut.ID= " & NoID.ToString
            Try
                oDS = ExecuteDataset("MTable", strsql)
                If oDS.Tables("MTable").Rows.Count = 0 Then
                    IsiDefault()
                Else
                    '                    IsGiro	bit	Checked
                    'NoGiro	varchar(50)	Checked
                    'NamaPenerimaGiro	varchar(50)	Checked
                    'TanggalJTGiro	datetime	Checked
                    'TanggalCairGiro	datetime	Checked
                    'TanggalBatalGiro	datetime	Checked
                    'IsCair	bit	Checked
                    'IsBatal	bit	Checked
                    'CatatanGiro	varchar(50)	Checked

                    IDKasBank = NullToLong(oDS.Tables("MTable").Rows(0).Item("IDKas"))
                    IDKasBankOld = NullToLong(oDS.Tables("MTable").Rows(0).Item("IDKas"))
                    IDAkun = NullToLong(oDS.Tables("MTable").Rows(0).Item("IDAkunKas"))
                    txtKas.Text = NullToStr(oDS.Tables("MTable").Rows(0).Item("KodeKas")) & " - " & NullToStr(oDS.Tables("MTable").Rows(0).Item("NamaKas"))
                    oldNota = NullToStr(oDS.Tables("MTable").Rows(0).Item("Kode"))
                    Tgl.EditValue = oDS.Tables("MTable").Rows(0).Item("Tanggal")
                    txtKodeReff.Text = NullToStr(oDS.Tables("MTable").Rows(0).Item("KodeReff"))
                    txtCatatan.Text = NullToStr(oDS.Tables("MTable").Rows(0).Item("Keterangan"))
                    oldTanggal = oDS.Tables("MTable").Rows(0).Item("Tanggal")
                    txtJumlah.EditValue = NullToDbl(oDS.Tables("MTable").Rows(0).Item("Jumlah"))
                    txtKode.Text = NullToStr(oDS.Tables("MTable").Rows(0).Item("Kode"))
                    KodeLama = txtKode.Text
                    chkGiro.Checked = NullToBool(oDS.Tables("MTable").Rows(0).Item("IsGiro"))
                    txtNoGiro.Text = NullToStr(oDS.Tables("MTable").Rows(0).Item("NoGiro"))
                    TglJTGiro.EditValue = oDS.Tables("MTable").Rows(0).Item("TanggalJTGiro")
                    txtPenerimaGiro.Text = NullToStr(oDS.Tables("MTable").Rows(0).Item("NamaPenerimaGiro"))
                    txtKodeSupplier.EditValue = NullToLong(oDS.Tables("MTable").Rows(0).Item("IDAlamat"))
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
            EksekusiSQL("DELETE FROM MKasOutD WHERE IDKasOut=" & NoID)
            SQL = "INSERT INTO MKasOutD " & vbCrLf
            SQL = SQL & " (IDKasOut, IDGudang, IDDepartemen, NamaDepartemen, IDWilayah, IDAkun, KodeAkun, NamaAkun, IDMataUang, MataUang, Kurs, Debet, Kredit, DebetA, KreditA, IDTransaksi, IDTerminal, " & vbCrLf
            SQL = SQL & " Keterangan, NoPOD, IsBukuPembantu, IDKasKeluar)" & vbCrLf
            SQL = SQL & " SELECT " & NoID & ", IDGudang, IDDepartemen, NamaDepartemen, IDWilayah, IDAkun, KodeAkun, NamaAkun, IDMataUang, MataUang, Kurs, Debet, Kredit, DebetA, KreditA, IDTransaksi, " & IDUserAktif & ", " & vbCrLf
            SQL = SQL & " Keterangan, NoPOD, IsBukuPembantu, IDKasKeluar" & vbCrLf
            SQL = SQL & " FROM TKasOutD "
            SQL = SQL & " WHERE TKasOutD.IDTerminal = " & IDUserAktif & " ORDER BY TKasOutD.ID"
            EksekusiSQL(SQL)
            EksekusiSQL("DELETE FROM TKasOutD WHERE IDTerminal=" & IDUserAktif)
        Catch ex As Exception
            FxMessage("Error : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error, , , ex.StackTrace)
        End Try
    End Sub
    Private Sub TampilkankanDetil()
        Dim SQL As String = ""
        Dim oDS As New DataSet
        Try
            SQL = "SELECT * FROM TKasOutD WHERE IDTerminal=" & IDUserAktif
            oDS = ExecuteDataset("tbl", SQL)
            If oDS.Tables(0).Rows.Count >= 1 Then
                If FxMessage("Masih ada data temporary, ingin ditampilkan ?", NamaAplikasi, MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.No Then
                    EksekusiSQL("DELETE FROM TKasOutD WHERE IDTerminal=" & IDUserAktif)
                End If
            End If
            SQL = "INSERT INTO TKasOutD " & vbCrLf
            SQL = SQL & " (IDKasOut, IDGudang,   IDAkun, KodeAkun, NamaAkun,   Debet, Kredit, IDTransaksi, IDTerminal, " & vbCrLf
            SQL = SQL & " Keterangan, NoPOD , IDKasKeluar)" & vbCrLf
            SQL = SQL & " SELECT " & NoID & ", IDGudang,  IDAkun, KodeAkun, NamaAkun, Debet, Kredit, IDTransaksi, " & IDUserAktif & ", " & vbCrLf
            SQL = SQL & " Keterangan, NoPOD , IDKasKeluar" & vbCrLf
            SQL = SQL & " FROM MKasOutD "
            SQL = SQL & " WHERE MKasOutD.IDKasOut = " & NoID & " ORDER BY MKasOutD.ID"
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
        Return KodeNota & Format(Id, "0000") & "/" & Format(Bulan, "00") & "/" & Format(Tahun, "0000")
    End Function

    Private Sub Tgl_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Tgl.EditValueChanged
        CariKode()
    End Sub

    Private Sub cmdNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdNew.Click
        Try
            Dim IDKasD As Long
            Dim x As New frmEntriKasOutD
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
            Dim x As New frmEntriKasOutD
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




    Private Sub cmdDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdDelete.Click
        Try
            Dim IDKasD As Long
            Dim view As ColumnView = GridControl1.FocusedView
            Dim row As System.Data.DataRow = view.GetDataRow(GridView1.FocusedRowHandle)
            IDKasD = NullToLong(row("ID"))
            If MsgBox("Apa item ini akan dihapus?", MessageBoxButtons.YesNo, NamaAplikasi) = Windows.Forms.DialogResult.Yes Then
                EksekusiSQL("DELETE FROM TKasOutD WHERE ID=" & IDKasD)
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

    Private Sub txtJumlah_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtJumlah.DoubleClick
        HitungTotal()
    End Sub

    Private Sub txtJumlah_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtJumlah.EditValueChanged

    End Sub

    Private Sub txtAlamat_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        NotaBaru()
    End Sub

    Private Sub txtKas_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtKas.LostFocus
        NotaBaru()
    End Sub

    Sub CariKode()
        If pStatus = mdlAccPublik.ptipe.Baru Then
            txtKode.Text = clsKode.MintaKodeBaru(IIf(NullToBool(EksekusiSQlSkalarNew("select IsKas from MBank where NoID=" & IDKasBank)), "BKK", "BBK"), "MKasOut", Tgl.DateTime, DefIDWilayah, 4, " Left(MKasOut.Kode,3)='" & IIf(NullToBool(EksekusiSQlSkalarNew("select IsKas from MBank where NoID=" & IDKasBank)), "BKK", "BBK") & "'")
        Else
            If IDKasBankOld <> IDKasBank Or Month(oldTanggal) <> Month(Tgl.DateTime) Or Year(oldTanggal) <> Year(Tgl.DateTime) Then
                txtKode.Text = clsKode.MintaKodeBaru(IIf(NullToBool(EksekusiSQlSkalarNew("select IsKas from MBank where NoID=" & IDKasBank)), "BKK", "BBK"), "MKasOut", Tgl.DateTime, DefIDWilayah, 4, " Left(MKasOut.Kode,3)='" & IIf(NullToBool(EksekusiSQlSkalarNew("select IsKas from MBank where NoID=" & IDKasBank)), "BKK", "BBK") & "'")
            End If
        End If
    End Sub

    Private Sub txtKas_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtKas.EditValueChanged

    End Sub

    Private Sub chkGiro_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkGiro.CheckedChanged
        If chkGiro.Checked Then
        Else
            txtPenerimaGiro.Text = ""
            txtNoGiro.Text = ""
            TglJTGiro.EditValue = Chr(0)
        End If
    End Sub

    Private Sub txtKodeSupplier_EditValueChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtKodeSupplier.EditValueChanged
        UbahSupplier()
    End Sub
    Private Sub ubahSupplier()
        Try
            txtNamaSupplier.Text = NullToStr(EksekusiSQlSkalarNew("SELECT Nama FROM MAlamat WHERE NoID=" & NullToLong(txtKodeSupplier.EditValue)))
        Catch ex As Exception

        End Try
    End Sub

    Private Sub SimpleButton2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SimpleButton2.Click
        'Dim SQL As String = ""
        'Dim IDKasD As Long
        'Dim x As New frmLUKasKasir
        'Try
        '    IDKasD = -1
        '    x.TglDari.DateTime = Tgl.DateTime
        '    x.TglSampai.DateTime = Tgl.DateTime
        '    x.pStatus = mdlAccPublik.ptipe.LookUp
        '    If x.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
        '        For Each i As Integer In x.GridView1.GetSelectedRows
        '            SQL = " Insert Into TKasOutD (IDTerminal,IDGudang,IDAkun,Debet,Kredit,KodeAkun,NamaAkun,Keterangan,NoPOD,IDKasKeluar) " & vbCrLf
        '            SQL &= " VALUES (" & IDUserAktif & ","
        '            SQL &= DefIDGudang & ","
        '            SQL &= NullToLong(x.IDAkunAlokasi) & ","
        '            SQL &= FixKoma(x.GridView1.GetRowCellValue(i, "Jumlah")) & ",0,"
        '            SQL &= "'" & FixApostropi(NullToStr(EksekusiSQlSkalarNew("SELECT Kode FROM MAkun WHERE ID=" & IDAkun))) & "',"
        '            SQL &= "'" & FixApostropi(NullToStr(EksekusiSQlSkalarNew("SELECT Nama FROM MAkun WHERE ID=" & IDAkun))) & "',"
        '            SQL &= "'" & FixApostropi(x.GridView1.GetRowCellValue(i, "NamaKasir")) & " : " & FixApostropi(NullToStr(x.GridView1.GetRowCellValue(i, "Keterangan")).ToString.ToUpper) & "',"
        '            SQL &= "'" & FixApostropi("") & "'," & NullToLong(x.GridView1.GetRowCellValue(i, "NoID"))
        '            SQL &= ") "
        '            EksekusiSQL(SQL)
        '        Next
        '        RefreshItem()
        '        txtCatatan.Text = ""
        '        For i As Integer = 0 To GridView1.RowCount - 1
        '            txtCatatan.Text &= IIf(i <> 0, ", ", "") & GridView1.GetRowCellValue(i, "Keterangan").ToString.Trim
        '        Next
        '    End If
        'Catch ex As Exception
        '    FxMessage(ex.Message, ".:: Pesan Kesalahan ::.", MessageBoxButtons.OK, MessageBoxIcon.Error, , , ex.StackTrace)
        'Finally
        '    x.Dispose()
        'End Try
    End Sub
End Class