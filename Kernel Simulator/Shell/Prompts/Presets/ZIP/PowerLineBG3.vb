
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
Imports KS.Shell.Shells.Zip

Namespace Shell.Prompts.Presets.ZIP
    Public Class ZipPowerLineBG3Preset
        Inherits PromptPresetBase
        Implements IPromptPreset

        Public Overrides ReadOnly Property PresetName As String = "PowerLineBG3" Implements IPromptPreset.PresetName

        Public Overrides ReadOnly Property PresetPrompt As String Implements IPromptPreset.PresetPrompt
            Get
                Return PresetPromptBuilder()
            End Get
        End Property

        Public Overrides ReadOnly Property PresetShellType As ShellType = ShellType.ZIPShell Implements IPromptPreset.PresetShellType

        Friend Overrides Function PresetPromptBuilder() As String Implements IPromptPreset.PresetPromptBuilder
            'PowerLine glyphs
            Dim TransitionChar As Char = Convert.ToChar(&HE0B0)
            Dim TransitionPartChar As Char = Convert.ToChar(&HE0B1)

            'PowerLine preset colors
            Dim FirstColorSegmentForeground As New Color(255, 255, 85)
            Dim FirstColorSegmentBackground As New Color(25, 25, 25)
            Dim SecondColorSegmentForeground As New Color(255, 255, 85)
            Dim SecondColorSegmentBackground As New Color(25, 25, 25)
            Dim LastTransitionForeground As New Color(25, 25, 25)

            'Builder
            Dim PresetStringBuilder As New StringBuilder

            'File name
            PresetStringBuilder.Append(FirstColorSegmentForeground.VTSequenceForeground)
            PresetStringBuilder.Append(FirstColorSegmentBackground.VTSequenceBackground)
            PresetStringBuilder.AppendFormat(" {0} ", Path.GetFileName(ZipShell_FileStream.Name))

            'Transition
            PresetStringBuilder.Append(FirstColorSegmentForeground.VTSequenceForeground)
            PresetStringBuilder.Append(SecondColorSegmentBackground.VTSequenceBackground)
            PresetStringBuilder.AppendFormat("{0}", TransitionPartChar)

            'Current archive directory
            PresetStringBuilder.Append(SecondColorSegmentForeground.VTSequenceForeground)
            PresetStringBuilder.Append(SecondColorSegmentBackground.VTSequenceBackground)
            PresetStringBuilder.AppendFormat(" {0} ", ZipShell_CurrentArchiveDirectory)

            'Transition
            PresetStringBuilder.Append(LastTransitionForeground.VTSequenceForeground)
            PresetStringBuilder.Append(If(SetBackground, BackgroundColor.VTSequenceBackground, GetEsc() + $"[49m"))
            PresetStringBuilder.AppendFormat("{0} ", TransitionChar)

            'Present final string
            Return PresetStringBuilder.ToString()
        End Function

    End Class
End Namespace
