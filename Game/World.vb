Imports System.Numerics

Public Class World
    Public EntityManager As New EntityManager()

    Public Transforms As New ComponentStore(Of TransformComponent)
    Public Movements As New ComponentStore(Of MovementComponent)
    Public Colliders As New ComponentStore(Of BoxCollider)
    Public Renders As New ComponentStore(Of RenderComponent)
    Public Players As New ComponentStore(Of PlayerComponent)
    Public Enemies As New ComponentStore(Of EnemyComponent)
    Public Healths As New ComponentStore(Of Health)
    Public Damages As New ComponentStore(Of DamageComponent)

    Public CollisionEvents As New List(Of CollisionEvent)
    Public EntityDestructionEvents As New List(Of EntityDestructionEvent)

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
        Systems.Add(New DamageSystem())
        Systems.Add(New RenderSystem(g))
    End Sub

    Public Sub Update(dt As Single)
        For Each sys In Systems
            sys.Update(Me, dt)
        Next
        DestructEntities()
    End Sub

    Public Sub Draw()
        For Each sys In Systems
            sys.Draw(Me)
        Next
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
            .size = 16,
            .brush = Brushes.White
        })
        Colliders.AddComponent(player, New BoxCollider With {
            .size = 16
        })
        Damages.AddComponent(player, New DamageComponent With {
                             .damage = 1})
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
        Healths.AddComponent(enemy, New Health With {
                             .health = 100})

        Enemies.AddComponent(enemy, New EnemyComponent())
    End Sub

    Public Sub CreateStain(pos As PointF)
        Dim bullet = EntityManager.CreateEntity()

        Transforms.AddComponent(bullet, New TransformComponent With {
            .pos = pos
        })
        Renders.AddComponent(bullet, New RenderComponent With {
           .size = 8,
           .brush = Brushes.Red
       })
    End Sub

    Public Sub DestructEntities()
        If EntityDestructionEvents.Count = 0 Then Return

        For Each ev In EntityDestructionEvents
            Dim e = ev.entityID

            ' Remove components from all stores
            Transforms.RemoveComponent(e)
            Movements.RemoveComponent(e)
            Colliders.RemoveComponent(e)
            Renders.RemoveComponent(e)
            Players.RemoveComponent(e)
            Enemies.RemoveComponent(e)
            Healths.RemoveComponent(e)
            Damages.RemoveComponent(e)

            ' Remove entity itself
            EntityManager.RemoveEntity(e)
            Debug.WriteLine("Destroyed entity: " & e)
        Next

        EntityDestructionEvents.Clear()
    End Sub
End Class
