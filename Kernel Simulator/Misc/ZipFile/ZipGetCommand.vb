
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

Imports System.IO
Imports System.IO.Compression
Imports System.Text
Imports System.Threading
Imports Microsoft.VisualBasic.FileIO

Public Module ZipGetCommand

    'Variables
    Public ZipShell_CommandThread As New Thread(AddressOf ZipShell_ParseCommand)

    Sub ZipShell_ParseCommand(ByVal CommandText As String)
        Try
            'Indicator if command finished
            Dim CommandDone As Boolean

            'Separate between command and arguments specified
            Dim Command As String = CommandText.Split(" ")(0)
            Dim Arguments() As String
            Dim TStream As New MemoryStream(Encoding.Default.GetBytes(CommandText))
            Dim Parser As New TextFieldParser(TStream) With {
                .Delimiters = {" "},
                .HasFieldsEnclosedInQuotes = True
            }
            Arguments = Parser.ReadFields
            If Arguments IsNot Nothing Then
                For i As Integer = 0 To Arguments.Length - 1
                    Arguments(i).Replace("""", "")
                Next
            End If

            'Remove first entry from array
            Dim ArgumentsList As List(Of String) = Arguments.ToList
            ArgumentsList.Remove(Command)
            Arguments = ArgumentsList.ToArray

            'Try to parse command
            If Command = "help" Then
                If Arguments?.Length > 0 Then
                    Wdbg("I", "Requested help for {0}", Arguments(0))
                    ZipShell_GetHelp(Arguments(0))
                Else
                    Wdbg("I", "Requested help for all commands")
                    ZipShell_GetHelp()
                End If
                CommandDone = True
            ElseIf Command = "exit" Then
                CommandDone = True
                ZipShell_Exiting = True
            ElseIf Command = "list" Then
                CommandDone = True
                Dim Entries As List(Of ZipArchiveEntry)
                If Arguments?.Length > 0 Then
                    Wdbg("I", "Listing entries with {0} as target directory", Arguments(0))
                    Entries = ListZipEntries(Arguments(0))
                Else
                    Wdbg("I", "Listing entries with current directory as target directory")
                    Entries = ListZipEntries(ZipShell_CurrentArchiveDirectory)
                End If
                For Each Entry As ZipArchiveEntry In Entries
                    W("- {0}: ", False, ColTypes.ListEntry, Entry.FullName) : W("{0} ({0})", True, ColTypes.ListValue, Entry.CompressedLength.FileSizeToString, Entry.Length.FileSizeToString)
                Next
            ElseIf Command = "get" Then
                If Arguments?.Length > 0 Then
                    CommandDone = True
                    Dim Where As String = ""
                    If Arguments?.Length > 1 Then
                        Where = NeutralizePath(Arguments(1))
                    End If
                    ExtractZipFileEntry(Arguments(0), Where)
                End If
            ElseIf Command = "chdir" Then
                If Arguments?.Length > 0 Then
                    CommandDone = True
                    If Not ChangeWorkingZipLocalDirectory(Arguments(0)) Then
                        W(DoTranslation("Directory {0} doesn't exist"), True, ColTypes.Err, Arguments(0))
                    End If
                End If
            ElseIf Command = "chadir" Then
                If Arguments?.Length > 0 Then
                    CommandDone = True
                    If Not ChangeWorkingArchiveDirectory(Arguments(0)) Then
                        W(DoTranslation("Archive directory {0} doesn't exist"), True, ColTypes.Err, Arguments(0))
                    End If
                End If
            End If

                'See if the command is done (passed all required arguments)
                If Not CommandDone Then
                W(DoTranslation("Required arguments are not passed to command {0}"), True, ColTypes.Err, Command)
                Wdbg("E", "Passed arguments were not enough to run command {0}. Arguments passed: {1}", Command, Arguments.Length)
                ZipShell_GetHelp(Command)
            End If
        Catch ex As Exception
            W(DoTranslation("Error trying to run command: {0}"), True, ColTypes.Err, ex.Message)
            Wdbg("E", "Error running command {0}: {1}", CommandText.Split(" ")(0), ex.Message)
            WStkTrc(ex)
            EventManager.RaiseTextCommandError(CommandText, ex)
        End Try
    End Sub

End Module
