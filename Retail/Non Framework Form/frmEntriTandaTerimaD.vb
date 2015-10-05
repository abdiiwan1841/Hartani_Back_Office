Imports System.Data
Imports System.Data.SqlClient
Imports DevExpress.XtraEditors
Imports DevExpress.Utils
Imports System.IO.File

Public Class frmEntriTandaTerimaD
    Public IsNew As Boolean = True
    Public IsPosted As Boolean = False
    Public NoID As Long = -1
    Public IDTandaTerima As Long = -1
    Public IDSupplier As Long = -1
    Public FormPemanggil As frmEntriTandaTerima
    Public IsFastEntri As Boolean = False

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
            SQL = "SELECT MReturBeli.NoID, MReturBeli.Kode, MReturBeli.Tanggal, MReturBeli.Total FROM MReturBeli WHERE MReturBeli.IsPosted=1 AND MReturBeli.IDSupplier=" & IDSupplier & " " & IIf(FormPemanggil.IsRevisi, "", " AND IsNull(MReturBeli.IsTT,0)=0 ") & " AND MReturBeli.NoID NOT IN (SELECT MTTD.IDRetur FROM MTTD WHERE MTTD.NoID<>" & NoID & " AND MTTD.IDTT=" & IDTandaTerima & ")"
            ds = ExecuteDataset("MReturBeli", SQL)
            txtRetur.Properties.DataSource = ds.Tables("MReturBeli")
            txtRetur.Properties.ValueMember = "NoID"
            txtRetur.Properties.DisplayMember = "Kode"

            SQL = "SELECT MBeli.NoID, MBeli.Kode, MBeli.Tanggal, MBeli.Subtotal AS Total FROM MBeli WHERE MBeli.IsPosted=1 AND MBeli.IDSupplier=" & IDSupplier & " " & IIf(FormPemanggil.IsRevisi, "", " AND IsNull(MBeli.IsTT,0)=0 ") & " AND MBeli.NoID NOT IN (SELECT MTTD.IDTransaksi FROM MTTD WHERE MTTD.NoID<>" & NoID & " AND MTTD.IDTT=" & IDTandaTerima & ")"
            ds = ExecuteDataset("MBarang", SQL)
            txtBeli.Properties.DataSource = ds.Tables("MBarang")
            txtBeli.Properties.ValueMember = "NoID"
            txtBeli.Properties.DisplayMember = "Kode"

            SQL = "SELECT MPO.NoID, MPO.Kode NoSPP, MBeli.Kode AS NoBPB, MBeli.Tanggal, MPO.Total " & _
                  " FROM MPO " & _
                  " INNER JOIN MPOD ON MPOD.IDPO=MPO.NoID " & _
                  " LEFT JOIN (MBeli INNER JOIN MBeliD ON MBeli.NoID=MBeliD.IDBeli) ON MPOD.NoID=MBeliD.IDPOD WHERE MPO.IsPosted=1 AND MBeli.IsPosted=1 AND MPO.IDSupplier=" & IDSupplier & " " & IIf(FormPemanggil.IsRevisi, "", " AND IsNull(MPO.IsTT,0)=0 ") & " AND MPO.NoID NOT IN (SELECT MTTD.IDPO FROM MTTD WHERE MTTD.NoID<>" & NoID & " AND MTTD.IDTT=" & IDTandaTerima & ")" & vbCrLf & _
                  " GROUP BY MPO.NoID, MPO.Kode, MBeli.Kode, MBeli.Tanggal, MPO.Total"
            ds = ExecuteDataset("MPO", SQL)
            txtSPP.Properties.DataSource = ds.Tables("MPO")
            txtSPP.Properties.ValueMember = "NoID"
            txtSPP.Properties.DisplayMember = "NoSPP"

        Catch ex As Exception
            XtraMessageBox.Show("Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            ds.Dispose()
        End Try
    End Sub
    Private Sub LoadData()
        Try
            SQL = "SELECT MTTD.*, MBeli.Kode AS KodeBeli, MReturBeli.Kode AS KodeRetur " & vbCrLf & _
                  " FROM MTTD LEFT JOIN MBeli ON MBeli.NoID=MTTD.IDTransaksi LEFT JOIN MReturBeli ON MReturBeli.NoID=MTTD.IDRetur " & vbCrLf & _
                  " WHERE MTTD.NoID= " & NoID
            oDS = New DataSet()
            oDS = ExecuteDataset("MTTD", SQL)
            If oDS.Tables("MTTD").Rows.Count >= 1 Then
                With oDS.Tables("MTTD").Rows(0)
                    txtBeli.EditValue = NullToLong(.Item("IDTransaksi"))
                    txtSPP.EditValue = NullToLong(.Item("IDPO"))
                    CheckEdit1.Checked = NullToBool(.Item("IsAdaFP"))
                    txtKodeReff.EditValue = NullToStr(.Item("KodeReff"))
                    txtNoFaktur.EditValue = NullToStr(.Item("NoFaktur"))
                    txtRetur.EditValue = NullToLong(.Item("IDRetur"))
                    txtJumlahBeli.EditValue = NullToDbl(.Item("Kredit"))
                    txtJumlahRetur.EditValue = NullToDbl(.Item("JumlahRetur"))
                    TextEdit1.EditValue = NullToDbl(.Item("JumlahFaktur"))
                    txtSisa.EditValue = NullToDbl(.Item("Sisa"))
                    txtCatatan.Text = NullToStr(.Item("Keterangan"))
                    tglFaktur.EditValue=.Item("TanggalTransaksi")
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
                NoID = GetNewID("MTTD", "NoID")
                SQL = "INSERT INTO [MTTD] ([NoID],[IDTT],[IDTransaksi],[TanggalTransaksi],[KodeReff],[IDJenisTransaksi],[Debet],[Kredit],[Keterangan],[IsTrue],[IDRetur],[JumlahRetur],[Sisa],[NoFaktur],JumlahFaktur,IDPO,IsAdaFP) VALUES (" & vbCrLf & _
                      NoID & ", " & IDTandaTerima & ", " & NullToLong(txtBeli.EditValue) & ", " & _
                      "'" & Format(tglFaktur.DateTime, "yyyy/MM/dd") & "', " & _
                      "'" & FixApostropi(txtKodeReff.Text) & "', 2, " & _
                      "0, " & FixKoma(txtJumlahBeli.EditValue) & ", " & _
                      "'" & FixApostropi(txtCatatan.Text) & "', 1, " & _
                      NullToLong(txtRetur.EditValue) & ", " & _
                      FixKoma(txtJumlahRetur.EditValue) & ", " & _
                      FixKoma(txtSisa.EditValue) & ",'" & FixApostropi(txtNoFaktur.Text) & "'," & FixKoma(NullToDbl(TextEdit1.EditValue)) & "," & NullToLong(txtSPP.EditValue) & "," & IIf(CheckEdit1.Checked, "1", "0") & " )"
            Else
                SQL = "UPDATE [MTTD] SET " & vbCrLf & _
                      " [IDTransaksi]=" & NullToLong(txtBeli.EditValue) & ", " & _
                      " [TanggalTransaksi]='" & Format(tglFaktur.DateTime, "yyyy/MM/dd") & "', " & _
                      " [KodeReff]='" & FixApostropi(txtKodeReff.Text) & "', [IDJenisTransaksi]=2, " & _
                      " [Debet]=0, [Kredit]=" & FixKoma(txtJumlahBeli.EditValue) & ", " & _
                      " [Keterangan]='" & FixApostropi(txtCatatan.Text) & "', [IsTrue]=1, " & _
                      " [IDRetur]=" & NullToLong(txtRetur.EditValue) & ", " & _
                      " [JumlahRetur]=" & FixKoma(txtJumlahRetur.EditValue) & ", " & _
                      " [NoFaktur]='" & FixApostropi(txtNoFaktur.Text) & "'," & _
                      " IDPO=" & NullToLong(txtSPP.EditValue) & "," & _
                      " [JumlahFaktur]=" & FixKoma(TextEdit1.EditValue) & ", " & _
                       " IsAdaFP=" & IIf(CheckEdit1.Checked, "1", "0") & "," & _
                      " [Sisa]=" & FixKoma(txtSisa.EditValue) & _
                      " WHERE NoID=" & NoID
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
        If txtBeli.Text = "" Then
            If XtraMessageBox.Show("No Pembelian masih kosong, Mau dilanjutkan?", NamaAplikasi, MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then

            Else

                txtBeli.Focus()
                Return False
                Exit Function
            End If
        End If
        If txtNoFaktur.Text = "" Then
            If XtraMessageBox.Show("No Faktur masih kosong." & vbCrLf & "Ingin melanjutkan penyimpanan?", NamaAplikasi, MessageBoxButtons.YesNo, MessageBoxIcon.Information, MessageBoxDefaultButton.Button2) = Windows.Forms.DialogResult.No Then
                txtNoFaktur.Focus()
                Return False
                Exit Function
            End If
        End If
        HitungJumlah()
        If txtSisa.EditValue <= 0 Then
            If XtraMessageBox.Show("Nilai Pembelian masih kurang dari atau sama dengan 0." & vbCrLf & "Ingin melanjutkan penyimpanan?", NamaAplikasi, MessageBoxButtons.YesNo, MessageBoxIcon.Information, MessageBoxDefaultButton.Button2) = Windows.Forms.DialogResult.No Then
                txtJumlahBeli.Focus()
                Return False
                Exit Function
            End If
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
                Ini.TulisIniPath(FolderLayouts & Me.Name & ".ini", "Form", "Width", Me.Width)
                Ini.TulisIniPath(FolderLayouts & Me.Name & ".ini", "Form", "Height", Me.Height)
                Me.Left = (Screen.PrimaryScreen.WorkingArea.Width - Me.Width) \ 2
                Me.Top = (Screen.PrimaryScreen.WorkingArea.Height - Me.Height) \ 2

                LayoutControl1.SaveLayoutToXml(FolderLayouts & Me.Name & LayoutControl1.Name & ".xml")
                gvRetur.SaveLayoutToXml(FolderLayouts & Me.Name & gvRetur.Name & ".xml")
                gvBeli.SaveLayoutToXml(FolderLayouts & Me.Name & gvBeli.Name & ".xml")

            End If
        Catch ex As Exception
            XtraMessageBox.Show("Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            xOtorisasi.Dispose()
        End Try
    End Sub

    Private Sub frmEntriLPBD_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        Try
            If DialogResult = Windows.Forms.DialogResult.OK Then
                FormPemanggil.RefreshDetil()
                FormPemanggil.Show()
                FormPemanggil.Focus()
                FormPemanggil.txtBarang.Focus()
            Else
                If IsFastEntri Then
                    EksekusiSQL("DELETE FROM MLPBD WHERE NoID=" & NoID)
                End If
                FormPemanggil.Show()
                FormPemanggil.Focus()
            End If
            Me.Dispose()
        Catch ex As Exception

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
            Me.Width = Ini.BacaIniPath(FolderLayouts & Me.Name & ".ini", "Form", "Width", Me.Width)
            Me.Height = Ini.BacaIniPath(FolderLayouts & Me.Name & ".ini", "Form", "Height", Me.Height)

            Me.Left = (Screen.PrimaryScreen.WorkingArea.Width - Me.Width) \ 2
            Me.Top = (Screen.PrimaryScreen.WorkingArea.Height - Me.Height) \ 2
            If FormPemanggil.IsRevisi Then
                txtRetur.Properties.ReadOnly = False
            Else
                txtRetur.Properties.ReadOnly = False
            End If
            LoadLayout()
            HighLightTxt()
            FungsiControl.SetForm(Me)
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
    Private Sub LoadLayout()
        If Exists(FolderLayouts & Me.Name & LayoutControl1.Name & ".xml") Then
            LayoutControl1.RestoreLayoutFromXml(FolderLayouts & Me.Name & LayoutControl1.Name & ".xml")
        End If
    End Sub
    Private Sub HitungJumlah()
        Try
            txtSisa.EditValue = NullToDbl(txtJumlahBeli.EditValue) - NullToDbl(txtJumlahRetur.EditValue)
        Catch ex As Exception
        End Try
    End Sub

    Private Sub txtQty_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtJumlahBeli.LostFocus
        HitungJumlah()
    End Sub

    'Private Sub RubahReff()
    '    Dim Ds As New DataSet
    '    Try
    '        SQL = "SELECT MBeli.Total,MBeli.IDPO,MBeli.KodeReff FROM MBeli WHERE MBeli.IsPosted=1 AND MBeli.NoID=" & NullToLong(txtBeli.EditValue)
    '        Try
    '            Ds = ExecuteDataset("TabelBeli", SQL)
    '            If Ds.Tables("TabelBeli").Rows.Count >= 1 Then
    '                txtJumlahBeli.EditValue = NullToLong(Ds.Tables(0).Rows(0).Item("Total"))
    '                txtSPP.EditValue = NullToLong(Ds.Tables(0).Rows(0).Item("IDPO"))
    '                txtKodeReff.Text = NullToStr(Ds.Tables(0).Rows(0).Item("KodeReff"))
    '            Else
    '                txtJumlahBeli.EditValue = 0
    '                txtSPP.EditValue = -1
    '                txtKodeReff.Text = ""
    '            End If
    '        Catch ex As Exception

    '        End Try

    '        SQL = "SELECT MReturBeli.NoID, MReturBeli.IDBeli, MReturBeli.Total, MBeli.Total AS TotalBeli FROM MReturBeli  INNER JOIN MBeli ON MBeli.NoID=MReturBeli.IDBeli WHERE MReturBeli.IsPosted=1 AND MReturBeli.IDBeli=" & NullToLong(txtBeli.EditValue)
    '        Try
    '            Ds = ExecuteDataset("Tabel", SQL)
    '            If Ds.Tables("Tabel").Rows.Count >= 1 Then
    '                txtRetur.EditValue = NullToLong(Ds.Tables(0).Rows(0).Item("NoID"))
    '                txtJumlahRetur.EditValue = NullToDbl(Ds.Tables(0).Rows(0).Item("Total"))
    '            Else
    '                txtRetur.EditValue = -1
    '                txtJumlahRetur.EditValue = 0
    '            End If
    '        Catch ex As Exception

    '        End Try
    '        HitungJumlah()
    '    Catch ex As Exception
    '        XtraMessageBox.Show("Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
    '    Finally
    '        Ds.Dispose()
    '    End Try
    'End Sub
    Private Sub RubahSPP()
        Dim Ds As New DataSet
        SQL = "SELECT MPO.Total, MPO.NoID, MPO.Kode, MBeli.NoID AS IDBeli, MBeli.Tanggal " & _
              " FROM MPOD " & _
              " INNER JOIN MPO ON MPO.NoID=MPOD.IDPO " & _
              " LEFT JOIN MBeliD ON MBeliD.IDPOD=MPOD.NoID " & _
              " LEFT JOIN MBeli ON MBeli.NoID=MBeliD.IDBeli " & _
              " WHERE MPO.IsPosted=1 AND MPO.NoID=" & NullToLong(txtSPP.EditValue)
        Try
            Ds = ExecuteDataset("TabelBeli", SQL)
            If Ds.Tables("TabelBeli").Rows.Count >= 1 Then
                txtBeli.EditValue = NullToLong(Ds.Tables(0).Rows(0).Item("IDBeli"))
                txtJumlahBeli.EditValue = NullToLong(Ds.Tables(0).Rows(0).Item("Total"))
                txtSPP.EditValue = NullToLong(Ds.Tables(0).Rows(0).Item("NoID"))
                txtKodeReff.Text = NullToStr(Ds.Tables(0).Rows(0).Item("Kode"))
                tglFaktur.EditValue = Ds.Tables(0).Rows(0).Item("Tanggal")
                'txtBeli.EditValue = NullToLong(Ds.Tables(0).Rows(0).Item("IDBeli"))
            Else
                txtJumlahBeli.EditValue = 0
                tglFaktur.EditValue = Chr(0)
                'txtSPP.EditValue = -1
                txtKodeReff.Text = ""
                txtBeli.EditValue = -1
            End If

            SQL = "SELECT MBeli.SubTotal, MBeli.IDPO, MBeli.KodeReff FROM MBeli WHERE MBeli.IsPosted=1 AND MBeli.NoID=" & NullToLong(txtBeli.EditValue)
            Ds = ExecuteDataset("TabelBeli", SQL)
            If Ds.Tables("TabelBeli").Rows.Count >= 1 Then
                txtJumlahBeli.EditValue = NullToLong(Ds.Tables(0).Rows(0).Item("SubTotal"))
                'txtSPP.EditValue = NullToLong(Ds.Tables(0).Rows(0).Item("IDPO"))
                txtKodeReff.Text = NullToStr(Ds.Tables(0).Rows(0).Item("KodeReff"))
            Else
                'txtJumlahBeli.EditValue = 0
                'txtSPP.EditValue = -1
                'txtKodeReff.Text = ""
            End If

            SQL = "SELECT MReturBeli.NoID, MReturBeli.IDBeli, MReturBeli.Total, MBeli.Total AS TotalBeli FROM MReturBeli  INNER JOIN MBeli ON MBeli.NoID=MReturBeli.IDBeli WHERE MReturBeli.IsPosted=1 AND MReturBeli.IDBeli=" & NullToLong(txtBeli.EditValue)
            Ds = ExecuteDataset("Tabel", SQL)
            If Ds.Tables("Tabel").Rows.Count >= 1 Then
                txtRetur.EditValue = NullToLong(Ds.Tables(0).Rows(0).Item("NoID"))
                txtJumlahRetur.EditValue = NullToDbl(Ds.Tables(0).Rows(0).Item("Total"))
            Else
                txtRetur.EditValue = -1
                txtJumlahRetur.EditValue = 0
            End If
            HitungJumlah()
        Catch ex As Exception
            XtraMessageBox.Show("Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            Ds.Dispose()
        End Try
    End Sub
    Private Sub BarButtonItem1_ItemClick_1(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles mnSave.ItemClick
        cmdSave.PerformClick()
    End Sub

    Private Sub BarButtonItem2_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem2.ItemClick
        cmdTutup.PerformClick()
    End Sub

    Private Sub gvGudang_DataSourceChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvRetur.DataSourceChanged
        If System.IO.File.Exists(FolderLayouts & Me.Name & gvRetur.Name & ".xml") Then
            gvRetur.RestoreLayoutFromXml(FolderLayouts & Me.Name & gvRetur.Name & ".xml")
        End If
        With gvRetur
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

    Private Sub gvPO_DataSourceChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvBeli.DataSourceChanged
        If System.IO.File.Exists(FolderLayouts & Me.Name & gvBeli.Name & ".xml") Then
            gvBeli.RestoreLayoutFromXml(FolderLayouts & Me.Name & gvBeli.Name & ".xml")
        End If
        With gvBeli
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

    Private Sub txtKonversi_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtJumlahRetur.LostFocus
        HitungJumlah()
    End Sub

    Private Sub txtQty_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtJumlahBeli.EditValueChanged
        HitungJumlah()
    End Sub

    Private Sub txtRetur_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtRetur.EditValueChanged
        Dim DS As New DataSet
        Try
            If Not FormPemanggil Is Nothing AndAlso FormPemanggil.IsRevisi Then
                SQL = "SELECT MReturBeli.NoID, MReturBeli.IDBeli, MReturBeli.Total, MBeli.Total AS TotalBeli FROM MReturBeli INNER JOIN MBeli ON MBeli.NoID=MReturBeli.IDBeli WHERE MReturBeli.IsPosted=1 AND MReturBeli.NoID=" & NullToLong(txtRetur.EditValue)
                DS = ExecuteDataset("Tabel", SQL)
                If DS.Tables("Tabel").Rows.Count >= 1 Then
                    'txtRetur.EditValue = NullToLong(DS.Tables(0).Rows(0).Item("NoID"))
                    txtJumlahRetur.EditValue = NullToDbl(DS.Tables(0).Rows(0).Item("Total"))
                Else
                    'txtRetur.EditValue = -1
                    txtJumlahRetur.EditValue = 0
                End If
                HitungJumlah()
            End If
        Catch ex As Exception
            XtraMessageBox.Show("Info Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
        Finally
            DS.Dispose()
        End Try
    End Sub

    Private Sub txtBeli_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtBeli.EditValueChanged
        
    End Sub

    Private Sub txtSPP_EditValueChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtSPP.EditValueChanged
        RubahSPP()
    End Sub

    Private Sub txtSPP_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtSPP.KeyDown
        
    End Sub

    Private Sub txtBeli_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtBeli.KeyDown
        Dim ds As New DataSet
        Try
            If e.KeyCode = Keys.Enter AndAlso Not FormPemanggil Is Nothing AndAlso FormPemanggil.IsRevisi Then
                SQL = "SELECT MReturBeli.NoID, MReturBeli.IDBeli, MReturBeli.Total, MBeli.Total AS TotalBeli FROM MReturBeli  INNER JOIN MBeli ON MBeli.NoID=MReturBeli.IDBeli WHERE MReturBeli.IsPosted=1 AND MReturBeli.IDBeli=" & NullToLong(txtBeli.EditValue)
                ds = ExecuteDataset("Tabel", SQL)
                If ds.Tables("Tabel").Rows.Count >= 1 Then
                    txtRetur.EditValue = NullToLong(ds.Tables(0).Rows(0).Item("NoID"))
                    txtJumlahRetur.EditValue = NullToDbl(ds.Tables(0).Rows(0).Item("Total"))
                Else
                    txtRetur.EditValue = -1
                    txtJumlahRetur.EditValue = 0
                End If
                HitungJumlah()
            End If
        Catch ex As Exception
            XtraMessageBox.Show("Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            ds.Dispose()
        End Try
    End Sub

    Private Sub GridView1_DataSourceChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles GridView1.DataSourceChanged
        If System.IO.File.Exists(FolderLayouts & Me.Name & GridView1.Name & ".xml") Then
            GridView1.RestoreLayoutFromXml(FolderLayouts & Me.Name & GridView1.Name & ".xml")
        End If
        With GridView1
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
End Class