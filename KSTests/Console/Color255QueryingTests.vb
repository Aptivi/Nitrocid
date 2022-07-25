
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

Imports KS.ConsoleBase.Colors
Imports Newtonsoft.Json.Linq

<TestFixture> Public Class Color255QueryingTests

    ''' <summary>
    ''' Tests querying 255-color data from JSON (parses only needed data by KS)
    ''' </summary>
    <Test, Description("Querying")> Public Sub TestQueryColorDataFromJson()
        For ColorIndex As Integer = 0 To 255
            Dim ColorData As JObject = ColorDataJson(ColorIndex)
            ColorData("colorId").ToString().ShouldBe(ColorIndex)
            Integer.TryParse(ColorData("rgb")("r").ToString(), 0).ShouldBeTrue
            Integer.TryParse(ColorData("rgb")("g").ToString(), 0).ShouldBeTrue
            Integer.TryParse(ColorData("rgb")("b").ToString(), 0).ShouldBeTrue
        Next
    End Sub

    ''' <summary>
    ''' Tests getting an escape character
    ''' </summary>
    <Test, Description("Querying")> Public Sub TestGetEsc()
        GetEsc.ShouldBe(Convert.ToChar(&H1B))
    End Sub

End Class