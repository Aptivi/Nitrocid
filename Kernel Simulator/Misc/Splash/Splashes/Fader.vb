
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

Namespace Misc.Splash.Splashes
    Class SplashFader
        Implements ISplash

        'Standalone splash information
        ReadOnly Property SplashName As String Implements ISplash.SplashName
            Get
                Return "Fader"
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

        'Fader-specific variables
        Friend RandomDriver As New Random()
        Friend Left, Top As Integer
        Friend FaderWrite As String = "Kernel Simulator"

        'Actual logic
        Public Sub Opening() Implements ISplash.Opening
            Wdbg(DebugLevel.I, "Splash opening. Clearing console...")
            Console.Clear()
        End Sub

        Public Sub Display() Implements ISplash.Display
            Try
                Wdbg(DebugLevel.I, "Splash displaying.")

                'Select the left and top position
                Left = RandomDriver.Next(Console.WindowWidth)
                Top = RandomDriver.Next(Console.WindowHeight)

                'In case we've selected the left position that is too near the end of buffer, decrement the selected left position
                'so that the text shows up in one line only.
                If FaderWrite.Length + Left >= Console.WindowWidth Then
                    Left -= FaderWrite.Length + 1
                End If

                'Loop until we got a closing notification
                While Not SplashClosing
                    Thread.Sleep(1)
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
            'Fade in as progress is getting reported
            Dim GreenColorLevel As Integer = 255 * (Progress / 100)
            Dim GreenColorInstance As New Color(0, GreenColorLevel, 0)
            SetConsoleColor(GreenColorInstance)
            WriteWhere(FaderWrite, Left, Top, False, GreenColorInstance)
        End Sub

    End Class
End Namespace