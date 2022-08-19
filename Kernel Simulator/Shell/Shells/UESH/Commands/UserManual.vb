
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

Namespace Shell.Shells.UESH.Commands
    ''' <summary>
    ''' Opens the web browser to this wiki or to the KS API for mods.
    ''' </summary>
    ''' <remarks>
    ''' <list type="table">
    ''' <listheader>
    ''' <term>Switches</term>
    ''' <description>Description</description>
    ''' </listheader>
    ''' <item>
    ''' <term>-createdir</term>
    ''' <description>Extracts the archive to the new directory that has the same name as the archive</description>
    ''' </item>
    ''' </list>
    ''' <br></br>
    ''' </remarks>
    Class UserManualCommand
        Inherits CommandExecutor
        Implements ICommand

        Public Overrides Sub Execute(StringArgs As String, ListArgsOnly As String(), ListSwitchesOnly As String()) Implements ICommand.Execute
            Dim ModDocumentation As Boolean = ListSwitchesOnly.Contains("-modapi")
            If ModDocumentation Then
                Process.Start("https://aptivi.github.io/Kernel-Simulator")
            Else
                Process.Start("https://github.com/Aptivi/Kernel-Simulator/wiki")
            End If
        End Sub

        Public Overrides Sub HelpHelper()
            Write(DoTranslation("This command has the below switches that change how it works:"), True, ColTypes.Neutral)
            Write("  -modapi: ", False, ColTypes.ListEntry) : Write(DoTranslation("Opens the mod API documentation for the structure of the source code in its most current form"), True, ColTypes.ListValue)
        End Sub

    End Class
End Namespace
