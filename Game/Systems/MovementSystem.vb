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

                vx = Math.Clamp(vx, -m.max_velocity, m.max_velocity)
                vy = Math.Clamp(vy, -m.max_velocity, m.max_velocity)

                Dim dampingFactor As Single = CSng(Math.Exp(-m.damping * dt))
                vx *= dampingFactor
                vy *= dampingFactor

                If Math.Abs(vx) < 0.01F Then vx = 0
                If Math.Abs(vy) < 0.01F Then vy = 0

                m.velocity = New PointF(vx, vy)

                t.pos = New PointF(
                    t.pos.X + vx * dt,
                    t.pos.Y + vy * dt
                )
            End If
        Next
    End Sub

    Public Sub Draw(world As World, g As Graphics) Implements ISystem.Draw

    End Sub
End Class
