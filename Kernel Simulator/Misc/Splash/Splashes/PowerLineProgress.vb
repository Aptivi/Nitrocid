
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

Imports System.Text
Imports System.Threading

Namespace Misc.Splash.Splashes
    Class SplashPowerLineProgress
        Implements ISplash

        'Standalone splash information
        ReadOnly Property SplashName As String Implements ISplash.SplashName
            Get
                Return "PowerLineProgress"
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

        ReadOnly Property ProgressWritePositionY As Integer
            Get
                Select Case PowerLineProgressProgressTextLocation
                    Case TextLocation.Top
                        Return 1
                    Case TextLocation.Bottom
                        Return Console.WindowHeight - 6
                    Case Else
                        Return 1
                End Select
            End Get
        End Property

        Private ReadOnly FirstColorSegmentForeground As New Color(85, 255, 255)
        Private ReadOnly FirstColorSegmentBackground As New Color(43, 127, 127)
        Private ReadOnly SecondColorSegmentForeground As New Color(0, 0, 0)
        Private ReadOnly SecondColorSegmentBackground As New Color(85, 255, 255)
        Private ReadOnly LastTransitionForeground As New Color(85, 255, 255)
        Private ReadOnly TransitionChar As Char = Convert.ToChar(&HE0B0)
        Private ReadOnly RandomDriver As New Random()

        'Actual logic
        Public Sub Opening() Implements ISplash.Opening
            Wdbg(DebugLevel.I, "Splash opening. Clearing console...")
            Console.Clear()
        End Sub

        Public Sub Display() Implements ISplash.Display
            Try
                Wdbg(DebugLevel.I, "Splash displaying.")

                'Display the progress bar
                UpdateProgressReport(Progress, ProgressText)

                'Loop until closing
                While Not SplashClosing
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
            UpdateProgressReport(Progress, ProgressReport, Vars)
        End Sub

        ''' <summary>
        ''' Updates the splash progress
        ''' </summary>
        ''' <param name="Progress">Progress percentage from 0 to 100</param>
        ''' <param name="ProgressReport">The progress text</param>
        Sub UpdateProgressReport(Progress As Integer, ProgressReport As String, ParamArray Vars() As Object)
            'Variables
            Dim PresetStringBuilder As New StringBuilder
            Dim RenderedText As String = ProgressReport.Truncate(Console.WindowWidth - 5)

            'Percentage
            PresetStringBuilder.Append(FirstColorSegmentForeground.VTSequenceForeground)
            PresetStringBuilder.Append(FirstColorSegmentBackground.VTSequenceBackground)
            PresetStringBuilder.AppendFormat(" {0}% ", Progress.ToString.PadLeft(3))

            'Transition
            PresetStringBuilder.Append(FirstColorSegmentBackground.VTSequenceForeground)
            PresetStringBuilder.Append(SecondColorSegmentBackground.VTSequenceBackground)
            PresetStringBuilder.AppendFormat("{0}", TransitionChar)

            'Progress text
            PresetStringBuilder.Append(SecondColorSegmentForeground.VTSequenceForeground)
            PresetStringBuilder.Append(SecondColorSegmentBackground.VTSequenceBackground)
            PresetStringBuilder.AppendFormat(" {0} ", ProgressReport)

            'Transition
            PresetStringBuilder.Append(LastTransitionForeground.VTSequenceForeground)
            PresetStringBuilder.Append(BackgroundColor.VTSequenceBackground)
            PresetStringBuilder.AppendFormat("{0} ", TransitionChar)

            'Display the text and percentage
            WriteWhere(PresetStringBuilder.ToString(), 0, ProgressWritePositionY, False, ColTypes.Progress, Vars)
            ClearLineToRight()

            'Display the progress bar
            If Not String.IsNullOrEmpty(PowerLineProgressProgressColor) And TryParseColor(PowerLineProgressProgressColor) Then
                Dim ProgressColor As New Color(PowerLineProgressProgressColor)
                WriteProgress(Progress, 4, Console.WindowHeight - 4, ProgressColor)
            Else
                WriteProgress(Progress, 4, Console.WindowHeight - 4)
            End If
        End Sub

    End Class
End Namespace
