Public Class Form1
    Public input As InputState
    Private game As Game
    Private lastTime As DateTime

    Private isDebug As Boolean = False
    Private count As Integer = 0
    Private fps As Integer = 0
    Private lastCheck As Date
    Private accumulator As Double = 0
    Private Const FIXED_DT As Double = 0.02 ' 5Hz

    Private isSpaceDown As Boolean = False
    Private isMouseDown As Boolean = False

    Private running As Boolean = False

    Private Sub UpdateFireState()
        If input IsNot Nothing Then
            input.fire = (isSpaceDown OrElse isMouseDown)
        End If
    End Sub

    Public Sub CalculateFPS()
        fps = count
        count = 0
        Debug.WriteLine("FPS: " & fps)
    End Sub

    Private Sub OnSize_Changed(sender As Object, e As EventArgs) Handles MyBase.SizeChanged
        If Not game Is Nothing Then
            game.ChangeCameraView()
        End If
    End Sub

    Public Sub OnClose() Handles Me.Closed
        running = False
        GameStateSerialization.SaveToFile(game, "data.json")
    End Sub
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.Text = "Nata Knight"

        Me.KeyPreview = True

        input = New InputState(False, False, False, False, False, New Point(0, 0))

        Me.SetStyle(
        ControlStyles.AllPaintingInWmPaint Or
        ControlStyles.UserPaint Or
        ControlStyles.OptimizedDoubleBuffer,
        True
    )

        game = New Game(input)

        lastTime = DateTime.Now

        running = True
        BeginInvoke(New Action(AddressOf GameLoop))
    End Sub

    Private Sub GameLoop()
        While running
            Application.DoEvents()

            Dim now = DateTime.Now
            Dim frameTime = (now - lastTime).TotalSeconds
            lastTime = now

            If frameTime > 0.1 Then frameTime = 0.1
            accumulator += frameTime

            While accumulator >= FIXED_DT
                game.Update(CSng(FIXED_DT))
                accumulator -= FIXED_DT
            End While

            Invalidate()
            Update()

            Threading.Thread.Sleep(1)
        End While
    End Sub

    Private Sub Form1_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
        Select Case e.KeyCode
            Case Keys.W, Keys.Up : input.up = True
            Case Keys.A, Keys.Left : input.left = True
            Case Keys.S, Keys.Down : input.down = True
            Case Keys.D, Keys.Right : input.right = True
            Case Keys.Space
                isSpaceDown = True
                UpdateFireState()

            Case Keys.E
                If game.gameState = GameState.GameOver Then
                    Return
                End If
                If game.gameState = GameState.Playing Then
                    game.gameState = GameState.Menu
                ElseIf game.gameState = GameState.Menu Then
                    game.gameState = GameState.Playing
                End If
        End Select
    End Sub

    Private Sub Form1_KeyUp(sender As Object, e As KeyEventArgs) Handles Me.KeyUp
        Select Case e.KeyCode
            Case Keys.W, Keys.Up : input.up = False
            Case Keys.A, Keys.Left : input.left = False
            Case Keys.S, Keys.Down : input.down = False
            Case Keys.D, Keys.Right : input.right = False
            Case Keys.Space
                isSpaceDown = False
                UpdateFireState()
        End Select
    End Sub

    Private Sub Form1_MouseDown(sender As Object, e As MouseEventArgs) Handles Me.MouseDown
        If e.Button = MouseButtons.Left Then
            isMouseDown = True
            UpdateFireState()
        End If

        Select Case game.gameState
            Case GameState.GameOver
                game.gameOverUI.HandleMouseClick(e.Location)
            Case GameState.Menu
                game.menuScreen.HandleMouseClick(e.Location)
            Case GameState.Starting
                game.startingMenuScreen.HandleMouseClick(e.Location)
            Case GameState.Tutorial
                game.tutorialScreen.HandleClick(e.Location)
        End Select
    End Sub

    Private Sub Form1_MouseUP(sender As Object, e As MouseEventArgs) Handles Me.MouseUp
        If e.Button = MouseButtons.Left Then
            isMouseDown = False
            UpdateFireState()
        End If
    End Sub

    Protected Overrides Sub OnPaint(e As PaintEventArgs)
        MyBase.OnPaint(e)

        If game IsNot Nothing Then
            game.Draw(e.Graphics)
        End If
        count += 1

        If isDebug AndAlso (DateTime.Now - lastCheck).TotalSeconds >= 1 Then
            fps = count
            count = 0
            lastCheck = DateTime.Now
            Debug.WriteLine("FPS: " & fps)
            Debug.WriteLine("Score: " & game.score)
        End If
    End Sub
End Class
