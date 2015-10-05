<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmEntriPembayaranPiutangOK
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
        Dim SerializableAppearanceObject1 As DevExpress.Utils.SerializableAppearanceObject = New DevExpress.Utils.SerializableAppearanceObject
        Dim SerializableAppearanceObject2 As DevExpress.Utils.SerializableAppearanceObject = New DevExpress.Utils.SerializableAppearanceObject
        Me.LayoutControl1 = New DevExpress.XtraLayout.LayoutControl
        Me.cmdAddDN = New DevExpress.XtraEditors.SimpleButton
        Me.cmdDelDN = New DevExpress.XtraEditors.SimpleButton
        Me.cmdDelCN = New DevExpress.XtraEditors.SimpleButton
        Me.cmdAddCN = New DevExpress.XtraEditors.SimpleButton
        Me.txtKeteranganKwitansi = New DevExpress.XtraEditors.TextEdit
        Me.txtAkunKwitansi = New DevExpress.XtraEditors.SearchLookUpEdit
        Me.BarManager1 = New DevExpress.XtraBars.BarManager(Me.components)
        Me.Bar2 = New DevExpress.XtraBars.Bar
        Me.BarSubItem1 = New DevExpress.XtraBars.BarSubItem
        Me.BarButtonItem1 = New DevExpress.XtraBars.BarButtonItem
        Me.BarButtonItem3 = New DevExpress.XtraBars.BarButtonItem
        Me.BarButtonItem2 = New DevExpress.XtraBars.BarButtonItem
        Me.BarButtonItem4 = New DevExpress.XtraBars.BarButtonItem
        Me.BarButtonItem5 = New DevExpress.XtraBars.BarButtonItem
        Me.BarButtonItem6 = New DevExpress.XtraBars.BarButtonItem
        Me.BarButtonItem7 = New DevExpress.XtraBars.BarButtonItem
        Me.Bar3 = New DevExpress.XtraBars.Bar
        Me.barDockControlTop = New DevExpress.XtraBars.BarDockControl
        Me.barDockControlBottom = New DevExpress.XtraBars.BarDockControl
        Me.barDockControlLeft = New DevExpress.XtraBars.BarDockControl
        Me.barDockControlRight = New DevExpress.XtraBars.BarDockControl
        Me.mnCancel = New DevExpress.XtraBars.BarButtonItem
        Me.GridView1 = New DevExpress.XtraGrid.Views.Grid.GridView
        Me.txtNoKwitansi = New DevExpress.XtraEditors.TextEdit
        Me.txtDN = New DevExpress.XtraEditors.TextEdit
        Me.txtJumlah = New DevExpress.XtraEditors.TextEdit
        Me.txtMaterai = New DevExpress.XtraEditors.TextEdit
        Me.txtPotongan = New DevExpress.XtraEditors.TextEdit
        Me.txtKwitansi = New DevExpress.XtraEditors.SearchLookUpEdit
        Me.gvKwitansiMasuk = New DevExpress.XtraGrid.Views.Grid.GridView
        Me.txtTglTransfer = New DevExpress.XtraEditors.DateEdit
        Me.txtNoACC = New DevExpress.XtraEditors.TextEdit
        Me.txtJumlahKwitansi = New DevExpress.XtraEditors.TextEdit
        Me.txtBayar = New DevExpress.XtraEditors.TextEdit
        Me.gcGiro = New DevExpress.XtraGrid.GridControl
        Me.gvGiro = New DevExpress.XtraGrid.Views.Grid.GridView
        Me.gcKredit = New DevExpress.XtraGrid.GridControl
        Me.gvKredit = New DevExpress.XtraGrid.Views.Grid.GridView
        Me.gnkNoID = New DevExpress.XtraGrid.Columns.GridColumn
        Me.gnkKode = New DevExpress.XtraGrid.Columns.GridColumn
        Me.gnkTanggal = New DevExpress.XtraGrid.Columns.GridColumn
        Me.gnkTotal = New DevExpress.XtraGrid.Columns.GridColumn
        Me.gnkPotong = New DevExpress.XtraGrid.Columns.GridColumn
        Me.gnkCatatan = New DevExpress.XtraGrid.Columns.GridColumn
        Me.gcDebet = New DevExpress.XtraGrid.GridControl
        Me.gvDebet = New DevExpress.XtraGrid.Views.Grid.GridView
        Me.cdnNoID = New DevExpress.XtraGrid.Columns.GridColumn
        Me.cdnKode = New DevExpress.XtraGrid.Columns.GridColumn
        Me.cdnTanggal = New DevExpress.XtraGrid.Columns.GridColumn
        Me.cdnKeterangan = New DevExpress.XtraGrid.Columns.GridColumn
        Me.cdnTotal = New DevExpress.XtraGrid.Columns.GridColumn
        Me.cdnPotong = New DevExpress.XtraGrid.Columns.GridColumn
        Me.gcPH = New DevExpress.XtraGrid.GridControl
        Me.gvPH = New DevExpress.XtraGrid.Views.Grid.GridView
        Me.cPNoID = New DevExpress.XtraGrid.Columns.GridColumn
        Me.CPIDPH = New DevExpress.XtraGrid.Columns.GridColumn
        Me.CpNoPH = New DevExpress.XtraGrid.Columns.GridColumn
        Me.CpTanggal = New DevExpress.XtraGrid.Columns.GridColumn
        Me.CpJumlah = New DevExpress.XtraGrid.Columns.GridColumn
        Me.CpPotong = New DevExpress.XtraGrid.Columns.GridColumn
        Me.gcTU = New DevExpress.XtraGrid.GridControl
        Me.gvTU = New DevExpress.XtraGrid.Views.Grid.GridView
        Me.cbNoID = New DevExpress.XtraGrid.Columns.GridColumn
        Me.cbNomor = New DevExpress.XtraGrid.Columns.GridColumn
        Me.cbTanggal = New DevExpress.XtraGrid.Columns.GridColumn
        Me.cbJumlah = New DevExpress.XtraGrid.Columns.GridColumn
        Me.cbPotong = New DevExpress.XtraGrid.Columns.GridColumn
        Me.gcPembelian = New DevExpress.XtraGrid.GridControl
        Me.gvPembelian = New DevExpress.XtraGrid.Views.Grid.GridView
        Me.cNoID = New DevExpress.XtraGrid.Columns.GridColumn
        Me.cIDBeli = New DevExpress.XtraGrid.Columns.GridColumn
        Me.cNoNota = New DevExpress.XtraGrid.Columns.GridColumn
        Me.cTanggal = New DevExpress.XtraGrid.Columns.GridColumn
        Me.CTotal = New DevExpress.XtraGrid.Columns.GridColumn
        Me.cBayar = New DevExpress.XtraGrid.Columns.GridColumn
        Me.txtWilayah = New DevExpress.XtraEditors.SearchLookUpEdit
        Me.gvWilayah = New DevExpress.XtraGrid.Views.Grid.GridView
        Me.tglPosting = New DevExpress.XtraEditors.DateEdit
        Me.tglEdit = New DevExpress.XtraEditors.DateEdit
        Me.tglEntri = New DevExpress.XtraEditors.DateEdit
        Me.cmdLoad = New DevExpress.XtraEditors.SimpleButton
        Me.txtDipostingOleh = New DevExpress.XtraEditors.ButtonEdit
        Me.txtDieditOleh = New DevExpress.XtraEditors.ButtonEdit
        Me.txtDientriOleh = New DevExpress.XtraEditors.ButtonEdit
        Me.cmdRefresh = New DevExpress.XtraEditors.SimpleButton
        Me.cmdDelete = New DevExpress.XtraEditors.SimpleButton
        Me.cmdEdit = New DevExpress.XtraEditors.SimpleButton
        Me.cmdBAru = New DevExpress.XtraEditors.SimpleButton
        Me.txtKodeAlamat = New DevExpress.XtraEditors.SearchLookUpEdit
        Me.SearchLookUpEdit1View = New DevExpress.XtraGrid.Views.Grid.GridView
        Me.txtKode = New DevExpress.XtraEditors.ButtonEdit
        Me.Tgl = New DevExpress.XtraEditors.DateEdit
        Me.txtBarang = New DevExpress.XtraEditors.ButtonEdit
        Me.gcRetur = New DevExpress.XtraGrid.GridControl
        Me.gvRetur = New DevExpress.XtraGrid.Views.Grid.GridView
        Me.cRNoID = New DevExpress.XtraGrid.Columns.GridColumn
        Me.CrIDRetur = New DevExpress.XtraGrid.Columns.GridColumn
        Me.crNoRetur = New DevExpress.XtraGrid.Columns.GridColumn
        Me.CrTanggal = New DevExpress.XtraGrid.Columns.GridColumn
        Me.CrTotal = New DevExpress.XtraGrid.Columns.GridColumn
        Me.CrPotong = New DevExpress.XtraGrid.Columns.GridColumn
        Me.cmdSave = New DevExpress.XtraEditors.SimpleButton
        Me.cmdTutup = New DevExpress.XtraEditors.SimpleButton
        Me.txtSubTotal = New DevExpress.XtraEditors.TextEdit
        Me.txtNamaAlamat = New DevExpress.XtraEditors.TextEdit
        Me.txtCatatan = New DevExpress.XtraEditors.MemoEdit
        Me.txtAlamat = New DevExpress.XtraEditors.MemoEdit
        Me.LayoutControlItem31 = New DevExpress.XtraLayout.LayoutControlItem
        Me.LayoutControlItem32 = New DevExpress.XtraLayout.LayoutControlItem
        Me.LayoutControlItem27 = New DevExpress.XtraLayout.LayoutControlItem
        Me.LayoutControlItem28 = New DevExpress.XtraLayout.LayoutControlItem
        Me.LayoutControlItem29 = New DevExpress.XtraLayout.LayoutControlItem
        Me.LayoutControlItem13 = New DevExpress.XtraLayout.LayoutControlItem
        Me.LayoutControlItem30 = New DevExpress.XtraLayout.LayoutControlItem
        Me.LayoutControlItem33 = New DevExpress.XtraLayout.LayoutControlItem
        Me.LayoutControlItem36 = New DevExpress.XtraLayout.LayoutControlItem
        Me.LayoutControlItem34 = New DevExpress.XtraLayout.LayoutControlItem
        Me.LayoutControlGroup9 = New DevExpress.XtraLayout.LayoutControlGroup
        Me.LayoutControlItem11 = New DevExpress.XtraLayout.LayoutControlItem
        Me.LayoutControlGroup6 = New DevExpress.XtraLayout.LayoutControlGroup
        Me.LayoutControlItem8 = New DevExpress.XtraLayout.LayoutControlItem
        Me.LayoutControlGroup5 = New DevExpress.XtraLayout.LayoutControlGroup
        Me.LayoutControlItem7 = New DevExpress.XtraLayout.LayoutControlItem
        Me.LayoutControlItem22 = New DevExpress.XtraLayout.LayoutControlItem
        Me.LayoutControlGroup1 = New DevExpress.XtraLayout.LayoutControlGroup
        Me.LayoutControlItem1 = New DevExpress.XtraLayout.LayoutControlItem
        Me.EmptySpaceItem2 = New DevExpress.XtraLayout.EmptySpaceItem
        Me.LayoutControlItem2 = New DevExpress.XtraLayout.LayoutControlItem
        Me.LayoutControlItem15 = New DevExpress.XtraLayout.LayoutControlItem
        Me.LayoutControlItem14 = New DevExpress.XtraLayout.LayoutControlItem
        Me.LayoutControlItem18 = New DevExpress.XtraLayout.LayoutControlItem
        Me.LayoutControlItem19 = New DevExpress.XtraLayout.LayoutControlItem
        Me.LayoutControlItem23 = New DevExpress.XtraLayout.LayoutControlItem
        Me.LayoutControlItem24 = New DevExpress.XtraLayout.LayoutControlItem
        Me.LayoutControlItem25 = New DevExpress.XtraLayout.LayoutControlItem
        Me.LayoutControlItem26 = New DevExpress.XtraLayout.LayoutControlItem
        Me.LayoutControlGroup2 = New DevExpress.XtraLayout.LayoutControlGroup
        Me.LayoutControlItem4 = New DevExpress.XtraLayout.LayoutControlItem
        Me.LayoutControlItem16 = New DevExpress.XtraLayout.LayoutControlItem
        Me.LayoutControlItem17 = New DevExpress.XtraLayout.LayoutControlItem
        Me.LayoutControlItem20 = New DevExpress.XtraLayout.LayoutControlItem
        Me.LayoutControlItem37 = New DevExpress.XtraLayout.LayoutControlItem
        Me.LayoutControlItem38 = New DevExpress.XtraLayout.LayoutControlItem
        Me.LayoutControlItem39 = New DevExpress.XtraLayout.LayoutControlItem
        Me.LayoutControlItem40 = New DevExpress.XtraLayout.LayoutControlItem
        Me.LayoutControlItem5 = New DevExpress.XtraLayout.LayoutControlItem
        Me.LayoutControlGroup3 = New DevExpress.XtraLayout.LayoutControlGroup
        Me.LayoutControlItem3 = New DevExpress.XtraLayout.LayoutControlItem
        Me.LayoutControlGroup4 = New DevExpress.XtraLayout.LayoutControlGroup
        Me.LayoutControlItem6 = New DevExpress.XtraLayout.LayoutControlItem
        Me.SplitterItem4 = New DevExpress.XtraLayout.SplitterItem
        Me.SplitterItem6 = New DevExpress.XtraLayout.SplitterItem
        Me.EmptySpaceItem1 = New DevExpress.XtraLayout.EmptySpaceItem
        Me.LayoutControlGroup7 = New DevExpress.XtraLayout.LayoutControlGroup
        Me.LayoutControlItem42 = New DevExpress.XtraLayout.LayoutControlItem
        Me.LayoutControlItem43 = New DevExpress.XtraLayout.LayoutControlItem
        Me.LayoutControlItem44 = New DevExpress.XtraLayout.LayoutControlItem
        Me.LayoutControlItem35 = New DevExpress.XtraLayout.LayoutControlItem
        Me.EmptySpaceItem3 = New DevExpress.XtraLayout.EmptySpaceItem
        Me.LayoutControlGroup8 = New DevExpress.XtraLayout.LayoutControlGroup
        Me.SplitterItem1 = New DevExpress.XtraLayout.SplitterItem
        Me.LayoutControlGroup10 = New DevExpress.XtraLayout.LayoutControlGroup
        Me.LayoutControlItem46 = New DevExpress.XtraLayout.LayoutControlItem
        Me.LayoutControlItem45 = New DevExpress.XtraLayout.LayoutControlItem
        Me.LayoutControlItem10 = New DevExpress.XtraLayout.LayoutControlItem
        Me.LayoutControlGroup11 = New DevExpress.XtraLayout.LayoutControlGroup
        Me.LayoutControlItem9 = New DevExpress.XtraLayout.LayoutControlItem
        Me.LayoutControlItem47 = New DevExpress.XtraLayout.LayoutControlItem
        Me.LayoutControlItem48 = New DevExpress.XtraLayout.LayoutControlItem
        Me.EmptySpaceItem6 = New DevExpress.XtraLayout.EmptySpaceItem
        Me.LayoutControlItem12 = New DevExpress.XtraLayout.LayoutControlItem
        Me.cKassa = New DevExpress.XtraGrid.Columns.GridColumn
        CType(Me.LayoutControl1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.LayoutControl1.SuspendLayout()
        CType(Me.txtKeteranganKwitansi.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtAkunKwitansi.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.BarManager1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.GridView1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtNoKwitansi.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtDN.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtJumlah.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtMaterai.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtPotongan.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtKwitansi.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.gvKwitansiMasuk, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtTglTransfer.Properties.VistaTimeProperties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtTglTransfer.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtNoACC.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtJumlahKwitansi.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtBayar.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.gcGiro, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.gvGiro, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.gcKredit, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.gvKredit, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.gcDebet, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.gvDebet, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.gcPH, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.gvPH, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.gcTU, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.gvTU, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.gcPembelian, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.gvPembelian, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtWilayah.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.gvWilayah, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.tglPosting.Properties.VistaTimeProperties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.tglPosting.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.tglEdit.Properties.VistaTimeProperties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.tglEdit.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.tglEntri.Properties.VistaTimeProperties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.tglEntri.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtDipostingOleh.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtDieditOleh.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtDientriOleh.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtKodeAlamat.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.SearchLookUpEdit1View, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtKode.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Tgl.Properties.VistaTimeProperties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Tgl.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtBarang.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.gcRetur, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.gvRetur, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtSubTotal.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtNamaAlamat.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtCatatan.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtAlamat.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem31, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem32, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem27, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem28, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem29, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem13, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem30, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem33, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem36, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem34, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlGroup9, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem11, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlGroup6, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem8, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlGroup5, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem7, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem22, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlGroup1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.EmptySpaceItem2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem15, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem14, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem18, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem19, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem23, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem24, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem25, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem26, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlGroup2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem4, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem16, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem17, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem20, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem37, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem38, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem39, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem40, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem5, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlGroup3, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem3, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlGroup4, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem6, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.SplitterItem4, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.SplitterItem6, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.EmptySpaceItem1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlGroup7, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem42, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem43, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem44, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem35, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.EmptySpaceItem3, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlGroup8, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.SplitterItem1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlGroup10, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem46, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem45, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem10, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlGroup11, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem9, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem47, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem48, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.EmptySpaceItem6, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem12, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'LayoutControl1
        '
        Me.LayoutControl1.Controls.Add(Me.cmdAddDN)
        Me.LayoutControl1.Controls.Add(Me.cmdDelDN)
        Me.LayoutControl1.Controls.Add(Me.cmdDelCN)
        Me.LayoutControl1.Controls.Add(Me.cmdAddCN)
        Me.LayoutControl1.Controls.Add(Me.txtKeteranganKwitansi)
        Me.LayoutControl1.Controls.Add(Me.txtAkunKwitansi)
        Me.LayoutControl1.Controls.Add(Me.txtNoKwitansi)
        Me.LayoutControl1.Controls.Add(Me.txtDN)
        Me.LayoutControl1.Controls.Add(Me.txtJumlah)
        Me.LayoutControl1.Controls.Add(Me.txtMaterai)
        Me.LayoutControl1.Controls.Add(Me.txtPotongan)
        Me.LayoutControl1.Controls.Add(Me.txtKwitansi)
        Me.LayoutControl1.Controls.Add(Me.txtTglTransfer)
        Me.LayoutControl1.Controls.Add(Me.txtNoACC)
        Me.LayoutControl1.Controls.Add(Me.txtJumlahKwitansi)
        Me.LayoutControl1.Controls.Add(Me.txtBayar)
        Me.LayoutControl1.Controls.Add(Me.gcGiro)
        Me.LayoutControl1.Controls.Add(Me.gcKredit)
        Me.LayoutControl1.Controls.Add(Me.gcDebet)
        Me.LayoutControl1.Controls.Add(Me.gcPH)
        Me.LayoutControl1.Controls.Add(Me.gcTU)
        Me.LayoutControl1.Controls.Add(Me.gcPembelian)
        Me.LayoutControl1.Controls.Add(Me.txtWilayah)
        Me.LayoutControl1.Controls.Add(Me.tglPosting)
        Me.LayoutControl1.Controls.Add(Me.tglEdit)
        Me.LayoutControl1.Controls.Add(Me.tglEntri)
        Me.LayoutControl1.Controls.Add(Me.cmdLoad)
        Me.LayoutControl1.Controls.Add(Me.txtDipostingOleh)
        Me.LayoutControl1.Controls.Add(Me.txtDieditOleh)
        Me.LayoutControl1.Controls.Add(Me.txtDientriOleh)
        Me.LayoutControl1.Controls.Add(Me.cmdRefresh)
        Me.LayoutControl1.Controls.Add(Me.cmdDelete)
        Me.LayoutControl1.Controls.Add(Me.cmdEdit)
        Me.LayoutControl1.Controls.Add(Me.cmdBAru)
        Me.LayoutControl1.Controls.Add(Me.txtKodeAlamat)
        Me.LayoutControl1.Controls.Add(Me.txtKode)
        Me.LayoutControl1.Controls.Add(Me.Tgl)
        Me.LayoutControl1.Controls.Add(Me.txtBarang)
        Me.LayoutControl1.Controls.Add(Me.gcRetur)
        Me.LayoutControl1.Controls.Add(Me.cmdSave)
        Me.LayoutControl1.Controls.Add(Me.cmdTutup)
        Me.LayoutControl1.Controls.Add(Me.txtSubTotal)
        Me.LayoutControl1.Controls.Add(Me.txtNamaAlamat)
        Me.LayoutControl1.Controls.Add(Me.txtCatatan)
        Me.LayoutControl1.Controls.Add(Me.txtAlamat)
        Me.LayoutControl1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.LayoutControl1.HiddenItems.AddRange(New DevExpress.XtraLayout.BaseLayoutItem() {Me.LayoutControlItem31, Me.LayoutControlItem32, Me.LayoutControlItem27, Me.LayoutControlItem28, Me.LayoutControlItem29, Me.LayoutControlItem13, Me.LayoutControlItem30, Me.LayoutControlItem33, Me.LayoutControlItem36, Me.LayoutControlItem34, Me.LayoutControlGroup9, Me.LayoutControlGroup6, Me.LayoutControlGroup5, Me.LayoutControlItem22})
        Me.LayoutControl1.Location = New System.Drawing.Point(0, 23)
        Me.LayoutControl1.Name = "LayoutControl1"
        Me.LayoutControl1.OptionsCustomizationForm.DesignTimeCustomizationFormPositionAndSize = New System.Drawing.Rectangle(715, 36, 250, 585)
        Me.LayoutControl1.Root = Me.LayoutControlGroup1
        Me.LayoutControl1.Size = New System.Drawing.Size(1020, 692)
        Me.LayoutControl1.TabIndex = 0
        Me.LayoutControl1.Text = "LayoutControl1"
        '
        'cmdAddDN
        '
        Me.cmdAddDN.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdAddDN.Appearance.Font = New System.Drawing.Font("Tahoma", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdAddDN.Appearance.ForeColor = System.Drawing.Color.Blue
        Me.cmdAddDN.Appearance.Options.UseFont = True
        Me.cmdAddDN.Appearance.Options.UseForeColor = True
        Me.cmdAddDN.ImageIndex = 10
        Me.cmdAddDN.Location = New System.Drawing.Point(579, 493)
        Me.cmdAddDN.Name = "cmdAddDN"
        Me.cmdAddDN.Size = New System.Drawing.Size(23, 25)
        Me.cmdAddDN.StyleController = Me.LayoutControl1
        Me.cmdAddDN.TabIndex = 21
        Me.cmdAddDN.Text = "+"
        '
        'cmdDelDN
        '
        Me.cmdDelDN.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdDelDN.Appearance.Font = New System.Drawing.Font("Tahoma", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdDelDN.Appearance.ForeColor = System.Drawing.Color.Blue
        Me.cmdDelDN.Appearance.Options.UseFont = True
        Me.cmdDelDN.Appearance.Options.UseForeColor = True
        Me.cmdDelDN.ImageIndex = 10
        Me.cmdDelDN.Location = New System.Drawing.Point(579, 522)
        Me.cmdDelDN.Name = "cmdDelDN"
        Me.cmdDelDN.Size = New System.Drawing.Size(23, 25)
        Me.cmdDelDN.StyleController = Me.LayoutControl1
        Me.cmdDelDN.TabIndex = 22
        Me.cmdDelDN.Text = "-"
        '
        'cmdDelCN
        '
        Me.cmdDelCN.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdDelCN.Appearance.Font = New System.Drawing.Font("Tahoma", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdDelCN.Appearance.ForeColor = System.Drawing.Color.Blue
        Me.cmdDelCN.Appearance.Options.UseFont = True
        Me.cmdDelCN.Appearance.Options.UseForeColor = True
        Me.cmdDelCN.ImageIndex = 10
        Me.cmdDelCN.Location = New System.Drawing.Point(419, 522)
        Me.cmdDelCN.Name = "cmdDelCN"
        Me.cmdDelCN.Size = New System.Drawing.Size(23, 25)
        Me.cmdDelCN.StyleController = Me.LayoutControl1
        Me.cmdDelCN.TabIndex = 21
        Me.cmdDelCN.Text = "-"
        '
        'cmdAddCN
        '
        Me.cmdAddCN.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdAddCN.Appearance.Font = New System.Drawing.Font("Tahoma", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdAddCN.Appearance.ForeColor = System.Drawing.Color.Blue
        Me.cmdAddCN.Appearance.Options.UseFont = True
        Me.cmdAddCN.Appearance.Options.UseForeColor = True
        Me.cmdAddCN.ImageIndex = 10
        Me.cmdAddCN.Location = New System.Drawing.Point(419, 493)
        Me.cmdAddCN.Name = "cmdAddCN"
        Me.cmdAddCN.Size = New System.Drawing.Size(23, 25)
        Me.cmdAddCN.StyleController = Me.LayoutControl1
        Me.cmdAddCN.TabIndex = 20
        Me.cmdAddCN.Text = "+"
        '
        'txtKeteranganKwitansi
        '
        Me.txtKeteranganKwitansi.EnterMoveNextControl = True
        Me.txtKeteranganKwitansi.Location = New System.Drawing.Point(144, 533)
        Me.txtKeteranganKwitansi.Name = "txtKeteranganKwitansi"
        Me.txtKeteranganKwitansi.Properties.Appearance.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtKeteranganKwitansi.Properties.Appearance.Options.UseFont = True
        Me.txtKeteranganKwitansi.Size = New System.Drawing.Size(131, 22)
        Me.txtKeteranganKwitansi.StyleController = Me.LayoutControl1
        Me.txtKeteranganKwitansi.TabIndex = 8
        '
        'txtAkunKwitansi
        '
        Me.txtAkunKwitansi.EditValue = ""
        Me.txtAkunKwitansi.EnterMoveNextControl = True
        Me.txtAkunKwitansi.Location = New System.Drawing.Point(144, 507)
        Me.txtAkunKwitansi.MenuManager = Me.BarManager1
        Me.txtAkunKwitansi.Name = "txtAkunKwitansi"
        Me.txtAkunKwitansi.Properties.Appearance.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtAkunKwitansi.Properties.Appearance.Options.UseFont = True
        Me.txtAkunKwitansi.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.txtAkunKwitansi.Properties.DisplayMember = "Kode"
        Me.txtAkunKwitansi.Properties.NullText = ""
        Me.txtAkunKwitansi.Properties.ValueMember = "NoID"
        Me.txtAkunKwitansi.Properties.View = Me.GridView1
        Me.txtAkunKwitansi.Size = New System.Drawing.Size(131, 22)
        Me.txtAkunKwitansi.StyleController = Me.LayoutControl1
        Me.txtAkunKwitansi.TabIndex = 6
        '
        'BarManager1
        '
        Me.BarManager1.Bars.AddRange(New DevExpress.XtraBars.Bar() {Me.Bar2, Me.Bar3})
        Me.BarManager1.DockControls.Add(Me.barDockControlTop)
        Me.BarManager1.DockControls.Add(Me.barDockControlBottom)
        Me.BarManager1.DockControls.Add(Me.barDockControlLeft)
        Me.BarManager1.DockControls.Add(Me.barDockControlRight)
        Me.BarManager1.Form = Me
        Me.BarManager1.Items.AddRange(New DevExpress.XtraBars.BarItem() {Me.BarSubItem1, Me.BarButtonItem1, Me.BarButtonItem3, Me.mnCancel, Me.BarButtonItem2, Me.BarButtonItem4, Me.BarButtonItem5, Me.BarButtonItem6, Me.BarButtonItem7})
        Me.BarManager1.MainMenu = Me.Bar2
        Me.BarManager1.MaxItemId = 10
        Me.BarManager1.StatusBar = Me.Bar3
        '
        'Bar2
        '
        Me.Bar2.BarName = "Main menu"
        Me.Bar2.DockCol = 0
        Me.Bar2.DockRow = 0
        Me.Bar2.DockStyle = DevExpress.XtraBars.BarDockStyle.Top
        Me.Bar2.LinksPersistInfo.AddRange(New DevExpress.XtraBars.LinkPersistInfo() {New DevExpress.XtraBars.LinkPersistInfo(Me.BarSubItem1)})
        Me.Bar2.OptionsBar.MultiLine = True
        Me.Bar2.OptionsBar.UseWholeRow = True
        Me.Bar2.Text = "Main menu"
        Me.Bar2.Visible = False
        '
        'BarSubItem1
        '
        Me.BarSubItem1.Caption = "&Menu"
        Me.BarSubItem1.Id = 0
        Me.BarSubItem1.LinksPersistInfo.AddRange(New DevExpress.XtraBars.LinkPersistInfo() {New DevExpress.XtraBars.LinkPersistInfo(Me.BarButtonItem1), New DevExpress.XtraBars.LinkPersistInfo(Me.BarButtonItem3), New DevExpress.XtraBars.LinkPersistInfo(Me.BarButtonItem2), New DevExpress.XtraBars.LinkPersistInfo(Me.BarButtonItem4), New DevExpress.XtraBars.LinkPersistInfo(Me.BarButtonItem5), New DevExpress.XtraBars.LinkPersistInfo(Me.BarButtonItem6), New DevExpress.XtraBars.LinkPersistInfo(Me.BarButtonItem7)})
        Me.BarSubItem1.Name = "BarSubItem1"
        '
        'BarButtonItem1
        '
        Me.BarButtonItem1.Caption = "&Simpan"
        Me.BarButtonItem1.Id = 1
        Me.BarButtonItem1.ItemShortcut = New DevExpress.XtraBars.BarShortcut(System.Windows.Forms.Keys.F6)
        Me.BarButtonItem1.Name = "BarButtonItem1"
        '
        'BarButtonItem3
        '
        Me.BarButtonItem3.Caption = "&Simpan Layout"
        Me.BarButtonItem3.Id = 3
        Me.BarButtonItem3.ItemShortcut = New DevExpress.XtraBars.BarShortcut(System.Windows.Forms.Keys.F10)
        Me.BarButtonItem3.Name = "BarButtonItem3"
        '
        'BarButtonItem2
        '
        Me.BarButtonItem2.Caption = "&Baru"
        Me.BarButtonItem2.Id = 5
        Me.BarButtonItem2.ItemShortcut = New DevExpress.XtraBars.BarShortcut(System.Windows.Forms.Keys.F1)
        Me.BarButtonItem2.Name = "BarButtonItem2"
        '
        'BarButtonItem4
        '
        Me.BarButtonItem4.Caption = "Edit"
        Me.BarButtonItem4.Id = 6
        Me.BarButtonItem4.ItemShortcut = New DevExpress.XtraBars.BarShortcut(System.Windows.Forms.Keys.F2)
        Me.BarButtonItem4.Name = "BarButtonItem4"
        '
        'BarButtonItem5
        '
        Me.BarButtonItem5.Caption = "Hapus"
        Me.BarButtonItem5.Id = 7
        Me.BarButtonItem5.ItemShortcut = New DevExpress.XtraBars.BarShortcut(System.Windows.Forms.Keys.F4)
        Me.BarButtonItem5.Name = "BarButtonItem5"
        '
        'BarButtonItem6
        '
        Me.BarButtonItem6.Caption = "Refresh"
        Me.BarButtonItem6.Id = 8
        Me.BarButtonItem6.ItemShortcut = New DevExpress.XtraBars.BarShortcut(System.Windows.Forms.Keys.F5)
        Me.BarButtonItem6.Name = "BarButtonItem6"
        '
        'BarButtonItem7
        '
        Me.BarButtonItem7.Caption = "Close"
        Me.BarButtonItem7.Id = 9
        Me.BarButtonItem7.ItemShortcut = New DevExpress.XtraBars.BarShortcut(System.Windows.Forms.Keys.F3)
        Me.BarButtonItem7.Name = "BarButtonItem7"
        '
        'Bar3
        '
        Me.Bar3.BarName = "Status bar"
        Me.Bar3.CanDockStyle = DevExpress.XtraBars.BarCanDockStyle.Bottom
        Me.Bar3.DockCol = 0
        Me.Bar3.DockRow = 0
        Me.Bar3.DockStyle = DevExpress.XtraBars.BarDockStyle.Bottom
        Me.Bar3.OptionsBar.AllowQuickCustomization = False
        Me.Bar3.OptionsBar.DrawDragBorder = False
        Me.Bar3.OptionsBar.UseWholeRow = True
        Me.Bar3.Text = "Status bar"
        Me.Bar3.Visible = False
        '
        'barDockControlTop
        '
        Me.barDockControlTop.CausesValidation = False
        Me.barDockControlTop.Dock = System.Windows.Forms.DockStyle.Top
        Me.barDockControlTop.Location = New System.Drawing.Point(0, 0)
        Me.barDockControlTop.Size = New System.Drawing.Size(1020, 23)
        '
        'barDockControlBottom
        '
        Me.barDockControlBottom.CausesValidation = False
        Me.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.barDockControlBottom.Location = New System.Drawing.Point(0, 715)
        Me.barDockControlBottom.Size = New System.Drawing.Size(1020, 27)
        '
        'barDockControlLeft
        '
        Me.barDockControlLeft.CausesValidation = False
        Me.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left
        Me.barDockControlLeft.Location = New System.Drawing.Point(0, 23)
        Me.barDockControlLeft.Size = New System.Drawing.Size(0, 692)
        '
        'barDockControlRight
        '
        Me.barDockControlRight.CausesValidation = False
        Me.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right
        Me.barDockControlRight.Location = New System.Drawing.Point(1020, 23)
        Me.barDockControlRight.Size = New System.Drawing.Size(0, 692)
        '
        'mnCancel
        '
        Me.mnCancel.Caption = "&Cancel"
        Me.mnCancel.Id = 4
        Me.mnCancel.ItemShortcut = New DevExpress.XtraBars.BarShortcut(System.Windows.Forms.Keys.F6)
        Me.mnCancel.Name = "mnCancel"
        '
        'GridView1
        '
        Me.GridView1.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus
        Me.GridView1.Name = "GridView1"
        Me.GridView1.OptionsSelection.EnableAppearanceFocusedCell = False
        Me.GridView1.OptionsView.ShowGroupPanel = False
        '
        'txtNoKwitansi
        '
        Me.txtNoKwitansi.EnterMoveNextControl = True
        Me.txtNoKwitansi.Location = New System.Drawing.Point(144, 481)
        Me.txtNoKwitansi.Name = "txtNoKwitansi"
        Me.txtNoKwitansi.Properties.Appearance.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtNoKwitansi.Properties.Appearance.Options.UseFont = True
        Me.txtNoKwitansi.Size = New System.Drawing.Size(131, 22)
        Me.txtNoKwitansi.StyleController = Me.LayoutControl1
        Me.txtNoKwitansi.TabIndex = 7
        '
        'txtDN
        '
        Me.txtDN.EditValue = New Decimal(New Integer() {0, 0, 0, 0})
        Me.txtDN.EnterMoveNextControl = True
        Me.txtDN.Location = New System.Drawing.Point(946, 487)
        Me.txtDN.Name = "txtDN"
        Me.txtDN.Properties.Appearance.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtDN.Properties.Appearance.Options.UseFont = True
        Me.txtDN.Properties.Mask.EditMask = "n2"
        Me.txtDN.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric
        Me.txtDN.Properties.Mask.UseMaskAsDisplayFormat = True
        Me.txtDN.Properties.ReadOnly = True
        Me.txtDN.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.txtDN.Size = New System.Drawing.Size(50, 22)
        Me.txtDN.StyleController = Me.LayoutControl1
        Me.txtDN.TabIndex = 12
        '
        'txtJumlah
        '
        Me.txtJumlah.EditValue = New Decimal(New Integer() {0, 0, 0, 0})
        Me.txtJumlah.EnterMoveNextControl = True
        Me.txtJumlah.Location = New System.Drawing.Point(772, 539)
        Me.txtJumlah.Name = "txtJumlah"
        Me.txtJumlah.Properties.Appearance.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtJumlah.Properties.Appearance.Options.UseFont = True
        Me.txtJumlah.Properties.Mask.EditMask = "n2"
        Me.txtJumlah.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric
        Me.txtJumlah.Properties.Mask.UseMaskAsDisplayFormat = True
        Me.txtJumlah.Properties.ReadOnly = True
        Me.txtJumlah.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.txtJumlah.Size = New System.Drawing.Size(224, 22)
        Me.txtJumlah.StyleController = Me.LayoutControl1
        Me.txtJumlah.TabIndex = 12
        '
        'txtMaterai
        '
        Me.txtMaterai.EditValue = New Decimal(New Integer() {0, 0, 0, 0})
        Me.txtMaterai.EnterMoveNextControl = True
        Me.txtMaterai.Location = New System.Drawing.Point(772, 513)
        Me.txtMaterai.Name = "txtMaterai"
        Me.txtMaterai.Properties.Appearance.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtMaterai.Properties.Appearance.Options.UseFont = True
        Me.txtMaterai.Properties.Mask.EditMask = "n2"
        Me.txtMaterai.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric
        Me.txtMaterai.Properties.Mask.UseMaskAsDisplayFormat = True
        Me.txtMaterai.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.txtMaterai.Size = New System.Drawing.Size(224, 22)
        Me.txtMaterai.StyleController = Me.LayoutControl1
        Me.txtMaterai.TabIndex = 12
        '
        'txtPotongan
        '
        Me.txtPotongan.EditValue = New Decimal(New Integer() {0, 0, 0, 0})
        Me.txtPotongan.EnterMoveNextControl = True
        Me.txtPotongan.Location = New System.Drawing.Point(772, 487)
        Me.txtPotongan.Name = "txtPotongan"
        Me.txtPotongan.Properties.Appearance.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtPotongan.Properties.Appearance.Options.UseFont = True
        Me.txtPotongan.Properties.Mask.EditMask = "n2"
        Me.txtPotongan.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric
        Me.txtPotongan.Properties.Mask.UseMaskAsDisplayFormat = True
        Me.txtPotongan.Properties.ReadOnly = True
        Me.txtPotongan.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.txtPotongan.Size = New System.Drawing.Size(50, 22)
        Me.txtPotongan.StyleController = Me.LayoutControl1
        Me.txtPotongan.TabIndex = 11
        '
        'txtKwitansi
        '
        Me.txtKwitansi.EditValue = ""
        Me.txtKwitansi.EnterMoveNextControl = True
        Me.txtKwitansi.Location = New System.Drawing.Point(902, 466)
        Me.txtKwitansi.MenuManager = Me.BarManager1
        Me.txtKwitansi.Name = "txtKwitansi"
        Me.txtKwitansi.Properties.Appearance.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtKwitansi.Properties.Appearance.Options.UseFont = True
        Me.txtKwitansi.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.txtKwitansi.Properties.DisplayMember = "Kode"
        Me.txtKwitansi.Properties.NullText = ""
        Me.txtKwitansi.Properties.ValueMember = "NoID"
        Me.txtKwitansi.Properties.View = Me.gvKwitansiMasuk
        Me.txtKwitansi.Size = New System.Drawing.Size(280, 22)
        Me.txtKwitansi.StyleController = Me.LayoutControl1
        Me.txtKwitansi.TabIndex = 5
        '
        'gvKwitansiMasuk
        '
        Me.gvKwitansiMasuk.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus
        Me.gvKwitansiMasuk.Name = "gvKwitansiMasuk"
        Me.gvKwitansiMasuk.OptionsSelection.EnableAppearanceFocusedCell = False
        Me.gvKwitansiMasuk.OptionsView.ShowGroupPanel = False
        '
        'txtTglTransfer
        '
        Me.txtTglTransfer.EditValue = Nothing
        Me.txtTglTransfer.EnterMoveNextControl = True
        Me.txtTglTransfer.Location = New System.Drawing.Point(772, 617)
        Me.txtTglTransfer.Name = "txtTglTransfer"
        Me.txtTglTransfer.Properties.Appearance.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtTglTransfer.Properties.Appearance.Options.UseFont = True
        Me.txtTglTransfer.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.txtTglTransfer.Properties.Mask.EditMask = "dd/MM/yyyy"
        Me.txtTglTransfer.Properties.Mask.UseMaskAsDisplayFormat = True
        Me.txtTglTransfer.Properties.VistaTimeProperties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton})
        Me.txtTglTransfer.Size = New System.Drawing.Size(224, 22)
        Me.txtTglTransfer.StyleController = Me.LayoutControl1
        Me.txtTglTransfer.TabIndex = 5
        '
        'txtNoACC
        '
        Me.txtNoACC.EnterMoveNextControl = True
        Me.txtNoACC.Location = New System.Drawing.Point(772, 591)
        Me.txtNoACC.Name = "txtNoACC"
        Me.txtNoACC.Properties.Appearance.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtNoACC.Properties.Appearance.Options.UseFont = True
        Me.txtNoACC.Size = New System.Drawing.Size(224, 22)
        Me.txtNoACC.StyleController = Me.LayoutControl1
        Me.txtNoACC.TabIndex = 6
        '
        'txtJumlahKwitansi
        '
        Me.txtJumlahKwitansi.EditValue = New Decimal(New Integer() {0, 0, 0, 0})
        Me.txtJumlahKwitansi.EnterMoveNextControl = True
        Me.txtJumlahKwitansi.Location = New System.Drawing.Point(144, 559)
        Me.txtJumlahKwitansi.Name = "txtJumlahKwitansi"
        Me.txtJumlahKwitansi.Properties.Appearance.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtJumlahKwitansi.Properties.Appearance.Options.UseFont = True
        Me.txtJumlahKwitansi.Properties.Mask.EditMask = "n2"
        Me.txtJumlahKwitansi.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric
        Me.txtJumlahKwitansi.Properties.Mask.UseMaskAsDisplayFormat = True
        Me.txtJumlahKwitansi.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.txtJumlahKwitansi.Size = New System.Drawing.Size(131, 22)
        Me.txtJumlahKwitansi.StyleController = Me.LayoutControl1
        Me.txtJumlahKwitansi.TabIndex = 11
        '
        'txtBayar
        '
        Me.txtBayar.EditValue = New Decimal(New Integer() {0, 0, 0, 0})
        Me.txtBayar.EnterMoveNextControl = True
        Me.txtBayar.Location = New System.Drawing.Point(772, 565)
        Me.txtBayar.Name = "txtBayar"
        Me.txtBayar.Properties.Appearance.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtBayar.Properties.Appearance.Options.UseFont = True
        Me.txtBayar.Properties.Mask.EditMask = "n2"
        Me.txtBayar.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric
        Me.txtBayar.Properties.Mask.UseMaskAsDisplayFormat = True
        Me.txtBayar.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.txtBayar.Size = New System.Drawing.Size(224, 22)
        Me.txtBayar.StyleController = Me.LayoutControl1
        Me.txtBayar.TabIndex = 10
        '
        'gcGiro
        '
        Me.gcGiro.Location = New System.Drawing.Point(24, 572)
        Me.gcGiro.MainView = Me.gvGiro
        Me.gcGiro.MenuManager = Me.BarManager1
        Me.gcGiro.Name = "gcGiro"
        Me.gcGiro.Size = New System.Drawing.Size(564, 80)
        Me.gcGiro.TabIndex = 23
        Me.gcGiro.ViewCollection.AddRange(New DevExpress.XtraGrid.Views.Base.BaseView() {Me.gvGiro})
        '
        'gvGiro
        '
        Me.gvGiro.CustomizationFormBounds = New System.Drawing.Rectangle(567, 278, 216, 178)
        Me.gvGiro.GridControl = Me.gcGiro
        Me.gvGiro.Name = "gvGiro"
        Me.gvGiro.OptionsBehavior.Editable = False
        Me.gvGiro.OptionsSelection.MultiSelect = True
        Me.gvGiro.OptionsView.ColumnAutoWidth = False
        Me.gvGiro.OptionsView.ShowFooter = True
        Me.gvGiro.OptionsView.ShowGroupPanel = False
        '
        'gcKredit
        '
        Me.gcKredit.Location = New System.Drawing.Point(315, 493)
        Me.gcKredit.MainView = Me.gvKredit
        Me.gcKredit.MenuManager = Me.BarManager1
        Me.gcKredit.Name = "gcKredit"
        Me.gcKredit.Size = New System.Drawing.Size(100, 134)
        Me.gcKredit.TabIndex = 23
        Me.gcKredit.ViewCollection.AddRange(New DevExpress.XtraGrid.Views.Base.BaseView() {Me.gvKredit})
        '
        'gvKredit
        '
        Me.gvKredit.Columns.AddRange(New DevExpress.XtraGrid.Columns.GridColumn() {Me.gnkNoID, Me.gnkKode, Me.gnkTanggal, Me.gnkTotal, Me.gnkPotong, Me.gnkCatatan})
        Me.gvKredit.CustomizationFormBounds = New System.Drawing.Rectangle(567, 278, 216, 178)
        Me.gvKredit.GridControl = Me.gcKredit
        Me.gvKredit.Name = "gvKredit"
        Me.gvKredit.OptionsBehavior.Editable = False
        Me.gvKredit.OptionsSelection.MultiSelect = True
        Me.gvKredit.OptionsView.ColumnAutoWidth = False
        Me.gvKredit.OptionsView.ShowFooter = True
        Me.gvKredit.OptionsView.ShowGroupPanel = False
        '
        'gnkNoID
        '
        Me.gnkNoID.Caption = "NoID"
        Me.gnkNoID.FieldName = "NoID"
        Me.gnkNoID.Name = "gnkNoID"
        '
        'gnkKode
        '
        Me.gnkKode.Caption = "No. Nota"
        Me.gnkKode.FieldName = "Kode"
        Me.gnkKode.Name = "gnkKode"
        Me.gnkKode.SummaryItem.DisplayFormat = "{0:n0}"
        Me.gnkKode.SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Count
        Me.gnkKode.Width = 99
        '
        'gnkTanggal
        '
        Me.gnkTanggal.Caption = "Tanggal"
        Me.gnkTanggal.DisplayFormat.FormatString = "dd-MM-yyyy"
        Me.gnkTanggal.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime
        Me.gnkTanggal.FieldName = "Tanggal"
        Me.gnkTanggal.Name = "gnkTanggal"
        '
        'gnkTotal
        '
        Me.gnkTotal.Caption = "Total"
        Me.gnkTotal.DisplayFormat.FormatString = "n0"
        Me.gnkTotal.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
        Me.gnkTotal.FieldName = "Total"
        Me.gnkTotal.Name = "gnkTotal"
        Me.gnkTotal.SummaryItem.DisplayFormat = "{0:n0}"
        Me.gnkTotal.SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum
        Me.gnkTotal.Visible = True
        Me.gnkTotal.VisibleIndex = 1
        Me.gnkTotal.Width = 83
        '
        'gnkPotong
        '
        Me.gnkPotong.Caption = "Potong"
        Me.gnkPotong.DisplayFormat.FormatString = "n0"
        Me.gnkPotong.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
        Me.gnkPotong.FieldName = "Potong"
        Me.gnkPotong.Name = "gnkPotong"
        Me.gnkPotong.SummaryItem.DisplayFormat = "{0:n0}"
        Me.gnkPotong.SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum
        Me.gnkPotong.Width = 204
        '
        'gnkCatatan
        '
        Me.gnkCatatan.Caption = "Catatan"
        Me.gnkCatatan.FieldName = "Catatan"
        Me.gnkCatatan.Name = "gnkCatatan"
        Me.gnkCatatan.Visible = True
        Me.gnkCatatan.VisibleIndex = 0
        Me.gnkCatatan.Width = 121
        '
        'gcDebet
        '
        Me.gcDebet.Location = New System.Drawing.Point(475, 493)
        Me.gcDebet.MainView = Me.gvDebet
        Me.gcDebet.MenuManager = Me.BarManager1
        Me.gcDebet.Name = "gcDebet"
        Me.gcDebet.Size = New System.Drawing.Size(100, 134)
        Me.gcDebet.TabIndex = 23
        Me.gcDebet.ViewCollection.AddRange(New DevExpress.XtraGrid.Views.Base.BaseView() {Me.gvDebet})
        '
        'gvDebet
        '
        Me.gvDebet.Columns.AddRange(New DevExpress.XtraGrid.Columns.GridColumn() {Me.cdnNoID, Me.cdnKode, Me.cdnTanggal, Me.cdnKeterangan, Me.cdnTotal, Me.cdnPotong})
        Me.gvDebet.CustomizationFormBounds = New System.Drawing.Rectangle(567, 278, 216, 178)
        Me.gvDebet.GridControl = Me.gcDebet
        Me.gvDebet.Name = "gvDebet"
        Me.gvDebet.OptionsBehavior.Editable = False
        Me.gvDebet.OptionsSelection.MultiSelect = True
        Me.gvDebet.OptionsView.ColumnAutoWidth = False
        Me.gvDebet.OptionsView.ShowFooter = True
        Me.gvDebet.OptionsView.ShowGroupPanel = False
        '
        'cdnNoID
        '
        Me.cdnNoID.Caption = "NoID"
        Me.cdnNoID.FieldName = "NoID"
        Me.cdnNoID.Name = "cdnNoID"
        '
        'cdnKode
        '
        Me.cdnKode.Caption = "No. Nota"
        Me.cdnKode.FieldName = "Kode"
        Me.cdnKode.Name = "cdnKode"
        Me.cdnKode.SummaryItem.DisplayFormat = "{0:n0}"
        Me.cdnKode.SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Count
        Me.cdnKode.Width = 108
        '
        'cdnTanggal
        '
        Me.cdnTanggal.Caption = "Tanggal"
        Me.cdnTanggal.DisplayFormat.FormatString = "dd-MM-yyyy"
        Me.cdnTanggal.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime
        Me.cdnTanggal.FieldName = "Tanggal"
        Me.cdnTanggal.Name = "cdnTanggal"
        '
        'cdnKeterangan
        '
        Me.cdnKeterangan.Caption = "Catatan"
        Me.cdnKeterangan.FieldName = "Catatan"
        Me.cdnKeterangan.Name = "cdnKeterangan"
        Me.cdnKeterangan.Visible = True
        Me.cdnKeterangan.VisibleIndex = 0
        Me.cdnKeterangan.Width = 88
        '
        'cdnTotal
        '
        Me.cdnTotal.Caption = "Total"
        Me.cdnTotal.DisplayFormat.FormatString = "n0"
        Me.cdnTotal.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
        Me.cdnTotal.FieldName = "Total"
        Me.cdnTotal.Name = "cdnTotal"
        Me.cdnTotal.SummaryItem.DisplayFormat = "{0:n0}"
        Me.cdnTotal.SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum
        Me.cdnTotal.Visible = True
        Me.cdnTotal.VisibleIndex = 1
        Me.cdnTotal.Width = 63
        '
        'cdnPotong
        '
        Me.cdnPotong.Caption = "Potong"
        Me.cdnPotong.DisplayFormat.FormatString = "n0"
        Me.cdnPotong.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
        Me.cdnPotong.FieldName = "Potong"
        Me.cdnPotong.Name = "cdnPotong"
        Me.cdnPotong.SummaryItem.DisplayFormat = "{0:n0}"
        Me.cdnPotong.SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum
        '
        'gcPH
        '
        Me.gcPH.Location = New System.Drawing.Point(36, 331)
        Me.gcPH.MainView = Me.gvPH
        Me.gcPH.MenuManager = Me.BarManager1
        Me.gcPH.Name = "gcPH"
        Me.gcPH.Size = New System.Drawing.Size(538, 64)
        Me.gcPH.TabIndex = 23
        Me.gcPH.ViewCollection.AddRange(New DevExpress.XtraGrid.Views.Base.BaseView() {Me.gvPH})
        '
        'gvPH
        '
        Me.gvPH.Columns.AddRange(New DevExpress.XtraGrid.Columns.GridColumn() {Me.cPNoID, Me.CPIDPH, Me.CpNoPH, Me.CpTanggal, Me.CpJumlah, Me.CpPotong})
        Me.gvPH.CustomizationFormBounds = New System.Drawing.Rectangle(567, 278, 216, 178)
        Me.gvPH.GridControl = Me.gcPH
        Me.gvPH.Name = "gvPH"
        Me.gvPH.OptionsBehavior.Editable = False
        Me.gvPH.OptionsSelection.MultiSelect = True
        Me.gvPH.OptionsView.ColumnAutoWidth = False
        Me.gvPH.OptionsView.ShowFooter = True
        Me.gvPH.OptionsView.ShowGroupPanel = False
        '
        'cPNoID
        '
        Me.cPNoID.Caption = "NoID"
        Me.cPNoID.FieldName = "NoID"
        Me.cPNoID.Name = "cPNoID"
        '
        'CPIDPH
        '
        Me.CPIDPH.Caption = "IDPH"
        Me.CPIDPH.FieldName = "IDPH"
        Me.CPIDPH.Name = "CPIDPH"
        '
        'CpNoPH
        '
        Me.CpNoPH.Caption = "NoPH"
        Me.CpNoPH.FieldName = "Kode"
        Me.CpNoPH.Name = "CpNoPH"
        Me.CpNoPH.SummaryItem.DisplayFormat = "{0:n0}"
        Me.CpNoPH.SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Count
        Me.CpNoPH.Visible = True
        Me.CpNoPH.VisibleIndex = 0
        Me.CpNoPH.Width = 135
        '
        'CpTanggal
        '
        Me.CpTanggal.Caption = "Tanggal"
        Me.CpTanggal.DisplayFormat.FormatString = "dd-MM-yyyy"
        Me.CpTanggal.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime
        Me.CpTanggal.FieldName = "Tanggal"
        Me.CpTanggal.Name = "CpTanggal"
        Me.CpTanggal.Visible = True
        Me.CpTanggal.VisibleIndex = 1
        Me.CpTanggal.Width = 89
        '
        'CpJumlah
        '
        Me.CpJumlah.Caption = "Total"
        Me.CpJumlah.DisplayFormat.FormatString = "#,##0"
        Me.CpJumlah.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
        Me.CpJumlah.FieldName = "Total"
        Me.CpJumlah.Name = "CpJumlah"
        Me.CpJumlah.SummaryItem.DisplayFormat = "{0:#,###,###,###,##0}"
        Me.CpJumlah.SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum
        Me.CpJumlah.Visible = True
        Me.CpJumlah.VisibleIndex = 2
        '
        'CpPotong
        '
        Me.CpPotong.Caption = "Potong"
        Me.CpPotong.DisplayFormat.FormatString = "#,##0"
        Me.CpPotong.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
        Me.CpPotong.FieldName = "Potong"
        Me.CpPotong.Name = "CpPotong"
        Me.CpPotong.SummaryItem.DisplayFormat = "{0:#,###,###,###,##0}"
        Me.CpPotong.SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum
        Me.CpPotong.Visible = True
        Me.CpPotong.VisibleIndex = 3
        Me.CpPotong.Width = 128
        '
        'gcTU
        '
        Me.gcTU.Location = New System.Drawing.Point(619, 268)
        Me.gcTU.MainView = Me.gvTU
        Me.gcTU.MenuManager = Me.BarManager1
        Me.gcTU.Name = "gcTU"
        Me.gcTU.Size = New System.Drawing.Size(563, 139)
        Me.gcTU.TabIndex = 22
        Me.gcTU.ViewCollection.AddRange(New DevExpress.XtraGrid.Views.Base.BaseView() {Me.gvTU})
        '
        'gvTU
        '
        Me.gvTU.Columns.AddRange(New DevExpress.XtraGrid.Columns.GridColumn() {Me.cbNoID, Me.cbNomor, Me.cbTanggal, Me.cbJumlah, Me.cbPotong})
        Me.gvTU.CustomizationFormBounds = New System.Drawing.Rectangle(567, 278, 216, 178)
        Me.gvTU.GridControl = Me.gcTU
        Me.gvTU.Name = "gvTU"
        Me.gvTU.OptionsBehavior.Editable = False
        Me.gvTU.OptionsSelection.MultiSelect = True
        Me.gvTU.OptionsView.ColumnAutoWidth = False
        Me.gvTU.OptionsView.ShowFooter = True
        Me.gvTU.OptionsView.ShowGroupPanel = False
        '
        'cbNoID
        '
        Me.cbNoID.Caption = "NoID"
        Me.cbNoID.FieldName = "NoID"
        Me.cbNoID.Name = "cbNoID"
        '
        'cbNomor
        '
        Me.cbNomor.Caption = "Nomor"
        Me.cbNomor.FieldName = "Nomor"
        Me.cbNomor.Name = "cbNomor"
        Me.cbNomor.Visible = True
        Me.cbNomor.VisibleIndex = 0
        Me.cbNomor.Width = 125
        '
        'cbTanggal
        '
        Me.cbTanggal.Caption = "Tanggal"
        Me.cbTanggal.DisplayFormat.FormatString = "dd-MM-yyyy"
        Me.cbTanggal.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime
        Me.cbTanggal.FieldName = "Tanggal"
        Me.cbTanggal.Name = "cbTanggal"
        Me.cbTanggal.Visible = True
        Me.cbTanggal.VisibleIndex = 1
        '
        'cbJumlah
        '
        Me.cbJumlah.Caption = "Jumlah"
        Me.cbJumlah.DisplayFormat.FormatString = "#,##0"
        Me.cbJumlah.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
        Me.cbJumlah.FieldName = "Jumlah"
        Me.cbJumlah.Name = "cbJumlah"
        Me.cbJumlah.SummaryItem.DisplayFormat = "{0:#,###,###,###,##0}"
        Me.cbJumlah.SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum
        Me.cbJumlah.Visible = True
        Me.cbJumlah.VisibleIndex = 2
        Me.cbJumlah.Width = 102
        '
        'cbPotong
        '
        Me.cbPotong.Caption = "Potong"
        Me.cbPotong.DisplayFormat.FormatString = "#,##0"
        Me.cbPotong.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
        Me.cbPotong.FieldName = "Potong"
        Me.cbPotong.Name = "cbPotong"
        Me.cbPotong.SummaryItem.DisplayFormat = "{0:#,###,###,###,##0}"
        Me.cbPotong.SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum
        Me.cbPotong.Visible = True
        Me.cbPotong.VisibleIndex = 3
        Me.cbPotong.Width = 133
        '
        'gcPembelian
        '
        Me.gcPembelian.Location = New System.Drawing.Point(24, 96)
        Me.gcPembelian.MainView = Me.gvPembelian
        Me.gcPembelian.MenuManager = Me.BarManager1
        Me.gcPembelian.Name = "gcPembelian"
        Me.gcPembelian.Size = New System.Drawing.Size(469, 332)
        Me.gcPembelian.TabIndex = 21
        Me.gcPembelian.ViewCollection.AddRange(New DevExpress.XtraGrid.Views.Base.BaseView() {Me.gvPembelian})
        '
        'gvPembelian
        '
        Me.gvPembelian.Columns.AddRange(New DevExpress.XtraGrid.Columns.GridColumn() {Me.cNoID, Me.cIDBeli, Me.cNoNota, Me.cTanggal, Me.CTotal, Me.cBayar, Me.cKassa})
        Me.gvPembelian.CustomizationFormBounds = New System.Drawing.Rectangle(567, 278, 216, 178)
        Me.gvPembelian.GridControl = Me.gcPembelian
        Me.gvPembelian.Name = "gvPembelian"
        Me.gvPembelian.OptionsBehavior.Editable = False
        Me.gvPembelian.OptionsSelection.MultiSelect = True
        Me.gvPembelian.OptionsView.ColumnAutoWidth = False
        Me.gvPembelian.OptionsView.ShowFooter = True
        Me.gvPembelian.OptionsView.ShowGroupPanel = False
        '
        'cNoID
        '
        Me.cNoID.Caption = "NoID"
        Me.cNoID.FieldName = "NoID"
        Me.cNoID.Name = "cNoID"
        Me.cNoID.OptionsColumn.AllowEdit = False
        '
        'cIDBeli
        '
        Me.cIDBeli.Caption = "IDBeli"
        Me.cIDBeli.FieldName = "IDBeli"
        Me.cIDBeli.Name = "cIDBeli"
        Me.cIDBeli.OptionsColumn.AllowEdit = False
        '
        'cNoNota
        '
        Me.cNoNota.Caption = "No. Nota"
        Me.cNoNota.FieldName = "NoNota"
        Me.cNoNota.Name = "cNoNota"
        Me.cNoNota.OptionsColumn.AllowEdit = False
        Me.cNoNota.SummaryItem.DisplayFormat = "{0:n0}"
        Me.cNoNota.SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Count
        Me.cNoNota.Visible = True
        Me.cNoNota.VisibleIndex = 0
        Me.cNoNota.Width = 131
        '
        'cTanggal
        '
        Me.cTanggal.Caption = "Tanggal"
        Me.cTanggal.DisplayFormat.FormatString = "dd-MM-yyyy"
        Me.cTanggal.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime
        Me.cTanggal.FieldName = "Tanggal"
        Me.cTanggal.Name = "cTanggal"
        Me.cTanggal.OptionsColumn.AllowEdit = False
        Me.cTanggal.Visible = True
        Me.cTanggal.VisibleIndex = 1
        Me.cTanggal.Width = 98
        '
        'CTotal
        '
        Me.CTotal.Caption = "Total"
        Me.CTotal.DisplayFormat.FormatString = "n0"
        Me.CTotal.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
        Me.CTotal.FieldName = "Total"
        Me.CTotal.Name = "CTotal"
        Me.CTotal.SummaryItem.DisplayFormat = "{0:#,###,###,###,##0}"
        Me.CTotal.SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum
        Me.CTotal.Visible = True
        Me.CTotal.VisibleIndex = 2
        Me.CTotal.Width = 142
        '
        'cBayar
        '
        Me.cBayar.Caption = "Bayar"
        Me.cBayar.DisplayFormat.FormatString = "n0"
        Me.cBayar.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
        Me.cBayar.FieldName = "Bayar"
        Me.cBayar.Name = "cBayar"
        Me.cBayar.SummaryItem.DisplayFormat = "{0:#,###,###,###,##0}"
        Me.cBayar.SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum
        Me.cBayar.Visible = True
        Me.cBayar.VisibleIndex = 3
        Me.cBayar.Width = 138
        '
        'txtWilayah
        '
        Me.txtWilayah.EditValue = ""
        Me.txtWilayah.EnterMoveNextControl = True
        Me.txtWilayah.Location = New System.Drawing.Point(132, 64)
        Me.txtWilayah.MenuManager = Me.BarManager1
        Me.txtWilayah.Name = "txtWilayah"
        Me.txtWilayah.Properties.Appearance.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtWilayah.Properties.Appearance.Options.UseFont = True
        Me.txtWilayah.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.txtWilayah.Properties.DisplayMember = "Kode"
        Me.txtWilayah.Properties.NullText = ""
        Me.txtWilayah.Properties.ReadOnly = True
        Me.txtWilayah.Properties.ValueMember = "NoID"
        Me.txtWilayah.Properties.View = Me.gvWilayah
        Me.txtWilayah.Size = New System.Drawing.Size(287, 22)
        Me.txtWilayah.StyleController = Me.LayoutControl1
        Me.txtWilayah.TabIndex = 6
        '
        'gvWilayah
        '
        Me.gvWilayah.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus
        Me.gvWilayah.Name = "gvWilayah"
        Me.gvWilayah.OptionsSelection.EnableAppearanceFocusedCell = False
        Me.gvWilayah.OptionsView.ShowGroupPanel = False
        '
        'tglPosting
        '
        Me.tglPosting.EditValue = Nothing
        Me.tglPosting.EnterMoveNextControl = True
        Me.tglPosting.Location = New System.Drawing.Point(408, 590)
        Me.tglPosting.Name = "tglPosting"
        Me.tglPosting.Properties.Appearance.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tglPosting.Properties.Appearance.Options.UseFont = True
        Me.tglPosting.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.tglPosting.Properties.Mask.EditMask = "dd/MM/yyyy  HH:mm"
        Me.tglPosting.Properties.Mask.UseMaskAsDisplayFormat = True
        Me.tglPosting.Properties.ReadOnly = True
        Me.tglPosting.Properties.VistaTimeProperties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton})
        Me.tglPosting.Size = New System.Drawing.Size(259, 22)
        Me.tglPosting.StyleController = Me.LayoutControl1
        Me.tglPosting.TabIndex = 6
        '
        'tglEdit
        '
        Me.tglEdit.EditValue = Nothing
        Me.tglEdit.EnterMoveNextControl = True
        Me.tglEdit.Location = New System.Drawing.Point(408, 590)
        Me.tglEdit.Name = "tglEdit"
        Me.tglEdit.Properties.Appearance.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tglEdit.Properties.Appearance.Options.UseFont = True
        Me.tglEdit.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.tglEdit.Properties.Mask.EditMask = "dd/MM/yyyy  HH:mm"
        Me.tglEdit.Properties.Mask.UseMaskAsDisplayFormat = True
        Me.tglEdit.Properties.ReadOnly = True
        Me.tglEdit.Properties.VistaTimeProperties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton})
        Me.tglEdit.Size = New System.Drawing.Size(259, 22)
        Me.tglEdit.StyleController = Me.LayoutControl1
        Me.tglEdit.TabIndex = 6
        '
        'tglEntri
        '
        Me.tglEntri.EditValue = Nothing
        Me.tglEntri.EnterMoveNextControl = True
        Me.tglEntri.Location = New System.Drawing.Point(1109, 64)
        Me.tglEntri.Name = "tglEntri"
        Me.tglEntri.Properties.Appearance.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tglEntri.Properties.Appearance.Options.UseFont = True
        Me.tglEntri.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.tglEntri.Properties.Mask.EditMask = "dd/MM/yyyy  HH:mm"
        Me.tglEntri.Properties.Mask.UseMaskAsDisplayFormat = True
        Me.tglEntri.Properties.ReadOnly = True
        Me.tglEntri.Properties.VistaTimeProperties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton})
        Me.tglEntri.Size = New System.Drawing.Size(85, 22)
        Me.tglEntri.StyleController = Me.LayoutControl1
        Me.tglEntri.TabIndex = 5
        '
        'cmdLoad
        '
        Me.cmdLoad.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdLoad.Appearance.Font = New System.Drawing.Font("Tahoma", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdLoad.Appearance.ForeColor = System.Drawing.Color.Blue
        Me.cmdLoad.Appearance.Options.UseFont = True
        Me.cmdLoad.Appearance.Options.UseForeColor = True
        Me.cmdLoad.ImageIndex = 10
        Me.cmdLoad.Location = New System.Drawing.Point(252, 655)
        Me.cmdLoad.Name = "cmdLoad"
        Me.cmdLoad.Size = New System.Drawing.Size(122, 25)
        Me.cmdLoad.StyleController = Me.LayoutControl1
        Me.cmdLoad.TabIndex = 20
        Me.cmdLoad.Text = "&Load"
        '
        'txtDipostingOleh
        '
        Me.txtDipostingOleh.EnterMoveNextControl = True
        Me.txtDipostingOleh.Location = New System.Drawing.Point(90, 623)
        Me.txtDipostingOleh.Name = "txtDipostingOleh"
        Me.txtDipostingOleh.Properties.Appearance.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtDipostingOleh.Properties.Appearance.Options.UseFont = True
        Me.txtDipostingOleh.Properties.ReadOnly = True
        Me.txtDipostingOleh.Size = New System.Drawing.Size(577, 22)
        Me.txtDipostingOleh.StyleController = Me.LayoutControl1
        Me.txtDipostingOleh.TabIndex = 8
        '
        'txtDieditOleh
        '
        Me.txtDieditOleh.EnterMoveNextControl = True
        Me.txtDieditOleh.Location = New System.Drawing.Point(90, 616)
        Me.txtDieditOleh.Name = "txtDieditOleh"
        Me.txtDieditOleh.Properties.Appearance.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtDieditOleh.Properties.Appearance.Options.UseFont = True
        Me.txtDieditOleh.Properties.ReadOnly = True
        Me.txtDieditOleh.Size = New System.Drawing.Size(577, 22)
        Me.txtDieditOleh.StyleController = Me.LayoutControl1
        Me.txtDieditOleh.TabIndex = 7
        '
        'txtDientriOleh
        '
        Me.txtDientriOleh.EnterMoveNextControl = True
        Me.txtDientriOleh.Location = New System.Drawing.Point(90, 590)
        Me.txtDientriOleh.Name = "txtDientriOleh"
        Me.txtDientriOleh.Properties.Appearance.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtDientriOleh.Properties.Appearance.Options.UseFont = True
        Me.txtDientriOleh.Properties.ReadOnly = True
        Me.txtDientriOleh.Size = New System.Drawing.Size(577, 22)
        Me.txtDientriOleh.StyleController = Me.LayoutControl1
        Me.txtDientriOleh.TabIndex = 6
        '
        'cmdRefresh
        '
        Me.cmdRefresh.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdRefresh.Appearance.Font = New System.Drawing.Font("Tahoma", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdRefresh.Appearance.ForeColor = System.Drawing.Color.Blue
        Me.cmdRefresh.Appearance.Options.UseFont = True
        Me.cmdRefresh.Appearance.Options.UseForeColor = True
        Me.cmdRefresh.ImageIndex = 10
        Me.cmdRefresh.Location = New System.Drawing.Point(498, 655)
        Me.cmdRefresh.Name = "cmdRefresh"
        Me.cmdRefresh.Size = New System.Drawing.Size(116, 25)
        Me.cmdRefresh.StyleController = Me.LayoutControl1
        Me.cmdRefresh.TabIndex = 19
        Me.cmdRefresh.Text = "&Refresh"
        '
        'cmdDelete
        '
        Me.cmdDelete.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdDelete.Appearance.Font = New System.Drawing.Font("Tahoma", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdDelete.Appearance.ForeColor = System.Drawing.Color.Blue
        Me.cmdDelete.Appearance.Options.UseFont = True
        Me.cmdDelete.Appearance.Options.UseForeColor = True
        Me.cmdDelete.ImageIndex = 10
        Me.cmdDelete.Location = New System.Drawing.Point(378, 655)
        Me.cmdDelete.Name = "cmdDelete"
        Me.cmdDelete.Size = New System.Drawing.Size(116, 25)
        Me.cmdDelete.StyleController = Me.LayoutControl1
        Me.cmdDelete.TabIndex = 19
        Me.cmdDelete.Text = "&Hapus"
        '
        'cmdEdit
        '
        Me.cmdEdit.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdEdit.Appearance.Font = New System.Drawing.Font("Tahoma", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdEdit.Appearance.ForeColor = System.Drawing.Color.Blue
        Me.cmdEdit.Appearance.Options.UseFont = True
        Me.cmdEdit.Appearance.Options.UseForeColor = True
        Me.cmdEdit.ImageIndex = 10
        Me.cmdEdit.Location = New System.Drawing.Point(132, 655)
        Me.cmdEdit.Name = "cmdEdit"
        Me.cmdEdit.Size = New System.Drawing.Size(116, 25)
        Me.cmdEdit.StyleController = Me.LayoutControl1
        Me.cmdEdit.TabIndex = 19
        Me.cmdEdit.Text = "&Edit"
        Me.cmdEdit.Visible = False
        '
        'cmdBAru
        '
        Me.cmdBAru.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdBAru.Appearance.Font = New System.Drawing.Font("Tahoma", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdBAru.Appearance.ForeColor = System.Drawing.Color.Blue
        Me.cmdBAru.Appearance.Options.UseFont = True
        Me.cmdBAru.Appearance.Options.UseForeColor = True
        Me.cmdBAru.ImageIndex = 10
        Me.cmdBAru.Location = New System.Drawing.Point(12, 655)
        Me.cmdBAru.Name = "cmdBAru"
        Me.cmdBAru.Size = New System.Drawing.Size(116, 25)
        Me.cmdBAru.StyleController = Me.LayoutControl1
        Me.cmdBAru.TabIndex = 19
        Me.cmdBAru.Text = "&Baru"
        Me.cmdBAru.Visible = False
        '
        'txtKodeAlamat
        '
        Me.txtKodeAlamat.EditValue = ""
        Me.txtKodeAlamat.EnterMoveNextControl = True
        Me.txtKodeAlamat.Location = New System.Drawing.Point(796, 12)
        Me.txtKodeAlamat.MenuManager = Me.BarManager1
        Me.txtKodeAlamat.Name = "txtKodeAlamat"
        Me.txtKodeAlamat.Properties.Appearance.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtKodeAlamat.Properties.Appearance.Options.UseFont = True
        Me.txtKodeAlamat.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.txtKodeAlamat.Properties.DisplayMember = "Kode"
        Me.txtKodeAlamat.Properties.NullText = ""
        Me.txtKodeAlamat.Properties.ValueMember = "NoID"
        Me.txtKodeAlamat.Properties.View = Me.SearchLookUpEdit1View
        Me.txtKodeAlamat.Size = New System.Drawing.Size(212, 22)
        Me.txtKodeAlamat.StyleController = Me.LayoutControl1
        Me.txtKodeAlamat.TabIndex = 4
        '
        'SearchLookUpEdit1View
        '
        Me.SearchLookUpEdit1View.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus
        Me.SearchLookUpEdit1View.Name = "SearchLookUpEdit1View"
        Me.SearchLookUpEdit1View.OptionsSelection.EnableAppearanceFocusedCell = False
        Me.SearchLookUpEdit1View.OptionsView.ShowGroupPanel = False
        '
        'txtKode
        '
        Me.txtKode.EnterMoveNextControl = True
        Me.txtKode.Location = New System.Drawing.Point(132, 12)
        Me.txtKode.Name = "txtKode"
        Me.txtKode.Properties.Appearance.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtKode.Properties.Appearance.Options.UseFont = True
        Me.txtKode.Properties.ReadOnly = True
        Me.txtKode.Size = New System.Drawing.Size(210, 22)
        Me.txtKode.StyleController = Me.LayoutControl1
        Me.txtKode.TabIndex = 0
        '
        'Tgl
        '
        Me.Tgl.EditValue = Nothing
        Me.Tgl.EnterMoveNextControl = True
        Me.Tgl.Location = New System.Drawing.Point(132, 38)
        Me.Tgl.Name = "Tgl"
        Me.Tgl.Properties.Appearance.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Tgl.Properties.Appearance.Options.UseFont = True
        Me.Tgl.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.Tgl.Properties.Mask.EditMask = "dd/MM/yyyy"
        Me.Tgl.Properties.Mask.UseMaskAsDisplayFormat = True
        Me.Tgl.Properties.ReadOnly = True
        Me.Tgl.Properties.VistaTimeProperties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton})
        Me.Tgl.Size = New System.Drawing.Size(210, 22)
        Me.Tgl.StyleController = Me.LayoutControl1
        Me.Tgl.TabIndex = 1
        '
        'txtBarang
        '
        Me.txtBarang.Location = New System.Drawing.Point(76, 597)
        Me.txtBarang.Name = "txtBarang"
        Me.txtBarang.Properties.Appearance.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtBarang.Properties.Appearance.Options.UseFont = True
        Me.txtBarang.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Ellipsis, "", -1, True, True, False, DevExpress.XtraEditors.ImageLocation.MiddleCenter, Nothing, New DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.PageDown), SerializableAppearanceObject1, "", Nothing, Nothing, True), New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Close, "", -1, True, True, False, DevExpress.XtraEditors.ImageLocation.MiddleCenter, Nothing, New DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.Delete), SerializableAppearanceObject2, "", Nothing, Nothing, True)})
        Me.txtBarang.Properties.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtBarang.Size = New System.Drawing.Size(591, 22)
        Me.txtBarang.StyleController = Me.LayoutControl1
        Me.txtBarang.TabIndex = 15
        '
        'gcRetur
        '
        Me.gcRetur.Location = New System.Drawing.Point(526, 96)
        Me.gcRetur.MainView = Me.gvRetur
        Me.gcRetur.MenuManager = Me.BarManager1
        Me.gcRetur.Name = "gcRetur"
        Me.gcRetur.Size = New System.Drawing.Size(470, 332)
        Me.gcRetur.TabIndex = 8
        Me.gcRetur.ViewCollection.AddRange(New DevExpress.XtraGrid.Views.Base.BaseView() {Me.gvRetur})
        '
        'gvRetur
        '
        Me.gvRetur.Columns.AddRange(New DevExpress.XtraGrid.Columns.GridColumn() {Me.cRNoID, Me.CrIDRetur, Me.crNoRetur, Me.CrTanggal, Me.CrTotal, Me.CrPotong})
        Me.gvRetur.CustomizationFormBounds = New System.Drawing.Rectangle(567, 278, 216, 178)
        Me.gvRetur.GridControl = Me.gcRetur
        Me.gvRetur.Name = "gvRetur"
        Me.gvRetur.OptionsBehavior.Editable = False
        Me.gvRetur.OptionsSelection.MultiSelect = True
        Me.gvRetur.OptionsView.ColumnAutoWidth = False
        Me.gvRetur.OptionsView.ShowFooter = True
        Me.gvRetur.OptionsView.ShowGroupPanel = False
        '
        'cRNoID
        '
        Me.cRNoID.Caption = "NoID"
        Me.cRNoID.FieldName = "NoID"
        Me.cRNoID.Name = "cRNoID"
        Me.cRNoID.OptionsColumn.AllowEdit = False
        '
        'CrIDRetur
        '
        Me.CrIDRetur.Caption = "IDRetur"
        Me.CrIDRetur.FieldName = "IDRetur"
        Me.CrIDRetur.Name = "CrIDRetur"
        Me.CrIDRetur.OptionsColumn.AllowEdit = False
        '
        'crNoRetur
        '
        Me.crNoRetur.Caption = "No. Retur"
        Me.crNoRetur.FieldName = "Kode"
        Me.crNoRetur.Name = "crNoRetur"
        Me.crNoRetur.OptionsColumn.AllowEdit = False
        Me.crNoRetur.SummaryItem.DisplayFormat = "{0:n0}"
        Me.crNoRetur.SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Count
        Me.crNoRetur.Visible = True
        Me.crNoRetur.VisibleIndex = 0
        '
        'CrTanggal
        '
        Me.CrTanggal.Caption = "Tanggal"
        Me.CrTanggal.DisplayFormat.FormatString = "dd-MM-yyyy"
        Me.CrTanggal.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime
        Me.CrTanggal.FieldName = "Tanggal"
        Me.CrTanggal.Name = "CrTanggal"
        Me.CrTanggal.OptionsColumn.AllowEdit = False
        Me.CrTanggal.Visible = True
        Me.CrTanggal.VisibleIndex = 1
        Me.CrTanggal.Width = 88
        '
        'CrTotal
        '
        Me.CrTotal.Caption = "Total"
        Me.CrTotal.DisplayFormat.FormatString = "#,##0"
        Me.CrTotal.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
        Me.CrTotal.FieldName = "Total"
        Me.CrTotal.Name = "CrTotal"
        Me.CrTotal.SummaryItem.DisplayFormat = "{0:#,###,###,###,###,##0}"
        Me.CrTotal.SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum
        Me.CrTotal.Visible = True
        Me.CrTotal.VisibleIndex = 2
        Me.CrTotal.Width = 131
        '
        'CrPotong
        '
        Me.CrPotong.Caption = "Potong"
        Me.CrPotong.DisplayFormat.FormatString = "#,##0"
        Me.CrPotong.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
        Me.CrPotong.FieldName = "Potong"
        Me.CrPotong.Name = "CrPotong"
        Me.CrPotong.SummaryItem.DisplayFormat = "{0:#,###,###,###,###,##0}"
        Me.CrPotong.SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum
        Me.CrPotong.Visible = True
        Me.CrPotong.VisibleIndex = 3
        '
        'cmdSave
        '
        Me.cmdSave.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdSave.Appearance.Font = New System.Drawing.Font("Tahoma", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdSave.Appearance.ForeColor = System.Drawing.Color.Blue
        Me.cmdSave.Appearance.Options.UseFont = True
        Me.cmdSave.Appearance.Options.UseForeColor = True
        Me.cmdSave.ImageIndex = 10
        Me.cmdSave.Location = New System.Drawing.Point(772, 655)
        Me.cmdSave.Name = "cmdSave"
        Me.cmdSave.Size = New System.Drawing.Size(116, 25)
        Me.cmdSave.StyleController = Me.LayoutControl1
        Me.cmdSave.TabIndex = 18
        Me.cmdSave.Text = "&Simpan"
        Me.cmdSave.Visible = False
        '
        'cmdTutup
        '
        Me.cmdTutup.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdTutup.Appearance.Font = New System.Drawing.Font("Tahoma", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdTutup.Appearance.ForeColor = System.Drawing.Color.Blue
        Me.cmdTutup.Appearance.Options.UseFont = True
        Me.cmdTutup.Appearance.Options.UseForeColor = True
        Me.cmdTutup.ImageIndex = 11
        Me.cmdTutup.Location = New System.Drawing.Point(892, 655)
        Me.cmdTutup.Name = "cmdTutup"
        Me.cmdTutup.Size = New System.Drawing.Size(116, 25)
        Me.cmdTutup.StyleController = Me.LayoutControl1
        Me.cmdTutup.TabIndex = 19
        Me.cmdTutup.Text = "&Tutup"
        '
        'txtSubTotal
        '
        Me.txtSubTotal.EditValue = New Decimal(New Integer() {0, 0, 0, 0})
        Me.txtSubTotal.EnterMoveNextControl = True
        Me.txtSubTotal.Location = New System.Drawing.Point(772, 461)
        Me.txtSubTotal.Name = "txtSubTotal"
        Me.txtSubTotal.Properties.Appearance.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtSubTotal.Properties.Appearance.Options.UseFont = True
        Me.txtSubTotal.Properties.Mask.EditMask = "n2"
        Me.txtSubTotal.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric
        Me.txtSubTotal.Properties.Mask.UseMaskAsDisplayFormat = True
        Me.txtSubTotal.Properties.ReadOnly = True
        Me.txtSubTotal.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.txtSubTotal.Size = New System.Drawing.Size(224, 22)
        Me.txtSubTotal.StyleController = Me.LayoutControl1
        Me.txtSubTotal.TabIndex = 9
        '
        'txtNamaAlamat
        '
        Me.txtNamaAlamat.Location = New System.Drawing.Point(796, 38)
        Me.txtNamaAlamat.Name = "txtNamaAlamat"
        Me.txtNamaAlamat.Properties.Appearance.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtNamaAlamat.Properties.Appearance.Options.UseFont = True
        Me.txtNamaAlamat.Properties.ReadOnly = True
        Me.txtNamaAlamat.Size = New System.Drawing.Size(212, 22)
        Me.txtNamaAlamat.StyleController = Me.LayoutControl1
        Me.txtNamaAlamat.TabIndex = 5
        '
        'txtCatatan
        '
        Me.txtCatatan.Location = New System.Drawing.Point(797, 634)
        Me.txtCatatan.Name = "txtCatatan"
        Me.txtCatatan.Properties.Appearance.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtCatatan.Properties.Appearance.Options.UseFont = True
        Me.txtCatatan.Size = New System.Drawing.Size(385, 18)
        Me.txtCatatan.StyleController = Me.LayoutControl1
        Me.txtCatatan.TabIndex = 7
        '
        'txtAlamat
        '
        Me.txtAlamat.Location = New System.Drawing.Point(901, 64)
        Me.txtAlamat.Name = "txtAlamat"
        Me.txtAlamat.Properties.Appearance.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtAlamat.Properties.Appearance.Options.UseFont = True
        Me.txtAlamat.Properties.ReadOnly = True
        Me.txtAlamat.Size = New System.Drawing.Size(293, 22)
        Me.txtAlamat.StyleController = Me.LayoutControl1
        Me.txtAlamat.TabIndex = 6
        '
        'LayoutControlItem31
        '
        Me.LayoutControlItem31.Control = Me.tglEdit
        Me.LayoutControlItem31.CustomizationFormText = "Tgl Edit"
        Me.LayoutControlItem31.Location = New System.Drawing.Point(318, 578)
        Me.LayoutControlItem31.Name = "LayoutControlItem31"
        Me.LayoutControlItem31.Size = New System.Drawing.Size(341, 52)
        Me.LayoutControlItem31.Text = "Tgl Edit"
        Me.LayoutControlItem31.TextSize = New System.Drawing.Size(83, 13)
        Me.LayoutControlItem31.TextToControlDistance = 5
        '
        'LayoutControlItem32
        '
        Me.LayoutControlItem32.Control = Me.tglPosting
        Me.LayoutControlItem32.CustomizationFormText = "Tgl Posting"
        Me.LayoutControlItem32.Location = New System.Drawing.Point(318, 578)
        Me.LayoutControlItem32.Name = "LayoutControlItem32"
        Me.LayoutControlItem32.Size = New System.Drawing.Size(341, 78)
        Me.LayoutControlItem32.Text = "Tgl Posting"
        Me.LayoutControlItem32.TextSize = New System.Drawing.Size(83, 13)
        Me.LayoutControlItem32.TextToControlDistance = 5
        '
        'LayoutControlItem27
        '
        Me.LayoutControlItem27.Control = Me.txtDientriOleh
        Me.LayoutControlItem27.CustomizationFormText = "Dientri Oleh"
        Me.LayoutControlItem27.Location = New System.Drawing.Point(0, 578)
        Me.LayoutControlItem27.Name = "LayoutControlItem27"
        Me.LayoutControlItem27.Size = New System.Drawing.Size(659, 26)
        Me.LayoutControlItem27.Text = "Dientri Oleh"
        Me.LayoutControlItem27.TextSize = New System.Drawing.Size(83, 13)
        Me.LayoutControlItem27.TextToControlDistance = 5
        '
        'LayoutControlItem28
        '
        Me.LayoutControlItem28.Control = Me.txtDieditOleh
        Me.LayoutControlItem28.CustomizationFormText = "Diedit Oleh"
        Me.LayoutControlItem28.Location = New System.Drawing.Point(0, 604)
        Me.LayoutControlItem28.Name = "LayoutControlItem28"
        Me.LayoutControlItem28.Size = New System.Drawing.Size(659, 26)
        Me.LayoutControlItem28.Text = "Diedit Oleh"
        Me.LayoutControlItem28.TextSize = New System.Drawing.Size(83, 13)
        Me.LayoutControlItem28.TextToControlDistance = 5
        '
        'LayoutControlItem29
        '
        Me.LayoutControlItem29.Control = Me.txtDipostingOleh
        Me.LayoutControlItem29.CustomizationFormText = "Disposting Oleh"
        Me.LayoutControlItem29.Location = New System.Drawing.Point(0, 611)
        Me.LayoutControlItem29.Name = "LayoutControlItem29"
        Me.LayoutControlItem29.Size = New System.Drawing.Size(659, 45)
        Me.LayoutControlItem29.Text = "Disposting Oleh"
        Me.LayoutControlItem29.TextSize = New System.Drawing.Size(83, 13)
        Me.LayoutControlItem29.TextToControlDistance = 5
        '
        'LayoutControlItem13
        '
        Me.LayoutControlItem13.Control = Me.txtBarang
        Me.LayoutControlItem13.CustomizationFormText = "Fast Entri"
        Me.LayoutControlItem13.Location = New System.Drawing.Point(0, 585)
        Me.LayoutControlItem13.Name = "LayoutControlItem13"
        Me.LayoutControlItem13.Size = New System.Drawing.Size(659, 71)
        Me.LayoutControlItem13.Text = "Fast Entri"
        Me.LayoutControlItem13.TextSize = New System.Drawing.Size(83, 13)
        Me.LayoutControlItem13.TextToControlDistance = 5
        '
        'LayoutControlItem30
        '
        Me.LayoutControlItem30.Control = Me.tglEntri
        Me.LayoutControlItem30.CustomizationFormText = "Tgl Entri"
        Me.LayoutControlItem30.Location = New System.Drawing.Point(977, 52)
        Me.LayoutControlItem30.Name = "LayoutControlItem30"
        Me.LayoutControlItem30.Size = New System.Drawing.Size(209, 26)
        Me.LayoutControlItem30.Text = "Tgl Entri"
        Me.LayoutControlItem30.TextSize = New System.Drawing.Size(83, 13)
        Me.LayoutControlItem30.TextToControlDistance = 5
        '
        'LayoutControlItem33
        '
        Me.LayoutControlItem33.Control = Me.txtAlamat
        Me.LayoutControlItem33.CustomizationFormText = "Alamat Customer"
        Me.LayoutControlItem33.Location = New System.Drawing.Point(769, 52)
        Me.LayoutControlItem33.Name = "LayoutControlItem33"
        Me.LayoutControlItem33.Size = New System.Drawing.Size(417, 26)
        Me.LayoutControlItem33.Text = "Alamat"
        Me.LayoutControlItem33.TextSize = New System.Drawing.Size(83, 13)
        Me.LayoutControlItem33.TextToControlDistance = 5
        '
        'LayoutControlItem36
        '
        Me.LayoutControlItem36.Control = Me.txtWilayah
        Me.LayoutControlItem36.CustomizationFormText = "Wilayah"
        Me.LayoutControlItem36.Location = New System.Drawing.Point(0, 52)
        Me.LayoutControlItem36.Name = "LayoutControlItem36"
        Me.LayoutControlItem36.Size = New System.Drawing.Size(411, 26)
        Me.LayoutControlItem36.Text = "Wilayah"
        Me.LayoutControlItem36.TextSize = New System.Drawing.Size(83, 13)
        Me.LayoutControlItem36.TextToControlDistance = 5
        '
        'LayoutControlItem34
        '
        Me.LayoutControlItem34.Control = Me.txtCatatan
        Me.LayoutControlItem34.CustomizationFormText = "Catatan"
        Me.LayoutControlItem34.Location = New System.Drawing.Point(0, 103)
        Me.LayoutControlItem34.Name = "LayoutControlItem34"
        Me.LayoutControlItem34.Size = New System.Drawing.Size(509, 22)
        Me.LayoutControlItem34.Text = "Catatan"
        Me.LayoutControlItem34.TextSize = New System.Drawing.Size(83, 13)
        Me.LayoutControlItem34.TextToControlDistance = 5
        '
        'LayoutControlGroup9
        '
        Me.LayoutControlGroup9.CustomizationFormText = "Pembayaran Dengan Giro"
        Me.LayoutControlGroup9.Items.AddRange(New DevExpress.XtraLayout.BaseLayoutItem() {Me.LayoutControlItem11})
        Me.LayoutControlGroup9.Location = New System.Drawing.Point(0, 528)
        Me.LayoutControlGroup9.Name = "LayoutControlGroup9"
        Me.LayoutControlGroup9.Size = New System.Drawing.Size(592, 128)
        Me.LayoutControlGroup9.Text = "Pembayaran Dengan Giro"
        '
        'LayoutControlItem11
        '
        Me.LayoutControlItem11.Control = Me.gcGiro
        Me.LayoutControlItem11.CustomizationFormText = "Giro"
        Me.LayoutControlItem11.Location = New System.Drawing.Point(0, 0)
        Me.LayoutControlItem11.Name = "LayoutControlItem11"
        Me.LayoutControlItem11.Size = New System.Drawing.Size(568, 84)
        Me.LayoutControlItem11.Text = "Giro"
        Me.LayoutControlItem11.TextSize = New System.Drawing.Size(0, 0)
        Me.LayoutControlItem11.TextToControlDistance = 0
        Me.LayoutControlItem11.TextVisible = False
        '
        'LayoutControlGroup6
        '
        Me.LayoutControlGroup6.CustomizationFormText = "PH Harga"
        Me.LayoutControlGroup6.Items.AddRange(New DevExpress.XtraLayout.BaseLayoutItem() {Me.LayoutControlItem8})
        Me.LayoutControlGroup6.Location = New System.Drawing.Point(0, 177)
        Me.LayoutControlGroup6.Name = "LayoutControlGroup6"
        Me.LayoutControlGroup6.Size = New System.Drawing.Size(566, 112)
        Me.LayoutControlGroup6.Text = "PH Harga"
        '
        'LayoutControlItem8
        '
        Me.LayoutControlItem8.Control = Me.gcPH
        Me.LayoutControlItem8.CustomizationFormText = "LayoutControlItem8"
        Me.LayoutControlItem8.Location = New System.Drawing.Point(0, 0)
        Me.LayoutControlItem8.Name = "LayoutControlItem8"
        Me.LayoutControlItem8.Size = New System.Drawing.Size(542, 68)
        Me.LayoutControlItem8.Text = "LayoutControlItem8"
        Me.LayoutControlItem8.TextSize = New System.Drawing.Size(0, 0)
        Me.LayoutControlItem8.TextToControlDistance = 0
        Me.LayoutControlItem8.TextVisible = False
        '
        'LayoutControlGroup5
        '
        Me.LayoutControlGroup5.CustomizationFormText = "Pemakaian Terima Uang"
        Me.LayoutControlGroup5.Items.AddRange(New DevExpress.XtraLayout.BaseLayoutItem() {Me.LayoutControlItem7})
        Me.LayoutControlGroup5.Location = New System.Drawing.Point(595, 224)
        Me.LayoutControlGroup5.Name = "LayoutControlGroup5"
        Me.LayoutControlGroup5.Size = New System.Drawing.Size(591, 187)
        Me.LayoutControlGroup5.Text = "Pemakaian Terima Uang"
        '
        'LayoutControlItem7
        '
        Me.LayoutControlItem7.Control = Me.gcTU
        Me.LayoutControlItem7.CustomizationFormText = "LayoutControlItem7"
        Me.LayoutControlItem7.Location = New System.Drawing.Point(0, 0)
        Me.LayoutControlItem7.Name = "LayoutControlItem7"
        Me.LayoutControlItem7.Size = New System.Drawing.Size(567, 143)
        Me.LayoutControlItem7.Text = "LayoutControlItem7"
        Me.LayoutControlItem7.TextSize = New System.Drawing.Size(0, 0)
        Me.LayoutControlItem7.TextToControlDistance = 0
        Me.LayoutControlItem7.TextVisible = False
        '
        'LayoutControlItem22
        '
        Me.LayoutControlItem22.Control = Me.txtKwitansi
        Me.LayoutControlItem22.CustomizationFormText = "Kwitansi"
        Me.LayoutControlItem22.Location = New System.Drawing.Point(0, 26)
        Me.LayoutControlItem22.Name = "LayoutControlItem22"
        Me.LayoutControlItem22.Size = New System.Drawing.Size(404, 26)
        Me.LayoutControlItem22.Text = "Kwitansi"
        Me.LayoutControlItem22.TextSize = New System.Drawing.Size(116, 13)
        Me.LayoutControlItem22.TextToControlDistance = 5
        '
        'LayoutControlGroup1
        '
        Me.LayoutControlGroup1.CustomizationFormText = "Root"
        Me.LayoutControlGroup1.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.[True]
        Me.LayoutControlGroup1.GroupBordersVisible = False
        Me.LayoutControlGroup1.Items.AddRange(New DevExpress.XtraLayout.BaseLayoutItem() {Me.LayoutControlItem1, Me.EmptySpaceItem2, Me.LayoutControlItem2, Me.LayoutControlItem15, Me.LayoutControlItem14, Me.LayoutControlItem18, Me.LayoutControlItem19, Me.LayoutControlItem23, Me.LayoutControlItem24, Me.LayoutControlItem25, Me.LayoutControlItem26, Me.LayoutControlGroup2, Me.LayoutControlItem5, Me.LayoutControlGroup3, Me.LayoutControlGroup4, Me.SplitterItem4, Me.SplitterItem6, Me.EmptySpaceItem1, Me.LayoutControlGroup7, Me.LayoutControlGroup8, Me.EmptySpaceItem6})
        Me.LayoutControlGroup1.Location = New System.Drawing.Point(0, 0)
        Me.LayoutControlGroup1.Name = "Root"
        Me.LayoutControlGroup1.Size = New System.Drawing.Size(1020, 692)
        Me.LayoutControlGroup1.Text = "Root"
        Me.LayoutControlGroup1.TextVisible = False
        '
        'LayoutControlItem1
        '
        Me.LayoutControlItem1.Control = Me.cmdTutup
        Me.LayoutControlItem1.CustomizationFormText = "LayoutControlItem1"
        Me.LayoutControlItem1.Location = New System.Drawing.Point(880, 643)
        Me.LayoutControlItem1.MaxSize = New System.Drawing.Size(120, 29)
        Me.LayoutControlItem1.MinSize = New System.Drawing.Size(120, 29)
        Me.LayoutControlItem1.Name = "LayoutControlItem1"
        Me.LayoutControlItem1.Size = New System.Drawing.Size(120, 29)
        Me.LayoutControlItem1.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom
        Me.LayoutControlItem1.Text = "LayoutControlItem1"
        Me.LayoutControlItem1.TextSize = New System.Drawing.Size(0, 0)
        Me.LayoutControlItem1.TextToControlDistance = 0
        Me.LayoutControlItem1.TextVisible = False
        '
        'EmptySpaceItem2
        '
        Me.EmptySpaceItem2.CustomizationFormText = "EmptySpaceItem2"
        Me.EmptySpaceItem2.Location = New System.Drawing.Point(606, 643)
        Me.EmptySpaceItem2.Name = "EmptySpaceItem2"
        Me.EmptySpaceItem2.Size = New System.Drawing.Size(154, 29)
        Me.EmptySpaceItem2.Text = "EmptySpaceItem2"
        Me.EmptySpaceItem2.TextSize = New System.Drawing.Size(0, 0)
        '
        'LayoutControlItem2
        '
        Me.LayoutControlItem2.Control = Me.cmdSave
        Me.LayoutControlItem2.CustomizationFormText = "LayoutControlItem2"
        Me.LayoutControlItem2.Location = New System.Drawing.Point(760, 643)
        Me.LayoutControlItem2.MaxSize = New System.Drawing.Size(120, 29)
        Me.LayoutControlItem2.MinSize = New System.Drawing.Size(120, 29)
        Me.LayoutControlItem2.Name = "LayoutControlItem2"
        Me.LayoutControlItem2.Size = New System.Drawing.Size(120, 29)
        Me.LayoutControlItem2.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom
        Me.LayoutControlItem2.Text = "LayoutControlItem2"
        Me.LayoutControlItem2.TextSize = New System.Drawing.Size(0, 0)
        Me.LayoutControlItem2.TextToControlDistance = 0
        Me.LayoutControlItem2.TextVisible = False
        '
        'LayoutControlItem15
        '
        Me.LayoutControlItem15.Control = Me.txtKode
        Me.LayoutControlItem15.CustomizationFormText = "No. Bukti"
        Me.LayoutControlItem15.Location = New System.Drawing.Point(0, 0)
        Me.LayoutControlItem15.Name = "LayoutControlItem15"
        Me.LayoutControlItem15.Size = New System.Drawing.Size(334, 26)
        Me.LayoutControlItem15.Text = "No. Bukti"
        Me.LayoutControlItem15.TextSize = New System.Drawing.Size(116, 13)
        '
        'LayoutControlItem14
        '
        Me.LayoutControlItem14.Control = Me.Tgl
        Me.LayoutControlItem14.CustomizationFormText = "Tgl."
        Me.LayoutControlItem14.Location = New System.Drawing.Point(0, 26)
        Me.LayoutControlItem14.Name = "LayoutControlItem14"
        Me.LayoutControlItem14.Size = New System.Drawing.Size(334, 26)
        Me.LayoutControlItem14.Text = "Tanggal"
        Me.LayoutControlItem14.TextSize = New System.Drawing.Size(116, 13)
        '
        'LayoutControlItem18
        '
        Me.LayoutControlItem18.Control = Me.txtNamaAlamat
        Me.LayoutControlItem18.CustomizationFormText = "Nama Supplier"
        Me.LayoutControlItem18.Location = New System.Drawing.Point(664, 26)
        Me.LayoutControlItem18.Name = "LayoutControlItem18"
        Me.LayoutControlItem18.Size = New System.Drawing.Size(336, 26)
        Me.LayoutControlItem18.Text = "Nama"
        Me.LayoutControlItem18.TextSize = New System.Drawing.Size(116, 13)
        '
        'LayoutControlItem19
        '
        Me.LayoutControlItem19.Control = Me.txtKodeAlamat
        Me.LayoutControlItem19.CustomizationFormText = "Kode Supplier"
        Me.LayoutControlItem19.Location = New System.Drawing.Point(664, 0)
        Me.LayoutControlItem19.Name = "LayoutControlItem19"
        Me.LayoutControlItem19.Size = New System.Drawing.Size(336, 26)
        Me.LayoutControlItem19.Text = "Kode Kontak"
        Me.LayoutControlItem19.TextSize = New System.Drawing.Size(116, 13)
        '
        'LayoutControlItem23
        '
        Me.LayoutControlItem23.Control = Me.cmdBAru
        Me.LayoutControlItem23.CustomizationFormText = "LayoutControlItem23"
        Me.LayoutControlItem23.Location = New System.Drawing.Point(0, 643)
        Me.LayoutControlItem23.MaxSize = New System.Drawing.Size(120, 29)
        Me.LayoutControlItem23.MinSize = New System.Drawing.Size(120, 29)
        Me.LayoutControlItem23.Name = "LayoutControlItem23"
        Me.LayoutControlItem23.Size = New System.Drawing.Size(120, 29)
        Me.LayoutControlItem23.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom
        Me.LayoutControlItem23.Text = "LayoutControlItem23"
        Me.LayoutControlItem23.TextSize = New System.Drawing.Size(0, 0)
        Me.LayoutControlItem23.TextToControlDistance = 0
        Me.LayoutControlItem23.TextVisible = False
        '
        'LayoutControlItem24
        '
        Me.LayoutControlItem24.Control = Me.cmdEdit
        Me.LayoutControlItem24.CustomizationFormText = "LayoutControlItem24"
        Me.LayoutControlItem24.Location = New System.Drawing.Point(120, 643)
        Me.LayoutControlItem24.MaxSize = New System.Drawing.Size(120, 29)
        Me.LayoutControlItem24.MinSize = New System.Drawing.Size(120, 29)
        Me.LayoutControlItem24.Name = "LayoutControlItem24"
        Me.LayoutControlItem24.Size = New System.Drawing.Size(120, 29)
        Me.LayoutControlItem24.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom
        Me.LayoutControlItem24.Text = "LayoutControlItem24"
        Me.LayoutControlItem24.TextSize = New System.Drawing.Size(0, 0)
        Me.LayoutControlItem24.TextToControlDistance = 0
        Me.LayoutControlItem24.TextVisible = False
        '
        'LayoutControlItem25
        '
        Me.LayoutControlItem25.Control = Me.cmdDelete
        Me.LayoutControlItem25.CustomizationFormText = "LayoutControlItem25"
        Me.LayoutControlItem25.Location = New System.Drawing.Point(366, 643)
        Me.LayoutControlItem25.MaxSize = New System.Drawing.Size(120, 29)
        Me.LayoutControlItem25.MinSize = New System.Drawing.Size(120, 29)
        Me.LayoutControlItem25.Name = "LayoutControlItem25"
        Me.LayoutControlItem25.Size = New System.Drawing.Size(120, 29)
        Me.LayoutControlItem25.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom
        Me.LayoutControlItem25.Text = "LayoutControlItem25"
        Me.LayoutControlItem25.TextSize = New System.Drawing.Size(0, 0)
        Me.LayoutControlItem25.TextToControlDistance = 0
        Me.LayoutControlItem25.TextVisible = False
        '
        'LayoutControlItem26
        '
        Me.LayoutControlItem26.Control = Me.cmdRefresh
        Me.LayoutControlItem26.CustomizationFormText = "LayoutControlItem26"
        Me.LayoutControlItem26.Location = New System.Drawing.Point(486, 643)
        Me.LayoutControlItem26.MaxSize = New System.Drawing.Size(120, 29)
        Me.LayoutControlItem26.MinSize = New System.Drawing.Size(120, 29)
        Me.LayoutControlItem26.Name = "LayoutControlItem26"
        Me.LayoutControlItem26.Size = New System.Drawing.Size(120, 29)
        Me.LayoutControlItem26.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom
        Me.LayoutControlItem26.Text = "LayoutControlItem26"
        Me.LayoutControlItem26.TextSize = New System.Drawing.Size(0, 0)
        Me.LayoutControlItem26.TextToControlDistance = 0
        Me.LayoutControlItem26.TextVisible = False
        '
        'LayoutControlGroup2
        '
        Me.LayoutControlGroup2.CustomizationFormText = "LayoutControlGroup2"
        Me.LayoutControlGroup2.Items.AddRange(New DevExpress.XtraLayout.BaseLayoutItem() {Me.LayoutControlItem4, Me.LayoutControlItem16, Me.LayoutControlItem17, Me.LayoutControlItem20, Me.LayoutControlItem37, Me.LayoutControlItem38, Me.LayoutControlItem39, Me.LayoutControlItem40})
        Me.LayoutControlGroup2.Location = New System.Drawing.Point(628, 437)
        Me.LayoutControlGroup2.Name = "LayoutControlGroup2"
        Me.LayoutControlGroup2.Size = New System.Drawing.Size(372, 206)
        Me.LayoutControlGroup2.Text = "LayoutControlGroup2"
        Me.LayoutControlGroup2.TextVisible = False
        '
        'LayoutControlItem4
        '
        Me.LayoutControlItem4.Control = Me.txtSubTotal
        Me.LayoutControlItem4.CustomizationFormText = "Subtotal 1"
        Me.LayoutControlItem4.Location = New System.Drawing.Point(0, 0)
        Me.LayoutControlItem4.Name = "LayoutControlItem4"
        Me.LayoutControlItem4.Size = New System.Drawing.Size(348, 26)
        Me.LayoutControlItem4.Text = "Subtotal"
        Me.LayoutControlItem4.TextSize = New System.Drawing.Size(116, 13)
        '
        'LayoutControlItem16
        '
        Me.LayoutControlItem16.Control = Me.txtBayar
        Me.LayoutControlItem16.CustomizationFormText = "Jumlah yang dibayarkan"
        Me.LayoutControlItem16.Location = New System.Drawing.Point(0, 104)
        Me.LayoutControlItem16.Name = "LayoutControlItem16"
        Me.LayoutControlItem16.Size = New System.Drawing.Size(348, 26)
        Me.LayoutControlItem16.Text = "Jumlah yang dibayarkan"
        Me.LayoutControlItem16.TextSize = New System.Drawing.Size(116, 13)
        '
        'LayoutControlItem17
        '
        Me.LayoutControlItem17.Control = Me.txtNoACC
        Me.LayoutControlItem17.CustomizationFormText = "No A/C"
        Me.LayoutControlItem17.Location = New System.Drawing.Point(0, 130)
        Me.LayoutControlItem17.Name = "LayoutControlItem17"
        Me.LayoutControlItem17.Size = New System.Drawing.Size(348, 26)
        Me.LayoutControlItem17.Text = "No A/C"
        Me.LayoutControlItem17.TextSize = New System.Drawing.Size(116, 13)
        '
        'LayoutControlItem20
        '
        Me.LayoutControlItem20.Control = Me.txtTglTransfer
        Me.LayoutControlItem20.CustomizationFormText = "Tgl Transfer"
        Me.LayoutControlItem20.Location = New System.Drawing.Point(0, 156)
        Me.LayoutControlItem20.Name = "LayoutControlItem20"
        Me.LayoutControlItem20.Size = New System.Drawing.Size(348, 26)
        Me.LayoutControlItem20.Text = "Tgl Transfer"
        Me.LayoutControlItem20.TextSize = New System.Drawing.Size(116, 13)
        '
        'LayoutControlItem37
        '
        Me.LayoutControlItem37.Control = Me.txtPotongan
        Me.LayoutControlItem37.CustomizationFormText = "CN / Materai"
        Me.LayoutControlItem37.Location = New System.Drawing.Point(0, 26)
        Me.LayoutControlItem37.Name = "LayoutControlItem37"
        Me.LayoutControlItem37.Size = New System.Drawing.Size(174, 26)
        Me.LayoutControlItem37.Text = "CN"
        Me.LayoutControlItem37.TextSize = New System.Drawing.Size(116, 13)
        '
        'LayoutControlItem38
        '
        Me.LayoutControlItem38.Control = Me.txtMaterai
        Me.LayoutControlItem38.CustomizationFormText = "Materai"
        Me.LayoutControlItem38.Location = New System.Drawing.Point(0, 52)
        Me.LayoutControlItem38.Name = "LayoutControlItem38"
        Me.LayoutControlItem38.Size = New System.Drawing.Size(348, 26)
        Me.LayoutControlItem38.Text = "Materai"
        Me.LayoutControlItem38.TextSize = New System.Drawing.Size(116, 13)
        '
        'LayoutControlItem39
        '
        Me.LayoutControlItem39.Control = Me.txtJumlah
        Me.LayoutControlItem39.CustomizationFormText = "Jumlah"
        Me.LayoutControlItem39.Location = New System.Drawing.Point(0, 78)
        Me.LayoutControlItem39.Name = "LayoutControlItem39"
        Me.LayoutControlItem39.Size = New System.Drawing.Size(348, 26)
        Me.LayoutControlItem39.Text = "Jumlah"
        Me.LayoutControlItem39.TextSize = New System.Drawing.Size(116, 13)
        '
        'LayoutControlItem40
        '
        Me.LayoutControlItem40.Control = Me.txtDN
        Me.LayoutControlItem40.CustomizationFormText = "DN"
        Me.LayoutControlItem40.Location = New System.Drawing.Point(174, 26)
        Me.LayoutControlItem40.Name = "LayoutControlItem40"
        Me.LayoutControlItem40.Size = New System.Drawing.Size(174, 26)
        Me.LayoutControlItem40.Text = "DN"
        Me.LayoutControlItem40.TextSize = New System.Drawing.Size(116, 13)
        '
        'LayoutControlItem5
        '
        Me.LayoutControlItem5.Control = Me.cmdLoad
        Me.LayoutControlItem5.CustomizationFormText = "LayoutControlItem5"
        Me.LayoutControlItem5.Location = New System.Drawing.Point(240, 643)
        Me.LayoutControlItem5.MaxSize = New System.Drawing.Size(126, 29)
        Me.LayoutControlItem5.MinSize = New System.Drawing.Size(126, 29)
        Me.LayoutControlItem5.Name = "LayoutControlItem5"
        Me.LayoutControlItem5.Size = New System.Drawing.Size(126, 29)
        Me.LayoutControlItem5.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom
        Me.LayoutControlItem5.Text = "LayoutControlItem5"
        Me.LayoutControlItem5.TextSize = New System.Drawing.Size(0, 0)
        Me.LayoutControlItem5.TextToControlDistance = 0
        Me.LayoutControlItem5.TextVisible = False
        '
        'LayoutControlGroup3
        '
        Me.LayoutControlGroup3.CustomizationFormText = "Kontra"
        Me.LayoutControlGroup3.Items.AddRange(New DevExpress.XtraLayout.BaseLayoutItem() {Me.LayoutControlItem3})
        Me.LayoutControlGroup3.Location = New System.Drawing.Point(502, 52)
        Me.LayoutControlGroup3.Name = "LayoutControlGroup3"
        Me.LayoutControlGroup3.Size = New System.Drawing.Size(498, 380)
        Me.LayoutControlGroup3.Text = "Retur"
        '
        'LayoutControlItem3
        '
        Me.LayoutControlItem3.Control = Me.gcRetur
        Me.LayoutControlItem3.CustomizationFormText = "GridControl"
        Me.LayoutControlItem3.Location = New System.Drawing.Point(0, 0)
        Me.LayoutControlItem3.Name = "LayoutControlItem3"
        Me.LayoutControlItem3.Size = New System.Drawing.Size(474, 336)
        Me.LayoutControlItem3.Text = "GridControl"
        Me.LayoutControlItem3.TextSize = New System.Drawing.Size(0, 0)
        Me.LayoutControlItem3.TextToControlDistance = 0
        Me.LayoutControlItem3.TextVisible = False
        '
        'LayoutControlGroup4
        '
        Me.LayoutControlGroup4.CustomizationFormText = "List Semua Nota"
        Me.LayoutControlGroup4.Items.AddRange(New DevExpress.XtraLayout.BaseLayoutItem() {Me.LayoutControlItem6})
        Me.LayoutControlGroup4.Location = New System.Drawing.Point(0, 52)
        Me.LayoutControlGroup4.Name = "LayoutControlGroup4"
        Me.LayoutControlGroup4.Size = New System.Drawing.Size(497, 380)
        Me.LayoutControlGroup4.Text = "Transaksi Pembelian"
        '
        'LayoutControlItem6
        '
        Me.LayoutControlItem6.Control = Me.gcPembelian
        Me.LayoutControlItem6.CustomizationFormText = "LayoutControlItem6"
        Me.LayoutControlItem6.Location = New System.Drawing.Point(0, 0)
        Me.LayoutControlItem6.Name = "LayoutControlItem6"
        Me.LayoutControlItem6.Size = New System.Drawing.Size(473, 336)
        Me.LayoutControlItem6.Text = "LayoutControlItem6"
        Me.LayoutControlItem6.TextSize = New System.Drawing.Size(0, 0)
        Me.LayoutControlItem6.TextToControlDistance = 0
        Me.LayoutControlItem6.TextVisible = False
        '
        'SplitterItem4
        '
        Me.SplitterItem4.AllowHotTrack = True
        Me.SplitterItem4.CustomizationFormText = "SplitterItem4"
        Me.SplitterItem4.Location = New System.Drawing.Point(497, 52)
        Me.SplitterItem4.Name = "SplitterItem4"
        Me.SplitterItem4.Size = New System.Drawing.Size(5, 380)
        '
        'SplitterItem6
        '
        Me.SplitterItem6.AllowHotTrack = True
        Me.SplitterItem6.CustomizationFormText = "SplitterItem6"
        Me.SplitterItem6.Location = New System.Drawing.Point(0, 432)
        Me.SplitterItem6.Name = "SplitterItem6"
        Me.SplitterItem6.Size = New System.Drawing.Size(1000, 5)
        '
        'EmptySpaceItem1
        '
        Me.EmptySpaceItem1.CustomizationFormText = "EmptySpaceItem1"
        Me.EmptySpaceItem1.Location = New System.Drawing.Point(334, 0)
        Me.EmptySpaceItem1.Name = "EmptySpaceItem1"
        Me.EmptySpaceItem1.Size = New System.Drawing.Size(330, 52)
        Me.EmptySpaceItem1.Text = "EmptySpaceItem1"
        Me.EmptySpaceItem1.TextSize = New System.Drawing.Size(0, 0)
        '
        'LayoutControlGroup7
        '
        Me.LayoutControlGroup7.CustomizationFormText = "Kwitansi"
        Me.LayoutControlGroup7.Items.AddRange(New DevExpress.XtraLayout.BaseLayoutItem() {Me.LayoutControlItem42, Me.LayoutControlItem43, Me.LayoutControlItem44, Me.LayoutControlItem35, Me.EmptySpaceItem3})
        Me.LayoutControlGroup7.Location = New System.Drawing.Point(0, 437)
        Me.LayoutControlGroup7.Name = "LayoutControlGroup7"
        Me.LayoutControlGroup7.Size = New System.Drawing.Size(279, 206)
        Me.LayoutControlGroup7.Text = "Kwitansi"
        '
        'LayoutControlItem42
        '
        Me.LayoutControlItem42.Control = Me.txtNoKwitansi
        Me.LayoutControlItem42.CustomizationFormText = "No Kwitansi"
        Me.LayoutControlItem42.Location = New System.Drawing.Point(0, 0)
        Me.LayoutControlItem42.Name = "LayoutControlItem42"
        Me.LayoutControlItem42.Size = New System.Drawing.Size(255, 26)
        Me.LayoutControlItem42.Text = "No Kwitansi"
        Me.LayoutControlItem42.TextSize = New System.Drawing.Size(116, 13)
        '
        'LayoutControlItem43
        '
        Me.LayoutControlItem43.Control = Me.txtAkunKwitansi
        Me.LayoutControlItem43.CustomizationFormText = "Perkiraan"
        Me.LayoutControlItem43.Location = New System.Drawing.Point(0, 26)
        Me.LayoutControlItem43.Name = "LayoutControlItem43"
        Me.LayoutControlItem43.Size = New System.Drawing.Size(255, 26)
        Me.LayoutControlItem43.Text = "Perkiraan"
        Me.LayoutControlItem43.TextSize = New System.Drawing.Size(116, 13)
        '
        'LayoutControlItem44
        '
        Me.LayoutControlItem44.Control = Me.txtKeteranganKwitansi
        Me.LayoutControlItem44.CustomizationFormText = "Keterangan"
        Me.LayoutControlItem44.Location = New System.Drawing.Point(0, 52)
        Me.LayoutControlItem44.Name = "LayoutControlItem44"
        Me.LayoutControlItem44.Size = New System.Drawing.Size(255, 26)
        Me.LayoutControlItem44.Text = "Keterangan"
        Me.LayoutControlItem44.TextSize = New System.Drawing.Size(116, 13)
        '
        'LayoutControlItem35
        '
        Me.LayoutControlItem35.Control = Me.txtJumlahKwitansi
        Me.LayoutControlItem35.CustomizationFormText = "Jumlah"
        Me.LayoutControlItem35.Location = New System.Drawing.Point(0, 78)
        Me.LayoutControlItem35.Name = "LayoutControlItem35"
        Me.LayoutControlItem35.Size = New System.Drawing.Size(255, 26)
        Me.LayoutControlItem35.Text = "Jumlah"
        Me.LayoutControlItem35.TextSize = New System.Drawing.Size(116, 13)
        '
        'EmptySpaceItem3
        '
        Me.EmptySpaceItem3.CustomizationFormText = "EmptySpaceItem3"
        Me.EmptySpaceItem3.Location = New System.Drawing.Point(0, 104)
        Me.EmptySpaceItem3.Name = "EmptySpaceItem3"
        Me.EmptySpaceItem3.Size = New System.Drawing.Size(255, 58)
        Me.EmptySpaceItem3.Text = "EmptySpaceItem3"
        Me.EmptySpaceItem3.TextSize = New System.Drawing.Size(0, 0)
        '
        'LayoutControlGroup8
        '
        Me.LayoutControlGroup8.CustomizationFormText = "Kredit Note"
        Me.LayoutControlGroup8.Items.AddRange(New DevExpress.XtraLayout.BaseLayoutItem() {Me.SplitterItem1, Me.LayoutControlGroup10, Me.LayoutControlGroup11})
        Me.LayoutControlGroup8.Location = New System.Drawing.Point(279, 437)
        Me.LayoutControlGroup8.Name = "LayoutControlGroup8"
        Me.LayoutControlGroup8.Size = New System.Drawing.Size(339, 206)
        Me.LayoutControlGroup8.Text = "Kredit Note"
        Me.LayoutControlGroup8.TextVisible = False
        '
        'SplitterItem1
        '
        Me.SplitterItem1.AllowHotTrack = True
        Me.SplitterItem1.CustomizationFormText = "SplitterItem1"
        Me.SplitterItem1.Location = New System.Drawing.Point(155, 0)
        Me.SplitterItem1.Name = "SplitterItem1"
        Me.SplitterItem1.Size = New System.Drawing.Size(5, 182)
        '
        'LayoutControlGroup10
        '
        Me.LayoutControlGroup10.CustomizationFormText = "CN"
        Me.LayoutControlGroup10.Items.AddRange(New DevExpress.XtraLayout.BaseLayoutItem() {Me.LayoutControlItem46, Me.LayoutControlItem45, Me.LayoutControlItem10})
        Me.LayoutControlGroup10.Location = New System.Drawing.Point(0, 0)
        Me.LayoutControlGroup10.Name = "LayoutControlGroup10"
        Me.LayoutControlGroup10.Size = New System.Drawing.Size(155, 182)
        Me.LayoutControlGroup10.Text = "CN"
        '
        'LayoutControlItem46
        '
        Me.LayoutControlItem46.Control = Me.cmdDelCN
        Me.LayoutControlItem46.CustomizationFormText = "LayoutControlItem46"
        Me.LayoutControlItem46.Location = New System.Drawing.Point(104, 29)
        Me.LayoutControlItem46.Name = "LayoutControlItem46"
        Me.LayoutControlItem46.Size = New System.Drawing.Size(27, 109)
        Me.LayoutControlItem46.Text = "LayoutControlItem46"
        Me.LayoutControlItem46.TextSize = New System.Drawing.Size(0, 0)
        Me.LayoutControlItem46.TextToControlDistance = 0
        Me.LayoutControlItem46.TextVisible = False
        '
        'LayoutControlItem45
        '
        Me.LayoutControlItem45.Control = Me.cmdAddCN
        Me.LayoutControlItem45.CustomizationFormText = "LayoutControlItem45"
        Me.LayoutControlItem45.Location = New System.Drawing.Point(104, 0)
        Me.LayoutControlItem45.Name = "LayoutControlItem45"
        Me.LayoutControlItem45.Size = New System.Drawing.Size(27, 29)
        Me.LayoutControlItem45.Text = "LayoutControlItem45"
        Me.LayoutControlItem45.TextSize = New System.Drawing.Size(0, 0)
        Me.LayoutControlItem45.TextToControlDistance = 0
        Me.LayoutControlItem45.TextVisible = False
        '
        'LayoutControlItem10
        '
        Me.LayoutControlItem10.Control = Me.gcKredit
        Me.LayoutControlItem10.CustomizationFormText = "KR"
        Me.LayoutControlItem10.Location = New System.Drawing.Point(0, 0)
        Me.LayoutControlItem10.Name = "LayoutControlItem10"
        Me.LayoutControlItem10.Size = New System.Drawing.Size(104, 138)
        Me.LayoutControlItem10.Text = "KR"
        Me.LayoutControlItem10.TextSize = New System.Drawing.Size(0, 0)
        Me.LayoutControlItem10.TextToControlDistance = 0
        Me.LayoutControlItem10.TextVisible = False
        '
        'LayoutControlGroup11
        '
        Me.LayoutControlGroup11.CustomizationFormText = "DN"
        Me.LayoutControlGroup11.Items.AddRange(New DevExpress.XtraLayout.BaseLayoutItem() {Me.LayoutControlItem9, Me.LayoutControlItem47, Me.LayoutControlItem48})
        Me.LayoutControlGroup11.Location = New System.Drawing.Point(160, 0)
        Me.LayoutControlGroup11.Name = "LayoutControlGroup11"
        Me.LayoutControlGroup11.Size = New System.Drawing.Size(155, 182)
        Me.LayoutControlGroup11.Text = "DN"
        '
        'LayoutControlItem9
        '
        Me.LayoutControlItem9.Control = Me.gcDebet
        Me.LayoutControlItem9.CustomizationFormText = "DB"
        Me.LayoutControlItem9.Location = New System.Drawing.Point(0, 0)
        Me.LayoutControlItem9.Name = "LayoutControlItem9"
        Me.LayoutControlItem9.Size = New System.Drawing.Size(104, 138)
        Me.LayoutControlItem9.Text = "DB"
        Me.LayoutControlItem9.TextSize = New System.Drawing.Size(0, 0)
        Me.LayoutControlItem9.TextToControlDistance = 0
        Me.LayoutControlItem9.TextVisible = False
        '
        'LayoutControlItem47
        '
        Me.LayoutControlItem47.Control = Me.cmdDelDN
        Me.LayoutControlItem47.CustomizationFormText = "LayoutControlItem47"
        Me.LayoutControlItem47.Location = New System.Drawing.Point(104, 29)
        Me.LayoutControlItem47.Name = "LayoutControlItem47"
        Me.LayoutControlItem47.Size = New System.Drawing.Size(27, 109)
        Me.LayoutControlItem47.Text = "LayoutControlItem47"
        Me.LayoutControlItem47.TextSize = New System.Drawing.Size(0, 0)
        Me.LayoutControlItem47.TextToControlDistance = 0
        Me.LayoutControlItem47.TextVisible = False
        '
        'LayoutControlItem48
        '
        Me.LayoutControlItem48.Control = Me.cmdAddDN
        Me.LayoutControlItem48.CustomizationFormText = "LayoutControlItem48"
        Me.LayoutControlItem48.Location = New System.Drawing.Point(104, 0)
        Me.LayoutControlItem48.Name = "LayoutControlItem48"
        Me.LayoutControlItem48.Size = New System.Drawing.Size(27, 29)
        Me.LayoutControlItem48.Text = "LayoutControlItem48"
        Me.LayoutControlItem48.TextSize = New System.Drawing.Size(0, 0)
        Me.LayoutControlItem48.TextToControlDistance = 0
        Me.LayoutControlItem48.TextVisible = False
        '
        'EmptySpaceItem6
        '
        Me.EmptySpaceItem6.CustomizationFormText = "EmptySpaceItem6"
        Me.EmptySpaceItem6.Location = New System.Drawing.Point(618, 437)
        Me.EmptySpaceItem6.Name = "EmptySpaceItem6"
        Me.EmptySpaceItem6.Size = New System.Drawing.Size(10, 206)
        Me.EmptySpaceItem6.Text = "EmptySpaceItem6"
        Me.EmptySpaceItem6.TextSize = New System.Drawing.Size(0, 0)
        '
        'LayoutControlItem12
        '
        Me.LayoutControlItem12.CustomizationFormText = "Fast Entri Barang"
        Me.LayoutControlItem12.Location = New System.Drawing.Point(0, 317)
        Me.LayoutControlItem12.Name = "LayoutControlItem2"
        Me.LayoutControlItem12.Size = New System.Drawing.Size(708, 26)
        Me.LayoutControlItem12.Text = "Fast Entri Barang"
        Me.LayoutControlItem12.TextSize = New System.Drawing.Size(83, 13)
        Me.LayoutControlItem12.TextToControlDistance = 5
        '
        'cKassa
        '
        Me.cKassa.Caption = "Kassa"
        Me.cKassa.DisplayFormat.FormatString = "n0"
        Me.cKassa.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
        Me.cKassa.FieldName = "Kassa"
        Me.cKassa.Name = "cKassa"
        Me.cKassa.OptionsColumn.AllowEdit = False
        Me.cKassa.Visible = True
        Me.cKassa.VisibleIndex = 4
        '
        'frmEntriPembayaranPiutangOK
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1020, 742)
        Me.Controls.Add(Me.LayoutControl1)
        Me.Controls.Add(Me.barDockControlLeft)
        Me.Controls.Add(Me.barDockControlRight)
        Me.Controls.Add(Me.barDockControlBottom)
        Me.Controls.Add(Me.barDockControlTop)
        Me.Name = "frmEntriPembayaranPiutangOK"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Entri Bayar Hutang / Piutang"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        CType(Me.LayoutControl1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.LayoutControl1.ResumeLayout(False)
        CType(Me.txtKeteranganKwitansi.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtAkunKwitansi.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.BarManager1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.GridView1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtNoKwitansi.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtDN.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtJumlah.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtMaterai.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtPotongan.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtKwitansi.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.gvKwitansiMasuk, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtTglTransfer.Properties.VistaTimeProperties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtTglTransfer.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtNoACC.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtJumlahKwitansi.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtBayar.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.gcGiro, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.gvGiro, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.gcKredit, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.gvKredit, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.gcDebet, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.gvDebet, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.gcPH, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.gvPH, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.gcTU, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.gvTU, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.gcPembelian, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.gvPembelian, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtWilayah.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.gvWilayah, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.tglPosting.Properties.VistaTimeProperties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.tglPosting.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.tglEdit.Properties.VistaTimeProperties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.tglEdit.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.tglEntri.Properties.VistaTimeProperties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.tglEntri.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtDipostingOleh.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtDieditOleh.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtDientriOleh.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtKodeAlamat.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.SearchLookUpEdit1View, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtKode.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Tgl.Properties.VistaTimeProperties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Tgl.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtBarang.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.gcRetur, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.gvRetur, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtSubTotal.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtNamaAlamat.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtCatatan.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtAlamat.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem31, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem32, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem27, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem28, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem29, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem13, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem30, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem33, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem36, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem34, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlGroup9, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem11, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlGroup6, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem8, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlGroup5, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem7, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem22, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlGroup1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.EmptySpaceItem2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem15, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem14, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem18, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem19, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem23, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem24, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem25, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem26, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlGroup2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem4, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem16, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem17, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem20, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem37, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem38, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem39, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem40, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem5, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlGroup3, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem3, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlGroup4, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem6, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.SplitterItem4, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.SplitterItem6, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.EmptySpaceItem1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlGroup7, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem42, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem43, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem44, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem35, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.EmptySpaceItem3, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlGroup8, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.SplitterItem1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlGroup10, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem46, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem45, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem10, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlGroup11, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem9, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem47, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem48, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.EmptySpaceItem6, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem12, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents LayoutControl1 As DevExpress.XtraLayout.LayoutControl
    Friend WithEvents LayoutControlGroup1 As DevExpress.XtraLayout.LayoutControlGroup
    Friend WithEvents cmdSave As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents cmdTutup As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents LayoutControlItem1 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents EmptySpaceItem2 As DevExpress.XtraLayout.EmptySpaceItem
    Friend WithEvents LayoutControlItem2 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents BarManager1 As DevExpress.XtraBars.BarManager
    Friend WithEvents Bar2 As DevExpress.XtraBars.Bar
    Friend WithEvents Bar3 As DevExpress.XtraBars.Bar
    Friend WithEvents barDockControlTop As DevExpress.XtraBars.BarDockControl
    Friend WithEvents barDockControlBottom As DevExpress.XtraBars.BarDockControl
    Friend WithEvents barDockControlLeft As DevExpress.XtraBars.BarDockControl
    Friend WithEvents barDockControlRight As DevExpress.XtraBars.BarDockControl
    Friend WithEvents BarSubItem1 As DevExpress.XtraBars.BarSubItem
    Friend WithEvents gcRetur As DevExpress.XtraGrid.GridControl
    Friend WithEvents gvRetur As DevExpress.XtraGrid.Views.Grid.GridView
    Friend WithEvents LayoutControlItem3 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents LayoutControlItem12 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents txtBarang As DevExpress.XtraEditors.ButtonEdit
    Friend WithEvents LayoutControlItem13 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents LayoutControlItem4 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents txtKode As DevExpress.XtraEditors.ButtonEdit
    Friend WithEvents Tgl As DevExpress.XtraEditors.DateEdit
    Friend WithEvents LayoutControlItem15 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents LayoutControlItem14 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents txtKodeAlamat As DevExpress.XtraEditors.SearchLookUpEdit
    Friend WithEvents SearchLookUpEdit1View As DevExpress.XtraGrid.Views.Grid.GridView
    Friend WithEvents EmptySpaceItem6 As DevExpress.XtraLayout.EmptySpaceItem
    Friend WithEvents LayoutControlItem18 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents LayoutControlItem19 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents BarButtonItem1 As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents txtSubTotal As DevExpress.XtraEditors.TextEdit
    Friend WithEvents BarButtonItem3 As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents cmdBAru As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents cmdDelete As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents cmdEdit As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents LayoutControlItem23 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents LayoutControlItem24 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents LayoutControlItem25 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents cmdRefresh As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents LayoutControlItem26 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents mnCancel As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents BarButtonItem2 As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents BarButtonItem4 As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents BarButtonItem5 As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents BarButtonItem6 As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents BarButtonItem7 As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents txtDipostingOleh As DevExpress.XtraEditors.ButtonEdit
    Friend WithEvents txtDieditOleh As DevExpress.XtraEditors.ButtonEdit
    Friend WithEvents txtDientriOleh As DevExpress.XtraEditors.ButtonEdit
    Friend WithEvents LayoutControlItem27 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents LayoutControlItem28 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents LayoutControlItem29 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents tglPosting As DevExpress.XtraEditors.DateEdit
    Friend WithEvents tglEdit As DevExpress.XtraEditors.DateEdit
    Friend WithEvents tglEntri As DevExpress.XtraEditors.DateEdit
    Friend WithEvents LayoutControlItem30 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents LayoutControlItem31 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents LayoutControlItem32 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents txtNamaAlamat As DevExpress.XtraEditors.TextEdit
    Friend WithEvents LayoutControlItem33 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents txtCatatan As DevExpress.XtraEditors.MemoEdit
    Friend WithEvents LayoutControlItem34 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents LayoutControlGroup2 As DevExpress.XtraLayout.LayoutControlGroup
    Friend WithEvents txtAlamat As DevExpress.XtraEditors.MemoEdit
    Friend WithEvents txtWilayah As DevExpress.XtraEditors.SearchLookUpEdit
    Friend WithEvents gvWilayah As DevExpress.XtraGrid.Views.Grid.GridView
    Friend WithEvents LayoutControlItem36 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents cmdLoad As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents LayoutControlItem5 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents gcPembelian As DevExpress.XtraGrid.GridControl
    Friend WithEvents gvPembelian As DevExpress.XtraGrid.Views.Grid.GridView
    Friend WithEvents LayoutControlGroup3 As DevExpress.XtraLayout.LayoutControlGroup
    Friend WithEvents LayoutControlGroup4 As DevExpress.XtraLayout.LayoutControlGroup
    Friend WithEvents LayoutControlItem6 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents gcTU As DevExpress.XtraGrid.GridControl
    Friend WithEvents gvTU As DevExpress.XtraGrid.Views.Grid.GridView
    Friend WithEvents LayoutControlGroup5 As DevExpress.XtraLayout.LayoutControlGroup
    Friend WithEvents LayoutControlItem7 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents gcKredit As DevExpress.XtraGrid.GridControl
    Friend WithEvents gvKredit As DevExpress.XtraGrid.Views.Grid.GridView
    Friend WithEvents gcDebet As DevExpress.XtraGrid.GridControl
    Friend WithEvents gvDebet As DevExpress.XtraGrid.Views.Grid.GridView
    Friend WithEvents gcPH As DevExpress.XtraGrid.GridControl
    Friend WithEvents gvPH As DevExpress.XtraGrid.Views.Grid.GridView
    Friend WithEvents LayoutControlGroup6 As DevExpress.XtraLayout.LayoutControlGroup
    Friend WithEvents LayoutControlItem8 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents LayoutControlGroup8 As DevExpress.XtraLayout.LayoutControlGroup
    Friend WithEvents LayoutControlItem10 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents gcGiro As DevExpress.XtraGrid.GridControl
    Friend WithEvents gvGiro As DevExpress.XtraGrid.Views.Grid.GridView
    Friend WithEvents LayoutControlGroup9 As DevExpress.XtraLayout.LayoutControlGroup
    Friend WithEvents LayoutControlItem11 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents txtBayar As DevExpress.XtraEditors.TextEdit
    Friend WithEvents LayoutControlItem16 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents txtNoACC As DevExpress.XtraEditors.TextEdit
    Friend WithEvents LayoutControlItem17 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents txtTglTransfer As DevExpress.XtraEditors.DateEdit
    Friend WithEvents LayoutControlItem20 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents cNoID As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents cIDBeli As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents cNoNota As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents cTanggal As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents CTotal As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents cBayar As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents cRNoID As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents CrIDRetur As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents crNoRetur As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents CrTanggal As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents CrTotal As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents CrPotong As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents cPNoID As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents CPIDPH As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents CpNoPH As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents CpTanggal As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents CpJumlah As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents CpPotong As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents cbNoID As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents cbNomor As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents cbTanggal As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents cbJumlah As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents cbPotong As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents SplitterItem1 As DevExpress.XtraLayout.SplitterItem
    Friend WithEvents SplitterItem4 As DevExpress.XtraLayout.SplitterItem
    Friend WithEvents LayoutControlItem9 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents cdnNoID As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents cdnTanggal As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents cdnKode As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents cdnTotal As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents cdnPotong As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents cdnKeterangan As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents gnkNoID As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents gnkKode As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents gnkTanggal As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents gnkTotal As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents gnkPotong As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents gnkCatatan As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents SplitterItem6 As DevExpress.XtraLayout.SplitterItem
    Friend WithEvents txtKwitansi As DevExpress.XtraEditors.SearchLookUpEdit
    Friend WithEvents gvKwitansiMasuk As DevExpress.XtraGrid.Views.Grid.GridView
    Friend WithEvents LayoutControlItem22 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents txtJumlahKwitansi As DevExpress.XtraEditors.TextEdit
    Friend WithEvents LayoutControlItem35 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents txtPotongan As DevExpress.XtraEditors.TextEdit
    Friend WithEvents LayoutControlItem37 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents txtMaterai As DevExpress.XtraEditors.TextEdit
    Friend WithEvents LayoutControlItem38 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents txtJumlah As DevExpress.XtraEditors.TextEdit
    Friend WithEvents LayoutControlItem39 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents txtDN As DevExpress.XtraEditors.TextEdit
    Friend WithEvents LayoutControlItem40 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents EmptySpaceItem1 As DevExpress.XtraLayout.EmptySpaceItem
    Friend WithEvents txtNoKwitansi As DevExpress.XtraEditors.TextEdit
    Friend WithEvents LayoutControlItem42 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents txtAkunKwitansi As DevExpress.XtraEditors.SearchLookUpEdit
    Friend WithEvents GridView1 As DevExpress.XtraGrid.Views.Grid.GridView
    Friend WithEvents LayoutControlItem43 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents txtKeteranganKwitansi As DevExpress.XtraEditors.TextEdit
    Friend WithEvents LayoutControlItem44 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents LayoutControlGroup7 As DevExpress.XtraLayout.LayoutControlGroup
    Friend WithEvents EmptySpaceItem3 As DevExpress.XtraLayout.EmptySpaceItem
    Friend WithEvents cmdAddCN As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents LayoutControlItem45 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents cmdDelCN As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents LayoutControlItem46 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents LayoutControlGroup10 As DevExpress.XtraLayout.LayoutControlGroup
    Friend WithEvents cmdAddDN As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents cmdDelDN As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents LayoutControlGroup11 As DevExpress.XtraLayout.LayoutControlGroup
    Friend WithEvents LayoutControlItem47 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents LayoutControlItem48 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents cKassa As DevExpress.XtraGrid.Columns.GridColumn
End Class
