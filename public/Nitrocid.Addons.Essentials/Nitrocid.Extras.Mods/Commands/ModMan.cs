//
// Nitrocid  Copyright (C) 2018-2026  Aptivi
//
// This file is part of Nitrocid
//
// Nitrocid is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// Nitrocid is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY, without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.
//

using System;
using System.IO;
using Nitrocid.Base.Files;
using Nitrocid.Base.Files.Paths;
using Nitrocid.Base.Kernel;
using Nitrocid.Base.Kernel.Debugging;
using Nitrocid.Base.Kernel.Exceptions;
using Nitrocid.Base.Languages;
using Nitrocid.Base.Security.Permissions;
using Nitrocid.Base.Users;
using Nitrocid.Extras.Mods.Modifications;
using Nitrocid.Extras.Mods.Modifications.Interactive;
using Terminaux.Inputs.Interactive;
using Terminaux.Shell.Arguments;
using Terminaux.Shell.Commands;
using Terminaux.Shell.Help;
using Terminaux.Shell.Shells;
using Terminaux.Themes.Colors;
using Terminaux.Writer.ConsoleWriters;

namespace Nitrocid.Extras.Mods.Commands
{
    /// <summary>
    /// Manages your mods
    /// </summary>
    /// <remarks>
    /// You can manage all your mods installed in Nitrocid by this command. It allows you to stop, start, reload, get info, and list all your mods.
    /// <br></br>
    /// The user must have at least the administrative privileges before they can run the below commands.
    /// </remarks>
    class ModManCommand : BaseCommand, ICommand
    {
        public override string Command =>
            "modman";

        public override string HelpDefinition =>
            LanguageTools.GetLocalized("NKS_MODS_COMMAND_MODMAN_DESC");

        public override CommandArgumentInfo[] CommandArgumentInfo =>
            [
                new CommandArgumentInfo(
                [
                    new CommandArgumentPart(true, "mode", new()
                    {
                        ExactWording = ["start", "stop", "info", "reload", "install", "uninstall"],
                        ArgumentDescription = /* Localizable */ "NKS_MODS_COMMAND_MODMAN_ARGUMENT_STARTSTOP_DESC"
                    }),
                    new CommandArgumentPart(true, "modfilename", new CommandArgumentPartOptions()
                    {
                        ArgumentDescription = /* Localizable */ "NKS_MODS_COMMAND_MODMAN_ARGUMENT_MODFILENAME_DESC"
                    }),
                ]),
                new CommandArgumentInfo(
                [
                    new CommandArgumentPart(true, "mode", new()
                    {
                        ExactWording = ["list", "reloadall", "stopall", "startall", "tui"],
                        ArgumentDescription = /* Localizable */ "NKS_MODS_COMMAND_MODMAN_ARGUMENT_LISTRELOAD_DESC"
                    }),
                ]),
            ];

