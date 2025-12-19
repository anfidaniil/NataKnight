Public Class BulletMovementSystem
    Implements ISystem

    Public Sub Update(world As World, dt As Single) Implements ISystem.Update
        For Each kv In world.Transforms.All
            Dim id = kv.Key
            If world.Movements.HasComponent(id) And Not world.Enemies.HasComponent(id) And Not world.Enemies.HasComponent(id) Then

                Dim t = kv.Value
                Dim m = world.Movements.GetComponent(id)

                Dim dx = m.velocity.X
                Dim dy = m.velocity.Y
                t.pos = New PointF(
                    t.pos.X + dx * m.velocity.X * dt,
                    t.pos.Y + dy * m.velocity.Y * dt)

            End If
        Next
    End Sub

    Public Sub Draw(world As World) Implements ISystem.Draw

    End Sub
End Class
