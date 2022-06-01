
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

Imports KS.Arguments.ArgumentBase

Namespace Arguments.KernelArguments
    Class CmdInjectArgument
        Inherits ArgumentExecutor
        Implements IArgument

        Public Overrides Sub Execute(StringArgs As String, ListArgs() As String, ListArgsOnly As String(), ListSwitchesOnly As String()) Implements IArgument.Execute
            If ListArgs IsNot Nothing Then
                For Each InjectedCommand As String In ListArgsOnly
                    InjectedCommands.AddRange(InjectedCommand.Split({" : "}, StringSplitOptions.RemoveEmptyEntries))
                    CommandFlag = True
                Next
            Else
                Write(DoTranslation("Available commands: {0}"), True, ColTypes.Neutral, String.Join(", ", Shell.Shell.Commands.Keys))
                Write(">> ", False, ColTypes.Input)
                InjectedCommands.AddRange(ReadLine().Split({" : "}, StringSplitOptions.RemoveEmptyEntries))
                If String.Join(", ", InjectedCommands) <> "q" Then
                    CommandFlag = True
                Else
                    Write(DoTranslation("Command injection has been cancelled."), True, ColTypes.Neutral)
                End If
            End If
        End Sub

    End Class
End Namespace