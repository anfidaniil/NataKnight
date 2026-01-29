<Serializable>
Public Class GameSaveData
    Public Property Version As Integer = 1
    Public Property GameState As GameState
    Public Property Score As Integer
    Public Property PlayerID As Integer
    Public Property Entities As List(Of EntitySaveData)
End Class

