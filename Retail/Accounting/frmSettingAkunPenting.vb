Imports DevExpress.XtraEditors
Imports System.Data
Imports System.Data.SqlClient

Public Class frmSettingAkunPenting
    Dim SQL As String
    Dim ds As New DataSet
    Private Sub SimpleButton6_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SimpleButton6.Click
        DialogResult = Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub SimpleButton9_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SimpleButton9.Click
        Try

            SQL = "DELETE FROM MSettingAkun"
            EksekusiSQL(SQL)

            SQL = "INSERT INTO [MSettingAkun] ([IDAkunSelisihBalancing],[IDAkunLRBerjalan]) VALUES (" & vbCrLf & _
                  NullToLong(txtAkunBalancing.EditValue) & ", " & NullToLong(txtAkunLRBerjalan.EditValue) & ")"
            If EksekusiSQL(SQL) >= 1 Then
                SQL = "UPDATE MSettingAkun SET " & vbCrLf & _
                      " IDAkunPembelian=" & NullToLong(txtAkunPembelian.EditValue) & "," & vbCrLf & _
                      " IDAkunKasPembelian=" & NullToLong(txtAkunKasPembelian.EditValue) & "," & vbCrLf & _
                      " IDAkunBiayaPembelian=" & NullToLong(txtAkunBiayaPembelian.EditValue) & "," & vbCrLf & _
                      " IDAkunReturPembelian=" & NullToLong(txtAkunReturPembelian.EditValue) & "," & vbCrLf & _
                      " IDAkunKasReturPembelian=" & NullToLong(txtAkunKasReturPembelian.EditValue) & "," & vbCrLf & _
                      " IDAkunBiayaReturPembelian=" & NullToLong(txtAkunBiayaReturPembelian.EditValue) & "," & vbCrLf & _
                      " IDAkunPenjualan=" & NullToLong(txtAkunPenjualan.EditValue) & "," & vbCrLf & _
                      " IDAkunKasPenjualan=" & NullToLong(txtAkunKasPenjualan.EditValue) & "," & vbCrLf & _
                      " IDAkunPotonganPenjualan=" & NullToLong(txtAkunPotonganPenjualan.EditValue) & "," & vbCrLf & _
                      " IDAkunBiayaPenjualan=" & NullToLong(txtAkunBiayaPenjualan.EditValue) & "," & vbCrLf & _
                      " IDAkunReturPenjualan=" & NullToLong(txtAkunReturPenjualan.EditValue) & "," & vbCrLf & _
                      " IDAkunKasReturPenjualan=" & NullToLong(txtAkunKasReturPenjualan.EditValue) & "," & vbCrLf & _
                      " IDAkunBiayaReturPenjualan=" & NullToLong(txtAkunBiayaReturPenjualan.EditValue)
                EksekusiSQL(SQL)
                AmbilSettingAccounting()
                Close()
            End If
        Catch ex As Exception
            DevExpress.XtraEditors.XtraMessageBox.Show("Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub frmSettingPerusahaan_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        Try
            LayoutControl1.SaveLayoutToXml(FolderLayouts & "\" & Me.Name & Me.LayoutControl1.Name & ".xml")
        Catch ex As Exception

        End Try
    End Sub

    Private Sub frmSettingPerusahaan_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        RefreshLookUp()
        cmdRefresh.PerformClick()
        If System.IO.File.Exists(FolderLayouts & "\" & Me.Name & Me.LayoutControl1.Name & ".xml") Then
            LayoutControl1.RestoreLayoutFromXml(FolderLayouts & "\" & Me.Name & Me.LayoutControl1.Name & ".xml")
        End If
    End Sub

    Private Sub RefreshLookUp()
        Try
            SQL = "SELECT MAkun.ID, MAkun.Kode+' - '+MAkun.Nama AS Perkiraan, MSubklasAkun.Nama as SubKlasifikasi, MKlasAkun.Nama as Klasifikasi,MKlasAkun.IsDebet " & _
                  " FROM ((MAkun LEFT JOIN MSubKlasAkun On MAkun.IDSubklasAkun=MSubKlasAkun.ID)" & _
                  " LEFT JOIN MKlasAkun On MSubKlasAkun.IDKlasAkun=MKlasAkun.ID)" & _
                  " LEFT JOIN MMataUang On MMataUang.ID=MAkun.IDMataUang"
            ds = ExecuteDataset("MSettingAkun", SQL)
            If Not ds.Tables("MSettingAkun") Is Nothing Then
                txtAkunBalancing.Properties.DataSource = ds.Tables("MSettingAkun")
                txtAkunLRBerjalan.Properties.DataSource = ds.Tables("MSettingAkun")

                txtAkunPembelian.Properties.DataSource = ds.Tables("MSettingAkun")
                txtAkunKasPembelian.Properties.DataSource = ds.Tables("MSettingAkun")
                txtAkunBiayaPembelian.Properties.DataSource = ds.Tables("MSettingAkun")

                txtAkunReturPembelian.Properties.DataSource = ds.Tables("MSettingAkun")
                txtAkunKasReturPembelian.Properties.DataSource = ds.Tables("MSettingAkun")
                txtAkunBiayaReturPembelian.Properties.DataSource = ds.Tables("MSettingAkun")

                txtAkunPenjualan.Properties.DataSource = ds.Tables("MSettingAkun")
                txtAkunKasPenjualan.Properties.DataSource = ds.Tables("MSettingAkun")
                txtAkunBiayaPenjualan.Properties.DataSource = ds.Tables("MSettingAkun")
                txtAkunPotonganPenjualan.Properties.DataSource = ds.Tables("MSettingAkun")

                txtAkunReturPenjualan.Properties.DataSource = ds.Tables("MSettingAkun")
                txtAkunKasReturPenjualan.Properties.DataSource = ds.Tables("MSettingAkun")
                txtAkunBiayaReturPenjualan.Properties.DataSource = ds.Tables("MSettingAkun")

            End If
            txtAkunBalancing.Properties.ValueMember = "ID"
            txtAkunBalancing.Properties.DisplayMember = "Perkiraan"
            txtAkunLRBerjalan.Properties.ValueMember = "ID"
            txtAkunLRBerjalan.Properties.DisplayMember = "Perkiraan"

            txtAkunPembelian.Properties.ValueMember = "ID"
            txtAkunPembelian.Properties.DisplayMember = "Perkiraan"
            txtAkunKasPembelian.Properties.ValueMember = "ID"
            txtAkunKasPembelian.Properties.DisplayMember = "Perkiraan"
            txtAkunBiayaPembelian.Properties.ValueMember = "ID"
            txtAkunBiayaPembelian.Properties.DisplayMember = "Perkiraan"

            txtAkunReturPembelian.Properties.ValueMember = "ID"
            txtAkunReturPembelian.Properties.DisplayMember = "Perkiraan"
            txtAkunKasReturPembelian.Properties.ValueMember = "ID"
            txtAkunKasReturPembelian.Properties.DisplayMember = "Perkiraan"
            txtAkunBiayaReturPembelian.Properties.ValueMember = "ID"
            txtAkunBiayaReturPembelian.Properties.DisplayMember = "Perkiraan"

            txtAkunPenjualan.Properties.ValueMember = "ID"
            txtAkunPenjualan.Properties.DisplayMember = "Perkiraan"
            txtAkunKasPenjualan.Properties.ValueMember = "ID"
            txtAkunKasPenjualan.Properties.DisplayMember = "Perkiraan"
            txtAkunBiayaPenjualan.Properties.ValueMember = "ID"
            txtAkunBiayaPenjualan.Properties.DisplayMember = "Perkiraan"
            txtAkunPotonganPenjualan.Properties.ValueMember = "ID"
            txtAkunPotonganPenjualan.Properties.DisplayMember = "Perkiraan"

            txtAkunReturPenjualan.Properties.ValueMember = "ID"
            txtAkunReturPenjualan.Properties.DisplayMember = "Perkiraan"
            txtAkunKasReturPenjualan.Properties.ValueMember = "ID"
            txtAkunKasReturPenjualan.Properties.DisplayMember = "Perkiraan"
            txtAkunBiayaReturPenjualan.Properties.ValueMember = "ID"
            txtAkunBiayaReturPenjualan.Properties.DisplayMember = "Perkiraan"

            SQL = "SELECT NoID, Kode, Nama FROM MGudang WHERE IsActive=1"
            ds = ExecuteDataset("MGudang", SQL)
            If Not ds.Tables("MGudang") Is Nothing Then
                txtGudang.Properties.DataSource = ds.Tables("MGudang")
            End If
            txtGudang.Properties.DisplayMember = "Kode"
            txtGudang.Properties.ValueMember = "NoID"
        Catch ex As Exception
            XtraMessageBox.Show("Info Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
        End Try
    End Sub
    Private Sub cmdRefresh_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdRefresh.Click
        Try
            SQL = "SELECT * FROM MSettingAkun "
            ds = ExecuteDataset("MSettingAkun", SQL)
            If Not ds.Tables("MSettingAkun") Is Nothing AndAlso ds.Tables("MSettingAkun").Rows.Count >= 1 Then
                txtAkunBalancing.EditValue = NullToLong(ds.Tables(0).Rows(0).Item("IDAkunSelisihBalancing"))
                txtAkunLRBerjalan.EditValue = NullToLong(ds.Tables(0).Rows(0).Item("IDAkunLRBerjalan"))

                txtAkunPembelian.EditValue = NullToLong(ds.Tables(0).Rows(0).Item("IDAkunPembelian"))
                txtAkunKasPembelian.EditValue = NullToLong(ds.Tables(0).Rows(0).Item("IDAkunKasPembelian"))
                txtAkunBiayaPembelian.EditValue = NullToLong(ds.Tables(0).Rows(0).Item("IDAkunBiayaPembelian"))

                txtAkunReturPembelian.EditValue = NullToLong(ds.Tables(0).Rows(0).Item("IDAkunReturPembelian"))
                txtAkunKasReturPembelian.EditValue = NullToLong(ds.Tables(0).Rows(0).Item("IDAkunKasReturPembelian"))
                txtAkunBiayaReturPembelian.EditValue = NullToLong(ds.Tables(0).Rows(0).Item("IDAkunBiayaReturPembelian"))

                txtAkunPenjualan.EditValue = NullToLong(ds.Tables(0).Rows(0).Item("IDAkunPenjualan"))
                txtAkunKasPenjualan.EditValue = NullToLong(ds.Tables(0).Rows(0).Item("IDAkunKasPenjualan"))
                txtAkunBiayaPenjualan.EditValue = NullToLong(ds.Tables(0).Rows(0).Item("IDAkunBiayaPenjualan"))
                txtAkunPotonganPenjualan.EditValue = NullToLong(ds.Tables(0).Rows(0).Item("IDAkunPotonganPenjualan"))

                txtAkunReturPenjualan.EditValue = NullToLong(ds.Tables(0).Rows(0).Item("IDAkunReturPenjualan"))
                txtAkunKasReturPenjualan.EditValue = NullToLong(ds.Tables(0).Rows(0).Item("IDAkunKasReturPenjualan"))
                txtAkunBiayaReturPenjualan.EditValue = NullToLong(ds.Tables(0).Rows(0).Item("IDAkunBiayaReturPenjualan"))

            Else
                txtAkunBalancing.EditValue = -1
                txtAkunLRBerjalan.EditValue = -1

                txtAkunPembelian.EditValue = -1
                txtAkunKasPembelian.EditValue = -1
                txtAkunBiayaPembelian.EditValue = -1

                txtAkunReturPembelian.EditValue = -1
                txtAkunKasReturPembelian.EditValue = -1
                txtAkunBiayaReturPembelian.EditValue = -1

                txtAkunPenjualan.EditValue = -1
                txtAkunKasPenjualan.EditValue = -1
                txtAkunBiayaPenjualan.EditValue = -1
                txtAkunPotonganPenjualan.EditValue = -1

                txtAkunReturPenjualan.EditValue = -1
                txtAkunKasReturPenjualan.EditValue = -1
                txtAkunBiayaReturPenjualan.EditValue = -1
            End If
        Catch ex As Exception
            XtraMessageBox.Show("Info Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
        End Try
    End Sub
End Class