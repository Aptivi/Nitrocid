
'    Kernel Simulator  Copyright (C) 2018-2022  Aptivi
'
'    This file is part of Kernel Simulator
'
'    Kernel Simulator is free software: you can redistribute it and/or modify
'    it under the terms of the GNU General Public License as published by
'    the Free Software Foundation, either version 3 of the License, or
'    (at your option) any later version.
'
'    Kernel Simulator is distributed in the hope that it will be useful,
'    but WITHOUT ANY WARRANTY; without even the implied warranty of
'    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
'    GNU General Public License for more details.
'
'    You should have received a copy of the GNU General Public License
'    along with this program.  If not, see <https://www.gnu.org/licenses/>.

Imports System.Threading

Namespace Misc.Games
    Public Module MeteorShooter

        'TODO: Make these configurable in day-one patch
        Public MeteorUsePowerLine As Boolean = True
        Private SpaceshipHeight As Integer = 0
        Private GameEnded As Boolean = False
        Private ReadOnly MaxBullets As Integer = 10
        Private ReadOnly Bullets As New List(Of Tuple(Of Integer, Integer))
        Private ReadOnly MaxMeteors As Integer = 10
        Private ReadOnly Meteors As New List(Of Tuple(Of Integer, Integer))
        Private ReadOnly MeteorDrawThread As New KernelThread("Meteor Shooter Draw Thread", True, AddressOf DrawGame)
        Private ReadOnly RandomDriver As New Random

        ''' <summary>
        ''' Initializes the Meteor game
        ''' </summary>
        Sub InitializeMeteor()
            'Clear all bullets and meteors
            Bullets.Clear()
            Meteors.Clear()

            'Make the spaceship height in the center
            SpaceshipHeight = Console.WindowHeight / 2

            'Start the draw thread
            MeteorDrawThread.Stop()
            MeteorDrawThread.Start()

            'Remove the cursor
            Console.CursorVisible = False

            'Now, handle input
            Dim Keypress As ConsoleKeyInfo
            While Not GameEnded
                If Console.KeyAvailable Then
                    'Read the key
                    Keypress = Console.ReadKey(True)

                    'Select command based on key value
                    Select Case Keypress.Key
                        Case ConsoleKey.UpArrow
                            If SpaceshipHeight > 0 Then SpaceshipHeight -= 1
                        Case ConsoleKey.DownArrow
                            If SpaceshipHeight < Console.WindowHeight Then SpaceshipHeight += 1
                        Case ConsoleKey.Spacebar
                            If Bullets.Count < MaxBullets Then Bullets.Add(New Tuple(Of Integer, Integer)(1, SpaceshipHeight))
                        Case ConsoleKey.Escape
                            GameEnded = True
                    End Select
                End If
            End While

            'Stop the draw thread since the game ended
            MeteorDrawThread.Wait()
            MeteorDrawThread.Stop()
            GameEnded = False
        End Sub

        Sub DrawGame()
            Try
                While Not GameEnded
                    'Clear screen
                    Console.Clear()

                    'Move the meteors left
                    For Meteor As Integer = 0 To Meteors.Count - 1
                        Dim MeteorX As Integer = Meteors(Meteor).Item1 - 1
                        Dim MeteorY As Integer = Meteors(Meteor).Item2
                        Meteors(Meteor) = New Tuple(Of Integer, Integer)(MeteorX, MeteorY)
                    Next

                    'Move the bullets right
                    For Bullet As Integer = 0 To Bullets.Count - 1
                        Dim BulletX As Integer = Bullets(Bullet).Item1 + 1
                        Dim BulletY As Integer = Bullets(Bullet).Item2
                        Bullets(Bullet) = New Tuple(Of Integer, Integer)(BulletX, BulletY)
                    Next

                    'If any bullet is out of X range, delete it
                    For BulletIndex As Integer = Bullets.Count - 1 To 0 Step -1
                        Dim Bullet = Bullets(BulletIndex)
                        If Bullet.Item1 >= Console.WindowWidth Then
                            'The bullet went beyond. Remove it.
                            Bullets.RemoveAt(BulletIndex)
                        End If
                    Next

                    'If any meteor is out of X range, delete it
                    For MeteorIndex As Integer = Meteors.Count - 1 To 0 Step -1
                        Dim Meteor = Meteors(MeteorIndex)
                        If Meteor.Item1 < 0 Then
                            'The meteor went beyond. Remove it.
                            Meteors.RemoveAt(MeteorIndex)
                        End If
                    Next

                    'Add new meteor if guaranteed
                    Dim MeteorShowProbability As Double = 10 / 100
                    Dim MeteorShowGuaranteed As Boolean = RandomDriver.NextDouble < MeteorShowProbability
                    If MeteorShowGuaranteed And Meteors.Count < MaxMeteors Then
                        Dim MeteorX As Integer = Console.WindowWidth - 1
                        Dim MeteorY As Integer = RandomDriver.Next(Console.WindowHeight - 1)
                        Meteors.Add(New Tuple(Of Integer, Integer)(MeteorX, MeteorY))
                    End If

                    'Draw the meteor, the bullet, and the spaceship if any of them are updated
                    DrawSpaceship()
                    For MeteorIndex As Integer = Meteors.Count - 1 To 0 Step -1
                        Dim Meteor = Meteors(MeteorIndex)
                        DrawMeteor(Meteor.Item1, Meteor.Item2)
                    Next
                    For BulletIndex As Integer = Bullets.Count - 1 To 0 Step -1
                        Dim Bullet = Bullets(BulletIndex)
                        DrawBullet(Bullet.Item1, Bullet.Item2)
                    Next

                    'Check to see if the spaceship is blown up
                    For MeteorIndex As Integer = Meteors.Count - 1 To 0 Step -1
                        Dim Meteor = Meteors(MeteorIndex)
                        If Meteor.Item1 = 0 And Meteor.Item2 = SpaceshipHeight Then
                            'The spaceship crashed! Game ended.
                            GameEnded = True
                        End If
                    Next

                    'Check to see if the meteor is blown up
                    For MeteorIndex As Integer = Meteors.Count - 1 To 0 Step -1
                        Dim Meteor = Meteors(MeteorIndex)
                        For BulletIndex As Integer = Bullets.Count - 1 To 0 Step -1
                            Dim Bullet = Bullets(BulletIndex)
                            If Meteor.Item1 <= Bullet.Item1 And Meteor.Item2 = Bullet.Item2 Then
                                'The meteor crashed! Remove both the bullet and the meteor
                                Bullets.RemoveAt(BulletIndex)
                                Meteors.RemoveAt(MeteorIndex)
                            End If
                        Next
                    Next

                    'Wait for a few milliseconds
                    SleepNoBlock(10, MeteorDrawThread)
                End While
            Catch ex As ThreadInterruptedException
                'Game is over. Move to the Finally block.
            Catch ex As Exception
                'Game is over with an unexpected error.
                WriteWhere(DoTranslation("Unexpected error") + ": {0}", 0, Console.WindowHeight - 1, False, ConsoleColor.Red, ex.Message)
                SleepNoBlock(3000, MeteorDrawThread)
                Console.Clear()
            Finally
                'Write game over
                WriteWhere(DoTranslation("Game over"), 0, Console.WindowHeight - 1, False, ConsoleColor.Red)
                SleepNoBlock(3000, MeteorDrawThread)
                Console.Clear()
            End Try
        End Sub

        Sub DrawSpaceship()
            Dim PowerLineSpaceship As Char = Convert.ToChar(&HE0B0)
            Dim SpaceshipSymbol As Char = If(MeteorUsePowerLine, PowerLineSpaceship, ">"c)
            WriteWhere(SpaceshipSymbol, 0, SpaceshipHeight, False, ConsoleColor.Green)
        End Sub

        Sub DrawMeteor(MeteorX As Integer, MeteorY As Integer)
            Dim MeteorSymbol As Char = "*"c
            WriteWhere(MeteorSymbol, MeteorX, MeteorY, False, ConsoleColor.Red)
        End Sub

        Sub DrawBullet(BulletX As Integer, BulletY As Integer)
            Dim BulletSymbol As Char = "-"c
            WriteWhere(BulletSymbol, BulletX, BulletY, False, ConsoleColor.Cyan)
        End Sub

    End Module
End Namespace
