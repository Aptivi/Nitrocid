
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
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using Extensification.StringExts;
using KS.ConsoleBase.Colors;
using KS.Files;
using KS.Files.Operations;
using KS.Files.Querying;
using KS.Kernel;
using KS.Kernel.Configuration;
using KS.Languages;
using KS.ManPages;
using KS.Misc.Screensaver.Customized;
using KS.Misc.Splash;
using KS.Misc.Writers.ConsoleWriters;
using KS.Misc.Writers.DebugWriters;
using KS.Network.RemoteDebug;
using KS.Shell.ShellBase.Commands;
using KS.Shell.ShellBase.Shells;
using KS.Shell.Shells.FTP;
using KS.Shell.Shells.Hex;
using KS.Shell.Shells.HTTP;
using KS.Shell.Shells.Json;
using KS.Shell.Shells.Mail;
using KS.Shell.Shells.Rar;
using KS.Shell.Shells.RSS;
using KS.Shell.Shells.SFTP;
using KS.Shell.Shells.Test;
using KS.Shell.Shells.Text;
using KS.Shell.Shells.Zip;
using KS.Shell.Shells.UESH;

namespace KS.Modifications
{
    public static class ModManager
    {

        public static string BlacklistedModsString = "";
        internal static Dictionary<string, ModInfo> Mods = new Dictionary<string, ModInfo>();

        /// <summary>
        /// Loads all mods in KSMods
        /// </summary>
        public static void StartMods()
        {
            string ModPath = Paths.GetKernelPath(KernelPathType.Mods);
            DebugWriter.Wdbg(DebugLevel.I, "Safe mode: {0}", Flags.SafeMode);
            if (!Flags.SafeMode)
            {
                // We're not in safe mode. We're good now.
                if (!Checking.FolderExists(ModPath))
                    Directory.CreateDirectory(ModPath);
                int count = Directory.EnumerateFiles(ModPath).Count();
                DebugWriter.Wdbg(DebugLevel.I, "Files count: {0}", count);

                // Check to see if we have mods
                if (count != 0)
                {
                    SplashReport.ReportProgress(Translate.DoTranslation("mod: Loading mods..."), 0, ColorTools.ColTypes.Neutral);
                    DebugWriter.Wdbg(DebugLevel.I, "Mods are being loaded. Total mods with screensavers = {0}", count);
                    int CurrentCount = 1;
                    foreach (string modFilePath in Directory.EnumerateFiles(ModPath))
                    {
                        string modFile = Path.GetFileName(modFilePath);
                        if (!GetBlacklistedMods().Contains(modFile))
                        {
                            DebugWriter.Wdbg(DebugLevel.I, "Mod {0} is not blacklisted.", modFile);
                            SplashReport.ReportProgress("[{1}/{2}] " + Translate.DoTranslation("Starting mod") + " {0}...", 0, ColorTools.ColTypes.Progress, modFile, CurrentCount.ToString(), count.ToString());
                            ModParser.ParseMod(modFile);
                        }
                        else
                        {
                            DebugWriter.Wdbg(DebugLevel.W, "Trying to start blacklisted mod {0}. Ignoring...", modFile);
                            SplashReport.ReportProgress("[{1}/{2}] " + Translate.DoTranslation("Mod {0} is blacklisted."), 0, ColorTools.ColTypes.Warning, modFile, CurrentCount.ToString(), count.ToString());
                        }
                        CurrentCount += 1;
                    }
                }
                else
                {
                    SplashReport.ReportProgress(Translate.DoTranslation("mod: No mods detected."), 0, ColorTools.ColTypes.Neutral);
                }
            }
            else
            {
                SplashReport.ReportProgress(Translate.DoTranslation("Parsing mods not allowed on safe mode."), 0, ColorTools.ColTypes.Error);
            }
        }

