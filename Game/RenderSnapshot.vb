
Public Class RenderEntity
    Public X As Single
    Public Y As Single
    Public Size As Integer
    Public SpriteX As Integer
    Public SpriteY As Integer
    Public IsImmovable As Boolean
End Class

Public Class RenderCamera
    Public X As Single
    Public Y As Single
    Public Width As Integer
    Public Height As Integer
End Class

Public Class RenderSnapshot
    Public Camera As RenderCamera
    Public Entities As RenderEntity()
    Public Score As Integer
    Public HealthBar As Bitmap
End Class


