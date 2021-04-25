
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
        Assert.IsNotNull(ColorInstance)
        Assert.IsNotNull(ColorInstance.PlainSequence)
        Assert.IsNotNull(ColorInstance.Type)
        Assert.IsNotNull(ColorInstance.VTSequenceBackground)
        Assert.IsNotNull(ColorInstance.VTSequenceForeground)

        'Check for property correctness
        Assert.AreEqual("13", ColorInstance.PlainSequence)
        Assert.AreEqual(ColorType._255Color, ColorInstance.Type)
        Assert.AreEqual(ChrW(&H1B) + "[48;5;13m", ColorInstance.VTSequenceBackground)
        Assert.AreEqual(ChrW(&H1B) + "[38;5;13m", ColorInstance.VTSequenceForeground)
    End Sub

    ''' <summary>
    ''' Tests initializing color instance from true color
    ''' </summary>
    <TestMethod()> <TestCategory("Initialization")> Public Sub TestInitializeColorInstanceFromTrueColor()
        'Create instance
        Dim ColorInstance As New Color("94;0;63")

        'Check for null
        Assert.IsNotNull(ColorInstance)
        Assert.IsNotNull(ColorInstance.PlainSequence)
        Assert.IsNotNull(ColorInstance.Type)
        Assert.IsNotNull(ColorInstance.VTSequenceBackground)
        Assert.IsNotNull(ColorInstance.VTSequenceForeground)

        'Check for property correctness
        Assert.AreEqual("94;0;63", ColorInstance.PlainSequence)
        Assert.AreEqual(ColorType.TrueColor, ColorInstance.Type)
        Assert.AreEqual(ChrW(&H1B) + "[48;2;94;0;63m", ColorInstance.VTSequenceBackground)
        Assert.AreEqual(ChrW(&H1B) + "[38;2;94;0;63m", ColorInstance.VTSequenceForeground)
    End Sub

End Class