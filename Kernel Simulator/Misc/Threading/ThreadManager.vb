
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

Public Module ThreadManager

    ''' <summary>
    ''' Sleeps until either the time specified, or the thread has finished or cancelled.
    ''' </summary>
    ''' <param name="Time">Time in milliseconds</param>
    ''' <param name="ThreadWork">The working thread</param>
    Public Sub SleepNoBlock(Time As Long, ThreadWork As BackgroundWorker)
        Dim WorkFinished As Boolean
        Dim TimeCount As Long
        AddHandler ThreadWork.RunWorkerCompleted, Sub() WorkFinished = True
        Do Until WorkFinished Or TimeCount = Time
            Threading.Thread.Sleep(1)
            If ThreadWork.CancellationPending Then WorkFinished = True
            TimeCount += 1
        Loop
    End Sub

End Module
