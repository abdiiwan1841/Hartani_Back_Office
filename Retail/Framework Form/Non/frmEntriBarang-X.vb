Imports System.Data.SqlClient
Imports System.Data.SQLite

Imports DevExpress.XtraLayout
Imports System.Linq

Public Class frmEntriBarang
    Public FormName As String = ""
    Public TableName As String = ""

    Public isNew As Boolean = True
    Public NoID As Long = -1
    Dim oda2 As SqlDataAdapter

    Dim ds As New DataSet
    Dim BS As New BindingSource
    Dim WithEvents cb As New DevExpress.XtraEditors.Controls.EditorButton

    Dim WithEvents txtEdit As DevExpress.XtraEditors.TextEdit
    Dim WithEvents clcEdit As DevExpress.XtraEditors.CalcEdit
    Dim ckedit As DevExpress.XtraEditors.CheckEdit
    Dim dtEdit As DevExpress.XtraEditors.DateEdit
    Dim WithEvents lkEdit As DevExpress.XtraEditors.LookUpEdit
    Sub GenerateForm()
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
        TableName = GetTableNamebyFormname(FormName)
        ocmd2.Connection = cn
        ocmd2.CommandType = CommandType.Text
        If isNew Then
            ocmd2.CommandText = "select * from " & TableName
        Else
            ocmd2.CommandText = "select * from " & TableName & " where noid=" & NoID.ToString
        End If
        cn.Open()
        sysSQLoda = New SQLite.SQLiteDataAdapter(sysSQLcommand)
        sysSQLoda.Fill(ds, "Master")
        oda2 = New SqlDataAdapter(ocmd2)
        oda2.Fill(ds, "Data")
        BS.DataSource = ds.Tables("Data")
        'MBarangD

        ocmd2.CommandText = "select * from MBarangD where IDBarang=" & NoID
        sysSQLoda = New SQLite.SQLiteDataAdapter(sysSQLcommand)
        sysSQLoda.Fill(ds, "Master")
        oda2 = New SqlDataAdapter(ocmd2)
        oda2.Fill(ds, "MBarangD")
        GridControl1.DataSource = ds.Tables("MBarangD")

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
                Case "calcedit"
                    clcEdit = New DevExpress.XtraEditors.CalcEdit
                    clcEdit.Name = "txt" + ds.Tables("Master").Rows(i).Item("nama")
                    clcEdit.EnterMoveNextControl = True
                    Select Case ds.Tables("Master").Rows(i).Item("tipe").ToString.ToLower
                        Case "int", "bigint", "smallint", "float", "numeric", "money", "real"
                            clcEdit.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric
                            clcEdit.Properties.Mask.EditMask = ds.Tables("Master").Rows(i).Item("format")
                            clcEdit.Properties.Mask.UseMaskAsDisplayFormat = True
                            txtEdit.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
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
                        clcEdit.DataBindings.Add("Text", BS _
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
                    dtEdit.Properties.Mask.EditMask = ds.Tables("Master").Rows(i).Item("format")

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
                    cb.Kind = DevExpress.XtraEditors.Controls.ButtonPredefines.Delete
                    lkEdit = New DevExpress.XtraEditors.LookUpEdit
                    lkEdit.Properties.Buttons.Add(cb)
                    lkEdit.Properties.NullText = ""
                    lkEdit.Name = "txt" + ds.Tables("Master").Rows(i).Item("nama")
                    lkEdit.Properties.DisplayMember = NullTostr(ds.Tables("Master").Rows(i).Item("lookupdisplay"))
                    lkEdit.Properties.ValueMember = NullTostr(ds.Tables("Master").Rows(i).Item("lookupvalue"))
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
                    oda2 = New SqlDataAdapter(ocmd2)
                    oda2.Fill(ds, ds.Tables("Master").Rows(i).Item("tablelookup"))
                    lkEdit.Properties.DataSource = ds.Tables(ds.Tables("Master").Rows(i).Item("tablelookup"))
                    AddHandler lkEdit.EditValueChanged, AddressOf lkEdit_EditValueChanged
                    AddHandler lkEdit.ButtonClick, AddressOf lkEdit_ButtonClick
            End Select
        Next
        If isNew Then
            BS.AddNew()
            IsiDefault()
        Else

        End If

    End Sub

    Private Sub frmSimpleEntri_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        LC1.SaveLayoutToXml(Application.StartupPath & "\System\Layouts\" & FormName & ".xml")
    End Sub

    Private Sub frmSimpleEntri_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            TableName = GetTableNamebyFormname(FormName)
            GenerateForm()
            ' isiTambahan()
            'LC1.BeginUpdate()
            If System.IO.File.Exists(Application.StartupPath & "\System\Layouts\" & FormName & ".xml") Then
                LC1.RestoreLayoutFromXml(Application.StartupPath & "\System\Layouts\" & FormName & ".xml")
            End If


        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical)
        End Try

        'LC1.EndUpdate()
    End Sub
    Sub isiTambahan()
        Dim cn As New SqlConnection(StrKonSql)
        Dim ocmd2 As New SqlCommand
        ocmd2.Connection = cn
        ocmd2.CommandType = CommandType.Text
        ' If isNew Then
        ocmd2.CommandText = "select * from " & "msatuan"
        'Else
        'ocmd2.CommandText = "select * from " & TableName & " where noid=" & NoID.ToString
        'End If
        cn.Open()
        oda2 = New SqlDataAdapter(ocmd2)
        oda2.Fill(ds, "Satuan")
        RepositoryItemGridLookUpEdit1.DataSource = ds.Tables("Satuan")

        ocmd2.Dispose()
        cn.Close()
        cn.Dispose()
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
            Dim hasil = From control In LC1.Controls Where control.name.ToString.ToLower = "txtnoid"
            For Each x In hasil
                x.editvalue = GetNewID(TableName)
                ocmd2.CommandText = "select * from " & TableName & " where noid=" & x.editvalue
                Exit For
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
        LC1.RestoreLayoutFromXml(Application.StartupPath & "\System\Layouts\" & FormName & ".xml")
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
                                ctrl.editvalue = Today
                            Case "now"
                                ctrl.editvalue = Now
                            Case Else
                                ctrl.editvalue = str(i).Substring(3)
                        End Select
                    End If
                Next
            End If
        Next
    End Sub

    Private Sub lkEdit_ButtonClick(ByVal sender As Object, ByVal e As DevExpress.XtraEditors.Controls.ButtonPressedEventArgs)
        Select Case e.Button.Kind
            Case DevExpress.XtraEditors.Controls.ButtonPredefines.Delete
                sender.editvalue = -1
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
    Private Function getValueFromLookup(ByVal sender As Object, ByVal fieldname As String) As String
        Dim lu As DevExpress.XtraEditors.LookUpEdit = CType(sender, DevExpress.XtraEditors.LookUpEdit)
        Dim strtablefield As String() = Split(fieldname, ".")
        Return lu.GetColumnValue(strtablefield(1))
    End Function

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
        Dim PesanSalah As String = ""
        If Simpan(PesanSalah) Then
            Me.DialogResult = Windows.Forms.DialogResult.OK
            Me.Close()
        Else
            MsgBox("Info kesalahan:" & vbCrLf & PesanSalah, MsgBoxStyle.Exclamation)
        End If
    End Sub

    Private Sub SimpleButton1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SimpleButton1.Click
        Dim PesanSalah As String = ""
        If Simpan(PesanSalah) = True Then
            Me.DialogResult = Windows.Forms.DialogResult.OK
            Me.Close()
        Else
            MsgBox("Info kesalahan:" & vbCrLf & PesanSalah, MsgBoxStyle.Exclamation)
        End If
    End Sub

    Private Sub SimpleButton3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SimpleButton3.Click
        Batal()
    End Sub
End Class