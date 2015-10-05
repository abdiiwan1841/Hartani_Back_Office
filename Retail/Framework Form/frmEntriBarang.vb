Imports System.Data.SqlClient
Imports System.Data.SQLite

Imports DevExpress.XtraLayout
'Imports System.Linq
Imports DevExpress.XtraGrid.Views.Base
Imports System.Drawing
Imports DevExpress.XtraEditors.Repository
Imports DevExpress.Utils
Imports DevExpress.XtraEditors
Imports DevExpress.XtraGrid

Public Class clsBarang
    Public FormName As String = ""
    Public TableName As String = ""

    Public isNew As Boolean = True
    Public NoID As Long = -1
    Dim oda2 As SqlDataAdapter

    Dim ds As New DataSet
    Dim dsGrid As New DataSet
    Dim BS As New BindingSource
    Dim WithEvents cb As New DevExpress.XtraEditors.Controls.EditorButton

    Dim WithEvents txtEdit As DevExpress.XtraEditors.TextEdit
    Dim WithEvents clcEdit As DevExpress.XtraEditors.CalcEdit
    Dim WithEvents ckedit As DevExpress.XtraEditors.CheckEdit
    Dim dtEdit As DevExpress.XtraEditors.DateEdit
    Dim picEdit As DevExpress.XtraEditors.PictureEdit
    Dim WithEvents lkEdit As DevExpress.XtraEditors.SearchLookUpEdit

    Dim repckedit As New RepositoryItemCheckEdit
    Dim repdateedit As New RepositoryItemDateEdit
    Dim reptextedit As New RepositoryItemTextEdit

    Dim KodeLama As String = ""
    Dim KodeAlias As String = ""
    Dim NamaLama As String = ""

    Dim KodeSekarang As String = ""
    Dim KodeAliasSekarang As String = ""

    Dim txtIDSatuanBase As New SearchLookUpEdit
    Dim TempInsertBaru As Boolean = False

    Public Enum TipeEntry
        All = 0
        BerdasarkanPembelian = 1
        SettingHarga = 2
    End Enum
    Public TipeEntri As TipeEntry
    Public FocusRowColumn As Long = -1
    'Sub TampilDetil()
    '    Dim cn As New SqlConnection(StrKonSql)
    '    Dim ocmd2 As New SqlCommand
    '    ocmd2.Connection = cn
    '    ocmd2.CommandType = CommandType.Text
    '    ' If isNew Then
    '    ocmd2.CommandText = "select * from " & "msatuan"
    '    'Else
    '    'ocmd2.CommandText = "select * from " & TableName & " where noid=" & NoID.ToString
    '    'End If
    '    cn.Open()
    '    oda2 = New SqlDataAdapter(ocmd2)
    '    If ds.Tables("Satuan") Is Nothing Then
    '    Else
    '        ds.Tables("Satuan").Clear()
    '    End If
    '    oda2.Fill(ds, "Satuan")
    '    'RepositoryItemGridLookUpEdit1.DataSource = ds.Tables("Satuan")

    '    ocmd2.CommandText = "select * from MBarangD where IDBarang=" & NoID

    '    oda2 = New SqlDataAdapter(ocmd2)
    '    If ds.Tables("MBarangD") Is Nothing Then
    '    Else
    '        ds.Tables("MBarangD").Clear()
    '    End If
    '    oda2.Fill(ds, "MBarangD")
    '    GC1.DataSource = ds.Tables("MBarangD")

    '    ocmd2.Dispose()
    '    cn.Close()
    '    cn.Dispose()
    'End Sub
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
                            txtEdit.Properties.Mask.EditMask = NullToStr(ds.Tables("Master").Rows(i).Item("format"))
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
                    txtEdit.Visible = NullToBool(ds.Tables("Master").Rows(i).Item("visible"))
                    txtEdit.Properties.ReadOnly = NullToBool(ds.Tables("Master").Rows(i).Item("readonly"))
                    If NullToStr(ds.Tables("Master").Rows(i).Item("default")) <> "" Then
                        txtEdit.Tag = "df:" & NullToStr(ds.Tables("Master").Rows(i).Item("default"))
                    End If
                    If NullToStr(ds.Tables("Master").Rows(i).Item("function")) <> "" Then
                        txtEdit.Tag = IIf(txtEdit.Tag <> "", txtEdit.Tag & ";", "") & NullToStr(ds.Tables("Master").Rows(i).Item("function"))
                    End If
                    If txtEdit.Name.ToLower = "txtkode".ToLower Or txtEdit.Name.ToLower = "txtalias".ToLower Then
                        If NullToStr(ds.Tables("Master").Rows(i).Item("fieldname")) <> "" Then
                            txtEdit.DataBindings.Add("editvalue", BS _
                         , UCase(ds.Tables("Master").Rows(i).Item("fieldname")))
                        End If
                    Else
                        If NullToStr(ds.Tables("Master").Rows(i).Item("fieldname")) <> "" Then
                            txtEdit.DataBindings.Add("editvalue", BS _
                         , ds.Tables("Master").Rows(i).Item("fieldname"))
                        End If
                    End If
                    itemLC = New LayoutControlItem
                    itemLC.Name = ds.Tables("Master").Rows(i).Item("caption")
                    itemLC.Parent = LC1.Root

                    itemLC.Control = txtEdit
                    AddHandler txtEdit.GotFocus, AddressOf txtEdit_GotFocus
                    AddHandler txtEdit.LostFocus, AddressOf txtEdit_LostFocus
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
                    If Not isNew Then
                        If txtEdit.Name.ToLower = "txtkode" AndAlso txtEdit.Text.Length >= 1 Then
                            txtEdit.Properties.ReadOnly = True
                        ElseIf txtEdit.Name.ToLower = "txtalias" AndAlso txtEdit.Text.Length >= 1 Then
                            txtEdit.Properties.ReadOnly = True
                        End If
                    End If
                    If txtEdit.Name.ToLower = "txtkode".ToLower Then
                        ''txtEdit.Properties.Mask.CreateDefaultMaskManager()
                        'txtEdit.Properties.Mask.MaskType = Mask.MaskType.Regular
                        ''txtEdit.Properties.Mask.ShowPlaceHolders = False
                        'txtEdit.Properties.Mask.EditMask = "\w*"
                        ''txtEdit.Properties.Mask.UseMaskAsDisplayFormat = True
                        txtEdit.Properties.CharacterCasing = CharacterCasing.Upper
                        KodeLama = txtEdit.Text
                        If Not isNew Then
                            AddHandler txtEdit.KeyDown, AddressOf txtKode_KeyDown
                        End If
                    ElseIf txtEdit.Name.ToLower = "txtnama".ToLower Then
                        NamaLama = txtEdit.Text
                        txtEdit.Properties.CharacterCasing = CharacterCasing.Normal
                    ElseIf txtEdit.Name.ToLower = "txtalias".ToLower Then
                        KodeAlias = txtEdit.Text
                        txtEdit.Properties.CharacterCasing = CharacterCasing.Upper
                        If Not isNew Then
                            AddHandler txtEdit.KeyDown, AddressOf txtKode_KeyDown
                        End If
                        'ElseIf (txtEdit.Name.ToLower = "txtStockMax".ToLower Or txtEdit.Name.ToLower = "txtStockMin".ToLower) Then
                        '    If DefTipeStock = DefTipeStock_.Penuh Then
                        '        txtEdit.Properties.ReadOnly = False
                        '    ElseIf DefTipeStock = DefTipeStock_.Penuh AndAlso Not IsSupervisor Then
                        '        txtEdit.Properties.ReadOnly = True
                        '    Else
                        '        txtEdit.Properties.ReadOnly = True
                        '    End If
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
                    'If clcEdit.Name.ToLower = "txtCtnPcs".ToLower Or clcEdit.Name.ToLower = "txtCtn_duz".ToLower Then
                    '    AddHandler clcEdit.LostFocus, AddressOf clcCtnPcs_LostFocus
                    'ElseIf clcEdit.Name.ToLower = "txtHargaPasar".ToLower Or clcEdit.Name.ToLower = "txtProsenUp".ToLower Then
                    '    clcEdit.Properties.EditFormat.FormatType = FormatType.Numeric
                    '    clcEdit.Properties.EditFormat.FormatString = "n0"
                    '    clcEdit.Properties.Mask.MaskType = Mask.MaskType.Numeric
                    '    clcEdit.Properties.Mask.EditMask = "n0"
                    '    clcEdit.Properties.Mask.UseMaskAsDisplayFormat = True
                    '    AddHandler clcEdit.EditValueChanged, AddressOf clcEdit_LostFocus

                    'End If
                    AddHandler clcEdit.GotFocus, AddressOf clcEdit_GotFocus
                    AddHandler clcEdit.LostFocus, AddressOf clcEdit_LostFocus
                    If (clcEdit.Name.ToLower = "txtStockMax".ToLower Or clcEdit.Name.ToLower = "txtStockMin".ToLower) Then
                        If isNew Then
                            clcEdit.Properties.ReadOnly = False
                        ElseIf DefTipeStock = DefTipeStock_.Penuh AndAlso IsSupervisor Then
                            clcEdit.Properties.ReadOnly = False
                        ElseIf DefTipeStock = DefTipeStock_.Penuh AndAlso Not IsSupervisor Then
                            clcEdit.Properties.ReadOnly = True
                        Else
                            clcEdit.Properties.ReadOnly = True
                        End If
                    End If
                Case "checkedit"
                    ckedit = New DevExpress.XtraEditors.CheckEdit
                    ckedit.Name = "txt" + ds.Tables("Master").Rows(i).Item("nama")
                    ckedit.Text = ds.Tables("Master").Rows(i).Item("caption")
                    ckedit.Visible = NullToBool(ds.Tables("Master").Rows(i).Item("visible"))
                    ckedit.Properties.ReadOnly = NullToBool(ds.Tables("Master").Rows(i).Item("readonly"))
                    ckedit.DataBindings.Add("editvalue", BS _
                    , ds.Tables("Master").Rows(i).Item("fieldname"))
                    'If ckedit.Name.ToLower = "txt".ToLower AndAlso DefTipeStock = DefTipeStock_.Penuh Then

                    'Else

                    'End If
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
                    'If NullToStr(ds.Tables("Master").Rows(i).Item("fieldname")).ToUpper <> "IsLockQtyMax".ToUpper Then
                    '    AddHandler ckedit.CheckedChanged, AddressOf ckedit_CheckedChanged
                    'End If
                    itemLC = New LayoutControlItem
                    itemLC.Name = ds.Tables("Master").Rows(i).Item("caption")

                    itemLC.Parent = LC1.Root
                    itemLC.Control = ckedit
                    AddHandler ckedit.CheckedChanged, AddressOf ckedit_CheckedChanged

                Case "pictureedit"
                    Dim FileImageSetting As String
                    picEdit = New DevExpress.XtraEditors.PictureEdit
                    picEdit.Name = "txt" + ds.Tables("Master").Rows(i).Item("nama")
                    picEdit.Text = ds.Tables("Master").Rows(i).Item("caption")
                    picEdit.Properties.SizeMode = DevExpress.XtraEditors.Controls.PictureSizeMode.Zoom
                    picEdit.Visible = NullToBool(ds.Tables("Master").Rows(i).Item("visible"))
                    picEdit.Properties.ReadOnly = NullToBool(ds.Tables("Master").Rows(i).Item("readonly"))

                    FileImageSetting = NullToStr(EksekusiSQlSkalarNew("SELECT PathImage FROM MSetting")) & "\" & NullToStr(EksekusiSQlSkalarNew("SELECT CASE WHEN (Kode IS NULL OR Kode='') THEN CONVERT(VARCHAR(50),NoID) ELSE KODE END FROM MBarang WHERE NoID=" & NoID))
                    'If System.IO.File.Exists(FileImageSetting & ".JPG") Then
                    '    If System.IO.File.Exists(FileImageSetting & "temp.JPG") Then
                    '        System.IO.File.Delete(FileImageSetting & "temp.JPG")
                    '    End If
                    '    System.IO.File.Copy(FileImageSetting & ".JPG", FileImageSetting & "temp.JPG")
                    '    picEdit.Image = Image.FromFile(FileImageSetting & "temp.JPG")
                    'Else
                    picEdit.DataBindings.Add("EditValue", BS _
                      , ds.Tables("Master").Rows(i).Item("fieldname"))
                    'End If
                    itemLC = New LayoutControlItem
                    itemLC.Name = ds.Tables("Master").Rows(i).Item("caption")

                    itemLC.Parent = LC1.Root
                    itemLC.Control = picEdit

                Case "dateedit"
                    dtEdit = New DevExpress.XtraEditors.DateEdit
                    dtEdit.Name = "txt" + ds.Tables("Master").Rows(i).Item("nama")
                    dtEdit.EnterMoveNextControl = True
                    dtEdit.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.DateTime
                    dtEdit.Properties.Mask.EditMask = NullToStr(ds.Tables("Master").Rows(i).Item("format"))

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
                    lkEdit.Properties.Buttons.Add(New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Ellipsis))
                    lkEdit.Properties.Buttons.Add(cb)
                    lkEdit.Properties.NullText = ""
                    lkEdit.Name = "txt" + ds.Tables("Master").Rows(i).Item("nama")
                    lkEdit.Properties.DisplayMember = NullToStr(ds.Tables("Master").Rows(i).Item("lookupdisplay"))
                    lkEdit.Properties.ValueMember = NullToStr(ds.Tables("Master").Rows(i).Item("lookupvalue"))
                    lkEdit.EnterMoveNextControl = True
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
                        ocmd2.CommandText = NullToStr(ds.Tables("Master").Rows(i).Item("sql"))
                    Else
                        ocmd2.CommandText = "Select * from " & ds.Tables("Master").Rows(i).Item("tablelookup")
                    End If
                    lkEdit.Tag = IIf(NullToStr(lkEdit.Tag).Length > 0, lkEdit.Tag & ";", "").ToString & "sqllookup:" & ocmd2.CommandText

                    oda2 = New SqlDataAdapter(ocmd2)
                    oda2.Fill(dsLookUp, NullToStr(ds.Tables("Master").Rows(i).Item("tablelookup")))
                    lkEdit.Properties.DataSource = dsLookUp.Tables(ds.Tables("Master").Rows(i).Item("tablelookup"))
                    AddHandler lkEdit.EditValueChanged, AddressOf lkEdit_EditValueChanged
                    AddHandler lkEdit.ButtonClick, AddressOf lkEdit_ButtonClick
                    If lkEdit.Name.ToLower = "txtidsatuan".ToLower Then
                        AddHandler lkEdit.EditValueChanged, AddressOf txtEdit_EditValueChanged
                    End If
                    dsLookUp.Dispose()
                    If System.IO.File.Exists(FolderLayouts & FormName & lkEdit.Name & ".xml") Then
                        lkEdit.Properties.View.RestoreLayoutFromXml(FolderLayouts & FormName & lkEdit.Name & ".xml")
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
            If ctrl.name.ToString.ToLower = "txtishargajualmanual" Then
                ckedit_CheckedChanged(ctrl, Nothing)
            End If
        Next

        'Catch ex As Exception
        '    XtraMessageBox.Show("Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        'End Try
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
    Private Sub clcCtnPcs_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        'Dim txt = TryCast(sender, CalcEdit)
        'Dim SQL As String = ""
        'Dim txtCtnPcs As New CalcEdit
        'Try
        '    If txt.Name.ToLower = "txtCtnPcs".ToLower Then
        '        SQL = "UPDATE MBARANGD SET MBarangD.KONVERSI = " & FixKoma(NullToDbl(txt.EditValue)) & _
        '              " FROM MBarangD LEFT JOIN MSatuan ON MBarangD.IDSatuan=MSatuan.NoID " & _
        '              " WHERE MBarangD.IDBarang=" & NoID & " AND UPPER(MSatuan.Nama) LIKE UPPER('Karton%')"
        '        EksekusiSQL(SQL)

        '        For Each ctrl In LC1.Controls
        '            If ctrl.name.ToString.ToLower = "txtctn_duz".ToLower Then
        '                txtCtnPcs = CType(ctrl, CalcEdit)
        '                Exit For
        '            End If
        '        Next
        '        If NullToDbl(txtCtnPcs.EditValue) = 0 Then
        '            'SQL = "UPDATE MBARANGD SET MBarangD.KONVERSI = " & FixKoma(NullToDbl(txt.EditValue) / 1) & _
        '            '  " FROM MBarangD LEFT JOIN MSatuan ON MBarangD.IDSatuan=MSatuan.NoID " & _
        '            '  " WHERE MBarangD.IDBarang=" & NoID & " AND UPPER(MSatuan.Nama) LIKE UPPER('Inner%')"
        '            SQL = "UPDATE MBARANGD SET MBarangD.KONVERSI = " & FixKoma(0) & _
        '              " FROM MBarangD LEFT JOIN MSatuan ON MBarangD.IDSatuan=MSatuan.NoID " & _
        '              " WHERE MBarangD.IDBarang=" & NoID & " AND UPPER(MSatuan.Nama) LIKE UPPER('Inner%')"
        '        Else
        '            SQL = "UPDATE MBARANGD SET MBarangD.KONVERSI = " & FixKoma(NullToDbl(txt.EditValue) / NullToDbl(txtCtnPcs.EditValue)) & _
        '              " FROM MBarangD LEFT JOIN MSatuan ON MBarangD.IDSatuan=MSatuan.NoID " & _
        '              " WHERE MBarangD.IDBarang=" & NoID & " AND UPPER(MSatuan.Nama) LIKE UPPER('Inner%')"
        '        End If
        '        EksekusiSQL(SQL)

        '        RefreshStock()
        '    ElseIf txt.Name.ToLower = "txtctn_duz".ToLower Then
        '        For Each ctrl In LC1.Controls
        '            If ctrl.name.ToString.ToLower = "txtCtnPcs".ToLower Then
        '                txtCtnPcs = CType(ctrl, CalcEdit)
        '                Exit For
        '            End If
        '        Next
        '        If NullToDbl(txt.EditValue) = 0 Then
        '            'SQL = "UPDATE MBARANGD SET MBarangD.KONVERSI = " & FixKoma(NullToDbl(txtCtnPcs.EditValue) / 1) & _
        '            '      " FROM MBarangD LEFT JOIN MSatuan ON MBarangD.IDSatuan=MSatuan.NoID " & _
        '            '      " WHERE MBarangD.IDBarang=" & NoID & " AND UPPER(MSatuan.Nama) LIKE UPPER('Inner%')"
        '            SQL = "UPDATE MBARANGD SET MBarangD.KONVERSI = " & FixKoma(0) & _
        '                  " FROM MBarangD LEFT JOIN MSatuan ON MBarangD.IDSatuan=MSatuan.NoID " & _
        '                  " WHERE MBarangD.IDBarang=" & NoID & " AND UPPER(MSatuan.Nama) LIKE UPPER('Inner%')"
        '        Else
        '            SQL = "UPDATE MBARANGD SET MBarangD.KONVERSI = " & FixKoma(NullToDbl(txtCtnPcs.EditValue) / NullToDbl(txt.EditValue)) & _
        '                  " FROM MBarangD LEFT JOIN MSatuan ON MBarangD.IDSatuan=MSatuan.NoID " & _
        '                  " WHERE MBarangD.IDBarang=" & NoID & " AND UPPER(MSatuan.Nama) LIKE UPPER('Inner%')"
        '        End If
        '        EksekusiSQL(SQL)
        '        RefreshStock()
        '    End If
        'Catch ex As Exception

        'End Try
    End Sub

    Private Sub clsBarang_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        If DialogResult = Windows.Forms.DialogResult.Cancel AndAlso SimpleButton1.Enabled Then
            If XtraMessageBox.Show("Yakin ingin keluar?" & vbCrLf & "Perubahan tidak akan tersimpan!", NamaAplikasi, MessageBoxButtons.OKCancel, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) = Windows.Forms.DialogResult.Cancel Then
                e.Cancel = True
            Else 'Close dengan tidak menyimpan
                If isNew Then

                Else
                    BS.CancelEdit()

                End If
            End If
        End If
    End Sub

    Private Sub frmSimpleEntri_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim dlg As DevExpress.Utils.WaitDialogForm = Nothing
        Try
            Dim curentcursor As Cursor = Windows.Forms.Cursor.Current
            Windows.Forms.Cursor.Current = Cursors.WaitCursor
            DialogResult = Windows.Forms.DialogResult.Cancel 'Default
            dlg = New DevExpress.Utils.WaitDialogForm(String.Format("Creating component on " & Me.FormName.ToString & ".{0}MOHON TUNGGU ...", vbCrLf), Application.ProductName)
            dlg.TopMost = False
            dlg.Show()
            Windows.Forms.Cursor.Current = Cursors.WaitCursor
            TableName = GetTableNamebyFormname(FormName, "")
            GenerateForm()
            'LC1.BeginUpdate()
            'If System.IO.File.Exists(FolderLayouts & FormName & TipeEntri.ToString & IDUserAktif & ".xml") Then
            '    LC1.RestoreLayoutFromXml(FolderLayouts & FormName & TipeEntri.ToString & IDUserAktif & ".xml")
            'Else
            If System.IO.File.Exists(FolderLayouts & FormName & TipeEntri.ToString & DefTipeStock.ToString & ".xml") Then
                LC1.RestoreLayoutFromXml(FolderLayouts & FormName & TipeEntri.ToString & DefTipeStock.ToString & ".xml")
            Else
                If System.IO.File.Exists(FolderLayouts & FormName & ".xml") Then
                    LC1.RestoreLayoutFromXml(FolderLayouts & FormName & ".xml")
                End If
            End If
            'End If
            TglDari.DateTime = TanggalSystem
            TglSampai.DateTime = TanggalSystem
            'InsertKanStockKeTemp()
            RefreshStock()
            'LC1.EndUpdate()
            SetTombol()
            Me.Width = Ini.BacaIniPath(FolderLayouts & FormName & TipeEntri.ToString & ".ini", "Form", "Width", Me.Width)
            Me.Height = Ini.BacaIniPath(FolderLayouts & FormName & TipeEntri.ToString & ".ini", "Form", "Height", Me.Height)
            Me.Left = (Screen.PrimaryScreen.WorkingArea.Width - Me.Width) \ 2
            Me.Top = (Screen.PrimaryScreen.WorkingArea.Height - Me.Height) \ 2
            'If System.IO.File.Exists(FolderLayouts & FormName & TipeEntri.ToString & IDUserAktif & "Barang Detail.xml") Then
            '    GV1.RestoreLayoutFromXml(FolderLayouts & FormName & TipeEntri.ToString & IDUserAktif & "Barang Detail.xml")
            'Else
            If System.IO.File.Exists(FolderLayouts & FormName & TipeEntri.ToString & DefTipeStock.ToString & "Barang Detail.xml") Then
                GV1.RestoreLayoutFromXml(FolderLayouts & FormName & TipeEntri.ToString & DefTipeStock.ToString & "Barang Detail.xml")
            Else
                If System.IO.File.Exists(FolderLayouts & FormName & "Barang Detail.xml") Then
                    GV1.RestoreLayoutFromXml(FolderLayouts & FormName & "Barang Detail.xml")
                End If
            End If
            'End If
            'If System.IO.File.Exists(FolderLayouts & FormName & TipeEntri.ToString & IDUserAktif & "Stock Barang Detail.xml") Then
            '    GridView1.RestoreLayoutFromXml(FolderLayouts & FormName & TipeEntri.ToString & IDUserAktif & "Stock Barang Detail.xml")
            'Else
            If System.IO.File.Exists(FolderLayouts & FormName & TipeEntri.ToString & DefTipeStock.ToString & "Stock Barang Detail.xml") Then
                GridView1.RestoreLayoutFromXml(FolderLayouts & FormName & TipeEntri.ToString & DefTipeStock.ToString & "Stock Barang Detail.xml")
            Else
                If System.IO.File.Exists(FolderLayouts & FormName & "Stock Barang Detail.xml") Then
                    GridView1.RestoreLayoutFromXml(FolderLayouts & FormName & "Stock Barang Detail.xml")
                End If
            End If
            'End If
            FungsiControl.SetForm(Me)
            For Each ctl As Control In LC1.Controls
                If ctl.Name.ToUpper = "txtNama".ToUpper Then
                    TryCast(ctl, TextEdit).Properties.CharacterCasing = CharacterCasing.Normal
                    Exit For
                End If
            Next
        Catch ex As Exception
            XtraMessageBox.Show(ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            Windows.Forms.Cursor.Current = Cursors.Default
            dlg.Close()
            dlg.Dispose()
        End Try
    End Sub
    'Private Sub InsertKanStockKeTemp()
    '    Dim SQL As String = ""
    '    Try
    '        EksekusiSQL("DELETE FROM TKartuStok WHERE IDBarang=" & NoID)
    '        SQL = "INSERT INTO TKartuStok (IDBarang,IDSatuan,IDGudang,Konversi,QtyMasuk,QtyKeluar,QtyMasukA,QtyKeluarA) "
    '        SQL &= "SELECT (SELECT SUM(A.QtyMasuk-A.QtyKeluar) FROM MKartuStok A WHERE A.IDBarang=MKartuStok.IDBarang AND A.IDGudang=MKartuStok.IDGudang AND A.IDSatuan=MKartuStok.IDSatuan) AS A FROM MKartuStok "
    '    Catch ex As Exception
    '        XtraMessageBox.Show("Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
    '    End Try
    'End Sub
    Public Sub Batal()

        DialogResult = Windows.Forms.DialogResult.Cancel
        Me.Close()

    End Sub

    Public Function Simpan(ByRef PesanSalah As String) As Boolean
        Dim ODC As SqlCommandBuilder
        Dim Sukses As Boolean = False
        Dim cn As New SqlConnection(StrKonSql)
        Dim ocmd2 As New SqlCommand
        Dim SQL As String = ""
        ocmd2.Connection = cn
        ocmd2.CommandType = CommandType.Text
        Dim txtFoto As New PictureEdit
        Dim FileSettingPath As String = ""
        If isNew Then
            'Dim hasil = From control In LC1.Controls Where control.name.ToString.ToLower = "txtnoid"
            'For Each x In hasil
            '    x.editvalue = GetNewID(TableName)
            '    ocmd2.CommandText = "select * from " & TableName & " where noid=" & x.editvalue
            '    Exit For
            'Next
            Dim ada As Boolean = False
            For Each ctrl In LC1.Controls
                If ctrl.name.ToString.ToLower = "txtnoid" Then
                    NoID = GetNewID(TableName)
                    ctrl.editvalue = NoID
                    ocmd2.CommandText = "select * from " & TableName & " where noid=" & NoID
                    ada = True
                    Exit For
                End If
            Next
            If Not ada Then
                XtraMessageBox.Show("Tabel harus mempunyai kolom NoID sebagai Primary Key", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
                Exit Function
            End If
        Else
            ocmd2.CommandText = "select * from " & TableName & " where noid=" & NoID.ToString
        End If
        cn.Open()
        oda2.Dispose()
        oda2 = New SqlDataAdapter(ocmd2)

        Try
            'For Each ctrl In LC1.Controls
            '    If ctrl.name.tolower = "txtNoID".ToLower Then
            '        MsgBox(CType(ctrl, TextEdit).EditValue)
            '        Exit For
            '    End If
            'Next
            ODC = New SqlCommandBuilder(oda2)
            ODC.ConflictOption = ConflictOption.OverwriteChanges
            'If BS.DataSource Is Nothing Then
            '    BS.DataSource = ds.Tables("Data")
            'End If
            Me.Validate()
            BS.EndEdit()
            oda2.Update(ds.Tables("Data"))
            ''UpdateImage
            'For Each ctrl In LC1.Controls
            '    If ctrl.name.tolower = "txtFoto".ToLower Then
            '        txtFoto = CType(ctrl, PictureEdit)
            '        Exit For
            '    End If
            'Next
            'If Not txtFoto.Image Is Nothing Then
            '    EksekusiSQL("UPDATE MBarang SET Foto=NULL WHERE NoID=" & NoID)
            '    FileSettingPath = NullTostr(EksekusiSQlSkalarNew("SELECT PathImage FROM MSetting"))
            '    If FileSettingPath.Trim.Length >= 1 Then
            '        FileSettingPath = FileSettingPath & "\" & _
            '        NullTostr(EksekusiSQlSkalarNew("SELECT CASE WHEN (Kode IS NULL OR Kode='') THEN CONVERT(VARCHAR(50),NoID) ELSE KODE END FROM MBarang WHERE NoID=" & NoID)) & ".JPG"
            '        If System.IO.File.Exists(FileSettingPath) Then
            '            System.IO.File.Delete(FileSettingPath)
            '            'XtraMessageBox.Show("Delete dulu file " & FileSettingPath, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
            '            'Return False
            '            'Exit Function
            '        End If
            '        txtFoto.Image.Save(FileSettingPath)
            '        txtFoto.Reset()
            '        'txtFoto.Image.Dispose() ' = Image.FromFile(FileSettingPath, True)
            '        'txtFoto.LoadImage()
            '        'txtFoto.Image.Dispose()
            '        'txtFoto.Dispose()
            '    End If
            'End If

            'Update Detil
            'HitungHarga()
            'With GV1
            '    For i = 0 To .RowCount - 1
            '        SQL = "Update MBarangD set "
            '        SQL &= " HargaJualA=" & FixKoma(NullToDbl(.GetRowCellValue(i, "HargaJualA")))
            '        SQL &= " , HargaJualB=" & FixKoma(NullToDbl(.GetRowCellValue(i, "HargaJualB")))
            '        SQL &= " , HargaJualC=" & FixKoma(NullToDbl(.GetRowCellValue(i, "HargaJualC")))
            '        SQL &= " , HargaJualD=" & FixKoma(NullToDbl(.GetRowCellValue(i, "HargaJualD")))
            '        SQL &= " , HargaJualE=" & FixKoma(NullToDbl(.GetRowCellValue(i, "HargaJualE")))
            '        SQL &= " , HargaJualF=" & FixKoma(NullToDbl(.GetRowCellValue(i, "HargaJualF")))
            '        SQL &= " , HargaMinA=" & FixKoma(NullToDbl(.GetRowCellValue(i, "HargaMinA")))
            '        SQL &= " , HargaMinB=" & FixKoma(NullToDbl(.GetRowCellValue(i, "HargaMinB")))
            '        SQL &= " , HargaMinC=" & FixKoma(NullToDbl(.GetRowCellValue(i, "HargaMinC")))
            '        SQL &= " , HargaMinD=" & FixKoma(NullToDbl(.GetRowCellValue(i, "HargaMinD")))
            '        SQL &= " , HargaMinE=" & FixKoma(NullToDbl(.GetRowCellValue(i, "HargaMinE")))
            '        SQL &= " , HargaMinF=" & FixKoma(NullToDbl(.GetRowCellValue(i, "HargaMinF")))
            '        SQL &= " WHERE NoID=" & NullToLong(.GetRowCellValue(i, "NoID"))
            '        EksekusiSQL(SQL)
            '    Next
            'End With
            ODC.Dispose()
            oda2.Dispose()
            Sukses = True
            If Sukses Then
                EksekusiSQL("UPDATE MBarang SET TerakhirUpdate=Getdate(), IDUserEdit=" & NullToLong(IDUserAktif) & " WHERE NoID=" & NoID)
            End If
        Catch ex As DBConcurrencyException
            PesanSalah = ex.Message
            For i As Integer = 0 To ex.RowCount - 1
                PesanSalah &= ", " & ex.Row(i).ToString
            Next
        Finally
            'If cn.State = ConnectionState.Open Then
            '    cn.Close()
            'End If
            'ocmd2.Dispose()
            'If Not ODC Is Nothing Then
            '    ODC.Dispose()
            'End If
        End Try
        Return Sukses

    End Function

    'Private Sub RestoreLayout()
    '    LC1.RestoreLayoutFromXml(folderLayouts &  FormName & ".xml")
    'End Sub
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
        Next
    End Sub
    Sub IsiVariabel()
        For Each ctrl In LC1.Controls
            If ctrl.name.ToString.ToLower = "txtidsatuan" Then
                modSqlServer.DefIDSatuanfrmBarang = NullToLong(ctrl.editvalue)
            End If
        Next
    End Sub
    Private Sub txtKode_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs)
        If e.KeyCode = Keys.F7 Then
            Dim x As New frmOtorisasiAdmin
            Try
                'For Each ctrl In LC1.Controls
                '    If ctrl.name.ToString.ToLower = "txtkode" Then
                '        If ctrl.properties.readonly Then
                '            If XtraMessageBox.Show("Yakin ingin merubah kode barang " & ctrl.editvalue.ToString & " ini ?" & vbCrLf & "Merubah kode barang akan merubah seluruhnya di transaksional.", NamaAplikasim, MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) = Windows.Forms.DialogResult.Yes Then
                '                If x.ShowDialog(Me)= Windows.Forms.DialogResult.OK Then
                '                    ctrl.Properties.ReadOnly = False
                '                End If
                '            End If
                '        End If
                '    End If
                'Next
                If sender.properties.readonly Then
                    If XtraMessageBox.Show("Yakin ingin merubah kode / alias barang " & sender.editvalue.ToString & " ini ?" & vbCrLf & "Merubah kode barang akan merubah seluruhnya di transaksional.", NamaAplikasi, MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) = Windows.Forms.DialogResult.Yes Then
                        If x.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
                            sender.Properties.ReadOnly = False
                        End If
                    End If
                End If
            Catch ex As Exception
                XtraMessageBox.Show("Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End If
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
                    lastexpresi = Replace(lastexpresi, "[" + nmobject + "]", NullToDbl(ctrl.editvalue))
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
        'Jika Kategri Otomatis muncul dari left(kode,5)
        'If sender.name.ToString.ToLower = "txtkode" Then
        '    Dim txtkode As DevExpress.XtraEditors.TextEdit
        '    Dim kodeKategori As String = ""
        '    Dim IDKategori = -1
        '    txtkode = sender
        '    'For i = 0 To txtkode.Text.Length - 1
        '    '    If IsNumeric(txtkode.Text.Substring(i, 1)) Then
        '    '        Exit For
        '    '    Else
        '    '        kodeKategori = kodeKategori & txtkode.Text.Substring(i, 1)
        '    '    End If
        '    'Next
        '    If txtkode.Text.Length >= 5 Then
        '        IDKategori = EksekusiSQLSkalar("select NoID from MKategori where Kode='" & Replace(Mid(txtkode.Text, 1, 5), "'", "''") & "'")
        '        'Dim lkedit As DevExpress.XtraEditors.SearchLookUpEdit
        '        For Each ctrl In LC1.Controls
        '            If ctrl.name.ToString.ToLower = "txtidkategori" Then
        '                ctrl.editvalue = IDKategori
        '                ' lkedit.see()
        '                Exit For 'semua yg dicari sudah ketemu
        '            End If
        '        Next
        '    End If


        'End If
    End Sub

    Private Sub txtEdit_EditValueChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim dstemp As New DataSet
        Dim SQL As String = ""
        Try
            'If sender.name = "txtkode" Then
            '    Dim kodekategori As String = ""
            '    'sender = sender.ToString.ToUpper
            '    For i As Integer = 1 To sender.text.length
            '        If IsNumeric(Mid(sender.text, i, 1)) Then

            '        Else
            '            kodekategori = kodekategori & Mid(sender.text, i, 1).ToUpper
            '        End If
            '    Next
            'End If
            If sender.name.ToString.ToLower = "txtidsatuan".ToString.ToLower Then
                If TempInsertBaru Then
                    EksekusiSQL("DELETE FROM MBarangD WHERE IDBarang=" & NoID)
                    For Each ctrl In LC1.Controls
                        If ctrl.name.tolower = "txtIDSatuan".ToLower Then
                            txtIDSatuanBase = CType(ctrl, SearchLookUpEdit)
                            Exit For
                        End If
                    Next
                    SQL = "SELECT MSatuan.* FROM MSatuan WHERE (NoID=" & NullToLong(txtIDSatuanBase.EditValue) & " OR IDBase=" & NullToLong(txtIDSatuanBase.EditValue) & ") AND IsActive=1 AND IsAutoInsert=1"
                    dstemp = ExecuteDataset("MSatuanTemp", SQL)
                    If dstemp.Tables("MSatuanTemp").Rows.Count >= 1 Then
                        For i As Integer = 0 To dstemp.Tables("MSatuanTemp").Rows.Count - 1
                            SQL = "Insert into mbarangd (NoID,IDBarang,IDSatuan,Konversi,IsActive,IsBeli,IsJual,IsJualPOS) values ("
                            SQL &= GetNewID("mbarangd", "NoID") & "," & NoID & "," & NullToLong(dstemp.Tables("MSatuanTemp").Rows(i).Item("NoID")) & "," & NullToDbl(dstemp.Tables("MSatuanTemp").Rows(i).Item("Konversi")) & ",1,1,1," & IIf(NullToDbl(dstemp.Tables("MSatuanTemp").Rows(i).Item("Konversi")) = 1, 1, 0) & ")"
                            EksekusiSQL(SQL)
                        Next
                    End If
                    RefreshStock()
                End If
            End If
        Catch ex As Exception

        Finally
            dstemp.Dispose()
        End Try
    End Sub
    Private Sub lkEdit_EditValueChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Try
            If sender.name.ToString.ToLower = "txtidtypepajak" Then
                HitungHarga()
                'Exit Sub
            End If
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
                                    If isNew And sender.name.tolower = "txtidkategori" And IsNumeric(sender.editvalue) Then
                                        If ctrl.name.ToString.ToLower = "txtkode" Then
                                            ctrl.editvalue = ctrl.editvalue & Format(NullToLong(EksekusiSQLSkalar("select max(right(Kode,4)) from mbarang where idkategori=" & sender.editvalue) + 1), "0000")
                                        ElseIf ctrl.name.ToString.ToLower = "txtbarcode" Then
                                            If TypeBarcode = TypeBarcode_.Ean13 Then
                                                If TypeKodeBarang = TypeKodeBarang_.TidakBerhubungan Then
                                                    ctrl.editvalue = Append_EAN13_Checksum("8" & (NullToLong(EksekusiSQlSkalarNew("SELECT MAX(NoID) FROM MBarangD")).ToString("0000000000") & 1))
                                                Else
                                                    ctrl.editvalue = Append_EAN13_Checksum("8" & CLng(ctrl.editvalue & Format(NullToLong(EksekusiSQLSkalar("select max(right(Kode,4)) from mbarang where idkategori=" & sender.editvalue) + 1), "0000")).ToString("00000000000"))
                                                End If
                                            ElseIf TypeBarcode = TypeBarcode_.Ean8 Then
                                                ctrl.editvalue = EAN8_Checksum(CLng(ctrl.editvalue & Format(NullToLong(EksekusiSQLSkalar("select max(right(Kode,4)) from mbarang where idkategori=" & sender.editvalue) + 1), "0000")).ToString("0000000"))
                                            Else


                                            End If

                                        End If
                                        Exit For
                                    End If
                                End If
                            Next
                        Next
                    End If
                Next
            End If
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Information)
        End Try
    End Sub

    Private Sub lkEdit_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles lkEdit.LostFocus

    End Sub

    Private Sub clcEdit_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        If sender.name.ToString.ToLower = "txthargabeli" Or sender.name.ToString.ToLower = "txthargabelipcsbruto" _
        Or sender.name.ToString.ToLower = "txtdiscbeli1" Or sender.name.ToString.ToLower = "txtdiscbeli2" _
        Or sender.name.ToString.ToLower = "txtdiscrp" Or sender.name.ToString.ToLower = "txtctnpcs" Or sender.name.ToString.ToLower = "txtdiscbeli3" Then
            'Or sender.name.ToString.ToLower = "txthargajual"
            If sender.name.ToString.ToLower = "txthargabeli" Or sender.name.ToString.ToLower = "txtctnpcs" Then
                HitungHargaBeliPcsByHargaBeli()
            ElseIf sender.name.ToString.ToLower = "txthargabelipcsbruto" Then
                HitungHargaBeliByHargaBeliPcs()
            End If
            HitungHarga()
            HitungHargaJualPromoProsenByRp()
            Exit Sub
        ElseIf sender.name.ToString.ToLower = "txtdiscpromorp" Then
            HitungHargaJualPromoProsenByRp()
        ElseIf sender.name.ToString.ToLower = "txtdiscpromo" Then
            HitungHargaJualPromoByProsen()
        ElseIf sender.name.ToString.ToLower = "txtprosenup" Then
            HitungHargaJualByMargin()
        ElseIf sender.name.ToString.ToLower = "txtprosenup2" Then
            HitungHargaJualByMargin2()
        ElseIf sender.name.ToString.ToLower = "txtprosenup3" Then
            HitungHargaJualByMargin3()
        ElseIf sender.name.ToString.ToLower = "txthargajual" Then
            HitungMarginByHargaJual()
        ElseIf sender.name.ToString.ToLower = "txthargajual2" Then
            HitungMarginByHargaJual2()
        ElseIf sender.name.ToString.ToLower = "txthargajual3" Then
            HitungMarginByHargaJual3()
        ElseIf sender.name.ToString.ToLower = "txtDiscMemberProsen2".ToLower Then
            HitungHargaJualMemberByProsen()
        ElseIf sender.name.ToString.ToLower = "txtDiscMemberRp2".ToLower Then
            HitungHargaJualMemberProsenByRp()
        ElseIf sender.name.ToString.ToLower = "txtHargaPDP".ToLower Or sender.name.ToString.ToLower = "txtHargaPDPMember".ToLower Then
            HitungHargaJualPDP()
        End If
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
        SimpleButton1.PerformClick()
    End Sub

    'Private Function IsValidasi()
    '    For Each ctrl In LC1.Controls
    '        If ctrl.name.tolower = "txtkode".ToLower Then
    '            Dim txtKode As TextEdit = CType(ctrl, TextEdit)
    '            If txtKode.Text = "" Then
    '                XtraMessageBox.Show("Kode masih kosong.")
    '                txtKode.Focus()
    '                Return False
    '                Exit Function
    '            End If
    '            If CekKodeValid(txtKode.Text, "", "", "Kode", Not isNew, " AND NoID<>" & NoID) Then
    '                XtraMessageBox.Show("Kode masih kosong.")
    '                txtKode.Focus()
    '                Return False
    '                Exit Function
    '            End If
    '        End If
    '    Next
    '    Return True
    'End Function
    Private Function IsValidasi()
        Dim txtKode As TextEdit = Nothing
        Dim txtAlias As TextEdit = Nothing
        Dim txtNama As TextEdit = Nothing
        Dim txtbarcode As TextEdit = Nothing
        Dim txtNamaAlias As TextEdit = Nothing
        Dim txtIsi1Karton As CalcEdit = Nothing
        Dim txtIsi1KartonPcs As CalcEdit = Nothing
        Dim txtKonversisatuanharga As CalcEdit = Nothing
        Dim txtHargapasar As CalcEdit = Nothing
        Dim HargaMinA As CalcEdit = Nothing, HargaA As CalcEdit = Nothing
        Dim HargaMinB As CalcEdit = Nothing, HargaB As CalcEdit = Nothing
        Dim HargaMinC As CalcEdit = Nothing, HargaC As CalcEdit = Nothing
        Dim HargaMinD As CalcEdit = Nothing, HargaD As CalcEdit = Nothing
        Dim HargaMinE As CalcEdit = Nothing, HargaE As CalcEdit = Nothing
        Dim HargaMinF As CalcEdit = Nothing, HargaF As CalcEdit = Nothing

        Dim txtHargaPokok As CalcEdit = Nothing, txtHargaJual As CalcEdit = Nothing

        Dim txtIDSupplier1 As SearchLookUpEdit = Nothing
        Dim txtIDSupplier2 As SearchLookUpEdit = Nothing
        Dim txtIDSupplier3 As SearchLookUpEdit = Nothing
        Dim txtIDSupplier4 As SearchLookUpEdit = Nothing
        Dim txtIDSupplier5 As SearchLookUpEdit = Nothing
        Dim txtIDKategori As SearchLookUpEdit = Nothing

        Dim SQL As String = ""
        For Each ctrl In LC1.Controls
            If ctrl.name.tolower = "txtkode".ToLower Then
                txtKode = CType(ctrl, TextEdit)
                KodeSekarang = txtKode.Text
            ElseIf ctrl.name.tolower = "txtalias".ToLower Then
                txtAlias = CType(ctrl, TextEdit)
                KodeAliasSekarang = txtAlias.Text
                'ElseIf ctrl.name.tolower = "txtalias".ToLower Then
                '    txtAlias = CType(ctrl, TextEdit)
                '    KodeAliasSekarang = txtAlias.Text
            ElseIf ctrl.name.tolower = "txtbarcode".ToLower Then
                txtbarcode = CType(ctrl, TextEdit)
            ElseIf ctrl.name.tolower = "txtNama".ToLower Then
                txtNama = CType(ctrl, TextEdit)
            ElseIf ctrl.name.tolower = "txtCtn_duz".ToLower Then
                txtIsi1Karton = CType(ctrl, CalcEdit)
            ElseIf ctrl.name.tolower = "txtCtnPcs".ToLower Then
                txtIsi1KartonPcs = CType(ctrl, CalcEdit)
            ElseIf ctrl.name.tolower = "txtHargapasar".ToLower Then
                txtHargapasar = CType(ctrl, CalcEdit)
            ElseIf ctrl.name.tolower = "txtKonversisatuanharga".ToLower Then
                txtKonversisatuanharga = CType(ctrl, CalcEdit)
                'ElseIf ctrl.name.tolower = "txtHargajuala".ToLower Then
                '    HargaA = CType(ctrl, CalcEdit)
                'ElseIf ctrl.name.tolower = "txtHargamina".ToLower Then
                '    HargaMinA = CType(ctrl, CalcEdit)
                'ElseIf ctrl.name.tolower = "txtHargajualb".ToLower Then
                '    HargaB = CType(ctrl, CalcEdit)
                'ElseIf ctrl.name.tolower = "txtHargaminb".ToLower Then
                '    HargaMinB = CType(ctrl, CalcEdit)
                'ElseIf ctrl.name.tolower = "txtHargajualc".ToLower Then
                '    HargaC = CType(ctrl, CalcEdit)
                'ElseIf ctrl.name.tolower = "txtHargaminc".ToLower Then
                '    HargaMinC = CType(ctrl, CalcEdit)
                'ElseIf ctrl.name.tolower = "txtHargajuald".ToLower Then
                '    HargaD = CType(ctrl, CalcEdit)
                'ElseIf ctrl.name.tolower = "txtHargamind".ToLower Then
                '    HargaMinD = CType(ctrl, CalcEdit)
                'ElseIf ctrl.name.tolower = "txtHargajuale".ToLower Then
                '    HargaE = CType(ctrl, CalcEdit)
                'ElseIf ctrl.name.tolower = "txtHargamine".ToLower Then
                '    HargaMinE = CType(ctrl, CalcEdit)
                'ElseIf ctrl.name.tolower = "txtHargajualf".ToLower Then
                '    HargaF = CType(ctrl, CalcEdit)
                'ElseIf ctrl.name.tolower = "txtHargaminf".ToLower Then
                '    HargaMinF = CType(ctrl, CalcEdit)

            ElseIf ctrl.name.tolower = "txtIDSupplier1".ToLower Then
                txtIDSupplier1 = CType(ctrl, SearchLookUpEdit)
            ElseIf ctrl.name.tolower = "txtIDSupplier2".ToLower Then
                txtIDSupplier2 = CType(ctrl, SearchLookUpEdit)
            ElseIf ctrl.name.tolower = "txtIDSupplier3".ToLower Then
                txtIDSupplier3 = CType(ctrl, SearchLookUpEdit)
            ElseIf ctrl.name.tolower = "txtIDSupplier4".ToLower Then
                txtIDSupplier4 = CType(ctrl, SearchLookUpEdit)
            ElseIf ctrl.name.tolower = "txtIDSupplier5".ToLower Then
                txtIDSupplier5 = CType(ctrl, SearchLookUpEdit)
            ElseIf ctrl.name.tolower = "txtHargaBeliPcs".ToLower Then
                txtHargaPokok = CType(ctrl, CalcEdit)
            ElseIf ctrl.name.tolower = "txtHargaJual".ToLower Then
                txtHargaJual = CType(ctrl, CalcEdit)
                'ElseIf ctrl.name.tolower = "txtIDSatuan".ToLower Then
                '    txtIDSatuanBase = CType(ctrl, SearchLookUpEdit)
            ElseIf ctrl.name.tolower = "txtIDKategori".ToLower Then
                txtIDKategori = CType(ctrl, SearchLookUpEdit)
            End If
        Next

        If txtKode Is Nothing Then
            XtraMessageBox.Show("Kolom kode belum didefinisikan", NamaAplikasi, MessageBoxButtons.OK)
            txtKode.Focus()
            Return False
        End If
        If txtAlias Is Nothing Then
            XtraMessageBox.Show("Kolom alias belum didefinisikan", NamaAplikasi, MessageBoxButtons.OK)
            txtKode.Focus()
            Return False
        End If
        If txtNama Is Nothing Then
            XtraMessageBox.Show("Kolom nama belum didefinisikan", NamaAplikasi, MessageBoxButtons.OK)
            txtNama.Focus()
            Return False
        End If
        'If txtIsi1Karton Is Nothing Then
        '    XtraMessageBox.Show("Kolom isi satu karton belum didefinisikan", NamaAplikasi, MessageBoxButtons.OK)
        '    txtIsi1Karton.Focus()
        '    Return False
        'End If
        'If txtIsi1KartonPcs Is Nothing Then
        '    XtraMessageBox.Show("Kolom isi satu karton Pcs belum didefinisikan", NamaAplikasi, MessageBoxButtons.OK)
        '    txtIsi1KartonPcs.Focus()
        '    Return False
        'End If
        'If txtKonversisatuanharga Is Nothing Then
        '    XtraMessageBox.Show("Kolom konversi satuan dasar belum didefinisikan", NamaAplikasi, MessageBoxButtons.OK)
        '    txtKonversisatuanharga.Focus()
        '    Return False
        'End If
        'If txtHargapasar Is Nothing Then
        '    XtraMessageBox.Show("Kolom Harga Pasar belum didefinisikan", NamaAplikasi, MessageBoxButtons.OK)
        '    txtHargapasar.Focus()
        '    Return False
        'End If
        'If HargaA Is Nothing Or HargaMinA Is Nothing Then
        '    XtraMessageBox.Show("Kolom HargaA belum didefinisikan", NamaAplikasi, MessageBoxButtons.OK)
        '    HargaA.Focus()
        '    Return False
        'End If
        'If HargaB Is Nothing Or HargaMinB Is Nothing Then
        '    XtraMessageBox.Show("Kolom HargaB belum didefinisikan", NamaAplikasi, MessageBoxButtons.OK)
        '    HargaB.Focus()
        '    Return False
        'End If
        'If HargaC Is Nothing Or HargaMinC Is Nothing Then
        '    XtraMessageBox.Show("Kolom HargaC belum didefinisikan", NamaAplikasi, MessageBoxButtons.OK)
        '    HargaC.Focus()
        '    Return False
        'End If
        'If HargaD Is Nothing Or HargaMinD Is Nothing Then
        '    XtraMessageBox.Show("Kolom HargaD belum didefinisikan", NamaAplikasi, MessageBoxButtons.OK)
        '    HargaD.Focus()
        '    Return False
        'End If
        'If HargaE Is Nothing Or HargaMinE Is Nothing Then
        '    XtraMessageBox.Show("Kolom HargaE belum didefinisikan", NamaAplikasi, MessageBoxButtons.OK)
        '    HargaE.Focus()
        '    Return False
        'End If
        'If HargaF Is Nothing Or HargaMinF Is Nothing Then
        '    XtraMessageBox.Show("Kolom HargaF belum didefinisikan", NamaAplikasi, MessageBoxButtons.OK)
        '    HargaF.Focus()
        '    Return False
        'End If
        If txtIDSupplier1 Is Nothing Or txtIDSupplier2 Is Nothing Or txtIDSupplier3 Is Nothing Or txtIDSupplier4 Is Nothing Or txtIDSupplier5 Is Nothing Then
            XtraMessageBox.Show("Kolom Default Supplier belum didefinisikan", NamaAplikasi, MessageBoxButtons.OK)
            Return False
        End If

        'If txtIDSatuanBase.Text.Trim = "" Then
        '    XtraMessageBox.Show("Satuan base masih kosong.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Warning)
        '    txtIDSatuanBase.Focus()
        '    Return False
        'End If
        If Not TypeKodeBarang = TypeKodeBarang_.TidakBerhubungan Then
            If txtIDKategori.Text.Trim = "" AndAlso txtKode.Text <> "" Then
                XtraMessageBox.Show("Kode masih tidak sesuai dengan golongan barang.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Warning)
                txtKode.Focus()
                Return False
            End If
            If txtIDKategori.Text.Trim <> "" Then
                If txtKode.Text.Length < NullToStr(EksekusiSQlSkalarNew("SELECT KODE FROM MKategori WHERE NoID=" & NullToLong(txtIDKategori.EditValue))).Length Then
                    XtraMessageBox.Show("Kode masih tidak sesuai dengan golongan barang.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Warning)
                    txtKode.Focus()
                    Return False
                End If
                If NullToStr(EksekusiSQlSkalarNew("SELECT KODE FROM MKategori WHERE NoID=" & NullToLong(txtIDKategori.EditValue))) <> txtKode.Text.Trim.Substring(0, NullToStr(EksekusiSQlSkalarNew("SELECT KODE FROM MKategori WHERE NoID=" & NullToLong(txtIDKategori.EditValue))).Length) Then
                    XtraMessageBox.Show("Kode masih tidak sesuai dengan golongan barang.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Warning)
                    txtKode.Focus()
                    Return False
                End If
            End If
        End If



        If txtIDSupplier5.Text.Trim <> "" AndAlso _
        (txtIDSupplier5.EditValue = NullToLong(txtIDSupplier1.EditValue) Or _
         txtIDSupplier5.EditValue = NullToLong(txtIDSupplier2.EditValue) Or _
         txtIDSupplier5.EditValue = NullToLong(txtIDSupplier3.EditValue) Or _
         txtIDSupplier5.EditValue = NullToLong(txtIDSupplier4.EditValue)) Then
            XtraMessageBox.Show("Kode default supplier5 masih sama dengan yg lain.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Warning)
            txtIDSupplier5.Focus()
            Return False
        End If
        If txtIDSupplier4.Text.Trim <> "" AndAlso _
        (txtIDSupplier4.EditValue = NullToLong(txtIDSupplier1.EditValue) Or _
         txtIDSupplier4.EditValue = NullToLong(txtIDSupplier2.EditValue) Or _
         txtIDSupplier4.EditValue = NullToLong(txtIDSupplier3.EditValue) Or _
         txtIDSupplier4.EditValue = NullToLong(txtIDSupplier5.EditValue)) Then
            XtraMessageBox.Show("Kode default supplier4 masih sama dengan yg lain.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Warning)
            txtIDSupplier4.Focus()
            Return False
        End If
        If txtIDSupplier3.Text.Trim <> "" AndAlso _
        (txtIDSupplier3.EditValue = NullToLong(txtIDSupplier1.EditValue) Or _
         txtIDSupplier3.EditValue = NullToLong(txtIDSupplier2.EditValue) Or _
         txtIDSupplier3.EditValue = NullToLong(txtIDSupplier4.EditValue) Or _
         txtIDSupplier3.EditValue = NullToLong(txtIDSupplier5.EditValue)) Then
            XtraMessageBox.Show("Kode default supplier3 masih sama dengan yg lain.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Warning)
            txtIDSupplier3.Focus()
            Return False
        End If
        If txtIDSupplier2.Text.Trim <> "" AndAlso _
        (txtIDSupplier2.EditValue = NullToLong(txtIDSupplier1.EditValue) Or _
         txtIDSupplier2.EditValue = NullToLong(txtIDSupplier3.EditValue) Or _
         txtIDSupplier2.EditValue = NullToLong(txtIDSupplier4.EditValue) Or _
         txtIDSupplier2.EditValue = NullToLong(txtIDSupplier5.EditValue)) Then
            XtraMessageBox.Show("Kode default supplier2 masih sama dengan yg lain.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Warning)
            txtIDSupplier2.Focus()
            Return False
        End If
        If txtIDSupplier1.Text.Trim <> "" AndAlso _
        (txtIDSupplier1.EditValue = NullToLong(txtIDSupplier2.EditValue) Or _
         txtIDSupplier1.EditValue = NullToLong(txtIDSupplier3.EditValue) Or _
         txtIDSupplier1.EditValue = NullToLong(txtIDSupplier4.EditValue) Or _
         txtIDSupplier1.EditValue = NullToLong(txtIDSupplier5.EditValue)) Then
            XtraMessageBox.Show("Kode default supplier1 masih sama dengan yg lain.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Warning)
            txtIDSupplier1.Focus()
            Return False
        End If

        'If NullToDbl(txtIsi1Karton.EditValue) <= 0 Then
        '    XtraMessageBox.Show("Isi satu Karton tidak boleh kurang dari atau sama dengan 0.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Warning)
        '    txtIsi1Karton.Focus()
        '    Return False
        'End If
        'If NullToDbl(txtIsi1KartonPcs.EditValue) <= 0 Then
        '    XtraMessageBox.Show("Isi satu Karton Pcs tidak boleh kurang dari atau sama dengan 0.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Warning)
        '    txtIsi1KartonPcs.Focus()
        '    Return False
        'End If
        'If NullToDbl(txtKonversisatuanharga.EditValue) <= 0 Then
        '    XtraMessageBox.Show("Konversi satuan dasar tidak boleh kurang dari atau sama dengan 0.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Warning)
        '    txtKonversisatuanharga.Focus()
        '    Return False
        'End If
        'If NullToDbl(txtHargapasar.EditValue) <= 0 Then
        '    XtraMessageBox.Show("Harga Pasar tidak boleh kurang dari atau sama dengan 0.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Warning)
        '    txtHargapasar.Focus()
        '    Return False
        'End If
        'If NullToDbl(HargaA.EditValue) <= 0 Or NullToDbl(HargaMinA.EditValue) <= 0 Then
        '    XtraMessageBox.Show("HargaA / Minimum HargaA tidak boleh kurang dari atau sama dengan 0.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Warning)
        '    HargaA.Focus()
        '    Return False
        'End If
        'If NullToDbl(HargaB.EditValue) <= 0 Or NullToDbl(HargaMinB.EditValue) <= 0 Then
        '    XtraMessageBox.Show("HargaB / Minimum HargaB tidak boleh kurang dari atau sama dengan 0.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Warning)
        '    HargaB.Focus()
        '    Return False
        'End If
        'If NullToDbl(HargaC.EditValue) <= 0 Or NullToDbl(HargaMinC.EditValue) <= 0 Then
        '    XtraMessageBox.Show("HargaC / Minimum HargaC tidak boleh kurang dari atau sama dengan 0.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Warning)
        '    HargaC.Focus()
        '    Return False
        'End If
        'If NullToDbl(HargaD.EditValue) <= 0 Or NullToDbl(HargaMinD.EditValue) <= 0 Then
        '    XtraMessageBox.Show("HargaD / Minimum HargaD tidak boleh kurang dari atau sama dengan 0.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Warning)
        '    HargaD.Focus()
        '    Return False
        'End If
        'If NullToDbl(HargaE.EditValue) <= 0 Or NullToDbl(HargaMinE.EditValue) <= 0 Then
        '    XtraMessageBox.Show("HargaE / Minimum HargaE tidak boleh kurang dari atau sama dengan 0.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Warning)
        '    HargaE.Focus()
        '    Return False
        'End If
        'If NullToDbl(HargaF.EditValue) <= 0 Or NullToDbl(HargaMinF.EditValue) <= 0 Then
        '    XtraMessageBox.Show("HargaF / Minimum HargaF tidak boleh kurang dari atau sama dengan 0.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Warning)
        '    HargaF.Focus()
        '    Return False
        'End If
        'If txtKode.Text.Length >= 10 Then
        '    XtraMessageBox.Show("Kode terlalu panjang.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Warning)
        '    txtKode.Focus()
        '    Return False
        'End If
        If txtHargaJual.EditValue < txtHargaPokok.EditValue Then
            XtraMessageBox.Show("Harga Jual lebih rendah dari Harga Modal.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Warning)
            txtHargaJual.Focus()
            Return False
        End If
        If txtNama.Text.Trim = "" Then
            XtraMessageBox.Show("Nama masih kosong.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Warning)
            txtNama.Focus()
            Return False
        End If
        If txtKode.Text.Trim = "" Then
            XtraMessageBox.Show("Kode masih kosong.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Warning)
            txtKode.Focus()
            Return False
        Else
            txtKode.EditValue = UCase(txtKode.Text)
            txtAlias.EditValue = UCase(txtAlias.Text)
        End If
        If txtbarcode.Text.Trim <> "" AndAlso isNew Then
            SQL = "SELECT COUNT(MBarangD.Barcode) FROM MBarang INNER JOIN MBarangD ON MBarang.NoID=MBarangD.IDBarang" & _
                  " WHERE UPPER(MBarangD.Barcode)='" & FixApostropi(txtbarcode.Text.ToUpper) & "'"
            If NullToLong(EksekusiSQlSkalarNew(SQL)) >= 1 Then
                XtraMessageBox.Show("Barcode Sudah dipakai.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Warning)
                txtbarcode.Focus()
                Return False
            End If
        End If
        If txtKode.Text.Trim <> "" Then
            If CekKodeValid(txtKode.Text, KodeLama, TableName, "Kode", Not isNew) Then
                XtraMessageBox.Show("Kode Sudah dipakai.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Warning)
                txtKode.Focus()
                Return False
            End If
        Else
            If CekKodeValid(txtKode.Text, KodeLama, TableName, "Kode", Not isNew, " AND KodeAlias='" & FixApostropi(txtAlias.Text) & "'") Then
                XtraMessageBox.Show("Kode Sudah dipakai.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Warning)
                txtKode.Focus()
                Return False
            End If
        End If
        'If CekKodeValid(txtAlias.Text, KodeAlias, TableName, "KodeAlias", Not isNew, " AND KodeAlias<>'' ") Then
        '    If XtraMessageBox.Show("Alias Sudah dipakai." & vbCrLf & "Ingin melanjutkan penyimpanan?", NamaAplikasi, MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) = Windows.Forms.DialogResult.No Then
        '        txtAlias.Focus()
        '        Return False
        '    End If
        'End If
        If CekKodeValid(txtNama.Text, NamaLama, TableName, "Nama", Not isNew, " AND Nama<>''") Then
            If XtraMessageBox.Show("Nama Sudah dipakai, yakin ingin melanjutkan penyimpanan?", NamaAplikasi, MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) = Windows.Forms.DialogResult.No Then
                txtNama.Focus()
                Return False
            End If
        End If
        'If CekKodeValid(txtbarcode.Text, barcodel, TableName, "Nama", Not isNew, " AND Nama<>''") Then
        '    If XtraMessageBox.Show("Nama Sudah dipakai, yakin ingin melanjutkan penyimpanan?", NamaAplikasi, MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) = Windows.Forms.DialogResult.No Then
        '        txtNama.Focus()
        '        Return False
        '    End If
        'End If
        Return True
    End Function
    Private Sub SimpleButton1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SimpleButton1.Click
        Dim PesanSalah As String = ""
        Dim SQL As String = ""
        Dim DS As New DataSet
        'Dim NewBarcode As String = ""
        Try
            If IsValidasi() Then
                If Simpan(PesanSalah) = True Then
                    If isNew Then
                        isNew = False
                        TempInsertBaru = True
                        SetTombol()
                        'CITY TOYS BERDASARKAN MASTER SATUAN
                        'SQL = "SELECT MSatuan.* FROM MSatuan WHERE (NoID=" & NullToLong(txtIDSatuanBase.EditValue) & " OR IDBase=" & NullToLong(txtIDSatuanBase.EditValue) & ") AND IsActive=1 AND IsAutoInsert=1"
                        'DS = ExecuteDataset("MSatuan", SQL)
                        'If DS.Tables("MSatuan").Rows.Count >= 1 Then
                        '    For i As Integer = 0 To DS.Tables("MSatuan").Rows.Count - 1
                        '        SQL = "insert into mbarangd (NoID,IDBarang,IDSatuan,Konversi,IsActive,IsBeli,IsJual,IsJualPOS) values ("
                        '        SQL &= GetNewID("mbarangd", "NoID") & "," & NoID & "," & NullToLong(DS.Tables("MSatuan").Rows(i).Item("NoID")) & "," & NullToDbl(DS.Tables("MSatuan").Rows(i).Item("Konversi")) & ",1,1,1," & IIf(NullToDbl(DS.Tables("MSatuan").Rows(i).Item("Konversi")) = 1, 1, 0) & ")"
                        '        EksekusiSQL(SQL)
                        '    Next
                        'End If
                        'SPBU 99, BERDASARKAN SATUAN DASAR/APA YG DIENTRI DI MASTER BARANg
                        SQL = "Insert into mbarangd (NoID,IDBarang,Barcode,IDSatuan,Konversi,HargaJual,HargaJual2,HargaJual3,ProsenUp,ProsenUp2,ProsenUp3,Qty1,Qty2,Qty3,IsActive,IsBeli,IsJual,IsJualPOS,IsAllowDisc,Harga1,Harga2,Harga3,IsGrosir,IsKelipatan,QtyKelipatan,HargaKelipatan) Select "
                        SQL &= GetNewID("mbarangd", "NoID") & ",MBarang.NoID,MBarang.Barcode,MBarang.IDSatuan,MSatuan.Konversi,MBarang.HargaJual,MBarang.HargaJual2,MBarang.HargaJual3,MBarang.ProsenUp,MBarang.ProsenUp2,MBarang.ProsenUp3,MBarang.Qty1,MBarang.Qty2,MBarang.Qty3,1,1,1,1,MBarang.HargaJual,MBarang.HargaJual2,MBarang.HargaJual3,0,0,0,0,0 from Mbarang inner join msatuan on mbarang.IDSatuan=MSatuan.NoID where MBarang.NoID=" & NoID
                        EksekusiSQL(SQL)
                        RefreshStock()
                        HitungHargaJualByMargin(True)
                        HitungHargaJualByMargin2(True)
                        HitungHargaJualByMargin3(True)
                        HitungHargaJualPromoByProsen(True)
                        HitungHargaJualPromoProsenByRp(True)
                        HitungMarginByHargaJual(True)
                        HitungMarginByHargaJual2(True)
                        HitungMarginByHargaJual3(True)
                        HitungHarga(True)
                        For Each ctrl In LC1.Controls
                            If ctrl.name.ToString.ToLower = "txtkode" Then
                                KodeLama = ctrl.text.ToString
                            ElseIf ctrl.name.ToString.ToLower = "txtnama" Then
                                NamaLama = ctrl.text.ToString
                            ElseIf ctrl.name.ToString.ToLower = "txtalias" Then
                                KodeAlias = ctrl.text.ToString
                            ElseIf ctrl.name.ToString.ToLower = "txtctn_duz" Then
                                clcCtnPcs_LostFocus(ctrl, e)
                            ElseIf ctrl.name.ToString.ToLower = "txtCtnPcs" Then
                                clcCtnPcs_LostFocus(ctrl, e)
                            End If
                        Next
                    Else
                        For Each ctl As Control In LC1.Controls
                            If ctl.Name.ToLower = "txthargabelipcsbruto".ToLower Then
                                clcEdit_LostFocus(ctl, Nothing)
                                Exit For
                            End If
                        Next
                        HitungHargaJualByMargin(True)
                        HitungHargaJualByMargin2(True)
                        HitungHargaJualByMargin3(True)
                        HitungHargaJualPromoByProsen(True)
                        HitungHargaJualPromoProsenByRp(True)
                        HitungMarginByHargaJual(True)
                        HitungMarginByHargaJual2(True)
                        HitungMarginByHargaJual3(True)
                        HitungHarga(True)
                        ''Update MPO/MBeli Alias
                        'If (KodeAlias <> "" And KodeLama = "") AndAlso (KodeSekarang <> "" And KodeAliasSekarang = "") Then
                        '    SQL = "UPDATE MPOD SET IsAlias=0 WHERE IsAlias=1 AND IDBarang=" & NoID
                        '    EksekusiSQL(SQL)
                        '    SQL = "UPDATE MBeliD SET IsAlias=0 WHERE IsAlias=1 AND IDBarang=" & NoID
                        '    EksekusiSQL(SQL)
                        'End If

                        'NewBarcode = Append_EAN13_Checksum("9" & NullToLong(NoID).ToString("00000000000")).ToString
                        'EksekusiSQL("UPDATE MBarang set Barcode='" & FixApostropi(NewBarcode) & "' WHERE NoID=" & NullToLong(NoID))

                        'For Each ctrl In LC1.Controls
                        '    If ctrl.name.ToString.ToLower = "txtfoto" Then
                        '        If Not TryCast(ctrl, PictureEdit).Image Is Nothing Then
                        '            If System.IO.File.Exists(FolderFoto & KodeSekarang & ".JPG") Then
                        '                System.IO.File.Delete(FolderFoto & KodeSekarang & ".JPG")
                        '                TryCast(ctrl, PictureEdit).Image.Save(FolderFoto & KodeSekarang & ".JPG")
                        '            Else
                        '                TryCast(ctrl, PictureEdit).Image.Save(FolderFoto & KodeSekarang & ".JPG")
                        '            End If
                        '        End If
                        '    End If
                        'Next

                        'KODE DIBAWAH UNTUK MEMBUKA FORM HANYA 1
                        Dim frmEntri As frmDaftarBarang = Nothing
                        Dim F As Object
                        For Each F In MdiParent.MdiChildren
                            If TypeOf F Is frmDaftarBarang Then
                                frmEntri = F
                                Exit For
                            End If
                        Next
                        If frmEntri Is Nothing Then
                            frmEntri = New frmDaftarBarang
                            frmEntri.WindowState = FormWindowState.Maximized
                            frmEntri.MdiParent = Me.MdiParent
                        End If
                        frmEntri.FocusRowColumn = FocusRowColumn
                        frmEntri.DirectNoID = NoID
                        frmEntri.ShowNoID = True
                        frmEntri.Show()
                        frmEntri.Focus()

                        'Refresh Public DataSet

                        'DialogResult = Windows.Forms.DialogResult.OK
                        Close()
                        Dispose()
                    End If
                Else
                    If PesanSalah <> "" Then
                        XtraMessageBox.Show("Info Kesalahan : " & vbCrLf & PesanSalah, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                    End If
                End If
            End If
        Catch ex As Exception
            XtraMessageBox.Show("Info kesalahan:" & vbCrLf & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            DS.Dispose()
        End Try
    End Sub

    Private Sub SimpleButton3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SimpleButton3.Click
        Batal()
    End Sub

    Private Sub SimpleButton5_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBaru.Click
        Select Case XtraTabControl1.SelectedTabPageIndex
            Case 0
                IsiVariabel()
                Baru()
            Case 1
            Case 2
                ItemPaketBaru()
        End Select
    End Sub
    Sub Baru()
        Dim FormEntriName As String = "EntriBarangD"
        'Select Case FormEntriName
        '    Case "EntriBarang"
        '        Dim Brg As New clsBarang
        '        Brg.FormName = FormEntriName
        '        Brg.isNew = True
        '        If Brg.Showdialog(Me) = Windows.Forms.DialogResult.OK Then
        '            EksekusiSQL("Update MBarangD set IDBarang=" & NoID & " where MBarangD.NoID=" & Brg.NoID)
        '            TampilDetil()
        '            GV1.FocusedRowHandle = GV1.LocateByValue("NoID", Brg.NoID)
        '        End If
        '        Brg.Dispose()
        '    Case Else
        Dim x As New frmSimpleEntri
        x.FormName = FormEntriName
        x.isNew = True
        x.IDParent = NoID
        If x.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
            EksekusiSQL("Update MBarangD set LastUpdated=getdate(), IDBarang=" & NoID & " Where MBarangD.NoID=" & x.NoID)
            RefreshStock()
            GV1.FocusedRowHandle = GV1.LocateByDisplayText(0, GV1.Columns("NoID"), NoID.ToString("#,##0"))

            'For Each ctrl In LC1.Controls
            '    If ctrl.name.tolower = "txtCtnPcs".ToLower Then
            '        If x.txtSatuanMBarangD.Text.Length >= 6 AndAlso x.txtSatuanMBarangD.Text.Trim.Substring(0, 6) = "KARTON" Then
            '            ctrl.editvalue = NullToDbl(x.KonversiCTN)
            '            clcCtnPcs_LostFocus(ctrl, Nothing)
            '        End If
            '        Exit For
            '    End If
            'Next
        End If
        x.Dispose()
        'End Select
    End Sub

    Sub ItemPaketBaru()
        Dim FormEntriName As String = "EntriBarangDPaket"
        'Select Case FormEntriName
        '    Case "EntriBarang"
        '        Dim Brg As New clsBarang
        '        Brg.FormName = FormEntriName
        '        Brg.isNew = True
        '        If Brg.Showdialog(Me) = Windows.Forms.DialogResult.OK Then
        '            EksekusiSQL("Update MBarangD set IDBarang=" & NoID & " where MBarangD.NoID=" & Brg.NoID)
        '            TampilDetil()
        '            GV1.FocusedRowHandle = GV1.LocateByValue("NoID", Brg.NoID)
        '        End If
        '        Brg.Dispose()
        '    Case Else
        Dim x As New frmSimpleEntri
        x.FormName = FormEntriName
        x.isNew = True
        x.IDParent = NoID
        If x.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
            EksekusiSQL("Update MBarangDPaket set IDBarang=" & NoID & " Where MBarangDPaket.NoID=" & x.NoID)
            RefreshStock()
            GridView2.FocusedRowHandle = GridView2.LocateByDisplayText(0, GridView2.Columns("NoID"), NoID.ToString("#,##0"))

            'For Each ctrl In LC1.Controls
            '    If ctrl.name.tolower = "txtCtnPcs".ToLower Then
            '        If x.txtSatuanMBarangD.Text.Length >= 6 AndAlso x.txtSatuanMBarangD.Text.Trim.Substring(0, 6) = "KARTON" Then
            '            ctrl.editvalue = NullToDbl(x.KonversiCTN)
            '            clcCtnPcs_LostFocus(ctrl, Nothing)
            '        End If
            '        Exit For
            '    End If
            'Next
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

                IsiVariabel()
                Edit()
            Case 1
            Case 2
                EditPaket()
        End Select
    End Sub

    Private Sub mnEditSatuan_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles mnEditSatuan.ItemClick
        btnEdit.PerformClick()
    End Sub

    Private Sub mnHapusSatuan_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles mnHapusSatuan.ItemClick
        Hapus()
    End Sub

    Private Sub SimpleButton2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnHapus.Click
        Select Case XtraTabControl1.SelectedTabPageIndex
            Case 0
                Hapus()
            Case 1
            Case 2
                HapusPaket()
        End Select
    End Sub
    Sub Hapus()
        Dim view As ColumnView = GC1.FocusedView
        Try
            Dim row As System.Data.DataRow = view.GetDataRow(GV1.FocusedRowHandle)
            Dim dc As Integer = GV1.FocusedRowHandle
            Dim NoID As Long = NullToLong(row("NoID"))
            If XtraMessageBox.Show("Ingin menghapus item ini ?", NamaAplikasi, MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) = Windows.Forms.DialogResult.Yes Then
                EksekusiSQL("Update MBarangD Set IsActive=0 where NoID=" & NoID.ToString)
                RefreshStock()
            End If
        Catch ex As Exception
            XtraMessageBox.Show("Untuk menghapus pilih item satuan yang akan dihapus terlebih dahulu lalu tekan tombol hapus", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally

        End Try
    End Sub
    Sub HapusPaket()
        Dim view As ColumnView = GridControl2.FocusedView
        Try
            Dim row As System.Data.DataRow = view.GetDataRow(GridView2.FocusedRowHandle)
            Dim dc As Integer = GridView2.FocusedRowHandle
            Dim NoID As Long = NullToLong(row("NoID"))
            If MsgBox("yakin mau hapus item barang paket ini?", MsgBoxStyle.YesNo + MsgBoxStyle.Question) = MsgBoxResult.Yes Then
                EksekusiSQL("Delete MBarangDPaket where NoID=" & NoID.ToString)
                RefreshStock()
            End If

        Catch ex As Exception
            XtraMessageBox.Show("Untuk menghapus pilih item satuan yang akan dihapus terlebih dahulu lalu tekan tombol hapus", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally

        End Try
    End Sub
    Sub Edit()
        Dim x As frmSimpleEntri
        'Dim Brg As clsBarang
        Dim FormEntriName As String = "EntriBarangD"
        Dim view As ColumnView = GC1.FocusedView
        Try
            Dim row As System.Data.DataRow = view.GetDataRow(GV1.FocusedRowHandle)
            Dim dc As Integer = GV1.FocusedRowHandle
            Dim IDDetil As Long = NullToLong(row("NoID"))
            'If FormEntriName = "EntriBarang" Then
            '    Brg = New clsBarang
            '    Brg.FormName = FormEntriName
            '    Brg.isNew = False
            '    Brg.NoID = NoID
            '    If Brg.Showdialog(Me) = Windows.Forms.DialogResult.OK Then
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
            x.NoID = IDDetil
            x.IDParent = NoID
            If x.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
                EksekusiSQL("Update MBarangD Set LastUpdated=getdate() where NoID=" & IDDetil.ToString)
                RefreshStock()
                GV1.ClearSelection()
                GV1.FocusedRowHandle = GV1.LocateByDisplayText(0, GV1.Columns("NoID"), NoID.ToString("#,##0"))
                GV1.SelectRow(GV1.FocusedRowHandle)

                For Each ctrl In LC1.Controls
                    If ctrl.name.tolower = "txtCtnPcs".ToLower Then
                        If x.txtSatuanMBarangD.Text.Length >= 6 AndAlso x.txtSatuanMBarangD.Text.Trim.Substring(0, 6) = "KARTON" Then
                            ctrl.editvalue = NullToDbl(x.KonversiCTN)
                            clcCtnPcs_LostFocus(ctrl, Nothing)
                        End If
                        Exit For
                    End If
                Next
            End If
            x.Dispose()
            'End If

        Catch ex As Exception
            XtraMessageBox.Show("Untuk mengedit pilih item satuan yang akan diedit terlebih dahulu lalu tekan tombol edit", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally

        End Try
    End Sub
    Sub EditPaket()
        Dim x As frmSimpleEntri
        'Dim Brg As clsBarang
        Dim FormEntriName As String = "EntriBarangDPaket"
        Dim view As ColumnView = GridControl2.FocusedView
        Try
            Dim row As System.Data.DataRow = view.GetDataRow(GridView2.FocusedRowHandle)
            Dim dc As Integer = GridView2.FocusedRowHandle
            Dim IDDetil As Long = NullToLong(row("NoID"))

            x = New frmSimpleEntri
            x.FormName = FormEntriName
            x.isNew = False
            x.NoID = IDDetil
            x.IDParent = NoID
            If x.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
                RefreshStock()
                GridView2.ClearSelection()
                GridView2.FocusedRowHandle = GridView2.LocateByDisplayText(0, GridView2.Columns("NoID"), NoID.ToString("#,##0"))
                GridView2.SelectRow(GridView2.FocusedRowHandle)
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
        If DefTipeStock = DefTipeStock_.LihatStock Then
            mnTambah.Enabled = False
            mnEditSatuan.Enabled = False
            mnHapusSatuan.Enabled = False
            btnBaru.Enabled = False
            btnEdit.Enabled = False
            btnHapus.Enabled = False
            SimpleButton1.Enabled = False
        End If
    End Sub

    Private Sub SimpleButton2_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SimpleButton2.Click
        Switch(True)
    End Sub
    Sub Switch(ByVal rubahtab As Boolean)
        'If rubahtab Then
        '    XtraTabControl1.SelectedTabPageIndex = (XtraTabControl1.SelectedTabPageIndex + 1) Mod XtraTabControl1.TabPages.Count
        '    'If XtraTabControl1.SelectedTabPageIndex = 1 Then
        'End If
        RefreshStock()
        'End If
    End Sub
    Sub RefreshStock()
        Dim cn As New SqlConnection(StrKonSql)
        Dim ocmd2 As New SqlCommand
        Dim SQL As String = ""
        Try
            ocmd2.Connection = cn
            ocmd2.CommandType = CommandType.Text
            ocmd2.CommandText = "select * from " & "msatuan"
            cn.Open()
            oda2 = New SqlDataAdapter(ocmd2)
            dsGrid.Clear()
            oda2.Fill(dsGrid, "Satuan")
            'RepositoryItemGridLookUpEdit1.DataSource = dsGrid.Tables("Satuan")
            If DefTipeStock = DefTipeStock_.Penuh Then
                SQL = "Select MBarangD.*, MSatuan.Nama AS Satuan, MBarang.DiscMemberProsen2 AS [Disc Member (%)], MBarang.DiscMemberRp2 AS NilaiDiskonMember, MBarang.HargaNettoMember FROM (MBarangD INNER JOIN MBarang ON MBarang.NoID=MBarangD.IDBarang) LEFT JOIN MSatuan ON MSatuan.NoID=MBarangD.IDSatuan where   MBarangD.IDBarang=" & NoID & IIf(ckAll.Checked, "", " and MBarangD.IsActive=1 ")
            Else
                SQL = "SELECT MBarangD.NoID, MBarangD.Konversi, MBarangD.IDBarang, MBarangD.IDSatuan, MBarangD.IsSatuanBase, MBarangD.IsActive, MBarangD.IsBeli, MBarangD.IsJual, MBarangD.IsJualPOS , MSatuan.Nama AS Satuan from (MBarangD INNER JOIN MBarang ON MBarang.NoID=MBarangD.IDBarang) LEFT JOIN MSatuan ON MSatuan.NoID=MBarangD.IDSatuan where MSatuan.IsActive=1 " & IIf(ckAll.Checked, "", " AND MBarangD.IsActive=1 ") & " AND MBarangD.IDBarang=" & NoID
            End If
            'If DefTipeStock = DefTipeStock_.LihatStock Then
            '    SQL &= " AND MBarangD.Konversi=1 "
            'End If
            ocmd2.CommandText = SQL
            oda2 = New SqlDataAdapter(ocmd2)
            oda2.Fill(dsGrid, "MBarangD")

            GC1.DataSource = Nothing
            GC1.DataSource = dsGrid.Tables("MBarangD")

            If XtraTabPage2.PageVisible Then
                SQL = "SELECT (CASE WHEN MGudang.IsBS=1 THEN 'Gudang BS' ELSE 'Gudang Non BS' END) AS Status, MGudang.Nama AS Gudang, MWilayah.Nama AS Wilayah, SUM((MKartustok.QTYMasuk*MKartustok.Konversi)-(MKartustok.QTYKeluar*MKartustok.Konversi)) AS QtyStok"
                SQL &= " FROM MKartuStok LEFT JOIN (MGudang LEFT JOIN MWilayah ON MWilayah.NoID=MGudang.IDWilayah) ON MGudang.NoID=MKartuStok.IDGudang "
                SQL &= " LEFT JOIN MSatuan ON MSatuan.NoID=MKartuStok.IDSatuan "
                SQL &= " WHERE MKartustok.Tanggal<'" & Format(DateAdd(DateInterval.Day, 1, TanggalSystem), "yyyy-MM-dd") & "' AND MKartuStok.IDBarang=" & NoID
                If DefTipeStock = DefTipeStock_.BongkarBarang Then
                    SQL &= " AND (MGudang.NoID=" & DefIDGudang & ")"
                Else
                    SQL &= " AND MGudang.NoID IN (SELECT MUserDAksesGudang.IDGudang FROM MUserDAksesGudang WHERE MUserDAksesGudang.IDUser=" & IDUserAktif & ")"
                End If
                SQL &= " GROUP BY MGudang.Nama, MWilayah.Nama, MGudang.IsBS "

                ocmd2.CommandText = SQL
                oda2 = New SqlDataAdapter(ocmd2)
                oda2.Fill(dsGrid, "MKartuStok")

                GridControl1.DataSource = Nothing
                GridControl1.DataSource = dsGrid.Tables("MKartuStok")
            End If


            SQL = "SELECT MbarangDPaket.NoID,MbarangDPaket.IDBarang,MbarangDPaket.IDBarangD,MBarang.Kode,Mbarang.Nama,MBarangd.Barcode,MbarangDPaket.Qty,MSatuan.Kode " & _
                  "from MBarangDPaket inner join MbarangD on MbarangDPaket.IDBarangD=MbarangD.NoID " & _
                  "inner join MBarang On MBarangD.IDBarang=MBarang.NoID " & _
                  "left join MSatuan On MBarangD.IDSatuan=MSatuan.NoID " & _
                  "where MbarangDPaket.IDBarang=" & NoID
            ocmd2.CommandText = SQL
            oda2 = New SqlDataAdapter(ocmd2)
            oda2.Fill(dsGrid, "MBarangDPaket") 
            GridControl2.DataSource = Nothing
            GridControl2.DataSource = dsGrid.Tables("MBarangDPaket")
            If dsGrid.Tables("MBarangDPaket").Rows.Count >= 1 Then
                ckBarangPaket.Checked = True
                EksekusiSQL("UPDATE MBarang SET IsBarangPaket=1 WHERE NoID=" & NoID)
            Else
                ckBarangPaket.Checked = False
                EksekusiSQL("UPDATE MBarang SET IsBarangPaket=0 WHERE NoID=" & NoID)
            End If
            ocmd2.Dispose()

            cn.Close()
            cn.Dispose()
            With GV1
                For i As Integer = 0 To .Columns.Count - 1
                    Select Case .Columns(i).ColumnType.Name.ToLower
                        Case "int32", "int64", "int"
                            .Columns(i).DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
                            .Columns(i).DisplayFormat.FormatString = "n0"
                        Case 2
                        Case "decimal", "single", "money", "double"
                            .Columns(i).DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
                            .Columns(i).DisplayFormat.FormatString = "n2"
                        Case "string"
                            .Columns(i).DisplayFormat.FormatType = DevExpress.Utils.FormatType.None
                            .Columns(i).DisplayFormat.FormatString = ""
                        Case "date"
                            GV1.Columns(i).DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime
                            GV1.Columns(i).DisplayFormat.FormatString = "dd-MM-yyyy"
                        Case "datetime"
                            GV1.Columns(i).DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime
                            GV1.Columns(i).DisplayFormat.FormatString = "dd-MM-yyyy HH:mm"
                        Case "boolean"
                            .Columns(i).ColumnEdit = repckedit
                    End Select
                    If GV1.Columns(i).FieldName.Length >= 4 AndAlso GV1.Columns(i).FieldName.Substring(0, 4).ToLower = "Kode".ToLower Then
                        GV1.Columns(i).Fixed = Columns.FixedStyle.Left
                    ElseIf GV1.Columns(i).FieldName.ToLower = "Nama".ToLower Then
                        GV1.Columns(i).Fixed = Columns.FixedStyle.Left
                    End If
                Next
            End With
            With GridView1
                For i As Integer = 0 To .Columns.Count - 1
                    Select Case .Columns(i).ColumnType.Name.ToLower
                        Case "int32", "int64", "int"
                            .Columns(i).DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
                            .Columns(i).DisplayFormat.FormatString = "n0"
                        Case 2
                        Case "decimal", "single", "money", "double"
                            .Columns(i).DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
                            .Columns(i).DisplayFormat.FormatString = "n2"
                        Case "string"
                            .Columns(i).DisplayFormat.FormatType = DevExpress.Utils.FormatType.None
                            .Columns(i).DisplayFormat.FormatString = ""
                        Case "date"
                            GV1.Columns(i).DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime
                            GV1.Columns(i).DisplayFormat.FormatString = "dd-MM-yyyy"
                        Case "datetime"
                            GV1.Columns(i).DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime
                            GV1.Columns(i).DisplayFormat.FormatString = "dd-MM-yyyy HH:mm"
                        Case "boolean"
                            .Columns(i).ColumnEdit = repckedit
                    End Select
                    If GV1.Columns(i).FieldName.Length >= 4 AndAlso GV1.Columns(i).FieldName.Substring(0, 4).ToLower = "Kode".ToLower Then
                        GV1.Columns(i).Fixed = Columns.FixedStyle.Left
                    ElseIf GV1.Columns(i).FieldName.ToLower = "Nama".ToLower Then
                        GV1.Columns(i).Fixed = Columns.FixedStyle.Left
                    End If
                Next
            End With
            'HitungHarga()
        Catch ex As Exception
            DevExpress.XtraEditors.XtraMessageBox.Show("Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            ocmd2.Dispose()
            cn.Close()
            cn.Dispose()
        End Try
    End Sub
    Private Sub BarButtonItem3_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem3.ItemClick
        SimpleButton2.PerformClick()
    End Sub

    Private Sub BarButtonItem4_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem4.ItemClick
        Dim xOtorisasi As New frmOtorisasiAdmin
        If XtraMessageBox.Show("Simpan Layout?", NamaAplikasi, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = Windows.Forms.DialogResult.Yes AndAlso xOtorisasi.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
            Ini.TulisIniPath(FolderLayouts & FormName & TipeEntri.ToString & ".ini", "Form", "Width", Me.Width)
            Ini.TulisIniPath(FolderLayouts & FormName & TipeEntri.ToString & ".ini", "Form", "Height", Me.Height)
            LC1.SaveLayoutToXml(FolderLayouts & FormName & TipeEntri.ToString & ".xml")
            GV1.SaveLayoutToXml(FolderLayouts & FormName & TipeEntri.ToString & "Barang Detail.xml")
            GridView1.SaveLayoutToXml(FolderLayouts & FormName & TipeEntri.ToString & "Stock Barang Detail.xml")

            'If XtraMessageBox.Show("Simpan layouts juga sebagai default untuk akses " & DefTipeStock.ToString & "?", NamaAplikasi, MessageBoxButtons.YesNo, MessageBoxIcon.Question) Then
            LC1.SaveLayoutToXml(FolderLayouts & FormName & TipeEntri.ToString & DefTipeStock.ToString & ".xml")
            GV1.SaveLayoutToXml(FolderLayouts & FormName & TipeEntri.ToString & DefTipeStock.ToString & "Barang Detail.xml")
            GridView1.SaveLayoutToXml(FolderLayouts & FormName & TipeEntri.ToString & DefTipeStock.ToString & "Stock Barang Detail.xml")
            'End If

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
                ''lu.Dispose()
            End Try
        End If
        xOtorisasi.Dispose()
    End Sub
    Sub HitungHarga(Optional ByVal UpdateHarga As Boolean = False)
        'KHUSUS CITY TOYS
        'Try
        Dim ctrlIDTypePajak As DevExpress.XtraEditors.SearchLookUpEdit = Nothing
        Dim ctrlHargaBeli As DevExpress.XtraEditors.CalcEdit = Nothing
        Dim ctrlDiscBeli1 As DevExpress.XtraEditors.CalcEdit = Nothing
        Dim ctrlDiscBeli2 As DevExpress.XtraEditors.CalcEdit = Nothing
        Dim ctrlDiscBeli3 As DevExpress.XtraEditors.CalcEdit = Nothing
        Dim ctrlDiscBeliRp As DevExpress.XtraEditors.CalcEdit = Nothing
        Dim ctrlHargaBeliPcs As DevExpress.XtraEditors.CalcEdit = Nothing
        Dim ctrlIDSatuan As DevExpress.XtraEditors.SearchLookUpEdit = Nothing
        Dim ctrlIsi1Dus As DevExpress.XtraEditors.CalcEdit = Nothing

        Dim ctrlprosenup As DevExpress.XtraEditors.CalcEdit = Nothing
        Dim ctrlHargaJual As DevExpress.XtraEditors.CalcEdit = Nothing
        Dim ctrlprosenup2 As DevExpress.XtraEditors.CalcEdit = Nothing
        Dim ctrlHargaJual2 As DevExpress.XtraEditors.CalcEdit = Nothing
        Dim ctrlprosenup3 As DevExpress.XtraEditors.CalcEdit = Nothing
        Dim ctrlHargaJual3 As DevExpress.XtraEditors.CalcEdit = Nothing
        Dim ctrlHargaPDP As DevExpress.XtraEditors.CalcEdit = Nothing
        Dim ctrlDiscPDP As DevExpress.XtraEditors.CalcEdit = Nothing
        Dim ctrlHargaPDPMember As DevExpress.XtraEditors.CalcEdit = Nothing
        Dim ctrlDiscPDPMember As DevExpress.XtraEditors.CalcEdit = Nothing
        '    Dim ctrlHargaE As DevExpress.XtraEditors.CalcEdit = Nothing
        '    Dim ctrlHargaF As DevExpress.XtraEditors.CalcEdit = Nothing
        '    Dim ctrlHargaMinimumA As DevExpress.XtraEditors.CalcEdit = Nothing
        '    Dim ctrlHargaMinimumB As DevExpress.XtraEditors.CalcEdit = Nothing
        '    Dim ctrlHargaMinimumC As DevExpress.XtraEditors.CalcEdit = Nothing
        '    Dim ctrlHargaMinimumD As DevExpress.XtraEditors.CalcEdit = Nothing
        '    Dim ctrlHargaMinimumE As DevExpress.XtraEditors.CalcEdit = Nothing
        '    Dim ctrlHargaMinimumF As DevExpress.XtraEditors.CalcEdit = Nothing

        '    Dim ctrlKonversiHarga As DevExpress.XtraEditors.CalcEdit = Nothing
        Dim isPPN As DevExpress.XtraEditors.CheckEdit = Nothing

        For Each ctrl In LC1.Controls
            If ctrl.name.ToString.ToLower = "txthargabeli" Then
                ctrlHargaBeli = ctrl
            ElseIf ctrl.name.ToString.ToLower = "txtdiscbeli1" Then
                ctrlDiscBeli1 = ctrl
            ElseIf ctrl.name.ToString.ToLower = "txtdiscbeli2" Then
                ctrlDiscBeli2 = ctrl
            ElseIf ctrl.name.ToString.ToLower = "txtdiscbeli3" Then
                ctrlDiscBeli3 = ctrl
            ElseIf ctrl.name.ToString.ToLower = "txtdiscrp" Then
                ctrlDiscBeliRp = ctrl
            ElseIf ctrl.name.ToString.ToLower = "txthargabelipcs" Then
                ctrlHargaBeliPcs = ctrl
            ElseIf ctrl.name.ToString.ToLower = "txtctnpcs" Then
                ctrlIsi1Dus = ctrl
            ElseIf ctrl.name.ToString.ToLower = "txtprosenup" Then
                ctrlprosenup = ctrl
            ElseIf ctrl.name.ToString.ToLower = "txthargajual" Then
                ctrlHargaJual = ctrl
            ElseIf ctrl.name.ToString.ToLower = "txtprosenup2" Then
                ctrlprosenup2 = ctrl
            ElseIf ctrl.name.ToString.ToLower = "txthargajual2" Then
                ctrlHargaJual2 = ctrl
            ElseIf ctrl.name.ToString.ToLower = "txtprosenup3" Then
                ctrlprosenup3 = ctrl
            ElseIf ctrl.name.ToString.ToLower = "txthargajual3" Then
                ctrlHargaJual3 = ctrl
            ElseIf ctrl.name.ToString.ToLower = "txthargapdp" Then
                ctrlHargaPDP = ctrl
            ElseIf ctrl.name.ToString.ToLower = "txtdiscpdp" Then
                ctrlDiscPDP = ctrl
            ElseIf ctrl.name.ToString.ToLower = "txthargapdpmember" Then
                ctrlHargaPDPMember = ctrl
            ElseIf ctrl.name.ToString.ToLower = "txtdiscpdpmember" Then
                ctrlDiscPDPMember = ctrl
            ElseIf ctrl.name.ToString.ToLower = "txtisppn" Then
                isPPN = ctrl
            ElseIf ctrl.name.ToString.ToLower = "txtidtypepajak" Then
                ctrlIDTypePajak = ctrl
            ElseIf ctrl.name.ToString.ToLower = "txtidsatuan" Then
                ctrlIDSatuan = ctrl
            End If
            If ctrlHargaBeli Is Nothing Or ctrlDiscBeli1 Is Nothing Or _
            ctrlDiscBeli2 Is Nothing Or ctrlDiscBeli3 Is Nothing Or ctrlHargaBeliPcs Is Nothing _
            Or ctrlDiscBeliRp Is Nothing Or ctrlprosenup Is Nothing Or ctrlHargaJual Is Nothing Or _
            ctrlprosenup2 Is Nothing Or ctrlHargaJual3 Is Nothing Or _
            ctrlprosenup3 Is Nothing Or ctrlHargaJual3 Is Nothing Or _
            ctrlIsi1Dus Is Nothing Or isPPN Is Nothing Or ctrlIDTypePajak Is Nothing Or ctrlIDSatuan Is Nothing Or _
            ctrlHargaPDP Is Nothing Or ctrlDiscPDP Is Nothing Or ctrlHargaPDPMember Is Nothing Or ctrlDiscPDPMember Is Nothing Then
            Else
                Exit For 'semua yg dicari sudah ketemu
            End If
        Next
        If ctrlHargaBeli Is Nothing Then
            XtraMessageBox.Show("Object:ctrlHargaBeli  masih ada yg belum didefinisikan!", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        ElseIf ctrlDiscBeli1 Is Nothing Then
            XtraMessageBox.Show("Object:ctrlDiscBeli1  masih ada yg belum didefinisikan!", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        ElseIf ctrlDiscBeli2 Is Nothing Then
            XtraMessageBox.Show("Object:ctrlDiscBeli2  masih ada yg belum didefinisikan!", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        ElseIf ctrlDiscBeli3 Is Nothing Then
            XtraMessageBox.Show("Object:ctrlDiscBeli3  masih ada yg belum didefinisikan!", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        ElseIf ctrlDiscBeliRp Is Nothing Then
            XtraMessageBox.Show("Object:ctrlDiscBeliRp  masih ada yg belum didefinisikan!", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        ElseIf ctrlHargaBeliPcs Is Nothing Then
            XtraMessageBox.Show("Object:ctrlHargaBeliPcs  masih ada yg belum didefinisikan!", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        ElseIf ctrlprosenup Is Nothing Then
            XtraMessageBox.Show("Object:ctrlprosenup  masih ada yg belum didefinisikan!", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        ElseIf ctrlHargaJual Is Nothing Then
            XtraMessageBox.Show("Object:ctrlHargaJual  masih ada yg belum didefinisikan!", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        ElseIf ctrlprosenup2 Is Nothing Then
            XtraMessageBox.Show("Object:ctrlprosenup2  masih ada yg belum didefinisikan!", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        ElseIf ctrlHargaJual2 Is Nothing Then
            XtraMessageBox.Show("Object:ctrlHargaJual2  masih ada yg belum didefinisikan!", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        ElseIf ctrlprosenup3 Is Nothing Then
            XtraMessageBox.Show("Object:ctrlprosenup3  masih ada yg belum didefinisikan!", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        ElseIf ctrlHargaJual3 Is Nothing Then
            XtraMessageBox.Show("Object:ctrlHargaJual3  masih ada yg belum didefinisikan!", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        ElseIf ctrlIsi1Dus Is Nothing Then
            XtraMessageBox.Show("Object:ctrlIsi1Dus  masih ada yg belum didefinisikan!", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        ElseIf ctrlIDTypePajak Is Nothing Then
            XtraMessageBox.Show("Object:IDTypePajak  masih ada yg belum didefinisikan!", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        ElseIf ctrlIDSatuan Is Nothing Then
            XtraMessageBox.Show("Object:IDSatuan masih ada yg belum didefinisikan!", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        ElseIf ctrlHargaPDP Is Nothing Then
            XtraMessageBox.Show("Object:IDSatuan masih ada yg belum didefinisikan!", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        ElseIf ctrlHargaPDP Is Nothing Then
            XtraMessageBox.Show("Object:IDSatuan masih ada yg belum didefinisikan!", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        ElseIf ctrlHargaPDPMember Is Nothing Then
            XtraMessageBox.Show("Object:IDSatuan masih ada yg belum didefinisikan!", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        ElseIf ctrlHargaPDPMember Is Nothing Then
            XtraMessageBox.Show("Object:IDSatuan masih ada yg belum didefinisikan!", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        ElseIf isPPN Is Nothing Then
            XtraMessageBox.Show("Object:isPPN  masih ada yg belum didefinisikan!", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
            btnBaru.Focus()
        Else
            isPPN.Checked = If(NullTolInt(ctrlIDTypePajak.EditValue) = 0, False, True)
            If isPPN.Checked Then
                If NullTolInt(ctrlIDTypePajak.EditValue) = 2 Then
                    ctrlHargaBeliPcs.EditValue = 1.1 * ((NullToDbl(ctrlHargaBeli.EditValue) * (100 - NullToDbl(ctrlDiscBeli1.EditValue)) * (100 - NullToDbl(ctrlDiscBeli2.EditValue)) * (100 - NullToDbl(ctrlDiscBeli3.EditValue)) / 1000000 - NullToDbl(ctrlDiscBeliRp.EditValue)) / IIf(NullToDbl(ctrlIsi1Dus.EditValue) = 0, 1, NullToDbl(ctrlIsi1Dus.EditValue)))
                Else
                    ctrlHargaBeliPcs.EditValue = 1.0 * ((NullToDbl(ctrlHargaBeli.EditValue) * (100 - NullToDbl(ctrlDiscBeli1.EditValue)) * (100 - NullToDbl(ctrlDiscBeli2.EditValue)) * (100 - NullToDbl(ctrlDiscBeli3.EditValue)) / 1000000 - NullToDbl(ctrlDiscBeliRp.EditValue)) / IIf(NullToDbl(ctrlIsi1Dus.EditValue) = 0, 1, NullToDbl(ctrlIsi1Dus.EditValue)))
                End If

            Else
                ctrlHargaBeliPcs.EditValue = 1.0 * ((NullToDbl(ctrlHargaBeli.EditValue) * (100 - NullToDbl(ctrlDiscBeli1.EditValue)) * (100 - NullToDbl(ctrlDiscBeli2.EditValue)) * (100 - NullToDbl(ctrlDiscBeli3.EditValue) - NullToDbl(ctrlDiscBeliRp.EditValue)) / 1000000) / IIf(NullToDbl(ctrlIsi1Dus.EditValue) = 0, 1, NullToDbl(ctrlIsi1Dus.EditValue)))
            End If
            'ctrlprosenup.EditValue = 100 * (ctrlHargaJual.EditValue - ctrlHargaBeliPcs.EditValue) / IIf(abs(ctrlHargaBeliPcs.EditValue) < 1, 100000000000000, ctrlHargaBeliPcs.EditValue)
            'ctrlHargaJual.EditValue = NullToDbl(ctrlHargaBeliPcs.EditValue) * (1 + NullToDbl(ctrlprosenup.EditValue) / 100)
            ctrlprosenup.EditValue = 100 * (NullToDbl(ctrlHargaJual.EditValue) - NullToDbl(ctrlHargaBeliPcs.EditValue)) / IIf(Math.Abs(ctrlHargaBeliPcs.EditValue) < 1, 100000000000000, ctrlHargaBeliPcs.EditValue)
            ctrlprosenup2.EditValue = 100 * (NullToDbl(ctrlHargaJual2.EditValue) - NullToDbl(ctrlHargaBeliPcs.EditValue)) / IIf(Math.Abs(ctrlHargaBeliPcs.EditValue) < 1, 100000000000000, ctrlHargaBeliPcs.EditValue)
            ctrlprosenup3.EditValue = 100 * (NullToDbl(ctrlHargaJual3.EditValue) - NullToDbl(ctrlHargaBeliPcs.EditValue)) / IIf(Math.Abs(ctrlHargaBeliPcs.EditValue) < 1, 100000000000000, ctrlHargaBeliPcs.EditValue)
            If NullToDbl(ctrlHargaPDP.EditValue) <= 0 Then
                ctrlDiscPDP.EditValue = 0
            Else
                ctrlDiscPDP.EditValue = NullToDbl(ctrlDiscPDP.EditValue)
            End If
            ctrlHargaPDP.EditValue = NullToDbl(ctrlHargaJual.EditValue) - NullToDbl(ctrlDiscPDP.EditValue)
            If NullToDbl(ctrlHargaPDPMember.EditValue) <= 0 Then
                ctrlDiscPDPMember.EditValue = 0
            Else
                ctrlDiscPDPMember.EditValue = NullToDbl(ctrlDiscPDPMember.EditValue)
            End If
            ctrlHargaPDPMember.EditValue = NullToDbl(ctrlHargaJual.EditValue) - NullToDbl(ctrlDiscPDPMember.EditValue)
        End If
        '        If NullToDbl(ctrlKonversiHarga.EditValue) = 0 Then Exit Sub
        '        If ishargajualmanual.Checked = False Then
        '            ctrlHargaA.Properties.ReadOnly = True
        '            ctrlHargaB.Properties.ReadOnly = True
        '            ctrlHargaC.Properties.ReadOnly = True
        '            ctrlHargaD.Properties.ReadOnly = True
        '            'ctrlHargaE.Properties.ReadOnly = True
        '            'ctrlHargaF.Properties.ReadOnly = True
        '            If NullToLong(ctrlprosen.EditValue) > 0 Then 'Prosen diisi
        '                ctrlHargaA.EditValue = BulatkanUp((NullToDbl(ctrlHargaPasar.EditValue / ctrlKonversiHarga.EditValue) * (100 - (NullToDbl(ctrlprosen.EditValue) - 5)) / 100), 25) * ctrlKonversiHarga.EditValue
        '                If NullToLong(ctrlprosen.EditValue) > 5 Then
        '                    ctrlHargaB.EditValue = BulatkanUp(NullToDbl(ctrlHargaPasar.EditValue / ctrlKonversiHarga.EditValue) * (100 - (NullToDbl(ctrlprosen.EditValue) - 10)) / 100, 25) * ctrlKonversiHarga.EditValue

        '                Else
        '                    ctrlHargaB.EditValue = BulatkanUp(NullToDbl(ctrlHargaPasar.EditValue / ctrlKonversiHarga.EditValue) * 100 / 90, 25) * ctrlKonversiHarga.EditValue
        '                End If
        '            Else
        '                ctrlHargaA.EditValue = BulatkanUp(NullToDbl(ctrlHargaPasar.EditValue / ctrlKonversiHarga.EditValue) * 100 / 95, 25) * ctrlKonversiHarga.EditValue
        '                ctrlHargaB.EditValue = BulatkanUp(NullToDbl(ctrlHargaPasar.EditValue / ctrlKonversiHarga.EditValue) * 100 / 90, 25) * ctrlKonversiHarga.EditValue
        '            End If
        '            ctrlHargaC.EditValue = BulatkanUp(ctrlHargaB.EditValue / ctrlKonversiHarga.EditValue * 100 / 80, 25) * ctrlKonversiHarga.EditValue
        '            ctrlHargaD.EditValue = BulatkanUp(ctrlHargaB.EditValue / ctrlKonversiHarga.EditValue * 100 / 80, 1000) * ctrlKonversiHarga.EditValue
        '            ctrlHargaE.EditValue = BulatkanUp(ctrlHargaB.EditValue / ctrlKonversiHarga.EditValue * 100 / 80, 1000) * ctrlKonversiHarga.EditValue
        '            ctrlHargaF.EditValue = BulatkanUp(ctrlHargaB.EditValue / ctrlKonversiHarga.EditValue * 100 / 80, 1000) * ctrlKonversiHarga.EditValue

        '        Else
        '            ctrlHargaA.Properties.ReadOnly = False
        '            ctrlHargaB.Properties.ReadOnly = False
        '            ctrlHargaC.Properties.ReadOnly = False
        '            ctrlHargaD.Properties.ReadOnly = False
        '            'ctrlHargaE.Properties.ReadOnly = False
        '            'ctrlHargaF.Properties.ReadOnly = False
        '        End If
        '    End If
        Try
            Dim konversi As Double = 0
            Dim IDSatuan As Integer
            For i = 0 To GV1.RowCount - 1
                IDSatuan = NullToDbl(GV1.GetRowCellValue(i, "IDSatuan"))
                If IDSatuan = NullTolInt(ctrlIDSatuan.EditValue) Then
                    If UpdateHarga Then
                        EksekusiSQL("Update MBarangD Set HargaJual=" & FixKoma(Bulatkan(NullToDbl(ctrlHargaJual.EditValue), 0)) & _
                                    ",ProsenUp=" & FixKoma(NullToDbl(ctrlprosenup.EditValue)) & _
                                    ",HargaJual2=" & FixKoma(Bulatkan(NullToDbl(ctrlHargaJual2.EditValue), 0)) & _
                                    ",ProsenUp2=" & FixKoma(NullToDbl(ctrlprosenup2.EditValue)) & _
                                    ",HargaJual3=" & FixKoma(Bulatkan(NullToDbl(ctrlHargaJual3.EditValue), 0)) & _
                                    ",ProsenUp3=" & FixKoma(NullToDbl(ctrlprosenup3.EditValue)) & _
                                    ",HargaNetto=" & FixKoma(Bulatkan(NullToDbl(ctrlHargaJual.EditValue), 0) - Bulatkan(NullToDbl(GV1.GetRowCellValue(i, "NilaiDiskon")), 0)) & " where NoID=" & NullTolInt(GV1.GetRowCellValue(i, "NoID")))
                    End If
                    GV1.SetRowCellValue(i, "ProsenUp", NullToDbl(ctrlprosenup.EditValue))
                    GV1.SetRowCellValue(i, "HargaJual", Bulatkan(NullToDbl(ctrlHargaJual.EditValue), 0))
                    GV1.SetRowCellValue(i, "ProsenUp2", NullToDbl(ctrlprosenup2.EditValue))
                    GV1.SetRowCellValue(i, "HargaJual2", Bulatkan(NullToDbl(ctrlHargaJual2.EditValue), 0))
                    GV1.SetRowCellValue(i, "ProsenUp3", NullToDbl(ctrlprosenup3.EditValue))
                    GV1.SetRowCellValue(i, "HargaJual3", Bulatkan(NullToDbl(ctrlHargaJual3.EditValue), 0))
                    GV1.SetRowCellValue(i, "HargaNetto", Bulatkan(NullToDbl(ctrlHargaJual.EditValue), 0) - Bulatkan(NullToDbl(GV1.GetRowCellValue(i, "NilaiDiskon")), 0))
                End If
                'konversi = NullToDbl(GV1.GetRowCellValue(i, "Konversi"))
                'If NullToDbl(ctrlKonversiHarga.EditValue) <= 0 Then
                '    ctrlKonversiHarga.EditValue = 1
                'End If
                'Harga Jual
                'GV1.SetRowCellValue(i, "HargaJualA", BulatkanUp(NullToDbl(ctrlHargaA.EditValue) * konversi / NullToDbl(ctrlKonversiHarga.EditValue), 25))
                'GV1.SetRowCellValue(i, "HargaJualB", BulatkanUp(NullToDbl(ctrlHargaB.EditValue) * konversi / NullToDbl(ctrlKonversiHarga.EditValue), 25))
                'GV1.SetRowCellValue(i, "HargaJualC", BulatkanUp(NullToDbl(ctrlHargaC.EditValue) * konversi / NullToDbl(ctrlKonversiHarga.EditValue), 25))
                'GV1.SetRowCellValue(i, "HargaJualD", BulatkanUp(NullToDbl(ctrlHargaD.EditValue) * konversi / NullToDbl(ctrlKonversiHarga.EditValue), 100))
                'GV1.SetRowCellValue(i, "HargaJualE", BulatkanUp(NullToDbl(ctrlHargaE.EditValue) * konversi / NullToDbl(ctrlKonversiHarga.EditValue), 25))
                'GV1.SetRowCellValue(i, "HargaJualF", BulatkanUp(NullToDbl(ctrlHargaF.EditValue) * konversi / NullToDbl(ctrlKonversiHarga.EditValue), 25))

                ''Harga Minimum
                'GV1.SetRowCellValue(i, "HargaMinA", BulatkanUp(NullToDbl(ctrlHargaMinimumA.EditValue) * konversi / NullToDbl(ctrlKonversiHarga.EditValue), 25))
                'GV1.SetRowCellValue(i, "HargaMinB", BulatkanUp(NullToDbl(ctrlHargaMinimumB.EditValue) * konversi / NullToDbl(ctrlKonversiHarga.EditValue), 25))
                'GV1.SetRowCellValue(i, "HargaMinC", BulatkanUp(NullToDbl(ctrlHargaMinimumC.EditValue) * konversi / NullToDbl(ctrlKonversiHarga.EditValue), 25))
                'GV1.SetRowCellValue(i, "HargaMinD", BulatkanUp(NullToDbl(ctrlHargaMinimumD.EditValue) * konversi / NullToDbl(ctrlKonversiHarga.EditValue), 100))
                'GV1.SetRowCellValue(i, "HargaMinE", BulatkanUp(NullToDbl(ctrlHargaMinimumE.EditValue) * konversi / NullToDbl(ctrlKonversiHarga.EditValue), 25))
                'GV1.SetRowCellValue(i, "HargaMinF", BulatkanUp(NullToDbl(ctrlHargaMinimumF.EditValue) * konversi / NullToDbl(ctrlKonversiHarga.EditValue), 25))

            Next
        Catch ex As Exception
            XtraMessageBox.Show(ex.Message)
        End Try

    End Sub
    Sub HitungHargaJualByMargin(Optional ByVal UpdateHarga As Boolean = False)
        Dim ctrlHargaBeliPcs As DevExpress.XtraEditors.CalcEdit = Nothing
        Dim ctrlprosenup As DevExpress.XtraEditors.CalcEdit = Nothing
        Dim ctrlIDSatuan As DevExpress.XtraEditors.SearchLookUpEdit = Nothing
        Dim ctrlHargaJual As DevExpress.XtraEditors.CalcEdit = Nothing
        Dim ctrlQty1 As DevExpress.XtraEditors.CalcEdit = Nothing

        For Each ctrl In LC1.Controls
            If ctrl.name.ToString.ToLower = "txthargabelipcs" Then
                ctrlHargaBeliPcs = ctrl
            ElseIf ctrl.name.ToString.ToLower = "txtprosenup" Then
                ctrlprosenup = ctrl
            ElseIf ctrl.name.ToString.ToLower = "txthargajual" Then
                ctrlHargaJual = ctrl
            ElseIf ctrl.name.ToString.ToLower = "txtqty1" Then
                ctrlQty1 = ctrl
            ElseIf ctrl.name.ToString.ToLower = "txtidsatuan" Then
                ctrlIDSatuan = ctrl
            End If
            If ctrlprosenup Is Nothing Or ctrlHargaBeliPcs Is Nothing _
            Or ctrlHargaJual Is Nothing Or ctrlIDSatuan Is Nothing Or ctrlQty1 Is Nothing Then
            Else
                Exit For 'semua yg dicari sudah ketemu
            End If
        Next
        If ctrlprosenup Is Nothing Then
            XtraMessageBox.Show("Object:ctrlprosenup  masih ada yg belum didefinisikan!", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        ElseIf ctrlHargaBeliPcs Is Nothing Then
            XtraMessageBox.Show("Object:ctrlHargaBeliPcs  masih ada yg belum didefinisikan!", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        ElseIf ctrlHargaJual Is Nothing Then
            XtraMessageBox.Show("Object:ctrlHargaJual  masih ada yg belum didefinisikan!", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        ElseIf ctrlIDSatuan Is Nothing Then
            XtraMessageBox.Show("Object:ctrlIDSatuan  masih ada yg belum didefinisikan!", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        ElseIf ctrlQty1 Is Nothing Then
            XtraMessageBox.Show("Object:ctrlQty1  masih ada yg belum didefinisikan!", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        Else
            ctrlHargaJual.EditValue = Bulatkan((1 + ctrlprosenup.EditValue / 100) * ctrlHargaBeliPcs.EditValue, 0)
        End If
        Try
            Dim konversi As Double = 0
            Dim IDSatuan As Integer
            For i = 0 To GV1.RowCount - 1
                IDSatuan = NullToDbl(GV1.GetRowCellValue(i, "IDSatuan"))
                If IDSatuan = NullTolInt(ctrlIDSatuan.EditValue) Then
                    If UpdateHarga Then
                        EksekusiSQL("Update MBarangD Set HargaJual=" & FixKoma(Bulatkan(NullToDbl(ctrlHargaJual.EditValue), 0)) & ",ProsenUp=" & FixKoma(NullToDbl(ctrlprosenup.EditValue)) & ",Qty1=" & FixKoma(NullToDbl(ctrlQty1.EditValue)) & " where NoID=" & NullTolInt(GV1.GetRowCellValue(i, "NoID")))
                    End If
                    GV1.SetRowCellValue(i, "HargaJual", Bulatkan(NullToDbl(ctrlHargaJual.EditValue), 0))
                    GV1.SetRowCellValue(i, "ProsenUp", NullToDbl(ctrlprosenup.EditValue))
                    GV1.SetRowCellValue(i, "Qty1", NullToDbl(ctrlQty1.EditValue))
                End If
            Next
        Catch ex As Exception
            XtraMessageBox.Show(ex.Message)
        End Try
    End Sub

    Sub HitungHargaJualPromoByProsen(Optional ByVal UpdateHarga As Boolean = False)
        Dim ctrlHargaJual As DevExpress.XtraEditors.CalcEdit = Nothing
        Dim ctrlHargaJualNetto As DevExpress.XtraEditors.CalcEdit = Nothing
        Dim ctrlDiscPromoRp As DevExpress.XtraEditors.CalcEdit = Nothing
        Dim ctrlDiscPromo As DevExpress.XtraEditors.CalcEdit = Nothing
        'Dim ctrlKonversi As DevExpress.XtraEditors.CalcEdit = Nothing

        For Each ctrl In LC1.Controls
            If ctrl.name.ToString.ToLower = "txthargajual" Then
                ctrlHargaJual = ctrl
            ElseIf ctrl.name.ToString.ToLower = "txthargajualnetto" Then
                ctrlHargaJualNetto = ctrl
            ElseIf ctrl.name.ToString.ToLower = "txtdiscpromorp" Then
                ctrlDiscPromoRp = ctrl
            ElseIf ctrl.name.ToString.ToLower = "txtdiscpromo" Then
                ctrlDiscPromo = ctrl
                'ElseIf ctrl.name.ToString.ToLower = "txtkonversi" Then
                '    ctrlKonversi = ctrl

            End If
            If ctrlHargaJual Is Nothing Or ctrlHargaJualNetto Is Nothing _
            Or ctrlDiscPromoRp Is Nothing Or ctrlDiscPromo Is Nothing Then
            Else
                Exit For 'semua yg dicari sudah ketemu
            End If
        Next
        If ctrlHargaJual Is Nothing Then
            XtraMessageBox.Show("Object:ctrlHargaJual  masih ada yg belum didefinisikan!", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        ElseIf ctrlHargaJualNetto Is Nothing Then
            XtraMessageBox.Show("Object:ctrlHargaJualNetto  masih ada yg belum didefinisikan!", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        ElseIf ctrlDiscPromoRp Is Nothing Then
            XtraMessageBox.Show("Object:ctrlDiscPromoRp  masih ada yg belum didefinisikan!", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        ElseIf ctrlDiscPromo Is Nothing Then
            XtraMessageBox.Show("Object:ctrlDiscPromo  masih ada yg belum didefinisikan!", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
            'ElseIf ctrlKonversi Is Nothing Then
            '    XtraMessageBox.Show("Object:ctrlKonversi  masih ada yg belum didefinisikan!", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        Else
            ctrlDiscPromoRp.EditValue = Bulatkan((NullToDbl(ctrlDiscPromo.EditValue) / 100) * NullToDbl(ctrlHargaJual.EditValue), 0)
            ctrlHargaJualNetto.EditValue = Bulatkan(NullToDbl(ctrlHargaJual.EditValue) - NullToDbl(ctrlDiscPromoRp.EditValue), 0)
            HitungHargaJualMemberByProsen(UpdateHarga)
            HitungHarga(UpdateHarga)
        End If
        Try
            Dim Konversi As Double = 0.0
            For i = 0 To GV1.RowCount - 1

                Konversi = NullToDbl(GV1.GetRowCellValue(i, "Konversi"))
                ' If IDSatuan = NullTolInt(ctrlIDSatuan.EditValue) Then
                If UpdateHarga Then
                    EksekusiSQL("Update MBarangD SET NilaiDiskon=" & FixKoma(Bulatkan(NullToDbl(ctrlDiscPromoRp.EditValue) * Konversi, 0)) & ", PromoDiskonJual=" & FixKoma(Bulatkan(NullToDbl(ctrlDiscPromo.EditValue), 0)) & ", HargaNetto=" & FixKoma(Bulatkan(NullToDbl(ctrlHargaJualNetto.EditValue) * Konversi, 0)) & " where NoID=" & NullTolInt(GV1.GetRowCellValue(i, "NoID")))
                End If
                GV1.SetRowCellValue(i, "HargaNetto", Bulatkan(NullToDbl(ctrlHargaJualNetto.EditValue) * Konversi, 0))
                GV1.SetRowCellValue(i, "PromoDiskonJual", Bulatkan(NullToDbl(ctrlDiscPromo.EditValue), 0))
                GV1.SetRowCellValue(i, "NilaiDiskon", Bulatkan(NullToDbl(ctrlDiscPromoRp.EditValue) * Konversi, 0))
                '  End If
            Next
        Catch ex As Exception
            XtraMessageBox.Show(ex.Message)
        End Try
    End Sub

    Sub HitungHargaJualPromoProsenByRp(Optional ByVal UpdateHarga As Boolean = False)
        Dim ctrlHargaJual As DevExpress.XtraEditors.CalcEdit = Nothing
        Dim ctrlHargaJualNetto As DevExpress.XtraEditors.CalcEdit = Nothing
        Dim ctrlDiscPromoRp As DevExpress.XtraEditors.CalcEdit = Nothing
        Dim ctrlDiscPromo As DevExpress.XtraEditors.CalcEdit = Nothing
        'Dim ctrlKonversi As DevExpress.XtraEditors.CalcEdit = Nothing

        For Each ctrl In LC1.Controls
            If ctrl.name.ToString.ToLower = "txthargajual" Then
                ctrlHargaJual = ctrl
            ElseIf ctrl.name.ToString.ToLower = "txthargajualnetto" Then
                ctrlHargaJualNetto = ctrl
            ElseIf ctrl.name.ToString.ToLower = "txtdiscpromorp" Then
                ctrlDiscPromoRp = ctrl
            ElseIf ctrl.name.ToString.ToLower = "txtdiscpromo" Then
                ctrlDiscPromo = ctrl
                'ElseIf ctrl.name.ToString.ToLower = "txtkonversi" Then
                '    ctrlKonversi = ctrl

            End If
            If ctrlHargaJual Is Nothing Or ctrlHargaJualNetto Is Nothing _
            Or ctrlDiscPromoRp Is Nothing Or ctrlDiscPromo Is Nothing Then
            Else
                Exit For 'semua yg dicari sudah ketemu
            End If
        Next
        If ctrlHargaJual Is Nothing Then
            XtraMessageBox.Show("Object:ctrlHargaJual  masih ada yg belum didefinisikan!", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        ElseIf ctrlHargaJualNetto Is Nothing Then
            XtraMessageBox.Show("Object:ctrlHargaJualNetto  masih ada yg belum didefinisikan!", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        ElseIf ctrlDiscPromoRp Is Nothing Then
            XtraMessageBox.Show("Object:ctrlDiscPromoRp  masih ada yg belum didefinisikan!", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        ElseIf ctrlDiscPromo Is Nothing Then
            XtraMessageBox.Show("Object:ctrlDiscPromo  masih ada yg belum didefinisikan!", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
            'ElseIf ctrlKonversi Is Nothing Then
            '    XtraMessageBox.Show("Object:ctrlKonversi  masih ada yg belum didefinisikan!", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        Else
            ctrlHargaJualNetto.EditValue = Bulatkan(NullToDbl(ctrlHargaJual.EditValue), 0) - Bulatkan(NullToDbl(ctrlDiscPromoRp.EditValue), 0)
            If NullToDbl(ctrlHargaJual.EditValue) = 0 Then
                ctrlDiscPromo.EditValue = 0.0
            Else
                ctrlDiscPromo.EditValue = NullToDbl(ctrlDiscPromoRp.EditValue) * 100 / NullToDbl(ctrlHargaJual.EditValue)
            End If
            HitungHarga(UpdateHarga)
        End If
        Try
            Dim Konversi As Double = 0.0
            For i = 0 To GV1.RowCount - 1

                Konversi = NullToDbl(GV1.GetRowCellValue(i, "Konversi"))
                ' If IDSatuan = NullTolInt(ctrlIDSatuan.EditValue) Then
                If UpdateHarga Then
                    EksekusiSQL("Update MBarangD Set NilaiDiskon=" & FixKoma(Bulatkan(NullToDbl(ctrlDiscPromoRp.EditValue) * Konversi, 0)) & ",PromoDiskonJual=" & FixKoma(NullToDbl(ctrlDiscPromo.EditValue)) & ",HargaNetto=" & FixKoma(Bulatkan(NullToDbl(ctrlHargaJualNetto.EditValue) * Konversi, 0)) & " where NoID=" & NullTolInt(GV1.GetRowCellValue(i, "NoID")))
                End If
                GV1.SetRowCellValue(i, "HargaNetto", Bulatkan(NullToDbl(ctrlHargaJualNetto.EditValue) * Konversi, 0))
                GV1.SetRowCellValue(i, "PromoDiskonJual", NullToDbl(ctrlDiscPromo.EditValue))
                GV1.SetRowCellValue(i, "NilaiDiskon", Bulatkan(NullToDbl(ctrlDiscPromoRp.EditValue) * Konversi, 0))
                '  End If
            Next
        Catch ex As Exception
            XtraMessageBox.Show(ex.Message)
        End Try
    End Sub

    Private Sub HitungHargaJualMemberByProsen(Optional ByVal UpdateHarga As Boolean = False)
        Dim ctrlHargaJual As DevExpress.XtraEditors.CalcEdit = Nothing
        Dim ctrlHargaJualNetto As DevExpress.XtraEditors.CalcEdit = Nothing
        Dim ctrlDiscPromoRp As DevExpress.XtraEditors.CalcEdit = Nothing
        Dim ctrlDiscPromo As DevExpress.XtraEditors.CalcEdit = Nothing

        For Each ctrl In LC1.Controls
            If ctrl.name.ToString.ToLower = "txthargajual" Then
                ctrlHargaJual = ctrl
            ElseIf ctrl.name.ToString.ToLower = "txtHargaNettoMember".ToLower Then
                ctrlHargaJualNetto = ctrl
            ElseIf ctrl.name.ToString.ToLower = "txtDiscMemberRp2".ToLower Then
                ctrlDiscPromoRp = ctrl
            ElseIf ctrl.name.ToString.ToLower = "txtDiscMemberProsen2".ToLower Then
                ctrlDiscPromo = ctrl
            End If
            If ctrlHargaJual Is Nothing Or ctrlHargaJualNetto Is Nothing _
            Or ctrlDiscPromoRp Is Nothing Or ctrlDiscPromo Is Nothing Then
            Else
                Exit For 'semua yg dicari sudah ketemu
            End If
        Next
        If ctrlHargaJual Is Nothing Then
            XtraMessageBox.Show("Object:ctrlHargaJual  masih ada yg belum didefinisikan!", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        ElseIf ctrlHargaJualNetto Is Nothing Then
            XtraMessageBox.Show("Object:ctrlHargaJualNetto  masih ada yg belum didefinisikan!", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        ElseIf ctrlDiscPromoRp Is Nothing Then
            XtraMessageBox.Show("Object:ctrlDiscPromoRp  masih ada yg belum didefinisikan!", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        ElseIf ctrlDiscPromo Is Nothing Then
            XtraMessageBox.Show("Object:ctrlDiscPromo  masih ada yg belum didefinisikan!", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        Else
            ctrlDiscPromoRp.EditValue = Bulatkan((NullToDbl(ctrlDiscPromo.EditValue) / 100) * NullToDbl(ctrlHargaJual.EditValue), 0)
            ctrlHargaJualNetto.EditValue = Bulatkan(NullToDbl(ctrlHargaJual.EditValue) - NullToDbl(ctrlDiscPromoRp.EditValue), 0)
            HitungHarga(UpdateHarga)
            RefreshStock()
        End If
    End Sub

    Private Sub HitungHargaJualMemberProsenByRp(Optional ByVal UpdateHarga As Boolean = False)
        Dim ctrlHargaJual As DevExpress.XtraEditors.CalcEdit = Nothing
        Dim ctrlHargaJualNetto As DevExpress.XtraEditors.CalcEdit = Nothing
        Dim ctrlDiscPromoRp As DevExpress.XtraEditors.CalcEdit = Nothing
        Dim ctrlDiscPromo As DevExpress.XtraEditors.CalcEdit = Nothing
        
        For Each ctrl In LC1.Controls
            If ctrl.name.ToString.ToLower = "txthargajual" Then
                ctrlHargaJual = ctrl
            ElseIf ctrl.name.ToString.ToLower = "txtHargaNettoMember".ToLower Then
                ctrlHargaJualNetto = ctrl
            ElseIf ctrl.name.ToString.ToLower = "txtDiscMemberRp2".ToLower Then
                ctrlDiscPromoRp = ctrl
            ElseIf ctrl.name.ToString.ToLower = "txtDiscMemberProsen2".ToLower Then
                ctrlDiscPromo = ctrl
            End If
            If ctrlHargaJual Is Nothing Or ctrlHargaJualNetto Is Nothing _
            Or ctrlDiscPromoRp Is Nothing Or ctrlDiscPromo Is Nothing Then
            Else
                Exit For 'semua yg dicari sudah ketemu
            End If
        Next
        If ctrlHargaJual Is Nothing Then
            XtraMessageBox.Show("Object:ctrlHargaJual  masih ada yg belum didefinisikan!", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        ElseIf ctrlHargaJualNetto Is Nothing Then
            XtraMessageBox.Show("Object:ctrlHargaJualNetto  masih ada yg belum didefinisikan!", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        ElseIf ctrlDiscPromoRp Is Nothing Then
            XtraMessageBox.Show("Object:ctrlDiscPromoRp  masih ada yg belum didefinisikan!", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        ElseIf ctrlDiscPromo Is Nothing Then
            XtraMessageBox.Show("Object:ctrlDiscPromo  masih ada yg belum didefinisikan!", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
            'ElseIf ctrlKonversi Is Nothing Then
            '    XtraMessageBox.Show("Object:ctrlKonversi  masih ada yg belum didefinisikan!", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        Else
            ctrlHargaJualNetto.EditValue = Bulatkan(NullToDbl(ctrlHargaJual.EditValue), 0) - Bulatkan(NullToDbl(ctrlDiscPromoRp.EditValue), 0)
            If NullToDbl(ctrlHargaJual.EditValue) = 0 Then
                ctrlDiscPromo.EditValue = 0.0
            Else
                ctrlDiscPromo.EditValue = NullToDbl(ctrlDiscPromoRp.EditValue) * 100 / NullToDbl(ctrlHargaJual.EditValue)
            End If
            HitungHarga(UpdateHarga)
            RefreshStock()
        End If
    End Sub

    Private Sub HitungHargaJualPDP(Optional ByVal UpdateHarga As Boolean = False)
        Dim ctrlHargaJual As DevExpress.XtraEditors.CalcEdit = Nothing
        Dim ctrlHargaPDP As DevExpress.XtraEditors.CalcEdit = Nothing
        Dim ctrlDiscPDP As DevExpress.XtraEditors.CalcEdit = Nothing
        Dim ctrlHargaPDPMember As DevExpress.XtraEditors.CalcEdit = Nothing
        Dim ctrlDiscPDPMember As DevExpress.XtraEditors.CalcEdit = Nothing

        For Each ctrl In LC1.Controls
            If ctrl.name.ToString.ToLower = "txthargajual" Then
                ctrlHargaJual = ctrl
            ElseIf ctrl.name.ToString.ToLower = "txtHargaPDP".ToLower Then
                ctrlHargaPDP = ctrl
            ElseIf ctrl.name.ToString.ToLower = "txtDiscPDP".ToLower Then
                ctrlDiscPDP = ctrl
            ElseIf ctrl.name.ToString.ToLower = "txtHargaPDPMember".ToLower Then
                ctrlHargaPDPMember = ctrl
            ElseIf ctrl.name.ToString.ToLower = "txtDiscPDPMember".ToLower Then
                ctrlDiscPDPMember = ctrl
            End If
            If ctrlHargaJual Is Nothing Or ctrlHargaPDP Is Nothing _
            Or ctrlDiscPDP Is Nothing Or ctrlHargaPDPMember Is Nothing _
            Or ctrlDiscPDPMember Is Nothing Then
            Else
                Exit For 'semua yg dicari sudah ketemu
            End If
        Next
        If ctrlHargaJual Is Nothing Then
            XtraMessageBox.Show("Object:ctrlHargaJual  masih ada yg belum didefinisikan!", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        ElseIf ctrlHargaPDP Is Nothing Then
            XtraMessageBox.Show("Object:ctrlHargaPDP masih ada yg belum didefinisikan!", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        ElseIf ctrlDiscPDP Is Nothing Then
            XtraMessageBox.Show("Object:ctrlDiscPDP masih ada yg belum didefinisikan!", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        ElseIf ctrlDiscPDPMember Is Nothing Then
            XtraMessageBox.Show("Object:ctrlDiscPDPMember masih ada yg belum didefinisikan!", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        ElseIf ctrlDiscPDPMember Is Nothing Then
            XtraMessageBox.Show("Object:ctrlDiscPromoMember masih ada yg belum didefinisikan!", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        Else
            ctrlDiscPDP.EditValue = NullToDbl(ctrlHargaJual.EditValue) - NullToDbl(ctrlHargaPDP.EditValue)
            ctrlDiscPDPMember.EditValue = NullToDbl(ctrlHargaJual.EditValue) - NullToDbl(ctrlHargaPDPMember.EditValue)
            HitungHarga(UpdateHarga)
            'RefreshStock()
        End If
    End Sub

    Sub HitungHargaBeliPcsByHargaBeli()
        Dim ctrlHargaBeliPcsBruto As DevExpress.XtraEditors.CalcEdit = Nothing
        Dim ctrlHargaBeli As DevExpress.XtraEditors.CalcEdit = Nothing
        Dim ctrlCtnPcs As DevExpress.XtraEditors.CalcEdit = Nothing
        For Each ctrl In LC1.Controls
            If ctrl.name.ToString.ToLower = "txthargabelipcsbruto" Then
                ctrlHargaBeliPcsBruto = ctrl
            ElseIf ctrl.name.ToString.ToLower = "txthargabeli" Then
                ctrlHargaBeli = ctrl
            ElseIf ctrl.name.ToString.ToLower = "txtctnpcs" Then
                ctrlCtnPcs = ctrl
            End If
            If ctrlHargaBeliPcsBruto Is Nothing Or ctrlHargaBeli Is Nothing _
            Or ctrlCtnPcs Is Nothing Then
            Else
                Exit For 'semua yg dicari sudah ketemu
            End If
        Next
        If ctrlHargaBeliPcsBruto Is Nothing Then
            XtraMessageBox.Show("Object:ctrlHargaBeliPcsBruto  masih ada yg belum didefinisikan!", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        ElseIf ctrlHargaBeli Is Nothing Then
            XtraMessageBox.Show("Object:ctrlHargaBeli  masih ada yg belum didefinisikan!", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        ElseIf ctrlCtnPcs Is Nothing Then
            XtraMessageBox.Show("Object:ctrlCtnPcs  masih ada yg belum didefinisikan!", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        Else
            ctrlHargaBeliPcsBruto.EditValue = NullToDbl(ctrlHargaBeli.EditValue) / IIf(NullToDbl(ctrlCtnPcs.EditValue) = 0, 1, NullToDbl(ctrlCtnPcs.EditValue))
        End If
    End Sub
    Sub HitungHargaBeliByHargaBeliPcs()
        Dim ctrlHargaBeliPcsBruto As DevExpress.XtraEditors.CalcEdit = Nothing
        Dim ctrlHargaBeli As DevExpress.XtraEditors.CalcEdit = Nothing
        Dim ctrlCtnPcs As DevExpress.XtraEditors.CalcEdit = Nothing
        For Each ctrl In LC1.Controls
            If ctrl.name.ToString.ToLower = "txthargabelipcsbruto" Then
                ctrlHargaBeliPcsBruto = ctrl
            ElseIf ctrl.name.ToString.ToLower = "txthargabeli" Then
                ctrlHargaBeli = ctrl
            ElseIf ctrl.name.ToString.ToLower = "txtctnpcs" Then
                ctrlCtnPcs = ctrl
            End If
            If ctrlHargaBeliPcsBruto Is Nothing Or ctrlHargaBeli Is Nothing _
            Or ctrlCtnPcs Is Nothing Then
            Else
                Exit For 'semua yg dicari sudah ketemu
            End If
        Next
        If ctrlHargaBeliPcsBruto Is Nothing Then
            XtraMessageBox.Show("Object:ctrlHargaBeliPcsBruto  masih ada yg belum didefinisikan!", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        ElseIf ctrlHargaBeli Is Nothing Then
            XtraMessageBox.Show("Object:ctrlHargaBeli  masih ada yg belum didefinisikan!", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        ElseIf ctrlCtnPcs Is Nothing Then
            XtraMessageBox.Show("Object:ctrlCtnPcs  masih ada yg belum didefinisikan!", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
       Else
            ctrlHargaBeli.EditValue = NullToDbl(ctrlCtnPcs.EditValue) * NullToDbl(ctrlHargaBeliPcsBruto.EditValue)
        End If
    End Sub

    Sub HitungHargaJualByMargin2(Optional ByVal UpdateHarga As Boolean = False)
        Dim ctrlHargaBeliPcs As DevExpress.XtraEditors.CalcEdit = Nothing
        Dim ctrlprosenup2 As DevExpress.XtraEditors.CalcEdit = Nothing
        Dim ctrlIDSatuan As DevExpress.XtraEditors.SearchLookUpEdit = Nothing
        Dim ctrlHargaJual2 As DevExpress.XtraEditors.CalcEdit = Nothing
        Dim ctrlQty2 As DevExpress.XtraEditors.CalcEdit = Nothing

        For Each ctrl In LC1.Controls
            If ctrl.name.ToString.ToLower = "txthargabelipcs" Then
                ctrlHargaBeliPcs = ctrl
            ElseIf ctrl.name.ToString.ToLower = "txtprosenup2" Then
                ctrlprosenup2 = ctrl
            ElseIf ctrl.name.ToString.ToLower = "txthargajual2" Then
                ctrlHargaJual2 = ctrl
            ElseIf ctrl.name.ToString.ToLower = "txtqty2" Then
                ctrlQty2 = ctrl
            ElseIf ctrl.name.ToString.ToLower = "txtidsatuan" Then
                ctrlIDSatuan = ctrl
            End If
            If ctrlprosenup2 Is Nothing Or ctrlHargaBeliPcs Is Nothing _
            Or ctrlHargaJual2 Is Nothing Or ctrlIDSatuan Is Nothing Or ctrlQty2 Is Nothing Then
            Else
                Exit For 'semua yg dicari sudah ketemu
            End If
        Next
        If ctrlprosenup2 Is Nothing Then
            XtraMessageBox.Show("Object:ctrlprosenup2  masih ada yg belum didefinisikan!", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        ElseIf ctrlHargaBeliPcs Is Nothing Then
            XtraMessageBox.Show("Object:ctrlHargaBeliPcs  masih ada yg belum didefinisikan!", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        ElseIf ctrlHargaJual2 Is Nothing Then
            XtraMessageBox.Show("Object:ctrlHargaJual2  masih ada yg belum didefinisikan!", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        ElseIf ctrlIDSatuan Is Nothing Then
            XtraMessageBox.Show("Object:ctrlIDSatuan  masih ada yg belum didefinisikan!", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        ElseIf ctrlQty2 Is Nothing Then
            XtraMessageBox.Show("Object:ctrlQty2  masih ada yg belum didefinisikan!", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        Else
            ctrlHargaJual2.EditValue = Bulatkan((1 + ctrlprosenup2.EditValue / 100) * ctrlHargaBeliPcs.EditValue, 0)
        End If
        Try
            Dim IDSatuan As Integer
            For i = 0 To GV1.RowCount - 1
                IDSatuan = NullToDbl(GV1.GetRowCellValue(i, "IDSatuan"))
                If IDSatuan = NullTolInt(ctrlIDSatuan.EditValue) Then
                    If UpdateHarga Then
                        EksekusiSQL("Update MBarangD Set HargaJual2=" & FixKoma(Bulatkan(NullToDbl(ctrlHargaJual2.EditValue), 0)) & ",ProsenUp2=" & FixKoma(NullToDbl(ctrlprosenup2.EditValue)) & ",Qty2=" & FixKoma(NullToDbl(ctrlQty2.EditValue)) & " where NoID=" & NullTolInt(GV1.GetRowCellValue(i, "NoID")))
                    End If
                    GV1.SetRowCellValue(i, "HargaJual2", Bulatkan(NullToDbl(ctrlHargaJual2.EditValue), 0))
                    GV1.SetRowCellValue(i, "ProsenUp2", NullToDbl(ctrlprosenup2.EditValue))
                    GV1.SetRowCellValue(i, "Qty2", NullToDbl(ctrlQty2.EditValue))
                End If
            Next
        Catch ex As Exception
            XtraMessageBox.Show(ex.Message)
        End Try
    End Sub
    Sub HitungHargaJualByMargin3(Optional ByVal UpdateHarga As Boolean = False)
        Dim ctrlHargaBeliPcs As DevExpress.XtraEditors.CalcEdit = Nothing
        Dim ctrlprosenup3 As DevExpress.XtraEditors.CalcEdit = Nothing
        Dim ctrlIDSatuan As DevExpress.XtraEditors.SearchLookUpEdit = Nothing
        Dim ctrlHargaJual3 As DevExpress.XtraEditors.CalcEdit = Nothing
        Dim ctrlQty3 As DevExpress.XtraEditors.CalcEdit = Nothing

        For Each ctrl In LC1.Controls
            If ctrl.name.ToString.ToLower = "txthargabelipcs" Then
                ctrlHargaBeliPcs = ctrl
            ElseIf ctrl.name.ToString.ToLower = "txtprosenup3" Then
                ctrlprosenup3 = ctrl
            ElseIf ctrl.name.ToString.ToLower = "txthargajual3" Then
                ctrlHargaJual3 = ctrl
            ElseIf ctrl.name.ToString.ToLower = "txtqty3" Then
                ctrlQty3 = ctrl
            ElseIf ctrl.name.ToString.ToLower = "txtidsatuan" Then
                ctrlIDSatuan = ctrl
            End If
            If ctrlprosenup3 Is Nothing Or ctrlHargaBeliPcs Is Nothing _
            Or ctrlHargaJual3 Is Nothing Or ctrlIDSatuan Is Nothing Or ctrlQty3 Is Nothing Then
            Else
                Exit For 'semua yg dicari sudah ketemu
            End If
        Next
        If ctrlprosenup3 Is Nothing Then
            XtraMessageBox.Show("Object:ctrlprosenup3  masih ada yg belum didefinisikan!", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        ElseIf ctrlHargaBeliPcs Is Nothing Then
            XtraMessageBox.Show("Object:ctrlHargaBeliPcs  masih ada yg belum didefinisikan!", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        ElseIf ctrlHargaJual3 Is Nothing Then
            XtraMessageBox.Show("Object:ctrlHargaJual3  masih ada yg belum didefinisikan!", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        ElseIf ctrlIDSatuan Is Nothing Then
            XtraMessageBox.Show("Object:ctrlIDSatuan  masih ada yg belum didefinisikan!", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        ElseIf ctrlQty3 Is Nothing Then
            XtraMessageBox.Show("Object:ctrlQty3  masih ada yg belum didefinisikan!", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        Else
            ctrlHargaJual3.EditValue = Bulatkan((1 + ctrlprosenup3.EditValue / 100) * ctrlHargaBeliPcs.EditValue, 0)
        End If
        Try
            Dim konversi As Double = 0
            Dim IDSatuan As Integer
            For i = 0 To GV1.RowCount - 1
                IDSatuan = NullToDbl(GV1.GetRowCellValue(i, "IDSatuan"))
                If IDSatuan = NullTolInt(ctrlIDSatuan.EditValue) Then
                    If UpdateHarga Then
                        EksekusiSQL("Update MBarangD Set HargaJual3=" & FixKoma(Bulatkan(NullToDbl(ctrlHargaJual3.EditValue), 0)) & ",ProsenUp3=" & FixKoma(NullToDbl(ctrlprosenup3.EditValue)) & ",Qty3=" & FixKoma(NullToDbl(ctrlQty3.EditValue)) & " where NoID=" & NullTolInt(GV1.GetRowCellValue(i, "NoID")))
                    End If
                    GV1.SetRowCellValue(i, "HargaJual3", Bulatkan(NullToDbl(ctrlHargaJual3.EditValue), 0))
                    GV1.SetRowCellValue(i, "ProsenUp3", NullToDbl(ctrlprosenup3.EditValue))
                    GV1.SetRowCellValue(i, "Qty3", NullToDbl(ctrlQty3.EditValue))
                End If

            Next
        Catch ex As Exception
            XtraMessageBox.Show(ex.Message)
        End Try
    End Sub
    Sub HitungMarginByHargaJual3(Optional ByVal UpdateHarga As Boolean = False)
        Dim ctrlHargaBeliPcs As DevExpress.XtraEditors.CalcEdit = Nothing
        Dim ctrlprosenup3 As DevExpress.XtraEditors.CalcEdit = Nothing
        Dim ctrlIDSatuan As DevExpress.XtraEditors.SearchLookUpEdit = Nothing
        Dim ctrlHargaJual3 As DevExpress.XtraEditors.CalcEdit = Nothing
        Dim ctrlQty3 As DevExpress.XtraEditors.CalcEdit = Nothing

        For Each ctrl In LC1.Controls
            If ctrl.name.ToString.ToLower = "txthargabelipcs" Then
                ctrlHargaBeliPcs = ctrl
            ElseIf ctrl.name.ToString.ToLower = "txtprosenup3" Then
                ctrlprosenup3 = ctrl
            ElseIf ctrl.name.ToString.ToLower = "txthargajual3" Then
                ctrlHargaJual3 = ctrl
            ElseIf ctrl.name.ToString.ToLower = "txtqty3" Then
                ctrlQty3 = ctrl
            ElseIf ctrl.name.ToString.ToLower = "txtidsatuan" Then
                ctrlIDSatuan = ctrl
            End If
            If ctrlprosenup3 Is Nothing Or ctrlHargaBeliPcs Is Nothing _
            Or ctrlHargaJual3 Is Nothing Or ctrlIDSatuan Is Nothing Or ctrlQty3 Is Nothing Then
            Else
                Exit For 'semua yg dicari sudah ketemu
            End If
        Next
        If ctrlprosenup3 Is Nothing Then
            XtraMessageBox.Show("Object:ctrlprosenup3  masih ada yg belum didefinisikan!", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        ElseIf ctrlHargaBeliPcs Is Nothing Then
            XtraMessageBox.Show("Object:ctrlHargaBeliPcs  masih ada yg belum didefinisikan!", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        ElseIf ctrlHargaJual3 Is Nothing Then
            XtraMessageBox.Show("Object:ctrlHargaJual3  masih ada yg belum didefinisikan!", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        ElseIf ctrlIDSatuan Is Nothing Then
            XtraMessageBox.Show("Object:ctrlIDSatuan  masih ada yg belum didefinisikan!", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        ElseIf ctrlQty3 Is Nothing Then
            XtraMessageBox.Show("Object:ctrlQty3  masih ada yg belum didefinisikan!", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        Else
            'ctrlHargaJual3.EditValue = (1 + ctrlprosenup3.EditValue / 100) * ctrlHargaBeliPcs.EditValue
            If NullToDbl(ctrlHargaBeliPcs.EditValue) > 0 Then
                ctrlprosenup3.EditValue = (NullToDbl(ctrlHargaJual3.EditValue) - NullToDbl(ctrlHargaBeliPcs.EditValue)) * 100 / NullToDbl(ctrlHargaBeliPcs.EditValue)
            Else
                ctrlprosenup3.EditValue = 0
            End If
        End If
        Try
            Dim konversi As Double = 0
            Dim IDSatuan As Integer
            For i = 0 To GV1.RowCount - 1
                IDSatuan = NullToDbl(GV1.GetRowCellValue(i, "IDSatuan"))
                If IDSatuan = NullTolInt(ctrlIDSatuan.EditValue) Then
                    If UpdateHarga Then
                        EksekusiSQL("Update MBarangD Set HargaJual3=" & FixKoma(Bulatkan(NullToDbl(ctrlHargaJual3.EditValue), 0)) & ",ProsenUp3=" & FixKoma(NullToDbl(ctrlprosenup3.EditValue)) & ",Qty3=" & FixKoma(NullToDbl(ctrlQty3.EditValue)) & " where NoID=" & NullTolInt(GV1.GetRowCellValue(i, "NoID")))
                    End If
                    GV1.SetRowCellValue(i, "HargaJual3", Bulatkan(NullToDbl(ctrlHargaJual3.EditValue), 0))
                    GV1.SetRowCellValue(i, "ProsenUp3", NullToDbl(ctrlprosenup3.EditValue))
                    GV1.SetRowCellValue(i, "Qty3", NullToDbl(ctrlQty3.EditValue))
                End If

            Next
        Catch ex As Exception
            XtraMessageBox.Show(ex.Message)
        End Try
    End Sub
    Sub HitungMarginByHargaJual2(Optional ByVal UpdateHarga As Boolean = False)
        Dim ctrlHargaBeliPcs As DevExpress.XtraEditors.CalcEdit = Nothing
        Dim ctrlprosenup2 As DevExpress.XtraEditors.CalcEdit = Nothing
        Dim ctrlIDSatuan As DevExpress.XtraEditors.SearchLookUpEdit = Nothing
        Dim ctrlHargaJual2 As DevExpress.XtraEditors.CalcEdit = Nothing
        Dim ctrlQty2 As DevExpress.XtraEditors.CalcEdit = Nothing

        For Each ctrl In LC1.Controls
            If ctrl.name.ToString.ToLower = "txthargabelipcs" Then
                ctrlHargaBeliPcs = ctrl
            ElseIf ctrl.name.ToString.ToLower = "txtprosenup2" Then
                ctrlprosenup2 = ctrl
            ElseIf ctrl.name.ToString.ToLower = "txthargajual2" Then
                ctrlHargaJual2 = ctrl
            ElseIf ctrl.name.ToString.ToLower = "txtqty2" Then
                ctrlQty2 = ctrl
            ElseIf ctrl.name.ToString.ToLower = "txtidsatuan" Then
                ctrlIDSatuan = ctrl
            End If
            If ctrlprosenup2 Is Nothing Or ctrlHargaBeliPcs Is Nothing _
            Or ctrlHargaJual2 Is Nothing Or ctrlIDSatuan Is Nothing Or ctrlQty2 Is Nothing Then
            Else
                Exit For 'semua yg dicari sudah ketemu
            End If
        Next
        If ctrlprosenup2 Is Nothing Then
            XtraMessageBox.Show("Object:ctrlprosenup2  masih ada yg belum didefinisikan!", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        ElseIf ctrlHargaBeliPcs Is Nothing Then
            XtraMessageBox.Show("Object:ctrlHargaBeliPcs  masih ada yg belum didefinisikan!", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        ElseIf ctrlHargaJual2 Is Nothing Then
            XtraMessageBox.Show("Object:ctrlHargaJual2  masih ada yg belum didefinisikan!", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        ElseIf ctrlIDSatuan Is Nothing Then
            XtraMessageBox.Show("Object:ctrlIDSatuan  masih ada yg belum didefinisikan!", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        ElseIf ctrlQty2 Is Nothing Then
            XtraMessageBox.Show("Object:ctrlQty2  masih ada yg belum didefinisikan!", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        Else
            'ctrlHargaJual3.EditValue = (1 + ctrlprosenup3.EditValue / 100) * ctrlHargaBeliPcs.EditValue
            If NullToDbl(ctrlHargaBeliPcs.EditValue) > 0 Then
                ctrlprosenup2.EditValue = (NullToDbl(ctrlHargaJual2.EditValue) - NullToDbl(ctrlHargaBeliPcs.EditValue)) * 100 / NullToDbl(ctrlHargaBeliPcs.EditValue)
            Else
                ctrlprosenup2.EditValue = 0
            End If
        End If
        Try
            Dim konversi As Double = 0
            Dim IDSatuan As Integer
            For i = 0 To GV1.RowCount - 1
                IDSatuan = NullToDbl(GV1.GetRowCellValue(i, "IDSatuan"))
                If IDSatuan = NullTolInt(ctrlIDSatuan.EditValue) Then
                    If UpdateHarga Then
                        EksekusiSQL("Update MBarangD Set HargaJual2=" & FixKoma(Bulatkan(NullToDbl(ctrlHargaJual2.EditValue), 0)) & ",ProsenUp2=" & FixKoma(NullToDbl(ctrlprosenup2.EditValue)) & ",Qty2=" & FixKoma(NullToDbl(ctrlQty2.EditValue)) & " where NoID=" & NullTolInt(GV1.GetRowCellValue(i, "NoID")))
                    End If
                    GV1.SetRowCellValue(i, "HargaJual2", Bulatkan(NullToDbl(ctrlHargaJual2.EditValue), 0))
                    GV1.SetRowCellValue(i, "ProsenUp2", NullToDbl(ctrlprosenup2.EditValue))
                    GV1.SetRowCellValue(i, "Qty2", NullToDbl(ctrlQty2.EditValue))
                End If

            Next
        Catch ex As Exception
            XtraMessageBox.Show(ex.Message)
        End Try
    End Sub
    Sub HitungMarginByHargaJual(Optional ByVal UpdateHarga As Boolean = False)
        Dim ctrlHargaBeliPcs As DevExpress.XtraEditors.CalcEdit = Nothing
        Dim ctrlprosenup As DevExpress.XtraEditors.CalcEdit = Nothing
        Dim ctrlIDSatuan As DevExpress.XtraEditors.SearchLookUpEdit = Nothing
        Dim ctrlHargaJual As DevExpress.XtraEditors.CalcEdit = Nothing
        Dim ctrlQty1 As DevExpress.XtraEditors.CalcEdit = Nothing

        For Each ctrl In LC1.Controls
            If ctrl.name.ToString.ToLower = "txthargabelipcs" Then
                ctrlHargaBeliPcs = ctrl
            ElseIf ctrl.name.ToString.ToLower = "txtprosenup" Then
                ctrlprosenup = ctrl
            ElseIf ctrl.name.ToString.ToLower = "txthargajual" Then
                ctrlHargaJual = ctrl
            ElseIf ctrl.name.ToString.ToLower = "txtqty1" Then
                ctrlQty1 = ctrl
            ElseIf ctrl.name.ToString.ToLower = "txtidsatuan" Then
                ctrlIDSatuan = ctrl
            End If
            If ctrlprosenup Is Nothing Or ctrlHargaBeliPcs Is Nothing _
            Or ctrlHargaJual Is Nothing Or ctrlIDSatuan Is Nothing Or ctrlQty1 Is Nothing Then
            Else
                Exit For 'semua yg dicari sudah ketemu
            End If
        Next
        If ctrlprosenup Is Nothing Then
            XtraMessageBox.Show("Object:ctrlprosenup  masih ada yg belum didefinisikan!", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        ElseIf ctrlHargaBeliPcs Is Nothing Then
            XtraMessageBox.Show("Object:ctrlHargaBeliPcs  masih ada yg belum didefinisikan!", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        ElseIf ctrlHargaJual Is Nothing Then
            XtraMessageBox.Show("Object:ctrlHargaJual  masih ada yg belum didefinisikan!", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        ElseIf ctrlIDSatuan Is Nothing Then
            XtraMessageBox.Show("Object:ctrlIDSatuan  masih ada yg belum didefinisikan!", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        ElseIf ctrlQty1 Is Nothing Then
            XtraMessageBox.Show("Object:ctrlQty1  masih ada yg belum didefinisikan!", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        Else
            'ctrlHargaJual3.EditValue = (1 + ctrlprosenup3.EditValue / 100) * ctrlHargaBeliPcs.EditValue
            If NullToDbl(ctrlHargaBeliPcs.EditValue) > 0 Then
                ctrlprosenup.EditValue = (NullToDbl(ctrlHargaJual.EditValue) - NullToDbl(ctrlHargaBeliPcs.EditValue)) * 100 / NullToDbl(ctrlHargaBeliPcs.EditValue)
            Else
                ctrlprosenup.EditValue = 0
            End If
        End If
        Try
            Dim konversi As Double = 0
            Dim IDSatuan As Integer
            For i = 0 To GV1.RowCount - 1
                IDSatuan = NullToDbl(GV1.GetRowCellValue(i, "IDSatuan"))
                If IDSatuan = NullTolInt(ctrlIDSatuan.EditValue) Then
                    If UpdateHarga Then
                        EksekusiSQL("Update MBarangD Set HargaJual=" & FixKoma(Bulatkan(NullToDbl(ctrlHargaJual.EditValue), 0)) & ",ProsenUp=" & FixKoma(NullToDbl(ctrlprosenup.EditValue)) & ",Qty1=" & FixKoma(NullToDbl(ctrlQty1.EditValue)) & " where NoID=" & NullTolInt(GV1.GetRowCellValue(i, "NoID")))
                    End If
                    GV1.SetRowCellValue(i, "HargaJual", Bulatkan(NullToDbl(ctrlHargaJual.EditValue), 0))
                    GV1.SetRowCellValue(i, "ProsenUp", NullToDbl(ctrlprosenup.EditValue))
                    GV1.SetRowCellValue(i, "Qty1", NullToDbl(ctrlQty1.EditValue))
                End If

            Next
        Catch ex As Exception
            XtraMessageBox.Show(ex.Message)
        End Try
    End Sub

    Function BulatkanUp(ByVal HargaAsal As Double, ByVal Pembulatan As Long) As Long
        Dim HargaBulat As Long
        HargaBulat = Math.Round(HargaAsal, 0)
        ' Math.Round(2.5,0)
        If Pembulatan = 0 Then
            Return HargaBulat
        Else
            Return (HargaBulat \ Pembulatan) * Pembulatan + IIf(HargaBulat Mod Pembulatan = 0, 0, Pembulatan)
        End If

    End Function

    Private Sub ckedit_Click(ByVal sender As Object, ByVal e As System.EventArgs)

    End Sub

    Private Sub ckedit_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) 'Handles ckedit.CheckedChanged
        If sender.name.ToString.ToLower = "txtisppn" Then
            HitungHarga()
        End If
    End Sub

    'Private Sub ckeditLockQty_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) 'Handles ckedit.CheckedChanged
    '    'Dim ckIsLock As CheckEdit = TryCast(sender, CheckEdit)
    '    'Dim txtQtyMax As TextEdit = Nothing
    '    'Dim txtQtyMin As TextEdit = Nothing
    '    'Try
    '    '    txtQtyMax.Properties.ReadOnly = ckIsLock.Checked
    '    '    txtQtyMin.Properties.ReadOnly = ckIsLock.Checked
    '    'Catch ex As Exception

    '    'End Try
    'End Sub

    Private Sub clsBarang_Shown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Shown
        Try
            If isNew Then
                BS.AddNew()
                IsiDefault()
            Else

            End If
            For Each ctl In LC1.Controls
                If ctl.name.ToString.ToLower = "txtkode".ToLower Then
                    TryCast(ctl, TextEdit).Focus()
                    Exit For
                End If
            Next
        Catch ex As Exception

        End Try
    End Sub

    Private Sub ckAll_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ckAll.CheckedChanged
        Switch(False)
    End Sub

    Public Sub Method()

    End Sub
End Class