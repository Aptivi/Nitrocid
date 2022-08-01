
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
Imports KS.Files.Folders

Namespace Shell.Prompts.Presets.UESH
    Public Class PowerLineBG1Preset
        Inherits PromptPresetBase
        Implements IPromptPreset

        Public Overrides ReadOnly Property PresetName As String = "PowerLineBG1" Implements IPromptPreset.PresetName

        Public Overrides ReadOnly Property PresetPrompt As String Implements IPromptPreset.PresetPrompt
            Get
                Return PresetPromptBuilder()
            End Get
        End Property

        Friend Overrides Function PresetPromptBuilder() As String Implements IPromptPreset.PresetPromptBuilder
            'PowerLine glyphs
            Dim TransitionChar As Char = Convert.ToChar(&HE0B0)
            Dim TransitionPartChar As Char = Convert.ToChar(&HE0B1)
            Dim PadlockChar As Char = Convert.ToChar(&HE0A2)

            'PowerLine preset colors
            Dim UserNameShellColorSegmentForeground As New Color(85, 255, 255)
            Dim UserNameShellColorSegmentBackground As New Color(25, 25, 25)
            Dim HostNameShellColorSegmentForeground As New Color(85, 255, 255)
            Dim HostNameShellColorSegmentBackground As New Color(25, 25, 25)
            Dim CurrentDirectoryShellColorSegmentForeground As New Color(85, 255, 255)
            Dim CurrentDirectoryShellColorSegmentBackground As New Color(25, 25, 25)
            Dim LastTransitionForeground As New Color(25, 25, 25)

            'Builder
            Dim PresetStringBuilder As New StringBuilder

            'Build the preset
            If Not Maintenance Then
                'Current username
                PresetStringBuilder.Append(UserNameShellColorSegmentForeground.VTSequenceForeground)
                PresetStringBuilder.Append(UserNameShellColorSegmentBackground.VTSequenceBackground)
                PresetStringBuilder.AppendFormat(" {0} ", CurrentUser.Username)

                'Transition
                PresetStringBuilder.Append(UserNameShellColorSegmentForeground.VTSequenceForeground)
                PresetStringBuilder.Append(HostNameShellColorSegmentBackground.VTSequenceBackground)
                PresetStringBuilder.AppendFormat("{0}", TransitionPartChar)

                'Current hostname
                PresetStringBuilder.Append(HostNameShellColorSegmentForeground.VTSequenceForeground)
                PresetStringBuilder.Append(HostNameShellColorSegmentBackground.VTSequenceBackground)
                PresetStringBuilder.AppendFormat(" {0} {1} ", PadlockChar, HostName)

                'Transition
                PresetStringBuilder.Append(HostNameShellColorSegmentForeground.VTSequenceForeground)
                PresetStringBuilder.Append(CurrentDirectoryShellColorSegmentBackground.VTSequenceBackground)
                PresetStringBuilder.AppendFormat("{0}", TransitionPartChar)

                'Current directory
                PresetStringBuilder.Append(CurrentDirectoryShellColorSegmentForeground.VTSequenceForeground)
                PresetStringBuilder.Append(CurrentDirectoryShellColorSegmentBackground.VTSequenceBackground)
                PresetStringBuilder.AppendFormat(" {0} ", CurrentDir)

                'Transition
                PresetStringBuilder.Append(LastTransitionForeground.VTSequenceForeground)
                PresetStringBuilder.Append(BackgroundColor.VTSequenceBackground)
                PresetStringBuilder.AppendFormat("{0} ", TransitionChar)
            Else
                'Maintenance mode
                PresetStringBuilder.Append(GetGray().VTSequenceForeground)
                PresetStringBuilder.Append(DoTranslation("Maintenance Mode") + "> ")
            End If

            'Present final string
            Return PresetStringBuilder.ToString()
        End Function

    End Class
End Namespace
