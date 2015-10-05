Imports DevExpress.XtraEditors
Imports System.IO
Public Class frmBacaLogStockOpname
    Public FormName As String = "LogStockOpname"
    Public Caption As String = ""
    Public isNew As Boolean = True
    Public NoID As Long = -1
    Public IsByVarian As Boolean = False
    Public JamSo As Date
    Private Sub frmSimpleEntri_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        If DialogResult = Windows.Forms.DialogResult.Cancel AndAlso cmdSave.Enabled Then
            If XtraMessageBox.Show("Yakin ingin keluar?" & vbCrLf & "Cancel untuk membatalkan", NamaAplikasi, MessageBoxButtons.OKCancel, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) = Windows.Forms.DialogResult.Cancel Then
                e.Cancel = True
            End If
        End If
    End Sub

    Private Sub BarButtonItem1_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem1.ItemClick
        cmdSave.PerformClick()
    End Sub
    Sub BukaFile()
        Dim Odlg = New OpenFileDialog
        Odlg.Filter = "Text File|*.txt|All Files|*.*"
        Odlg.Title = "File Stock Opname"
        If Odlg.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
            Dim FileReader As StreamReader

            FileReader = New StreamReader(Odlg.FileName)

            MemoEdit1.Text = FileReader.ReadToEnd()

            FileReader.Close()

        End If
        Odlg.Dispose()
    End Sub
    Sub SimpanFile()
        Dim FileWriter As StreamWriter
        Dim Sdlg = New SaveFileDialog

        Sdlg.Filter = "Text File|*.txt|All Files|*.*"
        Sdlg.Title = "File Stock Opname"

        If Sdlg.ShowDialog(Me) = DialogResult.OK Then

            FileWriter = New StreamWriter(Sdlg.FileName, False)

            FileWriter.Write(MemoEdit1.Text)

            FileWriter.Close()
        End If
        Sdlg.Dispose()
    End Sub
    Private Sub SimpleButton1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SimpleButton1.Click
        BukaFile()
    End Sub

    Private Sub SimpleButton2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SimpleButton2.Click
        SimpanFile()
    End Sub

    Private Sub cmdSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSave.Click
        ProsesBacaData()
    End Sub


    Sub ProsesBacaData()
        Dim i As Long
        Dim hasil As String = ""
        Dim IDBarangD As Integer
        Dim IDBarang As Integer
        Dim databarcode() As String
        For i = 0 To MemoEdit1.Lines.Length - 1
            ProgressBarControl1.Position = (i + 1) / MemoEdit1.Lines.Length * 100
            Application.DoEvents()
            databarcode = Split(MemoEdit1.Lines(i), ",")
            If databarcode.Length >= 2 Then
                'MsgBox(databarcode(0) & "= " & databarcode(1))
                If IsByVarian Then
                    IDBarangD = NullToLong(EksekusiSQlSkalarNew("Select NoID from MBarangD where Barcode='" & FixApostropi(databarcode(0)) & "'"))
                    If IDBarangD > 0 Then
                        hasil = hasil & databarcode(0) & "," & databarcode(1) & ",OK" & vbCrLf
                        If IsNumeric(databarcode(1)) Then
                            InsertkanDetilByVarian(IDBarangD, IDBarang, CDbl(databarcode(1)), databarcode(0))
                        End If
                    Else
                        hasil = hasil & databarcode(0) & "," & databarcode(1) & ",Gagal,Barcode tidak ditemukan" & vbCrLf
                    End If
                Else
                    IDBarang = NullToLong(EksekusiSQlSkalarNew("Select IDBarang from MBarangD where Barcode='" & FixApostropi(databarcode(0)) & "'"))
                    If IDBarang > 0 Then
                        hasil = hasil & databarcode(0) & "," & databarcode(1) & ",OK" & vbCrLf
                        If IsNumeric(databarcode(1)) Then
                            InsertkanDetil(IDBarang, CDbl(databarcode(1)), databarcode(0))
                        End If
                    Else
                        hasil = hasil & databarcode(0) & "," & databarcode(1) & ",Gagal,Barcode tidak ditemukan" & vbCrLf
                    End If
                End If
            End If
        Next
        MemoEdit1.Text = hasil
        Application.DoEvents()
    End Sub
    Private Sub InsertkanDetil(ByVal IDBarang As Long, ByVal QtyFisik As Double, ByVal Barcode As String)
        Dim SQL As String = ""
        Dim IDDetil As Long = -1
        Dim IDSatuan As Long = -1
        Dim Konversi As Double = 0.0
        Dim QtyKomputer As Double = 0.0
        Dim HargaPokok As Double = 0.0
        Try
            Konversi = 1
            IDSatuan = NullToDbl(EksekusiSQlSkalarNew("SELECT IDSatuan FROM MBarang  WHERE NoID=" & IDBarang & " "))
            HargaPokok = NullToDbl(EksekusiSQlSkalarNew("SELECT HargaBeliPcs FROM MBarang WHERE NoID=" & IDBarang))
            QtyKomputer = NullToDbl(EksekusiSQlSkalarNew("SELECT SUM((MKartuStok.Qtymasuk*MKartuStok.Konversi)-(MKartuStok.QtyKeluar*MKartuStok.Konversi)) AS Qty FROM MKartuStok WHERE IDGudang=" & NullToLong(txtGudang.EditValue) & " AND IDBarang=" & IDBarang & " AND Tanggal<='" & JamSo & "'"))
            IDDetil = NullToLong(EksekusiSQlSkalarNew("Select NoID from MStockOpnameD where IDHeader=" & NoID & " and IDBarang = " & IDBarang))
            If IDDetil <= 0 Then
                IDDetil = GetNewID("MStockOpnameD", "NoID")
                SQL = "INSERT INTO MStockOpnameD (NoID,IDGudang,IDHeader,IDBarang,QtyFisik,QtyKomputer,Qty,QtyPcs,IDSatuan,Konversi,HargaPokok,Keterangan) VALUES " & vbCrLf
                SQL &= "(" & IDDetil & "," & NullToLong(txtGudang.EditValue) & "," & NoID & "," & IDBarang & "," & FixKoma(QtyFisik) & "," & FixKoma(QtyKomputer) & "," & FixKoma(QtyFisik - QtyKomputer) & "," & FixKoma(QtyFisik - QtyKomputer) & "," & IDSatuan & "," & FixKoma(Konversi) & "," & FixKoma(HargaPokok) & ",'" & FixApostropi(Barcode) & "')"

                EksekusiSQL(SQL)
            Else
                SQL = "Update MStockOpnameD set "
                SQL &= " QtyFisik=QtyFisik +" & FixKoma(QtyFisik) & ","
                SQL &= " Qty=Qty+" & FixKoma(QtyFisik) & ","
                SQL &= " QtyPcs=+" & FixKoma(QtyFisik) & " where NoID=" & IDDetil
                EksekusiSQL(SQL)
            End If
         
            'SQL &= " QtyKomputer=" & FixKoma(txtQtyKomputer.EditValue) & ","
            'SQL &= " QtyFisik=" & FixKoma(txtQtyFisik.EditValue) & ","
            'SQL &= " IDBarang=" & NullToLong(txtBarang.EditValue) & ","
            'SQL &= " IDGudang=" & NullToLong(txtGudang.EditValue) & ","
            'SQL &= " IDSatuan=" & NullToLong(txtSatuan.EditValue) & ","
            'SQL &= " Konversi=" & FixKoma(txtKonversi.EditValue) & ","
            'SQL &= " Qty=" & FixKoma(txtQty.EditValue) & ","
            'SQL &= " QtyPcs=" & FixKoma(txtQtyPcs.EditValue) & ","
            'SQL &= " Keterangan='" & FixApostropi(txtCatatan.Text) & "',"
            'SQL &= " HargaPokok=" & FixKoma(txtHargaPokok.EditValue) & ","
            'SQL &= " Jumlah=" & FixKoma(txtJumlah.EditValue) & ""
        Catch ex As Exception
            XtraMessageBox.Show("Kesalahan : " & ex.Message, "Pesan ", MessageBoxButtons.OK)

        End Try
    End Sub
    Private Sub InsertkanDetilByVarian(ByVal IDBarangD As Long, ByVal IDBarang As Long, ByVal QtyFisik As Double, ByVal Barcode As String)
        Dim SQL As String = ""
        Dim IDDetil As Long = -1
        Dim IDSatuan As Long = -1
        Dim Konversi As Double = 0.0
        Dim QtyKomputer As Double = 0.0
        Dim HargaPokok As Double = 0.0
        Try
            Konversi = 1
            IDSatuan = NullToDbl(EksekusiSQlSkalarNew("SELECT IDSatuan FROM MBarangD WHERE NoID=" & IDBarangD & " "))
            HargaPokok = NullToDbl(EksekusiSQlSkalarNew("SELECT HargaBeliPcs FROM MBarang WHERE NoID=" & IDBarang))
            QtyKomputer = NullToDbl(EksekusiSQlSkalarNew("SELECT SUM((MKartuStok.Qtymasuk*MKartuStok.Konversi)-(MKartuStok.QtyKeluar*MKartuStok.Konversi)) AS Qty FROM MKartuStok WHERE IDGudang=" & NullToLong(txtGudang.EditValue) & " AND IDBarangD=" & IDBarangD & " AND Tanggal<='" & JamSo & "'"))
            IDDetil = NullToLong(EksekusiSQlSkalarNew("Select NoID from MStockOpnameD where IDHeader=" & NoID & " and IDBarangD= " & IDBarangD))
            If IDDetil <= 0 Then
                IDDetil = GetNewID("MStockOpnameD", "NoID")
                SQL = "INSERT INTO MStockOpnameD (NoID,IDGudang,IDHeader,IDBarang,IDBarangD,QtyFisik,QtyKomputer,Qty,QtyPcs,IDSatuan,Konversi,HargaPokok,Keterangan) VALUES " & vbCrLf
                SQL &= "(" & IDDetil & "," & NullToLong(txtGudang.EditValue) & "," & NoID & "," & IDBarang & "," & IDBarangD & "," & FixKoma(QtyFisik) & "," & FixKoma(QtyKomputer) & "," & FixKoma(QtyFisik - QtyKomputer) & "," & FixKoma(QtyFisik - QtyKomputer) & "," & IDSatuan & "," & FixKoma(Konversi) & "," & FixKoma(HargaPokok) & ",'" & FixApostropi(Barcode) & "')"

                EksekusiSQL(SQL)
            Else
                SQL = "Update MStockOpnameD set "
                SQL &= " QtyFisik=QtyFisik +" & FixKoma(QtyFisik) & ","
                SQL &= " Qty=Qty+" & FixKoma(QtyFisik) & ","
                SQL &= " QtyPcs=+" & FixKoma(QtyFisik) & " where NoID=" & IDDetil
                EksekusiSQL(SQL)
            End If

            'SQL &= " QtyKomputer=" & FixKoma(txtQtyKomputer.EditValue) & ","
            'SQL &= " QtyFisik=" & FixKoma(txtQtyFisik.EditValue) & ","
            'SQL &= " IDBarang=" & NullToLong(txtBarang.EditValue) & ","
            'SQL &= " IDGudang=" & NullToLong(txtGudang.EditValue) & ","
            'SQL &= " IDSatuan=" & NullToLong(txtSatuan.EditValue) & ","
            'SQL &= " Konversi=" & FixKoma(txtKonversi.EditValue) & ","
            'SQL &= " Qty=" & FixKoma(txtQty.EditValue) & ","
            'SQL &= " QtyPcs=" & FixKoma(txtQtyPcs.EditValue) & ","
            'SQL &= " Keterangan='" & FixApostropi(txtCatatan.Text) & "',"
            'SQL &= " HargaPokok=" & FixKoma(txtHargaPokok.EditValue) & ","
            'SQL &= " Jumlah=" & FixKoma(txtJumlah.EditValue) & ""
        Catch ex As Exception
            XtraMessageBox.Show("Kesalahan : " & ex.Message, "Pesan ", MessageBoxButtons.OK)

        End Try
    End Sub
    Private Sub SimpleButton3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SimpleButton3.Click
        Me.Close()
        Me.Dispose()
    End Sub
    Sub RefreshLookUp()
        Dim ds As New DataSet
        Dim strsql As String
        strSql = "SELECT MGudang.NoID, MGudang.Kode, MGudang.Nama FROM MGudang where IsActive=1 "
        ds = ExecuteDataset("MGudang", strsql)
        txtGudang.Properties.DataSource = ds.Tables("MGudang")
        txtGudang.Properties.ValueMember = "NoID"
        txtGudang.Properties.DisplayMember = "Nama"
        ds.Dispose()
    End Sub
    Private Sub frmBacaLogStockOpname_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        refreshLookUp()
    End Sub
End Class