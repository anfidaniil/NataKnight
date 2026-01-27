Imports System.Drawing
Imports Windows.Win32.UI.Input

Public Class TutorialScreen
    Private game As Game

    Private cards As New List(Of UICard)
    Public buttons As New List(Of UIButton)
    Public currentIndex As Integer = 0

    Private btnLeft As UIButtonArrowLeft
    Private btnRight As UIButtonArrowRight

    Public BackAction As Action

    Private imgTutorialBG As Bitmap

    Public Sub New(gameInstance As Game, Optional startPage As Integer = 0)
        Me.game = gameInstance
        Me.currentIndex = startPage
        BackAction = Sub() game.gameState = GameState.Starting
        LoadResources()
        InitializeButtons()
    End Sub

    Private Sub LoadResources()
        imgTutorialBG = My.Resources.GameResources.MAINmenu

        cards.Add(New UICard(My.Resources.GameResources.movementCART))
        cards.Add(New UICard(My.Resources.GameResources.tiroCART))
        cards.Add(New UICard(My.Resources.GameResources.menuCART))
    End Sub

    Private Sub InitializeButtons()
        Dim screenW As Integer = Form1.Width
        Dim screenH As Integer = Form1.Height
        Dim scale As Single = 1.0F
        Dim bottomMargin As Integer = 150

        If Form1.WindowState = FormWindowState.Maximized Then
            scale = 2.0F
            bottomMargin = 250
        End If

        Dim btnW As Integer = CInt(200 * scale)
        Dim btnH As Integer = CInt(50 * scale)
        Dim spacing As Integer = CInt(40 * scale)
        Dim arrowSize As Integer = CInt(50 * scale)
        Dim sideMargin As Integer = CInt(50 * scale)
        Dim btnY As Integer = screenH - bottomMargin
        Dim totalWidth As Integer = (btnW * 2) + spacing
        Dim startX As Integer = (screenW - totalWidth) \ 2
        Dim arrowY As Integer = CInt(screenH * 0.4)

        btnLeft = New UIButtonArrowLeft With {
            .bounds = New Rectangle(sideMargin, arrowY, arrowSize, arrowSize),
            .onClick = Sub()
                           AudioEngine.PlayOneShot("button_ui_1", 1.0F)
                           If currentIndex > 0 Then currentIndex -= 1
                       End Sub
        }
        buttons.Add(btnLeft)

        btnRight = New UIButtonArrowRight With {
            .bounds = New Rectangle(screenW - sideMargin - arrowSize, arrowY, arrowSize, arrowSize),
            .onClick = Sub()
                           AudioEngine.PlayOneShot("button_ui_1", 1.0F)
                           If currentIndex < cards.Count - 1 Then currentIndex += 1
                       End Sub
        }
        buttons.Add(btnRight)

        buttons.Add(New UIButtonGoBack With {
            .bounds = New Rectangle(startX, btnY, btnW, btnH),
            .text = "",
            .onClick = Sub()
                           AudioEngine.PlayOneShot("button_ui_1", 1.0F)
                           BackAction?.Invoke()
                       End Sub
        })

        buttons.Add(New UIButtonStartNewGame With {
            .bounds = New Rectangle(startX + btnW + spacing, btnY, btnW, btnH),
            .text = "",
            .onClick = Sub()
                           AudioEngine.PlayOneShot("button_ui_1", 1.0F)
                           game.StartNewGame()
                       End Sub
        })
    End Sub

    Public Sub Draw(g As Graphics, world As World)
        g.Clear(Color.Black)

        g.InterpolationMode = Drawing2D.InterpolationMode.NearestNeighbor
        g.PixelOffsetMode = Drawing2D.PixelOffsetMode.Half

        If imgTutorialBG IsNot Nothing Then
            Dim imgRatio As Single = imgTutorialBG.Width / imgTutorialBG.Height
            Dim formRatio As Single = Form1.Width / Form1.Height
            Dim drawW, drawH As Integer

            If formRatio > imgRatio Then
                drawH = Form1.Height
                drawW = CInt(Form1.Height * imgRatio)
            Else
                drawW = Form1.Width
                drawH = CInt(Form1.Width / imgRatio)
            End If

            Dim x As Integer = (Form1.Width - drawW) \ 2
            Dim y As Integer = (Form1.Height - drawH) \ 2
            g.DrawImage(imgTutorialBG, x, y, drawW, drawH)
        End If

        If cards.Count = 0 Then Return

        Dim currentCard = cards(currentIndex)
        Dim scale As Single = 1.0F

        If Form1.WindowState = FormWindowState.Maximized Then
            scale = 2.0F
        Else
            scale = 1.0F
        End If

        Dim screenWidth = Form1.Width
        Dim screenHeight = Form1.Height

        If currentCard.sprite.Width > screenWidth * 0.8 Then
            scale = (screenWidth * 0.8) / currentCard.sprite.Width
        End If

        Dim cardW As Integer = CInt(currentCard.sprite.Width * scale)
        Dim cardH As Integer = CInt(currentCard.sprite.Height * scale)
        Dim centerX As Integer = (screenWidth - cardW) \ 2
        Dim centerY As Integer = CInt(screenHeight * 0.1)


        If currentIndex > 0 Then
            Dim prevCard = cards(currentIndex - 1)
            Dim prevRect As New Rectangle(centerX - cardW - 50, centerY, cardW, cardH)
            g.DrawImage(prevCard.sprite, prevRect)
            Using brush As New SolidBrush(Color.FromArgb(150, 0, 0, 0))
                g.FillRectangle(brush, prevRect)
            End Using
        End If

        If currentIndex < cards.Count - 1 Then
            Dim nextCard = cards(currentIndex + 1)
            Dim nextRect As New Rectangle(centerX + cardW + 50, centerY, cardW, cardH)
            g.DrawImage(nextCard.sprite, nextRect)
            Using brush As New SolidBrush(Color.FromArgb(150, 0, 0, 0))
                g.FillRectangle(brush, nextRect)
            End Using
        End If

        Dim currRect As New Rectangle(centerX, centerY, cardW, cardH)
        g.DrawImage(currentCard.sprite, currRect)
        Using pen As New Pen(Color.Gold, 4)
            g.DrawRectangle(pen, currRect)
        End Using

        For Each btn In buttons
            If btn Is btnLeft AndAlso currentIndex = 0 Then Continue For
            If btn Is btnRight AndAlso currentIndex = cards.Count - 1 Then Continue For

            If (btn.sprite IsNot Nothing) Then
                g.DrawImage(btn.sprite, btn.bounds)
            Else
                g.FillRectangle(Brushes.DarkGray, btn.bounds)
                g.DrawRectangle(Pens.White, btn.bounds)
            End If
        Next
    End Sub

    Public Sub HandleClick(mousePos As Point)
        For Each btn In buttons
            If btn Is btnLeft AndAlso currentIndex = 0 Then Continue For
            If btn Is btnRight AndAlso currentIndex = cards.Count - 1 Then Continue For

            If btn.bounds.Contains(mousePos) Then
                btn.onClick?.Invoke()
            End If
        Next
    End Sub
End Class