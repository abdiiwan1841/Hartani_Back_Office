Imports System.Data.SqlClient
Imports DevExpress.XtraGrid.Views.Base
Imports DevExpress.XtraEditors
Public Class frmDaftarPiutangPerCustomer
    Dim NoID As Long
    Dim IDAlamat As Long
    Dim IDKasBank As Long
    Dim IDOPERATOR As Long
    Dim IDBarang As Long
    Private Sub SimpleButton5_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdClose.Click
        Me.Close()
    End Sub

    Private Sub SimpleButton9_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdRefresh.Click
        TampilData()
    End Sub
    Sub TampilData()
        Dim oConn As New SqlConnection
        Dim ocmd As SqlCommand
        Dim oDA As SqlDataAdapter
        Dim oDS As New DataSet
        Dim strsql As String
        Dim isPerluWhere As Boolean
        'Dim isAda As Boolean
        Dim curentcursor As Cursor = Windows.Forms.Cursor.Current
        Windows.Forms.Cursor.Current = Cursors.WaitCursor
        strsql = "Select  datediff(Day,MJual.Tanggal,getdate()) as Umur,MAlamat.Nama as Customer,MAlamat.KontakPerson, MJual.*,case when X.JumlahBayar IS NULL then 0 else X.JumlahBayar END as JumlahBayar,case when X.JumlahDiskon IS NULL then 0 else X.JumlahDiskon END as JumlahDiskon, " & _
        "MJUal.Total- (case when X.JumlahBayar IS NULL then 0 else X.JumlahBayar END + case when X.JumlahDiskon IS NULL then 0 else X.JumlahDiskon END ) as SisaAkhir " & _
        "FROM MJual Left Join (Select MBayarHutangD.IDBeli,Sum(MBayarHutangD.Bayar) as JumlahBayar, Sum(MBayarHutangD.Retur) as JumlahDiskon From " & _
        " MBayarHutangD  WHERE IsJual=1 Group By IDBeli) X " & _
        "ON (MJUal.IsPos Is NULL OR Mjual.IsPos=0) AND MJUal.NoID=X.IDBeli " & _
        "LEFT JOIN MAlamat ON MJual.IDCustomer=Malamat.NoID "
        If txtAlamat.Text <> "" Then
            If isPerluWhere Then
                strsql = strsql & " WHERE MJual.IDCustomer=" & IDAlamat
            Else
                strsql = strsql & " AND MJual.IDCustomer=" & IDAlamat
            End If
            isPerluWhere = False
        End If
        If chkLunas.Checked Then
            If isPerluWhere Then
                strsql = strsql & " WHERE (MJUal.Total- (case when X.JumlahBayar IS NULL then 0 else X.JumlahBayar END + case when X.JumlahDiskon IS NULL then 0 else X.JumlahDiskon END ) >0 )"
            Else
                strsql = strsql & " AND  (MJUal.Total- (case when X.JumlahBayar IS NULL then 0 else X.JumlahBayar END + case when X.JumlahDiskon IS NULL then 0 else X.JumlahDiskon END ) >0 )"
            End If
            isPerluWhere = False
        End If
        'If txtKasBank.Text <> "" Then
        '    If isPerluWhere Then
        '        strsql = strsql & " WHERE IDAKunKas=" & IDKasBank
        '    Else
        '        strsql = strsql & " AND IDAKunKas=" & IDKasBank
        '    End If
        '    isPerluWhere = False
        'End If
        If tglDari.Enabled = True Then
            If isPerluWhere Then
                strsql = strsql & " WHERE MJual.Tanggal>=convert(datetime,'" & Format(tglDari.DateTime, "MM/dd/yyyy") & "',101) and  MJual.Tanggal<=convert(datetime,'" & Format(TglSampai.DateTime, "MM/dd/yyyy") & "',101) "
            Else
                strsql = strsql & " AND MJual.Tanggal>=convert(datetime,'" & Format(tglDari.DateTime, "MM/dd/yyyy") & "',101) and  MJual.Tanggal<=convert(datetime,'" & Format(TglSampai.DateTime, "MM/dd/yyyy") & "',101) "
            End If
            isPerluWhere = False
        End If
        strsql = "select Customer,KontakPerson, Sum(case when Umur<30 then SisaAkhir else 0 end) as BelumJT,Sum(case when Umur>=30 then SisaAkhir else 0 end) as SudahJT, Sum(SisaAkhir) as Total " & _
                    "from (" & strsql & ") Z group by Customer,KontakPerson"
        isPerluWhere = True

        Try
            oConn.ConnectionString = StrKonSql
            ocmd = New SqlCommand(strsql, oConn)
            oConn.Open()
            oDA = New SqlDataAdapter(ocmd)
            oDA.Fill(oDS, "MJual")
        Catch ex As Exception
            XtraMessageBox.Show(Me, ex.Message, ".:: Pesan Kesalahan ::.", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            oConn.Close()
        End Try
        GridControl1.DataSource = oDS.Tables("MJual")
        Windows.Forms.Cursor.Current = curentcursor
        Application.DoEvents()
        'GridView1.FocusedRowHandle = GridView1.LocateByValue(0, colNoID, NoID)
    End Sub

    Private Sub DateEdit2_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TglSampai.EditValueChanged

    End Sub

    Private Sub LabelControl3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LabelControl3.Click
        tglDari.Enabled = Not tglDari.Enabled
        TglSampai.Enabled = tglDari.Enabled
    End Sub

    Private Sub ButtonEdit1_ButtonClick(ByVal sender As Object, ByVal e As DevExpress.XtraEditors.Controls.ButtonPressedEventArgs) Handles txtAlamat.ButtonClick
        Select Case e.Button.Index
            Case 0
                Dim x As New frLUAlamat
                If x.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
                    IDAlamat = x.NoID
                    txtAlamat.Text = x.Nama & " - " & x.Kontak
                End If
                x.Dispose()
            Case 1
                IDAlamat = -1
                txtAlamat.Text = ""
        End Select
        TampilData()
    End Sub

    Private Sub txtKasBank_ButtonClick(ByVal sender As Object, ByVal e As DevExpress.XtraEditors.Controls.ButtonPressedEventArgs) Handles txtKasBank.ButtonClick
        Select Case e.Button.Index
            Case 0
                Dim x As New frLUAlamat
                If x.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
                    IDOPERATOR = x.NoID
                    txtKasBank.Text = x.Nama
                End If
                x.Dispose()
            Case 1
                IDOPERATOR = -1
                txtKasBank.Text = ""
        End Select
        TampilData()
    End Sub

    Private Sub txtKasBank_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtKasBank.EditValueChanged

    End Sub

    Private Sub frmDaftarBayarHutang_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        GridView1.SaveLayoutToXml(Application.StartupPath & "\system\Layouts\DaftarPiutangPerCustomer.xml")

    End Sub

    Private Sub frmDaftarBayarHutang_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
         tglDari.EditValue = Today
        TglSampai.EditValue = Today
        If Dir(Application.StartupPath & "\system\Layouts\DaftarPiutangPerCustomer.xml") <> "" Then
            GridView1.RestoreLayoutFromXml(Application.StartupPath & "\system\Layouts\DaftarPiutangPerCustomer.xml")
        End If
    End Sub


    Private Sub cmdPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdPrint.Click
        GridControl1.ShowPrintPreview()

    End Sub

    Private Sub cmdExport_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdExport.Click
        Try
            Dim x As String
            x = Application.StartupPath & "\Daftar Piutang Per Customer.xls"
            GridControl1.ExportToXls(x)
            BukaFile(x)
        Catch ex As Exception
            XtraMessageBox.Show(Me, ex.Message, ".:: Pesan Kesalahan ::.", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub txtAlamat_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtAlamat.EditValueChanged

    End Sub
End Class