        /// <summary>
        /// Starts a specified mod
        /// </summary>
        /// <param name="ModFilename">Mod filename found in KSMods</param>
        public static void StartMod(string ModFilename)
        {
            string ModPath = Paths.GetKernelPath(KernelPathType.Mods);
            string PathToMod = Path.Combine(ModPath, ModFilename);
            DebugWriter.Wdbg(DebugLevel.I, "Safe mode: {0}", Flags.SafeMode);
            DebugWriter.Wdbg(DebugLevel.I, "Mod file path: {0}", PathToMod);

            if (!Flags.SafeMode)
            {
                if (Checking.FileExists(PathToMod))
                {
                    DebugWriter.Wdbg(DebugLevel.I, "Mod file exists! Starting...");
                    if (!HasModStarted(PathToMod))
                    {
                        if (!GetBlacklistedMods().Contains(PathToMod))
                        {
                            DebugWriter.Wdbg(DebugLevel.I, "Mod {0} is not blacklisted.", ModFilename);
                            SplashReport.ReportProgress(Translate.DoTranslation("Starting mod") + " {0}...", 0, ColorTools.ColTypes.Neutral, ModFilename);
                            ModParser.ParseMod(ModFilename);
                        }
                        else
                        {
                            DebugWriter.Wdbg(DebugLevel.W, "Trying to start blacklisted mod {0}. Ignoring...", ModFilename);
                            SplashReport.ReportProgress(Translate.DoTranslation("Mod {0} is blacklisted."), 0, ColorTools.ColTypes.Warning, ModFilename);
                        }
                    }
                    else
                    {
                        SplashReport.ReportProgress(Translate.DoTranslation("Mod has already been started!"), 0, ColorTools.ColTypes.Error);
                    }
                }
                else
                {
                    SplashReport.ReportProgress(Translate.DoTranslation("Mod {0} not found."), 0, ColorTools.ColTypes.Neutral, ModFilename);
                }
            }
            else
            {
                SplashReport.ReportProgress(Translate.DoTranslation("Parsing mods not allowed on safe mode."), 0, ColorTools.ColTypes.Error);
            }
        }

        /// <summary>
        /// Stops all mods in KSMods
        /// </summary>
        public static void StopMods()
        {
            string ModPath = Paths.GetKernelPath(KernelPathType.Mods);
            DebugWriter.Wdbg(DebugLevel.I, "Safe mode: {0}", Flags.SafeMode);
            if (!Flags.SafeMode)
            {
                // We're not in safe mode. We're good now.
                if (!Checking.FolderExists(ModPath))
                    Directory.CreateDirectory(ModPath);
                int count = Directory.EnumerateFiles(ModPath).Count();
                DebugWriter.Wdbg(DebugLevel.I, "Files count: {0}", count);

                // Check to see if we have mods
                if (count != 0)
                {
                    TextWriterColor.Write(Translate.DoTranslation("mod: Stopping mods..."), true, ColorTools.ColTypes.Neutral);
                    DebugWriter.Wdbg(DebugLevel.I, "Mods are being stopped. Total mods with screensavers = {0}", count);

                    // Enumerate and delete the script as soon as the stopping is complete
                    for (int ScriptIndex = Mods.Count - 1; ScriptIndex >= 0; ScriptIndex -= 1)
                    {
                        var TargetMod = Mods.Values.ElementAtOrDefault(ScriptIndex);
                        var ScriptParts = TargetMod.ModParts;

                        // Try to stop the mod and all associated parts
                        DebugWriter.Wdbg(DebugLevel.I, "Stopping... Mod name: {0}", TargetMod.ModName);
                        for (int PartIndex = ScriptParts.Count - 1; PartIndex >= 0; PartIndex -= 1)
                        {
                            var ScriptPartInfo = ScriptParts.Values.ElementAtOrDefault(PartIndex);
                            DebugWriter.Wdbg(DebugLevel.I, "Stopping part {0} v{1}", ScriptPartInfo.PartName, ScriptPartInfo.PartScript.Version);

                            // Stop the associated part
                            ScriptPartInfo.PartScript.StopMod();
                            if (!string.IsNullOrWhiteSpace(ScriptPartInfo.PartName) & !string.IsNullOrWhiteSpace(ScriptPartInfo.PartScript.Version))
                            {
                                TextWriterColor.Write(Translate.DoTranslation("{0} v{1} stopped"), true, ColorTools.ColTypes.Neutral, ScriptPartInfo.PartName, ScriptPartInfo.PartScript.Version);
                            }

                            // Remove the part from the list
                            ScriptParts.Remove(ScriptParts.Keys.ElementAtOrDefault(PartIndex));
                        }

                        // Remove the mod from the list
                        TextWriterColor.Write(Translate.DoTranslation("Mod {0} stopped"), true, ColorTools.ColTypes.Neutral, TargetMod.ModName);
                        Mods.Remove(Mods.Keys.ElementAtOrDefault(ScriptIndex));
                    }

                    // Clear all mod commands list, since we've stopped all mods.
                    foreach (string ShellTypeName in Enum.GetNames(typeof(ShellType)))
                    {
                        ShellType ShellTypeEnum = (ShellType)Convert.ToInt32(Enum.Parse(typeof(ShellType), ShellTypeName));
                        ListModCommands(ShellTypeEnum).Clear();
                        DebugWriter.Wdbg(DebugLevel.I, "Mod commands for {0} cleared.", ShellTypeEnum.ToString());
                    }

                    // Clear the custom screensavers
                    CustomSaverTools.CustomSavers.Clear();
                }
                else
                {
                    TextWriterColor.Write(Translate.DoTranslation("mod: No mods detected."), true, ColorTools.ColTypes.Neutral);
                }
            }
            else
            {
                TextWriterColor.Write(Translate.DoTranslation("Stopping mods not allowed on safe mode."), true, ColorTools.ColTypes.Error);
            }
        }

