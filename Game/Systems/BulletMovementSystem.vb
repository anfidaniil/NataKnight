Public Class MovementSystem
    Implements ISystem

    Public Sub Update(world As World, dt As Single)
        For Each kv In world.Transforms.All
            Dim id = kv.Key
            If world.Movements.HasComponent(id) Then
                Dim t = kv.Value
                Dim m = world.Movements.GetComponent(id)

                Dim dx = m.velocity.X
                Dim dy = m.velocity.Y
                t.pos = New PointF(
                    t.pos.X + dx * m.speed * dt,
                    t.pos.Y + dy * m.speed * dt)

            End If
        Next
    End Sub

    Public Sub Draw(world As World) Implements ISystem.Draw

    End Sub

    Private Sub ISystem_Update(world As World, dt As Single) Implements ISystem.Update
        Update(world, dt)
    End Sub
End Class
