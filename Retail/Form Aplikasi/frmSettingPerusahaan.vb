Public Class frmSettingPerusahaan 
    Dim SQL As String
    Private Sub SimpleButton6_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SimpleButton6.Click
        DialogResult = Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub SimpleButton9_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SimpleButton9.Click
        Try
            If System.IO.Directory.Exists(txtPathImage.Text) Then
                SQL = "DELETE FROM MSetting"
                EksekusiSQL(SQL)
                SQL = "INSERT INTO MSetting ([NoID],[NamaPerusahaan],[AlamatPerusahaan],[NPWP],[IDDepartemenPusat],[BungaPerforma],[DigitDibelakangKoma],[IsGudangBSDiRetur],[IsStockPerJenis],[IDServerAsal],[IDGudangInTransit],[PathImage],[PathLayouts],[UpdateHargaBeli],[PanjangKodeBarang],[TypeBarcode],[TypeKodeBarang],[IsStockReturPerSupplier],[IsStockPerSupplier],[FormatFP],[TglDitetapkanSO]) VALUES ("
                SQL &= " 1, '" & FixApostropi(txtNamaPerusahaan.Text) & "','" & FixApostropi(txtAlamatPerusahaan.Text) & "','" & FixApostropi(txtNPWP.Text) & "',0,0,2," & rbReturGudang.EditValue & "," & rbJenisBarang.EditValue & "," & FixKoma(txtNoServer.EditValue) & "," & NullToLong(txtGudang.EditValue) & ",'" & FixApostropi(txtPathImage.Text) & "','" & FixApostropi(txtPathLayouts.Text) & "'," & IIf(ckUpdateHargaBeli.Checked, 1, 0) & "," & NullTolInt(txtPanjangKode.Text) & "," & txtTypeBarcode.SelectedIndex & "," & txtTypeKodeBarang.SelectedIndex & "," & IIf(ckStokReturPerSupplier.Checked, 1, 0) & "," & IIf(ckBarangPerSupplier.Checked, 1, 0) & ",'" & FixApostropi(txtNoFakturPajak.Text) & "','" & txtTglDitetapkanSO.DateTime.ToString("yyyy-MM-dd") & "')"
                EksekusiSQL(SQL)
                SimpanDepartemen()
                SimpanMataUang()
                If System.IO.File.Exists(txtPathLayouts.Text) Then
                    FolderLayouts = txtPathLayouts.Text
                Else
                    FolderLayouts = Application.StartupPath & "\System\Layouts\"
                End If
                If System.IO.File.Exists(txtPathImage.Text) Then
                    FolderFoto = txtPathImage.Text
                Else
                    FolderFoto = Application.StartupPath & "\System\PathFoto\"
                End If
                PanjangKodeBarang = NullToLong(txtPanjangKode.EditValue)
                TypeBarcode = txtTypeBarcode.SelectedIndex
                TypeKodeBarang = txtTypeKodeBarang.SelectedIndex
                Close()
            Else
                DevExpress.XtraEditors.XtraMessageBox.Show("File image tidak ditemukan.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
                txtPathImage.Focus()
            End If
        Catch ex As Exception
            DevExpress.XtraEditors.XtraMessageBox.Show("Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub SimpanDepartemen()
        Try
            EksekusiSQL("DELETE FROM MDepartemen")
            SQL = "INSERT INTO [MDepartemen] ([NoID],[Kode],[Nama],[Alamat],[IsAktif],[IsDefault],[Islabarugi],[IsNeraca]) VALUES " & vbCrLf & _
                  "(1, '" & FixApostropi(txtNamaPerusahaan.Text.Trim.Substring(0, 5)) & "','" & FixApostropi(txtNamaPerusahaan.Text) & "','" & FixApostropi(txtAlamatPerusahaan.Text) & "',1,1,1,1)"
            EksekusiSQL(SQL)
        Catch ex As Exception

        End Try
    End Sub
    Private Sub SimpanMataUang()
        Try
            EksekusiSQL("DELETE FROM MMataUang")
            SQL = "INSERT INTO [MMataUang] ([ID],[Kode],[Nama],[Simbol],[IsStandart],[NilaiTukar],[IsBalancing],[KursTukar],[Kurs]) VALUES (" & vbCrLf & _
                  "1, 'IDR','RUPIAH INDONESIA','RP',1,1,1,1,1)"
            EksekusiSQL(SQL)
        Catch ex As Exception

        End Try
    End Sub
    Private Sub frmSettingPerusahaan_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        Try
            LayoutControl1.SaveLayoutToXml(FolderLayouts & "\" & Me.Name & Me.LayoutControl1.Name & ".xml")
        Catch ex As Exception

        End Try
    End Sub

    Private Sub frmSettingPerusahaan_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim ds As New DataSet
        Try
            ds = ExecuteDataset("MGudang", "SELECT MGudang.NoID, MGudang.Kode, IsNull(MGudang.Nama,'') + ' (' + IsNull(MWilayah.Nama,'') + ')' AS Nama, MWilayah.Nama AS Wilayah FROM MGudang LEFT JOIN MWilayah ON MWilayah.NoID=MGudang.IDWilayah WHERE MGudang.IsActive=1 AND MGudang.InTransit=1")
            If Not ds Is Nothing Then
                txtGudang.Properties.DataSource = ds.Tables("MGudang")
                txtGudang.Properties.DisplayMember = "Nama"
                txtGudang.Properties.ValueMember = "NoID"
            End If
            SQL = "SELECT * FROM MSetting"
            ds = ExecuteDataset("MSetting", SQL)
            If ds.Tables(0).Rows.Count >= 1 Then
                txtAlamatPerusahaan.Text = NullTostr(ds.Tables(0).Rows(0).Item("AlamatPerusahaan"))
                txtNamaPerusahaan.Text = NullTostr(ds.Tables(0).Rows(0).Item("NamaPerusahaan"))
                txtNPWP.Text = NullToStr(ds.Tables(0).Rows(0).Item("NPWP"))
                rbReturGudang.EditValue = IIf(NullToBool(ds.Tables(0).Rows(0).Item("IsGudangBSDiRetur")), "1", "0")
                rbJenisBarang.EditValue = IIf(NullTobool(ds.Tables(0).Rows(0).Item("IsStockPerJenis")), "1", "0")
                txtNoServer.EditValue = NullTolong(ds.Tables(0).Rows(0).Item("IDServerAsal"))
                txtGudang.EditValue = NullTolong(ds.Tables(0).Rows(0).Item("IDGudangInTransit"))
                txtPathImage.Text = NullToStr(ds.Tables(0).Rows(0).Item("PathImage"))
                txtNoFakturPajak.Text = NullToStr(ds.Tables(0).Rows(0).Item("FormatFP"))
                txtPathLayouts.Text = NullToStr(ds.Tables(0).Rows(0).Item("PathLayouts"))
                ckUpdateHargaBeli.Checked = NullToBool(ds.Tables(0).Rows(0).Item("UpdateHargaBeli"))
                ckStokReturPerSupplier.Checked = NullToBool(ds.Tables(0).Rows(0).Item("IsStockReturPerSupplier"))
                ckBarangPerSupplier.Checked = NullToBool(ds.Tables(0).Rows(0).Item("IsStockPerSupplier"))
                txtPanjangKode.Text = NullTolInt(ds.Tables(0).Rows(0).Item("PanjangKodeBarang"))
                txtTypeBarcode.SelectedIndex = NullTolInt(ds.Tables(0).Rows(0).Item("TypeBarcode"))
                txtTypeKodeBarang.SelectedIndex = NullTolInt(ds.Tables(0).Rows(0).Item("TypeKodeBarang"))
                txtTglDitetapkanSO.DateTime = NullToDate(ds.Tables(0).Rows(0).Item("TglDitetapkanSO"))
            End If
            If System.IO.File.Exists(FolderLayouts & "\" & Me.Name & Me.LayoutControl1.Name & ".xml") Then
                LayoutControl1.RestoreLayoutFromXml(FolderLayouts & "\" & Me.Name & Me.LayoutControl1.Name & ".xml")
            End If
            FungsiControl.SetForm(Me)
        Catch ex As Exception
            DevExpress.XtraEditors.XtraMessageBox.Show("Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            ds.Dispose()
        End Try
    End Sub

    Private Sub txtPathImage_ButtonClick(ByVal sender As Object, ByVal e As DevExpress.XtraEditors.Controls.ButtonPressedEventArgs) Handles txtPathImage.ButtonClick
        If e.Button.Index = 0 Then
            Dim x As New FolderBrowserDialog
            x.ShowNewFolderButton = True
            If x.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
                txtPathImage.Text = x.SelectedPath
            End If
            x.Dispose()
        ElseIf e.Button.Index = 1 Then
            txtPathImage.Text = ""
        End If
    End Sub

    Private Sub txtPathLayouts_ButtonClick(ByVal sender As Object, ByVal e As DevExpress.XtraEditors.Controls.ButtonPressedEventArgs) Handles txtPathLayouts.ButtonClick
        If e.Button.Index = 0 Then
            Dim x As New FolderBrowserDialog
            x.ShowNewFolderButton = True
            If x.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
                txtPathLayouts.Text = x.SelectedPath
            End If
            x.Dispose()
        ElseIf e.Button.Index = 1 Then
            txtPathLayouts.Text = ""
        End If
    End Sub
End Class