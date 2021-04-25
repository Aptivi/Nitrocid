
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

Imports System.IO
Imports KS

<TestClass()> Public Class ThemeInfoInitializationTests

    ''' <summary>
    ''' Tests initializing an instance of ThemeInfo from KS resources
    ''' </summary>
    <TestMethod()> <TestCategory("Initialization")> Public Sub TestInitializeThemeInfoFromResources()
        'Create instance
        Dim ThemeInfoInstance As New ThemeInfo("Hacker")

        'Check for null
        Assert.IsNotNull(ThemeInfoInstance.ThemeBackgroundColor)
        Assert.IsNotNull(ThemeInfoInstance.ThemeCmdDefColor)
        Assert.IsNotNull(ThemeInfoInstance.ThemeCmdListColor)
        Assert.IsNotNull(ThemeInfoInstance.ThemeContKernelErrorColor)
        Assert.IsNotNull(ThemeInfoInstance.ThemeErrorColor)
        Assert.IsNotNull(ThemeInfoInstance.ThemeHostNameShellColor)
        Assert.IsNotNull(ThemeInfoInstance.ThemeInputColor)
        Assert.IsNotNull(ThemeInfoInstance.ThemeLicenseColor)
        Assert.IsNotNull(ThemeInfoInstance.ThemeNeutralTextColor)
        Assert.IsNotNull(ThemeInfoInstance.ThemeOptionColor)
        Assert.IsNotNull(ThemeInfoInstance.ThemeStageColor)
        Assert.IsNotNull(ThemeInfoInstance.ThemeUncontKernelErrorColor)
        Assert.IsNotNull(ThemeInfoInstance.ThemeUserNameShellColor)
        Assert.IsNotNull(ThemeInfoInstance.ThemeWarningColor)
    End Sub

    ''' <summary>
    ''' Tests initializing an instance of ThemeInfo from file
    ''' </summary>
    <TestMethod()> <TestCategory("Initialization")> Public Sub TestInitializeThemeInfoFromFile()
        'Create instance
        Dim SourcePath As String = Path.GetFullPath("Hacker.json")
        Dim ThemeInfoInstance As New ThemeInfo(New StreamReader(SourcePath))

        'Check for null
        Assert.IsNotNull(ThemeInfoInstance.ThemeBackgroundColor)
        Assert.IsNotNull(ThemeInfoInstance.ThemeCmdDefColor)
        Assert.IsNotNull(ThemeInfoInstance.ThemeCmdListColor)
        Assert.IsNotNull(ThemeInfoInstance.ThemeContKernelErrorColor)
        Assert.IsNotNull(ThemeInfoInstance.ThemeErrorColor)
        Assert.IsNotNull(ThemeInfoInstance.ThemeHostNameShellColor)
        Assert.IsNotNull(ThemeInfoInstance.ThemeInputColor)
        Assert.IsNotNull(ThemeInfoInstance.ThemeLicenseColor)
        Assert.IsNotNull(ThemeInfoInstance.ThemeNeutralTextColor)
        Assert.IsNotNull(ThemeInfoInstance.ThemeOptionColor)
        Assert.IsNotNull(ThemeInfoInstance.ThemeStageColor)
        Assert.IsNotNull(ThemeInfoInstance.ThemeUncontKernelErrorColor)
        Assert.IsNotNull(ThemeInfoInstance.ThemeUserNameShellColor)
        Assert.IsNotNull(ThemeInfoInstance.ThemeWarningColor)
    End Sub

End Class