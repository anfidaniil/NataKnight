Imports System.Reflection.Emit
Imports System.Drawing

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
                    g.DrawImageUnscaled(
                        bmp,
                        tx * World.TILE_SIZE,
                        ty * World.TILE_SIZE
                    )
                End If
            Next
        Next

        For Each kv In world.Transforms.All
            Dim id = kv.Key

            If world.Renders.HasComponent(id) Then

                Dim t = kv.Value
                Dim r = world.Renders.GetComponent(id)

                Dim screenX = CInt(t.pos.X - r.size / 2)
                Dim screenY = CInt(t.pos.Y - r.size / 2)

                Dim src As New Rectangle(r.spriteX * 32, r.spriteY * 32, 32, 32)
                Dim dst As New Rectangle(screenX, screenY, 64, 64)

                If Not world.Immovables.HasComponent(id) Then
                    g.DrawImage(world.game.charSprites, dst, src, GraphicsUnit.Pixel)
                Else
                End If
            End If
        Next

        g.ResetClip()
        g.ResetTransform()

        Dim uiScale As Single = Math.Max(1.0F, Form1.Width / 1280.0F)

        Dim score = world.game.score

        Dim fontSize As Single = 20.0F * uiScale

        Using font As New Font("Arial", fontSize, FontStyle.Bold)
            Dim text = "Score: " & score
            Dim size = g.MeasureString(text, font)

            Dim paddingX As Integer = CInt(40 * uiScale)
            Dim paddingY As Integer = CInt(20 * uiScale)

            g.DrawString(text, font, Brushes.Gray, (Form1.Width - size.Width) - paddingX, paddingY)
        End Using

        If world.HealthBars.HasComponent(world.PlayerID) Then
            Dim hb = world.HealthBars.GetComponent(world.PlayerID)

            If hb.currentHealthSprite IsNot Nothing Then

                Dim finalScale As Single = 2.5F * uiScale

                Dim finalWidth As Integer = CInt(hb.currentHealthSprite.Width * finalScale)
                Dim finalHeight As Integer = CInt(hb.currentHealthSprite.Height * finalScale)

                Dim posX As Integer = CInt(20 * uiScale)
                Dim posY As Integer = CInt(20 * uiScale)

                g.DrawImage(
                    hb.currentHealthSprite,
                    posX,
                    posY,
                    finalWidth,
                    finalHeight
                )
            End If
        End If

    End Sub

End Class
