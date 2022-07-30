
'    Kernel Simulator  Copyright (C) 2018-2022  Aptivi
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
Imports KS.Misc.Probers

<TestFixture, Order(1)> Public Class MOTDSettingTests

    ''' <summary>
    ''' Tests setting MOTD
    ''' </summary>
    <Test, Description("Setting")> Public Sub TestSetMOTD()
        SetMOTD(ProbePlaces("Hello, I am on <system>"), MessageType.MOTD)
        Dim MOTDFile As New StreamReader(GetKernelPath(KernelPathType.MOTD))
        MOTDFile.ReadLine.ShouldBe(ProbePlaces("Hello, I am on <system>"))
        MOTDFile.Close()
    End Sub

    ''' <summary>
    ''' Tests setting MAL
    ''' </summary>
    <Test, Description("Setting")> Public Sub TestSetMAL()
        SetMOTD(ProbePlaces("Hello, I am on <system>"), MessageType.MAL)
        Dim MALFile As New StreamReader(GetKernelPath(KernelPathType.MAL))
        MALFile.ReadLine.ShouldBe(ProbePlaces("Hello, I am on <system>"))
        MALFile.Close()
    End Sub

End Class