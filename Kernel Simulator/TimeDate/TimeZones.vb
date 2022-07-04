
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

Imports System.TimeZoneInfo

Namespace TimeDate
    Public Module TimeZones

        ''' <summary>
        ''' Populates current time in all of the time zones (IANA on Unix).
        ''' </summary>
        Public Function GetTimeZones() As Dictionary(Of String, Date)
            'Get all system time zones (IANA on Unix)
            Dim Zones As TimeZoneInfo() = GetSystemTimeZones.ToArray
            Dim ZoneTimes As New Dictionary(Of String, Date)
            Wdbg(DebugLevel.I, "Found {0} time zones.", Zones.Length)

            'Run a cleanup in the list
            ZoneTimes.Clear()
            Wdbg(DebugLevel.I, "Cleaned up zoneTimes.")

            'Adds date and time to every single time zone to the list
            For Each Zone In Zones
                ZoneTimes.Add(Zone.Id, ConvertTime(KernelDateTime, FindSystemTimeZoneById(Zone.Id)))
            Next

            'Return the populated array
            Return ZoneTimes
        End Function

        ''' <summary>
        ''' Shows current time in selected time zone
        ''' </summary>
        ''' <param name="zone">Time zone</param>
        ''' <returns>True if found; False if not found</returns>
        Public Function ShowTimeZone(Zone As String) As Boolean
            Dim ZoneTimes As Dictionary(Of String, Date) = GetTimeZones()
            Dim ZoneFound As Boolean = ZoneTimes.ContainsKey(Zone)
            If ZoneFound Then
                Write(DoTranslation("- Time of {0}: {1}") + " ({2})", True, ColTypes.Neutral, Zone, ZoneTimes(Zone).ToString(), FindSystemTimeZoneById(Zone).GetUtcOffset(KernelDateTime).ToString)
            End If
            Return ZoneFound
        End Function

        ''' <summary>
        ''' Shows current time in selected time zone
        ''' </summary>
        ''' <param name="zone">Time zone to search</param>
        ''' <returns>True if found; False if not found</returns>
        Public Function ShowTimeZones(Zone As String) As Boolean
            Dim ZoneTimes As Dictionary(Of String, Date) = GetTimeZones()
            Dim ZoneFound As Boolean
            For Each ZoneName As String In ZoneTimes.Keys
                If ZoneName.Contains(Zone) Then
                    ZoneFound = True
                    Write(DoTranslation("- Time of {0}: {1}") + " ({2})", True, ColTypes.Neutral, ZoneName, ZoneTimes(ZoneName).ToString(), FindSystemTimeZoneById(ZoneName).GetUtcOffset(KernelDateTime).ToString)
                End If
            Next
            Return ZoneFound
        End Function

        ''' <summary>
        ''' Shows current time in all time zones
        ''' </summary>
        Public Sub ShowAllTimeZones()
            Dim ZoneTimes As Dictionary(Of String, Date) = GetTimeZones()
            For Each TimeZone In ZoneTimes.Keys
                Write(DoTranslation("- Time of {0}: {1}") + " ({2})", True, ColTypes.Neutral, TimeZone, ZoneTimes(TimeZone).ToString(), FindSystemTimeZoneById(TimeZone).GetUtcOffset(KernelDateTime).ToString)
            Next
        End Sub

    End Module
End Namespace
