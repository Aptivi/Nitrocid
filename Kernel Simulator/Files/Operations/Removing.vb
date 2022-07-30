
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

Namespace Files.Operations
    Public Module Removing

        ''' <summary>
        ''' Removes a directory
        ''' </summary>
        ''' <param name="Target">Target directory</param>
        Public Sub RemoveDirectory(Target As String)
            ThrowOnInvalidPath(Target)
            Dim Dir As String = NeutralizePath(Target)
            Directory.Delete(Dir, True)

            'Raise event
            KernelEventManager.RaiseDirectoryRemoved(Target)
        End Sub

        ''' <summary>
        ''' Removes a directory
        ''' </summary>
        ''' <param name="Target">Target directory</param>
        ''' <returns>True if successful; False if unsuccessful</returns>
        Public Function TryRemoveDirectory(Target As String) As Boolean
            Try
                RemoveDirectory(Target)
                Return True
            Catch ex As Exception
                WStkTrc(ex)
            End Try
            Return False
        End Function

        ''' <summary>
        ''' Removes a file
        ''' </summary>
        ''' <param name="Target">Target directory</param>
        Public Sub RemoveFile(Target As String)
            ThrowOnInvalidPath(Target)
            Dim Dir As String = NeutralizePath(Target)
            File.Delete(Dir)

            'Raise event
            KernelEventManager.RaiseFileRemoved(Target)
        End Sub

        ''' <summary>
        ''' Removes a file
        ''' </summary>
        ''' <param name="Target">Target directory</param>
        ''' <returns>True if successful; False if unsuccessful</returns>
        Public Function TryRemoveFile(Target As String) As Boolean
            Try
                RemoveFile(Target)
                Return True
            Catch ex As Exception
                WStkTrc(ex)
            End Try
            Return False
        End Function

    End Module
End Namespace
