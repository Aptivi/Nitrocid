
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

Namespace Shell.Shells.HTTP.Commands
    ''' <summary>
    ''' Sets a URL for the current shell instance
    ''' </summary>
    ''' <remarks>
    ''' If you want to be able to execute any HTTP command, you must issue this command first. It sets the target URL that you want to send commands to.
    ''' <br></br>
    ''' You must set the target URL to the one that supports the commands.
    ''' </remarks>
    Class HTTP_SetSiteCommand
        Inherits CommandExecutor
        Implements ICommand

        Public Overrides Sub Execute(StringArgs As String, ListArgsOnly As String(), ListSwitchesOnly As String()) Implements ICommand.Execute
            Try
                Dim SiteUri As New Uri(StringArgs)
                Write(DoTranslation("Setting site to") + " {0}...", True, ColTypes.Progress, SiteUri.ToString)
                HTTPSite = SiteUri.ToString
            Catch ex As Exception
                Write(DoTranslation("The site URI format is invalid."), True, ColTypes.Error)
            End Try
        End Sub

    End Class
End Namespace
