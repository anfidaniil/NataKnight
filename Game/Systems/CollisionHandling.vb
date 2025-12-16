Public Class CollisionHandling
    Implements ISystem
    Public Sub Update(world As World, dt As Single) Implements ISystem.Update
        For Each ev In world.CollisionEvents
            If world.Renders.HasComponent(ev.entityA) Then
                world.CreateStain(world.Transforms.GetComponent(ev.entityA).pos, world.Transforms.GetComponent(world.PlayerID).pos)
            End If
            If world.Renders.HasComponent(ev.entityB) Then
            End If
        Next

    End Sub

    Public Sub Draw(world As World) Implements ISystem.Draw
    End Sub
End Class
