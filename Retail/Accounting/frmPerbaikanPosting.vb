Imports DevExpress.XtraEditors
Imports DevExpress.Utils
Imports System.Data
Imports VPoint.clsPostingKartuStok
Imports VPoint.clsPostingMutasiWilayah
Imports VPoint.clsPostingPembelian
Imports VPoint.clsPostingPenjualan

Public Class frmPerbaikanPosting
    Dim IsJalan As Boolean = False
    Private Sub frmPerbaikanPosting_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim ds As New DataSet
        Dim SQL As String
        Try
            TglDari.DateTime = TanggalSystem
            TglSampai.DateTime = TanggalSystem
            SQL = "SELECT NoID, Kode, Nama, Barcode FROM MBarang WHERE MBarang.IsActive=1"
            ds = ExecuteDataset("MBarang", SQL)
            If Not ds Is Nothing Then
                txtBarang.Properties.DataSource = ds.Tables("MBarang")
                txtBarang.Properties.ValueMember = "NoID"
                txtBarang.Properties.DisplayMember = "Kode"
            End If
            FungsiControl.SetForm(Me)
        Catch ex As Exception
            XtraMessageBox.Show("Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK)
        Finally
            ds.Dispose()
        End Try
    End Sub
    Private Sub cmdCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdCancel.Click
        If IsJalan Then
            IsJalan = False
        Else
            DialogResult = Windows.Forms.DialogResult.Cancel
            Close()
        End If
    End Sub
    Private Sub cmdOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdOK.Click
        If ckPostingUlang.Checked Then
            'PerbaikiKartuStockFull()
            If XtraMessageBox.Show("Ingin melakukan perevisian data?", NamaAplikasi, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = Windows.Forms.DialogResult.Yes Then
                PerbaikiHPPSaja()
            End If
        Else
            If XtraMessageBox.Show("Ingin melakukan perevisian data?", NamaAplikasi, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = Windows.Forms.DialogResult.Yes Then
                PerbaikiHPPSaja()
            End If
        End If
    End Sub
    Sub PerbaikiHPPSaja()
        Dim strSQL As String = ""
        Dim IDBarang As Integer
        Dim HPPSebelum As Double = 0
        Dim HPPAkhir As Double = 0
        Dim QtySebelum As Double = 0
        Dim QtyAkhir As Double = 0
        Dim QtyIn As Double = 0
        Dim QtyOut As Double = 0
        Dim Konversi As Double = 0
        Dim HargaNetto As Double = 0
        Dim ds As New DataSet
        Try
            ProgressBarControl1.Visible = True
            ProgressBarControl1.Position = 0
            If TglDari.Enabled Then
                strSQL = "SELECT MKartuStok.NoID, MKartuStok.IDTransaksiDetil, MKartuStok.IDTransaksi, MKartuStok.IDJenisTransaksi, MJenisTransaksi.Nama AS Transaksi, " & _
                         " MKartuStok.IDBarang,MKartuStok.Tanggal, MKartuStok.HargaNetto, " & _
                         " MKartuStok.QtyMasuk, MKartuStok.QtyKeluar, MKartuStok.Konversi, MKartuStok.QtyAkhir," & _
                         " MKartuStok.HPP " & _
                         " FROM MKartuStok " & _
                         " LEFT JOIN MBarang ON MBarang.NoID=MKartuStok.IDBarang " & _
                         " LEFT JOIN MSatuan ON MSatuan.NoID=MKartuStok.IDSatuan " & _
                         " LEFT JOIN MGudang ON MGudang.NoID=MKartuStok.IDGUdang " & _
                         " LEFT JOIN MJenisTransaksi ON MJenisTransaksi.ID=MKartuStok.IDJenisTransaksi " & _
                         " WHERE MKartuStok.Tanggal>='" & TglDari.DateTime.ToString("yyyy-MM-dd") & "' AND MKartuStok.Tanggal<'" & DateAdd(DateInterval.Day, 1, TglSampai.DateTime).ToString("yyyy-MM-dd") & "'"
            Else
                strSQL = "SELECT MKartuStok.NoID, MKartuStok.IDTransaksiDetil, MKartuStok.IDTransaksi, MKartuStok.IDJenisTransaksi, MJenisTransaksi.Nama AS Transaksi, " & _
                         " MKartuStok.IDBarang,MKartuStok.Tanggal, MKartuStok.HargaNetto, " & _
                         " MKartuStok.QtyMasuk, MKartuStok.QtyKeluar, MKartuStok.Konversi, MKartuStok.QtyAkhir," & _
                         " MKartuStok.HPP " & _
                         " FROM MKartuStok " & _
                         " LEFT JOIN MBarang ON MBarang.NoID=MKartuStok.IDBarang " & _
                         " LEFT JOIN MSatuan ON MSatuan.NoID=MKartuStok.IDSatuan " & _
                         " LEFT JOIN MGudang ON MGudang.NoID=MKartuStok.IDGUdang " & _
                         " LEFT JOIN MJenisTransaksi ON MJenisTransaksi.ID=MKartuStok.IDJenisTransaksi " & _
                         " WHERE 1=1 "
            End If
            If txtBarang.Text <> "" Then
                strSQL &= " AND MBarang.NoID=" & NullToLong(txtBarang.EditValue)
            End If
            strSQL &= " ORDER BY MKartuStok.IDBarang, DATEADD(dd, 0, DATEDIFF(dd, 0, MKartuStok.Tanggal)), MJenisTransaksi.NoUrut, MKartuStok.NoID "

            ds = modSqlServer.ExecuteDataset("Data", strSQL)
            If ds.Tables("Data").Rows.Count >= 1 Then
                IsJalan = True
                'modSqlServer.EksekusiSQL("DELETE FROM MKartuStok WHERE (IDJenisTransaksi=2 or IDJenisTransaksi=3 or IDJenisTransaksi=6) AND Tanggal>='" & txtTglDari.DateTime.ToString("yyyy/MM/dd") & "' AND Tanggal<'" & DateAdd(DateInterval.Day, 1, txtTglSampai.DateTime).ToString("yyyy/MM/dd") & "'")
                For i As Integer = 0 To ds.Tables("Data").Rows.Count - 1
                    If i = 0 Then
                        IDBarang = ds.Tables("Data").Rows(i).Item("IDBarang")
                        GetHPPBefore(IDBarang, TglDari.DateTime, QtySebelum, HPPSebelum)
                    End If
                    If IDbarang <> ds.Tables("Data").Rows(i).Item("IDBarang") Then 'Sudah ganti Barang lain
                        IDbarang = ds.Tables("Data").Rows(i).Item("IDBarang")
                        GetHPPBefore(IDbarang, TglDari.DateTime, QtySebelum, HPPSebelum)
                    End If
                    If IsJalan Then
                        TypeTransaksi = ds.Tables("Data").Rows(i).Item("IDJenisTransaksi")
                        QtyIn = NullToDbl(ds.Tables("Data").Rows(i).Item("QtyMasuk"))
                        QtyOut = NullToDbl(ds.Tables("Data").Rows(i).Item("QtyKeluar"))
                        HargaNetto = NullToDbl(ds.Tables("Data").Rows(i).Item("HargaNetto"))
                        Konversi = NullToDbl(ds.Tables("Data").Rows(i).Item("Konversi"))
                        If HargaNetto = 0 Then
                            HargaNetto = GetHargaNetto(NullToLong(ds.Tables("Data").Rows(i).Item("IDBarang")), NullToDate(ds.Tables("Data").Rows(i).Item("Tanggal")))
                            HargaNetto = HargaNetto * Konversi
                        End If
                        'TypeTransaksi = ds.Tables("Data").Rows(i).Item("IDJenisTransaksi")
                        Select Case TypeTransaksi
                            Case cTypetransaksi.RubahHPP
                                QtyAkhir = QtySebelum
                                If QtyAkhir < 0 Then
                                    HPPAkhir = HPPSebelum
                                Else
                                    HPPAkhir = HargaNetto / Konversi
                                End If
                                EksekusiSQL("UPDATE MRubahHPPD SET HargaPokok=" & FixKoma(HPPAkhir) & " WHERE NoID=" & NullToLong(ds.Tables("Data").Rows(i).Item("IDTransaksiDetil")))
                            Case cTypetransaksi.SaldoAwal
                                QtyAkhir = QtySebelum + QtyIn * Konversi
                                If QtyAkhir = 0 Then
                                    If Konversi > 0 Then
                                        HPPAkhir = HargaNetto / Konversi
                                    Else
                                        HPPAkhir = HargaNetto
                                    End If
                                Else
                                    HPPAkhir = ((HPPSebelum * QtySebelum) + (QtyIn * HargaNetto)) / QtyAkhir
                                End If
                            Case cTypetransaksi.PenyesuaianBarang
                                If QtyIn > 0 Then 'penyesuaian + seperti pembelian
                                    If HPPSebelum <= 0 Then
                                        HPPAkhir = GetHargaBeliNettoTerakhirPcs(IDBarang, NullToDate(ds.Tables("Data").Rows(i).Item("Tanggal")))
                                    End If
                                    QtyAkhir = QtySebelum + (QtyIn - QtyOut) * Konversi
                                    If QtyAkhir = 0 Then
                                        If Konversi > 0 Then
                                            HPPAkhir = HargaNetto / Konversi
                                        Else
                                            HPPAkhir = HargaNetto
                                        End If
                                    Else
                                        'HPPAkhir = ((HPPSebelum * QtySebelum) + (QtyIn * HargaNetto)) / QtyAkhir
                                        HPPAkhir = HPPSebelum
                                    End If
                                Else 'seperti penjualan
                                    QtyAkhir = QtySebelum - QtyOut * Konversi
                                    If QtyAkhir = 0 Then
                                        If Konversi > 0 Then
                                            HPPAkhir = HPPSebelum
                                        Else
                                            HPPAkhir = HPPSebelum
                                        End If
                                    Else
                                        HPPAkhir = HPPSebelum
                                    End If
                                    EksekusiSQL("UPDATE MKartuStok SET HargaNetto=" & FixKoma(HPPAkhir * Konversi) & " WHERE NoID=" & NullToLong(ds.Tables("Data").Rows(i).Item("NoID")))
                                End If
                            Case cTypetransaksi.Purchase
                                QtyAkhir = QtySebelum + QtyIn * Konversi
                                If QtyAkhir = 0 Then
                                    If Konversi > 0 Then
                                        HPPAkhir = HargaNetto / Konversi
                                    Else
                                        HPPAkhir = HargaNetto
                                    End If
                                Else
                                    HPPAkhir = ((HPPSebelum * QtySebelum) + (QtyIn * HargaNetto)) / QtyAkhir
                                End If
                            Case cTypetransaksi.PurchaseReturn
                                QtyAkhir = QtySebelum + (QtyIn - QtyOut) * Konversi
                                If QtyAkhir = 0 Then
                                    If Konversi > 0 Then
                                        HPPAkhir = HPPSebelum
                                    Else
                                        HPPAkhir = HPPSebelum
                                    End If
                                Else
                                    HPPAkhir = ((HPPSebelum * QtySebelum) - (QtyOut * HargaNetto)) / QtyAkhir
                                End If
                            Case cTypetransaksi.MutasiKeluar
                                QtyAkhir = QtySebelum - QtyOut * Konversi
                                HPPAkhir = HPPSebelum
                            Case cTypetransaksi.MutasiMasuk
                                QtyAkhir = QtySebelum + QtyIn * Konversi
                                HPPAkhir = HPPSebelum
                            Case cTypetransaksi.Sales
                                QtyAkhir = QtySebelum - QtyOut * Konversi
                                If QtyAkhir = 0 Then
                                    If Konversi > 0 Then
                                        HPPAkhir = HPPSebelum
                                    Else
                                        HPPAkhir = HPPSebelum
                                    End If
                                Else
                                    HPPAkhir = HPPSebelum '((HPPSebelum * QtySebelum) - (QtyOut * HargaNetto)) / QtyAkhir
                                End If

                            Case cTypetransaksi.SalesReturn
                                QtyAkhir = QtySebelum - QtyOut * Konversi 'Seperti Mutasi Masuk
                                HPPAkhir = HPPSebelum

                                'If QtyAkhir = 0 Then
                                '    If Konversi > 0 Then
                                '        HPPAkhir = HargaNetto / Konversi
                                '    Else
                                '        HPPAkhir = HargaNetto
                                '    End If
                                'Else
                                '    HPPAkhir = HPPSebelum ' ((HPPSebelum * QtySebelum) + (QtyIn * HargaNetto)) / QtyAkhir
                                'End If
                            Case cTypetransaksi.StokOpname
                                QtyAkhir = QtySebelum + (QtyIn - QtyOut) * Konversi
                                HPPAkhir = HPPSebelum
                            Case cTypetransaksi.RubahHargaBeli
                                If QtyOut > 0 Then
                                    Dim QtyDiTransaksiPcs As Double = 0
                                    QtyDiTransaksiPcs = NullToDbl(EksekusiSQlSkalarNew("SELECT SUM(MRevisiHargaBeliD.Qty*MRevisiHargaBeliD.Konversi) FROM MRevisiHargaBeliD WHERE MRevisiHargaBeliD.NoID=" & NullToLong(ds.Tables("Data").Rows(i).Item("IDTransaksiDetil"))))
                                    If QtySebelum < QtyDiTransaksiPcs Then
                                        QtyDiTransaksiPcs = QtySebelum
                                    End If
                                    EksekusiSQL("UPDATE MRevisiHargaBeliD SET QtyHPPPcs=" & FixKoma(QtyDiTransaksiPcs) & ", HPP=" & FixKoma(HPPSebelum) & " WHERE NoID=" & NullToLong(ds.Tables("Data").Rows(i).Item("IDTransaksiDetil")))
                                    EksekusiSQL("UPDATE MKartuStok SET QtyKeluar=" & FixKoma(QtyDiTransaksiPcs / Konversi) & ", QtyKeluarA=" & FixKoma(QtyDiTransaksiPcs) & " WHERE NoID=" & NullToLong(ds.Tables("Data").Rows(i).Item("NoID")))
                                    QtyAkhir = QtySebelum - QtyDiTransaksiPcs
                                    HPPAkhir = HPPSebelum
                                Else
                                    Dim QtyDiTransaksiPcs As Double = 0
                                    Dim HargaBaruPcs As Double = 0
                                    QtyDiTransaksiPcs = NullToDbl(EksekusiSQlSkalarNew("SELECT SUM(MRevisiHargaBeliD.QtyHPPPcs) FROM MRevisiHargaBeliD WHERE MRevisiHargaBeliD.NoID=" & NullToLong(ds.Tables("Data").Rows(i).Item("IDTransaksiDetil"))))
                                    HargaBaruPcs = NullToDbl(EksekusiSQlSkalarNew("SELECT SUM(MRevisiHargaBeliD.HargaBaru/MRevisiHargaBeliD.Konversi) FROM MRevisiHargaBeliD WHERE MRevisiHargaBeliD.NoID=" & NullToLong(ds.Tables("Data").Rows(i).Item("IDTransaksiDetil"))))

                                    'EksekusiSQL("UPDATE MRevisiHargaBeliD SET QtyHPPPcs=" & FixKoma(QtyDiTransaksiPcs) & ", HPP=" & FixKoma(HPPAkhir) & " WHERE NoID=" & NullToLong(ds.Tables("Data").Rows(i).Item("IDTransaksiDetil")))
                                    EksekusiSQL("UPDATE MKartuStok SET QtyMasuk=" & FixKoma(QtyDiTransaksiPcs / Konversi) & ", QtyMasukA=" & FixKoma(QtyDiTransaksiPcs) & ", HargaNetto=" & FixKoma(HargaBaruPcs * Konversi) & " WHERE NoID=" & NullToLong(ds.Tables("Data").Rows(i).Item("NoID")))

                                    ds.Tables("Data").Rows(i).Item("HargaNetto") = HargaBaruPcs * Konversi

                                    QtyAkhir = QtySebelum + QtyDiTransaksiPcs

                                    If QtyAkhir = 0 Then
                                        HPPAkhir = HargaBaruPcs
                                    Else
                                        HPPAkhir = ((HPPSebelum * QtySebelum) + (QtyDiTransaksiPcs * HargaBaruPcs)) / QtyAkhir
                                    End If

                                End If
                            Case cTypetransaksi.TransferKode

                                If QtyOut > 0 Then
                                    Dim QtyDiTransaksiPcs As Double = 0
                                    QtyDiTransaksiPcs = NullToDbl(EksekusiSQlSkalarNew("SELECT SUM(MTransferKodeD.QtyLama*MTransferKodeD.KonversiLama) FROM MTransferKodeD WHERE MTransferKodeD.NoID=" & NullToLong(ds.Tables("Data").Rows(i).Item("IDTransaksiDetil"))))
                                    If QtySebelum < QtyDiTransaksiPcs Then
                                        QtyDiTransaksiPcs = QtySebelum
                                    End If
                                    EksekusiSQL("UPDATE MTransferKodeD SET QtyHPPPcs=" & FixKoma(QtyDiTransaksiPcs) & ", HPP=" & FixKoma(HPPSebelum) & " WHERE NoID=" & NullToLong(ds.Tables("Data").Rows(i).Item("IDTransaksiDetil")))
                                    EksekusiSQL("UPDATE MKartuStok SET QtyKeluar=" & FixKoma(QtyDiTransaksiPcs / Konversi) & ", QtyKeluarA=" & FixKoma(QtyDiTransaksiPcs) & " WHERE NoID=" & NullToLong(ds.Tables("Data").Rows(i).Item("NoID")))

                                    QtyAkhir = QtySebelum - QtyDiTransaksiPcs
                                    HPPAkhir = HPPSebelum
                                Else
                                    Dim QtyDiTransaksiPcs As Double = 0
                                    Dim HargaBaruPcs As Double = 0
                                    QtyDiTransaksiPcs = NullToDbl(EksekusiSQlSkalarNew("SELECT SUM(MTransferKodeD.QtyHPPPcs) FROM MTransferKodeD WHERE MTransferKodeD.NoID=" & NullToLong(ds.Tables("Data").Rows(i).Item("IDTransaksiDetil"))))
                                    HargaBaruPcs = NullToDbl(EksekusiSQlSkalarNew("SELECT SUM(MTransferKodeD.HPP) FROM MTransferKodeD WHERE MTransferKodeD.NoID=" & NullToLong(ds.Tables("Data").Rows(i).Item("IDTransaksiDetil"))))

                                    'EksekusiSQL("UPDATE MRevisiHargaBeliD SET QtyHPPPcs=" & FixKoma(QtyDiTransaksiPcs) & ", HPP=" & FixKoma(HPPAkhir) & " WHERE NoID=" & NullToLong(ds.Tables("Data").Rows(i).Item("IDTransaksiDetil")))
                                    EksekusiSQL("UPDATE MKartuStok SET QtyMasuk=" & FixKoma(QtyDiTransaksiPcs / Konversi) & ", QtyMasukA=" & FixKoma(QtyDiTransaksiPcs) & ", HargaNetto=" & FixKoma(HargaBaruPcs * Konversi) & " WHERE NoID=" & NullToLong(ds.Tables("Data").Rows(i).Item("NoID")))

                                    ds.Tables("Data").Rows(i).Item("HargaNetto") = HargaBaruPcs * Konversi

                                    QtyAkhir = QtySebelum + QtyDiTransaksiPcs
                                    If QtyAkhir = 0 Then
                                        HPPAkhir = HargaBaruPcs
                                    Else
                                        HPPAkhir = ((HPPSebelum * QtySebelum) + (QtyDiTransaksiPcs * HargaBaruPcs)) / QtyAkhir
                                    End If
                                End If
                            Case cTypetransaksi.RubahHargaJual
                                QtyAkhir = QtySebelum + (QtyIn - QtyOut) * Konversi
                                HPPAkhir = HPPSebelum
                            Case cTypetransaksi.PemakaianBarang
                                QtyAkhir = QtySebelum - QtyOut * Konversi
                                HPPAkhir = HPPSebelum
                        End Select

                        'Update ke KartuStok
                        If IsDBNull(ds.Tables("Data").Rows(i).Item("HargaNetto")) Or (NullToDbl(ds.Tables("Data").Rows(i).Item("HargaNetto")) = 0 And HargaNetto <> 0) Then
                            EksekusiSQL("UPDATE MKartuStok SET HargaNetto=" & FixKoma(Bulatkan(HargaNetto)) & ",HPP=" & FixKoma(Bulatkan(HPPAkhir)) & ",QtyAkhir=" & FixKoma(QtyAkhir) & _
                                         " WHERE NoID=" & NullToLong(ds.Tables("Data").Rows(i).Item("NoID")))
                        Else
                            EksekusiSQL("UPDATE MKartuStok SET HPP=" & FixKoma(Bulatkan(HPPAkhir)) & ",QtyAkhir=" & FixKoma(QtyAkhir) & _
                                        " WHERE NoID=" & NullToLong(ds.Tables("Data").Rows(i).Item("NoID")))
                        End If

                        'Updatekan ke Transaksi
                        If TypeTransaksi = cTypetransaksi.Sales Then
                            EksekusiSQL("UPDATE MJualD SET HargaPokok=" & FixKoma(Bulatkan(HPPAkhir)) & _
                                        " WHERE NoID=" & NullToLong(ds.Tables("Data").Rows(i).Item("IDTransaksiDetil")))
                        ElseIf TypeTransaksi = cTypetransaksi.PemakaianBarang Then
                            EksekusiSQL("UPDATE MPemakaianD SET HargaPokok=" & FixKoma(Bulatkan(HPPAkhir)) & _
                                        " WHERE NoID=" & NullToLong(ds.Tables("Data").Rows(i).Item("IDTransaksiDetil")))
                        ElseIf TypeTransaksi = cTypetransaksi.StokOpname Then
                            EksekusiSQL("UPDATE MStockOpnameD SET HargaPokok=" & FixKoma(Bulatkan(HPPAkhir)) & _
                                        " WHERE NoID=" & NullToLong(ds.Tables("Data").Rows(i).Item("IDTransaksiDetil")))
                        ElseIf TypeTransaksi = cTypetransaksi.SalesReturn Then
                            EksekusiSQL("UPDATE MReturJualD SET HargaPokok=" & FixKoma(Bulatkan(HPPAkhir)) & _
                                        " WHERE NoID=" & NullToLong(ds.Tables("Data").Rows(i).Item("IDTransaksiDetil")))
                        ElseIf TypeTransaksi = cTypetransaksi.PenyesuaianBarang Then
                            If QtyIn > 0 Then
                                EksekusiSQL("UPDATE MPenyesuaianD SET HargaPokok=" & FixKoma(HargaNetto) & _
                                            " WHERE NoID=" & NullToLong(ds.Tables("Data").Rows(i).Item("IDTransaksiDetil")))
                            Else
                                EksekusiSQL("UPDATE MPenyesuaianD SET HargaPokok=" & FixKoma(Bulatkan(HPPAkhir)) & _
                                            " WHERE NoID=" & NullToLong(ds.Tables("Data").Rows(i).Item("IDTransaksiDetil")))
                            End If
                        ElseIf TypeTransaksi = cTypetransaksi.SalesReturn Then
                            EksekusiSQL("UPDATE MReturJualD SET HargaPokok=" & FixKoma(Bulatkan(HPPAkhir)) & _
                                        " WHERE NoID=" & NullToLong(ds.Tables("Data").Rows(i).Item("IDTransaksiDetil")))
                        End If
                        QtySebelum = QtyAkhir
                        HPPSebelum = HPPAkhir
                        If CheckEdit1.Checked Then 'Jika dicentang updatekan ke MBarang
                            EksekusiSQL("UPDATE MBarang SET HPP=" & FixKoma(Bulatkan(HPPAkhir)) & _
                                        " WHERE NoID=" & NullToLong(ds.Tables("Data").Rows(i).Item("IDBarang")))
                        End If
                        'End If
                        ProgressBarControl1.Position = CDbl(i * 100 \ ds.Tables("Data").Rows.Count)
                        'ProgressBarControl1.Text = CStr(CDbl(i * 100 \ ds.Tables("Data").Rows.Count).ToString("###,###,###,###,##,###,##0") & "  of  " & (ds.Tables("Data").Rows.Count).ToString("###,###,###,###,##,###,##0") & " successes.")
                        ProgressBarControl1.Refresh()
                        Application.DoEvents()
                    Else
                        Exit For
                    End If
                Next
            End If
        Catch ex As Exception
            XtraMessageBox.Show("Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK)
        Finally
            IsJalan = False
            ds.Dispose()
            ProgressBarControl1.Visible = False
        End Try
    End Sub
    'Sub PerbaikiKartuStockFull()
    '    Dim strSQL As String = ""
    '    Dim ds As New DataSet
    '    Try
    '        ProgressBarControl1.Visible = True
    '        ProgressBarControl1.Position = 0
    '        If TglDari.Enabled Then
    '            strSQL = "SELECT MKartuStok.Kode, CONVERT(Date, MKartuStok.Tanggal,101) AS Tanggal, MKartuStok.IDTransaksi, MKartuStok.IDJenisTransaksi, MJenisTransaksi.Nama AS Transaksi " & _
    '                     " FROM MKartuStok " & _
    '                     " LEFT JOIN MJenisTransaksi ON MJenisTransaksi.ID=MKartuStok.IDJenisTransaksi " & vbCrLf & _
    '                     " WHERE MKartuStok.Tanggal>='" & TglDari.DateTime.ToString("yyyy-MM-dd") & "' AND MKartuStok.Tanggal<'" & DateAdd(DateInterval.Day, 1, TglSampai.DateTime).ToString("yyyy-MM-dd") & "' " & IIf(txtBarang.Text <> "", " AND MKartuStok.IDBarang=" & NullToLong(txtBarang.EditValue), "") & vbCrLf & _
    '                     " GROUP BY MKartuStok.Kode, CONVERT(Date, MKartuStok.Tanggal,101), MKartuStok.IDTransaksi, MKartuStok.IDJenisTransaksi, MJenisTransaksi.Nama, MJenisTransaksi.NoUrut "
    '        Else
    '            strSQL = "SELECT MKartuStok.Kode, CONVERT(Date, MKartuStok.Tanggal,101) AS Tanggal, MKartuStok.IDTransaksi, MKartuStok.IDJenisTransaksi, MJenisTransaksi.Nama AS Transaksi " & _
    '                     " FROM MKartuStok " & _
    '                     " LEFT JOIN MJenisTransaksi ON MJenisTransaksi.ID=MKartuStok.IDJenisTransaksi " & _
    '                     " WHERE 1=1 " & IIf(txtBarang.Text <> "", " AND MKartuStok.IDBarang=" & NullToLong(txtBarang.EditValue), "") & vbCrLf & _
    '                     " GROUP BY MKartuStok.Kode, CONVERT(Date, MKartuStok.Tanggal,101), MKartuStok.IDTransaksi, MKartuStok.IDJenisTransaksi, MJenisTransaksi.Nama, MJenisTransaksi.NoUrut "
    '        End If
    '        strSQL &= " ORDER BY CONVERT(Date, MKartuStok.Tanggal,101) DESC, MJenisTransaksi.NoUrut DESC, MKartuStok.IDTransaksi DESC"
    '        ds = modSqlServer.ExecuteDataset("Data", strSQL)
    '        If ds.Tables("Data").Rows.Count >= 1 Then
    '            IsJalan = True
    '            For i As Integer = 0 To ds.Tables("Data").Rows.Count - 1
    '                If IsJalan Then
    '                    TypeTransaksi = NullToLong(ds.Tables("Data").Rows(i).Item("IDJenisTransaksi"))
    '                    Select Case TypeTransaksi
    '                        Case cTypetransaksi.SaldoAwal
    '                            UnPostingSaldoAwalPersediaan(NullToLong(ds.Tables("Data").Rows(i).Item("IDTransaksi")), True)
    '                        Case cTypetransaksi.PenyesuaianBarang
    '                            UnPostingPenyesuaian(NullToLong(ds.Tables("Data").Rows(i).Item("IDTransaksi")), True)
    '                        Case cTypetransaksi.Purchase
    '                            UnPostingStokBarangPembelian(NullToLong(ds.Tables("Data").Rows(i).Item("IDTransaksi")), True)
    '                        Case cTypetransaksi.PurchaseReturn
    '                            UnPostingStokBarangReturPembelian(NullToLong(ds.Tables("Data").Rows(i).Item("IDTransaksi")), True)
    '                        Case cTypetransaksi.MutasiKeluar
    '                            UnPostingStokBarangMutasiGudang(NullToLong(ds.Tables("Data").Rows(i).Item("IDTransaksi")), True)
    '                        Case cTypetransaksi.MutasiMasuk
    '                            UnPostingStokBarangMutasiGudang(NullToLong(ds.Tables("Data").Rows(i).Item("IDTransaksi")), True)
    '                        Case cTypetransaksi.Sales
    '                            UnPostingStokBarangPenjualan(NullToLong(ds.Tables("Data").Rows(i).Item("IDTransaksi")), True)
    '                        Case cTypetransaksi.SalesReturn
    '                            UnPostingStokBarangReturPenjualan(NullToLong(ds.Tables("Data").Rows(i).Item("IDTransaksi")), True)
    '                        Case cTypetransaksi.StokOpname
    '                            UnPostingStockOpname(NullToLong(ds.Tables("Data").Rows(i).Item("IDTransaksi")), True)
    '                        Case cTypetransaksi.RubahHargaBeli
    '                            UnPostingStokBarangRevisiHargaPembelian(NullToLong(ds.Tables("Data").Rows(i).Item("IDTransaksi")), True)
    '                        Case cTypetransaksi.RubahHargaJual
    '                            UnPostingStokBarangRevisiHargaPenjualan(NullToLong(ds.Tables("Data").Rows(i).Item("IDTransaksi")), True)
    '                        Case cTypetransaksi.PemakaianBarang
    '                            UnPostingPemakaian(NullToLong(ds.Tables("Data").Rows(i).Item("IDTransaksi")), True)
    '                        Case cTypetransaksi.TransferKode
    '                            UnPostingTransferKode(NullToLong(ds.Tables("Data").Rows(i).Item("IDTransaksi")), True)
    '                        Case cTypetransaksi.MutasiMasukIntransit
    '                            UnPostingStokBarangMutasiGudangIntransit(NullToLong(ds.Tables("Data").Rows(i).Item("IDTransaksi")), True)
    '                        Case cTypetransaksi.MutasiKeluarIntransit
    '                            UnPostingStokBarangMutasiGudangIntransit(NullToLong(ds.Tables("Data").Rows(i).Item("IDTransaksi")), True)
    '                    End Select
    '                    ProgressBarControl1.Position = CDbl(i * 100 \ ds.Tables("Data").Rows.Count)
    '                    ProgressBarControl1.Refresh()
    '                    Application.DoEvents()
    '                Else
    '                    Exit For
    '                End If
    '            Next

    '            strSQL = "SELECT MSaldoAwalPersediaan.NoID AS IDTransaksi, MSaldoAwalPersediaan.Tanggal, 1 AS IDJenisTransaksi " & vbCrLf & _
    '                     " FROM MSaldoAwalPersediaan " & vbCrLf & _
    '                     " INNER JOIN MSaldoAwalPersediaanD ON MSaldoAwalPersediaanD.IDHeader=MSaldoAwalPersediaan.NoID " & vbCrLf & _
    '                     " WHERE " & IIf(TglDari.Enabled, " MSaldoAwalPersediaan.Tanggal>='" & TglDari.DateTime.ToString("yyyy/MM/dd") & "' AND MSaldoAwalPersediaan.Tanggal<'" & TglSampai.DateTime.AddDays(1).ToString("yyyy/MM/dd") & "'", " 1=1 ") & " AND " & IIf(txtBarang.Text <> "", "MSaldoAwalPersediaanD.IDBarang=" & NullToLong(txtBarang.EditValue), " 1=1") & vbCrLf & _
    '                     " GROUP BY MSaldoAwalPersediaan.NoID, MSaldoAwalPersediaan.Tanggal " & vbCrLf & _
    '                     " UNION ALL  " & vbCrLf & _
    '                     " SELECT MPenyesuaian.NoID, MPenyesuaian.Tanggal, 14  " & vbCrLf & _
    '                     " FROM MPenyesuaian " & vbCrLf & _
    '                     " INNER JOIN MPenyesuaianD ON MPenyesuaianD.IDHeader=MPenyesuaian.NoID " & vbCrLf & _
    '                     " WHERE " & IIf(TglDari.Enabled, " MPenyesuaian.Tanggal>='" & TglDari.DateTime.ToString("yyyy/MM/dd") & "' AND MPenyesuaian.Tanggal<'" & TglSampai.DateTime.AddDays(1).ToString("yyyy/MM/dd") & "'", " 1=1 ") & " AND " & IIf(txtBarang.Text <> "", "MPenyesuaianD.IDBarang=" & NullToLong(txtBarang.EditValue), " 1=1") & vbCrLf & _
    '                     " GROUP BY MPenyesuaian.NoID, MPenyesuaian.Tanggal " & vbCrLf & _
    '                     " UNION ALL  " & vbCrLf & _
    '                     " SELECT MBeli.NoID, MBeli.Tanggal, 2  " & vbCrLf & _
    '                     " FROM MBeli  " & vbCrLf & _
    '                     " INNER JOIN MBeliD ON MBeliD.IDBeli=MBeli.NoID " & vbCrLf & _
    '                     " WHERE " & IIf(TglDari.Enabled, " MBeli.Tanggal>='" & TglDari.DateTime.ToString("yyyy/MM/dd") & "' AND MBeli.Tanggal<'" & TglSampai.DateTime.AddDays(1).ToString("yyyy/MM/dd") & "'", " 1=1 ") & " AND " & IIf(txtBarang.Text <> "", "MBeliD.IDBarang=" & NullToLong(txtBarang.EditValue), " 1=1") & vbCrLf & _
    '                     " GROUP BY MBeli.NoID, MBeli.Tanggal " & vbCrLf & _
    '                     " UNION ALL " & vbCrLf & _
    '                     " SELECT MReturBeli.NoID, MReturBeli.Tanggal, 3  " & vbCrLf & _
    '                     " FROM MReturBeli " & vbCrLf & _
    '                     " INNER JOIN MReturBeliD ON MReturBeliD.IDReturBeli=MReturBeli.NoID " & vbCrLf & _
    '                     " WHERE " & IIf(TglDari.Enabled, " MReturBeli.Tanggal>='" & TglDari.DateTime.ToString("yyyy/MM/dd") & "' AND MReturBeli.Tanggal<'" & TglSampai.DateTime.AddDays(1).ToString("yyyy/MM/dd") & "'", " 1=1 ") & " AND " & IIf(txtBarang.Text <> "", "MReturBeliD.IDBarang=" & NullToLong(txtBarang.EditValue), " 1=1") & vbCrLf & _
    '                     " GROUP BY MReturBeli.NoID, MReturBeli.Tanggal " & vbCrLf & _
    '                     " UNION ALL " & vbCrLf & _
    '                     " SELECT MMutasiGudang.NoID, MMutasiGudang.Tanggal, 4 " & vbCrLf & _
    '                     " FROM MMutasiGudang " & vbCrLf & _
    '                     " INNER JOIN MMutasiGudangD ON MMutasiGudangD.IDMutasiGudang=MMutasiGudang.NoID " & vbCrLf & _
    '                     " WHERE " & IIf(TglDari.Enabled, " MMutasiGudang.Tanggal>='" & TglDari.DateTime.ToString("yyyy/MM/dd") & "' AND MMutasiGudang.Tanggal<'" & TglSampai.DateTime.AddDays(1).ToString("yyyy/MM/dd") & "'", " 1=1 ") & " AND " & IIf(txtBarang.Text <> "", "MMutasiGudangD.IDBarang=" & NullToLong(txtBarang.EditValue), " 1=1") & vbCrLf & _
    '                     " GROUP BY MMutasiGudang.NoID, MMutasiGudang.Tanggal " & vbCrLf & _
    '                     " UNION ALL " & vbCrLf & _
    '                     " SELECT MJual.NoID, MJual.Tanggal, 6 " & vbCrLf & _
    '                     " FROM MJual " & vbCrLf & _
    '                     " INNER JOIN MJualD ON MJualD.IDJual=MJual.NoID " & vbCrLf & _
    '                     " WHERE " & IIf(TglDari.Enabled, " MJual.Tanggal>='" & TglDari.DateTime.ToString("yyyy/MM/dd") & "' AND MJual.Tanggal<'" & TglSampai.DateTime.AddDays(1).ToString("yyyy/MM/dd") & "'", " 1=1 ") & " AND " & IIf(txtBarang.Text <> "", "MJualD.IDBarang=" & NullToLong(txtBarang.EditValue), " 1=1") & vbCrLf & _
    '                     " GROUP BY MJual.NoID, MJual.Tanggal " & vbCrLf & _
    '                     " UNION ALL " & vbCrLf & _
    '                     " SELECT MReturJual.NoID, MReturJual.Tanggal, 7 " & vbCrLf & _
    '                     " FROM MReturJual " & vbCrLf & _
    '                     " INNER JOIN MReturJualD ON MReturJualD.IDReturJual=MReturJual.NoID " & vbCrLf & _
    '                     " WHERE " & IIf(TglDari.Enabled, " MReturJual.Tanggal>='" & TglDari.DateTime.ToString("yyyy/MM/dd") & "' AND MReturJual.Tanggal<'" & TglSampai.DateTime.AddDays(1).ToString("yyyy/MM/dd") & "'", " 1=1 ") & " AND " & IIf(txtBarang.Text <> "", "MReturJualD.IDBarang=" & NullToLong(txtBarang.EditValue), " 1=1") & vbCrLf & _
    '                     " GROUP BY MReturJual.NoID, MReturJual.Tanggal " & vbCrLf & _
    '                     " UNION ALL " & vbCrLf & _
    '                     " SELECT MStockOpname.NoID, MStockOpname.Tanggal, 9 " & vbCrLf & _
    '                     " FROM MStockOpname " & vbCrLf & _
    '                     " INNER JOIN MStockOpnameD ON MStockOpnameD.IDHeader=MStockOpname.NoID " & vbCrLf & _
    '                     " WHERE " & IIf(TglDari.Enabled, " MStockOpname.Tanggal>='" & TglDari.DateTime.ToString("yyyy/MM/dd") & "' AND MStockOpname.Tanggal<'" & TglSampai.DateTime.AddDays(1).ToString("yyyy/MM/dd") & "'", " 1=1 ") & " AND " & IIf(txtBarang.Text <> "", "MStockOpnameD.IDBarang=" & NullToLong(txtBarang.EditValue), " 1=1") & vbCrLf & _
    '                     " GROUP BY MStockOpname.NoID, MStockOpname.Tanggal " & vbCrLf & _
    '                     " UNION ALL " & vbCrLf & _
    '                     " SELECT MRevisiHargaBeli.NoID, MRevisiHargaBeli.Tanggal, 11 " & vbCrLf & _
    '                     " FROM MRevisiHargaBeli " & vbCrLf & _
    '                     " INNER JOIN MRevisiHargaBeliD ON MRevisiHargaBeliD.IDRevisiHargaBeli=MRevisiHargaBeli.NoID " & vbCrLf & _
    '                     " WHERE " & IIf(TglDari.Enabled, " MRevisiHargaBeli.Tanggal>='" & TglDari.DateTime.ToString("yyyy/MM/dd") & "' AND MRevisiHargaBeli.Tanggal<'" & TglSampai.DateTime.AddDays(1).ToString("yyyy/MM/dd") & "'", " 1=1 ") & " AND " & IIf(txtBarang.Text <> "", "MRevisiHargaBeliD.IDBarang=" & NullToLong(txtBarang.EditValue), " 1=1") & vbCrLf & _
    '                     " GROUP BY MRevisiHargaBeli.NoID, MRevisiHargaBeli.Tanggal " & vbCrLf & _
    '                     " UNION ALL " & vbCrLf & _
    '                     " SELECT MRevisiHargaJual.NoID, MRevisiHargaJual.Tanggal, 15 " & vbCrLf & _
    '                     " FROM MRevisiHargaJual " & vbCrLf & _
    '                     " INNER JOIN MRevisiHargaJualD ON MRevisiHargaJualD.IDRevisiHargaJual=MRevisiHargaJual.NoID " & vbCrLf & _
    '                     " WHERE " & IIf(TglDari.Enabled, " MRevisiHargaJual.Tanggal>='" & TglDari.DateTime.ToString("yyyy/MM/dd") & "' AND MRevisiHargaJual.Tanggal<'" & TglSampai.DateTime.AddDays(1).ToString("yyyy/MM/dd") & "'", " 1=1 ") & " AND " & IIf(txtBarang.Text <> "", "MRevisiHargaJualD.IDBarang=" & NullToLong(txtBarang.EditValue), " 1=1") & vbCrLf & _
    '                     " GROUP BY MRevisiHargaJual.NoID, MRevisiHargaJual.Tanggal " & vbCrLf & _
    '                     " UNION ALL " & vbCrLf & _
    '                     " SELECT MPemakaian.NoID, MPemakaian.Tanggal, 8 " & vbCrLf & _
    '                     " FROM MPemakaian " & vbCrLf & _
    '                     " INNER JOIN MPemakaianD ON MPemakaianD.IDHeader=MPemakaian.NoID " & vbCrLf & _
    '                     " WHERE " & IIf(TglDari.Enabled, " MPemakaian.Tanggal>='" & TglDari.DateTime.ToString("yyyy/MM/dd") & "' AND MPemakaian.Tanggal<'" & TglSampai.DateTime.AddDays(1).ToString("yyyy/MM/dd") & "'", " 1=1 ") & " AND " & IIf(txtBarang.Text <> "", "MPemakaianD.IDBarang=" & NullToLong(txtBarang.EditValue), " 1=1") & vbCrLf & _
    '                     " GROUP BY MPemakaian.NoID, MPemakaian.Tanggal " & vbCrLf & _
    '                     " UNION ALL " & vbCrLf & _
    '                     " SELECT MTransferKode.NoID, MTransferKode.Tanggal, 16 " & vbCrLf & _
    '                     " FROM MTransferKode " & vbCrLf & _
    '                     " INNER JOIN MTransferKodeD ON MTransferKodeD.IDHeader=MTransferKode.NoID " & vbCrLf & _
    '                     " WHERE " & IIf(TglDari.Enabled, " MTransferKode.Tanggal>='" & TglDari.DateTime.ToString("yyyy/MM/dd") & "' AND MTransferKode.Tanggal<'" & TglSampai.DateTime.AddDays(1).ToString("yyyy/MM/dd") & "'", " 1=1 ") & " AND " & IIf(txtBarang.Text <> "", "(MTransferKodeD.IDBarangBaru=" & NullToLong(txtBarang.EditValue) & " OR MTransferKodeD.IDBarangLama=" & NullToLong(txtBarang.EditValue) & ")", " 1=1") & vbCrLf & _
    '                     " GROUP BY MTransferKode.NoID, MTransferKode.Tanggal " & vbCrLf & _
    '                     " UNION ALL " & vbCrLf & _
    '                     " SELECT MMutasiGudangIntransit.NoID, MMutasiGudangIntransit.Tanggal, 25 " & vbCrLf & _
    '                     " FROM MMutasiGudangIntransit " & vbCrLf & _
    '                     " INNER JOIN MMutasiGudangIntransitD ON MMutasiGudangIntransitD.IDMutasiGudangIntransit=MMutasiGudangIntransit.NoID " & vbCrLf & _
    '                     " WHERE " & IIf(TglDari.Enabled, " MMutasiGudangIntransit.Tanggal>='" & TglDari.DateTime.ToString("yyyy/MM/dd") & "' AND MMutasiGudangIntransit.Tanggal<'" & TglSampai.DateTime.AddDays(1).ToString("yyyy/MM/dd") & "'", " 1=1 ") & " AND " & IIf(txtBarang.Text <> "", "MMutasiGudangIntransitD.IDBarang=" & NullToLong(txtBarang.EditValue), " 1=1") & vbCrLf & _
    '                     " GROUP BY MMutasiGudangIntransit.NoID, MMutasiGudangIntransit.Tanggal"
    '            strSQL = "SELECT X.IDTransaksi, X.IDJenisTransaksi, CONVERT(DATE,X.Tanggal,101) AS Tanggal " & vbCrLf & _
    '                     " FROM (" & strSQL & ") X INNER JOIN MJenisTransaksi ON MJenisTransaksi.ID=X.IDJenisTransaksi " & vbCrLf & _
    '                     " GROUP BY X.IDTransaksi, CONVERT(DATE,X.Tanggal,101), X.IDJenisTransaksi, MJenisTransaksi.NoUrut " & vbCrLf & _
    '                     " ORDER BY CONVERT(DATE,X.Tanggal,101), MJenisTransaksi.NoUrut, X.IDTransaksi"
    '            ds = ExecuteDataset("Data", strSQL)
    '            If IsJalan Then
    '                For i As Integer = 0 To ds.Tables("Data").Rows.Count - 1
    '                    If IsJalan Then
    '                        TypeTransaksi = NullToLong(ds.Tables("Data").Rows(i).Item("IDJenisTransaksi"))
    '                        Select Case TypeTransaksi
    '                            Case cTypetransaksi.SaldoAwal
    '                                PostingSaldoAwalPersediaan(NullToLong(ds.Tables("Data").Rows(i).Item("IDTransaksi")))
    '                            Case cTypetransaksi.PenyesuaianBarang
    '                                PostingPenyesuaian(NullToLong(ds.Tables("Data").Rows(i).Item("IDTransaksi")))
    '                            Case cTypetransaksi.Purchase
    '                                PostingStokBarangPembelian(NullToLong(ds.Tables("Data").Rows(i).Item("IDTransaksi")))
    '                            Case cTypetransaksi.PurchaseReturn
    '                                PostingStokBarangReturPembelian(NullToLong(ds.Tables("Data").Rows(i).Item("IDTransaksi")))
    '                            Case cTypetransaksi.MutasiKeluar
    '                                PostingStokBarangMutasiGudang(NullToLong(ds.Tables("Data").Rows(i).Item("IDTransaksi")))
    '                            Case cTypetransaksi.MutasiMasuk
    '                                PostingStokBarangMutasiGudang(NullToLong(ds.Tables("Data").Rows(i).Item("IDTransaksi")))
    '                            Case cTypetransaksi.Sales
    '                                If NullToBool(EksekusiSQlSkalarNew("SELECT MJual.IsTanpaBarang FROM MJual WHERE NoID=" & NullToLong(ds.Tables("Data").Rows(i).Item("IDTransaksi")))) Then
    '                                    PostingStokBarangPenjualanTanpaBarang(NullToLong(ds.Tables("Data").Rows(i).Item("IDTransaksi")))
    '                                ElseIf NullToBool(EksekusiSQlSkalarNew("SELECT MJual.IsPOS FROM MJual WHERE NoID=" & NullToLong(ds.Tables("Data").Rows(i).Item("IDTransaksi")))) Then
    '                                    PostingStokBarangPenjualan(NullToLong(ds.Tables("Data").Rows(i).Item("IDTransaksi")), False)
    '                                Else
    '                                    PostingStokBarangPenjualan(NullToLong(ds.Tables("Data").Rows(i).Item("IDTransaksi")), True)
    '                                End If
    '                            Case cTypetransaksi.SalesReturn
    '                                PostingStokBarangReturPenjualan(NullToLong(ds.Tables("Data").Rows(i).Item("IDTransaksi")))
    '                            Case cTypetransaksi.StokOpname
    '                                PostingStockOpname(NullToLong(ds.Tables("Data").Rows(i).Item("IDTransaksi")))
    '                            Case cTypetransaksi.RubahHargaBeli
    '                                PostingStokBarangRevisiHargaPembelian(NullToLong(ds.Tables("Data").Rows(i).Item("IDTransaksi")))
    '                            Case cTypetransaksi.RubahHargaJual
    '                                PostingStokBarangRevisiHargaPenjualan(NullToLong(ds.Tables("Data").Rows(i).Item("IDTransaksi")))
    '                            Case cTypetransaksi.PemakaianBarang
    '                                PostingPemakaian(NullToLong(ds.Tables("Data").Rows(i).Item("IDTransaksi")))
    '                            Case cTypetransaksi.TransferKode
    '                                PostingTransferKode(NullToLong(ds.Tables("Data").Rows(i).Item("IDTransaksi")))
    '                            Case cTypetransaksi.MutasiMasukIntransit
    '                                PostingStokBarangMutasiGudangIntransit(NullToLong(ds.Tables("Data").Rows(i).Item("IDTransaksi")))
    '                            Case cTypetransaksi.MutasiKeluarIntransit
    '                                PostingStokBarangMutasiGudangIntransit(NullToLong(ds.Tables("Data").Rows(i).Item("IDTransaksi")))
    '                        End Select
    '                        ProgressBarControl1.Position = CDbl(i * 100 \ ds.Tables("Data").Rows.Count)
    '                        ProgressBarControl1.Refresh()
    '                        Application.DoEvents()
    '                    Else
    '                        Exit For
    '                    End If
    '                Next
    '            End If
    '        End If
    '    Catch ex As Exception
    '        XtraMessageBox.Show("Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK)
    '    Finally
    '        IsJalan = False
    '        ds.Dispose()
    '        ProgressBarControl1.Visible = False
    '    End Try
    'End Sub
    Sub GetHPPBefore(ByVal IDbarang As Integer, ByVal TglDari As Date, ByRef QtySebelum As Double, ByRef HPPSebelum As Double)
        Dim strsql As String
        Dim dst As New DataSet
        strsql = "SELECT Top 1 MKartuStok.NoID, MJenisTransaksi.ID, MJenisTransaksi.Nama AS Transaksi, MKartuStok.Tanggal, MKartuStok.Kode, MBarang.Kode AS KodeStock, MBarang.Nama AS NamaStock, MGudang.Kode AS Gudang, MKartuStok.HargaNetto, " & _
                 " MKartuStok.QtyMasuk, MKartuStok.QtyKeluar, MSatuan.Kode AS Satuan, MKartuStok.Konversi, MKartuStok.QtyAkhir, MKartuStok.HPP, MKartuStok.Keterangan " & _
                 " FROM MKartuStok " & _
                 " LEFT JOIN MBarang ON MBarang.NoID=MKartuStok.IDBarang " & _
                 " LEFT JOIN MSatuan ON MSatuan.NoID=MKartuStok.IDSatuan " & _
                 " LEFT JOIN MGudang ON MGudang.NoID=MKartuStok.IDGUdang " & _
                 " LEFT JOIN MJenisTransaksi ON MJenisTransaksi.ID=MKartuStok.IDJenisTransaksi " & _
                 " WHERE MKartuStok.Tanggal<'" & TglDari.ToString("yyyy-MM-dd") & "' AND MBarang.NoID=" & IDbarang

        strsql &= " ORDER BY DATEADD(dd, 0, DATEDIFF(dd, 0, MKartuStok.Tanggal)) DESC, MJenisTransaksi.NoUrut DESC, MKartuStok.NoID DESC"

        dst = modSqlServer.ExecuteDataset("DataTEMP", strsql)
        If dst.Tables("DataTEMP").Rows.Count >= 1 Then
            QtySebelum = NullToDbl(dst.Tables("DataTEMP").Rows(0).Item("QtyAkhir"))
            HPPSebelum = NullToDbl(dst.Tables("DataTEMP").Rows(0).Item("HPP"))
        Else
            QtySebelum = 0
            HPPSebelum = 0
        End If
        dst.Dispose()

    End Sub
    Private Sub txtBarang_ButtonClick(ByVal sender As Object, ByVal e As DevExpress.XtraEditors.Controls.ButtonPressedEventArgs) Handles txtBarang.ButtonClick
        If e.Button.Index = 1 Then
            txtBarang.EditValue = -1
        End If
    End Sub
    Private Sub txtBarang_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtBarang.EditValueChanged
        Try
            lbStock.Text = NullToStr(EksekusiSQlSkalarNew("SELECT Nama FROM MBarang WHERE NoID=" & NullToLong(txtBarang.EditValue)))
        Catch ex As Exception
            XtraMessageBox.Show("Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK)
        End Try
    End Sub

    Private Sub TglDari_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TglDari.EditValueChanged

    End Sub
End Class
