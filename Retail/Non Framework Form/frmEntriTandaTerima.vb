Imports System.Data
Imports System.Data.SqlClient
Imports DevExpress.XtraEditors
Imports DevExpress.Utils
Imports DevExpress.XtraGrid.Views.Base

Public Class frmEntriTandaTerima
    Dim SQL As String = ""
    Public Enum pStatus
        Baru = 0
        Edit = 1
    End Enum
    Public pTipe As pStatus
    Public Enum pTransaksi
        Supplier = 0
        Customer = 1
    End Enum
    
    Public pTrans As pTransaksi
    Public IsRevisi As Boolean = False

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

    Private Sub RefreshDataKontak()
        Dim ds As New DataSet
        Try
            If pTrans = pTransaksi.Supplier Then
                SQL = "SELECT NoID, Kode, Nama, Alamat FROM MAlamat WHERE IsActive=1 AND IsSupplier=1"
            ElseIf pTrans = pTransaksi.Customer Then
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
        Catch ex As Exception
            XtraMessageBox.Show("Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            ds.Dispose()
        End Try
    End Sub
    Dim DefImageList As New ImageList
    Private Sub frmEntriSO_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        If IsTempInsertBaru Then
            If Not NullToBool(EksekusiSQlSkalarNew("SELECT IsPosted FROM MTT WHERE NoID=" & NoID)) Then
                If XtraMessageBox.Show("Yakin ingin keluar?" & vbCrLf & "Ya, akan menghapus transaksi dan detil item.", NamaAplikasi, MessageBoxButtons.YesNo, MessageBoxIcon.Information, MessageBoxDefaultButton.Button2) = Windows.Forms.DialogResult.Yes Then
                    EksekusiSQL("DELETE FROM MTT WHERE NoID=" & NoID)
                    EksekusiSQL("DELETE FROM MTTD WHERE IDTT=" & NoID)
                Else
                    e.Cancel = True
                End If
            End If
        Else
            EksekusiSQL("UPDATE MTT SET IsEdit=0, UserEdit=NULL WHERE NoID=" & NoID)
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

            TglDari.DateTime = CDate(TanggalSystem.ToString("yyyy/MM/01"))
            TglSampai.DateTime = TanggalSystem

            SetTombol()
            RefreshData()
            SetTombol()
            Me.Width = Ini.BacaIniPath(FolderLayouts & Me.Name & ".ini", "Form", "Width", Me.Width)
            Me.Height = Ini.BacaIniPath(FolderLayouts & Me.Name & ".ini", "Form", "Height", Me.Height)

            Me.Left = (Screen.PrimaryScreen.WorkingArea.Width - Me.Width) \ 2
            Me.Top = (Screen.PrimaryScreen.WorkingArea.Height - Me.Height) \ 2

            If System.IO.File.Exists(FolderLayouts & Me.Name & LayoutControl1.Name & ".xml") Then
                LayoutControl1.RestoreLayoutFromXml(FolderLayouts & Me.Name & LayoutControl1.Name & ".xml")
            End If
            If System.IO.File.Exists(FolderLayouts & Me.Name & GV1.Name & ".xml") Then
                GV1.RestoreLayoutFromXml(FolderLayouts & Me.Name & GV1.Name & ".xml")
            End If
            If System.IO.File.Exists(FolderLayouts & Me.Name & GridView1.Name & ".xml") Then
                GridView1.RestoreLayoutFromXml(FolderLayouts & Me.Name & GridView1.Name & ".xml")
            End If
            If System.IO.File.Exists(FolderLayouts & Me.Name & SearchLookUpEdit1View.Name & ".xml") Then
                SearchLookUpEdit1View.RestoreLayoutFromXml(FolderLayouts & Me.Name & SearchLookUpEdit1View.Name & ".xml")
            End If
            Me.Text &= IIf(pTrans = pTransaksi.Supplier, " Supplier", " Customer")
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
                SQL = "SELECT MTT.*, MUserEntri.Nama AS UserEntri, MUserEdit.Nama AS UserEdit, MUserPosting.Nama AS UserPosted, MAlamat.Nama AS NamaAlamat, MAlamat.Kode as KodeAlamat "
                SQL &= " FROM MTT LEFT JOIN MAlamat ON MAlamat.NoID=MTT.IDCustomer "
                SQL &= " LEFT JOIN MUser MUserEntri ON MUserEntri.NoID=MTT.IDUserEntry "
                SQL &= " LEFT JOIN MUser MUserEdit ON MUserEdit.NoID=MTT.IDUserEdit "
                SQL &= " LEFT JOIN MUser MUserPosting ON MUserPosting.NoID=MTT.IDUserPosted "
                SQL &= " WHERE MTT.NoID = " & NoID
                DS = ExecuteDataset("Tabel", SQL)
                If DS.Tables("Tabel").Rows.Count >= 1 Then
                    txtKodeAlamat.EditValue = NullToLong(DS.Tables(0).Rows(0).Item("IDCustomer"))
                    txtWilayah.EditValue = NullToLong(DS.Tables(0).Rows(0).Item("IDWilayah"))
                    txtNamaAlamat.Text = NullToStr(DS.Tables(0).Rows(0).Item("NamaAlamat"))
                    txtKode.Text = NullToStr(DS.Tables(0).Rows(0).Item("Kode"))
                    KodeLama = txtKode.Text
                    Tgl.DateTime = NullToDate(DS.Tables(0).Rows(0).Item("Tanggal"))
                    tglJatuhTempo.DateTime = NullToDate(DS.Tables(0).Rows(0).Item("TglKembali"))
                    txtSubtotal.EditValue = NullToDbl(DS.Tables(0).Rows(0).Item("SubTotal"))
                    IsPosted = NullToBool(DS.Tables(0).Rows(0).Item("IsPosted"))
                    ckPB.Checked = NullToBool(DS.Tables(0).Rows(0).Item("IsProdukBaru"))
                    ckTulisanPPN.Checked = NullToBool(DS.Tables(0).Rows(0).Item("IsTulisanPPN"))
                    txtDientriOleh.Text = NullToStr(DS.Tables(0).Rows(0).Item("UserEntri"))
                    txtDieditOleh.Text = NullToStr(DS.Tables(0).Rows(0).Item("UserEdit"))
                    txtDipostingOleh.Text = NullToStr(DS.Tables(0).Rows(0).Item("UserPosted"))
                    tglEntri.DateTime = NullToDate(DS.Tables(0).Rows(0).Item("TglEntry"))
                    tglEdit.DateTime = NullToDate(DS.Tables(0).Rows(0).Item("TglEdit"))
                    tglPosting.DateTime = NullToDate(DS.Tables(0).Rows(0).Item("TglPosted"))
                    txtCatatan.Text = NullToStr(DS.Tables(0).Rows(0).Item("Catatan"))
                    txtBGAtasNama.Text = NullToStr(DS.Tables(0).Rows(0).Item("BgAtasNama"))
                    If IsPosted AndAlso Not IsSaveSelesai AndAlso Not IsRevisi Then
                        txtKodeAlamat.Properties.ReadOnly = True
                        cmdSave.Enabled = False
                    ElseIf IsPosted AndAlso IsSaveSelesai Then
                        txtKodeAlamat.Properties.ReadOnly = True
                        cmdSave.Enabled = True
                    End If
                    EksekusiSQL("Update MTT SET IsEdit=1, UserEdit='" & FixApostropi(NamaUserAktif) & "' WHERE NoID=" & NoID)
                Else
                    IsiDefault()
                End If
            End If
        Catch ex As Exception
            XtraMessageBox.Show("Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            RefreshDetil()
            DS.Dispose()
        End Try
    End Sub
    Public Sub RefreshDetil()
        Dim strsql As String
        Dim curentcursor As Cursor = Windows.Forms.Cursor.Current
        Dim repChekEdit As New DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit
        Windows.Forms.Cursor.Current = Cursors.WaitCursor
        Try
            strsql = "SELECT MTTD.*, " & IIf(pTrans = pTransaksi.Supplier, " MTTD.Kredit-MTTD.Debet AS Jumlah, ", " MTTD.Debet-MTTD.Kredit AS Jumlah, ") & " MJenisTransaksi.Nama AS Transaksi, MBeli.Kode AS NoBPB, MBeli.Tanggal AS TglTransaksi, MPO.Kode AS NoSPP, " & vbCrLf & _
                     IIf(pTrans = pTransaksi.Supplier, " MReturBeli.Kode AS KodeRetur", " MReturJual.Kode AS KodeRetur, ") & vbCrLf & _
                     " FROM MTTD " & vbCrLf & _
                     " INNER JOIN MTT ON MTT.NoID=MTTD.IDTT" & vbCrLf & _
                     " LEFT JOIN MBeli ON MBeli.NoID=MTTD.IDTransaksi " & vbCrLf & _
                     " LEFT JOIN MPO ON MPO.NoID=MTTD.IDPO " & vbCrLf & _
                     " LEFT JOIN MReturBeli ON MReturBeli.NoID=MTTD.IDRetur " & vbCrLf & _
                     " LEFT JOIN MReturJual ON MReturJual.NoID=MTTD.IDRetur " & vbCrLf & _
                     " LEFT JOIN MJenisTransaksi ON MJenisTransaksi.ID=MTTD.IDJenisTransaksi" & vbCrLf & _
                     " WHERE MTT.NoID=" & NoID
            ExecuteDBGrid(GC1, strsql & " AND IsNull(MTTD.IsTrue,0)=1", "NoID")
            ExecuteDBGrid(GridControl1, strsql & " AND IsNull(MTTD.IsTrue,0)=0", "NoID")
            'SetGridView(GC1)
            For x As Integer = 0 To GV1.Columns.Count - 1
                Select Case GV1.Columns(x).ColumnType.Name.ToLower
                    Case "int32", "int64", "int"
                        GV1.Columns(x).DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
                        GV1.Columns(x).DisplayFormat.FormatString = "n0"
                    Case "decimal", "single", "money", "double"
                        Select Case GV1.Columns(x).FieldName.Trim.ToLower
                            Case "discpersen1", "discpersen2", "discpersen3"
                                GV1.Columns(x).DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
                                GV1.Columns(x).DisplayFormat.FormatString = "n2"
                            Case "ctn"
                                GV1.Columns(x).DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
                                GV1.Columns(x).DisplayFormat.FormatString = "n3"
                            Case Else
                                GV1.Columns(x).DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
                                GV1.Columns(x).DisplayFormat.FormatString = "n2"
                        End Select
                    Case "string"
                        GV1.Columns(x).DisplayFormat.FormatType = DevExpress.Utils.FormatType.None
                        GV1.Columns(x).DisplayFormat.FormatString = ""
                    Case "date", "datetime"
                        If GV1.Columns(x).FieldName.Trim.ToLower = "jam" Then
                            GV1.Columns(x).DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime
                            GV1.Columns(x).DisplayFormat.FormatString = "HH:mm"
                        Else
                            GV1.Columns(x).DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime
                            GV1.Columns(x).DisplayFormat.FormatString = "dd-MM-yyyy"
                        End If
                    Case "boolean"
                        GV1.Columns(x).ColumnEdit = repChekEdit
                End Select
            Next
            For x As Integer = 0 To GridView1.Columns.Count - 1
                Select Case GridView1.Columns(x).ColumnType.Name.ToLower
                    Case "int32", "int64", "int"
                        GridView1.Columns(x).DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
                        GridView1.Columns(x).DisplayFormat.FormatString = "n0"
                    Case "decimal", "single", "money", "double"
                        Select Case GridView1.Columns(x).FieldName.Trim.ToLower
                            Case "discpersen1", "discpersen2", "discpersen3"
                                GridView1.Columns(x).DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
                                GridView1.Columns(x).DisplayFormat.FormatString = "n2"
                            Case "ctn"
                                GridView1.Columns(x).DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
                                GridView1.Columns(x).DisplayFormat.FormatString = "n3"
                            Case Else
                                GridView1.Columns(x).DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
                                GridView1.Columns(x).DisplayFormat.FormatString = "n2"
                        End Select
                    Case "string"
                        GridView1.Columns(x).DisplayFormat.FormatType = DevExpress.Utils.FormatType.None
                        GridView1.Columns(x).DisplayFormat.FormatString = ""
                    Case "date", "datetime"
                        If GridView1.Columns(x).FieldName.Trim.ToLower = "jam" Then
                            GridView1.Columns(x).DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime
                            GridView1.Columns(x).DisplayFormat.FormatString = "HH:mm"
                        Else
                            GridView1.Columns(x).DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime
                            GridView1.Columns(x).DisplayFormat.FormatString = "dd-MM-yyyy"
                        End If
                    Case "boolean"
                        GridView1.Columns(x).ColumnEdit = repChekEdit
                End Select
            Next
            HitungTotal()
            GV1.HideFindPanel()
            GridView1.HideFindPanel()
        Catch ex As Exception
            XtraMessageBox.Show(ex.Message, ".:: Pesan Kesalahan ::.", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
        Windows.Forms.Cursor.Current = curentcursor
        Application.DoEvents()
    End Sub
    Private Sub HitungTotal()
        Dim temp As Double = 0.0
        Try
            For i = 0 To GV1.RowCount
                'If pTrans = pTransaksi.Supplier Then
                '    temp = temp - NullToDbl(GV1.GetRowCellValue(i, "Debet")) + NullToDbl(GV1.GetRowCellValue(i, "Kredit"))
                'Else
                temp = temp + NullToDbl(GV1.GetRowCellValue(i, "Sisa"))
                'End If
            Next
            txtSubtotal.EditValue = temp
        Catch ex As Exception
            XtraMessageBox.Show("Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub
    Private Sub IsiDefault()
        Tgl.DateTime = TanggalSystem
        tglJatuhTempo.DateTime = DateAdd(DateInterval.Month, 2, Tgl.DateTime)
        txtWilayah.EditValue = DefIDWilayah
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
            If IsRevisi Then
                cmdEdit.Enabled = True
            Else
                cmdEdit.Enabled = False
            End If
            cmdDelete.Enabled = False
            cmdLoad.Enabled = False

            SimpleButton1.Enabled = False
            SimpleButton2.Enabled = False
            SimpleButton3.Enabled = False
            SimpleButton4.Enabled = False
        Else
            cmdBAru.Enabled = True
            cmdEdit.Enabled = True
            cmdDelete.Enabled = True
            cmdLoad.Enabled = True

            SimpleButton1.Enabled = True
            SimpleButton2.Enabled = True
            SimpleButton3.Enabled = True
            SimpleButton4.Enabled = True
        End If
    End Sub
    Private Sub SimpanDetil()
        Try
            SQL = "DELETE FROM MTTD WHERE IDTT=" & NoID & " AND IsNull(IsTrue,0)=0"
            EksekusiSQL(SQL)
        Catch ex As Exception
            XtraMessageBox.Show("Info Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
        End Try
    End Sub
    Private Sub cmdSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSave.Click
        Dim PesanSalah As String = ""
        If (Not IsPosted AndAlso Not IsRevisi) Or (IsPosted AndAlso IsRevisi) Then
            If IsValidasi() Then
                SimpanDetil()
                If Simpan(PesanSalah) = True Then
                    If pTipe = pStatus.Baru Then
                        KodeLama = txtKode.Text
                        pTipe = pStatus.Edit
                        SetTombol()
                        txtBarang.Focus()
                        'txtKodeCustomer.Properties.ReadOnly = True
                        IsTempInsertBaru = True
                    Else
                        clsPostingPembelian.PostingTandaTerimaSupplier(NoID)
                        'KODE DIBAWAH UNTUK MEMBUKA FORM HANYA 1
                        Dim frmEntri As frmDaftarMasterDetil = Nothing
                        Dim F As Object
                        For Each F In MdiParent.MdiChildren
                            If TypeOf F Is frmDaftarMasterDetil Then
                                frmEntri = F
                                If frmEntri.FormName = FormNameDaftar Then
                                    Exit For
                                Else
                                    frmEntri = Nothing
                                End If
                            End If
                        Next
                        If frmEntri Is Nothing Then
                            frmEntri = New frmDaftarMasterDetil
                            frmEntri.WindowState = FormWindowState.Maximized
                            frmEntri.MdiParent = MdiParent
                            frmEntri.FormName = FormNameDaftar
                            frmEntri.TableName = TableNameDaftar
                            frmEntri.Text = TextDaftar
                            frmEntri.FormEntriName = FormEntriDaftar
                            frmEntri.TableMaster = TableMasterDaftar
                        End If
                        frmEntri.DirectNoID = NoID
                        frmEntri.ShowNoID = True
                        frmEntri.Show()
                        frmEntri.Focus()

                        IsTempInsertBaru = False
                        'DialogResult = Windows.Forms.DialogResult.OK
                        Close()
                        Dispose()
                    End If
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
        If txtWilayah.Text = "" Then
            XtraMessageBox.Show("Wilayah masih kosong.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
            txtWilayah.Focus()
            Return False
            Exit Function
        End If
        If pTipe = pStatus.Baru AndAlso txtKode.Properties.ReadOnly Then
            txtKode.Text = clsKode.MintaKodeSPPBaru("TT", "MTT", Tgl.DateTime, DefIDWilayah, 5, , False) ' & IIf(pTrans = pTransaksi.Supplier, "S", "C"), "MTT", Tgl.DateTime, NullToLong(txtWilayah.EditValue), 5, " IsSupplier=" & IIf(pTrans = pTransaksi.Supplier, 1, 0))' clsKode.MintaKodeBaru("PA" & IIf(pTrans = pTransaksi.Supplier, "S", "C"), "MTT", Tgl.DateTime, NullToLong(txtWilayah.EditValue), 5, " IsSupplier=" & IIf(pTrans = pTransaksi.Supplier, 1, 0))
        End If
        If CekKodeValid(txtKode.Text, KodeLama, "MTT", "Kode", IIf(pTipe = pStatus.Edit, True, False)) Then
            XtraMessageBox.Show("Kode sudah dipakai.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
            txtKode.Focus()
            Return False
            Exit Function
        End If
        If pTipe = pStatus.Edit AndAlso GV1.RowCount <= 0 Then
            XtraMessageBox.Show("Item detil masih kosong." & vbCrLf & "Isi item detil atau tutup bila ingin membatalkan.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
            Return False
            Exit Function
        End If
        If clsKode.NotaDiPosting("MTT", NoID) AndAlso Not IsRevisi Then
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
                NoID = GetNewID("MTT")
                SQL = "INSERT INTO [MTT] ([NoID],[Kode],[Tanggal],[TglKembali],[IDCustomer],[IDDepartemen],[IDWilayah],[Keterangan],[IDUserEntry],[TglEntry],[IDUserEdit],[TglEdit],[IsEdit],[Subtotal],[Total],[BgAtasnama],[Catatan],IsProdukBaru,[IsSupplier],IsTulisanPPN) VALUES (" & vbCrLf & _
                      NoID & ",'" & FixApostropi(txtKode.Text) & "','" & Tgl.DateTime.ToString("yyyy-MM-dd HH:mm") & "','" & tglJatuhTempo.DateTime.ToString("yyyy-MM-dd HH:mm") & "'," & NullToLong(txtKodeAlamat.EditValue) & "," & DefIDDepartemen & "," & vbCrLf & _
                      DefIDWilayah & ",'" & FixApostropi(txtCatatan.Text) & "'," & IDUserAktif & ",GETDATE()," & IDUserAktif & ",Getdate(),1," & FixKoma(txtSubtotal.EditValue) & "," & FixKoma(txtSubtotal.EditValue) & ",'" & FixApostropi(txtBGAtasNama.Text) & "','" & FixApostropi(txtCatatan.Text) & "'," & IIf(ckPB.Checked, 1, 0) & "," & IIf(pTrans = pTransaksi.Supplier, 1, 0) & "," & IIf(ckTulisanPPN.Checked, 1, 0) & ")"
            Else
                SQL = "UPDATE [MTT] SET " & vbCrLf & _
                      " [Kode]='" & FixApostropi(txtKode.Text) & "'," & vbCrLf & _
                      " [Tanggal]='" & Tgl.DateTime.ToString("yyyy-MM-dd HH:mm") & "'," & vbCrLf & _
                      " [TglKembali]='" & tglJatuhTempo.DateTime.ToString("yyyy-MM-dd HH:mm") & "'," & vbCrLf & _
                      " [IDCustomer]=" & NullToLong(txtKodeAlamat.EditValue) & "," & vbCrLf & _
                      " [IDDepartemen]=" & DefIDDepartemen & "," & vbCrLf & _
                      " [IDWilayah]=" & DefIDWilayah & "," & vbCrLf & _
                      " [Keterangan]='" & FixApostropi(txtCatatan.Text) & "'," & vbCrLf & _
                      " [IDUserEntry]= " & IIf(IsRevisi, "IDUserEntry", IDUserAktif) & "," & vbCrLf & _
                      " [IDUserEdit]=" & IDUserAktif & "," & vbCrLf & _
                      " [TglEdit]=NULL," & vbCrLf & _
                      " [IsEdit]=NULL," & vbCrLf & _
                      " [IsProdukBaru]=" & IIf(ckPB.Checked, 1, 0) & "," & vbCrLf & _
                      " [Catatan]='" & FixApostropi(txtCatatan.Text) & "'," & vbCrLf & _
                      " [BgAtasNama]='" & FixApostropi(txtBGAtasNama.Text) & "'," & vbCrLf & _
                      " [Subtotal]=" & FixKoma(txtSubtotal.EditValue) & "," & vbCrLf & _
                      " [IsSupplier]=" & IIf(pTrans = pTransaksi.Supplier, 1, 0) & "," & vbCrLf & _
                      " [IsTulisanPPN]=" & IIf(ckTulisanPPN.Checked, 1, 0) & "," & vbCrLf & _
                      " [Total]=" & FixKoma(txtSubtotal.EditValue) & " " & vbCrLf & _
                      " WHERE NoID=" & NoID
            End If
            If EksekusiSQL(SQL) >= 1 Then
                Sukses = True
            Else
                Sukses = False
            End If
            If Sukses Then
                If IsRevisi Then
                    EksekusiSQL("UPDATE MTT SET IsRevisi=1 WHERE NoID=" & NoID)
                Else
                    EksekusiSQL("UPDATE MTT SET IsRevisi=0 WHERE NoID=" & NoID)
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
                GV1.SaveLayoutToXml(FolderLayouts & Me.Name & GV1.Name & ".xml")
                GridView1.SaveLayoutToXml(FolderLayouts & Me.Name & GridView1.Name & ".xml")
                SearchLookUpEdit1View.SaveLayoutToXml(FolderLayouts & Me.Name & SearchLookUpEdit1View.Name & ".xml")
                gvWilayah.SaveLayoutToXml(FolderLayouts & Me.Name & gvWilayah.Name & ".xml")
            End If
        Catch ex As Exception
            XtraMessageBox.Show("Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            xOtorisasi.Dispose()
        End Try
    End Sub

    Private Sub cmdRefresh_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdRefresh.Click
        'If Not IsPosted Then
        '    LoadData()
        'End If
        RefreshDetil()
    End Sub

    Private Sub cmdDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdDelete.Click
        Dim view As ColumnView = GC1.FocusedView
        Try
            Dim row As System.Data.DataRow = view.GetDataRow(GV1.FocusedRowHandle)
            Dim dc As Integer = GV1.FocusedRowHandle
            Dim IDDetil As Long = NullToLong(row("NoID"))
            If XtraMessageBox.Show("Item ini mau dihapus?", NamaAplikasi, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) = Windows.Forms.DialogResult.Yes Then
                EksekusiSQL("Delete From MTTD where NoID=" & IDDetil.ToString)
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
            txtBGAtasNama.Text = NullToStr(EksekusiSQlSkalarNew("SELECT NamaRekening1 FROM MAlamat WHERE NoID=" & NullToLong(txtKodeAlamat.EditValue)))
            TglAdd = NullToLong(EksekusiSQlSkalarNew("SELECT JatuhTempoCustomer FROM MAlamat WHERE NoID=" & NullToLong(txtKodeAlamat.EditValue)))
            If TglAdd = 0 Then
                tglJatuhTempo.DateTime = DateAdd(DateInterval.Month, 2, Tgl.DateTime)
            Else
                tglJatuhTempo.DateTime = DateAdd(DateInterval.Day, TglAdd, Tgl.DateTime)
            End If
        Catch ex As Exception

        End Try
    End Sub

    Private Sub Tgl_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Tgl.EditValueChanged
        Dim TglAdd As Long = 0
        Try
            'txtNamaCustomer.Text = NullTostr(SearchLookUpEdit1View.GetRowCellValue(SearchLookUpEdit1View.FocusedRowHandle, "Nama"))
            'txtAlamatCustomer.Text = NullTostr(EksekusiSQlSkalarNew("SELECT Alamat FROM MAlamat WHERE NoID=" & NullTolong(txtKodeCustomer.EditValue)))
            TglAdd = NullToLong(EksekusiSQlSkalarNew("SELECT JatuhTempoCustomer FROM MAlamat WHERE NoID=" & NullToLong(txtKodeAlamat.EditValue)))
            If TglAdd = 0 Then
                tglJatuhTempo.DateTime = DateAdd(DateInterval.Month, 2, Tgl.DateTime)
            Else
                tglJatuhTempo.DateTime = DateAdd(DateInterval.Day, TglAdd, Tgl.DateTime)
            End If
            If pTipe = pStatus.Baru Then
                txtKode.Text = clsKode.MintaKodeSPPBaru("TT", "MTT", Tgl.DateTime, DefIDWilayah, 5, , False) ' & IIf(pTrans = pTransaksi.Supplier, "S", "C"), "MTT", Tgl.DateTime, NullToLong(txtWilayah.EditValue), 5, " IsSupplier=" & IIf(pTrans = pTransaksi.Supplier, 1, 0))
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
    '                'IDDetil = GetNewID("MTTD", "NoID")
    '                'Konversi = NullToDbl(EksekusiSQlSkalarNew("SELECT Konversi FROM MBarangD WHERE IsJual=1 AND IDBarang=" & IDBarang & " AND IDSatuan=" & DefIDSatuan))
    '                'Harga = clsPostingPenjualan.HargaJual(NullToLong(IDBarang), NullToLong(DefIDSatuan), NullToLong(txtKodeCustomer.EditValue), 0, 0)
    '                'If Konversi <> 0 Then
    '                '    SQL = "INSERT INTO MTTD (NoID,IDWilayah,IDBarang,IDSatuan,Konversi,IDTT,NoUrut,Tgl,Jam,Harga) VALUES " & vbCrLf
    '                '    SQL &= "(" & IDDetil & "," & NullToLong(txtWilayah.EditValue) & "," & IDBarang & "," & DefIDSatuan & "," & FixKoma(Konversi) & "," & NoID & "," & GetNewID("MTTD", "NoUrut", " WHERE IDTT=" & NoID) & ",GetDate(),GetDate()," & FixKoma(Harga) & ")"
    '                'Else
    '                '    SQL = "INSERT INTO MTTD (NoID,IDWilayah,IDBarang,IDTT,NoUrut,Tgl,Jam,Harga) VALUES " & vbCrLf
    '                '    SQL &= "(" & IDDetil & "," & NullToLong(txtWilayah.EditValue) & "," & IDBarang & "," & NoID & "," & GetNewID("MTTD", "NoUrut", " WHERE IDTT=" & NoID) & ",GetDate(),GetDate()," & FixKoma(Harga) & ")"
    '                'End If
    '                'EksekusiSQL(SQL)
    '                frmEntri.IsNew = True
    '                frmEntri.NoID = IDDetil
    '                frmEntri.IDTT = NoID
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
    '                '    SQL = "DELETE FROM MTTD WHERE NoID=" & IDDetil
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
        Dim frmEntri As frmEntriTandaTerimaD = Nothing
        Dim x As New frmOtorisasiAdmin
        Dim IDDetil As Long = -1
        Try
            If NullToLong(EksekusiSQlSkalarNew("SELECT COUNT(NoID) FROM MTTD WHERE IDTT=" & NoID)) >= 8 Then
                If XtraMessageBox.Show("Item sudah lebih atau sama dengan 8." & vbCrLf & "No untuk membatalkan dan yes untuk melanjutkan?", NamaAplikasi, MessageBoxButtons.YesNo, MessageBoxIcon.Warning) = Windows.Forms.DialogResult.Yes Then
                    If x.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
                        x.Dispose()
                    Else
                        x.Dispose()
                        Exit Try
                    End If
                Else
                    x.Dispose()
                    Exit Try
                End If
            Else
                x.Dispose()
            End If
            frmEntri = New frmEntriTandaTerimaD
            frmEntri.NoID = IDDetil
            frmEntri.IsNew = True
            If txtKodeAlamat.Text <> "" Then
                frmEntri.IDSupplier = NullToLong(txtKodeAlamat.EditValue)
                frmEntri.NoID = IDDetil
                frmEntri.IsNew = True
                frmEntri.IDTandaTerima = NoID
                frmEntri.FormPemanggil = Me

                frmEntri.Show()
                frmEntri.Focus()
                frmEntri.txtSPP.Focus()
            Else
                XtraMessageBox.Show("Isi dulu Supplier, lalu click baru.", NamaAplikasi, MessageBoxButtons.OK)
            End If
        Catch ex As Exception
            XtraMessageBox.Show("Untuk membuat baru tekan tombol baru", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            x.Dispose()
        End Try
    End Sub

    Private Sub cmdEdit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdEdit.Click
        Dim view As ColumnView = GC1.FocusedView
        Dim frmEntri As New frmEntriTandaTerimaD
        Try
            Dim row As System.Data.DataRow = view.GetDataRow(GV1.FocusedRowHandle)
            Dim dc As Integer = GV1.FocusedRowHandle
            Dim IDDetil As Long = NullToLong(row("NoID"))
            If txtKodeAlamat.Text <> "" Then
                frmEntri.IDSupplier = NullToLong(txtKodeAlamat.EditValue)
                frmEntri.IsNew = False
                frmEntri.NoID = IDDetil
                frmEntri.IDTandaTerima = NoID

                frmEntri.FormPemanggil = Me
                frmEntri.Show()
                frmEntri.Focus()
            Else
                XtraMessageBox.Show("Isi dulu Supplier, lalu click Edit.", NamaAplikasi, MessageBoxButtons.OK)
            End If
        Catch ex As Exception
            XtraMessageBox.Show("Untuk mengedit pilih item satuan yang akan diedit terlebih dahulu lalu tekan tombol edit", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
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
                        tglJatuhTempo.Properties.ReadOnly = False
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
            txtKode.Text = clsKode.MintaKodeSPPBaru("TT", "MTT", Tgl.DateTime, DefIDWilayah, 5, , False) ' & IIf(pTrans = pTransaksi.Supplier, "S", "C"), "MTT", Tgl.DateTime, NullToLong(txtWilayah.EditValue), 5, " IsSupplier=" & IIf(pTrans = pTransaksi.Supplier, 1, 0))'clsKode.MintaKodeBaru("PA" & IIf(pTrans = pTransaksi.Supplier, "S", "C"), "MTT", Tgl.DateTime, NullToLong(txtWilayah.EditValue), 5, " IsSupplier=" & IIf(pTrans = pTransaksi.Supplier, 1, 0))
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

    Private Sub GV1_FocusedColumnChanged(ByVal sender As Object, ByVal e As DevExpress.XtraGrid.Views.Base.FocusedColumnChangedEventArgs) Handles GV1.FocusedColumnChanged
        If IsPosted AndAlso IsRevisi AndAlso IsSupervisor AndAlso GV1.FocusedColumn.FieldName.ToLower = "IsSelesai".ToLower Then
            GV1.OptionsBehavior.Editable = True
        Else
            GV1.OptionsBehavior.Editable = False
        End If
    End Sub

    'Private Sub LoadData()
    '    Dim SQL As String = ""
    '    Dim ds As New DataSet
    '    Try
    '        'Pembelian
    '        SQL = "SELECT MBeli.NoID, 2 AS IDJenisTransaksi, MBeli.Tanggal, MBeli.Kode, 0 AS Debet, MBeli.Total Kredit, MBeli.Keterangan " & vbCrLf & _
    '              " FROM MBeli " & vbCrLf & _
    '              " WHERE MBeli.IDWilayah=1 AND MBeli.IsPosted=1 AND MBeli.IDSupplier=" & NullToLong(txtKodeAlamat.EditValue) & " AND  " & vbCrLf & _
    '              " (0-MBeli.Total)<>ISNULL((SELECT SUM(MTTD.Debet-MTTD.Kredit) FROM MTTD INNER JOIN MTT ON MTT.NoID=MTTD.IDTT WHERE MTTD.IDTransaksi=MBeli.NoID AND MTTD.IDJenisTransaksi=2),0) " & vbCrLf & _
    '              " UNION ALL  " & vbCrLf & _
    '              " SELECT MReturBeli.NoID, 3, MReturBeli.Tanggal, MReturBeli.Kode, MReturBeli.Total, 0, MReturBeli.Keterangan  " & vbCrLf & _
    '              " FROM MReturBeli " & vbCrLf & _
    '              " WHERE MReturBeli.IDWilayah=1 AND MReturBeli.IsPosted=1 AND MReturBeli.IDSupplier=" & NullToLong(txtKodeAlamat.EditValue) & " AND  " & vbCrLf & _
    '              " (MReturBeli.Total-0)<>ISNULL((SELECT SUM(MTTD.Debet-MTTD.Kredit) FROM MTTD INNER JOIN MTT ON MTT.NoID=MTTD.IDTT WHERE MTTD.IDTransaksi=MReturBeli.NoID AND MTTD.IDJenisTransaksi=3),0) " & vbCrLf & _
    '              " UNION ALL " & vbCrLf & _
    '              " SELECT MRevisiHargaBeli.NoID, 11, MRevisiHargaBeli.Tanggal, MRevisiHargaBeli.Kode, ABS(CASE WHEN SUM(MRevisiHargaBeliD.KoreksiBL)>=1 THEN SUM(MRevisiHargaBeliD.KoreksiBL) ELSE 0 END), ABS(CASE WHEN SUM(MRevisiHargaBeliD.KoreksiBL)>=1 THEN 0 ELSE SUM(MRevisiHargaBeliD.KoreksiBL) END), MRevisiHargaBeli.Keterangan  " & vbCrLf & _
    '              " FROM MRevisiHargaBeli " & vbCrLf & _
    '              " INNER JOIN MRevisiHargaBeliD ON MRevisiHargaBeli.NoID=MRevisiHargaBeliD.IDRevisiHargaBeli " & vbCrLf & _
    '              " GROUP BY MRevisiHargaBeli.NoID, MRevisiHargaBeli.Tanggal, MRevisiHargaBeli.Kode, MRevisiHargaBeli.Keterangan, MRevisiHargaBeli.IDWilayah, MRevisiHargaBeli.IsPosted, MRevisiHargaBeli.IDSupplier " & vbCrLf & _
    '              " HAVING MRevisiHargaBeli.IDWilayah=1 AND MRevisiHargaBeli.IsPosted=1 AND MRevisiHargaBeli.IDSupplier=" & NullToLong(txtKodeAlamat.EditValue) & " AND  " & vbCrLf & _
    '              " ABS(CASE WHEN SUM(MRevisiHargaBeliD.KoreksiBL)>=1 THEN SUM(MRevisiHargaBeliD.KoreksiBL) ELSE 0 END)-ABS(CASE WHEN SUM(MRevisiHargaBeliD.KoreksiBL)>=1 THEN 0 ELSE SUM(MRevisiHargaBeliD.KoreksiBL) END)<>ISNULL((SELECT SUM(MTTD.Debet-MTTD.Kredit) FROM MTTD INNER JOIN MTT ON MTT.NoID=MTTD.IDTT WHERE MTTD.IDTransaksi=MRevisiHargaBeli.NoID AND MTTD.IDJenisTransaksi=11),0)" & vbCrLf & _
    '              " UNION ALL " & vbCrLf
    '        'Penjualan 
    '        SQL &= "SELECT MJual.NoID, 6, MJual.Tanggal, MJual.Kode, MJual.Total AS Debet, 0 AS Kredit, MJual.Keterangan " & vbCrLf & _
    '               " FROM MJual " & vbCrLf & _
    '               " WHERE MJual.IDWilayah=1 AND MJual.IsPosted=1 AND MJual.IDCustomer=" & NullToLong(txtKodeAlamat.EditValue) & " AND ISNULL(MJual.IsCash,0)=0 AND ISNULL(MJual.IsPOS,0)=0 AND " & vbCrLf & _
    '               " (MJual.Total-0)<>ISNULL((SELECT SUM(MTTD.Debet-MTTD.Kredit) FROM MTTD INNER JOIN MTT ON MTT.NoID=MTTD.IDTT WHERE MTTD.IDTransaksi=MJual.NoID AND MTTD.IDJenisTransaksi=6),0) " & vbCrLf & _
    '               " UNION ALL  " & vbCrLf & _
    '               " SELECT MReturJual.NoID, 7, MReturJual.Tanggal, MReturJual.Kode, 0, MReturJual.Total, MReturJual.Keterangan  " & vbCrLf & _
    '               " FROM MReturJual " & vbCrLf & _
    '               " WHERE MReturJual.IDWilayah=1 AND MReturJual.IsPosted=1 AND MReturJual.IDCustomer=" & NullToLong(txtKodeAlamat.EditValue) & " AND  " & vbCrLf & _
    '               " (0-MReturJual.Total)<>ISNULL((SELECT SUM(MTTD.Debet-MTTD.Kredit) FROM MTTD INNER JOIN MTT ON MTT.NoID=MTTD.IDTT WHERE MTTD.IDTransaksi=MReturJual.NoID AND MTTD.IDJenisTransaksi=7),0) " & vbCrLf & _
    '               " UNION ALL  " & vbCrLf & _
    '               " SELECT MRevisiHargaJual.NoID, 15, MRevisiHargaJual.Tanggal, MRevisiHargaJual.Kode, ABS(CASE WHEN SUM(MRevisiHargaJualD.KoreksiJL)>=1 THEN 0 ELSE SUM(MRevisiHargaJualD.KoreksiJL) END), ABS(CASE WHEN SUM(MRevisiHargaJualD.KoreksiJL)>=1 THEN SUM(MRevisiHargaJualD.KoreksiJL) ELSE 0 END), MRevisiHargaJual.Keterangan  " & vbCrLf & _
    '               " FROM MRevisiHargaJual " & vbCrLf & _
    '               " INNER JOIN MRevisiHargaJualD ON MRevisiHargaJual.NoID=MRevisiHargaJualD.IDRevisiHargaJual " & vbCrLf & _
    '               " GROUP BY MRevisiHargaJual.NoID, MRevisiHargaJual.Tanggal, MRevisiHargaJual.Kode, MRevisiHargaJual.Keterangan, MRevisiHargaJual.IDWilayah, MRevisiHargaJual.IsPosted, MRevisiHargaJual.IDCustomer " & vbCrLf & _
    '               " HAVING MRevisiHargaJual.IDWilayah=1 AND MRevisiHargaJual.IsPosted=1 AND MRevisiHargaJual.IDCustomer=" & NullToLong(txtKodeAlamat.EditValue) & " AND  " & vbCrLf & _
    '               " ABS(CASE WHEN SUM(MRevisiHargaJualD.KoreksiJL)>=1 THEN 0 ELSE SUM(MRevisiHargaJualD.KoreksiJL) END)-ABS(CASE WHEN SUM(MRevisiHargaJualD.KoreksiJL)>=1 THEN SUM(MRevisiHargaJualD.KoreksiJL) ELSE 0 END)<>ISNULL((SELECT SUM(MTTD.Debet-MTTD.Kredit) FROM MTTD INNER JOIN MTT ON MTT.NoID=MTTD.IDTT WHERE MTTD.IDTransaksi=MRevisiHargaJual.NoID AND MTTD.IDJenisTransaksi=15),0) " & vbCrLf
    '        'SQL &= "SELECT MDebetNote.NoID, 19, MDebetNote.Tanggal, MDebetNote.Kode, MDebetNote.Jumlah AS Debet, 0 AS Kredit, MDebetNote.Keterangan " & vbCrLf & _
    '        '       " FROM MDebetNote " & vbCrLf & _
    '        '       " WHERE MDebetNote.IDWilayah=1 AND MDebetNote.IsPosted=1 AND MDebetNote.IDAlamat=" & NullToLong(txtKodeAlamat.EditValue) & " AND " & vbCrLf & _
    '        '       " MDebetNote.Jumlah<>ISNULL((SELECT SUM(MTTD.Debet-MTTD.Kredit) FROM MTTD INNER JOIN MTT ON MTT.NoID=MTTD.IDTT WHERE MTTD.IDTransaksi=MDebetNote.NoID AND MTTD.IDJenisTransaksi=19),0)  " & vbCrLf & _
    '        '       " UNION ALL  " & vbCrLf & _
    '        '       " SELECT MCreditNote.NoID, 20, MCreditNote.Tanggal, MCreditNote.Kode, 0 AS Debet, MCreditNote.Jumlah AS Credit, MCreditNote.Keterangan  " & vbCrLf & _
    '        '       " FROM MCreditNote  " & vbCrLf & _
    '        '       " WHERE MCreditNote.IDWilayah=1 AND MCreditNote.IsPosted=1 AND MCreditNote.IDAlamat=" & NullToLong(txtKodeAlamat.EditValue) & " AND  " & vbCrLf & _
    '        '       " MCreditNote.Jumlah<>ISNULL((SELECT SUM(MTTD.Kredit-MTTD.Kredit) FROM MTTD INNER JOIN MTT ON MTT.NoID=MTTD.IDTT WHERE MTTD.IDTransaksi=MCreditNote.NoID AND MTTD.IDJenisTransaksi=20),0) "
    '        SQL = "SELECT SVR.* FROM (" & SQL & ") AS SVR WHERE SVR.Tanggal>='" & TglDari.DateTime.ToString("yyyy/MM/dd") & "' AND SVR.Tanggal<'" & TglSampai.DateTime.AddDays(1).ToString("yyyy/MM/dd") & "'"
    '        ds = ExecuteDataset("MTT", SQL)
    '        For i As Integer = 0 To ds.Tables("MTT").Rows.Count - 1
    '            SQL = "INSERT INTO [MTTD] ([NoID],[IDTT],[IDTransaksi],[TanggalTransaksi],[KodeReff],[IDJenisTransaksi],[Debet],[Kredit],[Keterangan]) VALUES (" & vbCrLf & _
    '                  GetNewID("MTTD", "NoID") & ", " & NoID & "," & vbCrLf & _
    '                  NullToLong(ds.Tables("MTT").Rows(i).Item("NoID")) & "," & vbCrLf & _
    '                  "'" & NullToDate(ds.Tables("MTT").Rows(i).Item("Tanggal")).ToString("yyyy-MM-dd") & "'," & vbCrLf & _
    '                  "'" & FixApostropi(NullToStr(ds.Tables("MTT").Rows(i).Item("Kode"))) & "'," & vbCrLf & _
    '                  NullToLong(ds.Tables("MTT").Rows(i).Item("IDJenisTransaksi")) & "," & vbCrLf & _
    '                  FixKoma(NullToDbl(ds.Tables("MTT").Rows(i).Item("Debet"))) & "," & vbCrLf & _
    '                  FixKoma(NullToDbl(ds.Tables("MTT").Rows(i).Item("Kredit"))) & "," & vbCrLf & _
    '                  "'" & FixApostropi(NullToStr(ds.Tables("MTT").Rows(i).Item("Keterangan"))) & "'" & vbCrLf & _
    '                  ")"
    '            EksekusiSQL(SQL)
    '        Next
    '    Catch ex As Exception
    '        XtraMessageBox.Show("Info Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
    '    End Try
    'End Sub

    Private Sub SimpleButton3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SimpleButton3.Click
        Dim IDDetil As Long
        Try
            IDDetil = NullToLong(GV1.GetRowCellValue(GV1.FocusedRowHandle, "NoID"))
            SQL = "UPDATE MTTD SET IsTrue=0 WHERE NoID=" & IDDetil
            EksekusiSQL(SQL)
            RefreshDetil()
        Catch ex As Exception
            XtraMessageBox.Show("Info Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
        End Try
    End Sub

    Private Sub SimpleButton4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SimpleButton4.Click
        Dim IDDetil As Long
        Try
            For Each i In GV1.GetSelectedRows
                IDDetil = NullToLong(GV1.GetRowCellValue(i, "NoID"))
                SQL = "UPDATE MTTD SET IsTrue=0 WHERE NoID=" & IDDetil
                EksekusiSQL(SQL)
            Next
            RefreshDetil()
        Catch ex As Exception
            XtraMessageBox.Show("Info Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
        End Try
    End Sub

    Private Sub SimpleButton2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SimpleButton2.Click
        Dim IDDetil As Long
        Try
            For Each i In GridView1.GetSelectedRows
                IDDetil = NullToLong(GridView1.GetRowCellValue(i, "NoID"))
                SQL = "UPDATE MTTD SET IsTrue=1 WHERE NoID=" & IDDetil
                EksekusiSQL(SQL)
            Next
            RefreshDetil()
        Catch ex As Exception
            XtraMessageBox.Show("Info Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
        End Try
    End Sub

    Private Sub SimpleButton1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SimpleButton1.Click
        Dim IDDetil As Long
        Try
            IDDetil = NullToLong(GridView1.GetRowCellValue(GridView1.FocusedRowHandle, "NoID"))
            SQL = "UPDATE MTTD SET IsTrue=1 WHERE NoID=" & IDDetil
            EksekusiSQL(SQL)
            RefreshDetil()
        Catch ex As Exception
            XtraMessageBox.Show("Info Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
        End Try
    End Sub
End Class