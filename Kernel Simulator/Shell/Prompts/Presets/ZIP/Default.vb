
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
    Public Class ZipDefaultPreset
        Inherits PromptPresetBase
        Implements IPromptPreset

        Public Overrides ReadOnly Property PresetName As String = "Default" Implements IPromptPreset.PresetName

        Public Overrides ReadOnly Property PresetPrompt As String Implements IPromptPreset.PresetPrompt
            Get
                Return PresetPromptBuilder()
            End Get
        End Property

        Public Overrides ReadOnly Property PresetShellType As ShellType = ShellType.ZIPShell Implements IPromptPreset.PresetShellType

        Friend Overrides Function PresetPromptBuilder() As String Implements IPromptPreset.PresetPromptBuilder
            'Build the preset
            Dim PresetStringBuilder As New StringBuilder

            'Opening
            PresetStringBuilder.Append(GetGray().VTSequenceForeground)
            PresetStringBuilder.Append("[")

            'File name
            PresetStringBuilder.Append(UserNameShellColor.VTSequenceForeground)
            PresetStringBuilder.AppendFormat(Path.GetFileName(ZipShell_FileStream.Name))

            'Current archive directory
            PresetStringBuilder.Append(UserNameShellColor.VTSequenceForeground)
            PresetStringBuilder.AppendFormat("{0}", ZipShell_CurrentArchiveDirectory)

            'Closing
            PresetStringBuilder.Append(GetGray().VTSequenceForeground)
            PresetStringBuilder.Append("] > ")

            'Present final string
            Return PresetStringBuilder.ToString()
        End Function

    End Class
End Namespace
