Imports System.Reflection.Emit

Public Class RenderSystem

    Implements ISystem

    Public Sub Draw(world As World, g As Graphics) Implements ISystem.Draw
        g.CompositingMode = Drawing2D.CompositingMode.SourceOver
        g.CompositingQuality = Drawing2D.CompositingQuality.HighSpeed
        g.InterpolationMode = Drawing2D.InterpolationMode.NearestNeighbor
        g.SmoothingMode = Drawing2D.SmoothingMode.None
        g.PixelOffsetMode = Drawing2D.PixelOffsetMode.Half

        g.Clear(Color.Black)

        Dim cameraID = world.Cameras.All.First().Key
        Dim camera = world.Cameras.GetComponent(cameraID)
        Dim cameraPos = world.Transforms.GetComponent(cameraID)

        Dim camX As Single = CInt(cameraPos.pos.X)
        Dim camY As Single = CInt(cameraPos.pos.Y)

        g.TranslateTransform(-camX, -camY)

        g.SetClip(New Rectangle(
            camX - 2,
            camY - 2,
            camera.viewWidth + 4,
            camera.viewHeight + 4
        ))

        Dim firstTileX As Integer = camX \ World.TILE_SIZE
        Dim firstTileY As Integer = camY \ World.TILE_SIZE

        Dim lastTileX As Integer = (camX + camera.viewWidth) \ World.TILE_SIZE
        Dim lastTileY As Integer = (camY + camera.viewHeight) \ World.TILE_SIZE

        ' Safety padding
        firstTileX -= 1
        firstTileY -= 1
        lastTileX += 1
        lastTileY += 1

        For ty = firstTileY To lastTileY
            For tx = firstTileX To lastTileX

                Dim tileKey As New Point(tx, ty)
                If Not world.game.level.ContainsKey(tileKey) Then Continue For

                Dim bmp = world.game.level(tileKey)

                g.DrawImageUnscaled(
            bmp,
            tx * World.TILE_SIZE,
            ty * World.TILE_SIZE
        )
            Next
        Next


        For Each kv In world.Transforms.All
            Dim id = kv.Key
            If world.Renders.HasComponent(id) Then
                Dim t = kv.Value
                Dim r = world.Renders.GetComponent(id)

                Dim screenX = CInt(t.pos.X - r.size / 2)
                Dim screenY = CInt(t.pos.Y - r.size / 2)

                g.FillRectangle(r.brush, screenX, screenY, r.size, r.size)
            End If
        Next
        g.ResetClip()
        g.ResetTransform()
    End Sub

    Public Sub Update(world As World, dt As Single) Implements ISystem.Update

    End Sub
End Class
