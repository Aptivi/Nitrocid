
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

Imports System.IO
Imports KS.ConsoleBase
Imports KS.Misc.Beautifiers
Imports Newtonsoft.Json
Imports Newtonsoft.Json.Linq

<TestFixture> Public Class JsonActionTests

    ''' <summary>
    ''' Tests beautifying the JSON text
    ''' </summary>
    <Test, Description("Action")> Public Sub TestBeautifyJsonText()
        Dim Beautified As String = BeautifyJsonText(JsonConvert.SerializeObject(ColorDataJson))
        Beautified.ShouldNotBeEmpty
        Beautified.ShouldBe(JsonConvert.SerializeObject(ColorDataJson, Formatting.Indented))
    End Sub

    ''' <summary>
    ''' Tests beautifying the JSON text
    ''' </summary>
    <Test, Description("Action")> Public Sub TestBeautifyJsonFile()
        Dim SourcePath As String = Path.GetFullPath("Hacker.json")
        Dim Beautified As String = BeautifyJson(SourcePath)
        Beautified.ShouldNotBeEmpty
        Beautified.ShouldBe(JsonConvert.SerializeObject(JToken.Parse(Beautified), Formatting.Indented))
    End Sub

    ''' <summary>
    ''' Tests minifying the JSON text
    ''' </summary>
    <Test, Description("Action")> Public Sub TestMinifyJsonText()
        Dim Minified As String = MinifyJsonText(JsonConvert.SerializeObject(ColorDataJson))
        Minified.ShouldNotBeEmpty
        Minified.ShouldBe(JsonConvert.SerializeObject(ColorDataJson, Formatting.None))
    End Sub

    ''' <summary>
    ''' Tests minifying the JSON text
    ''' </summary>
    <Test, Description("Action")> Public Sub TestMinifyJsonFile()
        Dim SourcePath As String = Path.GetFullPath("Hacker.json")
        Dim Minified As String = MinifyJson(SourcePath)
        Minified.ShouldNotBeEmpty
        Minified.ShouldBe(JsonConvert.SerializeObject(JToken.Parse(Minified), Formatting.None))
    End Sub

End Class