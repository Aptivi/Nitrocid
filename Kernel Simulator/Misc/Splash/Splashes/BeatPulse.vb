
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
Imports KS.Misc.Animations.BeatPulse

Namespace Misc.Splash.Splashes
    Class SplashBeatPulse
        Implements ISplash

        'Standalone splash information
        ReadOnly Property SplashName As String Implements ISplash.SplashName
            Get
                Return "BeatPulse"
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

        'BeatPulse-specific variables
        Friend BeatPulseSettings As New BeatPulseSettings With {
            .BeatPulse255Colors = False,
            .BeatPulseTrueColor = True,
            .BeatPulseBeatColor = 17,
            .BeatPulseCycleColors = True,
            .BeatPulseDelay = 50,
            .BeatPulseMaxSteps = 30,
            .BeatPulseMinimumRedColorLevel = 0,
            .BeatPulseMinimumGreenColorLevel = 0,
            .BeatPulseMinimumBlueColorLevel = 0,
            .BeatPulseMinimumColorLevel = 0,
            .BeatPulseMaximumRedColorLevel = 255,
            .BeatPulseMaximumGreenColorLevel = 255,
            .BeatPulseMaximumBlueColorLevel = 255,
            .BeatPulseMaximumColorLevel = 255
        }
        Friend RandomDriver As Random

        'Actual logic
        Public Sub Opening() Implements ISplash.Opening
            Wdbg(DebugLevel.I, "Splash opening. Clearing console...")
            Console.BackgroundColor = ConsoleColor.Black
            Console.Clear()
            RandomDriver = New Random
            BeatPulseSettings.RandomDriver = RandomDriver
        End Sub

        Public Sub Display() Implements ISplash.Display
            Try
                Wdbg(DebugLevel.I, "Splash displaying.")

                'Loop until we got a closing notification
                While Not SplashClosing
                    Simulate(BeatPulseSettings)
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
