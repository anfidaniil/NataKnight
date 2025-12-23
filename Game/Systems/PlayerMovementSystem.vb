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

                    Dim a = New PointF(
                        dx,
                        dy
                    )
                    Dim norm = NormalisePointFVector(a)
                    UpdateSprite(norm.X, norm.Y, r)
                    m.acceleration = New PointF(
                        norm.X * World.MAX_ACCELERATION * 2.0F,
                        norm.Y * World.MAX_ACCELERATION * 2.0F
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
