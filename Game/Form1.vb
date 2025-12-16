Public Class Form1
    Private input As InputState
    Private world As World
    Private lastTime As DateTime
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.KeyPreview = True
        input = New InputState(False, False, False, False)
        world = New World(Me.CreateGraphics(), input)
        'world.CreatePlayer()
        'world.CreateEnemy()
        world.CreateBullet(New PointF(0, 0))
        lastTime = DateTime.Now
        Timer1.Interval = 5
        Timer1.Start()
    End Sub

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        Dim now = DateTime.Now
        Dim dt = CSng((now - lastTime).TotalSeconds)
        lastTime = now

        world.Update(dt)
        world.Draw()
    End Sub

    Private Sub Form1_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
        Select Case e.KeyCode
            Case Keys.W, Keys.Up : input.up = True
            Case Keys.A, Keys.Left : input.left = True
            Case Keys.S, Keys.Down : input.down = True
            Case Keys.D, Keys.Right : input.right = True
        End Select
    End Sub

    Private Sub Form1_KeyUp(sender As Object, e As KeyEventArgs) Handles Me.KeyUp
        Select Case e.KeyCode
            Case Keys.W, Keys.Up : input.up = False
            Case Keys.A, Keys.Left : input.left = False
            Case Keys.S, Keys.Down : input.down = False
            Case Keys.D, Keys.Right : input.right = False
        End Select
    End Sub
End Class
