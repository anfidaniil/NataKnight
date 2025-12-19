Public Class DamageSystem
    Implements ISystem
    Public Sub Update(world As World, dt As Single) Implements ISystem.Update
        For Each kv In world.CollisionEvents
            If world.Damages.HasComponent(kv.entityA) And world.Healths.HasComponent(kv.entityB) Then
                Dim hc = world.Healths.GetComponent(kv.entityB)
                Dim dc = world.Damages.GetComponent(kv.entityA)

                hc.health = hc.health - dc.damage
                If hc.health < 0 Then
                    world.EntityDestructionEvents.Add(
                        New EntityDestructionEvent With {
                            .entityID = kv.entityB
                        }
                    )
                End If
            End If

            If world.Damages.HasComponent(kv.entityB) And world.Healths.HasComponent(kv.entityA) Then
                Dim hc = world.Healths.GetComponent(kv.entityA)
                Dim dc = world.Damages.GetComponent(kv.entityB)

                hc.health = hc.health - dc.damage
                If hc.health < 0 Then
                    world.EntityDestructionEvents.Add(
                        New EntityDestructionEvent With {
                            .entityID = kv.entityA
                        }
                    )
                End If
            End If
        Next
    End Sub

    Public Sub Draw(world As World) Implements ISystem.Draw
    End Sub
End Class
