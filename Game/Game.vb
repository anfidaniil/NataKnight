
Imports System.Drawing.Imaging
Imports System.Security.Policy

Public Class Game
    Public world As World
    Public gameOverUI As GameOverScreen
    Public gameState As GameState

    Public level As New Dictionary(Of Point, Bitmap)

    Dim tileMap As Bitmap = My.Resources.GameResources.tiles

    Public level1 As New Bitmap(My.Resources.GameResources.level1_map, New Size(2048, 2048))
    Public wallPositions As New List(Of Point)
    Public charSprites As New Bitmap(My.Resources.GameResources.character_sprites, New Size(480 * 2, 160 * 2))
    Public score As Integer = 0

    Public Sub New(input As InputState)
        Me.world = New World(input, Me)
        Me.gameState = GameState.Playing
        CreateTestWorld()
    End Sub

    Public Sub CreateTestWorld()
        CreateLevel()
        world.CreatePlayer()
        world.CreateCamera()
        'CreateEnemiesAroundPoint(128, 128, 4)
        'CreateEnemiesAroundPoint(400, 128, 4)
    End Sub

    Public Sub CreateMapCollisionBox(pos As PointF, size As Integer)
        world.CreateImmovableWall(pos, size)
    End Sub

    Public Sub GameOver()
        Debug.WriteLine("GameOver")
        gameState = GameState.GameOver
        gameOverUI = New GameOverScreen(
            Form1.Width,
            Form1.Height,
            Sub() RestartGame(),
            Sub() Form1.Close()
        )
    End Sub

    Public Sub RestartGame()
        Me.world = New World(Form1.input, Me)
        Me.gameState = GameState.Playing
        CreateTestWorld()
    End Sub

    Public Sub CreateEnemiesAroundPoint(posX As Integer, posY As Integer, numEnemies As Integer)
        For i = 1 To numEnemies
            world.CreateEnemy(New PointF(
                posX + Random.Shared.Next(-64, 64),
                posY + Random.Shared.Next(-64, 64)
            ))
        Next
    End Sub

    Public Sub Update(dt As Single)
        Select Case gameState
            Case GameState.Playing
                world.Update(dt)
                world.CollisionEvents.Clear()
            Case GameState.GameOver

        End Select
    End Sub

    Public Sub Draw(g As Graphics)
        Select Case gameState
            Case GameState.Playing
                world.Draw(g)
            Case GameState.GameOver
                world.Draw(g)
                gameOverUI.Draw(g, world)

        End Select
    End Sub

    Private Function GetSpriteFromPosition(x As Integer, y As Integer) As Bitmap
        Dim sprite As New Rectangle(32 * x, 32 * y, 32, 32)
        Dim tileSize As Integer = 64

        Dim tile As New Bitmap(tileSize, tileSize, Imaging.PixelFormat.Format32bppArgb)

        Using g As Graphics = Graphics.FromImage(tile)
            g.InterpolationMode = Drawing2D.InterpolationMode.NearestNeighbor
            g.PixelOffsetMode = Drawing2D.PixelOffsetMode.Half

            g.DrawImage(tileMap, New Rectangle(0, 0, tileSize, tileSize), sprite, GraphicsUnit.Pixel)
        End Using

        Return tile

    End Function

    Private Function GetTileFromPosition(x As Integer, y As Integer) As Bitmap
        Dim sprite As New Rectangle(128 * x, 128 * y, 128, 128)
        Dim tileSize As Integer = 256

        Dim tile As New Bitmap(tileSize, tileSize, Imaging.PixelFormat.Format32bppArgb)

        Using g As Graphics = Graphics.FromImage(tile)
            g.InterpolationMode = Drawing2D.InterpolationMode.NearestNeighbor
            g.PixelOffsetMode = Drawing2D.PixelOffsetMode.Half

            g.DrawImage(level1, New Rectangle(0, 0, tileSize, tileSize), sprite, GraphicsUnit.Pixel)
        End Using

        Return tile

    End Function

    Private Function Create128TileFromSprites(
        pos As Point,
        sprite1 As Bitmap,
        sprite2 As Bitmap,
        sprite3 As Bitmap,
        sprite4 As Bitmap
    ) As Bitmap
        Dim tileSize As Integer = 128

        Dim tile128 As New Bitmap(tileSize, tileSize, Imaging.PixelFormat.Format32bppArgb)

        Using g As Graphics = Graphics.FromImage(tile128)
            g.InterpolationMode = Drawing2D.InterpolationMode.NearestNeighbor
            g.PixelOffsetMode = Drawing2D.PixelOffsetMode.Half

            g.DrawImage(sprite1, 0, 0)
            g.DrawImage(sprite2, 0, 64)
            g.DrawImage(sprite3, 64, 0)
            g.DrawImage(sprite4, 64, 64)
        End Using

        Return tile128
    End Function
    Public Sub CreateLevel()
        For i = 0 To 15
            For j = 0 To 15
                If i = 0 And j > 3 And j < 13 Then
                    CreateMapCollisionBox(New PointF(World.TILE_SIZE + 128, j * World.TILE_SIZE), World.TILE_SIZE)
                End If
                If i = 15 And j > 3 And j < 13 Then
                    CreateMapCollisionBox(New PointF(World.TILE_SIZE * i - World.TILE_SIZE / 2, j * World.TILE_SIZE), World.TILE_SIZE)
                End If
                If j = 0 And i > 3 And i < 13 Then
                    CreateMapCollisionBox(New PointF(i * World.TILE_SIZE, World.TILE_SIZE + 128), World.TILE_SIZE)
                End If
                If j = 15 And i > 3 And i < 13 Then
                    CreateMapCollisionBox(New PointF(World.TILE_SIZE * i, World.TILE_SIZE * j - World.TILE_SIZE / 2), World.TILE_SIZE)
                End If

                level(New Point(i, j)) = GetTileFromPosition(i, j)
            Next
            'Left-Upper Corner
            CreateMapCollisionBox(New Point(576, 960), World.TILE_SIZE / 2)
            CreateMapCollisionBox(New Point(704, 832), World.TILE_SIZE / 2)
            CreateMapCollisionBox(New Point(832, 704), World.TILE_SIZE / 2)
            CreateMapCollisionBox(New Point(960, 576), World.TILE_SIZE / 2)
            'Right-Upper Corner
            CreateMapCollisionBox(New Point(3520, 960), World.TILE_SIZE / 2)
            CreateMapCollisionBox(New Point(3392, 832), World.TILE_SIZE / 2)
            CreateMapCollisionBox(New Point(3264, 704), World.TILE_SIZE / 2)
            CreateMapCollisionBox(New Point(3136, 576), World.TILE_SIZE / 2)
            'Left-Down Corner
            CreateMapCollisionBox(New Point(960, 3520), World.TILE_SIZE / 2)
            CreateMapCollisionBox(New Point(832, 3392), World.TILE_SIZE / 2)
            CreateMapCollisionBox(New Point(704, 3264), World.TILE_SIZE / 2)
            CreateMapCollisionBox(New Point(576, 3136), World.TILE_SIZE / 2)
            'Right-Down Corner
            CreateMapCollisionBox(New Point(3136, 3520), World.TILE_SIZE / 2)
            CreateMapCollisionBox(New Point(3264, 3392), World.TILE_SIZE / 2)
            CreateMapCollisionBox(New Point(3392, 3264), World.TILE_SIZE / 2)
            CreateMapCollisionBox(New Point(3520, 3136), World.TILE_SIZE / 2)
        Next
    End Sub
End Class
