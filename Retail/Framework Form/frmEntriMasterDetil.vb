Imports System.Data.SqlClient
Imports System.Data.SQLite
Imports DevExpress.XtraEditors
Imports DevExpress.Utils
Imports DevExpress.XtraLayout
'Imports System.Linq
Imports DevExpress.XtraGrid.Views.Base
Imports System.Drawing
Imports DevExpress.XtraEditors.Repository
Imports VPoint.mdlCetakCR

Public Class FrmEntriMasterDetil
    Public FormName As String = ""
    Public TableName As String = ""
    Public SqlDetil As String = ""

    Public TableNameD As String = ""

    Public FormEntriName As String = ""

    Public isNew As Boolean = True
    Public IsPosted As Boolean = False
    Public NoID As Long = -1
    Dim oda2 As SqlDataAdapter

    Dim ds As New DataSet
    Dim BS As New BindingSource
    Dim WithEvents cb As New DevExpress.XtraEditors.Controls.EditorButton

    Dim WithEvents txtEdit As DevExpress.XtraEditors.TextEdit
    Dim WithEvents clcEdit As DevExpress.XtraEditors.CalcEdit
    Dim ckedit As DevExpress.XtraEditors.CheckEdit
    Dim dtEdit As DevExpress.XtraEditors.DateEdit
    Dim WithEvents lkEdit As DevExpress.XtraEditors.SearchLookUpEdit
    Dim repckedit As New RepositoryItemCheckEdit
    Dim txtSubTotal As TextEdit
    Dim KodeLama As String = ""

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
                            
                        Case "date", "datetime", "time"
                            txtEdit.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.DateTime
                            txtEdit.Properties.Mask.EditMask = ds.Tables("Master").Rows(i).Item("format")
                            txtEdit.Properties.Mask.UseMaskAsDisplayFormat = True
                        Case Else
                            If Not IsDBNull(ds.Tables("Master").Rows(i).Item("format")) AndAlso ds.Tables("Master").Rows(i).Item("format") <> "" Then
                                txtEdit.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Custom
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

                    itemLC = New LayoutControlItem
                    itemLC.Name = ds.Tables("Master").Rows(i).Item("caption")
                    itemLC.Parent = LC1.Root

                    itemLC.Control = txtEdit
                    AddHandler txtEdit.LostFocus, AddressOf txtEdit_LostFocus

                    If txtEdit.Name.ToLower = "txtkode".ToLower Then
                        KodeLama = txtEdit.Text
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

                    itemLC = New LayoutControlItem
                    itemLC.Name = ds.Tables("Master").Rows(i).Item("caption")
                    itemLC.Parent = LC1.Root

                    itemLC.Control = clcEdit
                    AddHandler clcEdit.LostFocus, AddressOf clcEdit_LostFocus
                Case "checkedit"
                    ckedit = New DevExpress.XtraEditors.CheckEdit
                    ckedit.Name = "txt" + ds.Tables("Master").Rows(i).Item("nama")
                    ckedit.Text = ds.Tables("Master").Rows(i).Item("caption")
                    ckedit.Visible = NullTobool(ds.Tables("Master").Rows(i).Item("visible"))
                    ckedit.Properties.ReadOnly = NullTobool(ds.Tables("Master").Rows(i).Item("readonly"))
                    ckedit.DataBindings.Add("editvalue", BS _
                      , ds.Tables("Master").Rows(i).Item("fieldname"))
                    itemLC = New LayoutControlItem
                    itemLC.Name = ds.Tables("Master").Rows(i).Item("caption")

                    itemLC.Parent = LC1.Root
                    itemLC.Control = ckedit
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
                    'lkEdit_EditValueChanged(lkEdit, Nothing)
            End Select
        Next

        'Catch ex As Exception
        '    XtraMessageBox.Show("Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        'End Try
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

    End Sub

    Private Sub FrmEntriMasterDetil_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing

    End Sub

    Private Sub frmSimpleEntri_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim dlg As DevExpress.Utils.WaitDialogForm = Nothing
        Try
            Dim curentcursor As Cursor = Windows.Forms.Cursor.Current
            Windows.Forms.Cursor.Current = Cursors.WaitCursor
            dlg = New DevExpress.Utils.WaitDialogForm(String.Format("Creating component on " & Me.FormName.ToString & ".{0}MOHON TUNGGU ...", vbCrLf), Application.ProductName)
            dlg.TopMost = False
            dlg.Show()
            Windows.Forms.Cursor.Current = Cursors.WaitCursor
            Me.Width = Ini.BacaIniPath(folderLayouts & FormName & ".ini", "Form", "Width", Me.Width)
            Me.Height = Ini.BacaIniPath(folderLayouts & FormName & ".ini", "Form", "Height", Me.Height)

            Me.Left = (Screen.PrimaryScreen.WorkingArea.Width - Me.Width) \ 2
            Me.Top = (Screen.PrimaryScreen.WorkingArea.Height - Me.Height) \ 2
            GenerateForm()
            'LC1.BeginUpdate()
            If System.IO.File.Exists(folderLayouts & FormName & ".xml") Then
                LC1.RestoreLayoutFromXml(folderLayouts & FormName & ".xml")
            End If

            RefreshDetil()
            'LC1.EndUpdate()

            SetTombol()
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
            'LINQ
            'Dim hasil = From control In LC1.Controls Where control.name.ToString.ToLower = "txtnoid"
            'For Each x In hasil
            '    x.editvalue = GetNewID(TableName)
            '    NoID = x.EditValue
            '    ocmd2.CommandText = "select * from " & TableName & " where noid=" & NoID
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
        LC1.RestoreLayoutFromXml(folderLayouts & FormName & ".xml")
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
                                    ctrl.checked = NullTobool(str(i).Substring(3))
                                Else
                                    ctrl.editvalue = str(i).Substring(3)
                                End If
                        End Select
                    End If
                Next
            End If
        Next
    End Sub

    Private Sub lkEdit_ButtonClick(ByVal sender As Object, ByVal e As DevExpress.XtraEditors.Controls.ButtonPressedEventArgs)
        Dim lked As DevExpress.XtraEditors.SearchLookUpEdit = Nothing
        lked = TryCast(sender, DevExpress.XtraEditors.SearchLookUpEdit)
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
        Dim isada As Boolean = False
        Dim lastexpresi As String = ekspresi
        Dim nmobject As String
        While lastexpresi.Contains("[")
            Dim a As Integer = Strings.InStr(lastexpresi, "[")
            Dim b As Integer = Strings.InStr(lastexpresi, "]")
            nmobject = lastexpresi.Substring(a, b - a - 1)
            isada = False
            For Each ctrl In LC1.Controls
                If ctrl.name.ToString.ToLower = "txt" + nmobject.ToLower Then
                    lastexpresi = Replace(lastexpresi, "[" + nmobject + "]", CDbl(ctrl.editvalue))
                    isada = True
                    Exit For
                End If
            Next
            If Not isada Then
                XtraMessageBox.Show("Ada kesalahan penulisan expresi:" & vbCrLf & ekspresi & vbCrLf & " Objek :" & nmobject & " tidak ditemukan!", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
                Exit Function
            End If
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
    Private Function IsValidasi()
        For Each ctrl In LC1.Controls
            If ctrl.name.tolower = "txtkode".ToLower Then
                Dim txtKode As TextEdit
                txtKode = CType(ctrl, TextEdit)
                If txtKode.Text = "" Then
                    XtraMessageBox.Show("Kode masih kosong.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Warning)
                    txtKode.Focus()
                    Return False
                End If
                If CekKodeValid(txtKode.Text, KodeLama, TableName, "Kode", Not isNew) Then
                    XtraMessageBox.Show("Kode Sudah dipakai.", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Warning)
                    txtKode.Focus()
                    Return False
                End If
            End If
        Next
        Return True
    End Function
    Private Sub SimpleButton1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSave.Click
        Dim PesanSalah As String = ""
        If Not IsPosted Then
            If IsValidasi() Then
                If Simpan(PesanSalah) = True Then
                    If isNew Then
                        isNew = False
                        SetTombol()
                        txtBarang.Focus()
                    Else
                        DialogResult = Windows.Forms.DialogResult.OK
                        Close()
                    End If
                Else
                    XtraMessageBox.Show("Info Kesalahan : " & vbCrLf & PesanSalah, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                End If
            End If
        End If
    End Sub

    Private Sub SimpleButton3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SimpleButton3.Click
        Batal()
    End Sub

    Private Sub SimpleButton5_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBaru.Click
        Baru()
    End Sub
    Sub Baru()

        Dim x As New frmSimpleEntri
        x.FormName = FormEntriName

        x.isNew = True
        If x.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
            EksekusiSQL("Update " & TableNameD & " set ID" & TableName.Substring(1) & "=" & NoID & " where " & TableNameD & ".NoID=" & x.NoID)
            RefreshDetil()
            GV1.FocusedRowHandle = GV1.LocateByDisplayText(0, GV1.Columns("NoID"), NoID.ToString("#,##0"))
        End If
        x.Dispose()
    End Sub

    Private Sub mnTambah_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles mnTambah.ItemClick
        Baru()
    End Sub

    Private Sub SimpleButton4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEdit.Click
        Edit()
    End Sub

    Private Sub mnEditSatuan_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles mnEditSatuan.ItemClick
        Edit()
    End Sub

    Private Sub mnHapusSatuan_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles mnHapusSatuan.ItemClick
        Hapus()
    End Sub

    Private Sub SimpleButton2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnHapus.Click
        Hapus()
    End Sub
    Sub Hapus()
        Dim view As ColumnView = GC1.FocusedView
        Try
            Dim row As System.Data.DataRow = view.GetDataRow(GV1.FocusedRowHandle)
            Dim dc As Integer = GV1.FocusedRowHandle
            Dim NoID As Long = NullToLong(row("NoID"))
            If XtraMessageBox.Show("Item ini mau dihapus?", NamaAplikasi, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) = Windows.Forms.DialogResult.Yes Then
                EksekusiSQL("Delete From " & TableNameD & " where NoID=" & NoID.ToString)
                RefreshDetil()
            End If
        Catch ex As Exception
            XtraMessageBox.Show("Untuk mengedit pilih item satuan yang akan diedit terlebih dahulu lalu tekan tombol edit", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally

        End Try
    End Sub
    Sub Edit()
        Dim x As frmSimpleEntri
        'Dim Brg As clsBarang

        Dim view As ColumnView = GC1.FocusedView
        Try
            Dim row As System.Data.DataRow = view.GetDataRow(GV1.FocusedRowHandle)
            Dim dc As Integer = GV1.FocusedRowHandle
            Dim NoID As Long = NullToLong(row("NoID"))

            x = New frmSimpleEntri
            x.FormName = FormEntriName
            x.isNew = False
            x.NoID = NoID
            If x.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
                RefreshDetil()
                GV1.ClearSelection()
                GV1.FocusedRowHandle = GV1.LocateByDisplayText(0, GV1.Columns("NoID"), NoID.ToString("#,##0"))
                GV1.SelectRow(GV1.FocusedRowHandle)
            End If
            x.Dispose()

        Catch ex As Exception
            XtraMessageBox.Show("Untuk mengedit pilih item satuan yang akan diedit terlebih dahulu lalu tekan tombol edit", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally

        End Try
    End Sub
    Sub SetTombol()
        Try
            IsPosted = NullToBool(ds.Tables("Data").Rows(0).Item("IsPosted"))
            If isNew Or IsPosted Then
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
            If IsPosted Then
                cmdSave.Enabled = False
            Else
                cmdSave.Enabled = True
            End If
        Catch ex As Exception

        End Try
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
        ocmd2.CommandText = SqlDetil & " " & NoID

        oda2 = New SqlDataAdapter(ocmd2)
        If ds.Tables("MDetil") Is Nothing Then
        Else
            ds.Tables("MDetil").Clear()
        End If
        oda2.Fill(ds, "MDetil")
        GC1.DataSource = ds.Tables("MDetil")

        ocmd2.Dispose()
        cn.Close()
        cn.Dispose()
        For i As Integer = 0 To GV1.Columns.Count - 1
            ' MsgBox(GV1.Columns(i).fieldname.ToString)
            Select Case GV1.Columns(i).ColumnType.Name.ToLower

                Case "int32", "int64", "int"
                    GV1.Columns(i).DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
                    GV1.Columns(i).DisplayFormat.FormatString = "n0"
                Case 2
                Case "decimal", "single", "money", "double"
                    GV1.Columns(i).DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
                    GV1.Columns(i).DisplayFormat.FormatString = "n2"
                Case "string"
                    GV1.Columns(i).DisplayFormat.FormatType = DevExpress.Utils.FormatType.None
                    GV1.Columns(i).DisplayFormat.FormatString = ""
                Case "date", "datetime"
                    GV1.Columns(i).DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime
                    GV1.Columns(i).DisplayFormat.FormatString = "dd-MM-yyyy"
                    If GV1.Columns(i).FieldName.ToLower = "jam" Then
                        GV1.Columns(i).DisplayFormat.FormatString = "HH:mm:ss"
                    End If
                Case "boolean"
                    GV1.Columns(i).ColumnEdit = repckedit

            End Select
            If GV1.Columns(i).FieldName.Length >= 4 AndAlso GV1.Columns(i).FieldName.Substring(0, 4).ToLower = "Kode".ToLower Then
                GV1.Columns(i).Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left
            ElseIf GV1.Columns(i).FieldName.ToLower = "Nama".ToLower Then
                GV1.Columns(i).Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left
            End If
        Next

        'Fungsi Tambahan
        Dim tem As Double = 0.0
        For Each ctrl In LC1.Controls
            If ctrl.name.ToString.ToLower = "txtsubtotal".Trim.ToLower Then
                Dim txt As TextEdit = CType(ctrl, TextEdit)
                If TableName.ToLower = "mbeli".ToLower Then
                    For x As Integer = 0 To GV1.RowCount
                        tem = tem + NullToDbl(GV1.GetRowCellValue(x, "Jumlah"))
                        Exit For
                    Next
                    txt.EditValue = tem
                    clcEdit_LostFocus(txt, Nothing)
                    Exit For
                End If
            End If
        Next
        'If System.IO.File.Exists(folderLayouts &  FormName & "Grid Detil.xml") Then
        '    GridView1.RestoreLayoutFromXml(folderLayouts &  FormName & ".xml")
        'End If
        If System.IO.File.Exists(FolderLayouts & FormName & "Grid Detil.xml") Then
            GV1.RestoreLayoutFromXml(FolderLayouts & FormName & "Grid Detil.xml")
        End If

    End Sub

    Private Sub BarButtonItem4_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem4.ItemClick
        Dim xOtorisasi As New frmOtorisasiAdmin
        Try
            If XtraMessageBox.Show("Simpan Layout?", NamaAplikasi, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = Windows.Forms.DialogResult.Yes AndAlso xOtorisasi.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
                Ini.TulisIniPath(FolderLayouts & FormName & ".ini", "Form", "Width", Me.Width)
                Ini.TulisIniPath(FolderLayouts & FormName & ".ini", "Form", "Height", Me.Height)

                LC1.SaveLayoutToXml(FolderLayouts & FormName & ".xml")
                GV1.SaveLayoutToXml(FolderLayouts & FormName & "Grid Detil.xml")

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
        Catch ex As Exception
            XtraMessageBox.Show("Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            xOtorisasi.Dispose()
        End Try
    End Sub
    Sub UpdateSubtotal()

    End Sub

    Private Sub BarButtonItem5_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem5.ItemClick
        CetakFaktur(action_.Preview)
    End Sub
    'Untuk Perintah Cetaknya
    'Private Sub BarButtonItem10_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem10.ItemClick
    '    If EditRpt = True Then
    '        CetakBukti(action_.Edit, True)
    '    Else
    '        CetakBukti(action_.Preview, True)
    '    End If
    'End Sub
    Private Sub CetakFaktur(ByVal action As action_)
        Dim namafile As String
        Try
            namafile = Application.StartupPath & "\report\Faktur" & FormName & ".rpt"
            'If Not System.IO.File.Exists(namafile) Then Exit Sub
            'If action = action_.Edit Then
            '    BukaFile(namafile)
            '    Exit Sub
            'End If
            'ViewReport(action, namafile, "Faktur Penjualan", "{MJual.NoID}=" & NoID)
            'CetakCRViewer(action, Me.MdiParent, namafile, "Cetak " & Me.Text, , "NoID", NoID)
        Catch EX As Exception
            DevExpress.XtraEditors.XtraMessageBox.Show("Kesalahan:" & EX.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub SimpleButton1_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SimpleButton1.Click
        If frmMain.ckEditRpt.Checked Then
            CetakFaktur(action_.Edit)
        Else
            CetakFaktur(action_.Preview)
        End If
    End Sub

    Private Sub txtBarang_ButtonClick(ByVal sender As Object, ByVal e As DevExpress.XtraEditors.Controls.ButtonPressedEventArgs) Handles txtBarang.ButtonClick
        If isNew Then Exit Sub
        Select Case e.Button.Index
            Case 0
                InsertIntoDetil()
                txtBarang.Text = ""
            Case 1
                txtBarang.Text = ""
        End Select
    End Sub

    Private Sub txtBarang_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtBarang.EditValueChanged

    End Sub

    Private Sub InsertIntoDetil()
        Dim SQL As String = ""
        Dim NamaBarang As String = txtBarang.Text
        Dim IDBarang As Long = -1
        Dim IDDetil As Long = -1
        Try
            If CariBarang(IDBarang, NamaBarang) Then
                If XtraMessageBox.Show("Ingin menambah barang " & NamaBarang, "Fast Entri Says", MessageBoxButtons.YesNo) = Windows.Forms.DialogResult.Yes Then
                    IDDetil = GetNewID(TableNameD, "NoID")
                    SQL = "INSERT INTO " & TableNameD & " (NoID,IDBarang,ID" & TableName.Substring(1) & ") VALUES " & vbCrLf
                    SQL &= "(" & IDDetil & "," & IDBarang & "," & NoID & ")"
                    EksekusiSQL(SQL)

                    Dim x As frmSimpleEntri
                    Try
                        x = New frmSimpleEntri
                        x.FormName = FormEntriName
                        x.isNew = False
                        x.NoID = IDDetil
                        If x.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
                            RefreshDetil()
                            GV1.ClearSelection()
                            GV1.FocusedRowHandle = GV1.LocateByDisplayText(0, GV1.Columns("NoID"), IDDetil.ToString("#,##0"))
                            GV1.SelectRow(GV1.FocusedRowHandle)
                        Else
                            SQL = "DELETE FROM " & TableNameD & " WHERE NoID=" & IDDetil
                            EksekusiSQL(SQL)
                            RefreshDetil()
                            GV1.ClearSelection()
                            GV1.FocusedRowHandle = GV1.LocateByDisplayText(0, GV1.Columns("NoID"), IDDetil.ToString("#,##0"))
                            GV1.SelectRow(GV1.FocusedRowHandle)
                        End If
                        x.Dispose()
                    Catch ex As Exception

                    End Try
                End If
            End If
        Catch ex As Exception
            XtraMessageBox.Show("Kesalahan : " & ex.Message, "Fast Entri Says", MessageBoxButtons.OK)
        End Try
    End Sub
    Private Sub EditValue(ByVal IDDetil As Long)
        
    End Sub
    Private Function CariBarang(ByRef IDBarang As Long, ByRef NamaBarang As String) As Boolean
        Dim x As Boolean = False
        Dim SQL As String = ""
        Dim oDS As New DataSet
        Try
            SQL = "SELECT NoID,Kode,Nama FROM MBarang WHERE IsActive=1 AND (ISNULL(KODE,'')<>'') AND (UPPER(Kode) = '" & NamaBarang.Replace("'", "''").ToUpper & "' OR UPPER(NAMA) = '" & NamaBarang.Replace("'", "''").ToUpper & "') ORDER BY Kode"
            oDS = ExecuteDataset("Tbl", SQL)
            If oDS.Tables("Tbl").Rows.Count >= 1 Then
                NamaBarang = NullTostr(oDS.Tables(0).Rows(0).Item("Nama"))
                IDBarang = NullTolong(oDS.Tables(0).Rows(0).Item("NoID"))
                x = True
            Else
                x = False
            End If
        Catch ex As Exception
            x = False
        End Try
        Return x
    End Function

    Private Sub SimpleButton2_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SimpleButton2.Click

    End Sub

    Private Sub FrmEntriMasterDetil_Shown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Shown
        If isNew Then
            BS.AddNew()
            IsiDefault()
        Else

        End If
    End Sub
End Class