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

                    Dim a = New PointF(
                        targetX - t.pos.X,
                        targetY - t.pos.Y
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
