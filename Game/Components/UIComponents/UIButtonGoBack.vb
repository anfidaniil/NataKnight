Imports System.Drawing

Public Class UIButtonGoBack
    Inherits UIButton

    Public Sub New()
        Dim rawImg = My.Resources.GameResources.btnVOLTAR

        If rawImg IsNot Nothing Then
            Me.sprite = rawImg

        End If
    End Sub
End Class