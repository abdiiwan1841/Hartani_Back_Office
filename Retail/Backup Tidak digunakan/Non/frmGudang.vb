Imports DevExpress.XtraEditors

Public Class frmGudang
    Inherits frmSimpleEntri


    'Dim WithEvents sgiSF As New ClassLibrary1.ctrlSimpleEntri
    'Dim WithEvents txtedit As TextEdit = sgiSF.txtEdit
    'Private Sub SimpleButton2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SimpleButton2.Click
    '    Me.Close()
    'End Sub

    'Private Sub SimpleButton1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SimpleButton1.Click
    '    If sgiSF.Simpan Then
    '        Me.Close()
    '    End If
    'End Sub

    'Private Sub frmGudang_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
    '    sgiSF.FormName = "Kota"
    '    sgiSF.isNew = True
    '    pnlEntri.Controls.Add(sgiSF)
    '    sgiSF.Dock = DockStyle.Fill
    'End Sub

    'Private Sub txtedit_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtedit.LostFocus
    '    MsgBox(sender.name)
    'End Sub
    Sub txtEdit_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        XtraMessageBox.Show(sender.name)
    End Sub
    Private Sub frmGudang_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim ctedit As DevExpress.XtraEditors.TextEdit
        
        For Each ctrl In LC1.Controls
            If ctrl.name = "txtQty" Then
                ctedit = CType(ctrl, DevExpress.XtraEditors.TextEdit)
                AddHandler ctedit.LostFocus, AddressOf txtEdit_LostFocus
            End If
        Next
    End Sub
End Class