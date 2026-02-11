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

                    Dim px = playerPos.pos.X
                    Dim py = playerPos.pos.Y

                    If world.AudioTriggers.HasComponent(id) Then
                        Dim trig = world.AudioTriggers.GetComponent(id)
                        If trig.cooldown <= 0F Then
                            trig.playRequested = True
                            trig.cooldown = 0.2F
                        Else
                            trig.cooldown -= dt
                        End If
                    End If

                    Dim dx As Single = px - t.pos.X
                    Dim dy As Single = py - t.pos.Y

                    Dim distance As Single = Math.Sqrt(dx * dx + dy * dy)

                    Dim dirX As Single = 0
                    Dim dirY As Single = 0

                    If distance > 0 Then
                        dirX = dx / distance
                        dirY = dy / distance
                    End If

                    UpdateSprite(CInt(dx), CInt(dy), r)

                    Dim safeDistance As Single = 400.0F
                    Dim margin As Single = 50.0F

                    If distance > (safeDistance + margin) Then
                        m.acceleration = New PointF(
                            dirX * m.max_acceleration,
                            dirY * m.max_acceleration
                        )
                    ElseIf distance < (safeDistance - margin) Then
                        m.acceleration = New PointF(
                            -dirX * m.max_acceleration,
                            -dirY * m.max_acceleration
                        )
                    Else
                        m.acceleration = New PointF(0, 0)
                        m.velocity = New PointF(m.velocity.X * 0.9F, m.velocity.Y * 0.9F)
                    End If

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