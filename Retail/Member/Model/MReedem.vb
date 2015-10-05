Namespace Model
    Public Class MReedem
        Private _NoID As Integer
        Private _Nilai As Double
        Private _Poin As Integer
        Private _IsActive As Boolean

        Public Property NoID() As Integer
            Get
                Return _NoID
            End Get
            Set(ByVal value As Integer)
                _NoID = value
            End Set
        End Property
        Public Property Nilai() As Double
            Get
                Return _Nilai
            End Get
            Set(ByVal value As Double)
                _Nilai = value
            End Set
        End Property
        Public Property Poin() As Integer
            Get
                Return _Poin
            End Get
            Set(ByVal value As Integer)
                _Poin = value
            End Set
        End Property
        Public Property IsActive() As Boolean
            Get
                Return _IsActive
            End Get
            Set(ByVal value As Boolean)
                _IsActive = value
            End Set
        End Property
    End Class
    Public Class DaftarMReedem
        Inherits List(Of MReedem)
        Private _Pesan As New Pesan
        Public Property Pesan() As Pesan
            Get
                Return _Pesan
            End Get
            Set(ByVal value As Pesan)
                _Pesan = value
            End Set
        End Property
    End Class
End Namespace
