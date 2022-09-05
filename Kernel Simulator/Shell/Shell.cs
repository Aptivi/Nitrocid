
// Kernel Simulator  Copyright (C) 2018-2022  Aptivi
// 
// This file is part of Kernel Simulator
// 
// Kernel Simulator is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// Kernel Simulator is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Extensification.StringExts;
using FluentFTP.Helpers;
using KS.ConsoleBase;
using KS.ConsoleBase.Colors;
using KS.Files;
using KS.Files.PathLookup;
using KS.Files.Querying;
using KS.Kernel;
using KS.Kernel.Debugging;
using KS.Languages;
using KS.Misc.Execution;
using KS.Misc.Threading;
using KS.Misc.Writers.ConsoleWriters;
using KS.Misc.Writers.WriterBase;
using KS.Misc.Writers.WriterBase.PlainWriters;
using KS.Modifications;
using KS.Scripting;
using KS.Shell.ShellBase.Aliases;
using KS.Shell.ShellBase.Commands;
using KS.Shell.ShellBase.Shells;
using KS.Shell.UnifiedCommands;
using KS.Users.Groups;

namespace KS.Shell
{
    public static class Shell
    {

        internal static StreamWriter OutputTextWriter;
        internal static FileStream OutputStream;
        internal static KernelThread ProcessStartCommandThread = new("Executable Command Thread", false, (processParams) => ProcessExecutor.ExecuteProcess((ProcessExecutor.ExecuteProcessThreadParameters)processParams));

        /// <summary>
        /// Whether the shell is colored or not
        /// </summary>
        public static bool ColoredShell = true;
        /// <summary>
        /// Specifies where to lookup for executables in these paths. Same as in PATH implementation.
        /// </summary>
        public static string PathsToLookup = Environment.GetEnvironmentVariable("PATH");
        /// <summary>
        /// Path lookup delimiter, depending on the operating system
        /// </summary>
        public readonly static string PathLookupDelimiter = Convert.ToString(Path.PathSeparator);

        /// <summary>
        /// List of unified commands
        /// </summary>
        public readonly static Dictionary<string, CommandInfo> UnifiedCommandDict = new()
        {
            { "presets", new CommandInfo("presets", ShellType.Shell, "Opens the shell preset library", new CommandArgumentInfo(), new PresetsUnifiedCommand()) },
            { "exit", new CommandInfo("exit", ShellType.Shell, "Exits the shell if running on subshell", new CommandArgumentInfo(), new ExitUnifiedCommand()) },
            { "help", new CommandInfo("help", ShellType.Shell, "Help page", new CommandArgumentInfo(new[] { "[command]" }, false, 0, HelpUnifiedCommand.ListCmds), new HelpUnifiedCommand()) }
        };

        /// <summary>
        /// Current shell type
        /// </summary>
        public static ShellType CurrentShellType
        {
            get
            {
                return ShellStart.ShellStack[ShellStart.ShellStack.Count - 1].ShellType;
            }
        }

