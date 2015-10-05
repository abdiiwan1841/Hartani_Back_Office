Imports System.Data.SqlClient
Imports DevExpress.XtraGrid.Views.Base
Imports DevExpress.XtraEditors.Repository
Imports DevExpress.XtraEditors

Imports System.Drawing
Public Class frmLookupMaterai
    Public Strsql As String = ""
    Public NoID As Integer = -1
    Dim oda2 As SqlDataAdapter
    Dim ds As New DataSet
    Public Nilai As Double = 0.0
    Public FormName As String = ""
    Dim repckedit As New RepositoryItemCheckEdit
    Dim BolehAmbilData As Boolean = False
    Public row As System.Data.DataRow
    Private Sub SimpleButton6_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SimpleButton6.Click
        Batal()
    End Sub
    Public Sub Batal()
        DialogResult = Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub
    Public Sub AmbilData()
        Dim view As ColumnView = GC1.FocusedView
        Try
            row = view.GetDataRow(GV1.FocusedRowHandle)
            Dim dc As Integer = GV1.FocusedRowHandle
            NoID = NullToLong(row("NoID"))
            Nilai = NullToDbl(row("Nilai"))
            DialogResult = Windows.Forms.DialogResult.OK
            Me.Close()
        Catch ex As Exception
            XtraMessageBox.Show("Untuk menghapus pilih data yang akan dihapus terlebih dahulu lalu tekan tombol hapus!", NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally

        End Try
    End Sub

    Private Sub SimpleButton9_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SimpleButton9.Click
        AmbilData()
    End Sub
    Sub RefreshData()
        Dim cn As New SqlConnection(StrKonSql)
        Dim ocmd2 As New SqlCommand
        ocmd2.Connection = cn
        'Else
        'ocmd2.CommandText = "select * from " & TableName & " where noid=" & NoID.ToString
        'End If
        cn.Open()
        oda2 = New SqlDataAdapter(ocmd2)
        ocmd2.CommandText = Strsql

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
        Next
        'If System.IO.File.Exists(folderLayouts &  FormName & "Grid Detil.xml") Then
        '    GridView1.RestoreLayoutFromXml(folderLayouts &  FormName & ".xml")
        'End If
        If System.IO.File.Exists(folderLayouts & FormName & " Lookup.xml") Then
            GV1.RestoreLayoutFromXml(folderLayouts & FormName & " Lookup.xml")
        End If
    End Sub

    Private Sub frmLookup_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        GV1.SaveLayoutToXml(folderLayouts & FormName & " Lookup.xml")
        ds.Dispose()
    End Sub

    Private Sub frmLookup_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        RefreshData() 
        FungsiControl.SetForm(Me)
    End Sub

    Private Sub PanelControl2_Paint(ByVal sender As System.Object, ByVal e As System.Windows.Forms.PaintEventArgs) Handles PanelControl2.Paint

    End Sub

    Private Sub GC1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles GC1.Click

    End Sub

    Private Sub GC1_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles GC1.DoubleClick
        If BolehAmbilData Then
            AmbilData()
        End If
    End Sub

    Private Sub GC1_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles GC1.KeyDown
        If e.KeyCode = Keys.Enter Then
            AmbilData()
        ElseIf e.KeyCode = Keys.Escape Then
            DialogResult = Windows.Forms.DialogResult.Cancel
            Close()
        End If
    End Sub

    Private Sub GC1_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles GC1.MouseDown
        Dim HI As New DevExpress.XtraGrid.Views.Grid.ViewInfo.GridHitInfo
        HI = GV1.CalcHitInfo(e.X, e.Y)
        If HI.InRow Then
            BolehAmbilData = True
        Else
            BolehAmbilData = False
        End If
    End Sub
End Class