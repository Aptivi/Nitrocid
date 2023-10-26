//
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
// but WITHOUT ANY WARRANTY, without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.
//

using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using KS.ConsoleBase.Colors;
using KS.Files;
using KS.Files.Operations;
using KS.Kernel.Configuration;
using KS.Languages;
using KS.Misc.Splash;
using KS.Shell.ShellBase.Commands;
using KS.Shell.ShellBase.Shells;
using KS.Kernel.Debugging;
using KS.Kernel.Exceptions;
using KS.Modifications.ManPages;
using KS.ConsoleBase.Writers.ConsoleWriters;
using KS.Misc.Reflection;
using System.Diagnostics;
using KS.Misc.Screensaver;
using KS.Files.Operations.Querying;
using KS.Users.Permissions;
using KS.Kernel;

namespace KS.Modifications
{
    /// <summary>
    /// Mod management module
    /// </summary>
    public static class ModManager
    {

        internal static Dictionary<string, ModInfo> Mods = new();

        /// <summary>
        /// Blacklisted mods split by semicolons
        /// </summary>
        public static string BlacklistedModsString =>
            Config.MainConfig.BlacklistedModsString;

        /// <summary>
        /// Whether to start the kernel mods on boot
        /// </summary>
        public static bool StartKernelMods =>
            Config.MainConfig.StartKernelMods;

        /// <summary>
        /// Allow untrusted mods
        /// </summary>
        public static bool AllowUntrustedMods =>
            Config.MainConfig.AllowUntrustedMods;

