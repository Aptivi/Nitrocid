
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

Imports System.Threading
Imports KS.Network.RemoteDebug

Module AliasExecutor

    ''' <summary>
    ''' Translates alias to actual command, preserving arguments
    ''' </summary>
    ''' <param name="aliascmd">Specifies the alias with arguments</param>
    Sub ExecuteAlias(aliascmd As String)
        Dim FirstWordCmd As String = aliascmd.SplitEncloseDoubleQuotes(" ")(0)
        Dim actualCmd As String = aliascmd.Replace(FirstWordCmd, Aliases(FirstWordCmd))
        Wdbg(DebugLevel.I, "Actual command: {0}", actualCmd)
        Dim Params As New ExecuteCommandThreadParameters(actualCmd, ShellType.Shell, Nothing)
        StartCommandThread = New Thread(AddressOf ExecuteCommand) With {.Name = "Shell Command Thread"}
        StartCommandThread.Start(Params)
        StartCommandThread.Join()
    End Sub

    ''' <summary>
    ''' Executes the test shell alias
    ''' </summary>
    ''' <param name="aliascmd">Aliased command with arguments</param>
    Sub ExecuteTestAlias(aliascmd As String)
        Dim FirstWordCmd As String = aliascmd.SplitEncloseDoubleQuotes(" ")(0)
        Dim actualCmd As String = aliascmd.Replace(FirstWordCmd, TestShellAliases(FirstWordCmd))
        Wdbg(DebugLevel.I, "Actual command: {0}", actualCmd)
        Dim Params As New ExecuteCommandThreadParameters(actualCmd, ShellType.TestShell, Nothing)
        TStartCommandThread = New Thread(AddressOf ExecuteCommand) With {.Name = "Test Shell Command Thread"}
        TStartCommandThread.Start(Params)
        TStartCommandThread.Join()
    End Sub

    ''' <summary>
    ''' Executes the SFTP shell alias
    ''' </summary>
    ''' <param name="aliascmd">Aliased command with arguments</param>
    Sub ExecuteSFTPAlias(aliascmd As String)
        Dim FirstWordCmd As String = aliascmd.SplitEncloseDoubleQuotes(" ")(0)
        Dim actualCmd As String = aliascmd.Replace(FirstWordCmd, SFTPShellAliases(FirstWordCmd))
        Wdbg(DebugLevel.I, "Actual command: {0}", actualCmd)
        Dim Params As New ExecuteCommandThreadParameters(actualCmd, ShellType.SFTPShell, Nothing)
        SFTPStartCommandThread = New Thread(AddressOf ExecuteCommand) With {.Name = "SFTP Command Thread"}
        SFTPStartCommandThread.Start(Params)
        SFTPStartCommandThread.Join()
    End Sub

    ''' <summary>
    ''' Executes the RSS shell alias
    ''' </summary>
    ''' <param name="aliascmd">Aliased command with arguments</param>
    Sub ExecuteRSSAlias(aliascmd As String)
        Dim FirstWordCmd As String = aliascmd.SplitEncloseDoubleQuotes(" ")(0)
        Dim actualCmd As String = aliascmd.Replace(FirstWordCmd, RSSShellAliases(FirstWordCmd))
        Wdbg(DebugLevel.I, "Actual command: {0}", actualCmd)
        Dim Params As New ExecuteCommandThreadParameters(actualCmd, ShellType.RSSShell, Nothing)
        RSSCommandThread = New Thread(AddressOf ExecuteCommand) With {.Name = "RSS Shell Command Thread"}
        RSSCommandThread.Start(Params)
        RSSCommandThread.Join()
    End Sub

    ''' <summary>
    ''' Executes the remote debugger alias
    ''' </summary>
    ''' <param name="aliascmd">Aliased command with arguments</param>
    ''' <param name="SocketStream">A socket stream writer</param>
    ''' <param name="Address">IP Address</param>
    Sub ExecuteRDAlias(aliascmd As String, SocketStream As IO.StreamWriter, Address As String)
        Dim FirstWordCmd As String = aliascmd.Split(" "c)(0)
        Dim actualCmd As String = aliascmd.Replace(FirstWordCmd, RemoteDebugAliases(FirstWordCmd))
        ParseCmd(actualCmd, SocketStream, Address)
    End Sub

    ''' <summary>
    ''' Executes the mail shell alias
    ''' </summary>
    ''' <param name="aliascmd">Aliased command with arguments</param>
    Sub ExecuteMailAlias(aliascmd As String)
        Dim FirstWordCmd As String = aliascmd.SplitEncloseDoubleQuotes(" ")(0)
        Dim actualCmd As String = aliascmd.Replace(FirstWordCmd, MailShellAliases(FirstWordCmd))
        Wdbg(DebugLevel.I, "Actual command: {0}", actualCmd)
        Dim Params As New ExecuteCommandThreadParameters(actualCmd, ShellType.MailShell, Nothing)
        MailStartCommandThread = New Thread(AddressOf ExecuteCommand) With {.Name = "Mail Command Thread"}
        MailStartCommandThread.Start(Params)
        MailStartCommandThread.Join()
    End Sub

    ''' <summary>
    ''' Executes the FTP shell alias
    ''' </summary>
    ''' <param name="aliascmd">Aliased command with arguments</param>
    Sub ExecuteFTPAlias(aliascmd As String)
        Dim FirstWordCmd As String = aliascmd.SplitEncloseDoubleQuotes(" ")(0)
        Dim actualCmd As String = aliascmd.Replace(FirstWordCmd, FTPShellAliases(FirstWordCmd))
        Wdbg(DebugLevel.I, "Actual command: {0}", actualCmd)
        Dim Params As New ExecuteCommandThreadParameters(actualCmd, ShellType.FTPShell, Nothing)
        FTPStartCommandThread = New Thread(AddressOf ExecuteCommand) With {.Name = "FTP Command Thread"}
        FTPStartCommandThread.Start(Params)
        FTPStartCommandThread.Join()
    End Sub

    ''' <summary>
    ''' Executes the ZIP shell alias
    ''' </summary>
    ''' <param name="aliascmd">Aliased command with arguments</param>
    Sub ExecuteZIPAlias(aliascmd As String)
        Dim FirstWordCmd As String = aliascmd.SplitEncloseDoubleQuotes(" ")(0)
        Dim actualCmd As String = aliascmd.Replace(FirstWordCmd, ZIPShellAliases(FirstWordCmd))
        Wdbg(DebugLevel.I, "Actual command: {0}", actualCmd)
        Dim Params As New ExecuteCommandThreadParameters(actualCmd, ShellType.ZIPShell, Nothing)
        ZipShell_CommandThread = New Thread(AddressOf ExecuteCommand) With {.Name = "ZIP Shell Command Thread"}
        ZipShell_CommandThread.Start(Params)
        ZipShell_CommandThread.Join()
    End Sub

    ''' <summary>
    ''' Executes the text editor shell alias
    ''' </summary>
    ''' <param name="aliascmd">Aliased command with arguments</param>
    Sub ExecuteTextAlias(aliascmd As String)
        Dim FirstWordCmd As String = aliascmd.SplitEncloseDoubleQuotes(" ")(0)
        Dim actualCmd As String = aliascmd.Replace(FirstWordCmd, TextShellAliases(FirstWordCmd))
        Wdbg(DebugLevel.I, "Actual command: {0}", actualCmd)
        Dim Params As New ExecuteCommandThreadParameters(actualCmd, ShellType.TextShell, Nothing)
        TextEdit_CommandThread = New Thread(AddressOf ExecuteCommand) With {.Name = "Text Edit Command Thread"}
        TextEdit_CommandThread.Start(Params)
        TextEdit_CommandThread.Join()
    End Sub

    ''' <summary>
    ''' Executes the JSON shell alias
    ''' </summary>
    ''' <param name="aliascmd">Aliased command with arguments</param>
    Sub ExecuteJsonAlias(aliascmd As String)
        Dim FirstWordCmd As String = aliascmd.SplitEncloseDoubleQuotes(" ")(0)
        Dim actualCmd As String = aliascmd.Replace(FirstWordCmd, JsonShellAliases(FirstWordCmd))
        Wdbg(DebugLevel.I, "Actual command: {0}", actualCmd)
        Dim Params As New ExecuteCommandThreadParameters(actualCmd, ShellType.JsonShell, Nothing)
        JsonShell_CommandThread = New Thread(AddressOf ExecuteCommand) With {.Name = "JSON Shell Command Thread"}
        JsonShell_CommandThread.Start(Params)
        JsonShell_CommandThread.Join()
    End Sub

    ''' <summary>
    ''' Executes the HTTP shell alias
    ''' </summary>
    ''' <param name="aliascmd">Aliased command with arguments</param>
    Sub ExecuteHTTPAlias(aliascmd As String)
        Dim FirstWordCmd As String = aliascmd.SplitEncloseDoubleQuotes(" ")(0)
        Dim actualCmd As String = aliascmd.Replace(FirstWordCmd, HTTPShellAliases(FirstWordCmd))
        Wdbg(DebugLevel.I, "Actual command: {0}", actualCmd)
        Dim Params As New ExecuteCommandThreadParameters(actualCmd, ShellType.HTTPShell, Nothing)
        HTTPCommandThread = New Thread(AddressOf ExecuteCommand) With {.Name = "HTTP Shell Command Thread"}
        HTTPCommandThread.Start(Params)
        HTTPCommandThread.Join()
    End Sub

End Module
