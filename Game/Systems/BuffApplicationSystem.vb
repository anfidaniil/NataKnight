Public Class BuffApplicationSystem
    Implements ISystem

    Public Sub Update(world As World, dt As Single) Implements ISystem.Update
        For Each ev In world.CollisionEvents

            Dim playerID As Integer = -1
            Dim buffID As Integer = -1

            If world.Players.HasComponent(ev.entityA) AndAlso world.Buffs.HasComponent(ev.entityB) Then
                playerID = ev.entityA
                buffID = ev.entityB
            ElseIf world.Buffs.HasComponent(ev.entityA) AndAlso world.Players.HasComponent(ev.entityB) Then
                buffID = ev.entityA
                playerID = ev.entityB
            End If

            If playerID <> -1 AndAlso buffID <> -1 Then
                ApplyBuff(world, playerID, buffID)
            End If
        Next
    End Sub

    Private Sub ApplyBuff(world As World, playerID As Integer, buffID As Integer)
        Dim buff = world.Buffs.GetComponent(buffID)

        If buff.isConsumed Then Return

        Select Case buff.type
            Case BuffType.HealthRegen
                Dim healthBuff = TryCast(buff, HealthBuff)

                If healthBuff IsNot Nothing AndAlso world.Healths.HasComponent(playerID) Then
                    Dim hp = world.Healths.GetComponent(playerID)
                    hp.health += healthBuff.healthRegen

                    If hp.health > hp.maxHealth Then
                        hp.health = hp.maxHealth
                    End If

                    Debug.WriteLine("Curado! Vida: " & hp.health & "/" & hp.maxHealth)
                End If
        End Select

        buff.isConsumed = True
        world.EntityDestructionEvents.Add(buffID)
    End Sub

    Public Sub Draw(world As World, g As Graphics) Implements ISystem.Draw
    End Sub
End Class
