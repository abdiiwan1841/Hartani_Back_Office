Imports System.Data.SqlClient
Imports DevExpress.XtraEditors
Imports DevExpress.XtraGrid.Views.Base

Public Class frBayarPiutang
    Public IsNew As Boolean
    Public NoID As Long
    Private IsJual As Byte = 1 'Bayar Piutang,0 Bayar Hutang
    Private IDAlamat As Long
    Private IDKas As Long
    Private IDAkunHutang As Long
    Private IDAkunDenda As Long
    Private IDAkunDiskon As Long
    Private oldTanggal As DateTime
    Private oldNota As String

    Sub Simpan()
        Dim curentcursor As Cursor = Windows.Forms.Cursor.Current
        Windows.Forms.Cursor.Current = Cursors.WaitCursor

        Dim i As Integer
        Dim IDDetil As Long
        Dim Bayar, Discount, Denda As Double
        Dim IDBeli As Long
        Dim view As ColumnView = GridControl1.FocusedView
        If IsNew Then
            NoID = NullTolong(EksekusiSQLSkalar("Select Max(NoID) as x from MBayarHutang")) + 1
            EksekusiSQL("Insert Into MBayarHutang(NoID,Tanggal,Kode,KodeReff,IDAlamat,Keterangan," & _
            "Jumlah,Retur,Denda,IDAkunKas,IDAkunHutang,IDAkunRetur,IDAkunDenda,IsJual) Values(" & _
            NoID & ",convert(datetime,'" & Format(Tgl.DateTime, "MM/dd/yyyy") & "',101),'" & FixApostropi(txtKode.Text) & "','" & FixApostropi(txtKodeReff.Text) & "'," & IDAlamat & ",'" & FixApostropi(txtCatatan.Text) & "'," & _
            FixKoma(Jumlah.EditValue) & "," & FixKoma(txtDiscount.EditValue) & "," & FixKoma(txtDenda.EditValue) & "," & _
            IDKas & "," & IDAkunHutang & "," & IDAkunDiskon & "," & IDAkunDenda & ",1)")
        Else
            EksekusiSQL("update MBayarHutang " & _
            "SET  IsJual=1,Kode='" & FixApostropi(txtKode.Text) & "', KodeReff='" & FixApostropi(txtKodeReff.Text) & "'," & _
            "IDAlamat=" & IDAlamat & ", Keterangan='" & FixApostropi(txtCatatan.Text) & "', " & _
            "Jumlah=" & FixKoma(Jumlah.EditValue) & ",Retur=" & FixKoma(txtDiscount.EditValue) & ",Denda=" & FixKoma(txtDenda.EditValue) & ",IDAkunKas=" & IDKas & ",IDAkunHutang=" & IDAkunHutang & "," & _
            "IDAkunRetur=" & IDAkunDiskon & ",IDAkunDenda=" & IDAkunDenda & " Where NoID=" & NoID)
            EksekusiSQl("delete from MBayarHutangD where IDbayarHutang=" & NoID)
        End If
        For i = 0 To view.RowCount - 1
            'Dim row As System.Data.DataRow = view.GetDataRow(GridView1.FocusedRowHandle)
            'NoID = NullToLong(row("ID"))
            'Kode = NullTostr(row("Kode"))
            'Nama = NullTostr(row("Nama"))
            'Kontak = NullTostr(row("KontakPerson"))
            IDBeli = view.GetRowCellValue(i, "IDBeli")
            Bayar = view.GetRowCellValue(i, "Bayar")
            Discount = view.GetRowCellValue(i, "Retur")
            Denda = view.GetRowCellValue(i, "Denda")
            IDDetil = NullToLong(EksekusiSQlskalar("Select Max(ID) as x from MBayarHutangD")) + 1
            If Bayar <> 0 Or Discount <> 0 Or Denda <> 0 Then
                EksekusiSQl("Insert Into MBayarHutangD(ID,IDBayarHutang,IDBeli,Bayar,Retur,Denda,IsJual) Values(" & _
                IDDetil & "," & NoID & "," & IDBeli & "," & FixKoma(Bayar) & "," & FixKoma(Discount) & "," & FixKoma(Denda) & "," & IsJual & ")")
            End If
        Next
        Windows.Forms.Cursor.Current = curentcursor

    End Sub

    Private Sub ButtonEdit1_ButtonClick(ByVal sender As Object, ByVal e As DevExpress.XtraEditors.Controls.ButtonPressedEventArgs) Handles txtAlamat.ButtonClick
        Select Case e.Button.Index
            Case 0
                Dim x As New frLUAlamat
                x.IsCustomer = True
                If x.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
                    IDAlamat = x.NoID
                    txtAlamat.Text = x.Nama & " - " & x.Kontak

                End If
                x.Dispose()
            Case 1
                IDAlamat = -1
                txtAlamat.Text = ""
        End Select
        RefreshItem()
    End Sub

    Private Sub ButtonEdit1_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtAlamat.EditValueChanged

    End Sub

    Private Sub txtDiscount_ButtonClick(ByVal sender As Object, ByVal e As DevExpress.XtraEditors.Controls.ButtonPressedEventArgs) Handles txtDiscount.ButtonClick
        Dim x As New frLUKasBank
        If x.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
            IDAkunDiskon = x.NoID
            lbAkunDisc.Text = x.Nama
        End If
        x.Dispose()
    End Sub

    Private Sub ButtonEdit6_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtDiscount.EditValueChanged
        HitungJumlahBayar()
    End Sub

    Private Sub txtKas_ButtonClick(ByVal sender As Object, ByVal e As DevExpress.XtraEditors.Controls.ButtonPressedEventArgs) Handles txtKas.ButtonClick
        Select Case e.Button.Index
            Case 0
                Dim x As New frLUKasBank
                If x.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
                    IDKas = x.NoID
                    txtKas.Text = x.Nama
                End If
                x.Dispose()
            Case 1
                IDKas = -1
                txtKas.Text = ""
        End Select
    End Sub

    Sub RefreshItem()
        EksekusiSQl("Delete from TBayarHutangD Where IsJual=1 and IDUser=" & IDUserAktif)
        If IDAlamat <> -1 Then
            If IsNew Then
                EksekusiSQL("insert Into TBayarHutangD(IsJual,IDBeli,IDUser,Tanggal,JatuhTempo,Kode,KodeReff," & _
                "Jumlah,Terbayar,Sisa,Bayar,Retur,Denda) " & _
                "Select " & IsJual & " as IsJual, MJual.NoID," & IDUserAktif & " As IDUser,Tanggal,JatuhTempo,Kode,KodeReff," & _
                "Total,case when B.Bayar IS NULL then 0 else B.Bayar END,Total-(case when B.Bayar IS NULL then 0 else B.Bayar END) as Sisa,0,0,0 " & _
                "FROM (MJual Left Join (Select IDBeli,Sum(Bayar+Retur) as Bayar FROM MBayarHutangD where IsJual=1 group by IDBeli) B on Mjual.NoID=B.IDBeli) " & _
                "WHERE ISNULL(MJual.isPos,0)=0 and MJual.isPosted=1 and  Total-(case when B.Bayar IS NULL then 0 else B.Bayar END)<>0 and MJual.IDCustomer=" & IDAlamat)
            Else
                EksekusiSQL("insert Into TBayarHutangD(Isjual,IDBeli,IDUser,Tanggal,JatuhTempo,Kode,KodeReff," & _
                       "Jumlah,Terbayar,Sisa,Bayar,Retur,Denda) " & _
                       "Select " & IsJual & " as IsJual, MJual.NoID," & IDUserAktif & " As IDUser,Tanggal,JatuhTempo,Kode,KodeReff," & _
                       "Total,case when B.Bayar IS NULL then 0 else B.Bayar END,Total-(case when B.Bayar IS NULL then 0 else B.Bayar END) as Sisa,case when C.Bayar IS NULL then 0 else C.Bayar END,Case When C.Retur IS NULL then 0 else C.Retur END,case when C.Denda IS NULL then 0 else C.Denda END " & _
                       "FROM (MJual Left Join (Select IDBeli,Sum(Bayar+Retur) as Bayar FROM MBayarHutangD where IsJual=1 and MBayarHutangD.IDBayarHutang<>" & NoID & " group by IDBeli) B on MJual.NoID=B.IDBeli) " & _
                       "LEFT JOIN (select MBayarHutangD.IDBeli,MBayarHutangD.Bayar,MBayarHutangd.Retur,MBayarHutangD.Denda from MBayarHutangD where MBayarHutangD.IDBayarHutang=" & NoID & ") C on MJual.NoID=C.IDBeli " & _
                       "WHERE ISNULL(MJual.isPos,0)=0 and  MJual.isPosted=1 and  Total-(case when B.Bayar IS NULL then 0 else B.Bayar END)<>0 and MJual.IDCustomer=" & IDAlamat)

            End If
        End If

        Dim oConn As New SqlConnection
        Dim ocmd As SqlCommand
        Dim oDA As SqlDataAdapter
        Dim oDS As New DataSet
        Dim strsql As String
        'Dim isAda As Boolean
        Dim curentcursor As Cursor = Windows.Forms.Cursor.Current
        Windows.Forms.Cursor.Current = Cursors.WaitCursor
        strsql = "Select * From TBayarhutangD where IsJual=" & IsJual & " and  IDUser=" & IDUserAktif
        Try
            oConn.ConnectionString = StrKonSql
            ocmd = New SqlCommand(strsql, oConn)
            oConn.Open()
            oDA = New SqlDataAdapter(ocmd)
            oDA.Fill(oDS, "TBayarhutangD")
        Catch ex As Exception
            XtraMessageBox.Show(Me, ex.Message, ".:: Pesan Kesalahan ::.", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            oConn.Close()
        End Try
        GridControl1.DataSource = oDS.Tables("TBayarhutangD")
        Windows.Forms.Cursor.Current = curentcursor
        Application.DoEvents()
    End Sub

    Private Sub frBayarHutang_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        GridView1.SaveLayoutToXml(Application.StartupPath & "\System\Layouts\EntriBayarPiutangNF.xml")
    End Sub
    Dim DefImageList As New ImageList
    Private Sub SetTombol()
        DefImageList = frmMain.ImageList1

        cmdRefresh.ImageList = DefImageList
        cmdRefresh.ImageIndex = 5

        cmdSave.ImageList = DefImageList
        cmdSave.ImageIndex = 6

        cmdTutup.ImageList = DefImageList
        cmdTutup.ImageIndex = 3

    End Sub
    Private Sub frBayarHutang_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        SetTombol()
        TampilData()
        If Dir(Application.StartupPath & "\System\Layouts\EntriBayarPiutangNF.xml") <> "" Then
            GridView1.RestoreLayoutFromXml(Application.StartupPath & "\System\Layouts\EntriBayarPiutangNF.xml")
        End If
    End Sub

    Sub IsiDefault()
        Tgl.EditValue = Today
        txtDiscount.EditValue = 0 'NullToDbl(oDS.Tables("MBayarHutang").Rows(0).Item("Retur"))
        txtDenda.EditValue = 0 'NullToDbl(oDS.Tables("MBayarHutang").Rows(0).Item("Denda"))
        Jumlah.EditValue = 0 'NullToDbl(oDS.Tables("MBayarHutang").Rows(0).Item("Jumlah"))
    End Sub

    Sub NotaBaru()
        If IsNew Then
            txtKode.Text = GetKode("MBayarHutang", "AR", Tgl.DateTime.Month, Tgl.DateTime.Year)
        Else
            If oldTanggal.Year = Tgl.DateTime.Year AndAlso oldTanggal.Month = Tgl.DateTime.Month Then
                txtKode.Text = oldNota
            Else
                txtKode.Text = GetKode("MBayarHutang", "AR", Tgl.DateTime.Month, Tgl.DateTime.Year)
            End If
        End If
    End Sub

    Sub TampilData()
        If IsNew Then
            NotaBaru()
            IsiDefault()
        Else
            Dim oConn As New SqlConnection
            Dim ocmd As SqlCommand
            Dim oDA As SqlDataAdapter
            Dim oDS As New DataSet
            Dim strsql As String
            'Dim isAda As Boolean
            Dim curentcursor As Windows.Forms.Cursor = Windows.Forms.Cursor.Current
            Windows.Forms.Cursor.Current = Windows.Forms.Cursors.WaitCursor
            strsql = "Select MBayarHutang.*,Malamat.Nama as Alamat,MAkun.Kode as KodeKas,MAkun.Nama as AkunKas " & _
            "FROM ((MBayarHutang LEFT JOIN MAlamat ON MBayarHutang.IDAlamat=MAlamat.NoID) " & _
            "LEFT JOIN MAkun on MBayarHutang.IDAkunKas=MAkun.ID) " & _
            "Where MBayarHutang.NoID=" & NoID.ToString
            Try
                oConn.ConnectionString = StrKonSql
                ocmd = New SqlCommand(strsql, oConn)
                oConn.Open()
                oDA = New SqlDataAdapter(ocmd)
                oDA.Fill(oDS, "MBayarHutang")
                If oDS.Tables("MBayarHutang").Rows.Count = 0 Then
                    IsiDefault()
                    IsNew = True
                Else
                    IDAkunDenda = NullToLong(oDS.Tables("MBayarHutang").Rows(0).Item("IDAkunDenda"))
                    IDAkunDiskon = NullToLong(oDS.Tables("MBayarHutang").Rows(0).Item("IDAkunRetur"))
                    IDKas = NullToLong(oDS.Tables("MBayarHutang").Rows(0).Item("IDAkunKas"))
                    IDAkunHutang = NullToLong(oDS.Tables("MBayarHutang").Rows(0).Item("IDAkunHutang"))
                    IDAlamat = NullToLong(oDS.Tables("MBayarHutang").Rows(0).Item("IDAlamat"))

                    txtAlamat.Text = NullTostr(oDS.Tables("MBayarHutang").Rows(0).Item("Alamat"))
                    txtKode.Text = NullTostr(oDS.Tables("MBayarHutang").Rows(0).Item("Kode"))
                    txtKodeReff.Text = NullTostr(oDS.Tables("MBayarHutang").Rows(0).Item("KodeReff"))
                    txtCatatan.Text = NullTostr(oDS.Tables("MBayarHutang").Rows(0).Item("Keterangan"))
                    txtKas.Text = NullTostr(oDS.Tables("MBayarHutang").Rows(0).Item("KodeKas")) & " - " & NullTostr(oDS.Tables("MBayarHutang").Rows(0).Item("AkunKas"))
                    Tgl.EditValue = oDS.Tables("MBayarHutang").Rows(0).Item("Tanggal")
                    oldTanggal = oDS.Tables("MBayarHutang").Rows(0).Item("Tanggal")
                    oldNota = NullTostr(oDS.Tables("MBayarHutang").Rows(0).Item("Kode"))

                    txtDiscount.EditValue = NullToDbl(oDS.Tables("MBayarHutang").Rows(0).Item("Retur"))
                    txtDenda.EditValue = NullToDbl(oDS.Tables("MBayarHutang").Rows(0).Item("Denda"))
                    Jumlah.EditValue = NullToDbl(oDS.Tables("MBayarHutang").Rows(0).Item("Jumlah"))
                End If
                RefreshItem()
            Catch ex As Exception
                XtraMessageBox.Show(Me, ex.Message, ".:: Pesan Kesalahan ::.", Windows.Forms.MessageBoxButtons.OK + Windows.Forms.MessageBoxButtons.OK, Windows.Forms.MessageBoxIcon.Error)
            Finally
                oConn.Close()
            End Try

            Windows.Forms.Cursor.Current = curentcursor
        End If
    End Sub

    Private Sub Jumlah_ButtonClick(ByVal sender As Object, ByVal e As DevExpress.XtraEditors.Controls.ButtonPressedEventArgs) Handles Jumlah.ButtonClick
        Select Case e.Button.Index
            Case 0
                GenerateJumlah()
            Case 1
                Dim x As New frLUKasBank
                If x.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
                    IDAkunHutang = x.NoID
                    lbAkunHutang.Text = x.Nama
                End If
                x.Dispose()
        End Select
    End Sub

    Sub GenerateJumlah()
        If MsgBox("Apakah anda yakin mau generate pembayaran per nota?", MsgBoxStyle.Question + MsgBoxStyle.YesNoCancel) = MsgBoxResult.Yes Then
            Dim i As Integer = 0
            Dim Sisa As Double
            Dim DiscountRp As Double
            Dim DiscountProsen As Double
            Dim view As ColumnView = GridControl1.FocusedView
            Sisa = Jumlah.EditValue
            DiscountRp = txtDiscount.EditValue
            If Sisa = 0 Then DiscountProsen = 100 Else DiscountProsen = DiscountRp * 100 / (Sisa + DiscountRp)

            While i < view.RowCount 'Sisa > 0 And
                If view.GetRowCellValue(i, "Sisa") > (Sisa + DiscountRp) Then
                    view.SetRowCellValue(i, "Bayar", Sisa)
                    view.SetRowCellValue(i, "Retur", DiscountRp)
                    Sisa = 0
                    DiscountRp = 0
                Else
                    view.SetRowCellValue(i, "Retur", view.GetRowCellValue(i, "Sisa") * DiscountProsen / 100)
                    view.SetRowCellValue(i, "Bayar", view.GetRowCellValue(i, "Sisa") - view.GetRowCellValue(i, "Sisa") * DiscountProsen / 100)

                    Sisa = Sisa - view.GetRowCellValue(i, "Bayar")
                    DiscountRp = DiscountRp - view.GetRowCellValue(i, "Retur")
                End If
                i = i + 1
            End While
        End If
    End Sub

    Private Sub txtKas_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtKas.EditValueChanged

    End Sub

    Private Sub Jumlah_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Jumlah.EditValueChanged
        HitungJumlahBayar()
    End Sub

    Private Sub txtDenda_ButtonClick(ByVal sender As Object, ByVal e As DevExpress.XtraEditors.Controls.ButtonPressedEventArgs) Handles txtDenda.ButtonClick
        Dim x As New frLUKasBank
        If x.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
            IDAkunDenda = x.NoID
            lbAkunDenda.Text = x.Nama
        End If
        x.Dispose()
    End Sub

    Function GetKode(ByVal NmTable As String, ByVal KodeNota As String, ByVal Bulan As Integer, ByVal Tahun As Integer) As String
        Dim curentcursor As Cursor = Windows.Forms.Cursor.Current
        Windows.Forms.Cursor.Current = Cursors.WaitCursor
        Dim Id As Long
        Id = NullTolong(EksekusiSQLSkalar("Select count(NoID) as x from " & NmTable & " where month(tanggal)=" & Bulan & " and year(tanggal)=" & Tahun)) + 1
        Windows.Forms.Cursor.Current = curentcursor
        Return KodeNota & Format(Id, "0000") & "/" & Format(Bulan, "00") & "/" & Format(Tahun, "0000")

    End Function

    Private Sub Tgl_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Tgl.EditValueChanged
        NotaBaru()
    End Sub

    Private Sub cmdTutup_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdTutup.Click
        DialogResult = Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub cmdSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSave.Click
        Simpan()
        DialogResult = Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub

    Private Sub cmdRefresh_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdRefresh.Click
        If XtraMessageBox.Show("Refresh akan mereset detil yang sudah dientri." & vbCrLf & "Yakin ingin meneruskan?", NamaAplikasi, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) = Windows.Forms.DialogResult.Yes Then
            RefreshItem()
        End If
    End Sub

    Private Sub BarButtonItem1_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem1.ItemClick
        cmdSave.PerformClick()
    End Sub

    Private Sub BarButtonItem6_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem6.ItemClick
        cmdRefresh.PerformClick()
    End Sub

    Private Sub BarButtonItem7_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem7.ItemClick
        cmdTutup.PerformClick()
    End Sub

    Private Sub BarButtonItem3_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem3.ItemClick

    End Sub

    Private Sub GridControl1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles GridControl1.Click

    End Sub

    'Private Sub GridView1_CellValueChanged(ByVal sender As Object, ByVal e As DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs) Handles GridView1.CellValueChanged
    '    Try
    '        If GridView1.FocusedColumn.FieldName.ToLower = "Keterangan".ToLower Then
    '            If GridView1.GetRowCellValue(GridView1.FocusedRowHandle, "Keterangan").ToString.Length <= 100 Then
    '                EksekusiSQL("UPDATE MTransferOutD SET Keterangan='" & FixApostropi(GridView1.GetRowCellValue(GridView1.FocusedRowHandle, "Keterangan").ToString) & "' WHERE NoID=" & NullTolong(GridView1.GetRowCellValue(GridView1.FocusedRowHandle, "NoID")))
    '            Else
    '                XtraMessageBox.Show("Keterangan terlalu panjang.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
    '            End If
    '        End If
    '    Catch ex As Exception
    '        XtraMessageBox.Show("Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
    '    End Try
    'End Sub

    Private Sub txtDenda_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtDenda.EditValueChanged
        HitungJumlahBayar()
    End Sub
    Sub HitungJumlahBayar()
        txtJumlahBayar.EditValue = NullToDbl(Jumlah.EditValue) - NullToDbl(txtDiscount.EditValue) + NullToDbl(txtDenda.EditValue) '- NullToDbl(txtMaterai.EditValue)
    End Sub

    Private Sub GridView1_CellValueChanged(ByVal sender As Object, ByVal e As DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs) Handles GridView1.CellValueChanged
        Jumlah.EditValue = GridView1.Columns("Bayar").SummaryItem.SummaryValue
    End Sub

    Private Sub GridView1_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles GridView1.DoubleClick
        Dim row As System.Data.DataRow = GridView1.GetDataRow(GridView1.FocusedRowHandle)
        row("bayar") = NullToDbl(row("Sisa"))
        Application.DoEvents()
        Jumlah.EditValue = GridView1.Columns("Bayar").SummaryItem.SummaryValue
    End Sub
End Class