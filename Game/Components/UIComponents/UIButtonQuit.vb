Public Class UIButtonQuit
    Inherits UIButton
    Public Sub New()
        Dim btn = New Bitmap(My.Resources.GameResources.btnSAIR)
        Dim spriteRect As New Rectangle(0, 0, 200, 50)

        Dim btnFrame As New Bitmap(200, 50, Imaging.PixelFormat.Format32bppArgb)

        Using g As Graphics = Graphics.FromImage(btnFrame)
            g.InterpolationMode = Drawing2D.InterpolationMode.NearestNeighbor
            g.PixelOffsetMode = Drawing2D.PixelOffsetMode.Half

            g.DrawImage(btn, New Rectangle(0, 0, 200, 50), spriteRect, GraphicsUnit.Pixel)
        End Using
        sprite = btnFrame
    End Sub
End Class
