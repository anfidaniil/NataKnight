Public Class ComponentStore(Of T)
    Private components As New Dictionary(Of Integer, T)

    Public Sub AddComponent(entityId As Integer, component As T)
        components(entityId) = component
    End Sub

    Public Function HasComponent(entityId As Integer) As Boolean
        Return components.ContainsKey(entityId)
    End Function

    Public Function GetComponent(entityId As Integer) As T
        Return components(entityId)
    End Function

    Public Sub RemoveComponent(entityId As Integer)
        components.Remove(entityId)
    End Sub

    Public ReadOnly Property All As Dictionary(Of Integer, T)
        Get
            Return components
        End Get
    End Property
End Class
