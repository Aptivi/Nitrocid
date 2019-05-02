
'    Kernel Simulator  Copyright (C) 2018-2019  EoflaOE
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

Imports System.TimeZoneInfo

Public Module TimeZones

    'Time Zones in an array
    Public zones As TimeZoneInfo()
    Public zoneTimes As New Dictionary(Of String, DateTime)

    Public Sub InitTimesInZones()

        'Get all system time zones (IANA on Unix)
        zones = GetSystemTimeZones.ToArray

        'Run a cleanup in the list
        zoneTimes.Clear()

        'Adds date and time to every single time zone to the list
        For Each zone In zones
            zoneTimes.Add(zone.Id, ConvertTime(KernelDateTime, FindSystemTimeZoneById(zone.Id)))
        Next

    End Sub

    Public Sub ShowTimesInZones(Optional ByVal zone As String = "all")

        For Each timezone In zones
            If (timezone.DisplayName = zone) Then
                Wln(DoTranslation("- Time of {0}: {1}", currentLang), "neutralText", zone, zoneTimes(zone).ToString())
            End If
        Next
        If (zone = "all") Then
            For Each timezone In zoneTimes.Keys
                Wln(DoTranslation("- Time of {0}: {1}", currentLang), "neutralText", timezone, zoneTimes(timezone).ToString())
            Next
        End If

    End Sub

End Module
