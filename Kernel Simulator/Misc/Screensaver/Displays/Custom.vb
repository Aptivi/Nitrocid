
'    Kernel Simulator  Copyright (C) 2018-2021  EoflaOE
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

Imports System.ComponentModel

Public Module CustomDisplay

    Public WithEvents Custom As New BackgroundWorker With {.WorkerSupportsCancellation = True}

    ''' <summary>
    ''' Handles custom screensaver code
    ''' </summary>
    Sub Custom_DoWork(sender As Object, e As DoWorkEventArgs) Handles Custom.DoWork
        'To Screensaver Developers: ONLY put the effect code in your scrnSaver() sub.
        '                           Set colors, write welcome message, etc. with the exception of infinite loop and the effect code in preDisplay() sub
        '                           Recommended: Turn off console cursor, and clear the screen in preDisplay() sub.
        '                           Substitute: TextWriterColor.W() with System.Console.WriteLine() or System.Console.Write().
        'Preparations
        Console.CursorVisible = False

        'Screensaver logic
        CustomSaver.PreDisplay()
        Do While True
            If Custom.CancellationPending = True Then
                Wdbg(DebugLevel.W, "Cancellation requested. Showing ending...")
                CustomSaver.PostDisplay()
                HandleSaverCancel()
                Exit Do
            Else
                CustomSaver.ScrnSaver()
            End If
            If Not CustomSaver.DelayForEachWrite = Nothing Then
                SleepNoBlock(CustomSaver.DelayForEachWrite, Custom)
            End If
        Loop
    End Sub

    ''' <summary>
    ''' Checks for any screensaver error
    ''' </summary>
    Sub CheckForError(sender As Object, e As RunWorkerCompletedEventArgs) Handles Custom.RunWorkerCompleted
        HandleSaverError(e.Error)
    End Sub

End Module
