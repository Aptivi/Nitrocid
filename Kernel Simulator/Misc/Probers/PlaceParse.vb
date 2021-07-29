
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
    Public Function ProbePlaces(ByVal text As String) As String

        EventManager.RaisePlaceholderParsing(text)
        Try
            Wdbg("I", "Parsing text for placeholders...")
            If text.Contains("<user>") Then
                Wdbg("I", "Username placeholder found.")
                text = text.Replace("<user>", CurrentUser)
            End If
            If text.Contains("<ftpuser>") Then
                Wdbg("I", "FTP username placeholder found.")
                text = text.Replace("<ftpuser>", user)
            End If
            If text.Contains("<ftpaddr>") Then
                Wdbg("I", "FTP address placeholder found.")
                text = text.Replace("<ftpaddr>", ftpsite)
            End If
            If text.Contains("<currentftpdirectory>") Then
                Wdbg("I", "FTP directory placeholder found.")
                text = text.Replace("<currentftpdirectory>", currentremoteDir)
            End If
            If text.Contains("<currentftplocaldirectory>") Then
                Wdbg("I", "FTP local directory placeholder found.")
                text = text.Replace("<currentftplocaldirectory>", currDirect)
            End If
            If text.Contains("<currentftplocaldirectoryname>") Then
                Wdbg("I", "FTP local directory name placeholder found.")
                text = text.Replace("<currentftplocaldirectoryname>", New DirectoryInfo(currDirect).Name)
            End If
            If text.Contains("<sftpuser>") Then
                Wdbg("I", "SFTP username placeholder found.")
                text = text.Replace("<sftpuser>", SFTPUser)
            End If
            If text.Contains("<sftpaddr>") Then
                Wdbg("I", "SFTP address placeholder found.")
                text = text.Replace("<sftpaddr>", sftpsite)
            End If
            If text.Contains("<currentsftpdirectory>") Then
                Wdbg("I", "SFTP directory placeholder found.")
                text = text.Replace("<currentsftpdirectory>", SFTPCurrentRemoteDir)
            End If
            If text.Contains("<currentsftplocaldirectory>") Then
                Wdbg("I", "SFTP local directory placeholder found.")
                text = text.Replace("<currentsftplocaldirectory>", SFTPCurrDirect)
            End If
            If text.Contains("<currentsftplocaldirectoryname>") Then
                Wdbg("I", "SFTP local directory name placeholder found.")
                text = text.Replace("<currentsftplocaldirectoryname>", New DirectoryInfo(SFTPCurrDirect).Name)
            End If
            If text.Contains("<mailuser>") Then
                Wdbg("I", "Mail username placeholder found.")
                text = text.Replace("<mailuser>", Mail_Authentication.UserName)
            End If
            If text.Contains("<mailaddr>") Then
                Wdbg("I", "Mail address placeholder found.")
                text = text.Replace("<mailaddr>", Mail_Authentication.Domain)
            End If
            If text.Contains("<currentmaildirectory>") Then
                Wdbg("I", "Mail directory placeholder found.")
                text = text.Replace("<currentmaildirectory>", IMAP_CurrentDirectory)
            End If
            If text.Contains("<host>") Then
                Wdbg("I", "Hostname placeholder found.")
                text = text.Replace("<host>", HName)
            End If
            If text.Contains("<currentdirectory>") Then
                Wdbg("I", "Current directory placeholder found.")
                text = text.Replace("<currentdirectory>", CurrDir)
            End If
            If text.Contains("<currentdirectoryname>") Then
                Wdbg("I", "Current directory name placeholder found.")
                text = text.Replace("<currentdirectoryname>", New DirectoryInfo(CurrDir).Name)
            End If
            If text.Contains("<shortdate>") Then
                Wdbg("I", "Short Date placeholder found.")
                text = text.Replace("<shortdate>", KernelDateTime.ToShortDateString)
            End If
            If text.Contains("<longdate>") Then
                Wdbg("I", "Long Date placeholder found.")
                text = text.Replace("<longdate>", KernelDateTime.ToLongDateString)
            End If
            If text.Contains("<shorttime>") Then
                Wdbg("I", "Short Time placeholder found.")
                text = text.Replace("<shorttime>", KernelDateTime.ToShortTimeString)
            End If
            If text.Contains("<longtime>") Then
                Wdbg("I", "Long Time placeholder found.")
                text = text.Replace("<longtime>", KernelDateTime.ToShortDateString)
            End If
            If text.Contains("<date>") Then
                Wdbg("I", "Rendered Date placeholder found.")
                text = text.Replace("<date>", RenderDate)
            End If
            If text.Contains("<time>") Then
                Wdbg("I", "Rendered Time placeholder found.")
                text = text.Replace("<time>", RenderTime)
            End If
            If text.Contains("<timezone>") Then
                Wdbg("I", "Standard Time Zone placeholder found.")
                text = text.Replace("<timezone>", TimeZone.CurrentTimeZone.StandardName)
            End If
            If text.Contains("<summertimezone>") Then
                Wdbg("I", "Summer Time Zone placeholder found.")
                text = text.Replace("<summertimezone>", TimeZone.CurrentTimeZone.DaylightName)
            End If
            If text.Contains("<system>") Then
                Wdbg("I", "System placeholder found.")
                text = text.Replace("<system>", Environment.OSVersion.ToString)
            End If
            If text.Contains("<f:reset>") Then
                Wdbg("I", "Foreground color reset placeholder found.")
                text = text.Replace("<f:reset>", New Color(NeutralTextColor).VTSequenceForeground)
            End If
            If text.Contains("<b:reset>") Then
                Wdbg("I", "Background color reset placeholder found.")
                text = text.Replace("<b:reset>", New Color(BackgroundColor).VTSequenceBackground)
            End If
            If text.Contains("<f:") Then
                Wdbg("I", "Foreground color placeholder found.")
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
                Wdbg("I", "Background color placeholder found.")
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
                Wdbg("I", "UESH variable placeholder found.")
                Do While text.Contains("<$")
                    Dim StartShellVariableIndex As Integer = text.IndexOf("<$")
                    Dim EndShellVariableIndex As Integer = text.Substring(text.IndexOf("<$")).IndexOf(">")
                    Dim ShellVariableSubstring As String = text.Substring(text.IndexOf("<$"), length:=EndShellVariableIndex + 1)
                    Dim PlainShellVariable As String = ShellVariableSubstring.Substring(1, ShellVariableSubstring.Length - 1 - 1)
                    text = text.Replace(ShellVariableSubstring, GetVariable(PlainShellVariable))
                Loop
            End If
            EventManager.RaisePlaceholderParsed(text)
        Catch ex As Exception
            WStkTrc(ex)
            If DebugMode = True Then
                W(DoTranslation("Error trying to parse placeholders. {0} - Stack trace:") + vbNewLine + ex.StackTrace, True, ColTypes.Error, ex.Message)
            Else
                W(DoTranslation("Error trying to parse placeholders. {0}"), True, ColTypes.Error, ex.Message)
            End If
        End Try
        Return text

    End Function

End Module
