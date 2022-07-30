
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

Imports System.IO
Imports KS.Files.Querying
Imports KS.Misc.Configuration

<SetUpFixture> Public Class InitTest

    ''' <summary>
    ''' Initialize everything that is required before starting unit tests
    ''' </summary>
    <OneTimeSetUp> Public Shared Sub ReadyEverything()
        InitPaths()
        If Not FileExists(GetKernelPath(KernelPathType.Configuration)) Then
            CreateConfig()
        Else
            If Not FileExists(GetKernelPath(KernelPathType.Configuration) + ".old") Then File.Move(GetKernelPath(KernelPathType.Configuration), GetKernelPath(KernelPathType.Configuration) + ".old")
            CreateConfig()
        End If
        InitializeConfigToken()
        LoadUserToken()

        'NUnit sets current directory to a wrong directory, so set it to the test context directory
        Dim TestAssemblyDir As String = TestContext.CurrentContext.TestDirectory
        Environment.CurrentDirectory = TestAssemblyDir
    End Sub

    ''' <summary>
    ''' Clean up everything that the unit tests made
    ''' </summary>
    <OneTimeTearDown> Public Shared Sub CleanEverything()
        If FileExists(HomePath + "/Documents/TestText.txt") Then File.Delete(HomePath + "/Documents/TestText.txt")
        If FileExists(HomePath + "/Documents/Text.txt") Then File.Delete(HomePath + "/Documents/Text.txt")
        If FileExists(HomePath + "/NewFile.txt") Then File.Delete(HomePath + "/NewFile.txt")
        If FileExists(HomePath + "/NewFile.json") Then File.Delete(HomePath + "/NewFile.json")
        If FileExists(HomePath + "/1mb-test.csv") Then File.Delete(HomePath + "/1mb-test.csv")
        If FolderExists(HomePath + "/TestMovedDir2") Then Directory.Delete(HomePath + "/TestMovedDir2", True)
        If FolderExists(HomePath + "/NewDirectory") Then Directory.Delete(HomePath + "/NewDirectory", True)
        If FolderExists(HomePath + "/TestDir") Then Directory.Delete(HomePath + "/TestDir", True)
        If FolderExists(HomePath + "/TestDir2") Then Directory.Delete(HomePath + "/TestDir2", True)
        If FileExists(GetKernelPath(KernelPathType.Configuration) + ".old") Then
            If FileExists(GetKernelPath(KernelPathType.Configuration)) Then File.Delete(HomePath + "/KernelConfig.json")
            File.Move(GetKernelPath(KernelPathType.Configuration) + ".old", GetKernelPath(KernelPathType.Configuration))
        End If
    End Sub

End Class