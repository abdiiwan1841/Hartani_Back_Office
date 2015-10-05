'Imports VPOINT.Function.mPublic
'Imports VPOINT.Function.Fungsi
Imports System.Data
Imports DevExpress.XtraEditors
Imports DevExpress.Utils
Imports VPoint.FungsiControl

Public Class frmEntriTypeAsset

    Public ID, IDAAkumulasiSusut, IDAAsset, IDASusut As Long
    Public namaTabel As String = "MTypeAsset"
    Dim KodeLama As String
    Public pStatus As mdlAccPublik.ptipe

    Private Sub cmdClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdClose.Click
        DialogResult = Windows.Forms.DialogResult.Cancel
        close()
    End Sub
    Private Sub SetCtlMe()
        setform(Me)
        SetButton(cmdClose, button_.cmdCancelSave)
        SetButton(cmdSave, button_.cmdSave)

    End Sub
    Private Sub frmEntriKasInD_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        SetCtlMe()
        If pStatus = mdlAccPublik.ptipe.Edit Then
            GetData()
        End If
    End Sub
    Private Sub GetData()
        Dim strcekrecord As String = ""
        Dim oDS As New DataSet
        strcekrecord = "SELECT " & namaTabel & ".*, MAAsset.Nama as AAsset,MAAkumulasi.Nama as AAkumulasi,MABiaya.Nama as ABiaya "
        strcekrecord = strcekrecord & " FROM ((" & namaTabel & vbCrLf
        strcekrecord = strcekrecord & " LEFT JOIN MAkun MAAsset ON " & namaTabel & ".IDAAsset=MAAsset.ID) "
        strcekrecord = strcekrecord & " LEFT JOIN MAkun MAAkumulasi ON " & namaTabel & ".IDAAkumulasiSusut=MAAkumulasi.ID) "
        strcekrecord = strcekrecord & " LEFT JOIN MAkun MASusut ON " & namaTabel & ".IDASusut=MASusut.ID "
        strcekrecord = strcekrecord & " WHERE " & namaTabel & ".NoID = " & ID
        Try
            oDS = ExecuteDataset(namaTabel, strcekrecord)
            If oDS.Tables(namaTabel).Rows.Count >= 1 Then
                IDAAsset = NullToLong(oDS.Tables(namaTabel).Rows(0).Item("IDAAsset"))
                IDAAkumulasiSusut = NullToLong(oDS.Tables(namaTabel).Rows(0).Item("IDAAkumulasiSusut"))
                IDASusut = NullToLong(oDS.Tables(namaTabel).Rows(0).Item("IDASusut"))
                txtAAsset.Text = NullTostr(oDS.Tables(namaTabel).Rows(0).Item("AAsset"))
                txtAAkumulasi.Text = NullTostr(oDS.Tables(namaTabel).Rows(0).Item("AAkumulasi"))
                txtABiaya.Text = NullTostr(oDS.Tables(namaTabel).Rows(0).Item("ABiaya"))
                txtKode.Text = NullTostr(oDS.Tables(namaTabel).Rows(0).Item("Kode"))
                txtNama.Text = NullTostr(oDS.Tables(namaTabel).Rows(0).Item("Nama"))
                txtKet.Text = NullTostr(oDS.Tables(namaTabel).Rows(0).Item("Keterangan"))
                txtAlamat.EditValue = NullToLong(oDS.Tables(namaTabel).Rows(0).Item("BulanEkonomis"))
                ckAktif.Checked = NullToBool(oDS.Tables(namaTabel).Rows(0).Item("IsAktif"))
                KodeLama = txtKode.Text
            End If
        Catch ex As Exception
            FxMessage(ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            oDS.Dispose()
        End Try
    End Sub

    Private Sub cmdSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSave.Click
        Dim SQL As String, curentcursor As Windows.Forms.Cursor = Windows.Forms.Cursor.Current
        Dim dlg As DevExpress.Utils.WaitDialogForm = New DevExpress.Utils.WaitDialogForm("Menyimpan data." & vbCrLf & "Mohon tunggu beberapa saat ...", NamaAplikasi)
        dlg.TopMost = True
        dlg.Show()
        Windows.Forms.Cursor.Current = Windows.Forms.Cursors.WaitCursor
        If IsValidasi() Then
            If pStatus = ptipe.Edit Then
                SQL = "Update " & namaTabel & " Set " & _
                      "IDAAsset=" & IDAAsset & "," & _
                      "IDASusut=" & IDASusut & "," & _
                      "IDAAkumulasiSusut=" & IDAAkumulasiSusut & "," & _
                      "Kode='" & FixApostropi(txtKode.Text) & "'," & _
                      "Nama='" & FixApostropi(txtNama.Text) & "'," & _
                      "Keterangan='" & FixApostropi(txtKet.Text) & "'," & _
                      "BulanEkonomis=" & FixKoma(txtAlamat.EditValue) & "," & _
                      "IsAktif=" & IIf(ckAktif.Checked = True, 1, 0) & " " & _
                      " WHERE NoID=" & ID
            Else
                ID = GetNewID(namaTabel, "NoID")
                SQL = "Insert Into " & namaTabel & "(NoID,IDAAsset,IDASusut,IDAAkumulasiSusut,Kode,Nama,Keterangan,BulanEkonomis," & _
                      "IsAktif) " & _
                      " VALUES " & _
                      "(" & ID & "," & IDAAsset & "," & IDASusut & "," & IDAAkumulasiSusut & ",'" & FixApostropi(txtKode.Text) & "','" & FixApostropi(txtNama.Text) & "','" & FixApostropi(txtKet.Text) & "'," & _
                      "" & FixKoma(txtAlamat.EditValue) & "," & IIf(ckAktif.Checked = True, 1, 0) & ")"
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
        If txtAAsset.Text = "" Then
            FxMessage("Gudang masih kosong.", NamaAplikasi)
            IsValidasi = False
            txtAAsset.Focus()
            Exit Function
        End If
        If txtAAkumulasi.Text = "" Then
            FxMessage("Akun masih kosong.", NamaAplikasi)
            IsValidasi = False
            txtAAkumulasi.Focus()
            Exit Function
        End If
        If CekKodeValid(txtKode.Text, KodeLama, namaTabel, "Kode", IIf(pStatus = ptipe.Baru, False, True)) Then
            FxMessage("Kode sudah dipakai.", NamaAplikasi)
            IsValidasi = False
            Exit Function
        End If
        IsValidasi = True
    End Function

    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

    End Sub

    Private Sub txtAAsset_ButtonClick(ByVal sender As Object, ByVal e As DevExpress.XtraEditors.Controls.ButtonPressedEventArgs) Handles txtAAsset.ButtonClick
        Select Case e.Button.Index
            Case 0
                Dim x As New frmLUAkun
                x.pStatus = ptipe.LookUp
                If x.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
                    IDAAsset = x.NoID
                    txtAAsset.Text = x.Nama 'String.Format("{0} - {1}", x.Kode, x.Nama)
                End If
                x.Dispose()
            Case 1
                IDAAsset = -1
                txtAAsset.Text = ""
        End Select
    End Sub

    Private Sub txtAAsset_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtAAsset.EditValueChanged

    End Sub

    Private Sub txtAAkumulasi_ButtonClick(ByVal sender As Object, ByVal e As DevExpress.XtraEditors.Controls.ButtonPressedEventArgs) Handles txtAAkumulasi.ButtonClick
        Select Case e.Button.Index
            Case 0
                Dim x As New frmLUAkun
                x.pStatus = ptipe.LookUp
                If x.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
                    IDAAkumulasiSusut = x.NoID
                    txtAAkumulasi.Text = x.Nama 'String.Format("{0} - {1}", x.Kode, x.Nama)
                End If
                x.Dispose()
            Case 1
                IDAAkumulasiSusut = -1
                txtAAkumulasi.Text = ""
        End Select
    End Sub

    Private Sub txtAAkumulasi_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtAAkumulasi.EditValueChanged

    End Sub

    Private Sub txtABiaya_ButtonClick(ByVal sender As Object, ByVal e As DevExpress.XtraEditors.Controls.ButtonPressedEventArgs) Handles txtABiaya.ButtonClick
        Select Case e.Button.Index
            Case 0
                Dim x As New frmLUAkun
                x.pStatus = ptipe.LookUp
                If x.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
                    IDASusut = x.NoID
                    txtABiaya.Text = x.Nama 'String.Format("{0} - {1}", x.Kode, x.Nama)
                End If
                x.Dispose()
            Case 1
                IDASusut = -1
                txtABiaya.Text = ""
        End Select
    End Sub
End Class