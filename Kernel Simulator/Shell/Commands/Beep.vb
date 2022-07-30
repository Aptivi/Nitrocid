
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

Imports KS.Misc.Reflection

Namespace Shell.Commands
    Class BeepCommand
        Inherits CommandExecutor
        Implements ICommand

        <CodeAnalysis.SuppressMessage("Interoperability", "CA1416:Validate platform compatibility", Justification:="There is already a platform check in the command logic.")>
        Public Overrides Sub Execute(StringArgs As String, ListArgs() As String, ListArgsOnly As String(), ListSwitchesOnly As String()) Implements ICommand.Execute
            If IsStringNumeric(ListArgs(0)) And CInt(ListArgs(0)) >= 37 And CInt(ListArgs(0)) <= 32767 Then
                If IsStringNumeric(ListArgs(1)) Then
                    If IsOnWindows() Then
                        Console.Beep(ListArgs(0), ListArgs(1))
                    Else
                        Console.Beep()
                    End If
                Else
                    Write(DoTranslation("Time must be numeric."), True, ColTypes.Error)
                End If
            Else
                Write(DoTranslation("Frequency must be numeric. If it's numeric, ensure that it is >= 37 and <= 32767."), True, ColTypes.Error)
            End If
        End Sub

    End Class
End Namespace