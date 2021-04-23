
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
Imports System.Net.NetworkInformation

'Warning: Don't implement the unit tests related to downloading or uploading files. This causes AppVeyor to choke.
<TestClass()> Public Class NetworkActionTests

    ''' <summary>
    ''' Tests pinging
    ''' </summary>
    <TestMethod()> <TestCategory("Action")> Public Sub TestPingAddress()
        Assert.IsTrue(PingAddress("www.google.com").Status = IPStatus.Success, "Pinging failed.")
    End Sub

End Class