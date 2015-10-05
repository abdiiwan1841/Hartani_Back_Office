Imports System.Data.SqlClient
Imports System.Data.SQLite

Imports DevExpress.XtraLayout
'Imports System.Linq
Imports DevExpress.XtraGrid.Views.Base
Imports DevExpress.XtraEditors

Public Class frmEntriKontak
    Public FormName As String = ""
    Public TableName As String = ""
    Dim Caption As String = ""
    Public isNew As Boolean = True
    Public NoID As Long = -1
    Dim oda2 As SqlDataAdapter

    Dim ds As New DataSet
    Dim BS As New BindingSource
    Dim WithEvents cb As New DevExpress.XtraEditors.Controls.EditorButton

    Dim WithEvents txtEdit As DevExpress.XtraEditors.TextEdit
    Dim WithEvents txtMemo As DevExpress.XtraEditors.MemoEdit
    Dim WithEvents clcEdit As DevExpress.XtraEditors.CalcEdit
    Dim ckedit As DevExpress.XtraEditors.CheckEdit
    Dim dtEdit As DevExpress.XtraEditors.DateEdit
    Dim WithEvents lkEdit As DevExpress.XtraEditors.SearchLookUpEdit
    Dim KodeLama As String = ""
    Dim NamaLama As String = ""

    Public IsSupplier As Boolean = False
    Public IsMember As Boolean = False
    Public IsPegawai As Boolean = False

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
    Sub GenerateForm()
        'Try
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
        '  TableName = GetTableNamebyFormname(FormName)
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
                    Select Case ds.Tables("Master").Rows(i).Item("tipe").ToString.ToLower
                        Case "int", "bigint", "smallint", "float", "numeric", "money", "real"
                            txtEdit.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric
                            txtEdit.Properties.Mask.EditMask = NullTostr(ds.Tables("Master").Rows(i).Item("format"))
                            txtEdit.Properties.Mask.UseMaskAsDisplayFormat = True
                            txtEdit.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
                            AddHandler txtEdit.KeyDown, AddressOf FungsiControl.txtNumeric_KeyDown
                        Case "date", "datetime", "time"
                            txtEdit.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.DateTime
                            txtEdit.Properties.Mask.EditMask = ds.Tables("Master").Rows(i).Item("format")
                            txtEdit.Properties.Mask.UseMaskAsDisplayFormat = True
                        Case Else
                            If Not IsDBNull(ds.Tables("Master").Rows(i).Item("format")) AndAlso ds.Tables("Master").Rows(i).Item("format") <> "" Then
                                txtEdit.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Simple
                                txtEdit.Properties.Mask.EditMask = ds.Tables("Master").Rows(i).Item("format")
                                txtEdit.Properties.Mask.UseMaskAsDisplayFormat = True
                            Else
                                txtEdit.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.None
                            End If
                    End Select
                    txtEdit.Visible = NullTobool(ds.Tables("Master").Rows(i).Item("visible"))
                    txtEdit.Properties.ReadOnly = NullTobool(ds.Tables("Master").Rows(i).Item("readonly"))
                    If NullTostr(ds.Tables("Master").Rows(i).Item("default")) <> "" Then
                        txtEdit.Tag = "df:" & NullTostr(ds.Tables("Master").Rows(i).Item("default"))
                    End If
                    If NullTostr(ds.Tables("Master").Rows(i).Item("function")) <> "" Then
                        txtEdit.Tag = IIf(txtEdit.Tag <> "", txtEdit.Tag & ";", "") & NullTostr(ds.Tables("Master").Rows(i).Item("function"))
                    End If
                    If NullTostr(ds.Tables("Master").Rows(i).Item("fieldname")) <> "" Then
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

                    itemLC = New LayoutControlItem
                    itemLC.Name = ds.Tables("Master").Rows(i).Item("caption")
                    itemLC.Parent = LC1.Root

                    itemLC.Control = txtEdit
                    AddHandler txtEdit.LostFocus, AddressOf txtEdit_LostFocus
                    AddHandler txtEdit.GotFocus, AddressOf txtEdit_GotFocus

                    If txtEdit.Name.ToLower = "txtkode".ToLower Then
                        KodeLama = txtEdit.Text
                        txtEdit.Properties.CharacterCasing = CharacterCasing.Upper
                    ElseIf txtEdit.Name.ToLower = "txtnama".ToLower Then
                        NamaLama = txtEdit.Text
                    End If

                Case "memoedit"
                    txtMemo = New DevExpress.XtraEditors.MemoEdit
                    txtMemo.Name = "txt" + ds.Tables("Master").Rows(i).Item("nama")
                    txtMemo.EnterMoveNextControl = False
                    Select Case ds.Tables("Master").Rows(i).Item("tipe").ToString.ToLower
                        Case "int", "bigint", "smallint", "float", "numeric", "money", "real"
                            txtMemo.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric
                            txtMemo.Properties.Mask.EditMask = NullTostr(ds.Tables("Master").Rows(i).Item("format"))
                            txtMemo.Properties.Mask.UseMaskAsDisplayFormat = True
                            txtMemo.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
                            AddHandler txtMemo.KeyDown, AddressOf FungsiControl.txtNumeric_KeyDown
                        Case "date", "datetime", "time"
                            txtMemo.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.DateTime
                            txtMemo.Properties.Mask.EditMask = ds.Tables("Master").Rows(i).Item("format")
                            txtMemo.Properties.Mask.UseMaskAsDisplayFormat = True
                        Case Else
                            If Not IsDBNull(ds.Tables("Master").Rows(i).Item("format")) AndAlso ds.Tables("Master").Rows(i).Item("format") <> "" Then
                                txtMemo.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Custom
                                txtMemo.Properties.Mask.EditMask = ds.Tables("Master").Rows(i).Item("format")
                                txtMemo.Properties.Mask.UseMaskAsDisplayFormat = True
                            Else
                                txtMemo.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.None
                            End If
                    End Select
                    txtMemo.Visible = NullTobool(ds.Tables("Master").Rows(i).Item("visible"))
                    txtMemo.Properties.ReadOnly = NullTobool(ds.Tables("Master").Rows(i).Item("readonly"))
                    If NullTostr(ds.Tables("Master").Rows(i).Item("default")) <> "" Then
                        txtMemo.Tag = "df:" & NullTostr(ds.Tables("Master").Rows(i).Item("default"))
                    End If
                    If NullTostr(ds.Tables("Master").Rows(i).Item("function")) <> "" Then
                        txtMemo.Tag = IIf(txtEdit.Tag <> "", txtEdit.Tag & ";", "") & NullTostr(ds.Tables("Master").Rows(i).Item("function"))
                    End If
                    If NullTostr(ds.Tables("Master").Rows(i).Item("fieldname")) <> "" Then
                        txtMemo.DataBindings.Add("Editvalue", BS _
                     , ds.Tables("Master").Rows(i).Item("fieldname"))
                    End If

                    If ds.Tables("Master").Rows(i).Item("tipe").ToString.Length >= 10 AndAlso ds.Tables("Master").Rows(i).Item("tipe").ToString.Substring(0, 7).ToLower = "varchar" Then
                        'varchar(120)
                        If IsNumeric(ds.Tables("Master").Rows(i).Item("tipe").ToString.Substring(8, ds.Tables("Master").Rows(i).Item("tipe").ToString.Length - 9)) Then
                            txtMemo.Properties.MaxLength = CInt(ds.Tables("Master").Rows(i).Item("tipe").ToString.Substring(8, ds.Tables("Master").Rows(i).Item("tipe").ToString.Length - 9))
                        End If
                    End If
                    If (ds.Tables("Master").Rows(i).Item("tipe").ToString.Length >= 10) AndAlso ds.Tables("Master").Rows(i).Item("tipe").ToString.Substring(0, 7).ToLower = "numeric" Then
                        'Numeric(120)
                        If IsNumeric(ds.Tables("Master").Rows(i).Item("tipe").ToString.Substring(8, ds.Tables("Master").Rows(i).Item("tipe").ToString.Length - 9)) Then
                            txtMemo.Properties.MaxLength = CInt(ds.Tables("Master").Rows(i).Item("tipe").ToString.Substring(8, ds.Tables("Master").Rows(i).Item("tipe").ToString.Length - 9))
                        End If
                    End If
                    If (ds.Tables("Master").Rows(i).Item("tipe").ToString.Length >= 8) AndAlso ds.Tables("Master").Rows(i).Item("tipe").ToString.Substring(0, 5).ToLower = "money" Then
                        'Money(120)
                        If IsNumeric(ds.Tables("Master").Rows(i).Item("tipe").ToString.Substring(6, ds.Tables("Master").Rows(i).Item("tipe").ToString.Length - 7)) Then
                            txtMemo.Properties.MaxLength = CInt(ds.Tables("Master").Rows(i).Item("tipe").ToString.Substring(6, ds.Tables("Master").Rows(i).Item("tipe").ToString.Length - 7))
                        End If
                    End If

                    itemLC = New LayoutControlItem
                    itemLC.Name = ds.Tables("Master").Rows(i).Item("caption")
                    itemLC.Parent = LC1.Root

                    itemLC.Control = txtMemo
                    'AddHandler txtEdit.LostFocus, AddressOf txtEdit_LostFocus
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
                            AddHandler clcEdit.KeyDown, AddressOf FungsiControl.txtNumeric_KeyDown
                        Case "date", "datetime", "time"
                            clcEdit.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.DateTime
                            clcEdit.Properties.Mask.EditMask = ds.Tables("Master").Rows(i).Item("format")
                            clcEdit.Properties.Mask.UseMaskAsDisplayFormat = True
                        Case Else
                            If Not IsDBNull(ds.Tables("Master").Rows(i).Item("format")) AndAlso ds.Tables("Master").Rows(i).Item("format") <> "" Then
                                clcEdit.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Custom
                                clcEdit.Properties.Mask.EditMask = ds.Tables("Master").Rows(i).Item("format")
                                clcEdit.Properties.Mask.UseMaskAsDisplayFormat = True
                            Else
                                clcEdit.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.None
                            End If
                    End Select
                    clcEdit.Visible = NullTobool(ds.Tables("Master").Rows(i).Item("visible"))
                    clcEdit.Properties.ReadOnly = NullTobool(ds.Tables("Master").Rows(i).Item("readonly"))
                    If NullTostr(ds.Tables("Master").Rows(i).Item("default")) <> "" Then
                        clcEdit.Tag = "df:" & NullTostr(ds.Tables("Master").Rows(i).Item("default"))
                    End If
                    If NullTostr(ds.Tables("Master").Rows(i).Item("function")) <> "" Then
                        clcEdit.Tag = IIf(clcEdit.Tag <> "", clcEdit.Tag & ";", "") & NullTostr(ds.Tables("Master").Rows(i).Item("function"))
                    End If
                    If NullTostr(ds.Tables("Master").Rows(i).Item("fieldname")) <> "" Then
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
                    ckedit.Visible = NullTobool(ds.Tables("Master").Rows(i).Item("visible"))
                    ckedit.Properties.ReadOnly = NullTobool(ds.Tables("Master").Rows(i).Item("readonly"))
                    ckedit.DataBindings.Add("editvalue", BS _
                      , ds.Tables("Master").Rows(i).Item("fieldname"))

                    'ckedit.Visible = NullTobool(ds.Tables("Master").Rows(i).Item("visible"))
                    'ckedit.Properties.ReadOnly = NullTobool(ds.Tables("Master").Rows(i).Item("readonly"))
                    If NullTostr(ds.Tables("Master").Rows(i).Item("default")) <> "" Then
                        ckedit.Tag = "df:" & NullTostr(ds.Tables("Master").Rows(i).Item("default"))
                    End If
                    If NullTostr(ds.Tables("Master").Rows(i).Item("function")) <> "" Then
                        ckedit.Tag = IIf(ckedit.Tag <> "", ckedit.Tag & ";", "") & NullTostr(ds.Tables("Master").Rows(i).Item("function"))
                    End If
                    'If NullTostr(ds.Tables("Master").Rows(i).Item("fieldname")) <> "" Then
                    '    ckedit.DataBindings.Add("Text", BS _
                    ' , ds.Tables("Master").Rows(i).Item("fieldname"))
                    'End If

                    itemLC = New LayoutControlItem
                    itemLC.Name = ds.Tables("Master").Rows(i).Item("caption")

                    itemLC.Parent = LC1.Root
                    itemLC.Control = ckedit

                    If Not isNew AndAlso ckedit.Name.ToUpper = "txtispkp".ToUpper Then
                        ckedit.Properties.ReadOnly = True
                        AddHandler ckedit.KeyDown, AddressOf ckEdit_KeyDown
                    End If
                Case "dateedit"
                    dtEdit = New DevExpress.XtraEditors.DateEdit
                    dtEdit.Name = "txt" + ds.Tables("Master").Rows(i).Item("nama")
                    dtEdit.EnterMoveNextControl = True
                    dtEdit.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.DateTime
                    dtEdit.Properties.Mask.EditMask = NullTostr(ds.Tables("Master").Rows(i).Item("format"))

                    dtEdit.Properties.Mask.UseMaskAsDisplayFormat = True
                    dtEdit.Visible = NullTobool(ds.Tables("Master").Rows(i).Item("visible"))
                    dtEdit.Properties.ReadOnly = NullTobool(ds.Tables("Master").Rows(i).Item("readonly"))
                    If NullTostr(ds.Tables("Master").Rows(i).Item("default")) <> "" Then
                        dtEdit.Tag = "df:" & NullTostr(ds.Tables("Master").Rows(i).Item("default"))
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
                    lkEdit.Properties.Buttons.Add(New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Ellipsis))
                    lkEdit.Properties.Buttons.Add(cb)
                    lkEdit.Properties.NullText = ""
                    lkEdit.Name = "txt" + ds.Tables("Master").Rows(i).Item("nama")
                    lkEdit.Properties.DisplayMember = NullTostr(ds.Tables("Master").Rows(i).Item("lookupdisplay"))
                    lkEdit.Properties.ValueMember = NullTostr(ds.Tables("Master").Rows(i).Item("lookupvalue"))
                    lkEdit.EnterMoveNextControl = True
                    'lkEdit.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.DateTime
                    'lkEdit.Properties.Mask.EditMask = ds.Tables("Master").Rows(i).Item("format")
                    'lkEdit.Properties.Mask.UseMaskAsDisplayFormat = True
                    lkEdit.Visible = NullTobool(ds.Tables("Master").Rows(i).Item("visible"))
                    lkEdit.Properties.ReadOnly = NullTobool(ds.Tables("Master").Rows(i).Item("readonly"))
                    If NullTostr(ds.Tables("Master").Rows(i).Item("default")) <> "" Then
                        lkEdit.Tag = "df:" & NullTostr(ds.Tables("Master").Rows(i).Item("default"))
                    End If
                    If NullTostr(ds.Tables("Master").Rows(i).Item("function")) <> "" Then
                        lkEdit.Tag = IIf(lkEdit.Tag <> "", lkEdit.Tag & ";", "") & NullTostr(ds.Tables("Master").Rows(i).Item("function"))
                    End If
                    If NullTostr(ds.Tables("Master").Rows(i).Item("fieldname")) <> "" Then
                        lkEdit.DataBindings.Add("editvalue", BS _
                     , ds.Tables("Master").Rows(i).Item("fieldname"))
                    End If


                    itemLC = New LayoutControlItem
                    itemLC.Name = ds.Tables("Master").Rows(i).Item("caption")
                    itemLC.Parent = LC1.Root
                    itemLC.Control = lkEdit
                    If NullTostr(ds.Tables("Master").Rows(i).Item("sql")) <> "" Then
                        ocmd2.CommandText = NullTostr(ds.Tables("Master").Rows(i).Item("sql"))
                    Else
                        ocmd2.CommandText = "Select * from " & ds.Tables("Master").Rows(i).Item("tablelookup")
                    End If
                    lkEdit.Tag = IIf(NullTostr(lkEdit.Tag).Length > 0, lkEdit.Tag & ";", "").ToString & "sqllookup:" & ocmd2.CommandText

                    oda2 = New SqlDataAdapter(ocmd2)
                    oda2.Fill(dsLookUp, ds.Tables("Master").Rows(i).Item("tablelookup"))
                    lkEdit.Properties.DataSource = dsLookUp.Tables(ds.Tables("Master").Rows(i).Item("tablelookup"))
                    AddHandler lkEdit.EditValueChanged, AddressOf lkEdit_EditValueChanged
                    AddHandler lkEdit.ButtonClick, AddressOf lkEdit_ButtonClick
                    dsLookUp.Dispose()
                    If System.IO.File.Exists(folderLayouts & FormName & lkEdit.Name & ".xml") Then
                        lkEdit.Properties.View.RestoreLayoutFromXml(folderLayouts & FormName & lkEdit.Name & ".xml")
                    End If
            End Select
        Next

        Dim STR() As String
        For Each ctrl In LC1.Controls
            If ctrl.tag <> "" Then
                STR = Split(ctrl.tag, ";")
                For i = 0 To UBound(STR)
                    If STR(i).Substring(0, 2).ToLower = "lu" Then
                        lkEdit_EditValueChanged(ctrl, Nothing)
                    End If
                Next
            End If
        Next

        'Catch ex As Exception
        '    XtraMessageBox.Show("Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        'End Try
    End Sub

    Private Sub ckEdit_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs)
        Dim x As New frmOtorisasiAdmin
        Try
            If e.KeyCode = Keys.F7 AndAlso x.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
                TryCast(sender, CheckEdit).Properties.ReadOnly = False
            End If
        Catch ex As Exception
        Finally
            x.Dispose()
        End Try
    End Sub

    Private Sub frmSimpleEntri_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        If DialogResult = Windows.Forms.DialogResult.Cancel AndAlso cmdSave.Enabled Then
            If XtraMessageBox.Show("Yakin ingin keluar?" & vbCrLf & "Cancel untuk membatalkan", NamaAplikasi, MessageBoxButtons.OKCancel, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) = Windows.Forms.DialogResult.Cancel Then
                e.Cancel = True
            End If
        End If
    End Sub

    Private Sub frmSimpleEntri_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim dlg As DevExpress.Utils.WaitDialogForm = Nothing
        Try
            Dim curentcursor As Cursor = Windows.Forms.Cursor.Current
            Windows.Forms.Cursor.Current = Cursors.WaitCursor
            Dim x As New Size
            x.Width = 300
            x.Height = 70
            dlg = New DevExpress.Utils.WaitDialogForm(String.Format("Creating component on " & Me.FormName.ToString & ".{0}MOHON TUNGGU ...", vbCrLf), Application.ProductName, x)
            'dlg.Width = 500
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
            If IsPegawai Then
                If System.IO.File.Exists(FolderLayouts & FormName & "IsPegawai.xml") Then
                    LC1.RestoreLayoutFromXml(FolderLayouts & FormName & "IsPegawai.xml")
                Else
                    If System.IO.File.Exists(FolderLayouts & FormName & ".xml") Then
                        LC1.RestoreLayoutFromXml(FolderLayouts & FormName & ".xml")
                    End If
                End If
            ElseIf IsMember Then
                If System.IO.File.Exists(FolderLayouts & FormName & "IsMember.xml") Then
                    LC1.RestoreLayoutFromXml(FolderLayouts & FormName & "IsMember.xml")
                Else
                    If System.IO.File.Exists(FolderLayouts & FormName & ".xml") Then
                        LC1.RestoreLayoutFromXml(FolderLayouts & FormName & ".xml")
                    End If
                End If
            ElseIf IsSupplier Then
                If System.IO.File.Exists(FolderLayouts & FormName & "IsSupplier.xml") Then
                    LC1.RestoreLayoutFromXml(FolderLayouts & FormName & "IsSupplier.xml")
                Else
                    If System.IO.File.Exists(FolderLayouts & FormName & ".xml") Then
                        LC1.RestoreLayoutFromXml(FolderLayouts & FormName & ".xml")
                    End If
                End If
                RefreshDetilNPWP()
                btnBaru.Visible = True
                btnHapus.Visible = True
                btnEdit.Visible = True
                btnRefresh.Visible = True
                XtraTabPage1.PageVisible = False
                If System.IO.File.Exists(FolderLayouts & FormName & GridView1.Name & ".xml") Then
                    GridView1.RestoreLayoutFromXml(FolderLayouts & FormName & GridView1.Name & ".xml")
                End If
            Else
                If System.IO.File.Exists(FolderLayouts & FormName & ".xml") Then
                    LC1.RestoreLayoutFromXml(FolderLayouts & FormName & ".xml")
                End If
            End If
            RefreshDetil()
            If System.IO.File.Exists(FolderLayouts & FormName & GV1.Name & ".xml") Then
                GV1.RestoreLayoutFromXml(FolderLayouts & FormName & GV1.Name & ".xml")
            End If
            'LC1.EndUpdate()
            SetTombol()
            For Each ctl In LC1.Controls
                If ctl.name.ToString.ToLower = "txtkode".ToLower Then
                    TryCast(ctl, TextEdit).Focus()
                    Exit For
                End If
            Next
            If Not IsSupervisor AndAlso Not isNew Then
                btnBaru.Enabled = False
                btnEdit.Enabled = False
                btnHapus.Enabled = False
                btnRefresh.Enabled = False
                cmdSave.Enabled = False
            ElseIf Not IsSupervisor AndAlso isNew Then
                btnBaru.Enabled = False
                btnEdit.Enabled = False
                btnHapus.Enabled = False
                btnRefresh.Enabled = False
            End If
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
            '    ocmd2.CommandText = "select * from " & TableName & " where noid=" & x.editvalue
            '    NoID = x.editvalue
            '    Exit For
            'Next
            Dim ctrl As Control
            Dim xx As DevExpress.XtraEditors.TextEdit
            For Each ctrl In LC1.Controls
                If ctrl.Name.ToString.ToLower = "txtnoid" Then
                    xx = DirectCast(ctrl, DevExpress.XtraEditors.TextEdit)
                    xx.EditValue = GetNewID(TableName)
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
        'LC1.RestoreLayoutFromXml(folderLayouts & FormName & ".xml")
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
            If NullToStr(ctrl.name.ToString).ToUpper = "txtISCUSTOMER".ToUpper Then
                If IsMember Then
                    ctrl.editvalue = True
                End If
            ElseIf NullToStr(ctrl.name.ToString).ToUpper = "txtISPegawai".ToUpper Then
                If IsPegawai Then
                    ctrl.editvalue = True
                End If
            ElseIf NullToStr(ctrl.name.ToString).ToUpper = "txtISSupplier".ToUpper Then
                If IsSupplier Then
                    ctrl.editvalue = True
                End If
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
                    lastexpresi = Replace(lastexpresi, "[" + nmobject + "]", ctrl.editvalue)
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



    Private Sub lkEdit_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles lkEdit.LostFocus

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

    Private Sub SimpleButton1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSave.Click
        Dim PesanSalah As String = ""
        If IsValidasi() Then
            If Simpan(PesanSalah) = True Then
                'If isNew Then
                '    isNew = False
                '    SetTombol()
                '    For Each ctrl In LC1.Controls
                '        If ctrl.name.ToString.ToLower = "txtkode" Then
                '            KodeLama = ctrl.text.ToString
                '        ElseIf ctrl.name.ToString.ToLower = "txtnama" Then
                '            NamaLama = ctrl.text.ToString
                '        End If
                '    Next
                'Else
                'EksekusiSQL("INSERT FROM MLogEditMAlamat")
                'KODE DIBAWAH UNTUK MEMBUKA FORM HANYA 1
                Dim frmEntri As frmDaftarAlamat = Nothing
                Dim F As Object
                For Each F In MdiParent.MdiChildren
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
                frmEntri.DirectNoID = NoID
                frmEntri.ShowNoID = True
                frmEntri.Show()
                frmEntri.Focus()

                'DialogResult = Windows.Forms.DialogResult.OK
                Close()
                Dispose()
                'End If

            Else
                XtraMessageBox.Show("Info Kesalahan : " & vbCrLf & PesanSalah, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            End If
        End If
    End Sub
    Private Function IsValidasi()
        Dim ckPKP As CheckEdit = Nothing
        Dim ckSupplier As CheckEdit = Nothing
        Dim txtKode As TextEdit = Nothing
        Try
            For Each ctrl In LC1.Controls
                If ctrl.name.tolower = "txtkode".ToLower Then
                    txtKode = CType(ctrl, TextEdit)
                    If txtKode.Text.Trim = "" Then
                        XtraMessageBox.Show("Kode masih kosong.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Warning)
                        txtKode.Focus()
                        Return False
                    End If
                    If CekKodeValid(txtKode.Text, KodeLama, TableName, "Kode", Not isNew) Then
                        XtraMessageBox.Show("Kode Sudah dipakai.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Warning)
                        txtKode.Focus()
                        Return False
                    End If
                ElseIf ctrl.name.tolower = "txtispkp".ToLower Then
                    ckPKP = CType(ctrl, CheckEdit)
                ElseIf ctrl.name.tolower = "txtissupplier".ToLower Then
                    ckSupplier = CType(ctrl, CheckEdit)
                ElseIf ctrl.name.tolower = "txtnama".ToLower Then
                    Dim txtNama As TextEdit
                    txtNama = CType(ctrl, TextEdit)
                    If txtNama.Text.Trim = "" Then
                        XtraMessageBox.Show("Nama masih kosong.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Warning)
                        txtNama.Focus()
                        Return False
                    End If
                    If CekKodeValid(txtNama.Text, NamaLama, TableName, "Nama", Not isNew) Then
                        If XtraMessageBox.Show("Nama Sudah dipakai, yakin ingin melanjutkan penyimpanan?", NamaAplikasi, MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) = Windows.Forms.DialogResult.No Then
                            txtNama.Focus()
                            Return False
                        End If
                    End If
                ElseIf ctrl.name.tolower = "txtLimithutang".ToLower Then
                    Dim txtLimitHutang As CalcEdit
                    txtLimitHutang = CType(ctrl, CalcEdit)
                    If NullToDbl(txtLimitHutang.EditValue) < 0.0 Then
                        XtraMessageBox.Show("Limit Hutang harus lebih dari atau sama dengan nol.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Warning)
                        txtLimitHutang.Focus()
                        Return False
                    End If
                ElseIf ctrl.name.tolower = "txtJatuhtemposupplier".ToLower Or ctrl.name.tolower = "txtJatuhtempoCustomer".ToLower Then
                    Dim txtJatuhtempo As CalcEdit
                    txtJatuhtempo = CType(ctrl, CalcEdit)
                    If NullToDbl(txtJatuhtempo.EditValue) < 0 Then
                        XtraMessageBox.Show("Jatuh tempo harus lebih dari atau sama dengan nol.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Warning)
                        txtJatuhtempo.Focus()
                        Return False
                    End If
                ElseIf ctrl.name.tolower = "txtDisc".ToLower Then
                    Dim txtDisc As CalcEdit
                    txtDisc = CType(ctrl, CalcEdit)
                    If NullToDbl(txtDisc.EditValue) < 0 Then
                        XtraMessageBox.Show("Disc harus lebih dari atau sama dengan nol.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Warning)
                        txtDisc.Focus()
                        Return False
                    End If
                End If
            Next
            'If Not ckPKP Is Nothing AndAlso Not txtKode Is Nothing AndAlso Not ckSupplier Is Nothing Then
            '    Dim x As New frmOtorisasiAdmin
            '    Dim str() As String
            '    str = txtKode.Text.Split("/")
            '    If NullToBool(ckPKP.Checked) AndAlso str(2).ToUpper <> "P" Then 'PKP
            '        If XtraMessageBox.Show("Kode Supplier belum sesuai dengan tipe Perusahaan PKP" & vbCrLf & "Ingin tetap merubahnya?", NamaAplikasi, MessageBoxButtons.YesNo, MessageBoxIcon.Information, MessageBoxDefaultButton.Button2) = Windows.Forms.DialogResult.Yes Then
            '            If x.ShowDialog(Me) <> Windows.Forms.DialogResult.OK Then
            '                ckPKP.Focus()
            '                Return False
            '            End If
            '        Else
            '            ckPKP.Focus()
            '            Return False
            '        End If
            '    ElseIf Not NullToBool(ckPKP.Checked) AndAlso str(2).ToUpper <> "N" Then 'Non PKP
            '        If XtraMessageBox.Show("Kode Supplier belum sesuai dengan tipe Perusahaan Non PKP" & vbCrLf & "Ingin tetap merubahnya?", NamaAplikasi, MessageBoxButtons.YesNo, MessageBoxIcon.Information, MessageBoxDefaultButton.Button2) = Windows.Forms.DialogResult.Yes Then
            '            If x.ShowDialog(Me) <> Windows.Forms.DialogResult.OK Then
            '                ckPKP.Focus()
            '                Return False
            '            End If
            '        Else
            '            ckPKP.Focus()
            '            Return False
            '        End If
            '    End If
            '    x.Dispose()
            'End If
            Return True
        Catch ex As Exception
            XtraMessageBox.Show("Info Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK)
            Return False
        End Try
    End Function
    Private Sub SimpleButton3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SimpleButton3.Click
        Batal()
    End Sub

    Private Sub SimpleButton5_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBaru.Click
        Select Case XtraTabControl1.SelectedTabPageIndex
            Case 0
                Baru()
            Case 1 'NPWP
                NPWPBaru()
        End Select

    End Sub
    Sub Baru()
        Dim FormEntriName As String = "EntriAlamatDCustomer"
        'Select Case FormEntriName
        '    Case "EntriBarang"
        '        Dim Brg As New clsBarang
        '        Brg.FormName = FormEntriName
        '        Brg.isNew = True
        '        If Brg.ShowDialog(me) = Windows.Forms.DialogResult.OK Then
        '            tambahde()
        '            GV1.FocusedRowHandle = GV1.LocateByValue("NoID", Brg.NoID)
        '        End If
        '        Brg.Dispose()
        '    Case Else
        Dim x As New frmSimpleEntri
        x.FormName = FormEntriName
        x.isNew = True
        x.IDParent = NoID
        If x.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
            EksekusiSQL("Update MAlamatD set IDAlamat=" & NoID & " where MAlamatD.NoID=" & x.NoID)
            RefreshDetil()
            GV1.FocusedRowHandle = GV1.LocateByDisplayText(0, GV1.Columns("NoID"), NoID.ToString("#,##0"))

        End If
        x.Dispose()
        'End Select
    End Sub
    Sub NPWPBaru()
        Dim x As New frmSimpleEntri
        x.FormName = "EntriAlamatDNPWP"
        x.isNew = True
        x.IDParent = NoID
        If x.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
            EksekusiSQL("Update MAlamatDNPWP set IDAlamat=" & NoID & " where MAlamatDNPWP.NoID=" & x.NoID)
            RefreshDetilNPWP()
            GridView1.FocusedRowHandle = GridView1.LocateByDisplayText(0, GridView1.Columns("NoID"), NoID.ToString("#,##0"))

        End If
        x.Dispose()
        'End Select
    End Sub
    Private Sub mnTambah_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles mnTambah.ItemClick
        btnBaru.PerformClick()
    End Sub

    Private Sub SimpleButton4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEdit.Click
        Select Case XtraTabControl1.SelectedTabPageIndex
            Case 0
                Edit()
            Case 1
                EditNPWP()
        End Select
    End Sub

    Private Sub mnEditSatuan_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles mnEditSatuan.ItemClick
        btnEdit.PerformClick()
    End Sub

    Private Sub mnHapusSatuan_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles mnHapusSatuan.ItemClick
        btnHapus.PerformClick()
    End Sub

    Private Sub SimpleButton2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnHapus.Click
        Select Case XtraTabControl1.SelectedTabPageIndex
            Case 0
                Hapus()
            Case 1
                HapusNPWP()
        End Select
    End Sub
    Sub Hapus()
        Dim view As ColumnView = GC1.FocusedView
        Try
            Dim row As System.Data.DataRow = view.GetDataRow(GV1.FocusedRowHandle)
            Dim dc As Integer = GV1.FocusedRowHandle
            Dim NoID As Long = NullToLong(row("NoID"))
            EksekusiSQL("Update MAlamatD Set IsActive=0 where NoID=" & NoID.ToString)
            RefreshDetil()
        Catch ex As Exception
            XtraMessageBox.Show("Untuk menghapus pilih item yang akan dihapus terlebih dahulu lalu tekan tombol hapus", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally

        End Try
    End Sub
    Sub HapusNPWP()
        Dim view As ColumnView = GridControl1.FocusedView
        Try
            Dim row As System.Data.DataRow = view.GetDataRow(GridView1.FocusedRowHandle)
            Dim dc As Integer = GridView1.FocusedRowHandle
            Dim NoID As Long = NullToLong(row("NoID"))
            If NullToLong(EksekusiSQLSkalar("select count( NoID ) from MBeli where IDAlamatDNPWP=" & NoID.ToString)) > 0 Then
                MsgBox("NPWP ini telah di pakai transaksi, silahkan inactive jika tidak terpakai lagi!", MsgBoxStyle.Information)
            Else
                If MsgBox("Yakin mau Hapus NPWP No: " & NullToStr(row("NoID")), MsgBoxStyle.Question + MsgBoxStyle.YesNo) = MsgBoxResult.Yes Then
                    EksekusiSQL("Delete MAlamatDNPWP where NoID=" & NoID.ToString)
                    RefreshDetilNPWP()
                End If
            End If

        Catch ex As Exception
            XtraMessageBox.Show("Untuk menghapus pilih item yang akan dihapus terlebih dahulu lalu tekan tombol hapus", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally

        End Try
    End Sub
    Sub Edit()
        Dim x As frmSimpleEntri
        'Dim Brg As clsBarang
        Dim FormEntriName As String = "EntriAlamatDCustomer"
        Dim view As ColumnView = GC1.FocusedView
        Try
            Dim row As System.Data.DataRow = view.GetDataRow(GV1.FocusedRowHandle)
            Dim dc As Integer = GV1.FocusedRowHandle
            Dim NoID As Long = NullToLong(row("NoID"))
            'If FormEntriName = "EntriBarang" Then
            '    Brg = New clsBarang
            '    Brg.FormName = FormEntriName
            '    Brg.isNew = False
            '    Brg.NoID = NoID
            '    If Brg.ShowDialog(me) = Windows.Forms.DialogResult.OK Then
            '        isiTambahan()
            '        GV1.ClearSelection()
            '        GV1.FocusedRowHandle = GV1.LocateByDisplayText(0, GV1.Columns("NoID"), NoID.ToString("#,##0"))
            '        GV1.SelectRow(GV1.FocusedRowHandle)
            '    End If
            '    Brg.Dispose()
            'Else
            x = New frmSimpleEntri
            x.FormName = FormEntriName
            x.isNew = False
            x.NoID = NoID
            x.IDParent = NoID
            If x.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
                'EksekusiSQL("Update MAlamatD set IDAlamat=" & NoID & " where MAlamatD.NoID=" & x.NoID)
                RefreshDetil()
                GV1.ClearSelection()
                GV1.FocusedRowHandle = GV1.LocateByDisplayText(0, GV1.Columns("NoID"), NoID.ToString("#,##0"))
                GV1.SelectRow(GV1.FocusedRowHandle)
            End If
            x.Dispose()
            'End If

        Catch ex As Exception
            XtraMessageBox.Show("Untuk mengedit pilih item satuan yang akan diedit terlebih dahulu lalu tekan tombol edit", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally

        End Try
    End Sub
    Sub EditNPWP()
        Dim x As frmSimpleEntri
        'Dim Brg As clsBarang
        Dim FormEntriName As String = "EntriAlamatDNPWP"
        Dim view As ColumnView = GridControl1.FocusedView
        Try
            Dim row As System.Data.DataRow = view.GetDataRow(GridView1.FocusedRowHandle)
            Dim dc As Integer = GridView1.FocusedRowHandle
            Dim NoID As Long = NullToLong(row("NoID"))

            x = New frmSimpleEntri
            x.FormName = FormEntriName
            x.isNew = False
            x.NoID = NoID
            x.IDParent = NoID
            If x.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
                RefreshDetilNPWP()
                GridView1.ClearSelection()
                GridView1.FocusedRowHandle = GridView1.LocateByDisplayText(0, GridView1.Columns("NoID"), NoID.ToString("#,##0"))
                GridView1.SelectRow(GridView1.FocusedRowHandle)
            End If
            x.Dispose()
            'End If

        Catch ex As Exception
            XtraMessageBox.Show("Untuk mengedit pilih item satuan yang akan diedit terlebih dahulu lalu tekan tombol edit", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally

        End Try
    End Sub
    Sub SetTombol()
        If isNew Then
            mnTambah.Enabled = False
            mnEditSatuan.Enabled = False
            mnHapusSatuan.Enabled = False
            btnBaru.Enabled = False
            btnEdit.Enabled = False
            btnHapus.Enabled = False
        Else
            mnTambah.Enabled = True
            mnEditSatuan.Enabled = True
            mnHapusSatuan.Enabled = True
            btnBaru.Enabled = True
            btnEdit.Enabled = True
            btnHapus.Enabled = True
        End If
    End Sub

    Private Sub SimpleButton2_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Switch()
    End Sub
    Sub Switch()
        XtraTabControl1.SelectedTabPageIndex = (XtraTabControl1.SelectedTabPageIndex + 1) Mod XtraTabControl1.TabPages.Count
        If XtraTabControl1.SelectedTabPageIndex = 1 Then
            RefreshDetil()
        End If
    End Sub
    Sub RefreshDetil()
        Dim cn As New SqlConnection(StrKonSql)
        Dim ocmd2 As New SqlCommand
        ocmd2.Connection = cn
        'Else
        'ocmd2.CommandText = "select * from " & TableName & " where noid=" & NoID.ToString
        'End If
        cn.Open()
        oda2 = New SqlDataAdapter(ocmd2)

        ocmd2.CommandText = "select MAlamatD.*,MJenisHarga.nama as GolonganHarga,MJenisBarang.Nama as JenisBarang from MAlamatD " & _
        "Left Join MJenisHarga On MAlamatD.IDJenisHarga=MJenisHarga.NoID " & _
        "Left Join MJenisBarang On MAlamatD.IDJenisBarang=MJenisBarang.NoID " & _
        "where IDAlamat=" & NoID

        oda2 = New SqlDataAdapter(ocmd2)
        If ds.Tables("MAlamatD") Is Nothing Then
        Else
            ds.Tables("MAlamatD").Clear()
        End If
        oda2.Fill(ds, "MAlamatD")
        GC1.DataSource = ds.Tables("MAlamatD")

        ocmd2.Dispose()
        cn.Close()
        cn.Dispose()
    End Sub
    Sub RefreshDetilNPWP()
        Dim cn As New SqlConnection(StrKonSql)
        Dim ocmd2 As New SqlCommand
        ocmd2.Connection = cn
        'Else
        'ocmd2.CommandText = "select * from " & TableName & " where noid=" & NoID.ToString
        'End If
        cn.Open()
        oda2 = New SqlDataAdapter(ocmd2)

        ocmd2.CommandText = "select MAlamatDNPWP.* from MAlamatDNPWP " & _
        "where IDAlamat=" & NoID

        oda2 = New SqlDataAdapter(ocmd2)
        If ds.Tables("MAlamatDNPWP") Is Nothing Then
        Else
            ds.Tables("MAlamatDNPWP").Clear()
        End If
        oda2.Fill(ds, "MAlamatDNPWP")
        GridControl1.DataSource = ds.Tables("MAlamatDNPWP")

        ocmd2.Dispose()
        cn.Close()
        cn.Dispose()
    End Sub
    Private Sub BarButtonItem3_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem3.ItemClick
        btnRefresh.PerformClick()
    End Sub

    Private Sub BarButtonItem4_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs)

    End Sub

    Private Sub BarButtonItem5_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem5.ItemClick
        Dim xOtorisasi As New frmOtorisasiAdmin
        If XtraMessageBox.Show("Simpan Layout?", NamaAplikasi, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = Windows.Forms.DialogResult.Yes AndAlso xOtorisasi.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
            Ini.TulisIniPath(FolderLayouts & FormName & ".ini", "Form", "Width", Me.Width)
            Ini.TulisIniPath(FolderLayouts & FormName & ".ini", "Form", "Height", Me.Height)

            If IsSupplier Then
                LC1.SaveLayoutToXml(FolderLayouts & FormName & "IsSupplier.xml")
            ElseIf IsMember Then
                LC1.SaveLayoutToXml(FolderLayouts & FormName & "IsMember.xml")
            ElseIf IsPegawai Then
                LC1.SaveLayoutToXml(FolderLayouts & FormName & "IsPegawai.xml")
            Else
                LC1.SaveLayoutToXml(FolderLayouts & FormName & ".xml")
            End If

            GV1.SaveLayoutToXml(FolderLayouts & FormName & GV1.Name & ".xml")
            GridView1.SaveLayoutToXml(FolderLayouts & FormName & GridView1.Name & ".xml")
            Dim lu As New DevExpress.XtraEditors.SearchLookUpEdit
            Try
                For Each ctrl In LC1.Controls
                    If TypeOf ctrl Is DevExpress.XtraEditors.SearchLookUpEdit Then
                        lu = CType(ctrl, DevExpress.XtraEditors.SearchLookUpEdit)
                        lu.Properties.View.SaveLayoutToXml(FolderLayouts & FormName & lu.Name & ".xml")
                    End If
                Next

            Catch ex As Exception
                DevExpress.XtraEditors.XtraMessageBox.Show("Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
            Finally
                'lu.Dispose()
            End Try
        End If
        xOtorisasi.Dispose()
    End Sub

    Private Sub SimpleButton2_Click_2(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRefresh.Click
        RefreshDetil()
    End Sub

    Private Sub frmEntriKontak_Shown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Shown
        If isNew Then
            BS.AddNew()
            IsiDefault()
        Else

        End If
    End Sub
End Class