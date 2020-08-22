
'    Kernel Simulator  Copyright (C) 2018-2020  EoflaOE
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

Imports System.Globalization
Imports KS

<TestClass()> Public Class TimeTests

    <TestMethod()> Public Sub TestRenderKernelDate()
        Try
            KernelDateTime = Date.Now
            Assert.IsNotNull(RenderDate)
        Catch ex As Exception
            Assert.Fail("Rendering date failed")
        End Try
    End Sub

    <TestMethod()> Public Sub TestRenderKernelDateCult()
        Try
            KernelDateTime = Date.Now
            Dim TargetCult As New CultureInfo("es-ES")
            Assert.IsNotNull(RenderDate(TargetCult))
        Catch ex As Exception
            Assert.Fail("Rendering date failed")
        End Try
    End Sub

    <TestMethod()> Public Sub TestRenderCustomDate()
        Try
            Dim TargetDate As New DateTime(2018, 2, 22)
            Assert.IsNotNull(RenderDate(TargetDate))
        Catch ex As Exception
            Assert.Fail("Rendering date failed")
        End Try
    End Sub

    <TestMethod()> Public Sub TestRenderCustomDateCult()
        Try
            Dim TargetDate As New DateTime(2018, 2, 22)
            Dim TargetCult As New CultureInfo("es-ES")
            Assert.IsNotNull(RenderDate(TargetDate, TargetCult))
        Catch ex As Exception
            Assert.Fail("Rendering date failed")
        End Try
    End Sub

    <TestMethod()> Public Sub TestRenderKernelTime()
        Try
            KernelDateTime = Date.Now
            Assert.IsNotNull(RenderTime)
        Catch ex As Exception
            Assert.Fail("Rendering time failed")
        End Try
    End Sub

    <TestMethod()> Public Sub TestRenderKernelTimeCult()
        Try
            KernelDateTime = Date.Now
            Dim TargetCult As New CultureInfo("es-ES")
            Assert.IsNotNull(RenderTime(TargetCult))
        Catch ex As Exception
            Assert.Fail("Rendering time failed")
        End Try
    End Sub

    <TestMethod()> Public Sub TestRenderCustomTime()
        Try
            Dim TargetTime As New DateTime(2018, 2, 22, 5, 40, 37)
            Assert.IsNotNull(RenderTime(TargetTime))
        Catch ex As Exception
            Assert.Fail("Rendering time failed")
        End Try
    End Sub

    <TestMethod()> Public Sub TestRenderCustomTimeCult()
        Try
            Dim TargetTime As New DateTime(2018, 2, 22, 5, 40, 37)
            Dim TargetCult As New CultureInfo("es-ES")
            Assert.IsNotNull(RenderTime(TargetTime, TargetCult))
        Catch ex As Exception
            Assert.Fail("Rendering time failed")
        End Try
    End Sub

    <TestMethod()> Public Sub TestInitTimesInZones()
        Try
            KernelDateTime = Date.Now
            InitTimesInZones()
            Assert.IsFalse(zoneTimes.Count = 0)
        Catch ex As Exception
            Assert.Fail("Initialization of current time in zones failed")
        End Try
    End Sub

End Class