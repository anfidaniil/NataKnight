
Imports System.Drawing.Imaging
Imports System.Security.Policy

Public Class Game
    Public world As World
    Public gameOverUI As GameOverScreen
    Public menuScreen As MenuScreen
    Public startingMenuScreen As StartScreen
    Public gameState As GameState

    Public level As New Dictionary(Of Point, Bitmap)
    Public bgc As New Bitmap(My.Resources.GameResources.MAINmenu)

    Public level1 As New Bitmap(My.Resources.GameResources.level1_map)
    Public wallPositions As New List(Of Point)
    Public charSprites As New Bitmap(My.Resources.GameResources.character_sprites)
    Public score As Integer = 0

    Private Sub InitializeSounds()
        AudioEngine.Initialize()
        'AudioEngine.LoadSound("explosion", New SharpDX.Multimedia.SoundStream(My.Resources.GameResources.explosion))
        'AudioEngine.LoadSound("music", New SharpDX.Multimedia.SoundStream(My.Resources.GameResources.guitar_0001))
        'AudioEngine.PlayLoop("music", 1.0F)
    End Sub
    Public Sub New(input As InputState)
        'InitializeSounds()

        Me.world = New World(input, Me)
        CreateTestWorld()
        Me.gameState = GameState.Starting
        menuScreen = New MenuScreen(
            Form1.Width,
            Form1.Height,
            Sub() StartNewGame(),
            Sub() Form1.Close(),
            Sub() gameState = GameState.Playing
        )
        startingMenuScreen = New StartScreen(
            Form1.Width,
            Form1.Height,
            Sub() StartNewGame(),
            Sub() Form1.Close()
        )

    End Sub

    Public Sub ChangeCameraView()
        Dim id = world.Cameras.All.First.Key
        Dim cam = world.Cameras.GetComponent(id)
        cam.viewHeight = Form1.Height
        cam.viewWidth = Form1.Width

        menuScreen = New MenuScreen(
            Form1.Width,
            Form1.Height,
            Sub() StartNewGame(),
            Sub() Form1.Close(),
            Sub() gameState = GameState.Playing
        )

        startingMenuScreen = New StartScreen(
            Form1.Width,
            Form1.Height,
            Sub() StartNewGame(),
            Sub() Form1.Close()
        )
    End Sub

    Public Sub CreateTestWorld()
        CreateLevel()
        world.CreatePlayer()
        world.CreateCamera()
    End Sub

    Public Sub CreateMapCollisionBox(pos As PointF, size As Point)
        world.CreateImmovableWall(pos, size)
    End Sub

    Public Sub GameOver()
        Debug.WriteLine("GameOver")
        gameState = GameState.GameOver
        gameOverUI = New GameOverScreen(
            Form1.Width,
            Form1.Height,
            Sub() StartNewGame(),
            Sub() Form1.Close()
        )
    End Sub

    Public Sub StartNewGame()
        Me.world = New World(Form1.input, Me)
        Me.gameState = GameState.Playing
        Me.score = 0
        CreateTestWorld()
        Debug.WriteLine("Starting New Game")
    End Sub

    Public Sub CreateEnemiesAroundPoint(posX As Integer, posY As Integer, numEnemies As Integer)
        For i = 1 To numEnemies
            world.CreateEnemy(New PointF(
                posX + Random.Shared.Next(-10, 10),
                posY + Random.Shared.Next(-10, 10)
            ))
        Next
    End Sub

    Public Sub Update(dt As Single)
        Select Case gameState
            Case GameState.Menu

            Case GameState.Playing
                world.Update(dt)
                world.CollisionEvents.Clear()
            Case GameState.GameOver

        End Select
    End Sub

    Public Sub Draw(g As Graphics)
        Select Case gameState
            Case GameState.Starting
                startingMenuScreen.Draw(g, world)
            Case GameState.Menu
                world.Draw(g)
                menuScreen.Draw(g, world)
            Case GameState.Playing
                world.Draw(g)
            Case GameState.GameOver
                world.Draw(g)
                gameOverUI.Draw(g, world)

        End Select
    End Sub

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
    Public Sub CreateLevel()
        For i = 0 To 15
            For j = 0 To 15
                level(New Point(i, j)) = GetTileFromPosition(i, j)
            Next
        Next

        Dim T As Integer = World.TILE_SIZE
        Dim H As Integer = T \ 2
        CreateMapCollisionBox(
        New PointF(
            T + H,                 ' center X = left wall column
            T * 4 + (T * 9) / 2    ' center Y
        ),
        New Point(T, T * 9)
        )
        CreateMapCollisionBox(
            New PointF(
                T * 15 - H,
                T * 4 + (T * 9) / 2
            ),
            New Point(T, T * 9)
        )
        CreateMapCollisionBox(
            New PointF(
                T * 4 + (T * 9) / 2,
                T + H
            ),
            New Point(T * 9, T)
        )
        CreateMapCollisionBox(
            New PointF(
                T * 4 + (T * 9) / 2,
                T * 15 - H
            ),
            New Point(T * 9, T)
        )

        'Left-Upper Corner
        CreateMapCollisionBox(New Point(576, 960), New Point(World.TILE_SIZE / 2, World.TILE_SIZE / 2))
        CreateMapCollisionBox(New Point(704, 832), New Point(World.TILE_SIZE / 2, World.TILE_SIZE / 2))
        CreateMapCollisionBox(New Point(832, 704), New Point(World.TILE_SIZE / 2, World.TILE_SIZE / 2))
        CreateMapCollisionBox(New Point(960, 576), New Point(World.TILE_SIZE / 2, World.TILE_SIZE / 2))
        'Right-Upper Corner
        CreateMapCollisionBox(New Point(3520, 960), New Point(World.TILE_SIZE / 2, World.TILE_SIZE / 2))
        CreateMapCollisionBox(New Point(3392, 832), New Point(World.TILE_SIZE / 2, World.TILE_SIZE / 2))
        CreateMapCollisionBox(New Point(3264, 704), New Point(World.TILE_SIZE / 2, World.TILE_SIZE / 2))
        CreateMapCollisionBox(New Point(3136, 576), New Point(World.TILE_SIZE / 2, World.TILE_SIZE / 2))
        'Left-Down Corner
        CreateMapCollisionBox(New Point(960, 3520), New Point(World.TILE_SIZE / 2, World.TILE_SIZE / 2))
        CreateMapCollisionBox(New Point(832, 3392), New Point(World.TILE_SIZE / 2, World.TILE_SIZE / 2))
        CreateMapCollisionBox(New Point(704, 3264), New Point(World.TILE_SIZE / 2, World.TILE_SIZE / 2))
        CreateMapCollisionBox(New Point(576, 3136), New Point(World.TILE_SIZE / 2, World.TILE_SIZE / 2))
        'Right-Down Corner
        CreateMapCollisionBox(New Point(3136, 3520), New Point(World.TILE_SIZE / 2, World.TILE_SIZE / 2))
        CreateMapCollisionBox(New Point(3264, 3392), New Point(World.TILE_SIZE / 2, World.TILE_SIZE / 2))
        CreateMapCollisionBox(New Point(3392, 3264), New Point(World.TILE_SIZE / 2, World.TILE_SIZE / 2))
        CreateMapCollisionBox(New Point(3520, 3136), New Point(World.TILE_SIZE / 2, World.TILE_SIZE / 2))
    End Sub
End Class
