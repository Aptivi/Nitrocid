
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

Imports KS.Shell.Prompts
Imports KS.ConsoleBase.Inputs.Styles

Namespace Shell.UnifiedCommands
    Class PresetsUnifiedCommand
        Inherits CommandExecutor
        Implements ICommand

        Public Overrides Sub Execute(StringArgs As String, ListArgs() As String, ListArgsOnly As String(), ListSwitchesOnly As String()) Implements ICommand.Execute
            Dim ShellType As ShellType = ShellStack(ShellStack.Count - 1).ShellType
            Dim Presets As Dictionary(Of String, PromptPresetBase) = GetPresetsFromShell(ShellType)

            'Add the custom presets to the local dictionary
            For Each PresetName As String In GetCustomPresetsFromShell(ShellType).Keys
                Presets.Add(PresetName, Presets(PresetName))
            Next

            'Now, prompt the user
            Dim PresetNames As String() = Presets.Keys.ToArray
            Dim PresetDisplays As String() = Presets.Values.Select(Function(Preset) Preset.PresetPrompt).ToArray
            Dim SelectedPreset As String = PromptChoice(DoTranslation("Select preset for {0}:").FormatString(ShellType), String.Join("/", PresetNames), PresetDisplays, ChoiceOutputType.Modern, True)
            SetPreset(SelectedPreset, ShellType)
        End Sub

    End Class
End Namespace
