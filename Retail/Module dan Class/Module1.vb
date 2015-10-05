Imports Microsoft.VisualBasic
Imports System
Public Class DragObject
    Private taskNames As String() = New String() {"Project", "Deliverable", "Task"}
    Private index As Integer
    Public Sub New(ByVal index As Integer)
        Me.index = index
    End Sub
    Public ReadOnly Property DragData() As Object
        Get
            Return New Object() {taskNames(index), DateTime.Now, 0}
        End Get
    End Property
    Public ReadOnly Property ImageIndex() As Integer
        Get
            Return index
        End Get
    End Property
End Class
