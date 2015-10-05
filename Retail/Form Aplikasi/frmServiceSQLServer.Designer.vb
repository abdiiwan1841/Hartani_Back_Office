<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmServiceSQLServer
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
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmServiceSQLServer))
        Me.NotifyIcon1 = New System.Windows.Forms.NotifyIcon(Me.components)
        Me.ContextMenuStrip1 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.WindowsToolsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.WindowsExplorerToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.NotepadToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.CalculatorToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.OnScreenKeyboardToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.CommandPromptToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.TaskManagerToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.ToolStripSeparator1 = New System.Windows.Forms.ToolStripSeparator
        Me.ExitToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.ToolTipController1 = New DevExpress.Utils.ToolTipController(Me.components)
        Me.Timer1 = New System.Windows.Forms.Timer(Me.components)
        Me.ContextMenuStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'NotifyIcon1
        '
        Me.NotifyIcon1.BalloonTipIcon = System.Windows.Forms.ToolTipIcon.Info
        Me.NotifyIcon1.BalloonTipTitle = "Integration and Solution"
        Me.NotifyIcon1.ContextMenuStrip = Me.ContextMenuStrip1
        Me.NotifyIcon1.Icon = CType(resources.GetObject("NotifyIcon1.Icon"), System.Drawing.Icon)
        Me.NotifyIcon1.Text = "Intergrating module with SYS.Framework 1.2 ™"
        Me.NotifyIcon1.Visible = True
        '
        'ContextMenuStrip1
        '
        Me.ContextMenuStrip1.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ContextMenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.WindowsToolsToolStripMenuItem, Me.ToolStripSeparator1, Me.ExitToolStripMenuItem})
        Me.ContextMenuStrip1.Name = "ContextMenuStrip1"
        Me.ContextMenuStrip1.Size = New System.Drawing.Size(168, 54)
        '
        'WindowsToolsToolStripMenuItem
        '
        Me.WindowsToolsToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.WindowsExplorerToolStripMenuItem, Me.NotepadToolStripMenuItem, Me.CalculatorToolStripMenuItem, Me.OnScreenKeyboardToolStripMenuItem, Me.CommandPromptToolStripMenuItem, Me.TaskManagerToolStripMenuItem})
        Me.WindowsToolsToolStripMenuItem.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.WindowsToolsToolStripMenuItem.Name = "WindowsToolsToolStripMenuItem"
        Me.WindowsToolsToolStripMenuItem.Size = New System.Drawing.Size(167, 22)
        Me.WindowsToolsToolStripMenuItem.Text = "&Tools"
        '
        'WindowsExplorerToolStripMenuItem
        '
        Me.WindowsExplorerToolStripMenuItem.Name = "WindowsExplorerToolStripMenuItem"
        Me.WindowsExplorerToolStripMenuItem.Size = New System.Drawing.Size(184, 22)
        Me.WindowsExplorerToolStripMenuItem.Text = "&Windows Explorer"
        '
        'NotepadToolStripMenuItem
        '
        Me.NotepadToolStripMenuItem.Name = "NotepadToolStripMenuItem"
        Me.NotepadToolStripMenuItem.Size = New System.Drawing.Size(184, 22)
        Me.NotepadToolStripMenuItem.Text = "&Notepad"
        '
        'CalculatorToolStripMenuItem
        '
        Me.CalculatorToolStripMenuItem.Name = "CalculatorToolStripMenuItem"
        Me.CalculatorToolStripMenuItem.Size = New System.Drawing.Size(184, 22)
        Me.CalculatorToolStripMenuItem.Text = "C&alculator"
        '
        'OnScreenKeyboardToolStripMenuItem
        '
        Me.OnScreenKeyboardToolStripMenuItem.Name = "OnScreenKeyboardToolStripMenuItem"
        Me.OnScreenKeyboardToolStripMenuItem.Size = New System.Drawing.Size(184, 22)
        Me.OnScreenKeyboardToolStripMenuItem.Text = "&On Screen Keyboard"
        '
        'CommandPromptToolStripMenuItem
        '
        Me.CommandPromptToolStripMenuItem.Name = "CommandPromptToolStripMenuItem"
        Me.CommandPromptToolStripMenuItem.Size = New System.Drawing.Size(184, 22)
        Me.CommandPromptToolStripMenuItem.Text = "&Command Prompt"
        '
        'TaskManagerToolStripMenuItem
        '
        Me.TaskManagerToolStripMenuItem.Name = "TaskManagerToolStripMenuItem"
        Me.TaskManagerToolStripMenuItem.Size = New System.Drawing.Size(184, 22)
        Me.TaskManagerToolStripMenuItem.Text = "&Task Manager"
        '
        'ToolStripSeparator1
        '
        Me.ToolStripSeparator1.Name = "ToolStripSeparator1"
        Me.ToolStripSeparator1.Size = New System.Drawing.Size(164, 6)
        '
        'ExitToolStripMenuItem
        '
        Me.ExitToolStripMenuItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
        Me.ExitToolStripMenuItem.Name = "ExitToolStripMenuItem"
        Me.ExitToolStripMenuItem.ShortcutKeys = CType(((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.Shift) _
                    Or System.Windows.Forms.Keys.Q), System.Windows.Forms.Keys)
        Me.ExitToolStripMenuItem.Size = New System.Drawing.Size(167, 22)
        Me.ExitToolStripMenuItem.Text = "&Exit"
        Me.ExitToolStripMenuItem.ToolTipText = "Keluar dari program."
        '
        'ToolTipController1
        '
        Me.ToolTipController1.ToolTipLocation = DevExpress.Utils.ToolTipLocation.TopRight
        Me.ToolTipController1.ToolTipType = DevExpress.Utils.ToolTipType.SuperTip
        '
        'Timer1
        '
        Me.Timer1.Interval = 1000
        '
        'frmServiceSQLServer
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(211, 58)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.Name = "frmServiceSQLServer"
        Me.Opacity = 0
        Me.ShowIcon = False
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "frmService"
        Me.ContextMenuStrip1.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents NotifyIcon1 As System.Windows.Forms.NotifyIcon
    Friend WithEvents ToolTipController1 As DevExpress.Utils.ToolTipController
    Friend WithEvents Timer1 As System.Windows.Forms.Timer
    Friend WithEvents ContextMenuStrip1 As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents ExitToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents WindowsToolsToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents CommandPromptToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents CalculatorToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator1 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents WindowsExplorerToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents TaskManagerToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents NotepadToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents OnScreenKeyboardToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
End Class
