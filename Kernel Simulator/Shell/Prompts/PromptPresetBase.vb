
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

Imports KS.Shell.ShellBase.Shells

Namespace Shell.Prompts
    Public MustInherit Class PromptPresetBase
        Implements IPromptPreset

        Public Overridable ReadOnly Property PresetName As String = "BasePreset" Implements IPromptPreset.PresetName

        Public Overridable ReadOnly Property PresetPrompt As String = "> " Implements IPromptPreset.PresetPrompt

        Public Overridable ReadOnly Property PresetShellType As ShellType = ShellType.Shell Implements IPromptPreset.PresetShellType

        Friend Overridable Function PresetPromptBuilder() As String Implements IPromptPreset.PresetPromptBuilder
            Wdbg(DebugLevel.E, "Tried to call prompt builder on base.")
            Throw New NotImplementedException()
        End Function

    End Class
End Namespace
