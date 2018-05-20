
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

Imports System.IO

Module DebugWriter

    Public dbgWriter As New StreamWriter(Environ("USERPROFILE") + "\kernelDbg.log")

    Sub Wdbg(ByVal text As String, ByVal line As Boolean, ByVal ParamArray vars() As Object)

        If (DebugMode = True) Then
            If (line = False) Then
                dbgWriter.Write(FormatDateTime(CDate(strKernelTimeDate), DateFormat.ShortDate) + " " + FormatDateTime(CDate(strKernelTimeDate), DateFormat.ShortTime) + ": " + text, vars)
            ElseIf (line = True) Then
                dbgWriter.WriteLine(FormatDateTime(CDate(strKernelTimeDate), DateFormat.ShortDate) + " " + FormatDateTime(CDate(strKernelTimeDate), DateFormat.ShortTime) + ": " + text, vars)
            End If
        End If

    End Sub

End Module
