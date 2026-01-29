Imports Windows.Win32.UI.Input

Public Class StartScreen
    Public buttons As New List(Of UIButton)
    Private game As Game

    Public Sub New(gameInstance As Game, restart As Action, quit As Action, tutorial As Action)
        Me.game = gameInstance

        Dim screenW As Integer = Form1.Width
        Dim screenH As Integer = Form1.Height

        Dim scale As Single = game.GetUIElementScale()

        Dim buttonWidth As Integer = CInt(200 * scale)
        Dim buttonHeight As Integer = CInt(50 * scale)
        Dim gap As Integer = CInt(10 * scale)
        Dim verticalGap As Integer = CInt(20 * scale)

        Dim totalGroupWidth As Integer = (buttonWidth * 2) + gap
        Dim totalGroupHeight As Integer = (buttonHeight * 2) + verticalGap

        Dim offsetY As Integer = CInt(50 * scale)

        Dim startX As Integer = (screenW - totalGroupWidth) \ 2
        Dim startY As Integer = (screenH - totalGroupHeight) \ 2 + offsetY


        buttons.Add(New UIButtonStartNewGame With {
            .bounds = New Rectangle(startX, startY, buttonWidth, buttonHeight),
            .text = "",
            .onClick = restart
        })

        buttons.Add(New UIButtonTutorial With {
            .bounds = New Rectangle(startX + buttonWidth + gap, startY, buttonWidth, buttonHeight),
            .text = "",
            .onClick = tutorial
        })

        Dim centerX As Integer = screenW \ 2
        buttons.Add(New UIButtonQuit With {
            .bounds = New Rectangle(centerX - (buttonWidth \ 2), startY + buttonHeight + verticalGap, buttonWidth, buttonHeight),
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

        g.InterpolationMode = Drawing2D.InterpolationMode.NearestNeighbor
        g.PixelOffsetMode = Drawing2D.PixelOffsetMode.Half

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
