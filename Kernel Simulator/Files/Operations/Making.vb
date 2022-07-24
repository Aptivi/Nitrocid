
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

Imports KS.Files.Querying
Imports System.IO
Imports Newtonsoft.Json.Linq

Namespace Files.Operations
    Public Module Making

        ''' <summary>
        ''' Makes a directory
        ''' </summary>
        ''' <param name="NewDirectory">New directory</param>
        ''' <exception cref="IOException"></exception>
        Public Sub MakeDirectory(NewDirectory As String, Optional ThrowIfDirectoryExists As Boolean = True)
            ThrowOnInvalidPath(NewDirectory)
            NewDirectory = NeutralizePath(NewDirectory)
            Wdbg(DebugLevel.I, "New directory: {0} ({1})", NewDirectory, FolderExists(NewDirectory))
            If Not FolderExists(NewDirectory) Then
                Directory.CreateDirectory(NewDirectory)

                'Raise event
                KernelEventManager.RaiseDirectoryCreated(NewDirectory)
            ElseIf ThrowIfDirectoryExists Then
                Throw New IOException(DoTranslation("Directory {0} already exists.").FormatString(NewDirectory))
            End If
        End Sub

        ''' <summary>
        ''' Makes a directory
        ''' </summary>
        ''' <param name="NewDirectory">New directory</param>
        ''' <returns>True if successful; False if unsuccessful</returns>
        ''' <exception cref="IOException"></exception>
        Public Function TryMakeDirectory(NewDirectory As String, Optional ThrowIfDirectoryExists As Boolean = True) As Boolean
            Try
                MakeDirectory(NewDirectory, ThrowIfDirectoryExists)
                Return True
            Catch ex As Exception
                Return False
            End Try
        End Function

        ''' <summary>
        ''' Makes a file
        ''' </summary>
        ''' <param name="NewFile">New file</param>
        ''' <exception cref="IOException"></exception>
        Public Sub MakeFile(NewFile As String, Optional ThrowIfFileExists As Boolean = True)
            ThrowOnInvalidPath(NewFile)
            NewFile = NeutralizePath(NewFile)
            Wdbg(DebugLevel.I, "File path is {0} and .Exists is {0}", NewFile, FileExists(NewFile))
            If Not FileExists(NewFile) Then
                Try
                    Dim NewFileStream As FileStream = File.Create(NewFile)
                    Wdbg(DebugLevel.I, "File created")
                    NewFileStream.Close()
                    Wdbg(DebugLevel.I, "File closed")

                    'Raise event
                    KernelEventManager.RaiseFileCreated(NewFile)
                Catch ex As Exception
                    WStkTrc(ex)
                    Throw New IOException(DoTranslation("Error trying to create a file: {0}").FormatString(ex.Message))
                End Try
            ElseIf ThrowIfFileExists Then
                Throw New IOException(DoTranslation("File already exists."))
            End If
        End Sub

        ''' <summary>
        ''' Makes a file
        ''' </summary>
        ''' <param name="NewFile">New file</param>
        ''' <returns>True if successful; False if unsuccessful</returns>
        ''' <exception cref="IOException"></exception>
        Public Function TryMakeFile(NewFile As String, Optional ThrowIfFileExists As Boolean = True) As Boolean
            Try
                MakeFile(NewFile, ThrowIfFileExists)
                Return True
            Catch ex As Exception
                Return False
            End Try
        End Function

        ''' <summary>
        ''' Makes an empty JSON file
        ''' </summary>
        ''' <param name="NewFile">New JSON file</param>
        ''' <exception cref="IOException"></exception>
        Public Sub MakeJsonFile(NewFile As String, Optional ThrowIfFileExists As Boolean = True)
            ThrowOnInvalidPath(NewFile)
            NewFile = NeutralizePath(NewFile)
            Wdbg(DebugLevel.I, "File path is {0} and .Exists is {0}", NewFile, FileExists(NewFile))
            If Not FileExists(NewFile) Then
                Try
                    Dim NewFileStream As FileStream = File.Create(NewFile)
                    Wdbg(DebugLevel.I, "File created")
                    Dim NewJsonObject As JObject = JObject.Parse("{}")
                    Dim NewFileWriter As New StreamWriter(NewFileStream)
                    NewFileWriter.WriteLine(JsonConvert.SerializeObject(NewJsonObject))
                    NewFileStream.Close()
                    Wdbg(DebugLevel.I, "File closed")

                    'Raise event
                    KernelEventManager.RaiseFileCreated(NewFile)
                Catch ex As Exception
                    WStkTrc(ex)
                    Throw New IOException(DoTranslation("Error trying to create a file: {0}").FormatString(ex.Message))
                End Try
            ElseIf ThrowIfFileExists Then
                Throw New IOException(DoTranslation("File already exists."))
            End If
        End Sub

        ''' <summary>
        ''' Makes an empty JSON file
        ''' </summary>
        ''' <param name="NewFile">New JSON file</param>
        ''' <returns>True if successful; False if unsuccessful</returns>
        ''' <exception cref="IOException"></exception>
        Public Function TryMakeJsonFile(NewFile As String, Optional ThrowIfFileExists As Boolean = True) As Boolean
            Try
                MakeJsonFile(NewFile, ThrowIfFileExists)
                Return True
            Catch ex As Exception
                Return False
            End Try
        End Function

    End Module
End Namespace
