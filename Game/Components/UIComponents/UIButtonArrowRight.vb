Imports System.Drawing

Public Class UIButtonArrowRight
    Inherits UIButton

    Public Sub New()
        Dim btn = My.Resources.GameResources.btnDIREITA

        If btn IsNot Nothing Then
            Dim spriteRect As New Rectangle(0, 0, btn.Width, btn.Height)

            Dim btnFrame As New Bitmap(50, 50, Imaging.PixelFormat.Format32bppArgb)

            Using g As Graphics = Graphics.FromImage(btnFrame)
                g.InterpolationMode = Drawing2D.InterpolationMode.NearestNeighbor
                g.PixelOffsetMode = Drawing2D.PixelOffsetMode.Half

                g.DrawImage(btn, New Rectangle(0, 0, 50, 50), spriteRect, GraphicsUnit.Pixel)
            End Using

            Me.sprite = btnFrame
        End If
    End Sub
End Class