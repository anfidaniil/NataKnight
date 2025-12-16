Imports System.Numerics

Public Class World
    Public EntityManager As New EntityManager()

    Public Transforms As New ComponentStore(Of TransformComponent)
    Public Movements As New ComponentStore(Of MovementComponent)
    Public Colliders As New ComponentStore(Of BoxCollider)
    Public Renders As New ComponentStore(Of RenderComponent)
    Public Players As New ComponentStore(Of PlayerComponent)
    Public Enemies As New ComponentStore(Of EnemyComponent)

    Public CollisionEvents As New List(Of CollisionEvent)

    Private Systems As New List(Of ISystem)

    Public PlayerID As Integer

    Public Const MAX_ACCELERATION = 1000.0F
    Public Const MAX_VELOCITY = 200.0F

    Public Sub New(g As Graphics, input As InputState)
        Systems.Add(New PlayerMovementSystem(input))
        Systems.Add(New EnemyMovementSystem())
        Systems.Add(New MovementSystem())
        Systems.Add(New CollisionSystem())
        Systems.Add(New CollisionHandling())
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
        Renders.GetComponent(PlayerID).brush = Brushes.White
    End Sub

    Public Sub CreatePlayer()
        Dim player = EntityManager.CreateEntity()
        PlayerID = player

        Transforms.AddComponent(player, New TransformComponent With {
            .pos = New PointF(200, 200)
        })

        Movements.AddComponent(player, New MovementComponent With {
            .velocity = New PointF(0F, 0F),
            .acceleration = New PointF(0F, 0F),
            .damping = 2.0F
        })

        Renders.AddComponent(player, New RenderComponent With {
            .size = 32,
            .brush = Brushes.White
        })
        Colliders.AddComponent(player, New BoxCollider With {
            .size = 32
        })
        Players.AddComponent(player, New PlayerComponent())
    End Sub

    Public Sub CreateEnemy()
        Dim enemy = EntityManager.CreateEntity()

        Transforms.AddComponent(enemy, New TransformComponent With {
            .pos = New PointF(0, 0)
        })

        Movements.AddComponent(enemy, New MovementComponent With {
            .velocity = New PointF(0F, 0F),
            .acceleration = New PointF(0F, 0F),
            .damping = 1.0F
        })

        Renders.AddComponent(enemy, New RenderComponent With {
            .size = 16,
            .brush = Brushes.Red
        })
        Colliders.AddComponent(enemy, New BoxCollider With {
            .size = 16
        })
        Enemies.AddComponent(enemy, New EnemyComponent())
    End Sub

    Public Sub CreateStain(pos As PointF, pos2 As PointF)
        Dim bullet = EntityManager.CreateEntity()

        Dim velocity = New PointF(0.0F, 0.0F)
        Dim acceleration = New PointF(
           0, 0
        )
        Transforms.AddComponent(bullet, New TransformComponent With {
            .pos = pos
        })
        Movements.AddComponent(bullet, New MovementComponent With {
            .velocity = velocity,
            .acceleration = acceleration
        })
        Renders.AddComponent(bullet, New RenderComponent With {
           .size = 8,
           .brush = Brushes.Red
       })
        'Colliders.AddComponent(bullet, New BoxCollider With {
        '    .size = 8
        '})
    End Sub

End Class
