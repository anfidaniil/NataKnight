Public Class CollisionResolutionSystem
    Implements ISystem

    Public Sub Update(world As World, dt As Single) Implements ISystem.Update
        For Each ev In world.CollisionEvents
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

            If overlapX > 0 AndAlso overlapX < overlapY Then
                Dim push = overlapX / 2
                tA.pos = New PointF(Math.Round(posA.X + Math.Sign(dx) * push), Math.Round(posA.Y))
                tB.pos = New PointF(Math.Round(posB.X - Math.Sign(dx) * push), Math.Round(posB.Y))
            ElseIf overlapY > 0 Then
                Dim push = overlapY / 2
                tA.pos = New PointF(Math.Round(posA.X), Math.Round(posA.Y + Math.Sign(dy) * push))
                tB.pos = New PointF(Math.Round(posB.X), Math.Round(posB.Y - Math.Sign(dy) * push))
            End If
        Next
    End Sub

    Public Sub Draw(world As World) Implements ISystem.Draw
    End Sub
End Class
