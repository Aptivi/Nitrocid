
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

Imports System.Globalization
Imports KS

<TestClass()> Public Class TimeTests

    ''' <summary>
    ''' Tests rendering kernel date
    ''' </summary>
    <TestMethod()> Public Sub TestRenderKernelDate()
        KernelDateTime = Date.Now
        RenderDate.ShouldNotBeNullOrEmpty
    End Sub

    ''' <summary>
    ''' Tests rendering kernel date with specific format type
    ''' </summary>
    <TestMethod()> Public Sub TestRenderKernelDateType()
        KernelDateTime = Date.Now
        RenderDate(FormatType.Long).ShouldNotBeNullOrEmpty
        RenderDate(FormatType.Short).ShouldNotBeNullOrEmpty
    End Sub

    ''' <summary>
    ''' Tests rendering kernel date with specified culture
    ''' </summary>
    <TestMethod()> Public Sub TestRenderKernelDateCult()
        KernelDateTime = Date.Now
        Dim TargetCult As New CultureInfo("es-ES")
        RenderDate(TargetCult).ShouldNotBeNullOrEmpty
    End Sub

    ''' <summary>
    ''' Tests rendering kernel date with specified culture and format type
    ''' </summary>
    <TestMethod()> Public Sub TestRenderKernelDateCultType()
        KernelDateTime = Date.Now
        Dim TargetCult As New CultureInfo("es-ES")
        RenderDate(TargetCult, FormatType.Long).ShouldNotBeNullOrEmpty
        RenderDate(TargetCult, FormatType.Short).ShouldNotBeNullOrEmpty
    End Sub

    ''' <summary>
    ''' Tests rendering custom date
    ''' </summary>
    <TestMethod()> Public Sub TestRenderCustomDate()
        Dim TargetDate As New DateTime(2018, 2, 22)
        RenderDate(TargetDate).ShouldNotBeNullOrEmpty
    End Sub

    ''' <summary>
    ''' Tests rendering custom date
    ''' </summary>
    <TestMethod()> Public Sub TestRenderCustomDateType()
        Dim TargetDate As New DateTime(2018, 2, 22)
        RenderDate(TargetDate, FormatType.Long).ShouldNotBeNullOrEmpty
        RenderDate(TargetDate, FormatType.Short).ShouldNotBeNullOrEmpty
    End Sub

    ''' <summary>
    ''' Tests rendering custom date with specified culture
    ''' </summary>
    <TestMethod()> Public Sub TestRenderCustomDateCult()
        Dim TargetDate As New DateTime(2018, 2, 22)
        Dim TargetCult As New CultureInfo("es-ES")
        RenderDate(TargetDate, TargetCult).ShouldNotBeNullOrEmpty
    End Sub

    ''' <summary>
    ''' Tests rendering custom date with specified culture
    ''' </summary>
    <TestMethod()> Public Sub TestRenderCustomDateCultType()
        Dim TargetDate As New DateTime(2018, 2, 22)
        Dim TargetCult As New CultureInfo("es-ES")
        RenderDate(TargetDate, TargetCult, FormatType.Long).ShouldNotBeNullOrEmpty
        RenderDate(TargetDate, TargetCult, FormatType.Short).ShouldNotBeNullOrEmpty
    End Sub

    ''' <summary>
    ''' Tests rendering kernel time
    ''' </summary>
    <TestMethod()> Public Sub TestRenderKernelTime()
        KernelDateTime = Date.Now
        RenderTime.ShouldNotBeNullOrEmpty
    End Sub

    ''' <summary>
    ''' Tests rendering kernel time
    ''' </summary>
    <TestMethod()> Public Sub TestRenderKernelTimeType()
        KernelDateTime = Date.Now
        RenderTime(FormatType.Long).ShouldNotBeNullOrEmpty
        RenderTime(FormatType.Short).ShouldNotBeNullOrEmpty
    End Sub

    ''' <summary>
    ''' Tests rendering kernel time with specified culture
    ''' </summary>
    <TestMethod()> Public Sub TestRenderKernelTimeCult()
        KernelDateTime = Date.Now
        Dim TargetCult As New CultureInfo("es-ES")
        RenderTime(TargetCult).ShouldNotBeNullOrEmpty
    End Sub

    ''' <summary>
    ''' Tests rendering kernel time with specified culture
    ''' </summary>
    <TestMethod()> Public Sub TestRenderKernelTimeCultType()
        KernelDateTime = Date.Now
        Dim TargetCult As New CultureInfo("es-ES")
        RenderTime(TargetCult, FormatType.Long).ShouldNotBeNullOrEmpty
        RenderTime(TargetCult, FormatType.Short).ShouldNotBeNullOrEmpty
    End Sub

    ''' <summary>
    ''' Tests rendering custom time
    ''' </summary>
    <TestMethod()> Public Sub TestRenderCustomTime()
        Dim TargetTime As New DateTime(2018, 2, 22, 5, 40, 37)
        RenderTime(TargetTime).ShouldNotBeNullOrEmpty
    End Sub

    ''' <summary>
    ''' Tests rendering custom time
    ''' </summary>
    <TestMethod()> Public Sub TestRenderCustomTimeType()
        Dim TargetTime As New DateTime(2018, 2, 22, 5, 40, 37)
        RenderTime(TargetTime, FormatType.Long).ShouldNotBeNullOrEmpty
        RenderTime(TargetTime, FormatType.Short).ShouldNotBeNullOrEmpty
    End Sub

    ''' <summary>
    ''' Tests rendering custom time with specified culture
    ''' </summary>
    <TestMethod()> Public Sub TestRenderCustomTimeCult()
        Dim TargetTime As New DateTime(2018, 2, 22, 5, 40, 37)
        Dim TargetCult As New CultureInfo("es-ES")
        RenderTime(TargetTime, TargetCult).ShouldNotBeNullOrEmpty
    End Sub

    ''' <summary>
    ''' Tests rendering custom time with specified culture
    ''' </summary>
    <TestMethod()> Public Sub TestRenderCustomTimeCultType()
        Dim TargetTime As New DateTime(2018, 2, 22, 5, 40, 37)
        Dim TargetCult As New CultureInfo("es-ES")
        RenderTime(TargetTime, TargetCult, FormatType.Long).ShouldNotBeNullOrEmpty
        RenderTime(TargetTime, TargetCult, FormatType.Short).ShouldNotBeNullOrEmpty
    End Sub

    ''' <summary>
    ''' Tests rendering kernel date
    ''' </summary>
    <TestMethod()> Public Sub TestRenderKernel()
        KernelDateTime = Date.Now
        Render.ShouldNotBeNullOrEmpty
    End Sub

    ''' <summary>
    ''' Tests rendering kernel date
    ''' </summary>
    <TestMethod()> Public Sub TestRenderKernelType()
        KernelDateTime = Date.Now
        Render(FormatType.Long).ShouldNotBeNullOrEmpty
        Render(FormatType.Short).ShouldNotBeNullOrEmpty
    End Sub

    ''' <summary>
    ''' Tests rendering kernel date with specified culture
    ''' </summary>
    <TestMethod()> Public Sub TestRenderKernelCult()
        KernelDateTime = Date.Now
        Dim TargetCult As New CultureInfo("es-ES")
        Render(TargetCult).ShouldNotBeNullOrEmpty
    End Sub

    ''' <summary>
    ''' Tests rendering kernel date with specified culture
    ''' </summary>
    <TestMethod()> Public Sub TestRenderKernelCultType()
        KernelDateTime = Date.Now
        Dim TargetCult As New CultureInfo("es-ES")
        Render(TargetCult, FormatType.Long).ShouldNotBeNullOrEmpty
        Render(TargetCult, FormatType.Short).ShouldNotBeNullOrEmpty
    End Sub

    ''' <summary>
    ''' Tests rendering custom date
    ''' </summary>
    <TestMethod()> Public Sub TestRenderCustom()
        Dim TargetDate As New DateTime(2018, 2, 22)
        Render(TargetDate).ShouldNotBeNullOrEmpty
    End Sub

    ''' <summary>
    ''' Tests rendering custom date
    ''' </summary>
    <TestMethod()> Public Sub TestRenderCustomType()
        Dim TargetDate As New DateTime(2018, 2, 22)
        Render(TargetDate, FormatType.Long).ShouldNotBeNullOrEmpty
        Render(TargetDate, FormatType.Short).ShouldNotBeNullOrEmpty
    End Sub

    ''' <summary>
    ''' Tests rendering custom date with specified culture
    ''' </summary>
    <TestMethod()> Public Sub TestRenderCustomCult()
        Dim TargetDate As New DateTime(2018, 2, 22)
        Dim TargetCult As New CultureInfo("es-ES")
        Render(TargetDate, TargetCult).ShouldNotBeNullOrEmpty
    End Sub

    ''' <summary>
    ''' Tests rendering custom date with specified culture
    ''' </summary>
    <TestMethod()> Public Sub TestRenderCustomCultType()
        Dim TargetDate As New DateTime(2018, 2, 22)
        Dim TargetCult As New CultureInfo("es-ES")
        Render(TargetDate, TargetCult, FormatType.Long).ShouldNotBeNullOrEmpty
        Render(TargetDate, TargetCult, FormatType.Short).ShouldNotBeNullOrEmpty
    End Sub

    ''' <summary>
    ''' Tests initializing current times in all timezones
    ''' </summary>
    <TestMethod()> Public Sub TestInitTimesInZones()
        KernelDateTime = Date.Now
        InitTimesInZones()
        zoneTimes.ShouldNotBeNull
        zoneTimes.ShouldNotBeEmpty
    End Sub

End Class