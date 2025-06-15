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

using System.IO;
using Nitrocid.Kernel;
using Nitrocid.Shell.ShellBase.Help;
using Nitrocid.Shell.ShellBase.Commands;
using Nitrocid.Files;
using Nitrocid.ConsoleBase.Writers;
using Nitrocid.Languages;
using Nitrocid.Kernel.Exceptions;
using Nitrocid.Security.Permissions;
using Nitrocid.Files.Paths;
using Nitrocid.ConsoleBase.Colors;
using Terminaux.Writer.ConsoleWriters;
using Nitrocid.Extras.Mods.Modifications;
using Nitrocid.Extras.Mods.Modifications.Interactive;
using Terminaux.Inputs.Interactive;
using System;

namespace Nitrocid.Extras.Mods.Commands
{
    /// <summary>
    /// Manages your mods
    /// </summary>
    /// <remarks>
    /// You can manage all your mods installed in Nitrocid KS by this command. It allows you to stop, start, reload, get info, and list all your mods.
    /// <br></br>
    /// The user must have at least the administrative privileges before they can run the below commands.
    /// </remarks>
    class ModManCommand : BaseCommand, ICommand
    {

        public override int Execute(CommandParameters parameters, ref string variableValue)
        {
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
                        {
                            if (parameters.ArgumentsList.Length > 1)
                            {
                                TargetMod = parameters.ArgumentsList[1];
                                TargetModPath = FilesystemTools.NeutralizePath(TargetMod, PathsManagement.GetKernelPath(KernelPathType.Mods));
                                if (!(FilesystemTools.TryParsePath(TargetModPath) && FilesystemTools.FileExists(TargetModPath)))
                                {
                                    TextWriters.Write(LanguageTools.GetLocalized("NKS_MODS_MODMAN_MODNOTFOUND"), true, KernelColorType.Error);
                                    return KernelExceptionTools.GetErrorCode(KernelExceptionType.NoSuchMod);
                                }
                            }
                            else
                            {
                                TextWriters.Write(LanguageTools.GetLocalized("NKS_MODS_MODMAN_MODFILENEEDED"), true, KernelColorType.Error);
                                return KernelExceptionTools.GetErrorCode(KernelExceptionType.NoSuchMod);
                            }

                            break;
                        }
                    case "list":
                        {
                            if (parameters.ArgumentsList.Length > 1)
                                ModListTerm = parameters.ArgumentsList[1];

                            break;
                        }
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
                                    SeparatorWriterColor.WriteSeparatorColor(script, KernelColorTools.GetColor(KernelColorType.ListTitle));
                                    TextWriters.Write("- " + LanguageTools.GetLocalized("NKS_MODS_MODMAN_MODNAME") + " ", false, KernelColorType.ListEntry);
                                    TextWriters.Write(ModManager.Mods[script].ModName, true, KernelColorType.ListValue);
                                    TextWriters.Write("- " + LanguageTools.GetLocalized("NKS_MODS_MODMAN_MODFILENAME") + " ", false, KernelColorType.ListEntry);
                                    TextWriters.Write(ModManager.Mods[script].ModFileName, true, KernelColorType.ListValue);
                                    TextWriters.Write("- " + LanguageTools.GetLocalized("NKS_MODS_MODMAN_MODFILEPATH") + " ", false, KernelColorType.ListEntry);
                                    TextWriters.Write(ModManager.Mods[script].ModFilePath, true, KernelColorType.ListValue);
                                    TextWriters.Write("- " + LanguageTools.GetLocalized("NKS_MODS_MODMAN_MODVER") + " ", false, KernelColorType.ListEntry);
                                    TextWriters.Write(ModManager.Mods[script].ModVersion, true, KernelColorType.ListValue);
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
                            foreach (string Mod in ModManager.ListMods(ModListTerm).Keys)
                            {
                                SeparatorWriterColor.WriteSeparatorColor(Mod, KernelColorTools.GetColor(KernelColorType.ListTitle));
                                TextWriters.Write("- " + LanguageTools.GetLocalized("NKS_MODS_MODMAN_MODNAME") + " ", false, KernelColorType.ListEntry);
                                TextWriters.Write(ModManager.Mods[Mod].ModName, true, KernelColorType.ListValue);
                                TextWriters.Write("- " + LanguageTools.GetLocalized("NKS_MODS_MODMAN_MODFILENAME") + " ", false, KernelColorType.ListEntry);
                                TextWriters.Write(ModManager.Mods[Mod].ModFileName, true, KernelColorType.ListValue);
                                TextWriters.Write("- " + LanguageTools.GetLocalized("NKS_MODS_MODMAN_MODFILEPATH") + " ", false, KernelColorType.ListEntry);
                                TextWriters.Write(ModManager.Mods[Mod].ModFilePath, true, KernelColorType.ListValue);
                                TextWriters.Write("- " + LanguageTools.GetLocalized("NKS_MODS_MODMAN_MODVER") + " ", false, KernelColorType.ListEntry);
                                TextWriters.Write(ModManager.Mods[Mod].ModVersion, true, KernelColorType.ListValue);
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
                            TextWriters.Write(LanguageTools.GetLocalized("NKS_MODS_MODMAN_NOCOMMAND"), true, KernelColorType.Error, CommandMode);
                            HelpPrint.ShowHelp("modman");
                            return KernelExceptionTools.GetErrorCode(KernelExceptionType.ModManagement);
                        }
                }
            }
            else
            {
                TextWriters.Write(LanguageTools.GetLocalized("NKS_MODS_MODMAN_SAFEMODE"), true, KernelColorType.Error);
                return KernelExceptionTools.GetErrorCode(KernelExceptionType.ModManagement);
            }
            return 0;
        }

    }
}