        /// <summary>
        /// Last shell type
        /// </summary>
        public static ShellType LastShellType
        {
            get
            {
                if (ShellStart.ShellStack.Count == 0)
                {
                    // We don't have any shell. Return Shell.
                    return ShellType.Shell;
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
        /// Parses a specified command.
        /// </summary>
        /// <param name="FullCommand">The full command string</param>
        /// <remarks>All new shells implemented either in KS or by mods should use this routine to allow effective and consistent line parsing.</remarks>
        public static void GetLine(string FullCommand)
        {
            GetLine(FullCommand, "", CurrentShellType);
        }

        /// <summary>
        /// Parses a specified command.
        /// </summary>
        /// <param name="FullCommand">The full command string</param>
        /// <param name="OutputPath">Optional (non-)neutralized output path</param>
        /// <param name="ShellType">Shell type</param>
        /// <remarks>All new shells implemented either in KS or by mods should use this routine to allow effective and consistent line parsing.</remarks>
        public static void GetLine(string FullCommand, string OutputPath = "", ShellType ShellType = ShellType.Shell)
        {
            // Check for sanity
            if (string.IsNullOrEmpty(FullCommand))
                FullCommand = "";

            // Variables
            string TargetFile = "";
            string TargetFileName = "";

            // Check for a type of command
            var SplitCommands = FullCommand.Split(new[] { " : " }, StringSplitOptions.RemoveEmptyEntries);
            var Commands = GetCommand.GetCommands(ShellType);
            foreach (string Command in SplitCommands)
            {
                // Check to see if the command is a comment
                if ((string.IsNullOrEmpty(Command) | (Command?.StartsWithAnyOf(new[] { " ", "#" }))) == false)
                {
                    // Get the index of the first space
                    int indexCmd = Command.IndexOf(" ");
                    string cmdArgs = Command; // Command with args
                    DebugWriter.WriteDebug(DebugLevel.I, "Prototype indexCmd and Command: {0}, {1}", indexCmd, Command);
                    if (indexCmd == -1)
                        indexCmd = Command.Length;
                    string commandName = Command.Substring(0, indexCmd);
                    DebugWriter.WriteDebug(DebugLevel.I, "Finished indexCmd and finalCommand: {0}, {1}", indexCmd, commandName);

                    // Parse script command (if any)
                    var scriptArgs = commandName.Split(new[] { ".uesh " }, StringSplitOptions.RemoveEmptyEntries).ToList();
                    scriptArgs.RemoveAt(0);

                    // Get command parts
                    var Parts = commandName.SplitSpacesEncloseDoubleQuotes();

                    // Reads command written by user
                    do
                    {
                        try
                        {
                            // Set title
                            ConsoleExtensions.SetTitle($"{Kernel.Kernel.ConsoleTitle} - {Command}");

                            // Iterate through mod commands
                            DebugWriter.WriteDebug(DebugLevel.I, "Mod commands probing started with {0} from {1}", commandName, FullCommand);
                            if (ModManager.ListModCommands(ShellType).ContainsKey(Parts[0]))
                            {
                                DebugWriter.WriteDebug(DebugLevel.I, "Mod command: {0}", Parts[0]);
                                ModExecutor.ExecuteModCommand(commandName);
                            }

                            // Iterate through alias commands
                            DebugWriter.WriteDebug(DebugLevel.I, "Aliases probing started with {0} from {1}", commandName, FullCommand);
                            if (AliasManager.GetAliasesListFromType(ShellType).ContainsKey(Parts[0]))
                            {
                                DebugWriter.WriteDebug(DebugLevel.I, "Alias: {0}", Parts[0]);
                                AliasExecutor.ExecuteAlias(commandName, ShellType);
                            }

                            // Execute the built-in command
                            if (Commands.ContainsKey(commandName))
                            {
                                DebugWriter.WriteDebug(DebugLevel.I, "Executing built-in command");

                                // Check to see if the command supports redirection
                                if (Commands[commandName].Flags.HasFlag(CommandFlags.RedirectionSupported))
                                {
                                    DebugWriter.WriteDebug(DebugLevel.I, "Redirection supported!");
                                    InitializeRedirection(ref FullCommand);
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
                                    if (ShellType == ShellType.Shell)
                                    {
                                        if (GroupManagement.HasGroup(Login.Login.CurrentUser.Username, GroupManagement.GroupType.Administrator) == false & Commands[commandName].Flags.HasFlag(CommandFlags.Strict))
                                        {
                                            DebugWriter.WriteDebug(DebugLevel.W, "Cmd exec {0} failed: adminList(signedinusrnm) is False, strictCmds.Contains({0}) is True", commandName);
                                            TextWriterColor.Write(Translate.DoTranslation("You don't have permission to use {0}"), true, ColorTools.ColTypes.Error, commandName);
                                            break;
                                        }
                                    }

                                    // Check the command before starting
                                    if (Flags.Maintenance == true & Commands[commandName].Flags.HasFlag(CommandFlags.NoMaintenance))
                                    {
                                        DebugWriter.WriteDebug(DebugLevel.W, "Cmd exec {0} failed: In maintenance mode. {0} is in NoMaintenanceCmds", commandName);
                                        TextWriterColor.Write(Translate.DoTranslation("Shell message: The requested command {0} is not allowed to run in maintenance mode."), true, ColorTools.ColTypes.Error, commandName);
                                    }
                                    else
                                    {
                                        DebugWriter.WriteDebug(DebugLevel.I, "Cmd exec {0} succeeded. Running with {1}", commandName, cmdArgs);
                                        var Params = new GetCommand.ExecuteCommandParameters(FullCommand, ShellType);

                                        // Since we're probably trying to run a command using the alternative command threads, if the main shell command thread
                                        // is running, use that to execute the command. This ensures that commands like "wrap" that also execute commands from the
                                        // shell can do their job.
                                        var ShellInstance = ShellStart.ShellStack[ShellStart.ShellStack.Count - 1];
                                        var StartCommandThread = ShellInstance.ShellCommandThread;
                                        bool CommandThreadValid = true;
                                        if (StartCommandThread.IsAlive)
                                        {
                                            if (ShellInstance.AltCommandThreads.Count > 0)
                                            {
                                                StartCommandThread = ShellInstance.AltCommandThreads[ShellInstance.AltCommandThreads.Count - 1];
                                            }
                                            else
                                            {
                                                DebugWriter.WriteDebug(DebugLevel.W, "Cmd exec {0} failed: Alt command threads are not there.");
                                                CommandThreadValid = false;
                                            }
                                        }
                                        if (CommandThreadValid)
                                        {
                                            StartCommandThread.Start(Params);
                                            StartCommandThread.Wait();
                                            StartCommandThread.Stop();
                                        }
                                    }
                                }
                            }
                            else if (Parsing.TryParsePath(TargetFile) & ShellType == ShellType.Shell)
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
                                            cmdArgs = cmdArgs.Replace(TargetFileName, "");
                                            cmdArgs.RemoveNullsOrWhitespacesAtTheBeginning();
                                            DebugWriter.WriteDebug(DebugLevel.I, "Command: {0}, Arguments: {1}", TargetFile, cmdArgs);
                                            var Params = new ProcessExecutor.ExecuteProcessThreadParameters(TargetFile, cmdArgs);
                                            ProcessStartCommandThread.Start(Params);
                                            ProcessStartCommandThread.Wait();
                                            ProcessStartCommandThread.Stop();
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        DebugWriter.WriteDebug(DebugLevel.E, "Failed to start process: {0}", ex.Message);
                                        TextWriterColor.Write(Translate.DoTranslation("Failed to start \"{0}\": {1}"), true, ColorTools.ColTypes.Error, commandName, ex.Message);
                                        DebugWriter.WriteDebugStackTrace(ex);
                                    }
                                }
                                else if (Checking.FileExists(TargetFile) & TargetFile.EndsWith(".uesh"))
                                {
                                    try
                                    {
                                        DebugWriter.WriteDebug(DebugLevel.I, "Cmd exec {0} succeeded because it's a UESH script.", commandName);
                                        UESHParse.Execute(TargetFile, scriptArgs.Join(" "));
                                    }
                                    catch (Exception ex)
                                    {
                                        TextWriterColor.Write(Translate.DoTranslation("Error trying to execute script: {0}"), true, ColorTools.ColTypes.Error, ex.Message);
                                        DebugWriter.WriteDebugStackTrace(ex);
                                    }
                                }
                                else
                                {
                                    DebugWriter.WriteDebug(DebugLevel.W, "Cmd exec {0} failed: availableCmds.Cont({0}.Substring(0, {1})) = False", commandName, indexCmd);
                                    TextWriterColor.Write(Translate.DoTranslation("Shell message: The requested command {0} is not found. See 'help' for available commands."), true, ColorTools.ColTypes.Error, commandName);
                                }
                            }
                            else
                            {
                                DebugWriter.WriteDebug(DebugLevel.W, "Cmd exec {0} failed: availableCmds.Cont({0}.Substring(0, {1})) = False", commandName, indexCmd);
                                TextWriterColor.Write(Translate.DoTranslation("Shell message: The requested command {0} is not found. See 'help' for available commands."), true, ColorTools.ColTypes.Error, commandName);
                            }
                        }
                        catch (Exception ex)
                        {
                            DebugWriter.WriteDebugStackTrace(ex);
                            TextWriterColor.Write(Translate.DoTranslation("Error trying to execute command.") + Kernel.Kernel.NewLine + Translate.DoTranslation("Error {0}: {1}"), true, ColorTools.ColTypes.Error, ex.GetType().FullName, ex.Message);
                        }
                    }
                    while (false);
                }
            }

            // Restore console output to its original state if any
            if (WriterPlainManager.CurrentPlainName != "Console")
                WriterPlainManager.ChangePlain("Console");

            // Restore title
            ConsoleExtensions.SetTitle(Kernel.Kernel.ConsoleTitle);
        }

