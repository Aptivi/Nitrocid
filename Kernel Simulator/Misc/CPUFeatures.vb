
'    Kernel Simulator  Copyright (C) 2018-2020  EoflaOE
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

Public Class CPUFeatures_Win

    ''' <summary>
    ''' [Windows] Check for specific processor feature. More info: https://docs.microsoft.com/en-us/windows/win32/api/processthreadsapi/nf-processthreadsapi-isprocessorfeaturepresent
    ''' </summary>
    ''' <param name="processorFeature">An SSE version</param>
    ''' <returns>True if supported, false if not supported</returns>
    <DllImport("kernel32.dll")>
    Public Shared Function IsProcessorFeaturePresent(ByVal processorFeature As SSEnum) As <MarshalAs(UnmanagedType.Bool)> Boolean
    End Function

    ''' <summary>
    ''' [Windows] Collection of SSE versions
    ''' </summary>
    Public Enum SSEnum As UInteger
        ''' <summary>
        ''' [Windows] The SSE instruction set is available.
        ''' </summary>
        InstructionsSSEAvailable = 6
        ''' <summary>
        ''' [Windows] The SSE2 instruction set is available. (This is used in most apps nowadays, since recent processors have this capability.)
        ''' </summary>
        InstructionsSSE2Available = 10
        ''' <summary>
        ''' [Windows] The SSE3 instruction set is available.
        ''' </summary>
        InstructionsSSE3Available = 13
    End Enum

End Class

Public Module CPUFeatures_Linux

    ''' <summary>
    ''' [Linux] Checks for a specified CPU SSE version
    ''' </summary>
    ''' <param name="SSEVer">SSE version</param>
    ''' <returns>True if supported, false if not supported</returns>
    Public Function CheckSSE(ByVal SSEVer As Integer) As Boolean
        If EnvironmentOSType.Contains("Unix") Then
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
        Else
            Throw New PlatformNotSupportedException
        End If
    End Function

End Module
