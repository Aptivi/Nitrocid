
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

Imports KS.Misc.Editors.JsonShell

Namespace Shell.Shells.Json.Commands
    ''' <summary>
    ''' Saves changes to a JSON file
    ''' </summary>
    ''' <remarks>
    ''' If you're done with the JSON file, you can save it to the current JSON file. You can optionally beautify or minify the JSON file using the below switches:
    ''' <br></br>
    ''' <list type="table">
    ''' <listheader>
    ''' <term>Switches</term>
    ''' <description>Description</description>
    ''' </listheader>
    ''' <item>
    ''' <term>-b</term>
    ''' <description>Beautifies the JSON file while saving</description>
    ''' </item>
    ''' <item>
    ''' <term>-m</term>
    ''' <description>Minifies the JSON file while saving</description>
    ''' </item>
    ''' </list>
    ''' <br></br>
    ''' </remarks>
    Class JsonShell_SaveCommand
        Inherits CommandExecutor
        Implements ICommand

        Public Overrides Sub Execute(StringArgs As String, ListArgs() As String, ListArgsOnly As String(), ListSwitchesOnly As String()) Implements ICommand.Execute
            Dim TargetFormatting As Formatting = Formatting.Indented
            If ListSwitchesOnly.Length > 0 Then
                If ListSwitchesOnly(0) = "-b" Then TargetFormatting = Formatting.Indented
                If ListSwitchesOnly(0) = "-m" Then TargetFormatting = Formatting.None
            End If
            JsonShell_SaveFile(False, TargetFormatting)
        End Sub

        Public Overrides Sub HelpHelper()
            Write(DoTranslation("This command has the below switches that change how it works:"), True, ColTypes.Neutral)
            Write("  -b: ", False, ColTypes.ListEntry) : Write(DoTranslation("Saves the JSON file, beautifying it in the process"), True, ColTypes.ListValue)
            Write("  -m: ", False, ColTypes.ListEntry) : Write(DoTranslation("Saves the JSON file, minifying it in the process"), True, ColTypes.ListValue)
        End Sub

    End Class
End Namespace
