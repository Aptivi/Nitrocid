
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

Namespace Shell.ShellBase.Commands
    Public Class CommandInfo

        ''' <summary>
        ''' The command
        ''' </summary>
        Public ReadOnly Property Command As String
        ''' <summary>
        ''' The type of command
        ''' </summary>
        Public ReadOnly Property Type As ShellType
        ''' <summary>
        ''' The untranslated help definition of command. Translated by <see cref="GetTranslatedHelpEntry()"/>
        ''' </summary>
        Public Property HelpDefinition As String
        ''' <summary>
        ''' Command argument info
        ''' </summary>
        Public ReadOnly Property CommandArgumentInfo As CommandArgumentInfo
        ''' <summary>
        ''' Command base for execution
        ''' </summary>
        Public ReadOnly Property CommandBase As CommandExecutor
        ''' <summary>
        ''' Command properties
        ''' </summary>
        Public ReadOnly Property Flags As CommandFlags

        ''' <summary>
        ''' Installs a new instance of command info class
        ''' </summary>
        ''' <param name="Command">Command</param>
        ''' <param name="Type">Shell command type</param>
        ''' <param name="HelpDefinition">Command help definition</param>
        ''' <param name="CommandArgumentInfo">Command argument info</param>
        ''' <param name="CommandBase">Command base for execution</param>
        ''' <param name="Flags">Command flags</param>
        Public Sub New(Command As String, Type As ShellType, HelpDefinition As String, CommandArgumentInfo As CommandArgumentInfo, CommandBase As CommandExecutor, Optional Flags As CommandFlags = CommandFlags.None)
            Me.Command = Command
            Me.Type = Type
            Me.HelpDefinition = HelpDefinition
            Me.CommandArgumentInfo = CommandArgumentInfo
            Me.CommandBase = CommandBase
            Me.Flags = Flags
        End Sub

        ''' <summary>
        ''' Gets the translated version of help entry (KS built-in commands only)
        ''' </summary>
        Public Function GetTranslatedHelpEntry() As String
            Return DoTranslation(HelpDefinition)
        End Function

    End Class
End Namespace
