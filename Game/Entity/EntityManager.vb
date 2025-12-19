Public Class EntityManager

    Private nextId As Integer = 0

    Public Entities As New List(Of Integer)

    Public Function CreateEntity() As Integer
        Dim id = nextId
        nextId += 1
        Entities.Add(id)
        Return id
    End Function

    Public Sub RemoveEntity(entityId As Integer)
        Entities.Remove(entityId)
    End Sub

    Public Function Exists(entityId As Integer) As Boolean
        Return Entities.Contains(entityId)
    End Function
End Class
