
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

Imports KS.Misc.Threading

<TestClass()> Public Class KernelThreadTests

    Shared TargetThread As KernelThread
    Shared TargetParameterizedThread As KernelThread

    ''' <summary>
    ''' Tests initializing kernel thread
    ''' </summary>
    <TestMethod()> <TestCategory("Initialization")> Public Sub TestInitializeKernelThread()
        TargetThread = New KernelThread("Unit test thread #1", True, AddressOf WriteHello)
    End Sub

    ''' <summary>
    ''' Tests initializing kernel parameterized thread
    ''' </summary>
    <TestMethod()> <TestCategory("Initialization")> Public Sub TestInitializeKernelParameterizedThread()
        TargetParameterizedThread = New KernelThread("Unit test thread #2", True, AddressOf WriteHelloWithArgument)
    End Sub

    ''' <summary>
    ''' Tests starting kernel thread
    ''' </summary>
    <TestMethod()> <TestCategory("Initialization")> Public Sub TestStartKernelThread()
        TargetThread.Start()
    End Sub

    ''' <summary>
    ''' Tests starting kernel parameterized thread
    ''' </summary>
    <TestMethod()> <TestCategory("Initialization")> Public Sub TestStartKernelParameterizedThread()
        TargetParameterizedThread.Start("Agustin")
    End Sub

    ''' <summary>
    ''' Tests stopping kernel thread
    ''' </summary>
    <TestMethod()> <TestCategory("Initialization")> Public Sub TestStopKernelThread()
        Threading.Thread.Sleep(300)
        TargetThread.Stop()
        TargetThread.ShouldNotBeNull
    End Sub

    ''' <summary>
    ''' Tests stopping kernel parameterized thread
    ''' </summary>
    <TestMethod()> <TestCategory("Initialization")> Public Sub TestStopKernelParameterizedThread()
        Threading.Thread.Sleep(300)
        TargetParameterizedThread.Stop()
        TargetParameterizedThread.ShouldNotBeNull
    End Sub

End Class