Imports Windows.Win32.System

Public Class CameraFollowSystem
    Implements ISystem
    Public Sub Update(world As World, dt As Single) Implements ISystem.Update
        For Each kv In world.Cameras.All
            Dim camera = kv.Key
            If world.Players.HasComponent(world.PlayerID) Then
                If world.Transforms.HasComponent(world.PlayerID) Then
                    Dim t = world.Transforms.GetComponent(camera)
                    Dim m = world.Movements.GetComponent(camera)
                    Dim playerPos = world.Transforms.GetComponent(world.PlayerID)

                    Dim cam = world.Cameras.GetComponent(camera)

                    Dim targetX = playerPos.pos.X - cam.viewWidth / 2
                    Dim targetY = playerPos.pos.Y - cam.viewHeight / 2

                    Dim pos = New PointF(
                        targetX,
                        targetY
                    )
                    t.pos = pos

                End If
            End If
        Next
    End Sub

    Public Sub Draw(world As World) Implements ISystem.Draw

    End Sub
End Class
