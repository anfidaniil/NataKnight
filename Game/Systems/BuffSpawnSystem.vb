Public Class BuffsSpawnSystem
    Implements ISystem

    Private spawnTimer As Single = 0
    Private spawnInterval As Single = 15.0F
    Private maxBuffs As Integer = 5
    Private rng As New Random()

    Private Const SAFE_MIN As Integer = 1300
    Private Const SAFE_MAX As Integer = 2800

    Public Sub Update(world As World, dt As Single) Implements ISystem.Update
        spawnTimer -= dt

        If spawnTimer <= 0 Then
            spawnTimer = spawnInterval

            If world.Buffs.All.Count < maxBuffs Then
                Dim x As Single = rng.Next(SAFE_MIN, SAFE_MAX)
                Dim y As Single = rng.Next(SAFE_MIN, SAFE_MAX)

                CreateHealthBuff(world, New PointF(x, y))
            End If
        End If
    End Sub

    Private Sub CreateHealthBuff(world As World, pos As PointF)
        Dim entity = world.EntityManager.CreateEntity()

        world.Transforms.AddComponent(entity, New TransformComponent With {
            .pos = pos
        })

        world.Renders.AddComponent(entity, New RenderComponent With {
            .size = 32,
            .spriteX = 0,
            .spriteY = 3
        })

        world.Colliders.AddComponent(entity, New RectangleCollider With {
            .sA = 32,
            .sB = 32
        })

        world.Buffs.AddComponent(entity, New HealthBuff With {
            .healthRegen = 25
        })
    End Sub

    Public Sub Draw(world As World, g As Graphics) Implements ISystem.Draw
    End Sub
End Class