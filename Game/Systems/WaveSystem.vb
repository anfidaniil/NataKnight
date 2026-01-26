Imports System.Drawing
Imports System.Linq
Imports System.Drawing.Drawing2D

Public Class WaveSystem
    Implements ISystem

    Private Const SAFE_MIN As Integer = 1300
    Private Const SAFE_MAX As Integer = 2800

    Private currentState As WaveState = WaveState.FadingIn

    Private roundNumber As Integer = 1
    Private targetEnemies As Integer = 5
    Private enemiesSpawnedInThisRound As Integer = 0
    Private rng As New Random()

    Private opacity As Single = 0
    Private fadeSpeed As Single = 1.5F
    Private holdTimer As Single = 0
    Private holdDuration As Single = 2.0F
    Private showTextDuration As Single = 2.0F

    Private spawnTimer As Single = 0
    Private Const SPAWN_DELAY As Single = 0.5F

    Public Sub Update(world As World, dt As Single) Implements ISystem.Update

        Select Case currentState

            Case WaveState.FadingIn
                opacity += fadeSpeed * dt
                If opacity >= 1.0F Then
                    opacity = 1.0F
                    currentState = WaveState.Holding
                    holdTimer = holdDuration
                End If

            Case WaveState.Holding
                holdTimer -= dt
                If holdTimer <= 0 Then
                    currentState = WaveState.FadingOut
                End If

            Case WaveState.FadingOut
                opacity -= fadeSpeed * dt
                If opacity <= 0.0F Then
                    opacity = 0.0F
                    currentState = WaveState.Spawning
                    enemiesSpawnedInThisRound = 0
                    spawnTimer = 0
                End If

            Case WaveState.Spawning
                spawnTimer -= dt

                If spawnTimer <= 0 Then
                    If enemiesSpawnedInThisRound < targetEnemies Then
                        If world.Players.HasComponent(world.PlayerID) AndAlso world.Transforms.HasComponent(world.PlayerID) Then

                            Dim playerPos = world.Transforms.GetComponent(world.PlayerID).pos
                            Dim spawnPos = GetRandomSpawnPos(playerPos)
                            world.CreateEnemy(spawnPos)

                            enemiesSpawnedInThisRound += 1
                            spawnTimer = SPAWN_DELAY
                        Else
                            Return
                            Debug.WriteLine("Tentativa de spawn sem jogador vivo")
                        End If

                    Else
                        currentState = WaveState.Playing
                    End If
                End If

            Case WaveState.Playing
                If world.Enemies.All.Count = 0 Then
                    StartNextRound()
                End If

        End Select

    End Sub

    Private Sub StartNextRound()
        roundNumber += 1

        Dim calculatedEnemies = 5 + (roundNumber - 1)
        targetEnemies = Math.Min(calculatedEnemies, 25)

        currentState = WaveState.FadingIn
    End Sub

    Private Function GetRandomSpawnPos(playerPos As PointF) As PointF

        Dim rX As Single = CSng((rng.NextDouble() * 2.0) - 1.0)
        Dim rY As Single = CSng((rng.NextDouble() * 2.0) - 1.0)

        Dim finalX As Single = (rX * 400) + playerPos.X + (Math.Sign(rX) * 400)
        Dim finalY As Single = (rY * 400) + playerPos.Y + (Math.Sign(rY) * 400)

        If finalX < SAFE_MIN Then
            finalX = SAFE_MIN + 200
        ElseIf finalX > SAFE_MAX Then
            finalX = SAFE_MAX - 200
        End If

        If finalY < SAFE_MIN Then
            finalY = SAFE_MIN + 200
        ElseIf finalY > SAFE_MAX Then
            finalY = SAFE_MAX - 200
        End If

        Return New PointF(finalX, finalY)
    End Function

    Public Sub Draw(world As World, g As Graphics) Implements ISystem.Draw
        If currentState = WaveState.FadingIn OrElse
           currentState = WaveState.Holding OrElse
           currentState = WaveState.FadingOut Then

            Dim text As String = "RONDA " & roundNumber

            Using font As New Font("Arial", 40, FontStyle.Bold)
                Dim textSize = g.MeasureString(text, font)

                Dim state = g.Save()
                g.ResetTransform()

                Dim xPos As Single = CSng((Form1.Width - textSize.Width) / 2)
                Dim yPos As Single = CSng(Form1.Height / 4)

                Dim alpha As Integer = 255

                If currentState = WaveState.FadingOut AndAlso opacity < 1.0F Then
                    alpha = CInt(opacity * 255)
                ElseIf currentState = WaveState.FadingIn Then
                    alpha = CInt(opacity * 255)
                End If

                alpha = Math.Max(0, Math.Min(255, alpha))

                Using shadowBrush As New SolidBrush(Color.FromArgb(alpha, Color.Black))
                    Using textBrush As New SolidBrush(Color.FromArgb(alpha, Color.White))
                        g.DrawString(text, font, shadowBrush, xPos + 4, yPos + 4)
                        g.DrawString(text, font, textBrush, xPos, yPos)
                    End Using
                End Using

                g.Restore(state)
            End Using
        End If
    End Sub
End Class