Imports DevExpress.XtraEditors
Imports DevExpress.Utils
Imports DevExpress.XtraGrid
Imports DevExpress.XtraLayout
Imports System.Data
Imports System.Data.SqlClient
Imports System.Data.SQLite

Public Class frmSimpleEntri2
    Inherits frmSimpleEntri
    'Public WithEvents LC2 As LayoutControl
    'Public WithEvents LkEdit2 As SearchLookUpEdit
    'Public WithEvents txtEdit2 As TextEdit
    'Public WithEvents txtCalc2 As CalcEdit
    Dim isProsesLoad As Boolean = True

    Private Sub frmSimpleEntri2_Shown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Shown
        For Each ctl As Control In LC1.Controls
            If TypeOf ctl Is SearchLookUpEdit Then
                AddHandler lkEdit.EditValueChanged, AddressOf lkEdit_EditValueChanged
                'AddHandler lkEdit.ButtonClick, AddressOf lkEdit_ButtonClick
                'AddHandler lkEdit.Click, AddressOf lkEdit_Click
            End If
        Next
        isProsesLoad = False
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
End Class
