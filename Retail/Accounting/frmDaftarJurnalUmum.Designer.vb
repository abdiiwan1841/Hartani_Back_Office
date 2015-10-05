<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmDaftarJurnalUmum
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
        Me.GridControl1 = New DevExpress.XtraGrid.GridControl
        Me.GridView1 = New DevExpress.XtraGrid.Views.Grid.GridView
        Me.RepositoryItemCheckEdit1 = New DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit
        Me.RepositoryItemCheckEdit2 = New DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit
        Me.BarManager1 = New DevExpress.XtraBars.BarManager(Me.components)
        Me.barDockControlTop = New DevExpress.XtraBars.BarDockControl
        Me.barDockControlBottom = New DevExpress.XtraBars.BarDockControl
        Me.barDockControlLeft = New DevExpress.XtraBars.BarDockControl
        Me.barDockControlRight = New DevExpress.XtraBars.BarDockControl
        Me.BarSubItem1 = New DevExpress.XtraBars.BarSubItem
        Me.BarSubItem2 = New DevExpress.XtraBars.BarSubItem
        Me.mnPosting = New DevExpress.XtraBars.BarButtonItem
        Me.mnUnPosting = New DevExpress.XtraBars.BarButtonItem
        Me.BarSubItem5 = New DevExpress.XtraBars.BarSubItem
        Me.BarButtonItem10 = New DevExpress.XtraBars.BarButtonItem
        Me.BarButtonItem11 = New DevExpress.XtraBars.BarButtonItem
        Me.BarSubItem3 = New DevExpress.XtraBars.BarSubItem
        Me.mnFaktur = New DevExpress.XtraBars.BarButtonItem
        Me.mnPerbaikiData = New DevExpress.XtraBars.BarButtonItem
        Me.RepositoryItemTextEdit1 = New DevExpress.XtraEditors.Repository.RepositoryItemTextEdit
        Me.PopupMenu1 = New DevExpress.XtraBars.PopupMenu(Me.components)
        Me.LayoutControl1 = New DevExpress.XtraLayout.LayoutControl
        Me.PanelControl3 = New DevExpress.XtraEditors.PanelControl
        Me.cmdExport = New DevExpress.XtraEditors.SimpleButton
        Me.cmdRefresh = New DevExpress.XtraEditors.SimpleButton
        Me.cmdDelete = New DevExpress.XtraEditors.SimpleButton
        Me.cmdEdit = New DevExpress.XtraEditors.SimpleButton
        Me.cmdClose = New DevExpress.XtraEditors.SimpleButton
        Me.cmdNew = New DevExpress.XtraEditors.SimpleButton
        Me.cmdMark = New DevExpress.XtraEditors.SimpleButton
        Me.PanelControl1 = New DevExpress.XtraEditors.PanelControl
        Me.LabelControl4 = New DevExpress.XtraEditors.LabelControl
        Me.LabelControl5 = New DevExpress.XtraEditors.LabelControl
        Me.TglSampai = New DevExpress.XtraEditors.DateEdit
        Me.tglDari = New DevExpress.XtraEditors.DateEdit
        Me.LabelControl3 = New DevExpress.XtraEditors.LabelControl
        Me.Label2 = New System.Windows.Forms.Label
        Me.cbStatus = New DevExpress.XtraEditors.ComboBoxEdit
        Me.LayoutControlGroup1 = New DevExpress.XtraLayout.LayoutControlGroup
        Me.LayoutControlItem1 = New DevExpress.XtraLayout.LayoutControlItem
        Me.SplitterItem1 = New DevExpress.XtraLayout.SplitterItem
        Me.LayoutControlItem2 = New DevExpress.XtraLayout.LayoutControlItem
        Me.LayoutControlItem3 = New DevExpress.XtraLayout.LayoutControlItem
        Me.SplitterItem2 = New DevExpress.XtraLayout.SplitterItem
        CType(Me.GridControl1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.GridView1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RepositoryItemCheckEdit1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RepositoryItemCheckEdit2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.BarManager1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RepositoryItemTextEdit1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PopupMenu1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControl1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.LayoutControl1.SuspendLayout()
        CType(Me.PanelControl3, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.PanelControl3.SuspendLayout()
        CType(Me.PanelControl1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.PanelControl1.SuspendLayout()
        CType(Me.TglSampai.Properties.VistaTimeProperties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.TglSampai.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.tglDari.Properties.VistaTimeProperties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.tglDari.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.cbStatus.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlGroup1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.SplitterItem1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem3, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.SplitterItem2, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'GridControl1
        '
        Me.GridControl1.Location = New System.Drawing.Point(154, 113)
        Me.GridControl1.MainView = Me.GridView1
        Me.GridControl1.Name = "GridControl1"
        Me.GridControl1.RepositoryItems.AddRange(New DevExpress.XtraEditors.Repository.RepositoryItem() {Me.RepositoryItemCheckEdit1, Me.RepositoryItemCheckEdit2})
        Me.GridControl1.ShowOnlyPredefinedDetails = True
        Me.GridControl1.Size = New System.Drawing.Size(675, 254)
        Me.GridControl1.TabIndex = 11
        Me.GridControl1.ViewCollection.AddRange(New DevExpress.XtraGrid.Views.Base.BaseView() {Me.GridView1})
        '
        'GridView1
        '
        Me.GridView1.CustomizationFormBounds = New System.Drawing.Rectangle(592, 208, 208, 168)
        Me.GridView1.GridControl = Me.GridControl1
        Me.GridView1.Name = "GridView1"
        Me.GridView1.OptionsBehavior.AllowIncrementalSearch = True
        Me.GridView1.OptionsBehavior.Editable = False
        Me.GridView1.OptionsDetail.EnableDetailToolTip = True
        Me.GridView1.OptionsSelection.MultiSelect = True
        Me.GridView1.OptionsView.ColumnAutoWidth = False
        Me.GridView1.OptionsView.ShowFooter = True
        '
        'RepositoryItemCheckEdit1
        '
        Me.RepositoryItemCheckEdit1.Appearance.Options.UseTextOptions = True
        Me.RepositoryItemCheckEdit1.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.RepositoryItemCheckEdit1.Caption = ""
        Me.RepositoryItemCheckEdit1.Name = "RepositoryItemCheckEdit1"
        Me.RepositoryItemCheckEdit1.NullStyle = DevExpress.XtraEditors.Controls.StyleIndeterminate.Inactive
        '
        'RepositoryItemCheckEdit2
        '
        Me.RepositoryItemCheckEdit2.AutoHeight = False
        Me.RepositoryItemCheckEdit2.CheckStyle = DevExpress.XtraEditors.Controls.CheckStyles.Style1
        Me.RepositoryItemCheckEdit2.Name = "RepositoryItemCheckEdit2"
        Me.RepositoryItemCheckEdit2.NullStyle = DevExpress.XtraEditors.Controls.StyleIndeterminate.Unchecked
        '
        'BarManager1
        '
        Me.BarManager1.DockControls.Add(Me.barDockControlTop)
        Me.BarManager1.DockControls.Add(Me.barDockControlBottom)
        Me.BarManager1.DockControls.Add(Me.barDockControlLeft)
        Me.BarManager1.DockControls.Add(Me.barDockControlRight)
        Me.BarManager1.Form = Me
        Me.BarManager1.Items.AddRange(New DevExpress.XtraBars.BarItem() {Me.BarSubItem1, Me.BarSubItem2, Me.mnPosting, Me.BarSubItem5, Me.BarButtonItem10, Me.BarButtonItem11, Me.mnUnPosting, Me.BarSubItem3, Me.mnFaktur, Me.mnPerbaikiData})
        Me.BarManager1.MaxItemId = 24
        Me.BarManager1.RepositoryItems.AddRange(New DevExpress.XtraEditors.Repository.RepositoryItem() {Me.RepositoryItemTextEdit1})
        '
        'barDockControlTop
        '
        Me.barDockControlTop.CausesValidation = False
        Me.barDockControlTop.Dock = System.Windows.Forms.DockStyle.Top
        Me.barDockControlTop.Location = New System.Drawing.Point(0, 0)
        Me.barDockControlTop.Size = New System.Drawing.Size(841, 0)
        '
        'barDockControlBottom
        '
        Me.barDockControlBottom.CausesValidation = False
        Me.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.barDockControlBottom.Location = New System.Drawing.Point(0, 379)
        Me.barDockControlBottom.Size = New System.Drawing.Size(841, 0)
        '
        'barDockControlLeft
        '
        Me.barDockControlLeft.CausesValidation = False
        Me.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left
        Me.barDockControlLeft.Location = New System.Drawing.Point(0, 0)
        Me.barDockControlLeft.Size = New System.Drawing.Size(0, 379)
        '
        'barDockControlRight
        '
        Me.barDockControlRight.CausesValidation = False
        Me.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right
        Me.barDockControlRight.Location = New System.Drawing.Point(841, 0)
        Me.barDockControlRight.Size = New System.Drawing.Size(0, 379)
        '
        'BarSubItem1
        '
        Me.BarSubItem1.Caption = "Menu"
        Me.BarSubItem1.Id = 0
        Me.BarSubItem1.LinksPersistInfo.AddRange(New DevExpress.XtraBars.LinkPersistInfo() {New DevExpress.XtraBars.LinkPersistInfo(Me.BarSubItem2), New DevExpress.XtraBars.LinkPersistInfo(Me.BarSubItem5)})
        Me.BarSubItem1.Name = "BarSubItem1"
        Me.BarSubItem1.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
        '
        'BarSubItem2
        '
        Me.BarSubItem2.Caption = "Action"
        Me.BarSubItem2.Id = 2
        Me.BarSubItem2.LinksPersistInfo.AddRange(New DevExpress.XtraBars.LinkPersistInfo() {New DevExpress.XtraBars.LinkPersistInfo(Me.mnPosting), New DevExpress.XtraBars.LinkPersistInfo(Me.mnUnPosting)})
        Me.BarSubItem2.Name = "BarSubItem2"
        '
        'mnPosting
        '
        Me.mnPosting.Caption = "Posting"
        Me.mnPosting.Id = 16
        Me.mnPosting.Name = "mnPosting"
        '
        'mnUnPosting
        '
        Me.mnUnPosting.Caption = "UnPosting"
        Me.mnUnPosting.Id = 20
        Me.mnUnPosting.Name = "mnUnPosting"
        '
        'BarSubItem5
        '
        Me.BarSubItem5.Caption = "Cetak"
        Me.BarSubItem5.Id = 17
        Me.BarSubItem5.LinksPersistInfo.AddRange(New DevExpress.XtraBars.LinkPersistInfo() {New DevExpress.XtraBars.LinkPersistInfo(Me.BarButtonItem10), New DevExpress.XtraBars.LinkPersistInfo(Me.BarButtonItem11), New DevExpress.XtraBars.LinkPersistInfo(Me.BarButtonItem10), New DevExpress.XtraBars.LinkPersistInfo(Me.BarButtonItem11)})
        Me.BarSubItem5.Name = "BarSubItem5"
        '
        'BarButtonItem10
        '
        Me.BarButtonItem10.Caption = "BUKTI KAS MASUK"
        Me.BarButtonItem10.Id = 18
        Me.BarButtonItem10.Name = "BarButtonItem10"
        '
        'BarButtonItem11
        '
        Me.BarButtonItem11.Caption = "BUKTI BANK MASUK"
        Me.BarButtonItem11.Id = 19
        Me.BarButtonItem11.Name = "BarButtonItem11"
        '
        'BarSubItem3
        '
        Me.BarSubItem3.Caption = "Cetak"
        Me.BarSubItem3.Id = 21
        Me.BarSubItem3.LinksPersistInfo.AddRange(New DevExpress.XtraBars.LinkPersistInfo() {New DevExpress.XtraBars.LinkPersistInfo(Me.mnFaktur)})
        Me.BarSubItem3.Name = "BarSubItem3"
        '
        'mnFaktur
        '
        Me.mnFaktur.Caption = "Cetak Bukti"
        Me.mnFaktur.Id = 22
        Me.mnFaktur.Name = "mnFaktur"
        '
        'mnPerbaikiData
        '
        Me.mnPerbaikiData.Caption = "&Perbaiki Data"
        Me.mnPerbaikiData.Id = 23
        Me.mnPerbaikiData.Name = "mnPerbaikiData"
        '
        'RepositoryItemTextEdit1
        '
        Me.RepositoryItemTextEdit1.AutoHeight = False
        Me.RepositoryItemTextEdit1.Name = "RepositoryItemTextEdit1"
        '
        'PopupMenu1
        '
        Me.PopupMenu1.LinksPersistInfo.AddRange(New DevExpress.XtraBars.LinkPersistInfo() {New DevExpress.XtraBars.LinkPersistInfo(Me.BarSubItem2), New DevExpress.XtraBars.LinkPersistInfo(Me.BarSubItem3), New DevExpress.XtraBars.LinkPersistInfo(Me.mnPerbaikiData)})
        Me.PopupMenu1.Manager = Me.BarManager1
        Me.PopupMenu1.Name = "PopupMenu1"
        '
        'LayoutControl1
        '
        Me.LayoutControl1.Controls.Add(Me.PanelControl3)
        Me.LayoutControl1.Controls.Add(Me.PanelControl1)
        Me.LayoutControl1.Controls.Add(Me.GridControl1)
        Me.LayoutControl1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.LayoutControl1.Location = New System.Drawing.Point(0, 0)
        Me.LayoutControl1.Name = "LayoutControl1"
        Me.LayoutControl1.Root = Me.LayoutControlGroup1
        Me.LayoutControl1.Size = New System.Drawing.Size(841, 379)
        Me.LayoutControl1.TabIndex = 8
        Me.LayoutControl1.Text = "LayoutControl1"
        '
        'PanelControl3
        '
        Me.PanelControl3.Appearance.BackColor = System.Drawing.SystemColors.InactiveBorder
        Me.PanelControl3.Appearance.Options.UseBackColor = True
        Me.PanelControl3.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple
        Me.PanelControl3.Controls.Add(Me.cmdExport)
        Me.PanelControl3.Controls.Add(Me.cmdRefresh)
        Me.PanelControl3.Controls.Add(Me.cmdDelete)
        Me.PanelControl3.Controls.Add(Me.cmdEdit)
        Me.PanelControl3.Controls.Add(Me.cmdClose)
        Me.PanelControl3.Controls.Add(Me.cmdNew)
        Me.PanelControl3.Controls.Add(Me.cmdMark)
        Me.PanelControl3.Location = New System.Drawing.Point(12, 113)
        Me.PanelControl3.Name = "PanelControl3"
        Me.PanelControl3.Size = New System.Drawing.Size(133, 254)
        Me.PanelControl3.TabIndex = 9
        '
        'cmdExport
        '
        Me.cmdExport.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdExport.Appearance.BackColor = System.Drawing.Color.Snow
        Me.cmdExport.Appearance.BackColor2 = System.Drawing.Color.PowderBlue
        Me.cmdExport.Appearance.BorderColor = System.Drawing.SystemColors.ButtonFace
        Me.cmdExport.Appearance.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdExport.Appearance.ForeColor = System.Drawing.Color.Black
        Me.cmdExport.Appearance.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.ForwardDiagonal
        Me.cmdExport.Appearance.Options.UseBackColor = True
        Me.cmdExport.Appearance.Options.UseBorderColor = True
        Me.cmdExport.Appearance.Options.UseFont = True
        Me.cmdExport.Appearance.Options.UseForeColor = True
        Me.cmdExport.Cursor = System.Windows.Forms.Cursors.Hand
        Me.cmdExport.ImageIndex = 14
        Me.cmdExport.Location = New System.Drawing.Point(9, 178)
        Me.cmdExport.Name = "cmdExport"
        Me.cmdExport.Size = New System.Drawing.Size(115, 34)
        Me.cmdExport.TabIndex = 30
        Me.cmdExport.Text = "&Export Excel"
        '
        'cmdRefresh
        '
        Me.cmdRefresh.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdRefresh.Appearance.BackColor = System.Drawing.Color.Snow
        Me.cmdRefresh.Appearance.BackColor2 = System.Drawing.Color.PowderBlue
        Me.cmdRefresh.Appearance.BorderColor = System.Drawing.SystemColors.ButtonFace
        Me.cmdRefresh.Appearance.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdRefresh.Appearance.ForeColor = System.Drawing.Color.Black
        Me.cmdRefresh.Appearance.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.ForwardDiagonal
        Me.cmdRefresh.Appearance.Options.UseBackColor = True
        Me.cmdRefresh.Appearance.Options.UseBorderColor = True
        Me.cmdRefresh.Appearance.Options.UseFont = True
        Me.cmdRefresh.Appearance.Options.UseForeColor = True
        Me.cmdRefresh.Cursor = System.Windows.Forms.Cursors.Hand
        Me.cmdRefresh.ImageIndex = 13
        Me.cmdRefresh.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleLeft
        Me.cmdRefresh.Location = New System.Drawing.Point(9, 5)
        Me.cmdRefresh.Name = "cmdRefresh"
        Me.cmdRefresh.Size = New System.Drawing.Size(115, 34)
        Me.cmdRefresh.TabIndex = 29
        Me.cmdRefresh.Text = "&Refresh"
        '
        'cmdDelete
        '
        Me.cmdDelete.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdDelete.Appearance.BackColor = System.Drawing.Color.Snow
        Me.cmdDelete.Appearance.BackColor2 = System.Drawing.Color.PowderBlue
        Me.cmdDelete.Appearance.BorderColor = System.Drawing.SystemColors.ButtonFace
        Me.cmdDelete.Appearance.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdDelete.Appearance.ForeColor = System.Drawing.Color.Black
        Me.cmdDelete.Appearance.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.ForwardDiagonal
        Me.cmdDelete.Appearance.Options.UseBackColor = True
        Me.cmdDelete.Appearance.Options.UseBorderColor = True
        Me.cmdDelete.Appearance.Options.UseFont = True
        Me.cmdDelete.Appearance.Options.UseForeColor = True
        Me.cmdDelete.Cursor = System.Windows.Forms.Cursors.Hand
        Me.cmdDelete.ImageIndex = 10
        Me.cmdDelete.Location = New System.Drawing.Point(9, 125)
        Me.cmdDelete.Name = "cmdDelete"
        Me.cmdDelete.Size = New System.Drawing.Size(115, 34)
        Me.cmdDelete.TabIndex = 28
        Me.cmdDelete.Text = "&Delete"
        '
        'cmdEdit
        '
        Me.cmdEdit.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdEdit.Appearance.BackColor = System.Drawing.Color.Snow
        Me.cmdEdit.Appearance.BackColor2 = System.Drawing.Color.PowderBlue
        Me.cmdEdit.Appearance.BorderColor = System.Drawing.SystemColors.ButtonFace
        Me.cmdEdit.Appearance.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdEdit.Appearance.ForeColor = System.Drawing.Color.Black
        Me.cmdEdit.Appearance.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.ForwardDiagonal
        Me.cmdEdit.Appearance.Options.UseBackColor = True
        Me.cmdEdit.Appearance.Options.UseBorderColor = True
        Me.cmdEdit.Appearance.Options.UseFont = True
        Me.cmdEdit.Appearance.Options.UseForeColor = True
        Me.cmdEdit.Cursor = System.Windows.Forms.Cursors.Hand
        Me.cmdEdit.ImageIndex = 5
        Me.cmdEdit.Location = New System.Drawing.Point(9, 85)
        Me.cmdEdit.Name = "cmdEdit"
        Me.cmdEdit.Size = New System.Drawing.Size(115, 34)
        Me.cmdEdit.TabIndex = 25
        Me.cmdEdit.Text = "&Edit"
        '
        'cmdClose
        '
        Me.cmdClose.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdClose.Appearance.BackColor = System.Drawing.Color.Snow
        Me.cmdClose.Appearance.BackColor2 = System.Drawing.Color.PowderBlue
        Me.cmdClose.Appearance.BorderColor = System.Drawing.SystemColors.ButtonFace
        Me.cmdClose.Appearance.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdClose.Appearance.ForeColor = System.Drawing.Color.Black
        Me.cmdClose.Appearance.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.ForwardDiagonal
        Me.cmdClose.Appearance.Options.UseBackColor = True
        Me.cmdClose.Appearance.Options.UseBorderColor = True
        Me.cmdClose.Appearance.Options.UseFont = True
        Me.cmdClose.Appearance.Options.UseForeColor = True
        Me.cmdClose.Cursor = System.Windows.Forms.Cursors.Hand
        Me.cmdClose.ImageIndex = 1
        Me.cmdClose.Location = New System.Drawing.Point(9, 217)
        Me.cmdClose.Name = "cmdClose"
        Me.cmdClose.Size = New System.Drawing.Size(115, 34)
        Me.cmdClose.TabIndex = 27
        Me.cmdClose.Text = "&Close"
        '
        'cmdNew
        '
        Me.cmdNew.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdNew.Appearance.BackColor = System.Drawing.Color.Snow
        Me.cmdNew.Appearance.BackColor2 = System.Drawing.Color.PowderBlue
        Me.cmdNew.Appearance.BorderColor = System.Drawing.SystemColors.ButtonFace
        Me.cmdNew.Appearance.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdNew.Appearance.ForeColor = System.Drawing.Color.Black
        Me.cmdNew.Appearance.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.ForwardDiagonal
        Me.cmdNew.Appearance.Options.UseBackColor = True
        Me.cmdNew.Appearance.Options.UseBorderColor = True
        Me.cmdNew.Appearance.Options.UseFont = True
        Me.cmdNew.Appearance.Options.UseForeColor = True
        Me.cmdNew.Cursor = System.Windows.Forms.Cursors.Hand
        Me.cmdNew.ImageIndex = 6
        Me.cmdNew.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleLeft
        Me.cmdNew.Location = New System.Drawing.Point(9, 45)
        Me.cmdNew.Name = "cmdNew"
        Me.cmdNew.Size = New System.Drawing.Size(115, 34)
        Me.cmdNew.TabIndex = 26
        Me.cmdNew.Text = "&New"
        '
        'cmdMark
        '
        Me.cmdMark.Appearance.BackColor = System.Drawing.Color.Snow
        Me.cmdMark.Appearance.BackColor2 = System.Drawing.Color.PowderBlue
        Me.cmdMark.Appearance.BorderColor = System.Drawing.SystemColors.ButtonFace
        Me.cmdMark.Appearance.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdMark.Appearance.ForeColor = System.Drawing.Color.Black
        Me.cmdMark.Appearance.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.ForwardDiagonal
        Me.cmdMark.Appearance.Options.UseBackColor = True
        Me.cmdMark.Appearance.Options.UseBorderColor = True
        Me.cmdMark.Appearance.Options.UseFont = True
        Me.cmdMark.Appearance.Options.UseForeColor = True
        Me.cmdMark.ButtonStyle = DevExpress.XtraEditors.Controls.BorderStyles.HotFlat
        Me.cmdMark.Cursor = System.Windows.Forms.Cursors.Hand
        Me.cmdMark.ImageIndex = 9
        Me.cmdMark.Location = New System.Drawing.Point(12, 298)
        Me.cmdMark.Name = "cmdMark"
        Me.cmdMark.Size = New System.Drawing.Size(115, 34)
        Me.cmdMark.TabIndex = 21
        Me.cmdMark.Text = "Mark"
        Me.cmdMark.Visible = False
        '
        'PanelControl1
        '
        Me.PanelControl1.Controls.Add(Me.LabelControl4)
        Me.PanelControl1.Controls.Add(Me.LabelControl5)
        Me.PanelControl1.Controls.Add(Me.TglSampai)
        Me.PanelControl1.Controls.Add(Me.tglDari)
        Me.PanelControl1.Controls.Add(Me.LabelControl3)
        Me.PanelControl1.Controls.Add(Me.Label2)
        Me.PanelControl1.Controls.Add(Me.cbStatus)
        Me.PanelControl1.Location = New System.Drawing.Point(12, 12)
        Me.PanelControl1.Name = "PanelControl1"
        Me.PanelControl1.Size = New System.Drawing.Size(817, 92)
        Me.PanelControl1.TabIndex = 9
        '
        'LabelControl4
        '
        Me.LabelControl4.Appearance.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LabelControl4.Location = New System.Drawing.Point(172, 39)
        Me.LabelControl4.Name = "LabelControl4"
        Me.LabelControl4.Size = New System.Drawing.Size(18, 16)
        Me.LabelControl4.TabIndex = 11
        Me.LabelControl4.Text = "s/d"
        '
        'LabelControl5
        '
        Me.LabelControl5.Appearance.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LabelControl5.Cursor = System.Windows.Forms.Cursors.Hand
        Me.LabelControl5.Location = New System.Drawing.Point(5, 39)
        Me.LabelControl5.Name = "LabelControl5"
        Me.LabelControl5.Size = New System.Drawing.Size(46, 16)
        Me.LabelControl5.TabIndex = 10
        Me.LabelControl5.Text = "Tanggal"
        '
        'TglSampai
        '
        Me.TglSampai.EditValue = New Date(2013, 6, 21, 21, 6, 24, 105)
        Me.TglSampai.Enabled = False
        Me.TglSampai.EnterMoveNextControl = True
        Me.TglSampai.Location = New System.Drawing.Point(196, 36)
        Me.TglSampai.Name = "TglSampai"
        Me.TglSampai.Properties.Appearance.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TglSampai.Properties.Appearance.Options.UseFont = True
        Me.TglSampai.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.TglSampai.Properties.Mask.EditMask = "dd-MM-yyyy"
        Me.TglSampai.Properties.Mask.UseMaskAsDisplayFormat = True
        Me.TglSampai.Properties.VistaTimeProperties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton})
        Me.TglSampai.Size = New System.Drawing.Size(116, 22)
        Me.TglSampai.TabIndex = 9
        '
        'tglDari
        '
        Me.tglDari.EditValue = New Date(2013, 6, 21, 21, 6, 24, 105)
        Me.tglDari.Enabled = False
        Me.tglDari.EnterMoveNextControl = True
        Me.tglDari.Location = New System.Drawing.Point(57, 36)
        Me.tglDari.Name = "tglDari"
        Me.tglDari.Properties.Appearance.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tglDari.Properties.Appearance.Options.UseFont = True
        Me.tglDari.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.tglDari.Properties.Mask.EditMask = "dd-MM-yyyy"
        Me.tglDari.Properties.Mask.UseMaskAsDisplayFormat = True
        Me.tglDari.Properties.VistaTimeProperties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton})
        Me.tglDari.Size = New System.Drawing.Size(109, 22)
        Me.tglDari.TabIndex = 8
        '
        'LabelControl3
        '
        Me.LabelControl3.Appearance.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LabelControl3.Location = New System.Drawing.Point(5, 11)
        Me.LabelControl3.Name = "LabelControl3"
        Me.LabelControl3.Size = New System.Drawing.Size(36, 16)
        Me.LabelControl3.TabIndex = 6
        Me.LabelControl3.Text = "Status"
        '
        'Label2
        '
        Me.Label2.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label2.BackColor = System.Drawing.Color.Transparent
        Me.Label2.Font = New System.Drawing.Font("Rockwell", 15.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.ForeColor = System.Drawing.SystemColors.HotTrack
        Me.Label2.ImageAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.Label2.Location = New System.Drawing.Point(475, 11)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(337, 31)
        Me.Label2.TabIndex = 12
        Me.Label2.Text = "Daftar Jurnal Umum"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'cbStatus
        '
        Me.cbStatus.EditValue = "Semua"
        Me.cbStatus.Location = New System.Drawing.Point(57, 10)
        Me.cbStatus.Name = "cbStatus"
        Me.cbStatus.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.cbStatus.Properties.Items.AddRange(New Object() {"Semua", "Posting", "UnPosting"})
        Me.cbStatus.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor
        Me.cbStatus.Size = New System.Drawing.Size(255, 20)
        Me.cbStatus.TabIndex = 7
        '
        'LayoutControlGroup1
        '
        Me.LayoutControlGroup1.CustomizationFormText = "Root"
        Me.LayoutControlGroup1.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.[True]
        Me.LayoutControlGroup1.GroupBordersVisible = False
        Me.LayoutControlGroup1.Items.AddRange(New DevExpress.XtraLayout.BaseLayoutItem() {Me.LayoutControlItem1, Me.SplitterItem1, Me.LayoutControlItem2, Me.LayoutControlItem3, Me.SplitterItem2})
        Me.LayoutControlGroup1.Location = New System.Drawing.Point(0, 0)
        Me.LayoutControlGroup1.Name = "Root"
        Me.LayoutControlGroup1.Size = New System.Drawing.Size(841, 379)
        Me.LayoutControlGroup1.Text = "Root"
        Me.LayoutControlGroup1.TextVisible = False
        '
        'LayoutControlItem1
        '
        Me.LayoutControlItem1.Control = Me.PanelControl1
        Me.LayoutControlItem1.CustomizationFormText = "LayoutControlItem1"
        Me.LayoutControlItem1.Location = New System.Drawing.Point(0, 0)
        Me.LayoutControlItem1.MaxSize = New System.Drawing.Size(0, 96)
        Me.LayoutControlItem1.MinSize = New System.Drawing.Size(104, 96)
        Me.LayoutControlItem1.Name = "LayoutControlItem1"
        Me.LayoutControlItem1.Size = New System.Drawing.Size(821, 96)
        Me.LayoutControlItem1.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom
        Me.LayoutControlItem1.Text = "LayoutControlItem1"
        Me.LayoutControlItem1.TextSize = New System.Drawing.Size(0, 0)
        Me.LayoutControlItem1.TextToControlDistance = 0
        Me.LayoutControlItem1.TextVisible = False
        '
        'SplitterItem1
        '
        Me.SplitterItem1.AllowHotTrack = True
        Me.SplitterItem1.CustomizationFormText = "SplitterItem1"
        Me.SplitterItem1.Location = New System.Drawing.Point(0, 96)
        Me.SplitterItem1.Name = "SplitterItem1"
        Me.SplitterItem1.Size = New System.Drawing.Size(821, 5)
        '
        'LayoutControlItem2
        '
        Me.LayoutControlItem2.Control = Me.PanelControl3
        Me.LayoutControlItem2.CustomizationFormText = "LayoutControlItem2"
        Me.LayoutControlItem2.Location = New System.Drawing.Point(0, 101)
        Me.LayoutControlItem2.MaxSize = New System.Drawing.Size(137, 0)
        Me.LayoutControlItem2.MinSize = New System.Drawing.Size(137, 24)
        Me.LayoutControlItem2.Name = "LayoutControlItem2"
        Me.LayoutControlItem2.Size = New System.Drawing.Size(137, 258)
        Me.LayoutControlItem2.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom
        Me.LayoutControlItem2.Text = "LayoutControlItem2"
        Me.LayoutControlItem2.TextSize = New System.Drawing.Size(0, 0)
        Me.LayoutControlItem2.TextToControlDistance = 0
        Me.LayoutControlItem2.TextVisible = False
        '
        'LayoutControlItem3
        '
        Me.LayoutControlItem3.Control = Me.GridControl1
        Me.LayoutControlItem3.CustomizationFormText = "LayoutControlItem3"
        Me.LayoutControlItem3.Location = New System.Drawing.Point(142, 101)
        Me.LayoutControlItem3.Name = "LayoutControlItem3"
        Me.LayoutControlItem3.Size = New System.Drawing.Size(679, 258)
        Me.LayoutControlItem3.Text = "LayoutControlItem3"
        Me.LayoutControlItem3.TextSize = New System.Drawing.Size(0, 0)
        Me.LayoutControlItem3.TextToControlDistance = 0
        Me.LayoutControlItem3.TextVisible = False
        '
        'SplitterItem2
        '
        Me.SplitterItem2.AllowHotTrack = True
        Me.SplitterItem2.CustomizationFormText = "SplitterItem2"
        Me.SplitterItem2.Location = New System.Drawing.Point(137, 101)
        Me.SplitterItem2.Name = "SplitterItem2"
        Me.SplitterItem2.Size = New System.Drawing.Size(5, 258)
        '
        'frmDaftarJurnalUmum
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(841, 379)
        Me.Controls.Add(Me.LayoutControl1)
        Me.Controls.Add(Me.barDockControlLeft)
        Me.Controls.Add(Me.barDockControlRight)
        Me.Controls.Add(Me.barDockControlBottom)
        Me.Controls.Add(Me.barDockControlTop)
        Me.Name = "frmDaftarJurnalUmum"
        Me.ShowInTaskbar = False
        Me.Text = "Daftar Jurnal Umum"
        CType(Me.GridControl1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.GridView1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RepositoryItemCheckEdit1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RepositoryItemCheckEdit2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.BarManager1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RepositoryItemTextEdit1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PopupMenu1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControl1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.LayoutControl1.ResumeLayout(False)
        CType(Me.PanelControl3, System.ComponentModel.ISupportInitialize).EndInit()
        Me.PanelControl3.ResumeLayout(False)
        CType(Me.PanelControl1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.PanelControl1.ResumeLayout(False)
        Me.PanelControl1.PerformLayout()
        CType(Me.TglSampai.Properties.VistaTimeProperties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.TglSampai.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.tglDari.Properties.VistaTimeProperties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.tglDari.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.cbStatus.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlGroup1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.SplitterItem1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem3, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.SplitterItem2, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents BarManager1 As DevExpress.XtraBars.BarManager
    Friend WithEvents BarSubItem1 As DevExpress.XtraBars.BarSubItem
    Friend WithEvents BarSubItem2 As DevExpress.XtraBars.BarSubItem
    Friend WithEvents mnPosting As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents mnUnPosting As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents BarSubItem5 As DevExpress.XtraBars.BarSubItem
    Friend WithEvents BarButtonItem10 As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents BarButtonItem11 As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents barDockControlTop As DevExpress.XtraBars.BarDockControl
    Friend WithEvents barDockControlBottom As DevExpress.XtraBars.BarDockControl
    Friend WithEvents barDockControlLeft As DevExpress.XtraBars.BarDockControl
    Friend WithEvents barDockControlRight As DevExpress.XtraBars.BarDockControl
    Friend WithEvents RepositoryItemTextEdit1 As DevExpress.XtraEditors.Repository.RepositoryItemTextEdit
    Friend WithEvents PopupMenu1 As DevExpress.XtraBars.PopupMenu
    Friend WithEvents PanelControl1 As DevExpress.XtraEditors.PanelControl
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents LabelControl4 As DevExpress.XtraEditors.LabelControl
    Friend WithEvents LabelControl5 As DevExpress.XtraEditors.LabelControl
    Friend WithEvents TglSampai As DevExpress.XtraEditors.DateEdit
    Friend WithEvents tglDari As DevExpress.XtraEditors.DateEdit
    Friend WithEvents LabelControl3 As DevExpress.XtraEditors.LabelControl
    Friend WithEvents LayoutControl1 As DevExpress.XtraLayout.LayoutControl
    Friend WithEvents LayoutControlGroup1 As DevExpress.XtraLayout.LayoutControlGroup
    Friend WithEvents PanelControl3 As DevExpress.XtraEditors.PanelControl
    Friend WithEvents cmdExport As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents cmdRefresh As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents cmdDelete As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents cmdEdit As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents cmdClose As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents cmdNew As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents cmdMark As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents LayoutControlItem1 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents SplitterItem1 As DevExpress.XtraLayout.SplitterItem
    Friend WithEvents LayoutControlItem2 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents GridControl1 As DevExpress.XtraGrid.GridControl
    Friend WithEvents GridView1 As DevExpress.XtraGrid.Views.Grid.GridView
    Friend WithEvents RepositoryItemCheckEdit1 As DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit
    Friend WithEvents RepositoryItemCheckEdit2 As DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit
    Friend WithEvents LayoutControlItem3 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents SplitterItem2 As DevExpress.XtraLayout.SplitterItem
    Friend WithEvents BarSubItem3 As DevExpress.XtraBars.BarSubItem
    Friend WithEvents mnFaktur As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents cbStatus As DevExpress.XtraEditors.ComboBoxEdit
    Friend WithEvents mnPerbaikiData As DevExpress.XtraBars.BarButtonItem
End Class
