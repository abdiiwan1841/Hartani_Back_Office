Imports System
Imports System.Windows.Forms
Imports DevExpress.XtraEditors
Imports System.Data.OleDb
Imports DevExpress.XtraGrid.Views.Grid
Imports DevExpress.XtraEditors.Repository

Public Class frmCekPoin
    'Public Class RecordPoin
    '    Dim _NoUrut As Integer
    '    Dim _kode, _Barcode, _name, _Satuan As String
    '    Dim _HargaJual As Double
    '    Public Sub New(ByVal NoUrut As Integer, ByVal Kode As String, ByVal Barcode As String, ByVal name As String, ByVal Satuan As String, ByVal HargaJual As Double)
    '        _NoUrut = NoUrut
    '        _kode = Kode
    '        _name = name
    '        _Barcode = Barcode
    '        _Satuan = Satuan
    '        _HargaJual = HargaJual

    '    End Sub

    '    Public ReadOnly Property NoUrut() As Integer
    '        Get
    '            Return _NoUrut
    '        End Get
    '    End Property
    '    Public Property Nama() As String
    '        Get
    '            Return _name
    '        End Get
    '        Set(ByVal Value As String)
    '            _name = Value
    '        End Set
    '    End Property

    '    Public Property Kode() As String
    '        Get
    '            Return _kode
    '        End Get
    '        Set(ByVal Value As String)
    '            _kode = Value
    '        End Set
    '    End Property

    '    Public Property Barcode() As String
    '        Get
    '            Return _Barcode
    '        End Get
    '        Set(ByVal Value As String)
    '            _Barcode = Value
    '        End Set
    '    End Property

    '    Public Property Satuan() As String
    '        Get
    '            Return _Satuan
    '        End Get
    '        Set(ByVal Value As String)
    '            _Satuan = Value
    '        End Set
    '    End Property



    '    Public Property HargaJual() As Double
    '        Get
    '            Return _HargaJual
    '        End Get
    '        Set(ByVal Value As Double)
    '            _HargaJual = Value
    '        End Set
    '    End Property
    'End Class

    'Dim listDataSource As New System.ComponentModel.BindingList(Of Record)
    Dim repckedit As New RepositoryItemCheckEdit
    Dim repdateedit As New RepositoryItemDateEdit
    Dim reptextedit As New RepositoryItemTextEdit
    Dim reppicedit As New RepositoryItemPictureEdit
    Dim IDCustomer As Long = -1
    Private Sub txtPath_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtPath.KeyDown
        If e.KeyCode = Keys.Enter Then
            Dim ds As New DataSet
            Dim SQL As String
            Try
                'SQL = "SELECT Malamat.NoID, MAlamat.Kode, MAlamat.Nama, MAlamat.Alamat, IsNull((SELECT SUM(MJual.NilaiPoin) FROM MJual WHERE MJual.IDCustomer=MAlamat.NoID),0)-IsNull((SELECT SUM(MTukarPoin.Kredit) FROM MTukarPoin WHERE MTukarPoin.IDMember=MAlamat.NoID),0) AS Poin " & vbCrLf & _
                '      " FROM MAlamat " & vbCrLf & _
                '      " WHERE UPPER(MAlamat.Kode)='" & FixApostropi(txtPath.Text) & "'"

                SQL = "SELECT Malamat.NoID, MAlamat.Kode, MAlamat.Nama, MAlamat.Alamat, IsNull((SELECT vSaldoPoin.SaldoPoin FROM vSaldoPoin WHERE vSaldoPoin.IDCustomer=MAlamat.NoID),0) AS Poin " & vbCrLf & _
                      " FROM MAlamat " & vbCrLf & _
                      " WHERE UPPER(MAlamat.Kode)='" & FixApostropi(txtPath.Text) & "'"
                ds = ExecuteDataset("A", SQL)
                If ds.Tables("A").Rows.Count > 0 Then
                    IDCustomer = NullToLong(ds.Tables("A").Rows(0).Item("NoID"))
                    txtNama.Text = NullToStr(ds.Tables("A").Rows(0).Item("Nama"))
                    txtSatuan.Text = NullToStr(ds.Tables("A").Rows(0).Item("Alamat"))
                    txtHargaJual.EditValue = NullToDbl(ds.Tables("A").Rows(0).Item("Poin"))
                Else
                    txtNama.Text = ""
                    txtSatuan.Text = ""
                    txtHargaJual.EditValue = 0.0
                End If
                RefreshDetil()
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
        End If
    End Sub

    Private Sub RefreshDetil()
        Dim SQL As String = ""
        Dim ds As New DataSet
        Try
            'SQL = "SELECT MJual.Kode, MJual.Tanggal, MJual.Total, MJual.BarangPoin, MJual.NilaiPoin, MPOS.Nama AS Kassa" & vbCrLf & _
            '      " FROM MJual " & vbCrLf & _
            '      " INNER JOIN MPOS ON MPOS.NoID=MJual.IDPos" & vbCrLf & _
            '      " INNER JOIN MAlamat ON MAlamat.NoID=MJual.IDCustomer" & vbCrLf & _
            '      " WHERE ISNULL(MJual.NilaiPoin,0)<>0 AND MJual.IDCustomer=" & IDCustomer & " AND YEAR(MJual.Tanggal)=" & TanggalSystem.Date.Year & vbCrLf & _
            '      " UNION ALL " & vbCrLf & _
            '      " SELECT MTukarPoin.NoMember, MTukarPoin.Tanggal, 0, MTukarPoin.JumlahPoin, -1 * MTukarPoin.Kredit, MPOS.Nama " & vbCrLf & _
            '      " FROM MTukarPoin  " & vbCrLf & _
            '      " LEFT JOIN MPOS ON MPOS.NoID=MTukarPoin.IDKassa " & vbCrLf & _
            '      " WHERE MTukarPoin.IDMember=" & NullToLong(IDCustomer) & " AND YEAR(MTukarPoin.Tanggal)=" & TanggalSystem.Date.Year & vbCrLf & _
            '      " UNION ALL" & vbCrLf & _
            '      " SELECT 'Reedem Poin ' + MJual.Kode, MJual.Tanggal, 0, -1*MJual.ReedemNilai, -1*MJual.ReedemPoin, MPOS.Nama AS Kassa" & vbCrLf & _
            '      " FROM MJual " & vbCrLf & _
            '      " INNER JOIN MPOS ON MPOS.NoID=MJual.IDPos" & vbCrLf & _
            '      " INNER JOIN MAlamat ON MAlamat.NoID=MJual.IDCustomer" & vbCrLf & _
            '      " WHERE ISNULL(MJual.ReedemPoin,0)<>0 AND MJual.IDCustomer=" & IDCustomer & " AND YEAR(MJual.Tanggal)=" & TanggalSystem.Date.Year

            SQL = "EXEC sp_HistoriPoinMember " & FixKoma(IDCustomer)
            ds = ExecuteDataset("MDetil", SQL)
            GridControl1.DataSource = ds.Tables("MDetil")
            GridView1.OptionsBehavior.Editable = False
            For Each ctrl As GridView In GridControl1.Views
                With ctrl
                    For i As Integer = 0 To .Columns.Count - 1
                        Select Case .Columns(i).ColumnType.Name.ToLower
                            Case "int32", "int64", "int"
                                .Columns(i).DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
                                .Columns(i).DisplayFormat.FormatString = "n0"
                            Case "decimal", "single", "money", "double"
                                .Columns(i).DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
                                .Columns(i).DisplayFormat.FormatString = "n2"
                            Case "string"
                                .Columns(i).DisplayFormat.FormatType = DevExpress.Utils.FormatType.None
                                .Columns(i).DisplayFormat.FormatString = ""
                            Case "date", "datetime"
                                If .Columns(i).FieldName.Trim.ToLower = "jam" Then
                                    .Columns(i).DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime
                                    .Columns(i).DisplayFormat.FormatString = "HH:mm"
                                ElseIf .Columns(i).FieldName.Trim.ToLower = "tanggalstart" Or .Columns(i).FieldName.Trim.ToLower = "tanggalend" Then
                                    .Columns(i).DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime
                                    .Columns(i).DisplayFormat.FormatString = "dd-MM-yyyy HH:mm"
                                Else
                                    .Columns(i).DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime
                                    .Columns(i).DisplayFormat.FormatString = "dd-MM-yyyy"
                                End If

                            Case "byte[]"
                                reppicedit.SizeMode = DevExpress.XtraEditors.Controls.PictureSizeMode.Squeeze
                                .Columns(i).OptionsColumn.AllowGroup = False
                                .Columns(i).OptionsColumn.AllowSort = False
                                .Columns(i).OptionsFilter.AllowFilter = False
                                .Columns(i).ColumnEdit = reppicedit
                            Case "boolean"
                                .Columns(i).ColumnEdit = repckedit
                        End Select
                    Next
                End With
            Next
        Catch ex As Exception
            XtraMessageBox.Show("Info Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
        Finally
            If Not ds Is Nothing Then
                ds.Dispose()
            End If
        End Try
    End Sub

    Private Sub cmdTutup_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdTutup.Click
        DialogResult = Windows.Forms.DialogResult.Cancel
        Me.Dispose()
        Me.Close()
    End Sub

    Private Sub frmCekPoin_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        GridView1.SaveLayoutToXml(FolderLayouts & Me.Name & GridView1.Name & ".xml")
        GridView2.SaveLayoutToXml(FolderLayouts & Me.Name & GridView2.Name & ".xml")
    End Sub

    Private Sub frmCekPoin_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            RefreshDetil()
            If System.IO.File.Exists(FolderLayouts & Me.Name & GridView1.Name & ".xml") Then
                GridView1.RestoreLayoutFromXml(FolderLayouts & Me.Name & GridView1.Name & ".xml")
            End If
            If System.IO.File.Exists(FolderLayouts & Me.Name & GridView2.Name & ".xml") Then
                GridView2.RestoreLayoutFromXml(FolderLayouts & Me.Name & GridView2.Name & ".xml")
            End If
        Catch ex As Exception

        End Try
    End Sub

    Private Sub txtPath_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtPath.EditValueChanged

    End Sub
End Class