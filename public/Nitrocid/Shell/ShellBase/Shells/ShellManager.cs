//
// Nitrocid KS  Copyright (C) 2018-2025  Aptivi
//
// This file is part of Nitrocid KS
//
// Nitrocid KS is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// Nitrocid KS is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY, without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.
//

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using File = Nitrocid.Drivers.Console.Bases.File;
using FileIO = System.IO.File;
using System.Text.RegularExpressions;
using System.Linq;
using Terminaux.Reader;
using System.Collections.ObjectModel;
using Newtonsoft.Json;
using Nitrocid.Kernel;
using Nitrocid.Shell.ShellBase.Scripting;
using Nitrocid.Shell.ShellBase.Commands;
using Nitrocid.Kernel.Configuration;
using Nitrocid.Users;
using Nitrocid.Kernel.Debugging;
using Nitrocid.Shell.ShellBase.Commands.ProcessExecution;
using Nitrocid.Files;
using Nitrocid.Shell.ShellBase.Arguments;
using Nitrocid.Drivers;
using Nitrocid.Kernel.Threading;
using Nitrocid.ConsoleBase.Writers;
using Nitrocid.Languages;
using Nitrocid.Kernel.Exceptions;
using Nitrocid.Drivers.Console;
using Nitrocid.Shell.Prompts;
using Nitrocid.Security.Permissions;
using Nitrocid.Drivers.Console.Bases;
using Nitrocid.Files.Paths;
using Nitrocid.Kernel.Events;
using Nitrocid.ConsoleBase.Colors;
using Nitrocid.Kernel.Power;
using Nitrocid.Misc.Text.Probers.Regexp;
using Nitrocid.Shell.ShellBase.Switches;
using Nitrocid.Shell.ShellBase.Shells.Unified;
using Nitrocid.Shell.Shells.Admin;
using Nitrocid.Shell.Shells.UESH;
using Nitrocid.Shell.Shells.Text;
using Nitrocid.Shell.Shells.Hex;
using Nitrocid.Shell.Shells.Debug;
using Textify.General;
using Terminaux.Base;
using Nitrocid.ConsoleBase.Inputs;
using Terminaux.Base.Extensions;
using Terminaux.Reader.History;

namespace Nitrocid.Shell.ShellBase.Shells
{
    /// <summary>
    /// Base shell module
    /// </summary>
    public static class ShellManager
    {

        internal static List<ShellExecuteInfo> ShellStack = [];
        internal static string lastCommand = "";
        internal static KernelThread ProcessStartCommandThread = new("Executable Command Thread", false, (processParams) => ProcessExecutor.ExecuteProcess((ExecuteProcessThreadParameters?)processParams));
        internal static Dictionary<string, List<string>> histories = new()
        {
            { "General",                    new() },
            { $"{ShellType.AdminShell}",    new() },
            { $"{ShellType.DebugShell}",    new() },
            { $"{ShellType.HexShell}",      new() },
            { $"{ShellType.Shell}",         new() },
            { $"{ShellType.TextShell}",     new() },
        };

