Imports System.Data.SqlClient
Imports DevExpress.XtraGrid.Views.Base
Imports DevExpress.XtraEditors
Imports DevExpress.XtraGrid.Views.Grid
Imports DevExpress.XtraGrid.Views.Grid.ViewInfo
Imports DevExpress.XtraGrid.Menu
Public Class frmDaftarBayarPiutang
    Dim NoID As Long
    Dim IDAlamat As Long
    Dim IDKasBank As Long
    Dim ISALLOWDBLCLICK As Boolean
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
        strsql = "Select MBayarHutang.*,MBank.Kode as KodeKas,MBank.Nama as KasBank,MAlamat.Nama as Customer,MAlamat.Kode as KodeCustomer,Malamat.KontakPerson " & _
        "FROM ((MBayarHutang Inner Join MAlamat ON MBayarHutang.IDAlamat=MAlamat.NoID) " & _
        "LEFT JOIN MBank On MBayarHutang.IDBank=MBank.NoID) WHERE MBayarHutang.IsJual=1 "
        isPerluWhere = False
        If txtAlamat.Text <> "" Then
            If isPerluWhere Then
                strsql = strsql & " WHERE IDAlamat=" & IDAlamat
            Else
                strsql = strsql & " AND IDAlamat=" & IDAlamat
            End If
            isPerluWhere = False
        End If
        If txtKasBank.Text <> "" Then
            If isPerluWhere Then
                strsql = strsql & " WHERE IDAKunKas=" & IDKasBank
            Else
                strsql = strsql & " AND IDAKunKas=" & IDKasBank
            End If
            isPerluWhere = False
        End If
        If tglDari.Enabled = True Then
            If isPerluWhere Then
                strsql = strsql & " WHERE Tanggal>=convert(datetime,'" & Format(tglDari.DateTime, "MM/dd/yyyy") & "',101) and  Tanggal<=convert(datetime,'" & Format(TglSampai.DateTime, "MM/dd/yyyy") & "',101) "
            Else
                strsql = strsql & " AND Tanggal>=convert(datetime,'" & Format(tglDari.DateTime, "MM/dd/yyyy") & "',101) and  Tanggal<=convert(datetime,'" & Format(TglSampai.DateTime, "MM/dd/yyyy") & "',101) "
            End If
            isPerluWhere = False
        End If
        Try
            oConn.ConnectionString = StrKonSql
            ocmd = New SqlCommand(strsql, oConn)
            oConn.Open()
            oDA = New SqlDataAdapter(ocmd)
            oDA.Fill(oDS, "MBayarHutang")
        Catch ex As Exception
            XtraMessageBox.Show(Me, ex.Message, ".:: Pesan Kesalahan ::.", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            oConn.Close()
        End Try
        GridControl1.DataSource = oDS.Tables("MBayarHutang")
        GridView1.OptionsView.ShowFooter = True
        'If IsAdministrator Then
        '    GridView1.OptionsView.ShowFooter = True
        'Else
        '    GridView1.OptionsView.ShowFooter = False
        'End If
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
                x.IsCustomer = True
                If x.ShowDialog = Windows.Forms.DialogResult.OK Then
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
                Dim x As New frLUKasBank
                If x.ShowDialog = Windows.Forms.DialogResult.OK Then
                    IDKasBank = x.NoID
                    txtKasBank.Text = x.Nama '& " - " & x.SubKlas
                End If
                x.Dispose()
            Case 1
                IDKasBank = -1
                txtKasBank.Text = ""
        End Select
        TampilData()
    End Sub

    Private Sub txtKasBank_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtKasBank.EditValueChanged

    End Sub

    Private Sub frmDaftarBayarHutang_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        GridView1.SaveLayoutToXml(Application.StartupPath & "\System\Layouts\DaftarBayarHutang.xml")
    End Sub

    Private Sub frmDaftarBayarHutang_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        ' If IsAdministrator Then
        cmdUnposting.Enabled = True
        cmdPosting.Enabled = True
        cmdDelete.Enabled = True
        'Else
        'cmdUnposting.Enabled = False
        'cmdPosting.Enabled = False
        'cmdDelete.Enabled = False
        'End If
        tglDari.EditValue = Today
        TglSampai.EditValue = Today
        If Dir(Application.StartupPath & "\System\Layouts\DaftarBayarHutang.xml") <> "" Then
            GridView1.RestoreLayoutFromXml(Application.StartupPath & "\System\Layouts\DaftarBayarHutang.xml")
        End If
    End Sub


    Private Sub cmdNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdNew.Click
        Dim x As New frBayarPiutang
        x.IsNew = True
        If x.ShowDialog() = Windows.Forms.DialogResult.OK Then
            NoID = x.NoID
            TampilData()
            GridView1.FocusedRowHandle = GridView1.LocateByValue(0, cNoID, NoID)
        End If

    End Sub

    Private Sub cmdEdit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdEdit.Click
        Try
            Dim view As ColumnView = GridControl1.FocusedView
            Dim row As System.Data.DataRow = view.GetDataRow(GridView1.FocusedRowHandle)
            NoID = NullTolong(row("ID"))
            If NullToBool(row("IsPosted")) = False Then

                Dim x As New frBayarPiutang
                x.IsNew = False
                x.NoID = NoID
                If x.ShowDialog() = Windows.Forms.DialogResult.OK Then
                    NoID = x.NoID
                    TampilData()
                    GridView1.FocusedRowHandle = GridView1.LocateByValue(0, cNoID, NoID)
                End If
            Else
                Dim x As New frBayarPiutang
                x.IsNew = False
                x.NoID = NoID
                x.cmdSave.Enabled = False
                If x.ShowDialog() = Windows.Forms.DialogResult.OK Then
                    NoID = x.NoID
                    TampilData()
                    GridView1.FocusedRowHandle = GridView1.LocateByValue(0, cNoID, NoID)
                End If
                'MsgBox("Nota Sudah Posting tidak boleh diedit! ")
            End If

        Catch ex As Exception
            XtraMessageBox.Show(Me, ex.Message, ".:: Pesan Kesalahan ::.", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub cmdDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdDelete.Click
        Try

            Dim view As ColumnView = GridControl1.FocusedView
            Dim row As System.Data.DataRow = view.GetDataRow(GridView1.FocusedRowHandle)
            NoID = NullTolong(row("NoID"))
            If NullToBool(row("IsPosted")) = False Then
                If MsgBox("Apakah anda mau menghapus Nota no: " & NullTostr(row("Kode")) & " ini?", MsgBoxStyle.Question + MsgBoxStyle.YesNoCancel) = MsgBoxResult.Yes Then
                    If EksekusiSQl("Delete From MBayarHutangD where IDBayarHutang=" & NoID) Then
                        EksekusiSQL("Delete From MBayarHutang where NoID=" & NoID)
                        TampilData()
                    End If
                End If
            End If
        Catch ex As Exception
            XtraMessageBox.Show(Me, ex.Message, ".:: Pesan Kesalahan ::.", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub txtAlamat_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtAlamat.EditValueChanged

    End Sub

    Private Sub cmdPosting_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdPosting.Click
        Try
            Dim view As ColumnView = GridControl1.FocusedView
            Dim I As Integer
            For I = 0 To view.SelectedRowsCount - 1
                Dim row As System.Data.DataRow = view.GetDataRow(view.GetSelectedRows(I))

                NoID = NullTolong(row("NoID"))
                If NullToBool(row("IsPosted")) = False Then
                    EksekusiSQL("UPDATE MBayarHutang SET IsPosted=1 where NoID=" & NoID)
                End If
            Next
            TampilData()
        Catch ex As Exception
            XtraMessageBox.Show(Me, ex.Message, ".:: Pesan Kesalahan ::.", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try

    End Sub

    Private Sub cmdUnposting_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdUnposting.Click
        Try
            Dim view As ColumnView = GridControl1.FocusedView
            Dim I As Integer
            For I = 0 To view.SelectedRowsCount - 1
                Dim row As System.Data.DataRow = view.GetDataRow(view.GetSelectedRows(I))

                NoID = NullTolong(row("NoID"))
                If NullToBool(row("IsPosted")) = True Then
                    EksekusiSQL("UPDATE MBayarHutang SET IsPosted=0 where NoID=" & NoID)
                End If
            Next
            TampilData()

        Catch ex As Exception
            XtraMessageBox.Show(Me, ex.Message, ".:: Pesan Kesalahan ::.", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try

    End Sub

    Private Sub cmdPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdPrint.Click
        GridControl1.ShowPrintPreview()

    End Sub

    Private Sub cmdExport_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdExport.Click
        Try
            Dim x As String
            x = Application.StartupPath & "\Daftar Bayar Piutang.xls"
            GridControl1.ExportToXls(x)
            BukaFile(x)
        Catch ex As Exception
            XtraMessageBox.Show(Me, ex.Message, ".:: Pesan Kesalahan ::.", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub GridControl1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles GridControl1.Click

    End Sub



    Private Sub GridView1_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles GridView1.DoubleClick
        If ISALLOWDBLCLICK Then
            cmdEdit.PerformClick()
        End If
    End Sub

    Private Sub GridView1_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles GridView1.MouseDown
        Dim View As GridView = CType(sender, GridView)
        If View Is Nothing Then Return
        ' obtaining hit info
        Dim hitInfo As GridHitInfo = View.CalcHitInfo(New System.Drawing.Point(e.X, e.Y))
        If (e.Button = Windows.Forms.MouseButtons.Right) And (hitInfo.InRow) And _
          (Not View.IsGroupRow(hitInfo.RowHandle)) Then
            ISALLOWDBLCLICK = True
        Else
            ISALLOWDBLCLICK = False
        End If
    End Sub
End Class