Imports System.Data.SqlClient
Imports DevExpress.XtraEditors
Imports DevExpress.XtraGrid.Views.Base
Imports DevExpress.XtraEditors.Repository
Imports VPoint.mdlCetakCR
Imports DevExpress.XtraGrid.Views.Grid.ViewInfo
Imports DevExpress.XtraGrid.Views.Grid

Public Class frmGeneratePO
    Public FormName As String = "GeneratePO"
    Public FormEntriName As String = ""
    Public TableName As String = ""
    Public TableNameD As String = ""
    Public TableMaster As String = ""
    Public IDPO As Long = -1
    Public IDSupplier As Long = 0
    Dim oda2 As SqlDataAdapter

    Dim ds As New DataSet
    Dim BS As New BindingSource

    Dim repckedit As New RepositoryItemCheckEdit
    Dim reppicedit As New RepositoryItemPictureEdit
 
    Dim IDBarang As Long

     

    Private Sub ctrlDaftar_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim dlg As DevExpress.Utils.WaitDialogForm = Nothing
        Try
            Dim curentcursor As Cursor = Windows.Forms.Cursor.Current
            Windows.Forms.Cursor.Current = Cursors.WaitCursor
            dlg = New DevExpress.Utils.WaitDialogForm(String.Format("Creating component and analize database.{0}MOHON TUNGGU ...", vbCrLf), Application.ProductName)
            dlg.TopMost = False
            dlg.Show()
            Windows.Forms.Cursor.Current = Cursors.WaitCursor
            TglDari.DateTime = CDate(TanggalSystem.AddDays(-31).ToString("yyyy/MM/dd"))
            TglSampai.DateTime = CDate(TanggalSystem.AddDays(-1).ToString("yyyy/MM/dd"))
            RefreshLookUp()
            Me.lbDaftar.Text = Me.Text
            XtraTabPage1.Text = Me.Text
            RefreshData()
            RestoreLayout()
            txtSupplier.EditValue = IDSupplier
            FungsiControl.SetForm(Me)
        Catch ex As Exception
            XtraMessageBox.Show(ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            Windows.Forms.Cursor.Current = Cursors.Default
            dlg.Close()
            dlg.Dispose()
        End Try
    End Sub

    Private Sub RefreshLookUp()
        Dim ds As New DataSet
        Dim SQL As String = ""
        Try
            SQL = "SELECT NoID, Kode, Nama FROM MKategori WHERE IsActive=1  "
            ds = ExecuteDataset("MKategori", SQL)
            txtKategori.Properties.DataSource = ds.Tables("MKategori")
            txtKategori.Properties.DisplayMember = "Nama"
            txtKategori.Properties.ValueMember = "NoID"
            SQL = "SELECT NoID, Kode, Nama FROM MAlamat  WHERE IsActive=1 and IsSupplier=1 "
            ds = ExecuteDataset("MSupplier", SQL)
            txtSupplier.Properties.DataSource = ds.Tables("MSupplier")
            txtSupplier.Properties.DisplayMember = "Nama"
            txtSupplier.Properties.ValueMember = "NoID"
            SQL = "SELECT NoID, Kode, Nama FROM MSatuan  WHERE IsActive=1  "
            ds = ExecuteDataset("MSatuan", SQL)
            txtSatuan.DataSource = ds.Tables("MSatuan")
            txtSatuan.DisplayMember = "Nama"
            txtSatuan.ValueMember = "NoID"
        Catch ex As Exception
            XtraMessageBox.Show("Info : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
        Finally
            ds.Dispose()
        End Try
    End Sub
    Private Sub RestoreLayout()
        With GV1
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
                'If .Columns(i).FieldName.Length >= 4 AndAlso .Columns(i).FieldName.Substring(0, 4).ToLower = "Kode".ToLower Then
                '    .Columns(i).Fixed = FixedStyle.Left
                'ElseIf (.Columns(i).FieldName.Length >= 3 AndAlso .Columns(i).FieldName.Substring(0, 3).ToLower = "Qty".ToLower) Or .Columns(i).FieldName.Length >= 5 AndAlso .Columns(i).FieldName.Substring(0, 5).ToLower = "Total".ToLower Then
                '    .Columns(i).GroupFormat.FormatType = FormatType.Numeric
                '    .Columns(i).GroupFormat.FormatString = "{0:n2}"
                'ElseIf .Columns(i).FieldName.ToLower = "Nama".ToLower Then
                '    .Columns(i).Fixed = FixedStyle.Left
                'End If
            Next
        End With

        If System.IO.File.Exists(FolderLayouts & FormName & ".xml") Then
            GV1.RestoreLayoutFromXml(FolderLayouts & FormName & ".xml")
        End If
    End Sub

    Private Sub SimpleButton6_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SimpleButton6.Click
        Tutup()
    End Sub

    Public Sub New()
        ' This call is required by the Windows Form Designer.
        InitializeComponent()
        ' Add any initialization after the InitializeComponent() call.
    End Sub

    'Private Sub SimpleButton1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SimpleButton1.Click
    '    If NullTobool(Ini.BacaIni("Application", "UseFramework", False)) Then
    '        Baru()
    '    Else
    '        Select Case TableMaster.ToUpper
    '            Case "MLPB"
    '                Dim view As ColumnView = GC1.FocusedView
    '                Dim x As New frmEntriLPB
    '                Try
    '                    x.NoID = -1
    '                    x.pTipe = frmEntriLPB.pStatus.Baru
    '                    x.MdiParent = Me.MdiParent
    '                    x.WindowState = FormWindowState.Normal
    '                    x.Show()
    '                    x.Focus()
    '                    'If x.ShowDialog(Me)= Windows.Forms.DialogResult.OK Then
    '                    '    RefreshData()
    '                    '    GV1.ClearSelection()
    '                    '    GV1.FocusedRowHandle = GV1.LocateByDisplayText(0, GV1.Columns("NoID"), (x.NoID).ToString("#,##0"))
    '                    '    GV1.SelectRow(GV1.FocusedRowHandle)
    '                    'End If
    '                Catch ex As Exception
    '                    XtraMessageBox.Show("Error : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
    '                Finally
    '                    'x.Close()
    '                    'x.Dispose()
    '                End Try
    '            Case "MJUAL"
    '                Dim view As ColumnView = GC1.FocusedView
    '                Dim x As New frmEntriJual
    '                Try
    '                    x.NoID = -1
    '                    x.pTipe = frmEntriJual.pStatus.Baru
    '                    If x.ShowDialog(Me)= Windows.Forms.DialogResult.OK Then
    '                        RefreshData()
    '                        GV1.ClearSelection()
    '                        GV1.FocusedRowHandle = GV1.LocateByDisplayText(0, GV1.Columns("NoID"), (x.NoID).ToString("#,##0"))
    '                        GV1.SelectRow(GV1.FocusedRowHandle)
    '                    End If
    '                Catch ex As Exception
    '                    XtraMessageBox.Show("Error : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
    '                Finally
    '                    x.Close()
    '                    x.Dispose()
    '                End Try
    '            Case "MRETURJUAL"
    '                Dim view As ColumnView = GC1.FocusedView
    '                Dim x As New frmEntriReturJual
    '                Try
    '                    x.NoID = -1
    '                    x.pTipe = frmEntriReturJual.pStatus.Baru
    '                    If x.ShowDialog(Me)= Windows.Forms.DialogResult.OK Then
    '                        RefreshData()
    '                        GV1.ClearSelection()
    '                        GV1.FocusedRowHandle = GV1.LocateByDisplayText(0, GV1.Columns("NoID"), (x.NoID).ToString("#,##0"))
    '                        GV1.SelectRow(GV1.FocusedRowHandle)
    '                    End If
    '                Catch ex As Exception
    '                    XtraMessageBox.Show("Error : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
    '                Finally
    '                    x.Close()
    '                    x.Dispose()
    '                End Try
    '            Case "MPO"
    '                Dim view As ColumnView = GC1.FocusedView
    '                Dim x As New frmEntriPO
    '                Try
    '                    x.NoID = -1
    '                    x.pTipe = frmEntriPO.pStatus.Baru
    '                    If x.ShowDialog(Me)= Windows.Forms.DialogResult.OK Then
    '                        RefreshData()
    '                        GV1.ClearSelection()
    '                        GV1.FocusedRowHandle = GV1.LocateByDisplayText(0, GV1.Columns("NoID"), (x.NoID).ToString("#,##0"))
    '                        GV1.SelectRow(GV1.FocusedRowHandle)
    '                    End If
    '                Catch ex As Exception
    '                    XtraMessageBox.Show("Error : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
    '                Finally
    '                    x.Close()
    '                    x.Dispose()
    '                End Try
    '            Case "MSO"
    '                Dim view As ColumnView = GC1.FocusedView
    '                Dim x As New frmEntriSO
    '                Try
    '                    x.NoID = -1
    '                    x.pTipe = frmEntriSO.pStatus.Baru
    '                    If x.ShowDialog(Me)= Windows.Forms.DialogResult.OK Then
    '                        RefreshData()
    '                        GV1.ClearSelection()
    '                        GV1.FocusedRowHandle = GV1.LocateByDisplayText(0, GV1.Columns("NoID"), (x.NoID).ToString("#,##0"))
    '                        GV1.SelectRow(GV1.FocusedRowHandle)
    '                    End If
    '                Catch ex As Exception
    '                    XtraMessageBox.Show("Error : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
    '                Finally
    '                    x.Close()
    '                    x.Dispose()
    '                End Try
    '            Case "MBELI"
    '                Dim view As ColumnView = GC1.FocusedView
    '                Dim x As New frmEntriBeli
    '                Try
    '                    x.NoID = -1
    '                    x.pTipe = frmEntriBeli.pStatus.Baru
    '                    If x.ShowDialog(Me)= Windows.Forms.DialogResult.OK Then
    '                        RefreshData()
    '                        GV1.ClearSelection()
    '                        GV1.FocusedRowHandle = GV1.LocateByDisplayText(0, GV1.Columns("NoID"), (x.NoID).ToString("#,##0"))
    '                        GV1.SelectRow(GV1.FocusedRowHandle)
    '                    End If
    '                Catch ex As Exception
    '                    XtraMessageBox.Show("Error : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
    '                Finally
    '                    x.Close()
    '                    x.Dispose()
    '                End Try

    '            Case "MRETURBELI"
    '                Dim view As ColumnView = GC1.FocusedView
    '                Dim x As New frmEntriReturBeli
    '                Try
    '                    x.NoID = -1
    '                    x.pTipe = frmEntriReturBeli.pStatus.Baru
    '                    If x.ShowDialog(Me)= Windows.Forms.DialogResult.OK Then
    '                        RefreshData()
    '                        GV1.ClearSelection()
    '                        GV1.FocusedRowHandle = GV1.LocateByDisplayText(0, GV1.Columns("NoID"), (x.NoID).ToString("#,##0"))
    '                        GV1.SelectRow(GV1.FocusedRowHandle)
    '                    End If
    '                Catch ex As Exception
    '                    XtraMessageBox.Show("Error : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
    '                Finally
    '                    x.Close()
    '                    x.Dispose()
    '                End Try
    '        End Select
    '    End If
    'End Sub
    Sub GenerateData()
        Dim strsql As String = ""
        EksekusiSQL("delete from MGeneratePo where IDUser=" & IDUserAktif)
        strsql = "insert into MGeneratePO(IDuser,IDBarangD,IDBarang,IDSatuan,Harga,DiscPersen1,DiscPersen2,DiscPersen3,Disc1,HargaPcs,Konversi,HargaJualPcs) " & _
                             "select " & IDUserAktif & " as IDUser, MBarangD.NoID,MBarangD.IDBarang,MBarang.IDSatuanHarga,MBarang.HargaBeli,MBarang.DiscBeli1,MBarang.DiscBeli2,MBarang.DiscBeli3," & _
                             "MBarang.DiscRp,MBarang.HargaBeliPcs,MBarang.CtnPcs,MBarang.HargaJual " & _
                             "from MBarangD inner join MBarang on MBarangD.IDBarang=MBarang.NoID " & _
                             "where MBarangD.IsActive=1 and MBarang.IsActive=1"
        If txtKategori.Enabled Then
            strsql = strsql & " and MBarang.IDKategori=" & NullTolInt(txtKategori.EditValue)
        End If
        If txtSupplier.Enabled Then
            strsql = strsql & " and (MBarang.IDSupplier1=" & NullTolInt(txtSupplier.EditValue) & " or MBarang.IDSupplier2=" & NullTolInt(txtSupplier.EditValue) & " or MBarang.IDSupplier3=" & NullTolInt(txtSupplier.EditValue) & " or MBarang.IDSupplier4=" & NullTolInt(txtSupplier.EditValue) & " or MBarang.IDSupplier5=" & NullTolInt(txtSupplier.EditValue) & ")"
        End If
        EksekusiSQL(strsql)


        strsql = "Update MGeneratePO set QtyJualPcs=MJ.QtyJual from MGeneratePO " & _
                 "left join (select MJuald.IDBarangD, SUm(MJuald.Qty*MJualD.Konversi) QtyJual " & _
                 "from MJualD inner join Mjual on Mjuald.idjual=mjual.noid " & _
                 "inner join MBarangD on MJuald.IDBarangD=MBarangD.Noid " & _
                 "inner join MBarang on MBarangD.IDBarang=MBarang.Noid " & _
                 " where MJual.tanggal>='" & Format(TglDari.DateTime, "yyyy-MM-dd") & "' and MJual.tanggal<'" & Format(TglSampai.DateTime.AddDays(1), "yyyy-MM-dd") & "' "
        If txtKategori.Enabled Then
            strsql = strsql & " and MBarang.IDKategori=" & NullTolInt(txtKategori.EditValue)
        End If
        If txtSupplier.Enabled Then
            strsql = strsql & " and (MBarang.IDSupplier1=" & NullTolInt(txtSupplier.EditValue) & " or MBarang.IDSupplier2=" & NullTolInt(txtSupplier.EditValue) & " or MBarang.IDSupplier3=" & NullTolInt(txtSupplier.EditValue) & " or MBarang.IDSupplier4=" & NullTolInt(txtSupplier.EditValue) & " or MBarang.IDSupplier5=" & NullTolInt(txtSupplier.EditValue) & ")"
        End If
        strsql = strsql & "group by IDBarangD ) MJ on MJ.IDBarangD=MGeneratePO.IDBarangD " & _
                 "where MGeneratePO.IDUser=" & IDUserAktif
        EksekusiSQL(strsql)

        Dim isperluwhere As Boolean = True
        strsql = "Update  MGeneratePO set QtyAkhirPcs=MS.SaldoAkhir from MGeneratePO " & _
                "left join (select MKartustok.IDBarangD,sum(qtyMasuk*MKartustok.Konversi-QtyKeluar*MKartustok.Konversi) SaldoAkhir from MKartuStok " & _
                "inner join MBarangD on MKartustok.IDBarangD = MBarangD.NoID " & _
                "inner join MBarang on MBarangD.IDBarang= MBarang.NoID "
        If txtKategori.Enabled Then
            strsql = strsql & IIf(isperluwhere, " where ", " and ") & " MBarang.IDKategori=" & NullTolInt(txtKategori.EditValue)
            isperluwhere = False
        End If
        If txtSupplier.Enabled Then
            strsql = strsql & IIf(isperluwhere, " where ", " and ") & " (MBarang.IDSupplier1=" & NullTolInt(txtSupplier.EditValue) & " or MBarang.IDSupplier2=" & NullTolInt(txtSupplier.EditValue) & " or MBarang.IDSupplier3=" & NullTolInt(txtSupplier.EditValue) & " or MBarang.IDSupplier4=" & NullTolInt(txtSupplier.EditValue) & " or MBarang.IDSupplier5=" & NullTolInt(txtSupplier.EditValue) & ")"
            isperluwhere = False
        End If
        strsql = strsql & " group by IDbarangD) MS " & _
                 "on MGeneratePO.IDbarangD=MS.IDBarangD " & _
               "where MGeneratePO.IDUser=" & IDUserAktif
        EksekusiSQL(strsql)

        strsql = "update  MGeneratePO set QtyHitungOrderPcs=(isnull(QtyJualPcs,0)-case when isnull(QtyAkhirPcs,0)>0  then isnull(QtyAkhirPcs,0) else 0 end) / case when isnull(Konversi,0)<=0 then 1  else Konversi end " & _
       "where MGeneratePO.IDUser=" & IDUserAktif
        EksekusiSQL(strsql)

    End Sub
    Sub RefreshData()
        Dim dlg As DevExpress.Utils.WaitDialogForm = Nothing
        Dim cn As New SqlConnection(StrKonSql)
        Dim ocmd2 As New SqlCommand
        Dim Cur As Cursor = Windows.Forms.Cursor.Current
        Dim strsql As String
        Try
            Dim curentcursor As Cursor = Windows.Forms.Cursor.Current
            Windows.Forms.Cursor.Current = Cursors.WaitCursor
            dlg = New DevExpress.Utils.WaitDialogForm(String.Format("Sedang merefresh data.{0}MOHON TUNGGU ...", vbCrLf), Application.ProductName)
            dlg.TopMost = False
            dlg.Show()
            Windows.Forms.Cursor.Current = Cursors.WaitCursor
            strsql = "SELECT MGeneratePO.*,MBarangD.Barcode,MBarang.KodeAlias AS Alias, MBarang.Catatan AS CBarang,MBarang.Kode,rtrim(MBarang.Nama + ' ' + isnull(MBarangD.Varian,'')) as Nama from MGeneratePO " & _
                     " left join MbarangD on MGeneratePO.IDBarangD=MBarangD.NoID " & _
                     " left join Mbarang  on MBarangD.IDBarang=MBarang.NoID " & _
                     " where MGeneratePO.IDUser=" & IDUserAktif


            'If RadioButton1.Checked Then ' 
            '    If isperluwhere Then
            '        strsql = strsql & " where "
            '    Else
            '        strsql = strsql & " and "
            '    End If
            '    strsql = strsql & " XYZ.TotalQty<0"
            '    isperluwhere = False
            'ElseIf RadioButton2.Checked Then
            '    If isperluwhere Then
            '        strsql = strsql & " where "
            '    Else
            '        strsql = strsql & " and "
            '    End If
            '    strsql = strsql & " XYZ.TotalQty<=0"
            '    isperluwhere = False
            'ElseIf RadioButton3.Checked Then
            '    If isperluwhere Then
            '        strsql = strsql & " where "
            '    Else
            '        strsql = strsql & " and "
            '    End If
            '    strsql = strsql & " XYZ.TotalQty<XYZ.QtyMinimum"
            '    isperluwhere = False
            'ElseIf RadioButton4.Checked Then
            '    If isperluwhere Then
            '        strsql = strsql & " where "
            '    Else
            '        strsql = strsql & " and "
            '    End If
            '    strsql = strsql & " XYZ.TotalQty>0"
            '    isperluwhere = False
            'End If

            ds = ExecuteDataset("MGeneratePO", strsql)
            BS.DataSource = ds.Tables("MGeneratePO")
            GC1.DataSource = BS.DataSource

        Catch ex As Exception
            XtraMessageBox.Show("Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            ocmd2.Dispose()
            cn.Close()
            cn.Dispose()
            Windows.Forms.Cursor.Current = Cur
            Windows.Forms.Cursor.Current = Cursors.Default
            dlg.Close()
            dlg.Dispose()

        End Try
    End Sub
    'Dim cn As New SqlConnection(StrKonSql)
    'Dim ocmd2 As New SqlCommand
    'Dim strsql As String = ""
    'Dim Cur As Cursor = Windows.Forms.Cursor.Current
    '    Windows.Forms.Cursor.Current = Cursors.WaitCursor
    'Dim SQL As String = ""
    'Dim dsT2 As New DataSet
    '    strsql = IIf(InStr(TableName, "select", CompareMethod.Text) > 0, TableName, "select * from " & TableName)
    '    If TglDari.Enabled Then
    '        If InStr(strsql.ToLower, "where", CompareMethod.Text) > 0 Then
    '            strsql = strsql & " and (" & TableMaster & ".Tanggal>='" & Format(TglDari.DateTime, "yyyy/MM/dd") & "' and " & TableMaster & ".Tanggal<'" & Format(DateAdd(DateInterval.Day, 1, TglSampai.DateTime), "yyyy/MM/dd") & "') "
    '        Else
    '            strsql = strsql & " where (" & TableMaster & ".Tanggal>='" & Format(TglDari.DateTime, "yyyy/MM/dd") & "' and " & TableMaster & ".Tanggal<'" & Format(DateAdd(DateInterval.Day, 1, TglSampai.DateTime), "yyyy/MM/dd") & "') "
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
    '' MsgBox(GV1.Columns(i).fieldname.ToString)
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
    '            Case "date"
    '                GV1.Columns(i).DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime
    '                GV1.Columns(i).DisplayFormat.FormatString = "dd-MM-yyyy"
    '            Case "datetime"
    '                GV1.Columns(i).DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime
    '                GV1.Columns(i).DisplayFormat.FormatString = "dd-MM-yyyy HH:mm"
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

    '    If TableMaster.ToUpper = "MSO".ToUpper Then
    '        mnGenerateSPK.Visibility = DevExpress.XtraBars.BarItemVisibility.Always
    '    ElseIf TableMaster.ToUpper = "MPO".ToUpper Then
    '        mnSetPOSelesai.Visibility = DevExpress.XtraBars.BarItemVisibility.Always
    '        mnSetPOBelumSelesai.Visibility = DevExpress.XtraBars.BarItemVisibility.Always
    '    Else
    '        mnSetPOBelumSelesai.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
    '        mnSetPOSelesai.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
    '        mnGenerateSPK.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
    '    End If

    '    ocmd2.Dispose()
    '    cn.Close()
    '    cn.Dispose()

    '    If TableMaster.ToUpper = "MBeli".ToUpper Then
    '        SQL = "SELECT MPOD.NoID, MPO.Kode, MBarang.Kode AS KodeBarang, MAlamat.Kode AS KodeSupplier, MAlamat.Nama AS NamaSupplier, MBarang.Nama AS NamaBarang, "
    '        SQL &= " (MPOD.Harga-isnull(MPOD.DISC1,0)-isnull(MPOD.DISC2,0)-isnull(MPOD.DISC3,0)"
    '        SQL &= " -(MPOD.Harga*isnull(MPOD.DiscPersen1,0)/100)"
    '        SQL &= " -(MPOD.Harga*isnull(MPOD.DiscPersen2,0)/100)"
    '        SQL &= " -(MPOD.Harga*isnull(MPOD.DiscPersen3,0)/100)"
    '        SQL &= " ) AS Harga , MPOD.Qty, MSatuan.Nama AS Satuan, MPOD.HargaPcs AS [Harga(Pcs)], MPOD.Qty*MPOD.Konversi AS [Qty(Pcs)], MPOD.Qty*MPOD.Konversi- " & DiBeli() & "  AS [Sisa(Pcs)]"
    '        SQL &= " FROM MPOD LEFT JOIN MSatuan ON MSatuan.NoID=MPOD.IDSatuan "
    '        SQL &= " LEFT JOIN (MPO LEFT JOIN MAlamat ON MAlamat.NoID=MPO.IDSupplier) ON MPO.NoID=MPOD.IDPO "
    '        SQL &= " LEFT JOIN MBarang ON MBarang.NoID=MPOD.IDBarang "
    '        SQL &= " WHERE (MPO.IsSelesai=0 OR MPO.IsSelesai IS NULL) AND (MPOD.Qty*MPOD.Konversi - " & DiBeli() & ">0) AND MBarang.IsActive = 1 And MPO.IsPosted = 1 "
    '        dsT2 = ExecuteDataset("MPOD", SQL)
    '        GridControl1.DataSource = dsT2.Tables("MPOD")
    '        For i As Integer = 0 To GridView1.Columns.Count - 1
    '' MsgBox(GV1.Columns(i).fieldname.ToString)
    '            Select Case GridView1.Columns(i).ColumnType.Name.ToLower

    '                Case "int32", "int64", "int"
    '                    GridView1.Columns(i).DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
    '                    GridView1.Columns(i).DisplayFormat.FormatString = "n0"
    '                Case "decimal", "single", "money", "double"
    '                    GridView1.Columns(i).DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
    '                    GridView1.Columns(i).DisplayFormat.FormatString = "n2"
    '                Case "string"
    '                    GridView1.Columns(i).DisplayFormat.FormatType = DevExpress.Utils.FormatType.None
    '                    GridView1.Columns(i).DisplayFormat.FormatString = ""
    '                Case "date"
    '                    GridView1.Columns(i).DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime
    '                    GridView1.Columns(i).DisplayFormat.FormatString = "dd-MM-yyyy"
    '                Case "datetime"
    '                    GridView1.Columns(i).DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime
    '                    GridView1.Columns(i).DisplayFormat.FormatString = "dd-MM-yyyy HH:mm"
    '                Case "byte[]"
    '                    reppicedit.SizeMode = DevExpress.XtraEditors.Controls.PictureSizeMode.Squeeze
    '                    GridView1.Columns(i).OptionsColumn.AllowGroup = False
    '                    GridView1.Columns(i).OptionsColumn.AllowSort = False
    '                    GridView1.Columns(i).OptionsFilter.AllowFilter = False
    '                    GridView1.Columns(i).ColumnEdit = reppicedit
    '                Case "boolean"
    '                    GridView1.Columns(i).ColumnEdit = repckedit
    '            End Select
    '            If GridView1.Columns(i).FieldName.Length >= 4 AndAlso GridView1.Columns(i).FieldName.Substring(0, 4).ToLower = "Kode".ToLower Then
    '                GridView1.Columns(i).Fixed = FixedStyle.Left
    '            ElseIf GridView1.Columns(i).FieldName.ToLower = "Nama".ToLower Then
    '                GridView1.Columns(i).Fixed = FixedStyle.Left
    '            End If
    '        Next

    '        SQL = "SELECT MLPBD.NoID, MBarang.Kode AS KodeBarang, MBarang.Nama AS NamaBarang, MLPB.Kode, MLPB.Tanggal, MAlamat.Nama AS Supplier, MBagPembelian.Nama AS Penerima, MLPBD.Qty * MLPBD.Konversi AS [QtyPcs], MLPBD.Qty*MLPBD.Konversi-" & DiSTB() & "  AS [Sisa(Pcs)] "
    '        SQL &= " FROM (MLPBD LEFT OUTER JOIN (MLPB LEFT OUTER JOIN MAlamat AS MBagPembelian ON MBagPembelian.NoID = MLPB.IDBagPembelian "
    '        SQL &= " LEFT OUTER JOIN MAlamat ON MLPB.IDSupplier = MAlamat.NoID)"
    '        SQL &= " ON MLPB.NoID=MLPBD.IDLPB) "
    '        SQL &= " LEFT OUTER JOIN MBarang ON MLPBD.IDBarang = MBarang.NoID "
    '        SQL &= " WHERE MBarang.ISActive=1 AND (MLPBD.Qty*MLPBD.Konversi-" & DiSTB() & ">0) And MLPB.IsPosted = 1 "

    '        dsT2 = ExecuteDataset("MLPBD", SQL)
    '        GridControl2.DataSource = dsT2.Tables("MLPBD")
    '        For i As Integer = 0 To GridView2.Columns.Count - 1
    '' MsgBox(GV1.Columns(i).fieldname.ToString)
    '            Select Case GridView2.Columns(i).ColumnType.Name.ToLower

    '                Case "int32", "int64", "int"
    '                    GridView2.Columns(i).DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
    '                    GridView2.Columns(i).DisplayFormat.FormatString = "n0"
    '                Case "decimal", "single", "money", "double"
    '                    GridView2.Columns(i).DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
    '                    GridView2.Columns(i).DisplayFormat.FormatString = "n2"
    '                Case "string"
    '                    GridView2.Columns(i).DisplayFormat.FormatType = DevExpress.Utils.FormatType.None
    '                    GridView2.Columns(i).DisplayFormat.FormatString = ""
    '                Case "date"
    '                    GridView2.Columns(i).DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime
    '                    GridView2.Columns(i).DisplayFormat.FormatString = "dd-MM-yyyy"
    '                Case "datetime"
    '                    GridView2.Columns(i).DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime
    '                    GridView2.Columns(i).DisplayFormat.FormatString = "dd-MM-yyyy HH:mm"
    '                Case "byte[]"
    '                    reppicedit.SizeMode = DevExpress.XtraEditors.Controls.PictureSizeMode.Squeeze
    '                    GridView2.Columns(i).OptionsColumn.AllowGroup = False
    '                    GridView2.Columns(i).OptionsColumn.AllowSort = False
    '                    GridView2.Columns(i).OptionsFilter.AllowFilter = False
    '                    GridView2.Columns(i).ColumnEdit = reppicedit
    '                Case "boolean"
    '                    GridView2.Columns(i).ColumnEdit = repckedit
    '            End Select
    '            If GridView2.Columns(i).FieldName.Length >= 4 AndAlso GridView2.Columns(i).FieldName.Substring(0, 4).ToLower = "Kode".ToLower Then
    '                GridView2.Columns(i).Fixed = FixedStyle.Left
    '            ElseIf GridView2.Columns(i).FieldName.ToLower = "Nama".ToLower Then
    '                GridView2.Columns(i).Fixed = FixedStyle.Left
    '            End If
    '        Next
    '    Else
    '        XtraTabPage2.PageVisible = False
    '    End If

    '    Windows.Forms.Cursor.Current = Cur

    Private Sub BarButtonItem4_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem4.ItemClick
        Tutup()
    End Sub
    Sub Tutup()
        DialogResult = Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub SimpleButton8_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SimpleButton8.Click
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
            GV1.ExportToXls(dlgsave.FileName)
            BukaFile(dlgsave.FileName)
        End If
        dlgsave.Dispose()
    End Sub
    Sub PrintPreview()
        GC1.ShowPrintPreview()
    End Sub
    Private Sub CetakMRPTJual(ByVal Action As action_)

    End Sub
    Private Sub SimpleButton4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SimpleButton4.Click
        ExportExcel()
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
            If System.IO.File.Exists(namafile) Then
                Dim row As System.Data.DataRow = view.GetDataRow(GV1.FocusedRowHandle)
                Dim dc As Integer = GV1.FocusedRowHandle
                If row Is Nothing Then XtraMessageBox.Show("Untuk mencetak, pilih item yang mau dicetak dan click cetak faktur.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information) : Exit Sub
                Dim NoID As Long = NullToLong(row("NoID"))
                'ViewReport(action, namafile, "Faktur Penjualan", "{MJual.NoID}=" & NoID)
                If EditReport Then
                    action = action_.Edit
                Else
                    action = action_.Preview
                End If
                If Not EditReport Then
                    'mnPosting.PerformClick()
                End If
                ViewReport(Me.ParentForm, action, namafile, Me.Text, "{" & TableMaster & ".NoID}=" & NoID & " AND {" & TableMaster & ".IsPosted}=True")
                'CetakCRViewer(action, Me.MdiParent, namafile, "Cetak " & Me.Text, "{MJual.NoID}=" & NoID)
            Else
                DevExpress.XtraEditors.XtraMessageBox.Show("Nama File :" & namafile & " tidak ditemukan.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Stop)
            End If
        Catch EX As Exception
            XtraMessageBox.Show("Untuk mencetak, pilih item yang mau dicetak dan click cetak faktur.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
            'DevExpress.XtraEditors.XtraMessageBox.Show("Kesalahan:" & EX.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub
    Private Sub CetakFakturPanjang(ByVal action As action_)
        Dim namafile As String
        Dim view As ColumnView = GC1.FocusedView
        Try
            namafile = Application.StartupPath & "\report\FakturPanjang" & TableMaster & ".rpt"
            If System.IO.File.Exists(namafile) Then
                Dim row As System.Data.DataRow = view.GetDataRow(GV1.FocusedRowHandle)
                If row Is Nothing Then XtraMessageBox.Show("Untuk mencetak, pilih item yang mau dicetak dan click cetak faktur.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information) : Exit Sub
                Dim dc As Integer = GV1.FocusedRowHandle
                Dim NoID As Long = NullToLong(row("NoID"))

                'ViewReport(action, namafile, "Faktur Penjualan", "{MJual.NoID}=" & NoID)
                If EditReport Then
                    action = action_.Edit
                Else
                    action = action_.Preview
                End If
                If Not EditReport Then
                    'mnPosting.PerformClick()
                End If
                ViewReport(Me.ParentForm, action, namafile, Me.Text, "{" & TableMaster & ".NoID}=" & NoID)
                'CetakCRViewer(action, Me.MdiParent, namafile, "Cetak " & Me.Text, "{MJual.NoID}=" & NoID)
            Else
                DevExpress.XtraEditors.XtraMessageBox.Show("Nama File :" & namafile & " tidak ditemukan.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Stop)
            End If
        Catch EX As Exception
            XtraMessageBox.Show("Untuk mencetak, pilih item yang mau dicetak dan click cetak faktur.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
            'DevExpress.XtraEditors.XtraMessageBox.Show("Kesalahan:" & EX.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub GV1_CellValueChanged(ByVal sender As Object, ByVal e As DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs) Handles GV1.CellValueChanged
        Dim row As System.Data.DataRow = GV1.GetDataRow(GV1.FocusedRowHandle)
        Dim HargaPcs As Double
        Dim JumlahBeli As Double
        Dim JumlahJual As Double
        Dim qty As Double
        Dim NoID As Long
        qty = NullToDbl(row("Qty"))
        NoID = NullToDbl(row("NoID"))
        HargaPcs = (NullToDbl(row("Harga")) * (100 - NullToDbl(row("DiscPersen1"))) * (100 - NullToDbl(row("DiscPersen2"))) * (100 - NullToDbl(row("DiscPersen3"))) / 1000000 - NullToDbl(row("Disc1"))) / IIf(NullToDbl(row("Konversi")) = 0, 1, NullToDbl(row("Konversi")))
        row("HargaPcs") = HargaPcs
        JumlahBeli = NullToDbl(row("Qty")) * (NullToDbl(row("Harga")) * (100 - NullToDbl(row("DiscPersen1"))) * (100 - NullToDbl(row("DiscPersen2"))) * (100 - NullToDbl(row("DiscPersen3"))) / 1000000 - NullToDbl(row("Disc1")))
        row("JumlahBeli") = JumlahBeli
        JumlahJual = NullToDbl(row("Qty")) * NullToDbl(row("HargaJualPcs")) * NullToDbl(row("Konversi"))
        row("JumlahJual") = JumlahJual
        EksekusiSQL("update MGeneratePO set HargaPcs=" & FixKoma(HargaPcs) & ",JumlahBeli=" & FixKoma(JumlahBeli) & ",JumlahJual=" & FixKoma(JumlahJual) & ",Qty=" & FixKoma(qty) & " where NoID=" & NoID)
        Application.DoEvents()
    End Sub

    Private Sub GV1_FocusedRowChanged(ByVal sender As Object, ByVal e As DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs) Handles GV1.FocusedRowChanged
        Try
            If TableMaster.ToUpper = "MBeli".ToUpper Then
                If IsShowStock Then
                    clsPostingKartuStok.LihatHasilPosting(NullToLong(GV1.GetRowCellValue(GV1.FocusedRowHandle, "IDBeli")), TableMaster)
                End If
            ElseIf TableMaster.ToUpper = "MReturBeli".ToUpper Then
                If IsShowStock Then
                    clsPostingKartuStok.LihatHasilPosting(NullToLong(GV1.GetRowCellValue(GV1.FocusedRowHandle, "IDReturBeli")), TableMaster)
                End If
            ElseIf TableMaster.ToUpper = "MRevisiHargaBeli".ToUpper Then
                If IsShowStock Then
                    clsPostingKartuStok.LihatHasilPosting(NullToLong(GV1.GetRowCellValue(GV1.FocusedRowHandle, "IDRevisiHargaBeli")), TableMaster)
                End If
            Else
                If IsShowHasilPostingan Then
                    clsPostingKartuStok.HasilPosting(NullToLong(GV1.GetRowCellValue(GV1.FocusedRowHandle, "NoID")), TableMaster)
                End If
            End If
        Catch ex As Exception
            XtraMessageBox.Show("Info : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
        End Try
    End Sub

    Private Sub GV1_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles GV1.MouseDown
        Dim View As GridView = CType(sender, GridView)
        If View Is Nothing Then Return
        ' obtaining hit info
        Dim hitInfo As GridHitInfo = View.CalcHitInfo(New System.Drawing.Point(e.X, e.Y))
        If (e.Button = Windows.Forms.MouseButtons.Right) And (hitInfo.InRow) And _
          (Not View.IsGroupRow(hitInfo.RowHandle)) Then
            PopupMenu1.ShowPopup(Control.MousePosition)
        End If
    End Sub

    Private Sub mnSaveLayouts_ItemClick(ByVal sender As Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles mnSaveLayouts.ItemClick
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

    Private Sub XtraTabControl1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles XtraTabControl1.Click

    End Sub


    Private Sub mnRefresh_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles mnRefresh.ItemClick
        SimpleButton8.PerformClick()
    End Sub


    Private Sub LabelControl2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LabelControl2.Click
        txtKategori.Enabled = Not txtKategori.Enabled
    End Sub

    Private Sub LabelControl4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LabelControl4.Click
        txtSupplier.Enabled = Not txtSupplier.Enabled
    End Sub

    Private Sub ckAllStock_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ckAllStock.CheckedChanged
        RefreshData()
    End Sub

    Private Sub SimpleButton2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SimpleButton2.Click
        GenerateData()
        RefreshData()
    End Sub

    Private Sub GC1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles GC1.Click

    End Sub

    Private Sub GridView1_CellValueChanged(ByVal sender As Object, ByVal e As DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs) Handles GridView1.CellValueChanged
       
     
    End Sub

    Private Sub SimpleButton1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SimpleButton1.Click
        SimpleButton1.Enabled = True
        Application.DoEvents()
        Dim strsql As String = "select * from MGeneratePO where Qty>0 and IDUser=" & IDUserAktif
        Dim ds As DataSet
        Dim JumlahData As Long
        Dim i As Long
        ds = ExecuteDataset("AmbilGenerate", strsql)
        JumlahData = ds.Tables("AmbilGenerate").Rows.Count
        For i = 0 To JumlahData - 1
            strsql = "INSERT INTO MPOD (NoID,IDPO,NoUrut,Tgl,Jam,IDBarang,IDBarangD,IDSatuan,Qty,QtyPcs,Harga,HargaPcs,DiscPersen1,DiscPersen2,DiscPersen3,Disc1,Jumlah,Catatan,IDWilayah,Konversi,HargaNetto,HargaJualPcs) VALUES ("
            strsql &= NullToLong(GetNewID("MPOD", "NoID")) & ","
            strsql &= IDPO & ","
            strsql &= GetNewID("MPOD", "NoUrut", " WHERE IDPO=" & IDPO) & ","
            strsql &= "GetDate(),"
            strsql &= "GetDate(),"
            strsql &= NullToLong(ds.Tables("AmbilGenerate").Rows(i).Item("IDBarang")) & ","
            strsql &= NullToLong(ds.Tables("AmbilGenerate").Rows(i).Item("IDBarangD")) & ","
            strsql &= NullToLong(ds.Tables("AmbilGenerate").Rows(i).Item("IDSatuan")) & ","
            strsql &= FixKoma(ds.Tables("AmbilGenerate").Rows(i).Item("Qty")) & ","
            strsql &= FixKoma(NullToDbl(ds.Tables("AmbilGenerate").Rows(i).Item("Qty")) * NullToDbl(ds.Tables("AmbilGenerate").Rows(i).Item("Konversi"))) & ","
            strsql &= FixKoma(NullToDbl(ds.Tables("AmbilGenerate").Rows(i).Item("Harga"))) & ","
            strsql &= FixKoma(NullToDbl(ds.Tables("AmbilGenerate").Rows(i).Item("HargaPcs"))) & ","
            strsql &= FixKoma(NullToDbl(ds.Tables("AmbilGenerate").Rows(i).Item("DiscPersen1"))) & ","
            strsql &= FixKoma(NullToDbl(ds.Tables("AmbilGenerate").Rows(i).Item("DiscPersen2"))) & ","
            strsql &= FixKoma(NullToDbl(ds.Tables("AmbilGenerate").Rows(i).Item("DiscPersen3"))) & ","
            strsql &= FixKoma(NullToDbl(ds.Tables("AmbilGenerate").Rows(i).Item("Disc1"))) & ","
            strsql &= FixKoma(NullToDbl(ds.Tables("AmbilGenerate").Rows(i).Item("JumlahBeli"))) & ","
            strsql &= "'" & "" & "',"
            strsql &= NullToLong(DefIDWilayah) & ","
            strsql &= FixKoma(NullToDbl(ds.Tables("AmbilGenerate").Rows(i).Item("Konversi"))) & ","
            strsql &= FixKoma(NullToDbl(ds.Tables("AmbilGenerate").Rows(i).Item("HargaPcs"))) & ","
            strsql &= FixKoma(NullToDbl(ds.Tables("AmbilGenerate").Rows(i).Item("HargaJualPcs")))
            strsql &= ")"
            EksekusiSQL(strsql)
            ProgressBarControl1.Position = (i + 1) / JumlahData * 100
            Application.DoEvents()
        Next
        DialogResult = Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub

    Private Sub txtDiscProsen1_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtDiscProsen1.EditValueChanged

    End Sub

    Private Sub txtDiscProsen1_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtDiscProsen1.KeyDown
        Try
            If e.KeyCode = Keys.Enter Then
                For i As Integer = 0 To GV1.RowCount - 1
                    If NullToDbl(GV1.GetRowCellValue(i, "Qty")) <> 0 Then
                        GV1.ClearSelection()
                        GV1.FocusedRowHandle = i
                        GV1.SelectRow(GV1.FocusedRowHandle)
                        GV1.SetRowCellValue(i, "DiscPersen1", NullToDbl(txtDiscProsen1.EditValue))
                        Application.DoEvents()
                    End If
                Next
            End If
        Catch ex As Exception
            XtraMessageBox.Show("Info Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
        End Try
    End Sub
End Class