        /// <summary>
        /// Loads all mods in KSMods
        /// </summary>
        public static void StartMods()
        {
            string ModPath = Paths.GetKernelPath(KernelPathType.Mods);
            DebugWriter.WriteDebug(DebugLevel.I, "Safe mode: {0}", KernelEntry.SafeMode);
            if (!KernelEntry.SafeMode)
            {
                // We're not in safe mode. We're good now.
                if (!Checking.FolderExists(ModPath))
                    Directory.CreateDirectory(ModPath);
                int count = Directory.EnumerateFiles(ModPath).Count();
                DebugWriter.WriteDebug(DebugLevel.I, "Files count: {0}", count);

                // Check to see if we have mods
                if (count != 0)
                {
                    SplashReport.ReportProgress(Translate.DoTranslation("Loading mods..."));
                    DebugWriter.WriteDebug(DebugLevel.I, "Mods are being loaded. Total mods = {0}", count);
                    foreach (string modFilePath in Directory.EnumerateFiles(ModPath))
                    {
                        string modFile = Path.GetFileName(modFilePath);
                        StartMod(modFile);
                    }
                }
                else
                {
                    SplashReport.ReportProgress(Translate.DoTranslation("No mods detected."));
                }
            }
            else
            {
                SplashReport.ReportProgressError(Translate.DoTranslation("Parsing mods not allowed on safe mode."));
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
            DebugWriter.WriteDebug(DebugLevel.I, "Safe mode: {0}", KernelEntry.SafeMode);
            DebugWriter.WriteDebug(DebugLevel.I, "Mod file path: {0}", PathToMod);

            if (!KernelEntry.SafeMode)
            {
                if (Checking.FileExists(PathToMod))
                {
                    DebugWriter.WriteDebug(DebugLevel.I, "Mod file exists! Starting...");
                    if (!HasModStarted(PathToMod))
                    {
                        if (!GetBlacklistedMods().Contains(PathToMod))
                        {
                            DebugWriter.WriteDebug(DebugLevel.I, "Mod {0} is not blacklisted.", ModFilename);
                            SplashReport.ReportProgress(Translate.DoTranslation("Starting mod") + " {0}...", ModFilename);
                            ModParser.ParseMod(ModFilename);
                        }
                        else
                        {
                            DebugWriter.WriteDebug(DebugLevel.W, "Trying to start blacklisted mod {0}. Ignoring...", ModFilename);
                            SplashReport.ReportProgress(Translate.DoTranslation("Mod {0} is blacklisted."), ModFilename);
                        }
                    }
                    else
                    {
                        DebugWriter.WriteDebug(DebugLevel.E, "Mod already started!");
                        SplashReport.ReportProgressError(Translate.DoTranslation("Mod has already been started!"));
                    }
                }
                else
                {
                    DebugWriter.WriteDebug(DebugLevel.E, "Mod not found!");
                    SplashReport.ReportProgress(Translate.DoTranslation("Mod {0} not found."), ModFilename);
                }
            }
            else
            {
                DebugWriter.WriteDebug(DebugLevel.E, "Mod can't be loaded in safe mode!");
                SplashReport.ReportProgressError(Translate.DoTranslation("Parsing mods not allowed on safe mode."));
            }
        }

        /// <summary>
        /// Stops all mods in KSMods
        /// </summary>
        public static void StopMods()
        {
            string ModPath = Paths.GetKernelPath(KernelPathType.Mods);
            DebugWriter.WriteDebug(DebugLevel.I, "Safe mode: {0}", KernelEntry.SafeMode);
            if (!KernelEntry.SafeMode)
            {
                // We're not in safe mode. We're good now.
                if (!Checking.FolderExists(ModPath))
                    Directory.CreateDirectory(ModPath);
                int count = Directory.EnumerateFiles(ModPath).Count();
                DebugWriter.WriteDebug(DebugLevel.I, "Files count: {0}", count);

                // Check to see if we have mods
                if (count != 0)
                {
                    SplashReport.ReportProgress(Translate.DoTranslation("Stopping mods..."));
                    DebugWriter.WriteDebug(DebugLevel.I, "Mods are being stopped. Total mods with screensavers = {0}", count);

                    // Enumerate and delete the script as soon as the stopping is complete
                    for (int ScriptIndex = Mods.Count - 1; ScriptIndex >= 0; ScriptIndex -= 1)
                    {
                        var TargetMod = Mods.Values.ElementAtOrDefault(ScriptIndex);
                        var ScriptParts = TargetMod.ModParts;

                        // Try to stop the mod and all associated parts
                        DebugWriter.WriteDebug(DebugLevel.I, "Stopping... Mod name: {0}", TargetMod.ModName);
                        for (int PartIndex = ScriptParts.Count - 1; PartIndex >= 0; PartIndex -= 1)
                        {
                            var ScriptPartInfo = ScriptParts.Values.ElementAtOrDefault(PartIndex);
                            DebugWriter.WriteDebug(DebugLevel.I, "Stopping part {0} v{1}", ScriptPartInfo.PartName, ScriptPartInfo.PartScript.Version);

                            // Stop the associated part
                            ScriptPartInfo.PartScript.StopMod();
                            if (!string.IsNullOrWhiteSpace(ScriptPartInfo.PartName) & !string.IsNullOrWhiteSpace(ScriptPartInfo.PartScript.Version))
                            {
                                SplashReport.ReportProgress(Translate.DoTranslation("{0} v{1} stopped"), ScriptPartInfo.PartName, ScriptPartInfo.PartScript.Version);
                            }

                            // Remove the part from the list
                            ScriptParts.Remove(ScriptParts.Keys.ElementAtOrDefault(PartIndex));
                        }

                        // Remove the mod from the list
                        TextWriterColor.Write(Translate.DoTranslation("Mod {0} stopped"), TargetMod.ModName);
                        Mods.Remove(Mods.Keys.ElementAtOrDefault(ScriptIndex));

                        // Remove the mod dependency from the lookup
                        string ModDepPath = ModPath + "Deps/" + Path.GetFileNameWithoutExtension(TargetMod.ModFilePath) + "-" + FileVersionInfo.GetVersionInfo(TargetMod.ModFilePath).FileVersion + "/";
                        AssemblyLookup.RemovePathFromAssemblySearchPath(ModDepPath);
                    }

                    // Clear all mod commands list, since we've stopped all mods.
                    foreach (string ShellTypeName in ShellManager.AvailableShells.Keys)
                    {
                        ListModCommands(ShellTypeName).Clear();
                        DebugWriter.WriteDebug(DebugLevel.I, "Mod commands for {0} cleared.", ShellTypeName);
                    }

                    // Clear the custom screensavers
                    ScreensaverManager.CustomSavers.Clear();
                }
                else
                {
                    DebugWriter.WriteDebug(DebugLevel.E, "Mods not found!");
                    SplashReport.ReportProgress(Translate.DoTranslation("No mods detected."));
                }
            }
            else
            {
                DebugWriter.WriteDebug(DebugLevel.E, "Mod can't be stopped in safe mode!");
                SplashReport.ReportProgressError(Translate.DoTranslation("Stopping mods not allowed on safe mode."));
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
            DebugWriter.WriteDebug(DebugLevel.I, "Safe mode: {0}", KernelEntry.SafeMode);
            DebugWriter.WriteDebug(DebugLevel.I, "Mod file path: {0}", PathToMod);

            if (KernelEntry.SafeMode)
            {
                DebugWriter.WriteDebug(DebugLevel.E, "Mod can't be stopped in safe mode!");
                SplashReport.ReportProgressError(Translate.DoTranslation("Stopping mods not allowed on safe mode."));
                return;
            }
            if (!Checking.FileExists(PathToMod))
            {
                DebugWriter.WriteDebug(DebugLevel.E, "Mod not found!");
                SplashReport.ReportProgressError(Translate.DoTranslation("Mod {0} not found."), ModFilename);
                return;
            }
            if (!HasModStarted(PathToMod))
            {
                DebugWriter.WriteDebug(DebugLevel.E, "Mod not started yet!");
                SplashReport.ReportProgressError(Translate.DoTranslation("Mod hasn't started yet!"));
                return;
            }

            // Iterate through all the mods
            SplashReport.ReportProgress(Translate.DoTranslation("Stopping mod {0}..."), ModFilename);
            DebugWriter.WriteDebug(DebugLevel.I, "Mod {0} is being stopped.", ModFilename);
            for (int ScriptIndex = Mods.Count - 1; ScriptIndex >= 0; ScriptIndex -= 1)
            {
                var TargetMod = Mods.Values.ElementAtOrDefault(ScriptIndex);
                var ScriptParts = TargetMod.ModParts;

                // Try to stop the mod and all associated parts
                DebugWriter.WriteDebug(DebugLevel.I, "Checking mod {0}...", TargetMod.ModName);
                if (TargetMod.ModFileName != ModFilename)
                    continue;

                // Iterate through all the parts
                DebugWriter.WriteDebug(DebugLevel.I, "Found mod to be stopped. Stopping...");
                for (int PartIndex = ScriptParts.Count - 1; PartIndex >= 0; PartIndex -= 1)
                {
                    var ScriptPartInfo = ScriptParts.Values.ElementAtOrDefault(PartIndex);
                    DebugWriter.WriteDebug(DebugLevel.I, "Stopping part {0} v{1}", ScriptPartInfo.PartName, ScriptPartInfo.PartScript.Version);

                    // Stop the associated part
                    ScriptPartInfo.PartScript.StopMod();
                    if (!string.IsNullOrWhiteSpace(ScriptPartInfo.PartName) & !string.IsNullOrWhiteSpace(ScriptPartInfo.PartScript.Version))
                        SplashReport.ReportProgress(Translate.DoTranslation("{0} v{1} stopped"), ScriptPartInfo.PartName, ScriptPartInfo.PartScript.Version);

                    // Remove the part from the list
                    ScriptParts.Remove(ScriptParts.Keys.ElementAtOrDefault(PartIndex));
                }

                // Remove the mod from the list
                SplashReport.ReportProgress(Translate.DoTranslation("Mod {0} stopped"), TargetMod.ModName);
                Mods.Remove(Mods.Keys.ElementAtOrDefault(ScriptIndex));

                // Remove the mod dependency from the lookup
                string ModDepPath = ModPath + "Deps/" + Path.GetFileNameWithoutExtension(ModFilename) + "-" + FileVersionInfo.GetVersionInfo(ModPath + ModFilename).FileVersion + "/";
                AssemblyLookup.RemovePathFromAssemblySearchPath(ModDepPath);
            }
        }

        /// <summary>
        /// Reloads all mods
        /// </summary>
        public static void ReloadMods()
        {
            // Stop all mods
            StopMods();
            DebugWriter.WriteDebug(DebugLevel.I, "All mods stopped.");

            // Start all mods
            StartMods();
            DebugWriter.WriteDebug(DebugLevel.I, "All mods restarted.");
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
                DebugWriter.WriteDebug(DebugLevel.I, "Checking mod {0}...", ModName);
                foreach (string PartName in Mods[ModName].ModParts.Keys)
                {
                    DebugWriter.WriteDebug(DebugLevel.I, "Checking part {0}...", PartName);
                    if (Mods[ModName].ModParts[PartName].PartFilePath == ModFilename)
                    {
                        DebugWriter.WriteDebug(DebugLevel.I, "Found part {0} ({1}). Returning True...", PartName, ModFilename);
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
            ModFilename = FilesystemTools.NeutralizePath(ModFilename, Paths.GetKernelPath(KernelPathType.Mods));
            DebugWriter.WriteDebug(DebugLevel.I, "Adding {0} to the mod blacklist...", ModFilename);
            var BlacklistedMods = GetBlacklistedMods();
            if (!BlacklistedMods.Contains(ModFilename))
            {
                DebugWriter.WriteDebug(DebugLevel.I, "Mod {0} not on the blacklist. Adding...", ModFilename);
                BlacklistedMods.Add(ModFilename);
            }
            Config.MainConfig.BlacklistedModsString = string.Join(";", BlacklistedMods);
            Config.CreateConfig();
        }

        /// <summary>
        /// Removes the mod from the blacklist (specified mod will start on the next boot)
        /// </summary>
        /// <param name="ModFilename">Mod filename found in KSMods</param>
        public static void RemoveModFromBlacklist(string ModFilename)
        {
            ModFilename = FilesystemTools.NeutralizePath(ModFilename, Paths.GetKernelPath(KernelPathType.Mods));
            DebugWriter.WriteDebug(DebugLevel.I, "Removing {0} from the mod blacklist...", ModFilename);
            var BlacklistedMods = GetBlacklistedMods();
            if (BlacklistedMods.Contains(ModFilename))
            {
                DebugWriter.WriteDebug(DebugLevel.I, "Mod {0} on the blacklist. Removing...", ModFilename);
                BlacklistedMods.Remove(ModFilename);
            }
            Config.MainConfig.BlacklistedModsString = string.Join(";", BlacklistedMods);
            Config.CreateConfig();
        }

        /// <summary>
        /// Gets the blacklisted mods list
        /// </summary>
        public static List<string> GetBlacklistedMods() => BlacklistedModsString.Split(';').ToList();

        /// <summary>
        /// Installs the mod DLL or single code file to the mod directory
        /// </summary>
        /// <param name="ModPath">Target mod path</param>
        public static void InstallMod(string ModPath)
        {
            string TargetModPath = FilesystemTools.NeutralizePath(Path.GetFileName(ModPath), Paths.GetKernelPath(KernelPathType.Mods));
            string ModName = Path.GetFileNameWithoutExtension(ModPath);
            IMod Script;
            ModPath = FilesystemTools.NeutralizePath(ModPath, true);
            DebugWriter.WriteDebug(DebugLevel.I, "Installing mod {0} to {1}...", ModPath, TargetModPath);

            // Check for upgrade
            if (Checking.FileExists(TargetModPath))
            {
                TextWriterColor.WriteKernelColor(Translate.DoTranslation("Trying to install an already-installed mod. Updating mod..."), true, KernelColorType.Warning);
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
                            throw new KernelException(KernelExceptionType.ModInstall, Translate.DoTranslation("The mod file provided is incompatible."));
                    }
                    catch (ReflectionTypeLoadException ex)
                    {
                        DebugWriter.WriteDebug(DebugLevel.E, "Error trying to load dynamic mod {0}: {1}", ModPath, ex.Message);
                        DebugWriter.WriteDebugStackTrace(ex);
                        TextWriterColor.WriteKernelColor(Translate.DoTranslation("Mod can't be loaded because of the following: "), true, KernelColorType.Error);
                        foreach (Exception LoaderException in ex.LoaderExceptions)
                        {
                            DebugWriter.WriteDebug(DebugLevel.E, "Loader exception: {0}", LoaderException.Message);
                            DebugWriter.WriteDebugStackTrace(LoaderException);
                            TextWriterColor.WriteKernelColor(LoaderException.Message, true, KernelColorType.Error);
                        }
                        TextWriterColor.WriteKernelColor(Translate.DoTranslation("Contact the vendor of the mod to upgrade the mod to the compatible version."), true, KernelColorType.Error);
                        throw;
                    }
                }

                // Then, install the file.
                File.Copy(ModPath, TargetModPath, true);

                // Check for the manual pages
                if (Checking.FolderExists(ModPath + ".manual"))
                {
                    DebugWriter.WriteDebug(DebugLevel.I, "Found manual page directory. {0}.manual exists. Installing manual pages...", ModPath);
                    Directory.CreateDirectory(TargetModPath + ".manual");
                    foreach (string ModManualFile in Directory.EnumerateFiles(ModPath + ".manual", "*.man", SearchOption.AllDirectories))
                    {
                        string ManualFileName = Path.GetFileNameWithoutExtension(ModManualFile);
                        var ManualInstance = new Manual(ModName, ModManualFile);
                        if (!ManualInstance.ValidManpage)
                            throw new KernelException(KernelExceptionType.ModInstall, Translate.DoTranslation("The manual page {0} is invalid."), ManualFileName);
                        Copying.CopyFileOrDir(ModManualFile, TargetModPath + ".manual/" + ModManualFile);
                    }
                }

                // Finally, start the mod
                TextWriterColor.Write(Translate.DoTranslation("Starting mod") + " {0}...", Path.GetFileNameWithoutExtension(TargetModPath));
                StartMod(Path.GetFileName(TargetModPath));
            }
            catch (Exception ex)
            {
                DebugWriter.WriteDebug(DebugLevel.E, "Installation failed for {0}: {1}", ModPath, ex.Message);
                DebugWriter.WriteDebugStackTrace(ex);
                TextWriterColor.WriteKernelColor(Translate.DoTranslation("Installation failed for") + " {0}: {1}", true, KernelColorType.Error, ModPath, ex.Message);
            }
        }

        /// <summary>
        /// Uninstalls the mod from the mod directory
        /// </summary>
        /// <param name="ModPath">Target mod path found in KSMods</param>
        public static void UninstallMod(string ModPath)
        {
            string TargetModPath = FilesystemTools.NeutralizePath(ModPath, Paths.GetKernelPath(KernelPathType.Mods), true);
            string ModName = Path.GetFileNameWithoutExtension(ModPath);
            DebugWriter.WriteDebug(DebugLevel.I, "Uninstalling mod {0}...", TargetModPath);
            try
            {
                // First, stop all mods related to it
                StopMod(TargetModPath);

                // Then, remove the file.
                File.Delete(TargetModPath);

                // Finally, check for the manual pages and remove them
                if (Checking.FolderExists(ModPath + ".manual"))
                {
                    DebugWriter.WriteDebug(DebugLevel.I, "Found manual page directory. {0}.manual exists. Removing manual pages...", ModPath);
                    var modManuals = PageManager.ListAllPagesByMod(ModName);
                    foreach (var manual in modManuals)
                        PageManager.RemoveManualPage(manual);
                    Directory.Delete(ModPath + ".manual", true);
                }
            }
            catch (Exception ex)
            {
                DebugWriter.WriteDebug(DebugLevel.E, "Uninstallation failed for {0}: {1}", ModPath, ex.Message);
                DebugWriter.WriteDebugStackTrace(ex);
                TextWriterColor.WriteKernelColor(Translate.DoTranslation("Uninstallation failed for") + " {0}: {1}", true, KernelColorType.Error, ModPath, ex.Message);
            }
        }

        /// <summary>
        /// Lists the mods
        /// </summary>
        public static Dictionary<string, ModInfo> ListMods() =>
            ListMods("");

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
                    DebugWriter.WriteDebug(DebugLevel.I, "Added mod name {0} to list from search term {1}", ModName, SearchTerm);
                    ListedMods.Add(ModName, Mods[ModName]);
                }
            }
            return ListedMods;
        }