        /// <summary>
        /// Stops a specified mod
        /// </summary>
        /// <param name="ModFilename">Mod filename found in KSMods</param>
        public static void StopMod(string ModFilename)
        {
            string ModPath = Paths.GetKernelPath(KernelPathType.Mods);
            string PathToMod = Path.Combine(ModPath, ModFilename);
            DebugWriter.Wdbg(DebugLevel.I, "Safe mode: {0}", Flags.SafeMode);
            DebugWriter.Wdbg(DebugLevel.I, "Mod file path: {0}", PathToMod);

            if (!Flags.SafeMode)
            {
                if (Checking.FileExists(PathToMod))
                {
                    if (HasModStarted(PathToMod))
                    {
                        TextWriterColor.Write(Translate.DoTranslation("mod: Stopping mod {0}..."), true, ColorTools.ColTypes.Neutral, ModFilename);
                        DebugWriter.Wdbg(DebugLevel.I, "Mod {0} is being stopped.", ModFilename);

                        // Iterate through all the mods
                        for (int ScriptIndex = Mods.Count - 1; ScriptIndex >= 0; ScriptIndex -= 1)
                        {
                            var TargetMod = Mods.Values.ElementAtOrDefault(ScriptIndex);
                            var ScriptParts = TargetMod.ModParts;

                            // Try to stop the mod and all associated parts
                            DebugWriter.Wdbg(DebugLevel.I, "Checking mod {0}...", TargetMod.ModName);
                            if ((TargetMod.ModFileName ?? "") == (ModFilename ?? ""))
                            {
                                DebugWriter.Wdbg(DebugLevel.I, "Found mod to be stopped. Stopping...");

                                // Iterate through all the parts
                                for (int PartIndex = ScriptParts.Count - 1; PartIndex >= 0; PartIndex -= 1)
                                {
                                    var ScriptPartInfo = ScriptParts.Values.ElementAtOrDefault(PartIndex);
                                    DebugWriter.Wdbg(DebugLevel.I, "Stopping part {0} v{1}", ScriptPartInfo.PartName, ScriptPartInfo.PartScript.Version);

                                    // Remove all the commands associated with the part
                                    if (ScriptPartInfo.PartScript.Commands is not null)
                                    {
                                        foreach (CommandInfo CommandInfo in ScriptPartInfo.PartScript.Commands.Values)
                                        {
                                            DebugWriter.Wdbg(DebugLevel.I, "Removing command {0} from {1}...", CommandInfo.Command, CommandInfo.Type);
                                            ListModCommands(CommandInfo.Type).Remove(CommandInfo.Command);
                                        }
                                    }

                                    // Stop the associated part
                                    ScriptPartInfo.PartScript.StopMod();
                                    if (!string.IsNullOrWhiteSpace(ScriptPartInfo.PartName) & !string.IsNullOrWhiteSpace(ScriptPartInfo.PartScript.Version))
                                    {
                                        TextWriterColor.Write(Translate.DoTranslation("{0} v{1} stopped"), true, ColorTools.ColTypes.Neutral, ScriptPartInfo.PartName, ScriptPartInfo.PartScript.Version);
                                    }

                                    // Remove the part from the list
                                    ScriptParts.Remove(ScriptParts.Keys.ElementAtOrDefault(PartIndex));
                                }

                                // Remove the mod from the list
                                TextWriterColor.Write(Translate.DoTranslation("Mod {0} stopped"), true, ColorTools.ColTypes.Neutral, TargetMod.ModName);
                                Mods.Remove(Mods.Keys.ElementAtOrDefault(ScriptIndex));
                            }
                        }
                    }
                    else
                    {
                        TextWriterColor.Write(Translate.DoTranslation("Mod hasn't started yet!"), true, ColorTools.ColTypes.Error);
                    }
                }
                else
                {
                    TextWriterColor.Write(Translate.DoTranslation("Mod {0} not found."), true, ColorTools.ColTypes.Neutral, ModFilename);
                }
            }
            else
            {
                TextWriterColor.Write(Translate.DoTranslation("Stopping mods not allowed on safe mode."), true, ColorTools.ColTypes.Error);
            }
        }

