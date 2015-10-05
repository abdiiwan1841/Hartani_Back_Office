Imports DevExpress.XtraEditors
Imports DevExpress.Utils
Public Class frmPerbaikanPosting
    Dim IsJalan As Boolean = False
    Private Sub frmPerbaikanPosting_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        txtTglDari.DateTime = TanggalSystem
        txtTglSampai.DateTime = TanggalSystem
        FungsiControl.SetForm(Me)
    End Sub

    Private Sub cmdCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdCancel.Click
        If IsJalan Then
            IsJalan = False
        Else
            DialogResult = Windows.Forms.DialogResult.Cancel
            Close()
        End If
    End Sub

    Private Sub cmdOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdOK.Click
        Dim SQL As String = ""
        Dim ds As New DataSet
        Try
            ProgressBarControl1.Visible = True
            ProgressBarControl1.Position = 0
            SQL = "SELECT X.* FROM (" & vbCrLf
            SQL &= " SELECT MBeli.Tanggal, 'Pembelian' AS Status, 2 AS Status2, MBeli.NoID, MBeli.Kode FROM MBeli WHERE MBeli.Tanggal>='" & txtTglDari.DateTime.ToString("yyyy/MM/dd") & "' AND MBeli.Tanggal<'" & DateAdd(DateInterval.Day, 1, txtTglSampai.DateTime).ToString("yyyy/MM/dd") & "'" & vbCrLf
            SQL &= " UNION ALL " & vbCrLf
            SQL &= " SELECT MReturBeli.Tanggal, 'ReturPembelian' AS Status, 3 AS Status2, MReturBeli.NoID, MReturBeli.Kode FROM MReturBeli WHERE MReturBeli.Tanggal>='" & txtTglDari.DateTime.ToString("yyyy/MM/dd") & "' AND MReturBeli.Tanggal<'" & DateAdd(DateInterval.Day, 1, txtTglSampai.DateTime).ToString("yyyy/MM/dd") & "'" & vbCrLf
            SQL &= " UNION ALL " & vbCrLf
            SQL &= " SELECT MJual.Tanggal, 'Penjualan' AS Status, 6 AS Status2, MJual.NoID, MJual.Kode  FROM MJual WHERE MJual.Tanggal>='" & txtTglDari.DateTime.ToString("yyyy/MM/dd") & "' AND MJual.Tanggal<'" & DateAdd(DateInterval.Day, 1, txtTglSampai.DateTime).ToString("yyyy/MM/dd") & "'" & vbCrLf
            SQL &= " ) AS X ORDER BY Tanggal, Status2, NoID" & vbCrLf
            ds = modSqlServer.ExecuteDataset("Data", SQL)
            If ds.Tables("Data").Rows.Count >= 1 Then
                IsJalan = True
                'modSqlServer.EksekusiSQL("DELETE FROM MKartuStok WHERE (IDJenisTransaksi=2 or IDJenisTransaksi=3 or IDJenisTransaksi=6) AND Tanggal>='" & txtTglDari.DateTime.ToString("yyyy/MM/dd") & "' AND Tanggal<'" & DateAdd(DateInterval.Day, 1, txtTglSampai.DateTime).ToString("yyyy/MM/dd") & "'")
                For i As Integer = 0 To ds.Tables("Data").Rows.Count - 1
                    If IsJalan Then
                        If NullTostr(ds.Tables("Data").Rows(i).Item("Status")).ToLower = "pembelian" Then
                            clsPostingPembelian.UnPostingStokBarangPembelian(NullTolong(ds.Tables("Data").Rows(i).Item("NoID")))
                            clsPostingPembelian.PostingStokBarangPembelian(NullTolong(ds.Tables("Data").Rows(i).Item("NoID")))
                        ElseIf NullTostr(ds.Tables("Data").Rows(i).Item("Status")).ToLower = "returpembelian" Then
                            clsPostingPembelian.UnPostingStokBarangReturPembelian(NullTolong(ds.Tables("Data").Rows(i).Item("NoID")))
                            clsPostingPembelian.PostingStokBarangReturPembelian(NullTolong(ds.Tables("Data").Rows(i).Item("NoID")))
                        ElseIf NullTostr(ds.Tables("Data").Rows(i).Item("Status")).ToLower = "penjualan" Then
                            'clsPostingPembelian.UnPostingStokBarangPenjualan(NullTolong(ds.Tables("Data").Rows(i).Item("NoID")))
                            'clsPostingPembelian.PostingStokBarangPenjualan(NullTolong(ds.Tables("Data").Rows(i).Item("NoID")))
                        End If
                        ProgressBarControl1.Position = CDbl(i * 100 \ ds.Tables("Data").Rows.Count)
                        'ProgressBarControl1.Text = CStr(CDbl(i * 100 \ ds.Tables("Data").Rows.Count).ToString("###,###,###,###,##,###,##0") & "  of  " & (ds.Tables("Data").Rows.Count).ToString("###,###,###,###,##,###,##0") & " successes.")
                        ProgressBarControl1.Refresh()
                        Application.DoEvents()
                    Else
                        Exit For
                    End If
                Next
            End If
        Catch ex As Exception
            XtraMessageBox.Show("Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK)
        Finally
            IsJalan = False
            ds.Dispose()
            ProgressBarControl1.Visible = False
        End Try
    End Sub

    Private Sub txtTglSampai_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtTglSampai.EditValueChanged

    End Sub
End Class