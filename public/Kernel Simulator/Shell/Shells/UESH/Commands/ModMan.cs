
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

using System.IO;
using KS.ConsoleBase.Colors;
using KS.Files;
using KS.Files.Querying;
using KS.Kernel;
using KS.Languages;
using KS.Misc.Writers.ConsoleWriters;
using KS.Misc.Writers.FancyWriters;
using KS.Modifications;
using KS.Shell.ShellBase.Commands;

namespace KS.Shell.Shells.UESH.Commands
{
    /// <summary>
    /// Manages your mods
    /// </summary>
    /// <remarks>
    /// You can manage all your mods installed in Kernel Simulator by this command. It allows you to stop, start, reload, get info, and list all your mods.
    /// <br></br>
    /// The user must have at least the administrative privileges before they can run the below commands.
    /// </remarks>
    class ModManCommand : BaseCommand, ICommand
    {

        public override void Execute(string StringArgs, string[] ListArgsOnly, string[] ListSwitchesOnly)
        {
            if (!Flags.SafeMode)
            {
                string CommandMode = ListArgsOnly[0].ToLower();
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
                            if (ListArgsOnly.Length > 1)
                            {
                                TargetMod = ListArgsOnly[1];
                                TargetModPath = Filesystem.NeutralizePath(TargetMod, Paths.GetKernelPath(KernelPathType.Mods));
                                if (!(Parsing.TryParsePath(TargetModPath) && Checking.FileExists(TargetModPath)))
                                {
                                    TextWriterColor.Write(Translate.DoTranslation("Mod not found or file has invalid characters."), true, KernelColorType.Error);
                                    return;
                                }
                            }
                            else
                            {
                                TextWriterColor.Write(Translate.DoTranslation("Mod file is not specified."), true, KernelColorType.Error);
                                return;
                            }

                            break;
                        }
                    case "list":
                    case "listparts":
                        {
                            if (ListArgsOnly.Length > 1)
                            {
                                ModListTerm = ListArgsOnly[1];
                            }

                            break;
                        }
                }

