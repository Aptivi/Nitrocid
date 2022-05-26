
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

Imports KS.Misc.Configuration
Imports KS.Kernel.Flags
Imports Newtonsoft.Json.Linq

<TestFixture> Public Class FilesystemSettingTests

    ''' <summary>
    ''' Tests current directory setting
    ''' </summary>
    <Test, Description("Setting")> Public Sub TestSetCurrDir()
        CurrDir = HomePath
        Dim Path As String = HomePath + "/Documents"
        SetCurrDir(Path).ShouldBeTrue
        Path.ShouldBe(CurrDir)
    End Sub

    ''' <summary>
    ''' Tests setting size parse mode
    ''' </summary>
    <Test, Description("Setting")> Public Sub TestSetSizeParseMode()
        Dim Token As JToken = GetConfigCategory(ConfigCategory.Filesystem)
        SetSizeParseMode(True).ShouldBeTrue
        FullParseMode.ShouldBeTrue
        GetConfigValue(ConfigCategory.Filesystem, Token, "Size parse mode").ShouldBe(True)
        SetSizeParseMode(False).ShouldBeTrue
        FullParseMode.ShouldBeFalse
        GetConfigValue(ConfigCategory.Filesystem, Token, "Size parse mode").ShouldBe(False)
    End Sub

    ''' <summary>
    ''' Tests saving the current directory value
    ''' </summary>
    <Test, Description("Manipulation")> Public Sub TestSaveCurrDir()
        CurrDir = HomePath
        SaveCurrDir()
        GetConfigValue(ConfigCategory.Shell, GetConfigCategory(ConfigCategory.Shell), "Current Directory").ToString.ShouldBe(HomePath)
    End Sub

End Class