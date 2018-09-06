
'    Kernel Simulator  Copyright (C) 2018  EoflaOE
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

Public Module Network

    Public Sub PingTarget(ByVal Address As String, Optional ByVal repeatTimes As Int16 = 3)

        Dim i As Int16 = 1
        Do
            Try
                Dim s As New Stopwatch
                If (repeatTimes <> 1) And Not (repeatTimes < 0) Then
                    For i = i To repeatTimes
                        s.Start()
                        If My.Computer.Network.Ping(Address) Then
                            Wln("{0}/{1} {2}: {3} ms", "neutralText", repeatTimes, i, Address, s.ElapsedMilliseconds.ToString)
                        End If
                        s.Reset()
                    Next
                ElseIf (repeatTimes = 1) Then
                    s.Start()
                    If My.Computer.Network.Ping(Address) Then
                        Wln("net: Got response from {0} in {1} ms", "neutralText", Address, s.ElapsedMilliseconds.ToString)
                    End If
                    s.Stop()
                End If
                If (i - 1 = repeatTimes) Then
                    Exit Sub
                End If
            Catch pe As Net.NetworkInformation.PingException
                If (repeatTimes = 1) Then
                    Wln("{0}: Timed out, disconnected, or server offline.", "neutralText", Address)
                    Exit Do
                Else
                    Wln("{0}/{1} {2}: Timed out, disconnected, or server offline.", "neutralText", repeatTimes, i, Address)
                    If (repeatTimes = i) Then
                        Exit Do
                    End If
                    i = i + 1
                    Continue Do
                End If
            End Try
        Loop

    End Sub

End Module