        /// <summary>
        /// Lists the mods
        /// </summary>
        /// <param name="SearchTerm">Search term</param>
        public static Dictionary<string, ModInfo> ListModsStartingWith(string SearchTerm)
        {
            var ListedMods = new Dictionary<string, ModInfo>();

            // List the mods using the search term
            foreach (string ModName in Mods.Keys)
            {
                if (ModName.StartsWith(SearchTerm))
                {
                    DebugWriter.WriteDebug(DebugLevel.I, "Added mod name {0} to list (starts with {1})", ModName, SearchTerm);
                    ListedMods.Add(ModName, Mods[ModName]);
                }
            }
            return ListedMods;
        }

        /// <summary>
        /// Lists the mod commands based on the shell
        /// </summary>
        /// <param name="ShellType">Selected shell type</param>
        public static Dictionary<string, CommandInfo> ListModCommands(ShellType ShellType) =>
            ListModCommands(ShellManager.GetShellTypeName(ShellType));

        /// <summary>
        /// Lists the mod commands based on the shell
        /// </summary>
        /// <param name="ShellType">Selected shell type</param>
        public static Dictionary<string, CommandInfo> ListModCommands(string ShellType) =>
            ShellManager.GetShellInfo(ShellType).ModCommands;

        /// <summary>
        /// Gets a mod from the name
        /// </summary>
        /// <param name="modName">The mod name to search for</param>
        /// <returns>An instance of <see cref="ModInfo"/> containing information about your requested mod</returns>
        public static ModInfo GetMod(string modName)
        {
            var mods = ListMods();
            foreach (var mod in mods.Keys)
            {
                if (modName == mod)
                    return mods[mod];
            }
            return null;
        }

