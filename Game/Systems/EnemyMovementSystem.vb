Public Class EnemyMovementSystem
    Implements ISystem

    Public Sub Update(world As World, dt As Single) Implements ISystem.Update
        For Each kv In world.Transforms.All
            Dim id = kv.Key
            If world.Movements.HasComponent(id) Then
                Dim t = kv.Value
                Dim m = world.Movements.GetComponent(id)

                If world.Enemies.HasComponent(id) Then
                    Dim playerPos = world.Transforms.GetComponent(world.PlayerID)
                    Dim x = playerPos.pos.X
                    Dim y = playerPos.pos.Y

                    Dim a = New PointF(
                        x - t.pos.X,
                        y - t.pos.Y
                    )
                    If Math.Abs(a.X) < 0.1F Then a.X = 0
                    If Math.Abs(a.Y) < 0.1F Then a.Y = 0

                    Dim norm = NormalisePointFVector(a)
                    m.acceleration = New PointF(
                        norm.X * World.MAX_ACCELERATION,
                        norm.Y * World.MAX_ACCELERATION
                    )
                End If

            End If
        Next
    End Sub

    Public Sub Draw(world As World) Implements ISystem.Draw

    End Sub
End Class
