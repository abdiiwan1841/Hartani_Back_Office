Public Class Form1

    Private Sub TextBox1_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox1.TextChanged
        AxBarcodeX1.Caption = TextBox1.Text
    End Sub
End Class