Imports System.Data.SqlClient
Imports DevExpress.XtraEditors
Imports DevExpress.XtraGrid.Views.Base

Public Class frLaporanSaldoHutangRekapPerSupplier
    Public IsNew As Boolean
    Public NoID As Long
    'Private IsJual As Byte = 0 '1 Bayar Piutang, 0 Bayar Hutang
    Private IDAlamat As Long


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
    End Sub




    Sub RefreshItem()
        Dim strsql As String
        If IDAlamat = -1 Then

            'C=Nota yang Diedit
            'B=Beli
            'RB Retur Beli
            If MsgBox("Anda Yakin akan menampilkan Semua Data?, mungkin memerlukan waktu yang cukup lama", MsgBoxStyle.Question + MsgBoxStyle.YesNo) = MsgBoxResult.No Then
                Exit Sub
            End If
        End If
        ',IsNull( C.Bayar,0),Isnull(C.Retur,0),Isnull(C.Denda,0)
        strsql = "Select MAlamat.Kode as KodeSupplier,Malamat.Nama as Supplier, TRS.* from (Select MBeli.IDSupplier," & _
                    "Sum(Total) as Jumlah,Sum(IsNull(B.Bayar,0)) as Terbayar,Sum(IsNull(RB.NilaiRetur,0)) as Retur,Sum(Total-IsNull( B.Bayar,0)-IsNull(RB.NilaiRetur,0)) as Sisa " & _
                    "FROM (MBeli Left Join (Select IDBeli,Sum(Bayar+Retur) as Bayar FROM MBayarHutangD where IsJual=0 and MBayarHutangD.IDBayarHutang<>" & NoID & " group by IDBeli) B on MBeli.NoID=B.IDBeli) " & _
                    "LEFT JOIN (select MBayarHutangD.IDBeli,MBayarHutangD.Bayar,MBayarHutangd.Retur,MBayarHutangD.Denda from MBayarHutangD where MBayarHutangD.IDBayarHutang=" & NoID & ") C on MBeli.NoID=C.IDBeli " & _
                    "LEFT JOIN (Select IDBeli,Sum(MreturBeli.Total)  as NilaiRetur FROM MReturBeli where MReturBeli.Isposted=1 group by IDBeli) RB on MBeli.NoID=RB.IDBeli " & _
                    "WHERE MBeli.isPosted=1 " & IIf(CheckEdit1.Checked, "", " and  Total-Isnull(B.Bayar,0)-IsNull(RB.NilaiRetur,0)<>0") & IIf(IDAlamat = -1, "", " and MBeli.IDSupplier=" & IDAlamat) & " group by MBeli.IDSUpplier) TRS left join MAlamat on TRS.IDSupplier=MAlamat.NoID"



        Dim oConn As New SqlConnection
        Dim ocmd As SqlCommand
        Dim oDA As SqlDataAdapter
        Dim oDS As New DataSet
        'Dim isAda As Boolean
        Dim curentcursor As Cursor = Windows.Forms.Cursor.Current
        Windows.Forms.Cursor.Current = Cursors.WaitCursor
        Try
            oConn.ConnectionString = StrKonSql
            ocmd = New SqlCommand(strsql, oConn)
            oConn.Open()
            oDA = New SqlDataAdapter(ocmd)
            oDA.Fill(oDS, "LaporanPembayaranPersupplier")
        Catch ex As Exception
            XtraMessageBox.Show(Me, ex.Message, ".:: Pesan Kesalahan ::.", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            oConn.Close()
        End Try
        GridControl1.DataSource = oDS.Tables("LaporanPembayaranPersupplier")
        Windows.Forms.Cursor.Current = curentcursor
        Application.DoEvents()
    End Sub

    Private Sub frBayarHutang_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        GridView1.SaveLayoutToXml(FolderLayouts & "LaporanBayarHutangPerSupplier.xml")
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
        cmdExcel.ImageList = DefImageList
        cmdExcel.ImageIndex = 11
        cmdPreview.ImageList = DefImageList
        cmdPreview.ImageIndex = 8

    End Sub
    Private Sub frBayarHutang_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        SetTombol()
        RefreshItem()
        If Dir(FolderLayouts & "LaporanBayarHutangPerSupplier.xml") <> "" Then
            GridView1.RestoreLayoutFromXml(FolderLayouts & "LaporanBayarHutangPerSupplier.xml")
        End If
    End Sub



    Private Sub cmdTutup_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdTutup.Click
        DialogResult = Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub


    Private Sub cmdRefresh_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdRefresh.Click
        RefreshItem()

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
    Sub ExportExcel()
        Dim dlgsave As New SaveFileDialog
        dlgsave.Title = "Export Daftar ke Excel"
        dlgsave.Filter = "Excel Files|*.xls"
        If dlgsave.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
            GridView1.ExportToXls(dlgsave.FileName)
            BukaFile(dlgsave.FileName)
        End If
        dlgsave.Dispose()
    End Sub
    Sub PrintPreview()

        GridControl1.ShowPrintPreview()

    End Sub
    Private Sub cmdExcel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdExcel.Click
        ExportExcel()
    End Sub

    Private Sub cmdPreview_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdPreview.Click
        PrintPreview()
    End Sub
End Class