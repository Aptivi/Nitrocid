
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

Imports KS.ManPages

Namespace Shell.Shells.UESH.Commands
    ''' <summary>
    ''' Opens the mod manual
    ''' </summary>
    ''' <remarks>
    ''' If the mod has a manual page which you can refer to, you can use them by this command.
    ''' <br></br>
    ''' <list type="table">
    ''' <listheader>
    ''' <term>Switches</term>
    ''' <description>Description</description>
    ''' </listheader>
    ''' <item>
    ''' <term>-list</term>
    ''' <description>Lists all installed mod manuals</description>
    ''' </item>
    ''' </list>
    ''' <br></br>
    ''' </remarks>
    Class ModManualCommand
        Inherits CommandExecutor
        Implements ICommand

        Public Overrides Sub Execute(StringArgs As String, ListArgs() As String, ListArgsOnly As String(), ListSwitchesOnly As String()) Implements ICommand.Execute
            Dim ListMode As Boolean
            If ListSwitchesOnly.Contains("-list") Then ListMode = True
            If Not ListMode Then
                ViewPage(ListArgsOnly(0))
            Else
                WriteList(Pages.Keys, True)
            End If
        End Sub

    End Class
End Namespace
