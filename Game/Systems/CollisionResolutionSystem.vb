Public Class CollisionResolutionSystem
    Implements ISystem

    Public Sub Update(world As World, dt As Single) Implements ISystem.Update
        For Each ev In world.CollisionEvents
            If world.Immovables.HasComponent(ev.entityA) And world.Immovables.HasComponent(ev.entityB) Then
                Continue For
            End If

            Dim tA = world.Transforms.GetComponent(ev.entityA)
            Dim tB = world.Transforms.GetComponent(ev.entityB)

            Dim posA = tA.pos
            Dim posB = tB.pos
            Dim sA = world.Colliders.GetComponent(ev.entityA).size
            Dim sB = world.Colliders.GetComponent(ev.entityB).size

            Dim newX_A = posA.X
            Dim newY_A = posA.Y
            Dim newX_B = posB.X
            Dim newY_B = posB.Y

            Dim dx = posA.X - posB.X
            Dim dy = posA.Y - posB.Y
            Dim overlapX = (sA + sB) / 2 - Math.Abs(dx)
            Dim overlapY = (sA + sB) / 2 - Math.Abs(dy)

            If world.Immovables.HasComponent(ev.entityA) Then
                If overlapX < overlapY Then
                    Dim push = overlapX
                    tB.pos = New PointF(posB.X - Math.Sign(dx) * push, posB.Y)
                ElseIf overlapY > 0 Then
                    Dim push = overlapY
                    tB.pos = New PointF(posB.X, posB.Y - Math.Sign(dy) * push)
                End If
            ElseIf world.Immovables.HasComponent(ev.entityB) Then
                If overlapX < overlapY Then
                    Dim push = overlapX
                    tA.pos = New PointF(posA.X + Math.Sign(dx) * push, posA.Y)
                ElseIf overlapY > 0 Then
                    Dim push = overlapY
                    tA.pos = New PointF(posA.X, posA.Y + Math.Sign(dy) * push)
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
