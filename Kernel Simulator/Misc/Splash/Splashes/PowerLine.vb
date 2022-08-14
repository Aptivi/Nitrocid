
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

Namespace Misc.Splash.Splashes
    Class SplashPowerLine
        Implements ISplash

        'Standalone splash information
        ReadOnly Property SplashName As String Implements ISplash.SplashName
            Get
                Return "PowerLine"
            End Get
        End Property

        Private ReadOnly Property Info As SplashInfo
            Get
                Return SplashManager.Splashes(SplashName)
            End Get
        End Property

        'Property implementations
        Property SplashClosing As Boolean Implements ISplash.SplashClosing

        ReadOnly Property SplashDisplaysProgress As Boolean Implements ISplash.SplashDisplaysProgress
            Get
                Return Info.DisplaysProgress
            End Get
        End Property

        Private FirstColorSegmentBackground As Color = Color.Empty
        Private LastTransitionForeground As Color = Color.Empty
        Private PowerLineLength As Integer = 0
        Private LengthDecreasing As Boolean
        Private ReadOnly TransitionChar As Char = Convert.ToChar(&HE0B0)
        Private ReadOnly RandomDriver As New Random()

        'Actual logic
        Public Sub Opening() Implements ISplash.Opening
            Wdbg(DebugLevel.I, "Splash opening. Clearing console...")
            Console.Clear()

            'Select the color segment background and mirror it to the transition foreground color
            FirstColorSegmentBackground = New Color(RandomDriver.Next(256), RandomDriver.Next(256), RandomDriver.Next(256))
            LastTransitionForeground = FirstColorSegmentBackground
        End Sub

        Public Sub Display() Implements ISplash.Display
            Try
                Wdbg(DebugLevel.I, "Splash displaying.")
                While Not SplashClosing
                    'As the length increases, draw the PowerLine lines
                    For Top As Integer = 0 To Console.WindowHeight - 1
                        If SplashClosing Then Exit For
                        WriteWhere(" ".Repeat(PowerLineLength), 0, Top, Color.Empty, FirstColorSegmentBackground)
                        WriteWhere(TransitionChar, PowerLineLength, Top, LastTransitionForeground)
                        ClearLineToRight()
                    Next

                    'Increase the length until we reach the window width, then decrease it.
                    If LengthDecreasing Then
                        PowerLineLength -= 1

                        'If we reached the start, increase the length
                        If PowerLineLength = 0 Then
                            LengthDecreasing = False
                        End If
                    Else
                        PowerLineLength += 1

                        'If we reached the end, decrease the length
                        If PowerLineLength = Console.WindowWidth - 1 Then
                            LengthDecreasing = True
                        End If
                    End If

                    'Sleep to draw
                    Thread.Sleep(10)
                End While
            Catch ex As ThreadInterruptedException
                Wdbg(DebugLevel.I, "Splash done.")
            End Try
        End Sub

        Public Sub Closing() Implements ISplash.Closing
            SplashClosing = True
            Wdbg(DebugLevel.I, "Splash closing. Clearing console...")
            SetConsoleColor(ColTypes.Neutral)
            SetConsoleColor(BackgroundColor, True)
            Console.Clear()
        End Sub

        Public Sub Report(Progress As Integer, ProgressReport As String, ParamArray Vars() As Object) Implements ISplash.Report
        End Sub

    End Class
End Namespace