        /// <summary>
        /// Reloads all mods
        /// </summary>
        public static void ReloadMods()
        {
            // Stop all mods
            StopMods();
            DebugWriter.Wdbg(DebugLevel.I, "All mods stopped.");

            // Start all mods
            StartMods();
            DebugWriter.Wdbg(DebugLevel.I, "All mods restarted.");
        }

        /// <summary>
        /// Reloads a specified mod
        /// </summary>
        /// <param name="ModFilename">Mod filename found in KSMods</param>
        public static void ReloadMod(string ModFilename)
        {
            StopMod(ModFilename);
            StartMod(ModFilename);
        }

        /// <summary>
        /// Checks to see if the mod has started
        /// </summary>
        /// <param name="ModFilename">Mod filename found in KSMods</param>
        public static bool HasModStarted(string ModFilename)
        {
            // Iterate through each mod and mod part
            foreach (string ModName in Mods.Keys)
            {
                DebugWriter.Wdbg(DebugLevel.I, "Checking mod {0}...", ModName);
                foreach (string PartName in Mods[ModName].ModParts.Keys)
                {
                    DebugWriter.Wdbg(DebugLevel.I, "Checking part {0}...", PartName);
                    if ((Mods[ModName].ModParts[PartName].PartFilePath ?? "") == (ModFilename ?? ""))
                    {
                        DebugWriter.Wdbg(DebugLevel.I, "Found part {0} ({1}). Returning True...", PartName, ModFilename);
                        return true;
                    }
                }
            }

            // If not found, exit with mod not started yet
            return false;
        }

        /// <summary>
        /// Adds the mod to the blacklist (specified mod will not start on the next boot)
        /// </summary>
        /// <param name="ModFilename">Mod filename found in KSMods</param>
        public static void AddModToBlacklist(string ModFilename)
        {
            ModFilename = Filesystem.NeutralizePath(ModFilename, Paths.GetKernelPath(KernelPathType.Mods));
            DebugWriter.Wdbg(DebugLevel.I, "Adding {0} to the mod blacklist...", ModFilename);
            var BlacklistedMods = GetBlacklistedMods();
            if (!BlacklistedMods.Contains(ModFilename))
            {
                DebugWriter.Wdbg(DebugLevel.I, "Mod {0} not on the blacklist. Adding...", ModFilename);
                BlacklistedMods.Add(ModFilename);
            }
            BlacklistedModsString = string.Join(";", BlacklistedMods);
            var Token = ConfigTools.GetConfigCategory(Config.ConfigCategory.Misc);
            ConfigTools.SetConfigValue(Config.ConfigCategory.Misc, Token, "Blacklisted mods", BlacklistedModsString);
        }

        /// <summary>
        /// Removes the mod from the blacklist (specified mod will start on the next boot)
        /// </summary>
        /// <param name="ModFilename">Mod filename found in KSMods</param>
        public static void RemoveModFromBlacklist(string ModFilename)
        {
            ModFilename = Filesystem.NeutralizePath(ModFilename, Paths.GetKernelPath(KernelPathType.Mods));
            DebugWriter.Wdbg(DebugLevel.I, "Removing {0} from the mod blacklist...", ModFilename);
            var BlacklistedMods = GetBlacklistedMods();
            if (BlacklistedMods.Contains(ModFilename))
            {
                DebugWriter.Wdbg(DebugLevel.I, "Mod {0} on the blacklist. Removing...", ModFilename);
                BlacklistedMods.Remove(ModFilename);
            }
            BlacklistedModsString = string.Join(";", BlacklistedMods);
            var Token = ConfigTools.GetConfigCategory(Config.ConfigCategory.Misc);
            ConfigTools.SetConfigValue(Config.ConfigCategory.Misc, Token, "Blacklisted mods", BlacklistedModsString);
        }

