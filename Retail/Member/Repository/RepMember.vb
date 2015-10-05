Imports VPoint.Model
Imports System.Data
Imports System.Data.SqlClient

Namespace Member
    Public Class RepMember
        Public Function DaftarMReedem(ByVal Filter As String) As Model.DaftarMReedem
            Dim cn As New SqlConnection(StrKonSql)
            Dim com As New SqlCommand
            Dim oDR As SqlDataReader = Nothing
            Dim Pesan As Pesan, obj As MReedem, Daftar As DaftarMReedem
            Try
                cn.Open()
                com.Connection = cn
                com.CommandText = "SELECT * FROM MReedem (NOLOCK) WHERE 1=1 "
                If Filter <> "" Then
                    com.CommandText &= " AND " & Filter
                End If
                oDR = com.ExecuteReader()

                Daftar = New DaftarMReedem
                If oDR.HasRows Then
                    Pesan = New Pesan
                    Pesan.Nomor = Pesan.DITEMUKAN
                    Pesan.Keterangan = ""
                    Daftar.Pesan = Pesan
                End If
                While oDR.Read
                    obj = New MReedem
                    obj.NoID = NullTolInt(oDR.Item("NoID"))
                    obj.Nilai = NullToDbl(oDR.Item("Nilai"))
                    obj.Poin = NullTolInt(oDR.Item("Poin"))
                    obj.IsActive = NullToBool(oDR.Item("IsActive"))
                    Daftar.Add(obj)
                End While
                oDR.Close()

            Catch ex As Exception
                Daftar = New DaftarMReedem
                Pesan = New Pesan
                Pesan.Nomor = Pesan.GAGAL
                Pesan.Keterangan = ex.Message
                Daftar.Pesan = Pesan
            Finally
                If cn.State = ConnectionState.Open Then
                    cn.Close()
                End If
                cn.Dispose()
                com.Dispose()
            End Try
            Return Daftar
        End Function
        Public Function MenampilkanMReedemByID(ByVal NoID As Integer) As Model.MReedem
            Dim cn As New SqlConnection(StrKonSql)
            Dim com As New SqlCommand
            Dim oDR As SqlDataReader = Nothing
            Dim obj As New MReedem
            Try
                cn.Open()
                com.Connection = cn
                com.CommandText = "SELECT * FROM MReedem (NOLOCK) WHERE NoID=" & NoID
                oDR = com.ExecuteReader()

                If oDR.HasRows Then
                    obj.NoID = NullTolInt(oDR.Item("NoID"))
                    obj.Nilai = NullToDbl(oDR.Item("Nilai"))
                    obj.Poin = NullTolInt(oDR.Item("Poin"))
                    obj.IsActive = NullToBool(oDR.Item("IsActive"))
                End If
                oDR.Close()
            Catch ex As Exception
                
            Finally
                If cn.State = ConnectionState.Open Then
                    cn.Close()
                End If
                cn.Dispose()
                com.Dispose()
            End Try
            Return obj
        End Function
        Private Function IsValidasi(ByVal Obj As Model.MReedem, ByRef Pesan As Pesan) As Boolean
            Dim Hasil As Boolean = True
            If Obj.Nilai <= 0 Then
                Pesan = New Pesan
                Pesan.Nomor = Pesan.GAGAL
                Pesan.Keterangan = "Nilai masih 0."
                Hasil = False
            End If
            If Obj.Poin <= 0 Then
                Pesan = New Pesan
                Pesan.Nomor = Pesan.GAGAL
                Pesan.Keterangan = "Poin masih 0."
                Hasil = False
            End If
            Return Hasil
        End Function
        Public Function MenyimpanMReedem(ByVal Obj As Model.MReedem, ByVal ObjLama As Model.MReedem, ByVal IsEdit As Boolean) As Pesan
            Dim cn As New SqlConnection(StrKonSql)
            Dim com As New SqlCommand
            Dim Tran As SqlTransaction = Nothing
            Dim Pesan As Pesan = Nothing
            Try
                cn.Open()
                com.Connection = cn
                Tran = cn.BeginTransaction
                com.Transaction = Tran

                If IsValidasi(Obj, Pesan) Then
                    If IsEdit Then
                        com.CommandText = "UPDATE [MReedem] SET " & vbCrLf & _
                                          "NoID=@NoID,Nilai=@Nilai,Poin=@Poin,IsActive=@IsActive WHERE NoID=@NoID"
                        com.Parameters.Clear()
                        com.Parameters.Add(New SqlParameter("@NoID", ObjLama.NoID))
                        com.Parameters.Add(New SqlParameter("@Nilai", Obj.Nilai))
                        com.Parameters.Add(New SqlParameter("@Poin", Obj.Poin))
                        com.Parameters.Add(New SqlParameter("@IsActive", IIf(Obj.IsActive, 1, 0)))
                        com.ExecuteNonQuery()

                        Pesan = New Pesan
                        Pesan.Nomor = Pesan.DITEMUKAN
                        Pesan.Keterangan = "Simpan Berhasil."
                    Else
                        com.CommandText = "SELECT MAX([NoID]) FROM [MReedem]"
                        Obj.NoID = NullToLong(com.ExecuteScalar())
                        com.CommandText = "INSERT INTO [dbo].[MReedem] ([NoID],[Nilai],[Poin],[IsActive]) VALUES ( " & vbCrLf & _
                                          "@NoID,@Nilai,@Poin,@IsActive)"
                        com.Parameters.Clear()
                        com.Parameters.Add(New SqlParameter("@NoID", Obj.NoID))
                        com.Parameters.Add(New SqlParameter("@Nilai", Obj.Nilai))
                        com.Parameters.Add(New SqlParameter("@Poin", Obj.Poin))
                        com.Parameters.Add(New SqlParameter("@IsActive", IIf(Obj.IsActive, 1, 0)))
                        com.ExecuteNonQuery()

                        Pesan = New Pesan
                        Pesan.Nomor = Pesan.DITEMUKAN
                        Pesan.Keterangan = "Simpan Berhasil."
                    End If
                End If
                If Not Tran Is Nothing Then
                    Tran.Commit()
                End If
                Tran = Nothing
            Catch ex As Exception
                If Not Tran Is Nothing Then
                    Tran.Rollback()
                End If
                Tran = Nothing

                Pesan = New Pesan
                Pesan.Nomor = Pesan.GAGAL
                Pesan.Keterangan = "Simpan Data Gagal. " & vbCrLf & ex.Message
            Finally
                If cn.State = ConnectionState.Open Then
                    cn.Close()
                End If
                cn.Dispose()
                com.Dispose()
            End Try
            Return Pesan
        End Function
        Public Function MenghapusDataMReedem(ByVal NoID As Integer) As Pesan
            Dim cn As New SqlConnection(StrKonSql)
            Dim com As New SqlCommand
            Dim Tran As SqlTransaction = Nothing
            Dim Pesan As Pesan = Nothing
            Try
                cn.Open()
                com.Connection = cn
                Tran = cn.BeginTransaction
                com.Transaction = Tran

                com.CommandText = "UPDATE [MReedem] SET " & vbCrLf & _
                                  "IsActive=0 WHERE NoID=@NoID"
                com.Parameters.Clear()
                com.Parameters.Add(New SqlParameter("@NoID", NoID))
                com.ExecuteNonQuery()
                If Not Tran Is Nothing Then
                    Tran.Commit()
                End If
                Tran = Nothing

                Pesan = New Pesan
                Pesan.Nomor = Pesan.DITEMUKAN
                Pesan.Keterangan = "Hapus Data Berhasil."
            Catch ex As Exception
                If Not Tran Is Nothing Then
                    Tran.Rollback()
                End If
                Tran = Nothing

                Pesan = New Pesan
                Pesan.Nomor = Pesan.GAGAL
                Pesan.Keterangan = "Hapus Data Gagal. " & vbCrLf & ex.Message
            Finally
                If cn.State = ConnectionState.Open Then
                    cn.Close()
                End If
                cn.Dispose()
                com.Dispose()
            End Try
            Return Pesan
        End Function
    End Class
End Namespace
