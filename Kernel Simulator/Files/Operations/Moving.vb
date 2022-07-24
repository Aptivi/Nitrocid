
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
    Public Module Moving

        ''' <summary>
        ''' Moves a file or directory
        ''' </summary>
        ''' <param name="Source">Source file or directory</param>
        ''' <param name="Destination">Target file or directory</param>
        ''' <exception cref="IOException"></exception>
        Public Sub MoveFileOrDir(Source As String, Destination As String)
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
                Directory.Move(Source, Destination)

                'Raise event
                KernelEventManager.RaiseDirectoryMoved(Source, Destination)
            ElseIf FileExists(Source) And FolderExists(Destination) Then
                Wdbg(DebugLevel.I, "Source is a file and destination is a directory")
                File.Move(Source, Destination + "/" + FileName)

                'Raise event
                KernelEventManager.RaiseFileMoved(Source, Destination + "/" + FileName)
            ElseIf FileExists(Source) Then
                Wdbg(DebugLevel.I, "Source is a file and destination is a file")
                File.Move(Source, Destination)

                'Raise event
                KernelEventManager.RaiseFileMoved(Source, Destination)
            Else
                Wdbg(DebugLevel.E, "Source or destination are invalid.")
                Throw New IOException(DoTranslation("The path is neither a file nor a directory."))
            End If
        End Sub

        ''' <summary>
        ''' Moves a file or directory
        ''' </summary>
        ''' <param name="Source">Source file or directory</param>
        ''' <param name="Destination">Target file or directory</param>
        ''' <returns>True if successful; False if unsuccessful</returns>
        ''' <exception cref="IOException"></exception>
        Public Function TryMoveFileOrDir(Source As String, Destination As String) As Boolean
            Try
                MoveFileOrDir(Source, Destination)
                Return True
            Catch ex As Exception
                Wdbg(DebugLevel.E, "Failed to move {0} to {1}: {2}", Source, Destination, ex.Message)
                WStkTrc(ex)
            End Try
            Return False
        End Function

    End Module
End Namespace
