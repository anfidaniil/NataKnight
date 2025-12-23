Public Class DamageSystem
    Implements ISystem
    Public Sub Update(world As World, dt As Single) Implements ISystem.Update
        For Each ev In world.CollisionEvents

            If world.Enemies.HasComponent(ev.entityB) And world.Enemies.HasComponent(ev.entityA) Then
                Continue For
            End If

            If world.Projectiles.HasComponent(ev.entityA) Then
                If world.Projectiles.GetComponent(ev.entityA).entityType = 0 And world.Enemies.HasComponent(ev.entityB) Then
                    Continue For
                End If
                If world.Projectiles.GetComponent(ev.entityA).entityType = 1 And world.Players.HasComponent(ev.entityB) Then
                    Continue For
                End If
            End If
            If world.Projectiles.HasComponent(ev.entityB) Then
                If world.Projectiles.GetComponent(ev.entityB).entityType = 0 And world.Enemies.HasComponent(ev.entityA) Then
                    Continue For
                End If
                If world.Projectiles.GetComponent(ev.entityB).entityType = 1 And world.Players.HasComponent(ev.entityA) Then
                    Continue For
                End If
            End If

            If world.Damages.HasComponent(ev.entityA) And world.Healths.HasComponent(ev.entityB) Then
                Dim hc = world.Healths.GetComponent(ev.entityB)
                Dim dc = world.Damages.GetComponent(ev.entityA)


                If world.IFrames.HasComponent(ev.entityB) Then
                    'Debug.WriteLine("Entity is invincible")
                    Continue For
                End If

                hc.health = hc.health - dc.damage

                world.IFrames.AddComponent(
                    ev.entityB,
                    New InvincibilityComponent With {
                        .timeRemaining = World.IFRAMES_DURATION
                    }
                )

                If hc.health < 0 Then
                    world.EntityDestructionEvents.Add(
                        New EntityDestructionEvent With {
                            .entityID = ev.entityB
                        }
                    )
                End If
            End If

            If world.Damages.HasComponent(ev.entityB) And world.Healths.HasComponent(ev.entityA) Then
                Dim hc = world.Healths.GetComponent(ev.entityA)
                Dim dc = world.Damages.GetComponent(ev.entityB)

                If world.IFrames.HasComponent(ev.entityA) Then
                    'Debug.WriteLine("Entity is invincible")
                    Return
                End If

                hc.health = hc.health - dc.damage

                If hc.health < 0 Then
                    world.EntityDestructionEvents.Add(
                        New EntityDestructionEvent With {
                            .entityID = ev.entityA
                        }
                    )
                End If
            End If

            If (world.Projectiles.HasComponent(ev.entityA)) Then
                world.EntityDestructionEvents.Add(
                        New EntityDestructionEvent With {
                            .entityID = ev.entityA
                        }
                    )
            End If

            If (world.Projectiles.HasComponent(ev.entityB)) Then
                world.EntityDestructionEvents.Add(
                    New EntityDestructionEvent With {
                        .entityID = ev.entityB
                    }
                )
            End If
        Next
    End Sub

    Public Sub Draw(world As World, g As Graphics) Implements ISystem.Draw
    End Sub
End Class
