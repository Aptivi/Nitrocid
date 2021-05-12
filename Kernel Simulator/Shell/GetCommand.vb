
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
Imports System.IO.Compression
Imports System.Net.NetworkInformation
Imports System.Text
Imports System.Threading
Imports Microsoft.VisualBasic.FileIO

Public Module GetCommand

    Public StartCommandThread As New Thread(AddressOf ExecuteCommand)

    ''' <summary>
    ''' Executes a command
    ''' </summary>
    ''' <param name="requestedCommand">A command. It may contain arguments</param>
    Public Sub ExecuteCommand(ByVal requestedCommand As String)
        'Variables
        Dim Command As String
        Dim RequiredArgumentsProvided As Boolean = True

        '1. Get the index of the first space (Used for step 3)
        Dim index As Integer = requestedCommand.IndexOf(" ")
        If index = -1 Then index = requestedCommand.Length
        Wdbg("I", "Index: {0}", index)

        '2. Split the requested command string into words
        Dim words() As String = requestedCommand.Split({" "c})
        For i As Integer = 0 To words.Length - 1
            Wdbg("I", "Word {0}: {1}", i + 1, words(i))
        Next
        Command = words(0)

        '3. Get the string of arguments
        Dim strArgs As String = requestedCommand.Substring(index)
        Wdbg("I", "Prototype strArgs: {0}", strArgs)
        If Not index = requestedCommand.Length Then strArgs = strArgs.Substring(1)
        Wdbg("I", "Finished strArgs: {0}", strArgs)

        '4. Split the arguments with enclosed quotes and set the required boolean variable
        Dim eqargs() As String
        Dim TStream As New MemoryStream(Encoding.Default.GetBytes(strArgs))
        Dim Parser As New TextFieldParser(TStream) With {
            .Delimiters = {" "},
            .HasFieldsEnclosedInQuotes = True,
            .TrimWhiteSpace = False
        }
        eqargs = Parser.ReadFields
        If eqargs IsNot Nothing Then
            For i As Integer = 0 To eqargs.Length - 1
                eqargs(i).Replace("""", "")
            Next
            RequiredArgumentsProvided = eqargs?.Length >= Commands(Command).MinimumArguments
        ElseIf Commands(Command).ArgumentsRequired And eqargs Is Nothing Then
            RequiredArgumentsProvided = False
        End If

        '4a. Debug: get all arguments from eqargs()
        If eqargs IsNot Nothing Then Wdbg("I", "Arguments parsed from eqargs(): " + String.Join(", ", eqargs))

        '5. Check to see if a requested command is obsolete
        If Commands(Command).Obsolete Then
            Wdbg("I", "The command requested {0} is obsolete", Command)
            W(DoTranslation("This command is obsolete and will be removed in a future release."), True, ColTypes.Neutral)
        End If

        '6. Execute a command
        Try
            Select Case Command
                Case "help"

                    If requestedCommand = "help" Then
                        ShowHelp()
                    Else
                        If RequiredArgumentsProvided Then
                            ShowHelp(eqargs(0))
                        Else
                            ShowHelp(Command)
                        End If
                    End If

                Case "adduser"

                    If RequiredArgumentsProvided Then
                        If eqargs?.Length = 1 Then
                            W(DoTranslation("usrmgr: Creating username {0}..."), True, ColTypes.Neutral, eqargs(0))
                            AddUser(eqargs(0))
                        ElseIf eqargs?.Length > 2 Then
                            If eqargs(1) = eqargs(2) Then
                                W(DoTranslation("usrmgr: Creating username {0}..."), True, ColTypes.Neutral, eqargs(0))
                                AddUser(eqargs(0), eqargs(1))
                            Else
                                W(DoTranslation("Passwords don't match."), True, ColTypes.Err)
                            End If
                        End If
                    End If

                Case "alias"

                    If RequiredArgumentsProvided Then
                        If eqargs?.Length > 3 Then
                            If eqargs(0) = "add" And (eqargs(1) = AliasType.Shell Or eqargs(1) = AliasType.RDebug Or eqargs(1) = AliasType.FTPShell Or eqargs(1) = AliasType.SFTPShell Or eqargs(1) = AliasType.MailShell) Then
                                ManageAlias(eqargs(0), eqargs(1), eqargs(2), eqargs(3))
                            Else
                                W(DoTranslation("Invalid type {0}."), True, ColTypes.Err, eqargs(1))
                            End If
                        ElseIf eqargs?.Length = 3 Then
                            If eqargs(0) = "rem" And (eqargs(1) = AliasType.Shell Or eqargs(1) = AliasType.RDebug Or eqargs(1) = AliasType.FTPShell Or eqargs(1) = AliasType.SFTPShell Or eqargs(1) = AliasType.MailShell) Then
                                ManageAlias(eqargs(0), eqargs(1), eqargs(2))
                            Else
                                W(DoTranslation("Invalid type {0}."), True, ColTypes.Err, eqargs(1))
                            End If
                        End If
                    End If

                Case "arginj"

                    'Argument Injection
                    If RequiredArgumentsProvided Then
                        Dim FinalArgs As New List(Of String)
                        For Each arg As String In eqargs
                            Wdbg("I", "Parsing argument {0}...", arg)
                            If AvailableArgs.Contains(arg) Then
                                Wdbg("I", "Adding argument {0}...", arg)
                                FinalArgs.Add(arg)
                            End If
                        Next
                        If FinalArgs.Count = 0 Then
                            W(DoTranslation("No arguments specified. Hint: Specify multiple arguments separated by spaces"), True, ColTypes.Err)
                        Else
                            answerargs = String.Join(",", FinalArgs)
                            argsInjected = True
                            W(DoTranslation("Injected arguments, {0}, will be scheduled to run at next reboot."), True, ColTypes.Neutral, answerargs)
                        End If
                    End If

                Case "beep"

                    If RequiredArgumentsProvided Then
                        If eqargs(0).IsNumeric And CInt(eqargs(0)) >= 37 And CInt(eqargs(0)) <= 32767 Then 'Frequency must be numeric, and must be >= 37 and <= 32767
                            If eqargs(1).IsNumeric Then 'Time must be numeric
                                Console.Beep(eqargs(0), eqargs(1))
                            Else
                                W(DoTranslation("Time must be numeric."), True, ColTypes.Err)
                            End If
                        Else
                            W(DoTranslation("Frequency must be numeric. If it's numeric, ensure that it is >= 37 and <= 32767."), True, ColTypes.Err)
                        End If
                    End If

                Case "blockdbgdev"

                    If RequiredArgumentsProvided Then
                        If Not RDebugBlocked.Contains(eqargs(0)) Then
                            If AddToBlockList(eqargs(0)) Then
                                W(DoTranslation("{0} can't join remote debug now."), True, ColTypes.Neutral, eqargs(0))
                            Else
                                W(DoTranslation("Failed to block {0}."), True, ColTypes.Neutral, eqargs(0))
                            End If
                        Else
                            W(DoTranslation("{0} is already blocked."), True, ColTypes.Neutral, eqargs(0))
                        End If
                    End If

                Case "cat"

                    If RequiredArgumentsProvided Then
                        Try
                            ReadContents(eqargs(0))
                        Catch ex As Exception
                            WStkTrc(ex)
                            W(ex.Message, True, ColTypes.Err)
                        End Try
                    End If

                Case "calc"

                    If RequiredArgumentsProvided Then
                        Try
                            Dim Res As String = Evaluate(strArgs)
                            Wdbg("I", "Res = {0}", Res)
                            If Res = "" Then 'If there is an error in calculation
                                W(DoTranslation("Error in calculation."), True, ColTypes.Err)
                            Else 'Calculation done
                                W(strArgs + " = " + Res, True, ColTypes.Neutral)
                            End If
                        Catch ex As Exception
                            WStkTrc(ex)
                            W(DoTranslation("Error in calculation."), True, ColTypes.Err)
                        End Try
                    End If

                Case "cdbglog"

                    If DebugMode Then
                        Try
                            dbgWriter.Close()
                            dbgWriter = New StreamWriter(paths("Debugging")) With {.AutoFlush = True}
                            W(DoTranslation("Debug log removed. All connected debugging devices may still view messages."), True, ColTypes.Neutral)
                        Catch ex As Exception
                            W(DoTranslation("Debug log removal failed: {0}"), True, ColTypes.Err, ex.Message)
                            WStkTrc(ex)
                        End Try
                    Else
                        W(DoTranslation("You must turn on debug mode before you can clear debug log."), True, ColTypes.Neutral)
                    End If

                Case "chattr"

                    If RequiredArgumentsProvided Then
                        Dim NeutralizedFilePath As String = NeutralizePath(eqargs(0))
                        If File.Exists(NeutralizedFilePath) Then
                            If eqargs(1).EndsWith("Normal") Or eqargs(1).EndsWith("ReadOnly") Or eqargs(1).EndsWith("Hidden") Or eqargs(1).EndsWith("Archive") Then
                                If eqargs(1).StartsWith("+") Then
                                    Dim Attrib As FileAttributes = [Enum].Parse(GetType(FileAttributes), eqargs(1).Remove(0, 1))
                                    If AddAttributeToFile(NeutralizedFilePath, Attrib) Then
                                        W(DoTranslation("Attribute has been added successfully."), True, ColTypes.Neutral, eqargs(1))
                                    Else
                                        W(DoTranslation("Failed to add attribute."), True, ColTypes.Neutral, eqargs(1))
                                    End If
                                ElseIf eqargs(1).StartsWith("-") Then
                                    Dim Attrib As FileAttributes = [Enum].Parse(GetType(FileAttributes), eqargs(1).Remove(0, 1))
                                    If RemoveAttributeFromFile(NeutralizedFilePath, Attrib) Then
                                        W(DoTranslation("Attribute has been removed successfully."), True, ColTypes.Neutral, eqargs(1))
                                    Else
                                        W(DoTranslation("Failed to remove attribute."), True, ColTypes.Neutral, eqargs(1))
                                    End If
                                End If
                            Else
                                W(DoTranslation("Attribute ""{0}"" is invalid."), True, ColTypes.Err, eqargs(1))
                            End If
                        Else
                            W(DoTranslation("File not found."), True, ColTypes.Err)
                        End If
                    End If

                Case "chdir"

                    If RequiredArgumentsProvided Then
                        Try
                            SetCurrDir(eqargs(0))
                        Catch sex As Security.SecurityException
                            Wdbg("E", "Security error: {0} ({1})", sex.Message, sex.PermissionType)
                            W(DoTranslation("You are unauthorized to set current directory to {0}: {1}"), True, ColTypes.Err, eqargs(0), sex.Message)
                            WStkTrc(sex)
                        Catch ptlex As PathTooLongException
                            Wdbg("I", "Directory length: {0}", NeutralizePath(eqargs(0)).Length)
                            W(DoTranslation("The path you've specified is too long."), True, ColTypes.Err)
                            WStkTrc(ptlex)
                        Catch ex As Exception
                            W(DoTranslation("Changing directory has failed: {0}"), True, ColTypes.Err, ex.Message)
                            WStkTrc(ex)
                        End Try
                    End If

                Case "chhostname"

                    If RequiredArgumentsProvided Then
                        If eqargs(0) = "" Then
                            W(DoTranslation("Blank host name."), True, ColTypes.Err)
                        ElseIf eqargs(0).IndexOfAny("[~`!@#$%^&*()-+=|{}':;.,<>/?]".ToCharArray) <> -1 Then
                            W(DoTranslation("Special characters are not allowed."), True, ColTypes.Err)
                        Else
                            W(DoTranslation("Changing from: {0} to {1}..."), True, ColTypes.Neutral, HName, eqargs(0))
                            ChangeHostname(eqargs(0))
                        End If
                    End If

                Case "chlang"

                    If RequiredArgumentsProvided Then
                        PromptForSetLang(eqargs(0))
                    End If

                Case "chmotd"

                    If eqargs?.Length > 0 Then
                        If strArgs = "" Then
                            W(DoTranslation("Blank message of the day."), True, ColTypes.Err)
                        Else
                            W(DoTranslation("Changing MOTD..."), True, ColTypes.Neutral)
                            SetMOTD(strArgs, MessageType.MOTD)
                        End If
                    Else
                        InitializeTextShell(paths("MOTD"))
                        W(DoTranslation("Changing MOTD..."), True, ColTypes.Neutral)
                        ReadMOTDFromFile(MessageType.MOTD)
                    End If

                Case "chmal"

                    If eqargs?.Length > 0 Then
                        If strArgs = "" Then
                            W(DoTranslation("Blank MAL After Login."), True, ColTypes.Err)
                        Else
                            W(DoTranslation("Changing MAL..."), True, ColTypes.Neutral)
                            SetMOTD(strArgs, MessageType.MAL)
                        End If
                    Else
                        InitializeTextShell(paths("MAL"))
                        W(DoTranslation("Changing MAL..."), True, ColTypes.Neutral)
                        ReadMOTDFromFile(MessageType.MAL)
                    End If

                Case "choice"

                    If RequiredArgumentsProvided Then
                        PromptChoice(strArgs, eqargs(0), eqargs(1))
                    End If

                Case "chpwd"

                    If RequiredArgumentsProvided Then
                        Try
                            If InStr(eqargs(3), " ") > 0 Then
                                W(DoTranslation("Spaces are not allowed."), True, ColTypes.Err)
                            ElseIf eqargs(3) = eqargs(2) Then
                                ChangePassword(eqargs(0), eqargs(1), eqargs(2))
                            ElseIf eqargs(3) <> eqargs(2) Then
                                W(DoTranslation("Passwords doesn't match."), True, ColTypes.Err)
                            End If
                        Catch ex As Exception
                            W(DoTranslation("Failed to change password of username: {0}"), True, ColTypes.Err, ex.Message)
                            WStkTrc(ex)
                        End Try
                    End If

                Case "chusrname"

                    If RequiredArgumentsProvided Then
                        ChangeUsername(eqargs(0), eqargs(1))
                        W(DoTranslation("Username has been changed to {0}!"), True, ColTypes.Neutral, eqargs(1))
                        If eqargs(0) = signedinusrnm Then
                            LogoutRequested = True
                        End If
                    End If

                Case "cls"

                    Console.Clear()

                Case "copy"

                    If RequiredArgumentsProvided Then
                        CopyFileOrDir(eqargs(0), eqargs(1))
                    End If

                Case "dirinfo"

                    If RequiredArgumentsProvided Then
                        For Each Dir As String In eqargs
                            Dim DirectoryPath As String = NeutralizePath(Dir)
                            Wdbg("I", "Neutralized directory path: {0} ({1})", DirectoryPath, Directory.Exists(DirectoryPath))
                            W(">> {0}", True, ColTypes.Stage, Dir)
                            If Directory.Exists(DirectoryPath) Then
                                Dim DirInfo As New DirectoryInfo(DirectoryPath)
                                W(DoTranslation("Name: {0}"), True, ColTypes.Neutral, DirInfo.Name)
                                W(DoTranslation("Full name: {0}"), True, ColTypes.Neutral, NeutralizePath(DirInfo.FullName))
                                W(DoTranslation("Size: {0}"), True, ColTypes.Neutral, GetAllSizesInFolder(DirInfo).FileSizeToString)
                                W(DoTranslation("Creation time: {0}"), True, ColTypes.Neutral, Render(DirInfo.CreationTime))
                                W(DoTranslation("Last access time: {0}"), True, ColTypes.Neutral, Render(DirInfo.LastAccessTime))
                                W(DoTranslation("Last write time: {0}"), True, ColTypes.Neutral, Render(DirInfo.LastWriteTime))
                                W(DoTranslation("Attributes: {0}"), True, ColTypes.Neutral, DirInfo.Attributes)
                                W(DoTranslation("Parent directory: {0}"), True, ColTypes.Neutral, NeutralizePath(DirInfo.Parent.FullName))
                            Else
                                W(DoTranslation("Can't get information about nonexistent directory."), True, ColTypes.Err)
                            End If
                        Next
                    End If

                Case "disconndbgdev"

                    If RequiredArgumentsProvided Then
                        DisconnectDbgDev(eqargs(0))
                        W(DoTranslation("Device {0} disconnected."), True, ColTypes.Neutral, eqargs(0))
                    End If

                Case "dismissnotif"

                    If RequiredArgumentsProvided Then
                        Dim NotifIndex As Integer = eqargs(0) - 1
                        If NotifDismiss(NotifIndex) Then
                            W(DoTranslation("Notification dismissed successfully."), True, ColTypes.Neutral)
                        Else
                            W(DoTranslation("Error trying to dismiss notification."), True, ColTypes.Err)
                        End If
                    End If

                Case "echo"

                    W(strArgs, True, ColTypes.Neutral)

                Case "edit"

                    If RequiredArgumentsProvided Then
                        eqargs(0) = NeutralizePath(eqargs(0))
                        Wdbg("I", "File path is {0} and .Exists is {0}", eqargs(0), File.Exists(eqargs(0)))
                        If File.Exists(eqargs(0)) Then
                            InitializeTextShell(eqargs(0))
                        Else
                            W(DoTranslation("File doesn't exist."), True, ColTypes.Err)
                        End If
                    End If

                Case "fileinfo"

                    If RequiredArgumentsProvided Then
                        For Each FileName As String In eqargs
                            Dim FilePath As String = NeutralizePath(FileName)
                            Wdbg("I", "Neutralized file path: {0} ({1})", FilePath, File.Exists(FilePath))
                            W(">> {0}", True, ColTypes.Stage, FileName)
                            If File.Exists(FilePath) Then
                                Dim FileInfo As New FileInfo(FilePath)
                                W(DoTranslation("Name: {0}"), True, ColTypes.Neutral, FileInfo.Name)
                                W(DoTranslation("Full name: {0}"), True, ColTypes.Neutral, NeutralizePath(FileInfo.FullName))
                                W(DoTranslation("File size: {0}"), True, ColTypes.Neutral, FileInfo.Length.FileSizeToString)
                                W(DoTranslation("Creation time: {0}"), True, ColTypes.Neutral, Render(FileInfo.CreationTime))
                                W(DoTranslation("Last access time: {0}"), True, ColTypes.Neutral, Render(FileInfo.LastAccessTime))
                                W(DoTranslation("Last write time: {0}"), True, ColTypes.Neutral, Render(FileInfo.LastWriteTime))
                                W(DoTranslation("Attributes: {0}"), True, ColTypes.Neutral, FileInfo.Attributes)
                                W(DoTranslation("Where to find: {0}"), True, ColTypes.Neutral, NeutralizePath(FileInfo.DirectoryName))
                            Else
                                W(DoTranslation("Can't get information about nonexistent file."), True, ColTypes.Err)
                            End If
                        Next
                    End If

                Case "firedevents"

                    WriteList(EventManager.FiredEvents)

                Case "ftp"

                    Try
                        If eqargs?.Length = 0 Or IsNothing(eqargs) Then
                            InitiateShell()
                        Else
                            InitiateShell(True, eqargs(0))
                        End If
                    Catch ftpex As Exceptions.FTPShellException
                        W(ftpex.Message, True, ColTypes.Err)
                    Catch ex As Exception
                        WStkTrc(ex)
                        W(DoTranslation("Unknown FTP shell error:") + " {0}", True, ColTypes.Err, ex.Message)
                    End Try

                Case "gettimeinfo"

                    If RequiredArgumentsProvided Then
                        Dim DateTimeInfo As Date
                        Dim UnixEpoch As New Date(1970, 1, 1, 0, 0, 0, 0)
                        If Date.TryParse(eqargs(0), DateTimeInfo) Then
                            W("-- " + DoTranslation("Information for") + " {0} --" + vbNewLine, True, ColTypes.Neutral, Render(DateTimeInfo))
                            W(DoTranslation("Milliseconds:") + " {0}", True, ColTypes.Neutral, DateTimeInfo.Millisecond)
                            W(DoTranslation("Seconds:") + " {0}", True, ColTypes.Neutral, DateTimeInfo.Second)
                            W(DoTranslation("Minutes:") + " {0}", True, ColTypes.Neutral, DateTimeInfo.Minute)
                            W(DoTranslation("Hours:") + " {0}", True, ColTypes.Neutral, DateTimeInfo.Hour)
                            W(DoTranslation("Days:") + " {0}", True, ColTypes.Neutral, DateTimeInfo.Day)
                            W(DoTranslation("Months:") + " {0}", True, ColTypes.Neutral, DateTimeInfo.Month)
                            W(DoTranslation("Year:") + " {0}" + vbNewLine, True, ColTypes.Neutral, DateTimeInfo.Year)
                            W(DoTranslation("Date:") + " {0}", True, ColTypes.Neutral, RenderDate(DateTimeInfo))
                            W(DoTranslation("Time:") + " {0}" + vbNewLine, True, ColTypes.Neutral, RenderTime(DateTimeInfo))
                            W(DoTranslation("Day of Year:") + " {0}", True, ColTypes.Neutral, DateTimeInfo.DayOfYear)
                            W(DoTranslation("Day of Week:") + " {0}" + vbNewLine, True, ColTypes.Neutral, DateTimeInfo.DayOfWeek.ToString)
                            W(DoTranslation("Binary:") + " {0}", True, ColTypes.Neutral, DateTimeInfo.ToBinary)
                            W(DoTranslation("Local Time:") + " {0}", True, ColTypes.Neutral, Render(DateTimeInfo.ToLocalTime))
                            W(DoTranslation("Universal Time:") + " {0}", True, ColTypes.Neutral, Render(DateTimeInfo.ToUniversalTime))
                            W(DoTranslation("Unix Time:") + " {0}", True, ColTypes.Neutral, (DateTimeInfo - UnixEpoch).TotalSeconds)
                        Else
                            W(DoTranslation("Failed to parse date information for") + " {0}. " + DoTranslation("Ensure that the format is correct."), True, ColTypes.Err, eqargs(0))
                        End If
                    End If

                Case "get"
#Disable Warning BC42104
                    If RequiredArgumentsProvided Then
                        Dim RetryCount As Integer = 1
                        Dim URL As String = eqargs(0)
                        Wdbg("I", "URL: {0}", URL)
                        While Not RetryCount > DRetries
                            Try
                                If Not (URL.StartsWith("ftp://") Or URL.StartsWith("ftps://") Or URL.StartsWith("ftpes://")) Then
                                    If Not URL.StartsWith(" ") Then
                                        Dim Credentials As NetworkCredential
                                        If eqargs.Count > 1 Then 'Username specified
                                            Credentials = New NetworkCredential With {
                                                .UserName = eqargs(1)
                                            }
                                            W(DoTranslation("Enter password: "), False, ColTypes.Input)
                                            Credentials.Password = ReadLineNoInput("*")
                                            Console.WriteLine()
                                        End If
                                        W(DoTranslation("Downloading from {0}..."), True, ColTypes.Neutral, URL)
                                        If DownloadFile(eqargs(0), Credentials) Then
                                            W(DoTranslation("Download has completed."), True, ColTypes.Neutral)
                                        End If
                                    Else
                                        W(DoTranslation("Specify the address"), True, ColTypes.Err)
                                    End If
                                Else
                                    W(DoTranslation("Please use ""ftp"" if you are going to download files from the FTP server."), True, ColTypes.Err)
                                End If
                                Exit Sub
                            Catch ex As Exception
                                DFinish = False
                                W(DoTranslation("Download failed in try {0}: {1}"), True, ColTypes.Err, RetryCount, ex.Message)
                                RetryCount += 1
                                Wdbg("I", "Try count: {0}", RetryCount)
                                WStkTrc(ex)
                            End Try
                        End While
                    End If
#Enable Warning BC42104
                Case "hwinfo"

                    'CPU information
                    W("- " + DoTranslation("CPU information:"), True, ColTypes.Neutral)
                    For Each Processor As String In HardwareInfo.Hardware.CPU.Keys
                        W("  - " + DoTranslation("Processor name:") + " {0}", True, ColTypes.Neutral, Processor)
                        W("  - " + DoTranslation("Processor speed:") + " {0}", True, ColTypes.Neutral, HardwareInfo.Hardware.CPU(Processor).Speed)
                        W("  - " + DoTranslation("Processor bits:") + " {0}-bit", True, ColTypes.Neutral, HardwareInfo.Hardware.CPU(Processor).Bits)
                        W("  - " + DoTranslation("Processor rev:") + " {0}", True, ColTypes.Neutral, HardwareInfo.Hardware.CPU(Processor).CPURev)
                        W("  - " + DoTranslation("Processor topology:") + " {0}", True, ColTypes.Neutral, HardwareInfo.Hardware.CPU(Processor).Topology)
                        W("  - " + DoTranslation("Processor type:") + " {0}", True, ColTypes.Neutral, HardwareInfo.Hardware.CPU(Processor).Type)
                        W("  - " + DoTranslation("Processor milestone:") + " {0}", True, ColTypes.Neutral, HardwareInfo.Hardware.CPU(Processor).Milestone)
                        W("  - " + DoTranslation("Processor BogoMips:") + " {0}", True, ColTypes.Neutral, HardwareInfo.Hardware.CPU(Processor).CPUBogoMips)
                        W("  - " + DoTranslation("Processor L2 cache size:") + " {0}", True, ColTypes.Neutral, HardwareInfo.Hardware.CPU(Processor).L2)
                        W("  - " + DoTranslation("Processor L3 cache size:") + " {0}", True, ColTypes.Neutral, HardwareInfo.Hardware.CPU(Processor).L3)
                        W("  - " + DoTranslation("Processor features:") + " {0}", True, ColTypes.Neutral, Join(HardwareInfo.Hardware.CPU(Processor).Flags))
                        Console.WriteLine()
                    Next

                    'HDD information
                    W("- " + DoTranslation("HDD information:"), True, ColTypes.Neutral)
                    For Each Drive As String In HardwareInfo.Hardware.HDD.Keys
                        W("  - " + DoTranslation("Drive name:") + " {0}", True, ColTypes.Neutral, Drive)
                        W("  - " + DoTranslation("Drive model:") + " {0}", True, ColTypes.Neutral, HardwareInfo.Hardware.HDD(Drive).Model)
                        W("  - " + DoTranslation("Drive vendor:") + " {0}", True, ColTypes.Neutral, HardwareInfo.Hardware.HDD(Drive).Vendor)
                        W("  - " + DoTranslation("Drive speed:") + " {0}", True, ColTypes.Neutral, HardwareInfo.Hardware.HDD(Drive).Speed)
                        W("  - " + DoTranslation("Drive size:") + " {0}", True, ColTypes.Neutral, If(IsOnUnix(), HardwareInfo.Hardware.HDD(Drive).Size, CLng(HardwareInfo.Hardware.HDD(Drive).Size).FileSizeToString))
                        W("  - " + DoTranslation("Drive serial:") + " {0}", True, ColTypes.Neutral, HardwareInfo.Hardware.HDD(Drive).Serial)
                        W("  - " + DoTranslation("Drive ID:") + " {0}", True, ColTypes.Neutral, HardwareInfo.Hardware.HDD(Drive).ID)
                        W("  - " + DoTranslation("Drive partition count:") + " {0}", True, ColTypes.Neutral, HardwareInfo.Hardware.HDD(Drive).Partitions.Count)
                        For PartitionIndex As Integer = 0 To HardwareInfo.Hardware.HDD(Drive).Partitions.Count - 1
                            W("    - [{0}] " + DoTranslation("Partition ID:") + " {1}", True, ColTypes.Neutral, PartitionIndex + 1, HardwareInfo.Hardware.HDD(Drive).Partitions.Values(PartitionIndex).ID)
                            W("    - [{0}] " + DoTranslation("Partition filesystem:") + " {1}", True, ColTypes.Neutral, PartitionIndex + 1, HardwareInfo.Hardware.HDD(Drive).Partitions.Values(PartitionIndex).FileSystem)
                            W("    - [{0}] " + DoTranslation("Partition size:") + " {1}", True, ColTypes.Neutral, PartitionIndex + 1, If(IsOnUnix(), HardwareInfo.Hardware.HDD(Drive).Partitions.Values(PartitionIndex).Size, CLng(HardwareInfo.Hardware.HDD(Drive).Partitions.Values(PartitionIndex).Size).FileSizeToString))
                            W("    - [{0}] " + DoTranslation("Partition used:") + " {1}", True, ColTypes.Neutral, PartitionIndex + 1, HardwareInfo.Hardware.HDD(Drive).Partitions.Values(PartitionIndex).Used)
                        Next
                        Console.WriteLine()
                    Next

                    'GPU information
                    W(" - " + DoTranslation("GPU information:"), True, ColTypes.Neutral)
                    For Each GPU As String In HardwareInfo.Hardware.GPU.Keys
                        W("  - " + DoTranslation("GPU name:") + " {0}", True, ColTypes.Neutral, GPU)
                        W("  - " + DoTranslation("GPU driver:") + " {0}", True, ColTypes.Neutral, HardwareInfo.Hardware.GPU(GPU).Driver)
                        W("  - " + DoTranslation("GPU version:") + " {0}", True, ColTypes.Neutral, HardwareInfo.Hardware.GPU(GPU).DriverVersion)
                    Next

                    'RAM information
                    W("- " + DoTranslation("RAM information:"), True, ColTypes.Neutral)
                    W("  - " + DoTranslation("RAM free:") + " {0}", True, ColTypes.Neutral, If(IsOnUnix(), HardwareInfo.Hardware.RAM.FreeMemory, CLng(HardwareInfo.Hardware.RAM.FreeMemory).FileSizeToString))
                    W("  - " + DoTranslation("RAM total:") + " {0}", True, ColTypes.Neutral, If(IsOnUnix(), HardwareInfo.Hardware.RAM.TotalMemory, CLng(HardwareInfo.Hardware.RAM.TotalMemory).FileSizeToString))
                    W("  - " + DoTranslation("RAM used:") + " {0}", True, ColTypes.Neutral, If(IsOnUnix(), HardwareInfo.Hardware.RAM.UsedMemory, CLng(HardwareInfo.Hardware.RAM.UsedMemory).FileSizeToString))
                    Console.WriteLine()

                Case "input"

                    If RequiredArgumentsProvided Then
                        PromptInput(strArgs.Replace(eqargs(0) + " ", ""), eqargs(0))
                    End If

                Case "lockscreen"

                    LockScreen()

                Case "list"

                    'Lists folders and files
                    If eqargs?.Length = 0 Or IsNothing(eqargs) Then
                        List(CurrDir)
                    Else
                        For Each Directory As String In eqargs
                            Dim direct As String = NeutralizePath(Directory)
                            List(direct)
                        Next
                    End If

                Case "loteresp"

                    InitializeLoteresp()

                Case "lsmail"

                    If KeepAlive Then
                        OpenMailShell(Mail_Authentication.Domain)
                    Else
                        If eqargs?.Length = 0 Or IsNothing(eqargs) Then
                            PromptUser()
                        ElseIf Not eqargs(0) = "" Then
                            PromptPassword(eqargs(0))
                        End If
                    End If

                Case "logout"

                    'Logs out of the user
                    LogoutRequested = True

                Case "lsdbgdev"

                    For Each DebugDevice As String In DebugDevices.Values
                        W($"- {DebugDevice}", True, ColTypes.ListEntry)
                    Next

                Case "mathbee"

                    InitializeSolver()

                Case "md"

                    If RequiredArgumentsProvided Then
                        'Create directory
                        MakeDirectory(eqargs(0))
                    End If

                Case "mkfile"

                    If RequiredArgumentsProvided Then
                        MakeFile(eqargs(0))
                    End If

                Case "mktheme"

                    If RequiredArgumentsProvided Then
                        StartThemeStudio(eqargs(0))
                    End If

                Case "move"

                    If RequiredArgumentsProvided Then
                        MoveFileOrDir(eqargs(0), eqargs(1))
                    End If

                Case "netinfo"

                    PrintAdapterProperties()

                Case "perm"

                    If RequiredArgumentsProvided Then
                        Permission([Enum].Parse(GetType(PermissionType), eqargs(1)), eqargs(0), [Enum].Parse(GetType(PermissionManagementMode), eqargs(2)))
                    End If

                Case "ping"

                    If RequiredArgumentsProvided Then
                        'If the pinged address is actually a number of times
                        Dim PingTimes As Integer = 4
                        Dim StepsToSkip As Integer = 0
                        If IsNumeric(eqargs(0)) Then
                            Wdbg("I", "eqargs(0) is numeric. Assuming number of times: {0}", eqargs(0))
                            PingTimes = eqargs(0)
                            StepsToSkip = 1
                        End If
                        For Each PingedAddress As String In eqargs.Skip(StepsToSkip)
                            If PingedAddress <> "" Then
                                W(">> {0}", True, ColTypes.Stage, PingedAddress)
                                For CurrentTime As Integer = 1 To PingTimes
                                    Try
                                        Dim PingReplied As PingReply = PingAddress(PingedAddress)
                                        If PingReplied.Status = IPStatus.Success Then
                                            W("[{1}] " + DoTranslation("Ping succeeded in {0} ms."), True, ColTypes.Neutral, PingReplied.RoundtripTime, CurrentTime)
                                        Else
                                            W("[{2}] " + DoTranslation("Failed to ping {0}: {1}"), True, ColTypes.Err, PingedAddress, PingReplied.Status, CurrentTime)
                                        End If
                                    Catch ex As Exception
                                        W("[{2}] " + DoTranslation("Failed to ping {0}: {1}"), True, ColTypes.Err, PingedAddress, ex.Message, CurrentTime)
                                        WStkTrc(ex)
                                    End Try
                                Next
                            Else
                                W(DoTranslation("Address may not be empty."), True, ColTypes.Err)
                            End If
                        Next
                    End If

                Case "put"
#Disable Warning BC42104
                    If RequiredArgumentsProvided Then
                        Dim RetryCount As Integer = 1
                        Dim FileName As String = NeutralizePath(eqargs(0))
                        Dim URL As String = eqargs(1)
                        Wdbg("I", "URL: {0}", URL)
                        While Not RetryCount > URetries
                            Try
                                If Not (URL.StartsWith("ftp://") Or URL.StartsWith("ftps://") Or URL.StartsWith("ftpes://")) Then
                                    If Not URL.StartsWith(" ") Then
                                        Dim Credentials As NetworkCredential
                                        If eqargs.Count > 2 Then 'Username specified
                                            Credentials = New NetworkCredential With {
                                                .UserName = eqargs(2)
                                            }
                                            W(DoTranslation("Enter password: "), False, ColTypes.Input)
                                            Credentials.Password = ReadLineNoInput("*")
                                            Console.WriteLine()
                                        End If
                                        W(DoTranslation("Uploading {0} to {1}..."), True, ColTypes.Neutral, FileName, URL)
                                        If UploadFile(FileName, URL, Credentials) Then
                                            W(DoTranslation("Upload has completed."), True, ColTypes.Neutral)
                                        End If
                                    Else
                                        W(DoTranslation("Specify the address"), True, ColTypes.Err)
                                    End If
                                Else
                                    W(DoTranslation("Please use ""ftp"" if you are going to upload files to the FTP server."), True, ColTypes.Err)
                                End If
                                Exit Sub
                            Catch ex As Exception
                                UFinish = False
                                W(DoTranslation("Upload failed in try {0}: {1}"), True, ColTypes.Err, RetryCount, ex.Message)
                                RetryCount += 1
                                Wdbg("I", "Try count: {0}", RetryCount)
                                WStkTrc(ex)
                            End Try
                        End While
                    End If
#Enable Warning BC42104
                Case "reloadconfig"

                    'Reload configuration
                    ReloadConfig()
                    W(DoTranslation("Configuration reloaded. You might need to reboot the kernel for some changes to take effect."), True, ColTypes.Neutral)

                Case "reboot"

                    'Reboot the simulated system
                    If Not eqargs?.Length = 0 Then
                        If eqargs(0) = "safe" Then
                            PowerManage("rebootsafe")
                        ElseIf eqargs(0) <> "" Then
                            If eqargs?.Length > 1 Then
                                PowerManage("remoterestart", eqargs(0), eqargs(1))
                            Else
                                PowerManage("remoterestart", eqargs(0))
                            End If
                        Else
                            PowerManage("reboot")
                        End If
                    Else
                        PowerManage("reboot")
                    End If

                Case "reloadmods"

                    'Reload mods
                    If Not SafeMode Then
                        ReloadMods()
                        W(DoTranslation("Mods reloaded."), True, ColTypes.Neutral)
                    Else
                        W(DoTranslation("Reloading not allowed in safe mode."), True, ColTypes.Err)
                    End If

                Case "reloadsaver"

                    If RequiredArgumentsProvided Then
                        If Not SafeMode Then
                            CompileCustom(eqargs(0))
                        Else
                            W(DoTranslation("Reloading not allowed in safe mode."), True, ColTypes.Err)
                        End If
                    End If

                Case "reportbug"

                    PromptForBug()

                Case "rexec"

                    If RequiredArgumentsProvided Then
                        If eqargs?.Length = 2 Then
                            SendCommand("<Request:Exec>(" + eqargs(1) + ")", eqargs(0))
                        Else
                            SendCommand("<Request:Exec>(" + eqargs(2) + ")", eqargs(0), eqargs(1))
                        End If
                    End If

                Case "rdebug"

                    If DebugMode Then
                        If RDebugThread.IsAlive Then
                            StartRDebugThread(False)
                        Else
                            StartRDebugThread(True)
                        End If
                    Else
                        W(DoTranslation("Debugging not enabled."), True, ColTypes.Err)
                    End If

                Case "rm"

                    If RequiredArgumentsProvided Then
                        For Each Path As String In eqargs
                            Dim NeutPath As String = NeutralizePath(Path)
                            If File.Exists(NeutPath) Then
                                Wdbg("I", "{0} is a file. Removing...", Path)
                                RemoveFile(Path)
                            ElseIf Directory.Exists(NeutPath) Then
                                Wdbg("I", "{0} is a folder. Removing...", Path)
                                RemoveDirectory(Path)
                            Else
                                Wdbg("W", "Trying to remove {0} which is not found.", Path)
                                W(DoTranslation("Can't remove {0} because it doesn't exist."), True, ColTypes.Err, Path)
                            End If
                        Next
                    End If

                Case "rmuser"

                    If RequiredArgumentsProvided Then
                        RemoveUser(eqargs(0))
                        W(DoTranslation("User {0} removed."), True, ColTypes.Neutral, eqargs(0))
                    End If

                Case "savescreen"

                    If Not eqargs?.Length = 0 Then
                        ShowSavers(eqargs(0))
                    Else
                        ShowSavers(defSaverName)
                    End If

                Case "search"

                    If RequiredArgumentsProvided Then
                        Try
                            Dim Matches As List(Of String) = SearchFileForStringRegexp(eqargs(1), New RegularExpressions.Regex(eqargs(0), RegularExpressions.RegexOptions.IgnoreCase))
                            For Each Match As String In Matches
                                W(Match, True, ColTypes.Neutral)
                            Next
                        Catch ex As Exception
                            Wdbg("E", "Error trying to search {0} for {1}", eqargs(0), eqargs(1))
                            WStkTrc(ex)
                            W(DoTranslation("Searching {0} for {1} failed.") + " {2}", True, ColTypes.Err, eqargs(0), eqargs(1), ex.Message)
                        End Try
                    End If

                Case "searchword"

                    If RequiredArgumentsProvided Then
                        Try
                            Dim Matches As List(Of String) = SearchFileForString(eqargs(1), eqargs(0))
                            For Each Match As String In Matches
                                W(Match, True, ColTypes.Neutral)
                            Next
                        Catch ex As Exception
                            Wdbg("E", "Error trying to search {0} for {1}", eqargs(0), eqargs(1))
                            WStkTrc(ex)
                            W(DoTranslation("Searching {0} for {1} failed.") + " {2}", True, ColTypes.Err, eqargs(0), eqargs(1), ex.Message)
                        End Try
                    End If

                Case "savecurrdir"

                    SaveCurrDir()

                Case "setsaver"

                    Dim modPath As String = paths("Mods")
                    If RequiredArgumentsProvided Then
                        If ScrnSvrdb.ContainsKey(strArgs) Then
                            SetDefaultScreensaver(strArgs)
                            If ScrnSvrdb(strArgs) Then
                                W(DoTranslation("{0} is set to default screensaver."), True, ColTypes.Neutral, strArgs)
                            Else
                                W(DoTranslation("{0} is no longer set to default screensaver."), True, ColTypes.Neutral, strArgs)
                            End If
                        Else
                            If FileIO.FileSystem.FileExists($"{modPath}{strArgs}") And Not SafeMode Then
                                SetDefaultScreensaver(strArgs)
                                If ScrnSvrdb(strArgs) Then
                                    W(DoTranslation("{0} is set to default screensaver."), True, ColTypes.Neutral, strArgs)
                                Else
                                    W(DoTranslation("{0} is no longer set to default screensaver."), True, ColTypes.Neutral, strArgs)
                                End If
                            Else
                                W(DoTranslation("Screensaver {0} not found."), True, ColTypes.Err, strArgs)
                            End If
                        End If
                    End If

                Case "setthemes"

                    If RequiredArgumentsProvided Then
                        If ColoredShell = True Then
                            Dim ThemePath As String = NeutralizePath(eqargs(0))
                            If File.Exists(ThemePath) Then
                                ApplyThemeFromFile(ThemePath)
                            Else
                                ApplyThemeFromResources(eqargs(0))
                            End If
                        Else
                            W(DoTranslation("Colors are not available. Turn on colored shell in the kernel config."), True, ColTypes.Neutral)
                        End If
                    End If

                Case "settings"

                    OpenMainPage()

                Case "set"

                    If RequiredArgumentsProvided Then
                        SetVariable(eqargs(0), eqargs(1))
                    End If

                Case "sftp"

                    Try
                        If eqargs?.Length = 0 Or IsNothing(eqargs) Then
                            SFTPInitiateShell()
                        Else
                            SFTPInitiateShell(True, eqargs(0))
                        End If
                    Catch sftpex As Exceptions.SFTPShellException
                        W(sftpex.Message, True, ColTypes.Err)
                    Catch ex As Exception
                        WStkTrc(ex)
                        W(DoTranslation("Unknown SFTP shell error:") + " {0}", True, ColTypes.Err, ex.Message)
                    End Try

                Case "shownotifs"

                    Dim Count As Integer = 1
                    If Not NotifRecents.Count = 0 Then
                        For Each Notif As Notification In NotifRecents
                            W($"{Count}: {Notif.Title}" + vbNewLine +
                              $"{Count}: {Notif.Desc}" + vbNewLine +
                              $"{Count}: {Notif.Priority}", True, ColTypes.Neutral)
                            Count += 1
                        Next
                    Else
                        W(DoTranslation("No recent notifications"), True, ColTypes.Neutral)
                    End If

                Case "showtd"

                    ShowCurrentTimes()

                Case "showtdzone"

                    If RequiredArgumentsProvided Then
                        InitTimesInZones()
                        Dim DoneFlag As Boolean = False
                        If zoneTimes.Keys.Contains(strArgs) Then
                            DoneFlag = True
                            ShowTimesInZones(strArgs)
                        End If
                        If DoneFlag = False Then
                            If eqargs(0) = "all" Then
                                ShowTimesInZones()
                            Else
                                W(DoTranslation("Timezone is specified incorrectly."), True, ColTypes.Err)
                            End If
                        End If
                    End If

                Case "shutdown"

                    'Shuts down the simulated system
                    If Not eqargs?.Length = 0 Then
                        If eqargs?.Length = 1 Then
                            PowerManage("remoteshutdown", eqargs(0))
                        Else
                            PowerManage("remoteshutdown", eqargs(0), eqargs(1))
                        End If
                    Else
                        PowerManage("shutdown")
                    End If

                Case "speedpress"

                    If RequiredArgumentsProvided Then
                        If eqargs(0) = "e" Or eqargs(0) = "m" Or eqargs(0) = "h" Then
                            InitializeSpeedPress(eqargs(0))
                        Else
                            W(DoTranslation("Invalid difficulty") + " {0}", True, ColTypes.Err, eqargs(0))
                        End If
                    End If

                Case "spellbee"

                    InitializeWords()

                Case "sshell"

                    If RequiredArgumentsProvided Then
                        Dim AddressDelimiter() As String = eqargs(0).Split(":")
                        Dim Address As String = AddressDelimiter(0)
                        If AddressDelimiter.Length > 1 Then
                            Dim Port As Integer = AddressDelimiter(1)
                            InitializeSSH(Address, Port, eqargs(1), ConnectionType.Shell)
                        Else
                            InitializeSSH(Address, 22, eqargs(1), ConnectionType.Shell)
                        End If
                    End If

                Case "sshcmd"

                    If RequiredArgumentsProvided Then
                        Dim AddressDelimiter() As String = eqargs(0).Split(":")
                        Dim Address As String = AddressDelimiter(0)
                        If AddressDelimiter.Length > 1 Then
                            Dim Port As Integer = AddressDelimiter(1)
                            InitializeSSH(Address, Port, eqargs(1), ConnectionType.Command, eqargs(2))
                        Else
                            InitializeSSH(Address, 22, eqargs(1), ConnectionType.Command, eqargs(2))
                        End If
                    End If

                Case "sumfile"

                    If RequiredArgumentsProvided Then
                        Dim file As String = NeutralizePath(eqargs(1))
                        If IO.File.Exists(file) Then
                            If eqargs(0) = "SHA512" Then
                                Dim spent As New Stopwatch
                                spent.Start() 'Time when you're on a breakpoint is counted
                                W(GetEncryptedFile(file, Algorithms.SHA512), True, ColTypes.Neutral)
                                W(DoTranslation("Time spent: {0} milliseconds"), True, ColTypes.Neutral, spent.ElapsedMilliseconds)
                                spent.Stop()
                            ElseIf eqargs(0) = "SHA256" Then
                                Dim spent As New Stopwatch
                                spent.Start() 'Time when you're on a breakpoint is counted
                                W(GetEncryptedFile(file, Algorithms.SHA256), True, ColTypes.Neutral)
                                W(DoTranslation("Time spent: {0} milliseconds"), True, ColTypes.Neutral, spent.ElapsedMilliseconds)
                                spent.Stop()
                            ElseIf eqargs(0) = "SHA1" Then
                                Dim spent As New Stopwatch
                                spent.Start() 'Time when you're on a breakpoint is counted
                                W(GetEncryptedFile(file, Algorithms.SHA1), True, ColTypes.Neutral)
                                W(DoTranslation("Time spent: {0} milliseconds"), True, ColTypes.Neutral, spent.ElapsedMilliseconds)
                                spent.Stop()
                            ElseIf eqargs(0) = "MD5" Then
                                Dim spent As New Stopwatch
                                spent.Start() 'Time when you're on a breakpoint is counted
                                W(GetEncryptedFile(file, Algorithms.MD5), True, ColTypes.Neutral)
                                W(DoTranslation("Time spent: {0} milliseconds"), True, ColTypes.Neutral, spent.ElapsedMilliseconds)
                                spent.Stop()
                            ElseIf eqargs(0) = "all" Then
                                For Each Algorithm As String In [Enum].GetNames(GetType(Algorithms))
                                    Dim AlgorithmEnum As Algorithms = [Enum].Parse(GetType(Algorithms), Algorithm)
                                    Dim spent As New Stopwatch
                                    spent.Start() 'Time when you're on a breakpoint is counted
                                    W("{0} ({1})", True, ColTypes.Neutral, GetEncryptedFile(file, AlgorithmEnum), AlgorithmEnum)
                                    W(DoTranslation("Time spent: {0} milliseconds"), True, ColTypes.Neutral, spent.ElapsedMilliseconds)
                                    spent.Stop()
                                Next
                            Else
                                W(DoTranslation("Invalid encryption algorithm."), True, ColTypes.Err)
                            End If
                        Else
                            W(DoTranslation("{0} is not found."), True, ColTypes.Err, file)
                        End If
                    End If

                Case "sumfiles"

                    If RequiredArgumentsProvided Then
                        Dim folder As String = NeutralizePath(eqargs(1))
                        Dim out As String = ""
                        Dim FileBuilder As New StringBuilder
                        If Not eqargs.Length < 3 Then
                            out = NeutralizePath(eqargs(2))
                        End If
                        If Directory.Exists(folder) Then
                            For Each file As String In Directory.EnumerateFiles(folder, "*", IO.SearchOption.TopDirectoryOnly)
                                file = NeutralizePath(file)
                                W(">> {0}", True, ColTypes.Stage, file)
                                If eqargs(0) = "SHA512" Then
                                    Dim spent As New Stopwatch
                                    spent.Start() 'Time when you're on a breakpoint is counted
                                    Dim encrypted As String = GetEncryptedFile(file, Algorithms.SHA512)
                                    W(encrypted, True, ColTypes.Neutral)
                                    W(DoTranslation("Time spent: {0} milliseconds"), True, ColTypes.Neutral, spent.ElapsedMilliseconds)
                                    FileBuilder.AppendLine($"- {file}: {encrypted} ({eqargs(0)})")
                                    spent.Stop()
                                ElseIf eqargs(0) = "SHA256" Then
                                    Dim spent As New Stopwatch
                                    spent.Start() 'Time when you're on a breakpoint is counted
                                    Dim encrypted As String = GetEncryptedFile(file, Algorithms.SHA256)
                                    W(encrypted, True, ColTypes.Neutral)
                                    W(DoTranslation("Time spent: {0} milliseconds"), True, ColTypes.Neutral, spent.ElapsedMilliseconds)
                                    FileBuilder.AppendLine($"- {file}: {encrypted} ({eqargs(0)})")
                                    spent.Stop()
                                ElseIf eqargs(0) = "SHA1" Then
                                    Dim spent As New Stopwatch
                                    spent.Start() 'Time when you're on a breakpoint is counted
                                    Dim encrypted As String = GetEncryptedFile(file, Algorithms.SHA1)
                                    W(encrypted, True, ColTypes.Neutral)
                                    W(DoTranslation("Time spent: {0} milliseconds"), True, ColTypes.Neutral, spent.ElapsedMilliseconds)
                                    FileBuilder.AppendLine($"- {file}: {encrypted} ({eqargs(0)})")
                                    spent.Stop()
                                ElseIf eqargs(0) = "MD5" Then
                                    Dim spent As New Stopwatch
                                    spent.Start() 'Time when you're on a breakpoint is counted
                                    Dim encrypted As String = GetEncryptedFile(file, Algorithms.MD5)
                                    W(encrypted, True, ColTypes.Neutral)
                                    W(DoTranslation("Time spent: {0} milliseconds"), True, ColTypes.Neutral, spent.ElapsedMilliseconds)
                                    FileBuilder.AppendLine($"- {file}: {encrypted} ({eqargs(0)})")
                                    spent.Stop()
                                ElseIf eqargs(0) = "all" Then
                                    For Each Algorithm As String In [Enum].GetNames(GetType(Algorithms))
                                        Dim AlgorithmEnum As Algorithms = [Enum].Parse(GetType(Algorithms), Algorithm)
                                        Dim spent As New Stopwatch
                                        spent.Start() 'Time when you're on a breakpoint is counted
                                        Dim encrypted As String = GetEncryptedFile(file, AlgorithmEnum)
                                        W("{0} ({1})", True, ColTypes.Neutral, encrypted, AlgorithmEnum)
                                        W(DoTranslation("Time spent: {0} milliseconds"), True, ColTypes.Neutral, spent.ElapsedMilliseconds)
                                        FileBuilder.AppendLine($"- {file}: {encrypted} ({AlgorithmEnum})")
                                        spent.Stop()
                                    Next
                                Else
                                    W(DoTranslation("Invalid encryption algorithm."), True, ColTypes.Err)
                                    Exit For
                                End If
                                Console.WriteLine()
                            Next
                            If Not out = "" Then
                                Dim FStream As New StreamWriter(out)
                                FStream.Write(FileBuilder.ToString)
                                FStream.Flush()
                            End If
                        Else
                            W(DoTranslation("{0} is not found."), True, ColTypes.Err, folder)
                        End If
                    End If

                Case "sysinfo"

                    'Shows system information
                    W(DoTranslation("[ Kernel settings (Running on {0}) ]"), True, ColTypes.ListEntry, Environment.OSVersion.ToString)

                    'Kernel section
                    W(vbNewLine + DoTranslation("Kernel Version:") + " {0}" + vbNewLine +
                                  DoTranslation("Debug Mode:") + " {1}" + vbNewLine +
                                  DoTranslation("Colored Shell:") + " {2}" + vbNewLine +
                                  DoTranslation("Arguments on Boot:") + " {3}" + vbNewLine +
                                  DoTranslation("Help command simplified:") + " {4}" + vbNewLine +
                                  DoTranslation("MOTD on Login:") + " {5}" + vbNewLine +
                                  DoTranslation("Time/Date on corner:") + " {6}" + vbNewLine, True, ColTypes.Neutral, KernelVersion, DebugMode.ToString, ColoredShell.ToString, argsOnBoot.ToString, simHelp.ToString, showMOTD.ToString, CornerTD.ToString)

                    'Hardware section
                    W(DoTranslation("[ Hardware settings ]{0}"), True, ColTypes.ListEntry, vbNewLine)
                    ListHardware()
                    W(DoTranslation("Use ""hwinfo"" for extended information about hardware."), True, ColTypes.Neutral)

                    'User section
                    W(DoTranslation("{0}[ User settings ]"), True, ColTypes.ListEntry, vbNewLine)
                    W(vbNewLine + DoTranslation("Current user name:") + " {0}" + vbNewLine +
                                  DoTranslation("Current host name:") + " {1}" + vbNewLine +
                                  DoTranslation("Available usernames:") + " {2}", True, ColTypes.Neutral, signedinusrnm, HName, String.Join(", ", userword.Keys))

                    'Messages Section
                    W(vbNewLine + "[ MOTD ]", True, ColTypes.ListEntry)
                    W(vbNewLine + ProbePlaces(MOTDMessage), True, ColTypes.Neutral)
                    W(vbNewLine + "[ MAL ]", True, ColTypes.ListEntry)
                    W(vbNewLine + ProbePlaces(MAL), True, ColTypes.Neutral)

                Case "unblockdbgdev"

                    If RequiredArgumentsProvided Then
                        If RDebugBlocked.Contains(eqargs(0)) Then
                            If RemoveFromBlockList(eqargs(0)) Then
                                W(DoTranslation("{0} can now join remote debug again."), True, ColTypes.Neutral, eqargs(0))
                            Else
                                W(DoTranslation("Failed to unblock {0}."), True, ColTypes.Neutral, eqargs(0))
                            End If
                        Else
                            W(DoTranslation("{0} is not blocked yet."), True, ColTypes.Neutral, eqargs(0))
                        End If
                    End If

                Case "unzip"

                    If RequiredArgumentsProvided And eqargs?.Length = 1 Then
                        Dim ZipArchiveName As String = NeutralizePath(eqargs(0))
                        ZipFile.ExtractToDirectory(ZipArchiveName, CurrDir)
                    ElseIf RequiredArgumentsProvided And eqargs?.Length > 1 Then
                        Dim ZipArchiveName As String = NeutralizePath(eqargs(0))
                        Dim Destination As String = If(Not eqargs(1) = "-createdir", NeutralizePath(eqargs(1)), "")
                        If eqargs?.Contains("-createdir") Then
                            Destination = $"{If(Not eqargs(1) = "-createdir", NeutralizePath(eqargs(1)), "")}/{If(Not eqargs(1) = "-createdir", Path.GetFileNameWithoutExtension(ZipArchiveName), NeutralizePath(Path.GetFileNameWithoutExtension(ZipArchiveName)))}"
                            If Destination(0) = "/" Then Destination = Destination.RemoveLetter(0)
                        End If
                        ZipFile.ExtractToDirectory(ZipArchiveName, Destination)
                    End If

                Case "update"

#If SPECIFIER = "REL" Then
                    CheckKernelUpdates()
#Else
                    W(DoTranslation("Checking for updates is disabled because you're running a development version."), True, ColTypes.Err)
#End If

                Case "usermanual"

                    Process.Start("https://github.com/EoflaOE/Kernel-Simulator/wiki")

                Case "verify"

                    If RequiredArgumentsProvided Then
                        Try
                            Dim HashFile As String = NeutralizePath(eqargs(2))
                            If File.Exists(HashFile) Then
                                If VerifyHashFromHashesFile(eqargs(3), [Enum].Parse(GetType(Algorithms), eqargs(0)), eqargs(2), eqargs(1)) Then
                                    W(DoTranslation("Hashes match."), True, ColTypes.Neutral)
                                Else
                                    W(DoTranslation("Hashes don't match."), True, ColTypes.Warning)
                                End If
                            Else
                                If VerifyHashFromHash(eqargs(3), [Enum].Parse(GetType(Algorithms), eqargs(0)), eqargs(2), eqargs(1)) Then
                                    W(DoTranslation("Hashes match."), True, ColTypes.Neutral)
                                Else
                                    W(DoTranslation("Hashes don't match."), True, ColTypes.Warning)
                                End If
                            End If
                        Catch ihae As Exceptions.InvalidHashAlgorithmException
                            WStkTrc(ihae)
                            W(DoTranslation("Invalid encryption algorithm."), True, ColTypes.Err)
                        Catch ihe As Exceptions.InvalidHashException
                            WStkTrc(ihe)
                            W(DoTranslation("Hashes are malformed."), True, ColTypes.Err)
                        Catch fnfe As FileNotFoundException
                            WStkTrc(fnfe)
                            W(DoTranslation("{0} is not found."), True, ColTypes.Err, eqargs(3))
                        End Try
                    End If

                Case "weather"

                    If RequiredArgumentsProvided Then
                        If eqargs(0) = "listcities" Then
                            Dim Cities As Dictionary(Of Long, String) = ListAllCities()
                            WriteList(Cities)
                        Else
                            Dim APIKey As String
                            W(DoTranslation("You can get your own API key at https://home.openweathermap.org/api_keys."), True, ColTypes.Neutral)
                            W(DoTranslation("Enter your API key:") + " ", False, ColTypes.Input)
                            APIKey = ReadLineNoInput("*")
                            Console.WriteLine()
                            PrintWeatherInfo(eqargs(0), APIKey)
                        End If
                    End If

                Case "wrap"

                    If RequiredArgumentsProvided Then
                        Dim CommandToBeWrapped As String = eqargs(0).Split(" ")(0)
                        If Commands.ContainsKey(CommandToBeWrapped) Then
                            If Commands(CommandToBeWrapped).Wrappable Then
                                Dim WrapOutputPath As String = paths("Temp") + "/wrapoutput.txt"
                                GetLine(False, eqargs(0), False, WrapOutputPath)
                                Dim WrapOutputStream As New StreamReader(WrapOutputPath)
                                WriteWrapped(WrapOutputStream.ReadToEnd(), True, ColTypes.Neutral)
                                WrapOutputStream.Close()
                                File.Delete(WrapOutputPath)
                            Else
                                Dim WrappableCmds As New ArrayList
                                For Each CommandInfo As CommandInfo In Commands.Values
                                    If CommandInfo.Wrappable Then WrappableCmds.Add(CommandInfo.Command)
                                Next
                                W(DoTranslation("The command is not wrappable. These commands are wrappable:") + " {0}", True, ColTypes.Err, String.Join(", ", WrappableCmds))
                            End If
                        Else
                            W(DoTranslation("The wrappable command is not found."), True, ColTypes.Err)
                        End If
                    End If

                Case "zip"

                    If RequiredArgumentsProvided Then
                        Dim ZipArchiveName As String = NeutralizePath(eqargs(0))
                        Dim Destination As String = NeutralizePath(eqargs(1))
                        Dim ZipCompression As CompressionLevel = CompressionLevel.Optimal
                        Dim ZipBaseDir As Boolean = True
                        If eqargs?.Contains("-fast") Then
                            ZipCompression = CompressionLevel.Fastest
                        ElseIf eqargs?.Contains("-nocomp") Then
                            ZipCompression = CompressionLevel.NoCompression
                        End If
                        If eqargs?.Contains("-nobasedir") Then
                            ZipBaseDir = False
                        End If
                        ZipFile.CreateFromDirectory(Destination, ZipArchiveName, ZipCompression, ZipBaseDir)
                    End If

                Case "zipshell"

                    If RequiredArgumentsProvided Then
                        eqargs(0) = NeutralizePath(eqargs(0))
                        Wdbg("I", "File path is {0} and .Exists is {0}", eqargs(0), File.Exists(eqargs(0)))
                        If File.Exists(eqargs(0)) Then
                            InitializeZipShell(eqargs(0))
                        Else
                            W(DoTranslation("File doesn't exist."), True, ColTypes.Err)
                        End If
                    End If

            End Select

            'If not enough arguments, throw exception
            If Commands(Command).ArgumentsRequired And Not RequiredArgumentsProvided Then
                Throw New Exceptions.NotEnoughArgumentsException(DoTranslation("There was not enough arguments. See below for usage:"))
            End If
        Catch neaex As Exceptions.NotEnoughArgumentsException
            Wdbg("W", "User hasn't provided enough arguments for {0}", Command)
            W(neaex.Message, True, ColTypes.Neutral)
            ShowHelp(Command)
        Catch taex As ThreadAbortException
            CancelRequested = False
            Exit Sub
        Catch ex As Exception
            EventManager.RaiseCommandError(requestedCommand, ex)
            If DebugMode = True Then
                W(DoTranslation("Error trying to execute command") + " {3}." + vbNewLine + DoTranslation("Error {0}: {1}") + vbNewLine + "{2}", True, ColTypes.Err,
                  Err.Number, ex.Message, ex.StackTrace, Command)
                WStkTrc(ex)
            Else
                W(DoTranslation("Error trying to execute command") + " {2}." + vbNewLine + DoTranslation("Error {0}: {1}"), True, ColTypes.Err, Err.Number, ex.Message, Command)
            End If
        End Try
        StartCommandThread.Abort()
    End Sub

    ''' <summary>
    ''' Cancels any running command
    ''' </summary>
    ''' <param name="sender">Sender object</param>
    ''' <param name="e">Arguments of event</param>
    Sub CancelCommand(sender As Object, e As ConsoleCancelEventArgs)
        If e.SpecialKey = ConsoleSpecialKey.ControlC Then
            CancelRequested = True
            Console.WriteLine()
            DefConsoleOut = Console.Out
            Console.SetOut(StreamWriter.Null)
            e.Cancel = True
            StartCommandThread.Abort()
        End If
    End Sub

End Module
