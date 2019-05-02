
'    Kernel Simulator  Copyright (C) 2018-2019  EoflaOE
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

Module PageParser

    'TODO: In the final version, implement a manpage parser created by mods
    'Variables
    Public Pages As New Dictionary(Of String, Manual)
    Public AvailablePages() As String = {"Introduction to the Kernel", "Available manual pages", "Available commands", "History of Kernel Simulator",
                                         "Available FTP commands", "Modding guide", "Screensaver modding guide", "adduser", "alias", "arginj",
                                         "calc", "cdir", "chdir", "chhostname", "chmal", "chmotd", "chpwd", "chusrname", "cls", "debuglog", "FTP changelocaldir", "FTP cdl",
                                         "FTP changeremotedir", "FTP cdr", "FTP connect", "FTP currlocaldir", "FTP pwdl", "FTP currremotedir", "FTP pwdr", "FTP delete",
                                         "FTP del", "FTP disconnect", "FTP download", "FTP get", "FTP listlocal", "FTP lsl", "FTP listremote", "FTP lsr", "FTP rename",
                                         "FTP ren", "FTP upload", "FTP put", "ftp", "list", "reloadsaver", "lockscreen", "logout", "lscomp", "lsnet", "lsnettree",
                                         "md", "netinfo", "noaliases", "perm", "ping", "rd", "read", "reboot", "reloadconfig", "rmuser", "savescreen", "scical", "setcolors",
                                         "setsaver", "setthemes", "showmotd", "showtd", "showtdzone", "shutdown", "sysinfo", "unitconv", "useddeps", "Available command-line arguments",
                                         "Available kernel arguments", "Configuration for your Kernel"}
    Public AvailableLayouts() As String = {"0.0.5.9-OR-ABOVE"}
    Private InternalParseDone As Boolean = False
    Private ManTitle As String
    Private BodyParsing As Boolean = False
    Private ColorParsing As Boolean = False
    Private SectionParsing As Boolean = False
    Private UnknownTitleCount As Integer = 0

    'Checks for manual page if it's valid
    Public Sub CheckManual(ByVal Title As String)
        Try
            ManTitle = Title
            If (AvailablePages.Contains(ManTitle)) Then
                Dim manLines As String() = {}
                Select Case ManTitle
                    Case "Introduction to the Kernel"
                        manLines = My.Resources.Introduction_to_the_Kernel.Replace(Chr(13), "").Split(Chr(10))
                    Case "Available manual pages"
                        manLines = My.Resources.Available_manual_pages.Replace(Chr(13), "").Split(Chr(10))
                    Case "Available commands"
                        manLines = My.Resources.Available_commands.Replace(Chr(13), "").Split(Chr(10))
                    Case "History of Kernel Simulator"
                        manLines = My.Resources.History_of_Kernel_Simulator.Replace(Chr(13), "").Split(Chr(10))
                    Case "Available FTP commands"
                        manLines = My.Resources.Available_FTP_commands.Replace(Chr(13), "").Split(Chr(10))
                    Case "Modding guide"
                        manLines = My.Resources.Modding_guide.Replace(Chr(13), "").Split(Chr(10))
                    Case "Screensaver modding guide"
                        manLines = My.Resources.Screensaver_modding_guide.Replace(Chr(13), "").Split(Chr(10))
                    Case "adduser"
                        manLines = My.Resources.REV_0_0_1___adduser.Replace(Chr(13), "").Split(Chr(10))
                    Case "alias"
                        manLines = My.Resources.REV_0_0_1___alias.Replace(Chr(13), "").Split(Chr(10))
                    Case "arginj"
                        manLines = My.Resources.REV_0_0_1___arginj.Replace(Chr(13), "").Split(Chr(10))
                    Case "calc"
                        manLines = My.Resources.REV_0_0_1___calc.Replace(Chr(13), "").Split(Chr(10))
                    Case "cdir"
                        manLines = My.Resources.REV_0_0_1___cdir.Replace(Chr(13), "").Split(Chr(10))
                    Case "chdir"
                        manLines = My.Resources.REV_0_0_1___chdir.Replace(Chr(13), "").Split(Chr(10))
                    Case "chhostname"
                        manLines = My.Resources.REV_0_0_1___chhostname.Replace(Chr(13), "").Split(Chr(10))
                    Case "chmal"
                        manLines = My.Resources.REV_0_0_1___chmal.Replace(Chr(13), "").Split(Chr(10))
                    Case "chmotd"
                        manLines = My.Resources.REV_0_0_1___chmotd.Replace(Chr(13), "").Split(Chr(10))
                    Case "chpwd"
                        manLines = My.Resources.REV_0_0_1___chpwd.Replace(Chr(13), "").Split(Chr(10))
                    Case "chusrname"
                        manLines = My.Resources.REV_0_0_1___chusrname.Replace(Chr(13), "").Split(Chr(10))
                    Case "cls"
                        manLines = My.Resources.REV_0_0_1___cls.Replace(Chr(13), "").Split(Chr(10))
                    Case "debuglog"
                        manLines = My.Resources.REV_0_0_1___debuglog.Replace(Chr(13), "").Split(Chr(10))
                    Case "FTP changelocaldir", "FTP cdl"
                        manLines = My.Resources.REV_0_0_1___FTP_changelocaldir_or_cdl.Replace(Chr(13), "").Split(Chr(10))
                    Case "FTP changeremotedir", "FTP cdr"
                        manLines = My.Resources.REV_0_0_1___FTP_changeremotedir_or_cdr.Replace(Chr(13), "").Split(Chr(10))
                    Case "FTP connect"
                        manLines = My.Resources.REV_0_0_1___FTP_connect.Replace(Chr(13), "").Split(Chr(10))
                    Case "FTP currlocaldir", "FTP pwdl"
                        manLines = My.Resources.REV_0_0_1___FTP_currlocaldir_or_pwdl.Replace(Chr(13), "").Split(Chr(10))
                    Case "FTP currremotedir", "FTP pwdr"
                        manLines = My.Resources.REV_0_0_1___FTP_currremotedir_or_pwdr.Replace(Chr(13), "").Split(Chr(10))
                    Case "FTP delete", "FTP del"
                        manLines = My.Resources.REV_0_0_1___FTP_delete_or_del.Replace(Chr(13), "").Split(Chr(10))
                    Case "FTP disconnect"
                        manLines = My.Resources.REV_0_0_1___FTP_disconnect.Replace(Chr(13), "").Split(Chr(10))
                    Case "FTP download", "FTP get"
                        manLines = My.Resources.REV_0_0_1___FTP_download_or_get.Replace(Chr(13), "").Split(Chr(10))
                    Case "FTP listlocal", "FTP lsl"
                        manLines = My.Resources.REV_0_0_1___FTP_listlocal_or_lsl.Replace(Chr(13), "").Split(Chr(10))
                    Case "FTP listremote", "FTP lsr"
                        manLines = My.Resources.REV_0_0_1___FTP_listremote_or_lsr.Replace(Chr(13), "").Split(Chr(10))
                    Case "FTP rename", "FTP ren"
                        manLines = My.Resources.REV_0_0_1___FTP_rename_or_ren.Replace(Chr(13), "").Split(Chr(10))
                    Case "FTP upload", "FTP put"
                        manLines = My.Resources.REV_0_0_1___FTP_upload_or_put.Replace(Chr(13), "").Split(Chr(10))
                    Case "ftp"
                        manLines = My.Resources.REV_0_0_1___ftp.Replace(Chr(13), "").Split(Chr(10))
                    Case "list"
                        manLines = My.Resources.REV_0_0_1___list.Replace(Chr(13), "").Split(Chr(10))
                    Case "reloadsaver"
                        manLines = My.Resources.REV_0_0_1_1___reloadsaver.Replace(Chr(13), "").Split(Chr(10))
                    Case "lockscreen"
                        manLines = My.Resources.REV_0_0_1___lockscreen.Replace(Chr(13), "").Split(Chr(10))
                    Case "logout"
                        manLines = My.Resources.REV_0_0_1___logout.Replace(Chr(13), "").Split(Chr(10))
                    Case "lscomp"
                        manLines = My.Resources.REV_0_0_1___lscomp.Replace(Chr(13), "").Split(Chr(10))
                    Case "lsnet"
                        manLines = My.Resources.REV_0_0_1___lsnet.Replace(Chr(13), "").Split(Chr(10))
                    Case "lsnettree"
                        manLines = My.Resources.REV_0_0_1___lsnettree.Replace(Chr(13), "").Split(Chr(10))
                    Case "md"
                        manLines = My.Resources.REV_0_0_1___md.Replace(Chr(13), "").Split(Chr(10))
                    Case "netinfo"
                        manLines = My.Resources.REV_0_0_1___netinfo.Replace(Chr(13), "").Split(Chr(10))
                    Case "noaliases"
                        manLines = My.Resources.REV_0_0_1___noaliases.Replace(Chr(13), "").Split(Chr(10))
                    Case "perm"
                        manLines = My.Resources.REV_0_0_1___perm.Replace(Chr(13), "").Split(Chr(10))
                    Case "ping"
                        manLines = My.Resources.REV_0_0_1___ping.Replace(Chr(13), "").Split(Chr(10))
                    Case "rd"
                        manLines = My.Resources.REV_0_0_1___rd.Replace(Chr(13), "").Split(Chr(10))
                    Case "read"
                        manLines = My.Resources.REV_0_0_1___read.Replace(Chr(13), "").Split(Chr(10))
                    Case "reboot"
                        manLines = My.Resources.REV_0_0_1___reboot.Replace(Chr(13), "").Split(Chr(10))
                    Case "reloadconfig"
                        manLines = My.Resources.REV_0_0_1___reloadconfig.Replace(Chr(13), "").Split(Chr(10))
                    Case "rmuser"
                        manLines = My.Resources.REV_0_0_1___rmuser.Replace(Chr(13), "").Split(Chr(10))
                    Case "savescreen"
                        manLines = My.Resources.REV_0_0_1___savescreen.Replace(Chr(13), "").Split(Chr(10))
                    Case "scical"
                        manLines = My.Resources.REV_0_0_1___scical.Replace(Chr(13), "").Split(Chr(10))
                    Case "setcolors"
                        manLines = My.Resources.REV_0_0_1___setcolors.Replace(Chr(13), "").Split(Chr(10))
                    Case "setsaver"
                        manLines = My.Resources.REV_0_0_1___setsaver.Replace(Chr(13), "").Split(Chr(10))
                    Case "setthemes"
                        manLines = My.Resources.REV_0_0_1___setthemes.Replace(Chr(13), "").Split(Chr(10))
                    Case "showmotd"
                        manLines = My.Resources.REV_0_0_1___showmotd.Replace(Chr(13), "").Split(Chr(10))
                    Case "showtd"
                        manLines = My.Resources.REV_0_0_1___showtd.Replace(Chr(13), "").Split(Chr(10))
                    Case "showtdzone"
                        manLines = My.Resources.REV_0_0_1___showtdzone.Replace(Chr(13), "").Split(Chr(10))
                    Case "shutdown"
                        manLines = My.Resources.REV_0_0_1___shutdown.Replace(Chr(13), "").Split(Chr(10))
                    Case "sysinfo"
                        manLines = My.Resources.REV_0_0_1___sysinfo.Replace(Chr(13), "").Split(Chr(10))
                    Case "unitconv"
                        manLines = My.Resources.REV_0_0_1___unitconv.Replace(Chr(13), "").Split(Chr(10))
                    Case "useddeps"
                        manLines = My.Resources.REV_0_0_1___useddeps.Replace(Chr(13), "").Split(Chr(10))
                    Case "Available command-line arguments"
                        manLines = My.Resources.Available_command_line_arguments.Replace(Chr(13), "").Split(Chr(10))
                    Case "Available kernel arguments"
                        manLines = My.Resources.Available_kernel_arguments.Replace(Chr(13), "").Split(Chr(10))
                    Case "Configuration for your Kernel"
                        manLines = My.Resources.Configuration_for_your_Kernel.Replace(Chr(13), "").Split(Chr(10))
                End Select
                Wdbg("Checking manual {0}", ManTitle)
                For Each manLine As String In manLines
                    If (InternalParseDone = True) Then 'Check for the rest if the manpage has MAN START section
                        CheckTODO(manLine)
                        If (BodyParsing = True) Then
                            ParseBody(manLine)
                        ElseIf (ColorParsing = True) Then
                            ParseColor(manLine)
                        ElseIf (SectionParsing = True) Then
                            ParseSection(manLine)
                        Else
                            ParseMan_INTERNAL(manLine)
                        End If
                    ElseIf (InternalParseDone = False) Then 'Check for the MAN START section
                        If (manLine = "(*MAN START*)") Then
                            Wdbg("Successfully found (*MAN START*) in manpage {0}.", ManTitle)
                            InternalParseDone = True
                        End If
                    End If
                Next
                If (InternalParseDone = True) Then
                    Wdbg("Valid manual page! ({0})", ManTitle)
                    Sanity_INTERNAL(ManTitle)
                Else
                    Throw New EventsAndExceptions.TruncatedManpageException(DoTranslation("The manual page {0} is somehow truncated.", currentLang))
                End If
            End If
        Catch ex As Exception
            Wdbg("The manual page {0} is somehow truncated. {1}", ManTitle, ex.Message)
            Wdbg(ex.StackTrace)
            Wln(DoTranslation("There is an error when trying to load the manual page {0} becuase {1}.", currentLang), "neutralText", ManTitle, ex.Message)
            If (DebugMode = True) Then
                Wln(ex.StackTrace, "neutralText")
            End If
        End Try
    End Sub

    'Check for any TODO
    Public Sub CheckTODO(ByVal line As String)
        If (line.Contains("TODO")) Then
            Wdbg("TODO found on this line: {0}", line)
            Dim TODOindex As Integer = InStr(line, "TODO")
            Pages(ManTitle).Todos.Add(line.Substring(TODOindex + "TODO".Length + 1))
        End If
    End Sub

    'Parse manual file from KS, not mods
    Public Sub ParseMan_INTERNAL(ByVal line As String)
        If (line.StartsWith("-REVISION:")) Then
            Wdbg("Revision found on this line: {0}", line)
            Dim Rev As String = line.Substring(line.IndexOf(":") + 1)
            If (Rev = "") Then
                Wdbg("Revision not defined. Assuming v1...")
                Rev = "1"
            End If
            Pages(ManTitle).ManualRevision = Rev
        ElseIf (line.StartsWith("-KSLAYOUT:")) Then
            Dim Lay As String = line.Substring(line.IndexOf(":") + 1)
            If Not (AvailableLayouts.Contains(Lay)) Then
                Wdbg("Layout {0} not found in the available layouts. Assuming 0.0.5.9-OR-ABOVE...", Lay)
                Lay = "0.0.5.9-OR-ABOVE"
            End If
            Pages(ManTitle).ManualLayoutVersion = Lay
        ElseIf (line = "-BODY START-") Then
            BodyParsing = True
        ElseIf (line = "-COLOR CONFIGURATION-") Then
            ColorParsing = True
        ElseIf (line = "-SECTIONS-") Then
            SectionParsing = True
        End If
    End Sub

    'Parse manual file from mods (Not implemented yet)
