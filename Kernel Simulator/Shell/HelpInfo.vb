
'    Kernel Simulator  Copyright (C) 2018-2021  EoflaOE
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

Public Class HelpInfo

    ''' <summary>
    ''' The untranslated help definition for command
    ''' </summary>
    Public ReadOnly Property UntranslatedHelpDefinition As String
    ''' <summary>
    ''' The translated help definition for command
    ''' </summary>
    Public ReadOnly Property TranslatedHelpDefinition As String
    ''' <summary>
    ''' The command usages
    ''' </summary>
    Public ReadOnly Property Usages As String()

    ''' <summary>
    ''' Makes a new class instance of help information for specific command
    ''' </summary>
    ''' <param name="HelpDefinition">The untranslated help definition for command</param>
    ''' <param name="Usages">The command usages</param>
    Friend Sub New(HelpDefinition As String, Usages As String())
        UntranslatedHelpDefinition = HelpDefinition
        TranslatedHelpDefinition = DoTranslation(HelpDefinition)
        Me.Usages = Usages
    End Sub

End Class
