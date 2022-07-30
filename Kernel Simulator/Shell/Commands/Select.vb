
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

Imports KS.Scripting.Interaction

Namespace Shell.Commands
    Class SelectCommand
        Inherits CommandExecutor
        Implements ICommand

        Public Overrides Sub Execute(StringArgs As String, ListArgs() As String, ListArgsOnly As String(), ListSwitchesOnly As String()) Implements ICommand.Execute
            Dim Titles As New List(Of String)

            'Add the provided working titles
            If ListArgsOnly.Length > 3 Then
                Titles.AddRange(ListArgsOnly.Skip(3))
            End If

            'Prompt for selection
            PromptSelectionAndSet(ListArgsOnly(2), ListArgsOnly(0), ListArgsOnly(1), Titles.ToArray)
        End Sub

        Public Overrides Sub HelpHelper()
            Write(DoTranslation("where <$variable> is any variable that will be used to store response") + NewLine +
                  DoTranslation("where <answers> are one-lettered answers of the question separated in slashes"), True, ColTypes.Neutral)
        End Sub

    End Class
End Namespace