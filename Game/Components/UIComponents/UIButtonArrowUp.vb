Imports System.Drawing

Public Class UIButtonArrowUp
    Inherits UIButton

    Public Sub New()
        Dim btn = My.Resources.GameResources.btnDIREITA

        If btn IsNot Nothing Then
            Dim clone As Bitmap = btn.Clone()
            clone.RotateFlip(RotateFlipType.Rotate270FlipNone)

            Dim btnFrame As New Bitmap(50, 50, Imaging.PixelFormat.Format32bppArgb)

            Using g As Graphics = Graphics.FromImage(btnFrame)
                g.InterpolationMode = Drawing2D.InterpolationMode.NearestNeighbor
                g.PixelOffsetMode = Drawing2D.PixelOffsetMode.Half

                g.DrawImage(clone, New Rectangle(0, 0, 50, 50))
            End Using

            Me.sprite = btnFrame
        End If
    End Sub
End Class