        /// <summary>
        /// Initializes the redirection
        /// </summary>
        private static void InitializeRedirection(ref string Command)
        {
            // If requested command has output redirection sign after arguments, remove it from final command string and set output to that file
            DebugWriter.WriteDebug(DebugLevel.I, "Does the command contain the redirection sign \">>>\" or \">>\"? {0} and {1}", Command.Contains(">>>"), Command.Contains(">>"));
            if (Command.Contains(">>>"))
            {
                DebugWriter.WriteDebug(DebugLevel.I, "Output redirection found with append.");
                string OutputFileName = Command.Substring(Command.LastIndexOf(">") + 2);
                OutputFileName = Filesystem.NeutralizePath(OutputFileName);
                WriterPlainManager.ChangePlain("File");
                ((FilePlainWriter)WriterPlainManager.CurrentPlain).PathToWrite = OutputFileName;
                Command = Command.Replace(" >>> " + OutputFileName, "");
            }
            else if (Command.Contains(">>"))
            {
                DebugWriter.WriteDebug(DebugLevel.I, "Output redirection found with overwrite.");
                string OutputFileName = Command.Substring(Command.LastIndexOf(">") + 2);
                OutputFileName = Filesystem.NeutralizePath(OutputFileName);
                WriterPlainManager.ChangePlain("File");
                ((FilePlainWriter)WriterPlainManager.CurrentPlain).PathToWrite = OutputFileName;
                FileStream clearer = new(OutputFileName, FileMode.OpenOrCreate, FileAccess.ReadWrite);
                clearer.SetLength(0);
                clearer.Close();
                Command = Command.Replace(" >> " + OutputFileName, "");
            }
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
                WriterPlainManager.ChangePlain("File");
                ((FilePlainWriter)WriterPlainManager.CurrentPlain).PathToWrite = OutputPath;
            }
        }

    }
}
