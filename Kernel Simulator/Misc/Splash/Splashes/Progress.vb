
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

Class SplashProgress
    Implements ISplash

    'Standalone splash information
    ReadOnly Property SplashName As String Implements ISplash.SplashName
        Get
            Return "Progress"
        End Get
    End Property

    Private ReadOnly Property Info As SplashInfo
        Get
            Return Splashes(SplashName)
        End Get
    End Property

    'Property implementations
    Property SplashClosing As Boolean Implements ISplash.SplashClosing

    ReadOnly Property ProgressWritePositionX As Integer Implements ISplash.ProgressWritePositionX
        Get
            Return Info.ProgressWritePositionX
        End Get
    End Property

    ReadOnly Property ProgressWritePositionY As Integer Implements ISplash.ProgressWritePositionY
        Get
            Return Info.ProgressWritePositionY
        End Get
    End Property

    ReadOnly Property ProgressReportWritePositionX As Integer Implements ISplash.ProgressReportWritePositionX
        Get
            Return Info.ProgressReportWritePositionX
        End Get
    End Property

    ReadOnly Property ProgressReportWritePositionY As Integer Implements ISplash.ProgressReportWritePositionY
        Get
            Return Info.ProgressReportWritePositionY
        End Get
    End Property

    'Actual logic
    Public Sub Opening() Implements ISplash.Opening
        Wdbg(DebugLevel.I, "Splash opening. Clearing console...")
        Console.Clear()
    End Sub

    Public Sub Display() Implements ISplash.Display
        Wdbg(DebugLevel.I, "Splash displaying.")

        'Display the text and percentage
        WriteWhere("{0}%", ProgressWritePositionX, ProgressWritePositionY, True, ColTypes.Progress, Progress.ToString.PadLeft(3))
        WriteWhere(ProgressText, ProgressReportWritePositionX, ProgressReportWritePositionY, False, ColTypes.Neutral)
        ClearLineToRight()

        'Display the progress bar
        WriteProgress(Progress, 4, Console.WindowHeight - 4)

        While Not SplashClosing
        End While
        Wdbg(DebugLevel.I, "Splash done.")
    End Sub

    Public Sub Closing() Implements ISplash.Closing
        SplashClosing = True
        Wdbg(DebugLevel.I, "Splash closing. Clearing console...")
        Console.Clear()
    End Sub

    Public Sub Report(Progress As Integer, ProgressReport As String, ProgressWritePositionX As Integer, ProgressWritePositionY As Integer, ProgressReportWritePositionX As Integer, ProgressReportWritePositionY As Integer, ParamArray Vars() As Object) Implements ISplash.Report
        WriteWhere("{0}%", ProgressWritePositionX, ProgressWritePositionY, True, ColTypes.Progress, Progress.ToString.PadLeft(3))
        WriteWhere(ProgressReport, ProgressReportWritePositionX, ProgressReportWritePositionY, False, ColTypes.Neutral, Vars)
        ClearLineToRight()

        'Display the progress bar
        WriteProgress(Progress, 4, Console.WindowHeight - 4)
    End Sub

End Class
