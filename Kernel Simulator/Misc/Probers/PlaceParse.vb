
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

Public Module PlaceParse

    ''' <summary>
    ''' Probes the placeholders found in string
    ''' </summary>
    ''' <param name="text">Specified string</param>
    ''' <returns>A string that has the parsed placeholders</returns>
    Public Function ProbePlaces(text As String) As String

        Kernel.EventManager.RaisePlaceholderParsing(text)
        Try
            Wdbg(DebugLevel.I, "Parsing text for placeholders...")
            If text.Contains("<user>") Then
                Wdbg(DebugLevel.I, "Username placeholder found.")
                text = text.Replace("<user>", CurrentUser.Username)
            End If
            If text.Contains("<ftpuser>") Then
                Wdbg(DebugLevel.I, "FTP username placeholder found.")
                text = text.Replace("<ftpuser>", FtpUser)
            End If
            If text.Contains("<ftpaddr>") Then
                Wdbg(DebugLevel.I, "FTP address placeholder found.")
                text = text.Replace("<ftpaddr>", FtpSite)
            End If
            If text.Contains("<currentftpdirectory>") Then
                Wdbg(DebugLevel.I, "FTP directory placeholder found.")
                text = text.Replace("<currentftpdirectory>", FtpCurrentRemoteDir)
            End If
            If text.Contains("<currentftplocaldirectory>") Then
                Wdbg(DebugLevel.I, "FTP local directory placeholder found.")
                text = text.Replace("<currentftplocaldirectory>", FtpCurrentDirectory)
            End If
            If text.Contains("<currentftplocaldirectoryname>") Then
                Wdbg(DebugLevel.I, "FTP local directory name placeholder found.")
                text = text.Replace("<currentftplocaldirectoryname>", New DirectoryInfo(FtpCurrentDirectory).Name)
            End If
            If text.Contains("<sftpuser>") Then
                Wdbg(DebugLevel.I, "SFTP username placeholder found.")
                text = text.Replace("<sftpuser>", SFTPUser)
            End If
            If text.Contains("<sftpaddr>") Then
                Wdbg(DebugLevel.I, "SFTP address placeholder found.")
                text = text.Replace("<sftpaddr>", SFTPSite)
            End If
            If text.Contains("<currentsftpdirectory>") Then
                Wdbg(DebugLevel.I, "SFTP directory placeholder found.")
                text = text.Replace("<currentsftpdirectory>", SFTPCurrentRemoteDir)
            End If
            If text.Contains("<currentsftplocaldirectory>") Then
                Wdbg(DebugLevel.I, "SFTP local directory placeholder found.")
                text = text.Replace("<currentsftplocaldirectory>", SFTPCurrDirect)
            End If
            If text.Contains("<currentsftplocaldirectoryname>") Then
                Wdbg(DebugLevel.I, "SFTP local directory name placeholder found.")
                text = text.Replace("<currentsftplocaldirectoryname>", New DirectoryInfo(SFTPCurrDirect).Name)
            End If
            If text.Contains("<mailuser>") Then
                Wdbg(DebugLevel.I, "Mail username placeholder found.")
                text = text.Replace("<mailuser>", Mail_Authentication.UserName)
            End If
            If text.Contains("<mailaddr>") Then
                Wdbg(DebugLevel.I, "Mail address placeholder found.")
                text = text.Replace("<mailaddr>", Mail_Authentication.Domain)
            End If
            If text.Contains("<currentmaildirectory>") Then
                Wdbg(DebugLevel.I, "Mail directory placeholder found.")
                text = text.Replace("<currentmaildirectory>", IMAP_CurrentDirectory)
            End If
            If text.Contains("<host>") Then
                Wdbg(DebugLevel.I, "Hostname placeholder found.")
                text = text.Replace("<host>", HostName)
            End If
            If text.Contains("<currentdirectory>") Then
                Wdbg(DebugLevel.I, "Current directory placeholder found.")
                text = text.Replace("<currentdirectory>", CurrDir)
            End If
            If text.Contains("<currentdirectoryname>") Then
                Wdbg(DebugLevel.I, "Current directory name placeholder found.")
                text = text.Replace("<currentdirectoryname>", New DirectoryInfo(CurrDir).Name)
            End If
            If text.Contains("<shortdate>") Then
                Wdbg(DebugLevel.I, "Short Date placeholder found.")
                text = text.Replace("<shortdate>", KernelDateTime.ToShortDateString)
            End If
            If text.Contains("<longdate>") Then
                Wdbg(DebugLevel.I, "Long Date placeholder found.")
                text = text.Replace("<longdate>", KernelDateTime.ToLongDateString)
            End If
            If text.Contains("<shorttime>") Then
                Wdbg(DebugLevel.I, "Short Time placeholder found.")
                text = text.Replace("<shorttime>", KernelDateTime.ToShortTimeString)
            End If
            If text.Contains("<longtime>") Then
                Wdbg(DebugLevel.I, "Long Time placeholder found.")
                text = text.Replace("<longtime>", KernelDateTime.ToShortDateString)
            End If
            If text.Contains("<date>") Then
                Wdbg(DebugLevel.I, "Rendered Date placeholder found.")
                text = text.Replace("<date>", RenderDate)
            End If
            If text.Contains("<time>") Then
                Wdbg(DebugLevel.I, "Rendered Time placeholder found.")
                text = text.Replace("<time>", RenderTime)
            End If
            If text.Contains("<timezone>") Then
                Wdbg(DebugLevel.I, "Standard Time Zone placeholder found.")
                text = text.Replace("<timezone>", TimeZone.CurrentTimeZone.StandardName)
            End If
            If text.Contains("<summertimezone>") Then
                Wdbg(DebugLevel.I, "Summer Time Zone placeholder found.")
                text = text.Replace("<summertimezone>", TimeZone.CurrentTimeZone.DaylightName)
            End If
            If text.Contains("<system>") Then
                Wdbg(DebugLevel.I, "System placeholder found.")
                text = text.Replace("<system>", Environment.OSVersion.ToString)
            End If
            If text.Contains("<newline>") Then
                Wdbg(DebugLevel.I, "Newline placeholder found.")
                text = text.Replace("<newline>", vbNewLine)
            End If
            If text.Contains("<f:reset>") Then
                Wdbg(DebugLevel.I, "Foreground color reset placeholder found.")
                text = text.Replace("<f:reset>", New Color(NeutralTextColor).VTSequenceForeground)
            End If
            If text.Contains("<b:reset>") Then
                Wdbg(DebugLevel.I, "Background color reset placeholder found.")
                text = text.Replace("<b:reset>", New Color(BackgroundColor).VTSequenceBackground)
            End If
            If text.Contains("<f:") Then
                Wdbg(DebugLevel.I, "Foreground color placeholder found.")
                Do While text.Contains("<f:")
                    Dim StartForegroundIndex As Integer = text.IndexOf("<f:")
                    Dim EndForegroundIndex As Integer = text.Substring(text.IndexOf("<f:")).IndexOf(">")
                    Dim SequenceSubstring As String = text.Substring(text.IndexOf("<f:"), length:=EndForegroundIndex + 1)
                    Dim PlainSequence As String = SequenceSubstring.Substring(3, SequenceSubstring.Length - 1 - 3)
                    Dim VTSequence As String = New Color(PlainSequence).VTSequenceForeground
                    text = text.Replace(SequenceSubstring, VTSequence)
                Loop
            End If
            If text.Contains("<b:") Then
                Wdbg(DebugLevel.I, "Background color placeholder found.")
                Do While text.Contains("<b:")
                    Dim StartBackgroundIndex As Integer = text.IndexOf("<b:")
                    Dim EndBackgroundIndex As Integer = text.Substring(text.IndexOf("<b:")).IndexOf(">")
                    Dim SequenceSubstring As String = text.Substring(text.IndexOf("<b:"), length:=EndBackgroundIndex + 1)
                    Dim PlainSequence As String = SequenceSubstring.Substring(3, SequenceSubstring.Length - 1 - 3)
                    Dim VTSequence As String = New Color(PlainSequence).VTSequenceBackground
                    text = text.Replace(SequenceSubstring, VTSequence)
                Loop
            End If
            If text.Contains("<$") Then
                Wdbg(DebugLevel.I, "UESH variable placeholder found.")
                Do While text.Contains("<$")
                    Dim StartShellVariableIndex As Integer = text.IndexOf("<$")
                    Dim EndShellVariableIndex As Integer = text.Substring(text.IndexOf("<$")).IndexOf(">")
                    Dim ShellVariableSubstring As String = text.Substring(text.IndexOf("<$"), length:=EndShellVariableIndex + 1)
                    Dim PlainShellVariable As String = ShellVariableSubstring.Substring(1, ShellVariableSubstring.Length - 1 - 1)
                    text = text.Replace(ShellVariableSubstring, GetVariable(PlainShellVariable))
                Loop
            End If
            Kernel.EventManager.RaisePlaceholderParsed(text)
        Catch ex As Exception
            WStkTrc(ex)
            W(DoTranslation("Error trying to parse placeholders. {0}"), True, ColTypes.Error, ex.Message)
        End Try
        Return text

    End Function

End Module
