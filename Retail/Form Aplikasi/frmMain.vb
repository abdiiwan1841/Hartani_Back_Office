Imports DevExpress.XtraBars
Imports DevExpress.XtraEditors
Imports System.Data.SqlClient
Imports System.Data.SQLite
Imports VPoint.Ini
Imports VPoint.mdlCetakCR

Public Class frmMain
    Dim WithEvents itemParent As BarSubItem
    Dim WithEvents itemParentBar2 As BarSubItem
    Dim WithEvents barTemp As New Bar
    Dim WithEvents itemmenu As BarButtonItem
    Dim WithEvents barsubitemmenu As BarSubItem
    Public SkinKu As String = "Summer 2008"
    Dim DashBoard As New frmDashBoard

    Private Sub OnPaintStyleClick(ByVal sender As Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs)
        SkinKu = e.Item.Tag.ToString()
        DefaultLookAndFeel1.LookAndFeel.SetSkinStyle(SkinKu)
    End Sub
    Private Sub Loadskin()
        Try
            For Each skin As DevExpress.Skins.SkinContainer In DevExpress.Skins.SkinManager.Default.Skins
                Dim item As DevExpress.XtraBars.BarButtonItem = BarManager1.Items.CreateButton(skin.SkinName)
                item.Tag = skin.SkinName
                'Dim item As BarButtonItem = New BarButtonItem(BarManager1, skin.SkinName)
                mnSkins.AddItem(item)
                AddHandler item.ItemClick, AddressOf OnPaintStyleClick
            Next skin
            SkinKu = BacaIni("Applications", "Skins", "Summer 2008")
            If SkinKu = "" Or SkinKu = Nothing Then
                SkinKu = "Summer 2008"
            End If
            Me.DefaultLookAndFeel1.LookAndFeel.UseWindowsXPTheme = False
            Me.DefaultLookAndFeel1.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.Skin
            Me.DefaultLookAndFeel1.LookAndFeel.SkinName = SkinKu
        Catch ex As Exception
            DevExpress.XtraEditors.XtraMessageBox.Show(ex.Message, "Admin Says", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub
    Private Sub BarButtonItem4_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles mnFormEditor.ItemClick
        Dim frm As New frmGenerateForm
        frm.ShowDialog(Me)
        frm.Dispose()
    End Sub

    Private Sub frmMain_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        If XtraMessageBox.Show("Yakin anda ingin keluar dari aplikasi?", NamaAplikasi, MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) = Windows.Forms.DialogResult.No Then
            e.Cancel = True
        Else
            'BarManager1.SaveLayoutToXml(folderLayouts &  Me.Name & "Bars.xml")
            TulisIni("Applications", "Skins", DefaultLookAndFeel1.LookAndFeel.SkinName.ToString)
            TulisIni("Application", "UseFramework", ckFx.Checked)
            BarManager1.SaveLayoutToXml(FolderLayouts & Me.Name & "_BarMenu.xml")
            HapusLogUser()
            End
        End If
    End Sub

    Private Sub frmMain_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Dim exe As String = Application.StartupPath & "\ToysRetail.exe"
        'Dim ObjFSO As New Scripting.FileSystemObject
        'Dim objFile As Scripting.File
        Dim exe As System.IO.FileInfo
        Dim str As String
        'Dim ToolTip As New DevExpress.Utils.SuperToolTip

        Try
            LoadImageListFromDLL()
            GenerateMenu()
            Loadskin()
            ckFx.Checked = NullToBool(BacaIni("Application", "UseFramework", True))
            mnTglSystem.Caption = "Tanggal System : " & TanggalSystem.ToString("dd/MMM/yyyy")
            mnStatusPeriode.Caption = "Periode : " & TanggalSystem.ToString("MMMM yyyy")
            mnIP.Caption = "Online on : " & IPLokal.ToUpper
            'Me.Text = Me.Text & " " & "'SPBU 99'" & " [Version : " & Application.ProductVersion.ToString & "]"
            'Me.Text = "System Distributor untuk UD Tunggal Jaya " & " [Version : " & Application.ProductVersion.ToString & "]"
            'Me.Text = "VPoint Retail System " & " [Version : " & Application.ProductVersion.ToString & "]"
            str = Application.StartupPath & "\" & My.Application.Info.AssemblyName.ToString & ".exe"
            If System.IO.File.Exists(str) Then
                exe = New System.IO.FileInfo(str)
                Me.Text = "VPoint Retail System - " & Me.Text & " [Version : " & Application.ProductVersion.ToString & "] AT " & exe.LastWriteTime.ToString("dd-MM-yyyy")
            Else
                Me.Text = "VPoint Retail System - " & Me.Text & " [Version : " & Application.ProductVersion.ToString & "]"
            End If

            'mnStatusUser.SuperTip.Items(0). = "Tiara Muda/Click disini untuk merubah Password anda./Retail System"
            FungsiControl.SetForm(Me)
            iCascade.Visibility = BarItemVisibility.Never
            iTileHorizontal.Visibility = BarItemVisibility.Never
            iTileVertical.Visibility = BarItemVisibility.Never
            frmServiceSQLServer.Show()
            Application.DoEvents()
            DashBoard.MdiParent = Me
            DashBoard.Show()
            DashBoard.Focus()
            Login(mnloginout)
            'If System.IO.File.Exists(folderLayouts &  Me.Name & "Bars.xml") Then
            '    BarManager1.RestoreLayoutFromXml(folderLayouts &  Me.Name & "Bars.xml")
            'End If
        Catch ex As Exception

        End Try


    End Sub
    Sub GenerateMenu()
        'Dim oConn As SqlConnection
        'Dim ocmd As SqlCommand
        'Dim odr As SqlDataReader
        'Dim oConn1 As SqlConnection
        'Dim ocmd1 As SqlCommand
        'Dim odr1 As SqlDataReader
        'Dim strsql As String
        'strsql = "Select mmenu.noid,mmenu.kode,mmenu.caption,mshortcut.idshortcut," & _
        '"mmenu.icon,mmenu.isawalgroup,mmenu.objectrun from mmenu left join mshortcut on mmenu.shortcut=mshortcut.nama where isactive=1 and idparent=-1 order by nourut"
        'oConn = New SqlConnection(StrKonSql)
        'ocmd = New SqlCommand(strsql, oConn)
        'oConn.Open()
        'oConn1 = New SqlConnection(StrKonSql)
        'oConn1.Open()
        'odr = ocmd.ExecuteReader
        'Do While odr.Read
        '    itemParent = New BarSubItem
        '    'Create a new bar item representing a hyperlink editor
        '    itemParent.Name = NullTostr(odr.GetValue(1))
        '    itemParent.Caption = NullTostr(odr.GetValue(2))
        '    itemParent.ShortCut = NullTolInt(odr.GetValue(3))
        '    itemParent.ImageIndex = NullTolInt(odr.GetValue(4))
        '    AddHandler itemParent.ItemClick, AddressOf itemParent_ItemClick
        '    Bar1.AddItem(itemParent)
        '    strsql = "Select mmenu.noid,mmenu.kode,mmenu.caption,mshortcut.idshortcut," & _
        '             "mmenu.icon,mmenu.isawalgroup,mmenu.objectrun from mmenu left join mshortcut on mmenu.shortcut=mshortcut.nama where isactive=1 and idparent=" & NullTolong(odr.GetValue(0)) & " order by nourut"
        '    ocmd1 = New SqlCommand(strsql, oConn1)
        '    odr1 = ocmd1.ExecuteReader
        '    Do While odr1.Read
        '        itemmenu = New BarButtonItem
        '        itemmenu.Name = NullTostr(odr1.GetValue(1))
        '        itemmenu.Caption = NullTostr(odr1.GetValue(2))
        '        itemmenu.ShortCut = NullTolInt(odr1.GetValue(3))
        '        itemmenu.ImageIndex = NullTolInt(odr1.GetValue(4))
        '        AddHandler itemmenu.ItemClick, AddressOf itemmenu_ItemClick
        '        itemParent.ItemLinks.Add(itemmenu, NullTobool(odr1.GetValue(5)))
        '    Loop
        '    ocmd1.Dispose()
        '    odr1.Close()
        'Loop
        'ocmd.Dispose()
        'oConn.Close()
        'oConn.Dispose()
        'oConn1.Close()
        'oConn1.Dispose()
        ''itemParent.Dispose()
        ' ''Bar1.AddItem()
    End Sub
    Sub AktivekanMenubyUser()
        Dim oConn As SqlConnection
        Dim ocmd As SqlCommand
        Dim odr As SqlDataReader
        Dim oConn1 As SqlConnection
        Dim ocmd1 As SqlCommand
        Dim odr1 As SqlDataReader
        Dim oConn2 As SqlConnection
        Dim ocmd2 As SqlCommand
        Dim odr2 As SqlDataReader
        Dim strsql As String
        Try
            strsql = "Select mmenu.noid,mmenu.kode,mmenu.caption,mshortcut.idshortcut," & _
            " mmenu.icon,mmenu.isawalgroup,mmenu.objectrun,mmenu.shortcut,mmenu.keyshortcut from mmenu left join mshortcut on mmenu.shortcut=mshortcut.nama where isactive=1 and idparent=-1 AND MMenu.NoID IN (SELECT MUserD.IDMenu FROM MUserD WHERE MUserD.[Visible]=1 AND MUserD.IDUser=" & IDUserAktif & ") order by nourut"
            oConn = New SqlConnection(StrKonSql)
            ocmd = New SqlCommand(strsql, oConn)
            oConn.Open()
            oConn1 = New SqlConnection(StrKonSql)
            oConn1.Open()
            oConn2 = New SqlConnection(StrKonSql)
            oConn2.Open()
            odr = ocmd.ExecuteReader
            Do While odr.Read
                itemParent = New BarSubItem
                itemParentBar2 = New BarSubItem
                'Create a new bar item representing a hyperlink editor
                itemParent.PaintStyle = BarItemPaintStyle.CaptionInMenu
                itemParent.Name = NullToStr(odr.GetValue(1))
                itemParentBar2.Name = NullToStr(odr.GetValue(1)) & "Bar2"
                itemParent.Caption = NullToStr(odr.GetValue(2))
                itemParentBar2.Caption = NullToStr(odr.GetValue(2))
                'itemParent.ShortCut = NullTolInt(odr.GetValue(3))
                'itemParentBar2.ShortCut = NullTolInt(odr.GetValue(3))
                If NullTolInt(odr.GetValue(8)) <> 0 Then
                    Dim a As New System.Windows.Forms.Shortcut
                    a = NullTolInt(odr.GetValue(8))
                    itemParentBar2.ItemShortcut = New DevExpress.XtraBars.BarShortcut(a)
                    itemParentBar2.ShortcutKeyDisplayString = NullToStr(odr.GetValue(7))
                    itemParent.ItemShortcut = New DevExpress.XtraBars.BarShortcut(a)
                    itemParent.ShortcutKeyDisplayString = NullToStr(odr.GetValue(7))
                End If
                Try
                    itemParent.Glyph = ImageCollectionLarge.Images(NullTolInt(odr.GetValue(4)))
                    itemParent.GlyphDisabled = ImageCollectionLarge.Images(NullTolInt(odr.GetValue(4)))
                Catch
                End Try
                AddHandler itemParent.ItemClick, AddressOf itemParent_ItemClick
                AddHandler itemParentBar2.ItemClick, AddressOf itemParent_ItemClick
                Bar1.AddItem(itemParent)
                'Bar2.AddItem(itemParentBar2)
                barTemp.AddItem(itemParent)
                strsql = "Select mmenu.noid,mmenu.kode,mmenu.caption,mshortcut.idshortcut," & _
                         "mmenu.icon,mmenu.isawalgroup,mmenu.objectrun,mmenu.shortcut,mmenu.keyshortcut,mmenu.IsBarSubItem from mmenu left join mshortcut on mmenu.shortcut=mshortcut.nama where isactive=1 And ISNULL(IDBarSubItem,0)=0 AND MMenu.NoID IN (SELECT MUserD.IDMenu FROM MUserD WHERE MUserD.[Visible]=1 AND MUserD.IDUser=" & IDUserAktif & ") and idparent=" & NullToLong(odr.GetValue(0)) & " order by nourut"
                ocmd1 = New SqlCommand(strsql, oConn1)
                odr1 = ocmd1.ExecuteReader
                Do While odr1.Read
                    If Not NullToBool(odr1.GetValue(9)) Then
                        itemmenu = New BarButtonItem
                        itemmenu.Name = NullToStr(odr1.GetValue(1))
                        itemmenu.Caption = NullToStr(odr1.GetValue(2))
                        'itemmenu.ShortCut = NullTolInt(odr1.GetValue(3))
                        If NullTolInt(odr1.GetValue(8)) <> 0 Then
                            Dim a As New System.Windows.Forms.Shortcut
                            a = NullTolInt(odr1.GetValue(8))
                            itemmenu.ItemShortcut = New DevExpress.XtraBars.BarShortcut(a)
                            itemmenu.ShortcutKeyDisplayString = NullToStr(odr1.GetValue(7))
                        End If
                        itemmenu.ImageIndex = NullTolInt(odr1.GetValue(4))
                        itemmenu.Tag = NullToStr(odr1.GetValue(6))
                        AddHandler itemmenu.ItemClick, AddressOf itemmenu_ItemClick
                        itemParent.ItemLinks.Add(itemmenu, NullToBool(odr1.GetValue(5)))
                        itemParentBar2.ItemLinks.Add(itemmenu, NullToBool(odr1.GetValue(5)))
                        'mnBarSubMaster.ItemLinks.Add(itemmenu, NullTobool(odr1.GetValue(5)))
                    Else
                        barsubitemmenu = New BarSubItem
                        barsubitemmenu.Name = NullToStr(odr1.GetValue(1))
                        barsubitemmenu.Caption = NullToStr(odr1.GetValue(2))
                        'itemmenu.ShortCut = NullTolInt(odr1.GetValue(3))
                        If NullTolInt(odr1.GetValue(8)) <> 0 Then
                            Dim a As New System.Windows.Forms.Shortcut
                            a = NullTolInt(odr1.GetValue(8))
                            barsubitemmenu.ItemShortcut = New DevExpress.XtraBars.BarShortcut(a)
                            barsubitemmenu.ShortcutKeyDisplayString = NullToStr(odr1.GetValue(7))
                        End If
                        barsubitemmenu.ImageIndex = NullTolInt(odr1.GetValue(4))
                        barsubitemmenu.Tag = NullToStr(odr1.GetValue(6))
                        'AddHandler barsubitemmenu.ItemClick, AddressOf itemmenu_ItemClick
                        itemParent.ItemLinks.Add(barsubitemmenu, NullToBool(odr1.GetValue(5)))
                        itemParentBar2.ItemLinks.Add(barsubitemmenu, NullToBool(odr1.GetValue(5)))
                        'mnBarSubMaster.ItemLinks.Add(itemmenu, NullTobool(odr1.GetValue(5)))

                        strsql = "Select mmenu.noid,mmenu.kode,mmenu.caption,mshortcut.idshortcut," & _
                             "mmenu.icon,mmenu.isawalgroup,mmenu.objectrun,mmenu.shortcut,mmenu.keyshortcut from mmenu left join mshortcut on mmenu.shortcut=mshortcut.nama where isactive=1 AND MMenu.NoID IN (SELECT MUserD.IDMenu FROM MUserD WHERE MUserD.[Visible]=1 AND MUserD.IDUser=" & IDUserAktif & ") and idparent=" & NullToLong(odr.GetValue(0)) & " AND IDBarSubItem=" & NullToLong(odr1.GetValue(0)) & " order by nourut"
                        ocmd2 = New SqlCommand(strsql, oConn2)
                        odr2 = ocmd2.ExecuteReader
                        Do While odr2.Read
                            itemmenu = New BarButtonItem
                            itemmenu.Name = NullToStr(odr2.GetValue(1))
                            itemmenu.Caption = NullToStr(odr2.GetValue(2))
                            'itemmenu.ShortCut = NullTolInt(odr1.GetValue(3))
                            If NullTolInt(odr2.GetValue(8)) <> 0 Then
                                Dim a As New System.Windows.Forms.Shortcut
                                a = NullTolInt(odr2.GetValue(8))
                                itemmenu.ItemShortcut = New DevExpress.XtraBars.BarShortcut(a)
                                itemmenu.ShortcutKeyDisplayString = NullToStr(odr2.GetValue(7))
                            End If
                            itemmenu.ImageIndex = NullTolInt(odr2.GetValue(4))
                            itemmenu.Tag = NullToStr(odr2.GetValue(6))
                            'itemmenu.isa = NullToBool(odr2.GetValue(5))
                            AddHandler itemmenu.ItemClick, AddressOf itemmenu_ItemClick
                            barsubitemmenu.AddItem(itemmenu)
                            'itemParentBar2.ItemLinks.Add(itemmenu, NullToBool(odr2.GetValue(5)))
                            'mnBarSubMaster.ItemLinks.Add(itemmenu, NullTobool(odr1.GetValue(5)))
                        Loop
                        ocmd2.Dispose()
                        odr2.Close()
                    End If
                Loop
                ocmd1.Dispose()
                odr1.Close()
            Loop
            ocmd.Dispose()
            oConn.Close()
            oConn.Dispose()
            oConn1.Close()
            oConn1.Dispose()
            oConn2.Close()
            oConn2.Dispose()
            TanggalSystem = EksekusiSQlSkalarNew("SELECT Getdate() ")
            mnStatusUser.Caption = "Login : " & NamaUserAktif
            mnStatusGudang.Caption = "Gudang : " & NullToStr(EksekusiSQlSkalarNew("SELECT MGudang.Nama FROM Muser LEFT JOIN MGudang ON MGudang.NoID=MUSer.IDGudangDefault WHERE MUser.NoID=" & IDUserAktif))
            mnStatusRole.Caption = "Role : (None)"

            For Each mnParent As BarSubItemLink In Bar1.ItemLinks
                If mnParent.Item.ItemLinks.Count <= 0 Then
                    mnParent.Visible = False
                End If
            Next

        Catch ex As Exception

        End Try
        'If System.IO.File.Exists(folderLayouts &  Me.Name & "_BarMenu.xml") Then
        '    BarManager1.RestoreLayoutFromXml(folderLayouts &  Me.Name & "_BarMenu.xml")
        'End If
    End Sub
    Private Sub itemParent_ItemClick(ByVal sender As Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs)
        'MsgBox(e.Item.Name)
    End Sub

    Private Sub itemmenu_ItemClick(ByVal sender As Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs)
        If Not IsLogin Then Exit Sub
        If e.Item.Tag.ToString <> "" Then
            Dim perintah() As String
            perintah = Split(e.Item.Tag.ToString, ":")
            '
            If perintah(0).Trim.ToLower = "DaftarKartuStokperGudang".ToLower Then
                'KODE DIBAWAH UNTUK MEMBUKA FORM HANYA 1
                Dim frmEntri As frmDaftarKartuStok = Nothing
                Dim F As Object
                For Each F In MdiChildren
                    If TypeOf F Is frmDaftarKartuStok Then
                        frmEntri = F
                        Exit For
                    End If
                Next
                If frmEntri Is Nothing Then
                    frmEntri = New frmDaftarKartuStok
                    frmEntri.WindowState = FormWindowState.Maximized
                    frmEntri.MdiParent = Me
                End If
                frmEntri.Show()
                frmEntri.Focus()
                'laporan Hasil Stock Opname
            ElseIf perintah(0).Trim.ToLower = "LaporanDetilPenjualan".ToLower Then
                'KODE DIBAWAH UNTUK MEMBUKA FORM HANYA 1
                Dim frmEntri As frmLaporanPenjualanPromoDiskon = Nothing
                Dim F As Object
                For Each F In MdiChildren
                    If TypeOf F Is frmLUKas Then
                        frmEntri = F
                        Exit For
                    End If
                Next
                If frmEntri Is Nothing Then
                    frmEntri = New frmLaporanPenjualanPromoDiskon
                    frmEntri.WindowState = FormWindowState.Maximized
                    frmEntri.MdiParent = Me
                End If
                frmEntri.Show()
                frmEntri.Focus()
            ElseIf perintah(0).Trim.ToLower = "Daftarkasbank".ToLower Then
                'KODE DIBAWAH UNTUK MEMBUKA FORM HANYA 1
                Dim frmEntri As frmLUKas = Nothing
                Dim F As Object
                For Each F In MdiChildren
                    If TypeOf F Is frmLUKas Then
                        frmEntri = F
                        Exit For
                    End If
                Next
                If frmEntri Is Nothing Then
                    frmEntri = New frmLUKas
                    frmEntri.WindowState = FormWindowState.Maximized
                    frmEntri.MdiParent = Me
                End If
                frmEntri.PanelControl1.Visible = True
                frmEntri.Show()
                frmEntri.Focus()
            ElseIf perintah(0).Trim.ToLower = "DaftarAsset".ToLower Then
                'KODE DIBAWAH UNTUK MEMBUKA FORM HANYA 1
                Dim frmEntri As frDaftarAktivaTetap = Nothing
                Dim F As Object
                For Each F In MdiChildren
                    If TypeOf F Is frDaftarAktivaTetap Then
                        frmEntri = F
                        Exit For
                    End If
                Next
                If frmEntri Is Nothing Then
                    frmEntri = New frDaftarAktivaTetap
                    frmEntri.WindowState = FormWindowState.Maximized
                    frmEntri.MdiParent = Me
                End If
                'frmEntri.pStatus = Publik.ptipe.Lihat

                frmEntri.Show()
                frmEntri.Focus()
            ElseIf perintah(0).Trim.ToLower = "DaftarJurnalUmum".ToLower Then
                'KODE DIBAWAH UNTUK MEMBUKA FORM HANYA 1
                Dim frmEntri As frmDaftarJurnalUmum = Nothing
                Dim F As Object
                For Each F In MdiChildren
                    If TypeOf F Is frmDaftarJurnalUmum Then
                        frmEntri = F
                        Exit For
                    End If
                Next
                If frmEntri Is Nothing Then
                    frmEntri = New frmDaftarJurnalUmum
                    frmEntri.WindowState = FormWindowState.Maximized
                    frmEntri.MdiParent = Me
                End If
                'frmEntri.pStatus = Publik.ptipe.Lihat

                frmEntri.Show()
                frmEntri.Focus()
            ElseIf perintah(0).Trim.ToLower = "LaporanBukuBesar".ToLower Then
                'KODE DIBAWAH UNTUK MEMBUKA FORM HANYA 1
                Dim frmEntri As frDaftarBukuBesar = Nothing
                Dim F As Object
                For Each F In MdiChildren
                    If TypeOf F Is frDaftarBukuBesar Then
                        If TryCast(F, frDaftarBukuBesar).PtipeBukuBesar = frDaftarBukuBesar.PTipe.BukuBesar Then
                            frmEntri = F
                            Exit For
                        End If
                    End If
                Next
                If frmEntri Is Nothing Then
                    frmEntri = New frDaftarBukuBesar
                    frmEntri.WindowState = FormWindowState.Maximized
                    frmEntri.MdiParent = Me
                End If
                'frmEntri.pStatus = Publik.ptipe.Lihat

                frmEntri.PtipeBukuBesar = frDaftarBukuBesar.PTipe.BukuBesar
                frmEntri.Show()
                frmEntri.Focus()
            ElseIf perintah(0).Trim.ToLower = "LaporanJurnalKosong".ToLower Then
                'KODE DIBAWAH UNTUK MEMBUKA FORM HANYA 1
                Dim frmEntri As frDaftarBukuBesar = Nothing
                Dim F As Object
                For Each F In MdiChildren
                    If TypeOf F Is frDaftarBukuBesar Then
                        If TryCast(F, frDaftarBukuBesar).PtipeBukuBesar = frDaftarBukuBesar.PTipe.JurnalKosong Then
                            frmEntri = F
                            Exit For
                        End If
                    End If
                Next
                If frmEntri Is Nothing Then
                    frmEntri = New frDaftarBukuBesar
                    frmEntri.WindowState = FormWindowState.Maximized
                    frmEntri.MdiParent = Me
                End If
                'frmEntri.pStatus = Publik.ptipe.Lihat

                frmEntri.PtipeBukuBesar = frDaftarBukuBesar.PTipe.JurnalKosong
                frmEntri.Show()
                frmEntri.Focus()
            ElseIf perintah(0).Trim.ToLower = "LaporanJurnalTidakBalance".ToLower Then
                'KODE DIBAWAH UNTUK MEMBUKA FORM HANYA 1
                Dim frmEntri As frDaftarBukuBesar = Nothing
                Dim F As Object
                For Each F In MdiChildren
                    If TypeOf F Is frDaftarBukuBesar Then
                        If TryCast(F, frDaftarBukuBesar).PtipeBukuBesar = frDaftarBukuBesar.PTipe.JurnalTidakBalance Then
                            frmEntri = F
                            Exit For
                        End If
                    End If
                Next
                If frmEntri Is Nothing Then
                    frmEntri = New frDaftarBukuBesar
                    frmEntri.WindowState = FormWindowState.Maximized
                    frmEntri.MdiParent = Me
                End If
                'frmEntri.pStatus = Publik.ptipe.Lihat

                frmEntri.PtipeBukuBesar = frDaftarBukuBesar.PTipe.JurnalTidakBalance
                frmEntri.Show()
                frmEntri.Focus()
            ElseIf perintah(0).Trim.ToLower = "LaporanNeracaPercobaan".ToLower Then
                'KODE DIBAWAH UNTUK MEMBUKA FORM HANYA 1
                Dim frmEntri As frLaporanMutasiBukuBesar = Nothing
                Dim F As Object
                For Each F In MdiChildren
                    If TypeOf F Is frLaporanMutasiBukuBesar Then
                        frmEntri = F
                        Exit For
                    End If
                Next
                If frmEntri Is Nothing Then
                    frmEntri = New frLaporanMutasiBukuBesar
                    frmEntri.WindowState = FormWindowState.Maximized
                    frmEntri.MdiParent = Me
                End If
                'frmEntri.pStatus = Publik.ptipe.Lihat

                frmEntri.Show()
                frmEntri.Focus()
            ElseIf perintah(0).Trim.ToLower = "LaporanLabaRugi".ToLower Then
                'KODE DIBAWAH UNTUK MEMBUKA FORM HANYA 1
                Dim frmEntri As frLaporanLabaRugi = Nothing
                Dim F As Object
                For Each F In MdiChildren
                    If TypeOf F Is frLaporanLabaRugi Then
                        frmEntri = F
                        Exit For
                    End If
                Next
                If frmEntri Is Nothing Then
                    frmEntri = New frLaporanLabaRugi
                    frmEntri.WindowState = FormWindowState.Maximized
                    frmEntri.MdiParent = Me
                End If
                'frmEntri.pStatus = Publik.ptipe.Lihat

                frmEntri.Show()
                frmEntri.Focus()
            ElseIf perintah(0).Trim.ToLower = "LaporanNeraca".ToLower Then
                'KODE DIBAWAH UNTUK MEMBUKA FORM HANYA 1
                Dim frmEntri As frLaporanNeraca = Nothing
                Dim F As Object
                For Each F In MdiChildren
                    If TypeOf F Is frLaporanNeraca Then
                        frmEntri = F
                        Exit For
                    End If
                Next
                If frmEntri Is Nothing Then
                    frmEntri = New frLaporanNeraca
                    frmEntri.WindowState = FormWindowState.Maximized
                    frmEntri.MdiParent = Me
                End If
                'frmEntri.pStatus = Publik.ptipe.Lihat

                frmEntri.Show()
                frmEntri.Focus()

            ElseIf perintah(0).Trim.ToLower = "DaftarPenyusutanAsset".ToLower Then
                'KODE DIBAWAH UNTUK MEMBUKA FORM HANYA 1
                Dim frmEntri As frDaftarPenyusutanAktiva = Nothing
                Dim F As Object
                For Each F In MdiChildren
                    If TypeOf F Is frDaftarPenyusutanAktiva Then
                        frmEntri = F
                        Exit For
                    End If
                Next
                If frmEntri Is Nothing Then
                    frmEntri = New frDaftarPenyusutanAktiva
                    frmEntri.WindowState = FormWindowState.Maximized
                    frmEntri.MdiParent = Me
                End If
                frmEntri.Show()
                frmEntri.Focus()
            ElseIf perintah(0).Trim.ToLower = "DaftarAkun".ToLower Then
                'KODE DIBAWAH UNTUK MEMBUKA FORM HANYA 1
                Dim frmEntri As frmLUAkun = Nothing
                Dim F As Object
                For Each F In MdiChildren
                    If TypeOf F Is frmLUAkun Then
                        frmEntri = F
                        Exit For
                    End If
                Next
                If frmEntri Is Nothing Then
                    frmEntri = New frmLUAkun
                    frmEntri.WindowState = FormWindowState.Maximized
                    frmEntri.MdiParent = Me
                End If
                frmEntri.pStatus = mdlAccPublik.ptipe.Lihat

                frmEntri.Show()
                frmEntri.Focus()
            ElseIf perintah(0).Trim.ToLower = "DaftarSubKlasAkun".ToLower Then
                'KODE DIBAWAH UNTUK MEMBUKA FORM HANYA 1
                Dim frmEntri As frmLUSubKlasAkun = Nothing
                Dim F As Object
                For Each F In MdiChildren
                    If TypeOf F Is frmLUSubKlasAkun Then
                        frmEntri = F
                        Exit For
                    End If
                Next
                If frmEntri Is Nothing Then
                    frmEntri = New frmLUSubKlasAkun
                    frmEntri.WindowState = FormWindowState.Maximized
                    frmEntri.MdiParent = Me
                End If
                frmEntri.pStatus = mdlAccPublik.ptipe.Lihat

                frmEntri.Show()
                frmEntri.Focus()
            ElseIf perintah(0).Trim.ToLower = "DaftarKlasAkun".ToLower Then
                'KODE DIBAWAH UNTUK MEMBUKA FORM HANYA 1
                Dim frmEntri As frmLUKlasAkun = Nothing
                Dim F As Object
                For Each F In MdiChildren
                    If TypeOf F Is frmLUKlasAkun Then
                        frmEntri = F
                        Exit For
                    End If
                Next
                If frmEntri Is Nothing Then
                    frmEntri = New frmLUKlasAkun
                    frmEntri.WindowState = FormWindowState.Maximized
                    frmEntri.MdiParent = Me
                End If
                frmEntri.pStatus = mdlAccPublik.ptipe.Lihat

                frmEntri.Show()
                frmEntri.Focus()
            ElseIf perintah(0).Trim.ToLower = "DaftarKasKeluar".ToLower Then
                'KODE DIBAWAH UNTUK MEMBUKA FORM HANYA 1
                Dim frmEntri As frmDaftarKasOut = Nothing
                Dim F As Object
                For Each F In MdiChildren
                    If TypeOf F Is frmDaftarKasOut Then
                        frmEntri = F
                        Exit For
                    End If
                Next
                If frmEntri Is Nothing Then
                    frmEntri = New frmDaftarKasOut
                    frmEntri.WindowState = FormWindowState.Maximized
                    frmEntri.MdiParent = Me
                End If
                frmEntri.Show()
                frmEntri.Focus()
            ElseIf perintah(0).Trim.ToLower = "DaftarKasMasuk".ToLower Then
                'KODE DIBAWAH UNTUK MEMBUKA FORM HANYA 1
                Dim frmEntri As frmDaftarKasIN = Nothing
                Dim F As Object
                For Each F In MdiChildren
                    If TypeOf F Is frmDaftarKasIN Then
                        frmEntri = F
                        Exit For
                    End If
                Next
                If frmEntri Is Nothing Then
                    frmEntri = New frmDaftarKasIN
                    frmEntri.WindowState = FormWindowState.Maximized
                    frmEntri.MdiParent = Me
                End If
                frmEntri.Show()
                frmEntri.Focus()
            ElseIf perintah(0).Trim.ToLower = "LaporanStockOpnameD".ToLower Then
                'KODE DIBAWAH UNTUK MEMBUKA FORM HANYA 1
                Dim frmEntri As frmLaporanStockOpnameD = Nothing
                Dim F As Object
                For Each F In MdiChildren
                    If TypeOf F Is frmLaporanStockOpnameD Then
                        frmEntri = F
                        Exit For
                    End If
                Next
                If frmEntri Is Nothing Then
                    frmEntri = New frmLaporanStockOpnameD
                    frmEntri.WindowState = FormWindowState.Maximized
                    frmEntri.MdiParent = Me
                End If
                frmEntri.Show()
                frmEntri.Focus()

            ElseIf perintah(0).Trim.ToLower = "LaporanCrew".ToLower Then
                Dim frmEntri As New frmLihatPenjualanGuide
                frmEntri.ShowDialog(Me)
                frmEntri.Dispose()

                'KODE DIBAWAH UNTUK MEMBUKA FORM HANYA 1
                'Dim frmEntri As frmLihatPenjualanGuide = Nothing
                'Dim F As Object
                'For Each F In MdiChildren
                '    If TypeOf F Is frmLihatPenjualanGuide Then
                '        frmEntri = F
                '        Exit For
                '    End If
                'Next
                'If frmEntri Is Nothing Then
                '    frmEntri = New frmLihatPenjualanGuide
                '    frmEntri.WindowState = FormWindowState.Maximized
                '    frmEntri.MdiParent = Me
                'End If
                'frmEntri.Show()
                'frmEntri.Focus()

                'DaftarPPNMasukkan
            ElseIf perintah(0).Trim.ToLower = "DaftarPPNMasukkan".ToLower Then
                'KODE DIBAWAH UNTUK MEMBUKA FORM HANYA 1
                Dim frmEntri As frmDaftarPPNMasukkan = Nothing
                Dim F As Object
                For Each F In MdiChildren
                    If TypeOf F Is frmDaftarPPNMasukkan Then
                        frmEntri = F
                        Exit For
                    End If
                Next
                If frmEntri Is Nothing Then
                    frmEntri = New frmDaftarPPNMasukkan
                    frmEntri.WindowState = FormWindowState.Maximized
                    frmEntri.MdiParent = Me
                End If
                frmEntri.Show()
                frmEntri.Focus()
            ElseIf perintah(0).Trim.ToLower = "DaftarKartuStokVarianPerGudang".ToLower Then
                'KODE DIBAWAH UNTUK MEMBUKA FORM HANYA 1
                Dim frmEntri As frmDaftarKartuStokVarian = Nothing
                Dim F As Object
                For Each F In MdiChildren
                    If TypeOf F Is frmDaftarKartuStokVarian Then
                        frmEntri = F
                        Exit For
                    End If
                Next
                If frmEntri Is Nothing Then
                    frmEntri = New frmDaftarKartuStokVarian
                    frmEntri.WindowState = FormWindowState.Maximized
                    frmEntri.MdiParent = Me
                End If
                frmEntri.Show()
                frmEntri.Focus()
                'RekapPenjualanPerDepartemen
            ElseIf perintah(0).Trim.ToLower = "RekapPenjualanPerDepartemen".ToLower Then
                'KODE DIBAWAH UNTUK MEMBUKA FORM HANYA 1
                Dim frmEntri As frmLaporanPenjualanPerDepartemen = Nothing
                Dim F As Object
                For Each F In MdiChildren
                    If TypeOf F Is frmLaporanPenjualanPerDepartemen Then
                        frmEntri = F
                        Exit For
                    End If
                Next
                If frmEntri Is Nothing Then
                    frmEntri = New frmLaporanPenjualanPerDepartemen
                    frmEntri.WindowState = FormWindowState.Maximized
                    frmEntri.MdiParent = Me
                End If
                frmEntri.Show()
                frmEntri.Focus()

                'LaporanRekapSaldoHutang
            ElseIf perintah(0).Trim.ToLower = "LaporanKartuPiutang".ToLower Then
                'KODE DIBAWAH UNTUK MEMBUKA FORM HANYA 1
                Dim frmEntri As frmLaporanKartuHutangPerSupplier = Nothing
                Dim F As Object
                For Each F In MdiChildren
                    If TypeOf F Is frmLaporanKartuHutangPerSupplier Then
                        frmEntri = F
                        Exit For
                    End If
                Next
                If frmEntri Is Nothing Then
                    frmEntri = New frmLaporanKartuHutangPerSupplier
                    frmEntri.WindowState = FormWindowState.Maximized
                    frmEntri.MdiParent = Me
                End If
                frmEntri.Show()
                frmEntri.Focus()
            ElseIf perintah(0).Trim.ToLower = "LaporanKartuHutang".ToLower Then
                'KODE DIBAWAH UNTUK MEMBUKA FORM HANYA 1
                Dim frmEntri As frmLaporanKartuHutangPerSupplier = Nothing
                Dim F As Object
                For Each F In MdiChildren
                    If TypeOf F Is frmLaporanKartuHutangPerSupplier Then
                        frmEntri = F
                        Exit For
                    End If
                Next
                If frmEntri Is Nothing Then
                    frmEntri = New frmLaporanKartuHutangPerSupplier
                    frmEntri.WindowState = FormWindowState.Maximized
                    frmEntri.MdiParent = Me
                End If
                frmEntri.Show()
                frmEntri.Focus()
            ElseIf perintah(0).Trim.ToLower = "LaporanKartuHutangGroup".ToLower Then
                'KODE DIBAWAH UNTUK MEMBUKA FORM HANYA 1
                Dim frmEntri As frmLaporanKartuHutangPerGroupSupplier = Nothing
                Dim F As Object
                For Each F In MdiChildren
                    If TypeOf F Is frmLaporanKartuHutangPerGroupSupplier Then
                        frmEntri = F
                        Exit For
                    End If
                Next
                If frmEntri Is Nothing Then
                    frmEntri = New frmLaporanKartuHutangPerGroupSupplier
                    frmEntri.WindowState = FormWindowState.Maximized
                    frmEntri.MdiParent = Me
                End If
                frmEntri.Show()
                frmEntri.Focus()
            ElseIf perintah(0).Trim.ToLower = "LaporanOmzetAccBonus".ToLower Then
                'KODE DIBAWAH UNTUK MEMBUKA FORM HANYA 1
                Dim frmEntri As frmLaporanOmzetBonus = Nothing
                Dim F As Object
                For Each F In MdiChildren
                    If TypeOf F Is frmLaporanOmzetBonus Then
                        frmEntri = F
                        Exit For
                    End If
                Next
                If frmEntri Is Nothing Then
                    frmEntri = New frmLaporanOmzetBonus
                    frmEntri.WindowState = FormWindowState.Maximized
                    frmEntri.MdiParent = Me
                End If
                frmEntri.Show()
                frmEntri.Focus()
            ElseIf perintah(0).Trim.ToLower = "LaporanRekapSaldoHutang".ToLower Then
                'KODE DIBAWAH UNTUK MEMBUKA FORM HANYA 1
                Dim frmEntri As frmLaporanSaldoHutangPerSupplier = Nothing
                Dim F As Object
                For Each F In MdiChildren
                    If TypeOf F Is frmLaporanSaldoHutangPerSupplier Then
                        frmEntri = F
                        Exit For
                    End If
                Next
                If frmEntri Is Nothing Then
                    frmEntri = New frmLaporanSaldoHutangPerSupplier
                    frmEntri.WindowState = FormWindowState.Maximized
                    frmEntri.MdiParent = Me
                End If
                frmEntri.Show()
                frmEntri.Focus()
            ElseIf perintah(0).Trim.ToLower = "LaporanPerbandinganQtyPembelianDanPenjualan".ToLower Then
                'KODE DIBAWAH UNTUK MEMBUKA FORM HANYA 1
                Dim frmEntri As frmLaporanPembelianvsPenjualan = Nothing
                Dim F As Object
                For Each F In MdiChildren
                    If TypeOf F Is frmLaporanPembelianvsPenjualan Then
                        frmEntri = F
                        Exit For
                    End If
                Next
                If frmEntri Is Nothing Then
                    frmEntri = New frmLaporanPembelianvsPenjualan
                    frmEntri.WindowState = FormWindowState.Maximized
                    frmEntri.MdiParent = Me
                End If
                frmEntri.Show()
                frmEntri.Focus()
                'CekHargaJual
            ElseIf perintah(0).Trim.ToLower = "LaporanSaldoStockPerVarian".ToLower Then
                'KODE DIBAWAH UNTUK MEMBUKA FORM HANYA 1
                Dim frmEntri As frmLaporanSaldoStokPerVarian = Nothing
                Dim F As Object
                For Each F In MdiChildren
                    If TypeOf F Is frmLaporanSaldoStokPerVarian Then
                        frmEntri = F
                        Exit For
                    End If
                Next
                If frmEntri Is Nothing Then
                    frmEntri = New frmLaporanSaldoStokPerVarian
                    frmEntri.WindowState = FormWindowState.Maximized
                    frmEntri.MdiParent = Me
                End If
                frmEntri.Show()
                frmEntri.Focus()
                'CekHargaJual
            ElseIf perintah(0).Trim.ToLower = "LaporanMutasiStock".ToLower Then
                'KODE DIBAWAH UNTUK MEMBUKA FORM HANYA 1
                Dim frmEntri As frmLaporanMutasiStock = Nothing
                Dim F As Object
                For Each F In MdiChildren
                    If TypeOf F Is frmLaporanMutasiStock Then
                        frmEntri = F
                        Exit For
                    End If
                Next
                If frmEntri Is Nothing Then
                    frmEntri = New frmLaporanMutasiStock
                    frmEntri.WindowState = FormWindowState.Maximized
                    frmEntri.MdiParent = Me
                End If
                frmEntri.Show()
                frmEntri.Focus()
            ElseIf perintah(0).Trim.ToLower = "LaporanSaldoStock".ToLower Then
                'KODE DIBAWAH UNTUK MEMBUKA FORM HANYA 1
                Dim frmEntri As frmLaporanSaldoStok = Nothing
                Dim F As Object
                For Each F In MdiChildren
                    If TypeOf F Is frmLaporanSaldoStok Then
                        frmEntri = F
                        Exit For
                    End If
                Next
                If frmEntri Is Nothing Then
                    frmEntri = New frmLaporanSaldoStok
                    frmEntri.WindowState = FormWindowState.Maximized
                    frmEntri.MdiParent = Me
                End If
                frmEntri.Show()
                frmEntri.Focus()
                'CekHargaJual
            ElseIf perintah(0).Trim.ToLower = "CekHargaJual".ToLower Then
                Dim x As New FrmCekHargaJual
                Try
                    If x.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then

                    End If
                Catch ex As Exception
                    XtraMessageBox.Show("Kesalahan : " & ex.Message)
                Finally
                    x.Dispose()
                End Try
            ElseIf perintah(0).Trim.ToLower = "CekHargaBeli".ToLower Then
                Dim x As New FrmCekHargaBeli
                Try
                    If x.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then

                    End If
                Catch ex As Exception
                    XtraMessageBox.Show("Kesalahan : " & ex.Message)
                Finally
                    x.Dispose()
                End Try
            ElseIf perintah(0).Trim.ToLower = "CekPoinMember".ToLower Then
                Dim x As New frmCekPoin
                Try
                    If x.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then

                    End If
                Catch ex As Exception
                    XtraMessageBox.Show("Kesalahan : " & ex.Message)
                Finally
                    x.Dispose()
                End Try
            ElseIf perintah(0).Trim.ToLower = "LaporanTopTenSales".ToLower Then
                Dim frmEntri As frmLaporanTopTenSales = Nothing
                Dim F As Object
                For Each F In MdiChildren
                    If TypeOf F Is frmLaporanTopTenSales Then
                        frmEntri = F
                        Exit For
                    End If
                Next
                If frmEntri Is Nothing Then
                    frmEntri = New frmLaporanTopTenSales
                    frmEntri.WindowState = FormWindowState.Maximized
                    frmEntri.MdiParent = Me
                End If
                frmEntri.Show()
                frmEntri.Focus()
            ElseIf perintah(0).Trim.ToLower = "LaporanRekapPenjualanPerCustomer".ToLower Then
                Dim frmEntri As frmLaporanRekapPenjualanPerCustomer = Nothing
                Dim F As Object
                For Each F In MdiChildren
                    If TypeOf F Is frmLaporanRekapPenjualanPerCustomer Then
                        frmEntri = F
                        Exit For
                    End If
                Next
                If frmEntri Is Nothing Then
                    frmEntri = New frmLaporanRekapPenjualanPerCustomer
                    frmEntri.WindowState = FormWindowState.Maximized
                    frmEntri.MdiParent = Me
                End If
                frmEntri.Show()
                frmEntri.Focus()
            ElseIf perintah(0).Trim.ToLower = "LaporanDetilPenjualanPerCustomer".ToLower Then
                Dim frmEntri As frmLaporanDetilPenjualanPerCustomer = Nothing
                Dim F As Object
                For Each F In MdiChildren
                    If TypeOf F Is frmLaporanRekapPenjualanPerCustomer Then
                        frmEntri = F
                        Exit For
                    End If
                Next
                If frmEntri Is Nothing Then
                    frmEntri = New frmLaporanDetilPenjualanPerCustomer
                    frmEntri.WindowState = FormWindowState.Maximized
                    frmEntri.MdiParent = Me
                End If
                frmEntri.Show()
                frmEntri.Focus()
            ElseIf perintah(0).Trim.ToLower = "LaporanPenjualanPerJam".ToLower Then
                Dim frmEntri As frmLaporanPenjualanPerJam = Nothing
                Dim F As Object
                For Each F In MdiChildren
                    If TypeOf F Is frmLaporanPenjualanPerJam Then
                        frmEntri = F
                        Exit For
                    End If
                Next
                If frmEntri Is Nothing Then
                    frmEntri = New frmLaporanPenjualanPerJam
                    frmEntri.WindowState = FormWindowState.Maximized
                    frmEntri.MdiParent = Me
                End If
                frmEntri.Show()
                frmEntri.Focus()
            ElseIf perintah(0).Trim.ToLower = "RekapPenjualanPerDepartemenBulanan".ToLower Then
                Dim frmEntri As frmLaporanRekapPenjualanPerDepartemen = Nothing
                Dim F As Object
                For Each F In MdiChildren
                    If TypeOf F Is frmLaporanRekapPenjualanPerDepartemen Then
                        frmEntri = F
                        Exit For
                    End If
                Next
                If frmEntri Is Nothing Then
                    frmEntri = New frmLaporanRekapPenjualanPerDepartemen
                    frmEntri.WindowState = FormWindowState.Maximized
                    frmEntri.MdiParent = Me
                End If
                frmEntri.Show()
                frmEntri.Focus()
            ElseIf perintah(0).Trim.ToLower = "RekapPenjualanPerDepartemenTahunan".ToLower Then
                Dim frmEntri As frmLaporanRekapPenjualanPerDepartemenTahunan = Nothing
                Dim F As Object
                For Each F In MdiChildren
                    If TypeOf F Is frmLaporanRekapPenjualanPerDepartemenTahunan Then
                        frmEntri = F
                        Exit For
                    End If
                Next
                If frmEntri Is Nothing Then
                    frmEntri = New frmLaporanRekapPenjualanPerDepartemenTahunan
                    frmEntri.WindowState = FormWindowState.Maximized
                    frmEntri.MdiParent = Me
                End If
                frmEntri.Show()
                frmEntri.Focus()
            ElseIf perintah(0).Trim.ToLower = "LaporanReturPembelian".ToLower Then
                Dim frmEntri As frmLaporanReturPembelianPerSupplier = Nothing
                Dim F As Object
                For Each F In MdiChildren
                    If TypeOf F Is frmLaporanReturPembelianPerSupplier Then
                        frmEntri = F
                        Exit For
                    End If
                Next
                If frmEntri Is Nothing Then
                    frmEntri = New frmLaporanReturPembelianPerSupplier
                    frmEntri.WindowState = FormWindowState.Maximized
                    frmEntri.MdiParent = Me
                End If
                frmEntri.Show()
                frmEntri.Focus()
            ElseIf perintah(0).Trim.ToLower = "LaporanPembelianPerDepartemenBulanan".ToLower Then
                Dim frmEntri As frmLaporanPembelianPerDepartemenBulanan = Nothing
                Dim F As Object
                For Each F In MdiChildren
                    If TypeOf F Is frmLaporanPembelianPerDepartemenBulanan Then
                        frmEntri = F
                        Exit For
                    End If
                Next
                If frmEntri Is Nothing Then
                    frmEntri = New frmLaporanPembelianPerDepartemenBulanan
                    frmEntri.WindowState = FormWindowState.Maximized
                    frmEntri.MdiParent = Me
                End If
                frmEntri.Show()
                frmEntri.Focus()
            ElseIf perintah(0).Trim.ToLower = "LaporanPembelianPerSupplierBulanan".ToLower Then
                Dim frmEntri As frmLaporanPembelianPerSupplierBulanan = Nothing
                Dim F As Object
                For Each F In MdiChildren
                    If TypeOf F Is frmLaporanPembelianPerSupplierBulanan Then
                        frmEntri = F
                        Exit For
                    End If
                Next
                If frmEntri Is Nothing Then
                    frmEntri = New frmLaporanPembelianPerSupplierBulanan
                    frmEntri.WindowState = FormWindowState.Maximized
                    frmEntri.MdiParent = Me
                End If
                frmEntri.Show()
                frmEntri.Focus()
            ElseIf perintah(0).Trim.ToLower = "LaporanLabaKotorPerBarang".ToLower Then
                Dim frmEntri As frmLaporanLabaKotorPerBarang = Nothing
                Dim F As Object
                For Each F In MdiChildren
                    If TypeOf F Is frmLaporanLabaKotorPerBarang Then
                        frmEntri = F
                        Exit For
                    End If
                Next
                If frmEntri Is Nothing Then
                    frmEntri = New frmLaporanLabaKotorPerBarang
                    frmEntri.WindowState = FormWindowState.Maximized
                    frmEntri.MdiParent = Me
                End If
                frmEntri.Show()
                frmEntri.Focus()
            ElseIf perintah(0).Trim.ToLower = "LaporanLabaKotorPerKategori".ToLower Then
                Dim frmEntri As frmLaporanLabaKotorPerKategori = Nothing
                Dim F As Object
                For Each F In MdiChildren
                    If TypeOf F Is frmLaporanLabaKotorPerKategori Then
                        frmEntri = F
                        Exit For
                    End If
                Next
                If frmEntri Is Nothing Then
                    frmEntri = New frmLaporanLabaKotorPerKategori
                    frmEntri.WindowState = FormWindowState.Maximized
                    frmEntri.MdiParent = Me
                End If
                frmEntri.Show()
                frmEntri.Focus()
            ElseIf perintah(0).Trim.ToLower = "laporanomsetpersupplierperdepartemen".ToLower Then
                Dim frmEntri As frmLaporanOmzetPerSupplierPerDepartemen = Nothing
                Dim F As Object
                For Each F In MdiChildren
                    If TypeOf F Is frmLaporanOmzetPerSupplierPerDepartemen Then
                        frmEntri = F
                        Exit For
                    End If
                Next
                If frmEntri Is Nothing Then
                    frmEntri = New frmLaporanOmzetPerSupplierPerDepartemen
                    frmEntri.WindowState = FormWindowState.Maximized
                    frmEntri.MdiParent = Me
                End If
                frmEntri.Show()
                frmEntri.Focus()
                'ElseIf perintah(0).Trim.ToLower = "LaporanLabaKotorPerHari".ToLower Then
                '    Dim frmEntri As frmLaporanLabaKotorPerhari = Nothing
                '    Dim F As Object
                '    For Each F In MdiChildren
                '        If TypeOf F Is frmLaporanLabaKotorPerhari Then
                '            frmEntri = F
                '            Exit For
                '        End If
                '    Next
                '    If frmEntri Is Nothing Then
                '        frmEntri = New frmLaporanLabaKotorPerhari
                '        frmEntri.WindowState = FormWindowState.Maximized
                '        frmEntri.MdiParent = Me
                '    End If
                '    frmEntri.Show()
                '    frmEntri.Focus()
            ElseIf perintah(0).Trim.ToLower = "DownloadPos".ToLower Then
                Dim x As New frmDownloadPenjualanKasir
                Try
                    If x.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then

                    End If
                Catch ex As Exception
                    XtraMessageBox.Show("Kesalahan : " & ex.Message)
                Finally
                    x.Dispose()
                End Try

            ElseIf perintah(0).Trim.ToLower = "RubahMasterJenis".ToLower Then
                Dim x As New frmRubahJenisBarang
                Try
                    If x.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then

                    End If
                Catch ex As Exception
                    XtraMessageBox.Show("Kesalahan : " & ex.Message)
                Finally
                    x.Dispose()
                End Try
            ElseIf perintah(0).Trim.ToLower = "DaftarPembayaranHutang".ToLower Then
                'KODE DIBAWAH UNTUK MEMBUKA FORM HANYA 1
                Dim frmEntri As frBayarHutang = Nothing
                Dim F As Object
                For Each F In MdiChildren
                    If TypeOf F Is frBayarHutang Then
                        frmEntri = F
                        Exit For
                    End If
                Next
                If frmEntri Is Nothing Then
                    frmEntri = New frBayarHutang
                    frmEntri.WindowState = FormWindowState.Maximized
                    frmEntri.MdiParent = Me
                End If
                frmEntri.Show()
                frmEntri.Focus()
            ElseIf perintah(0).Trim.ToLower = "DaftarHutangSupplier".ToLower Then
                'KODE DIBAWAH UNTUK MEMBUKA FORM HANYA 1
                Dim frmEntri As frmDaftarBayarHutang = Nothing
                Dim F As Object
                For Each F In MdiChildren
                    If TypeOf F Is frmDaftarBayarHutang Then
                        frmEntri = F
                        If frmEntri.IsHutang Then
                            Exit For
                        Else
                            frmEntri = Nothing
                        End If
                        Exit For
                    End If
                Next
                If frmEntri Is Nothing Then
                    frmEntri = New frmDaftarBayarHutang
                    frmEntri.WindowState = FormWindowState.Maximized
                    frmEntri.MdiParent = Me
                    frmEntri.IsHutang = True
                End If
                frmEntri.Show()
                frmEntri.Focus()
            ElseIf perintah(0).Trim.ToLower = "DaftarPembayaranPiutang".ToLower Then
                'KODE DIBAWAH UNTUK MEMBUKA FORM HANYA 1
                Dim frmEntri As frBayarPiutang = Nothing
                Dim F As Object
                For Each F In MdiChildren
                    If TypeOf F Is frBayarPiutang Then
                        frmEntri = F
                        Exit For
                    End If
                Next
                If frmEntri Is Nothing Then
                    frmEntri = New frBayarPiutang
                    frmEntri.WindowState = FormWindowState.Maximized
                    frmEntri.MdiParent = Me
                End If
                frmEntri.Show()
                frmEntri.Focus()
            ElseIf perintah(0).Trim.ToLower = "DaftarPiutangCustomer".ToLower Then
                'KODE DIBAWAH UNTUK MEMBUKA FORM HANYA 1
                Dim frmEntri As frmDaftarBayarHutang = Nothing
                Dim F As Object
                For Each F In MdiChildren
                    If TypeOf F Is frmDaftarBayarHutang Then
                        frmEntri = F
                        If Not frmEntri.IsHutang Then
                            Exit For
                        Else
                            frmEntri = Nothing
                        End If
                        Exit For
                    End If
                Next
                If frmEntri Is Nothing Then
                    frmEntri = New frmDaftarBayarHutang
                    frmEntri.WindowState = FormWindowState.Maximized
                    frmEntri.MdiParent = Me
                    frmEntri.IsHutang = False
                End If
                frmEntri.Show()
                frmEntri.Focus()
            ElseIf perintah(0).Trim.ToLower = "DaftarLapPiutangPerCustomer".ToLower Then
                'KODE DIBAWAH UNTUK MEMBUKA FORM HANYA 1
                Dim frmEntri As frmDaftarPiutangPerCustomer = Nothing
                Dim F As Object
                For Each F In MdiChildren
                    If TypeOf F Is frmDaftarPiutangPerCustomer Then
                        frmEntri = F
                        Exit For
                    End If
                Next
                If frmEntri Is Nothing Then
                    frmEntri = New frmDaftarPiutangPerCustomer
                    frmEntri.WindowState = FormWindowState.Maximized
                    frmEntri.MdiParent = Me
                End If
                frmEntri.Show()
                frmEntri.Focus()
            ElseIf perintah(0).Trim.ToLower = "DaftarBarang".ToLower Then
                'KODE DIBAWAH UNTUK MEMBUKA FORM HANYA 1
                Dim frmEntri As frmDaftarBarang = Nothing
                Dim F As Object
                For Each F In MdiChildren
                    If TypeOf F Is frmDaftarBarang Then
                        frmEntri = F
                        Exit For
                    End If
                Next
                If frmEntri Is Nothing Then
                    frmEntri = New frmDaftarBarang
                    frmEntri.WindowState = FormWindowState.Maximized
                    frmEntri.MdiParent = Me
                End If
                frmEntri.Show()
                frmEntri.Focus()
            ElseIf perintah(0).Trim.ToLower = "DaftarAlamat".ToLower Then
                'KODE DIBAWAH UNTUK MEMBUKA FORM HANYA 1
                Dim frmEntri As frmDaftarAlamat = Nothing
                Dim F As Object
                For Each F In MdiChildren
                    If TypeOf F Is frmDaftarAlamat Then
                        frmEntri = F
                        Exit For
                    End If
                Next
                If frmEntri Is Nothing Then
                    frmEntri = New frmDaftarAlamat
                    frmEntri.WindowState = FormWindowState.Maximized
                    frmEntri.MdiParent = Me
                End If
                frmEntri.Show()
                frmEntri.Focus()
            ElseIf perintah(0).Trim.ToLower = "DaftarJenisBarang".ToLower Then
                'KODE DIBAWAH UNTUK MEMBUKA FORM HANYA 1
                Dim frmEntri As frmDaftarJenisBarang = Nothing
                Dim F As Object
                For Each F In MdiChildren
                    If TypeOf F Is frmDaftarJenisBarang Then
                        frmEntri = F
                        Exit For
                    End If
                Next
                If frmEntri Is Nothing Then
                    frmEntri = New frmDaftarJenisBarang
                    frmEntri.WindowState = FormWindowState.Maximized
                    frmEntri.MdiParent = Me
                End If
                frmEntri.Show()
                frmEntri.Focus()
            ElseIf perintah(0).Trim.ToLower = "DaftarBarcodeV2".ToLower Then
                'KODE DIBAWAH UNTUK MEMBUKA FORM HANYA 1
                Dim frmEntri As New frmCetakBarcodeVer2
                Try
                    If frmEntri.ShowDialog(Me) Then

                    End If
                Catch ex As Exception
                    XtraMessageBox.Show("Info " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
                Finally
                    frmEntri.Dispose()
                End Try
            ElseIf perintah(0).Trim.ToLower = "DaftarBarcode".ToLower Then
                'KODE DIBAWAH UNTUK MEMBUKA FORM HANYA 1
                Dim frmEntri As New frmCetakBarcode
                Try
                    If frmEntri.ShowDialog(Me) Then

                    End If
                Catch ex As Exception
                    XtraMessageBox.Show("Info " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
                Finally
                    frmEntri.Dispose()
                End Try
            ElseIf UBound(perintah) >= 0 Then
                ExecuteFormbyCommand(perintah(0).Trim)
            End If
        Else
            XtraMessageBox.Show("Perintah menu " & e.Item.Caption & " (" & e.Item.Name & ") belum disetting!", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If
    End Sub

    Private Sub mnexit_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles mnexit.ItemClick
        'If MsgBox("Yakin mau keluar aplikasi?", MsgBoxStyle.YesNo + MsgBoxStyle.Question) = MsgBoxResult.Yes Then
        '    Application.Exit()
        'End If
        Close()
    End Sub

    Private Sub mnloginout_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles mnloginout.ItemClick
        Login(mnloginout)
    End Sub

    Private Sub Login(ByVal e As Object)
        If TryCast(e, BarBaseButtonItem).Caption = "&Logout" Then
            If XtraMessageBox.Show("Yakin mau menutup form?", NamaAplikasi, MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                For Each frm In Me.MdiChildren
                    If Not TypeOf frm Is frmDashBoard Then
                        frm.Close()
                    End If
                Next
                For Each mn As BarSubItemLink In barTemp.ItemLinks
                    Bar1.ItemLinks.Clear()
                    mnBarSubMaster.ItemLinks.Clear()
                Next
                barTemp.ClearLinks()
                mnloginout.Caption = "&Login"
                IsLogin = False

                mnStatusUser.Caption = "Login : (None)"
                mnStatusGudang.Caption = "Gudang : (None)"
                mnStatusRole.Caption = "Role : (None)"
                mnStatusServer.Caption = "Server : (None)"
                HapusLogUser()

                IDUserAktif = -1
                DefIDDepartemen = -1
                DefIDWilayah = -1
                DefIDGudang = -1
                DefIDGudangCustomer = -1
                DefIDGudangSupplier = -1
                DefIDSatuan = -1
                DefIDPegawai = -1
                DefIDCustomer = -1
                DefIDSupplier = -1
                NamaUserAktif = ""
                KodeUserAktif = ""
                IsSupervisor = False
                IsSupervisor = False
                IsEditLayout = False
                IsKasir = False
                IsAutoPosting = False
                IsAccMutasi = False

                mnVPOS.Visibility = BarItemVisibility.Never
                mnDatabase.Visibility = BarItemVisibility.Never
                mnDeveloper.Visibility = BarItemVisibility.Never
                mnSetting.Visibility = BarItemVisibility.Never

                DashBoard.TampilkanLinkBerdasarkanUser()
                Timer1.Enabled = False
            End If
        Else
            Dim x As New frmLogin
            If x.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
                frmServiceSQLServer.NotifyIcon1.BalloonTipText = "Selamat bertugas " & NamaUserAktif & "!, gunakan aplikasi dengan penuh tanggung jawab."
                frmServiceSQLServer.NotifyIcon1.ShowBalloonTip(10)
                'XtraMessageBox.Show(, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
                mnloginout.Caption = "&Logout"
                IsUpdateHargaBeli = NullToBool(EksekusiSQlSkalarNew("SELECT UpdateHargaBeli FROM MSetting"))
                AktivekanMenubyUser()

                NamaPerusahaan = NullToStr(EksekusiSQlSkalarNew("SELECT NamaPerusahaan FROM MSetting"))
                AlamatPerusahaan = NullToStr(EksekusiSQlSkalarNew("SELECT NamaPerusahaan FROM MSetting"))
                FormatFP = NullToStr(EksekusiSQlSkalarNew("SELECT FormatFP FROM MSetting"))
                TglDitetapkanSO = NullToDate(EksekusiSQlSkalarNew("SELECT TglDitetapkanSO FROM MSetting"))

                mnStatusServer.Caption = "Server : " & BacaIni("dbconfig", "Server", "(local)").ToLower

                If NullToBool(EksekusiSQlSkalarNew("SELECT IsMnVPOS FROM MUser WHERE NoID=" & IDUserAktif)) Then
                    mnVPOS.Visibility = BarItemVisibility.Always
                Else
                    mnVPOS.Visibility = BarItemVisibility.Never
                End If
                If NullToBool(EksekusiSQlSkalarNew("SELECT IsMnDatabase FROM MUser WHERE NoID=" & IDUserAktif)) Then
                    mnDatabase.Visibility = BarItemVisibility.Always
                Else
                    mnDatabase.Visibility = BarItemVisibility.Never
                End If
                If NullToBool(EksekusiSQlSkalarNew("SELECT IsMnSetting FROM MUser WHERE NoID=" & IDUserAktif)) Then
                    mnSetting.Visibility = BarItemVisibility.Always
                Else
                    mnSetting.Visibility = BarItemVisibility.Never
                End If
                If NullToBool(EksekusiSQlSkalarNew("SELECT IsMnDeveloper FROM MUser WHERE NoID=" & IDUserAktif)) Then
                    mnDeveloper.Visibility = BarItemVisibility.Always
                Else
                    mnDeveloper.Visibility = BarItemVisibility.Never
                End If

                IsLogin = True
                Timer1.Interval = 2000
                Timer1.Enabled = True
                DashBoard.TampilkanLinkBerdasarkanUser()
                TampilkanReminder()
            End If
            x.Dispose()
        End If
    End Sub
    Private Sub TampilkanReminder()
        Try
            If NullToBool(EksekusiSQlSkalarNew("SELECT IsReminderHutang FROM MUser WHERE NoID=" & IDUserAktif)) Then
                ReminderHutang = True
                ShowReminder = True
                Dim frmRemind As New frmReminder
                'frmRemind.Parent = Me
                frmRemind.TopMost = True
                frmRemind.Show()
            Else
                ReminderHutang = False
                ShowReminder = False
            End If
            Application.DoEvents()
        Catch ex As Exception

        End Try
    End Sub

    Private Sub HapusLogUser()
        Dim SQL As String = ""
        Try
            If IsLogin Then
                SQL = "UPDATE TLogUser SET TanggalEnd=Getdate() WHERE IDUser=" & IDUserAktif & " AND IP='" & FixApostropi(IPLokal) & "' AND TanggalEnd IS NULL"
                EksekusiSQL(SQL)
            End If
        Catch ex As Exception
            XtraMessageBox.Show("Info " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
        End Try
    End Sub
    Private Sub mnMenuEditor_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles mnMenuEditor.ItemClick
        Dim frm As New frmMenu
        If IsLogin Then
            frm.ShowDialog(Me)
            frm.Dispose()
        End If
    End Sub
    Sub LoadImageListFromDLL()

    End Sub
    Sub ExecuteFormbyCommand(ByVal FormName As String)
        Dim SQLconnect As New SQLite.SQLiteConnection()
        Dim SQLcommand As SQLiteCommand
        Dim odr As SQLite.SQLiteDataReader
        SQLconnect.ConnectionString = "Data Source=" & Application.StartupPath & "\System\engine\syssgi.sqlite" & ";"
        SQLconnect.Open()
        SQLcommand = SQLconnect.CreateCommand
        SQLcommand.CommandText = "SELECT idtipeform,namaform,namatabel,namatabeldetil,caption,namaformentri,namatabelmaster FROM sysformheader where namaform='" & FormName & "'"
        odr = SQLcommand.ExecuteReader
        If odr.Read Then
            Select Case NullTolInt(odr.GetValue(0))
                Case 0 'Daftar
                    'KODE DIBAWAH UNTUK MEMBUKA FORM HANYA 1
                    Dim frmEntri As frmDaftar = Nothing
                    Dim F As Object
                    For Each F In MdiChildren
                        If TypeOf F Is frmDaftar Then
                            frmEntri = F
                            If frmEntri.FormName = NullToStr(odr.GetValue(1)) Then
                                Exit For
                            Else
                                frmEntri = Nothing
                            End If
                        End If
                    Next
                    If frmEntri Is Nothing Then
                        frmEntri = New frmDaftar
                        frmEntri.WindowState = FormWindowState.Maximized
                        frmEntri.MdiParent = Me
                        frmEntri.FormName = NullToStr(odr.GetValue(1))
                        frmEntri.TableName = NullToStr(odr.GetValue(2))
                        frmEntri.Text = NullToStr(odr.GetValue(4))
                        frmEntri.FormEntriName = NullToStr(odr.GetValue(5))
                        frmEntri.TableMaster = NullToStr(odr.GetValue(6))
                    End If
                    frmEntri.Show()
                    frmEntri.Focus()

                    'Dim x As New frmDaftar
                    'x.FormName = NullTostr(odr.GetValue(1))
                    'x.TableName = NullTostr(odr.GetValue(2))
                    'x.Text = NullTostr(odr.GetValue(4))
                    'x.FormEntriName = NullTostr(odr.GetValue(5))
                    'x.TableMaster = NullTostr(odr.GetValue(6))
                    'x.MdiParent = Me
                    'x.Show()
                    'x.Focus()
                    'x.WindowState = FormWindowState.Maximized
                Case 1 'Simple Entri
                    If NullToStr(odr.GetValue(1)) = "EntriUser" Then
                        Dim x As New frmEntriUser
                        'x.FormName = NullTostr(odr.GetValue(1))
                        'x.TableName = NullTostr(odr.GetValue(2))
                        x.Text = NullToStr(odr.GetValue(4))
                        x.ShowDialog(Me)
                        x.Dispose()
                    Else
                        Dim x As New frmSimpleEntri
                        x.FormName = NullToStr(odr.GetValue(1))
                        x.TableName = NullToStr(odr.GetValue(2))
                        x.Text = NullToStr(odr.GetValue(4))
                        x.ShowDialog(Me)
                        x.Dispose()
                    End If
                Case 2 'Daftar + Filter Tanggal
                    'KODE DIBAWAH UNTUK MEMBUKA FORM HANYA 1
                    Dim frmEntri As frmDaftarFilterTanggal = Nothing
                    Dim F As Object
                    For Each F In MdiChildren
                        If TypeOf F Is frmDaftarFilterTanggal Then
                            frmEntri = F
                            If frmEntri.FormName = NullToStr(odr.GetValue(1)) Then
                                Exit For
                            Else
                                frmEntri = Nothing
                            End If
                        End If
                    Next
                    If frmEntri Is Nothing Then
                        frmEntri = New frmDaftarFilterTanggal
                        frmEntri.WindowState = FormWindowState.Maximized
                        frmEntri.MdiParent = Me
                        frmEntri.FormName = NullToStr(odr.GetValue(1))
                        frmEntri.TableName = NullToStr(odr.GetValue(2))
                        frmEntri.Text = NullToStr(odr.GetValue(4))
                        frmEntri.FormEntriName = NullToStr(odr.GetValue(5))
                        frmEntri.TableMaster = NullToStr(odr.GetValue(6))
                    End If
                    frmEntri.Show()
                    frmEntri.Focus()
                Case 3 'Daftar Tree
                    'KODE DIBAWAH UNTUK MEMBUKA FORM HANYA 1
                    Dim frmEntri As frmDaftarTree = Nothing
                    Dim F As Object
                    For Each F In MdiChildren
                        If TypeOf F Is frmDaftarTree Then
                            frmEntri = F
                            If frmEntri.FormName = NullToStr(odr.GetValue(1)) Then
                                Exit For
                            Else
                                frmEntri = Nothing
                            End If
                        End If
                    Next
                    If frmEntri Is Nothing Then
                        frmEntri = New frmDaftarTree
                        frmEntri.WindowState = FormWindowState.Maximized
                        frmEntri.MdiParent = Me
                        frmEntri.FormName = NullToStr(odr.GetValue(1))
                        frmEntri.TableName = NullToStr(odr.GetValue(2))
                        frmEntri.Text = NullToStr(odr.GetValue(4))
                        frmEntri.FormEntriName = NullToStr(odr.GetValue(5))
                        frmEntri.TableMaster = NullToStr(odr.GetValue(6))
                    End If
                    frmEntri.Show()
                    frmEntri.Focus()
                Case 4 'Daftar MasterDetil
                    'KODE DIBAWAH UNTUK MEMBUKA FORM HANYA 1
                    Dim frmEntri As frmDaftarMasterDetil = Nothing
                    Dim F As Object
                    For Each F In MdiChildren
                        If TypeOf F Is frmDaftarMasterDetil Then
                            frmEntri = F
                            If frmEntri.FormName = NullToStr(odr.GetValue(1)) Then
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
                        frmEntri.FormName = NullToStr(odr.GetValue(1))
                        frmEntri.TableName = NullToStr(odr.GetValue(2))
                        frmEntri.Text = NullToStr(odr.GetValue(4))
                        frmEntri.FormEntriName = NullToStr(odr.GetValue(5))
                        frmEntri.TableMaster = NullToStr(odr.GetValue(6))
                        frmEntri.CaptionCetak1 = NullToStr(EksekusiSQlSkalarNew("Select CaptionCetak1 from MMenu where objectrun='" & FormName & "'"))
                        frmEntri.CaptionCetak2 = NullToStr(EksekusiSQlSkalarNew("Select CaptionCetak2 from MMenu where objectrun='" & FormName & "'"))

                    End If
                    frmEntri.Show()
                    frmEntri.Focus()

                    'Dim x As New frmDaftarMasterDetil
                    'x.FormName = NullTostr(odr.GetValue(1))
                    'x.TableName = NullTostr(odr.GetValue(2))
                    'x.Text = NullTostr(odr.GetValue(4))
                    'x.FormEntriName = NullTostr(odr.GetValue(5))
                    'x.TableMaster = NullTostr(odr.GetValue(6))
                    'x.MdiParent = Me
                    'x.Show()
                    'x.Focus()
                    'x.WindowState = FormWindowState.Maximized
                Case 5 'Daftar MutasiWilayah
                    'KODE DIBAWAH UNTUK MEMBUKA FORM HANYA 1
                    Dim frmEntri As frmDaftarMutasiWilayah = Nothing
                    Dim F As Object
                    For Each F In MdiChildren
                        If TypeOf F Is frmDaftarMutasiWilayah Then
                            frmEntri = F
                            If frmEntri.FormName = NullToStr(odr.GetValue(1)) Then
                                Exit For
                            Else
                                frmEntri = Nothing
                            End If
                        End If
                    Next
                    If frmEntri Is Nothing Then
                        frmEntri = New frmDaftarMutasiWilayah
                        frmEntri.WindowState = FormWindowState.Maximized
                        frmEntri.MdiParent = Me
                        frmEntri.FormName = NullToStr(odr.GetValue(1))
                        frmEntri.TableName = NullToStr(odr.GetValue(2))
                        frmEntri.Text = NullToStr(odr.GetValue(4))
                        frmEntri.FormEntriName = NullToStr(odr.GetValue(5))
                        frmEntri.TableMaster = NullToStr(odr.GetValue(6))
                    End If
                    frmEntri.Show()
                    frmEntri.Focus()

                    'Dim x As New frmDaftarMasterDetil
                    'x.FormName = NullTostr(odr.GetValue(1))
                    'x.TableName = NullTostr(odr.GetValue(2))
                    'x.Text = NullTostr(odr.GetValue(4))
                    'x.FormEntriName = NullTostr(odr.GetValue(5))
                    'x.TableMaster = NullTostr(odr.GetValue(6))
                    'x.MdiParent = Me
                    'x.Show()
                    'x.Focus()
                    'x.WindowState = FormWindowState.Maximized
                Case 6 'Daftar Laporan
                    'KODE DIBAWAH UNTUK MEMBUKA FORM HANYA 1
                    Dim frmEntri As frmLaporan = Nothing
                    Dim F As Object
                    For Each F In MdiChildren
                        If TypeOf F Is frmLaporan Then
                            frmEntri = F
                            If frmEntri.FormName = NullToStr(odr.GetValue(1)) Then
                                Exit For
                            Else
                                frmEntri = Nothing
                            End If
                        End If
                    Next
                    If frmEntri Is Nothing Then
                        frmEntri = New frmLaporan
                        frmEntri.WindowState = FormWindowState.Maximized
                        frmEntri.MdiParent = Me
                        frmEntri.FormName = NullToStr(odr.GetValue(1))
                        frmEntri.TableName = NullToStr(odr.GetValue(2))
                        frmEntri.Text = NullToStr(odr.GetValue(4))
                        frmEntri.FormEntriName = NullToStr(odr.GetValue(5))
                        frmEntri.TableMaster = NullToStr(odr.GetValue(6))
                    End If
                    frmEntri.Show()
                    frmEntri.Focus()

                    'Dim x As New frmDaftarMasterDetil
                    'x.FormName = NullTostr(odr.GetValue(1))
                    'x.TableName = NullTostr(odr.GetValue(2))
                    'x.Text = NullTostr(odr.GetValue(4))
                    'x.FormEntriName = NullTostr(odr.GetValue(5))
                    'x.TableMaster = NullTostr(odr.GetValue(6))
                    'x.MdiParent = Me
                    'x.Show()
                    'x.Focus()
                    'x.WindowState = FormWindowState.Maximized
                Case 7 'Daftar Accounting
                    'KODE DIBAWAH UNTUK MEMBUKA FORM HANYA 1
                    Dim frmEntri As frmDaftarTransaksiAccounting = Nothing
                    Dim F As Object
                    For Each F In MdiChildren
                        If TypeOf F Is frmDaftarTransaksiAccounting Then
                            frmEntri = F
                            If frmEntri.FormName = NullToStr(odr.GetValue(1)) Then
                                Exit For
                            Else
                                frmEntri = Nothing
                            End If
                        End If
                    Next
                    If frmEntri Is Nothing Then
                        frmEntri = New frmDaftarTransaksiAccounting
                        frmEntri.WindowState = FormWindowState.Maximized
                        frmEntri.MdiParent = Me
                        frmEntri.FormName = NullToStr(odr.GetValue(1))
                        frmEntri.TableName = NullToStr(odr.GetValue(2))
                        frmEntri.Text = NullToStr(odr.GetValue(4))
                        frmEntri.FormEntriName = NullToStr(odr.GetValue(5))
                        frmEntri.TableMaster = NullToStr(odr.GetValue(6))
                        'frmEntri.CaptionCetak1 = NullToStr(EksekusiSQlSkalarNew("Select CaptionCetak1 from MMenu where objectrun='" & FormName & "'"))
                        'frmEntri.CaptionCetak2 = NullToStr(EksekusiSQlSkalarNew("Select CaptionCetak2 from MMenu where objectrun='" & FormName & "'"))

                    End If
                    frmEntri.Show()
                    frmEntri.Focus()
            End Select

        End If
        odr.Close()
        SQLcommand.Dispose()
        SQLconnect.Close()
        SQLconnect.Dispose()

    End Sub
    Private Sub BarCheckItem1_CheckedChanged(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarCheckItem1.CheckedChanged
        If Not BarCheckItem1.Checked Then
            XtraTabbedMdiManager1.MdiParent = Nothing
            iCascade.Visibility = BarItemVisibility.Always
            iTileHorizontal.Visibility = BarItemVisibility.Always
            iTileVertical.Visibility = BarItemVisibility.Always
        Else
            XtraTabbedMdiManager1.MdiParent = Me
            iCascade.Visibility = BarItemVisibility.Never
            iTileHorizontal.Visibility = BarItemVisibility.Never
            iTileVertical.Visibility = BarItemVisibility.Never
        End If
    End Sub

    Private Sub BarButtonItem4_ItemClick_1(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles mnUser.ItemClick
        'KODE DIBAWAH UNTUK MEMBUKA FORM HANYA 1
        Dim frmEntri As frmDaftarUser = Nothing
        Dim F As Object
        For Each F In MdiChildren
            If TypeOf F Is frmDaftarUser Then
                frmEntri = F
            End If
        Next
        If frmEntri Is Nothing Then
            frmEntri = New frmDaftarUser
            frmEntri.WindowState = FormWindowState.Maximized
            frmEntri.MdiParent = Me
        End If
        frmEntri.Show()
        frmEntri.Focus()
    End Sub

    Private Sub mnKoneksi_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles mnKoneksi.ItemClick
        Dim x As New frmSetting
        x.ShowDialog(Me)
        x.Dispose()
    End Sub

    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

    End Sub

    Private Sub ckEditRpt_CheckedChanged(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles ckEditRpt.CheckedChanged
        EditReport = ckEditRpt.Checked
    End Sub

    Private Sub mnPerbaikanStok_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles mnPerbaikanStok.ItemClick
        Dim x As New frmPerbaikanPosting
        If IsLogin Then
            x.ShowDialog(Me)
        End If
        x.Dispose()
    End Sub

    Private Sub ckFx_CheckedChanged(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles ckFx.CheckedChanged
        TulisIni("Application", "UseFramework", ckFx.Checked)
    End Sub

    Private Sub mnSettingPerusahaan_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles mnSettingPerusahaan.ItemClick
        Dim x As New frmSettingPerusahaan
        If IsLogin Then
            x.ShowDialog(Me)
        End If
        x.Dispose()
    End Sub

    Private Sub Timer1_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer1.Tick
        Dim SQL As String = ""
        Dim ds As New DataSet
        Try
            If CekKoneksi() Then
                ds = ExecuteDataset("Server", "SELECT GetDate() AS TglSystem ")
                If ds.Tables(0).Rows.Count >= 1 Then
                    TanggalSystem = ds.Tables(0).Rows(0).Item("TglSystem")
                    mnTglSystem.Caption = "Tanggal System : " & TanggalSystem.ToString("dd/MM/yyyy HH:mm")
                    mnStatusPeriode.Caption = "Periode : " & TanggalSystem.ToString("MMMM yyyy")
                    mnStatusServer.Caption = "Server : " & BacaIni("dbconfig", "Server", "(local)").ToLower
                End If
            Else
                Timer1.Enabled = False
            End If

            Application.DoEvents()
            If ReminderHutang AndAlso Not ShowReminder AndAlso DateDiff(DateInterval.Minute, TglUpdateReminder, TanggalSystem) >= 20 Then
                ReminderHutang = True
                ShowReminder = True
                Dim frmRemind As New frmReminder
                'frmRemind.Parent = Me
                frmRemind.TopMost = True
                frmRemind.Show()
            Else
                'ReminderHutang = False
                'ShowReminder = False
            End If
            Application.DoEvents()
            'If DateDiff(DateInterval.Day, CDate("2012,12,01"), TanggalSystem) >= 1 Then 'Trial Terpaksa
            '    XtraMessageBox.Show("Silahkan untuk mengaktivasi program.", "VPoint Developer", MessageBoxButtons.OK, MessageBoxIcon.Stop)
            '    End
            'End If
            Application.DoEvents()
        Catch ex As Exception
        Finally
            ds.Dispose()
        End Try
    End Sub

    Private Sub mnSettingKode_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles mnSettingKode.ItemClick
        Dim x As New frmSettingPerusahaan
        If IsLogin Then
            x.ShowDialog(Me)
        End If
        x.Dispose()
    End Sub

    Private Sub mnStatusUser_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles mnStatusUser.ItemClick
        Dim x As New FrmGantiPassword
        Try
            If IsLogin Then
                If x.ShowDialog(Me) Then

                End If
            End If
        Catch ex As Exception
            XtraMessageBox.Show("Pesan kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            x.Dispose()
        End Try
    End Sub

    Private Sub iCascade_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles iCascade.ItemClick
        Me.LayoutMdi(MdiLayout.Cascade)
    End Sub

    Private Sub iTileHorizontal_ItemClick(ByVal sender As Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles iTileHorizontal.ItemClick
        Me.LayoutMdi(MdiLayout.TileHorizontal)
    End Sub

    Private Sub iTileVertical_ItemClick(ByVal sender As Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles iTileVertical.ItemClick
        Me.LayoutMdi(MdiLayout.TileVertical)
    End Sub

    Private Sub ckLangsungCetak_CheckedChanged(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles ckLangsungCetak.CheckedChanged
        LangsungCetak = ckLangsungCetak.Checked
    End Sub

    Private Sub mnCPUActive_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles mnCPUActive.ItemClick
        If IsLogin Then
            ExecuteFormbyCommand("TLogUser")
        End If
    End Sub

    Private Sub mnMesinPOS_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles mnMesinPOS.ItemClick
        If IsLogin Then
            ExecuteFormbyCommand("MPOS")
        End If
    End Sub

    Private Sub mnHelp_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles mnHelp.ItemClick
        Dim FileHelp As String = ""
        Try
            FileHelp = Application.StartupPath & "\system\book.chm"
            If System.IO.File.Exists(FileHelp) Then
                BukaFile(FileHelp)
            Else
                XtraMessageBox.Show("File : " & FileHelp & " tidak ditemukan.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Warning)
            End If
        Catch ex As Exception
            XtraMessageBox.Show("Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
        End Try
    End Sub

    Private Sub BarButtonItem9_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem9.ItemClick
        Dim x As New frmImportDataBarang
        Try
            If IsLogin AndAlso x.ShowDialog(Me) Then

            End If
            x.Dispose()
        Catch ex As Exception
            XtraMessageBox.Show("Pesan kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            x.Dispose()
        End Try
    End Sub

    Private Sub mnBackupDB_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles mnBackupDB.ItemClick
        Dim x As New frmBackupDatabase
        Try
            If IsLogin Then
                x.tipe = frmBackupDatabase.TypeDB.Backup
                x.StartPosition = FormStartPosition.CenterParent
                x.ShowDialog(Me)
            End If
        Catch ex As Exception
            XtraMessageBox.Show("Info Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            x.Dispose()
        End Try
    End Sub

    Private Sub mnRestoreDatabase_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles mnRestoreDatabase.ItemClick
        Dim x As New frmBackupDatabase
        Try
            If IsLogin AndAlso IsSupervisor Then
                x.tipe = frmBackupDatabase.TypeDB.Restore
                x.StartPosition = FormStartPosition.CenterParent
                x.ShowDialog(Me)
            End If
        Catch ex As Exception
            XtraMessageBox.Show("Info Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            x.Dispose()
        End Try
    End Sub

    Private Sub mnSQLExecutor_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles mnSQLExecutor.ItemClick
        Dim a As New frmOtorisasiAdmin
        Try
            If IsLogin Then
                a.UID = "sa"
                a.PWD = "sys"
                a.IsForceSystem = True
                If a.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
                    Dim frmEntri As frmSQLEditor = Nothing
                    Dim F As Object
                    For Each F In MdiChildren
                        If TypeOf F Is frmSQLEditor Then
                            frmEntri = F
                            Exit For
                        End If
                    Next
                    If frmEntri Is Nothing Then
                        frmEntri = New frmSQLEditor
                        frmEntri.WindowState = FormWindowState.Maximized
                        frmEntri.MdiParent = Me
                    End If
                    frmEntri.Show()
                    frmEntri.Focus()
                End If
            End If
        Catch ex As Exception
            XtraMessageBox.Show("Info Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            a.Dispose()
        End Try
    End Sub

    Private Sub mnSettingAkun_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles mnSettingAkun.ItemClick
        Dim x As New frmSettingAkunPenting
        Dim xa As New frmOtorisasiAdmin
        Try
            If IsLogin Then
                xa.UID = "sa"
                xa.PWD = "sys"
                xa.IsForceSystem = True
                If xa.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
                    x.ShowDialog(Me)
                End If
            End If
        Catch ex As Exception
            XtraMessageBox.Show("Info Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
        Finally
            xa.Dispose()
            x.Dispose()
        End Try
    End Sub

    Private Sub BarButtonItem10_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem10.ItemClick
        Dim x As New frmOtorisasiAdmin
        Dim frmSett As New frmSettingApplikasi
        Try
            x.IsForceSystem = True
            x.UID = "sa"
            x.PWD = "sys"
            If x.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
                frmSett.ShowDialog()
            End If
        Catch ex As Exception
            XtraMessageBox.Show("Info Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
        Finally
            x.Dispose()
            frmSett.Dispose()
        End Try
    End Sub

    Private Sub mnBackupDataKasir_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles mnBackupDataKasir.ItemClick
        Dim x As New frmExportDataKasir
        Try
            If IsLogin AndAlso IsSupervisor Then
                x.ShowDialog(Me)
            End If
        Catch ex As Exception
            XtraMessageBox.Show("Info Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK)
        Finally
            x.Dispose()
        End Try
    End Sub

    Private Sub frmMain_Shown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Shown

    End Sub

    Private Sub mnMutasiData_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles mnMutasiData.ItemClick
        Dim x As New frmMutasiData
        If x.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then

        End If
        x.Dispose()
    End Sub
End Class