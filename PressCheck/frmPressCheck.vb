Imports System.Data
Imports System.Data.SqlClient
Imports DevExpress.XtraEditors
Imports System.Threading
Imports System.IO

Public Class frmPressCheck
    Dim con As SqlConnection = Nothing
    Dim comm As SqlCommand = Nothing
    Dim oDA As SqlDataAdapter = Nothing
    Dim ds As New DataSet
    Dim SQL As String = ""
    Private RunNextSlide As Boolean = False
    'Dim bmp As Bitmap
    Private JumFileImage As Integer
    Private NmFileImage() As String
    Private NotesImage() As String
    Private Thread As Thread
    Private Ke As Integer = 0
    Dim pic As New PictureBox
    'Dim bitmapfile As FileStream = Nothing
    Dim bmp As Bitmap = Nothing

    Private Sub frmPressCheck_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Activated
        TextEdit1.Focus()
    End Sub

    Private Sub frmPressCheck_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            con = New SqlConnection
            con.ConnectionString = StrKonSql
            con.Open()
            comm = New SqlCommand
            comm.Connection = con
            oDA = New SqlDataAdapter
            oDA.SelectCommand = comm

            PanelControlBarang.Visible = False
            PanelControlMember.Visible = False

            getListFromdbs()
            'StartThread()
            LabelControl1.Text = Ini.BacaIni("PRESS CHECK", "Header1", "Hartani") & vbCrLf & Ini.BacaIni("PRESS CHECK", "Header2", "Prices Checkers")
            Timer1.Enabled = False
            Timer2.Enabled = True
            tmrDelay.Enabled = False
        Catch ex As Exception
            XtraMessageBox.Show("Info Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
            End
        End Try
    End Sub
    'Private Sub StartThread()
    '    'Thread.Abort()
    '    ' give the thread time to die
    '    Threading.Thread.Sleep(100)
    '    Invalidate()
    '    Thread = New Thread(New ThreadStart(AddressOf DrawMovingText))
    '    Thread.Start()
    '    '// <summary>
    '    '// DrawMovingText is the thread function that will draw and update the text
    '    '// </summary>
    'End Sub
    'Private Sub DrawMovingText()

    '    Dim grfx As Graphics
    '    Dim Fnt As Font
    '    Dim str As StringBuilder
    '    Dim Rect As Rectangle
    '    Dim Numcycles As Integer
    '    Dim i As Integer
    '    grfx = PnlScrollText.CreateGraphics ' CreateGraphics()
    '    Fnt = New Font("Courier New", 36, FontStyle.Bold)
    '    str = New StringBuilder(getstrFromdbs)
    '    Rect = CreateRect(grfx, str, Fnt)
    '    Numcycles = str.Length * 20 + 1
    '    For i = 0 To Numcycles - 1

    '        grfx.FillRectangle(Brushes.White, Rect)
    '        grfx.DrawString(str.ToString(), Fnt, Brushes.Red, Rect)
    '        '// relocate the first char to the end of the string
    '        str.Append(str(0))
    '        str.Remove(0, 1)
    '        '// pause for visual effect
    '        Threading.Thread.Sleep(200)
    '    Next
    '    grfx.Dispose()
    '    DrawMovingText()
    'End Sub
    'Private Function CreateRect(ByVal grfx As Graphics, ByVal str As StringBuilder, ByVal font As Font) As Rectangle
    '    Dim w, h, x, y As Integer
    '    w = grfx.MeasureString(str.ToString(), font).Width + 5 '; // +5 to allow last char to fit
    '    h = grfx.MeasureString(str.ToString(), font).Height
    '    x = PnlScrollText.Width / 2 - w / 2
    '    y = 0 'PnlScrollText.Height / 2 - h
    '    Return New Rectangle(x, y, w, h)
    'End Function
    Sub getListFromdbs()
        Dim dirSalah As IO.DirectoryInfo = Nothing
        Dim fileFoto As IO.FileInfo()
        Dim fileFotoEmpty As IO.FileInfo
        Dim i As Integer = 0
        Try
            'Ini.TulisIni("PRESS CHECK", "Path", "E:\FOTO\ANDROID\")
            Dim path As String = Ini.BacaIni("PRESS CHECK", "Path", Application.StartupPath & "\System\PressCheck\")
            If Directory.Exists(path) Then
                dirSalah = New IO.DirectoryInfo(path)
                fileFoto = dirSalah.GetFiles
                i = 1
                For Each fileFotoEmpty In fileFoto
                    If fileFotoEmpty.Extension.ToString.ToUpper = ".JPG" Or fileFotoEmpty.Extension.ToString.ToUpper = ".PNG" Or fileFotoEmpty.Extension.ToString.ToUpper = ".BMP" Then
                        ReDim Preserve NmFileImage(i)
                        NmFileImage(i) = fileFotoEmpty.DirectoryName.ToString & "\" & fileFotoEmpty.Name.ToString
                        i = i + 1
                    End If
                Next
                JumFileImage = fileFoto.Length
            End If
        Catch ex As Exception
            XtraMessageBox.Show("Info Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub TextEdit1_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles TextEdit1.KeyDown
        If e.KeyCode = Keys.Enter Then
            Cari()
            TextEdit1.Text = ""
        ElseIf e.KeyCode = Keys.Escape Then
            End
        ElseIf e.KeyCode = Keys.Home Then
            TextEdit1.Text = ""
            PanelControlBarang.Visible = False
            PanelControlMember.Visible = False
            PanelControlAnimation.Visible = True
            Timer2.Enabled = True
        End If
    End Sub

    Private Sub Cari()
        Dim TglDari As Date = Now
        Dim TglSampai As Date = Now
        Dim SaldoPoin As Double = 0
        Dim TglDari2 As Date = Now
        Dim TglSampai2 As Date = Now
        If TextEdit1.Text.Length = 8 AndAlso TextEdit1.Text.Substring(0, 2) = "90" Then
            'Cari Poin Member
            SQL = "SELECT Malamat.NoID, MAlamat.Kode, MAlamat.Nama, MAlamat.Alamat " & vbCrLf & _
                  " FROM MAlamat " & vbCrLf & _
                  " WHERE UPPER(MAlamat.Kode)='" & FixApostropi(TextEdit1.Text) & "'"
            If Not ds.Tables("MAlamat") Is Nothing Then
                ds.Tables("MAlamat").Clear()
            End If
            comm.CommandText = SQL
            oDA.Fill(ds, "MAlamat")
            With ds.Tables("MAlamat")
                If ds.Tables("MAlamat").Rows.Count > 0 Then
                    lbKodeMember.Text = NullToStr(.Rows(0).Item("Kode"))
                    lbNamaMember.Text = NullToStr(.Rows(0).Item("Nama"))
                    'txtSatuan.Text = NullToStr(ds.Tables("A").Rows(0).Item("Alamat"))
                    lbBarcodeMember.Text = NullToStr(.Rows(0).Item("Kode"))
                    'SQL = "SELECT " & _
                    '" IsNull((SELECT SUM(MJual.NilaiPoin) FROM MJual WHERE MJual.IDCustomer=" & NullToLong(.Rows(0).Item("NoID")) & "),0)-" & _
                    '" IsNull((SELECT SUM(MTukarPoin.Kredit) FROM MTukarPoin WHERE MTukarPoin.IDMember=" & NullToLong(.Rows(0).Item("NoID")) & "),0)" & _
                    '" AS POIN"
                    SQL = "SELECT vSaldoPoin.SaldoPoin FROM vSaldoPoin WHERE vSaldoPoin.IDCustomer=" & NullToLong(.Rows(0).Item("NoID"))
                    SaldoPoin = NullToDbl(EksekusiSQlSkalarNew(SQL))
                    lbPoinMember.Text = "Poin : " & SaldoPoin.ToString("#,##0")
                Else
                    lbKodeMember.Text = "Maaf, member yang anda cari tidak ditemukan."
                    lbNamaMember.Text = ""
                    'txtSatuan.Text = NullToStr(ds.Tables("A").Rows(0).Item("Alamat"))
                    lbBarcodeMember.Text = "00000000"
                    lbPoinMember.Text = "Poin : 0.00"
                End If
            End With
            PanelControlBarang.Visible = False
            PanelControlMember.Visible = True
            PanelControlAnimation.Visible = False
            PanelControlMember.Dock = DockStyle.Fill
        Else
            'Cari Barcode Barang
            SQL = "SELECT MBarang.NoID, MBarang.Kode, Mbarang.Nama,MSatuan.Nama as Satuan,MBarang.Barcode,MBarangD.HargaJual,MBarangD.HargaJual2,MBarangD.HargaJual3,MBarangD.NilaiDiskon, MBarang.Qty1, MBarang.Qty2, MBarang.Qty3, MBarang.TglDariDiskon, MBarang.TglSampaiDiskon, MBarang.TglDariDiskon2, MBarang.TglSampaiDiskon2, MBarang.DiscMemberRp2, MBarang.DiscMemberProsen2 " & _
                  "FROM MBarangD INNER JOIN MBarang On MbarangD.IDbarang=Mbarang.NoID left Join MSatuan On MbarangD.IDsatuan=MSatuan.NoID WHERE MBarang.IsActive=1 AND MBarangD.IsActive=1 AND (MBarang.Kode='" & FixApostropi(TextEdit1.Text) & "' or MBarangD.Barcode='" & FixApostropi(TextEdit1.Text) & "')"
            If Not ds.Tables("MBarang") Is Nothing Then
                ds.Tables("MBarang").Clear()
            End If
            comm.CommandText = SQL
            oDA.Fill(ds, "MBarang")
            If ds.Tables("MBarang").Rows.Count > 0 Then
                With ds.Tables("MBarang")
                    lbKodeBarang.Text = NullToStr(.Rows(0).Item("Kode"))
                    lbBarcodeBarang.Text = NullToStr(.Rows(0).Item("Barcode"))
                    lbNamaBarang.Text = NullToStr(.Rows(0).Item("Nama"))
                    If NullToDbl(.Rows(0).Item("Qty1")) <> NullToDbl(.Rows(0).Item("Qty2")) <> NullToDbl(.Rows(0).Item("Qty3")) Then
                        'lbHarga.Visible = False
                        lbHarga.Text = "Rp. " & NullToDbl(.Rows(0).Item("HargaJual")).ToString("#,##0")
                        lbHargaJualA.Text = "Pembelian  " & NullToDbl(.Rows(0).Item("Qty1")).ToString("#,##0") & " s/d " & CDbl(NullToDbl(.Rows(0).Item("Qty2")) - 1).ToString("#,##0") & "  Rp. " & NullToDbl(.Rows(0).Item("HargaJual")).ToString("#,##0")
                        lbHargaJualB.Text = "Pembelian  " & NullToDbl(.Rows(0).Item("Qty2")).ToString("#,##0") & " s/d " & CDbl(NullToDbl(.Rows(0).Item("Qty3")) - 1).ToString("#,##0") & "  Rp. " & NullToDbl(.Rows(0).Item("HargaJual2")).ToString("#,##0")
                        lbHargaJualC.Text = "Pembelian  " & NullToDbl(.Rows(0).Item("Qty3")).ToString("#,##0") & " s/d Keatas  Rp. " & NullToDbl(.Rows(0).Item("HargaJual3")).ToString("#,##0")
                        PanelHargaABC.Visible = True
                    Else
                        lbHarga.Text = "Rp. " & NullToDbl(.Rows(0).Item("HargaJual")).ToString("#,##0")
                        'lbHarga.Visible = True
                        lbHargaJualA.Text = "Pembelian  0 s/d 0  Rp. 0"
                        lbHargaJualB.Text = "Pembelian  0 s/d 0  Rp. 0"
                        lbHargaJualC.Text = "Pembelian  0 s/d 0  Rp. 0"
                        PanelHargaABC.Visible = False
                    End If
                    TglDari = NullToDate(.Rows(0).Item("TglDariDiskon"))
                    TglSampai = NullToDate(.Rows(0).Item("TglSampaiDiskon"))
                    TglDari2 = NullToDate(.Rows(0).Item("TglDariDiskon2"))
                    TglSampai2 = NullToDate(.Rows(0).Item("TglSampaiDiskon2"))

                    If Now.Date >= TglDari2.Date AndAlso Now.Date <= TglSampai2.Date Then
                        PanelControlPromo.Visible = True
                        lbDiskon.Text = ": " & NullToDbl(.Rows(0).Item("DiscMemberRp2")).ToString("#,##0.00")
                        lbHargaPromo.Text = "Disc " & NullToDbl(.Rows(0).Item("DiscMemberProsen2")).ToString("#,##0.00") & "%, Rp. " & CDbl(NullToDbl(.Rows(0).Item("HargaJual")) - NullToDbl(.Rows(0).Item("DiscMemberRp2"))).ToString("#,##0")
                        lbTglPromo.Text = ": " & TglDari2.Date.ToString("dd-MM-yyyy") & " s/d " & TglSampai2.Date.ToString("dd-MM-yyyy") & " (Member " & Ini.BacaIni("PRESS CHECK", "Header1", "Hartani") & ")"
                        'PanelBottom.Visible = True
                    ElseIf Now.Date >= TglDari.Date AndAlso Now.Date <= TglSampai.Date Then
                        PanelControlPromo.Visible = True
                        lbDiskon.Text = ": " & NullToDbl(.Rows(0).Item("NilaiDiskon")).ToString("#,##0.00")
                        lbHargaPromo.Text = "Rp. " & CDbl(NullToDbl(.Rows(0).Item("HargaJual")) - NullToDbl(.Rows(0).Item("NilaiDiskon"))).ToString("#,##0")
                        lbTglPromo.Text = ": " & TglDari.Date.ToString("dd-MM-yyyy") & " s/d " & TglSampai.Date.ToString("dd-MM-yyyy")
                        'PanelBottom.Visible = True
                    Else
                        PanelControlPromo.Visible = False
                        'If PanelHargaABC.Visible Then
                        '    PanelBottom.Visible = True
                        'Else
                        '    PanelBottom.Visible = False
                        'End If
                    End If
                    'If Not PanelControlPromo.Visible AndAlso Not PanelHargaABC.Visible Then
                    '    PanelBottom.Visible = False
                    'Else
                    '    PanelBottom.Visible = True
                    'End If
                    PanelBottom.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder
                    PanelBottom.Visible = True
                End With
            Else
                lbKodeBarang.Text = "Maaf, barang yang anda cari tidak ditemukan."
                lbBarcodeBarang.Text = "0000000000000"
                lbNamaBarang.Text = ""
                lbHarga.Text = "Rp. 0.00"
                'lbHarga.Visible = True
                lbHargaJualA.Text = "Pembelian  0 s/d 0  Rp. 0"
                lbHargaJualB.Text = "Pembelian  0 s/d 0  Rp. 0"
                lbHargaJualC.Text = "Pembelian  0 s/d 0  Rp. 0"
                PanelBottom.Visible = False
            End If
            PanelControlBarang.Visible = True
            PanelControlMember.Visible = False
            PanelControlAnimation.Visible = False
            PanelControlBarang.Dock = DockStyle.Fill
        End If
    End Sub

    Private Sub lbBarcodeBarang_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbBarcodeBarang.TextChanged
        Try
            AxBarcodeXBarang.Caption = lbBarcodeBarang.Text
        Catch ex As Exception

        End Try
    End Sub

    Private Sub Timer2_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer2.Tick
        If RunNextSlide Then
            Timer1.Enabled = True
            PanelControlBarang.Visible = False
            PanelControlMember.Visible = False
            PanelControlAnimation.Visible = True
        End If
        RunNextSlide = Not RunNextSlide
        'DigitalGauge1.Text = Format(Now(), "HH:mm")
    End Sub

    Private Sub Timer1_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer1.Tick
        Dim modelslide As Integer
        Dim i As Integer = 0
        Try
            If JumFileImage > 0 AndAlso PanelControlAnimation.Visible And (PanelControlBarang.Visible Or PanelControlMember.Visible) Then
                Timer1.Enabled = False
                Timer2.Enabled = False
                Randomize()
                modelslide = 28 * Rnd()
                Ke = (Ke + 1) Mod JumFileImage
                AnimationControl1.AnimatedFadeImage = Nothing
                AnimationControl1.AnimatedImage = Nothing
                'If Not bmp Is Nothing Then
                '    bmp.Dispose()
                'End If
                'If Not bitmapfile Is Nothing Then
                '    bitmapfile.Close()
                '    bitmapfile.Dispose()
                'End If
                'bitmapfile = New FileStream(NmFileImage(Ke), FileMode.Open, FileAccess.Read, FileShare.ReadWrite)
                'bmp = Bitmap.FromStream(bitmapfile)
                bmp = pic.Image
                AnimationControl1.AnimatedFadeImage = bmp
                'pic.ImageLocation = NmFileImage(Ke)
                If Ke = 0 Then
                    bmp = Bitmap.FromFile(NmFileImage(Ke + 1))
                Else
                    bmp = Bitmap.FromFile(NmFileImage(Ke))
                End If

                AnimationControl1.AnimatedImage = bmp
                AnimationControl1.AnimationType = modelslide
                AnimationControl1.Animate(30)
                AnimationControl1.Refresh()
                Application.DoEvents()
                PanelControlAnimation.ContentImage = AnimationControl1.AnimatedImage
                RunNextSlide = False
                If Not tmrDelay.Enabled Then
                    Timer2.Enabled = True
                End If
                PanelControlAnimation.Visible = True
            End If
        Catch ex As Exception
            XtraMessageBox.Show("Info Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            'If Not bmp Is Nothing Then
            '    bmp.Dispose()
            'End If
            'If Not bitmapfile Is Nothing Then
            '    bitmapfile.Close()
            '    bitmapfile.Dispose()
            'End If
        End Try
    End Sub

    Private Sub TextEdit1_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextEdit1.EditValueChanged

    End Sub

    Private Sub tmrDelay_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tmrDelay.Tick

    End Sub

    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

    End Sub
End Class