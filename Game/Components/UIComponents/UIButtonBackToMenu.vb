Imports System.Drawing
Imports System.Reflection.Metadata

Public Class UIButtonBackToMenu
    Inherits UIButton

    Public Sub New()
        Dim btn = New Bitmap(My.Resources.GameResources.btnVOLTARAOMENU_export)

        If btn IsNot Nothing Then
            Dim spriteRect As New Rectangle(0, 0, 200, 50)

            Dim btnFrame As New Bitmap(200, 50, Imaging.PixelFormat.Format32bppArgb)

            Using g As Graphics = Graphics.FromImage(btnFrame)
                g.InterpolationMode = Drawing2D.InterpolationMode.NearestNeighbor
                g.PixelOffsetMode = Drawing2D.PixelOffsetMode.Half

                g.DrawImage(btn, New Rectangle(0, 0, 200, 50), spriteRect, GraphicsUnit.Pixel)
            End Using

            Me.sprite = btnFrame
        End If
    End Sub
End Class