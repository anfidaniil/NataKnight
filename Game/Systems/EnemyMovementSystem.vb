Public Class EnemyMovementSystem
    Implements ISystem

    Public Sub Update(world As World, dt As Single) Implements ISystem.Update
        For Each kv In world.Enemies.All
            Dim id = kv.Key
            If world.Movements.HasComponent(id) Then
                Dim t = world.Transforms.GetComponent(id)
                Dim m = world.Movements.GetComponent(id)
                Dim r = world.Renders.GetComponent(id)

                If world.Enemies.HasComponent(id) Then
                    Dim playerPos = world.Transforms.GetComponent(world.PlayerID)
                    Dim x = playerPos.pos.X
                    Dim y = playerPos.pos.Y

                    If world.AudioTriggers.HasComponent(id) Then
                        Dim trig = world.AudioTriggers.GetComponent(id)

                        If trig.cooldown <= 0F Then
                            trig.playRequested = True
                            trig.cooldown = 0.2F
                        Else
                            trig.cooldown -= dt
                        End If
                    End If

                    Dim a = New PointF(
                        x - t.pos.X,
                        y - t.pos.Y
                    )
                    If Math.Abs(a.X) < 0.1F Then a.X = 0
                    If Math.Abs(a.Y) < 0.1F Then a.Y = 0

                    Dim norm = NormalisePointFVector(a)
                    UpdateSprite(norm.X, norm.Y, r)
                    m.acceleration = New PointF(
                        norm.X * World.MAX_ENEMY_ACCELERATION,
                        norm.Y * World.MAX_ENEMY_ACCELERATION
                    )
                End If

            End If
        Next
    End Sub

    Private Sub UpdateSprite(dx As Integer, dy As Integer, r As RenderComponent)

        If (Math.Abs(dx) >= Math.Abs(dy)) Then
            If (dx > 0) Then
                r.spriteX = 4
            End If
            If (dx < 0) Then
                r.spriteX = 2
            End If
        Else
            If (dy < 0) Then
                r.spriteX = 10
            End If

            If (dy > 0) Then
                r.spriteX = 3
            End If
        End If
    End Sub

    Public Sub Draw(world As World, g As Graphics) Implements ISystem.Draw

    End Sub
End Class
