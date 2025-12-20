Imports Game.My.Resources

Public Class Game
    Private world As World
    Private g As Graphics
    Private gameState As GameState
    Public level As New Dictionary(Of Point, Bitmap)

    Public Sub New(g As Graphics, input As InputState)
        Me.world = New World(g, input, Me)
        Me.g = g
        Me.gameState = GameState.Playing
        CreateTestWorld()
    End Sub

    Public Sub CreateTestWorld()
        CreateLevel()
        world.CreatePlayer()
        world.CreateCamera()
        CreateEnemiesAroundPoint(0, 0, 1)
        CreateEnemiesAroundPoint(400, 0, 1)
    End Sub

    Public Sub CreateLevel()
        Dim tileBmp As Bitmap = GameResources.worldTile128
        For y = 0 To 0
            For x = 2 To 3
                level(New Point(x, y)) = tileBmp
            Next
        Next
        For y = 1 To 3
            For x = 0 To 5
                level(New Point(x, y)) = tileBmp
            Next
        Next
        For y = 4 To 4
            For x = 2 To 3
                level(New Point(x, y)) = tileBmp
            Next
        Next
        For y = 5 To 9
            For x = 0 To 5
                level(New Point(x, y)) = tileBmp
            Next
        Next
        For y = 10 To 10
            For x = 2 To 3
                level(New Point(x, y)) = tileBmp
            Next
        Next
    End Sub

    Public Sub GameOver()
        Debug.WriteLine("GameOver")
        gameState = GameState.GameOver
    End Sub

    Public Sub CreateEnemiesAroundPoint(posX As Integer, posY As Integer, numEnemies As Integer)
        For i = 1 To numEnemies
            world.CreateEnemy(New PointF(
                posX + Random.Shared.Next(-200, 200),
                posY + Random.Shared.Next(-200, 200)
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

    Public Sub Draw()
        Select Case gameState
            Case GameState.Playing
                world.Draw()
            Case GameState.GameOver
                g.Clear(Color.Beige)
        End Select
    End Sub
End Class
