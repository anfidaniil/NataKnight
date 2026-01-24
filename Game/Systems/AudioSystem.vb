Class AudioSystem
    Implements ISystem

    Public Sub Update(world As World, dt As Single) Implements ISystem.Update
        For Each e In world.AudioSources.All
            If world.AudioTriggers.HasComponent(e.Key) Then
                Dim src = e.Value
                Dim trig = world.AudioTriggers.GetComponent(e.Key)

                If trig.PlayRequested Then
                    AudioEngine.PlayOneShot(src.SoundId, src.Volume)
                    trig.PlayRequested = False
                End If
            End If
        Next
    End Sub

    Public Sub Draw(world As World, g As Graphics) Implements ISystem.Draw

    End Sub
End Class
