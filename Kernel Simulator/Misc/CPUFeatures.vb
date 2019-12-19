
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

Imports System.Runtime.InteropServices

Public Class CPUFeatures

    ' ----------------------------- Windows functions -----------------------------
    <DllImport("kernel32.dll")> 'Check for specific processor feature https://docs.microsoft.com/en-us/windows/win32/api/processthreadsapi/nf-processthreadsapi-isprocessorfeaturepresent
    Public Shared Function IsProcessorFeaturePresent(ByVal processorFeature As SSEnum) As <MarshalAs(UnmanagedType.Bool)> Boolean
    End Function

    Public Enum SSEnum As UInteger
        ''' <summary>
        ''' The SSE instruction set is available.
        ''' </summary>
        InstructionsSSEAvailable = 6
        ''' <summary>
        ''' The SSE2 instruction set is available. (This is used in most apps nowadays, since recent processors have this capability.)
        ''' </summary>
        InstructionsSSE2Available = 10
        ''' <summary>
        ''' The SSE3 instruction set is available.
        ''' </summary>
        InstructionsSSE3Available = 13
    End Enum

    ' ----------------------------- Unix functions -----------------------------
    Public Function CheckSSE(ByVal SSEVer As Integer) As Boolean
        Dim cpuinfo As New IO.StreamReader("/proc/cpuinfo")
        Dim ln As String
        Do While Not cpuinfo.EndOfStream
            ln = cpuinfo.ReadLine
            If ln.StartsWith("flags") Then
                If ln.Contains("sse") And SSEVer = 1 Then
                    Return True
                End If
                If ln.Contains("sse2") And SSEVer = 2 Then
                    Return True
                End If
                If ln.Contains("sse3") And SSEVer = 3 Then
                    Return True
                End If
                Exit Do
            End If
        Loop
        Return False
    End Function

End Class
