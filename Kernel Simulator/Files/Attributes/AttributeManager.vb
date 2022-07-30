
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
Imports System.Runtime.CompilerServices

Namespace Files
    Public Module AttributeManager

        ''' <summary>
        ''' Adds attribute to file
        ''' </summary>
        ''' <param name="FilePath">File path</param>
        ''' <param name="Attributes">Attributes</param>
        Public Sub AddAttributeToFile(FilePath As String, Attributes As FileAttributes)
            ThrowOnInvalidPath(FilePath)
            FilePath = NeutralizePath(FilePath)
            Wdbg(DebugLevel.I, "Setting file attribute to {0}...", Attributes)
            File.SetAttributes(FilePath, Attributes)

            'Raise event
            KernelEventManager.RaiseFileAttributeAdded(FilePath, Attributes)
        End Sub

        ''' <summary>
        ''' Adds attribute to file
        ''' </summary>
        ''' <param name="FilePath">File path</param>
        ''' <param name="Attributes">Attributes</param>
        ''' <returns>True if successful; False if unsuccessful</returns>
        Public Function TryAddAttributeToFile(FilePath As String, Attributes As FileAttributes) As Boolean
            Try
                AddAttributeToFile(FilePath, Attributes)
                Return True
            Catch ex As Exception
                Wdbg(DebugLevel.E, "Failed to add attribute {0} for file {1}: {2}", Attributes, Path.GetFileName(FilePath), ex.Message)
                WStkTrc(ex)
            End Try
            Return False
        End Function

        ''' <summary>
        ''' Removes attribute
        ''' </summary>
        ''' <param name="attributes">All attributes</param>
        ''' <param name="attributesToRemove">Attributes to remove</param>
        ''' <returns>Attributes without target attribute</returns>
        <Extension>
        Public Function RemoveAttribute(attributes As FileAttributes, attributesToRemove As FileAttributes) As FileAttributes
            Return attributes And (Not attributesToRemove)
        End Function

        ''' <summary>
        ''' Removes attribute from file
        ''' </summary>
        ''' <param name="FilePath">File path</param>
        ''' <param name="Attributes">Attributes</param>
        Public Sub RemoveAttributeFromFile(FilePath As String, Attributes As FileAttributes)
            ThrowOnInvalidPath(FilePath)
            FilePath = NeutralizePath(FilePath)
            Dim Attrib As FileAttributes = File.GetAttributes(FilePath)
            Wdbg(DebugLevel.I, "File attributes: {0}", Attrib)
            Attrib = Attrib.RemoveAttribute(Attributes)
            Wdbg(DebugLevel.I, "Setting file attribute to {0}...", Attrib)
            File.SetAttributes(FilePath, Attrib)

            'Raise event
            KernelEventManager.RaiseFileAttributeRemoved(FilePath, Attributes)
        End Sub

        ''' <summary>
        ''' Removes attribute from file
        ''' </summary>
        ''' <param name="FilePath">File path</param>
        ''' <param name="Attributes">Attributes</param>
        ''' <returns>True if successful; False if unsuccessful</returns>
        Public Function TryRemoveAttributeFromFile(FilePath As String, Attributes As FileAttributes) As Boolean
            Try
                RemoveAttributeFromFile(FilePath, Attributes)
                Return True
            Catch ex As Exception
                Wdbg(DebugLevel.E, "Failed to remove attribute {0} for file {1}: {2}", Attributes, Path.GetFileName(FilePath), ex.Message)
                WStkTrc(ex)
            End Try
            Return False
        End Function

    End Module
End Namespace
