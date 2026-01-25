Imports System.Drawing

Public Class TutorialScreen
    Private game As Game
    Private cards As New List(Of Bitmap)
    Private currentIndex As Integer = 0

    Private leftArrowRect As Rectangle
    Private rightArrowRect As Rectangle

    Private btnBackRect As Rectangle
    Private btnStartRect As Rectangle

    Private imgBtnStart As Bitmap
    Private imgBtnBack As Bitmap

    Private imgTutorialBG As Bitmap

    Public Sub New(gameInstance As Game)
        Me.game = gameInstance
        LoadResources()
    End Sub

    Private Sub LoadResources()
        cards.Add(My.Resources.GameResources.movementCART)
        cards.Add(My.Resources.GameResources.tiroCART)
        cards.Add(My.Resources.GameResources.menuCART)

        imgBtnStart = My.Resources.GameResources.btnNOVOJOGO
        imgBtnBack = My.Resources.GameResources.btnSAIR

        imgTutorialBG = My.Resources.GameResources.MAINmenu
    End Sub

    Public Sub Draw(g As Graphics, screenWidth As Integer, screenHeight As Integer)
        If imgTutorialBG IsNot Nothing Then
            g.DrawImage(imgTutorialBG, 0, 0, screenWidth, screenHeight)
        Else
            Using brush As New SolidBrush(Color.FromArgb(230, 20, 20, 40))
                g.FillRectangle(brush, 0, 0, screenWidth, screenHeight)
            End Using
        End If

        If cards.Count = 0 Then Return

        Dim currentCard = cards(currentIndex)
        Dim scale As Single = 1.0F
        If currentCard.Width > screenWidth * 0.8 Then scale = (screenWidth * 0.8) / currentCard.Width

        Dim cardW As Integer = CInt(currentCard.Width * scale)
        Dim cardH As Integer = CInt(currentCard.Height * scale)

        Dim centerX As Integer = (screenWidth - cardW) \ 2
        Dim centerY As Integer = (screenHeight - cardH) \ 2

        If currentIndex > 0 Then
            Dim prevCard = cards(currentIndex - 1)
            Dim prevX As Integer = centerX - cardW - 50
            g.DrawImage(prevCard, prevX, centerY, cardW, cardH)

            Using brush As New SolidBrush(Color.FromArgb(150, 0, 0, 0))
                g.FillRectangle(brush, prevX, centerY, cardW, cardH)
            End Using
        End If

        If currentIndex < cards.Count - 1 Then
            Dim nextCard = cards(currentIndex + 1)
            Dim nextX As Integer = centerX + cardW + 50
            g.DrawImage(nextCard, nextX, centerY, cardW, cardH)

            Using brush As New SolidBrush(Color.FromArgb(150, 0, 0, 0))
                g.FillRectangle(brush, nextX, centerY, cardW, cardH)
            End Using
        End If

        g.DrawImage(currentCard, centerX, centerY, cardW, cardH)
        Using pen As New Pen(Color.Gold, 4)
            g.DrawRectangle(pen, centerX, centerY, cardW, cardH)
        End Using

        leftArrowRect = New Rectangle(50, centerY + (cardH \ 2) - 25, 50, 50)
        rightArrowRect = New Rectangle(screenWidth - 100, centerY + (cardH \ 2) - 25, 50, 50)

        If currentIndex > 0 Then
            g.FillPolygon(Brushes.White, {New Point(leftArrowRect.Right, leftArrowRect.Top), New Point(leftArrowRect.Left, leftArrowRect.Top + 25), New Point(leftArrowRect.Right, leftArrowRect.Bottom)})
        End If

        If currentIndex < cards.Count - 1 Then
            g.FillPolygon(Brushes.White, {New Point(rightArrowRect.Left, rightArrowRect.Top), New Point(rightArrowRect.Right, rightArrowRect.Top + 25), New Point(rightArrowRect.Left, rightArrowRect.Bottom)})
        End If


        Dim btnY As Integer = screenHeight - 100
        Dim btnW As Integer = 200
        Dim btnH As Integer = 50

        btnBackRect = New Rectangle(centerX, btnY, btnW, btnH)
        If imgBtnBack IsNot Nothing Then
            Dim srcRect As New Rectangle(0, 0, imgBtnBack.Width, imgBtnBack.Height / 3)
            g.DrawImage(imgBtnBack, btnBackRect, srcRect, GraphicsUnit.Pixel)
        End If

        btnStartRect = New Rectangle(centerX + cardW - btnW, btnY, btnW, btnH)
        If imgBtnStart IsNot Nothing Then
            Dim srcRect As New Rectangle(0, 0, imgBtnStart.Width, imgBtnStart.Height / 3)
            g.DrawImage(imgBtnStart, btnStartRect, srcRect, GraphicsUnit.Pixel)
        End If

    End Sub

    Public Sub HandleClick(mousePos As Point)
        If leftArrowRect.Contains(mousePos) AndAlso currentIndex > 0 Then
            currentIndex -= 1
        End If

        If rightArrowRect.Contains(mousePos) AndAlso currentIndex < cards.Count - 1 Then
            currentIndex += 1
        End If

        If btnBackRect.Contains(mousePos) Then
            game.gameState = GameState.Starting
        End If

        If btnStartRect.Contains(mousePos) Then
            game.StartNewGame()
        End If
    End Sub
End Class
