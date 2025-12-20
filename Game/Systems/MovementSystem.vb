Public Class MovementSystem
    Implements ISystem

    Public Sub Update(world As World, dt As Single) Implements ISystem.Update
        For Each kv In world.Movements.All
            Dim id = kv.Key
            If world.Transforms.HasComponent(id) Then
                Dim m = kv.Value
                Dim t = world.Transforms.GetComponent(id)

                Dim vx = m.velocity.X + m.acceleration.X * dt
                Dim vy = m.velocity.Y + m.acceleration.Y * dt

                vx = Math.Max(-World.MAX_VELOCITY, Math.Min(World.MAX_VELOCITY, vx))
                vy = Math.Max(-World.MAX_VELOCITY, Math.Min(World.MAX_VELOCITY, vy))

                vx *= Math.Max(0, 1 - m.damping * dt)
                vy *= Math.Max(0, 1 - m.damping * dt)

                If Math.Abs(vx) < 1.0F Then vx = 0
                If Math.Abs(vy) < 1.0F Then vy = 0

                m.velocity = New PointF(vx, vy)

                t.pos = New PointF(
                    CSng(Math.Round(t.pos.X + vx * dt)),
                    CSng(Math.Round(t.pos.Y + vy * dt))
                )
            End If
        Next
    End Sub

    Public Sub Draw(world As World) Implements ISystem.Draw

    End Sub
End Class
