Public Class BuffApplicationSystem
    Implements ISystem

    Public Sub Update(world As World, dt As Single) Implements ISystem.Update
        For Each ev In world.CollisionEvents

            Dim aIsBuff = world.Buffs.HasComponent(ev.entityA)
            Dim bIsBuff = world.Buffs.HasComponent(ev.entityB)

            If Not aIsBuff AndAlso Not bIsBuff Then
                Continue For
            End If

            Dim buffID As Integer
            Dim candidatePlayerID As Integer

            If aIsBuff Then
                buffID = ev.entityA
                candidatePlayerID = ev.entityB
            Else
                buffID = ev.entityB
                candidatePlayerID = ev.entityA
            End If

            If Not world.Players.HasComponent(candidatePlayerID) Then
                Continue For
            End If

            Dim buff = world.Buffs.GetComponent(buffID)
            If buff.isConsumed Then
                Continue For
            End If

            ApplyBuff(world, candidatePlayerID, buff, buffID)
        Next
    End Sub

    Private Sub ApplyBuff(world As World, playerID As Integer, buff As BuffComponent, buffID As Integer)

        Select Case buff.type
            Case BuffType.HealthRegen
                Dim healthBuff = TryCast(buff, HealthBuff)

                If healthBuff Is Nothing OrElse Not world.Healths.HasComponent(playerID) Then
                    Return
                End If

                Dim hp = world.Healths.GetComponent(playerID)

                hp.health += healthBuff.healthRegen

                If hp.health > hp.maxHealth Then
                    hp.health = hp.maxHealth
                End If
        End Select

        buff.isConsumed = True
        world.EntityDestructionEvents.Add(buffID)
    End Sub

    Public Sub Draw(world As World, g As Graphics) Implements ISystem.Draw
    End Sub
End Class