                // Now, the actual logic
                switch (CommandMode)
                {
                    case "start":
                        {
                            TextWriterColor.Write(Translate.DoTranslation("Starting mod") + " {0}...", Path.GetFileNameWithoutExtension(TargetMod));
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
                                    SeparatorWriterColor.WriteSeparator(script, true);
                                    TextWriterColor.Write("- " + Translate.DoTranslation("Mod name:") + " ", false, KernelColorType.ListEntry);
                                    TextWriterColor.Write(ModManager.Mods[script].ModName, true, KernelColorType.ListValue);
                                    TextWriterColor.Write("- " + Translate.DoTranslation("Mod file name:") + " ", false, KernelColorType.ListEntry);
                                    TextWriterColor.Write(ModManager.Mods[script].ModFileName, true, KernelColorType.ListValue);
                                    TextWriterColor.Write("- " + Translate.DoTranslation("Mod file path:") + " ", false, KernelColorType.ListEntry);
                                    TextWriterColor.Write(ModManager.Mods[script].ModFilePath, true, KernelColorType.ListValue);
                                    TextWriterColor.Write("- " + Translate.DoTranslation("Mod version:") + " ", false, KernelColorType.ListEntry);
                                    TextWriterColor.Write(ModManager.Mods[script].ModVersion, true, KernelColorType.ListValue);
                                    TextWriterColor.Write("- " + Translate.DoTranslation("Mod parts:") + " ", false, KernelColorType.ListEntry);
                                    TextWriterColor.Write(ModManager.Mods[script].ModParts.Count.ToString(), true, KernelColorType.ListValue);
                                    foreach (string ModPart in ModManager.Mods[script].ModParts.Keys)
                                    {
                                        SeparatorWriterColor.WriteSeparator("-- {0}", false, ModPart);
                                        TextWriterColor.Write("- " + Translate.DoTranslation("Part version:") + " ", false, KernelColorType.ListEntry);
                                        TextWriterColor.Write(ModManager.Mods[script].ModParts[ModPart].PartScript.Version, true, KernelColorType.ListValue);
                                        TextWriterColor.Write("- " + Translate.DoTranslation("Part file name:") + " ", false, KernelColorType.ListEntry);
                                        TextWriterColor.Write(ModManager.Mods[script].ModParts[ModPart].PartFileName, true, KernelColorType.ListValue);
                                        TextWriterColor.Write("- " + Translate.DoTranslation("Part file path:") + " ", false, KernelColorType.ListEntry);
                                        TextWriterColor.Write(ModManager.Mods[script].ModParts[ModPart].PartFilePath, true, KernelColorType.ListValue);
                                        if (ModManager.Mods[script].ModParts[ModPart].PartScript.Commands is not null)
                                        {
                                            foreach (string ModCommand in ModManager.Mods[script].ModParts[ModPart].PartScript.Commands.Keys)
                                            {
                                                SeparatorWriterColor.WriteSeparator("--- {0}", false, ModCommand);
                                                TextWriterColor.Write("- " + Translate.DoTranslation("Command name:") + " ", false, KernelColorType.ListEntry);
                                                TextWriterColor.Write(ModCommand, true, KernelColorType.ListValue);
                                                TextWriterColor.Write("- " + Translate.DoTranslation("Command definition:") + " ", false, KernelColorType.ListEntry);
                                                TextWriterColor.Write(ModManager.Mods[script].ModParts[ModPart].PartScript.Commands[ModCommand].HelpDefinition, true, KernelColorType.ListValue);
                                                TextWriterColor.Write("- " + Translate.DoTranslation("Command type:") + " ", false, KernelColorType.ListEntry);
                                                TextWriterColor.Write(ModManager.Mods[script].ModParts[ModPart].PartScript.Commands[ModCommand].Type.ToString(), true, KernelColorType.ListValue);
                                                TextWriterColor.Write("- " + Translate.DoTranslation("Strict command?") + " ", false, KernelColorType.ListEntry);
                                                TextWriterColor.Write(ModManager.Mods[script].ModParts[ModPart].PartScript.Commands[ModCommand].Flags.HasFlag(CommandFlags.Strict).ToString(), true, KernelColorType.ListValue);
                                                TextWriterColor.Write("- " + Translate.DoTranslation("Setting shell variable?") + " ", false, KernelColorType.ListEntry);
                                                TextWriterColor.Write(ModManager.Mods[script].ModParts[ModPart].PartScript.Commands[ModCommand].Flags.HasFlag(CommandFlags.SettingVariable).ToString(), true, KernelColorType.ListValue);
                                                TextWriterColor.Write("- " + Translate.DoTranslation("Can not run in maintenance mode?") + " ", false, KernelColorType.ListEntry);
                                                TextWriterColor.Write(ModManager.Mods[script].ModParts[ModPart].PartScript.Commands[ModCommand].Flags.HasFlag(CommandFlags.NoMaintenance).ToString(), true, KernelColorType.ListValue);
                                                TextWriterColor.Write("- " + Translate.DoTranslation("Obsolete?") + " ", false, KernelColorType.ListEntry);
                                                TextWriterColor.Write(ModManager.Mods[script].ModParts[ModPart].PartScript.Commands[ModCommand].Flags.HasFlag(CommandFlags.Obsolete).ToString(), true, KernelColorType.ListValue);
                                                TextWriterColor.Write("- " + Translate.DoTranslation("Redirection supported?") + " ", false, KernelColorType.ListEntry);
                                                TextWriterColor.Write(ModManager.Mods[script].ModParts[ModPart].PartScript.Commands[ModCommand].Flags.HasFlag(CommandFlags.RedirectionSupported).ToString(), true, KernelColorType.ListValue);
                                                if (ModManager.Mods[script].ModParts[ModPart].PartScript.Commands[ModCommand].CommandArgumentInfo is not null)
                                                {
                                                    foreach (string Usage in ModManager.Mods[script].ModParts[ModPart].PartScript.Commands[ModCommand].CommandArgumentInfo.HelpUsages)
                                                    {
                                                        TextWriterColor.Write("- " + Translate.DoTranslation("Command usage:") + " ", false, KernelColorType.ListEntry);
                                                        TextWriterColor.Write(Usage, true, KernelColorType.ListValue);
                                                    }
                                                    TextWriterColor.Write("- " + Translate.DoTranslation("Arguments required?") + " ", false, KernelColorType.ListEntry);
                                                    TextWriterColor.Write(ModManager.Mods[script].ModParts[ModPart].PartScript.Commands[ModCommand].CommandArgumentInfo.ArgumentsRequired.ToString(), true, KernelColorType.ListValue);
                                                    TextWriterColor.Write("- " + Translate.DoTranslation("Minimum count of required arguments:") + " ", false, KernelColorType.ListEntry);
                                                    TextWriterColor.Write(ModManager.Mods[script].ModParts[ModPart].PartScript.Commands[ModCommand].CommandArgumentInfo.MinimumArguments.ToString(), true, KernelColorType.ListValue);
                                                }
                                            }
                                        }
                                    }
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
                                SeparatorWriterColor.WriteSeparator(Mod, true);
                                TextWriterColor.Write("- " + Translate.DoTranslation("Mod name:") + " ", false, KernelColorType.ListEntry);
                                TextWriterColor.Write(ModManager.Mods[Mod].ModName, true, KernelColorType.ListValue);
                                TextWriterColor.Write("- " + Translate.DoTranslation("Mod file name:") + " ", false, KernelColorType.ListEntry);
                                TextWriterColor.Write(ModManager.Mods[Mod].ModFileName, true, KernelColorType.ListValue);
                                TextWriterColor.Write("- " + Translate.DoTranslation("Mod file path:") + " ", false, KernelColorType.ListEntry);
                                TextWriterColor.Write(ModManager.Mods[Mod].ModFilePath, true, KernelColorType.ListValue);
                                TextWriterColor.Write("- " + Translate.DoTranslation("Mod version:") + " ", false, KernelColorType.ListEntry);
                                TextWriterColor.Write(ModManager.Mods[Mod].ModVersion, true, KernelColorType.ListValue);
                                TextWriterColor.Write("- " + Translate.DoTranslation("Mod parts:") + " ", false, KernelColorType.ListEntry);
                                TextWriterColor.Write(ModManager.Mods[Mod].ModParts.Count.ToString(), true, KernelColorType.ListValue);
                            }

                            break;
                        }
                    case "listparts":
                        {
                            var ModList = ModManager.ListMods(ModListTerm);
                            foreach (string Mod in ModList.Keys)
                            {
                                foreach (string Part in ModList[Mod].ModParts.Keys)
                                {
                                    SeparatorWriterColor.WriteSeparator($"{Mod} > {Part}", true);
                                    TextWriterColor.Write("- " + Translate.DoTranslation("Mod part name:") + " ", false, KernelColorType.ListEntry);
                                    TextWriterColor.Write(ModList[Mod].ModParts[Part].PartName, true, KernelColorType.ListValue);
                                    TextWriterColor.Write("- " + Translate.DoTranslation("Mod part file name:") + " ", false, KernelColorType.ListEntry);
                                    TextWriterColor.Write(ModManager.Mods[Mod].ModParts[Part].PartFileName, true, KernelColorType.ListValue);
                                    TextWriterColor.Write("- " + Translate.DoTranslation("Mod part file path:") + " ", false, KernelColorType.ListEntry);
                                    TextWriterColor.Write(ModManager.Mods[Mod].ModParts[Part].PartFilePath, true, KernelColorType.ListValue);
                                }
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

                    default:
                        {
                            TextWriterColor.Write(Translate.DoTranslation("Invalid command {0}. Check the usage below:"), true, KernelColorType.Error, CommandMode);
                            HelpSystem.ShowHelp("modman");
                            break;
                        }
                }
            }
            else
            {
                TextWriterColor.Write(Translate.DoTranslation("Mod management is disabled in safe mode."), true, KernelColorType.Error);
            }
        }

    }
}
