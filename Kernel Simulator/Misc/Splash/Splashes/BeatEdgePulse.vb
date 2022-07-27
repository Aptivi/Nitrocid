
'    Kernel Simulator  Copyright (C) 2018-2022  EoflaOE
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
Imports KS.Misc.Animations.BeatEdgePulse

Namespace Misc.Splash.Splashes
    Class SplashBeatEdgePulse
        Implements ISplash

        'Standalone splash information
        ReadOnly Property SplashName As String Implements ISplash.SplashName
            Get
                Return "BeatEdgePulse"
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

        'BeatEdgePulse-specific variables
        Friend BeatEdgePulseSettings As New BeatEdgePulseSettings With {
            .BeatEdgePulse255Colors = False,
            .BeatEdgePulseTrueColor = True,
            .BeatEdgePulseBeatColor = 17,
            .BeatEdgePulseCycleColors = True,
            .BeatEdgePulseDelay = 50,
            .BeatEdgePulseMaxSteps = 30,
            .BeatEdgePulseMinimumRedColorLevel = 0,
            .BeatEdgePulseMinimumGreenColorLevel = 0,
            .BeatEdgePulseMinimumBlueColorLevel = 0,
            .BeatEdgePulseMinimumColorLevel = 0,
            .BeatEdgePulseMaximumRedColorLevel = 255,
            .BeatEdgePulseMaximumGreenColorLevel = 255,
            .BeatEdgePulseMaximumBlueColorLevel = 255,
            .BeatEdgePulseMaximumColorLevel = 255
        }
        Friend RandomDriver As Random

        'Actual logic
        Public Sub Opening() Implements ISplash.Opening
            Wdbg(DebugLevel.I, "Splash opening. Clearing console...")
            Console.BackgroundColor = ConsoleColor.Black
            Console.Clear()
            RandomDriver = New Random
            BeatEdgePulseSettings.RandomDriver = RandomDriver
        End Sub

        Public Sub Display() Implements ISplash.Display
            Try
                Wdbg(DebugLevel.I, "Splash displaying.")

                'Loop until we got a closing notification
                While Not SplashClosing
                    Simulate(BeatEdgePulseSettings)
                End While
            Catch ex As ThreadInterruptedException
                Wdbg(DebugLevel.I, "Splash done.")
            End Try
        End Sub

        Public Sub Closing() Implements ISplash.Closing
            SplashClosing = True
            Wdbg(DebugLevel.I, "Splash closing. Clearing console...")
            SetConsoleColor(BackgroundColor, True)
            Console.Clear()
        End Sub

        Public Sub Report(Progress As Integer, ProgressReport As String, ParamArray Vars() As Object) Implements ISplash.Report
        End Sub

    End Class
End Namespace
