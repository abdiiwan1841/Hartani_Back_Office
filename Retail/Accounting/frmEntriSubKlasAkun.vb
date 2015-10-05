
Public Class frmEntriSubKlasAkun

    Public ID As Long
    Public pStatus As mdlAccPublik.Ptipe, namaTabel As String = "msubklasakun"
    Public IDKlasAkun As Long
    Dim KodeLama As String
    Private Sub cmdClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdClose.Click
        DialogResult = Windows.Forms.DialogResult.Cancel
        close()
    End Sub

    Private Sub frmEntriKasInD_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If pStatus = mdlAccPublik.ptipe.Edit Then
            GetData()
        Else
            IsiDefault()
        End If
    End Sub
    Private Sub GetData()
        Dim strcekrecord As String
        Dim oDS As New DataSet
        strcekrecord = "SELECT  " & namaTabel & ".*, MKlasAkun.Nama as NamaKlas, MKlasAkun.Kode as KodeKlas " & vbCrLf
        strcekrecord = strcekrecord & " FROM " & namaTabel & vbCrLf
        strcekrecord = strcekrecord & " LEFT JOIN MKlasAkun ON " & namaTabel & ".IDKlasAKun = MKlasAkun.ID" & vbCrLf
        strcekrecord = strcekrecord & " WHERE " & namaTabel & ".ID = " & ID
        Try
            oDS = ExecuteDataset(namaTabel, strcekrecord)
            If oDS.Tables(namaTabel).Rows.Count >= 1 Then
                ID = NullToLong(oDS.Tables(namaTabel).Rows(0).Item("ID").ToString)
                IDKlasAkun = NullToLong(oDS.Tables(namaTabel).Rows(0).Item("IDKlasAkun").ToString)
                txtKlas.Text = NullToStr(oDS.Tables(namaTabel).Rows(0).Item("NamaKlas").ToString)
                txtKode.Text = NullToStr(oDS.Tables(namaTabel).Rows(0).Item("Kode").ToString)
                txtNama.Text = NullToStr(oDS.Tables(namaTabel).Rows(0).Item("Nama").ToString)
                txtAlias.Text = NullToStr(oDS.Tables(namaTabel).Rows(0).Item("Alias").ToString)
                KodeLama = txtKode.Text
            End If
        Catch ex As Exception
            FxMessage(ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error, , , ex.StackTrace)
        End Try
    End Sub
    Sub IsiDefault()
        IDKlasAkun = 0
    End Sub

    Private Sub cmdSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSave.Click
        Dim SQL As String, curentcursor As Windows.Forms.Cursor = Windows.Forms.Cursor.Current
        Dim dlg As DevExpress.Utils.WaitDialogForm
        dlg = New DevExpress.Utils.WaitDialogForm("Menyimpan data." & vbCrLf & "Mohon tunggu beberapa saat ...", NamaAplikasi)
        dlg.TopMost = True
        dlg.Show()
        Windows.Forms.Cursor.Current = Windows.Forms.Cursors.WaitCursor
        If IsValidasi() Then
            If pStatus = mdlAccPublik.ptipe.Edit Then
                SQL = "Update " & namaTabel & " Set " & _
                       "IDKlasAkun=" & IDKlasAkun & "," & _
                       "Nama='" & FixApostropi(txtNama.Text) & "'," & _
                       "Kode='" & FixApostropi(txtKode.Text) & "'," & _
                       "Alias='" & FixApostropi(txtAlias.Text) & "' " & _
                       " WHERE ID=" & ID
            Else
                ID = GetNewID(namaTabel, "ID")
                SQL = "Insert Into " & namaTabel & "(ID,Kode,Nama,Alias," & _
                      "IDKlasAkun) " & _
                      " VALUES " & _
                      "(" & ID & ",'" & FixApostropi(txtKode.Text) & "','" & FixApostropi(txtNama.Text) & "','" & FixApostropi(txtAlias.Text) & "'," & _
                      IDKlasAkun & ")"
            End If
            If EksekusiSQL(SQL) Then
                dlg.Close()
                dlg.Dispose()
                Windows.Forms.Cursor.Current = Cursors.Default
                DialogResult = Windows.Forms.DialogResult.OK
                Close()
            End If
        End If
        dlg.Close()
        dlg.Dispose()
        Windows.Forms.Cursor.Current = Cursors.Default
    End Sub

    Private Function IsValidasi() As Boolean
        If txtKode.Text = "" Then
            FxMessage("Kode masih kosong.", NamaAplikasi)
            IsValidasi = False
            txtKode.Focus()
            Exit Function
        End If
        If txtNama.Text = "" Then
            FxMessage("Nama masih kosong.", NamaAplikasi)
            IsValidasi = False
            txtNama.Focus()
            Exit Function
        End If
        If CekKodeValid(txtKode.Text, KodeLama, namaTabel, "Kode", IIf(pStatus = mdlAccPublik.ptipe.Baru, False, True)) Then
            FxMessage("Kode sudah dipakai.", NamaAplikasi)
            IsValidasi = False
            Exit Function
        End If
        IsValidasi = True
    End Function

    Private Sub txtKlas_ButtonClick(ByVal sender As Object, ByVal e As DevExpress.XtraEditors.Controls.ButtonPressedEventArgs) Handles txtKlas.ButtonClick
        Select Case e.Button.Index
            Case 0
                Dim x As New frmLUKlasAkun
                x.pStatus = mdlAccPublik.ptipe.LookUp
                If x.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
                    IDKlasAkun = x.NoID
                    txtKlas.Text = x.Nama
                End If
                x.Dispose()
            Case 1
                IDKlasAkun = -1
                txtKlas.Text = ""
        End Select
    End Sub

    Private Sub txtKlas_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtKlas.EditValueChanged

    End Sub

    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

    End Sub
End Class