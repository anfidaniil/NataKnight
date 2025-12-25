Public Class PlayerAttackSystem
    Implements ISystem

    Private input As InputState
    Public Sub New(inputState As InputState)
        input = inputState
    End Sub

    Public Sub Update(world As World, dt As Single) Implements ISystem.Update
        For Each kv In world.Players.All
            Dim id = kv.Key
            Dim a = world.Attacks.GetComponent(id)
            If (world.Transforms.HasComponent(id)) Then
                Dim t = world.Transforms.GetComponent(id)
                If input.fire = True And a.timeRemaining <= 0 Then
                    Dim mouseScreen As Point = Form1.PointToClient(Cursor.Position)

                    Dim cameraID = world.Cameras.All.First().Key
                    Dim cameraPos = world.Transforms.GetComponent(cameraID)

                    Dim mouseWorld As New Point(
                        mouseScreen.X + cameraPos.pos.X,
                        mouseScreen.Y + cameraPos.pos.Y
                    )
                    input.cursorPos = mouseWorld
                    a.timeRemaining = a.attackCooldown
                    world.CreateBullet(t.pos, input.cursorPos, 1)
                End If
                a.timeRemaining -= dt
            End If
        Next
    End Sub

    Public Sub Draw(world As World, g As Graphics) Implements ISystem.Draw

    End Sub
End Class
