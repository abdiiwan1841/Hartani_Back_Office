Imports VPOINT.Function.mPublic
Imports VPOINT.Function.Fungsi
Imports System.Data
Imports DevExpress.XtraEditors
Imports DevExpress.Utils
Imports VPOINT.Serialshield.Ini
Imports DevExpress.XtraGrid.Views.Base
Public Class frmEntriSaldoAwalStokFisik
    Public pStatus As pStatus_, namaTabel As String = "MLPBD"
    Public IDSTBLain, IDBarang, ID As Long
    Public IsTester As Boolean = False
    Public IDSaldoAwal As Long = -1
    Public IDWilayah, IDGudang, IDWadah, IDAkunPersediaan, IDSatuanParent, IDSatuan As Long
    Dim IDNotaTimbang As Long
    Dim appdat As String = Application.StartupPath & "\system\layouts\" & Name & ".ini"
    Public Shared IDKadar1, IDKadar2, IDKadar3, IDKadar4, IDKadar5, IDKadar6, IDKadar7, IDKadar8, IDKadar9, IDKadar10 As Long
    Public IDSupplier As Long = -1, NamaSupplier As String = ""
    Dim OldNota As String = ""
    Public Enum pStatus_
        Baru = 0
        Edit = 1
    End Enum
    Private Sub cmdClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdClose.Click
        DialogResult = Windows.Forms.DialogResult.Cancel
        Close()
    End Sub
    Private Sub SetCtlMe()
        appdat = Application.StartupPath & "\system\layouts\" & Name & ".dat"
        'IDGudang = defIDGudang
        'IDWilayah = defIDWilayah
        'txtGudang.Text = defNamaGudang
        txtTgl.DateTime = Date.Today 'VPOINT.Function.mPublic.TanggalSystem
        'If IsTester Then
        namaTabel = "MLPBD"
        'End If
        'If Not IsAdministrator Then
        '    txtGudang.Enabled = False
        'End If
        With txtNetto1
            .Properties.DisplayFormat.FormatString = Ini.BacaIniPath(appdat, .Name, "DisplayFormat", "n2")
            .Properties.EditFormat.FormatString = Ini.BacaIniPath(appdat, .Name, "EditFormat", "n2")
            .Properties.Mask.EditMask = Ini.BacaIniPath(appdat, .Name, "EditMask", "n2")
        End With
        Ini.TulisIniPath(appdat, Width, "Height", Name)
        Ini.TulisIniPath(appdat, Height, "Height", Name)
    End Sub

    Private Sub frmEntriSTBD_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        With txtNetto1
            Ini.TulisIniPath(appdat, .Name, "DisplayFormat", .Properties.DisplayFormat.FormatString.ToString)
            Ini.TulisIniPath(appdat, .Name, "EditFormat", .Properties.EditFormat.FormatString.ToString)
            Ini.TulisIniPath(appdat, .Name, "EditMask", .Properties.Mask.EditMask.ToString)
        End With
        Ini.TulisIniPath(appdat, Width, "Height", Name)
        Ini.TulisIniPath(appdat, Height, "Height", Name)
    End Sub
    Private Sub frmEntriKasInD_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        SetCtlMe()
        If pStatus = ptipe.Edit Then
            GetData()
        Else
            IsiDefault()
        End If
        If IsTester Then
            TesterBarang.PageVisible = True
            TabTester.PageVisible = False
            TabUmum.PageEnabled = False
            XtraTabControl1.SelectedTabPage = TesterBarang
        Else
            TesterBarang.PageVisible = False
            TabTester.PageVisible = False
            TabUmum.PageEnabled = True
        End If
        If System.IO.File.Exists(LayOutKu(Me.Name & GridView1.Name)) Then
            GridView1.RestoreLayoutFromXml(LayOutKu(Me.Name & GridView1.Name))
        End If
    End Sub
    Private Sub GetData()
        Dim SQL As String
        Dim oDS As New DataSet
        SQL = "SELECT " & IIf(VPOINT.Serialshield.Serial.IsTrial, " top 100 ", "") & namaTabel & ".*, MAlamat.Nama AS NamaSupplier, MLPBMaster.IDSupplier, MLPBMaster.Kode AS Kode, MLPBMaster.Tanggal AS TanggalSaldoAwal, MLPBMaster.ID AS IDSaldoAwal, MLPBMaster.IDWilayah, MLPB.Kode AS KodeLPBLain, MLPBD_1.ID AS IDLPBLain, MNotaTimbang.Kode AS KodeNotaTimbang, MWadah.Kode as KdWadah, MWadah.Nama as NmWadah, MGudang.Kode as KdGudang, MGudang.Nama as NmGudang, MBarang.Nama as NmBrg, MBarang.Satuan As SatBrg, MBarang.Kode As KdBrg,MBarang.IDSatuan AS IDSatuanParent , MSatuan.Nama As NamaSatuan" & vbCrLf
        SQL = SQL & " FROM ((((((" & namaTabel & " LEFT Join MBarang ON MBarang.ID=" & namaTabel & ".IDBarang) " & vbCrLf
        SQL = SQL & " LEFT JOIN MGudang ON MGudang.ID=" & namaTabel & ".IDGudang) " & vbCrLf
        SQL = SQL & " LEFT JOIN MWadah ON MWadah.ID=" & namaTabel & ".IDWadah) " & vbCrLf
        SQL = SQL & " LEFT JOIN MSatuan ON MSatuan.NoID=" & namaTabel & ".IDSatuan) " & vbCrLf
        SQL = SQL & " LEFT JOIN (MLPB AS MLPBMaster LEFT JOIN MAlamat ON MAlamat.ID=MLPBMaster.IDSupplier) ON MLPBMaster.ID=" & namaTabel & ".IDBeli) " & vbCrLf
        SQL = SQL & " LEFT JOIN (MLPBD AS MLPBD_1 LEFT JOIN MLPB ON MLPB.ID=MLPBD_1.IDBeli) ON MLPBD_1.NoID=" & namaTabel & ".IDSTBD )" & vbCrLf
        SQL = SQL & " LEFT JOIN MNotaTimbang ON MNotaTimbang.ID=" & namaTabel & ".IDNotaTimbang " & vbCrLf
        SQL = SQL & " WHERE " & namaTabel & ".NoID = " & ID

        Try
            oDS = MyConn.ExecuteDataset(namaTabel, SQL)
            If oDS.Tables(namaTabel).Rows.Count >= 1 Then
                IDBarang = NullToLong(oDS.Tables(namaTabel).Rows(0).Item("IDBarang").ToString)
                IDSupplier = NullToLong(oDS.Tables(namaTabel).Rows(0).Item("IDSupplier").ToString)
                IDNotaTimbang = NullToLong(oDS.Tables(namaTabel).Rows(0).Item("IDNotaTimbang").ToString)
                IDGudang = NullToLong(oDS.Tables(namaTabel).Rows(0).Item("IDGudang").ToString)
                IDWilayah = NullToLong(oDS.Tables(namaTabel).Rows(0).Item("IDWilayah").ToString)
                IDSaldoAwal = NullToLong(oDS.Tables(namaTabel).Rows(0).Item("IDSaldoAwal").ToString)
                Tgl.Text = NullTostr(oDS.Tables(namaTabel).Rows(0).Item("Kode"))
                txtTgl.DateTime = NullToDate(oDS.Tables(namaTabel).Rows(0).Item("TanggalSaldoAwal"))
                OldNota = NullTostr(oDS.Tables(namaTabel).Rows(0).Item("Kode").ToString)
                IDWadah = NullToLong(oDS.Tables(namaTabel).Rows(0).Item("IDWadah").ToString)
                txtGudang.Text = NullTostr(oDS.Tables(namaTabel).Rows(0).Item("KdGudang").ToString & " - " & oDS.Tables(namaTabel).Rows(0).Item("NmGudang").ToString)
                txtBarang.Text = NullTostr(oDS.Tables(namaTabel).Rows(0).Item("NmBrg").ToString) & " - " & NullTostr(oDS.Tables(namaTabel).Rows(0).Item("SatBrg"))
                txtBarang2.Text = txtBarang.Text
                txtBarang3.Text = txtBarang.Text
                ckBelumDiBeli.Checked = NullToBool(oDS.Tables(namaTabel).Rows(0).Item("issabelumdibeli"))
                ckBelumDiproses.Checked = NullToBool(oDS.Tables(namaTabel).Rows(0).Item("issabelumdiproses"))
                ckMasukStok.Checked = NullToBool(oDS.Tables(namaTabel).Rows(0).Item("issamasukstok"))
                IDAkunPersediaan = NullToLong(oDS.Tables(namaTabel).Rows(0).Item("IDAKunPersediaan").ToString)
                txtNotaTimbang.Text = NullTostr(oDS.Tables(namaTabel).Rows(0).Item("KodeNotaTimbang").ToString)
                txtWadah.Text = NullTostr(oDS.Tables(namaTabel).Rows(0).Item("NmWadah").ToString)
                txtCatatan.Text = NullTostr(oDS.Tables(namaTabel).Rows(0).Item("Catatan").ToString)
                IDSatuan = NullToLong(oDS.Tables(namaTabel).Rows(0).Item("IDSatuan").ToString)
                txtSatuan.Text = NullTostr(oDS.Tables(namaTabel).Rows(0).Item("NamaSatuan").ToString)
                IDSatuanParent = NullToLong(oDS.Tables(namaTabel).Rows(0).Item("IDSatuanParent").ToString)
                txtSupplier.Text = NullTostr(oDS.Tables(namaTabel).Rows(0).Item("NamaSupplier").ToString)
                IDSTBLain = NullToLong(oDS.Tables(namaTabel).Rows(0).Item("IDSTBD").ToString)
                txtSTB.Text = NullTostr(oDS.Tables(namaTabel).Rows(0).Item("KodeLPBLain").ToString) & " - " & NullTostr(oDS.Tables(namaTabel).Rows(0).Item("IDLPBLain").ToString)
                txtColly.EditValue = NullToDbl(oDS.Tables(namaTabel).Rows(0).Item("JmlWadah").ToString)
                txtBruto.EditValue = NullToDbl(oDS.Tables(namaTabel).Rows(0).Item("Bruto").ToString)
                txtTarra.EditValue = NullToDbl(oDS.Tables(namaTabel).Rows(0).Item("Tara").ToString)
                txtPotongan.EditValue = NullToDbl(oDS.Tables(namaTabel).Rows(0).Item("Potongan").ToString)
                txtBS.EditValue = NullToDbl(oDS.Tables(namaTabel).Rows(0).Item("BS").ToString)
                HitungJumlah()
                HitungKirim()

                txtKadar1.Text = NullTostr(oDS.Tables(namaTabel).Rows(0).Item("KodeKadar1").ToString)
                txtKadar2.Text = NullTostr(oDS.Tables(namaTabel).Rows(0).Item("KodeKadar2").ToString)
                txtKadar3.Text = NullTostr(oDS.Tables(namaTabel).Rows(0).Item("KodeKadar3").ToString)
                txtKadar4.Text = NullTostr(oDS.Tables(namaTabel).Rows(0).Item("KodeKadar4").ToString)
                txtKadar5.Text = NullTostr(oDS.Tables(namaTabel).Rows(0).Item("KodeKadar5").ToString)
                txtKadar6.Text = NullTostr(oDS.Tables(namaTabel).Rows(0).Item("KodeKadar6").ToString)
                txtKadar7.Text = NullTostr(oDS.Tables(namaTabel).Rows(0).Item("KodeKadar7").ToString)
                txtKadar8.Text = NullTostr(oDS.Tables(namaTabel).Rows(0).Item("KodeKadar8").ToString)
                txtKadar9.Text = NullTostr(oDS.Tables(namaTabel).Rows(0).Item("KodeKadar9").ToString)
                txtKadar10.Text = NullTostr(oDS.Tables(namaTabel).Rows(0).Item("KodeKadar10").ToString)

                IDKadar1 = NullToLong(oDS.Tables(namaTabel).Rows(0).Item("IDKadar1"))
                IDKadar2 = NullToLong(oDS.Tables(namaTabel).Rows(0).Item("IDKadar2"))
                IDKadar3 = NullToLong(oDS.Tables(namaTabel).Rows(0).Item("IDKadar3"))
                IDKadar4 = NullToLong(oDS.Tables(namaTabel).Rows(0).Item("IDKadar4"))
                IDKadar5 = NullToLong(oDS.Tables(namaTabel).Rows(0).Item("IDKadar5"))
                IDKadar6 = NullToLong(oDS.Tables(namaTabel).Rows(0).Item("IDKadar6"))
                IDKadar7 = NullToLong(oDS.Tables(namaTabel).Rows(0).Item("IDKadar7"))
                IDKadar8 = NullToLong(oDS.Tables(namaTabel).Rows(0).Item("IDKadar8"))
                IDKadar9 = NullToLong(oDS.Tables(namaTabel).Rows(0).Item("IDKadar9"))
                IDKadar10 = NullToLong(oDS.Tables(namaTabel).Rows(0).Item("IDKadar10"))

                txtJmlKadar1.EditValue = NullToDbl(oDS.Tables(namaTabel).Rows(0).Item("NilaiKadar1"))
                txtJmlKadar2.EditValue = NullToDbl(oDS.Tables(namaTabel).Rows(0).Item("NilaiKadar2"))
                txtJmlKadar3.EditValue = NullToDbl(oDS.Tables(namaTabel).Rows(0).Item("NilaiKadar3"))
                txtJmlKadar4.EditValue = NullToDbl(oDS.Tables(namaTabel).Rows(0).Item("NilaiKadar4"))
                txtJmlKadar5.EditValue = NullToDbl(oDS.Tables(namaTabel).Rows(0).Item("NilaiKadar5"))
                txtJmlKadar6.EditValue = NullToDbl(oDS.Tables(namaTabel).Rows(0).Item("NilaiKadar6"))
                txtJmlKadar7.EditValue = NullToDbl(oDS.Tables(namaTabel).Rows(0).Item("NilaiKadar7"))
                txtJmlKadar8.EditValue = NullToDbl(oDS.Tables(namaTabel).Rows(0).Item("NilaiKadar8"))
                txtJmlKadar9.EditValue = NullToDbl(oDS.Tables(namaTabel).Rows(0).Item("NilaiKadar9"))
                txtJmlKadar10.EditValue = NullToDbl(oDS.Tables(namaTabel).Rows(0).Item("NilaiKadar10"))

                txtClaim.EditValue = NullToDbl(oDS.Tables(namaTabel).Rows(0).Item("Claim"))
                txtPenyesuaianClaim.EditValue = NullToDbl(oDS.Tables(namaTabel).Rows(0).Item("PenyesuaianClaim"))
                txtTotalClaim.EditValue = NullToDbl(oDS.Tables(namaTabel).Rows(0).Item("TotalClaim"))

            End If
        Catch ex As Exception
            FxMessage(ex.Message, NamaApplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error, , , ex.StackTrace)
        Finally
            oDS.Dispose()
        End Try
        RefreshItem()
    End Sub
    Private Sub RefreshItem()
        Try
            Dim SQL As String
            SQL = "SELECT MLPBDTester.*, MTester.NoUrut, MBarang.Nama AS NamaBarang, MBarang.Konversi, MSatuan.Nama AS Satuan " & vbCrLf
            SQL = SQL & " FROM (MLPBDTester LEFT JOIN " & vbCrLf
            SQL = SQL & " (MBarang LEFT JOIN MSatuan ON MSatuan.NoID=MBarang.IDsatuan) ON MLPBDTester.IDBarang=MBarang.ID)" & vbCrLf
            SQL = SQL & " LEFT JOIN MTester ON MLPBDTester.IDTester=MTester.ID " & vbCrLf
            SQL = SQL & " WHERE MLPBDTester.IDLPBD=" & ID
            SQL = SQL & " ORDER BY MTester.NoUrut"
            MyConn.ExecuteDBGrid(GridControl1, SQL)
        Catch ex As Exception

        End Try
    End Sub
    Private Sub GetDataSTB(ByVal NoID As Long)
        Dim SQL As String
        Dim oDS As New DataSet
        SQL = "SELECT " & IIf(VPOINT.Serialshield.Serial.IsTrial, " top 100 ", "") & "MLPBD.*, MNotaTimbang.Kode AS KodeNotaTimbang, MWadah.Kode as KdWadah, MWadah.Nama as NmWadah, MGudang.Kode as KdGudang, MGudang.Nama as NmGudang, MBarang.Nama as NmBrg, MBarang.Satuan As SatBrg, MBarang.Kode As KdBrg,MBarang.IDSatuan AS IDSatuanParent , MSatuan.Nama As NamaSatuan" & vbCrLf
        SQL = SQL & " FROM ((((MLPBD LEFT Join MBarang ON MBarang.ID=MLPBD.IDBarang) " & vbCrLf
        SQL = SQL & " LEFT JOIN MGudang ON MGudang.ID=MLPBD.IDGudang) "
        SQL = SQL & " LEFT JOIN MWadah ON MWadah.ID=MLPBD.IDWadah) "
        SQL = SQL & " LEFT JOIN MSatuan ON MSatuan.NoID=MLPBD.IDSatuan) "
        SQL = SQL & " LEFT JOIN MNotaTimbang ON MNotaTimbang.ID=MLPBD.IDNotaTimbang "
        SQL = SQL & " WHERE MLPBD.NoID = " & NoID
        Try
            oDS = MyConn.ExecuteDataset(namaTabel, SQL)
            If oDS.Tables(namaTabel).Rows.Count >= 1 Then
                IDBarang = NullToLong(oDS.Tables(namaTabel).Rows(0).Item("IDBarang").ToString)
                IDNotaTimbang = NullToLong(oDS.Tables(namaTabel).Rows(0).Item("IDNotaTimbang").ToString)
                IDGudang = NullToLong(oDS.Tables(namaTabel).Rows(0).Item("IDGudang").ToString)
                IDWadah = NullToLong(oDS.Tables(namaTabel).Rows(0).Item("IDWadah").ToString)
                txtGudang.Text = NullTostr(oDS.Tables(namaTabel).Rows(0).Item("KdGudang").ToString & " - " & oDS.Tables(namaTabel).Rows(0).Item("NmGudang").ToString)
                txtBarang.Text = NullTostr(oDS.Tables(namaTabel).Rows(0).Item("NmBrg").ToString) & " - " & NullTostr(oDS.Tables(namaTabel).Rows(0).Item("SatBrg"))
                txtBarang2.Text = txtBarang.Text
                txtBarang3.Text = txtBarang.Text
                IDAkunPersediaan = NullToLong(oDS.Tables(namaTabel).Rows(0).Item("IDAKunPersediaan").ToString)
                txtNotaTimbang.Text = NullTostr(oDS.Tables(namaTabel).Rows(0).Item("KodeNotaTimbang").ToString)
                txtWadah.Text = NullTostr(oDS.Tables(namaTabel).Rows(0).Item("NmWadah").ToString)
                txtCatatan.Text = NullTostr(oDS.Tables(namaTabel).Rows(0).Item("Catatan").ToString)
                IDSatuan = NullToLong(oDS.Tables(namaTabel).Rows(0).Item("IDSatuan").ToString)
                txtSatuan.Text = NullTostr(oDS.Tables(namaTabel).Rows(0).Item("NamaSatuan").ToString)
                IDSatuanParent = NullToLong(oDS.Tables(namaTabel).Rows(0).Item("IDSatuanParent").ToString)

                txtColly.EditValue = NullToDbl(oDS.Tables(namaTabel).Rows(0).Item("JmlWadah").ToString)
                txtBruto.EditValue = NullToDbl(oDS.Tables(namaTabel).Rows(0).Item("Bruto").ToString)
                txtTarra.EditValue = NullToDbl(oDS.Tables(namaTabel).Rows(0).Item("Tara").ToString)
                txtPotongan.EditValue = NullToDbl(oDS.Tables(namaTabel).Rows(0).Item("Potongan").ToString)
                txtBS.EditValue = NullToDbl(oDS.Tables(namaTabel).Rows(0).Item("BS").ToString)
                HitungJumlah()
                HitungKirim()

                txtKadar1.Text = NullTostr(oDS.Tables(namaTabel).Rows(0).Item("KodeKadar1").ToString)
                txtKadar2.Text = NullTostr(oDS.Tables(namaTabel).Rows(0).Item("KodeKadar2").ToString)
                txtKadar3.Text = NullTostr(oDS.Tables(namaTabel).Rows(0).Item("KodeKadar3").ToString)
                txtKadar4.Text = NullTostr(oDS.Tables(namaTabel).Rows(0).Item("KodeKadar4").ToString)
                txtKadar5.Text = NullTostr(oDS.Tables(namaTabel).Rows(0).Item("KodeKadar5").ToString)
                txtKadar6.Text = NullTostr(oDS.Tables(namaTabel).Rows(0).Item("KodeKadar6").ToString)
                txtKadar7.Text = NullTostr(oDS.Tables(namaTabel).Rows(0).Item("KodeKadar7").ToString)
                txtKadar8.Text = NullTostr(oDS.Tables(namaTabel).Rows(0).Item("KodeKadar8").ToString)
                txtKadar9.Text = NullTostr(oDS.Tables(namaTabel).Rows(0).Item("KodeKadar9").ToString)
                txtKadar10.Text = NullTostr(oDS.Tables(namaTabel).Rows(0).Item("KodeKadar10").ToString)

                IDKadar1 = NullToLong(oDS.Tables(namaTabel).Rows(0).Item("IDKadar1"))
                IDKadar2 = NullToLong(oDS.Tables(namaTabel).Rows(0).Item("IDKadar2"))
                IDKadar3 = NullToLong(oDS.Tables(namaTabel).Rows(0).Item("IDKadar3"))
                IDKadar4 = NullToLong(oDS.Tables(namaTabel).Rows(0).Item("IDKadar4"))
                IDKadar5 = NullToLong(oDS.Tables(namaTabel).Rows(0).Item("IDKadar5"))
                IDKadar6 = NullToLong(oDS.Tables(namaTabel).Rows(0).Item("IDKadar6"))
                IDKadar7 = NullToLong(oDS.Tables(namaTabel).Rows(0).Item("IDKadar7"))
                IDKadar8 = NullToLong(oDS.Tables(namaTabel).Rows(0).Item("IDKadar8"))
                IDKadar9 = NullToLong(oDS.Tables(namaTabel).Rows(0).Item("IDKadar9"))
                IDKadar10 = NullToLong(oDS.Tables(namaTabel).Rows(0).Item("IDKadar10"))

                txtJmlKadar1.EditValue = NullToDbl(oDS.Tables(namaTabel).Rows(0).Item("NilaiKadar1"))
                txtJmlKadar2.EditValue = NullToDbl(oDS.Tables(namaTabel).Rows(0).Item("NilaiKadar2"))
                txtJmlKadar3.EditValue = NullToDbl(oDS.Tables(namaTabel).Rows(0).Item("NilaiKadar3"))
                txtJmlKadar4.EditValue = NullToDbl(oDS.Tables(namaTabel).Rows(0).Item("NilaiKadar4"))
                txtJmlKadar5.EditValue = NullToDbl(oDS.Tables(namaTabel).Rows(0).Item("NilaiKadar5"))
                txtJmlKadar6.EditValue = NullToDbl(oDS.Tables(namaTabel).Rows(0).Item("NilaiKadar6"))
                txtJmlKadar7.EditValue = NullToDbl(oDS.Tables(namaTabel).Rows(0).Item("NilaiKadar7"))
                txtJmlKadar8.EditValue = NullToDbl(oDS.Tables(namaTabel).Rows(0).Item("NilaiKadar8"))
                txtJmlKadar9.EditValue = NullToDbl(oDS.Tables(namaTabel).Rows(0).Item("NilaiKadar9"))
                txtJmlKadar10.EditValue = NullToDbl(oDS.Tables(namaTabel).Rows(0).Item("NilaiKadar10"))


                txtClaim.EditValue = NullToDbl(oDS.Tables(namaTabel).Rows(0).Item("Claim"))
                txtPenyesuaianClaim.EditValue = NullToDbl(oDS.Tables(namaTabel).Rows(0).Item("PenyesuaianClaim"))
                txtTotalClaim.EditValue = NullToDbl(oDS.Tables(namaTabel).Rows(0).Item("TotalClaim"))

            End If
        Catch ex As Exception
            FxMessage(ex.Message, NamaApplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error, , , ex.StackTrace)
        Finally
            oDS.Dispose()
        End Try
    End Sub
    Private Sub GetDataSKB(ByVal NoID As Long)
        Dim SQL As String
        Dim oDS As New DataSet
        SQL = "SELECT " & IIf(VPOINT.Serialshield.Serial.IsTrial, " top 100 ", "") & "MDOD.*, MNotaTimbang.Kode AS KodeNotaTimbang, MWadah.Kode as KdWadah, MWadah.Nama as NmWadah, MGudang.Kode as KdGudang, MGudang.Nama as NmGudang, MBarang.Nama as NmBrg, MBarang.Satuan As SatBrg, MBarang.Kode As KdBrg,MBarang.IDSatuan AS IDSatuanParent , MSatuan.Nama As NamaSatuan" & vbCrLf
        SQL = SQL & " FROM ((((MDOD LEFT Join MBarang ON MBarang.ID=MDOD.IDBarang) " & vbCrLf
        SQL = SQL & " LEFT JOIN MGudang ON MGudang.ID=MDOD.IDGudang) "
        SQL = SQL & " LEFT JOIN MWadah ON MWadah.ID=MDOD.IDWadah) "
        SQL = SQL & " LEFT JOIN MSatuan ON MSatuan.NoID=MDOD.IDSatuan) "
        SQL = SQL & " LEFT JOIN MNotaTimbang ON MNotaTimbang.ID=MDOD.IDNotaTimbang "
        SQL = SQL & " WHERE MDOD.NoID = " & NoID
        Try
            oDS = MyConn.ExecuteDataset(namaTabel, SQL)
            If oDS.Tables(namaTabel).Rows.Count >= 1 Then
                IDBarang = NullToLong(oDS.Tables(namaTabel).Rows(0).Item("IDBarang").ToString)
                IDNotaTimbang = NullToLong(oDS.Tables(namaTabel).Rows(0).Item("IDNotaTimbang").ToString)
                IDGudang = NullToLong(oDS.Tables(namaTabel).Rows(0).Item("IDGudang").ToString)
                IDWadah = NullToLong(oDS.Tables(namaTabel).Rows(0).Item("IDWadah").ToString)
                txtGudang.Text = NullTostr(oDS.Tables(namaTabel).Rows(0).Item("KdGudang").ToString & " - " & oDS.Tables(namaTabel).Rows(0).Item("NmGudang").ToString)
                txtBarang.Text = NullTostr(oDS.Tables(namaTabel).Rows(0).Item("NmBrg").ToString) & " - " & NullTostr(oDS.Tables(namaTabel).Rows(0).Item("SatBrg"))
                txtBarang2.Text = txtBarang.Text
                txtBarang3.Text = txtBarang.Text
                IDAkunPersediaan = NullToLong(oDS.Tables(namaTabel).Rows(0).Item("IDAKunPersediaan").ToString)
                txtNotaTimbang.Text = NullTostr(oDS.Tables(namaTabel).Rows(0).Item("KodeNotaTimbang").ToString)
                txtWadah.Text = NullTostr(oDS.Tables(namaTabel).Rows(0).Item("NmWadah").ToString)
                txtCatatan.Text = NullTostr(oDS.Tables(namaTabel).Rows(0).Item("Catatan").ToString)
                IDSatuan = NullToLong(oDS.Tables(namaTabel).Rows(0).Item("IDSatuan").ToString)
                txtSatuan.Text = NullTostr(oDS.Tables(namaTabel).Rows(0).Item("NamaSatuan").ToString)
                IDSatuanParent = NullToLong(oDS.Tables(namaTabel).Rows(0).Item("IDSatuanParent").ToString)

                txtColly.EditValue = NullToDbl(oDS.Tables(namaTabel).Rows(0).Item("JmlWadah").ToString)
                txtBruto.EditValue = NullToDbl(oDS.Tables(namaTabel).Rows(0).Item("Bruto").ToString)
                txtTarra.EditValue = NullToDbl(oDS.Tables(namaTabel).Rows(0).Item("Tara").ToString)
                txtPotongan.EditValue = NullToDbl(oDS.Tables(namaTabel).Rows(0).Item("Potongan").ToString)
                txtBS.EditValue = NullToDbl(oDS.Tables(namaTabel).Rows(0).Item("BS").ToString)
                HitungJumlah()
                HitungKirim()

                txtKadar1.Text = NullTostr(oDS.Tables(namaTabel).Rows(0).Item("KodeKadar1").ToString)
                txtKadar2.Text = NullTostr(oDS.Tables(namaTabel).Rows(0).Item("KodeKadar2").ToString)
                txtKadar3.Text = NullTostr(oDS.Tables(namaTabel).Rows(0).Item("KodeKadar3").ToString)
                txtKadar4.Text = NullTostr(oDS.Tables(namaTabel).Rows(0).Item("KodeKadar4").ToString)
                txtKadar5.Text = NullTostr(oDS.Tables(namaTabel).Rows(0).Item("KodeKadar5").ToString)
                txtKadar6.Text = NullTostr(oDS.Tables(namaTabel).Rows(0).Item("KodeKadar6").ToString)
                txtKadar7.Text = NullTostr(oDS.Tables(namaTabel).Rows(0).Item("KodeKadar7").ToString)
                txtKadar8.Text = NullTostr(oDS.Tables(namaTabel).Rows(0).Item("KodeKadar8").ToString)
                txtKadar9.Text = NullTostr(oDS.Tables(namaTabel).Rows(0).Item("KodeKadar9").ToString)
                txtKadar10.Text = NullTostr(oDS.Tables(namaTabel).Rows(0).Item("KodeKadar10").ToString)

                IDKadar1 = NullToLong(oDS.Tables(namaTabel).Rows(0).Item("IDKadar1"))
                IDKadar2 = NullToLong(oDS.Tables(namaTabel).Rows(0).Item("IDKadar2"))
                IDKadar3 = NullToLong(oDS.Tables(namaTabel).Rows(0).Item("IDKadar3"))
                IDKadar4 = NullToLong(oDS.Tables(namaTabel).Rows(0).Item("IDKadar4"))
                IDKadar5 = NullToLong(oDS.Tables(namaTabel).Rows(0).Item("IDKadar5"))
                IDKadar6 = NullToLong(oDS.Tables(namaTabel).Rows(0).Item("IDKadar6"))
                IDKadar7 = NullToLong(oDS.Tables(namaTabel).Rows(0).Item("IDKadar7"))
                IDKadar8 = NullToLong(oDS.Tables(namaTabel).Rows(0).Item("IDKadar8"))
                IDKadar9 = NullToLong(oDS.Tables(namaTabel).Rows(0).Item("IDKadar9"))
                IDKadar10 = NullToLong(oDS.Tables(namaTabel).Rows(0).Item("IDKadar10"))

                txtJmlKadar1.EditValue = NullToDbl(oDS.Tables(namaTabel).Rows(0).Item("NilaiKadar1"))
                txtJmlKadar2.EditValue = NullToDbl(oDS.Tables(namaTabel).Rows(0).Item("NilaiKadar2"))
                txtJmlKadar3.EditValue = NullToDbl(oDS.Tables(namaTabel).Rows(0).Item("NilaiKadar3"))
                txtJmlKadar4.EditValue = NullToDbl(oDS.Tables(namaTabel).Rows(0).Item("NilaiKadar4"))
                txtJmlKadar5.EditValue = NullToDbl(oDS.Tables(namaTabel).Rows(0).Item("NilaiKadar5"))
                txtJmlKadar6.EditValue = NullToDbl(oDS.Tables(namaTabel).Rows(0).Item("NilaiKadar6"))
                txtJmlKadar7.EditValue = NullToDbl(oDS.Tables(namaTabel).Rows(0).Item("NilaiKadar7"))
                txtJmlKadar8.EditValue = NullToDbl(oDS.Tables(namaTabel).Rows(0).Item("NilaiKadar8"))
                txtJmlKadar9.EditValue = NullToDbl(oDS.Tables(namaTabel).Rows(0).Item("NilaiKadar9"))
                txtJmlKadar10.EditValue = NullToDbl(oDS.Tables(namaTabel).Rows(0).Item("NilaiKadar10"))


                txtClaim.EditValue = NullToDbl(oDS.Tables(namaTabel).Rows(0).Item("Claim"))
                txtPenyesuaianClaim.EditValue = NullToDbl(oDS.Tables(namaTabel).Rows(0).Item("PenyesuaianClaim"))
                txtTotalClaim.EditValue = NullToDbl(oDS.Tables(namaTabel).Rows(0).Item("TotalClaim"))

            End If
        Catch ex As Exception
            FxMessage(ex.Message, NamaApplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error, , , ex.StackTrace)
        Finally
            oDS.Dispose()
        End Try
    End Sub
    Sub IsiDefault()
        txtBarang.Text = ""
        txtBarang2.Text = txtBarang.Text
        txtBarang3.Text = txtBarang.Text
        IDBarang = -1
        txtnetto.EditValue = 0
        txtCatatan.Text = ""
    End Sub
    Private Sub cmdSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSave.Click
        Dim SQL As String, curentcursor As Windows.Forms.Cursor = Windows.Forms.Cursor.Current
        Dim dlg As DevExpress.Utils.WaitDialogForm = New DevExpress.Utils.WaitDialogForm("Menyimpan data." & vbCrLf & "Mohon tunggu beberapa saat ...", NamaApplikasi)
        Dim oDS As New DataSet
        dlg.TopMost = True
        dlg.Show()
        Windows.Forms.Cursor.Current = Windows.Forms.Cursors.WaitCursor
        Try
            If IsValidasi() Then
                If Not IsTester Then
                    If pStatus = ptipe.Baru Then
                        IDSaldoAwal = MyConn.GetNewID("MLPB", "ID")
                        SQL = "INSERT INTO MLPB (ID, Kode, IDUser, IDGudang, IDWilayah, Tanggal, Keterangan, IsPosted, IsSaldoAwal, IDSupplier) VALUES (" & vbCrLf
                        SQL &= IDSaldoAwal & ", '" & FixApostropi(Tgl.Text) & "'," & IDUserAktif & "," & IDGudang & "," & IDWilayah & ",'" & Format(txtTgl.DateTime, "yyyy/MM/dd") & "','" & FixApostropi(txtCatatan.Text) & "',0,1," & IDSupplier & ")"
                        MyConn.EksekusiSQl(SQL)

                        ID = MyConn.GetNewID(namaTabel, "ID", " WHERE IDUser=" & IDUserAktif & " AND IP='" & FixApostropi(IPLokal) & "'")
                        SQL = "Insert Into " & namaTabel & "(ID,IDBeli,IDUser,IP) VALUES (" & ID & "," & IDSaldoAwal & "," & IDUserAktif & ",'" & FixApostropi(IPLokal) & "')"
                        MyConn.EksekusiSQl(SQL)
                    Else
                        SQL = "UPDATE MLPB SET Kode='" & FixApostropi(Tgl.Text) & "', IDUser=" & IDUserAktif & ", IDGudang=" & IDGudang & ", IDWilayah=" & IDWilayah & ", Tanggal='" & Format(txtTgl.DateTime, "yyyy/MM/dd") & "', Keterangan='" & FixApostropi(txtCatatan.Text) & "', IDSupplier='" & IDSupplier & "', IsSaldoAwal=1"
                        SQL &= " WHERE ID=" & IDSaldoAwal
                        MyConn.EksekusiSQl(SQL)
                    End If
                    'Tambahan
                    HitungJumlah()
                    HitungKirim()
                    Dim HargaBeli As Double = NullToDbl(VPOINT.Databases.SQLServer.EksekusiSQlskalar("SELECT " & IIf(VPOINT.Serialshield.Serial.IsTrial, " top 100 ", "") & "HargaBeli from MBarang where ID=" & IDBarang))
                    SQL = "Update " & namaTabel & " Set " & vbCrLf & _
                          "issabelumdibeli=" & IIf(ckBelumDiBeli.Checked, 1, 0) & "," & _
                          "issabelumdiproses=" & IIf(ckBelumDiproses.Checked, 1, 0) & "," & _
                          "issamasukstok=" & IIf(ckMasukStok.Checked, 1, 0) & "," & _
                          "IDKadar1=" & IDKadar1 & "," & _
                          "KodeKadar1='" & FixApostropi(txtKadar1.Text) & "'," & _
                          "NilaiKadar1=" & FixKoma(txtJmlKadar1.EditValue) & "," & _
                          "IDKadar2=" & IDKadar2 & "," & _
                          "KodeKadar2='" & FixApostropi(txtKadar2.Text) & "'," & _
                          "NilaiKadar2=" & FixKoma(txtJmlKadar2.EditValue) & "," & _
                          "IDKadar3=" & IDKadar3 & "," & _
                          "KodeKadar3='" & FixApostropi(txtKadar3.Text) & "'," & _
                          "NilaiKadar3=" & FixKoma(txtJmlKadar3.EditValue) & "," & _
                          "IDKadar4=" & IDKadar4 & "," & _
                          "KodeKadar4='" & FixApostropi(txtKadar4.Text) & "'," & _
                          "NilaiKadar4=" & FixKoma(txtJmlKadar4.EditValue) & "," & _
                          "IDKadar5=" & IDKadar5 & "," & _
                          "KodeKadar5='" & FixApostropi(txtKadar5.Text) & "'," & _
                          "NilaiKadar5=" & FixKoma(txtJmlKadar5.EditValue) & "," & _
                          "IDKadar6=" & IDKadar6 & "," & _
                          "KodeKadar6='" & FixApostropi(txtKadar6.Text) & "'," & _
                          "NilaiKadar6=" & FixKoma(txtJmlKadar6.EditValue) & "," & _
                          "IDKadar7=" & IDKadar7 & "," & _
                          "KodeKadar7='" & FixApostropi(txtKadar7.Text) & "'," & _
                          "NilaiKadar7=" & FixKoma(txtJmlKadar7.EditValue) & "," & _
                          "IDKadar8=" & IDKadar8 & "," & _
                          "KodeKadar8='" & FixApostropi(txtKadar8.Text) & "'," & _
                          "NilaiKadar8=" & FixKoma(txtJmlKadar8.EditValue) & "," & _
                          "IDKadar9=" & IDKadar9 & "," & _
                          "KodeKadar9='" & FixApostropi(txtKadar9.Text) & "'," & _
                          "NilaiKadar9=" & FixKoma(txtJmlKadar9.EditValue) & "," & _
                          "IDKadar10=" & IDKadar10 & "," & _
                          "KodeKadar10='" & FixApostropi(txtKadar10.Text) & "'," & _
                          "NilaiKadar10=" & FixKoma(txtJmlKadar10.EditValue) & "," & _
                          "IDSTBD=" & IDSTBLain & "," & _
                          "IDSatuan=" & IDSatuan & "," & vbCrLf & _
                          "IDNotaTimbang=" & IDNotaTimbang & "," & vbCrLf & _
                          "IDGudang=" & IDGudang & "," & vbCrLf & _
                          "IDAkunPersediaan=" & IDAkunPersediaan & "," & vbCrLf & _
                          "IDDepartemen=" & defIDDepartemen & "," & vbCrLf & _
                          "IDUser=" & IDUserAktif & "," & vbCrLf & _
                          "IDBeli=" & IDSaldoAwal & "," & vbCrLf & _
                          "IDWadah=" & IDWadah & "," & vbCrLf & _
                          "JmlWadah=" & FixKoma(txtColly.EditValue) & "," & vbCrLf & _
                          "Bruto=" & FixKoma(txtBruto.EditValue) & "," & vbCrLf & _
                          "Tara=" & FixKoma(txtTarra.EditValue) & "," & vbCrLf & _
                          "Netto1=" & FixKoma(txtnetto.EditValue) & "," & vbCrLf & _
                          "Potongan=" & FixKoma(txtPotongan.EditValue) & "," & vbCrLf & _
                          "BS=" & FixKoma(txtBS.EditValue) & "," & vbCrLf & _
                          "Netto=" & FixKoma(txtNetto1.EditValue) & "," & vbCrLf & _
                          "IDBarang=" & IDBarang & "," & vbCrLf & _
                          "Claim=" & FixKoma(txtClaim.EditValue) & "," & vbCrLf & _
                          "PenyesuaianClaim=" & FixKoma(txtPenyesuaianClaim.EditValue) & "," & vbCrLf & _
                          "TotalClaim=" & FixKoma(txtTotalClaim.EditValue) & "," & vbCrLf & _
                          "IP='" & FixApostropi(IPLokal) & "'," & vbCrLf & _
                          "Catatan='" & FixApostropi(txtCatatan.Text) & "'" & _
                          " WHERE " & IIf(pStatus = ptipe.Baru, " ID=" & ID & " AND IDBeli=" & IDSaldoAwal & " AND IDUser=" & IDUserAktif & " AND IP='" & FixApostropi(IPLokal) & "'", " NoID=" & ID)
                    MyConn.EksekusiSQl(SQL)
                End If
                dlg.Close()
                dlg.Dispose()
                Windows.Forms.Cursor.Current = Cursors.Default
                DialogResult = Windows.Forms.DialogResult.OK
                Close()
            Else
                SQL = "UPDATE MLPB SET IsBelumDitester=1 "
                SQL &= " WHERE ID=" & IDSaldoAwal
                MyConn.EksekusiSQl(SQL)
            End If
        Catch ex As Exception
            pStatus = ptipe.Edit
        Finally
            oDS.Dispose()
        End Try
        dlg.Close()
        dlg.Dispose()
        Windows.Forms.Cursor.Current = Cursors.Default
    End Sub
    Private Function IsValidasi() As Boolean
        If txtBarang.Text = "" Then
            FxMessage("Nama Barang masih kosong.", NamaApplikasi)
            IsValidasi = False
            txtBarang.Focus()
            Exit Function
        End If
        If txtGudang.Text = "" Then
            FxMessage("Gudang masih kosong.", NamaApplikasi)
            IsValidasi = False
            txtGudang.Focus()
            Exit Function
        End If
        If Tgl.Text = "" Then
            FxMessage("Periode masih kosong.", NamaApplikasi)
            IsValidasi = False
            Tgl.Focus()
            Exit Function
        End If
        If txtTgl.Text = "" Then
            FxMessage("Tanggal masih kosong.", NamaApplikasi)
            IsValidasi = False
            txtTgl.Focus()
            Exit Function
        End If
        If txtWadah.Text = "" Then
            FxMessage("Jenis masih kosong.", NamaApplikasi)
            IsValidasi = False
            txtWadah.Focus()
            Exit Function
        End If
        If txtnetto.EditValue <= 0 Then
            FxMessage("Qty masih nol.", NamaApplikasi)
            IsValidasi = False
            txtnetto.Focus()
            Exit Function
        End If
        If MyConn.CekKodeValid(Tgl.Text, OldNota, "MLPB", "Kode", IIf(pStatus = ptipe.Edit, 1, 0), " AND (MLPB.IsSaldoAwal=1) ") Then
            FxMessage("Kode sudah dipakai.", NamaApplikasi)
            Return False
            Exit Function
        End If
        IsValidasi = True
    End Function

    Private Sub txtSubKlas_ButtonClick(ByVal sender As Object, ByVal e As DevExpress.XtraEditors.Controls.ButtonPressedEventArgs) Handles txtBarang.ButtonClick
        Select Case e.Button.Index
            Case 0
                Dim x As New frmLUBarang
                x.pStatus = ptipe.LookUp
                If x.ShowDialog = Windows.Forms.DialogResult.OK Then
                    IDBarang = x.NoID
                    IDAkunPersediaan = x.IDAkunPersediaan
                    txtBarang.Text = x.Nama
                    IDSatuanParent = x.IDSatuan
                    SetBarang(x.NoID)
                End If
                x.Dispose()
            Case 1
                IDBarang = -1
                IDSatuanParent = -1
                txtBarang.Text = ""
        End Select
        txtBarang2.Text = txtBarang.Text
        txtBarang3.Text = txtBarang.Text
    End Sub
    Public Function SetBarang(ByVal IDBrg As Long) As Boolean
        Dim oDS As New DataSet
        Dim SQL As String = "", SQL1 As String = ""
        'Dim NamaTabel As String = "MLPBD"
        Try
            SQL = "SELECT MBarang.*, MWadah.Nama as JenisBarang, MSatuan.IDParent, MSatuan.Nama as NamaSatuan FROM (MBarang " & vbCrLf
            SQL = SQL & " LEFT JOIN MSatuan ON MSatuan.NoID=MBarang.IDSatuan) " & vbCrLf
            SQL = SQL & " LEFT JOIN MWadah ON MWadah.ID=MBarang.IDJenis " & vbCrLf
            SQL = SQL & " WHERE MBarang.ID = " & IDBrg
            oDS = MyConn.ExecuteDataset(namaTabel, SQL)
            If oDS.Tables(0).Rows.Count >= 1 Then
                txtKadar2.Text = NullTostr(oDS.Tables(namaTabel).Rows(0).Item("KodeKadar1").ToString)
                txtKadar2.Text = NullTostr(oDS.Tables(namaTabel).Rows(0).Item("KodeKadar2").ToString)
                txtKadar3.Text = NullTostr(oDS.Tables(namaTabel).Rows(0).Item("KodeKadar3").ToString)
                txtKadar4.Text = NullTostr(oDS.Tables(namaTabel).Rows(0).Item("KodeKadar4").ToString)
                txtKadar5.Text = NullTostr(oDS.Tables(namaTabel).Rows(0).Item("KodeKadar5").ToString)
                txtKadar6.Text = NullTostr(oDS.Tables(namaTabel).Rows(0).Item("KodeKadar6").ToString)
                txtKadar7.Text = NullTostr(oDS.Tables(namaTabel).Rows(0).Item("KodeKadar7").ToString)
                txtKadar8.Text = NullTostr(oDS.Tables(namaTabel).Rows(0).Item("KodeKadar8").ToString)
                txtKadar9.Text = NullTostr(oDS.Tables(namaTabel).Rows(0).Item("KodeKadar9").ToString)
                txtKadar10.Text = NullTostr(oDS.Tables(namaTabel).Rows(0).Item("KodeKadar10").ToString)

                IDKadar1 = NullToLong(oDS.Tables(namaTabel).Rows(0).Item("IDKadar1").ToString)
                IDKadar2 = NullToLong(oDS.Tables(namaTabel).Rows(0).Item("IDKadar2").ToString)
                IDKadar3 = NullToLong(oDS.Tables(namaTabel).Rows(0).Item("IDKadar3").ToString)
                IDKadar4 = NullToLong(oDS.Tables(namaTabel).Rows(0).Item("IDKadar4").ToString)
                IDKadar5 = NullToLong(oDS.Tables(namaTabel).Rows(0).Item("IDKadar5").ToString)
                IDKadar6 = NullToLong(oDS.Tables(namaTabel).Rows(0).Item("IDKadar6").ToString)
                IDKadar7 = NullToLong(oDS.Tables(namaTabel).Rows(0).Item("IDKadar7").ToString)
                IDKadar8 = NullToLong(oDS.Tables(namaTabel).Rows(0).Item("IDKadar8").ToString)
                IDKadar9 = NullToLong(oDS.Tables(namaTabel).Rows(0).Item("IDKadar9").ToString)
                IDKadar10 = NullToLong(oDS.Tables(namaTabel).Rows(0).Item("IDKadar10").ToString)

                IDWadah = NullToLong(oDS.Tables(0).Rows(0).Item("IDJenis"))
                IDSatuan = NullToLong(oDS.Tables(0).Rows(0).Item("IDSatuan"))
                IDSatuanParent = NullToLong(oDS.Tables(0).Rows(0).Item("IDParent"))
                txtWadah.Text = NullTostr(oDS.Tables(0).Rows(0).Item("JenisBarang"))
                txtSatuan.Text = NullTostr(oDS.Tables(0).Rows(0).Item("NamaSatuan"))

             End If
        Catch ex As Exception
            FxMessage("Error : " & ex.Message, NamaApplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error, , , ex.StackTrace)
        Finally
            oDS.Dispose()
        End Try
    End Function

    Private Sub ButtonEdit2_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        HitungJumlah()
    End Sub

    Private Sub HitungJumlah()
        txtnetto.EditValue = txtBruto.EditValue - txtTarra.EditValue - txtPotongan.EditValue
        txtNetto1.EditValue = txtnetto.EditValue + txtBS.EditValue
        If txtKirimBruto.EditValue = 0 Then
            txtKirimBruto.EditValue = txtBruto.EditValue
        ElseIf txtKirimColly.EditValue = 0 Then
            txtKirimColly.EditValue = txtColly.EditValue
        ElseIf txtKirimTarra.EditValue = 0 Then
            txtKirimTarra.EditValue = txtTarra.EditValue
        ElseIf txtKirimPotongan.EditValue = 0 Then
            txtKirimPotongan.EditValue = txtPotongan.EditValue
        End If
        HitungKirim()
    End Sub

    Private Sub HitungKirim()
        txtKirimNetto.EditValue = txtKirimBruto.EditValue - txtKirimTarra.EditValue - txtKirimPotongan.EditValue
        txtKirimNetto1.EditValue = txtKirimNetto.EditValue + txtKirimBS.EditValue
    End Sub

    Private Sub txtTarra_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        HitungJumlah()
    End Sub

    Private Sub txtPotongan_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)

    End Sub

    Private Sub txtPotongan_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        HitungJumlah()
    End Sub

    Private Sub txtBantuSusut_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)

    End Sub

    Private Sub txtBantuSusut_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        HitungJumlah()
    End Sub

    Private Sub txtGudang_ButtonClick(ByVal sender As Object, ByVal e As DevExpress.XtraEditors.Controls.ButtonPressedEventArgs) Handles txtGudang.ButtonClick
        Select Case e.Button.Index
            Case 0
                Dim x As New frmLUGudang
                x.pStatus = ptipe.LookUp
                x.IDWilayah = frmEntriSTB.IDWilayah
                If x.ShowDialog = Windows.Forms.DialogResult.OK Then
                    IDGudang = x.NoID
                    txtGudang.Text = x.Kode & " - " & x.Nama
                    IDWilayah = x.IDWilayah
                End If
                x.Dispose()
            Case 1
                IDGudang = -1
                IDWilayah = -1
                txtGudang.Text = ""
        End Select
    End Sub

    Private Sub txtGudang_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtGudang.EditValueChanged

    End Sub

    Private Sub txtWadah_ButtonClick(ByVal sender As Object, ByVal e As DevExpress.XtraEditors.Controls.ButtonPressedEventArgs) Handles txtWadah.ButtonClick
        Select Case e.Button.Index
            Case 0
                Dim x As New frmLUJenis
                x.pStatus = ptipe.LookUp
                If x.ShowDialog = Windows.Forms.DialogResult.OK Then
                    IDWadah = x.NoID
                    txtWadah.Text = x.Kode & " - " & x.Nama
                End If
                x.Dispose()
            Case 1
                IDWadah = -1
                txtWadah.Text = ""
        End Select
    End Sub

    Private Sub txtWadah_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtWadah.EditValueChanged

    End Sub

    Private Sub txtBarang_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtBarang.EditValueChanged
        txtBarang2.Text = txtBarang.Text
        txtBarang3.Text = txtBarang.Text
    End Sub

    Private Sub txtKadar1_ButtonClick(ByVal sender As Object, ByVal e As DevExpress.XtraEditors.Controls.ButtonPressedEventArgs)
        Select Case e.Button.Index
            Case 0
                Dim x As New frmLUTester
                x.pStatus = ptipe.LookUp
                If x.ShowDialog = Windows.Forms.DialogResult.OK Then
                    IDKadar1 = x.NoID
                    txtKadar1.Text = x.Kode
                End If
                x.Dispose()
            Case 1
                IDKadar1 = -1
                txtKadar1.Text = ""
        End Select
        txtBarang2.Text = txtBarang.Text
        txtBarang3.Text = txtBarang.Text
    End Sub

    Private Sub txtKadar1_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)

    End Sub

    Private Sub txtKadar10_ButtonClick(ByVal sender As Object, ByVal e As DevExpress.XtraEditors.Controls.ButtonPressedEventArgs)
        Select Case e.Button.Index
            Case 0
                Dim x As New frmLUTester
                x.pStatus = ptipe.LookUp
                If x.ShowDialog = Windows.Forms.DialogResult.OK Then
                    IDKadar10 = x.NoID
                    txtKadar10.Text = x.Kode
                End If
                x.Dispose()
            Case 1
                IDKadar10 = -1
                txtKadar10.Text = ""
        End Select
    End Sub

    Private Sub txtKadar2_ButtonClick(ByVal sender As Object, ByVal e As DevExpress.XtraEditors.Controls.ButtonPressedEventArgs)
        Select Case e.Button.Index
            Case 0
                Dim x As New frmLUTester
                x.pStatus = ptipe.LookUp
                If x.ShowDialog = Windows.Forms.DialogResult.OK Then
                    IDKadar2 = x.NoID
                    txtKadar2.Text = x.Kode
                End If
                x.Dispose()
            Case 1
                IDKadar2 = -1
                txtKadar2.Text = ""
        End Select
    End Sub

    Private Sub txtKadar3_ButtonClick(ByVal sender As Object, ByVal e As DevExpress.XtraEditors.Controls.ButtonPressedEventArgs)
        Select Case e.Button.Index
            Case 0
                Dim x As New frmLUTester
                x.pStatus = ptipe.LookUp
                If x.ShowDialog = Windows.Forms.DialogResult.OK Then
                    IDKadar3 = x.NoID
                    txtKadar3.Text = x.Kode
                End If
                x.Dispose()
            Case 1
                IDKadar3 = -1
                txtKadar3.Text = ""
        End Select
    End Sub

    Private Sub txtKadar4_ButtonClick(ByVal sender As Object, ByVal e As DevExpress.XtraEditors.Controls.ButtonPressedEventArgs)
        Select Case e.Button.Index
            Case 0
                Dim x As New frmLUTester
                x.pStatus = ptipe.LookUp
                If x.ShowDialog = Windows.Forms.DialogResult.OK Then
                    IDKadar4 = x.NoID
                    txtKadar4.Text = x.Kode
                End If
                x.Dispose()
            Case 1
                IDKadar4 = -1
                txtKadar4.Text = ""
        End Select
    End Sub

    Private Sub txtKadar5_ButtonClick(ByVal sender As Object, ByVal e As DevExpress.XtraEditors.Controls.ButtonPressedEventArgs)
        Select Case e.Button.Index
            Case 0
                Dim x As New frmLUTester
                x.pStatus = ptipe.LookUp
                If x.ShowDialog = Windows.Forms.DialogResult.OK Then
                    IDKadar5 = x.NoID
                    txtKadar5.Text = x.Kode
                End If
                x.Dispose()
            Case 1
                IDKadar5 = -1
                txtKadar5.Text = ""
        End Select
    End Sub

    Private Sub txtKadar6_ButtonClick(ByVal sender As Object, ByVal e As DevExpress.XtraEditors.Controls.ButtonPressedEventArgs)
        Select Case e.Button.Index
            Case 0
                Dim x As New frmLUTester
                x.pStatus = ptipe.LookUp
                If x.ShowDialog = Windows.Forms.DialogResult.OK Then
                    IDKadar6 = x.NoID
                    txtKadar6.Text = x.Kode
                End If
                x.Dispose()
            Case 1
                IDKadar6 = -1
                txtKadar6.Text = ""
        End Select
    End Sub

    Private Sub txtKadar7_ButtonClick(ByVal sender As Object, ByVal e As DevExpress.XtraEditors.Controls.ButtonPressedEventArgs)
        Select Case e.Button.Index
            Case 0
                Dim x As New frmLUTester
                x.pStatus = ptipe.LookUp
                If x.ShowDialog = Windows.Forms.DialogResult.OK Then
                    IDKadar7 = x.NoID
                    txtKadar7.Text = x.Kode
                End If
                x.Dispose()
            Case 1
                IDKadar7 = -1
                txtKadar7.Text = ""
        End Select
    End Sub

    Private Sub txtKadar8_ButtonClick(ByVal sender As Object, ByVal e As DevExpress.XtraEditors.Controls.ButtonPressedEventArgs)
        Select Case e.Button.Index
            Case 0
                Dim x As New frmLUTester
                x.pStatus = ptipe.LookUp
                If x.ShowDialog = Windows.Forms.DialogResult.OK Then
                    IDKadar8 = x.NoID
                    txtKadar8.Text = x.Kode
                End If
                x.Dispose()
            Case 1
                IDKadar8 = -1
                txtKadar8.Text = ""
        End Select
    End Sub

    Private Sub txtKadar9_ButtonClick(ByVal sender As Object, ByVal e As DevExpress.XtraEditors.Controls.ButtonPressedEventArgs)
        Select Case e.Button.Index
            Case 0
                Dim x As New frmLUTester
                x.pStatus = ptipe.LookUp
                If x.ShowDialog = Windows.Forms.DialogResult.OK Then
                    IDKadar9 = x.NoID
                    txtKadar9.Text = x.Kode
                End If
                x.Dispose()
            Case 1
                IDKadar9 = -1
                txtKadar9.Text = ""
        End Select
    End Sub

    Private Sub txtKirimBruto_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)

    End Sub

    Private Sub txtKirimBruto_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        HitungKirim()
    End Sub

    Private Sub txtKirimBS_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        HitungKirim()
    End Sub

    Private Sub txtKirimPotongan_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        HitungKirim()
    End Sub

    Private Sub txtKirimTarra_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        HitungKirim()
    End Sub

    Private Sub txtBruto_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)

    End Sub

    Private Sub txtNotaTimbang_ButtonClick(ByVal sender As Object, ByVal e As DevExpress.XtraEditors.Controls.ButtonPressedEventArgs)
        Select Case e.Button.Index
            Case 0
                Dim x As New frmDaftarNotaTimbang
                x.pStatus = ptipe.LookUp
                x.IDGudang = IDGudang
                x.txtGudang.Text = txtGudang.Text
                x.IDSupplier = IDSupplier
                x.txtSupplier.Text = NamaSupplier
                If x.ShowDialog = Windows.Forms.DialogResult.OK Then
                    IDNotaTimbang = x.ID
                    txtNotaTimbang.Text = x.Kode
                    IDBarang = x.IDBarang
                    txtBarang.Text = x.NamaBarang & " - " & x.Satuan
                    txtBarang2.Text = txtBarang.Text
                    txtBarang3.Text = txtBarang.Text
                    txtBruto.EditValue = x.Bruto
                    txtColly.EditValue = x.Colly
                    txtTarra.EditValue = x.Tara
                    txtPotongan.EditValue = x.Potongan
                    txtBS.EditValue = x.BS
                    txtnetto.EditValue = x.Netto1
                    txtNetto1.EditValue = x.Netto
                    HitungJumlah()
                    SetBarang(IDBarang)
                End If
                x.Dispose()
            Case 1
                IDNotaTimbang = -1
                txtNotaTimbang.Text = ""
                SetBarang(IDBarang)
        End Select
    End Sub

    Private Sub txtSatuan_ButtonClick(ByVal sender As Object, ByVal e As DevExpress.XtraEditors.Controls.ButtonPressedEventArgs) Handles txtSatuan.ButtonClick
        Select Case e.Button.Index
            Case 0
                Dim x As New frmLUSatuan : x.IDParent = IDSatuanParent
                x.pStatus = ptipe.LookUp
                If x.ShowDialog = Windows.Forms.DialogResult.OK Then
                    IDSatuan = x.NoID
                    txtSatuan.Text = x.Nama
                End If
                x.Dispose()
            Case 1
                IDBarang = -1
                txtBarang.Text = ""
        End Select
        txtBarang2.Text = txtBarang.Text
        txtBarang3.Text = txtBarang.Text
    End Sub

    Private Sub txtSTB_ButtonClick(ByVal sender As Object, ByVal e As DevExpress.XtraEditors.Controls.ButtonPressedEventArgs)
        Select Case e.Button.Index
            Case 0
                If IDGudang >= 1 Then
                    Dim x As New frmLUSKBD : x.filter = " (MDO.IDGudang=" & IDGudang & " OR MDO.IDGudangTujuan=" & IDGudang & ")"
                    x.NoID = IDSTBLain
                    x.IDGudang = IDGudang
                    x.txtGudang.Text = txtGudang.Text
                    x.pStatus = ptipe.LookUp
                    If x.ShowDialog = Windows.Forms.DialogResult.OK Then
                        IDSTBLain = x.NoID
                        txtSTB.Text = x.Kode
                        GetDataSKB(IDSTBLain)
                    End If
                    x.Dispose()
                Else
                    FxMessage("Isi dahulu nama gudang tujuan.", , MessageBoxButtons.OK, MessageBoxIcon.Information)
                    txtGudang.Focus()
                End If
            Case 1
                IDSTBLain = -1
                txtSTB.Text = ""
        End Select
    End Sub

    Private Sub cmdRefresh_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim SQL As String = ""
        Try
            If FxMessage("Yakin ingin mengambil sesuai master tester?", NamaApplikasi, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) = Windows.Forms.DialogResult.Yes Then
                'MyConn.ExcecuteSQl("DELETE FROM MLPBDTester WHERE IDLPBD=" & ID)
                SQL = " INSERT INTO MLPBDTester"
                SQL = SQL & " (IDUser, IDLPBD, IDBeliD, IDTester, NamaTester, NilaiTester, Catatan, IDBarang,IP)"
                SQL = SQL & " SELECT " & IDUserAktif & " AS IDUser, " & ID & " AS IDLPBD, NULL AS IDBeliD, MTester.ID, MTester.Keterangan, 0 AS NilaiTester, '' AS Catatan, MTester.IDBarang,'" & FixApostropi(IPLokal) & "'"
                SQL = SQL & " FROM MTester LEFT OUTER JOIN"
                SQL = SQL & " MWilayah ON MTester.IDWilayah = MWilayah.ID"
                SQL = SQL & " WHERE MTester.IDParent IN (SELECT ID FROM MTester) AND MTester.IDBarang = " & IDBarang & " AND MTester.IDWilayah = " & IDWilayah
                SQL = SQL & " AND MTester.ID Not IN (SELECT IDTester FROM MLPBDTester WHERE IDLPBD=" & ID & ")"
                SQL = SQL & " ORDER BY MTester.NoUrut "
                MyConn.ExcecuteSQl(SQL)
                RefreshItem()
            End If
        Catch ex As Exception
            FxMessage(ex.Message, NamaApplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error, , , ex.StackTrace)
        End Try
    End Sub

    Private Sub txtSTB_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)

    End Sub

    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

    End Sub

    Private Sub txtNotaTimbang_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)

    End Sub

    Private Sub cmdTambah_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim frmEntri As New frmEntriSTBDTester
        Dim view As ColumnView = Nothing 'IIf(XtraTabControl1.SelectedTabPageIndex = 1, GridControl2.FocusedView(), GridControl2.FocusedView())
        Try
            Dim IDDetil As Long = -1
            frmEntri.IDWilayah = IDWilayah
            frmEntri.IDBarang = IDBarang
            frmEntri.txtBarang.Text = txtBarang.Text
            frmEntri.pStatus = ptipe.Baru
            frmEntri.ID = IDDetil
            frmEntri.IDLPBD = ID
            If frmEntri.ShowDialog = Windows.Forms.DialogResult.OK Then
                RefreshItem()
            End If
        Catch ex As Exception
            FxMessage("Kesalahan : " & ex.Message, ".:: Pesan Kesalahan ::.", MessageBoxButtons.OK, MessageBoxIcon.Error, , , ex.StackTrace)
        Finally
            frmEntri.Close()
            frmEntri.Dispose()
        End Try
    End Sub

    Private Sub cmdHapus_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim view As ColumnView = Nothing
        Dim row As System.Data.DataRow = Nothing
        view = GridControl1.FocusedView
        row = view.GetDataRow(GridView1.FocusedRowHandle)
        Dim NOid As Long = row("IDLPBD")
        If FxMessage("Yakin Mau Hapus data ini?", ".:: HAPUS ITEM TESTER STB ::.", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
            MyConn.EksekusiSQl("DELETE FROM MLPBDTester WHERE IDLPBD=" & NOid)
            RefreshItem()
        End If
    End Sub

    Private Sub cmdEdit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim frmEntri As New frmEntriSTBDTester
        Dim view As ColumnView = Nothing 'IIf(XtraTabControl1.SelectedTabPageIndex = 1, GridControl2.FocusedView(), GridControl2.FocusedView())
        Try
            Dim row As System.Data.DataRow = Nothing
            view = GridControl1.FocusedView
            row = view.GetDataRow(GridView1.FocusedRowHandle)
            Dim IDDetil As Long = row("NoID")
            frmEntri.IDWilayah = IDWilayah
            frmEntri.IDBarang = IDBarang
            frmEntri.txtBarang.Text = txtBarang.Text
            frmEntri.pStatus = ptipe.Edit
            frmEntri.ID = IDDetil
            frmEntri.IDLPBD = ID
            If frmEntri.ShowDialog = Windows.Forms.DialogResult.OK Then
                RefreshItem()
            End If
        Catch ex As Exception
            FxMessage("Kesalahan : " & ex.Message, ".:: Pesan Kesalahan ::.", MessageBoxButtons.OK, MessageBoxIcon.Error, , , ex.StackTrace)
        Finally
            frmEntri.Close()
            frmEntri.Dispose()
        End Try

    End Sub

    Private Sub GridView1_CellValueChanged(ByVal sender As Object, ByVal e As DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs)
        If e.Column.FieldName = "NilaiTester" Then
            MyConn.ExcecuteSQl("Update MLPBDTester SET NilaiTester=" & FixKoma(e.Value) & " WHERE NoID=" & GridView1.GetRowCellValue(e.RowHandle, "NoID"))
        ElseIf e.Column.FieldName = "Catatan" Then
            MyConn.ExcecuteSQl("Update MLPBDTester SET Catatan='" & FixApostropi(e.Value) & "' WHERE NoID=" & GridView1.GetRowCellValue(e.RowHandle, "NoID"))
        End If
    End Sub

    Private Sub GridView1_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs)
        Try
            If e.KeyCode = Keys.Enter AndAlso GridView1.FocusedRowHandle <= GridView1.RowCount Then
                GridView1.FocusedRowHandle = GridView1.FocusedRowHandle + 1
            End If
        Catch ex As Exception

        End Try
    End Sub

    Private Sub txtSupplier_ButtonClick(ByVal sender As Object, ByVal e As DevExpress.XtraEditors.Controls.ButtonPressedEventArgs) Handles txtSupplier.ButtonClick
        Select Case e.Button.Index
            Case 0
                Dim x As New frmLUAlamat
                x.DaftarAlamat = frmLUAlamat.DaftarAlamat_.Supplier
                x.pStatus = ptipe.LookUp
                If x.ShowDialog = Windows.Forms.DialogResult.OK Then
                    IDSupplier = x.NoID
                    txtSupplier.Text = x.Nama '& " - " & x.Kontak
                End If
                x.Dispose()
            Case 1
                IDSupplier = -1
                txtSupplier.Text = ""
        End Select
    End Sub

    Private Sub txtSupplier_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtSupplier.EditValueChanged

    End Sub

    Private Sub txtBarang3_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)

    End Sub

    Private Sub GridControl1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)

    End Sub
End Class