Imports DevExpress.XtraGrid.Views.Base
Imports DevExpress.XtraGrid.Views.Grid
Imports DevExpress.XtraGrid.Views.Grid.ViewInfo
Imports VPoint.mdlCetakCR
Public Class frmDaftarKasOut 
    Dim IDGudang As Long 
    Dim IDAkun As Long
    Dim IDKas As Long
    Dim SQL As String
    Dim NoID As Long
    Private Enum action_
        Edit = 0
        Preview = 1
        Print = 2
    End Enum
    Private Sub cmdClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdClose.Click
        Me.Close()
    End Sub

    Private Sub frmDaftarKasOut_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        GridView1.SaveLayoutToXml(Application.StartupPath & "\System\Layouts\" & Me.Name & ".xml")
    End Sub
    Private Sub SetCtlMe()
        IDGudang = DefIDGudang
        ' txtGudang.Text = defNamaGudang
        tglDari.DateTime = Today
        TglSampai.DateTime = Today
    End Sub
    Private Sub frmDaftarKasOut_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'TODO: This line of code loads data into the 'LintasDataSet.mkasoutd' table. You can move, or remove it, as needed.
        'Me.MkasoutdTableAdapter.Fill(Me.LintasDataSet.mkasoutd)
        SetCtlMe()
        TampilData()
        If Dir(Application.StartupPath & "\System\Layouts\" & Me.Name & ".xml") <> "" Then
            GridView1.RestoreLayoutFromXml(Application.StartupPath & "\System\Layouts\" & Me.Name & ".xml")
        End If

    End Sub



    Private Sub txtAkun_ButtonClick(ByVal sender As Object, ByVal e As DevExpress.XtraEditors.Controls.ButtonPressedEventArgs) Handles txtAkun.ButtonClick
        Select Case e.Button.Index
            Case 0
                Dim x As New frmLUAkun : x.pStatus = mdlAccPublik.ptipe.LookUp
                If x.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
                    IDAkun = x.NoID
                    txtAkun.Text = x.Nama & " - " & x.SubKlas
                End If
                x.Dispose()
            Case 1
                IDAkun = -1
                txtAkun.Text = ""
        End Select
        TampilData()
    End Sub
    Private Function SQLDetil() As String
        SQL = "SELECT mkasoutd.*, MAKUN2.Nama AS NamaAkun"
        SQL = SQL & " FROM mkasoutd LEFT OUTER JOIN"
        SQL = SQL & " makun MAKUN2 ON MAKUN2.ID = mkasoutd.IDAkun"
        Return SQL
    End Function
    Private Function SQLMaster() As String
        Dim isperluwhere As Boolean = True
        SQL = "SELECT mkasout.*, MGudang.Nama AS NamaGudang, " & vbCrLf
        SQL = SQL & " MBank.Nama as KasBank,makun.Nama AS NamaAkun " & vbCrLf
        SQL = SQL & " FROM mkasout LEFT OUTER JOIN" & vbCrLf
        SQL = SQL & "  MGudang ON MKasOut.IDGudang = MGudang.NoID LEFT OUTER JOIN" & vbCrLf
        SQL = SQL & " MBank  ON mkasout.IDKas = MBank.NoID LEFT OUTER JOIN" & vbCrLf
        SQL = SQL & " makun ON makun.ID = MBank.IDAkun   "

        If tglDari.Enabled And tglDari.Text <> "" Then
            If isperluwhere Then SQL = SQL & " Where " Else SQL = SQL & " and "
            SQL = SQL & " MKasOut.Tanggal>='" & Format(tglDari.DateTime, "yyyy/MM/dd") & "' AND MKasOut.Tanggal<='" & Format(TglSampai.DateTime, "yyyy/MM/dd") & "' "
            isperluwhere = False
        End If

        If txtkas.Enabled Then
            If isperluwhere Then SQL = SQL & " Where " Else SQL = SQL & " and "
            SQL = SQL & " Mkasout.IDKas=" & IDKas & " "
            isperluwhere = False
        End If
        If txtAkun.Enabled Then
            If isperluwhere Then SQL = SQL & " Where " Else SQL = SQL & " and "
            SQL = SQL & "  MBank.IDAkun=" & IDAkun & " "
            isperluwhere = False
        End If


        If cbStatus.EditValue.ToString = "1" Then
            If isperluwhere Then SQL = SQL & " Where " Else SQL = SQL & " and "
            SQL = SQL & "  MKasOut.IsPosted=1 "
            isperluwhere = False
        ElseIf cbStatus.EditValue.ToString = "0" Then
            If isperluwhere Then SQL = SQL & " Where " Else SQL = SQL & " and "
            SQL = SQL & "  MKasOut.IsPosted=0 OR MKasOut.IsPosted IS NULL"
            isperluwhere = False
        End If
        '  SQL = SQL & " ORDER BY MKasOut.ID"
        Return SQL
    End Function

    Private Sub TampilData()
        Try
            ExecuteDBGrid(GridControl1, SQLMaster)

        Catch ex As Exception
            FxMessage(ex.Message, ".:: Pesan Kesalahan ::.", MessageBoxButtons.OK, MessageBoxIcon.Error, , , ex.StackTrace)
        End Try
    End Sub

    Private Sub cbStatus_EditValueChanging(ByVal sender As Object, ByVal e As DevExpress.XtraEditors.Controls.ChangingEventArgs) Handles cbStatus.EditValueChanging
        TampilData()
    End Sub

    Private Sub LabelControl5_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LabelControl5.Click
        tglDari.Enabled = Not tglDari.Enabled
        TglSampai.Enabled = tglDari.Enabled
    End Sub

    Private Sub cmdRefresh_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdRefresh.Click
        Dim x
        x = GridView1.FocusedRowHandle
        TampilData()

        GridView1.ClearSelection()
        GridView1.ClearColumnsFilter()
        GridView1.ClearColumnErrors()
        GridView1.OptionsFilter.Reset()
        GridView1.ShowFindPanel()
        GridView1.FocusedRowHandle = x
        GridView1.SelectRow(GridView1.FocusedRowHandle)

    End Sub

    Private Sub cmdEdit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdEdit.Click
        Try
            Dim view As ColumnView = GridControl1.FocusedView
            Dim row As System.Data.DataRow = view.GetDataRow(GridView1.FocusedRowHandle)
            NoID = NullToLong(row("ID"))
            If NullToBool(row("IsPosted")) = False Then
                Dim x As New frmEntriKasOut
                x.NoID = NoID
                x.pStatus = mdlAccPublik.ptipe.Edit
                If x.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
                    TampilData()
                    GridView1.ClearSelection()
                    GridView1.FocusedRowHandle = GridView1.LocateByValue("ID", x.NoID)
                    GridView1.SelectRow(GridView1.FocusedRowHandle)
                End If
                x.Close()
                x.Dispose()
            Else
                Dim x As New frmEntriKasOut
                x.NoID = NoID
                x.SimpleButton9.Enabled = False
                x.pStatus = mdlAccPublik.ptipe.Edit
                If x.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
                    TampilData()
                    GridView1.ClearSelection()
                    GridView1.FocusedRowHandle = GridView1.LocateByValue("ID", x.NoID)
                    GridView1.SelectRow(GridView1.FocusedRowHandle)
                End If
                x.Close()
                x.Dispose()
                'msgbox("Nota Sudah Posting tidak boleh diedit! ")
            End If

        Catch ex As Exception
            FxMessage(ex.Message, ".:: Pesan Kesalahan ::.", MessageBoxButtons.OK, MessageBoxIcon.Error, , , ex.StackTrace)
        End Try
    End Sub

    Private Sub cmdNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdNew.Click
        Try
            Dim x As New frmEntriKasOut
            x.pStatus = mdlAccPublik.ptipe.Baru
            x.NoID = 0
            If x.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
                TampilData()
                GridView1.ClearSelection()
                GridView1.FocusedRowHandle = GridView1.LocateByValue("ID", x.NoID)
                GridView1.SelectRow(GridView1.FocusedRowHandle)
            End If
            x.Close()
            x.Dispose()
        Catch ex As Exception
            FxMessage(ex.Message, ".:: Pesan Kesalahan ::.", MessageBoxButtons.OK, MessageBoxIcon.Error, , , ex.StackTrace)
        End Try
    End Sub

    Private Sub cmdDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdDelete.Click
        Dim view As ColumnView = GridControl1.FocusedView
        'Try
        Dim row As System.Data.DataRow = view.GetDataRow(GridView1.FocusedRowHandle)
        Dim NOid As Long = row("ID")
        'HapusData(Noid, "MASTSTNK")
        If NullToBool(row("IsPosted")) = False Then
            If FxMessage("Yakin Mau Hapus data ini?", ".:: HAPUS DATA KAS KELUAR ::.", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                DeleteRowByID("MKasOut", "ID", NOid)
                DeleteRowByID("MKasOutD", "IDKasOut", NOid)
                TampilData()
            End If
        End If
    End Sub

    Private Sub BarButtonItem10_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem10.ItemClick
        If EditReport = True Then
            CetakBukti(action_.Edit, True)
        Else
            CetakBukti(action_.Preview, True)
        End If
    End Sub
    Private Sub CetakBukti(ByVal action As action_, ByVal iskas As Boolean)
        Dim namafile As String
        Dim view As ColumnView = GridControl1.FocusedView
        Dim row As System.Data.DataRow = view.GetDataRow(GridView1.FocusedRowHandle)
        Try
            namafile = Application.StartupPath & "\report\" & IIf(iskas, "BKK.rpt", "BBK.rpt")
            NoID = NullToLong(row("ID"))
            If System.IO.File.Exists(namafile) Then
                If EditReport Then
                    action = action_.Edit
                Else
                    action = action_.Preview
                End If

                ViewReport(Me.ParentForm, action, namafile, Me.Text, , , "NoID=" & NoID)
            Else
                DevExpress.XtraEditors.XtraMessageBox.Show("Nama File :" & namafile & " tidak ditemukan.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Stop)
            End If
        Catch EX As Exception
            FxMessage(EX.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error, , , EX.StackTrace)
        End Try
    End Sub

    Private Sub BarButtonItem11_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem11.ItemClick
        CetakBukti(IIf(EditReport = True, action_.Edit, action_.Preview), False)
    End Sub

    Private Sub GridView1_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles GridView1.MouseDown
        Dim View As GridView = CType(sender, GridView)
        If View Is Nothing Then Return
        ' obtaining hit info
        Dim hitInfo As GridHitInfo = View.CalcHitInfo(New System.Drawing.Point(e.X, e.Y))
        If (e.Button = Windows.Forms.MouseButtons.Right) And (hitInfo.InRow) And _
          (Not View.IsGroupRow(hitInfo.RowHandle)) Then
            PopupMenu1.ShowPopup(Control.MousePosition)
        End If
    End Sub

    Private Sub mnPosting_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles mnPosting.ItemClick
        Dim i As Integer
        Dim a As String = ""
        Dim j As Integer = 1
        Dim dlg As frmProgres
        dlg = New frmProgres 'WaitDialogForm("Query sedang diproses...", "Mohon Tunggu Sebentar.")
        dlg.Show()
        dlg.Owner = Me
        dlg.TopMost = False
        dlg.ProgressBarControl1.Position = 0
        Dim jumItem As Integer = GridView1.SelectedRowsCount
        For Each i In GridView1.GetSelectedRows
            a = CStr(GridView1.GetDataRow(i).Item("ID"))
            'VPOINT.Posting.PostingKasOut.Unposting(NullTolong(a))
            If Not NullToBool(GridView1.GetDataRow(i).Item("IsPosted")) Then
                PostingKasKeluar(a, NullToStr(GridView1.GetDataRow(i).Item("Keterangan")))
                EksekusiSQL("Update MKasOut set IsPosted=1 where ID=" & a)
            End If
            dlg.ProgressBarControl1.Position = j * 100 \ jumItem
            dlg.ProgressBarControl1.Refresh()
            Application.DoEvents()
            j = j + 1
        Next
        dlg.Dispose()
        TampilData()
        'Dim i As Integer
        'Dim a As String = ""
        'Dim j As Integer = 1
        'Dim ds As New DataSet
        'Dim dlg As frmProgres, pos As New VPOINT.Posting.PostingKasOut
        'dlg = New frmProgres 'WaitDialogForm("Query sedang diproses...", "Mohon Tunggu Sebentar.")
        'dlg.Show()
        'dlg.Owner = Me
        'dlg.TopMost = False
        'dlg.ProgressBarControl1.Position = 0
        'Dim jumItem As Integer = GridView1.SelectedRowsCount
        'For Each i In GridView1.GetSelectedRows
        '    a = CStr(GridView1.GetDataRow(i).Item("ID"))
        '    Dim x As String = "SELECT MKasOut.*, " & _
        '    " case when malamat.jenis=3 then mmatauang.idakunpiutangpegawai when malamat.jenis=4 then mmatauang.idakunPiutangRekan when malamat.jenis=1 then mmatauang.idakunPiutang when malamat.jenis=2 then mmatauang.idakunPiutang else -1 end as idakunpiutang " & _
        '    " FROM (MKasOut left JOIN MAlamat on MKasOut.IDAlamat=MAlamat.id) LEFT JOIN MMataUang ON MKasOut.IDMataUang=MMataUang.ID WHERE MKasOut.ID=" & NullTolong(a)
        '    ds = ExecuteDataset("tbl", x)
        '    If ds.Tables("tbl").Rows.Count >= 1 Then
        '        If NullTobool(ds.Tables("tbl").Rows(0).Item("IsPosted")) = False Then
        '            Try
        '                VPOINT.Posting.PostingKasOut.PostingKasOut(a, NullTolong(ds.Tables("tbl").Rows(0).Item("TypeKasOut")), _
        '                , NullTolong(ds.Tables("tbl").Rows(0).Item("NilaiKurs")), _
        '                NullTolong(ds.Tables("tbl").Rows(0).Item("IDAlamat")), _
        '                NullTolong(ds.Tables("tbl").Rows(0).Item("IDAkunPiutang")), _
        '                NullTolong(ds.Tables("tbl").Rows(0).Item("IDAkunPiutang")), NullTostr(ds.Tables("tbl").Rows(0).Item("Keterangan")))

        '                SQL = "UPDATE MKasOut SET IsPosted=1 WHERE ID=" & NullTolong(a)
        '                EksekusiSQL(SQL)

        '            Catch ex As Exception
        '                If FxMessage("Ada kesalahan, gagalkan Posting?" & vbCrLf & ex.Message, NamaAplikasi, MessageBoxButtons.YesNo, MessageBoxIcon.Error, MessageBoxDefaultButton.Button2) = Windows.Forms.DialogResult.Yes Then
        '                    Exit For
        '                End If
        '            End Try
        '        End If
        '    End If
        '    ds.Dispose()
        '    dlg.ProgressBarControl1.Position = j * 100 \ jumItem
        '    dlg.ProgressBarControl1.Refresh()
        '    Application.DoEvents()
        '    j = j + 1
        'Next
        'dlg.Dispose()
        'TampilData()
    End Sub

    Private Sub mnUnPosting_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles mnUnPosting.ItemClick
        Dim i As Integer
        Dim a As String = ""
        Dim j As Integer = 1
        Dim dlg As frmProgres
        dlg = New frmProgres 'WaitDialogForm("Query sedang diproses...", "Mohon Tunggu Sebentar.")
        dlg.Show()
        dlg.Owner = Me
        dlg.TopMost = False
        dlg.ProgressBarControl1.Position = 0
        Dim jumItem As Integer = GridView1.SelectedRowsCount
        For Each i In GridView1.GetSelectedRows
            a = CStr(GridView1.GetDataRow(i).Item("ID"))
            Unposting(a)
            EksekusiSQL("Update MKasOut set IsPosted=0 where ID=" & a)
            dlg.ProgressBarControl1.Position = j * 100 \ jumItem
            dlg.ProgressBarControl1.Refresh()
            Application.DoEvents()
            j = j + 1
        Next
        dlg.Dispose()
        TampilData()
    End Sub

    Private Sub cmdExport_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdExport.Click
        'ExportGrid(GridControl1, ExportTo.Excel)

    End Sub



    Private Sub mnLihathasil_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles mnHasilPosting.ItemClick
        Dim view As ColumnView = GridControl1.FocusedView
        Dim row As System.Data.DataRow = view.GetDataRow(GridView1.FocusedRowHandle)
        NoID = NullToLong(row("ID"))
        If NullToBool(row("IsPosted")) Then
            Dim x As New frmViewJurnal
            x.IDTransaksi = NoID
            x.IDTypeTransaksi = 3
            If x.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
                'TampilData()
            End If
            x.Close()
            x.Dispose()
        End If
    End Sub

    Private Sub BarButtonItem1_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles mnRepairPosting.ItemClick
        Dim i As Integer
        Dim a As String = ""
        Dim j As Integer = 1
        Dim ds As New DataSet
        Dim ptr
        Dim dlg As frmProgres
        dlg = New frmProgres 'WaitDialogForm("Query sedang diproses...", "Mohon Tunggu Sebentar.")
        dlg.Show()
        dlg.Owner = Me
        dlg.TopMost = False
        dlg.ProgressBarControl1.Position = 0
        Dim jumItem As Integer = GridView1.SelectedRowsCount
        ptr = GridView1.FocusedRowHandle
        For Each i In GridView1.GetSelectedRows
            a = CStr(GridView1.GetDataRow(i).Item("ID"))
            'VPOINT.Posting.PostingKasOut.Unposting(NullTolong(a))
            SQL = "UPDATE MKasOut SET IsPosted=0 WHERE ID=" & NullToLong(a)

            Application.DoEvents()
            'Posting Lagi
            Dim x As String = "SELECT MKasOut.*, " & _
           " case when malamat.jenis=3 then mmatauang.idakunpiutangpegawai when malamat.jenis=4 then mmatauang.idakunPiutangRekan when malamat.jenis=1 then mmatauang.idakunPiutang when malamat.jenis=2 then mmatauang.idakunPiutang else -1 end as idakunpiutang " & _
           " FROM (MKasOut left JOIN MAlamat on MKasOut.IDAlamat=MAlamat.id) LEFT JOIN MMataUang ON MKasOut.IDMataUang=MMataUang.ID WHERE MKasOut.ID=" & NullToLong(a)
            ds = ExecuteDataset("tbl", x)
            If ds.Tables("tbl").Rows.Count >= 1 Then
                If NullToBool(ds.Tables("tbl").Rows(0).Item("IsPosted")) = False Then
                    Try
                        'VPOINT.Posting.PostingKasOut.PostingKasOut(a, NullTolong(ds.Tables("tbl").Rows(0).Item("TypeKasOut")), _
                        ', NullTolong(ds.Tables("tbl").Rows(0).Item("NilaiKurs")), _
                        'NullTolong(ds.Tables("tbl").Rows(0).Item("IDAlamat")), _
                        'NullTolong(ds.Tables("tbl").Rows(0).Item("IDAkunPiutang")), _
                        'NullTolong(ds.Tables("tbl").Rows(0).Item("IDAkunPiutang")), NullTostr(ds.Tables("tbl").Rows(0).Item("Keterangan")))
                        PostingKasKeluar(a, NullToStr(ds.Tables("tbl").Rows(0).Item("Keterangan")))
                        SQL = "UPDATE MKasOut SET IsPosted=1 WHERE ID=" & NullToLong(a)
                        EksekusiSQL(SQL)

                    Catch ex As Exception
                        If FxMessage("Ada kesalahan, gagalkan Posting?" & vbCrLf & ex.Message, NamaAplikasi, MessageBoxButtons.YesNo, MessageBoxIcon.Error, MessageBoxDefaultButton.Button2) = Windows.Forms.DialogResult.Yes Then
                            Exit For
                        End If
                    End Try
                End If
            End If
            Application.DoEvents()

            dlg.ProgressBarControl1.Position = j * 100 \ jumItem
            dlg.ProgressBarControl1.Refresh()
            Application.DoEvents()
            j = j + 1
        Next
        dlg.Dispose()
        ds.Clear()
        ds.Dispose()
        TampilData()

        GridView1.ClearSelection()
        GridView1.FocusedRowHandle = ptr
        GridView1.SelectRow(GridView1.FocusedRowHandle)

    End Sub

    Private Sub txtkas_ButtonClick(ByVal sender As Object, ByVal e As DevExpress.XtraEditors.Controls.ButtonPressedEventArgs) Handles txtkas.ButtonClick
        Select Case e.Button.Index
            Case 0
                Dim x As New frmLUKas : x.pStatus = mdlAccPublik.ptipe.LookUp
                If x.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
                    IDKas = x.NoID
                    txtkas.Text = x.Nama '& "-" & x.nore
                End If
                x.Dispose()
            Case 1
                IDKas = -1
                txtkas.Text = ""
        End Select
        TampilData()
    End Sub

    Private Sub txtkas_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtkas.EditValueChanged

    End Sub

    Private Sub LabelControl2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lbKas.Click
        txtkas.Enabled = Not txtkas.Enabled
    End Sub

    Private Sub lbAkun_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lbAkun.Click
        txtAkun.Enabled = Not txtAkun.Enabled
    End Sub

    Public Shared Sub Unposting(ByVal ID As Long)
        Dim str As String, tbl As String = "kas"
        Dim dbs As New DataSet

        str = "SELECT * FROM MJurnal WHERE IDTransaksi=" & ID & " AND IDJenisTransaksi=3 AND IsPosting=1"
        dbs = ExecuteDataset(tbl, str)
        If Not dbs.Tables(tbl).Rows.Count >= 1 Then
            str = "DELETE MJurnalD FROM MJurnalD inner Join Mjurnal ON MJurnald.IDJurnal=MJurnal.ID WHERE Mjurnal.IDTransaksi=" & ID & " AND Mjurnal.IDJenisTransaksi=3"
            EksekusiSQL(str)
            str = "DELETE FROM MJurnal WHERE IDTransaksi=" & ID & " AND IDJenisTransaksi=3"
            EksekusiSQL(str)
            str = "DELETE FROM MHutang WHERE MHutang.IDTransaksi=" & ID & " AND MHutang.IDJenisTransaksi=3"
            EksekusiSQL(str)
            str = "DELETE FROM MKartuKas WHERE MKartuKas .IDTransaksi=" & ID & " AND MKartuKas .IDJenisTransaksi=3"
            EksekusiSQL(str)
            'str = "DELETE FROM MPiutang WHERE MPiutang.IDTransaksi=" & ID & " AND MPiutang.IDJenisTransaksi=3"
            'clssql.EksekusiSQl(str)
            str = "Update MKasOut  Set IdJurnal= -1,IsPosted=0,IDUserPosting=0 " & _
            "WHERE MKasOut.ID=" & ID
            EksekusiSQL(str)
        End If
    End Sub

    Public Shared Function PostingKasKeluar(ByVal ID As Integer, ByVal keterangan As String) As Boolean
        Dim ds As New DataSet, i As Integer, namatabel As String = "MKasOutD"
        Dim strSQL As String
        Dim IDJurnal As Long
        Dim IDHutang As Long
        'Dim IDJurnalD As Long
        Try
            'KARTU KAS/BANK
            strSQL = "INSERT INTO MKartuKas ( IDJenistransaksi,IDTransaksi, IDKas,Tanggal,Kode,KodeReff,Keterangan, " & _
              "Kredit, KreditA,  Kurs, IDMataUang, Debet, DebetA ) " & _
              "SELECT 3 AS IDJenisTransaksi,ID,MKasOut.IDKas,MKasOut.Tanggal,MKasOut.Kode,MKasOut.KodeReff,MKasOut.Keterangan," & _
              "Jumlah,JumlahA,NilaiKurs,IDMataUang,0 as Debet,0 as DebetA  " & _
              "FROM MKasOut " & _
              "WHERE MKasOut.ID=" & ID
            EksekusiSQL(strSQL)

            IDJurnal = GetNewID("MJurnal", "ID")
            strSQL = "INSERT INTO MJurnal ( NoPOD,KodeReff,ID,IDDepartemenUser, Tanggal, Kode, Keterangan, IDJenisTransaksi,IDTransaksi,IDUSerEntry, IDUserPosting ) " & _
                  "SELECT NoPOD,KodeReff," & IDJurnal & " AS ID,IDDepartemenUser, Tanggal, Kode, Keterangan, 3 AS IDJenisTransaksi,ID,IDUSerEntry, " & IDUserAktif & " AS IDUserPosting " & _
                  "FROM MKasOut " & _
                  "WHERE MKasOut.ID=" & ID
            EksekusiSQL(strSQL)
            If Not IsJurnalKasSatuLawanSatu Then
                'IDJurnalD = clsSQL.GetNewID("MJurnalD", "ID")
                strSQL = "INSERT INTO MJurnalD ( IDJurnal, IDGudang,IDWilayah,IDDepartemen, IDKas, IDAkun, " & _
                      "Kredit, KreditA,  Kurs, IDMataUang, Debet, DebetA,IsBalancing ) " & _
                      "SELECT " & IDJurnal & " AS IDJurnal ," & _
                      "IDGudang, (select mgudang.idwilayah from mgudang where mgudang.NoID=idgudang),IDDepartemen,IDKas,(Select MBank.IDAkun FROM MBank WHERE MBank.NoID=MKasOut.IDKas),Jumlah,JumlahA,NilaiKurs,IDMataUang,0 as Krdeit,0 as Debet, 0 as Isbalancing " & _
                      "FROM MKasOut " & _
                      "WHERE MKasOut.ID=" & ID
                EksekusiSQL(strSQL)
            End If
            'Jurnal Hutang/piutang Kredit
            ds = ExecuteDataset(namatabel, "Select MKasOutD.ID,MKasOutD.IsBukuPembantu,MKasOutD.IDDepartemen,MKasOutD.IdMataUang,MKasOutD.DebetA,MKasOutD.KreditA,MKasOutD.Kurs,MKasOut.IDDepartemen as IDDepartemenKas " & _
                 "FROM MKasOutD  inner join MKasOut on MKasOutD.IDKasOut=MKasOut.ID " & _
                 "WHERE MKasOutD.IDKasOut=" & ID)
            If ds.Tables(namatabel).Rows.Count > 0 Then
                For i = 0 To ds.Tables(namatabel).Rows.Count - 1
                    'IDHutang = clssql.GetNewID("MHutang", "ID")
                    'SaldoHutang = GetSaldoSupplier(IDAlamat, ds.Tables(namatabel).Rows(i).Item("IDDepartemen"))
                    If NullToLong(ds.Tables(namatabel).Rows(i).Item("IsBukuPembantu")) <> 0 Then
                        IDHutang = GetNewID("MHutang", "ID")
                        strSQL = "INSERT INTO MHutang ( ID,IDWilayah,IDDepartemen,Kode, KodeReff, IDCard, Tanggal, JatuhTempo, IDTransaksi, IDJenisTransaksi, IDPembelian,Keterangan, KreditA,DebetA,Kredit,Debet,retur, IDMataUang, NilaiTukar, IsSaldoAwal) " & _
                        "SELECT " & IDHutang & ",MkasOutD.IDWilayah,MkasOutD.IDDepartemen,Kode,KodeReff,MKasOut.IDAlamat, Tanggal, Tanggal As JatuhTempo, MkasOut.ID, 3 AS JenisTransaksi,0 As IDPembelian,MkasOutD.Keterangan, 0 As Pemjualan,MkasOutD.KreditA   AS Pembayaran,0 As Pemjualan,MkasOutD.KreditA*MkasOutD.Kurs AS Pembayaran,0 as retur, MkasOutD.IDMataUang, MkasOutD.Kurs, 0 AS IsSaldoAwal " & _
                        "FROM MKasOut INNER JOIN MKasOutD on MkasOut.ID=MkasOutD.IDKasOut " & _
                        "WHERE MKasOutD.ID=" & CLng(ds.Tables(namatabel).Rows(i).Item("ID"))
                        EksekusiSQL(strSQL)

                    End If
                    If IsJurnalKasSatuLawanSatu Then
                        'IDJurnalD = clsSQL.GetNewID("MJurnalD", "ID")
                        'KAS
                        strSQL = "INSERT INTO MJurnalD ( IDJurnal, IDGudang,IDWilayah,IDDepartemen, IDAkun, " & _
                              "Kredit, KreditA,Kurs, IDMataUang,Debet, DebetA, Keterangan, IsBalancing,IDPasangan ) " & _
                              "Select " & IDJurnal & " AS IDJurnal, MKASOUT.IDGudang,(SELECT MGudang.IDWilayah FROM MGudang WHERE MKASOUT.IDGudang=MGudang.NoID),MKASOUT.IDDepartemen, " & "(Select MBank.IDAkun from MBank where MBank.NoID=MKasout.IDKas) " & " as IDAkun, " & _
                              "(MKasOutD.Debet), ((MKasOutD.Debet) ), 1, 1 as IDMataUang, 0, 0,MKasOutD.Keterangan, 0 AS IsBalancing  " & _
                              "," & (i + 1) & " FROM MKasOutD INNER JOIN MKASOUT ON MKASOUTD.IDKASOUT=MKASOUT.ID " & _
                              "WHERE MKasOutD.ID=" & CLng(ds.Tables(namatabel).Rows(i).Item("ID"))
                        EksekusiSQL(strSQL)
                    End If

                    'IDJurnalD = clsSQL.GetNewID("MJurnalD", "ID")
                    'LAWAN KAS/DETILNYA
                    strSQL = "INSERT INTO MJurnalD ( IDJurnal, IDGudang,IDWilayah,IDDepartemen, IDAkun, " & _
                          " Debet, DebetA,Kurs, IDMataUang,Kredit, KreditA, Keterangan, IsBalancing,IDPasangan ) " & _
                          "Select " & IDJurnal & " AS IDJurnal, IDGudang,IDWilayah,IDDepartemen, IDAkun, " & _
                          "Debet, Debet, Kurs, IDMataUang, Kredit, Kredit,Keterangan, 0 AS IsBalancing  " & _
                           "," & (i + 1) & "FROM MKasOutD " & _
                          "WHERE MKasOutD.ID=" & CLng(ds.Tables(namatabel).Rows(i).Item("ID"))
                    EksekusiSQL(strSQL)
                    'Update Aneka Rempah Jaya jurnal 1 lawan 1 2008-03-13
                    'Antar Kantor
                Next
            End If
            CekAntarKantor(IDJurnal, keterangan)
            strSQL = "Update MKasOut  Set IdJurnal= " & IDJurnal & ", IsPosted=1,IDUserPosting=" & IDUserAktif & " " & _
                  "WHERE MKasOut.ID=" & ID
            EksekusiSQL(strSQL)
            Return True
        Catch ex As Exception
            MsgBox(ex.Message, MessageBoxButtons.OK + MessageBoxIcon.Error)
            Return False
        End Try
    End Function
    Public Shared Sub CekAntarKantor(ByVal idjurnal As Long, ByVal Keterangan As String)
        'Dim dbs As New ADODB.Connection
        'Dim RsCekRecord As New ADODB.Recordset
        Dim str As String
        Dim fieldDetildef As String
        Dim NilaiDetildef As String
        Dim ds As New DataSet
        Dim i As Integer
        'dbs = New ADODB.Connection
        'dbs.ConnectionString = KoneksiStr
        'dbs.Open()
        fieldDetildef = "IDJurnal,IDDepartemen,IDAkun,IdMataUang,kurs,DebetA,KreditA,Debet,Kredit,IDTransaksi,IsBalancing,Keterangan"
        'Balancing antar departemen
        'Set RsCekRecord = dbs.Execute("Select IDDepartemen,sum(convert(Decimal(19,2),Kurs*DebetA)-convert(Decimal(19,2),Kurs*KreditA)) as selisih  From MJurnalD Where IDJurnal=" & IDJurnal & " GROUP BY IDDepartemen hAVING sum(convert(Decimal(19,2),Kurs*DebetA)-convert(Decimal(19,2),Kurs*KreditA))<>0")
        '        RsCekRecord = dbs.Execute("Select IDDepartemen,sum(convert(Decimal(19,2),Debet)-convert(Decimal(19,2),Kredit)) as selisih  From MJurnalD Where IDJurnal=" & IDJurnal & " GROUP BY IDDepartemen hAVING sum(convert(Decimal(19,2),Debet)-convert(Decimal(19,2),Kredit))<>0")
        ds = ExecuteDataset("AntarKantor", "Select IDDepartemen,sum(convert(Decimal(19,2),Debet)-convert(Decimal(19,2),Kredit)) as selisih  From MJurnalD Where IDJurnal=" & idjurnal & " GROUP BY IDDepartemen hAVING sum(convert(Decimal(19,2),Debet)-convert(Decimal(19,2),Kredit))<>0")
        If ds.Tables("AntarKantor").Rows.Count > 0 Then
            For i = 0 To ds.Tables("AntarKantor").Rows.Count - 1
                Application.DoEvents()
                If NullToDbl(ds.Tables("AntarKantor").Rows(i).Item("Selisih")) > 0 Then

                    NilaiDetildef = idjurnal & "," & _
                     NullToLong(ds.Tables("AntarKantor").Rows(i).Item("IDDepartemen")) & "," & _
                    defIDAkunAntarKantor & "," & _
                    defIDMataUang & "," & _
                    1 & "," & _
                    0 & "," & _
                    NullToLong(ds.Tables("AntarKantor").Rows(i).Item("Selisih")) & "," & _
                    0 & "," & _
                     NullToLong(ds.Tables("AntarKantor").Rows(i).Item("Selisih")) & "," & _
                    1 & "," & _
                   1 & ",rtrim('Antar kantor' + ' " & FixApostropi(Keterangan) & "')"
                Else
                    NilaiDetildef = idjurnal & "," & _
                    NullToLong(ds.Tables("AntarKantor").Rows(i).Item("IDDepartemen")) & "," & _
                    defIDAkunAntarKantor & "," & _
                    defIDMataUang & "," & _
                    1 & "," & _
                    -1 * NullToLong(ds.Tables("AntarKantor").Rows(i).Item("Selisih")) & "," & _
                    0 & "," & _
                    -1 * NullToLong(ds.Tables("AntarKantor").Rows(i).Item("Selisih")) & "," & _
                    0 & "," & _
                    1 & "," & _
                    1 & ",rtrim('Antar kantor' + ' " & FixApostropi(Keterangan) & "')"
                End If
                'RUMUS
                'RsCekRecord!Kredit=RsCekRecord!KreditA * RsCekRecord!Kurs
                'RsCekRecord!Debet=RsCekRecord!DebetA * RsCekRecord!Kurs

                str = "INSERT INTO MJurnalD(" & fieldDetildef & ") VALUES(" & NilaiDetildef & ")"
                EksekusiSQL(str)
                'IDDetil = IDDetil + 1
            Next
        End If
    End Sub
End Class