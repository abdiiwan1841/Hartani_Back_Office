Imports DevExpress.XtraEditors
Imports DevExpress.Utils
Imports VPoint.Ini
'Imports AxCrystal
'Imports AxCrystal.AxCrystalReport

Public Class mdlCetakCR
    Dim mySection As CrystalDecisions.CrystalReports.Engine.Section
    Dim mySections As CrystalDecisions.CrystalReports.Engine.Sections
    Dim ConInfo As New CrystalDecisions.Shared.TableLogOnInfo
    Public cRpt As New CrystalDecisions.CrystalReports.Engine.ReportDocument
    'call variabel
    Dim obj_RepDoc As New CrystalDecisions.CrystalReports.Engine.ReportDocument
    Dim Obj_ConInfo As New CrystalDecisions.Shared.ConnectionInfo
    Dim Obj_TabLog As New CrystalDecisions.Shared.TableLogOnInfo
    Public Shared EditReport As Boolean = False
    Public Shared LangsungCetak As Boolean = False
    'Private Shared AxCrViewer As AxCrystalReport

    Public Enum action_
        Edit = 0
        Preview = 1
        Print = 2
    End Enum

    'Untuk Perintah Cetaknya
    'Private Sub BarButtonItem10_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem10.ItemClick
    '    If EditRpt = True Then
    '        CetakBukti(action_.Edit, True)
    '    Else
    '        CetakBukti(action_.Preview, True)
    '    End If
    'End Sub
    'Private Sub CetakBukti(ByVal action As action_, ByVal iskas As Boolean)
    '    Dim namafile As String
    '    Dim view As ColumnView = GridControl1.FocusedView
    '    Dim row As System.Data.DataRow = view.GetDataRow(GridView1.FocusedRowHandle)
    '    Try
    '        namafile = Application.StartupPath & "\report\" & IIf(iskas, "KasIN.rpt", "BankIN.rpt")
    '        NoID = NullToLong(row("ID"))
    '        CetakCRViewer(action, Me.MdiParent, namafile, "Cetak Bukti Kas Masuk", , "ID", NoID)
    '    Catch EX As Exception
    '        FxMessage(EX.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error, , , EX.StackTrace)
    '    End Try
    'End Sub
    Public Shared Sub CetakCrystalReport(ByVal form As XtraForm, ByVal Title As String, ByVal FileName As String, ByVal SelectionFormula As String, ByVal FormulaField As String)
        Try
            If EditReport Then
                ViewReport(form, action_.Edit, FileName, Title, SelectionFormula, , FormulaField)
            Else
                ViewReport(form, action_.Preview, FileName, Title, SelectionFormula, , FormulaField)
            End If
        Catch ex As Exception
            XtraMessageBox.Show("Info Kesalahan : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
        End Try
    End Sub
    Public Shared Function ViewReport(ByVal frmParent As XtraForm, ByVal Action As action_, ByVal sReportName As String, ByVal Judul As String, Optional ByVal sSelectionFormula As String = "", Optional ByVal param As String = "", Optional ByVal Formula As String = "", Optional ByVal SortOrder As String = "") As Boolean
        Dim intCounter As Integer
        Dim intCounter1 As Integer
        Dim objReport As New CrystalDecisions.CrystalReports.Engine.ReportDocument
        Dim ConInfo As New CrystalDecisions.Shared.TableLogOnInfo

        Dim paraValue As New CrystalDecisions.Shared.ParameterDiscreteValue
        Dim currValue As CrystalDecisions.Shared.ParameterValues
        Dim mySubReportObject As CrystalDecisions.CrystalReports.Engine.SubreportObject
        Dim mySubRepDoc As New CrystalDecisions.CrystalReports.Engine.ReportDocument
        Dim FieldDef As CrystalDecisions.CrystalReports.Engine.FieldDefinition

        Dim strParamenters As String = param
        Dim strParValPair() As String
        Dim strVal() As String
        Dim sFileName As String = ""

        Dim sFormulaName() As String
        Dim sFormulaValues() As String
        'Dim index As Integer=
        Dim dlg As WaitDialogForm
        Dim frmctk As frmCetakMDI = Nothing
        dlg = New WaitDialogForm("Sedang diproses...", "Mohon Tunggu Sebentar.")
        dlg.Show()
        Try

            sFileName = sReportName 'DownloadReport(sReportName, m_strReportDir)
            If Action = action_.Edit Then
                dlg.Close()
                dlg.Dispose()
                BukaFile(sFileName)
                Exit Try
            End If
            objReport.Load(sFileName)

            intCounter = objReport.DataDefinition.ParameterFields.Count
            If intCounter = 1 Then
                If InStr(objReport.DataDefinition.ParameterFields(0).ParameterFieldName, ".", CompareMethod.Text) > 0 Then
                    intCounter = 0
                End If
            End If

            If intCounter > 0 And Trim(param) <> "" Then
                strParValPair = strParamenters.Split("&")
                For index = 0 To UBound(strParValPair)
                    If InStr(strParValPair(index), "=") > 0 Then
                        strVal = strParValPair(index).Split("=")
                        paraValue.Value = strVal(1)
                        currValue = objReport.DataDefinition.ParameterFields(strVal(0)).CurrentValues
                        currValue.Add(paraValue)
                        objReport.DataDefinition.ParameterFields(strVal(0)).ApplyCurrentValues(currValue)
                    End If
                Next
            End If

            'ConInfo.ConnectionInfo.UserID = BacaIni("dbconfig", "Username", "sa")
            'ConInfo.ConnectionInfo.Password = BacaIni("dbconfig", "Password", "sahaysstem")
            'ConInfo.ConnectionInfo.ServerName = BacaIni("dbconfig", "Server", "CityToys")
            'ConInfo.ConnectionInfo.DatabaseName = BacaIni("dbconfig", "Database", "DBCityToys")

            ConInfo.ConnectionInfo.UserID = BacaIni("odbcconfig", "Username", "sa")
            ConInfo.ConnectionInfo.Password = BacaIni("odbcconfig", "Password", "sahaysstem")
            ConInfo.ConnectionInfo.ServerName = BacaIni("odbcconfig", "Server", "CityToys")
            ConInfo.ConnectionInfo.DatabaseName = BacaIni("odbcconfig", "Database", "DBCityToys")
            ConInfo.ConnectionInfo.AllowCustomConnection = True

            For intCounter = 0 To objReport.Database.Tables.Count - 1
                objReport.Database.Tables(intCounter).ApplyLogOnInfo(ConInfo)
            Next

            For index As Integer = 0 To objReport.ReportDefinition.Sections.Count - 1
                For intCounter = 0 To objReport.ReportDefinition.Sections(index).ReportObjects.Count - 1
                    With objReport.ReportDefinition.Sections(index)
                        If .ReportObjects(intCounter).Kind = CrystalDecisions.Shared.ReportObjectKind.SubreportObject Then
                            mySubReportObject = CType(.ReportObjects(intCounter), CrystalDecisions.CrystalReports.Engine.SubreportObject)
                            mySubRepDoc = mySubReportObject.OpenSubreport(mySubReportObject.SubreportName)
                            For intCounter1 = 0 To mySubRepDoc.Database.Tables.Count - 1
                                mySubRepDoc.Database.Tables(intCounter1).ApplyLogOnInfo(ConInfo)
                            Next
                        End If
                    End With
                Next
            Next
            If sSelectionFormula.Length > 0 Then
                objReport.RecordSelectionFormula = sSelectionFormula
            End If

            For xx As Integer = 0 To objReport.DataDefinition.FormulaFields.Count - 1
                If objReport.DataDefinition.FormulaFields(xx).Name = "NamaPerusahaan" Then
                    objReport.DataDefinition.FormulaFields("NamaPerusahaan").Text = "'" & NamaPerusahaan.ToUpper.Replace(Chr(13), "'+ CHR(13) +'").Replace(Chr(10), "'+ CHR(10) +'") & "'"
                ElseIf objReport.DataDefinition.FormulaFields(xx).Name = "AlamatPerusahaan" Then
                    objReport.DataDefinition.FormulaFields("AlamatPerusahaan").Text = "'" & AlamatPerusahaan.ToUpper.Replace(Chr(13), "'+ CHR(13) +'").Replace(Chr(10), "'+ CHR(10) +'") & "'"
                ElseIf objReport.DataDefinition.FormulaFields(xx).Name = "KotaPerusahaan" Then
                    objReport.DataDefinition.FormulaFields("KotaPerusahaan").Text = "'" & "Surabaya".ToUpper.Replace(Chr(13), "'+ CHR(13) +'").Replace(Chr(10), "'+ CHR(10) +'") & "'"
                End If
            Next

            If Formula.ToString.Length >= 1 Then
                sFormulaName = Formula.ToString.Split("&")
                For i As Integer = 0 To sFormulaName.Length - 1
                    sFormulaValues = sFormulaName(i).ToString.Split("=")
                    objReport.DataDefinition.FormulaFields(sFormulaValues(0).ToString).Text = sFormulaValues(1).ToString
                Next
            End If
            Dim strDB As String()
            If SortOrder.ToString.Length >= 1 Then
                strParValPair = SortOrder.Split("&")
                For index As Integer = 0 To UBound(strParValPair)
                    If InStr(strParValPair(index), "=") > 0 Then
                        strVal = strParValPair(index).Split("=")
                        strDB = strVal(0).Split(".")
                        FieldDef = objReport.Database.Tables(strDB(0)).Fields(strDB(1))
                        objReport.DataDefinition.SortFields.Item(index).Field = FieldDef
                        If strVal(1).ToString.ToUpper = "Descending".ToUpper Then
                            objReport.DataDefinition.SortFields(index).SortDirection = CrystalDecisions.Shared.SortDirection.DescendingOrder
                        Else
                            objReport.DataDefinition.SortFields(index).SortDirection = CrystalDecisions.Shared.SortDirection.AscendingOrder
                        End If
                    End If
                Next
            End If
            'If sSelectionFormula.Length > 0 Then
            '    o = sSelectionFormula
            'End If
            Application.DoEvents()

            ''KODE DIBAWAH UNTUK MEMBUKA FORM HANYA 1
            'Dim F As Object
            'For Each F In frmParent.MdiChildren
            '    If TypeOf F Is frmCetakMDI Then
            '        frmctk = CType(F, frmCetakMDI)
            '        Exit For
            '    End If
            'Next
            'If frmctk Is Nothing Then
            '    frmctk = New frmCetakMDI
            '    frmctk.MdiParent = frmParent
            'End If
            'frmctk.Text = Judul
            'frmctk.CRViewer.ReportSource = Nothing
            'frmctk.CRViewer.ReportSource = objReport
            'frmctk.CRViewer.RefreshReport()
            'frmctk.Show()
            'If Action = action_.Preview Then
            '    frmctk.CRViewer.Show()
            'Else
            '    frmctk.CRViewer.PrintReport()
            'End If
            'frmctk.Focus()

            'BANYAK FORMS
            If frmctk Is Nothing Then
                frmctk = New frmCetakMDI
                frmctk.MdiParent = frmParent
            End If
            frmctk.Text = Judul
            frmctk.CrViewer.ReportSource = Nothing
            frmctk.CrViewer.ReportSource = objReport
            frmctk.CrViewer.RefreshReport()
            frmctk.Show()
            If Action = action_.Preview Then
                frmctk.CrViewer.Show()
            Else
                frmctk.CrViewer.PrintReport()
            End If
            frmctk.Focus()

            Return True
        Catch ex As System.Exception
            XtraMessageBox.Show("Kesalahan : " & ex.Message & " file " & sFileName.ToString)
        Finally
            dlg.Close()
            dlg.Dispose()
        End Try
    End Function
    Public Shared Function PrintReport(ByVal Action As action_, ByVal sReportName As String, ByVal Judul As String, Optional ByVal sSelectionFormula As String = "", Optional ByVal param As String = "", Optional ByVal Formula As String = "") As Boolean
        Dim intCounter As Integer
        Dim intCounter1 As Integer
        Dim objReport As New CrystalDecisions.CrystalReports.Engine.ReportDocument
        Dim ConInfo As New CrystalDecisions.Shared.TableLogOnInfo

        Dim paraValue As New CrystalDecisions.Shared.ParameterDiscreteValue
        Dim currValue As CrystalDecisions.Shared.ParameterValues
        Dim mySubReportObject As CrystalDecisions.CrystalReports.Engine.SubreportObject
        Dim mySubRepDoc As New CrystalDecisions.CrystalReports.Engine.ReportDocument

        Dim strParamenters As String = param
        Dim strParValPair() As String
        Dim strVal() As String
        Dim sFileName As String = ""

        Dim sFormulaName() As String
        Dim sFormulaValues() As String
        'Dim index As Integer=
        Dim dlg As WaitDialogForm
        Dim frmctk As frmCetakMDI = Nothing
        dlg = New WaitDialogForm("Sedang diproses...", "Mohon Tunggu Sebentar.")
        dlg.Show()
        Try
            sFileName = sReportName 'DownloadReport(sReportName, m_strReportDir)
            If Action = action_.Edit Then
                dlg.Close()
                dlg.Dispose()
                BukaFile(sFileName)
                Exit Try
            End If
            objReport.Load(sFileName)

            intCounter = objReport.DataDefinition.ParameterFields.Count
            If intCounter = 1 Then
                If InStr(objReport.DataDefinition.ParameterFields(0).ParameterFieldName, ".", CompareMethod.Text) > 0 Then
                    intCounter = 0
                End If
            End If

            If intCounter > 0 And Trim(param) <> "" Then
                strParValPair = strParamenters.Split("&")
                For index = 0 To UBound(strParValPair)
                    If InStr(strParValPair(index), "=") > 0 Then
                        strVal = strParValPair(index).Split("=")
                        paraValue.Value = strVal(1)
                        currValue = objReport.DataDefinition.ParameterFields(strVal(0)).CurrentValues
                        currValue.Add(paraValue)
                        objReport.DataDefinition.ParameterFields(strVal(0)).ApplyCurrentValues(currValue)
                    End If
                Next
            End If

            'ConInfo.ConnectionInfo.UserID = BacaIni("dbconfig", "Username", "sa")
            'ConInfo.ConnectionInfo.Password = BacaIni("dbconfig", "Password", "sahaysstem")
            'ConInfo.ConnectionInfo.ServerName = BacaIni("dbconfig", "Server", "CityToys")
            'ConInfo.ConnectionInfo.DatabaseName = BacaIni("dbconfig", "Database", "DBCityToys")

            ConInfo.ConnectionInfo.UserID = BacaIni("odbcconfig", "Username", "sa")
            ConInfo.ConnectionInfo.Password = BacaIni("odbcconfig", "Password", "sahaysstem")
            ConInfo.ConnectionInfo.ServerName = BacaIni("odbcconfig", "Server", "CityToys")
            ConInfo.ConnectionInfo.DatabaseName = BacaIni("odbcconfig", "Database", "DBCityToys")
            ConInfo.ConnectionInfo.AllowCustomConnection = True

            For intCounter = 0 To objReport.Database.Tables.Count - 1
                objReport.Database.Tables(intCounter).ApplyLogOnInfo(ConInfo)
            Next

            For index As Integer = 0 To objReport.ReportDefinition.Sections.Count - 1
                For intCounter = 0 To objReport.ReportDefinition.Sections(index).ReportObjects.Count - 1
                    With objReport.ReportDefinition.Sections(index)
                        If .ReportObjects(intCounter).Kind = CrystalDecisions.Shared.ReportObjectKind.SubreportObject Then
                            mySubReportObject = CType(.ReportObjects(intCounter), CrystalDecisions.CrystalReports.Engine.SubreportObject)
                            mySubRepDoc = mySubReportObject.OpenSubreport(mySubReportObject.SubreportName)
                            For intCounter1 = 0 To mySubRepDoc.Database.Tables.Count - 1
                                mySubRepDoc.Database.Tables(intCounter1).ApplyLogOnInfo(ConInfo)
                            Next
                        End If
                    End With
                Next
            Next
            If sSelectionFormula.Length > 0 Then
                objReport.RecordSelectionFormula = sSelectionFormula
            End If
            If Formula.ToString.Length >= 1 Then
                sFormulaName = Formula.ToString.Split("&")
                For i As Integer = 0 To sFormulaName.Length - 1
                    sFormulaValues = sFormulaName(i).ToString.Split("=")
                    objReport.DataDefinition.FormulaFields(sFormulaValues(0).ToString).Text = sFormulaValues(1).ToString
                Next
            End If

            Application.DoEvents()
            objReport.PrintToPrinter(1, True, 0, 0)
            Return True
        Catch ex As System.Exception
            XtraMessageBox.Show("Kesalahan : " & ex.Message & " file " & sFileName.ToString)
        Finally
            dlg.Close()
            dlg.Dispose()
        End Try
    End Function
    'Public Shared Function ViewReport(ByVal frmParent As XtraForm, ByVal Action As action_, ByVal sReportName As String, ByVal Judul As String, Optional ByVal sSelectionFormula As String = "", Optional ByVal param As String = "") As Boolean
    '    Dim sFileName As String = ""
    '    Dim dlg As WaitDialogForm
    '    dlg = New WaitDialogForm("Sedang diproses...", "Mohon Tunggu Sebentar.")
    '    dlg.Show()
    '    Try
    '        sFileName = sReportName
    '        If Action = action_.Edit Then
    '            BukaFile(sFileName)
    '            Exit Function
    '        End If
    '        AxCrViewer = New AxCrystalReport
    '        AxCrViewer.Reset()
    '        AxCrViewer.ReportFileName = sFileName
    '        AxCrViewer.DiscardSavedData = True
    '        AxCrViewer.RetrieveDataFiles()
    '        AxCrViewer.Connect = "DSN=" & BacaIni("odbcconfig", "Server", "CityToys") & ";UID =" & BacaIni("odbcconfig", "Username", "sa") & ";PWD =" & BacaIni("odbcconfig", "Password", "sahaysstem") & ";DSQ=" & BacaIni("odbcconfig", "Database", "DBCityToys")
    '        AxCrViewer.Parent = frmParent
    '        'AxCrystalReport1.set_Formulas(0, "NOCHASIS='" & NOCHASIS & "'")
    '        'AxCrViewer.set_Formulas(0, "NoID=" & NoID & "")
    '        'AxCrViewer.set_Formulas(1, "IDPROSES=" & IDPROSES & "")
    '        AxCrViewer.ReplaceSelectionFormula(sSelectionFormula)
    '        'MsgBox(NamaFileCetak)

    '        'AxCrystalReport1.WindowParentHandle = Me.ParentForm.MdiChildren
    '        AxCrViewer.WindowShowCancelBtn = True
    '        AxCrViewer.WindowShowSearchBtn = True
    '        AxCrViewer.WindowShowPrintBtn = True
    '        AxCrViewer.WindowShowPrintSetupBtn = True

    '        AxCrViewer.WindowShowRefreshBtn = True
    '        AxCrViewer.Show()
    '        'AxCrViewer.WindowState()
    '        'If Action = action_.Preview Then
    '        '    AxCrViewer.Destination = Crystal.DestinationConstants.crptToWindow
    '        'Else
    '        '    AxCrViewer.Destination = Crystal.DestinationConstants.crptToPrinter
    '        'End If
    '        AxCrViewer.Action = 1
    '        AxCrViewer.PageZoom(100)
    '    Catch ex As System.Exception
    '        XtraMessageBox.Show("Kesalahan : " & ex.Message & " file " & sFileName.ToString)
    '    Finally
    '        dlg.Close()
    '        dlg.Dispose()
    '    End Try
    'End Function

    'Private Sub CetakPullMode()
    '    obj_RepDoc = New CrystalReport1
    '    Obj_TabLog = obj_RepDoc.Database.Tables(0).LogOnInfo
    '    With Obj_ConInfo
    '        .ServerName = BacaIni("dbconfig", "Server", "(local)")
    '        .UserID = BacaIni("dbconfig", "username", "sa")
    '        .Password = BacaIni("dbconfig", "Password", "sahasystem")
    '        .DatabaseName = BacaIni("dbconfig", "Database", "DBCITYCOYS")
    '    End With
    '    Obj_TabLog.ConnectionInfo = Obj_ConInfo
    '    obj_RepDoc.Database.Tables(0).ApplyLogOnInfo(Obj_TabLog)
    '    'CrystalReportViewer1.ReportSource = obj_RepDoc
    'End Sub
    'Public Sub CetakCRViewer(ByVal Action As action_, ByVal frmParent As XtraForm, ByVal namafilerpt As String, Optional ByVal Judul As String = "Print Cetak", Optional ByVal SelectionFormula As String = "", Optional ByVal ParameterField As String = "", Optional ByVal ValueParameterField As Object = Nothing)
    '    Dim dlg As WaitDialogForm
    '    Dim frmctk As frmCetakMDI = Nothing
    '    dlg = New WaitDialogForm("Sedang diproses...", "Mohon Tunggu Sebentar.")
    '    dlg.Show()
    '    Try
    '        dlg.Owner = frmParent
    '        dlg.TopMost = True
    '        If Action = action_.Edit Then
    '            dlg.Close()
    '            dlg.Dispose()
    '            BukaFile(namafilerpt)
    '            Exit Try
    '        End If
    '        If cRpt.IsLoaded Then
    '            cRpt.Dispose()
    '        End If
    '        'ConInfo.ConnectionInfo.UserID = ""
    '        cRpt = New CrystalDecisions.CrystalReports.Engine.ReportDocument
    '        cRpt.Load(namafilerpt, CrystalDecisions.Shared.OpenReportMethod.OpenReportByDefault)
    '        cRpt.SetDatabaseLogon(BacaIni("dbconfig", "Username", "sa"), BacaIni("dbconfig", "Password", "sgi"), BacaIni("dbconfig", "Server", "."), BacaIni("dbconfig", "Database", "DBCityToys"), True)
    '        'cRpt.VerifyDatabase()
    '        If SelectionFormula <> "" Then
    '            cRpt.RecordSelectionFormula = SelectionFormula
    '        End If
    '        If ParameterField <> "" Then
    '            cRpt.SetParameterValue(ParameterField, ValueParameterField)
    '        End If

    '        'KODE DIBAWAH UNTUK MEMBUKA FORM HANYA 1
    '        Dim F As Object
    '        For Each F In frmParent.MdiChildren
    '            If TypeOf F Is frmCetakMDI Then
    '                frmctk = CType(F, frmCetakMDI)
    '                Exit For
    '            End If
    '        Next
    '        If frmctk Is Nothing Then
    '            frmctk = New frmCetakMDI
    '            frmctk.MdiParent = frmParent
    '        End If
    '        frmctk.Text = Judul
    '        frmctk.CRViewer.ReportSource = cRpt
    '        'frmctk.CRViewer.RefreshReport()
    '        frmctk.Show()
    '        dlg.Close()
    '        dlg.Dispose()
    '        If Action = action_.Preview Then
    '            frmctk.CRViewer.Show()
    '        Else
    '            frmctk.CRViewer.PrintReport()
    '        End If
    '        frmctk.Focus()
    '    Catch EX As Exception
    '        XtraMessageBox.Show("Ada kesalahan cetak : " & EX.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Error)
    '    Finally
    '        dlg.Close()
    '        dlg.Dispose()
    '    End Try
    'End Sub
    Public Shared Sub InsertTemporary(ByVal IsHutang As Boolean, ByVal IDTransaksi As Long, ByVal IDAlamat As Long)
        Try
            EksekusiSQL("Delete from TBayarHutangD Where IDBayarHutang=" & IDTransaksi & " AND IsJual=" & IIf(IsHutang, 0, 1) & " and IDUser=" & IDUserAktif)
            If IsHutang Then
                'EksekusiSQL("insert Into TBayarHutangD(Isjual,IDBayarHutang,IDBeli,IDUser,Tanggal,JatuhTempo,Kode,KodeReff," & _
                '            "Jumlah,Terbayar,Sisa,Bayar,Retur,Denda) " & _
                '            "Select " & IIf(IsHutang, 0, 1) & " as IsJual, " & IDTransaksi & ",MBeli.NoID," & IDUserAktif & " As IDUser,Tanggal,JatuhTempo,Kode,KodeReff," & _
                '            "Total,case when B.Bayar IS NULL then 0 else B.Bayar END,Total-(case when B.Bayar IS NULL then 0 else B.Bayar END) as Sisa,case when C.Bayar IS NULL then 0 else C.Bayar END,Case When C.Retur IS NULL then 0 else C.Retur END,case when C.Denda IS NULL then 0 else C.Denda END " & _
                '            "FROM (MBeli Left Join (Select IDBeli,Sum(Bayar+Retur) as Bayar FROM MBayarHutangD where IsJual=0 and MBayarHutangD.IDBayarHutang<>" & IDTransaksi & " group by IDBeli) B on MBeli.NoID=B.IDBeli) " & _
                '            "LEFT JOIN (select MBayarHutangD.IDBeli,MBayarHutangD.Bayar,MBayarHutangd.Retur,MBayarHutangD.Denda from MBayarHutangD where MBayarHutangD.IDBayarHutang=" & IDTransaksi & ") C on MBeli.NoID=C.IDBeli " & _
                '            "WHERE MBeli.isPosted=1 and  Total-(case when B.Bayar IS NULL then 0 else B.Bayar END)<>0 and MBeli.IDSupplier=" & IDAlamat)
                EksekusiSQL("insert Into TBayarHutangD(Isjual,IDBayarHutang,IDBeli,IDUser,Tanggal,JatuhTempo,Kode,KodeReff," & _
                     "Jumlah,Terbayar,PotongRetur,Sisa,Bayar,Retur,Denda) " & _
                     "Select 0 as IsJual, " & IDTransaksi & ",MBeli.NoID," & IDUserAktif & " As IDUser,Tanggal,JatuhTempo,Kode,KodeReff," & _
                     "Total,IsNull(B.Bayar,0),IsNull(RB.NilaiRetur,0),Total-IsNull( B.Bayar,0)-IsNull(RB.NilaiRetur,0) as Sisa,IsNull( C.Bayar,0),Isnull(C.Retur,0),Isnull(C.Denda,0) " & _
                     "FROM (MBeli Left Join (Select IDBeli,Sum(Bayar+Retur) as Bayar FROM MBayarHutangD where IsJual=0 and MBayarHutangD.IDBayarHutang<>" & IDTransaksi & " group by IDBeli) B on MBeli.NoID=B.IDBeli) " & _
                     "LEFT JOIN (select MBayarHutangD.IDBeli,MBayarHutangD.Bayar,MBayarHutangd.Retur,MBayarHutangD.Denda from MBayarHutangD where MBayarHutangD.IDBayarHutang=" & IDTransaksi & ") C on MBeli.NoID=C.IDBeli " & _
                     "LEFT JOIN (Select IDBeli,Sum(MreturBeli.Total)  as NilaiRetur FROM MReturBeli where MReturBeli.Isposted=1 group by IDBeli) RB on MBeli.NoID=RB.IDBeli " & _
                     "WHERE MBeli.isPosted=1 and  Total-Isnull(B.Bayar,0)-IsNull(RB.NilaiRetur,0)<>0 and MBeli.IDSupplier=" & IDAlamat)

            Else
                EksekusiSQL("insert Into TBayarHutangD(Isjual,IDBayarHutang,IDBeli,IDUser,Tanggal,JatuhTempo,Kode,KodeReff," & _
                            "Jumlah,Terbayar,Sisa,Bayar,Retur,Denda) " & _
                            "Select " & IIf(IsHutang, 0, 1) & " as IsJual, " & IDTransaksi & ",MJual.NoID," & IDUserAktif & " As IDUser,Tanggal,JatuhTempo,Kode,KodeReff," & _
                            "Total,case when B.Bayar IS NULL then 0 else B.Bayar END,Total-(case when B.Bayar IS NULL then 0 else B.Bayar END) as Sisa,case when C.Bayar IS NULL then 0 else C.Bayar END,Case When C.Retur IS NULL then 0 else C.Retur END,case when C.Denda IS NULL then 0 else C.Denda END " & _
                            "FROM (MJual Left Join (Select IDBeli,Sum(Bayar+Retur) as Bayar FROM MBayarHutangD where IsJual=1 and MBayarHutangD.IDBayarHutang<>" & IDTransaksi & " group by IDBeli) B on MJual.NoID=B.IDBeli) " & _
                            "LEFT JOIN (select MBayarHutangD.IDBeli,MBayarHutangD.Bayar,MBayarHutangd.Retur,MBayarHutangD.Denda from MBayarHutangD where MBayarHutangD.IDBayarHutang=" & IDTransaksi & ") C on MJual.NoID=C.IDBeli " & _
                            "WHERE MJual.isPosted=1 and  Total-(case when B.Bayar IS NULL then 0 else B.Bayar END)<>0 and MJual.IDCustomer=" & IDAlamat)
            End If
        Catch ex As Exception
            XtraMessageBox.Show("Info : " & ex.Message, NamaAplikasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
        End Try
    End Sub
End Class
