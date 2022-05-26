
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

Imports System.Globalization
Imports KS.TimeDate

<TestFixture> Public Class TimeActionTests

    ''' <summary>
    ''' Tests rendering kernel date
    ''' </summary>
    <Test, Description("Action")> Public Sub TestRenderKernelDate()
        KernelDateTime = Date.Now
        RenderDate.ShouldNotBeNullOrEmpty
    End Sub

    ''' <summary>
    ''' Tests rendering kernel date with specific format type
    ''' </summary>
    <Test, Description("Action")> Public Sub TestRenderKernelDateType()
        KernelDateTime = Date.Now
        RenderDate(FormatType.Long).ShouldNotBeNullOrEmpty
        RenderDate(FormatType.Short).ShouldNotBeNullOrEmpty
    End Sub

    ''' <summary>
    ''' Tests rendering kernel date with specified culture
    ''' </summary>
    <Test, Description("Action")> Public Sub TestRenderKernelDateCult()
        KernelDateTime = Date.Now
        Dim TargetCult As New CultureInfo("es-ES")
        RenderDate(TargetCult).ShouldNotBeNullOrEmpty
    End Sub

    ''' <summary>
    ''' Tests rendering kernel date with specified culture and format type
    ''' </summary>
    <Test, Description("Action")> Public Sub TestRenderKernelDateCultType()
        KernelDateTime = Date.Now
        Dim TargetCult As New CultureInfo("es-ES")
        RenderDate(TargetCult, FormatType.Long).ShouldNotBeNullOrEmpty
        RenderDate(TargetCult, FormatType.Short).ShouldNotBeNullOrEmpty
    End Sub

    ''' <summary>
    ''' Tests rendering kernel date (UTC)
    ''' </summary>
    <Test, Description("Action")> Public Sub TestRenderKernelDateUtc()
        KernelDateTimeUtc = Date.UtcNow
        RenderDateUtc.ShouldNotBeNullOrEmpty
    End Sub

    ''' <summary>
    ''' Tests rendering kernel date with specific format type (UTC)
    ''' </summary>
    <Test, Description("Action")> Public Sub TestRenderKernelDateUtcType()
        KernelDateTimeUtc = Date.UtcNow
        RenderDateUtc(FormatType.Long).ShouldNotBeNullOrEmpty
        RenderDateUtc(FormatType.Short).ShouldNotBeNullOrEmpty
    End Sub

    ''' <summary>
    ''' Tests rendering kernel date with specified culture (UTC)
    ''' </summary>
    <Test, Description("Action")> Public Sub TestRenderKernelDateUtcCult()
        KernelDateTimeUtc = Date.UtcNow
        Dim TargetCult As New CultureInfo("es-ES")
        RenderDateUtc(TargetCult).ShouldNotBeNullOrEmpty
    End Sub

    ''' <summary>
    ''' Tests rendering kernel date with specified culture and format type (UTC)
    ''' </summary>
    <Test, Description("Action")> Public Sub TestRenderKernelDateUtcCultType()
        KernelDateTimeUtc = Date.UtcNow
        Dim TargetCult As New CultureInfo("es-ES")
        RenderDateUtc(TargetCult, FormatType.Long).ShouldNotBeNullOrEmpty
        RenderDateUtc(TargetCult, FormatType.Short).ShouldNotBeNullOrEmpty
    End Sub

    ''' <summary>
    ''' Tests rendering custom date
    ''' </summary>
    <Test, Description("Action")> Public Sub TestRenderCustomDate()
        Dim TargetDate As New DateTime(2018, 2, 22)
        RenderDate(TargetDate).ShouldNotBeNullOrEmpty
    End Sub

    ''' <summary>
    ''' Tests rendering custom date
    ''' </summary>
    <Test, Description("Action")> Public Sub TestRenderCustomDateType()
        Dim TargetDate As New DateTime(2018, 2, 22)
        RenderDate(TargetDate, FormatType.Long).ShouldNotBeNullOrEmpty
        RenderDate(TargetDate, FormatType.Short).ShouldNotBeNullOrEmpty
    End Sub

    ''' <summary>
    ''' Tests rendering custom date with specified culture
    ''' </summary>
    <Test, Description("Action")> Public Sub TestRenderCustomDateCult()
        Dim TargetDate As New DateTime(2018, 2, 22)
        Dim TargetCult As New CultureInfo("es-ES")
        RenderDate(TargetDate, TargetCult).ShouldNotBeNullOrEmpty
    End Sub

    ''' <summary>
    ''' Tests rendering custom date with specified culture
    ''' </summary>
    <Test, Description("Action")> Public Sub TestRenderCustomDateCultType()
        Dim TargetDate As New DateTime(2018, 2, 22)
        Dim TargetCult As New CultureInfo("es-ES")
        RenderDate(TargetDate, TargetCult, FormatType.Long).ShouldNotBeNullOrEmpty
        RenderDate(TargetDate, TargetCult, FormatType.Short).ShouldNotBeNullOrEmpty
    End Sub

    ''' <summary>
    ''' Tests rendering custom date (UTC)
    ''' </summary>
    <Test, Description("Action")> Public Sub TestRenderCustomDateUtc()
        Dim TargetDate As New DateTime(2018, 2, 22)
        RenderDateUtc(TargetDate).ShouldNotBeNullOrEmpty
    End Sub

    ''' <summary>
    ''' Tests rendering custom date (UTC)
    ''' </summary>
    <Test, Description("Action")> Public Sub TestRenderCustomDateUtcType()
        Dim TargetDate As New DateTime(2018, 2, 22)
        RenderDateUtc(TargetDate, FormatType.Long).ShouldNotBeNullOrEmpty
        RenderDateUtc(TargetDate, FormatType.Short).ShouldNotBeNullOrEmpty
    End Sub

    ''' <summary>
    ''' Tests rendering custom date with specified culture (UTC)
    ''' </summary>
    <Test, Description("Action")> Public Sub TestRenderCustomDateUtcCult()
        Dim TargetDate As New DateTime(2018, 2, 22)
        Dim TargetCult As New CultureInfo("es-ES")
        RenderDateUtc(TargetDate, TargetCult).ShouldNotBeNullOrEmpty
    End Sub

    ''' <summary>
    ''' Tests rendering custom date with specified culture (UTC)
    ''' </summary>
    <Test, Description("Action")> Public Sub TestRenderCustomDateUtcCultType()
        Dim TargetDate As New DateTime(2018, 2, 22)
        Dim TargetCult As New CultureInfo("es-ES")
        RenderDateUtc(TargetDate, TargetCult, FormatType.Long).ShouldNotBeNullOrEmpty
        RenderDateUtc(TargetDate, TargetCult, FormatType.Short).ShouldNotBeNullOrEmpty
    End Sub

    ''' <summary>
    ''' Tests rendering kernel time
    ''' </summary>
    <Test, Description("Action")> Public Sub TestRenderKernelTime()
        KernelDateTime = Date.Now
        RenderTime.ShouldNotBeNullOrEmpty
    End Sub

    ''' <summary>
    ''' Tests rendering kernel time
    ''' </summary>
    <Test, Description("Action")> Public Sub TestRenderKernelTimeType()
        KernelDateTime = Date.Now
        RenderTime(FormatType.Long).ShouldNotBeNullOrEmpty
        RenderTime(FormatType.Short).ShouldNotBeNullOrEmpty
    End Sub

    ''' <summary>
    ''' Tests rendering kernel time with specified culture
    ''' </summary>
    <Test, Description("Action")> Public Sub TestRenderKernelTimeCult()
        KernelDateTime = Date.Now
        Dim TargetCult As New CultureInfo("es-ES")
        RenderTime(TargetCult).ShouldNotBeNullOrEmpty
    End Sub

    ''' <summary>
    ''' Tests rendering kernel time with specified culture
    ''' </summary>
    <Test, Description("Action")> Public Sub TestRenderKernelTimeCultType()
        KernelDateTime = Date.Now
        Dim TargetCult As New CultureInfo("es-ES")
        RenderTime(TargetCult, FormatType.Long).ShouldNotBeNullOrEmpty
        RenderTime(TargetCult, FormatType.Short).ShouldNotBeNullOrEmpty
    End Sub

    ''' <summary>
    ''' Tests rendering kernel time (UTC)
    ''' </summary>
    <Test, Description("Action")> Public Sub TestRenderKernelTimeUtc()
        KernelDateTimeUtc = Date.UtcNow
        RenderTimeUtc.ShouldNotBeNullOrEmpty
    End Sub

    ''' <summary>
    ''' Tests rendering kernel time with specific format type (UTC)
    ''' </summary>
    <Test, Description("Action")> Public Sub TestRenderKernelTimeUtcType()
        KernelDateTimeUtc = Date.UtcNow
        RenderTimeUtc(FormatType.Long).ShouldNotBeNullOrEmpty
        RenderTimeUtc(FormatType.Short).ShouldNotBeNullOrEmpty
    End Sub

    ''' <summary>
    ''' Tests rendering kernel time with specified culture (UTC)
    ''' </summary>
    <Test, Description("Action")> Public Sub TestRenderKernelTimeUtcCult()
        KernelDateTimeUtc = Date.UtcNow
        Dim TargetCult As New CultureInfo("es-ES")
        RenderTimeUtc(TargetCult).ShouldNotBeNullOrEmpty
    End Sub

    ''' <summary>
    ''' Tests rendering kernel time with specified culture and format type (UTC)
    ''' </summary>
    <Test, Description("Action")> Public Sub TestRenderKernelTimeUtcCultType()
        KernelDateTimeUtc = Date.UtcNow
        Dim TargetCult As New CultureInfo("es-ES")
        RenderTimeUtc(TargetCult, FormatType.Long).ShouldNotBeNullOrEmpty
        RenderTimeUtc(TargetCult, FormatType.Short).ShouldNotBeNullOrEmpty
    End Sub

    ''' <summary>
    ''' Tests rendering custom time
    ''' </summary>
    <Test, Description("Action")> Public Sub TestRenderCustomTime()
        Dim TargetTime As New DateTime(2018, 2, 22, 5, 40, 37)
        RenderTime(TargetTime).ShouldNotBeNullOrEmpty
    End Sub

    ''' <summary>
    ''' Tests rendering custom time
    ''' </summary>
    <Test, Description("Action")> Public Sub TestRenderCustomTimeType()
        Dim TargetTime As New DateTime(2018, 2, 22, 5, 40, 37)
        RenderTime(TargetTime, FormatType.Long).ShouldNotBeNullOrEmpty
        RenderTime(TargetTime, FormatType.Short).ShouldNotBeNullOrEmpty
    End Sub

    ''' <summary>
    ''' Tests rendering custom time with specified culture
    ''' </summary>
    <Test, Description("Action")> Public Sub TestRenderCustomTimeCult()
        Dim TargetTime As New DateTime(2018, 2, 22, 5, 40, 37)
        Dim TargetCult As New CultureInfo("es-ES")
        RenderTime(TargetTime, TargetCult).ShouldNotBeNullOrEmpty
    End Sub

    ''' <summary>
    ''' Tests rendering custom time with specified culture
    ''' </summary>
    <Test, Description("Action")> Public Sub TestRenderCustomTimeCultType()
        Dim TargetTime As New DateTime(2018, 2, 22, 5, 40, 37)
        Dim TargetCult As New CultureInfo("es-ES")
        RenderTime(TargetTime, TargetCult, FormatType.Long).ShouldNotBeNullOrEmpty
        RenderTime(TargetTime, TargetCult, FormatType.Short).ShouldNotBeNullOrEmpty
    End Sub

    ''' <summary>
    ''' Tests rendering custom time (UTC)
    ''' </summary>
    <Test, Description("Action")> Public Sub TestRenderCustomTimeUtc()
        Dim TargetTime As New DateTime(2018, 2, 22, 5, 40, 37)
        RenderTimeUtc(TargetTime).ShouldNotBeNullOrEmpty
    End Sub

    ''' <summary>
    ''' Tests rendering custom time (UTC)
    ''' </summary>
    <Test, Description("Action")> Public Sub TestRenderCustomTimeUtcType()
        Dim TargetTime As New DateTime(2018, 2, 22, 5, 40, 37)
        RenderTimeUtc(TargetTime, FormatType.Long).ShouldNotBeNullOrEmpty
        RenderTimeUtc(TargetTime, FormatType.Short).ShouldNotBeNullOrEmpty
    End Sub

    ''' <summary>
    ''' Tests rendering custom time with specified culture (UTC)
    ''' </summary>
    <Test, Description("Action")> Public Sub TestRenderCustomTimeUtcCult()
        Dim TargetTime As New DateTime(2018, 2, 22, 5, 40, 37)
        Dim TargetCult As New CultureInfo("es-ES")
        RenderTimeUtc(TargetTime, TargetCult).ShouldNotBeNullOrEmpty
    End Sub

    ''' <summary>
    ''' Tests rendering custom time with specified culture (UTC)
    ''' </summary>
    <Test, Description("Action")> Public Sub TestRenderCustomTimeUtcCultType()
        Dim TargetTime As New DateTime(2018, 2, 22, 5, 40, 37)
        Dim TargetCult As New CultureInfo("es-ES")
        RenderTimeUtc(TargetTime, TargetCult, FormatType.Long).ShouldNotBeNullOrEmpty
        RenderTimeUtc(TargetTime, TargetCult, FormatType.Short).ShouldNotBeNullOrEmpty
    End Sub

    ''' <summary>
    ''' Tests rendering kernel date
    ''' </summary>
    <Test, Description("Action")> Public Sub TestRenderKernel()
        KernelDateTime = Date.Now
        Render.ShouldNotBeNullOrEmpty
    End Sub

    ''' <summary>
    ''' Tests rendering kernel date
    ''' </summary>
    <Test, Description("Action")> Public Sub TestRenderKernelType()
        KernelDateTime = Date.Now
        Render(FormatType.Long).ShouldNotBeNullOrEmpty
        Render(FormatType.Short).ShouldNotBeNullOrEmpty
    End Sub

    ''' <summary>
    ''' Tests rendering kernel date with specified culture
    ''' </summary>
    <Test, Description("Action")> Public Sub TestRenderKernelCult()
        KernelDateTime = Date.Now
        Dim TargetCult As New CultureInfo("es-ES")
        Render(TargetCult).ShouldNotBeNullOrEmpty
    End Sub

    ''' <summary>
    ''' Tests rendering kernel date with specified culture
    ''' </summary>
    <Test, Description("Action")> Public Sub TestRenderKernelCultType()
        KernelDateTime = Date.Now
        Dim TargetCult As New CultureInfo("es-ES")
        Render(TargetCult, FormatType.Long).ShouldNotBeNullOrEmpty
        Render(TargetCult, FormatType.Short).ShouldNotBeNullOrEmpty
    End Sub

    ''' <summary>
    ''' Tests rendering kernel date (UTC)
    ''' </summary>
    <Test, Description("Action")> Public Sub TestRenderKernelUtc()
        KernelDateTimeUtc = Date.UtcNow
        RenderUtc.ShouldNotBeNullOrEmpty
    End Sub

    ''' <summary>
    ''' Tests rendering kernel date with specific format type (UTC)
    ''' </summary>
    <Test, Description("Action")> Public Sub TestRenderKernelUtcType()
        KernelDateTimeUtc = Date.UtcNow
        RenderUtc(FormatType.Long).ShouldNotBeNullOrEmpty
        RenderUtc(FormatType.Short).ShouldNotBeNullOrEmpty
    End Sub

    ''' <summary>
    ''' Tests rendering kernel date with specified culture (UTC)
    ''' </summary>
    <Test, Description("Action")> Public Sub TestRenderKernelUtcCult()
        KernelDateTimeUtc = Date.UtcNow
        Dim TargetCult As New CultureInfo("es-ES")
        RenderUtc(TargetCult).ShouldNotBeNullOrEmpty
    End Sub

    ''' <summary>
    ''' Tests rendering kernel date with specified culture and format type (UTC)
    ''' </summary>
    <Test, Description("Action")> Public Sub TestRenderKernelUtcCultType()
        KernelDateTimeUtc = Date.UtcNow
        Dim TargetCult As New CultureInfo("es-ES")
        RenderUtc(TargetCult, FormatType.Long).ShouldNotBeNullOrEmpty
        RenderUtc(TargetCult, FormatType.Short).ShouldNotBeNullOrEmpty
    End Sub

    ''' <summary>
    ''' Tests rendering custom date
    ''' </summary>
    <Test, Description("Action")> Public Sub TestRenderCustom()
        Dim TargetDate As New DateTime(2018, 2, 22)
        Render(TargetDate).ShouldNotBeNullOrEmpty
    End Sub

    ''' <summary>
    ''' Tests rendering custom date
    ''' </summary>
    <Test, Description("Action")> Public Sub TestRenderCustomType()
        Dim TargetDate As New DateTime(2018, 2, 22)
        Render(TargetDate, FormatType.Long).ShouldNotBeNullOrEmpty
        Render(TargetDate, FormatType.Short).ShouldNotBeNullOrEmpty
    End Sub

    ''' <summary>
    ''' Tests rendering custom date with specified culture
    ''' </summary>
    <Test, Description("Action")> Public Sub TestRenderCustomCult()
        Dim TargetDate As New DateTime(2018, 2, 22)
        Dim TargetCult As New CultureInfo("es-ES")
        Render(TargetDate, TargetCult).ShouldNotBeNullOrEmpty
    End Sub

    ''' <summary>
    ''' Tests rendering custom date with specified culture
    ''' </summary>
    <Test, Description("Action")> Public Sub TestRenderCustomCultType()
        Dim TargetDate As New DateTime(2018, 2, 22)
        Dim TargetCult As New CultureInfo("es-ES")
        Render(TargetDate, TargetCult, FormatType.Long).ShouldNotBeNullOrEmpty
        Render(TargetDate, TargetCult, FormatType.Short).ShouldNotBeNullOrEmpty
    End Sub

    ''' <summary>
    ''' Tests rendering custom date (UTC)
    ''' </summary>
    <Test, Description("Action")> Public Sub TestRenderCustomUtc()
        Dim TargetDate As New DateTime(2018, 2, 22)
        RenderUtc(TargetDate).ShouldNotBeNullOrEmpty
    End Sub

    ''' <summary>
    ''' Tests rendering custom date (UTC)
    ''' </summary>
    <Test, Description("Action")> Public Sub TestRenderCustomUtcType()
        Dim TargetDate As New DateTime(2018, 2, 22)
        RenderUtc(TargetDate, FormatType.Long).ShouldNotBeNullOrEmpty
        RenderUtc(TargetDate, FormatType.Short).ShouldNotBeNullOrEmpty
    End Sub

    ''' <summary>
    ''' Tests rendering custom date with specified culture (UTC)
    ''' </summary>
    <Test, Description("Action")> Public Sub TestRenderCustomUtcCult()
        Dim TargetDate As New DateTime(2018, 2, 22)
        Dim TargetCult As New CultureInfo("es-ES")
        RenderUtc(TargetDate, TargetCult).ShouldNotBeNullOrEmpty
    End Sub

    ''' <summary>
    ''' Tests rendering custom date with specified culture (UTC)
    ''' </summary>
    <Test, Description("Action")> Public Sub TestRenderCustomUtcCultType()
        Dim TargetDate As New DateTime(2018, 2, 22)
        Dim TargetCult As New CultureInfo("es-ES")
        RenderUtc(TargetDate, TargetCult, FormatType.Long).ShouldNotBeNullOrEmpty
        RenderUtc(TargetDate, TargetCult, FormatType.Short).ShouldNotBeNullOrEmpty
    End Sub

End Class