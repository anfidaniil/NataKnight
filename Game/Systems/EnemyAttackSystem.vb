Public Class EnemyAttackSystem
    Implements ISystem

    Public Sub Update(world As World, dt As Single) Implements ISystem.Update
        For Each kv In world.Enemies.All
            Dim id = kv.Key
            If world.Attacks.HasComponent(id) Then
                Dim a = world.Attacks.GetComponent(id)
                If world.Transforms.HasComponent(id) And world.Transforms.HasComponent(world.PlayerID) Then
                    Dim t = world.Transforms.GetComponent(id)
                    Dim playerPos = world.Transforms.GetComponent(world.PlayerID).pos

                    If a.timeRemaining <= 0 Then
                        a.timeRemaining = 3.0F
                        world.CreateBullet(t.pos, playerPos, 0)
                    End If
                    a.timeRemaining -= dt
                End If
            End If
        Next
    End Sub
    Public Sub Draw(world As World, g As Graphics) Implements ISystem.Draw

    End Sub
End Class
