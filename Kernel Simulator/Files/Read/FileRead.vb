
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

Imports System.IO

Namespace Files.Read
    Public Module FileRead

        ''' <summary>
        ''' Reads the contents of a file and writes it to the array. This is blocking and will put a lock on the file until read.
        ''' </summary>
        ''' <param name="filename">Full path to file</param>
        ''' <returns>An array full of file contents</returns>
        Public Function ReadContents(filename As String) As String()
            'Read the contents
            ThrowOnInvalidPath(filename)
            Dim FileContents As New List(Of String)
            filename = NeutralizePath(filename)
            Using FStream As New StreamReader(filename)
                Wdbg(DebugLevel.I, "Stream to file {0} opened.", filename)
                While Not FStream.EndOfStream
                    FileContents.Add(FStream.ReadLine)
                End While
            End Using
            Return FileContents.ToArray
        End Function

        ''' <summary>
        ''' Opens a file, reads all lines, and returns the array of lines
        ''' </summary>
        ''' <param name="path">Path to file</param>
        ''' <returns>Array of lines</returns>
        Public Function ReadAllLinesNoBlock(path As String) As String()
            ThrowOnInvalidPath(path)

            'Read all the lines, bypassing the restrictions.
            path = NeutralizePath(path)
            Dim AllLnList As New List(Of String)
            Dim FOpen As New StreamReader(File.Open(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            While Not FOpen.EndOfStream
                AllLnList.Add(FOpen.ReadLine)
            End While
            FOpen.Close()
            Return AllLnList.ToArray
        End Function

    End Module
End Namespace
