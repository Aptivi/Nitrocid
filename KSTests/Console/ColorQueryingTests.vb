
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

Imports KS.ConsoleBase.Colors

<TestFixture> Public Class ColorQueryingTests

    ''' <summary>
    ''' Tests trying to parse the color from hex
    ''' </summary>
    <TestCase("#0F0F0F", ExpectedResult:=True),
     TestCase("#0G0G0G", ExpectedResult:=False),
     Description("Querying")>
    Public Function TestTryParseColorFromHex(TargetHex As String) As Boolean
        Debug.WriteLine($"Trying {TargetHex}...")
        Return TryParseColor(TargetHex)
    End Function

    ''' <summary>
    ''' Tests trying to parse the color from color numbers
    ''' </summary>
    <TestCase(26, ExpectedResult:=True),
     TestCase(260, ExpectedResult:=False),
     TestCase(-26, ExpectedResult:=False),
     Description("Querying")>
    Public Function TestTryParseColorFromColorNum(TargetColorNum As Integer) As Boolean
        Debug.WriteLine($"Trying colornum {TargetColorNum}...")
        Return TryParseColor(TargetColorNum)
    End Function

    ''' <summary>
    ''' Tests trying to parse the color from RGB
    ''' </summary>
    <TestCase(4, 4, 4, ExpectedResult:=True),
     TestCase(400, 4, 4, ExpectedResult:=False),
     TestCase(4, 400, 4, ExpectedResult:=False),
     TestCase(4, 4, 400, ExpectedResult:=False),
     TestCase(4, 400, 400, ExpectedResult:=False),
     TestCase(400, 4, 400, ExpectedResult:=False),
     TestCase(400, 400, 4, ExpectedResult:=False),
     TestCase(400, 400, 400, ExpectedResult:=False),
     TestCase(-4, 4, 4, ExpectedResult:=False),
     TestCase(4, -4, 4, ExpectedResult:=False),
     TestCase(4, 4, -4, ExpectedResult:=False),
     TestCase(4, -4, -4, ExpectedResult:=False),
     TestCase(-4, 4, -4, ExpectedResult:=False),
     TestCase(-4, -4, 4, ExpectedResult:=False),
     TestCase(-4, -4, -4, ExpectedResult:=False),
     Description("Querying")>
    Public Function TestTryParseColorFromRGB(R As Integer, G As Integer, B As Integer) As Boolean
        Debug.WriteLine($"Trying rgb {R}, {G}, {B}...")
        Return TryParseColor(R, G, B)
    End Function

    ''' <summary>
    ''' Tests trying to convert from hex to RGB
    ''' </summary>
    <Test, Description("Querying")> Public Sub TestConvertFromHexToRGB()
        Debug.WriteLine("Converting #0F0F0F...")
        ConvertFromHexToRGB("#0F0F0F").ShouldBe("15;15;15")
    End Sub

    ''' <summary>
    ''' Tests trying to convert from RGB sequence to hex
    ''' </summary>
    <Test, Description("Querying")> Public Sub TestConvertFromRGBSequenceToHex()
        Debug.WriteLine("Converting 15;15;15...")
        ConvertFromRGBToHex("15;15;15").ShouldBe("#0F0F0F")
    End Sub

    ''' <summary>
    ''' Tests trying to convert from RGB numbers to hex
    ''' </summary>
    <Test, Description("Querying")> Public Sub TestConvertFromRGBNumbersToHex()
        Debug.WriteLine("Converting 15, 15, 15...")
        ConvertFromRGBToHex(15, 15, 15).ShouldBe("#0F0F0F")
    End Sub

End Class