        internal readonly static List<CommandInfo> unifiedCommandDict =
        [
            new CommandInfo("exec", /* Localizable */ "NKS_SHELL_SHELLS_COMMAND_EXEC_DESC",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "process", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "NKS_SHELL_SHELLS_COMMAND_EXEC_ARGUMENT_PATH_DESC"
                        }),
                        new CommandArgumentPart(false, "args", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "NKS_SHELL_SHELLS_COMMAND_EXEC_ARGUMENT_ARGS_DESC"
                        })
                    ],
                    [
                        new SwitchInfo("forked", /* Localizable */ "NKS_SHELL_SHELLS_COMMAND_EXEC_SWITCH_FORKED_DESC", new SwitchOptions()
                        {
                            AcceptsValues = false
                        })
                    ])
                ], new ExecUnifiedCommand()),

            new CommandInfo("exit", /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_EXIT_DESC", new ExitUnifiedCommand()),

            new CommandInfo("findcmds", /* Localizable */ "NKS_SHELL_SHELLS_COMMAND_FINDCMDS_DESC",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "search", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = "Search phrase"
                        })
                    ], false)
                ], new FindCmdsUnifiedCommand()),

            new CommandInfo("help", /* Localizable */ "NKS_SHELL_SHELLS_COMMAND_HELP_DESC",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(false, "command", new CommandArgumentPartOptions()
                        {
                            AutoCompleter = (_) => CommandManager.GetCommandNames(CurrentShellType),
                            ArgumentDescription = /* Localizable */ "NKS_SHELL_SHELLS_COMMAND_HELP_ARGUMENT_COMMAND_DESC"
                        })
                    ],
                    [
                        new SwitchInfo("general", /* Localizable */ "NKS_SHELL_SHELLS_COMMAND_HELP_SWITCH_GENERAL_DESC", new SwitchOptions()
                        {
                            AcceptsValues = false
                        }),
                        new SwitchInfo("mod", /* Localizable */ "NKS_SHELL_SHELLS_COMMAND_HELP_SWITCH_MOD_DESC", new SwitchOptions()
                        {
                            AcceptsValues = false
                        }),
                        new SwitchInfo("alias", /* Localizable */ "NKS_SHELL_SHELLS_COMMAND_HELP_SWITCH_ALIAS_DESC", new SwitchOptions()
                        {
                            AcceptsValues = false
                        }),
                        new SwitchInfo("unified", /* Localizable */ "NKS_SHELL_SHELLS_COMMAND_HELP_SWITCH_UNIFIED_DESC", new SwitchOptions()
                        {
                            AcceptsValues = false
                        }),
                        new SwitchInfo("addon", /* Localizable */ "NKS_SHELL_SHELLS_COMMAND_HELP_SWITCH_ADDON_DESC", new SwitchOptions()
                        {
                            AcceptsValues = false
                        }),
                        new SwitchInfo("all", /* Localizable */ "NKS_SHELL_SHELLS_COMMAND_HELP_SWITCH_ALL_DESC", new SwitchOptions()
                        {
                            AcceptsValues = false
                        }),
                        new SwitchInfo("simplified", /* Localizable */ "NKS_SHELL_SHELLS_COMMAND_HELP_SWITCH_SIMPLIFIED_DESC", new SwitchOptions()
                        {
                            AcceptsValues = false
                        }),
                    ], false)
                ], new HelpUnifiedCommand(), CommandFlags.Wrappable),

            new CommandInfo("loadhistories", /* Localizable */ "NKS_SHELL_SHELLS_COMMAND_LOADHISTORIES_DESC", new LoadHistoriesUnifiedCommand()),

            new CommandInfo("pipe", /* Localizable */ "NKS_SHELL_SHELLS_COMMAND_PIPE_DESC",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "sourceCommand", new CommandArgumentPartOptions()
                        {
                            AutoCompleter = (_) => CommandManager.GetCommandNames(CurrentShellType),
                            ArgumentDescription = /* Localizable */ "NKS_SHELL_SHELLS_COMMAND_PIPE_ARGUMENT_SOURCE_DESC"
                        }),
                        new CommandArgumentPart(true, "targetCommand", new CommandArgumentPartOptions()
                        {
                            AutoCompleter = (_) => CommandManager.GetCommandNames(CurrentShellType),
                            ArgumentDescription = /* Localizable */ "NKS_SHELL_SHELLS_COMMAND_PIPE_ARGUMENT_TARGET_DESC"
                        }),
                    ],
                    [
                        new SwitchInfo("quoted", /* Localizable */ "NKS_SHELL_SHELLS_COMMAND_PIPE_SWITCH_QUOTED_DESC")
                    ], true)
                ], new PipeUnifiedCommand()),

            new CommandInfo("presets", /* Localizable */ "NKS_SHELL_SHELLS_COMMAND_PRESETS_DESC", new PresetsUnifiedCommand()),

            new CommandInfo("repeat", /* Localizable */ "NKS_SHELL_SHELLS_COMMAND_REPEAT_DESC",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "times", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true,
                            ArgumentDescription = /* Localizable */ "NKS_SHELL_SHELLS_COMMAND_PIPE_ARGUMENT_TARGET_DESC"
                        }),
                        new CommandArgumentPart(false, "command"),
                    ])
                ], new RepeatUnifiedCommand()),

            new CommandInfo("savehistories", /* Localizable */ "NKS_SHELL_SHELLS_COMMAND_SAVEHISTORIES_DESC", new SaveHistoriesUnifiedCommand()),

            new CommandInfo("tip", /* Localizable */ "NKS_SHELL_SHELLS_COMMAND_TIP_DESC", new TipUnifiedCommand()),

            new CommandInfo("wrap", /* Localizable */ "NKS_SHELL_SHELLS_COMMAND_WRAP_DESC",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "command", new CommandArgumentPartOptions()
                        {
                            AutoCompleter = (_) => CommandExecutor.GetWrappableCommands(CurrentShellType),
                            ArgumentDescription = /* Localizable */ "NKS_SHELL_SHELLS_COMMAND_WRAP_ARGUMENT_COMMAND_DESC"
                        })
                    ])
                ], new WrapUnifiedCommand()),
        ];

        internal readonly static Dictionary<string, BaseShellInfo> availableShells = new()
        {
            { "Shell", new UESHShellInfo() },
            { "TextShell", new TextShellInfo() },
            { "HexShell", new HexShellInfo() },
            { "AdminShell", new AdminShellInfo() },
            { "DebugShell", new DebugShellInfo() }
        };
        internal readonly static Dictionary<string, BaseShellInfo> availableCustomShells = [];

        /// <summary>
        /// List of unified commands
        /// </summary>
        public static CommandInfo[] UnifiedCommands =>
            [.. unifiedCommandDict];

        /// <summary>
        /// List of available shells
        /// </summary>
        public static ReadOnlyDictionary<string, BaseShellInfo> AvailableShells =>
            new(availableShells.Union(availableCustomShells).ToDictionary());

        /// <summary>
        /// Current shell type
        /// </summary>
        public static string CurrentShellType =>
            ShellStack[^1].ShellType;

        /// <summary>
        /// Last shell type
        /// </summary>
        public static string LastShellType
        {
            get
            {
                if (ShellStack.Count == 0)
                {
                    // We don't have any shell. Return Shell.
                    DebugWriter.WriteDebug(DebugLevel.W, "Trying to call LastShellType on empty shell stack. Assuming UESH...");
                    return "Shell";
                }
                else if (ShellStack.Count == 1)
                {
                    // We only have one shell. Consider current as last.
                    DebugWriter.WriteDebug(DebugLevel.W, "Trying to call LastShellType on shell stack containing only one shell. Assuming curent...");
                    return CurrentShellType;
                }
                else
                {
                    // We have more than one shell. Return the shell type for a shell before the last one.
                    var type = ShellStack[^2].ShellType;
                    DebugWriter.WriteDebug(DebugLevel.I, "Returning shell type {0} for last shell from the stack...", vars: [type]);
                    return type;
                }
            }
        }

        /// <summary>
        /// Inputs for command then parses a specified command.
        /// </summary>
        /// <remarks>All new shells implemented either in KS or by mods should use this routine to allow effective and consistent line parsing.</remarks>
        public static void GetLine() =>
            GetLine("", "", CurrentShellType, true, Config.MainConfig.SetTitleOnCommandExecution);

        /// <summary>
        /// Parses a specified command.
        /// </summary>
        /// <param name="FullCommand">The full command string</param>
        /// <remarks>All new shells implemented either in KS or by mods should use this routine to allow effective and consistent line parsing.</remarks>
        public static void GetLine(string FullCommand) =>
            GetLine(FullCommand, "", CurrentShellType, true, Config.MainConfig.SetTitleOnCommandExecution);

        /// <summary>
        /// Parses a specified command.
        /// </summary>
        /// <param name="FullCommand">The full command string</param>
        /// <param name="OutputPath">Optional (non-)neutralized output path</param>
        /// <remarks>All new shells implemented either in KS or by mods should use this routine to allow effective and consistent line parsing.</remarks>
        public static void GetLine(string FullCommand, string OutputPath = "") =>
            GetLine(FullCommand, OutputPath, CurrentShellType, true, Config.MainConfig.SetTitleOnCommandExecution);

        /// <summary>
        /// Parses a specified command.
        /// </summary>
        /// <param name="FullCommand">The full command string</param>
        /// <param name="OutputPath">Optional (non-)neutralized output path</param>
        /// <param name="ShellType">Shell type</param>
        /// <param name="restoreDriver">Whether to restore the driver to the previous state</param>
        /// <remarks>All new shells implemented either in KS or by mods should use this routine to allow effective and consistent line parsing.</remarks>
        public static void GetLine(string FullCommand, string OutputPath = "", ShellType ShellType = ShellType.Shell, bool restoreDriver = true) =>
            GetLine(FullCommand, OutputPath, GetShellTypeName(ShellType), restoreDriver, Config.MainConfig.SetTitleOnCommandExecution);

        /// <summary>
        /// Parses a specified command.
        /// </summary>
        /// <param name="FullCommand">The full command string</param>
        /// <param name="OutputPath">Optional (non-)neutralized output path</param>
        /// <param name="ShellType">Shell type</param>
        /// <param name="restoreDriver">Whether to restore the driver to the previous state</param>
        /// <remarks>All new shells implemented either in KS or by mods should use this routine to allow effective and consistent line parsing.</remarks>
        public static void GetLine(string FullCommand, string OutputPath = "", string ShellType = "Shell", bool restoreDriver = true) =>
            GetLine(FullCommand, OutputPath, ShellType, restoreDriver, Config.MainConfig.SetTitleOnCommandExecution);

        /// <summary>
        /// Parses a specified command.
        /// </summary>
        /// <param name="FullCommand">The full command string</param>
        /// <param name="OutputPath">Optional (non-)neutralized output path</param>
        /// <param name="ShellType">Shell type</param>
        /// <param name="restoreDriver">Whether to restore the driver to the previous state</param>
        /// <param name="setTitle">Whether to set the console title</param>
        /// <remarks>All new shells implemented either in KS or by mods should use this routine to allow effective and consistent line parsing.</remarks>
        internal static void GetLine(string FullCommand, string OutputPath, string ShellType, bool restoreDriver, bool setTitle)
        {
            // Check for sanity
            if (string.IsNullOrEmpty(FullCommand))
                FullCommand = "";

            // Variables
            string? TargetFile = "";
            string TargetFileName = "";

            // Get the shell info
            var shellInfo = GetShellInfo(ShellType);

            // Now, initialize the command autocomplete handler. This will not be invoked if we have auto completion disabled.
            var settings = new TermReaderSettings(InputTools.globalSettings)
            {
                Suggestions = (text, index, _) => CommandAutoComplete.GetSuggestions(text, index),
                SuggestionsDelimiters = [' '],
                TreatCtrlCAsInput = true,
                InputForegroundColor = KernelColorTools.GetColor(KernelColorType.Input),
                HistoryName = ShellType,
                HistoryEnabled = Config.MainConfig.InputHistoryEnabled,
            };

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
                string prompt = "";
                lock (CancellationHandlers.GetCancelSyncLock(ShellType))
                {
                    // Print a prompt
                    var preset = PromptPresetManager.GetCurrentPresetBaseFromShell(ShellType);
                    if (!string.IsNullOrEmpty(FullCommand))
                        prompt = preset.PresetPromptCompletion;
                    else
                        prompt = preset.PresetPrompt;
                }

                // Raise shell initialization event
                EventsManager.FireEvent(EventType.ShellInitialized, ShellType);

                // Wait for command
                DebugWriter.WriteDebug(DebugLevel.I, "Waiting for command");
                string strcommand =
                    shellInfo.OneLineWrap ?
                    InputTools.ReadLineWrapped(prompt, "", settings) :
                    InputTools.ReadLine(prompt, "", settings);
                DebugWriter.WriteDebug(DebugLevel.I, "Waited for command [{0}]", vars: [strcommand]);
                if (strcommand == ";")
                    strcommand = "";

                // Add command to command builder and return the final result. The reason to add the extra space before the second command written is that
                // because if we need to provide a second command to the shell in a separate line, we usually add the semicolon at the end of the primary
                // command input.
                if (!string.IsNullOrEmpty(FullCommand) && !string.IsNullOrEmpty(strcommand))
                    commandBuilder.Append(' ');

                // There are cases when strcommand may be empty, so ignore that if it's empty.
                if (!string.IsNullOrEmpty(strcommand))
                    commandBuilder.Append(strcommand);
                FullCommand = commandBuilder.ToString();

                // There are cases when the kernel panics or reboots in the middle of the command input. If reboot is requested,
                // ensure that we're really gone.
                if (PowerManager.RebootRequested)
                    return;
            }

            // Check for a type of command
            CancellationHandlers.AllowCancel();
            var SplitCommands = FullCommand.Split([" ; "], StringSplitOptions.RemoveEmptyEntries);
            var Commands = CommandManager.GetCommands(ShellType);
            for (int i = 0; i < SplitCommands.Length; i++)
            {
                string Command = SplitCommands[i];

                // Then, check to see if this shell uses the slash command
                if (shellInfo.SlashCommand)
                {
                    if (!Command.StartsWith('/'))
                    {
                        // Not a slash command. Do things differently
                        var ShellInstance = ShellStack[^1];
                        DebugWriter.WriteDebug(DebugLevel.I, "Non-slash cmd exec succeeded. Running with {0}", vars: [Command]);
                        var Params = new CommandExecutorParameters(Command, shellInfo.NonSlashCommandInfo, ShellType, ShellInstance);
                        CommandExecutor.StartCommandThread(Params);
                        UESHVariables.SetVariable("UESHErrorCode", $"{ShellInstance.LastErrorCode}");
                        continue;
                    }
                    else
                    {
                        // Strip the slash
                        Command = Command[1..].Trim();
                    }
                }

                // Fire an event of PreExecuteCommand
                EventsManager.FireEvent(EventType.PreExecuteCommand, ShellType, Command);

                // Initialize local UESH variables (if found)
                string localVarStoreMatchRegex = /* lang=regex */ @"^\((.+)\)\s+";
                var localVarStoreMatch = RegexpTools.Match(Command, localVarStoreMatchRegex);
                string varStoreString = localVarStoreMatch.Groups[1].Value;
                DebugWriter.WriteDebug(DebugLevel.I, "varStoreString is: {0}", vars: [varStoreString]);
                string varStoreStringFull = localVarStoreMatch.Value;
                var varStoreVars = UESHVariables.GetVariablesFrom(varStoreString);

                // First, check to see if we already have that variable. If we do, get its old value.
                List<(string, string)> oldVarValues = [];
                foreach (string varStoreKey in varStoreVars.varStoreKeys)
                {
                    if (UESHVariables.Variables.ContainsKey(varStoreKey))
                        oldVarValues.Add((varStoreKey, UESHVariables.GetVariable(varStoreKey)));
                }
                UESHVariables.InitializeVariablesFrom(varStoreString);
                Command = Command[varStoreStringFull.Length..];

                // Check to see if the command is a comment
                if (!string.IsNullOrWhiteSpace(Command) && !Command.StartsWithAnyOf([" ", "#"]))
                {
                    // Get the command name
                    var words = Command.SplitEncloseDoubleQuotes();
                    string commandName = words[0].ReleaseDoubleQuotes();

                    // Verify that we aren't tricked into processing an empty command
                    if (string.IsNullOrEmpty(commandName))
                        break;

                    // Now, split the arguments
                    string arguments = string.Join(' ', words.Skip(1));

                    // Get the target file and path
                    TargetFile = RegexpTools.Unescape(commandName);
                    bool existsInPath = PathLookupTools.FileExistsInPath(commandName, ref TargetFile);
                    bool pathValid = FilesystemTools.TryParsePath(TargetFile ?? "");
                    if (!existsInPath || string.IsNullOrEmpty(TargetFile))
                        TargetFile = FilesystemTools.NeutralizePath(commandName);
                    if (pathValid)
                        TargetFileName = Path.GetFileName(TargetFile);
                    DebugWriter.WriteDebug(DebugLevel.I, "Finished finalCommand: {0}", vars: [commandName]);
                    DebugWriter.WriteDebug(DebugLevel.I, "Finished TargetFile: {0}", vars: [TargetFile]);

                    // Reads command written by user
                    try
                    {
                        // Set title
                        if (setTitle)
                            ConsoleMisc.SetTitle($"{KernelReleaseInfo.ConsoleTitle} - {Command}");

                        // Check the command
                        bool exists = Commands.Any((ci) => ci.Command == commandName || ci.Aliases.Any((ai) => ai.Alias == commandName));
                        if (exists)
                        {
                            // Execute the command
                            DebugWriter.WriteDebug(DebugLevel.I, "Executing command");
                            var cmdInfo = Commands.Single((ci) => ci.Command == commandName || ci.Aliases.Any((ai) => ai.Alias == commandName));

                            // Check to see if the command supports redirection
                            if (cmdInfo.Flags.HasFlag(CommandFlags.RedirectionSupported))
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

                            if (!string.IsNullOrEmpty(commandName) || !commandName.StartsWithAnyOf([" ", "#"]))
                            {

                                // Check to see if a user is able to execute a command
                                if (ShellType == "Shell")
                                {
                                    if (cmdInfo.Flags.HasFlag(CommandFlags.Strict))
                                    {
                                        if (!PermissionsTools.IsPermissionGranted(PermissionTypes.RunStrictCommands) &&
                                            !UserManagement.CurrentUser.Flags.HasFlag(UserFlags.Administrator))
                                        {
                                            DebugWriter.WriteDebug(DebugLevel.W, "Cmd exec {0} failed: adminList(signedinusrnm) is False, strictCmds.Contains({0}) is True", vars: [commandName]);
                                            TextWriters.Write(LanguageTools.GetLocalized("NKS_SHELL_SHELLS_NEEDSPERM"), true, KernelColorType.Error, commandName);
                                            UESHVariables.SetVariable("UESHErrorCode", "-4");
                                            break;
                                        }
                                    }
                                }

                                // Check the command before starting
                                if (KernelEntry.Maintenance & cmdInfo.Flags.HasFlag(CommandFlags.NoMaintenance))
                                {
                                    DebugWriter.WriteDebug(DebugLevel.W, "Cmd exec {0} failed: In maintenance mode. {0} is in NoMaintenanceCmds", vars: [commandName]);
                                    TextWriters.Write(LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UNUSABLEINMAINTENANCE"), true, KernelColorType.Error, commandName);
                                    UESHVariables.SetVariable("UESHErrorCode", "-3");
                                }
                                else
                                {
                                    var ShellInstance = ShellStack[^1];
                                    DebugWriter.WriteDebug(DebugLevel.I, "Cmd exec {0} succeeded. Running with {1}", vars: [commandName, Command]);
                                    var Params = new CommandExecutorParameters(Command, cmdInfo, ShellType, ShellInstance);
                                    CommandExecutor.StartCommandThread(Params);
                                    UESHVariables.SetVariable("UESHErrorCode", $"{ShellInstance.LastErrorCode}");
                                }
                            }
                        }
                        else if (pathValid & ShellType == "Shell")
                        {
                            // If we're in the UESH shell, parse the script file or executable file
                            if (FilesystemTools.FileExists(TargetFile) & !TargetFile.EndsWith(".uesh"))
                            {
                                DebugWriter.WriteDebug(DebugLevel.I, "Cmd exec {0} succeeded because file is found.", vars: [commandName]);
                                try
                                {
                                    // Create a new instance of process
                                    PermissionsTools.Demand(PermissionTypes.ExecuteProcesses);
                                    if (pathValid)
                                    {
                                        var targetCommand = Command.Replace(TargetFileName, "");
                                        targetCommand = targetCommand.TrimStart('\0', ' ');
                                        DebugWriter.WriteDebug(DebugLevel.I, "Command: {0}, Arguments: {1}", vars: [TargetFile, targetCommand]);
                                        var Params = new ExecuteProcessThreadParameters(TargetFile, targetCommand);
                                        ProcessStartCommandThread.Start(Params);
                                        ProcessStartCommandThread.Wait();
                                        ProcessStartCommandThread.Stop();
                                        UESHVariables.SetVariable("UESHErrorCode", "0");
                                    }
                                }
                                catch (Exception ex)
                                {
                                    DebugWriter.WriteDebug(DebugLevel.E, "Failed to start process: {0}", vars: [ex.Message]);
                                    TextWriters.Write(LanguageTools.GetLocalized("NKS_SHELL_BASE_COMMANDSTARTFAILED"), true, KernelColorType.Error, commandName, ex.Message);
                                    DebugWriter.WriteDebugStackTrace(ex);
                                    if (ex is KernelException kex)
                                        UESHVariables.SetVariable("UESHErrorCode", $"{KernelExceptionTools.GetErrorCode(kex)}");
                                    else
                                        UESHVariables.SetVariable("UESHErrorCode", $"{ex.GetHashCode()}");
                                }
                            }
                            else if (FilesystemTools.FileExists(TargetFile) & TargetFile.EndsWith(".uesh"))
                            {
                                try
                                {
                                    PermissionsTools.Demand(PermissionTypes.ExecuteScripts);
                                    DebugWriter.WriteDebug(DebugLevel.I, "Cmd exec {0} succeeded because it's a UESH script.", vars: [commandName]);
                                    UESHParse.Execute(TargetFile, arguments);
                                    UESHVariables.SetVariable("UESHErrorCode", "0");
                                }
                                catch (Exception ex)
                                {
                                    TextWriters.Write(LanguageTools.GetLocalized("NKS_SHELL_BASE_SCRIPTING_CANTEXECUTE"), true, KernelColorType.Error, ex.Message);
                                    DebugWriter.WriteDebugStackTrace(ex);
                                    if (ex is KernelException kex)
                                        UESHVariables.SetVariable("UESHErrorCode", $"{KernelExceptionTools.GetErrorCode(kex)}");
                                    else
                                        UESHVariables.SetVariable("UESHErrorCode", $"{ex.GetHashCode()}");
                                }
                            }
                            else
                            {
                                DebugWriter.WriteDebug(DebugLevel.W, "Cmd exec {0} failed: command {0} not found parsing target file", vars: [commandName]);
                                TextWriters.Write(LanguageTools.GetLocalized("NKS_SHELL_SHELLS_COMMANDNOTFOUND"), true, KernelColorType.Error, commandName);
                                UESHVariables.SetVariable("UESHErrorCode", "-2");
                            }
                        }
                        else
                        {
                            DebugWriter.WriteDebug(DebugLevel.W, "Cmd exec {0} failed: command {0} not found", vars: [commandName]);
                            TextWriters.Write(LanguageTools.GetLocalized("NKS_SHELL_SHELLS_COMMANDNOTFOUND"), true, KernelColorType.Error, commandName);
                            UESHVariables.SetVariable("UESHErrorCode", "-1");
                        }
                    }
                    catch (Exception ex)
                    {
                        DebugWriter.WriteDebugStackTrace(ex);
                        TextWriters.Write(LanguageTools.GetLocalized("NKS_SHELL_BASE_COMMANDS_ERROREXECUTE2") + CharManager.NewLine + LanguageTools.GetLocalized("NKS_COMMON_ERRORDESC"), true, KernelColorType.Error, ex.GetType().FullName ?? "<null>", ex.Message);
                        if (ex is KernelException kex)
                            UESHVariables.SetVariable("UESHErrorCode", $"{KernelExceptionTools.GetErrorCode(kex)}");
                        else
                            UESHVariables.SetVariable("UESHErrorCode", $"{ex.GetHashCode()}");
                    }
                }

                // Fire an event of PostExecuteCommand and reset all local variables
                var varStoreKeys = varStoreVars.varStoreKeys;
                foreach (string varStoreKey in varStoreKeys)
                    UESHVariables.RemoveVariable(varStoreKey);
                foreach (var varStoreKeyOld in oldVarValues)
                {
                    string key = varStoreKeyOld.Item1;
                    string value = varStoreKeyOld.Item2;
                    UESHVariables.InitializeVariable(key);
                    UESHVariables.SetVariable(key, value);
                }
                EventsManager.FireEvent(EventType.PostExecuteCommand, ShellType, Command);
            }

            // Restore console output to its original state if any
            if (DriverHandler.CurrentConsoleDriverLocal.DriverName != "Default" && restoreDriver)
            {
                if (DriverHandler.CurrentConsoleDriverLocal is File writer)
                    writer.FilterVT = false;
                if (DriverHandler.CurrentConsoleDriverLocal is FileSequence writerSeq)
                    writerSeq.FilterVT = false;
                DriverHandler.EndLocalDriver<IConsoleDriver>();
            }

            // Restore title and cancel possibility state
            if (setTitle)
                ConsoleMisc.SetTitle(KernelReleaseInfo.ConsoleTitle);
            CancellationHandlers.InhibitCancel();
            lastCommand = FullCommand;
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
            AvailableShells.TryGetValue(shellType, out BaseShellInfo? baseShellInfo) ? baseShellInfo : AvailableShells["Shell"];

        /// <summary>
        /// Starts the shell
        /// </summary>
        /// <param name="ShellType">The shell type</param>
        /// <param name="ShellArgs">Arguments to pass to shell</param>
        public static void StartShell(ShellType ShellType, params object[] ShellArgs) =>
            StartShell(GetShellTypeName(ShellType), ShellArgs);

        /// <summary>
        /// Starts the shell
        /// </summary>
        /// <param name="ShellType">The shell type</param>
        /// <param name="ShellArgs">Arguments to pass to shell</param>
        public static void StartShell(string ShellType, params object[] ShellArgs)
        {
            if (ShellStack.Count >= 1)
            {
                // The shell stack has a mother shell. Start another shell.
                StartShellInternal(ShellType, ShellArgs);
            }
            else
                throw new KernelException(KernelExceptionType.ShellOperation, LanguageTools.GetLocalized("NKS_SHELL_SHELLS_EXCEPTION_CANTSTARTWITHOUTMOTHERSHELL"));
        }

        /// <summary>
        /// Kills the last running shell
        /// </summary>
        public static void KillShell()
        {
            // We must have at least two shells to kill the last shell. Else, we will have zero shells running, making us look like we've logged out!
            if (IsOnMotherShell())
                throw new KernelException(KernelExceptionType.ShellOperation, LanguageTools.GetLocalized("NKS_SHELL_SHELLS_EXCEPTION_KILLINGMOTHERSHELL"));

            // Not a mother shell, so bail.
            KillShellInternal();
        }

        /// <summary>
        /// Cleans up the shell stack
        /// </summary>
        public static void PurgeShells() =>
            // Remove these shells from the stack
            ShellStack.RemoveAll(x => x.ShellBase?.Bail ?? true);

        /// <summary>
        /// Gets the shell executor based on the shell type
        /// </summary>
        /// <param name="ShellType">The requested shell type</param>
        public static BaseShell? GetShellExecutor(ShellType ShellType) =>
            GetShellExecutor(GetShellTypeName(ShellType));

        /// <summary>
        /// Gets the shell executor based on the shell type
        /// </summary>
        /// <param name="ShellType">The requested shell type</param>
        public static BaseShell? GetShellExecutor(string ShellType) =>
            GetShellInfo(ShellType).ShellBase;

        /// <summary>
        /// Registers the custom shell. Should be called when mods start up.
        /// </summary>
        /// <param name="ShellType">The shell type</param>
        /// <param name="ShellTypeInfo">The shell type information</param>
        public static void RegisterShell(string ShellType, BaseShellInfo ShellTypeInfo)
        {
            if (!ShellTypeExists(ShellType))
            {
                // First, add the shell
                availableCustomShells.Add(ShellType, ShellTypeInfo);

                // Then, add the default preset if the current preset is not found
                if (PromptPresetManager.CurrentPresets.ContainsKey(ShellType))
                    return;

                // Rare state.
                DebugWriter.WriteDebug(DebugLevel.I, "Reached rare state or unconfigurable shell.");
                var presets = ShellTypeInfo.ShellPresets;
                var basePreset = new PromptPresetBase();
                if (presets is not null)
                {
                    // Add a default preset
                    if (presets.ContainsKey("Default"))
                        PromptPresetManager.CurrentPresets.Add(ShellType, "Default");
                    else
                        PromptPresetManager.CurrentPresets.Add(ShellType, basePreset.PresetName);
                }
                else
                {
                    // Make a base shell preset and set it as default.
                    PromptPresetManager.CurrentPresets.Add(ShellType, basePreset.PresetName);
                }
            }
        }

        /// <summary>
        /// Unregisters the custom shell. Should be called when mods shut down.
        /// </summary>
        /// <param name="ShellType">The shell type</param>
        public static void UnregisterShell(string ShellType)
        {
            if (!IsShellBuiltin(ShellType))
            {
                // First, remove the shell
                availableCustomShells.Remove(ShellType);

                // Then, remove the preset
                PromptPresetManager.CurrentPresets.Remove(ShellType);
            }
        }

        /// <summary>
        /// Is the shell pre-defined in Nitrocid KS?
        /// </summary>
        /// <param name="ShellType">Shell type</param>
        /// <returns>If available in ShellType, then it's a built-in shell, thus returning true. Otherwise, false for custom shells.</returns>
        public static bool IsShellBuiltin(string ShellType) =>
            availableShells.ContainsKey(ShellType);

        /// <summary>
        /// Does the shell exist?
        /// </summary>
        /// <param name="ShellType">Shell type to check</param>
        /// <returns>True if it exists; false otherwise.</returns>
        public static bool ShellTypeExists(string ShellType) =>
            AvailableShells.ContainsKey(ShellType);

        /// <summary>
        /// Is the current shell a mother shell?
        /// </summary>
        /// <returns>True if the shell stack is at most one shell. False if running in the second shell or higher.</returns>
        public static bool IsOnMotherShell() =>
            ShellStack.Count < 2;

        /// <summary>
        /// Registers the addon shell. Should be called when addons start up.
        /// </summary>
        /// <param name="ShellType">The shell type</param>
        /// <param name="ShellTypeInfo">The shell type information</param>
        internal static void RegisterAddonShell(string ShellType, BaseShellInfo ShellTypeInfo)
        {
            if (!ShellTypeExists(ShellType))
            {
                // First, add the shell
                availableShells.Add(ShellType, ShellTypeInfo);

                // Then, add the default preset if the current preset is not found
                if (PromptPresetManager.CurrentPresets.ContainsKey(ShellType))
                    return;

                // Rare state.
                DebugWriter.WriteDebug(DebugLevel.I, "Reached rare state or unconfigurable shell.");
                var presets = ShellTypeInfo.ShellPresets;
                var basePreset = new PromptPresetBase();
                if (presets is not null)
                {
                    // Add a default preset
                    if (presets.ContainsKey("Default"))
                        PromptPresetManager.CurrentPresets.Add(ShellType, "Default");
                    else
                        PromptPresetManager.CurrentPresets.Add(ShellType, basePreset.PresetName);
                }
                else
                {
                    // Make a base shell preset and set it as default.
                    PromptPresetManager.CurrentPresets.Add(ShellType, basePreset.PresetName);
                }
            }
        }

        /// <summary>
        /// Unregisters the addon shell. Should be called when addons shut down.
        /// </summary>
        /// <param name="ShellType">The shell type</param>
        internal static void UnregisterAddonShell(string ShellType)
        {
            if (IsShellBuiltin(ShellType))
            {
                // First, remove the shell
                availableShells.Remove(ShellType);

                // Then, remove the preset
                PromptPresetManager.CurrentPresets.Remove(ShellType);
            }
        }

        internal static void SaveHistories()
        {
            foreach (string history in histories.Keys)
            {
                if (HistoryTools.IsHistoryRegistered(history))
                    histories[history] = [.. HistoryTools.GetHistoryEntries(history)];
            }
            FileIO.WriteAllText(PathsManagement.ShellHistoriesPath, JsonConvert.SerializeObject(histories, Formatting.Indented));
        }

        internal static void LoadHistories()
        {
            string path = PathsManagement.ShellHistoriesPath;
            if (!FilesystemTools.FileExists(path))
                return;
            histories = JsonConvert.DeserializeObject<Dictionary<string, List<string>>>(FileIO.ReadAllText(path)) ?? [];
            foreach (string history in histories.Keys)
            {
                if (!HistoryTools.IsHistoryRegistered(history))
                    HistoryTools.LoadFromInstance(new HistoryInfo(history, histories[history]));
                else
                    HistoryTools.Switch(history, [.. histories[history]]);
            }
        }

        /// <summary>
        /// Force starts the shell
        /// </summary>
        /// <param name="ShellType">The shell type</param>
        /// <param name="ShellArgs">Arguments to pass to shell</param>
        internal static void StartShellInternal(ShellType ShellType, params object[] ShellArgs) =>
            StartShellInternal(GetShellTypeName(ShellType), ShellArgs);

        /// <summary>
        /// Force starts the shell
        /// </summary>
        /// <param name="ShellType">The shell type</param>
        /// <param name="ShellArgs">Arguments to pass to shell</param>
        internal static void StartShellInternal(string ShellType, params object[] ShellArgs)
        {
            int shellCount = ShellStack.Count;
            try
            {
                // Make a shell executor based on shell type to select a specific executor (if the shell type is not UESH, and if the new shell isn't a mother shell)
                // Please note that the remote debug shell is not supported because it works on its own space, so it can't be interfaced using the standard IShell.
                var ShellExecute = GetShellExecutor(ShellType) ??
                    throw new KernelException(KernelExceptionType.ShellOperation, LanguageTools.GetLocalized("NKS_SHELL_SHELLS_EXCEPTION_NOEXECUTOR") + $" {ShellType}");

                // Make a new instance of shell information
                var ShellCommandThread = new KernelThread($"{ShellType} Command Thread", false, (cmdThreadParams) => CommandExecutor.ExecuteCommand((CommandExecutorParameters?)cmdThreadParams));
                var ShellInfo = new ShellExecuteInfo(ShellType, ShellExecute, ShellCommandThread);

                // Add a new shell to the shell stack to indicate that we have a new shell (a visitor)!
                ShellStack.Add(ShellInfo);
                if (!HistoryTools.IsHistoryRegistered(ShellType))
                    HistoryTools.LoadFromInstance(new HistoryInfo(ShellType, []));

                // Reset title in case we're going to another shell
                ConsoleMisc.SetTitle(KernelReleaseInfo.ConsoleTitle);
                ShellExecute.InitializeShell(ShellArgs);
            }
            catch (Exception ex)
            {
                // There is an exception trying to run the shell. Throw the message to the debugger and to the caller.
                DebugWriter.WriteDebug(DebugLevel.E, "Failed initializing shell!!! Type: {0}, Message: {1}", vars: [ShellType, ex.Message]);
                DebugWriter.WriteDebug(DebugLevel.E, "Additional info: Args: {0} [{1}], Shell Stack: {2} shells, shellCount: {3} shells", vars: [ShellArgs.Length, string.Join(", ", ShellArgs), ShellStack.Count, shellCount]);
                DebugWriter.WriteDebug(DebugLevel.E, "This shell needs to be killed in order for the shell manager to proceed. Passing exception to caller...");
                DebugWriter.WriteDebugStackTrace(ex);
                DebugWriter.WriteDebug(DebugLevel.E, "If you don't see \"Purge\" from {0} after few lines, this indicates that we're in a seriously corrupted state.", vars: [nameof(StartShellInternal)]);
                throw new KernelException(KernelExceptionType.ShellOperation, LanguageTools.GetLocalized("NKS_SHELL_SHELLS_EXCEPTION_CANTINITSHELL"), ex);
            }
            finally
            {
                // There is either an unknown shell error trying to be initialized or a shell has manually set Bail to true prior to exiting, like the JSON shell
                // that sets this property when it fails to open the JSON file due to syntax error or something. If we haven't added the shell to the shell stack,
                // do nothing. Else, purge that shell with KillShell(). Otherwise, we'll get another shell's commands in the wrong shell and other problems will
                // occur until the ghost shell has exited either automatically or manually, so check to see if we have added the newly created shell to the shell
                // stack and kill that faulted shell so that we can have the correct shell in the most recent shell, ^1, from the stack.
                int newShellCount = ShellStack.Count;
                DebugWriter.WriteDebug(DebugLevel.I, "Purge: newShellCount: {0} shells, shellCount: {1} shells", vars: [newShellCount, shellCount]);
                if (newShellCount > shellCount)
                    KillShellInternal();

                // Terminaux has introduced recent changes surrounding the history feature of the reader that allows it to save and load custom histories, so we
                // need to make use of it to be able to save histories in one file.
                SaveHistories();
            }
        }

        /// <summary>
        /// Force kills the last running shell
        /// </summary>
        internal static void KillShellInternal()
        {
            if (ShellStack.Count >= 1)
            {
                var shellBase = ShellStack[^1].ShellBase;
                if (shellBase is not null)
                    shellBase.Bail = true;
                PurgeShells();
            }
        }

        /// <summary>
        /// Kills all the shells
        /// </summary>
        internal static void KillAllShells()
        {
            for (int i = ShellStack.Count - 1; i >= 0; i--)
            {
                var shellBase = ShellStack[i].ShellBase;
                if (shellBase is not null)
                    shellBase.Bail = true;
            }
            PurgeShells();
        }

        /// <summary>
        /// Initializes the redirection
        /// </summary>
        private static string InitializeRedirection(string Command)
        {
            // If requested command has output redirection sign after arguments, remove it from final command string and set output to that file
            string RedirectionPattern = /*lang=regex*/ @"(?:( (?:>>|>>>) )(.+?))+$";
            if (RegexpTools.IsMatch(Command, RedirectionPattern))
            {
                var outputMatch = Regex.Match(Command, RedirectionPattern);
                var outputFiles = outputMatch.Groups[2].Captures.Select((cap) => cap.Value).ToArray();
                var outputFileModes = outputMatch.Groups[1].Captures.Select((cap) => cap.Value).ToArray();
                List<string> filePaths = [];
                for (int i = 0; i < outputFiles.Length; i++)
                {
                    string outputFile = outputFiles[i];
                    bool isOverwrite = outputFileModes[i] != " >>> ";
                    string OutputFilePath = FilesystemTools.NeutralizePath(outputFile);
                    DebugWriter.WriteDebug(DebugLevel.I, "Output redirection found for file {1} with overwrite mode [{0}].", vars: [isOverwrite, OutputFilePath]);
                    if (isOverwrite)
                        FilesystemTools.ClearFile(OutputFilePath);
                    filePaths.Add(OutputFilePath);
                }
                DriverHandler.BeginLocalDriver<IConsoleDriver>("FileSequence");
                ((FileSequence)DriverHandler.CurrentConsoleDriverLocal).PathsToWrite = [.. filePaths];
                ((FileSequence)DriverHandler.CurrentConsoleDriverLocal).FilterVT = true;
                Command = Command.RemoveSuffix(outputMatch.Value);
            }
            else if (Command.EndsWith(" |SILENT|"))
            {
                DebugWriter.WriteDebug(DebugLevel.I, "Silence found. Redirecting to null writer...");
                DriverHandler.BeginLocalDriver<IConsoleDriver>("Null");
                Command = Command.RemoveSuffix(" |SILENT|");
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
                DebugWriter.WriteDebug(DebugLevel.I, "Optional output redirection found using OutputPath ({0}).", vars: [OutputPath]);
                OutputPath = FilesystemTools.NeutralizePath(OutputPath);
                DriverHandler.BeginLocalDriver<IConsoleDriver>("File");
                ((File)DriverHandler.CurrentConsoleDriverLocal).PathToWrite = OutputPath;
            }
        }

    }
}
