
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

<TestClass()> Public Class HelpInitializationTests

    ''' <summary>
    ''' Tests initialization of FTP help
    ''' </summary>
    <TestMethod()> <TestCategory("Initialization")> Public Sub TestInitFTPHelp()
        InitFTPHelp()
        FTPDefinitions.ShouldNotBeNull
        FTPDefinitions.ShouldNotBeEmpty
    End Sub

    ''' <summary>
    ''' Tests initialization of mail help
    ''' </summary>
    <TestMethod()> <TestCategory("Initialization")> Public Sub TestInitMailHelp()
        IMAPInitHelp()
        MailDefinitions.ShouldNotBeNull
        MailDefinitions.ShouldNotBeEmpty
    End Sub

    ''' <summary>
    ''' Tests initialization of shell help
    ''' </summary>
    <TestMethod()> <TestCategory("Initialization")> Public Sub TestInitShellHelp()
        InitHelp()
        definitions.ShouldNotBeNull
        definitions.ShouldNotBeEmpty
    End Sub

    ''' <summary>
    ''' Tests initialization of text help
    ''' </summary>
    <TestMethod()> <TestCategory("Initialization")> Public Sub TestInitTextHelp()
        TextEdit_UpdateHelp()
        TextEdit_HelpEntries.ShouldNotBeNull
        TextEdit_HelpEntries.ShouldNotBeEmpty
    End Sub

    ''' <summary>
    ''' Tests initialization of ZIP help
    ''' </summary>
    <TestMethod()> <TestCategory("Initialization")> Public Sub TestInitZipHelp()
        ZipShell_UpdateHelp()
        ZipShell_HelpEntries.ShouldNotBeNull
        ZipShell_HelpEntries.ShouldNotBeEmpty
    End Sub

    ''' <summary>
    ''' Tests initialization of mail help
    ''' </summary>
    <TestMethod()> <TestCategory("Initialization")> Public Sub TestInitSFTPHelp()
        InitSFTPHelp()
        SFTPDefinitions.ShouldNotBeNull
        SFTPDefinitions.ShouldNotBeEmpty
    End Sub

    ''' <summary>
    ''' Tests initialization of shell help
    ''' </summary>
    <TestMethod()> <TestCategory("Initialization")> Public Sub TestInitRSSHelp()
        InitRSSHelp()
        RSSDefinitions.ShouldNotBeNull
        RSSDefinitions.ShouldNotBeEmpty
    End Sub

    ''' <summary>
    ''' Tests initialization of text help
    ''' </summary>
    <TestMethod()> <TestCategory("Initialization")> Public Sub TestInitRDebugHelp()
        InitRDebugHelp()
        RDebugDefinitions.ShouldNotBeNull
        RDebugDefinitions.ShouldNotBeEmpty
    End Sub

    ''' <summary>
    ''' Tests initialization of ZIP help
    ''' </summary>
    <TestMethod()> <TestCategory("Initialization")> Public Sub TestInitTestHelp()
        InitTestHelp()
        TestDefinitions.ShouldNotBeNull
        TestDefinitions.ShouldNotBeEmpty
    End Sub

End Class