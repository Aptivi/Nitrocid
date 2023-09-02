
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
using KS.ConsoleBase;
using KS.ConsoleBase.Colors;
using KS.ConsoleBase.Inputs;
using KS.Files;
using KS.Files.PathLookup;
using KS.Files.Querying;
using KS.Kernel;
using KS.Kernel.Debugging;
using KS.Languages;
using KS.Misc.Text;
using KS.Drivers;
using KS.Shell.Prompts;
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
using KS.Kernel.Events;
using File = KS.Drivers.Console.Consoles.File;
using KS.Users.Permissions;
using KS.Drivers.Console;
using Manipulation = KS.Files.Operations.Manipulation;
using KS.Misc.Probers.Regexp;
using System.Text.RegularExpressions;
using System.Linq;
using KS.Drivers.Console.Consoles;
using FluentFTP.Helpers;
using KS.Kernel.Configuration;
using KS.Shell.Shells.Sql;
using KS.Users;
using KS.Shell.Shells.Debug;
using KS.Kernel.Exceptions;
using KS.Shell.ShellBase.Commands.Execution;
using KS.Kernel.Threading;
using KS.ConsoleBase.Writers.ConsoleWriters;
using KS.Shell.ShellBase.Scripting;
using Terminaux.Reader;
using KS.Misc.Screensaver;

namespace KS.Shell
{
    /// <summary>
    /// Base shell module
    /// </summary>
    public static class ShellManager
    {

        internal static KernelThread ProcessStartCommandThread = new("Executable Command Thread", false, (processParams) => ProcessExecutor.ExecuteProcess((ProcessExecutor.ExecuteProcessThreadParameters)processParams)) { isCritical = true };

        /// <summary>
        /// List of unified commands
        /// </summary>
        public readonly static Dictionary<string, CommandInfo> UnifiedCommandDict = new()
        {
            { "presets",
                new CommandInfo("presets", ShellType.Shell, /* Localizable */ "Opens the shell preset library",
                    new[] {
                        new CommandArgumentInfo()
                    }, new PresetsUnifiedCommand()) 
            },

            { "exec",
                new CommandInfo("exec", ShellType.Shell, /* Localizable */ "Executes an external process",
                    new[] {
                        new CommandArgumentInfo(new[] { "process", "args" }, Array.Empty<SwitchInfo>(), true, 1)
                    }, new ExecUnifiedCommand())
            },

            { "exit",
                new CommandInfo("exit", ShellType.Shell, /* Localizable */ "Exits the shell if running on subshell",
                    new[] {
                        new CommandArgumentInfo()
                    }, new ExitUnifiedCommand())
            },

            { "help",
                new CommandInfo("help", ShellType.Shell, /* Localizable */ "Help page",
                    new[] {
                        new CommandArgumentInfo(new[] { "command" }, Array.Empty<SwitchInfo>(), false, 0, false, (_, _, _) => HelpUnifiedCommand.ListCmds())
                    }, new HelpUnifiedCommand(), CommandFlags.Wrappable)
            },

            { "wrap",
                new CommandInfo("wrap", ShellType.Shell, /* Localizable */ "Wraps the console output",
                    new[] {
                        new CommandArgumentInfo(new[] { "command" }, Array.Empty<SwitchInfo>(), true, 1)
                    }, new WrapUnifiedCommand())
            }
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
            { "AdminShell", new AdminShellInfo() },
            { "SqlShell", new SqlShellInfo() },
            { "DebugShell", new DebugShellInfo() }
        };

