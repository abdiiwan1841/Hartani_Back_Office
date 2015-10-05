 
Imports FormsAlias = System.Windows.Forms

Public Class frmEntriAkun

    Public IDAkun As Long
    Public pStatus As mdlAccPublik.ptipe, namaTabel As String = "makun"
    Public IDKelompok As Long, IDSubKlas As Long
    Public IDMataUang As Long
    Public IDDepartemen As Long
    Public DebetA As Double
    Public KodeMataUang As String
    Dim KodeLama As String

    Private Sub cmdClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdClose.Click
        DialogResult = FormsAlias.DialogResult.Cancel
        close()
    End Sub

    Private Sub frmEntriKasInD_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If pStatus = mdlAccPublik.ptipe.Edit Then
            GetData("SELECT " & namaTabel & ".*, msubklasakun.Nama as NamaSubKlas, MKelompokLabaRugi.Nama AS NamaKelompok " & vbCrLf)
        Else
            IsiDefault()
        End If
    End Sub
    Private Sub GetData(ByVal strcekrecord As String)
        Dim oDS As New DataSet
        strcekrecord = strcekrecord & " FROM makun" & vbCrLf
        strcekrecord = strcekrecord & " LEFT JOIN msubklasakun ON makun.IDSubKlasAkun = msubklasakun.ID" & vbCrLf
        strcekrecord = strcekrecord & " LEFT JOIN mmatauang ON makun.IDMataUang = mmatauang.ID" & vbCrLf
        strcekrecord = strcekrecord & " LEFT JOIN mkelompoklabarugi ON makun.IDKelompokLabaRugi = mkelompoklabarugi.ID" & vbCrLf
        strcekrecord = strcekrecord & " LEFT JOIN mdepartemen ON makun.IdDepartemen = mdepartemen.NoID" & vbCrLf
        strcekrecord = strcekrecord & " WHERE " & namaTabel & ".ID = " & IDAkun

        Try
            oDS = ExecuteDataset(namaTabel, strcekrecord)
            If oDS.Tables(namaTabel).Rows.Count >= 1 Then
                IDAkun = NullToLong(oDS.Tables(namaTabel).Rows(0).Item("ID").ToString)
                IDDepartemen = NullToLong(oDS.Tables(namaTabel).Rows(0).Item("IDDepartemen").ToString)
                IDMataUang = NullToLong(oDS.Tables(namaTabel).Rows(0).Item("IDMataUang").ToString)
                IDKelompok = NullToLong(oDS.Tables(namaTabel).Rows(0).Item("IDKelompokLabaRugi").ToString)
                IDSubKlas = NullToLong(oDS.Tables(namaTabel).Rows(0).Item("IDSubKlasAkun").ToString)
                txtKelompok.Text = NullTostr(oDS.Tables(namaTabel).Rows(0).Item("NamaKelompok").ToString)
                txtSubKlas.Text = NullTostr(oDS.Tables(namaTabel).Rows(0).Item("NamaSubKlas").ToString)
                txtKode.Text = NullTostr(oDS.Tables(namaTabel).Rows(0).Item("Kode").ToString)
                txtNama.Text = NullTostr(oDS.Tables(namaTabel).Rows(0).Item("Nama").ToString)
                txtAlias.Text = NullTostr(oDS.Tables(namaTabel).Rows(0).Item("Alias").ToString)
                ckAktif.Checked = NullToBool(oDS.Tables(namaTabel).Rows(0).Item("IsAktif"))
                ckKas.Checked = NullToBool(oDS.Tables(namaTabel).Rows(0).Item("IsKas"))
                KodeLama = txtKode.Text
            End If
        Catch ex As Exception
            FxMessage(ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error, , , ex.StackTrace)
        Finally
            oDS.Dispose()
        End Try
    End Sub
    Sub IsiDefault()
    End Sub

    Private Sub cmdSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSave.Click
        Dim SQL As String, curentcursor As FormsAlias.Cursor = FormsAlias.Cursor.Current
        Dim dlg As DevExpress.Utils.WaitDialogForm = New DevExpress.Utils.WaitDialogForm("Menyimpan data." & vbCrLf & "Mohon tunggu beberapa saat ...", NamaAplikasi)
        dlg.TopMost = True
        dlg.Show()
        FormsAlias.Cursor.Current = FormsAlias.Cursors.WaitCursor
        If IsValidasi() Then
            If pStatus = mdlAccPublik.ptipe.Edit Then
                SQL = "Update " & namaTabel & " Set " & _
                       "IDDepartemen=" & IDDepartemen & "," & _
                       "IDMataUang=" & IDMataUang & "," & _
                       "IDKelompokLabaRugi=" & IDKelompok & "," & _
                       "IDSubKlasAkun=" & IDSubKlas & "," & _
                       "Nama='" & FixApostropi(txtNama.Text) & "'," & _
                       "Kode='" & FixApostropi(txtKode.Text) & "'," & _
                       "Alias='" & FixApostropi(txtAlias.Text) & "'," & _
                       "IsAktif=" & IIf(ckAktif.Checked, 1, 0) & "," & _
                       "IsKas=" & IIf(ckKas.Checked, 1, 0) & _
                       " WHERE ID=" & IDAkun
            Else
                IDAkun = GetNewID(namaTabel, "ID")
                SQL = "Insert Into " & namaTabel & "(ID,Kode,Nama,Alias," & _
                      "IDMataUang,IDDepartemen,IDSubKlasAkun,IDKelompokLabaRugi,IsAktif,IsKas) " & _
                      " VALUES " & _
                      "(" & IDAkun & ",'" & FixApostropi(txtKode.Text) & "','" & FixApostropi(txtNama.Text) & "','" & FixApostropi(txtAlias.Text) & "'," & _
                      IDMataUang & "," & IDDepartemen & "," & IDSubKlas & "," & IDKelompok & "," & IIf(ckAktif.Checked, 1, 0) & "," & IIf(ckKas.Checked, 1, 0) & ")"
            End If
            EksekusiSQL(SQL)
            dlg.Close()
            dlg.Dispose()
            FormsAlias.Cursor.Current = Cursors.Default
            DialogResult = FormsAlias.DialogResult.OK
            Close()
        End If
        dlg.Close()
        dlg.Dispose()
        FormsAlias.Cursor.Current = Cursors.Default
    End Sub

    Private Function IsValidasi() As Boolean
        If txtKode.Text = "" Then
            FxMessage("Kode masih kosong.", NamaAplikasi)
            IsValidasi = False
            txtSubKlas.Focus()
            Exit Function
        End If
        If txtNama.Text = "" Then
            FxMessage("Nama masih kosong.", NamaAplikasi)
            IsValidasi = False
            txtKelompok.Focus()
            Exit Function
        End If

        If CekKodeValid(txtKode.Text, KodeLama, namaTabel, "Kode", IIf(pStatus = mdlAccPublik.ptipe.Baru, False, True), " AND " & namaTabel & ".IDDepartemen=" & IDDepartemen) Then
            FxMessage("Kode sudah dipakai.", NamaAplikasi)
            IsValidasi = False
            Exit Function
        End If
        IsValidasi = True
    End Function



    Private Sub txtSubKlas_ButtonClick(ByVal sender As Object, ByVal e As DevExpress.XtraEditors.Controls.ButtonPressedEventArgs) Handles txtSubKlas.ButtonClick
        Select Case e.Button.Index
            Case 0
                Dim x As New frmLUSubKlasAkun
                x.pStatus = mdlAccPublik.ptipe.LookUp
                If x.ShowDialog(Me) = FormsAlias.DialogResult.OK Then
                    IDSubKlas = x.NoID
                    txtSubKlas.Text = x.Nama
                End If
                x.Dispose()
            Case 1
                IDSubKlas = -1
                txtSubKlas.Text = ""
        End Select
    End Sub

    Private Sub txtKelompok_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtKelompok.EditValueChanged

    End Sub

    Private Sub txtSubKlas_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtSubKlas.EditValueChanged

    End Sub

    Private Sub txtMataUang_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)

    End Sub

    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

    End Sub
End Class