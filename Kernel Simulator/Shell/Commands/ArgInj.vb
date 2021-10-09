
'    Kernel Simulator  Copyright (C) 2018-2021  EoflaOE
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

Class ArgInjCommand
    Inherits CommandExecutor
    Implements ICommand

    Public Overrides Sub Execute(StringArgs As String, ListArgs() As String, ListArgsOnly As String(), ListSwitchesOnly As String()) Implements ICommand.Execute
        Dim FinalArgs As New List(Of String)
        For Each arg As String In ListArgs
            Wdbg(DebugLevel.I, "Parsing argument {0}...", arg)
            If AvailableArgs.Contains(arg) Then
                Wdbg(DebugLevel.I, "Adding argument {0}...", arg)
                FinalArgs.Add(arg)
            Else
                Wdbg(DebugLevel.W, "Argument {0} not found.", arg)
                W(DoTranslation("Argument {0} not found to inject."), True, ColTypes.Warning, arg)
            End If
        Next
        If FinalArgs.Count = 0 Then
            W(DoTranslation("No arguments specified. Hint: Specify multiple arguments separated by spaces"), True, ColTypes.Error)
        Else
            EnteredArguments = New List(Of String)(FinalArgs)
            ArgsInjected = True
            W(DoTranslation("Injected arguments, {0}, will be scheduled to run at next reboot."), True, ColTypes.Neutral, String.Join(", ", EnteredArguments))
        End If
    End Sub

End Class