
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

Imports System.Diagnostics.Process

Public Module DisposeExit

    Private Declare Function SetProcessWorkingSetSize Lib "kernel32.dll" (ByVal hProcess As IntPtr, ByVal dwMinimumWorkingSetSize As Int32, ByVal dwMaximumWorkingSetSize As Int32) As Int32

    Public Sub DisposeAll()

        Try
            Wdbg("Garbage collector starting... Max generators: {0}", GC.MaxGeneration.ToString)
            GC.Collect()
            GC.WaitForPendingFinalizers()
            If (Environment.OSVersion.Platform = PlatformID.Win32NT) Then
                SetProcessWorkingSetSize(GetCurrentProcess().Handle, -1, -1)
            End If
            EventManager.RaiseGarbageCollected()
        Catch ex As Exception
            Wln(DoTranslation("Error trying to free RAM: {0} - Continuing...", currentLang), "neutralText", Err.Description)
            If (DebugMode = True) Then
                Wln(ex.StackTrace, "neutralText") : Wdbg("Error freeing RAM: {0} " + vbNewLine + "{1}", Err.Description, ex.StackTrace)
            End If
        End Try

    End Sub

End Module
