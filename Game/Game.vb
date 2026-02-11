Imports System.Drawing.Imaging
Imports System.Security.Policy
Imports SharpDX.Multimedia

Public Class Game
    Public world As World
    Public input As InputState
    Public gameOverUI As GameOverScreen
    Public menuScreen As MenuScreen
    Public startingMenuScreen As StartScreen
    Public tutorialScreen As TutorialScreen

    Public exitScreen As ExitScreen
    Public previousState As GameState

    Public gameState As GameState

    Public level As New Dictionary(Of Point, Bitmap)
    Public levelRenderLast As New Dictionary(Of Point, Bitmap)
    Public bgc As New Bitmap(My.Resources.GameResources.MAINmenu)

    Public level1 As New Bitmap(My.Resources.GameResources.level1_map)
    Public bottomGate As New Bitmap(My.Resources.GameResources.zabornijnyy)
    Public wallPositions As New List(Of Point)
    Public charSprites As New Bitmap(My.Resources.GameResources.character_sprites)
    Public score As Integer = 0

    Public loadedWithSuccess = False

    Private Sub InitializeSounds()
        AudioEngine.Initialize()
        AudioEngine.LoadSound("bulletImpact1", New SharpDX.Multimedia.SoundStream(My.Resources.GameResources.bulletimpact1))
        AudioEngine.LoadSound("bulletImpact2", New SharpDX.Multimedia.SoundStream(My.Resources.GameResources.bulletimpact2))
        AudioEngine.LoadSound("button_ui_1", New SharpDX.Multimedia.SoundStream(My.Resources.GameResources.button_ui_1))
        AudioEngine.LoadSound("button_ui_2", New SharpDX.Multimedia.SoundStream(My.Resources.GameResources.button_ui_2))
        AudioEngine.LoadSound("footsteps_1", New SharpDX.Multimedia.SoundStream(My.Resources.GameResources.footstep_1))
        AudioEngine.LoadSound("footsteps_2", New SharpDX.Multimedia.SoundStream(My.Resources.GameResources.footstep_2))
        AudioEngine.LoadSound("footsteps_3", New SharpDX.Multimedia.SoundStream(My.Resources.GameResources.footstep_3))
        AudioEngine.LoadSound("footsteps_4", New SharpDX.Multimedia.SoundStream(My.Resources.GameResources.footstep_4))

        AudioEngine.LoadSound("shoot1", New SharpDX.Multimedia.SoundStream(My.Resources.GameResources.shoot0))
        AudioEngine.LoadSound("shoot2", New SharpDX.Multimedia.SoundStream(My.Resources.GameResources.shoot2))
        AudioEngine.LoadSound("shoot3", New SharpDX.Multimedia.SoundStream(My.Resources.GameResources.shoot3))
        AudioEngine.LoadSound("shoot4", New SharpDX.Multimedia.SoundStream(My.Resources.GameResources.shoot4))
        'AudioEngine.LoadSound("music", New SharpDX.Multimedia.SoundStream(My.Resources.GameResources.guitar_0001))
        'AudioEngine.PlayLoop("music", 1.0F)
    End Sub

    Public Function GetCardScale() As Single
        Dim baseHeight As Single = 720.0F
        Dim currentHeight As Single = Form1.Height
        Return Math.Max(0.5F, currentHeight / baseHeight)
    End Function

    Public Function GetUIElementScale() As Single
        Dim currentHeight As Single = Form1.Height

        If currentHeight > 1080 Then
            Return currentHeight / 720.0F
        Else
            Return 1.0F
        End If
    End Function

    Public Sub New(input As InputState)
        InitializeSounds()
        Me.input = input

        Try
            GameStateSerialization.LoadFromFile(Me, "data.json")
            world.CreatePlayer()
            world.CreatePlayerShootingSound()
            world.CreateCamera()
            CreateLevel()
            world.Update(0.01F)
            loadedWithSuccess = True

        Catch ex As Exception
            Me.gameState = GameState.Starting
            Me.world = New World(input, Me)

            Dim waveDataId = Me.world.EntityManager.CreateEntity()
            Me.world.WaveData.AddComponent(waveDataId, New WaveComponent())

            Debug.WriteLine(ex.Message)
        End Try
        CreateScreens()
    End Sub

    Public Sub ContinueGame()
        If loadedWithSuccess Then
            AudioEngine.PlayOneShot("button_ui_1", 1.0F)
            gameState = GameState.Playing
        Else
            AudioEngine.PlayOneShot("button_ui_2", 1.0F)
        End If
    End Sub

    Public Sub CreateScreens(Optional savedPage As Integer = 0)
        Me.tutorialScreen = New TutorialScreen(Me, savedPage)

        menuScreen = New MenuScreen(
            Me,
            Sub() StartNewGame(),
            Sub() GoBackFromTutorialToGame(),
            Sub() Restart(),
            Sub() GoToStartingScreen()
        )

        startingMenuScreen = New StartScreen(
            Me,
            Sub() StartNewGame(),
            Sub() ContinueGame(),
            Sub()
                AudioEngine.PlayOneShot("button_ui_1", 1.0F)
                tutorialScreen.BackAction = Sub() gameState = GameState.Starting
                gameState = GameState.Tutorial
            End Sub,
            Sub() Quit()
        )

        gameOverUI = New GameOverScreen(
            Form1.Width,
            Form1.Height,
            Sub() StartNewGame(),
            Sub() Quit(),
            Sub() GoToStartingScreen()
        )

        exitScreen = New ExitScreen(
            Me,
            Sub() RealQuit(),
            Sub() CancelExit()
        )
    End Sub

    Public Sub ChangeCameraView()
        Dim id = world.Cameras.All.First.Key
        Dim cam = world.Cameras.GetComponent(id)
        cam.viewHeight = Form1.Height
        cam.viewWidth = Form1.Width

        Dim savedPage As Integer = 0
        If Me.tutorialScreen IsNot Nothing Then
            savedPage = Me.tutorialScreen.currentIndex
        End If

        CreateScreens(savedPage)
    End Sub

    Public Sub CreateTestWorld()
        CreateLevel()

        world.CreatePlayerShootingSound()
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
            Sub() Quit(),
            Sub() GoToStartingScreen()
        )
    End Sub

    Public Sub StartNewGame()
        Me.world = New World(Form1.input, Me)
        Me.gameState = GameState.Playing
        Me.score = 0
        Dim waveDataId = Me.world.EntityManager.CreateEntity()
        Me.world.WaveData.AddComponent(waveDataId, New WaveComponent())
        CreateTestWorld()
        AudioEngine.PlayOneShot("button_ui_1", 1.0F)
        loadedWithSuccess = False
        Debug.WriteLine("Starting New Game")
    End Sub

    Public Sub Restart()
        AudioEngine.PlayOneShot("button_ui_1", 1.0F)
        gameState = GameState.Playing
    End Sub

    Public Sub Quit()
        If gameState = GameState.ExitConfirmation Then Exit Sub
        AudioEngine.PlayOneShot("button_ui_1", 1.0F)
        previousState = gameState
        gameState = GameState.ExitConfirmation
        GameStateSerialization.SaveToFile(Me, "data.json")
    End Sub

    Public Sub CancelExit()
        AudioEngine.PlayOneShot("button_ui_1", 1.0F)
        gameState = previousState
    End Sub

    Public Sub RealQuit()
        AudioEngine.PlayOneShot("button_ui_1", 1.0F)
        Me.gameState = GameState.Starting
        GameStateSerialization.SaveToFile(Me, "data.json")
        Form1.allowClose = True
        Form1.Close()
    End Sub

    Public Sub GoToStartingScreen()
        AudioEngine.PlayOneShot("button_ui_1", 1.0F)
        If gameState = GameState.Menu Or gameState = GameState.Playing Then
            gameState = GameState.Starting
            loadedWithSuccess = True
            GameStateSerialization.SaveToFile(Me, "data.json")
        Else
            gameState = GameState.Starting
        End If
        CreateScreens()
    End Sub

    Public Sub GoBackFromTutorialToGame()
        AudioEngine.PlayOneShot("button_ui_1", 1.0F)
        tutorialScreen.BackAction = Sub() gameState = GameState.Menu
        gameState = GameState.Tutorial
    End Sub

    Public Sub Update(dt As Single)
        Select Case gameState
            Case GameState.Menu

            Case GameState.Tutorial

            Case GameState.Playing
                world.Update(dt)
                world.CollisionEvents.Clear()
            Case GameState.GameOver

            Case GameState.Starting

            Case GameState.ExitConfirmation
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
            Case GameState.Tutorial
                tutorialScreen.Draw(g, world)
            Case GameState.ExitConfirmation
                Select Case previousState
                    Case GameState.Tutorial
                        tutorialScreen.Draw(g, world)

                    Case GameState.Playing
                        world.Draw(g)
                    Case GameState.Menu
                        world.Draw(g)
                        menuScreen.Draw(g, world)
                    Case GameState.Starting
                        startingMenuScreen.Draw(g, world)
                    Case GameState.GameOver
                        world.Draw(g)
                        gameOverUI.Draw(g, world)
                    Case Else
                        world.Draw(g)
                End Select
                exitScreen.Draw(g, world)
        End Select
    End Sub

    Private Function GetTileFromPosition(x As Integer, y As Integer, map As Bitmap) As Bitmap
        Dim sprite As New Rectangle(World.TILE_SIZE * x, World.TILE_SIZE * y, World.TILE_SIZE, World.TILE_SIZE)
        Dim tileSize As Integer = World.TILE_SIZE

        Dim tile As New Bitmap(tileSize, tileSize, Imaging.PixelFormat.Format32bppArgb)

        Using g As Graphics = Graphics.FromImage(tile)
            g.InterpolationMode = Drawing2D.InterpolationMode.NearestNeighbor
            g.PixelOffsetMode = Drawing2D.PixelOffsetMode.Half

            g.DrawImage(map, New Rectangle(0, 0, tileSize, tileSize), sprite, GraphicsUnit.Pixel)
        End Using

        Return tile

    End Function
    Public Sub CreateLevel()
        For i = 0 To 7
            For j = 0 To 7
                level(New Point(i, j)) = GetTileFromPosition(i, j, level1)
                levelRenderLast(New Point(i, j)) = GetTileFromPosition(i, j, bottomGate)
            Next
        Next

        If (Not loadedWithSuccess Or world.Immovables.All.Count = 0) Then
            Dim T As Integer = World.TILE_SIZE / 2
            Dim H As Integer = T \ 2
            CreateMapCollisionBox(
            New PointF(
                T + H,                 ' center X = left wall column
                T * 4 + (T * 9) / 2 - H + 16 ' center Y
            ),
            New Point(T, T * 13)
            )
            CreateMapCollisionBox(
                New PointF(
                    T * 15 - 16,
                    T * 4 + (T * 9) / 2 - H + 16
                ),
                New Point(T, T * 13)
            )
            CreateMapCollisionBox(
                New PointF(
                    T * 4 + (T * 9) / 2 - H,
                    T + H - 48
                ),
                New Point(T * 13, T)
            )
            CreateMapCollisionBox(
                New PointF(
                    T * 4 + (T * 9) / 2 - 40,
                    T * 15 - H + 48
                ),
                New Point(T * 13 - 64 - 16, T)
            )

            ''Left-Upper Corner
            'CreateMapCollisionBox(New Point(576, 960), New Point(World.TILE_SIZE / 2, World.TILE_SIZE / 2))
            'CreateMapCollisionBox(New Point(704, 832), New Point(World.TILE_SIZE / 2, World.TILE_SIZE / 2))
            'CreateMapCollisionBox(New Point(832, 704), New Point(World.TILE_SIZE / 2, World.TILE_SIZE / 2))
            'CreateMapCollisionBox(New Point(960, 576), New Point(World.TILE_SIZE / 2, World.TILE_SIZE / 2))
            ''Right-Upper Corner
            'CreateMapCollisionBox(New Point(3520, 960), New Point(World.TILE_SIZE / 2, World.TILE_SIZE / 2))
            'CreateMapCollisionBox(New Point(3392, 832), New Point(World.TILE_SIZE / 2, World.TILE_SIZE / 2))
            'CreateMapCollisionBox(New Point(3264, 704), New Point(World.TILE_SIZE / 2, World.TILE_SIZE / 2))
            'CreateMapCollisionBox(New Point(3136, 576), New Point(World.TILE_SIZE / 2, World.TILE_SIZE / 2))
            ''Left-Down Corner
            'CreateMapCollisionBox(New Point(960, 3520), New Point(World.TILE_SIZE / 2, World.TILE_SIZE / 2))
            'CreateMapCollisionBox(New Point(832, 3392), New Point(World.TILE_SIZE / 2, World.TILE_SIZE / 2))
            'CreateMapCollisionBox(New Point(704, 3264), New Point(World.TILE_SIZE / 2, World.TILE_SIZE / 2))
            'CreateMapCollisionBox(New Point(576, 3136), New Point(World.TILE_SIZE / 2, World.TILE_SIZE / 2))
            ''Right-Down Corner
            'CreateMapCollisionBox(New Point(3136, 3520), New Point(World.TILE_SIZE / 2, World.TILE_SIZE / 2))
            'CreateMapCollisionBox(New Point(3264, 3392), New Point(World.TILE_SIZE / 2, World.TILE_SIZE / 2))
            'CreateMapCollisionBox(New Point(3392, 3264), New Point(World.TILE_SIZE / 2, World.TILE_SIZE / 2))
            'CreateMapCollisionBox(New Point(3520, 3136), New Point(World.TILE_SIZE / 2, World.TILE_SIZE / 2))
        End If
    End Sub
End Class
