Imports System.Data
Imports System.Data.SqlClient
Imports DevExpress.XtraEditors
Imports DevExpress.Utils

Public Class frmEntriPOD
    Public IsNew As Boolean = True
    Public IsPosted As Boolean = False
    Public NoID As Long = -1
    Public IDPO As Long = -1
    Public IDSupplier As Long = -1

    Dim oDS As New DataSet
    Dim SQL As String = ""
    Dim DefImageList As ImageList

    Private Sub IsiDefault()
        DefImageList = frmMain.ImageList1
        cmdSave.ImageList = DefImageList
        cmdSave.ImageIndex = 6

        cmdTutup.ImageList = DefImageList
        cmdTutup.ImageIndex = 3
        RefreshLookUp()
    End Sub
    Private Sub RefreshLookUp()
        Dim ds As New DataSet
        Try

            SQL = "SELECT MGudang.NoID, MGudang.Kode, MGudang.Nama, MWilayah.Nama AS Wilayah FROM MGudang LEFT JOIN MWilayah ON MWilayah.NoID=MGudang.IDWilayah WHERE MGudang.IsActive=1 AND MGudang.IsBS=0 "
            ds = ExecuteDataset("MGudang", SQL)
            txtGudang.Properties.DataSource = ds.Tables("MGudang")
            txtGudang.Properties.ValueMember = "NoID"
            txtGudang.Properties.DisplayMember = "Nama"
            If System.IO.File.Exists(Application.StartupPath & "\system\layouts\" & Me.Name & gvGudang.Name & ".xml") Then
                gvGudang.RestoreLayoutFromXml(Application.StartupPath & "\system\layouts\" & Me.Name & gvGudang.Name & ".xml")
            End If
            With gvGudang
                For x As Integer = 0 To .Columns.Count - 1
                    Select Case .Columns(x).ColumnType.Name.ToLower
                        Case "int32", "int64", "int"
                            .Columns(x).DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
                            .Columns(x).DisplayFormat.FormatString = "n0"
                        Case "decimal", "single", "money", "double"
                            .Columns(x).DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
                            .Columns(x).DisplayFormat.FormatString = "n2"
                        Case "string"
                            .Columns(x).DisplayFormat.FormatType = DevExpress.Utils.FormatType.None
                            .Columns(x).DisplayFormat.FormatString = ""
                        Case "date", "datetime"
                            If .Columns(x).FieldName.Trim.ToLower = "jam" Then
                                .Columns(x).DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime
                                .Columns(x).DisplayFormat.FormatString = "hh:mm"
                            Else
                                .Columns(x).DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime
                                .Columns(x).DisplayFormat.FormatString = "dd-MM-yyyy"
                            End If
                    End Select
                Next
            End With

            SQL = "SELECT MBarang.NoID, MBarang.Kode, MBarang.Nama, MSatuan.Nama AS Satuan FROM MBarang LEFT JOIN MSatuan ON MSatuan.NoID=MBarang.IDSatuan WHERE MBarang.IsActive=1 "
            'If IDSupplier >= 1 Then
            '    SQL &= " AND (MBarang.IDSupplier1=" & IDSupplier
            '    SQL &= " OR MBarang.IDSupplier2=" & IDSupplier
            '    SQL &= " OR MBarang.IDSupplier3=" & IDSupplier
            '    SQL &= " OR MBarang.IDSupplier4=" & IDSupplier
            '    SQL &= " OR MBarang.IDSupplier5=" & IDSupplier & ")"
            'End If
            ds = ExecuteDataset("MBarang", SQL)
            txtPegawai.Properties.DataSource = ds.Tables("MBarang")
            txtPegawai.Properties.ValueMember = "NoID"
            txtPegawai.Properties.DisplayMember = "Kode"
            If System.IO.File.Exists(Application.StartupPath & "\system\layouts\" & Me.Name & gvBarang.Name & ".xml") Then
                gvBarang.RestoreLayoutFromXml(Application.StartupPath & "\system\layouts\" & Me.Name & gvBarang.Name & ".xml")
            End If
            With gvBarang
                For x As Integer = 0 To .Columns.Count - 1
                    Select Case .Columns(x).ColumnType.Name.ToLower
                        Case "int32", "int64", "int"
                            .Columns(x).DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
                            .Columns(x).DisplayFormat.FormatString = "n0"
                        Case "decimal", "single", "money", "double"
                            .Columns(x).DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
                            .Columns(x).DisplayFormat.FormatString = "n2"
                        Case "string"
                            .Columns(x).DisplayFormat.FormatType = DevExpress.Utils.FormatType.None
                            .Columns(x).DisplayFormat.FormatString = ""
                        Case "date", "datetime"
                            If .Columns(x).FieldName.Trim.ToLower = "jam" Then
                                .Columns(x).DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime
                                .Columns(x).DisplayFormat.FormatString = "hh:mm"
                            Else
                                .Columns(x).DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime
                                .Columns(x).DisplayFormat.FormatString = "dd-MM-yyyy"
                            End If
                    End Select
                Next
            End With

            SQL = "SELECT MSatuan.NoID, MSatuan.Kode, MSatuan.Nama FROM MSatuan WHERE MSatuan.IsActive=1 AND MSatuan.NoID IN (SELECT MBarangD.IDSatuan FROM MBarangD WHERE MBarangD.IDBarang=" & NullTolong(txtPegawai.EditValue) & ")"
            ds = ExecuteDataset("MSatuan", SQL)
            txtSatuan.Properties.DataSource = ds.Tables("MSatuan")
            txtSatuan.Properties.ValueMember = "NoID"
            txtSatuan.Properties.DisplayMember = "Kode"
            If System.IO.File.Exists(Application.StartupPath & "\system\layouts\" & Me.Name & gvSatuan.Name & ".xml") Then
                gvSatuan.RestoreLayoutFromXml(Application.StartupPath & "\system\layouts\" & Me.Name & gvSatuan.Name & ".xml")
            End If
            With gvSatuan
                For x As Integer = 0 To .Columns.Count - 1
                    Select Case .Columns(x).ColumnType.Name.ToLower
                        Case "int32", "int64", "int"
                            .Columns(x).DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
                            .Columns(x).DisplayFormat.FormatString = "n0"
                        Case "decimal", "single", "money", "double"
                            .Columns(x).DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
                            .Columns(x).DisplayFormat.FormatString = "n2"
                        Case "string"
                            .Columns(x).DisplayFormat.FormatType = DevExpress.Utils.FormatType.None
                            .Columns(x).DisplayFormat.FormatString = ""
                        Case "date", "datetime"
                            If .Columns(x).FieldName.Trim.ToLower = "jam" Then
                                .Columns(x).DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime
                                .Columns(x).DisplayFormat.FormatString = "hh:mm"
                            Else
                                .Columns(x).DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime
                                .Columns(x).DisplayFormat.FormatString = "dd-MM-yyyy"
                            End If
                    End Select
                Next
            End With
        Catch ex As Exception
            XtraMessageBox.Show("Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            ds.Dispose()
        End Try
    End Sub
    Private Sub LoadData()
        Try
            SQL = "SELECT MPOD.*, MBarang.Kode AS KodeBarang, MBarang.Nama AS NamaBarang, MGudang.Kode AS KodeGudang, MGudang.Nama AS NamaGudang, MSatuan.Nama AS NamaSatuan "
            SQL &= " FROM ((MPOD LEFT JOIN MGudang ON MGudang.NoID=MPOD.IDGudang) "
            SQL &= " LEFT JOIN MBarang ON MBarang.NoID=MPOD.IDBarang)"
            SQL &= " LEFT JOIN MSatuan ON MSatuan.NoID=MPOD.IDSatuan "
            SQL &= " WHERE MPOD.NoID= " & NoID
            oDS = New DataSet()
            oDS = ExecuteDataset("MPOD", SQL)
            If oDS.Tables(0).Rows.Count >= 1 Then
                With oDS.Tables("MPOD").Rows(0)
                    NoID = NullTolong(.Item("NoID"))
                    IDPO = NullTolong(.Item("IDPO"))
                    txtPegawai.EditValue = NullTolong(.Item("IDBarang"))
                    'txtBarang.Text = NullTostr(.Item("KodeBarang"))
                    txtSatuan.EditValue = NullTolong(.Item("IDSatuan"))
                    'txtSatuan.Text = NullTostr(.Item("KodeSatuan"))
                    txtGudang.EditValue = NullTolong(.Item("IDGudang"))
                    'txtGudang.Text = NullTostr(.Item("KodeGudang"))
                    txtQty.EditValue = NullToDbl(.Item("Qty"))
                    txtHarga.EditValue = NullToDbl(.Item("Harga"))
                    txtCtn.EditValue = NullToDbl(.Item("CTN"))
                    txtDiscPersen1.EditValue = NullToDbl(.Item("DiscPersen1"))
                    txtDiscPersen2.EditValue = NullToDbl(.Item("DiscPersen2"))
                    txtDiscPersen3.EditValue = NullToDbl(.Item("DiscPersen3"))
                    txtDiscRp1.EditValue = NullToDbl(.Item("Disc1"))
                    txtDiscRp2.EditValue = NullToDbl(.Item("Disc2"))
                    txtDiscRp3.EditValue = NullToDbl(.Item("Disc3"))
                    txtJUmlah.EditValue = NullToDbl(.Item("Jumlah"))
                    txtCatatan.Text = NullTostr(.Item("Catatan"))
                    txtKonversi.EditValue = NullToDbl(.Item("Konversi"))
                End With

            Else
                IsiDefault()
            End If
        Catch ex As Exception
            XtraMessageBox.Show("Kesalahan: " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            oDS.Dispose()
        End Try
    End Sub

    Private Sub cmdSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSave.Click
        If IsValidasi() Then
            HitungJumlah()
            If Simpan() Then
                DialogResult = Windows.Forms.DialogResult.OK
                Me.Close()
            End If
        End If
    End Sub
    Private Function Simpan() As Boolean
        Try
            If IsNew Then
                SQL = "INSERT INTO MPOD (NoID,IDPO,NoUrut,Tgl,Jam,IDBarang,IDSatuan,Qty,QtyPcs,Harga,HargaPcs,CTN,DiscPersen1,DiscPersen2,DiscPersen3,Disc1,Disc2,Disc3,Jumlah,Catatan,IDGudang,Konversi) VALUES ("
                SQL &= NullTolong(GetNewID("MPOD", "NoID")) & ","
                SQL &= IDPO & ","
                SQL &= GetNewID("MPOD", "NoUrut", " WHERE IDPO=" & IDPO) & ","
                SQL &= "GetDate(),"
                SQL &= "GetDate(),"
                SQL &= NullTolong(txtPegawai.EditValue) & ","
                SQL &= NullTolong(txtSatuan.EditValue) & ","
                SQL &= FixKoma(txtQty.EditValue) & ","
                SQL &= FixKoma(txtQty.EditValue * txtKonversi.EditValue) & ","
                SQL &= FixKoma(txtHarga.EditValue) & ","
                SQL &= FixKoma((txtHarga.EditValue - txtDiscRp1.EditValue - txtDiscRp2.EditValue - txtDiscRp1.EditValue - (txtHarga.EditValue * txtDiscPersen1.EditValue / 100) - (txtHarga.EditValue * txtDiscPersen2.EditValue / 100) - (txtHarga.EditValue * txtDiscPersen3.EditValue / 100)) / txtKonversi.EditValue) & ","
                SQL &= FixKoma(txtCtn.EditValue) & ","
                SQL &= FixKoma(txtDiscPersen1.EditValue) & ","
                SQL &= FixKoma(txtDiscPersen2.EditValue) & ","
                SQL &= FixKoma(txtDiscPersen3.EditValue) & ","
                SQL &= FixKoma(txtDiscRp1.EditValue) & ","
                SQL &= FixKoma(txtDiscRp2.EditValue) & ","
                SQL &= FixKoma(txtDiscRp3.EditValue) & ","
                SQL &= FixKoma(txtJUmlah.EditValue) & ","
                SQL &= "'" & FixApostropi(txtCatatan.Text) & "',"
                SQL &= NullTolong(txtGudang.EditValue) & ","
                SQL &= FixKoma(txtKonversi.EditValue) & ""
                SQL &= ")"
            Else
                SQL = "UPDATE MPOD SET "
                SQL &= " Tgl=GetDate(),"
                SQL &= " Jam=GetDate(),"
                SQL &= " IDPO=" & IDPO & ","
                SQL &= " IDBarang=" & NullTolong(txtPegawai.EditValue) & ","
                SQL &= " IDSatuan=" & NullTolong(txtSatuan.EditValue) & ","
                SQL &= " Qty=" & FixKoma(txtQty.EditValue) & ","
                SQL &= " QtyPcs=" & FixKoma(txtQty.EditValue * txtKonversi.EditValue) & ","
                SQL &= " Harga=" & FixKoma(txtHarga.EditValue) & ","
                SQL &= " HargaPcs=" & FixKoma((txtHarga.EditValue - txtDiscRp1.EditValue - txtDiscRp2.EditValue - txtDiscRp1.EditValue - (txtHarga.EditValue * txtDiscPersen1.EditValue / 100) - (txtHarga.EditValue * txtDiscPersen2.EditValue / 100) - (txtHarga.EditValue * txtDiscPersen3.EditValue / 100)) / txtKonversi.EditValue) & ","
                SQL &= " CTN=" & NullToDbl(txtCtn.EditValue) & ","
                SQL &= " DiscPersen1=" & FixKoma(txtDiscPersen1.EditValue) & ","
                SQL &= " DiscPersen2=" & FixKoma(txtDiscPersen2.EditValue) & ","
                SQL &= " DiscPersen3=" & FixKoma(txtDiscPersen3.EditValue) & ","
                SQL &= " Disc1=" & FixKoma(txtDiscRp1.EditValue) & ","
                SQL &= " Disc2=" & FixKoma(txtDiscRp2.EditValue) & ","
                SQL &= " Disc3=" & FixKoma(txtDiscRp3.EditValue) & ","
                SQL &= " Jumlah=" & FixKoma(txtJUmlah.EditValue) & ","
                SQL &= " Catatan='" & FixApostropi(txtCatatan.Text) & "',"
                SQL &= " IDGudang=" & NullTolong(txtGudang.EditValue) & ","
                SQL &= " Konversi=" & FixKoma(txtKonversi.EditValue) & ""
                SQL &= " WHERE NoID=" & NoID
            End If
            If EksekusiSQL(SQL) >= 1 Then
                Return True
            Else
                Return False
            End If
        Catch ex As Exception
            XtraMessageBox.Show("Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return False
        End Try
    End Function
    Private Function IsValidasi() As Boolean
        If txtPegawai.Text = "" Then
            XtraMessageBox.Show("Barang masih kosong.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
            txtPegawai.Focus()
            Return False
            Exit Function
        End If
        If txtSatuan.Text = "" Then
            XtraMessageBox.Show("Satuan masih kosong.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
            txtSatuan.Focus()
            Return False
            Exit Function
        End If
        If txtGudang.Text = "" Then
            XtraMessageBox.Show("Gudang masih kosong.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
            txtGudang.Focus()
            Return False
            Exit Function
        End If
        If txtQty.EditValue = 0 Then
            XtraMessageBox.Show("Qty masih 0.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
            txtQty.Focus()
            Return False
            Exit Function
        End If
        Return True
    End Function
    Private Sub cmdTutup_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdTutup.Click
        DialogResult = Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub BarButtonItem1_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles cmdSaveLayout.ItemClick
        Try
            If XtraMessageBox.Show("Simpan Layout?", NamaAplikasi, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = Windows.Forms.DialogResult.Yes Then
                Ini.TulisIniPath(Application.StartupPath & "\system\Layouts\" & Me.Name & ".ini", "Form", "Width", Me.Width)
                Ini.TulisIniPath(Application.StartupPath & "\system\Layouts\" & Me.Name & ".ini", "Form", "Height", Me.Height)
                Me.Left = (Screen.PrimaryScreen.WorkingArea.Width - Me.Width) \ 2
                Me.Top = (Screen.PrimaryScreen.WorkingArea.Height - Me.Height) \ 2

                LayoutControl1.SaveLayoutToXml(Application.StartupPath & "\System\Layouts\" & Me.Name & LayoutControl1.Name & ".xml")
                gvBarang.SaveLayoutToXml(Application.StartupPath & "\System\Layouts\" & Me.Name & gvBarang.Name & ".xml")
                gvGudang.SaveLayoutToXml(Application.StartupPath & "\System\Layouts\" & Me.Name & gvGudang.Name & ".xml")
                gvSatuan.SaveLayoutToXml(Application.StartupPath & "\System\Layouts\" & Me.Name & gvSatuan.Name & ".xml")
            End If
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical)
        End Try
    End Sub

    Private Sub frmEntriPOD_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing

    End Sub

    Private Sub frmEntriPOD_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim dlg As DevExpress.Utils.WaitDialogForm = Nothing
        Try
            Dim curentcursor As Cursor = Windows.Forms.Cursor.Current
            Windows.Forms.Cursor.Current = Cursors.WaitCursor
            dlg = New DevExpress.Utils.WaitDialogForm(String.Format("Creating component and analize database.{0}MOHON TUNGGU ...", vbCrLf), Application.ProductName)
            dlg.TopMost = False
            dlg.Show()
            Windows.Forms.Cursor.Current = Cursors.WaitCursor
            IsiDefault()
            If Not IsNew Then
                LoadData()
            End If
            Me.Width = Ini.BacaIniPath(Application.StartupPath & "\system\Layouts\" & Me.Name & ".ini", "Form", "Width", Me.Width)
            Me.Height = Ini.BacaIniPath(Application.StartupPath & "\system\Layouts\" & Me.Name & ".ini", "Form", "Height", Me.Height)

            Me.Left = (Screen.PrimaryScreen.WorkingArea.Width - Me.Width) \ 2
            Me.Top = (Screen.PrimaryScreen.WorkingArea.Height - Me.Height) \ 2

            If System.IO.File.Exists(Application.StartupPath & "\System\Layouts\" & Me.Name & LayoutControl1.Name & ".xml") Then
                LayoutControl1.RestoreLayoutFromXml(Application.StartupPath & "\System\Layouts\" & Me.Name & LayoutControl1.Name & ".xml")
            End If
        Catch ex As Exception
            XtraMessageBox.Show(ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            Windows.Forms.Cursor.Current = Cursors.Default
            dlg.Close()
            dlg.Dispose()
        End Try
    End Sub

    Private Sub HitungJumlah()
        Try
            Dim SubTotal As Double = (txtHarga.EditValue - txtDiscRp1.EditValue - txtDiscRp2.EditValue - txtDiscRp3.EditValue)
            Dim DiscA As Double = txtHarga.EditValue * txtQty.EditValue * txtDiscPersen1.EditValue / 100
            Dim DiscB As Double = txtHarga.EditValue * txtQty.EditValue * txtDiscPersen2.EditValue / 100
            Dim DiscC As Double = txtHarga.EditValue * txtQty.EditValue * txtDiscPersen3.EditValue / 100
            txtJUmlah.EditValue = (SubTotal * txtQty.EditValue) - DiscA - DiscB - DiscC
        Catch ex As Exception

        End Try
    End Sub

    Private Sub txtQty_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtQty.LostFocus
        HitungJumlah()
    End Sub

    Private Sub txtDiscPersen1_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtDiscPersen1.LostFocus
        HitungJumlah()
    End Sub
    Private Sub txtDiscPersen2_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtDiscPersen2.LostFocus
        HitungJumlah()
    End Sub
    Private Sub txtDiscPersen3_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtDiscPersen3.LostFocus
        HitungJumlah()
    End Sub
    Private Sub txtDisc1_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtDiscRp1.LostFocus
        HitungJumlah()
    End Sub
    Private Sub txtDisc2_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtDiscRp2.LostFocus
        HitungJumlah()
    End Sub
    Private Sub txtDisc3_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtDiscRp3.LostFocus
        HitungJumlah()
    End Sub

    Private Sub txtBarang_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtPegawai.EditValueChanged
        Dim Ds As New DataSet
        Try
            SQL = "SELECT MBarang.IDSatuan, MBarang.Nama AS NamaBarang, MSatuan.Konversi FROM MBarang LEFT JOIN MSatuan ON MSatuan.NoID=MBarang.IDSatuan WHERE MBarang.NoID=" & NullTolong(txtPegawai.EditValue)
            Ds = ExecuteDataset("Tabel", SQL)
            If Ds.Tables("Tabel").Rows.Count >= 1 Then
                txtSatuan.EditValue = NullTolong(Ds.Tables(0).Rows(0).Item("IDSatuan"))
                txtKonversi.EditValue = NullToDbl(Ds.Tables(0).Rows(0).Item("Konversi"))
                txtNama.Text = NullTostr(Ds.Tables(0).Rows(0).Item("NamaBarang"))
                txtHarga.EditValue = NullToDbl(EksekusiSQlSkalarNew("SELECT TOP 1 MBeliD.Harga FROM MBeliD INNER JOIN MBeli ON MBeli.NoID=MBeliD.IDBeli WHERE MBeliD.IDBarang=" & NullTolong(txtPegawai.EditValue) & " Order By MBeli.Tanggal Desc, MBeliD.Tgl Desc"))
            End If
            RefreshLookUp()
        Catch ex As Exception
            XtraMessageBox.Show("Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            Ds.Dispose()
        End Try
    End Sub

    Private Sub txtSatuan_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtSatuan.EditValueChanged
        Dim Ds As New DataSet
        Try
            SQL = "SELECT MBarangD.Konversi FROM MBarangD LEFT JOIN MSatuan ON MSatuan.NoID=MBarangD.IDSatuan WHERE MBarangD.IDBarang=" & NullTolong(txtPegawai.EditValue) & " AND MSatuan.NoID=" & NullTolong(txtSatuan.EditValue)
            Ds = ExecuteDataset("Tabel", SQL)
            If Ds.Tables("Tabel").Rows.Count >= 1 Then
                txtKonversi.EditValue = NullToDbl(Ds.Tables(0).Rows(0).Item("Konversi"))
                txtCtn.EditValue = txtQty.EditValue / IIf(txtKonversi.EditValue = 0, 1, txtKonversi.EditValue)
            End If
            HitungJumlah()
        Catch ex As Exception
            XtraMessageBox.Show("Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            Ds.Dispose()
        End Try
    End Sub

    Private Sub BarButtonItem1_ItemClick_1(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem1.ItemClick
        cmdSave.PerformClick()
    End Sub

    Private Sub BarButtonItem2_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem2.ItemClick
        cmdTutup.PerformClick()
    End Sub

    Private Sub txtGudang_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtGudang.EditValueChanged
        Dim ds As New DataSet
        Try
            ds = ExecuteDataset("MWilayah", "SELECT MWilayah.Nama AS Wilayah FROM MGudang LEFT JOIN MWilayah ON MWilayah.NoID=mGudang.IDWilayah WHERE MGudang.NoID=" & NullTolong(txtGudang.EditValue))
            If ds.Tables(0).Rows.Count >= 1 Then
                txtKode.Text = NullTostr(ds.Tables(0).Rows(0).Item("Wilayah"))
            End If
        Catch ex As Exception
            XtraMessageBox.Show("Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            ds.Dispose()
        End Try
    End Sub
End Class