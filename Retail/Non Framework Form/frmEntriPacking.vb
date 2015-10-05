Imports System.Data
Imports System.Data.SqlClient
Imports DevExpress.XtraEditors
Imports DevExpress.Utils
Imports DevExpress.XtraGrid.Views.Base

Public Class frmEntriPacking
    Dim SQL As String = ""
    Public Enum pStatus
        Baru = 0
        Edit = 1
    End Enum
    Dim IDadmin As Long = -1
    Public Enum SPK
        IsSPK = 0
        IsPacking = 1
    End Enum

    Public pTipe As pStatus
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
    Dim IDSPKOld As Long = -1
    Dim IsTempInsertBaru As Boolean = False
    Dim IsLoad As Boolean = True

    Private Sub RefreshDataKontak()
        Dim ds As New DataSet
        Try
            SQL = "SELECT NoID, Kode, Nama FROM MAlamat WHERE IsActive=1 AND IsCustomer=1"
            ds = ExecuteDataset("master", SQL)
            txtKodeCustomer.Properties.DataSource = ds.Tables("master")
            If System.IO.File.Exists(folderLayouts & Me.Name & SearchLookUpEdit1View.Name & ".xml") Then
                SearchLookUpEdit1View.RestoreLayoutFromXml(folderLayouts & Me.Name & SearchLookUpEdit1View.Name & ".xml")
            End If

            SQL = "SELECT MGudang.NoID, MGudang.Kode, MGudang.Kode + ' - ' + MGudang.Nama AS Nama, MWilayah.Nama AS Wilayah FROM MGudang LEFT JOIN MWilayah ON MGudang.IDWilayah=MWilayah.NoID WHERE MGudang.IsActive=1 AND MGudang.InTransit=0 AND MGudang.IDWilayah=" & DefIDWilayah
            ds = ExecuteDataset("master", SQL)
            txtGudang.Properties.DataSource = ds.Tables("master")
            txtGudang.Properties.ValueMember = "NoID"
            txtGudang.Properties.DisplayMember = "Nama"
            If System.IO.File.Exists(FolderLayouts & Me.Name & gvGudang.Name & ".xml") Then
                gvGudang.RestoreLayoutFromXml(FolderLayouts & Me.Name & gvGudang.Name & ".xml")
            End If
        Catch ex As Exception
            XtraMessageBox.Show("Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            ds.Dispose()
        End Try
    End Sub
    Dim DefImageList As New ImageList

    Private Sub frmEntriPacking_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        If IsTempInsertBaru Then
            If XtraMessageBox.Show("Yakin ingin keluar?" & vbCrLf & "Ya, akan menghapus transaksi dan detil item.", NamaAplikasi, MessageBoxButtons.YesNo, MessageBoxIcon.Information, MessageBoxDefaultButton.Button2) = Windows.Forms.DialogResult.Yes Then
                EksekusiSQL("DELETE FROM MPacking WHERE NoID=" & NoID)
                EksekusiSQL("DELETE FROM MPackingD WHERE IDPacking=" & NoID)
            Else
                e.Cancel = True
            End If
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
            SetTombol()
            RefreshData()
            SetTombol()
            Me.Width = Ini.BacaIniPath(folderLayouts & Me.Name & ".ini", "Form", "Width", Me.Width)
            Me.Height = Ini.BacaIniPath(folderLayouts & Me.Name & ".ini", "Form", "Height", Me.Height)

            Me.Left = (Screen.PrimaryScreen.WorkingArea.Width - Me.Width) \ 2
            Me.Top = (Screen.PrimaryScreen.WorkingArea.Height - Me.Height) \ 2

            If System.IO.File.Exists(folderLayouts & Me.Name & LayoutControl1.Name & ".xml") Then
                LayoutControl1.RestoreLayoutFromXml(folderLayouts & Me.Name & LayoutControl1.Name & ".xml")
            End If
            If System.IO.File.Exists(folderLayouts & Me.Name & GV1.Name & ".xml") Then
                GV1.RestoreLayoutFromXml(folderLayouts & Me.Name & GV1.Name & ".xml")
            End If
            If System.IO.File.Exists(folderLayouts & Me.Name & SearchLookUpEdit1View.Name & ".xml") Then
                SearchLookUpEdit1View.RestoreLayoutFromXml(folderLayouts & Me.Name & SearchLookUpEdit1View.Name & ".xml")
            End If
            FungsiControl.SetForm(Me)
            IsLoad = False
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
                SQL = "SELECT MPacking.*, MWilayah.Nama AS Wilayah, MUserEntri.Nama AS UserEntri, MUserEdit.Nama AS UserEdit, MUserPosting.Nama AS UserPosting, MAlamat.Nama AS NamaAlamat, MAlamat.Kode as KodeAlamat "
                SQL &= " FROM MPacking LEFT JOIN MAlamat ON MAlamat.NoID=MPacking.IDCustomer "
                SQL &= " LEFT JOIN MUser MUserEntri ON MUserEntri.NoID=MPacking.IDUserEntry "
                SQL &= " LEFT JOIN (MGudang LEFT JOIN MWilayah ON MWilayah.NoID=MGudang.IDWilayah) ON MGudang.NoID=MPacking.IDGudang "
                SQL &= " LEFT JOIN MUser MUserEdit ON MUserEdit.NoID=MPacking.IDUserEdit "
                SQL &= " LEFT JOIN MUser MUserPosting ON MUserPosting.NoID=MPacking.IDUserPosting "
                SQL &= " WHERE MPacking.NoID = " & NoID
                DS = ExecuteDataset("Tabel", SQL)
                If DS.Tables("Tabel").Rows.Count >= 1 Then
                    txtKodeCustomer.EditValue = NullToLong(DS.Tables(0).Rows(0).Item("IDCustomer"))
                    txtGudang.EditValue = NullToLong(DS.Tables(0).Rows(0).Item("IDGudang"))
                    txtWilayah.EditValue = NullToStr(DS.Tables(0).Rows(0).Item("Wilayah"))
                    'txtKodeCustomer.Text = NullTostr(DS.Tables(0).Rows(0).Item("KodeAlamat"))
                    txtNamaCustomer.Text = NullTostr(DS.Tables(0).Rows(0).Item("NamaAlamat"))
                    txtKode.Text = NullTostr(DS.Tables(0).Rows(0).Item("Kode"))
                    KodeLama = txtKode.Text
                    txtKodeReff.Text = NullTostr(DS.Tables(0).Rows(0).Item("KodeReff"))
                    Tgl.DateTime = NullToDate(DS.Tables(0).Rows(0).Item("Tanggal"))
                    tglStok.DateTime = NullToDate(DS.Tables(0).Rows(0).Item("TanggalStock"))
                    tglJatuhTempo.DateTime = NullToDate(DS.Tables(0).Rows(0).Item("JatuhTempo"))
                    tglSJ.DateTime = NullToDate(DS.Tables(0).Rows(0).Item("TanggalSJ"))
                    txtNoSJ.Text = NullTostr(DS.Tables(0).Rows(0).Item("NoSJ"))
                    txtSubtotal.EditValue = NullToDbl(DS.Tables(0).Rows(0).Item("SubTotal"))
                    txtDiscPersen.EditValue = NullToDbl(DS.Tables(0).Rows(0).Item("DiskonNotaProsen"))
                    txtDiscRp.EditValue = NullToDbl(DS.Tables(0).Rows(0).Item("DiskonNotaRp"))
                    txtDiscTotal.EditValue = NullToDbl(DS.Tables(0).Rows(0).Item("DiskonNotaTotal"))
                    txtBiaya.EditValue = NullToDbl(DS.Tables(0).Rows(0).Item("Biaya"))
                    txtTotal.EditValue = NullToDbl(DS.Tables(0).Rows(0).Item("Total"))
                    txtBayar.EditValue = NullToDbl(DS.Tables(0).Rows(0).Item("Bayar"))
                    txtSisa.EditValue = NullToDbl(DS.Tables(0).Rows(0).Item("Sisa"))
                    IsPosted = NullTobool(DS.Tables(0).Rows(0).Item("IsPosted"))
                    IDSPKOld = NullTolong(DS.Tables(0).Rows(0).Item("IDSPK"))
                    txtSPK.EditValue = NullToLong(DS.Tables(0).Rows(0).Item("IDSPK"))
                    RefreshSPK()
                    txtDientriOleh.Text = NullTostr(DS.Tables(0).Rows(0).Item("UserEntri"))
                    txtDieditOleh.Text = NullTostr(DS.Tables(0).Rows(0).Item("UserEdit"))
                    txtDipostingOleh.Text = NullTostr(DS.Tables(0).Rows(0).Item("UserPosting"))
                    tglEntri.DateTime = NullToDate(DS.Tables(0).Rows(0).Item("TglEntri"))
                    tglEdit.DateTime = NullToDate(DS.Tables(0).Rows(0).Item("TglEdit"))
                    tglPosting.DateTime = NullToDate(DS.Tables(0).Rows(0).Item("TglPosting"))
                    If IsPosted Then
                        txtKodeCustomer.Properties.ReadOnly = True
                    End If
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
            strsql = "select MPackingD.*, MBarang.CtnPcs AS IsiCtn, MSPK.Kode AS NoSPK, MWilayah.Nama AS Wilayah, MBarang.Kode as KodeStock,MBarang.Nama as NamaStock,MSatuan.Kode as Satuan,Mgudang.Kode as KodeGudang " & vbCrLf & _
                     " From (MPackingD Inner Join MPacking On MPackingD.IDPacking=MPacking.NoID) " & vbCrLf & _
                     " LEFT JOIN MBarang ON MPackingD.IDBarang=MBarang.NoID " & vbCrLf & _
                     " LEFT JOIN MSatuan ON MPackingD.IDSatuan=MSatuan.NoID " & vbCrLf & _
                     " LEFT JOIN (MSPKD LEFT JOIN MSPK ON MSPK.NoID=MSPKD.IDSPK) ON MPackingD.IDSPKD=MSPKD.NoID " & vbCrLf & _
                     " LEFT JOIN (MGudang LEFT JOIN MWilayah ON MWilayah.NoID=MGudang.IDWilayah) ON MPackingD.IDGudang=MGudang.NoID " & vbCrLf & _
                     " where MPackingD.IDPacking = " & NoID
            ExecuteDBGrid(GC1, strsql, "NoID")
            'SetGridView(GC1)
            For x As Integer = 0 To GV1.Columns.Count - 1
                Select Case GV1.Columns(x).ColumnType.Name.ToLower
                    Case "int32", "int64", "int"
                        GV1.Columns(x).DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
                        GV1.Columns(x).DisplayFormat.FormatString = "n0"
                    Case "decimal", "single", "money", "double"
                        GV1.Columns(x).DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
                        GV1.Columns(x).DisplayFormat.FormatString = "n2"
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
            HitungTotal()
            GV1.ShowFindPanel()
            If pTipe = pStatus.Edit Then
                RefreshSPK()
            End If
        Catch ex As Exception
            XtraMessageBox.Show(ex.Message, ".:: Pesan Kesalahan ::.", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
        Windows.Forms.Cursor.Current = curentcursor
        Application.DoEvents()
    End Sub
    Private Sub HitungTotal()
        Dim temp As Double = 0.0
        Dim ds As New DataSet
        Try
            For i = 0 To GV1.RowCount
                temp = temp + NullToDbl(GV1.GetRowCellValue(i, "Jumlah"))
            Next
            txtSubtotal.EditValue = temp
            txtDiscTotal.EditValue = (NullToDbl(txtDiscPersen.EditValue) * NullToDbl(txtSubtotal.EditValue) / 100) '+ txtDiscRp.EditValue
            txtTotal.EditValue = txtSubtotal.EditValue - txtDiscTotal.EditValue + txtBiaya.EditValue
            txtSisa.EditValue = txtTotal.EditValue - txtBayar.EditValue

            txtTotalCTN.EditValue = NullToDbl(EksekusiSQlSkalarNew("SELECT SUM(Ctn) FROM MPackingD WHERE IDPacking=" & NoID))
            txtJumlahItem.EditValue = NullToDbl(EksekusiSQlSkalarNew("SELECT COUNT(IDBarang) FROM MPackingD WHERE IDPacking=" & NoID & " GROUP BY IDBarang, IDPacking"))
            txtPackingTerakhir.EditValue = NullToStr(EksekusiSQlSkalarNew("SELECT MAX(NoPacking) FROM MPackingD WHERE IDPacking=" & NoID & " GROUP BY NoPacking Order By NoPacking DESC"))

            txtKodeReff.Text = ""
            SQL = "SELECT MSPK.Kode FROM MPackingD LEFT JOIN (MSPKD INNER JOIN MSPK ON MSPK.NoID=MSPKD.IDSPK) ON MSPKD.NoID=MPackingD.IDSPKD WHERE MPackingD.IDPacking=" & NoID & " GROUP BY MSPK.Kode ORDER BY MSPK.Kode"
            ds = ExecuteDataset("MSPK", SQL)
            For i As Integer = 0 To ds.Tables("MSPK").Rows.Count - 1
                txtKodeReff.Text = txtKodeReff.Text & IIf(i = 0, "", ", ") & NullToStr(ds.Tables("MSPK").Rows(i).Item("Kode"))
            Next
        Catch ex As Exception
            XtraMessageBox.Show("Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            ds.Dispose()
        End Try
    End Sub
    Private Sub IsiDefault()
        Tgl.DateTime = TanggalSystem
        tglJatuhTempo.DateTime = DateAdd(DateInterval.Month, 1, Tgl.DateTime)
        tglSJ.DateTime = TanggalSystem
        tglStok.DateTime = TanggalSystem
        SetTombol()
        txtGudang.EditValue = DefIDGudang
        RubahGudang()
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
        Else
            cmdBAru.Enabled = True
            cmdEdit.Enabled = True
            cmdDelete.Enabled = True
        End If
    End Sub
    Private Sub cmdSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSave.Click
        Dim PesanSalah As String = ""
        Dim ds As New DataSet
        If Not IsPosted Then
            If IsValidasi() Then
                If Simpan(PesanSalah) = True Then
                    If pTipe = pStatus.Baru Then
                        KodeLama = txtKode.Text
                        pTipe = pStatus.Edit
                        SetTombol()
                        txtSPK.Focus()
                        'txtKodeCustomer.Properties.ReadOnly = True

                        EksekusiSQL("DELETE FROM MPackingD WHERE IDPacking=" & NoID)
                        IDSPKOld = NullToLong(txtSPK.EditValue)
                        SQL = "SELECT MSPKD.*, MBarang.CtnPcs, MSatuanBase.NoID AS IDSatuanBase, MSatuanBase.Konversi AS KonversiBase, (MSPKD.Qty*MSPKD.Konversi)-IsNull((SELECT SUM(MPackingD.Qty*MPackingD.Konversi) FROM MPackingD INNER JOIN MPacking ON MPacking.NoID=MPackingD.IDPacking WHERE MPackingD.IDSPKD=MSPKD.NoID),0) AS SisaQty " & _
                              " FROM MSPKD " & _
                              " LEFT JOIN MBarang ON MBarang.NoID=MSPKD.IDBarang " & _
                              " LEFT JOIN (MSatuan LEFT JOIN MSatuan MSatuanBase ON MSatuanBase.NoID=MSatuan.IDBase) ON MSatuan.NoID=MSPKD.IDSatuan " & _
                              " WHERE MSPKD.IDGudang=" & NullToLong(txtGudang.EditValue) & " AND MSPKD.IDSPK=" & NullToLong(txtSPK.EditValue) & _
                              " AND (MSPKD.Qty*MSPKD.Konversi)-IsNull((SELECT SUM(MPackingD.Qty*MPackingD.Konversi) FROM MPackingD INNER JOIN MPacking ON MPacking.NoID=MPackingD.IDPacking WHERE MPackingD.IDSPKD=MSPKD.NoID),0)>0"
                        ds = ExecuteDataset("MSPKD", SQL)
                        With Ds.Tables("MSPKD")
                            If .Rows.Count >= 1 Then
                                For i As Integer = 0 To .Rows.Count - 1
                                    'txtQty.EditValue / IsiKarton * txtKonversi.EditValue
                                    If NullToDbl(.Rows(i).Item("Konversi")) <= NullToDbl(.Rows(i).Item("SisaQty")) Then
                                        SQL = "INSERT INTO [MPackingD] ([NoID],[Tgl],[Jam],[IDPacking],[IDBarang],[IDSatuan],[IDGudang],[Ctn],[Qty],[Harga],[DiscPersen1],[DiscPersen2],[DiscPersen3],[Disc1],[Disc2],[Disc3],[Jumlah],[IDSPKD],[Catatan],[NoUrut],[HargaPcs],[QtyPcs],[Konversi])" & vbCrLf & _
                                        " VALUES (" & NullToLong(GetNewID("MPackingD", "NoID")) & " ,getdate() ,getdate() ," & NoID & " ," & NullToLong(.Rows(i).Item("IDBarang")) & " ," & NullToLong(.Rows(i).Item("IDSatuan")) & " ," & NullToLong(txtGudang.EditValue) & " ," & FixKoma(IIf(NullToDbl(.Rows(i).Item("CtnPcs")) = 0, 0, NullToDbl(.Rows(i).Item("SisaQty")) / NullToDbl(.Rows(i).Item("CtnPcs")))) & " ," & FixKoma(NullToDbl(.Rows(i).Item("SisaQty")) / IIf(NullToDbl(.Rows(i).Item("Konversi")) = 0, 1, NullToDbl(.Rows(i).Item("Konversi")))) & " ," & FixKoma(NullToDbl(.Rows(i).Item("Harga"))) & " ," & FixKoma(NullToDbl(.Rows(i).Item("DiscPersen1"))) & vbCrLf & _
                                        " ," & FixKoma(NullToDbl(.Rows(i).Item("DiscPersen2"))) & " ," & FixKoma(NullToDbl(.Rows(i).Item("DiscPersen3"))) & " ," & FixKoma(NullToDbl(.Rows(i).Item("Disc1"))) & " ," & FixKoma(NullToDbl(.Rows(i).Item("Disc2"))) & " ," & FixKoma(NullToDbl(.Rows(i).Item("Disc3"))) & " ," & FixKoma(NullToDbl(.Rows(i).Item("Jumlah")) / NullToDbl(.Rows(i).Item("Qty")) * NullToDbl(.Rows(i).Item("SisaQty")) / NullToDbl(.Rows(i).Item("Konversi"))) & " , " & NullToLong(.Rows(i).Item("NoID")) & ",'" & FixApostropi(NullToStr(.Rows(i).Item("Catatan"))) & "' ," & vbCrLf & _
                                        NullToLong(GetNewID("MPackingD", "NoUrut", " WHERE MPackingD.IDPacking=" & NoID)) & " ," & FixKoma(NullToDbl(.Rows(i).Item("HargaPcs"))) & " ," & FixKoma(NullToDbl(.Rows(i).Item("QtyPcs"))) & " ," & FixKoma(NullToDbl(.Rows(i).Item("Konversi"))) & ")"
                                    Else
                                        SQL = "INSERT INTO [MPackingD] ([NoID],[Tgl],[Jam],[IDPacking],[IDBarang],[IDSatuan],[IDGudang],[Ctn],[Qty],[Harga],[DiscPersen1],[DiscPersen2],[DiscPersen3],[Disc1],[Disc2],[Disc3],[Jumlah],[IDSPKD],[Catatan],[NoUrut],[HargaPcs],[QtyPcs],[Konversi])" & vbCrLf & _
                                        " VALUES (" & NullToLong(GetNewID("MPackingD", "NoID")) & " ,getdate() ,getdate() ," & NoID & " ," & NullToLong(.Rows(i).Item("IDBarang")) & " ," & NullToLong(.Rows(i).Item("IDSatuanBase")) & " ," & NullToLong(txtGudang.EditValue) & " ," & FixKoma(IIf(NullToDbl(.Rows(i).Item("CtnPcs")) = 0, 0, NullToDbl(.Rows(i).Item("SisaQty")) / NullToDbl(.Rows(i).Item("CtnPcs")))) & " ," & FixKoma(NullToDbl(.Rows(i).Item("SisaQty")) / IIf(NullToDbl(.Rows(i).Item("KonversiBase")) = 0, 1, NullToDbl(.Rows(i).Item("KonversiBase")))) & " ," & FixKoma(NullToDbl(.Rows(i).Item("Harga")) / IIf(NullToDbl(.Rows(i).Item("KonversiBase")) = 0, 1, NullToDbl(.Rows(i).Item("KonversiBase")))) & " ," & FixKoma(NullToDbl(.Rows(i).Item("DiscPersen1"))) & vbCrLf & _
                                        " ," & FixKoma(NullToDbl(.Rows(i).Item("DiscPersen2"))) & " ," & FixKoma(NullToDbl(.Rows(i).Item("DiscPersen3"))) & " ," & FixKoma(NullToDbl(.Rows(i).Item("Disc1"))) & " ," & FixKoma(NullToDbl(.Rows(i).Item("Disc2"))) & " ," & FixKoma(NullToDbl(.Rows(i).Item("Disc3"))) & " ," & FixKoma(NullToDbl(.Rows(i).Item("Jumlah")) / NullToDbl(.Rows(i).Item("Qty")) * NullToDbl(.Rows(i).Item("SisaQty")) / NullToDbl(.Rows(i).Item("KonversiBase"))) & " , " & NullToLong(.Rows(i).Item("NoID")) & ",'" & FixApostropi(NullToStr(.Rows(i).Item("Catatan"))) & "' ," & vbCrLf & _
                                        NullToLong(GetNewID("MPackingD", "NoUrut", " WHERE MPackingD.IDPacking=" & NoID)) & " ," & FixKoma(NullToDbl(.Rows(i).Item("HargaPcs"))) & " ," & FixKoma(NullToDbl(.Rows(i).Item("QtyPcs"))) & " ," & FixKoma(NullToDbl(.Rows(i).Item("KonversiBase"))) & ")"
                                    End If
                                    EksekusiSQL(SQL)
                                Next
                            End If
                        End With
                        RefreshDetil()
                        IsTempInsertBaru = True
                    Else
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
                            frmEntri.MdiParent = Me
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
        If txtGudang.Text = "" Then
            XtraMessageBox.Show("Gudang masih kosong.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
            txtGudang.Focus()
            Return False
            Exit Function
        End If
        If txtKodeCustomer.Text = "" Then
            XtraMessageBox.Show("Customer masih kosong.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
            txtKodeCustomer.Focus()
            Return False
            Exit Function
        End If
        If Tgl.Text = "" Then
            XtraMessageBox.Show("Tanggal masih kosong.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
            Tgl.Focus()
            Return False
            Exit Function
        End If
        'If pTipe = pStatus.Edit AndAlso txtSPK.Text = "" Then
        '    If XtraMessageBox.Show("Item SPK masih kosong." & vbCrLf & "Ingin melanjutkan penyimpanan?", NamaAplikasi, MessageBoxButtons.YesNo, MessageBoxIcon.Information) = Windows.Forms.DialogResult.No Then
        '        txtSPK.Focus()
        '        Return False
        '        Exit Function
        '    End If
        'End If
        If CekKodeValid(txtKode.Text, KodeLama, "MPacking", "Kode", IIf(pTipe = pStatus.Edit, True, False)) Then
            XtraMessageBox.Show("Kode sudah dipakai.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
            txtKode.Focus()
            Return False
            Exit Function
        End If
        If pTipe = pStatus.Edit AndAlso GV1.RowCount <= 0 Then
            XtraMessageBox.Show("Item detil masih kosong." & vbCrLf & "Isi item detil atau tutup bila ingin membatailkan.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
            Return False
            Exit Function
        End If
        If pTipe = pStatus.Edit Then
            RefreshDetil()
            For i As Integer = 0 To GV1.RowCount - 1
                If GV1.GetRowCellValue(i, "NoPacking").ToString = "" Then
                    XtraMessageBox.Show("No Paking di item " & GV1.GetRowCellValue(i, "NamaStock").ToString & " masih kosong.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Stop)
                    GV1.ClearSelection()
                    GV1.FocusedRowHandle = i
                    GV1.SelectRow(GV1.FocusedRowHandle)
                    cmdEdit.Focus()
                    Return False
                    Exit Function
                End If
            Next
        End If
        Return True
    End Function
    Dim KodeLama As String = ""
    Public Function Simpan(ByRef PesanSalah As String) As Boolean
        Dim Sukses As Boolean = False
        HitungTotal()
        Try
            If pTipe = pStatus.Baru Then
                NoID = GetNewID("MPacking")
                SQL = "INSERT INTO MPacking (IDGudang,IDWilayah,IDSPK,NoID,Kode,KodeReff,Tanggal,TanggalStock,JatuhTempo,"
                SQL &= " IDCustomer,TanggalSJ,NoSJ,SubTotal,DiskonNotaProsen,DiskonNotaRp,DiskonNotaTotal,"
                SQL &= "  Biaya, Total, Bayar, Sisa,IDUserEntry,IDUserEdit,IDUserPosting,TglEntri,IDAdmin) VALUES (" & vbCrLf
                SQL &= NullToLong(txtGudang.EditValue) & ","
                SQL &= DefIDWilayah & ","
                SQL &= NullTolong(txtSPK.EditValue) & ","
                SQL &= NoID & ","
                SQL &= "'" & FixApostropi(txtKode.Text) & "',"
                SQL &= "'" & FixApostropi(txtKodeReff.Text) & "',"
                SQL &= "'" & Tgl.DateTime.ToString("yyyy-MM-dd HH:mm") & "',"
                SQL &= "'" & tglStok.DateTime.ToString("yyyy-MM-dd HH:mm") & "',"
                SQL &= "'" & tglJatuhTempo.DateTime.ToString("yyyy-MM-dd HH:mm") & "',"
                SQL &= txtKodeCustomer.EditValue & ","
                SQL &= "'" & tglSJ.DateTime.ToString("yyyy-MM-dd HH:mm") & "',"
                SQL &= "'" & FixApostropi(txtNoSJ.Text) & "',"
                SQL &= FixKoma(txtSubtotal.EditValue) & ","
                SQL &= FixKoma(txtDiscPersen.EditValue) & ","
                SQL &= FixKoma(txtDiscRp.EditValue) & ","
                SQL &= FixKoma(txtDiscTotal.EditValue) & ","
                SQL &= FixKoma(txtBiaya.EditValue) & ","
                SQL &= FixKoma(txtTotal.EditValue) & ","
                SQL &= FixKoma(txtBayar.EditValue) & ","
                SQL &= FixKoma(txtSisa.EditValue) & ","
                SQL &= IDUserAktif & ","
                SQL &= "-1,"
                SQL &= "-1,"
                SQL &= "'" & TanggalSystem.ToString("yyyy-MM-dd HH:mm") & "'," & IDadmin & ")"
                EksekusiSQL(SQL)
            Else
                SQL = "UPDATE MPacking SET "
                If Not Tgl.Properties.ReadOnly Or Not txtKode.Properties.ReadOnly Then
                    SQL &= "IDAdmin=" & IDadmin & ","
                End If
                SQL &= "IDGudang=" & NullToLong(txtGudang.EditValue) & ","
                SQL &= "IDSPK=" & NullTolong(txtSPK.EditValue) & ","
                SQL &= "Kode='" & FixApostropi(txtKode.Text) & "',"
                SQL &= "KodeReff='" & FixApostropi(txtKodeReff.Text) & "',"
                SQL &= "Tanggal='" & Tgl.DateTime.ToString("yyyy-MM-dd HH:mm") & "',"
                SQL &= "TanggalStock='" & tglStok.DateTime.ToString("yyyy-MM-dd HH:mm") & "',"
                SQL &= "JatuhTempo='" & tglJatuhTempo.DateTime.ToString("yyyy-MM-dd HH:mm") & "',"
                SQL &= "IDCustomer=" & txtKodeCustomer.EditValue & ","
                SQL &= "TanggalSJ='" & tglSJ.DateTime.ToString("yyyy-MM-dd HH:mm") & "',"
                SQL &= "NoSJ='" & FixApostropi(txtNoSJ.Text) & "',"
                SQL &= "SubTotal=" & FixKoma(txtSubtotal.EditValue) & ","
                SQL &= "DiskonNotaProsen=" & FixKoma(txtDiscPersen.EditValue) & ","
                SQL &= "DiskonNotaRp=" & FixKoma(txtDiscRp.EditValue) & ","
                SQL &= "DiskonNotaTotal=" & FixKoma(txtDiscTotal.EditValue) & ","
                SQL &= "Biaya=" & FixKoma(txtBiaya.EditValue) & ","
                SQL &= "Total=" & FixKoma(txtTotal.EditValue) & ","
                SQL &= "Bayar=" & FixKoma(txtBayar.EditValue) & ","
                SQL &= "Sisa=" & FixKoma(txtSisa.EditValue) & ","
                SQL &= "TglEdit='" & TanggalSystem.ToString("yyyy-MM-dd HH:mm") & "',"
                SQL &= "IDUserEdit=" & IDUserAktif
                SQL &= " WHERE NoID=" & NoID
                EksekusiSQL(SQL)
            End If
            Sukses = True
            If Sukses Then
                SQL = "UPDATE MPackingD SET IDGudang=" & NullToLong(txtGudang.EditValue) & " WHERE IDPacking=" & NoID
                EksekusiSQL(SQL)
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
                Ini.TulisIniPath(folderLayouts & Me.Name & ".ini", "Form", "Width", Me.Width)
                Ini.TulisIniPath(folderLayouts & Me.Name & ".ini", "Form", "Height", Me.Height)
                Me.Left = (Screen.PrimaryScreen.WorkingArea.Width - Me.Width) \ 2
                Me.Top = (Screen.PrimaryScreen.WorkingArea.Height - Me.Height) \ 2

                LayoutControl1.SaveLayoutToXml(folderLayouts & Me.Name & LayoutControl1.Name & ".xml")
                GV1.SaveLayoutToXml(folderLayouts & Me.Name & GV1.Name & ".xml")
                SearchLookUpEdit1View.SaveLayoutToXml(folderLayouts & Me.Name & SearchLookUpEdit1View.Name & ".xml")
                gvSPK.SaveLayoutToXml(folderLayouts & Me.Name & gvSPK.Name & ".xml")
            End If
        Catch ex As Exception
            XtraMessageBox.Show("Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            xOtorisasi.Dispose()
        End Try
    End Sub

    Private Sub cmdRefresh_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdRefresh.Click
        RefreshDetil()
    End Sub

    Private Sub cmdDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdDelete.Click
        Dim view As ColumnView = GC1.FocusedView
        Try
            Dim row As System.Data.DataRow = view.GetDataRow(GV1.FocusedRowHandle)
            Dim dc As Integer = GV1.FocusedRowHandle
            Dim IDDetil As Long = NullTolong(row("NoID"))
            If XtraMessageBox.Show("Item ini mau dihapus?", NamaAplikasi, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) = Windows.Forms.DialogResult.Yes Then
                EksekusiSQL("Delete From MPackingD where NoID=" & IDDetil.ToString)
                RefreshDetil()
            End If
            txtBarang.Focus()
            If pTipe = pStatus.Edit Then
                RefreshSPK()
            End If
        Catch ex As Exception
            XtraMessageBox.Show("Untuk mengedit pilih item satuan yang akan diedit terlebih dahulu lalu tekan tombol edit", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub txtKodeCustomer_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtKodeCustomer.EditValueChanged
        Dim TglAdd As Long = 0
        Try
            txtNamaCustomer.Text = NullTostr(EksekusiSQlSkalarNew("SELECT Nama FROM MAlamat WHERE NoID=" & NullTolong(txtKodeCustomer.EditValue)))
            txtAlamatCustomer.Text = NullTostr(EksekusiSQlSkalarNew("SELECT Alamat FROM MAlamat WHERE NoID=" & NullTolong(txtKodeCustomer.EditValue)))
            TglAdd = NullTolong(EksekusiSQlSkalarNew("SELECT JatuhTempoCustomer FROM MAlamat WHERE NoID=" & NullTolong(txtKodeCustomer.EditValue)))
            If pTipe = pStatus.Edit Then
                RefreshSPK()
            End If
            If TglAdd = 0 Then
                tglJatuhTempo.DateTime = DateAdd(DateInterval.Month, 2, Tgl.DateTime)
            Else
                tglJatuhTempo.DateTime = DateAdd(DateInterval.Day, TglAdd, Tgl.DateTime)
            End If
            RefreshSPK()
        Catch ex As Exception

        End Try
    End Sub
    Private Sub RefreshSPK()
        Dim ds As New DataSet
        Try
            'SQL = " SELECT NoID, NoSPK, Tanggal, SUM(Total) AS Total FROM (" & vbCrLf & _
            '      " SELECT MSPK.NoID, MSPK.Kode AS NoSPK, MSPK.Tanggal, (MSPKD.Qty*MSPKD.Konversi)-IsNull((SELECT SUM(A.Qty*A.Konversi) FROM MPackingD A INNER JOIN MPacking B ON B.NoID=A.IDPacking WHERE A.IDSPKD=MSPKD.NoID),0) AS Total" & vbCrLf & _
            '      " FROM MSPKD INNER JOIN MSPK ON MSPK.NoID=MSPKD.IDSPK " & vbCrLf & _
            '      " WHERE MSPKD.IDGudang=" & NullToLong(txtGudang.EditValue) & " AND (MSPKD.Qty*MSPKD.Konversi)-IsNull((SELECT SUM(A.Qty*A.Konversi) FROM MPackingD A INNER JOIN MPacking B ON B.NoID=A.IDPacking WHERE A.IDSPKD=MSPKD.NoID),0)>0 AND (MSPK.IsSelesai=0 OR MSPK.IsSelesai Is NULL) AND MSPK.IsPosted=1 AND MSPK.IDWilayah=" & DefIDWilayah & " AND MSPK.IDCustomer=" & NullToLong(txtKodeCustomer.EditValue) & " ) X" & vbCrLf & _
            '      " GROUP BY NoID, NoSPK, Tanggal"
            SQL = "SELECT MSPK.NoID, MSPK.Kode AS NoSPK, MSPK.Tanggal " & vbCrLf & _
                  " FROM (MSPKD " & vbCrLf & _
                  " INNER JOIN MSPK ON MSPK.NoID=MSPKD.IDSPK) " & vbCrLf & _
                  " WHERE ((MSPKD.Qty*MSPKD.Konversi)-IsNull((SELECT SUM(MPackingD.Qty*MPackingD.Konversi) FROM MPackingD INNER JOIN MPacking ON MPacking.NoID=MPackingD.IDPacking WHERE MPackingD.IDSPKD=MSPKD.NoID),0))>0 AND IsNull(MSPK.IsSelesai,0)=0 AND IsNull(MSPK.IsPosted,0)=1 AND MSPK.IDWilayah=" & DefIDWilayah & " AND MSPKD.IDGudang=" & NullToLong(txtGudang.EditValue) & " AND MSPK.IDCustomer=" & NullToLong(txtKodeCustomer.EditValue) & vbCrLf & _
                  " GROUP BY MSPK.NoID, MSPK.Kode, MSPK.Tanggal, MSPK.IsSelesai, MSPK.IsPosted, MSPK.IDWilayah, MSPK.IDCustomer"
            ds = ExecuteDataset("MSPK", SQL)
            txtSPK.Properties.DataSource = ds.Tables("MSPK")
            txtSPK.Properties.ValueMember = "NoID"
            txtSPK.Properties.DisplayMember = "NoSPK"
        Catch ex As Exception
            XtraMessageBox.Show("Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            ds.Dispose()
        End Try
    End Sub
    Private Sub Tgl_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Tgl.EditValueChanged
        Dim TglAdd As Long = 0
        Try
            'txtNamaCustomer.Text = NullTostr(SearchLookUpEdit1View.GetRowCellValue(SearchLookUpEdit1View.FocusedRowHandle, "Nama"))
            'txtAlamatCustomer.Text = NullTostr(EksekusiSQlSkalarNew("SELECT Alamat FROM MAlamat WHERE NoID=" & NullTolong(txtKodeCustomer.EditValue)))
            TglAdd = NullTolong(EksekusiSQlSkalarNew("SELECT JatuhTempoCustomer FROM MAlamat WHERE NoID=" & NullTolong(txtKodeCustomer.EditValue)))
            If TglAdd = 0 Then
                tglJatuhTempo.DateTime = DateAdd(DateInterval.Month, 2, Tgl.DateTime)
            Else
                tglJatuhTempo.DateTime = DateAdd(DateInterval.Day, TglAdd, Tgl.DateTime)
            End If
            If pTipe = pStatus.Baru Then
                txtKode.Text = clsKode.MintaKodeBaru("SPC", "MPacking", Tgl.DateTime, DefIDWilayah, 5)
            End If
        Catch ex As Exception

        End Try
    End Sub

    Private Sub txtSubtotal_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtSubtotal.LostFocus
        HitungTotal()
    End Sub

    Private Sub txtDiscPersen_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtDiscPersen.LostFocus
        HitungTotal()
    End Sub

    Private Sub txtDiscRp_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtDiscRp.LostFocus
        HitungTotal()
    End Sub

    Private Sub txtDiscTotal_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtDiscTotal.LostFocus
        HitungTotal()
    End Sub

    Private Sub txtBiaya_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtBiaya.LostFocus
        HitungTotal()
    End Sub

    Private Sub txtTotal_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtTotal.LostFocus
        HitungTotal()
    End Sub

    Private Sub txtBayar_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtBayar.LostFocus
        HitungTotal()
    End Sub

    Private Sub txtSisa_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtSisa.LostFocus
        HitungTotal()
    End Sub

    Private Sub txtBarang_ButtonClick(ByVal sender As Object, ByVal e As DevExpress.XtraEditors.Controls.ButtonPressedEventArgs) Handles txtBarang.ButtonClick
        If pTipe = pStatus.Baru Then Exit Sub
        Select Case e.Button.Index
            Case 0
                InsertIntoDetil()
                txtBarang.Text = ""
                txtBarang.Focus()
            Case 1
                txtBarang.Text = ""
        End Select
    End Sub
    Private Function CariBarang(ByRef IDBarang As Long, ByRef NamaBarang As String, ByRef KodeBarang As String) As Boolean
        Dim x As Boolean = False
        Dim SQL As String = ""
        Dim oDS As New DataSet
        Try
            SQL = "SELECT NoID,Kode,Nama FROM MBarang WHERE IsActive=1 AND (ISNULL(KODE,'')<>'') AND (UPPER(Kode) = '" & NamaBarang.Replace("'", "''").ToUpper & "' OR UPPER(NAMA) = '" & NamaBarang.Replace("'", "''").ToUpper & "') ORDER BY Kode"
            oDS = ExecuteDataset("Tbl", SQL)
            If oDS.Tables("Tbl").Rows.Count >= 1 Then
                NamaBarang = NullToStr(oDS.Tables(0).Rows(0).Item("Nama"))
                KodeBarang = NullToStr(oDS.Tables(0).Rows(0).Item("Kode"))
                IDBarang = NullToLong(oDS.Tables(0).Rows(0).Item("NoID"))
                x = True
            Else
                x = False
            End If
        Catch ex As Exception
            x = False
        End Try
        Return x
    End Function
    Private Sub InsertIntoDetil()
        Dim SQL As String = ""
        Dim NamaBarang As String = txtBarang.Text
        Dim KodeBarang As String = ""
        Dim IDBarang As Long = -1
        Dim IDDetil As Long = -1
        Dim frmEntri As New frmEntriPackingD
        Dim Konversi As Double = 0.0

        Try
            If txtKodeCustomer.Text = "" Then XtraMessageBox.Show("Customer masih kosong.", NamaBarang) : txtKodeCustomer.Focus() : Exit Sub
            If CariBarang(IDBarang, NamaBarang, KodeBarang) Then
                If XtraMessageBox.Show("Ingin menambah Item Packing dengan Kode Stock " & KodeBarang & " Nama Stock " & NamaBarang, "Fast Entri Says", MessageBoxButtons.YesNo) = Windows.Forms.DialogResult.Yes Then
                    IDDetil = GetNewID("MPackingD", "NoID")
                    Konversi = NullToDbl(EksekusiSQlSkalarNew("SELECT Konversi FROM MBarangD WHERE IsJual=1 AND IDBarang=" & IDBarang & " AND IDSatuan=" & DefIDSatuan))
                    If Konversi <> 0 Then
                        SQL = "INSERT INTO MPackingD (NoID,IDBarang,IDSatuan,Konversi,IDPacking,NoUrut,Tgl,Jam) VALUES " & vbCrLf
                        SQL &= "(" & IDDetil & "," & IDBarang & "," & DefIDSatuan & "," & FixKoma(Konversi) & "," & NoID & "," & GetNewID("MPackingD", "NoUrut", " WHERE IDPacking=" & NoID) & ",GetDate(),GetDate())"
                    Else
                        SQL = "INSERT INTO MPackingD (NoID,IDBarang,IDPacking,NoUrut,Tgl,Jam) VALUES " & vbCrLf
                        SQL &= "(" & IDDetil & "," & IDBarang & "," & NoID & "," & GetNewID("MPackingD", "NoUrut", " WHERE IDPacking=" & NoID) & ",GetDate(),GetDate())"
                    End If
                    EksekusiSQL(SQL)
                    frmEntri.IsNew = False
                    frmEntri.NoID = IDDetil
                    frmEntri.IDPacking = NoID
                    frmEntri.IDCustomer = NullToLong(txtKodeCustomer.EditValue)
                    frmEntri.IDSPK = NullToLong(txtSPK.EditValue)
                    frmEntri.IsFastEntri = True
                    frmEntri.FormPemanggil = Me
                    frmEntri.Show()
                    frmEntri.Focus()
                    'If frmEntri.ShowDialog(Me)= Windows.Forms.DialogResult.OK Then
                    '    RefreshDetil()
                    '    GV1.ClearSelection()
                    '    GV1.FocusedRowHandle = GV1.LocateByDisplayText(0, GV1.Columns("NoID"), IDDetil.ToString("#,##0"))
                    '    GV1.SelectRow(GV1.FocusedRowHandle)
                    'Else
                    '    SQL = "DELETE FROM MPackingD WHERE NoID=" & IDDetil
                    '    EksekusiSQL(SQL)
                    '    RefreshDetil()
                    '    GV1.ClearSelection()
                    '    GV1.FocusedRowHandle = GV1.LocateByDisplayText(0, GV1.Columns("NoID"), IDDetil.ToString("#,##0"))
                    '    GV1.SelectRow(GV1.FocusedRowHandle)
                    'End If
                End If
            End If
        Catch ex As Exception
            XtraMessageBox.Show("Kesalahan : " & ex.Message, "Fast Entri Says", MessageBoxButtons.OK)
            'frmEntri.Dispose()
        End Try
    End Sub

    'Private Sub InsertIntoDetilSO(ByVal IDSO As Long)
    '    Dim SQL As String = ""
    '    Dim ds As New DataSet
    '    Try
    '        If pPacking = SPK.IsPacking Then XtraMessageBox.Show("Packing tidak boleh menambah baru.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error) : Exit Sub
    '        SQL = "SELECT MSOD.*, (MSOD.Qty*MSOD.Konversi)-IsNull((SELECT SUM(A.Qty*A.Konversi) FROM MSPKD A WHERE A.IDSOD=MSOD.NoID),0) AS SisaQty "
    '        SQL &= " FROM MSOD WHERE MSOD.IDSO=" & IDSO & " AND (MSOD.Qty*MSOD.Konversi)-IsNull((SELECT SUM(A.Qty*A.Konversi) FROM MSPKD A WHERE A.IDSOD=MSOD.NoID),0)>0 "
    '        ds = ExecuteDataset("MSO", SQL)
    '        With ds.Tables("MSO")
    '            If .Rows.Count >= 1 Then
    '                For i As Integer = 0 To .Rows.Count - 1
    '                    SQL = "INSERT INTO MSPKD ([NoID],[Tgl],[Jam],[IDSPK],[IDBarang],[IDSatuan],[IDGudang],[Ctn],[Qty],[Harga],[DiscPersen1],[DiscPersen2],[DiscPersen3],[Disc1],[Disc2],[Disc3],[Jumlah]," & _
    '                         "[IDSOD],[Catatan],[NoUrut],[HargaPcs],[QtyPcs],[Konversi],[IsPacking],[NoPacking],[QtyKarton]) VALUES (" & _
    '                          NullTolong(GetNewID("MSPKD", "NoID")) & ", getdate(), getdate(), " & NoID & ", " & NullTolong(.Rows(i).Item("IDBarang")) & ", " & NullTolong(.Rows(i).Item("IDSatuan")) & ", " & _
    '                          NullTolong(.Rows(i).Item("IDGudang")) & ", " & FixKoma(NullToDbl(.Rows(i).Item("Ctn"))) & ", " & FixKoma(NullToDbl(.Rows(i).Item("SisaQty"))) & ", " & FixKoma(NullToDbl(.Rows(i).Item("Harga"))) & ", " & _
    '                          FixKoma(NullToDbl(.Rows(i).Item("DiscPersen1"))) & ", " & FixKoma(NullToDbl(.Rows(i).Item("DiscPersen2"))) & ", " & FixKoma(NullToDbl(.Rows(i).Item("DiscPersen3"))) & ", " & _
    '                          FixKoma(NullToDbl(.Rows(i).Item("Disc1"))) & ", " & FixKoma(NullToDbl(.Rows(i).Item("Disc2"))) & ", " & FixKoma(NullToDbl(.Rows(i).Item("Disc3"))) & ", " & _
    '                          FixKoma(NullToDbl(.Rows(i).Item("Jumlah"))) & ", " & NullTolong(.Rows(i).Item("NoID")) & ", '" & FixApostropi(NullTostr(.Rows(i).Item("Catatan"))) & "', " & NullTolong(GetNewID("MSPKD", "NoUrut", " WHERE IDSPK=" & NoID)) & ", " & _
    '                          FixKoma(NullToDbl(.Rows(i).Item("HargaPcs"))) & ", " & FixKoma(NullToDbl(.Rows(i).Item("QtyPcs"))) & ", " & FixKoma(NullToDbl(.Rows(i).Item("Konversi"))) & ", 0,'',0)"
    '                Next
    '                EksekusiSQL(SQL)
    '            End If
    '        End With
    '        RefreshDetil()
    '    Catch ex As Exception
    '        XtraMessageBox.Show("Kesalahan : " & ex.Message, "Fast Entri Says", MessageBoxButtons.OK)
    '    Finally
    '        ds.Dispose()
    '    End Try
    'End Sub

    Private Sub cmdBAru_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdBAru.Click
        Dim frmEntri As New frmEntriPackingD
        Try
            Dim IDDetil As Long = -1
            frmEntri.NoID = IDDetil
            frmEntri.IsNew = True
            frmEntri.IDSPK = NoID
            If txtKodeCustomer.Text <> "" Then
                frmEntri.IDCustomer = NullTolong(txtKodeCustomer.EditValue)
                frmEntri.NoID = IDDetil
                frmEntri.IsNew = True
                frmEntri.IDPacking = NoID
                frmEntri.IDSPK = NullToLong(txtSPK.EditValue)
                frmEntri.IDGudang = NullToLong(txtGudang.EditValue)
                frmEntri.FormPemanggil = Me
                frmEntri.Show()
                frmEntri.Focus()
                'If frmEntri.ShowDialog(Me)= Windows.Forms.DialogResult.OK Then
                '    RefreshDetil()
                'End If
                'txtBarang.Focus()
            Else
                XtraMessageBox.Show("Isi dulu Customer, lalu click baru.", NamaAplikasi, MessageBoxButtons.OK)
            End If
        Catch ex As Exception
            XtraMessageBox.Show("Untuk membuat baru tekan tombol baru", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            'frmEntri.Dispose()
        End Try
    End Sub

    Private Sub cmdEdit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdEdit.Click
        Dim view As ColumnView = GC1.FocusedView
        Dim frmEntri As New frmEntriPackingD
        Try
            Dim row As System.Data.DataRow = view.GetDataRow(GV1.FocusedRowHandle)
            Dim dc As Integer = GV1.FocusedRowHandle
            Dim IDDetil As Long = NullTolong(row("NoID"))
            If txtKodeCustomer.Text <> "" Then
                frmEntri.IDCustomer = NullTolong(txtKodeCustomer.EditValue)
                frmEntri.IsNew = False
                frmEntri.NoID = IDDetil
                frmEntri.IDPacking = NoID
                frmEntri.IDSPK = NullToLong(txtSPK.EditValue)
                frmEntri.IDGudang = NullToLong(txtGudang.EditValue)
                frmEntri.FormPemanggil = Me
                frmEntri.Show()
                frmEntri.Focus()
                'If frmEntri.ShowDialog(Me)= Windows.Forms.DialogResult.OK Then
                '    RefreshDetil()
                'End If
                'txtBarang.Focus()
            Else
                XtraMessageBox.Show("Isi dulu Customer, lalu click Edit.", NamaAplikasi, MessageBoxButtons.OK)
            End If
        Catch ex As Exception
            XtraMessageBox.Show("Untuk mengedit pilih item satuan yang akan diedit terlebih dahulu lalu tekan tombol edit", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            'frmEntri.Dispose()
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

    Private Sub txtNamaCustomer_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtNamaCustomer.EditValueChanged

    End Sub

    Private Sub Tgl_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Tgl.KeyDown
        If e.KeyCode = Keys.F7 Then
            Dim x As New frmOtorisasiAdmin
            Try
                If Tgl.Properties.ReadOnly Then
                    If x.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
                        IDadmin = x.IDUserAdmin
                        Tgl.Properties.ReadOnly = False
                        tglStok.Properties.ReadOnly = False
                        tglJatuhTempo.Properties.ReadOnly = False
                    End If
                End If
            Catch ex As Exception
                XtraMessageBox.Show("Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End If
    End Sub

    Private Sub txtSPK_ButtonClick(ByVal sender As Object, ByVal e As DevExpress.XtraEditors.Controls.ButtonPressedEventArgs) Handles txtSPK.ButtonClick
        If e.Button.Index = 1 Then
            AmbilDataSPK()
        End If
    End Sub
    Private Sub AmbilDataSPK()
        Dim Ds As New DataSet
        Try
            If pTipe = pStatus.Edit AndAlso IsPosted = False Then
                'If IDSPKOld <> NullToLong(txtSPK.EditValue) AndAlso XtraMessageBox.Show("Bersihkan item packing dan ambil data dari SPK?", NamaAplikasi, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) = Windows.Forms.DialogResult.Yes Then
                '    EksekusiSQL("DELETE FROM MPackingD WHERE IDPacking=" & NoID)
                'ElseIf IDSPKOld = NullToLong(txtSPK.EditValue) Then

                'Else
                '    txtSPK.EditValue = IDSPKOld
                '    Exit Sub
                'End If
                IDSPKOld = NullToLong(txtSPK.EditValue)
                SQL = "SELECT MSPKD.*, MBarang.CtnPcs, MSatuanBase.NoID AS IDSatuanBase, MSatuanBase.Konversi AS KonversiBase, (MSPKD.Qty*MSPKD.Konversi)-IsNull((SELECT SUM(MPackingD.Qty*MPackingD.Konversi) FROM MPackingD INNER JOIN MPacking ON MPacking.NoID=MPackingD.IDPacking WHERE MPackingD.IDSPKD=MSPKD.NoID),0) AS SisaQty " & _
                      " FROM MSPKD " & _
                      " LEFT JOIN MBarang ON MBarang.NoID=MSPKD.IDBarang " & _
                      " LEFT JOIN (MSatuan LEFT JOIN MSatuan MSatuanBase ON MSatuanBase.NoID=MSatuan.IDBase) ON MSatuan.NoID=MSPKD.IDSatuan " & _
                      " WHERE MSPKD.IDGudang=" & NullToLong(txtGudang.EditValue) & " AND MSPKD.IDSPK=" & NullToLong(txtSPK.EditValue) & _
                      " AND (MSPKD.Qty*MSPKD.Konversi)-IsNull((SELECT SUM(MPackingD.Qty*MPackingD.Konversi) FROM MPackingD INNER JOIN MPacking ON MPacking.NoID=MPackingD.IDPacking WHERE MPackingD.IDSPKD=MSPKD.NoID),0)>0"
                Ds = ExecuteDataset("MSPKD", SQL)
                With Ds.Tables("MSPKD")
                    If .Rows.Count >= 1 Then
                        For i As Integer = 0 To .Rows.Count - 1
                            'txtQty.EditValue / IsiKarton * txtKonversi.EditValue
                            If NullToDbl(.Rows(i).Item("Konversi")) <= NullToDbl(.Rows(i).Item("SisaQty")) Then
                                SQL = "INSERT INTO [MPackingD] ([NoID],[Tgl],[Jam],[IDPacking],[IDBarang],[IDSatuan],[IDGudang],[Ctn],[Qty],[Harga],[DiscPersen1],[DiscPersen2],[DiscPersen3],[Disc1],[Disc2],[Disc3],[Jumlah],[IDSPKD],[Catatan],[NoUrut],[HargaPcs],[QtyPcs],[Konversi])" & vbCrLf & _
                                " VALUES (" & NullToLong(GetNewID("MPackingD", "NoID")) & " ,getdate() ,getdate() ," & NoID & " ," & NullToLong(.Rows(i).Item("IDBarang")) & " ," & NullToLong(.Rows(i).Item("IDSatuan")) & " ," & NullToLong(txtGudang.EditValue) & " ," & FixKoma(IIf(NullToDbl(.Rows(i).Item("CtnPcs")) = 0, 0, NullToDbl(.Rows(i).Item("SisaQty")) / NullToDbl(.Rows(i).Item("CtnPcs")))) & " ," & FixKoma(NullToDbl(.Rows(i).Item("SisaQty")) / IIf(NullToDbl(.Rows(i).Item("Konversi")) = 0, 1, NullToDbl(.Rows(i).Item("Konversi")))) & " ," & FixKoma(NullToDbl(.Rows(i).Item("Harga"))) & " ," & FixKoma(NullToDbl(.Rows(i).Item("DiscPersen1"))) & vbCrLf & _
                                " ," & FixKoma(NullToDbl(.Rows(i).Item("DiscPersen2"))) & " ," & FixKoma(NullToDbl(.Rows(i).Item("DiscPersen3"))) & " ," & FixKoma(NullToDbl(.Rows(i).Item("Disc1"))) & " ," & FixKoma(NullToDbl(.Rows(i).Item("Disc2"))) & " ," & FixKoma(NullToDbl(.Rows(i).Item("Disc3"))) & " ," & FixKoma(NullToDbl(.Rows(i).Item("Jumlah")) / NullToDbl(.Rows(i).Item("Qty")) * NullToDbl(.Rows(i).Item("SisaQty")) / NullToDbl(.Rows(i).Item("Konversi"))) & " , " & NullToLong(.Rows(i).Item("NoID")) & ",'" & FixApostropi(NullToStr(.Rows(i).Item("Catatan"))) & "' ," & vbCrLf & _
                                NullToLong(GetNewID("MPackingD", "NoUrut", " WHERE MPackingD.IDPacking=" & NoID)) & " ," & FixKoma(NullToDbl(.Rows(i).Item("HargaPcs"))) & " ," & FixKoma(NullToDbl(.Rows(i).Item("QtyPcs"))) & " ," & FixKoma(NullToDbl(.Rows(i).Item("Konversi"))) & ")"
                            Else
                                SQL = "INSERT INTO [MPackingD] ([NoID],[Tgl],[Jam],[IDPacking],[IDBarang],[IDSatuan],[IDGudang],[Ctn],[Qty],[Harga],[DiscPersen1],[DiscPersen2],[DiscPersen3],[Disc1],[Disc2],[Disc3],[Jumlah],[IDSPKD],[Catatan],[NoUrut],[HargaPcs],[QtyPcs],[Konversi])" & vbCrLf & _
                                " VALUES (" & NullToLong(GetNewID("MPackingD", "NoID")) & " ,getdate() ,getdate() ," & NoID & " ," & NullToLong(.Rows(i).Item("IDBarang")) & " ," & NullToLong(.Rows(i).Item("IDSatuanBase")) & " ," & NullToLong(txtGudang.EditValue) & " ," & FixKoma(IIf(NullToDbl(.Rows(i).Item("CtnPcs")) = 0, 0, NullToDbl(.Rows(i).Item("SisaQty")) / NullToDbl(.Rows(i).Item("CtnPcs")))) & " ," & FixKoma(NullToDbl(.Rows(i).Item("SisaQty")) / IIf(NullToDbl(.Rows(i).Item("KonversiBase")) = 0, 1, NullToDbl(.Rows(i).Item("KonversiBase")))) & " ," & FixKoma(NullToDbl(.Rows(i).Item("Harga")) / IIf(NullToDbl(.Rows(i).Item("KonversiBase")) = 0, 1, NullToDbl(.Rows(i).Item("KonversiBase")))) & " ," & FixKoma(NullToDbl(.Rows(i).Item("DiscPersen1"))) & vbCrLf & _
                                " ," & FixKoma(NullToDbl(.Rows(i).Item("DiscPersen2"))) & " ," & FixKoma(NullToDbl(.Rows(i).Item("DiscPersen3"))) & " ," & FixKoma(NullToDbl(.Rows(i).Item("Disc1"))) & " ," & FixKoma(NullToDbl(.Rows(i).Item("Disc2"))) & " ," & FixKoma(NullToDbl(.Rows(i).Item("Disc3"))) & " ," & FixKoma(NullToDbl(.Rows(i).Item("Jumlah")) / NullToDbl(.Rows(i).Item("Qty")) * NullToDbl(.Rows(i).Item("SisaQty")) / NullToDbl(.Rows(i).Item("KonversiBase"))) & " , " & NullToLong(.Rows(i).Item("NoID")) & ",'" & FixApostropi(NullToStr(.Rows(i).Item("Catatan"))) & "' ," & vbCrLf & _
                                NullToLong(GetNewID("MPackingD", "NoUrut", " WHERE MPackingD.IDPacking=" & NoID)) & " ," & FixKoma(NullToDbl(.Rows(i).Item("HargaPcs"))) & " ," & FixKoma(NullToDbl(.Rows(i).Item("QtyPcs"))) & " ," & FixKoma(NullToDbl(.Rows(i).Item("KonversiBase"))) & ")"
                            End If
                            EksekusiSQL(SQL)
                        Next
                    End If
                End With
                RefreshDetil()
            Else
                txtSPK.EditValue = IDSPKOld
            End If
        Catch ex As Exception
            XtraMessageBox.Show("Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            'RefreshLookUpSTB()
            Ds.Dispose()
        End Try
    End Sub
    Public Sub LoadDataSPK()
        Dim Ds As New DataSet
        Try
            If pTipe = pStatus.Edit AndAlso IsPosted = False Then
                SQL = "SELECT MSPKD.*, MBarang.CtnPcs, MSatuanBase.NoID AS IDSatuanBase, MSatuanBase.Konversi AS KonversiBase, (MSPKD.Qty*MSPKD.Konversi)-IsNull((SELECT SUM(MPackingD.Qty*MPackingD.Konversi) FROM MPackingD INNER JOIN MPacking ON MPacking.NoID=MPackingD.IDPacking WHERE MPackingD.IDSPKD=MSPKD.NoID),0) AS SisaQty " & _
                      " FROM MSPKD " & _
                      " LEFT JOIN MBarang ON MBarang.NoID=MSPKD.IDBarang " & _
                      " LEFT JOIN (MSatuan LEFT JOIN MSatuan MSatuanBase ON MSatuanBase.NoID=MSatuan.IDBase) ON MSatuan.NoID=MSPKD.IDSatuan " & _
                      " WHERE MSPKD.IDGudang=" & NullToLong(txtGudang.EditValue) & " AND MSPKD.IDSPK IN (SELECT MSPKD.IDSPK FROM MPackingD INNER JOIN MSPKD ON MSPKD.NoID=MPackingD.IDSPKD WHERE MPackingD.IDPacking=" & NoID & ") " & _
                      " AND (MSPKD.Qty*MSPKD.Konversi)-IsNull((SELECT SUM(MPackingD.Qty*MPackingD.Konversi) FROM MPackingD INNER JOIN MPacking ON MPacking.NoID=MPackingD.IDPacking WHERE MPackingD.IDSPKD=MSPKD.NoID),0)>0"
                Ds = ExecuteDataset("MSPKD", SQL)
                With Ds.Tables("MSPKD")
                    If .Rows.Count >= 1 Then
                        For i As Integer = 0 To .Rows.Count - 1
                            'txtQty.EditValue / IsiKarton * txtKonversi.EditValue
                            If NullToDbl(.Rows(i).Item("Konversi")) <= NullToDbl(.Rows(i).Item("SisaQty")) Then
                                SQL = "INSERT INTO [MPackingD] ([NoID],[Tgl],[Jam],[IDPacking],[IDBarang],[IDSatuan],[IDGudang],[Ctn],[Qty],[Harga],[DiscPersen1],[DiscPersen2],[DiscPersen3],[Disc1],[Disc2],[Disc3],[Jumlah],[IDSPKD],[Catatan],[NoUrut],[HargaPcs],[QtyPcs],[Konversi])" & vbCrLf & _
                                " VALUES (" & NullToLong(GetNewID("MPackingD", "NoID")) & " ,getdate() ,getdate() ," & NoID & " ," & NullToLong(.Rows(i).Item("IDBarang")) & " ," & NullToLong(.Rows(i).Item("IDSatuan")) & " ," & NullToLong(txtGudang.EditValue) & " ," & FixKoma(IIf(NullToDbl(.Rows(i).Item("CtnPcs")) = 0, 0, NullToDbl(.Rows(i).Item("SisaQty")) / NullToDbl(.Rows(i).Item("CtnPcs")))) & " ," & FixKoma(NullToDbl(.Rows(i).Item("SisaQty")) / IIf(NullToDbl(.Rows(i).Item("Konversi")) = 0, 1, NullToDbl(.Rows(i).Item("Konversi")))) & " ," & FixKoma(NullToDbl(.Rows(i).Item("Harga"))) & " ," & FixKoma(NullToDbl(.Rows(i).Item("DiscPersen1"))) & vbCrLf & _
                                " ," & FixKoma(NullToDbl(.Rows(i).Item("DiscPersen2"))) & " ," & FixKoma(NullToDbl(.Rows(i).Item("DiscPersen3"))) & " ," & FixKoma(NullToDbl(.Rows(i).Item("Disc1"))) & " ," & FixKoma(NullToDbl(.Rows(i).Item("Disc2"))) & " ," & FixKoma(NullToDbl(.Rows(i).Item("Disc3"))) & " ," & FixKoma(NullToDbl(.Rows(i).Item("Jumlah")) / NullToDbl(.Rows(i).Item("Qty")) * NullToDbl(.Rows(i).Item("SisaQty")) / NullToDbl(.Rows(i).Item("Konversi"))) & " , " & NullToLong(.Rows(i).Item("NoID")) & ",'" & FixApostropi(NullToStr(.Rows(i).Item("Catatan"))) & "' ," & vbCrLf & _
                                NullToLong(GetNewID("MPackingD", "NoUrut", " WHERE MPackingD.IDPacking=" & NoID)) & " ," & FixKoma(NullToDbl(.Rows(i).Item("HargaPcs"))) & " ," & FixKoma(NullToDbl(.Rows(i).Item("QtyPcs"))) & " ," & FixKoma(NullToDbl(.Rows(i).Item("Konversi"))) & ")"
                            Else
                                SQL = "INSERT INTO [MPackingD] ([NoID],[Tgl],[Jam],[IDPacking],[IDBarang],[IDSatuan],[IDGudang],[Ctn],[Qty],[Harga],[DiscPersen1],[DiscPersen2],[DiscPersen3],[Disc1],[Disc2],[Disc3],[Jumlah],[IDSPKD],[Catatan],[NoUrut],[HargaPcs],[QtyPcs],[Konversi])" & vbCrLf & _
                                " VALUES (" & NullToLong(GetNewID("MPackingD", "NoID")) & " ,getdate() ,getdate() ," & NoID & " ," & NullToLong(.Rows(i).Item("IDBarang")) & " ," & NullToLong(.Rows(i).Item("IDSatuanBase")) & " ," & NullToLong(txtGudang.EditValue) & " ," & FixKoma(IIf(NullToDbl(.Rows(i).Item("CtnPcs")) = 0, 0, NullToDbl(.Rows(i).Item("SisaQty")) / NullToDbl(.Rows(i).Item("CtnPcs")))) & " ," & FixKoma(NullToDbl(.Rows(i).Item("SisaQty")) / IIf(NullToDbl(.Rows(i).Item("KonversiBase")) = 0, 1, NullToDbl(.Rows(i).Item("KonversiBase")))) & " ," & FixKoma(NullToDbl(.Rows(i).Item("Harga")) / IIf(NullToDbl(.Rows(i).Item("KonversiBase")) = 0, 1, NullToDbl(.Rows(i).Item("KonversiBase")))) & " ," & FixKoma(NullToDbl(.Rows(i).Item("DiscPersen1"))) & vbCrLf & _
                                " ," & FixKoma(NullToDbl(.Rows(i).Item("DiscPersen2"))) & " ," & FixKoma(NullToDbl(.Rows(i).Item("DiscPersen3"))) & " ," & FixKoma(NullToDbl(.Rows(i).Item("Disc1"))) & " ," & FixKoma(NullToDbl(.Rows(i).Item("Disc2"))) & " ," & FixKoma(NullToDbl(.Rows(i).Item("Disc3"))) & " ," & FixKoma(NullToDbl(.Rows(i).Item("Jumlah")) / NullToDbl(.Rows(i).Item("Qty")) * NullToDbl(.Rows(i).Item("SisaQty")) / NullToDbl(.Rows(i).Item("KonversiBase"))) & " , " & NullToLong(.Rows(i).Item("NoID")) & ",'" & FixApostropi(NullToStr(.Rows(i).Item("Catatan"))) & "' ," & vbCrLf & _
                                NullToLong(GetNewID("MPackingD", "NoUrut", " WHERE MPackingD.IDPacking=" & NoID)) & " ," & FixKoma(NullToDbl(.Rows(i).Item("HargaPcs"))) & " ," & FixKoma(NullToDbl(.Rows(i).Item("QtyPcs"))) & " ," & FixKoma(NullToDbl(.Rows(i).Item("KonversiBase"))) & ")"
                            End If
                            EksekusiSQL(SQL)
                        Next
                    End If
                End With
                'RefreshDetil()
            End If
        Catch ex As Exception
            XtraMessageBox.Show("Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            'RefreshLookUpSTB()
            Ds.Dispose()
        End Try
    End Sub

    Private Sub gvSO_DataSourceChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvSPK.DataSourceChanged
        If System.IO.File.Exists(folderLayouts & Me.Name & gvSPK.Name & ".xml") Then
            gvSPK.RestoreLayoutFromXml(folderLayouts & Me.Name & gvSPK.Name & ".xml")
        End If
        With gvSPK
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

    Private Sub txtBarang_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtBarang.KeyDown
        If pTipe = pStatus.Baru Then Exit Sub
        Select Case e.KeyCode
            Case Keys.Enter
                InsertIntoDetil()
                txtBarang.Text = ""
                'txtBarang.Focus()
            Case Keys.Escape
                txtBarang.Text = ""
        End Select
    End Sub

    Private Sub txtTglKirim_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtTglKirim.EditValueChanged

    End Sub

    Private Sub txtKode_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtKode.EditValueChanged

    End Sub

    Private Sub txtKode_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtKode.KeyDown
        If e.KeyCode = Keys.F7 Then
            Dim x As New frmOtorisasiAdmin
            Try
                If txtKode.Properties.ReadOnly Then
                    If x.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
                        IDadmin = x.IDUserAdmin
                        txtKode.Properties.ReadOnly = False
                    End If
                End If
            Catch ex As Exception
                XtraMessageBox.Show("Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End If
    End Sub

    Private Sub txtSPK_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtSPK.KeyDown
        If e.KeyCode = Keys.Enter Then
            AmbilDataSPK()
        End If
    End Sub
    Private Sub txtGudang_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtGudang.EditValueChanged
        RubahGudang()
    End Sub
    Private Sub RubahGudang()
        RefreshSPK()
        txtWilayah.Text = NullToStr(EksekusiSQlSkalarNew("SELECT MWilayah.Nama FROM MGudang LEFT JOIN MWilayah ON MWilayah.NOID=MGudang.IDWilayah WHERE MGudang.NoID=" & NullToLong(txtGudang.EditValue)))
    End Sub

    Private Sub txtGudang_EditValueChanging(ByVal sender As Object, ByVal e As DevExpress.XtraEditors.Controls.ChangingEventArgs) Handles txtGudang.EditValueChanging
        If Not IsLoad AndAlso (IsTempInsertBaru Or (pTipe = pStatus.Edit AndAlso Not IsPosted)) Then
            If XtraMessageBox.Show("Bila gudang dirubah item detil akan dibersihkan?" & vbCrLf & "Ingin melanjutkan?", NamaAplikasi, MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) = Windows.Forms.DialogResult.Yes Then
                EksekusiSQL("DELETE FROM MPackingD WHERE IDPacking=" & NoID)
                RefreshDetil()
            Else
                e.Cancel = True
            End If
        End If
    End Sub

    Private Sub txtGudang_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtGudang.KeyDown
        If e.KeyCode = Keys.F7 Then
            Dim x As New frmOtorisasiAdmin
            Try
                If txtGudang.Properties.ReadOnly Then
                    If x.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
                        IDadmin = x.IDUserAdmin
                        txtGudang.Properties.ReadOnly = False
                    End If
                End If
            Catch ex As Exception
                XtraMessageBox.Show("Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End If
    End Sub

    Private Sub txtSPK_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtSPK.EditValueChanged

    End Sub
End Class