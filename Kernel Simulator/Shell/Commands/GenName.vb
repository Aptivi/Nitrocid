
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

Imports KS.Misc

Namespace Shell.Commands
    Class GenNameCommand
        Inherits CommandExecutor
        Implements ICommand

        Public Overrides Sub Execute(StringArgs As String, ListArgs() As String, ListArgsOnly As String(), ListSwitchesOnly As String()) Implements ICommand.Execute
            Dim NamesCount As Integer = 10
            Dim NamePrefix As String = ""
            Dim NameSuffix As String = ""
            Dim SurnamePrefix As String = ""
            Dim SurnameSuffix As String = ""
            Dim NamesList As List(Of String)
            If ListArgsOnly.Length >= 1 Then NamesCount = Integer.Parse(ListArgsOnly(0))
            If ListArgsOnly.Length >= 2 Then NamePrefix = ListArgsOnly(1)
            If ListArgsOnly.Length >= 3 Then NameSuffix = ListArgsOnly(2)
            If ListArgsOnly.Length >= 4 Then SurnamePrefix = ListArgsOnly(3)
            If ListArgsOnly.Length >= 5 Then SurnameSuffix = ListArgsOnly(4)

            'Generate n names
            PopulateNames()
            NamesList = GenerateNames(NamesCount, NamePrefix, NameSuffix, SurnamePrefix, SurnameSuffix)
            WriteList(NamesList)
        End Sub

    End Class
End Namespace