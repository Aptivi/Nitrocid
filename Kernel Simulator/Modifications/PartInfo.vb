
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

Namespace Modifications
    Public Class PartInfo

        ''' <summary>
        ''' The mod name. If no name is specified, or if it only consists of whitespaces (space), the file name is taken.
        ''' </summary>
        Public ReadOnly Property ModName As String
        ''' <summary>
        ''' The part name.
        ''' </summary>
        Public ReadOnly Property PartName As String
        ''' <summary>
        ''' The mod part file name
        ''' </summary>
        Public ReadOnly Property PartFileName As String
        ''' <summary>
        ''' The mod part file path
        ''' </summary>
        Public ReadOnly Property PartFilePath As String
        ''' <summary>
        ''' The mod part script
        ''' </summary>
        Public ReadOnly Property PartScript As IScript

        ''' <summary>
        ''' Creates new mod info instance
        ''' </summary>
        Friend Sub New(ModName As String, PartName As String, PartFileName As String, PartFilePath As String, PartScript As IScript)
            'Validate values. Check to see if the name is null. If so, it will take the mod file name.
            If String.IsNullOrWhiteSpace(ModName) Then
                ModName = PartFileName
            End If

            'Check to see if the part script is null. If so, throw exception.
            If PartScript Is Nothing Then
                Throw New Exceptions.ModNoPartsException(DoTranslation("Mod part is nothing."))
            End If

            'Install values to new instance
            Me.ModName = ModName
            Me.PartName = PartName
            Me.PartFileName = PartFileName
            Me.PartFilePath = PartFilePath
            Me.PartScript = PartScript
        End Sub

    End Class
End Namespace
