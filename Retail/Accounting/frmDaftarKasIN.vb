Imports DevExpress.XtraGrid.Views.Base
Imports DevExpress.XtraGrid.Views.Grid
Imports DevExpress.XtraGrid.Views.Grid.ViewInfo

Imports VPoint.mdlCetakCR
Public Class frmDaftarKasIN
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

    Private Sub frmDaftarKasIN_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        GridView1.SaveLayoutToXml(Application.StartupPath & "\System\Layouts\" & Me.Name & "1.xml")
    End Sub
    Private Sub SetCtlMe()
        tglDari.DateTime = Today
        TglSampai.DateTime = Today
    End Sub
    Private Sub frmDaftarKasIN_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'TODO: This line of code loads data into the 'LintasDataSet.MKasINd' table. You can move, or remove it, as needed.
        'Me.MKasINdTableAdapter.Fill(Me.LintasDataSet.MKasINd)
        SetCtlMe()
        TampilData()
        If Dir(Application.StartupPath & "\System\Layouts\" & Me.Name & "1.xml") <> "" Then
            GridView1.RestoreLayoutFromXml(Application.StartupPath & "\System\Layouts\" & Me.Name & "1.xml")
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
        SQL = "SELECT MKasINd.*, MAKUN2.Nama AS NamaAkun"
        SQL = SQL & " FROM MKasINd LEFT OUTER JOIN"
        SQL = SQL & " makun MAKUN2 ON MAKUN2.ID = MKasINd.IDAkun"
        Return SQL
    End Function
    Private Function SQLMaster() As String
        SQL = "SELECT MKasIN.*, " & vbCrLf
        SQL = SQL & " MBank.Nama as KasBank, makun.Nama AS NamaAkun " & vbCrLf
        SQL = SQL & " FROM MKasIN LEFT OUTER JOIN" & vbCrLf
        SQL = SQL & " MBank  On MKasIn.IDKas=MBank.NoID LEFT OUTER JOIN" & vbCrLf
        SQL = SQL & "  makun ON MBank.IDAkun = makun.ID " & vbCrLf

        If tglDari.Enabled And tglDari.Text <> "" Then
            SQL = SQL & "WHERE MKasIN.Tanggal>='" & Format(tglDari.DateTime, "yyyy/MM/dd") & "' AND MKasIN.Tanggal<='" & Format(TglSampai.DateTime, "yyyy/MM/dd") & "' "
        Else
            SQL = SQL & " WHERE 1=1 "
        End If


        If txtkas.Enabled Then
            SQL = SQL & " AND MKasIn.IDKas=" & IDKas & " "
        End If
        If txtAkun.Enabled Then
            SQL = SQL & " AND MBank.IDAkun=" & IDAkun & " "
        End If

        If cbStatus.EditValue.ToString = "1" Then
            SQL = SQL & " AND MKasIN.IsPosted=1 "
        ElseIf cbStatus.EditValue.ToString = "0" Then
            SQL = SQL & " AND MKasIN.IsPosted=0 OR MKasIN.IsPosted IS NULL"
        End If

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
        TampilData()
    End Sub

    Private Sub cmdEdit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdEdit.Click
        Try
            Dim view As ColumnView = GridControl1.FocusedView
            Dim row As System.Data.DataRow = view.GetDataRow(GridView1.FocusedRowHandle)
            NoID = NullToLong(row("ID"))
            If NullToBool(row("IsPosted")) = False Then
                Dim x As New frmEntriKasIN
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
                Dim x As New frmEntriKasIN
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
            Dim x As New frmEntriKasIN
            x.pStatus = mdlAccPublik.ptipe.Baru
            x.NoID = 0
            If x.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
                TampilData()
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
                DeleteRowByID("MKasIN", "ID", NOid)
                DeleteRowByID("MKasIND", "IDKasIN", NOid)
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
            namafile = Application.StartupPath & "\report\" & IIf(iskas, "BKM.rpt", "BBM.rpt")
            NoID = NullToLong(row("ID"))
            ViewReport(Me.ParentForm, action, namafile, Me.Text, , , "NoID=" & NoID)
            'VPOINT.Function.Fungsi.CetakCRViewer(action, Me.MdiParent, namafile, "Cetak Bukti Kas Masuk", , "ID", NoID)
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

                PostingLainlain(a, NullToStr(GridView1.GetDataRow(i).Item("Keterangan")))
                EksekusiSQL("Update MKasIN set IsPosted=1 where ID=" & a)
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
        'Dim dlg As frmProgres ', pos As New VPOINT.Posting.PostingKasIN
        'dlg = New frmProgres 'WaitDialogForm("Query sedang diproses...", "Mohon Tunggu Sebentar.")
        'Try
        '    dlg.Show()
        '    dlg.Owner = Me
        '    dlg.TopMost = False
        '    dlg.ProgressBarControl1.Position = 0
        '    Dim jumItem As Integer = GridView1.SelectedRowsCount
        '    For Each i In GridView1.GetSelectedRows
        '        a = CStr(GridView1.GetDataRow(i).Item("ID"))
        '        Dim x As String = "SELECT MKasIN.*, " & vbCrLf & _
        '        " case when malamat.jenis=3 then mmatauang.idakunpiutangpegawai when malamat.jenis=4 then mmatauang.idakunPiutangRekan when malamat.jenis=1 then mmatauang.idakunPiutang when malamat.jenis=2 then mmatauang.idakunPiutang else -1 end as idakunpiutang " & vbCrLf & _
        '        " FROM (MKasIN left JOIN MAlamat on MKasIN.IDAlamat=MAlamat.id) LEFT JOIN MMataUang ON MKasIN.IDMataUang=MMataUang.ID WHERE MKasIN.ID=" & NullToLong(a)
        '        ds = ExecuteDataset("tbl", x)
        '        If ds.Tables("tbl").Rows.Count >= 1 Then
        '            If NullTobool(ds.Tables("tbl").Rows(0).Item("IsPosted")) = False Then
        '                Try
        '                    'VPOINT.Posting.PostingKasIN.PostingKasIN(a, NullTolong(ds.Tables("tbl").Rows(0).Item("TypeKasIN")), _
        '                    ', NullToDbl(ds.Tables("tbl").Rows(0).Item("NilaiKurs")), _
        '                    'NullTolong(ds.Tables("tbl").Rows(0).Item("IDAlamat")), _
        '                    'NullTolong(ds.Tables("tbl").Rows(0).Item("idakunpiutang")), _
        '                    'NullTolong(ds.Tables("tbl").Rows(0).Item("idakunpiutang")), NullTostr(ds.Tables("tbl").Rows(0).Item("Keterangan")))

        '                    SQL = "UPDATE MKasIN SET IsPosted=1 WHERE ID=" & NullTolong(a)
        '                    EksekusiSQL(SQL)

        '                Catch ex As Exception
        '                    If FxMessage("Ada kesalahan, gagalkan Posting?" & vbCrLf & ex.Message, NamaAplikasi, MessageBoxButtons.YesNo, MessageBoxIcon.Error, MessageBoxDefaultButton.Button2) = Windows.Forms.DialogResult.Yes Then
        '                        Exit For
        '                    End If
        '                End Try
        '            End If
        '        End If
        '        dlg.ProgressBarControl1.Position = j * 100 \ jumItem
        '        dlg.ProgressBarControl1.Refresh()
        '        Application.DoEvents()
        '        j = j + 1
        '    Next
        'Catch ex As Exception
        '    FxMessage("Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        'Finally
        '    dlg.Close()
        '    dlg.Dispose()
        '    ds.Dispose()
        '    TampilData()
        'End Try
    End Sub

    Private Sub mnUnPosting_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles mnUnPosting.ItemClick
        Dim a As String = ""
        Dim j As Integer = 1
        Dim str As String = ""
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
            str = "DELETE MJurnalD FROM MJurnalD inner Join Mjurnal ON MJurnald.IDJurnal=MJurnal.ID WHERE Mjurnal.IDTransaksi=" & a & " AND Mjurnal.IDJenisTransaksi=2"
            EksekusiSQL(str)
            str = "DELETE FROM MJurnal WHERE IDTransaksi=" & a & " AND IDJenisTransaksi=2"
            EksekusiSQL(str)
            str = "DELETE FROM MHutang WHERE MHutang.IDTransaksi=" & a & " AND MHutang.IDJenisTransaksi=2"
            EksekusiSQL(str)
            str = "DELETE FROM MKartuKas WHERE IDTransaksi=" & a & " AND IDJenisTransaksi=2"
            EksekusiSQL(str)

            EksekusiSQL("Update MKasIN set IsPosted=0 where ID=" & a)
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
        'Dim dlg As frmProgres
        'dlg = New frmProgres 'WaitDialogForm("Query sedang diproses...", "Mohon Tunggu Sebentar.")
        'dlg.Show()
        'dlg.Owner = Me
        'dlg.TopMost = False
        'dlg.ProgressBarControl1.Position = 0
        'Dim jumItem As Integer = GridView1.SelectedRowsCount
        'For Each i In GridView1.GetSelectedRows
        '    a = CStr(GridView1.GetDataRow(i).Item("ID"))
        '    'VPOINT.Posting.PostingKasIN.Unposting(NullTolong(a))
        '    dlg.ProgressBarControl1.Position = j * 100 \ jumItem
        '    dlg.ProgressBarControl1.Refresh()
        '    Application.DoEvents()
        '    j = j + 1
        'Next
        'dlg.Dispose()
        'TampilData()
    End Sub

    Private Sub cmdExport_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdExport.Click
        ExportGrid(GridControl1, ExportTo.Excel)
    End Sub

    Private Sub GridControl1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles GridControl1.Click

    End Sub


    Private Sub mnHasilPosting_ItemClick(ByVal sender As Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles mnHasilPosting.ItemClick
        Dim view As ColumnView = GridControl1.FocusedView
        Dim row As System.Data.DataRow = view.GetDataRow(GridView1.FocusedRowHandle)
        NoID = NullToLong(row("ID"))
        If NullToBool(row("IsPosted")) Then
            Dim x As New frmViewJurnal
            x.IDTransaksi = NoID
            x.IDTypeTransaksi = 2
            If x.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
                'TampilData()
            End If
            x.Close()
            x.Dispose()
        End If
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



    Private Sub lbKas_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lbKas.Click
        txtkas.Enabled = Not txtkas.Enabled
    End Sub

    Private Sub LabelControl2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LabelControl2.Click
        txtAkun.Enabled = Not txtAkun.Enabled
    End Sub


    Private Sub mnRepairPosting_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles mnRepairPosting.ItemClick
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
            'VPOINT.Posting.PostingKasIN.Unposting(NullTolong(a))
            SQL = "UPDATE MKasIN SET IsPosted=1 WHERE ID=" & NullTolong(a)
            EksekusiSQL(SQL)
            Application.DoEvents()
            'Posting Lagi
            Dim x As String = "SELECT MKasIN.*, " & vbCrLf & _
                 " case when malamat.jenis=3 then mmatauang.idakunpiutangpegawai when malamat.jenis=4 then mmatauang.idakunPiutangRekan when malamat.jenis=1 then mmatauang.idakunPiutang when malamat.jenis=2 then mmatauang.idakunPiutang else -1 end as idakunpiutang " & vbCrLf & _
                 " FROM (MKasIN left JOIN MAlamat on MKasIN.IDAlamat=MAlamat.id) LEFT JOIN MMataUang ON MKasIN.IDMataUang=MMataUang.ID WHERE MKasIN.ID=" & NullTolong(a)
            ds = ExecuteDataset("tbl", x)
            If ds.Tables("tbl").Rows.Count >= 1 Then
                If NullTobool(ds.Tables("tbl").Rows(0).Item("IsPosted")) = False Then
                    Try
                        'VPOINT.Posting.PostingKasIN.PostingKasIN(a, NullTolong(ds.Tables("tbl").Rows(0).Item("TypeKasIN")), _
                        ', NullToDbl(ds.Tables("tbl").Rows(0).Item("NilaiKurs")), _
                        'NullTolong(ds.Tables("tbl").Rows(0).Item("IDAlamat")), _
                        'NullTolong(ds.Tables("tbl").Rows(0).Item("idakunpiutang")), _
                        'NullTolong(ds.Tables("tbl").Rows(0).Item("idakunpiutang")), NullTostr(ds.Tables("tbl").Rows(0).Item("Keterangan")))

                        SQL = "UPDATE MKasIN SET IsPosted=1 WHERE ID=" & NullTolong(a)
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
    Public Shared Function PostingLainlain(ByVal ID As Integer, ByVal Keterangan As String) As Boolean
        Dim ds As New DataSet, i As Integer, namatabel As String = "MKasIND"
        Dim strsql As String
        Dim IDJurnal As Long
        Dim IDHutang As Long
        'Dim IDJurnalD As Long
        Try
            'KARTU KAS/BANK
            strsql = "INSERT INTO MKartuKas ( IDJenistransaksi,IDTransaksi, IDKas,Tanggal,Kode,KodeReff,Keterangan, " & _
      " DebetA,Debet,   Kurs, IDMataUang,Kredit, KreditA ) " & _
      "SELECT 2 AS IDJenisTransaksi,ID,MKasIn.IDKas,MKasIn.Tanggal,MKasIn.Kode,MKasIn.KodeReff,MKasIn.Keterangan," & _
      "Jumlah,JumlahA,NilaiKurs,IDMataUang,0 as kredit,0 as kreditA  " & _
      "FROM MKasIn " & _
      "WHERE MKasIn.ID=" & ID
            EksekusiSQL(strsql)

            IDJurnal = GetNewID("MJurnal", "ID")
            strsql = "INSERT INTO MJurnal ( NoPOD,KodeReff,ID,IDDepartemenUser, Tanggal, Kode, Keterangan, IDJenisTransaksi,IDTransaksi,IDUSerEntry, IDUserPosting ) " & _
                  "SELECT NoPOD,KodeReff," & IDJurnal & " AS ID,IDDepartemenUser, Tanggal, Kode, Keterangan, 2 AS IDJenisTransaksi,ID,IDUSerEntry, " & IDUserAktif & " AS IDUserPosting " & _
                  "FROM MKasIN " & _
                  "WHERE MKasIN.ID=" & ID
            EksekusiSQL(strsql)
            If Not IsJurnalKasSatuLawanSatu Then
                'IDJurnalD = clsSQL.GetNewID("MJurnalD", "ID")
                strsql = "INSERT INTO MJurnalD ( IDJurnal, IDGudang,IDWilayah,IDDepartemen, IDKas, IDAkun, " & _
                      "Debet, DebetA,Kurs, IDMataUang, Kredit, KreditA,  IsBalancing ) " & _
                      "SELECT " & IDJurnal & " AS IDJurnal ," & _
                      "IDGudang, (select mgudang.idwilayah from mgudang where mgudang.NoID=idgudang),IDDepartemen,IDKas,(Select MBank.IDAkun FROM MBank WHERE MBank.ID=MKasIN.IDKas),Jumlah,JumlahA,NilaiKurs,IDMataUang,0 as Krdeit,0 as Debet, 0 as Isbalancing " & _
                      "FROM MKasIN " & _
                      "WHERE MKasIN.ID=" & ID
                EksekusiSQL(strsql)
            End If
            'Jurnal Hutang/piutang Kredit
            ds = ExecuteDataset(namatabel, "Select ID,IsBukuPembantu,IDDepartemen,IdMataUang,DebetA,KreditA,Kurs " & _
                 "FROM MKasIND " & _
                 "WHERE MKasIND.IDKasIN=" & ID)
            If ds.Tables(namatabel).Rows.Count >= 1 Then
                For i = 0 To ds.Tables(namatabel).Rows.Count - 1
                    'IDHutang = clssql.GetNewID("MHutang", "ID")
                    'SaldoHutang = GetSaldoSupplier(IDAlamat, ds.Tables(namatabel).Rows(i).Item("IDDepartemen"))
                    If NullToLong(ds.Tables(namatabel).Rows(i).Item("IsBukuPembantu")) <> 0 Then
                        IDHutang = GetNewID("MHutang", "ID")
                        strsql = "INSERT INTO MHutang ( ID,IDWilayah,IDDepartemen,Kode,KodeReff, IDCard, Tanggal, JatuhTempo, IDTransaksi, IDJenisTransaksi, IDPembelian,Keterangan, KreditA,DebetA,Kredit,Debet,retur, IDMataUang, NilaiTukar, IsSaldoAwal) " & _
                        "SELECT " & IDHutang & ",MKasIND.IDWilayah,MKasIND.IDDepartemen,Kode,KodeReff, IDAlamat, Tanggal, Tanggal As JatuhTempo, MKasIN.ID, 2 AS JenisTransaksi,0 As IDPembelian,MKasIND.Keterangan, 0 As Penjualan,MKasIND.debet   AS Pembayaran,0 As Pemjualan,MKasIND.DebetA*MKasIND.Kurs AS Pembayaran,0 as retur, MKasIND.IDMataUang, MKasIND.Kurs, 0 AS IsSaldoAwal " & _
                        "FROM MKasIN INNER JOIN MKasIND on MKasIN.ID=MKasIND.IDKasIN " & _
                        "WHERE MKasIND.ID=" & CLng(ds.Tables(namatabel).Rows(i).Item("ID"))
                        EksekusiSQL(strsql)

                    End If
                    If IsJurnalKasSatuLawanSatu Then
                        'KAS
                        strsql = "INSERT INTO MJurnalD ( IDJurnal, IDGudang,IDWilayah,IDDepartemen, IDAkun, " & _
                              "Debet, DebetA, Kurs, IDMataUang,Kredit, KreditA,Keterangan, IsBalancing,IDPasangan ) " & _
                              "Select " & IDJurnal & " AS IDJurnal, MBank.IDGudang," & _
                              "MGudang.IDWilayah,1 as IDDepartemen, MBank.IDAkun, " & _
                              "MKasIND.Debet ,MKasIND.Debet ," & _
                              "1 as NilaiKurs, 1 as IDMataUang, 0, 0,MKasIND.Keterangan, 0 AS IsBalancing," & (i + 1) & " " & _
                              "FROM MKasIND INNER JOIN MKasIN ON MKasIND.IDKasIN=MKasIN.ID " & _
                              "LEFT JOIN MBank On MKasIn.IDKas=MBank.NoID " & _
                              "LEFT JOIN MGudang On MBank.IDGudang=MGudang.NoID " & _
                              "LEFT JOIN MWilayah On MGudang.IDWilayah=MWilayah.NoID " & _
                              "WHERE MKasIND.ID=" & CLng(ds.Tables(namatabel).Rows(i).Item("ID"))
                        EksekusiSQL(strsql)
                    End If
                    'LAWAN KAS/DETIL
                    strsql = "INSERT INTO MJurnalD ( IDJurnal, IDGudang,IDWilayah,IDDepartemen, IDAkun, " & _
                          "KreditA, Kredit, Kurs, IDMataUang,DebetA, Debet, Keterangan, IsBalancing,IDPasangan ) " & _
                          "Select " & IDJurnal & " AS IDJurnal, IDGudang,1 as IDWilayah,1 as IDDepartemen,  IDAkun, " & _
                          "Debet, Debet,1 as Kurs, 1 as IDMataUang, 0, 0,Keterangan, 0 AS IsBalancing ," & (i + 1) & " " & _
                          "FROM MKasIND " & _
                          "WHERE MKasIND.ID=" & CLng(ds.Tables(namatabel).Rows(i).Item("ID"))
                    EksekusiSQL(strsql)

                Next
            End If
            CekAntarKantor(IDJurnal, Keterangan)

            strsql = "Update MKasIN  Set IdJurnal= " & IDJurnal & ", IsPosted=1,IDUserPosting=" & IDUserAktif & " " & _
                  "WHERE MKasIN.ID=" & ID
            EksekusiSQL(strsql)
            PostingLainlain = True
        Catch ex As Exception
            MsgBox(ex.Message, MessageBoxButtons.OK + MessageBoxIcon.Error)
            PostingLainlain = False
        End Try
    End Function

    Public Shared Sub CekAntarKantor(ByVal IDJurnal As Long, ByVal Keterangan As String)
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
        ds = ExecuteDataset("AntarKantor", "Select IDDepartemen,sum(convert(Decimal(19,2),Debet)-convert(Decimal(19,2),Kredit)) as selisih  From MJurnalD Where IDJurnal=" & IDJurnal & " GROUP BY IDDepartemen hAVING sum(convert(Decimal(19,2),Debet)-convert(Decimal(19,2),Kredit))<>0")
        If ds.Tables("AntarKantor").Rows.Count > 0 Then
            For i = 0 To ds.Tables("AntarKantor").Rows.Count - 1
                Application.DoEvents()
                If NullToDbl(ds.Tables("AntarKantor").Rows(i).Item("Selisih")) > 0 Then

                    NilaiDetildef = IDJurnal & "," & _
                     NullToLong(ds.Tables("AntarKantor").Rows(i).Item("IDDepartemen")) & "," & _
                    defIDAkunAntarKantor & "," & _
                    defIDMataUang & "," & _
                    1 & "," & _
                    0 & "," & _
                    NullToLong(ds.Tables("AntarKantor").Rows(i).Item("Selisih")) & "," & _
                    0 & "," & _
                     NullToLong(ds.Tables("AntarKantor").Rows(i).Item("Selisih")) & "," & _
                    1 & "," & _
                   1 & ",'" & FixApostropi(Keterangan) & "'"
                Else
                    NilaiDetildef = IDJurnal & "," & _
                    NullToLong(ds.Tables("AntarKantor").Rows(i).Item("IDDepartemen")) & "," & _
                    defIDAkunAntarKantor & "," & _
                    defIDMataUang & "," & _
                    1 & "," & _
                    -1 * NullToLong(ds.Tables("AntarKantor").Rows(i).Item("Selisih")) & "," & _
                    0 & "," & _
                    -1 * NullToLong(ds.Tables("AntarKantor").Rows(i).Item("Selisih")) & "," & _
                    0 & "," & _
                    1 & "," & _
                    1 & ",'" & FixApostropi(Keterangan) & "'"
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