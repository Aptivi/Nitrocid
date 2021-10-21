
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
Imports System.Text

Class SumFilesCommand
    Inherits CommandExecutor
    Implements ICommand

    Public Overrides Sub Execute(StringArgs As String, ListArgs() As String, ListArgsOnly As String(), ListSwitchesOnly As String()) Implements ICommand.Execute
        Dim folder As String = NeutralizePath(ListArgs(1))
        Dim out As String = ""
        Dim FileBuilder As New StringBuilder
        If Not ListArgs.Length < 3 Then
            out = NeutralizePath(ListArgs(2))
        End If
        If FolderExists(folder) Then
            For Each file As String In Directory.EnumerateFiles(folder, "*", SearchOption.TopDirectoryOnly)
                file = NeutralizePath(file)
                WriteSeparator(file, True)
                Dim AlgorithmEnum As Algorithms
                If ListArgs(0) = "all" Then
                    For Each Algorithm As String In [Enum].GetNames(GetType(Algorithms))
                        AlgorithmEnum = [Enum].Parse(GetType(Algorithms), Algorithm)
                        Dim spent As New Stopwatch
                        spent.Start() 'Time when you're on a breakpoint is counted
                        Dim encrypted As String = GetEncryptedFile(file, AlgorithmEnum)
                        W("{0} ({1})", True, ColTypes.Neutral, encrypted, AlgorithmEnum)
                        W(DoTranslation("Time spent: {0} milliseconds"), True, ColTypes.Neutral, spent.ElapsedMilliseconds)
                        FileBuilder.AppendLine($"- {file}: {encrypted} ({AlgorithmEnum})")
                        spent.Stop()
                    Next
                ElseIf [Enum].TryParse(ListArgs(0), AlgorithmEnum) Then
                    Dim spent As New Stopwatch
                    spent.Start() 'Time when you're on a breakpoint is counted
                    Dim encrypted As String = GetEncryptedFile(file, AlgorithmEnum)
                    W(encrypted, True, ColTypes.Neutral)
                    W(DoTranslation("Time spent: {0} milliseconds"), True, ColTypes.Neutral, spent.ElapsedMilliseconds)
                    FileBuilder.AppendLine($"- {file}: {encrypted} ({AlgorithmEnum})")
                    spent.Stop()
                Else
                    W(DoTranslation("Invalid encryption algorithm."), True, ColTypes.Error)
                    Exit For
                End If
                Console.WriteLine()
            Next
            If Not out = "" Then
                Dim FStream As New StreamWriter(out)
                FStream.Write(FileBuilder.ToString)
                FStream.Flush()
            End If
        Else
            W(DoTranslation("{0} is not found."), True, ColTypes.Error, folder)
        End If
    End Sub

End Class