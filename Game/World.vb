Public Class World
    Public EntityManager As New EntityManager()

    Public Transforms As New ComponentStore(Of TransformComponent)
    Public Movements As New ComponentStore(Of MovementComponent)
    Public Renders As New ComponentStore(Of RenderComponent)

    Private Systems As New List(Of ISystem)

    Public Sub New(g As Graphics, input As InputState)
        Systems.Add(New PlayerMovementSystem(input))
        Systems.Add(New RenderSystem(g))
    End Sub

    Public Sub Update(dt As Single)
        For Each sys In Systems
            sys.Update(Me, dt)
        Next
    End Sub

    Public Sub Draw()
        For Each sys In Systems
            sys.Draw(Me)
        Next
    End Sub

    Public Sub CreatePlayer()
        Dim player = EntityManager.CreateEntity()

        Transforms.AddComponent(player, New TransformComponent With {
            .pos = New PointF(200, 200)
        })

        Movements.AddComponent(player, New MovementComponent With {
            .speed = 200.0F
        })

        Renders.AddComponent(player, New RenderComponent With {
            .size = 32,
            .brush = Brushes.White
        })
    End Sub

End Class
