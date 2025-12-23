Public Class InputState
    Public up As Boolean
    Public down As Boolean
    Public left As Boolean
    Public right As Boolean

    Public fire As Boolean
    Public cursorPos As Point

    Public Sub New(up As Boolean, down As Boolean, left As Boolean, right As Boolean, fire As Boolean, cursor As Point)
        up = up
        down = down
        left = left
        right = right
        fire = fire
        cursorPos = cursor
    End Sub
End Class
