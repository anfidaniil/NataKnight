Public Class HealthBuff
    Inherits BuffComponent

    Public Property healthRegen As Integer = 20

    Public Sub New()
        Me.type = BuffType.HealthRegen
    End Sub

End Class
