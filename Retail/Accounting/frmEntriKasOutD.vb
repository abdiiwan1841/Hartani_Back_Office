 

Public Class frmEntriKasOutD
    Public ID As Long
    Public pStatus As mdlAccPublik.Ptipe, namaTabel As String = "TKasOutD"
    Public IDAkun, IDGudang As Long
    Public IDMataUang As Long
    Public IDDepartemen, IDWilayah As Long
    Public DebetA As Double, KreditA As Double
    Public KodeAkun As String = "", NamaAkun As String = ""
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
        LayoutControl1.SaveLayoutToXml(Application.StartupPath & "\System\Layouts\" & Me.Name & Me.LayoutControl1.Name & ".xml")
    End Sub
    Private Sub SetCtlMe()
        If txtGudang.Text = "" Then
            IDGudang = DefIDGudang
            txtGudang.Text = DefNamaGudang
        End If

    End Sub
    Private Sub frmEntriKasInD_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'DialogResult = Windows.Forms.DialogResult.Cancel
        SetCtlMe()
        If pStatus = mdlAccPublik.ptipe.Edit Then
            GetData()
        Else
            IsiDefault()
        End If
        If System.IO.File.Exists(Application.StartupPath & "\System\Layouts\" & Me.Name & Me.LayoutControl1.Name & ".xml") Then
            LayoutControl1.RestoreLayoutFromXml(Application.StartupPath & "\System\Layouts\" & Me.Name & Me.LayoutControl1.Name & ".xml")
        End If
    End Sub
    Private Sub GetData()
        Dim strcekrecord As String
        Dim oDS As New DataSet
        strcekrecord = "select " & namaTabel & ".*, MGudang.Nama AS NamaGudang "
        strcekrecord &= " FROM " & namaTabel & vbCrLf
        strcekrecord &= " LEFT JOIN MGudang ON  " & namaTabel & ".IDGudang=MGudang.NoID  " & vbCrLf
        strcekrecord &= " WHERE " & namaTabel & ".IDTerminal = " & IDUserAktif & " And " & namaTabel & ".ID = " & ID
        Try
            oDS = ExecuteDataset(namaTabel, strcekrecord)
            If oDS.Tables(namaTabel).Rows.Count >= 1 Then
                ID = NullToLong(oDS.Tables(namaTabel).Rows(0).Item("ID").ToString)
                IDAkun = NullToLong(oDS.Tables(namaTabel).Rows(0).Item("IDAkun").ToString)
                txtCatatan.Text = NullToStr(oDS.Tables(namaTabel).Rows(0).Item("Keterangan"))
                txtJml.EditValue = NullToDbl(oDS.Tables(namaTabel).Rows(0).Item("Debet"))
                KodeAkun = NullToStr(oDS.Tables(namaTabel).Rows(0).Item("KodeAkun").ToString)
                NamaAkun = NullToStr(oDS.Tables(namaTabel).Rows(0).Item("NamaAkun").ToString)
                txtAkun.Text = NamaAkun
                IDGudang = NullToLong(oDS.Tables(namaTabel).Rows(0).Item("IDGudang").ToString)
                txtGudang.Text = NullToStr(oDS.Tables(namaTabel).Rows(0).Item("NamaGudang").ToString)
            End If
        Catch ex As Exception
            FxMessage(ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error, , , ex.StackTrace)
        End Try
    End Sub
    Private Sub IsiDefault()
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
                If pStatus = mdlAccPublik.ptipe.Baru Then
                    SQL = " Insert Into " & namaTabel & "(IDTerminal,IDGudang,IDAkun,Debet,Kredit,KodeAkun,NamaAkun,Keterangan,NoPOD) " & vbCrLf
                    SQL = SQL & " VALUES (" & IDUserAktif & ","
                    SQL = SQL & IDGudang & ","
                    SQL = SQL & IDAkun & ","
                    SQL = SQL & FixKoma(txtJml.EditValue) & ",0,"
                    SQL = SQL & "'" & FixApostropi(IIf(KodeAkun = Nothing, "", KodeAkun)) & "',"
                    SQL = SQL & "'" & FixApostropi(txtAkun.Text) & "',"
                    SQL = SQL & "'" & FixApostropi(txtCatatan.Text) & "',"
                    SQL = SQL & "'" & FixApostropi(NullToStr(NoPOD)) & "'"
                    SQL = SQL & ") "
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
        If pStatus = mdlAccPublik.ptipe.Edit Then
            Dim SQL As String
            SQL = "Update " & namaTabel & " Set " & vbCrLf
            SQL = SQL & "IDTerminal=" & IDUserAktif & ","
            SQL = SQL & "IDGudang=" & IIf(JenisTransaksi = JenisTransaksi_.All, IDGudang, "NULL") & ","
            SQL = SQL & "IDAkun=" & IIf(JenisTransaksi = JenisTransaksi_.All, IDAkun, "NULL") & ","
            SQL = SQL & "Debet=" & FixKoma(txtJml.EditValue) & ","
            SQL = SQL & "Kredit=" & 0 & ","
            SQL = SQL & "KodeAkun='" & FixApostropi(IIf(KodeAkun = Nothing, "", KodeAkun)) & "',"
            SQL = SQL & "NamaAkun='" & FixApostropi(txtAkun.Text) & "',"
            SQL = SQL & "Keterangan='" & FixApostropi(txtCatatan.Text) & "',"
            SQL = SQL & "NoPOD='" & FixApostropi(NullToStr(NoPOD)) & "' "
            SQL = SQL & "WHERE ID=" & ID
            EksekusiSQL(SQL)
        End If
    End Sub

    Private Function IsValidasi() As Boolean
        If JenisTransaksi = JenisTransaksi_.All AndAlso txtAkun.Text = "" Then
            MsgBox("Akun masih kosong.", NamaAplikasi)
            IsValidasi = False
            txtAkun.Focus()
            Exit Function
        End If

        If JenisTransaksi = JenisTransaksi_.All AndAlso txtGudang.Text = "" Then
            MsgBox("Gudang masih kosong.", NamaAplikasi)
            IsValidasi = False
            txtGudang.Focus()
            Exit Function
        End If

        If CDbl(txtJml.EditValue) = 0 Then
            MsgBox("Jumlah masih nol.", NamaAplikasi)
            IsValidasi = False
            txtJml.Focus()
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
                End If
                x.Dispose()
            Case 1
                IDAkun = -1
                txtAkun.Text = ""
        End Select
    End Sub


    Private Sub txtGudang_ButtonClick(ByVal sender As Object, ByVal e As DevExpress.XtraEditors.Controls.ButtonPressedEventArgs) Handles txtGudang.ButtonClick
        Select Case e.Button.Index
            Case 0
                Dim x As New frmLUGudang
                x.pStatus = mdlAccPublik.ptipe.LookUp
                If x.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
                    IDGudang = x.NoID
                    txtGudang.Text = x.Nama
                End If
                x.Dispose()
            Case 1
                IDGudang = -1
                txtGudang.Text = ""
        End Select
    End Sub

    Private Sub txtGudang_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtGudang.EditValueChanged

    End Sub
        
    Private Sub txtJml_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtJml.EditValueChanged

    End Sub

    Private Sub txtJml_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtJml.GotFocus
        txtJml.SelectAll()
    End Sub
End Class