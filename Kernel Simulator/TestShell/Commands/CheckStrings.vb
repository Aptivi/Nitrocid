
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

Namespace TestShell.Commands
    Class Test_CheckStringsCommand
        Inherits CommandExecutor
        Implements ICommand

        Public Overrides Sub Execute(StringArgs As String, ListArgs() As String, ListArgsOnly As String(), ListSwitchesOnly As String()) Implements ICommand.Execute
            Dim TextPath As String = ListArgsOnly(0)
            Dim LocalizedStrings As Dictionary(Of String, String) = PrepareDict("eng")
            Dim Texts() As String = ReadContents(TextPath)
            For Each Text As String In Texts
                If LocalizedStrings.ContainsKey(Text) Then
                    Write("[+] {0}", True, ColTypes.Success, Text)
                Else
                    Write("[-] {0}", True, ColTypes.Neutral, Text)
                End If
            Next
        End Sub

    End Class
End Namespace