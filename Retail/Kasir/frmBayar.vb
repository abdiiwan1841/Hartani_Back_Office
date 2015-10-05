Imports DevExpress.XtraEditors

Public Class frmBayar

    Private Sub txtTunai_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtTunai.DoubleClick
        txtTunai.EditValue = txtSubtotal.EditValue
    End Sub
    'Public IDCC As Long = -1
    'Public IDDC As Long = -1
    'Public NoCC As String = ""
    'Public NoDC As String = ""
    'Public CC As Double = 0.0
    'Public DC As Double = 0.0
    'Public Tunai As Double = 0.0
    'Public Kembalian As Double = 0.0

    Private Sub txtTunai_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtTunai.EditValueChanged
        HitungTotal()
    End Sub

    Private Sub HitungTotal()
        Dim Sisa As Double = 0.0
        Dim Total As Double = 0.0
        Try
            Total = (txtSubtotal.EditValue + txtRounding.EditValue)
            Sisa = Total - txtTunai.EditValue
            If txtBankCC.Text <> "" AndAlso txtBankDC.Text = "" Then
                If Sisa > 0 Then
                    txtCC.EditValue = Sisa
                    txtChargeCC.EditValue = NullToDbl(EksekusiSQlSkalarNew("SELECT ChargeMember FROM MJenisKartu WHERE NoID=" & NullToLong(txtBankCC.EditValue)))
                    txtBK1.EditValue = txtCC.EditValue * (1 + (txtChargeCC.EditValue / 100))
                Else
                    txtCC.EditValue = 0
                    txtChargeCC.EditValue = 0
                    txtBK1.EditValue = 0
                End If
                txtDC.EditValue = 0 : txtChargeDC.EditValue = 0 : txtBK2.EditValue = 0
            ElseIf txtBankDC.Text <> "" AndAlso txtBankCC.Text = "" Then
                If Sisa > 0 Then
                    txtDC.EditValue = Sisa
                    txtChargeDC.EditValue = NullToDbl(EksekusiSQlSkalarNew("SELECT ChargeMember FROM MJenisKartu WHERE NoID=" & NullToLong(txtBankDC.EditValue)))
                    txtBK2.EditValue = txtDC.EditValue * (1 + (txtChargeDC.EditValue / 100))
                Else
                    txtDC.EditValue = 0
                    txtChargeDC.EditValue = 0
                    txtBK2.EditValue = 0
                End If
                txtCC.EditValue = 0 : txtChargeCC.EditValue = 0 : txtBK1.EditValue = 0
            ElseIf txtBankCC.Text <> "" AndAlso txtBankDC.Text <> "" Then
                If Sisa > 0 Then
                    txtCC.EditValue = Sisa / 2
                    txtChargeCC.EditValue = NullToDbl(EksekusiSQlSkalarNew("SELECT ChargeMember FROM MJenisKartu WHERE NoID=" & NullToLong(txtBankCC.EditValue)))
                    txtDC.EditValue = Sisa - txtCC.EditValue
                    txtChargeDC.EditValue = NullToDbl(EksekusiSQlSkalarNew("SELECT ChargeMember FROM MJenisKartu WHERE NoID=" & NullToLong(txtBankDC.EditValue)))
                    txtBK1.EditValue = txtCC.EditValue * (1 + (txtChargeCC.EditValue / 100))
                    txtBK2.EditValue = txtDC.EditValue * (1 + (txtChargeDC.EditValue / 100))
                Else
                    txtCC.EditValue = 0
                    txtChargeCC.EditValue = 0
                    txtDC.EditValue = 0
                    txtChargeDC.EditValue = 0
                    txtBK1.EditValue = 0
                    txtBK2.EditValue = 0
                End If
            Else
                txtDC.EditValue = 0
                txtCC.EditValue = 0
                txtChargeCC.EditValue = 0
                txtChargeDC.EditValue = 0
                txtBK1.EditValue = 0
                txtBK2.EditValue = 0
            End If
            txtKembali.EditValue = (txtTunai.EditValue + txtCC.EditValue + txtDC.EditValue) - Total
        Catch ex As Exception

        End Try
    End Sub

    Private Sub txtSubtotal_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtSubtotal.EditValueChanged
        HitungTotal()
    End Sub

    Private Sub txtCC_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtCC.EditValueChanged
        HitungTotal()
    End Sub

    Private Sub txtBankCC_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtBankCC.EditValueChanged
        HitungTotal()
    End Sub

    Private Sub txtBankDC_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtBankDC.EditValueChanged
        HitungTotal()
    End Sub

    Private Sub txtNoCC_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtNoCC.EditValueChanged
        HitungTotal()
    End Sub

    Private Sub txtNoDC_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtNoDC.EditValueChanged
        HitungTotal()
    End Sub

    Private Sub txtDC_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtDC.EditValueChanged
        HitungTotal()
    End Sub

    Private Sub frmBayar_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        RefreshLookUp()
        FungsiControl.SetForm(Me)
        txtTunai.Focus()
    End Sub

    Private Sub RefreshLookUp()
        Dim DS As New DataSet
        Try
            DS = ExecuteDataset("MCC", "SELECT NoID, Kode, Nama, Charge, ChargeMember FROM MJenisKartu WHERE IsActive=1 AND IsKartuKredit=1")
            txtBankCC.Properties.DataSource = DS.Tables("MCC")
            txtBankCC.Properties.ValueMember = "NoID"
            txtBankCC.Properties.DisplayMember = "Kode"

            DS = ExecuteDataset("MDC", "SELECT NoID, Kode, Nama, Charge, ChargeMember FROM MJenisKartu WHERE IsActive=1 AND IsKartuDebet=1")
            txtBankDC.Properties.DataSource = DS.Tables("MDC")
            txtBankDC.Properties.ValueMember = "NoID"
            txtBankDC.Properties.DisplayMember = "Kode"
        Catch ex As Exception

        End Try
    End Sub

    Private Sub mnSaveLayOuts_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles mnSaveLayOuts.ItemClick
        Dim xOtorisasi As New frmOtorisasiAdmin
        Try
            If XtraMessageBox.Show("Simpan Layout?", NamaAplikasi, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = Windows.Forms.DialogResult.Yes AndAlso xOtorisasi.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
                Ini.TulisIniPath(FolderLayouts & Me.Name & ".ini", "Form", "Width", Me.Width)
                Ini.TulisIniPath(FolderLayouts & Me.Name & ".ini", "Form", "Height", Me.Height)
                Me.Left = (Screen.PrimaryScreen.WorkingArea.Width - Me.Width) \ 2
                Me.Top = (Screen.PrimaryScreen.WorkingArea.Height - Me.Height) \ 2

                gvBankCC.SaveLayoutToXml(FolderLayouts & Me.Name & gvBankCC.Name & ".xml")
                gvBankDC.SaveLayoutToXml(FolderLayouts & Me.Name & gvBankDC.Name & ".xml")
            End If
        Catch ex As Exception
            XtraMessageBox.Show("Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            xOtorisasi.Dispose()
        End Try
    End Sub

    Private Sub gvBankCC_DataSourceChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvBankCC.DataSourceChanged
        If System.IO.File.Exists(FolderLayouts & Me.Name & sender.Name & ".xml") Then
            sender.RestoreLayoutFromXml(FolderLayouts & Me.Name & sender.Name & ".xml")
        End If
        With sender
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

    Private Sub gvBankDC_DataSourceChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvBankDC.DataSourceChanged
        If System.IO.File.Exists(FolderLayouts & Me.Name & sender.Name & ".xml") Then
            sender.RestoreLayoutFromXml(FolderLayouts & Me.Name & sender.Name & ".xml")
        End If
        With sender
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

    Private Sub cmdCancel_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles cmdCancel.ItemClick
        DialogResult = Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub cmdSimpan_ItemClick(ByVal sender As Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles cmdSimpan.ItemClick
        Try
            HitungTotal()
            If IsValidasi() Then
                'IDCC = NullToLong(txtBankCC.EditValue)
                'IDDC = NullToLong(txtBankDC.EditValue)
                'NoCC = NullToStr(txtNoCC.Text)
                'NoDC = NullToStr(txtNoDC.Text)
                'CC = NullToDbl(txtCC.EditValue)
                'DC = NullToDbl(txtDC.EditValue)
                'Tunai = NullToDbl(txtTunai.EditValue)
                'Kembalian = NullToDbl(txtKembali.EditValue)

                DialogResult = Windows.Forms.DialogResult.OK
                Me.Close()
            End If
        Catch ex As Exception

        End Try
    End Sub

    Private Function IsValidasi() As Boolean
        If txtBankCC.Text <> "" AndAlso txtBankDC.EditValue <> "" Then
            XtraMessageBox.Show("Pilih salah satu, kartu debet atau kartu kredit.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
            Return False
            Exit Function
        End If
        If txtBankCC.Text <> "" AndAlso txtCC.EditValue <= 0 Then
            XtraMessageBox.Show("Jumlah kartu kredit masih 0.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
            Return False
            Exit Function
        End If
        If txtBankDC.Text <> "" AndAlso txtDC.EditValue <= 0 Then
            XtraMessageBox.Show("Jumlah kartu debet masih 0.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
            Return False
            Exit Function
        End If
        If txtKembali.EditValue < 0 Then
            XtraMessageBox.Show("Jumlah Masih belum sesuai.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
            Return False
            Exit Function
        End If
        If txtBankCC.Text <> "" AndAlso txtNoCC.Text.Trim = "" Then
            XtraMessageBox.Show("No Kartu Kredit Masih Kosong.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
            Return False
            Exit Function
        End If
        If txtBankDC.Text <> "" AndAlso txtNoDC.Text.Trim = "" Then
            XtraMessageBox.Show("No Kartu Debet Masih Kosong.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
            Return False
            Exit Function
        End If
        If txtSubtotal.EditValue < 0 Then
            Dim x As New frmOtorisasiAdmin
            If Not x.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
                x.Dispose()
                Return False
                Exit Function
            Else
                x.Dispose()
            End If
        End If
        Return True
    End Function

    Private Sub txtKembali_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtKembali.EditValueChanged

    End Sub

    Private Sub txtRounding_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtRounding.EditValueChanged
        HitungTotal()
    End Sub
End Class