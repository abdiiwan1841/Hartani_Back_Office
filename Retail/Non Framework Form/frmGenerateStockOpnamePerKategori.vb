Imports System.Windows.Forms
Imports System.Data.SqlClient
Imports DevExpress.XtraEditors
Imports System.Data.Odbc

Public Class frmGenerateStockOpnamePerKategori
    Public IDStockOpname As Long
    Private Sub cmdTutup_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdTutup.Click
        DialogResult = Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub cmdOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdOK.Click
        ProsesTambahData()
        MsgBox("Proses Selesai!", MsgBoxStyle.Information + MsgBoxStyle.OkOnly)
        'DialogResult = Windows.Forms.DialogResult.OK
        'Me.Close()
    End Sub
    Sub ProsesTambahData()
        Dim ds As New DataSet
        Dim strsql As String
        Dim i As Integer
        Try
            If ckAll.Checked Then
                strsql = "SELECT  MBarangD.NoID,MbarangD.IDBarang,MbarangD.IDSatuan,MbarangD.Konversi,MBarang.HargaBeliPcs,MBarangD.HargaJual FROM MBarangD inner Join Mbarang On MbarangD.IDbarang=MBarang.NoID " & _
                "where MBarang.IDKategori=" & NullToLong(txtKategori.EditValue) & " and MBarangD.NoID Not In (select distinct IDBarangD from MStockOpnameD where MStockOpnameD.IDHeader=" & IDStockOpname & ")"
            Else
                If txtKategori.Enabled Then
                    strsql = "SELECT  MBarangD.NoID,MbarangD.IDBarang,MbarangD.IDSatuan,MbarangD.Konversi,MBarang.HargaBeliPcs,MBarangD.HargaJual FROM MBarangD inner Join Mbarang On MbarangD.IDbarang=MBarang.NoID " & _
                    "where MBarang.IDKategori=" & NullToLong(txtKategori.EditValue) & " and MBarangD.NoID Not In " & _
                    "(select distinct MStockOpnameD.IDBarangD from MStockOpnameD inner join mstockopname on mstockopnamed.IDHeader= mstockopname.NoID where mstockopname.Tanggal>='" & Format(TglDari.DateTime, "yyyy-MM-dd") & "' and mstockopname.Tanggal<'" & Format(TglSampai.DateTime.AddDays(1), "yyyy-MM-dd") & "') order by Mbarang.Kode "
                Else
                    strsql = "SELECT  MBarangD.NoID,MbarangD.IDBarang,MbarangD.IDSatuan,MbarangD.Konversi,MBarang.HargaBeliPcs,MBarangD.HargaJual FROM MBarangD inner Join Mbarang On MbarangD.IDbarang=MBarang.NoID " & _
                "where MBarangD.NoID Not In " & _
                "(select distinct MStockOpnameD.IDBarangD from MStockOpnameD inner join mstockopname on mstockopnamed.IDHeader= mstockopname.NoID where mstockopname.Tanggal>='" & Format(TglDari.DateTime, "yyyy-MM-dd") & "' and mstockopname.Tanggal<'" & Format(TglSampai.DateTime.AddDays(1), "yyyy-MM-dd") & "') order by Mbarang.Kode "

                End If
                End If
            ds = ExecuteDataset("DataBarangD", strsql)
            For i = 0 To ds.Tables("DataBarangD").Rows.Count - 1
                TambahItem(NullTolInt(ds.Tables("DataBarangD").Rows(i).Item("NoID")), _
                           NullTolInt(ds.Tables("DataBarangD").Rows(i).Item("IDBarang")), _
                            NullTolInt(ds.Tables("DataBarangD").Rows(i).Item("IDSatuan")), _
                            NullTolInt(ds.Tables("DataBarangD").Rows(i).Item("Konversi")), _
                            NullTolInt(ds.Tables("DataBarangD").Rows(i).Item("HargaBeliPcs")), _
                            NullTolInt(ds.Tables("DataBarangD").Rows(i).Item("HargaJual")))
                ProgressBarControl1.Position = (i + 1) * 100 / ds.Tables("DataBarangD").Rows.Count
                Application.doevents()
            Next

        Catch ex As Exception
            XtraMessageBox.Show("Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            ds.Dispose()
        End Try
    End Sub
    Sub TambahItem(ByVal IDBarangD As Integer, ByVal IDBarang As Integer, ByVal IDSatuan As Integer, ByVal Konversi As Double, ByVal HargaPokok As Double, ByVal HargaJual As Double)
        Dim Sql As String
        Dim QtyKomputer As Double
        QtyKomputer = NullToDbl(EksekusiSQlSkalarNew("SELECT SUM((MKartuStok.Qtymasuk*MKartuStok.Konversi)-(MKartuStok.QtyKeluar*MKartuStok.Konversi)) AS Qty FROM MKartuStok WHERE IDGudang=" & NullToLong(txtGudang.EditValue) & " AND IDBarangD=" & IDBarangD & " ")) / 1

        Sql = "INSERT INTO [MStockOpnameD] ([QtyKomputer],[QtyFisik],[NoID],[IDHeader],[IDBarang],[IDGudang],[IDSatuan],[Konversi],[Qty],[QtyPCS],[Keterangan],[HargaPokok],[Jumlah],[IDBarangD],[HargaJual]) VALUES ("
        Sql &= FixKoma(QtyKomputer) & ","
        Sql &= FixKoma(0) & ","
        Sql &= NullToLong(GetNewID("MStockOpnameD", "NoID")) & ","
        Sql &= IDStockOpname & ","
        Sql &= IDBarang & ","
        Sql &= NullToLong(txtGudang.EditValue) & ","
        Sql &= IDSatuan & ","
        Sql &= FixKoma(Konversi) & ","
        Sql &= FixKoma(0 - QtyKomputer) & ","
        Sql &= FixKoma(Konversi * (0 - QtyKomputer)) & ","
        Sql &= "'Barang Kosong',"
        Sql &= FixKoma(HargaPokok) & ","
        Sql &= FixKoma(HargaPokok * (Konversi * (0 - QtyKomputer))) & ","
        Sql &= NullToLong(IDBarangD) & ","
        Sql &= FixKoma(HargaJual) & " "
        Sql &= ")"
        EksekusiSQL(Sql)
    End Sub
    Sub refreshLookUp()
        Dim ds As New DataSet
        Dim strsql As String
        Try
            strsql = "SELECT  NoID, Kode, Nama FROM MKategori where isactive=1"
            ds = ExecuteDataset("masterKategori", strsql)
            txtKategori.Properties.DataSource = ds.Tables("masterKategori")
            txtKategori.Properties.ValueMember = "NoID"
            txtKategori.Properties.DisplayMember = "Nama"
            If System.IO.File.Exists(FolderLayouts & Me.Name & gvKategori.Name & ".xml") Then
                gvKategori.RestoreLayoutFromXml(FolderLayouts & Me.Name & gvKategori.Name & ".xml")
            End If

            strsql = "SELECT MGudang.NoID, MGudang.Kode, MGudang.Nama, MWilayah.Nama AS Wilayah FROM MGudang LEFT JOIN MWilayah ON MWilayah.NoID=MGUdang.IDWilayah WHERE MGudang.IsActive=1"
            ds = ExecuteDataset("MWilayah", strsql)
            txtGudang.Properties.DataSource = ds.Tables("MWilayah")
            txtGudang.Properties.ValueMember = "NoID"
            txtGudang.Properties.DisplayMember = "Kode"

        Catch ex As Exception
            XtraMessageBox.Show("Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            ds.Dispose()
        End Try
    End Sub

    Private Sub ckAll_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ckAll.CheckedChanged
        If ckAll.Checked = True Then
            TglDari.Enabled = False
            TglSampai.Enabled = False
        Else
            TglDari.Enabled = True
            TglSampai.Enabled = True

        End If
    End Sub

    Private Sub frmGenerateStockOpnamePerKategori_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        refreshLookUp()
        txtGudang.EditValue = DefIDGudang
    End Sub

    Private Sub LayoutControlItem3_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LayoutControlItem3.Click
        txtKategori.Enabled = Not txtKategori.Enabled
    End Sub
End Class