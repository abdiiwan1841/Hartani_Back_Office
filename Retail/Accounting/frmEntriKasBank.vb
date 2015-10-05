Imports System.Data
Imports DevExpress.XtraEditors
Imports DevExpress.Utils

Public Class frmEntriKasBank
    Public Enum ptipe
        Lihat = 0
        LookUp = 1
        LookUpParent = 2
        Baru = 3
        Edit = 4
    End Enum
    Public ID, IDAKun, IDGudang, IDMataUang As Long
    Public pStatus As mdlAccPublik.Ptipe, namaTabel As String = "MBank"
    Dim KodeLama As String
    Dim IDKasTujuan As Long = -1
    Dim IDAkunKasTujuan As Long = -1

    Private Sub cmdClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdClose.Click
        DialogResult = Windows.Forms.DialogResult.Cancel
        close()
    End Sub
    Private Sub SetCtlMe()

        If System.IO.File.Exists(FolderLayouts & Me.Name & LayoutControl1.Name & ".xml") Then
            LayoutControl1.RestoreLayoutFromXml(FolderLayouts & Me.Name & LayoutControl1.Name & ".xml")
        End If
        IDGudang = DefIDGudang
        txtGudang.Text = NullToStr(EksekusiSQlSkalarNew("SELECT MGudang.Kode + ' - ' + MGudang.Nama + ' - ' + MWilayah.Nama FROM MGudang INNER JOIN MWilayah ON MWilayah.NoID=MGudang.IDWIlayah WHERE MGUdang.NoID=" & DefIDGudang))
        RadioGroup1.SelectedIndex = 0
        Dim x As New System.EventArgs
        RadioGroup1_SelectedIndexChanged(0, x)
        'If Not IsAdministrator Then
        '    txtGudang.Enabled = False
        'End If
    End Sub

    Private Sub frmEntriKasBank_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        LayoutControl1.SaveLayoutToXml(FolderLayouts & Me.Name & LayoutControl1.Name & ".xml")
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
        strcekrecord = "SELECT " & namaTabel & ".*,  MGudang.Nama as Gudang, MAkun.Nama as NamaAkun " & vbCrLf
        strcekrecord = strcekrecord & " FROM ((" & namaTabel & vbCrLf
        strcekrecord = strcekrecord & " LEFT JOIN MGudang ON MGudang.NoID=" & namaTabel & ".IDGudang) "
        strcekrecord = strcekrecord & " LEFT JOIN MAkun ON MAkun.ID=" & namaTabel & ".IDAKun) "
        strcekrecord = strcekrecord & " WHERE " & namaTabel & ".NoID = " & ID
        Try
            oDS = ExecuteDataset(namaTabel, strcekrecord)
            If oDS.Tables(namaTabel).Rows.Count >= 1 Then
                IDGudang = NullToLong(oDS.Tables(namaTabel).Rows(0).Item("IDGudang"))
                txtGudang.Text = NullToStr(oDS.Tables(namaTabel).Rows(0).Item("Gudang"))
                IDAKun = NullToLong(oDS.Tables(namaTabel).Rows(0).Item("IDAkun"))
                txtAkun.Text = NullToStr(oDS.Tables(namaTabel).Rows(0).Item("NamaAkun"))
                txtAlamat.EditValue = NullToStr(oDS.Tables(namaTabel).Rows(0).Item("Alamat"))
                txtKode.Text = NullToStr(oDS.Tables(namaTabel).Rows(0).Item("Kode"))
                txtNama.Text = NullToStr(oDS.Tables(namaTabel).Rows(0).Item("Nama"))
                txtKota.Text = NullToStr(oDS.Tables(namaTabel).Rows(0).Item("Kota"))
                txtTelp.Text = NullToStr(oDS.Tables(namaTabel).Rows(0).Item("NoTelp"))
                txtNorek.Text = NullToStr(oDS.Tables(namaTabel).Rows(0).Item("NoRekening"))
                ckAktif.Checked = NullToBool(oDS.Tables(namaTabel).Rows(0).Item("IsActive"))
                IDMataUang = NullToLong(oDS.Tables(namaTabel).Rows(0).Item("IDMataUang"))
                txtNamaBank.Text = NullToStr(oDS.Tables(namaTabel).Rows(0).Item("NamaBank"))
                txtNamaRekening.Text = NullToStr(oDS.Tables(namaTabel).Rows(0).Item("NamaRekening"))
                txtCabangBank.Text = NullToStr(oDS.Tables(namaTabel).Rows(0).Item("Cabang"))
                txtKet.Text = NullToStr(oDS.Tables(namaTabel).Rows(0).Item("Keterangan"))
                KodeLama = txtKode.Text
                If NullToBool(oDS.Tables(namaTabel).Rows(0).Item("IsKas")) Then
                    RadioGroup1.SelectedIndex = 0
                Else
                    RadioGroup1.SelectedIndex = 1
                End If
                Dim x As New System.EventArgs
                RadioGroup1_SelectedIndexChanged(0, x)
            End If
        Catch ex As Exception
            FxMessage(ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error, , , ex.StackTrace)
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
            If pStatus = mdlAccPublik.ptipe.Edit Then
                SQL = "Update " & namaTabel & " Set " & _
                      "IDGudang=" & IDGudang & "," & _
                      "IDAKun=" & IDAKun & "," & _
                      "Nama='" & FixApostropi(txtNama.Text) & "'," & _
                      "Kode='" & FixApostropi(txtKode.Text) & "'," & _
                      "Alamat='" & FixApostropi(txtAlamat.Text) & "'," & _
                      "Kota='" & FixApostropi(txtKota.Text) & "'," & _
                      "NoTelp='" & FixApostropi(txtTelp.Text) & "'," & _
                      "NoRekening='" & FixApostropi(txtNorek.Text) & "'," & _
                      "NamaRekening='" & FixApostropi(txtNamaRekening.Text) & "'," & _
                         "NamaBank='" & FixApostropi(txtNamaBank.Text) & "'," & _
                   "Cabang='" & FixApostropi(txtCabangBank.Text) & "'," & _
                      "Keterangan='" & FixApostropi(txtKet.Text) & "'," & _
                      "IsKas=" & IIf(RadioGroup1.SelectedIndex = 0, 1, 0) & ", " & _
                      "IsActive=" & IIf(ckAktif.Checked = True, 1, 0) & " " & _
                      " WHERE NoID=" & ID
            Else
                ID = GetNewID(namaTabel, "NoID")
                SQL = "Insert Into " & namaTabel & "(NoID,NamaRekening,Nama,Cabang,Keterangan,IDGudang,IDAkun,Kode,NamaBank,Alamat,Kota,NoTelp,NoRekening," & _
                      "IsActive,IsKas) " & _
                      " VALUES " & _
                      "(" & ID & ",'" & FixApostropi(txtNamaRekening.Text) & "','" & FixApostropi(txtNama.Text) & "','" & FixApostropi(txtCabangBank.Text) & "','" & FixApostropi(txtKet.Text) & "'," & IDGudang & "," & IDAKun & ",'" & FixApostropi(txtKode.Text) & "','" & FixApostropi(txtNamaBank.Text) & "','" & FixApostropi(txtAlamat.Text) & "'," & _
                      "'" & FixApostropi(txtKota.Text) & "','" & FixApostropi(txtTelp.Text) & "','" & FixApostropi(txtNorek.Text) & "'," & IIf(ckAktif.Checked = True, 1, 0) & "," & IIf(RadioGroup1.SelectedIndex = 0, 1, 0) & ")"
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
        If txtGudang.Text = "" Then
            FxMessage("Gudang masih kosong.", NamaAplikasi)
            IsValidasi = False
            txtGudang.Focus()
            Exit Function
        End If
        If txtAkun.Text = "" Then
            FxMessage("Akun masih kosong.", NamaAplikasi)
            IsValidasi = False
            txtAkun.Focus()
            Exit Function
        End If
        If CekKodeValid(txtKode.Text, KodeLama, namaTabel, "Kode", IIf(pStatus = mdlAccPublik.ptipe.Baru, False, True)) Then
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

    Private Sub txtGudang_ButtonClick(ByVal sender As Object, ByVal e As DevExpress.XtraEditors.Controls.ButtonPressedEventArgs) Handles txtGudang.ButtonClick
        Select Case e.Button.Index
            Case 0
                Dim x As New frmLUGudang
                x.IDWilayah = DefIDWilayah
                x.pStatus = mdlAccPublik.ptipe.LookUp
                If x.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
                    IDGudang = x.NoID
                    txtGudang.Text = String.Format("{0} - {1} - {2}", x.Kode, x.Nama, x.Wilayah)
                End If
                x.Dispose()
            Case 1
                IDGudang = -1
                txtGudang.Text = ""
        End Select
    End Sub

    Private Sub txtWilayah_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtGudang.EditValueChanged

    End Sub

    Private Sub txtAkun_ButtonClick(ByVal sender As Object, ByVal e As DevExpress.XtraEditors.Controls.ButtonPressedEventArgs) Handles txtAkun.ButtonClick
        Select Case e.Button.Index
            Case 0
                Dim x As New frmLUAkun
                x.pStatus = mdlAccPublik.ptipe.LookUp
                If x.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
                    IDAKun = x.NoID
                    txtAkun.Text = String.Format("{0} - {1}", x.Kode, x.Nama)
                End If
                x.Dispose()
            Case 1
                IDAKun = -1
                txtAkun.Text = ""
        End Select
    End Sub

    Private Sub txtAkun_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtAkun.EditValueChanged

    End Sub

    Private Sub RadioGroup1_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RadioGroup1.SelectedIndexChanged
        If RadioGroup1.SelectedIndex = 0 Then
            txtNamaRekening.Visible = False
        Else
            txtNamaRekening.Visible = True
        End If
        txtNorek.Visible = txtNamaRekening.Visible
        txtNama.Visible = txtNamaRekening.Visible
        txtCabangBank.Visible = txtNamaRekening.Visible
    End Sub



End Class