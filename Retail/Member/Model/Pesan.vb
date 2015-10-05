Public Class Pesan
    Public Const NORMAL As Integer = 0
    Public Const DITEMUKAN As Integer = 1
    Public Const TIDAK_DITEMUKAN As Integer = 2
    Public Const GAGAL As Integer = 999
    Private _Nomor As Integer
    Private _Keterangan As String
    Sub New()
        _Nomor = 0
    End Sub
    Property Nomor() As Integer
        Get
            Return _Nomor
        End Get
        Set(ByVal value As Integer)
            _Nomor = value
        End Set
    End Property
    Property Keterangan() As String
        Get
            Return _Keterangan
        End Get
        Set(ByVal value As String)
            _Keterangan = value
        End Set
    End Property
End Class