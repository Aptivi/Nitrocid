
// Nitrocid KS  Copyright (C) 2018-2023  Aptivi
// 
// This file is part of Nitrocid KS
// 
// Nitrocid KS is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// Nitrocid KS is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Extensification.StringExts;
using KS.ConsoleBase;
using KS.ConsoleBase.Colors;
using KS.ConsoleBase.Inputs;
using KS.Files;
using KS.Files.PathLookup;
using KS.Files.Querying;
using KS.Kernel;
using KS.Kernel.Debugging;
using KS.Languages;
using KS.Misc.Execution;
using KS.Misc.Text;
using KS.Misc.Threading;
using KS.Misc.Writers.ConsoleWriters;
using KS.Drivers;
using KS.Modifications;
using KS.Scripting;
using KS.Shell.Prompts;
using KS.Shell.ShellBase.Aliases;
using KS.Shell.ShellBase.Commands;
using KS.Shell.ShellBase.Shells;
using KS.Shell.ShellBase.Commands.UnifiedCommands;
using KS.Shell.Shells.UESH;
using KS.Shell.Shells.FTP;
using KS.Shell.Shells.Mail;
using KS.Shell.Shells.SFTP;
using KS.Shell.Shells.Text;
using KS.Shell.Shells.RSS;
using KS.Shell.Shells.Json;
using KS.Shell.Shells.HTTP;
using KS.Shell.Shells.Hex;
using KS.Shell.Shells.Archive;
using KS.Shell.Shells.Admin;
using KS.Users.Login;
using KS.Kernel.Events;
using File = KS.Drivers.Console.Consoles.File;
using static KS.Users.UserManagement;
using KS.Users.Permissions;
using KS.Drivers.Console;

namespace KS.Shell
{
    /// <summary>
    /// Base shell module
    /// </summary>
    public static class Shell
    {

        internal static StreamWriter OutputTextWriter;
        internal static FileStream OutputStream;
        internal static KernelThread ProcessStartCommandThread = new("Executable Command Thread", false, (processParams) => ProcessExecutor.ExecuteProcess((ProcessExecutor.ExecuteProcessThreadParameters)processParams)) { isCritical = true };

        /// <summary>
        /// Path lookup delimiter, depending on the operating system
        /// </summary>
        public readonly static string PathLookupDelimiter = Convert.ToString(Path.PathSeparator);

        /// <summary>
        /// List of unified commands
        /// </summary>
        public readonly static Dictionary<string, CommandInfo> UnifiedCommandDict = new()
        {
            { "presets", new CommandInfo("presets", ShellType.Shell, /* Localizable */ "Opens the shell preset library", new CommandArgumentInfo(), new PresetsUnifiedCommand()) },
            { "exit", new CommandInfo("exit", ShellType.Shell, /* Localizable */ "Exits the shell if running on subshell", new CommandArgumentInfo(), new ExitUnifiedCommand()) },
            { "help", new CommandInfo("help", ShellType.Shell, /* Localizable */ "Help page", new CommandArgumentInfo(new[] { "[command]" }, false, 0, (startFrom, _, _) => HelpUnifiedCommand.ListCmds(startFrom)), new HelpUnifiedCommand()) }
        };

        /// <summary>
        /// List of available shells
        /// </summary>
        internal readonly static Dictionary<string, BaseShellInfo> AvailableShells = new()
        {
            { "Shell", new UESHShellInfo() },
            { "FTPShell", new FTPShellInfo() },
            { "MailShell", new MailShellInfo() },
            { "SFTPShell", new SFTPShellInfo() },
            { "TextShell", new TextShellInfo() },
            { "RSSShell", new RSSShellInfo() },
            { "JsonShell", new JsonShellInfo() },
            { "HTTPShell", new HTTPShellInfo() },
            { "HexShell", new HexShellInfo() },
            { "ArchiveShell", new ArchiveShellInfo() },
            { "AdminShell", new AdminShellInfo() }
        };

        /// <summary>
        /// Current shell type
        /// </summary>
        public static string CurrentShellType => ShellStart.ShellStack[ShellStart.ShellStack.Count - 1].ShellType;

