
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

Namespace Shell.Shells.Test.Commands
    Class Test_CheckStringCommand
        Inherits CommandExecutor
        Implements ICommand

        Public Overrides Sub Execute(StringArgs As String, ListArgs() As String, ListArgsOnly As String(), ListSwitchesOnly As String()) Implements ICommand.Execute
            Dim Text As String = ListArgsOnly(0)
            Dim LocalizedStrings As Dictionary(Of String, String) = PrepareDict("eng")
            If LocalizedStrings.ContainsKey(Text) Then
                Write(DoTranslation("String found in the localization resources."), True, ColTypes.Success)
            Else
                Write(DoTranslation("String not found in the localization resources."), True, ColTypes.Neutral)
            End If
        End Sub

    End Class
End Namespace
