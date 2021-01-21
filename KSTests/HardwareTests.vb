
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

Imports InxiFrontend

<TestClass()> Public Class HardwareTests

    ''' <summary>
    ''' Tests hardware probation
    ''' </summary>
    <TestMethod()> Public Sub TestProbeHardware()
        Dim TempHardwareInfo As New Inxi
        Assert.IsFalse(TempHardwareInfo.Hardware.CPU Is Nothing Or (TempHardwareInfo.Hardware.CPU IsNot Nothing And TempHardwareInfo.Hardware.CPU.Count = 0), "CPU is not properly parsed.")
        Assert.IsFalse(TempHardwareInfo.Hardware.HDD Is Nothing Or (TempHardwareInfo.Hardware.HDD IsNot Nothing And TempHardwareInfo.Hardware.HDD.Count = 0), "HDD is not properly parsed.")
        Assert.IsFalse(TempHardwareInfo.Hardware.RAM Is Nothing, "RAM is not properly parsed.")
    End Sub

End Class