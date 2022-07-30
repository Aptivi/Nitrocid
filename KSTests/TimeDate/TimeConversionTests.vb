
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

Imports KS.TimeDate

<TestFixture> Public Class TimeConversionTests

    ''' <summary>
    ''' Tests converting the date to Unix time (seconds since 1970/1/1)
    ''' </summary>
    <Test, Description("Conversion")> Public Sub TestDateToUnix()
        'Convert the target date (for example: September 20, 2014, 8:04:34 AM) to Unix
        Dim TargetDate As New Date(2014, 9, 20, 5, 4, 34, DateTimeKind.Utc)
        Dim ExpectedUnixTime As Double = 1411189474
        Dim UnixTime As Double = DateToUnix(TargetDate)
        UnixTime.ShouldBe(ExpectedUnixTime)
    End Sub

    ''' <summary>
    ''' Tests converting the Unix time (seconds since 1970/1/1) to date
    ''' </summary>
    <Test, Description("Conversion")> Public Sub TestUnixToDate()
        'Convert the target date (for example: September 20, 2014, 8:04:34 AM) to Unix
        Dim TargetUnixTime As Double = 1411189474
        Dim ExpectedDate As New Date(2014, 9, 20, 5, 4, 34, DateTimeKind.Utc)
        Dim ActualDate As Date = UnixToDate(TargetUnixTime)
        ActualDate.ShouldBe(ExpectedDate)
    End Sub

End Class