        /// <summary>
        /// Current shell type
        /// </summary>
        public static string CurrentShellType =>
            ShellStart.ShellStack[^1].ShellType;

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
                    DebugWriter.WriteDebug(DebugLevel.W, "Trying to call LastShellType on empty shell stack. Assuming UESH...");
                    return "Shell";
                }
                else if (ShellStart.ShellStack.Count == 1)
                {
                    // We only have one shell. Consider current as last.
                    DebugWriter.WriteDebug(DebugLevel.W, "Trying to call LastShellType on shell stack containing only one shell. Assuming curent...");
                    return CurrentShellType;
                }
                else
                {
                    // We have more than one shell. Return the shell type for a shell before the last one.
                    var type = ShellStart.ShellStack[^2].ShellType;
                    DebugWriter.WriteDebug(DebugLevel.I, "Returning shell type {0} for last shell from the stack...", type);
                    return type;
                }
            }
        }

        /// <summary>
        /// Inputs for command then parses a specified command.
        /// </summary>
        /// <remarks>All new shells implemented either in KS or by mods should use this routine to allow effective and consistent line parsing.</remarks>
        public static void GetLine() =>
            GetLine("", "", CurrentShellType);

        /// <summary>
        /// Parses a specified command.
        /// </summary>
        /// <param name="FullCommand">The full command string</param>
        /// <remarks>All new shells implemented either in KS or by mods should use this routine to allow effective and consistent line parsing.</remarks>
        public static void GetLine(string FullCommand) =>
            GetLine(FullCommand, "", CurrentShellType);

        /// <summary>
        /// Parses a specified command.
        /// </summary>
        /// <param name="FullCommand">The full command string</param>
        /// <param name="OutputPath">Optional (non-)neutralized output path</param>
        /// <remarks>All new shells implemented either in KS or by mods should use this routine to allow effective and consistent line parsing.</remarks>
        public static void GetLine(string FullCommand, string OutputPath = "") =>
            GetLine(FullCommand, OutputPath, CurrentShellType);

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
        /// <param name="restoreDriver">Whether to restore the driver to the previous state</param>
        /// <remarks>All new shells implemented either in KS or by mods should use this routine to allow effective and consistent line parsing.</remarks>
        public static void GetLine(string FullCommand, string OutputPath = "", string ShellType = "Shell", bool restoreDriver = true)
        {
            // Check for sanity
            if (string.IsNullOrEmpty(FullCommand))
                FullCommand = "";

            // Variables
            string TargetFile = "";
            string TargetFileName = "";

            // Now, initialize the command autocomplete handler. This will not be invoked if we have auto completion disabled.
            var settings = new TermReaderSettings()
            {
                Suggestions = CommandAutoComplete.GetSuggestions,
                SuggestionsDelimiters = new[] { ' ' },
                TreatCtrlCAsInput = true,
            };

            // Check to see if the full command string ends with the semicolon
            while (FullCommand.EndsWith(";") || string.IsNullOrEmpty(FullCommand))
            {
                // Enable cursor
                ConsoleWrapper.CursorVisible = true;

                // Tell the user to provide the command
                StringBuilder commandBuilder = new(FullCommand);

                // If we are on the shell suppress lock mode, we need to read a key to ensure that ENTER or any key that causes strcommand to return
                // doesn't cause the shell prompt to be written twice. For example, when getting out of the lock screen by pressing ENTER when lockscreen
                // is invoked, we need to make sure that we don't write the shell prompt twice.
                if (ScreensaverManager.ShellSuppressLockMode)
                {
                    ScreensaverManager.ShellSuppressLockMode = false;
                    if (ConsoleWrapper.KeyAvailable)
                        ConsoleWrapper.ReadKey(true);
                    continue;
                }

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
                string strcommand = Input.ReadLine("", "", settings);
                DebugWriter.WriteDebug(DebugLevel.I, "Waited for command [{0}]", strcommand);

                // Add command to command builder and return the final result. The reason to add the extra space before the second command written is that
                // because if we need to provide a second command to the shell in a separate line, we usually add the semicolon at the end of the primary
                // command input.
                if (!string.IsNullOrEmpty(FullCommand) && !string.IsNullOrEmpty(strcommand))
                    commandBuilder.Append(' ');

                // There are cases when strcommand may be empty, so ignore that if it's empty.
                if (!string.IsNullOrEmpty(strcommand))
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
                    // Get the command name
                    var words = Command.SplitEncloseDoubleQuotes();
                    string commandName = words[0].ReleaseDoubleQuotes();
                    string arguments = string.Join(' ', words.Skip(1));
                    TargetFile = DriverHandler.CurrentRegexpDriverLocal.Unescape(commandName);
                    bool existsInPath = PathLookupTools.FileExistsInPath(commandName, ref TargetFile);
                    bool pathValid = Parsing.TryParsePath(TargetFile);
                    if (!existsInPath || string.IsNullOrEmpty(TargetFile))
                        TargetFile = Filesystem.NeutralizePath(commandName);
                    if (pathValid)
                        TargetFileName = Path.GetFileName(TargetFile);
                    DebugWriter.WriteDebug(DebugLevel.I, "Finished finalCommand: {0}", commandName);
                    DebugWriter.WriteDebug(DebugLevel.I, "Finished TargetFile: {0}", TargetFile);

                    // Reads command written by user
                    try
                    {
                        // Set title
                        if (Config.MainConfig.SetTitleOnCommandExecution)
                            ConsoleExtensions.SetTitle($"{KernelTools.ConsoleTitle} - {Command}");

                        if (Commands.ContainsKey(commandName))
                        {
                            // Execute the command
                            DebugWriter.WriteDebug(DebugLevel.I, "Executing command");

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
                                            !UserManagement.CurrentUser.Admin)
                                        {
                                            DebugWriter.WriteDebug(DebugLevel.W, "Cmd exec {0} failed: adminList(signedinusrnm) is False, strictCmds.Contains({0}) is True", commandName);
                                            TextWriterColor.Write(Translate.DoTranslation("You don't have permission to use {0}"), true, KernelColorType.Error, commandName);
                                            UESHVariables.SetVariable("UESHErrorCode", "-4");
                                            break;
                                        }
                                    }
                                }

                                // Check the command before starting
                                if (Flags.Maintenance == true & Commands[commandName].Flags.HasFlag(CommandFlags.NoMaintenance))
                                {
                                    DebugWriter.WriteDebug(DebugLevel.W, "Cmd exec {0} failed: In maintenance mode. {0} is in NoMaintenanceCmds", commandName);
                                    TextWriterColor.Write(Translate.DoTranslation("Shell message: The requested command {0} is not allowed to run in maintenance mode."), true, KernelColorType.Error, commandName);
                                    UESHVariables.SetVariable("UESHErrorCode", "-3");
                                }
                                else
                                {
                                    var ShellInstance = ShellStart.ShellStack[^1];
                                    CancellationHandlers.canCancel = true;
                                    DebugWriter.WriteDebug(DebugLevel.I, "Cmd exec {0} succeeded. Running with {1}", commandName, Command);
                                    var Params = new CommandExecutor.ExecuteCommandParameters(Command, ShellType, ShellInstance);
                                    CommandExecutor.StartCommandThread(Params);
                                    UESHVariables.SetVariable("UESHErrorCode", $"{ShellInstance.LastErrorCode}");
                                }
                            }
                        }
                        else if (pathValid & ShellType == "Shell")
                        {
                            // If we're in the UESH shell, parse the script file or executable file
                            if (Checking.FileExists(TargetFile) & !TargetFile.EndsWith(".uesh"))
                            {
                                DebugWriter.WriteDebug(DebugLevel.I, "Cmd exec {0} succeeded because file is found.", commandName);
                                try
                                {
                                    // Create a new instance of process
                                    PermissionsTools.Demand(PermissionTypes.ExecuteProcesses);
                                    if (pathValid)
                                    {
                                        CancellationHandlers.canCancel = true;
                                        var targetCommand = Command.Replace(TargetFileName, "");
                                        targetCommand = targetCommand.TrimStart('\0', ' ');
                                        DebugWriter.WriteDebug(DebugLevel.I, "Command: {0}, Arguments: {1}", TargetFile, targetCommand);
                                        var Params = new ProcessExecutor.ExecuteProcessThreadParameters(TargetFile, targetCommand);
                                        ProcessStartCommandThread.Start(Params);
                                        ProcessStartCommandThread.Wait();
                                        ProcessStartCommandThread.Stop();
                                        UESHVariables.SetVariable("UESHErrorCode", "0");
                                    }
                                }
                                catch (Exception ex)
                                {
                                    DebugWriter.WriteDebug(DebugLevel.E, "Failed to start process: {0}", ex.Message);
                                    TextWriterColor.Write(Translate.DoTranslation("Failed to start \"{0}\": {1}"), true, KernelColorType.Error, commandName, ex.Message);
                                    DebugWriter.WriteDebugStackTrace(ex);
                                    if (ex is KernelException kex)
                                        UESHVariables.SetVariable("UESHErrorCode", $"{10000 + (int)kex.ExceptionType}");
                                    else
                                        UESHVariables.SetVariable("UESHErrorCode", $"{ex.GetHashCode()}");
                                }
                            }
                            else if (Checking.FileExists(TargetFile) & TargetFile.EndsWith(".uesh"))
                            {
                                try
                                {
                                    CancellationHandlers.canCancel = true;
                                    PermissionsTools.Demand(PermissionTypes.ExecuteScripts);
                                    DebugWriter.WriteDebug(DebugLevel.I, "Cmd exec {0} succeeded because it's a UESH script.", commandName);
                                    UESHParse.Execute(TargetFile, arguments);
                                    UESHVariables.SetVariable("UESHErrorCode", "0");
                                }
                                catch (Exception ex)
                                {
                                    TextWriterColor.Write(Translate.DoTranslation("Error trying to execute script: {0}"), true, KernelColorType.Error, ex.Message);
                                    DebugWriter.WriteDebugStackTrace(ex);
                                    if (ex is KernelException kex)
                                        UESHVariables.SetVariable("UESHErrorCode", $"{10000 + (int)kex.ExceptionType}");
                                    else
                                        UESHVariables.SetVariable("UESHErrorCode", $"{ex.GetHashCode()}");
                                }
                            }
                            else
                            {
                                DebugWriter.WriteDebug(DebugLevel.W, "Cmd exec {0} failed: command {0} not found parsing target file", commandName);
                                TextWriterColor.Write(Translate.DoTranslation("Shell message: The requested command {0} is not found. See 'help' for available commands."), true, KernelColorType.Error, commandName);
                                UESHVariables.SetVariable("UESHErrorCode", "-2");
                            }
                        }
                        else
                        {
                            DebugWriter.WriteDebug(DebugLevel.W, "Cmd exec {0} failed: command {0} not found", commandName);
                            TextWriterColor.Write(Translate.DoTranslation("Shell message: The requested command {0} is not found. See 'help' for available commands."), true, KernelColorType.Error, commandName);
                            UESHVariables.SetVariable("UESHErrorCode", "-1");
                        }
                    }
                    catch (Exception ex)
                    {
                        DebugWriter.WriteDebugStackTrace(ex);
                        TextWriterColor.Write(Translate.DoTranslation("Error trying to execute command.") + CharManager.NewLine + Translate.DoTranslation("Error {0}: {1}"), true, KernelColorType.Error, ex.GetType().FullName, ex.Message);
                        if (ex is KernelException kex)
                            UESHVariables.SetVariable("UESHErrorCode", $"{10000 + (int)kex.ExceptionType}");
                        else
                            UESHVariables.SetVariable("UESHErrorCode", $"{ex.GetHashCode()}");
                    }
                }

                // Fire an event of PostExecuteCommand
                EventsManager.FireEvent(EventType.PostExecuteCommand, ShellType, Command);
            }

            // Restore console output to its original state if any
            if (DriverHandler.CurrentConsoleDriverLocal.DriverName != "Default" && restoreDriver)
            {
                if (DriverHandler.CurrentConsoleDriverLocal is File writer)
                    writer.FilterVT = false;
                DriverHandler.EndLocalDriver<IConsoleDriver>();
            }

            // Restore title and cancel possibility state
            ConsoleExtensions.SetTitle(KernelTools.ConsoleTitle);
            CancellationHandlers.canCancel = false;
        }

        /// <summary>
        /// Gets the shell type name
        /// </summary>
        /// <param name="shellType">Shell type enumeration</param>
        public static string GetShellTypeName(ShellType shellType) =>
            shellType.ToString();

        /// <summary>
        /// Gets the shell information instance
        /// </summary>
        /// <param name="shellType">Shell type from enum</param>
        public static BaseShellInfo GetShellInfo(ShellType shellType) =>
            GetShellInfo(GetShellTypeName(shellType));

        /// <summary>
        /// Gets the shell information instance
        /// </summary>
        /// <param name="shellType">Shell type name</param>
        public static BaseShellInfo GetShellInfo(string shellType) =>
            AvailableShells.ContainsKey(shellType) ? AvailableShells[shellType] : AvailableShells["Shell"];

        /// <summary>
        /// Initializes the redirection
        /// </summary>
        private static string InitializeRedirection(string Command)
        {
            // If requested command has output redirection sign after arguments, remove it from final command string and set output to that file
            string RedirectionPattern = /*lang=regex*/ @"( (>>|>>>) .+?)+$";
            if (RegexpTools.IsMatch(Command, RedirectionPattern))
            {
                var outputMatch = Regex.Match(Command, RedirectionPattern);
                var outputFiles = outputMatch.Groups[1].Captures.Select((cap) => cap.Value);
                List<string> filePaths = new();
                foreach (var outputFile in outputFiles)
                {
                    bool isOverwrite = !outputFile.StartsWith(" >>> ");
                    string OutputFileName = outputFile[(outputFile.LastIndexOf(">") + 2)..];
                    string OutputFilePath = Filesystem.NeutralizePath(OutputFileName);
                    DebugWriter.WriteDebug(DebugLevel.I, "Output redirection found for file {1} with overwrite mode [{0}].", isOverwrite, OutputFilePath);
                    if (isOverwrite)
                        Manipulation.ClearFile(OutputFilePath);
                    filePaths.Add(OutputFilePath);
                }
                DriverHandler.BeginLocalDriver<IConsoleDriver>("FileSequence");
                ((FileSequence)DriverHandler.CurrentConsoleDriverLocal).PathsToWrite = filePaths.ToArray();
                ((FileSequence)DriverHandler.CurrentConsoleDriverLocal).FilterVT = true;
                Command = Command.RemovePostfix(outputMatch.Value);
            }
            else if (Command.EndsWith(" |SILENT|"))
            {
                DebugWriter.WriteDebug(DebugLevel.I, "Silence found. Redirecting to null writer...");
                DriverHandler.BeginLocalDriver<IConsoleDriver>("Null");
                Command = Command.RemovePostfix(" |SILENT|");
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
                DriverHandler.BeginLocalDriver<IConsoleDriver>("File");
                ((File)DriverHandler.CurrentConsoleDriverLocal).PathToWrite = OutputPath;
            }
        }

    }
}