        public override int Execute(IShell? shell, CommandParameters parameters, ref string variableValue)
        {
#pragma warning disable NLOC0001
            if (!PermissionsTools.IsPermissionGranted(PermissionTypes.RunStrictCommands) &&
                !UserManagement.CurrentUser.Flags.HasFlag(UserFlags.Administrator))
            {
                DebugWriter.WriteDebug(DebugLevel.W, "Cmd exec {0} failed: adminList(signedinusrnm) is False, strictCmds.Contains({0}) is True", vars: [parameters.CommandText]);
                TextWriterColor.Write(LanguageTools.GetLocalized("NKS_SHELL_SHELLS_NEEDSPERM"), true, ThemeColorType.Error, parameters.CommandText);
                return -4;
            }
#pragma warning restore NLOC0001

            if (!KernelEntry.SafeMode)
            {
                PermissionsTools.Demand(PermissionTypes.ManageMods);
                string CommandMode = parameters.ArgumentsList[0].ToLower();
                string TargetMod = "";
                string TargetModPath = "";
                string ModListTerm = "";

                // These command modes require two arguments to be passed, so re-check here and there. Optional arguments also lie there.
                switch (CommandMode)
                {
                    case "start":
                    case "stop":
                    case "info":
                    case "reload":
                    case "install":
                    case "uninstall":
                        TargetMod = parameters.ArgumentsList[1];
                        TargetModPath = FilesystemTools.NeutralizePath(TargetMod, PathsManagement.GetKernelPath(KernelPathType.Mods));
                        if (!FilesystemTools.FileExists(TargetModPath))
                        {
                            TextWriterColor.Write(LanguageTools.GetLocalized("NKS_MODS_MODMAN_MODNOTFOUND"), true, ThemeColorType.Error);
                            return KernelExceptionTools.GetErrorCode(KernelExceptionType.NoSuchMod);
                        }
                        break;
                }

                // Now, the actual logic
                switch (CommandMode)
                {
                    case "start":
                        {
                            TextWriterColor.Write(LanguageTools.GetLocalized("NKS_MODS_STARTINGMOD") + " {0}...", Path.GetFileNameWithoutExtension(TargetMod));
                            ModManager.StartMod(Path.GetFileName(TargetModPath));
                            break;
                        }
                    case "stop":
                        {
                            ModManager.StopMod(Path.GetFileName(TargetModPath));
                            break;
                        }
                    case "info":
                        {
                            foreach (string script in ModManager.Mods.Keys)
                            {
                                if (ModManager.Mods[script].ModFilePath == TargetModPath)
                                {
                                    SeparatorWriterColor.WriteSeparatorColor(script, ThemeColorsTools.GetColor(ThemeColorType.ListTitle));
                                    TextWriterColor.Write("- " + LanguageTools.GetLocalized("NKS_MODS_MODMAN_MODNAME") + " ", false, ThemeColorType.ListEntry);
                                    TextWriterColor.Write(ModManager.Mods[script].ModName, true, ThemeColorType.ListValue);
                                    TextWriterColor.Write("- " + LanguageTools.GetLocalized("NKS_MODS_MODMAN_MODFILENAME") + " ", false, ThemeColorType.ListEntry);
                                    TextWriterColor.Write(ModManager.Mods[script].ModFileName, true, ThemeColorType.ListValue);
                                    TextWriterColor.Write("- " + LanguageTools.GetLocalized("NKS_MODS_MODMAN_MODFILEPATH") + " ", false, ThemeColorType.ListEntry);
                                    TextWriterColor.Write(ModManager.Mods[script].ModFilePath, true, ThemeColorType.ListValue);
                                    TextWriterColor.Write("- " + LanguageTools.GetLocalized("NKS_MODS_MODMAN_MODVER") + " ", false, ThemeColorType.ListEntry);
                                    TextWriterColor.Write(ModManager.Mods[script].ModVersion, true, ThemeColorType.ListValue);
                                }
                            }

                            break;
                        }
                    case "reload":
                        {
                            ModManager.ReloadMod(Path.GetFileName(TargetModPath));
                            break;
                        }
                    case "install":
                        {
                            ModManager.InstallMod(TargetMod);
                            break;
                        }
                    case "uninstall":
                        {
                            ModManager.UninstallMod(TargetMod);
                            break;
                        }
                    case "list":
                        {
                            if (parameters.ArgumentsList.Length > 1)
                                ModListTerm = parameters.ArgumentsList[1];
                            foreach (string Mod in ModManager.ListMods(ModListTerm).Keys)
                            {
                                SeparatorWriterColor.WriteSeparatorColor(Mod, ThemeColorsTools.GetColor(ThemeColorType.ListTitle));
                                TextWriterColor.Write("- " + LanguageTools.GetLocalized("NKS_MODS_MODMAN_MODNAME") + " ", false, ThemeColorType.ListEntry);
                                TextWriterColor.Write(ModManager.Mods[Mod].ModName, true, ThemeColorType.ListValue);
                                TextWriterColor.Write("- " + LanguageTools.GetLocalized("NKS_MODS_MODMAN_MODFILENAME") + " ", false, ThemeColorType.ListEntry);
                                TextWriterColor.Write(ModManager.Mods[Mod].ModFileName, true, ThemeColorType.ListValue);
                                TextWriterColor.Write("- " + LanguageTools.GetLocalized("NKS_MODS_MODMAN_MODFILEPATH") + " ", false, ThemeColorType.ListEntry);
                                TextWriterColor.Write(ModManager.Mods[Mod].ModFilePath, true, ThemeColorType.ListValue);
                                TextWriterColor.Write("- " + LanguageTools.GetLocalized("NKS_MODS_MODMAN_MODVER") + " ", false, ThemeColorType.ListEntry);
                                TextWriterColor.Write(ModManager.Mods[Mod].ModVersion, true, ThemeColorType.ListValue);
                            }

                            break;
                        }
                    case "reloadall":
                        {
                            ModManager.ReloadMods();
                            break;
                        }
                    case "stopall":
                        {
                            ModManager.StopMods();
                            break;
                        }
                    case "startall":
                        {
                            ModManager.StartMods();
                            break;
                        }
                    case "tui":
                        {
                            var tui = new ModManagerTui();
                            tui.Bindings.AddRange([
                                new(LanguageTools.GetLocalized("NKS_MODS_TUI_KEYBINDING_STARTMODSELECT"), ConsoleKey.F1, (_, _, _, _) => tui.StartModPrompt(false), true),
                                new(LanguageTools.GetLocalized("NKS_MODS_TUI_KEYBINDING_STARTMODINPUT"), ConsoleKey.F1, ConsoleModifiers.Shift, (_, _, _, _) => tui.StartModPrompt(true), true),
                                new(LanguageTools.GetLocalized("NKS_MODS_TUI_KEYBINDING_STOPMOD"), ConsoleKey.F2, (modName, _, _, _) => tui.StopMod(modName)),
                                new(LanguageTools.GetLocalized("NKS_MODS_TUI_KEYBINDING_RELOADMOD"), ConsoleKey.F3, (modName, _, _, _) => tui.ReloadMod(modName)),
                                new(LanguageTools.GetLocalized("NKS_MODS_TUI_KEYBINDING_INSTALLMODSELECT"), ConsoleKey.F4, (_, _, _, _) => tui.InstallModPrompt(false), true),
                                new(LanguageTools.GetLocalized("NKS_MODS_TUI_KEYBINDING_INSTALLMODINPUT"), ConsoleKey.F4, ConsoleModifiers.Shift, (_, _, _, _) => tui.InstallModPrompt(true), true),
                                new(LanguageTools.GetLocalized("NKS_MODS_TUI_KEYBINDING_UNINSTALLMOD"), ConsoleKey.F5, (modName, _, _, _) => tui.UninstallMod(modName)),
                                new(LanguageTools.GetLocalized("NKS_MODS_TUI_KEYBINDING_STARTALL"), ConsoleKey.F6, (_, _, _, _) => ModManager.StartMods(), true),
                                new(LanguageTools.GetLocalized("NKS_MODS_TUI_KEYBINDING_STOPALL"), ConsoleKey.F7, (_, _, _, _) => ModManager.StopMods(), true),
                                new(LanguageTools.GetLocalized("NKS_MODS_TUI_KEYBINDING_RELOADALL"), ConsoleKey.F8, (_, _, _, _) => ModManager.ReloadMods(), true),
                            ]);
                            InteractiveTuiTools.OpenInteractiveTui(tui);
                            break;
                        }

                    default:
                        {
                            TextWriterColor.Write(LanguageTools.GetLocalized("NKS_MODS_MODMAN_NOCOMMAND"), true, ThemeColorType.Error, CommandMode);
                            return KernelExceptionTools.GetErrorCode(KernelExceptionType.ModManagement);
                        }
                }
            }
            else
            {
                TextWriterColor.Write(LanguageTools.GetLocalized("NKS_MODS_MODMAN_SAFEMODE"), true, ThemeColorType.Error);
                return KernelExceptionTools.GetErrorCode(KernelExceptionType.ModManagement);
            }
            return 0;
        }

    }
}
