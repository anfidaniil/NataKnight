Public Class RenderSystem

    Implements ISystem

    Private g As Graphics

    Public Sub New(graphics As Graphics)
        g = graphics
    End Sub

    Public Sub Draw(world As World)
        g.Clear(Color.Black)
        For Each kv In world.Transforms.All
            Dim id = kv.Key
            If world.Renders.HasComponent(id) Then
                Dim t = kv.Value
                Dim r = world.Renders.GetComponent(id)
                g.FillRectangle(r.brush, t.pos.X, t.pos.Y, r.size, r.size)
            End If
        Next
    End Sub

    Public Sub Update(world As World, dt As Single) Implements ISystem.Update

    End Sub

    Private Sub ISystem_Draw(world As World) Implements ISystem.Draw
        Draw(world)
    End Sub
End Class
