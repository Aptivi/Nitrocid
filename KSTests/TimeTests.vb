
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

    ''' <summary>
    ''' Tests rendering kernel date
    ''' </summary>
    <TestMethod()> Public Sub TestRenderKernelDate()
        KernelDateTime = Date.Now
        Assert.IsNotNull(RenderDate, "Rendering date failed. Got null.")
    End Sub

    ''' <summary>
    ''' Tests rendering kernel date with specified culture
    ''' </summary>
    <TestMethod()> Public Sub TestRenderKernelDateCult()
        KernelDateTime = Date.Now
        Dim TargetCult As New CultureInfo("es-ES")
        Assert.IsNotNull(RenderDate(TargetCult), "Rendering date failed. Got null.")
    End Sub

    ''' <summary>
    ''' Tests rendering custom date
    ''' </summary>
    <TestMethod()> Public Sub TestRenderCustomDate()
        Dim TargetDate As New DateTime(2018, 2, 22)
        Assert.IsNotNull(RenderDate(TargetDate), "Rendering date failed. Got null.")
    End Sub

    ''' <summary>
    ''' Tests rendering custom date with specified culture
    ''' </summary>
    <TestMethod()> Public Sub TestRenderCustomDateCult()
        Dim TargetDate As New DateTime(2018, 2, 22)
        Dim TargetCult As New CultureInfo("es-ES")
        Assert.IsNotNull(RenderDate(TargetDate, TargetCult), "Rendering date failed. Got null.")
    End Sub

    ''' <summary>
    ''' Tests rendering kernel time
    ''' </summary>
    <TestMethod()> Public Sub TestRenderKernelTime()
        KernelDateTime = Date.Now
        Assert.IsNotNull(RenderTime, "Rendering time failed. Got null.")
    End Sub

    ''' <summary>
    ''' Tests rendering kernel time with specified culture
    ''' </summary>
    <TestMethod()> Public Sub TestRenderKernelTimeCult()
        KernelDateTime = Date.Now
        Dim TargetCult As New CultureInfo("es-ES")
        Assert.IsNotNull(RenderTime(TargetCult), "Rendering time failed. Got null.")
    End Sub

    ''' <summary>
    ''' Tests rendering custom time
    ''' </summary>
    <TestMethod()> Public Sub TestRenderCustomTime()
        Dim TargetTime As New DateTime(2018, 2, 22, 5, 40, 37)
        Assert.IsNotNull(RenderTime(TargetTime), "Rendering time failed. Got null.")
    End Sub

    ''' <summary>
    ''' Tests rendering custom time with specified culture
    ''' </summary>
    <TestMethod()> Public Sub TestRenderCustomTimeCult()
        Dim TargetTime As New DateTime(2018, 2, 22, 5, 40, 37)
        Dim TargetCult As New CultureInfo("es-ES")
        Assert.IsNotNull(RenderTime(TargetTime, TargetCult), "Rendering time failed. Got null.")
    End Sub

    ''' <summary>
    ''' Tests rendering kernel date
    ''' </summary>
    <TestMethod()> Public Sub TestRenderKernel()
        KernelDateTime = Date.Now
        Assert.IsNotNull(Render, "Rendering failed. Got null.")
    End Sub

    ''' <summary>
    ''' Tests rendering kernel date with specified culture
    ''' </summary>
    <TestMethod()> Public Sub TestRenderKernelCult()
        KernelDateTime = Date.Now
        Dim TargetCult As New CultureInfo("es-ES")
        Assert.IsNotNull(Render(TargetCult), "Rendering failed. Got null.")
    End Sub

    ''' <summary>
    ''' Tests rendering custom date
    ''' </summary>
    <TestMethod()> Public Sub TestRenderCustom()
        Dim TargetDate As New DateTime(2018, 2, 22)
        Assert.IsNotNull(Render(TargetDate), "Rendering failed. Got null.")
    End Sub

    ''' <summary>
    ''' Tests rendering custom date with specified culture
    ''' </summary>
    <TestMethod()> Public Sub TestRenderCustomCult()
        Dim TargetDate As New DateTime(2018, 2, 22)
        Dim TargetCult As New CultureInfo("es-ES")
        Assert.IsNotNull(Render(TargetDate, TargetCult), "Rendering failed. Got null.")
    End Sub

    ''' <summary>
    ''' Tests initializing current times in all timezones
    ''' </summary>
    <TestMethod()> Public Sub TestInitTimesInZones()
        KernelDateTime = Date.Now
        InitTimesInZones()
        Assert.IsFalse(zoneTimes.Count = 0, "Initialization of current time in zones failed. Got 0.")
    End Sub

End Class