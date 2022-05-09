
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
    Class SplashSimple
        Implements ISplash

        'Standalone splash information
        ReadOnly Property SplashName As String Implements ISplash.SplashName
            Get
                Return "Simple"
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

        ReadOnly Property ProgressWritePositionX As Integer Implements ISplash.ProgressWritePositionX
            Get
                Return Info.ProgressWritePositionX
            End Get
        End Property

        ReadOnly Property ProgressWritePositionY As Integer Implements ISplash.ProgressWritePositionY
            Get
                Select Case SimpleProgressTextLocation
                    Case TextLocation.Top
                        Return Info.ProgressWritePositionY
                    Case TextLocation.Bottom
                        Return Console.WindowHeight - 2
                    Case Else
                        Return Info.ProgressWritePositionY
                End Select
            End Get
        End Property

        ReadOnly Property ProgressReportWritePositionX As Integer Implements ISplash.ProgressReportWritePositionX
            Get
                Return Info.ProgressReportWritePositionX
            End Get
        End Property

        ReadOnly Property ProgressReportWritePositionY As Integer Implements ISplash.ProgressReportWritePositionY
            Get
                Select Case SimpleProgressTextLocation
                    Case TextLocation.Top
                        Return Info.ProgressReportWritePositionY
                    Case TextLocation.Bottom
                        Return Console.WindowHeight - 2
                    Case Else
                        Return Info.ProgressReportWritePositionY
                End Select
            End Get
        End Property

        'Actual logic
        Public Sub Opening() Implements ISplash.Opening
            Wdbg(DebugLevel.I, "Splash opening. Clearing console...")
            Console.Clear()
        End Sub

        Public Sub Display() Implements ISplash.Display
            Wdbg(DebugLevel.I, "Splash displaying.")

            'Display the progress text
            UpdateProgressReport(Progress, ProgressText, ProgressWritePositionX, ProgressWritePositionY, ProgressReportWritePositionX, ProgressReportWritePositionY)

            'Loop until closing
            While Not SplashClosing
                Thread.Sleep(1)
            End While
            Wdbg(DebugLevel.I, "Splash done.")
        End Sub

        Public Sub Closing() Implements ISplash.Closing
            SplashClosing = True
            Wdbg(DebugLevel.I, "Splash closing. Clearing console...")
            Console.Clear()
        End Sub

        Public Sub Report(Progress As Integer, ProgressReport As String, ProgressWritePositionX As Integer, ProgressWritePositionY As Integer, ProgressReportWritePositionX As Integer, ProgressReportWritePositionY As Integer, ParamArray Vars() As Object) Implements ISplash.Report
            UpdateProgressReport(Progress, ProgressReport, ProgressWritePositionX, ProgressWritePositionY, ProgressReportWritePositionX, ProgressReportWritePositionY, Vars)
        End Sub

        ''' <summary>
        ''' Updates the splash progress
        ''' </summary>
        ''' <param name="Progress">Progress percentage from 0 to 100</param>
        ''' <param name="ProgressReport">The progress text</param>
        ''' <param name="ProgressWritePositionX">The left position of the progress write position</param>
        ''' <param name="ProgressWritePositionY">The top position of the progress write position</param>
        ''' <param name="ProgressReportWritePositionX">The left position of the progress report write position</param>
        ''' <param name="ProgressReportWritePositionY">The top position of the progress report write position</param>
        Sub UpdateProgressReport(Progress As Integer, ProgressReport As String, ProgressWritePositionX As Integer, ProgressWritePositionY As Integer, ProgressReportWritePositionX As Integer, ProgressReportWritePositionY As Integer, ParamArray Vars() As Object)
            Dim RenderedText As String = ProgressReport.Truncate(Console.WindowWidth - ProgressReportWritePositionX - ProgressWritePositionX - 3)
            WriteWhere("{0}%", ProgressWritePositionX, ProgressWritePositionY, True, ColTypes.Progress, Progress.ToString.PadLeft(3))
            WriteWhere(RenderedText, ProgressReportWritePositionX, ProgressReportWritePositionY, False, ColTypes.Neutral, Vars)
            ClearLineToRight()
        End Sub

    End Class
End Namespace
