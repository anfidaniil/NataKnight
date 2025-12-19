Public Class InvincibilitySystem
    Implements ISystem
    Public Sub Update(world As World, dt As Single) Implements ISystem.Update
        For Each kv In world.IFrames.All
            Dim iFrame = world.IFrames.GetComponent(kv.Key)
            iFrame.timeRemaining = iFrame.timeRemaining - dt

            If (iFrame.timeRemaining < 0) Then
                world.IFrames.RemoveComponent(kv.Key)
            End If
        Next
    End Sub

    Public Sub Draw(world As World) Implements ISystem.Draw

    End Sub
End Class
