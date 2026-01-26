Imports Windows.Win32.UI.Input

Public Class StartScreen
    Public buttons As New List(Of UIButton)
    Dim buttonWidth = 200
    Dim buttonHeight = 50

    Public Sub New(screenWidth As Integer, screenHeight As Integer, restart As Action, quit As Action, tutorial As Action)
        Dim centerX = (screenWidth - buttonWidth) \ 2
        Dim centerY = screenHeight \ 2

        Dim gap As Integer = 10


        buttons.Add(New UIButtonStartNewGame With {
            .bounds = New Rectangle(centerX - (buttonWidth \ 2) - gap, centerY, buttonWidth, buttonHeight),
            .text = "",
            .onClick = restart
        })

        buttons.Add(New UIButtonTutorial With {
            .bounds = New Rectangle(centerX + (buttonWidth \ 2) + gap, centerY, buttonWidth, buttonHeight),
            .text = "",
            .onClick = tutorial
        })

        buttons.Add(New UIButtonQuit With {
            .bounds = New Rectangle(centerX, centerY + buttonHeight + 20, buttonWidth, buttonHeight),
            .text = "",
            .onClick = quit
        })

        ' Botão Continuar (Desativado)
        ' buttons.Add(New UIButtonContinue With {
        '     .bounds = New Rectangle(centerX, centerY + (buttonHeight * 2) + 40, buttonWidth, buttonHeight),
        '     .text = "Continue",
        '     .onClick = restart
        ' })
    End Sub

    Public Sub Draw(g As Graphics, world As World)
        g.Clear(Color.Black)

        Dim imgRatio As Single = world.game.bgc.Width / world.game.bgc.Height
        Dim formRatio As Single = Form1.Width / Form1.Height

        Dim drawW As Integer
        Dim drawH As Integer

        If formRatio > imgRatio Then
            drawH = Form1.Height
            drawW = CInt(Form1.Height * imgRatio)
        Else
            drawW = Form1.Width
            drawH = CInt(Form1.Width / imgRatio)
        End If

        ' Center the image
        Dim x As Integer = (Form1.Width - drawW) \ 2
        Dim y As Integer = (Form1.Height - drawH) \ 2

        g.DrawImage(world.game.bgc, x, y, drawW, drawH)

        Using font As New Font("Arial", 24, FontStyle.Bold)
            Dim text = World.GAME_NAME
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
