
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

Imports System.Text
Imports KS.Files.Folders

Namespace Shell.Prompts.Presets
    Public Class DefaultPreset
        Inherits PromptPresetBase
        Implements IPromptPreset

        Public Overrides ReadOnly Property PresetName As String = "Default" Implements IPromptPreset.PresetName

        Public Overrides ReadOnly Property PresetPrompt As String Implements IPromptPreset.PresetPrompt
            Get
                Return PresetPromptBuilder()
            End Get
        End Property

        Friend Overrides Function PresetPromptBuilder() As String Implements IPromptPreset.PresetPromptBuilder
            Dim PresetStringBuilder As New StringBuilder
            Dim UserDollarSign As Char = If(HasPermission(CurrentUser.Username, PermissionType.Administrator), "#"c, "$"c)

            'Build the preset
            If Not Maintenance Then
                'Opening
                PresetStringBuilder.Append(GetGray().VTSequenceForeground)
                PresetStringBuilder.Append("[")

                'Current username
                PresetStringBuilder.Append(UserNameShellColor.VTSequenceForeground)
                PresetStringBuilder.AppendFormat("{0}", CurrentUser.Username)

                '"At" sign
                PresetStringBuilder.Append(GetGray().VTSequenceForeground)
                PresetStringBuilder.Append("@")

                'Current hostname
                PresetStringBuilder.Append(HostNameShellColor.VTSequenceForeground)
                PresetStringBuilder.AppendFormat("{0}", HostName)

                'Current directory
                PresetStringBuilder.Append(GetGray().VTSequenceForeground)
                PresetStringBuilder.AppendFormat("]{0}", CurrentDir)

                'User dollar sign
                PresetStringBuilder.Append(UserDollarColor.VTSequenceForeground)
                PresetStringBuilder.AppendFormat(" {0} ", UserDollarSign)
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
