Public Class InputState
    Public up As Boolean
    Public down As Boolean
    Public left As Boolean
    Public right As Boolean

    Public Sub New(up As Boolean, down As Boolean, left As Boolean, right As Boolean)
        up = up
        down = down
        left = left
        right = right
    End Sub
End Class
