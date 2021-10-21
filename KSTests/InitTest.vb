
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

<TestClass()> Public Class InitTest

    ''' <summary>
    ''' Initialize everything that is required before starting unit tests
    ''' </summary>
    ''' <param name="Context">Test context</param>
    <AssemblyInitialize()> Public Shared Sub ReadyEverything(Context As TestContext)
        InitPaths()
        If Not FileExists(GetKernelPath(KernelPathType.Configuration)) Then
            CreateConfig()
        Else
            If Not FileExists(GetKernelPath(KernelPathType.Configuration) + ".old") Then File.Move(GetKernelPath(KernelPathType.Configuration), GetKernelPath(KernelPathType.Configuration) + ".old")
            CreateConfig()
        End If
        InitializeConfigToken()
        LoadUserToken()
    End Sub

    ''' <summary>
    ''' Clean up everything that the unit tests made
    ''' </summary>
    <AssemblyCleanup()> Public Shared Sub CleanEverything()
        If FileExists(GetOtherPath(OtherPathType.Home) + "/Documents/TestText.txt") Then File.Delete(GetOtherPath(OtherPathType.Home) + "/Documents/TestText.txt")
        If FileExists(GetOtherPath(OtherPathType.Home) + "/Documents/Text.txt") Then File.Delete(GetOtherPath(OtherPathType.Home) + "/Documents/Text.txt")
        If FileExists(GetOtherPath(OtherPathType.Home) + "/NewFile.txt") Then File.Delete(GetOtherPath(OtherPathType.Home) + "/NewFile.txt")
        If FileExists(GetOtherPath(OtherPathType.Home) + "/1mb-test.csv") Then File.Delete(GetOtherPath(OtherPathType.Home) + "/1mb-test.csv")
        If FolderExists(GetOtherPath(OtherPathType.Home) + "/TestMovedDir2") Then Directory.Delete(GetOtherPath(OtherPathType.Home) + "/TestMovedDir2", True)
        If FolderExists(GetOtherPath(OtherPathType.Home) + "/NewDirectory") Then Directory.Delete(GetOtherPath(OtherPathType.Home) + "/NewDirectory", True)
        If FolderExists(GetOtherPath(OtherPathType.Home) + "/TestDir") Then Directory.Delete(GetOtherPath(OtherPathType.Home) + "/TestDir", True)
        If FolderExists(GetOtherPath(OtherPathType.Home) + "/TestDir2") Then Directory.Delete(GetOtherPath(OtherPathType.Home) + "/TestDir2", True)
        If FileExists(GetKernelPath(KernelPathType.Configuration) + ".old") Then
            If FileExists(GetKernelPath(KernelPathType.Configuration)) Then File.Delete(GetOtherPath(OtherPathType.Home) + "/KernelConfig.json")
            File.Move(GetKernelPath(KernelPathType.Configuration) + ".old", GetKernelPath(KernelPathType.Configuration))
        End If
    End Sub

End Class