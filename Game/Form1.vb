Public Class Form1
    Public input As InputState
    Private game As Game
    Private lastTime As DateTime

    Private isDebug As Boolean = True
    Private count As Integer = 0
    Private fps As Integer = 0
    Private lastCheck As Date

    Public Sub CalculateFPS()
        fps = count
        count = 0
        Debug.WriteLine("FPS: " & fps)
    End Sub

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.KeyPreview = True
        input = New InputState(False, False, False, False, False, New Point(0, 0))
        Me.SetStyle(
            ControlStyles.AllPaintingInWmPaint Or
            ControlStyles.UserPaint Or
            ControlStyles.OptimizedDoubleBuffer,
            True
        )
        Me.UpdateStyles()
        game = New Game(input)
        lastTime = DateTime.Now
        Timer1.Interval = 10
        Timer1.Start()
    End Sub

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        Dim now = DateTime.Now
        Dim dt = (now - lastTime).TotalSeconds
        dt = Math.Min(dt, 0.05F)
        lastTime = now

        game.Update(dt)
        Me.Invalidate()
    End Sub

    Private Sub Form1_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
        Select Case e.KeyCode
            Case Keys.W, Keys.Up : input.up = True
            Case Keys.A, Keys.Left : input.left = True
            Case Keys.S, Keys.Down : input.down = True
            Case Keys.D, Keys.Right : input.right = True
            Case Keys.Space : input.fire = True
        End Select
    End Sub

    Private Sub Form1_KeyUp(sender As Object, e As KeyEventArgs) Handles Me.KeyUp
        Select Case e.KeyCode
            Case Keys.W, Keys.Up : input.up = False
            Case Keys.A, Keys.Left : input.left = False
            Case Keys.S, Keys.Down : input.down = False
            Case Keys.D, Keys.Right : input.right = False
            Case Keys.Space : input.fire = False
        End Select
    End Sub

    Private Sub Form1_MouseDown(sender As Object, e As MouseEventArgs) Handles Me.MouseDown
        'Doesn't work when pressing left mouse button
        'Only works when clicking the right button
        input.fire = True

        If game.gameState = GameState.GameOver Then
            game.gameOverUI.HandleMouseClick(e.Location)
        End If
    End Sub

    Private Sub Form1_MouseUP(sender As Object, e As MouseEventArgs) Handles Me.MouseUp
        input.fire = False
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
        End If
    End Sub
End Class