        /// <summary>
        /// Last shell type
        /// </summary>
        public static string LastShellType
        {
            get
            {
                if (ShellStart.ShellStack.Count == 0)
                {
                    // We don't have any shell. Return Shell.
                    return "Shell";
                }
                else if (ShellStart.ShellStack.Count == 1)
                {
                    // We only have one shell. Consider current as last.
                    return CurrentShellType;
                }
                else
                {
                    // We have more than one shell. Return the shell type for a shell before the last one.
                    return ShellStart.ShellStack[ShellStart.ShellStack.Count - 2].ShellType;
                }
            }
        }

        /// <summary>
        /// Specifies where to lookup for executables in these paths. Same as in PATH implementation.
        /// </summary>
        public static string PathsToLookup { get; set; } = Environment.GetEnvironmentVariable("PATH");

        /// <summary>
        /// Inputs for command then parses a specified command.
        /// </summary>
        /// <remarks>All new shells implemented either in KS or by mods should use this routine to allow effective and consistent line parsing.</remarks>
        public static void GetLine() => GetLine("", "", CurrentShellType);

        /// <summary>
        /// Parses a specified command.
        /// </summary>
        /// <param name="FullCommand">The full command string</param>
        /// <remarks>All new shells implemented either in KS or by mods should use this routine to allow effective and consistent line parsing.</remarks>
        public static void GetLine(string FullCommand) => GetLine(FullCommand, "", CurrentShellType);

        /// <summary>
        /// Parses a specified command.
        /// </summary>
        /// <param name="FullCommand">The full command string</param>
        /// <param name="OutputPath">Optional (non-)neutralized output path</param>
        /// <remarks>All new shells implemented either in KS or by mods should use this routine to allow effective and consistent line parsing.</remarks>
        public static void GetLine(string FullCommand, string OutputPath = "") => GetLine(FullCommand, OutputPath, CurrentShellType);

        /// <summary>
        /// Parses a specified command.
        /// </summary>
        /// <param name="FullCommand">The full command string</param>
        /// <param name="OutputPath">Optional (non-)neutralized output path</param>
        /// <param name="ShellType">Shell type</param>
        /// <remarks>All new shells implemented either in KS or by mods should use this routine to allow effective and consistent line parsing.</remarks>
        public static void GetLine(string FullCommand, string OutputPath = "", ShellType ShellType = ShellType.Shell) =>
            GetLine(FullCommand, OutputPath, GetShellTypeName(ShellType));

