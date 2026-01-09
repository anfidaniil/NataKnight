Public Class StartScreen
    Public buttons As New List(Of UIButton)
    Dim buttonWidth = 200
    Dim buttonHeight = 50

    Public Sub New(screenWidth As Integer, screenHeight As Integer, restart As Action, quit As Action)
        Dim centerX = (screenWidth - buttonWidth) \ 2
        Dim centerY = screenHeight \ 2

        buttons.Add(New UIButton With {
            .bounds = New Rectangle(centerX - buttonWidth / 2 - 20, centerY, buttonWidth, buttonHeight),
            .text = "Start New Game",
            .onClick = restart
        })

        buttons.Add(New UIButton With {
            .bounds = New Rectangle(centerX + buttonWidth / 2 + 20, centerY, buttonWidth, buttonHeight),
            .text = "Quit",
            .onClick = quit
        })
    End Sub

    Public Sub Draw(g As Graphics, world As World)
        Using overlayBrush As New SolidBrush(Color.FromArgb(255, 0, 0, 0))
            g.FillRectangle(
            overlayBrush,
            0,
            0,
            Form1.Width,
            Form1.Height
        )
        End Using

        Using font As New Font("Arial", 24, FontStyle.Bold)
            Dim text = world.GAME_NAME
            Dim size = g.MeasureString(text, font)
            g.DrawString(text, font, Brushes.White,
                (Form1.Width - size.Width) / 2, 100)
        End Using

        For Each btn In buttons
            g.FillRectangle(Brushes.DarkGray, btn.bounds)
            g.DrawRectangle(Pens.White, btn.bounds)

            Using font As New Font("Arial", 16, FontStyle.Bold)
                Dim size = g.MeasureString(btn.text, font)
                g.DrawString(btn.text, font, Brushes.White,
                    btn.bounds.X + (btn.bounds.Width - size.Width) \ 2,
                    btn.bounds.Y + (btn.bounds.Height - size.Height) \ 2)
            End Using
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
