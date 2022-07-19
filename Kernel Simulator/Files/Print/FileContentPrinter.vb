
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

Imports KS.Files.Folders
Imports KS.Files.Read

Namespace Files.Print
    Public Module FileContentPrinter

        ''' <summary>
        ''' Prints the contents of a file to the console
        ''' </summary>
        ''' <param name="filename">Full path to file</param>
        Public Sub PrintContents(filename As String)
            PrintContents(filename, PrintLineNumbers)
        End Sub

        ''' <summary>
        ''' Prints the contents of a file to the console
        ''' </summary>
        ''' <param name="filename">Full path to file with wildcards supported</param>
        Public Sub PrintContents(filename As String, PrintLineNumbers As Boolean)
            'Read the contents
            ThrowOnInvalidPath(filename)
            filename = NeutralizePath(filename)
            For Each FilePath As String In GetFilesystemEntries(filename, True)
                Dim Contents As String() = ReadContents(FilePath)
                For ContentIndex As Integer = 0 To Contents.Length - 1
                    If PrintLineNumbers Then
                        Write("{0,4}: ", False, ColTypes.ListEntry, ContentIndex + 1)
                    End If
                    Write(Contents(ContentIndex), True, ColTypes.Neutral)
                Next
            Next
        End Sub

    End Module
End Namespace