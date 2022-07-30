
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

Imports KS.Files.Folders
Imports KS.Misc.Configuration

<TestFixture> Public Class FilesystemSettingTests

    ''' <summary>
    ''' Tests current directory setting
    ''' </summary>
    <Test, Description("Setting")> Public Sub TestSetCurrDir()
        CurrentDir = HomePath
        Dim Path As String = HomePath + "/Documents"
        SetCurrDir(Path)
        Path.ShouldBe(CurrentDir)
    End Sub

    ''' <summary>
    ''' Tests current directory setting
    ''' </summary>
    <Test, Description("Setting")> Public Sub TestTrySetCurrDir()
        CurrentDir = HomePath
        Dim Path As String = HomePath + "/Documents"
        TrySetCurrDir(Path).ShouldBeTrue
        Path.ShouldBe(CurrentDir)
    End Sub

    ''' <summary>
    ''' Tests saving the current directory value
    ''' </summary>
    <Test, Description("Manipulation")> Public Sub TestSaveCurrDir()
        CurrentDir = HomePath
        SaveCurrDir()
        GetConfigValue(ConfigCategory.Shell, "Current Directory").ToString.ShouldBe(HomePath)
    End Sub

End Class
