
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

Namespace Kernel
    Public Module EventsManager

        ''' <summary>
        ''' Lists all the fired events with arguments
        ''' </summary>
        Public Function ListAllFiredEvents() As Dictionary(Of String, Object())
            Return ListAllFiredEvents("")
        End Function

        ''' <summary>
        ''' Lists all the fired events with arguments
        ''' </summary>
        ''' <param name="SearchTerm">The search term</param>
        Public Function ListAllFiredEvents(SearchTerm As String) As Dictionary(Of String, Object())
            Dim FiredEvents As New Dictionary(Of String, Object())

            'Enumerate all the fired events
            For Each FiredEvent As String In KernelEventManager.FiredEvents.Keys
                If FiredEvent.Contains(SearchTerm) Then
                    Dim EventArguments As Object() = KernelEventManager.FiredEvents(FiredEvent)
                    FiredEvents.Add(FiredEvent, EventArguments)
                End If
            Next
            Return FiredEvents
        End Function

        ''' <summary>
        ''' Clears all the fired events
        ''' </summary>
        Public Sub ClearAllFiredEvents()
            KernelEventManager.FiredEvents.Clear()
        End Sub

    End Module
End Namespace