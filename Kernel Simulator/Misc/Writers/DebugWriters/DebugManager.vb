
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

Imports KS.Files.Read
Imports System.IO

Namespace Misc.Writers.DebugWriters
    Public Module DebugManager

        Public DebugQuota As Double = 1073741824 '1073741824 bytes = 1 GiB (1 GB for Windows)

        ''' <summary>
        ''' Checks to see if the debug file exceeds the quota
        ''' </summary>
        Public Sub CheckForDebugQuotaExceed()
            Try
                Dim FInfo As New FileInfo(GetKernelPath(KernelPathType.Debugging))
                Dim OldSize As Double = FInfo.Length
                If OldSize > DebugQuota Then
                    Dim Lines() As String = ReadAllLinesNoBlock(GetKernelPath(KernelPathType.Debugging)).Skip(5).ToArray
                    DebugStreamWriter.Close()
                    DebugStreamWriter = New StreamWriter(GetKernelPath(KernelPathType.Debugging)) With {.AutoFlush = True}
                    For l As Integer = 0 To Lines.Length - 1 'Remove the first 5 lines from stream.
                        DebugStreamWriter.WriteLine(Lines(l))
                    Next
                    Wdbg(DebugLevel.W, "Max debug quota size exceeded, was {0} bytes.", OldSize)
                End If
            Catch ex As Exception
                WStkTrc(ex)
            End Try
        End Sub

    End Module
End Namespace