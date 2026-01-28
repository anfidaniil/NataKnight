Public Class EntityManager

    Private nextId As Integer = 0
    Private availableAfterDestruction As Stack(Of Integer) = New Stack(Of Integer)
    Private destroyed As New HashSet(Of Integer)

    Public Entities As New List(Of Integer)

    Public Function CreateEntity() As Integer
        Dim id As Integer
        If availableAfterDestruction.Count > 0 Then
            id = availableAfterDestruction.Pop()
            destroyed.Remove(id)
        Else
            id = nextId
            nextId += 1
        End If

        Entities.Add(id)
        Return id
    End Function

    Public Function CreateEntityWithId(id As Integer) As Integer
        Entities.Add(id)
        If id >= nextId Then
            nextId = id + 1
        End If
        Return id
    End Function


    Public Sub RemoveEntity(entityId As Integer)
        If destroyed.Contains(entityId) Then
            Throw New Exception($"Entity {entityId} destroyed twice")
        End If

        If Entities.Remove(entityId) Then
            destroyed.Add(entityId)
            availableAfterDestruction.Push(entityId)
        End If
    End Sub

    Public Function Exists(entityId As Integer) As Boolean
        Return Entities.Contains(entityId)
    End Function
End Class
