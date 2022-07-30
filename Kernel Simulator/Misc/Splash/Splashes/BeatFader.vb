
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
    Class SplashBeatFader
        Implements ISplash

        'Standalone splash information
        ReadOnly Property SplashName As String Implements ISplash.SplashName
            Get
                Return "BeatFader"
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

        'BeatFader-specific variables
        Friend RandomDriver As New Random()
        Friend BeatFaderSettingsInstance As New Animations.BeatFader.BeatFaderSettings With {
            .BeatFader255Colors = False,
            .BeatFaderTrueColor = True,
            .BeatFaderCycleColors = True,
            .BeatFaderBeatColor = 17,
            .BeatFaderDelay = 50,
            .BeatFaderMaxSteps = 30,
            .BeatFaderMinimumRedColorLevel = 0,
            .BeatFaderMinimumGreenColorLevel = 0,
            .BeatFaderMinimumBlueColorLevel = 0,
            .BeatFaderMinimumColorLevel = 0,
            .BeatFaderMaximumRedColorLevel = 255,
            .BeatFaderMaximumGreenColorLevel = 255,
            .BeatFaderMaximumBlueColorLevel = 255,
            .BeatFaderMaximumColorLevel = 255,
            .RandomDriver = RandomDriver
        }

        'Actual logic
        Public Sub Opening() Implements ISplash.Opening
            Wdbg(DebugLevel.I, "Splash opening. Clearing console...")
            Console.Clear()
        End Sub

        Public Sub Display() Implements ISplash.Display
            Try
                Wdbg(DebugLevel.I, "Splash displaying.")

                'Loop until we got a closing notification
                While Not SplashClosing
                    Animations.BeatFader.Simulate(BeatFaderSettingsInstance)
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
