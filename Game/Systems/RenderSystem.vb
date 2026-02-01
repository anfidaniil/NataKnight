Imports System.Drawing
Imports System.Reflection.Emit

Public Class RenderSystem
    Implements ISystem

    Public Sub Update(world As World, dt As Single) Implements ISystem.Update
    End Sub

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

        firstTileX -= 1
        firstTileY -= 1
        lastTileX += 1
        lastTileY += 1

        For ty = firstTileY To lastTileY
            For tx = firstTileX To lastTileX
                Dim tileKey As New Point(tx, ty)
                If world.game.level.ContainsKey(tileKey) Then
                    Dim bmp = world.game.level(tileKey)
                    g.DrawImageUnscaled(bmp, tx * World.TILE_SIZE, ty * World.TILE_SIZE)
                End If
            Next
        Next

        Dim renderables = world.Renders.All
        For Each kv In renderables
            Dim id = kv.Key
            If world.Transforms.HasComponent(id) Then

                Dim t = world.Transforms.GetComponent(id)
                Dim r = kv.Value

                Dim screenX = CInt(t.pos.X - r.size / 2)
                Dim screenY = CInt(t.pos.Y - r.size / 2)

                Dim src As New Rectangle(r.spriteX * 32, r.spriteY * 32, 32, 32)
                Dim dst As New Rectangle(screenX, screenY, r.size, r.size)

                If Not world.Immovables.HasComponent(id) Or world.Buffs.HasComponent(id) Then
                    g.DrawImage(world.game.charSprites, dst, src, GraphicsUnit.Pixel)
                End If
            End If
        Next

        g.ResetClip()
        g.ResetTransform()

        Dim score = world.game.score
        Using font As New Font("Arial", 16, FontStyle.Bold)
            Dim text = "Score: " & score
            Dim size = g.MeasureString(text, font)
            g.DrawString(text, font, Brushes.Gray, (Form1.Width - size.Width) - 40, 20)
        End Using

        If world.HealthBars.HasComponent(world.PlayerID) Then
            Dim hb = world.HealthBars.GetComponent(world.PlayerID)

            If hb.currentHealthSprite IsNot Nothing Then

                Dim scale As Single = 2.0F

                If Form1.WindowState = FormWindowState.Maximized OrElse Form1.Width > 1400 Then
                    scale = 4.0F
                End If

                Dim finalWidth As Integer = CInt(hb.currentHealthSprite.Width * scale)
                Dim finalHeight As Integer = CInt(hb.currentHealthSprite.Height * scale)

                Dim padding As Integer = If(scale > 2.0F, 40, 20)

                g.DrawImage(
                    hb.currentHealthSprite,
                    padding,
                    padding,
                    finalWidth,
                    finalHeight
                )
            End If
        End If

    End Sub

End Class