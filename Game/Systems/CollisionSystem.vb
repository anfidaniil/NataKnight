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
    Public Sub ShallowPass(world As World)
        world.CollisionLookupTable.Clear()

        For Each c In world.Colliders.All
            If Not world.Transforms.HasComponent(c.Key) Then Continue For

            Dim pos = world.Transforms.GetComponent(c.Key).pos
            Dim col = world.Colliders.GetComponent(c.Key)

            Dim halfW As Single = col.sA / 2.0F
            Dim halfH As Single = col.sB / 2.0F

            Dim minX As Single = pos.X - halfW
            Dim maxX As Single = pos.X + halfW
            Dim minY As Single = pos.Y - halfH
            Dim maxY As Single = pos.Y + halfH

            Dim minCellX As Integer = CInt(Math.Floor(minX / World.TILE_SIZE))
            Dim maxCellX As Integer = CInt(Math.Floor(maxX / World.TILE_SIZE))
            Dim minCellY As Integer = CInt(Math.Floor(minY / World.TILE_SIZE))
            Dim maxCellY As Integer = CInt(Math.Floor(maxY / World.TILE_SIZE))

            For cx = minCellX To maxCellX
                For cy = minCellY To maxCellY
                    Dim cell As New Point(cx, cy)

                    If Not world.CollisionLookupTable.ContainsKey(cell) Then
                        world.CollisionLookupTable(cell) = New List(Of Integer)
                    End If

                    world.CollisionLookupTable(cell).Add(c.Key)
                Next
            Next
        Next
    End Sub

    Public Sub Update(world As World, dt As Single) Implements ISystem.Update
        ShallowPass(world)

        For Each id In world.CollisionLookupTable.Keys
            For dx = -1 To 1
                For dy = -1 To 1
                    If dx < 0 OrElse (dx = 0 AndAlso dy < 0) Then Continue For

                    Dim neighbor As New Point(id.X + dx, id.Y + dy)
                    If Not world.CollisionLookupTable.ContainsKey(neighbor) Then Continue For


                    For Each a In world.CollisionLookupTable(id)
                        For Each b In world.CollisionLookupTable(neighbor)
                            If a < b Then
                                If world.Immovables.HasComponent(a) And world.Immovables.HasComponent(b) Then
                                    Continue For
                                End If
                                If world.Projectiles.HasComponent(a) And world.Projectiles.HasComponent(b) Then
                                    If world.Projectiles.GetComponent(a).entityType = world.Projectiles.GetComponent(b).entityType Then
                                        Continue For
                                    End If
                                End If

                                If isColliding(a, b, world) Then
                                    world.CollisionEvents.Add(New CollisionEvent With {
                                        .entityA = a,
                                        .entityB = b})
                                End If
                            End If
                        Next
                    Next
                Next
            Next
        Next
    End Sub

    Public Sub Draw(world As World, g As Graphics) Implements ISystem.Draw
    End Sub
End Class


