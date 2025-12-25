Public Class EntityManager

    Private nextId As Integer = 0
    Private availableAfterDestruction As Queue(Of Integer) = New Queue(Of Integer)

    Public Entities As New List(Of Integer)

    Public Function CreateEntity() As Integer
        Dim id As Integer
        If availableAfterDestruction.Count > 0 Then
            id = availableAfterDestruction.Dequeue()
        Else
            id = nextId
            nextId += 1
        End If

        Entities.Add(id)
        Return id
    End Function

    Public Sub RemoveEntity(entityId As Integer)
        Entities.Remove(entityId)
        availableAfterDestruction.Enqueue(entityId)
    End Sub

    Public Function Exists(entityId As Integer) As Boolean
        Return Entities.Contains(entityId)
    End Function
End Class
