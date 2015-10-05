Imports System.Data
Imports System.Data.SqlClient
Imports DevExpress.XtraEditors
Imports DevExpress.Utils
Imports DevExpress.XtraGrid.Views.Base

Public Class frmEntriTransferPoin
    Dim SQL As String = ""
    Public Enum pStatus
        Baru = 0
        Edit = 1
    End Enum
    Public pTipe As pStatus
    Public IsPosted As Boolean = False
    Public NoID As Long = -1
    Dim oda2 As SqlDataAdapter
    Dim KodeLama As String = ""
    Dim oDS As New DataSet
    Dim BS As New BindingSource
    Dim DefImageList As New ImageList
    Dim IDAdmin As Long = -1
    Dim IDWilayah As Long = DefIDWilayah

    ''For Back Action
    'Public FormNameDaftar As String = ""
    'Public TableNameDaftar As String = ""
    'Public TextDaftar As String = ""
    'Public FormEntriDaftar As String = ""
    'Public TableMasterDaftar As String = ""

    Private Sub RefreshDataKontak()
        Dim ds As New DataSet
        Try
            SQL = "SELECT NoID, Kode, Nama FROM MAlamat WHERE IsActive=1 AND IsCustomer=1"
            ds = ExecuteDataset("master", SQL)
            txtKodeCustomerLama.Properties.DataSource = ds.Tables("master")
            txtKodeCustomerLama.Properties.DisplayMember = "Kode"
            txtKodeCustomerLama.Properties.ValueMember = "NoID"
            If System.IO.File.Exists(folderLayouts & Me.Name & gvMemberLama.Name & ".xml") Then
                gvMemberLama.RestoreLayoutFromXml(folderLayouts & Me.Name & gvMemberLama.Name & ".xml")
            End If
            txtKodeCustomerBaru.Properties.DataSource = ds.Tables("master")
            txtKodeCustomerBaru.Properties.DisplayMember = "Kode"
            txtKodeCustomerBaru.Properties.ValueMember = "NoID"
            If System.IO.File.Exists(FolderLayouts & Me.Name & gvMemberBaru.Name & ".xml") Then
                gvMemberBaru.RestoreLayoutFromXml(FolderLayouts & Me.Name & gvMemberBaru.Name & ".xml")
            End If

        Catch ex As Exception
            XtraMessageBox.Show("Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            ds.Dispose()
        End Try
    End Sub

    Private Sub frmEntriBeli_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing

    End Sub

    Private Sub frmEntriBeli_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim dlg As DevExpress.Utils.WaitDialogForm = Nothing
        Try
            Dim curentcursor As Cursor = Windows.Forms.Cursor.Current
            Windows.Forms.Cursor.Current = Cursors.WaitCursor
            dlg = New DevExpress.Utils.WaitDialogForm(String.Format("Creating component and analize database.{0}MOHON TUNGGU ...", vbCrLf), Application.ProductName)
            dlg.TopMost = False
            dlg.Show()
            Windows.Forms.Cursor.Current = Cursors.WaitCursor
            SetTombol()
            RefreshDataKontak()
            RefreshData()
            SetTombol()
            Me.Width = Ini.BacaIniPath(folderLayouts & Me.Name & ".ini", "Form", "Width", Me.Width)
            Me.Height = Ini.BacaIniPath(folderLayouts & Me.Name & ".ini", "Form", "Height", Me.Height)

            Me.Left = (Screen.PrimaryScreen.WorkingArea.Width - Me.Width) \ 2
            Me.Top = (Screen.PrimaryScreen.WorkingArea.Height - Me.Height) \ 2

            If System.IO.File.Exists(folderLayouts & Me.Name & LayoutControl1.Name & ".xml") Then
                LayoutControl1.RestoreLayoutFromXml(folderLayouts & Me.Name & LayoutControl1.Name & ".xml")
            End If
            If System.IO.File.Exists(folderLayouts & Me.Name & gvMemberLama.Name & ".xml") Then
                gvMemberLama.RestoreLayoutFromXml(folderLayouts & Me.Name & gvMemberLama.Name & ".xml")
            End If
            If System.IO.File.Exists(FolderLayouts & Me.Name & gvMemberBaru.Name & ".xml") Then
                gvMemberBaru.RestoreLayoutFromXml(FolderLayouts & Me.Name & gvMemberBaru.Name & ".xml")
            End If
            FungsiControl.SetForm(Me)
            HighLightTxt()
            Me.Text = "Transfer Poin"
            Tgl.Properties.EditMask = "dd-MM-yyyy HH:mm"
        Catch ex As Exception
            XtraMessageBox.Show(ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            Windows.Forms.Cursor.Current = Cursors.Default
            dlg.Close()
            dlg.Dispose()
        End Try
    End Sub
    Sub HighLightTxt()
        For Each ctrl In LayoutControl1.Controls
            If TypeOf ctrl Is DevExpress.XtraEditors.TextEdit Then
                AddHandler TryCast(ctrl, DevExpress.XtraEditors.TextEdit).GotFocus, AddressOf txt_GotFocus
            End If
        Next
    End Sub
    Private Sub txt_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim txt As DevExpress.XtraEditors.TextEdit = TryCast(sender, DevExpress.XtraEditors.TextEdit)
        If txt.Properties.Mask.MaskType = Mask.MaskType.Numeric Then
            txt.SelectAll()
        End If
    End Sub
    Private Sub RefreshData()
        Dim DS As New DataSet
        Try
            'RefreshDataKontak()
            If pTipe = pStatus.Baru Then
                IsiDefault()
            Else
                SQL = "SELECT MTransferPoin.* FROM MTransferPoin LEFT JOIN MAlamat MCustomerLama ON MCustomerLama.NoID=MTransferPoin.IDCustomerLama LEFT JOIN MAlamat MCustomerBaru ON MCustomerBaru.NoID=MTransferPoin.IDCustomerBaru WHERE MTransferPoin.NoID=" & NoID
                DS = ExecuteDataset("Tabel", SQL)
                If DS.Tables("Tabel").Rows.Count >= 1 Then
                    txtKode.Text = NullToStr(DS.Tables(0).Rows(0).Item("Kode"))
                    Tgl.DateTime = NullToDate(DS.Tables(0).Rows(0).Item("Tanggal"))
                    txtKodeCustomerLama.EditValue = NullToLong(DS.Tables(0).Rows(0).Item("IDCustomerLama"))
                    txtKodeCustomerBaru.EditValue = NullToLong(DS.Tables(0).Rows(0).Item("IDCustomerBaru"))
                    txtJumlahPoin.EditValue = NullToDbl(DS.Tables(0).Rows(0).Item("JumlahPoin"))
                    txtPoin.EditValue = NullToDbl(DS.Tables(0).Rows(0).Item("PoinTransfer"))
                    txtKeterangan.Text = NullToStr(DS.Tables(0).Rows(0).Item("Keterangan"))
                    IsPosted = NullToBool(DS.Tables(0).Rows(0).Item("IsPosted"))
                    If IsPosted Then
                        txtKodeCustomerLama.Properties.ReadOnly = True
                        txtKodeCustomerBaru.Properties.ReadOnly = True
                        cmdSave.Enabled = False
                    End If
                Else
                    IsiDefault()
                End If
            End If
        Catch ex As Exception
            XtraMessageBox.Show("Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            DS.Dispose()
        End Try
    End Sub

    Private Sub IsiDefault()
        Tgl.DateTime = TanggalSystem
        SetTombol()
    End Sub

    Private Sub BarButtonItem1_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem1.ItemClick
        cmdSave.PerformClick()
    End Sub

    Private Sub cmdTutup_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdTutup.Click
        DialogResult = Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub
    Private Sub SetTombol()
        DefImageList = frmMain.ImageList1

        cmdSave.ImageList = DefImageList
        cmdSave.ImageIndex = 6

        cmdTutup.ImageList = DefImageList
        cmdTutup.ImageIndex = 3

    End Sub

    Private Sub cmdSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSave.Click
        Dim PesanSalah As String = ""
        If Not IsPosted Then
            If IsValidasi() Then
                If Simpan(PesanSalah) = True Then
                    clsPostingPembelian.PostingTransferPoin(NoID)
                    DialogResult = Windows.Forms.DialogResult.OK
                    Close()
                    Dispose()
                Else
                    XtraMessageBox.Show("Info Kesalahan : " & vbCrLf & PesanSalah, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                End If
            End If
        End If
    End Sub
    Private Function IsValidasi() As Boolean
        If txtKode.Text = "" Then
            XtraMessageBox.Show("Kode masih kosong.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
            txtKode.Focus()
            Return False
            Exit Function
        End If
        If txtKodeCustomerLama.Text = "" Then
            XtraMessageBox.Show("Member lama masih kosong.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
            txtKodeCustomerLama.Focus()
            Return False
            Exit Function
        End If
        If txtKodeCustomerBaru.Text = "" Then
            XtraMessageBox.Show("Member baru masih kosong.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
            txtKodeCustomerBaru.Focus()
            Return False
            Exit Function
        End If
        If Tgl.Text = "" Then
            XtraMessageBox.Show("Tanggal masih kosong.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
            Tgl.Focus()
            Return False
            Exit Function
        End If
        If txtPoin.EditValue <= 0 Then
            XtraMessageBox.Show("Nilai Poin yang ditukar tidak boleh kurang dari nol.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
            txtPoin.Focus()
            Return False
            Exit Function
        End If
        If txtJumlahPoin.EditValue - txtPoin.EditValue < 0 Then
            XtraMessageBox.Show("Nilai Poin yang ditukar melebihi jumlah poin.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
            txtPoin.Focus()
            Return False
            Exit Function
        End If
        If CekKodeValid(txtKode.Text, KodeLama, "MTransferPoin", "Kode", IIf(pTipe = pStatus.Edit, True, False)) Then
            XtraMessageBox.Show("Kode sudah dipakai.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
            txtKode.Focus()
            Return False
            Exit Function
        End If

        Return True
    End Function
    Public Function Simpan(ByRef PesanSalah As String) As Boolean
        Dim Sukses As Boolean = False
        Try
            If pTipe = pStatus.Baru Then
                NoID = GetNewID("MTransferPoin")
                SQL = "INSERT INTO [MTransferPoin] ([NoID],[Tanggal],[Jam],[Kode],[IDWilayah],[IDCustomerLama],[IDCustomerBaru],[JumlahPoin],[PoinTransfer],[Saldo],[Keterangan],[IsPosted],[IDUserEntri],[IDUserEdit]) VALUES (" & _
                      NoID & "," & _
                      "'" & Tgl.DateTime.ToString("yyyy-MM-dd HH:mm") & "'," & _
                      "'" & Tgl.DateTime.ToString("yyyy-MM-dd HH:mm") & "'," & _
                      "'" & FixApostropi(txtKode.Text) & "'," & _
                      IDWilayah & "," & _
                      NullToLong(txtKodeCustomerLama.EditValue) & "," & _
                      NullToLong(txtKodeCustomerBaru.EditValue) & "," & _
                      FixKoma(txtJumlahPoin.EditValue) & "," & _
                      FixKoma(txtPoin.EditValue) & "," & _
                      FixKoma(txtJumlahPoin.EditValue - txtPoin.EditValue) & "," & _
                      "'" & FixApostropi(txtKeterangan.EditValue) & "', " & _
                      "0, " & _
                      IDUserAktif & ",NULL)"
            Else
                SQL = "UPDATE [MTransferPoin] SET " & _
                      " [Tanggal]='" & Tgl.DateTime.ToString("yyyy-MM-dd HH:mm") & "'," & _
                      " [Jam]='" & Tgl.DateTime.ToString("yyyy-MM-dd HH:mm") & "'," & _
                      " [Kode]='" & FixApostropi(txtKode.Text) & "'," & _
                      " [IDCustomerLama]=" & NullToLong(txtKodeCustomerLama.EditValue) & "," & _
                      " [IDCustomerBaru]=" & NullToLong(txtKodeCustomerBaru.EditValue) & "," & _
                      " [JumlahPoin]=" & FixKoma(txtJumlahPoin.EditValue) & "," & _
                      " [PoinTransfer]=" & FixKoma(txtPoin.EditValue) & "," & _
                      " [Saldo]=" & FixKoma(txtJumlahPoin.EditValue - txtPoin.EditValue) & "," & _
                      " [Keterangan]='" & FixApostropi(txtKeterangan.EditValue) & "', " & _
                      " [IDUserEdit]=NULL " & _
                      " WHERE NoID=" & NoID
            End If
            If EksekusiSQL(SQL) >= 1 Then
                Sukses = True
            Else
                Sukses = False
            End If
        Catch ex As Exception
            PesanSalah = ex.Message
            Sukses = False
        Finally

        End Try
        Return Sukses
    End Function

    Private Sub BarButtonItem3_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem3.ItemClick
        Dim xOtorisasi As New frmOtorisasiAdmin
        Try
            If XtraMessageBox.Show("Simpan Layout?", NamaAplikasi, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = Windows.Forms.DialogResult.Yes AndAlso xOtorisasi.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
                Ini.TulisIniPath(FolderLayouts & Me.Name & ".ini", "Form", "Width", Me.Width)
                Ini.TulisIniPath(FolderLayouts & Me.Name & ".ini", "Form", "Height", Me.Height)
                Me.Left = (Screen.PrimaryScreen.WorkingArea.Width - Me.Width) \ 2
                Me.Top = (Screen.PrimaryScreen.WorkingArea.Height - Me.Height) \ 2

                LayoutControl1.SaveLayoutToXml(FolderLayouts & Me.Name & LayoutControl1.Name & ".xml")

                gvMemberLama.SaveLayoutToXml(FolderLayouts & Me.Name & gvMemberLama.Name & ".xml")
                gvMemberBaru.SaveLayoutToXml(FolderLayouts & Me.Name & gvMemberBaru.Name & ".xml")
            End If
        Catch ex As Exception
            XtraMessageBox.Show("Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            xOtorisasi.Dispose()
        End Try
    End Sub

    Private Sub txtKodeCustomer_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtKodeCustomerLama.EditValueChanged
        RubahCustomer()
        'SQL = "SELECT " & _
        '      " IsNull((SELECT SUM(MJual.NilaiPoin) FROM MJual WHERE MJual.IDCustomer=" & NullToLong(txtKodeCustomerLama.EditValue) & "),0)-" & _
        '      " IsNull((SELECT SUM(MTukarPoin.Kredit) FROM MTukarPoin WHERE MTukarPoin.IDMember=" & NullToLong(txtKodeCustomerLama.EditValue) & "),0)" & _
        '      " AS POIN"
        SQL = "SELECT vSaldoPoin.SaldoPoin FROM vSaldoPoin WHERE vSaldoPoin.IDCustomer=" & NullToLong(txtKodeCustomerLama.EditValue)
        txtJumlahPoin.EditValue = NullToDbl(EksekusiSQlSkalarNew(SQL))
    End Sub
    Private Sub RubahCustomer()
        Try
            txtNamaCustomerLama.Text = NullToStr(EksekusiSQlSkalarNew("SELECT Nama FROM MAlamat WHERE NoID=" & NullToLong(txtKodeCustomerLama.EditValue)))
            txtAlamatCustomerLama.Text = NullToStr(EksekusiSQlSkalarNew("SELECT Alamat FROM MAlamat WHERE NoID=" & NullToLong(txtKodeCustomerLama.EditValue)))
            txtNamaCustomerBaru.Text = NullToStr(EksekusiSQlSkalarNew("SELECT Nama FROM MAlamat WHERE NoID=" & NullToLong(txtKodeCustomerBaru.EditValue)))
            txtAlamatCustomerBaru.Text = NullToStr(EksekusiSQlSkalarNew("SELECT Alamat FROM MAlamat WHERE NoID=" & NullToLong(txtKodeCustomerBaru.EditValue)))
        Catch ex As Exception

        End Try
    End Sub

    Private Sub Tgl_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Tgl.EditValueChanged
        Try
            If pTipe = pStatus.Baru Then
                txtKode.Text = clsKode.MintaKodeBaru("TP", "MTransferPoin", Tgl.DateTime, IDWilayah, 5)
            End If
        Catch ex As Exception

        End Try
    End Sub

    Private Sub BarButtonItem7_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem7.ItemClick
        cmdTutup.PerformClick()
    End Sub

    Private Sub txtKode_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtKode.KeyDown
        If e.KeyCode = Keys.F7 Then
            Dim x As New frmOtorisasiAdmin
            Try
                If txtKode.Properties.ReadOnly Then
                    If x.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
                        IDAdmin = x.IDUserAdmin
                        txtKode.Properties.ReadOnly = False
                    End If
                End If

            Catch ex As Exception
                XtraMessageBox.Show("Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        ElseIf e.KeyCode = Keys.Enter Then
            If pTipe = pStatus.Baru Then
                If NullToLong(EksekusiSQlSkalarNew("select NoID from MTransferPoin WHERE Kode='" & FixApostropi(txtKode.Text) & "'")) > 0 Then
                    MsgBox("Kode Sudah Ada!", MsgBoxStyle.Exclamation)
                End If
            Else
                If NullToLong(EksekusiSQlSkalarNew("select NoID from MTransferPoin WHERE Kode<>'" & FixApostropi(KodeLama) & " and Kode='" & FixApostropi(txtKode.Text) & "'")) > 0 Then
                    MsgBox("Kode Sudah Ada!", MsgBoxStyle.Exclamation)
                End If
            End If
        End If
    End Sub

    Private Sub gvMemberLama_DataSourceChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvMemberLama.DataSourceChanged
        If System.IO.File.Exists(FolderLayouts & Me.Name & gvMemberLama.Name & ".xml") Then
            gvMemberLama.RestoreLayoutFromXml(FolderLayouts & Me.Name & gvMemberLama.Name & ".xml")
        End If
        With gvMemberLama
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
                            .Columns(x).DisplayFormat.FormatString = "HH:mm"
                        Else
                            .Columns(x).DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime
                            .Columns(x).DisplayFormat.FormatString = "dd-MM-yyyy"
                        End If
                End Select
            Next
        End With
    End Sub
    Private Sub gvMemberBaru_DataSourceChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvMemberBaru.DataSourceChanged
        If System.IO.File.Exists(FolderLayouts & Me.Name & gvMemberBaru.Name & ".xml") Then
            gvMemberBaru.RestoreLayoutFromXml(FolderLayouts & Me.Name & gvMemberBaru.Name & ".xml")
        End If
        With gvMemberBaru
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
                            .Columns(x).DisplayFormat.FormatString = "HH:mm"
                        Else
                            .Columns(x).DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime
                            .Columns(x).DisplayFormat.FormatString = "dd-MM-yyyy"
                        End If
                End Select
            Next
        End With
    End Sub

    Private Sub txtKodeCustomerBaru_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtKodeCustomerBaru.EditValueChanged
        RubahCustomer()
    End Sub
End Class