
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
Imports KS.Misc.Text
Imports KS.Shell.Shells.HTTP

Namespace Shell.Prompts.Presets.HTTP
    Public Class HTTPPowerLine1Preset
        Inherits PromptPresetBase
        Implements IPromptPreset

        Public Overrides ReadOnly Property PresetName As String = "PowerLine1" Implements IPromptPreset.PresetName

        Public Overrides ReadOnly Property PresetPrompt As String Implements IPromptPreset.PresetPrompt
            Get
                Return PresetPromptBuilder()
            End Get
        End Property

        Public Overrides ReadOnly Property PresetShellType As ShellType = ShellType.HTTPShell Implements IPromptPreset.PresetShellType

        Friend Overrides Function PresetPromptBuilder() As String Implements IPromptPreset.PresetPromptBuilder
            'PowerLine glyphs
            Dim TransitionChar As Char = Convert.ToChar(&HE0B0)
            Dim PadlockChar As Char = Convert.ToChar(&HE0A2)

            'PowerLine preset colors
            Dim FirstColorSegmentForeground As New Color(85, 255, 255)
            Dim FirstColorSegmentBackground As New Color(43, 127, 127)
            Dim LastTransitionForeground As New Color(43, 127, 127)

            'Builder
            Dim PresetStringBuilder As New StringBuilder

            'Build the preset
            If HTTPConnected Then
                'Current username
                PresetStringBuilder.Append(FirstColorSegmentForeground.VTSequenceForeground)
                PresetStringBuilder.Append(FirstColorSegmentBackground.VTSequenceBackground)
                PresetStringBuilder.AppendFormat(" {0} {1} ", PadlockChar, HTTPSite)

                'Transition
                PresetStringBuilder.Append(LastTransitionForeground.VTSequenceForeground)
                PresetStringBuilder.Append(If(SetBackground, BackgroundColor.VTSequenceBackground, GetEsc() + $"[49m"))
                PresetStringBuilder.AppendFormat("{0} ", TransitionChar)
            Else
                'HTTP current directory
                PresetStringBuilder.Append(FirstColorSegmentForeground.VTSequenceForeground)
                PresetStringBuilder.Append(FirstColorSegmentBackground.VTSequenceBackground)
                PresetStringBuilder.AppendFormat(" Not connected ")

                'Transition
                PresetStringBuilder.Append(LastTransitionForeground.VTSequenceForeground)
                PresetStringBuilder.Append(If(SetBackground, BackgroundColor.VTSequenceBackground, GetEsc() + $"[49m"))
                PresetStringBuilder.AppendFormat("{0} ", TransitionChar)
            End If

            'Present final string
            Return PresetStringBuilder.ToString()
        End Function

    End Class
End Namespace