#Disable Warning IDE0060
    Public Sub ParseMan_EXTERNAL(ByVal line As String, ByVal ManFile As String)
#Enable Warning IDE0060
        Throw New NotImplementedException
    End Sub

    'Get strings until end of body
    Public Sub ParseBody(ByVal line As String)
        If (line <> "-BODY END-") Then
            If line <> "" Then Wdbg("Appending {0} to builder", line)
            Pages(ManTitle).Body.Append(line + vbNewLine)
        ElseIf (line.StartsWith("~~-") = False) Then 'If the line does not start with the comment
            BodyParsing = False
        End If
    End Sub

    'The colors on the manpage will be parsed
    Public Sub ParseColor(ByVal line As String)
        If (line <> "-COLOR CONFIG END-") Then
            Dim colors_MAN() As String = line.Split("=>".ToCharArray, StringSplitOptions.RemoveEmptyEntries)
            If Not (colors_MAN.Length = 0) Then
                If Not (Pages(ManTitle).Colors.ContainsKey(colors_MAN(0))) Then
                    Pages(ManTitle).Colors.Add(colors_MAN(0), CType([Enum].Parse(GetType(ConsoleColor), colors_MAN(1)), ConsoleColor))
                    Wdbg("The color {0} is being assigned to {1}, according to: {2}", colors_MAN(1).ToString, colors_MAN(0), line)
                End If
            End If
        ElseIf (line.StartsWith("~~-") = False) Then
            ColorParsing = False
        End If
    End Sub

    'Parse sections
    Public Sub ParseSection(ByVal line As String)
        If (line <> "-SECTIONS END-") Then
            Dim sections() As String = line.Split("=>".ToCharArray, StringSplitOptions.RemoveEmptyEntries)
            Pages(ManTitle).Sections.Add(sections(0), sections(1))
            Wdbg("The section {0} is being assigned to {1}, according to {2}", sections(1), sections(0), line)
        ElseIf (line.StartsWith("~~-") = False) Then
            SectionParsing = False
        End If
    End Sub

    'Perform a sanity check on internal manpages
    Public Sub Sanity_INTERNAL(ByVal title As String)
        If (title = "") Then
            UnknownTitleCount += 1
            Wdbg("The manual page #{0} seems to have no title.", UnknownTitleCount)
            Dim originalValue As Manual = Pages(title)
            Pages.Remove(title)
            title = "Manual page #" + UnknownTitleCount
            Pages.Add(title, originalValue)
            Pages(title).ManualTitle = title
            Wdbg("Title has changed to ""{0}""", title)
            Wln(DoTranslation("This manual page title is not written", currentLang), "neutralText")
        ElseIf (Pages(title).Body.ToString = "") Then
            Wdbg("Body for ""{0}"" does not contain anything.", title)
            Wln(DoTranslation("This manual page ({0}) does not contain any body text. Deleting page...", currentLang), "neutralText", title)
            Pages.Remove(title)
        ElseIf (Pages(title).Sections.Count = 0) Then
            Wdbg("No sections for ""{0}""", title)
            Wln(DoTranslation("This manual page ({0}) does not contain any section. Deleting page...", currentLang), "neutralText", title)
            Pages.Remove(title)
        End If
    End Sub

    'Perform a sanity check on mod manpages (Not implemented)
#Disable Warning IDE0060
    Public Sub Sanity_EXTERNAL(ByVal title As String)
#Enable Warning IDE0060
        Throw New NotImplementedException
    End Sub

End Module
