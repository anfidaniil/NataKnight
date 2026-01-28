Public Class MenuScreen
    Public buttons As New List(Of UIButton)
    Private game As Game

    Public Sub New(gameInstance As Game, restart As Action, tutorial As Action, continueAction As Action, backToMenu As Action)
        Me.game = gameInstance

        Dim screenW As Integer = Form1.Width
        Dim screenH As Integer = Form1.Height
        Dim scale As Single = game.GetUIElementScale()

        Dim buttonWidth As Integer = CInt(200 * scale)
        Dim buttonHeight As Integer = CInt(50 * scale)
        Dim gap As Integer = CInt(20 * scale)
        Dim rowGap As Integer = CInt(90 * scale)

        Dim centerX As Integer = screenW \ 2
        Dim centerY As Integer = screenH \ 2

        buttons.Add(New UIButtonContinue With {
            .bounds = New Rectangle(centerX - buttonWidth - gap, centerY, buttonWidth, buttonHeight),
            .text = "Continue",
            .onClick = continueAction
        })

        buttons.Add(New UIButtonStartNewGame With {
            .bounds = New Rectangle(centerX + gap, centerY, buttonWidth, buttonHeight),
            .text = "Start New Game",
            .onClick = restart
        })

        buttons.Add(New UIButtonTutorial With {
            .bounds = New Rectangle(centerX - buttonWidth - gap, centerY + rowGap, buttonWidth, buttonHeight),
            .text = "Tutorial",
            .onClick = tutorial
        })

        buttons.Add(New UIButtonBackToMenu With {
            .bounds = New Rectangle(centerX + gap, centerY + rowGap, buttonWidth, buttonHeight),
            .text = "Menu",
            .onClick = backToMenu
        })
    End Sub

    Public Sub Draw(g As Graphics, world As World)
        g.InterpolationMode = Drawing2D.InterpolationMode.NearestNeighbor
        g.PixelOffsetMode = Drawing2D.PixelOffsetMode.Half

        Using overlayBrush As New SolidBrush(Color.FromArgb(150, 0, 0, 0))
            g.FillRectangle(
            overlayBrush,
            0,
            0,
            Form1.Width,
            Form1.Height
        )
        End Using

        Using font As New Font("Arial", 24, FontStyle.Bold)
            Dim text = "PAUSA"
            Dim size = g.MeasureString(text, font)
            g.DrawString(text, font, Brushes.White,
                (Form1.Width - size.Width) / 2, 100)
        End Using

        For Each btn In buttons
            If (btn.sprite IsNot Nothing) Then
                g.DrawImage(btn.sprite, btn.bounds)
            Else
                g.FillRectangle(Brushes.DarkGray, btn.bounds)
                g.DrawRectangle(Pens.White, btn.bounds)

                Using font As New Font("Arial", 16, FontStyle.Bold)
                    Dim size = g.MeasureString(btn.text, font)
                    g.DrawString(btn.text, font, Brushes.White,
                        btn.bounds.X + (btn.bounds.Width - size.Width) \ 2,
                        btn.bounds.Y + (btn.bounds.Height - size.Height) \ 2)
                End Using
            End If
        Next
    End Sub

    Public Sub HandleMouseClick(mousePos As Point)
        For Each btn In buttons
            If btn.bounds.Contains(mousePos) Then
                btn.onClick?.Invoke()
            End If
        Next
    End Sub
End Class
