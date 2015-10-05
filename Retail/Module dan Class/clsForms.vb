Namespace VPOINT.Forms
    Public Class EntriPOD
        Public Shared anInstance As frmEntriPOD
        Public Shared ReadOnly Property Instance() As frmEntriPOD
            Get
                If anInstance Is Nothing Then
                    anInstance = New frmEntriPOD
                End If
                Return anInstance
            End Get
        End Property
    End Class

    Public Class EntriBeliD
        Public Shared adInstance As frmEntriBeliD
        Public Shared ReadOnly Property Instance() As frmEntriBeliD
            Get
                If adInstance Is Nothing Then
                    adInstance = New frmEntriBeliD
                End If
                Return adInstance
            End Get
        End Property
    End Class
End Namespace
