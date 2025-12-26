Imports System.Numerics

Public Class World
    Public game As Game

    Public EntityManager As New EntityManager()

    Public Transforms As New ComponentStore(Of TransformComponent)
    Public Movements As New ComponentStore(Of MovementComponent)
    Public Colliders As New ComponentStore(Of BoxCollider)

    Public Renders As New ComponentStore(Of RenderComponent)
    Public Cameras As New ComponentStore(Of CameraComponent)
    Public Players As New ComponentStore(Of PlayerComponent)
    Public Enemies As New ComponentStore(Of EnemyComponent)
    Public Projectiles As New ComponentStore(Of ProjectileComponent)

    Public Healths As New ComponentStore(Of Health)

    Public Damages As New ComponentStore(Of DamageComponent)
    Public IFrames As New ComponentStore(Of InvincibilityComponent)

    Public Immovables As New ComponentStore(Of ImmovableComponent)

    Public Attacks As New ComponentStore(Of AttackComponent)

    Public CollisionEvents As New List(Of CollisionEvent)
    Public EntityDestructionEvents As New HashSet(Of Integer)

    Private Systems As New List(Of ISystem)

    Public PlayerID As Integer

    Public SCREEN_HEIGHT = Form1.Height
    Public SCREEN_WIDTH = Form1.Width

    Public Const MAX_ACCELERATION = 1000.0F
    Public Const MAX_ENEMY_ACCELERATION = 500.0F
    Public Const MAX_VELOCITY = 200.0F

    Public Const IFRAMES_DURATION = 0.1F
    Public DEFAULT_POSITION = New PointF(0, 0)

    Public Const TILE_SIZE As Integer = 256


    Public Sub New(input As InputState, game As Game)
        Me.game = game

        Systems.Add(New PlayerMovementSystem(input))
        Systems.Add(New CameraFollowSystem())
        Systems.Add(New EnemyMovementSystem())
        Systems.Add(New MovementSystem())

        Systems.Add(New PlayerAttackSystem(input))
        Systems.Add(New EnemyAttackSystem)

        Systems.Add(New InvincibilitySystem())

        Systems.Add(New CollisionSystem())
        Systems.Add(New DamageSystem())
        Systems.Add(New CollisionResolutionSystem())

        Systems.Add(New RenderSystem())
    End Sub

    Public Sub Update(dt As Single)
        For Each sys In Systems
            sys.Update(Me, dt)
        Next
        DestructEntities()
    End Sub

    Public Sub Draw(g)
        For Each sys In Systems
            sys.Draw(Me, g)
        Next
    End Sub

    Public Sub CreateCamera()
        Dim camera = EntityManager.CreateEntity()
        Cameras.AddComponent(camera, New CameraComponent With {
            .viewHeight = SCREEN_HEIGHT,
            .viewWidth = SCREEN_WIDTH}
        )
        Movements.AddComponent(camera, New MovementComponent With {
                               .acceleration = New PointF(0, 0),
                               .velocity = New PointF(0, 0),
                               .damping = 1.5F}
                               )
        Transforms.AddComponent(camera, New TransformComponent With {
                                .pos = New PointF(200 - SCREEN_WIDTH / 2, 100 - SCREEN_HEIGHT / 2)})
    End Sub

    Public Sub CreatePlayer()
        Dim player = EntityManager.CreateEntity()
        PlayerID = player

        Transforms.AddComponent(player, New TransformComponent With {
            .pos = New PointF(2020, 2020)
        })

        Movements.AddComponent(player, New MovementComponent With {
            .velocity = New PointF(0F, 0F),
            .acceleration = New PointF(0F, 0F),
            .damping = 2.0F
        })

        Renders.AddComponent(player, New RenderComponent With {
            .size = 64,
            .spriteX = 3,
            .spriteY = 0
        })
        Colliders.AddComponent(player, New BoxCollider With {
            .size = 64
        })
        Healths.AddComponent(player, New Health With {
            .health = 50
        })
        Damages.AddComponent(player, New DamageComponent With {
                             .damage = 1})
        Players.AddComponent(player, New PlayerComponent())
        Attacks.AddComponent(player, New AttackComponent With {
            .attack = False,
            .attackCooldown = 0.1F,
            .timeRemaining = 1.0F
        })
    End Sub

    Public Sub CreateEnemy(pos As PointF)
        Dim enemy = EntityManager.CreateEntity()


        Transforms.AddComponent(enemy, New TransformComponent With {
            .pos = pos
        })

        Movements.AddComponent(enemy, New MovementComponent With {
            .velocity = New PointF(0F, 0F),
            .acceleration = New PointF(0F, 0F),
            .damping = 1.0F
        })

        Renders.AddComponent(enemy, New RenderComponent With {
            .size = 64,
            .spriteX = 3,
            .spriteY = 1
        })
        Colliders.AddComponent(enemy, New BoxCollider With {
            .size = 64
        })
        Healths.AddComponent(enemy, New Health With {
            .health = 100
        })
        Damages.AddComponent(enemy, New DamageComponent With {
                             .damage = 1})

        Enemies.AddComponent(enemy, New EnemyComponent())
        Attacks.AddComponent(enemy, New AttackComponent With {
            .attack = False,
            .attackCooldown = 3.0F,
            .timeRemaining = 3.0F
        })
    End Sub

    Public Sub CreateBullet(startPos As PointF, targetPos As PointF, entityType As Integer)
        Dim velocity = New PointF(
            targetPos.X - startPos.X,
            targetPos.Y - startPos.Y
        )

        Dim norm = Utils.NormalisePointFVector(velocity)
        norm = New PointF(
            norm.X * 400.0F,
            norm.Y * 400.0F
        )

        Dim bullet = EntityManager.CreateEntity()
        Transforms.AddComponent(bullet, New TransformComponent With {
            .pos = startPos})
        Movements.AddComponent(bullet, New MovementComponent With {
            .acceleration = New PointF(0, 0),
            .velocity = norm,
            .max_velocity = 400.0F,
            .damping = 0.01F
        })
        Renders.AddComponent(bullet, New RenderComponent With {
            .size = 64,
            .spriteX = 2 + 3 * entityType,
            .spriteY = 2
        })
        Colliders.AddComponent(bullet, New BoxCollider With {.size = 16})
        Damages.AddComponent(bullet, New DamageComponent With {
                             .damage = 10})

        Projectiles.AddComponent(bullet, New ProjectileComponent With {.entityType = entityType, .timeLeft = 10.0F})
    End Sub
    Public Sub CreateImmovableWall(pos As PointF, size As Integer)
        Dim wall = EntityManager.CreateEntity()

        Transforms.AddComponent(wall, New TransformComponent With {.pos = pos})

        Colliders.AddComponent(wall, New BoxCollider With {.size = size})
        Renders.AddComponent(wall, New RenderComponent With {
           .size = size
        })
        Immovables.AddComponent(wall, New ImmovableComponent)

    End Sub

    Public Sub DestructEntities()
        If EntityDestructionEvents.Count = 0 Then Return

        For Each e In EntityDestructionEvents

            ' Remove components from all stores
            Transforms.RemoveComponent(e)
            Movements.RemoveComponent(e)
            Colliders.RemoveComponent(e)
            Renders.RemoveComponent(e)
            Players.RemoveComponent(e)
            Enemies.RemoveComponent(e)
            Healths.RemoveComponent(e)
            Damages.RemoveComponent(e)
            IFrames.RemoveComponent(e)
            Immovables.RemoveComponent(e)
            Cameras.RemoveComponent(e)
            Attacks.RemoveComponent(e)
            Projectiles.RemoveComponent(e)

            ' Remove entity itself
            EntityManager.RemoveEntity(e)
            If (e = PlayerID) Then
                OnPlayerDestruction()
            End If
            Debug.WriteLine("Destroyed entity: " & e)
        Next
        EntityDestructionEvents.Clear()
    End Sub

    Private Sub OnPlayerDestruction()
        game.GameOver()
    End Sub
End Class
