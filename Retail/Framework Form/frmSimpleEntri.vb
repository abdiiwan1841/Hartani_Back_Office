Imports System.Data.SqlClient
Imports System.Data.SQLite

Imports DevExpress.XtraEditors
Imports DevExpress.XtraLayout
Imports VPoint.Ini
Public Class frmSimpleEntri
    Public FormName As String = ""
    Public Caption As String = ""
    Public TableName As String = ""

    Public isNew As Boolean = True
    Public NoID As Long = -1
    Dim oda2 As SqlDataAdapter

    Dim ds As New DataSet
    Dim BS As New BindingSource
    Public WithEvents cb As New DevExpress.XtraEditors.Controls.EditorButton

    Public WithEvents txtEdit As DevExpress.XtraEditors.TextEdit
    Public WithEvents clcEdit As DevExpress.XtraEditors.CalcEdit
    Public WithEvents ckedit As DevExpress.XtraEditors.CheckEdit
    Public WithEvents dtEdit As DevExpress.XtraEditors.DateEdit
    Public WithEvents lkEdit As DevExpress.XtraEditors.SearchLookUpEdit
    Dim KodeLama As String = ""
    Dim NamaLama As String = ""
    Dim BarcodeLama As String = ""
    Dim SQLtxtGudangTujuan As String = ""
    Dim isProsesLoad As Boolean = True
    Public IDParent As Long = -1

    Public KonversiCTN As Double = 0
    Public txtSatuanMBarangD As SearchLookUpEdit = Nothing
    Sub GenerateForm()
        Try
            Dim cn As New SqlConnection(StrKonSql)
            Dim ocmd2 As New SqlCommand
            Dim itemLC As LayoutControlItem

            Dim sysSQLconnect As New SQLite.SQLiteConnection()
            Dim sysSQLcommand As SQLiteCommand
            Dim sysSQLoda As SQLite.SQLiteDataAdapter
            sysSQLconnect.ConnectionString = "Data Source=" & Application.StartupPath & "\\System\engine\syssgi.sqlite" & ";"
            sysSQLconnect.Open()
            sysSQLcommand = sysSQLconnect.CreateCommand
            sysSQLcommand.CommandText = "SELECT * FROM sysform where formname='" & FormName & "'"
            TableName = GetTableNamebyFormname(FormName, "")
            ocmd2.Connection = cn
            ocmd2.CommandType = CommandType.Text
            If isNew Then
                ocmd2.CommandText = "select * from " & TableName & " where noid=" & NoID.ToString
            Else
                ocmd2.CommandText = "select * from " & TableName & " where noid=" & NoID.ToString
            End If
            cn.Open()
            sysSQLoda = New SQLite.SQLiteDataAdapter(sysSQLcommand)
            sysSQLoda.Fill(ds, "Master")
            oda2 = New SqlDataAdapter(ocmd2)
            oda2.Fill(ds, "Data")
            BS.DataSource = ds.Tables("Data")
            For i = 0 To ds.Tables("Master").Rows.Count - 1
                Select Case ds.Tables("Master").Rows(i).Item("control").ToString.ToLower
                    Case "textedit"
                        txtEdit = New DevExpress.XtraEditors.TextEdit

                        txtEdit.Name = "txt" + ds.Tables("Master").Rows(i).Item("nama")

                        txtEdit.EnterMoveNextControl = True
                        txtEdit.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near

                        Select Case ds.Tables("Master").Rows(i).Item("tipe").ToString.ToLower
                            Case "int", "bigint", "smallint", "float", "numeric", "money", "real"
                                txtEdit.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric
                                txtEdit.Properties.Mask.EditMask = NullToStr(ds.Tables("Master").Rows(i).Item("format"))
                                txtEdit.Properties.Mask.UseMaskAsDisplayFormat = True
                                txtEdit.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
                                txtEdit.Properties.AllowNullInput = DevExpress.Utils.DefaultBoolean.False
                                txtEdit.EditValue = 0
                                AddHandler txtEdit.KeyDown, AddressOf FungsiControl.txtNumeric_KeyDown
                            Case "date", "datetime", "time"
                                txtEdit.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.DateTime
                                txtEdit.Properties.Mask.EditMask = ds.Tables("Master").Rows(i).Item("format")
                                txtEdit.Properties.Mask.UseMaskAsDisplayFormat = True
                                txtEdit.Properties.AllowNullInput = DevExpress.Utils.DefaultBoolean.False
                                txtEdit.EditValue = Date.Today
                            Case Else
                                If Not IsDBNull(ds.Tables("Master").Rows(i).Item("format")) AndAlso ds.Tables("Master").Rows(i).Item("format") <> "" Then
                                    txtEdit.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Simple
                                    txtEdit.Properties.Mask.EditMask = ds.Tables("Master").Rows(i).Item("format")
                                    txtEdit.Properties.Mask.UseMaskAsDisplayFormat = True
                                    txtEdit.Properties.AllowNullInput = DevExpress.Utils.DefaultBoolean.False
                                    txtEdit.EditValue = ""
                                Else
                                    txtEdit.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.None
                                    txtEdit.Properties.AllowNullInput = DevExpress.Utils.DefaultBoolean.False
                                    txtEdit.EditValue = ""
                                End If
                                txtEdit.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near
                                'txtEdit.Properties.AppearanceDisabled.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near
                                'txtEdit.Properties.AppearanceFocused.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near
                                'txtEdit.Properties.AppearanceReadOnly.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near

                                'MsgBox(txtEdit.Name)
                        End Select
                        txtEdit.Visible = NullToBool(ds.Tables("Master").Rows(i).Item("visible"))
                        txtEdit.Properties.ReadOnly = NullToBool(ds.Tables("Master").Rows(i).Item("readonly"))
                        If NullToStr(ds.Tables("Master").Rows(i).Item("default")) <> "" Then
                            txtEdit.Tag = "df:" & NullToStr(ds.Tables("Master").Rows(i).Item("default"))
                        End If
                        If NullToStr(ds.Tables("Master").Rows(i).Item("function")) <> "" Then
                            txtEdit.Tag = IIf(txtEdit.Tag <> "", txtEdit.Tag & ";", "") & NullToStr(ds.Tables("Master").Rows(i).Item("function"))
                        End If
                        If NullToStr(ds.Tables("Master").Rows(i).Item("fieldname")) <> "" Then
                            txtEdit.DataBindings.Add("Editvalue", BS _
                         , ds.Tables("Master").Rows(i).Item("fieldname"))
                        End If

                        If ds.Tables("Master").Rows(i).Item("tipe").ToString.Length >= 10 AndAlso ds.Tables("Master").Rows(i).Item("tipe").ToString.Substring(0, 7).ToLower = "varchar" Then
                            'varchar(120)
                            If IsNumeric(ds.Tables("Master").Rows(i).Item("tipe").ToString.Substring(8, ds.Tables("Master").Rows(i).Item("tipe").ToString.Length - 9)) Then
                                txtEdit.Properties.MaxLength = CInt(ds.Tables("Master").Rows(i).Item("tipe").ToString.Substring(8, ds.Tables("Master").Rows(i).Item("tipe").ToString.Length - 9))
                            End If
                        End If
                        If (ds.Tables("Master").Rows(i).Item("tipe").ToString.Length >= 10) AndAlso ds.Tables("Master").Rows(i).Item("tipe").ToString.Substring(0, 7).ToLower = "numeric" Then
                            'Numeric(120)
                            If IsNumeric(ds.Tables("Master").Rows(i).Item("tipe").ToString.Substring(8, ds.Tables("Master").Rows(i).Item("tipe").ToString.Length - 9)) Then
                                txtEdit.Properties.MaxLength = CInt(ds.Tables("Master").Rows(i).Item("tipe").ToString.Substring(8, ds.Tables("Master").Rows(i).Item("tipe").ToString.Length - 9))
                            End If
                        End If
                        If (ds.Tables("Master").Rows(i).Item("tipe").ToString.Length >= 8) AndAlso ds.Tables("Master").Rows(i).Item("tipe").ToString.Substring(0, 5).ToLower = "money" Then
                            'Money(120)
                            If IsNumeric(ds.Tables("Master").Rows(i).Item("tipe").ToString.Substring(6, ds.Tables("Master").Rows(i).Item("tipe").ToString.Length - 7)) Then
                                txtEdit.Properties.MaxLength = CInt(ds.Tables("Master").Rows(i).Item("tipe").ToString.Substring(6, ds.Tables("Master").Rows(i).Item("tipe").ToString.Length - 7))
                            End If
                        End If
                        If UCase(ds.Tables("Master").Rows(i).Item("nama")) = "NPWP" Then
                            txtEdit.Properties.Mask.MaskType = Mask.MaskType.Simple
                            txtEdit.Properties.Mask.EditMask = "00.000.000.0-000.000"
                        End If
                        ' txtEdit.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near
                        itemLC = New LayoutControlItem
                        itemLC.Name = ds.Tables("Master").Rows(i).Item("caption")
                        itemLC.Parent = LC1.Root

                        itemLC.Control = txtEdit

                        AddHandler txtEdit.GotFocus, AddressOf txtEdit_GotFocus
                        AddHandler txtEdit.LostFocus, AddressOf txtEdit_LostFocus
                        'AddHandler txtEdit.MouseDown, AddressOf txtEdit_MouseDn
                        If txtEdit.Name.ToLower = "txtkode".ToLower Then
                            KodeLama = txtEdit.Text
                            txtEdit.Properties.CharacterCasing = CharacterCasing.Upper
                        ElseIf txtEdit.Name.ToLower = "txtnama".ToLower Then
                            NamaLama = txtEdit.Text
                        ElseIf txtEdit.Name.ToLower = "txtbarcode".ToLower Then
                            BarcodeLama = txtEdit.Text
                        End If

                    Case "calcedit"
                        clcEdit = New DevExpress.XtraEditors.CalcEdit
                        clcEdit.Name = "txt" + ds.Tables("Master").Rows(i).Item("nama")
                        clcEdit.EnterMoveNextControl = True
                        Select Case ds.Tables("Master").Rows(i).Item("tipe").ToString.ToLower
                            Case "int", "bigint", "smallint", "float", "numeric", "money", "real"
                                clcEdit.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric
                                clcEdit.Properties.Mask.EditMask = ds.Tables("Master").Rows(i).Item("format")
                                clcEdit.Properties.Mask.UseMaskAsDisplayFormat = True
                                clcEdit.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
                                clcEdit.EditValue = 0
                                AddHandler clcEdit.KeyDown, AddressOf FungsiControl.txtNumeric_KeyDown
                            Case "date", "datetime", "time"
                                clcEdit.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.DateTime
                                clcEdit.Properties.Mask.EditMask = ds.Tables("Master").Rows(i).Item("format")
                                clcEdit.Properties.Mask.UseMaskAsDisplayFormat = True
                                clcEdit.EditValue = Date.Today
                            Case Else
                                If Not IsDBNull(ds.Tables("Master").Rows(i).Item("format")) AndAlso ds.Tables("Master").Rows(i).Item("format") <> "" Then
                                    clcEdit.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Custom
                                    clcEdit.Properties.Mask.EditMask = ds.Tables("Master").Rows(i).Item("format")
                                    clcEdit.Properties.Mask.UseMaskAsDisplayFormat = True
                                    clcEdit.EditValue = ""
                                Else
                                    clcEdit.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.None
                                    clcEdit.EditValue = ""
                                End If
                        End Select
                        clcEdit.Visible = NullToBool(ds.Tables("Master").Rows(i).Item("visible"))
                        clcEdit.Properties.ReadOnly = NullToBool(ds.Tables("Master").Rows(i).Item("readonly"))
                        If NullToStr(ds.Tables("Master").Rows(i).Item("default")) <> "" Then
                            clcEdit.Tag = "df:" & NullToStr(ds.Tables("Master").Rows(i).Item("default"))
                        End If
                        If NullToStr(ds.Tables("Master").Rows(i).Item("function")) <> "" Then
                            clcEdit.Tag = IIf(clcEdit.Tag <> "", clcEdit.Tag & ";", "") & NullToStr(ds.Tables("Master").Rows(i).Item("function"))
                        End If
                        If NullToStr(ds.Tables("Master").Rows(i).Item("fieldname")) <> "" Then
                            clcEdit.DataBindings.Add("editvalue", BS _
                         , ds.Tables("Master").Rows(i).Item("fieldname"))
                        End If

                        If (ds.Tables("Master").Rows(i).Item("tipe").ToString.Length >= 10) AndAlso ds.Tables("Master").Rows(i).Item("tipe").ToString.Substring(0, 7).ToLower = "numeric" Then
                            'Numeric(120)
                            If IsNumeric(ds.Tables("Master").Rows(i).Item("tipe").ToString.Substring(8, ds.Tables("Master").Rows(i).Item("tipe").ToString.Length - 9)) Then
                                clcEdit.Properties.MaxLength = CInt(ds.Tables("Master").Rows(i).Item("tipe").ToString.Substring(8, ds.Tables("Master").Rows(i).Item("tipe").ToString.Length - 9))
                            End If
                        End If
                        If (ds.Tables("Master").Rows(i).Item("tipe").ToString.Length >= 8) AndAlso ds.Tables("Master").Rows(i).Item("tipe").ToString.Substring(0, 5).ToLower = "money" Then
                            'Money(120)
                            If IsNumeric(ds.Tables("Master").Rows(i).Item("tipe").ToString.Substring(6, ds.Tables("Master").Rows(i).Item("tipe").ToString.Length - 7)) Then
                                clcEdit.Properties.MaxLength = CInt(ds.Tables("Master").Rows(i).Item("tipe").ToString.Substring(6, ds.Tables("Master").Rows(i).Item("tipe").ToString.Length - 7))
                            End If
                        End If

                        itemLC = New LayoutControlItem
                        itemLC.Name = ds.Tables("Master").Rows(i).Item("caption")
                        itemLC.Parent = LC1.Root

                        itemLC.Control = clcEdit
                        AddHandler clcEdit.LostFocus, AddressOf clcEdit_LostFocus
                        AddHandler clcEdit.GotFocus, AddressOf clcEdit_GotFocus
                        'AddHandler clcEdit.MouseDown, AddressOf clcEdit_MouseDn
                    Case "checkedit"
                        'ckedit = New DevExpress.XtraEditors.CheckEdit
                        'ckedit.Name = "txt" + ds.Tables("Master").Rows(i).Item("nama")
                        'ckedit.Text = ds.Tables("Master").Rows(i).Item("caption")
                        'ckedit.Visible = NullTobool(ds.Tables("Master").Rows(i).Item("visible"))
                        'ckedit.Properties.ReadOnly = NullTobool(ds.Tables("Master").Rows(i).Item("readonly"))
                        'ckedit.DataBindings.Add("editvalue", BS _
                        '  , ds.Tables("Master").Rows(i).Item("fieldname"))
                        'itemLC = New LayoutControlItem
                        'itemLC.Name = ds.Tables("Master").Rows(i).Item("caption")

                        'itemLC.Parent = LC1.Root
                        'itemLC.Control = ckedit

                        ckedit = New DevExpress.XtraEditors.CheckEdit
                        ckedit.Name = "txt" + ds.Tables("Master").Rows(i).Item("nama")
                        ckedit.Text = ds.Tables("Master").Rows(i).Item("caption")
                        ckedit.Checked = False
                        ckedit.Visible = NullToBool(ds.Tables("Master").Rows(i).Item("visible"))
                        ckedit.Properties.ReadOnly = NullToBool(ds.Tables("Master").Rows(i).Item("readonly"))
                        ckedit.DataBindings.Add("editvalue", BS _
                          , ds.Tables("Master").Rows(i).Item("fieldname"))

                        'ckedit.Visible = NullTobool(ds.Tables("Master").Rows(i).Item("visible"))
                        'ckedit.Properties.ReadOnly = NullTobool(ds.Tables("Master").Rows(i).Item("readonly"))
                        If NullToStr(ds.Tables("Master").Rows(i).Item("default")) <> "" Then
                            ckedit.Tag = "df:" & NullToStr(ds.Tables("Master").Rows(i).Item("default"))
                        End If
                        If NullToStr(ds.Tables("Master").Rows(i).Item("function")) <> "" Then
                            ckedit.Tag = IIf(ckedit.Tag <> "", ckedit.Tag & ";", "") & NullToStr(ds.Tables("Master").Rows(i).Item("function"))
                        End If
                        'If NullTostr(ds.Tables("Master").Rows(i).Item("fieldname")) <> "" Then
                        '    ckedit.DataBindings.Add("Text", BS _
                        ' , ds.Tables("Master").Rows(i).Item("fieldname"))
                        'End If

                        itemLC = New LayoutControlItem
                        itemLC.Name = ds.Tables("Master").Rows(i).Item("caption")

                        itemLC.Parent = LC1.Root
                        itemLC.Control = ckedit
                    Case "dateedit"
                        dtEdit = New DevExpress.XtraEditors.DateEdit
                        dtEdit.Name = "txt" + ds.Tables("Master").Rows(i).Item("nama")
                        dtEdit.EnterMoveNextControl = True
                        dtEdit.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.DateTime
                        dtEdit.Properties.Mask.EditMask = ds.Tables("Master").Rows(i).Item("format")
                        dtEdit.DateTime = Date.Today

                        dtEdit.Properties.Mask.UseMaskAsDisplayFormat = True
                        dtEdit.Visible = NullToBool(ds.Tables("Master").Rows(i).Item("visible"))
                        dtEdit.Properties.ReadOnly = NullToBool(ds.Tables("Master").Rows(i).Item("readonly"))
                        If NullToStr(ds.Tables("Master").Rows(i).Item("default")) <> "" Then
                            dtEdit.Tag = "df:" & NullToStr(ds.Tables("Master").Rows(i).Item("default"))
                        End If

                        dtEdit.DataBindings.Add("editvalue", BS _
                          , ds.Tables("Master").Rows(i).Item("fieldname"))
                        itemLC = New LayoutControlItem
                        itemLC.Name = ds.Tables("Master").Rows(i).Item("caption")
                        itemLC.Parent = LC1.Root
                        itemLC.Control = dtEdit
                    Case "lookupedit"
                        Dim dsLookUp As New DataSet
                        cb.Kind = DevExpress.XtraEditors.Controls.ButtonPredefines.Delete
                        lkEdit = New DevExpress.XtraEditors.SearchLookUpEdit
                        If CStr("txt" + ds.Tables("Master").Rows(i).Item("nama")).ToLower <> "txtidgudangtujuan" Then
                            lkEdit.Properties.Buttons.Add(New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Ellipsis))
                        End If
                        lkEdit.Properties.Buttons.Add(cb)
                        lkEdit.Properties.NullText = ""
                        lkEdit.Name = "txt" + ds.Tables("Master").Rows(i).Item("nama")
                        lkEdit.Properties.DisplayMember = NullToStr(ds.Tables("Master").Rows(i).Item("lookupdisplay"))
                        lkEdit.Properties.ValueMember = NullToStr(ds.Tables("Master").Rows(i).Item("lookupvalue"))
                        lkEdit.EnterMoveNextControl = True
                        lkEdit.EditValue = -1
                        'lkEdit.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.DateTime
                        'lkEdit.Properties.Mask.EditMask = ds.Tables("Master").Rows(i).Item("format")
                        'lkEdit.Properties.Mask.UseMaskAsDisplayFormat = True
                        lkEdit.Visible = NullToBool(ds.Tables("Master").Rows(i).Item("visible"))
                        lkEdit.Properties.ReadOnly = NullToBool(ds.Tables("Master").Rows(i).Item("readonly"))
                        If NullToStr(ds.Tables("Master").Rows(i).Item("default")) <> "" Then
                            lkEdit.Tag = "df:" & NullToStr(ds.Tables("Master").Rows(i).Item("default"))
                        End If
                        If NullToStr(ds.Tables("Master").Rows(i).Item("function")) <> "" Then
                            lkEdit.Tag = IIf(lkEdit.Tag <> "", lkEdit.Tag & ";", "") & NullToStr(ds.Tables("Master").Rows(i).Item("function"))
                        End If
                        If NullToStr(ds.Tables("Master").Rows(i).Item("fieldname")) <> "" Then
                            lkEdit.DataBindings.Add("editvalue", BS _
                         , ds.Tables("Master").Rows(i).Item("fieldname"))
                        End If


                        itemLC = New LayoutControlItem
                        itemLC.Name = ds.Tables("Master").Rows(i).Item("caption")
                        itemLC.Parent = LC1.Root
                        itemLC.Control = lkEdit
                        If NullToStr(ds.Tables("Master").Rows(i).Item("sql")) <> "" Then
                            ocmd2.CommandText = modSqlServer.IsiVariabelDef(NullToStr(ds.Tables("Master").Rows(i).Item("sql")))
                        Else
                            ocmd2.CommandText = "Select * from " & ds.Tables("Master").Rows(i).Item("tablelookup")
                        End If

                        lkEdit.Tag = IIf(NullToStr(lkEdit.Tag).Length > 0, lkEdit.Tag & ";", "").ToString & "sqllookup:" & ocmd2.CommandText

                        oda2 = New SqlDataAdapter(ocmd2)
                        oda2.Fill(dsLookUp, ds.Tables("Master").Rows(i).Item("tablelookup"))
                        lkEdit.Properties.DataSource = dsLookUp.Tables(ds.Tables("Master").Rows(i).Item("tablelookup"))
                        AddHandler lkEdit.EditValueChanged, AddressOf lkEdit_EditValueChanged
                        AddHandler lkEdit.ButtonClick, AddressOf lkEdit_ButtonClick
                        AddHandler lkEdit.Click, AddressOf lkEdit_Click
                        If FormName = "EntriRuleMutasiGudang" AndAlso lkEdit.Name.ToLower = "txtidgudangasal".ToLower Then
                            AddHandler lkEdit.EditValueChanged, AddressOf txtIDGudangAsal_EditValueChanged
                            lkEditGudangAsal = lkEdit
                        ElseIf FormName = "EntriRuleMutasiGudang" AndAlso lkEdit.Name.ToLower = "txtidgudangtujuan".ToLower Then
                            AddHandler lkEdit.GotFocus, AddressOf lkEdit_GotFocus
                            SQLtxtGudangTujuan = ocmd2.CommandText
                        End If
                        dsLookUp.Dispose()
                End Select
            Next

            Dim STR() As String
            For Each ctrl In LC1.Controls
                If ctrl.tag <> "" Then
                    STR = Split(ctrl.tag, ";")
                    For i = 0 To UBound(STR)
                        If STR(i).Substring(0, 2).Trim.ToLower = "lu" Then
                            lkEdit_EditValueChanged(ctrl, Nothing)
                        End If
                    Next
                End If
            Next

        Catch ex As Exception
            XtraMessageBox.Show("Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub
    Dim lkEditGudangAsal As SearchLookUpEdit

    Private Sub frmSimpleEntri_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        If DialogResult = Windows.Forms.DialogResult.Cancel AndAlso cmdSave.Enabled Then
            If XtraMessageBox.Show("Yakin ingin keluar?" & vbCrLf & "Cancel untuk membatalkan", NamaAplikasi, MessageBoxButtons.OKCancel, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) = Windows.Forms.DialogResult.Cancel Then
                e.Cancel = True
            End If
        End If
    End Sub

    Private Sub frmSimpleEntri_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        isProsesLoad = True
        Dim dlg As DevExpress.Utils.WaitDialogForm = Nothing
        Try
            Dim curentcursor As Cursor = Windows.Forms.Cursor.Current
            Windows.Forms.Cursor.Current = Cursors.WaitCursor
            dlg = New DevExpress.Utils.WaitDialogForm(String.Format("Creating component on " & Me.FormName.ToString & ".{0}MOHON TUNGGU ...", vbCrLf), Application.ProductName)
            dlg.TopMost = False
            dlg.Show()
            Windows.Forms.Cursor.Current = Cursors.WaitCursor

            TableName = GetTableNamebyFormname(FormName, Caption)
            Me.Text = Caption
            Me.Width = Ini.BacaIniPath(FolderLayouts & FormName & ".ini", "Form", "Width", Me.Width)
            Me.Height = Ini.BacaIniPath(FolderLayouts & FormName & ".ini", "Form", "Height", Me.Height)

            Me.Left = (Screen.PrimaryScreen.WorkingArea.Width - Me.Width) \ 2
            Me.Top = (Screen.PrimaryScreen.WorkingArea.Height - Me.Height) \ 2
            GenerateForm()
            'LC1.BeginUpdate()
            If System.IO.File.Exists(FolderLayouts & FormName & ".xml") Then
                LC1.RestoreLayoutFromXml(FolderLayouts & FormName & ".xml")
            End If
            'LC1.EndUpdate()
            For Each ctl In LC1.Controls
                If ctl.name.ToString.ToLower = "txtkode".ToLower Then
                    TryCast(ctl, TextEdit).Focus()
                    Exit For
                ElseIf ctl.name.ToString.ToLower = "txtkode".ToLower AndAlso FormName.ToLower = "" Then
                    If ctl.text = "" Then

                    End If
                End If
            Next

            FungsiControl.SetForm(Me)
        Catch ex As Exception
            XtraMessageBox.Show(ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            Windows.Forms.Cursor.Current = Cursors.Default
            dlg.Close()
            dlg.Dispose()
        End Try

    End Sub

    Public Sub Batal()
        If isNew Then

        Else
            BS.CancelEdit()

        End If
        DialogResult = Windows.Forms.DialogResult.Cancel
        Me.Close()

    End Sub

    Public Function Simpan(ByRef PesanSalah As String) As Boolean
        Dim ODC As SqlCommandBuilder
        Dim Sukses As Boolean = False
        Dim cn As New SqlConnection(StrKonSql)
        Dim ocmd2 As New SqlCommand
        ocmd2.Connection = cn
        ocmd2.CommandType = CommandType.Text
        If isNew Then
            'Dim hasil = From control In LC1.Controls Where control.name.ToString.ToLower = "txtnoid"
            'For Each x In hasil
            '    x.editvalue = GetNewID(TableName)
            '    NoID = x.editvalue
            '    ocmd2.CommandText = "select * from " & TableName & " where noid=" & x.editvalue
            '    Exit For
            'Next
            Dim ctrl As Control
            Dim xx As DevExpress.XtraEditors.TextEdit
            For Each ctrl In LC1.Controls
                If ctrl.Name.ToString.ToLower = "txtnoid" Then
                    xx = DirectCast(ctrl, DevExpress.XtraEditors.TextEdit)
                    xx.EditValue = GetNewID(TableName)
                    NoID = xx.EditValue
                    ocmd2.CommandText = "select * from " & TableName & " where noid=" & NullToLong(xx.EditValue)
                    Exit For
                End If
            Next
        Else
            ocmd2.CommandText = "select * from " & TableName & " where noid=" & NoID.ToString
        End If
        cn.Open()
        oda2.Dispose()
        oda2 = New SqlDataAdapter(ocmd2)

        Try

            Me.Validate()
            BS.EndEdit()
            ODC = New SqlCommandBuilder(oda2)
            oda2.Update(ds.Tables("Data"))
            ODC.Dispose()
            oda2.Dispose()
            Sukses = True
        Catch ex As Exception
            PesanSalah = ex.Message
        Finally

        End Try
        Return Sukses

    End Function



    Private Sub RestoreLayout()
        LC1.RestoreLayoutFromXml(FolderLayouts & FormName & ".xml")
    End Sub

    Private Sub txtIDGudangAsal_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim ds As New DataSet
        Try
            'ds = ExecuteDataset("MGudang", "SELECT MGudang.NoID, MGudang.Kode, MGudang.Nama, MWilayah.Nama AS Wilayah FROM MGudang LEFT JOIN MWilayah ON MWilayah.NoID=MGudang.IDWilayah WHERE MGudang.IDWilayah=" & NullTolong(EksekusiSQlSkalarNew("SELECT IDWilayah FROM MGudang WHERE NoID=" & NullTolong(sender.editvalue))))
            ds = ExecuteDataset("MGudang", SQLtxtGudangTujuan & " AND MGudang.IDWilayah=" & NullToLong(EksekusiSQlSkalarNew("SELECT IDWilayah FROM MGudang WHERE NoID=" & NullToLong(sender.editvalue))))
            If Not ds Is Nothing Then
                For Each ctrl In LC1.Controls
                    If ctrl.name.ToString.ToLower = "txtidgudangtujuan" Then
                        ctrl.properties.datasource = ds.Tables("MGudang")
                    End If
                Next
            End If
        Catch ex As Exception
            XtraMessageBox.Show("Informasi " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
        Finally
            ds.Dispose()
        End Try
    End Sub

    Sub IsiDefault()
        Dim str As String()
        For Each ctrl In LC1.Controls
            If ctrl.tag <> "" Then
                str = Split(ctrl.tag, ";")
                For i = 0 To UBound(str)
                    If str(i).Substring(0, 3) = "df:" Then
                        Select Case str(i).Substring(3).Trim.ToLower
                            Case "today"
                                ctrl.editvalue = TanggalSystem
                            Case "now"
                                ctrl.editvalue = TanggalSystem
                            Case Else
                                If TypeOf ctrl Is CalcEdit Then
                                    ctrl.editvalue = NullToDbl(str(i).Substring(3))
                                ElseIf TypeOf ctrl Is CheckEdit Then
                                    ctrl.checked = NullToBool(str(i).Substring(3))
                                Else
                                    ctrl.editvalue = str(i).Substring(3)
                                End If
                        End Select
                    End If
                Next
            End If
            If TypeKodeBarang = TypeKodeBarang_.TidakBerhubungan AndAlso ctrl.Name.ToString.ToLower = "txtbarcode".ToLower Then
                ctrl.editvalue = Append_EAN13_Checksum("8" & (NullToLong(EksekusiSQlSkalarNew("SELECT MAX(NoID) FROM MBarangD")).ToString("0000000000") & 1))
            End If
        Next
    End Sub

    Private Sub lkEdit_ButtonClick(ByVal sender As Object, ByVal e As DevExpress.XtraEditors.Controls.ButtonPressedEventArgs)
        Dim lked As DevExpress.XtraEditors.SearchLookUpEdit = Nothing
        lked = TryCast(sender, DevExpress.XtraEditors.SearchLookUpEdit)
        If lked.Properties.ReadOnly = True Then Exit Sub
        ' lkEdit = sender
        Select Case e.Button.Kind
            Case DevExpress.XtraEditors.Controls.ButtonPredefines.Delete
                sender.editvalue = -1
                lkEdit_EditValueChanged(sender, e)
            Case DevExpress.XtraEditors.Controls.ButtonPredefines.Ellipsis
                Dim strsql() As String = {""}
                Dim i As Integer
                i = InStr(sender.tag.ToString, "sqllookup:")
                If i >= 1 Then
                    strsql = Split(sender.tag.ToString.Substring((i - 1) + 11 - 1), ";")
                End If
                If strsql(0) <> "" Then
                    Dim frmlookup As New frmLookup
                    frmlookup.Strsql = strsql(0)
                    frmlookup.FormName = sender.name
                    If frmlookup.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
                        sender.editvalue = frmlookup.NoID
                    End If
                    frmlookup.Dispose()
                Else
                    XtraMessageBox.Show("Table lookup belum didefinisikan", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                End If
        End Select
    End Sub

    Private Sub txtEdit_EditValueChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtEdit.EditValueChanged

    End Sub

    Function getValueFromFunction(ByVal ekspresi As String) As Double
        '[qty]*[harga]*(1-[disk1]/100)
        Dim lastexpresi As String = ekspresi
        Dim nmobject As String
        While lastexpresi.Contains("[")
            Dim a As Integer = Strings.InStr(lastexpresi, "[")
            Dim b As Integer = Strings.InStr(lastexpresi, "]")
            nmobject = lastexpresi.Substring(a, b - a - 1)
            For Each ctrl In LC1.Controls
                If ctrl.name.ToString.ToLower = "txt" + nmobject.ToLower Then
                    lastexpresi = Replace(lastexpresi, "[" + nmobject + "]", NullToDbl(ctrl.editvalue).ToString)
                    Exit For
                End If
            Next
        End While
        Return Evaluate(lastexpresi)
    End Function

    Private Sub txtEdit_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim str As String()
        Dim ekspresi As String()
        Dim operasi As String()
        If sender.tag <> "" Then
            str = Split(sender.tag, ";")
            For i = 0 To UBound(str)
                If str(i).Substring(0, 3) = "fn:" Then 'fn:[Jumlah]=[Qty]*[Harga]*(1-[Disk1]/100) | [Disk1Rp]=[Harga]*[Disk1]/100
                    ekspresi = Split(str(i).Substring(3).Trim.ToLower, "|")
                    For j = 0 To UBound(ekspresi)
                        operasi = (Split(ekspresi(j), "="))
                        For Each ctrl In LC1.Controls
                            'MsgBox(ctrl.name.ToString)
                            If ctrl.name.ToString.ToLower = "txt" + operasi(0).Trim.ToLower Then
                                ctrl.editvalue = getValueFromFunction(operasi(1).Trim.ToLower)
                                Exit For
                            End If
                        Next
                    Next
                ElseIf str(i).Substring(0, 3) = "lu:" Then 'lu:[Alamat]=[mkota].[alamat] | [Telp]=[mkota].[Telp]
                    ekspresi = Split(str(i).Substring(3).Trim.ToLower, "|")
                    For j = 0 To UBound(ekspresi)
                        operasi = (Split(ekspresi(j), "="))
                        For Each ctrl In LC1.Controls
                            'MsgBox(ctrl.name.ToString)
                            If ctrl.name.ToString.ToLower = "txt" + operasi(0).Trim.ToLower Then
                                ctrl.editvalue = getValueFromLookup(sender, operasi(1).Trim.ToLower)
                                Exit For
                            End If

                        Next
                    Next
                End If
            Next
        End If
    End Sub

    Private Sub lkEdit_EditValueChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        If FormName.ToLower <> "" AndAlso isProsesLoad Then Exit Sub
        Dim str As String()
        Dim ekspresi As String()
        Dim operasi As String()
        If sender.tag <> "" Then
            str = Split(sender.tag, ";")
            For i = 0 To UBound(str)
                If str(i).Substring(0, 3) = "fn:" Then 'fn:[Jumlah]=[Qty]*[Harga]*(1-[Disk1]/100) | [Disk1Rp]=[Harga]*[Disk1]/100
                    ekspresi = Split(str(i).Substring(3).Trim.ToLower, "|")
                    For j = 0 To UBound(ekspresi)
                        operasi = (Split(ekspresi(j), "="))
                        For Each ctrl In LC1.Controls
                            'MsgBox(ctrl.name.ToString)
                            If ctrl.name.ToString.ToLower = "txt" + operasi(0).Trim.ToLower Then
                                ctrl.editvalue = getValueFromFunction(operasi(1).Trim.ToLower)
                                Exit For
                            End If
                        Next
                    Next
                ElseIf str(i).Substring(0, 3) = "lu:" Then 'lu:[Alamat]=[mkota].[alamat] | [Telp]=[mkota].[Telp]
                    ekspresi = Split(str(i).Substring(3).Trim, "|")
                    For j = 0 To UBound(ekspresi)
                        operasi = (Split(ekspresi(j), "="))
                        For Each ctrl In LC1.Controls
                            'MsgBox(ctrl.name.ToString)
                            'MsgBox(ctrl.name.ToString)
                            If ctrl.name.ToString.ToLower = "txt" + operasi(0).Trim.ToLower Then
                                If TryCast(sender, SearchLookUpEdit).Text = "" Then
                                    If TryCast(ctrl, TextEdit).Properties.Mask.MaskType = Mask.MaskType.Numeric Then
                                        TryCast(ctrl, TextEdit).EditValue = -1
                                    ElseIf TryCast(ctrl, TextEdit).Properties.Mask.MaskType = Mask.MaskType.DateTime Then
                                        TryCast(ctrl, DateEdit).DateTime = Date.Today
                                    Else
                                        TryCast(ctrl, TextEdit).EditValue = ""
                                    End If
                                Else
                                    ctrl.editvalue = getValueFromLookup(sender, operasi(1).Trim)
                                End If
                                Exit For
                            End If

                        Next
                    Next
                End If
            Next
        End If
        If FormName = "EntriGudang" AndAlso TryCast(sender, SearchLookUpEdit).Name.ToLower = "txtidwilayah".ToLower Then
            Dim SQL As String
            Dim dswil As New DataSet
            Try
                SQL = "SELECT * FROM MWilayah WHERE NoID=" & NullToLong(TryCast(sender, SearchLookUpEdit).EditValue)
                dswil = ExecuteDataset("MWilayah", SQL)
                If dswil.Tables("MWilayah").Rows.Count >= 1 Then
                    For Each ctl In LC1.Controls
                        If ctl.Name.ToLower = "txtidkota".ToLower Then
                            TryCast(ctl, SearchLookUpEdit).EditValue = NullToLong(dswil.Tables(0).Rows(0).Item("IDKota"))
                        ElseIf ctl.Name.ToLower = "txtalamat".ToLower Then
                            TryCast(ctl, TextEdit).Text = NullToStr(dswil.Tables(0).Rows(0).Item("Alamat"))
                        ElseIf ctl.Name.ToLower = "txttelpon".ToLower Then
                            TryCast(ctl, TextEdit).Text = NullToStr(dswil.Tables(0).Rows(0).Item("Telp"))
                        End If
                    Next
                End If
            Catch ex As Exception

            Finally
                dswil.Dispose()
            End Try

        End If
    End Sub

    Private Sub clcEdit_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim str As String()
        Dim ekspresi As String()
        Dim operasi As String()
        If sender.tag <> "" Then
            str = Split(sender.tag, ";")
            For i = 0 To UBound(str)
                If str(i).Substring(0, 3) = "fn:" Then 'fn:[Jumlah]=[Qty]*[Harga]*(1-[Disk1]/100) | [Disk1Rp]=[Harga]*[Disk1]/100
                    ekspresi = Split(str(i).Substring(3).Trim.ToLower, "|")
                    For j = 0 To UBound(ekspresi)
                        operasi = (Split(ekspresi(j), "="))
                        For Each ctrl In LC1.Controls
                            'MsgBox(ctrl.name.ToString)
                            If ctrl.name.ToString.ToLower = "txt" + operasi(0).Trim.ToLower Then
                                ctrl.editvalue = getValueFromFunction(operasi(1).Trim.ToLower)
                                Exit For
                            End If

                        Next
                    Next
                ElseIf str(i).Substring(0, 3) = "lu:" Then 'lu:[Alamat]=[mkota].[alamat] | [Telp]=[mkota].[Telp]
                    ekspresi = Split(str(i).Substring(3).Trim.ToLower, "|")
                    For j = 0 To UBound(ekspresi)
                        operasi = (Split(ekspresi(j), "="))
                        For Each ctrl In LC1.Controls
                            'MsgBox(ctrl.name.ToString)
                            If ctrl.name.ToString.ToLower = "txt" + operasi(0).Trim.ToLower Then
                                ctrl.editvalue = getValueFromLookup(sender, operasi(1).Trim.ToLower)
                                Exit For
                            End If

                        Next
                    Next
                End If
            Next
        End If
    End Sub

    Private Sub BarButtonItem2_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem2.ItemClick
        Batal()
    End Sub

    Private Sub BarButtonItem1_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem1.ItemClick
        cmdSave.PerformClick()
    End Sub

    Private Function IsValidasi()
        Dim txtGudangAsal As New SearchLookUpEdit
        Dim txtGudangTujuan As New SearchLookUpEdit
        Dim txtIDJenisBarang As New SearchLookUpEdit
        Dim txtIDJenisHarga As New SearchLookUpEdit
        Dim txtIdpegawai As New SearchLookUpEdit
        Dim txtIsiKarton As New CalcEdit
        Dim txtSatuan As New SearchLookUpEdit
        Dim ds As New DataSet
        Try
            For Each ctrl In LC1.Controls
                If ctrl.name.tolower = "txtkode".ToLower Then
                    Dim txtKode As TextEdit
                    txtKode = CType(ctrl, TextEdit)
                    If txtKode.Text.Trim = "" Then
                        XtraMessageBox.Show("Kode masih kosong.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Warning)
                        txtKode.Focus()
                        Return False
                    End If
                    If TableName <> "MBarangDPaket" Then
                        If CekKodeValid(txtKode.Text, KodeLama, TableName, "Kode", Not isNew) Then
                            XtraMessageBox.Show("Kode Sudah dipakai.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Warning)
                            txtKode.Focus()
                            Return False
                        End If
                    End If
                    'ElseIf ctrl.name.tolower = "txtnoid".ToLower Then
                    '    NoID = NullToLong(ctrl.editvalue)
                ElseIf ctrl.name.tolower = "txtBarcode".ToLower Then
                    Dim txtBarcode As New TextEdit
                    txtBarcode = CType(ctrl, TextEdit)
                    If TableName.ToUpper = "MBarangD".ToUpper Then
                        If CekKodeValid(txtBarcode.Text, BarcodeLama, TableName, "Barcode", Not isNew) Then
                            XtraMessageBox.Show("Barcode Sudah dipakai.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Warning)
                            txtBarcode.Focus()
                            Return False
                        End If
                    End If
                ElseIf ctrl.name.tolower = "txtnama".ToLower Then
                    Dim txtNama As TextEdit
                    txtNama = CType(ctrl, TextEdit)
                    If txtNama.Text.Trim = "" Then
                        XtraMessageBox.Show("Nama masih kosong.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Warning)
                        txtNama.Focus()
                        Return False
                    End If
                    If TableName <> "MBarangDPaket" Then

                        If CekKodeValid(txtNama.Text, NamaLama, TableName, "Nama", Not isNew) Then
                            If XtraMessageBox.Show("Nama Sudah dipakai, yakin ingin meneruskan penyimpanan.", NamaAplikasi, MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) = Windows.Forms.DialogResult.No Then
                                txtNama.Focus()
                                Return False
                            End If
                        End If
                    End If
                ElseIf FormName.ToLower = "EntriAlamatDCustomer".ToLower AndAlso ctrl.name.tolower = "txtIdjenisbarang".ToLower Then
                    txtIDJenisBarang = CType(ctrl, SearchLookUpEdit)
                ElseIf FormName.ToLower = "EntriAlamatDCustomer".ToLower AndAlso ctrl.name.tolower = "txtIdjenisharga".ToLower Then
                    txtIDJenisHarga = CType(ctrl, SearchLookUpEdit)
                ElseIf ctrl.name.tolower = "txtidgudangasal".ToLower Then
                    txtGudangAsal = CType(ctrl, SearchLookUpEdit)
                ElseIf ctrl.name.tolower = "txtidgudangtujuan".ToLower Then
                    txtGudangTujuan = CType(ctrl, SearchLookUpEdit)
                ElseIf ctrl.name.tolower = "txtIdpegawai".ToLower Then
                    txtIdpegawai = CType(ctrl, SearchLookUpEdit)
                ElseIf ctrl.name.tolower = "txtKonversi".ToLower Then
                    txtIsiKarton = CType(ctrl, CalcEdit)
                    KonversiCTN = NullToDbl(txtIsiKarton.EditValue)
                ElseIf ctrl.name.tolower = "txtIDSatuan".ToLower Then
                    txtSatuan = CType(ctrl, SearchLookUpEdit)
                    txtSatuanMBarangD = txtSatuan
                End If
            Next

            If TableName.ToLower = "mrulemutasi" Then
                If txtGudangAsal.Text.Trim = "" Then
                    XtraMessageBox.Show("Gudang asal masih kosong.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
                    txtGudangAsal.Focus()
                    Return False
                End If
                If txtIdpegawai.Text.Trim = "" Then
                    XtraMessageBox.Show("Nama Pegawai masih kosong.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
                    txtIdpegawai.Focus()
                    Return False
                End If
                If txtGudangTujuan.EditValue = txtGudangAsal.EditValue Then
                    XtraMessageBox.Show("Gudang asal dan tujuan sama.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
                    txtGudangTujuan.Focus()
                    Return False
                End If
                ds = ExecuteDataset("MGudang", "SELECT MRuleMutasi.* FROM MRuleMutasi WHERE MRuleMutasi.NoID<>" & NoID & " AND MRuleMutasi.IDGudangAsal=" & NullToLong(txtGudangAsal.EditValue) & " AND MRuleMutasi.IDGudangTujuan=" & NullToLong(txtGudangTujuan.EditValue) & " AND MRuleMutasi.IDPegawai=" & NullToLong(txtIdpegawai.EditValue))
                If ds.Tables("MGudang").Rows.Count >= 1 Then
                    XtraMessageBox.Show("Rule gudang ini sudah ada.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
                    txtGudangTujuan.Focus()
                    Return False
                End If
            End If

            If TableName.ToLower = "malamatd" Then
                If txtIDJenisBarang.Text.Trim = "" Then
                    XtraMessageBox.Show("Jenis Barang masih kosong.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
                    txtIDJenisBarang.Focus()
                    Return False
                End If
                If txtIDJenisHarga.Text.Trim = "" Then
                    XtraMessageBox.Show("Default Tipe Harga masih kosong.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
                    txtIDJenisHarga.Focus()
                    Return False
                End If
                ds = ExecuteDataset("MAlamatD", "SELECT MAlamatD.* FROM MAlamatD WHERE MAlamatD.NoID<>" & NoID & " AND MAlamatD.IDAlamat=" & IDParent & " AND MAlamatD.IDJenisBarang=" & NullToLong(txtIDJenisBarang.EditValue))
                If ds.Tables("MAlamatD").Rows.Count >= 1 Then
                    XtraMessageBox.Show("Jenis Barang ini sudah ada.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
                    txtIDJenisBarang.Focus()
                    Return False
                End If
            End If

            If TableName.ToLower = "mbarangd" Then
                If NullToDbl(txtIsiKarton.EditValue) <= 0 Then
                    If XtraMessageBox.Show("Isi konversi harus lebih besar 0" & vbCrLf & "Ingin tetap melakukan penyimpanan?", NamaAplikasi, MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) = Windows.Forms.DialogResult.No Then
                        txtIsiKarton.Focus()
                        Return False
                    End If
                End If
                'ds = ExecuteDataset("MBarangD", "SELECT MBarangD.* FROM MBarangD WHERE MBarangD.NoID<>" & NoID & " AND MBarangD.IDBarang=" & IDParent & " AND MBarangD.IDSatuan=" & NullTolong(txtSatuan.EditValue))
                'If ds.Tables("MBarangD").Rows.Count >= 1 Then
                '    XtraMessageBox.Show("Satuan Barang ini sudah ada.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
                '    txtSatuan.Focus()
                '    Return False
                'End If
            End If

            Return True
        Catch ex As Exception
            Return False
        Finally
            ds.Dispose()
        End Try
    End Function

    Private Sub SimpleButton1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSave.Click
        Dim PesanSalah As String = ""
        If IsValidasi() Then
            If Simpan(PesanSalah) = True Then
                Me.DialogResult = Windows.Forms.DialogResult.OK
                Me.Close()
            Else
                XtraMessageBox.Show("Info Kesalahan : " & vbCrLf & PesanSalah, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            End If
        End If
    End Sub

    Private Sub SimpleButton3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SimpleButton3.Click
        Batal()
    End Sub

    Private Sub SimpleButton1_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cmdSave.KeyDown

    End Sub

    Private Sub SimpleButton1_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles cmdSave.MouseDown

    End Sub

    Private Sub BarButtonItem3_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem3.ItemClick
        Dim xOtorisasi As New frmOtorisasiAdmin
        If XtraMessageBox.Show("Simpan Layout?", NamaAplikasi, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = Windows.Forms.DialogResult.Yes AndAlso xOtorisasi.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
            Ini.TulisIniPath(FolderLayouts & FormName & ".ini", "Form", "Width", Me.Width)
            Ini.TulisIniPath(FolderLayouts & FormName & ".ini", "Form", "Height", Me.Height)

            LC1.SaveLayoutToXml(FolderLayouts & FormName & ".xml")

        End If
        xOtorisasi.Dispose()
    End Sub

    Private Sub frmSimpleEntri_Shown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Shown
        If isNew Then
            BS.AddNew()
            IsiDefault()
        Else
        End If
        isProsesLoad = False
    End Sub

    Private Sub txtEdit_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim txt = TryCast(sender, TextEdit)
        If txt.Properties.Mask.MaskType = Mask.MaskType.Numeric Then
            txt.SelectAll()
        End If
    End Sub

    Private Sub clcEdit_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim txt = TryCast(sender, CalcEdit)
        If txt.Properties.Mask.MaskType = Mask.MaskType.Numeric Then
            txt.SelectAll()
        End If
    End Sub

    'Private Sub txtEdit_MouseDn(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs)
    '    Dim txt = TryCast(sender, TextEdit)
    '    txt.SelectAll()
    'End Sub
    'Private Sub clcEdit_MouseDn(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs)
    '    Dim txt = TryCast(sender, CalcEdit)
    '    txt.SelectAll()
    'End Sub

    Private Sub lkEdit_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        If isProsesLoad Then Exit Sub
        Dim str As String()
        Dim ekspresi As String()
        Dim operasi As String()
        If sender.tag <> "" Then
            str = Split(sender.tag, ";")
            For i = 0 To UBound(str)
                If str(i).Substring(0, 3) = "fn:" Then 'fn:[Jumlah]=[Qty]*[Harga]*(1-[Disk1]/100) | [Disk1Rp]=[Harga]*[Disk1]/100
                    ekspresi = Split(str(i).Substring(3).Trim.ToLower, "|")
                    For j = 0 To UBound(ekspresi)
                        operasi = (Split(ekspresi(j), "="))
                        For Each ctrl In LC1.Controls
                            'MsgBox(ctrl.name.ToString)
                            If ctrl.name.ToString.ToLower = "txt" + operasi(0).Trim.ToLower Then
                                ctrl.editvalue = getValueFromFunction(operasi(1).Trim.ToLower)
                                Exit For
                            End If

                        Next
                    Next
                ElseIf str(i).Substring(0, 3) = "lu:" Then 'lu:[Alamat]=[mkota].[alamat] | [Telp]=[mkota].[Telp]
                    ekspresi = Split(str(i).Substring(3).Trim, "|")
                    For j = 0 To UBound(ekspresi)
                        operasi = (Split(ekspresi(j), "="))
                        For Each ctrl In LC1.Controls
                            'MsgBox(ctrl.name.ToString)
                            'MsgBox(ctrl.name.ToString)
                            If ctrl.name.ToString.ToLower = "txt" + operasi(0).Trim.ToLower Then
                                ctrl.editvalue = getValueFromLookup(sender, operasi(1).Trim)
                                Exit For
                            End If

                        Next
                    Next
                End If
            Next
        End If
    End Sub

    Private Sub lkEdit_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim ds As New DataSet
        Dim lkedit As SearchLookUpEdit = Nothing
        Try
            If FormName.ToLower = "entrirulemutasigudang" Then
                For Each ctrl In LC1.Controls
                    If ctrl.name.ToString.ToLower = "txtidgudangasal".ToLower Then
                        lkedit = CType(ctrl, SearchLookUpEdit)
                        Exit For
                    End If
                Next
                If Not lkedit Is Nothing Then
                    'ds = ExecuteDataset("MGudang", "SELECT MGudang.NoID, MGudang.Kode, MGudang.Nama, MWilayah.Nama AS Wilayah FROM MGudang LEFT JOIN MWilayah ON MWilayah.NoID=MGudang.IDWilayah WHERE MGudang.IDWilayah=" & NullTolong(EksekusiSQlSkalarNew("SELECT IDWilayah FROM MGudang WHERE NoID=" & NullTolong(sender.editvalue))))
                    ds = ExecuteDataset("MGudang", SQLtxtGudangTujuan & " AND MGudang.IDWilayah=" & NullToLong(EksekusiSQlSkalarNew("SELECT IDWilayah FROM MGudang WHERE NoID=" & NullToLong(lkedit.EditValue))))
                    If Not ds Is Nothing Then
                        For Each ctrl In LC1.Controls
                            If ctrl.name.ToString.ToLower = "txtidgudangtujuan" Then
                                ctrl.properties.datasource = ds.Tables("MGudang")
                            End If
                        Next
                    End If
                End If
            End If
        Catch ex As Exception
            XtraMessageBox.Show("Informasi " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
        Finally
            ds.Dispose()
        End Try
    End Sub

    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

    End Sub
End Class