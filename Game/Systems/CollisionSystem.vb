Class CollisionSystem
    Implements ISystem

    Private Function isColliding(pos1 As PointF, pos2 As PointF, s1 As Single, s2 As Single) As Boolean
        Dim condition1 = (Math.Abs(pos1.X - pos2.X) <= (s1 + s2) / 2)
        Dim condition2 = (Math.Abs(pos1.Y - pos2.Y) <= (s1 + s2) / 2)
        Return condition1 AndAlso condition2

    End Function
    Public Sub Update(world As World, dt As Single) Implements ISystem.Update
        For Each a In world.Colliders.All
            For Each b In world.Colliders.All
                If a.Key < b.Key Then
                    If isColliding(
                             pos1:=world.Transforms.GetComponent(a.Key).pos,
                             pos2:=world.Transforms.GetComponent(b.Key).pos,
                             s1:=world.Colliders.GetComponent(a.Key).size,
                             s2:=world.Colliders.GetComponent(b.Key).size
                         ) Then
                        world.CollisionEvents.Add(New CollisionEvent With {
                    .entityA = a.Key,
                    .entityB = b.Key
                })
                        Debug.WriteLine("COLLISION")
                    End If
                End If
            Next
        Next
    End Sub

    Public Sub Draw(world As World) Implements ISystem.Draw
    End Sub
End Class