        /// <summary>
        /// Executes a custom mod function
        /// </summary>
        /// <param name="modName">The mod name to query</param>
        /// <param name="functionName">Function name defined in the <see cref="IMod.PubliclyAvailableFunctions"/> dictionary to query</param>
        public static object ExecuteCustomModFunction(string modName, string functionName) =>
            ExecuteCustomModFunction(modName, functionName, null);

        /// <summary>
        /// Executes a custom mod function
        /// </summary>
        /// <param name="modName">The mod name to query</param>
        /// <param name="functionName">Function name defined in the <see cref="IMod.PubliclyAvailableFunctions"/> dictionary to query</param>
        /// <param name="parameters">Parameters to execute the function with</param>
        public static object ExecuteCustomModFunction(string modName, string functionName, params object[] parameters)
        {
            // Check the user permission
            PermissionsTools.Demand(PermissionTypes.IntermodCommunication);

            // Get the mod
            var modInfo = GetMod(modName) ??
                throw new KernelException(KernelExceptionType.NoSuchMod, Translate.DoTranslation("Can't execute custom function with non-existent mod"));

            // Now, check the list of available functions
            var modParts = modInfo.ModParts;
            foreach (var modPart in modParts.Keys)
            {
                // Get a mod part
                var mod = modParts[modPart];
                DebugWriter.WriteDebug(DebugLevel.I, "Trying to get list of available functions from mod {0} part {1}...", modInfo.ModName, modPart);

                // Get a list of functions
                var functions = mod.PartScript.PubliclyAvailableFunctions;
                if (functions is null || functions.Count == 0)
                    continue;

                // Assuming that we have functions, get a single function containing that name
                if (!functions.ContainsKey(functionName))
                    continue;

                // Assuming that we have that function, get a single function delegate
                var function = functions[functionName];
                if (function is null)
                    continue;

                // The function instance is valid. Try to dynamically invoke it.
                return function.DynamicInvoke(args: parameters);
            }
            return null;
        }

    }
}
