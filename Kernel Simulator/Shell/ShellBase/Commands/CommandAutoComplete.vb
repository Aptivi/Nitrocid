
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

Imports KS.Files.Folders
Imports ReadLineReboot

Namespace Shell.ShellBase
    Public Class CommandAutoComplete
        Implements IAutoCompleteHandler

        Private ReadOnly ShellTypeToAutocomplete As ShellType = ShellType.Shell

        Public Property Separators As Char() = {" "c} Implements IAutoCompleteHandler.Separators

        Public Function GetSuggestions(text As String, index As Integer) As String() Implements IAutoCompleteHandler.GetSuggestions
            Dim ShellCommands = GetCommands(ShellTypeToAutocomplete)
            If ShellStack.Count > 0 Then
                If String.IsNullOrEmpty(text) Then
                    Return ShellCommands.Keys.ToArray()
                Else
                    If text.Contains(" ") Then
                        'We're providing completion for argument.
                        Dim CommandName As String = text.SplitEncloseDoubleQuotes(" ")(0)
                        Dim FileFolderList As String() = CreateList(CurrentDir, True).Select(Function(x) x.Name).ToArray()
                        If ShellCommands.ContainsKey(CommandName) Then
                            'We have the command. Check its entry for argument info
                            Dim CommandArgumentInfo As CommandArgumentInfo = ShellCommands(CommandName).CommandArgumentInfo
                            If CommandArgumentInfo IsNot Nothing Then
                                'There are arguments! Now, check to see if it has the accessible auto completer
                                Dim AutoCompleter As Func(Of String()) = CommandArgumentInfo.AutoCompleter
                                If AutoCompleter IsNot Nothing Then
                                    'We have the delegate! Invoke it.
                                    Return AutoCompleter.Invoke()
                                Else
                                    'No delegate. Return file list
                                    Return FileFolderList
                                End If
                            Else
                                'No arguments. Return file list
                                Return FileFolderList
                            End If
                        End If
                        Return FileFolderList
                    Else
                        'We're providing completion for command.
                        Return ShellCommands.Keys.Where(Function(x) x.StartsWith(text)).ToArray()
                    End If
                End If
            Else
                Return Nothing
            End If
        End Function

        Protected Friend Sub New(Optional ShellType As ShellType = ShellType.Shell)
            ShellTypeToAutocomplete = ShellType
        End Sub

    End Class
End Namespace
