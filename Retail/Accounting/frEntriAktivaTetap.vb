Imports System.Data.SqlClient
Imports System.Data
Imports DevExpress.XtraEditors

Public Class frEntriAktivaTetap

    Public Enum pStatus
        Baru = 0
        Edit = 1
    End Enum

    Public pStatus_ As pStatus
    Public NoID As Long, KodeLama As String, IDKaryawan As Long
    Dim IDTipeAsset As Integer = 0
    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()
        If IsAdministrator Then
            LayoutControl1.AllowCustomizationMenu = True
        Else
            LayoutControl1.AllowCustomizationMenu = False
        End If
        ' Add any initialization after the InitializeComponent() call.

    End Sub

    Private Sub frEntriPinjaman_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        LayoutControl1.SaveLayoutToXml(Application.StartupPath & "\system\layouts\" & Me.Name & LayoutControl1.Name & ".xml")
    End Sub

    Private Sub frEntriPinjaman_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim curentcursor As Cursor = Windows.Forms.Cursor.Current
        Windows.Forms.Cursor.Current = Cursors.WaitCursor
        Setme()
        If pStatus_ = pStatus.Edit Then
            TampilData()
        Else
            tglPerolehan.DateTime = Date.Today
            tglPerolehan.DateTime = DateAdd(DateInterval.Day, 7, Date.Today)
            txtKode.Text = Format(Date.Today, "yyyyMM")
        End If
        Windows.Forms.Cursor.Current = curentcursor
    End Sub
    Private Sub TampilData()
        Dim oConn As New SqlConnection
        Dim ocmd As SqlCommand
        Dim oDA As SqlDataAdapter
        Dim oDS As New DataSet
        Dim strsql As String
        'Dim isAda As Boolean
        strsql = "Select MAsset.*, MTypeAsset.Nama as Tipe FROM " & _
        "MAsset Left join MTypeAsset on MAsset.IDTypeAsset=MTypeAsset.NoID " & _
        "WHERE MAsset.NoID=" & NoID
        Try
            oConn.ConnectionString = StrKonSql
            ocmd = New SqlCommand(strsql, oConn)
            oConn.Open()
            oDA = New SqlDataAdapter(ocmd)
            oDA.Fill(oDS, "MAsset")
            If oDS.Tables("MAsset").Rows.Count = 0 Then
                tglPerolehan.DateTime = Date.Today
                TglMulaiSusut.DateTime = Date.Today
                pStatus_ = pStatus.Baru
            Else
                txtKode.Text = NullTostr(oDS.Tables("MAsset").Rows(0).Item("Kode"))
                txtNama.Text = NullTostr(oDS.Tables("MAsset").Rows(0).Item("Nama"))
                txtCatatan.Text = NullTostr(oDS.Tables("MAsset").Rows(0).Item("Catatan"))
                tglPerolehan.DateTime = NullToDate(oDS.Tables("MAsset").Rows(0).Item("Tanggal"))
                TglMulaiSusut.DateTime = NullToDate(oDS.Tables("MAsset").Rows(0).Item("TanggaldiBukukan"))
                TglHapus.EditValue = NullToDate(oDS.Tables("MAsset").Rows(0).Item("TanggalHapus"))

                txtNilaiPerolehan.EditValue = NullToDbl(oDS.Tables("MAsset").Rows(0).Item("HargaPerolehan"))
                txtNilaiMulaiSusut.EditValue = NullToDbl(oDS.Tables("MAsset").Rows(0).Item("NilaiDibukukan"))
                txtNilaiSisa.EditValue = NullToDbl(oDS.Tables("MAsset").Rows(0).Item("NilaiSisa"))
                txtBulanEkonomis.EditValue = NullToDbl(oDS.Tables("MAsset").Rows(0).Item("BulanEkonomis"))

                txtTipe.Text = NullTostr(oDS.Tables("MAsset").Rows(0).Item("Tipe"))
                KodeLama = txtKode.Text
                IDTipeAsset = NullToLong(oDS.Tables("MAsset").Rows(0).Item("IDTypeAsset"))
                chkIsAda.Checked = NullToBool(oDS.Tables("MAsset").Rows(0).Item("IsAda"))
            End If
        Catch ex As Exception
            XtraMessageBox.Show(Me, ex.Message, ".:: Pesan Kesalahan ::.", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            oConn.Close()
        End Try
    End Sub
    Private Sub Setme()
        If Dir(Application.StartupPath & "\system\layouts\" & Me.Name & LayoutControl1.Name & ".xml") <> "" Then
            LayoutControl1.RestoreLayoutFromXml(Application.StartupPath & "\system\layouts\" & Me.Name & LayoutControl1.Name & ".xml")
        End If
        If IsAdministrator Then
            LayoutControl1.AllowCustomizationMenu = True
        Else
            LayoutControl1.AllowCustomizationMenu = False
        End If
    End Sub

    Private Sub cmdCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdCancel.Click
        Me.DialogResult = Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub cmdSimpan_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSimpan.Click
        If IsSimpan() Then
            Me.DialogResult = Windows.Forms.DialogResult.OK
            Me.Close()
        End If
    End Sub
    Function IsSimpan() As Boolean
        Dim oConn As New SqlConnection
        Dim ocmd As SqlCommand
        Dim strsql As String
        Dim curentcursor As Cursor = Windows.Forms.Cursor.Current
        Windows.Forms.Cursor.Current = Cursors.WaitCursor
        If pStatus_ = pStatus.Baru Then
            NoID = GetNewID("Masset", "NoID")
            strsql = "INSERT INTO Masset(NoID,Kode,Nama) " & _
                    "values(" & NoID.ToString & ",'" & FixApostropi(txtKode.Text) & "','" & _
                    FixApostropi(txtNama.Text) & "')"
            oConn.ConnectionString = StrKonSql
            ocmd = New SqlCommand(strsql, oConn)
            oConn.Open()
            ocmd.ExecuteNonQuery()
            ocmd.Dispose()
            oConn.Close()
        End If
        strsql = "UPDATE Masset Set " & _
        " Kode='" & FixApostropi(txtKode.Text) & "'," & _
        " Nama='" & FixApostropi(txtNama.Text) & "'," & _
        " Catatan='" & FixApostropi(txtCatatan.Text) & "'," & _
        " IsAda=" & IIf(chkIsAda.Checked, 1, 0) & "," & _
        " Tanggal='" & Format(tglPerolehan.DateTime, "yyyy/MM/dd") & "', " & _
        " TanggalDibukukan='" & Format(TglMulaiSusut.DateTime, "yyyy/MM/dd") & "', " & _
        " TanggalHapus=" & IIf(NullTostr(TglHapus.Text = ""), "NULL", "'" & Format(TglHapus.DateTime, "yyyy/MM/dd") & "'") & ", " & _
        " Hargaperolehan=" & FixApostropi(txtNilaiPerolehan.EditValue) & "," & _
        " NilaiDibukukan=" & FixApostropi(txtNilaiMulaiSusut.EditValue) & ", " & _
        " NilaiSisa=" & FixApostropi(txtNilaiSisa.EditValue) & ", " & _
        " BulanEkonomis=" & FixApostropi(txtBulanEkonomis.EditValue) & ", " & _
        " IDTypeAsset=" & IDTipeAsset & "  " & _
        " WHERE NoID=" + NoID.ToString
        Try
            If IsValidasi() Then
                oConn.ConnectionString = StrKonSql
                ocmd = New SqlCommand(strsql, oConn)
                oConn.Open()
                ocmd.ExecuteNonQuery()
                EksekusiSQL("Update MAsset set IDABarang=MTypeAsset.IDAAsset,IDAPenyusutan=MTypeAsset.IDASusut,IDAAkumulasiSusut=MTypeAsset.IDAAkumulasiSusut from " & _
 "MAsset inner join MTypeAsset ON MAsset.IDTypeAsset=MTypeAsset.NoID where MAsset.NoID=" & NoID.ToString)

                Return True
            Else
                Return False
            End If
         Catch ex As Exception
            XtraMessageBox.Show(ex.Message, ".:: Pesan Kesalahan ::.", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return False
        Finally
            oConn.Close()
        End Try
        Windows.Forms.Cursor.Current = curentcursor
    End Function
    Private Function IsValidasi() As Boolean
        If txtKode.Text = "" Or txtNama.Text = "" Then '
            XtraMessageBox.Show("Kode dan Nama tidak boleh kosong.", "Admin")
            txtKode.Focus()
            Return False : Exit Function
        End If
        'If IDKaryawan <= 0 Then '
        '    XtraMessageBox.Show("Karyawan masih kosong.", "Admin")
        '    txtKaryawan.Focus()
        '    Return False : Exit Function
        'End If
        'If txtNilaiPerolehan.EditValue <= 0 Then '
        '    XtraMessageBox.Show("Ada field yang masih kosong.", "Admin")
        '    txtNilaiPerolehan.Focus()
        '    Return False : Exit Function
        'End If
        Return True
    End Function

    Private Sub txtKaryawan_ButtonClick(ByVal sender As Object, ByVal e As DevExpress.XtraEditors.Controls.ButtonPressedEventArgs) Handles txtTipe.ButtonClick
        Select Case e.Button.Index
            Case 0
                Dim x As New frmDaftarTypeAsset
                If x.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
                    IDTipeAsset = NullToLong(x.row("NoID"))
                    txtTipe.Text = NullToStr(x.row("Nama"))
                End If
            Case 1
                IDTipeAsset = 0
                txtTipe.Text = ""
        End Select
    End Sub

    Private Sub txtKaryawan_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtTipe.EditValueChanged

    End Sub
End Class