
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

Imports KS

<TestClass()> Public Class RGBInitializationTests

    ''' <summary>
    ''' Tests initializing the RGB instance from color levels (R, G, B)
    ''' </summary>
    <TestMethod()> <TestCategory("Initialization")> Public Sub TestInitializeRGBInstanceFromLevels()
        'Create instance
        Dim RGBInstance As New RGB(94, 0, 63)

        'Check for null
        RGBInstance.ShouldNotBeNull

        'Check for correctness
        RGBInstance.Red.ShouldBe(94)
        RGBInstance.Green.ShouldBe(0)
        RGBInstance.Blue.ShouldBe(63)
    End Sub

    ''' <summary>
    ''' Tests initializing the RGB instance from color string (RRR;GGG;BBB)
    ''' </summary>
    <TestMethod()> <TestCategory("Initialization")> Public Sub TestInitializeRGBInstanceFromString()
        'Create instance
        Dim RGBInstance As New RGB("94;0;63")

        'Check for null
        RGBInstance.ShouldNotBeNull

        'Check for correctness
        RGBInstance.Red.ShouldBe(94)
        RGBInstance.Green.ShouldBe(0)
        RGBInstance.Blue.ShouldBe(63)
    End Sub

    ''' <summary>
    ''' Tests initializing the Color instance from an RGB instance
    ''' </summary>
    <TestMethod()> <TestCategory("Initialization")> Public Sub TestMakeInstanceOfColorFromRGB()
        'Create instance
        Dim RGBInstance As New RGB("94;0;63")

        'Try to create instance
        Dim ColorInstance As New Color(RGBInstance.ToString)

        'Check for null
        ColorInstance.ShouldNotBeNull
        ColorInstance.PlainSequence.ShouldNotBeNullOrEmpty
        ColorInstance.VTSequenceBackground.ShouldNotBeNullOrEmpty
        ColorInstance.VTSequenceForeground.ShouldNotBeNullOrEmpty

        'Check for property correctness
        ColorInstance.PlainSequence.ShouldBe("94;0;63")
        ColorInstance.Type.ShouldBe(ColorType.TrueColor)
        ColorInstance.VTSequenceBackground.ShouldBe(GetEsc() + "[48;2;94;0;63m")
        ColorInstance.VTSequenceForeground.ShouldBe(GetEsc() + "[38;2;94;0;63m")
        ColorInstance.R.ShouldBe(94)
        ColorInstance.G.ShouldBe(0)
        ColorInstance.B.ShouldBe(63)
        ColorInstance.IsBright.ShouldBeTrue
    End Sub

End Class