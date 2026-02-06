Public Class PlayerMovementSystem
    Implements ISystem

    Private input As InputState

    Public Sub New(inputState As InputState)
        input = inputState
    End Sub
    Public Sub Update(world As World, dt As Single) Implements ISystem.Update
        For Each kv In world.Players.All
            Dim id = kv.Key
            If world.Movements.HasComponent(id) Then
                Dim t = world.Transforms.GetComponent(id)
                Dim m = world.Movements.GetComponent(id)
                Dim r = world.Renders.GetComponent(id)

                Dim dx As Double = 0
                Dim dy As Double = 0

                If world.Players.HasComponent(id) Then
                    If input.up Then dy -= 1
                    If input.down Then dy += 1
                    If input.left Then dx -= 1
                    If input.right Then dx += 1

                    If (dx <> 0 Or dy <> 0) Then
                        r.cooldown -= dt
                        Dim trig = world.AudioTriggers.GetComponent(world.PlayerID)

                        If trig.cooldown <= 0F Then
                            trig.playRequested = True
                            trig.cooldown = 0.2F
                        Else
                            trig.cooldown -= dt
                        End If
                    End If

                    Dim a = New PointF(
                        dx,
                        dy
                    )
                    Dim norm = NormalisePointFVector(a)
                    UpdateSprite(a.X, a.Y, r)

                    m.acceleration = New PointF(
                        norm.X * m.max_acceleration,
                        norm.Y * m.max_acceleration
                    )
                End If
            End If
        Next
    End Sub

    Private Sub UpdateSprite(dx As Integer, dy As Integer, r As RenderComponent)
        Select Case dx
            Case 1
                Select Case dy
                    Case 1
                        r.spriteX = 4
                    Case 0
                        r.spriteX = 1
                    Case -1
                        r.spriteX = 6
                End Select
            Case 0
                Select Case dy
                    Case 1
                        r.spriteX = 3
                    Case 0
                        r.spriteY = 0
                        Return
                    Case -1
                        r.spriteX = 10
                End Select
            Case -1
                Select Case dy
                    Case 1
                        r.spriteX = 2
                    Case 0
                        r.spriteX = 0
                    Case -1
                        r.spriteX = 5
                End Select
        End Select

        If r.cooldown <= 0 Then
            r.cooldown = 0.1F
            If r.spriteY + 1 > 2 Then
                r.spriteY = 0
            Else
                r.spriteY += 1
            End If
        End If

    End Sub

    Public Sub Draw(world As World, g As Graphics) Implements ISystem.Draw

    End Sub
End Class
