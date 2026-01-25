Imports System.Numerics

Class AudioSystem
    Implements ISystem

    Private Shared rng As New Random()

    Public Sub Update(world As World, dt As Single) Implements ISystem.Update
        If Not world.Transforms.HasComponent(world.PlayerID) Then Return

        Dim listenerPos = world.Transforms.GetComponent(world.PlayerID).pos

        For Each e In world.AudioSources.All
            If Not world.AudioTriggers.HasComponent(e.Key) Then Continue For

            Dim src = e.Value
            Dim trig = world.AudioTriggers.GetComponent(e.Key)

            If Not trig.playRequested Then Continue For

            Dim id = rng.Next(0, src.soundId.Count)

            Dim volume As Single = src.volume

            If world.Transforms.HasComponent(e.Key) Then
                Dim sourcePos = world.Transforms.GetComponent(e.Key).pos

                Dim d = Vector2.Distance(sourcePos, listenerPos)

                Dim attenuation As Single =
                    src.volume - ((d - World.MIN_AUDIO_DIST) / (World.MAX_AUDIO_DIST - World.MIN_AUDIO_DIST))

                attenuation = Math.Clamp(attenuation, 0.0F, 1.0F)
                volume *= attenuation
            End If

            AudioEngine.PlayOneShot(src.soundId(id), volume)
            trig.playRequested = False
        Next
    End Sub

    Public Sub Draw(world As World, g As Graphics) Implements ISystem.Draw
    End Sub
End Class
