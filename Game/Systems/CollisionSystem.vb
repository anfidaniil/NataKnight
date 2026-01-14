Class CollisionSystem
    Implements ISystem

    Private Function isColliding(entityA As Integer, entityB As Integer, world As World) As Boolean
        Dim pos1 = world.Transforms.GetComponent(entityA).pos
        Dim pos2 = world.Transforms.GetComponent(entityB).pos
        Dim s1 = world.Colliders.GetComponent(entityA)
        Dim s2 = world.Colliders.GetComponent(entityB)

        Dim s1a = s1.sA
        Dim s2a = s2.sA
        Dim s1b = s1.sB
        Dim s2b = s2.sB

        Dim condition1 = (Math.Abs(pos1.X - pos2.X) <= (s1a + s2a) / 2)
        Dim condition2 = (Math.Abs(pos1.Y - pos2.Y) <= (s1b + s2b) / 2)
        Return condition1 AndAlso condition2

    End Function
    Public Sub Update(world As World, dt As Single) Implements ISystem.Update
        For Each a In world.Colliders.All
            For Each b In world.Colliders.All
                If a.Key < b.Key Then
                    If world.Immovables.HasComponent(a.Key) And world.Immovables.HasComponent(b.Key) Then
                        Continue For
                    End If
                    If world.Projectiles.HasComponent(a.Key) And world.Projectiles.HasComponent(b.Key) Then
                        If world.Projectiles.GetComponent(a.Key).entityType = world.Projectiles.GetComponent(b.Key).entityType Then
                            Continue For
                        End If
                    End If

                    If isColliding(
                             a.Key,
                             b.Key,
                             world
                         ) Then
                        world.CollisionEvents.Add(New CollisionEvent With {
                    .entityA = a.Key,
                    .entityB = b.Key
                })
                        'Debug.WriteLine("COLLISION")
                    End If
                End If
            Next
        Next
    End Sub

    Public Sub Draw(world As World, g As Graphics) Implements ISystem.Draw
    End Sub
End Class


