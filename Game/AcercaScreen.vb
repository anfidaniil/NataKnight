Imports System.Drawing
Imports Windows.Win32.UI.Input

Public Class AcercaScreen
    Private game As Game

    Private cards As New List(Of UICard)
    Public buttons As New List(Of UIButton)


    Public BackAction As Action

    Private imgBackground As Bitmap

    Public Sub New(gameInstance As Game)
        Me.game = gameInstance

        BackAction = Sub() game.gameState = GameState.Starting

        LoadResources()
        InitializeButtons()
    End Sub

    Private Sub LoadResources()
        imgBackground = My.Resources.GameResources.MAINmenu
        cards.Add(New UICard(My.Resources.GameResources.ancientTUALETNAYAbumaga))
    End Sub

    Private Sub InitializeButtons()
        Dim screenW As Integer = Form1.Width
        Dim screenH As Integer = Form1.Height
        Dim scale As Single = game.GetUIElementScale()

        Dim btnW As Integer = CInt(200 * scale)
        Dim btnH As Integer = CInt(50 * scale)
        Dim bottomMargin As Integer = CInt(120 * scale)

        Dim btnY As Integer = screenH - bottomMargin

        Dim startX As Integer = (screenW - btnW) \ 2

        buttons.Add(New UIButtonGoBack With {
            .bounds = New Rectangle(startX, btnY, btnW, btnH),
            .text = "",
            .onClick = Sub()
                           AudioEngine.PlayOneShot("button_ui_1", 1.0F)
                           BackAction?.Invoke()
                       End Sub
        })
    End Sub

    Public Sub Draw(g As Graphics, world As World)
        g.Clear(Color.Black)

        g.InterpolationMode = Drawing2D.InterpolationMode.NearestNeighbor
        g.PixelOffsetMode = Drawing2D.PixelOffsetMode.Half

        If imgBackground IsNot Nothing Then
            Dim imgRatio As Single = imgBackground.Width / imgBackground.Height
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
            g.DrawImage(imgBackground, x, y, drawW, drawH)
        End If

        If cards.Count = 0 Then Return

        Dim currentCard = cards(0)

        Dim scale As Single = game.GetCardScale() * 1.2F

        Dim screenWidth = Form1.Width
        Dim screenHeight = Form1.Height

        If currentCard.sprite.Width * scale > screenWidth * 0.8 Then
            scale = (screenWidth * 0.8) / currentCard.sprite.Width
        End If

        Dim cardW As Integer = CInt(currentCard.sprite.Width * scale)
        Dim cardH As Integer = CInt(currentCard.sprite.Height * scale)
        Dim centerX As Integer = (screenWidth - cardW) \ 2
        Dim centerY As Integer = CInt(screenHeight * 0.05)

        Dim currRect As New Rectangle(centerX, centerY, cardW, cardH)

        g.DrawImage(currentCard.sprite, currRect)

        DrawCreditsText(g, currRect)
        For Each btn In buttons
            If (btn.sprite IsNot Nothing) Then
                g.DrawImage(btn.sprite, btn.bounds)
            Else
                g.FillRectangle(Brushes.DarkGray, btn.bounds)
                g.DrawRectangle(Pens.White, btn.bounds)
            End If
        Next
    End Sub

    Private Sub DrawCreditsText(g As Graphics, rect As Rectangle)
        Dim fontSizeTitle As Single = 22.0F
        Dim fontSizeName As Single = 16.0F
        Dim fontSizeRole As Single = 13.0F
        Dim fontSizeSmall As Single = 11.0F

        Using titleFont As New Font("Courier New", fontSizeTitle, FontStyle.Bold),
              nameFont As New Font("Courier New", fontSizeName, FontStyle.Bold),
              roleFont As New Font("Courier New", fontSizeRole, FontStyle.Regular),
              italicFont As New Font("Courier New", fontSizeSmall, FontStyle.Italic)

            Dim brush As Brush = Brushes.Black
            Dim centerX As Single = rect.X + (rect.Width / 2)

            Dim currentY As Single = rect.Y + (rect.Height * 0.12F)

            Dim spacing As Single = 22.0F

            DrawCenteredText(g, "EQUIPA DE COZINHEIROS", titleFont, brush, centerX, currentY)
            currentY += spacing * 2.5

            ' --- DANIIL ---
            DrawCenteredText(g, "Daniil", nameFont, brush, centerX, currentY)
            currentY += spacing
            DrawCenteredText(g, "Low Level Code", roleFont, brush, centerX, currentY)
            currentY += spacing
            DrawCenteredText(g, "(A Massa Folhada)", italicFont, brush, centerX, currentY)
            currentY += spacing * 2

            ' --- FRANCISCO ---
            DrawCenteredText(g, "Francisco", nameFont, brush, centerX, currentY)
            currentY += spacing
            DrawCenteredText(g, "High Level Code", roleFont, brush, centerX, currentY)
            currentY += spacing
            DrawCenteredText(g, "(O Creme)", italicFont, brush, centerX, currentY)
            currentY += spacing * 2

            ' --- SHAMIN ---
            DrawCenteredText(g, "Shamin", nameFont, brush, centerX, currentY)
            currentY += spacing
            DrawCenteredText(g, "Designer", roleFont, brush, centerX, currentY)
            currentY += spacing
            DrawCenteredText(g, "(A Canela)", italicFont, brush, centerX, currentY)
            currentY += spacing * 2.5

            ' --- FUNNY QUOTE ---
            Dim quote As String = "Nota: Nenhum pixel foi queimado" & vbCrLf & "durante a produção deste jogo."
            DrawCenteredText(g, quote, italicFont, brush, centerX, currentY)

        End Using
    End Sub

    Private Sub DrawCenteredText(g As Graphics, text As String, font As Font, brush As Brush, centerX As Single, y As Single)
        Dim size = g.MeasureString(text, font)
        g.DrawString(text, font, brush, centerX - (size.Width / 2), y)
    End Sub

    Public Sub HandleClick(mousePos As Point)
        For Each btn In buttons
            If btn.bounds.Contains(mousePos) Then
                btn.onClick?.Invoke()
            End If
        Next
    End Sub
End Class