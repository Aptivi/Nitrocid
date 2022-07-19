
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

Imports System.IO
Imports KS.Files.Querying
Imports KS.Misc.Beautifiers

Namespace Shell.Commands
    Class JsonMinifyCommand
        Inherits CommandExecutor
        Implements ICommand

        Public Overrides Sub Execute(StringArgs As String, ListArgs() As String, ListArgsOnly As String(), ListSwitchesOnly As String()) Implements ICommand.Execute
            Dim JsonFile As String = NeutralizePath(ListArgs(0))
            Dim JsonOutputFile As String
            Dim MinifiedJson As String

            If FileExists(JsonFile) Then
                'Minify the JSON and display it on screen
                MinifiedJson = MinifyJson(JsonFile)
                Write(MinifiedJson, True, ColTypes.Neutral)

                'Minify it to an output file specified (optional)
                If ListArgs.Count > 1 Then
                    JsonOutputFile = NeutralizePath(ListArgs(1))
                    File.WriteAllText(JsonOutputFile, MinifiedJson)
                End If
            Else
                Write(DoTranslation("File {0} not found."), True, ColTypes.Error, JsonFile)
            End If
        End Sub

    End Class
End Namespace