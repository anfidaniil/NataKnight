Imports System.IO
Imports System.Numerics
Imports System.Drawing

Public Class World
    Public game As Game

    Public EntityManager As New EntityManager()

    Public WaveData As New WaveComponent()

    Public Transforms As New ComponentStore(Of TransformComponent)
    Public Movements As New ComponentStore(Of MovementComponent)
    Public Colliders As New ComponentStore(Of RectangleCollider)

    Public Renders As New ComponentStore(Of RenderComponent)
    Public Cameras As New ComponentStore(Of CameraComponent)
    Public Players As New ComponentStore(Of PlayerComponent)
    Public Enemies As New ComponentStore(Of EnemyComponent)
    Public Projectiles As New ComponentStore(Of ProjectileComponent)
    Public Buffs As New ComponentStore(Of BuffComponent)

    Public Healths As New ComponentStore(Of Health)
    Public HealthBars As New ComponentStore(Of HealthBarComponent)

    Public Damages As New ComponentStore(Of DamageComponent)
    Public IFrames As New ComponentStore(Of InvincibilityComponent)

    Public Immovables As New ComponentStore(Of ImmovableComponent)

    Public Attacks As New ComponentStore(Of AttackComponent)

    Public AudioSources As New ComponentStore(Of AudioSourceComponent)
    Public AudioTriggers As New ComponentStore(Of AudioTriggerComponent)

    Public CollisionEvents As New List(Of CollisionEvent)
    Public CollisionLookupTable As New Dictionary(Of Point, List(Of Integer))
    Public EntityDestructionEvents As New HashSet(Of Integer)



    Private Systems As New List(Of ISystem)

    Public PlayerID As Integer
    Public BtnId As Integer
    Public ShootSounds As Integer

    Public SCREEN_HEIGHT = Form1.Height
    Public SCREEN_WIDTH = Form1.Width

    Public Const MAX_ACCELERATION = 1000.0F
    Public Const MAX_ENEMY_ACCELERATION = 500.0F
    Public Const MAX_VELOCITY = 200.0F

    Public Const IFRAMES_DURATION = 0.1F
    Public DEFAULT_POSITION = New PointF(0, 0)

    Public Const TILE_SIZE As Integer = 256
    Public Const GAME_NAME = "GAME NAME"

    Public Const MIN_AUDIO_DIST = 50.0F
    Public Const MAX_AUDIO_DIST = 600.0F


    Public Sub New(input As InputState, game As Game)
        Me.game = game

        Systems.Add(New PlayerMovementSystem(input))
        Systems.Add(New CameraFollowSystem())
        Systems.Add(New EnemyMovementSystem())
        Systems.Add(New MovementSystem())

        Systems.Add(New PlayerAttackSystem(input))
        Systems.Add(New EnemyAttackSystem)

        Systems.Add(New InvincibilitySystem())

        Systems.Add(New BuffsSpawnSystem())

        Systems.Add(New CollisionSystem())
        Systems.Add(New DamageSystem())
        Systems.Add(New CollisionResolutionSystem())
        Systems.Add(New BuffApplicationSystem())
        Systems.Add(New AudioSystem())

        Systems.Add(New EntityDestructionSystem())

        Systems.Add(New RenderSystem())
        Systems.Add(New WaveSystem())

    End Sub

    Public Sub Update(dt As Single)
        For Each sys In Systems
            sys.Update(Me, dt)
        Next
    End Sub

    Public Sub Draw(g)
        For Each sys In Systems
            sys.Draw(Me, g)
        Next
    End Sub

    Public Sub CreatePlayerShootingSound()
        Dim shootSounds = EntityManager.CreateEntity()
        Me.ShootSounds = shootSounds

        AudioSources.AddComponent(shootSounds, New AudioSourceComponent With {
             .soundId = New List(Of String) From {"shoot1", "shoot2", "shoot3", "shoot4"},
             .volume = 0.05F
        })
        AudioTriggers.AddComponent(shootSounds, New AudioTriggerComponent With {.playRequested = False})
    End Sub


    Public Sub CreateButton()
        Dim btn = EntityManager.CreateEntity()
        BtnId = btn

        AudioSources.AddComponent(btn, New AudioSourceComponent With {
             .soundId = New List(Of String) From {"button_ui_1", "button_ui_2"},
             .volume = 0.5F
        })
        AudioTriggers.AddComponent(btn, New AudioTriggerComponent With {.playRequested = False})
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
        Colliders.AddComponent(player, New RectangleCollider With {
            .sA = 64,
            .sB = 64
        })
        Damages.AddComponent(player, New DamageComponent With {
                             .damage = 1})
        Players.AddComponent(player, New PlayerComponent())
        Attacks.AddComponent(player, New AttackComponent With {
            .attack = False,
            .attackCooldown = 0.1F,
            .timeRemaining = 1.0F
        })

        Transforms.AddComponent(player, New TransformComponent With {.pos = New PointF(2020, 2020)})
        Movements.AddComponent(player, New MovementComponent With {.damping = 2.0F})
        Renders.AddComponent(player, New RenderComponent With {.size = 64, .spriteX = 3, .spriteY = 0})
        Colliders.AddComponent(player, New RectangleCollider With {.sA = 64, .sB = 64})
        Players.AddComponent(player, New PlayerComponent())
        Damages.AddComponent(player, New DamageComponent With {.damage = 1})
        Attacks.AddComponent(player, New AttackComponent With {.attack = False, .attackCooldown = 0.1F, .timeRemaining = 1.0F})

        Healths.AddComponent(player, New Health With {
            .health = 100,
            .maxHealth = 100
        })
                                        
        Dim full = New Bitmap(My.Resources.GameResources.FullHealth)
        Dim empty = New Bitmap(My.Resources.GameResources.emptyhealth)
        Dim current As New Bitmap(My.Resources.GameResources.FullHealth)
        HealthBars.AddComponent(player, New HealthBarComponent With {
            .fullHealthSprite = full,
            .emptyHealthSprite = empty,
            .currentHealthSprite = current,
            .position = New PointF(20, 20)
        })
                                          
        AudioSources.AddComponent(player, New AudioSourceComponent With {
             .soundId = New List(Of String) From {"footsteps_1", "footsteps_2", "footsteps_3", "footsteps_4"},
             .volume = 0.5F
        })
        AudioTriggers.AddComponent(player, New AudioTriggerComponent With {.playRequested = False})

        
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
        Colliders.AddComponent(enemy, New RectangleCollider With {
            .sA = 64,
            .sB = 64
        })
        Damages.AddComponent(enemy, New DamageComponent With {
            .damage = 2
        })

        Enemies.AddComponent(enemy, New EnemyComponent())
        Attacks.AddComponent(enemy, New AttackComponent With {
            .attack = False,
            .attackCooldown = 3.0F,
            .timeRemaining = 3.0F
        })

        Healths.AddComponent(enemy, New Health With {
            .health = 40,
            .maxHealth = 40
        })
        AudioSources.AddComponent(enemy, New AudioSourceComponent With {
             .soundId = New List(Of String) From {"footsteps_1", "footsteps_2", "footsteps_3", "footsteps_4"},
             .volume = 0.5F
        })
        AudioTriggers.AddComponent(enemy, New AudioTriggerComponent With {.playRequested = False})
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
        Colliders.AddComponent(bullet, New RectangleCollider With {
            .sA = 16,
            .sB = 16
        })
        Damages.AddComponent(bullet, New DamageComponent With {
                             .damage = 10})

        Projectiles.AddComponent(bullet, New ProjectileComponent With {.entityType = entityType, .timeLeft = 10.0F})

        'When we'll have sounds we would add those components
        AudioSources.AddComponent(bullet, New AudioSourceComponent With {
             .soundId = New List(Of String) From {"bulletImpact1", "bulletImpact2"},
             .volume = 0.5F
        })
        AudioTriggers.AddComponent(bullet, New AudioTriggerComponent With {.playRequested = False})
    End Sub
    Public Sub CreateImmovableWall(pos As PointF, size As Point)
        Dim wall = EntityManager.CreateEntity()

        Transforms.AddComponent(wall, New TransformComponent With {.pos = pos})

        Colliders.AddComponent(wall, New RectangleCollider With {
            .sA = size.X,
            .sB = size.Y
        })
        'Renders.AddComponent(wall, New RenderComponent With {
        '   .size = size.X
        '})
        Immovables.AddComponent(wall, New ImmovableComponent)

    End Sub


    Public Sub OnPlayerDestruction()
        game.GameOver()
    End Sub

    Public Sub UpdatePlayerHealthBar()
        If Not HealthBars.HasComponent(PlayerID) OrElse Not Healths.HasComponent(PlayerID) Then Return

        Dim hb = HealthBars.GetComponent(PlayerID)
        Dim hp = Healths.GetComponent(PlayerID)

        If hp.maxHealth <= 0 Then hp.maxHealth = 100

        Dim barWidth As Integer = hb.fullHealthSprite.Width
        Dim barHeight As Integer = hb.fullHealthSprite.Height
        Dim healthPercentage As Single = Math.Max(0, CSng(hp.health) / CSng(hp.maxHealth))
        Dim visibleWidth As Integer = CInt(healthPercentage * barWidth)

        Using g As Graphics = Graphics.FromImage(hb.currentHealthSprite)
            g.Clear(Color.Transparent)

            g.DrawImage(hb.emptyHealthSprite, 0, 0, barWidth, barHeight)

            If visibleWidth > 0 Then
                Dim srcRect As New Rectangle(0, 0, visibleWidth, barHeight)
                Dim destRect As New Rectangle(0, 0, visibleWidth, barHeight)
                g.DrawImage(hb.fullHealthSprite, destRect, srcRect, GraphicsUnit.Pixel)
            End If
        End Using
    End Sub
End Class