        /// <summary>
        /// Gets the blacklisted mods list
        /// </summary>
        public static List<string> GetBlacklistedMods()
        {
            return BlacklistedModsString.Split(';').ToList();
        }

        /// <summary>
        /// Installs the mod DLL or single code file to the mod directory
        /// </summary>
        /// <param name="ModPath">Target mod path</param>
        public static void InstallMod(string ModPath)
        {
            string TargetModPath = Filesystem.NeutralizePath(Path.GetFileName(ModPath), Paths.GetKernelPath(KernelPathType.Mods));
            IScript Script;
            ModPath = Filesystem.NeutralizePath(ModPath, true);
            DebugWriter.Wdbg(DebugLevel.I, "Installing mod {0} to {1}...", ModPath, TargetModPath);

            // Check for upgrade
            if (Checking.FileExists(TargetModPath))
            {
                TextWriterColor.Write(Translate.DoTranslation("Trying to install an already-installed mod. Updating mod..."), true, ColorTools.ColTypes.Warning);
                StopMod(Path.GetFileName(TargetModPath));
            }

            try
            {
                // First, parse the mod file
                if (Path.GetExtension(ModPath) == ".dll")
                {
                    // Mod is a dynamic DLL
                    try
                    {
                        Script = ModParser.GetModInstance(Assembly.LoadFrom(ModPath));
                        if (Script is null)
                            throw new Kernel.Exceptions.ModInstallException(Translate.DoTranslation("The mod file provided is incompatible."));
                    }
                    catch (ReflectionTypeLoadException ex)
                    {
                        DebugWriter.Wdbg(DebugLevel.E, "Error trying to load dynamic mod {0}: {1}", ModPath, ex.Message);
                        DebugWriter.WStkTrc(ex);
                        TextWriterColor.Write(Translate.DoTranslation("Mod can't be loaded because of the following: "), true, ColorTools.ColTypes.Error);
                        foreach (Exception LoaderException in ex.LoaderExceptions)
                        {
                            DebugWriter.Wdbg(DebugLevel.E, "Loader exception: {0}", LoaderException.Message);
                            DebugWriter.WStkTrc(LoaderException);
                            TextWriterColor.Write(LoaderException.Message, true, ColorTools.ColTypes.Error);
                        }
                        TextWriterColor.Write(Translate.DoTranslation("Contact the vendor of the mod to upgrade the mod to the compatible version."), true, ColorTools.ColTypes.Error);
                        throw;
                    }
                    catch (Kernel.Exceptions.ModInstallException ex)
                    {
                        throw ex;
                    }
                }

                // Then, install the file.
                File.Copy(ModPath, TargetModPath, true);

                // Check for the manual pages
                if (Checking.FolderExists(ModPath + ".manual"))
                {
                    DebugWriter.Wdbg(DebugLevel.I, "Found manual page directory. {0}.manual exists. Installing manual pages...", ModPath);
                    Directory.CreateDirectory(TargetModPath + ".manual");
                    foreach (string ModManualFile in Directory.EnumerateFiles(ModPath + ".manual", "*.man", SearchOption.AllDirectories))
                    {
                        string ManualFileName = Path.GetFileNameWithoutExtension(ModManualFile);
                        var ManualInstance = new Manual(ModManualFile);
                        if (!ManualInstance.ValidManpage)
                            throw new Kernel.Exceptions.ModInstallException(Translate.DoTranslation("The manual page {0} is invalid.").FormatString(ManualFileName));
                        Copying.CopyFileOrDir(ModManualFile, TargetModPath + ".manual/" + ModManualFile);
                    }
                }

                // Finally, start the mod
                TextWriterColor.Write(Translate.DoTranslation("Starting mod") + " {0}...", true, ColorTools.ColTypes.Neutral, Path.GetFileNameWithoutExtension(TargetModPath));
                StartMod(Path.GetFileName(TargetModPath));
            }
            catch (Exception ex)
            {
                DebugWriter.Wdbg(DebugLevel.E, "Installation failed for {0}: {1}", ModPath, ex.Message);
                DebugWriter.WStkTrc(ex);
                TextWriterColor.Write(Translate.DoTranslation("Installation failed for") + " {0}: {1}", true, ColorTools.ColTypes.Error, ModPath, ex.Message);
            }
        }

