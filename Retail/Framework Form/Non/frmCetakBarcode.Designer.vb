<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmCetakBarcode
    Inherits DevExpress.XtraEditors.XtraForm

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing AndAlso components IsNot Nothing Then
            components.Dispose()
        End If
        MyBase.Dispose(disposing)
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.PrintControl1 = New DevExpress.XtraPrinting.Control.PrintControl
        Me.SuspendLayout()
        '
        'PrintControl1
        '
        Me.PrintControl1.BackColor = System.Drawing.Color.Empty
        Me.PrintControl1.ForeColor = System.Drawing.Color.Empty
        Me.PrintControl1.IsMetric = True
        Me.PrintControl1.Location = New System.Drawing.Point(12, 12)
        Me.PrintControl1.Name = "PrintControl1"
        Me.PrintControl1.Size = New System.Drawing.Size(419, 353)
        Me.PrintControl1.TabIndex = 0
        Me.PrintControl1.TooltipFont = New System.Drawing.Font("Tahoma", 8.25!)
        '
        'frmCetakBarcode
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(443, 437)
        Me.Controls.Add(Me.PrintControl1)
        Me.Name = "frmCetakBarcode"
        Me.Text = "frmCetakBarcode"
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents PrintControl1 As DevExpress.XtraPrinting.Control.PrintControl
End Class
