Public Class CollisionResolutionSystem
    Implements ISystem

    Public Sub Update(world As World, dt As Single) Implements ISystem.Update
        For Each ev In world.CollisionEvents
            If world.Immovables.HasComponent(ev.entityA) And world.Immovables.HasComponent(ev.entityB) Then
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

            Dim tA = world.Transforms.GetComponent(ev.entityA)
            Dim tB = world.Transforms.GetComponent(ev.entityB)

            Dim posA = tA.pos
            Dim posB = tB.pos
            Dim s1 = world.Colliders.GetComponent(ev.entityA)
            Dim s2 = world.Colliders.GetComponent(ev.entityB)

            Dim s1a = s1.sA
            Dim s2a = s2.sA
            Dim s1b = s1.sB
            Dim s2b = s2.sB

            Dim newX_A = posA.X
            Dim newY_A = posA.Y
            Dim newX_B = posB.X
            Dim newY_B = posB.Y

            Dim dx = posA.X - posB.X
            Dim dy = posA.Y - posB.Y
            Dim overlapX = (s1a + s2a) / 2 - Math.Abs(dx)
            Dim overlapY = (s1b + s2b) / 2 - Math.Abs(dy)

            If world.Immovables.HasComponent(ev.entityA) Then

                If overlapX < overlapY Then
                    Dim push = overlapX
                    tB.pos = New PointF(posB.X - Math.Sign(dx) * push, posB.Y)
                    If (world.Movements.HasComponent(ev.entityB)) Then
                        Dim mA = world.Movements.GetComponent(ev.entityB)
                        mA.velocity.X = 0
                    End If
                ElseIf overlapY > 0 Then
                    Dim push = overlapY
                    tB.pos = New PointF(posB.X, posB.Y - Math.Sign(dy) * push)
                    If (world.Movements.HasComponent(ev.entityB)) Then
                        Dim mA = world.Movements.GetComponent(ev.entityB)
                        mA.velocity.Y = 0
                    End If
                End If
            ElseIf world.Immovables.HasComponent(ev.entityB) Then
                If overlapX < overlapY Then
                    Dim push = overlapX
                    tA.pos = New PointF(posA.X + Math.Sign(dx) * push, posA.Y)
                    If (world.Movements.HasComponent(ev.entityA)) Then
                        Dim mA = world.Movements.GetComponent(ev.entityA)
                        mA.velocity.X = 0
                    End If
                ElseIf overlapY > 0 Then
                    Dim push = overlapY
                    tA.pos = New PointF(posA.X, posA.Y + Math.Sign(dy) * push)
                    If (world.Movements.HasComponent(ev.entityA)) Then
                        Dim mA = world.Movements.GetComponent(ev.entityA)
                        mA.velocity.Y = 0
                    End If
                End If
            Else
                If overlapX < overlapY Then
                    Dim push = overlapX / 2
                    tA.pos = New PointF(posA.X + Math.Sign(dx) * push, posA.Y)
                    tB.pos = New PointF(posB.X - Math.Sign(dx) * push, posB.Y)
                ElseIf overlapY > 0 Then
                    Dim push = overlapY / 2
                    tA.pos = New PointF(posA.X, posA.Y + Math.Sign(dy) * push)
                    tB.pos = New PointF(posB.X, posB.Y - Math.Sign(dy) * push)
                End If
            End If
        Next
    End Sub

    Public Sub Draw(world As World, g As Graphics) Implements ISystem.Draw
    End Sub
End Class