        /// <summary>
        /// Uninstalls the mod from the mod directory
        /// </summary>
        /// <param name="ModPath">Target mod path found in KSMods</param>
        public static void UninstallMod(string ModPath)
        {
            string TargetModPath = Filesystem.NeutralizePath(ModPath, Paths.GetKernelPath(KernelPathType.Mods), true);
            DebugWriter.Wdbg(DebugLevel.I, "Uninstalling mod {0}...", TargetModPath);
            try
            {
                // First, stop all mods related to it
                StopMod(TargetModPath);

                // Then, remove the file.
                File.Delete(TargetModPath);

                // Finally, check for the manual pages and remove them
                if (Checking.FolderExists(ModPath + ".manual"))
                {
                    DebugWriter.Wdbg(DebugLevel.I, "Found manual page directory. {0}.manual exists. Removing manual pages...", ModPath);
                    foreach (string ModManualFile in Directory.EnumerateFiles(ModPath + ".manual", "*.man", SearchOption.AllDirectories))
                    {
                        string ManualFileName = Path.GetFileNameWithoutExtension(ModManualFile);
                        var ManualInstance = new Manual(ModManualFile);
                        if (ManualInstance.ValidManpage)
                        {
                            PageManager.Pages.Remove(ManualInstance.Title);
                        }
                        else
                        {
                            throw new Kernel.Exceptions.ModUninstallException(Translate.DoTranslation("The manual page {0} is invalid.").FormatString(ManualFileName));
                        }
                    }
                    Directory.Delete(ModPath + ".manual", true);
                }
            }
            catch (Exception ex)
            {
                DebugWriter.Wdbg(DebugLevel.E, "Uninstallation failed for {0}: {1}", ModPath, ex.Message);
                DebugWriter.WStkTrc(ex);
                TextWriterColor.Write(Translate.DoTranslation("Uninstallation failed for") + " {0}: {1}", true, ColorTools.ColTypes.Error, ModPath, ex.Message);
            }
        }

        /// <summary>
        /// Lists the mods
        /// </summary>
        public static Dictionary<string, ModInfo> ListMods()
        {
            return ListMods("");
        }

        /// <summary>
        /// Lists the mods
        /// </summary>
        /// <param name="SearchTerm">Search term</param>
        public static Dictionary<string, ModInfo> ListMods(string SearchTerm)
        {
            var ListedMods = new Dictionary<string, ModInfo>();

            // List the mods using the search term
            foreach (string ModName in Mods.Keys)
            {
                if (ModName.Contains(SearchTerm))
                {
                    ListedMods.Add(ModName, Mods[ModName]);
                }
            }
            return ListedMods;
        }

        /// <summary>
        /// Lists the mod commands based on the shell
        /// </summary>
        /// <param name="ShellType">Selected shell type</param>
        public static Dictionary<string, CommandInfo> ListModCommands(ShellType ShellType)
        {
            switch (ShellType)
            {
                case ShellType.Shell:
                    {
                        return UESHShellCommon.ModCommands;
                    }
                case ShellType.RemoteDebugShell:
                    {
                        return RemoteDebugCmd.DebugModCmds;
                    }
                case ShellType.FTPShell:
                    {
                        return FTPShellCommon.FTPModCommands;
                    }
                case ShellType.SFTPShell:
                    {
                        return SFTPShellCommon.SFTPModCommands;
                    }
                case ShellType.MailShell:
                    {
                        return MailShellCommon.MailModCommands;
                    }
                case ShellType.TextShell:
                    {
                        return TextEditShellCommon.TextEdit_ModCommands;
                    }
                case ShellType.TestShell:
                    {
                        return TestShellCommon.Test_ModCommands;
                    }
                case ShellType.ZIPShell:
                    {
                        return ZipShellCommon.ZipShell_ModCommands;
                    }
                case ShellType.RSSShell:
                    {
                        return RSSShellCommon.RSSModCommands;
                    }
                case ShellType.JsonShell:
                    {
                        return JsonShellCommon.JsonShell_ModCommands;
                    }
                case ShellType.HTTPShell:
                    {
                        return HTTPShellCommon.HTTPModCommands;
                    }
                case ShellType.HexShell:
                    {
                        return HexEditShellCommon.HexEdit_ModCommands;
                    }
                case ShellType.RARShell:
                    {
                        return RarShellCommon.RarShell_ModCommands;
                    }

                default:
                    {
                        return UESHShellCommon.ModCommands;
                    }
            }
        }

    }
}
