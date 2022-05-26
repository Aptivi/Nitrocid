
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

Imports KS.ConsoleBase

<TestFixture> Public Class ColorQueryingTests

    ''' <summary>
    ''' Tests trying to parse the color from hex
    ''' </summary>
    <Test, Description("Querying")> Public Sub TestTryParseColorFromHex()
        Debug.WriteLine("Trying #0F0F0F...")
        TryParseColor("#0F0F0F").ShouldBeTrue
        Debug.WriteLine("Trying #0G0G0G...")
        TryParseColor("#0G0G0G").ShouldBeFalse
    End Sub

    ''' <summary>
    ''' Tests trying to parse the color from color numbers
    ''' </summary>
    <Test, Description("Querying")> Public Sub TestTryParseColorFromColorNum()
        Debug.WriteLine("Trying colornum 26...")
        TryParseColor(26).ShouldBeTrue
        Debug.WriteLine("Trying colornum 260...")
        TryParseColor(260).ShouldBeFalse
        Debug.WriteLine("Trying colornum -26...")
        TryParseColor(-26).ShouldBeFalse
    End Sub

    ''' <summary>
    ''' Tests trying to parse the color from RGB
    ''' </summary>
    <Test, Description("Querying")> Public Sub TestTryParseColorFromRGB()
        Debug.WriteLine("Trying rgb 4, 4, 4...")
        TryParseColor(4, 4, 4).ShouldBeTrue
        Debug.WriteLine("Trying rgb 400, 4, 4...")
        TryParseColor(400, 4, 4).ShouldBeFalse
        Debug.WriteLine("Trying rgb 4, 400, 4...")
        TryParseColor(4, 400, 4).ShouldBeFalse
        Debug.WriteLine("Trying rgb 4, 4, 400...")
        TryParseColor(4, 4, 400).ShouldBeFalse
        Debug.WriteLine("Trying rgb 4, 400, 400...")
        TryParseColor(4, 400, 400).ShouldBeFalse
        Debug.WriteLine("Trying rgb 400, 4, 400...")
        TryParseColor(400, 4, 400).ShouldBeFalse
        Debug.WriteLine("Trying rgb 400, 400, 4...")
        TryParseColor(400, 400, 4).ShouldBeFalse
        Debug.WriteLine("Trying rgb 400, 400, 400...")
        TryParseColor(400, 400, 400).ShouldBeFalse
        Debug.WriteLine("Trying rgb -4, 4, 4...")
        TryParseColor(-4, 4, 4).ShouldBeFalse
        Debug.WriteLine("Trying rgb 4, -4, 4...")
        TryParseColor(4, -4, 4).ShouldBeFalse
        Debug.WriteLine("Trying rgb 4, 4, -4...")
        TryParseColor(4, 4, -4).ShouldBeFalse
        Debug.WriteLine("Trying rgb 4, -4, -4...")
        TryParseColor(4, -4, -4).ShouldBeFalse
        Debug.WriteLine("Trying rgb -4, -4, 4...")
        TryParseColor(-4, -4, 4).ShouldBeFalse
        Debug.WriteLine("Trying rgb -4, 4, -4...")
        TryParseColor(-4, 4, -4).ShouldBeFalse
        Debug.WriteLine("Trying rgb -4, -4, -4...")
        TryParseColor(-4, -4, -4).ShouldBeFalse
    End Sub

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