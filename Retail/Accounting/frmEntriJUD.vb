Imports System.Data
Imports DevExpress.XtraEditors
Imports DevExpress.Utils
Imports VPoint.FungsiControl
Public Class frmEntriJUD
    Public ID As Long
    Public pStatus As FungsiControl.ptipe, namaTabel As String = "TJurnalD"
    Public IDAkun, IDGudang As Long
    Public IDMataUang As Long
    Public IDDepartemen, IDWilayah As Long
    Public DebetA As Double, KreditA As Double
    Public KodeAkun As String, NamaAkun As String
    Public NoPOD As String = "", KodeMataUang As String, Tgl As Date
    Public Enum JenisTransaksi_ As Integer
        All = 0
        Alamat = 1
    End Enum
    Public JenisTransaksi As JenisTransaksi_

    Private Sub cmdClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdClose.Click
        DialogResult = Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub frmEntriKasIND_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        LayoutControl1.SaveLayoutToXml(LayOutKu(Me.Name & Me.LayoutControl1.Name))
    End Sub
    Private Sub SetCtlMe()
        'SetForm(Me)
        SetButton(cmdClose, button_.cmdCancelSave)
        SetButton(cmdSave, button_.cmdSave)
        'IDDepartemen = defIDDepartemen
        'IDMataUang = defIDMataUang
        'txtDepartemen.Text = defNamaDepartemen
        'txtMataUang.Text = defNamaMataUang
        'txtNilaiTukar.EditValue = defNilaiTukarMataUang
        'If txtGudang.Text = "" Then
        '    IDGudang = defIDGudang
        '    txtGudang.Text = defNamaGudang
        'End If
        'If txtWilayah.Text = "" Then
        '    IDWilayah = defIDWilayah
        '    txtWilayah.Text = defNamaWilayah
        'End If
        'If txtDepartemen.Text = "" Then
        '    IDDepartemen = defIDDepartemen
        '    txtDepartemen.Text = defNamaDepartemen
        'End If
        'If Not txtAkun.Visible Then
        '    txtGudang.Visible = False
        'End If
        'If Not IsAdministrator Then
        '    txtDepartemen.Enabled = False
        'End If
        If JenisTransaksi = JenisTransaksi_.All Then
            txtWilayah.Enabled = True
        End If
    End Sub
    Private Sub frmEntriKasInD_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'DialogResult = Windows.Forms.DialogResult.Cancel
        SetCtlMe()
        SetForm(Me)
        txtDebet.EditValue = 0.0
        txtDebet.Properties.Mask.EditMask = "n2"
        txtDebet.Properties.Mask.MaskType = Mask.MaskType.Numeric
        txtDebet.Properties.Mask.UseMaskAsDisplayFormat = True
        txtKredit.EditValue = 0.0
        txtKredit.Properties.Mask.EditMask = "n2"
        txtKredit.Properties.Mask.MaskType = Mask.MaskType.Numeric
        txtKredit.Properties.Mask.UseMaskAsDisplayFormat = True
        If pStatus = mdlAccPublik.ptipe.Edit Then
            GetData()
        Else
            IsiDefault()
        End If
        If System.IO.File.Exists(FungsiControl.LayOutKu(Me.Name & Me.LayoutControl1.Name)) Then
            LayoutControl1.RestoreLayoutFromXml(FungsiControl.LayOutKu(Me.Name & Me.LayoutControl1.Name))
        End If
    End Sub
    Private Sub GetData()
        Dim strcekrecord As String
        Dim oDS As New DataSet
        strcekrecord = "SELECT " & namaTabel & ".*,MAkun.Kode as KodeAkun,MAkun.Nama as NamaAkun,MMataUang.Nama as MataUang, " & _
        "MGudang.Nama AS NamaGudang, MWilayah.Nama AS NamaWilayah," & _
        "MDepartemen.Nama AS NamaDep "
        strcekrecord &= " FROM ((" & namaTabel & vbCrLf
        strcekrecord &= " LEFT JOIN MAkun ON " & namaTabel & ".IDAkun=MAkun.ID " & vbCrLf
        strcekrecord &= " LEFT JOIN MMataUang ON " & namaTabel & ".IDMataUang=MMataUang.ID " & vbCrLf
        strcekrecord &= " LEFT JOIN MGudang ON MGudang.NoID=" & namaTabel & ".IDGudang) " & vbCrLf
        strcekrecord &= " LEFT JOIN MWilayah ON MWilayah.NoID=" & namaTabel & ".IDWilayah) " & vbCrLf
        strcekrecord &= " LEFT JOIN MDepartemen ON MDepartemen.NoID=" & namaTabel & ".IDDepartemen " & vbCrLf
        strcekrecord &= " WHERE " & namaTabel & ".IDTerminal = " & IDUserAktif & " And " & namaTabel & ".ID = " & ID
        Try
            oDS = ExecuteDataset(namaTabel, strcekrecord)
            If oDS.Tables(namaTabel).Rows.Count >= 1 Then
                ID = NullToLong(oDS.Tables(namaTabel).Rows(0).Item("ID").ToString)
                IDAkun = NullToLong(oDS.Tables(namaTabel).Rows(0).Item("IDAkun").ToString)
                IDDepartemen = NullToLong(oDS.Tables(namaTabel).Rows(0).Item("IDDepartemen"))
                IDWilayah = NullToLong(oDS.Tables(namaTabel).Rows(0).Item("IDWilayah"))
                IDMataUang = NullToLong(oDS.Tables(namaTabel).Rows(0).Item("IDMataUang").ToString)
                txtDepartemen.Text = NullToStr(oDS.Tables(namaTabel).Rows(0).Item("NamaDep").ToString)
                txtAkun.Text = NullToStr(oDS.Tables(namaTabel).Rows(0).Item("NamaAkun").ToString)
                txtMataUang.Text = NullToStr(oDS.Tables(namaTabel).Rows(0).Item("MataUang").ToString)
                txtNilaiTukar.EditValue = NullToDbl(oDS.Tables(namaTabel).Rows(0).Item("Kurs").ToString)
                DebetA = NullToDbl(oDS.Tables(namaTabel).Rows(0).Item("DebetA"))
                KreditA = NullToDbl(oDS.Tables(namaTabel).Rows(0).Item("KreditA"))
                txtCatatan.Text = NullToStr(oDS.Tables(namaTabel).Rows(0).Item("Keterangan"))
                txtDebet.EditValue = NullToDbl(oDS.Tables(namaTabel).Rows(0).Item("Debet"))
                txtKredit.EditValue = NullToDbl(oDS.Tables(namaTabel).Rows(0).Item("Kredit"))
                KodeAkun = NullToStr(oDS.Tables(namaTabel).Rows(0).Item("KodeAkun").ToString)
                NamaAkun = NullToStr(oDS.Tables(namaTabel).Rows(0).Item("NamaAkun").ToString)
                IDGudang = NullToLong(oDS.Tables(namaTabel).Rows(0).Item("IDGudang").ToString)
                txtGudang.Text = NullToStr(oDS.Tables(namaTabel).Rows(0).Item("NamaGudang").ToString)
                txtWilayah.Text = NullToStr(oDS.Tables(namaTabel).Rows(0).Item("NamaWilayah").ToString)
            End If
        Catch ex As Exception
            FxMessage(ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error, , , ex.StackTrace)
        End Try
    End Sub
    Private Sub IsiDefault()
        'txtJumlah.EditValue = 0
        If txtMataUang.Text = "" Then
            txtMataUang.Text = defNamaMataUang
            IDMataUang = defIDMataUang
            txtNilaiTukar.EditValue = defNilaiTukarMataUang
        End If

        IDGudang = DefIDGudang
        txtGudang.Text = NullToStr(EksekusiSQlSkalarNew("SELECT Nama FROM MGudang WHERE NoID=" & DefIDGudang))
        IDWilayah = DefIDWilayah
        SetGudang()

        'txtDepartemen.Text = defNamaDepartemen
        'IDDepartemen = defIDDepartemen
        'txtAkun.Text = defNamaAkun
        'IDAkun = defIDAkun
        'KodeAkun = NullTostr( EksekusiSQlSkalarNew("SELECT Kode FROM MAkun WHERE ID=" & IDAkun))
    End Sub
    Private Sub txtDepartemen_ButtonClick(ByVal sender As Object, ByVal e As DevExpress.XtraEditors.Controls.ButtonPressedEventArgs) Handles txtDepartemen.ButtonClick
        'Select Case e.Button.Index
        '    Case 0
        '        Dim x As New frmLUDepartemen
        '        x.pStatus = publik.ptipe.LookUp
        '        If x.ShowDialog(Me)= Windows.Forms.DialogResult.OK Then
        '            IDDepartemen = x.NoID
        '            txtDepartemen.Text = x.Nama
        '        End If
        '        x.Dispose()
        '    Case 1
        '        IDDepartemen = -1
        '        txtDepartemen.Text = ""
        'End Select
    End Sub

    Private Sub cmdSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSave.Click
        Dim SQL As String, curentcursor As Windows.Forms.Cursor = Windows.Forms.Cursor.Current
        Dim dlg As DevExpress.Utils.WaitDialogForm
        dlg = New DevExpress.Utils.WaitDialogForm("Menyimpan data." & vbCrLf & "Mohon tunggu beberapa saat ...", NamaAplikasi)
        dlg.TopMost = True
        dlg.Show()
        Windows.Forms.Cursor.Current = Windows.Forms.Cursors.WaitCursor
        Try
            If IsValidasi() Then
                HitungTotal()
                If pStatus = mdlAccPublik.ptipe.Baru Then
                    SQL = " INSERT INTO " & namaTabel & "(IDWilayah,IDTerminal,IDDepartemen,IDGudang,IDMataUang,IDAkun,Debet,Kredit,DebetA,KreditA,Keterangan,Kurs) " & vbCrLf
                    SQL = SQL & " VALUES (" & IDWilayah & "," & IDUserAktif & ","
                    SQL = SQL & IDDepartemen & ","
                    SQL = SQL & IDGudang & ","
                    SQL = SQL & IDMataUang & ","
                    SQL = SQL & IDAkun & ","
                    SQL = SQL & FixKoma(txtDebet.EditValue) & ","
                    SQL = SQL & FixKoma(txtKredit.EditValue) & ","
                    SQL = SQL & FixKoma(DebetA) & ","
                    SQL = SQL & FixKoma(KreditA) & ","
                    SQL = SQL & "'" & FixApostropi(txtCatatan.Text) & "',"
                    SQL = SQL & FixKoma(txtNilaiTukar.EditValue) & ") "
                Else
                    SQL = " UPDATE " & namaTabel & " SET IDTerminal=" & IDUserAktif & " WHERE ID=" & ID
                End If
                If EksekusiSQL(SQL) Then
                    TambahUpdatetan()
                    dlg.Close()
                    dlg.Dispose()
                    Windows.Forms.Cursor.Current = Cursors.Default
                    DialogResult = Windows.Forms.DialogResult.OK
                    Me.Close()
                    'Me.Dispose()
                End If
            End If
        Catch ex As Exception
            FxMessage("Error : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error, , , ex.StackTrace)
        End Try
        dlg.Close()
        dlg.Dispose()
        Windows.Forms.Cursor.Current = Cursors.Default
    End Sub
    Private Sub TambahUpdatetan()
        Dim SQL As String
        If pStatus = mdlAccPublik.ptipe.Edit Then
            SQL = "Update " & namaTabel & " Set " & vbCrLf
            SQL = SQL & "IDTerminal=" & IDUserAktif & ","
            SQL = SQL & "IDDepartemen=" & IDDepartemen & "," 'IIf(txtDepartemen.Visible, IDDepartemen, "NULL") & ","
            SQL = SQL & "IDWilayah=" & IDWilayah & "," 'IIf(txtWilayah.Visible, IDWilayah, "NULL") & ","
            SQL = SQL & "IDGudang=" & IIf(JenisTransaksi = JenisTransaksi_.All, IDGudang, "NULL") & ","
            SQL = SQL & "IDMataUang=" & IDMataUang & ","
            SQL = SQL & "IDAkun=" & IIf(JenisTransaksi = JenisTransaksi_.All, IDAkun, "NULL") & ","
            SQL = SQL & "Debet=" & FixKoma(txtDebet.EditValue) & ","
            SQL = SQL & "Kredit=" & FixKoma(txtKredit.EditValue) & ","
            SQL = SQL & "DebetA=" & FixKoma(DebetA) & ","
            SQL = SQL & "KreditA=" & FixKoma(KreditA) & ","
            SQL = SQL & "Keterangan='" & FixApostropi(txtCatatan.Text) & "',"
            SQL = SQL & "Kurs=" & FixKoma(txtNilaiTukar.EditValue) & " "
            SQL = SQL & "WHERE ID=" & ID
            EksekusiSQL(SQL)
        End If
    End Sub
    Sub HitungTotal()
        KreditA = Bulatkan(txtKredit.EditValue * txtNilaiTukar.EditValue, 2)
        DebetA = Bulatkan(txtDebet.EditValue * txtNilaiTukar.EditValue, 2)
    End Sub
    Private Function IsValidasi() As Boolean
        If JenisTransaksi = JenisTransaksi_.All AndAlso txtAkun.Text = "" Then
            FxMessage("Akun masih kosong.", NamaAplikasi)
            IsValidasi = False
            txtAkun.Focus()
            Exit Function
        End If
        If txtDepartemen.Text = "" Then
            FxMessage("Departemen masih kosong.", NamaAplikasi)
            IsValidasi = False
            txtDepartemen.Focus()
            Exit Function
        End If
        If JenisTransaksi = JenisTransaksi_.All AndAlso txtGudang.Text = "" Then
            FxMessage("Gudang masih kosong.", NamaAplikasi)
            IsValidasi = False
            txtGudang.Focus()
            Exit Function
        End If
        'If txtMataUang.Text = "" Then
        '    FxMessage("Mata uang masih kosong.", NamaAplikasi)
        '    IsValidasi = False
        '    txtMataUang.Focus()
        '    Exit Function
        'End If
        If CDbl(txtDebet.EditValue) = 0 And CDbl(txtKredit.EditValue) = 0 Then
            FxMessage("Jumlah masih nol.", NamaAplikasi)
            IsValidasi = False
            txtDebet.Focus()
            Exit Function
        End If
        IsValidasi = True
    End Function

    Private Sub txtAkun_ButtonClick(ByVal sender As Object, ByVal e As DevExpress.XtraEditors.Controls.ButtonPressedEventArgs) Handles txtAkun.ButtonClick
        Select Case e.Button.Index
            Case 0
                Dim x As New frmLUAkun : x.pStatus = mdlAccPublik.ptipe.LookUp
                If x.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
                    IDAkun = x.NoID
                    txtAkun.Text = x.Nama & " - " & x.SubKlas
                    'IDMataUang = NullToLong(x.row("IDMataUang"))
                    'txtMataUang.Text = NullToStr(x.row("NamaMataUang"))
                    'GetKursBI(IDMataUang, Tgl, txtNilaiTukar.EditValue)
                End If
                x.Dispose()
            Case 1
                IDAkun = -1
                txtAkun.Text = ""
        End Select
    End Sub
    Private Sub txtMataUang_ButtonClick(ByVal sender As Object, ByVal e As DevExpress.XtraEditors.Controls.ButtonPressedEventArgs) Handles txtMataUang.ButtonClick
        'Select Case e.Button.Index
        '    Case 0
        '        Dim x As New frmLUKurs
        '        x.pStatus = publik.ptipe.LookUp
        '        If x.ShowDialog(Me)= Windows.Forms.DialogResult.OK Then
        '            IDMataUang = x.NoID
        '            txtMataUang.Text = x.Nama '& " - " & x.SubKlas
        '            GetKursBI(IDMataUang, Tgl, txtNilaiTukar.EditValue)
        '        End If
        '        x.Dispose()
        '    Case 1
        '        IDMataUang = -1
        '        txtMataUang.Text = ""
        '        txtNilaiTukar.EditValue = 0
        'End Select
    End Sub

    Private Sub txtGudang_ButtonClick(ByVal sender As Object, ByVal e As DevExpress.XtraEditors.Controls.ButtonPressedEventArgs) Handles txtGudang.ButtonClick
        Select Case e.Button.Index
            Case 0
                Dim x As New frmLUGudang
                x.pStatus = mdlAccPublik.ptipe.LookUp
                x.IDWilayah = DefIDWilayah
                If x.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
                    IDGudang = x.NoID
                    txtGudang.Text = x.Nama
                    IDWilayah = x.IDWilayah
                    'idwilayah = NullToLong( EksekusiSQlSkalarNew("SELECT MDepartemen.ID FROM MGudang LEFT JOIN (MWilayah LEFT JOIN MDepartemen ON MDepartemen.ID=MWilayah.IDDepartemen) ON MWilayah.ID=MGudang.IDWilayah WHERE MGudang.ID=" & IDGudang))
                    'txtDepartemen.Text = NullTostr( EksekusiSQlSkalarNew("SELECT MDepartemen.Nama FROM MGudang LEFT JOIN (MWilayah LEFT JOIN MDepartemen ON MDepartemen.ID=MWilayah.IDDepartemen) ON MWilayah.ID=MGudang.IDWilayah WHERE MGudang.ID=" & IDGudang))
                    'IDDepartemen = NullToLong( EksekusiSQlSkalarNew("SELECT MDepartemen.ID FROM MGudang LEFT JOIN (MWilayah LEFT JOIN MDepartemen ON MDepartemen.ID=MWilayah.IDDepartemen) ON MWilayah.ID=MGudang.IDWilayah WHERE MGudang.ID=" & IDGudang))
                    'txtDepartemen.Text = NullTostr( EksekusiSQlSkalarNew("SELECT MDepartemen.Nama FROM MGudang LEFT JOIN (MWilayah LEFT JOIN MDepartemen ON MDepartemen.ID=MWilayah.IDDepartemen) ON MWilayah.ID=MGudang.IDWilayah WHERE MGudang.ID=" & IDGudang))
                    SetGudang()
                End If
                x.Dispose()
            Case 1
                IDGudang = -1
                txtGudang.Text = ""
                IDWilayah = -1
                SetGudang()
        End Select
    End Sub

    Private Sub SetGudang()
        Dim SQL As String = ""
        Dim oDS As New DataSet
        SQL = "SELECT MWilayah.IDDepartemen, MDepartemen.Kode AS KodeDepartemen, MDepartemen.Nama AS NamaDepartemen, MWilayah.Kode AS KodeWilayah, MWilayah.Nama AS NamaWilayah FROM MWilayah "
        SQL = SQL & " LEFT JOIN MDepartemen ON MDepartemen.NoID=MWilayah.IDDepartemen WHERE MWilayah.NoID=" & IDWilayah
        Try
            oDS = ExecuteDataset("tbl", SQL)
            If oDS.Tables(0).Rows.Count >= 1 Then
                txtWilayah.Text = NullToStr(oDS.Tables(0).Rows(0).Item("NamaWilayah"))
                txtDepartemen.Text = NullToStr(oDS.Tables(0).Rows(0).Item("NamaDepartemen"))
                IDDepartemen = NullToLong(oDS.Tables(0).Rows(0).Item("IDDepartemen"))
            End If
        Catch ex As Exception
            FxMessage("Error : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error, , , ex.StackTrace)
        Finally
            oDS.Dispose()
        End Try
    End Sub
    Private Sub SetWilayah()
        Dim SQL As String = ""
        Dim oDS As New DataSet
        SQL = "SELECT MDepartemen.ID AS IDDep, MDepartemen.Kode AS KodeDepartemen, MDepartemen.Nama AS NamaDepartemen, MDepartemen.ID FROM MDepartemen WHERE MDepartemen.ID=" & IDDepartemen
        Try
            oDS = ExecuteDataset("tbl", SQL)
            If oDS.Tables(0).Rows.Count >= 1 Then
                txtDepartemen.Text = NullToStr(oDS.Tables(0).Rows(0).Item("NamaDepartemen"))
                IDDepartemen = NullToLong(oDS.Tables(0).Rows(0).Item("IDDep"))
            End If
        Catch ex As Exception
            FxMessage("Error : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error, , , ex.StackTrace)
        Finally
            oDS.Dispose()
        End Try
    End Sub

    Private Sub txtWilayah_ButtonClick(ByVal sender As Object, ByVal e As DevExpress.XtraEditors.Controls.ButtonPressedEventArgs) Handles txtWilayah.ButtonClick
        'Select Case e.Button.Index
        '    Case 0
        '        Dim x As New frmLUWilayah
        '        x.pStatus = publik.ptipe.LookUp
        '        x.IDDepartemen = IDDepartemen
        '        If x.ShowDialog(Me)= Windows.Forms.DialogResult.OK Then
        '            IDWilayah = x.NoID
        '            txtWilayah.Text = String.Format("{0} - {1}", x.Kode, x.Nama)
        '            SetWilayah()
        '        End If
        '        x.Dispose()
        '    Case 1
        '        IDWilayah = -1
        '        txtWilayah.Text = ""
        '        SetWilayah()
        'End Select
    End Sub

    Private Sub txtJumlah_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)

    End Sub

    Private Sub txtJumlah_EditValueChanged_1(ByVal sender As System.Object, ByVal e As System.EventArgs)

    End Sub

    Private Sub txtWilayah_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtWilayah.EditValueChanged

    End Sub

    Private Sub txtGudang_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtGudang.EditValueChanged

    End Sub

    Private Sub txtAkun_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtAkun.EditValueChanged

    End Sub
End Class