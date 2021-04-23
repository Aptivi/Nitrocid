
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
        Dim Done As Boolean = False
        Dim Command As String

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

        '4. Split the arguments with enclosed quotes
        Dim eqargs() As String
        Dim TStream As New MemoryStream(Encoding.Default.GetBytes(strArgs))
        Dim Parser As New TextFieldParser(TStream) With {
            .Delimiters = {" "},
            .HasFieldsEnclosedInQuotes = True
        }
        eqargs = Parser.ReadFields
        If eqargs IsNot Nothing Then
            For i As Integer = 0 To eqargs.Length - 1
                eqargs(i).Replace("""", "")
            Next
        End If

        '4a. Debug: get all arguments from eqargs()
        If eqargs IsNot Nothing Then Wdbg("I", "Arguments parsed from eqargs(): " + String.Join(", ", eqargs))

        '5. Check to see if a requested command is obsolete
        If obsoleteCmds.Contains(Command) Then
            Wdbg("I", "The command requested {0} is obsolete", Command)
            W(DoTranslation("This command is obsolete and will be removed in a future release."), True, ColTypes.Neutral)
        End If

        '6. Execute a command
        Try
            Select Case words(0)
                Case "help"

                    If requestedCommand = "help" Then
                        ShowHelp()
                    Else
                        If eqargs?.Length - 1 >= 0 Then
                            ShowHelp(eqargs(0))
                        Else
                            ShowHelp(words(0))
                        End If
                    End If
                    Done = True

                Case "adduser"

                    If requestedCommand <> "adduser" Then
                        If eqargs?.Length - 1 = 0 Then
                            W(DoTranslation("usrmgr: Creating username {0}..."), True, ColTypes.Neutral, eqargs(0))
                            AddUser(eqargs(0))
                            Done = True
                        ElseIf eqargs.Count - 1 >= 2 Then
                            If eqargs(1) = eqargs(2) Then
                                W(DoTranslation("usrmgr: Creating username {0}..."), True, ColTypes.Neutral, eqargs(0))
                                AddUser(eqargs(0), eqargs(1))
                                Done = True
                            Else
                                W(DoTranslation("Passwords don't match."), True, ColTypes.Err)
                                Done = True
                            End If
                        End If
                    End If

                Case "alias"

                    If requestedCommand <> "alias" Then
                        If eqargs?.Length - 1 > 2 Then
                            If eqargs(0) = "add" And (eqargs(1) = AliasType.Shell Or eqargs(1) = AliasType.RDebug Or eqargs(1) = AliasType.FTPShell Or eqargs(1) = AliasType.SFTPShell Or eqargs(1) = AliasType.MailShell) Then
                                ManageAlias(eqargs(0), eqargs(1), eqargs(2), eqargs(3))
                                Done = True
                            Else
                                W(DoTranslation("Invalid type {0}."), True, ColTypes.Err, eqargs(1))
                            End If
                        ElseIf eqargs?.Length - 1 = 2 Then
                            If eqargs(0) = "rem" And (eqargs(1) = AliasType.Shell Or eqargs(1) = AliasType.RDebug Or eqargs(1) = AliasType.FTPShell Or eqargs(1) = AliasType.SFTPShell Or eqargs(1) = AliasType.MailShell) Then
                                ManageAlias(eqargs(0), eqargs(1), eqargs(2))
                                Done = True
                            Else
                                W(DoTranslation("Invalid type {0}."), True, ColTypes.Err, eqargs(1))
                            End If
                        End If
                    End If

                Case "arginj"

                    'Argument Injection
                    If eqargs?.Length - 1 >= 0 Then
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
                            Done = True
                        Else
                            answerargs = String.Join(",", FinalArgs)
                            argsInjected = True
                            W(DoTranslation("Injected arguments, {0}, will be scheduled to run at next reboot."), True, ColTypes.Neutral, answerargs)
                            Done = True
                        End If
                    End If

                Case "beep"

                    If eqargs?.Length > 1 Then
                        If eqargs(0).IsNumeric And CInt(eqargs(0)) >= 37 And CInt(eqargs(0)) <= 32767 Then 'Frequency must be numeric, and must be >= 37 and <= 32767
                            If eqargs(1).IsNumeric Then 'Time must be numeric
                                Console.Beep(eqargs(0), eqargs(1))
                            Else
                                W(DoTranslation("Time must be numeric."), True, ColTypes.Err)
                            End If
                        Else
                            W(DoTranslation("Frequency must be numeric. If it's numeric, ensure that it is >= 37 and <= 32767."), True, ColTypes.Err)
                        End If
                        Done = True
                    End If

                Case "blockdbgdev"

                    If eqargs?.Length - 1 >= 0 Then
                        If Not RDebugBlocked.Contains(eqargs(0)) Then
                            If AddToBlockList(eqargs(0)) Then
                                W(DoTranslation("{0} can't join remote debug now."), True, ColTypes.Neutral, eqargs(0))
                            Else
                                W(DoTranslation("Failed to block {0}."), True, ColTypes.Neutral, eqargs(0))
                            End If
                        Else
                            W(DoTranslation("{0} is already blocked."), True, ColTypes.Neutral, eqargs(0))
                        End If
                        Done = True
                    End If

                Case "cat"

                    If eqargs?.Length > 0 Then
                        Try
                            ReadContents(eqargs(0))
                        Catch ex As Exception
                            WStkTrc(ex)
                            W(ex.Message, True, ColTypes.Err)
                        End Try
                        Done = True
                    End If

                Case "calc"

                    If eqargs?.Length > 0 Then
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
                        Done = True
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
                    Done = True

                Case "chattr"

                    If eqargs?.Length > 1 Then
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
                        Done = True
                    End If

                Case "chdir"

                    If eqargs?.Length > 0 Then
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
                        Done = True
                    End If

                Case "chhostname"

                    If eqargs?.Length > 0 Then
                        If eqargs(0) = "" Then
                            W(DoTranslation("Blank host name."), True, ColTypes.Err)
                        ElseIf eqargs(0).IndexOfAny("[~`!@#$%^&*()-+=|{}':;.,<>/?]".ToCharArray) <> -1 Then
                            W(DoTranslation("Special characters are not allowed."), True, ColTypes.Err)
                        Else
                            Done = True
                            W(DoTranslation("Changing from: {0} to {1}..."), True, ColTypes.Neutral, HName, eqargs(0))
                            ChangeHostname(eqargs(0))
                        End If
                    End If

                Case "chlang"

                    If eqargs?.Length > 0 Then
                        PromptForSetLang(eqargs(0))
                        Done = True
                    End If

                Case "chmotd"

                    If eqargs?.Length > 0 Then
                        If strArgs = "" Then
                            W(DoTranslation("Blank message of the day."), True, ColTypes.Err)
                        Else
                            W(DoTranslation("Changing MOTD..."), True, ColTypes.Neutral)
                            SetMOTD(strArgs, MessageType.MOTD)
                            Done = True
                        End If
                    Else
                        InitializeTextShell(paths("Home") + "/MOTD.txt")
                        W(DoTranslation("Changing MOTD..."), True, ColTypes.Neutral)
                        ReadMOTDFromFile(MessageType.MOTD)
                        Done = True
                    End If

                Case "chmal"

                    If eqargs?.Length > 0 Then
                        If strArgs = "" Then
                            W(DoTranslation("Blank MAL After Login."), True, ColTypes.Err)
                        Else
                            W(DoTranslation("Changing MAL..."), True, ColTypes.Neutral)
                            SetMOTD(strArgs, MessageType.MAL)
                            Done = True
                        End If
                    Else
                        InitializeTextShell(paths("Home") + "/MAL.txt")
                        W(DoTranslation("Changing MAL..."), True, ColTypes.Neutral)
                        ReadMOTDFromFile(MessageType.MAL)
                        Done = True
                    End If

                Case "choice"

                    If eqargs?.Length > 2 Then
                        PromptChoice(strArgs, eqargs(0), eqargs(1))
                        Done = True
                    End If

                Case "chpwd"

                    If eqargs?.Length - 1 >= 3 Then
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
                        Done = True
                    End If

                Case "chusrname"

                    If eqargs?.Length - 1 >= 1 Then
                        ChangeUsername(eqargs(0), eqargs(1))
                        W(DoTranslation("Username has been changed to {0}!"), True, ColTypes.Neutral, eqargs(1))
                        If eqargs(0) = signedinusrnm Then
                            LogoutRequested = True
                        End If
                        Done = True
                    End If

                Case "cls"

                    Console.Clear() : Done = True

                Case "copy"

                    If eqargs?.Length >= 2 Then
                        CopyFileOrDir(eqargs(0), eqargs(1))
                        Done = True
                    End If

                Case "debuglog"

                    Dim line As String
                    Try
                        Using dbglog = File.Open(paths("Debugging"), FileMode.OpenOrCreate, FileAccess.Read, FileShare.ReadWrite), reader As New StreamReader(dbglog)
                            line = reader.ReadLine()
                            Do While reader.EndOfStream <> True
                                W(line, True, ColTypes.Neutral)
                                line = reader.ReadLine
                            Loop
                        End Using
                    Catch ex As Exception
                        W(DoTranslation("Debug log not found"), True, ColTypes.Err)
                        WStkTrc(ex)
                    End Try
                    Done = True

                Case "dirinfo"

                    If eqargs?.Length > 0 Then
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
                        Done = True
                    End If

                Case "disconndbgdev"

                    If eqargs?.Length - 1 >= 0 Then
                        DisconnectDbgDev(eqargs(0))
                        W(DoTranslation("Device {0} disconnected."), True, ColTypes.Neutral, eqargs(0))
                        Done = True
                    End If

                Case "dismissnotif"

                    If eqargs?.Length - 1 >= 0 Then
                        Dim NotifIndex As Integer = eqargs(0) - 1
                        If NotifDismiss(NotifIndex) Then
                            W(DoTranslation("Notification dismissed successfully."), True, ColTypes.Neutral)
                        Else
                            W(DoTranslation("Error trying to dismiss notification."), True, ColTypes.Err)
                        End If
                        Done = True
                    End If

                Case "echo"

                    W(strArgs, True, ColTypes.Neutral)
                    Done = True

                Case "edit"

                    If eqargs?.Length >= 1 Then
                        eqargs(0) = NeutralizePath(eqargs(0))
                        Wdbg("I", "File path is {0} and .Exists is {0}", eqargs(0), File.Exists(eqargs(0)))
                        If File.Exists(eqargs(0)) Then
                            InitializeTextShell(eqargs(0))
                        Else
                            W(DoTranslation("File doesn't exist."), True, ColTypes.Err)
                        End If
                        Done = True
                    End If

                Case "fileinfo"

                    If eqargs?.Length > 0 Then
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
                        Done = True
                    End If

                Case "firedevents"

                    Done = True
                    WriteList(EventManager.FiredEvents)

                Case "ftp"

                    If eqargs?.Length = 0 Then
                        InitiateShell()
                    Else
                        InitiateShell(True, eqargs(0))
                    End If
                    Done = True

                Case "get"
#Disable Warning BC42104
                    If eqargs?.Length <> 0 Then
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
                                        If DownloadFile(eqargs(0), ShowProgress, Credentials) Then
                                            W(vbNewLine + DoTranslation("Download has completed."), True, ColTypes.Neutral)
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
                        Done = True
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
                        W("  - " + DoTranslation("Drive size:") + " {0}", True, ColTypes.Neutral, HardwareInfo.Hardware.HDD(Drive).Size)
                        W("  - " + DoTranslation("Drive serial:") + " {0}", True, ColTypes.Neutral, HardwareInfo.Hardware.HDD(Drive).Serial)
                        W("  - " + DoTranslation("Drive ID:") + " {0}", True, ColTypes.Neutral, HardwareInfo.Hardware.HDD(Drive).ID)
                        W("  - " + DoTranslation("Drive partition count:") + " {0}", True, ColTypes.Neutral, HardwareInfo.Hardware.HDD(Drive).Partitions.Count)
                        For PartitionIndex As Integer = 0 To HardwareInfo.Hardware.HDD(Drive).Partitions.Count - 1
                            W("    - [{0}] " + DoTranslation("Partition ID:") + " {1}", True, ColTypes.Neutral, PartitionIndex, HardwareInfo.Hardware.HDD(Drive).Partitions.Values(PartitionIndex).ID)
                            W("    - [{0}] " + DoTranslation("Partition filesystem:") + " {1}", True, ColTypes.Neutral, PartitionIndex, HardwareInfo.Hardware.HDD(Drive).Partitions.Values(PartitionIndex).FileSystem)
                            W("    - [{0}] " + DoTranslation("Partition size:") + " {1}", True, ColTypes.Neutral, PartitionIndex, HardwareInfo.Hardware.HDD(Drive).Partitions.Values(PartitionIndex).Size)
                            W("    - [{0}] " + DoTranslation("Partition used:") + " {1}", True, ColTypes.Neutral, PartitionIndex, HardwareInfo.Hardware.HDD(Drive).Partitions.Values(PartitionIndex).Used)
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
                    W("  - " + DoTranslation("RAM free:") + " {0}", True, ColTypes.Neutral, HardwareInfo.Hardware.RAM.FreeMemory)
                    W("  - " + DoTranslation("RAM total:") + " {0}", True, ColTypes.Neutral, HardwareInfo.Hardware.RAM.TotalMemory)
                    W("  - " + DoTranslation("RAM used:") + " {0}", True, ColTypes.Neutral, HardwareInfo.Hardware.RAM.UsedMemory)
                    Console.WriteLine()

                Case "input"

                    If eqargs?.Length > 1 Then
                        PromptInput(strArgs.Replace(eqargs(0) + " ", ""), eqargs(0))
                        Done = True
                    End If

                Case "lockscreen"

                    Done = True
                    LockScreen()

                Case "list"

                    'Lists folders and files
                    If eqargs?.Length = 0 Or IsNothing(eqargs) Then
                        List(CurrDir)
                        Done = True
                    Else
                        For Each Directory As String In eqargs
                            Dim direct As String = NeutralizePath(Directory)
                            List(direct)
                            Done = True
                        Next
                    End If

                Case "loteresp"

                    Done = True
                    InitializeLoteresp()

                Case "lsmail"

                    If KeepAlive Then
                        OpenMailShell(Mail_Authentication.Domain)
                        Done = True
                    Else
                        If eqargs?.Length = 0 Or IsNothing(eqargs) Then
                            PromptUser()
                            Done = True
                        ElseIf Not eqargs(0) = "" Then
                            PromptPassword(eqargs(0))
                            Done = True
                        End If
                    End If

                Case "logout"

                    'Logs out of the user
                    Done = True
                    LogoutRequested = True

                Case "lsdbgdev"

                    Done = True
                    For Each DebugDevice As String In DebugDevices.Values
                        W($"- {DebugDevice}", True, ColTypes.ListEntry)
                    Next

                Case "mathbee"

                    Done = True
                    InitializeSolver()

                Case "md"

                    If eqargs?.Length > 0 Then
                        'Create directory
                        MakeDirectory(eqargs(0))
                        Done = True
                    End If

                Case "mkfile"

                    If eqargs?.Length >= 1 Then
                        MakeFile(eqargs(0))
                        Done = True
                    End If

                Case "mktheme"

                    If eqargs?.Length >= 1 Then
                        StartThemeStudio(eqargs(0))
                        Done = True
                    End If

                Case "move"

                    If eqargs?.Length >= 2 Then
                        MoveFileOrDir(eqargs(0), eqargs(1))
                        Done = True
                    End If

                Case "netinfo"

                    PrintAdapterProperties()
                    Done = True

                Case "perm"

                    If eqargs?.Length - 1 >= 2 Then
                        Permission([Enum].Parse(GetType(PermissionType), eqargs(1)), eqargs(0), [Enum].Parse(GetType(PermissionManagementMode), eqargs(2)))
                        Done = True
                    End If

                Case "ping"

                    If eqargs?.Length > 0 Then
                        'If the pinged address is actually a number of times
                        Dim PingTimes As Integer = 4
                        If IsNumeric(eqargs(0)) Then
                            Wdbg("I", "eqargs(0) is numeric. Assuming number of times: {0}", eqargs(0))
                            PingTimes = eqargs(0)
                        End If
                        For Each PingedAddress As String In eqargs.Skip(1)
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
                        Done = True
                    End If

                Case "put"
#Disable Warning BC42104
                    If eqargs?.Length <> 0 Then
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
                                        If UploadFile(FileName, URL, ShowProgress, Credentials) Then
                                            W(vbNewLine + DoTranslation("Upload has completed."), True, ColTypes.Neutral)
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
                        Done = True
                    End If
#Enable Warning BC42104
                Case "reloadconfig"

                    'Reload configuration
                    Done = True
                    ReloadConfig()
                    W(DoTranslation("Configuration reloaded. You might need to reboot the kernel for some changes to take effect."), True, ColTypes.Neutral)

                Case "reboot"

                    'Reboot the simulated system
                    Done = True
                    If Not eqargs?.Length = 0 Then
                        If eqargs(0) = "safe" Then
                            PowerManage("rebootsafe")
                        ElseIf eqargs(0) <> "" Then
                            PowerManage("remoterestart", eqargs(0))
                        Else
                            PowerManage("reboot")
                        End If
                    Else
                        PowerManage("reboot")
                    End If

                Case "rebootmods"

                    'Reload mods
                    Done = True
                    If Not SafeMode Then
                        ReloadMods()
                        W(DoTranslation("Mods reloaded."), True, ColTypes.Neutral)
                    Else
                        W(DoTranslation("Reloading not allowed in safe mode."), True, ColTypes.Err)
                    End If

                Case "reloadsaver"

                    If eqargs?.Length - 1 >= 0 Then
                        If Not SafeMode Then
                            CompileCustom(eqargs(0))
                        Else
                            W(DoTranslation("Reloading not allowed in safe mode."), True, ColTypes.Err)
                        End If
                        Done = True
                    End If

                Case "rexec"

                    If eqargs?.Length > 1 Then
                        Done = True
                        SendCommand("<Request:Exec>(" + eqargs(1) + ")", eqargs(0))
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
                    Done = True

                Case "rm"

                    If eqargs?.Length - 1 >= 0 Then
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
                        Done = True
                    End If

                Case "rmuser"

                    If eqargs?.Length - 1 >= 0 Then
                        RemoveUser(eqargs(0))
                        W(DoTranslation("User {0} removed."), True, ColTypes.Neutral, eqargs(0))
                        Done = True
                    End If

                Case "savescreen"

                    Done = True
                    If eqargs?.Length >= 1 Then
                        ShowSavers(eqargs(0))
                    Else
                        ShowSavers(defSaverName)
                    End If

                Case "search"

                    If eqargs?.Length >= 2 Then
                        Dim Matches As List(Of String) = SearchFileForString(eqargs(1), eqargs(0))
                        For Each Match As String In Matches
                            W(Match, True, ColTypes.Neutral)
                        Next
                        Done = True
                    End If

                Case "setsaver"

                    Dim modPath As String = paths("Mods")
                    If eqargs?.Length > 0 Then
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
                        Done = True
                    End If

                Case "setthemes"

                    If eqargs?.Length - 1 >= 0 Then
                        If ColoredShell = True Then
                            TemplateSet(eqargs(0))
                        Else
                            W(DoTranslation("Colors are not available. Turn on colored shell in the kernel config."), True, ColTypes.Neutral)
                        End If
                        Done = True
                    End If

                Case "settings"

                    OpenMainPage()
                    Done = True

                Case "set"

                    If eqargs?.Length >= 2 Then
                        Done = True
                        SetVariable(eqargs(0), eqargs(1))
                    End If

                Case "sftp"

                    If eqargs?.Length = 0 Then
                        SFTPInitiateShell()
                    Else
                        SFTPInitiateShell(True, eqargs(0))
                    End If
                    Done = True

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
                    Done = True

                Case "showtd"

                    ShowCurrentTimes()
                    Done = True

                Case "showtdzone"

                    If eqargs?.Length > 0 Then
                        InitTimesInZones()
                        Dim DoneFlag As Boolean = False
                        If zoneTimes.Keys.Contains(strArgs) Then
                            DoneFlag = True : Done = True
                            ShowTimesInZones(strArgs)
                        End If
                        If DoneFlag = False Then
                            If eqargs(0) = "all" Then
                                ShowTimesInZones()
                                Done = True
                            Else
                                W(DoTranslation("Timezone is specified incorrectly."), True, ColTypes.Err)
                                Done = True
                            End If
                        End If
                    End If

                Case "shutdown"

                    'Shuts down the simulated system
                    Done = True
                    If Not eqargs?.Length = 0 Then
                        If eqargs(0) <> "" Then
                            PowerManage("remoteshutdown", eqargs(0))
                        End If
                    Else
                        PowerManage("shutdown")
                    End If

                Case "spellbee"

                    Done = True
                    InitializeWords()

                Case "sshell"

                    If eqargs?.Length >= 3 Then
                        InitializeSSH(eqargs(0), eqargs(1), eqargs(2), ConnectionType.Shell)
                        Done = True
                    End If

                Case "sshcmd"

                    If eqargs?.Length >= 3 Then
                        InitializeSSH(eqargs(0), eqargs(1), eqargs(2), ConnectionType.Command, eqargs(3))
                        Done = True
                    End If

                Case "sumfile"

                    If eqargs?.Length >= 2 Then
                        Done = True
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

                    If eqargs?.Length >= 2 Then
                        Done = True
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
                    Done = True
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
                    ListDrivers()
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

                    If eqargs?.Length - 1 >= 0 Then
                        If RDebugBlocked.Contains(eqargs(0)) Then
                            If RemoveFromBlockList(eqargs(0)) Then
                                W(DoTranslation("{0} can now join remote debug again."), True, ColTypes.Neutral, eqargs(0))
                            Else
                                W(DoTranslation("Failed to unblock {0}."), True, ColTypes.Neutral, eqargs(0))
                            End If
                        Else
                            W(DoTranslation("{0} is not blocked yet."), True, ColTypes.Neutral, eqargs(0))
                        End If
                        Done = True
                    End If

                Case "unzip"

                    If eqargs?.Length = 1 Then
                        Done = True
                        Dim ZipArchiveName As String = NeutralizePath(eqargs(0))
                        ZipFile.ExtractToDirectory(ZipArchiveName, CurrDir)
                    ElseIf eqargs?.Length > 1 Then
                        Done = True
                        Dim ZipArchiveName As String = NeutralizePath(eqargs(0))
                        Dim Destination As String = NeutralizePath(eqargs(1))
                        If eqargs?.Contains("-createdir") Then
                            Destination = $"{NeutralizePath($"{Path.GetFileNameWithoutExtension(ZipArchiveName)}")}/{eqargs(1)}"
                        End If
                        ZipFile.ExtractToDirectory(ZipArchiveName, Destination)
                    End If

                Case "update"

                    Done = True
#If SPECIFIER = "REL" Then
                    CheckKernelUpdates()
#Else
                    W(DoTranslation("Checking for updates is disabled because you're running a development version."), True, ColTypes.Err)
#End If

                Case "usermanual"

                    Done = True
                    Process.Start("https://github.com/EoflaOE/Kernel-Simulator/wiki")

                Case "verify"

                    If eqargs?.Length >= 4 Then
                        Done = True
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

                    If eqargs?.Length >= 0 Then
                        Done = True
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

                    If eqargs?.Length >= 0 Then
                        Done = True
                        If WrappableCmds.Contains(eqargs(0).Split(" ")(0)) Then
                            Dim WrapOutputPath As String = paths("Temp") + "/wrapoutput.txt"
                            GetLine(False, eqargs(0), WrapOutputPath)
                            Dim WrapOutputStream As New StreamReader(WrapOutputPath)
                            WriteWrapped(WrapOutputStream.ReadToEnd(), True, ColTypes.Neutral)
                            WrapOutputStream.Close()
                            File.Delete(WrapOutputPath)
                        Else
                            W(DoTranslation("The command is not wrappable. These commands are wrappable:") + " {0}", True, ColTypes.Err, String.Join(", ", WrappableCmds))
                        End If
                    End If

                Case "zip"

                    If eqargs?.Length >= 2 Then
                        Done = True
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

                    If eqargs?.Length >= 1 Then
                        eqargs(0) = NeutralizePath(eqargs(0))
                        Wdbg("I", "File path is {0} and .Exists is {0}", eqargs(0), File.Exists(eqargs(0)))
                        If File.Exists(eqargs(0)) Then
                            InitializeZipShell(eqargs(0))
                        Else
                            W(DoTranslation("File doesn't exist."), True, ColTypes.Err)
                        End If
                        Done = True
                    End If

            End Select
            If Done = False Then
                Throw New Exceptions.NotEnoughArgumentsException(DoTranslation("There was not enough arguments. See below for usage:"))
            End If
        Catch neaex As Exceptions.NotEnoughArgumentsException
            Wdbg("W", "User hasn't provided enough arguments for {0}", words(0))
            W(neaex.Message, True, ColTypes.Neutral)
            ShowHelp(words(0))
        Catch taex As ThreadAbortException
            CancelRequested = False
            Exit Sub
        Catch ex As Exception
            EventManager.RaiseCommandError(requestedCommand, ex)
            If DebugMode = True Then
                W(DoTranslation("Error trying to execute command") + " {3}." + vbNewLine + DoTranslation("Error {0}: {1}") + vbNewLine + "{2}", True, ColTypes.Err,
                  Err.Number, ex.Message, ex.StackTrace, words(0))
                WStkTrc(ex)
            Else
                W(DoTranslation("Error trying to execute command") + " {2}." + vbNewLine + DoTranslation("Error {0}: {1}"), True, ColTypes.Err, Err.Number, ex.Message, words(0))
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
            DefConsoleOut = Console.Out
            Console.SetOut(StreamWriter.Null)
            e.Cancel = True
            StartCommandThread.Abort()
        End If
    End Sub

End Module
