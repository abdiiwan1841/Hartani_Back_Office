Imports System
Imports System.Windows.Forms
Imports DevExpress.XtraEditors
Imports System.Data.OleDb
Public Class FrmCekHargaBeli
    Public Class RecordPembelian
        Dim _NoUrut As Integer
        Dim _kode, _Barcode, _name As String
        Dim _TotalBeli, _AvgHarga As Double
        Public Sub New(ByVal NoUrut As Integer, ByVal Kode As String, ByVal Barcode As String, ByVal Name As String, ByVal AvgHarga As Double, ByVal TotalBeli As Double)
            _NoUrut = NoUrut
            _kode = Kode
            _name = Name
            _Barcode = Barcode
            _AvgHarga = AvgHarga
            _TotalBeli = TotalBeli
        End Sub

        Public ReadOnly Property NoUrut() As Integer
            Get
                Return _NoUrut
            End Get
        End Property
        Public Property Nama() As String
            Get
                Return _name
            End Get
            Set(ByVal Value As String)
                _name = Value
            End Set
        End Property

        Public Property Kode() As String
            Get
                Return _kode
            End Get
            Set(ByVal Value As String)
                _kode = Value
            End Set
        End Property

        Public Property Barcode() As String
            Get
                Return _Barcode
            End Get
            Set(ByVal Value As String)
                _Barcode = Value
            End Set
        End Property

        Public Property AvgHarga() As Double
            Get
                Return _AvgHarga
            End Get
            Set(ByVal Value As Double)
                _AvgHarga = Value
            End Set
        End Property

        Public Property TotalBeli() As Double
            Get
                Return _TotalBeli
            End Get
            Set(ByVal Value As Double)
                _TotalBeli = Value
            End Set
        End Property
    End Class

    Dim listDataSource As New System.ComponentModel.BindingList(Of RecordPembelian)
    Dim Urut As Integer = 0
    Dim Kode, Barcode As String
    Private Sub txtPath_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtPath.KeyDown
        If e.KeyCode = Keys.Enter Then
            Dim ds As New DataSet
            Dim sql As String
            Try
                sql = "SELECT MBarang.Barcode, MBarang.NoID, MBarang.Kode, Mbarang.Nama,MSatuan.Nama as Satuan,MBarang.Barcode,MBarangD.HargaJual,MBarangD.NilaiDiskon,Mbarang.TglDariDiskon,Mbarang.TglSampaiDiskon, " & _
                " IsNull((SELECT AVG(MBeliD.Harga/MBeliD.Konversi) FROM MBeliD INNER JOIN MBeli ON MBeli.NoID=MBeliD.IDBeli WHERE MBeli.Tanggal>='" & Tgl1.DateTime.ToString("yyyy/MM/dd") & "' AND MBeli.Tanggal<'" & Tgl2.DateTime.AddDays(1).ToString("yyyy/MM/dd") & "' AND MBeliD.IDBarang=MBarang.NoID AND MBarangD.NoID=MBeliD.IDBarangD),0) AS AvgHarga, " & _
                " IsNull((SELECT SUM(MBeliD.Jumlah) FROM MBeliD INNER JOIN MBeli ON MBeli.NoID=MBeliD.IDBeli WHERE MBeli.Tanggal>='" & Tgl1.DateTime.ToString("yyyy/MM/dd") & "' AND MBeli.Tanggal<'" & Tgl2.DateTime.AddDays(1).ToString("yyyy/MM/dd") & "' AND MBeliD.IDBarang=MBarang.NoID AND MBarangD.NoID=MBeliD.IDBarangD),0) AS TotalBeli" & _
                " FROM MBarangD inner Join Mbarang On MbarangD.IDbarang=Mbarang.NoID left Join MSatuan On MbarangD.IDsatuan=MSatuan.NoID WHERE  MBarang.IsActive=1 AND MBarangD.IsActive=1 AND (MBarang.Kode='" & FixApostropi(txtPath.Text) & "' or MBarangD.Barcode='" & FixApostropi(txtPath.Text) & "')"
                ds = ExecuteDataset("A", sql)
                If ds.Tables("A").Rows.Count > 0 Then
                    Kode = NullToStr(ds.Tables("A").Rows(0).Item("Kode"))
                    Barcode = NullToStr(ds.Tables("A").Rows(0).Item("Barcode"))
                    txtNama.Text = NullToStr(ds.Tables("A").Rows(0).Item("Nama"))
                    txtSatuan.Text = NullToStr(ds.Tables("A").Rows(0).Item("Satuan"))
                    txtHargaJual.EditValue = NullToDbl(ds.Tables("A").Rows(0).Item("HargaJual"))
                    txtDiskon.EditValue = NullToDbl(ds.Tables("A").Rows(0).Item("NilaiDiskon"))
                    txtHargaNetto.EditValue = NullToDbl(ds.Tables("A").Rows(0).Item("HargaJual")) - NullToDbl(ds.Tables("A").Rows(0).Item("NilaiDiskon"))
                    TglDari.EditValue = ds.Tables("A").Rows(0).Item("TglDariDiskon")
                    TglSampai.EditValue = ds.Tables("A").Rows(0).Item("TglSampaiDiskon")
                    Urut = Urut + 1
                    listDataSource.Add(New RecordPembelian(Urut, Kode, Barcode, txtNama.Text, NullToDbl(ds.Tables("A").Rows(0).Item("AvgHarga")), NullToDbl(ds.Tables("A").Rows(0).Item("TotalBeli"))))
                    GridView1.ClearSelection()
                    GridView1.FocusedRowHandle = GridView1.LocateByDisplayText(0, GridView1.Columns("NoUrut"), Urut.ToString("##0"))
                    GridView1.SelectRow(GridView1.FocusedRowHandle)
                    'GridControl1.DataSource = listDataSource
                    txtPath.Text = ""
                    txtNama.Text = ""
                    txtSatuan.Text = ""
                    txtHargaJual.EditValue = 0.0
                    txtDiskon.EditValue = 0.0
                    txtHargaNetto.EditValue = 0.0
                    TglDari.EditValue = Chr(0)
                    TglSampai.EditValue = Chr(0)

                Else
                    txtNama.Text = ""
                    txtSatuan.Text = ""
                    txtHargaJual.EditValue = 0.0
                    txtDiskon.EditValue = 0.0
                    txtHargaNetto.EditValue = 0.0
                    TglDari.EditValue = Chr(0)
                    TglSampai.EditValue = Chr(0)

                End If
            Catch ex As Exception
                XtraMessageBox.Show("Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
            Finally
                ds.Dispose()
            End Try
        ElseIf e.KeyCode = Keys.Home Then
            txtPath.Text = ""
            txtNama.Text = ""
            txtSatuan.Text = ""
            txtHargaJual.EditValue = 0.0
            txtDiskon.EditValue = 0.0
            txtHargaNetto.EditValue = 0.0
            TglDari.EditValue = Chr(0)
            TglSampai.EditValue = Chr(0)


        End If
    End Sub

    Private Sub TglDari_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TglDari.EditValueChanged

    End Sub

    Private Sub txtPath_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtPath.EditValueChanged
        'If txtPath.Text.Length >= 6 Then
        '    Dim ds As New DataSet
        '    Dim sql As String
        '    Try
        '        sql = "SELECT MBarang.NoID, MBarang.Kode, Mbarang.Nama,MSatuan.Nama as Satuan,MBarang.Barcode,MBarangD.HargaJual,MBarangD.NilaiDiskon,TglDariDiskon,TglSampaiDiskon " & _
        '        "FROM MBarangD inner Join Mbarang On MbarangD.IDbarang=Mbarang.NoID left Join MSatuan On MbarangD.IDsatuan=MSatuan.NoID WHERE MBarang.Kode='" & FixApostropi(txtPath.Text) & "' or MBarangD.Barcode='" & FixApostropi(txtPath.Text) & "'"
        '        ds = ExecuteDataset("A", sql)
        '        If ds.Tables("A").Rows.Count > 0 Then
        '            Urut = Urut + 1
        '            Kode = NullToStr(ds.Tables("A").Rows(0).Item("Kode"))
        '            Barcode = NullToStr(ds.Tables("A").Rows(0).Item("Barcode"))
        '            txtNama.Text = NullToStr(ds.Tables("A").Rows(0).Item("Nama"))
        '            txtSatuan.Text = NullToStr(ds.Tables("A").Rows(0).Item("Satuan"))
        '            txtHargaJual.EditValue = NullToDbl(ds.Tables("A").Rows(0).Item("HargaJual"))
        '            txtDiskon.EditValue = NullToDbl(ds.Tables("A").Rows(0).Item("NilaiDiskon"))
        '            txtHargaNetto.EditValue = NullToDbl(ds.Tables("A").Rows(0).Item("HargaJual")) - NullToDbl(ds.Tables("A").Rows(0).Item("NilaiDiskon"))
        '            TglDari.EditValue = ds.Tables("A").Rows(0).Item("TglDariDiskon")
        '            TglSampai.EditValue = ds.Tables("A").Rows(0).Item("TglSampaiDiskon")
        '            listDataSource.Add(New Record(Urut, Kode, Barcode, txtNama.Text, txtSatuan.Text, NullToDbl(txtHargaNetto.EditValue)))
        '            'GridControl1.DataSource = listDataSource
        '        Else
        '            txtNama.Text = ""
        '            txtSatuan.Text = ""
        '            txtHargaJual.EditValue = 0.0
        '            txtDiskon.EditValue = 0.0
        '            txtHargaNetto.EditValue = 0.0
        '            TglDari.EditValue = Chr(0)
        '            TglSampai.EditValue = Chr(0)

        '        End If
        '    Catch ex As Exception
        '        MsgBox(ex.Message)
        '    End Try
        'End If
    End Sub

    Private Sub cmdTutup_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdTutup.Click
        DialogResult = Windows.Forms.DialogResult.Cancel
        Me.Dispose()
        Me.Close()
    End Sub

    Private Sub SimpleButton1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SimpleButton1.Click
        Urut = 0
        listDataSource.Clear()

    End Sub

    Private Sub FrmCekHargaJual_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Tgl1.DateTime = CDate(TanggalSystem.Year & "/" & TanggalSystem.Month & "/01")
        Tgl2.DateTime = TanggalSystem

        GridControl1.DataSource = listDataSource
    End Sub

    Private Sub SimpleButton2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SimpleButton2.Click
        Try
            listDataSource.RemoveAt(GridView1.FocusedRowHandle)
        Catch
        End Try
    End Sub
End Class