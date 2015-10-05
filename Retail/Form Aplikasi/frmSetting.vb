Imports System
Imports System.Windows.Forms
Imports DevExpress.XtraEditors
Imports System.Data
Imports System.Data.Odbc
Imports VPoint.Ini
Public Class frmSetting
    Inherits DevExpress.XtraEditors.XtraForm

#Region " Windows Form Designer generated code "

    Public Sub New()
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()

        'Add any initialization after the InitializeComponent() : setme call

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
    Friend WithEvents ToolTipController1 As DevExpress.Utils.ToolTipController
    Friend WithEvents PanelControl1 As DevExpress.XtraEditors.PanelControl
    Friend WithEvents PanelControl2 As DevExpress.XtraEditors.PanelControl
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents sbClose As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents SimpleButton1 As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents XtraTabPage1 As DevExpress.XtraTab.XtraTabPage
    Friend WithEvents chkloginwindows As DevExpress.XtraEditors.CheckEdit
    Friend WithEvents txt4 As DevExpress.XtraEditors.TextEdit
    Friend WithEvents txt3 As DevExpress.XtraEditors.TextEdit
    Friend WithEvents txt2 As DevExpress.XtraEditors.TextEdit
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents chkLoginWindowsPos As DevExpress.XtraEditors.CheckEdit
    Friend WithEvents txtPos4 As DevExpress.XtraEditors.TextEdit
    Friend WithEvents txtPos3 As DevExpress.XtraEditors.TextEdit
    Friend WithEvents txtPos1 As DevExpress.XtraEditors.TextEdit
    Friend WithEvents txtPos2 As DevExpress.XtraEditors.TextEdit
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents Label12 As System.Windows.Forms.Label
    Friend WithEvents Label13 As System.Windows.Forms.Label
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents txt1 As DevExpress.XtraEditors.ButtonEdit
    Friend WithEvents LayoutControl1 As DevExpress.XtraLayout.LayoutControl
    Friend WithEvents LayoutControlGroup1 As DevExpress.XtraLayout.LayoutControlGroup
    Friend WithEvents LayoutControlItem1 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents LayoutControlItem2 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents LayoutControlItem3 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents txtTimeOut As DevExpress.XtraEditors.TextEdit
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents TextEdit1 As DevExpress.XtraEditors.ButtonEdit
    Friend WithEvents txtPOS5 As DevExpress.XtraEditors.TextEdit
    Friend WithEvents Label14 As System.Windows.Forms.Label
    Friend WithEvents XtraTabPage2 As DevExpress.XtraTab.XtraTabPage
    Friend WithEvents Label15 As System.Windows.Forms.Label
    Friend WithEvents txtDllReport As DevExpress.XtraEditors.ComboBoxEdit
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents txtODBC4 As DevExpress.XtraEditors.TextEdit
    Friend WithEvents txtODBC3 As DevExpress.XtraEditors.TextEdit
    Friend WithEvents txtODBC2 As DevExpress.XtraEditors.TextEdit
    Friend WithEvents Label16 As System.Windows.Forms.Label
    Friend WithEvents Label17 As System.Windows.Forms.Label
    Friend WithEvents Label18 As System.Windows.Forms.Label
    Friend WithEvents Label19 As System.Windows.Forms.Label
    Friend WithEvents txtODBC1 As DevExpress.XtraEditors.ButtonEdit
    Friend WithEvents Label20 As System.Windows.Forms.Label
    Friend WithEvents Label21 As System.Windows.Forms.Label
    Friend WithEvents XtraTabControl1 As DevExpress.XtraTab.XtraTabControl
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim SerializableAppearanceObject1 As DevExpress.Utils.SerializableAppearanceObject = New DevExpress.Utils.SerializableAppearanceObject
        Me.ToolTipController1 = New DevExpress.Utils.ToolTipController(Me.components)
        Me.PanelControl1 = New DevExpress.XtraEditors.PanelControl
        Me.Label1 = New System.Windows.Forms.Label
        Me.PanelControl2 = New DevExpress.XtraEditors.PanelControl
        Me.SimpleButton1 = New DevExpress.XtraEditors.SimpleButton
        Me.sbClose = New DevExpress.XtraEditors.SimpleButton
        Me.XtraTabControl1 = New DevExpress.XtraTab.XtraTabControl
        Me.XtraTabPage1 = New DevExpress.XtraTab.XtraTabPage
        Me.txtPOS5 = New DevExpress.XtraEditors.TextEdit
        Me.Label14 = New System.Windows.Forms.Label
        Me.txtTimeOut = New DevExpress.XtraEditors.TextEdit
        Me.Label2 = New System.Windows.Forms.Label
        Me.txt1 = New DevExpress.XtraEditors.ButtonEdit
        Me.Label9 = New System.Windows.Forms.Label
        Me.chkLoginWindowsPos = New DevExpress.XtraEditors.CheckEdit
        Me.txtPos4 = New DevExpress.XtraEditors.TextEdit
        Me.txtPos3 = New DevExpress.XtraEditors.TextEdit
        Me.txtPos1 = New DevExpress.XtraEditors.TextEdit
        Me.txtPos2 = New DevExpress.XtraEditors.TextEdit
        Me.Label10 = New System.Windows.Forms.Label
        Me.Label11 = New System.Windows.Forms.Label
        Me.Label12 = New System.Windows.Forms.Label
        Me.Label13 = New System.Windows.Forms.Label
        Me.Label8 = New System.Windows.Forms.Label
        Me.chkloginwindows = New DevExpress.XtraEditors.CheckEdit
        Me.txt4 = New DevExpress.XtraEditors.TextEdit
        Me.txt3 = New DevExpress.XtraEditors.TextEdit
        Me.txt2 = New DevExpress.XtraEditors.TextEdit
        Me.Label7 = New System.Windows.Forms.Label
        Me.Label6 = New System.Windows.Forms.Label
        Me.Label5 = New System.Windows.Forms.Label
        Me.Label3 = New System.Windows.Forms.Label
        Me.TextEdit1 = New DevExpress.XtraEditors.ButtonEdit
        Me.XtraTabPage2 = New DevExpress.XtraTab.XtraTabPage
        Me.txtODBC4 = New DevExpress.XtraEditors.TextEdit
        Me.txtODBC3 = New DevExpress.XtraEditors.TextEdit
        Me.txtODBC2 = New DevExpress.XtraEditors.TextEdit
        Me.Label16 = New System.Windows.Forms.Label
        Me.Label17 = New System.Windows.Forms.Label
        Me.Label18 = New System.Windows.Forms.Label
        Me.Label19 = New System.Windows.Forms.Label
        Me.Label15 = New System.Windows.Forms.Label
        Me.txtDllReport = New DevExpress.XtraEditors.ComboBoxEdit
        Me.Label4 = New System.Windows.Forms.Label
        Me.txtODBC1 = New DevExpress.XtraEditors.ButtonEdit
        Me.LayoutControl1 = New DevExpress.XtraLayout.LayoutControl
        Me.LayoutControlGroup1 = New DevExpress.XtraLayout.LayoutControlGroup
        Me.LayoutControlItem1 = New DevExpress.XtraLayout.LayoutControlItem
        Me.LayoutControlItem2 = New DevExpress.XtraLayout.LayoutControlItem
        Me.LayoutControlItem3 = New DevExpress.XtraLayout.LayoutControlItem
        Me.Label20 = New System.Windows.Forms.Label
        Me.Label21 = New System.Windows.Forms.Label
        CType(Me.PanelControl1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.PanelControl1.SuspendLayout()
        CType(Me.PanelControl2, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.PanelControl2.SuspendLayout()
        CType(Me.XtraTabControl1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.XtraTabControl1.SuspendLayout()
        Me.XtraTabPage1.SuspendLayout()
        CType(Me.txtPOS5.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtTimeOut.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txt1.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.chkLoginWindowsPos.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtPos4.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtPos3.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtPos1.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtPos2.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.chkloginwindows.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txt4.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txt3.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txt2.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.TextEdit1.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.XtraTabPage2.SuspendLayout()
        CType(Me.txtODBC4.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtODBC3.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtODBC2.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtDllReport.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtODBC1.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControl1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.LayoutControl1.SuspendLayout()
        CType(Me.LayoutControlGroup1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem3, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'PanelControl1
        '
        Me.PanelControl1.Appearance.BackColor = System.Drawing.Color.Transparent
        Me.PanelControl1.Appearance.Options.UseBackColor = True
        Me.PanelControl1.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder
        Me.PanelControl1.Controls.Add(Me.Label1)
        Me.PanelControl1.Location = New System.Drawing.Point(12, 12)
        Me.PanelControl1.LookAndFeel.SkinName = "Glass Oceans"
        Me.PanelControl1.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.UltraFlat
        Me.PanelControl1.Name = "PanelControl1"
        Me.PanelControl1.Size = New System.Drawing.Size(627, 67)
        Me.PanelControl1.TabIndex = 0
        '
        'Label1
        '
        Me.Label1.BackColor = System.Drawing.Color.Transparent
        Me.Label1.Font = New System.Drawing.Font("Tahoma", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.ForeColor = System.Drawing.SystemColors.HotTrack
        Me.Label1.Location = New System.Drawing.Point(20, 10)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(273, 40)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "FORM SETTING " & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "KONEKSI APLIKASI"
        '
        'PanelControl2
        '
        Me.PanelControl2.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder
        Me.PanelControl2.Controls.Add(Me.SimpleButton1)
        Me.PanelControl2.Controls.Add(Me.sbClose)
        Me.PanelControl2.Location = New System.Drawing.Point(12, 372)
        Me.PanelControl2.LookAndFeel.SkinName = "Liquid Sky"
        Me.PanelControl2.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.UltraFlat
        Me.PanelControl2.LookAndFeel.UseDefaultLookAndFeel = False
        Me.PanelControl2.Name = "PanelControl2"
        Me.PanelControl2.Size = New System.Drawing.Size(627, 42)
        Me.PanelControl2.TabIndex = 2
        '
        'SimpleButton1
        '
        Me.SimpleButton1.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.SimpleButton1.Appearance.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.SimpleButton1.Appearance.Options.UseFont = True
        Me.SimpleButton1.Location = New System.Drawing.Point(418, 11)
        Me.SimpleButton1.Name = "SimpleButton1"
        Me.SimpleButton1.Size = New System.Drawing.Size(100, 28)
        Me.SimpleButton1.TabIndex = 0
        Me.SimpleButton1.Text = "&Simpan"
        '
        'sbClose
        '
        Me.sbClose.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.sbClose.Appearance.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.sbClose.Appearance.Options.UseFont = True
        Me.sbClose.Location = New System.Drawing.Point(522, 11)
        Me.sbClose.Name = "sbClose"
        Me.sbClose.Size = New System.Drawing.Size(100, 28)
        Me.sbClose.TabIndex = 1
        Me.sbClose.Text = "&Tutup"
        '
        'XtraTabControl1
        '
        Me.XtraTabControl1.Font = New System.Drawing.Font("Tahoma", 9.75!)
        Me.XtraTabControl1.Location = New System.Drawing.Point(12, 83)
        Me.XtraTabControl1.Name = "XtraTabControl1"
        Me.XtraTabControl1.SelectedTabPage = Me.XtraTabPage1
        Me.XtraTabControl1.Size = New System.Drawing.Size(627, 285)
        Me.XtraTabControl1.TabIndex = 1
        Me.XtraTabControl1.TabPages.AddRange(New DevExpress.XtraTab.XtraTabPage() {Me.XtraTabPage1, Me.XtraTabPage2})
        '
        'XtraTabPage1
        '
        Me.XtraTabPage1.Controls.Add(Me.Label21)
        Me.XtraTabPage1.Controls.Add(Me.Label20)
        Me.XtraTabPage1.Controls.Add(Me.txtPOS5)
        Me.XtraTabPage1.Controls.Add(Me.Label14)
        Me.XtraTabPage1.Controls.Add(Me.txtTimeOut)
        Me.XtraTabPage1.Controls.Add(Me.Label2)
        Me.XtraTabPage1.Controls.Add(Me.txt1)
        Me.XtraTabPage1.Controls.Add(Me.Label9)
        Me.XtraTabPage1.Controls.Add(Me.chkLoginWindowsPos)
        Me.XtraTabPage1.Controls.Add(Me.txtPos4)
        Me.XtraTabPage1.Controls.Add(Me.txtPos3)
        Me.XtraTabPage1.Controls.Add(Me.txtPos1)
        Me.XtraTabPage1.Controls.Add(Me.txtPos2)
        Me.XtraTabPage1.Controls.Add(Me.Label10)
        Me.XtraTabPage1.Controls.Add(Me.Label11)
        Me.XtraTabPage1.Controls.Add(Me.Label12)
        Me.XtraTabPage1.Controls.Add(Me.Label13)
        Me.XtraTabPage1.Controls.Add(Me.Label8)
        Me.XtraTabPage1.Controls.Add(Me.chkloginwindows)
        Me.XtraTabPage1.Controls.Add(Me.txt4)
        Me.XtraTabPage1.Controls.Add(Me.txt3)
        Me.XtraTabPage1.Controls.Add(Me.txt2)
        Me.XtraTabPage1.Controls.Add(Me.Label7)
        Me.XtraTabPage1.Controls.Add(Me.Label6)
        Me.XtraTabPage1.Controls.Add(Me.Label5)
        Me.XtraTabPage1.Controls.Add(Me.Label3)
        Me.XtraTabPage1.Controls.Add(Me.TextEdit1)
        Me.XtraTabPage1.Font = New System.Drawing.Font("Tahoma", 9.75!)
        Me.XtraTabPage1.Name = "XtraTabPage1"
        Me.XtraTabPage1.Size = New System.Drawing.Size(621, 259)
        Me.XtraTabPage1.Text = "Koneksi Data"
        '
        'txtPOS5
        '
        Me.txtPOS5.EditValue = ""
        Me.txtPOS5.EnterMoveNextControl = True
        Me.txtPOS5.Location = New System.Drawing.Point(402, 131)
        Me.txtPOS5.Name = "txtPOS5"
        Me.txtPOS5.Properties.Appearance.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtPOS5.Properties.Appearance.Options.UseFont = True
        Me.txtPOS5.Properties.Mask.EditMask = "##0"
        Me.txtPOS5.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric
        Me.txtPOS5.Properties.Mask.UseMaskAsDisplayFormat = True
        Me.txtPOS5.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.txtPOS5.Size = New System.Drawing.Size(59, 22)
        Me.txtPOS5.TabIndex = 22
        '
        'Label14
        '
        Me.Label14.AutoSize = True
        Me.Label14.BackColor = System.Drawing.Color.Transparent
        Me.Label14.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label14.ForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.Label14.Location = New System.Drawing.Point(327, 137)
        Me.Label14.Name = "Label14"
        Me.Label14.Size = New System.Drawing.Size(57, 16)
        Me.Label14.TabIndex = 21
        Me.Label14.Text = "TimeOut"
        '
        'txtTimeOut
        '
        Me.txtTimeOut.EditValue = ""
        Me.txtTimeOut.EnterMoveNextControl = True
        Me.txtTimeOut.Location = New System.Drawing.Point(95, 131)
        Me.txtTimeOut.Name = "txtTimeOut"
        Me.txtTimeOut.Properties.Appearance.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtTimeOut.Properties.Appearance.Options.UseFont = True
        Me.txtTimeOut.Properties.Mask.EditMask = "##0"
        Me.txtTimeOut.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric
        Me.txtTimeOut.Properties.Mask.UseMaskAsDisplayFormat = True
        Me.txtTimeOut.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.txtTimeOut.Size = New System.Drawing.Size(71, 22)
        Me.txtTimeOut.TabIndex = 10
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.BackColor = System.Drawing.Color.Transparent
        Me.Label2.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.ForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.Label2.Location = New System.Drawing.Point(11, 134)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(57, 16)
        Me.Label2.TabIndex = 9
        Me.Label2.Text = "TimeOut"
        '
        'txt1
        '
        Me.txt1.EnterMoveNextControl = True
        Me.txt1.Location = New System.Drawing.Point(95, 19)
        Me.txt1.Name = "txt1"
        Me.txt1.Properties.Appearance.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txt1.Properties.Appearance.Options.UseFont = True
        Me.txt1.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton})
        Me.txt1.Size = New System.Drawing.Size(197, 22)
        Me.txt1.TabIndex = 1
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.BackColor = System.Drawing.Color.Transparent
        Me.Label9.Font = New System.Drawing.Font("Tahoma", 9.75!, CType((System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Underline), System.Drawing.FontStyle), System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label9.ForeColor = System.Drawing.Color.Maroon
        Me.Label9.Location = New System.Drawing.Point(327, 0)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(148, 16)
        Me.Label9.TabIndex = 12
        Me.Label9.Text = "Server Lain (jika ada)"
        '
        'chkLoginWindowsPos
        '
        Me.chkLoginWindowsPos.Location = New System.Drawing.Point(400, 159)
        Me.chkLoginWindowsPos.Name = "chkLoginWindowsPos"
        Me.chkLoginWindowsPos.Properties.Appearance.BackColor = System.Drawing.Color.Transparent
        Me.chkLoginWindowsPos.Properties.Appearance.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkLoginWindowsPos.Properties.Appearance.ForeColor = System.Drawing.SystemColors.ActiveCaption
        Me.chkLoginWindowsPos.Properties.Appearance.Options.UseBackColor = True
        Me.chkLoginWindowsPos.Properties.Appearance.Options.UseFont = True
        Me.chkLoginWindowsPos.Properties.Appearance.Options.UseForeColor = True
        Me.chkLoginWindowsPos.Properties.Caption = "Menggunakan Login Windows"
        Me.chkLoginWindowsPos.Size = New System.Drawing.Size(217, 21)
        Me.chkLoginWindowsPos.TabIndex = 23
        '
        'txtPos4
        '
        Me.txtPos4.EditValue = ""
        Me.txtPos4.EnterMoveNextControl = True
        Me.txtPos4.Location = New System.Drawing.Point(402, 103)
        Me.txtPos4.Name = "txtPos4"
        Me.txtPos4.Properties.Appearance.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtPos4.Properties.Appearance.Options.UseFont = True
        Me.txtPos4.Properties.PasswordChar = Global.Microsoft.VisualBasic.ChrW(42)
        Me.txtPos4.Size = New System.Drawing.Size(204, 22)
        Me.txtPos4.TabIndex = 20
        '
        'txtPos3
        '
        Me.txtPos3.EditValue = ""
        Me.txtPos3.EnterMoveNextControl = True
        Me.txtPos3.Location = New System.Drawing.Point(402, 75)
        Me.txtPos3.Name = "txtPos3"
        Me.txtPos3.Properties.Appearance.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtPos3.Properties.Appearance.Options.UseFont = True
        Me.txtPos3.Size = New System.Drawing.Size(204, 22)
        Me.txtPos3.TabIndex = 18
        '
        'txtPos1
        '
        Me.txtPos1.EditValue = ""
        Me.txtPos1.EnterMoveNextControl = True
        Me.txtPos1.Location = New System.Drawing.Point(402, 19)
        Me.txtPos1.Name = "txtPos1"
        Me.txtPos1.Properties.Appearance.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtPos1.Properties.Appearance.Options.UseFont = True
        Me.txtPos1.Size = New System.Drawing.Size(204, 22)
        Me.txtPos1.TabIndex = 14
        '
        'txtPos2
        '
        Me.txtPos2.EditValue = ""
        Me.txtPos2.EnterMoveNextControl = True
        Me.txtPos2.Location = New System.Drawing.Point(402, 47)
        Me.txtPos2.Name = "txtPos2"
        Me.txtPos2.Properties.Appearance.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtPos2.Properties.Appearance.Options.UseFont = True
        Me.txtPos2.Size = New System.Drawing.Size(204, 22)
        Me.txtPos2.TabIndex = 16
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.BackColor = System.Drawing.Color.Transparent
        Me.Label10.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label10.ForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.Label10.Location = New System.Drawing.Point(327, 106)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(63, 16)
        Me.Label10.TabIndex = 19
        Me.Label10.Text = "Password"
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.BackColor = System.Drawing.Color.Transparent
        Me.Label11.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label11.ForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.Label11.Location = New System.Drawing.Point(327, 78)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(66, 16)
        Me.Label11.TabIndex = 17
        Me.Label11.Text = "Username"
        '
        'Label12
        '
        Me.Label12.AutoSize = True
        Me.Label12.BackColor = System.Drawing.Color.Transparent
        Me.Label12.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label12.ForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.Label12.Location = New System.Drawing.Point(327, 50)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(61, 16)
        Me.Label12.TabIndex = 15
        Me.Label12.Text = "Database"
        '
        'Label13
        '
        Me.Label13.AutoSize = True
        Me.Label13.BackColor = System.Drawing.Color.Transparent
        Me.Label13.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label13.ForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.Label13.Location = New System.Drawing.Point(327, 22)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(46, 16)
        Me.Label13.TabIndex = 13
        Me.Label13.Text = "Server"
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.BackColor = System.Drawing.Color.Transparent
        Me.Label8.Font = New System.Drawing.Font("Tahoma", 9.75!, CType((System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Underline), System.Drawing.FontStyle), System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label8.ForeColor = System.Drawing.Color.Maroon
        Me.Label8.Location = New System.Drawing.Point(11, 0)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(213, 16)
        Me.Label8.TabIndex = 0
        Me.Label8.Text = "Server Accounting/Operasional"
        '
        'chkloginwindows
        '
        Me.chkloginwindows.Location = New System.Drawing.Point(93, 159)
        Me.chkloginwindows.Name = "chkloginwindows"
        Me.chkloginwindows.Properties.Appearance.BackColor = System.Drawing.Color.Transparent
        Me.chkloginwindows.Properties.Appearance.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkloginwindows.Properties.Appearance.ForeColor = System.Drawing.SystemColors.ActiveCaption
        Me.chkloginwindows.Properties.Appearance.Options.UseBackColor = True
        Me.chkloginwindows.Properties.Appearance.Options.UseFont = True
        Me.chkloginwindows.Properties.Appearance.Options.UseForeColor = True
        Me.chkloginwindows.Properties.Caption = "Menggunakan Login Windows"
        Me.chkloginwindows.Size = New System.Drawing.Size(212, 21)
        Me.chkloginwindows.TabIndex = 11
        '
        'txt4
        '
        Me.txt4.EditValue = ""
        Me.txt4.EnterMoveNextControl = True
        Me.txt4.Location = New System.Drawing.Point(95, 103)
        Me.txt4.Name = "txt4"
        Me.txt4.Properties.Appearance.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txt4.Properties.Appearance.Options.UseFont = True
        Me.txt4.Properties.PasswordChar = Global.Microsoft.VisualBasic.ChrW(42)
        Me.txt4.Size = New System.Drawing.Size(197, 22)
        Me.txt4.TabIndex = 8
        '
        'txt3
        '
        Me.txt3.EditValue = ""
        Me.txt3.EnterMoveNextControl = True
        Me.txt3.Location = New System.Drawing.Point(95, 75)
        Me.txt3.Name = "txt3"
        Me.txt3.Properties.Appearance.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txt3.Properties.Appearance.Options.UseFont = True
        Me.txt3.Size = New System.Drawing.Size(197, 22)
        Me.txt3.TabIndex = 6
        '
        'txt2
        '
        Me.txt2.EditValue = ""
        Me.txt2.EnterMoveNextControl = True
        Me.txt2.Location = New System.Drawing.Point(95, 47)
        Me.txt2.Name = "txt2"
        Me.txt2.Properties.Appearance.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txt2.Properties.Appearance.Options.UseFont = True
        Me.txt2.Size = New System.Drawing.Size(197, 22)
        Me.txt2.TabIndex = 4
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.BackColor = System.Drawing.Color.Transparent
        Me.Label7.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label7.ForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.Label7.Location = New System.Drawing.Point(11, 106)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(63, 16)
        Me.Label7.TabIndex = 7
        Me.Label7.Text = "Password"
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.BackColor = System.Drawing.Color.Transparent
        Me.Label6.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label6.ForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.Label6.Location = New System.Drawing.Point(11, 78)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(66, 16)
        Me.Label6.TabIndex = 5
        Me.Label6.Text = "Username"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.BackColor = System.Drawing.Color.Transparent
        Me.Label5.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.ForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.Label5.Location = New System.Drawing.Point(11, 50)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(61, 16)
        Me.Label5.TabIndex = 3
        Me.Label5.Text = "Database"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.BackColor = System.Drawing.Color.Transparent
        Me.Label3.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.ForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.Label3.Location = New System.Drawing.Point(11, 22)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(46, 16)
        Me.Label3.TabIndex = 2
        Me.Label3.Text = "Server"
        '
        'TextEdit1
        '
        Me.TextEdit1.EditValue = ""
        Me.TextEdit1.Location = New System.Drawing.Point(95, 219)
        Me.TextEdit1.Name = "TextEdit1"
        Me.TextEdit1.Properties.Appearance.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextEdit1.Properties.Appearance.Options.UseFont = True
        Me.TextEdit1.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton})
        Me.TextEdit1.Size = New System.Drawing.Size(511, 22)
        Me.TextEdit1.TabIndex = 26
        '
        'XtraTabPage2
        '
        Me.XtraTabPage2.Controls.Add(Me.txtODBC4)
        Me.XtraTabPage2.Controls.Add(Me.txtODBC3)
        Me.XtraTabPage2.Controls.Add(Me.txtODBC2)
        Me.XtraTabPage2.Controls.Add(Me.Label16)
        Me.XtraTabPage2.Controls.Add(Me.Label17)
        Me.XtraTabPage2.Controls.Add(Me.Label18)
        Me.XtraTabPage2.Controls.Add(Me.Label19)
        Me.XtraTabPage2.Controls.Add(Me.Label15)
        Me.XtraTabPage2.Controls.Add(Me.txtDllReport)
        Me.XtraTabPage2.Controls.Add(Me.Label4)
        Me.XtraTabPage2.Controls.Add(Me.txtODBC1)
        Me.XtraTabPage2.Name = "XtraTabPage2"
        Me.XtraTabPage2.Size = New System.Drawing.Size(621, 259)
        Me.XtraTabPage2.Text = "ODBC Report"
        '
        'txtODBC4
        '
        Me.txtODBC4.EditValue = ""
        Me.txtODBC4.EnterMoveNextControl = True
        Me.txtODBC4.Location = New System.Drawing.Point(95, 103)
        Me.txtODBC4.Name = "txtODBC4"
        Me.txtODBC4.Properties.Appearance.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtODBC4.Properties.Appearance.Options.UseFont = True
        Me.txtODBC4.Properties.PasswordChar = Global.Microsoft.VisualBasic.ChrW(42)
        Me.txtODBC4.Size = New System.Drawing.Size(197, 22)
        Me.txtODBC4.TabIndex = 10
        '
        'txtODBC3
        '
        Me.txtODBC3.EditValue = ""
        Me.txtODBC3.EnterMoveNextControl = True
        Me.txtODBC3.Location = New System.Drawing.Point(95, 75)
        Me.txtODBC3.Name = "txtODBC3"
        Me.txtODBC3.Properties.Appearance.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtODBC3.Properties.Appearance.Options.UseFont = True
        Me.txtODBC3.Size = New System.Drawing.Size(197, 22)
        Me.txtODBC3.TabIndex = 8
        '
        'txtODBC2
        '
        Me.txtODBC2.EditValue = ""
        Me.txtODBC2.EnterMoveNextControl = True
        Me.txtODBC2.Location = New System.Drawing.Point(417, 19)
        Me.txtODBC2.Name = "txtODBC2"
        Me.txtODBC2.Properties.Appearance.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtODBC2.Properties.Appearance.Options.UseFont = True
        Me.txtODBC2.Size = New System.Drawing.Size(197, 22)
        Me.txtODBC2.TabIndex = 6
        Me.txtODBC2.Visible = False
        '
        'Label16
        '
        Me.Label16.AutoSize = True
        Me.Label16.BackColor = System.Drawing.Color.Transparent
        Me.Label16.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label16.ForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.Label16.Location = New System.Drawing.Point(11, 106)
        Me.Label16.Name = "Label16"
        Me.Label16.Size = New System.Drawing.Size(63, 16)
        Me.Label16.TabIndex = 9
        Me.Label16.Text = "Password"
        '
        'Label17
        '
        Me.Label17.AutoSize = True
        Me.Label17.BackColor = System.Drawing.Color.Transparent
        Me.Label17.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label17.ForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.Label17.Location = New System.Drawing.Point(11, 78)
        Me.Label17.Name = "Label17"
        Me.Label17.Size = New System.Drawing.Size(66, 16)
        Me.Label17.TabIndex = 7
        Me.Label17.Text = "Username"
        '
        'Label18
        '
        Me.Label18.AutoSize = True
        Me.Label18.BackColor = System.Drawing.Color.Transparent
        Me.Label18.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label18.ForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.Label18.Location = New System.Drawing.Point(333, 22)
        Me.Label18.Name = "Label18"
        Me.Label18.Size = New System.Drawing.Size(61, 16)
        Me.Label18.TabIndex = 5
        Me.Label18.Text = "Database"
        Me.Label18.Visible = False
        '
        'Label19
        '
        Me.Label19.AutoSize = True
        Me.Label19.BackColor = System.Drawing.Color.Transparent
        Me.Label19.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label19.ForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.Label19.Location = New System.Drawing.Point(11, 50)
        Me.Label19.Name = "Label19"
        Me.Label19.Size = New System.Drawing.Size(77, 16)
        Me.Label19.TabIndex = 3
        Me.Label19.Text = "Nama ODBC"
        '
        'Label15
        '
        Me.Label15.AutoSize = True
        Me.Label15.BackColor = System.Drawing.Color.Transparent
        Me.Label15.Font = New System.Drawing.Font("Tahoma", 9.75!, CType((System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Underline), System.Drawing.FontStyle), System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label15.ForeColor = System.Drawing.Color.Maroon
        Me.Label15.Location = New System.Drawing.Point(11, 0)
        Me.Label15.Name = "Label15"
        Me.Label15.Size = New System.Drawing.Size(163, 16)
        Me.Label15.TabIndex = 0
        Me.Label15.Text = "Koneksi ODBC ke Report"
        '
        'txtDllReport
        '
        Me.txtDllReport.EditValue = "pdsoledb.dll"
        Me.txtDllReport.EnterMoveNextControl = True
        Me.txtDllReport.Location = New System.Drawing.Point(95, 19)
        Me.txtDllReport.Name = "txtDllReport"
        Me.txtDllReport.Properties.Appearance.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtDllReport.Properties.Appearance.Options.UseFont = True
        Me.txtDllReport.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.txtDllReport.Properties.Items.AddRange(New Object() {"pdssql.dll", "pdsoledb.dll", "pdsodbc.dll"})
        Me.txtDllReport.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor
        Me.txtDllReport.Size = New System.Drawing.Size(197, 22)
        Me.txtDllReport.TabIndex = 2
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.BackColor = System.Drawing.Color.Transparent
        Me.Label4.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.ForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.Label4.Location = New System.Drawing.Point(11, 22)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(64, 16)
        Me.Label4.TabIndex = 1
        Me.Label4.Text = "Dll Report"
        '
        'txtODBC1
        '
        Me.txtODBC1.EditValue = ""
        Me.txtODBC1.Location = New System.Drawing.Point(95, 47)
        Me.txtODBC1.Name = "txtODBC1"
        Me.txtODBC1.Properties.Appearance.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtODBC1.Properties.Appearance.Options.UseFont = True
        Me.txtODBC1.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Ellipsis, "", -1, True, True, False, DevExpress.XtraEditors.ImageLocation.MiddleCenter, Nothing, New DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.PageDown), SerializableAppearanceObject1, "", Nothing, Nothing, True)})
        Me.txtODBC1.Size = New System.Drawing.Size(197, 22)
        Me.txtODBC1.TabIndex = 4
        '
        'LayoutControl1
        '
        Me.LayoutControl1.AllowCustomizationMenu = False
        Me.LayoutControl1.Controls.Add(Me.PanelControl1)
        Me.LayoutControl1.Controls.Add(Me.PanelControl2)
        Me.LayoutControl1.Controls.Add(Me.XtraTabControl1)
        Me.LayoutControl1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.LayoutControl1.Location = New System.Drawing.Point(0, 0)
        Me.LayoutControl1.Name = "LayoutControl1"
        Me.LayoutControl1.OptionsCustomizationForm.DesignTimeCustomizationFormPositionAndSize = New System.Drawing.Rectangle(974, 204, 250, 350)
        Me.LayoutControl1.Root = Me.LayoutControlGroup1
        Me.LayoutControl1.Size = New System.Drawing.Size(651, 426)
        Me.LayoutControl1.TabIndex = 0
        Me.LayoutControl1.Text = "LayoutControl1"
        '
        'LayoutControlGroup1
        '
        Me.LayoutControlGroup1.CustomizationFormText = "LayoutControlGroup1"
        Me.LayoutControlGroup1.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.[True]
        Me.LayoutControlGroup1.GroupBordersVisible = False
        Me.LayoutControlGroup1.Items.AddRange(New DevExpress.XtraLayout.BaseLayoutItem() {Me.LayoutControlItem1, Me.LayoutControlItem2, Me.LayoutControlItem3})
        Me.LayoutControlGroup1.Location = New System.Drawing.Point(0, 0)
        Me.LayoutControlGroup1.Name = "LayoutControlGroup1"
        Me.LayoutControlGroup1.Size = New System.Drawing.Size(651, 426)
        Me.LayoutControlGroup1.Text = "LayoutControlGroup1"
        Me.LayoutControlGroup1.TextVisible = False
        '
        'LayoutControlItem1
        '
        Me.LayoutControlItem1.Control = Me.PanelControl1
        Me.LayoutControlItem1.CustomizationFormText = "LayoutControlItem1"
        Me.LayoutControlItem1.Location = New System.Drawing.Point(0, 0)
        Me.LayoutControlItem1.Name = "LayoutControlItem1"
        Me.LayoutControlItem1.Size = New System.Drawing.Size(631, 71)
        Me.LayoutControlItem1.Text = "LayoutControlItem1"
        Me.LayoutControlItem1.TextSize = New System.Drawing.Size(0, 0)
        Me.LayoutControlItem1.TextToControlDistance = 0
        Me.LayoutControlItem1.TextVisible = False
        '
        'LayoutControlItem2
        '
        Me.LayoutControlItem2.Control = Me.XtraTabControl1
        Me.LayoutControlItem2.CustomizationFormText = "LayoutControlItem2"
        Me.LayoutControlItem2.Location = New System.Drawing.Point(0, 71)
        Me.LayoutControlItem2.Name = "LayoutControlItem2"
        Me.LayoutControlItem2.Size = New System.Drawing.Size(631, 289)
        Me.LayoutControlItem2.Text = "LayoutControlItem2"
        Me.LayoutControlItem2.TextSize = New System.Drawing.Size(0, 0)
        Me.LayoutControlItem2.TextToControlDistance = 0
        Me.LayoutControlItem2.TextVisible = False
        '
        'LayoutControlItem3
        '
        Me.LayoutControlItem3.Control = Me.PanelControl2
        Me.LayoutControlItem3.CustomizationFormText = "LayoutControlItem3"
        Me.LayoutControlItem3.Location = New System.Drawing.Point(0, 360)
        Me.LayoutControlItem3.Name = "LayoutControlItem3"
        Me.LayoutControlItem3.Size = New System.Drawing.Size(631, 46)
        Me.LayoutControlItem3.Text = "LayoutControlItem3"
        Me.LayoutControlItem3.TextSize = New System.Drawing.Size(0, 0)
        Me.LayoutControlItem3.TextToControlDistance = 0
        Me.LayoutControlItem3.TextVisible = False
        '
        'Label20
        '
        Me.Label20.AutoSize = True
        Me.Label20.BackColor = System.Drawing.Color.Transparent
        Me.Label20.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label20.ForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.Label20.Location = New System.Drawing.Point(11, 222)
        Me.Label20.Name = "Label20"
        Me.Label20.Size = New System.Drawing.Size(77, 16)
        Me.Label20.TabIndex = 25
        Me.Label20.Text = "File Gambar"
        '
        'Label21
        '
        Me.Label21.AutoSize = True
        Me.Label21.BackColor = System.Drawing.Color.Transparent
        Me.Label21.Font = New System.Drawing.Font("Tahoma", 9.75!, CType((System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Underline), System.Drawing.FontStyle), System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label21.ForeColor = System.Drawing.Color.Maroon
        Me.Label21.Location = New System.Drawing.Point(11, 200)
        Me.Label21.Name = "Label21"
        Me.Label21.Size = New System.Drawing.Size(152, 16)
        Me.Label21.TabIndex = 24
        Me.Label21.Text = "Backgound Dashboard"
        '
        'frmSetting
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 14)
        Me.ClientSize = New System.Drawing.Size(651, 426)
        Me.Controls.Add(Me.LayoutControl1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.Name = "frmSetting"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = ".:: Seting Aplikasi ::."
        CType(Me.PanelControl1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.PanelControl1.ResumeLayout(False)
        CType(Me.PanelControl2, System.ComponentModel.ISupportInitialize).EndInit()
        Me.PanelControl2.ResumeLayout(False)
        CType(Me.XtraTabControl1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.XtraTabControl1.ResumeLayout(False)
        Me.XtraTabPage1.ResumeLayout(False)
        Me.XtraTabPage1.PerformLayout()
        CType(Me.txtPOS5.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtTimeOut.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txt1.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.chkLoginWindowsPos.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtPos4.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtPos3.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtPos1.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtPos2.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.chkloginwindows.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txt4.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txt3.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txt2.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.TextEdit1.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        Me.XtraTabPage2.ResumeLayout(False)
        Me.XtraTabPage2.PerformLayout()
        CType(Me.txtODBC4.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtODBC3.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtODBC2.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtDllReport.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtODBC1.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControl1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.LayoutControl1.ResumeLayout(False)
        CType(Me.LayoutControlGroup1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem3, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

#End Region

    Private Sub sbClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles sbClose.Click
        DialogResult = Windows.Forms.DialogResult.Cancel
    End Sub
    Private Sub SetCtlMe()
        If System.IO.File.Exists(FolderLayouts & Me.Name & ".xml") Then
            LayoutControl1.RestoreLayoutFromXml(FolderLayouts & Me.Name & ".xml")
        End If
    End Sub
    Private Sub XtraForm1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        txt1.Text = BacaIni("dbconfig", "Server", "localhost")
        txtTimeOut.EditValue = BacaIni("dbconfig", "Timeout", "15")
        txt2.Text = BacaIni("dbconfig", "Database", "dbcityoys")
        txt3.Text = BacaIni("dbconfig", "Username", "sa")
        txt4.Text = BacaIni("dbconfig", "Password", "sahasystem")
        chkloginwindows.Checked = IIf(BacaIni("dbconfig", "Login windows", "1") = "1", True, False)
        txtPos1.Text = BacaIni("dbconfig2", "Server", "localhost")
        txtPos2.Text = BacaIni("dbconfig2", "Database", "klns")
        txtPos3.Text = BacaIni("dbconfig2", "Username", "root")
        txtPos4.Text = BacaIni("dbconfig2", "Password", "vpoint")
        txtPOS5.EditValue = BacaIni("dbconfig2", "Timeout", "15")
        chkLoginWindowsPos.Checked = IIf(BacaIni("dbconfig2", "Login windows", "1") = "1", True, False)

        'ODBC Report
        txtDllReport.Text = BacaIni("odbcconfig", "Report", txtDllReport.Text)
        txtODBC1.Text = BacaIni("odbcconfig", "Server", "citytoys")
        txtODBC2.Text = BacaIni("odbcconfig", "Database", "dbcityoys")
        txtODBC3.Text = BacaIni("odbcconfig", "Username", "sa")
        txtODBC4.Text = BacaIni("odbcconfig", "Password", "sahasystem")

        TextEdit1.Text = BacaIni("Application", "Background", Application.StartupPath & "\system\image\BG.jpg")

        'Dim oDS As New DataSet
        'Try
        '    Dim strsql As String
        '    strsql = String.Format("SELECT {0}LastPostSales From MSettingPerusahaan", IIf(VPOINT.Serialshield.Serial.IsTrial, " top 100 ", ""))
        '    oDS = MyConn.ExecuteDataset("tbl", strsql)
        '    If oDS.Tables(0).Rows.Count >= 1 Then
        '        TglPosting.EditValue = NullToDate(oDS.Tables(0).Rows(0).Item(0))
        '    End If
        'Catch ex As Exception

        'Finally
        '    oDS.Dispose()
        'End Try
        FungsiControl.SetForm(Me)
    End Sub

    Private Sub SimpleButton1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SimpleButton1.Click
        Dim curentcursor As Cursor = Windows.Forms.Cursor.Current
        Windows.Forms.Cursor.Current = Cursors.WaitCursor

        TulisIni("dbconfig", "Server", txt1.Text)
        TulisIni("dbconfig", "Database", txt2.Text)
        TulisIni("dbconfig", "Username", txt3.Text)
        TulisIni("dbconfig", "Password", txt4.Text)
        TulisIni("dbconfig", "Timeout", IIf(txtTimeOut.EditValue <= 10, 15, txtTimeOut.EditValue))
        If chkloginwindows.Checked Then
            TulisIni("dbconfig", "Login windows", "1")
        Else
            TulisIni("dbconfig", "Login windows", "0")
        End If
        If BacaIni("dbconfig", "Login windows", "1") = "1" Then 'Login windows
            StrKonSql = "Data Source=" & BacaIni("dbconfig", "Server", "localhost") & _
                        ";initial Catalog=" & BacaIni("dbconfig", "Database", "dbcityoys") & _
                        ";Integrated Security=True;Connect Timeout=" & BacaIni("dbconfig", "Timeout", "15")
        Else
            StrKonSql = "Data Source=" & BacaIni("dbconfig", "Server", "localhost") & _
                        ";initial Catalog=" & BacaIni("dbconfig", "Database", "dbcityoys") & _
                        ";User ID=" & BacaIni("dbconfig", "Username", "sa") & _
                        ";Password=" & BacaIni("dbconfig", "Password", "sahasystem") & ";Connect Timeout=" & BacaIni("dbconfig", "Timeout", "15")
        End If

        TulisIni("dbconfig2", "Server", txtPos1.Text)
        TulisIni("dbconfig2", "Database", txtPos2.Text)
        TulisIni("dbconfig2", "Username", txtPos3.Text)
        TulisIni("dbconfig2", "Password", txtPos4.Text)
        TulisIni("dbconfig2", "Timeout", IIf(txtPOS5.EditValue <= 10, 15, txtPOS5.EditValue))
        If chkLoginWindowsPos.Checked Then
            TulisIni("dbconfig2", "Login windows", "1")
        Else
            TulisIni("dbconfig2", "Login windows", "0")
        End If
        If BacaIni("dbconfig2", "Login windows", "1") = "1" Then 'Login windows
            StrKonSqlServer2 = "Data Source=" & BacaIni("dbconfig2", "Server", "localhost") & _
                        ";initial Catalog=" & BacaIni("dbconfig2", "Database", "dbcityoys") & _
                        ";Integrated Security=True;Connect Timeout=" & BacaIni("dbconfig2", "Timeout", "15")
        Else
            StrKonSqlServer2 = "Data Source=" & BacaIni("dbconfig2", "Server", "localhost") & _
                               ";initial Catalog=" & BacaIni("dbconfig2", "Database", "dbcityoys") & _
                               ";User ID=" & BacaIni("dbconfig2", "Username", "sa") & _
                               ";Password=" & BacaIni("dbconfig2", "Password", "sahasystem") & ";Connect Timeout=" & BacaIni("dbconfig2", "Timeout", "15")
        End If

        Windows.Forms.Cursor.Current = curentcursor
        'FxMessage("Setting data telah tersimpan.", "Setting data1", MessageBoxButtons.OK, MessageBoxIcon.Asterisk, MessageBoxDefaultButton.Button1, MessageBoxOptions.RtlReading)
        If CekKoneksi() Then
            mdlODBC.CreateSystemDSN(txtODBC1.Text, txt1.Text, txtODBC3.Text, txtODBC4.Text, txt2.Text)
            If CekKoneksiODBC() Then
                'ODBC Report
                TulisIni("odbcconfig", "Report", txtDllReport.Text)
                TulisIni("odbcconfig", "Server", txtODBC1.Text)
                TulisIni("odbcconfig", "Database", txtODBC2.Text)
                TulisIni("odbcconfig", "Username", txtODBC3.Text)
                TulisIni("odbcconfig", "Password", txtODBC4.Text)

                DialogResult = Windows.Forms.DialogResult.OK
                Close()
            Else
                If XtraMessageBox.Show("Koneksi ODBC ke Report belum tersambung, yakin ingin meneruskan penyimpanan?", NamaAplikasi, MessageBoxButtons.YesNo, MessageBoxIcon.Warning) = Windows.Forms.DialogResult.Yes Then
                    DialogResult = Windows.Forms.DialogResult.OK
                    Close()
                End If
            End If

            'If txtNamaODBCBarcode.Text <> "" AndAlso CekKoneksiODBCBarcode() Then
            '    'ODBC Cetak Barcode
            '    TulisIni("odbcbarcodeconfig", "Server", txtNamaODBCBarcode.Text)
            '    TulisIni("odbcbarcodeconfig", "Username", txtUIDODBCBarcode.Text)
            '    TulisIni("odbcbarcodeconfig", "Password", txtPWDBarcode.Text)
            'End If
            If IO.File.Exists(TextEdit1.Text) Then
                TulisIni("Application", "Background", TextEdit1.Text)
            End If
        Else
            ' FxMessage("Setting data masih belum benar.", ".:: Setting data ::.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        End If
        'EksekusiSQlPos("Update MSettingPerusahaan Set LastPostSales=convert(datetime,'" & TglPosting.DateTime.ToString("MM/dd/yyyy") & "',101)")
    End Sub

    Public Function CekKoneksiODBC() As Boolean
        Dim oConn As OdbcConnection = Nothing
        Dim hasil As Boolean = False
        Dim strConn As String = ""
        Try
            If txtODBC3.Text <> "" Then
                strConn = "Dsn=" & txtODBC1.Text & ";uid=" & txtODBC3.Text & ";pwd=" & txtODBC4.Text
            Else
                strConn = "Dsn=" & txtODBC1.Text
            End If
            oConn = New OdbcConnection(strConn)
            oConn.Open()
            oConn.Close()
            hasil = True
        Catch ex As Exception
            XtraMessageBox.Show("Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
            hasil = False
        Finally
            oConn.Dispose()
        End Try
        Return hasil
    End Function
     
    Sub RunODBC()
        Shell("odbcad32.exe", AppWinStyle.NormalFocus, True)
    End Sub

    Private Sub TextEdit1_ButtonClick(ByVal sender As Object, ByVal e As DevExpress.XtraEditors.Controls.ButtonPressedEventArgs) Handles TextEdit1.ButtonClick
        Dim x As New OpenFileDialog
        If e.Button.Index = 0 Then
            x.Title = "Background VPoint Retail System"
            x.Filter = "Image Files(*.BMP;*.JPG;*.JPEG;*.PNG;*.GIF)|*.BMP;*.JPG;*.JPEG;*.PNG;*.GIF"
            If x.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
                TextEdit1.Text = x.FileName
            End If
        End If
        x.Dispose()
    End Sub

    Private Sub txtODBC1_ButtonClick(ByVal sender As Object, ByVal e As DevExpress.XtraEditors.Controls.ButtonPressedEventArgs) Handles txtODBC1.ButtonClick
        If e.Button.Index = 0 Then
            RunODBC()
        End If
    End Sub

    Private Sub txtODBC1_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtODBC1.EditValueChanged

    End Sub

    Private Sub txt3_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txt3.EditValueChanged
        txtODBC3.Text = txt3.Text
    End Sub

    Private Sub txt2_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txt2.EditValueChanged
        txtODBC2.Text = txt2.Text
    End Sub

    Private Sub txt4_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txt4.EditValueChanged
        txtODBC4.Text = txt4.Text
    End Sub

    Private Sub txtNamaODBCBarcode_ButtonClick(ByVal sender As Object, ByVal e As DevExpress.XtraEditors.Controls.ButtonPressedEventArgs)
        If e.Button.Index = 0 Then
            RunODBC()
        End If
    End Sub

    Private Sub PanelControl2_Paint(ByVal sender As System.Object, ByVal e As System.Windows.Forms.PaintEventArgs) Handles PanelControl2.Paint

    End Sub

    Private Sub TextEdit1_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextEdit1.EditValueChanged

    End Sub
End Class

