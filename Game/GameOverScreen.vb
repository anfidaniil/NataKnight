Imports System.Drawing.Design
Imports System.Drawing.Imaging

Public Class GameOverScreen
    Public buttons As New List(Of UIButton)
    Dim buttonWidth = 200
    Dim buttonHeight = 50

    Public Sub New(screenWidth As Integer, screenHeight As Integer, restart As Action, quit As Action)
        Dim centerX = screenWidth \ 2 - buttonWidth
        Dim centerY = screenHeight \ 2

        buttons.Add(New UIButton With {
            .bounds = New Rectangle(centerX - 80, centerY, buttonWidth, buttonHeight),
            .text = "Restart",
            .onClick = restart
        })
        buttons.Add(New UIButton With {
            .bounds = New Rectangle(centerX + buttonWidth + 80, centerY, buttonWidth, buttonHeight),
            .text = "Quit",
            .onClick = quit
        })
    End Sub

    Public Sub Draw(g As Graphics, world As World)

        Dim posX = Form1.Width \ 4
        Dim posY = Form1.Height \ 4
        Dim sizeX = Form1.Width \ 2
        Dim sizeY = Form1.Height \ 2

        g.FillRectangle(New SolidBrush(Color.Beige), posX, posY, sizeX, sizeY)

        For Each btn In buttons
            g.FillRectangle(Brushes.DarkGray, btn.bounds)
            g.DrawRectangle(Pens.White, btn.bounds)

            Using font As New Font("Arial", 14, FontStyle.Bold)
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
