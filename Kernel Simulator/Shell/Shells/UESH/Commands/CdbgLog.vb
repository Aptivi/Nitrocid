
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

Imports System.IO

Namespace Shell.Shells.UESH.Commands
    ''' <summary>
    ''' Clears debugging log
    ''' </summary>
    ''' <remarks>
    ''' The user must have at least the administrative privileges before they can run the below commands.
    ''' </remarks>
    Class CdbgLogCommand
        Inherits CommandExecutor
        Implements ICommand

        Public Overrides Sub Execute(StringArgs As String, ListArgs() As String, ListArgsOnly As String(), ListSwitchesOnly As String()) Implements ICommand.Execute
            If DebugMode Then
                Try
                    DebugStreamWriter.Close()
                    DebugStreamWriter = New StreamWriter(GetKernelPath(KernelPathType.Debugging)) With {.AutoFlush = True}
                    Write(DoTranslation("Debug log removed. All connected debugging devices may still view messages."), True, ColTypes.Neutral)
                Catch ex As Exception
                    Write(DoTranslation("Debug log removal failed: {0}"), True, ColTypes.Error, ex.Message)
                    WStkTrc(ex)
                End Try
            Else
                Write(DoTranslation("You must turn on debug mode before you can clear debug log."), True, ColTypes.Neutral)
            End If
        End Sub

    End Class
End Namespace
