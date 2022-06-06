
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

Namespace Modifications
    ''' <summary>
    ''' Interface for mods
    ''' </summary>
    Public Interface IScript
        ''' <summary>
        ''' List of commands for mod
        ''' </summary>
        Property Commands As Dictionary(Of String, CommandInfo)
        ''' <summary>
        ''' Mod name
        ''' </summary>
        Property Name As String
        ''' <summary>
        ''' Name of part of mod
        ''' </summary>
        Property ModPart As String
        ''' <summary>
        ''' Mod version
        ''' </summary>
        Property Version As String
        ''' <summary>
        ''' Code executed when starting mod
        ''' </summary>
        Sub StartMod()
        ''' <summary>
        ''' Code executed when stopping mod
        ''' </summary>
        Sub StopMod()
        ''' <summary>
        ''' Code executed when performing command
        ''' </summary>
        ''' <param name="Command">A command.</param>
        ''' <param name="args">Arguments. Make sure to split your arguments if necessary.</param>
        <Obsolete("Investigate the usage of CommandInfo.CommandBase.Execute instead.")>
        Sub PerformCmd(Command As CommandInfo, Optional Args As String = "")
        ''' <summary>
        ''' Code executed when initializing events
        ''' </summary>
        ''' <param name="ev">Event name. Look it up on <see cref="Events"/></param>
        Sub InitEvents(ev As String)
        ''' <summary>
        ''' Code executed when initializing events
        ''' </summary>
        ''' <param name="ev">Event name. Look it up on <see cref="Events"/></param>
        ''' <param name="Args">Arguments.</param>
        Sub InitEvents(ev As String, ParamArray Args() As Object)
    End Interface
End Namespace
