
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

Imports KS.Shell.ShellBase.Shells

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
        ''' The help usages of command.
        ''' </summary>
        Public ReadOnly Property HelpUsages As String()
        ''' <summary>
        ''' Does the command require arguments?
        ''' </summary>
        Public ReadOnly Property ArgumentsRequired As Boolean
        ''' <summary>
        ''' User must specify at least this number of arguments
        ''' </summary>
        Public ReadOnly Property MinimumArguments As Integer
        ''' <summary>
        ''' Command base for execution
        ''' </summary>
        Public ReadOnly Property CommandBase As CommandExecutor
        ''' <summary>
        ''' Is the command admin-only?
        ''' </summary>
        Public ReadOnly Property Strict As Boolean
        ''' <summary>
        ''' Is the command wrappable?
        ''' </summary>
        Public ReadOnly Property Wrappable As Boolean
        ''' <summary>
        ''' If true, the command can't be run in maintenance mode
        ''' </summary>
        Public ReadOnly Property NoMaintenance As Boolean
        ''' <summary>
        ''' Is the command obsolete?
        ''' </summary>
        Public ReadOnly Property Obsolete As Boolean
        ''' <summary>
        ''' Does the command set a UESH $variable?
        ''' </summary>
        Public ReadOnly Property SettingVariable As Boolean

        ''' <summary>
        ''' Installs a new instance of command info class
        ''' </summary>
        ''' <param name="Command">Command</param>
        ''' <param name="Type">Shell command type</param>
        ''' <param name="HelpDefinition">Command help definition</param>
        ''' <param name="HelpUsages">Command help usages</param>
        ''' <param name="ArgumentsRequired">Does the command require arguments?</param>
        ''' <param name="MinimumArguments">User must specify at least this number of arguments</param>
        ''' <param name="CommandBase">Command base for execution</param>
        ''' <param name="Strict">Is the command admin-only?</param>
        ''' <param name="Wrappable">Is the command wrappable?</param>
        ''' <param name="NoMaintenance">If true, the command can't be run in maintenance mode</param>
        ''' <param name="Obsolete">Is the command obsolete?</param>
        ''' <param name="SettingVariable">Does the command set a UESH $variable?</param>
        Public Sub New(Command As String, Type As ShellType, HelpDefinition As String, HelpUsages As String(), ArgumentsRequired As Boolean, MinimumArguments As Integer, CommandBase As CommandExecutor, Optional Strict As Boolean = False, Optional Wrappable As Boolean = False, Optional NoMaintenance As Boolean = False, Optional Obsolete As Boolean = False, Optional SettingVariable As Boolean = False)
            Me.Command = Command
            Me.Type = Type
            Me.HelpDefinition = HelpDefinition
            Me.HelpUsages = HelpUsages
            Me.ArgumentsRequired = ArgumentsRequired
            Me.MinimumArguments = MinimumArguments
            Me.CommandBase = CommandBase
            Me.Strict = Strict
            Me.Wrappable = Wrappable
            Me.NoMaintenance = NoMaintenance
            Me.Obsolete = Obsolete
            Me.SettingVariable = SettingVariable
        End Sub

        ''' <summary>
        ''' Gets the translated version of help entry (KS built-in commands only)
        ''' </summary>
        Public Function GetTranslatedHelpEntry() As String
            Return DoTranslation(HelpDefinition)
        End Function

    End Class
End Namespace