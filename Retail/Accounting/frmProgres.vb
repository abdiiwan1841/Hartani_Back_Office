Public Class frmProgres
    Inherits DevExpress.XtraEditors.XtraForm

#Region " Windows Form Designer generated code "

    Public Sub New()
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()

        'Add any initialization after the InitializeComponent() call

    End Sub

    'Form overrides dispose to clean up the component list.
    Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing Then
            If Not (components Is Nothing) Then
                components.Dispose()
            End If
        End If
        MyBase.Dispose(disposing)
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    Friend WithEvents ProgressBarControl1 As DevExpress.XtraEditors.ProgressBarControl
    Friend WithEvents lbitem As System.Windows.Forms.Label
    Friend WithEvents lbtime As System.Windows.Forms.Label
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.ProgressBarControl1 = New DevExpress.XtraEditors.ProgressBarControl
        Me.lbitem = New System.Windows.Forms.Label
        Me.lbtime = New System.Windows.Forms.Label
        CType(Me.ProgressBarControl1.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'ProgressBarControl1
        '
        Me.ProgressBarControl1.Dock = System.Windows.Forms.DockStyle.Top
        Me.ProgressBarControl1.Location = New System.Drawing.Point(0, 0)
        Me.ProgressBarControl1.Name = "ProgressBarControl1"
        Me.ProgressBarControl1.Properties.ShowTitle = True
        Me.ProgressBarControl1.Size = New System.Drawing.Size(408, 24)
        Me.ProgressBarControl1.TabIndex = 0
        '
        'lbitem
        '
        Me.lbitem.BackColor = System.Drawing.Color.Transparent
        Me.lbitem.Dock = System.Windows.Forms.DockStyle.Top
        Me.lbitem.ForeColor = System.Drawing.Color.Navy
        Me.lbitem.Location = New System.Drawing.Point(0, 24)
        Me.lbitem.Name = "lbitem"
        Me.lbitem.Size = New System.Drawing.Size(408, 16)
        Me.lbitem.TabIndex = 1
        Me.lbitem.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lbtime
        '
        Me.lbtime.BackColor = System.Drawing.Color.Transparent
        Me.lbtime.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.lbtime.ForeColor = System.Drawing.Color.Navy
        Me.lbtime.Location = New System.Drawing.Point(0, 46)
        Me.lbtime.Name = "lbtime"
        Me.lbtime.Size = New System.Drawing.Size(408, 16)
        Me.lbtime.TabIndex = 2
        Me.lbtime.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'frmProgres
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 14)
        Me.ClientSize = New System.Drawing.Size(408, 62)
        Me.ControlBox = False
        Me.Controls.Add(Me.lbtime)
        Me.Controls.Add(Me.lbitem)
        Me.Controls.Add(Me.ProgressBarControl1)
        Me.Name = "frmProgres"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = ".:: Progress... ::."
        CType(Me.ProgressBarControl1.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

#End Region

 
End Class

