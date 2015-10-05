Imports System.Data.SqlClient
Imports DevExpress.XtraTreeList.Nodes
Imports DevExpress.XtraTreeList
Imports DevExpress.XtraEditors

Public Class frmMenu

    Dim ds As New DataSet
    Dim BS As New BindingSource
    Private Sub SimpleButton2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SimpleButton2.Click
        Me.Close()
    End Sub
    Sub RefreshData()
        Try
            Dim curentcursor As Cursor = Windows.Forms.Cursor.Current
            Windows.Forms.Cursor.Current = Cursors.WaitCursor
            Dim oConn As SqlConnection
            Dim ocmd As SqlCommand
            Dim oda As SqlDataAdapter
            Dim strsql As String
            strsql = "Select mmenu.* from mmenu "
            oConn = New SqlConnection(StrKonSql)
            ocmd = New SqlCommand(strsql, oConn)
            oda = New SqlDataAdapter(ocmd)
            If ds.Tables("Menu") Is Nothing Then
            Else
                ds.Tables("Menu").Clear()
            End If
            oda.Fill(ds, "Menu")
            BS.DataSource = ds.Tables("Menu")
            TreeList1.DataSource = BS.DataSource
            TreeList1.ParentFieldName = "IDParent"
            TreeList1.KeyFieldName = "NoID"
            oConn.Open()
            ocmd.Dispose()
            oConn.Close()
            oConn.Dispose()
            Windows.Forms.Cursor.Current = curentcursor
            TreeList1.ExpandAll()
            cmdApply.Enabled = True
        Catch ex As Exception
        End Try
    End Sub

    Private Sub SimpleButton1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SimpleButton1.Click
        RefreshData()
    End Sub

    Private Sub SimpleButton3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdApply.Click
        Try
            Dim ODC As SqlCommandBuilder
            Dim SQLconnect As New SqlConnection()
            Dim SQLcommand As SqlCommand
            Dim oda As SqlDataAdapter
            SQLconnect.ConnectionString = StrKonSql
            SQLconnect.Open()
            SQLcommand = SQLconnect.CreateCommand
            SQLcommand.CommandText = "SELECT * FROM mmenu"
            oda = New SqlDataAdapter(SQLcommand)
            ' oda.TableMappings.Add("Table", "Contacts")


            ' If isNew Then
            'Me.Validate()
            'BS.EndEdit()
            ODC = New SqlCommandBuilder(oda)
            oda.Update(ds.Tables("Menu"))
            '   End If

            SQLcommand.Dispose()
            SQLconnect.Close()
            SQLconnect.Dispose()
            oda.Dispose()
            XtraMessageBox.Show("Data Tersimpan")
            'Me.Close()

        Catch ex As Exception
            XtraMessageBox.Show("ada kesalahan :" & vbCr & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try

    End Sub

    Private Sub BarButtonItem3_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem3.ItemClick
        Dim frheader As New frmGenerateFormH
        If frheader.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
            'txtformname.Text = frheader.FormName
        End If
        frheader.Dispose()
    End Sub

    Private Sub frmMenu_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        RefreshData()
        FungsiControl.SetForm(Me)
    End Sub




    Private Sub TreeList1_DragDrop(ByVal sender As Object, ByVal e As System.Windows.Forms.DragEventArgs) Handles TreeList1.DragDrop
        TreeList1.SuspendLayout()
        Dim hi As TreeListHitInfo = TreeList1.CalcHitInfo(TreeList1.PointToClient(New Point(e.X, e.Y)))
        If hi Is Nothing Then
            Exit Sub
        End If
        Dim parentNode As TreeListNode = hi.Node.ParentNode
        If Not parentNode Is Nothing AndAlso (e.KeyState And 4) <> 0 Then
            Dim index As Integer = -1
            If Not parentNode.ParentNode Is Nothing Then
                index = parentNode.ParentNode.Nodes.IndexOf(parentNode)
            End If
            For i = 0 To TreeList1.Selection.Count - 1
                TreeList1.MoveNode(TreeList1.Selection(i), parentNode.ParentNode)
                TreeList1.SetNodeIndex(TreeList1.Selection(i), index)
            Next i


        End If
        TreeList1.ResumeLayout()
    End Sub

    Private Sub TreeList1_FocusedNodeChanged(ByVal sender As System.Object, ByVal e As DevExpress.XtraTreeList.FocusedNodeChangedEventArgs) Handles TreeList1.FocusedNodeChanged

    End Sub
End Class