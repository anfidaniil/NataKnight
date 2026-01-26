Imports System.Drawing

Public Class UIButtonArrowLeft
        Inherits UIButton

        Public Sub New()
            Dim rawImg = My.Resources.GameResources.btnESQUERDA

            If rawImg IsNot Nothing Then
            Me.sprite = rawImg
            Me.bounds = New Rectangle(0, 0, rawImg.Width, rawImg.Height)
            End If
        End Sub
End Class