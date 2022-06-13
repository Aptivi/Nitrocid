
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

Imports System.Threading
Imports KS.Misc.Screensaver.Customized

Namespace Misc.Screensaver.Displays
    Public Module CustomDisplay

        Public ReadOnly Custom As New KernelThread("Custom screensaver thread", True, AddressOf Custom_DoWork)

        ''' <summary>
        ''' Handles custom screensaver code
        ''' </summary>
        Sub Custom_DoWork()
            Try
                'To Screensaver Developers: ONLY put the effect code in your scrnSaver() sub.
                '                           Set colors, write welcome message, etc. with the exception of infinite loop and the effect code in preDisplay() sub
                '                           Recommended: Turn off console cursor, and clear the screen in preDisplay() sub.
                '                           Substitute: TextWriterColor.Write() with System.Console.WriteLine() or System.Console.Write().
                'Preparations
                Console.CursorVisible = False

                'Screensaver logic
                Wdbg(DebugLevel.I, "Entered CustomSaver.PreDisplay().")
                CustomSaver.PreDisplay()
                Wdbg(DebugLevel.I, "Exited CustomSaver.PreDisplay().")
                Do While True
                    Wdbg(DebugLevel.I, "Entered CustomSaver.ScrnSaver().")
                    CustomSaver.ScrnSaver()
                    Wdbg(DebugLevel.I, "Exited CustomSaver.ScrnSaver().")
                    If Not CustomSaver.DelayForEachWrite = Nothing Then
                        SleepNoBlock(CustomSaver.DelayForEachWrite, Custom)
                    End If
                Loop
            Catch taex As ThreadInterruptedException
                Wdbg(DebugLevel.W, "Cancellation requested. Showing ending...")
                Wdbg(DebugLevel.I, "Entered CustomSaver.PostDisplay().")
                CustomSaver.PostDisplay()
                Wdbg(DebugLevel.I, "Exited CustomSaver.PostDisplay().")
                HandleSaverCancel()
            Catch ex As Exception
                HandleSaverError(ex)
            End Try
        End Sub

    End Module
End Namespace
