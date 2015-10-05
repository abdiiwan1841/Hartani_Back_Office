
Imports System.Data.SqlClient
Imports System.Data.SQLite
Imports System.Windows.Forms
Imports DevExpress.XtraEditors
Imports DevExpress.XtraGrid.Views
Imports DevExpress.XtraGrid.Columns
Imports DevExpress.XtraGrid.Views.Base
Imports System.Drawing
Imports DevExpress.XtraEditors.Repository
Imports VPoint.mdlCetakCR
Imports System.Xml

Public Class frmDaftarPPNMasukkan
    Public FormName As String = "DaftarPPNMasukkan"
    Public FormEntriName As String = ""
    Public TableName As String = "MBeli"
    Public TableMaster As String = "MBeli"
    Public NoID As Long = -1
    Dim oda2 As SqlDataAdapter
    Dim ds As New DataSet
    Dim BS As New BindingSource

    Dim repckedit As New RepositoryItemCheckEdit
    Dim repdateedit As New RepositoryItemDateEdit
    Dim reptextedit As New RepositoryItemTextEdit
    Dim reppicedit As New RepositoryItemPictureEdit

    Private Sub frmDaftar_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing

    End Sub

    Private Sub ctrlDaftar_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        
        TglDari.EditValue = TanggalSystem
        TgMasapajak.DateTime = CDate(Format(TanggalSystem, "yyyy,MM,01"))
        TglSampai.EditValue = TanggalSystem
        RefreshPendukung()
        RefreshData()
        RestoreLayout()
        Me.lbDaftar.Text = Me.Text
        FungsiControl.SetForm(Me)
        TgMasapajak.Properties.EditMask = "MMMM-yyyy"

    End Sub
    Private Sub RefreshPendukung()
        Dim ds As New DataSet
        Dim sql As String
        Try
            sql = "SELECT MAlamatDNPWP.NoID, MAlamatDNPWP.NPWP, MAlamatDNPWP.NamaWp FROM MAlamatDNPWP INNER JOIN MAlamat ON MAlamat.NoID=MAlamatDNPWP.IDAlamat WHERE MAlamatDNPWP.IsActive=1 AND MAlamat.IsActive=1 AND MAlamat.IsSupplier=1"
            ds = ExecuteDataset("master", sql)
            txtCustomer.Properties.DataSource = ds.Tables("master")
            txtCustomer.Properties.ValueMember = "NPWP"
            txtCustomer.Properties.DisplayMember = "NamaWp"
            If System.IO.File.Exists(FolderLayouts & Me.Name & gvCustomer.Name & ".xml") Then
                gvCustomer.RestoreLayoutFromXml(FolderLayouts & Me.Name & gvCustomer.Name & ".xml")
            End If

     
          
        Catch ex As Exception
            XtraMessageBox.Show("Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            ds.Dispose()
        End Try
    End Sub
    Private Sub RestoreLayout()
        If System.IO.File.Exists(folderLayouts & FormName & ".xml") Then
            GV1.RestoreLayoutFromXml(folderLayouts & FormName & ".xml")
        End If
    End Sub
    'Sub generateform()
    '    Dim cn As New SqlConnection(StrKonSql)
    '    Dim ocmd2 As New SqlCommand
    '    Dim strsql As String = ""
    '    Dim Cur As Cursor = Windows.Forms.Cursor.Current
    '    Windows.Forms.Cursor.Current = Cursors.WaitCursor
    '    strsql = "select Mbeli.NoID,Mbeli.TglFakturPajak,MBeli.Total,Mbeli.NoFakturPajak,MalamatDNPWP.NPWP,MalamatDNPWP.NamaWP,MBeli.DPP,MBeli.PPN,MBeli.TglTerimaFakturPajak,MBeli.MasaPajak " & _
    '     "from MBeli left join MAlamatDNPWP on MBeli.IDAlamatDNPWP=MAlamatDNPWP.NoID where MBeli.IsTerimaFakturPajak=1 "

    '    'TglTerimaFakturPajak	datetime	Checked
    '    'TglFakturPajak	datetime	Checked
    '    'NoFakturPajak	varchar(20)	Checked
    '    'IDUserEntriFakturPajak	int	Checked
    '    'MasaPajak	datetime	Checked
    '    If TglDari.Enabled Then
    '        If InStr(strsql.ToLower, "where", CompareMethod.Text) > 0 Then
    '            strsql = strsql & " and (" & TableMaster & ".TglFakturPajak>='" & Format(TglDari.DateTime, "yyyy/MM/dd") & "' and " & TableMaster & ".TglFakturPajak<'" & Format(DateAdd(DateInterval.Day, 1, TglSampai.DateTime), "yyyy/MM/dd") & "') "
    '        Else
    '            strsql = strsql & " where (" & TableMaster & ".TglFakturPajak>='" & Format(TglDari.DateTime, "yyyy/MM/dd") & "' and " & TableMaster & ".TglFakturPajak<'" & Format(DateAdd(DateInterval.Day, 1, TglSampai.DateTime), "yyyy/MM/dd") & "') "
    '        End If
    '    End If
    '    If txtCustomer.Enabled Then
    '        If InStr(strsql.ToLower, "where", CompareMethod.Text) > 0 Then
    '            strsql = strsql & " and (" & TableMaster & ".IDAlamatDNPWP=" & NullTolInt(txtCustomer.EditValue) & ") "
    '        Else
    '            strsql = strsql & " where (" & TableMaster & ".IDAlamatDNPWP=" & NullTolInt(txtCustomer.EditValue) & ") "
    '        End If
    '    End If
    '    If TglDari.Enabled Then
    '        If InStr(strsql.ToLower, "where", CompareMethod.Text) > 0 Then
    '            strsql = strsql & " and (year(" & TableMaster & ".masaPajak)=" & TgMasapajak.DateTime.Year & " and month(" & TableMaster & ".TglFakturPajak)=" & TgMasapajak.DateTime.Month & ") "
    '        Else
    '            strsql = strsql & " where (year(" & TableMaster & ".masaPajak)=" & TgMasapajak.DateTime.Year & " and month(" & TableMaster & ".TglFakturPajak)=" & TgMasapajak.DateTime.Month & ") "
    '        End If
    '    End If
    '    ocmd2.Connection = cn
    '    ocmd2.CommandType = CommandType.Text
    '    ocmd2.CommandText = strsql
    '    cn.Open()

    '    oda2 = New SqlDataAdapter(ocmd2)
    '    oda2.Fill(ds, "Data")
    '    BS.DataSource = ds.Tables("Data")
    '    GC1.DataSource = BS.DataSource
    '    For i As Integer = 0 To GV1.Columns.Count - 1
    '        ' MsgBox(GV1.Columns(i).fieldname.ToString)
    '        Select Case GV1.Columns(i).ColumnType.Name.ToLower
    '            Case "int32", "int64", "int"
    '                GV1.Columns(i).DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
    '                GV1.Columns(i).DisplayFormat.FormatString = "n0"
    '            Case "decimal", "single", "money", "double"
    '                GV1.Columns(i).DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
    '                GV1.Columns(i).DisplayFormat.FormatString = "n2"
    '            Case "string"
    '                GV1.Columns(i).DisplayFormat.FormatType = DevExpress.Utils.FormatType.None
    '                GV1.Columns(i).DisplayFormat.FormatString = ""
    '            Case "date", "datetime"
    '                GV1.Columns(i).DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime
    '                GV1.Columns(i).DisplayFormat.FormatString = "dd-MM-yyyy"
    '            Case "byte[]"
    '                reppicedit.SizeMode = DevExpress.XtraEditors.Controls.PictureSizeMode.Squeeze
    '                GV1.Columns(i).OptionsColumn.AllowGroup = False
    '                GV1.Columns(i).OptionsColumn.AllowSort = False
    '                GV1.Columns(i).OptionsFilter.AllowFilter = False
    '                GV1.Columns(i).ColumnEdit = reppicedit
    '            Case "boolean"
    '                GV1.Columns(i).ColumnEdit = repckedit

    '        End Select
    '        If GV1.Columns(i).FieldName.Length >= 4 AndAlso GV1.Columns(i).FieldName.Substring(0, 4).ToLower = "Kode".ToLower Then
    '            GV1.Columns(i).Fixed = FixedStyle.Left
    '        ElseIf GV1.Columns(i).FieldName.ToLower = "Nama".ToLower Then
    '            GV1.Columns(i).Fixed = FixedStyle.Left
    '        End If
    '    Next
    '    'For i = 0 To ds.Tables("Master").Rows.Count - 1

    '    '    If NullTostr(ds.Tables("Master").Rows(i).Item("fieldname")).Trim = "" Then
    '    '        Dim unbColumn As GridColumn = GV1.Columns.AddField(NullTostr(ds.Tables("Master").Rows(i).Item("nama")))
    '    '        unbColumn.VisibleIndex = GV1.Columns.Count
    '    '        Select Case NullTostr(ds.Tables("Master").Rows(i).Item("Tipe"))

    '    '            Case "string"
    '    '                unbColumn.UnboundType = DevExpress.Data.UnboundColumnType.String

    '    '            Case "int", "bigint", "real", "money", "real", "Decimal", "float"
    '    '                unbColumn.UnboundType = DevExpress.Data.UnboundColumnType.Decimal
    '    '                ' Specify format settings.
    '    '                unbColumn.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
    '    '                unbColumn.DisplayFormat.FormatString = ds.Tables("Master").Rows(i).Item("format")
    '    '                unbColumn.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
    '    '            Case "date", "time", "datetime"
    '    '                unbColumn.UnboundType = DevExpress.Data.UnboundColumnType.DateTime
    '    '                ' Specify format settings.
    '    '                unbColumn.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime
    '    '                unbColumn.DisplayFormat.FormatString = ds.Tables("Master").Rows(i).Item("format")
    '    '        End Select

    '    '        ' Disable editing.
    '    '        unbColumn.OptionsColumn.AllowEdit = False

    '    '        ' Customize the appearance settings.
    '    '        unbColumn.AppearanceCell.BackColor = Color.LemonChiffon
    '    '    Else
    '    '        Dim bndColumn As GridColumn = GV1.Columns(NullTostr(ds.Tables("Master").Rows(i).Item("fieldname"))) ' GV1.Columns.AddField(NullTostr(ds.Tables("Master").Rows(i).Item("fieldname")))
    '    '        bndColumn.Caption = ds.Tables("Master").Rows(i).Item("caption")
    '    '        bndColumn.Visible = NullTobool(ds.Tables("Master").Rows(i).Item("visible"))
    '    '        If bndColumn.Visible Then
    '    '            bndColumn.VisibleIndex = GV1.Columns.Count
    '    '        End If
    '    '        ' GV1.Columns.AddField(ds.Tables("Master").Rows(i).Item("fieldname").ToString)
    '    '        Select Case ds.Tables("Master").Rows(i).Item("control")
    '    '            Case "checkedit"
    '    '                bndColumn.ColumnEdit = repckedit
    '    '            Case "textedit"
    '    '                bndColumn.ColumnEdit = reptextedit
    '    '            Case "dateedit"
    '    '                repdateedit.Mask.EditMask = ds.Tables("Master").Rows(i).Item("format").ToString
    '    '                repdateedit.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.DateTime
    '    '                repdateedit.Mask.UseMaskAsDisplayFormat = True
    '    '                bndColumn.ColumnEdit = repdateedit
    '    '            Case "lookupedit"
    '    '            Case "string"
    '    '                bndColumn.DisplayFormat.FormatType = DevExpress.Utils.FormatType.None
    '    '            Case "numeric"
    '    '                bndColumn.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
    '    '                bndColumn.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
    '    '                bndColumn.DisplayFormat.FormatString = ds.Tables("Master").Rows(i).Item("format")
    '    '            Case "int", "bigint", "real", "money", "real", "Decimal", "float"
    '    '                bndColumn.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
    '    '                bndColumn.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
    '    '                bndColumn.DisplayFormat.FormatString = ds.Tables("Master").Rows(i).Item("format")
    '    '            Case "date"
    '    '                bndColumn.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime
    '    '                bndColumn.DisplayFormat.FormatString = ds.Tables("Master").Rows(i).Item("format")
    '    '        End Select
    '    '    End If

    '    'Next
    '    ocmd2.Dispose()
    '    cn.Close()
    '    cn.Dispose()

    '    Windows.Forms.Cursor.Current = Cur
    'End Sub

    Private Sub SimpleButton6_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SimpleButton6.Click
        Tutup()
    End Sub

    Private Sub SimpleButton9_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SimpleButton9.Click
        DialogResult = Windows.Forms.DialogResult.OK
        Me.Close()

    End Sub

    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

    End Sub

    Private Sub SimpleButton2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SimpleButton2.Click
        Try
            If GV1.RowCount >= 1 Then
                If NullToLong(GV1.GetRowCellValue(GV1.FocusedRowHandle, "IsBeli")) >= 1 Then
                    Edit()
                Else
                    EditRetur()
                End If
            End If
        Catch ex As Exception
            XtraMessageBox.Show("Info Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
        End Try
    End Sub

    Private Sub EditRetur()
        Dim view As ColumnView = GC1.FocusedView
        Dim x As New frmEntriReturBeli
        Dim frmO As New frmOtorisasiAdmin
        Try
            Dim row As System.Data.DataRow = view.GetDataRow(GV1.FocusedRowHandle)
            Dim dc As Integer = GV1.FocusedRowHandle
            Dim IDDetil As Long = NullToLong(row("NoID"))
            Dim MasaPajak As Date = NullToDate(row("TglMasaPajak"))
            If Not clsPostingPembelian.IsLockPeriodeFP(MasaPajak) Then
                x.NoID = IDDetil
                x.pTipe = frmEntriReturBeli.pStatus.Edit
                x.MdiParent = Me.MdiParent
                x.WindowState = FormWindowState.Maximized

                'frmMain.BarManager1.Items("").Name

                'For Back Action
                x.FormNameDaftar = FormName
                x.TableNameDaftar = TableName
                x.TextDaftar = Text
                x.FormEntriDaftar = FormEntriName
                x.TableMasterDaftar = TableMaster
                x.IsDariDaftarPPN = True
                If Not clsPostingPembelian.ReturAdaDiPembayaranHutang(IDDetil) Then
                    x.IsEntriFakturPajak = True
                    x.Show()
                    x.Focus()
                Else
                    XtraMessageBox.Show(Me, "Informasi : Data Sudah Masuk ke Pembayaran Hutang.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
                    If frmO.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
                        x.IsEntriFakturPajak = True
                        x.Show()
                        x.Focus()
                    Else
                        x.IsEntriFakturPajak = False
                    End If
                End If
            Else
                XtraMessageBox.Show("Masa Pajak " & MasaPajak.ToString("MMMM-yyyy") & " sudah dikunci.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If
        Catch ex As Exception
            XtraMessageBox.Show("Untuk mengedit pilih data yang akan diedit terlebih dahulu lalu tekan tombol edit!", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Warning)
        Finally
            frmO.Dispose()
            x.Dispose()
        End Try
    End Sub

    Private Sub SimpleButton3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SimpleButton3.Click
        Hapus()
    End Sub

    Private Sub SimpleButton1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SimpleButton1.Click
        Baru()
    End Sub
    Sub Baru()
        Dim Brg As New frmEntriPPNMasukkan
        If Not clsPostingPembelian.IsLockPeriodeFP(TgMasapajak.DateTime) Then
            Brg.IsNew = True
            Brg.TgMasapajak.EditValue = TgMasapajak.EditValue
            Brg.tglTerimaFP.EditValue = Today
            If Brg.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
                RefreshData()
                GV1.FocusedRowHandle = GV1.LocateByDisplayText(0, GV1.Columns("NoID"), NoID.ToString("#,##0"))
            End If
        Else
            XtraMessageBox.Show("Masa Pajak " & TgMasapajak.DateTime.ToString("MMMM-yyyy") & " sudah dikunci.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If
        Brg.Dispose()
    End Sub
    Sub RefreshData()
        Dim cn As New SqlConnection(StrKonSql)
        Dim ocmd2 As New SqlCommand
        Dim Cur As Cursor = Windows.Forms.Cursor.Current
        Dim dlg As DevExpress.Utils.WaitDialogForm = Nothing
        Dim strsql As String = ""
        Try
            dlg = New DevExpress.Utils.WaitDialogForm(String.Format("Sedang merefresh data.{0}MOHON TUNGGU ...", vbCrLf), Application.ProductName)
            dlg.TopMost = False
            dlg.Show()
            Windows.Forms.Cursor.Current = Cursors.WaitCursor
            'Dengan SPP
            strsql = "SELECT MBeli.MasaPajak AS TglMasaPajak, MBeli.NoBendelPajak AS Bendel, ROUND(MBeli.DPP*0.10,0)-MBeli.PPN AS Selisih, MBeli.NoID, 'B' AS KodePajak, '2' AS KodeTransaksi, '1' AS KodeStatus, '1' AS KodeDokumen, '0' AS FlagVAT, " & _
                     " MAlamat.Kode AS KodeSupplier, MAlamatDNPWP.NPWP AS [NPWP], MAlamatDNPWP.NamaWP AS [Nama Supplier], " & _
                     " MBeli.NoFakturPajak AS  [Nomor Faktur Pajak], 0 AS [Jenis Dokumen], '' AS [Nomor Faktur Pajak Pengganti], '' AS [Jenis Dokumen Dokumen Pengganti / Retur], MBeli.TglFakturPajak AS [Tanggal Faktur Pajak]," & _
                     " NULL AS TanggalSSP, SUBSTRING(CONVERT(VARCHAR(10), MBeli.MasaPajak, 102), 6, 2) + SUBSTRING(CONVERT(VARCHAR(10), MBeli.MasaPajak, 102), 6, 2) AS MasaPajak, SUBSTRING(CONVERT(VARCHAR(10), MBeli.MasaPajak, 102), 1, 4) AS TahunPajak, " & _
                     " 0 AS Pembetulan, MBeli.DPP, MBeli.PPN, 0 AS [PPnBM], 1 AS IsBeli, (SELECT TOP 1 MPO.Kode FROM (MPOD INNER JOIN MPO ON MPO.NoID=MPOD.IDPO) INNER JOIN MBeliD ON MBeliD.IDPOD=MPOD.NoID WHERE MBeliD.IDBeli=MBeli.NoID) AS NoSPP, MBeli.Kode AS [KodeFaktur] " & _
                     " FROM MBeli " & _
                     " LEFT JOIN MAlamat ON MAlamat.NoID=MBeli.IDSupplier" & _
                     " LEFT JOIN MAlamatDNPWP ON MAlamatDNPWP.NoID=MBeli.IDAlamatDNPWP" & vbCrLf & _
                     " WHERE IsNull(MBeli.IsTanpaBarang,0)=0 AND MBeli.IsTerimaFakturPajak=1 "
            'If TglDari.Enabled AndAlso TglDari.Text <> "" Then
            '    strsql = strsql & " AND (MBeli.MasaPajak>='" & Format(TglDari.DateTime, "yyyy/MM/dd") & "' AND MBeli.MasaPajak<'" & Format(DateAdd(DateInterval.Day, 1, TglSampai.DateTime), "yyyy/MM/dd") & "') "
            'End If
            If txtCustomer.Enabled Then
                strsql = strsql & " AND (MAlamatDNPWP.NPWP='" & FixApostropi(txtCustomer.EditValue) & "') "
            End If
            If TgMasapajak.Enabled AndAlso TgMasapajak.Text <> "" Then
                strsql = strsql & " AND (YEAR(MBeli.MasaPajak)=" & TgMasapajak.DateTime.Year & " AND MONTH(MBeli.MasaPajak)=" & TgMasapajak.DateTime.Month & ") "
            End If

            If CheckEdit2.Checked Then
                strsql &= "UNION ALL SELECT MBeli.MasaPajak AS TglMasaPajak, MBeli.NoBendelPajak, ROUND(MBeli.DPP*0.10,0)-MBeli.PPN AS Selisih, MBeli.NoID, 'B' AS KodePajak, '2' AS KodeTransaksi, '1' AS KodeStatus, '1' AS KodeDokumen, '0' AS FlagVAT," & _
                         " MAlamat.Kode AS KodeSupplier, MAlamatDNPWP.NPWP AS [NPWP / Nomor Paspor], MAlamatDNPWP.NamaWP AS [Lawan Transaksi], " & _
                         " MBeli.NoFakturPajak AS  [Nomor Faktur / Dokumen], 0 AS [Jenis Dokumen], '' AS [Nomor Faktur Pengganti / Retur], '' AS [Jenis Dokumen Dokumen Pengganti / Retur], MBeli.TglFakturPajak AS [Tanggal Faktur / Dokumen]," & _
                         " NULL AS TanggalSSP, SUBSTRING(CONVERT(VARCHAR(10), MBeli.MasaPajak, 102), 6, 2) + SUBSTRING(CONVERT(VARCHAR(10), MBeli.MasaPajak, 102), 6, 2) AS MasaPajak, SUBSTRING(CONVERT(VARCHAR(10), MBeli.MasaPajak, 102), 1, 4) AS TahunPajak, " & _
                         " 0 AS Pembetulan, MBeli.DPP, MBeli.PPN, 0 AS [PPnBM], 1 AS IsBeli, '' AS NoSPP, MBeli.Kode " & _
                         " FROM MBeli " & _
                         " LEFT JOIN MAlamat ON MAlamat.NoID=MBeli.IDSupplier" & _
                         " LEFT JOIN MAlamatDNPWP ON MAlamatDNPWP.NoID=MBeli.IDAlamatDNPWP" & vbCrLf & _
                         " WHERE IsNull(MBeli.IsTanpaBarang,0)=1 AND MBeli.IsTerimaFakturPajak=1 "
                'If TglDari.Enabled AndAlso TglDari.Text <> "" Then
                '    strsql = strsql & " AND (MBeli.MasaPajak>='" & Format(TglDari.DateTime, "yyyy/MM/dd") & "' AND MBeli.MasaPajak<'" & Format(DateAdd(DateInterval.Day, 1, TglSampai.DateTime), "yyyy/MM/dd") & "') "
                'End If
                If txtCustomer.Enabled Then
                    strsql = strsql & " AND (MBeli.IDAlamatDNPWP=" & NullTolInt(txtCustomer.EditValue) & ") "
                End If
                If TgMasapajak.Enabled AndAlso TgMasapajak.Text <> "" Then
                    strsql = strsql & " AND (YEAR(MBeli.MasaPajak)=" & TgMasapajak.DateTime.Year & " AND MONTH(MBeli.MasaPajak)=" & TgMasapajak.DateTime.Month & ") "
                End If
            End If

            If CheckEdit1.Checked Then
                'Include Retur
                strsql &= "UNION ALL SELECT MReturBeli.MasaPajak AS TglMasaPajak, 0, ROUND(MReturBeli.DPP*0.10,0)-MReturBeli.NilaiPPN AS Selisih, MReturBeli.NoID, 'B' AS KodePajak, '2' AS KodeTransaksi, '1' AS KodeStatus, '4' AS KodeDokumen, '0' AS FlagVAT," & _
                          " MAlamat.Kode AS KodeSupplier, MAlamatDNPWP.NPWP AS [NPWP / Nomor Paspor], MAlamatDNPWP.NamaWP AS [Lawan Transaksi]," & _
                          " MReturBeli.NoFakturPajak AS  [Nomor Faktur / Dokumen], 1 AS [Jenis Dokumen], CONVERT(VARCHAR(50),MReturBeli.NoFPMasukkan) AS [Nomor Faktur Pengganti / Retur], '0' AS [Jenis Dokumen Dokumen Pengganti / Retur], MReturBeli.MasaPajak AS [Tanggal Faktur / Dokumen]," & _
                          " NULL AS TanggalSSP, SUBSTRING(CONVERT(VARCHAR(10), MReturBeli.MasaPajak, 102), 6, 2) + SUBSTRING(CONVERT(VARCHAR(10), MReturBeli.MasaPajak, 102), 6, 2) AS MasaPajak, SUBSTRING(CONVERT(VARCHAR(10), MReturBeli.MasaPajak, 102), 1, 4) AS TahunPajak," & _
                          " 0 AS Pembetulan, -1*MReturBeli.DPP AS DPP, -1*MReturBeli.NilaiPPN AS PPN, 0 AS [PPnBM], 0 AS IsBeli, '' AS NoSPP, MReturBeli.Kode " & _
                          " FROM MReturBeli " & _
                          " LEFT JOIN MAlamat ON MAlamat.NoID=MReturBeli.IDSupplier" & _
                          " LEFT JOIN MAlamatDNPWP ON MAlamatDNPWP.NoID=MReturBeli.IDAlamatDNPWP" & _
                          " WHERE MReturBeli.IsProsesPajak=1 "
                If TgMasapajak.Enabled AndAlso TgMasapajak.Text <> "" Then
                    strsql &= " AND (YEAR(MReturBeli.MasaPajak)=" & TgMasapajak.DateTime.Year & " AND MONTH(MReturBeli.MasaPajak)=" & TgMasapajak.DateTime.Month & ") "
                End If
                If txtCustomer.Enabled Then
                    strsql = strsql & " AND (MReturBeli.IDAlamatDNPWP=" & NullTolInt(txtCustomer.EditValue) & ") "
                End If
            End If

            strsql = "SELECT PPNMasukkan.* FROM (" & strsql & ") AS PPNMasukkan"
            ocmd2.Connection = cn
            ocmd2.CommandType = CommandType.Text
            ocmd2.CommandText = strsql
            cn.Open()
            oda2 = New SqlDataAdapter(ocmd2)
            If ds.Tables("Data") Is Nothing Then
            Else
                ds.Tables("Data").Clear()
            End If
            oda2.Fill(ds, "Data")
            BS.DataSource = ds.Tables("Data")
            GC1.DataSource = BS.DataSource
            For i As Integer = 0 To GV1.Columns.Count - 1
                Select Case GV1.Columns(i).ColumnType.Name.ToLower
                    Case "int32", "int64", "int"
                        GV1.Columns(i).DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
                        GV1.Columns(i).DisplayFormat.FormatString = "n0"
                    Case "decimal", "single", "money", "double"
                        GV1.Columns(i).DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
                        GV1.Columns(i).DisplayFormat.FormatString = "n2"
                    Case "string"
                        GV1.Columns(i).DisplayFormat.FormatType = DevExpress.Utils.FormatType.None
                        GV1.Columns(i).DisplayFormat.FormatString = ""
                    Case "date", "datetime"
                        GV1.Columns(i).DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime
                        GV1.Columns(i).DisplayFormat.FormatString = "dd-MM-yyyy"
                    Case "byte[]"
                        reppicedit.SizeMode = DevExpress.XtraEditors.Controls.PictureSizeMode.Squeeze
                        GV1.Columns(i).OptionsColumn.AllowGroup = False
                        GV1.Columns(i).OptionsColumn.AllowSort = False
                        GV1.Columns(i).OptionsFilter.AllowFilter = False
                        GV1.Columns(i).ColumnEdit = reppicedit
                    Case "boolean"
                        GV1.Columns(i).ColumnEdit = repckedit
                End Select
                'If GV1.Columns(i).FieldName.Length >= 4 AndAlso GV1.Columns(i).FieldName.Substring(0, 4).ToLower = "Kode".ToLower Then
                '    GV1.Columns(i).Fixed = FixedStyle.Left
                'ElseIf GV1.Columns(i).FieldName.ToLower = "Nama".ToLower Then
                '    GV1.Columns(i).Fixed = FixedStyle.Left
                'End If
            Next
            GV1.ShowFindPanel()
        Catch ex As Exception
            XtraMessageBox.Show("Info Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
        Finally
            ocmd2.Dispose()
            If cn.State = ConnectionState.Open Then
                cn.Close()
            End If
            cn.Dispose()
            Windows.Forms.Cursor.Current = Cursors.Default
            If Not dlg Is Nothing Then
                dlg.Close()
                dlg.Dispose()
            End If
        End Try
    End Sub
    Sub Edit()
        Dim brg As New frmEntriPPNMasukkan
        Dim view As ColumnView = GC1.FocusedView
        Try
            Dim row As System.Data.DataRow = view.GetDataRow(GV1.FocusedRowHandle)
            Dim dc As Integer = GV1.FocusedRowHandle
            Dim NoID As Long = NullToLong(row("NoID"))
            Dim MasaPajak As Date = NullToDate(row("TglMasaPajak"))
            If Not clsPostingPembelian.IsLockPeriodeFP(MasaPajak) Then
                brg.IsNew = False
                brg.NoID = NoID
                If brg.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
                    RefreshData()
                    GV1.ClearSelection()
                    GV1.FocusedRowHandle = GV1.LocateByDisplayText(0, GV1.Columns("NoID"), NoID.ToString("#,##0"))
                    GV1.SelectRow(GV1.FocusedRowHandle)
                End If
            Else
                XtraMessageBox.Show("Masa Pajak " & MasaPajak.ToString("MMMM-yyyy") & " sudah dikunci.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If
        Catch ex As Exception
            XtraMessageBox.Show("Untuk mengedit pilih data yang akan diedit terlebih dahulu lalu tekan tombol edit", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            brg.Dispose()
        End Try
    End Sub
    Sub Hapus()
        Dim view As ColumnView = GC1.FocusedView
        Try
            Dim row As System.Data.DataRow = view.GetDataRow(GV1.FocusedRowHandle)
            Dim dc As Integer = GV1.FocusedRowHandle
            Dim NoID As Long = NullToLong(row("NoID"))
            Dim MasaPajak As Date = NullToDate(row("TglMasaPajak"))
            If Not clsPostingPembelian.IsLockPeriodeFP(MasaPajak) Then
                If XtraMessageBox.Show("Yakin mau hapus data ini ?", NamaAplikasi, MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                    If NullToLong(GV1.GetRowCellValue(GV1.FocusedRowHandle, "IsBeli")) >= 1 Then
                        EksekusiSQL("Update MBeli set IsTerimaFakturPajak=0, MasaPajak=NULL, TglFakturPajak=Null where NoID= " & NoID.ToString)
                    Else
                        EksekusiSQL("Update MReturBeli set IsProsesPajak=0 WHERE NoID= " & NoID.ToString)
                    End If
                    RefreshData()
                    GV1.ClearSelection()
                    GV1.FocusedRowHandle = GV1.LocateByDisplayText(0, GV1.Columns("NoID"), (NoID).ToString("#,##0"))
                    GV1.SelectRow(GV1.FocusedRowHandle)
                End If
            Else
                XtraMessageBox.Show("Masa Pajak " & MasaPajak.ToString("MMMM-yyyy") & " sudah dikunci.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If
        Catch ex As Exception
            XtraMessageBox.Show("Untuk menghapus pilih data yang akan dihapus terlebih dahulu lalu tekan tombol hapus!", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
        End Try
    End Sub

    Private Sub BarButtonItem1_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem1.ItemClick
        Baru()
    End Sub

    Private Sub BarButtonItem2_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem2.ItemClick
        Edit()
    End Sub

    Private Sub BarButtonItem3_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem3.ItemClick
        Hapus()
    End Sub

    Private Sub BarButtonItem4_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem4.ItemClick
        Tutup()
    End Sub
    Sub Tutup()
        DialogResult = Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub SimpleButton8_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdRefresh.Click
        RefreshData()
    End Sub

    Private Sub BarButtonItem6_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem6.ItemClick
        PrintPreview()
    End Sub
    Sub ExportExcel()
        Dim dlgsave As New SaveFileDialog
        dlgsave.Title = "Export Daftar ke Excel"
        dlgsave.Filter = "Excel Files|*.xls"
        If dlgsave.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
            GC1.ExportToXls(dlgsave.FileName)
            BukaFile(dlgsave.FileName)
        End If
        dlgsave.Dispose()
    End Sub
    Sub PrintPreview()
        Dim NamaFile As String = Application.StartupPath & "\Report\LaporanPPNMasukkan.rpt"
        Try
            If EditReport Then
                ViewReport(Me.MdiParent, action_.Edit, NamaFile, "Laporan PPN Masukkan", , , "")
            Else
                ViewReport(Me.MdiParent, action_.Preview, NamaFile, "Laporan PPN Masukkan", , , "MasaPajak=CDATE(" & TgMasapajak.DateTime.ToString("yyyy,MM,01") & ")&IsRetur=" & IIf(CheckEdit1.Checked, True, False) & "&IsOngkos=" & IIf(CheckEdit2.Checked, True, False))
            End If
        Catch ex As Exception

        End Try
    End Sub

    Private Sub SimpleButton4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SimpleButton4.Click
        Dim x As New frmCSVViewer
        Try
            x.ShowDialog(Me)
        Catch ex As Exception
        Finally
            x.Dispose()
        End Try
    End Sub

    Private Sub SimpleButton5_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SimpleButton5.Click
        PrintPreview()
    End Sub

    Private Sub BarButtonItem5_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem5.ItemClick
        ExportExcel()
    End Sub

    Private Sub LabelControl1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LabelControl1.Click
        TglDari.Enabled = Not TglDari.Enabled
        TglSampai.Enabled = TglDari.Enabled
    End Sub
    Private Sub CetakFaktur(ByVal action As action_)
        Dim namafile As String
        Dim view As ColumnView = GC1.FocusedView
        Try
            namafile = Application.StartupPath & "\report\Faktur" & TableMaster & ".rpt"
            Dim row As System.Data.DataRow = view.GetDataRow(GV1.FocusedRowHandle)
            Dim dc As Integer = GV1.FocusedRowHandle
            Dim NoID As Long = NullTolong(row("NoID"))
            'ViewReport(action, namafile, "Faktur Penjualan", "{MJual.NoID}=" & NoID)
            If EditReport Then
                action = action_.Edit
            Else
                action = action_.Preview
            End If
            ViewReport(Me.ParentForm, action, namafile, Me.Text, "{" & TableMaster & ".NoID}=" & NoID)
            'CetakCRViewer(action, Me.MdiParent, namafile, "Cetak " & Me.Text, "{MJual.NoID}=" & NoID)
        Catch EX As Exception
            DevExpress.XtraEditors.XtraMessageBox.Show("Kesalahan:" & EX.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub
    Private Sub mnFaktur_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles mnFaktur.ItemClick
        If frmMain.ckEditRpt.Checked Then
            CetakFaktur(action_.Edit)
        Else
            CetakFaktur(action_.Preview)
        End If
    End Sub

    Private Sub cmdFaktur_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdFaktur.Click
        ExportCSV()
    End Sub

    Private Sub ExportCSV()
        Dim SQL As String = ""
        Dim ds As New DataSet
        Dim dt1 As DataTable = Nothing, dt2 As DataTable = Nothing
        'Dim dlg As New SaveFileDialog
        Dim csvfile As String = ""
        Dim dlgRefresh As DevExpress.Utils.WaitDialogForm = Nothing
        Dim myWriter As System.IO.StreamWriter = Nothing
        Dim Kalimat As New System.Text.StringBuilder
        Dim fileSource As String = Application.StartupPath & "\System\PPNMasukkan.csv"
        Dim AdaHeader As Boolean = False
        Dim frmOpsiExport As New frmOpsiExportFP
        Try
            If TgMasapajak.Enabled AndAlso TgMasapajak.Text <> "" Then
                frmOpsiExport.TgMasapajak.DateTime = TgMasapajak.DateTime
                frmOpsiExport.txtPembetulan.EditValue = 0
                If frmOpsiExport.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
                    dlgRefresh = New DevExpress.Utils.WaitDialogForm(String.Format("Sedang mengexport data.{0}MOHON TUNGGU ...", vbCrLf), Application.ProductName)
                    dlgRefresh.TopMost = False
                    dlgRefresh.Show()
                    Windows.Forms.Cursor.Current = Cursors.WaitCursor

                    If System.IO.File.Exists(fileSource) Then
                        System.IO.File.Delete(fileSource)
                    End If
                    Application.DoEvents()

                    csvfile = frmOpsiExport.txtFileName.Text
                    myWriter = New System.IO.StreamWriter(fileSource)
                    SQL = "SELECT 'B' AS KodePajak, '2' AS KodeTransaksi, '1' AS KodeStatus, '1' AS KodeDokumen, '0' AS FlagVAT," & _
                          " MAlamatDNPWP.NPWP AS [NPWP / Nomor Paspor], MAlamatDNPWP.NamaWP AS [Lawan Transaksi], " & _
                          " MBeli.NoFakturPajak AS  [Nomor Faktur / Dokumen], 0 AS [Jenis Dokumen], NULL AS [Nomor Faktur Pengganti / Retur], NULL AS [Jenis Dokumen Dokumen Pengganti / Retur], MBeli.TglFakturPajak AS [Tanggal Faktur / Dokumen]," & _
                          " NULL AS TanggalSSP, SUBSTRING(CONVERT(VARCHAR(10), MBeli.MasaPajak, 102), 6, 2) + SUBSTRING(CONVERT(VARCHAR(10), MBeli.MasaPajak, 102), 6, 2) AS MasaPajak, SUBSTRING(CONVERT(VARCHAR(10), MBeli.MasaPajak, 102), 1, 4) AS TahunPajak, " & _
                          " " & NullToLong(frmOpsiExport.txtPembetulan.EditValue) & " AS Pembetulan, MBeli.DPP, MBeli.PPN, 0 AS [PPnBM]" & _
                          " FROM MBeli " & _
                          " LEFT JOIN MAlamat ON MAlamat.NoID=MBeli.IDSupplier" & _
                          " LEFT JOIN MAlamatDNPWP ON MAlamatDNPWP.NoID=MBeli.IDAlamatDNPWP" & vbCrLf & _
                          " WHERE MBeli.IsTerimaFakturPajak=1 " & _
                          " AND (YEAR(MBeli.MasaPajak)=" & TgMasapajak.DateTime.Year & " AND MONTH(MBeli.MasaPajak)=" & TgMasapajak.DateTime.Month & ") "
                    ds = ExecuteDataset("Data", SQL)
                    For Each dt1 In ds.Tables
                        Kalimat.Remove(0, Kalimat.Length)
                        If Not AdaHeader Then
                            For col As Integer = 0 To dt1.Columns.Count - 1
                                Kalimat.Append(dt1.Columns(col).ColumnName.ToString & IIf(col = dt1.Columns.Count - 1, "", ";"))
                                AdaHeader = True
                            Next
                        End If
                        If Kalimat.Length >= 1 Then
                            'Kalimat.AppendLine()
                            myWriter.WriteLine(Kalimat)
                        End If
                        For i As Integer = 0 To ds.Tables(dt1.TableName).Rows.Count - 1
                            Kalimat.Remove(0, Kalimat.Length)
                            With ds.Tables(dt1.TableName).Rows(i)
                                Kalimat.Append(NullToStr(.Item("KodePajak")) & ";" & _
                                               NullToStr(.Item("KodeTransaksi")) & ";" & _
                                               NullToStr(.Item("KodeStatus")) & ";" & _
                                               NullToStr(.Item("KodeDokumen")) & ";" & _
                                               NullToStr(.Item("FlagVAT")) & ";" & _
                                               NullToStr(.Item("NPWP / Nomor Paspor")).Replace(".", "").Replace("-", "") & ";" & _
                                               NullToStr(.Item("Lawan Transaksi")) & ";" & _
                                               NullToStr(.Item("Nomor Faktur / Dokumen")) & ";" & _
                                               NullToStr(.Item("Jenis Dokumen")) & ";" & _
                                               NullToStr(.Item("Nomor Faktur Pengganti / Retur")) & ";" & _
                                               NullToStr(.Item("Jenis Dokumen Dokumen Pengganti / Retur")) & ";" & _
                                               NullToDate(.Item("Tanggal Faktur / Dokumen")).ToString("dd/MM/yyyy") & ";" & _
                                               NullToStr(.Item("TanggalSSP")) & ";" & _
                                               NullToStr(.Item("MasaPajak")) & ";" & _
                                               NullToStr(.Item("TahunPajak")) & ";" & _
                                               NullToStr(.Item("Pembetulan")) & ";" & _
                                               NullToDbl(.Item("DPP")).ToString("##########0") & ";" & _
                                               NullToDbl(.Item("PPN")).ToString("##########0") & ";" & _
                                               NullToDbl(.Item("PPnBM")).ToString("##########0"))
                            End With
                            myWriter.WriteLine(Kalimat)
                        Next
                        'myWriter.WriteLine("")
                    Next

                    'Include Retur
                    SQL = "SELECT 'B' AS KodePajak, '2' AS KodeTransaksi, '1' AS KodeStatus, '4' AS KodeDokumen, '0' AS FlagVAT," & _
                          " MAlamatDNPWP.NPWP AS [NPWP / Nomor Paspor], MAlamatDNPWP.NamaWP AS [Lawan Transaksi]," & _
                          " MReturBeli.NoFakturPajak AS  [Nomor Faktur / Dokumen], 1 AS [Jenis Dokumen], MReturBeli.NoFPMasukkan AS [Nomor Faktur Pengganti / Retur], 0 AS [Jenis Dokumen Dokumen Pengganti / Retur], MReturBeli.MasaPajak AS [Tanggal Faktur / Dokumen], MReturBeli.MasaPajak AS [MasaPajak Faktur / Dokumen]," & _
                          " NULL AS TanggalSSP, SUBSTRING(CONVERT(VARCHAR(10), MReturBeli.MasaPajak, 102), 6, 2) + SUBSTRING(CONVERT(VARCHAR(10), MReturBeli.MasaPajak, 102), 6, 2) AS MasaPajak, SUBSTRING(CONVERT(VARCHAR(10), MReturBeli.MasaPajak, 102), 1, 4) AS TahunPajak," & _
                          " " & NullToLong(frmOpsiExport.txtPembetulan.EditValue) & " AS Pembetulan, -1*MReturBeli.DPP AS DPP, -1*MReturBeli.NilaiPPN AS PPN, 0 AS [PPnBM]" & _
                          " FROM MReturBeli " & _
                          " LEFT JOIN MAlamat ON MAlamat.NoID=MReturBeli.IDSupplier" & _
                          " LEFT JOIN MAlamatDNPWP ON MAlamatDNPWP.NoID=MReturBeli.IDAlamatDNPWP" & _
                          " WHERE MReturBeli.IsProsesPajak=1 " & _
                          " AND (YEAR(MReturBeli.MasaPajak)=" & TgMasapajak.DateTime.Year & " AND MONTH(MReturBeli.MasaPajak)=" & TgMasapajak.DateTime.Month & ") "
                    ds = ExecuteDataset("Data", SQL)
                    For Each dt2 In ds.Tables
                        Kalimat.Remove(0, Kalimat.Length)
                        If Not AdaHeader Then
                            For col As Integer = 0 To dt2.Columns.Count - 1
                                Kalimat.Append(dt2.Columns(col).ColumnName.ToString & IIf(col = dt2.Columns.Count - 1, "", ";"))
                                AdaHeader = True
                            Next
                        End If
                        If Kalimat.Length >= 1 Then
                            'Kalimat.AppendLine()
                            myWriter.WriteLine(Kalimat)
                        End If
                        For i As Integer = 0 To ds.Tables(dt2.TableName).Rows.Count - 1
                            Kalimat.Remove(0, Kalimat.Length)
                            With ds.Tables(dt2.TableName).Rows(i)
                                Kalimat.Append(NullToStr(.Item("KodePajak")) & ";" & _
                                               NullToStr(.Item("KodeTransaksi")) & ";" & _
                                               NullToStr(.Item("KodeStatus")) & ";" & _
                                               NullToStr(.Item("KodeDokumen")) & ";" & _
                                               NullToStr(.Item("FlagVAT")) & ";" & _
                                               NullToStr(.Item("NPWP / Nomor Paspor")).Replace(".", "").Replace("-", "") & ";" & _
                                               NullToStr(.Item("Lawan Transaksi")) & ";" & _
                                               NullToStr(.Item("Nomor Faktur / Dokumen")) & ";" & _
                                               NullToStr(.Item("Jenis Dokumen")) & ";" & _
                                               NullToStr(.Item("Nomor Faktur Pengganti / Retur")) & ";" & _
                                               NullToStr(.Item("Jenis Dokumen Dokumen Pengganti / Retur")) & ";" & _
                                               NullToDate(.Item("Tanggal Faktur / Dokumen")).ToString("dd/MM/yyyy") & ";" & _
                                               NullToStr(.Item("TanggalSSP")) & ";" & _
                                               NullToStr(.Item("MasaPajak")) & ";" & _
                                               NullToStr(.Item("TahunPajak")) & ";" & _
                                               NullToStr(.Item("Pembetulan")) & ";" & _
                                               NullToDbl(.Item("DPP")).ToString("##########0") & ";" & _
                                               NullToDbl(.Item("PPN")).ToString("##########0") & ";" & _
                                               NullToDbl(.Item("PPnBM")).ToString("##########0"))
                            End With
                            myWriter.WriteLine(Kalimat)
                        Next
                        'myWriter.WriteLine("")
                    Next

                    myWriter.Close()
                    myWriter.Dispose()

                    If System.IO.File.Exists(fileSource) Then
                        System.IO.File.Copy(fileSource, csvfile, True)
                        XtraMessageBox.Show("Data Berhasil diexport.", NamaAplikasi, MessageBoxButtons.OK)
                    End If
                End If
            Else
                XtraMessageBox.Show("Tentukan dulu masa pajak yang akan diexport.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If
        Catch ex As Exception
            XtraMessageBox.Show("Info Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
        Finally
            frmOpsiExport.Dispose()
            ds.Dispose()
            Windows.Forms.Cursor.Current = Cursors.Default
            If Not dlgRefresh Is Nothing Then
                dlgRefresh.Close()
                dlgRefresh.Dispose()
            End If
            If Not myWriter Is Nothing Then
                myWriter.Close()
            End If
        End Try
    End Sub

    Private Sub lbCustomer_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lbCustomer.Click
        txtCustomer.Enabled = Not txtCustomer.Enabled

    End Sub

    
    Private Sub TgMasapajak_EditValueChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles TgMasapajak.EditValueChanged
        Try
            TglDari.DateTime = CDate(Format(TgMasapajak.DateTime, "yyyy,MM,01"))
            TglSampai.DateTime = DateAdd(DateInterval.Day, -1, DateAdd(DateInterval.Month, 1, TglDari.DateTime))
        Catch ex As Exception

        End Try
    End Sub

    Private Sub LabelControl3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LabelControl3.Click
        Try
            TgMasapajak.Enabled = Not TgMasapajak.Enabled
        Catch ex As Exception

        End Try
    End Sub

    Private Sub mnSimpanLayouts_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles mnSimpanLayouts.ItemClick
        Dim xOtorisasi As New frmOtorisasiAdmin
        Try
            If XtraMessageBox.Show("Simpan Layout?", NamaAplikasi, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = Windows.Forms.DialogResult.Yes AndAlso xOtorisasi.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
                GV1.SaveLayoutToXml(FolderLayouts & FormName & ".xml")
            End If
        Catch ex As Exception
            XtraMessageBox.Show("Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            xOtorisasi.Dispose()
        End Try
    End Sub

    Private Sub mnRefresh_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles mnRefresh.ItemClick
        cmdRefresh.PerformClick()
    End Sub

    Private Sub ckEdit_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ckEdit.CheckedChanged

    End Sub

    Private Sub ckEdit_CheckStateChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ckEdit.CheckStateChanged
        'Dim x As New frmOtorisasiAdmin
        'If x.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
        'Else

        'End If
    End Sub

    Private Sub ckEdit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ckEdit.Click

    End Sub
End Class