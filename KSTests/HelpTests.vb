
'    Kernel Simulator  Copyright (C) 2018-2020  EoflaOE
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

<TestClass()> Public Class HelpTests

    <TestMethod()> Public Sub TestInitFTPHelp()
        Try
            InitFTPHelp()
            Assert.IsNotNull(ftpDefinitions)
        Catch afex As AssertFailedException
            Assert.Fail("Initialization of FTP help failed")
        End Try
    End Sub

    <TestMethod()> Public Sub TestInitMailHelp()
        Try
            IMAPInitHelp()
            Assert.IsNotNull(IMAP_definitions)
        Catch afex As AssertFailedException
            Assert.Fail("Initialization of mail help failed")
        End Try
    End Sub

    <TestMethod()> Public Sub TestInitShellHelp()
        Try
            InitHelp()
            Assert.IsNotNull(definitions)
        Catch afex As AssertFailedException
            Assert.Fail("Initialization of shell help failed")
        End Try
    End Sub

    <TestMethod()> Public Sub TestInitTextHelp()
        Try
            TextEdit_UpdateHelp()
            Assert.IsNotNull(TextEdit_HelpEntries)
        Catch afex As AssertFailedException
            Assert.Fail("Initialization of text editor help failed")
        End Try
    End Sub

End Class