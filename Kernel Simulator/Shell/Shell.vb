
'    Kernel Simulator  Copyright (C) 2018-2022  Aptivi
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
Imports KS.Files.PathLookup
Imports KS.Files.Querying
Imports KS.Misc.Execution
Imports KS.Modifications
Imports KS.Scripting
Imports KS.Shell.Commands
Imports KS.Shell.ShellBase.Aliases
Imports KS.Shell.UnifiedCommands

Namespace Shell
    Public Module Shell

        Friend OutputTextWriter As StreamWriter
        Friend OutputStream As FileStream
        Friend ProcessStartCommandThread As New KernelThread("Executable Command Thread", False, AddressOf ExecuteProcess)

        ''' <summary>
        ''' Whether the shell is colored or not
        ''' </summary>
        Public ColoredShell As Boolean = True
        ''' <summary>
        ''' Specifies where to lookup for executables in these paths. Same as in PATH implementation.
        ''' </summary>
        Public PathsToLookup As String = Environment.GetEnvironmentVariable("PATH")
        ''' <summary>
        ''' Path lookup delimiter, depending on the operating system
        ''' </summary>
        Public ReadOnly PathLookupDelimiter As String = Path.PathSeparator

        ''' <summary>
        ''' List of unified commands
        ''' </summary>
        Public ReadOnly UnifiedCommandDict As New Dictionary(Of String, CommandInfo) From {
            {"presets", New CommandInfo("presets", ShellType.Shell, "Opens the shell preset library", New CommandArgumentInfo(), New PresetsUnifiedCommand)},
            {"exit", New CommandInfo("exit", ShellType.Shell, "Exits the shell if running on subshell", New CommandArgumentInfo(), New ExitUnifiedCommand)},
            {"help", New CommandInfo("help", ShellType.Shell, "Help page", New CommandArgumentInfo({"[command]"}, False, 0, AddressOf HelpUnifiedCommand.ListCmds), New HelpUnifiedCommand)}
        }

        ''' <summary>
        ''' Current shell type
        ''' </summary>
        Public ReadOnly Property CurrentShellType As ShellType
            Get
                Return ShellStack(ShellStack.Count - 1).ShellType
            End Get
        End Property

        ''' <summary>
        ''' Last shell type
        ''' </summary>
        Public ReadOnly Property LastShellType As ShellType
            Get
                If ShellStack.Count = 0 Then
                    'We don't have any shell. Return Shell.
                    Return ShellType.Shell
                ElseIf ShellStack.Count = 1 Then
                    'We only have one shell. Consider current as last.
                    Return CurrentShellType
                Else
                    'We have more than one shell. Return the shell type for a shell before the last one.
                    Return ShellStack(ShellStack.Count - 2).ShellType
                End If
            End Get
        End Property

        ''' <summary>
        ''' Parses a specified command.
        ''' </summary>
        ''' <param name="FullCommand">The full command string</param>
        ''' <remarks>All new shells implemented either in KS or by mods should use this routine to allow effective and consistent line parsing.</remarks>
        Public Sub GetLine(FullCommand As String)
            GetLine(FullCommand, "", CurrentShellType)
        End Sub

        ''' <summary>
        ''' Parses a specified command.
        ''' </summary>
        ''' <param name="FullCommand">The full command string</param>
        ''' <param name="OutputPath">Optional (non-)neutralized output path</param>
        ''' <param name="ShellType">Shell type</param>
        ''' <remarks>All new shells implemented either in KS or by mods should use this routine to allow effective and consistent line parsing.</remarks>
        Public Sub GetLine(FullCommand As String, Optional OutputPath As String = "", Optional ShellType As ShellType = ShellType.Shell)
            'Check for sanity
            If String.IsNullOrEmpty(FullCommand) Then FullCommand = ""

            'Variables
            Dim TargetFile As String = ""
            Dim TargetFileName As String = ""

            'Check for a type of command
            Dim SplitCommands As String() = FullCommand.Split({" : "}, StringSplitOptions.RemoveEmptyEntries)
            Dim Commands As Dictionary(Of String, CommandInfo) = GetCommands(ShellType)
            For Each Command As String In SplitCommands

                'Check to see if the command is a comment
                If Not (Command = Nothing Or Command?.StartsWithAnyOf({" ", "#"})) Then
                    'Get the index of the first space
                    Dim indexCmd As Integer = Command.IndexOf(" ")
                    Dim cmdArgs As String = Command 'Command with args
                    Wdbg(DebugLevel.I, "Prototype indexCmd and Command: {0}, {1}", indexCmd, Command)
                    If indexCmd = -1 Then indexCmd = Command.Length
                    Command = Command.Substring(0, indexCmd)
                    Wdbg(DebugLevel.I, "Finished indexCmd and Command: {0}, {1}", indexCmd, Command)

                    'Parse script command (if any)
                    Dim scriptArgs As List(Of String) = Command.Split({".uesh "}, StringSplitOptions.RemoveEmptyEntries).ToList
                    scriptArgs.RemoveAt(0)

                    'Get command parts
                    Dim Parts As String() = Command.SplitSpacesEncloseDoubleQuotes
                    'Reads command written by user
                    Try
                        'Set title
                        SetTitle($"{ConsoleTitle} - {Command}")

                        'Iterate through mod commands
                        Wdbg(DebugLevel.I, "Mod commands probing started with {0} from {1}", Command, FullCommand)
                        If ListModCommands(ShellType).ContainsKey(Parts(0)) Then
                            Wdbg(DebugLevel.I, "Mod command: {0}", Parts(0))
                            ExecuteModCommand(Command)
                        End If

                        'Iterate through alias commands
                        Wdbg(DebugLevel.I, "Aliases probing started with {0} from {1}", Command, FullCommand)
                        If GetAliasesListFromType(ShellType).ContainsKey(Parts(0)) Then
                            Wdbg(DebugLevel.I, "Alias: {0}", Parts(0))
                            ExecuteAlias(Command, ShellType)
                        End If

                        'Execute the built-in command
                        If Commands.ContainsKey(Command) Then
                            Wdbg(DebugLevel.I, "Executing built-in command")

                            'Check to see if the command supports redirection
                            If Commands(Command).Flags.HasFlag(CommandFlags.RedirectionSupported) Then
                                Wdbg(DebugLevel.I, "Redirection supported!")
                                InitializeRedirection(Command, OutputPath)
                            End If
                            If Not (Command = Nothing Or Command.StartsWithAnyOf({" ", "#"}) = True) Then

                                'Check to see if a user is able to execute a command
                                If ShellType = ShellType.Shell Then
                                    If HasPermission(CurrentUser.Username, PermissionType.Administrator) = False And Commands(Command).Flags.HasFlag(CommandFlags.Strict) Then
                                        Wdbg(DebugLevel.W, "Cmd exec {0} failed: adminList(signedinusrnm) is False, strictCmds.Contains({0}) is True", Command)
                                        Write(DoTranslation("You don't have permission to use {0}"), True, ColTypes.Error, Command)
                                        Exit Try
                                    End If
                                End If

                                'Check the command before starting
                                If Maintenance = True And Commands(Command).Flags.HasFlag(CommandFlags.NoMaintenance) Then
                                    Wdbg(DebugLevel.W, "Cmd exec {0} failed: In maintenance mode. {0} is in NoMaintenanceCmds", Command)
                                    Write(DoTranslation("Shell message: The requested command {0} is not allowed to run in maintenance mode."), True, ColTypes.Error, Command)
                                Else
                                    Wdbg(DebugLevel.I, "Cmd exec {0} succeeded. Running with {1}", Command, cmdArgs)
                                    Dim Params As New ExecuteCommandThreadParameters(FullCommand, ShellType, Nothing)

                                    'Since we're probably trying to run a command using the alternative command threads, if the main shell command thread
                                    'is running, use that to execute the command. This ensures that commands like "wrap" that also execute commands from the
                                    'shell can do their job.
                                    Dim ShellInstance As ShellInfo = ShellStack(ShellStack.Count - 1)
                                    Dim StartCommandThread As KernelThread = ShellInstance.ShellCommandThread
                                    Dim CommandThreadValid As Boolean = True
                                    If StartCommandThread.IsAlive Then
                                        If ShellInstance.AltCommandThreads.Count > 0 Then
                                            StartCommandThread = ShellInstance.AltCommandThreads(ShellInstance.AltCommandThreads.Count - 1)
                                        Else
                                            Wdbg(DebugLevel.W, "Cmd exec {0} failed: Alt command threads are not there.")
                                            CommandThreadValid = False
                                        End If
                                    End If
                                    If CommandThreadValid Then
                                        StartCommandThread.Start(Params)
                                        StartCommandThread.Wait()
                                        StartCommandThread.Stop()
                                    End If
                                End If
                            End If
                        ElseIf TryParsePath(TargetFile) And ShellType = ShellType.Shell Then
                            'Scan PATH for file existence and set file name as needed
                            FileExistsInPath(Command, TargetFile)
                            If String.IsNullOrEmpty(TargetFile) Then TargetFile = NeutralizePath(Command)
                            If TryParsePath(TargetFile) Then TargetFileName = Path.GetFileName(TargetFile)

                            'If we're in the UESH shell, parse the script file or executable file
                            If FileExists(TargetFile) And Not TargetFile.EndsWith(".uesh") Then
                                Wdbg(DebugLevel.I, "Cmd exec {0} succeeded because file is found.", Command)
                                Try
                                    'Create a new instance of process
                                    If TryParsePath(TargetFile) Then
                                        cmdArgs = cmdArgs.Replace(TargetFileName, "")
                                        cmdArgs.RemoveNullsOrWhitespacesAtTheBeginning()
                                        Wdbg(DebugLevel.I, "Command: {0}, Arguments: {1}", TargetFile, cmdArgs)
                                        Dim Params As New ExecuteProcessThreadParameters(TargetFile, cmdArgs)
                                        ProcessStartCommandThread.Start(Params)
                                        ProcessStartCommandThread.Wait()
                                        ProcessStartCommandThread.Stop()
                                    End If
                                Catch ex As Exception
                                    Wdbg(DebugLevel.E, "Failed to start process: {0}", ex.Message)
                                    Write(DoTranslation("Failed to start ""{0}"": {1}"), True, ColTypes.Error, Command, ex.Message)
                                    WStkTrc(ex)
                                End Try
                            ElseIf FileExists(TargetFile) And TargetFile.EndsWith(".uesh") Then
                                Try
                                    Wdbg(DebugLevel.I, "Cmd exec {0} succeeded because it's a UESH script.", Command)
                                    Execute(TargetFile, scriptArgs.Join(" "))
                                Catch ex As Exception
                                    Write(DoTranslation("Error trying to execute script: {0}"), True, ColTypes.Error, ex.Message)
                                    WStkTrc(ex)
                                End Try
                            Else
                                Wdbg(DebugLevel.W, "Cmd exec {0} failed: availableCmds.Cont({0}.Substring(0, {1})) = False", Command, indexCmd)
                                Write(DoTranslation("Shell message: The requested command {0} is not found. See 'help' for available commands."), True, ColTypes.Error, Command)
                            End If
                        Else
                            Wdbg(DebugLevel.W, "Cmd exec {0} failed: availableCmds.Cont({0}.Substring(0, {1})) = False", Command, indexCmd)
                            Write(DoTranslation("Shell message: The requested command {0} is not found. See 'help' for available commands."), True, ColTypes.Error, Command)
                        End If

                        'Restore title
                        SetTitle(ConsoleTitle)
                    Catch ex As Exception
                        WStkTrc(ex)
                        Write(DoTranslation("Error trying to execute command.") + NewLine +
                              DoTranslation("Error {0}: {1}"), True, ColTypes.Error, ex.GetType.FullName, ex.Message)
                    End Try
                End If
            Next

            'Restore console output to its original state if any
            If DefConsoleOut IsNot Nothing Then
                Console.SetOut(DefConsoleOut)
                OutputTextWriter?.Close()
            End If
        End Sub

        ''' <summary>
        ''' Initializes the redirection
        ''' </summary>
        Private Sub InitializeRedirection(ByRef Command As String, OutputPath As String)
            'If requested command has output redirection sign after arguments, remove it from final command string and set output to that file
            Wdbg(DebugLevel.I, "Does the command contain the redirection sign "">>>"" or "">>""? {0} and {1}", Command.Contains(">>>"), Command.Contains(">>"))
            If Command.Contains(">>>") Then
                Wdbg(DebugLevel.I, "Output redirection found with append.")
                Dim OutputFileName As String = Command.Substring(Command.LastIndexOf(">") + 2)
                DefConsoleOut = Console.Out
                OutputStream = New FileStream(NeutralizePath(OutputFileName), FileMode.Append, FileAccess.Write)
                OutputTextWriter = New StreamWriter(OutputStream) With {.AutoFlush = True}
                Console.SetOut(OutputTextWriter)
                Command = Command.Replace(" >>> " + OutputFileName, "")
            ElseIf Command.Contains(">>") Then
                Wdbg(DebugLevel.I, "Output redirection found with overwrite.")
                Dim OutputFileName As String = Command.Substring(Command.LastIndexOf(">") + 2)
                DefConsoleOut = Console.Out
                OutputStream = New FileStream(NeutralizePath(OutputFileName), FileMode.OpenOrCreate, FileAccess.Write)
                OutputTextWriter = New StreamWriter(OutputStream) With {.AutoFlush = True}
                Console.SetOut(OutputTextWriter)
                Command = Command.Replace(" >> " + OutputFileName, "")
            End If

            'Checks to see if the user provided optional path
            If Not String.IsNullOrWhiteSpace(OutputPath) Then
                Wdbg(DebugLevel.I, "Optional output redirection found using OutputPath ({0}).", OutputPath)
                DefConsoleOut = Console.Out
                OutputStream = New FileStream(NeutralizePath(OutputPath), FileMode.OpenOrCreate, FileAccess.Write)
                OutputTextWriter = New StreamWriter(OutputStream) With {.AutoFlush = True}
                Console.SetOut(OutputTextWriter)
            End If
        End Sub

    End Module
End Namespace
