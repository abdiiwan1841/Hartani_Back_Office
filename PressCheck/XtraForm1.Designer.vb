<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class XtraForm1
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(XtraForm1))
        Me.AxBarcodeX1 = New AxBARCODEXLib.AxBarcodeX
        CType(Me.AxBarcodeX1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'AxBarcodeX1
        '
        Me.AxBarcodeX1.Enabled = True
        Me.AxBarcodeX1.Location = New System.Drawing.Point(0, 0)
        Me.AxBarcodeX1.Name = "AxBarcodeX1"
        Me.AxBarcodeX1.OcxState = CType(resources.GetObject("AxBarcodeX1.OcxState"), System.Windows.Forms.AxHost.State)
        Me.AxBarcodeX1.Size = New System.Drawing.Size(300, 150)
        Me.AxBarcodeX1.TabIndex = 0
        '
        'XtraForm1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(284, 262)
        Me.Controls.Add(Me.AxBarcodeX1)
        Me.Name = "XtraForm1"
        Me.Text = "XtraForm1"
        CType(Me.AxBarcodeX1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents AxBarcodeX1 As AxBARCODEXLib.AxBarcodeX
End Class
