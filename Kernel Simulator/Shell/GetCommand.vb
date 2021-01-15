
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
        '1. Get the index of the first space (Used for step 3)
        Dim index As Integer = requestedCommand.IndexOf(" ")
        If index = -1 Then index = requestedCommand.Length
        Wdbg("I", "Index: {0}", index)

        '2. Split the requested command string into words
        Dim words() As String = requestedCommand.Split({" "c})
        For i As Integer = 0 To words.Length - 1
            Wdbg("I", "Word {0}: {1}", i + 1, words(i))
        Next

        '3. Get the string of arguments
        Dim strArgs As String = requestedCommand.Substring(index)
        Wdbg("I", "Prototype strArgs: {0}", strArgs)
        If Not index = requestedCommand.Length Then strArgs = strArgs.Substring(1)
        Wdbg("I", "Finished strArgs: {0}", strArgs)

        '5. Split the arguments (again) this time with enclosed quotes
        Dim eqargs() As String
        Dim TStream As New MemoryStream(Encoding.Default.GetBytes(strArgs))
        Dim Parser As New TextFieldParser(TStream) With {
            .Delimiters = {" "},
            .HasFieldsEnclosedInQuotes = True
        }
        eqargs = Parser.ReadFields
        If Not eqargs Is Nothing Then
            For i As Integer = 0 To eqargs.Length - 1
                eqargs(i).Replace("""", "")
            Next
        End If

        '5a. Debug: get all arguments from eqargs()
        If Not eqargs Is Nothing Then Wdbg("I", "Arguments parsed from eqargs(): " + String.Join(", ", eqargs))

        'The command is done
        Dim Done As Boolean = False

        '6. Check to see if a requested command is obsolete
        If obsoleteCmds.Contains(words(0)) Then
            Wdbg("I", "The command requested {0} is obsolete", words(0))
            W(DoTranslation("This command is obsolete and will be removed in a future release.", currentLang), True, ColTypes.Neutral)
        End If

        '7. Execute a command
        Try
            If words(0) = "help" Then

                If requestedCommand = "help" Then
                    ShowHelp()
                Else
                    If eqargs?.Count - 1 >= 0 Then
                        ShowHelp(eqargs(0))
                    Else
                        ShowHelp(words(0))
                    End If
                End If
                Done = True

            ElseIf words(0) = "adduser" Then

                If requestedCommand <> "adduser" Then
                    If eqargs?.Count - 1 = 0 Then
                        W(DoTranslation("usrmgr: Creating username {0}...", currentLang), True, ColTypes.Neutral, eqargs(0))
                        AddUser(eqargs(0))
                        Done = True
                    ElseIf eqargs.Count - 1 >= 2 Then
                        If eqargs(1) = eqargs(2) Then
                            W(DoTranslation("usrmgr: Creating username {0}...", currentLang), True, ColTypes.Neutral, eqargs(0))
                            AddUser(eqargs(0), eqargs(1))
                            Done = True
                        Else
                            W(DoTranslation("Passwords don't match.", currentLang), True, ColTypes.Err)
                            Done = True
                        End If
                    End If
                End If

            ElseIf words(0) = "alias" Then

                If requestedCommand <> "alias" Then
                    If eqargs?.Count - 1 > 2 Then
                        If eqargs(0) = "add" And (eqargs(1) = AliasType.Shell Or eqargs(1) = AliasType.RDebug) Then
                            ManageAlias(eqargs(0), eqargs(1), eqargs(2), eqargs(3))
                            Done = True
                        ElseIf eqargs(0) = "add" And (eqargs(1) <> AliasType.Shell Or eqargs(1) <> AliasType.RDebug) Then
                            W(DoTranslation("Invalid type {0}.", currentLang), True, ColTypes.Err, eqargs(1))
                        End If
                    ElseIf eqargs?.Count - 1 = 2 Then
                        If eqargs(0) = "rem" And (eqargs(1) = AliasType.Shell Or eqargs(1) = AliasType.RDebug) Then
                            ManageAlias(eqargs(0), eqargs(1), eqargs(2))
                            Done = True
                        ElseIf eqargs(0) = "rem" And (eqargs(1) <> AliasType.Shell Or eqargs(1) <> AliasType.RDebug) Then
                            W(DoTranslation("Invalid type {0}.", currentLang), True, ColTypes.Err, eqargs(1))
                        End If
                    End If
                End If

            ElseIf words(0) = "arginj" Then

                'Argument Injection
                If requestedCommand <> "arginj" Then
                    If eqargs?.Count - 1 >= 0 Then
                        Dim FinalArgs As New List(Of String)
                        For Each arg As String In eqargs
                            Wdbg("I", "Parsing argument {0}...", arg)
                            If AvailableArgs.Contains(arg) Then
                                Wdbg("I", "Adding argument {0}...", arg)
                                FinalArgs.Add(arg)
                            End If
                        Next
                        If FinalArgs.Count = 0 Then
                            W(DoTranslation("No arguments specified. Hint: Specify multiple arguments separated by spaces", currentLang), True, ColTypes.Err)
                            Done = True
                        Else
                            answerargs = String.Join(",", FinalArgs)
                            argsInjected = True
                            W(DoTranslation("Injected arguments, {0}, will be scheduled to run at next reboot.", currentLang), True, ColTypes.Neutral, answerargs)
                            Done = True
                        End If
                    End If
                End If

            ElseIf words(0) = "beep" Then

                If eqargs?.Count > 1 Then
                    If eqargs(0).IsNumeric And CInt(eqargs(0)) >= 37 And CInt(eqargs(0)) <= 32767 Then 'Frequency must be numeric, and must be >= 37 and <= 32767
                        If eqargs(1).IsNumeric Then 'Time must be numeric
                            Console.Beep(eqargs(0), eqargs(1))
                        Else
                            W(DoTranslation("Time must be numeric.", currentLang), True, ColTypes.Err)
                        End If
                    Else
                        W(DoTranslation("Frequency must be numeric. If it's numeric, ensure that it is >= 37 and <= 32767.", currentLang), True, ColTypes.Err)
                    End If
                    Done = True
                End If

            ElseIf words(0) = "blockdbgdev" Then

                If eqargs?.Count - 1 >= 0 Then
                    If Not RDebugBlocked.Contains(eqargs(0)) Then
                        If AddToBlockList(eqargs(0)) Then
                            W(DoTranslation("{0} can't join remote debug now.", currentLang), True, ColTypes.Neutral, eqargs(0))
                        Else
                            W(DoTranslation("Failed to block {0}.", currentLang), True, ColTypes.Neutral, eqargs(0))
                        End If
                    Else
                        W(DoTranslation("{0} is already blocked.", currentLang), True, ColTypes.Neutral, eqargs(0))
                    End If
                    Done = True
                End If

            ElseIf words(0) = "bsynth" Then

                If eqargs?.Count > 0 Then
                    Try
                        ProbeSynth(eqargs(0))
                    Catch ex As Exception
                        W(ex.Message, True, ColTypes.Err)
                    End Try
                    Done = True
                End If

            ElseIf words(0) = "calc" Then

                If eqargs?.Count > 0 Then
                    Dim Res As Dictionary(Of Double, Boolean) = DoCalc(strArgs)
                    Wdbg("I", "Res.Values(0) = {0}", Res.Values(0))
                    If Not Res.Values(0) Then 'If there is an error in calculation
                        W(DoTranslation("Error in calculation.", currentLang), True, ColTypes.Err)
                    Else 'Calculation done
                        W(strArgs + " = " + CStr(Res.Keys(0)), True, ColTypes.Neutral)
                    End If
                    Done = True
                End If

            ElseIf requestedCommand = "cdbglog" Then

                If DebugMode Then
                    Try
                        dbgWriter.Close()
                        dbgWriter = New StreamWriter(paths("Debugging")) With {.AutoFlush = True}
                        W(DoTranslation("Debug log removed. All connected debugging devices may still view messages.", currentLang), True, ColTypes.Neutral)
                    Catch ex As Exception
                        W(DoTranslation("Debug log removal failed: {0}", currentLang), True, ColTypes.Err, ex.Message)
                        WStkTrc(ex)
                    End Try
                Else
                    W(DoTranslation("You must turn on debug mode before you can clear debug log.", currentLang), True, ColTypes.Neutral)
                End If
                Done = True

            ElseIf words(0) = "chattr" Then

                If eqargs?.Count > 1 Then
                    Dim NeutralizedFilePath As String = NeutralizePath(eqargs(0))
                    If File.Exists(NeutralizedFilePath) Then
                        If eqargs(1).EndsWith("Normal") Or eqargs(1).EndsWith("ReadOnly") Or eqargs(1).EndsWith("Hidden") Or eqargs(1).EndsWith("Archive") Then
                            If eqargs(1).StartsWith("+") Then
                                Dim Attrib As FileAttributes = [Enum].Parse(GetType(FileAttributes), eqargs(1).Remove(0, 1))
                                If AddAttributeToFile(NeutralizedFilePath, Attrib) Then
                                    W(DoTranslation("Attribute has been added successfully.", currentLang), True, ColTypes.Neutral, eqargs(1))
                                Else
                                    W(DoTranslation("Failed to add attribute.", currentLang), True, ColTypes.Neutral, eqargs(1))
                                End If
                            ElseIf eqargs(1).StartsWith("-") Then
                                Dim Attrib As FileAttributes = [Enum].Parse(GetType(FileAttributes), eqargs(1).Remove(0, 1))
                                If RemoveAttributeFromFile(NeutralizedFilePath, Attrib) Then
                                    W(DoTranslation("Attribute has been removed successfully.", currentLang), True, ColTypes.Neutral, eqargs(1))
                                Else
                                    W(DoTranslation("Failed to remove attribute.", currentLang), True, ColTypes.Neutral, eqargs(1))
                                End If
                            End If
                        Else
                            W(DoTranslation("Attribute ""{0}"" is invalid.", currentLang), True, ColTypes.Err, eqargs(1))
                        End If
                    Else
                        W(DoTranslation("File not found.", currentLang), True, ColTypes.Err)
                    End If
                    Done = True
                End If

            ElseIf words(0) = "chdir" Then

                Try
                    SetCurrDir(strArgs)
                Catch sex As Security.SecurityException
                    Wdbg("E", "Security error: {0} ({1})", sex.Message, sex.PermissionType)
                    W(DoTranslation("You are unauthorized to set current directory to {0}: {1}", currentLang), True, ColTypes.Err, Dir, sex.Message)
                    WStkTrc(sex)
                Catch ptlex As PathTooLongException
                    Wdbg("I", "Directory length: {0}", Dir.Length)
                    W(DoTranslation("The path you've specified is too long.", currentLang), True, ColTypes.Err)
                    WStkTrc(ptlex)
                Catch ex As Exception
                    W(DoTranslation("Changing directory has failed: {0}", currentLang), True, ColTypes.Err, ex.Message)
                    WStkTrc(ex)
                End Try
                Done = True

            ElseIf words(0) = "chhostname" Then

                If requestedCommand <> "chhostname" Then
                    If words(1) = "" Then
                        W(DoTranslation("Blank host name.", currentLang), True, ColTypes.Err)
                    ElseIf words(1).IndexOfAny("[~`!@#$%^&*()-+=|{}':;.,<>/?]".ToCharArray) <> -1 Then
                        W(DoTranslation("Special characters are not allowed.", currentLang), True, ColTypes.Err)
                    Else
                        Done = True
                        W(DoTranslation("Changing from: {0} to {1}...", currentLang), True, ColTypes.Neutral, HName, words(1))
                        ChangeHostname(words(1))
                    End If
                End If

            ElseIf words(0) = "chlang" Then

                If requestedCommand <> "chlang" Then
                    PromptForSetLang(words(1))
                    Done = True
                End If

            ElseIf words(0) = "chmotd" Then

                If requestedCommand <> "chmotd" Then
                    If strArgs = "" Then
                        W(DoTranslation("Blank message of the day.", currentLang), True, ColTypes.Err)
                    Else
                        W(DoTranslation("Changing MOTD...", currentLang), True, ColTypes.Neutral)
                        SetMOTD(strArgs, MessageType.MOTD)
                        Done = True
                    End If
                Else
                    InitializeTextShell(paths("Home") + "/MOTD.txt")
                    W(DoTranslation("Changing MOTD...", currentLang), True, ColTypes.Neutral)
                    ReadMOTDFromFile(MessageType.MOTD)
                    Done = True
                End If

            ElseIf words(0) = "chmal" Then

                If requestedCommand <> "chmal" Then
                    If strArgs = "" Then
                        W(DoTranslation("Blank MAL After Login.", currentLang), True, ColTypes.Err)
                    Else
                        W(DoTranslation("Changing MAL...", currentLang), True, ColTypes.Neutral)
                        SetMOTD(strArgs, MessageType.MAL)
                        Done = True
                    End If
                Else
                    InitializeTextShell(paths("Home") + "/MAL.txt")
                    W(DoTranslation("Changing MAL...", currentLang), True, ColTypes.Neutral)
                    ReadMOTDFromFile(MessageType.MAL)
                    Done = True
                End If

            ElseIf words(0) = "choice" Then

                If eqargs?.Count > 2 Then
                    PromptChoice(strArgs, eqargs(0), eqargs(1))
                    Done = True
                End If

            ElseIf words(0) = "chpwd" Then

                If requestedCommand <> "chpwd" Then
                    If eqargs?.Count - 1 >= 3 Then
                        Try
                            If InStr(eqargs(3), " ") > 0 Then
                                W(DoTranslation("Spaces are not allowed.", currentLang), True, ColTypes.Err)
                            ElseIf eqargs(3) = eqargs(2) Then
                                ChangePassword(eqargs(0), eqargs(1), eqargs(2))
                            ElseIf eqargs(3) <> eqargs(2) Then
                                W(DoTranslation("Passwords doesn't match.", currentLang), True, ColTypes.Err)
                            End If
                        Catch ex As Exception
                            W(DoTranslation("Failed to change password of username: {0}", currentLang), True, ColTypes.Err, ex.Message)
                            WStkTrc(ex)
                        End Try
                        Done = True
                    End If
                End If

            ElseIf words(0) = "chusrname" Then

                If requestedCommand <> "chusrname" Then
                    If eqargs?.Count - 1 >= 1 Then
                        ChangeUsername(eqargs(0), eqargs(1))
                        W(DoTranslation("Username has been changed to {0}!", currentLang), True, ColTypes.Neutral, eqargs(1))
                        If eqargs(0) = signedinusrnm Then
                            LogoutRequested = True
                        End If
                        Done = True
                    End If
                End If

            ElseIf requestedCommand = "cls" Then

                Console.Clear() : Done = True

            ElseIf words(0) = "copy" Then

                If eqargs?.Length >= 2 Then
                    CopyFileOrDir(eqargs(0), eqargs(1))
                    Done = True
                End If

            ElseIf requestedCommand = "debuglog" Then

                PrintLog()
                Done = True

            ElseIf words(0) = "dirinfo" Then

                If eqargs?.Length > 0 Then
                    For Each Dir As String In eqargs
                        Dim DirectoryPath As String = NeutralizePath(Dir)
                        Wdbg("I", "Neutralized directory path: {0} ({1})", DirectoryPath, Directory.Exists(DirectoryPath))
                        W(">> {0}", True, ColTypes.Stage, Dir)
                        If Directory.Exists(DirectoryPath) Then
                            Dim DirInfo As New DirectoryInfo(DirectoryPath)
                            W(DoTranslation("Name: {0}", currentLang), True, ColTypes.Neutral, DirInfo.Name)
                            W(DoTranslation("Full name: {0}", currentLang), True, ColTypes.Neutral, NeutralizePath(DirInfo.FullName))
                            W(DoTranslation("Size: {0}", currentLang), True, ColTypes.Neutral, GetAllSizesInFolder(DirInfo).FileSizeToString)
                            W(DoTranslation("Creation time: {0}", currentLang), True, ColTypes.Neutral, Render(DirInfo.CreationTime))
                            W(DoTranslation("Last access time: {0}", currentLang), True, ColTypes.Neutral, Render(DirInfo.LastAccessTime))
                            W(DoTranslation("Last write time: {0}", currentLang), True, ColTypes.Neutral, Render(DirInfo.LastWriteTime))
                            W(DoTranslation("Attributes: {0}", currentLang), True, ColTypes.Neutral, DirInfo.Attributes)
                            W(DoTranslation("Parent directory: {0}", currentLang), True, ColTypes.Neutral, NeutralizePath(DirInfo.Parent.FullName))
                        Else
                            W(DoTranslation("Can't get information about nonexistent directory.", currentLang), True, ColTypes.Err)
                        End If
                    Next
                    Done = True
                End If

            ElseIf words(0) = "dismissnotif" Then

                If requestedCommand <> "dismissnotif" Then
                    If eqargs?.Count - 1 >= 0 Then
                        Dim NotifIndex As Integer = eqargs(0) - 1
                        If NotifDismiss(NotifIndex) Then
                            W(DoTranslation("Notification dismissed successfully.", currentLang), True, ColTypes.Neutral)
                        Else
                            W(DoTranslation("Error trying to dismiss notification.", currentLang), True, ColTypes.Err)
                        End If
                        Done = True
                    End If
                End If

            ElseIf words(0) = "disconndbgdev" Then

                If requestedCommand <> "disconndbgdev" Then
                    If eqargs?.Count - 1 >= 0 Then
                        DisconnectDbgDev(eqargs(0))
                        W(DoTranslation("Device {0} disconnected.", currentLang), True, ColTypes.Neutral, eqargs(0))
                        Done = True
                    End If
                End If

            ElseIf words(0) = "echo" Then

                W(strArgs, True, ColTypes.Neutral)
                Done = True

            ElseIf words(0) = "edit" Then

                If eqargs?.Count >= 1 Then
                    eqargs(0) = NeutralizePath(eqargs(0))
                    Wdbg("I", "File path is {0} and .Exists is {0}", eqargs(0), File.Exists(eqargs(0)))
                    If File.Exists(eqargs(0)) Then
                        InitializeTextShell(eqargs(0))
                    Else
                        W(DoTranslation("File doesn't exist.", currentLang), True, ColTypes.Err)
                    End If
                    Done = True
                End If

            ElseIf words(0) = "fileinfo" Then

                If eqargs?.Length > 0 Then
                    For Each FileName As String In eqargs
                        Dim FilePath As String = NeutralizePath(FileName)
                        Wdbg("I", "Neutralized file path: {0} ({1})", FilePath, File.Exists(FilePath))
                        W(">> {0}", True, ColTypes.Stage, FileName)
                        If File.Exists(FilePath) Then
                            Dim FileInfo As New FileInfo(FilePath)
                            W(DoTranslation("Name: {0}", currentLang), True, ColTypes.Neutral, FileInfo.Name)
                            W(DoTranslation("Full name: {0}", currentLang), True, ColTypes.Neutral, NeutralizePath(FileInfo.FullName))
                            W(DoTranslation("File size: {0}", currentLang), True, ColTypes.Neutral, FileInfo.Length.FileSizeToString)
                            W(DoTranslation("Creation time: {0}", currentLang), True, ColTypes.Neutral, Render(FileInfo.CreationTime))
                            W(DoTranslation("Last access time: {0}", currentLang), True, ColTypes.Neutral, Render(FileInfo.LastAccessTime))
                            W(DoTranslation("Last write time: {0}", currentLang), True, ColTypes.Neutral, Render(FileInfo.LastWriteTime))
                            W(DoTranslation("Attributes: {0}", currentLang), True, ColTypes.Neutral, FileInfo.Attributes)
                            W(DoTranslation("Where to find: {0}", currentLang), True, ColTypes.Neutral, NeutralizePath(FileInfo.DirectoryName))
                        Else
                            W(DoTranslation("Can't get information about nonexistent file.", currentLang), True, ColTypes.Err)
                        End If
                    Next
                    Done = True
                End If

            ElseIf words(0) = "ftp" Then

                If requestedCommand = "ftp" Then
                    InitiateShell()
                Else
                    InitiateShell(True, words(1))
                End If
                Done = True

            ElseIf words(0) = "get" Then
#Disable Warning BC42104
                If eqargs?.Count <> 0 Then
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
                                        W(DoTranslation("Enter password: ", currentLang), False, ColTypes.Input)
                                        Credentials.Password = ReadLineNoInput("*")
                                        Console.WriteLine()
                                    End If
                                    W(DoTranslation("Downloading from {0}...", currentLang), True, ColTypes.Neutral, URL)
                                    If DownloadFile(eqargs(0), ShowProgress, Credentials) Then
                                        W(vbNewLine + DoTranslation("Download has completed.", currentLang), True, ColTypes.Neutral)
                                    End If
                                Else
                                    W(DoTranslation("Specify the address", currentLang), True, ColTypes.Err)
                                End If
                            Else
                                W(DoTranslation("Please use ""ftp"" if you are going to download files from the FTP server.", currentLang), True, ColTypes.Err)
                            End If
                            Exit Sub
                        Catch ex As Exception
                            DFinish = False
                            W(DoTranslation("Download failed in try {0}: {1}", currentLang), True, ColTypes.Err, RetryCount, ex.Message)
                            RetryCount += 1
                            Wdbg("I", "Try count: {0}", RetryCount)
                            WStkTrc(ex)
                        End Try
                    End While
                    Done = True
                End If
#Enable Warning BC42104
            ElseIf words(0) = "input" Then

                If eqargs?.Count > 1 Then
                    PromptInput(strArgs.Replace(eqargs(0) + " ", ""), eqargs(0))
                    Done = True
                End If

            ElseIf requestedCommand = "lockscreen" Then

                Done = True
                LockScreen()

            ElseIf words(0) = "list" Then

                'Lists folders and files
                If eqargs?.Count = 0 Or IsNothing(eqargs) Then
                    List(CurrDir)
                    Done = True
                Else
                    For Each Directory As String In eqargs
                        Dim direct As String = NeutralizePath(Directory)
                        List(direct)
                        Done = True
                    Next
                End If

            ElseIf words(0) = "loteresp" Then

                Done = True
                InitializeLoteresp()

            ElseIf words(0) = "lsmail" Then

                If KeepAlive Then
                    OpenShell(Mail_Authentication.Domain)
                    Done = True
                Else
                    If eqargs?.Count = 0 Or IsNothing(eqargs) Then
                        PromptUser()
                        Done = True
                    ElseIf Not eqargs(0) = "" Then
                        PromptPassword(eqargs(0))
                        Done = True
                    End If
                End If

            ElseIf requestedCommand = "logout" Then

                'Logs out of the user
                Done = True
                LogoutRequested = True

            ElseIf requestedCommand = "lsdbgdev" Then

                Done = True
                For Each DebugDevice As String In DebugDevices.Values
                    W($"- {DebugDevice}", True, ColTypes.HelpCmd)
                Next

            ElseIf requestedCommand = "netinfo" Then

                PrintAdapterProperties()
                Done = True

            ElseIf words(0) = "mathbee" Then

                Done = True
                InitializeSolver()

            ElseIf words(0) = "md" Then

                If eqargs?.Count > 0 Then
                    'Create directory
                    MakeDirectory(eqargs(0))
                    Done = True
                End If

            ElseIf words(0) = "mkfile" Then

                If eqargs?.Length >= 1 Then
                    MakeFile(eqargs(0))
                    Done = True
                End If

            ElseIf words(0) = "move" Then

                If eqargs?.Length >= 2 Then
                    MoveFileOrDir(eqargs(0), eqargs(1))
                    Done = True
                End If

            ElseIf words(0) = "perm" Then

                If requestedCommand <> "perm" Then
                    If eqargs?.Count - 1 >= 2 Then
                        Permission([Enum].Parse(GetType(PermissionType), eqargs(1)), eqargs(0), [Enum].Parse(GetType(PermissionManagementMode), eqargs(2)))
                        Done = True
                    End If
                End If

            ElseIf words(0) = "ping" Then

                If eqargs?.Count > 0 Then
                    For Each PingedAddress As String In eqargs
                        If PingedAddress <> "" Then
                            W(">> {0}", True, ColTypes.Stage, PingedAddress)
                            Dim PingReplied As PingReply = PingAddress(PingedAddress)
                            If PingReplied.Status = IPStatus.Success Then
                                W(DoTranslation("Ping succeeded in {0} ms.", currentLang), True, ColTypes.Neutral, PingReplied.RoundtripTime)
                            Else
                                W(DoTranslation("Failed to ping {0}: {1}", currentLang), True, ColTypes.Err, PingedAddress, PingReplied.Status)
                            End If
                        Else
                            W(DoTranslation("Address may not be empty.", currentLang), True, ColTypes.Err)
                        End If
                    Next
                    Done = True
                End If

            ElseIf words(0) = "put" Then
#Disable Warning BC42104
                If eqargs?.Count <> 0 Then
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
                                        W(DoTranslation("Enter password: ", currentLang), False, ColTypes.Input)
                                        Credentials.Password = ReadLineNoInput("*")
                                        Console.WriteLine()
                                    End If
                                    W(DoTranslation("Uploading {0} to {1}...", currentLang), True, ColTypes.Neutral, FileName, URL)
                                    If UploadFile(FileName, URL, ShowProgress, Credentials) Then
                                        W(vbNewLine + DoTranslation("Upload has completed.", currentLang), True, ColTypes.Neutral)
                                    End If
                                Else
                                    W(DoTranslation("Specify the address", currentLang), True, ColTypes.Err)
                                End If
                            Else
                                W(DoTranslation("Please use ""ftp"" if you are going to upload files to the FTP server.", currentLang), True, ColTypes.Err)
                            End If
                            Exit Sub
                        Catch ex As Exception
                            UFinish = False
                            W(DoTranslation("Upload failed in try {0}: {1}", currentLang), True, ColTypes.Err, RetryCount, ex.Message)
                            RetryCount += 1
                            Wdbg("I", "Try count: {0}", RetryCount)
                            WStkTrc(ex)
                        End Try
                    End While
                    Done = True
                End If
#Enable Warning BC42104
            ElseIf requestedCommand = "reloadconfig" Then

                'Reload configuration
                Done = True
                ReloadConfig()
                W(DoTranslation("Configuration reloaded. You might need to reboot the kernel for some changes to take effect.", currentLang), True, ColTypes.Neutral)

            ElseIf words(0) = "reboot" Then

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

            ElseIf words(0) = "reloadmods" Then

                'Reload mods
                Done = True
                If Not SafeMode Then
                    ReloadMods()
                    W(DoTranslation("Mods reloaded.", currentLang), True, ColTypes.Neutral)
                Else
                    W(DoTranslation("Reloading not allowed in safe mode.", currentLang), True, ColTypes.Err)
                End If

            ElseIf words(0) = "reloadsaver" Then

                If requestedCommand <> "reloadsaver" Then
                    If eqargs?.Count - 1 >= 0 Then
                        If Not SafeMode Then
                            CompileCustom(eqargs(0))
                        Else
                            W(DoTranslation("Reloading not allowed in safe mode.", currentLang), True, ColTypes.Err)
                        End If
                        Done = True
                    End If
                End If

            ElseIf words(0) = "rexec" Then

                If requestedCommand <> "rexec" Then
                    If eqargs?.Count > 1 Then
                        Done = True
                        SendCommand("<Request:Exec>(" + eqargs(1) + ")", eqargs(0))
                    End If
                End If

            ElseIf words(0) = "rdebug" Then

                If DebugMode Then
                    If RDebugThread.IsAlive Then
                        StartRDebugThread(False)
                    Else
                        StartRDebugThread(True)
                    End If
                Else
                    W(DoTranslation("Debugging not enabled.", currentLang), True, ColTypes.Err)
                End If
                Done = True

            ElseIf words(0) = "rm" Then

                If eqargs?.Count - 1 >= 0 Then
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
                            W(DoTranslation("Can't remove {0} because it doesn't exist.", currentLang), True, ColTypes.Err, Path)
                        End If
                    Next
                    Done = True
                End If

            ElseIf words(0) = "rmuser" Then

                If requestedCommand <> "rmuser" Then
                    If eqargs?.Count - 1 >= 0 Then
                        RemoveUser(eqargs(0))
                        W(DoTranslation("User {0} removed.", currentLang), True, ColTypes.Neutral, eqargs(0))
                        Done = True
                    End If
                End If

            ElseIf requestedCommand = "savescreen" Then

                Done = True
                ShowSavers(defSaverName)

            ElseIf words(0) = "search" Then

                If eqargs?.Count >= 2 Then
                    Dim Matches As List(Of String) = SearchFileForString(eqargs(1), eqargs(0))
                    For Each Match As String In Matches
                        W(Match, True, ColTypes.Neutral)
                    Next
                    Done = True
                End If

            ElseIf words(0) = "setsaver" Then

                Dim modPath As String = paths("Mods")
                If requestedCommand <> "setsaver" Then
                    If ScrnSvrdb.ContainsKey(strArgs) Then
                        SetDefaultScreensaver(strArgs)
                        If ScrnSvrdb(strArgs) Then
                            W(DoTranslation("{0} is set to default screensaver.", currentLang), True, ColTypes.Neutral, strArgs)
                        Else
                            W(DoTranslation("{0} is no longer set to default screensaver.", currentLang), True, ColTypes.Neutral, strArgs)
                        End If
                    Else
                        If FileIO.FileSystem.FileExists($"{modPath}{strArgs}") And Not SafeMode Then
                            SetDefaultScreensaver(strArgs)
                            If ScrnSvrdb(strArgs) Then
                                W(DoTranslation("{0} is set to default screensaver.", currentLang), True, ColTypes.Neutral, strArgs)
                            Else
                                W(DoTranslation("{0} is no longer set to default screensaver.", currentLang), True, ColTypes.Neutral, strArgs)
                            End If
                        Else
                            W(DoTranslation("Screensaver {0} not found.", currentLang), True, ColTypes.Err, strArgs)
                        End If
                    End If
                    Done = True
                End If

            ElseIf words(0) = "setthemes" Then

                If requestedCommand <> "setthemes" Then
                    If eqargs?.Count - 1 >= 0 Then
                        If ColoredShell = True Then
                            TemplateSet(eqargs(0))
                        Else
                            W(DoTranslation("Colors are not available. Turn on colored shell in the kernel config.", currentLang), True, ColTypes.Neutral)
                        End If
                        Done = True
                    End If
                End If

            ElseIf words(0) = "settings" Then

                OpenMainPage()
                Done = True

            ElseIf words(0) = "sftp" Then

                If requestedCommand = "sftp" Then
                    SFTPInitiateShell()
                Else
                    SFTPInitiateShell(True, words(1))
                End If
                Done = True

            ElseIf words(0) = "shownotifs" Then

                Dim Count As Integer = 1
                If Not NotifRecents.Count = 0 Then
                    For Each Notif As Notification In NotifRecents
                        W($"{Count}: {Notif.Title}" + vbNewLine +
                          $"{Count}: {Notif.Desc}" + vbNewLine +
                          $"{Count}: {Notif.Priority}", True, ColTypes.Neutral)
                        Count += 1
                    Next
                Else
                    W(DoTranslation("No recent notifications", currentLang), True, ColTypes.Neutral)
                End If
                Done = True

            ElseIf requestedCommand = "showtd" Then

                ShowCurrentTimes()
                Done = True

            ElseIf words(0) = "showtdzone" Then

                If requestedCommand <> "showtdzone" Then
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
                            W(DoTranslation("Timezone is specified incorrectly.", currentLang), True, ColTypes.Err)
                            Done = True
                        End If
                    End If
                End If

            ElseIf words(0) = "shutdown" Then

                'Shuts down the simulated system
                Done = True
                If Not eqargs?.Length = 0 Then
                    If eqargs(0) <> "" Then
                        PowerManage("remoteshutdown", eqargs(0))
                    End If
                Else
                    PowerManage("shutdown")
                End If

            ElseIf words(0) = "spellbee" Then

                Done = True
                InitializeWords()

            ElseIf words(0) = "sshell" Then

                If eqargs?.Length >= 3 Then
                    InitializeSSH(eqargs(0), eqargs(1), eqargs(2))
                    Done = True
                End If

            ElseIf words(0) = "sumfile" Then

                If eqargs?.Length >= 2 Then
                    Done = True
                    Dim file As String = NeutralizePath(eqargs(1))
                    If IO.File.Exists(file) Then
                        If eqargs(0) = "SHA512" Then
                            Dim spent As New Stopwatch
                            spent.Start() 'Time when you're on a breakpoint is counted
                            W(GetEncryptedFile(file, Algorithms.SHA512), True, ColTypes.Neutral)
                            W(DoTranslation("Time spent: {0} milliseconds", currentLang), True, ColTypes.Neutral, spent.ElapsedMilliseconds)
                            spent.Stop()
                        ElseIf eqargs(0) = "SHA256" Then
                            Dim spent As New Stopwatch
                            spent.Start() 'Time when you're on a breakpoint is counted
                            W(GetEncryptedFile(file, Algorithms.SHA256), True, ColTypes.Neutral)
                            W(DoTranslation("Time spent: {0} milliseconds", currentLang), True, ColTypes.Neutral, spent.ElapsedMilliseconds)
                            spent.Stop()
                        ElseIf eqargs(0) = "SHA1" Then
                            Dim spent As New Stopwatch
                            spent.Start() 'Time when you're on a breakpoint is counted
                            W(GetEncryptedFile(file, Algorithms.SHA1), True, ColTypes.Neutral)
                            W(DoTranslation("Time spent: {0} milliseconds", currentLang), True, ColTypes.Neutral, spent.ElapsedMilliseconds)
                            spent.Stop()
                        ElseIf eqargs(0) = "MD5" Then
                            Dim spent As New Stopwatch
                            spent.Start() 'Time when you're on a breakpoint is counted
                            W(GetEncryptedFile(file, Algorithms.MD5), True, ColTypes.Neutral)
                            W(DoTranslation("Time spent: {0} milliseconds", currentLang), True, ColTypes.Neutral, spent.ElapsedMilliseconds)
                            spent.Stop()
                        Else
                            W(DoTranslation("Invalid encryption algorithm.", currentLang), True, ColTypes.Err)
                        End If
                    Else
                        W(DoTranslation("{0} is not found.", currentLang), True, ColTypes.Err, file)
                    End If
                End If

            ElseIf words(0) = "sumfiles" Then

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
                                W(DoTranslation("Time spent: {0} milliseconds", currentLang), True, ColTypes.Neutral, spent.ElapsedMilliseconds)
                                FileBuilder.AppendLine($"- {file}: {encrypted} ({eqargs(0)})")
                                spent.Stop()
                            ElseIf eqargs(0) = "SHA256" Then
                                Dim spent As New Stopwatch
                                spent.Start() 'Time when you're on a breakpoint is counted
                                Dim encrypted As String = GetEncryptedFile(file, Algorithms.SHA256)
                                W(encrypted, True, ColTypes.Neutral)
                                W(DoTranslation("Time spent: {0} milliseconds", currentLang), True, ColTypes.Neutral, spent.ElapsedMilliseconds)
                                FileBuilder.AppendLine($"- {file}: {encrypted} ({eqargs(0)})")
                                spent.Stop()
                            ElseIf eqargs(0) = "SHA1" Then
                                Dim spent As New Stopwatch
                                spent.Start() 'Time when you're on a breakpoint is counted
                                Dim encrypted As String = GetEncryptedFile(file, Algorithms.SHA1)
                                W(encrypted, True, ColTypes.Neutral)
                                W(DoTranslation("Time spent: {0} milliseconds", currentLang), True, ColTypes.Neutral, spent.ElapsedMilliseconds)
                                FileBuilder.AppendLine($"- {file}: {encrypted} ({eqargs(0)})")
                                spent.Stop()
                            ElseIf eqargs(0) = "MD5" Then
                                Dim spent As New Stopwatch
                                spent.Start() 'Time when you're on a breakpoint is counted
                                Dim encrypted As String = GetEncryptedFile(file, Algorithms.MD5)
                                W(encrypted, True, ColTypes.Neutral)
                                W(DoTranslation("Time spent: {0} milliseconds", currentLang), True, ColTypes.Neutral, spent.ElapsedMilliseconds)
                                FileBuilder.AppendLine($"- {file}: {encrypted} ({eqargs(0)})")
                                spent.Stop()
                            Else
                                W(DoTranslation("Invalid encryption algorithm.", currentLang), True, ColTypes.Err)
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
                        W(DoTranslation("{0} is not found.", currentLang), True, ColTypes.Err, folder)
                    End If
                End If

            ElseIf requestedCommand = "sysinfo" Then

                Done = True

                'Shows system information
                W(DoTranslation("{0}[ Kernel settings (Running on {1}) ]", currentLang), True, ColTypes.HelpCmd, vbNewLine, EnvironmentOSType)

                'Kernel section
                W(vbNewLine + DoTranslation("Kernel Version:", currentLang) + " {0}" + vbNewLine +
                              DoTranslation("Debug Mode:", currentLang) + " {1}" + vbNewLine +
                              DoTranslation("Colored Shell:", currentLang) + " {2}" + vbNewLine +
                              DoTranslation("Arguments on Boot:", currentLang) + " {3}" + vbNewLine +
                              DoTranslation("Help command simplified:", currentLang) + " {4}" + vbNewLine +
                              DoTranslation("MOTD on Login:", currentLang) + " {5}" + vbNewLine +
                              DoTranslation("Time/Date on corner:", currentLang) + " {6}" + vbNewLine +
                              DoTranslation("Current theme:", currentLang) + " {7}" + vbNewLine, True, ColTypes.Neutral, KernelVersion, DebugMode.ToString, ColoredShell.ToString, argsOnBoot.ToString, simHelp.ToString, showMOTD.ToString, CornerTD.ToString, currentTheme)

                'Hardware section
                W(DoTranslation("[ Hardware settings ]{0}", currentLang), True, ColTypes.HelpCmd, vbNewLine)
                ListDrivers()

                'User section
                W(DoTranslation("{0}[ User settings ]", currentLang), True, ColTypes.HelpCmd, vbNewLine)
                W(vbNewLine + DoTranslation("Current user name:", currentLang) + " {0}" + vbNewLine +
                              DoTranslation("Current host name:", currentLang) + " {1}" + vbNewLine +
                              DoTranslation("Available usernames:", currentLang) + " {2}" + vbNewLine +
                              DoTranslation("Computer host name:", currentLang) + " {3}", True, ColTypes.Neutral, signedinusrnm, HName, String.Join(", ", userword.Keys), My.Computer.Name)

                'Messages Section
                W(vbNewLine + "[ MOTD ]", True, ColTypes.HelpCmd)
                W(vbNewLine + ProbePlaces(MOTDMessage), True, ColTypes.Neutral)
                W(vbNewLine + "[ MAL ]", True, ColTypes.HelpCmd)
                W(vbNewLine + ProbePlaces(MAL), True, ColTypes.Neutral)

            ElseIf words(0) = "unblockdbgdev" Then

                If requestedCommand <> "unblockdbgdev" Then
                    If eqargs?.Count - 1 >= 0 Then
                        If RDebugBlocked.Contains(eqargs(0)) Then
                            If RemoveFromBlockList(eqargs(0)) Then
                                W(DoTranslation("{0} can now join remote debug again.", currentLang), True, ColTypes.Neutral, eqargs(0))
                            Else
                                W(DoTranslation("Failed to unblock {0}.", currentLang), True, ColTypes.Neutral, eqargs(0))
                            End If
                        Else
                            W(DoTranslation("{0} is not blocked yet.", currentLang), True, ColTypes.Neutral, eqargs(0))
                        End If
                        Done = True
                    End If
                End If

            ElseIf words(0) = "update" Then

                Done = True
                CheckKernelUpdates()

            ElseIf words(0) = "usermanual" Then

                Done = True
                Process.Start("https://github.com/EoflaOE/Kernel-Simulator/wiki")

            ElseIf words(0) = "verify" Then

                If eqargs?.Length >= 4 Then
                    Done = True
                    Dim file As String = NeutralizePath(eqargs(3))
                    Dim HashFile As String = NeutralizePath(eqargs(2))
                    Dim ExpectedHashLength As Integer
                    Dim ExpectedHash As String = ""
                    Dim ActualHash As String = ""
                    If IO.File.Exists(file) Then
                        If eqargs(0) = "SHA512" Then
                            ExpectedHashLength = 128
                        ElseIf eqargs(0) = "SHA256" Then
                            ExpectedHashLength = 64
                        ElseIf eqargs(0) = "SHA1" Then
                            ExpectedHashLength = 40
                        ElseIf eqargs(0) = "MD5" Then
                            ExpectedHashLength = 32
                        Else
                            W(DoTranslation("Invalid encryption algorithm.", currentLang), True, ColTypes.Err)
                            Exit Try
                        End If

                        'Verify the hash
                        If IO.File.Exists(HashFile) Then
                            Dim HashStream As New StreamReader(HashFile)
                            Wdbg("I", "Stream length: {0}", HashStream.BaseStream.Length)
                            Do While Not HashStream.EndOfStream
                                'Check if made from KS, and take it from before-last split space. If not, take it from the beginning
                                Dim StringLine As String = HashStream.ReadLine
                                If StringLine.StartsWith("- ") Then
                                    Wdbg("I", "Hashes file is of KS format")
                                    If StringLine.StartsWith("- " + file) Then
                                        Dim HashSplit() As String = StringLine.Split(" "c)
                                        ExpectedHash = HashSplit(HashSplit.Length - 2).ToUpper
                                        ActualHash = eqargs(1).ToUpper
                                    End If
                                Else
                                    Wdbg("I", "Hashes file is of standard format")
                                    If StringLine.EndsWith(Path.GetFileName(file)) Then
                                        Dim HashSplit() As String = StringLine.Split(" "c)
                                        ExpectedHash = HashSplit(0).ToUpper
                                        ActualHash = eqargs(1).ToUpper
                                    End If
                                End If
                            Loop
                        Else
                            ExpectedHash = eqargs(2).ToUpper
                            ActualHash = eqargs(1).ToUpper
                        End If

                        If ActualHash.Length = ExpectedHashLength And ExpectedHash.Length = ExpectedHashLength Then
                            Wdbg("I", "Hashes are consistent.")
                            Wdbg("I", "Hashes {0} and {1}", ActualHash, ExpectedHash)
                            If ActualHash = ExpectedHash Then
                                Wdbg("I", "Hashes match.")
                                W(DoTranslation("Hashes match.", currentLang), True, ColTypes.Neutral)
                            Else
                                Wdbg("W", "Hashes don't match.")
                                W(DoTranslation("Hashes don't match.", currentLang), True, ColTypes.Neutral)
                            End If
                        Else
                            Wdbg("E", "{0} ({1}) or {2} ({3}) is malformed. Check the algorithm ({4}). Expected length: {5}", ActualHash, ActualHash.Length, ExpectedHash, ExpectedHash.Length, eqargs(0), ExpectedHashLength)
                            W(DoTranslation("Hashes are malformed.", currentLang), True, ColTypes.Err)
                        End If
                    Else
                        W(DoTranslation("{0} is not found.", currentLang), True, ColTypes.Err, file)
                    End If
                End If

            ElseIf words(0) = "weather" Then

                If requestedCommand <> "weather" Then
                    Done = True
                    Dim APIKey As String
                    W(DoTranslation("You can get your own API key at https://home.openweathermap.org/api_keys.", currentLang), True, ColTypes.Neutral)
                    W(DoTranslation("Enter your API key:", currentLang) + " ", False, ColTypes.Input)
                    APIKey = ReadLineNoInput("*")
                    Console.WriteLine()
                    Dim WeatherInfo As ForecastInfo = GetWeatherInfo(eqargs(0), APIKey, PreferredUnit)
                    Dim WeatherSpecifier As String = "°"
                    Dim WindSpeedSpecifier As String = "m.s"
                    W(DoTranslation("-- Weather info for {0} --", currentLang), True, ColTypes.Stage, WeatherInfo.CityName)
                    W(DoTranslation("Weather: {0}", currentLang), True, ColTypes.Neutral, WeatherInfo.Weather)
                    If WeatherInfo.TemperatureMeasurement = UnitMeasurement.Metric Then
                        WeatherSpecifier += "C"
                    ElseIf WeatherInfo.TemperatureMeasurement = UnitMeasurement.Kelvin Then
                        WeatherSpecifier += "K"
                    ElseIf WeatherInfo.TemperatureMeasurement = UnitMeasurement.Imperial Then
                        WeatherSpecifier += "F"
                        WindSpeedSpecifier = "mph"
                    End If
                    W(DoTranslation("Temperature: {0}", currentLang) + WeatherSpecifier, True, ColTypes.Neutral, FormatNumber(WeatherInfo.Temperature, 2))
                    W(DoTranslation("Feels like: {0}", currentLang) + WeatherSpecifier, True, ColTypes.Neutral, FormatNumber(WeatherInfo.FeelsLike, 2))
                    W(DoTranslation("Wind speed: {0}", currentLang) + " {1}", True, ColTypes.Neutral, FormatNumber(WeatherInfo.WindSpeed, 2), WindSpeedSpecifier)
                    W(DoTranslation("Wind direction: {0}", currentLang) + "°", True, ColTypes.Neutral, FormatNumber(WeatherInfo.WindDirection, 2))
                    W(DoTranslation("Pressure: {0}", currentLang) + " hPa", True, ColTypes.Neutral, FormatNumber(WeatherInfo.Pressure, 2))
                    W(DoTranslation("Humidity: {0}", currentLang) + "%", True, ColTypes.Neutral, FormatNumber(WeatherInfo.Humidity, 2))
                End If

            End If
            If Done = False Then
                Throw New EventsAndExceptions.NotEnoughArgumentsException(DoTranslation("There was not enough arguments. See below for usage:", currentLang))
            End If
        Catch neaex As EventsAndExceptions.NotEnoughArgumentsException
            Wdbg("W", "User hasn't provided enough arguments for {0}", words(0))
            W(neaex.Message, True, ColTypes.Neutral)
            ShowHelp(words(0))
        Catch taex As ThreadAbortException
            Exit Sub
        Catch ex As Exception
            EventManager.RaiseCommandError(requestedCommand, ex)
            If DebugMode = True Then
                W(DoTranslation("Error trying to execute command", currentLang) + " {3}." + vbNewLine + DoTranslation("Error {0}: {1}", currentLang) + vbNewLine + "{2}", True, ColTypes.Err,
                  Err.Number, ex.Message, ex.StackTrace, words(0))
                WStkTrc(ex)
            Else
                W(DoTranslation("Error trying to execute command", currentLang) + " {2}." + vbNewLine + DoTranslation("Error {0}: {1}", currentLang), True, ColTypes.Err, Err.Number, ex.Message, words(0))
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
            DefConsoleOut = Console.Out
            Console.SetOut(StreamWriter.Null)
            e.Cancel = True
            StartCommandThread.Abort()
        End If
    End Sub

End Module
