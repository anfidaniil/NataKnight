Imports Windows.Win32.UI.Input
Imports System.Drawing.Imaging

Public Class StartScreen
    Public buttons As New List(Of UIButton)
    Private game As Game

    Public Sub New(gameInstance As Game, restart As Action, continueAction As Action, tutorial As Action, quit As Action)
        Me.game = gameInstance

        Dim screenW As Integer = Form1.Width
        Dim screenH As Integer = Form1.Height

        Dim scale As Single = game.GetUIElementScale()

        Dim buttonWidth As Integer = CInt(200 * scale)
        Dim buttonHeight As Integer = CInt(50 * scale)
        Dim gap As Integer = CInt(10 * scale)
        Dim verticalGap As Integer = CInt(20 * scale)

        Dim row1Width As Integer = (buttonWidth * 3) + (gap * 2)
        Dim row1StartX As Integer = (screenW - row1Width) \ 2

        Dim row2Width As Integer = (buttonWidth * 2) + gap
        Dim row2StartX As Integer = (screenW - row2Width) \ 2

        Dim totalHeight As Integer = (buttonHeight * 2) + verticalGap
        Dim startY As Integer = (screenH - totalHeight) \ 2 + CInt(50 * scale)

        Dim row1Y As Integer = startY
        Dim row2Y As Integer = startY + buttonHeight + verticalGap

        buttons.Add(New UIButtonStartNewGame With {
            .bounds = New Rectangle(row1StartX, row1Y, buttonWidth, buttonHeight),
            .text = "",
            .onClick = restart
        })

        Dim btnContinue As New UIButtonContinue With {
            .bounds = New Rectangle(row1StartX + buttonWidth + gap, row1Y, buttonWidth, buttonHeight),
            .text = "",
            .onClick = continueAction
        }

        If Not game.loadedWithSuccess Then
            If btnContinue.sprite IsNot Nothing Then
                btnContinue.sprite = ToGrayscale(btnContinue.sprite)
            End If
        End If
        buttons.Add(btnContinue)

        buttons.Add(New UIButtonTutorial With {
            .bounds = New Rectangle(row1StartX + (buttonWidth + gap) * 2, row1Y, buttonWidth, buttonHeight),
            .text = "",
            .onClick = tutorial
        })

        buttons.Add(New UIButton With {
            .bounds = New Rectangle(row2StartX, row2Y, buttonWidth, buttonHeight),
            .text = "Acerca",
            .onClick = Sub()
                           AudioEngine.PlayOneShot("button_ui_2", 1.0F)
                       End Sub
        })

        buttons.Add(New UIButtonQuit With {
            .bounds = New Rectangle(row2StartX + buttonWidth + gap, row2Y, buttonWidth, buttonHeight),
            .text = "",
            .onClick = quit
        })
    End Sub

    Private Function ToGrayscale(original As Bitmap) As Bitmap
        Dim newBitmap As New Bitmap(original.Width, original.Height)

        Using g As Graphics = Graphics.FromImage(newBitmap)
            Dim colorMatrix As New ColorMatrix(New Single()() {
                New Single() {0.3, 0.3, 0.3, 0, 0},
                New Single() {0.59, 0.59, 0.59, 0, 0},
                New Single() {0.11, 0.11, 0.11, 0, 0},
                New Single() {0, 0, 0, 1, 0},
                New Single() {0, 0, 0, 0, 1}
            })

            Dim attributes As New ImageAttributes()
            attributes.SetColorMatrix(colorMatrix)

            g.DrawImage(original, New Rectangle(0, 0, original.Width, original.Height),
                        0, 0, original.Width, original.Height, GraphicsUnit.Pixel, attributes)
        End Using

        Return newBitmap
    End Function

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
