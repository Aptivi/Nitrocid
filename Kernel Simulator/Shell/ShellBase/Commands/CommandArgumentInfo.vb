
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

Namespace Shell.ShellBase.Commands
    Public Class CommandArgumentInfo

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
        ''' Auto completion function delegate
        ''' </summary>
        Public ReadOnly Property AutoCompleter As Func(Of String())

        ''' <summary>
        ''' Installs a new instance of the command argument info class
        ''' </summary>
        ''' <param name="HelpUsages">Help usages</param>
        ''' <param name="ArgumentsRequired">Arguments required</param>
        ''' <param name="MinimumArguments">Minimum arguments</param>
        ''' <param name="AutoCompleter">Auto completion function</param>
        Public Sub New(HelpUsages() As String, ArgumentsRequired As Boolean, MinimumArguments As Integer, Optional AutoCompleter As Func(Of String()) = Nothing)
            Me.HelpUsages = HelpUsages
            Me.ArgumentsRequired = ArgumentsRequired
            Me.MinimumArguments = MinimumArguments
            Me.AutoCompleter = AutoCompleter
        End Sub

    End Class
End Namespace
