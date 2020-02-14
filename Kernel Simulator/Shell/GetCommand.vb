
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

Imports System.IO
Imports System.Security.Cryptography
Imports System.Text
Imports Microsoft.VisualBasic.FileIO

Public Module GetCommand

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

        '4. Split the arguments string into array (args: No empty entries | eargs: Empty entries)
        Dim args() As String = strArgs.Split({" "c}, StringSplitOptions.RemoveEmptyEntries)
        Dim eargs() As String = strArgs.Split({" "c})

        '4a. Debug: get all arguments from args()
        Wdbg("I", "Arguments parsed from args(): " + String.Join(", ", args))

        '4b. Debug: get all arguments from eargs()
        Wdbg("I", "Arguments parsed from eargs(): " + String.Join(", ", eargs))

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

        '5a. Debug: get all arguments from eqargs() (NOTICE: args() and eargs() will be removed in the future.)
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
                    If args.Count - 1 = 0 Then
                        ShowHelp(args(0))
                    Else
                        ShowHelp(words(0))
                    End If
                End If
                Done = True

            ElseIf words(0) = "adduser" Then

                If requestedCommand <> "adduser" Then
                    If args.Count - 1 = 0 Then
                        AddUser(args(0))
                        Done = True
                    ElseIf args.Count - 1 = 2 Then
                        If args(1) = args(2) Then
                            AddUser(args(0), args(1))
                            Done = True
                        Else
                            W(DoTranslation("Passwords don't match.", currentLang), True, ColTypes.Neutral)
                            Done = True
                        End If
                    End If
                End If

            ElseIf words(0) = "alias" Then

                If requestedCommand <> "alias" Then
                    If args.Count - 1 > 2 Then
                        If args(0) = "add" And (args(1) = AliasType.Shell Or args(1) = AliasType.RDebug) Then
                            ManageAlias(args(0), args(1), args(2), args(3))
                            Done = True
                        ElseIf args(0) = "add" And (args(1) <> AliasType.Shell Or args(1) <> AliasType.RDebug) Then
                            W(DoTranslation("Invalid type {0}.", currentLang), True, ColTypes.Neutral, args(1))
                        End If
                    ElseIf args.Count - 1 = 2 Then
                        If args(0) = "rem" And (args(1) = AliasType.Shell Or args(1) = AliasType.RDebug) Then
                            ManageAlias(args(0), args(1), args(2))
                            Done = True
                        ElseIf args(0) = "rem" And (args(1) <> AliasType.Shell Or args(1) <> AliasType.RDebug) Then
                            W(DoTranslation("Invalid type {0}.", currentLang), True, ColTypes.Neutral, args(1))
                        End If
                    End If
                End If

            ElseIf words(0) = "arginj" Then

                'Argument Injection
                If requestedCommand <> "arginj" Then
                    If args.Count - 1 >= 0 Then
                        Dim FinalArgs As New List(Of String)
                        For Each arg As String In args
                            Wdbg("I", "Parsing argument {0}...", arg)
                            If AvailableArgs.Contains(arg) Then
                                Wdbg("I", "Adding argument {0}...", arg)
                                FinalArgs.Add(arg)
                            End If
                        Next
                        If FinalArgs.Count = 0 Then
                            W(DoTranslation("No arguments specified. Hint: Specify multiple arguments separated by spaces", currentLang), True, ColTypes.Neutral)
                            Done = True
                        Else
                            answerargs = String.Join(",", FinalArgs)
                            argsInjected = True
                            W(DoTranslation("Injected arguments, {0}, will be scheduled to run at next reboot.", currentLang), True, ColTypes.Neutral, answerargs)
                            Done = True
                        End If
                    End If
                End If

            ElseIf words(0) = "bsynth" Then

                If eqargs?.Count > 0 Then
                    ProbeSynth(eqargs(0))
                    Done = True
                End If

            ElseIf requestedCommand = "cdbglog" Then

                Try
                    dbgWriter.Close()
                    dbgWriter = New StreamWriter(paths("Debugging")) With {.AutoFlush = True}
                    W(DoTranslation("Debug log removed. All connected debugging devices may still view messages.", currentLang), True, ColTypes.Neutral)
                Catch ex As Exception
                    W(DoTranslation("Debug log removal failed: {0}", currentLang), True, ColTypes.Neutral, ex.Message)
                    WStkTrc(ex)
                End Try
                Done = True

            ElseIf words(0) = "chdir" Then

                Try
                    SetCurrDir(strArgs)
                Catch ex As Exception
                    W(DoTranslation("Changing directory has failed: {0}", currentLang), True, ColTypes.Neutral, ex.Message)
                    WStkTrc(ex)
                End Try
                Done = True

            ElseIf words(0) = "chhostname" Then

                If requestedCommand <> "chhostname" Then
                    If words(1) = "" Then
                        W(DoTranslation("Blank host name.", currentLang), True, ColTypes.Neutral)
                    ElseIf words(1).IndexOfAny("[~`!@#$%^&*()-+=|{}':;.,<>/?]".ToCharArray) <> -1 Then
                        W(DoTranslation("Special characters are not allowed.", currentLang), True, ColTypes.Neutral)
                    Else
                        Done = True
                        W(DoTranslation("Changing from: {0} to {1}...", currentLang), True, ColTypes.Neutral, HName, words(1))
                        HName = words(1)
                        Dim ksconf As New IniFile()
                        Dim pathConfig As String = paths("Configuration")
                        ksconf.Load(pathConfig)
                        ksconf.Sections("Login").Keys("Host Name").Value = HName
                        ksconf.Save(pathConfig)
                    End If
                End If

            ElseIf words(0) = "chlang" Then

                If requestedCommand <> "chlang" Then
                    SetLang(words(1))
                    Done = True
                End If

            ElseIf words(0) = "chmotd" Then

                If requestedCommand <> "chmotd" Then
                    If strArgs = "" Then
                        W(DoTranslation("Blank message of the day.", currentLang), True, ColTypes.Neutral)
                    Else
                        W(DoTranslation("Changing MOTD...", currentLang), True, ColTypes.Neutral)
                        SetMOTD(strArgs, MessageType.MOTD)
                        Done = True
                    End If
                End If

            ElseIf words(0) = "chmal" Then

                If requestedCommand <> "chmal" Then
                    If strArgs = "" Then
                        W(DoTranslation("Blank MAL After Login.", currentLang), True, ColTypes.Neutral)
                    Else
                        W(DoTranslation("Changing MAL...", currentLang), True, ColTypes.Neutral)
                        SetMOTD(strArgs, MessageType.MAL)
                        Done = True
                    End If
                End If

            ElseIf words(0) = "chpwd" Then

                If requestedCommand <> "chpwd" Then
                    If eargs.Count - 1 = 3 Then
                        Try
                            If InStr(eargs(3), " ") > 0 Then
                                W(DoTranslation("Spaces are not allowed.", currentLang), True, ColTypes.Neutral)
                            ElseIf eargs(3) = eargs(2) Then
                                Dim hashbyte As Byte() = SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes(eargs(1)))
                                eargs(1) = GetArrayEnc(hashbyte)
                                If eargs(1) = userword(eargs(0)) Then
                                    If adminList(eargs(0)) And adminList(signedinusrnm) Then
                                        hashbyte = SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes(eargs(2)))
                                        eargs(2) = GetArrayEnc(hashbyte)
                                        userword.Item(eargs(0)) = eargs(2)
                                    ElseIf adminList(eargs(0)) And Not adminList(signedinusrnm) Then
                                        W(DoTranslation("You are not authorized to change password of {0} because the target was an admin.", currentLang), True, ColTypes.Neutral, eargs(0))
                                    End If
                                Else
                                    W(DoTranslation("Wrong user password.", currentLang), True, ColTypes.Neutral)
                                End If
                            ElseIf eargs(3) <> eargs(2) Then
                                W(DoTranslation("Passwords doesn't match.", currentLang), True, ColTypes.Neutral)
                            End If
                        Catch ex As KeyNotFoundException
                            W(DoTranslation("Username is wrong", currentLang), True, ColTypes.Neutral)
                            WStkTrc(ex)
                        End Try
                        Done = True
                    End If
                End If

            ElseIf words(0) = "chusrname" Then

                If requestedCommand <> "chusrname" Then
                    Dim DoneFlag As Boolean = False
                    If args.Count - 1 = 1 Then
                        If userword.ContainsKey(args(0)) = True Then
                            If Not userword.ContainsKey(args(1)) = True Then
                                DoneFlag = True
                                Dim temporary As String = userword(args(0))
                                userword.Remove(args(0))
                                userword.Add(args(1), temporary)
                                PermissionEditForNewUser(args(0), args(1))
                                W(DoTranslation("Username has been changed to {0}!", currentLang), True, ColTypes.Neutral, args(1))
                                If args(0) = signedinusrnm Then
                                    LoginPrompt()
                                End If
                            Else
                                W(DoTranslation("The new name you entered is already found.", currentLang), True, ColTypes.Neutral)
                                Exit Sub
                            End If
                        End If
                        If DoneFlag = False Then
                            W(DoTranslation("User {0} not found.", currentLang), True, ColTypes.Neutral, args(0))
                        End If
                        Done = True
                    End If
                End If

            ElseIf requestedCommand = "cls" Then

                Console.Clear() : Done = True

            ElseIf words(0) = "copy" Then

                If eqargs?.Length = 2 Then
                    eqargs(0) = CurrDir + "/" + eqargs(0).Replace("\", "/")
                    eqargs(1) = CurrDir + "/" + eqargs(1).Replace("\", "/")
                    If eqargs(0).Contains(CurrDir.Replace("\", "/")) And eqargs(0).AllIndexesOf(CurrDir.Replace("\", "/")).Count > 1 Then
                        eqargs(0) = ReplaceLastOccurrence(eqargs(0), CurrDir, "")
                    End If
                    If eqargs(1).Contains(CurrDir.Replace("\", "/")) And eqargs(1).AllIndexesOf(CurrDir.Replace("\", "/")).Count > 1 Then
                        eqargs(1) = ReplaceLastOccurrence(eqargs(1), CurrDir, "")
                    End If
                    Dim source As String = eqargs(0)
                    Dim target As String = eqargs(1)
                    If Directory.Exists(source) And Directory.Exists(target) Then
                        FileIO.FileSystem.CopyDirectory(source, target)
                    ElseIf File.Exists(source) And Directory.Exists(target) Then
                        FileIO.FileSystem.CopyFile(source, target)
                    ElseIf File.Exists(source) Then
                        FileIO.FileSystem.CopyFile(source, target)
                    Else
                        W(DoTranslation("The path is neither a file nor a directory.", currentLang), True, ColTypes.Neutral)
                    End If
                    Done = True
                End If

            ElseIf requestedCommand = "debuglog" Then

                Dim line As String
                Using dbglog = File.Open(paths("Debugging"), FileMode.OpenOrCreate, FileAccess.Read, FileShare.ReadWrite), reader As New StreamReader(dbglog)
                    line = reader.ReadLine()
                    Do While reader.EndOfStream <> True
                        W(line, True, ColTypes.Neutral)
                        line = reader.ReadLine
                    Loop
                End Using
                Done = True

            ElseIf words(0) = "dismissnotif" Then

                If requestedCommand <> "dismissnotif" Then
                    If args.Count - 1 = 0 Then
                        NotifDismiss(args(0))
                        Done = True
                    End If
                End If

            ElseIf words(0) = "disconndbgdev" Then

                If requestedCommand <> "disconndbgdev" Then
                    If args.Count - 1 = 0 Then
                        DisconnectDbgDevCmd(args(0))
                        Done = True
                    End If
                End If

            ElseIf words(0) = "ftp" Then

                If requestedCommand = "ftp" Then
                    InitiateShell()
                Else
                    InitiateShell(True, words(1))
                End If
                Done = True

            ElseIf words(0) = "get" Then

                If args.Count <> 0 Then
                    DownloadFile(args(0))
                    Done = True
                End If

            ElseIf requestedCommand = "lockscreen" Then

                Done = True
                LockMode = True
                ShowSavers(defSaverName)
                ShowPasswordPrompt(signedinusrnm)

            ElseIf words(0) = "list" Then

                'Lists folders and files
                If requestedCommand = "list" Then
                    List(CurrDir)
                    Done = True
                Else
                    Dim direct As String = CurrDir + "/" + strArgs.Replace("\", "/")
                    If direct.Contains(CurrDir.Replace("\", "/")) And direct.AllIndexesOf(CurrDir.Replace("\", "/")).Count > 1 Then
                        direct = ReplaceLastOccurrence(direct, CurrDir, "")
                    End If
                    List(direct)
                    Done = True
                End If

            ElseIf words(0) = "listdrives" Then

                Done = True
                Dim DriveNum As Integer = 1
                If EnvironmentOSType.Contains("Windows") Then
                    For Each HD As HDD In HDDList
                        W("==========================================", True, ColTypes.Neutral)
                        W(DoTranslation("Drive Number: {0}", currentLang), True, ColTypes.Neutral, DriveNum)
                        W("ID: {0}", True, ColTypes.Neutral, HD.ID)
                        W(DoTranslation("Manufacturer: {0}", currentLang), True, ColTypes.Neutral, HD.Manufacturer)
                        W(DoTranslation("Model: {0}", currentLang), True, ColTypes.Neutral, HD.Model)
                        W(DoTranslation("Capacity: {0} GB ({1}, {2}, {3})", currentLang), True, ColTypes.Neutral, FormatNumber(HD.Size / 1024 / 1024 / 1024, 2), HD.Cylinders, HD.Heads, HD.Sectors)
                        W(DoTranslation("Interface Type: {0}", currentLang), True, ColTypes.Neutral, HD.InterfaceType)
                        DriveNum += 1
                    Next
                    W("==========================================", True, ColTypes.Neutral)
                ElseIf EnvironmentOSType.Contains("Unix") Then
                    For Each HD As HDD_Linux In HDDList
                        W("==========================================", True, ColTypes.Neutral)
                        W(DoTranslation("Drive Number: {0}", currentLang), True, ColTypes.Neutral, DriveNum)
                        W(DoTranslation("Manufacturer: {0}", currentLang), True, ColTypes.Neutral, HD.Vendor_LNX)
                        W(DoTranslation("Model: {0}", currentLang), True, ColTypes.Neutral, HD.Model_LNX)
                        W(DoTranslation("Capacity: {0}", currentLang), True, ColTypes.Neutral, HD.Size_LNX)
                        W(DoTranslation("Partition Count: {0}", currentLang), True, ColTypes.Neutral, HD.Parts_LNX.Count)
                        DriveNum += 1
                    Next
                    W("==========================================", True, ColTypes.Neutral)
                End If

            ElseIf words(0) = "listparts" Then

                If args.Count > 0 Then
                    Done = True
                    Dim PartNum As Integer = 1
                    If EnvironmentOSType.Contains("Windows") Then
                        For Each P As Part In HDDList(args(0)).Parts
                            W("==========================================", True, ColTypes.Neutral)
                            W(DoTranslation("Physical partition: {0}", currentLang), True, ColTypes.Neutral, PartNum)
                            W(DoTranslation("Boot flag: {0} ({1})", currentLang), True, ColTypes.Neutral, P.Boot, P.Bootable)
                            W(DoTranslation("Primary flag: {0}", currentLang), True, ColTypes.Neutral, P.Primary)
                            W(DoTranslation("Size: {0} GB", currentLang), True, ColTypes.Neutral, FormatNumber(P.Size / 1024 / 1024 / 1024, 2))
                            PartNum += 1
                        Next
                    ElseIf EnvironmentOSType.Contains("Unix") Then
                        For Each P As Part_Linux In HDDList(args(0)).Parts
                            W("==========================================", True, ColTypes.Neutral)
                            W(DoTranslation("Physical partition: {0}", currentLang), True, ColTypes.Neutral, PartNum)
                            W(DoTranslation("File system: {0}", currentLang), True, ColTypes.Neutral, P.FileSystem)
                            W(DoTranslation("Size: {0}", currentLang), True, ColTypes.Neutral, P.SizeMEAS)
                            W(DoTranslation("Used: {0}", currentLang), True, ColTypes.Neutral, P.Used)
                            PartNum += 1
                        Next
                    End If
                    W("==========================================", True, ColTypes.Neutral)
                    If EnvironmentOSType.Contains("Windows") Then
                        PartNum = 1
                        W(DoTranslation("Listing all logical partitions on all drives.", currentLang), True, ColTypes.Neutral)
                        For Each LP As Logical In LogList
                            W(DoTranslation("Logical partition: {0}", currentLang), True, ColTypes.Neutral, PartNum)
                            W(DoTranslation("Compressed: {0}", currentLang), True, ColTypes.Neutral, LP.Compressed)
                            W(DoTranslation("Name: {0}", currentLang), True, ColTypes.Neutral, LP.Name)
                            W(DoTranslation("File system: {0}", currentLang), True, ColTypes.Neutral, LP.FileSystem)
                            W(DoTranslation("Capacity: {0} GB free of {1} GB", currentLang), True, ColTypes.Neutral, FormatNumber(LP.Free / 1024 / 1024 / 1024, 2), FormatNumber(LP.Size / 1024 / 1024 / 1024, 2))
                            W("==========================================", True, ColTypes.Neutral)
                            PartNum += 1
                        Next
                    End If
                End If

            ElseIf words(0) = "loteresp" Then

                Done = True
                InitializeLoteresp()

            ElseIf words(0) = "lset" Then

                If Not args.Count = 0 Then
                    Done = True
                    Try
                        FullParseMode = args(0)
                        Dim ksconf As New IniFile()
                        Dim pathConfig As String = paths("Configuration")
                        ksconf.Load(pathConfig)
                        ksconf.Sections("Misc").Keys("Size parse mode").Value = FullParseMode
                        ksconf.Save(pathConfig)
                        W(DoTranslation("Set successfully.", currentLang), True, ColTypes.Neutral)
                    Catch ex As Exception
                        W(DoTranslation("Error when trying to set parse mode. Check the value and try again. If this is correct, see the stack trace when kernel debugging is enabled.", currentLang), True, ColTypes.Neutral)
                        WStkTrc(ex)
                    End Try
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

                GetProperties()
                Done = True

            ElseIf words(0) = "mathbee" Then

                Done = True
                InitializeSolver()

            ElseIf words(0) = "md" Then

                If args.Count > 0 Then
                    'Create directory
                    Dim direct As String = CurrDir + "/" + strArgs.Replace("\", "/")
                    If direct.Contains(CurrDir.Replace("\", "/")) And direct.AllIndexesOf(CurrDir.Replace("\", "/")).Count > 1 Then
                        direct = ReplaceLastOccurrence(direct, CurrDir, "")
                    End If
                    If Not Directory.Exists(direct) Then
                        Directory.CreateDirectory(direct)
                    Else
                        W(DoTranslation("Directory {0} already exists.", currentLang), True, ColTypes.Neutral, direct)
                    End If
                    Done = True
                End If

            ElseIf words(0) = "move" Then

                If eqargs?.Length = 2 Then
                    eqargs(0) = CurrDir + "/" + eqargs(0).Replace("\", "/")
                    eqargs(1) = CurrDir + "/" + eqargs(1).Replace("\", "/")
                    If eqargs(0).Contains(CurrDir.Replace("\", "/")) And eqargs(0).AllIndexesOf(CurrDir.Replace("\", "/")).Count > 1 Then
                        eqargs(0) = ReplaceLastOccurrence(eqargs(0), CurrDir, "")
                    End If
                    If eqargs(1).Contains(CurrDir.Replace("\", "/")) And eqargs(1).AllIndexesOf(CurrDir.Replace("\", "/")).Count > 1 Then
                        eqargs(1) = ReplaceLastOccurrence(eqargs(1), CurrDir, "")
                    End If
                    Dim source As String = eqargs(0)
                    Dim target As String = eqargs(1)
                    Dim filesrc As String = Path.GetFileName(source)
                    If Directory.Exists(source) And Directory.Exists(target) Then
                        Directory.Move(source, target + "/" + filesrc)
                    ElseIf File.Exists(source) And Directory.Exists(target) Then
                        File.Move(source, target + "/" + filesrc)
                    ElseIf File.Exists(source) And Not File.Exists(target) Then
                        File.Move(source, target)
                    Else
                        W(DoTranslation("The path is neither a file nor a directory.", currentLang), True, ColTypes.Neutral)
                    End If
                    Done = True
                End If

            ElseIf words(0) = "perm" Then

                If requestedCommand <> "perm" Then
                    If args.Count - 1 = 2 Then
                        Permission(args(1), args(0), args(2))
                        Done = True
                    End If
                End If

            ElseIf words(0) = "read" Then

                If requestedCommand <> "read" Then
                    If args.Count - 1 = 0 Then
                        Dim FileRead As String = CurrDir + "/" + strArgs.Replace("\", "/")
                        If FileRead.Contains(CurrDir.Replace("\", "/")) And FileRead.AllIndexesOf(CurrDir.Replace("\", "/")).Count > 1 Then
                            FileRead = ReplaceLastOccurrence(FileRead, CurrDir, "")
                        End If
                        If File.Exists(FileRead) Then
                            ReadContents(FileRead)
                        Else
                            W(DoTranslation("{0} is not found.", currentLang), True, ColTypes.Neutral, strArgs)
                        End If
                        Done = True
                    End If
                End If

            ElseIf requestedCommand = "reloadconfig" Then

                'Reload configuration
                Done = True
                InitializeConfig()
                W(DoTranslation("Configuration reloaded. You might need to reboot the kernel for some changes to take effect.", currentLang), True, ColTypes.Neutral)

            ElseIf words(0) = "reboot" Then

                'Reboot the simulated system
                Done = True
                If Not args.Length = 0 Then
                    If args(0) = "safe" Then
                        PowerManage("rebootsafe")
                    ElseIf args(0) <> "" Then
                        PowerManage("remoterestart", args(0))
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
                    W(DoTranslation("Reloading not allowed in safe mode.", currentLang), True, ColTypes.Neutral)
                End If

            ElseIf words(0) = "reloadsaver" Then

                If requestedCommand <> "reloadsaver" Then
                    If args.Count - 1 >= 0 Then
                        If Not SafeMode Then
                            CompileCustom(strArgs)
                        Else
                            W(DoTranslation("Reloading not allowed in safe mode.", currentLang), True, ColTypes.Neutral)
                        End If
                        Done = True
                    End If
                End If

            ElseIf words(0) = "rexec" Then

                If requestedCommand <> "rexec" Then
                    If eqargs.Count > 1 Then
                        Done = True
                        SendCommand("<Request:Exec>(" + eqargs(1) + ")", args(0))
                    End If
                End If

            ElseIf words(0) = "rd" Then

                If args.Count - 1 >= 0 Then
                    Try
                        Dim Dir As String = CurrDir + "/" + strArgs.Replace("\", "/")
                        If Dir.Contains(CurrDir.Replace("\", "/")) And Dir.AllIndexesOf(CurrDir.Replace("\", "/")).Count > 1 Then
                            Dir = ReplaceLastOccurrence(Dir, CurrDir, "")
                        End If
                        Directory.Delete(Dir, True)
                    Catch ex As Exception
                        W(DoTranslation("Unable to remove directory: {0}", currentLang), True, ColTypes.Neutral, ex.Message)
                        WStkTrc(ex)
                    End Try
                    Done = True
                End If

            ElseIf words(0) = "rdebug" Then

                If DebugMode Then
                    If RDebugThread.IsAlive Then
                        StartRDebugThread(False)
                    Else
                        StartRDebugThread(True)
                    End If
                Else
                    W(DoTranslation("Debugging not enabled.", currentLang), True, ColTypes.Neutral)
                End If
                Done = True

            ElseIf words(0) = "rmuser" Then

                If requestedCommand <> "rmuser" Then
                    If args.Count - 1 = 0 Then
                        RemoveUserFromDatabase(args(0))
                        Done = True
                    End If
                End If

            ElseIf requestedCommand = "savescreen" Then

                Done = True
                ShowSavers(defSaverName)

            ElseIf words(0) = "search" Then

                If eqargs.Count = 2 Then
                    Dim ToBeFound As String = eqargs(0)
                    Dim Dir As String = CurrDir + "/" + eqargs(1).Replace("\", "/")
                    If Dir.Contains(CurrDir.Replace("\", "/")) And Dir.AllIndexesOf(CurrDir.Replace("\", "/")).Count > 1 Then
                        Dir = ReplaceLastOccurrence(Dir, CurrDir, "")
                    End If
                    Dim Filebyte() As String = File.ReadAllLines(Dir)
                    Dim MatchNum As Integer = 1
                    For Each Str As String In Filebyte
                        If Str.Contains(ToBeFound) Then
                            W(DoTranslation("Match {0}: {1}", currentLang), True, ColTypes.Neutral, MatchNum, Str)
                            MatchNum += 1
                        End If
                    Next
                    Done = True
                End If

            ElseIf words(0) = "setcolors" Then

                If requestedCommand <> "setcolors" Then
                    If args.Count - 1 = 9 Then
                        Done = True
                        If ColoredShell = True Then
                            If args.Contains("RESET") Then
                                ResetColors()
                                W(DoTranslation("Everything is reset to normal settings.", currentLang), True, ColTypes.Neutral)
                            ElseIf args.Contains("def") Then
                                If args(0) = "def" Then
                                    args(0) = "White"
                                    inputColor = CType([Enum].Parse(GetType(ConsoleColors), args(0)), ConsoleColors)
                                End If
                                If args(1) = "def" Then
                                    args(1) = "White"
                                    licenseColor = CType([Enum].Parse(GetType(ConsoleColors), args(1)), ConsoleColors)
                                End If
                                If args(2) = "def" Then
                                    args(2) = "Yellow"
                                    contKernelErrorColor = CType([Enum].Parse(GetType(ConsoleColors), args(2)), ConsoleColors)
                                End If
                                If args(3) = "def" Then
                                    args(3) = "Red"
                                    uncontKernelErrorColor = CType([Enum].Parse(GetType(ConsoleColors), args(3)), ConsoleColors)
                                End If
                                If args(4) = "def" Then
                                    args(4) = "DarkGreen"
                                    hostNameShellColor = CType([Enum].Parse(GetType(ConsoleColors), args(4)), ConsoleColors)
                                End If
                                If args(5) = "def" Then
                                    args(5) = "Green"
                                    userNameShellColor = CType([Enum].Parse(GetType(ConsoleColors), args(5)), ConsoleColors)
                                End If
                                If args(6) = "def" Then
                                    args(6) = "Black"
                                    backgroundColor = CType([Enum].Parse(GetType(ConsoleColors), args(6)), ConsoleColors)
                                    Load()
                                End If
                                If args(7) = "def" Then
                                    args(7) = "Gray"
                                    neutralTextColor = CType([Enum].Parse(GetType(ConsoleColors), args(7)), ConsoleColors)
                                End If
                                If args(8) = "def" Then
                                    args(8) = "DarkYellow"
                                    cmdListColor = CType([Enum].Parse(GetType(ConsoleColors), args(8)), ConsoleColors)
                                End If
                                If args(9) = "def" Then
                                    args(9) = "DarkGray"
                                    cmdDefColor = CType([Enum].Parse(GetType(ConsoleColors), args(9)), ConsoleColors)
                                End If
                            ElseIf IsNumeric(args(0)) And IsNumeric(args(1)) And IsNumeric(args(2)) And IsNumeric(args(3)) And IsNumeric(args(4)) And
                                IsNumeric(args(5)) And IsNumeric(args(6)) And IsNumeric(args(7)) And IsNumeric(args(8)) And IsNumeric(args(9)) And
                                args(0) <= 255 And args(1) <= 255 And args(2) <= 255 And args(3) <= 255 And args(4) <= 255 And args(5) <= 255 And
                                args(6) <= 255 And args(7) <= 255 And args(8) <= 255 And args(9) <= 255 Then
                                inputColor = CType([Enum].Parse(GetType(ConsoleColors), args(0)), ConsoleColors)
                                licenseColor = CType([Enum].Parse(GetType(ConsoleColors), args(1)), ConsoleColors)
                                contKernelErrorColor = CType([Enum].Parse(GetType(ConsoleColors), args(2)), ConsoleColors)
                                uncontKernelErrorColor = CType([Enum].Parse(GetType(ConsoleColors), args(3)), ConsoleColors)
                                hostNameShellColor = CType([Enum].Parse(GetType(ConsoleColors), args(4)), ConsoleColors)
                                userNameShellColor = CType([Enum].Parse(GetType(ConsoleColors), args(5)), ConsoleColors)
                                backgroundColor = CType([Enum].Parse(GetType(ConsoleColors), args(6)), ConsoleColors)
                                neutralTextColor = CType([Enum].Parse(GetType(ConsoleColors), args(7)), ConsoleColors)
                                cmdListColor = CType([Enum].Parse(GetType(ConsoleColors), args(8)), ConsoleColors)
                                cmdDefColor = CType([Enum].Parse(GetType(ConsoleColors), args(9)), ConsoleColors)
                                Load()
                            Else
                                W(DoTranslation("One or more of the colors is invalid.", currentLang), True, ColTypes.Neutral)
                            End If
                            MakePermanent()
                        Else
                            W(DoTranslation("Colors are not available. Turn on colored shell in the kernel config.", currentLang), True, ColTypes.Neutral)
                        End If
                    End If
                End If

            ElseIf words(0) = "setsaver" Then

                Dim modPath As String = paths("Mods")
                If requestedCommand <> "setsaver" Then
                    If args.Count >= 0 Then
                        If ScrnSvrdb.ContainsKey(strArgs) Then
                            SetDefaultScreensaver(strArgs)
                        Else
                            If FileIO.FileSystem.FileExists($"{modPath}{strArgs}") And Not SafeMode Then
                                SetDefaultScreensaver(strArgs)
                            Else
                                W(DoTranslation("Screensaver {0} not found.", currentLang), True, ColTypes.Neutral, strArgs)
                            End If
                        End If
                        Done = True
                    End If
                End If

            ElseIf words(0) = "setthemes" Then

                If requestedCommand <> "setthemes" Then
                    If args.Count - 1 = 0 Then
                        If ColoredShell = True Then TemplateSet(args(0)) Else W(DoTranslation("Colors are not available. Turn on colored shell in the kernel config.", currentLang), True, ColTypes.Neutral)
                        Done = True
                    End If
                End If

            ElseIf words(0) = "shownotifs" Then

                Dim Count As Integer = 1
                For Each Notif As Notification In NotifRecents
                    W($"{Count}: {Notif.Title}" + vbNewLine +
                      $"{Count}: {Notif.Desc}" + vbNewLine +
                      $"{Count}: {Notif.Priority.ToString}", True, ColTypes.Neutral)
                    Count += 1
                Next
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
                        If eargs(0) = "all" Then
                            ShowTimesInZones()
                            Done = True
                        Else
                            W(DoTranslation("Timezone is specified incorrectly.", currentLang), True, ColTypes.Neutral)
                            Done = True
                        End If
                    End If
                End If

            ElseIf words(0) = "shutdown" Then

                'Shuts down the simulated system
                Done = True
                If Not args.Length = 0 Then
                    If args(0) <> "" Then
                        PowerManage("remoteshutdown", args(0))
                    End If
                Else
                    PowerManage("shutdown")
                End If


            ElseIf words(0) = "speak" Then

                Speak(strArgs)
                Done = True

            ElseIf words(0) = "spellbee" Then

                Done = True
                InitializeWords()

            ElseIf words(0) = "sshell" Then

                If eqargs?.Length = 3 Then
                    InitializeSSH(eqargs(0), eqargs(1), eqargs(2))
                    Done = True
                End If

            ElseIf words(0) = "sumfile" Then

                If args.Length >= 2 Then
                    Done = True
                    Dim file As String
                    eqargs(1) = eqargs(1).Replace("\", "/")
                    file = $"{CurrDir}/{eqargs(1)}"
                    If file.Contains(CurrDir.Replace("\", "/")) And file.AllIndexesOf(CurrDir.Replace("\", "/")).Count > 1 Then
                        file = ReplaceLastOccurrence(file, CurrDir, "")
                    End If
                    If IO.File.Exists(file) Then
                        If args(0) = "SHA256" Then
                            Dim spent As New Stopwatch
                            spent.Start() 'Time when you're on a breakpoint is counted
                            Dim hashbyte As Byte() = SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes(IO.File.ReadAllText(file)))
                            W(GetArrayEnc(hashbyte), True, ColTypes.Neutral)
                            W(DoTranslation("Time spent: {0} milliseconds", currentLang), True, ColTypes.Neutral, spent.ElapsedMilliseconds)
                            spent.Stop()
                        ElseIf args(0) = "MD5" Then
                            Dim spent As New Stopwatch
                            spent.Start() 'Time when you're on a breakpoint is counted
                            Dim hashbyte As Byte() = MD5.Create().ComputeHash(Encoding.UTF8.GetBytes(IO.File.ReadAllText(file)))
                            W(GetArrayEnc(hashbyte), True, ColTypes.Neutral)
                            W(DoTranslation("Time spent: {0} milliseconds", currentLang), True, ColTypes.Neutral, spent.ElapsedMilliseconds)
                            spent.Stop()
                        Else
                            W(DoTranslation("Invalid encryption algorithm.", currentLang), True, ColTypes.Neutral)
                        End If
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
                If Not EnvironmentOSType.Contains("Unix") Then
                    ListDrivers()
                Else
                    ListDrivers_Linux()
                End If

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

            ElseIf words(0) = "usermanual" Then

                Done = True
                Process.Start("https://github.com/EoflaOE/Kernel-Simulator/wiki")

            End If
            If Done = False Then
                Throw New EventsAndExceptions.NotEnoughArgumentsException(DoTranslation("There was not enough arguments. See below for usage:", currentLang))
            End If
        Catch neaex As EventsAndExceptions.NotEnoughArgumentsException
            Wdbg("W", "User hasn't provided enough arguments for {0}", words(0))
            W(neaex.Message, True, ColTypes.Neutral)
            ShowHelp(words(0))
        Catch ex As Exception
            If DebugMode = True Then
                W(DoTranslation("Error trying to execute command", currentLang) + " {3}." + vbNewLine + DoTranslation("Error {0}: {1}", currentLang) + vbNewLine + "{2}", True, ColTypes.Neutral,
                    Err.Number, ex.Message, ex.StackTrace, words(0))
                WStkTrc(ex)
            Else
                W(DoTranslation("Error trying to execute command", currentLang) + " {2}." + vbNewLine + DoTranslation("Error {0}: {1}", currentLang), True, ColTypes.Neutral, Err.Number, ex.Message, words(0))
            End If
        End Try
    End Sub

End Module
