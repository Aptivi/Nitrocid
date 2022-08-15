
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

Imports KS.Scripting.Conditions

Namespace Shell.Shells.UESH.Commands
    Class IfCommand
        Inherits CommandExecutor
        Implements ICommand

        Public Overrides Sub Execute(StringArgs As String, ListArgs() As String, ListArgsOnly As String(), ListSwitchesOnly As String()) Implements ICommand.Execute
            Try
                If ConditionSatisfied(ListArgsOnly(0)) Then
                    Dim CommandString As String = String.Join(" ", ListArgsOnly.Skip(1).ToArray)
                    Dim AltThreads As List(Of KernelThread) = ShellStack(ShellStack.Count - 1).AltCommandThreads
                    If AltThreads.Count = 0 OrElse AltThreads(AltThreads.Count - 1).IsAlive Then
                        Dim CommandThread As New KernelThread($"Alternative Shell Command Thread", False, AddressOf ExecuteCommand)
                        ShellStack(ShellStack.Count - 1).AltCommandThreads.Add(CommandThread)
                    End If
                    GetLine(CommandString)
                End If
            Catch ex As Exception
                Wdbg(DebugLevel.E, "Failed to satisfy condition. See above for more information: {0}", ex.Message)
                WStkTrc(ex)
                Write(DoTranslation("Failed to satisfy condition. More info here:") + " {0}", True, ColTypes.Error, ex.Message)
            End Try
        End Sub

    End Class
End Namespace
