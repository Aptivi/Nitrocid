
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

<TestClass()> Public Class ColorInitializationTests

    ''' <summary>
    ''' Tests initializing color instance from 255 colors
    ''' </summary>
    <TestMethod()> <TestCategory("Initialization")> Public Sub TestInitializeColorInstanceFrom255Colors()
        'Create instance
        Dim ColorInstance As New Color(13)

        'Check for null
        ColorInstance.ShouldNotBeNull
        ColorInstance.PlainSequence.ShouldNotBeNullOrEmpty
        ColorInstance.VTSequenceBackground.ShouldNotBeNullOrEmpty
        ColorInstance.VTSequenceForeground.ShouldNotBeNullOrEmpty

        'Check for property correctness
        ColorInstance.PlainSequence.ShouldBe("13")
        ColorInstance.Type.ShouldBe(ColorType._255Color)
        ColorInstance.VTSequenceBackground.ShouldBe(ChrW(&H1B) + "[48;5;13m")
        ColorInstance.VTSequenceForeground.ShouldBe(ChrW(&H1B) + "[38;5;13m")
        ColorInstance.R.ShouldBe(255)
        ColorInstance.G.ShouldBe(0)
        ColorInstance.B.ShouldBe(255)
        ColorInstance.IsDark.ShouldBeTrue
    End Sub

    ''' <summary>
    ''' Tests initializing color instance from true color
    ''' </summary>
    <TestMethod()> <TestCategory("Initialization")> Public Sub TestInitializeColorInstanceFromTrueColor()
        'Create instance
        Dim ColorInstance As New Color("94;0;63")

        'Check for null
        ColorInstance.ShouldNotBeNull
        ColorInstance.PlainSequence.ShouldNotBeNullOrEmpty
        ColorInstance.VTSequenceBackground.ShouldNotBeNullOrEmpty
        ColorInstance.VTSequenceForeground.ShouldNotBeNullOrEmpty

        'Check for property correctness
        ColorInstance.PlainSequence.ShouldBe("94;0;63")
        ColorInstance.Type.ShouldBe(ColorType.TrueColor)
        ColorInstance.VTSequenceBackground.ShouldBe(ChrW(&H1B) + "[48;2;94;0;63m")
        ColorInstance.VTSequenceForeground.ShouldBe(ChrW(&H1B) + "[38;2;94;0;63m")
        ColorInstance.R.ShouldBe(94)
        ColorInstance.G.ShouldBe(0)
        ColorInstance.B.ShouldBe(63)
        ColorInstance.IsDark.ShouldBeTrue
    End Sub

End Class