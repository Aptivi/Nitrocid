
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

Module PlainDisplay

    Public WithEvents Plain As New NamedBackgroundWorker("Plain screensaver thread") With {.WorkerSupportsCancellation = True}

    ''' <summary>
    ''' Handles the code of Plain
    ''' </summary>
    Sub Plain_DoWork(sender As Object, e As DoWorkEventArgs) Handles Plain.DoWork
        'Preparations
        Console.BackgroundColor = ConsoleColor.Black
        Console.ForegroundColor = ConsoleColor.White
        Console.Clear()
        Console.CursorVisible = False
        Do While True
            If Plain.CancellationPending = True Then
                HandleSaverCancel()
                Exit Do
            End If
            SleepNoBlock(10, Plain)
        Loop
    End Sub

    ''' <summary>
    ''' Checks for any screensaver error
    ''' </summary>
    Sub CheckForError(sender As Object, e As RunWorkerCompletedEventArgs) Handles Plain.RunWorkerCompleted
        HandleSaverError(e.Error)
    End Sub

End Module