        /// <summary>
        /// Parses a specified command.
        /// </summary>
        /// <param name="FullCommand">The full command string</param>
        /// <param name="OutputPath">Optional (non-)neutralized output path</param>
        /// <param name="ShellType">Shell type</param>
        /// <remarks>All new shells implemented either in KS or by mods should use this routine to allow effective and consistent line parsing.</remarks>
        public static void GetLine(string FullCommand, string OutputPath = "", string ShellType = "Shell")
        {
            // Check for sanity
            if (string.IsNullOrEmpty(FullCommand))
                FullCommand = "";

            // Variables
            string TargetFile = "";
            string TargetFileName = "";

            // Check to see if the full command string ends with the semicolon
            while (FullCommand.EndsWith(";") || string.IsNullOrEmpty(FullCommand))
            {
                // Enable cursor
                ConsoleWrapper.CursorVisible = true;

                // Tell the user to provide the command
                StringBuilder commandBuilder = new(FullCommand);

                // We need to put a synclock in the below steps, because the cancellation handlers seem to be taking their time to try to suppress the
                // thread abort error messages. If the shell tried to write to the console while these handlers were still working, the command prompt
                // would either be incomplete or not printed to the console at all.
                lock (CancellationHandlers.GetCancelSyncLock(ShellType))
                {
                    // Print a prompt
                    if (!string.IsNullOrEmpty(FullCommand))
                        PromptPresetManager.WriteShellCompletionPrompt(ShellType);
                    else
                        PromptPresetManager.WriteShellPrompt(ShellType);
                }

                // Raise shell initialization event
                EventsManager.FireEvent(EventType.ShellInitialized, ShellType);

                // Wait for command
                DebugWriter.WriteDebug(DebugLevel.I, "Waiting for command");
                string strcommand = Input.ReadLine();

                // Add command to command builder and return the final result. The reason to add the extra space before the second command written is that
                // because if we need to provide a second command to the shell in a separate line, we usually add the semicolon at the end of the primary
                // command input.
                if (!string.IsNullOrEmpty(FullCommand))
                    commandBuilder.Append(" ");
                commandBuilder.Append(strcommand);
                FullCommand = commandBuilder.ToString();
            }

            // Check for a type of command
            var SplitCommands = FullCommand.Split(new[] { " ; " }, StringSplitOptions.RemoveEmptyEntries);
            var Commands = CommandManager.GetCommands(ShellType);
            for (int i = 0; i < SplitCommands.Length; i++)
            {
                string Command = SplitCommands[i];

                // Fire an event of PreExecuteCommand
                EventsManager.FireEvent(EventType.PreExecuteCommand, ShellType, Command);

                // Check to see if the command is a comment
                if ((string.IsNullOrEmpty(Command) | (Command?.StartsWithAnyOf(new[] { " ", "#" }))) == false)
                {
                    // Get the index of the first space
                    int indexCmd = Command.IndexOf(" ");
                    DebugWriter.WriteDebug(DebugLevel.I, "Prototype indexCmd and Command: {0}, {1}", indexCmd, Command);
                    if (indexCmd == -1)
                        indexCmd = Command.Length;
                    string commandName = Command.Substring(0, indexCmd);
                    DebugWriter.WriteDebug(DebugLevel.I, "Finished indexCmd and finalCommand: {0}, {1}", indexCmd, commandName);

                    // Get arguments
                    var commandArguments = new ProvidedCommandArgumentsInfo(Command, ShellType);

                    // Reads command written by user
                    do
                    {
                        try
                        {
                            // Set title
                            ConsoleExtensions.SetTitle($"{Kernel.Kernel.ConsoleTitle} - {Command}");

                            if (ModManager.ListModCommands(ShellType).ContainsKey(commandName))
                            {
                                // Iterate through mod commands
                                DebugWriter.WriteDebug(DebugLevel.I, "Mod commands probing started with {0} from {1}", commandName, Command);
                                ModExecutor.ExecuteModCommand(Command);
                            }
                            else if (AliasManager.GetAliasesListFromType(ShellType).ContainsKey(commandName))
                            {
                                // Iterate through alias commands
                                DebugWriter.WriteDebug(DebugLevel.I, "Aliases probing started with {0} from {1}", Command, Command);
                                AliasExecutor.ExecuteAlias(Command, ShellType);
                            }
                            else if (Commands.ContainsKey(commandName))
                            {
                                // Execute the built-in command
                                DebugWriter.WriteDebug(DebugLevel.I, "Executing built-in command");

                                // Check to see if the command supports redirection
                                if (Commands[commandName].Flags.HasFlag(CommandFlags.RedirectionSupported))
                                {
                                    DebugWriter.WriteDebug(DebugLevel.I, "Redirection supported!");
                                    Command = InitializeRedirection(Command);
                                }

                                // Check to see if the optional path is specified
                                if (!string.IsNullOrEmpty(OutputPath))
                                {
                                    DebugWriter.WriteDebug(DebugLevel.I, "Output path provided!");
                                    InitializeOutputPathWriter(OutputPath);
                                }

                                if (!(string.IsNullOrEmpty(commandName) | commandName.StartsWithAnyOf(new[] { " ", "#" }) == true))
                                {

                                    // Check to see if a user is able to execute a command
                                    if (ShellType == "Shell")
                                    {
                                        if (Commands[commandName].Flags.HasFlag(CommandFlags.Strict))
                                        {
                                            if (!PermissionsTools.IsPermissionGranted(PermissionTypes.RunStrictCommands) && 
                                                !(bool)GetUserProperty(Login.CurrentUser.Username, UserProperty.Admin))
                                            {
                                                DebugWriter.WriteDebug(DebugLevel.W, "Cmd exec {0} failed: adminList(signedinusrnm) is False, strictCmds.Contains({0}) is True", commandName);
                                                TextWriterColor.Write(Translate.DoTranslation("You don't have permission to use {0}"), true, KernelColorType.Error, commandName);
                                                break;
                                            }
                                        }
                                    }

                                    // Check the command before starting
                                    if (Flags.Maintenance == true & Commands[commandName].Flags.HasFlag(CommandFlags.NoMaintenance))
                                    {
                                        DebugWriter.WriteDebug(DebugLevel.W, "Cmd exec {0} failed: In maintenance mode. {0} is in NoMaintenanceCmds", commandName);
                                        TextWriterColor.Write(Translate.DoTranslation("Shell message: The requested command {0} is not allowed to run in maintenance mode."), true, KernelColorType.Error, commandName);
                                    }
                                    else
                                    {
                                        DebugWriter.WriteDebug(DebugLevel.I, "Cmd exec {0} succeeded. Running with {1}", commandName, Command);
                                        var Params = new CommandExecutor.ExecuteCommandParameters(Command, ShellType);
                                        CommandExecutor.StartCommandThread(Params);
                                    }
                                }
                            }
                            else if (Parsing.TryParsePath(TargetFile) & ShellType == "Shell")
                            {
                                // Scan PATH for file existence and set file name as needed
                                PathLookupTools.FileExistsInPath(commandName, ref TargetFile);
                                if (string.IsNullOrEmpty(TargetFile))
                                    TargetFile = Filesystem.NeutralizePath(commandName);
                                if (Parsing.TryParsePath(TargetFile))
                                    TargetFileName = Path.GetFileName(TargetFile);

                                // If we're in the UESH shell, parse the script file or executable file
                                if (Checking.FileExists(TargetFile) & !TargetFile.EndsWith(".uesh"))
                                {
                                    DebugWriter.WriteDebug(DebugLevel.I, "Cmd exec {0} succeeded because file is found.", commandName);
                                    try
                                    {
                                        // Create a new instance of process
                                        if (Parsing.TryParsePath(TargetFile))
                                        {
                                            var targetCommand = Command.Replace(TargetFileName, "");
                                            targetCommand.RemoveNullsOrWhitespacesAtTheBeginning();
                                            DebugWriter.WriteDebug(DebugLevel.I, "Command: {0}, Arguments: {1}", TargetFile, targetCommand);
                                            var Params = new ProcessExecutor.ExecuteProcessThreadParameters(TargetFile, targetCommand);
                                            ProcessStartCommandThread.Start(Params);
                                            ProcessStartCommandThread.Wait();
                                            ProcessStartCommandThread.Stop();
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        DebugWriter.WriteDebug(DebugLevel.E, "Failed to start process: {0}", ex.Message);
                                        TextWriterColor.Write(Translate.DoTranslation("Failed to start \"{0}\": {1}"), true, KernelColorType.Error, commandName, ex.Message);
                                        DebugWriter.WriteDebugStackTrace(ex);
                                    }
                                }
                                else if (Checking.FileExists(TargetFile) & TargetFile.EndsWith(".uesh"))
                                {
                                    try
                                    {
                                        DebugWriter.WriteDebug(DebugLevel.I, "Cmd exec {0} succeeded because it's a UESH script.", commandName);
                                        UESHParse.Execute(TargetFile, commandArguments.ArgumentsText);
                                    }
                                    catch (Exception ex)
                                    {
                                        TextWriterColor.Write(Translate.DoTranslation("Error trying to execute script: {0}"), true, KernelColorType.Error, ex.Message);
                                        DebugWriter.WriteDebugStackTrace(ex);
                                    }
                                }
                                else
                                {
                                    DebugWriter.WriteDebug(DebugLevel.W, "Cmd exec {0} failed: availableCmds.Cont({0}.Substring(0, {1})) = False", commandName, indexCmd);
                                    TextWriterColor.Write(Translate.DoTranslation("Shell message: The requested command {0} is not found. See 'help' for available commands."), true, KernelColorType.Error, commandName);
                                }
                            }
                            else
                            {
                                DebugWriter.WriteDebug(DebugLevel.W, "Cmd exec {0} failed: availableCmds.Cont({0}.Substring(0, {1})) = False", commandName, indexCmd);
                                TextWriterColor.Write(Translate.DoTranslation("Shell message: The requested command {0} is not found. See 'help' for available commands."), true, KernelColorType.Error, commandName);
                            }
                        }
                        catch (Exception ex)
                        {
                            DebugWriter.WriteDebugStackTrace(ex);
                            TextWriterColor.Write(Translate.DoTranslation("Error trying to execute command.") + CharManager.NewLine + Translate.DoTranslation("Error {0}: {1}"), true, KernelColorType.Error, ex.GetType().FullName, ex.Message);
                        }
                    }
                    while (false);
                }

                // Fire an event of PostExecuteCommand
                EventsManager.FireEvent(EventType.PostExecuteCommand, ShellType, Command);
            }

            // Restore console output to its original state if any
            if (DriverHandler.CurrentConsoleDriver.DriverName != "Default")
            {
                if (DriverHandler.CurrentConsoleDriver is File writer)
                    writer.FilterVT = false;
                DriverHandler.SetDriver<IConsoleDriver>("Default");
            }

            // Restore title
            ConsoleExtensions.SetTitle(Kernel.Kernel.ConsoleTitle);
        }

        /// <summary>
        /// Gets the shell type name
        /// </summary>
        /// <param name="shellType">Shell type enumeration</param>
        public static string GetShellTypeName(ShellType shellType) => shellType.ToString();

        /// <summary>
        /// Gets the shell information instance
        /// </summary>
        /// <param name="shellType">Shell type from enum</param>
        public static BaseShellInfo GetShellInfo(ShellType shellType) => GetShellInfo(GetShellTypeName(shellType));

        /// <summary>
        /// Gets the shell information instance
        /// </summary>
        /// <param name="shellType">Shell type name</param>
        public static BaseShellInfo GetShellInfo(string shellType) => AvailableShells.ContainsKey(shellType) ? AvailableShells[shellType] : AvailableShells["Shell"];

        /// <summary>
        /// Initializes the redirection
        /// </summary>
        private static string InitializeRedirection(string Command)
        {
            // If requested command has output redirection sign after arguments, remove it from final command string and set output to that file
            if (Command.Contains(" >> "))
            {
                bool isOverwrite = !Command.Contains(" >>> ");
                string redirectSyntax = isOverwrite ? " >> " : " >>> ";
                DebugWriter.WriteDebug(DebugLevel.I, "Output redirection found with overwrite mode [{0}].", isOverwrite);
                string OutputFileName = Command.Substring(Command.LastIndexOf(">") + 2);
                string OutputFilePath = Filesystem.NeutralizePath(OutputFileName);
                DriverHandler.SetDriver<IConsoleDriver>("File");
                ((File)DriverHandler.CurrentConsoleDriver).PathToWrite = OutputFilePath;
                ((File)DriverHandler.CurrentConsoleDriver).FilterVT = true;
                if (isOverwrite)
                {
                    FileStream clearer = new(OutputFilePath, FileMode.OpenOrCreate, FileAccess.ReadWrite);
                    clearer.SetLength(0);
                    clearer.Close();
                }
                Command = Command.Replace(redirectSyntax + OutputFileName, "");
            }
            else if (Command.EndsWith(" |SILENT|"))
            {
                DebugWriter.WriteDebug(DebugLevel.I, "Silence found. Redirecting to null writer...");
                DriverHandler.SetDriver<IConsoleDriver>("Null");
                Command = Command.Replace(" |SILENT|", "");
            }

            return Command;
        }

        /// <summary>
        /// Initializes the optional file path writer
        /// </summary>
        private static void InitializeOutputPathWriter(string OutputPath)
        {
            // Checks to see if the user provided optional path
            if (!string.IsNullOrWhiteSpace(OutputPath))
            {
                DebugWriter.WriteDebug(DebugLevel.I, "Optional output redirection found using OutputPath ({0}).", OutputPath);
                OutputPath = Filesystem.NeutralizePath(OutputPath);
                DriverHandler.SetDriver<IConsoleDriver>("File");
                ((File)DriverHandler.CurrentConsoleDriver).PathToWrite = OutputPath;
            }
        }

    }
}
