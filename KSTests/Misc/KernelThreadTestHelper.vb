
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

Public Module KernelThreadTestHelper

    ''' <summary>
    ''' [Kernel thread test] Write hello to console
    ''' </summary>
    Sub WriteHello()
        Try
            Console.WriteLine("Hello world!")
            Console.WriteLine("- Writing from thread: {0} [{1}]", Thread.CurrentThread.Name, Thread.CurrentThread.ManagedThreadId)
            While True
                Thread.Sleep(1)
            End While
        Catch ex As ThreadInterruptedException
            Console.WriteLine("- Goodbye from thread: {0} [{1}]", Thread.CurrentThread.Name, Thread.CurrentThread.ManagedThreadId)
        End Try
    End Sub

    ''' <summary>
    ''' [Kernel thread test] Write hello to console with argument
    ''' </summary>
    Sub WriteHelloWithArgument(Name As String)
        Try
            Console.WriteLine("Hello, {0}!", Name)
            Console.WriteLine("- Writing from thread: {0} [{1}]", Thread.CurrentThread.Name, Thread.CurrentThread.ManagedThreadId)
            While True
                Thread.Sleep(1)
            End While
        Catch ex As ThreadInterruptedException
            Console.WriteLine("- Goodbye from thread: {0} [{1}]", Thread.CurrentThread.Name, Thread.CurrentThread.ManagedThreadId)
        End Try
    End Sub

End Module
