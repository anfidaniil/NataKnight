Imports System.Drawing
Imports Windows.Win32.UI.Input

Public Class AcercaScreen
    Private game As Game

    Private cards As New List(Of UICard)
    Public buttons As New List(Of UIButton)


    Public BackAction As Action

    Private imgBackground As Bitmap

    Private Structure MemberData
        Public Name As String
        Public Role As String
        Public Note As String
    End Structure

    Private members As New List(Of MemberData)
    Private currentMemberIndex As Integer = 0

    Public Sub New(gameInstance As Game)
        Me.game = gameInstance

        BackAction = Sub() game.gameState = GameState.Starting

        LoadResources()
        InitializeTeamData()
        InitializeButtons()
    End Sub

    Private Sub LoadResources()
        imgBackground = My.Resources.GameResources.MAINmenu
        cards.Add(New UICard(My.Resources.GameResources.ancientTUALETNAYAbumaga))
    End Sub

    Private Sub InitializeTeamData()
        members.Add(New MemberData With {.Name = "Daniil", .Role = "Low Level Code", .Note = "A Massa Folhada (Fez a base para o jogo não se desfazer)"})
        members.Add(New MemberData With {.Name = "Francisco", .Role = "High Level Code", .Note = "O Creme (Deu sabor à lógica e à jogabilidade)"})
        members.Add(New MemberData With {.Name = "Shamin", .Role = "Designer", .Note = "A Canela (A única razão disto ter bom aspeto)"})
    End Sub

    Private Sub InitializeButtons()
        buttons.Clear()
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

        Dim currentCard = cards(0)
        Dim scrollScale As Single = game.GetCardScale() * 1.2F

        If currentCard.sprite.Width * scrollScale > screenW * 0.8 Then
            scrollScale = (screenW * 0.8) / currentCard.sprite.Width
        End If

        Dim cardW As Integer = CInt(currentCard.sprite.Width * scrollScale)
        Dim cardH As Integer = CInt(currentCard.sprite.Height * scrollScale)

        Dim scrollX As Integer = (screenW - cardW) \ 2
        Dim scrollY As Integer = CInt(screenH * 0.05)

        Dim scrollRightEdge As Integer = scrollX + cardW
        Dim scrollBottomEdge As Integer = scrollY + cardH

        Dim arrowSize As Integer = CInt(50 * scale)
        Dim centerYtext As Integer = CInt(screenH * 0.45)
        Dim hMargin As Integer = CInt(250 * scale)

        buttons.Add(New UIButtonArrowLeft With {
            .bounds = New Rectangle((screenW \ 2) - hMargin - arrowSize, centerYtext - (arrowSize \ 2), arrowSize, arrowSize),
            .onClick = Sub()
                           AudioEngine.PlayOneShot("button_ui_1", 1.0F)
                           currentMemberIndex -= 1
                           If currentMemberIndex < 0 Then currentMemberIndex = members.Count - 1
                       End Sub
        })

        buttons.Add(New UIButtonArrowRight With {
            .bounds = New Rectangle((screenW \ 2) + hMargin, centerYtext - (arrowSize \ 2), arrowSize, arrowSize),
            .onClick = Sub()
                           AudioEngine.PlayOneShot("button_ui_1", 1.0F)
                           currentMemberIndex += 1
                           If currentMemberIndex >= members.Count Then currentMemberIndex = 0
                       End Sub
        })

        Dim vertArrowSize As Integer = CInt(30 * scale)
        Dim paddingRight As Integer = CInt(130 * scale)
        Dim paddingBottom As Integer = CInt(70 * scale)
        Dim vertArrowX As Integer = scrollRightEdge - paddingRight - vertArrowSize
        Dim vertArrowY As Integer = scrollBottomEdge - paddingBottom - vertArrowSize

        Dim btnUp As New UIButtonArrowUp()
        btnUp.bounds = New Rectangle(vertArrowX, vertArrowY - vertArrowSize, vertArrowSize, vertArrowSize)
        btnUp.onClick = Sub()
                            AudioEngine.PlayOneShot("button_ui_2", 1.0F) ' Placeholder
                        End Sub
        buttons.Add(btnUp)

        Dim btnDown As New UIButtonArrowDown()
        btnDown.bounds = New Rectangle(vertArrowX, vertArrowY, vertArrowSize, vertArrowSize)
        btnDown.onClick = Sub()
                              AudioEngine.PlayOneShot("button_ui_2", 1.0F) ' Placeholder
                          End Sub
        buttons.Add(btnDown)
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
        Dim fontSizeTitle As Single = 26.0F
        Dim fontSizeName As Single = 24.0F
        Dim fontSizeRole As Single = 18.0F
        Dim fontSizeSmall As Single = 10.0F

        Using titleFont As New Font("Courier New", fontSizeTitle, FontStyle.Bold),
              nameFont As New Font("Courier New", fontSizeName, FontStyle.Bold),
              roleFont As New Font("Courier New", fontSizeRole, FontStyle.Regular),
              italicFont As New Font("Courier New", fontSizeSmall, FontStyle.Italic)

            Dim brush As Brush = Brushes.Black
            Dim centerX As Single = rect.X + (rect.Width / 2)

            Dim currentY As Single = rect.Y + (rect.Height * 0.15F)
            Dim spacing As Single = 30.0F

            DrawCenteredText(g, "EQUIPA DE COZINHEIROS", titleFont, brush, centerX, currentY)
            currentY += spacing * 5.4

            Dim member = members(currentMemberIndex)

            DrawCenteredText(g, member.Name, nameFont, brush, centerX, currentY)
            currentY += spacing * 1.2

            DrawCenteredText(g, member.Role, roleFont, brush, centerX, currentY)
            currentY += spacing * 1.2

            DrawCenteredText(g, member.Note, italicFont, brush, centerX, currentY)


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