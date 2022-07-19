
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

Namespace Files.Operations
    Public Module Copying

        ''' <summary>
        ''' Copies a file or directory
        ''' </summary>
        ''' <param name="Source">Source file or directory</param>
        ''' <param name="Destination">Target file or directory</param>
        ''' <returns>True if successful; False if unsuccessful</returns>
        ''' <exception cref="IOException"></exception>
        Public Function CopyFileOrDir(Source As String, Destination As String) As Boolean
            Try
                ThrowOnInvalidPath(Source)
                ThrowOnInvalidPath(Destination)
                Source = NeutralizePath(Source)
                Wdbg(DebugLevel.I, "Source directory: {0}", Source)
                Destination = NeutralizePath(Destination)
                Wdbg(DebugLevel.I, "Target directory: {0}", Destination)
                Dim FileName As String = Path.GetFileName(Source)
                Wdbg(DebugLevel.I, "Source file name: {0}", FileName)
                If FolderExists(Source) Then
                    Wdbg(DebugLevel.I, "Source and destination are directories")
                    CopyDirectory(Source, Destination)

                    'Raise event
                    KernelEventManager.RaiseDirectoryCopied(Source, Destination)
                    Return True
                ElseIf FileExists(Source) And FolderExists(Destination) Then
                    Wdbg(DebugLevel.I, "Source is a file and destination is a directory")
                    File.Copy(Source, Destination + "/" + FileName, True)

                    'Raise event
                    KernelEventManager.RaiseFileCopied(Source, Destination + "/" + FileName)
                    Return True
                ElseIf FileExists(Source) Then
                    Wdbg(DebugLevel.I, "Source is a file and destination is a file")
                    File.Copy(Source, Destination, True)

                    'Raise event
                    KernelEventManager.RaiseFileCopied(Source, Destination)
                    Return True
                Else
                    Wdbg(DebugLevel.E, "Source or destination are invalid.")
                    Throw New IOException(DoTranslation("The path is neither a file nor a directory."))
                End If
            Catch ex As Exception
                Wdbg(DebugLevel.E, "Failed to copy {0} to {1}: {2}", Source, Destination, ex.Message)
                WStkTrc(ex)
                Throw New IOException(DoTranslation("Failed to copy file or directory: {0}").FormatString(ex.Message))
            End Try
            Return False
        End Function

        ''' <summary>
        ''' Copies the directory from source to destination
        ''' </summary>
        ''' <param name="Source">Source directory</param>
        ''' <param name="Destination">Target directory</param>
        Public Sub CopyDirectory(Source As String, Destination As String)
            CopyDirectory(Source, Destination, ShowFilesystemProgress)
        End Sub

        ''' <summary>
        ''' Copies the directory from source to destination
        ''' </summary>
        ''' <param name="Source">Source directory</param>
        ''' <param name="Destination">Target directory</param>
        ''' <param name="ShowProgress">Whether or not to show what files are being copied</param>
        Public Sub CopyDirectory(Source As String, Destination As String, ShowProgress As Boolean)
            ThrowOnInvalidPath(Source)
            ThrowOnInvalidPath(Destination)
            If Not FolderExists(Source) Then Throw New IOException(DoTranslation("Directory {0} not found.").FormatString(Source))

            'Get all source directories and files
            Dim SourceDirInfo As New DirectoryInfo(Source)
            Dim SourceDirectories As DirectoryInfo() = SourceDirInfo.GetDirectories
            Wdbg(DebugLevel.I, "Source directories: {0}", SourceDirectories.Length)
            Dim SourceFiles As FileInfo() = SourceDirInfo.GetFiles
            Wdbg(DebugLevel.I, "Source files: {0}", SourceFiles.Length)

            'Make a destination directory if it doesn't exist
            If Not FolderExists(Destination) Then
                Wdbg(DebugLevel.I, "Destination directory {0} doesn't exist. Creating...", Destination)
                Directory.CreateDirectory(Destination)
            End If

            'Iterate through every file and copy them to destination
            For Each SourceFile As FileInfo In SourceFiles
                Dim DestinationFilePath As String = Path.Combine(Destination, SourceFile.Name)
                Wdbg(DebugLevel.I, "Copying file {0} to destination...", DestinationFilePath)
                If ShowProgress Then Write("-> {0}", True, ColTypes.Neutral, DestinationFilePath)
                SourceFile.CopyTo(DestinationFilePath, True)
            Next

            'Iterate through every subdirectory and copy them to destination
            For Each SourceDirectory As DirectoryInfo In SourceDirectories
                Dim DestinationDirectoryPath As String = Path.Combine(Destination, SourceDirectory.Name)
                Wdbg(DebugLevel.I, "Calling CopyDirectory() with destination {0}...", DestinationDirectoryPath)
                If ShowProgress Then Write("* {0}", True, ColTypes.Neutral, DestinationDirectoryPath)
                CopyDirectory(SourceDirectory.FullName, DestinationDirectoryPath)
            Next
        End Sub

    End Module
End Namespace