Imports System.Drawing
Imports Windows.Win32.UI.Input

Public Class TutorialScreen
    Private game As Game

    Private cards As New List(Of UICard)
    Private currentIndex As Integer = 0

    Public buttons As New List(Of UIButton)

    Private leftArrowRect As Rectangle
    Private rightArrowRect As Rectangle
    Private imgTutorialBG As Bitmap
    Private imgArrowLeft As Bitmap
    Private imgArrowRight As Bitmap
    Private imgBtnBack As Bitmap

    Public Sub New(gameInstance As Game)
        Me.game = gameInstance
        LoadResources()
        InitializeButtons()
    End Sub

    Private Sub LoadResources()
        imgTutorialBG = My.Resources.GameResources.MAINmenu

        cards.Add(New UICard(My.Resources.GameResources.movementCART))
        cards.Add(New UICard(My.Resources.GameResources.tiroCART))
        cards.Add(New UICard(My.Resources.GameResources.menuCART))

        imgArrowLeft = My.Resources.GameResources.btnESQUERDA
        imgArrowRight = My.Resources.GameResources.btnDIREITA
        imgBtnBack = My.Resources.GameResources.btnVOLTAR
    End Sub

    Private Sub InitializeButtons()
        Dim screenW As Integer = Form1.Width
        Dim screenH As Integer = Form1.Height
        Dim btnW As Integer = 200
        Dim btnH As Integer = 50
        Dim spacing As Integer = 40

        Dim btnY As Integer = screenH - 150
        Dim totalWidth As Integer = (btnW * 2) + spacing
        Dim startX As Integer = (screenW - totalWidth) \ 2

        buttons.Add(New UIButtonQuit With {
            .bounds = New Rectangle(startX, btnY, btnW, btnH),
            .text = "",
            .onClick = Sub() game.gameState = GameState.Starting,
            .sprite = imgBtnBack
        })


        buttons.Add(New UIButtonStartNewGame With {
            .bounds = New Rectangle(startX + btnW + spacing, btnY, btnW, btnH),
            .text = "",
            .onClick = Sub() game.StartNewGame()
        })
    End Sub

    Public Sub Draw(g As Graphics, world As World)
        g.Clear(Color.Black)

        Dim imgRatio As Single = world.game.bgc.Width / world.game.bgc.Height
        Dim formRatio As Single = Form1.Width / Form1.Height

        Dim drawW As Integer
        Dim drawH As Integer
        Dim screenWidth = Form1.Width
        Dim screenHeight = Form1.Height

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

        If cards.Count = 0 Then Return

        ' Layout
        Dim currentCard = cards(currentIndex)
        Dim scale As Single = 1.0F
        If currentCard.Width > screenWidth * 0.8 Then
            scale = (screenWidth * 0.8) / currentCard.Width
        End If

        Dim cardW As Integer = CInt(currentCard.Width * scale)
        Dim cardH As Integer = CInt(currentCard.Height * scale)
        Dim centerX As Integer = (screenWidth - cardW) \ 2
        Dim centerY As Integer = CInt(screenHeight * 0.1)

        If currentIndex > 0 Then
            Dim prevX As Integer = centerX - cardW - 50
            cards(currentIndex - 1).Draw(g, New Rectangle(prevX, centerY, cardW, cardH), False)
        End If

        If currentIndex < cards.Count - 1 Then
            Dim nextX As Integer = centerX + cardW + 50
            cards(currentIndex + 1).Draw(g, New Rectangle(nextX, centerY, cardW, cardH), False)
        End If

        cards(currentIndex).Draw(g, New Rectangle(centerX, centerY, cardW, cardH), True)

        leftArrowRect = New Rectangle(50, centerY + (cardH \ 2) - 25, 50, 50)
        rightArrowRect = New Rectangle(screenWidth - 100, centerY + (cardH \ 2) - 25, 50, 50)

        If currentIndex > 0 AndAlso imgArrowLeft IsNot Nothing Then
            g.DrawImage(imgArrowLeft, leftArrowRect)
        End If

        If currentIndex < cards.Count - 1 AndAlso imgArrowRight IsNot Nothing Then
            g.DrawImage(imgArrowRight, rightArrowRect)
        End If

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

    Public Sub HandleClick(mousePos As Point)
        If leftArrowRect.Contains(mousePos) AndAlso currentIndex > 0 Then
            currentIndex -= 1
        End If

        If rightArrowRect.Contains(mousePos) AndAlso currentIndex < cards.Count - 1 Then
            currentIndex += 1
        End If

        For Each btn In buttons
            If btn.bounds.Contains(mousePos) Then
                btn.onClick?.Invoke()
            End If
        Next
    End Sub
End Class