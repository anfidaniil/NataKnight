Public Class PlayerMovementSystem
    Implements ISystem

    Private input As InputState

    Public Sub New(inputState As InputState)
        input = inputState
    End Sub
    Public Sub Update(world As World, dt As Single) Implements ISystem.Update
        For Each kv In world.Transforms.All
            Dim id = kv.Key
            If world.Movements.HasComponent(id) Then
                Dim t = kv.Value
                Dim m = world.Movements.GetComponent(id)

                Dim dx As Single = 0
                Dim dy As Single = 0

                If input.up Then dy -= 1
                If input.down Then dy += 1
                If input.left Then dx -= 1
                If input.right Then dx += 1

                t.pos = New PointF(
                    t.pos.X + dx * m.speed * dt,
                    t.pos.Y + dy * m.speed * dt)
            End If
        Next
    End Sub

    Public Sub Draw(world As World) Implements ISystem.Draw

    End Sub
End Class
