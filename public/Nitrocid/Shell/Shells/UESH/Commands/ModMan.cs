//
// Nitrocid KS  Copyright (C) 2018-2026  Aptivi
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
using Terminaux.Writer.FancyWriters;
using Nitrocid.Kernel.Exceptions;
using Nitrocid.Modifications;
using Nitrocid.Security.Permissions;
using Nitrocid.Files.Paths;
using Nitrocid.ConsoleBase.Colors;
using Nitrocid.Files.Operations.Querying;
using Terminaux.Writer.ConsoleWriters;

namespace Nitrocid.Shell.Shells.UESH.Commands
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
                                if (!(Parsing.TryParsePath(TargetModPath) && Checking.FileExists(TargetModPath)))
                                {
                                    TextWriters.Write(Translate.DoTranslation("Mod not found or file has invalid characters."), true, KernelColorType.Error);
                                    return KernelExceptionTools.GetErrorCode(KernelExceptionType.NoSuchMod);
                                }
                            }
                            else
                            {
                                TextWriters.Write(Translate.DoTranslation("Mod file is not specified."), true, KernelColorType.Error);
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
                                    TextWriters.Write("- " + Translate.DoTranslation("Mod name:") + " ", false, KernelColorType.ListEntry);
                                    TextWriters.Write(ModManager.Mods[script].ModName, true, KernelColorType.ListValue);
                                    TextWriters.Write("- " + Translate.DoTranslation("Mod file name:") + " ", false, KernelColorType.ListEntry);
                                    TextWriters.Write(ModManager.Mods[script].ModFileName, true, KernelColorType.ListValue);
                                    TextWriters.Write("- " + Translate.DoTranslation("Mod file path:") + " ", false, KernelColorType.ListEntry);
                                    TextWriters.Write(ModManager.Mods[script].ModFilePath, true, KernelColorType.ListValue);
                                    TextWriters.Write("- " + Translate.DoTranslation("Mod version:") + " ", false, KernelColorType.ListEntry);
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
                                SeparatorWriterColor.WriteSeparator(Mod, true);
                                TextWriters.Write("- " + Translate.DoTranslation("Mod name:") + " ", false, KernelColorType.ListEntry);
                                TextWriters.Write(ModManager.Mods[Mod].ModName, true, KernelColorType.ListValue);
                                TextWriters.Write("- " + Translate.DoTranslation("Mod file name:") + " ", false, KernelColorType.ListEntry);
                                TextWriters.Write(ModManager.Mods[Mod].ModFileName, true, KernelColorType.ListValue);
                                TextWriters.Write("- " + Translate.DoTranslation("Mod file path:") + " ", false, KernelColorType.ListEntry);
                                TextWriters.Write(ModManager.Mods[Mod].ModFilePath, true, KernelColorType.ListValue);
                                TextWriters.Write("- " + Translate.DoTranslation("Mod version:") + " ", false, KernelColorType.ListEntry);
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

                    default:
                        {
                            TextWriters.Write(Translate.DoTranslation("Invalid command {0}. Check the usage below:"), true, KernelColorType.Error, CommandMode);
                            HelpPrint.ShowHelp("modman");
                            return KernelExceptionTools.GetErrorCode(KernelExceptionType.ModManagement);
                        }
                }
            }
            else
            {
                TextWriters.Write(Translate.DoTranslation("Mod management is disabled in safe mode."), true, KernelColorType.Error);
                return KernelExceptionTools.GetErrorCode(KernelExceptionType.ModManagement);
            }
            return 0;
        }

    }
}
