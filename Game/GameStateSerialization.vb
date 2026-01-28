Imports System.Text.Json
Imports System.IO

Public Module GameStateSerialization


    Public Sub SaveToFile(game As Game, filePath As String)
        Dim saveData = SaveWorld(game)

        Dim options As New JsonSerializerOptions With {
        .WriteIndented = True
    }

        Dim json As String = JsonSerializer.Serialize(saveData, options)
        File.WriteAllText(filePath, json)
    End Sub

    Public Sub LoadFromFile(game As Game, filePath As String)
        If Not File.Exists(filePath) Then
            Throw New FileNotFoundException("Save file not found.", filePath)
        End If

        Dim json As String = File.ReadAllText(filePath)

        Dim options As New JsonSerializerOptions()
        Dim saveData = JsonSerializer.Deserialize(Of GameSaveData)(json, options)

        LoadWorld(game, saveData)
    End Sub


    Public Function SaveWorld(game As Game) As GameSaveData
        Dim save As New GameSaveData With {
        .GameState = game.gameState,
        .Score = game.score,
        .PlayerID = game.world.PlayerID,
        .Entities = New List(Of EntitySaveData)
    }

        For Each entityId In game.world.EntityManager.Entities
            Dim e As New EntitySaveData With {.Id = entityId}

            If game.world.Transforms.HasComponent(entityId) Then
                Dim t = game.world.Transforms.GetComponent(entityId)
                e.Components("Transform") = JsonSerializer.Serialize(
                New TransformSave With {.X = t.pos.X, .Y = t.pos.Y})
            End If

            If game.world.Movements.HasComponent(entityId) Then
                Dim m = game.world.Movements.GetComponent(entityId)
                e.Components("Movement") = JsonSerializer.Serialize(New MovementSave With {
                .vX = m.velocity.X,
                .vY = m.velocity.Y,
                .aX = m.acceleration.X,
                .aY = m.acceleration.Y,
                .damping = m.damping,
                .max_velocity = m.max_velocity,
                .max_acceleration = m.max_acceleration
            })
            End If

            If game.world.Healths.HasComponent(entityId) Then
                Dim h = game.world.Healths.GetComponent(entityId)
                e.Components("Health") = JsonSerializer.Serialize(New HealthSave With {
                .health = h.health,
                .maxHealth = h.maxHealth
            })
            End If

            If game.world.Renders.HasComponent(entityId) Then
                Dim r = game.world.Renders.GetComponent(entityId)
                e.Components("Renders") = JsonSerializer.Serialize(New RenderComponent With {
                .size = r.size,
                .spriteX = r.spriteX,
                .spriteY = r.spriteY
            })
            End If

            If game.world.Attacks.HasComponent(entityId) Then
                Dim a = game.world.Attacks.GetComponent(entityId)
                e.Components("Attacks") = JsonSerializer.Serialize(New AttackSave With {
                    .attack = a.attack,
                    .attackCooldown = a.attackCooldown,
                    .timeRemaining = a.timeRemaining
                })
            End If

            If game.world.Buffs.HasComponent(entityId) Then
                Dim b = game.world.Buffs.GetComponent(entityId)
                e.Components("Buffs") = JsonSerializer.Serialize(New BuffSave With {
                    .isConsumed = b.isConsumed,
                    .type = b.type
                })
            End If

            If game.world.IFrames.HasComponent(entityId) Then
                Dim i = game.world.IFrames.GetComponent(entityId)
                e.Components("IFrames") = JsonSerializer.Serialize(New InvincibilitySave With {
                    .timeRemaining = i.timeRemaining
                })
            End If

            If game.world.Projectiles.HasComponent(entityId) Then
                Dim p = game.world.Projectiles.GetComponent(entityId)
                e.Components("Projectiles") = JsonSerializer.Serialize(New ProjectileSave With {
                    .entityType = p.entityType,
                    .timeLeft = p.timeLeft
                })
            End If

            If game.world.Colliders.HasComponent(entityId) Then
                Dim c = game.world.Colliders.GetComponent(entityId)
                e.Components("RectCollider") = JsonSerializer.Serialize(New RectangleColliderSave With {
                    .sA = c.sA,
                    .sB = c.sB
                })
            End If

            If game.world.Enemies.HasComponent(entityId) Then
                e.Components("Enemy") = Nothing
            End If

            If game.world.Players.HasComponent(entityId) Then
                e.Components("Player") = Nothing
            End If

            If game.world.Immovables.HasComponent(entityId) Then
                e.Components("Immovable") = Nothing
            End If

            save.Entities.Add(e)
        Next

        Return save
    End Function

    Public Sub LoadWorld(game As Game, save As GameSaveData)

        game.world = New World(game.input, game)

        Dim world = game.world

        For Each eSave In save.Entities
            world.EntityManager.CreateEntityWithId(eSave.Id)
            Dim id = eSave.Id
            Dim c = eSave.Components

            If c.ContainsKey("Transform") Then
                Dim t = JsonSerializer.Deserialize(Of TransformSave)(c("Transform"))
                world.Transforms.AddComponent(id, New TransformComponent With {
                .pos = New PointF(t.X, t.Y)
            })
            End If

            If c.ContainsKey("Movement") Then
                Dim m = JsonSerializer.Deserialize(Of MovementSave)(c("Movement"))
                world.Movements.AddComponent(id, New MovementComponent With {
                .velocity = New PointF(m.vX, m.vY),
                .acceleration = New PointF(m.aX, m.aY),
                .damping = m.damping,
                .max_velocity = m.max_velocity,
                .max_acceleration = m.max_acceleration
            })
            End If

            If c.ContainsKey("Health") Then
                Dim h = JsonSerializer.Deserialize(Of HealthSave)(c("Health"))
                world.Healths.AddComponent(id, New Health With {
                .health = h.health,
                .maxHealth = h.maxHealth
            })
            End If

            If c.ContainsKey("Renders") Then
                Dim r = JsonSerializer.Deserialize(Of RenderSave)(c("Renders"))
                world.Renders.AddComponent(id, New RenderComponent With {
                .size = r.size,
                .spriteX = r.spriteX,
                .spriteY = r.spriteY
            })
            End If

            If c.ContainsKey("Attacks") Then
                Dim a = JsonSerializer.Deserialize(Of AttackSave)(c("Attacks"))
                world.Attacks.AddComponent(id, New AttackComponent With {
                .attack = a.attack,
                .attackCooldown = a.attackCooldown,
                .timeRemaining = a.timeRemaining
            })
            End If

            If c.ContainsKey("Buffs") Then
                Dim b = JsonSerializer.Deserialize(Of BuffSave)(c("Buffs"))
                world.Buffs.AddComponent(id, New BuffComponent With {
                .isConsumed = b.isConsumed,
                .type = b.type
            })
            End If

            If c.ContainsKey("IFrames") Then
                Dim i = JsonSerializer.Deserialize(Of InvincibilitySave)(c("IFrames"))
                world.IFrames.AddComponent(id, New InvincibilityComponent With {
                .timeRemaining = i.timeRemaining
            })
            End If

            If c.ContainsKey("Projectiles") Then
                Dim p = JsonSerializer.Deserialize(Of ProjectileSave)(c("Projectiles"))
                world.Projectiles.AddComponent(id, New ProjectileComponent With {
                .entityType = p.entityType,
                .timeLeft = p.timeLeft
            })
            End If

            If c.ContainsKey("RectCollider") Then
                Dim r = JsonSerializer.Deserialize(Of RectangleColliderSave)(c("RectCollider"))
                world.Colliders.AddComponent(id, New RectangleCollider With {
                .sA = r.sA,
                .sB = r.sB
            })
            End If

            If c.ContainsKey("Enemy") Then
                world.Enemies.AddComponent(id, New EnemyComponent())
            End If

            If c.ContainsKey("Player") Then
                world.Players.AddComponent(id, New PlayerComponent())
            End If

            If c.ContainsKey("Immovable") Then
                world.Immovables.AddComponent(id, New ImmovableComponent())
            End If

        Next

        game.gameState = save.GameState
        game.score = save.Score
        world.PlayerID = save.PlayerID
    End Sub


End Module
