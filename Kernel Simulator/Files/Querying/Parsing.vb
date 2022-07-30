
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

Imports System.IO

Namespace Files.Querying
    Public Module Parsing

        ''' <summary>
        ''' Gets all the invalid path characters
        ''' </summary>
        Public Function GetInvalidPathChars() As Char()
            Dim FinalInvalidPathChars As Char() = Path.GetInvalidPathChars()
            Dim WindowsInvalidPathChars As Char() = {"""", "<", ">"}
            If KernelSimulatorMoniker = ".NET CoreCLR" And IsOnWindows() Then
                'It's weird of .NET 6.0 to not consider the above three Windows invalid directory chars to be invalid,
                'so make them invalid as in .NET Framework.
                ReDim Preserve FinalInvalidPathChars(35)
                WindowsInvalidPathChars.CopyTo(FinalInvalidPathChars, FinalInvalidPathChars.Length - 3)
            End If
            Return FinalInvalidPathChars
        End Function

        ''' <summary>
        ''' Tries to parse the path (For file names and only names, use <see cref="TryParseFileName(String)"/> instead.)
        ''' </summary>
        ''' <param name="Path">The path to be parsed</param>
        ''' <returns>True if successful; false if unsuccessful</returns>
        Public Function TryParsePath(Path As String) As Boolean
            Try
                ThrowOnInvalidPath(Path)
                Return Not Path.IndexOfAny(GetInvalidPathChars()) >= 0
            Catch ex As Exception
                WStkTrc(ex)
                Wdbg(DebugLevel.E, "Failed to parse path {0}: {1}", Path, ex.Message)
            End Try
            Return False
        End Function

        ''' <summary>
        ''' Tries to parse the file name (For full paths, use <see cref="TryParsePath(String)"/> instead.)
        ''' </summary>
        ''' <param name="Name">The file name to be parsed</param>
        ''' <returns>True if successful; false if unsuccessful</returns>
        Public Function TryParseFileName(Name As String) As Boolean
            Try
                ThrowOnInvalidPath(Name)
                Return Not Name.IndexOfAny(Path.GetInvalidFileNameChars()) >= 0
            Catch ex As Exception
                WStkTrc(ex)
                Wdbg(DebugLevel.E, "Failed to parse file name {0}: {1}", Name, ex.Message)
            End Try
            Return False
        End Function

    End Module
End Namespace
