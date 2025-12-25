
Public Class Game
    Public world As World
    Public gameOverUI As GameOverScreen
    Public gameState As GameState
    Public level As New Dictionary(Of Point, Bitmap)
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
        CreateEnemiesAroundPoint(128, 128, 4)
        CreateEnemiesAroundPoint(400, 128, 4)
    End Sub

    Public Sub CreateMapCollisionBox(pos As PointF)
        world.CreateImmovableWall(pos)
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

    Public Sub CreateLevel()
        Dim tileBmp As Bitmap = My.Resources.GameResources.worldTile128

        CreateMapCollisionBox(New PointF(+World.TILE_SIZE / 2, +World.TILE_SIZE / 2))
        CreateMapCollisionBox(New PointF(+World.TILE_SIZE / 2 + World.TILE_SIZE * 10, +World.TILE_SIZE / 2))
        For y = 0 To 0
            For x = 1 To 9
                level(New Point(x, y)) = tileBmp
                CreateMapCollisionBox(New PointF(World.TILE_SIZE / 2 + World.TILE_SIZE * x, -World.TILE_SIZE / 2))
            Next
        Next
        For y = 1 To 3
            For x = 0 To 10
                level(New Point(x, y)) = tileBmp
            Next
            CreateMapCollisionBox(New PointF(-World.TILE_SIZE / 2, World.TILE_SIZE / 2 + World.TILE_SIZE * y))
            CreateMapCollisionBox(New PointF(+World.TILE_SIZE / 2 + World.TILE_SIZE * 11, World.TILE_SIZE / 2 + World.TILE_SIZE * y))
        Next
        For y = 4 To 9
            For x = 0 To 10
                level(New Point(x, y)) = tileBmp
            Next
            CreateMapCollisionBox(New PointF(-World.TILE_SIZE / 2, World.TILE_SIZE / 2 + World.TILE_SIZE * y))
            CreateMapCollisionBox(New PointF(+World.TILE_SIZE / 2 + World.TILE_SIZE * 11, World.TILE_SIZE / 2 + World.TILE_SIZE * y))
        Next
        For y = 10 To 10
            For x = 1 To 9
                level(New Point(x, y)) = tileBmp
                CreateMapCollisionBox(New PointF(World.TILE_SIZE / 2 + World.TILE_SIZE * x, World.TILE_SIZE / 2 + World.TILE_SIZE * (y + 1)))
            Next
        Next
        CreateMapCollisionBox(New PointF(+World.TILE_SIZE / 2, World.TILE_SIZE / 2 + World.TILE_SIZE * 10))
        CreateMapCollisionBox(New PointF(+World.TILE_SIZE / 2 + World.TILE_SIZE * 10, World.TILE_SIZE / 2 + World.TILE_SIZE * 10))
    End Sub
End Class
