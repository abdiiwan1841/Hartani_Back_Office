Imports FastReport
Imports FastReport.Utils
Imports DevExpress.XtraEditors
Imports DevExpress.Utils
Imports System.Data
Imports System.Data.SqlClient
Imports System.Linq
Public Class clsFastReport
    Public Enum OptionPrint
        Preview = 0
        Design = 1
        Print = 2
    End Enum
    Public Shared oDSReport As New DataSet
    Public Shared FR As New Report

    Public Shared Sub CetakFastreport(ByVal PrintFs As OptionPrint, ByVal namafile As String, ByVal DataReport As String, Optional ByVal ParameterValues As String = ";") 'Paramaeter add split ;
        Dim strsql() As String = {""}
        Try
            If oDSReport Is Nothing AndAlso oDSReport.Tables(DataReport) Is Nothing Then
                XtraMessageBox.Show("Datasource belum diset.", "Retail System", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Exit Sub
            End If
            'If FastReport.IsRunning Then
            FR = New Report
            'End If
            If Not System.IO.File.Exists(namafile) Then
                CreateReport(namafile, DataReport)
            Else
                If PrintFs = OptionPrint.Preview Then
                    'Add Parameter
                    strsql = Split(ParameterValues, ";")

                    'Load from file (*.frx)
                    FR.Load(namafile)

                    ' register all data tables and relations
                    FR.RegisterData(oDSReport)
                    ' enable the "Categories" and "Products" tables to use it in the report
                    FR.GetDataSource(DataReport).Enabled = True

                    'Perintah tampil
                    FR.Show()
                ElseIf PrintFs = OptionPrint.Print Then
                    'Load from file (*.frx)
                    FR.Load(namafile)

                    ' register all data tables and relations
                    FR.RegisterData(oDSReport)
                    ' enable the "Categories" and "Products" tables to use it in the report
                    FR.GetDataSource(DataReport).Enabled = True

                    'Perintah print
                    FR.Print()
                ElseIf PrintFs = OptionPrint.Design Then
                    'Load from file (*.frx)
                    FR.Load(namafile)

                    ' register all data tables and relations
                    FR.RegisterData(oDSReport)
                    ' enable the "Categories" and "Products" tables to use it in the report
                    FR.GetDataSource(DataReport).Enabled = True

                    'Perintah edit report
                    FR.Design()
                End If
            End If
        Catch ex As Exception
            XtraMessageBox.Show("Kesalahan : " & ex.Message, "Retail System", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            FR.Dispose()
            oDSReport.Dispose()
        End Try
    End Sub
    Public Shared Sub CreateReport(ByVal namafile As String, ByVal DataReport As String)
        Try
            If oDSReport Is Nothing AndAlso oDSReport.Tables(DataReport) Is Nothing Then
                XtraMessageBox.Show("Datasource belum diset.", "Retail System", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Exit Sub
            End If
            FR = New Report
            ' register all data tables and relations
            FR.RegisterData(oDSReport)
            ' enable the "Categories" and "Products" tables to use it in the report
            FR.GetDataSource(DataReport).Enabled = True
            FR.FileName = namafile
            FR.Design()


            '' add report page
            'Dim page As New ReportPage
            'report.Pages.Add(page)
            '' always give names to objects you create. You can use CreateUniqueName method to do this;
            '' call it after the object is added to a report.
            'page.CreateUniqueName()

            '' create master data band
            'Dim masterDataBand As New DataBand
            'page.Bands.Add(masterDataBand)
            'masterDataBand.CreateUniqueName()
            'masterDataBand.DataSource = report.GetDataSource("Data")
            'masterDataBand.Height = (Units.Centimeters * 0.5!)

            '' create category name text
            'Dim categoryText As New TextObject
            'categoryText.Parent = masterDataBand
            'categoryText.CreateUniqueName()
            'categoryText.Bounds = New RectangleF(0.0!, 0.0!, (Units.Centimeters * 5.0!), (Units.Centimeters * 0.5!))
            'categoryText.Font = New Font("Arial", 10.0!, FontStyle.Bold)
            'categoryText.Text = "[Data.Kode]"

        Catch ex As Exception
            XtraMessageBox.Show("Kesalahan : " & ex.Message, "Retail System", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            FR.Dispose()
            oDSReport.Dispose()
        End Try
    End Sub
End Class
