Imports System.Drawing

Public Class UIButtonArrowRight
    Inherits UIButton

    Public Sub New()
        Dim rawImg = My.Resources.GameResources.btnDIREITA

        If rawImg IsNot Nothing Then
            Me.sprite = rawImg
            Me.bounds = New Rectangle(0, 0, rawImg.Width, rawImg.Height)
        End If
    End Sub
End Class
