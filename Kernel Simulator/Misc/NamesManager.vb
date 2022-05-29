
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

Imports KS.Network

Namespace Misc
    Public Module NamesManager

        Friend Names() As String = {}
        Friend Surnames() As String = {}

        ''' <summary>
        ''' Populates the names and the surnames for the purpose of initialization
        ''' </summary>
        Public Sub PopulateNames()
            Try
                Wdbg(DebugLevel.I, "Populating names...")
                If Names.Length = 0 Then Names = DownloadString("https://cdn.jsdelivr.net/gh/smashew/NameDatabases@master/NamesDatabases/first%20names/all.txt").SplitNewLines
                Wdbg(DebugLevel.I, "Populating surnames...")
                If Surnames.Length = 0 Then Surnames = DownloadString("https://cdn.jsdelivr.net/gh/smashew/NameDatabases@master/NamesDatabases/surnames/all.txt").SplitNewLines
                Wdbg(DebugLevel.I, "Got {0} names and {1} surnames.", Names.Length, Surnames.Length)
            Catch ex As Exception
                Wdbg(DebugLevel.E, "Can't get names and surnames: {0}", ex.Message)
                WStkTrc(ex)
                Throw New Exception(DoTranslation("Can't get names and surnames:") + $" {ex.Message}", ex)
            End Try
        End Sub

        ''' <summary>
        ''' Generates the names
        ''' </summary>
        ''' <returns>List of generated names</returns>
        Public Function GenerateNames(Count As Integer) As List(Of String)
            Dim RandomDriver As New Random()
            Dim NamesList As New List(Of String)

            'Initialize names
            PopulateNames()

            'Select random names
            For NameNum As Integer = 1 To Count
                Dim GeneratedName As String = Names(RandomDriver.Next(Names.Length))
                Dim GeneratedSurname As String = Surnames(RandomDriver.Next(Surnames.Length))
                NamesList.Add(GeneratedName + " " + GeneratedSurname)
            Next
            Return NamesList
        End Function

    End Module
End Namespace
