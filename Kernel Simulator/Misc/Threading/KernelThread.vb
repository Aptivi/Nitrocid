
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

Namespace Misc.Threading
    ''' <summary>
    ''' The kernel thread to simplify the access to making new threads, starting them, and stopping them
    ''' </summary>
    Public Class KernelThread

        Private BaseThread As Thread
        Private ReadOnly ThreadDelegate As ThreadStart
        Private ReadOnly ThreadDelegateParameterized As ParameterizedThreadStart
        Private ReadOnly IsParameterized As Boolean

        ''' <summary>
        ''' The name of the thread
        ''' </summary>
        Public ReadOnly Property Name() As String

        ''' <summary>
        ''' Is the thread a background thread?
        ''' </summary>
        Public ReadOnly Property IsBackground() As Boolean

        ''' <summary>
        ''' Is the kernel thread alive?
        ''' </summary>
        Public ReadOnly Property IsAlive() As Boolean
            Get
                Return BaseThread.IsAlive
            End Get
        End Property

        ''' <summary>
        ''' Makes a new kernel thread
        ''' </summary>
        ''' <param name="ThreadName">The thread name</param>
        ''' <param name="Background">Indicates if the kernel thread is background</param>
        ''' <param name="Executor">The thread delegate</param>
        Public Sub New(ThreadName As String, Background As Boolean, Executor As ThreadStart)
            BaseThread = New Thread(Executor) With {.Name = ThreadName, .IsBackground = Background}
            IsParameterized = False
            ThreadDelegate = Executor
            Name = ThreadName
            IsBackground = Background
            Wdbg(DebugLevel.I, "Made a new kernel thread {0} with ID {1}", ThreadName, BaseThread.ManagedThreadId)
        End Sub

        ''' <summary>
        ''' Makes a new kernel thread
        ''' </summary>
        ''' <param name="ThreadName">The thread name</param>
        ''' <param name="Background">Indicates if the kernel thread is background</param>
        ''' <param name="Executor">The thread delegate</param>
        Public Sub New(ThreadName As String, Background As Boolean, Executor As ParameterizedThreadStart)
            BaseThread = New Thread(Executor) With {.Name = ThreadName, .IsBackground = Background}
            IsParameterized = True
            ThreadDelegateParameterized = Executor
            Name = ThreadName
            IsBackground = Background
            Wdbg(DebugLevel.I, "Made a new kernel thread {0} with ID {1}", ThreadName, BaseThread.ManagedThreadId)
        End Sub

        ''' <summary>
        ''' Starts the kernel thread
        ''' </summary>
        Public Sub Start()
            Wdbg(DebugLevel.I, "Starting kernel thread {0} with ID {1}", BaseThread.Name, BaseThread.ManagedThreadId)
            BaseThread.Start()
        End Sub

        ''' <summary>
        ''' Starts the kernel thread
        ''' </summary>
        ''' <param name="Parameter">The parameter class instance containing multiple parameters, or a usual single parameter</param>
        Public Sub Start(Parameter As Object)
            Wdbg(DebugLevel.I, "Starting kernel thread {0} with ID {1} with parameters", BaseThread.Name, BaseThread.ManagedThreadId)
            BaseThread.Start(Parameter)
        End Sub

        ''' <summary>
        ''' Stops the kernel thread
        ''' </summary>
        Public Sub [Stop]()
            Wdbg(DebugLevel.I, "Stopping kernel thread {0} with ID {1}", Name, BaseThread.ManagedThreadId)
            BaseThread.Interrupt()

            'Remake the thread to avoid illegal state exceptions
            If IsParameterized Then
                BaseThread = New Thread(ThreadDelegateParameterized) With {.Name = Name, .IsBackground = IsBackground}
            Else
                BaseThread = New Thread(ThreadDelegate) With {.Name = Name, .IsBackground = IsBackground}
            End If
            Wdbg(DebugLevel.I, "Made a new kernel thread {0} with ID {1}", Name, BaseThread.ManagedThreadId)
        End Sub

        ''' <summary>
        ''' Waits for the kernel thread to finish
        ''' </summary>
        Public Sub Wait()
            Wdbg(DebugLevel.I, "Waiting for kernel thread {0} with ID {1}", BaseThread.Name, BaseThread.ManagedThreadId)
            BaseThread.Join()
        End Sub

    End Class
End Namespace