Imports System.Data
Imports System.Data.SqlClient
Imports DevExpress.XtraEditors
Imports DevExpress.Utils

Public Class frmEntriUser
    Public IsNew As Boolean = True
    Public IsPosted As Boolean = False
    Public NoID As Long = -1
    Public IDPO As Long = -1
    Public IDSupplier As Long = -1

    Dim oDS As New DataSet
    Dim SQL As String = ""
    Dim DefImageList As ImageList
    Dim KodeLama As String = ""

    'For Back Action
    Public FormNameDaftar As String = ""
    Public TableNameDaftar As String = ""
    Public TextDaftar As String = ""
    Public FormEntriDaftar As String = ""
    Public TableMasterDaftar As String = ""

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

            SQL = "SELECT MGudang.NoID, MGudang.Kode, MGudang.Nama, MWilayah.Nama AS Wilayah FROM MGudang LEFT JOIN MWilayah ON MWilayah.NoID=MGudang.IDWilayah WHERE MGudang.IsActive=1 "
            ds = ExecuteDataset("MGudang", SQL)
            txtGudang.Properties.DataSource = ds.Tables("MGudang")
            txtGudang.Properties.ValueMember = "NoID"
            txtGudang.Properties.DisplayMember = "Nama"

            txtGudangSupplier.Properties.DataSource = ds.Tables("MGudang")
            txtGudangSupplier.Properties.ValueMember = "NoID"
            txtGudangSupplier.Properties.DisplayMember = "Nama"

            txtGudangCustomer.Properties.DataSource = ds.Tables("MGudang")
            txtGudangCustomer.Properties.ValueMember = "NoID"
            txtGudangCustomer.Properties.DisplayMember = "Nama"

            SQL = "SELECT NoID, Kode, Nama, Alamat FROM MAlamat WHERE IsActive=1 AND IsPegawai=1 "
            ds = ExecuteDataset("MAlamat", SQL)
            txtPegawai.Properties.DataSource = ds.Tables("MAlamat")
            txtPegawai.Properties.ValueMember = "NoID"
            txtPegawai.Properties.DisplayMember = "Nama"
            If System.IO.File.Exists(folderLayouts & Me.Name & gvPegawai.Name & ".xml") Then
                gvPegawai.RestoreLayoutFromXml(folderLayouts & Me.Name & gvPegawai.Name & ".xml")
            End If
            With gvPegawai
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

            SQL = "SELECT MSatuan.NoID, MSatuan.Kode, MSatuan.Nama FROM MSatuan WHERE MSatuan.IsActive=1 "
            ds = ExecuteDataset("MSatuan", SQL)
            txtSatuan.Properties.DataSource = ds.Tables("MSatuan")
            txtSatuan.Properties.ValueMember = "NoID"
            txtSatuan.Properties.DisplayMember = "Kode"
            If System.IO.File.Exists(folderLayouts & Me.Name & gvSatuan.Name & ".xml") Then
                gvSatuan.RestoreLayoutFromXml(folderLayouts & Me.Name & gvSatuan.Name & ".xml")
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
                                .Columns(x).DisplayFormat.FormatString = "HH:mm"
                            Else
                                .Columns(x).DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime
                                .Columns(x).DisplayFormat.FormatString = "dd-MM-yyyy"
                            End If
                    End Select
                Next
            End With

            SQL = "SELECT NoID, Kode, Nama, Alamat FROM MAlamat WHERE IsActive=1 AND IsCustomer=1 "
            ds = ExecuteDataset("MAlamat", SQL)
            txtCustomer.Properties.DataSource = ds.Tables("MAlamat")
            txtCustomer.Properties.ValueMember = "NoID"
            txtCustomer.Properties.DisplayMember = "Nama"
            If System.IO.File.Exists(folderLayouts & Me.Name & gvCustomer.Name & ".xml") Then
                gvCustomer.RestoreLayoutFromXml(folderLayouts & Me.Name & gvCustomer.Name & ".xml")
            End If
            With gvCustomer
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

            SQL = "SELECT NoID, Kode, Nama, Alamat FROM MAlamat WHERE IsActive=1 AND IsSupplier=1 "
            ds = ExecuteDataset("MAlamat", SQL)
            txtSupplier.Properties.DataSource = ds.Tables("MAlamat")
            txtSupplier.Properties.ValueMember = "NoID"
            txtSupplier.Properties.DisplayMember = "Nama"
            If System.IO.File.Exists(folderLayouts & Me.Name & gvSupplier.Name & ".xml") Then
                gvSupplier.RestoreLayoutFromXml(folderLayouts & Me.Name & gvSupplier.Name & ".xml")
            End If
            With gvSupplier
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

            'Dash Board
            SQL = "SELECT MMenu.NoID, MMenu.Caption " & vbCrLf & _
                  " FROM MMenu " & _
                  " WHERE MMenu.isactive = 1 And IsNull(MMenu.IsBarSubItem, 0) = 0 " & vbCrLf & _
                  " AND MMenu.noid NOT IN (SELECT A.idparent FROM MMenu A) " & vbCrLf & _
                  " AND MMenu.noid IN (SELECT MUserD.IDMenu FROM MUser INNER JOIN MUserD ON MUser.NoID=MUserD.IDUser WHERE MUserD.[Enable]=1 AND MUserD.[Visible]=1 AND MUser.NoID=" & NoID & ")"
            ds = ExecuteDataset("MDashBoard", SQL)
            txtDashboard1.Properties.DataSource = ds.Tables("MDashBoard")
            txtDashboard1.Properties.ValueMember = "NoID"
            txtDashboard1.Properties.DisplayMember = "Caption"

            txtDashboard2.Properties.DataSource = ds.Tables("MDashBoard")
            txtDashboard2.Properties.ValueMember = "NoID"
            txtDashboard2.Properties.DisplayMember = "Caption"

            txtDashboard3.Properties.DataSource = ds.Tables("MDashBoard")
            txtDashboard3.Properties.ValueMember = "NoID"
            txtDashboard3.Properties.DisplayMember = "Caption"

            txtDashboard4.Properties.DataSource = ds.Tables("MDashBoard")
            txtDashboard4.Properties.ValueMember = "NoID"
            txtDashboard4.Properties.DisplayMember = "Caption"

        Catch ex As Exception
            XtraMessageBox.Show("Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            ds.Dispose()
        End Try
    End Sub
    Private Sub LoadData()
        Try
            SQL = "SELECT MUser.* FROM MUser WHERE MUser.NoID= " & NoID
            oDS = New DataSet()
            oDS = ExecuteDataset("MUser", SQL)
            If oDS.Tables(0).Rows.Count >= 1 Then
                With oDS.Tables("MUser").Rows(0)
                    txtKode.Text = NullTostr(.Item("Kode"))
                    txtPwd.Text = DecryptText(NullTostr(.Item("Pwd")), "vpoint")
                    txtNama.Text = NullTostr(.Item("Nama"))
                    ckSupervisor.Checked = NullTobool(.Item("IsSupervisor"))
                    ckKasir.Checked = NullTobool(.Item("IsKasir"))
                    ckAutoPosting.Checked = NullTobool(.Item("IsAutoPosting"))
                    txtGudang.EditValue = NullTolong(.Item("IDGudangDefault"))
                    txtPegawai.EditValue = NullTolong(.Item("IDAlamat"))
                    txtSupplier.EditValue = NullTolong(.Item("IDSupplier"))
                    txtCustomer.EditValue = NullToLong(.Item("IDPelanggan"))
                    txtGudangSupplier.EditValue = NullToLong(.Item("IDGudangPenerimaanSupplier"))
                    txtGudangCustomer.EditValue = NullToLong(.Item("IDGudangPenerimaanCustomer"))
                    txtSatuan.EditValue = NullTolong(.Item("IDSatuan"))
                    ckAktif.Checked = NullTobool(.Item("IsActive"))
                    txtCatatan.Text = NullTostr(.Item("Keterangan"))
                    ckEditLayouts.Checked = NullToBool(.Item("IsEditLayout"))
                    ckAccPusat.Checked = NullToBool(.Item("IsAccMutasi"))
                    ckReminderHutang.Checked = NullToBool(.Item("IsReminderHutang"))
                    ckPengawasKasir.Checked = NullToBool(.Item("IsPengawasKasir"))
                    ckPengawasUtama.Checked = NullToBool(.Item("IsPengawasUtamaKasir"))
                    cbTipe.SelectedIndex = NullTolInt(.Item("Tipe"))

                    ckMnVPOS.Checked = NullToBool(.Item("IsMnVPOS"))
                    ckMnSetting.Checked = NullToBool(.Item("IsMnSetting"))
                    ckMnDatabase.Checked = NullToBool(.Item("IsMnDatabase"))
                    ckMnDeveloper.Checked = NullToBool(.Item("IsMnDeveloper"))

                    txtDashboard1.EditValue = NullToLong(.Item("IDDashBoard1"))
                    txtDashboard2.EditValue = NullToLong(.Item("IDDashBoard2"))
                    txtDashboard3.EditValue = NullToLong(.Item("IDDashBoard3"))
                    txtDashboard4.EditValue = NullToLong(.Item("IDDashBoard4"))

                    KodeLama = txtKode.Text
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
    Public Sub RefreshDetil()
        Dim strsql As String
        Dim curentcursor As Cursor = Windows.Forms.Cursor.Current
        Dim repChekEdit As New DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit
        Windows.Forms.Cursor.Current = Cursors.WaitCursor
        Try
            strsql = "SELECT MUserDAksesGudang.*, MGudang.Kode AS KodeGudang, MGudang.Nama AS Gudang, MWilayah.Nama AS Wilayah " & vbCrLf & _
                     " FROM MUserDAksesGudang " & vbCrLf & _
                     " LEFT JOIN (MGudang LEFT JOIN MWilayah ON MWIlayah.NoID=MGudang.IDWilayah) ON MGudang.NoID=MUserDAksesGudang.IDGudang" & vbCrLf & _
                     " WHERE MUserDAksesGudang.IDUser = " & NoID
            ExecuteDBGrid(GC1, strsql, "NoID")
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
            GV1.HideFindPanel()

            strsql = "SELECT MUserD.NoID, MHeader.Caption AS Tab, MMenu.Caption, MUserD.[Enable], MUserD.Visible, MMenu.NoUrut " & vbCrLf & _
                     " FROM MUserD " & vbCrLf & _
                     " LEFT JOIN (MMenu LEFT JOIN MMenu MHeader ON MHeader.NoID=MMenu.IDParent) ON MMenu.NoID=MUserD.IDMenu " & vbCrLf & _
                     " WHERE MMenu.IsActive=1 AND MUserD.IDUser = " & NoID & _
                     " ORDER BY MMenu.NoUrut "
            ExecuteDBGrid(GridControl1, strsql, "NoID")
            For x As Integer = 0 To GridView1.Columns.Count - 1
                Select Case GridView1.Columns(x).ColumnType.Name.ToLower
                    Case "int32", "int64", "int"
                        GridView1.Columns(x).DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
                        GridView1.Columns(x).DisplayFormat.FormatString = "n0"
                    Case "decimal", "single", "money", "double"
                        GridView1.Columns(x).DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
                        GridView1.Columns(x).DisplayFormat.FormatString = "n2"
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
            GridView1.HideFindPanel()
            GridView1.OptionsCustomization.AllowSort = False
        Catch ex As Exception
            XtraMessageBox.Show(ex.Message, ".:: Pesan Kesalahan ::.", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
        Windows.Forms.Cursor.Current = curentcursor
        Application.DoEvents()
    End Sub
    Private Sub cmdSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSave.Click
        If IsValidasi() Then
            If Simpan() Then
                'KODE DIBAWAH UNTUK MEMBUKA FORM HANYA 1
                Dim frmEntri As frmDaftarUser = Nothing
                Dim F As Object
                For Each F In MdiParent.MdiChildren
                    If TypeOf F Is frmDaftarUser Then
                        frmEntri = F
                        If frmEntri.FormName = FormNameDaftar Then
                            Exit For
                        Else
                            frmEntri = Nothing
                        End If
                    End If
                Next
                If frmEntri Is Nothing Then
                    frmEntri = New frmDaftarUser
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

                'DialogResult = Windows.Forms.DialogResult.OK
                Close()
                Dispose()
            End If
        End If
    End Sub
    Private Function Simpan() As Boolean
        Try
            If IsNew Then
                SQL = "INSERT INTO MUser ([NoID],[Kode],[Pwd],[Nama],[IsSupervisor],[IsKasir],[IsAutoPosting],[IDGudangDefault],[IDAlamat],[IDSupplier],[IDPelanggan],[IDBarang],[IDSatuan],[IsActive],[Keterangan],[IsEditLayout],[Tipe],[IDGudangPenerimaanSupplier],[IDGudangPenerimaanCustomer],[IsAccMutasi],[IsPengawasKasir],[IsPengawasUtamaKasir],[IsMnVPOS],[IsMnSetting],[IsMnDatabase],[IsMnDeveloper],[IsReminderHutang],[IDDashBoard1],[IDDashBoard2],[IDDashBoard3],[IDDashBoard4]) VALUES ("
                SQL &= NullTolong(GetNewID("MUser", "NoID")) & ","
                SQL &= "'" & FixApostropi(txtKode.Text) & "',"
                SQL &= "'" & FixApostropi(EncryptText(txtPwd.Text.ToUpper, "vpoint")) & "',"
                SQL &= "'" & FixApostropi(txtNama.Text) & "',"
                SQL &= IIf(ckSupervisor.Checked, 1, 0) & ","
                SQL &= IIf(ckKasir.Checked, 1, 0) & ","
                SQL &= IIf(ckAutoPosting.Checked, 1, 0) & ","
                SQL &= NullTolong(txtGudang.EditValue) & ","
                SQL &= NullTolong(txtPegawai.EditValue) & ","
                SQL &= NullTolong(txtSupplier.EditValue) & ","
                SQL &= NullTolong(txtCustomer.EditValue) & ","
                SQL &= "-1,"
                SQL &= NullTolong(txtSatuan.EditValue) & ","
                SQL &= IIf(ckAktif.Checked, 1, 0) & ","
                SQL &= "'" & FixApostropi(txtCatatan.Text) & "',"
                SQL &= IIf(ckEditLayouts.Checked, 1, 0) & ","
                SQL &= NullTolInt(cbTipe.SelectedIndex) & ","
                SQL &= NullToLong(txtGudangSupplier.EditValue) & ","
                SQL &= NullToLong(txtGudangCustomer.EditValue) & ","
                SQL &= IIf(ckAccPusat.Checked, 1, 0) & ","
                SQL &= IIf(ckPengawasKasir.Checked, 1, 0) & ","
                SQL &= IIf(ckPengawasUtama.Checked, 1, 0) & ","
                SQL &= IIf(ckMnVPOS.Checked, 1, 0) & ","
                SQL &= IIf(ckMnSetting.Checked, 1, 0) & ","
                SQL &= IIf(ckMnDatabase.Checked, 1, 0) & ","
                SQL &= IIf(ckMnDeveloper.Checked, 1, 0) & ", "
                SQL &= IIf(ckReminderHutang.Checked, 1, 0) & ", "
                SQL &= NullToLong(txtDashboard1.EditValue) & ", "
                SQL &= NullToLong(txtDashboard2.EditValue) & ", "
                SQL &= NullToLong(txtDashboard3.EditValue) & ", "
                SQL &= NullToLong(txtDashboard4.EditValue) & " "
                SQL &= " )"
            Else
                SQL = "UPDATE MUser SET "
                SQL &= " Kode='" & FixApostropi(txtKode.Text) & "',"
                SQL &= " Pwd='" & FixApostropi(EncryptText(txtPwd.Text.ToUpper, "vpoint")) & "',"
                SQL &= " Nama='" & FixApostropi(txtNama.Text) & "',"
                SQL &= " IsSupervisor=" & IIf(ckSupervisor.Checked, 1, 0) & ","
                SQL &= " IsKasir=" & IIf(ckKasir.Checked, 1, 0) & ","
                SQL &= " IsAutoPosting=" & IIf(ckAutoPosting.Checked, 1, 0) & ","
                SQL &= " IDGudangDefault=" & NullTolong(txtGudang.EditValue) & ","
                SQL &= " IDAlamat=" & NullTolong(txtPegawai.EditValue) & ","
                SQL &= " IDSupplier=" & NullTolong(txtSupplier.EditValue) & ","
                SQL &= " IDPelanggan=" & NullTolong(txtCustomer.EditValue) & ","
                SQL &= " IDSatuan=" & NullTolong(txtSatuan.EditValue) & ","
                SQL &= " IsActive=" & IIf(ckAktif.Checked, 1, 0) & ","
                SQL &= " Keterangan='" & FixApostropi(txtCatatan.Text) & "',"
                SQL &= " Tipe=" & NullTolInt(cbTipe.SelectedIndex) & ","
                SQL &= " IDGudangPenerimaanSupplier=" & NullToLong(txtGudangSupplier.EditValue) & ","
                SQL &= " IDGudangPenerimaanCustomer=" & NullToLong(txtGudangCustomer.EditValue) & ","
                SQL &= " IsEditLayout=" & IIf(ckEditLayouts.Checked, 1, 0) & ","
                SQL &= " IsAccMutasi=" & IIf(ckAccPusat.Checked, 1, 0) & ","
                SQL &= " IsPengawasKasir=" & IIf(ckPengawasKasir.Checked, 1, 0) & ","
                SQL &= " IsPengawasUtamaKasir=" & IIf(ckPengawasUtama.Checked, 1, 0) & ","
                SQL &= " IsMnVPOS=" & IIf(ckMnVPOS.Checked, 1, 0) & ","
                SQL &= " IsMnSetting=" & IIf(ckMnSetting.Checked, 1, 0) & ","
                SQL &= " IsMnDatabase=" & IIf(ckMnDatabase.Checked, 1, 0) & ","
                SQL &= " IsMnDeveloper=" & IIf(ckMnDeveloper.Checked, 1, 0) & ", "
                SQL &= " IsReminderHutang=" & IIf(ckReminderHutang.Checked, 1, 0) & ", "
                SQL &= " IDDashBoard1=" & NullToLong(txtDashboard1.EditValue) & ", "
                SQL &= " IDDashBoard2=" & NullToLong(txtDashboard2.EditValue) & ", "
                SQL &= " IDDashBoard3=" & NullToLong(txtDashboard3.EditValue) & ", "
                SQL &= " IDDashBoard4=" & NullToLong(txtDashboard4.EditValue) & " "
                SQL &= " WHERE NoID=" & NoID
            End If
            If EksekusiSQL(SQL) >= 1 Then
                For i As Integer = 0 To GV1.RowCount - 1
                    SQL = "UPDATE MUserDAksesGudang SET IsActive=" & IIf(NullToBool(GV1.GetRowCellValue(i, "IsActive")), 1, 0) & " WHERE NoID=" & NullToLong(GV1.GetRowCellValue(i, "NoID"))
                    EksekusiSQL(SQL)
                Next
                GridView1.ExpandAllGroups()
                GridView1.RefreshData()
                For i As Integer = 0 To GridView1.RowCount - 1
                    'If NullToLong(GridView1.GetRowCellValue(i, "NoID")) = 1800 Then
                    '    MsgBox("test")
                    'End If
                    SQL = "UPDATE MUserD SET [Visible]=" & IIf(NullToBool(GridView1.GetRowCellValue(i, "Visible")), 1, 0) & ", [Enable]=" & IIf(NullToBool(GridView1.GetRowCellValue(i, "Enable")), 1, 0) & " WHERE NoID=" & NullToLong(GridView1.GetRowCellValue(i, "NoID"))
                    'If NullToBool(GridView1.GetRowCellValue(i, "Visible")) Then
                    EksekusiSQL(SQL)
                    'End If
                Next
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
        If txtKode.Text = "" Then
            XtraMessageBox.Show("Kode tidak boleh kosong.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
            txtKode.Focus()
            Return False
            Exit Function
        End If
        If txtNama.Text = "" Then
            XtraMessageBox.Show("Nama tidak boleh kosong.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
            txtKode.Focus()
            Return False
            Exit Function
        End If
        If CekKodeValid(txtKode.Text, KodeLama, "MUser", "Kode", Not IsNew) Then
            XtraMessageBox.Show("Kode sudah dipakai.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
            txtKode.Focus()
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
        Dim xOtorisasi As New frmOtorisasiAdmin
        Try
            If XtraMessageBox.Show("Simpan Layout?", NamaAplikasi, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = Windows.Forms.DialogResult.Yes AndAlso xOtorisasi.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
                Ini.TulisIniPath(folderLayouts & Me.Name & ".ini", "Form", "Width", Me.Width)
                Ini.TulisIniPath(folderLayouts & Me.Name & ".ini", "Form", "Height", Me.Height)
                Me.Left = (Screen.PrimaryScreen.WorkingArea.Width - Me.Width) \ 2
                Me.Top = (Screen.PrimaryScreen.WorkingArea.Height - Me.Height) \ 2

                LayoutControl1.SaveLayoutToXml(folderLayouts & Me.Name & LayoutControl1.Name & ".xml")
                gvPegawai.SaveLayoutToXml(folderLayouts & Me.Name & gvPegawai.Name & ".xml")
                gvCustomer.SaveLayoutToXml(folderLayouts & Me.Name & gvCustomer.Name & ".xml")
                gvSupplier.SaveLayoutToXml(folderLayouts & Me.Name & gvSupplier.Name & ".xml")
                gvGudang.SaveLayoutToXml(folderLayouts & Me.Name & gvGudang.Name & ".xml")
                gvSatuan.SaveLayoutToXml(FolderLayouts & Me.Name & gvSatuan.Name & ".xml")
                GV1.SaveLayoutToXml(FolderLayouts & Me.Name & GV1.Name & ".xml")
                GridView1.SaveLayoutToXml(FolderLayouts & Me.Name & GridView1.Name & ".xml")
            End If
        Catch ex As Exception
            XtraMessageBox.Show("Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            xOtorisasi.Dispose()
        End Try
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
            SQL = "INSERT INTO MUserDAksesGudang (IDUser,IDGudang,IDWilayah,IsActive)" & _
                  " SELECT " & NoID & ", MGudang.NoID, MGudang.IDWilayah, 0 FROM MGudang WHERE MGudang.NoID NOT IN (SELECT MUserDAksesGudang.IDGudang FROM MUserDAksesGudang WHERE MUserDAksesGudang.IDUser=" & NoID & ")"
            EksekusiSQL(SQL)
            'isi Menu Baru
            SQL = "INSERT INTO MUserD (IDUser,IDMenu,Caption,Enable,Visible)" & _
                  " SELECT " & NoID & ", MMenu.NoID, MMenu.Caption, 1, 1 FROM MMenu WHERE MMenu.NoID NOT IN (SELECT MUserD.IDMenu FROM MUserD WHERE MMenu.IsActive=1 AND MUserD.IDUser=" & NoID & ")"
            EksekusiSQL(SQL)
            'hapus Menu Double
            SQL = "Delete MUserD where NoID In (select Max(NoID) from muserD where IDUser=" & NoID & " group by IDMenu having count(NoID)>1)"
            EksekusiSQL(SQL)
            'Hapus Menu Tidak Aktif
            SQL = "Delete   MUserD Where IDMenu IN( SELECT NoID FROM MMenu WHERE MMenu.IsActive=0) AND MUserD.IDUser=" & NoID & " "
            EksekusiSQL(SQL)
            RefreshDetil()

            Me.Width = Ini.BacaIniPath(FolderLayouts & Me.Name & ".ini", "Form", "Width", Me.Width)
            Me.Height = Ini.BacaIniPath(folderLayouts & Me.Name & ".ini", "Form", "Height", Me.Height)

            Me.Left = (Screen.PrimaryScreen.WorkingArea.Width - Me.Width) \ 2
            Me.Top = (Screen.PrimaryScreen.WorkingArea.Height - Me.Height) \ 2

            If System.IO.File.Exists(folderLayouts & Me.Name & LayoutControl1.Name & ".xml") Then
                LayoutControl1.RestoreLayoutFromXml(folderLayouts & Me.Name & LayoutControl1.Name & ".xml")
            End If
            If System.IO.File.Exists(FolderLayouts & Me.Name & GV1.Name & ".xml") Then
                GV1.RestoreLayoutFromXml(FolderLayouts & Me.Name & GV1.Name & ".xml")
            End If
            If System.IO.File.Exists(FolderLayouts & Me.Name & GridView1.Name & ".xml") Then
                GridView1.RestoreLayoutFromXml(FolderLayouts & Me.Name & GridView1.Name & ".xml")
            End If
            FungsiControl.SetForm(Me)
        Catch ex As Exception
            XtraMessageBox.Show(ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            Windows.Forms.Cursor.Current = Cursors.Default
            dlg.Close()
            dlg.Dispose()
        End Try
    End Sub

    Private Sub BarButtonItem1_ItemClick_1(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem1.ItemClick
        cmdSave.PerformClick()
    End Sub

    Private Sub BarButtonItem2_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem2.ItemClick
        cmdTutup.PerformClick()
    End Sub

    Private Sub GV1_FocusedColumnChanged(ByVal sender As Object, ByVal e As DevExpress.XtraGrid.Views.Base.FocusedColumnChangedEventArgs) Handles GV1.FocusedColumnChanged
        If GV1.FocusedColumn.FieldName.ToLower = "IsActive".ToLower Then
            GV1.OptionsBehavior.Editable = True
        Else
            GV1.OptionsBehavior.Editable = False
        End If
    End Sub

    Private Sub GridView1_FocusedColumnChanged(ByVal sender As Object, ByVal e As DevExpress.XtraGrid.Views.Base.FocusedColumnChangedEventArgs) Handles GridView1.FocusedColumnChanged
        If (GridView1.FocusedColumn.FieldName.ToLower = "Enable".ToLower) Or (GridView1.FocusedColumn.FieldName.ToLower = "Visible".ToLower) Then
            GridView1.OptionsBehavior.Editable = True
        Else
            GridView1.OptionsBehavior.Editable = False
        End If
    End Sub

    Private Sub frmEntriUser_Shown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Shown
        txtKode.Focus()
    End Sub
End Class