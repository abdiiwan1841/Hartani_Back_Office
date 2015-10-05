Imports System.Data
Imports System.Data.SqlClient
Imports DevExpress.XtraEditors
Imports DevExpress.Utils
Imports DevExpress.XtraGrid.Views.Base

Public Class frmEntriPembayaranHutangOK
    Public IsNew As Boolean = False
    Public IsJual As Boolean = False
    Dim SQL As String = ""
    Public Enum pStatus
        Baru = 0
        Edit = 1
    End Enum
    Public pTipe As pStatus
    Public Enum pTransaksi
        Pembelian = 0
        Penjualan = 1
    End Enum
    Public pTrans As pTransaksi

    Public IsSaveSelesai As Boolean = False
    Public IsPosted As Boolean = False
    Public NoID As Long = -1
    Dim oda2 As SqlDataAdapter

    Dim oDS As New DataSet
    Dim BS As New BindingSource
    'For Back Action
    Public FormNameDaftar As String = ""
    Public TableNameDaftar As String = ""
    Public TextDaftar As String = ""
    Public FormEntriDaftar As String = ""
    Public TableMasterDaftar As String = ""
    Dim IDAdmin As Long = -1
    Dim IsTempInsertBaru As Boolean = False
    Dim RepLookUpBank As New Repository.RepositoryItemSearchLookUpEdit
    Dim repckedit As New Repository.RepositoryItemCheckEdit
    Dim reppicedit As New Repository.RepositoryItemPictureEdit

    Private Sub RefreshDataKontak()
        Dim ds As New DataSet
        Try
            If Not IsJual Then
                SQL = "SELECT NoID, Kode, Nama, Alamat FROM MAlamat WHERE IsActive=1 AND IsSupplier=1"
            ElseIf IsJual Then
                SQL = "SELECT NoID, Kode, Nama, Alamat FROM MAlamat WHERE IsActive=1 AND IsCustomer=1"
            Else
                SQL = "SELECT NoID, Kode, Nama, Alamat FROM MAlamat WHERE IsActive=1"
            End If
            ds = ExecuteDataset("master", SQL)
            txtKodeAlamat.Properties.DataSource = ds.Tables("master")
            If System.IO.File.Exists(FolderLayouts & Me.Name & SearchLookUpEdit1View.Name & ".xml") Then
                SearchLookUpEdit1View.RestoreLayoutFromXml(FolderLayouts & Me.Name & SearchLookUpEdit1View.Name & ".xml")
            End If

            SQL = "SELECT NoID, Kode, Nama FROM MWilayah WHERE IsActive=1"
            ds = ExecuteDataset("master", SQL)
            txtWilayah.Properties.DataSource = ds.Tables("master")
            txtWilayah.Properties.ValueMember = "NoID"
            txtWilayah.Properties.DisplayMember = "Nama"
            If System.IO.File.Exists(FolderLayouts & Me.Name & gvWilayah.Name & ".xml") Then
                gvWilayah.RestoreLayoutFromXml(FolderLayouts & Me.Name & gvWilayah.Name & ".xml")
            End If

            SQL = "SELECT MAkun.ID, MAkun.Kode, MAkun.Nama, MSubklasAkun.Nama AS SubKlasifikasi, MKlasAkun.Nama as Klasifikasi, MKlasAkun.IsDebet " & _
                  "FROM ((MAkun LEFT JOIN MSubKlasAkun On MAkun.IDSubklasAkun=MSubKlasAkun.ID)" & _
                  "LEFT JOIN MKlasAkun On MSubKlasAkun.IDKlasAkun=MKlasAkun.ID)" & _
                  "LEFT JOIN MMataUang On MMataUang.ID=MAkun.IDMataUang"
            ds = ExecuteDataset("master", SQL)
            txtAkunKwitansi.Properties.DataSource = ds.Tables("master")
            txtAkunKwitansi.Properties.ValueMember = "ID"
            txtAkunKwitansi.Properties.DisplayMember = "Nama"
            If System.IO.File.Exists(FolderLayouts & Me.Name & SearchLookUpEdit1View.Name & ".xml") Then
                SearchLookUpEdit1View.RestoreLayoutFromXml(FolderLayouts & Me.Name & SearchLookUpEdit1View.Name & ".xml")
            End If

            SQL = "SELECT MBank.NoID, MBank.Kode AS KodeBank, MBank.Nama AS KasBank, MAkun.Kode + ' - ' + MAkun.Nama AS AkunKas " & _
                  " FROM MBank " & _
                  " LEFT JOIN MAkun ON MAkun.ID=MBank.IDAkun " & _
                  " WHERE MBank.IsActive=1 "
            ds = ExecuteDataset("master", SQL)
            txtBank.Properties.DataSource = ds.Tables("master")
            txtBank.Properties.ValueMember = "NoID"
            txtBank.Properties.DisplayMember = "KasBank"
            If System.IO.File.Exists(FolderLayouts & Me.Name & gvBank.Name & ".xml") Then
                gvBank.RestoreLayoutFromXml(FolderLayouts & Me.Name & gvBank.Name & ".xml")
            End If

            RepLookUpBank.DataSource = txtBank.Properties.DataSource
            RepLookUpBank.ValueMember = txtBank.Properties.ValueMember
            RepLookUpBank.DisplayMember = txtBank.Properties.DisplayMember

        Catch ex As Exception
            XtraMessageBox.Show("Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            ds.Dispose()
        End Try
    End Sub

    Dim DefImageList As New ImageList
    Private Sub frmEntriSO_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        If IsTempInsertBaru Then
            If Not NullToBool(EksekusiSQlSkalarNew("SELECT IsPosted FROM MBayarHutang WHERE NoID=" & NoID)) Then
                If XtraMessageBox.Show("Yakin ingin keluar?" & vbCrLf & "Ya, akan menghapus transaksi dan detil item.", NamaAplikasi, MessageBoxButtons.YesNo, MessageBoxIcon.Information, MessageBoxDefaultButton.Button2) = Windows.Forms.DialogResult.Yes Then
                    EksekusiSQL("DELETE FROM MBayarHutang WHERE NoID=" & NoID)
                    EksekusiSQL("DELETE FROM MBayarHutangD WHERE IDBayarHutang=" & NoID)
                Else
                    e.Cancel = True
                End If
            End If
        Else
            EksekusiSQL("UPDATE MBayarHutang SET IsEdit=0, UserEdit=NULL WHERE NoID=" & NoID)
        End If
    End Sub

    Private Sub frmEntriPO_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim dlg As DevExpress.Utils.WaitDialogForm = Nothing
        Try
            Dim curentcursor As Cursor = Windows.Forms.Cursor.Current
            Windows.Forms.Cursor.Current = Cursors.WaitCursor
            dlg = New DevExpress.Utils.WaitDialogForm(String.Format("Creating component and analize database.{0}MOHON TUNGGU ...", vbCrLf), Application.ProductName)
            dlg.TopMost = False
            dlg.Show()
            Windows.Forms.Cursor.Current = Cursors.WaitCursor
            Me.Width = Ini.BacaIniPath(FolderLayouts & Me.Name & ".ini", "Form", "Width", Me.Width)
            Me.Height = Ini.BacaIniPath(FolderLayouts & Me.Name & ".ini", "Form", "Height", Me.Height)

            Me.Left = (Screen.PrimaryScreen.WorkingArea.Width - Me.Width) \ 2
            Me.Top = (Screen.PrimaryScreen.WorkingArea.Height - Me.Height) \ 2

            If System.IO.File.Exists(FolderLayouts & Me.Name & LayoutControl1.Name & ".xml") Then
                LayoutControl1.RestoreLayoutFromXml(FolderLayouts & Me.Name & LayoutControl1.Name & ".xml")
            End If

            SetTombol()
            RefreshData()
            SetTombol()

            If System.IO.File.Exists(FolderLayouts & Me.Name & gvRetur.Name & ".xml") Then
                gvRetur.RestoreLayoutFromXml(FolderLayouts & Me.Name & gvRetur.Name & ".xml")
            End If
            If System.IO.File.Exists(FolderLayouts & Me.Name & gvPembelian.Name & ".xml") Then
                gvPembelian.RestoreLayoutFromXml(FolderLayouts & Me.Name & gvPembelian.Name & ".xml")
            End If
            If System.IO.File.Exists(FolderLayouts & Me.Name & gvDebet.Name & ".xml") Then
                gvDebet.RestoreLayoutFromXml(FolderLayouts & Me.Name & gvDebet.Name & ".xml")
            End If
            If System.IO.File.Exists(FolderLayouts & Me.Name & gvKredit.Name & ".xml") Then
                gvKredit.RestoreLayoutFromXml(FolderLayouts & Me.Name & gvKredit.Name & ".xml")
            End If
            If System.IO.File.Exists(FolderLayouts & Me.Name & gvGiro.Name & ".xml") Then
                gvGiro.RestoreLayoutFromXml(FolderLayouts & Me.Name & gvGiro.Name & ".xml")
            End If
            If System.IO.File.Exists(FolderLayouts & Me.Name & gviGiro.Name & ".xml") Then
                gviGiro.RestoreLayoutFromXml(FolderLayouts & Me.Name & gviGiro.Name & ".xml")
            End If
            If System.IO.File.Exists(FolderLayouts & Me.Name & gvTU.Name & ".xml") Then
                gvTU.RestoreLayoutFromXml(FolderLayouts & Me.Name & gvTU.Name & ".xml")
            End If
            If System.IO.File.Exists(FolderLayouts & Me.Name & gvPH.Name & ".xml") Then
                gvPH.RestoreLayoutFromXml(FolderLayouts & Me.Name & gvPH.Name & ".xml")
            End If
            If System.IO.File.Exists(FolderLayouts & Me.Name & SearchLookUpEdit1View.Name & ".xml") Then
                SearchLookUpEdit1View.RestoreLayoutFromXml(FolderLayouts & Me.Name & SearchLookUpEdit1View.Name & ".xml")
            End If
            gvKredit.OptionsCustomization.AllowSort = False
            gvDebet.OptionsCustomization.AllowSort = False
            gvKredit.Columns("NoID").SortOrder = DevExpress.Data.ColumnSortOrder.Ascending
            gvDebet.Columns("NoID").SortOrder = DevExpress.Data.ColumnSortOrder.Ascending

            Me.Text &= IIf(IsJual, " Customer", " Supplier")
            FungsiControl.SetForm(Me)
        Catch ex As Exception
            XtraMessageBox.Show(ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            Windows.Forms.Cursor.Current = Cursors.Default
            dlg.Close()
            dlg.Dispose()
        End Try
    End Sub
    Private Sub RefreshData()
        Dim DS As New DataSet
        Try
            RefreshDataKontak()
            If pTipe = pStatus.Baru Then
                IsiDefault()
            Else
                SQL = "SELECT MBayarHutang.*, MUserEntri.Nama AS UserEntri, MUserEdit.Nama AS UserEdit, MUserPosting.Nama AS UserPosted, MAlamat.Nama AS NamaAlamat, MAlamat.Kode as KodeAlamat "
                SQL &= " FROM MBayarHutang LEFT JOIN MAlamat ON MAlamat.NoID=MBayarHutang.IDAlamat "
                SQL &= " LEFT JOIN MUser MUserEntri ON MUserEntri.NoID=MBayarHutang.IDUserEntry "
                SQL &= " LEFT JOIN MUser MUserEdit ON MUserEdit.NoID=MBayarHutang.IDUserEdit "
                SQL &= " LEFT JOIN MUser MUserPosting ON MUserPosting.NoID=MBayarHutang.IDUserPosted "
                SQL &= " WHERE MBayarHutang.NoID = " & NoID
                DS = ExecuteDataset("Tabel", SQL)
                If DS.Tables("Tabel").Rows.Count >= 1 Then
                    txtKodeAlamat.EditValue = NullToLong(DS.Tables(0).Rows(0).Item("IDALamat"))
                    txtWilayah.EditValue = NullToLong(DS.Tables(0).Rows(0).Item("IDWilayah"))
                    txtNamaAlamat.Text = NullToStr(DS.Tables(0).Rows(0).Item("NamaAlamat"))
                    txtKode.Text = NullToStr(DS.Tables(0).Rows(0).Item("Kode"))
                    txtNoTT.EditValue = NullToLong(DS.Tables(0).Rows(0).Item("IDTT"))
                    KodeLama = txtKode.Text
                    Tgl.DateTime = NullToDate(DS.Tables(0).Rows(0).Item("Tanggal"))
                    txtSubTotal.EditValue = NullToDbl(DS.Tables(0).Rows(0).Item("SubTotal"))
                    txtKwitansi.EditValue = NullToLong(DS.Tables(0).Rows(0).Item("IDKwitansi"))
                    txtJumlahKwitansi.EditValue = NullToDbl(DS.Tables(0).Rows(0).Item("JumlahKwitansi"))
                    txtBayar.EditValue = NullToDbl(DS.Tables(0).Rows(0).Item("Total"))
                    txtPotongan.EditValue = NullToDbl(DS.Tables(0).Rows(0).Item("Potongan"))
                    txtDN.EditValue = NullToDbl(DS.Tables(0).Rows(0).Item("DN"))
                    txtMaterai.EditValue = NullToDbl(DS.Tables(0).Rows(0).Item("Materai"))
                    IsPosted = NullToBool(DS.Tables(0).Rows(0).Item("IsPosted"))
                    txtDientriOleh.Text = NullToStr(DS.Tables(0).Rows(0).Item("UserEntri"))
                    txtDieditOleh.Text = NullToStr(DS.Tables(0).Rows(0).Item("UserEdit"))
                    txtDipostingOleh.Text = NullToStr(DS.Tables(0).Rows(0).Item("UserPosted"))
                    tglEntri.DateTime = NullToDate(DS.Tables(0).Rows(0).Item("TglEntry"))
                    tglEdit.DateTime = NullToDate(DS.Tables(0).Rows(0).Item("TglEdit"))
                    tglPosting.DateTime = NullToDate(DS.Tables(0).Rows(0).Item("TglPosted"))
                    txtCatatan.Text = NullToStr(DS.Tables(0).Rows(0).Item("Catatan"))
                    txtBank.EditValue = NullToLong(DS.Tables(0).Rows(0).Item("IDBank"))
                    txtAkunKwitansi.EditValue = NullToLong(DS.Tables(0).Rows(0).Item("IDAkunKwitansiD"))
                    txtNoKwitansi.Text = NullToStr(DS.Tables(0).Rows(0).Item("NoKwitansi"))
                    txtKeteranganKwitansi.Text = NullToStr(DS.Tables(0).Rows(0).Item("KetKwitansi"))
                    txtTglKembali.DateTime = NullToDate(DS.Tables(0).Rows(0).Item("TglKembali"))
                    If IsPosted AndAlso Not IsSaveSelesai Then
                        txtKodeAlamat.Properties.ReadOnly = True
                        cmdSave.Enabled = False
                    ElseIf IsPosted AndAlso IsSaveSelesai Then
                        txtKodeAlamat.Properties.ReadOnly = True
                        cmdSave.Enabled = True
                    End If
                    EksekusiSQL("Update MBayarHutang SET IsEdit=1, UserEdit='" & FixApostropi(NamaUserAktif) & "' WHERE NoID=" & NoID)
                    HitungTotal()
                Else
                    IsiDefault()
                End If
                InsertkanKeDetil()
            End If
            If IsPosted Then
                gvPembelian.OptionsBehavior.Editable = False
                gvRetur.OptionsBehavior.Editable = False

                gvKredit.OptionsBehavior.Editable = False
                gvDebet.OptionsBehavior.Editable = False
                gviGiro.OptionsBehavior.Editable = False
            Else
                gvPembelian.OptionsBehavior.Editable = True
                gvRetur.OptionsBehavior.Editable = True

                gvKredit.OptionsBehavior.Editable = True
                gvDebet.OptionsBehavior.Editable = True
                gviGiro.OptionsBehavior.Editable = True
            End If
            RefreshDetil()
        Catch ex As Exception
            XtraMessageBox.Show("Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            InsertkanKeDetil()
            DS.Dispose()
        End Try
    End Sub
    Public Sub RefreshDetil()
        Dim strsql As String
        Dim curentcursor As Cursor = Windows.Forms.Cursor.Current
        Windows.Forms.Cursor.Current = Cursors.WaitCursor
        Try

            Dim oConn As New SqlConnection
            Dim ocmd As SqlCommand
            Dim oDA As SqlDataAdapter
            Dim oDS As New DataSet
            'Dim isAda As Boolean
            Windows.Forms.Cursor.Current = Cursors.WaitCursor
            strsql = "Select TBayarhutangD.*, MBeli.Kode as NoNota,TBayarhutangD.TglBeli AS Tanggal From TBayarhutangD inner join MBeli on TBayarHutangD.IDBeli=MBeli.NoID where IsJual=" & IIf(IsJual, 1, 0) & " and  TBayarHutangD.IDUser=" & IDUserAktif
            Try
                oConn.ConnectionString = StrKonSql
                ocmd = New SqlCommand(strsql, oConn)
                oConn.Open()
                oDA = New SqlDataAdapter(ocmd)
                oDA.Fill(oDS, "TBayarHutangD")
                'Retur
                strsql = "Select TBayarhutangDRetur.*,MReturBeli.Kode,TBayarhutangDRetur.TglRetur AS Tanggal From TBayarhutangDRetur inner join MReturBeli on TBayarhutangDRetur.IDReturBeli=MReturBeli.NoID where TBayarHutangDRetur.IsJual=" & IIf(IsJual, 1, 0) & " and  TBayarHutangDRetur.IDUser=" & IDUserAktif
                ocmd.CommandText = strsql
                oDA.Dispose()
                oDA = New SqlDataAdapter(ocmd)
                oDA.Fill(oDS, "TBayarHutangDRetur")
                'PH Harga
                strsql = "Select TBayarhutangDPH.*,MRevisiHargaBeli.Kode,MRevisiHargaBeli.Tanggal,MPHD.Total From TBayarhutangDPH inner join MRevisiHargaBeli on TBayarhutangDPH.IDRevisiHarga=MRevisiHargaBeli.NoID " & _
                "inner join (select IDRevisiHargaBeli,Sum(KoreksiBL) as Total from MRevisiHargaBeliD group by IDRevisiHargaBeli) MPHD on TBayarhutangDPH.IDRevisiHarga=MPHD.IDRevisiHargaBeli where TBayarHutangDPH.IsJual=" & IIf(IsJual, 1, 0) & " and  TBayarHutangDPH.IDUser=" & IDUserAktif
                ocmd.CommandText = strsql
                oDA.Dispose()
                oDA = New SqlDataAdapter(ocmd)
                oDA.Fill(oDS, "TBayarHutangDPH")

                'Nota Debet
                'strsql = "Select TBayarHutangDDebet.*,MDebetNote.Kode,MDebetNote.Tanggal,MDebetNote.Jumlah as Total,MDebetNote.Keterangan Catatan From TBayarHutangDDebet inner join MDebetNote on TBayarhutangDDebet.IDDebetNote=MDebetNote.NoID where TBayarHutangDDebet.IsJual=" & IIf(IsJual, 1, 0) & " and  TBayarHutangDDebet.IDUser=" & IDUserAktif
                strsql = "Select MBayarHutangDDebet.*, Potong AS Total From MBayarHutangDDebet WHERE MBayarHutangDDebet.IDBayarHutang=" & NoID
                ocmd.CommandText = strsql
                oDA.Dispose()
                oDA = New SqlDataAdapter(ocmd)
                oDA.Fill(oDS, "TBayarHutangDDebet")

                'Nota Kredit
                'strsql = "Select TBayarHutangDKredit.*,MCreditNote.Kode,MCreditNote.Tanggal,MCreditNote.Jumlah as Total,MCreditNote.Keterangan Catatan From TBayarHutangDKredit inner join MCreditNote on TBayarhutangDKredit.IDKreditNote=MCreditNote.NoID where TBayarHutangDKredit.IsJual=" & IIf(IsJual, 1, 0) & " and  TBayarHutangDKredit.IDUser=" & IDUserAktif
                strsql = "Select MBayarHutangDKredit.*, Potong AS Total From MBayarHutangDKredit WHERE MBayarHutangDKredit.IDBayarHutang=" & NoID
                ocmd.CommandText = strsql
                oDA.Dispose()
                oDA = New SqlDataAdapter(ocmd)
                oDA.Fill(oDS, "TBayarHutangDKredit")

                'Giro
                strsql = "Select MBayarHutangDGiro.* FROM MBayarHutangDGiro WHERE MBayarHutangDGiro.IDBayarHutang=" & NoID
                ocmd.CommandText = strsql
                oDA.Dispose()
                oDA = New SqlDataAdapter(ocmd)
                oDA.Fill(oDS, "TBayarHutangDGiro")

                With gviGiro
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
                        If .Columns(i).FieldName.ToUpper = "IDBank".ToUpper Then
                            .Columns(i).ColumnEdit = RepLookUpBank
                        End If
                    Next
                End With
            Catch ex As Exception
                XtraMessageBox.Show(Me, ex.Message, ".:: Pesan Kesalahan ::.", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Finally
                oConn.Close()
            End Try
            gcPembelian.DataSource = oDS.Tables("TBayarHutangD")
            gcRetur.DataSource = oDS.Tables("TBayarHutangDRetur")
            gcPH.DataSource = oDS.Tables("TBayarHutangDPH")
            gcDebet.DataSource = oDS.Tables("TBayarHutangDDebet")
            gcKredit.DataSource = oDS.Tables("TBayarHutangDKredit")
            gciGiro.DataSource = oDS.Tables("TBayarHutangDGiro")
            'cmdBAru.Visible = False
            'cmdEdit.Visible = False
            'cmdLoad.Visible = False
            'cmdDelete.Visible = False
            Windows.Forms.Cursor.Current = curentcursor
            Application.DoEvents()
            'strsql = "SELECT MBayarHutangD.*" & vbCrLf & _
            '         " FROM MBayarHutangD " & vbCrLf & _
            '         " INNER JOIN MBayarHutang ON MBayarHutang.NoID=MBayarHutangD.IDBayarHutang" & vbCrLf & _
            '         " WHERE MBayarHutang.NoID=" & NoID
            'ExecuteDBGrid(gcRetur, strsql & " AND IsNull(MBayarHutangD.IsRetur,0)=1 ", "NoID")
            'ExecuteDBGrid(gcPembelian, strsql & " AND IsNull(MBayarHutangD.IsBeli,0)=1 ", "NoID")
            'ExecuteDBGrid(gcTU, strsql & " AND IsNull(MBayarHutangD.IsUangMuka,0)=1 ", "NoID")
            'ExecuteDBGrid(gcDebet, strsql & " AND IsNull(MBayarHutangD.IsNotaDebet,0)=1 ", "NoID")
            'ExecuteDBGrid(gcKredit, strsql & " AND IsNull(MBayarHutangD.IsNotaKredit,0)=1 ", "NoID")
            'ExecuteDBGrid(gcPH, strsql & " AND IsNull(MBayarHutangD.IsPH,0)=1 ", "NoID")
            'ExecuteDBGrid(gcDebet, strsql & " AND IsNull(MBayarHutangD.IsNotaDebet,0)=1 ", "NoID")
            'ExecuteDBGrid(gcKredit, strsql & " AND IsNull(MBayarHutangD.IsNotaKredit,0)=1 ", "NoID")
            'ExecuteDBGrid(gcGiro, strsql & " AND IsNull(MBayarHutangD.IsGiro,0)=1 ", "NoID")

            'SetGV(gvRetur)
            'SetGV(gvPembelian)
            'SetGV(gvTU)
            'SetGV(gvDebet)
            'SetGV(gvKredit)
            'SetGV(gvPH)
            'SetGV(gvDebet)
            'SetGV(gvKredit)
            'SetGV(gvGiro)

            'HitungTotal()
        Catch ex As Exception
            XtraMessageBox.Show(ex.Message, ".:: Pesan Kesalahan ::.", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
        Windows.Forms.Cursor.Current = curentcursor
        Application.DoEvents()
    End Sub
    Private Sub SetGV(ByVal gv As DevExpress.XtraGrid.Views.Grid.GridView)
        Dim repChekEdit As New DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit
        Try
            With gv
                For x As Integer = 0 To .Columns.Count - 1
                    Select Case .Columns(x).ColumnType.Name.ToLower
                        Case "int32", "int64", "int"
                            .Columns(x).DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
                            .Columns(x).DisplayFormat.FormatString = "n0"
                        Case "decimal", "single", "money", "double"
                            Select Case .Columns(x).FieldName.Trim.ToLower
                                Case "discpersen1", "discpersen2", "discpersen3"
                                    .Columns(x).DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
                                    .Columns(x).DisplayFormat.FormatString = "n2"
                                Case "ctn"
                                    .Columns(x).DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
                                    .Columns(x).DisplayFormat.FormatString = "n3"
                                Case Else
                                    .Columns(x).DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
                                    .Columns(x).DisplayFormat.FormatString = "n2"
                            End Select
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
                        Case "boolean"
                            .Columns(x).ColumnEdit = repChekEdit
                    End Select
                Next
            End With
            gv.HideFindPanel()
        Catch ex As Exception
            XtraMessageBox.Show("Info Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
        End Try
    End Sub
    Private Sub HitungTotal()
        'Dim temp As Double = 0.0
        Try
            'For i = 0 To gvRetur.RowCount
            '    temp = temp - NullToDbl(gvRetur.GetRowCellValue(i, "Bayar"))
            'Next
            'For i = 0 To gvPembelian.RowCount
            '    temp = temp + NullToDbl(gvPembelian.GetRowCellValue(i, "Jumlah"))
            'Next
            'For i = 0 To gvPH.RowCount
            '    temp = temp - NullToDbl(gvPH.GetRowCellValue(i, "Jumlah"))
            'Next
            'For i = 0 To gvDebet.RowCount
            '    temp = temp - NullToDbl(gvDebet.GetRowCellValue(i, "Jumlah"))
            'Next
            'For i = 0 To gvKredit.RowCount
            '    temp = temp + NullToDbl(gvKredit.GetRowCellValue(i, "Jumlah"))
            'Next
            'For i = 0 To gvTU.RowCount
            '    temp = temp - NullToDbl(gvTU.GetRowCellValue(i, "Jumlah"))
            'Next
            'For i = 0 To gvGiro.RowCount
            '    temp = temp - NullToDbl(gvGiro.GetRowCellValue(i, "Jumlah"))
            'Next
            'Me.Refresh()

            gvPembelian.RefreshData()
            gvRetur.RefreshData()
            gvPH.RefreshData()
            gvGiro.RefreshData()
            gvTU.RefreshData()
            gvDebet.RefreshData()
            gvKredit.RefreshData()
            Application.DoEvents()

            txtSubTotal.EditValue = Bulatkan(NullToDbl(gvPembelian.Columns("Bayar").SummaryItem.SummaryValue), 0) - Bulatkan(NullToDbl(gvRetur.Columns("Potong").SummaryItem.SummaryValue), 0) - Bulatkan(NullToDbl(gvPH.Columns("Potong").SummaryItem.SummaryValue), 0)
            txtPotongan.EditValue = Bulatkan(NullToDbl(gvKredit.Columns("Potong").SummaryItem.SummaryValue), 0)
            txtDN.EditValue = Bulatkan(NullToDbl(gvDebet.Columns("Potong").SummaryItem.SummaryValue), 0)
            txtJumlah.EditValue = txtSubTotal.EditValue - txtJumlahKwitansi.EditValue - txtPotongan.EditValue - txtMaterai.EditValue + txtDN.EditValue
            txtPotongan.Properties.ReadOnly = True
            txtDN.Properties.ReadOnly = True
        Catch ex As Exception
            'XtraMessageBox.Show("Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub
    Private Sub IsiDefault()
        Tgl.DateTime = TanggalSystem
        txtTglKembali.DateTime = Tgl.DateTime
        txtWilayah.EditValue = DefIDWilayah
        txtKode.Text = clsKode.MintaKodeBaru("BY" & IIf(IsJual, "P", "H"), "MBayarHutang", Tgl.DateTime, NullToLong(txtWilayah.EditValue), 5, " IsJual=" & IIf(IsJual, 1, 0))

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

        cmdBAru.ImageList = DefImageList
        cmdBAru.ImageIndex = 1

        cmdEdit.ImageList = DefImageList
        cmdEdit.ImageIndex = 2

        cmdDelete.ImageList = DefImageList
        cmdDelete.ImageIndex = 4

        cmdRefresh.ImageList = DefImageList
        cmdRefresh.ImageIndex = 5

        cmdSave.ImageList = DefImageList
        cmdSave.ImageIndex = 6

        cmdTutup.ImageList = DefImageList
        cmdTutup.ImageIndex = 3

        If pTipe = pStatus.Baru Or IsPosted Then
            cmdBAru.Enabled = False
            cmdEdit.Enabled = False
            cmdDelete.Enabled = False
            cmdLoad.Enabled = False
        Else
            cmdBAru.Enabled = True
            cmdEdit.Enabled = True
            cmdDelete.Enabled = True
            cmdLoad.Enabled = True
        End If
    End Sub
    Private Sub SimpanDetil()
        Try
            Dim IDBeli As Long
            Dim Bayar As Double
            Dim IDDetil As Double
            'Pembelian-->Bayar HutangD
            SQL = "DELETE FROM MBayarHutangD WHERE IDBayarHutang=" & NoID
            EksekusiSQL(SQL)
            For i = 0 To gvPembelian.RowCount - 1
                IDBeli = gvPembelian.GetRowCellValue(i, "IDBeli")
                Bayar = NullToDbl(gvPembelian.GetRowCellValue(i, "Bayar"))
                IDDetil = GetNewID("MBayarHutangD")
                If Bayar <> 0 Then
                    EksekusiSQL("Insert Into MBayarHutangD(NoID,IDBayarHutang,IDBeli,Bayar,IsJual,Total,TglBeli) Values(" & _
                    IDDetil & "," & NoID & "," & IDBeli & "," & FixKoma(Bayar) & "," & IIf(IsJual, 1, 0) & "," & FixKoma(NullToDbl(gvPembelian.GetRowCellValue(i, "Total"))) & ",'" & NullToDate(gvPembelian.GetRowCellValue(i, "Tanggal")).ToString("yyyy-MM-dd HH:mm") & "')")
                End If
            Next
            'Retur Pembelian-->BayarHutangDRetur
            SQL = "DELETE FROM MBayarHutangDRetur WHERE IDBayarHutang=" & NoID
            EksekusiSQL(SQL)
            For i = 0 To gvRetur.RowCount - 1
                IDBeli = gvRetur.GetRowCellValue(i, "IDReturBeli")
                Bayar = NullToDbl(gvRetur.GetRowCellValue(i, "Potong"))
                IDDetil = GetNewID("MBayarHutangDRetur")
                If Bayar <> 0 Then
                    EksekusiSQL("Insert Into MBayarHutangDRetur(NoID,IDBayarHutang,IDReturBeli,Potong,IsJual,Total,TglRetur) Values(" & _
                    IDDetil & "," & NoID & "," & IDBeli & "," & FixKoma(Bayar) & "," & IIf(IsJual, 1, 0) & "," & FixKoma(NullToDbl(gvRetur.GetRowCellValue(i, "Total"))) & ",'" & NullToDate(gvRetur.GetRowCellValue(i, "Tanggal")).ToString("yyyy-MM-dd HH:mm") & "')")
                End If
            Next
            'Revisi Harga -->BayarHutangDPH
            SQL = "DELETE FROM MBayarHutangDPH WHERE IDBayarHutang=" & NoID
            EksekusiSQL(SQL)
            For i = 0 To gvPH.RowCount - 1
                IDBeli = gvPH.GetRowCellValue(i, "IDRevisiHarga")
                Bayar = NullToDbl(gvPH.GetRowCellValue(i, "Potong"))
                IDDetil = GetNewID("MBayarHutangDPH")
                If Bayar <> 0 Then
                    EksekusiSQL("Insert Into MBayarHutangDPH(NoID,IDBayarHutang,IDRevisiHarga,Potong,IsJual) Values(" & _
                    IDDetil & "," & NoID & "," & IDBeli & "," & FixKoma(Bayar) & "," & IIf(IsJual, 1, 0) & ")")
                End If
            Next

            'Debet Note-->BayarHutangDDebet
            SQL = "DELETE FROM MBayarHutangDDebet WHERE IDBayarHutang=" & NoID
            EksekusiSQL(SQL)
            For i = 0 To gvDebet.RowCount - 1
                IDBeli = NoID 'gvDebet.GetRowCellValue(i, "IDDebetNote")
                Bayar = NullToDbl(gvDebet.GetRowCellValue(i, "Potong"))
                IDDetil = GetNewID("MBayarHutangDDebet")
                If Bayar <> 0 Then
                    EksekusiSQL("Insert Into MBayarHutangDDebet(NoID,IDBayarHutang,IDDebetNote,Potong,IsJual,Catatan) Values(" & _
                    IDDetil & "," & NoID & "," & IDBeli & "," & FixKoma(Bayar) & "," & IIf(IsJual, 1, 0) & ",'" & FixApostropi(gvDebet.GetRowCellValue(i, "Catatan")) & "')")
                End If
            Next

            'Kredit Note-->BayarHutangDKredit
            SQL = "DELETE FROM MBayarHutangDKredit WHERE IDBayarHutang=" & NoID
            EksekusiSQL(SQL)
            For i = 0 To gvKredit.RowCount - 1
                IDBeli = NoID 'gvKredit.GetRowCellValue(i, "IDKreditNote")
                Bayar = NullToDbl(gvKredit.GetRowCellValue(i, "Potong"))
                IDDetil = GetNewID("MBayarHutangDKredit")
                If Bayar <> 0 Then
                    EksekusiSQL("Insert Into MBayarHutangDKredit(NoID,IDBayarHutang,IDKreditNote,Potong,IsJual,Catatan) Values(" & _
                    IDDetil & "," & NoID & "," & IDBeli & "," & FixKoma(Bayar) & "," & IIf(IsJual, 1, 0) & ",'" & FixApostropi(gvKredit.GetRowCellValue(i, "Catatan")) & "')")
                End If
            Next

            'Giro-->BayarHutangDGiro
            SQL = "DELETE FROM MBayarHutangDGiro WHERE IDBayarHutang=" & NoID
            EksekusiSQL(SQL)
            For i = 0 To gviGiro.RowCount - 1
                IDBeli = NoID 'gviGiro.GetRowCellValue(i, "IDDebetNote")
                Bayar = NullToDbl(gviGiro.GetRowCellValue(i, "Total"))
                IDDetil = GetNewID("MBayarHutangDGiro")
                If Bayar <> 0 Then
                    EksekusiSQL("INSERT INTO MBayarHutangDGiro(NoID,IDBayarHutang,Tanggal,Total,Keterangan,TglJatuhTempo,NoGiro,IDBank) Values(" & _
                    IDDetil & "," & NoID & ",'" & NullToDate(gviGiro.GetRowCellValue(i, "Tanggal")).ToString("yyyy-MM-dd") & "'," & FixKoma(Bayar) & ",'" & FixApostropi(gviGiro.GetRowCellValue(i, "Keterangan")) & "','" & NullToDate(gviGiro.GetRowCellValue(i, "TglJatuhTempo")).ToString("yyyy-MM-dd") & "','" & FixApostropi(gviGiro.GetRowCellValue(i, "NoGiro")) & "'," & NullToLong(gviGiro.GetRowCellValue(i, "IDBank")) & ")")
                End If
            Next

            'Auto Posting
            EksekusiSQL("UPDATE MBayarHutang SET IsPosted=1 where NoID=" & NoID)
        Catch ex As Exception
            XtraMessageBox.Show("Info Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
        End Try
    End Sub
    Private Sub cmdSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSave.Click
        Dim PesanSalah As String = ""
        cmdSave.Enabled = False
        If Not IsPosted Then
            If IsValidasi() Then
                If Simpan(PesanSalah) = True Then
                    SimpanDetil()
                    DialogResult = Windows.Forms.DialogResult.OK
                    Me.Close()
                Else
                    XtraMessageBox.Show("Info Kesalahan : " & vbCrLf & PesanSalah, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                End If
            End If
        End If
        cmdSave.Enabled = True
    End Sub
    Private Function IsValidasi() As Boolean
        If txtKode.Text = "" Then
            XtraMessageBox.Show("Kode masih kosong.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
            txtKode.Focus()
            Return False
            Exit Function
        End If
        If txtKodeAlamat.Text = "" Then
            XtraMessageBox.Show("Customer masih kosong.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
            txtKodeAlamat.Focus()
            Return False
            Exit Function
        End If
        If Tgl.Text = "" Then
            XtraMessageBox.Show("Tanggal masih kosong.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
            Tgl.Focus()
            Return False
            Exit Function
        End If
        HitungTotal()
        If txtSubTotal.EditValue - txtJumlahKwitansi.EditValue - txtPotongan.EditValue - txtMaterai.EditValue + txtDN.EditValue <> txtBayar.EditValue Then
            If XtraMessageBox.Show("Jumlah Bayar Belum Sesuai, yakin ingin melanjutkan penyimpanan?", NamaAplikasi, MessageBoxButtons.YesNo, MessageBoxIcon.Warning) = Windows.Forms.DialogResult.No Then
                txtBayar.Focus()
                Return False
                Exit Function
            End If
        End If
        If txtWilayah.Text = "" Then
            XtraMessageBox.Show("Wilayah masih kosong.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
            txtWilayah.Focus()
            Return False
            Exit Function
        End If
        If pTipe = pStatus.Baru AndAlso txtKode.Properties.ReadOnly Then
            txtKode.Text = clsKode.MintaKodeBaru("BY" & IIf(IsJual, "P", "H"), "MBayarHutang", Tgl.DateTime, NullToLong(txtWilayah.EditValue), 5, " IsJual=" & IIf(IsJual, 1, 0))
        End If
        If CekKodeValid(txtKode.Text, KodeLama, "MBayarHutang", "Kode", IIf(pTipe = pStatus.Edit, True, False)) Then
            XtraMessageBox.Show("Kode sudah dipakai.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
            txtKode.Focus()
            Return False
            Exit Function
        End If
        'If pTipe = pStatus.Edit AndAlso gvRetur.RowCount <= 0 Then
        '    XtraMessageBox.Show("Item detil masih kosong." & vbCrLf & "Isi item detil atau tutup bila ingin membatalkan.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
        '    Return False
        '    Exit Function
        'End If
        If clsKode.NotaDiPosting("MBayarHutang", NoID) Then
            XtraMessageBox.Show("Data ini sudah diposting." & vbCrLf & "Tutup bila ingin membatalkan.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
            Return False
            Exit Function
        End If
        Return True
    End Function
    Dim KodeLama As String = ""
    Public Function Simpan(ByRef PesanSalah As String) As Boolean
        Dim Sukses As Boolean = False
        HitungTotal()
        Try
            If pTipe = pStatus.Baru Then
                NoID = GetNewID("MBayarHutang")
                'IDAkunKwitansi INT NULL, " & _
                '" IDAkunKwitansiD INT NULL, " & _
                '" NoKwitansi Varchar(30) NULL, " & _
                '" KetKwitansi Varchar(100) NULL "
                SQL = "INSERT INTO [MBayarHutang] ([NoID],[Kode],[Tanggal],[IDALamat],[IDDepartemen],[IDWilayah],[Keterangan],[IDUserEntry],[TglEntry],[IDUserEdit],[TglEdit],[IsEdit],[JumlahBeli],[JumlahRetur],[JumlahPH],[NotaDebet],[NotaKredit],[Subtotal],[JumlahUangMuka],[Total],[Catatan],[IsSupplier],[IsJual],[IDTT],[IDKwitansi],[JumlahKwitansi],[Potongan],[Materai],[DN],[TglKembali],[IDBank]) VALUES (" & vbCrLf & _
                      NoID & "," & _
                      "'" & FixApostropi(txtKode.Text) & "'," & _
                      "'" & Tgl.DateTime.ToString("yyyy-MM-dd HH:mm") & "'," & _
                      NullToLong(txtKodeAlamat.EditValue) & "," & _
                      DefIDDepartemen & "," & vbCrLf & _
                      DefIDWilayah & "," & _
                      "'" & FixApostropi(txtCatatan.Text) & "'," & _
                      IDUserAktif & "," & _
                      "GETDATE()," & _
                      IDUserAktif & "," & _
                      "Getdate()," & _
                      "1," & _
                      FixKoma(Bulatkan(NullToDbl(gvPembelian.Columns("Bayar").SummaryItem.SummaryValue), 0)) & "," & _
                      FixKoma(Bulatkan(NullToDbl(gvRetur.Columns("Potong").SummaryItem.SummaryValue), 0)) & "," & _
                      FixKoma(Bulatkan(NullToDbl(gvPH.Columns("Potong").SummaryItem.SummaryValue), 0)) & "," & _
                      FixKoma(Bulatkan(NullToDbl(gvDebet.Columns("Potong").SummaryItem.SummaryValue), 0)) & "," & _
                      FixKoma(Bulatkan(NullToDbl(gvKredit.Columns("Potong").SummaryItem.SummaryValue), 0)) & "," & _
                      FixKoma(NullToDbl(txtSubTotal.EditValue)) & "," & _
                      FixKoma(Bulatkan(NullToDbl(gvTU.Columns("Potong").SummaryItem.SummaryValue), 0)) & "," & _
                      FixKoma(NullToDbl(txtBayar.EditValue)) & "," & _
                      "'" & FixApostropi(txtCatatan.Text) & "'," & _
                      IIf(IsJual, 0, 1) & "," & _
                      IIf(IsJual, 1, 0) & "," & NullToLong(txtNoTT.EditValue) & "," & NullToLong(txtKwitansi.EditValue) & "," & FixKoma(txtJumlahKwitansi.EditValue) & "," & FixKoma(NullToDbl(txtPotongan.EditValue)) & "," & FixKoma(NullToDbl(txtMaterai.EditValue)) & "," & FixKoma(txtDN.EditValue) & ",'" & FixApostropi(txtTglKembali.DateTime.ToString("yyyy/MM/dd HH:mm")) & "'," & NullToLong(txtBank.EditValue) & ")"
            Else
                SQL = "UPDATE [MBayarHutang] SET " & vbCrLf & _
                      " Kode='" & FixApostropi(txtKode.Text) & "'," & _
                      " Tanggal='" & Tgl.DateTime.ToString("yyyy-MM-dd HH:mm") & "'," & _
                      " TglKembali='" & txtTglKembali.DateTime.ToString("yyyy-MM-dd HH:mm") & "'," & _
                      " IDAlamat=" & NullToLong(txtKodeAlamat.EditValue) & "," & _
                      " Keterangan='" & FixApostropi(txtCatatan.Text) & "'," & _
                      " IDUserEdit=" & IDUserAktif & "," & _
                      " TglEdit=Getdate()," & _
                      " IsEdit=0," & _
                      " JumlahBeli=" & FixKoma(Bulatkan(NullToDbl(gvPembelian.Columns("Bayar").SummaryItem.SummaryValue), 0)) & "," & _
                      " JumlahRetur=" & FixKoma(Bulatkan(NullToDbl(gvRetur.Columns("Potong").SummaryItem.SummaryValue), 0)) & "," & _
                      " JumlahPH=" & FixKoma(Bulatkan(NullToDbl(gvPH.Columns("Potong").SummaryItem.SummaryValue), 0)) & "," & _
                      " NotaDebet=" & FixKoma(Bulatkan(NullToDbl(gvDebet.Columns("Potong").SummaryItem.SummaryValue), 0)) & "," & _
                      " NotaKredit=" & FixKoma(Bulatkan(NullToDbl(gvKredit.Columns("Potong").SummaryItem.SummaryValue), 0)) & "," & _
                      " Subtotal=" & FixKoma(NullToDbl(txtSubTotal.EditValue)) & "," & _
                      " IDKwitansi=" & NullToLong(txtKwitansi.EditValue) & "," & _
                      " JumlahKwitansi=" & FixKoma(NullToDbl(txtJumlahKwitansi.EditValue)) & "," & _
                      " JumlahUangMuka=" & FixKoma(Bulatkan(NullToDbl(gvTU.Columns("Potong").SummaryItem.SummaryValue), 0)) & "," & _
                      " Total=" & FixKoma(NullToDbl(txtBayar.EditValue)) & "," & _
                      " Potongan=" & FixKoma(NullToDbl(txtPotongan.EditValue)) & "," & _
                      " DN=" & FixKoma(NullToDbl(txtDN.EditValue)) & "," & _
                      " Materai=" & FixKoma(NullToDbl(txtMaterai.EditValue)) & "," & _
                      " Catatan='" & FixApostropi(txtCatatan.Text) & "'," & _
                      " IsSupplier=" & IIf(IsJual, 0, 1) & "," & _
                      " IDTT=" & NullToLong(txtNoTT.EditValue) & "," & _
                      " IDBank=" & NullToLong(txtBank.EditValue) & "," & _
                      " IsJual=" & IIf(IsJual, 1, 0) & _
                      " WHERE NoID=" & NoID
            End If
            If EksekusiSQL(SQL) >= 1 Then
                Sukses = True
            Else
                Sukses = False
            End If
            If Sukses Then
                SQL = "UPDATE [MBayarHutang] SET " & vbCrLf & _
                      " IDAkunKwitansi=" & defIDAkunAntarKantor & "," & _
                      " IDAkunKwitansiD=" & NullToLong(txtAkunKwitansi.EditValue) & "," & _
                      " NoKwitansi='" & FixApostropi(txtNoKwitansi.Text) & "'," & _
                      " KetKwitansi='" & FixApostropi(txtKeteranganKwitansi.Text) & "' " & _
                      " WHERE NoID=" & NoID
                If EksekusiSQL(SQL) >= 1 Then
                    Sukses = True
                Else
                    Sukses = False
                End If
            End If
        Catch ex As Exception
            PesanSalah = ex.Message
            Sukses = False
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
                gvRetur.SaveLayoutToXml(FolderLayouts & Me.Name & gvRetur.Name & ".xml")
                gvPembelian.SaveLayoutToXml(FolderLayouts & Me.Name & gvPembelian.Name & ".xml")
                SearchLookUpEdit1View.SaveLayoutToXml(FolderLayouts & Me.Name & SearchLookUpEdit1View.Name & ".xml")
                gvWilayah.SaveLayoutToXml(FolderLayouts & Me.Name & gvWilayah.Name & ".xml")

                gvDebet.SaveLayoutToXml(FolderLayouts & Me.Name & gvDebet.Name & ".xml")
                gvKredit.SaveLayoutToXml(FolderLayouts & Me.Name & gvKredit.Name & ".xml")
                gviGiro.SaveLayoutToXml(FolderLayouts & Me.Name & gviGiro.Name & ".xml")
            End If
        Catch ex As Exception
            XtraMessageBox.Show("Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            xOtorisasi.Dispose()
        End Try
    End Sub

    Private Sub cmdRefresh_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdRefresh.Click
        'LoadData()
        RefreshDetil()
    End Sub

    Private Sub cmdDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdDelete.Click
        Dim view As ColumnView = gcRetur.FocusedView
        Try
            Dim row As System.Data.DataRow = view.GetDataRow(gvRetur.FocusedRowHandle)
            Dim dc As Integer = gvRetur.FocusedRowHandle
            Dim IDDetil As Long = NullToLong(row("NoID"))
            If XtraMessageBox.Show("Item ini mau dihapus?", NamaAplikasi, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) = Windows.Forms.DialogResult.Yes Then
                EksekusiSQL("Delete From MBayarHutangD where NoID=" & IDDetil.ToString)
                RefreshDetil()
            End If
            txtBarang.Focus()
        Catch ex As Exception
            XtraMessageBox.Show("Untuk menghapus pilih item yang akan dihapus terlebih dahulu lalu tekan tombol hapus.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub txtKodeCustomer_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtKodeAlamat.EditValueChanged
        Dim TglAdd As Long = 0
        Try
            txtNamaAlamat.Text = NullToStr(EksekusiSQlSkalarNew("SELECT Nama FROM MAlamat WHERE NoID=" & NullToLong(txtKodeAlamat.EditValue)))
            txtAlamat.Text = NullToStr(EksekusiSQlSkalarNew("SELECT Alamat FROM MAlamat WHERE NoID=" & NullToLong(txtKodeAlamat.EditValue)))
            TglAdd = NullToLong(EksekusiSQlSkalarNew("SELECT JatuhTempoCustomer FROM MAlamat WHERE NoID=" & NullToLong(txtKodeAlamat.EditValue)))
            RefreshTT()
            'If TglAdd = 0 Then
            '    tglJatuhTempo.DateTime = DateAdd(DateInterval.Month, 2, Tgl.DateTime)
            'Else
            '    tglJatuhTempo.DateTime = DateAdd(DateInterval.Day, TglAdd, Tgl.DateTime)
            'End If
        Catch ex As Exception

        End Try
    End Sub
    Private Sub RefreshTT()
        Dim ds As New DataSet
        Try
            SQL = "SELECT NoID, Kode AS NoTT, Tanggal FROM MTT WHERE IsPosted=1 AND IDCustomer=" & NullToLong(txtKodeAlamat.EditValue) & " AND NoID NOT IN (SELECT IsNull(IDTT,0) FROM MBayarHutang WHERE NoID<>" & NoID & ")"
            ds = ExecuteDataset("MTT", SQL)
            txtNoTT.Properties.DataSource = ds.Tables("MTT")
            txtNoTT.Properties.DisplayMember = "NoTT"
            txtNoTT.Properties.ValueMember = "NoID"

            SQL = "SELECT ID, Kode, NoKwitansi, Jumlah FROM MKasIN WHERE IsPosted=1 AND IDAlamat=" & NullToLong(txtKodeAlamat.EditValue) & " AND ID NOT IN (SELECT IsNull(IDKwitansi,0) FROM MBayarHutang WHERE NoID<>" & NoID & ")"
            ds = ExecuteDataset("MTT", SQL)
            txtKwitansi.Properties.DataSource = ds.Tables("MTT")
            txtKwitansi.Properties.DisplayMember = "NoKwitansi"
            txtKwitansi.Properties.ValueMember = "ID"
        Catch ex As Exception
            XtraMessageBox.Show("Info Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
        Finally
            ds.Dispose()
        End Try
    End Sub
    Sub InsertkanKeDetil()
        EksekusiSQL("Delete from TBayarHutangD Where IsJual=" & IIf(IsJual, 1, 0) & " and IDUser=" & IDUserAktif)
        EksekusiSQL("Delete from TBayarHutangDRetur Where IsJual=" & IIf(IsJual, 1, 0) & " and IDUser=" & IDUserAktif)
        EksekusiSQL("Delete from TBayarHutangDPH Where IsJual=" & IIf(IsJual, 1, 0) & " and IDUser=" & IDUserAktif)
        EksekusiSQL("Delete from TBayarHutangDDebet Where IsJual=" & IIf(IsJual, 1, 0) & " and IDUser=" & IDUserAktif)
        EksekusiSQL("Delete from TBayarHutangDKredit Where IsJual=" & IIf(IsJual, 1, 0) & " and IDUser=" & IDUserAktif)

        If NullToLong(txtKodeAlamat.EditValue) > 0 Then
            If IsNew Then
                'Saldo Awal

                'Pembelian
                If txtNoTT.Text <> "" Then 'Hadeh Bikin Pusing aja ini
                    EksekusiSQL("insert Into TBayarHutangD(IsJual,IDBeli,IDUser,Total,TglBeli) " & _
                                "Select " & IIf(IsJual, 1, 0) & " as IsJual, MBeli.NoID," & IDUserAktif & " As IDUser, ROUND(MBeli.Total,0), MBeli.Tanggal" & _
                                " FROM ((MBeli inner join (select IDTransaksi from MTTD  " & _
                                "inner join MTT on MTTD.IDTT=MTT.NoID " & _
                                "where (MTT.IDCustomer= " & txtKodeAlamat.EditValue & " And MTTD.IDJenisTransaksi = 2 AND MTT.NoID=" & NullToLong(txtNoTT.EditValue) & ") " & _
                                ") KB on MBeli.NoID=KB.IDTransaksi) " & _
                                " Left Join " & _
                                "(Select IDBeli, Sum(Bayar) as Bayar FROM MBayarHutangD where IsJual=0 group by IDBeli) " & _
                                "B on MBeli.NoID=B.IDBeli) " & _
                                "WHERE MBeli.isPosted=1 and ROUND(Mbeli.Total,0)-ISNULL(B.Bayar,0)<>0 and MBeli.IDSupplier=" & txtKodeAlamat.EditValue)
                Else
                    EksekusiSQL("insert Into TBayarHutangD(IsJual,IDBeli,IDUser,Total,TglBeli) " & _
                                " SELECT " & IIf(IsJual, 1, 0) & " as IsJual, MBeli.NoID," & IDUserAktif & " As IDUser, ROUND(MBeli.Total,0), MBeli.Tanggal " & _
                                " FROM (MBeli " & _
                                " Left Join " & _
                                " (Select IDBeli, Sum(Bayar) as Bayar FROM MBayarHutangD where IsJual=0 group by IDBeli) " & _
                                " B on MBeli.NoID=B.IDBeli) " & _
                                " WHERE MBeli.isPosted=1 and ROUND(Mbeli.Total,0)-ISNULL(B.Bayar,0)<>0 and MBeli.IDSupplier=" & txtKodeAlamat.EditValue)
                End If
                'Retur Beli
                EksekusiSQL("insert Into TBayarHutangDRetur(IsJual,IDReturBeli,IDUser,Total,TglRetur) " & _
                            "Select " & IIf(IsJual, 1, 0) & " as IsJual, MReturBeli.NoID," & IDUserAktif & " As IDUser, ROUND(MReturBeli.Total,0), MReturBeli.Tanggal " & _
                            " FROM (MReturBeli " & _
                            " Left Join " & _
                            "(Select IDReturBeli,Sum(Potong) as Bayar FROM MBayarHutangDRetur where IsJual=0 group by IDReturBeli) " & _
                            "B on MReturBeli.NoID=B.IDReturBeli) " & _
                            "WHERE MReturBeli.isPosted=1 and ROUND(MReturBeli.Total,0)-ISNULL(B.Bayar,0)<>0 and MReturBeli.IDSupplier=" & txtKodeAlamat.EditValue)
            Else
                'C=Nota yang Diedit
                'B=Beli
                'RB Retur Beli
                If txtNoTT.Text <> "" Then 'Hadeh Bikin Pusing aja ini
                    EksekusiSQL("insert Into TBayarHutangD(Isjual,IDBeli,IDUser,Bayar,Total,TglBeli) " & _
                               "Select " & IIf(IsJual, 1, 0) & " as IsJual, MBeli.NoID," & IDUserAktif & " As IDUser,C.TerBayar, CASE WHEN IsNull(C.Total,0)=0 THEN ROUND(MBeli.Total,0) ELSE C.Total END AS Total, " & _
                               " CASE WHEN C.TglBeli IS NULL THEN MBeli.Tanggal ELSE C.TglBeli END AS TglBeli " & vbCrLf & _
                               " FROM ((MBeli inner join (select IDTransaksi   from MTTD  " & _
                               "inner join MTT on MTTD.IDTT=MTT.NoID " & _
                               "where (MTT.IDCustomer= " & txtKodeAlamat.EditValue & " And MTTD.IDJenisTransaksi = 2 AND MTT.NoID=" & NullToLong(txtNoTT.EditValue) & ") " & _
                               ") KB on MBeli.NoID=KB.IDTransaksi) Left Join (Select IDBeli,Sum(Bayar) as Bayar FROM MBayarHutangD where IsJual=0 and MBayarHutangD.IDBayarHutang<>" & NoID & " group by IDBeli) B on MBeli.NoID=B.IDBeli) " & _
                               "LEFT JOIN (select MBayarHutangD.TglBeli, MBayarHutangD.IDBeli,MBayarHutangD.Bayar as TerBayar, MBayarHutangD.Total from MBayarHutangD where MBayarHutangD.IDBayarHutang=" & NoID & ") C on MBeli.NoID=C.IDBeli " & _
                               "WHERE MBeli.isPosted=1 and  ROUND(MBeli.Total,0)-Isnull(B.Bayar,0)<>0 and MBeli.IDSupplier=" & txtKodeAlamat.EditValue)
                Else
                    EksekusiSQL("insert Into TBayarHutangD(Isjual,IDBeli,IDUser,Bayar,Total,TglBeli) " & _
                               " SELECT " & IIf(IsJual, 1, 0) & " as IsJual, MBeli.NoID," & IDUserAktif & " As IDUser,C.TerBayar, CASE WHEN IsNull(C.Total,0)=0 THEN ROUND(MBeli.Total,0) ELSE C.Total END AS Total, " & vbCrLf & _
                               " CASE WHEN C.TglBeli IS NULL THEN MBeli.Tanggal ELSE C.TglBeli END AS TglBeli " & vbCrLf & _
                               " FROM (MBeli Left Join (Select IDBeli,Sum(Bayar) as Bayar FROM MBayarHutangD where IsJual=0 and MBayarHutangD.IDBayarHutang<>" & NoID & " group by IDBeli) B on MBeli.NoID=B.IDBeli) " & vbCrLf & _
                               " LEFT JOIN (select MBayarHutangD.TglBeli, MBayarHutangD.IDBeli,MBayarHutangD.Bayar as TerBayar, MBayarHutangD.Total from MBayarHutangD where MBayarHutangD.IDBayarHutang=" & NoID & ") C on MBeli.NoID=C.IDBeli " & vbCrLf & _
                               " WHERE MBeli.IsPosted=1 AND ROUND(MBeli.Total,0)-IsNull(B.Bayar,0)<>0 AND MBeli.IDSupplier=" & txtKodeAlamat.EditValue)
                End If


                'Retur Pembelian
                EksekusiSQL("insert Into TBayarHutangDRetur(Isjual,IDReturBeli,IDUser,Potong,Total,TglRetur) " & _
                       "Select " & IIf(IsJual, 1, 0) & " as IsJual, MReturBeli.NoID," & IDUserAktif & " As IDUser,C.TerBayar, CASE WHEN IsNull(C.Total,0)=0 THEN ROUND(MReturBeli.Total,0) ELSE C.Total END AS Total, " & _
                       " CASE WHEN C.TglRetur IS NULL THEN MReturBeli.Tanggal ELSE C.TglRetur END AS TglRetur " & vbCrLf & _
                       " FROM (MReturBeli Left Join (Select IDReturBeli,Sum(Potong) as Bayar FROM MBayarHutangDRetur where IsJual=" & IIf(IsJual, 1, 0) & " and MBayarHutangDRetur.IDBayarHutang<>" & NoID & " group by IDReturBeli) B on MReturBeli.NoID=B.IDReturBeli) " & _
                       "LEFT JOIN (select MBayarHutangDRetur.TglRetur, MBayarHutangDRetur.IDReturBeli,MBayarHutangDRetur.Potong as TerBayar, MBayarHutangDRetur.Total from MBayarHutangDRetur where MBayarHutangDRetur.IDBayarHutang=" & NoID & ") C on MReturBeli.NoID=C.IDReturBeli " & _
                       "WHERE MReturBeli.isPosted=1 and  ROUND(MReturBeli.Total,0)-Isnull(B.Bayar,0)<>0 and MReturBeli.IDSupplier=" & txtKodeAlamat.EditValue)


                'Revisi Harga Beli
                EksekusiSQL("insert Into TBayarHutangDPH(Isjual,IDRevisiHarga,IDUser,Potong) " & _
                       "Select " & IIf(IsJual, 1, 0) & " as IsJual, MRevisiHargaBeli.NoID," & IDUserAktif & " As IDUser,C.TerBayar " & _
                       "FROM ((MRevisiHargaBeli inner join " & _
                        "(select IDRevisiHargaBeli,Sum(KoreksiBL) as Total from MRevisiHargaBeliD group by IDRevisiHargaBeli) MPHD " & _
                        "on MRevisiHargaBeli.NoID=MPHD.IDRevisiHargaBeli inner join (select IDTransaksi   from MTTD  " & _
                        "inner join MTT on MTTD.IDTT=MTT.NoID " & _
                        "where (MTT.IDCustomer= " & txtKodeAlamat.EditValue & " And MTTD.IDJenisTransaksi = 11) " & _
                        ") KB on MRevisiHargaBeli.NoID=KB.IDTransaksi) Left Join (Select IDRevisiHarga,Sum(Potong) as Bayar FROM MBayarHutangDPH where IsJual=" & IIf(IsJual, 1, 0) & " and MBayarHutangDPH.IDBayarHutang<>" & NoID & " group by IDRevisiHarga) B on MRevisiHargaBeli.NoID=B.IDRevisiHarga) " & _
                       "LEFT JOIN (select MBayarHutangDPH.IDRevisiHarga,MBayarHutangDPH.Potong as TerBayar from MBayarHutangDPH where MBayarHutangDPH.IDBayarHutang=" & NoID & ") C on MRevisiHargaBeli.NoID=C.IDRevisiHarga " & _
                       "WHERE MRevisiHargaBeli.isPosted=1 and  MPHD.Total-Isnull(B.Bayar,0)<>0 and MRevisiHargaBeli.IDSupplier=" & txtKodeAlamat.EditValue)
            End If
        End If
        Application.DoEvents()
        RefreshDetil()

    End Sub
    Private Sub Tgl_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Tgl.EditValueChanged
        Dim TglAdd As Long = 0
        Try
            'txtNamaCustomer.Text = NullTostr(SearchLookUpEdit1View.GetRowCellValue(SearchLookUpEdit1View.FocusedRowHandle, "Nama"))
            'txtAlamatCustomer.Text = NullTostr(EksekusiSQlSkalarNew("SELECT Alamat FROM MAlamat WHERE NoID=" & NullTolong(txtKodeCustomer.EditValue)))
            'TglAdd = NullToLong(EksekusiSQlSkalarNew("SELECT JatuhTempoCustomer FROM MAlamat WHERE NoID=" & NullToLong(txtKodeAlamat.EditValue)))
            'If TglAdd = 0 Then
            '    tglJatuhTempo.DateTime = DateAdd(DateInterval.Month, 2, Tgl.DateTime)
            'Else
            '    tglJatuhTempo.DateTime = DateAdd(DateInterval.Day, TglAdd, Tgl.DateTime)
            'End If
            If pTipe = pStatus.Baru Then
                txtKode.Text = clsKode.MintaKodeBaru("BY" & IIf(IsJual, "P", "H"), "MBayarHutang", Tgl.DateTime, NullToLong(txtWilayah.EditValue), 5, " IsJual=" & IIf(IsJual, 1, 0))
            End If
        Catch ex As Exception

        End Try
    End Sub

    Private Sub txtBarang_ButtonClick(ByVal sender As Object, ByVal e As DevExpress.XtraEditors.Controls.ButtonPressedEventArgs) Handles txtBarang.ButtonClick
        If pTipe = pStatus.Baru Then Exit Sub
        Select Case e.Button.Index
            Case 0
                'InsertIntoDetil()
                txtBarang.Text = ""
                'txtBarang.Focus()
            Case 1
                txtBarang.Text = ""
        End Select
    End Sub
    'Private Function CariBarang(ByRef IDBarang As Long, ByRef NamaBarang As String, ByRef KodeBarang As String) As Boolean
    '    Dim x As Boolean = False
    '    Dim SQL As String = ""
    '    Dim oDS As New DataSet
    '    Try
    '        SQL = "SELECT NoID,Kode,Nama FROM MBarang WHERE " & IIf(IsSO, "", " IsNonStock=1 AND ") & " MBarang.IsActive=1 AND (UPPER(Kode) = '" & NamaBarang.Replace("'", "''").ToUpper & "' OR UPPER(NAMA) = '" & NamaBarang.Replace("'", "''").ToUpper & "') ORDER BY Kode"
    '        oDS = ExecuteDataset("Tbl", SQL)
    '        If oDS.Tables("Tbl").Rows.Count >= 1 Then
    '            NamaBarang = NullToStr(oDS.Tables(0).Rows(0).Item("Nama"))
    '            KodeBarang = NullToStr(oDS.Tables(0).Rows(0).Item("Kode"))
    '            IDBarang = NullToLong(oDS.Tables(0).Rows(0).Item("NoID"))
    '            x = True
    '        Else
    '            x = False
    '        End If
    '    Catch ex As Exception
    '        x = False
    '    End Try
    '    Return x
    'End Function
    'Private Sub InsertIntoDetil()
    '    Dim SQL As String = ""
    '    Dim NamaBarang As String = txtBarang.Text
    '    Dim KodeBarang As String = ""
    '    Dim IDBarang As Long = -1
    '    Dim IDDetil As Long = -1
    '    Dim frmEntri As New frmEntriSOD
    '    Dim Konversi As Double = 0.0
    '    Dim Harga As Double = 0.0
    '    Try
    '        If txtKodeAlamat.Text = "" Then XtraMessageBox.Show("Customer masih kosong.", NamaBarang) : txtKodeAlamat.Focus() : Exit Sub
    '        If CariBarang(IDBarang, NamaBarang, KodeBarang) Then
    '            If XtraMessageBox.Show("Ingin menambah Item Sales Order dengan Kode Stock " & KodeBarang & " Nama Stock " & NamaBarang, "Fast Entri Says", MessageBoxButtons.YesNo) = Windows.Forms.DialogResult.Yes Then
    '                'IDDetil = GetNewID("MBayarHutangD", "NoID")
    '                'Konversi = NullToDbl(EksekusiSQlSkalarNew("SELECT Konversi FROM MBarangD WHERE IsJual=1 AND IDBarang=" & IDBarang & " AND IDSatuan=" & DefIDSatuan))
    '                'Harga = clsPostingPenjualan.HargaJual(NullToLong(IDBarang), NullToLong(DefIDSatuan), NullToLong(txtKodeCustomer.EditValue), 0, 0)
    '                'If Konversi <> 0 Then
    '                '    SQL = "INSERT INTO MBayarHutangD (NoID,IDWilayah,IDBarang,IDSatuan,Konversi,IDBayarHutang,NoUrut,Tgl,Jam,Harga) VALUES " & vbCrLf
    '                '    SQL &= "(" & IDDetil & "," & NullToLong(txtWilayah.EditValue) & "," & IDBarang & "," & DefIDSatuan & "," & FixKoma(Konversi) & "," & NoID & "," & GetNewID("MBayarHutangD", "NoUrut", " WHERE IDBayarHutang=" & NoID) & ",GetDate(),GetDate()," & FixKoma(Harga) & ")"
    '                'Else
    '                '    SQL = "INSERT INTO MBayarHutangD (NoID,IDWilayah,IDBarang,IDBayarHutang,NoUrut,Tgl,Jam,Harga) VALUES " & vbCrLf
    '                '    SQL &= "(" & IDDetil & "," & NullToLong(txtWilayah.EditValue) & "," & IDBarang & "," & NoID & "," & GetNewID("MBayarHutangD", "NoUrut", " WHERE IDBayarHutang=" & NoID) & ",GetDate(),GetDate()," & FixKoma(Harga) & ")"
    '                'End If
    '                'EksekusiSQL(SQL)
    '                frmEntri.IsNew = True
    '                frmEntri.NoID = IDDetil
    '                frmEntri.IDBayarHutang = NoID
    '                frmEntri.IsFastEntri = False
    '                frmEntri.FormPemanggil = Me
    '                frmEntri.Show()
    '                frmEntri.IDWilayah = NullToLong(txtWilayah.EditValue)
    '                frmEntri.IDCustomer = NullToLong(txtKodeAlamat.EditValue)
    '                frmEntri.txtBarang.EditValue = NullToLong(IDBarang)
    '                frmEntri.txtSatuan.EditValue = DefIDSatuan
    '                frmEntri.Focus()
    '                If Not frmEntri.txtGudang.Properties.ReadOnly Then
    '                    frmEntri.txtGudang.Focus()
    '                Else
    '                    frmEntri.txtSatuan.Focus()
    '                End If
    '                'If frmEntri.ShowDialog(Me)= Windows.Forms.DialogResult.OK Then
    '                '    RefreshDetil()
    '                '    GV1.ClearSelection()
    '                '    GV1.FocusedRowHandle = GV1.LocateByDisplayText(0, GV1.Columns("NoID"), IDDetil.ToString("#,##0"))
    '                '    GV1.SelectRow(GV1.FocusedRowHandle)
    '                'Else
    '                '    SQL = "DELETE FROM MBayarHutangD WHERE NoID=" & IDDetil
    '                '    EksekusiSQL(SQL)
    '                '    RefreshDetil()
    '                '    GV1.ClearSelection()
    '                '    GV1.FocusedRowHandle = GV1.LocateByDisplayText(0, GV1.Columns("NoID"), IDDetil.ToString("#,##0"))
    '                '    GV1.SelectRow(GV1.FocusedRowHandle)
    '                'End If

    '            End If
    '        End If
    '    Catch ex As Exception
    '        XtraMessageBox.Show("Kesalahan : " & ex.Message, "Fast Entri Says", MessageBoxButtons.OK)
    '        'frmEntri.Dispose()
    '    End Try
    'End Sub

    Private Sub cmdBAru_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdBAru.Click
        'Dim frmEntri As New frmEntriSOD
        'Try
        '    Dim IDDetil As Long = -1
        '    frmEntri.NoID = IDDetil
        '    frmEntri.IsNew = True
        '    frmEntri.IDBayarHutang = NoID
        '    frmEntri.IsSO = IsSO
        '    If Not IsSO Then
        '        frmEntri.Text = "Entri Detil Permintaan Barang"
        '    End If
        '    If txtKodeAlamat.Text <> "" Then
        '        frmEntri.IDCustomer = NullToLong(txtKodeAlamat.EditValue)
        '        frmEntri.NoID = IDDetil
        '        frmEntri.IsNew = True
        '        frmEntri.IDBayarHutang = NoID
        '        frmEntri.IDWilayah = NullToLong(txtWilayah.EditValue)
        '        frmEntri.FormPemanggil = Me

        '        'frmEntri.MdiParent = MdiParent.MdiParent
        '        'frmEntri.WindowState = FormWindowState.Normal
        '        'frmEntri.StartPosition = FormStartPosition.CenterParent
        '        'frmEntri.FormBorderStyle = Windows.Forms.FormBorderStyle.SizableToolWindow

        '        frmEntri.Show()
        '        frmEntri.Focus()
        '        frmEntri.txtBarcode.Focus()
        '        'If frmEntri.ShowDialog(Me)= Windows.Forms.DialogResult.OK Then
        '        '    RefreshDetil()
        '        'End If
        '        'txtBarang.Focus()
        '    Else
        '        XtraMessageBox.Show("Isi dulu Customer, lalu click baru.", NamaAplikasi, MessageBoxButtons.OK)
        '    End If
        'Catch ex As Exception
        '    XtraMessageBox.Show("Untuk membuat baru tekan tombol baru", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        'Finally
        '    'frmEntri.Dispose()
        'End Try
    End Sub

    Private Sub cmdEdit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdEdit.Click
        'Dim view As ColumnView = GC1.FocusedView
        'Dim frmEntri As New frmEntriSOD
        'Try
        '    Dim row As System.Data.DataRow = view.GetDataRow(GV1.FocusedRowHandle)
        '    Dim dc As Integer = GV1.FocusedRowHandle
        '    Dim IDDetil As Long = NullToLong(row("NoID"))
        '    If txtKodeAlamat.Text <> "" Then
        '        frmEntri.IDCustomer = NullToLong(txtKodeAlamat.EditValue)
        '        frmEntri.IsNew = False
        '        frmEntri.NoID = IDDetil
        '        frmEntri.IDBayarHutang = NoID
        '        frmEntri.IsSO = IsSO
        '        frmEntri.IDWilayah = NullToLong(txtWilayah.EditValue)
        '        If Not IsSO Then
        '            frmEntri.Text = "Entri Detil Permintaan Barang"
        '        End If
        '        frmEntri.FormPemanggil = Me
        '        frmEntri.Show()
        '        frmEntri.Focus()
        '        'If frmEntri.ShowDialog(Me)= Windows.Forms.DialogResult.OK Then
        '        '    RefreshDetil()
        '        'End If
        '        'txtBarang.Focus()
        '    Else
        '        XtraMessageBox.Show("Isi dulu Customer, lalu click Edit.", NamaAplikasi, MessageBoxButtons.OK)
        '    End If
        'Catch ex As Exception
        '    XtraMessageBox.Show("Untuk mengedit pilih item satuan yang akan diedit terlebih dahulu lalu tekan tombol edit", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        'Finally
        '    'frmEntri.Dispose()
        'End Try
    End Sub

    Private Sub txtBarang_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtBarang.EditValueChanged

    End Sub

    Private Sub BarButtonItem2_ItemClick_1(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem2.ItemClick
        If cmdBAru.Enabled Then
            cmdBAru.PerformClick()
        End If
    End Sub

    Private Sub BarButtonItem4_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem4.ItemClick
        If cmdEdit.Enabled Then
            cmdEdit.PerformClick()
        End If
    End Sub

    Private Sub BarButtonItem5_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem5.ItemClick
        If cmdDelete.Enabled Then
            cmdDelete.PerformClick()
        End If
    End Sub

    Private Sub BarButtonItem6_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem6.ItemClick
        cmdRefresh.PerformClick()

    End Sub

    Private Sub BarButtonItem7_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem7.ItemClick
        cmdTutup.PerformClick()
    End Sub

    Private Sub txtNamaCustomer_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtNamaAlamat.EditValueChanged

    End Sub

    Private Sub Tgl_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Tgl.KeyDown
        If e.KeyCode = Keys.F7 Then
            Dim x As New frmOtorisasiAdmin
            Try
                If Tgl.Properties.ReadOnly Then
                    If x.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
                        IDAdmin = x.IDUserAdmin
                        Tgl.Properties.ReadOnly = False
                        'tglJatuhTempo.Properties.ReadOnly = False
                        txtKode.Properties.ReadOnly = False
                    End If
                End If
            Catch ex As Exception
                XtraMessageBox.Show("Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End If
    End Sub

    Private Sub txtBarang_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtBarang.KeyDown
        If pTipe = pStatus.Baru Then Exit Sub
        Select Case e.KeyCode
            Case Keys.Enter
                'InsertIntoDetil()
                txtBarang.Text = ""
                'txtBarang.Focus()
            Case Keys.Escape
                txtBarang.Text = ""
        End Select
    End Sub

    Private Sub txtKode_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtKode.EditValueChanged

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
        End If
    End Sub

    Private Sub txtWilayah_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtWilayah.EditValueChanged
        If pTipe = pStatus.Baru Then
            txtKode.Text = clsKode.MintaKodeBaru("BY" & IIf(IsJual, "P", "H"), "MBayarHutang", Tgl.DateTime, NullToLong(txtWilayah.EditValue), 5, " IsJual=" & IIf(IsJual, 1, 0))
        End If
    End Sub

    Private Sub txtWilayah_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtWilayah.KeyDown
        If e.KeyCode = Keys.F7 Then
            Dim x As New frmOtorisasiAdmin
            Try
                If txtWilayah.Properties.ReadOnly Then
                    If x.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
                        IDAdmin = x.IDUserAdmin
                        txtWilayah.Properties.ReadOnly = False
                    End If
                End If
            Catch ex As Exception
                XtraMessageBox.Show("Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End If
    End Sub

    Private Sub gvWilayah_DataSourceChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvWilayah.DataSourceChanged
        If System.IO.File.Exists(FolderLayouts & Me.Name & gvWilayah.Name & ".xml") Then
            gvWilayah.RestoreLayoutFromXml(FolderLayouts & Me.Name & gvWilayah.Name & ".xml")
        End If
        With gvWilayah
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

    Private Sub SearchLookUpEdit1View_DataSourceChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles SearchLookUpEdit1View.DataSourceChanged
        If System.IO.File.Exists(FolderLayouts & Me.Name & SearchLookUpEdit1View.Name & ".xml") Then
            SearchLookUpEdit1View.RestoreLayoutFromXml(FolderLayouts & Me.Name & SearchLookUpEdit1View.Name & ".xml")
        End If
        With SearchLookUpEdit1View
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

    Private Sub gvRetur_CellValueChanged(ByVal sender As Object, ByVal e As DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs) Handles gvRetur.CellValueChanged
        HitungTotal()
    End Sub

    Private Sub gvRetur_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvRetur.DoubleClick
        Try
            If IsPosted Then Exit Try
            Dim row As System.Data.DataRow = gvRetur.GetDataRow(gvRetur.FocusedRowHandle)
            If NullToDbl(row("Potong")) = 0 Then
                row("Potong") = NullToDbl(row("Total"))
            Else
                row("Potong") = 0
            End If

            Application.DoEvents()
            HitungTotal()
        Catch ex As Exception

        End Try
    End Sub

    Private Sub GV1_FocusedColumnChanged(ByVal sender As Object, ByVal e As DevExpress.XtraGrid.Views.Base.FocusedColumnChangedEventArgs) Handles gvRetur.FocusedColumnChanged
        'If IsPosted AndAlso IsSupervisor AndAlso gvRetur.FocusedColumn.FieldName.ToLower = "IsSelesai".ToLower Then
        '    gvRetur.OptionsBehavior.Editable = True
        'Else
        '    gvRetur.OptionsBehavior.Editable = False
        'End If
    End Sub

    'Private Sub LoadData()
    '    Dim SQL As String = ""
    '    Dim ds As New DataSet
    '    Try
    '        'Pembelian
    '        SQL = "SELECT MBeli.NoID, 2 AS IDJenisTransaksi, MBeli.Tanggal, MBeli.Kode, 0 AS Debet, MBeli.Total Kredit, MBeli.Keterangan " & vbCrLf & _
    '              " FROM MBeli " & vbCrLf & _
    '              " WHERE MBeli.IDWilayah=1 AND MBeli.IsPosted=1 AND MBeli.IDSupplier=" & NullToLong(txtKodeAlamat.EditValue) & " AND  " & vbCrLf & _
    '              " (0-MBeli.Total)<>ISNULL((SELECT SUM(MBayarHutangD.Debet-MBayarHutangD.Kredit) FROM MBayarHutangD INNER JOIN MBayarHutang ON MBayarHutang.NoID=MBayarHutangD.IDBayarHutang WHERE MBayarHutangD.IDTransaksi=MBeli.NoID AND MBayarHutangD.IDJenisTransaksi=2),0) " & vbCrLf & _
    '              " UNION ALL  " & vbCrLf & _
    '              " SELECT MReturBeli.NoID, 3, MReturBeli.Tanggal, MReturBeli.Kode, MReturBeli.Total, 0, MReturBeli.Keterangan  " & vbCrLf & _
    '              " FROM MReturBeli " & vbCrLf & _
    '              " WHERE MReturBeli.IDWilayah=1 AND MReturBeli.IsPosted=1 AND MReturBeli.IDSupplier=" & NullToLong(txtKodeAlamat.EditValue) & " AND  " & vbCrLf & _
    '              " (MReturBeli.Total-0)<>ISNULL((SELECT SUM(MBayarHutangD.Debet-MBayarHutangD.Kredit) FROM MBayarHutangD INNER JOIN MBayarHutang ON MBayarHutang.NoID=MBayarHutangD.IDBayarHutang WHERE MBayarHutangD.IDTransaksi=MReturBeli.NoID AND MBayarHutangD.IDJenisTransaksi=3),0) " & vbCrLf & _
    '              " UNION ALL " & vbCrLf & _
    '              " SELECT MRevisiHargaBeli.NoID, 11, MRevisiHargaBeli.Tanggal, MRevisiHargaBeli.Kode, ABS(CASE WHEN SUM(MRevisiHargaBeliD.KoreksiBL)>=1 THEN SUM(MRevisiHargaBeliD.KoreksiBL) ELSE 0 END), ABS(CASE WHEN SUM(MRevisiHargaBeliD.KoreksiBL)>=1 THEN 0 ELSE SUM(MRevisiHargaBeliD.KoreksiBL) END), MRevisiHargaBeli.Keterangan  " & vbCrLf & _
    '              " FROM MRevisiHargaBeli " & vbCrLf & _
    '              " INNER JOIN MRevisiHargaBeliD ON MRevisiHargaBeli.NoID=MRevisiHargaBeliD.IDRevisiHargaBeli " & vbCrLf & _
    '              " GROUP BY MRevisiHargaBeli.NoID, MRevisiHargaBeli.Tanggal, MRevisiHargaBeli.Kode, MRevisiHargaBeli.Keterangan, MRevisiHargaBeli.IDWilayah, MRevisiHargaBeli.IsPosted, MRevisiHargaBeli.IDSupplier " & vbCrLf & _
    '              " HAVING MRevisiHargaBeli.IDWilayah=1 AND MRevisiHargaBeli.IsPosted=1 AND MRevisiHargaBeli.IDSupplier=" & NullToLong(txtKodeAlamat.EditValue) & " AND  " & vbCrLf & _
    '              " ABS(CASE WHEN SUM(MRevisiHargaBeliD.KoreksiBL)>=1 THEN SUM(MRevisiHargaBeliD.KoreksiBL) ELSE 0 END)-ABS(CASE WHEN SUM(MRevisiHargaBeliD.KoreksiBL)>=1 THEN 0 ELSE SUM(MRevisiHargaBeliD.KoreksiBL) END)<>ISNULL((SELECT SUM(MBayarHutangD.Debet-MBayarHutangD.Kredit) FROM MBayarHutangD INNER JOIN MBayarHutang ON MBayarHutang.NoID=MBayarHutangD.IDBayarHutang WHERE MBayarHutangD.IDTransaksi=MRevisiHargaBeli.NoID AND MBayarHutangD.IDJenisTransaksi=11),0)" & vbCrLf & _
    '              " UNION ALL " & vbCrLf
    '        'Penjualan 
    '        SQL &= "SELECT MJual.NoID, 6, MJual.Tanggal, MJual.Kode, MJual.Total AS Debet, 0 AS Kredit, MJual.Keterangan " & vbCrLf & _
    '               " FROM MJual " & vbCrLf & _
    '               " WHERE MJual.IDWilayah=1 AND MJual.IsPosted=1 AND MJual.IDCustomer=" & NullToLong(txtKodeAlamat.EditValue) & " AND ISNULL(MJual.IsCash,0)=0 AND ISNULL(MJual.IsPOS,0)=0 AND " & vbCrLf & _
    '               " (MJual.Total-0)<>ISNULL((SELECT SUM(MBayarHutangD.Debet-MBayarHutangD.Kredit) FROM MBayarHutangD INNER JOIN MBayarHutang ON MBayarHutang.NoID=MBayarHutangD.IDBayarHutang WHERE MBayarHutangD.IDTransaksi=MJual.NoID AND MBayarHutangD.IDJenisTransaksi=6),0) " & vbCrLf & _
    '               " UNION ALL  " & vbCrLf & _
    '               " SELECT MReturJual.NoID, 7, MReturJual.Tanggal, MReturJual.Kode, 0, MReturJual.Total, MReturJual.Keterangan  " & vbCrLf & _
    '               " FROM MReturJual " & vbCrLf & _
    '               " WHERE MReturJual.IDWilayah=1 AND MReturJual.IsPosted=1 AND MReturJual.IDCustomer=" & NullToLong(txtKodeAlamat.EditValue) & " AND  " & vbCrLf & _
    '               " (0-MReturJual.Total)<>ISNULL((SELECT SUM(MBayarHutangD.Debet-MBayarHutangD.Kredit) FROM MBayarHutangD INNER JOIN MBayarHutang ON MBayarHutang.NoID=MBayarHutangD.IDBayarHutang WHERE MBayarHutangD.IDTransaksi=MReturJual.NoID AND MBayarHutangD.IDJenisTransaksi=7),0) " & vbCrLf & _
    '               " UNION ALL  " & vbCrLf & _
    '               " SELECT MRevisiHargaJual.NoID, 15, MRevisiHargaJual.Tanggal, MRevisiHargaJual.Kode, ABS(CASE WHEN SUM(MRevisiHargaJualD.KoreksiJL)>=1 THEN 0 ELSE SUM(MRevisiHargaJualD.KoreksiJL) END), ABS(CASE WHEN SUM(MRevisiHargaJualD.KoreksiJL)>=1 THEN SUM(MRevisiHargaJualD.KoreksiJL) ELSE 0 END), MRevisiHargaJual.Keterangan  " & vbCrLf & _
    '               " FROM MRevisiHargaJual " & vbCrLf & _
    '               " INNER JOIN MRevisiHargaJualD ON MRevisiHargaJual.NoID=MRevisiHargaJualD.IDRevisiHargaJual " & vbCrLf & _
    '               " GROUP BY MRevisiHargaJual.NoID, MRevisiHargaJual.Tanggal, MRevisiHargaJual.Kode, MRevisiHargaJual.Keterangan, MRevisiHargaJual.IDWilayah, MRevisiHargaJual.IsPosted, MRevisiHargaJual.IDCustomer " & vbCrLf & _
    '               " HAVING MRevisiHargaJual.IDWilayah=1 AND MRevisiHargaJual.IsPosted=1 AND MRevisiHargaJual.IDCustomer=" & NullToLong(txtKodeAlamat.EditValue) & " AND  " & vbCrLf & _
    '               " ABS(CASE WHEN SUM(MRevisiHargaJualD.KoreksiJL)>=1 THEN 0 ELSE SUM(MRevisiHargaJualD.KoreksiJL) END)-ABS(CASE WHEN SUM(MRevisiHargaJualD.KoreksiJL)>=1 THEN SUM(MRevisiHargaJualD.KoreksiJL) ELSE 0 END)<>ISNULL((SELECT SUM(MBayarHutangD.Debet-MBayarHutangD.Kredit) FROM MBayarHutangD INNER JOIN MBayarHutang ON MBayarHutang.NoID=MBayarHutangD.IDBayarHutang WHERE MBayarHutangD.IDTransaksi=MRevisiHargaJual.NoID AND MBayarHutangD.IDJenisTransaksi=15),0) " & vbCrLf
    '        'SQL &= "SELECT MDebetNote.NoID, 19, MDebetNote.Tanggal, MDebetNote.Kode, MDebetNote.Jumlah AS Debet, 0 AS Kredit, MDebetNote.Keterangan " & vbCrLf & _
    '        '       " FROM MDebetNote " & vbCrLf & _
    '        '       " WHERE MDebetNote.IDWilayah=1 AND MDebetNote.IsPosted=1 AND MDebetNote.IDAlamat=" & NullToLong(txtKodeAlamat.EditValue) & " AND " & vbCrLf & _
    '        '       " MDebetNote.Jumlah<>ISNULL((SELECT SUM(MBayarHutangD.Debet-MBayarHutangD.Kredit) FROM MBayarHutangD INNER JOIN MBayarHutang ON MBayarHutang.NoID=MBayarHutangD.IDBayarHutang WHERE MBayarHutangD.IDTransaksi=MDebetNote.NoID AND MBayarHutangD.IDJenisTransaksi=19),0)  " & vbCrLf & _
    '        '       " UNION ALL  " & vbCrLf & _
    '        '       " SELECT MCreditNote.NoID, 20, MCreditNote.Tanggal, MCreditNote.Kode, 0 AS Debet, MCreditNote.Jumlah AS Credit, MCreditNote.Keterangan  " & vbCrLf & _
    '        '       " FROM MCreditNote  " & vbCrLf & _
    '        '       " WHERE MCreditNote.IDWilayah=1 AND MCreditNote.IsPosted=1 AND MCreditNote.IDAlamat=" & NullToLong(txtKodeAlamat.EditValue) & " AND  " & vbCrLf & _
    '        '       " MCreditNote.Jumlah<>ISNULL((SELECT SUM(MBayarHutangD.Kredit-MBayarHutangD.Kredit) FROM MBayarHutangD INNER JOIN MBayarHutang ON MBayarHutang.NoID=MBayarHutangD.IDBayarHutang WHERE MBayarHutangD.IDTransaksi=MCreditNote.NoID AND MBayarHutangD.IDJenisTransaksi=20),0) "
    '        SQL = "SELECT SVR.* FROM (" & SQL & ") AS SVR WHERE SVR.Tanggal>='" & TglDari.DateTime.ToString("yyyy/MM/dd") & "' AND SVR.Tanggal<'" & TglSampai.DateTime.AddDays(1).ToString("yyyy/MM/dd") & "'"
    '        ds = ExecuteDataset("MBayarHutang", SQL)
    '        For i As Integer = 0 To ds.Tables("MBayarHutang").Rows.Count - 1
    '            SQL = "INSERT INTO [MBayarHutangD] ([NoID],[IDBayarHutang],[IDTransaksi],[TanggalTransaksi],[KodeReff],[IDJenisTransaksi],[Debet],[Kredit],[Keterangan]) VALUES (" & vbCrLf & _
    '                  GetNewID("MBayarHutangD", "NoID") & ", " & NoID & "," & vbCrLf & _
    '                  NullToLong(ds.Tables("MBayarHutang").Rows(i).Item("NoID")) & "," & vbCrLf & _
    '                  "'" & NullToDate(ds.Tables("MBayarHutang").Rows(i).Item("Tanggal")).ToString("yyyy-MM-dd") & "'," & vbCrLf & _
    '                  "'" & FixApostropi(NullToStr(ds.Tables("MBayarHutang").Rows(i).Item("Kode"))) & "'," & vbCrLf & _
    '                  NullToLong(ds.Tables("MBayarHutang").Rows(i).Item("IDJenisTransaksi")) & "," & vbCrLf & _
    '                  FixKoma(NullToDbl(ds.Tables("MBayarHutang").Rows(i).Item("Debet"))) & "," & vbCrLf & _
    '                  FixKoma(NullToDbl(ds.Tables("MBayarHutang").Rows(i).Item("Kredit"))) & "," & vbCrLf & _
    '                  "'" & FixApostropi(NullToStr(ds.Tables("MBayarHutang").Rows(i).Item("Keterangan"))) & "'" & vbCrLf & _
    '                  ")"
    '            EksekusiSQL(SQL)
    '        Next
    '    Catch ex As Exception
    '        XtraMessageBox.Show("Info Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
    '    End Try
    'End Sub
    Private Sub cmdLoad_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdLoad.Click
        Try
            If Not IsPosted AndAlso pTipe = pStatus.Edit AndAlso XtraMessageBox.Show("Ingin meload seluruh data transaksi-transaksi yang belum dikontra bon customer " & txtNamaAlamat.Text, NamaAplikasi, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) = Windows.Forms.DialogResult.Yes Then
                SQL = "DELETE FROM MBayarHutangD WHERE IDBayarHutang=" & NoID & " AND IsNull(IsTrue,0)=0"
                EksekusiSQL(SQL)
                'LoadData()
                RefreshDetil()
            End If
        Catch ex As Exception
            XtraMessageBox.Show("Info Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
        End Try
    End Sub

    Private Sub gvPembelian_CellValueChanged(ByVal sender As Object, ByVal e As DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs) Handles gvPembelian.CellValueChanged
        HitungTotal()
    End Sub

    Private Sub gvPembelian_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvPembelian.DoubleClick
        Try
            If IsPosted Then Exit Try
            Dim row As System.Data.DataRow = gvPembelian.GetDataRow(gvPembelian.FocusedRowHandle)
            If NullToDbl(row("Bayar")) = 0 Then
                row("Bayar") = NullToDbl(row("Total"))
            Else
                row("Bayar") = 0
            End If
            Application.DoEvents()
            HitungTotal()
        Catch ex As Exception

        End Try
    End Sub

    Private Sub gcPembelian_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles gcPembelian.Click

    End Sub

    Private Sub gcRetur_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles gcRetur.Click

    End Sub

    Private Sub gcPH_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles gcPH.Click

    End Sub

    Private Sub gvPH_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvPH.DoubleClick
        Dim row As System.Data.DataRow = gvPH.GetDataRow(gvPH.FocusedRowHandle)
        If NullToDbl(row("Potong")) = 0 Then
            row("Potong") = NullToDbl(row("Total"))
        Else
            row("Potong") = 0
        End If

        Application.DoEvents()
        HitungTotal()

    End Sub

    Private Sub gcDebet_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles gcDebet.Click

    End Sub

    Private Sub gvDebet_CellValueChanged(ByVal sender As Object, ByVal e As DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs) Handles gvDebet.CellValueChanged
        Try
            'If gvDebet.RowCount <= 1 Then Exit Try
            Dim row As System.Data.DataRow = gvDebet.GetDataRow(gvDebet.FocusedRowHandle)
            row("Potong") = NullToDbl(row("Total"))
            gvDebet.RefreshData()
            Application.DoEvents()
            HitungTotal()
        Catch ex As Exception

        End Try
    End Sub

    Private Sub gvDebet_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvDebet.DoubleClick
        Try
            'If gvDebet.RowCount <= 1 Then Exit Try
            Dim row As System.Data.DataRow = gvDebet.GetDataRow(gvDebet.FocusedRowHandle)
            If NullToDbl(row("Potong")) = 0 Then
                row("Potong") = NullToDbl(row("Total"))
            Else
                row("Potong") = 0
            End If
            gvKredit.RefreshData()
            Application.DoEvents()
            HitungTotal()
        Catch ex As Exception

        End Try
    End Sub

    Private Sub gvKredit_CellValueChanged(ByVal sender As Object, ByVal e As DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs) Handles gvKredit.CellValueChanged
        Try
            'If gvKredit.RowCount <= 1 Then Exit Try
            Dim row As System.Data.DataRow = gvKredit.GetDataRow(gvKredit.FocusedRowHandle)
            row("Potong") = NullToDbl(row("Total"))
            gvKredit.RefreshData()
            Application.DoEvents()
            HitungTotal()
        Catch ex As Exception

        End Try
    End Sub

    Private Sub gvKredit_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvKredit.DoubleClick
        Try
            'If gvKredit.RowCount <= 1 Then Exit Try
            Dim row As System.Data.DataRow = gvKredit.GetDataRow(gvKredit.FocusedRowHandle)
            If NullToDbl(row("Potong")) = 0 Then
                row("Potong") = NullToDbl(row("Total"))
            Else
                row("Potong") = 0
            End If
            gvKredit.RefreshData()
            Application.DoEvents()
            HitungTotal()
        Catch ex As Exception

        End Try
    End Sub

    Private Sub txtNoTT_ButtonClick(ByVal sender As Object, ByVal e As DevExpress.XtraEditors.Controls.ButtonPressedEventArgs) Handles txtNoTT.ButtonClick
        If e.Button.Index = 1 Then
            Try
                txtTglKembali.DateTime = CDate(EksekusiSQlSkalarNew("SELECT TglKembali FROM MTT WHERE NoID=" & NullToLong(txtNoTT.EditValue)))
            Catch ex As Exception

            End Try
            InsertkanKeDetil()
        End If
    End Sub

    Private Sub txtNoTT_EditValueChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtNoTT.EditValueChanged
        Try
            txtTglKembali.DateTime = CDate(EksekusiSQlSkalarNew("SELECT TglKembali FROM MTT WHERE NoID=" & NullToLong(txtNoTT.EditValue)))
        Catch ex As Exception

        End Try
        InsertkanKeDetil()
    End Sub

    Private Sub txtKwitansi_EditValueChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtKwitansi.EditValueChanged
        Try
            'txtKwitansi.EditValue
            txtJumlahKwitansi.EditValue = NullToDbl(EksekusiSQlSkalarNew("SELECT JUMLAH FROM MKASIN WHERE ID=" & NullToLong(txtKwitansi.EditValue)))
        Catch ex As Exception

        End Try
    End Sub

    Private Sub gvPembelian_FocusedColumnChanged(ByVal sender As Object, ByVal e As DevExpress.XtraGrid.Views.Base.FocusedColumnChangedEventArgs) Handles gvPembelian.FocusedColumnChanged
        'If e.FocusedColumn.FieldName.ToUpper = "Total".ToUpper Then
        '    gvPembelian.OptionsBehavior.Editable = True
        'Else
        '    gvPembelian.OptionsBehavior.Editable = False
        'End If
    End Sub

    Private Sub txtSubTotal_EditValueChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtSubTotal.EditValueChanged
        HitungTotal()
    End Sub

    Private Sub txtJumlahKwitansi_EditValueChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtJumlahKwitansi.EditValueChanged
        HitungTotal()
    End Sub

    Private Sub txtBayar_EditValueChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtBayar.EditValueChanged
        HitungTotal()
    End Sub

    Private Sub txtPotongan_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtPotongan.EditValueChanged
        HitungTotal()
    End Sub

    Private Sub txtMaterai_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtMaterai.EditValueChanged
        HitungTotal()
    End Sub

    Private Sub txtDN_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtDN.EditValueChanged
        HitungTotal()
    End Sub

    Private Sub cmdAddCN_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdAddCN.Click
        Dim hasil As Boolean = True

        Try
            For i As Integer = 0 To gvKredit.RowCount - 1
                If NullToDbl(gvKredit.GetRowCellValue(i, "Total")) = 0 Then
                    hasil = False
                End If
            Next
            If hasil Then
                gvKredit.AddNewRow()
                gvKredit.RefreshData()
                Application.DoEvents()
                If gvKredit.RowCount = 0 Then
                    gvKredit.RefreshData()
                    Application.DoEvents()
                End If
                For i As Integer = 0 To gvKredit.RowCount - 1
                    If NullToLong(gvKredit.GetRowCellValue(i, "NoID")) = 0 Then
                        gvKredit.ClearSelection()
                        gvKredit.FocusedRowHandle = i
                        gvKredit.SelectRow(gvKredit.FocusedRowHandle)
                    End If
                Next
                gvKredit.SetRowCellValue(gvKredit.FocusedRowHandle, "NoID", gvKredit.RowCount)
            End If
        Catch ex As Exception
            XtraMessageBox.Show("Info Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
        End Try
    End Sub

    Private Sub cmdDelCN_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdDelCN.Click
        Dim hasil As Boolean = True
        Try
            gvKredit.DeleteSelectedRows()
            HitungTotal()
        Catch ex As Exception
            XtraMessageBox.Show("Info Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
        End Try
    End Sub

    Private Sub cmdDelDN_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdDelDN.Click
        Dim hasil As Boolean = True
        Try
            gvDebet.DeleteSelectedRows()
            HitungTotal()
        Catch ex As Exception
            XtraMessageBox.Show("Info Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
        End Try
    End Sub

    Private Sub cmdAddDN_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdAddDN.Click
        Dim hasil As Boolean = True
        Try
            For i As Integer = 0 To gvDebet.RowCount - 1
                If NullToDbl(gvDebet.GetRowCellValue(i, "Total")) = 0 Then
                    hasil = False
                End If
            Next
            If hasil Then
                gvDebet.AddNewRow()
                gvDebet.RefreshData()
                Application.DoEvents()
                If gvDebet.RowCount = 0 Then
                    gvDebet.RefreshData()
                    Application.DoEvents()
                End If
                For i As Integer = 0 To gvDebet.RowCount - 1
                    If NullToLong(gvDebet.GetRowCellValue(i, "NoID")) = 0 Then
                        gvDebet.ClearSelection()
                        gvDebet.FocusedRowHandle = i
                        gvDebet.SelectRow(gvDebet.FocusedRowHandle)
                    End If
                Next
                gvDebet.SetRowCellValue(gvDebet.FocusedRowHandle, "NoID", gvDebet.RowCount)
            End If
        Catch ex As Exception
            XtraMessageBox.Show("Info Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
        End Try
    End Sub

    Private Sub gcKredit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles gcKredit.Click

    End Sub

    Private Sub SimpleButton1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SimpleButton1.Click
        Dim hasil As Boolean = True
        Try
            For i As Integer = 0 To gviGiro.RowCount - 1
                If NullToDbl(gviGiro.GetRowCellValue(i, "Total")) = 0 Then
                    hasil = False
                End If
            Next
            If hasil Then
                gviGiro.AddNewRow()
                gviGiro.RefreshData()
                Application.DoEvents()
                If gviGiro.RowCount = 0 Then
                    gviGiro.RefreshData()
                    Application.DoEvents()
                End If
                For i As Integer = 0 To gviGiro.RowCount - 1
                    If NullToLong(gviGiro.GetRowCellValue(i, "NoID")) = 0 Then
                        gviGiro.ClearSelection()
                        gviGiro.FocusedRowHandle = i
                        gviGiro.SelectRow(gviGiro.FocusedRowHandle)
                    End If
                Next
                gviGiro.SetRowCellValue(gviGiro.FocusedRowHandle, "NoID", gviGiro.RowCount)
            End If
        Catch ex As Exception
            XtraMessageBox.Show("Info Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
        End Try
    End Sub

    Private Sub SimpleButton2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SimpleButton2.Click
        Dim hasil As Boolean = True
        Try
            gviGiro.DeleteSelectedRows()
            HitungTotal()
        Catch ex As Exception
            XtraMessageBox.Show("Info Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
        End Try
    End Sub
End Class