<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form1
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Form1))
        Me.AxBarcodeX1 = New AxBARCODEXLib.AxBarcodeX
        Me.TextBox1 = New System.Windows.Forms.TextBox
        CType(Me.AxBarcodeX1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'AxBarcodeX1
        '
        Me.AxBarcodeX1.Dock = System.Windows.Forms.DockStyle.Top
        Me.AxBarcodeX1.Enabled = True
        Me.AxBarcodeX1.Location = New System.Drawing.Point(0, 0)
        Me.AxBarcodeX1.Name = "AxBarcodeX1"
        Me.AxBarcodeX1.OcxState = CType(resources.GetObject("AxBarcodeX1.OcxState"), System.Windows.Forms.AxHost.State)
        Me.AxBarcodeX1.Size = New System.Drawing.Size(484, 107)
        Me.AxBarcodeX1.TabIndex = 0
        '
        'TextBox1
        '
        Me.TextBox1.Dock = System.Windows.Forms.DockStyle.Top
        Me.TextBox1.Location = New System.Drawing.Point(0, 107)
        Me.TextBox1.Name = "TextBox1"
        Me.TextBox1.Size = New System.Drawing.Size(484, 20)
        Me.TextBox1.TabIndex = 1
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(484, 130)
        Me.Controls.Add(Me.TextBox1)
        Me.Controls.Add(Me.AxBarcodeX1)
        Me.Name = "Form1"
        Me.Text = "Form1"
        CType(Me.AxBarcodeX1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents AxBarcodeX1 As AxBARCODEXLib.AxBarcodeX
    Friend WithEvents TextBox1 As System.Windows.Forms.TextBox
End Class
