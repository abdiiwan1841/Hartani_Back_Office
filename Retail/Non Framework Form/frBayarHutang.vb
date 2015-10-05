Imports System.Data.SqlClient
Imports DevExpress.XtraEditors
Imports DevExpress.XtraGrid.Views.Base
Imports DevExpress.XtraGrid.Columns
Imports DevExpress.XtraGrid.Views.Grid
Imports DevExpress.XtraGrid.Views.Grid.ViewInfo
Imports DevExpress.XtraGrid.Menu
Imports DevExpress.Utils.Menu

Public Class frBayarHutang
    Public IsNew As Boolean
    Public NoID As Long
    Private IsJual As Byte = 0 '1 Bayar Piutang, 0 Bayar Hutang
    Private IDAlamat As Long
    Private IDKas As Long
    Private IDAkunHutang As Long
    Private IDAkunDenda As Long
    Private IDAkunDiskon As Long
    Private oldTanggal As DateTime
    Private oldNota As String

    Function Simpan() As Boolean

        Dim hasil As Boolean = True
        Dim curentcursor As Cursor = Windows.Forms.Cursor.Current
        Try
            Windows.Forms.Cursor.Current = Cursors.WaitCursor

            Dim i As Integer
            Dim Pending As Boolean
            Dim IDDetil As Long
            Dim Bayar, Discount, Denda As Double
            Dim IDBeli As Long
            Dim view As ColumnView = GridControl1.FocusedView
            If IsNew Then
                NoID = NullToLong(EksekusiSQLSkalar("Select Max(NoID) as x from MBayarHutang")) + 1
                EksekusiSQL("Insert Into MBayarHutang(NoID,IDTT,Tanggal,Kode,KodeReff,IDAlamat,Keterangan," & _
                "Jumlah,Retur,Denda,Materai,IDAkunKas,IDAkunHutang,IDAkunRetur,IDAkunDenda,IsJual) Values(" & _
                NoID & "," & NullToLong(txtTT.EditValue) & ",convert(datetime,'" & Format(Tgl.DateTime, "MM/dd/yyyy") & "',101),'" & FixApostropi(txtKode.Text) & "','" & FixApostropi(txtKodeReff.Text) & "'," & IDAlamat & ",'" & FixApostropi(txtCatatan.Text) & "'," & _
                FixKoma(Jumlah.EditValue) & "," & FixKoma(txtDiscount.EditValue) & "," & FixKoma(txtDenda.EditValue) & "," & _
                FixKoma(txtMaterai.EditValue) & "," & IDKas & "," & IDAkunHutang & "," & IDAkunDiskon & "," & IDAkunDenda & ",0)")
            Else
                EksekusiSQL("update MBayarHutang " & _
                "SET  Kode='" & FixApostropi(txtKode.Text) & "', KodeReff='" & FixApostropi(txtKodeReff.Text) & "'," & _
                 "IDTT=" & NullToLong(txtTT.EditValue) & ",IDAlamat=" & IDAlamat & ", Keterangan='" & FixApostropi(txtCatatan.Text) & "', " & _
                "Jumlah=" & FixKoma(Jumlah.EditValue) & ",Retur=" & FixKoma(txtDiscount.EditValue) & ",Denda=" & FixKoma(txtDenda.EditValue) & "," & _
                "Materai=" & FixKoma(txtMaterai.EditValue) & ",IDAkunKas=" & IDKas & ",IDAkunHutang=" & IDAkunHutang & "," & _
                "IDAkunRetur=" & IDAkunDiskon & ",IDAkunDenda=" & IDAkunDenda & ",IsJual=0 Where NoID=" & NoID)
                EksekusiSQL("delete from MBayarHutangD where IDbayarHutang=" & NoID)
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
                Pending = NullToBool(view.GetRowCellValue(i, "Pending"))
                IDDetil = NullToLong(EksekusiSQLSkalar("Select Max(ID) as x from MBayarHutangD")) + 1
                If Bayar <> 0 Or Discount <> 0 Or Denda <> 0 Then
                    EksekusiSQL("Insert Into MBayarHutangD(ID,IDBayarHutang,IDBeli,Bayar,Retur,Denda,IsJual) Values(" & _
                    IDDetil & "," & NoID & "," & IDBeli & "," & FixKoma(Bayar) & "," & FixKoma(Discount) & "," & FixKoma(Denda) & "," & IsJual & ")")
                End If
                EksekusiSQL("update MBeli set Pending=" & IIf(Pending, 1, 0) & " From mbeli where NoID=" & IDBeli)
            Next
            'Posting
            EksekusiSQL("UPDATE MBayarHutang SET IsPosted=1 where NoID=" & NoID)

        Catch ex As Exception
            hasil = False
        End Try
        Windows.Forms.Cursor.Current = curentcursor
        Return hasil
    End Function
    Sub HitungJumlahBayar()
        txtJumlahBayar.EditValue = NullToDbl(Jumlah.EditValue) - NullToDbl(txtDiscount.EditValue) + NullToDbl(txtDenda.EditValue) - NullToDbl(txtMaterai.EditValue)
    End Sub
    Private Sub ButtonEdit1_ButtonClick(ByVal sender As Object, ByVal e As DevExpress.XtraEditors.Controls.ButtonPressedEventArgs) Handles txtAlamat.ButtonClick
        Select Case e.Button.Index
            Case 0
                Dim x As New frLUAlamat
                x.IsSupplier = True
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
        RefreshTT()
    End Sub
    Sub RefreshTT()
        Dim strsql As String
        Dim ds As New DataSet
        Try
            strsql = "SELECT MTT.NoID, MTT.Kode AS NoTT, MTT.Tanggal " & vbCrLf & _
                  " FROM  MTT WHERE IDCustomer=" & IDAlamat & " and NoID Not In (select distinct ISNULL(IDTT,-1) from MBayarHutang where MBayarHutang.IDAlamat=" & IDAlamat & " and MBayarHutang.NoID<>" & NoID & ")"
            ds = ExecuteDataset("MTT", strsql)
            txtTT.Properties.DataSource = ds.Tables("MTT")
            txtTT.Properties.ValueMember = "NoID"
            txtTT.Properties.DisplayMember = "NoTT"
        Catch ex As Exception
            XtraMessageBox.Show("Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            If Not ds Is Nothing Then
                ds.Dispose()
            End If
        End Try
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
        EksekusiSQL("Delete from TBayarHutangD Where IsJual=0 and IDUser=" & IDUserAktif)
        If IDAlamat <> -1 Then
            If IsNew Then
                'Pembelian
                EksekusiSQL("insert Into TBayarHutangD(IsJual,Pending,IDBeli,IDUser,Tanggal,JatuhTempo,Kode,KodeReff," & _
                "Jumlah,Terbayar,PotongRetur,Sisa,Bayar,Retur,Denda) " & _
                "Select " & IsJual & " as IsJual, MBeli.Pending,MBeli.NoID," & IDUserAktif & " As IDUser,Tanggal,JatuhTempo,Kode,IsNull((SELECT TOP 1 MPO.Kode FROM MPOD INNER JOIN MPO ON MPO.NoID=MPOD.IDPO INNER JOIN MBeliD ON MBeliD.IDPOD=MPOD.NoID WHERE MBeliD.IDBeli=MBeli.NoID GROUP BY MPO.Kode),'') AS KodeReff," & _
                "Total,ISNULL(B.Bayar,0),IsNull(RB.NilaiRetur,0) as Terbayar,Total-isnull( B.Bayar,0)-isnull(RB.NilaiRetur,0) as Sisa,0,0,0 " & _
                "FROM (MBeli Left Join (Select IDBeli,Sum(Bayar+Retur) as Bayar FROM MBayarHutangD where IsJual=0 group by IDBeli) B on MBeli.NoID=B.IDBeli) " & _
                "LEFT JOIN (Select IDBeli,Sum(MreturBeli.Total)  as NilaiRetur FROM MReturBeli where MReturBeli.Isposted=1 group by IDBeli) RB on MBeli.NoID=RB.IDBeli " & _
                "WHERE MBeli.isPosted=1 and  Total-ISNULL(B.Bayar,0)-ISNull(RB.NilaiRetur,0)<>0 and MBeli.IDSupplier=" & IDAlamat)

            Else
                'C=Nota yang Diedit
                'B=Beli
                'RB Retur Beli
                EksekusiSQL("insert Into TBayarHutangD(Isjual,Pending,IDBeli,IDUser,Tanggal,JatuhTempo,Kode,KodeReff," & _
                       "Jumlah,Terbayar,PotongRetur,Sisa,Bayar,Retur,Denda) " & _
                       "Select " & IsJual & " as IsJual,MBeli.Pending, MBeli.NoID," & IDUserAktif & " As IDUser,Tanggal,JatuhTempo,Kode,KodeReff," & _
                       "Total,IsNull(B.Bayar,0),IsNull(RB.NilaiRetur,0),Total-IsNull( B.Bayar,0)-IsNull(RB.NilaiRetur,0) as Sisa,IsNull( C.Bayar,0),Isnull(C.Retur,0),Isnull(C.Denda,0) " & _
                       "FROM (MBeli Left Join (Select IDBeli,Sum(Bayar+Retur) as Bayar FROM MBayarHutangD where IsJual=0 and MBayarHutangD.IDBayarHutang<>" & NoID & " group by IDBeli) B on MBeli.NoID=B.IDBeli) " & _
                       "LEFT JOIN (select MBayarHutangD.IDBeli,MBayarHutangD.Bayar,MBayarHutangd.Retur,MBayarHutangD.Denda from MBayarHutangD where MBayarHutangD.IDBayarHutang=" & NoID & ") C on MBeli.NoID=C.IDBeli " & _
                       "LEFT JOIN (Select IDBeli,Sum(MreturBeli.Total)  as NilaiRetur FROM MReturBeli where MReturBeli.Isposted=1 group by IDBeli) RB on MBeli.NoID=RB.IDBeli " & _
                       "WHERE MBeli.isPosted=1 and  Total-Isnull(B.Bayar,0)-IsNull(RB.NilaiRetur,0)<>0 and MBeli.IDSupplier=" & IDAlamat)

            End If
        End If

        TampilDetil()


        Application.DoEvents()
    End Sub
    Sub TampilDetil()
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
    End Sub

    Private Sub frBayarHutang_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        GridView1.SaveLayoutToXml(FolderLayouts & "EntriBayarHutangNF.xml")
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
        If Dir(FolderLayouts & "EntriBayarHutangNF.xml") <> "" Then
            GridView1.RestoreLayoutFromXml(FolderLayouts & "EntriBayarHutangNF.xml")
        End If
        If cmdSave.Enabled = False Then
            GridView1.Columns("Bayar").FilterInfo = _
              New ColumnFilterInfo("[Bayar] <> 0")
        Else
            GridView1.Columns("Bayar").ClearFilter()
        End If
    End Sub

    Sub IsiDefault()
        Tgl.EditValue = Today
        txtDiscount.EditValue = 0 'NullToDbl(oDS.Tables("MBayarHutang").Rows(0).Item("Retur"))
        txtDenda.EditValue = 0 'NullToDbl(oDS.Tables("MBayarHutang").Rows(0).Item("Denda"))
        Jumlah.EditValue = 0 'NullToDbl(oDS.Tables("MBayarHutang").Rows(0).Item("Jumlah"))
        txtMaterai.EditValue = 0
    End Sub

    Sub NotaBaru()
        If IsNew Then
            txtKode.Text = GetKode("MBayarHutang", "AP", Tgl.DateTime.Month, Tgl.DateTime.Year, "IsJual=0")
        Else
            If oldTanggal.Year = Tgl.DateTime.Year AndAlso oldTanggal.Month = Tgl.DateTime.Month Then
                txtKode.Text = oldNota
            Else
                txtKode.Text = GetKode("MBayarHutang", "AP", Tgl.DateTime.Month, Tgl.DateTime.Year, "IsJual=0")
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
            strsql = "Select MBayarHutang.*,Malamat.Nama as Alamat,MBank.Kode as KodeKas,MBank.Nama as AkunKas " & _
            "FROM ((MBayarHutang LEFT JOIN MAlamat ON MBayarHutang.IDAlamat=MAlamat.NoID) " & _
            "LEFT JOIN MBank on MBayarHutang.IDAkunKas=MBank.NoID) " & _
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
                    RefreshTT()
                    txtAlamat.Text = NullTostr(oDS.Tables("MBayarHutang").Rows(0).Item("Alamat"))
                    txtKode.Text = NullTostr(oDS.Tables("MBayarHutang").Rows(0).Item("Kode"))
                    txtKodeReff.Text = NullTostr(oDS.Tables("MBayarHutang").Rows(0).Item("KodeReff"))
                    txtCatatan.Text = NullTostr(oDS.Tables("MBayarHutang").Rows(0).Item("Keterangan"))
                    txtKas.Text = NullToStr(oDS.Tables("MBayarHutang").Rows(0).Item("AkunKas")) 'NullTostr(oDS.Tables("MBayarHutang").Rows(0).Item("KodeKas")) & " - " &
                    Tgl.EditValue = NullToDate(oDS.Tables("MBayarHutang").Rows(0).Item("Tanggal"))
                    oldTanggal = NullToDate(oDS.Tables("MBayarHutang").Rows(0).Item("Tanggal"))
                    oldNota = NullTostr(oDS.Tables("MBayarHutang").Rows(0).Item("Kode"))
                    txtTT.EditValue = NullToLong(oDS.Tables("MBayarHutang").Rows(0).Item("IDTT"))
                    txtDiscount.EditValue = NullToDbl(oDS.Tables("MBayarHutang").Rows(0).Item("Retur"))
                    txtDenda.EditValue = NullToDbl(oDS.Tables("MBayarHutang").Rows(0).Item("Denda"))
                    Jumlah.EditValue = NullToDbl(oDS.Tables("MBayarHutang").Rows(0).Item("Jumlah"))
                    txtMaterai.EditValue = NullToDbl(oDS.Tables("MBayarHutang").Rows(0).Item("Materai"))
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
            Dim Denda As Double
            Dim view As ColumnView = GridControl1.FocusedView
            Sisa = Jumlah.EditValue
            DiscountRp = txtDiscount.EditValue
            DiscountRp = txtDenda.EditValue
            If Sisa = 0 Then DiscountProsen = 100 Else DiscountProsen = DiscountRp * 100 / (Sisa + DiscountRp)
            While i < view.RowCount 'Sisa > 0 And
                If view.GetRowCellValue(i, "Sisa") > (Sisa + DiscountRp + Denda) Then
                    view.SetRowCellValue(i, "Bayar", Sisa)
                    view.SetRowCellValue(i, "Retur", DiscountRp)
                    view.SetRowCellValue(i, "Denda", Denda)
                    Sisa = 0
                    DiscountRp = 0
                Else
                    view.SetRowCellValue(i, "Retur", view.GetRowCellValue(i, "Sisa") * DiscountProsen / 100)
                    view.SetRowCellValue(i, "Bayar", view.GetRowCellValue(i, "Sisa") - view.GetRowCellValue(i, "Sisa") * DiscountProsen / 100)
                    view.SetRowCellValue(i, "Denda", view.GetRowCellValue(i, "Denda"))
                    Sisa = Sisa - view.GetRowCellValue(i, "Bayar")
                    view.SetRowCellValue(i, "SisaPembayaran", Sisa)
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

    Function GetKode(ByVal NmTable As String, ByVal KodeNota As String, ByVal Bulan As Integer, ByVal Tahun As Integer, Optional ByVal Filter As String = "") As String
        Dim curentcursor As Cursor = Windows.Forms.Cursor.Current
        Windows.Forms.Cursor.Current = Cursors.WaitCursor
        Dim Id As Long
        Id = NullTolong(EksekusiSQLSkalar("Select count(NoID) as x from " & NmTable & " where month(tanggal)=" & Bulan & " and year(tanggal)=" & Tahun & " " & IIf(Filter <> "", " and ", "") & Filter)) + 1
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
        If Cekvaliditas Then
            If Simpan() Then
                DialogResult = Windows.Forms.DialogResult.OK
                Me.Close()
            End If
        End If

    End Sub
    Function CekValiditas() As Boolean
        Dim hasil As Boolean = True
        Dim bayar As Double
        Dim Pending As Boolean
        Dim i As Integer
        Dim view As ColumnView = GridControl1.FocusedView
        'jumlah potong Hutang harus sama dengan detil yg dibayar
        If Jumlah.EditValue <> GridView1.Columns("Bayar").SummaryItem.SummaryValue Then
            MsgBox("Jumlah item yang dibayar harus sama dengan Sub Total", MsgBoxStyle.Information)
            hasil = False
        End If
        For i = 0 To View.RowCount - 1

            Bayar = View.GetRowCellValue(i, "Bayar")
            Pending = NullToBool(View.GetRowCellValue(i, "Pending"))
            If Bayar <> 0 And Pending Then
                MsgBox("Item Pending tidak boleh di Bayar", MsgBoxStyle.Information)
                hasil = False
                Exit For
            End If
        Next
        Return hasil
    End Function
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

    Private Sub txtDenda_EditValueChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtDenda.EditValueChanged
        HitungJumlahBayar()
    End Sub

    Private Sub LabelControl1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LabelControl1.Click

    End Sub

    Private Sub LabelControl12_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LabelControl12.Click

    End Sub

    Private Sub LabelControl7_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LabelControl7.Click

    End Sub

    Private Sub LabelControl8_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LabelControl8.Click

    End Sub

    Private Sub lbAkunDenda_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lbAkunDenda.Click

    End Sub

    Private Sub LabelControl11_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LabelControl11.Click

    End Sub

    Private Sub LabelControl3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LabelControl3.Click

    End Sub

    Private Sub txtJumlahBayar_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtJumlahBayar.EditValueChanged

    End Sub

    Private Sub ButtonEdit1_ButtonClick1(ByVal sender As Object, ByVal e As DevExpress.XtraEditors.Controls.ButtonPressedEventArgs) Handles txtMaterai.ButtonClick
        Dim frm As New frmLookupMaterai
        frm.Strsql = "Select MMaterai.* from MMaterai "
        If frm.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
            txtMaterai.EditValue = frm.Nilai
        End If
        frm.Dispose()
    End Sub
 

    Private Sub txtMaterai_EditValueChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtMaterai.EditValueChanged
        HitungJumlahBayar()
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

    Private Sub txtTT_ButtonClick(ByVal sender As Object, ByVal e As DevExpress.XtraEditors.Controls.ButtonPressedEventArgs) Handles txtTT.ButtonClick
        If e.Button.Index = 1 Then
            GenerateByTT()
        End If
    End Sub
    Sub GenerateByTT()
        'Dim Ds As New DataSet
        Dim strsql As String
        Try
            strsql = "update TBayarHutangD  SET Bayar=MTTD.JumlahFaktur " & _
            "from MTTD inner join TBayarHutangD on IDBeli=MTTD.IDTransaksi " & _
            "and MTTD.IDJenisTransaksi=2 " & _
            "where MTTD.IDTT=" & NullToLong(txtTT.EditValue)
            EksekusiSQL(strsql)
            TampilDetil()
            Jumlah.EditValue = GridView1.Columns("Bayar").SummaryItem.SummaryValue
            'strsql = "SELECT MTTD.*  " & _
            '      " FROM MTTD  " & _
            '      " WHERE MTTD.IDTT=" & NullToLong(txtTT.EditValue) & _
            '       " ORDER BY MTTD.NoID"
            'Ds = ExecuteDataset("MTTD", strsql)
            'With Ds.Tables("MTTD")
            '    If .Rows.Count >= 1 Then
            '        For i As Integer = 0 To .Rows.Count - 1
            '            GridView1.
            '            GridView1.SetRowCellValue(
            '            'txtQty.EditValue / IsiKarton * txtKonversi.EditValue
            '            'Total sudah include PPN (jika Ada)
            '            Sql = "INSERT INTO MBeliD (NoID,IDBeli,IDBarangD,IDPOD,IDLPBD,NoUrut,Tgl,Jam,ExpiredDate,IDBarang," & _
            '                 "IDSatuan,Qty,QtyPcs,Harga,Biaya,HargaPcs,CTN,DiscPersen1,DiscPersen2,DiscPersen3,Disc1,Disc2,Disc3,Jumlah,Catatan,IDGudang,Konversi,PPN,HargaNetto,ProsenMargin,HitungJual,HargaJual)" & vbCrLf & _
            '                 " SELECT " & GetNewID("MBeliD", "NoID") & ", " & NoID & ", MPOD.IDBarangD, MPOD.NoID, NULL, " & GetNewID("MBeliD", "NoUrut", " WHERE MBeliD.IDBeli=" & NoID) & ", MPOD.Tgl, MPOD.Jam, NULL, MPOD.IDBarang, " & _
            '                 "MPOD.IDSatuan, MPOD.Qty, MPOD.QtyPcs, MPOD.Harga, 0, MPOD.HargaPcs, MPOD.Ctn, MPOD.DiscPersen1, MPOD.DiscPersen2, MPOD.DiscPersen3, MPOD.Disc1, MPOD.Disc2, MPOD.Disc3, MPOD.Jumlah, MPOD.Catatan, " & NullToLong(txtGudang.EditValue) & ", MPOD.Konversi,0, MPOD.HargaNetto, 0, 0, 0" & vbCrLf & _
            '                 " FROM MPOD INNER JOIN MPO ON MPO.NoID=MPOD.IDPO WHERE MPOD.NoID=" & NullToLong(.Rows(i).Item("NoID"))
            '            EksekusiSQL(Sql)
            '        Next
            '    End If
            'End With

            'Else 
            'End If
        Catch ex As Exception
            XtraMessageBox.Show("Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            'RefreshLookUpSTB()
            'Ds.Dispose()
        End Try
    End Sub
    Private Sub txtTT_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtTT.EditValueChanged

    End Sub

    Private Sub txtAlamat_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtAlamat.KeyUp

    End Sub
End Class