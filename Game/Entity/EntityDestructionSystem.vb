Imports System.Security.Cryptography.Xml

Public Class EntityDestructionSystem
    Implements ISystem

    Public Sub Update(world As World, dt As Single) Implements ISystem.Update
        If world.EntityDestructionEvents.Count = 0 Then Return

        For Each e In world.EntityDestructionEvents

            ' Remove components from all stores
            world.Transforms.RemoveComponent(e)
            world.Movements.RemoveComponent(e)
            world.Colliders.RemoveComponent(e)
            world.Renders.RemoveComponent(e)
            world.Players.RemoveComponent(e)
            world.Enemies.RemoveComponent(e)
            world.Healths.RemoveComponent(e)
            world.Damages.RemoveComponent(e)
            world.IFrames.RemoveComponent(e)
            world.Immovables.RemoveComponent(e)
            world.Cameras.RemoveComponent(e)
            world.Attacks.RemoveComponent(e)
            world.Projectiles.RemoveComponent(e)
            world.Buffs.RemoveComponent(e)

            ' Remove entity itself
            world.EntityManager.RemoveEntity(e)
            If (e = world.PlayerID) Then
                world.OnPlayerDestruction()
            End If
            'Debug.WriteLine("Destroyed entity: " & e)
        Next
        world.EntityDestructionEvents.Clear()
    End Sub

    Public Sub Draw(world As World, g As Graphics) Implements ISystem.Draw

    End Sub
End Class
