Imports System.Reflection.Emit

Public Class RenderSystem

    Implements ISystem

    Private g As Graphics

    Public Sub New(graphics As Graphics)
        g = graphics
    End Sub

    Public Sub Draw(world As World)
        g.CompositingMode = Drawing2D.CompositingMode.SourceCopy
        g.CompositingQuality = Drawing2D.CompositingQuality.HighSpeed
        g.InterpolationMode = Drawing2D.InterpolationMode.NearestNeighbor
        g.SmoothingMode = Drawing2D.SmoothingMode.None
        g.PixelOffsetMode = Drawing2D.PixelOffsetMode.None

        g.Clear(Color.Black)

        Dim cameraID = world.Cameras.All.First().Key
        Dim camera = world.Cameras.GetComponent(cameraID)
        Dim cameraPos = world.Transforms.GetComponent(cameraID)

        Dim camX As Single = CSng(Math.Floor(cameraPos.pos.X))
        Dim camY As Single = CSng(Math.Floor(cameraPos.pos.Y))



        g.TranslateTransform(-camX, -camY)
        g.SetClip(New Rectangle(
            CInt(camX),
            CInt(camY),
            camera.viewWidth,
            camera.viewHeight
        ))


        g.DrawImageUnscaled(world.game.level, 0, 0)

        For Each kv In world.Transforms.All
            Dim id = kv.Key
            If world.Renders.HasComponent(id) Then
                Dim t = kv.Value
                Dim r = world.Renders.GetComponent(id)

                Dim screenX = t.pos.X
                Dim screenY = t.pos.Y
                g.FillRectangle(r.brush, screenX, screenY, r.size, r.size)
            End If
        Next
        g.ResetClip()
        g.ResetTransform()
    End Sub

    Public Sub Update(world As World, dt As Single) Implements ISystem.Update

    End Sub

    Private Sub ISystem_Draw(world As World) Implements ISystem.Draw
        Draw(world)
    End Sub
End Class
