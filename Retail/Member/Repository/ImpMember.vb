Imports VPoint.Model
Namespace Member
    Public Interface IRSMember
        Function MenyimpanMReedem(ByVal Obj As Model.MReedem, ByVal ObjLama As Model.MReedem, ByVal IsEdit As Boolean) As Pesan
        Function MenampilkanMReedemByID(ByVal NoID As Integer) As MReedem
        Function MenghapusDataMReedem(ByVal NoID As Integer) As Pesan
        Function DaftarMReedem(ByVal Filter As String) As DaftarMReedem
    End Interface

    Public Class ImpMember
        Implements IRSMember
        Private Rep As New RepMember

        Public Function DaftarMReedem(ByVal Filter As String) As Model.DaftarMReedem Implements IRSMember.DaftarMReedem
            Return Rep.DaftarMReedem(Filter)
        End Function

        Public Function MenampilkanMReedemByID(ByVal NoID As Integer) As Model.MReedem Implements IRSMember.MenampilkanMReedemByID
            Return Rep.MenampilkanMReedemByID(NoID)
        End Function

        Public Function MenyimpanMReedem(ByVal Obj As Model.MReedem, ByVal ObjLama As Model.MReedem, ByVal IsEdit As Boolean) As Pesan Implements IRSMember.MenyimpanMReedem
            Return Rep.MenyimpanMReedem(Obj, ObjLama, IsEdit)
        End Function

        Public Function MenghapusDataMReedem(ByVal NoID As Integer) As Pesan Implements IRSMember.MenghapusDataMReedem
            Return Rep.MenghapusDataMReedem(NoID)
        End Function
    End Class
End Namespace