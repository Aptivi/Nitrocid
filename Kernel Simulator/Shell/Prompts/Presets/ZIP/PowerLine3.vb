
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

Imports System.IO
Imports System.Text
Imports KS.Misc.ZipFile

Namespace Shell.Prompts.Presets.ZIP
    Public Class ZipPowerLine3Preset
        Inherits PromptPresetBase
        Implements IPromptPreset

        Public Overrides ReadOnly Property PresetName As String = "PowerLine3" Implements IPromptPreset.PresetName

        Public Overrides ReadOnly Property PresetPrompt As String Implements IPromptPreset.PresetPrompt
            Get
                Return PresetPromptBuilder()
            End Get
        End Property

        Public Overrides ReadOnly Property PresetShellType As ShellType = ShellType.ZIPShell Implements IPromptPreset.PresetShellType

        Friend Overrides Function PresetPromptBuilder() As String Implements IPromptPreset.PresetPromptBuilder
            'PowerLine glyphs
            Dim TransitionChar As Char = Convert.ToChar(&HE0B0)

            'PowerLine preset colors
            Dim UserNameShellColorSegmentForeground As New Color(255, 255, 85)
            Dim UserNameShellColorSegmentBackground As New Color(127, 127, 43)
            Dim HostNameShellColorSegmentForeground As New Color(0, 0, 0)
            Dim HostNameShellColorSegmentBackground As New Color(255, 255, 85)
            Dim CurrentDirectoryShellColorSegmentForeground As New Color(0, 0, 0)
            Dim CurrentDirectoryShellColorSegmentBackground As New Color(255, 255, 255)
            Dim LastTransitionForeground As New Color(255, 255, 255)

            'Builder
            Dim PresetStringBuilder As New StringBuilder

            'File name
            PresetStringBuilder.Append(UserNameShellColorSegmentForeground.VTSequenceForeground)
            PresetStringBuilder.Append(UserNameShellColorSegmentBackground.VTSequenceBackground)
            PresetStringBuilder.AppendFormat(" {0} ", Path.GetFileName(ZipShell_FileStream.Name))

            'Transition
            PresetStringBuilder.Append(UserNameShellColorSegmentBackground.VTSequenceForeground)
            PresetStringBuilder.Append(HostNameShellColorSegmentBackground.VTSequenceBackground)
            PresetStringBuilder.AppendFormat("{0}", TransitionChar)

            'Current archive directory
            PresetStringBuilder.Append(HostNameShellColorSegmentForeground.VTSequenceForeground)
            PresetStringBuilder.Append(HostNameShellColorSegmentBackground.VTSequenceBackground)
            PresetStringBuilder.AppendFormat(" {0} ", ZipShell_CurrentArchiveDirectory)

            'Transition
            PresetStringBuilder.Append(HostNameShellColorSegmentBackground.VTSequenceForeground)
            PresetStringBuilder.Append(BackgroundColor.VTSequenceBackground)
            PresetStringBuilder.AppendFormat("{0} ", TransitionChar)

            'Present final string
            Return PresetStringBuilder.ToString()
        End Function

    End Class
End Namespace
